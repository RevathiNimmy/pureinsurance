<%@ Page Language="VB" AutoEventWireup="false" CodeFile="List Policy.aspx.vb" Title="List Policy" Inherits="MTA_List_Policy" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>List Policy</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table style="width: 361px; height: 206px">
                <tr>
                    <td style="width: 99px; height: 26px;">
                        Client Code<asp:TextBox ID="txtClientCode" runat="server" ReadOnly="True" Width="201px"></asp:TextBox>
                    
                        </td>
                </tr>
                <tr>
                    <td colspan="2" style="height: 134px" width:100px; ><br />LIVE POLICY
                   <div style="overflow: auto; height: 152px; width: 376px;">   
                        <asp:GridView ID="gvResult" runat="server" CellPadding="3" Height="132px" Width="345px"
                            AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None"
                            BorderWidth="1px">
                            <Columns>
                                 <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"InsuranceFolderKey")%>'
                                            CommandName="Select"> Select</asp:LinkButton>                                            
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="PolicyRef" HeaderText="Policy Number" />
                                <asp:BoundField DataField="ProductDesc" HeaderText="Product" />                             
                                <asp:BoundField DataField="RiskTypeDescription" HeaderText="Risk Type Description" />
                                <asp:BoundField DataField="InsuranceFileTypeCode" HeaderText="Insurance FileType Code"  Visible=  false/>
                            </Columns>
                            <FooterStyle BackColor="White" ForeColor="#000066" />
                            <RowStyle ForeColor="#000066" />
                            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                        </asp:GridView>
                        </div>
                                                                      
                    </td>  
                            
                </tr>
                <tr>
                <td style="height: 231px"><br />QUOTE
                <div style="overflow: auto; height: 152px; width: auto">   
                        <asp:GridView ID="GridView1" runat="server" CellPadding="3" Height="132px" Width="345px"
                            AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None"
                            BorderWidth="1px">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"InsuranceFolderKey")%>'
                                            CommandName="Select"> Select</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="PolicyRef" HeaderText="Policy Number" />
                                <asp:BoundField DataField="ProductDesc" HeaderText="Product" />
                              
                                <asp:BoundField DataField="RiskTypeDescription" HeaderText="Risk Type Description" />
                                <asp:BoundField DataField="InsuranceFileTypeCode" HeaderText="Insurance FileType Code"  Visible=  false/>
                            </Columns>
                            <FooterStyle BackColor="White" ForeColor="#000066" />
                            <RowStyle ForeColor="#000066" />
                            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                        </asp:GridView>
                        </div>
                </td>   
                <td style="width: 222px; height: 231px"><br />POLICY VERSIONS
                    <div style="overflow: auto; height: 152px; width: 500px;">   
                        <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" BackColor="White"
                        BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" Height="132px"
                        Width="345px">
                        <FooterStyle BackColor="White" ForeColor="#000066" />
                        <RowStyle ForeColor="#000066" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"InsuranceFileKey")%>'
                                        CommandName="Select"> Select</asp:LinkButton>  
                                        <asp:HiddenField runat=server Value='<%# DataBinder.Eval(Container.DataItem,"InsuranceFileKey")%>' ID="hd" />                                      
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                            
                            </asp:TemplateField>
                            <asp:BoundField DataField="PolicyRef" HeaderText="Policy Number" />
                            <asp:BoundField DataField="PolicyStatus" HeaderText="Policy Status" />
                            <asp:BoundField DataField="PolicyTypeCode" HeaderText="Policy Type" />
                            <asp:BoundField DataField="CoverStartDate" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Cover Start Date"
                                 />
                            <asp:BoundField DataField="ExpiryDate" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Cover End" />
                            <asp:BoundField DataField="RenewalDate" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Renewal Date" />
                            <asp:BoundField DataField="Premium" HeaderText="Premium" />
                            <asp:BoundField DataField="EventDesc" HeaderText="Event Description" />
                            <asp:BoundField DataField="InsuranceFileTypeCode" />
                        </Columns>
                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                    </asp:GridView>
                        </div>
                    </td>                            
                </tr>
   <tr>
                <td style="width: 1066px">
                    </td>
            </tr>
                <tr>
                <td><br />RENEWAL
                <div style="overflow: auto; height: 152px; width: 376px;">   
                        <asp:GridView ID="GridView2" runat="server" CellPadding="3" Height="132px" Width="345px"
                            AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None"
                            BorderWidth="1px">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"InsuranceFolderKey")%>'
                                            CommandName="Select"> Select</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="PolicyRef" HeaderText="Policy Number" />
                                <asp:BoundField DataField="ProductDesc" HeaderText="Product" />
                              
                                <asp:BoundField DataField="RiskTypeDescription" HeaderText="Risk Type Description" />
                                <asp:BoundField DataField="InsuranceFileTypeCode" HeaderText="Insurance FileType Code"  Visible=  false/>
                            </Columns>
                            <FooterStyle BackColor="White" ForeColor="#000066" />
                            <RowStyle ForeColor="#000066" />
                            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                        </asp:GridView>
                        </div>
                </td>      
                <td style="width: 222px">RISKS                
                <div style="overflow: auto; height: 152px; width: 500px;">                
                    <asp:GridView ID="grdLiskRisk" runat="server" CellPadding="4" ForeColor="#333333"
                                        Width="100%" AutoGenerateColumns="true" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None"
                            BorderWidth="1px">
                            <Columns>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkSelect" runat="server" CausesValidation="False" CommandName="Select"
                                            Text="Select"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                </Columns>
                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                        <RowStyle ForeColor="#000066" Wrap="False" />
                                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />                                        
                                    </asp:GridView>
                                    </div>
                    </td>           
                </tr>
            </table>
        </div>          
    </form>
</body>
</html>
