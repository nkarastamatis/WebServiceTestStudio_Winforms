using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebServiceStudio;
using WebServiceTestStudio.Model;
using WebServiceTestStudio.UserInterface;

namespace WebServiceTestStudio
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            WSSWebRequestCreate.RegisterPrefixes();            
            TypeDescriptorModifier.modifyType(typeof(System.Dynamic.ExpandoObject));

            TestStudioFormBuilder formBuilder = TestStudioFormBuilder.Instance;
            TestStudioFormDesigner formDesigner = new TestStudioFormDesigner(formBuilder, true);
            var form = formBuilder.GetForm();

            Application.Run(form);

            GC.Collect();
            GC.WaitForPendingFinalizers();
            DeleteTempFiles();
        }

        static void DeleteTempFiles()
        {
            foreach (var file in Wsdl.tempfiles)
            {
                // Delete a file by using File class static method...
                if (System.IO.File.Exists(file))
                {
                    // Use a try block to catch IOExceptions, to
                    // handle the case of the file already being
                    // opened by another process.
                    try
                    {
                        System.IO.File.Delete(file);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);                       
                    }
                }
            }
        }
        
    }
}
