<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="true" CodeFile="~/Modal/OverridePolicyNumber.aspx.vb"
    Inherits="Nexus.Modal_OverridePolicyNumber" Title="Untitled Page" EnableViewState="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script language="javascript" type="text/javascript">

        String.prototype.trim = function () {
            return this.replace(/^\s*/, "").replace(/\s*$/, "");
        }

        var bStatus = null;
        function CheckDuplicateRecord(source, arguments) {

            var PolicyNo = document.getElementById('<%=txtPolicyNumber.ClientID%>').value;

            if (PolicyNo.length != 0 && bStatus == null) {
                PageMethods.CheckDuplicateRecord(PolicyNo, CallSuccess, CallFailed);
            }
            else if (PolicyNo.length != 0 && bStatus == true) {
                arguments.IsValid = true;
                UpdateData();
            }
            else if (PolicyNo.length != 0 && bStatus == false) {
                arguments.IsValid = false;
            }
            else {
                arguments.IsValid = true;
            }
        }

        // set the destination textbox value with the ContactName
        function CallSuccess(res, destCtrl) {
            bStatus = res;
            alert("res:" + res)
            alert("status:" + bStatus)
            Page_ClientValidate();
        }

        // alert message on some failure
        function CallFailed(res, destCtrl) {
            alert(res.get_message());
        }

        function UpdateData() {
            var PolicyNo = document.getElementById('<%=txtPolicyNumber.ClientID%>').value;
            self.parent.tb_remove();
            self.parent.setPolicyNumber(PolicyNo);
        }

        function CloseBox() {
            self.parent.tb_remove();
        }
    </script>

    <asp:ScriptManager ID="ScriptManagerOverridePolicyNo" runat="server" EnablePageMethods="true"></asp:ScriptManager>
    <div class="OverridePolicyNumber">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Label ID="lblHeading1" runat="server" Text="<%$ Resources:lbl_Override_Details %>"></asp:Label>
                </h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <p>
                        <asp:Literal ID="lblDocumentsText" runat="server" Text="<%$ Resources:lbl_DocumentsText %>"></asp:Literal>
                    </p>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPolicyNumber" runat="server" AssociatedControlID="txtPolicyNumber" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="ltPolicyNumber" runat="server" Text="<%$ Resources:lbl_Override_PolicyNumber %>"></asp:Literal></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtPolicyNumber" runat="server" MaxLength="30" CssClass="form-control"></asp:TextBox>
                        </div>
                        <asp:RegularExpressionValidator ID="regexpName" runat="server" ErrorMessage="<%$ Resources:lblPolicyNumber %>" Display="none" SetFocusOnError="True" ControlToValidate="txtPolicyNumber" ValidationExpression="[A-Za-z0-9^]*"></asp:RegularExpressionValidator>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnCancel" runat="server" Text="<%$ Resources:lbl_PolicyOverride_Cancel %>" CausesValidation="false" OnClientClick="CloseBox()" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnSubmit" runat="server" Text="<%$ Resources:lbl_PolicyOverride_Submit %>" CausesValidation="true" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
        <asp:CustomValidator ID="custValidator" runat="server" ErrorMessage="<%$ Resources:lbl_DuplicatePolicyNo %>" Display="None" ClientValidationFunction="CheckDuplicateRecord" EnableClientScript="false"></asp:CustomValidator>
        <asp:ValidationSummary ID="ValidationSummary" ShowSummary="true" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_OverridePolicyNumber_ValidationSummary %>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
    </div>

</asp:Content>
