using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xaml;

namespace AP.Serialization
{
    /// <summary>
    /// Contains methods for deserialization
    /// </summary>
    public abstract class Deserialize : StaticType
    {
        /// <summary>
        /// Deserializes a Xaml string into an object and casts it into the desired generic type.
        /// </summary>
        /// <typeparam name="T">The type the object should be cast to.</typeparam>
        /// <param name="xaml">The xaml string.</param>
        /// <returns>The object.</returns>
        public static T Xaml<T>(string xaml)
        {
            return (T)Xaml(xaml);
        }

        /// <summary>
        /// Deserializes a Xaml string into an object.
        /// </summary>
        /// <param name="xaml">The xaml string.</param>
        /// <returns>The object.</returns>
        public static object Xaml(string xaml)
        {
            return XamlServices.Parse(xaml);
        }
        
        /// <summary>
        /// Deserializes a byte array into an object.
        /// </summary>
        /// <param name="binaries">The byte array.</param>
        /// <returns>The object.</returns>
        public static object Binaries(byte[] binaries)
        {
            object obj = null;

            using (MemoryStream stream = new MemoryStream(binaries))
            {
                stream.Position = 0;
                BinaryFormatter serializer = new BinaryFormatter();

                obj = serializer.Deserialize(stream);
            }
            return obj;
        }

        /// <summary>
        /// Deserializes a byte array into an object.
        /// </summary>
        /// <typeparam name="T">The type the object should be cast to.</typeparam>
        /// <param name="binaries">The byte array.</param>
        /// <returns>The object.</returns>        
        public static T Binaries<T>(byte[] binaries)
        {
            return (T)Binaries(binaries);
        }

        /// <summary>
        /// Deserializes a Json string into an object and casts it into the desired generic type.
        /// </summary>
        /// <typeparam name="T">The type the object should be cast to.</typeparam>
        /// <param name="json">The json string.</param>
        /// <returns>The object.</returns>
        public static T Json<T>(string json)
        {
            return (T)Json(json, typeof(T));
        }
        
        /// <summary>
        /// Deserializes a Json string into an object and casts it into the desired generic type.
        /// </summary>
        /// <param name="json">The json string.</param>
        /// <returns>The object.</returns>
        // code analysis bug: it still occurs after using the proposed syntax to fix this warning.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public static object Json(string json, Type type)
        {
            object obj = null;

            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream();
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(json);
                    stream.Position = 0;
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(type);
                    obj = serializer.ReadObject(stream);
                    stream = null;
                }
            }
            finally
            {
                if (stream != null)
                    stream.Dispose();
            }

            return obj;
        }

        /// <summary>
        /// Deserializes a XML string into an object and casts it into the desired generic type.
        /// </summary>
        /// <typeparam name="T">The type the object should be cast to.</typeparam>
        /// <param name="xml">The xml string.</param>
        /// <returns>The object.</returns>
        public static T Xml<T>(string xml)
        {
            return (T)Xml(xml, typeof(T));
        }

        /// <summary>
        /// Deserializes a XML string into an object and casts it into the desired generic type.
        /// </summary>
        /// <param name="xml">The xml string.</param>
        /// <returns>The object.</returns>
        // code analysis bug: it still occurs after using the proposed syntax to fix this warning.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public static object Xml(string xml, Type type)
        {
            object obj = null;

            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream();
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(xml);
                    stream.Position = 0;
                    DataContractSerializer serializer = new DataContractSerializer(type);
                    obj = serializer.ReadObject(stream);
                    stream = null;
                }
            }
            finally
            {
                if (stream != null)
                    stream.Dispose();
            }

            return obj;
        }
        /// <summary>
        /// Deserializes a NetXML string into an object and casts it into the desired generic type.
        /// </summary>
        /// <typeparam name="T">The type the object should be cast to.</typeparam>
        /// <param name="netXml">The NetXML string.</param>
        /// <returns>The object.</returns>
        // code analysis bug: it still occurs after using the proposed syntax to fix this warning.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times - CA2202 BUG")]
        public static object NetXml(string netXml)
        {
            object obj = null;
            
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream();
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(netXml);
                    stream.Position = 0;
                    NetDataContractSerializer serializer = new NetDataContractSerializer();
                    obj = serializer.ReadObject(stream);
                    stream = null;
                }
            }
            finally
            {
                if (stream != null)
                    stream.Dispose();
            }
            return obj;
        }   
        
        /// <summary>
        /// Deserializes a NetXML string into an object and casts it into the desired generic type.
        /// </summary>
        /// <typeparam name="T">The type the object should be cast to.</typeparam>
        /// <param name="netXml">The NetXML string.</param>
        /// <returns>The object.</returns>
        public static T NetXml<T>(string netXml)
        {
            return (T)NetXml(netXml);
        }
    }
}
