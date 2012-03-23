using System.Data;
using System.IO;
using Kiwi.Json.Conversion;
using NUnit.Framework;

namespace Kiwi.Json.Tests.Conversion
{
    public abstract class DataTableConverterFixtureBase
    {
        protected string GetDataTableXml(DataTable dt)
        {
            dt.TableName = "must have name for xml...";
            var writer = new StringWriter();
            dt.WriteXml(writer);
            return writer.ToString();
        }

        protected static DataTable CreateSampleDataTable()
        {
            var dt = new DataTable();
            dt.Columns.AddRange(new[] {new DataColumn("A"), new DataColumn("B"), new DataColumn("C")});
            dt.Rows.Add(1, 2, 3);
            dt.Rows.Add("four", "five", "six");
            dt.Rows.Add(7, 8, 9);
            return dt;
        }

        [Test]
        public void Convert()
        {
            var sourceDataTable = CreateSampleDataTable();
            var sourceDataTableXml = GetDataTableXml(sourceDataTable);

            var jsonText = JsonConvert.Write(sourceDataTable, CreateDatatableConverter());

            var parsedDataTable = JsonConvert.Parse<DataTable>(jsonText, CreateDatatableConverter());

            var parsedDataTableXml = GetDataTableXml(parsedDataTable);

            Assert.That(parsedDataTableXml, Is.EqualTo(sourceDataTableXml));
        }

        protected abstract IJsonConverter CreateDatatableConverter();
    }
}