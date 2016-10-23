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

            using (var db = dbFactory.Open())
            {
                db.CreateTables(overwrite:false, tableTypes:NorthwindFactory.ModelTypes.ToArray());

                db.SaveAll(NorthwindData.Categories);
                db.SaveAll(NorthwindData.Customers);
                db.SaveAll(NorthwindData.CustomerCustomerDemos);
                db.SaveAll(NorthwindData.Employees);
                db.SaveAll(NorthwindData.EmployeeTerritories);
                db.SaveAll(NorthwindData.OrderDetails);
                db.SaveAll(NorthwindData.Orders);
                db.SaveAll(NorthwindData.Products);
                db.SaveAll(NorthwindData.Products);
                db.SaveAll(NorthwindData.Regions);
                db.SaveAll(NorthwindData.Shippers);
                db.SaveAll(NorthwindData.Suppliers);
                db.SaveAll(NorthwindData.Territories);
            }
        }
    }
}
