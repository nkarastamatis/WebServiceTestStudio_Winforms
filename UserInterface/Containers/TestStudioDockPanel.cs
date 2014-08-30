using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServiceTestStudio.Core;
using WeifenLuo.WinFormsUI.Docking;

namespace WebServiceTestStudio.UserInterface
{
    public class TestStudioDockPanel : DockPanel, ITestStudioContainer
    {
        static int count = 0;
        readonly int filenumber;
        public TestStudioDockPanel()
        {
            Initialize();
            filenumber = count++;
            //LoadFromXml("C:\\dev\\RTS\\source_w1\\00\\TransportationWebService\\test\\cSharp\\WebServiceTestStudio\\bin\\Debug\\saveasxml" + filenumber, DeserializeDockContent);
            
        }

        private IDockContent DeserializeDockContent(string persistString)
        {
            return null;
        }

        protected override void Dispose(bool disposing)
        {
            
            //SaveAsXml("C:\\dev\\RTS\\source_w1\\00\\TransportationWebService\\test\\cSharp\\WebServiceTestStudio\\bin\\Debug\\saveasxml" + filenumber);
            base.Dispose(disposing);
        }

        #region ITestStudioContainer Members

        public void Initialize()
        {
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Name = "dockPanel";
            this.TabIndex = 0;
            this.DockTopPortion = 0.06;
            this.DockBottomPortion = 0.50;
        }

        public void AddChild(ITestStudioControl child)
        {
            var testStudioContent = child as TestStudioContent;

            if (testStudioContent != null)
                testStudioContent.Show(this, testStudioContent.InitialDockState);
            
        }

        public ITestStudioControl SelectedChild
        {
            get { return ActiveDocument as ITestStudioControl; }
        }

        #endregion
    }
}
