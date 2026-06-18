<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ListPolicyVersions.aspx.vb"  Title="ListPolicyVersions" Inherits="MTA_ListPolicyVersions" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>List Policy Version</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style="width: 361px; height: 206px">
            <tr>
                <td style="width: 99px; height: 26px">
                    Client Code</td>
                <td style="width: 97px; height: 26px">
                    <asp:TextBox ID="txtClientCode" runat="server" ReadOnly="True" Width="201px"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 99px; height: 26px">
                    Policy</td>
                <td style="width: 97px; height: 26px">
                    <asp:TextBox ID="txtPolicy" runat="server" ReadOnly="True" Width="201px"></asp:TextBox></td>
            </tr>
            <tr>
                <td colspan="2" style="height: 188px">
                    <asp:GridView ID="gvResult" runat="server" AutoGenerateColumns="False" BackColor="White"
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
                       <asp:Button ID="btnRisks" runat="server" Text="RISKS" />
                </td>
                                     

            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
