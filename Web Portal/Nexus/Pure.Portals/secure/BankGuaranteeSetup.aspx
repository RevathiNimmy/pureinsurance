<%@ Page Language="VB" AutoEventWireup="false" CodeFile="BankGuaranteeSetup.aspx.vb"
    Inherits="Nexus.Secure_BankGuaranteeSetup" MasterPageFile="~/default.master"
    EnableViewState="true" %>

<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/AddTaskButton.ascx" TagName="AddTask" TagPrefix="uc6" %>
<%@ Register Src="~/Controls/BranchPickList.ascx" TagName="BranchPickList" TagPrefix="Branch" %>
<%@ Register Src="~/Controls/ProductPickList.ascx" TagName="ProductPickList" TagPrefix="Product" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script language="javascript" type="text/javascript">

        function setBank(sBankShortName, sBankKey, sBankName) {
            tb_remove();
            document.getElementById('<%= txtBankName.ClientId%>').value = unescape(sBankName);;
            document.getElementById('<%= txtBankNameKey.ClientId%>').value = sBankKey;
            document.getElementById('<%= txtBankName.ClientId%>').focus();

        }


        function CloseFindAccount() {
            tb_remove();
        }
        function setLimit() {
            document.getElementById('<%= txtLimitsAvl.ClientId%>').value = document.getElementById('<%= txtAmount.ClientId%>').value;
            document.getElementById('<%= txtLimitsAvlKey.ClientId%>').value = document.getElementById('<%= txtAmount.ClientId%>').value;

        }
    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div id="secure_BankGuaranteeSetup">
        <div class="card">
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblBankGuaranteeDetails" runat="server" Text="<%$ Resources:lbl_BankGuaranteeDetails %>"></asp:Label>
                    </legend>


                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label runat="Server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtBankName" Text="<%$ Resources:btn_BankName %>" ID="lblbtnBankName"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="txtBankName" runat="server" CssClass="field-mandatory form-control"></asp:TextBox>
                                <span class="input-group-btn">
                                    <asp:LinkButton ID="btnBankName" runat="server" CausesValidation="false" OnClientClick="tb_show(null , '../Modal/FindBank.aspx?modal=true&KeepThis=true&FromPage=PC&TB_iframe=true&height=400&width=500' , null);return false;" SkinID="btnModal">
                                        <i class="fa fa-search"></i>
                                        <span class="btn-fnd-txt">Bank Name</span>
                                    </asp:LinkButton></span>
                            </div>
                        </div>
                        <asp:HiddenField ID="txtBankNameKey" runat="server"></asp:HiddenField>
                        <asp:RequiredFieldValidator ID="vldBankName" runat="server" ControlToValidate="txtBankName" Display="none" Enabled="true" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_BankName_err%>"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblBankBranch" runat="server" AssociatedControlID="txtBankBranch" Text="<%$ Resources:lbl_BankBranch %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtBankBranch" runat="server" CssClass="field-mandatory form-control"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="vldBankBranch" runat="server" ControlToValidate="txtBankBranch" Display="none" Enabled="true" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_BankBranch_err%>"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblNumber" runat="server" AssociatedControlID="txtNumber" Text="<%$ Resources:lbl_Number %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtNumber" runat="server" CssClass="field-mandatory form-control"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="vldNumber" runat="server" ControlToValidate="txtNumber" Display="none" Enabled="true" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_Number_err%>"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblBGCustodyBranch" runat="server" AssociatedControlID="ddlBGCustodyBranch" Text="<%$ Resources:lbl_BGCustodyBranch %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <NexusProvider:LookupList ID="ddlBGCustodyBranch" runat="server" ListCode="source" ListType="PMLookup" Sort="ASC" DataItemText="Description" DataItemValue="Code" CssClass="field-medium form-control form-select"></NexusProvider:LookupList>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblCurrency" runat="server" AssociatedControlID="ddlCurrency" Text="<%$ Resources:lbl_Currency %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <NexusProvider:LookupList ID="ddlCurrency" runat="server" ListCode="currency" ListType="PMLookup" Sort="ASC" DataItemText="Description" DataItemValue="Code" CssClass="field-medium form-control form-select"></NexusProvider:LookupList>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblAmount" runat="server" AssociatedControlID="txtAmount" Text="<%$ Resources:lbl_Amount %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtAmount" runat="server" CssClass="field-mandatory form-control" onBlur="setLimit()"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="vldAmount" runat="server" ControlToValidate="txtAmount" Display="none" Enabled="true" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_Amount_err%>"></asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="rngAmount" runat="server" SetFocusOnError="true" CssClass="error" ControlToValidate="txtAmount" MinimumValue="0.0" MaximumValue="9999999999999" Display="none" ErrorMessage="<%$ Resources:lblAmount_err %>"></asp:RangeValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblLimitsAvl" runat="server" AssociatedControlID="txtLimitsAvl" Text="<%$ Resources:lbl_LimitsAvl %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtLimitsAvl" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                        <asp:HiddenField ID="txtLimitsAvlKey" runat="server"></asp:HiddenField>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblIssueDate" runat="server" AssociatedControlID="txtIssueDate" Text="<%$ Resources:lbl_IssueDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="txtIssueDate" runat="server" CssClass="field-mandatory form-control"></asp:TextBox><uc1:CalendarLookup ID="calIssueDate" runat="server" LinkedControl="txtIssueDate" HLevel="1"></uc1:CalendarLookup>
                            </div>
                        </div>

                        <asp:RequiredFieldValidator ID="vldIssueDate" runat="server" ControlToValidate="txtIssueDate" Display="none" Enabled="true" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_IssueDate_err%>"></asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="rngIssueDate" runat="server" CssClass="error" ControlToValidate="txtIssueDate" ErrorMessage="<%$ Resources:lblIssueDate_err %>" Type="Date" MinimumValue="01/01/1900" MaximumValue="01/01/9999" Display="none" SetFocusOnError="true"></asp:RangeValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblExpiryDate" runat="server" AssociatedControlID="txtExpiryDate" Text="<%$ Resources:lbl_ExpiryDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="txtExpiryDate" runat="server" CssClass="field-date field-mandatory form-control"></asp:TextBox><uc1:CalendarLookup ID="calExpiryDate" runat="server" LinkedControl="txtExpiryDate" HLevel="1"></uc1:CalendarLookup>
                            </div>
                        </div>

                        <asp:RequiredFieldValidator ID="vldExpiryDate" runat="server" ControlToValidate="txtExpiryDate" Display="none" Enabled="true" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_ExpiryDate_err%>"></asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="rngExpiryDate" runat="server" CssClass="error" ControlToValidate="txtExpiryDate" ErrorMessage="<%$ Resources:lblExpiryDate_err %>" Type="Date" MinimumValue="01/01/1900" MaximumValue="01/01/9999" Display="none" SetFocusOnError="true"></asp:RangeValidator>
                        <asp:CompareValidator ID="vldCompExpiryDate" runat="server" ControlToCompare="txtIssueDate" Display="None" ErrorMessage="<%$ Resources:lbl_CompExpiryDate_err %>" ControlToValidate="txtExpiryDate" SetFocusOnError="true" Operator="GreaterThanEqual" Type="Date" Enabled="True"></asp:CompareValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblSinglePolicyLock" runat="server" AssociatedControlID="chkSinglePolicyLock" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="litSinglePolicyLock" runat="server" Text="<%$ Resources:lbl_SinglePolicyLock %>"></asp:Literal></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:CheckBox ID="chkSinglePolicyLock" runat="server" Text=" " CssClass="asp-check"></asp:CheckBox>
                        </div>
                    </div>

                    <asp:HiddenField ID="txtPartyKey" runat="server"></asp:HiddenField>
                </div>
                <Product:ProductPickList ID="pckProduct" runat="server"></Product:ProductPickList>
                <Branch:BranchPickList ID="pckBranch" runat="server"></Branch:BranchPickList>

                <div id="Polices" runat="server" visible="false" class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblPolicies" runat="server" Text="<%$ Resources:lbl_Policies %>"></asp:Label>
                    </legend>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblTotalTransactionAmount" runat="server" AssociatedControlID="txtTotalTransactionAmount" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <asp:Label ID="txtTotalTransactionAmount" runat="server" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    </div>
                </div>
                <div class="grid-card table-responsive">
                    <asp:GridView ID="grdvBankGuaranteePolicies" runat="server" DataKeyNames="BGKey" PageSize="10" PagerSettings-Mode="Numeric" GridLines="None" AutoGenerateColumns="False" AllowPaging="true" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                        <Columns>
                            <asp:BoundField DataField="ClientCode" HeaderText="<%$ Resources:lbl_ClientCode %>"></asp:BoundField>
                            <asp:BoundField DataField="ClientName" HeaderText="<%$ Resources:lbl_ClientName%>"></asp:BoundField>
                            <asp:BoundField DataField="Reference" HeaderText="<%$ Resources:lbl_InsuranceFileRef%>"></asp:BoundField>
                            <asp:BoundField DataField="AgentCode" HeaderText="<%$ Resources:lbl_AgentShortName%>"></asp:BoundField>
                            <asp:BoundField DataField="BranchDescription" HeaderText="<%$ Resources:lbl_BranchDescription %>"></asp:BoundField>
                            <asp:BoundField DataField="ProductDescription" HeaderText="<%$ Resources:lbl_ProductCode %>"></asp:BoundField>
                            <asp:BoundField DataField="PremiumDueNet" HeaderText="<%$ Resources:lbl_AnnualPremium %>" HtmlEncode="False"></asp:BoundField>
                            <asp:BoundField DataField="CoverStartDate" HeaderText="<%$ Resources:lbl_CoverStartDate %>" DataFormatString="{0:d}"></asp:BoundField>
                            <asp:BoundField DataField="CoverEndDate" HeaderText="<%$ Resources:lbl_CoverEndDate %>" DataFormatString="{0:d}"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

            <div class="card-footer">
                <asp:LinkButton ID="btnCancel" runat="server" Text="<%$ Resources:btn_Cancel %>" CausesValidation="false" PostBackUrl="../secure/BankGuarantee.aspx" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnSubmit" runat="server" Text="<%$ Resources:btn_Submit %>" SkinID="btnPrimary"></asp:LinkButton>
                <uc6:AddTask ID="btnAddTask" runat="server"></uc6:AddTask>
            </div>
        </div>
        <asp:CustomValidator ID="VldProductAndBranch" runat="server" Display="None" SetFocusOnError="true" CssClass="error" Enabled="false"></asp:CustomValidator>
        <asp:ValidationSummary ID="vldSummeryBankGuarantee" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
    </div>

</asp:Content>
