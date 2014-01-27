using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_TimelineJoggeling : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DataBind(); 
    }

    protected DateTime StartDate { get; set; }
    protected DateTime EndDate { get; set; }

    //String deadlineFormat = "['{0}', '{1}', new Date({2}, {3}, {4}), new Date({5}, {6}, {7})]";
    String deadlineFormat = "[ '{0}', '{1}', new Date({2}, {3}, {4}), new Date({5}, {6}, {7}) ]";

    protected String Links { get; set; }
    protected Boolean showOnlyWithContent;

    protected String GetData()
    {
        StartDate = DateTime.MaxValue;
        EndDate = DateTime.MinValue;

        Links = "links = [";
        var csv = Request.QueryString["csv"];
        var hasColumnNames = Request.QueryString["hasColumnNames"] == "1";
        var groupBy = Request.QueryString["groupBy"] ?? "";
        var startDateColumn = Request.QueryString["startDateColumn"] ?? "";
        var endDateColumn = Request.QueryString["endDateColumn"] ?? "";
        var textColumn = Request.QueryString["TextColumn"] ?? "";
        var filterBy = Request.QueryString["filterBy"] ?? "";
        var filterValue = Request.QueryString["filterValue"] ?? "";
        showOnlyWithContent = Request.QueryString["onlyWithContent"] == "1" ? true : false;
        var excatFilterMatch = Request.QueryString["excatMatch"] == "1" ? true : false;
        
        int numberOfDaysToShow;
        
        if( !int.TryParse(Request.QueryString["numOfDaysToShow"], out numberOfDaysToShow) )
            numberOfDaysToShow = -1;

        var list = CsvListRepository.GetList(
            csv,
            hasColumnNames, groupBy, filterBy, filterValue, excatFilterMatch, null);

        StringBuilder sb = new StringBuilder();

        var cols = list.ColumnNames.Select(x => x.Name).ToList();

        int iStartDateColumn = cols.IndexOf(startDateColumn);
        int iEndDateColumn = cols.IndexOf(endDateColumn);
        int iTextColumn = cols.IndexOf(textColumn);

        if (iStartDateColumn == -1)
            iStartDateColumn = 2;

        if (iTextColumn == -1)
            iTextColumn = 1;

        String sep = ",";
        DateTime dt = DateTime.Now;
        DateTime dtEnd = dt.AddDays(1);
        sb.AppendFormat(deadlineFormat, "Idag", "Idag", dt.Year, dt.Month - 1, dt.Day, dtEnd.Year, dtEnd.Month - 1, dtEnd.Day);

        int linkCount = 0;

        String sepLink = "";
        String qsSep = "";
        foreach (IListEntry entry in list) //.OrderBy(x => x.Rows.First().Values[iStartDateColumn].Value ) )
        {
            linkCount++;
            String label = entry.Title.Replace("'", @"\'");
            String groupLink = "";

            if (entry.Rows.Count > 0 && String.IsNullOrEmpty(Request.QueryString["filterBy"]))
            {

                var indexList = Request.QueryString.Cast<String>().ToList();
                var keys = Request.QueryString.AllKeys;

                Dictionary<String, String> newQS = new Dictionary<string, string>();

                StringBuilder sbLink = new StringBuilder();
                qsSep = "";

                Dictionary<String, String> newQs = new Dictionary<string, string>();
                newQs.Add("groupby", textColumn);
                newQs.Add("filterby", groupBy);
                newQs.Add("filtervalue", entry.Title);

                foreach (String key in Request.QueryString.AllKeys)
                {
                    String val = Request.QueryString[key];

                    if ( !newQs.ContainsKey(key.ToLower()) )
                    {
                        sbLink.Append(qsSep);
                        sbLink.AppendFormat("{0}={1}", key, HttpUtility.UrlEncode(val));
                        qsSep = "&";
                    }
                }

                foreach ( String key in newQs.Keys )
                {
                    sbLink.AppendFormat("&{0}={1}", key, newQs[key]); 
                }
                
                groupLink = Request.RawUrl.Replace(Request.Url.Query, "?") + sbLink.ToString();
                
            }

            Links += sepLink;
            Links += String.Format("'{0}'\r\n", groupLink);
            sepLink = ",";

            if (!showOnlyWithContent)
            {
                sb.AppendLine(",");
                sb.AppendFormat(deadlineFormat, label, "", "1111", "1", "1", "1111", "1", "1");
            }

            foreach (var row in entry.Rows )
            {
                if (row.Values.Count > iStartDateColumn && row.Values[iStartDateColumn].Value is DateTime )
                {
                    dt = (DateTime)row.Values[iStartDateColumn].Value;
                    dtEnd = dt.AddDays(1);

                    if (iEndDateColumn > -1 && row.Values[iEndDateColumn].Value is DateTime )
                    {
                        dtEnd = (DateTime)row.Values[iEndDateColumn].Value;
                    }

                    if (StartDate > dt)
                        StartDate = dt;

                    if (EndDate < dtEnd)
                        EndDate = dtEnd;


                    //if( dt != DateTime.MinValue ) 
                    //if ( DateTime.Now.AddDays(-7) < dt && dt < DateTime.Now.AddMonths(1) )
                    {
                        sb.AppendLine(",");
                        sb.AppendFormat(deadlineFormat, label, row.Values[iTextColumn].Value.ToString().Replace("\n", "").Replace("'", @"\'"), dt.Year, dt.Month - 1, dt.Day, dtEnd.Year, dtEnd.Month - 1, dtEnd.Day);
                    }
                }
            }
        }

        if( numberOfDaysToShow != -1 )
        {
            StartDate = DateTime.Now;

            if( StartDate.AddDays(numberOfDaysToShow) < EndDate )
                EndDate = StartDate.AddDays(numberOfDaysToShow);

            StartDate = StartDate.AddDays(-3);   
        }



        Links += "]";
        return "var timeLineObjects = [" + sb.ToString() + "]";


        
    }

    protected String Width
    {
        get
        {
            if (Request.QueryString["width"] == null)
                return "";

            return String.Format(" style=\"width: {0}px;\"", Request.QueryString["width"]);
        }
    }
}