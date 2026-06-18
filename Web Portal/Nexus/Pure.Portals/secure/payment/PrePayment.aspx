<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PrePayment.aspx.vb" Inherits="Nexus.secure_payment_PrePayment"
    MasterPageFile="~/default.master" Title="Pre Payment" %>

<%@ Register Src="../../Controls/ProgressBar.ascx" TagName="ProgressBar" TagPrefix="uc3" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script type="text/javascript">

        var cntMainBody = "ctl00_cntMainBody_";

        function CheckUnAllocatedCredit(source, arguments) {
            var oControllitUnAllocatedCredit = document.getElementById('<%= litUnAllocatedCredit.ClientId%>');
            var oControllitTotalDue = document.getElementById('<%= txtTotalDue.ClientId%>');
            var UnAllocatedCredit = oControllitUnAllocatedCredit.value;
            if (parseFloat(UnAllocatedCredit) < 0) {
                UnAllocatedCredit = parseFloat(UnAllocatedCredit) * -1;
            }
            if (parseFloat(UnAllocatedCredit) < parseFloat(oControllitTotalDue.value)) {
                arguments.IsValid = false;
            }
            else {
                arguments.IsValid = true;
            }
        }

        function CheckDebitAgainst(source, arguments)// To Validate Whether atleast one DebitAgainst RadioButton is Selected or not.
        {
            var rdoDebitAgainstOptionsCount = document.getElementById(cntMainBody + 'radioDebitAgainst').getElementsByTagName('input').length;

            for (var CountVar = 0; CountVar < rdoDebitAgainstOptionsCount; CountVar++) {
                if (document.getElementById(cntMainBody + 'radioDebitAgainst_' + CountVar).checked) {
                    arguments.IsValid = true;
                    return;
                }
            }
            ValidatorEnable(document.getElementById('<%= cvUnAllocatedCreditchk.ClientId%>'), false); //Make insufficient balance error message false at this moment 
            arguments.IsValid = false;
        }

        function pageLoad() {
            var manager = Sys.WebForms.PageRequestManager.getInstance();
            manager.add_beginRequest(OnBeginRequest);
            manager.add_endRequest(OnEndRequest);
        }

        function OnBeginRequest(sender, args) {
            //disable the button (or whatever else we need to do) here
            document.getElementById('<%= btnSubmit.ClientId%>').disabled = true;
        }

        function OnEndRequest(sender, args) {
            //enable the button (or whatever else we need to do) here
            document.getElementById('<%= btnSubmit.ClientId%>').disabled = false;
        }

        function ConfirmValidation() {
            Page_ClientValidate();

            if (Page_IsValid == true) {
                return true;
            }
            else {
                return false;
            }
        }
    </script>

    <div id="secure_payment_PrePayment">
        <uc3:ProgressBar ID="ucProgressBar" runat="server"></uc3:ProgressBar>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblPrePaymentHeading" runat="server" Text="<%$ Resources:lbl_Prepayment_heading %>"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPolicyRefheading" runat="server" CssClass="col-md-4 col-sm-3 control-label" Text="<%$ Resources:lbl_PolicyRef_heading %>"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <p class="form-control-static font-bold">
                                <asp:Literal runat="server" ID="litPolicyRefheading"></asp:Literal>
                            </p>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblTotalDueheading" runat="server" CssClass="col-md-4 col-sm-3 control-label" Text="<%$ Resources:lbl_TotalDue_heading %>"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:Literal ID="litTotalDueheading" runat="server" Visible="false"></asp:Literal>
                            <%-- <asp:Literal ID="litTotalDue" runat="server"  />--%>
                            <asp:TextBox ID="txtTotalDue" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <asp:UpdatePanel ID="UpdatePrePayment" runat="server" UpdateMode="Always" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <div id="pnlAccountetails" runat="server">
                            <div class="form-horizontal">
                                <legend>
                                    <asp:Literal ID="LblAccountHeading" runat="server" Text="<%$ Resources:lbl_Account_heading %>"></asp:Literal></legend>
                                <asp:RadioButtonList ID="radioUserType" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" AutoPostBack="true" CssClass="asp-radio">
                                    <asp:ListItem Text="<%$ Resources:lbl_radioType_Option1 %>" Enabled="false"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:lbl_radioType_Option2 %>" Enabled="false"></asp:ListItem>
                                </asp:RadioButtonList>

                            </div>
                            <div class="form-horizontal">
                                <legend>
                                    <asp:Literal ID="litDebitAgainst" runat="server" Text="<%$ Resources:lbl_DebitAgainst_heading %>"></asp:Literal></legend>
                                <asp:RadioButtonList ID="radioDebitAgainst" runat="server" RepeatLayout="Flow" RepeatDirection="Vertical" AutoPostBack="true" CssClass="asp-radio">
                                </asp:RadioButtonList>
                                <asp:CustomValidator ID="CustVldDebitAgainst" runat="server" ErrorMessage="<%$Resources:lbl_Error_DebitAgainstChk %>" Display="None" ClientValidationFunction="CheckDebitAgainst" SetFocusOnError="true"></asp:CustomValidator>
                                <input id="litUnAllocatedCredit" type="hidden" runat="server" value="0.00">
                                <asp:CustomValidator ID="cvUnAllocatedCreditchk" runat="server" ErrorMessage="<%$Resources:lbl_Error_UnAllocatedCredit %>" Display="None" ClientValidationFunction="CheckUnAllocatedCredit" SetFocusOnError="True">*</asp:CustomValidator>
                                <div class="grid-card table-responsive">
                                    <asp:GridView ID="grdvPrePayment" runat="server" AutoGenerateColumns="False" GridLines="None" Enabled="False" AllowPaging="True" PagerSettings-Mode="Numeric" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ChkBoxUnallocatedAmount" runat="server" AutoPostBack="true" Checked='<%# Bind("IsSelected") %>' OnCheckedChanged="ChkBoxUnallocatedAmount_Selected" Text=" " CssClass="asp-check"></asp:CheckBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="DocumentRef" HeaderText="<%$ Resources:lbl_Grid_DocumentRef %>"></asp:BoundField>
                                            <asp:BoundField DataField="MediaType" HeaderText="<%$ Resources:lbl_Grid_MediaType %>"></asp:BoundField>
                                            <asp:BoundField DataField="Reference" HeaderText="<%$ Resources:lbl_Grid_Reference %>"></asp:BoundField>
                                            <Nexus:BoundField DataField="Amount" HeaderText="<%$ Resources:lbl_Grid_Amount %>" DataType="Currency">
                                            </Nexus:BoundField>
                                            <asp:BoundField DataField="CollectionDate" DataFormatString="{0:d}" HeaderText="<%$ Resources:lbl_Grid_CollectionDate %>" HtmlEncode="False"></asp:BoundField>
                                            <asp:BoundField DataField="AccountKey" HeaderText="AccKey"></asp:BoundField>
                                            <asp:BoundField DataField="TransDetailKey" HeaderText="TransDetailKey"></asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:CustomValidator ID="cvCollectionDatechk" runat="server" ErrorMessage="<%$ Resources:lbl_Error_CollectionDate %>" Display="None" Enabled="false"></asp:CustomValidator>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="radioUserType" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="radioDebitAgainst" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="grdvPrePayment" EventName="RowCommand"></asp:AsyncPostBackTrigger>
                    </Triggers>
                </asp:UpdatePanel>
                <Nexus:ProgressIndicator ID="UpPrePayment" OverlayCssClass="updating" AssociatedUpdatePanelID="UpdatePrePayment" runat="server">
                    <progresstemplate>
                    </progresstemplate>
                </Nexus:ProgressIndicator>
                <!--end of bank details-->

            </div>
            <div class="card-footer">
                <asp:Literal ID="lblReview" runat="server" Text="<%$ Resources:lbl_Review %>"></asp:Literal>
                <asp:LinkButton ID="btnCancel" runat="server" CausesValidation="false" Text="<%$ Resources:btn_Cancel %>" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnSubmit" runat="server" Text="<%$ Resources:AppResources.strings, btn_Submit %>" CausesValidation="true" OnClientClick="return ConfirmValidation();" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
        <asp:ValidationSummary ID="ValidationSummary" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
