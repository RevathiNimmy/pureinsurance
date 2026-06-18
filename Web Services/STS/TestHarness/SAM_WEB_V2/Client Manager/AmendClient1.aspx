<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AmendClient1.aspx.vb" Inherits="Client_Manager_AmendClient1" %>
<%@ Register Src="../UserControl/Header.ascx" TagName="Header" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/Footer.ascx" TagName="Footer" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <script language="javascript" type="text/javascript">
    
    function LoadWindows(url)
    {
    window.open(url, "win", "width=600 ,height=650, resizable=1, scrollbars=1, status=1")
       }
    
function TABLE2_onclick() {

}

    </script>

    <link href="../Images/loginstylesheet.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/GlobalTheme/GlobalStyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/GlobalTheme/GlobalStyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/GlobalTheme/GlobalStyleSheet.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table width="100%">
                <tr>
                    <td>
                        <uc2:Header ID="Header1" runat="server" />
                    </td>
                </tr>
            </table>
            <asp:Menu ID="Menu1" runat="server" BackColor="#FFFBD6" DynamicHorizontalOffset="2"
                Font-Names="Verdana" Font-Size="0.8em" ForeColor="#990000" Orientation="Horizontal"
                StaticSubMenuIndent="10px">
                <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                <DynamicHoverStyle BackColor="#990000" ForeColor="White" />
                <DynamicMenuStyle BackColor="#FFFBD6" />
                <StaticSelectedStyle BackColor="#FFCC66" />
                <DynamicSelectedStyle BackColor="#FFCC66" />
                <DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                <Items>
                    <asp:MenuItem Text="1 - Identity" Value="0"></asp:MenuItem>
                    <asp:MenuItem Text="2- Contacts" Value="1"></asp:MenuItem>
                    <asp:MenuItem Text="3 - Additions" Value="2"></asp:MenuItem>
                    <asp:MenuItem Text="4-Convictions" Value="3"></asp:MenuItem>
                    <asp:MenuItem Text="5-Life Style" Value="4"></asp:MenuItem>
                    <asp:MenuItem Text="6-Misc" Value="5"></asp:MenuItem>
                    <asp:MenuItem Text="7-Prospecting" Value="6"></asp:MenuItem>
                    <asp:MenuItem Text="8-Tax" Value="7"></asp:MenuItem>
                </Items>
                <StaticHoverStyle BackColor="#990000" ForeColor="White" />
            </asp:Menu>
            <br />
            <asp:MultiView ID="Mv" runat="server" ActiveViewIndex="0">
                <asp:View ID="Identity" runat="server">
                    <table cellpadding="2" cellspacing="2">
                        <tr>
                            <td style="width: 50%" valign="top">
                                <table>
                                    <tr>
                                        <td>
                                            Client Code:&nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtClientCode" runat="server" ReadOnly="True"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <% If Session("CLIENTTYPE") = "PC" Then%>
                                    <tr>
                                        <td>
                                            <strong>Last Name:</strong>&nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLastName" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvLastName" runat="server" ErrorMessage="*" ControlToValidate="txtLastName"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <strong>ForeName:&nbsp;</strong></td>
                                        <td>
                                            <asp:TextBox ID="txtForeName" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvForeName" runat="server" ErrorMessage="*" ControlToValidate="txtForeName"> </asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <strong>Title:&nbsp;</strong></td>
                                        <td>
                                            <asp:DropDownList ID="ddlTitle" runat="server">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <strong>Initials:&nbsp;</strong></td>
                                        <td>
                                            <asp:TextBox ID="txtInitial" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvInitial" runat="server" ErrorMessage="*" ControlToValidate="txtInitial"> </asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <% End If%>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblTradingName" runat="server" Text="Trading Name"></asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="txtTradingName" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <% If Session("CLIENTTYPE") = "CC" Then%>
                                    <tr>
                                        <td>
                                            Company Reg:&nbsp;</td>
                                        <td>
                                            <asp:TextBox ID="txtCompanyReg" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Main Contact:</td>
                                        <td>
                                            <asp:TextBox ID="txtMainContact" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <% End If%>
                                </table>
                            </td>
                            <td style="width: 50%" valign="top">
                                <table>
                                    <tr>
                                        <td>
                                            Account Balance:&nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAccbalance" runat="server" Enabled="False"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Year to Date Turnover:&nbsp;</td>
                                        <td>
                                            <asp:TextBox ID="txtYearTodateTurnOver" runat="server" Enabled="False"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Last Year Turnover:&nbsp;</td>
                                        <td>
                                            <asp:TextBox ID="txtLastYearturnOver" runat="server" Enabled="False"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsprospect" runat="server" Text="Is Prospect?" /></td>
                                        <td>
                                            <asp:CheckBox ID="chkIsagent" runat="server" Text="Is Agent?" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" valign="top">
                                <br />
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 50%" valign="top">
                                <table>
                                    <tr>
                                        <td>
                                            Alternative Identifier:&nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAlternativeIdentifier" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Service Level:&nbsp;</td>
                                        <td>
                                            <asp:DropDownList ID="ddServiceLevel" runat="server">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Branch:&nbsp;</td>
                                        <td>
                                            <asp:DropDownList ID="ddBranch" runat="server">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Sub Branch:&nbsp;</td>
                                        <td>
                                            <asp:DropDownList ID="ddSubBranch" runat="server">
                                            </asp:DropDownList></td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 50%" valign="top">
                                <table>
                                    <tr>
                                        <td>
                                            <strong>Lead Agent:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</strong>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnLeadAgentCode" runat="server" Text="Code..." OnClientClick="javascript:LoadWindows('AmendClient-FindAgent.aspx')" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLeadAgentCode" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Name:&nbsp;</td>
                                        <td>
                                            <asp:TextBox ID="txtLeadAgentName" runat="server"></asp:TextBox></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" valign="top">
                                <br />
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 50%" valign="top">
                                <% If Session("CLIENTTYPE") = "PC" Then%>
                                <table>
                                    <tr>
                                        <td>
                                            Area:&nbsp;
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlArea" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            File code:&nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtFileCode" runat="server"></asp:TextBox></td>
                                    </tr>
                                </table>
                                <% End If%>
                                <% If Session("CLIENTTYPE") = "CC" Then%>
                                <table>
                                    <tr>
                                        <td>
                                            <strong>Business Details:</strong></td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Business:&nbsp;</td>
                                        <td>
                                            <asp:DropDownList ID="ddlBusiness" runat="server">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Trade:&nbsp;</td>
                                        <td>
                                            <asp:DropDownList ID="ddlTrade" runat="server">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            SIC code:&nbsp;</td>
                                        <td>
                                            <asp:DropDownList ID="ddlSICcode" runat="server">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Trading since:&nbsp;</td>
                                        <td>
                                            <asp:TextBox ID="txtTradingSince" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            No. of Offices:</td>
                                        <td>
                                            <asp:TextBox ID="txtNoOfOffices" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            No. of Emplyees:</td>
                                        <td>
                                            <asp:DropDownList ID="ddlNoOfEmployees" runat="server">
                                            </asp:DropDownList></td>
                                    </tr>
                                </table>
                                <% End If%>
                            </td>
                            <td style="width: 50%" valign="top">
                                <table>
                                    <tr>
                                        <td>
                                            <strong>Account Executive:&nbsp;</strong>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnAccountExecutive" runat="server" Text="Code..." OnClientClick="javascript:LoadWindows('AmendClient-AccEcecutive.aspx')" /></td>
                                        <td>
                                            <asp:TextBox ID="txtAccExecutiveCode" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Name:&nbsp;</td>
                                        <td>
                                            <asp:TextBox ID="txtAccExecutiveName" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <asp:HiddenField ID="hdAccExecutiveCode" runat="server" />
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="hdLeadAgent" runat="server" />
                </asp:View>
                <asp:View ID="Contacts" runat="server">
                    <br />
                    <asp:Panel ID="Panel2" runat="server" Height="50px" Width="360px" GroupingText="Address">
                        <asp:GridView ID="GVContacts" runat="server" Height="48px" Width="584px" AutoGenerateColumns="False">
                            <Columns>
                                <asp:CommandField ShowSelectButton="True" />
                                <asp:CommandField ShowDeleteButton="True" />
                                <asp:BoundField DataField="AddressTypeCode" HeaderText="AddressTypeCode" />
                                <asp:BoundField DataField="AddressLine2" HeaderText="AddressLine2" />
                                <asp:BoundField DataField="AddressLine3" HeaderText="AddressLine3" />
                                <asp:BoundField DataField="PostCode" HeaderText="PostCode" />
                                <asp:BoundField DataField="AddressLine1" HeaderText="AddressLine1" />
                                <asp:BoundField DataField="AddressLine4" HeaderText="AddressLine4" />
                                <asp:BoundField DataField="CountryCode" HeaderText="CountryCode" />                                
                            </Columns>
                        </asp:GridView>
                        <asp:Button ID="btnaddaddress" runat="server" Text="Add" /></asp:Panel>
                    <br />
                    <asp:Panel ID="Panel3" runat="server" Height="50px" Width="536px" GroupingText="Contacts">
                        <asp:GridView AutoGenerateColumns="false" ID="GVContacts1" runat="server" Width="584px">
                            <Columns>
                                <asp:CommandField ShowSelectButton="True" />
                                <asp:BoundField DataField="AreaCode" HeaderText="AreaCode" />
                                <asp:BoundField DataField="Number" HeaderText="Number" />
                                <asp:BoundField DataField="Extension" HeaderText="Extension" />
                                <asp:BoundField DataField="Type" HeaderText="Type" />
                                <asp:BoundField DataField="Description" HeaderText="Description" />
                            </Columns>
                        </asp:GridView>
                        <asp:Button ID="btnaddcontacts" runat="server" Text="Add" />
                    </asp:Panel>
                    <br />
                    <asp:Panel ID="pnlcorrespondence" runat="server" Height="50px" Width="360px" BorderColor="DarkGray"
                        BorderWidth="0" GroupingText="Correspondence">
                        <table id="TABLE2" onclick="return TABLE2_onclick()">
                            <tr>
                                <td>
                                    Salutation</td>
                                <td>
                                    <asp:TextBox ID="txtsalutation" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>
                                    Prefered Correspondence</td>
                                <td>
                                    &nbsp;<asp:DropDownList ID="ddlPreferedCorres" runat="server">
                                    </asp:DropDownList></td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Is TPS</td>
                                <td>
                                    &nbsp;<asp:CheckBox ID="chkTPS" runat="server" /></td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Is MPS</td>
                                <td>
                                    &nbsp;<asp:CheckBox ID="chkMPS" runat="server" /></td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Is eMPS</td>
                                <td>
                                    &nbsp;<asp:CheckBox ID="chkeMPS" runat="server" /></td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <br />
                    <asp:Panel ID="pnlAddAddress" Visible="false" runat="server" Height="50px" Width="592px"
                        BorderColor="DarkGray" BorderWidth="0" GroupingText="Address">
                        <table style="width: 680px; height: 64px">
                            <tr>
                                <td style="width: 206px; height: 24px;">
                                    Reference</td>
                                <td style="width: 247px; height: 24px;">
                                    <asp:Label ID="lblref" runat="server"></asp:Label></td>
                                <td style="width: 247px; height: 24px;">
                                    Postcode</td>
                                <td style="width: 247px; height: 24px;">
                                    <asp:Label ID="lblpostcode" runat="server"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="width: 206px; height: 24px;">
                                    Type</td>
                                <td style="width: 247px; height: 24px;">
                                    &nbsp;<asp:DropDownList ID="ddlconType" runat="server">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td style="width: 206px; height: 26px;">
                                    Street Name
                                </td>
                                <td style="width: 247px; height: 26px;">
                                    <asp:TextBox ID="txtstname" runat="server"></asp:TextBox></td>
                                <td style="width: 247px; height: 26px;">
                                    Locality</td>
                                <td style="width: 247px; height: 26px;">
                                    <asp:TextBox ID="txtlocality" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td style="width: 206px; height: 26px;">
                                    Post Town</td>
                                <td style="width: 247px; height: 26px;">
                                    <asp:TextBox ID="txtposttown" runat="server"></asp:TextBox></td>
                                <td style="width: 247px; height: 26px;">
                                    county</td>
                                <td style="width: 247px; height: 26px;">
                                    <asp:TextBox ID="txtcounty" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td style="width: 206px; height: 26px;">
                                    Post code</td>
                                <td style="width: 247px; height: 26px;">
                                    <asp:TextBox ID="txtpostcode" runat="server"></asp:TextBox></td>
                                <td style="width: 247px; height: 26px;">
                                    country</td>
                                <td style="width: 247px; height: 26px;">
                                    &nbsp;<asp:DropDownList ID="ddlconCountry" runat="server">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnaok" runat="server" Text="Ok" />
                                    <asp:Button ID="btnacancel" runat="server" Text="Cancel" /></td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <br />
                    <asp:Panel ID="plnAddContact" Visible="false" runat="server" Height="50px" Width="592px"
                        BorderColor="DarkGray" BorderWidth="0" GroupingText="Contact">
                        <table style="width: 680px; height: 64px">
                            <tr>
                                <td style="width: 206px; height: 24px;">
                                    Reference</td>
                                <td style="width: 247px; height: 24px;">
                                    <asp:Label ID="lblcref" runat="server"></asp:Label></td>
                                <td style="width: 247px; height: 24px;">
                                    Postcode</td>
                                <td style="width: 247px; height: 24px;">
                                    <asp:Label ID="lblcpostcode" runat="server"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="width: 206px; height: 26px;">
                                    Contact
                                </td>
                                <td style="width: 247px; height: 26px;">
                                    &nbsp;<asp:DropDownList ID="ddlconContact" runat="server">
                                    </asp:DropDownList></td>
                                <td style="width: 247px; height: 26px;">
                                    Description</td>
                                <td style="width: 247px; height: 26px;">
                                    <asp:TextBox ID="txtdescription" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td style="width: 206px; height: 26px;">
                                    Areacode</td>
                                <td style="width: 247px; height: 26px;">
                                    <asp:TextBox ID="txtareacode" runat="server"></asp:TextBox></td>
                                <td style="width: 247px; height: 26px;">
                                    Number</td>
                                <td style="width: 247px; height: 26px;">
                                    <asp:TextBox ID="txtNumber" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td style="width: 206px; height: 26px;">
                                    Extension</td>
                                <td style="width: 247px; height: 26px;">
                                    <asp:TextBox ID="txtextension" runat="server"></asp:TextBox></td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btncok" runat="server" Text="Ok" />
                                    <asp:Button ID="btnccancel" runat="server" Text="Cancel" /></td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:View>
                <asp:View ID="Additions" runat="server">
                    <br />
                    <table cellpadding="3" cellspacing="3">
                        <tr>
                            <td>
                                Currency</td>
                            <td>
                                <asp:DropDownList ID="ddlCurrency" runat="server">
                                </asp:DropDownList></td>
                            <td>
                                Terms of Payement</td>
                            <td>
                                <asp:DropDownList ID="ddlTermsOfPayment" runat="server">
                                    <asp:ListItem>--Select--</asp:ListItem>
                                    <asp:ListItem>30 days</asp:ListItem>
                                    <asp:ListItem>60 days</asp:ListItem>
                                    <asp:ListItem>90 days</asp:ListItem>
                                    <asp:ListItem>120 days</asp:ListItem>
                                    <asp:ListItem>150 days</asp:ListItem>
                                    <asp:ListItem>180 days</asp:ListItem>
                                </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td>
                                Payment Method
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlPaymentMethod" runat="server">
                                </asp:DropDownList></td>
                            <td>
                                Renewal Stop Code</td>
                            <td>
                                <asp:DropDownList ID="ddlrenewaldtopcode" runat="server">
                                </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td>
                                Reminder Type</td>
                            <td>
                                <asp:DropDownList ID="ddlReminderType" runat="server">
                                </asp:DropDownList></td>
                            <td>
                                Source</td>
                            <td>
                                <asp:TextBox ID="txtsource" runat="server"></asp:TextBox></td>
                        </tr>
                        <% If Session("CLIENTTYPE") = "CC" Then%>
                        <tr>
                            <td>
                                Loyalty number
                            </td>
                            <td>
                                <asp:TextBox ID="txtLoyaltyNumber" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                Seasonal gift
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSeasonalGift" runat="server">
                                </asp:DropDownList></td>
                        </tr>
                        <% End If%>
                        <tr>
                            <td colspan="4">
                                <br />
                                <hr />
                            </td>
                        </tr>
                        <% If Session("CLIENTTYPE") = "PC" Then%>
                        <tr>
                            <td colspan="2">
                                <strong>Employer</strong></td>
                            <td colspan="2">
                                <strong>Secondary Employer</strong></td>
                        </tr>
                        <tr>
                            <td>
                                Occupation</td>
                            <td>
                                <asp:DropDownList ID="ddlPriOccupation" runat="server">
                                </asp:DropDownList></td>
                            <td>
                                Occupation</td>
                            <td>
                                <asp:DropDownList ID="ddlSecOccupation" runat="server">
                                </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td>
                                Employer's Business</td>
                            <td>
                                <asp:DropDownList ID="ddlPriEmpBusiness" runat="server">
                                </asp:DropDownList></td>
                            <td>
                                Employer's Business</td>
                            <td>
                                <asp:DropDownList ID="ddlSecEmpsBusiness" runat="server">
                                </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td>
                                Status</td>
                            <td>
                                <asp:DropDownList ID="ddlPriStatus" runat="server">
                                </asp:DropDownList></td>
                            <td>
                                Status</td>
                            <td>
                                <asp:DropDownList ID="ddlSecStatus" runat="server">
                                </asp:DropDownList></td>
                        </tr>
                        <% End If%>
                        <% If Session("CLIENTTYPE") = "CC" Then%>
                        <tr>
                            <td colspan="4">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Wageroll</td>
                            <td>
                                <asp:TextBox ID="txtwage" runat="server"></asp:TextBox></td>
                            <td>
                                Turnover</td>
                            <td>
                                <asp:DropDownList ID="ddlturnover" runat="server">
                                </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                                Financial year</td>
                            <td>
                                <asp:TextBox ID="txtfinancial" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <br />
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Area</td>
                            <td>
                                <asp:DropDownList ID="ddlAdditionsArea" runat="server">
                                </asp:DropDownList></td>
                            <td>
                                File code</td>
                            <td>
                                <asp:TextBox ID="txtAdditionsFileCode" runat="server"></asp:TextBox></td>
                        </tr>
                        <% End If%>
                        <tr>
                            <td colspan="4">
                                <br />
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnAssosicates" runat="server" Text="Associates" /></td>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:Panel ID="pnlAddAssociates" runat="server" Visible="False" Width="560px" BorderColor="DarkGray"
                        BorderWidth="0" GroupingText="Associates">
                        <asp:GridView AutoGenerateColumns="False" ID="gvassociates" runat="server" Width="552px">
                            <Columns>
                                <asp:CommandField ShowSelectButton="True" />
                                <asp:CommandField ShowDeleteButton="True" />
                                <asp:BoundField HeaderText="RelationshipCode" DataField="RelationshipCode" />
                                <asp:BoundField HeaderText="RelationshipDescription" DataField="RelationshipDescription" />
                                <asp:BoundField HeaderText="ClientKey" DataField="ClientKey" />
                                <asp:BoundField HeaderText="AssociateKey" DataField="AssociateKey" />
                                <asp:BoundField HeaderText="AssociateName" DataField="AssociateName" />
                            </Columns>
                        </asp:GridView>
                        <asp:Button ID="btnNewAssociates" runat="server" Text="New" />
                        &nbsp;<br />
                        <br />
                        &nbsp;<br />
                        <asp:Panel ID="pnlNewAssociates" runat="server" Visible="False" Width="472px" GroupingText="Associates">
                            <br />
                            <asp:Button ID="btnAssClient" runat="server" Text="Client..." OnClientClick="javascript:LoadWindows('AmendClient-Associate.aspx')"
                                Font-Bold="True" />
                            &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp;
                            <asp:TextBox ID="txtclient" runat="server" ReadOnly="True"></asp:TextBox><br />
                            <br />
                            RelationShip Desc:
                            <asp:DropDownList ID="ddlrelationshipcode" runat="server">
                            </asp:DropDownList><br />
                            <br />
                            <asp:Button ID="btnNewAssociatesOk" runat="server" Text="Ok" />
                            <asp:Button ID="btnNewAssociatesCancel" runat="server" Text="Cancel" />
                            <asp:HiddenField ID="hdAssociateKey" runat="server" />
                        </asp:Panel>
                    </asp:Panel>
                </asp:View>
                <asp:View ID="Covictions" runat="server">
                    <asp:Panel ID="pnlConvictions" runat="server" Width="544px" GroupingText="Convictions">
                        <asp:GridView ID="gvConcictions" AutoGenerateColumns="False" runat="server" Width="536px">
                            <Columns>
                                <asp:CommandField ShowSelectButton="True" />
                                <asp:CommandField ShowDeleteButton="True" />
                                <asp:BoundField HeaderText="SentenceTypeCode" DataField="SentenceTypeCode" />
                                <asp:BoundField HeaderText="SentenceDuration" DataField="SentenceDuration" />
                                <asp:BoundField HeaderText="TypeCode" DataField="TypeCode" />
                                <asp:BoundField HeaderText="DrivingLicensePenaltyPoints" DataField="DrivingLicensePenaltyPoints" />
                                <asp:BoundField HeaderText="SentenceEffectiveDate" DataField="SentenceEffectiveDate" />
                                <asp:BoundField HeaderText="FineAmount" DataField="FineAmount" />
                                <asp:BoundField HeaderText="Date" DataField="Date" />
                                <asp:BoundField HeaderText="ConvictionKey" DataField="ConvictionKey" />
                                <asp:BoundField HeaderText="AlcoholMeasurementMethod" DataField="AlcoholMeasurementMethod" />
                                <asp:BoundField HeaderText="SentenceDurationQualifier" DataField="SentenceDurationQualifier" />
                                <asp:BoundField HeaderText="SentenceDescription" DataField="SentenceDescription" />
                                <asp:BoundField HeaderText="Description" DataField="Description" />
                                <asp:BoundField HeaderText="AlcoholLevel" DataField="AlcoholLevel" />
                                <asp:BoundField HeaderText="StatusCode" DataField="StatusCode" />
                            </Columns>
                        </asp:GridView>
                        <asp:Button ID="btnAdd" runat="server" Text="Add" />
                        &nbsp;
                    </asp:Panel>
                    <br />
                    County court judgements:&nbsp;<asp:TextBox ID="txtCountyCourtJudge" runat="server"></asp:TextBox><br />
                    <br />
                    <asp:Panel ID="pnlAddConvictions" runat="server" Visible="false" Width="552px" BorderColor="DarkGray"
                        BorderWidth="0" GroupingText="Convictions">
                        <table cellpadding="2" cellspacing="2">
                            <tr>
                                <td>
                                    <br />
                                    <br />
                                    <b>Convictions Details</b><br />
                                    <table cellpadding="2" cellspacing="2">
                                        <tr>
                                            <td>
                                                <strong>Type</strong></td>
                                            <td>
                                                <asp:DropDownList ID="ddlconvictiontype" runat="server">
                                                </asp:DropDownList></td>
                                            <td>
                                                Fine</td>
                                            <td>
                                                <asp:TextBox ID="txtcfine" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <strong>Status</strong></td>
                                            <td>
                                                <asp:DropDownList ID="ddlconvictionstatus" runat="server">
                                                </asp:DropDownList></td>
                                            <td>
                                                <strong>Date</strong></td>
                                            <td>
                                                <asp:TextBox ID="txtcdate" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtcdate"
                                                    ErrorMessage="*"></asp:RequiredFieldValidator></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <strong>Description</strong></td>
                                            <td>
                                                <asp:TextBox ID="txtcdescription" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtcdescription"
                                                    ErrorMessage="*"></asp:RequiredFieldValidator></td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <hr />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Sentence:</b></td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Type</td>
                                            <td>
                                                <asp:DropDownList ID="ddlsentencetype" runat="server">
                                                </asp:DropDownList></td>
                                            <td>
                                                Date</td>
                                            <td>
                                                <asp:TextBox ID="txtsdate" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Description</td>
                                            <td>
                                                <asp:TextBox ID="txtsdescription" runat="server"></asp:TextBox></td>
                                            <td>
                                                Duration</td>
                                            <td>
                                                <asp:TextBox ID="txtsduration" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                Time unit</td>
                                            <td>
                                                <asp:DropDownList ID="ddlsentencetime" runat="server">
                                                </asp:DropDownList></td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <hr />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Motoring Related:</b></td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Alcohol Measurement Method</td>
                                            <td>
                                                <asp:DropDownList ID="ddlAlcoholmethod" runat="server">
                                                </asp:DropDownList></td>
                                            <td>
                                                Alcohol Level</td>
                                            <td>
                                                <asp:TextBox ID="txtalcohollevel" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                Penality point</td>
                                            <td>
                                                <asp:TextBox ID="txtpenality" runat="server"></asp:TextBox></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnOk" runat="server" Text="OK" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:View>
                <asp:View ID="LifeStyle" runat="server">
                    <% If Session("CLIENTTYPE") = "PC" Then%>
                    <asp:Panel ID="pnlLifeStyle" runat="server" Width="216px" GroupingText="Personal Details">
                        <table cellpadding="2" cellspacing="2" border="0">
                            <tr>
                                <td>
                                    <br />
                                    <table cellpadding="2" cellspacing="2" border="0">
                                        <tr>
                                            <td>
                                                DOB</td>
                                            <td>
                                                <asp:TextBox ID="txtDobirth" runat="server">
                                                </asp:TextBox></td>
                                            <td>
                                                Gender</td>
                                            <td>
                                                <asp:DropDownList ID="ddgender" runat="server">
                                                </asp:DropDownList></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Marital Status</td>
                                            <td>
                                                <asp:DropDownList ID="ddmaritalstatus" runat="server">
                                                </asp:DropDownList></td>
                                            <td>
                                                Nationality</td>
                                            <td>
                                                <asp:DropDownList ID="ddnationality" runat="server">
                                                </asp:DropDownList></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Seasonal Gift</td>
                                            <td>
                                                <asp:DropDownList ID="ddseasonalgift" runat="server">
                                                </asp:DropDownList></td>
                                            <td>
                                                Accomodation</td>
                                            <td>
                                                <asp:DropDownList ID="ddlAccomodation" runat="server">
                                                    <asp:ListItem>--Select--</asp:ListItem>
                                                    <asp:ListItem>Borrower - to be deleted AUG98</asp:ListItem>
                                                    <asp:ListItem>Co-Owner</asp:ListItem>
                                                    <asp:ListItem>Credit Sale</asp:ListItem>
                                                    <asp:ListItem>Debenture Holder</asp:ListItem>
                                                    <asp:ListItem>First Mortgagee</asp:ListItem>
                                                    <asp:ListItem>Freeholder</asp:ListItem>
                                                    <asp:ListItem>Ground Landlord</asp:ListItem>
                                                    <asp:ListItem>Heritable Creditor - Primo Loco</asp:ListItem>
                                                    <asp:ListItem>Heritable Creditor - Secundo Loco</asp:ListItem>
                                                    <asp:ListItem>Heritable Creditor - Tertio Loco</asp:ListItem>
                                                    <asp:ListItem>Hire Purchase</asp:ListItem>
                                                    <asp:ListItem>Hirer</asp:ListItem>
                                                    <asp:ListItem>Holder Of A Floating Charge</asp:ListItem>
                                                    <asp:ListItem>Landlord</asp:ListItem>
                                                    <asp:ListItem>Leaseholder</asp:ListItem>
                                                    <asp:ListItem>Lessee</asp:ListItem>
                                                    <asp:ListItem>Lessor</asp:ListItem>
                                                    <asp:ListItem>Mortgagee</asp:ListItem>
                                                    <asp:ListItem>Mortgagee Of The Leaseholder Interest</asp:ListItem>
                                                    <asp:ListItem>Mortgagor</asp:ListItem>
                                                    <asp:ListItem>Other Lender</asp:ListItem>
                                                    <asp:ListItem>Other Occupier</asp:ListItem>
                                                    <asp:ListItem>Owner</asp:ListItem>
                                                    <asp:ListItem>Proprietor In Reversion</asp:ListItem>
                                                    <asp:ListItem>Registered Keeper</asp:ListItem>
                                                    <asp:ListItem>Second Mortgagee</asp:ListItem>
                                                    <asp:ListItem>Standard Bank Interest</asp:ListItem>
                                                    <asp:ListItem>Tenant</asp:ListItem>
                                                    <asp:ListItem>ZZ - Not Covered By Any Other Code</asp:ListItem>
                                                </asp:DropDownList>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Loyalty number</td>
                                            <td>
                                                <asp:TextBox ID="txtloyalty" runat="server">
                                                </asp:TextBox></td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Pets</td>
                                            <td>
                                                <asp:CheckBox ID="chkPets" runat="server" /></td>
                                            <td>
                                                Smoker</td>
                                            <td>
                                                <asp:CheckBox ID="chksmoker" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <hr />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Dependents
                                    <asp:GridView AutoGenerateColumns="False" ID="gvDependents" runat="server" Width="488px">
                                        <Columns>
                                            <asp:CommandField ShowSelectButton="True" />
                                            <asp:CommandField ShowDeleteButton="True" />
                                            <asp:BoundField DataField="SecOccupationCode" HeaderText="SecOccupationCode" />
                                            <asp:BoundField DataField="OccupationCode" HeaderText="OccupationCode" />
                                            <asp:BoundField DataField="CategoryCode" HeaderText="CategoryCode" />
                                            <asp:BoundField DataField="LifestyleKey" HeaderText="LifestyleKey" />
                                            <asp:BoundField DataField="Name" HeaderText="Name" />
                                            <asp:BoundField DataField="Smoker" HeaderText="Smoker" />
                                            <asp:BoundField DataField="DateOfBirth" HeaderText="DateOfBirth" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnLSAdd" runat="server" Text="Add" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <br />
                    <asp:Panel ID="pnlAddDependents" runat="server" Height="72px" Visible="False" Width="512px"
                        BorderColor="DarkGray" BorderWidth="0" GroupingText="General">
                        <br />
                        <table cellpadding="2" cellspacing="2" border="0">
                            <tr>
                                <td>
                                    Name</td>
                                <td>
                                    <asp:TextBox ID="txtName" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>
                                    DateOfBirth</td>
                                <td>
                                    <asp:TextBox ID="txtDOB" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Category</td>
                                <td>
                                    <asp:DropDownList ID="ddlcategory" runat="server">
                                        <asp:ListItem></asp:ListItem>
                                        <asp:ListItem Value="0">Male</asp:ListItem>
                                        <asp:ListItem Value="1">Female</asp:ListItem>
                                        <asp:ListItem Value="NA">Not Applicable</asp:ListItem>
                                        <asp:ListItem Value="NAV">Not Available</asp:ListItem>
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td>
                                    Gender Code</td>
                                <td>
                                    <asp:DropDownList ID="ddlGenderCode" runat="server">
                                        <asp:ListItem></asp:ListItem>
                                        <asp:ListItem Value="F">Female</asp:ListItem>
                                        <asp:ListItem Value="M">Male</asp:ListItem>
                                        <asp:ListItem Value="NAV">Not available</asp:ListItem>
                                        <asp:ListItem Value="NA">Not applicable</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Occupation</td>
                                <td>
                                    <asp:DropDownList ID="ddloccupationcode" runat="server">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td>
                                    Sec Occupation
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlsecoccupationcode" runat="server">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td>
                                    Is smoker</td>
                                <td>
                                    <asp:CheckBox ID="chkIssmoker" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
                        <asp:Button ID="btnLSOk" runat="server" Text="OK" />
                        <asp:Button ID="btnLSCancel" runat="server" Text="Cancel" /></asp:Panel>
                    <% End If%>
                </asp:View>
                <asp:View ID="Misc" runat="server">
                    <asp:Panel ID="pnlLoyaltySchemes" runat="server" Height="50px" Width="125px" GroupingText="Loyalty Schemes">
                        <table style="width: 488px">
                            <tr>
                                <td>
                                    <asp:GridView ID="gvloyalty" AutoGenerateColumns="False" runat="server" Width="488px">
                                        <Columns>
                                            <asp:CommandField ShowSelectButton="True" />
                                            <asp:CommandField ShowDeleteButton="True" />
                                            <asp:BoundField DataField="MainMember" HeaderText="MainMember" />
                                            <asp:BoundField DataField="OtherReference" HeaderText="OtherReference" />
                                            <asp:BoundField DataField="EndDate" HeaderText="EndDate" />
                                            <asp:BoundField DataField="Active" HeaderText="Active" />
                                            <asp:BoundField DataField="StartDate" HeaderText="StartDate" />
                                            <asp:BoundField DataField="LoyaltySchemeKey" HeaderText="LoyaltySchemeKey" />
                                            <asp:BoundField DataField="LoyaltySchemeCode" HeaderText="LoyaltySchemeCode" />
                                            <asp:BoundField DataField="MembershipNumber" HeaderText="MembershipNumber" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnLoyaltyAdd" runat="server" Text="Add" />&nbsp;
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <br />
                    <asp:Panel ID="pnlAddLoyaltySchemes" runat="server" Height="50px" Visible="False"
                        Width="125px" BorderColor="DarkGray" BorderWidth="0">
                        <asp:Panel ID="Panel6" runat="server" Height="50px" Width="496px" GroupingText="Loyalty Scheme Membership">
                            <table style="width: 496px">
                                <tr>
                                    <td style="width: 193px">
                                        Loyalty Schemes</td>
                                    <td>
                                        <asp:DropDownList ID="ddlLoyaltySchemes" runat="server">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td style="width: 193px; height: 21px">
                                        <strong>MemberShip number</strong></td>
                                    <td style="height: 21px">
                                        <asp:TextBox ID="txtmembership" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtmembership"
                                            ErrorMessage="*"></asp:RequiredFieldValidator></td>
                                </tr>
                                <tr>
                                    <td style="width: 193px">
                                        other Reference</td>
                                    <td>
                                        <asp:TextBox ID="txtotherref" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 193px; height: 26px;">
                                        <strong>Start Date</strong></td>
                                    <td style="height: 26px">
                                        <asp:TextBox ID="txtstart" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtstart"
                                            ErrorMessage="*"></asp:RequiredFieldValidator></td>
                                </tr>
                                <tr>
                                    <td style="width: 193px; height: 26px;">
                                        End Date</td>
                                    <td style="height: 26px">
                                        <asp:TextBox ID="txtend" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 193px">
                                        Main Member</td>
                                    <td>
                                        <asp:TextBox ID="txtMain" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 193px; height: 26px;">
                                        IS Active</td>
                                    <td style="height: 26px">
                                        &nbsp;<asp:CheckBox ID="chkActive" runat="server" /></td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Button ID="btnAddLoyaltyAdd" runat="server" Text="OK" />
                        <asp:Button ID="btnAddLoyaltyCancel" runat="server" Text="Cancel" CausesValidation="False" /></asp:Panel>
                </asp:View>
                <asp:View ID="Prospecting" runat="server">
                    <table>
                        <tr>
                            <td>
                                <table cellpadding="2" cellspacing="2">
                                    <tr>
                                        <td>
                                            Agent
                                            <br />
                                            Reference&nbsp;</td>
                                        <td>
                                            &nbsp;<asp:TextBox ID="txtagentref" runat="server"></asp:TextBox></td>
                                        <td>
                                            Strength Code
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlprosStrengthCode" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnCurrentAgent" runat="server" OnClientClick="javascript:LoadWindows('AmendClient-CurrentAgent.aspx')"
                                                Text="Current agent..." />&nbsp;</td>
                                        <td>
                                            <asp:TextBox ID="txtcurrentagent" runat="server"></asp:TextBox></td>
                                        <td>
                                            Status
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlprosStatus" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <br />
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Previous Insurer</td>
                                        <td>
                                        </td>
                                        <td>
                                            Previous Broker</td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnPrevInsurer" runat="server" OnClientClick="javascript:LoadWindows('AmendClient-PrevInsurer.aspx')"
                                                Text="Code..." /></td>
                                        <td>
                                            <asp:TextBox ID="txtPIcode" runat="server"></asp:TextBox></td>
                                        <td>
                                            <asp:Button ID="btnPrevBroker" runat="server" OnClientClick="javascript:LoadWindows('AmendClient-PrevBroker.aspx')"
                                                Text="Code..." /></td>
                                        <td>
                                            <asp:TextBox ID="txtPBcode" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Name</td>
                                        <td>
                                            <asp:TextBox ID="txtPIname" runat="server"></asp:TextBox></td>
                                        <td>
                                            Name</td>
                                        <td>
                                            <asp:TextBox ID="txtPBname" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <br />
                                            <hr />
                                        </td>
                                    </tr>
                                </table>
                                <asp:HiddenField ID="hdCurrentAgentKey" runat="server" />
                                <asp:HiddenField ID="hdPreviousInsurerKey" runat="server" />
                                <asp:HiddenField ID="hdPrevBrokerKey" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            Policies
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <asp:GridView AutoGenerateColumns="False" ID="gvpolicies" runat="server">
                                                <Columns>
                                                    <asp:CommandField ShowSelectButton="True" />
                                                    <asp:CommandField ShowDeleteButton="True" />
                                                    <asp:BoundField HeaderText="ProspectPolicyKey" DataField="ProspectPolicyKey" />
                                                    <asp:BoundField HeaderText="ProspectTypeCode" DataField="ProspectTypeCode" />
                                                    <asp:BoundField HeaderText="TimesQuoted" DataField="TimesQuoted" />
                                                    <asp:BoundField HeaderText="TargetPremium" DataField="TargetPremium" />
                                                    <asp:BoundField HeaderText="RenewalDate" DataField="RenewalDate" />
                                                </Columns>
                                            </asp:GridView>
                                            <table border="0">
                                                <tr>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnAddPolicies" runat="server" Text="Add" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Panel ID="pnlAddPolicies" runat="server" Visible="False" Width="420px" BorderColor="DarkGray"
                                                            BorderWidth="0" GroupingText="General" Height="200px">
                                                            <table cellpadding="2" cellspacing="2" border="0">
                                                                <tr>
                                                                    <td>
                                                                        Type</td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlprostype" runat="server">
                                                                        </asp:DropDownList></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        Renewal Date</td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtprewnal" runat="server"></asp:TextBox></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        Times Quoted</td>
                                                                    <td>
                                                                        <asp:TextBox ID="txttimequoted" runat="server"></asp:TextBox></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        Targeted premium</td>
                                                                    <td>
                                                                        <asp:TextBox ID="txttargetpremium" runat="server"></asp:TextBox></td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <asp:Button ID="btnPolicyOk" runat="server" Text="Ok" />
                                                                        <asp:Button ID="btnPolicyCancel" runat="server" Text="Cancel" /></td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="Tax" runat="server">
                    <table>
                        <tr>
                            <td>
                                Tax Number</td>
                            <td>
                                <asp:TextBox ID="txttaxno" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>
                                Is Domicile For Tax</td>
                            <td>
                                &nbsp;<asp:CheckBox ID="chkDomicileTax" runat="server" /></td>
                        </tr>
                        <tr>
                            <td>
                                Tax Exempt</td>
                            <td>
                                &nbsp;<asp:CheckBox ID="chkTaxExempt" runat="server" /></td>
                        </tr>
                        <tr>
                            <td>
                                Tax percentage</td>
                            <td>
                                <asp:TextBox ID="txtpercentage" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                </td>
                        </tr>
                        <tr>
                            <td>
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />
                            </td>
                        </tr>
                    </table>
                </asp:View>
            </asp:MultiView>
            <asp:Button ID="save" runat="server" Text="Update Party" />
                                <asp:Button ID="btnAddParty" runat="server" Text="ADD PARTY" />
            <table width="100%">
                <tr>
                    <td align="center">
                        <uc1:Footer ID="Footer1" runat="server" />
                    </td>
                </tr>
            </table>
    </div>
    </form>
</body>
</html>
