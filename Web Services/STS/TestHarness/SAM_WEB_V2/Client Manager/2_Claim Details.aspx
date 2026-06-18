<%@ Page Language="VB" AutoEventWireup="false" CodeFile="2_Claim Details.aspx.vb"
    Inherits="Maintain_Claim_Default" %>



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
                                            &nbsp;<asp:Panel ID="Panel1" runat="server" Enabled="False" Height="50px" Width="125px">
                                                <table width="900">
                                                    <tr>
                                                        <td style="width: 15">
                                                            <strong>Claim Number</strong></td>
                                                        <td style="width: 35%">
                                                            <asp:TextBox ID="txtClaimNumber" runat="server"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <strong>Claim Handler</strong></td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlClaimHandler" runat="server">
                                                            </asp:DropDownList></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Progress Status</td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlProgressStatus" runat="server">
                                                            </asp:DropDownList></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 15%">
                                                            Claim Status</td>
                                                        <td style="width: 35%">
                                                            <asp:TextBox ID="txtClaimStatus" runat="server"></asp:TextBox></td>
                                                        <td style="width: 15%">
                                                            Claim Status Date</td>
                                                        <td style="width: 35%">
                                                            <asp:TextBox ID="txtClaimStatusDate" runat="server"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Description</td>
                                                        <td>
                                                            <asp:TextBox ID="txtDescription" runat="server" Width="456px"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 15%">
                                                            Primary Cause</td>
                                                        <td style="width: 35%">
                                                            <asp:DropDownList ID="ddlPrimaryCause" runat="server" Width="136px">
                                                            </asp:DropDownList></td>
                                                        <td style="width: 15%">
                                                            Secondary Cause</td>
                                                        <td style="width: 35%">
                                                            <asp:DropDownList ID="ddlSecondary" runat="server" Width="192px">
                                                            </asp:DropDownList></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Catastrophe Code</td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlCatastrophe" runat="server">
                                                            </asp:DropDownList></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 15%">
                                                            Town</td>
                                                        <td style="width: 35%">
                                                            <asp:DropDownList ID="ddlTown" runat="server">
                                                            </asp:DropDownList></td>
                                                        <td style="width: 15%">
                                                            Location</td>
                                                        <td style="width: 35%">
                                                            <asp:TextBox ID="txtLocation" runat="server"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 15%">
                                                            <strong>Loss Date</strong></td>
                                                        <td style="width: 35%">
                                                            <asp:TextBox ID="txtLossDate" runat="server"></asp:TextBox></td>
                                                        <td style="width: 15%">
                                                            Loss To Date</td>
                                                        <td style="width: 35%">
                                                            <asp:TextBox ID="txtLossToDate" runat="server"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <strong>Reported Date</strong></td>
                                                        <td>
                                                            <asp:TextBox ID="txtReportedDate" runat="server"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Last Modified Date</td>
                                                        <td>
                                                            <asp:TextBox ID="txtLastModifiedDate" runat="server"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 15px">
                                                            <strong>Risk Type</strong></td>
                                                        <td style="width: 35%">
                                                            <asp:DropDownList ID="ddlRiskType" runat="server">
                                                            </asp:DropDownList></td>
                                                        <td style="width: 15%">
                                                            Information</td>
                                                        <td style="width: 35%">
                                                            <asp:CheckBox ID="chkInformationOnly" runat="server" Text="Information" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <strong>Currency</strong></td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlCurrency" runat="server">
                                                            </asp:DropDownList></td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </asp:View>
                                        <asp:View ID="ClientDetails" runat="server">
                                            <table width="900">
                                                <tr>
                                                    <td style="width: 15%">
                                                        Client Name</td>
                                                    <td style="width: 30%">
                                                        <asp:TextBox ID="txtClientName" runat="server"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 15%">
                                                        Address</td>
                                                    <td style="width: 35%">
                                                        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Telephone Number</td>
                                                    <td>
                                                        <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Fax Number</td>
                                                    <td>
                                                        <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 203px">
                                                    </td>
                                                    <td style="width: 370px">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 203px">
                                                    </td>
                                                    <td style="width: 370px">
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:View>
                                        <asp:View ID="AgentDetails" runat="server">
                                        </asp:View>
                                        <asp:View ID="PaymentHistory" runat="server">
                                            <asp:GridView ID="gvPaymenyHistory" runat="server" Width="248px" CellPadding="4"
                                                Font-Size="Smaller" ForeColor="#333333" GridLines="None">
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
                </table> </td> </tr>
            </table>
            <%-- <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="FinancialDetails.aspx"
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
                            &nbsp;<asp:Panel ID="Panel1" runat="server" Enabled="False" Height="50px" Width="125px">
                            <table width="900">
                                <tr>
                                    <td style="width: 128px; height: 21px">
                                        <strong>Claim Number</strong></td>
                                    <td style="width: 80%">
                                        <asp:TextBox ID="txtClaimNumber" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 128px">
                                        <strong>Claim Handler</strong></td>
                                    <td style="width: 80%">
                                        <asp:DropDownList ID="ddlClaimHandler" runat="server">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td style="width: 128px; height: 21px">
                                        Progress Status</td>
                                    <td style="width: 80%">
                                        <asp:DropDownList ID="ddlProgressStatus" runat="server">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td style="width: 128px">
                                        Claim Status</td>
                                    <td style="width: 80%">
                                        <asp:TextBox ID="txtClaimStatus" runat="server"></asp:TextBox>
                                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; Claim Status Date
                                        <asp:TextBox ID="txtClaimStatusDate" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 128px">
                                        Description</td>
                                    <td style="width: 80%">
                                        <asp:TextBox ID="txtDescription" runat="server" Width="456px"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 128px">
                                        Primary Cause</td>
                                    <td style="width: 80%">
                                        <asp:DropDownList ID="ddlPrimaryCause" runat="server" Width="136px">
                                        </asp:DropDownList><strong>
                                        &nbsp; Secondary Cause&nbsp;</strong><asp:DropDownList ID="ddlSecondary" runat="server" Width="192px">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td style="width: 128px">
                                        Catastrophe Code</td>
                                    <td style="width: 80%">
                                        <asp:DropDownList ID="ddlCatastrophe" runat="server">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td style="width: 128px">
                                        Town</td>
                                    <td style="width: 80%">
                                        <asp:DropDownList ID="ddlTown" runat="server">
                                        </asp:DropDownList>
                                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; Location&nbsp;
                                        <asp:TextBox ID="txtLocation" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 128px">
                                        <strong>Loss Date</strong></td>
                                    <td style="width: 80%">
                                        <asp:TextBox ID="txtLossDate" runat="server"></asp:TextBox>&nbsp; Loss To Date&nbsp;
                                        <asp:TextBox ID="txtLossToDate" runat="server"></asp:TextBox></td>
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
                                        <asp:DropDownList ID="ddlRiskType" runat="server">
                                        </asp:DropDownList>
                                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                                        <asp:CheckBox ID="chkInformationOnly" runat="server" Text="Information" /></td>
                                </tr>
                                <tr>
                                    <td style="width: 128px; height: 21px">
                                        <strong>Currency</strong></td>
                                    <td style="width: 80%">
                                        <asp:DropDownList ID="ddlCurrency" runat="server">
                                        </asp:DropDownList></td>
                                </tr>
                            </table>
                            </asp:Panel>
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
                                        Address</td>
                                    <td style="width: 370px; height: 9px">
                                        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 203px">
                                        Telephone Number<asp:TextBox ID="TextBox3" runat="server"></asp:TextBox></td>
                                    <td style="width: 370px">
                                        Fax Number &nbsp;
                                        <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 203px">
                                    </td>
                                    <td style="width: 370px">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 203px">
                                    </td>
                                    <td style="width: 370px">
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
        </table>--%>
        </div>
    </form>
</body>
</html>
