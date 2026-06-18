<%@ Control Language="VB" AutoEventWireup="false" CodeFile="AgentStatement.ascx.vb" Inherits="Nexus.Controls_ReportControls_AgentStatement" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<%@ Register TagPrefix="uc6" TagName="FindParty" Src="~/Controls/FindParty.ascx" %>

<script language="javascript" type="text/javascript">
    function setAgent(sName, sKey, sCode, sAgentType) {
        tb_remove();
        document.getElementById('<%= RP__AgentShortName.ClientId%>').value = sCode;
        document.getElementById('<%= txtAgentKey.ClientId%>').value = sAgentType;
        document.getElementById('<%= RP__AgentShortName.ClientId%>').focus
    }

    function setctl00_cntMainBody_PartyNameOtherParty(sName, sKey, sAgentCode, sType) {
        document.getElementById('ctl00_cntMainBody_ctl00_PartyName_txtPartyName').value = sAgentCode;
        document.getElementById('<%= RP__AgentGroupCode.ClientId%>').value = sAgentCode;
        document.getElementById('ctl00_cntMainBody_ctl00_PartyName_hPartyKey').value = sKey;
        document.getElementById('ctl00_cntMainBody_ctl00_PartyName_hPartyType').value = sType;

        self.parent.tb_remove();
        if (sKey == 0) {
            ValidatorEnable($("#<%= validateParty.ClientID %>")[0], true);
        }
        else {
            ValidatorEnable($("#<%= validateParty.ClientID %>")[0], false);
        }

    }
