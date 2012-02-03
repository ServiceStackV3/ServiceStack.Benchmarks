using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Northwind.Common.DataModel;
using NUnit.Framework;
using ProtoBuf;

namespace Northwind.Benchmarks.Serialization
{
	[TestFixture]
	public class NorthwindDatabaseRowsSerialization
		: SerializationTestBase
	{
		public NorthwindDatabaseRowsSerialization(int iterations)
			: this()
		{
			this.MultipleIterations = new List<int> { iterations };
		}

		public NorthwindDatabaseRowsSerialization()
		{
			this.MultipleIterations = new List<int> { 100000 };

			NorthwindDtoData.LoadData(false);
		}

		[TestFixtureTearDown]
		public void AfterAllTests()
		{
			var htmlSummary =
				"These benchmarks show the total time in ticks (1/1000ms) that it takes"
			  + " each serializer to serialize and deserialize the first row from each table in the "
              + " <a href='https://github.com/ServiceStack/ServiceStack.Benchmarks/blob/master/tests/ServiceStack.Northwind.Tests/Support/NorthwindData.cs'>Northwind Database </a> "
			  + "<strong>" + this.MultipleIterations.Sum().ToString("#,##0") + "</strong> Times. <br/><br/>"
			  + "The full source code of the serialization benchmarks (which generated this report)"
              + " is <a href='https://github.com/ServiceStack/ServiceStack.Benchmarks/blob/master/src/Northwind.Benchmarks/Northwind.Benchmarks.Console/Program.cs'>available here</a>.";

			base.GenerateHtmlReport(htmlSummary);
		}

		[Test]
		public void serialize_Categories()
		{
			SerializeDto(NorthwindDtoData.Instance.Categories[0]);
		}

		[Test]
		public void serialize_Customers()
		{
			SerializeDto(NorthwindDtoData.Instance.Customers[0]);
		}

		[Test]
		public void serialize_Employees()
		{
			SerializeDto(NorthwindDtoData.Instance.Employees[0]);
		}

		[Test]
		public void serialize_EmployeeTerritories()
		{
			SerializeDto(NorthwindDtoData.Instance.EmployeeTerritories[0]);
		}

		[Test]
		public void serialize_OrderDetails()
		{
			SerializeDto(NorthwindDtoData.Instance.OrderDetails[0]);
		}

		[Test]
		public void serialize_Orders()
		{
			SerializeDto(NorthwindDtoData.Instance.Orders[0]);
		}

		[Test]
		public void serialize_Products()
		{
			SerializeDto(NorthwindDtoData.Instance.Products[0]);
		}

		[Test]
		public void serialize_Regions()
		{
			SerializeDto(NorthwindDtoData.Instance.Regions[0]);
		}

		[Test]
		public void serialize_Shippers()
		{
			SerializeDto(NorthwindDtoData.Instance.Shippers[0]);
		}

		[Test]
		public void serialize_Suppliers()
		{
			SerializeDto(NorthwindDtoData.Instance.Suppliers[0]);
		}

		[Test]
		public void serialize_Territories()
		{
			SerializeDto(NorthwindDtoData.Instance.Territories[0]);
		}

		//[Test]
		public void serialize_SimpleObject()
		{
			var dto = new SimpleObject { Id = 1, Name = "Name", Address = "Address", Scores = new[] { 1, 2, 3 } };
			SerializeDto(dto);
		}

		//[Test]
		public void serialize_Customer()
		{
			var dto = CustomerFactory.CreateCustomers(1)[0];
			SerializeDto(dto);
		}

		[ProtoContract(ImplicitFields = ImplicitFields.AllPublic, InferTagFromName = true)]
		[DataContract]
		public class SimpleObject
		{
			[DataMember]
			public int Id { get; set; }

			[DataMember]
			public string Name { get; set; }

			[DataMember]
			public string Address { get; set; }

			[DataMember]
			public int[] Scores { get; set; }
		}

		public enum ShoppingIndexes
		{
			Level0 = 0,
			Level1 = 10,
			Level2 = 20,
			Level3 = 30
		}

		[ProtoContract(ImplicitFields = ImplicitFields.AllPublic, InferTagFromName = true)]
		[DataContract]
		public class Customer
		{
			[DataMember]
			public int Id { get; set; }

			[DataMember]
			public int CustomerNo { get; set; }

			[DataMember]
			public string Firstname { get; set; }

			[DataMember]
			public string Lastname { get; set; }

			[DataMember]
			public ShoppingIndexes ShoppingIndex { get; set; }

			[DataMember]
			public DateTime CustomerSince { get; set; }

			[DataMember]
			public Address BillingAddress { get; set; }

			[DataMember]
			public Address DeliveryAddress { get; set; }

			public Customer()
			{
				ShoppingIndex = ShoppingIndexes.Level0;
				BillingAddress = new Address();
				DeliveryAddress = new Address();
			}
		}

		[ProtoContract(ImplicitFields = ImplicitFields.AllPublic, InferTagFromName = true)]
		[DataContract]
		public class Address
		{
			[DataMember]
			public string Street { get; set; }

			[DataMember]
			public string Zip { get; set; }

			[DataMember]
			public string City { get; set; }

			[DataMember]
			public string Country { get; set; }

			[DataMember]
			public int AreaCode { get; set; }
		}

		internal static class CustomerFactory
		{
			internal static IList<Customer> CreateCustomers(int numOfCustomers)
			{
				var customers = new List<Customer>();

				for (var c = 0; c < numOfCustomers; c++)
				{
					var n = c + 1;
					customers.Add(new Customer {
						CustomerNo = n,
						Firstname = "Daniel",
						Lastname = "Wertheim",
						ShoppingIndex = ShoppingIndexes.Level1,
						CustomerSince = DateTime.Now,
						BillingAddress = {
							Street = "The billing street " + n,
							Zip = "12345",
							City = "The billing City",
							Country = "Sweden-billing",
							AreaCode = 1000 + n
						},
						DeliveryAddress = {
							Street = "The delivery street #" + n,
							Zip = "54321",
							City = "The delivery City",
							Country = "Sweden-delivery",
							AreaCode = -1000 - n
						}
					});
				}

				return customers;
			}
		}
	}
}