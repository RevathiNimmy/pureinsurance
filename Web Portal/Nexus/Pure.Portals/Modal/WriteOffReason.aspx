<%@ Page Language="VB" AutoEventWireup="false" CodeFile="WriteOffReason.aspx.vb" Inherits="Nexus.Modal_WriteOffReason"
    MasterPageFile="~/default.master" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script language="javascript" type="text/javascript">
    
              
    </script>

    <asp:ScriptManager runat="server"></asp:ScriptManager>
    <div id="Modal_WriteOffReason">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Label ID="lblWriteOffReasonHeading" runat="server" Text="<%$ Resources:lbl_WriteOffReasonHeading %>"></asp:Label>
                </h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <div class="form-group form-group-sm col-lg-12 col-md-12 col-sm-12">
                       <asp:Label runat="server" ID="lblWriteOffReason" Text="<%$ Resources:lbl_WriteOffReason %>" AssociatedControlID="ddlWriteOffReason" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:DropDownList ID="ddlWriteOffReason" AutoPostBack="true" runat="server" CssClass="field-medium form-control form-select"  ></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="cstmWriteOffReason" runat="server"  ControlToValidate="ddlWriteOffReason" Display="none" ErrorMessage="<%$ Resources:lbl_ErrorMsg_WriteOffReason %>" Enabled="true"  ></asp:RequiredFieldValidator>
                             <asp:HiddenField ID="hdnWriteOffReasonID" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnCancel" runat="server" Text="<%$ Resources:btnCancel %>" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnOk" runat="server" Text="<%$ Resources:btnOk %>" SkinID="btnPrimary" CausesValidation="true"  ></asp:LinkButton>                

            </div>
        </div>
        <asp:ValidationSummary ID="ValidationSummary" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
