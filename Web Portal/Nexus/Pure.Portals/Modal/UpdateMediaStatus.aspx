<%@ Page Language="VB" AutoEventWireup="false" CodeFile="UpdateMediaStatus.aspx.vb"
    Inherits="Nexus.Modal_UpdateMediaStatus" MasterPageFile="~/default.master" %>

<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <div id="Modal_UpdateMediaStatus">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblFindReceipts" runat="server" Text="Update Media Type Status" EnableViewState="false"></asp:Literal>
                </h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblMediaTypeStatus" runat="server" AssociatedControlID="GISMediaTypeStatus" Text="<%$ Resources:lbl_MediaTypeStatus %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <NexusProvider:LookupList ID="GISMediaTypeStatus" runat="server" DataItemValue="Code" DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="MEDIATYPE_STATUS" CssClass="field-medium field-mandatory form-control form-select" DefaultText="<%$ Resources:lbl_DefaultText%>"></NexusProvider:LookupList>
                        </div>
                        <asp:RequiredFieldValidator ID="rqdMediaTypeStatus" runat="server" ControlToValidate="GISMediaTypeStatus" ValidationGroup="MediaTypeStatusGroup" ErrorMessage="<%$ Resources:lbl_MediaTypeStatusError%>" Display="none" SetFocusOnError="True" Enabled="true"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblUpdateDate" runat="server" AssociatedControlID="txtUpdateDate" Text="<%$ Resources:lbl_UpdateDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="txtUpdateDate" CssClass="field-mandatory form-control" runat="server"></asp:TextBox><uc1:CalendarLookup ID="calUpdateDate" runat="server" LinkedControl="txtUpdateDate" HLevel="1"></uc1:CalendarLookup>
                            </div>
                        </div>

                        <asp:RequiredFieldValidator ID="RqdUpdateDate" runat="server" ControlToValidate="txtUpdateDate" ValidationGroup="MediaTypeStatusGroup" ErrorMessage="<%$ Resources:lbl_UpdateDateError%>" Display="none" SetFocusOnError="True" Enabled="true"></asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="rvUpdateDate" runat="Server" Type="Date" ValidationGroup="MediaTypeStatusGroup" Display="none" ControlToValidate="txtUpdateDate" MinimumValue="01/01/1900" ErrorMessage="<%$ Resources:lbl_DateFormat %>" SetFocusOnError="true"></asp:RangeValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="LblComments" runat="server" AssociatedControlID="txtComments" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="ltComments" runat="server" Text="<%$ Resources:lbl_Comments %>"></asp:Literal>
                        </asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtComments" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnCancel" runat="server" Text="<%$ Resources:lbl_btnCancel %>" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnOk" runat="server" Text="<%$ Resources:lbl_btnOk %>" ValidationGroup="MediaTypeStatusGroup" SkinID="btnPrimary"></asp:LinkButton>
                
            </div>
        </div>
        <asp:ValidationSummary ID="ValidationSummary" ShowSummary="true" HeaderText="<%$ Resources:lbl_ValidationSummary %>" DisplayMode="BulletList" ValidationGroup="MediaTypeStatusGroup" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
