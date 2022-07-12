using WTLib.Logger;
using System;
using System.IO;
using System.Xml.Serialization;

namespace WTLib.Utils
{
    public abstract class XmlContext<T> where T : class
    {
        public T Context { get; set; }

        public XmlContext(T context)
        {
            Context = context;
        }

        public void Load(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                Context = Initialize();
                return;
            }

            using (var reader = new StreamReader(filePath))
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    Context = serializer.Deserialize(reader) as T;
                }
                catch (Exception ex)
                {
                    Log.Trace.Error("MappingContext - Load file:{0}, Error: {1}", filePath, ex);
                }
            }
        }

        public void Save(string filePath)
        {
            if (Context == null)
            {
                return;
            }

            using (var writer = new StreamWriter(filePath))
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    serializer.Serialize(writer, Context);
                }
                catch (Exception ex)
                {
                    Log.Trace.Error("MappingContext - Save file:{0}, Error: {1}", filePath, ex);
                }
            }
        }

        protected abstract T Initialize();
    }
}
