<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PolicyCancel.aspx.vb" Inherits="Nexus.Modal_PolicyCancel"
    MasterPageFile="~/default.master" %>

<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script language="javascript" type="text/javascript">
        
    </script>

    <asp:ScriptManager runat="server"></asp:ScriptManager>
    <div id="Modal_PolicyCancel">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Label ID="lblPolicyCancel" runat="server" Text="<%$ Resources:lbl_PolicyCancel %>"></asp:Label>
                </h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label runat="server" ID="lblInfoMessage" Text="<%$ Resources:lbl_InfoMessage %>"></asp:Label>
                    </legend>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <div class="col-md-8 col-sm-9">
                            <asp:CheckBox ID="chkWriteOff" Text="<%$ Resources:lbl_WriteOff %>" TextAlign="Right" runat="server" CssClass="asp-check"></asp:CheckBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <div class="col-md-8 col-sm-9">
                            <asp:CheckBox runat="server" ID="chkSpoolPolicy" Text="<%$ Resources:lbl_SpoolPolicy %>" TextAlign="Right" CssClass="asp-check"></asp:CheckBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label runat="server" ID="lblCancelReason" Text="<%$ Resources:lbl_CancelReason %>" AssociatedControlID="ddlCancelReason" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <NexusProvider:LookupList ID="ddlCancelReason" runat="server" DataItemValue="Code" DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="LAPSED_REASON" DefaultText="<%$ Resources:lbl_None %>" CssClass="field-medium form-control form-select"></NexusProvider:LookupList>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvCancelReason" ValidationGroup="GroupCancelPolicy" runat="server" ControlToValidate="ddlCancelReason" Display="none" ErrorMessage="<%$ Resources:msgCancelReason %>" SetFocusOnError="True" InitialValue="" Enabled="true"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPolicyLapseDate" runat="server" AssociatedControlID="txtPolicyLapseDate" Text="<%$ Resources:lbl_PolicyLapseDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="txtPolicyLapseDate" runat="server" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="calPolicyLapseDate" runat="server" LinkedControl="txtPolicyLapseDate" HLevel="1"></uc1:CalendarLookup>
                            </div>
                        </div>

                        <asp:RequiredFieldValidator ID="rfvPolicyLapseDate" ValidationGroup="GroupCancelPolicy" runat="server" ControlToValidate="txtPolicyLapseDate" Display="none" ErrorMessage="<%$ Resources:msgPolicyLapseDate %>" SetFocusOnError="True" Enabled="true"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnCancel" runat="server" CausesValidation="false" Text="<%$ Resources:btnCancel %>" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnOk" runat="server" CausesValidation="true" ValidationGroup="GroupCancelPolicy" Text="<%$ Resources:btnOk %>" SkinID="btnPrimary"></asp:LinkButton>
                
            </div>
        </div>
        <asp:ValidationSummary ID="ValidationSummary" ValidationGroup="GroupCancelPolicy" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
