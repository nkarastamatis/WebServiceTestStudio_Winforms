using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeifenLuo.WinFormsUI.Docking;
using System.Reflection;
using System.Windows.Forms;

namespace WebServiceTestStudio.UserInterface
{
    public partial class MethodListDockContent : DockContent
    {
        public MethodListDockContent()
        {
            InitializeComponent();

            methodsListBox.DisplayMember = "Name";
        }

        public MethodListDockContent(IContainer container)
        {
            container.Add(this);

            InitializeComponent();            
        }

        public int AddItem(MethodInfo item)
        {
            return methodsListBox.Items.Add(item);
            
        }

        public void AddRange(object[] items)
        {
            methodsListBox.Items.AddRange(items);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var mainFrm = Application.OpenForms.OfType<MainForm>().FirstOrDefault();
            mainFrm.DisplayParameters(methodsListBox.SelectedItem);
        }

    }
}
