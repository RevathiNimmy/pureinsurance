<%@ Page Language="VB" AutoEventWireup="false" CodeFile="wfrmFindInsuranceFile.aspx.vb" Inherits="Lookup_Screens_FindInsuranceFile" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <script language="javascript" type ="text/javascript">
    
    function passresult()
    {
    if (opener != null)
    {
    opener.document.getElementById("txtInsuranceRef").value = document.getElementById("txtInsuranceRef").value;
    self.close();
    }
    else
    {      
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

                        <asp:View ID="Policy" runat="server">
                            <table style="width: 376px">
                                <tr>
                                    <td style="width: 248px">
                                        Reference : &nbsp; &nbsp;
                                        <asp:TextBox ID="txtReference" runat="server"></asp:TextBox><br />
                                        Risk: Index &nbsp; &nbsp;
                                        <asp:TextBox ID="txtRiskIndex" runat="server"></asp:TextBox></td>
                                    <td>
                                        <asp:RadioButtonList ID="rblPolicyType" runat="server">
                                            <asp:ListItem Value="ALL">All Types</asp:ListItem>
                                            <asp:ListItem Value="0">NB Quote</asp:ListItem>
                                            <asp:ListItem Value="1">MTA Quote</asp:ListItem>
                                            <asp:ListItem Value="2">Policy</asp:ListItem>
                                            <asp:ListItem Value="3">Renewal</asp:ListItem>
                                        </asp:RadioButtonList></td>
                                </tr>
                            </table>
                        </asp:View>
                        <asp:View ID="RelatedClient" runat="server">
                            Short Name &nbsp;
                            <asp:TextBox ID="txtShortName" runat="server"></asp:TextBox></asp:View>
                    </asp:MultiView></td>
                <td style="width: 91px">
                    
                    <asp:Button ID="btnNewSearch" runat="server" Text="New Search" CausesValidation="False" /></td>
            </tr>
            <tr>
                <td colspan="2">
                <asp:Panel ID="pnlSearchResult" runat="server" Height="300px" ScrollBars="Auto" Width="450px">
                <asp:Button ID="btnFind" runat="server" Text="Find Now" CausesValidation="False" />
                <asp:GridView ID="gvSearchResult" runat="server" Width="528px" AutoGenerateColumns="False" DataKeyNames="InsuranceFileKey,InsuranceRef">
                        <Columns>
                            <asp:CommandField ShowSelectButton="True" />
                            <asp:BoundField DataField="InsuranceRef" HeaderText="Folder Reference" />
                            <asp:BoundField DataField="ProductCode" HeaderText="Product" />
                            <asp:BoundField DataField="InsuranceFileType" HeaderText="Status" />
                            <asp:BoundField DataField="ClientName" HeaderText="Insurance Holder" />
                            <asp:BoundField DataField="LastModifiedDate" HeaderText="DateModified" />
                            <asp:BoundField DataField="InsuranceFileKey" HeaderText="InsuranceFileKey" Visible="False" />
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
                    
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnOK" runat="server" Text="OK"  Width="75" OnClientClick="Javascript:passresult()" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="False" />
                    <asp:TextBox ID="txtInsuranceRef" runat="server" Visible="True"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvInsuranceRef" runat="server" ControlToValidate="txtInsuranceRef"
                        ErrorMessage="*"></asp:RequiredFieldValidator></td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
