<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ReAssignTask.aspx.vb" Inherits="Work_Manager_Exposure_ReAssignTask" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>

    <script language="javascript" type="text/javascript">
// <!CDATA[

function TABLE1_onclick() {

}

// ]]>
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:GridView ID="gvSelectedTask" runat="server" Style="z-index: 100; left: 34px;
                position: absolute; top: 83px">
            </asp:GridView>
            &nbsp; &nbsp;&nbsp; &nbsp;<tr>
                <asp:Button ID="btnOk" Text="OK" runat="server" Style="z-index: 103; left: 37px;
                    position: absolute; top: 272px" />
            </tr>
            <table id="TABLE1" style="z-index: 104; left: 23px; position: absolute; top: 23px"
                onclick="return TABLE1_onclick()">
                <tr>
                    <td style="width: 150px; height: 5px">
                        <asp:DropDownList ID="ddlUserGroup" runat="server" Style="z-index: 101; left: 9px;
                            position: absolute; top: 1px" AutoPostBack="True" Width="132px">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 104px; height: 5px">
                        <asp:DropDownList ID="ddlUser" runat="server" Style="z-index: 102; left: 163px; position: absolute;
                            top: 0px" Width="99px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px; height: 21px">
                    </td>
                    <td style="width: 104px; height: 21px">
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
