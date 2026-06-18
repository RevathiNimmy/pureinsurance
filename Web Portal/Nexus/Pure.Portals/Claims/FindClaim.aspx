<%@ page language="VB" autoeventwireup="false" inherits="Nexus.Framework_FindClaim" CodeFile="~/Claims/FindClaim.aspx.vb" masterpagefile="~/Default.master" validaterequest="false" enableEventValidation="false" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="CalendarLookup" Src="~/Controls/CalendarLookup.ascx" %>
<%@ Register TagPrefix="uc2" TagName="FindParty" Src="~/Controls/FindParty.ascx" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="server">

    <script type="text/javascript">
        $(document).ready(function() {
            try {
                var Policyno = getQuerystring('Policyno');

                var LossStartDatetxt = getQuerystring('LossStartDate');
                var LossEndDatetxt = getQuerystring('LossEndDate');
                var Claimno = getQuerystring('Claimno').replace("%20", " ");
                LossStartDatetxt = LossStartDatetxt.replace("%2f", "/");
                LossStartDatetxt = LossStartDatetxt.replace("%2f", "/");

                LossEndDatetxt = LossEndDatetxt.replace("%2f", "/");
                LossEndDatetxt = LossEndDatetxt.replace("%2f", "/");
                var hpageloadflag = document.getElementById('ctl00_cntMainBody_hpageloadflag').value;



                if (Policyno != '' && hpageloadflag == '0') {
                    document.getElementById('ctl00_cntMainBody_hpageloadflag').value = '1';
                    document.getElementById('<%=txtPolicy.ClientId%>').value = Policyno;

                    document.getElementById('<%=Claims__LossDateStartLimit.ClientId%>').value = LossStartDatetxt;

                    document.getElementById('<%=Claims__LossDateEndLimit.ClientId%>').value = LossEndDatetxt;

                    document.getElementById('ctl00_cntMainBody_btnFindNow').click();
                }

                if (Claimno != '' && hpageloadflag == '0') {
                    document.getElementById('ctl00_cntMainBody_hpageloadflag').value = '1';
                    document.getElementById('<%=txtClaimReference.ClientId%>').value = Claimno;
                    document.getElementById('ctl00_cntMainBody_btnFindNow').click();
                }


                var hdnAttachedPartyName = document.getElementById('ctl00_cntMainBody_hdnAttachedPartyName').value;
                if (hdnAttachedPartyName != '') {
                    document.getElementById('ctl00_cntMainBody_PartyName_txtPartyName').value = hdnAttachedPartyName;
                    document.getElementById('ctl00_cntMainBody_PartyName_txtPartyName').disabled = true;
                    document.getElementById('ctl00_cntMainBody_PartyName_btnFindParty').disabled = true;
                }
            }
            catch (err) {
                //
            }
        });


        function LinkCaseConfirmation(sMessage) {
            var result = confirm(sMessage);
            return result;
        }

        function CleanFormValidation() {
            var rngdateStartLimit = document.getElementById('<%=rngLossDateStartLimit.ClientId %>');
            var rngdateEndLimit = document.getElementById('<%=rngLossDateEndLimit.ClientId %>');
            ValidatorEnable(rngdateStartLimit, false);
            ValidatorEnable(rngdateEndLimit, false);
        }

        function AddFormValidation() {
            var rngdateStartLimit = document.getElementById('<%=rngLossDateStartLimit.ClientId %>');
            var rngdateEndLimit = document.getElementById('<%=rngLossDateEndLimit.ClientId %>');
            ValidatorEnable(rngdateStartLimit, true);
            ValidatorEnable(rngdateEndLimit, true);
        }
        //Sets Casekey and Case Number from popup window "FindCase".
        function setCaseReference(sCaseKey, sCaseNumber) {
            tb_remove();
            $('#<%= hfCaseKey.ClientId%>').val(sCaseKey);
            $('#<%= txtCaseNumber.ClientId%>').val(sCaseNumber);
        }
        //It Reset Case key value. When ther is a wild card search is done casekey need to be reset
        function ResetCaseKey() {
            $('#<%= hfCaseKey.ClientId%>').val('');
        }
        function getQuerystring(key, default_) {

            if (default_ == null) default_ = "";
            key = key.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
            var regex = new RegExp("[\\?&]" + key + "=([^&#]*)");
            var qs = regex.exec(window.location.href);
            if (qs == null)
                return default_;
            else
                return qs[1];

        }
        function setPolicy(sPolicyRef) {
            tb_remove();
            var sPolicy = document.getElementById('<%=txtPolicy.ClientId%>')

            if (sPolicy == null) {
                document.getElementById('<%=txtPolicy.ClientId%>').value = sPolicyRef;
            }
            else {
                document.getElementById('<%=txtPolicy.ClientId%>').value = sPolicyRef;
            }

        }

        function setClient(sClientName, sClientKey) {
            tb_remove();
            document.getElementById('<%=txtClient.ClientId %>').value = unescape(sClientName);
        }


        function setctl00_cntMainBody_PartyNameOtherParty(sName, sKey, sAgentCode, sType) {
            document.getElementById('ctl00_cntMainBody_PartyName_txtPartyName').value = sAgentCode;
            document.getElementById('ctl00_cntMainBody_PartyName_hPartyKey').value = sKey;
            document.getElementById('ctl00_cntMainBody_PartyName_hPartyType').value = sType;

            self.parent.tb_remove();
        }
    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="Claims_FindClaim">
        <asp:Panel ID="PnlFindClaim" runat="server" DefaultButton="btnFindNow" CssClass="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblTitle" runat="server" Text="<%$ Resources:lbl_Title %>"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblGeneral" runat="server" Text="<%$ Resources:lbl_General %>"></asp:Label></legend>

                    <div id="liCaseNumber" runat="server" visible="false" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label runat="Server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtCaseNumber" Text="<%$ Resources:lbl_CaseNumber %>" ID="lblbtnCaseNumber"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="txtCaseNumber" runat="server" CssClass="form-control" onblur="ResetCaseKey();"></asp:TextBox>
                                <span class="input-group-btn">
                                    <asp:LinkButton ID="btnCaseNumber" runat="server" CausesValidation="false" SkinID="btnModal"><i class="fa fa-search"></i><span class="btn-fnd-txt">Case Number</span></asp:LinkButton>
                                </span>
                            </div>
                        </div>
                        <asp:HiddenField ID="hfCaseKey" runat="server"></asp:HiddenField>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblClaimReference" runat="server" AssociatedControlID="txtClaimReference" Text="<%$ Resources:lbl_ClaimNumber %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtClaimReference" CssClass="field-medium form-control" runat="server"></asp:TextBox>
                        </div>
                        <asp:HiddenField ID="hvMakeClaimNumberReadOnly" runat="server"></asp:HiddenField>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblLossDateStartLimit" runat="server" AssociatedControlID="Claims__LossDateStartLimit" Text="<%$ Resources:lbl_LossDateStartLimit %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="Claims__LossDateStartLimit" CssClass="field-date form-control" runat="server"></asp:TextBox><uc1:CalendarLookup ID="LossDateStartLimit_CalendarLookup" runat="server" LinkedControl="Claims__LossDateStartLimit" HLevel="2"></uc1:CalendarLookup>
                            </div>
                        </div>

                        <asp:RangeValidator ID="rngLossDateStartLimit" runat="Server" Type="Date" Display="none" ControlToValidate="Claims__LossDateStartLimit" SetFocusOnError="True" MinimumValue="01/01/1900" ErrorMessage="<%$ Resources:lbl_LossDateStartLimit_Range_err %>"></asp:RangeValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label runat="Server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtPolicy" Text="<%$ Resources:btn_Policy %>" ID="lblbtnPolicy"></asp:Label><div class="col-md-8 col-sm-9">
                            <div class="input-group mx-1">
                                <asp:TextBox ID="txtPolicy" CssClass="field-medium form-control" runat="server"></asp:TextBox><span class="input-group-btn">
                                    <asp:LinkButton ID="btnPolicy" runat="server" CausesValidation="false" SkinID="btnModal"><i class="fa fa-search"></i><span class="btn-fnd-txt">Policy</span></asp:LinkButton>
                                </span>
                            </div>
                        </div>


                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblLossDateEndLimit" runat="server" AssociatedControlID="Claims__LossDateEndLimit" Text="<%$ Resources:lbl_LossDateEndLimit %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="Claims__LossDateEndLimit" CssClass="field-date form-control" runat="server"></asp:TextBox><uc1:CalendarLookup ID="LossDateEndLimit_CalendarLookup" runat="server" LinkedControl="Claims__LossDateEndLimit" HLevel="2"></uc1:CalendarLookup>
                            </div>
                        </div>

                        <asp:RangeValidator ID="rngLossDateEndLimit" runat="Server" Type="Date" Display="none" ControlToValidate="Claims__LossDateEndLimit" SetFocusOnError="True" MinimumValue="01/01/1900" ErrorMessage="<%$ Resources:lbl_LossDateEndLimit_Range_err %>"></asp:RangeValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label runat="Server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtClient" Text="<%$ Resources:btn_Client%>" ID="lblbtnClient"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="txtClient" CssClass="field-medium form-control" runat="server" MaxLength="20"></asp:TextBox>
                                <span class="input-group-btn">
                                    <asp:LinkButton ID="btnClient" runat="server" CausesValidation="false" SkinID="btnModal">
                                        <i class="fa fa-search"></i>
                                        <span class="btn-fnd-txt">Client</span>
                                    </asp:LinkButton>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblIncludeClosedClaims" runat="server" AssociatedControlID="chkIncludeClosedClaims" Text="<%$ Resources:lbl_IncludeClosedClaims %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:CheckBox ID="chkIncludeClosedClaims" runat="server" Text=" " CssClass="asp-check"></asp:CheckBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblDescription" runat="server" AssociatedControlID="TxtDescription" Text="<%$ Resources:lbl_Description %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="TxtDescription" CssClass="field-medium form-control" runat="server"></asp:TextBox>
                        </div>

                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblRiskIndex" runat="server" AssociatedControlID="txtRiskIndex" Text="<%$ Resources:lbl_RiskIndex %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtRiskIndex" CssClass="field-medium form-control" runat="server"  MaxLength="255"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <uc2:FindParty ID="PartyName" TextToShow="<%$ Resources:lbl_OtherParty %>" Type="Third Party" runat="server" ModalURL="Modal/FindOtherParty.aspx" EnabledWriteOnly="true"></uc2:FindParty>
                        <asp:HiddenField ID="hdnAttachedPartyName" runat="server"></asp:HiddenField>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnNewSearch" runat="server" Text="<%$ Resources:btn_NewSearch%>" CausesValidation="false" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnFindNow" runat="server" Text="<%$ Resources:btn_FindNow%>" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </asp:Panel>
        <Nexus:WildCardValidator ID="vldWildCard" AllowWildCardAtEndErrorMessage="<%$ Resources:lbl_WildCardAtEnd_error %>" NoWildCardErrorMessage="<%$ Resources:lbl_NoWildCard_error %>" Condition="Auto" Display="none" runat="server" EnableClientScript="true">
        </Nexus:WildCardValidator>
       
        <asp:UpdatePanel runat="server" ID="updClaimSearch" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div class="grid-card table-responsive">
                    <asp:GridView ID="grdvSearchResults" runat="server" PageSize="10" AllowPaging="true" PagerSettings-Mode="Numeric" AutoGenerateColumns="false" GridLines="None" EmptyDataText="<%$ Resources:ErrorMessage %>" EmptyDataRowStyle-CssClass="noData" AllowSorting="true" DataKeyNames="InsuranceFileKey">
                        <Columns>
                            <asp:BoundField DataField="CaseNumber" HeaderText="<%$ Resources:lbl_CaseNumber%>" Visible="true" SortExpression="CaseNumber"></asp:BoundField>
                            <asp:BoundField DataField="ClaimNumber" HeaderText="<%$ Resources:lbl_ClaimNumber%>" SortExpression="ClaimNumber"></asp:BoundField>
                            <asp:BoundField DataField="InsuranceRef" HeaderText="<%$ Resources:lbl_PolicyRef %>" SortExpression="InsuranceRef"></asp:BoundField>
                            <asp:BoundField DataField="ClientName" HeaderText="<%$ Resources:lbl_Client %>" SortExpression="ClientName"></asp:BoundField>
                            <asp:TemplateField HeaderText="<%$ Resources:lbl_AssociatedClients %>" SortExpression="AssociatedClients"
                                ItemStyle-CssClass="span-4">
                                <ItemTemplate>
                                    <asp:Repeater ID="rptrAssociateClient" runat="server">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAssociateClientName" runat="server" Text='<%#CType(Container.DataItem, System.Xml.XmlElement).GetAttribute("Name")%>'></asp:Label>
                                            <br />
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                        </HeaderTemplate>
                                        <SeparatorTemplate>
                                        </SeparatorTemplate>
                                    </asp:Repeater>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ProductDescription" HeaderText="<%$ Resources:lbl_ProductCode %>" SortExpression="ProductDescription"></asp:BoundField>
                            <asp:BoundField DataField="ClaimRiskField" HeaderText="<%$ Resources:lbl_ClaimRiskField %>" SortExpression="ClaimRiskField" />
                            <asp:BoundField DataField="LossDateFrom" HeaderText="<%$ Resources:lbl_LossDate %>" HtmlEncode="false" DataFormatString="{0:d}" SortExpression="LossDateFrom"></asp:BoundField>
                             <asp:BoundField DataField="PrimaryCauseDescription" HeaderText="<%$ Resources:lbl_PrimaryCause %>" SortExpression="PrimaryCauseDescription" />
                             <asp:BoundField DataField="SecondaryCauseDescription" HeaderText="<%$ Resources:lbl_SecondaryCause %>" SortExpression="SecondaryCauseDescription" />
                            
                            <asp:BoundField DataField="ProgressStatusDescription" HeaderText="<%$ Resources:lbl_ProgressStatus %>"></asp:BoundField>
                             <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>                                             
                                                       <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("ClaimDescription") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                            <asp:TemplateField>
                                <ItemTemplate>
                                    <div class="rowMenu">
                                        <ol class="list-inline no-margin">
                                            <li class="dropdown no-padding">
                                                <a href="#" title="Action Menu" md-ink-ripple="" data-bs-toggle="dropdown" class="md-btn grey-100 md-flat md-btn-circle">
                                                    <i class="fa fa-ellipsis-v" aria-hidden="true"></i>
                                                </a>
                                                <ol id='menu_<%# Eval("ClaimKey") %>' class="dropdown-menu dropdown-menu-scale pull-right pull-up top text-color">
                                                    <li>
                                                        <asp:LinkButton ID="btnSelect" runat="server" Text="<%$ Resources:FindClaim_btnSelect %>" CommandName="Select" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ClaimNumber")%>' CausesValidation="false"></asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="btnView" runat="server" Text="<%$ Resources:FindClaim_btnView %>" CommandName="View" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ClaimNumber")%>' CausesValidation="false"></asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="btnEdit" runat="server" Text="<%$ Resources:FindClaim_btnEdit %>" CommandName="EditClaim" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ClaimNumber")%>' CausesValidation="false"></asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="btnPay" runat="server" Text="<%$ Resources:FindClaim_btnPay %>" CommandName="Pay" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ClaimNumber")%>' CausesValidation="false"></asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="btnSalvage" runat="server" Text="<%$ Resources:FindClaim_btnSalvage %>" CommandName="Salvage" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ClaimNumber")%>' CausesValidation="false"></asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="btnTPRecovery" runat="server" Text="<%$ Resources:FindClaim_btnTPReceipt %>" CommandName="TPRecovery" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ClaimNumber")%>' CausesValidation="false"></asp:LinkButton>
                                                    </li>
                                                </ol>
                                            </li>
                                        </ol>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:Label ID="lbl_policyLock" runat="server"></asp:Label>
                </div>
                <asp:CustomValidator ID="AllowClaimPayment" runat="server" CssClass="error" ErrorMessage="<%$ Resources:lbl_AllowClaimPayment_error %>" Display="none"></asp:CustomValidator>
                <asp:CustomValidator runat="server" ID="ChkRecoveryReserver" CssClass="error" ErrorMessage="<%$ Resources:lbl_RecoveryReserver_error %>" Display="none"></asp:CustomValidator>
                <asp:CustomValidator ID="vldMediaTypeStatus" runat="server" CssClass="error" ErrorMessage="<%$ Resources:lbl_MediaTypeStatus_Error %>" Display="none"></asp:CustomValidator>
                <asp:CustomValidator ID="ChkClosedClaim" runat="server" CssClass="error" ErrorMessage="<%$ Resources:lbl_ClosedClaim_Error %>" Display="none"></asp:CustomValidator>
                <asp:CustomValidator ID="cvAllowMultipleClaimPayment" runat="server" CssClass="error" ErrorMessage="<%$ Resources:lbl_cvAllowMultipleClaimPayment_error %>" Display="none"></asp:CustomValidator>
                <asp:CustomValidator ID="cvWrittenTypePolicy" runat="server" CssClass="error" ErrorMessage="<%$ Resources:lbl_cvWrittentypePolicy %>" Display="none"></asp:CustomValidator>
                            <asp:ValidationSummary ID="vldFindInsuranceFileSummary" runat="server" DisplayMode="BulletList"
                                CssClass="error" ShowSummary="true" HeaderText="<%$Resources:lbl_ValidationSummary %>" />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="grdvSearchResults" EventName="DataBound"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="grdvSearchResults" EventName="PageIndexChanging"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="grdvSearchResults" EventName="RowCommand"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="grdvSearchResults" EventName="RowDataBound"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="grdvSearchResults" EventName="RowEditing"></asp:AsyncPostBackTrigger>
            </Triggers>
        </asp:UpdatePanel>
        <Nexus:ProgressIndicator ID="upSearchClaim" OverlayCssClass="updating" AssociatedUpdatePanelID="updClaimSearch" runat="server">
            <progresstemplate>
            </progresstemplate>
        </Nexus:ProgressIndicator>
        <asp:HiddenField ID="hpageloadflag" runat="server" Value="0"></asp:HiddenField>
    </div>
</asp:Content>
