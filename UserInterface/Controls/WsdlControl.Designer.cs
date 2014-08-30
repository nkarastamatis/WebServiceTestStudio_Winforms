namespace WebServiceTestStudio.UserInterface
{
    partial class WsdlControl
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.MethodFilterComboBox = new WebServiceTestStudio.UserInterface.TestStudioComboBox();
            this.MethodsListBox = new WebServiceTestStudio.UserInterface.TestStudioListBox();
            this.LoadButton = new WebServiceTestStudio.UserInterface.TestStudioButton();
            this.BrowseButton = new WebServiceTestStudio.UserInterface.TestStudioButton();
            this.WsdlPathComboBox = new WebServiceTestStudio.UserInterface.TestStudioComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Wsdl Path";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Methods";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Filter";
            // 
            // MethodFilterComboBox
            // 
            this.MethodFilterComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MethodFilterComboBox.Content = null;
            this.MethodFilterComboBox.FormattingEnabled = true;
            this.MethodFilterComboBox.Label = "";
            this.MethodFilterComboBox.Location = new System.Drawing.Point(18, 74);
            this.MethodFilterComboBox.Name = "MethodFilterComboBox";
            this.MethodFilterComboBox.SelectedContentItem = null;
            this.MethodFilterComboBox.Size = new System.Drawing.Size(296, 21);
            this.MethodFilterComboBox.TabIndex = 6;
            // 
            // MethodsListBox
            // 
            this.MethodsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MethodsListBox.Content = null;
            this.MethodsListBox.DisplayMember = "Name";
            this.MethodsListBox.FormattingEnabled = true;
            this.MethodsListBox.Label = "";
            this.MethodsListBox.Location = new System.Drawing.Point(18, 118);
            this.MethodsListBox.Name = "MethodsListBox";
            this.MethodsListBox.SelectedContentItem = null;
            this.MethodsListBox.Size = new System.Drawing.Size(380, 550);
            this.MethodsListBox.TabIndex = 4;
            // 
            // LoadButton
            // 
            this.LoadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LoadButton.Content = "Load";
            this.LoadButton.Label = "Load";
            this.LoadButton.Location = new System.Drawing.Point(323, 74);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.SelectedContentItem = null;
            this.LoadButton.Size = new System.Drawing.Size(75, 23);
            this.LoadButton.TabIndex = 3;
            this.LoadButton.Text = "Load";
            this.LoadButton.UseVisualStyleBackColor = true;
            // 
            // BrowseButton
            // 
            this.BrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowseButton.Content = "Browse";
            this.BrowseButton.Label = "Browse";
            this.BrowseButton.Location = new System.Drawing.Point(323, 29);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.SelectedContentItem = null;
            this.BrowseButton.Size = new System.Drawing.Size(75, 23);
            this.BrowseButton.TabIndex = 1;
            this.BrowseButton.Text = "Browse";
            this.BrowseButton.UseVisualStyleBackColor = true;
            // 
            // WsdlPathComboBox
            // 
            this.WsdlPathComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WsdlPathComboBox.Content = null;
            this.WsdlPathComboBox.FormattingEnabled = true;
            this.WsdlPathComboBox.Label = "";
            this.WsdlPathComboBox.Location = new System.Drawing.Point(18, 31);
            this.WsdlPathComboBox.Name = "WsdlPathComboBox";
            this.WsdlPathComboBox.SelectedContentItem = null;
            this.WsdlPathComboBox.Size = new System.Drawing.Size(299, 21);
            this.WsdlPathComboBox.TabIndex = 0;
            // 
            // WsdlControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.MethodFilterComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.MethodsListBox);
            this.Controls.Add(this.LoadButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BrowseButton);
            this.Controls.Add(this.WsdlPathComboBox);
            this.Name = "WsdlControl";
            this.Size = new System.Drawing.Size(418, 681);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public TestStudioComboBox WsdlPathComboBox;
        public TestStudioButton BrowseButton;
        private System.Windows.Forms.Label label1;
        public TestStudioButton LoadButton;
        public TestStudioListBox MethodsListBox;
        private System.Windows.Forms.Label label2;
        public TestStudioComboBox MethodFilterComboBox;
        private System.Windows.Forms.Label label3;
    }
}
