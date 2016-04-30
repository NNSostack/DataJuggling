<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ListJoggeling.ascx.cs" Inherits="UserControls_ListJoggeling" %>

<table class="table table-striped table-hover"<%# Width %>>

    <asp:Repeater ID="columnNames" runat="server">
        <HeaderTemplate>
            <thead>
                <tr>
        </HeaderTemplate>

        <ItemTemplate>
            <th><%# Eval("Name") %></th>
        </ItemTemplate>

        <FooterTemplate>
            </tr>
        </thead>
        </FooterTemplate>

    </asp:Repeater>

    <asp:Repeater runat="server" ID="listJoggler">
        <ItemTemplate>
                <tbody class="table-responsive">
                    <tr>
                        <td colspan="<%# ((System.Collections.Generic.List<Csv.Interfaces.IColumnName>)columnNames.DataSource).Count %>">
                            <h2><%# Eval("title") %></h2>
                        </td>
                    </tr>
                    <asp:Repeater runat="server" DataSource='<%# Eval("Rows") %>'>
                        <ItemTemplate>
                            <tr>
                                <asp:Repeater runat="server" DataSource='<%# Eval("Values") %>'>
                                    <ItemTemplate>
                                        <td><%# Format(Eval("Value")) %></td>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
        </ItemTemplate>

    </asp:Repeater>
</table>

