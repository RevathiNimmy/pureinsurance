<%@ Page Language="VB" MasterPageFile="~/Default.master" AutoEventWireup="false"
    CodeFile="QQPremium.aspx.vb" Inherits="Nexus.QQPremium" Title="Untitled Page" %>

<%@ Register Src="~/Controls/ProgressBar.ascx" TagName="ProgressBar" TagPrefix="uc3" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="QQPremium">
        <uc3:ProgressBar ID="ucProgressBar" runat="server"></uc3:ProgressBar>


        <div class="card">
            <div class="card-body clearfix">

                <div class="card-heading">
                    <h1>
                        <asp:Label ID="lblHeading" runat="server" Text="<%$ Resources:lbl_Summary_heading%>"></asp:Label><asp:Label ID="lblPolicyRef" runat="server"></asp:Label>
                    </h1>
                </div>
                <hr>
                <div id="form-wrapper">
                    <asp:Panel ID="pnlSummary" runat="server">
                        <p>
                            <h2>
                                <asp:Label ID="lblPremiumIndicationText" runat="server" Text="<%$ Resources:lbl_Premium_Indication%>"></asp:Label>&nbsp;
                                            <asp:Label ID="lblPremiumIndication" runat="server"></asp:Label></h2>
                        </p>
                        <asp:Literal ID="ltSummary" runat="server" Text="<%$ Resources:lt_Summary_Customer  %>"></asp:Literal>
                    </asp:Panel>
                    <asp:Label ID="lblSaveSuccess" runat="server" Text="<%$ Resources:lbl_Save_Success %>" Visible="false"></asp:Label>
                    <br>
                </div>
            </div>

        </div>


        <div class="card-footer">
            <asp:LinkButton ID="btnSaveQuote" runat="server" Text="<%$ Resources:btn_Save_Quote %>" SkinID="btnPrimary"></asp:LinkButton>
            <asp:LinkButton ID="btnBuyNow" runat="server" Text="<%$ Resources:btn_Buy_Now %>" SkinID="btnPrimary"></asp:LinkButton>
        </div>

    </div>
</asp:Content>
