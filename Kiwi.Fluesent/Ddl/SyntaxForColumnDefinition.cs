using Microsoft.Isam.Esent.Interop;
using Microsoft.Isam.Esent.Interop.Vista;

namespace Kiwi.Fluesent.Ddl
{
    public static class SyntaxForColumnDefinition
    {
        public const JET_CP DefaultCodePage = JET_CP.Unicode;
        public const int DefaultStringLength = 0;
        public const int DefaultBinaryLength = 0;
        public const int DefaultTextLength = 0;
        public const int DefaultLongBinaryLength = 0;

        public static IDatabaseDefinition Database(this IColumnDefinition definition)
        {
            return definition.Table.Database;
        }

        public static IColumnDefinition Column(this IColumnDefinition definition, string name)
        {
            return definition.Table.Column(name);
        }
        public static IIndexDefinition Index(this IColumnDefinition definition, string name)
        {
            return definition.Table.Index(name);
        }
        public static ITableDefinition Table(this IColumnDefinition definition, string name)
        {
            return definition.Table.Database.Table(name);
        }


        public static IColumnDefinition Fixed(this IColumnDefinition definition)
        {
            definition.JetColumnDef.grbit |= ColumndefGrbit.ColumnFixed;
            return definition;
        }

        public static IColumnDefinition Tagged(this IColumnDefinition definition)
        {
            definition.JetColumnDef.grbit |= ColumndefGrbit.ColumnTagged;
            return definition;
        }

        public static IColumnDefinition MultiValued(this IColumnDefinition definition)
        {
            definition.JetColumnDef.grbit |= ColumndefGrbit.ColumnMultiValued;
            return definition;
        }

        public static IColumnDefinition Unversioned(this IColumnDefinition definition)
        {
            definition.JetColumnDef.grbit |= ColumndefGrbit.ColumnUnversioned;
            return definition;
        }

        public static IColumnDefinition Null(this IColumnDefinition definition)
        {
            definition.JetColumnDef.grbit |= ColumndefGrbit.ColumnMaybeNull;
            return definition;
        }

        public static IColumnDefinition NotNull(this IColumnDefinition definition)
        {
            definition.JetColumnDef.grbit |= ColumndefGrbit.ColumnNotNULL;
            return definition;
        }

        public static IColumnDefinition AutoIncrement(this IColumnDefinition definition)
        {
            definition.JetColumnDef.grbit |= ColumndefGrbit.ColumnAutoincrement;
            return definition;
        }

        public static IColumnDefinition AsString(this IColumnDefinition definition)
        {
            return AsString(definition, DefaultStringLength, DefaultCodePage);
        }

        public static IColumnDefinition AsString(this IColumnDefinition definition, int maxLen)
        {
            return definition.AsString(maxLen, DefaultCodePage);
        }

        public static IColumnDefinition AsString(this IColumnDefinition definition, int maxLen, JET_CP codePage)
        {
            definition.JetColumnDef.coltyp = JET_coltyp.Text;
            definition.JetColumnDef.cp = codePage;
            definition.JetColumnDef.cbMax = maxLen;
            return definition;
        }

        public static IColumnDefinition AsText(this IColumnDefinition definition)
        {
            return definition.AsText(DefaultTextLength, DefaultCodePage);
        }

        public static IColumnDefinition AsText(this IColumnDefinition definition, JET_CP codePage)
        {
            return definition.AsText(DefaultTextLength, codePage);
        }

        public static IColumnDefinition AsText(this IColumnDefinition definition, int maxLen)
        {
            return definition.AsText(maxLen, DefaultCodePage);
        }

        public static IColumnDefinition AsText(this IColumnDefinition definition, int maxLen, JET_CP codePage)
        {
            definition.JetColumnDef.coltyp = JET_coltyp.LongText;
            definition.JetColumnDef.cp = codePage;
            definition.JetColumnDef.cbMax = maxLen;
            return definition;
        }

        public static IColumnDefinition AsInt16(this IColumnDefinition definition)
        {
            definition.JetColumnDef.coltyp = JET_coltyp.Short;
            return definition;
        }
 
        public static IColumnDefinition AsInt32(this IColumnDefinition definition)
        {
            definition.JetColumnDef.coltyp = JET_coltyp.Long;
            return definition;
        }

        public static IColumnDefinition AsInt64(this IColumnDefinition definition)
        {
            definition.JetColumnDef.coltyp = VistaColtyp.LongLong;
            return definition;
        }

        public static IColumnDefinition AsDouble(this IColumnDefinition definition)
        {
            definition.JetColumnDef.coltyp = JET_coltyp.IEEEDouble;
            return definition;
        }
        public static IColumnDefinition AsSingle(this IColumnDefinition definition)
        {
            definition.JetColumnDef.coltyp = JET_coltyp.IEEESingle;
            return definition;
        }
        public static IColumnDefinition AsDateTime(this IColumnDefinition definition)
        {
            definition.JetColumnDef.coltyp = JET_coltyp.DateTime;
            return definition;
        }
        public static IColumnDefinition AsBoolean(this IColumnDefinition definition)
        {
            definition.JetColumnDef.coltyp = JET_coltyp.Bit;
            return definition;
        }

        public static IColumnDefinition AsBinary(this IColumnDefinition definition)
        {
            return definition.AsBinary(DefaultBinaryLength);
        }

        public static IColumnDefinition AsBinary(this IColumnDefinition definition, int maxLen)
        {
            definition.JetColumnDef.coltyp = JET_coltyp.Binary;
            definition.JetColumnDef.cbMax = maxLen;
            return definition;
        }

        public static IColumnDefinition AsLongBinary(this IColumnDefinition definition)
        {
            return definition.AsLongBinary(DefaultLongBinaryLength);
        }

        public static IColumnDefinition AsLongBinary(this IColumnDefinition definition, int maxLen)
        {
            definition.JetColumnDef.coltyp = JET_coltyp.LongBinary;
            definition.JetColumnDef.cbMax = maxLen;
            return definition;
        }

    }
}