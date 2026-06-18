<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FindCashDepositAccount.aspx.vb"
    Inherits="FindCashDepositAccount" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Find Cash Deposit Account</title>
      <script language="javascript"  type ="text/javascript">
    
    function Trim(str)
            {  while(str.charAt(0) == (' ') )
              {  str = str.substring(1);
              }
              while(str.charAt(str.length-1) == ' ' )
              {  str = str.substring(0,str.length-1);
              }
              return str;
            }
    
    function LoadWindows(url)
    {
   //window.open(url,"","width=600,height=1000,scrollbars=true")
    window.open(url, null, 'width=600,height=500,scrollbars=yes');
 
       }
    
    function OnClientKeyTextChanged()
    {   
      if(Trim(document.getElementById('txtClient').value) != '')
        {
            document.getElementById('txtAgent').value = '';
            document.getElementById('BtnAdd').disabled = false;
            document.getElementById('btnFind').disabled = false;
        }
      if(Trim(document.getElementById('txtClient').value) == '' && Trim(document.getElementById('txtAgent').value) == '') 
        {
            document.getElementById('BtnAdd').disabled = true;
            document.getElementById('btnFind').disabled = true;
        }
    }
    
    function OnAgentKeyTextChanged()
    { 
      if(Trim(document.getElementById('txtAgent').value) != '')
        {
            document.getElementById('txtClient').value = '';
            document.getElementById('BtnAdd').disabled = false;
            document.getElementById('btnFind').disabled = false;
        }
      if(Trim(document.getElementById('txtClient').value) == '' && Trim(document.getElementById('txtAgent').value) == '') 
        {
            document.getElementById('BtnAdd').disabled = true;
            document.getElementById('btnFind').disabled = true;
        }   
    }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div style="text-align: left">
                <table cellpadding="5" cellspacing="0" border="1">
                    <tr><td style="width: 100px; white-space: nowrap;">
                        <strong><span style="font-family: Palatino Linotype; white-space: nowrap">Client Key</span></strong></td>
                        <td style="width: 100px; ">
                            <asp:TextBox ID="txtClient" runat="server" onkeyup="OnClientKeyTextChanged();"></asp:TextBox>
                            <asp:Button ID="btnFindClient" runat="server"
                                Text="..." Width="24px" OnClientClick='LoadWindows("FindParty.aspx")'/></td>
                               <td style="width: 100px; white-space: nowrap;">
                            <strong><span style=" font-family: Palatino Linotype; white-space: nowrap">Cash Deposit
                                Number</span></strong></td>
                        <td style="width: 100px;">
                            <asp:TextBox ID="txtCashDepositNumber" runat="server"></asp:TextBox></td>
                             <td style="width: 100px; white-space: nowrap; ">
                            <asp:Button ID="btnFind" runat="server" Text="Find Now" /></td>
                       
                       
                    </tr>
                    <tr>
                       
                    </tr>
                    <tr>
                        <td style="width: 100px; white-space: nowrap;">
                            <strong><span style="font-family: Palatino Linotype; white-space: nowrap">Agent Key</span></strong></td>
                        <td style="width: 100px; ">
                            <asp:TextBox ID="txtAgent" runat="server" onkeyup="OnAgentKeyTextChanged();"></asp:TextBox>
                            <asp:Button ID="btnFindAgent" runat="server"
                                Text="..." Width="24px" OnClientClick='LoadWindows("FindAgent.aspx")'/></td>
                        <td style="width: 100px; white-space: nowrap; ">
                            <strong><span style=" font-family: Palatino Linotype">Bank Name</span></strong></td>
                        <td style="width: 100px; ">
                            <asp:DropDownList ID="ddlBankName" runat="server">
                            </asp:DropDownList></td>
                                                  
                             <td style="width: 100px; white-space: nowrap; ">
                            <asp:Button ID="BtnNewsearch" runat="server" Text="New search" /></td>
                       
                    </tr>
                    <tr>
                       
                    </tr>
                    <tr>
                     
                    </tr>
                    <tr>
                       
                    </tr>
                    <tr>
                       
                    </tr>
                </table>
              
                   <asp:GridView ID="gvFindCashDeposit" runat="server" CellPadding="4"
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
                <asp:HiddenField ID="hfAgentKey" runat="server" /><asp:HiddenField ID="hfClientKey" runat="server" />
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
