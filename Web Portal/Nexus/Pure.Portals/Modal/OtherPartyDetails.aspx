<%@ Page Language="VB" AutoEventWireup="false" CodeFile="OtherPartyDetails.aspx.vb" Inherits="Nexus.Modal_OtherPartyDetails"
    Title="Other Party Details" MasterPageFile="~/Default.master" EnableViewState="true" %>

<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register Src="~/Controls/ProgressBar.ascx" TagName="ProgressBar" TagPrefix="uc1" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc2" %>
<%@ Register Src="~/controls/AddressCntrl.ascx" TagName="AddressCntrl" TagPrefix="uc3" %>
<%@ Register Src="~/Controls/Addresses.ascx" TagName="Addresses" TagPrefix="uc4" %>
<%@ Register Src="~/Controls/Convictions.ascx" TagName="Convictions" TagPrefix="uc5" %>
<%@ Register Src="~/Controls/ClientAccounts.ascx" TagName="ClientAccounts" TagPrefix="uc6" %>
<%@ Register Src="~/Controls/BankDetails.ascx" TagName="BankDetail" TagPrefix="uc7" %>
<%@ Register Src="~/Controls/Accidents.ascx" TagName="Accidents" TagPrefix="uc8" %>
<%@ Register Src="~/Controls/BranchPickList.ascx" TagName="BranchPickList" TagPrefix="uc9" %>
<%@ Register Src="~/Controls/SupplyPickList.ascx" TagName="SupplyPickList" TagPrefix="uc10" %>
<%@ Register Src="~/Controls/SpecialityPickList.ascx" TagName="SpecialityPickList" TagPrefix="uc11" %>
<%@ Register Src="~/Controls/Contacts.ascx" TagName="Contacts" TagPrefix="uc12" %>

