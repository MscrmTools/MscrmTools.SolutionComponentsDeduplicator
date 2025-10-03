using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace MscrmTools.SolutionComponentsDeduplicator.Forms
{
    public partial class ActionForm : DockContent
    {
        public ActionForm()
        {
            InitializeComponent();
        }

        public event EventHandler OnKeepOneSolutionSelected;

        public Entity SelectedSolution => pnlSolutions.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked)?.Tag as Entity;

        public void AddSolutions(List<Entity> solutions)
        {
            foreach (var solution in solutions.OrderBy(s => s.GetAttributeValue<string>("friendlyname")))
            {
                var rb = new RadioButton
                {
                    Text = solution.GetAttributeValue<string>("friendlyname"),
                    Tag = solution,
                    Dock = DockStyle.Top
                };

                pnlSolutions.Controls.Add(rb);
            }

            pnlSolutions.Height = pnlSolutions.Controls[0].Height * solutions.Count + 10;
        }

        internal void SetNumberOfSelectedItems(List<Entity> selectedComponents)
        {
            lblNumberOfSelectedItems.Text = lblNumberOfSelectedItems.Text.Split(':')[0] + ": " + selectedComponents.Count;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (!pnlSolutions.Controls.OfType<RadioButton>().Any(r => r.Checked))
            {
                MessageBox.Show("Please select a solution first.", "No solution selected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            OnKeepOneSolutionSelected?.Invoke(this, new EventArgs());
        }
    }
}