using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WebServiceTestStudio
{
    public static class ApplicationConstants
    {
        public readonly static string FileHistoryLocation = Path.Combine(Application.UserAppDataPath, "fileHistory.xml");
    }
}
