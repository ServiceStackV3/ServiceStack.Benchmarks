using System.IO;
using NUnit.Framework;
using ServiceStack.Common.Utils;
using ServiceStack.Northwind.Tests.Support;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Sqlite;

namespace ServiceStack.Northwind.Tests
{
	[TestFixture]
	public class PopulateDataTests
	{
		[Test]
		public void Create_Sqlite_Database()
		{
			var dbPath = "~/App_Data/db.sqlite".MapAbsolutePath();
			var dbFactory = new OrmLiteConnectionFactory(
				dbPath, SqliteOrmLiteDialectProvider.Instance);

			if (File.Exists(dbPath)) File.Delete(dbPath);

			NorthwindData.LoadData();

			dbFactory.Exec(dbCmd =>
			{
				dbCmd.CreateTables(false, NorthwindFactory.ModelTypes.ToArray());

				dbCmd.SaveAll(NorthwindData.Categories);
				dbCmd.SaveAll(NorthwindData.Customers);
				dbCmd.SaveAll(NorthwindData.CustomerCustomerDemos);
				dbCmd.SaveAll(NorthwindData.Employees);
				dbCmd.SaveAll(NorthwindData.EmployeeTerritories);
				dbCmd.SaveAll(NorthwindData.OrderDetails);
				dbCmd.SaveAll(NorthwindData.Orders);
				dbCmd.SaveAll(NorthwindData.Products);
				dbCmd.SaveAll(NorthwindData.Products);
				dbCmd.SaveAll(NorthwindData.Regions);
				dbCmd.SaveAll(NorthwindData.Shippers);
				dbCmd.SaveAll(NorthwindData.Suppliers);
				dbCmd.SaveAll(NorthwindData.Territories);
			});
		}

	}


}
