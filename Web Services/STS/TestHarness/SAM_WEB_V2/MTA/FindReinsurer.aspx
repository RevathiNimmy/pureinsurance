<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FindReinsurer.aspx.vb" Inherits="Find_Party_Find_Party" %>

<%@ Register Src="../UserControl/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>

<%@ Register Src="../UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Find ReInsurer</title>
    <script language="javascript" type ="text/javascript">
    
    function passresult()
    {
    
    if (opener != null)
    {
    opener.document.getElementById("txtCoinsurer").value = document.getElementById("txtShortName").value;
    self.close();
    }
   
   
   
    
    
   }
    


    </script>
    

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
                <td style="height: 659px">
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
                                        Reinsurer Code</td>
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
                                            <asp:ListItem Value="6">Insurer</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:CheckBox ID="chkClosedBranches" runat="server" Text="Include Closed Branches" /></td>
                                </tr>
                            </table>
                        </asp:View>
                                         </asp:MultiView></td>
                <td style="width: 91px">
                    &nbsp;</td>
            </tr>
            <tr>
                <td colspan="2">
                <hr />
                    &nbsp;
                    <asp:Button ID="btnFind" runat="server" Text="Find Now" />
                    <asp:Button ID="btnNewSearch" runat="server" Text="New Search" />
                    <asp:Panel ID="Panel1" runat="server" Height="300px" ScrollBars="Auto" Width="450px">
                    <asp:GridView ID="gvSearchResult" runat="server" Width="528px" CellPadding="4" ForeColor="#333333" GridLines="None">
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
                    <asp:Label ID="lblErrorMessage" runat="server" Width="192px"></asp:Label></td>
            </tr>
            <tr>
                <td colspan="2" style="height: 27px">
                    &nbsp;<input id="btnOk" type="button" value="Ok" onclick="javascript:passresult()" />&nbsp;
                                      
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
                    <asp:TextBox ID="txtShortName" runat="server" Visible="True"></asp:TextBox></td>
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
