<%@ Page Language="VB" AutoEventWireup="false" CodeFile="2_Claim Details.aspx.vb"
    Inherits="Maintain_Claim_Default" %>

<%@ Register Src="../UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
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
                <tr style="height: 90%">
                    <td>
                        <div>
                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="FinancialDetails.aspx"
                                Width="128px">Financial details</asp:HyperLink>
                            <table style="width: 472px">
                                <tr>
                                    <td style="width: 530px">
                                        <asp:Menu ID="Menu1" runat="server" BackColor="#F7F6F3" DynamicHorizontalOffset="2"
                                            Font-Names="Verdana" Font-Size="0.8em" ForeColor="#7C6F57" Orientation="Horizontal"
                                            StaticSubMenuIndent="10px" Width="100%">
                                            <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                                            <DynamicHoverStyle BackColor="#7C6F57" ForeColor="White" />
                                            <DynamicMenuStyle BackColor="#F7F6F3" />
                                            <StaticSelectedStyle BackColor="#5D7B9D" />
                                            <DynamicSelectedStyle BackColor="#5D7B9D" />
                                            <DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                                            <Items>
                                                <asp:MenuItem Text="Claim Details" Value="0"></asp:MenuItem>
                                                <asp:MenuItem Text="Client Details" Value="1"></asp:MenuItem>
                                                <asp:MenuItem Selectable="False" Text="AgentDetails" Value="2"></asp:MenuItem>
                                                <asp:MenuItem Text="Payment History" Value="3"></asp:MenuItem>
                                            </Items>
                                            <StaticHoverStyle BackColor="#7C6F57" ForeColor="White" />
                                        </asp:Menu>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 530px">
                                        <asp:MultiView ID="mvClaimDetails" runat="server" ActiveViewIndex="0">
                                            <asp:View ID="ClaimDetails" runat="server">
                                                <table width="900" class ="body">
                                                    <tr>
                                                        <td style="width: 15%">
                                                            <strong>Claim Number</strong></td>
                                                        <td style="width: 222px">
                                                            <asp:TextBox ID="txtClaimNumber" runat="server" Width="205px"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtClaimNumber"
                                                                ErrorMessage="*" Font-Bold="True"></asp:RequiredFieldValidator></td>
                                                        <td style="width: 166px">
                                                        </td>
                                                        <td style="width: 45%">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 133px">
                                                            <strong>Claim Handler</strong></td>
                                                        <td style="width: 222px">
                                                            <asp:DropDownList ID="ddlClaimHandler" runat="server" Width="210px">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddlClaimHandler"
                                                                ErrorMessage="*" Font-Bold="True"></asp:RequiredFieldValidator></td>
                                                        <td style="width: 166px">
                                                        </td>
                                                        <td style="width: 183px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 133px">
                                                            <strong>Progress Status</strong></td>
                                                        <td style="width: 222px">
                                                            <asp:DropDownList ID="ddlProgressStatus" runat="server" Width="211px">
                                                            </asp:DropDownList><br />
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="ddlProgressStatus"
                                                                ErrorMessage="*" Font-Bold="True"></asp:RequiredFieldValidator></td>
                                                        <td style="width: 166px">
                                                        </td>
                                                        <td style="width: 183px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 133px">
                                                            Claim Status</td>
                                                        <td style="width: 222px">
                                                            <asp:TextBox ID="txtClaimStatus" runat="server" Width="204px" Height="19px">Live Open Cliam</asp:TextBox>
                                                        </td>
                                                        <td >
                                                            Claim Status Date </td>
                                                        <td >
                                                            <asp:TextBox ID="txtClaimStatusDate" runat="server" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 133px">
                                                            <strong>Description</strong></td>
                                                        <td style="width: 222px">
                                                            <asp:TextBox ID="txtDescription" runat="server" Width="204px"></asp:TextBox></td>
                                                        <td style="width: 166px">
                                                        </td>
                                                        <td style="width: 183px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 133px">
                                                            <strong>Primary Cause</strong></td>
                                                        <td style="width: 222px">
                                                            <asp:DropDownList ID="ddlPrimaryCause" runat="server" Width="211px">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlPrimaryCause"
                                                                ErrorMessage="*" Font-Bold="True"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td style="width: 166px">
                                                            &nbsp;Secondary Cause</td>
                                                        <td style="width: 183px">
                                                            <asp:DropDownList ID="ddlSecondary" runat="server" Width="153px">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 133px">
                                                            Catastrophe Code</td>
                                                        <td style="width: 222px">
                                                            <asp:DropDownList ID="ddlCatastrophe" runat="server" Width="210px">
                                                            </asp:DropDownList></td>
                                                        <td style="width: 166px">
                                                        </td>
                                                        <td style="width: 183px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 133px">
                                                            Town</td>
                                                        <td style="width: 222px">
                                                            <asp:DropDownList ID="ddlTown" runat="server" Width="211px">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="width: 166px">
                                                            Location&nbsp;&nbsp; &nbsp; &nbsp; &nbsp;</td>
                                                        <td style="width: 183px">
                                                            <asp:TextBox ID="txtLocation" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 133px">
                                                            <strong>Loss Date</strong></td>
                                                        <td style="width: 222px">
                                                            <asp:TextBox ID="txtLossDate" runat="server" Width="205px"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtLossDate"
                                                                ErrorMessage="*" Font-Bold="True"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td style="width: 166px">
                                                            Loss To Date&nbsp;&nbsp;&nbsp;</td>
                                                        <td style="width: 183px">
                                                            <asp:TextBox ID="txtLossToDate" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 133px">
                                                            <strong>Reported Date</strong></td>
                                                        <td style="width: 222px">
                                                            <asp:TextBox ID="txtReportedDate" runat="server" Width="207px"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtReportedDate"
                                                                ErrorMessage="*" Font-Bold="True"></asp:RequiredFieldValidator></td>
                                                        <td style="width: 166px">
                                                        </td>
                                                        <td style="width: 183px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 133px">
                                                            Last Modified Date</td>
                                                        <td style="width: 222px">
                                                            <asp:TextBox ID="txtLastModifiedDate" runat="server" Width="208px" Enabled="False"></asp:TextBox></td>
                                                        <td style="width: 166px">
                                                        </td>
                                                        <td style="width: 183px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 133px">
                                                            <strong>Risk Type</strong></td>
                                                        <td style="width: 222px">
                                                            <asp:DropDownList ID="ddlRiskType" runat="server" Width="210px">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlRiskType"
                                                                ErrorMessage="*" Font-Bold="True"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td style="width: 166px">
                                                            <asp:CheckBox ID="chkInformationOnly" runat="server" Text="Information" />
                                                        </td>
                                                        <td style="width: 183px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 133px">
                                                            <strong>Currency</strong></td>
                                                        <td style="width: 222px">
                                                            <asp:DropDownList ID="ddlCurrency" runat="server" Width="212px">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="ddlCurrency"
                                                                ErrorMessage="*" Font-Bold="True"></asp:RequiredFieldValidator></td>
                                                        <td style="width: 166px">
                                                        </td>
                                                        <td style="width: 183px">
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:View>
                                            <asp:View ID="ClientDetails" runat="server">
                                                <table width="900">
                                                    <tr>
                                                        <td style="width: 15%; height: 23px;">
                                                            Client Name</td>
                                                        <td style="width: 25%; height: 23px;">
                                                            <asp:TextBox ID="txtClientName" runat="server" Width="173px"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 15%; height: 23px;">
                                                        </td>
                                                        <td style="width: 45%; height: 23px;">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 203px; height: 45px">
                                                            <asp:Button ID="btnAddress" runat="server" Text="Address" CausesValidation="False" /></td>
                                                        <td style="width: 247px; height: 45px">
                                                            <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 370px; height: 45px">
                                                        </td>
                                                        <td style="width: 370px; height: 45px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 203px; height: 32px;">
                                                            Telephone Number(H)</td>
                                                        <td style="width: 247px; height: 32px">
                                                            <asp:TextBox ID="TextBox3" runat="server" Width="173px"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 370px; height: 32px;">
                                                            Fax Number&nbsp;<br />
                                                            &nbsp;
                                                        </td>
                                                        <td style="width: 370px; height: 32px">
                                                            <asp:TextBox ID="TextBox4" runat="server" Width="173px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 203px; height: 36px;">
                                                            Telephone Number(O)
                                                        </td>
                                                        <td style="width: 247px; height: 36px">
                                                            <asp:TextBox ID="txtTelephoneOffice" runat="server" Width="173px"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 370px; height: 36px;">
                                                            Mobile Number<br />
                                                        </td>
                                                        <td style="width: 370px; height: 36px">
                                                            <asp:TextBox ID="txtMobileNumber" runat="server" Width="173px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 203px">
                                                            Vat Registration number</td>
                                                        <td style="width: 247px">
                                                            <asp:TextBox ID="txtVATRegistrationNumber" runat="server" Width="173px"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 370px">
                                                            Email Number<br />
                                                        </td>
                                                        <td style="width: 370px">
                                                            <asp:TextBox ID="txtEmailNumber" runat="server" Width="173px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 203px; height: 26px;">
                                                            Client Claim number</td>
                                                        <td style="width: 247px; height: 26px">
                                                            <asp:TextBox ID="txtClientClaimNumber" runat="server" Width="173px"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 370px; height: 26px;">
                                                            VAT Registered<br />
                                                        </td>
                                                        <td style="width: 370px; height: 26px">
                                                            <asp:CheckBox ID="chkVatRegistered" runat="server" Width="173px" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <asp:Panel ID="pnlAddress" runat="server" Height="224px" Visible="False" Width="568px">
                                                                <table style="width: 512px; height: 216px">
                                                                    <tr>
                                                                        <td style="width: 95px">
                                                                        </td>
                                                                        <td>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 95px">
                                                                        </td>
                                                                        <td>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 95px">
                                                                            Type</td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddlAddressType" runat="server" Width="168px">
                                                                            </asp:DropDownList></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 95px">
                                                                            Name Street</td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtStreetName" runat="server" Width="161px"></asp:TextBox></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 95px">
                                                                            Locality</td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtLocality" runat="server" Width="164px"></asp:TextBox></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 95px">
                                                                            Post Town</td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtpostTown" runat="server" Width="163px"></asp:TextBox></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 95px">
                                                                            Country</td>
                                                                        <td>
                                                                            &nbsp;<asp:DropDownList ID="ddlCountry" runat="server">
                                                                            </asp:DropDownList>
                                                                            <asp:TextBox ID="txtCountry" runat="server" Visible="False"></asp:TextBox></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 95px">
                                                                            PostCode</td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtAddressPostCode" runat="server" Width="162px"></asp:TextBox></td>
                                                                    </tr>
                                                                </table>
                                                                <asp:Button ID="btnAddressOk" runat="server" Text="Ok" CausesValidation="False" />
                                                                <asp:Button ID="btnAddressCancel" runat="server" Text="Cancel" CausesValidation="False" /></asp:Panel>
                                                        </td>
                                                        <td colspan="1">
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:View>
                                            <asp:View ID="AgentDetails" runat="server">
                                            </asp:View>
                                            <asp:View ID="PaymentHistory" runat="server">
                                                <asp:GridView ID="gvPaymenyHistory" runat="server" Width="608px" CellPadding="4"
                                                    Font-Size="Smaller" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False">
                                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                    <EditRowStyle BackColor="#999999" />
                                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                    <Columns>
                                                        <asp:BoundField DataField="PaymentAmount" HeaderText="Payment Amount" />
                                                        <asp:BoundField DataField="BaseAmount" HeaderText="BaseAmount" />
                                                        <asp:BoundField DataField="PaymentDate" HeaderText="Payment Date" />
                                                        <asp:BoundField DataField="LossAmount" HeaderText="Loss Amount" />
                                                        <asp:BoundField DataField="CurrencyDescription" HeaderText="Currency" />
                                                        <asp:BoundField DataField="TaxAmount" HeaderText="Tax" />
                                                        <asp:BoundField DataField="Payee" HeaderText="Payee" />
                                                    </Columns>
                                                </asp:GridView>
                                            </asp:View>
                                        </asp:MultiView></td>
                                </tr>
                                <tr>
                                    <td style="width: 530px">
                                        <asp:Button ID="btnOk" runat="server" Text="Ok" /></td>
                                </tr>
                            </table>
                        </div>
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowSummary="False" />
                    </td>
                </tr>
            </table>
        </div>
        <%-- <div>
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="FinancialDetails.aspx"
            Width="128px">Financial details</asp:HyperLink>
        <table style="width: 472px">
            <tr>
                <td style="width: 530px">
                    <asp:Menu ID="Menu1" runat="server" BackColor="#F7F6F3" DynamicHorizontalOffset="2"
                        Font-Names="Verdana" Font-Size="0.8em" ForeColor="#7C6F57" Orientation="Horizontal"
                        StaticSubMenuIndent="10px" Width="100%">
                        <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                        <DynamicHoverStyle BackColor="#7C6F57" ForeColor="White" />
                        <DynamicMenuStyle BackColor="#F7F6F3" />
                        <StaticSelectedStyle BackColor="#5D7B9D" />
                        <DynamicSelectedStyle BackColor="#5D7B9D" />
                        <DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                        <Items>
                            <asp:MenuItem Text="Claim Details" Value="0"></asp:MenuItem>
                            <asp:MenuItem Text="Client Details" Value="1"></asp:MenuItem>
                            <asp:MenuItem Selectable="False" Text="AgentDetails" Value="2"></asp:MenuItem>
                            <asp:MenuItem Text="Payment History" Value="3"></asp:MenuItem>
                        </Items>
                        <StaticHoverStyle BackColor="#7C6F57" ForeColor="White" />
                    </asp:Menu>
                </td>
            </tr>
            <tr>
                <td style="width: 530px">
                    <asp:MultiView ID="mvClaimDetails" runat="server" ActiveViewIndex="0">
                        <asp:View ID="ClaimDetails" runat="server">
                            <table width="900">
                                <tr>
                                    <td >
                                        <strong>Claim Number</strong></td>
                                    <td >
                                        <asp:TextBox ID="txtClaimNumber" runat="server" Width="205px"></asp:TextBox></td>
                                    <td >
                                    </td>
                                    <td >
                                    </td>
                                </tr>
                                <tr>
                                    <td >
                                        <strong>Claim Handler</strong></td>
                                    <td >
                                        <asp:DropDownList ID="ddlClaimHandler" runat="server" Width="210px">
                                        </asp:DropDownList></td>
                                    <td >
                                    </td>
                                    <td >
                                    </td>
                                </tr>
                                <tr>
                                    <td >
                                        <strong>Progress Status</strong></td>
                                    <td >
                                        <asp:DropDownList ID="ddlProgressStatus" runat="server" Width="211px">
                                        </asp:DropDownList></td>
                                    <td >
                                    </td>
                                    <td >
                                    </td>
                                </tr>
                                <tr>
                                    <td >
                                        Claim Status</td>
                                    <td >
                                        <asp:TextBox ID="txtClaimStatus" runat="server" Width="204px" Height="19px"></asp:TextBox>
                                       </td>
                                    <td >
                                        &nbsp;Claim Status Date &nbsp;&nbsp;</td>
                                    <td >
                                        <asp:TextBox ID="txtClaimStatusDate" runat="server" ></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td >
                                        <strong>Description</strong></td>
                                    <td >
                                        <asp:TextBox ID="txtDescription" runat="server" Width="204px"></asp:TextBox></td>
                                    <td >
                                    </td>
                                    <td >
                                    </td>
                                </tr>
                                <tr>
                                    <td >
                                        <strong>Primary Cause</strong></td>
                                    <td style="width: 20%; height: 24px;">
                                        <asp:DropDownList ID="ddlPrimaryCause" runat="server" Width="211px">
                                        </asp:DropDownList>
                                      </td>
                                    <td style="width: 41%; height: 24px">
                                        Secondary Cause</td>
                                    <td style="width: 610%; height: 24px">
                                        <asp:DropDownList ID="ddlSecondary" runat="server" Width="153px" >
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td >
                                        Catastrophe Code</td>
                                    <td >
                                        <asp:DropDownList ID="ddlCatastrophe" runat="server" Width="210px">
                                        </asp:DropDownList></td>
                                    <td >
                                    </td>
                                    <td >
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 41%; height: 26px;">
                                        Town</td>
                                    <td style="width: 20%; height: 26px;">
                                        <asp:DropDownList ID="ddlTown" runat="server" Width="211px">
                                        </asp:DropDownList>
                                       
                                    </td>
                                    <td style="width: 41%; height: 26px">
                                        Location&nbsp;&nbsp; &nbsp; &nbsp; &nbsp;</td>
                                    <td style="width: 610%; height: 26px">
                                        <asp:TextBox ID="txtLocation" runat="server" ></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td >
                                        <strong>Loss Date</strong></td>
                                    <td >
                                        <asp:TextBox ID="txtLossDate" runat="server" Width="205px"></asp:TextBox>
                                    </td>
                                    <td >
                                        Loss To Date&nbsp;&nbsp;&nbsp;</td>
                                    <td >
                                        <asp:TextBox ID="txtLossToDate" runat="server" ></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td >
                                        <strong>Reported Date</strong></td>
                                    <td >
                                        <asp:TextBox ID="txtReportedDate" runat="server" Width="207px"></asp:TextBox></td>
                                    <td >
                                    </td>
                                    <td >
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 41%; height: 46px;">
                                        Last Modified Date</td>
                                    <td style="width: 20%; height: 46px;">
                                        <asp:TextBox ID="txtLastModifiedDate" runat="server" Width="208px"></asp:TextBox></td>
                                    <td style="width: 41%; height: 46px">
                                    </td>
                                    <td style="width: 610%; height: 46px">
                                    </td>
                                </tr>
                                <tr>
                                    <td >
                                        <strong>Risk Type</strong></td>
                                    <td >
                                        <asp:DropDownList ID="ddlRiskType" runat="server" Width="210px">
                                        </asp:DropDownList>
                                        </td>
                                    <td >
                                        <asp:CheckBox ID="chkInformationOnly" runat="server" Text="Information"  />
                                    </td>
                                    <td >
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 41%; height: 21px">
                                        <strong>Currency</strong></td>
                                    <td >
                                        <asp:DropDownList ID="ddlCurrency" runat="server" Width="212px">
                                        </asp:DropDownList></td>
                                    <td >
                                    </td>
                                    <td >
                                    </td>
                                </tr>
                            </table>
                        </asp:View>
                        <asp:View ID="ClientDetails" runat="server">
                            <table width="900">
                                <tr>
                                    <td style="width: 203px">
                                        Client Name</td>
                                    <td style="width: 370px">
                                        <asp:TextBox ID="txtClientName" runat="server" Width="173px"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 203px; height: 9px">
                                        <asp:Button ID="btnAddress" runat="server" Text="Address" /></td>
                                    <td style="width: 370px; height: 9px">
                                        <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 203px; height: 51px;">
                                        Telephone Number(H)<asp:TextBox ID="TextBox3" runat="server"></asp:TextBox></td>
                                    <td style="width: 370px; height: 51px;">
                                        Fax Number&nbsp;<br />
                                        &nbsp;
                                        <asp:TextBox ID="TextBox4" runat="server" Width="164px"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 203px">
                                        Telephone Number(O)
                                        <asp:TextBox ID="txtTelephoneOffice" runat="server"></asp:TextBox></td>
                                    <td style="width: 370px">
                                        Mobile Number<br />
                                        <asp:TextBox ID="txtMobileNumber" runat="server" Width="172px"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 203px">
                                        Vat Registration number<asp:TextBox ID="txtVATRegistrationNumber" runat="server"></asp:TextBox></td>
                                    <td style="width: 370px">
                                        Email Number<br />
                                        <asp:TextBox ID="txtEmailNumber" runat="server" Width="171px"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 203px">
                                        Client Claim number<asp:TextBox ID="txtClientClaimNumber" runat="server"></asp:TextBox></td>
                                    <td style="width: 370px">
                                        VAT Registered<br />
                                        <asp:CheckBox ID="chkVatRegistered" runat="server" Width="179px" /></td>
                                </tr>
                                <tr>
                                    <td style="width: 203px">
                                    </td>
                                    <td style="width: 370px">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Panel ID="pnlAddress" runat="server" Height="224px" Visible="False" Width="568px">
                                            <table style="width: 512px; height: 216px">
                                                <tr>
                                                    <td style="width: 95px">
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 95px">
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 95px">
                                                        Type</td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlAddressType" runat="server" Width="168px">
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 95px">
                                                        Name Street</td>
                                                    <td>
                                                        <asp:TextBox ID="txtStreetName" runat="server" Width="161px"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 95px">
                                                        Locality</td>
                                                    <td>
                                                        <asp:TextBox ID="txtLocality" runat="server" Width="164px"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 95px">
                                                        Post Town</td>
                                                    <td>
                                                        <asp:TextBox ID="txtpostTown" runat="server" Width="163px"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 95px">
                                                        Country</td>
                                                    <td>
                                                        &nbsp;<asp:DropDownList ID="ddlCountry" runat="server">
                                                        </asp:DropDownList>
                                                        <asp:TextBox ID="txtCountry" runat="server" Visible="False"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 95px">
                                                        PostCode</td>
                                                    <td>
                                                        <asp:TextBox ID="txtAddressPostCode" runat="server" Width="162px"></asp:TextBox></td>
                                                </tr>
                                            </table>
                                            <asp:Button ID="btnAddressOk" runat="server" Text="Ok" />
                                            <asp:Button ID="btnAddressCancel" runat="server" Text="Cancel" /></asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </asp:View>
                        <asp:View ID="AgentDetails" runat="server">
                        </asp:View>
                        <asp:View ID="PaymentHistory" runat="server">
                            <asp:GridView ID="gvPaymenyHistory" runat="server" Width="248px" CellPadding="4" Font-Size="Smaller" ForeColor="#333333" GridLines="None">
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <EditRowStyle BackColor="#999999" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            </asp:GridView>
                        </asp:View>
                    </asp:MultiView></td>
            </tr>
            <tr>
                <td style="width: 530px">
                    <asp:Button ID="btnOk" runat="server" Text="Ok" /></td>
            </tr>
        </table>
    
    </div>--%>
    </form>
</body>
</html>
