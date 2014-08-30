using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebServiceTestStudio.Core;

namespace WebServiceTestStudio.UserInterface
{
    public class TestStudioListBox : ListBox, ITestStudioControl
    {
        
        public TestStudioListBox()
        {
            Initialize();
        }

        #region ITestStudioContent Members

        public object Content
        {
            get { return DataSource; }
            set
            {
                DataSource = value;
            }
        }

        public void Initialize()
        {
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FormattingEnabled = true;
            this.ItemHeight = 16;
            this.Name = "listBox";
            this.DisplayMember = "Name";
        }

        public object SelectedContentItem
        {
            get { return base.SelectedItem; }
            set { base.SelectedItem = value; }
        }

        public string Label
        {
            get { return ((Control)this).Text; }
            set { ((Control)this).Text = value; }
        }

        #endregion

        


    }
}
