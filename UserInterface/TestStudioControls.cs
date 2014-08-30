using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServiceTestStudio.Core;

namespace WebServiceTestStudio.UserInterface
{

    public class TestStudioControls
    {
        public TestStudioForm MainForm;
        public TestStudioForm InvokeTab;

        public TestStudioListBox ClassesListBox;
        public TestStudioListBox MethodsByClassListBox;
        public TestStudioListBox MethodsListBox;
        public TestStudioComboBox WsdlPathComboBox;
        public TestStudioButton BrowseButton;
        public TestStudioButton LoadButton;

        public TestStudioTextBox RequestTextBox;
        public TestStudioTextBox ResponseTextBox;
    }
}
