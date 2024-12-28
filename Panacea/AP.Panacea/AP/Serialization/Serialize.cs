using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xaml;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization.Formatters.Binary;

namespace AP.Serialization
{
    /// <summary>
    /// Contains methods for serialization.
    /// </summary>
    public abstract class Serialize : StaticType
    {
        /// <summary>
        /// Serializes an object to a Xaml string
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Xaml(object obj)
        {
            return XamlServices.Save(obj);
        }

        /// <summary>
        /// Serializes an object into a byte array.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>The byte array.</returns>
        public static byte[] Binaries(object obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter serializer = new BinaryFormatter();
                serializer.Serialize(stream, obj);

                return stream.ToArray();
            }
        }

        /// <summary>
        /// Serializes an object into a json string.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>The json string.</returns>
        // code analysis bug: it still occurs after using the proposed syntax to fix this warning.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public static string Json(object obj)
        {
            string json = null;

            MemoryStream stream = null;

            try
            {
                stream = new MemoryStream();

                DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                serializer.WriteObject(stream, obj);

                stream.Position = 0;

                using (StreamReader reader = new StreamReader(stream))
                {
                    json = reader.ReadToEnd();
                    stream = null;
                }
            }
            finally
            {
                if (stream != null)
                    stream.Dispose();
            }

            return json;
        }

        /// <summary>
        /// Serializes an object into a NetXML string.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>The the netXml string.</returns>
        // code analysis bug: it still occurs after using the proposed syntax to fix this warning.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public static string NetXml(object obj)
        {
            string xml = null;
            
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(); 

                NetDataContractSerializer serializer = new NetDataContractSerializer();
                serializer.WriteObject(stream, obj);

                stream.Position = 0;

                using (StreamReader reader = new StreamReader(stream))
                {
                    xml = reader.ReadToEnd();
                    stream = null;
                }
            }
            finally
            {
                if (stream != null)
                    stream.Dispose();
            }

            return xml;
        }


        /// <summary>
        /// Serializes an object into a XML string.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>The XML string.</returns>
        // code analysis bug: it still occurs after using the proposed syntax to fix this warning.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public static string Xml(object obj)
        {
            string xml = null;
                        
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(); 
                DataContractSerializer serializer = new DataContractSerializer(obj.GetType());
                serializer.WriteObject(stream, obj);

                stream.Position = 0;

                using (StreamReader reader = new StreamReader(stream))
                {
                    xml = reader.ReadToEnd();
                    stream = null;
                }
            } 
            finally
            {
                if (stream != null)
                    stream.Dispose();
            }

            return xml;
        }
    }
}
