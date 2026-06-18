<%@ Page Title="" Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false" CodeFile="AuthorizeManualJournal.aspx.vb" Inherits="Nexus.AuthorizeManualJournal" EnableViewState="true" %>

<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <asp:ScriptManager ID="scriptAP" runat="server">
    </asp:ScriptManager>
    <script language="javascript" type="text/javascript">   

        $(document).ready(function () {
            $("#" + "<%= ColumnSelectorExtender1.ClientId%>")[0].childNodes['0'].href = "/localhost/Pure.Portal/secure/AuthorizeManualJournal.aspx#TB_inline?height=400&width=200&inlineId=ColumnSelector";
        });


    </script>
    <script language="javascript" type="text/javascript">

        function setAccount(sShortCode, shiddenShortCode, sAccountName, iPartyKey, sCurrencyCode, sType) //setAccount
        {
            tb_remove();

            //SetAccount
            document.getElementById('<%= txtAccountCode.ClientId%>').value = unescape(sShortCode);
            document.getElementById('<%= hiddenAccountCode.ClientId%>').value = unescape(shiddenShortCode);


        }
    </script>

    <div id="secure_AuthorisePayments">
        <asp:UpdatePanel ID="upAuthorizePayment" runat="server" UpdateMode="Always" ChildrenAsTriggers="true">
            <ContentTemplate>
                <asp:Panel ID="PnlAuthCP" runat="server" DefaultButton="btnFindNow">
                    <div class="card">
                        <div class="card-heading">
                            <h1>
                                <asp:Literal ID="litPaymentsHeader" runat="server" Text="<%$ Resources:lbl_ManualJournalHeader%>" EnableViewState="false"></asp:Literal>
                            </h1>
                        </div>
                        <div id="divHidden" runat="server" visible="false">
                            <div class="card-body clearfix">
                                <div class="form-horizontal">
                                    <legend><span>
                                        <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:lbl_PendingTransaction %>"></asp:Literal></span>
                                    </legend>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Literal ID="litHiddenText" runat="server" Text="<%$ Resources:lbl_ErrorMessage %>"></asp:Literal>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div id="divHeader" runat="server" class="card-body clearfix">
                            <%--------------------------Accordian Start-------------------------------%>

                            <div class="accordion" id="accordionExample">
                                <div class="accordion-item">
                                    <legend>
                                        <div class="row">
                                            <div class="col-lg-6 col-md-6 col-sm-12">

                                                <asp:Literal ID="litHidden" runat="server" Text="<%$ Resources:lbl_PendingTransaction %>"></asp:Literal>

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
                                                    <asp:Label runat="Server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtAccountCode" Text="<%$ Resources:lbl_AccountCode %>" ID="lblbtnAccountCode"></asp:Label>
                                                    <div class="col-md-8 col-sm-9">
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtAccountCode" runat="server" CssClass="form-control"></asp:TextBox><span class="input-group-btn">
                                                                <asp:LinkButton ID="btnAccountCode" runat="server" CausesValidation="false" SkinID="btnModal" OnClientClick="tb_show(null , '../Modal/FindAccount.aspx?modal=true&KeepThis=true&FromPage=IACC&TB_iframe=true&height=500&width=800' , null);return false;">
        <i class="fa fa-search"></i>
        <%--<span class="btn-fnd-txt" >AccountCode</span>--%>
                                                                </asp:LinkButton></span>
                                                        </div>
                                                    </div>
                                                    <asp:HiddenField ID="txtAccountCodeKey" runat="server"></asp:HiddenField>
                                                    <asp:HiddenField ID="hiddenAccountCode" runat="server"></asp:HiddenField>
                                                    <asp:HiddenField ID="hiddenAccountname" runat="server"></asp:HiddenField>
                                                </div>
                                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                                    <asp:Label ID="lblBranch" runat="server" AssociatedControlID="PMLookup_DocumentType" class="col-md-4 col-sm-3 control-label">
                                                        <asp:Literal ID="litType" runat="server" Text="<%$ Resources:lbl_Type %>"></asp:Literal>

                                                    </asp:Label>
                                                    <div class="col-md-8 col-sm-9">

                                                        <asp:DropDownList ID="ddlDocumentType" runat="server" CssClass="field-medium form-control form-select"></asp:DropDownList>

                                                    </div>
                                                </div>
                                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                                    <asp:Label ID="lblTransactionDateFrom" runat="server" AssociatedControlID="txtDateFrom" class="col-md-4 col-sm-3 control-label">
                                                        <asp:Literal ID="DateFrom" runat="server" Text="<%$ Resources:lbl_TransactionDateFrom %>"></asp:Literal>
                                                    </asp:Label>
                                                    <div class="col-md-8 col-sm-9">
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                                                            <uc1:CalendarLookup ID="CalendarLookup2" runat="server" LinkedControl="txtDateFrom" HLevel="5" tabindex="23"></uc1:CalendarLookup>
                                                        </div>
                                                        <asp:RangeValidator ID="rvtxtdatefrom" runat="server" Type="date" Display="none" ControlToValidate="txtdatefrom" MinimumValue="01/01/1900" ErrorMessage="<%$ resources:lbl_errordate%>"></asp:RangeValidator>
                                                    </div>

                                                </div>
                                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                                    <asp:Label ID="lblTransactionDateTo" runat="server" AssociatedControlID="txtDateTo" class="col-md-4 col-sm-3 control-label">
                                                        <asp:Literal ID="DateTo" runat="server" Text="<%$ Resources:lbl_TransactionDateTo %>"></asp:Literal>
                                                    </asp:Label>
                                                    <div class="col-md-8 col-sm-9">
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control" TabIndex="4"></asp:TextBox>
                                                            <uc1:CalendarLookup ID="CalendarLookup1" runat="server" LinkedControl="txtDateTo" HLevel="5" tabindex="25"></uc1:CalendarLookup>
                                                        </div>
                                                        <asp:RangeValidator ID="rvtxtDateTo" runat="Server" Type="Date" Display="none" ControlToValidate="txtDateTo" MinimumValue="01/01/1900" ErrorMessage="<%$ Resources:lbl_ErrorDate%>"></asp:RangeValidator>
                                                    </div>

                                                </div>
                                                <asp:CustomValidator ID="custVldDate" runat="server" Display="none"></asp:CustomValidator>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="hidden">
                                        <NexusProvider:LookupList ID="PMLookup_DocumentType" runat="server" DataItemValue="Code" DataItemText="Description" Sort="ASC" ListType="pmlookup" ListCode="DOCUMENTTYPE" CssClass="field-medium field-mandatory form-control" AutoPostBack="true"></NexusProvider:LookupList>

                                    </div>
                                    <asp:HiddenField ID="hdnJournalAuthoriser" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="hdnApprovalAuthorityLimit" runat="server"></asp:HiddenField>
                                    <%--------------------------Accordian End-----------------------------%>
                                </div>
                                <div id="divSubmit" runat="server" class="card-footer">
                                    <asp:LinkButton ID="btnClear" runat="server" Text="<%$ Resources:btn_Clear %>" CausesValidation="false" SkinID="btnSecondary"></asp:LinkButton>
                                    <asp:LinkButton ID="btnFindNow" runat="server" Text="<%$ Resources:btn_Submit %>" SkinID="btnPrimary"></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                    <asp:ValidationSummary ID="ValidationSummary" ShowSummary="true" DisplayMode="BulletList" HeaderText="" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>


        <nexus:ColumnSelectorExtender ID="ColumnSelectorExtender1" runat="server" CssClass="p-v-sm" ControlToExtend="grdManualJournalTransactions" LinkText="<%$Resources:lbl_SelectColumns_LinkText%>" ButtonText="<%$Resources:lbl_SelectColumns_ButtonText%>" EnableViewState="true" Style="display: none;"></nexus:ColumnSelectorExtender>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>

                <asp:Panel ID="Panel2" runat="server">
                    <div class="grid-card table-responsive">
                        <asp:GridView ID="grdManualJournalTransactions" runat="server" AllowPaging="true" AutoGenerateColumns="False" GridLines="None" PagerSettings-Mode="Numeric" PageSize="10" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>" AllowSorting="true" EnableViewState="true">
                            <Columns>

                                <asp:BoundField DataField="AccountCode" HeaderText="<%$ Resources:lbl_AccountCode %>" SortExpression="AccountCode"></asp:BoundField>
                                <nexus:BoundField HeaderText="<%$ Resources:lbl_Amount %>" DataField="Amount" DataType="Currency" SortExpression="Amount"></nexus:BoundField>
                                <nexus:BoundField HeaderText="<%$ Resources:lbl_CurrencyRate %>" DataField="CurrencyRate" DataType="Currency" SortExpression="CurrencyRate"></nexus:BoundField>
                                <nexus:BoundField DataField="BaseAmount" HeaderText="<%$ Resources:lbl_BaseAmount %>" DataType="Currency" SortExpression="BaseAmount"></nexus:BoundField>

                                <asp:BoundField DataField="AlternateRef" HeaderText="<%$ Resources:lbl_AltReference %>" Visible="false" SortExpression="AlternateRef"></asp:BoundField>
                                <asp:BoundField DataField="Comment" HeaderText="<%$ Resources:lbl_Comment %>" SortExpression="Comment"></asp:BoundField>
                                <asp:BoundField DataField="UnderwritingYearId" HeaderText="<%$ Resources:lbl_UnderwritingYear %>" SortExpression="UnderwritingYearId"></asp:BoundField>
                                <asp:BoundField DataField="CostCenterId" HeaderText="<%$ Resources:lbl_CostCentre%>" SortExpression="CostCenterId"></asp:BoundField>
                                <asp:BoundField DataField="InsuranceRef" HeaderText="<%$ Resources:lbl_InsuranceRef%>" Visible="false" SortExpression="InsuranceRef"></asp:BoundField>
                                <asp:BoundField DataField="PurchaseOrderNumber" HeaderText="<%$ Resources:lbl_PurchaseOrderNo%>" Visible="false" SortExpression="PurchaseOrderNumber"></asp:BoundField>
                                <asp:BoundField DataField="PurchaseInvoiceNumber" HeaderText="<%$ Resources:lbl_PurchaseInvoiceNo%>" Visible="false" SortExpression="PurchaseInvoiceNumber"></asp:BoundField>
                                <asp:BoundField DataField="Status" HeaderText="<%$ Resources:lbl_Status %>" SortExpression="Status"></asp:BoundField>
                                <asp:BoundField DataField="CreatedBy" HeaderText="<%$ Resources:lbl_CreatedBy %>" SortExpression="CreatedBy"></asp:BoundField>
                                <asp:BoundField DataField="CreatedDate" HeaderText="<%$ Resources:lbl_CreatedDate %>" SortExpression="CreatedDate"></asp:BoundField>

                                <asp:TemplateField ShowHeader="True">
                                    <ItemTemplate>
                                        <div class="rowMenu">
                                            <ol class="list-inline no-margin">
                                                <li class="dropdown no-padding"><a href="#" title="Action Menu" md-ink-ripple="" data-bs-toggle="dropdown" class="md-btn grey-100 md-flat md-btn-circle"><i class="fa fa-ellipsis-v" aria-hidden="true"></i></a>
                                                    <ol id='menu_' class="dropdown-menu dropdown-menu-scale pull-right pull-up top text-color">

                                                        <li id="liAuthorise" runat="server">
                                                            <asp:LinkButton ID="lnkAuthorise" runat="server" CausesValidation="False" CommandName="Authorise" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ManualJournalId").ToString.Trim + "," + DataBinder.Eval(Container.DataItem, "Amount").ToString.Trim + "," + DataBinder.Eval(Container.DataItem, "CreatedBy").ToString.Trim  %>' Text="<%$ Resources:lbl_Authorise %>">
                                                            </asp:LinkButton>
                                                        </li>
                                                        <li id="liDecline" runat="server">
                                                            <asp:LinkButton ID="lnkDecline" runat="server" CausesValidation="False" CommandName="Decline" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ManualJournalId").ToString.Trim + "," + DataBinder.Eval(Container.DataItem, "Amount").ToString.Trim + "," + DataBinder.Eval(Container.DataItem, "CreatedBy").ToString.Trim    %>' Text="<%$ Resources:lbl_Decline %>">

                                                            </asp:LinkButton>
                                                        </li>
                                                        <li id="liView" runat="server">
                                                            <asp:LinkButton ID="lnkView" runat="server" CommandName="View" CausesValidation="False" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ManualJournalId").ToString.Trim + "," + DataBinder.Eval(Container.DataItem, "Amount").ToString.Trim + "," + DataBinder.Eval(Container.DataItem, "CreatedBy").ToString.Trim    %>' Text="<%$ Resources:lbl_View %>">
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
                <%--<asp:AsyncPostBackTrigger ControlID="grdManualJournalTransactions" EventName="PageIndexChanging"></asp:AsyncPostBackTrigger>
        <asp:AsyncPostBackTrigger ControlID="grdManualJournalTransactions" EventName="RowCommand"></asp:AsyncPostBackTrigger>
        <asp:AsyncPostBackTrigger ControlID="grdManualJournalTransactions" EventName="RowDataBound"></asp:AsyncPostBackTrigger>--%>
                <asp:AsyncPostBackTrigger ControlID="btnFindNow" EventName="Click"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click"></asp:AsyncPostBackTrigger>
            </Triggers>
        </asp:UpdatePanel>

    </div>
</asp:Content>

