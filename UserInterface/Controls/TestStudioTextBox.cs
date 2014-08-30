using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebServiceTestStudio.Core;

namespace WebServiceTestStudio.UserInterface
{
    public class TestStudioTextBox : TextBox, ITestStudioControl
    {
        public TestStudioTextBox()
        {
            Initialize();
        }

        #region ITestStudioControl Members

        public object Content
        {
            get { return base.Text; }
            set
            {
                var text = value as string;
                if (text != null)
                    base.Text = text;
            }
        }

        public void Initialize()
        {
            //base.AutoSize = true;
            base.Dock = DockStyle.Fill;
            base.FontHeight = 30;            
        }

        public object SelectedContentItem
        {
            get { return base.Text; }
            set { base.Text = value as string; }
        }

        public string Label
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        #endregion
    }
}
