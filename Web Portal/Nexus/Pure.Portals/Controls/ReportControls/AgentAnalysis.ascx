<%@ Control Language="VB" AutoEventWireup="false" CodeFile="AgentAnalysis.ascx.vb"
    Inherits="Nexus.Controls_ReportControls_AgentAnalysis" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<%@ Register TagPrefix="uc6" TagName="FindParty" Src="~/Controls/FindParty.ascx" %>

<script language="javascript" type="text/javascript">
    function setAgent(sName, sKey, sCode, sAgentType) {
        tb_remove();
        document.getElementById('<%= RP__AGENT.ClientId%>').value = sCode;
        // document.getElementById('<%= RP__AGENT.ClientId%>').value=sKey;
        document.getElementById('<%= txtAgentKey.ClientId%>').value = sAgentType;
        document.getElementById('<%= RP__AGENT.ClientId%>').focus
    }
    function setctl00_cntMainBody_ctl00_PartyNameOtherParty(sName, sKey, sAgentCode, sType) {
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
                    <asp:Label ID="lblPeriodEndDate" runat="server" AssociatedControlID="RP__PERIODDATE" Text="<%$ Resources:lbl_PeriodEndDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <div class="input-group">
                            <asp:TextBox ID="RP__PERIODDATE" runat="server" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="calPeriodEndDate" runat="server" LinkedControl="RP__PERIODDATE" HLevel="1"></uc1:CalendarLookup>
                        </div>
                    </div>

                    <asp:RequiredFieldValidator ID="reqdvldPeriodEndDate" Display="None" ControlToValidate="RP__PERIODDATE" runat="server" ErrorMessage="<%$ Resources:lbl_req_PeriodEndDate %>" SetFocusOnError="True" ValidationGroup="vldReportsControlsGroup"> </asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="comvldPeriodEndDate" runat="server" Display="None" ControlToValidate="RP__PERIODDATE" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_invalid_PeriodEndDate %>" Operator="DataTypeCheck" Type="Date" ValidationGroup="vldReportsControlsGroup"></asp:CompareValidator>
                    <asp:RangeValidator ID="rngvldPeriodEndDate" runat="server" ErrorMessage="<%$ Resources:lbl_invalidrange_PeriodEndDate %>" ControlToValidate="RP__PERIODDATE" Display="None" ValidationGroup="vldReportsControlsGroup">
                    </asp:RangeValidator>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblAgentCode" AssociatedControlID="RP__AGENT" Text="<%$ Resources:lbl_Agent %>" runat="server" class="col-md-4 col-sm-3 control-label">
                    </asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <div class="input-group">
                            <asp:TextBox ID="RP__AGENT" runat="server" CssClass="form-control" Text="ALL"></asp:TextBox>
                            <span class="input-group-btn">
                                <asp:LinkButton ID="btnAgentCode" runat="server" SkinID="btnModal" OnClientClick="tb_show(null , '../Modal/FindAgent.aspx?AgentType=Broker&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;">
                                    <i class="fa fa-search"></i>
                                    <span class="btn-fnd-txt">Agent</span>
                                </asp:LinkButton>
                            </span>
                        </div>
                    </div>

                    <asp:HiddenField ID="txtAgentKey" runat="server"></asp:HiddenField>
                    <asp:RequiredFieldValidator ID="rqdClient" runat="server" ControlToValidate="RP__AGENT" Display="None" ErrorMessage="<%$ Resources:lbl_req_Agent %>" SetFocusOnError="true" ValidationGroup="vldReportsControlsGroup"></asp:RequiredFieldValidator>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblReportBasis" runat="server" AssociatedControlID="RP__SBASIS" Text="<%$ Resources:lbl_ReportBasis %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__SBASIS" runat="server" CssClass="field-medium form-control form-select">
                            <asp:ListItem Text="<%$ Resources:li_TransactionPeriod %>" Value="Transaction Period"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_TransactionDate %>" Value="Transaction Date"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div id="liAgentGroup" runat="server" class="form-group form-group-sm">
                    <uc6:FindParty ID="PartyName" TextToShow="<%$ Resources:lbl_AgentGroup %>" Type="AGG" runat="server" ModalURL="Modal/FindOtherParty.aspx" PassedClientID="ctl00_cntMainBody_PartyName" EnabledTextSearch="true"></uc6:FindParty>
                    <asp:HiddenField ID="RP__AgentGroupCode" runat="server"></asp:HiddenField>
                    <asp:CustomValidator ID="validateParty" runat="Server" ValidationGroup="vldReportsControlsGroup" Enabled="true" EnableClientScript="true" OnServerValidate="ServerValidation" ValidateEmptyText="True" Display="Dynamic" ErrorMessage="Validation Failed"></asp:CustomValidator>
                </div>
            </div>
        </div>
        <div class="card-footer">
            <asp:LinkButton ID="btnGenerateReport" runat="server" Text="<%$ Resources:btnGenerateReport %>" OnClick="GenerateReport" ValidationGroup="vldReportsControlsGroup" SkinID="btnPrimary"></asp:LinkButton>
        </div>
    </div>
</div>
