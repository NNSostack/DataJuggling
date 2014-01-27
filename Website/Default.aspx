<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default" MasterPageFile="~/MasterPage.master" %>

<asp:Content runat="server" ContentPlaceHolderID="content">
    <div data-ng-show="step() == 1" data-ng-include src="'Views/Step1.html'"></div>
    <div data-ng-show="step() == 2" data-ng-include src="'Views/Step2.html'"></div>
    <div data-ng-show="step() == 3" data-ng-include src="'Views/Step3.html'"></div>
    <div data-ng-show="step() == 4" data-ng-include src="'Views/Step4.html'"></div>

</asp:Content> 
