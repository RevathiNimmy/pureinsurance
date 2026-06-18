<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CheckUnPaidStatus.aspx.vb"
    Inherits="Nexus.Claims_CheckUnPaidStatus" MasterPageFile="~/default.master" %>

<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script type="text/javascript">

        function ClaimAbortConfirmation(sender, args) {

            var answer = confirm("Aborting the process will lose all your current details. Do you wish to continue?")

            if (answer) {

                return true;

            }

            else {

                return false;

            }
        }

    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div id="Claims_DuplicateClaimCheck">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal runat="server" Text="<%$ Resources:lbl_CheckunPaidStatus_title %>" ID="Literal1"></asp:Literal>
                </h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPolicynumber" runat="server" AssociatedControlID="txtPolicyNo" Text="<%$ Resources:lbl_CheckunPaidStatus_Policynumber %>" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtPolicyNo" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="Label1" runat="server" AssociatedControlID="txtClaimDate" Text="<%$ Resources:lbl_CheckunPaidStatus_claimdate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtClaimDate" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblClaimNumber" runat="server" AssociatedControlID="txtClaimNumber" Text="<%$ Resources:lbl_CheckunPaidStatus_claimno %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtClaimNumber" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>


                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblClient" runat="server" AssociatedControlID="txtClientName" Text="<%$ Resources:lbl_CheckunPaidStatus_Clientname %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtClientName" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblOverDueInsalments" runat="server" AssociatedControlID="txtOverdueInstalments" Text="<%$ Resources:lbl_CheckunPaidStatus_OverdueIns %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtOverdueInstalments" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblShowOutstandingOnly" runat="server" AssociatedControlID="chkIsShowOutstandingOnly" Text="<%$ Resources:lbl_CheckunPaidStatus_Showoutstanding %>" ReadOnly="true" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:CheckBox ID="chkIsShowOutstandingOnly" runat="server" AutoPostBack="true" Text=" " CssClass="asp-check"></asp:CheckBox>
                        </div>

                    </div>
                </div>
            </div>
            <div class="card-body clearfix">
                <div class="grid-card table-responsive clearfix">
                    <asp:GridView ID="grdPolicyTransactions" runat="server" AutoGenerateColumns="false" GridLines="None" AllowSorting="true" EmptyDataRowStyle-CssClass="noData" EmptyDataText="Records no Found">
                        <Columns>
                            <asp:BoundField DataField="BranchDescription" HeaderText="<%$ Resources:lbl_gv_Branchname %>" SortExpression="BranchDescription"></asp:BoundField>
                            <asp:BoundField DataField="ShortCode" HeaderText="<%$ Resources:lbl_gv_Account %>" SortExpression="ShortCode"></asp:BoundField>
                            <asp:BoundField DataField="DocRef" HeaderText="<%$ Resources:lbl_gv_DocRef %>" SortExpression="DocRef"></asp:BoundField>
                            <Nexus:BoundField DataField="DocumentDate" HeaderText="<%$ Resources:lbl_gv_transdate %>" SortExpression="DocumentDate" DataFormatString="{0:d}"></Nexus:BoundField>
                            <Nexus:BoundField HeaderText="<%$ Resources:lbl_gv_Amount %>" DataField="Amount" SortExpression="Amount" DataType="Currency"></Nexus:BoundField>
                            <Nexus:BoundField HeaderText="<%$ Resources:lbl_gv_OSAmount %>" DataField="OutstandingAmount" DataType="Currency" SortExpression="OutstandingAmount"></Nexus:BoundField>
                            <asp:BoundField DataField="DocumentType" HeaderText="<%$ Resources:lbl_gv_doctype %>" SortExpression="DocumentType"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

            <div class="card-footer">
                <asp:LinkButton ID="btnAbort" runat="server" Text="<%$ Resources:btn_CheckunPaidStatus_btnabort %>" OnClientClick="return ClaimAbortConfirmation();" CausesValidation="false" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnContinue" runat="server" Text="<%$ Resources:btn_CheckunPaidStatus_btnContinue %>" CausesValidation="false" SkinID="btnPrimary"></asp:LinkButton>
                
            </div>
        </div>
        <asp:HiddenField ID="hdndueinstalments" runat="server"></asp:HiddenField>
    </div>
</asp:Content>


