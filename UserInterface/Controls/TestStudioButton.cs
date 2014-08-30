using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebServiceTestStudio.Core;

namespace WebServiceTestStudio.UserInterface
{
    public class TestStudioButton : Button, ITestStudioControl
    {
        public TestStudioButton()
        {
            Initialize();            
        }
        #region ITestStudioControl Members

        public object Content 
        {
            get { return Text; }
            set { Text = value as string; }
        }

        public void Initialize()
        {
            
        }

        public object SelectedContentItem { get; set; }

        public string Label
        {
            get { return ((Control)this).Text; }
            set { ((Control)this).Text = value; }
        }

        #endregion
    }
}
