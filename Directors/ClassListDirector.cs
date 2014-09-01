using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebServiceTestStudio.Core;

namespace WebServiceTestStudio.Directors
{
    internal class ClassListDirector
    {
        private ITestStudioControl classListBox;
        private ITestStudioControl methodsListBox;
        
        public GetMethods GetMethodsByType;

        public ClassListDirector(ITestStudioControl classBox, ITestStudioControl methodBox)
        {
            classListBox = classBox;
            methodsListBox = methodBox;

            classListBox.AddEventHandler("SelectedIndexChanged", new EventHandler(classListBox_SelectedIndexChanged));
        }

        private void classListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GetMethodsByType != null)
            {
                var methods = GetMethodsByType(classListBox.SelectedContentItem as Type);
                methodsListBox.Content = methods.ToArray();
            }
        }
    }
}
