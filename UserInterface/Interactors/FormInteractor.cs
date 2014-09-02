using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebServiceTestStudio.Core;
using WebServiceTestStudio.UserInterface;
using System.Reflection;
using WebServiceTestStudio.Model;
using WebServiceStudio;
using WebServiceTestStudio.Directors;

using WebServiceTestStudio.UserInterface.Commands;

namespace WebServiceTestStudio.UserInterface.Interactors
{
    public class FormInteractor
    {
        private readonly InvokeDirector invokeDirector;

        public FormInteractor(Form form, InvokeDirector invokeDirector)
        {
            this.invokeDirector = invokeDirector;

            form.KeyPreview = true;
            form.KeyUp += form_KeyUp;
        }

        private void form_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    var command = new InvokeCommand(invokeDirector);
                    if (command.CanExecute(null))
                        command.Execute(null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        String.Format("{0} \nInner Exception: {1}.", 
                        ex.Message, 
                        ex.InnerException != null ? ex.InnerException.Message : "none"));
                }
            }
        }
    }
}
