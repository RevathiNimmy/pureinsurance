<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CashListItem.aspx.vb" Inherits="Claim_Payment_CashListItem" %>

<%@ Register Src="../UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CashList Item</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
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
                    <asp:MenuItem Text="Payment" Value="2"></asp:MenuItem>
                    <asp:MenuItem Text="Address" Value="3"></asp:MenuItem>
                </Items>
                <StaticHoverStyle BackColor="#7C6F57" ForeColor="White" />
            </asp:Menu>
            <asp:MultiView ID="mvCashListItem" runat="server" ActiveViewIndex="0">
                <asp:View ID="vDetails" runat="server">
                    <strong>
                        <br />
                        Payment &nbsp;Type:&nbsp; </strong>
                    <asp:DropDownList ID="ddlPaymentType" runat="server" Width="152px">
                    </asp:DropDownList><strong> &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    </strong>
                    <asp:CheckBox ID="chkProduceDocument" runat="server" Font-Bold="False" Text="Produce Document" /><br />
                    <hr />
                    <strong>Transaction Indormation<br />
                        </strong>
                    <strong></strong>
                    <table>
                        <tr>
                            <td style="width: 100px">
                                <strong>
                        Transaction date</strong></td>
                            <td style="width: 100px">
                                <asp:TextBox ID="txtTransactionDate" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 100px">
                                <strong>Media Type &nbsp; &nbsp; :</strong>
                            </td>
                            <td style="width: 100px">
                                <asp:DropDownList ID="ddlMediaType"
                        runat="server" Width="160px" AutoPostBack="True">
                    </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td style="width: 100px">
                    Media reference<strong>:</strong></td>
                            <td style="width: 100px">
                                <asp:TextBox ID="txtMediaReference" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 100px">
                    Our Reference &nbsp;&nbsp;</td>
                            <td style="width: 100px">
                    <asp:TextBox ID="txtOurReference" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 100px">
                                &nbsp;&nbsp; Their Reference</td>
                            <td style="width: 100px">
                                <asp:TextBox
                        ID="txtTheirReference" runat="server"></asp:TextBox></td>
                        </tr>
                    </table>
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;<br />
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
                    <asp:TextBox ID="txtAmount" runat="server" Enabled="False"></asp:TextBox></asp:View>
                <asp:View ID="vRecipt" runat="server">
                    <strong>Cheque Information<br />
                        <br />
                    </strong>Name: &nbsp; &nbsp; &nbsp; &nbsp;
                    <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; Bank<asp:DropDownList ID="ddlBank"
                        runat="server">
                    </asp:DropDownList><br />
                    <br />
                    Cheque Date<asp:TextBox ID="txtChequeDate" runat="server"></asp:TextBox><br />
                    <hr />
                </asp:View>
                <br />
                <asp:View ID="vPayments" runat="server">
                    <strong>Payment Information<br />
                        <br />
                    </strong>Status :
                    <asp:DropDownList ID="ddlStatus" runat="server" Width="120px">
                    </asp:DropDownList><br />
                    <hr />
                    <asp:Panel ID="pnlPayee" runat="server" Height="50px" Width="608px">
                        <strong>Payee Information<br />
                        </strong>
                        <table >
                            <tr>
                                <td style="width: 100px">
                                    Payee Name :</td>
                                <td style="width: 100px">
                        <asp:TextBox ID="txtPayeeName" runat="server"></asp:TextBox></td>
                                <td style="width: 100px">
                                    &nbsp;Account code
                                </td>
                                <td style="width: 100px">
                        <asp:TextBox ID="txtAccountCode" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td style="width: 100px">
                                    Expiry Date: &nbsp;</td>
                                <td style="width: 100px">
                        <asp:TextBox ID="txtPaymentExpDate" runat="server"></asp:TextBox></td>
                                <td style="width: 100px">
                                    &nbsp; &nbsp;Branch Code: &nbsp; &nbsp;&nbsp;</td>
                                <td style="width: 100px">
                        <asp:TextBox ID="txtBranchCode" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td style="width: 100px">
                        Reference 1:&nbsp;&nbsp;</td>
                                <td style="width: 100px">
                        <asp:TextBox ID="txtRef1" runat="server"></asp:TextBox></td>
                                <td style="width: 100px">
                                    &nbsp;Reference 2 &nbsp; &nbsp;</td>
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
                            Date Presented&nbsp;
                            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                             <asp:CheckBox ID="CheckBox1" runat="server" Text="In Possesion" /><br />
                            <br />
                            Stop Requested
                            <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                            &nbsp; &nbsp;Confirmation<asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>&nbsp;<br />
                            <br />
                            Reason &nbsp;&nbsp;
                            <asp:TextBox ID="txtReason" runat="server" Height="56px" Width="448px"></asp:TextBox><br />
                        </asp:Panel>
                        <asp:Panel ID="pnlCreditCard" runat="server" Height="50px" Visible="False" Width="600px">
                            Credit Card Information<br />
                            <table>
                                <tr>
                                    <td style="width: 100px">
                                        Card Number :
                                    </td>
                                    <td style="width: 100px">
                            <asp:TextBox ID="txtCardNumber" runat="server"></asp:TextBox></td>
                                    <td style="width: 100px">
                                        Name On The Card</td>
                                    <td style="width: 100px">
                                        <asp:TextBox ID="txtNameOnCard"
                                runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 100px">
                                        Expiry Date: &nbsp;
                                    </td>
                                    <td style="width: 100px">
                            <asp:TextBox ID="txtExpiryDate" runat="server"></asp:TextBox></td>
                                    <td style="width: 100px">
                                        Start Date: 
                                    </td>
                                    <td style="width: 100px">
                            <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 100px; height: 26px">
                            Issue Number</td>
                                    <td style="width: 100px; height: 26px">
                            <asp:TextBox ID="txtIssueNumber" runat="server"></asp:TextBox></td>
                                    <td style="width: 100px; height: 26px">
                                        CSV/PIN</td>
                                    <td style="width: 100px; height: 26px">
                            <asp:TextBox ID="txtCSVPIN" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 100px">
                            Auth Code</td>
                                    <td style="width: 100px">
                            <asp:TextBox ID="txtAuthCode" runat="server"></asp:TextBox></td>
                                    <td style="width: 100px">
                                        Manual Auth &nbsp; &nbsp; 
                                    </td>
                                    <td style="width: 100px">
                            <asp:TextBox ID="txtManualAuth" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 100px">
                            Customer &nbsp; &nbsp;Present </td>
                                    <td style="width: 100px">
                            <asp:CheckBox ID="chkCustomerPresent" runat="server" Width="240px" /></td>
                                    <td style="width: 100px">
                                    </td>
                                    <td style="width: 100px">
                                    </td>
                                </tr>
                            </table>
                              </asp:Panel>
                    </strong>
                    <br />
                    &nbsp;</asp:View>
                <br />
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
                    <asp:Button ID="btnAddressOk" runat="server" Text="Ok" />
                    <asp:Button ID="btnAddressCancel" runat="server" Text="Cancel" /></asp:View>
            </asp:MultiView>&nbsp;<asp:Button ID="btnOk" runat="server" Text="Ok" Width="88px" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
            </td>
            </tr>
            </table>
            </div>
            
    </form>
</body>
</html>
