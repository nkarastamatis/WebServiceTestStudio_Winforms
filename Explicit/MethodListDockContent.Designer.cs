namespace WebServiceTestStudio.UserInterface
{
    partial class MethodListDockContent
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
            components = new System.ComponentModel.Container();

            this.methodsListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // methodsListBox
            // 
            this.methodsListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.methodsListBox.FormattingEnabled = true;
            this.methodsListBox.ItemHeight = 16;
            //this.methodsListBox.Location = new System.Drawing.Point(0, 0);
            this.methodsListBox.Name = "listBox1";
            //this.methodsListBox.Size = new System.Drawing.Size(336, 688);
            this.methodsListBox.TabIndex = 0;
            this.methodsListBox.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // MethodListDockContent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(336, 688);
            this.Controls.Add(this.methodsListBox);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MethodListDockContent";
            this.Text = "Methods";
            this.ResumeLayout(false);
        }


        #endregion

        private System.Windows.Forms.ListBox methodsListBox;

    }
}
