<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="ClaimCaseChange.aspx.vb" Inherits="Nexus.Modal_ClaimCaseChange" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <div id="Portal_Templates_ClaimCaseChange">
        <asp:Panel ID="pnlNewCase" runat="server" DefaultButton="btnCancel">
            <div class="card">
                <div class="card-heading">
                    <h1>
                        <asp:Literal ID="lblTitle" runat="server" Text="<%$ Resources:lbl_Title %>"></asp:Literal></h1>
                </div>
                <div class="card-body clearfix">
                    <div class="form-horizontal">
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12" runat="server" id="liDescription">
                            <asp:Label ID="lblChangeDescription" runat="server" AssociatedControlID="txtChangeDescription" Text="<%$ Resources:lbl_ChangeDesc %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:TextBox ID="txtChangeDescription" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12" runat="server" id="liConfirmation" visible="false">
                            <asp:Label ID="lblCaseUpdated" runat="server" Text="<%$ Resources:lbl_CaseUpdated %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        </div>
                    </div>
                </div>
                <div class="card-footer"> 
                    <asp:LinkButton ID="btnCancel" runat="server" Text="<%$ Resources:btn_Cancel %>" CausesValidation="false" OnClientClick="self.parent.tb_remove();" SkinID="btnSecondary"></asp:LinkButton>
                    <asp:LinkButton ID="btnSubmit" runat="server" Text="<%$ Resources:btn_Submit %>" SkinID="btnPrimary"></asp:LinkButton>
                   
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
