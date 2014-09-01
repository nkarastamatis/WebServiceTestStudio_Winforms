using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WebServiceTestStudio.Core;
using WebServiceTestStudio.UserInterface.Enums;
using WeifenLuo.WinFormsUI.Docking;

namespace WebServiceTestStudio.UserInterface
{
    public class TestStudioFormBuilder
    {
        private static TestStudioFormBuilder instance;

        private TestStudioForm form;
        private ITestStudioControl lastControlAdded;
        //private TestStudioForm currentCompositeControl;
        private TestStudioCompositeControl currentCompositeControl;
        private Dictionary<TestStudioTab, TestStudioCompositeControl> tabs = new Dictionary<TestStudioTab, TestStudioCompositeControl>();
        bool wrapChildren = true;

        private TestStudioControlFactory factory = new TestStudioControlFactory();
        private Dictionary<string, ITestStudioControl> controls = new Dictionary<string, ITestStudioControl>();

        public event EventHandler ControlRemoved;

        public static TestStudioFormBuilder Instance
        {
            get 
            {
                if (instance == null)
                    instance = new TestStudioFormBuilder();

                return instance;
            }
        }

        public void Reset()
        {
            lastControlAdded = null;
            currentCompositeControl = null;
            foreach (var tab in tabs)
                ((Control)tab.Value).Dispose();
            tabs = new Dictionary<TestStudioTab, TestStudioCompositeControl>();
            controls = new Dictionary<string, ITestStudioControl>();
        }

        private TestStudioFormBuilder()
        {

        }

        public void CreateForm(bool useDockPanel = true)
        {
            this.form = new TestStudioForm(useDockPanel ? new TestStudioDockPanel() : null);
            InitializeForm(form);
        }

        public void AddControl(TestStudioControlType type, string label, DockStyle dockStyle, string compositeId)
        {
            currentCompositeControl = GetControl(compositeId) as TestStudioCompositeControl;
            if (currentCompositeControl == null)
                return;

            AddControl(type, label, dockStyle);

            currentCompositeControl = null;
        }

        public void AddControl(TestStudioControlType type, string label, DockStyle dockStyle, ITestStudioContainer compositeControl)
        {
            currentCompositeControl = compositeControl as TestStudioCompositeControl;
            if (currentCompositeControl == null)
                return;

            AddControl(type, label, dockStyle);

            currentCompositeControl = null;
        }

        public void AddControl(TestStudioControlType type, string label, DockStyle dockStyle, TestStudioTab tabId = TestStudioTab.None)
        {
            var control = (type==TestStudioControlType.Composite)? factory.GetCompositeControl(wrapChildren) : factory.GetControl(type);

            if (tabId == TestStudioTab.None)
            {
                if (currentCompositeControl != null)
                    AddChildToComposite(control, label, dockStyle);
                else
                    AddChildToForm(control, label, ConvertToDockState(dockStyle));
            }
            else
            {
                AddChildToTab(tabId, control, label, ConvertToDockState(dockStyle));
            }

            control.AddEventHandler("HandleDestroyed", new EventHandler(TestStudioFormBuilder_HandleDestroyed));
            control.Label = label;

            // TODO: How do I keep this unique??
            try
            {
                controls.Add(label, control);
            }
            catch { }

            lastControlAdded = control;
        }

        void TestStudioFormBuilder_HandleDestroyed(object sender, EventArgs e)
        {
            var control = sender as ITestStudioControl;
            if (control != null)
                controls.Remove(control.Label);

            ControlRemoved(sender, e);
        }        

        public void BeginCompositeControl(string label, bool wrapChildren = false)
        {
            this.wrapChildren = wrapChildren;
            currentCompositeControl = factory.GetCompositeControl(wrapChildren);
            currentCompositeControl.Label = label;
        }

