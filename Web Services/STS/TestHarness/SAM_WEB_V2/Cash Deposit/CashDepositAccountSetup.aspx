<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CashDepositAccountSetup.aspx.vb"
    Inherits="CashDeposit_CashDepositAccountSetup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cash Deposit Account Setup</title>

    <script language="javascript" type="text/javascript">
    
    function LoadWindows(url)
    {
    window.open(url,"","width=600" ,"height=1000","scrollbars=1")
       }
    
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="lblSamErrorMessage" runat="server" ForeColor="Red"></asp:Label>
            <table style="width: 648px">
                <tr>
                    <td colspan="4">
                        <strong>Cash Deposit Account Setup</strong></td>
                </tr>
                <tr>
                    <td style="width: 103px">
                        CD Number</td>
                    <td style="width: 111px">
                        <asp:TextBox ID="txtCDNumber" runat="server"></asp:TextBox></td>
                    <td colspan="2" rowspan="1" align="right">
                        &nbsp;<asp:CheckBox ID="ChkSinglePolicyLock" runat="server" Text="Single Policy Lock"
                            TextAlign="Left" /></td>
                </tr>
            </table>
            <br />
        </div>
        <div>
            <strong><span style="color: #ffffff; background-color: #336699">SELECT PRODUCTS</span></strong>
            <table>
                <tr>
                    <td style="width: 199px; height: 218px">
                        <asp:ListBox ID="lstAllProducts" runat="server" Height="221px" Width="194px"></asp:ListBox></td>
                    <td style="width: 156px; height: 218px">
                        &nbsp; &nbsp; &nbsp;
                        <asp:Button ID="btnSingleAddProd" runat="server" Style="z-index: 100; left: 276px;
                            position: absolute; top: 152px" Text=">" Width="41px" />
                        <asp:Button ID="BtnRemoveTaskProd" runat="server" Style="z-index: 102; left: 274px;
                            position: absolute; top: 209px" Text="<" Width="41px" />
                        <asp:Button ID="BtnAddAllProducts" runat="server" Style="z-index: 100; left: 274px;
                            position: absolute; top: 258px" Text=">>" Width="41px" />
                        <asp:Button ID="BtnRemoveAllProducts" runat="server" Style="z-index: 100; left: 275px;
                            position: absolute; top: 314px" Text="<<" Width="41px" />
                    </td>
                    <td style="width: 200px; height: 218px">
                        <asp:ListBox ID="lstSelectedProducts" runat="server" Height="230px" Width="208px">
                            <asp:ListItem Value="2">5</asp:ListItem>
                        </asp:ListBox></td>
                </tr>
            </table>
        </div>
        <div>
            <strong><span style="color: #ffffff; background-color: #336699">SELECT BRANCHES</span></strong>
            <table>
                <tr>
                    <td style="width: 199px; height: 218px">
                        <asp:ListBox ID="lstAllBranches" runat="server" Height="221px" Width="194px"></asp:ListBox></td>
                    <td style="width: 156px; height: 218px">
                        &nbsp; &nbsp; &nbsp;
                        <asp:Button ID="btnSingleAddBranch" runat="server" Style="z-index: 100; left: 275px;
                            position: absolute; top: 399px" Text=">" Width="41px" OnClick="Button2_Click" />
                        <asp:Button ID="BtnRemoveTaskBranch" runat="server" Style="z-index: 102; left: 274px;
                            position: absolute; top: 456px" Text="<" Width="41px" />
                        <asp:Button ID="BtnAddAllBranches" runat="server" Style="z-index: 100; left: 275px;
                            position: absolute; top: 514px" Text=">>" Width="41px" />
                        <asp:Button ID="BtnRemoveAllBranches" runat="server" Style="z-index: 100; left: 276px;
                            position: absolute; top: 569px" Text="<<" Width="41px" />
                    </td>
                    <td style="width: 200px; height: 218px">
                        <asp:ListBox ID="lstSelectedBranches" runat="server" Height="230px" Width="208px"></asp:ListBox></td>
                </tr>
            </table>
        </div>
        <asp:HiddenField ID="hfAgentKey" runat="server" />
        <asp:Button ID="BtnApply" runat="server" Text="Apply" />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
    </form>
</body>
</html>
