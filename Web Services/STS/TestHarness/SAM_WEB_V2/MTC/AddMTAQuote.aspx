<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddMTAQuote.aspx.vb" Inherits="MTC_AddMTAQuote" %>

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
                ForeColor="#C000C0" Text="Add MTA Quote" Width="124px"></asp:Label><br />
            <br />
            <table>
                <tr>
                    <td>
                        Insured Name</td>
                    <td>
                        <asp:TextBox ID="txtInsuredName" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>
                        PolicyKey</td>
                    <td>
                        <asp:TextBox ID="txtPolicyKey" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>
                        Regarding</td>
                    <td>
                        <asp:TextBox ID="txtRegarding" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>
                        AlternateReference</td>
                    <td>
                        <asp:TextBox ID="txtAlternateReference" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>
                        PolicyStatusCode</td>
                    <td>
                        <asp:TextBox ID="txtPolicyStatusCode" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>
                        AnalysisCode</td>
                    <td>
                        <asp:TextBox ID="txtAnalysisCode" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>
                        BusinessTypeCode</td>
                    <td>
                        <asp:TextBox ID="txtBusinessTypeCode" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>
                        IssueDate</td>
                    <td>
                        <asp:TextBox ID="txtIssueDate" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>
                        ProposalDate</td>
                    <td>
                        <asp:TextBox ID="txtProposalDate" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>
                        FrequencyCode</td>
                    <td>
                        <asp:TextBox ID="txtFrequencyCode" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>
                        LTUExpiryDate</td>
                    <td>
                        <asp:TextBox ID="txtLTUExpiryDate" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>
                        StopReasonCode</td>
                    <td>
                        <asp:TextBox ID="txtStopReasonCode" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>
                        RenewalMethodCode</td>
                    <td>
                        <asp:TextBox ID="txtRenewalMethodCode" runat="server"></asp:TextBox></td>
                </tr>
                  <tr>
                    <td>
                        LapseCancelReasonCode</td>
                    <td>
                        <asp:TextBox ID="txtLapseCancelReasonCode" runat="server"></asp:TextBox></td>
                </tr>
                  <tr>
                    <td>
                        LapseCancelDate</td>
                    <td>
                        <asp:TextBox ID="txtLapseCancelDate" runat="server"></asp:TextBox></td>
                </tr>
                  <tr>
                    <td>
                        ReferredAtRenewal</td>
                    <td>
                        <asp:TextBox ID="txtReferredAtRenewal" runat="server"></asp:TextBox></td>
                </tr>
                  <tr>
                    <td>
                        ReferredOnMTA</td>
                    <td>
                        <asp:TextBox ID="txtReferredOnMTA" runat="server"></asp:TextBox></td>
                </tr>
                  <tr>
                    <td>
                        PolicyStyleCode</td>
                    <td>
                        <asp:TextBox ID="txtPolicyStyleCode" runat="server"></asp:TextBox></td>
                </tr>
                  
            </table>
            <asp:Button ID="btnAddMTAQuote" runat="server" Text="AddMTAQuote" />
            <asp:Button ID="btnNextScreen" runat="server" Enabled="False" ForeColor="#C000C0" Text="Next Screen" />
            <hr />
            <asp:Label ID="lblOutput" runat="server" Width="184px"></asp:Label><br />
            <asp:Label ID="Label3" runat="server" Text="InsuranceFileKey"></asp:Label>
            <asp:TextBox ID="txtInsuranceFileKey" runat="server"></asp:TextBox><br />
            <asp:GridView
                ID="aMTAOutput" runat="server" BackColor="#FFC0FF" CellSpacing="1">
                <HeaderStyle BackColor="White" BorderColor="White" Font-Bold="False" Font-Italic="False" />
            </asp:GridView>
            <br />
            &nbsp;</div>
    
    </div>
    </form>
</body>
</html>
