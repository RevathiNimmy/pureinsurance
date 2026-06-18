<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="ChangeClaim.aspx.vb" Inherits="Nexus.Claims_ChangeClaim" Title="Untitled Page" %>

<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <div id="Claims_ChangeClaim">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal runat="server" Text="<%$ Resources:ltPageHeading %>" ID="ltPageHeading"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblReasonForChange" runat="server" Text="<%$ Resources:lbl_ReasonForChange %>"></asp:Label></legend>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblReason" runat="server" AssociatedControlID="ddlReason" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="ltReason" runat="server" Text="<%$ Resources:lbl_Reason%>"></asp:Literal></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:DropDownList ID="ddlReason" runat="server" CssClass="field-medium form-control form-select"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblIfOther" runat="server" AssociatedControlID="txtIfOther" Text="<%$ Resources:lbl_IfOther%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtIfOther" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnOk" runat="server" ValidationGroup="ChangeClaim" Text="<%$ Resources:btnOk %>" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
        <asp:HiddenField ID="hidChkChoice" runat="server" Value="False"></asp:HiddenField>
        <asp:HiddenField ID="hidChlClaimClose" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hidChkPaymentMsg" runat="server"></asp:HiddenField>
        <asp:ValidationSummary ID="ValidationSummary" DisplayMode="BulletList" ValidationGroup="ChangeClaim" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
