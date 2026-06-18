<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Payment.aspx.vb" Inherits="Nexus.Modal_Payment"
    MasterPageFile="~/default.master" EnableViewState="false" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script language="javascript" type="text/javascript">
        function CloseFindAccount() {
            tb_remove();
        }
    </script>
    <div id="Modal_Payment">
        <div class="card">
            <div id="Div1" class="card-body clearfix" runat="server">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label runat="server" ID="lblFindClient" Text="<%$ Resources:lbl_Heading_Payment %>"></asp:Label>
                    </legend>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblEnterPaymentAmount" runat="server" AssociatedControlID="txtEnterPaymentAmount" Text="<%$ Resources:lblEnterPaymentAmount %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtEnterPaymentAmount" TabIndex="1" CssClass="form-control" MaxLength="9" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-12 col-md-12 col-sm-12">
                        <asp:Label ID="rvPartPayRange" runat="server" Visible="false" CssClass="error" SetFocusOnError="true"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnCancel" SkinID="btnSecondary" runat="server" TabIndex="5" CausesValidation="false" Text="<%$ Resources:btnCancel %>"></asp:LinkButton>
                <asp:LinkButton ID="btnOk" SkinID="btnPrimary" runat="server" TabIndex="5" CausesValidation="True" Text="<%$ Resources:btnOk %>"></asp:LinkButton>

            </div>
        </div>
    </div>
</asp:Content>
