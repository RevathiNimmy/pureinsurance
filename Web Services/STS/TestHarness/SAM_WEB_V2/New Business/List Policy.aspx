<%@ Page Language="VB" AutoEventWireup="false" MaintainScrollPositionOnPostback="true" CodeFile="List Policy.aspx.vb" Title="List Policy"
    Inherits="MTA_List_Policy" %>

<%@ Register Src="../UserControl/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>

<%@ Register Src="../UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>List Policy</title>
    <link href="../App_Themes/GlobalTheme/GlobalStyleSheet.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table width="100%" style="height: 100%" border="0" cellspacing="0" cellpadding="0">
                <tr style="height: 10%">
                    <td style="width: 10%" align="right">
                        <table width="100%">
                            <tr>
                                <td>
                                    <uc1:Header ID="Header1" runat="server" />
                                 
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>
                            <asp:Label ID="lblSamErrorMessage" runat="server" ForeColor="Red"></asp:Label><br />
                            <table style="width: 361px; height: 206px">
                                <tr>
                                    <td style="width:20%" height: 28px" >
                                        <asp:Label ID="Label2" runat="server" Text="Client Code"></asp:Label></td>
                                    <td style="width: 103px; height: 28px">
                                        <asp:TextBox ID="txtClientCode" runat="server" ReadOnly="True" Width="201px"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 80px; height: 28px;" valign="Top">
                                        <asp:Label ID="Label1" runat="server" Text="Status"></asp:Label></td>
                                    <td style="width: 103px; height: 28px;" valign="middle">
                                        <asp:Label ID="lblStatus" runat="server" Text="Quote"></asp:Label><br />
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="height: 134px">
                                       <asp:Panel ID="Panel1" runat="server" Height="300px" ScrollBars="Auto" Width="980px">
                                       
                                            <asp:GridView ID="gvResult" runat="server" CellPadding="4" Height="128px" 
                                                AutoGenerateColumns="False" ForeColor="#333333" GridLines="None">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"InsuranceFolderKey")%>'
                                                                CommandName="Select"> Select</asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="PolicyRef" HeaderText="Policy Number" />
                                                    <asp:BoundField DataField="PolicyTypeDesc" HeaderText="Policy Type" />
                                                    <asp:BoundField DataField="ProductDesc" HeaderText="Product" />
                                                    <asp:BoundField DataField="Regarding" HeaderText="Regarding" />
                                                    <asp:BoundField DataField="RenewalDate" HeaderText="Renewal Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                                    <asp:BoundField DataField="AgentShortName" HeaderText="Agent" />
                                                    <asp:BoundField DataField="ThisPremium" HeaderText="Premium" />
                                                    <asp:BoundField DataField="PolicyStatus" HeaderText="Policy Status" />
                                                    <asp:BoundField DataField="RiskTypeDescription" HeaderText="Risk Type Description" />
                                                    <asp:BoundField DataField="EventDescription" HeaderText="Event Description" />
                                                </Columns>
                                                <FooterStyle BackColor="#5D7B9D" ForeColor="White" Font-Bold="True" />
                                                <RowStyle ForeColor="#333333" BackColor="#F7F6F3" Font-Size="Smaller" />
                                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" Font-Size="Smaller" />
                                                <EditRowStyle BackColor="#999999" />
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            </asp:GridView>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <asp:Button ID="btnNew" runat="server" Text="New" />
                    </td>
                </tr>
                <tr><td>
                    <uc2:Footer ID="Footer1" runat="server" />
                </td></tr>
            </table>
        </div>
    </form>
</body>
</html>
