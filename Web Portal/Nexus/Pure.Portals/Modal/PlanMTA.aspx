<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PlanMTA.aspx.vb" Inherits="Nexus.Modal_PlanMTA"
    MasterPageFile="~/default.master" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script language="javascript" type="text/javascript">
        function CheckOneItem(obj) {
            document.getElementById('<%=chkAddTransaction.ClientID %>').checked = false;
            document.getElementById('<%=chkChangeFrequency.ClientID %>').checked = false;
            document.getElementById(obj).checked = true;
        }

    </script>

    <asp:ScriptManager runat="server"></asp:ScriptManager>
    <div id="Modal_PlanMTA">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Label ID="lblPlanMTA" runat="server" Text="<%$ Resources:lbl_PlanMTA %>"></asp:Label>
                </h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <div class="form-group form-group-sm col-lg-12 col-md-12 col-sm-12">
                        <asp:CheckBox runat="server" ID="chkChangeFrequency" Text="<%$ Resources:lbl_ChangeFrequency %>" TextAlign="Right" onclick="CheckOneItem(this.id);" CssClass="asp-check"></asp:CheckBox>
                    </div>
                    <div class="form-group form-group-sm col-lg-12 col-md-12 col-sm-12">
                        <asp:CheckBox runat="server" ID="chkAddTransaction" Text="<%$ Resources:lbl_AddTransaction %>" TextAlign="Right" onclick="CheckOneItem(this.id);" CssClass="asp-check"></asp:CheckBox>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnCancel" runat="server" Text="<%$ Resources:btnCancel %>" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnOk" runat="server" Text="<%$ Resources:btnOk %>" SkinID="btnPrimary"></asp:LinkButton>
                
            </div>
        </div>
        <asp:ValidationSummary ID="ValidationSummary" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
