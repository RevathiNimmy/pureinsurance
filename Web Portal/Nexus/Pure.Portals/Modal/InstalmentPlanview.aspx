<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="InstalmentPlanview.aspx.vb" Inherits="Nexus.Modal_InstalmentPlanview" %>

<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register Src="~/Controls/InstallmentDetails.ascx" TagName="InstallmentDetails"
    TagPrefix="uc1" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <asp:ScriptManager ID="smModalInstalmentPlanView" runat="server" />
    <div id="Modal_InstalmentPlanView">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Label ID="lblTitle" runat="server" Text="<%$ Resources:lbl_InstalmentPlan_Title %>" /></h1>
            </div>
            <div class="card-body clearfix">
                <uc1:InstallmentDetails ID="ucInstalmentDetails" runat="server" />
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnOK" runat="server" Text="<%$ Resources:lbl_btnOk %>" SkinID="btnPrimary" CausesValidation="False" />
            </div>
        </div>
    </div>
</asp:Content>
