using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_RSSJoggeling : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var csv = Request.QueryString["csv"];
        var hasColumnNames = Request.QueryString["hasColumnNames"] == "1";
        var groupBy = Request.QueryString["groupBy"] ?? "";
        var filterBy = Request.QueryString["filterBy"] ?? "";
        var filterValue = Request.QueryString["filterValue"] ?? "";
        var excatFilterMatch = Request.QueryString["excatMatch"] == "1" ? true : false;

        var dateColumn = Request.QueryString["dateColumn"] ?? "";
        var linkColumn = Request.QueryString["linkColumn"] ?? "";
        var titleColumn = Request.QueryString["titleColumn"] ?? "";
        var textColumn = Request.QueryString["descriptionColumn"] ?? "";
        
        var title = Request.QueryString["title"];
        var description = Request.QueryString["Description"];

        var list = Csv.CsvListRepository.GetList(
            csv,
            hasColumnNames, groupBy, filterBy, filterValue, excatFilterMatch, null);

        int numberOfDaysToShow;

        if (!int.TryParse(Request.QueryString["numOfDaysToShow"], out numberOfDaysToShow))
            numberOfDaysToShow = -1;

        
        var cols = list.ColumnNames.Select(x => x.Name).ToList();

        int iDateColumn = cols.IndexOf(dateColumn);
        int iLinkColumn = cols.IndexOf(linkColumn);
        int iTitleColumn = cols.IndexOf(titleColumn);
        int iTextColumn = cols.IndexOf(textColumn);

        StringBuilder sb = new StringBuilder();

        
        var orderedList = list.ToList<Csv.Interfaces.IListEntry>().SelectMany(x => x.Rows).ToList();

        if (iDateColumn > -1)
        {
            DateTime now = DateTime.Now; 
            DateTime nowNumberOfDays = now.AddDays(numberOfDaysToShow); 
            orderedList = orderedList.Where(y => ((DateTime)y.Values[iDateColumn].Value).Date >= now
                && (numberOfDaysToShow == -1 || ((DateTime)y.Values[iDateColumn].Value).Date <= nowNumberOfDays)  
                ).ToList();
            orderedList = orderedList.OrderBy(y => ((DateTime)y.Values[iDateColumn].Value)).ToList();  
        }

        foreach (var row in orderedList)
        {
            //foreach( var row in item.Rows )
            {
                sb.AppendFormat(itemFormat, row.Values[iTitleColumn].Value, iTextColumn >= 0 ? row.Values[iTextColumn].Value : "", iLinkColumn >= 0 ? row.Values[iLinkColumn].Value : "", ((DateTime)row.Values[iDateColumn].Value).ToString("r")); 
            }
        }

        var ret = String.Format(rssFormat, title, description, "", DateTime.Now, DateTime.Now, sb.ToString());
        Response.ContentType = "text/xml";
        Response.Write(ret);
        //columnNames.DataSource = list.ColumnNames;

        //listJoggler.DataSource = list;
        //DataBind();
    }



    /// <summary>
    /// DateString.
    /// </summary>
    static string DateString(DateTime pubDate)
    {
        var value = pubDate.ToString("ddd',' d MMM yyyy HH':'mm':'ss") +
            " " +
            pubDate.ToString("zzzz").Replace(":", "");
        return value;
    }

    String rssFormat = @"<?xml version=""1.0"" encoding=""UTF-8"" ?>
<rss version=""2.0"">
    <channel>
        <title>{0}</title>
        <description>{1}</description>
        <link>{2}</link>
        <lastBuildDate>{3}</lastBuildDate>
        <pubDate>{4}</pubDate>
        <ttl>1800</ttl>
        {5}    
    </channel>
</rss>";

    String itemFormat = @"<item>
            <title>{0}</title>
            <description>{1}</description>
            <link>{2}</link>
            <pubDate>{3}</pubDate>
         </item>";
}