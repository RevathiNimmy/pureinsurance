<%@ Control Language="VB" AutoEventWireup="false" CodeFile="MultiSelectDD.ascx.vb"
    Inherits="Nexus.Controls_MultiSelectDD" EnableViewState="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<script language="javascript" type="text/javascript">

    function CheckItem(checkBoxList) {
        var options = checkBoxList.getElementsByTagName('input');
        var arrayOfCheckBoxLabels = checkBoxList.getElementsByTagName("label");
        var s = "";

        for (i = 0; i < options.length; i++) {
            var opt = options[i];
            if (opt.checked) {
                s = s + ", " + arrayOfCheckBoxLabels[i].innerText;
            }
        }
        if (s.length > 0) {
            s = s.substring(2, s.length);
        }
        var TxtBox = document.getElementById("<%=txtCombo.ClientID%>");
        TxtBox.value = s;
        document.getElementById('<%=hidVal.ClientID %>').value = s;
    }

    function DropDownExtender111_pageLoad() {
        $find('DDEBI').unhover = doNothing;
        $find('DDEBI')._dropWrapperHoverBehavior_onhover();
    }
    function doNothing() { }
    Sys.Application.add_load(DropDownExtender111_pageLoad);


</script>
<style>
    #ctl00_cntMainBody_MultiSelectDD1_Panel111 table tr td input {
        margin-right: 5px;
    }

    .ajax__dropdown_arrow_wrapper {
        top: 30%;
        left: 40%;
    }
</style>

<asp:TextBox ID="txtCombo" runat="server" ReadOnly="true" Width="100%" CssClass="form-control"></asp:TextBox>
<%--<input id="btnDropDown" runat="server" value=" v " type="button" causesvalidation="false"
    style="width: 32px" />--%>
<%-- <asp:Image ID="imgDropDown" runat="server" ImageUrl="../images/DDImage.bmp" />--%>
<%--<cc1:PopupControlExtender ID="PopupControlExtender111" runat="server" TargetControlID="imgDropDown"
    PopupControlID="Panel111" Position="Bottom" OffsetX="-250" OffsetY="5">
</cc1:PopupControlExtender>--%>
<cc1:DropDownExtender ID="DropDownExtender111" runat="server" Enabled="true" TargetControlID="txtCombo" DropDownControlID="Panel111" DropArrowImageUrl="~/images/ddl-arrow.png" BehaviorID="DDEBI" HighlightBackColor="White" HighlightBorderColor="LightGray" DropArrowBackColor="White">
</cc1:DropDownExtender>
<input type="hidden" name="hidVal" id="hidVal" runat="server">
<asp:Panel ID="Panel111" runat="server" ScrollBars="Vertical" Width="28.4%" Height="150" CssClass="md-whiteframe-z0 grey-50 p-sm">
    <asp:CheckBoxList ID="chkList" runat="server" Height="250" onclick="CheckItem(this)" CssClass="asp-check">
    </asp:CheckBoxList>
</asp:Panel>

