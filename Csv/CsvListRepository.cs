using Csv.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;


namespace Csv
{
    /// <summary>
    /// Summary description for CsvListRepository
    /// </summary>
    public class CsvListRepository
    {
        public CsvListRepository()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        class MyClient : WebClient
        {
            public bool HeadOnly { get; set; }
            protected override WebRequest GetWebRequest(Uri address)
            {
                WebRequest req = base.GetWebRequest(address);
                if (HeadOnly && req.Method == "GET")
                {
                    req.Method = "HEAD";
                }
                return req;
            }
        }

        static public IList GetList(String csvLink, Boolean hasColumnNames, String groupBy, String filterBy, String filterValue, Boolean excatFilterMatch, String[] columnNamesToInclude)
        {
            ListImpl list = new ListImpl();

            //WebClient client = new WebClient();
            String content = "";



            content = GetContent(csvLink.Replace("dl=0", "dl=1"));

            int iStart = content.IndexOf("//dl-web.dropbox.com/");

            if (iStart > 0)
            {
                int iEnd = content.IndexOf("\"", iStart);
                if (iEnd > 0)
                {
                    csvLink = content.Substring(iStart, iEnd - iStart);
                    content = GetContent(csvLink);
                }
            }
            if( columnNamesToInclude != null )
                columnNamesToInclude = columnNamesToInclude.Select(x => x.ToLower()).ToArray();

            var lines = content.Split('\r');

            //var lines = System.IO.File.ReadAllLines(fileName, System.Text.Encoding.UTF7);

            List<int> colsWithContent = new List<int>();

            List<IRow> rows = new List<IRow>();

            for (int i = 0; i < lines.Count(); i++)
            {
                var row = lines[i].Split(';');
                IRow newRow = new RowImpl();
                for (int iCol = 0; iCol < row.Length; iCol++)
                {
                    String colValue = row[iCol];

                    //  ColumnNames
                    if (i == 0 && hasColumnNames && (columnNamesToInclude == null || columnNamesToInclude.Contains(colValue.ToLower())))
                    {
                        if (!String.IsNullOrEmpty(colValue))
                        {
                            if (!String.IsNullOrEmpty(colValue))
                            {
                                colsWithContent.Add(iCol);
                            }

                            list.ColumnNames.Add(new ColumnNameImpl { Name = colValue });
                        }
                    }
                    else if (i > 0 || !hasColumnNames)
                    {
                        if (!hasColumnNames || colsWithContent.Contains(iCol))
                        {
                            int realCol = colsWithContent.IndexOf(iCol);
                            if (hasColumnNames && String.IsNullOrEmpty(list.ColumnNames[realCol].Type))
                                list.ColumnNames[realCol].Type = GetType(colValue);

                            var newVal = new ValueImpl { Value = colValue };
                            Boolean addRow = true;
                            DateTime tTemp;
                            if (list.ColumnNames[realCol].Type == "DateTime")
                            {
                                if (!DateTime.TryParse(colValue, out tTemp))
                                    tTemp = DateTime.MinValue;

                                newVal.Value = tTemp;

                            }

                            if (newVal.Value is String)
                                newVal.Value = newVal.Value.ToString().Replace("\n", "");

                            if (addRow)
                                newRow.Values.Add(newVal);


                        }
                    }
                }

                if (!hasColumnNames || (newRow.Values.Count == list.ColumnNames.Count && newRow.Values.Any(y => !String.IsNullOrEmpty(y.Value as String)) && (i > 0 || !hasColumnNames)))
                    rows.Add(newRow);
            }

            ListEntryImpl listEntry = null;


            if (!String.IsNullOrEmpty(filterBy))
            {
                int index = list.ColumnNames.Select(x => x.Name.ToLower()).ToList().IndexOf(filterBy.ToLower());

                if (index > -1)
                {
                    if (excatFilterMatch)
                        rows = rows.Where(x => (x.Values[index].Value ?? "").ToString() == filterValue).ToList();
                    else
                        rows = rows.Where(x => (x.Values[index].Value ?? "").ToString().ToLower().Contains(filterValue.ToLower())).ToList();
                }
            }



            if (!String.IsNullOrEmpty(groupBy))
            {
                int index = list.ColumnNames.Select(x => x.Name).ToList().IndexOf(groupBy);

                if (index > -1)
                {
                    var grouping = rows.GroupBy(x => x.Values[index].Value.ToString()).OrderBy(x => x.Key);
                    rows = new List<IRow>();

                    foreach (var group in grouping)
                    {
                        listEntry = new ListEntryImpl { Title = group.Key.Replace("\n", ""), Parent = list, Rows = group.ToList() };
                        list.Add(listEntry);
                    }

                }
            }

            if (list.Count() == 0 && rows.Count > 0)
            {
                listEntry = new ListEntryImpl { Parent = list, Rows = rows };
                list.Add(listEntry);
            }

            list.CsvLink = csvLink;

            return list;

        }

        private static string GetContent(String csvLink)
        {
            if (System.IO.File.Exists(csvLink))
                return System.IO.File.ReadAllText(csvLink);

            String content = "";
            using (MyClient client = new MyClient())
            {
                client.HeadOnly = true;
                string uri = csvLink;
                byte[] body = client.DownloadData(uri); // note should be 0-length
                string type = client.ResponseHeaders["content-type"];
                client.HeadOnly = false;
                // check 'tis not binary... we'll use text/, but could
                // check for text/html
                if (type == "application/json" || type.StartsWith(@"text/") || IsTextExtension(csvLink))
                {
                    client.HeadOnly = false;
                    content = client.DownloadString(uri);
                }
                else
                    throw new Exception(@"Dokumentet skal være tekst-baseret. Se den gratis demo for hjælp");
            }
            return content;
        }

        static Boolean IsTextExtension(String url)
        {
            String[] extensions = new String[] { "txt", "csv", "html", "htm" };
            return extensions.Any(x => url.EndsWith("." + x));
        }

        static String GetType(String val)
        {
            DateTime dt;
            Boolean b;
            Uri uri;
            if (DateTime.TryParse(val, out dt))
                return "DateTime";
            if (Boolean.TryParse(val, out b))
                return "Boolean";
            if (Uri.TryCreate(val, UriKind.Absolute, out uri))
                return "Uri";

            return "String";

        }

    }
}