<%@ Page Language="C#" AutoEventWireup="true" CodeFile="List.aspx.cs" Inherits="_List" %>

<%@ Register Src="~/UserControls/ListJoggeling.ascx" TagPrefix="uc1" TagName="ListJoggeling" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Scripts/bootstrap.min.js"></script>
    <script src="Scripts/jquery-1.9.1.min.js"></script>
    <link rel="stylesheet" type="text/css" href="Content/bootstrap-theme.min.css" /> 
    <link rel="stylesheet" type="text/css" href="Content/bootstrap.min.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <uc1:ListJoggeling runat="server" ID="ListJoggeling" />
    </div>
    </form>
</body>
</html>
