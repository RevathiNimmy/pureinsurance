<%@ Page Language="VB" AutoEventWireup="false" MaintainScrollPositionOnPostback="true"
    CodeFile="CashListItem.aspx.vb" Inherits="Claim_Payment_CashListItem" %>

<%@ Register Src="../UserControl/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <link href="../App_Themes/GlobalTheme/GlobalStyleSheet.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
    function Showmodalpopup()
    {
        
         var ReturnChildValue = window.showModalDialog("../CurrencyExchange/CurrencyExchange.aspx", window, "dialogWidth:700px;dialogHeight:400px");
         if (ReturnChildValue != null)
         {
         //alert(ReturnChildValue)
          document.getElementById("Cancel").value = ReturnChildValue;
          
         }
    }

    </script>
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
            <tr style="height: 90%">
                <td>
                    <asp:Label ID="lblSamErrorMessage" runat="server" ForeColor="Red"></asp:Label><br />
                    <asp:Menu ID="Menu1" runat="server" BackColor="#F7F6F3" DynamicHorizontalOffset="2"
                        Font-Names="Verdana" Font-Size="0.8em" ForeColor="#7C6F57" Orientation="Horizontal"
                        StaticSubMenuIndent="10px" Width="528px">
                        <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                        <DynamicHoverStyle BackColor="#7C6F57" ForeColor="White" />
                        <DynamicMenuStyle BackColor="#F7F6F3" />
                        <StaticSelectedStyle BackColor="#5D7B9D" />
                        <DynamicSelectedStyle BackColor="#5D7B9D" />
                        <DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                        <Items>
                            <asp:MenuItem Text="Details" Value="0"></asp:MenuItem>
                            <asp:MenuItem Text="Reciept" Value="1"></asp:MenuItem>
                            <asp:MenuItem Text="Address" Value="3"></asp:MenuItem>
                        </Items>
                        <StaticHoverStyle BackColor="#7C6F57" ForeColor="White" />
                    </asp:Menu>
                    <asp:MultiView ID="mvCashListItem" runat="server" ActiveViewIndex="0">
                        <asp:View ID="vDetails" runat="server">
                            <table width="100%">
                                <tr>
                                    <td style="width: 15%">
                                        <strong>Receipt Type</strong>
                                    </td>
                                    <td style="width: 25%">
                                        <asp:DropDownList ID="ddlPaymentType" runat="server">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlPaymentType"
                                            ErrorMessage="*" Font-Bold="True"></asp:RequiredFieldValidator></td>
                                    <td style="width: 12%">
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkProduceDocument" runat="server" Font-Bold="False" Text="Produce Document" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <strong>Transaction Information </strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <strong>Transaction date</strong>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTransactionDate" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtTransactionDate"
                                            ErrorMessage="*" Font-Bold="True"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <strong>Media Type</strong>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlMediaType" runat="server" Width="160px" AutoPostBack="True" />
                                    </td>
                                    <td style="width: 242px">
                                        Media reference
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMediaReference" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Their Reference
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTheirReference" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        Bank reference
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBankAccntName" runat="server" />&nbsp;
                                    </td>
                                    <td style="width: 242px">
                                        CollectionDate
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCollectionDate" runat="server"></asp:TextBox>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Our reference
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOurReference" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        Comments</td>
                                    <td>
                                        <asp:TextBox ID="txtComments" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        <strong>Posting Information</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Account
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAccount" runat="server">CLMPAYBLE</asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtAccount"
                                            ErrorMessage="*" Font-Bold="True"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        Allocation Status
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAllocationStatus" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <strong>Amount</strong></td>
                                    <td>
                                        <asp:TextBox ID="txtAmount" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtAmount"
                                            ErrorMessage="*" Font-Bold="True"></asp:RequiredFieldValidator></td>
                                </tr>
                            </table>
                            <%--<strong>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; </strong>
                    <asp:CheckBox ID="chkProduceDocument" runat="server" Font-Bold="False" Text="Produce Document" /><br />
                    <hr /> 
                    Transaction date<asp:TextBox ID="txtTransactionDate" runat="server"></asp:TextBox><br />
                    <br />
                    <strong>Media Type &nbsp; &nbsp; &nbsp; :</strong><asp:DropDownList ID="ddlMediaType"
                        runat="server" Width="160px" AutoPostBack="True">
                    </asp:DropDownList><br />
                    <br />
                    Media reference<strong>:</strong><asp:TextBox ID="txtMediaReference" runat="server"></asp:TextBox> 
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; Bank Reference<asp:TextBox
                        ID="txtBankAccntName" runat="server"></asp:TextBox><br />
                    <br />
                    Our Reference &nbsp;
                    <asp:TextBox ID="txtOurReference" runat="server"></asp:TextBox>
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; Their Reference<asp:TextBox
                        ID="txtTheirReference" runat="server"></asp:TextBox><br />
                    <br />
                    CollectionDate &nbsp;
                    <asp:TextBox ID="txtCollectionDate" runat="server"></asp:TextBox>
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; Comments<asp:TextBox ID="txtComments"
                        runat="server"></asp:TextBox><br />
                    <hr />
                    <strong>Posting Information
                        <br />
                    </strong>
                    <br />
                    Account:
                    <asp:TextBox ID="txtAccount" runat="server">CLMPAYBLE</asp:TextBox>
                    &nbsp; &nbsp; &nbsp;&nbsp; Allocation Status<asp:TextBox ID="txtAllocationStatus"
                        runat="server"></asp:TextBox><br />
                    <hr />
                    <strong>Amounts<br />
                    </strong>&nbsp; &nbsp; &nbsp; &nbsp;&nbsp; <strong>Amount</strong> &nbsp;
                    <asp:TextBox ID="txtAmount" runat="server"></asp:TextBox><br />
                    <strong>&nbsp; &nbsp; &nbsp; &nbsp;&nbsp; </strong>
                    --%>
                        </asp:View>
                        <asp:View ID="vRecipt" runat="server">
                            <table width="100%">
                                <tr>
                                    <td colspan="4">
                                        Cheque Information
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10%">
                                        Name
                                    </td>
                                    <td style="width: 20%">
                                        <asp:TextBox ID="txtName" runat="server" Enabled="False"></asp:TextBox>
                                    </td>
                                    <td style="width: 15%">
                                        Bank
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlBank" runat="server" Enabled="False">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Cheque Date
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtChequeDate" runat="server" Enabled="False"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        Credit Card Information
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Card Number
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCardNumber" runat="server" Enabled="False"></asp:TextBox>
                                    </td>
                                    <td>
                                        Name On the Card</td>
                                    <td>
                                        <asp:TextBox ID="txtNameOnCard" runat="server" Enabled="False"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        Expiry Date
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtExpiryDate" runat="server" Enabled="False"></asp:TextBox>
                                    </td>
                                    <td>
                                        Start Date
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtStartDate" runat="server" Enabled="False"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Issue Number
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIssueNumber" runat="server" Enabled="False"></asp:TextBox>
                                    </td>
                                    <td>
                                        CSV/PIN
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCSVPIN" runat="server" Enabled="False"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        Auth Code
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAuthCode" runat="server" Enabled="False"></asp:TextBox>
                                    </td>
                                    <td>
                                        Manual Auth
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtManualAuth" runat="server" Enabled="False"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Customer
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkCustomerPresent" runat="server" Width="240px" Enabled="False" />
                                    </td>
                                </tr>
                            </table>
                            <%--
                    <strong>Cheque Information<br />
                        <br />
                    </strong>Name: &nbsp; &nbsp; &nbsp; &nbsp;
                    <asp:TextBox ID="txtName" runat="server" Enabled="False"></asp:TextBox>
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; Bank<asp:DropDownList ID="ddlBank"
                        runat="server" Enabled="False">
                    </asp:DropDownList><br />
                    <br />
                    Cheque Date<asp:TextBox ID="txtChequeDate" runat="server" Enabled="False"></asp:TextBox><br />
                    <hr />
                    Credit Card Information<br />
                    <br />
                    Card Number :
                    <asp:TextBox ID="txtCardNumber" runat="server" Enabled="False"></asp:TextBox>
                    &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; Name On The Card<asp:TextBox ID="txtNameOnCard"
                        runat="server" Enabled="False"></asp:TextBox><br />
                    <br />
                    Expiry Date: &nbsp; &nbsp;&nbsp;
                    <asp:TextBox ID="txtExpiryDate" runat="server" Enabled="False"></asp:TextBox>
                    &nbsp; &nbsp; &nbsp; &nbsp; Start Date: &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp;
                    <asp:TextBox ID="txtStartDate" runat="server" Enabled="False"></asp:TextBox><br />
                    <br />
                    Issue Number &nbsp;
                    <asp:TextBox ID="txtIssueNumber" runat="server" Enabled="False"></asp:TextBox>
                    &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; CSV/PIN &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                    <asp:TextBox ID="txtCSVPIN" runat="server" Enabled="False"></asp:TextBox><br />
                    <br />
                    Auth Code &nbsp; &nbsp; &nbsp; &nbsp;
                    <asp:TextBox ID="txtAuthCode" runat="server" Enabled="False"></asp:TextBox>
                    &nbsp; &nbsp; &nbsp; &nbsp; Manual Auth &nbsp; &nbsp; &nbsp;&nbsp;
                    <asp:TextBox ID="txtManualAuth" runat="server" Enabled="False"></asp:TextBox><br />
                    <br />
                    Customer &nbsp; &nbsp;Present &nbsp; &nbsp; &nbsp; &nbsp;
                    <asp:CheckBox ID="chkCustomerPresent" runat="server" Width="240px" Enabled="False" />
                    --%>
                        </asp:View>
                        <%-- <br /> --%>
                        <asp:View ID="vPayments" runat="server">
                            <strong>Payment Information<br />
                            </strong>Status :
                            <asp:DropDownList ID="ddlStatus" runat="server" Width="120px">
                            </asp:DropDownList><br />
                            <hr />
                            <asp:Panel ID="pnlPayee" runat="server" Height="50px" Width="608px">
                                <strong>Payee Information</strong><table>
                                    <tr>
                                        <td style="width: 100px">
                                            Payee Name :</td>
                                        <td style="width: 100px">
                                            <asp:TextBox ID="txtPayeeName" runat="server"></asp:TextBox></td>
                                        <td style="width: 100px">
                                            Account code</td>
                                        <td style="width: 100px">
                                            <asp:TextBox ID="txtAccountCode" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100px">
                                            Expiry Date:
                                        </td>
                                        <td style="width: 100px">
                                            <asp:TextBox ID="txtPaymentExpDate" runat="server"></asp:TextBox></td>
                                        <td style="width: 100px">
                                            Branch Code:</td>
                                        <td style="width: 100px">
                                            <asp:TextBox ID="txtBranchCode" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100px">
                                            Reference 1:&nbsp;</td>
                                        <td style="width: 100px">
                                            <asp:TextBox ID="txtRef1" runat="server"></asp:TextBox></td>
                                        <td style="width: 100px">
                                            Reference 2 &nbsp; &nbsp;</td>
                                        <td style="width: 100px">
                                            <asp:TextBox ID="txtref2" runat="server"></asp:TextBox></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <br />
                            <hr />
                            <strong>
                                <asp:Panel ID="pnlBank" runat="server" Height="50px" Width="568px">
                                    Bank<br />
                                    <table>
                                        <tr>
                                            <td>
                                                Date Presented</td>
                                            <td>
                                                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox></td>
                                            <td>
                                                <asp:CheckBox ID="CheckBox1" runat="server" Text="In Possesion" Width="107px" /></td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Stop Requested</td>
                                            <td>
                                                <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox></td>
                                            <td style="width: 100px">
                                                Confirmation</td>
                                            <td style="width: 100px">
                                                <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Reason&nbsp;</td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtReason" runat="server" Height="56px" Width="448px"></asp:TextBox></td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                &nbsp;</strong></asp:View>
                        <%--  <br /> --%>
                        <asp:View ID="Address" runat="server">
                            <table style="width: 512px; height: 216px">
                                <tr>
                                    <td style="width: 95px">
                                        Name</td>
                                    <td>
                                        <asp:TextBox ID="txtContactName" runat="server" Width="208px"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 95px">
                                        Name Street</td>
                                    <td>
                                        <asp:TextBox ID="txtStreetName" runat="server" Width="208px"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 95px">
                                        Locality</td>
                                    <td>
                                        <asp:TextBox ID="txtLocality" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 95px">
                                        Post Town</td>
                                    <td>
                                        <asp:TextBox ID="txtpostTown" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 95px; height: 18px">
                                        County</td>
                                    <td style="height: 18px">
                                        <asp:TextBox ID="txtCountry" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 95px; height: 32px;">
                                        PostCode</td>
                                    <td style="height: 32px">
                                        <asp:TextBox ID="txtAddressPostCode" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 95px; height: 24px;">
                                        Country</td>
                                    <td style="height: 24px">
                                        <asp:DropDownList ID="ddlCountry" runat="server">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td style="width: 95px; height: 56px">
                                        Further Details</td>
                                    <td style="height: 56px">
                                        <asp:TextBox ID="txtFurtherDetails" runat="server" Height="72px" Width="304px"></asp:TextBox></td>
                                </tr>
                            </table>
                            <asp:Button ID="btnAddressOk" runat="server" Text="Ok" CausesValidation="False" />
                            <asp:Button ID="btnAddressCancel" runat="server" Text="Cancel" CausesValidation="False" /></asp:View>
                    </asp:MultiView>&nbsp;<asp:Button ID="btnOk" runat="server" Text="Ok" Width="88px" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="False" /><br />
                    <asp:Label ID="lblPolicyNum" runat="server"></asp:Label><br />
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowSummary="False" />
                </td>
            </tr>
            <tr style="height: 100px">
                <td>
                    <uc2:Footer ID="Footer1" runat="server" />
                </td>
            </tr>
        </table>
        <input type="hidden" id="Cancel" runat="server" />
    </form>
</body>
</html>
