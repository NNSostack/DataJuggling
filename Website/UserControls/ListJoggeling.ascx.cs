using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_ListJoggeling : System.Web.UI.UserControl
{


    protected void Page_Load(object sender, EventArgs e)
    {
        var url = Server.UrlEncode("http://intra.netmester.dk/upload/TeamOrca - deadlines.csv");

        //https%3a%2f%2fdl.dropboxusercontent.com%2fs%2f2h8q6zgp0ie1eth%2fArbejdsbeskrivelser.csv%3fdl%3d1%26token_hash%3dAAF3Amy6NBJ_gZQGfII3070HL5XpLHD4uyikwlNjoJL89g&hasColumnNames=true&columnsToIgnore=Ekstra%20info&groupBy=Ansvarlig
 

        //"?https://dl.dropboxusercontent.com/s/2h8q6zgp0ie1eth/Arbejdsbeskrivelser.csv?dl=1&token_hash=AAF3Amy6NBJ_gZQGfII3070HL5XpLHD4uyikwlNjoJL89g&hasColumnNames=true&columnsToIgnore=Ekstra%20info&groupBy=Ansvarlig"
        var csv = Request.QueryString["csv"];
        var hasColumnNames = Request.QueryString["hasColumnNames"] == "1";
        var groupBy = Request.QueryString["groupBy"] ?? "";
        var filterBy = Request.QueryString["filterBy"] ?? "";
        var filterValue = Request.QueryString["filterValue"] ?? "";
        var excatFilterMatch = Request.QueryString["excatMatch"] == "1" ? true : false;
        var columns = (Request.QueryString["include"] ?? "").Split('|');

        var list = Csv.CsvListRepository.GetList(
            csv,
            hasColumnNames, groupBy, filterBy, filterValue, excatFilterMatch, columns);

        columnNames.DataSource = list.ColumnNames;

        listJoggler.DataSource = list;
        DataBind();
    }

    protected String Format(Object obj)
    {
        if (obj is DateTime)
            return ((DateTime)obj).ToString("dd-MM-yyyy");

        return obj.ToString();
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