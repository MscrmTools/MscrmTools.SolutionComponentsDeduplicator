using McTools.Xrm.Connection;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using MscrmTools.SolutionComponentsDeduplicator.AppCode;
using MscrmTools.SolutionComponentsDeduplicator.Forms;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.ServiceModel;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace MscrmTools.SolutionComponentsDeduplicator
{
    public partial class SolutionComponentDeduplicatorControl : MultipleConnectionsPluginControlBase, IPayPalPlugin, IGitHubPlugin, IHelpPlugin
    {
        private ActionForm af;
        private ComponentsForm cf;
        private List<Tuple<int, string>> componentsDefs;
        private List<EntityMetadata> emds;
        private LogForm lf;
        private Logger log;
        private SolutionsForm sf;
        private ConnectionDetail targetDetail;

        public SolutionComponentDeduplicatorControl()
        {
            InitializeComponent();

            SetTheme();
            dpMain.DockLeftPortion = 300;
            dpMain.DockRightPortion = 300;
            sf = new SolutionsForm();
            sf.Width = 300;
            sf.AutoHidePortion = 300;
            sf.Show(dpMain, DockState.DockLeft);

            cf = new ComponentsForm();
            cf.OnItemChecked += Cf_OnItemChecked;
            cf.Show(dpMain, DockState.Document);

            log = new Logger();
            lf = new LogForm(log);
            lf.Show(dpMain, DockState.DockBottomAutoHide);
        }

        #region Interfaces

        public string DonationDescription => "Donation for Solution Components Deduplicator";

        public string EmailAccount => "tanguy92@hotmail.com";

        public string HelpUrl => "https://github.com/MscrmTools/MscrmTools.SolutionComponentsDeduplicator";
        public string RepositoryName => "MscrmTools.SolutionComponentsDeduplicator";

        public string UserName => "MscrmTools";

        #endregion Interfaces

        #region XrmToolBox tool events

        /// <summary>
        /// This event occurs when the connection has been updated in XrmToolBox
        /// </summary>
        public override void UpdateConnection(IOrganizationService newService, ConnectionDetail detail, string actionName, object parameter)
        {
            base.UpdateConnection(newService, detail, actionName, parameter);
        }

        protected override void ConnectionDetailsUpdated(NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                targetDetail = e.NewItems.Cast<ConnectionDetail>().First();
                tssbCheckTarget.Text = tssbCheckTarget.Text.Split('(')[0].Trim() + $" ({targetDetail.ConnectionName})";
                tssbCheckTarget_ButtonClick(tssbCheckTarget, new EventArgs());
            }
        }

        private void MyPluginControl_Load(object sender, EventArgs e)
        {
        }

        #endregion XrmToolBox tool events

        #region Toolbar events

        private void tsbAnalyzeSolutions_Click(object sender, EventArgs e)
        {
            var solutions = sf.CheckedSolutions;
            af?.Close();

            tssbCheckTarget.Visible = false;
            cf.ResetNotification();

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Analyzing solutions...",
                Work = (worker, args) =>
                {
                    log.Log("Analyzing solutions...");

                    cf.Service = Service;
                    cf.LoadComponents(solutions, emds);

                    log.Log("Solutions analyzed successfuly");
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    cf.Solutions = sf.CheckedSolutions;
                    cf.ComponentDefinitions = componentsDefs;
                    var hasDuplicates = cf.DisplayComponents(true);
                    if (hasDuplicates)
                    {
                        af = new ActionForm();
                        af.OnKeepOneSolutionSelected += Af_OnKeepOneSolutionSelected;
                        af.AutoHidePortion = 200;
                        af.AddSolutions(sf.CheckedSolutions);
                        af.Show(dpMain, DockState.DockRight);
                    }

                    cf.AdjustColumnSize();

                    var allSamePublisher = sf.CheckedSolutions.All(cs => cs.GetAttributeValue<EntityReference>("publisherid").Id == sf.CheckedSolutions.First().GetAttributeValue<EntityReference>("publisherid").Id);
                    tssbCheckTarget.Visible = !allSamePublisher;
                }
            });
        }

        private void tsmiCheckAnotherEnvironment_Click(object sender, EventArgs e)
        {
            RemoveAdditionalOrganization(targetDetail);
            AddAdditionalOrganization();
            return;
        }

        private void tssbCheckTarget_ButtonClick(object sender, EventArgs e)
        {
            if (AdditionalConnectionDetails == null || !AdditionalConnectionDetails.Any())
            {
                base.AddAdditionalOrganization();
                return;
            }

            var detail = AdditionalConnectionDetails.First();
            var components = cf.SelectedComponents;
            cf.ResetNotification();

            if (components.Count == 0)
            {
                MessageBox.Show("Please select components to check first.", "No component selected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Checking target env...",
                Work = (worker, args) =>
                {
                    var service = detail.GetCrmServiceClient();
                    var targetService = targetDetail.GetCrmServiceClient();
                    var layers = new List<Entity>();
                    var targetEmds = new List<EntityMetadata>();

                    foreach (var item in components)
                    {
                        worker.ReportProgress(0, "Checking component " + item.GetAttributeValue<string>("name") + "...");

                        var componentName = ((ComponentType)item.GetAttributeValue<OptionSetValue>("componenttype").Value).ToString();
                        if (componentName == "418") componentName = "msdyn_dataflow"; // Dataflow (418) is in fact a Workflow component
                        var isMetadata = item.GetAttributeValue<bool>("ismetadata");

                        var objectid = item.GetAttributeValue<Guid>("objectid");
                        if (isMetadata)
                        {
                            EntityMetadata md;
                            switch ((ComponentType)item.GetAttributeValue<OptionSetValue>("componenttype").Value)
                            {
                                case ComponentType.Entity:
                                    var logicalName = emds.FirstOrDefault(emd2 => emd2.MetadataId == objectid)?.LogicalName;
                                    md = GetEmd(targetEmds, logicalName, targetService);
                                    objectid = md?.MetadataId ?? Guid.Empty;
                                    break;

                                case ComponentType.Attribute:
                                    var attr = emds.SelectMany(emd2 => emd2.Attributes).FirstOrDefault(a => a.MetadataId == objectid);
                                    md = GetEmd(targetEmds, attr.EntityLogicalName, targetService);
                                    objectid = md.Attributes.FirstOrDefault(a => a.LogicalName == attr.LogicalName)?.MetadataId ?? Guid.Empty;
                                    break;

                                case ComponentType.EntityRelationship:
                                    var rel = emds.SelectMany(emd2 => emd2.ManyToOneRelationships).FirstOrDefault(r => r.MetadataId == objectid);
                                    md = GetEmd(targetEmds, rel.ReferencingEntity, targetService);
                                    objectid = md.OneToManyRelationships.FirstOrDefault(a => a.SchemaName == rel.SchemaName)?.MetadataId ?? Guid.Empty;
                                    break;

                                case ComponentType.Relationship:
                                    var rel2 = emds.SelectMany(emd2 => emd2.ManyToManyRelationships).FirstOrDefault(r => r.MetadataId == objectid);
                                    md = GetEmd(targetEmds, rel2.Entity1LogicalName, targetService);
                                    objectid = md.ManyToManyRelationships.FirstOrDefault(a => a.SchemaName == rel2.SchemaName)?.MetadataId ?? Guid.Empty;
                                    break;

                                case ComponentType.EntityKey:
                                    var key = emds.SelectMany(emd2 => emd2.Keys).FirstOrDefault(k => k.MetadataId == objectid);
                                    md = GetEmd(targetEmds, key.EntityLogicalName, targetService);
                                    objectid = md.Keys.FirstOrDefault(a => a.SchemaName == key.SchemaName)?.MetadataId ?? Guid.Empty;
                                    break;
                            }

                            item["targetMetadataId"] = objectid;
                        }
                        else if (componentName == ComponentType.OptionSet.ToString())
                        {
                            var allOptionSets = ((RetrieveAllOptionSetsResponse)targetService.Execute(new RetrieveAllOptionSetsRequest())).OptionSetMetadata.ToList();
                            var optionSet = allOptionSets.FirstOrDefault(o => o.Name == null);
                            if (optionSet != null)
                            {
                                objectid = optionSet.MetadataId.Value;
                            }
                        }

                        var query = new QueryExpression("msdyn_componentlayer")
                        {
                            ColumnSet = new ColumnSet("msdyn_solutionname", "msdyn_publishername", "msdyn_componentid"),
                            Criteria = new FilterExpression
                            {
                                Conditions =
                                {
                                    new ConditionExpression("msdyn_solutioncomponentname", ConditionOperator.Equal, componentName),
                                    new ConditionExpression("msdyn_componentid", ConditionOperator.Equal, objectid)
                                }
                            }
                        };

                        layers.AddRange(targetService.RetrieveMultiple(query).Entities.Where(r => r.GetAttributeValue<int>("msdyn_order") == 1).ToList());
                    }

                    args.Result = layers;
                },
                ProgressChanged = (args) =>
                {
                    SetWorkingMessage(args.UserState.ToString());
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    cf.ApplySuggestions(args.Result as List<Entity>);
                }
            });
        }

        #endregion Toolbar events

        private void Af_OnKeepOneSolutionSelected(object sender, EventArgs e)
        {
            var solutionToKeep = af.SelectedSolution;

            if (DialogResult.No == MessageBox.Show($"Are you sure you want to keep solution '{solutionToKeep.GetAttributeValue<string>("friendlyname")}' and remove selected components form the others ?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                return;
            }

            var componentsToRemove = cf.SelectedComponents;
            var allComponents = cf.AllComponents;
            var solutionsToClean = sf.CheckedSolutions.Where(s => s.Id != solutionToKeep.Id).ToList();
            var removedComponents = new List<Entity>();

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Cleaning...",
                Work = (worker, args) =>
                {
                    var total = componentsToRemove.Count;
                    var current = 0;
                    foreach (var solution in solutionsToClean)
                    {
                        worker.ReportProgress((int)((current / (double)total) * 100), $"Cleaning solution '{solution.GetAttributeValue<string>("friendlyname")}'...");

                        foreach (var component in componentsToRemove)
                        {
                            var comp = allComponents.FirstOrDefault(c => c.GetAttributeValue<Guid>("objectid") == component.GetAttributeValue<Guid>("objectid") && c.GetAttributeValue<EntityReference>("solutionid").Id == solution.Id);
                            if (comp != null)
                            {
                                try
                                {
                                    Service.Execute(new RemoveSolutionComponentRequest
                                    {
                                        ComponentId = comp.GetAttributeValue<Guid>("objectid"),
                                        ComponentType = comp.GetAttributeValue<OptionSetValue>("componenttype").Value,
                                        SolutionUniqueName = solution.GetAttributeValue<string>("uniquename")
                                    });

                                    log.Log($"Component '{component.GetAttributeValue<string>("name")}' ({component.FormattedValues["componenttype"]}) removed successfuly from solution '{solution.GetAttributeValue<string>("friendlyname")}'");

                                    removedComponents.Add(comp);
                                }
                                catch (FaultException<OrganizationServiceFault> error)
                                {
                                    // Not found in solution
                                    if (error.Detail.ErrorCode == -2147160031) continue;

                                    log.Log($"Error when removing component '{component.GetAttributeValue<string>("name")}' ({component.FormattedValues["componenttype"]}) from solution '{solution.GetAttributeValue<string>("friendlyname")}': {error.Message}");
                                }
                            }
                            current++;
                        }

                        args.Result = removedComponents;
                    }
                },
                ProgressChanged = (args) =>
                {
                    SetWorkingMessage(args.UserState.ToString());
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(this, args.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    cf.RemoveItems(args.Result as List<Entity>);

                    af.SetNumberOfSelectedItems(cf.SelectedComponents);

                    if (cf.Components.Count == 0)
                    {
                        af.Close();
                        af = null;
                    }
                }
            });
        }

        private void Cf_OnItemChecked(object sender, EventArgs e)
        {
            af?.SetNumberOfSelectedItems(cf.SelectedComponents);
        }

        private EntityMetadata GetEmd(List<EntityMetadata> emds, string entityLogicalName, IOrganizationService service)
        {
            EntityMetadata emd = emds.FirstOrDefault(te => te.LogicalName == entityLogicalName);
            if (emd == null)
            {
                log.Log($"Retrieving {entityLogicalName} metadata...");
                emd = MetadataHelper.RetrieveEntityMetadata(service, entityLogicalName);
                emds.Add(emd);
                log.Log($"{entityLogicalName} metadata retrieved successfully");
            }

            return emd;
        }

        private void LoadComponentDefinitions()
        {
            log.Log("Loading component definitions...");

            var definitions = Service.RetrieveMultiple(new QueryExpression("solutioncomponentdefinition")
            {
                NoLock = true,
                ColumnSet = new ColumnSet(true)
            }).Entities;

            var omc = ((OptionSetMetadata)((RetrieveOptionSetResponse)Service.Execute(
                        new RetrieveOptionSetRequest
                        {
                            Name = "componenttype"
                        })).OptionSetMetadata).Options;

            componentsDefs = definitions.Select(d => new Tuple<int, string>(d.GetAttributeValue<int>("solutioncomponenttype"), d.GetAttributeValue<string>("name"))).ToList();
            componentsDefs.AddRange(omc.Select(o => new Tuple<int, string>(o.Value.Value, o.Label?.UserLocalizedLabel?.Label ?? o.Value.ToString())).ToList());
            componentsDefs.Add(new Tuple<int, string>(80, "Model driven app"));

            log.Log("Components definitions loaded successfuly");
        }

        private void LoadSolutions()
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Loading solutions...",
                Work = (worker, args) =>
                {
                    sf.Service = Service;
                    sf.LoadSolutions();

                    worker.ReportProgress(0, "Loading entities metadata...");
                    emds = MetadataHelper.RetrieveEntitiesMainInfo(Service);
                    log.Log("Entities metadata loaded successfuly");

                    worker.ReportProgress(0, "Loading components definitions...");
                    LoadComponentDefinitions();
                },
                ProgressChanged = (args) =>
                {
                    SetWorkingMessage(args.UserState.ToString());
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    sf.DisplaySolutions();
                }
            });
        }

        private void SetTheme()
        {
            if (XrmToolBox.Options.Instance.Theme != null)
            {
                switch (XrmToolBox.Options.Instance.Theme)
                {
                    case "Blue theme":
                        {
                            var theme = new VS2015BlueTheme();
                            dpMain.Theme = theme;
                        }
                        break;

                    case "Light theme":
                        {
                            var theme = new VS2015LightTheme();
                            dpMain.Theme = theme;
                        }
                        break;

                    case "Dark theme":
                        {
                            var theme = new VS2015DarkTheme();
                            dpMain.Theme = theme;
                        }
                        break;
                }
            }
        }

        private void tsbLoad_Click(object sender, EventArgs e)
        {
            // The ExecuteMethod method handles connecting to an
            // organization if XrmToolBox is not yet connected
            ExecuteMethod(LoadSolutions);
        }
    }
}