namespace MscrmTools.SolutionComponentsDeduplicator
{
    partial class SolutionComponentDeduplicatorControl
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SolutionComponentDeduplicatorControl));
            this.toolStripMenu = new System.Windows.Forms.ToolStrip();
            this.tsbLoad = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbAnalyzeSolutions = new System.Windows.Forms.ToolStripButton();
            this.tssbCheckTarget = new System.Windows.Forms.ToolStripSplitButton();
            this.tsmiCheckAnotherEnvironment = new System.Windows.Forms.ToolStripMenuItem();
            this.dpMain = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.toolStripMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMenu
            // 
            this.toolStripMenu.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStripMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbLoad,
            this.toolStripSeparator1,
            this.tsbAnalyzeSolutions,
            this.tssbCheckTarget});
            this.toolStripMenu.Location = new System.Drawing.Point(0, 0);
            this.toolStripMenu.Name = "toolStripMenu";
            this.toolStripMenu.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStripMenu.Size = new System.Drawing.Size(746, 39);
            this.toolStripMenu.TabIndex = 4;
            this.toolStripMenu.Text = "toolStrip1";
            // 
            // tsbLoad
            // 
            this.tsbLoad.Image = global::MscrmTools.SolutionComponentsDeduplicator.Properties.Resources.Dataverse_32x32;
            this.tsbLoad.Name = "tsbLoad";
            this.tsbLoad.Size = new System.Drawing.Size(78, 36);
            this.tsbLoad.Text = "Load";
            this.tsbLoad.ToolTipText = "Load necessary data and metadata to use this tool";
            this.tsbLoad.Click += new System.EventHandler(this.tsbLoad_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // tsbAnalyzeSolutions
            // 
            this.tsbAnalyzeSolutions.Image = global::MscrmTools.SolutionComponentsDeduplicator.Properties.Resources.Search32;
            this.tsbAnalyzeSolutions.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAnalyzeSolutions.Name = "tsbAnalyzeSolutions";
            this.tsbAnalyzeSolutions.Size = new System.Drawing.Size(162, 36);
            this.tsbAnalyzeSolutions.Text = "Analyze Solutions";
            this.tsbAnalyzeSolutions.ToolTipText = "This button will report all components that are present in at least two of the solutions you selected" +
    "";
            this.tsbAnalyzeSolutions.Click += new System.EventHandler(this.tsbAnalyzeSolutions_Click);
            // 
            // tssbCheckTarget
            // 
            this.tssbCheckTarget.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCheckAnotherEnvironment});
            this.tssbCheckTarget.Image = global::MscrmTools.SolutionComponentsDeduplicator.Properties.Resources.target;
            this.tssbCheckTarget.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tssbCheckTarget.Name = "tssbCheckTarget";
            this.tssbCheckTarget.Size = new System.Drawing.Size(230, 36);
            this.tssbCheckTarget.Text = "Check target environment";
            this.tssbCheckTarget.ToolTipText = resources.GetString("tssbCheckTarget.ToolTipText");
            this.tssbCheckTarget.Visible = false;
            this.tssbCheckTarget.ButtonClick += new System.EventHandler(this.tssbCheckTarget_ButtonClick);
            // 
            // tsmiCheckAnotherEnvironment
            // 
            this.tsmiCheckAnotherEnvironment.Name = "tsmiCheckAnotherEnvironment";
            this.tsmiCheckAnotherEnvironment.Size = new System.Drawing.Size(273, 26);
            this.tsmiCheckAnotherEnvironment.Text = "Check another environment";
            this.tsmiCheckAnotherEnvironment.Click += new System.EventHandler(this.tsmiCheckAnotherEnvironment_Click);
            // 
            // dpMain
            // 
            this.dpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dpMain.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingWindow;
            this.dpMain.Location = new System.Drawing.Point(0, 39);
            this.dpMain.Name = "dpMain";
            this.dpMain.Size = new System.Drawing.Size(746, 331);
            this.dpMain.TabIndex = 5;
            // 
            // SolutionComponentDeduplicatorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dpMain);
            this.Controls.Add(this.toolStripMenu);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SolutionComponentDeduplicatorControl";
            this.Size = new System.Drawing.Size(746, 370);
            this.Load += new System.EventHandler(this.MyPluginControl_Load);
            this.toolStripMenu.ResumeLayout(false);
            this.toolStripMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStripMenu;
        private System.Windows.Forms.ToolStripButton tsbLoad;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dpMain;
        private System.Windows.Forms.ToolStripButton tsbAnalyzeSolutions;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSplitButton tssbCheckTarget;
        private System.Windows.Forms.ToolStripMenuItem tsmiCheckAnotherEnvironment;
    }
}
