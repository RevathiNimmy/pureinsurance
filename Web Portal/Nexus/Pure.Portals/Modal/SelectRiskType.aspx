<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SelectRiskType.aspx.vb"
    Inherits="Modal_SelectRiskType" EnableEventValidation="false" MasterPageFile="~/default.master" %>

<%@ Register Src="~/Controls/SelectRiskType.ascx" TagName="SelectRiskType" TagPrefix="uc1" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <div id="Modal_SelectRiskType">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblTitle" runat="server" Text="<%$ Resources:lbl_SelectRiskType_Title %>"></asp:Literal></h1>
            </div>
            <uc1:SelectRiskType ID="ucSelectRiskType" runat="server"></uc1:SelectRiskType>
        </div>
    </div>
</asp:Content>
