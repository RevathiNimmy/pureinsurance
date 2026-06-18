<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RiskPremiumDetails.aspx.vb" Title="RiskPremiumDetails" Inherits="MTC_RiskPremiumDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
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
                    <div align="center">
                        <asp:Label ID="lblOutput" runat="server" Width="184px" Font-Bold="True"></asp:Label>
                    </div>
                    <asp:GridView   ID="grd_Output" runat="server" BackColor="#FFC0FF" CellSpacing="1"
                        AutoGenerateColumns="False">
                        <HeaderStyle BackColor="White" BorderColor="White" Font-Bold="False" Font-Italic="False" />
                    <Columns>
                    
                 
                            
                           
                            
                            <asp:TemplateField HeaderText="RatingSectionType">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtRatingSectionType" ReadOnly="true" runat="server" Text='<%# Bind("RatingSectionType") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("RatingSectionType") %>'></asp:Label>
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
            </asp:MultiView>
            <tr>
                <td>
                    <asp:Button ID="btnAddMTAQuote" runat="server" Text="Add New" Visible="False" Enabled="False" />
                </td>
            </tr>
            <asp:MultiView ID="MultiView2" ActiveViewIndex="0" runat="Server">
                <asp:View ID="View2" runat="Server">
                    <div align="center">
                        <asp:Label ID="Label1" runat="server" Width="184px" Font-Bold="True"></asp:Label>
                    </div>
                    <asp:GridView ID="grd_Output1" runat="server" BackColor="#FFC0FF" CellSpacing="1">
                        <HeaderStyle BackColor="White" BorderColor="White" Font-Bold="False" Font-Italic="False" />
                    </asp:GridView>
                </asp:View>
            </asp:MultiView>
            <asp:MultiView ID="MultiView3" ActiveViewIndex="0" runat="Server">
                <asp:View ID="View3" runat="Server">
                    <div align="center">
                        <asp:Label ID="Label3" runat="server" Width="184px" Font-Bold="True"></asp:Label>
                    </div>
                    <asp:GridView ID="grd_Output2" runat="server" BackColor="#FFC0FF" CellSpacing="1">
                        <HeaderStyle BackColor="White" BorderColor="White" Font-Bold="False" Font-Italic="False" />
                    </asp:GridView>
                </asp:View>
            </asp:MultiView></div>
    </form>
</body>
</html>
