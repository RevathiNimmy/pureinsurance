<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="ReInsurance.aspx.vb" Inherits="Nexus.ReInsurance" Title="Untitled Page" %>

<%@ Register Src="../Controls/ProgressBar.ascx" TagName="ProgressBar" TagPrefix="uc1" %>
<%@ Register Src="../Controls/Reinsurance.ascx" TagName="ReInsurance" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cntMainBody" runat="Server">
    <div id="secure_ReInsurance">
        <uc1:ProgressBar ID="ucProgressBar" runat="server"></uc1:ProgressBar>
        <div class="card">
            <div class="card-body clearfix">
                <div class="card-heading">
                    <h1>
                        <asp:Literal ID="lblPageheader" runat="server" Text="<%$ Resources:lbl_Page_header %>"></asp:Literal></h1>
                </div>
                <uc2:ReInsurance ID="ReInsuranceCntrl" runat="server"></uc2:ReInsurance>
            </div>
            <div class="card-footer">
                <asp:Literal ID="lblReview" runat="server" Text="<%$ Resources:lbl_Review %>"></asp:Literal>
                <asp:LinkButton ID="btnSubmit" runat="server" Text="<%$ Resources:btn_Submit%>" TabIndex="3" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
    </div>
</asp:Content>
