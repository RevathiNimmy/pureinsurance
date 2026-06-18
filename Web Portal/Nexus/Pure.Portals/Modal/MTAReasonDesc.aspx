<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MTAReasonDesc.aspx.vb" Inherits="Nexus.Modal_MTAReasonDesc"
    MasterPageFile="~/default.master" %>

<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <script type="text/javascript">
        function submit1() {

            self.parent.tb_remove();
            self.parent.ReceiveDescription(document.getElementById('<%=txtDescription.ClientID %>').value);
        }
        function close1() {
            self.parent.tb_remove();
        }
    </script>

    <div id="Modal_MTAReasonDesc">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblTitle" runat="server" Text="<%$ Resources:lbl_MTAReasonOther_Title %>"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblHeading" runat="server" Text="<%$ Resources:lbl_Heading %>"></asp:Label>
                    </legend>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblDescription" runat="server" AssociatedControlID="txtDescription" Text="<%$ Resources:lbl_Description %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtDescription" runat="server" class="fieldset-wrapper" CssClass="form-control">
                            </asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card-footer">

                <asp:LinkButton ID="btnCancel" runat="server" Text="<%$ Resources:btnCancel %>" CausesValidation="true" SkinID="btnSecondary" OnClientClick="close1()"></asp:LinkButton>
                <asp:LinkButton ID="btnOK" runat="server" Text="<%$ Resources:btnOK %>" CausesValidation="true" SkinID="btnPrimary" OnClientClick="submit1()"></asp:LinkButton>
            </div>
        </div>
    </div>
</asp:Content>
