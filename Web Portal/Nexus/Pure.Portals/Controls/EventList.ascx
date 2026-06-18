<%@ Control Language="VB" AutoEventWireup="false" CodeFile="EventList.ascx.vb" Inherits="Nexus.secure_Controls_EventList" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>

<script language="Javascript" type="text/javascript">

    function setClaimReference(sClaimNumber, sClaimKey, sFileKey) {
        tb_remove();
        var oControl = document.getElementById('<%= txtClaimCode.ClientId%>');
        var oControl1 = document.getElementById('<%= hiddenClaimCode.ClientId%>');
        document.getElementById('<%= txtPolicyCode.ClientId%>').value = '';
        document.getElementById('<%= hInsuranceFileKey.ClientId%>').value = '';
        oControl.value = sClaimNumber;
        oControl1.value = sClaimKey;
    }

    function setQuote(sInsuranceFileRef, iFileKey) {
        tb_remove();
        document.getElementById('<%= txtPolicyCode.ClientId%>').value = sInsuranceFileRef;
        document.getElementById('<%= hInsuranceFileKey.ClientId%>').value = iFileKey;
        document.getElementById('<%= txtClaimCode.ClientId%>').value = '';
        document.getElementById('<%= hiddenClaimCode.ClientId%>').value = '';
    }
    //Sets Casekey and Case Number from popup window "FindCase".
    function setCaseReference(sCaseKey, sCaseNumber) {
        tb_remove();
        $('#<%= hfCaseKey.ClientId%>').val(sCaseKey);
        $('#<%= txtCaseNumber.ClientId%>').val(sCaseNumber);
    }
    //It Reset Case key value. 
    //When ther is a wild card search is done casekey need to be reset
    function ResetCaseKey() {
        $('#<%= hfCaseKey.ClientId%>').val('');
    }
