using Microsoft.Xrm.Sdk;
using System;
using System.Windows.Forms;

namespace MscrmTools.SolutionComponentsDeduplicator.UserControls
{
    public partial class SolutionControl : UserControl
    {
        private Entity solution;

        public SolutionControl(Entity solution)
        {
            InitializeComponent();

            this.solution = solution;

            label.Text = $"{solution.GetAttributeValue<string>("friendlyname")} v{solution.GetAttributeValue<string>("version")}";
        }

        public event EventHandler SolutionSelected;

        public Entity Solution => solution;

        private void btn_Click(object sender, EventArgs e)
        {
            SolutionSelected?.Invoke(this, new EventArgs());
        }
    }
}