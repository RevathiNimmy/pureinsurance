<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="CDAccountDetails.aspx.vb" Inherits="Nexus.secure_CDAccountDetails"
    EnableViewState="true" %>

<%@ Register Src="~/Controls/BranchPickList.ascx" TagName="BranchPickList" TagPrefix="Branch" %>
<%@ Register Src="~/Controls/ProductPickList.ascx" TagName="ProductPickList" TagPrefix="Product" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script language="javascript" type="text/javascript">
        function AgentClientValidation(source, arguments) {
            if ((document.getElementById("ctl00_cntMainBody_pckBranch_PckBranch_CurrentList") == null) || (document.getElementById("ctl00_cntMainBody_pckProduct_PckProduct_CurrentList") == null)) {
                //first need to check the existance of controls as products list is not populated on page load
                //if the controls are NOT populated yet, so show the error message
                arguments.IsValid = false;
            }
            else {
                var PckBranchSelectedItems = document.getElementById("ctl00_cntMainBody_pckBranch_PckBranch_CurrentList").length;
                var PckProductSelectedItems = document.getElementById("ctl00_cntMainBody_pckProduct_PckProduct_CurrentList").length;

                if ((PckBranchSelectedItems != 0) && (PckProductSelectedItems != 0)) {
                    arguments.IsValid = true;
                }
                else {
                    //select atleast one product or branch
                    arguments.IsValid = false;
                }
            }
        }
    </script>

    <div id="secure_CDAccountDetails">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lt_Heading" runat="server" Text="<%$ Resources:lt_Heading %>"></asp:Literal>
                </h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblPageheader" runat="server" Text="<%$ Resources:lbl_Page_header%>"></asp:Label></legend>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblCDNumber" runat="server" Text="<%$ Resources:lblCDNumber %>" AssociatedControlID="lblCDNumberValue" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <asp:Label ID="lblCDNumberValue" runat="server" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblSinglePolicyLock" runat="server" Text="<%$ Resources:lblSinglePolicyLock %>" AssociatedControlID="chkSinglePOlicyLock" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:CheckBox ID="chkSinglePolicyLock" runat="server" Text=" " CssClass="asp-check"></asp:CheckBox>
                        </div>
                    </div>
                </div>
                <Branch:BranchPickList ID="pckBranch" UseSearch="true" runat="server"></Branch:BranchPickList>
                <Product:ProductPickList ID="pckProduct" UseSearch="true" runat="server"></Product:ProductPickList>
            </div>

            <div class="card-footer">
                <asp:Literal ID="lt_MsgReview" runat="server" Text="<%$ Resources:lt_MsgReview %>"></asp:Literal>
                <asp:LinkButton ID="btnCancel" runat="server" Text="<%$ Resources:btn_Cancel %>" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnSubmit" runat="server" Text="<%$ Resources:btn_Submit %>" ValidationGroup="SubmitCDAccounts" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
        <asp:CustomValidator ID="custvldBranchProductRequired" runat="server" Display="None" ClientValidationFunction="AgentClientValidation" ErrorMessage="<%$ Resources:lbl_Err_BranchProductRequired %>" ValidationGroup="SubmitCDAccounts"></asp:CustomValidator>
        <asp:ValidationSummary ID="ValidationSummary1" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" ValidationGroup="SubmitCDAccounts" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
