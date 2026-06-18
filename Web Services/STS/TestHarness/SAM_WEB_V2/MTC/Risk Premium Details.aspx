<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Risk Premium Details.aspx.vb" Inherits="MTC_Risk_Premium_Details" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div>
            &nbsp;<asp:Label ID="Label2" runat="server" Width="60px"></asp:Label>
            <asp:Label ID="Label1" runat="server" BorderStyle="Double" Font-Bold="True" Font-Italic="True"
                ForeColor="#C000C0" Text="Risk Premium Details" Width="134px"></asp:Label><br />
            <br />
            InsuranceFileKey:
            <asp:TextBox ID="txtInsuranceFileKey" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtInsuranceFileKey"
                ErrorMessage="* Required Field"></asp:RequiredFieldValidator><br />
            &nbsp;
            <br />
            RiskKey &nbsp; &nbsp;&nbsp; :&nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
            <asp:TextBox ID="txtRiskKey" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtRiskKey"
                ErrorMessage="* Required Field"></asp:RequiredFieldValidator><br />
            &nbsp;<br />
            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<asp:Button
                ID="btnRiskPremiumDetails" runat="server" Text="Risk Premium Details" Width="154px" />&nbsp;
            <asp:Button ID="btnnextscreen" runat="server" ForeColor="#C000C0" Text="Next Screen" /><br />
            <hr />
            &nbsp;
            <asp:Label ID="lblOutput" runat="server" Width="184px"></asp:Label>
            <asp:GridView ID="GRDOutput" runat="server" BackColor="#FFC0FF" CellSpacing="1">
                <HeaderStyle BackColor="White" BorderColor="White" Font-Bold="False" Font-Italic="False" />
            </asp:GridView>
        </div>
    
    </div>
    </form>
</body>
</html>
