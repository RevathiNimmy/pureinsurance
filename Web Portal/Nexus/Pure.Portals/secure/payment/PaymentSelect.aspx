<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PaymentSelect.aspx.vb" Inherits="Nexus.secure_payment_PaymentSelect"
    MasterPageFile="~/default.master" Title="Untitled Page" %>

<%@ Register Src="../../Controls/ProgressBar.ascx" TagName="ProgressBar" TagPrefix="uc3" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script language="javascript" type="text/javascript">
        function SelectPayment(source, arguments) {
            var rdoPaymentOptionsCount = document.getElementById('<%= rdoPaymentOptionsCount.ClientId%>').value;
            for (var CountVar = 0; CountVar < rdoPaymentOptionsCount; CountVar++) {
                if (document.getElementById("PayOption" + CountVar).checked) {
                    arguments.IsValid = true;
                    return;
                }
            }
            arguments.IsValid = false;
        }
        function selectaccunt(PaymentType) {

            if ((PaymentType.indexOf("AgentCollection") != -1) || (PaymentType.indexOf("Direct Debit") != -1)) {
                if ((document.getElementById("ctl00_cntMainBody_hfPrePayment").value == "0") && (document.getElementById("ctl00_cntMainBody_hfAgentType").value == "INTERMEDIARY") && (document.getElementById("ctl00_cntMainBody_hfTransType").value == "NB")) {
                    $(document).ready(function () {
                        $("#ctl00_cntMainBody_btnBuy").bind("click", function () {
                            tb_show(null, '../../modal/SelectAccount.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=400', null); return false;
                        });
                    });
                }
            }
        }
        function GetAccountType(s1, s2) {
            if (s1 == true) {
                document.getElementById('<%=hdselectaccount.ClientID %>').value = "Client"
            }
            if (s2 == true) {
                document.getElementById('<%=hdselectaccount.ClientID %>').value = "Agent"
            }
            __doPostBack('', 'GetAccount')
        }
    </script>

    <div id="secure_payment_PaymentSelect">

        <uc3:ProgressBar ID="ucProgressBar" runat="server"></uc3:ProgressBar>
        <div class="card">
            <div class="card-body clearfix">
                <legend>
                    <asp:Label ID="lblTransactionHeading" runat="server" Text="<%$ Resources:lbl_Payment_heading %>"></asp:Label>
                </legend>
                <div class="grid-card table-responsive no-margin">
                    <asp:HiddenField ID="hfPrePayment" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hfAgentType" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hfTransType" runat="server"></asp:HiddenField>
                    <asp:GridView ID="GridPaymentOptions" runat="server" AutoGenerateColumns="False" Caption="<%$ Resources:lbl_PaymentTable_Caption %>">
                        <Columns>
                            <asp:TemplateField HeaderText="<%$ Resources:lbl_PaymentTable_Col1Heading %>">
                                <ItemTemplate>
                                    <asp:Literal ID="literal" runat="server" Text='<%#Eval("Column1")%>'></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:lbl_PaymentTable_Col2Heading %>">
                                <ItemTemplate>
                                    <input type="radio" id='<%#Eval("Column3")%>' name="rdoPaymentOptions" value='<%#Eval("Column2")%>' onclick="selectaccunt(this.value)">
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <asp:HiddenField ID="hdselectaccount" runat="server"></asp:HiddenField>
                <asp:TextBox ID="rdoPaymentOptionsCount" Style="visibility: hidden;" runat="server"></asp:TextBox>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnBuy" runat="server" Visible="true" Text="<%$ Resources:AppResources.strings, btn_Continue %>" SkinID="btnPrimary"></asp:LinkButton>

            </div>
        </div>
        <asp:CustomValidator ID="CustVldPaymentOptionRequired" runat="server" ErrorMessage="<%$ Resources:ddlPayment_Option_PleaseSelect %>" ClientValidationFunction="SelectPayment" SetFocusOnError="true" Display="None"></asp:CustomValidator>
        <asp:ValidationSummary ID="ValidationSummary" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>

    </div>
</asp:Content>
