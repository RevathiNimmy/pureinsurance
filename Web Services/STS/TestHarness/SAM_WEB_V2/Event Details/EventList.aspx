<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EventList.aspx.vb" Inherits="Event_Details_EventList" %>

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
        <br />
        <br />
        <br />
        <table style="width: 518px">
            <tr>
                <td style="width: 112px">
                    <asp:Button ID="btnPolicyCode" runat="server" Text="Policy Code" OnClientClick='LoadWindows("FindInsuranceFile.aspx")' /></td>
                <td style="width: 222px">
                    <asp:TextBox ID="txtPolicyCode" runat="server"></asp:TextBox></td>
                <td style="width: 101px">
                    <asp:Button ID="btnClaimCode" runat="server" Text="Claim Code" OnClientClick='window.open("1_FindClaim.aspx")' /></td>
                <td style="width: 102px">
                    <asp:TextBox ID="txtClaimCode" runat="server"></asp:TextBox>
                    <asp:HiddenField ID="hfClaimKey" runat="server" />
                </td>
                <td style="width: 9135px">
                    <asp:Button ID="btnRefresh" runat="server" Text="Refresh" /></td>
            </tr>
            <tr>
                <td style="width: 112px">
                    Event Type</td>
                <td style="width: 222px">
                    <asp:DropDownList ID="ddlEventType" runat="server" Width="153px" AutoPostBack="True">
                        <asp:ListItem>All</asp:ListItem>
                    </asp:DropDownList></td>
                <td style="width: 101px">
                    User Name</td>
                <td style="width: 102px">
                    <asp:DropDownList ID="ddlUserName" runat="server" Width="152px" AutoPostBack="True">
                        <asp:ListItem>All</asp:ListItem>
                        <asp:ListItem>sirius</asp:ListItem>
                        <asp:ListItem>siriuscomm</asp:ListItem>
                    </asp:DropDownList></td>
                <td style="width: 9135px">
                </td>
            </tr>
            <tr>
                <td style="width: 112px; height: 26px;">
                    From date</td>
                <td style="width: 222px; height: 26px;">
                    <asp:TextBox ID="txtFromDate" runat="server"></asp:TextBox></td>
                <td style="width: 101px; height: 26px;">
                    To date</td>
                <td style="width: 102px; height: 26px">
                    <asp:TextBox ID="txtToDate" runat="server"></asp:TextBox></td>
                <td style="width: 9135px; height: 26px">
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <hr />
                    &nbsp;<asp:Panel ID="Panel1" runat="server" Height="200px" ScrollBars="Vertical"
                        Width="100%">
                    <asp:GridView ID="gvEventList" runat="server" CellPadding="4" ForeColor="#333333"
                        GridLines="None" AutoGenerateColumns="False" Width="1533px">
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#F7F6F3" Font-Size="Smaller" ForeColor="#333333" />
                        <EditRowStyle BackColor="#999999" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:CommandField ShowSelectButton="True" />
                             <asp:BoundField DataField="EventDate" HeaderText="Event Date" />
                            <asp:BoundField DataField="EventType" HeaderText="EventType" />
                            <asp:BoundField DataField="PolicyCode" HeaderText="Policy Code" />
                            <asp:BoundField DataField="ClaimNumber" HeaderText="Claim Number" />
                            <asp:BoundField DataField="Description" HeaderText="Description" /> 
                            <asp:BoundField DataField="UserName" HeaderText="UserName" />   
                            <asp:BoundField DataField="Priority" HeaderText="Priority" />
                            <%--<asp:BoundField DataField="Status" HeaderText="Status" /> --%>
                            <asp:TemplateField HeaderText="Status">
                            <ItemTemplate >
                                <asp:Label ID="Status" runat="server" ></asp:Label>
                            </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    </asp:Panel>
                </td>
                <td colspan="1" style="width: 9135px">
                </td>
            </tr>
            <tr>
                <td style="width: 112px">
                    <asp:Button ID="btnAdd" runat="server" Text="AddNotes" Width="112px" />
                    <asp:Button ID="BtnViewnote" runat="server" Text="View Notes" Width="112px" /></td>
                <td style="width: 222px">
                    <asp:Button ID="btnEventNotes" runat="server" OnClientClick='window.open("EventNotes.aspx")'
                        Text="Event Notes" />
                    <asp:Button ID="btnOk" runat="server" Text="Ok" Width="53px" PostBackUrl="~/Client Manager/ClientManager.aspx" />
                    <asp:Button ID="BtnCancel" runat="server" Text="Cancel" Width="53px" PostBackUrl="~/Client Manager/ClientManager.aspx" /></td>
                <td colspan="4">
                    &nbsp;</td>
            </tr>
        </table>
    
    </div>
        <asp:HiddenField ID="hfinsurancekey" runat="server" />
    </form>
</body>
</html>
