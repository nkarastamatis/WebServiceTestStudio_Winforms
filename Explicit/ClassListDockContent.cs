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
    public partial class ClassListDockContent : DockContent
    {
        public ClassListDockContent()
        {
            InitializeComponent();
        }

        public int AddItem(Type item)
        {
            return listBox1.Items.Add(item);
        }

        public void AddRange(string[] items)
        {
            listBox1.Items.AddRange(items);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var form = new NewOrRetrieveForm();
            if (form.Setup(listBox1.SelectedItem))
            {
                var result = form.ShowDialog();
            }
        }
    }
}
