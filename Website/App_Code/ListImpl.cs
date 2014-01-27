using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


/// <summary>
/// Summary description for ListImpl
/// </summary>
public class ListImpl : List<IListEntry>, IList
{
    public ListImpl()
    {
        ColumnNames = new List<IColumnName>();
        //ListEntries = new List<IListEntry>(); 
    }
    
    public List<IColumnName> ColumnNames { get; set; }
    //public List<IListEntry> ListEntries { get; set; }

    public string CsvLink { get; set; }
}

public class ColumnNameImpl : IColumnName
{
    public ColumnNameImpl()
    {
        Name = "";
        Type = "";
    }
    
    public string Name { get; set; }
    public string Type { get; set; }
}

public class ListEntryImpl : IListEntry
{
    public ListEntryImpl()
    {
        Rows = new List<IRow>(); 
    }

    public string Title { get; set; }
    public IList Parent { get; set; }
    public List<IRow> Rows { get; set; }
}

public class RowImpl : IRow
{
    public RowImpl()
    {
        Values = new List<IValue>();   
    }
     public List<IValue> Values { get; set; }
}

public class ValueImpl : IValue
{

    public Object Value { get; set; }
}
