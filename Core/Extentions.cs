using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using WebServiceStudio;

namespace WebServiceTestStudio.Core
{
    public static class Extentions
    {
        /// <summary>
        /// Extension method for ITestStudioControl to add event handlers via the interface.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="eventName">Name of event on object.</param>
        /// <param name="handler">The event handler you want to add.</param>
        public static void AddEventHandler(this ITestStudioControl c, string eventName, Delegate handler)
        {
            var e = c.GetType().GetEvent(eventName);

            // If the event does not exist, warn the user/developer, but keep going.
            if (e == null)
            {
                try { throw new Exception(String.Format("Event: {0} not found on {1}.", eventName, c.ToString())); }
                catch { return; }
            }

            e.AddEventHandler(c, handler);
        }

        public static object CreateObject(this Type type)
        {
            object obj = null;
            if (type == typeof(String))
                obj = Activator.CreateInstance(type, '\0', 0);
            else if (type.BaseType == typeof(Array))
                obj = Activator.CreateInstance(type, 1);
            else if (type.Namespace != "System")
                obj = Activator.CreateInstance(type);
            

            return obj;
        }

        public static object Clone(this object source)
        {
            Type type = source.GetType();
            if (!type.IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return type.CreateObject();
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(stream);
            }
        }
    }
}
