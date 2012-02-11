using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xaml;
using System.Xml;
using JsonFx.Serialization;
using JsonFx.Serialization.Resolvers;
using ServiceStack.Text;
using SerializationException = System.Runtime.Serialization.SerializationException;

namespace Northwind.Benchmarks.Serialization
{
	public static class Utils
	{
		public static string AssemblyVersion(this Type type)
		{
			var version = FileVersionInfo.GetVersionInfo(type.Assembly.Location);
			return " - v{0}.{1}.{2}".Fmt(version.FileMajorPart, version.FileMinorPart, version.FileBuildPart.ToString(CultureInfo.InvariantCulture)[0]);
			
			//var version = type.Assembly.GetName().Version;
			//return " - v{0}.{1}.{2}".Fmt(version.Major, version.MajorRevision, version.Minor);
		}

		public static void Times(this int times, Action<int> actionFn)
		{
			for (var i = 0; i < times; i++)
			{
				actionFn(i);
			}
		}

		public static void Times(this int times, Action actionFn)
		{
			for (var i = 0; i < times; i++)
			{
				actionFn();
			}
		}

		private static readonly BinaryFormatter formatter = new BinaryFormatter();
		public static byte[] SerializeBinaryFormatter<T>(T from)
		{
			try
			{
				using (var ms = new MemoryStream())
				{
					formatter.Serialize(ms, from);
					ms.Flush();
					return ms.ToArray();
				}
			}
			catch (Exception ex)
			{
				throw new SerializationException(string.Format("Error serializing object of type {0}", from.GetType().FullName), ex);
			}
		}

		public static To DeserializeBinaryFormatter<To>(byte[] bytes)
		{
			var type = typeof(To);
			return (To)DeserializeBinaryFormatter(bytes, type);
		}

		public static object DeserializeBinaryFormatter(byte[] bytes, Type type)
		{
			try
			{
				if (bytes == null) throw new ArgumentNullException("bytes");

				using (var ms = new MemoryStream(bytes))
				{
					var obj = formatter.Deserialize(ms);
					return obj;
				}
			}
			catch (Exception ex)
			{
				throw new SerializationException("BinaryFormatterDeserializer: Error converting type: " + ex.Message, ex);
			}
		}

		public static string SerializeXaml<T>(T from)
		{
			try
			{
				using (var sw = new StringWriter())
				{
					XamlServices.Save(sw, from);
					return sw.ToString();
				}
			}
			catch (Exception ex)
			{
				throw new SerializationException(string.Format("Error serializing object of type {0}", from.GetType().FullName), ex);
			}
		}

		public static T DeserializeXaml<T>(string from)
		{
			try
			{
				using (var sw = new StringReader(from))
				{
					return (T) XamlServices.Load(sw);
				}
			}
			catch (Exception ex)
			{
				throw new SerializationException(string.Format("Error serializing object of type {0}", from.GetType().FullName), ex);
			}
		}

        public static T DeserializeXmlSerializer<T>(string xml)
        {
            var type = typeof(T);
            return (T)DeserializeXmlSerializer(xml, type);
        }

        public static object DeserializeXmlSerializer(string xml, Type type)
        {
            try
            {
                using (var reader = new StringReader(xml))
                {
                    var serializer = new System.Xml.Serialization.XmlSerializer(type);
                    return serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                throw new SerializationException("XmlSerializer: Error converting type: " + ex.Message, ex);
            }
        }

