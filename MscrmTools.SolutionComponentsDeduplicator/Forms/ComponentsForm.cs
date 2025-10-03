using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using MscrmTools.SolutionComponentsDeduplicator.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace MscrmTools.SolutionComponentsDeduplicator.Forms
{
    public partial class ComponentsForm : DockContent
    {
        private List<Entity> allComponents = new List<Entity>();
        private List<Entity> sComponents = new List<Entity>();

        public ComponentsForm()
        {
            InitializeComponent();
        }

        public event EventHandler OnItemChecked;

        public List<Entity> AllComponents => allComponents;
        public List<Tuple<int, string>> ComponentDefinitions { get; internal set; }
        public List<Entity> Components => lvComponents.Items.Cast<ListViewItem>().Select(i => (Entity)i.Tag).ToList();
        public List<Entity> SelectedComponents => lvComponents.CheckedItems.OfType<ListViewItem>().Select(i => i.Tag as Entity).ToList();
        public IOrganizationService Service { get; set; }

        public List<Entity> Solutions { get; set; }

        public void AdjustColumnSize()
        {
            lvComponents.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            lvComponents.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        public bool DisplayComponents(bool forceTypesProcessing = false)
        {
            pnlSuccess.Visible = sComponents.Count == 0;
            pnlTop.Visible = sComponents.Count > 0;
            pnlMain.Visible = sComponents.Count > 0;

            if (sComponents.Count == 0)
            {
                return false;
            }

            lvComponents.Items.Clear();

            var filteredComponents = sComponents
                .Where(c => (string.IsNullOrEmpty(txtSearch.Text)
                || !string.IsNullOrEmpty(txtSearch.Text) && c.GetAttributeValue<string>("name").IndexOf(txtSearch.Text, StringComparison.InvariantCultureIgnoreCase) >= 0)
                && (comboBox1.SelectedIndex <= 0 || c.GetAttributeValue<OptionSetValue>("componenttype").Value == ComponentDefinitions.First(d => d.Item2 == comboBox1.SelectedItem.ToString().Split('(')[0].Trim()).Item1))
             .OrderBy(e => e.GetAttributeValue<string>("name"));

            #region Combobx types

            if (comboBox1.Items.Count == 0 || forceTypesProcessing)
            {
                comboBox1.SuspendLayout();
                comboBox1.Items.Clear();
                comboBox1.Items.Add($"All types ({filteredComponents.Count()})");
                comboBox1.SelectedIndexChanged -= cbbTypeFilter_SelectedIndexChanged;
                var maxText = "";
                foreach (var type in filteredComponents.GroupBy(c => c.GetAttributeValue<OptionSetValue>("componenttype").Value))
                {
                    var text = $"{ComponentDefinitions.FirstOrDefault(c => c.Item1 == type.Key)?.Item2 ?? type.Key.ToString()} ({type.Count()})";
                    comboBox1.Items.Add(text);

                    if (text.Length > maxText.Length) maxText = text;
                }

                var maxSize = TextRenderer.MeasureText(maxText, comboBox1.Font);
                comboBox1.Width = maxSize.Width;

                comboBox1.SelectedIndex = 0;
                comboBox1.SelectedIndexChanged += cbbTypeFilter_SelectedIndexChanged;
                comboBox1.ResumeLayout();
            }

            #endregion Combobx types

            #region Listview components

            lvComponents.BeginUpdate();
            lvComponents.ItemChecked -= lvComponents_ItemChecked;

            if (lvComponents.Groups.Count == 0 || forceTypesProcessing)
            {
                lvComponents.Groups.Clear();
                var types = sComponents.GroupBy(c => c.GetAttributeValue<OptionSetValue>("componenttype").Value);
                foreach (var type in types)
                {
                    lvComponents.Groups.Add(new ListViewGroup
                    {
                        Name = type.Key.ToString(),
                        Header = ComponentDefinitions.FirstOrDefault(c => c.Item1 == type.Key)?.Item2 ?? type.Key.ToString()
                    });
                }
            }

            #region Columns

            if (lvComponents.Columns.Count == 2 || forceTypesProcessing)
            {
                for (int i = lvComponents.Columns.Count - 1; i > 1; i--)
                {
                    lvComponents.Columns.RemoveAt(i);
                }

                foreach (var solution in Solutions)
                {
                    var ch = new ColumnHeader
                    {
                        Text = solution.GetAttributeValue<string>("friendlyname").Trim(),
                        Tag = solution,
                        TextAlign = HorizontalAlignment.Center
                    };

                    lvComponents.Columns.Add(ch);
                }
            }

            #endregion Columns

            #region items

            var tmp = new List<ListViewItem>();
            foreach (var c in filteredComponents)
            {
                var item = new ListViewItem
                {
                    Text = c.GetAttributeValue<string>("name") ?? c.Id.ToString(),
                    Group = lvComponents.Groups[c.GetAttributeValue<OptionSetValue>("componenttype").Value.ToString()],
                    SubItems =
                    {
                        new ListViewItem.ListViewSubItem
                        {
                            Text = c.GetAttributeValue<string>("entityname")
                        }
                    },
                    Tag = c,
                    UseItemStyleForSubItems = false
                };

                foreach (var s in Solutions)
                {
                    var sc = allComponents.FirstOrDefault(ac => ac.GetAttributeValue<EntityReference>("solutionid").Id == s.Id && c.GetAttributeValue<Guid>("objectid") == ac.GetAttributeValue<Guid>("objectid"));

                    item.SubItems.Add(new ListViewItem.ListViewSubItem { Text = sc?.GetAttributeValue<string>("behavior") ?? (sc != null ? "X" : "") });
                }

                tmp.Add(item);
            }

            lvComponents.Items.AddRange(tmp.ToArray());

            lvComponents.ItemChecked += lvComponents_ItemChecked;
            lvComponents.EndUpdate();
            lvComponents_ItemChecked(lvComponents, new ItemCheckedEventArgs(null));

            #endregion items

            #endregion Listview components

            return true;
        }

        public void LoadComponents(List<Entity> solutions, List<EntityMetadata> emds)
        {
            var tempComponents = Service.RetrieveMultiple(new QueryExpression
            {
                EntityName = "solutioncomponent",
                ColumnSet = new ColumnSet("objectid", "componenttype", "solutioncomponentid", "solutionid", "rootcomponentbehavior", "rootsolutioncomponentid", "ismetadata"),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression("solutionid", ConditionOperator.In, solutions.Select(e => e.Id).ToArray())
                    }
                }
            }).Entities.ToList();

            allComponents = tempComponents.GroupBy(c => c.GetAttributeValue<Guid>("objectid")).Where(g => g.Count() > 1).SelectMany(c => c).ToList();
            sComponents = tempComponents.GroupBy(c => c.GetAttributeValue<Guid>("objectid")).Where(g => g.Count() > 1).Select(g => g.First()).ToList();

            var scTypes = sComponents.GroupBy(c => c.GetAttributeValue<OptionSetValue>("componenttype").Value);
            foreach (var scType in scTypes)
            {
                var type = scType.Key;
                var emd = emds.FirstOrDefault(e => e.ObjectTypeCode == type);

                if (type == 61)
                {
                    emd = emds.FirstOrDefault(e => e.LogicalName == "webresource");
                }

                if (type == 1)
                {
                    foreach (var sc in scType)
                    {
                        var objectId = sc.GetAttributeValue<Guid>("objectid");

                        foreach (var asc in allComponents.Where(ac => ac.GetAttributeValue<Guid>("objectid") == objectId))
                        {
                            var behavior = "Shell only";
                            switch (asc.GetAttributeValue<OptionSetValue>("rootcomponentbehavior")?.Value)
                            {
                                case 0: behavior = "All components"; break;
                                case 1: behavior = "Metadata only"; break;
                            }

                            asc["name"] = emds.First(e => e.MetadataId == objectId).DisplayName?.UserLocalizedLabel?.Label;
                            asc["behavior"] = behavior;
                        }
                    }
                }
                else if (type == 2)
                {
                    foreach (var sc in scType)
                    {
                        var attr = emds.SelectMany(e => e.Attributes).FirstOrDefault(e => e.MetadataId == sc.GetAttributeValue<Guid>("objectid"));
                        if (attr == null) continue;
                        var entity = emds.First(e => e.LogicalName == attr.EntityLogicalName).DisplayName?.UserLocalizedLabel?.Label;
                        var attrLabel = attr.DisplayName?.UserLocalizedLabel?.Label;
                        sc["name"] = $"{attrLabel} ({entity})";
                        sc["entityname"] = entity;
                    }
                }
                else if (type == 14)
                {
                    foreach (var sc in scType)
                    {
                        var key = emds.SelectMany(e => e.Keys).FirstOrDefault(e => e.MetadataId == sc.GetAttributeValue<Guid>("objectid"));
                        if (key == null) continue;
                        var entity = emds.First(e => e.LogicalName == key.EntityLogicalName).DisplayName?.UserLocalizedLabel?.Label;
                        var attrLabel = key.DisplayName?.UserLocalizedLabel?.Label;
                        sc["name"] = $"{attrLabel} ({entity})";
                        sc["entityname"] = entity;
                    }
                }
                else if (type == 9)
                {
                    var osms = ((RetrieveAllOptionSetsResponse)Service.Execute(new RetrieveAllOptionSetsRequest())).OptionSetMetadata.ToList();

                    foreach (var sc in scType)
                    {
                        var osm = osms.FirstOrDefault(o => o.MetadataId == sc.GetAttributeValue<Guid>("objectid"));
                        if (osm == null) continue;

                        sc["name"] = osm.DisplayName?.UserLocalizedLabel?.Label;
                    }

                    continue;
                }
                else if (type == 10)
                {
                    foreach (var sc in scType)
                    {
                        var rel = emds.SelectMany(e => e.ManyToOneRelationships).FirstOrDefault(e => e.MetadataId == sc.GetAttributeValue<Guid>("objectid"));
                        if (rel != null)
                        {
                            var emd1 = emds.First(e => e.LogicalName == rel.ReferencingEntity).DisplayName?.UserLocalizedLabel?.Label;
                            var emd2 = emds.First(e => e.LogicalName == rel.ReferencedEntity).DisplayName?.UserLocalizedLabel?.Label;

                            sc["name"] = $"{emd1} 1 - N {emd2}";
                        }
                        else
                        {
                            var mrel = emds.SelectMany(e => e.ManyToManyRelationships).FirstOrDefault(e => e.MetadataId == sc.GetAttributeValue<Guid>("objectid"));

                            var emd1 = emds.First(e => e.LogicalName == mrel.Entity1LogicalName).DisplayName?.UserLocalizedLabel?.Label;
                            var emd2 = emds.First(e => e.LogicalName == mrel.Entity2LogicalName).DisplayName?.UserLocalizedLabel?.Label;

                            sc["name"] = $"{emd1} N - N {emd2}";
                        }
                    }
                }
                else if (type == 60)
                {
                    emd = emds.First(e => e.LogicalName == "systemform");
                }
                else if (type == 20)
                {
                    emd = emds.First(e => e.LogicalName == "role");
                }
                else if (type == 22)
                {
                    emd = emds.First(e => e.LogicalName == "displaystring");
                }
                else if (type == 26)
                {
                    emd = emds.First(e => e.LogicalName == "savedquery");
                }
                else if (type == 59)
                {
                    emd = emds.First(e => e.LogicalName == "savedqueryvisualization");
                }
                else if (type == 91)
                {
                    emd = emds.First(e => e.LogicalName == "pluginassembly");
                }
                else if (type == 92)
                {
                    emd = emds.First(e => e.LogicalName == "sdkmessageprocessingstep");
                }
                else if (type == 93)
                {
                    emd = emds.First(e => e.LogicalName == "sdkmessageprocessingstepimage");
                }

                if (emd == null)
                {
                    continue;
                }
                if (emd.LogicalName == "subscription")
                {
                    emd = emds.FirstOrDefault(e => e.LogicalName == "workflow");
                }

                var query = new QueryExpression(emd.LogicalName)
                {
                    ColumnSet = new ColumnSet(emd.PrimaryNameAttribute),
                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(emd.PrimaryIdAttribute, ConditionOperator.In, scType.Select(c => c.GetAttributeValue<Guid>("objectid")).ToArray())
                        }
                    }
                };

                if (emd.LogicalName == "organizationsetting")
                {
                    query.ColumnSet.AddColumn("uniquename");
                    query.LinkEntities.Add(new LinkEntity
                    {
                        LinkFromEntityName = "organizationsetting",
                        LinkFromAttributeName = "settingdefinitionid",
                        LinkToAttributeName = "settingdefinitionid",
                        LinkToEntityName = "settingdefinition",
                        Columns = new ColumnSet("displayname"),
                        EntityAlias = "settingdefinition"
                    });
                }
                else if (emd.LogicalName == "displaystring")
                {
                    query.ColumnSet = new ColumnSet("publisheddisplaystring", "customdisplaystring", "displaystringkey");
                    query.LinkEntities.Add(new LinkEntity
                    {
                        LinkFromEntityName = "displaystring",
                        LinkFromAttributeName = "displaystringid",
                        LinkToAttributeName = "displaystringid",
                        LinkToEntityName = "displaystringmap",
                        Columns = new ColumnSet("objecttypecode"),
                        EntityAlias = "displaystringmap"
                    });
                }
                else if (emd.LogicalName == "systemform")
                {
                    query.ColumnSet.AddColumn("objecttypecode");
                }
                else if (emd.LogicalName == "savedquery")
                {
                    query.ColumnSet.AddColumn("returnedtypecode");
                }
                else if (emd.LogicalName == "savedqueryvisualization")
                {
                    query.ColumnSet.AddColumn("primaryentitytypecode");
                }

                try
                {
                    var records = Service.RetrieveMultiple(query).Entities;
                    foreach (var record in records)
                    {
                        var sc = sComponents.FirstOrDefault(c => c.GetAttributeValue<Guid>("objectid") == record.Id);
                        if (sc != null)
                        {
                            if (emd.PrimaryNameAttribute != null)
                                sc.Attributes["name"] = record.GetAttributeValue<string>(emd.PrimaryNameAttribute);

                            if (emd.LogicalName == "organizationsetting")
                            {
                                sc.Attributes["name"] = $"{record.GetAttributeValue<AliasedValue>("settingdefinition.displayname").Value.ToString()} ({record.GetAttributeValue<string>("uniquename")})";
                            }

                            if (emd.LogicalName == "systemform")
                            {
                                sc["entityname"] = emds.First(e => e.LogicalName == record.GetAttributeValue<string>("objecttypecode")).DisplayName?.UserLocalizedLabel?.Label;
                            }
                            else if (emd.LogicalName == "savedquery")
                            {
                                sc["entityname"] = emds.First(e => e.LogicalName == record.GetAttributeValue<string>("returnedtypecode")).DisplayName?.UserLocalizedLabel?.Label;
                            }
                            else if (emd.LogicalName == "savedqueryvisualization")
                            {
                                sc["entityname"] = emds.First(e => e.LogicalName == record.GetAttributeValue<string>("primaryentitytypecode")).DisplayName?.UserLocalizedLabel?.Label;
                            }
                            else if (emd.LogicalName == "displaystring")
                            {
                                sc["name"] = record.GetAttributeValue<string>("publisheddisplaystring") ?? record.GetAttributeValue<string>("displaystringkey")
                                ;
                                sc["entityname"] = emds.FirstOrDefault(e => e.LogicalName == record.GetAttributeValue<AliasedValue>("displaystringmap.objecttypecode").Value.ToString()).DisplayName?.UserLocalizedLabel?.Label;
                            }
                        }
                    }
                }
                catch (Exception error)
                {
                }
            }
        }

        public void RemoveItems(List<Entity> components)
        {
            allComponents.RemoveAll(c => components.Any(rc => rc.GetAttributeValue<Guid>("objectid") == c.GetAttributeValue<Guid>("objectid")));
            sComponents.RemoveAll(c => components.Any(rc => rc.GetAttributeValue<Guid>("objectid") == c.GetAttributeValue<Guid>("objectid")));

            foreach (var component in components)
            {
                var item = lvComponents.Items.OfType<ListViewItem>().FirstOrDefault(i => (i.Tag as Entity).GetAttributeValue<Guid>("objectid") == component.GetAttributeValue<Guid>("objectid"));
                if (item != null)
                {
                    lvComponents.Items.Remove(item);
                }
            }
        }

        internal void ApplySuggestions(List<Entity> layers)
        {
            foreach (ListViewItem item in lvComponents.CheckedItems)
            {
                var component = item.Tag as Entity;

                var componentId = component.Contains("targetMetadataId") ? component.GetAttributeValue<Guid>("targetMetadataId") : component.GetAttributeValue<Guid>("objectid");
                var targetBaseLayer = layers.FirstOrDefault(l => l.GetAttributeValue<Guid>("msdyn_componentid") == componentId);
                if (targetBaseLayer == null) continue;

                for (int i = 2; i < lvComponents.Columns.Count; i++)
                {
                    var solution = lvComponents.Columns[i].Tag as Entity;
                    var publisherName = solution.GetAttributeValue<EntityReference>("publisherid").Name;

                    item.SubItems[i].Tag = targetBaseLayer.GetAttributeValue<string>("msdyn_publishername") == publisherName;
                    item.UseItemStyleForSubItems = false;
                }
            }

            pnlBottom.Visible = true;
            lvComponents.Invalidate();
        }

        private void cbbTypeFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayComponents();
        }

        private void llCheckAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            lvComponents.ItemChecked -= lvComponents_ItemChecked;
            foreach (ListViewItem item in lvComponents.Items)
            {
                item.Checked = true;
            }
            lvComponents.ItemChecked += lvComponents_ItemChecked;
            lvComponents_ItemChecked(lvComponents, new ItemCheckedEventArgs(null));
        }

        private void llInvertSelection_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            lvComponents.ItemChecked -= lvComponents_ItemChecked;
            foreach (ListViewItem item in lvComponents.Items)
            {
                item.Checked = !item.Checked;
            }
            lvComponents.ItemChecked += lvComponents_ItemChecked;
            lvComponents_ItemChecked(lvComponents, new ItemCheckedEventArgs(null));
        }

        private void llUncheckAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            lvComponents.ItemChecked -= lvComponents_ItemChecked;
            foreach (ListViewItem item in lvComponents.Items)
            {
                item.Checked = false;
            }
            lvComponents.ItemChecked += lvComponents_ItemChecked;
            lvComponents_ItemChecked(lvComponents, new ItemCheckedEventArgs(null));
        }

        private void lvComponents_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            OnItemChecked?.Invoke(this, new EventArgs());
        }

        private void txtSearch_TextChanged(object sender, System.EventArgs e)
        {
            DisplayComponents();
        }

        #region Draw methods

        internal void ResetNotification()
        {
            pnlBottom.Visible = false;
        }

        private void lvComponents_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void lvComponents_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void lvComponents_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            if (e.ColumnIndex <= 1)
            {
                e.DrawDefault = true;
                return;
            }

            if (string.IsNullOrEmpty(e.SubItem.Text))
            {
                e.DrawDefault = true;
                return;
            }

            if (e.SubItem.Text != "X")
            {
                var textwidth = Convert.ToInt32(e.Graphics.MeasureString(e.SubItem.Text, e.SubItem.Font).Width);
                var isSuggested = (e.SubItem.Tag as bool?) == true;

                e.Graphics.FillRectangle(new SolidBrush(isSuggested ? Color.Green : e.Item.Selected ? (e.Item.ListView.Focused ? SystemColors.Highlight : SystemColors.InactiveCaption) : Color.White), e.Bounds);
                e.Graphics.DrawString(e.SubItem.Text, e.Item.Font, new SolidBrush(isSuggested ? Color.White : e.Item.Selected ? (e.Item.ListView.Focused ? SystemColors.InactiveCaptionText : SystemColors.HighlightText) : e.Item.ForeColor), new Point(e.Bounds.Left + e.Bounds.Width / 2 - textwidth / 2, e.Bounds.Top + 2));
                return;
            }

            var imgLeft = e.Bounds.Left + (e.Bounds.Width / 2) - 8;
            if ((e.SubItem.Tag as bool?) == true)
            {
                e.Graphics.DrawImage(Resources.Notification_Confirmation_16, new Point(imgLeft, e.Bounds.Top));
            }
            else
            {
                e.Graphics.DrawImage(Resources.blue, new Rectangle(new Point(imgLeft, e.Bounds.Top), new Size(16, 16)));
            }
        }

        #endregion Draw methods
    }
}