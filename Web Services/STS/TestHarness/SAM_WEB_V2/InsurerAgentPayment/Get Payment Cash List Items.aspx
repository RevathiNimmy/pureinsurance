<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Get Payment Cash List Items.aspx.vb" Inherits="CASHLIST_Get_Receipt_Cash_List_Details" %>

<%@ Register Src="../UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link href="../App_Themes/GlobalTheme/GlobalStyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../Images/loginstylesheet.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <uc1:Header ID="Header1" runat="server" />
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
                    <div>
    <table>
    <tr>
    <td>
    <span style="color: #ffffff"><span style="background-color: #006699">Cast List Items</span><br />
        </span>
    </td>
    </tr>
    <tr>
    </tr>
    
         </table>
 
        
       
           <asp:GridView ID="gvResult" runat="server" CellPadding="3" Height="132px" Width="345px"
                            AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None"
                            BorderWidth="1px">
                            
                            <Columns> 
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"CashListItemKey")%>'
                                        CommandName="Select"> Select</asp:LinkButton>
                                        <asp:HiddenField runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"CashListItemKey")%>' ID="hd" />
                                </ItemTemplate>
                            </asp:TemplateField>                               
                                <asp:BoundField DataField="MediaReference" HeaderText="Media Reference" />
                                <asp:BoundField DataField="MediaType" HeaderText="Media Type" />                             
                                <asp:BoundField DataField="Amount" HeaderText="Amount" />
                                <asp:BoundField DataField="AccountShortCode" HeaderText="Account"/>
                                  <asp:BoundField DataField="Status" HeaderText="Status" />
                                <%--<asp:BoundField DataField="Letter" HeaderText="Letter ?"/>--%>
                                 <asp:TemplateField HeaderText="Letter ?">
                            <ItemTemplate >
                                <asp:Label ID="Letter" runat="server" ></asp:Label>
                            </ItemTemplate>
                            </asp:TemplateField>
                            </Columns>
                            <FooterStyle BackColor="White" ForeColor="#000066" />
                            <RowStyle ForeColor="#000066" />
                            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                        </asp:GridView>
                         <table>
    <tr>
    <td style="width: 502px">
     <asp:Button ID="btnadd" runat="server" Text="Add" />
        <asp:Button ID="btnview" runat="server" Text="Edit" Visible="False" />
        <asp:Button ID="btnRemove" runat="server" Text="Remove" Visible="False" />
        <asp:Button ID="Btnpost" runat="server" Text="Post" Visible="False" />
         <asp:Button ID="Btnallocate" runat="server" Text="Allocate" />
        <asp:Label ID="lbltotal" runat="server" Text="Total"></asp:Label>
        <asp:TextBox ID="txttotal" runat="server"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" Text="Close" /></td>
    </tr>
   
    
         </table>
        <br />
        <br />
       
                   
      </div> 
                    </td> 
                    </tr>
                    <tr>
                    <td>
                        <uc2:Footer ID="Footer1" runat="server" />
                    
                    </td></tr></table> 
   
    
  
    </form>
</body>
</html>
