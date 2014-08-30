using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebServiceTestStudio.Core;

namespace WebServiceTestStudio.UserInterface
{
    public class TestStudioCompositeControl : UserControl, ITestStudioControl, ITestStudioContainer, IEnumerable<ITestStudioControl>
    {
        List<ITestStudioControl> children = new List<ITestStudioControl>();
        protected ITestStudioContainer container;

        public TestStudioCompositeControl()
        {
            Initialize();
        }

        public TestStudioCompositeControl(ITestStudioContainer container = null)
        {
            this.container = container;

            Initialize();

            if (this.container != null)
                (this.container as Control).Parent = this;
        }

        #region ITestStudioContainer Members

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

                var form = child as Form;
                if (form != null)
                    form.TopLevel = false;
             
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

        #region ITestStudioControl Members

        public object Content
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public void Initialize()
        {
            AutoSize = true;
        }

        public object SelectedContentItem
        {
            get { return container; }
            set { container = container; }
        }

        public string Label
        {
            get { return Text; }
            set { Text = value; }
        }

        #endregion


        #region IEnumerable<TestStudioContent> Members

        public IEnumerator<ITestStudioControl> GetEnumerator()
        {
            foreach (ITestStudioControl child in children)
                if (((Control)child).IsDisposed)
                    continue;
                else
                    yield return child;
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

    }
}
