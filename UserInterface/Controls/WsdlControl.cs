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
    public partial class WsdlControl : UserControl, ITestStudioControl
    {
        public WsdlControl()
        {
            InitializeComponent();
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
                throw new NotImplementedException();
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
