<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ListPolicyVersions.aspx.vb"  Title="ListPolicyVersions" Inherits="MTA_ListPolicyVersions" %>

<%@ Register Src="../UserControl/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>

<%@ Register Src="../UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>List Policy Version</title>
    <link href="../App_Themes/GlobalTheme/GlobalStyleSheet.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
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
        <table style="width: 361px; height: 206px">
            <tr>
                <td style="width: 99px; height: 26px">
                    Client Code</td>
                <td style="width: 97px; height: 26px">
                    <asp:TextBox ID="txtClientCode" runat="server" ReadOnly="True" Width="201px" Enabled="False"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 99px; height: 26px">
                    Policy</td>
                <td style="width: 97px; height: 26px">
                    <asp:TextBox ID="txtPolicy" runat="server" ReadOnly="True" Width="201px" Enabled="False"></asp:TextBox></td>
            </tr>
            <tr>
                <td colspan="2" style="height: 188px">
                    <asp:GridView ID="gvResult" runat="server" AutoGenerateColumns="False" CellPadding="4" Height="132px"
                        Width="345px" ForeColor="#333333" GridLines="None">
                        <FooterStyle BackColor="#5D7B9D" ForeColor="White" Font-Bold="True" />
                        <RowStyle ForeColor="#333333" BackColor="#F7F6F3" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"InsuranceFileKey")%>'
                                        CommandName="Select" OnClientClick="<script>confirm('Are you sure you want to reinstate cancelled policy?');</script>"> Select</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:BoundField DataField="PolicyRef" HeaderText="Policy Number" />--%>
                            <asp:BoundField DataField="PolicyStatus" HeaderText="Policy Status" />
                            
                            <asp:BoundField DataField="InsuranceFileTypedesc" HeaderText="Policy Type"/>
                            <asp:BoundField DataField="CoverStartDate" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Cover Start Date"
                                 />
                            <asp:BoundField DataField="ExpiryDate" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Cover End" />
                            <asp:BoundField DataField="RenewalDate" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Renewal Date" />
                            <asp:BoundField DataField="Premium" HeaderText="Premium" />
                            <asp:BoundField DataField="EventDesc" HeaderText="Event Description" />
                            
                        </Columns>
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#999999" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td><asp:Button ID="btnCancel" runat="server" Text="Cancel" /></td>
                </tr>
        </table>
    
    </div>
    </td>
    </tr>
    <tr>
    <td>
        <uc2:Footer ID="Footer1" runat="server" />
    
    </td>
    </tr>
    </table>
    </form>
</body>
</html>
