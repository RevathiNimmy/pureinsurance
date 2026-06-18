<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PolicyHeader.aspx.vb" Title="PolicyHeader" Inherits="MTA_PolicyHeader" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        &nbsp;</div>
        <table>
            <tr>
                <td style="width: 1066px; height: 41px;">
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
                <asp:MenuItem Text="Main Details" Value="0"></asp:MenuItem>
                <asp:MenuItem Text="Addition Details" Value="1"></asp:MenuItem>
                <asp:MenuItem Text="Sub-Agent Details" Value="2"></asp:MenuItem>
            </Items>
            <StaticHoverStyle BackColor="#7C6F57" ForeColor="White" />
        </asp:Menu>
                </td>
            </tr>
            <tr>
                <td style="width: 1066px; height: 517px; background-color: #FFF5EE;">
        <asp:MultiView ID="mvPolicyHeaders" runat="server">
            <asp:View ID="View1" runat="server">
                <table>
                    <tr>
                        <td style="width: 280px;">
                            Insured Name</td>
                        <td style="width: 449px;">
                            <asp:TextBox ID="txtInsuredName" runat="server" Width="200px"></asp:TextBox></td>
                        <td style="width: 75616px">
                            <asp:Label ID="Label4" runat="server" Width="40px"></asp:Label></td>
                        <td style="width: 98211px;">
                            <asp:Label ID="Label2" runat="server" Font-Names="Arial" Font-Size="8pt" Text="Alternate Reference"
                                Width="112px"></asp:Label></td>
                        <td style="width: 106990px;">
                            <asp:TextBox ID="txtAlternateRef" runat="server" Width="200px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td style="width: 280px; height: 18px;">
                            Policy No</td>
                        <td style="width: 449px; height: 18px;">
                            <asp:TextBox ID="txtPolicyNo" runat="server" Width="200px"></asp:TextBox></td>
                        <td style="width: 75616px; height: 18px">
                        </td>
                        <td style="width: 98211px; font-size: 8pt; font-family: Arial; height: 18px;">
                            Status</td>
                        <td style="width: 106990px; height: 18px;">
                            <asp:TextBox ID="txtStatus" runat="server" Width="200px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td style="width: 280px; font-size: 8pt; font-family: Arial;">
                            Product</td>
                        <td style="width: 449px;">
                            <asp:TextBox ID="txtProduct" runat="server" Width="200px"></asp:TextBox></td>
                        <td style="width: 75616px;">
                        </td>
                        <td style="width: 98211px; font-size: 8pt; font-family: Arial;">
                            Policy Status</td>
                        <td style="width: 106990px;">
                            <asp:DropDownList ID="ddlPolictStatus" runat="server" Width="200px">
                            </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td style="font-size: 8pt; width: 280px; font-family: Arial">
                            Branch Code</td>
                        <td style="width: 449px">
                            <asp:DropDownList ID="ddlBranchCode" runat="server" Width="200px">
                            </asp:DropDownList></td>
                        <td style="width: 75616px">
                        </td>
                        <td style="font-size: 8pt; width: 98211px; font-family: Arial">
                            Analysis Code</td>
                        <td style="width: 106990px">
                            <asp:DropDownList ID="ddlAnalysisCode" runat="server" Width="200px">
                            </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td style="font-size: 8pt; width: 280px; font-family: Arial">
                            Sub Branch</td>
                        <td style="width: 449px">
                            <asp:DropDownList ID="ddlSubBranch" runat="server" Width="200px">
                            </asp:DropDownList></td>
                        <td style="width: 75616px">
                        </td>
                        <td style="font-size: 8pt; width: 98211px; font-family: Arial">
                            Business Type</td>
                        <td style="width: 106990px">
                            <asp:DropDownList ID="ddlBusinessType" runat="server" Width="200px">
                            </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td style="font-size: 8pt; width: 280px; font-family: Arial">
                            Agent Code</td>
                        <td style="width: 449px"><asp:DropDownList ID="ddlAgencyCode" runat="server" Width="200px">
                        </asp:DropDownList></td>
                        <td style="width: 75616px">
                        </td>
                        <td style="font-size: 8pt; width: 98211px; font-family: Arial">
                            Currency</td>
                        <td style="width: 106990px">
                            <asp:DropDownList ID="ddlCurrency" runat="server" Width="200px">
                            </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td style="font-size: 8pt; width: 280px; font-family: Arial">
                            Handler</td>
                        <td style="width: 449px"><asp:DropDownList ID="DropDownList4" runat="server" Width="200px">
                        </asp:DropDownList></td>
                        <td style="width: 75616px">
                        </td>
                        <td style="width: 98211px">
                        </td>
                        <td style="width: 106990px">
                        </td>
                    </tr>
                    <tr>
                        <td style="font-size: 8pt; width: 280px; font-family: Arial">
                            Regarding</td>
                        <td style="width: 449px">
                            <asp:TextBox ID="txtRegarding" runat="server" Width="200px"></asp:TextBox></td>
                        <td style="width: 75616px">
                        </td>
                        <td style="width: 98211px">
                        </td>
                        <td style="width: 106990px">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" style="background-color: #cc99ff">
                        </td>
                    </tr>
                    <tr>
                        <td style="font-size: 8pt; width: 280px; font-family: Arial">
                            Cover From</td>
                        <td style="font-size: 8pt; width: 449px; font-family: Arial">
                            <asp:TextBox ID="txtcoveredFrom" runat="server" Width="200px"></asp:TextBox></td>
                        <td style="font-size: 8pt; width: 75616px; font-family: Arial">
                        </td>
                        <td style="font-size: 8pt; width: 98211px; font-family: Arial">
                            Cover To</td>
                        <td style="width: 106990px">
                            <asp:TextBox ID="txtcoveredTo" runat="server" Width="200px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td style="font-size: 8pt; width: 280px; font-family: Arial; height: 26px;">
                            Inception</td>
                        <td style="font-size: 8pt; width: 449px; font-family: Arial; height: 26px;">
                            <asp:TextBox ID="txtinceptionDate" runat="server" Width="200px"></asp:TextBox></td>
                        <td style="font-size: 8pt; width: 75616px; font-family: Arial; height: 26px">
                        </td>
                        <td style="font-size: 8pt; width: 98211px; font-family: Arial; height: 26px;">
                            Renewal</td>
                        <td style="width: 106990px; height: 26px;">
                            <asp:TextBox ID="txtRenewalDate" runat="server" Width="200px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td style="font-size: 8pt; width: 280px; font-family: Arial">
                            Inception TPI</td>
                        <td style="font-size: 8pt; width: 449px; font-family: Arial">
                            <asp:TextBox ID="txtInceptionTPI" runat="server" Width="200px"></asp:TextBox></td>
                        <td style="font-size: 8pt; width: 75616px; font-family: Arial">
                        </td>
                        <td style="font-size: 8pt; width: 98211px; font-family: Arial">
                            Issued</td>
                        <td style="width: 106990px">
                            <asp:TextBox ID="txtIssuedDate" runat="server" Width="200px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td style="font-size: 8pt; width: 280px; font-family: Arial">
                            H/C Expiry</td>
                        <td style="font-size: 8pt; width: 449px; font-family: Arial">
                            <asp:TextBox ID="txtHCExpiry" runat="server" Width="200px"></asp:TextBox></td>
                        <td style="font-size: 8pt; width: 75616px; font-family: Arial">
                        </td>
                        <td style="font-size: 8pt; width: 98211px; font-family: Arial">
                            Quote Expiry</td>
                        <td style="width: 106990px">
                            <asp:TextBox ID="txtQuoteexpiry" runat="server" Width="200px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td style="font-size: 8pt; width: 280px; font-family: Arial">
                            Policy Deductable</td>
                        <td style="font-size: 8pt; width: 449px; font-family: Arial"><asp:DropDownList ID="DropDownList1" runat="server" Width="200px">
                        </asp:DropDownList></td>
                        <td style="font-size: 8pt; width: 75616px; font-family: Arial">
                        </td>
                        <td style="font-size: 8pt; width: 98211px; font-family: Arial">
                            Policy Limits</td>
                        <td style="width: 106990px"><asp:DropDownList ID="DropDownList2" runat="server" Width="200px">
                        </asp:DropDownList></td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="View2" runat="server"><table>
                <tr>
                    <td style="width: 280px;">
                        Frequency :</td>
                    <td style="width: 449px;">
                        <asp:DropDownList ID="ddlFrequencyCode" runat="server" Width="200px">
                        </asp:DropDownList></td><td style="font-size: 8pt; width: 98211px; font-family: Arial">
                        Renewal Method :</td>
                    <td style="width: 106990px;">
                        <asp:DropDownList ID="ddlRenewalmethodCode" runat="server" Width="200px">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td style="width: 280px; height: 7px;">
                        LTU expiry :&nbsp;</td>
                    <td style="width: 449px; height: 7px;">
                        <asp:TextBox ID="TextBox3" runat="server" Width="200px"></asp:TextBox></td>
                    <td style="width: 98211px; font-size: 8pt; font-family: Arial; height: 7px;">
                        Lapse/Cancellation Reason :</td>
                    <td style="width: 106990px; height: 7px;">
                        &nbsp;<asp:DropDownList ID="ddlLapseCancellation" runat="server" Width="200px">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td style="width: 280px; font-size: 8pt; font-family: Arial;">
                        Stop Reason :</td>
                    <td style="width: 449px;">
                        <asp:DropDownList ID="ddlStopReasoCode" runat="server" Width="200px">
                        </asp:DropDownList></td>
                    <td style="width: 98211px; font-size: 8pt; font-family: Arial;">
                        Lapse/Cancellation Date :</td>
                    <td style="width: 106990px;">
                        <asp:TextBox ID="TextBox1" runat="server" Width="200px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="font-size: 8pt; width: 280px; font-family: Arial">
                        Times renewed :</td>
                    <td style="width: 449px">
                        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label></td>
                    <td style="font-size: 8pt; width: 98211px; font-family: Arial">
                        <asp:CheckBox ID="CkBxReferrredatrenewal" runat="server" Text="Referrred at renewal ?" /></td>
                    <td style="width: 106990px">
                        &nbsp;<asp:CheckBox ID="CkBxReferrredonMTA" runat="server" Text="Referred on MTA ?" /></td>
                </tr>
                <tr>
                    <td style="font-size: 8pt; width: 280px; font-family: Arial">
                        Sub Branch</td>
                    <td style="width: 449px">
                        <asp:DropDownList ID="DropDownList8" runat="server" Width="200px">
                        </asp:DropDownList></td>
                    <td style="font-size: 8pt; width: 98211px; font-family: Arial">
                        Business Type</td>
                    <td style="width: 106990px">
                        <asp:DropDownList ID="DropDownList9" runat="server" Width="200px">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td style="font-size: 8pt; width: 280px; font-family: Arial">
                        Agent Code</td>
                    <td style="width: 449px">
                        <asp:DropDownList ID="DropDownList10" runat="server" Width="200px">
                        </asp:DropDownList></td>
                    <td style="font-size: 8pt; width: 98211px; font-family: Arial">
                        Currency</td>
                    <td style="width: 106990px">
                        <asp:DropDownList ID="DropDownList11" runat="server" Width="200px">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td style="font-size: 8pt; width: 280px; font-family: Arial">
                        Handler</td>
                    <td style="width: 449px">
                        <asp:DropDownList ID="DropDownList12" runat="server" Width="200px">
                        </asp:DropDownList></td><td style="font-size: 8pt; width: 98211px; font-family: Arial">
                            Standard Policy Wording Quote:</td>
                    <td style="width: 106990px">
                        <asp:TextBox ID="txtStPolicyWording" runat="server" Width="200px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="font-size: 8pt; width: 280px; font-family: Arial">
                        Regarding</td>
                    <td style="width: 449px">
                        <asp:TextBox ID="TextBox6" runat="server" Width="200px"></asp:TextBox></td><td style="font-size: 8pt; width: 98211px; font-family: Arial">
                            Standard Policy Wording Description :</td>
                    <td style="width: 106990px">
                        <asp:TextBox ID="txtstdPolicyDesc" runat="server" Width="200px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="font-size: 8pt; width: 280px; font-family: Arial">
                    </td>
                    <td style="width: 449px">
                    </td>
                    <td style="width: 98211px">
                    </td>
                    <td style="width: 106990px">
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="background-color: #cc99ff">
                    </td>
                </tr>
                <tr>
                    <td style="font-size: 8pt; width: 280px; font-family: Arial">
                        Cover From</td>
                    <td style="font-size: 8pt; width: 449px; font-family: Arial">
                        <asp:TextBox ID="TextBox7" runat="server" Width="200px"></asp:TextBox></td>
                    <td style="font-size: 8pt; width: 98211px; font-family: Arial">
                        Cover To</td>
                    <td style="width: 106990px">
                        <asp:TextBox ID="TextBox8" runat="server" Width="200px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="font-size: 8pt; width: 280px; font-family: Arial; height: 26px;">
                        Inception</td>
                    <td style="font-size: 8pt; width: 449px; font-family: Arial; height: 26px;">
                        <asp:TextBox ID="TextBox9" runat="server" Width="200px"></asp:TextBox></td>
                    <td style="font-size: 8pt; width: 98211px; font-family: Arial; height: 26px;">
                        Renewal</td>
                    <td style="width: 106990px; height: 26px;">
                        <asp:TextBox ID="TextBox10" runat="server" Width="200px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="font-size: 8pt; width: 280px; font-family: Arial; height: 73px;">
                        Inception TPI</td>
                    <td style="font-size: 8pt; width: 449px; font-family: Arial; height: 73px;">
                        <asp:TextBox ID="TextBox11" runat="server" Width="200px"></asp:TextBox></td>
                    <td style="font-size: 8pt; width: 98211px; font-family: Arial; height: 73px;">
                        Issued</td>
                    <td style="width: 106990px; height: 73px;">
                        <asp:TextBox ID="TextBox12" runat="server" Width="200px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="font-size: 8pt; width: 280px; font-family: Arial">
                        H/C Expiry</td>
                    <td style="font-size: 8pt; width: 449px; font-family: Arial">
                        <asp:TextBox ID="TextBox13" runat="server" Width="200px"></asp:TextBox></td>
                    <td style="font-size: 8pt; width: 98211px; font-family: Arial">
                        Quote Expiry</td>
                    <td style="width: 106990px">
                        <asp:TextBox ID="TextBox14" runat="server" Width="200px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="font-size: 8pt; width: 280px; font-family: Arial">
                        Policy Deductable</td>
                    <td style="font-size: 8pt; width: 449px; font-family: Arial">
                        <asp:DropDownList ID="DropDownList13" runat="server" Width="200px">
                        </asp:DropDownList></td>
                    <td style="font-size: 8pt; width: 98211px; font-family: Arial">
                        Policy Limits</td>
                    <td style="width: 106990px">
                        <asp:DropDownList ID="DropDownList14" runat="server" Width="200px">
                        </asp:DropDownList></td>
                </tr>
            </table>
            </asp:View>
            <asp:View ID="View3" runat="server">
                <asp:Label ID="Label5" runat="server" Font-Names="Arial" Font-Size="8pt" Text="Sub Agents"
                    Width="176px"></asp:Label>
                <asp:GridView ID="GridView1" runat="server">
                </asp:GridView>
                &nbsp;&nbsp;
                <asp:Button ID="Button1" runat="server" Text="Button" /><br />
                <asp:Panel ID="Panel1" runat="server" Font-Names="Arial" Font-Size="8pt" Height="40px"
                    Width="784px">
                    Cover Note Book &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    <asp:TextBox ID="txtCoverBook" runat="server" Width="200px"></asp:TextBox>
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; Cover Note
                    Sheet &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                    <asp:TextBox ID="txtCoverSheet" runat="server" Width="200px"></asp:TextBox></asp:Panel>
            </asp:View>
        </asp:MultiView></td>
            </tr>
            <tr>
                <td style="width: 1066px">
                    <asp:Button ID="cmdOk" runat="server" Text="Ok" Width="96px" /></td>
            </tr>
        </table>
    </form>
</body>
</html>
