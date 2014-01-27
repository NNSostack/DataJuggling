using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class CsvController : ApiController
    {
        public class CsvData
        {
            public Boolean CsvDocumentValid { get; set; }
            public String CsvDocument { get; set; }
            public String DisplayType { get; set; }
            public Boolean HasColumnNames { get; set; }
            public int Width { get; set; }
            public Timeline Timeline { get; set; }
            public List<Column> Columns { get; set; }
            public String GroupBy { get; set; }
            public String FilterBy { get; set; }
            public String FilterValue { get; set; }
            public Boolean ExactFilterMatch { get; set; }
            public Boolean isError { get; set; }
            public String ErrorMessage { get; set; }

            public Boolean TimelineNotSupported { get; set; }
            public Boolean ListNotSupported { get; set; }
        }

        public class Timeline
        {
            public Timeline()
            {
                NumberOfDaysToShowFromNow = -1;
            }

            public String startDateColumn { get; set; }
            public String endDateColumn { get; set; }
            public String TextColumn { get; set; }
            public int NumberOfDaysToShowFromNow { get; set; }
            public Boolean OnlyShowLinesWithContent { get; set; }
        }

        public class Column
        {
            public Column()
            {
                Include = true;
            }
            public String Name { get; set; }
            public String Type { get; set; }
            public Boolean Include { get; set; }
        }

        [HttpPost]
        public CsvData AnalyzeCsvData(CsvData data)
        {
            try
            {
                if (String.IsNullOrEmpty(data.CsvDocument))
                    throw new Exception("Du skal indtaste et link til dokumentet");
                
                data.CsvDocumentValid = true;
                data.Width = 900;
                data.Columns = new List<Column>();
                data.HasColumnNames = true;
                data.GroupBy = "";
                data.FilterBy = "";
                data.FilterValue = "";

                data.Timeline = new Timeline();
                data.Timeline.OnlyShowLinesWithContent = true;
                data.Timeline.NumberOfDaysToShowFromNow = 28;
                data.Timeline.endDateColumn = "";

                data.DisplayType = "Timeline";

                var list = CsvListRepository.GetList(data.CsvDocument, true, "", "", "", false, null);

                foreach (var col in list.ColumnNames)
                {
                    data.Columns.Add(new Column { Name = col.Name, Type = col.Type });
                    if (col.Type == "DateTime" && String.IsNullOrEmpty(data.Timeline.startDateColumn))
                        data.Timeline.startDateColumn = col.Name;
                    else if (col.Type == "DateTime" && String.IsNullOrEmpty(data.Timeline.endDateColumn))
                        data.Timeline.endDateColumn = col.Name;


                    if (col.Type == "String")
                    {
                        if (String.IsNullOrEmpty(data.GroupBy))
                            data.GroupBy = col.Name;
                        else if (String.IsNullOrEmpty(data.Timeline.TextColumn))
                            data.Timeline.TextColumn = col.Name;
                    }

                }
                data.CsvDocument = list.CsvLink;

                if (String.IsNullOrEmpty(data.Timeline.startDateColumn))
                {
                    data.TimelineNotSupported = true;
                    data.DisplayType = "List";
                }
            }
            catch (Exception ex)
            {
                data.CsvDocumentValid = false;
                data.isError = true;
                data.ErrorMessage = ex.Message;
            }

            
            

            return data;


        }


        String urlFormat = "http://{0}/{1}.aspx?csv={2}&hasColumnNames={3}&groupBy={4}&width={5}&filterBy={6}&filterValue={7}&excatMatch={8}";

        [HttpPost]
        public HttpResponseMessage GetUrl(CsvData data)
        {
            var src = String.Format(urlFormat,
                Request.RequestUri.Host + (Request.RequestUri.Port != 80 ? ":" + Request.RequestUri.Port : ""),
                data.DisplayType,
                HttpUtility.UrlEncode(data.CsvDocument),
                data.HasColumnNames ? "1" : "0",
                data.GroupBy,
                data.Width,
                data.FilterBy,
                data.FilterValue,
                data.ExactFilterMatch ? "1" : "0");

            if (data.DisplayType == "Timeline")
                src += "&startDateColumn=" + data.Timeline.startDateColumn +
                   "&endDateColumn=" + data.Timeline.endDateColumn +
                   "&textColumn=" + data.Timeline.TextColumn +
                   "&numOfDaysToShow=" + data.Timeline.NumberOfDaysToShowFromNow +
                   "&onlyWithContent=" + (data.Timeline.OnlyShowLinesWithContent ? "1" : "0");

            if (data.DisplayType == "List")
                src += "&include=" + String.Join("|", data.Columns.Where(x => x.Include).Select(x => x.Name).ToArray());


            var ret = new HttpResponseMessage();
            ret.Content = new StringContent(src);
            return ret;

        }

    }
}
