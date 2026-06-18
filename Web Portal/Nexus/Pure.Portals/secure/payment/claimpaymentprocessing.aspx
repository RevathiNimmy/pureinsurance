<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="ClaimPaymentProcessing.aspx.vb" Inherits="Nexus.secure_payment_ClaimPaymentProcessing" %>

<%@ Register TagPrefix="uc1" TagName="CalendarLookup" Src="~/Controls/CalendarLookup.ascx" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script type="text/javascript">

        function setAccount(sShortCode, shiddenShortCode, sAccountName) {
            tb_remove();
            document.getElementById('<%= txtAccountCode.ClientId%>').value = unescape(sShortCode);
            document.getElementById('<%= txtAccountName.ClientId%>').value = unescape(sAccountName);
            document.getElementById('<%= hiddenAccountKey.ClientId%>').value = shiddenShortCode;
            var PaymentDate = document.getElementById('<%= txtPaymentDate.ClientId%>');
            PaymentDate.disabled = true;
            $(".hasDatepicker").datepicker("option", "disabled", true);
            var PDCal = document.getElementById('ctl00_cntMainBody_PaymentDate_CalendarLookup_imgCalendar');
            PDCal.disabled = true;
        }


        function ValidateRequiredSearch(oSrc, args) {
            var sAccountCode = document.getElementById('<%= txtAccountCode.ClientId%>');
            var sPaymentDate = document.getElementById('<%= txtPaymentDate.ClientId%>');
            var sPaymentDateTo = document.getElementById('<%= txtPaymentDateTo.ClientId%>');

            if (sAccountCode.value == '' && sPaymentDate.value == '' && sPaymentDateTo.value == '') {
                args.IsValid = false;
            }
            else {
                args.IsValid = true;
            }
        }

        function DisplayControl() {
            var sAccountCode = document.getElementById('<%= txtAccountCode.ClientId%>');
            var sPaymentDate = document.getElementById('<%= txtPaymentDate.ClientId%>');
            sPaymentDate.disabled = true;
        }

        function DisableControl() {
            var AccountCode = document.getElementById('<%= txtAccountCode.ClientId%>').value;
            var paymentdate = document.getElementById('<%= txtPaymentDate.ClientId%>').value;
            var paymentdateTo = document.getElementById('<%= txtPaymentDateTo.ClientId%>').value;

            if (AccountCode != '') {
                document.getElementById('<%= txtPaymentDate.ClientId%>').disabled = true;
                document.getElementById('<%= txtPaymentDateTo.ClientId%>').disabled = true;
                //document.getElementById('ctl00_cntMainBody_PaymentDate_CalendarLookup_imgCalendar').disabled = true;
                //document.getElementById('ctl00_cntMainBody_PaymentDateTo_CalendarLookup_imgCalendar').disabled = true;
                $(".hasDatepicker").datepicker("option", "disabled", true);
            }
            else {
                if (paymentdate != '') {
                    document.getElementById('<%= btnAccountCode.ClientId%>').disabled = true;
                    document.getElementById('<%= txtAccountCode.ClientId%>').disabled = true;
                }
                else {
                    document.getElementById('<%= btnAccountCode.ClientId%>').disabled = false;
                    document.getElementById('<%= txtAccountCode.ClientId%>').disabled = false;
                }
            }
        }

    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600">
    </asp:ScriptManager>
    <div id="secure_payment_claimpaymentprocessing">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="litClaimPaymentsHeader" runat="server" Text="<%$ Resources:litClaimPaymentsHeader%>" EnableViewState="false"></asp:Literal>
                </h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend><span>
                        <asp:Literal ID="lblSearchByAccount" runat="server" Text="<%$ Resources:lbl_SearchByAccount %>"></asp:Literal></span></legend>

                    <asp:UpdatePanel ID="updateAccount" runat="server" ChildrenAsTriggers="false" UpdateMode="conditional">
                        <ContentTemplate>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblAccountCode" runat="server" AssociatedControlID="txtAccountCode" Text="<%$ Resources:btn_AccountCode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtAccountCode" CssClass="form-control" runat="server" onblur="javascript:DisableControl()"></asp:TextBox>
                                        <span class="input-group-btn">
                                            <asp:LinkButton ID="btnAccountCode" runat="server" SkinID="btnModal" OnClientClick="tb_show(null , '../../Modal/FindAccount.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;">
                                                          <i class="fa fa-search"></i>
                                                             <span class="btn-fnd-txt">Account Code</span>  
                                            </asp:LinkButton>
                                        </span>
                                    </div>
                                </div>
                                <asp:HiddenField ID="hiddenAccountKey" runat="server"></asp:HiddenField>
                                <asp:Button ID="btnRefreshAccount" runat="server" Style="display: none" CausesValidation="False"></asp:Button>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblAccountName" runat="server" AssociatedControlID="txtAccountName" class="col-md-4 col-sm-3 control-label">
                                    <asp:Literal ID="ltAccountName" runat="server" Text="<%$ Resources:lbl_AccountName %>"></asp:Literal>
                                </asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ID="txtAccountName" CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnRefreshAccount" EventName="Click"></asp:AsyncPostBackTrigger>
                        </Triggers>
                    </asp:UpdatePanel>
                    <Nexus:ProgressIndicator ID="upAccount" OverlayCssClass="updating" AssociatedUpdatePanelID="updateAccount" runat="server">
                        <progresstemplate>
                                    </progresstemplate>
                    </Nexus:ProgressIndicator>
                </div>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend><span>
                        <asp:Literal ID="LitSearchByDate" runat="server" Text="<%$ Resources:lbl_SearchByDate %>"></asp:Literal></span></legend>

                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPaymentDate" runat="server" AssociatedControlID="txtPaymentDate" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="ltPaymentDate" runat="server" Text="<%$ Resources:lbl_PaymentDate %>"></asp:Literal>
                        </asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="txtPaymentDate" CssClass="form-control" runat="server" onblur="javascript:DisableControl()"></asp:TextBox>
                                <uc1:CalendarLookup ID="PaymentDate_CalendarLookup" runat="server" LinkedControl="txtPaymentDate" HLevel="1"></uc1:CalendarLookup>
                            </div>
                        </div>

                        <asp:RangeValidator ID="rngPaymentDate" runat="Server" Type="Date" Display="none" ControlToValidate="txtPaymentDate" SetFocusOnError="True" MinimumValue="01/01/1900" MaximumValue="01/12/9998" ErrorMessage="<%$ Resources:lbl_PaymentDate_Range_err %>" ValidationGroup="PaymentGroup"></asp:RangeValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPaymentDateTo" runat="server" AssociatedControlID="txtPaymentDateTo" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="liPaymentDateTo" runat="server" Text="<%$ Resources:lbl_PaymentDateTo %>"></asp:Literal>
                        </asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="txtPaymentDateTo" CssClass="form-control" runat="server" onblur="javascript:DisableControl()"></asp:TextBox>
                                <uc1:CalendarLookup ID="PaymentDateTo_CalendarLookup" runat="server" LinkedControl="txtPaymentDateTo" HLevel="1"></uc1:CalendarLookup>
                            </div>
                        </div>
                        <asp:RangeValidator ID="rngPaymentDateTo" runat="Server" Type="Date" Display="none" ControlToValidate="txtPaymentDateTo" SetFocusOnError="True" MinimumValue="01/01/1900" MaximumValue="01/12/9998" ErrorMessage="<%$ Resources:lbl_PaymentDateTo_Range_err %>" ValidationGroup="PaymentGroup"></asp:RangeValidator>
                    </div>

                </div>
            </div>
            <Nexus:WildCardValidator ID="vldWildCard" AllowWildCardAtEndErrorMessage="<%$ Resources:lbl_WildCardAtEnd_error %>" NoWildCardErrorMessage="<%$ Resources:lbl_NoWildCard_error %>" ControlsToValidate="txtAccountCode,txtAccountName" Condition="Auto" Display="none" runat="server" EnableClientScript="true" ValidationGroup="PaymentGroup">
            </Nexus:WildCardValidator>
            <div class="card-footer">
                <asp:LinkButton ID="btnFindNow" runat="server" TabIndex="12" Text="<%$ Resources:btn_FindNow %>" ValidationGroup="PaymentGroup" CausesValidation="true" SkinID="btnPrimary"></asp:LinkButton>
                <asp:LinkButton ID="btnNewSearch" runat="server" TabIndex="14" Text="<%$ Resources:btn_NewSearch %>" CausesValidation="false" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
        <asp:UpdatePanel ID="UpdAllocateClaimPayment" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="grid-card table-responsive">
                    <asp:GridView ID="grdUnAllocatedClaimPayment" runat="server" PageSize="10" AllowPaging="true" PagerSettings-Mode="Numeric" AutoGenerateColumns="false" GridLines="None" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>" AllowSorting="true">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkHeader" runat="server" AutoPostBack="true" CausesValidation="false" OnCheckedChanged="chkHeader_OnCheckedChanged" Text=" " CssClass="asp-check"></asp:CheckBox>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkRow" runat="server" AutoPostBack="true" CausesValidation="false" OnCheckedChanged="chkRow_OnCheckedChanged" Text=" " CssClass="asp-check"></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="AccountName" SortExpression="AccountName" HeaderText="<%$ Resources:lbl_AccountName %>"></asp:BoundField>
                            <asp:BoundField DataField="DocumentRef" SortExpression="DocumentRef" HeaderText="<%$ Resources:lbl_DocumentRef %>"></asp:BoundField>
                            <asp:BoundField DataField="ClaimNumber" SortExpression="ClaimNumber" HeaderText="<%$ Resources:lbl_ClaimNumber  %>"></asp:BoundField>
                            <asp:BoundField DataField="DateOfPayment" SortExpression="DateOfPayment" HeaderText="<%$ Resources:lbl_PaymentDate %>" DataFormatString="{0:dd/M/yyyy}"></asp:BoundField>
                            <Nexus:BoundField DataField="CurrencyAmount" SortExpression="CurrencyAmount" HeaderText="<%$ Resources:lbl_PaymentAmount %>" DataType="Currency"></Nexus:BoundField>
                            <asp:BoundField DataField="CurrencyDescription" SortExpression="CurrencyDescription" HeaderText="<%$ Resources:lbl_PaymentCurrency %>"></asp:BoundField>
                            <Nexus:BoundField DataField="AccountAmount" SortExpression="AccountAmount" HeaderText="<%$ Resources:lbl_BaseAmount %>" DataType="Currency"></Nexus:BoundField>
                            <asp:BoundField DataField="BaseCurrencyDescription" SortExpression="BaseCurrencyDescription" HeaderText="<%$ Resources:lbl_BaseCurrency %>"></asp:BoundField>
                            <asp:BoundField DataField="Status" SortExpression="Status" HeaderText="<%$ Resources:lbl_Status  %>"></asp:BoundField>
                            <asp:BoundField DataField="MediaTypeDesc" SortExpression="MediaTypeDesc" HeaderText="<%$ Resources:lbl_MediaType  %>"></asp:BoundField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <div class="rowMenu">
                                        <ol id="menu_<%# Eval("BaseClaimPaymentKey") %>" class="list-inline no-margin">
                                            <li>
                                                <asp:LinkButton Text="<%$ Resources:lbl_Pay %>" ID="lnkPay" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ClaimPaymentKey")%>' CommandName="Pay" SkinID="btnGrid"></asp:LinkButton>
                                            </li>
                                        </ol>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div class="card-footer">
                    <asp:LinkButton ID="btnSettleAll" Enabled="false" runat="server" TabIndex="16" Text="<%$ Resources:btn_SettleAll %>" ValidationGroup="SettleAll" CausesValidation="true" SkinID="btnPrimary"></asp:LinkButton>
                </div>
                <asp:CustomValidator runat="server" ID="cvSettleAll" CssClass="error" ValidationGroup="SettleAll" OnServerValidate="cvSettleAll_ServerValidate" ErrorMessage="<%$ Resources:lbl_SettleAll_error %>" Display="none"></asp:CustomValidator>
                <asp:ValidationSummary ID="vsSettleAll" runat="server" DisplayMode="BulletList" ShowSummary="true" ValidationGroup="SettleAll" HeaderText="<%$Resources:lbl_ValidationSummary %>" CssClass="validation-summary"></asp:ValidationSummary>
            </ContentTemplate>
        </asp:UpdatePanel>
        <Nexus:ProgressIndicator ID="UpAllocateClmPayment" OverlayCssClass="updating" AssociatedUpdatePanelID="UpdAllocateClaimPayment" runat="server">
            <progresstemplate>
                        </progresstemplate>
        </Nexus:ProgressIndicator>
        <asp:CustomValidator runat="server" ID="IsUnAllocatedPaymentExist" CssClass="error" ErrorMessage="<%$ Resources:lbl_UnAllocatedPayment_error %>" Display="none" ClientValidationFunction="ValidateRequiredSearch" ValidationGroup="PaymentGroup"></asp:CustomValidator><asp:ValidationSummary ID="vldClaimPaymentProcessing" runat="server" DisplayMode="BulletList" ShowSummary="true" ValidationGroup="PaymentGroup" HeaderText="<%$Resources:lbl_ValidationSummary %>" CssClass="validation-summary"></asp:ValidationSummary>

    </div>
</asp:Content>
