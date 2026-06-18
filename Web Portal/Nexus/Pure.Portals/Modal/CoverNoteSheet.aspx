<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CoverNoteSheet.aspx.vb"
    Inherits="Nexus.Modal_CoverNoteSheet" Title="Untitled Page" MasterPageFile="~/default.master" %>

<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script language="javascript" type="text/javascript">
        function updated() {
            tb_remove();
        }

        function validateComment(source, args) {
            if (args.Value.length > 1024) {
                args.IsValid = false;
            }
            else {
                args.IsValid = true;
            }
        }

    </script>

    <div id="Modal_CoverNoteSheet">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Label runat="server" ID="lblFindCoverNoteBook" Text="<%$ Resources:lbl_CoverNoteSheet %>"></asp:Label>
                </h1>
            </div>
            <div id="Div1" class="card-body clearfix" runat="server">
                <div class="form-horizontal">
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblSheetNumber" runat="server" AssociatedControlID="txtSheetNumber" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="litSheetNumber" runat="server" Text="<%$ Resources:lbl_SheetNumber %>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                <asp:TextBox ID="txtSheetNumber" CssClass="field-medium form-control" runat="server"></asp:TextBox>
                            </div>
                        <asp:CustomValidator ID="cusValidSheetNo" runat="server" SetFocusOnError="true" ErrorMessage="<%$ Resources:Err_VldSheetNumber %>" Display="None"></asp:CustomValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPolicyNumber" runat="server" AssociatedControlID="txtPolicyNumber" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="litPolicyNumber" runat="server" Text="<%$ Resources:lbl_PolicyNumber %>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                <asp:TextBox ID="txtPolicyNumber" CssClass="field-medium form-control" runat="server"></asp:TextBox>
                            </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lbl_AssignedDate" runat="server" AssociatedControlID="txtAssignedDate" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="litAssignedDate" runat="server" Text="<%$ Resources:lbl_AssignedDate%>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                <div class="input-group">
                                    <asp:TextBox ID="txtAssignedDate" Enabled="false" runat="server" CssClass="field-date form-control"></asp:TextBox><uc1:CalendarLookup ID="calAssignedDate" runat="server" LinkedControl="txtAssignedDate" HLevel="2"></uc1:CalendarLookup>
                                </div>
                            </div>

                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblSheetStatus" runat="server" AssociatedControlID="ddlSheetStatus" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="litSheetStatus" runat="server" Text="<%$ Resources:lbl_SheetStatus%>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                <asp:DropDownList ID="ddlSheetStatus" runat="server" CssClass="form-control form-select"></asp:DropDownList>
                            </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblComments" runat="server" AssociatedControlID="litlblComments" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="litlblComments" runat="server" Text="<%$ Resources:lblComments %>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                <asp:TextBox ID="txtComments" CssClass="field-medium form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        <asp:CustomValidator ID="cvComment" runat="server" ControlToValidate="txtComments" Display="None" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_invalidComment %>" ClientValidationFunction="validateComment"></asp:CustomValidator>
                    </div>
                    <asp:Panel CssClass="error" ID="PnlError" runat="server" Visible="false">
                        <asp:Label runat="server" ID="lblError" ForeColor="black"></asp:Label>
                    </asp:Panel>
                </div>

            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnOk" runat="server" Text="<%$ Resources:lbl_Ok %>" CausesValidation="true" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
        <asp:ValidationSummary ID="ValidationSummary" ShowSummary="true" HeaderText="<%$ Resources:lbl_ValidationSummary %>" DisplayMode="BulletList" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
