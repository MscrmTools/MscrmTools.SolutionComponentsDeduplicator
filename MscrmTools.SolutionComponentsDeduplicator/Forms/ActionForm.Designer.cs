namespace MscrmTools.SolutionComponentsDeduplicator.Forms
{
    partial class ActionForm
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
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblProblem = new System.Windows.Forms.Label();
            this.pbProblem = new System.Windows.Forms.PictureBox();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnApply = new System.Windows.Forms.Button();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlSolutions = new System.Windows.Forms.Panel();
            this.lblSolutions = new System.Windows.Forms.Label();
            this.lblInfo = new System.Windows.Forms.Label();
            this.lblNumberOfSelectedItems = new System.Windows.Forms.Label();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbProblem)).BeginInit();
            this.pnlBottom.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.lblProblem);
            this.pnlTop.Controls.Add(this.pbProblem);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(10, 10);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(726, 201);
            this.pnlTop.TabIndex = 0;
            // 
            // lblProblem
            // 
            this.lblProblem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblProblem.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProblem.ForeColor = System.Drawing.Color.Red;
            this.lblProblem.Location = new System.Drawing.Point(0, 109);
            this.lblProblem.Name = "lblProblem";
            this.lblProblem.Size = new System.Drawing.Size(726, 92);
            this.lblProblem.TabIndex = 1;
            this.lblProblem.Text = "Duplicates found!";
            this.lblProblem.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbProblem
            // 
            this.pbProblem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pbProblem.Dock = System.Windows.Forms.DockStyle.Top;
            this.pbProblem.Image = global::MscrmTools.SolutionComponentsDeduplicator.Properties.Resources.despair__1_;
            this.pbProblem.Location = new System.Drawing.Point(0, 0);
            this.pbProblem.Name = "pbProblem";
            this.pbProblem.Size = new System.Drawing.Size(726, 109);
            this.pbProblem.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbProblem.TabIndex = 0;
            this.pbProblem.TabStop = false;
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnApply);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(10, 711);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(726, 45);
            this.pnlBottom.TabIndex = 1;
            // 
            // btnApply
            // 
            this.btnApply.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnApply.Location = new System.Drawing.Point(0, 0);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(726, 45);
            this.btnApply.TabIndex = 0;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.lblNumberOfSelectedItems);
            this.pnlMain.Controls.Add(this.pnlSolutions);
            this.pnlMain.Controls.Add(this.lblSolutions);
            this.pnlMain.Controls.Add(this.lblInfo);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(10, 211);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(726, 500);
            this.pnlMain.TabIndex = 2;
            // 
            // pnlSolutions
            // 
            this.pnlSolutions.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSolutions.Location = new System.Drawing.Point(0, 108);
            this.pnlSolutions.Name = "pnlSolutions";
            this.pnlSolutions.Size = new System.Drawing.Size(726, 119);
            this.pnlSolutions.TabIndex = 2;
            // 
            // lblSolutions
            // 
            this.lblSolutions.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSolutions.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSolutions.Location = new System.Drawing.Point(0, 63);
            this.lblSolutions.Name = "lblSolutions";
            this.lblSolutions.Size = new System.Drawing.Size(726, 45);
            this.lblSolutions.TabIndex = 1;
            this.lblSolutions.Text = "Solutions";
            this.lblSolutions.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblInfo
            // 
            this.lblInfo.BackColor = System.Drawing.SystemColors.Info;
            this.lblInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblInfo.Location = new System.Drawing.Point(0, 0);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(726, 63);
            this.lblInfo.TabIndex = 0;
            this.lblInfo.Text = "Select the solution where you want to keep the selected components. These compone" +
    "nts will be removed from the other solutions";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblNumberOfSelectedItems
            // 
            this.lblNumberOfSelectedItems.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblNumberOfSelectedItems.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumberOfSelectedItems.Location = new System.Drawing.Point(0, 227);
            this.lblNumberOfSelectedItems.Name = "lblNumberOfSelectedItems";
            this.lblNumberOfSelectedItems.Size = new System.Drawing.Size(726, 45);
            this.lblNumberOfSelectedItems.TabIndex = 3;
            this.lblNumberOfSelectedItems.Text = "Number of items to process : 0";
            this.lblNumberOfSelectedItems.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ActionForm
            // 
            this.AutoHidePortion = 200D;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(746, 766);
            this.CloseButton = false;
            this.CloseButtonVisible = false;
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlTop);
            this.Name = "ActionForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Text = "Actions";
            this.pnlTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbProblem)).EndInit();
            this.pnlBottom.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.PictureBox pbProblem;
        private System.Windows.Forms.Label lblProblem;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Panel pnlSolutions;
        private System.Windows.Forms.Label lblSolutions;
        private System.Windows.Forms.Label lblNumberOfSelectedItems;
    }
}