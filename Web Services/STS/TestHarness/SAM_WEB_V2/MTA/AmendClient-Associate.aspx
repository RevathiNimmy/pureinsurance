<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AmendClient-Associate.aspx.vb"
    Inherits="Find_Party_Find_Party" Title="Find Party" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>

    <script language="javascript" type="text/javascript">
    
    function passresult()
    {
        if (opener != null)
        {
            if (document.getElementById("txtShortName") != null && document.getElementById("hfAgentKey") != null)
            {
                opener.document.getElementById("txtclient").value = document.getElementById("txtShortName").value;
                //opener.document.getElementById("txtLeadAgentCode").value = document.getElementById("txtShortName").value;
                opener.document.getElementById("hdAssociateKey").value = document.getElementById("hfAgentKey").value;
            }
            else
            {
                window.opener.__doPostBack("","")
            }
            window.close();
       }
   }
    
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            &nbsp;<table>
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
                    <td>
                        <asp:MultiView ID="mvFindParty" runat="server">
                            <asp:View ID="vCompanyPerson" runat="server">
                                <table>
                                    <tr>
                                        <td>
                                            Client Code</td>
                                        <td>
                                            <asp:TextBox ID="txtClientCode" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Name</td>
                                        <td>
                                            <asp:TextBox ID="txtName" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            File Code</td>
                                        <td>
                                            <asp:TextBox ID="txtFileCode" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Type</td>
                                        <td>
                                            <asp:DropDownList ID="ddlType" runat="server">
                                                <asp:ListItem Value="0">Personal Client</asp:ListItem>
                                                <asp:ListItem Value="3">Corporate Client</asp:ListItem>
                                                <asp:ListItem Value="1">Group Client</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:CheckBox ID="chkClosedBranches" runat="server" Text="Include Closed Branches" /></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Status</td>
                                        <td>
                                            <asp:DropDownList ID="ddlStatus" runat="server">
                                                <asp:ListItem Value="2" Selected="True">Client</asp:ListItem>
                                                <asp:ListItem Value="1">Prospect</asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="cAddressTelephone" runat="server">
                                <table id="tblAddress" runat="server">
                                    <tr>
                                        <td>
                                            Address Line 1</td>
                                        <td>
                                            <asp:TextBox ID="txtAddrLine1" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Post Code</td>
                                        <td>
                                            <asp:TextBox ID="txtPostCode" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Telephone</td>
                                        <td>
                                            <asp:TextBox ID="txtAreaCode" runat="server" Width="56px"></asp:TextBox>
                                            &nbsp;
                                            <asp:TextBox ID="txtTelephoneNumber" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr id="trDateOfBirth" runat="server">
                                        <td>
                                            Date of Birth</td>
                                        <td>
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
                                <table>
                                    <tr>
                                        <td>
                                            Claim Number</td>
                                        <td>
                                            <asp:TextBox ID="txtClaimNumber" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Risk Index</td>
                                        <td>
                                            <asp:TextBox ID="txtRiskIndex" runat="server"></asp:TextBox></td>
                                    </tr>
                                </table>
                            </asp:View>
                        </asp:MultiView>
                        <asp:Button ID="btnFind" runat="server" Text="Find Now" />
                        <asp:Button ID="btnNewSearch" runat="server" Text="New Search" />
                    </td>
                    <td align="left">
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="gvSearchResult" runat="server" CellPadding="4" ForeColor="#333333"
                            GridLines="None" Width="528px" AllowPaging="True">
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
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;<input id="btnOk" type="button" value="Ok" onclick="Javascript:passresult()" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
                        <asp:TextBox ID="txtShortName" runat="server" Visible="True"></asp:TextBox>
                        <asp:HiddenField ID="hfAgentKey" runat="server" />
                        <asp:HiddenField ID="hfLeadName" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
