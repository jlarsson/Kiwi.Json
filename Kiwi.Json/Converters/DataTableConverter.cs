using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Kiwi.Json.Conversion;
using Kiwi.Json.Conversion.TypeBuilders;
using Kiwi.Json.Conversion.TypeWriters;

namespace Kiwi.Json.Converters
{
    public class DataTableConverter : AbstractJsonConverter
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
}