<asp:Content ID="cntMainBody" runat="server" ContentPlaceHolderID="cntMainBody">
    <script type="text/javascript">
        function IsAlphaNumeric(e) {
            var key = window.event ? e.keyCode : e.which;
            var keychar = String.fromCharCode(key);
            reg = /^[a-z0-9]+$/i;
            return reg.test(keychar);
        }
          $(document).ready(function () {
            $(".picklist table").addClass("picklist-table");
        });
        
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div id="secure_agent_OtherPartyDetails">
        <asp:HiddenField ID="hdnManualTransfer" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hdnEditMode" Value="false" runat="server"></asp:HiddenField>
        <uc1:ProgressBar ID="ucProgressBar" runat="server"></uc1:ProgressBar>
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Label ID="lblNewOtherParty" runat="server" Text="<%$ Resources:lbl_OtherParty_header %>" EnableViewState="false"></asp:Label>
                    <asp:Label ID="lblEditOtherParty" runat="server" Visible="false" Text="<%$ Resources:lbl_EditOtherParty_header %>" EnableViewState="false"></asp:Label>
                    <asp:Label ID="lblViewOtherParty" runat="server" Visible="false" Text="<%$ Resources:lbl_ViewOtherParty_header %>" EnableViewState="false"></asp:Label>
                </h1>
            </div>
            <asp:Panel ID="pnlRegisterOP" runat="server" DefaultButton="btnSubmit">
                <div id="otherParty-control" class="card-body clearfix">
                    <div class="md-whiteframe-z0 bg-white">
                        <ul class="nav nav-lines nav-tabs b-danger">
                            <li><a href="#tab-editGeneral" data-bs-toggle="tab" aria-expanded="true" class="active">General</a></li>
                            <li><a href="#tab-editAddresses" data-bs-toggle="tab" aria-expanded="true">Addresses</a></li>
                            <li><a href="#tab-editContacts" data-bs-toggle="tab" aria-expanded="true">Contacts</a></li>
                            <li><a href="#tab-editSupply" data-bs-toggle="tab" aria-expanded="true">Supply</a></li>
                            <li><a href="#tab-editConvictions" data-bs-toggle="tab" aria-expanded="true">Convictions</a></li>
                            <li><a href="#tab-editAccidents" data-bs-toggle="tab" aria-expanded="true">Accidents</a></li>
                            <li><a href="#tab-editTax" data-bs-toggle="tab" aria-expanded="true">Tax</a></li>
                            <li><a href="#tab-editBank" data-bs-toggle="tab" aria-expanded="true">Bank</a></li>
                            <li><a href="#tab-editBranch" data-bs-toggle="tab" aria-expanded="true">Branch</a></li>
                        </ul>
                        <div class="tab-content clearfix p b-t b-t-2x">

                            <div id="tab-editGeneral" class="tab-pane animated fadeIn active" role="tabpanel">
                                <div class="form-horizontal">
                                    <legend>
                                        <asp:Label ID="lblHeading" runat="server" Text="<%$ Resources:lbl_Heading %>"></asp:Label>

                                    </legend>

                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblCode" runat="server" AssociatedControlID="txtCode" Text="<%$ Resources:lbl_Code %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <asp:TextBox ID="txtCode" runat="server" onkeypress="return IsAlphaNumeric(event);" MaxLength="20" onpaste="return false;" CssClass="field-mandatory form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                        
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblDOB" runat="server" AssociatedControlID="txtDOB" Text="<%$ Resources:lbl_DOB %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <asp:Label ID="lblDOBRequired" runat="server" AssociatedControlID="txtDOB" Text="<%$ Resources:lbl_DOB %>" Visible="false" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <div class="input-group">
                                                <asp:TextBox ID="txtDOB" runat="server" CssClass="field-date form-control"></asp:TextBox>
                                                <uc2:CalendarLookup ID="calDOB" runat="server" LinkedControl="txtDOB" HLevel="2"></uc2:CalendarLookup>
                                            </div>
                                        </div>
                                        <asp:CustomValidator ID="custrngvldDOB" Enabled="true" ControlToValidate="txtDOB" runat="server" Display="None" ErrorMessage="<%$ Resources:lbl_InvalidDOB %>" OnServerValidate="custrngvldDOB_ServerValidate"></asp:CustomValidator>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblName" runat="server" AssociatedControlID="txtName" Text="<%$ Resources:lbl_Name%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <asp:TextBox ID="txtName" runat="server" onpaste="return false;" CssClass="field-mandatory form-control"></asp:TextBox>
                                        </div>
                                        <asp:RequiredFieldValidator ID="vldNameRequired" runat="server" Display="none" ControlToValidate="txtName" ErrorMessage="<%$ Resources:lbl_Name %>" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegexName" runat="server" Display="none" ControlToValidate="txtName" ErrorMessage="<%$ Resources:lbl_RegexName %>" ValidationExpression="[^{}:~#%&*:<>?^/\\|\u2022,\u2023,\u25E6,\u2043,\u2219]+" SetFocusOnError="True"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblGender" runat="server" AssociatedControlID="ddlGender" Text="<%$ Resources:lbl_Gender %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <NexusProvider:LookupList ID="ddlGender" runat="server" DataItemValue="description" DataItemText="description"
                                                ListType="UserDefined" ListCode="131091" DefaultText="(Please select)" CssClass="field-medium form-control form-select"></NexusProvider:LookupList>
                                        </div>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblLicenceType" runat="server" AssociatedControlID="Licence_Type" Text="<%$ Resources:lbl_LicenceType%>" class="col-md-4 col-sm-3 control-label" />
                                        <div class="col-md-8 col-sm-9">
                                            <NexusProvider:LookupList ID="Licence_Type" runat="server" DataItemValue="Code" DataItemText="Description"
                                                Sort="ASC" ListType="PMLookup" ListCode="License_Type" DefaultText=" " CssClass="field-medium form-control form-select" />
                                        </div>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblLicenceNO" runat="server" AssociatedControlID="txtLicenceNO" Text="<%$ Resources:lbl_LicenceNO %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <asp:TextBox ID="txtLicenceNO" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblStatus" runat="server" AssociatedControlID="ddlDriverStatus" Text="<%$ Resources:lbl_DriverStatus%>" class="col-md-4 col-sm-3 control-label" />
                                        <div class="col-md-8 col-sm-9">
                                            <NexusProvider:LookupList ID="ddlDriverStatus" runat="server" DataItemValue="Code" DataItemText="Description"
                                                Sort="ASC" ListType="PMLookup" ListCode="Driver_Status" DefaultText="Please Select" CssClass="field-medium form-control form-select" />
                                        </div>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblRegistrationNO" runat="server" AssociatedControlID="txtRegistrationNO" Text="<%$ Resources:lbl_RegistrationNO %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <asp:TextBox ID="txtRegistrationNO" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div id="liBranchCode" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblBranch" runat="server" AssociatedControlID="ddlBranch" Text="<%$ Resources:lbl_Branch%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <asp:DropDownList ID="ddlBranch" DataTextField="Description" DataValueField="Code" runat="server" CssClass="field-medium form-control form-select"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div id="liSubBranchCode" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblSubBranch" runat="server" AssociatedControlID="ddlSubBranch" Text="<%$ Resources:lbl_SubBranch%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <asp:DropDownList ID="ddlSubBranch" DataTextField="Description" DataValueField="Code" runat="server" CssClass="field-medium form-control form-select"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblCurrency" runat="server" AssociatedControlID="ddlCurrency" Text="<%$ Resources:lbl_Currency%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <asp:DropDownList ID="ddlCurrency" DataTextField="Description" DataValueField="Code" runat="server" CssClass="field-medium form-control form-select"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblTPASettle" runat="server" AssociatedControlID="ddlTPASettle" Text="<%$ Resources:lbl_TPASettle %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <asp:DropDownList ID="ddlTPASettle" TabIndex="13" CssClass="field-medium form-control form-select" runat="server">
                                                <asp:ListItem Value="" Text="<%$ Resources:ddlTPASettle_ListItem1 %>"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="<%$ Resources:ddlTPASettle_ListItem2 %>"></asp:ListItem>
                                                <asp:ListItem Value="0" Text="<%$ Resources:ddlTPASettle_ListItem3 %>"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="tab-editAddresses" class="tab-pane animated fadeIn" role="tabpanel">
                                <uc4:Addresses ID="Addresses" runat="server"></uc4:Addresses>

                            </div>
                            <div id="tab-editContacts" class="tab-pane animated fadeIn" role="tabpanel">
                                <uc12:Contacts ID="Contact" runat="server"></uc12:Contacts>

                            </div>
                            <div id="tab-editSupply" class="tab-pane animated fadeIn" role="tabpanel">
                                <uc10:SupplyPickList ID="SupplyPickList" runat="server"></uc10:SupplyPickList>
                                <uc11:SpecialityPickList ID="SpecialityPickList" runat="server"></uc11:SpecialityPickList>

                            </div>
                            <div id="tab-editConvictions" class="tab-pane animated fadeIn" role="tabpanel">
                                <uc5:Convictions ID="Conviction" runat="server"></uc5:Convictions>

                            </div>
                            <div id="tab-editAccidents" class="tab-pane animated fadeIn" role="tabpanel">
                                <uc8:Accidents ID="Accidents" runat="server"></uc8:Accidents>

                            </div>
                            <div id="tab-editTax" class="tab-pane animated fadeIn" role="tabpanel">
                                <div class="form-horizontal">
                                    <legend>
                                        <asp:Label ID="lblHeadingTax" runat="server" Text="<%$ Resources:lbl_HeadingTax %>"></asp:Label>
                                    </legend>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblTaxNumber" runat="server" AssociatedControlID="txtTaxNumber" Text="<%$ Resources:lbl_TaxNumber %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <asp:TextBox ID="txtTaxNumber" runat="server" onkeypress="return IsAlphaNumeric(event);" onpaste="return false;" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblIsDomiciledForTax" runat="server" AssociatedControlID="chkIsDomiciledForTax" class="col-md-4 col-sm-3 control-label">
                                            <asp:Literal ID="litIsDomiciledForTax" runat="server" Text="<%$ Resources:lbl_IsDomiciledForTax  %>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                                <asp:CheckBox ID="chkIsDomiciledForTax" runat="server" TabIndex="16" Text=" " CssClass="asp-check"></asp:CheckBox>
                                            </div>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblTaxPercentage" runat="server" AssociatedControlID="txtTaxPercentage" Text="<%$ Resources:lbl_TaxPercentage%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <asp:TextBox ID="txtTaxPercentage" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="none" ControlToValidate="txtTaxPercentage" ErrorMessage="<%$ Resources:lbl_RegexName %>"
                                            ValidationExpression="^100(\.0{0,2})? *%?$|^\d{1,2}(\.\d{1,2})??$" SetFocusOnError="True"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblTaxExempt" runat="server" AssociatedControlID="chkTaxExempt" Text="<%$ Resources:lbl_TaxExempt  %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <asp:CheckBox ID="chkTaxExempt" runat="server" TabIndex="17" Text=" " CssClass="asp-check"></asp:CheckBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="tab-editBank" class="tab-pane animated fadeIn" role="tabpanel">
                                <uc7:BankDetail ID="BankDetail" runat="server"></uc7:BankDetail>

                            </div>
                            <div id="tab-editBranch" class="tab-pane animated fadeIn" role="tabpanel">
                                <uc9:BranchPickList ID="BranchPickList" runat="server"></uc9:BranchPickList>

                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <div class="card-footer">
                <asp:Label runat="server" ID="lblReviewText" Text="<%$ Resources:lbl_Review %>" EnableViewState="false" CssClass="label-margin-right"></asp:Label>
                <asp:LinkButton runat="server" ID="btnCancel" Text="<%$ Resources:lbl_Cancel %>" CausesValidation="false" SkinID="btnSecondary" OnClientClick="self.parent.tb_remove(); return false;"></asp:LinkButton>
                <asp:LinkButton runat="server" ID="btnSubmit" Text="<%$ Resources:lbl_Submit %>" EnableViewState="false" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
        <asp:CustomValidator ID="cusvldBranch" runat="server" Display="None" OnServerValidate="cusvldBranch_ServerValidate" ErrorMessage="<%$ Resources:err_VldBranchDDL %>"></asp:CustomValidator>
        <asp:CustomValidator ID="cusvldAddress" runat="server" Display="None" OnServerValidate="cusvldAddress_ServerValidate"></asp:CustomValidator>
        <asp:CustomValidator ID="VldBranchPickList" runat="server" Display="None" SetFocusOnError="true" CssClass="error" Enabled="false"></asp:CustomValidator>
        <asp:ValidationSummary ID="ValidationSummary" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
