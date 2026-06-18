<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CoInsurance.aspx.vb" Inherits="New_Business_CoInsurance" %>

<%@ Register Src="../UserControl/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>

<%@ Register Src="../UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link href="../App_Themes/GlobalTheme/GlobalStyleSheet.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
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
        &nbsp;<asp:CheckBox ID="chkIsRecovered" runat="server" Text="Is Recovered " />
        &nbsp; &nbsp; &nbsp;
        <asp:CheckBox ID="chkIsSurcharged" runat="server" Text="Is Surcharged" />
        &nbsp; &nbsp;&nbsp; % Allocated :<asp:Label ID="lblTotalShare" runat="server"></asp:Label>
        &nbsp; &nbsp;<asp:DropDownList ID="ddlDefaults" runat="server">
        </asp:DropDownList>
        <br />
        <br />
        <asp:GridView ID="gvCoinsuranceValues" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None">
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <Columns>
                <asp:CommandField ShowSelectButton="True" />
                <asp:BoundField DataField="CoInsurer" HeaderText="CoInsurer" />
                <asp:BoundField DataField="ArrangementRef" HeaderText="ArrangementRef" />
                <asp:BoundField DataField="SharePerc" HeaderText="Share %" />
                <asp:BoundField DataField="CommissionPerc" HeaderText="Commission %" />
            </Columns>
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <EditRowStyle BackColor="#999999" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
    
    </div>
        <asp:Button ID="btnOk" runat="server" Text="OK" Width="64px" />
        &nbsp;
        <asp:Button ID="btnNew" runat="server" Text="New" />&nbsp;
        <asp:Button ID="btnEdit" runat="server" Text="Edit" /><br />
        <br />
        <asp:Label ID="lblErrorMessage" runat="server"></asp:Label><br />
        <br />
        <br />
        <asp:Panel ID="pnlCoinsurance" runat="server" Height="176px" Width="368px" Visible="False">
            <br />
            <table >
                <tr>
                    <td >
                <asp:Button ID="btnCoInsurer" runat="server" Text="Coinsurer"  />
                    </td>
                    <td >
                   <asp:TextBox ID="txtCoinsurer" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 102px">
                        Arrangement Ref</td>
                    <td style="width: 100px">
            <asp:TextBox ID="txtArrangementRef" runat="server" CausesValidation="True" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 102px">
                        <strong>Share % </strong>
                    </td>
                    <td style="width: 100px">
                    <asp:TextBox ID="txtShare" runat="server" ></asp:TextBox>
                          <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
                Font-Bold="True"  ControlToValidate="txtShare" 
                ></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td >
                     Commission % 
                    </td>
                    <td >
                    <asp:TextBox ID="txtCommission" runat="server">
                    </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                    <asp:Button ID="btnnewOk" runat="server" Text="Ok" Width="56px"  />
                     <asp:Button ID="btnNewCancel" runat="server" Text="Cancel" CausesValidation="False"  />
                    </td>
                </tr>
            </table>
         
      
          
        </asp:Panel>
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowSummary="False"
                         />
            </td>
            </tr>
            <tr>
            <td>
                <uc2:Footer ID="Footer1" runat="server" />
            
            </td></tr>
            </table>
    </form>
</body>
</html>
