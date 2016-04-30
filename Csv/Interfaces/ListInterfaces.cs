using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Csv.Interfaces
{
    public interface IColumnName
    {
        String Name { get; set; }
        String Type { get; set; }
    }

    public interface IList : IList<IListEntry>
    {
        List<IColumnName> ColumnNames { get; set; }
        String CsvLink { get; set; }
    }

    public interface IListEntry
    {
        String Title { get; set; }
        IList Parent { get; set; }
        List<IRow> Rows { get; set; }
    }

    public interface IRow
    {

        List<IValue> Values { get; set; }

    }

    public interface IValue
    {
        Object Value { get; set; }
    }
}