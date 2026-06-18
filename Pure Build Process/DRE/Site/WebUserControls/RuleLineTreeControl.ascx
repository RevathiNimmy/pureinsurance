<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    Inherits="WebUserControls_RuleLineTreeControl" 
    Codebehind="RuleLineTreeControl.ascx.cs" %>
<%@ Register 
    Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI"
    TagPrefix="ajax" 
%>

<script type="text/javascript">
/*<![CDATA[*/
function LeafNodeClicked(ruleLineGuid)
{
    var ele = document.getElementById('<%=btnClientGuidPostButton.ClientID%>');
    ele.value = ruleLineGuid;
    ele.click();       
}

function setScroll(divTree) 
{
    var hfScrollPosistion = $get('<%= hfScrollPosistion.ClientID %>');
    if (hfScrollPosistion) hfScrollPosistion.value = divTree.scrollTop;
}

function pageLoad()
{
    var divTree = $get('ruleLinesTree');
    var hfScrollPosistion = $get('<%= hfScrollPosistion.ClientID %>');
    if (divTree)
    {
        divTree.scrollTop = hfScrollPosistion.value;
    }
}
/*]]>*/
</script>

<asp:HiddenField ID="hfSelectedNode" runat="server" />
<asp:HiddenField ID="hfScrollPosistion" runat="server" />

<div id="ruleLinesTree" class="ruleLinesTree" onscroll="javascript:setScroll(this);">
    <asp:TreeView ID="tvRuleLines" CssClass="ruleLineTreeControl" runat="server" NodeWrap="false" />
</div>

<div style="display:none">
    <ajax:UpdatePanel ID="updDummyForClientDetailPost" runat="server" UpdateMode="Conditional" EnableViewState="true">
        <ContentTemplate>
            <asp:Button runat="server" CausesValidation="false" ID="btnClientGuidPostButton" />
        </ContentTemplate>
    </ajax:UpdatePanel>
</div>
