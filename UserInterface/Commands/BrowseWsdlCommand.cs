using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServiceTestStudio.Core;
using WebServiceTestStudio.Directors;
using WebServiceTestStudio.UserInterface.Enums;
using System.Reflection;
using System.Windows.Input;
using System.Windows.Forms;

namespace WebServiceTestStudio.UserInterface.Commands
{
    public class BrowseWsdlCommand : ICommand
    {
        private readonly LoadWsdlDirector loadWsdlDirector;

        public BrowseWsdlCommand(LoadWsdlDirector loadWsdlDirector)
        {
            this.loadWsdlDirector = loadWsdlDirector;
        }

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return true;

            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "WSDL Files|*.wsdl";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                loadWsdlDirector.FileSelected(fileDialog.FileName);
            }
        }

        #endregion
    }
}
