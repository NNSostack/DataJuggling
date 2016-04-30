<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Notification.aspx.cs" Inherits="Notification" %>
<%@ Register Src="~/UserControls/Notification.ascx" TagPrefix="uc1" TagName="Notification" %>    


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <uc1:Notification runat="server" ID="Notification1" />
    </div>
    </form>
</body>
</html>
