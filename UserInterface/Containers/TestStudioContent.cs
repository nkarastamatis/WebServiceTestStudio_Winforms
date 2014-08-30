using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using WebServiceTestStudio.Core;

namespace WebServiceTestStudio.UserInterface
{
    public class TestStudioContent : DockContent, ITestStudioControl, ITestStudioDisplay
    {
        protected ITestStudioControl control;

        public TestStudioContent(ITestStudioControl control)
        {
            this.control = control;
            Initialize();
            DockHandler.GetPersistStringCallback = pString;
        }

        public string pString()
        {
            return Text;
        }

        public DockState InitialDockState { get; set; }

        #region ITestStudioControl Members

        public object Content
        {
            get { return control; }
            set { control = value as ITestStudioControl; }
        }

        public void AddEvent(string eventName, Delegate handler)
        {
            control.AddEventHandler(eventName, handler);
        }

        public void Initialize()
        {
            this.SuspendLayout();

            if (this.control != null)
            {
                // call initialize in the constructor
                this.control.Initialize();
                this.Controls.Add(this.control as Control);
            }

            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            //this.ClientSize = new System.Drawing.Size(336, 688);            
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //this.Name = "MethodListDockContent";
            this.ResumeLayout(false);
        }

        public object SelectedContentItem
        {
            get { return control; }
            set { control = value as ITestStudioControl; }
        }

        public string Label
        {
            get { return Text; }
            set { Text = value; }
        }

        #endregion


        #region ITestStudioContainer Members

        void ITestStudioDisplay.Close()
        {
            base.Close();
        }

        void ITestStudioDisplay.Focus()
        {
            base.Activate();
        }

        #endregion
    }
}
