<%@ Page Title="" Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="ClaimCase.aspx.vb" Inherits="Nexus.Claims_ClaimCase" %>

<%@ Register TagPrefix="uc1" TagName="CalendarLookup" Src="~/Controls/CalendarLookup.ascx" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script language="javascript" type="text/javascript">
        function ShowCloseErrMsg(sMessage) {
            alert(sMessage)
        }

        function CloseClaimChangeCase() {
            tb_remove();
            window.location = '<%=ResolveClientUrl("../Claims/FindCase.aspx") %>';
        }
        function showUpdMsg(sMessage) {
            alert(sMessage)
        }

        function UnLinkCaseConfirmation(sMessage) {
            var result = confirm(sMessage);
            return result;
        }
        function SaveCase(sDesc) {
            tb_remove();
            document.getElementById('<%=hDesc.ClientID %>').value = sDesc;
            __doPostBack('', 'SaveCase')
        }

        function CloseCaseConfirmation(sMsg, sCaseKey, sBaseCaseKey, sUrl) {
            var result = confirm(sMsg);
            if (result == true) {
                document.getElementById('<%=hCaseKey.ClientID %>').value = sCaseKey;
                document.getElementById('<%=hBaseCaseKey.ClientID %>').value = sBaseCaseKey;
                tb_show(null, sUrl, null);
                return false;
            }
            else {
                return false;
            }
        }

        function CloseCase(sDesc) {
            tb_remove();
            document.getElementById('<%=hDesc.ClientID %>').value = sDesc;
            __doPostBack('', 'CloseCase')
        }


        function setClaimReference(sClaimNumber, iClaimKey, sReturn) {
            tb_remove();
            document.getElementById('<%=hClaimNumber.ClientID %>').value = sClaimNumber;
            document.getElementById('<%=hClaimKey.ClientID %>').value = iClaimKey;

            if (sReturn == 'CC') {
                document.getElementById('<%=txtClaimNumber.ClientID %>').value = sClaimNumber;
                document.getElementById('<%=hiddenClaimCode.ClientID %>').value = iClaimKey;
            }
            else {
                __doPostBack('', 'LinkRefresh')
            }
        }

        function pageLoad() {
            //this is needed if the trigger is external to the update panel   
            var manager = Sys.WebForms.PageRequestManager.getInstance();
            manager.add_beginRequest(OnBeginRequest);
        }

        function OnBeginRequest(sender, args) {
            var postBackElement = args.get_postBackElement();
            if (postBackElement.id == 'btnMaintain' || postBackElement.id == "btnTPRecovery" || postBackElement.id == "btnSalvage" || postBackElement.id == "btnUnlink") {
                $get(UpPnlLinkedClaims).style.display = "block";
            }
            if (postBackElement.id == 'btnDetails') {
                $get(UpPnlEvents).style.display = "block";
            }
        }
    </script>

    <asp:ScriptManager ID="smClaimCase" runat="server"></asp:ScriptManager>
    <div id="Portal_Templates_ClaimCase">
        <asp:Panel ID="pnlNewCase" runat="server" CssClass="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblTitle" runat="server" Text="<%$ Resources:lbl_ClaimCase %>"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div class="md-whiteframe-z0 bg-white">
                    <ul class="nav nav-lines nav-tabs b-danger">
                        <li>
                            <a href="#tab-details" data-bs-toggle="tab" aria-expanded="true"  class="active">
                                <asp:Literal runat="server" ID="ltl_tab_Details" Text="<%$ Resources:lbl_tab_Details %>"></asp:Literal>
                            </a>
                        </li>
                        <li id="liTabEvents" runat="server" visible="false">
                            <a href="#tab-caseevent" data-bs-toggle="tab" aria-expanded="true">
                                <asp:Literal runat="server" ID="ltl_tab_Events" Text="<%$ Resources:lbl_tab_Events %>"></asp:Literal>
                            </a>
                        </li>
                    </ul>
                    <div class="tab-content clearfix p b-t b-t-2x">
                        <div id="tab-details" class="tab-pane animated fadeIn active" role="tabpanel">
                            <div class="card-body clearfix no-padding">
                                <div class="form-horizontal">
                                    <legend>
                                        <asp:Label ID="lblFindCase" runat="server" Text="<%$ Resources:lbl_FindCase %>"></asp:Label></legend>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblCaseNumber" runat="server" AssociatedControlID="txtCaseNumber" Text="<%$ Resources:lbl_CaseNumber %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <asp:TextBox ID="txtCaseNumber" Enabled="false" runat="server" CssClass="field-mandator form-control"></asp:TextBox>
                                        </div>
                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCaseNumber" ID="rqdCaseNumber" SetFocusOnError="true" Display="None" ErrorMessage="<%$ Resources:rqderr_CaseNumber %>" Enabled="false"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblProgressStatus" runat="server" AssociatedControlID="drpProgressStatus" Text="<%$ Resources:lbl_ProgressStatus %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <asp:DropDownList ID="drpProgressStatus" runat="server" CssClass="field-medium field-mandatory form-control form-select"></asp:DropDownList>
                                        </div>
                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="drpProgressStatus" ID="rqdProgressStatus" SetFocusOnError="true" Display="None" ErrorMessage="<%$ Resources:rqderr_ProgressStatus %>"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblCaseOpenDate" runat="server" AssociatedControlID="txtCaseOpenDate" Text="<%$ Resources:lbl_CaseOpenDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <div class="input-group">
                                                <asp:TextBox ID="txtCaseOpenDate" runat="server" CssClass="field-date field-mandatory form-control"></asp:TextBox><uc1:CalendarLookup ID="LossDateEndLimit_CalendarLookup" runat="server" LinkedControl="txtCaseOpenDate" HLevel="2"></uc1:CalendarLookup>
                                            </div>
                                        </div>

                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCaseOpenDate" ID="rqdCaseOpenDate" SetFocusOnError="true" Display="None" ErrorMessage="<%$ Resources:rqderr_CaseOpenDate %>"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="rngCaseOpenDate" runat="server" Display="None" SetFocusOnError="true" Type="Date" MinimumValue="01/01/1900" ControlToValidate="txtCaseOpenDate" ErrorMessage="<%$ Resources:rngerr_CaseOpenDate %>"></asp:RangeValidator>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblAnalyst" runat="server" AssociatedControlID="drpAnalyst" Text="<%$ Resources:lbl_Analyst %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <asp:DropDownList ID="drpAnalyst" runat="server" CssClass="field-medium field-mandatory form-control form-select"></asp:DropDownList>
                                        </div>
                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="drpAnalyst" ID="rqdAnalyst" SetFocusOnError="true" Display="None" ErrorMessage="<%$ Resources:rqderr_Analyst %>" InitialValue=""></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblAssistant" runat="server" AssociatedControlID="drpAssistant" Text="<%$ Resources:lbl_Assistant %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <asp:DropDownList ID="drpAssistant" runat="server" CssClass="field-medium field-mandatory form-control form-select"></asp:DropDownList>
                                        </div>
                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="drpAssistant" ID="rqdAssistant" SetFocusOnError="true" Display="None" ErrorMessage="<%$ Resources:rqderr_Assistant %>" InitialValue=""></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblCaseVersion" runat="server" AssociatedControlID="txtCaseVersion" Text="<%$ Resources:lbl_CaseVersion %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <asp:TextBox ID="txtCaseVersion" Enabled="false" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <asp:UpdatePanel ID="PnlLinkedClaims" UpdateMode="Always" ChildrenAsTriggers="true" runat="server">
                                    <ContentTemplate>
                                        <legend>
                                            <asp:Label ID="lblLinkedClaims" runat="server" Text="Linked Claims"></asp:Label></legend>
                                        <asp:Label ID="lblInformation" runat="server" Visible="false"></asp:Label>
                                        <div class="grid-card table-responsive">
                                            <asp:GridView ID="grdvLinkedClaims" runat="server" PageSize="10" AllowPaging="true" AllowSorting="true" PagerSettings-Mode="Numeric" AutoGenerateColumns="false" DataKeyNames="InsuranceFileKey">
                                                <Columns>
                                                    <asp:BoundField DataField="ClaimNumber" HeaderText="<%$ Resources:grdhdr_ClaimNumber %>" SortExpression="ClaimNumber"></asp:BoundField>
                                                    <asp:BoundField DataField="LossDate" HeaderText="<%$ Resources:grdhdr_LossDate %>" SortExpression="LossDate" DataFormatString="{0:d}"></asp:BoundField>
                                                    <asp:BoundField DataField="ClaimHandlerDescription" HeaderText="<%$ Resources:grdhdr_ClaimHandlerDescription %>" SortExpression="ClaimHandlerDescription"></asp:BoundField>
                                                    <asp:BoundField DataField="RiskType" HeaderText="<%$ Resources:grdhdr_RiskType %>" SortExpression="RiskType"></asp:BoundField>
                                                    <asp:BoundField DataField="ClaimStatus" HeaderText="<%$ Resources:grdhdr_ClaimStatus %>" SortExpression="ClaimStatus"></asp:BoundField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <div class="rowMenu">
                                                                <ol class="list-inline no-margin">
                                                                    <li class="dropdown no-padding">
                                                                        <a href="#" title="Action Menu" md-ink-ripple="" data-bs-toggle="dropdown" class="md-btn grey-100 md-flat md-btn-circle">
                                                                            <i class="fa fa-ellipsis-v" aria-hidden="true"></i>
                                                                        </a>
                                                                        <ol id='menu_<%# Eval("ClaimNumber") %>' class="dropdown-menu dropdown-menu-scale pull-right pull-up top text-color">
                                                                            <li>
                                                                                <asp:LinkButton ID="btnEdit" runat="server" Text="<%$ Resources:FindClaim_btnEdit %>" CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ClaimNumber")%>' CausesValidation="false"></asp:LinkButton>
                                                                            </li>
                                                                            <li>
                                                                                <asp:LinkButton ID="btnPay" runat="server" Text="<%$ Resources:FindClaim_btnPay %>" CommandName="Pay" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ClaimNumber")%>'></asp:LinkButton>
                                                                            </li>
                                                                            <li>
                                                                                <asp:LinkButton ID="btnSalvage" runat="server" Text="<%$ Resources:FindClaim_btnSalvage %>" CommandName="Salvage" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ClaimNumber")%>'></asp:LinkButton>
                                                                            </li>
                                                                            <li>
                                                                                <asp:LinkButton ID="btnTPRecovery" runat="server" Text="<%$ Resources:FindClaim_btnTPReceipt %>" CommandName="TPRecovery" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ClaimNumber")%>'></asp:LinkButton>
                                                                            </li>
                                                                            <li>
                                                                                <asp:LinkButton ID="btnUnLink" runat="server" Text="<%$ Resources:lnkbtn_Unlink %>" CommandName="UnLink" CausesValidation="false"></asp:LinkButton>
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
                                        <asp:CustomValidator ID="AllowClaimPayment" runat="server" CssClass="error" ErrorMessage="<%$ Resources:lbl_AllowClaimPayment_error %>" Display="none"></asp:CustomValidator>
                                        <asp:CustomValidator runat="server" ID="ChkRecoveryReserver" CssClass="error" ErrorMessage="<%$ Resources:lbl_RecoveryReserver_error %>" Display="none"></asp:CustomValidator>
                                        <asp:CustomValidator ID="vldMediaTypeStatus" runat="server" CssClass="error" ErrorMessage="<%$ Resources:lbl_MediaTypeStatus_Error %>" Display="none"></asp:CustomValidator>
                                        <asp:CustomValidator ID="ChkClosedClaim" runat="server" CssClass="error" ErrorMessage="<%$ Resources:lbl_ClosedClaim_Error %>" Display="none"></asp:CustomValidator>
                                        <asp:ValidationSummary ID="vldClaimCaseSummary" runat="server" DisplayMode="BulletList" ShowSummary="true" HeaderText="<%$Resources:lbl_ValidationSummary %>" CssClass="validation-summary"></asp:ValidationSummary>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="grdvLinkedClaims" EventName="DataBound"></asp:AsyncPostBackTrigger>
                                        <asp:AsyncPostBackTrigger ControlID="grdvLinkedClaims" EventName="PageIndexChanging"></asp:AsyncPostBackTrigger>
                                        <asp:AsyncPostBackTrigger ControlID="grdvLinkedClaims" EventName="RowCommand"></asp:AsyncPostBackTrigger>
                                        <asp:AsyncPostBackTrigger ControlID="grdvLinkedClaims" EventName="RowDataBound"></asp:AsyncPostBackTrigger>
                                        <asp:AsyncPostBackTrigger ControlID="grdvLinkedClaims" EventName="RowEditing"></asp:AsyncPostBackTrigger>
                                    </Triggers>
                                </asp:UpdatePanel>
                                <nexus:ProgressIndicator ID="UpPnlLinkedClaims" OverlayCssClass="updating" AssociatedUpdatePanelID="PnlLinkedClaims" runat="server">
                                    <ProgressTemplate>
                                    </ProgressTemplate>
                                </nexus:ProgressIndicator>
                            </div>
                            <div class="card-footer no-h-padding">
                                <asp:LinkButton ID="btnClose" runat="server" Text="<%$ Resources:btn_CloseClaim %>" CausesValidation="false" Visible="false" SkinID="btnSecondary"></asp:LinkButton>
                                <asp:LinkButton ID="btnSubmit" runat="server" Text="<%$ Resources:btn_Submit %>" CausesValidation="true" SkinID="btnPrimary"></asp:LinkButton>
                                <asp:LinkButton ID="btnOpen" runat="server" Text="<%$ Resources:btn_OpenClaim %>" CausesValidation="false" Visible="false" SkinID="btnPrimary"></asp:LinkButton>
                                <asp:LinkButton ID="btnLink" runat="server" Visible="false" Text="<%$ Resources:btn_LinkClaim %>" SkinID="btnPrimary"></asp:LinkButton>
                            </div>
                        </div>
                        <div id="tab-caseevent" class="tab-pane animated fadeIn" role="tabpanel">
                            <asp:Panel ID="pnlTabEvents" runat="server" Visible="false">
                                <div class="card-body clearfix no-padding">
                                    <div class="form-horizontal">
                                        <legend>
                                            <asp:Label ID="lblEvents" runat="server" Text="Events"></asp:Label></legend>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblEventCaseNumber" runat="server" AssociatedControlID="txtEventCaseNumber" Text="<%$ Resources:lbl_EventCaseNumber %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                            <div class="col-md-8 col-sm-9">
                                                <asp:TextBox ID="txtEventCaseNumber" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblClaimCode" runat="Server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtClaimNumber" Text="<%$ Resources:btn_ClaimNumber %>"></asp:Label>
                                            <div class="col-md-8 col-sm-9">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtClaimNumber" runat="server" CssClass="field-mandatory form-control"></asp:TextBox>
                                                    <span class="input-group-btn">
                                                        <asp:LinkButton ID="btnClaimCode" runat="server" CausesValidation="false" SkinID="btnModal">
                                                              <i class="fa fa-search"></i>
                                                              <span class="btn-fnd-txt">Client</span>
                                                        </asp:LinkButton>
                                                    </span>
                                                </div>
                                            </div>
                                            <asp:HiddenField ID="hiddenClaimCode" runat="server"></asp:HiddenField>
                                            <asp:Button ID="btnRefreshClaimCode" runat="server" Text="HiddenRefreshButton" Style="display: none"></asp:Button>
                                        </div>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblEventType" runat="server" AssociatedControlID="drpEventType" Text="<%$ Resources:lbl_EventType %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                            <div class="col-md-8 col-sm-9">
                                                <NexusProvider:LookupList ID="drpEventType" runat="server" DataItemValue="Key" DataItemText="Description" ListType="PMLookup" ListCode="event_type_group" DefaultText="(All)" CssClass="field-medium field-mandatory form-control form-select" AutoPostBack="true" OnSelectedIndexChange="drpEventType_SelectedIndexChange"></NexusProvider:LookupList>
                                            </div>
                                        </div>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblUserName" runat="server" AssociatedControlID="drpUserName" Text="<%$ Resources:lbl_UserName %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                            <div class="col-md-8 col-sm-9">
                                                <asp:DropDownList ID="drpUserName" runat="server" AutoPostBack="true" CssClass="form-control form-select"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblFromDate" runat="server" AssociatedControlID="txtFromDate" Text="<%$ Resources:lbl_FromDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                            <div class="col-md-8 col-sm-9">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtFromDate" CssClass="field-date form-control" runat="server" ValidationGroup="TabEvents"></asp:TextBox><uc1:CalendarLookup ID="CalendarLookup1" runat="server" LinkedControl="txtFromDate" HLevel="3"></uc1:CalendarLookup>
                                                </div>
                                            </div>

                                            <asp:RangeValidator ID="rangevldFromDate" runat="server" Display="None" Type="date" ErrorMessage="<%$ Resources:lbl_RanErrMsgInvalidFromDate %>" ControlToValidate="txtFromDate" MinimumValue="01/01/1900" SetFocusOnError="True" Enabled="true" MaximumValue="01/12/9998" ValidationGroup="TabEvents"></asp:RangeValidator>
                                        </div>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblToDate" runat="server" AssociatedControlID="txtToDate" Text="<%$ Resources:lbl_ToDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                            <div class="col-md-8 col-sm-9">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtToDate" CssClass="field-date form-control" runat="server" ValidationGroup="TabEvents"></asp:TextBox><uc1:CalendarLookup ID="CalendarLookup2" runat="server" LinkedControl="txtToDate" HLevel="3"></uc1:CalendarLookup>
                                                </div>
                                            </div>

                                            <asp:RangeValidator ID="rangevldToDate" runat="server" Display="None" Type="date" ErrorMessage="<%$ Resources:lbl_RanErrMsgInvalidToDate %>" ControlToValidate="txtToDate" MinimumValue="01/01/1900" SetFocusOnError="True" Enabled="true" MaximumValue="01/12/9998" ValidationGroup="TabEvents"></asp:RangeValidator>
                                            <asp:CustomValidator ID="CustVldDate" runat="server" Display="None" SetFocusOnError="True" ValidationGroup="TabEvents"></asp:CustomValidator>
                                        </div>
                                    </div>
                                </div>
                                <div class="card-footer">
                                    <asp:LinkButton ID="btnRefresh" runat="server" Text="<%$ Resources:btn_Refresh %>" ValidationGroup="TabEvents" SkinID="btnPrimary"></asp:LinkButton>
                                </div>
                                <asp:UpdatePanel ID="pnlEvents" UpdateMode="Always" ChildrenAsTriggers="true" runat="server">
                                    <ContentTemplate>
                                        <div class="grid-card table-responsive no-margin">
                                            <asp:GridView ID="grdvEvents" runat="server" PageSize="10" AllowPaging="true" PagerSettings-Mode="Numeric" AutoGenerateColumns="false" AllowSorting="true" OnPageIndexChanging="grdvEvents_PageIndexChanging" EmptyDataText="<%$ Resources:ErrorMessage %>" EmptyDataRowStyle-CssClass="noData">
                                                <Columns>
                                                    <asp:BoundField DataField="EventDate" HeaderText="<%$ Resources:grdhdr_EventDate %>" DataFormatString="{0:d}" SortExpression="EventDate"></asp:BoundField>
                                                    <asp:BoundField DataField="EventType" HeaderText="<%$ Resources:grdhdr_Type %>" SortExpression="EventType"></asp:BoundField>
                                                    <asp:BoundField DataField="PolicyCode" HeaderText="<%$ Resources:grdhdr_Policy %>" SortExpression="PolicyCode"></asp:BoundField>
                                                    <asp:BoundField DataField="ClaimNumber" HeaderText="<%$ Resources:grdhdr_Claim %>" SortExpression="ClaimNumber"></asp:BoundField>
                                                    <asp:BoundField DataField="Description" HeaderText="<%$ Resources:grdhdr_Description %>" SortExpression="Description"></asp:BoundField>
                                                    <asp:BoundField DataField="UserName" HeaderText="<%$ Resources:grdhdr_User %>" SortExpression="UserName"></asp:BoundField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <div class="rowMenu">
                                                                <ol id="menu_<%# Eval("EventKey") %>" class="list-inline no-margin">
                                                                    <li>
                                                                        <asp:LinkButton ID="btnDetails" runat="server" Text="<%$ Resources:lnk_Details %>" CommandName="Details" CausesValidation="false" SkinID="btnGrid"></asp:LinkButton>
                                                                    </li>
                                                                </ol>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="grdvEvents" EventName="PageIndexChanging"></asp:AsyncPostBackTrigger>
                                    </Triggers>
                                </asp:UpdatePanel>
                                <nexus:ProgressIndicator ID="UpPnlEvents" OverlayCssClass="updating" AssociatedUpdatePanelID="pnlEvents" runat="server">
                                    <ProgressTemplate>
                                    </ProgressTemplate>
                                </nexus:ProgressIndicator>
                            </asp:Panel>
                        </div>

                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="BulletList" ShowSummary="true" HeaderText="<%$Resources:lbl_ValidationSummary %>" ValidationGroup="TabEvents" CssClass="validation-summary"></asp:ValidationSummary>
    <nexus:WildCardValidator ID="vldWildCard" AllowWildCardAtEndErrorMessage="<%$ Resources:lbl_WildCardAtEnd_error %>" NoWildCardErrorMessage="<%$ Resources:lbl_NoWildCard_error %>" ControlsToValidate="txtCaseNumber" Condition="Auto" Display="none" runat="server" EnableClientScript="true">
    </nexus:WildCardValidator>
    <asp:HiddenField ID="hDesc" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hClaimNumber" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hClaimKey" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hBaseCaseKey" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hCaseKey" runat="server"></asp:HiddenField>
</asp:Content>
