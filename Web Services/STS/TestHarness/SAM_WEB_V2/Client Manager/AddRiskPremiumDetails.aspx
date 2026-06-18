<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddRiskPremiumDetails.aspx.vb" Title="Add Risk Premium Details" Inherits="MTC_AddRiskPremiumDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Label ID="Label2" runat="server" Width="60px"></asp:Label>
            <asp:Label ID="Label1" runat="server" BorderStyle="Double" Font-Bold="True" Font-Italic="True"
                ForeColor="#C000C0" Text="Add Risk Premium" Width="150px"></asp:Label><br />
            <br />
            <tr>
            <td>
             <asp:Label ID="Label3" runat="server" Width="150px" Font-Bold="True" Font-Italic="True"
                ForeColor="#C000C0"></asp:Label>
            </td>
            </tr>
        <br />
        <br />
            <table>
             <tr>
                    <td>
                        BranchCode</td>
                    <td>
                        <asp:TextBox ID="txtBranchCode" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>
                        InsuranceFileKey</td>
                    <td>
                        <asp:TextBox ID="txtInsuranceFileKey" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>
                        Risk Key</td>
                    <td>
                        <asp:TextBox ID="txtRiskKey" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>
                        TumeStamp</td>
                    <td>
                        <asp:TextBox ID="txtTimeStamp" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>
                        RatingSectionTypeId</td>
                    <td>
                        <asp:DropDownList ID="ddratingsection" runat="server">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td>
                        EarningPatternId</td>
                    <td>
                        <asp:DropDownList ID="ddearningpattern" runat="server">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td>
                        RateTypeId</td>
                    <td>
                        <asp:DropDownList ID="ddratetype" runat="server">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td>
                        Annual Rate</td>
                    <td>
                        <asp:TextBox ID="txtAnnualRate" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>
                        Sum Insured</td>
                    <td>
                        <asp:TextBox ID="txtSumInsured" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>
                        Annual Premium</td>
                    <td>
                        <asp:TextBox ID="txtAnnualPremium" runat="server"></asp:TextBox></td>
                </tr>
                 <tr>
                    <td>
                        This Premium</td>
                    <td>
                        <asp:TextBox ID="txtThisPremium" runat="server"></asp:TextBox></td>
                </tr>
                 <tr>
                    <td>
                        CountryId</td>
                    <td>
                        <asp:DropDownList ID="ddcountry" runat="server">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td style="height: 24px">
                        StateId</td>
                    <td style="height: 24px">
                        <asp:DropDownList ID="ddstate" runat="server">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td>
                        Override Reason</td>
                    <td>
                        <asp:TextBox ID="txtOverrideReason" runat="server"></asp:TextBox></td>
                </tr>
                
                  
            </table>
            <asp:Button ID="btnAddRiskPremium" runat="server" Text="Add New" />
        <asp:Button ID="Button1" runat="server" Text="Back" /></div>
    </form>
</body>
</html>
