<%@ Page Language="VB" AutoEventWireup="false" CodeFile="findaccount.aspx.vb" Inherits="findaccount" %>

<%@ Register Src="../UserControl/Header.ascx" TagName="Header" TagPrefix="uc3" %>




<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link href="../App_Themes/GlobalTheme/GlobalStyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../Images/loginstylesheet.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/GlobalTheme/GlobalStyleSheet.css" rel="stylesheet" type="text/css" />
     <script language="javascript" type ="text/javascript">
    
    function passresult()
    {

    if (opener != null)
    {
  
    if (opener.document.getElementById("txtShortName") != null && opener.document.getElementById("hfAccountKey") != null)
    {
     
    opener.document.getElementById("txtShortName").value = document.getElementById("txtShortName").value;
  
    opener.document.getElementById("hfAccountKey").value = document.getElementById("hfAccountKey").value;
    }
    else
    {
    window.opener.__doPostBack("","")
    }
    
    self.close();
   }

   }
    
    </script>

    <link href="../App_Themes/GlobalTheme/GlobalStyleSheet.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <table>
            <tr>
                <td style="height: 43px">
                    &nbsp;&nbsp;
                    <uc3:Header ID="Header1" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" BorderStyle="Double" Font-Bold="True" Font-Italic="True"
                        ForeColor="#C000C0" Text="Find Accont" Width="256px"></asp:Label>
                    <table width="100%">
                        <tr>
                            <td>
         
                                <asp:Menu ID="mnuGetAccountDetails" runat="server" BackColor="#F7F6F3" DynamicHorizontalOffset="2"
                                    Font-Names="Verdana" Font-Size="0.8em" ForeColor="#7C6F57" Orientation="Horizontal"
                                    StaticSubMenuIndent="10px">
                                    <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                                    <DynamicHoverStyle BackColor="#7C6F57" ForeColor="White" />
                                    <DynamicMenuStyle BackColor="#F7F6F3" />
                                    <StaticSelectedStyle BackColor="#5D7B9D" />
                                    <DynamicSelectedStyle BackColor="#5D7B9D" />
                                    <DynamicMenuItemStyle HorizontalPadding="5px" ItemSpacing="5px" VerticalPadding="2px" />
                                    <Items>
                                        <asp:MenuItem Text="1 - Details" Value="0"></asp:MenuItem>
                                        <asp:MenuItem Text="2 - Reference" Value="1"></asp:MenuItem>
                                       
                                    </Items>
                                    <StaticHoverStyle BackColor="#7C6F57" ForeColor="White" />
                                </asp:Menu>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 528px">
                                <asp:MultiView ID="mvGetAccountDetails" runat="server" ActiveViewIndex="0">
                                <asp:View runat="server" ID="vDetails">
                                <table>
                                <tr>
                                <td >
                             Short Name:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtShortAccountName" runat="server"></asp:TextBox>
                                </td>
                                </tr>
                                <tr>
                                <td >
                               Name:
                                </td>
                                <td >
                                   <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                                </td>
                                </tr>
                                <tr>
                                <td >
                                Code:
                                </td>
                                    
                                <td>
                               <asp:TextBox ID="txtCode" runat="server"></asp:TextBox>
                                </td>
                                </tr>
                                <tr>
                                <td >
                            Account:
                                </td>
                                <td>
                                   <asp:DropDownList ID="ddlAccount" runat="server">
                                    </asp:DropDownList>
                                </td>
                                </tr>
                                <tr>
                                <td>
                                    <asp:CheckBox ID="chkShowAccountBalance"  Text="Show Account Balance" runat="server" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkShowdeletedAccounts"  Text =" ShowdeletedAccounts" runat="server" />
                                </td>
                                </tr>
                                </table>
                                  </asp:View>
                                <asp:View runat="server" ID="vReference">
                                <table>
                                <tr>
                                <td >
                                Insurance Ref
                                </td>
                                <td>
                                    <asp:TextBox ID="txtInsuranceRef" runat="server"></asp:TextBox>
                                </td>
                                </tr>
                                <tr>
                                <td >
                                Operator
                                </td>
                                <td >
                                    <asp:DropDownList ID="ddlOperator" runat="server">
                                    </asp:DropDownList>
                                </td>
                                </tr>
                                <tr>
                                <td >
                                PurChase Order No
                                </td>
                                    
                                <td>
                                <asp:TextBox ID="txtPurChaseOrderNo" runat="server"></asp:TextBox>
                                </td>
                                </tr>
                                <tr>
                                <td >
                              PurChase Invoice No
                                </td>
                                <td>
                                <asp:TextBox ID="txtPurChaseInvoiceNo" runat="server"></asp:TextBox>
                                </td>
                                </tr>
                                </table>
                                </asp:View>
                                
                                    </asp:MultiView></td>
                        </tr>
                        <tr>
                        <td>
                            <asp:Button ID="btnFindAcconut" runat="server" Text="FindAccount" />
                            <asp:Button ID="btnNewSearch" runat="server" Text="NewSearch" />
                            <asp:TextBox ID="txtShortName" runat="server" 
                  Visible="True"></asp:TextBox>
                  <input id="hfAccountKey" runat="server" 
                        type="hidden" />
                        </td>
                      
                        </tr>
                        <tr><td>
                              <asp:GridView ID="gvFindAccount" runat="server" CellPadding="4" ForeColor="#333333"
                                GridLines="None" Width="528px"  >
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
                            </asp:GridView></td>
                        </tr>
                    </table>
                    <input id="btnOK" type="button" value="OK" onclick = "Javascript:passresult()" /></td>
            </tr>
        </table>
    </form>
</body>
</html>
