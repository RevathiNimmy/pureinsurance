<%@ Control Language="VB" AutoEventWireup="false" CodeFile="BrokerPerformanceReport.ascx.vb"
    Inherits="Nexus.Controls_ReportControls_BrokerPerformanceReport" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<script language="javascript" type="text/javascript">

    function setAgent(sName, sKey, sCode, sAgentType) {
        tb_remove();
        document.getElementById('<%= RP__AGENTSHORTNAME.ClientId%>').value = sCode;
        document.getElementById('<%= txtAgentKey.ClientId%>').value = sAgentType;
        document.getElementById('<%= RP__AGENTSHORTNAME.ClientId%>').focus
    }

</script>
<%-- <asp:ScriptManager ID="ScriptManagerMainDetails" runat="server" />--%>
<div id="BrokerPerformance-control">
    <div class="card">
        <div class="card-body clearfix">
            <div class="form-horizontal">
                <legend>
                    <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:lbl_header %>"></asp:Label></legend>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label runat="Server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="RP__AGENTSHORTNAME" Text="AgentCode" ID="lblbtnAgentCode"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <div class="input-group">
                            <asp:TextBox ID="RP__AGENTSHORTNAME" runat="server" CssClass="form-control" Text="ALL"></asp:TextBox><span class="input-group-btn">
                                <asp:LinkButton ID="btnAgentCode" runat="server" OnClientClick="tb_show(null , '../Modal/FindAgent.aspx?AgentType=Broker&modal=true&KeepThis=true&TB_iframe=true&height=500&width=500' , null);return false;" SkinID="btnModal">
                                <i class="fa fa-search"></i>
                                 <span class="btn-fnd-txt">Agent Code</span>
                                </asp:LinkButton>

                            </span>
                        </div>
                    </div>


                    <asp:HiddenField ID="txtAgentKey" runat="server"></asp:HiddenField>

                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblPeriodStartDate" runat="server" AssociatedControlID="RP__PERIODSTARTDATE" Text="<%$ Resources:lbl_PeriodStartDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <div class="input-group">
                            <asp:TextBox ID="RP__PERIODSTARTDATE" runat="server" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="calPeriodStartDate" runat="server" LinkedControl="RP__PERIODSTARTDATE" HLevel="1"></uc1:CalendarLookup>
                        </div>
                    </div>

                    <asp:RequiredFieldValidator ID="reqvldPeriodStartDate" Display="None" ControlToValidate="RP__PERIODSTARTDATE" runat="server" ErrorMessage="<%$ Resources:lbl_req_PeriodStartDate %>" SetFocusOnError="True" ValidationGroup="vldReportsControlsGroup"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="compvldPeriodStartDate" runat="server" Display="None" ControlToValidate="RP__PERIODSTARTDATE" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_invalid_PeriodStartDate %>" Operator="DataTypeCheck" Type="Date" ValidationGroup="vldReportsControlsGroup"></asp:CompareValidator>
                    <asp:RangeValidator ID="rngvldPeriodStartDate" runat="server" ErrorMessage="<%$ Resources:lbl_invalidrange_PeriodStartDate %>" ControlToValidate="RP__PERIODSTARTDATE" Display="None" ValidationGroup="vldReportsControlsGroup">
                    </asp:RangeValidator>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblPeriodEndDate" runat="server" AssociatedControlID="RP__PERIODENDDATE" Text="<%$ Resources:lbl_PeriodEndDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <div class="input-group">
                            <asp:TextBox ID="RP__PERIODENDDATE" runat="server" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="calPeriodEndDate" runat="server" LinkedControl="RP__PERIODENDDATE" HLevel="1"></uc1:CalendarLookup>
                        </div>
                    </div>

                    <asp:RequiredFieldValidator ID="reqdvldPeriodEndDate" Display="None" ControlToValidate="RP__PERIODENDDATE" runat="server" ErrorMessage="<%$ Resources:lbl_req_PeriodEndDate %>" SetFocusOnError="True" ValidationGroup="vldReportsControlsGroup"> </asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="comvldPeriodEndDate" runat="server" Display="None" ControlToValidate="RP__PERIODENDDATE" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_invalid_PeriodEndDate %>" Operator="DataTypeCheck" Type="Date" ValidationGroup="vldReportsControlsGroup"></asp:CompareValidator>
                    <asp:RangeValidator ID="rngvldPeriodEndDate" runat="server" ErrorMessage="<%$ Resources:lbl_invalidrange_PeriodEndDate %>" ControlToValidate="RP__PERIODENDDATE" Display="None" ValidationGroup="vldReportsControlsGroup">
                    </asp:RangeValidator>
                </div>
            </div>

        </div>
        <div class="card-footer">
            <asp:LinkButton ID="btnGenerateReport" runat="server" Text="<%$ Resources:btnGenerateReport %>" OnClick="GenerateReport" ValidationGroup="vldReportsControlsGroup" SkinID="btnPrimary"></asp:LinkButton>
        </div>
        <asp:HiddenField ID="hvAgentName" runat="server"></asp:HiddenField>

    </div>

</div>
