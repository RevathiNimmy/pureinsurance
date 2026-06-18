<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Find Party.aspx.vb" Inherits="Find_Party_Find_Party"
    Title="Find Party" %>

<%@ Register Src="../UserControl/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <link href="../App_Themes/GlobalTheme/GlobalStyleSheet.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table width="100%">
                <tr>
                    <td colspan="2">
                        <uc1:Header ID="Header1" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Menu ID="mnuFindParty" runat="server" BackColor="#F7F6F3" DynamicHorizontalOffset="2"
                            Font-Names="Verdana" Font-Size="0.8em" ForeColor="#7C6F57" Orientation="Horizontal"
                            StaticSubMenuIndent="10px" Width="100%">
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
                                        <td style="width: 84px">
                                            Client Code</td>
                                        <td>
                                            <asp:TextBox ID="txtClientCode" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 84px">
                                            Name</td>
                                        <td>
                                            <asp:TextBox ID="txtName" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 84px">
                                            File Code</td>
                                        <td>
                                            <asp:TextBox ID="txtFileCode" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 84px">
                                            Type</td>
                                        <td>
                                            <asp:DropDownList ID="ddlType" runat="server">
                                                <asp:ListItem Value="0">Personal Client</asp:ListItem>
                                                <asp:ListItem Value="3">Corporate Client</asp:ListItem>
                                                <asp:ListItem Value="1">Group Client</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 84px">
                                            Status</td>
                                        <td>
                                            <asp:DropDownList ID="ddlStatus" runat="server">
                                                <asp:ListItem Value="2" Selected="True">Client</asp:ListItem>
                                                <asp:ListItem Value="1">Prospect</asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td colspan="1" style="height: 21px">
                                        </td>
                                        <td colspan="2" style="height: 21px">
                                            <asp:CheckBox ID="chkClosedBranches" runat="server" Text="Include Closed Branches" />
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 84px">
                                            <asp:Button ID="btnFind" runat="server" Text="Find Now" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnNewSearch" runat="server" Text="New Search" />
                                        </td>
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
                                        <td style="height: 26px">
                                            Date of Birth</td>
                                        <td style="height: 26px">
                                            <asp:TextBox ID="txtDateOfBirth" runat="server"></asp:TextBox></td>
                                    </tr>
                                    
                                </table>
                            </asp:View>
                            <asp:View ID="Policy" runat="server">
                                <br />
                                     <table>
                                     <tr>
                                     <td>
                                <asp:Button ID="btnPolicyNumber" runat="server" Text="Policy Number" />
                              </td>
                                <td><asp:TextBox ID="txtPolicyNumber" runat="server">
                                 </asp:TextBox></td>
                           </tr>
                               
                </table>
                               </asp:View>
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
                        </asp:MultiView></td>
                    <td align="left">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2" style="height: 787px">
                        <asp:GridView ID="gvSearchResult" runat="server" AllowPaging="True" AutoGenerateColumns="False">
                            <Columns>
                            
                            
                                <asp:TemplateField ShowHeader="False">
                                
                                    <ItemTemplate>
                                    
                                       <br />
                                       
                                   <asp:LinkButton ID="lnkList"  runat="server" CommandName="ListPolicy" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"PartyKey")%>'>List Policy</asp:LinkButton>
  
                                                                           </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Type" HeaderText="Type " />
                                <asp:BoundField DataField="ShortName" HeaderText="ShortName " />
                                <asp:BoundField DataField="DateOfBirth" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="DateOfBirth " />
                                <asp:BoundField DataField="ContactTelephoneNumber" HeaderText="ContactTelephoneNumber " />
                                <asp:BoundField DataField="ResolvedName" HeaderText="ResolvedName " />
                                <asp:BoundField DataField="PostCode" HeaderText="PostCode " />
                                <asp:BoundField DataField="SwiftLink" HeaderText="SwiftLink " />
                                <asp:BoundField DataField="AgentKey" HeaderText="AgentKey " />
                                <asp:BoundField DataField="AddressLine2" HeaderText="AddressLine2 " />
                                <asp:BoundField DataField="AddressLine1" HeaderText="AddressLine1 " />
                                <asp:BoundField DataField="FileCode" HeaderText="FileCode " />
                                <asp:BoundField DataField="Status" HeaderText="Status " />
                                <asp:BoundField DataField="PartyKey" HeaderText="PartyKey " />
                                <asp:BoundField DataField="DateOfBirthSpecified" Visible="False" />
                                <asp:BoundField DataField="AgentKeySpecified" Visible="False" />
                            </Columns>
                            <FooterStyle BackColor="White" ForeColor="#000066" />
                            <RowStyle ForeColor="#000066" />
                            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                        </asp:GridView>
                    </td>
                </tr>
               <tr>
                    <td colspan="2">
                        <uc2:Footer ID="Footer2" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
