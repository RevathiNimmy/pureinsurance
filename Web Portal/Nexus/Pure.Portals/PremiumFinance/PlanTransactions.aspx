<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/default.master"
    CodeFile="PlanTransactions.aspx.vb" Inherits="PremiumFinance_PlanTransactions" MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="~/Controls/PlanTransactions.ascx" TagName="PlanTransactions" TagPrefix="uc1" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <script type="text/javascript" language="javascript">
        function tb_updated(postbackargument, postbacktarget) {
            tb_remove();
            __doPostBack(postbackargument, postbacktarget);

        }
    </script>
    <div id="secure_FinancePlanDetails">
        <uc1:PlanTransactions ID="ucPlanTransactions" runat="server"></uc1:PlanTransactions>
    </div>
</asp:Content>
