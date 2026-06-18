<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EventDetails.aspx.vb" Inherits="Nexus.Modal_EventDetails"
    MasterPageFile="~/default.master" EnableViewState="false" validateRequest="false" %>

<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<%@ Register Src="~/Controls/Document.ascx" TagName="Document" TagPrefix="uc1" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script language="javascript" type="text/javascript">
        function ValidateDetails(source, args) {
            if (args.Value.length >= 700) {
                args.IsValid = false;
            }
            else {
                args.IsValid = true;
            }
        }

    </script>

    <div id="Modal_EventDetails">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblTitle" runat="server" Text="<%$ Resources:lbl_EventDetails_Title %>"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblEventDetails" runat="server" Text="<%$ Resources:lbl_EventDetails %>"></asp:Label></legend>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblContext" runat="server" AssociatedControlID="lblContextData" Text="<%$ Resources:lblContext %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <p class="form-control-static font-bold">
                                <asp:Label ID="lblContextData" runat="server"></asp:Label>
                            </p>
                        </div>

                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblSubject" runat="server" AssociatedControlID="lblSubjectData" Text="<%$ Resources:lblSubject %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <p class="form-control-static font-bold">
                                <asp:Label ID="lblSubjectData" runat="server"></asp:Label>
                            </p>
                        </div>

                    </div>
                    <div class="form-group form-group-sm col-lg-12 col-md-12 col-sm-12">
                        <asp:Label ID="lblDetails" runat="server" AssociatedControlID="lblDetailsData" Text="<%$ Resources:lblDetails %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <p class="form-control-static font-bold">
                                <asp:Label ID="lblDetailsData" runat="server"></asp:Label>
                            </p>
                        </div>
                    </div>
                     <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                         <asp:Label ID="lblPriority" runat="server" AssociatedControlID="lblPriorityData" Text="<%$ Resources:lblPriority %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                         <div class="col-md-8 col-sm-9">
                            <p class="form-control-static font-bold">
                                <asp:Label ID="lblPriorityData" runat="server" ></asp:Label>
                            </p>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                         <asp:Label ID="lblStatus" runat="server" AssociatedControlID="lblStatusData" Text="<%$ Resources:lblStatus %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                         <div class="col-md-8 col-sm-9">
                            <p class="form-control-static font-bold">
                                <asp:Label ID="lblStatusData" runat="server"  ></asp:Label>
                            </p>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-12 col-md-12 col-sm-12">
                        <asp:Label ID="lblNewEventNotes" runat="server" AssociatedControlID="txtNewEventNotes" Text="<%$ Resources:lblNewEventNotes %>" class="col-md-2 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-10 col-sm-9">
                            <asp:TextBox ID="txtNewEventNotes" CssClass="field-mandatory form-control" runat="server" Rows="6" TextMode="MultiLine" onkeypress="return this.value.length<=700"></asp:TextBox>
                        </div>
                        <asp:RegularExpressionValidator Display = "None" ControlToValidate = "txtNewEventNotes" ID="RegularExpressionValidator3" ValidationExpression = "^[\s\S]{0,700}$" runat="server" ErrorMessage="<%$ Resources:lbl_invalidDetailsError %>" ></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator ID="reqNewEventNote" runat="server" ControlToValidate="txtNewEventNotes" Display="None" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_regError %>"></asp:RequiredFieldValidator>
                        <%--<asp:CustomValidator ID="cvDetails" runat="server" ControlToValidate="txtNewEventNotes" Display="None" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_invalidDetailsError %>" ClientValidationFunction="ValidateDetails"></asp:CustomValidator>--%>
                    </div>
                    <div class="form-group form-group-sm col-lg-12 col-md-12 col-sm-12">
                        <asp:Label ID="lblEventNotes" runat="server" AssociatedControlID="txtEventNotes" Text="<%$ Resources:lblEventNotes %>" class="col-md-2 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-10 col-sm-9">
                            <asp:TextBox ID="txtEventNotes" runat="server" CssClass="form-control" Rows="6" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-12 col-md-12 col-sm-12">
                        <asp:HyperLink ID="hypDoc" runat="server" Visible="false" SkinID="btnHLink" Text="<%$ Resources:Print_Document %>" Target="_blank"></asp:HyperLink>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnAddNote" runat="server" Text="<%$ Resources:btn_AddNote %>" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
        <asp:ValidationSummary ID="ValidationSummary1" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