</script>
<div id="Controls_ReportControls_AgentAnalysis">
    <div class="card">
        <div class="card-body clearfix">

            <div class="form-horizontal">
                <legend>
                    <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:lbl_header %>"></asp:Label></legend>


                <div id="liBranch" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblBranch" runat="server" AssociatedControlID="RP__BRANCH_ID" Text="<%$ Resources:lbl_Branch %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__BRANCH_ID" runat="server" CssClass="field-medium form-control form-select">
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblAgentCode" AssociatedControlID="RP__AgentShortName" runat="server" Text="<%$ Resources:lbl_Agent %>" class="col-md-4 col-sm-3 control-label">
                    </asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <div class="input-group">
                            <asp:TextBox ID="RP__AgentShortName" runat="server" CssClass="form-control" Text="ALL"></asp:TextBox>
                            <span class="input-group-btn">
                                <asp:LinkButton ID="btnAgentCode" runat="server" SkinID="btnModal" OnClientClick="tb_show(null , '../Modal/FindAgent.aspx?AgentType=Broker&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;">
                                    <i class="fa fa-search"></i>
                                     <span class="btn-fnd-txt">Agent</span>
                                </asp:LinkButton>
                            </span>
                        </div>
                    </div>
                    <asp:HiddenField ID="txtAgentKey" runat="server"></asp:HiddenField>
                    <asp:RequiredFieldValidator ID="rqdClient" runat="server" ControlToValidate="RP__AgentShortName" Display="None" ErrorMessage="<%$ Resources:lbl_req_Agent %>" SetFocusOnError="true" ValidationGroup="vldReportsControlsGroup"></asp:RequiredFieldValidator>
                </div>


                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblFromDate" runat="server" AssociatedControlID="RP__start_Date" Text="<%$ Resources:lbl_FromDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <div class="input-group">
                            <asp:TextBox ID="RP__start_Date" runat="server" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="calFromDate" runat="server" LinkedControl="RP__start_Date" HLevel="1"></uc1:CalendarLookup>
                        </div>
                    </div>

                    <asp:RequiredFieldValidator ID="reqdvldFromDate" Display="None" ControlToValidate="RP__start_Date" runat="server" ErrorMessage="<%$ Resources:lbl_req_FromDate %>" SetFocusOnError="True" ValidationGroup="vldReportsControlsGroup"> </asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="comvldFromDate" runat="server" Display="None" ControlToValidate="RP__start_Date" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_invalid_FromDate %>" Operator="DataTypeCheck" Type="Date" ValidationGroup="vldReportsControlsGroup"></asp:CompareValidator>
                </div>


                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblToDate" runat="server" AssociatedControlID="RP__End_date" Text="<%$ Resources:lbl_ToDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <div class="input-group">
                            <asp:TextBox ID="RP__End_date" runat="server" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="CalToDate" runat="server" LinkedControl="RP__End_date" HLevel="1"></uc1:CalendarLookup>
                        </div>
                    </div>

                    <asp:RequiredFieldValidator ID="reqdvldToDate" Display="None" ControlToValidate="RP__End_date" runat="server" ErrorMessage="<%$ Resources:lbl_req_ToDate %>" SetFocusOnError="True" ValidationGroup="vldReportsControlsGroup"> </asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="comvldToDate" runat="server" Display="None" ControlToValidate="RP__End_date" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_invalid_ToDate %>" Operator="DataTypeCheck" Type="Date" ValidationGroup="vldReportsControlsGroup"></asp:CompareValidator>


                    <asp:CompareValidator ID="cmpvldStartEndDates" ForeColor="Red" runat="server" ControlToValidate="RP__START_DATE" ControlToCompare="RP__END_DATE" Operator="LessThanEqual" Type="Date" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_invalidGreater_StartDate %>" Display="None" ValidationGroup="vldReportsControlsGroup"></asp:CompareValidator>
                </div>

                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblDateBasis" runat="server" AssociatedControlID="RP__Basis" Text="<%$ Resources:lbl_DateBasis %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__Basis" runat="server" CssClass="field-medium form-control form-select">
                            <asp:ListItem Text="<%$ Resources:li_DateBasis_TransactionDate %>" Value="Transaction Date"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_DateBasis_EffectiveDate %>" Value="Effective Date"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_DateBasis_DueDate %>" Value="Due Date"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblCurrencyType" runat="server" AssociatedControlID="RP__TypeOfCurrency" Text="<%$ Resources:lbl_CurrencyType %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__TypeOfCurrency" runat="server" CssClass="field-medium form-control form-select">
                            <asp:ListItem Text="<%$ Resources:li_CurrencyType_Account %>" Value="Account"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_CurrencyType_Base %>" Value="Base"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_CurrencyType_System %>" Value="System"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_CurrencyType_Transaction%>" Value="Transaction"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblIncludeBalanceAccount" runat="server" AssociatedControlID="RP__IncludeBalanceAccount" Text="<%$ Resources:lbl_IncludeBalanceAccount %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__IncludeBalanceAccount" runat="server" CssClass="field-medium form-control form-select">
                            <asp:ListItem Text="<%$ Resources:li_IncludeBalanceAccount_No %>" Value="No"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_IncludeBalanceAccount_Yes %>" Value="Yes"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div id="liGroupBy" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblGroupBy" runat="server" AssociatedControlID="RP__GroupByCode" Text="<%$ Resources:lbl_GroupBy %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__GroupByCode" runat="server" CssClass="field-medium form-control form-select">
                            <asp:ListItem Text="<%$ Resources:li_GroupBy_NoGrouping %>" Value="No Grouping"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_GroupBy_Branch %>" Value="Branch"></asp:ListItem>

                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblTransactionType" runat="server" AssociatedControlID="RP__TransactionType" Text="<%$ Resources:lbl_TransactionType %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__TransactionType" runat="server" CssClass="field-medium form-control form-select">
                            <asp:ListItem Text="<%$ Resources:li_TransactionType_PremiumClaimTransactions %>" Value="Premium & Claim Transactions"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_TransactionType_ClaimsTransactionsOnly %>" Value="Claims Transactions Only"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_TransactionType_PremiumTransactionsOnly %>" Value="Premium Transactions Only"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblAgeDisplayUnAllocatedCash" runat="server" AssociatedControlID="RP__AgeAlloc" Text="<%$ Resources:lbl_AgeDisplayUnAllocatedCash %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__AgeAlloc" runat="server" CssClass="field-medium form-control form-select">
                            <asp:ListItem Text="<%$ Resources:li_AgeDisplayUnAllocatedCash_Yes %>" Value="Yes"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_AgeDisplayUnAllocatedCash_No %>" Value="No"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                
				<div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblUnderwritingYear" runat="server" AssociatedControlID="RP__Underwriting_Year" Text="<%$ Resources:lbl_UnderwritingYear %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:TextBox ID="RP__Underwriting_Year" runat="server" CssClass="field-medium form-control">                           
                        </asp:TextBox>
                    </div>
                </div>
                <div id="liAgentGroup" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">

                    <uc6:FindParty ID="PartyName" TextToShow="<%$ Resources:lbl_AgentGroup %>" Type="AGG" runat="server" ModalURL="Modal/FindOtherParty.aspx" PassedClientID="ctl00_cntMainBody_PartyName" EnabledTextSearch="true"></uc6:FindParty>
                    <asp:HiddenField ID="RP__AgentGroupCode" runat="server"></asp:HiddenField>
                    <asp:CustomValidator ID="validateParty" runat="Server" ValidationGroup="vldReportsControlsGroup" Enabled="true" EnableClientScript="true" OnServerValidate="ServerValidation" Display="Dynamic" ValidateEmptyText="True" ErrorMessage="Validation Failed"></asp:CustomValidator>
                </div>

            </div>
        </div>
        <div class="card-footer">
            <asp:LinkButton ID="btnGenerateReport" runat="server" Text="<%$ Resources:btnGenerateReport %>" OnClick="GenerateReport" ValidationGroup="vldReportsControlsGroup" SkinID="btnPrimary"></asp:LinkButton>
        </div>
    </div>

</div>
