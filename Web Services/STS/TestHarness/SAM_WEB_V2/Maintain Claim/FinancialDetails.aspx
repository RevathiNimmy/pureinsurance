<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FinancialDetails.aspx.vb" Inherits="OpenClaim_FinancialDetails" %>

<%@ Register Src="../UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Financial Details</title>
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
    <asp:Label ID="Label9" runat="server" Width="60px"></asp:Label><asp:Label ID="lblFinancialDetails"
            runat="server" BorderStyle="Double" Font-Bold="True" Font-Italic="True" ForeColor="#C000C0"
            Text="Claim Financial Details" Width="200px"></asp:Label>
        <br />
        <br />
     <table>
            <tr>
                <td>
                    <strong>Include Totals</strong></td>
                <td><asp:TextBox ID="txtIncludeTotals" runat="server" /></td>                
            </tr>
            <tr>
                <td style="height: 26px">
                    <strong>Include TPRecovery</strong></td>
                <td style="height: 26px"><asp:TextBox ID="txtIncludeTPRecovery" runat="server" /></td>
            </tr>     
            <tr>
                <td>
                    <strong>Include Salvage Recovery</strong></td>
                <td><asp:TextBox ID="txtIncludeSalvageecovery" runat="server" /></td>
            </tr>   
            <tr>
                <td>
                    <strong>Include Reserve Types</strong></td>
                <td><asp:TextBox ID="txtIncludeReserveTypes" runat="server" /></td>
            </tr>          
            </table>            
        <hr />
    </div>
                    </td>
                </tr>
        
        <tr>
                    <td align="left" colspan="3">
                        <asp:Menu ID="Menu1" runat="server" Orientation="Horizontal" Width="500px">
                            <Items>
                                <asp:MenuItem Text="Total" Value="Total"></asp:MenuItem>                                
                                <asp:MenuItem Text="TP Recovery" Value="TP Recovery"></asp:MenuItem>
                                <asp:MenuItem Text="Salvage" Value="Salvage"></asp:MenuItem>
                                <asp:MenuItem Text="Reserve" Value="Reserve"></asp:MenuItem>
                                <asp:MenuItem Text="Sample Reserve" Value="Sample Reserve"></asp:MenuItem>
                            </Items>
                        </asp:Menu>
                    </td>
         </tr>   
          <asp:MultiView id="MultiView1" ActiveViewIndex=0 runat="Server">     
             <asp:View id="View1" runat="Server"> 
             <div align="center">
                 <br />
                 <br />
                 <br />
             <asp:Label ID="lblOutput" runat="server"  Width="300px" Font-Bold="True"></asp:Label>
                 <br />
                 <asp:Label ID="lblCode" runat="server" Font-Bold="True" Width="300px"></asp:Label><br />
                 <asp:Label ID="lblDescription" runat="server" Font-Bold="True" Width="300px"></asp:Label></div>            
             <asp:GridView
            ID="grd_Output" runat="server" BackColor="#FFC0FF" CellSpacing="1">
            <HeaderStyle BackColor="White" BorderColor="White" Font-Bold="False" Font-Italic="False" />           

        </asp:GridView>             
             </asp:View>          
         </asp:MultiView></table>
    </div>
  <%--  <div>
    <asp:Label ID="Label9" runat="server" Width="60px"></asp:Label><asp:Label ID="lblFinancialDetails"
            runat="server" BorderStyle="Double" Font-Bold="True" Font-Italic="True" ForeColor="#C000C0"
            Text="Claim Financial Details" Width="200px"></asp:Label>
        <br />
        <br />
     <table>
            <tr>
                <td>
                    <strong>Include Totals</strong></td>
                <td><asp:TextBox ID="txtIncludeTotals" runat="server" /></td>                
            </tr>
            <tr>
                <td style="height: 26px">
                    <strong>Include TPRecovery</strong></td>
                <td style="height: 26px"><asp:TextBox ID="txtIncludeTPRecovery" runat="server" /></td>
            </tr>     
            <tr>
                <td>
                    <strong>Include Salvage Recovery</strong></td>
                <td><asp:TextBox ID="txtIncludeSalvageecovery" runat="server" /></td>
            </tr>   
            <tr>
                <td>
                    <strong>Include Reserve Types</strong></td>
                <td><asp:TextBox ID="txtIncludeReserveTypes" runat="server" /></td>
            </tr>          
            </table>            
        <hr />
        
        <tr>
                    <td align="left" colspan="3">
                        <asp:Menu ID="Menu1" runat="server" Orientation="Horizontal" Width="500px">
                            <Items>
                                <asp:MenuItem Text="Total" Value="Total"></asp:MenuItem>                                
                                <asp:MenuItem Text="TP Recovery" Value="TP Recovery"></asp:MenuItem>
                                <asp:MenuItem Text="Salvage" Value="Salvage"></asp:MenuItem>
                                <asp:MenuItem Text="Reserve" Value="Reserve"></asp:MenuItem>
                                <asp:MenuItem Text="Sample Reserve" Value="Sample Reserve"></asp:MenuItem>
                            </Items>
                        </asp:Menu>
                    </td>
         </tr>   
          <asp:MultiView id="MultiView1" ActiveViewIndex=0 runat="Server">     
             <asp:View id="View1" runat="Server"> 
             <div align="center">
                 <br />
                 <br />
                 <br />
             <asp:Label ID="lblOutput" runat="server"  Width="300px" Font-Bold="True"></asp:Label>
                 <br />
                 <asp:Label ID="lblCode" runat="server" Font-Bold="True" Width="300px"></asp:Label><br />
                 <asp:Label ID="lblDescription" runat="server" Font-Bold="True" Width="300px"></asp:Label></div>            
             <asp:GridView
            ID="grd_Output" runat="server" BackColor="#FFC0FF" CellSpacing="1">
            <HeaderStyle BackColor="White" BorderColor="White" Font-Bold="False" Font-Italic="False" />           

        </asp:GridView>             
             </asp:View>          
         </asp:MultiView>
    </div>--%>
    </form>
</body>
</html>
