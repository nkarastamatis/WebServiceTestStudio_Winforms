using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebServiceTestStudio.Core;

namespace WebServiceTestStudio.UserInterface
{
    public class TestStudioComboBox : ComboBox, ITestStudioControl
    {
        public TestStudioComboBox()
        {
            Initialize();
        }

        #region ITestStudioControl Members

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
            DropDown += TestStudioComboBox_DropDown;
        }

        private void TestStudioComboBox_DropDown(object sender, EventArgs e)
        {
            var senderComboBox = (ComboBox)sender;
            int width = senderComboBox.DropDownWidth;

            int vertScrollBarWidth = (senderComboBox.Items.Count > senderComboBox.MaxDropDownItems)
                    ? SystemInformation.VerticalScrollBarWidth : 0;

            var itemsList = senderComboBox.Items.Cast<object>().Select(item => item.ToString());

            foreach (string s in itemsList)
            {
                int newWidth = TextRenderer.MeasureText(s, senderComboBox.Font).Width;

                if (width < newWidth)
                {
                    width = newWidth;
                }
            }

            senderComboBox.DropDownWidth = width;
        }

        public object SelectedContentItem
        {
            get { return SelectedItem; }
            set { base.Text = value as string; }
        }

        public string Label
        {
            get { return ((Control)this).Text; }
            set { ((Control)this).Text = value; }
        }

        #endregion
    }
}
