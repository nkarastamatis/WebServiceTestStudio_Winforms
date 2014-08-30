namespace WebServiceTestStudio.UserInterface
{
    partial class RequestControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.requestDataPropertyGrid = new WebServiceTestStudio.UserInterface.TestStudioPropertyGrid();
            this.resultCompositeControl = new WebServiceTestStudio.UserInterface.TestStudioCompositeControl(new TestStudioDockPanel());
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.requestDataPropertyGrid);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.resultCompositeControl);
            this.splitContainer.Size = new System.Drawing.Size(999, 527);
            this.splitContainer.SplitterDistance = 387;
            this.splitContainer.TabIndex = 0;
            // 
            // requestDataPropertyGrid
            // 
            this.requestDataPropertyGrid.Content = null;
            this.requestDataPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.requestDataPropertyGrid.Label = "PropertyGrid";
            this.requestDataPropertyGrid.Location = new System.Drawing.Point(0, 0);
            this.requestDataPropertyGrid.Name = "requestDataPropertyGrid";
            this.requestDataPropertyGrid.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.requestDataPropertyGrid.TabIndex = 0;
            this.requestDataPropertyGrid.ToolbarVisible = false;
            // 
            // testStudioCompositeControl1
            // 
            this.resultCompositeControl.AutoSize = true;
            this.resultCompositeControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultCompositeControl.Label = "";
            this.resultCompositeControl.Location = new System.Drawing.Point(220, 138);
            this.resultCompositeControl.Name = "resultCompositeControl";
            this.resultCompositeControl.SelectedContentItem = null;
            this.resultCompositeControl.TabIndex = 0;
            // 
            // RequestControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer);
            this.Name = "RequestControl";
            this.Size = new System.Drawing.Size(999, 527);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        void SplitContainer_DoubleClick(object sender, System.EventArgs e)
        {
            if (this.splitContainer.Orientation == System.Windows.Forms.Orientation.Horizontal)
                this.splitContainer.Orientation = System.Windows.Forms.Orientation.Vertical;
            else
                this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        public TestStudioPropertyGrid requestDataPropertyGrid;
        public TestStudioCompositeControl resultCompositeControl;
    }
}
