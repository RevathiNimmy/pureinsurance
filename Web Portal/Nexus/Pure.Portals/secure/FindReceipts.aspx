<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="FindReceipts.aspx.vb" Inherits="Nexus.FindReceipts" EnableEventValidation="false" %>

<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script language="javascript" type="text/javascript">


        function CallMe(checkstate, v_iCashListReceiptRowID) {
            var cb = document.getElementById(checkstate);
            if (cb != null && cb.checked == true) {
                if ('<%= HttpContext.Current.Session.IsCookieless %>==True') {
                    //need to pass session id in request path
                    PageMethods.set_path('<%= System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot") %>(S(<%=Session.SessionID%>))/secure/FindReceipts.aspx');
                }
                PageMethods.SelectRecord(cb.checked, v_iCashListReceiptRowID, CallSuccess, CallFailed);
                document.getElementById('<%= btnUpdateAll.ClientId%>').disabled = false;
            }
            else {
                var bCheck = false;
                if (CheckBoxIDs != null) {
                    for (var i = 0; i < CheckBoxIDs.length; i++) {
                        var cbAll = document.getElementById(CheckBoxIDs[i]);
                        if (cbAll != null && cbAll.checked == true) {
                            bCheck = true;
                        }
                    }
                }
                if (bCheck == false) {
                    document.getElementById('<%= btnUpdateAll.ClientId%>').disabled = true;
                }
            }
        }
        // set the destination textbox value with the ContactName
        function CallSuccess(res, destCtrl) {

        }

        // alert message on some failure
        function CallFailed(res, destCtrl) {
            alert(res.get_message());
        }

        onload = function () {
            document.getElementById('<%= btnUpdateAll.ClientId%>').disabled = true;
        }

            function setQuote(sInsuranceFileRef, iFileKey) {
                tb_remove();
                var sPolicy = document.getElementById('<%= txtPolicy.ClientId%>')
                if (sPolicy == null) {
                    document.getElementById('<%= txtPolicy.ClientId%>').value = sInsuranceFileRef;
                }
                else {
                    document.getElementById('<%= txtPolicy.ClientId%>').value = sInsuranceFileRef;
                }

            }

            function MediaTypeConfirmation(message, cb, v_iCashListReceiptRowID) {
                if (document.getElementById(cb).checked) {
                    var r = confirm(message);
                    document.getElementById(cb).checked = r;

                    if (r == true) {
                        if ('<%= HttpContext.Current.Session.IsCookieless %>==True') {
                            //need to pass session id in request path
                            PageMethods.set_path('<%= System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot") %>(S(<%=Session.SessionID%>))/secure/FindReceipts.aspx');
                        }
                        PageMethods.SelectRecord(r, v_iCashListReceiptRowID, CallSuccess, CallFailed);
                        document.getElementById('<%= btnUpdateAll.ClientId%>').disabled = false;
                    }
                    else {
                        document.getElementById('<%= btnUpdateAll.ClientId%>').disabled = true;
                    }
                }
                else {
                    document.getElementById('<%= btnUpdateAll.ClientId%>').disabled = true;
                }
            }

            function setClient(sClientName, sClientKey) {

                tb_remove();
                var sClient = document.getElementById('<%= txtClient.ClientId%>')
                if (sClient == null) {
                    document.getElementById('<%= txtClient.ClientId%>').value = unescape(sClientName);
                document.getElementById('<%= hdClientID.ClientId%>').value = sClientKey;
            }
            else {
                document.getElementById('<%= txtClient.ClientId%>').value = unescape(sClientName);
                document.getElementById('<%= hdClientID.ClientId%>').value = sClientKey;
            }
        }

        function ChangeCheckBoxState(id, checkState) {
            var cb = document.getElementById(id);
            if (cb != null) {
                if (!cb.disabled) {
                    cb.checked = checkState;
                }
            }
        }

        function ChangeAllCheckBoxStates(checkState, message, hdCtrl, IsPayment) {
            var r = true;
            if (IsPayment == 'True' && checkState) {
                r = confirm(message);
                document.getElementById(hdCtrl).checked = r;
            }

            // Toggles through all of the checkboxes defined in the CheckBoxIDs array
            // and updates their value to the checkState input parameter            
            if (CheckBoxIDs != null && r == true) {
                for (var i = 0; i < CheckBoxIDs.length; i++) {
                    ChangeCheckBoxState(CheckBoxIDs[i], checkState);
                    if ('<%= HttpContext.Current.Session.IsCookieless %>==True') {
                        //need to pass session id in request path
                        PageMethods.set_path('<%= System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot") %>(S(<%=Session.SessionID%>))/secure/FindReceipts.aspx');
                    }
                    PageMethods.SelectRecord(checkState, CRRowIDs[i], CallSuccess, CallFailed);
                }
            }
            if (checkState == true && r == true) {
                document.getElementById('<%= btnUpdateAll.ClientId%>').disabled = false;
            }
            else {
                document.getElementById('<%= btnUpdateAll.ClientId%>').disabled = true;
            }
        }

        function RefreshStatus(UpdatePanel, message) {
            tb_remove();
            document.getElementById('<%= btnUpdateAll.ClientId%>').disabled = true;
            __doPostBack(UpdatePanel, message)
        }

        function pageLoad() {
            //this is needed if the trigger is external to the update panel   
            var manager = Sys.WebForms.PageRequestManager.getInstance();
            manager.add_beginRequest(OnBeginRequest);
        }

        function OnBeginRequest(sender, args) {
            var postBackElement = args.get_postBackElement();
            if (postBackElement.id == 'btnUpdateAll') {
                $get(uprogQuotes).style.display = "block";
            }
        }

    </script>

    <asp:ScriptManager ID="ScriptManager" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <div id="secure_FindReceipts">
        <asp:UpdatePanel ID="updFR_UILayout" runat="server" UpdateMode="Always" ChildrenAsTriggers="true">
            <ContentTemplate>
                <asp:Panel ID="PnlFindReceipt" runat="server" CssClass="card" DefaultButton="btnFindNow">
                    <div class="card-heading">
                        <h1>
                            <asp:Literal ID="lblPageHeader" runat="server" Text="<%$ Resources:lblPageHeader%>"></asp:Literal></h1>
                    </div>
                    <div class="card-body clearfix">

                        <div class="form-horizontal">
                            <legend><span>
                                <asp:Literal ID="lblFindReceipts" runat="server" Text="<%$ Resources:lbl_FindReceipts %>"></asp:Literal></span>
                            </legend>


                            <div id="liBranch" runat="server" visible="false" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblBranch" runat="server" AssociatedControlID="ddlBranchCode" Text="<%$ Resources:lbl_SelectBranch %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:DropDownList ID="ddlBranchCode" runat="server" CssClass="field-medium form-control form-select"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblBankAccount" runat="server" AssociatedControlID="ddlBankAccount" Text="<%$ Resources:lbl_BankAccount %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:DropDownList ID="ddlBankAccount" runat="server" CssClass="field-medium form-control form-select"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label runat="Server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtClient" Text="<%$ Resources:btn_Client%>" ID="lblbtnClient"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtClient" CssClass="form-control" runat="server"></asp:TextBox><span class="input-group-btn"><asp:LinkButton ID="btnClient" runat="server" CausesValidation="false" SkinID="btnModal">
                                            <i class="fa fa-search"></i>
                                            <span class="btn-fnd-txt">Client</span>
                                        </asp:LinkButton></span>
                                    </div>
                                </div>


                                <asp:HiddenField ID="hdClientID" runat="server"></asp:HiddenField>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label runat="Server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtPolicy" Text="<%$ Resources:btn_Policy %>" ID="lblbtnPolicy"></asp:Label><div class="col-md-8 col-sm-9">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtPolicy" CssClass="form-control" runat="server"></asp:TextBox><span class="input-group-btn">
                                            <asp:LinkButton ID="btnPolicy" runat="server" CausesValidation="false" SkinID="btnModal">
                                                <i class="fa fa-search"></i>
                                                <span class="btn-fnd-txt">Policy</span>
                                            </asp:LinkButton></span>
                                    </div>
                                </div>


                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblCollectionDateFrom" runat="server" AssociatedControlID="txtCollectionDateFrom" class="col-md-4 col-sm-3 control-label">
                                    <asp:Literal ID="ltCollectionDateFrom" runat="server" Text="<%$ Resources:lbl_CollectionDateFrom %>"></asp:Literal>
                                </asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtCollectionDateFrom" CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>
                                        <uc1:CalendarLookup ID="calCollectionDateFrom" runat="server" LinkedControl="txtCollectionDateFrom" Enabled="false" HLevel="2"></uc1:CalendarLookup>
                                    </div>
                                </div>
                                <asp:RegularExpressionValidator ID="RegValCollectionDateFrom" runat="server" ControlToValidate="txtCollectionDateFrom" Display="None" ErrorMessage="<%$ Resources:lbl_CollectionDateFrom_Regular_err %>" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((1[6-9]|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"></asp:RegularExpressionValidator>
                                <asp:RangeValidator ID="rngCollectionDateFrom" runat="Server" Type="Date" Display="none" ControlToValidate="txtCollectionDateFrom" SetFocusOnError="True" MinimumValue="01/01/1900" MaximumValue="01/01/2100" ErrorMessage="<%$ Resources:lbl_CollectionDateFrom_Range_err %>"></asp:RangeValidator>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblCollectionDateTo" runat="server" AssociatedControlID="txtCollectionDateTo" class="col-md-4 col-sm-3 control-label">
                                    <asp:Literal ID="ltCollectionDateTo" runat="server" Text="<%$ Resources:lbl_CollectionDateTo %>"></asp:Literal>
                                </asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtCollectionDateTo" CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>
                                        <uc1:CalendarLookup ID="calCollectionDateTo" runat="server" LinkedControl="txtCollectionDateTo" Enabled="false" HLevel="2"></uc1:CalendarLookup>
                                    </div>
                                </div>
                                <asp:RegularExpressionValidator ID="RegValCollectionDateTo" runat="server" ControlToValidate="txtCollectionDateTo" Display="None" ErrorMessage="<%$ Resources:lbl_CollectionDateTo_Regular_err %>" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((1[6-9]|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"></asp:RegularExpressionValidator>
                                <asp:RangeValidator ID="rngCollectionDateTo" runat="Server" Type="Date" Display="none" ControlToValidate="txtCollectionDateTo" SetFocusOnError="True" MinimumValue="01/01/1900" MaximumValue="01/01/2100" ErrorMessage="<%$ Resources:lbl_CollectionDateTo_Range_err %>"></asp:RangeValidator>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblMediaReference" runat="server" AssociatedControlID="txtMediaReference" class="col-md-4 col-sm-3 control-label">
                                    <asp:Literal ID="ltMediaReference" runat="server" Text="<%$ Resources:lbl_MediaReference %>"></asp:Literal>
                                </asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ID="txtMediaReference" CssClass="form-control" runat="server" MaxLength="25"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblMediaTypeStatus" runat="server" AssociatedControlID="GISMediaTypeStatus" class="col-md-4 col-sm-3 control-label">
                                    <asp:Literal ID="ltMediaTypeStatus" runat="server" Text="<%$ Resources:lbl_MediaTypeStatus %>"></asp:Literal>
                                </asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <NexusProvider:LookupList ID="GISMediaTypeStatus" runat="server" DataItemValue="Code" DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="MEDIATYPE_STATUS" CssClass="field-medium form-control form-select" DefaultText="<%$ Resources:lbl_DefaultText%>"></NexusProvider:LookupList>
                                </div>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblDrawnBankName" runat="server" AssociatedControlID="GISDrawnBankName" class="col-md-4 col-sm-3 control-label">
                                    <asp:Literal ID="ltDrawnBankName" runat="server" Text="<%$ Resources:lbl_DrawnBankName %>"></asp:Literal>
                                </asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <NexusProvider:LookupList ID="GISDrawnBankName" runat="server" DataItemValue="Code" DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="cashlistItem_Bank" CssClass="field-medium form-control form-select" DefaultText="<%$ Resources:lbl_DefaultText%>"></NexusProvider:LookupList>
                                </div>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblDocumentRef" runat="server" AssociatedControlID="txtDocumentRef" class="col-md-4 col-sm-3 control-label">
                                    <asp:Literal ID="ltDocumentRef" runat="server" Text="<%$ Resources:lbl_DocumentRef %>"></asp:Literal>
                                </asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ID="txtDocumentRef" CssClass="form-control" runat="server" MaxLength="25"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="card-footer">
                        <asp:LinkButton ID="btnNewsearch" runat="server" Text="<%$ Resources:lbl_btnNewSearch %>" SkinID="btnSecondary"></asp:LinkButton>
                        <asp:LinkButton ID="btnUpdateAll" runat="server" TabIndex="4" Text="<%$ Resources:lbl_btnUpdateAll %>" SkinID="btnPrimary"></asp:LinkButton>
                        <asp:LinkButton ID="btnFindNow" runat="server" Text="<%$ Resources:lbl_btnFindNow %>" SkinID="btnPrimary"></asp:LinkButton>
                    </div>
                </asp:Panel>
                <nexus:WildCardValidator ID="vldWildCard" AllowWildCardAtEndErrorMessage="<%$ Resources:lbl_WildCardAtEnd_Error %>" NoWildCardErrorMessage="<%$ Resources:lbl_NoWildCard_Error %>" ControlsToValidate="txtClient,txtPolicy,txtMediaReference,txtDocumentRef" Condition="Auto" Display="none" runat="server" EnableClientScript="true">
                </nexus:WildCardValidator>
                <asp:ValidationSummary ID="ValidationSummary" HeaderText="<%$ Resources:lbl_ValidationSummary %>" DisplayMode="BulletList" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
                <asp:HiddenField ID="hdCheckAll" runat="server"></asp:HiddenField>

            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="gvGetReceiptsdetails" EventName="RowDataBound"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="gvGetReceiptsdetails" EventName="PageIndexChanging"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="btnFindNow" EventName="Click"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="btnNewsearch" EventName="Click"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="btnUpdateAll" EventName="Click"></asp:AsyncPostBackTrigger>
            </Triggers>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdFindReceipt" runat="server" UpdateMode="Always" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div class="grid-card table-responsive">
                    <asp:GridView ID="gvGetReceiptsdetails" runat="server" DataKeyNames="CashListReceiptRowID" PageSize="10" AllowSorting="true" PagerSettings-Mode="Numeric" GridLines="None" AutoGenerateColumns="False" AllowPaging="true" EmptyDataText="<%$ Resources:ErrorMessage %>" EmptyDataRowStyle-CssClass="noData">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="IsPaymentInitiated" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_MediaReference %>" DataField="CashListReceiptRowID" SortExpression="CashListReceiptRowID"></asp:BoundField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox runat="server" ID="chkSelectAll" Text=" " CssClass="asp-check"></asp:CheckBox>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" runat="server" Text=" " CssClass="asp-check"></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_PolicyNumber %>" DataField="PolicyNumber" SortExpression="PolicyNumber"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_ClientCode %>" DataField="ClientCode" SortExpression="ClientCode"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_ClientName %>" DataField="ClientName" SortExpression="ClientName"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_MediaType %>" DataField="MediaTypeDescription" SortExpression="MediaTypeDescription"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_MediaReference %>" DataField="MediaReference" SortExpression="MediaReference"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_DocumentRef %>" DataField="DocumentRef" SortExpression="DocumentRef"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_ChequeStatus %>" DataField="MediaTypeStatusDescription" SortExpression="MediaTypeStatusDescription"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="gvGetReceiptsdetails" EventName="RowDataBound"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="gvGetReceiptsdetails" EventName="PageIndexChanging"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="btnFindNow" EventName="Click"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="btnNewsearch" EventName="Click"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="btnUpdateAll" EventName="Click"></asp:AsyncPostBackTrigger>
            </Triggers>
        </asp:UpdatePanel>
        <nexus:ProgressIndicator ID="upFindReceipt" OverlayCssClass="updating" AssociatedUpdatePanelID="UpdFindReceipt" runat="server">
            <ProgressTemplate>
            </ProgressTemplate>
        </nexus:ProgressIndicator>
    </div>
</asp:Content>
