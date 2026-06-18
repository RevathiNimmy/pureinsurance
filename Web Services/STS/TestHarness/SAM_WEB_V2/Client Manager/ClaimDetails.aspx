<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ClaimDetails.aspx.vb" Inherits="View_Claim_FindClaim" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Find Claim</title>

    <script language="javascript" type="text/javascript">
    
    function LoadWindows(url,width,height)
    {
    window.open(url,"","width=" + width+ ",height="+ height+",scrollbars=1")
    
    }
    
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <span style="color: #ffffff; background-color: #009999">CLAIM DETAILS</span><br />
            <br />
            
                    <asp:GridView ID="gvResult" runat="server" DataKeyNames="ClaimKey" AutoGenerateColumns="False"
                        CellPadding="4" ForeColor="#333333" GridLines="None">
                        <Columns>
                            <asp:CommandField ShowSelectButton="True" />
                            <asp:BoundField DataField="ClaimNumber" HeaderText="Claim Reference" />
                            <asp:BoundField DataField="InsuranceRef" HeaderText="Policy Reference" />
                            <asp:BoundField DataField="ClientShortName" HeaderText="Client" />
                            <asp:BoundField DataField="ProductDescription" HeaderText="Product Description" />
                            <asp:BoundField DataField="LossDateFrom" HeaderText="Loss Date" />
                        </Columns>
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#F7F6F3" Font-Size="Smaller" ForeColor="#333333" />
                        <EditRowStyle BackColor="#999999" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" Font-Size="Smaller" ForeColor="White" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    </asp:GridView>
            &nbsp;<td colspan="4" style="height: 13px">
                   <%-- <hr />--%>
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" /></td>
           
        &nbsp;
           
        </div>
    </form>
</body>
</html>
