<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddEvent.aspx.vb" Inherits="Nexus.Modal_AddEvent"
    MasterPageFile="~/default.master" %>

<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ScriptIncludes" runat="Server"></asp:Content>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <script language="javascript" type="text/javascript">
        function ValidateDetails(source, args) {
            if (args.Value.length >= 7500) {
                args.IsValid = false;
            }
            else {
                args.IsValid = true;
            }
        }

        function ShowFields() {
            var oDDL;
            oDDL = document.getElementById('<%=ddlContext.ClientID %>');
            if (oDDL.options[oDDL.selectedIndex].value == '37') {
                //Show Priority and Status
                document.getElementById('<%=Priority.ClientID %>').style.visibility = "visible";
                document.getElementById('<%=Status.ClientID %>').style.visibility = "visible";
            }
            else {
                //Hide Priority and Status
                document.getElementById('<%=Priority.ClientID %>').style.visibility = "hidden";
                document.getElementById('<%=Status.ClientID %>').style.visibility = "hidden";
            }
        }
        $(document).ready(function () {
            ShowFields();
        });

    </script>

    <div id="Modal_AddEvent">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Label ID="lblPageHeader" runat="server" Text="<%$ Resources:lbl_AddEvent %>"></asp:Label></h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblAddEvent" runat="server" Text="<%$ Resources:lbl_AddEvent %>"></asp:Label></legend>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblContext" runat="server" AssociatedControlID="ddlContext" Text="<%$ Resources:lblContext %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:DropDownList ID="ddlContext" runat="server" CssClass="field-medium form-control form-select" onchange="ShowFields()"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblSubject" runat="server" AssociatedControlID="ddlSubject" Text="<%$ Resources:lblSubject %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <NexusProvider:LookupList ID="ddlSubject" runat="server" ListCode="Event_Log_Subject" ListType="PMLookup" Sort="ASC" DataItemText="Description" DataItemValue="Key" CssClass="field-medium form-control form-select"></NexusProvider:LookupList>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblDetails" runat="server" AssociatedControlID="txtDetails" Text="<%$ Resources:lblDetails %>" class="col-md-4 col-sm-3 control-label"/>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtDetails" CssClass="field-medium form-control field-mandatory" runat="server"
                                TextMode="MultiLine" />
                        </div>
                        <asp:RequiredFieldValidator ID="reqDetails" runat="server" ControlToValidate="txtDetails"
                            Display="None" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_regError %>" />
                        <asp:CustomValidator ID="cvDetails" runat="server" ControlToValidate="txtDetails"
                            Display="None" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_invalidDetailsError %>"
                            ClientValidationFunction="ValidateDetails" />
                    </div>
                    <div id="Priority" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPriority" runat="server" AssociatedControlID="ddlPriority" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="liPriority" runat="server" Text="<%$ Resources:lblPriority %>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                <asp:DropDownList ID="ddlPriority" runat="server" CssClass="field-medium form-control form-select">
                                    <asp:ListItem Selected="true" Text="Red" Value="Red"></asp:ListItem>
                                    <asp:ListItem Text="Amber" Value="Amber"></asp:ListItem>
                                    <asp:ListItem Text="Green" Value="Green"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                    </div>
                    <div id="Status" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblStatus" runat="server" AssociatedControlID="ddlStatus" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="liStatus" runat="server" Text="<%$ Resources:lblStatus %>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="field-medium form-control form-select">
                                    <asp:ListItem Selected="true" Text="Outstanding" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Completed" Value="1"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                    </div>
                    
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnCancel" runat="server" Text="<%$ Resources:btnCancel %> " CausesValidation="false" OnClientClick="self.parent.tb_remove(); return false;" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnOk" runat="server" Text="<%$ Resources:btn_Ok %>" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
        <asp:ValidationSummary ID="ValidationSummary1" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