        public static string SerializeXmlSerializer<T>(T from)
        {
            try
            {
                using (var writer = new StringWriter())
                {
                    var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                    serializer.Serialize(writer, from);
                    return writer.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new SerializationException(string.Format("XmlSerializer: Error serializing object of type {0}", from.GetType().FullName), ex);
            }
        }

        public static T DeserializeDCS<T>(string xml)
        {
            var type = typeof(T);
            return (T)DeserializeDCS(xml, type);
        }

        public static object DeserializeDCS(string xml, Type type)
        {
            try
            {
                var bytes = Encoding.UTF8.GetBytes(xml);

                using (var reader = XmlDictionaryReader.CreateTextReader(bytes, new XmlDictionaryReaderQuotas()))
                {
                    var serializer = new DataContractSerializer(type);
                    return serializer.ReadObject(reader);
                }
            }
            catch (Exception ex)
            {
                throw new SerializationException("DeserializeDataContract: Error converting type: " + ex.Message, ex);
            }
        }

        public static string SerializeDCS<T>(T from)
        {
            try
            {
                using (var ms = new MemoryStream())
                using (var xw = XmlWriter.Create(ms))
                {
                    var serializer = new DataContractSerializer(from.GetType());
                    serializer.WriteObject(xw, from);
                    xw.Flush();
                    ms.Seek(0, SeekOrigin.Begin);
                    var reader = new StreamReader(ms);
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                throw new SerializationException(string.Format("SerializeDCS: Error serializing object of type {0}", from.GetType().FullName), ex);
            }
        }

        public static T DeserializeNetDCS<T>(string xml)
        {
            var type = typeof(T);
            return (T)DeserializeNetDCS(xml, type);
        }

        public static object DeserializeNetDCS(string xml, Type type)
        {
            try
            {
                var bytes = Encoding.UTF8.GetBytes(xml);

                using (var reader = XmlDictionaryReader.CreateTextReader(bytes, new XmlDictionaryReaderQuotas()))
                {
                    var serializer = new NetDataContractSerializer();
                    return serializer.ReadObject(reader);
                }
            }
            catch (Exception ex)
            {
                throw new SerializationException("DeserializeNetDataContract: Error converting type: " + ex.Message, ex);
            }
        }

        public static string SerializeNetDCS<T>(T from)
        {
            try
            {
                using (var stream = new MemoryStream())
                {
                    var serializer = new NetDataContractSerializer();
                    serializer.Serialize(stream, from);
                    stream.Seek(0, SeekOrigin.Begin);
                    var reader = new StreamReader(stream);
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                throw new SerializationException(string.Format("SerializeNetDataContract: Error serializing object of type {0}", from.GetType().FullName), ex);
            }
        }

		public static object DeserializeDCJS(string json, Type returnType)
		{
			try
			{
				using (var ms = new MemoryStream())
				{
					var bytes = Encoding.UTF8.GetBytes(json);
					ms.Write(bytes, 0, bytes.Length);
					ms.Position = 0;
					var serializer = new DataContractJsonSerializer(returnType);
					return serializer.ReadObject(ms);
				}
			}
			catch (Exception ex)
			{
                throw new SerializationException("DeserializeDCJS: Error converting to type: " + ex.Message, ex);
			}
		}

		public static T DeserializeDCJS<T>(string json)
		{
			return (T)DeserializeDCJS(json, typeof(T));
		}

		public static string SerializeDCJS<T>(T obj)
		{
			if (obj == null) return null;
			var type = obj.GetType();
			try
			{
				using (var ms = new MemoryStream())
				{
					var serializer = new DataContractJsonSerializer(type);
					serializer.WriteObject(ms, obj);
					ms.Position = 0;
					using (var sr = new StreamReader(ms))
					{
						return sr.ReadToEnd();
					}
				}
			}
			catch (Exception ex)
			{
                throw new SerializationException("SerializeDCJS: Error converting type: " + ex.Message, ex);
			}
		}

		private static readonly DataReaderSettings JsonFxReaderSettings = new DataReaderSettings(new DataContractResolverStrategy());
		private static readonly DataWriterSettings JsonFxWriterSettings = new DataWriterSettings(new DataContractResolverStrategy());

        public static T DeserializeJsonFx<T>(string json)
        {
            var reader = new JsonFx.Json.JsonReader(JsonFxReaderSettings);
            return reader.Read<T>(json);
        }

        public static string SerializeJsonFx<T>(T obj)
        {
            var writer = new JsonFx.Json.JsonWriter(JsonFxWriterSettings);
            return writer.Write(obj);
        }

        //http://james.newtonking.com/archive/2009/12/26/json-net-3-5-release-6-binary-json-bson-support.aspx
        public static T DeserializeJsonNetBson<T>(byte[] bson)
        {
            using (var ms = new MemoryStream(bson))
            {
                var serializer = new Newtonsoft.Json.JsonSerializer();
                var reader = new Newtonsoft.Json.Bson.BsonReader(ms);
                return serializer.Deserialize<T>(reader);
            }
        }

        public static byte[] SerializeJsonNetBson<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var serializer = new Newtonsoft.Json.JsonSerializer();
                var writer = new Newtonsoft.Json.Bson.BsonWriter(ms);
                serializer.Serialize(writer, obj);
                var bytes = ms.ToArray();
                return bytes;
            }
        }
		 
	}
}