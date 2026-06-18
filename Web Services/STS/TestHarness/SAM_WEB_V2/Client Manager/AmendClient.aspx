<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AmendClient.aspx.vb" Inherits="MTA_AmendClient" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>

    <script language="javascript" type="text/javascript">
    
    function LoadWindows(url)
    {
    window.open(url,"","width=600" ,"height=650","scrollbars=1")
       }
    
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
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
            <asp:MultiView ID="Mv" runat="server" ActiveViewIndex="0">
                <asp:View ID="Identity" runat="server">
                    <asp:Panel ID="Panel1" runat="server" Height="50px" Width="312px">
                    <table id="MainTable">
                    <tr><td>
                            <table style="width: 168px; height: 144px">
                                                        
                                <tr>
                                    <td valign="top" style="width: 168px">
                                        Client Code</td>
                                    <td style="width: 401px">
                                        <asp:TextBox ID="txtClientCode" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 168px; height: 40px;">
                                        <strong>
                                            <asp:Label ID="lblLastName" runat="server" Font-Bold="True" Text="Last Name"></asp:Label></strong></td>
                                    <td style="width: 401px; height: 40px;">
                                        <asp:TextBox ID="txtLastName" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 168px; height: 26px;">
                                        <strong>
                                            <asp:Label ID="lblForeName" runat="server" Font-Bold="True" Text="Fore  Name"></asp:Label></strong></td>
                                    <td style="width: 401px; height: 26px;">
                                        <asp:TextBox ID="txtForeName" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 168px; height: 26px;">
                                        <strong>
                                            <asp:Label ID="lblTitle" runat="server" Font-Bold="True" Text="Title"></asp:Label>
                                        </strong>
                                    </td>
                                    <td style="width: 401px; height: 26px;">
                                        <asp:DropDownList ID="ddltitle" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 168px; height: 26px;">
                                        <strong>
                                            <asp:Label ID="lblInitials" runat="server" Font-Bold="True" Text="Initials"></asp:Label></strong></td>
                                    <td style="width: 401px; height: 26px;">
                                        <asp:TextBox ID="txtInitial" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 168px; height: 40px">
                                        <asp:Label ID="lblTradingName" runat="server" Font-Bold="True" Text="TradingName"></asp:Label></td>
                                    <td style="width: 401px; height: 40px">
                                        &nbsp;<asp:TextBox ID="txtTradingName" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 168px; height: 40px">
                                        Is Prospect</td>
                                    <td style="width: 401px; height: 40px">
                                        <asp:CheckBox ID="chkIsprospect" runat="server" />
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="width: 168px; height: 40px">
                                        Is Agent</td>
                                    <td style="width: 401px; height: 40px">
                                        <asp:CheckBox ID="chkIsagent" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 168px; height: 26px;">
                                        <strong>
                                            <asp:Label ID="lblArea" runat="server" Text="Area"></asp:Label></strong></td>
                                    <td style="width: 401px; height: 26px;">
                                        <asp:DropDownList ID="ddlArea" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 168px; height: 26px;">
                                        <strong>
                                            <asp:Label ID="lblBusiness" runat="server" Text="Business"></asp:Label></strong></td>
                                    <td style="width: 401px; height: 26px;">
                                        <asp:DropDownList ID="ddlbusiness" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 168px; height: 26px;">
                                        <strong>
                                            <asp:Label ID="lblTrade" runat="server" Text="Trade"></asp:Label></strong></td>
                                    <td style="width: 401px; height: 26px;">
                                        <asp:DropDownList ID="ddlTrade" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 168px; height: 26px;">
                                        <strong>
                                            <asp:Label ID="lblSICcode" runat="server" Text="SIC code "></asp:Label></strong></td>
                                    <td style="width: 401px; height: 26px;">
                                        <asp:DropDownList ID="ddlSic" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 168px; height: 26px;">
                                        <strong>
                                            <asp:Label ID="lblNoofEmployees" runat="server" Text="No of  Employees"></asp:Label></strong>
                                    </td>
                                    <td style="width: 401px; height: 26px;">
                                        <asp:DropDownList ID="ddlNoofEmployees" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 168px; height: 26px">
                                        <asp:Label ID="lblNoOfOffices" runat="server" Font-Bold="True" Text="No of Offices"></asp:Label></td>
                                    <td style="width: 401px; height: 26px">
                                        <asp:TextBox ID="txtNoOfOffices" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 168px; height: 26px">
                                        <asp:Label ID="lblTradeSince" runat="server" Font-Bold="True" Text="Trade Since"></asp:Label></td>
                                    <td style="width: 401px; height: 26px">
                                        <asp:TextBox ID="txtTradeSince" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 168px; height: 26px">
                                        <asp:Label ID="lblFileCode" runat="server" Text="File Code" Font-Bold="True"></asp:Label></td>
                                    <td style="width: 401px; height: 26px">
                                        <asp:TextBox ID="txtFileCode" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 168px; height: 26px">
                                        <asp:Label ID="lblMainContact" runat="server" Font-Bold="True" Text="MainContact"></asp:Label></td>
                                    <td style="width: 401px; height: 26px">
                                        <asp:TextBox ID="txtMainContact" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 168px; height: 26px">
                                        <asp:Label ID="lblCompanyName" runat="server" Font-Bold="True" Text="Company Name"></asp:Label></td>
                                    <td style="width: 401px; height: 26px">
                                        <asp:TextBox ID="txtCompanyName" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 168px; height: 26px">
                                        <asp:Label ID="lblCharityDetails" runat="server" Font-Bold="True" Text="CharityDetails"></asp:Label></td>
                                    <td style="width: 401px; height: 26px">
                                        <asp:Panel ID="pnlCharityDetails" runat="server" Height="50px" Width="125px">
                                            <table>
                                                <tr>
                                                    <td style="width: 100px; height: 21px;">
                                                        Charity</td>
                                                    <td style="width: 100px; height: 21px;">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 100px">
                                                        No Of Members</td>
                                                    <td style="width: 100px">
                                                        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 100px">
                                                        Charity No</td>
                                                    <td style="width: 100px">
                                                        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox></td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 168px; height: 40px">
                                        <asp:Label ID="lblGroupName" runat="server" Font-Bold="False" Text="Group Name"></asp:Label></td>
                                    <td style="width: 401px; height: 40px">
                                        <asp:TextBox ID="txtgrname" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 168px; height: 40px">
                                        <asp:Label ID="lblGroupType" runat="server" Font-Bold="False" Text="Group Type"></asp:Label>
                                    </td>
                                    <td style="width: 401px; height: 40px">
                                        <asp:TextBox ID="txtgrtype" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 168px; height: 40px">
                                        Account Balance</td>
                                    <td style="width: 401px; height: 40px">
                                        <asp:TextBox ID="txtAccbalance" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 168px; height: 40px">
                                        year to date turn over</td>
                                    <td style="width: 401px; height: 40px">
                                        <asp:TextBox ID="txtYearTodateTurnOver" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 168px; height: 40px">
                                        &nbsp;last year turnover</td>
                                    <td style="width: 401px; height: 40px">
                                        <asp:TextBox ID="txtLastYearturnOver" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 168px; height: 40px">
                                        Alternative Identifier</td>
                                    <td style="width: 401px; height: 40px">
                                        <asp:TextBox ID="txtAlternativeIdentifier" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 168px; height: 40px">
                                        Branch</td>
                                    <td style="width: 401px; height: 40px">
                                        <asp:DropDownList ID="ddBranch" runat="server">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td style="width: 168px; height: 40px">
                                        Sub Branch</td>
                                    <td style="width: 401px; height: 40px">
                                        <asp:DropDownList ID="ddSubBranch" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 168px; height: 40px">
                                        Service Level</td>
                                    <td style="width: 401px; height: 40px">
                                        <asp:DropDownList ID="ddServiceLevel" runat="server">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td style="width: 168px; height: 40px">
                                        Account Excecutive</td>
                                    <td style="width: 401px; height: 40px">
                                        &nbsp;
                                        <asp:Panel ID="Panel5" runat="server" Height="50px" Width="125px">
                                            <table>
                                                <tr>
                                                    <td style="width: 100px">
                                                        Code</td>
                                                    <td style="width: 100px">
                                                        <asp:TextBox ID="txtAccExecutiveCode" runat="server"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 100px">
                                                        Name</td>
                                                    <td style="width: 100px">
                                                        <asp:TextBox ID="txtAccExecutiveName" runat="server"></asp:TextBox></td>
                                                </tr>
                                            </table>
                                            <asp:Button ID="btnAccExecutive" runat="server" OnClientClick="LoadWindows('../MTA/FindAccountExecuctive.aspx')"
                                                Text="Acc Excecutive" /></asp:Panel>
                                    </td>
                                    <td style="width: 433px; height: 40px">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 168px; height: 40px">
                                        Lead Agent</td>
                                    <td style="width: 401px; height: 40px">
                                        &nbsp;&nbsp;
                                        <table>
                                            <tr>
                                                <td style="width: 100px">
                                                    Code</td>
                                                <td style="width: 100px">
                                                    <asp:TextBox ID="txtLeadAgentCode" runat="server"></asp:TextBox></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 100px">
                                                    Name</td>
                                                <td style="width: 100px">
                                                    <asp:TextBox ID="txtLeadAgentName" runat="server"></asp:TextBox></td>
                                            </tr>
                                        </table>
                                        <asp:Button ID="btnLeadAgent" runat="server" OnClientClick="LoadWindows('../MTA/FindAgent.aspx')"
                                            Text="Lead Agent" />
                                        <asp:HiddenField ID="hdLeadAgent" runat="server" />
                                    </td>
                                    <td style="width: 433px; height: 40px">
                                    </td>
                                </tr>
                        </table>
                        </td>
                        <td valign="top" align="left">
                        
                        </td>
                        </tr>
                        </table>
                    </asp:Panel>
                    &nbsp;&nbsp;
                </asp:View>
                
                <asp:View ID="Contacts" runat="server">
                    <asp:Panel ID="Panel2" runat="server" Height="50px" Width="360px">
                        <asp:GridView ID="GVContacts" runat="server" Height="48px" Width="584px" AutoGenerateColumns="False">
                            <Columns>
                                <asp:CommandField ShowSelectButton="True" />
                                <asp:CommandField ShowDeleteButton="True" />
                                <asp:BoundField DataField="AddressLine2" HeaderText="AddressLine2" />
                                <asp:BoundField DataField="AddressLine3" HeaderText="AddressLine3" />
                                <asp:BoundField DataField="PostCode" HeaderText="PostCode" />
                                <asp:BoundField DataField="AddressLine1" HeaderText="AddressLine1" />
                                <asp:BoundField DataField="AddressLine4" HeaderText="AddressLine4" />
                                <asp:BoundField DataField="CountryCode" HeaderText="CountryCode" />
                                
                                
                            </Columns>
                        </asp:GridView>
                        <asp:Button ID="btnaddaddress" runat="server" Text="Add" /></asp:Panel>
                    <asp:Panel ID="Panel3" runat="server" Height="50px" Width="536px">
                        <asp:GridView AutoGenerateColumns=false ID="GVContacts1" runat="server" Width="584px">
                        <Columns>
                           <asp:CommandField ShowSelectButton="True" />
                           <asp:CommandField ShowDeleteButton="True" />
                           <asp:BoundField DataField="AreaCode" HeaderText="AreaCode" />
                           <asp:BoundField DataField="Description" HeaderText="Description" />                          
                        </Columns>
                        </asp:GridView>
                        <asp:Button ID="btnaddcontacts" runat="server" Text="Add" />
                    </asp:Panel>
                    <asp:Panel ID="pnlcorrespondence" runat="server" Height="50px" Width="592px" BorderColor="DarkGray"
                        BorderWidth="1">
                        <table style="width: 680px; height: 64px">
                            <tr>
                                <td style="width: 206px; height: 24px;">
                                    Correspondence</td>
                                <td style="width: 247px; height: 24px;">
                                </td>
                                <td style="width: 247px; height: 24px;">
                                </td>
                                <td style="width: 247px; height: 24px;">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 206px; height: 24px;">
                                    Salutation</td>
                                <td style="width: 247px; height: 24px;">
                                    <asp:TextBox ID="txtsalutation" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td style="width: 206px; height: 26px;">
                                    Prefered Correspondence</td>
                                <td style="width: 247px; height: 26px;">
                                    <asp:TextBox ID="txtpreferedcorr" runat="server"></asp:TextBox></td>
                                <td style="width: 247px; height: 26px;">
                                </td>
                                <td style="width: 247px; height: 26px;">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 206px; height: 26px;">
                                    Is TPS</td>
                                <td style="width: 247px; height: 26px;">
                                    &nbsp;<asp:CheckBox ID="chkTPS" runat="server" /></td>
                                <td style="width: 247px; height: 26px;">
                                </td>
                                <td style="width: 247px; height: 26px;">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 206px; height: 26px;">
                                    Is MPS</td>
                                <td style="width: 247px; height: 26px;">
                                    &nbsp;<asp:CheckBox ID="chkMPS" runat="server" /></td>
                                <td style="width: 247px; height: 26px;">
                                </td>
                                <td style="width: 247px; height: 26px;">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 206px; height: 26px;">
                                    Is eMPS</td>
                                <td style="width: 247px; height: 26px;">
                                    &nbsp;<asp:CheckBox ID="chkeMPS" runat="server" /></td>
                                <td style="width: 247px; height: 26px;">
                                </td>
                                <td style="width: 247px; height: 26px;">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlAddAddress" Visible="false" runat="server" Height="50px" Width="592px"
                        BorderColor="DarkGray" BorderWidth="1">
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
                    <asp:Panel ID="plnAddContact" Visible="false" runat="server" Height="50px" Width="592px"
                        BorderColor="DarkGray" BorderWidth="1">
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
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btncok" runat="server" Text="Ok" />
                                    <asp:Button ID="btnccancel" runat="server" Text="Cancel" /></td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:View>
                <asp:View ID="Additions" runat="server">
                    <asp:Panel ID="Panel4" runat="server" Height="50px" Width="592px">
                        <table style="width: 680px; height: 64px">
                            <tr>
                                <td style="width: 206px; height: 24px;">
                                    Currency</td>
                                <td style="width: 247px; height: 24px;">
                                    &nbsp;<asp:DropDownList ID="ddlCurrency" runat="server" Width="176px">
                                    </asp:DropDownList></td>
                                <td style="width: 247px; height: 24px;">
                                    Terms of Payement</td>
                                <td style="width: 247px; height: 24px;">
                                    <asp:TextBox ID="txttermspay" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 206px; height: 26px;">
                                    Payment Method
                                </td>
                                <td style="width: 247px; height: 26px;">
                                    &nbsp;<asp:DropDownList ID="ddlPaymentMethod" runat="server" Width="176px">
                                    </asp:DropDownList></td>
                                <td style="width: 247px; height: 26px;">
                                    Renewal Stop Code</td>
                                <td style="width: 247px; height: 26px;">
                                    &nbsp;<asp:DropDownList ID="ddlrenewaldtopcode" runat="server">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td style="width: 206px; height: 26px;">
                                    Reminder Type</td>
                                <td style="width: 247px; height: 26px;">
                                    &nbsp;<asp:DropDownList ID="ddlReminderType" runat="server" Width="176px">
                                    </asp:DropDownList></td>
                                <td style="width: 247px; height: 26px;">
                                    Source</td>
                                <td style="width: 247px; height: 26px;">
                                    <asp:TextBox ID="txtsource" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td style="width: 206px; height: 26px">
                                    <asp:Label ID="lblLoyalty" runat="server" Text="LoyaltyNumber"></asp:Label></td>
                                <td style="width: 247px; height: 26px"><asp:DropDownList ID="ddlLoyaltyNumber" runat="server" Width="176px">
                                </asp:DropDownList></td>
                                <td style="width: 247px; height: 26px">
                                    <asp:Label ID="lblSeasonalgift" runat="server" Text="Seasonal gift"></asp:Label></td>
                                <td style="width: 247px; height: 26px">
                                    <asp:TextBox ID="txtSeasonalgift" runat="server"></asp:TextBox></td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlemp" runat="server" Height="50px" Width="680px">
                        <table style="width: 680px">
                            <tr>
                                <td colspan="2" style="width: 142px; height: 26px;">
                                    Employer</td>
                                <td colspan="2" style="width: 131px; height: 26px;">
                                    secondary Employer</td>
                            </tr>
                            <tr>
                                <td style="width: 142px; height: 26px;">
                                    Occupation</td>
                                <td style="width: 161px; height: 26px;">
                                    &nbsp;<asp:DropDownList ID="ddlPriOccupation" runat="server" Width="144px">
                                    </asp:DropDownList></td>
                                <td style="width: 131px; height: 26px;">
                                    Occupation</td>
                                <td style="height: 26px">
                                    &nbsp;<asp:DropDownList ID="ddlSecOccupation" runat="server" Width="184px">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td style="width: 142px">
                                    Employer's Business</td>
                                <td style="width: 161px">
                                    &nbsp;<asp:DropDownList ID="ddlPriEmpBusiness" runat="server" Width="144px">
                                    </asp:DropDownList></td>
                                <td style="width: 131px">
                                    Employer's Business</td>
                                <td>
                                    &nbsp;<asp:DropDownList ID="ddlSecEmpsBusiness" runat="server" Width="184px">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td style="width: 142px">
                                    Status</td>
                                <td style="width: 161px">
                                    &nbsp;<asp:DropDownList ID="ddlPriStatus" runat="server" Width="144px">
                                    </asp:DropDownList></td>
                                <td style="width: 131px">
                                    Status</td>
                                <td>
                                    &nbsp;<asp:DropDownList ID="ddlSecStatus" runat="server" Width="184px">
                                    </asp:DropDownList></td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlcc" runat="server" Height="50px" Width="680px">
                        <table style="width: 680px">
                            <tr>
                                <td style="width: 142px; height: 26px;">
                                    Wageroll</td>
                                <td style="width: 161px; height: 26px;">
                                    <asp:TextBox ID="txtwage" runat="server"></asp:TextBox></td>
                                <td style="width: 131px; height: 26px;">
                                    turnover</td>
                                <td style="height: 26px">
                                    &nbsp;<asp:DropDownList ID="ddlturnover" runat="server">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td style="width: 142px; height: 26px;">
                                    Financial year</td>
                                <td style="width: 161px; height: 26px;">
                                    <asp:TextBox ID="txtfinancial" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td style="width: 142px; height: 26px;">
                                    <asp:Label ID="lblAreaCode" runat="server" Text="Area" Width="54px"></asp:Label></td>
                                <td style="width: 161px; height: 26px;">
                                    <asp:DropDownList ID="ddlAreaCode" runat="server">
                                    </asp:DropDownList></td>
                                <td style="width: 131px; height: 26px;">
                                    <asp:Label ID="lblFile" runat="server" Text="File Code" Width="124px"></asp:Label></td>
                                <td style="height: 26px">
                                    &nbsp;<asp:DropDownList ID="ddlFile" runat="server">
                                    </asp:DropDownList></td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Button ID="btnAssosicates" runat="server" Text="Associates" />
                    <asp:Panel ID="pnlAddAssociates" runat="server" Height="104px" Visible="False" Width="560px"
                        BorderColor="DarkGray" BorderWidth="1">
                        <%-- <asp:GridView ID="gvassociates" runat="server" Width="552px">
                        </asp:GridView>--%>
                        <asp:GridView AutoGenerateColumns="false" ID="gvassociates" runat="server" Width="552px">
                            <Columns>
                                <asp:BoundField HeaderText="RelationshipCode" DataField="RelationshipCode" />
                                <asp:BoundField HeaderText="RelationshipDescription" DataField="RelationshipDescription" />
                                <asp:BoundField HeaderText="ClientKey" DataField="ClientKey" />
                                <asp:BoundField HeaderText="AssociateKey" DataField="AssociateKey" />
                            </Columns>
                        </asp:GridView>
                        <asp:Button ID="btnNewAssociates" runat="server" Text="New" />
                        &nbsp;<br />
                        <br />
                        &nbsp;<br />
                        <asp:Panel ID="pnlNewAssociates" runat="server" Height="104px" Visible="False" Width="472px">
                            <br />
                            Associate key : &nbsp; &nbsp; &nbsp; &nbsp;
                            <asp:TextBox ID="txtclient" runat="server"></asp:TextBox><br />
                            <br />
                            RelationShip Desc:
                            <asp:DropDownList ID="ddlrelationshipcode" runat="server">
                            </asp:DropDownList><br />
                            <br />
                            <asp:Button ID="btnNewAssociatesOk" runat="server" Text="Ok" />
                            <asp:Button ID="btnNewAssociatesCancel" runat="server" Text="Cancel" /></asp:Panel>
                    </asp:Panel>
                </asp:View>
                <asp:View ID="Covictions" runat="server">
                    <asp:Panel ID="pnlConvictions" runat="server" Height="50px" Width="544px">
                        <%--<asp:GridView ID="gvConcictions" runat="server" Width="536px">
                        </asp:GridView>--%>
                        <asp:GridView ID="gvConcictions" AutoGenerateColumns="false" runat="server" Width="536px">
                            <Columns>
                                <asp:CommandField ShowSelectButton="True" />
                                <asp:CommandField ShowDeleteButton="True" />
                                <asp:BoundField HeaderText="SentenceTypeCode" DataField="SentenceTypeCode" />
                                <%-- <asp:BoundField HeaderText="FineAmountSpecified" DataField="FineAmountSpecified" />--%>
                                <%--<asp:BoundField HeaderText="SentenceDurationSpecified" DataField="SentenceDurationSpecified" />--%>
                                <asp:BoundField HeaderText="SentenceDuration" DataField="SentenceDuration" />
                                <asp:BoundField HeaderText="TypeCode" DataField="TypeCode" />
                                <asp:BoundField HeaderText="DrivingLicensePenaltyPoints" DataField="DrivingLicensePenaltyPoints" />
                                <asp:BoundField HeaderText="SentenceEffectiveDate" DataField="SentenceEffectiveDate" />
                                <asp:BoundField HeaderText="FineAmount" DataField="FineAmount" />
                                <%--<asp:BoundField HeaderText="DrivingLicensePenaltyPointsSpecified" DataField="DrivingLicensePenaltyPointsSpecified" />--%>
                                <asp:BoundField HeaderText="Date" DataField="Date" />
                                <asp:BoundField HeaderText="ConvictionKey" DataField="ConvictionKey" />
                                <asp:BoundField HeaderText="AlcoholMeasurementMethod" DataField="AlcoholMeasurementMethod" />
                                <%--<asp:BoundField HeaderText="AlcoholLevelSpecified" DataField="AlcoholLevelSpecified" />--%>
                                <%--<asp:BoundField HeaderText="SentenceEffectiveDateSpecified" DataField="SentenceEffectiveDateSpecified" />--%>
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
                    <asp:Panel ID="pnlAddConvictions" runat="server" Height="50px" Visible="False" Width="552px"
                        BorderColor="DarkGray" BorderWidth="1">
                        <table style="width: 528px">
                            <tr>
                                <td style="height: 169px">
                                    <br />
                                    <br />
                                    Convictions Details<br />
                                    <table style="width: 520px; height: 88px">
                                        <tr>
                                            <td style="width: 138px">
                                                <strong>Type</strong></td>
                                            <td style="width: 173px">
                                                &nbsp;<asp:DropDownList ID="ddlconvictiontype" runat="server">
                                                </asp:DropDownList></td>
                                            <td style="width: 57px">
                                                Fine</td>
                                            <td>
                                                <asp:TextBox ID="txtcfine" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 138px; height: 26px;">
                                                <strong>Status</strong></td>
                                            <td style="width: 173px; height: 26px;">
                                                &nbsp;<asp:DropDownList ID="ddlconvictionstatus" runat="server">
                                                </asp:DropDownList></td>
                                            <td style="width: 57px; height: 26px;">
                                                <strong>Date</strong></td>
                                            <td style="height: 26px">
                                                <asp:TextBox ID="txtcdate" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 138px; height: 27px">
                                                <strong>Description</strong></td>
                                            <td style="width: 173px; height: 27px">
                                                <asp:TextBox ID="txtcdescription" runat="server"></asp:TextBox></td>
                                            <td style="width: 57px; height: 27px">
                                            </td>
                                            <td style="height: 27px">
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 11px">
                                    Sentence<br />
                                    <table style="width: 512px" id="TABLE1" onclick="return TABLE1_onclick()">
                                        <tr>
                                            <td style="height: 21px; width: 127px;">
                                                Type</td>
                                            <td style="height: 21px; width: 155px;">
                                                &nbsp;<asp:DropDownList ID="ddlsentencetype" runat="server">
                                                </asp:DropDownList></td>
                                            <td style="height: 21px; width: 82px;">
                                                Date</td>
                                            <td style="height: 21px; width: 158px;">
                                                <asp:TextBox ID="txtsdate" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="height: 21px; width: 127px;">
                                                Description</td>
                                            <td style="height: 21px; width: 155px;">
                                                <asp:TextBox ID="txtsdescription" runat="server"></asp:TextBox></td>
                                            <td style="height: 21px; width: 82px;">
                                                Duration</td>
                                            <td style="height: 21px; width: 158px;">
                                                <asp:TextBox ID="txtsduration" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="height: 21px; width: 127px;">
                                            </td>
                                            <td style="height: 21px; width: 155px;">
                                            </td>
                                            <td style="height: 21px; width: 82px;">
                                                Time unit</td>
                                            <td style="height: 21px; width: 158px;">
                                                &nbsp;<asp:DropDownList ID="ddlsentencetime" runat="server">
                                                </asp:DropDownList></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 88px">
                                    Motoring Related<br />
                                    <table style="width: 512px">
                                        <tr>
                                            <td style="height: 21px; width: 135px;">
                                                Alcohol Measurement Method</td>
                                            <td style="height: 21px; width: 161px;">
                                                &nbsp;<asp:DropDownList ID="ddlAlcoholmethod" runat="server">
                                                </asp:DropDownList></td>
                                            <td style="height: 21px; width: 55px;">
                                                Alcohol Level</td>
                                            <td style="height: 21px">
                                                <asp:TextBox ID="txtalcohollevel" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="height: 21px; width: 135px;">
                                            </td>
                                            <td style="height: 21px; width: 161px;">
                                            </td>
                                            <td style="height: 21px; width: 55px;">
                                                Penality point</td>
                                            <td style="height: 21px">
                                                <asp:TextBox ID="txtpenality" runat="server"></asp:TextBox></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnOk" runat="server" Text="OK" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" /></td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:View>
                <asp:View ID="LifeStyle" runat="server">
                    <asp:Panel ID="pnlLifeStyle" runat="server" Height="72px" Width="216px">
                        <table style="width: 512px">
                            <tr>
                                <td style="width: 559px">
                                    Personal Details<br />
                                    <table style="width: 480px">
                                        <tr>
                                            <td style="width: 115px; height: 21px">
                                                DOB</td>
                                            <td style="width: 159px; height: 21px">
                                                <asp:TextBox ID="txtDobirth" runat="server">
                                                </asp:TextBox></td>
                                            <td style="height: 21px">
                                                Gender</td>
                                            <td style="width: 159px; height: 21px">
                                                &nbsp;<asp:DropDownList ID="ddgender" runat="server">
                                                </asp:DropDownList></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 115px; height: 21px">
                                                Marital Status</td>
                                            <td style="width: 159px; height: 21px">
                                                &nbsp;<asp:DropDownList ID="ddmaritalstatus" runat="server">
                                                </asp:DropDownList></td>
                                            <td style="height: 21px">
                                                Nationality</td>
                                            <td style="width: 159px; height: 21px">
                                                &nbsp;<asp:DropDownList ID="ddnationality" runat="server">
                                                </asp:DropDownList></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 115px">
                                                Seasonal Gift</td>
                                            <td style="width: 159px; height: 21px">
                                                &nbsp;<asp:DropDownList ID="ddseasonalgift" runat="server">
                                                </asp:DropDownList></td>
                                            <td>
                                                Accomodation</td>
                                            <td style="width: 121px">
                                                <asp:TextBox ID="txtaccomodation" runat="server">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table style="width: 480px">
                                        <tr>
                                            <td style="width: 59px">
                                                Loyalty number</td>
                                            <td style="width: 153px">
                                                <asp:TextBox ID="txtloyalty" runat="server">
                                                </asp:TextBox></td>
                                            <td style="width: 91px">
                                                Smoker</td>
                                            <td>
                                                <asp:CheckBox ID="chksmoker" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table style="width: 480px">
                                        <tr>
                                            <td style="width: 60px">
                                                Pets</td>
                                            <td style="width: 121px">
                                                <asp:TextBox ID="txtpet" runat="server">
                                                </asp:TextBox></td>
                                            <td style="width: 90px">
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 559px; height: 21px">
                                    Dependents<asp:GridView AutoGenerateColumns="false" ID="gvDependents" runat="server"
                                        Width="488px">
                                        <Columns>
                                            <asp:BoundField DataField="SecOccupationCode" HeaderText="SecOccupationCode" />
                                            <asp:BoundField DataField="OccupationCode" HeaderText="OccupationCode" />
                                            <asp:BoundField DataField="CategoryCode" HeaderText="CategoryCode" />
                                            <asp:BoundField DataField="LifestyleKey" HeaderText="LifestyleKey" />
                                            <%--<asp:BoundField DataField="SmokerSpecified" HeaderText="SmokerSpecified" />--%>
                                            <asp:BoundField DataField="Name" HeaderText="Name" />
                                            <asp:BoundField DataField="Smoker" HeaderText="Smoker" />
                                            <%-- <asp:BoundField DataField="DateOfBirthSpecified" HeaderText="DateOfBirthSpecified" />
                                            <asp:BoundField DataField="GenderCodeSpecified" HeaderText="GenderCodeSpecified" />--%>
                                            <asp:BoundField DataField="DateOfBirth" HeaderText="DateOfBirth" />
                                        </Columns>
                                    </asp:GridView>
                                    <%-- <asp:GridView AutoGenerateColumns="true" ID="gvDependents" runat="server"
                                        Width="488px">
                                    </asp:GridView>--%>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 559px">
                                    <asp:Button ID="btnLSAdd" runat="server" Text="Add" />&nbsp;
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlAddDependents" runat="server" Height="72px" Visible="False" Width="512px"
                        BorderColor="DarkGray" BorderWidth="1">
                        General<br />
                        <table style="width: 512px">
                            <tr>
                                <td style="width: 166px">
                                    Name</td>
                                <td>
                                    <asp:TextBox ID="txtName" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td style="width: 166px">
                                    DateOfBirth</td>
                                <td>
                                    <asp:TextBox ID="txtDOB" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td style="width: 166px">
                                    Category</td>
                                <td>
                                    &nbsp;<asp:DropDownList ID="ddlcategory" runat="server">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td style="width: 166px">
                                    Gendercode</td>
                                <td>
                                    <asp:TextBox ID="txtgendercode" runat="server" Enabled="False">Female</asp:TextBox></td>
                            </tr>
                            <tr>
                                <td style="width: 166px; height: 26px;">
                                    Occupationcode</td>
                                <td style="height: 26px">
                                    &nbsp;<asp:DropDownList ID="ddloccupationcode" runat="server">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td style="width: 166px">
                                    Sec Occupation code</td>
                                <td>
                                    &nbsp;<asp:DropDownList ID="ddlsecoccupationcode" runat="server">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td style="width: 166px; height: 26px;">
                                    Is smoker</td>
                                <td style="height: 26px">
                                    &nbsp;<asp:CheckBox ID="chkIssmoker" runat="server" /></td>
                            </tr>
                            <tr>
                                <td style="width: 166px">
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
                        <asp:Button ID="btnLSOk" runat="server" Text="OK" />
                        <asp:Button ID="btnLSCancel" runat="server" Text="Cancel" /></asp:Panel>
                    &nbsp;
                </asp:View>
                <asp:View ID="Misc" runat="server">
                    <asp:Panel ID="pnlLoyaltySchemes" runat="server" Height="50px" Width="125px">
                        Loyalty Schemes<br />
                        <table style="width: 488px">
                            <tr>
                                <td>
                                    <%-- <asp:GridView ID="gvloyalty" runat="server" Width="488px">
                                    </asp:GridView>--%>
                                    <asp:GridView ID="gvloyalty" AutoGenerateColumns="false" runat="server" Width="488px">
                                        <Columns>
                                            <asp:BoundField DataField="MainMember" HeaderText="MainMember" />
                                            <asp:BoundField DataField="OtherReference" HeaderText="OtherReference" />
                                            <asp:BoundField DataField="EndDate" HeaderText="EndDate" />
                                            <asp:BoundField DataField="Active" HeaderText="Active" />
                                            <asp:BoundField DataField="StartDate" HeaderText="StartDate" />
                                            <asp:BoundField DataField="LoyaltySchemeKey" HeaderText="LoyaltySchemeKey" />
                                            <%-- <asp:BoundField DataField="EndDateSpecified" HeaderText="EndDateSpecified" />--%>
                                            <asp:BoundField DataField="LoyaltySchemeCode" HeaderText="LoyaltySchemeCode" />
                                            <asp:BoundField DataField="MembershipNumber" HeaderText="MembershipNumber" />
                                            <%-- <asp:BoundField DataField="ActiveSpecified" HeaderText="ActiveSpecified" />--%>
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
                    <asp:Panel ID="pnlAddLoyaltySchemes" runat="server" Height="50px" Visible="False"
                        Width="125px" BorderColor="DarkGray" BorderWidth="1">
                        <asp:Panel ID="Panel6" runat="server" Height="50px" Width="496px">
                            <table style="width: 496px">
                                <tr>
                                    <td style="width: 193px">
                                        Loyalty Schemes</td>
                                    <td>
                                        <asp:TextBox ID="txtloyaltyscheme" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 193px; height: 21px">
                                        <strong>MemberShip number</strong></td>
                                    <td style="height: 21px">
                                        <asp:TextBox ID="txtmembership" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 193px">
                                        other Reference</td>
                                    <td>
                                        <asp:TextBox ID="txtotherref" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 193px">
                                        <strong>Start Date</strong></td>
                                    <td>
                                        <asp:TextBox ID="txtstart" runat="server"></asp:TextBox></td>
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
                        <asp:Button ID="btnAddLoyaltyCancel" runat="server" Text="Cancel" /></asp:Panel>
                </asp:View>
                <asp:View ID="Prospecting" runat="server">
                    <table style="width: 520px; height: 96px">
                        <tr>
                            <td style="width: 351px; height: 186px;">
                                <table style="width: 496px">
                                    <tr>
                                        <td style="width: 193px">
                                            Agent
                                            <br />
                                            Reference&nbsp;</td>
                                        <td>
                                            &nbsp;<asp:TextBox ID="txtagentref" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 193px; height: 21px">
                                            Current
                                            <br />
                                            Agent &nbsp;</td>
                                        <td style="height: 21px">
                                            <asp:TextBox ID="txtcurrentagent" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 193px">
                                            Strength Code</td>
                                        <td>
                                            <asp:TextBox ID="txtstrength" runat="server"></asp:TextBox>&nbsp;<asp:DropDownList
                                                ID="ddlprosStrengthCode" runat="server">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 193px">
                                            Status &nbsp;</td>
                                        <td>
                                            <asp:TextBox ID="txtpstatus" runat="server"></asp:TextBox>&nbsp;<asp:DropDownList
                                                ID="ddlprosStatus" runat="server">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 193px; height: 26px;">
                                        </td>
                                        <td style="height: 26px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 193px">
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                                &nbsp;<br />
                                &nbsp; &nbsp; &nbsp;</td>
                            <td style="width: 294px; height: 186px">
                                &nbsp; &nbsp;<br />
                                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 351px">
                                <br />
                                <strong>Previous Insurer<br />
                                </strong>
                                <br />
                                Code
                                <asp:TextBox ID="txtPIcode" runat="server"></asp:TextBox><br />
                                Name<asp:TextBox ID="txtPIname" runat="server"></asp:TextBox></td>
                            <td style="width: 294px">
                                <br />
                                <strong>Previous Broker<br />
                                </strong>
                                <br />
                                Code
                                <asp:TextBox ID="txtPBcode" runat="server"></asp:TextBox><br />
                                Name<asp:TextBox ID="txtPBname" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 351px">
                                <br />
                                <br />
                                <strong>Campaigns<br />
                                </strong>
                                <br />
                                <asp:GridView ID="gvcampaigns" runat="server" Height="32px" Width="240px">
                                </asp:GridView>
                                <br />
                                <br />
                                <br />
                                &nbsp;</td>
                            <td style="width: 294px">
                                <table style="width: 200px">
                                    <tr>
                                        <td style="height: 144px">
                                            <strong>Policies<br />
                                            </strong>
                                            <br />
                                            <%--<asp:GridView ID="gvpolicies" runat="server">
                                            </asp:GridView>--%>
                                            <asp:GridView AutoGenerateColumns="false" ID="gvpolicies" runat="server">
                                                <Columns>
                                                    <%-- <asp:BoundField HeaderText="TimesQuotedSpecified" DataField="TimesQuotedSpecified" />
                                            <asp:BoundField HeaderText="TargetPremiumSpecified" DataField="TargetPremiumSpecified" />--%>
                                                    <asp:BoundField HeaderText="ProspectPolicyKey" DataField="ProspectPolicyKey" />
                                                    <asp:BoundField HeaderText="ProspectTypeCode" DataField="ProspectTypeCode" />
                                                    <%-- <asp:BoundField HeaderText="RenewalDateSpecified" DataField="RenewalDateSpecified" />--%>
                                                    <asp:BoundField HeaderText="TimesQuoted" DataField="TimesQuoted" />
                                                    <asp:BoundField HeaderText="TargetPremium" DataField="TargetPremium" />
                                                    <asp:BoundField HeaderText="RenewalDate" DataField="RenewalDate" />
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnAddPolicies" runat="server" Text="Add" />&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <asp:Panel ID="pnlAddPolicies" runat="server" Height="50px" Visible="False" Width="520px"
                        BorderColor="DarkGray" BorderWidth="1">
                        &nbsp; General<table style="width: 504px">
                            <tr>
                                <td style="width: 182px; height: 21px">
                                    Type</td>
                                <td style="height: 21px">
                                    &nbsp;<asp:DropDownList ID="ddlprostype" runat="server">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td style="width: 182px">
                                    Renewal Date</td>
                                <td>
                                    <asp:TextBox ID="txtprewnal" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td style="width: 182px">
                                    Times Quoted</td>
                                <td>
                                    <asp:TextBox ID="txttimequoted" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td style="width: 182px">
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
                </asp:View>
                <asp:View ID="Tax" runat="server">
                    <table style="width: 512px">
                        <tr>
                            <td style="width: 218px">
                                Tax Number</td>
                            <td>
                                <asp:TextBox ID="txttaxno" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 218px; height: 44px;">
                                Is Domicile For Tax</td>
                            <td style="height: 44px">
                                &nbsp;<asp:CheckBox ID="chkDomicileTax" runat="server" /></td>
                        </tr>
                        <tr>
                            <td style="width: 218px">
                                Tax Exempt</td>
                            <td>
                                <asp:TextBox ID="txtexempt" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 218px">
                                Tax percentage</td>
                            <td>
                                <asp:TextBox ID="txtpercentage" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="save" runat="server" Text="UPDATE PARTY" />
                                <asp:Button ID="btnAddParty" runat="server" Text="ADD PARTY" /></td>
                        </tr>
                    </table>
                </asp:View>
            </asp:MultiView>
        </div>
    </form>
</body>
</html>
