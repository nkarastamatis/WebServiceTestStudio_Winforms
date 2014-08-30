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
using WebServiceTestStudio.Model;

namespace WebServiceTestStudio.UserInterface
{
    public partial class PropertiesDockContent : DockContent
    {
        public PropertiesDockContent()
        {
            InitializeComponent();
            propertyGrid1.MouseClick +=propertyGrid1_MouseClick;
            propertyGrid1.SelectedObjectsChanged +=new System.EventHandler(this.propertyGrid1_SelectedObjectsChanged);
        }

        private void propertyGrid1_SelectedObjectsChanged(object sender, EventArgs e)
        {
            foreach (System.Windows.Forms.Control control in this.propertyGrid1.Controls[2].Controls)
            {
                control.MouseDown += new System.Windows.Forms.MouseEventHandler(propertyGrid1_MouseClick);
            }
        }

        private void propertyGrid1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var selectedObj = propertyGrid1.SelectedGridItem;

                var mainFrm = Application.OpenForms.OfType<MainForm>().FirstOrDefault();

                var contextMenu = new ContextMenu(mainFrm.GetMenuItems(selectedObj.Value.GetType()));
                contextMenu.Show(sender as Control, e.Location);
            }
        }

        public void SetItem(Type item)
        {
            object obj = null;
            if (item == typeof(String))
                obj = Activator.CreateInstance(item, '\0', 0);
            else if (item.BaseType == typeof(Array))
                obj = Activator.CreateInstance(item, 1);
            else
                obj = Activator.CreateInstance(item);

            TypeDescriptorModifier.modifyType(obj.GetType());
            propertyGrid1.SelectedObject = obj;
            this.Text = item.Name;
        }

        public void SetItem(System.Dynamic.ExpandoObject obj)
        {
            propertyGrid1.SelectedObject = obj;
            TypeDescriptorModifier.modifyType(obj.GetType());
        }
    }
}
