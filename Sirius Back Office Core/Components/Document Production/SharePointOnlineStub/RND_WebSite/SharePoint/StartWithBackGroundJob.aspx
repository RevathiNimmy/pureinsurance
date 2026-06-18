<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StartWithBackGroundJob.aspx.cs" Inherits="SharePoint_StartWithBackGroundJob" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">

        <div style="border: 1px solid #cccccc; height: 200px; margin-bottom:40px; margin-top:20px;">
            <div style="width: 95%; float: left;">
                <span style="float: left; margin: 3px;">SharePointOnline Root Document Path :</span>
                <asp:TextBox ID="textBox1" Text="https://ssplimited.sharepoint.com/sites/Test" runat="server" Width="474px"
                    Enabled="false" Style="float: left;"></asp:TextBox>
                <br />
                <br />
                <br />
                <span style="float: left; margin: 3px;">Local File path :</span>
                <asp:TextBox ID="txtlocalfile" Text="" runat="server" Width="474px" Style="float: left;"></asp:TextBox>
                <br />
                <br />
            </div>
            
            <div style="width: 95%; float: left; margin-top:30px;margin-left:15px;">
            <asp:Button ID="btnAddFile" runat="server" Text="Archieve Document from BackGround Job" style="width:340px;" OnClick="btnAddFile_Click" />
             </div>
        </div>


        <div>
            <asp:Button ID="btnGetList" runat="server" OnClick="btnGetList_Click" Text="Get all lists" />
            <br />
            <asp:ListBox ID="lbList" runat="server" Height="138px" Width="167px"></asp:ListBox>
            <asp:Button ID="btnDeleteList" runat="server" Text="Delete List" OnClick="btnDeleteList_Click" />
        </div>
        <div>
            <br />
            Title:<asp:TextBox ID="txtListName" runat="server" Width="474px"></asp:TextBox><br />
            Description:<asp:TextBox ID="txtListDescription" runat="server" Width="474px"></asp:TextBox><br />
            <asp:Button ID="btnCraeteList" runat="server" Text="Create New List Item" OnClick="btnCraeteList_Click" />
        </div>
        <br />

        
        <br />
        <asp:Button ID="btnDeleteFolder" runat="server" Text="DeleteFolder" OnClick="btnDeleteFolder_Click" />

        <br />
        <asp:Button ID="btnCreateFolder" runat="server" Text="CreateFolder" OnClick="btnCreateFolder_Click" />
        <br />
        <br />
        <div style="margin: 50px; background-color: #fbf1f1; color: #17374d">
            <asp:Label ID="lblMessage" runat="server" Font-Bold="true" Font-Size="Small"></asp:Label>
        </div>

    </form>



</body>
</html>
