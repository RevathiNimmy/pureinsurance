<%@ Page Language="VB" AutoEventWireup="false" CodeFile="WmTaskLogEntry.aspx.vb"
    Inherits="Nexus.Modal_WmTaskLogEntry" Title="TaskLogEntry" MasterPageFile="~/default.master" %>

<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <div id="Modal_WmTaskLogEntry">
        <div class="card">
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="Literal1" runat="server" Text="<%$Resources:lbl_TaskLog%>"></asp:Label></legend>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblCreatedby" runat="server" AssociatedControlID="txtCreatedBy" Text="<%$ Resources:lbl_CreatedBy %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtCreatedBy" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblDate" runat="server" AssociatedControlID="txtDate" Text="<%$ Resources:lbl_Date %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtDate" runat="server" ReadOnly="true " CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblEntry" runat="server" AssociatedControlID="txtEntry" Text="<%$ Resources:lbl_Entry %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtEntry" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="vldEntryRequired" runat="server" Display="none" ControlToValidate="txtEntry" ErrorMessage=" <%$ Resources:lbl_vldEntryRequired %> " SetFocusOnError="True" ValidationGroup="AddTaskLOg"></asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="rngvDueDate" ControlToValidate="txtDueDate" CssClass="" Type="Date" MinimumValue="" MaximumValue=""></asp:RangeValidator>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnCancel" runat="server" Text="<%$ Resources:btnCancel %> " SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnOK" runat="server" Text="<%$ Resources:btnOK %> " ValidationGroup="AddTaskLOg" SkinID="btnPrimary"></asp:LinkButton>

            </div>
        </div>
        <asp:ValidationSummary ID="vldAddTaskButton" DisplayMode="BulletList" runat="server" ValidationGroup="AddTaskLOg" ShowSummary="true " CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
