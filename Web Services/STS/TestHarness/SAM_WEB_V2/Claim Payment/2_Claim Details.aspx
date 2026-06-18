<%@ Page Language="VB" AutoEventWireup="false" CodeFile="2_Claim Details.aspx.vb"
    Inherits="Maintain_Claim_Default" %>

<%@ Register Src="../UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Claim Details</title>
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
                        <asp:Label ID="Label1" runat="server" BorderStyle="Double" Font-Bold="True" Font-Italic="True"
                            ForeColor="#C000C0" Text="Claim Details" Width="192px"></asp:Label><br />
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
                                                <tr visible="false"> 
                                                    <td style="width: 128px; height: 21px">
                                                        Claim Number</td>
                                                    <td style="width: 80%">
                                                        <asp:TextBox ID="txtClaimNumber" runat="server"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 128px; height: 24px;">
                                                        <strong>Claim Handler</strong></td>
                                                    <td style="width: 80%; height: 24px;">
                                                        <asp:DropDownList ID="ddlClaimHandler" runat="server" Width="154px">
                                                        </asp:DropDownList></td>
                                                </tr>
                                                 <tr>
                                                    <td style="width: 128px; height: 24px;">
                                                        <strong>Progress Status</strong></td>
                                                    <td style="width: 80%; height: 24px;">
                                                        <asp:DropDownList ID="ddlProgressStatus" runat="server" Width="154px">
                                                        </asp:DropDownList></td>
                                                </tr>                                                
                                                <tr>
                                                    <td style="width: 128px">
                                                        Claim Status</td>
                                                    <td style="width: 80%">
                                                        <asp:TextBox ID="txtClaimStatus" runat="server">Live Open Claim</asp:TextBox></td>
                                                        <td style="width: 15%">
                                                        Claim Status Date</td>
                                                    <td style="width: 35%">
                                                        <asp:TextBox ID="txtClaimStatusDate" runat="server"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 128px">
                                                        <strong>Description</strong></td>
                                                    <td style="width: 80%">
                                                        <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 128px">
                                                        <strong>Primary Cause</strong></td>
                                                    <td style="width: 80%">
                                                        <asp:DropDownList ID="ddlPrimaryCause" runat="server" Width="154px">
                                                        </asp:DropDownList>
                                                      </td>
                                                      <td style="width: 128px">
                                                        Secondary Cause</td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlSecondary" runat="server" Width="192px">
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 128px">
                                                        Catastrophe Code</td>
                                                    <td style="width: 80%">
                                                        <asp:DropDownList ID="ddlCatastrophe" runat="server" Width="154px">
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 128px">
                                                        Town</td>
                                                    <td style="width: 80%">
                                                        <asp:DropDownList ID="ddlTown" runat="server" Width="154px">
                                                        </asp:DropDownList> 
                                                       </td>
                                                       <td style="width: 128px">
                                                        Location</td>
                                                    <td style="width: 128px">
                                                        <asp:TextBox ID="txtLocation" runat="server"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 128px; height: 26px;">
                                                        <strong>Loss Date</strong></td>
                                                    <td style="width: 80%; height: 26px;">
                                                        <asp:TextBox ID="txtLossDate" runat="server"></asp:TextBox></td>
                                                        <td style="width: 128px">
                                                        Loss To Date</td>
                                                    <td style="width: 128px">
                                                        <asp:TextBox ID="txtLossToDate" runat="server"></asp:TextBox>
                                                        </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 128px">
                                                        <strong>Reported Date</strong></td>
                                                    <td style="width: 80%">
                                                        <asp:TextBox ID="txtReportedDate" runat="server"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 128px">
                                                        Last Modified Date</td>
                                                    <td style="width: 80%">
                                                        <asp:TextBox ID="txtLastModifiedDate" runat="server"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 128px">
                                                        <strong>Risk Type</strong></td>
                                                    <td style="width: 80%">
                                                        <asp:DropDownList ID="ddlRiskType" runat="server" Width="154px">
                                                        </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                        Information
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkInformation" runat="server" TextAlign="Left" Width="128px" /></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 128px; height: 21px">
                                                        <strong>Currency</strong></td>
                                                    <td style="width: 80%">
                                                        <asp:DropDownList ID="ddlCurrency" runat="server" Width="154px">
                                                        </asp:DropDownList>
                                                        </td>
                                                         <td>
                                                        Claim Version</td>
                                                    <td>
                                                        <asp:TextBox ID="txtClaimVersion" runat="server" Enabled="False" Width="56px">1</asp:TextBox></td>
                                                </tr>
                                            </table>
                                        </asp:View>
                                        <asp:View ID="ClientDetails" runat="server">
                                            <table width="900">
                                                <tr>
                                                    <td style="width: 203px">
                                                        Client Name</td>
                                                    <td style="width: 370px">
                                                        <asp:TextBox ID="txtClientName" runat="server"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 203px; height: 9px">
                                                        <asp:Button ID="btnAddress" runat="server" Text="Address" /></td>
                                                    <td style="width: 370px; height: 9px">
                                                        <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 203px">
                                                        Telephone Number(H)<asp:TextBox ID="txtTelephoneNumber" runat="server"></asp:TextBox></td>
                                                    <td style="width: 370px">
                                                        Fax Number &nbsp;&nbsp; &nbsp;<asp:TextBox ID="txtFaxNumber" runat="server"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 203px">
                                                        Telephone Number(O)
                                                        <asp:TextBox ID="txtTelephoneOffice" runat="server"></asp:TextBox></td>
                                                    <td style="width: 370px">
                                                        Mobile Number<asp:TextBox ID="txtMobileNumber" runat="server"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 203px; height: 19px">
                                                        Vat Registration number<asp:TextBox ID="txtVATRegistrationNumber" runat="server"></asp:TextBox></td>
                                                    <td style="width: 370px; height: 19px">
                                                        Email Number&nbsp;
                                                        <asp:TextBox ID="txtEmailNumber" runat="server"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 203px; height: 19px">
                                                        Client Claim number<asp:TextBox ID="txtClientClaimNumber" runat="server"></asp:TextBox></td>
                                                    <td style="width: 370px; height: 19px">
                                                        VAT Registered<asp:CheckBox ID="chkVatRegistered" runat="server" /></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <hr />
                                                        <asp:Panel ID="pnlAddress" runat="server" Height="224px" Width="568px" Visible="False">
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
                                                                        <asp:DropDownList ID="ddlAddressType" runat="server" Width="154px">
                                                                        </asp:DropDownList></td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 95px">
                                                                        Name Street</td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtStreetName" runat="server"></asp:TextBox></td>
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
                                                                    <td style="width: 95px">
                                                                        Country</td>
                                                                    <td>
                                                                        &nbsp;<asp:DropDownList ID="ddlCountry" runat="server" Width="154px">
                                                                        </asp:DropDownList>
                                                                        <asp:TextBox ID="txtCountry" runat="server" Visible="False"></asp:TextBox></td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 95px">
                                                                        PostCode</td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtAddressPostCode" runat="server"></asp:TextBox></td>
                                                                </tr>
                                                            </table>
                                                            <asp:Button ID="btnAddressOk" runat="server" Text="Ok" />
                                                            <asp:Button ID="btnAddressCancel" runat="server" Text="Cancel" /></asp:Panel>
                                                        <hr />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:View>
                                        <asp:View ID="AgentDetails" runat="server">
                                        </asp:View>
                                        <asp:View ID="PaymentHistory" runat="server">
                                            <div style="width: 100%; height: 200px; overflow: scroll">
                                                <asp:GridView ID="gvPaymenyHistory" runat="server" >
                                                </asp:GridView>
                                            </div>
                                        </asp:View>
                                    </asp:MultiView></td>
                            </tr>
                            <tr>
                                <td style="width: 530px">
                                    <hr />
                                    <asp:Button ID="btnOk" runat="server" Text="Ok" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
