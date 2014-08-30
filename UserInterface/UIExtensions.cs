using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebServiceTestStudio.Core;


namespace WebServiceTestStudio.UserInterface
{
    public static class UIExtensions
    {
        public static ITestStudioDisplay GetContainer(this ITestStudioControl c)
        {
            var control = c as Control;
            return control.Parent as ITestStudioDisplay;
        }
    }
}
