<%@ Control Language="VB" AutoEventWireup="false" CodeFile="AgentPerformanceByYear.ascx.vb"
    Inherits="Nexus.Controls_ReportControls_AgentPerformanceByYear" %>
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

<div id="Controls_ReportControls_AgentPerformanceByYear">
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
                    <asp:Label ID="lblAgentCode" AssociatedControlID="RP__AgentShortName" Text="<%$ Resources:lbl_Agent %>" runat="server" class="col-md-4 col-sm-3 control-label">
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
                    <asp:Label ID="lblReportBasis" runat="server" AssociatedControlID="RP__SBASIS" Text="<%$ Resources:lbl_ReportBasis %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__SBASIS" runat="server" CssClass="field-medium form-control form-select">
                            <asp:ListItem Text="<%$ Resources:li_TransactionPeriod %>" Value="Transaction Period"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_TransactionDate %>" Value="Transaction Date"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <!-- Detail level -->
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblDetailsLevel" runat="server" AssociatedControlID="RP__sReportLevel" Text="<%$ Resources:lbl_DetailLevel %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__sReportLevel" runat="server" CssClass="field-medium form-control form-select">
                            <asp:ListItem Text="<%$ Resources:li_DetailsLevel_Summary %>" Value="Summary"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_DetailsLevel_Detail %>" Value="Detail"></asp:ListItem>

                        </asp:DropDownList>
                    </div>
                </div>

                <!-- Type of Currency -->
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblTypeOfCurrency" runat="server" AssociatedControlID="RP__TypeOfCurrency" Text="<%$ Resources:lbl_TypeOfCurrency %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__TypeOfCurrency" runat="server" CssClass="field-medium form-control form-select">
                            <asp:ListItem Text="<%$ Resources:li_TypeOfCurrency_System %>" Value="System"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_TypeOfCurrency_Base %>" Value="Base"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_TypeOfCurrency_Account %>" Value="Account"></asp:ListItem>


                        </asp:DropDownList>
                    </div>
                </div>

                <!-- Group By -->
                <div id="liGroupBy" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lbl_GroupBy" runat="server" AssociatedControlID="RP__GroupbyCode" Text="<%$ Resources:lbl_GroupBy %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__GroupbyCode" runat="server" CssClass="field-medium form-control form-select">
                            <asp:ListItem Text="<%$ Resources:li_GroupBy_NoGrouping %>" Value="No Grouping"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_GroupBy_Branch %>" Value="Branch"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>



                <div id="liAgentGroup" runat="server" class="form-group form-group-sm">

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
