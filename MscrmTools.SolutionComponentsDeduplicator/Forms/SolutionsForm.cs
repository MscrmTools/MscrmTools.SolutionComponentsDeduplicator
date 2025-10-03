using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace MscrmTools.SolutionComponentsDeduplicator.Forms
{
    public partial class SolutionsForm : DockContent
    {
        private List<Entity> solutions = new List<Entity>();

        public SolutionsForm()
        {
            InitializeComponent();
        }

        public List<Entity> CheckedSolutions => lvSolutions.CheckedItems.Cast<ListViewItem>().Select(i => (Entity)i.Tag).ToList();

        public IOrganizationService Service { get; set; }

        public void DisplaySolutions()
        {
            lvSolutions.SuspendLayout();
            lvSolutions.Items.Clear();
            lvSolutions.Items.AddRange(solutions.Where(
                e => e.GetAttributeValue<string>("friendlyname").IndexOf(txtSearch.Text, System.StringComparison.InvariantCultureIgnoreCase) >= 0
                || e.GetAttributeValue<string>("uniquename").IndexOf(txtSearch.Text, System.StringComparison.InvariantCultureIgnoreCase) >= 0
                )
                .Select(e => new ListViewItem
                {
                    Text = e.GetAttributeValue<string>("friendlyname"),
                    Tag = e
                }).ToArray());
            lvSolutions.ResumeLayout();

            lvSolutions.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        public void LoadSolutions()
        {
            solutions = Service.RetrieveMultiple(new QueryExpression
            {
                EntityName = "solution",
                ColumnSet = new ColumnSet("friendlyname", "uniquename", "publisherid"),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression("ismanaged", ConditionOperator.Equal, false),
                        new ConditionExpression("isvisible", ConditionOperator.Equal, true),
                        new ConditionExpression("uniquename",ConditionOperator.NotEqual, "Default")
                    }
                },
                Orders =
                {
                    new OrderExpression("friendlyname", OrderType.Ascending)
                }
            }).Entities.ToList();
        }

        private void txtSearch_TextChanged(object sender, System.EventArgs e)
        {
            DisplaySolutions();
        }
    }
}