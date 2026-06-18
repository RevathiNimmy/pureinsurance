<%@ Page Language="VB" MaintainScrollPositionOnPostback="true" AutoEventWireup="false" CodeFile="SelectClauses.aspx.vb" Inherits="New_Business_SelectProduct" %>

<%@ Register Src="../UserControl/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>

<%@ Register Src="../UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script type="text/javascript" language="javascript">
    function passresult()
    {
    
    window.opener.__doPostBack("","")
     self.close();

   }
    </script>
    <title>Untitled Page</title>
    <link href="../App_Themes/GlobalTheme/GlobalStyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/GlobalTheme/GlobalStyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../Images/loginstylesheet.css" rel="stylesheet" type="text/css" />
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
            <tr style="height: 90%">
                <td>
                    <asp:Label ID="Label1" runat="server" BorderStyle="Double" Font-Bold="True" Font-Italic="True"
                        ForeColor="#C000C0" Text="Select Clauses" Width="160px"></asp:Label>
                    
                    <br />
                    <asp:Label ID="lblSamErrorMessage" runat="server" ForeColor="Red"></asp:Label><br />
                    &nbsp; &nbsp;<br />
                    &nbsp;
                    
                    <br />
                    <table style="width: 430px; height: 244px">
                        <tr>
                            <td style="width: 256px">
                                <asp:ListBox ID="lstAvailableClause" runat="server" Height="224px" Width="256px"></asp:ListBox></td>
                            <td>
                                <asp:Button ID="Button2" runat="server" Text=">>" Width="54px" /><br />
                                <br />
                                <br />
                                <br />
                                <br />
                                <asp:Button ID="Button1" runat="server" Text="<<" Width="54px" /></td>
                            <td>
                                <asp:ListBox ID="lstSelectedClause" runat="server" Height="224px" Width="256px"></asp:ListBox></td>
                        </tr>
                    </table>
                    &nbsp;
                    <asp:Button ID="Button3" runat="server" OnClientClick="passresult()" Text="Ok" Width="88px" /></td>
            </tr>
            <tr style="height:110px">
            <td></td>
            </tr>
            <tr><td>
                <uc2:Footer ID="Footer1" runat="server" />
            </td> </tr>
        </table>
        <%--  <div>
        <asp:Label ID="Label1" runat="server" BorderStyle="Double" Font-Bold="True" Font-Italic="True"
            ForeColor="#C000C0" Text="Select Product" Width="160px"></asp:Label>
        <br />
        <br />
        <asp:Label ID="lblSamErrorMessage" runat="server" ForeColor="Red"></asp:Label><br />
        <br />
        <br />
        <asp:ListBox ID="lstProducts" runat="server" Height="224px" Width="256px"></asp:ListBox>
        <br />
        <br />
        <br />
        Select Branch : &nbsp; &nbsp; &nbsp;&nbsp;
        <asp:DropDownList ID="ddlBranch" runat="server" Width="152px">
        </asp:DropDownList>
        <br />
        <br />
        <br />
        <asp:Button ID="btnOk" runat="server" Text="Ok" Width="72px" /></div>
        --%>
    </form>
</body>
</html>
