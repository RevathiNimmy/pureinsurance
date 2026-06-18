<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Find Party.aspx.vb" Inherits="Find_Party_Find_Party" %>

<%@ Register Src="../UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>

    <script language="javascript" type="text/javascript">
    
    function passresult()
    {
    if (opener != null)
    {
    opener.document.getElementById("txtShortName").value = document.getElementById("txtShortName").value;
    self.close();
    }
    else
    {      
    location.href = "../New Business/list Policy.aspx"
    }
    
    
   }
    
    </script>

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
            <table style="width: 536px">
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
                                <asp:MenuItem Text="Address/Telephone" Value="1"></asp:MenuItem>
                                <asp:MenuItem Text="Policy" Value="2"></asp:MenuItem>
                                <asp:MenuItem Text="Claim" Value="3"></asp:MenuItem>
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
                                            Client Code</td>
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
                                                <asp:ListItem Value="20">Loss Adjuster</asp:ListItem>
                                                <asp:ListItem Value="16">Other Party</asp:ListItem>
                                                <asp:ListItem Value="15">Repairer</asp:ListItem>
                                                <asp:ListItem Value="21">Solicitor</asp:ListItem>
                                                <asp:ListItem Value="19">Supplier</asp:ListItem>
                                                <asp:ListItem Value="16">Third Party</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:CheckBox ID="chkClosedBranches" runat="server" Text="Include Closed Branches" /></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 99px">
                                            Status</td>
                                        <td>
                                            <asp:DropDownList ID="ddlStatus" runat="server">
                                                <asp:ListItem>All</asp:ListItem>
                                                <asp:ListItem Value="2">Client</asp:ListItem>
                                                <asp:ListItem Value="1">Prospect</asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="cAddressTelephone" runat="server">
                                <table id="tblAddress" style="width: 376px" runat="server">
                                    <tr>
                                        <td style="width: 141px; height: 26px;">
                                            Address Line 1</td>
                                        <td style="height: 26px; width: 227px;">
                                            <asp:TextBox ID="txtAddrLine1" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 141px">
                                            Post Code</td>
                                        <td style="width: 227px">
                                            <asp:TextBox ID="txtPostCode" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 141px">
                                            Telephone</td>
                                        <td style="width: 227px">
                                            <asp:TextBox ID="txtAreaCode" runat="server" Width="56px"></asp:TextBox>
                                            &nbsp;
                                            <asp:TextBox ID="txtTelephoneNumber" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr id="trDateOfBirth" runat="server">
                                        <td style="width: 141px">
                                            Date of Birth</td>
                                        <td style="width: 227px">
                                            <asp:TextBox ID="txtDateOfBirth" runat="server"></asp:TextBox></td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="Policy" runat="server">
                                <br />
                                <asp:Button ID="btnPolicyNumber" runat="server" Text="Policy Number" />
                                &nbsp;&nbsp;
                                <asp:TextBox ID="txtPolicyNumber" runat="server"></asp:TextBox></asp:View>
                            <asp:View ID="Claim" runat="server">
                                <table style="width: 384px">
                                    <tr>
                                        <td style="width: 137px">
                                            Claim Number</td>
                                        <td>
                                            <asp:TextBox ID="txtClaimNumber" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 137px">
                                            Risk Index</td>
                                        <td>
                                            <asp:TextBox ID="txtRiskIndex" runat="server"></asp:TextBox></td>
                                    </tr>
                                </table>
                            </asp:View>
                        </asp:MultiView></td>
                    <td style="width: 91px">
                        &nbsp;<asp:Button ID="btnNewSearch" runat="server" Text="New Search" /></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                        &nbsp;
                        <asp:Button ID="btnFind" runat="server" Text="Find Now" />
                        <asp:Panel ID="Panel1" runat="server" Height="300px" ScrollBars="Auto" Width="450px">
                            <asp:GridView ID="gvSearchResult" runat="server" Width="528px" CellPadding="4" ForeColor="#333333"
                                GridLines="None">
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
                        &nbsp;<input id="btnOk" type="button" value="Ok" onclick="Javascript:passresult()" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
                        <asp:TextBox ID="txtShortName" runat="server" Visible="True"></asp:TextBox></td>
                </tr>
            </table>
            </td>
            </tr>
            </table>
        </div>
    </form>
</body>
</html>
