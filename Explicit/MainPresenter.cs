using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Protocols;
using WebServiceTestStudio.Core;
using WebServiceTestStudio.UserInterface;
using WebServiceTestStudio.Model;
using System.Reflection;
using System.Collections.Specialized;
using System.Windows.Forms;

namespace WebServiceTestStudio.Directors
{
    public class MainPresenter
    {
        private readonly MainForm mainForm;
        private readonly WsdlModel mainModel;
        private readonly TestStudioForm form;

        // Uses Explicit Form and DockContent
        public MainPresenter(MainForm mainForm, WsdlModel mainModel)
        {
            this.mainForm = mainForm;
            this.mainModel = mainModel;

            mainModel.WsdlPath = @"c:\Dev\rts\source_w1\00\bin\release\TransportationWebService.wsdl";

            TypeDescriptorModifier.modifyType(typeof(System.Dynamic.ExpandoObject));
            InitializeForm();
        }

        public MainPresenter(TestStudioForm form, WsdlModel mainModel)
        {
            this.form = form;
            this.mainModel = mainModel;

            mainModel.WsdlPath = @"c:\Dev\rts\source_w1\00\bin\release\TransportationWebService.wsdl";

            

        }        



        #region Helper Functions

        private void InitializeForm()
        {           
            if (mainForm != null)
            {
                mainForm.ClassList = new ClassListDockContent();
                mainForm.ClassList.Show(mainForm.mainDockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockLeft);
                mainForm.MethodList = new MethodListDockContent();
                mainForm.MethodList.Show(mainForm.mainDockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockLeft);

                mainForm.ClassList.AddRange(mainModel.Classes.Cast<string>().ToArray());
                mainForm.MethodList.AddRange(mainModel.Methods.ToArray());
            }

            if (form != null)
            {
                form.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
                form.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                //form.ClientSize = new System.Drawing.Size(1306, 679);
                form.WindowState = FormWindowState.Maximized;
                form.IsMdiContainer = true;
                form.Name = "MainForm";
                form.Text = "Web Service Test Studio";

            }
        }

        #endregion

    }
   
}
