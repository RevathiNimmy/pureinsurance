<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AmendClient-AccEcecutive.aspx.vb"
    Inherits="Lookup_Screens_FindAgent" %>

<%@ Register Src="../UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Find Agent</title>

    <script language="javascript" type="text/javascript">
    
    function passresult()
    {
        if (opener != null)
        {
            if (document.getElementById("txtShortName") != null && document.getElementById("hfAccExecutiveCode") != null)
            {
                opener.document.getElementById("txtAccExecutiveCode").value = document.getElementById("hfAccExecutiveCode").value;
                opener.document.getElementById("txtAccExecutiveName").value = document.getElementById("txtShortName").value;
                opener.document.getElementById("hdAccExecutiveCode").value = document.getElementById("hfAccExecutiveCode").value;
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
                        <div>
                            &nbsp;<table style="width: 536px">
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="width: 419px">
                                        <table style="width: 320px">
                                            <tr>
                                                <td style="width: 99px">
                                                    Account Executive Code</td>
                                                <td>
                                                    <asp:TextBox ID="txtAccExecutiveCode" runat="server"></asp:TextBox></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 99px; height: 20px">
                                                    Name</td>
                                                <td style="height: 20px">
                                                    <asp:TextBox ID="txtName" runat="server"></asp:TextBox></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 99px">
                                                    Type</td>
                                                <td>
                                                    <asp:DropDownList ID="ddlType" runat="server">
                                                        <asp:ListItem>AccountExecutive</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:CheckBox ID="chkClosedBranches" runat="server" Text="Include Closed Branches" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="width: 91px">
                                        &nbsp;<asp:Button ID="btnNewSearch" runat="server" Text="New Search" /></td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <hr />
                                        &nbsp;
                                        <asp:Button ID="btnFind" runat="server" Text="Find Now" />
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
                                        <input id="btnOk" type="button" value="Ok" onclick="javascript:passresult()" />
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
                                        <asp:TextBox ID="txtShortName" runat="server" Visible="True"></asp:TextBox>
                                        <asp:HiddenField ID="hfAccExecutiveCode" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
