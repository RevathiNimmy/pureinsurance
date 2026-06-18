<%@ Page Language="VB" AutoEventWireup="false" MaintainScrollPositionOnPostback="true" CodeFile="FindDocumentTemplates.aspx.vb" Inherits="Lookup_Screens_FindDocumentTemplates" %>

<%@ Register Src="../UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    
    <script type="text/javascript" language="javascript">
    function passresult()
    {
    
    window.opener.__doPostBack("","")
     self.close();

   }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <table width="100%" style="height: 100%" border="0" cellspacing="0" cellpadding="0">
            <tr style="height: 10%">
                <td style="width: 10%" align="right">
                    <table width="100%">
                        <tr>
                            <td>
                               
                               
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
        <table>
            <tr>
                <td style="width: 100px">
        Code :</td>
                <td style="width: 100px">
        <asp:TextBox ID="txtCode" runat="server"></asp:TextBox></td>
                <td style="width: 100px">
                    Effective Date:</td>
                <td style="width: 100px">
        <asp:TextBox ID="txtEffectiveDate" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 100px">
        Type: &nbsp; &nbsp;</td>
                <td style="width: 100px">
        <asp:DropDownList ID="ddlType" runat="server" Width="120px">
        </asp:DropDownList></td>
                <td style="width: 100px">
                    <asp:Button ID="btnFindNow" runat="server" Text="Find Now" /></td>
                <td style="width: 100px">
                </td>
            </tr>
        </table>
        
        <hr />
    
    </div>
        <asp:GridView ID="gvDoctemplate" runat="server" CellPadding="4" ForeColor="#333333"
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
        <hr />
        <asp:Button ID="btnOk" runat="server" Text="Ok" Width="88px" OnClientClick="passresult()" />
        </td>
        </tr>
        </table>
    </form>
</body>
</html>
