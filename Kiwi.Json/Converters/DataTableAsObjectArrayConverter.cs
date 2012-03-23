using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Kiwi.Json.Conversion.TypeBuilders;
using Kiwi.Json.Conversion.TypeWriters;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.Converters
{
    public class DataTableAsObjectArrayConverter: AbstractJsonConverter
    {
        private DataTable ConvertJsonArrayToDataTable(IJsonArray array)
        {
            var columnMap = array.OfType<IJsonObject>()
                .Select(obj => obj.Keys)
                .SelectMany(keys => keys)
                .Distinct()
                .Select(
                    (key, index) =>
                    new {Key = key, Index = index})
                .ToDictionary(t => t.Key, t => t.Index);

            var dt = new DataTable();
            dt.Columns.AddRange(columnMap
                                    .OrderBy(kv => kv.Value)
                                    .Select(kv => new DataColumn(kv.Key))
                                    .ToArray());

            foreach (var @object in array.OfType<IJsonObject>())
            {
                var data = new object[columnMap.Count];
                foreach (var kv in @object)
                {
                    data[columnMap[kv.Key]] =
                        kv.Value.ToObject();
                }
                dt.Rows.Add(data);
            }
            return dt;
        }

        private IEnumerable<Dictionary<string, object>> ConvertDataTableToJsonArray(DataTable dt)
        {
            var columnMap =
                dt.Columns.OfType<DataColumn>().Select((c, i) => new {Name = c.ColumnName, Index = i}).ToDictionary(
                    t => t.Index, t => t.Name);

            foreach (var rowData in dt.Rows.OfType<DataRow>().Select(r => r.ItemArray))
            {
                var @object = new Dictionary<string, object>();

                foreach (var kv in columnMap)
                {
                    @object[kv.Value] = rowData[kv.Key];
                }
                yield return @object;
            }

        }

        public override ITypeBuilder CreateTypeBuilder(Type type)
        {
            return TryCreateTypeBuilder<DataTable, IJsonArray>(type, ConvertJsonArrayToDataTable);
        }

        public override ITypeWriter CreateTypeWriter(Type type)
        {
            return TryCreateWriter<DataTable>(type, ConvertDataTableToJsonArray);
        }
    }
}