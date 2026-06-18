<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FindInsuranceFile.aspx.vb" Inherits="Lookup_Screens_FindInsuranceFile" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
     <script language="javascript" type ="text/javascript">
     function passresult()
    {
  
    if (opener != null)
    {
        
    if (opener.document.getElementById("txtPolicyCode") != null && opener.document.getElementById("hfinsurancekey") != null)
    {
    opener.document.getElementById("txtPolicyCode").value = document.getElementById("txtinsuranceref").value;
    opener.document.getElementById("hfinsurancekey").value = document.getElementById("hfinsurancekey").value;
    
    }
    else
    {
   
    }
     window.opener.__doPostBack("","")
    self.close();
   }

   }


    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style="width: 536px">
            <tr>
                <td colspan="2">
                    <asp:Menu ID="mnuInsuranceFile" runat="server" BackColor="#F7F6F3" DynamicHorizontalOffset="2"
                        Font-Names="Verdana" Font-Size="0.8em" ForeColor="#7C6F57" Orientation="Horizontal"
                        StaticSubMenuIndent="10px">
                        <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                        <DynamicHoverStyle BackColor="#7C6F57" ForeColor="White" />
                        <DynamicMenuStyle BackColor="#F7F6F3" />
                        <StaticSelectedStyle BackColor="#5D7B9D" />
                        <DynamicSelectedStyle BackColor="#5D7B9D" />
                        <DynamicMenuItemStyle HorizontalPadding="5px" ItemSpacing="5px" VerticalPadding="2px" />
                        <Items>
                            <asp:MenuItem Text="Policy" Value="0"></asp:MenuItem>
                            <asp:MenuItem Text="Related Client" Value="1"></asp:MenuItem>
                        </Items>
                        <StaticHoverStyle BackColor="#7C6F57" ForeColor="White" />
                    </asp:Menu>
                </td>
            </tr>
            <tr>
                <td style="width: 419px">
                    <asp:MultiView ID="mvFindInsuranceFile" runat="server">
                        &nbsp; &nbsp;
                        <asp:View ID="Policy" runat="server">
                            <table style="width: 376px">
                                <tr>
                                    <td style="width: 248px">
                                        Reference : &nbsp; &nbsp;
                                        <asp:TextBox ID="txtinsuranceref" runat="server"></asp:TextBox><br />
                                        Risk: Index &nbsp; &nbsp;
                                        <asp:TextBox ID="txtriskindex" runat="server"></asp:TextBox></td>
                                    <td>
                                        <asp:RadioButtonList ID="rdtype" runat="server">
                                            <asp:ListItem>All Types</asp:ListItem>
                                            <asp:ListItem>NB Quote</asp:ListItem>
                                            <asp:ListItem>MTA Quote</asp:ListItem>
                                            <asp:ListItem>Policy</asp:ListItem>
                                            <asp:ListItem>Renewal</asp:ListItem>
                                        </asp:RadioButtonList></td>
                                         <td style="width: 91px">
                    <br />
                    <asp:Button ID="btnOk" runat="server" Text="Ok" OnClientClick = "passresult()"/><asp:Button ID="btnCancel" runat="server" Text="Cancel" /><asp:Button ID="btnFind" runat="server" Text="Find Now" />
                    <asp:Button ID="btnNewSearch" runat="server" Text="New Search" /></td>
                                </tr>
                            </table>
                        </asp:View>
                        <asp:View ID="RelatedClient" runat="server">
                            Short Name &nbsp;
                            <asp:TextBox ID="txtshortname" runat="server"></asp:TextBox></asp:View>
                    </asp:MultiView></td>
               
            </tr>
            <tr>
                <td colspan="2">
                    <asp:GridView ID="gvSearchResult" runat="server" Width="528px">
                        <Columns>
                            <asp:CommandField ShowSelectButton="True" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="height: 26px">
                    &nbsp;
                </td>
            </tr>
        </table>
    
    </div>
        <asp:HiddenField ID="hfinsurancekey" runat="server" />
    </form>
</body>
</html>
