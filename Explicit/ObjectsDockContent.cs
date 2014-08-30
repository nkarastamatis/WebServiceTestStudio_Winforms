using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace WebServiceTestStudio.UserInterface
{
    public partial class ObjectsDockContent : DockContent
    {
        public ObjectsDockContent()
        {
            InitializeComponent();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = (Type)listBox1.SelectedItem;
            var firstOrDefault = Application.OpenForms.OfType<MainForm>().FirstOrDefault();
            if (firstOrDefault != null)
                firstOrDefault.PropertyGrid.SetItem(item);
        }
    }
}
