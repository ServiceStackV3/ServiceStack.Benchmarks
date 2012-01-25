using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using JsonFx.Serialization;
using JsonFx.Serialization.Resolvers;
using Northwind.Common.ComplexModel;
using Northwind.Common.ServiceModel;
using Northwind.Perf;
using NUnit.Framework;
using ServiceStack.Text;
using JsonConvert=Jayrock.Json.Conversion.JsonConvert;
using SerializationException = System.Runtime.Serialization.SerializationException;

namespace Northwind.Benchmarks.Serialization
{
	[TestFixture]
	public class TextDeserializerPerfTests
		: PerfTestBase
	{
		public TextDeserializerPerfTests()
		{
			this.MultipleIterations = new List<int> { 1000, 10000 };			
		}

		public void WriteLog()
		{
			var fileName = string.Format(@"C:\src\ServiceStack.Benchmarks\src\Northwind.Benchmarks\Northwind.Benchmarks\App_Data.{0:yyyy-MM-dd}.log", DateTime.Now);
			using (var writer = new StreamWriter(fileName, true))
			{
				writer.Write(SbLog);
			}
		}

		protected void RunMultipleTimes(ScenarioBase scenarioBase)
		{
			RunMultipleTimes(scenarioBase.Run, scenarioBase.GetType().Name);
		}

		[Test]
		public void Deserialize_Customer()
		{
			var dto = DtoFactory.CustomerDto;

			var dtoXml = JsonUtils.SerializeDCS(dto);
			var dtoJson = JsonUtils.SerializeDCJS(dto);
			var dtoJayrock = JsonConvert.ExportToString(dto);
			var dtoJsv = TypeSerializer.SerializeToString(dto);
			var dtoJsonSs = JsonSerializer.SerializeToString(dto);
			var dtoJsonFx = JsonUtils.SerializeJsonFx(dto);

			RunMultipleTimes(() => JsonUtils.DeserializeDCS<CustomerDto>(dtoXml), "JsonUtils.DeserializeDCS<CustomerDto>(dtoXml)");
			RunMultipleTimes(() => JsonUtils.DeserializeDCJS<CustomerDto>(dtoJson), "JsonUtils.DeserializeDCJS<CustomerDto>(dtoJson)");
			RunMultipleTimes(() => JsonConvert.Import(typeof(CustomerDto), dtoJayrock), "JsonConvert.Import(typeof(CustomerDto), dtoJayrock)");
			RunMultipleTimes(() => TypeSerializer.DeserializeFromString<CustomerDto>(dtoJsv), "TypeSerializer.DeserializeFromString<CustomerDto>(dtoJsv)");
			RunMultipleTimes(() => JsonSerializer.DeserializeFromString<CustomerDto>(dtoJsonSs), "JsonSerializer.DeserializeFromString<CustomerDto>(dtoSSJson)");
			RunMultipleTimes(() => JsonUtils.DeserializeJsonFx<CustomerDto>(dtoJsonFx), "JsonUtils.DeserializeJsonFx<CustomerDto>(dtoJsonFx)");
		}

		[Test]
		public void Deserialize_Order()
		{
			var dto = DtoFactory.OrderDto;

			var dtoXml = JsonUtils.SerializeDCS(dto);
			var dtoJson = JsonUtils.SerializeDCJS(dto);
			var dtoString = TypeSerializer.SerializeToString(dto);

			RunMultipleTimes(() => JsonUtils.DeserializeDCS<OrderDto>(dtoXml), "JsonUtils.DeserializeDCS<OrderDto>(dtoXml)");
			RunMultipleTimes(() => JsonUtils.DeserializeDCJS<OrderDto>(dtoJson), "JsonUtils.DeserializeDCJS<OrderDto>(dtoJson)");
			RunMultipleTimes(() => TypeSerializer.DeserializeFromString<OrderDto>(dtoString), "TypeSerializer.DeserializeFromString<OrderDto>(dtoString)");
			RunMultipleTimes(() => JsonSerializer.DeserializeFromString<OrderDto>(dtoString), "JsonSerializer.DeserializeFromString<OrderDto>(dtoString)");
		}

		[Test]
		public void Deserialize_Supplier()
		{
			var dto = DtoFactory.SupplierDto;

			var dtoXml = JsonUtils.SerializeDCS(dto);
			var dtoJson = JsonUtils.SerializeDCJS(dto);
			var dtoString = TypeSerializer.SerializeToString(dto);
			var dtoPlatformText = TextSerializer.SerializeToString(dto);

			RunMultipleTimes(() => JsonUtils.DeserializeDCS<SupplierDto>(dtoXml), "JsonUtils.DeserializeDCS<SupplierDto>(dtoXml)");
			RunMultipleTimes(() => JsonUtils.DeserializeDCJS<SupplierDto>(dtoJson), "JsonUtils.DeserializeDCJS<SupplierDto>(dtoJson)");
			RunMultipleTimes(() => TypeSerializer.DeserializeFromString<SupplierDto>(dtoString), "TypeSerializer.DeserializeFromString<SupplierDto>(dtoString)");
			RunMultipleTimes(() => TextSerializer.DeserializeFromString<SupplierDto>(dtoPlatformText), "TextSerializer.DeserializeFromString<SupplierDto>(dtoPlatformText)");
		}

		[Test]
		public void Deserialize_MultiDtoWithOrders()
		{
			var dto = DtoFactory.MultiDtoWithOrders;

			var dtoXml = JsonUtils.SerializeDCS(dto);
			var dtoJson = JsonUtils.SerializeDCJS(dto);
			var dtoString = TypeSerializer.SerializeToString(dto);
			var dtoPlatformText = TextSerializer.SerializeToString(dto);

			RunMultipleTimes(() => JsonUtils.DeserializeDCS<MultiDtoWithOrders>(dtoXml), "JsonUtils.DeserializeDCS<MultiDtoWithOrders>(dtoXml)");
			RunMultipleTimes(() => JsonUtils.DeserializeDCJS<MultiDtoWithOrders>(dtoJson), "JsonUtils.DeserializeDCJS<MultiDtoWithOrders>(dtoJson)");
			RunMultipleTimes(() => TypeSerializer.DeserializeFromString<MultiDtoWithOrders>(dtoString), "TypeSerializer.DeserializeFromString<MultiDtoWithOrders>(dtoString)");
			RunMultipleTimes(() => TextSerializer.DeserializeFromString<MultiDtoWithOrders>(dtoPlatformText), "TextSerializer.DeserializeFromString<MultiDtoWithOrders>(dtoPlatformText)");
		}

		[Test]
		public void Deserialize_ArrayDtoWithOrders()
		{
			var dto = DtoFactory.ArrayDtoWithOrders;

			var dtoXml = JsonUtils.SerializeDCS(dto);
			var dtoJson = JsonUtils.SerializeDCJS(dto);
			var dtoString = TypeSerializer.SerializeToString(dto);
			var dtoPlatformText = TextSerializer.SerializeToString(dto);

			RunMultipleTimes(() => JsonUtils.DeserializeDCS<ArrayDtoWithOrders>(dtoXml), "JsonUtils.DeserializeDCS<ArrayDtoWithOrders>(dtoXml)");
			RunMultipleTimes(() => JsonUtils.DeserializeDCJS<ArrayDtoWithOrders>(dtoJson), "JsonUtils.DeserializeDCJS<ArrayDtoWithOrders>(dtoJson)");
			RunMultipleTimes(() => TypeSerializer.DeserializeFromString<ArrayDtoWithOrders>(dtoString), "TypeSerializer.DeserializeFromString<ArrayDtoWithOrders>(dtoString)");
			RunMultipleTimes(() => TextSerializer.DeserializeFromString<ArrayDtoWithOrders>(dtoPlatformText), "TextSerializer.DeserializeFromString<ArrayDtoWithOrders>(dtoPlatformText)");
		}

		[Test]
		public void Deserialize_CustomerOrderListDto()
		{
			var dto = DtoFactory.CustomerOrderListDto;
			Console.WriteLine(dto.Dump());

			var dtoXml = JsonUtils.SerializeDCS(dto);
			var dtoJson = JsonUtils.SerializeDCJS(dto);
			var dtoString = TypeSerializer.SerializeToString(dto);
			var dtoPlatformText = TextSerializer.SerializeToString(dto);

			RunMultipleTimes(() => JsonUtils.DeserializeDCS<CustomerOrderListDto>(dtoXml), "JsonUtils.DeserializeDCS<CustomerOrderListDto>(dtoXml)");
			RunMultipleTimes(() => JsonUtils.DeserializeDCJS<CustomerOrderListDto>(dtoJson), "JsonUtils.DeserializeDCJS<CustomerOrderListDto>(dtoJson)");
			RunMultipleTimes(() => TypeSerializer.DeserializeFromString<CustomerOrderListDto>(dtoString), "TypeSerializer.DeserializeFromString<CustomerOrderListDto>(dtoString)");
			RunMultipleTimes(() => TextSerializer.DeserializeFromString<CustomerOrderListDto>(dtoPlatformText), "TextSerializer.DeserializeFromString<CustomerOrderListDto>(dtoPlatformText)");
		}

		[Test]
		public void Deserialize_CustomerOrderArrayDto()
		{
			var dto = DtoFactory.CustomerOrderArrayDto;

			var dtoXml = JsonUtils.SerializeDCS(dto);
			var dtoJson = JsonUtils.SerializeDCJS(dto);
			//var dtoJayrock = JsonConvert.ExportToString(dto);
			var dtoString = TypeSerializer.SerializeToString(dto);
			var dtoPlatformText = TextSerializer.SerializeToString(dto);

			RunMultipleTimes(() => JsonUtils.DeserializeDCS<CustomerOrderArrayDto>(dtoXml), "JsonUtils.DeserializeDCS<CustomerOrderArrayDto>(dtoXml)");
			RunMultipleTimes(() => JsonUtils.DeserializeDCJS<CustomerOrderArrayDto>(dtoJson), "JsonUtils.DeserializeDCJS<CustomerOrderArrayDto>(dtoJson)");
			//RunMultipleTimes(() => JsonConvert.Import(typeof(CustomerOrderArrayDto), dtoJayrock), "JsonConvert.Import(typeof(CustomerOrderArrayDto), dtoJayrock)");  #doesn't do nullables
			RunMultipleTimes(() => TypeSerializer.DeserializeFromString<CustomerOrderArrayDto>(dtoString), "TypeSerializer.DeserializeFromString<CustomerOrderArrayDto>(dtoString)");
			RunMultipleTimes(() => TextSerializer.DeserializeFromString<CustomerOrderArrayDto>(dtoPlatformText), "TextSerializer.DeserializeFromString<CustomerOrderArrayDto>(dtoPlatformText)");
		}
	}
}