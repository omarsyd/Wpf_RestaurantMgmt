using System;
using System.IO;
using System.Windows;
using System.Xml.Serialization;

namespace Wpf_Restaurant
{
    public class MyStorage
    {
        public static void WriteXml<T>(T data, string fileName)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                FileStream stream;
                stream = new FileStream(fileName, FileMode.Create);
                serializer.Serialize(stream, data);
                stream.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message);
                throw;
            }
        }

        public static T ReadXml<T>(string fileName)
        {
            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    XmlSerializer xmlSer = new XmlSerializer(typeof(T));
                    return (T)xmlSer.Deserialize(sr);
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message, "Caution...");
                return default(T);
            }

        }
    }
}