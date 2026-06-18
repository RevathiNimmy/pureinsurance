<%@ Page Language="VB" AutoEventWireup="false" CodeFile="GetHeaderandSummaries.aspx.vb" Title="GetHeaderandSummaries" Inherits="MTC_GetHeaderandSummaries" %>

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
            &nbsp;<asp:Label ID="Label2" runat="server" Width="60px"></asp:Label>
            <asp:Label ID="Label1" runat="server" BorderStyle="Double" Font-Bold="True" Font-Italic="True"
                ForeColor="#C000C0" Text="Policy Header Details" Width="134px"></asp:Label><br />
            <br />
            InsuranceFileKey:
            <asp:TextBox ID="txtInsuranceFilelKey" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtInsuranceFilelKey"
                ErrorMessage="* Required Field"></asp:RequiredFieldValidator><br />
            &nbsp;
            <br />            
            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<asp:Button
                ID="btnGetPolicyHeaderDetails" runat="server" Text="GetPolicyHeaderDetails"
                Width="154px" />&nbsp;
            <asp:Button ID="btnNextScreen" runat="server" ForeColor="#C000C0" Text="Next Screen" /><br />
            <hr />
            &nbsp;
            <asp:Label ID="lblOutput" runat="server" Width="184px"></asp:Label>
            <br />        
            <asp:Label ID="Label9" runat="server" Text="CoverEndDate"></asp:Label>
            &nbsp;
            <asp:Label ID="Label10" runat="server"></asp:Label><br />
         
            <asp:Label ID="Label5" runat="server" Text="CoverStartDate"></asp:Label>&nbsp;&nbsp;
            <asp:Label ID="Label11" runat="server"></asp:Label><br />
            <asp:Label ID="Label6" runat="server" Text="Description"></asp:Label>&nbsp;&nbsp;
            <asp:Label ID="Label12" runat="server"></asp:Label><br />   
            <asp:Label ID="Label7" runat="server" Text="InceptionDate "></asp:Label>&nbsp;&nbsp;
            <asp:Label ID="Label13" runat="server"></asp:Label>&nbsp;
            <br />
            <br />
            <asp:Label ID="Label8" runat="server" Text="InsuranceFileRef"></asp:Label>&nbsp;&nbsp;
            <asp:Label ID="Label14" runat="server"></asp:Label><br />   
            <asp:Label ID="Label3" runat="server" Text="InsuranceFileStatusCode"></asp:Label>&nbsp;&nbsp;
            
            <asp:Label ID="Label15" runat="server"></asp:Label><br />
            <asp:Label ID="Label4" runat="server" Text="InsuranceFileTypeCode"></asp:Label>&nbsp;&nbsp;
            <asp:Label ID="Label16" runat="server"></asp:Label><br />
            <asp:Label ID="Label17" runat="server" Text="InsuranceFileVersion"></asp:Label>
            &nbsp;
            <asp:Label ID="Label18" runat="server"></asp:Label><br />
            <br />
         
            <asp:Label ID="Label19" runat="server" Text="InsuranceFolderKey"></asp:Label>&nbsp;&nbsp;
            <asp:Label ID="Label20" runat="server"></asp:Label><br />
            <asp:Label ID="Label21" runat="server" Text="PartyKey"></asp:Label>&nbsp;&nbsp;
            <asp:Label ID="Label22" runat="server"></asp:Label><br />   
            <asp:Label ID="Label23" runat="server" Text="PaymentMethodCode"></asp:Label>&nbsp;&nbsp;
            <asp:Label ID="Label24" runat="server"></asp:Label>&nbsp;
            <br />
            <asp:Label ID="Label25" runat="server" Text="ProductCode"></asp:Label>&nbsp;&nbsp;
            <asp:Label ID="Label26" runat="server"></asp:Label><br />
            <br />   
            <asp:Label ID="Label27" runat="server" Text="QuoteExpiryDate"></asp:Label>&nbsp;&nbsp;
            
            <asp:Label ID="Label28" runat="server"></asp:Label><br />
            <asp:Label ID="Label29" runat="server" Text="QuoteIsLocked"></asp:Label>&nbsp;&nbsp;
            <asp:Label ID="Label30" runat="server"></asp:Label><br /> 
             <asp:Label ID="Label31" runat="server" Text="QuoteTimeStamp" Visible="False"></asp:Label>
            &nbsp;
            <asp:Label ID="Label32" runat="server"></asp:Label><br />
            <br />
         
            <asp:Label ID="Label33" runat="server" Text="SubBranchCode"></asp:Label>&nbsp;&nbsp;
            <asp:Label ID="Label34" runat="server"></asp:Label><br />
            <asp:Label ID="Label35" runat="server" Text="ConsolidatedLeadAgentCommission"></asp:Label>&nbsp;&nbsp;
            <asp:Label ID="Label36" runat="server"></asp:Label><br />   
            <asp:Label ID="Label37" runat="server" Text="ConsolidatedSubAgentCommission"></asp:Label>&nbsp;&nbsp;
            <asp:Label ID="Label38" runat="server"></asp:Label>&nbsp;
            <br />
            <asp:Label ID="Label39" runat="server" Text="PolicyLevelTaxesAndFees" Visible="False"></asp:Label>&nbsp;&nbsp;
            <asp:Label ID="Label40" runat="server" Visible="False"></asp:Label>
            &nbsp;&nbsp;<br />
            <br />
            <asp:Label ID="Label41" runat="server" Text="Insued Parties Details"></asp:Label><br />
            <br />   
                <asp:GridView ID="PHDOutput" runat="server" BackColor="#FFC0FF" CellSpacing="1">
                <HeaderStyle BackColor="White" BorderColor="White" Font-Bold="False" Font-Italic="False" />
            </asp:GridView>
            <br />
            <asp:Label ID="Label42" runat="server" Text="Risk Details"></asp:Label><br />
            <br />
            <asp:GridView ID="PHDOUTPUT1" runat="server" BackColor="#FFC0FF" CellSpacing="1">
                <HeaderStyle BackColor="White" BorderColor="White" Font-Bold="False" Font-Italic="False" />
            </asp:GridView></div>
    
    </div>
    </td>
    </tr>
    <tr>
    <td>
        <uc2:Footer ID="Footer1" runat="server" />
    
    </td>
    </tr>
    </table>
    </form>
</body>
</html>
