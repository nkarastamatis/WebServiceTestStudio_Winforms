using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using WebServiceTestStudio.Core;

namespace WebServiceTestStudio.UserInterface
{
    public class TestStudioForm : Form, IEnumerable<ITestStudioControl>, ITestStudioContainer
    {
        List<ITestStudioControl> children = new List<ITestStudioControl>();
        protected ITestStudioContainer container;

        public TestStudioForm(ITestStudioContainer container)
        {
            this.container = container;
            Initialize();
        }

        #region ITestStudioContainer Members

        public void Initialize()
        {
            this.SuspendLayout();

            if (this.container != null)
            {
                // call initialize in the constructor
                this.container.Initialize();
                this.Controls.Add(this.container as Control);
            }

            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            //this.ClientSize = new System.Drawing.Size(336, 688);            
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //this.Name = "MethodListDockContent";
            this.ResumeLayout(false);
        }

        public void AddChild(ITestStudioControl child)
        {
            children.Add(child);
            if (container != null)
            {

                container.AddChild(child);
            }
            else
            {
                this.SuspendLayout();

                // If we have a form being added to another for,
                // then we need to have
                var form = child as Form;
                if (form != null)
                {
                    form.TopLevel = false;
                    form.MdiParent = this;
                    form.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                    form.Dock = DockStyle.Top;
                }
                else
                this.Controls.Add(child as Control);

                if (form != null)
                    form.Show();

                this.ResumeLayout(false);
            }
        }

        public ITestStudioControl SelectedChild
        {
            get
            {
                if (container != null)
                {
                    return container.SelectedChild;
                }
                else
                {
                    foreach (var child in children)
                        if (child is Control && ((Control)child).Focused)
                            return child;
                }

                return null;
            }
        }

        #endregion

        #region IEnumerable<TestStudioContent> Members

        public IEnumerator<ITestStudioControl> GetEnumerator()
        {
            foreach (ITestStudioControl child in children)
                yield return child;
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

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
    }
}
