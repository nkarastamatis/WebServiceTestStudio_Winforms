using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using WebServiceStudio;
using WebServiceTestStudio.Model;

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
                obj = WsdlModel.ProxyAssembly.CreateInstance(type.Name);
            

            return obj;
        }

        public static void Copy(this object source, ref object copy)
        {
            if (source == null)
                return;

            Type type = source.GetType();
            if (!type.IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }          

            
            foreach (var fieldInfo in type.GetFields())
            {
                var sourceArray = fieldInfo.GetValue(source) as Array;
                if (sourceArray != null)
                {
                    // Create and set an arry of the same type and size
                    var elemType = fieldInfo.FieldType.GetElementType();
                    var copyArray = Array.CreateInstance(elemType, sourceArray.Length);
                    fieldInfo.SetValue(copy, copyArray);

                    // For each element, copy and set
                    for (int ix=0; ix<sourceArray.Length; ix++)
                    {
                        var copyItem = elemType.CreateObject();
                        sourceArray.GetValue(ix).Copy(ref copyItem);
                        copyArray.SetValue(copyItem, ix);
                    }

                    continue;
                }

                if (fieldInfo.FieldType.Namespace == "System" || fieldInfo.FieldType.BaseType.Name == "Enum")
                    fieldInfo.SetValue(copy, fieldInfo.GetValue(source));
                else
                {
                    object childClone = fieldInfo.GetValue(copy);
                    if (childClone == null)
                        childClone = fieldInfo.FieldType.CreateObject();
                    fieldInfo.GetValue(source).Copy(ref childClone);
                    fieldInfo.SetValue(copy, childClone);
                }
            }


            //IFormatter formatter = new BinaryFormatter();
            //Stream stream = new MemoryStream();
            //using (stream)
            //{
            //    formatter.Serialize(stream, source);
            //    stream.Seek(0, SeekOrigin.Begin);
            //    var obj = (object)formatter.Deserialize(stream);
            //    return obj;
            //}
        }

        //public static object Clone(this object source, out object clone)
        //{
        //    if (source == null)
        //        return null;

        //    Type type = source.GetType();
        //    if (!type.IsSerializable)
        //    {
        //        throw new ArgumentException("The type must be serializable.", "source");
        //    }

        //    // Don't serialize a null object, simply return the default for that object
        //    if (Object.ReferenceEquals(source, null))
        //    {
        //        return type.CreateObject();
        //    }

        //    var clone = type.CreateObject();
        //    foreach (var fieldInfo in type.GetFields())
        //    {
        //        if (fieldInfo.FieldType.Namespace == "System")
        //            fieldInfo.SetValue(clone, fieldInfo.GetValue(source));
        //        else
        //            fieldInfo.SetValue(clone, fieldInfo.GetValue(source).Clone());
        //    }

        //    return clone;

        //    //IFormatter formatter = new BinaryFormatter();
        //    //Stream stream = new MemoryStream();
        //    //using (stream)
        //    //{
        //    //    formatter.Serialize(stream, source);
        //    //    stream.Seek(0, SeekOrigin.Begin);
        //    //    var obj = (object)formatter.Deserialize(stream);
        //    //    return obj;
        //    //}
        //}
    }
}
