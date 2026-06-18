<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PolicyAssociates.aspx.vb" Inherits="Nexus.Modal_PolicyAssociates" MasterPageFile="~/default.master" %>

<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register Src="~/Controls/PolicyAssociates.ascx" TagName="PolicyAssociates"  TagPrefix="ucPA" %>

<%--The Purpose of the Page is to show the List of Associates in View Mode.--%> 

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    
    <div id="Modal_PolicyAssociates">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblTitle" runat="server" Text="<%$ Resources:lbl_PolicyAssociates %>"></asp:Literal></h1>
            </div>

            <div class="card-body clearfix">
                <ucPA:PolicyAssociates ID="ucPolicyAssociates" runat="server" IsViewOnly="true" />
            </div>

            
            <div class="card-footer">
                <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" CausesValidation="true" SkinID="btnSecondary"></asp:LinkButton>
            </div>
        </div>
    </div>
</asp:Content>