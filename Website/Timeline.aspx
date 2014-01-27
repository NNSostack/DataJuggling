<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Timeline.aspx.cs" Inherits="Timeline" %>

<%@ Register Src="~/UserControls/TimelineJoggeling.ascx" TagPrefix="uc1" TagName="TimelineJoggeling" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Timeline by DataJuggling</title>
    <link rel="stylesheet" type="text/css" href="Content/bootstrap.min.css" />

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <uc1:TimelineJoggeling runat="server" ID="TimelineJoggeling" />
    </div>
    </form>
</body>
</html>
