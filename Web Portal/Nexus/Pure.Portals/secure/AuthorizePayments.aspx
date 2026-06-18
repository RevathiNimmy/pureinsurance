<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/default.master" CodeFile="AuthorizePayments.aspx.vb" Inherits="Nexus.AuthorizePayments" EnableViewState="true" %>

<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <asp:ScriptManager ID="scriptAP" runat="server">
    </asp:ScriptManager>
    <script language="javascript" type="text/javascript">   

        $(document).ready(function () {
            $("#"+"<%= ColumnSelectorExtender1.ClientId%>")[0].childNodes['0'].href = "/localhost/Pure.Portal/secure/AuthorizePayments.aspx#TB_inline?height=400&width=200&inlineId=ColumnSelector";
        });

        function setUser(sID, sUserName) {
            tb_remove();
            document.getElementById('<%= txtCreatedBy.ClientId%>').value = sID;
            document.getElementById('<%= txtCreatedBy.ClientId%>').focus();
        }
        function CloseFidUser() {
            tb_remove();
        }
    </script>
    <asp:HiddenField ID="hdnfIsAuthorize" runat="server"></asp:HiddenField>
    <div id="secure_AuthorisePayments">
        <asp:UpdatePanel ID="upAuthorizePayment" runat="server" UpdateMode="Always" ChildrenAsTriggers="true">
            <ContentTemplate>
                <asp:Panel ID="PnlAuthCP" runat="server" DefaultButton="btnFindNow">
                    <div class="card">
                        <div class="card-heading">
                            <h1>
                                <asp:Literal ID="litPaymentsHeader" runat="server" Text="<%$ Resources:litClaimPaymentsHeader%>" EnableViewState="false"></asp:Literal>
                            </h1>
                        </div>

                        <div id="divHidden" runat="server" visible="false">
                            <div class="card-body clearfix">
                                <div class="form-horizontal">
                                    <legend><span>
                                        <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:lbl_AuthorisePayments %>"></asp:Literal></span>
                                    </legend>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Literal ID="litHiddenText" runat="server" Text="<%$ Resources:lbl_ErrorMessage %>"></asp:Literal>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <asp:HiddenField ID="sIsAuthoriser" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hiddenPaymentAmount" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hiddenLimitAmount" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hiddenSource" runat="server"></asp:HiddenField>

                        <div id="divHeader" runat="server" class="card-body clearfix">
                            <%--------------------------Accordian Start-------------------------------%>

                            <div class="accordion" id="accordionExample">
                                <div class="accordion-item">
                                    <legend>
                                        <div class="row">
                                            <div class="col-lg-6 col-md-6 col-sm-12">
                                                <asp:Literal ID="litHidden" runat="server" Text="<%$ Resources:lbl_AuthorisePayments %>"></asp:Literal>
                                            </div>
                                            <div class="col-lg-6 col-md-6 col-sm-6">

                                                <div class="col-lg-1 col-md-1 col-sm-1 float-end">
                                                    <button class="accordion-button acc-style" type="button" data-bs-toggle="collapse" data-bs-target="#collapseOne" aria-expanded="true" aria-controls="collapseOne">
                                                    </button>
                                                </div>
                                            </div>

                                        </div>
                                    </legend>
                                    <div id="collapseOne" class="accordion-collapse collapse show" aria-labelledby="headingOne" data-bs-parent="#accordionExample">
                                        <div class="accordion-body">
                                            <div class="form-horizontal">
                                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                                    <asp:Label ID="lblPayeeName" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtPayeeName" Text="<%$ Resources:lbl_PayeeName %>"></asp:Label>
                                                    <div class="col-md-8 col-sm-9">
                                                        <div class="input-group" style="margin-left: -2.5px">
                                                            <asp:TextBox ID="txtPayeeName" runat="server" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                                    <asp:Label ID="lblBranch" runat="server" AssociatedControlID="ddlBranch" class="col-md-4 col-sm-3 control-label">
                                                        <asp:Literal ID="litBGCustodyBranch" runat="server" Text="<%$ Resources:lbl_Branch %>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                                            <asp:DropDownList ID="ddlBranch" runat="server" CssClass="field-medium form-control form-select"></asp:DropDownList>
                                                        </div>
                                                </div>

                                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                                    <asp:Label ID="lblAccountCode" runat="server" AssociatedControlID="txtDateFrom" class="col-md-4 col-sm-3 control-label">
                                                        <asp:Literal ID="DateFrom" runat="server" Text="Date From"></asp:Literal>
                                                    </asp:Label><div class="col-md-8 col-sm-9">
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control" TabIndex="22"></asp:TextBox><uc1:CalendarLookup ID="CalendarLookup2" runat="server" LinkedControl="txtDateFrom" HLevel="5" TabIndex="23"></uc1:CalendarLookup>
                                                        </div>
                                                        <asp:RangeValidator ID="rvtxtDateFrom" runat="Server" Type="Date" Display="none" ControlToValidate="txtDateFrom" MinimumValue="01/01/1900" ErrorMessage="<%$ Resources:lbl_ErrorDate%>"></asp:RangeValidator>
                                                    </div>

                                                </div>
                                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                                    <asp:Label ID="Label2" runat="server" AssociatedControlID="txtDateTo" class="col-md-4 col-sm-3 control-label">
                                                        <asp:Literal ID="Literal6" runat="server" Text="Date To"></asp:Literal>
                                                    </asp:Label><div class="col-md-8 col-sm-9">
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control" TabIndex="24"></asp:TextBox><uc1:CalendarLookup ID="CalendarLookup1" runat="server" LinkedControl="txtDateTo" HLevel="5" TabIndex="25"></uc1:CalendarLookup>
                                                        </div>
                                                        <asp:RangeValidator ID="rvtxtDateTo" runat="Server" Type="Date" Display="none" ControlToValidate="txtDateTo" MinimumValue="01/01/1900" ErrorMessage="<%$ Resources:lbl_ErrorDate%>"></asp:RangeValidator>
                                                    </div>

                                                    <%--<asp:CustomValidator ID="CustVldDueDate" runat="server" Display="None" SetFocusOnError="true"></asp:CustomValidator>--%>
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
                                                    <asp:Label ID="lblAssignedTo" runat="server" AssociatedControlID="ddlAssignedTo" class="col-md-4 col-sm-3 control-label">
                                                        <asp:Literal ID="Literal2" runat="server" Text="Assigned To"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                                            <asp:DropDownList ID="ddlAssignedTo" runat="server" CssClass="field-medium form-control form-select"></asp:DropDownList>
                                                        </div>
                                                </div>

                                                <div id="liPaymentType" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                                    <asp:Panel ID="Panel1" runat="server">
                                                        <asp:Label ID="lblPaymentType" runat="server" AssociatedControlID="GISLookup_PaymentType" class="col-md-4 col-sm-3 control-label">
                                                            <asp:Literal ID="ltPaymentType" runat="server" Text="Payment Type"></asp:Literal></asp:Label>
                                                        <div class="col-md-8 col-sm-9">
                                                            <asp:DropDownList ID="ddlPaymentType" runat="server" CssClass="field-medium form-control form-select"></asp:DropDownList>
                                                        </div>
                                                    </asp:Panel>
                                                </div>
                                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                                    <asp:Label ID="lblShowOtherPayments" runat="server" AssociatedControlID="lblShowOtherPayments" class="col-md-4 col-sm-3 control-label">
                                                        <asp:Literal ID="Literal4" runat="server" Text="Show all Other Payments"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                                            <asp:CheckBox ID="chklblShowOtherPayments" TabIndex="4" runat="server" Text=" " AutoPostBack="true" OnCheckedChanged="chklblShowOtherPayments_OnCheckedChange" CssClass="asp-check"></asp:CheckBox>
                                                        </div>
                                                </div>
                                                <asp:CustomValidator ID="custVldDate" runat="server" Display="none"></asp:CustomValidator>
                                            </div>
                                        </div>
                                    </div>
                                    
                                    <div class="hidden">
                                        <NexusProvider:LookupList ID="GISLookup_PaymentType" runat="server" DataItemText="Description" DataItemValue="Code" ListCode="CashListItem_Payment_Type" ListType="PMLookup" Sort="Asc" CssClass="field-medium form-control form-select"></NexusProvider:LookupList>
                                    </div>
                                    <%--------------------------Accordian End-----------------------------%>
                                </div>
                                <div id="divSubmit" runat="server" class="card-footer">
                                    <asp:LinkButton ID="btnClear" runat="server" Text="<%$ Resources:btn_Clear %>" CausesValidation="false" SkinID="btnSecondary"></asp:LinkButton>
                                    <asp:LinkButton ID="btnFindNow" runat="server" Text="<%$ Resources:btn_Submit %>" SkinID="btnPrimary"></asp:LinkButton>
                                </div>
                            </div>
                        </div>

                    </div>
                    <nexus:WildCardValidator ID="vldWildCard" AllowWildCardAtEndErrorMessage="<%$ Resources:Err_WildCardAtEnd %>" NoWildCardErrorMessage="<%$ Resources:Err_NoWildCard %>" ControlsToValidate="txtCreatedBy,txtPayeeName,txtDateFrom,txtDateTo" Condition="Auto" Display="none" runat="server" EnableClientScript="true">
                    </nexus:WildCardValidator>
                    <asp:ValidationSummary ID="ValidationSummary" ShowSummary="true" DisplayMode="BulletList" HeaderText="" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
                </asp:Panel>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="grdvAuthorisepayments" EventName="PageIndexChanging"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="grdvAuthorisepayments" EventName="RowCommand"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="grdvAuthorisepayments" EventName="RowDataBound"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="btnFindNow" EventName="Click"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click"></asp:AsyncPostBackTrigger>
            </Triggers>
        </asp:UpdatePanel>
        <nexus:ProgressIndicator ID="ProgressIndicator1" OverlayCssClass="updating" AssociatedUpdatePanelID="upAuthorizePayment" runat="server">
            <ProgressTemplate>
            </ProgressTemplate>
        </nexus:ProgressIndicator>

        <nexus:ColumnSelectorExtender ID="ColumnSelectorExtender1" runat="server" CssClass="p-v-sm" ControlToExtend="grdvAuthorisepayments" LinkText="<%$Resources:lbl_SelectColumns_LinkText%>" ButtonText="<%$Resources:lbl_SelectColumns_ButtonText%>" EnableViewState="true" Style="display:none;"></nexus:ColumnSelectorExtender>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>

                <asp:Panel ID="Panel2" runat="server">
                    <div class="grid-card table-responsive">
                        <asp:GridView ID="grdvAuthorisepayments" runat="server" AllowPaging="true" AutoGenerateColumns="False" GridLines="None" PagerSettings-Mode="Numeric" PageSize="10" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>" AllowSorting="true" EnableViewState="true">
                            <Columns>
                                <asp:BoundField DataField="CashListItemId" HeaderText="CashListItemId" SortExpression="CashListItemId"></asp:BoundField>
                                <asp:BoundField DataField="Branch" Visible="false" HeaderText="<%$ Resources:lbl_Branch %>" SortExpression="Branch"></asp:BoundField>
                                <asp:BoundField DataField="BankAccount" Visible="false" HeaderText="<%$ Resources:lbl_BankAccount %>" SortExpression="BankAccount"></asp:BoundField>
                                <asp:BoundField DataField="TransactionDate" HeaderText="<%$ Resources:lbl_TransDate %>" HtmlEncode="False" DataFormatString="{0:d}" SortExpression="TransactionDate"></asp:BoundField>
                                <asp:BoundField DataField="PaymentType" HeaderText="<%$ Resources:lbl_PaymentType %>" SortExpression="PaymentType"></asp:BoundField>
                                <asp:BoundField DataField="MediaType" HeaderText="<%$ Resources:lbl_MediaType %>" SortExpression="MediaType"></asp:BoundField>
                                <asp:BoundField DataField="MediaRef" HeaderText="<%$ Resources:lbl_MediaRef %>" SortExpression="MediaRef"></asp:BoundField>
                                <asp:BoundField DataField="PolicyRef" HeaderText="<%$ Resources:lbl_PolicyRef %>" SortExpression="PolicyRef"></asp:BoundField>
                                <asp:BoundField DataField="ClaimRef" HeaderText="<%$ Resources:btn_ClaimReference%>" SortExpression="ClaimRef"></asp:BoundField>
                                <asp:BoundField DataField="PayeeAccountName" HeaderText="<%$ Resources:lbl_PayeeName %>" SortExpression="PayeeAccountName"></asp:BoundField>
                                <asp:BoundField DataField="Currency" HeaderText="<%$ Resources:lbl_Currency %>" SortExpression="Currency"></asp:BoundField>
                                <asp:BoundField DataField="Amount" HeaderText="<%$ Resources:lbl_Amount %>" DataFormatString="{0:n2}" HtmlEncode="false" SortExpression="Amount"></asp:BoundField>
                                <asp:BoundField DataField="Status" HeaderText="<%$ Resources:lbl_Status %>" SortExpression="Status"></asp:BoundField>
                                <asp:BoundField DataField="Assignedto" Visible="false" HeaderText="<%$ Resources:lbl_AssignedTo %>" SortExpression="Assignedto"></asp:BoundField>
                                <asp:BoundField DataField="DateAssigned" Visible="false" HeaderText="<%$ Resources:lbl_DateAssigned %>" SortExpression="DateAssigned"></asp:BoundField>
                                <asp:BoundField DataField="BaseCurrencyAmount" HeaderText="<%$ Resources:lbl_BaseCurrencyAmount %>" DataFormatString="{0:n2}" HtmlEncode="false" SortExpression="BaseCurrencyAmount"></asp:BoundField>
                                <asp:BoundField DataField="CreatedBy" HeaderText="<%$ Resources:lbl_CreatedBy %>" SortExpression="CreatedBy"></asp:BoundField>

                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <div class="rowMenu">
                                            <ol class="list-inline no-margin">
                                                <li class="dropdown no-padding"><a href="#" title="Action Menu" md-ink-ripple="" data-bs-toggle="dropdown" class="md-btn grey-100 md-flat md-btn-circle"><i class="fa fa-ellipsis-v" aria-hidden="true"></i></a>
                                                    <ol id='menu_' class="dropdown-menu dropdown-menu-scale pull-right pull-up top text-color">

                                                        <li id="liAuthorise" runat="server">
                                                            <asp:LinkButton ID="lnkAuthorise" runat="server" CausesValidation="False" CommandName="Authorise" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CashListItemId").ToString.Trim + "," + DataBinder.Eval(Container.DataItem, "CreatedBy").ToString.Trim + "," + DataBinder.Eval(Container.DataItem, "Amount").ToString.Trim  %>' Text="<%$ Resources:lbl_Authorise %>">
                                                            </asp:LinkButton>
                                                        </li>
                                                        <li id="liDecline" runat="server">
                                                            <asp:LinkButton ID="lnkDecline" runat="server" CausesValidation="False" CommandName="Decline" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CashListItemId").ToString.Trim + "," + DataBinder.Eval(Container.DataItem, "CreatedBy").ToString.Trim + "," + DataBinder.Eval(Container.DataItem, "Amount").ToString.Trim  %>' Text="<%$ Resources:lbl_Decline %>">

                                                            </asp:LinkButton>
                                                        </li>
                                                        <li id="liView" runat="server">
                                                            <asp:LinkButton ID="lnkView" runat="server" CommandName="Select" CausesValidation="False" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CashListItemId").ToString.Trim + "," + DataBinder.Eval(Container.DataItem, "CreatedBy").ToString.Trim + "," + DataBinder.Eval(Container.DataItem, "Amount").ToString.Trim  %>' Text="<%$ Resources:lbl_View %>">
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
                <asp:AsyncPostBackTrigger ControlID="grdvAuthorisepayments" EventName="PageIndexChanging"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="grdvAuthorisepayments" EventName="RowCommand"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="grdvAuthorisepayments" EventName="RowDataBound"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="btnFindNow" EventName="Click"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click"></asp:AsyncPostBackTrigger>
            </Triggers>
        </asp:UpdatePanel>
        <nexus:ProgressIndicator ID="upAuthoPayment" OverlayCssClass="updating" AssociatedUpdatePanelID="upAuthorizePayment" runat="server">
            <ProgressTemplate>
            </ProgressTemplate>
        </nexus:ProgressIndicator>
        <asp:HiddenField ID="hdnDuplicateClaimPaymentReason" runat="server" />
    </div>
</asp:Content>
