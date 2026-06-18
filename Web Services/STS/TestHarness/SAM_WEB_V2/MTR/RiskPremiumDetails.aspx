<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RiskPremiumDetails.aspx.vb"
    Title="RiskPremiumDetails" Inherits="MTC_RiskPremiumDetails" %>

<%@ Register Src="../UserControl/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>

<%@ Register Src="../UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
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
            <tr style="height: 90%">
                <td>
                    <asp:Label ID="lblSamErrorMessage" runat="server" ForeColor="Red"></asp:Label><br />
                    <br />
                    <br />
                    <asp:Label ID="Label2" runat="server" Width="60px"></asp:Label><asp:Label ID="lblRiskFees"
                        runat="server" BorderStyle="Double" Font-Bold="True" Font-Italic="True" ForeColor="#C000C0"
                        Text="Risk Premium Details" Width="150px"></asp:Label>
                    <br />
                    <br />
                    <hr />
                    <table>
                        <tr>
                            <td>
                                Insurance File Key------</td>
                            <td>
                                <asp:Label ID="lblInsuranceFileKey" runat="server" Width="60px" Font-Bold="True"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>
                                Client Code-------------</td>
                            <td>
                                <asp:Label ID="lblClientCode" runat="server" Width="60px" Font-Bold="True"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>
                                Insurance File Ref-------</td>
                            <td>
                                <asp:Label ID="lblInsuranceFileRef" runat="server" Width="60px" Font-Bold="True"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>
                                Currency---------------</td>
                            <td>
                                <asp:Label ID="lblCurrency" runat="server" Width="200px" Font-Bold="True"></asp:Label></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="left" colspan="3">
                    <asp:Menu ID="Menu1" runat="server" Orientation="Horizontal" Width="341px">
                        <Items>
                            <asp:MenuItem Text="General" Value="Risk"></asp:MenuItem>
                            <asp:MenuItem Text="Risk Fees" Value="Policy Fees"></asp:MenuItem>
                            <asp:MenuItem Text="Risk Tax" Value="Policy Tax"></asp:MenuItem>
                        </Items>
                    </asp:Menu>
                </td>
            </tr>
            <asp:MultiView ID="MultiView1" ActiveViewIndex="0" runat="Server">
                <asp:View ID="View1" runat="Server">
                    <asp:Label ID="lblOutput" runat="server" Width="184px" Font-Bold="True"></asp:Label>
                    <asp:GridView ID="grd_Output" runat="server" BackColor="#FFC0FF" CellSpacing="1"
                        AutoGenerateColumns="False">
                        <HeaderStyle BackColor="White" BorderColor="White" Font-Bold="False" Font-Italic="False" />
                        <Columns>
                            <asp:CommandField ShowEditButton="True" />
                            <asp:CommandField ShowDeleteButton="True" />
                            <asp:TemplateField HeaderText="RatingSectionType">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtRatingSectionType" ReadOnly="true" runat="server" Text='<%# Bind("RatingSectionType") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label  ID="Label1" runat="server" Text='<%# Bind("RatingSectionType") %>'></asp:Label>
                                    <%--<asp:HiddenField runat="server" ID="hdRatingSectionTypeID" Value='<%# Bind("RatingSectionTypeID") %>' />--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="EarningPattern">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEarningPattern" ReadOnly="true" runat="server" Text='<%# Bind("EarningPattern") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("EarningPattern") %>'></asp:Label>
                                    <%-- <asp:HiddenField runat="server" ID="hdEarningPatternID" Value='<%# Bind("EarningPatternID") %>' />--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="RateType">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtRateType" runat="server" Text='<%# Bind("RateType") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label3" runat="server" Text='<%# Bind("RateType") %>'></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdRateTypeID" Value='<%# Bind("RateTypeID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="AnnualRate">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtAnnualRate" runat="server" Text='<%# Bind("AnnualRate") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label4" runat="server" Text='<%# Bind("AnnualRate") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SumInsured">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtSumInsured" runat="server" Text='<%# Bind("SumInsured") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label5" runat="server" Text='<%# Bind("SumInsured") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ThisPremium">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtThisPremium" runat="server" Text='<%# Bind("ThisPremium") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label6" runat="server" Text='<%# Bind("ThisPremium") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="AnnualPremium">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtAnnualPremium" runat="server" Text='<%# Bind("AnnualPremium") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label7" runat="server" Text='<%# Bind("AnnualPremium") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Country">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtCountry" ReadOnly="true" runat="server" Text='<%# Bind("Country") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label8" runat="server" Text='<%# Bind("Country") %>'></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdCountryID" Value='<%# Bind("CountryID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="State">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtState" ReadOnly="true" runat="server" Text='<%# Bind("State") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label9" runat="server" Text='<%# Bind("State") %>'></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdRStateID" Value='<%# Bind("StateID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                   
                </asp:View>
                <asp:View ID="View2" runat="Server">
                    <asp:Label ID="Label1" runat="server" Width="184px" Font-Bold="True"></asp:Label>
                    <asp:GridView ID="grd_Output1" runat="server" BackColor="#FFC0FF" CellSpacing="1">
                        <HeaderStyle BackColor="White" BorderColor="White" Font-Bold="False" Font-Italic="False" />
                    </asp:GridView>
                </asp:View>
                <asp:View ID="View3" runat="Server">
                    <asp:Label ID="Label3" runat="server" Width="184px" Font-Bold="True"></asp:Label>
                    <asp:GridView ID="grd_Output2" runat="server" BackColor="#FFC0FF" CellSpacing="1">
                        <HeaderStyle BackColor="White" BorderColor="White" Font-Bold="False" Font-Italic="False" />
                    </asp:GridView>
                </asp:View>
            </asp:MultiView>&nbsp;<tr>
                <td>
                    &nbsp;
                    <table style="width: 768px">
                        <tr>
                            <td style="width: 154px; height: 21px">
                                Net Total :<asp:Label ID="lblNetTotal" runat="server" Width="88px"></asp:Label></td>
                            <td style="width: 143px; height: 21px">
                                Fees Total:<asp:Label ID="lblFeesTotal" runat="server"></asp:Label></td>
                            <td style="width: 165px; height: 21px">
                                Tax Total:<asp:Label ID="lblTaxTotal" runat="server"></asp:Label></td>
                            <td style="width: 222px; height: 21px">
                                Gross Total:<asp:Label ID="lblGrossTotal" runat="server" Width="104px"></asp:Label></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnOk" runat="server" Text="Ok" /> <asp:Button ID="btnAddMTAQuote" runat="server" Text="Add New" /></td>
            </tr>
            <tr style="height:180px">
            <td>
                <uc2:Footer ID="Footer1" runat="server" />
            
            </td>
            </tr>
        </table>
    </form>
</body>
</html>
