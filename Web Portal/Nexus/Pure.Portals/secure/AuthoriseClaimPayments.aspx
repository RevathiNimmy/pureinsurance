<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="AuthoriseClaimPayments.aspx.vb" Inherits="Nexus.AuthoriseClaimPayments"
    EnableViewState="true" %>

<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <asp:ScriptManager ID="scriptAP" runat="server">
    </asp:ScriptManager>

    <script language="javascript" type="text/javascript">

        function FindClaimNumber(sClaimNumber) {
            tb_remove();
            document.getElementById('<%= txtClaimReference.ClientId%>').value = sClaimNumber;
        }
        function setClient(sClientName, sClientKey) {
            tb_remove();
            document.getElementById('<%= txtClientKey.ClientId%>').value = sClientKey;
            document.getElementById('<%= txtClient.ClientId%>').value = sClientName;
            document.getElementById('<%= txtClient.ClientId%>').focus();
        }
        function setClaimReference(sClaimNumber) {
            tb_remove();
            document.getElementById('<%= txtClaimReference.ClientId%>').value = sClaimNumber;
            document.getElementById('<%= txtClaimReference.ClientId%>').focus();
        }
        function setPolicy(sPolicy) {
            tb_remove();
            document.getElementById('<%= txtPolicy.ClientId%>').value = sPolicy;
            document.getElementById('<%= txtPolicy.ClientId%>').focus();
        }
        function setUser(sID, sUserName) {
            tb_remove();
            document.getElementById('<%= txtCreatedBy.ClientId%>').value = sID;
            document.getElementById('<%= txtCreatedBy.ClientId%>').focus();

        }
        function checkAuthorisationAmount(oControl, authorizeMsg, declineMsg, reason, WarningMsg1, WarningMsg2, invalidusermsg) {

            var dPayAmount = document.getElementById('<%= hiddenPaymentAmount.ClientId%>').value;
            var dLimit = document.getElementById('<%= hiddenLimitAmount.ClientId%>').value;

            var bResponse;
            var dPayAmount = document.getElementById('<%= hiddenPaymentBaseCurrencyAmount.ClientId%>').value;
            var dLimit = document.getElementById('<%= hiddenLimitBaseCurrencyAmount.ClientId%>').value;

            if (oControl == 1) //Authorise
            {
                if ((Number(dPayAmount) > Number(dLimit)) && (oControl == 1)) {
                    window.alert(WarningMsg1 + " " + dLimit + WarningMsg2 + dPayAmount);
                    return false;
                }
                else {
                    bResponse = window.confirm(authorizeMsg);
                }
            }
            else   //decline
            {
                bResponse = window.confirm(declineMsg);

            }
            if (bResponse) {
                if (oControl == 1) {
                    //Authorise
                    var reason = prompt(reason, '');
                    if (reason == null) {
                        reason = '';
                    }
                    var oControl = document.getElementById('<%= hiddenDeclineReason.ClientId%>');
                    oControl.value = reason;
                }
                else {
                    if (oControl == 2) {
                        //decline
                        if (invalidusermsg == 'null') {
                            //if multistep approval is OFF then same user can decline it's own claim payments
                            var reason = prompt(reason, '');
                            var oControl = document.getElementById('<%= hiddenDeclineReason.ClientId%>');
                            oControl.value = reason;
                        }
                        else {
                            //if multistep approval is ON then same user can decline others claim payments
                            var reason = prompt(reason, '');
                            var oControl = document.getElementById('<%= hiddenDeclineReason.ClientId%>');
                            oControl.value = reason;
                        }
                    }

                    return true;
                }
            }
            else {
                return false;
            }
        }
        function updatePayamount(chk, status) {
            var amountValue = chk.value.split(':')[0];
            var amountBaseCurrencyValue = chk.value.split(':')[3];
            document.getElementById('<%= hiddenCurrencyRate.ClientID%>').value = chk.value.split(':')[3];

            var oPaymentAmount = document.getElementById('<%= hiddenPaymentAmount.ClientId%>');
            var oPaymentBaseCurrencyAmount = document.getElementById('<%= hiddenPaymentBaseCurrencyAmount.ClientId%>');
            var olblControlValue = Number(oPaymentAmount.value);
            var olblControlBaseCurrecyValue = Number(oPaymentAmount.value);

            if (status == true) {

                oPaymentAmount.value = Number(amountValue);
                oPaymentBaseCurrencyAmount.value = Number(amountBaseCurrencyValue);

            }
            else {

                oPaymentAmount.value = (Number(olblControlValue) - Number(amountValue));
                oPaymentBaseCurrencyAmount.value = (Number(olblControlBaseCurrecyValue) - Number(amountBaseCurrencyValue));
            }
        }
        function CloseFidUser() {
            tb_remove();
        }
        function DeclinePayment(sMessage) {
            alert(sMessage);
        }
        //Sets Casekey and Case Number from popup window "FindCase".
        function setCaseReference(sCaseKey, sCaseNumber) {
            tb_remove();
            $('#<%= hfCaseKey.ClientId%>').val(sCaseKey);
            $('#<%= txtCaseReference.ClientId%>').val(sCaseNumber);
        }
        //It Reset Case key value. When ther is a wild card search is done casekey need to be reset.
        function ResetCaseKey() {
            $('#<%= hfCaseKey.ClientId%>').val('');
        }
        function ProcessAuthorisePayment(Description)
        {
            
        document.getElementById('<%=hdnDuplicateClaimPaymentReason.ClientID %>').value = Description
        __doPostBack('', 'AuthorisePayment');
        
    }
    </script>
    <asp:HiddenField ID="hdnfIsAuthorize" runat="server"></asp:HiddenField>
    <div id="secure_AuthoriseClaimPayments">
        <asp:UpdatePanel ID="upAuthorizePayment" runat="server" UpdateMode="Always" ChildrenAsTriggers="true">
            <ContentTemplate>
                <asp:Panel ID="PnlAuthCP" runat="server" DefaultButton="btnFindNow">
                    <div class="card">
                        <div class="card-heading">
                            <h1>
                                <asp:Literal ID="litClaimPaymentsHeader" runat="server" Text="<%$ Resources:litClaimPaymentsHeader%>" EnableViewState="false"></asp:Literal>
                            </h1>
                        </div>
                        <div id="divHidden" runat="server" visible="false">
                            <div class="card-body clearfix">
                                <div class="form-horizontal">
                                    <legend><span>
                                        <asp:Literal ID="litHidden" runat="server" Text="<%$ Resources:lbl_AuthoriseClaimPayments %>"></asp:Literal></span>
                                    </legend>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Literal ID="litHiddenText" runat="server" Text="<%$ Resources:lbl_ErrorMessage %>"></asp:Literal>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="divHeader" runat="server" class="card-body clearfix">
                            <div class="form-horizontal">
                                <legend>
                                    <asp:Literal ID="lblBankGuaranteeDetails" runat="server" Text="<%$ Resources:lbl_AuthoriseClaimPayments %>"></asp:Literal>
                                </legend>

                                <asp:HiddenField ID="sIsAuthoriser" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="sIsRecommender" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hiddenPaymentAmount" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hiddenLimitAmount" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hiddenDeclineReason" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hMultiStepApproval" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hiddenPaymentBaseCurrencyAmount" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hiddenLimitBaseCurrencyAmount" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hiddenAuthorizedCurrencyId" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hiddenCurrencyRate" runat="server"></asp:HiddenField>
                                <div id="liCaseNumber" runat="server" visible="false" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label runat="Server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtCaseReference" Text="<%$ Resources:lbl_CaseReference %>" ID="lblbtnCaseNumber"></asp:Label><div class="col-md-8 col-sm-9">
                                        <div class="input-group">
                                            <asp:TextBox ID="txtCaseReference" runat="server" CssClass="form-control" onblur="ResetCaseKey();"></asp:TextBox><span class="input-group-btn">
                                                <asp:LinkButton ID="btnCaseNumber" runat="server" CausesValidation="false" SkinID="btnModal">
                                                    <i class="fa fa-search"></i>
                                                     <span class="btn-fnd-txt">Case Number</span>
                                                </asp:LinkButton></span>
                                        </div>
                                    </div>
                                    <asp:HiddenField ID="hfCaseKey" runat="server"></asp:HiddenField>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblPayeeName" runat="server" AssociatedControlID="txtPayeeName" Text="<%$ Resources:lbl_PayeeName %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:TextBox ID="txtPayeeName" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label runat="Server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtClaimReference" Text="<%$ Resources:btn_ClaimReference %>" ID="lblbtnClaimReference"></asp:Label><div class="col-md-8 col-sm-9">
                                        <div class="input-group">
                                            <asp:TextBox ID="txtClaimReference" runat="server" CssClass="form-control"></asp:TextBox><span class="input-group-btn">
                                                <asp:LinkButton ID="btnClaimReference" runat="server" CausesValidation="false" SkinID="btnModal">
                                                    <i class="fa fa-search"></i>
                                                    <span class="btn-fnd-txt">Claim Reference</span>
                                                </asp:LinkButton></span>
                                        </div>
                                    </div>
                                    <asp:HiddenField ID="txtClaimReferenceKey" runat="server"></asp:HiddenField>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblPaymentDate" runat="server" AssociatedControlID="txtPaymentDate" class="col-md-4 col-sm-3 control-label">
                                        <asp:Literal ID="litPaymentDate" runat="server" Text="<%$ Resources:lit_PaymentDate %>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                            <div class="input-group">
                                                <asp:TextBox ID="txtPaymentDate" runat="server" CssClass="field-date form-control"></asp:TextBox><uc1:CalendarLookup ID="calPaymentDate" runat="server" LinkedControl="txtPaymentDate" HLevel="5"></uc1:CalendarLookup>
                                            </div>
                                        </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label runat="Server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtPolicy" Text="<%$ Resources:btn_Policy %>" ID="lblbtnPolicy"></asp:Label><div class="col-md-8 col-sm-9">
                                        <div class="input-group">
                                            <asp:TextBox ID="txtPolicy" runat="server" CssClass="form-control"></asp:TextBox><span class="input-group-btn">
                                                <asp:LinkButton ID="btnPolicy" runat="server" CausesValidation="false" SkinID="btnModal">
                                                    <i class="fa fa-search"></i>
                                                     <span class="btn-fnd-txt">Policy</span>
                                                </asp:LinkButton></span>
                                        </div>
                                    </div>
                                    <asp:HiddenField ID="txtPolicyKey" runat="server"></asp:HiddenField>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label runat="Server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtCreatedBy" Text="<%$ Resources:btn_CreatedBy %>" ID="lblbtnCreatedBy"></asp:Label><div class="col-md-8 col-sm-9">
                                        <div class="input-group">
                                            <asp:TextBox ID="txtCreatedBy" runat="server" CssClass="form-control"></asp:TextBox><span class="input-group-btn">
                                                <asp:LinkButton ID="btnCreatedBy" runat="server" CausesValidation="false" SkinID="btnModal">
                                                    <i class="fa fa-search"></i>
                                                    <span class="btn-fnd-txt">Created By</span>
                                                </asp:LinkButton></span>
                                        </div>
                                    </div>
                                    <asp:HiddenField ID="txtCreatedByKey" runat="server"></asp:HiddenField>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label runat="Server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtClient" Text="<%$ Resources:btn_Client %>" ID="lblbtnClient"></asp:Label><div class="col-md-8 col-sm-9">
                                        <div class="input-group">
                                            <asp:TextBox ID="txtClient" runat="server" CssClass="form-control"></asp:TextBox><span class="input-group-btn">
                                                <asp:LinkButton ID="btnClient" runat="server" SkinID="btnModal">
                                                    <i class="fa fa-search"></i>
                                                     <span class="btn-fnd-txt">Client</span>
                                                </asp:LinkButton>

                                            </span>
                                        </div>
                                    </div>
                                    <asp:HiddenField ID="txtClientKey" runat="server"></asp:HiddenField>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblBranch" runat="server" AssociatedControlID="ddlBranch" class="col-md-4 col-sm-3 control-label">
                                        <asp:Literal ID="litBGCustodyBranch" runat="server" Text="<%$ Resources:lbl_Branch %>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                            <asp:DropDownList ID="ddlBranch" runat="server" CssClass="field-medium form-control form-select"></asp:DropDownList>
                                        </div>
                                </div>
                            </div>
                        </div>
                        <div id="divSubmit" runat="server" class="card-footer">
                            <asp:LinkButton ID="btnClear" runat="server" Text="<%$ Resources:btn_Clear %>" CausesValidation="false" SkinID="btnSecondary"></asp:LinkButton>
                            <asp:LinkButton ID="btnFindNow" runat="server" Text="<%$ Resources:btn_Submit %>" SkinID="btnPrimary"></asp:LinkButton>
                        </div>
                    </div>
                    <Nexus:WildCardValidator ID="vldWildCard" AllowWildCardAtEndErrorMessage="<%$ Resources:Err_WildCardAtEnd %>" NoWildCardErrorMessage="<%$ Resources:Err_NoWildCard %>" ControlsToValidate="txtClaimReference,txtPolicy,txtCreatedBy,txtClient,txtCaseReference,txtPayeeName" Condition="Auto" Display="none" runat="server" EnableClientScript="true">
                    </Nexus:WildCardValidator>
                    <asp:ValidationSummary ID="ValidationSummary" ShowSummary="true" DisplayMode="BulletList" runat="server" CssClass="validation-summary"></asp:ValidationSummary>

                    <div class="grid-card table-responsive">
                        <asp:GridView ID="grdvAuthoriseclaimpayments" runat="server" AllowPaging="true" AutoGenerateColumns="False" GridLines="None" PagerSettings-Mode="Numeric" PageSize="10" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>" AllowSorting="true" EnableViewState="true">
                            <Columns>
                                <asp:BoundField DataField="ClaimPaymentKey" HeaderText="<%$ Resources:lbl_ClaimPaymentKey %>" SortExpression="ClaimPaymentKey"></asp:BoundField>
                                <asp:BoundField DataField="ClaimKey" HeaderText="<%$ Resources:lbl_ClaimKey %>" SortExpression="ClaimKey"></asp:BoundField>
                                <asp:BoundField DataField="CaseNumber" HeaderText="<%$ Resources:lbl_CaseReference %>" SortExpression="CaseNumber"></asp:BoundField>
                                <asp:BoundField DataField="ClaimNumber" HeaderText="<%$ Resources:lbl_ClaimNumber %>" SortExpression="ClaimNumber"></asp:BoundField>
                                <asp:BoundField DataField="PolicyNumber" HeaderText="<%$ Resources:lbl_PolicyNumber %>" SortExpression="PolicyNumber"></asp:BoundField>
                                <asp:BoundField DataField="ClientName" HeaderText="<%$ Resources:lbl_ClientName %>" SortExpression="ClientName"></asp:BoundField>
                                <asp:BoundField DataField="PayeeName" HeaderText="<%$ Resources:lbl_PayeeName %>" SortExpression="PayeeName"></asp:BoundField>
                                <asp:BoundField DataField="PartyType" HeaderText="<%$ Resources:lbl_PartyType %>" SortExpression="PartyType"></asp:BoundField>
                                <Nexus:BoundField DataField="PaymentAmount" HeaderText="<%$ Resources:lbl_PaymentAmount %>" SortExpression="PaymentAmount"></Nexus:BoundField>
                                <asp:BoundField DataField="PaymentDate" HeaderText="<%$ Resources:lbl_PaymentDate %>" HtmlEncode="False" DataFormatString="{0:d}" SortExpression="PaymentDate"></asp:BoundField>
                                <asp:BoundField DataField="CreatedBy" HeaderText="<%$ Resources:lbl_CreatedBy %>" SortExpression="CreatedBy"></asp:BoundField>
                                <asp:BoundField DataField="Status" HeaderText="<%$ Resources:lbl_Status %>" SortExpression="Status"></asp:BoundField>
                                <asp:BoundField DataField="IsReferredForRecommendation" HeaderText="<%$ Resources:lbl_IsReferredForRecommendation %>" SortExpression="IsReferredForRecommendation"></asp:BoundField>
                                <asp:BoundField DataField="RecommendedBy" HeaderText="<%$ Resources:lbl_RecommendedBy %>" SortExpression="RecommendedBy"></asp:BoundField>
                                <asp:BoundField DataField="PaymentAmountBaseCurrency" DataFormatString="{0:F2}" HeaderText="<%$ Resources:lbl_BaseCurrencyAmount %>" SortExpression="PaymentAmountBaseCurrency"></asp:BoundField>                                
                                <asp:BoundField DataField="CurrencyId" Visible="false" HeaderText="Currency Id"></asp:BoundField>

                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <div class="rowMenu">
                                            <ol class="list-inline no-margin">
                                                <li class="dropdown no-padding"><a href="#" title="Action Menu" md-ink-ripple="" data-bs-toggle="dropdown" class="md-btn grey-100 md-flat md-btn-circle"><i class="fa fa-ellipsis-v" aria-hidden="true"></i></a>
                                                    <ol id='menu_<%# Eval("ClaimKey") %>' class="dropdown-menu dropdown-menu-scale pull-right pull-up top text-color">
                                                        <li id="liRecommend" runat="server">
                                                            <asp:LinkButton ID="lnkRecommend" runat="server" CausesValidation="False" CommandName="Recommend" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ClaimKey")%>' Text="<%$ Resources:lbl_Recommend %>">
                                                            </asp:LinkButton>

                                                        </li>
                                                        <li id="liAuthorise" runat="server">
                                                            <asp:LinkButton ID="lnkAuthorise" runat="server" CausesValidation="False" CommandName="Authorise" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ClaimKey")%>' Text="<%$ Resources:lbl_Authorise %>">
                                                            </asp:LinkButton>

                                                        </li>
                                                        <li id="liDecline" runat="server">
                                                            <asp:LinkButton ID="lnkDecline" runat="server" CausesValidation="False" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ClaimKey")%>' Text="<%$ Resources:lbl_Decline %>">
                                                            </asp:LinkButton>
                                                        </li>
                                                        <li id="liView" runat="server">
                                                            <asp:LinkButton ID="lnkView" runat="server" CausesValidation="False" CommandName="Select" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ClaimKey")%>' Text="<%$ Resources:lbl_View %>">
                                                            </asp:LinkButton>
                                                        </li>
                                                    </ol>
                                                </li>
                                            </ol>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </asp:Panel>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="grdvAuthoriseclaimpayments" EventName="PageIndexChanging"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="grdvAuthoriseclaimpayments" EventName="RowCommand"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="grdvAuthoriseclaimpayments" EventName="RowDataBound"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="btnFindNow" EventName="Click"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click"></asp:AsyncPostBackTrigger>
            </Triggers>
        </asp:UpdatePanel>
        <Nexus:ProgressIndicator ID="upAuthoPayment" OverlayCssClass="updating" AssociatedUpdatePanelID="upAuthorizePayment" runat="server">
            <progresstemplate>
                        </progresstemplate>
        </Nexus:ProgressIndicator>
        <asp:HiddenField ID="hdnDuplicateClaimPaymentReason" runat="server"/>
    </div>
</asp:Content>
