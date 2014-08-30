using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebServiceTestStudio.Core;

namespace WebServiceTestStudio.UserInterface
{
    public partial class RequestControl : UserControl, ITestStudioControl
    {
        public RequestControl()
        {
            InitializeComponent();
            TestStudioDockPanel panel = resultCompositeControl.SelectedContentItem as TestStudioDockPanel;
            panel.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingWindow;
        }

        #region ITestStudioControl Members

        public void Initialize()
        {
        }

        public string Label { get; set; }

        public object Content
        {
            get
            {
                return requestDataPropertyGrid;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public object SelectedContentItem
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
