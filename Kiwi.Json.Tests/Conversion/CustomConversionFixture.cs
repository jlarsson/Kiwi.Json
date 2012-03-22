using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Kiwi.Json.Conversion;
using Kiwi.Json.Conversion.TypeBuilders;
using Kiwi.Json.Conversion.TypeWriters;
using NUnit.Framework;

namespace Kiwi.Json.Tests.Conversion
{
    [TestFixture]
    public class CustomConversionFixture
    {
        public class DataTableConverter : JsonProxyConverter
        {
            public override ITypeBuilder CreateTypeBuilder(Type type)
            {
                return TryCreateTypeBuilder<DataTable, DataTableProxy>(type, proxy =>
                                                                                 {
                                                                                     var dt = new DataTable();
                                                                                     dt.Columns.AddRange(
                                                                                         proxy.Columns.Select(
                                                                                             n => new DataColumn(n)).
                                                                                             ToArray());
                                                                                     foreach (var row in proxy.Rows)
                                                                                     {
                                                                                         dt.Rows.Add(row);
                                                                                     }
                                                                                     return dt;
                                                                                 });
            }

            public override ITypeWriter CreateTypeWriter(Type type)
            {
                return TryCreateProxyWriter<DataTable>(type, dt => new DataTableProxy
                                                                       {
                                                                           Columns =
                                                                               dt.Columns.OfType<DataColumn>().Select(
                                                                                   c => c.ColumnName),
                                                                           Rows =
                                                                               dt.Rows.OfType<DataRow>().Select(
                                                                                   r => r.ItemArray)
                                                                       });
            }

            #region Nested type: DataTableProxy

            public class DataTableProxy
            {
                public IEnumerable<string> Columns { get; set; }
                public IEnumerable<object[]> Rows { get; set; }
            }

            #endregion
        }

        private string GetDataTableXml(DataTable dt)
        {
            dt.TableName = "must have name for xml...";
            var writer = new StringWriter();
            dt.WriteXml(writer);
            return writer.ToString();
        }

        private static DataTable CreateSampleDataTable()
        {
            var dt = new DataTable();
            dt.Columns.AddRange(new[] {new DataColumn("A"), new DataColumn("B"), new DataColumn("C")});
            dt.Rows.Add(1, 2, 3);
            dt.Rows.Add("four", "five", "six");
            dt.Rows.Add(7, 8, 9);
            return dt;
        }

        [Test]
        public void CustomConverterPerCall()
        {
            var sourceDataTable = CreateSampleDataTable();
            var sourceDataTableXml = GetDataTableXml(sourceDataTable);

            var jsonText = JsonConvert.Write(sourceDataTable, new DataTableConverter());

            var parsedDataTable = JsonConvert.Parse<DataTable>(jsonText, new DataTableConverter());

            var parsedDataTableXml = GetDataTableXml(parsedDataTable);

            Assert.That(parsedDataTableXml, Is.EqualTo(sourceDataTableXml));
        }

        [Test]
        public void GloballyRegisteredConverter()
        {
            JsonConvert.RegisterCustomConverters(new DataTableConverter());

            var sourceDataTable = CreateSampleDataTable();
            var sourceDataTableXml = GetDataTableXml(sourceDataTable);

            var jsonText = JsonConvert.Write(sourceDataTable);

            var parsedDataTable = JsonConvert.Parse<DataTable>(jsonText);

            var parsedDataTableXml = GetDataTableXml(parsedDataTable);

            Assert.That(parsedDataTableXml, Is.EqualTo(sourceDataTableXml));
        }
    }
}