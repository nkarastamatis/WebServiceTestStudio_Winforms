using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebServiceTestStudio.UserInterface
{
    public partial class NewOrRetrieveForm : Form
    {
        public object Item { get; set; }

        public NewOrRetrieveForm()
        {
            InitializeComponent();
        }

        public bool Setup(object item)
        {
            Item = item;
            var mainFrm = Application.OpenForms.OfType<MainForm>().FirstOrDefault();
            var hasSave = false;
            var hasGet = false;

            if (mainFrm != null)
            {
                var methods =
                    mainFrm.Methods.Where(
                        method => method.ReturnType.FullName == ((Type)Item).FullName || method.ReturnType.FullName == ((Type)Item).FullName + "[]");

                foreach (var method in methods)
                {
                    if (method.Name.Contains("Save"))
                    {
                        hasSave = true;
                    }

                    if (method.Name.Contains("Retrieve"))
                    {
                        hasGet = true;
                        comboBox1.Items.Add(method);
                    }
                }

                radioButton1.Enabled = hasSave;
                radioButton1.Checked = hasSave;

                radioButton2.Enabled = hasGet;
                radioButton2.Checked = hasGet && !hasSave;
            }
            return (hasSave || hasGet);
        }

        private void NewOrRetrieveForm_Load(object sender, EventArgs e)
        {

        }
    }
}