</script>
<%--<asp:ScriptManager ID="smEventDetails" runat="server" />--%>
<div id="Controls_EventList">

    <asp:UpdatePanel ID="updEventList" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>
            <div class="card">
                <div class="card-heading">
                    <h1>
                        <asp:Literal ID="lblHeading" runat="server" Text="<%$ Resources:lblPageHeader %>"></asp:Literal></h1>
                </div>
                <div class="card-body clearfix">
                    <legend>
                        <asp:Label ID="lblHeading1" runat="server" Text="<%$ Resources:lbl_Heading %>"></asp:Label></legend>

                    <div class="form-horizontal">
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblPolicyCode" runat="server" AssociatedControlID="txtPolicyCode" CssClass="col-md-4 col-sm-3 control-label"
                                Text="<%$ Resources:lbl_PolicyCode%>" />
                            <div class="col-md-8 col-sm-9">
                                <div class="input-group">
                                    <asp:TextBox ID="txtPolicyCode" runat="server" CssClass="form-control field-mandatory" />
                                    <span class="input-group-btn">
                                        <asp:LinkButton ID="btnPolicyCode" SkinID="btnModal" runat="server" CausesValidation="false">
                                            <i class="fa fa-search"></i><span class="btn-fnd-txt">Policy Code</span>
                                        </asp:LinkButton>
                                    </span>
                                </div>

                                <asp:HiddenField ID="hInsuranceFileKey" runat="server" />
                                <asp:Button ID="btnRefreshPolicy" runat="server" Text="HiddenRefreshButton" CausesValidation="false"
                                    Style="display: none" />
                            </div>
                        </div>
                        <div id="liCaseNumber" runat="server" visible="false" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblCaseNumber" runat="server" AssociatedControlID="txtCaseNumber" CssClass="col-md-4 col-sm-3 control-label"
                                Text="<%$ Resources:gv_CaseNumber %>" />
                            <div class="col-md-8 col-sm-9">
                                <div class="input-group">
                                    <asp:TextBox ID="txtCaseNumber" runat="server" CssClass="form-control" onblur="ResetCaseKey();" />
                                    <span class="input-group-btn">
                                        <asp:LinkButton ID="btnCaseNumber" SkinID="btnModal" runat="server" CausesValidation="false">
                                            <i class="fa fa-search"></i><span class="btn-fnd-txt">Case Number</span>
                                        </asp:LinkButton>
                                    </span>
                                </div>
                                <asp:HiddenField ID="hfCaseKey" runat="server" />
                            </div>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblClaimCode" runat="server" AssociatedControlID="txtClaimCode" CssClass="col-md-4 col-sm-3 control-label"
                                Text="<%$ Resources:lbl_ClaimCode%>" />
                            <div class="col-md-8 col-sm-9">
                                <div class="input-group">
                                    <asp:TextBox ID="txtClaimCode" runat="server" CssClass="form-control field-mandatory" />
                                    <span class="input-group-btn">
                                        <asp:LinkButton ID="btnClaimCode" SkinID="btnModal" runat="server" CausesValidation="false">
                                            <i class="fa fa-search"></i><span class="btn-fnd-txt">Claim Code</span>
                                        </asp:LinkButton>
                                    </span>
                                </div>

                                <asp:HiddenField ID="hiddenClaimCode" runat="server" />
                                <asp:Button ID="btnRefreshClaimCode" runat="server" Text="HiddenRefreshButton" Style="display: none" />
                            </div>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblEventType" runat="server" AssociatedControlID="ddlEventType" CssClass="col-md-4 col-sm-3 control-label">
                                <asp:Literal ID="litEventType" runat="server" Text="<%$ Resources:lbl_EventType %>" /></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:DropDownList ID="ddlEventType" runat="server" AutoPostBack="true" CssClass="form-control form-select"
                                    OnSelectedIndexChanged="ddlEventType_SelectedIndexChange" />
                            </div>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblUserName" runat="server" AssociatedControlID="ddlUserName" CssClass="col-md-4 col-sm-3 control-label">
                                <asp:Literal ID="litUserName" runat="server" Text="<%$ Resources:lbl_UserName %>" /></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:DropDownList ID="ddlUserName" runat="server" CssClass="form-control form-select" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlUserName_SelectedIndexChange" />
                            </div>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12 clear">
                            <asp:Label ID="lbl_EventFromDate" runat="server" AssociatedControlID="txtEventFromDate" CssClass="col-md-4 col-sm-3 control-label">
                                <asp:Literal ID="litEventFromDate" runat="server" Text="<%$ Resources:lbl_EventFromDate%>" /></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <div class="input-group">
                                    <asp:TextBox ID="txtEventFromDate" runat="server" CssClass="form-control" />
                                    <uc1:CalendarLookup ID="calEventFromDate" runat="server" LinkedControl="txtEventFromDate"
                                        HLevel="3" />
                                </div>
                                <asp:RangeValidator ID="rangevldFromDate" runat="server" Display="None" Type="date"
                                    ErrorMessage="<%$ Resources:lbl_RanErrMsgInvalidFromDate %>" ControlToValidate="txtEventFromDate"
                                    MinimumValue="01/01/1900" SetFocusOnError="True" Enabled="true" ValidationGroup="EventListGroup"
                                    MaximumValue="01/12/9998" />
                            </div>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lbl_EventToDate" runat="server" AssociatedControlID="txtEventToDate" CssClass="col-md-4 col-sm-3 control-label">
                                <asp:Literal ID="litEventToDate" runat="server" Text="<%$ Resources:lbl_EventToDate %>" /></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <div class="input-group">
                                    <asp:TextBox ID="txtEventToDate" runat="server" CssClass="form-control" />
                                    <uc1:CalendarLookup ID="calEventToDate" runat="server" LinkedControl="txtEventToDate"
                                        HLevel="3" />
                                </div>
                                <asp:RangeValidator ID="rangevldToDate" runat="server" Display="None" Type="date"
                                    ErrorMessage="<%$ Resources:lbl_RanErrMsgInvalidToDate %>" ControlToValidate="txtEventToDate"
                                    MinimumValue="01/01/1900" SetFocusOnError="True" Enabled="true" ValidationGroup="EventListGroup"
                                    MaximumValue="01/12/9998" />
                                <asp:CustomValidator ID="CustVldDate" runat="server" Display="None" SetFocusOnError="True"
                                    ValidationGroup="EventListGroup" />
                            </div>
                        </div>
                        <asp:HiddenField ID="hfClaimKey" runat="server" />
                        <asp:HiddenField ID="hfinsurancekey" runat="server" />
                    </div>
                </div>
                <div class="card-footer">
                    <asp:LinkButton runat="server" ID="btnBack" Text="<%$ Resources:btn_btnBack %>" EnableViewState="false" ValidationGroup="EventListGroup" CausesValidation="true" Visible="false" SkinID="btnSecondary"></asp:LinkButton>
                    <asp:LinkButton runat="server" ID="btnNewSearch" Text="<%$ Resources:btn_btnNewSearch %>" EnableViewState="false" ValidationGroup="EventListGroup" CausesValidation="false" SkinID="btnSecondary"></asp:LinkButton>
                    <asp:LinkButton runat="server" ID="btnRefresh" Text="<%$ Resources:btn_Refresh %>" EnableViewState="false" ValidationGroup="EventListGroup" CausesValidation="true" SkinID="btnPrimary"></asp:LinkButton>
                    <asp:LinkButton ID="btnAddEvent" runat="server" Text="<%$ Resources:btn_AddEvent%>" SkinID="btnPrimary"></asp:LinkButton>
                </div>
            </div>
            <asp:ValidationSummary ID="vldSummary" runat="server" DisplayMode="BulletList" ShowSummary="true" ValidationGroup="EventListGroup" HeaderText="<%$Resources:lbl_ValidationSummary %>" CssClass="validation-summary"></asp:ValidationSummary>
            <div class="grid-card table-responsive no-margin">
                <asp:GridView ID="gvEventList" runat="server" AutoGenerateColumns="False" GridLines="None" PageSize="25" AllowPaging="true" AllowSorting="true" PagerSettings-Mode="Numeric" OnRowDataBound="gvEventList_RowDataBound" OnPageIndexChanging="gvEventList_PageIndexChanging" EmptyDataText="<%$ Resources:ErrorMessage %>" EmptyDataRowStyle-CssClass="noData">
                    <Columns>
                          <asp:TemplateField HeaderText="<%$ Resources:gv_EventDate %>" SortExpression="EventDate">
                            <ItemTemplate>
                                <asp:Label ID="lblEventDate" runat="server" Text='<%# Eval("EventDate") %>' DataFormatString="{0:dd/MM/yy H:mm}" Width="120px" ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="EventType" HeaderText="<%$ Resources:gv_EventType %>" SortExpression="EventType"></asp:BoundField>
                        <asp:BoundField DataField="PolicyCode" HeaderText="<%$ Resources:gv_PolicyCode %>" SortExpression="PolicyCode"></asp:BoundField>
                        <asp:BoundField DataField="CaseNumber" HeaderText="<%$ Resources:gv_CaseNumber %>" SortExpression="CaseNumber"></asp:BoundField>
                        <asp:BoundField DataField="ClaimNumber" HeaderText="<%$ Resources:gv_ClaimNumber %>" SortExpression="ClaimNumber"></asp:BoundField>
                        <asp:TemplateField HeaderText="<%$ Resources:gv_Description %>" SortExpression="Description">
                            <ItemTemplate>
                                <asp:Label ID="lblEventDescription" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="UserName" HeaderText="<%$ Resources:gv_UserName %>" SortExpression="UserName"></asp:BoundField>
                        <asp:BoundField DataField="Priority" HeaderText="<%$ Resources:gv_Priority %>" SortExpression="Priority"></asp:BoundField>
                        <asp:TemplateField HeaderText="<%$ Resources:gv_Status %>" SortExpression="StatusKey">
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server" Visible="true" Enabled="true"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="hypEventDetails" runat="server" SkinID="btnGrid" Text="<%$ Resources:hyp_Details %>" CausesValidation="False"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnRefresh" EventName="Click"></asp:AsyncPostBackTrigger>
            <asp:AsyncPostBackTrigger ControlID="btnNewSearch" EventName="Click"></asp:AsyncPostBackTrigger>
            <asp:AsyncPostBackTrigger ControlID="btnBack" EventName="Click"></asp:AsyncPostBackTrigger>
            <asp:AsyncPostBackTrigger ControlID="ddlEventType" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
            <asp:AsyncPostBackTrigger ControlID="ddlUserName" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
        </Triggers>
    </asp:UpdatePanel>
    <Nexus:ProgressIndicator ID="upEventList" OverlayCssClass="updating" AssociatedUpdatePanelID="updEventList" runat="server">
        <progresstemplate>
        </progresstemplate>
    </Nexus:ProgressIndicator>
</div>
