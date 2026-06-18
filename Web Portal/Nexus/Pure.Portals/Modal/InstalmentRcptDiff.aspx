<%@ Page Language="VB" AutoEventWireup="false" CodeFile="InstalmentRcptDiff.aspx.vb" Inherits="Nexus.Modal_InstalmentRcptDiff"
    MasterPageFile="~/default.master" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script language="javascript" type="text/javascript">
    
        function ReadWriteOffReason(WriteOffReasonID) {
            //document.getElementById("<%= hdnWriteOffReasonID.ClientID%>").value = WriteOffReasonID;
            //tb_remove();
            self.parent.ReadWriteOffReason(WriteOffReasonID);
           
        }
    </script>

    <asp:ScriptManager runat="server"></asp:ScriptManager>
    <div id="Modal_InstalmentRcptDiff">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Label ID="lblInstalmentRcptDiff" runat="server" Text="<%$ Resources:lbl_InstalmentRcptDiff %>"></asp:Label>
                </h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <div class="form-group form-group-sm col-lg-12 col-md-12 col-sm-12">
                        <asp:Label ID="lblMessage" runat="server" Text="<%$ Resources:lbl_DifferenceMessage %>"></asp:Label>
                        <asp:HiddenField ID="hdnWriteOffReasonID" runat="server" />
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnCancel" runat="server" Text="<%$ Resources:btnCancel %>" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnWriteOffDiff" runat="server" Text="<%$ Resources:btnWriteOffDiff %>" SkinID="btnPrimary"></asp:LinkButton>
                 <asp:LinkButton ID="btnTakeExactAmt" runat="server" Text="<%$ Resources:btnTakeExactAmt %>" SkinID="btnPrimary"></asp:LinkButton>                
                
            </div>
        </div>
        <asp:ValidationSummary ID="ValidationSummary" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