        public void AddCompositeControlToForm(DockStyle dockStyle, TestStudioTab tabId = TestStudioTab.None)
        {
            // If we are not using a dockpanel then we need to set the 
            // native Dock property.
            if (wrapChildren == false)
                (currentCompositeControl as Control).Dock = dockStyle;

            if (tabId == TestStudioTab.None)
            {
                AddChildToForm(currentCompositeControl, currentCompositeControl.Label, ConvertToDockState(dockStyle));
                
            }
            else
            {
                (currentCompositeControl as Control).Dock = DockStyle.Fill;
                AddChildToTab(tabId, currentCompositeControl, currentCompositeControl.Label, ConvertToDockState(dockStyle));
            }

            currentCompositeControl.AddEventHandler("HandleDestroyed", new EventHandler(TestStudioFormBuilder_HandleDestroyed));
            controls.Add(currentCompositeControl.Label, currentCompositeControl);

            currentCompositeControl = null;
        }

        public void CreateCompositeControl(string controlId, bool useDockPanel, DockStyle dockStyle, string compositeId)
        {
            wrapChildren = useDockPanel;
            AddControl(TestStudioControlType.Composite, controlId, dockStyle, compositeId);
        }

        public void CreateCompositeControl(string controlId, bool useDockPanel, DockStyle dockStyle, TestStudioTab tabId = TestStudioTab.None)
        {
            wrapChildren = useDockPanel;
            AddControl(TestStudioControlType.Composite, controlId, dockStyle, tabId);
        }

        public void CreateTab(TestStudioTab tabId)
        {
            var tab = factory.GetCompositeControl(true);
            tab.Label = tabId.ToString();
            tabs.Add(tabId, tab);
            AddChildToForm(tab, tabId.ToString(), DockState.Document);
        }

        public ITestStudioControl GetLastControlAdded()
        {
            return lastControlAdded;
        }

        public ITestStudioControl GetControl(string id)
        {
            ITestStudioControl control = null;
            controls.TryGetValue(id, out control);
            return control;
        }

        public TestStudioForm GetForm()
        {
            return form;
        }

        public TestStudioCompositeControl GetTab(TestStudioTab tabId)
        {
            TestStudioCompositeControl tab = null;
            tabs.TryGetValue(tabId, out tab);
            return tab;
        }


        #region Private Methods

        private void InitializeForm(TestStudioForm form)
        {
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

        private ITestStudioControl WrapChild(ITestStudioControl child, DockState dockState)
        {
            var testStudioContent = new TestStudioContent(child);
            testStudioContent.InitialDockState = dockState;
            return testStudioContent;
        }

        private void AddChildToForm(ITestStudioControl child, string label, DockState dockState)
        {
            var testStudioContent = WrapChild(child, dockState);
            form.AddChild(testStudioContent);
            testStudioContent.Label = label;
        }

        private void AddChildToComposite(ITestStudioControl child, string label, DockStyle dockStyle)
        {
            ITestStudioControl childToAdd = child;

            if (currentCompositeControl.SelectedContentItem != null)
            {
                childToAdd = WrapChild(child, ConvertToDockState(dockStyle));
            }
            else
            {
                var control = child as Control;
                control.Dock = dockStyle;
            }

            childToAdd.Label = label;
            currentCompositeControl.AddChild(childToAdd);
        }

        private void AddChildToTab(TestStudioTab tabId, ITestStudioControl child, string label, DockState dockState)
        {
            var testStudioContent = WrapChild(child, dockState);
            testStudioContent.Label = label;

            TestStudioCompositeControl tab;
            if (tabs.TryGetValue(tabId, out tab))
                tab.AddChild(testStudioContent);
            else
            {
                MessageBox.Show(String.Format("{0} tab does not exist.", tabId.ToString()));
                return;
            }
            
        }

        private DockState ConvertToDockState(DockStyle style)
        {
            switch (style)
            {
                case DockStyle.Bottom:
                    return DockState.DockBottom;
                case DockStyle.Left:
                    return DockState.DockLeft;
                case DockStyle.Right:
                    return DockState.DockRight;
                case DockStyle.Top:
                    return DockState.DockTop;
                default:
                    return DockState.Document;
            }
        }
        #endregion
    }
}
