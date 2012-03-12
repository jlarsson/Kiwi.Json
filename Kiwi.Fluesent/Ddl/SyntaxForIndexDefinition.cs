using System.Linq;
using Microsoft.Isam.Esent.Interop;

namespace Kiwi.Fluesent.Ddl
{
    public static class SyntaxForIndexDefinition
    {
        public static IIndexDefinition Asc(this IIndexDefinition definition, params string[] columns)
        {
            definition.Columns.AddRange(from column in columns
                                        select new IndexColumnDefinition() {ColumnName = column, SortAscending = true});
            return definition;
        }
        public static IIndexDefinition Desc(this IIndexDefinition definition, params string[] columns)
        {
            definition.Columns.AddRange(from column in columns
                                        select new IndexColumnDefinition() { ColumnName = column, SortAscending = false });
            return definition;
        }
        
        public static IIndexDefinition Unique(this IIndexDefinition definition)
        {
            definition.Grbit |= CreateIndexGrbit.IndexUnique;
            return definition;
        }

        public static IIndexDefinition Primary(this IIndexDefinition definition)
        {
            definition.Grbit |= CreateIndexGrbit.IndexPrimary;
            return definition;
        }

        public static IIndexDefinition IgnoreNull(this IIndexDefinition definition)
        {
            definition.Grbit |= CreateIndexGrbit.IndexIgnoreNull;
            return definition;
        }

        public static IIndexDefinition IgnoreAnyNull(this IIndexDefinition definition)
        {
            definition.Grbit |= CreateIndexGrbit.IndexIgnoreAnyNull;
            return definition;
        }

        public static IIndexDefinition IgnoreFirstNull(this IIndexDefinition definition)
        {
            definition.Grbit |= CreateIndexGrbit.IndexIgnoreFirstNull;
            return definition;
        }

        public static IIndexDefinition DisallowNull(this IIndexDefinition definition)
        {
            definition.Grbit |= CreateIndexGrbit.IndexDisallowNull;
            return definition;
        }

        public static IIndexDefinition SortNullsHigh(this IIndexDefinition definition)
        {
            definition.Grbit |= CreateIndexGrbit.IndexSortNullsHigh;
            return definition;
        }

        public static IIndexDefinition Empty(this IIndexDefinition definition)
        {
            definition.Grbit |= CreateIndexGrbit.IndexEmpty | CreateIndexGrbit.IndexIgnoreAnyNull;
            return definition;
        }

        public static IIndexDefinition LazyFlush(this IIndexDefinition definition)
        {
            definition.Grbit |= CreateIndexGrbit.IndexLazyFlush;
            return definition;
        }

        public static IIndexDefinition Unversioned(this IIndexDefinition definition)
        {
            definition.Grbit |= CreateIndexGrbit.IndexUnversioned;
            return definition;
        }
    }
}