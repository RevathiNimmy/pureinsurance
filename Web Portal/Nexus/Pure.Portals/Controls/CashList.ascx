<%@ Control Language="VB" AutoEventWireup="false" CodeFile="CashList.ascx.vb" Inherits="Nexus.Controls_CashListtab" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register Src="~/Controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Currencies.ascx" TagName="Currencies" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<div id="Controls_CashList">
    <div class="card">
        <div class="card-heading">
            <h1>
                <asp:Label ID="lblCashListHeading" runat="server" Text="<%$ Resources:lbl_CashListHeading %>"></asp:Label>
            </h1>
        </div>
        <div class="card-body pt-2 clearfix">

            <div class="form-horizontal">


                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12" style ="display :none">
                    <asp:Label ID="lblType" runat="server" AssociatedControlID="GISLookup_Type" Text="<%$ Resources:lbl_Type %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <NexusProvider:LookupList ID="GISLookup_Type" runat="server" DataItemText="Description" DataItemValue="Code" ListCode="CashListType" ListType="PMLookup" CssClass="field-medium form-control form-select"></NexusProvider:LookupList>
                    </div>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblBankAccount" runat="server" AssociatedControlID="GISLookup_BankAccount" Text="<%$ Resources:lbl_BankAccount %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="GISLookup_BankAccount" runat="server" AutoPostBack="true" CssClass="field-medium form-control form-select"></asp:DropDownList></div>
                </div>
				 <asp:RequiredFieldValidator ID="rqdBankAccount" runat="server" CssClass="error" ControlToValidate="GISLookup_BankAccount" ErrorMessage="<%$Resources:lbl_BankAccountError %>" Display="none" SetFocusOnError="true" ValidationGroup="grpvalcashlist"></asp:RequiredFieldValidator>

                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblCurrency" runat="server" AssociatedControlID="CashList_Currencies" Text="<%$ Resources:lbl_Currency %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="CashList_Currencies" runat="server" CssClass="field-medium form-control form-select"></asp:DropDownList></div>
                    <%-- <uc1:Currencies ID="CashList_Currencies" runat="server" FilterByBranch="true" CssClass="field-medium" />--%>
                    <asp:CustomValidator runat="server" ID="cvCurrency" Display="none" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_CurrencyError %>"  ValidationGroup="grpvalcashlist"></asp:CustomValidator>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblDate" runat="server" AssociatedControlID="Cash_List__Date" Text="<%$ Resources:lbl_CashListDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <div class="input-group">
                            <asp:TextBox ID="Cash_List__Date" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="CashListDate_uctCalendarLookup" runat="server" LinkedControl="" HLevel="1" ></uc1:CalendarLookup>
                        <asp:RequiredFieldValidator ID="rqdCashListDate" runat="server" CssClass="error"
                            ControlToValidate="Cash_List__Date" ErrorMessage="<%$Resources:lbl_CashListDateError %>"
                            Display="none" SetFocusOnError="true"  ValidationGroup="grpvalcashlist"/>
                        <asp:CompareValidator ID="compCashListDate" runat="server" CssClass="error" ErrorMessage="<%$Resources:lbl_InvalidFormat %>"
                            ControlToValidate="Cash_List__Date" Display="none" SetFocusOnError="true" Operator="DataTypeCheck"
                            Type="Date"  ValidationGroup="grpvalcashlist"/>
                    </div>

                   
                </div>
            </div>
                <div id="liBranchCode" runat="server" visible="false" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="Cash_List_BranchCode" runat="server" AssociatedControlID="ddlBranchCode" Text="<%$ Resources:lbl_Branch %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="ddlBranchCode" AutoPostBack="true" runat="server" DataValueField="Code" DataTextField="Description" CssClass="field-medium form-control form-select"></asp:DropDownList></div>
                </div>
                <div id="liSubBranchCode" runat="server" visible="true" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="Cash_List_SubBranchCode" runat="server" AssociatedControlID="ddlSubBranchCode" Text="<%$ Resources:lbl_SubBranch %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="ddlSubBranchCode" AutoPostBack="true" runat="server" DataValueField="Code" DataTextField="Description" CssClass="field-medium form-control"></asp:DropDownList>
                        <asp:requiredfieldvalidator id="vldSubBranchCodeRequired" runat="server" display="None" controltovalidate="ddlSubBranchCode" errormessage="<%$ Resources:vldSubBranchCode %>" setfocusonerror="True" ValidationGroup="grpvalcashlist">

                        </asp:requiredfieldvalidator>
                    </div>
                </div>
            </div>
            <asp:ValidationSummary ID="ValidationSummary" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary" ValidationGroup="grpvalcashlist"></asp:ValidationSummary>
        </div>

       <%-- <div class="card-footer">
            <asp:LinkButton ID="btnCancel" runat="server" Text="<%$ Resources:lbl_Cancel %>" SkinID="btnSecondary" CausesValidation="false"></asp:LinkButton>
            <asp:LinkButton ID="btnOk" runat="server" Text="<%$ Resources:lbl_Ok %>" SkinID="btnPrimary"  ValidationGroup="grpvalcashlist"></asp:LinkButton>

        </div>--%>
    </div>
    <asp:HiddenField ID="hdnBankAccountCode" runat="server" />
</div>
