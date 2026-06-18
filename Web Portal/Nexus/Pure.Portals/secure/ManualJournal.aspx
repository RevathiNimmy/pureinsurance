<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/default.master"
    CodeFile="ManualJournal.aspx.vb" Inherits="secure_ManualJournal" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<asp:Content ID="cntScriptIncludes" ContentPlaceHolderID="ScriptIncludes" runat="server">

    <script language="javascript" type="text/javascript">
        function alertMessage(msg) { alert(msg); }
        function ValidateOccursContent() {
            var vartxtOccurs = document.getElementById('<%= txtOccurs.ClientID %>');
            if (vartxtOccurs == null) { }
            else {
                if (vartxtOccurs.value > 0 && vartxtOccurs.value < 13) { }
                else {
                    vartxtOccurs.value = 1;
                    return false;
                }
            }
        }
        function ValidatePeriodContent() {
            var vartxtPeriod = document.getElementById('<%= txtPeriod.ClientID %>');
            if (vartxtPeriod == null) { }
            else {
                if (vartxtPeriod.value > 0 && vartxtPeriod.value < 366) { }
                else {
                    vartxtPeriod.value = 1;
                    return false;
                }
            }
        }
        function ValidateMonthContent() {
            var vartxtMonth = document.getElementById('<%= txtMonth.ClientID %>');
            if (vartxtMonth == null) { }
            else {
                if (vartxtMonth.value > 0 && vartxtMonth.value < 13) { }
                else {
                    vartxtMonth.value = 1;
                    return false;
                }
            }
        }
        function ValidateQuarterContent() {
            var vartxtQuarter = document.getElementById('<%= txtQuarter.ClientID %>');
            if (vartxtQuarter == null) { }
            else {
                if (vartxtQuarter.value > 0 && vartxtQuarter.value < 13) { }
                else {
                    vartxtQuarter.value = 1;
                    return false;
                }
            }
        }
        function funcTransaction(source, arguments) {
            var varHiddenRowCount = document.getElementById('<%= hiddentGridRowCount.ClientID %>');
            if (varHiddenRowCount.value == "0") {
                arguments.IsValid = false;
                return;
            }
        }
        function ValidateBalanceamount(source, arguments) {
            var varHidden = document.getElementById('<%= hiddenDocumentBalance.ClientID %>');
            if (varHidden.value > 0) {
                arguments.IsValid = false;
            }
            else {
                arguments.IsValid = true;
            }
        }
        function ValidatePageValidators() {
            var isPageValid = true;
            isPageValid = Page_ClientValidate();

            if (isPageValid) {
                var varHidden = document.getElementById('<%= hiddenDocumentBalance.ClientID %>');
                var varmultistep = document.getElementById('<%= hMultistep.ClientID %>');
                var varDebtorGroup = document.getElementById('<%= hDebtorGroup.ClientID %>');
                var varHiddenRowCount = document.getElementById('<%= hiddentGridRowCount.ClientID %>');

                if ((varHidden.value == "0" && varHiddenRowCount.value != "0") || (varHidden.value == "0" && varHiddenRowCount.value != "") || (varHidden.value == "0.00" && varHiddenRowCount.value != "") || (varHidden.value == "0.00" && varHiddenRowCount.value != "0")) {
                <%--   return confirm(document.getElementById('<%= hiddenDocumentBalanceScript.ClientID %>').value);--%>

                    if (confirm(document.getElementById('<%= hiddenDocumentBalanceScript.ClientID %>').value) == true) {
                        if (varmultistep.value == "1" && varDebtorGroup.value == "1") {
                            alert(document.getElementById('<%= hdnmsg_AuthAlert.ClientID %>').value);
                        }
                    }
                    else {
                        alert("Save Cancelled!");
                    }


                }
            }
            else {
                return;
            }
        }

        function DeclineMsg() {
            var isPageValid = true;
            isPageValid = Page_ClientValidate();
            var varUrl = document.getElementById('<%= hdnUrl.ClientID %>');
            if (isPageValid) {
                if (confirm(document.getElementById('<%= hiddenDeclineMsg.ClientID %>').value) == true) {
                    return true;
                }
                else {

                    return false;

                }


            }
        }


        function ValidateOccurs(source, arguments) {
            var varDocType = document.getElementById('<%= PMLookup_DocumentType.ClientID %>');
            var selectedDoc = varDocType.options[varDocType.selectedIndex].value;
            var radioDay = document.getElementById('<%= rbPeriod.ClientID %>');
            var radioMonth = document.getElementById('<%= rbMonth.ClientID %>');
            var radioQuarter = document.getElementById('<%= rbQuarter.ClientID %>');

            if (selectedDoc == "RCJ" || selectedDoc == "DPJ") {
                if (document.getElementById('<%= txtOccurs.ClientID %>').value == '' || document.getElementById('<%= txtOccurs.ClientID %>').value == '0') {
                    arguments.IsValid = false;
                    return;
                }
                if (!radioDay.checked && !radioMonth.checked && !radioQuarter.checked) {
                    arguments.IsValid = false;
                    return;
                }
            }
            else {
                return true;
            }
        }
        function ValidateReverseDate(source, arguments) {
            var varDocType = document.getElementById('<%= PMLookup_DocumentType.ClientID %>');
            var selectedDoc = varDocType.options[varDocType.selectedIndex].value;
            if (selectedDoc == "ACC" || selectedDoc == "RVJ" || selectedDoc == "PPT") {
                if (document.getElementById('<%= txtReversesDate.ClientID %>').value == '') {
                    arguments.IsValid = false;
                    return;
                }
            }
            else {
                return true;
            }
        }

        function pageLoad() {
            //this is needed if the trigger is external to the update panel   
            var manager = Sys.WebForms.PageRequestManager.getInstance();
            manager.add_beginRequest(OnBeginRequest);
        }

        function OnBeginRequest(sender, args) {
            var postBackElement = args.get_postBackElement();
            if (postBackElement.id == 'btnAdd') {
                $get(uprogQuotes).style.display = "block";
            }
        }





        function ValidateDate() {
            var daterequested = document.getElementById('<%= txtDate.ClientID %>').value;
            var dateMMDDYYYRegex = "^[0-9]{2}/[0-9]{2}/[0-9]{4}$";
            if (daterequested.match(dateMMDDYYYRegex)) {

                return true;
            } else {
                alert('Enter a valid date');
                return false;
            }
        }
    </script>

