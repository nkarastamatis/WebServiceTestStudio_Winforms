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
    public class LoadWsdlCommand : ICommand
    {
        private readonly LoadWsdlDirector loadWsdlDirector;

        public LoadWsdlCommand(LoadWsdlDirector loadWsdlDirector)
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
            Cursor.Current = Cursors.WaitCursor;
            loadWsdlDirector.UpdateWsdl();
        }

        #endregion
    }
}
