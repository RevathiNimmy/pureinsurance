<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CoverNoteBook.aspx.vb" Inherits="CoverNote_CoverNoteBook" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
     <script language="javascript"  type ="text/javascript">
    
    function LoadWindows(url)
    {
    window.open(url,"","width=600" ,"height=1000","scrollbars=1")
       }
    
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblSamErrorMessage" runat="server" ForeColor="Red"></asp:Label>
        <table style="width: 648px">
            <tr>
                <td colspan="4">
                    <strong>Cover Note Book Details</strong></td>
            </tr>
            <tr>
                <td style="width: 103px">
                    Book Number</td>
                <td style="width: 111px">
                    <asp:TextBox ID="txtBookNumber" runat="server"></asp:TextBox></td>
                <td colspan="2" rowspan="8">
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="width: 103px">
                    Start Number</td>
                <td style="width: 111px">
                    <asp:TextBox ID="txtStartNumber" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 85px; height: 26px;">
                    End Number</td>
                <td style="width: 111px; height: 26px;">
                    <asp:TextBox ID="txtendnumber" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 103px; height: 26px;">
                    Effective Date</td>
                <td style="width: 111px; height: 26px;">
                    <asp:TextBox ID="txteffectivedate" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 103px; height: 50px;">
                    Agent</td>
                <td style="width: 111px; height: 50px;">
                    <asp:TextBox ID="txtAgent" runat="server"></asp:TextBox>
                    <asp:Button ID="btnFindAgent" runat="server" Text="..." Width="24px" OnClientClick='LoadWindows("FindAgent.aspx")' /></td>
            </tr>
            <tr>
                <td style="width: 103px; height: 21px">
                    Branch</td>
                <td style="width: 111px; height: 21px">
                    <asp:DropDownList ID="ddlBranch" runat="server">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 103px; height: 21px">
                    Book Status</td>
                <td style="width: 111px; height: 21px">
                    <asp:DropDownList ID="ddlCoverNoteStatus" runat="server">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 103px; height: 21px">
                    Created Date</td>
                <td style="width: 111px; height: 21px">
                    <asp:Label ID="lblcreateddate" runat="server"></asp:Label></td>
            </tr>
          
            <tr>
                <td colspan="4" style="height: 21px">
                    </td>
            </tr>
        </table>
        <br />
        <strong><span style="color: #ffffff; background-color: #336699">PRODUCT NAME</span></strong></div>
        <table>
            <tr>
                <td style="width: 199px; height: 318px">
                    <asp:ListBox ID="lstAllProducts" runat="server" Height="321px" SelectionMode="Multiple"
                        Width="194px"></asp:ListBox></td>
                <td style="width: 156px; height: 318px">
                    &nbsp; &nbsp; &nbsp;                 
                    <asp:Button ID="btnSingleAdd" runat="server" Style="z-index: 100; left: 270px; position: absolute;
                        top: 459px" Text=">" Width="41px" />                       
                    <asp:Button ID="BtnRemoveTask" runat="server" Style="z-index: 102; left: 272px; position: absolute;
                        top: 509px" Text="<" Width="41px" />
                    <asp:Button ID="BtnAddAllProducts" runat="server" Style="z-index: 100; left: 272px;
                        position: absolute; top: 558px" Text=">>" Width="41px" />
                    <asp:Button ID="RemoveAllProducts" runat="server" Style="z-index: 100; left: 273px;
                        position: absolute; top: 614px" Text="<<" Width="41px" />
                </td>
                <td style="width: 200px; height: 318px">
                    <asp:ListBox ID="lstSelectedProducts" runat="server" Height="330px" SelectionMode="Multiple"
                        Width="208px"></asp:ListBox></td>
            </tr>
              <tr>
                <td colspan="4" style="height: 21px">
                    <hr />
                    <asp:GridView ID="gvCoverSheet" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        ForeColor="#333333" GridLines="None" Width="100%" RowHeaderColumn="sdfs" ShowFooter="True">
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <Columns>                            
                            <asp:TemplateField HeaderText="Cover Sheet Number">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Customer Name" />
                            <asp:BoundField HeaderText="Status" />
                            <asp:BoundField HeaderText="Policy number" />
                            <asp:BoundField HeaderText="Branch" />
                            <asp:BoundField HeaderText="Agent" />
                            <asp:BoundField HeaderText="Date Imported" />
                        </Columns>
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <EditRowStyle BackColor="#999999" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    </asp:GridView>
                </td>
            </tr>
                        
          
       
            
            
        </table>
        <asp:HiddenField ID="hfAgentKey" runat="server" />        
                    &nbsp;<asp:Button ID="btnAdd" runat="server" Text="Add" Width="72px" Enabled="False" />
                    <asp:Button ID="btnEdit" runat="server" Text="Edit" Width="64px" Enabled="False" />
                    <asp:Button ID="btnDelete" runat="server" Text="Delete" Enabled="False" /><br />
        <asp:Button ID="Button1" runat="server" Text="OK" />
        <asp:Button ID="btncancel" runat="server" Text="Cancel" />
        <asp:Button ID="Btnapply" runat="server" Text="Apply" />     
           
         
      
    </form>
</body>
</html>