</asp:Content>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></cc1:ToolkitScriptManager>
    <div id="secure_ManualJournal">

        <div class="card-heading">
            <h1>
                <asp:Literal ID="lblPageHeader" runat="server" Text="<%$ Resources:lblPageHeader %>"></asp:Literal></h1>
        </div>
        <div class="card-body clearfix">
            <div class="card">

                <div class="form-horizontal">
                    <ul class="nav nav-lines nav-tabs b-danger">
                        <li>
                            <a data-bs-toggle="tab" href="#tab-general" aria-expanded="true" class="active">
                                <asp:Literal ID="lbltab_general" runat="server" Text="<%$Resources:lbl_tabSearchFormLegend %>"></asp:Literal></a>
                        </li>
                        <li id="liCommentTab" runat="server" class="">
                            <a data-bs-toggle="tab" href="#tab-comment" aria-expanded="false">
                                <asp:Literal ID="lbltab_Comment" runat="server" Text="<%$Resources:lbl_tabComment %>"></asp:Literal></a>
                        </li>
                    </ul>

                    <div class="tab-content clearfix p b-t b-t-2x">
                        <div id="tab-general" class="tab-pane animated fadeIn active" role="tabpanel">

                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblType" AssociatedControlID="PMLookup_DocumentType" runat="server" Text="<%$ Resources:lblType %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <NexusProvider:LookupList ID="PMLookup_DocumentType" runat="server" DataItemValue="Code" DataItemText="Description" Sort="ASC" ListType="pmlookup" ListCode="DOCUMENTTYPE" CssClass="field-medium field-mandatory form-control" AutoPostBack="true"></NexusProvider:LookupList>
                                </div>
                            </div>
                            <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Always" ChildrenAsTriggers="true">
                                <ContentTemplate>

                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblBranch" AssociatedControlID="drpBranch" runat="server" Text="<%$Resources:lblBranch %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <asp:DropDownList ID="drpBranch" AutoPostBack="true" runat="server" CssClass="field-mandatory form-control"></asp:DropDownList>
                                        </div>
                                        <asp:RequiredFieldValidator ID="vldrqdBranch" runat="server" ControlToValidate="drpBranch" Display="None" ErrorMessage="<%$ Resources: vldrqdBranch%>" SetFocusOnError="true" ValidationGroup="JournalErroGroup"></asp:RequiredFieldValidator>
                                    </div>
                                    <div id="liSubBranchCode" runat="server" visible="true" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="SubBranchCode" runat="server" AssociatedControlID="ddlSubBranchCode" Text="<%$ Resources:lbl_SubBranch %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <asp:DropDownList ID="ddlSubBranchCode" runat="server" DataValueField="Code" DataTextField="Description" CssClass="field-mandatory form-control"></asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="vldSubBranchCodeRequired" runat="server" Display="None" ControlToValidate="ddlSubBranchCode" ErrorMessage="<%$ Resources:vldSubBranchCode %>" SetFocusOnError="True">

                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label runat="server" ID="lblDate" AssociatedControlID="txtDate" Text="<%$Resources:lblDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <div class="input-group">
                                                <asp:TextBox runat="server" ID="txtDate" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="calDateDate" runat="server" LinkedControl="txtDate" HLevel="3"></uc1:CalendarLookup>
                                            </div>
                                        </div>

                                        <asp:RangeValidator ID="rangevldDate" runat="server" Display="None" ErrorMessage="<%$ Resources:custvldDate %>" ControlToValidate="txtDate" Type="Date" SetFocusOnError="true"></asp:RangeValidator>

                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label runat="server" ID="lblComments" AssociatedControlID="txtComments" Text="<%$Resources:lblComments %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <asp:TextBox runat="server" ID="txtComments" MaxLength="255" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div id="divRecurring" runat="server">
                                        <legend>
                                            <asp:Label ID="lblRecurring" runat="server" Text="<%$Resources:lblRecurring %>"></asp:Label></legend>
                                         <div class="form-horizontal">
                                             
                                        <div class=" form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label runat="server" ID="lblOccurs" AssociatedControlID="txtDate" Text="<%$Resources:lblOccurs %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                            <div class="col-md-8 col-sm-9">
                                                <asp:TextBox runat="server" ID="txtOccurs" CssClass="field-small field-mandatory form-control" onkeypress="javascript:return false;" onblur="javascript:return ValidateOccursContent();"></asp:TextBox>
                                                <asp:ImageButton ID="btnOccursUp" runat="server" SkinID="uparrow" ImageUrl="<%$Resources:btnOccursUp %>" CausesValidation="false" CssClass="upimage"></asp:ImageButton>

                                                <asp:ImageButton ID="btnOccursDown" runat="server" SkinID="downarrow" ImageUrl="<%$Resources:btnOccursDown %>" CausesValidation="false" CssClass="downimage"></asp:ImageButton>
                                                <cc1:NumericUpDownExtender ID="nudeOccurs" runat="server" TargetControlID="txtOccurs" TargetButtonDownID="btnOccursDown" TargetButtonUpID="btnOccursUp" Minimum="1" Maximum="12"></cc1:NumericUpDownExtender>
                                                <asp:Label runat="server" ID="lblTimes" Text="<%$Resources:lblTimes %>" class="col-md-4 col-sm-3 control-label"></asp:Label>

                                            </div>
                                            <asp:CustomValidator ID="custvldOccurs" runat="server" Display="None" ErrorMessage="<%$Resources:vldrqdOccurs %>" EnableClientScript="true" ClientValidationFunction="ValidateOccurs" OnServerValidate="custvldOccurs_ServerValidate"></asp:CustomValidator>
                                        </div>
                                        <div class=" form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:RadioButton runat="server" GroupName="rblPer" ID="rbPeriod" Text="<%$Resources:lblPeriod %>" class="col-md-4 col-sm-4  asp-radio"></asp:RadioButton>
                                            <asp:Label runat="server" ID="lblPeriod" AssociatedControlID="txtDate"></asp:Label>
                                            <div class="col-md-8 col-sm-9">
                                                <asp:TextBox runat="server" ID="txtPeriod" CssClass="field-small form-control" onkeypress="javascript:return false;" onblur="javascript:return ValidatePeriodContent();"></asp:TextBox>
                                                <asp:ImageButton ID="btnPeriodUp" runat="server" SkinID="uparrow" ImageUrl="<%$Resources:btnPeriodUp %>" CausesValidation="false" CssClass="upimage"></asp:ImageButton>

                                                <asp:ImageButton ID="btnPeriodDown" runat="server" SkinID="downarrow" ImageUrl="<%$Resources:btnPeriodDown %>" CausesValidation="false" CssClass="downimage"></asp:ImageButton>
                                                <cc1:NumericUpDownExtender ID="nudePeriod" runat="server" TargetControlID="txtPeriod" TargetButtonDownID="btnPeriodDown" TargetButtonUpID="btnPeriodUp" Minimum="1" Maximum="365"></cc1:NumericUpDownExtender>

                                            </div>
                                        </div>
                                        <div class=" form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:RadioButton runat="server" GroupName="rblPer" ID="rbMonth" Text="<%$Resources:lblMonth %>" class="col-md-4 col-sm-3 asp-radio"></asp:RadioButton>
                                            <asp:Label runat="server" ID="lblMonth" AssociatedControlID="txtDate"> </asp:Label>
                                            <div class="col-md-8 col-sm-9">
                                                <asp:TextBox runat="server" ID="txtMonth" CssClass="field-small form-control" onkeypress="javascript:return false;" onblur="javascript:return ValidateMonthContent();"></asp:TextBox>

                                                <asp:ImageButton ID="btnMonthUp" runat="server" SkinID="uparrow" ImageUrl="<%$Resources:btnMonthUp %>" CausesValidation="false" CssClass="upimage"></asp:ImageButton>

                                                <asp:ImageButton ID="btnMonthDown" runat="server" SkinID="downarrow" ImageUrl="<%$Resources:btnMonthDown %>" CausesValidation="false" CssClass="downimage"></asp:ImageButton>
                                                <cc1:NumericUpDownExtender ID="nudeMonth" runat="server" TargetControlID="txtMonth" TargetButtonDownID="btnMonthDown" TargetButtonUpID="btnMonthUp" Minimum="1" Maximum="12"></cc1:NumericUpDownExtender>
                                            </div>
                                        </div>
                                        <div class=" form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:RadioButton runat="server" GroupName="rblPer" ID="rbQuarter" Text="<%$Resources:lblQuarter %>" class="col-md-4 col-sm-3 asp-radio"></asp:RadioButton>
                                            <asp:Label runat="server" ID="lblQuarter" AssociatedControlID="txtDate"></asp:Label>
                                            <div class="col-md-8 col-sm-9">
                                                <asp:TextBox runat="server" ID="txtQuarter" CssClass="field-small form-control" onkeypress="javascript:return false;" onblur="javascript:return ValidateQuarterContent();"></asp:TextBox>

                                                <asp:ImageButton ID="btnQuarterUp" runat="server" ImageUrl="<%$Resources:btnQuarterUp %>" CausesValidation="false" CssClass="upimage"></asp:ImageButton>

                                                <asp:ImageButton ID="btnQuarterDown" runat="server" ImageUrl="<%$Resources:btnQuarterDown %>" CausesValidation="false" CssClass="downimage"></asp:ImageButton>
                                                <cc1:NumericUpDownExtender ID="nudeQuarter" runat="server" TargetControlID="txtQuarter" TargetButtonDownID="btnQuarterDown" TargetButtonUpID="btnQuarterUp" Minimum="1" Maximum="12"></cc1:NumericUpDownExtender>
                                            </div>
                                        </div>
                                        <div class=" form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblReverseDate" AssociatedControlID="txtReversesDate" runat="server" Text="<%$Resources:lblReverseDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                            <div class="col-md-8 col-sm-9">
                                                <div class="input-group">
                                                    <asp:TextBox runat="server" ID="txtReversesDate" CssClass="field-mandatory form-control"></asp:TextBox><uc1:CalendarLookup ID="calRevDate" runat="server" LinkedControl="txtReversesDate" HLevel="4"></uc1:CalendarLookup>
                                                </div>
                                            </div>

                                            <asp:CustomValidator ID="custvldReverseDate" runat="server" Display="None" ErrorMessage="<%$Resources:vldrqdReverseDate %>" ClientValidationFunction="ValidateReverseDate" EnableClientScript="true" OnServerValidate="custvldReverseDate_ServerValidate"></asp:CustomValidator>
                                            <asp:RangeValidator ID="rangevldReverseDate" runat="server" Display="None" ErrorMessage="<%$ Resources:custvldReverseDate %>" ControlToValidate="txtReversesDate" Type="Date" SetFocusOnError="true"></asp:RangeValidator>
                                        </div>
                                             </div>

                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div id="tab-comment" class="tab-pane animated fadeIn " role="tabpanel">
                            <asp:Panel ID="PnlComment" runat="server" Visible="true">
                                <%-- <asp:UpdatePanel ID="UpdComment" runat="server" ChildrenAsTriggers="true" UpdateMode="Always">
        <ContentTemplate>--%>
                                <div class="form-horizontal">
                                    <div class="row">
                                        <div class="col-3">
                                            <asp:Label ID="lblCommentstxt" Style="margin-left: 15px; margin-top: 40px;" runat="server" AssociatedControlID="txtApproversComments" Text="<%$ Resources:lblComments%>" class="col-md-3 col-sm-4 control-label"></asp:Label>
                                        </div>
                                        <div class="col-8">
                                            <asp:TextBox runat="server" Disabled="true" TextMode="MultiLine" Style="margin-top: 20px; margin-bottom: 15px; height: 100px;" CssClass="form-control" ID="txtApproversComments">
                                            </asp:TextBox>

                                        </div>
                                    </div>
                                </div>
                                <%--        </ContentTemplate>
    </asp:UpdatePanel>
    <Nexus:ProgressIndicator ID="PIComment" OverlayCssClass="updating" AssociatedUpdatePanelID="UpdpnlPayee" runat="server">
        <progresstemplate>
                </progresstemplate>
    </Nexus:ProgressIndicator>--%>
                            </asp:Panel>

                        </div>

                    </div>

                    <div class="card-footer">
                        <asp:LinkButton ID="btnAdd" runat="server" Text="<%$Resources:btnAdd %>" CausesValidation="false" OnClientClick="javascript: return ValidateDate();" SkinID="btnPrimary"></asp:LinkButton>
                        <asp:LinkButton ID="btnFinish" runat="server" Text="<%$Resources:btnFinish %>" OnClientClick="javascript: return ValidatePageValidators();" SkinID="btnPrimary"></asp:LinkButton>
                    </div>
                    <div>
                        <asp:ValidationSummary ID="vldSummary" HeaderText="<%$ Resources:vldSummary %>" DisplayMode="BulletList" runat="server" ShowSummary="true" CssClass="validation-summary"></asp:ValidationSummary>
                        <asp:CustomValidator ID="custvldTransaction" runat="server" Display="None" ErrorMessage="<%$ Resources:custvldTransaction %>" ClientValidationFunction="funcTransaction" OnServerValidate="custvldTransaction_ServerValidate">
                        </asp:CustomValidator>
                        <asp:CustomValidator ID="custvldDocumentBalance" runat="server" Display="None" ErrorMessage="<%$ Resources:custvldDocumentBalance %>" OnServerValidate="custvldDocumentBalance_ServerValidate" ClientValidationFunction="ValidateBalanceamount">
                        </asp:CustomValidator>
                        <asp:CustomValidator ID="custvldCurrencyType" runat="server" Display="None" ErrorMessage="<%$ Resources:custvldCurrencyType %>" OnServerValidate="custvldCurrencyType_ServerValidate">
                        </asp:CustomValidator>
                        <asp:HiddenField ID="hiddenDocumentBalanceScript" runat="server" Value="<%$Resources:hiddenDocumentBalanceScript %>"></asp:HiddenField>

                        <asp:HiddenField ID="hiddenDeclineMsg" runat="server" Value="<%$Resources:DeclineMsg %>"></asp:HiddenField>
                        <asp:HiddenField ID="hiddenApproveMsg" runat="server" Value="<%$Resources:NotLastStep %>"></asp:HiddenField>
                        <asp:HiddenField ID="hdnUrl" runat="server" Value="<%$Resources:DeclineMsg %>"></asp:HiddenField>
                        <asp:HiddenField ID="hiddenDocumentBalance" runat="server" Value="0"></asp:HiddenField>
                        <asp:HiddenField ID="hPartyKey" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hPartyName" runat="server"></asp:HiddenField>

                        <asp:HiddenField ID="hdnmsg_AuthAlert" Value="<%$Resources:msg_AuthAlert %>" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hMultistep" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hDebtorGroup" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hdnJournalAuthoriser" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hdnApprovalAuthorityLimit" runat="server"></asp:HiddenField>
                    </div>

                    <div class="card-heading">
                        <h1>
                            <asp:Label ID="lbl_legend_transaction" runat="server" Text="<%$Resources:lbl_legend_transaction %>"></asp:Label>
                        </h1>
                    </div>
                    <div class="grid-card table-responsive">
                        <asp:GridView ID="drgManualJournal" DataKeyNames="ManualJournalKey" runat="server" AutoGenerateColumns="False" GridLines="None" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                            <Columns>
                                <asp:TemplateField ShowHeader="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblManualJournalKey" runat="server" Text='<%# Eval("ManualJournalKey") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="AccountKey" HeaderText="<%$ Resources:grdHeaderAccount %>"></asp:BoundField>
                                <asp:TemplateField HeaderText="<%$ Resources:grdHeaderCurrency %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCurrencyType" runat="server" Text='<%# Eval("CurrencyTypeDescription") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <nexus:TemplateField HeaderText="<%$ Resources:grdHeaderAmount %>" DataType="Currency">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("Amount") %>' Visible="true"></asp:Label>
                                    </ItemTemplate>
                                </nexus:TemplateField>
                                <nexus:BoundField DataField="CurrencyRate" HeaderText="<%$ Resources:grdHeaderCurrencyRate %>" DataType="Currency"></nexus:BoundField>
                                <nexus:BoundField DataField="BaseAmount" HeaderText="<%$ Resources:grdHeaderBaseAmount %>" DataType="Currency"></nexus:BoundField>
                                <asp:BoundField DataField="AltReference" HeaderText="<%$ Resources:grdHeaderAltReference %>"></asp:BoundField>
                                <asp:BoundField DataField="Comment" HeaderText="<%$ Resources:grdHeaderComment %>"></asp:BoundField>
                                <asp:BoundField DataField="UnderwritingYearDescription" HeaderText="<%$ Resources:grdHeaderUnderwritingYear %>"></asp:BoundField>
                                <asp:BoundField DataField="CostCentreDescription" HeaderText="<%$ Resources:grdHeaderCostCentre %>"></asp:BoundField>
                                <asp:BoundField DataField="InsuranceRef" HeaderText="<%$ Resources:grdHeaderInsuranceRef %>"></asp:BoundField>
                                <asp:BoundField DataField="PurchaseOrderNumber" HeaderText="<%$ Resources:grdHeaderPONo %>"></asp:BoundField>
                                <asp:BoundField DataField="PurchaseInvoiceNumber" HeaderText="<%$ Resources:grdHeaderPOInvNo %>"></asp:BoundField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <div class="rowMenu">
                                            <ol class="list-inline no-margin">
                                                <li class="dropdown no-padding"><a href="#" title="Action Menu" md-ink-ripple="" data-bs-toggle="dropdown" class="md-btn grey-100 md-flat md-btn-circle"><i class="fa fa-ellipsis-v" aria-hidden="true"></i></a>
                                                    <ol id='menu_<%# Eval("ManualJournalKey") %>' class="dropdown-menu dropdown-menu-scale pull-right pull-up top text-color">
                                                        <li>
                                                            <asp:LinkButton ID="hypManualJournalEdit" runat="server" Text="<%$ Resources:hypManualJournalEdit %>" CausesValidation="false"></asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="hypManualJournalDelete" runat="server" Text="<%$ Resources:hypManualJournalDelete %>" CausesValidation="false" CommandName="Delete" CommandArgument='<%# Eval("ManualJournalKey") %>'></asp:LinkButton>
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
                    <asp:HiddenField ID="hiddentGridRowCount" runat="server" Value="0"></asp:HiddenField>
                    <asp:Label ID="lblDocumentBalanceLabel" AssociatedControlID="lblDocumentBalance" runat="server" Text="<%$Resources:lblDocumentBalanceLabel %>"></asp:Label>
                    <asp:Label runat="server" ID="lblDocumentBalance" Text="0.00"></asp:Label>

                </div>
            </div>
        </div>


        <div class="card-footer">
            <asp:LinkButton ID="btnApprove" runat="server" Text="<%$ Resources:lbl_Approve %>" CausesValidation="true" Visible="false" SkinID="btnPrimary"></asp:LinkButton>
            <asp:LinkButton ID="btnDecline" runat="server" Text="<%$ Resources:lbl_Decline %>" OnClientClick="javascript: return DeclineMsg();" CausesValidation="true" Visible="false" SkinID="btnPrimary"></asp:LinkButton>
            <asp:LinkButton ID="btnBack" runat="server" Text="<%$ Resources:lbl_Back %>" CausesValidation="true" Visible="false" SkinID="btnPrimary"></asp:LinkButton>
        </div>

        <nexus:ProgressIndicator ID="upManualJournal" OverlayCssClass="updating" AssociatedUpdatePanelID="up1" runat="server">
            <ProgressTemplate>
            </ProgressTemplate>
        </nexus:ProgressIndicator>
    </div>
</asp:Content>
