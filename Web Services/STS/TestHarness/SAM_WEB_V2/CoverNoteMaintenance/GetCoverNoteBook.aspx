<%@ Page Language="VB" AutoEventWireup="false" CodeFile="GetCoverNoteBook.aspx.vb" Inherits="CoverNote_CoverNoteBook" %>

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
                    <asp:TextBox ID="txtBookNumber" runat="server" Enabled="False"></asp:TextBox></td>
                <td colspan="2" rowspan="8">
                    &nbsp;<table>
                        <tr>
                            <td style="width: 100px">
                                Available Product</td>
                            <td style="width: 100px">
                                <asp:ListBox ID="lstAllProducts" runat="server" Height="321px" SelectionMode="Multiple"
                                    Width="194px"></asp:ListBox></td>
                            <td style="width: 100px">
                                &nbsp; &nbsp;
                                <asp:Button ID="btnSingleAdd" runat="server"  Text=">" Width="41px" />
                                <asp:Button style="Z-INDEX: 100; " id="BtnAddAllProducts" runat="server" Width="41px" Text=">>"></asp:Button>
                                <asp:Button style="Z-INDEX: 100; " id="RemoveAllProducts" runat="server" Width="41px" Text="<<"></asp:Button>
                            
                                <asp:Button ID="BtnRemoveTask" runat="server" Text="<" Width="41px" /></td>
                            <td style="width: 100px">
                                Chosen Product</td>
                            <td style="width: 100px">
                    <asp:ListBox ID="lstSelectedProducts" runat="server" Height="330px" SelectionMode="Multiple"
                        Width="208px"></asp:ListBox></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="width: 103px">
                    Start Number</td>
                <td style="width: 111px">
                    <asp:TextBox ID="txtStartNumber" runat="server" Enabled="False"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 85px; height: 26px;">
                    End Number</td>
                <td style="width: 111px; height: 26px;">
                    <asp:TextBox ID="txtendnumber" runat="server" Enabled="False"></asp:TextBox></td>
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
                    <hr />
                    <asp:GridView ID="gvCoverSheet" runat="server" CellPadding="4"
                        ForeColor="#333333" GridLines="None" Width="100%" AutoGenerateColumns="false">
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <Columns>
                           <asp:CommandField ShowSelectButton="True"  />
                            <asp:BoundField HeaderText="Cover Sheet Number" DataField="CoverNoteSheetNumber" />
                            <asp:BoundField HeaderText="Customer Name" DataField="CustomerName" />
                            <asp:BoundField HeaderText="Status" DataField="CoverNoteSheetStatusDescription" />
                            <asp:BoundField HeaderText="Policy number" DataField="PolicyNumber" />
                            <asp:BoundField HeaderText="Branch"  DataField="BranchName" />
                            <asp:BoundField HeaderText="Agent" DataField="AgentName" />
                            <asp:BoundField HeaderText="Date Imported" DataField="DateImported" />
                            <asp:BoundField HeaderText="CoverNoteSheetKey" DataField="CoverNoteSheetKey" />
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
            <tr>
                <td colspan="4" style="height: 21px">
                    </td>
            </tr>
        </table>
        <br />
        <strong><span style="color: #ffffff; background-color: #336699">PRODUCT NAME</span></strong></div>
        
        <asp:HiddenField ID="hfAgentKey" runat="server" />
        <br />
                    <hr />
                    <asp:Button ID="btnaddcovernotesheet" runat="server"  Text="Add" />&nbsp;
                    <asp:Button ID="btnEdit" runat="server" Text="Edit" Width="64px" />
                    <asp:Button ID="btnDelete" runat="server" Text="Delete" />&nbsp;
        <br />
        <asp:Button ID="BtnOk" runat="server" Text="Ok" Width="72px" />
        <asp:Button ID="btnAdd" runat="server" Text="Apply" Width="72px" />
        <asp:Button ID="Btncancel" runat="server" Text="Cancel" Width="72px" /><br />
           
    </form>
</body>
</html>
