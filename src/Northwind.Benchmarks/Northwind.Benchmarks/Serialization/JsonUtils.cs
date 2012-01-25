using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using JsonFx.Serialization;
using JsonFx.Serialization.Resolvers;
using SerializationException = System.Runtime.Serialization.SerializationException;

namespace Northwind.Benchmarks.Serialization
{
	public static class JsonUtils
	{
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
				{
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
			}
			catch (Exception ex)
			{
				throw new SerializationException(string.Format("Error serializing object of type {0}", from.GetType().FullName), ex);
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
				throw new SerializationException("JsonDataContractDeserializer: Error converting to type: " + ex.Message, ex);
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
				throw new SerializationException("JsonDataContractSerializer: Error converting type: " + ex.Message, ex);
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
		 
	}
}