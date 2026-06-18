<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="RenewalStatusType.aspx.vb" Inherits="Nexus.Modal_RenewalStatusType"
    Title="Renewal Status" %>

<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <div id="Modal_RenewalStatusType">
        <div class="card">
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblStatus" runat="server" Text="<%$ Resources:lbl_Heading %>"></asp:Label></legend>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblStatusType" runat="server" AssociatedControlID="RenewalStatusType" Text="<%$ Resources:lbl_StatusType %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:DropDownList ID="RenewalStatusType" runat="server" CssClass="field-large form-control form-select">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnUpdateStatus" runat="server" Text="<%$ Resources:btn_ChangeStatus %>" ValidationGroup="RenewalStatusGroup" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
        <asp:ValidationSummary ID="ValidationSummary" ShowSummary="true" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" ValidationGroup="RenewalStatusGroup" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
