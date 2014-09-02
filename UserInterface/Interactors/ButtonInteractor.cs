using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServiceTestStudio.Core;
using System.Windows.Forms;
using System.Windows.Input;


namespace WebServiceTestStudio.UserInterface.Interactors
{
    public class ButtonInteractor
    {
        public ButtonInteractor()
        {

        }

        public void Add(Button button, ICommand command)
        {
            button.Click += new EventHandler(click);
            button.Tag = command;
        }

        private void click(object sender, EventArgs e)
        {
            try
            {
                var button = sender as Button;
                var command = button.Tag as ICommand;
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
