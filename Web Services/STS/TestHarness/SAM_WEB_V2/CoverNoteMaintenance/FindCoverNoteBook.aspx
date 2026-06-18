<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FindCoverNoteBook.aspx.vb"
    Inherits="Cover_Note_Maintenance_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
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
            <div style="text-align: left">
                <table cellpadding="5" cellspacing="0" border="1">
                    <tr>
                        <td style="width: 100px; ">
                            <span style=" font-family: Palatino Linotype; white-space: nowrap"><strong>
                                Book Number</strong></span></td>
                        <td style="width: 100px; ">
                            <asp:TextBox ID="txtBookNumber" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td style="width: 100px; ">
                            <strong><span style="font-family: Palatino Linotype; white-space: nowrap">
                                Start Number</span></strong></td>
                        <td style="width: 100px; ">
                            <asp:TextBox ID="txtStartNumber" runat="server"></asp:TextBox></td>
                             <td style="width: 100px;  white-space: nowrap">
                            <strong><span style=" font-family: Palatino Linotype; white-space: nowrap">
                                End Number</span></strong></td>
                        <td style="width: 100px;">
                            <asp:TextBox ID="txtEndNumber" runat="server"></asp:TextBox></td>
                             <td style="width: 100px; white-space: nowrap; ">
                            <asp:Button ID="btnFind" runat="server" Text="Find Now" /></td>
                       
                       
                    </tr>
                    <tr>
                       
                    </tr>
                    <tr>
                        <td style="width: 100px; white-space: nowrap;">
                            <strong><span style="font-family: Palatino Linotype; white-space: nowrap">
                                AgentKey</span></strong></td>
                        <td style="width: 100px; ">
                            <asp:TextBox ID="txtAgent" runat="server"></asp:TextBox>
                            <asp:Button ID="btnFindAgent" runat="server"
                                Text="..." Width="24px" OnClientClick='LoadWindows("FindAgent.aspx")'/></td>
                                 <td style="width: 100px; white-space: nowrap; ">
                            <strong><span style=" font-family: Palatino Linotype; white-space: nowrap">
                                Last Updated</span></strong></td>
                        <td style="width: 100px; ">
                            <asp:TextBox ID="txtLastUpdated" runat="server"></asp:TextBox></td>
                                                  
                             <td style="width: 100px; white-space: nowrap; ">
                            <asp:Button ID="BtnNewsearch" runat="server" Text="New search" /></td>
                       
                    </tr>
                    <tr>
                        <td style="width: 100px; white-space: nowrap; ">
                            <strong><span style=" font-family: Palatino Linotype">Branch</span></strong></td>
                        <td style="width: 100px; ">
                            <asp:DropDownList ID="ddlCoverNoteBranchCode" runat="server">
                            </asp:DropDownList></td>
                               <td style="width: 100px; white-space: nowrap;">
                            <strong><span style=" font-family: Palatino Linotype; white-space: nowrap">
                                Policy Number</span></strong></td>
                        <td style="width: 100px;">
                            <asp:TextBox ID="txtPolicyNumber" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                       
                    </tr>
                    <tr>
                        <td style="width: 100px; white-space: nowrap; ">
                            <strong><span style=" font-family: Palatino Linotype; white-space: nowrap">
                                ConverNoteStatus</span></strong></td>
                        <td style="width: 100px; ">
                            <asp:DropDownList ID="ddlConverNoteStatusCode" runat="server">
                            </asp:DropDownList></td>
                             <td style="width: 100px; white-space: nowrap;">
                            <strong><span style=" font-family: Palatino Linotype; white-space: nowrap">
                                Assigned Date</span></strong></td>
                        <td style="width: 100px; ">
                            <asp:TextBox ID="txtAssignedDate" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                     
                    </tr>
                    <tr>
                       
                    </tr>
                    <tr>
                       
                    </tr>
                </table>
              
                   <asp:GridView ID="gvFindCoverNoteBook" runat="server" CellPadding="4"
                        ForeColor="#333333" GridLines="None" Width="100%" AutoGenerateColumns="false">
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <Columns>
                           <asp:CommandField ShowSelectButton="True"  />
                            <asp:BoundField HeaderText="Book Number" DataField="BookNumber" />
                            <asp:BoundField HeaderText="Start Number" DataField="StartNumber" />
                            <asp:BoundField HeaderText="End Number" DataField="EndNumber" />
                            <asp:BoundField HeaderText="Agent" DataField="AgentName" />
                            <asp:BoundField HeaderText="Status"  DataField="CoverNoteStatusDescription" />
                            <asp:BoundField HeaderText="Branch" DataField="CoverNoteBranchDescription" />
                            <asp:BoundField HeaderText="Date Updated" DataField="LastUpdated" />
                            <asp:BoundField HeaderText="Created Date" DataField="DateCreated" />
                        </Columns>
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <EditRowStyle BackColor="#999999" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    </asp:GridView>
                <asp:HiddenField ID="hfAgentKey" runat="server" />
                <br />
                <asp:Button ID="BtnNew" runat="server" Text="New" />
                <asp:Button ID="BtnEdit" runat="server" Text="Edit" />
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp;
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                &nbsp; &nbsp;
                <asp:Button ID="BtnClose" runat="server" Text="Close" /><br />
                <br />
            </div>
        </div>
    </form>
</body>
</html>
