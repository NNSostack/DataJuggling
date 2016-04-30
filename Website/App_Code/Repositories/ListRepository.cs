//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

///// <summary>
///// Summary description for ListRepository
///// </summary>
//public class ListRepository
//{
//    public static IList GetList(String query)
//    {
//        ListImpl list = new ListImpl();

//        list.ColumnNames.Add(new ColumnNameImpl { Name = "Column1" });
//        list.ColumnNames.Add(new ColumnNameImpl { Name = "Column2" });
//        list.ColumnNames.Add(new ColumnNameImpl { Name = "Column3" });
//        list.ColumnNames.Add(new ColumnNameImpl { Name = "Column4" });

//        list.AddRange(GetListEntries(list, "MyList", 4));

//        return list;
//    }

//    static List<IListEntry> GetListEntries(IList parent, String prefix, int count)
//    {
//        List<IListEntry> list = new List<IListEntry>();
//        for (int i = 0; i < count; i++)
//        {
//            list.Add(new ListEntryImpl { Parent = parent, Title = prefix + i.ToString(), Rows = GetRows(prefix, count) });       
//        }

//        return list;
//    }

//    static List<IRow> GetRows(String prefix, int count)
//    {
//        List<IRow> list = new List<IRow>();
//        for (int i = 0; i < count; i++)
//        {
//             list.Add(new RowImpl { Values = GetValues(prefix, count) });    
//        }

//        return list;
//    }

//    static List<IValue> GetValues(String prefix, int count)
//    {
//        List<IValue> list = new List<IValue>();
//        for (int i = 0; i < count; i++)
//        {
//            list.Add(new ValueImpl { Value = prefix + i.ToString() });  
//        }

//        return list;

//    }
//}