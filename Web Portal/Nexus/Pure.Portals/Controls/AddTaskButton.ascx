<%@ Control Language="VB" AutoEventWireup="false" CodeFile="AddTaskButton.ascx.vb"
    Inherits="Nexus.AddTaskButtonCntrl" EnableViewState="false" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<script language="Javascript" type="text/javascript">

    function getAbsolutePath(requesturl) {

        var loc = window.location;

        var pathName = loc.pathname.substring(1, loc.pathname.lastIndexOf('/') + 1);

        var mySplitResult = pathName.split("/");

        var getActualPath = '';

        for (i = 0; i < mySplitResult.length - 2; i++) {

            getActualPath = getActualPath + "../";

        }



        var setActualPath = getActualPath + requesturl;

        tb_show(null, setActualPath, null);

        return false;

    }

</script>



<span id="Controls_AddTaskButton">
    <asp:LinkButton ID="btnAddTask" runat="server" SkinID="btnPrimary" CausesValidation="false" Text="<%$ Resources:lbl_AddTask %>"
        OnClientClick="getAbsolutePath('Modal/WrmTask.aspx?modal=true&FromPage=WM&KeepThis=true&TB_iframe=true&height=550&width=850');return false;" />
</span>
