<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CashDepositAccount.aspx.vb" Inherits="CashDepositAccount" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cash Deposit Account</title>
      <script language="javascript"  type ="text/javascript">
    
    function LoadWindows(url)
    {
   //window.open(url,"","width=600,height=1000,scrollbars=true")
    window.open(url, null, 'width=600,height=500,scrollbars=yes');
 
       }
       
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div style="text-align: left">
                <table cellpadding="5" cellspacing="0" border="1">
                    <tr><td style="width: 100px; white-space: nowrap;">
                        <strong><span style="font-family: Palatino Linotype; white-space: nowrap">Party code</span></strong></td>
                        <td style="width: 100px; ">
                            <asp:TextBox ID="txtPartyCode" runat="server" onkeyup="OnClientKeyTextChanged();" Enabled="False"></asp:TextBox>
                            </td>
                               <td style="width: 100px; white-space: nowrap;">
                            <strong><span style=" font-family: Palatino Linotype; white-space: nowrap">Party Name</span></strong></td>
                        <td style="width: 312px;">
                            <asp:TextBox ID="txtPartyName" runat="server" Enabled="False"></asp:TextBox></td>
                       
                       
                  
                </table>
              
                   <asp:GridView ID="gvCashDeposit" runat="server" CellPadding="4"
                        ForeColor="#333333" GridLines="None" Width="100%" AutoGenerateColumns="False">
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <Columns>
                           <asp:CommandField ShowSelectButton="True"  />
                            <asp:BoundField HeaderText="Bank Name" DataField="BankName" />
                            <asp:BoundField HeaderText="CD Number" DataField="CashDepositRef" />
                            <asp:BoundField HeaderText="Available Balance" DataField="AvailableBalance" />
                            <asp:BoundField HeaderText="Party" DataField="PartyName" />
                            <asp:BoundField HeaderText="Product"  DataField="Product" />
                            <asp:BoundField HeaderText="Branch" DataField="Branch" />
                        </Columns>
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <EditRowStyle BackColor="#999999" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    </asp:GridView>
                &nbsp;
                <br />
                <asp:Button ID="BtnAdd" runat="server" Text="Add" />
                <asp:Button ID="BtnEdit" runat="server" Text="Edit" />
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp;
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                &nbsp; &nbsp;
                <asp:Button ID="BtnClose" runat="server" Text="Close" /><br />
                <br />
            </div>
        </div>
    </form>
</body>
</html>
