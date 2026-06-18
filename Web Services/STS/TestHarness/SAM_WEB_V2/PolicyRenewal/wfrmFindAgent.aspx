<%@ Page Language="VB" AutoEventWireup="false" CodeFile="wfrmFindAgent.aspx.vb" Inherits="Lookup_Screens_FindAgent" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Find Agent</title>
    <script language="javascript" type ="text/javascript">
    
    function passresult()
    {
    if (opener != null)
    {
   
    opener.document.getElementById("txtAgentCode").value = document.getElementById("txtShortName").value;
       
    self.close();
   }

   }
    
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div>
            &nbsp;<table style="width: 536px">
                <tr>
                    <td colspan="2">
                        <asp:Menu ID="mnuFindParty" runat="server" BackColor="#F7F6F3" DynamicHorizontalOffset="2"
                            Font-Names="Verdana" Font-Size="0.8em" ForeColor="#7C6F57" Orientation="Horizontal"
                            StaticSubMenuIndent="10px">
                            <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                            <DynamicHoverStyle BackColor="#7C6F57" ForeColor="White" />
                            <DynamicMenuStyle BackColor="#F7F6F3" />
                            <StaticSelectedStyle BackColor="#5D7B9D" />
                            <DynamicSelectedStyle BackColor="#5D7B9D" />
                            <DynamicMenuItemStyle HorizontalPadding="5px" ItemSpacing="5px" VerticalPadding="2px" />
                            <Items>
                                <asp:MenuItem Text="Company/Person" Value="0"></asp:MenuItem>
                            </Items>
                            <StaticHoverStyle BackColor="#7C6F57" ForeColor="White" />
                        </asp:Menu>
                    </td>
                </tr>
                <tr>
                    <td style="width: 419px">
                        <asp:MultiView ID="mvFindParty" runat="server" ActiveViewIndex="0">
                            <asp:View ID="vCompanyPerson" runat="server">
                                <table style="width: 320px">
                                    <tr>
                                        <td style="width: 99px">
                                            Agent Code</td>
                                        <td>
                                            <asp:TextBox ID="txtClientCode" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 99px; height: 20px">
                                            Name</td>
                                        <td style="height: 20px">
                                            <asp:TextBox ID="txtName" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 99px">
                                            File Code</td>
                                        <td>
                                            <asp:TextBox ID="txtFileCode" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 99px">
                                            Type</td>
                                        <td>
                                            <asp:DropDownList ID="ddlType" runat="server">
                                                <asp:ListItem>All</asp:ListItem>
                                                <asp:ListItem Value="0">Broker</asp:ListItem>
                                                <asp:ListItem Value="3">Intermediary Agent</asp:ListItem>
                                                <asp:ListItem Value="2">Commission Agent</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:CheckBox ID="chkClosedBranches" runat="server" Text="Include Closed Branches" /></td>
                                    </tr>
                                </table>
                            </asp:View>
                           
                        </asp:MultiView></td>
                    <td style="width: 91px">
                        &nbsp;<asp:Button ID="btnNewSearch" runat="server" Text="New Search" CausesValidation="False" /></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                        &nbsp;
                        <asp:Button ID="btnFind" runat="server" Text="Find Now" CausesValidation="False" />
                        <asp:Panel ID="Panel1" runat="server" Height="300px" ScrollBars="Auto" Width="450px">
                            <asp:GridView ID="gvSearchResult" runat="server" CellPadding="4" ForeColor="#333333"
                                GridLines="None" Width="528px">
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <RowStyle BackColor="#F7F6F3" Font-Size="Smaller" ForeColor="#333333" />
                                <EditRowStyle BackColor="#999999" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:CommandField ShowSelectButton="True" />
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                        <asp:Button ID="btnOK" runat="server" Text="OK"  Width="75" OnClientClick="Javascript:passresult()" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="False" />
                        <asp:TextBox ID="txtShortName" runat="server" Visible="True"></asp:TextBox>&nbsp;
                        </td>
                </tr>
            </table>
        </div>
    
    </div>
    </form>
</body>
</html>
