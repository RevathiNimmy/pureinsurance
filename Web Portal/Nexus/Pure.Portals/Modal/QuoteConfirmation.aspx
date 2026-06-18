<%@ Page Title="" Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="QuoteConfirmation.aspx.vb" Inherits="Nexus.Modal_QuoteConfirmation" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <asp:ScriptManager ID="scriptMgr1" runat="server"></asp:ScriptManager>
    <div id="Modal_ProspectPolicies">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblTitle" runat="server" Text="<%$ Resources:lblTitle %>"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <div class="doublewidth form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblType" runat="server" Text="<%$ Resources:lblMessage %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:UpdatePanel ID="updPanelClientClaims" runat="server">
                    <ContentTemplate>
                        <asp:LinkButton ID="btnCancel" runat="server" Text="<%$ Resources:btnCancel %>" SkinID="btnSecondary"></asp:LinkButton>
                        <asp:LinkButton ID="btnDelete" runat="server" Text="<%$ Resources:btnDelete %>" SkinID="btnSecondary"></asp:LinkButton>
                        <asp:LinkButton ID="btnComplete" runat="server" Text="<%$ Resources:btnComplete %>" SkinID="btnPrimary"></asp:LinkButton>
                        
                    </ContentTemplate>
                </asp:UpdatePanel>
                <Nexus:ProgressIndicator ID="upClientClaims" OverlayCssClass="updating" AssociatedUpdatePanelID="updPanelClientClaims" runat="server">
                    <ProgressTemplate>
                    </ProgressTemplate>
                </Nexus:ProgressIndicator>
            </div>
        </div>

    </div>
</asp:Content>
