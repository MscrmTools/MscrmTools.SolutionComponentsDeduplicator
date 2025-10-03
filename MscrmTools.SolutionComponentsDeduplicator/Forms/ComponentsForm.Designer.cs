namespace MscrmTools.SolutionComponentsDeduplicator.Forms
{
    partial class ComponentsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ComponentsForm));
            this.pnlTop = new System.Windows.Forms.Panel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblType = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.llInvertSelection = new System.Windows.Forms.LinkLabel();
            this.llUncheckAll = new System.Windows.Forms.LinkLabel();
            this.llCheckAll = new System.Windows.Forms.LinkLabel();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.lblTargetInfoColor = new System.Windows.Forms.Label();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.lvComponents = new System.Windows.Forms.ListView();
            this.chObjectId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chParent = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pnlSuccess = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.pbSuccess = new System.Windows.Forms.PictureBox();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pnlTop.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.pnlSuccess.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSuccess)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.txtSearch);
            this.pnlTop.Controls.Add(this.lblType);
            this.pnlTop.Controls.Add(this.comboBox1);
            this.pnlTop.Controls.Add(this.lblSearch);
            this.pnlTop.Controls.Add(this.llInvertSelection);
            this.pnlTop.Controls.Add(this.llUncheckAll);
            this.pnlTop.Controls.Add(this.llCheckAll);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Padding = new System.Windows.Forms.Padding(10);
            this.pnlTop.Size = new System.Drawing.Size(944, 41);
            this.pnlTop.TabIndex = 0;
            // 
            // txtSearch
            // 
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearch.Location = new System.Drawing.Point(82, 10);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(279, 22);
            this.txtSearch.TabIndex = 4;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // lblType
            // 
            this.lblType.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblType.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblType.Location = new System.Drawing.Point(361, 10);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(47, 21);
            this.lblType.TabIndex = 3;
            this.lblType.Text = "Type";
            this.lblType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBox1
            // 
            this.comboBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(408, 10);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(229, 24);
            this.comboBox1.TabIndex = 2;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.cbbTypeFilter_SelectedIndexChanged);
            // 
            // lblSearch
            // 
            this.lblSearch.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblSearch.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblSearch.Location = new System.Drawing.Point(10, 10);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(72, 21);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "Search";
            this.lblSearch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // llInvertSelection
            // 
            this.llInvertSelection.Dock = System.Windows.Forms.DockStyle.Right;
            this.llInvertSelection.Location = new System.Drawing.Point(637, 10);
            this.llInvertSelection.Name = "llInvertSelection";
            this.llInvertSelection.Size = new System.Drawing.Size(119, 21);
            this.llInvertSelection.TabIndex = 7;
            this.llInvertSelection.TabStop = true;
            this.llInvertSelection.Text = "Invert selection";
            this.llInvertSelection.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.llInvertSelection.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llInvertSelection_LinkClicked);
            // 
            // llUncheckAll
            // 
            this.llUncheckAll.Dock = System.Windows.Forms.DockStyle.Right;
            this.llUncheckAll.Location = new System.Drawing.Point(756, 10);
            this.llUncheckAll.Name = "llUncheckAll";
            this.llUncheckAll.Size = new System.Drawing.Size(98, 21);
            this.llUncheckAll.TabIndex = 6;
            this.llUncheckAll.TabStop = true;
            this.llUncheckAll.Text = "Uncheck all";
            this.llUncheckAll.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.llUncheckAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llUncheckAll_LinkClicked);
            // 
            // llCheckAll
            // 
            this.llCheckAll.Dock = System.Windows.Forms.DockStyle.Right;
            this.llCheckAll.Location = new System.Drawing.Point(854, 10);
            this.llCheckAll.Name = "llCheckAll";
            this.llCheckAll.Size = new System.Drawing.Size(80, 21);
            this.llCheckAll.TabIndex = 5;
            this.llCheckAll.TabStop = true;
            this.llCheckAll.Text = "Check all";
            this.llCheckAll.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.llCheckAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llCheckAll_LinkClicked);
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.lblTargetInfoColor);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.pnlBottom.Location = new System.Drawing.Point(0, 414);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Padding = new System.Windows.Forms.Padding(10);
            this.pnlBottom.Size = new System.Drawing.Size(944, 67);
            this.pnlBottom.TabIndex = 1;
            this.pnlBottom.Visible = false;
            // 
            // lblTargetInfoColor
            // 
            this.lblTargetInfoColor.BackColor = System.Drawing.SystemColors.Info;
            this.lblTargetInfoColor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTargetInfoColor.Location = new System.Drawing.Point(10, 10);
            this.lblTargetInfoColor.Name = "lblTargetInfoColor";
            this.lblTargetInfoColor.Size = new System.Drawing.Size(924, 47);
            this.lblTargetInfoColor.TabIndex = 0;
            this.lblTargetInfoColor.Text = resources.GetString("lblTargetInfoColor.Text");
            this.lblTargetInfoColor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.lvComponents);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 41);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(944, 373);
            this.pnlMain.TabIndex = 2;
            // 
            // lvComponents
            // 
            this.lvComponents.CheckBoxes = true;
            this.lvComponents.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chObjectId,
            this.chParent,
            this.columnHeader1});
            this.lvComponents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvComponents.FullRowSelect = true;
            this.lvComponents.HideSelection = false;
            this.lvComponents.Location = new System.Drawing.Point(0, 0);
            this.lvComponents.Name = "lvComponents";
            this.lvComponents.OwnerDraw = true;
            this.lvComponents.Size = new System.Drawing.Size(944, 373);
            this.lvComponents.TabIndex = 0;
            this.lvComponents.UseCompatibleStateImageBehavior = false;
            this.lvComponents.View = System.Windows.Forms.View.Details;
            this.lvComponents.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.lvComponents_DrawColumnHeader);
            this.lvComponents.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.lvComponents_DrawItem);
            this.lvComponents.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.lvComponents_DrawSubItem);
            this.lvComponents.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lvComponents_ItemChecked);
            // 
            // chObjectId
            // 
            this.chObjectId.Text = "Name";
            // 
            // chParent
            // 
            this.chParent.Text = "Parent";
            // 
            // pnlSuccess
            // 
            this.pnlSuccess.Controls.Add(this.label1);
            this.pnlSuccess.Controls.Add(this.pbSuccess);
            this.pnlSuccess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSuccess.Location = new System.Drawing.Point(0, 0);
            this.pnlSuccess.Name = "pnlSuccess";
            this.pnlSuccess.Padding = new System.Windows.Forms.Padding(10);
            this.pnlSuccess.Size = new System.Drawing.Size(944, 481);
            this.pnlSuccess.TabIndex = 1;
            this.pnlSuccess.Visible = false;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(10, 152);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(924, 186);
            this.label1.TabIndex = 1;
            this.label1.Text = "Congratulations!\r\n\r\nNo duplicate components have been found in selected solutions" +
    "!";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbSuccess
            // 
            this.pbSuccess.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pbSuccess.Dock = System.Windows.Forms.DockStyle.Top;
            this.pbSuccess.Image = global::MscrmTools.SolutionComponentsDeduplicator.Properties.Resources.satisfaction__1_;
            this.pbSuccess.Location = new System.Drawing.Point(10, 10);
            this.pbSuccess.Name = "pbSuccess";
            this.pbSuccess.Size = new System.Drawing.Size(924, 142);
            this.pbSuccess.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbSuccess.TabIndex = 0;
            this.pbSuccess.TabStop = false;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "";
            // 
            // ComponentsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 481);
            this.CloseButton = false;
            this.CloseButtonVisible = false;
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.pnlSuccess);
            this.ForeColor = System.Drawing.Color.ForestGreen;
            this.HideOnClose = true;
            this.Name = "ComponentsForm";
            this.Text = "Solution Components";
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.pnlSuccess.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbSuccess)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.ListView lvComponents;
        private System.Windows.Forms.ColumnHeader chObjectId;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.Panel pnlSuccess;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pbSuccess;
        private System.Windows.Forms.ColumnHeader chParent;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.Label lblTargetInfoColor;
        private System.Windows.Forms.LinkLabel llInvertSelection;
        private System.Windows.Forms.LinkLabel llUncheckAll;
        private System.Windows.Forms.LinkLabel llCheckAll;
        private System.Windows.Forms.ColumnHeader columnHeader1;
    }
}