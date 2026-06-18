<%@ Page Language="VB" AutoEventWireup="false" CodeFile="OtherPartyDetails.aspx.vb" Inherits="Nexus.secure_OtherPartyDetails"
    Title="Other Party Details" MasterPageFile="~/Default.master" EnableViewState="true" %>

<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register Src="~/Controls/ProgressBar.ascx" TagName="ProgressBar" TagPrefix="uc1" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/Addresses.ascx" TagName="Addresses" TagPrefix="uc3" %>
<%@ Register Src="~/Controls/Convictions.ascx" TagName="Convictions" TagPrefix="uc4" %>
<%@ Register Src="~/Controls/BankDetails.ascx" TagName="BankDetail" TagPrefix="uc5" %>
<%@ Register Src="~/Controls/Accidents.ascx" TagName="Accidents" TagPrefix="uc6" %>
<%@ Register Src="~/Controls/BranchPickList.ascx" TagName="BranchPickList" TagPrefix="uc7" %>
<%@ Register Src="~/Controls/SupplyPickList.ascx" TagName="SupplyPickList" TagPrefix="uc8" %>
<%@ Register Src="~/Controls/SpecialityPickList.ascx" TagName="SpecialityPickList" TagPrefix="uc9" %>
<%@ Register Src="~/Controls/Contacts.ascx" TagName="Contacts" TagPrefix="uc10" %>

<asp:Content ID="cntMainBody" runat="server" ContentPlaceHolderID="cntMainBody">
    <script type="text/javascript">
        function IsAlphaNumeric(e) {
     var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
     var ret = ((keyCode >= 65 && keyCode <= 90) || (keyCode >= 97 && keyCode <= 122) || (keyCode == 44 || keyCode == 39 || keyCode == 32 || keyCode == 45));
     return ret;
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
            <asp:Panel ID="pnlRegisterOP" runat="server">
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
                                            <asp:TextBox ID="txtCode" runat="server" onkeypress="return IsAlphaNumeric(event);" MaxLength="20" onpaste="return false;" Enabled="false" CssClass="field-mandatory form-control">  </asp:TextBox>
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
                                                ListType="UserDefined" ListCode="131091" DefaultText="(Please select)" CssClass="field-medium form-control"></NexusProvider:LookupList>
                                        </div>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblLicenceType" runat="server" AssociatedControlID="Licence_Type" Text="<%$ Resources:lbl_LicenceType%>" class="col-md-4 col-sm-3 control-label" />
                                        <div class="col-md-8 col-sm-9">
                                            <NexusProvider:LookupList ID="Licence_Type" runat="server" DataItemValue="Code" DataItemText="Description"
                                                Sort="ASC" ListType="PMLookup" ListCode="License_Type" DefaultText=" " CssClass="field-medium form-control" />
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
                                                Sort="ASC" ListType="PMLookup" ListCode="Driver_Status" DefaultText="Please Select" CssClass="field-medium form-control" />
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
                                            <asp:DropDownList ID="ddlBranch" DataTextField="Description" DataValueField="Code" runat="server" AutoPostBack="true" CssClass="field-medium field-mandatory form-control"
                                                OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" >
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div id="liSubBranchCode" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblSubBranch" runat="server" AssociatedControlID="ddlSubBranch" Text="<%$ Resources:lbl_SubBranch%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <asp:DropDownList ID="ddlSubBranch" DataTextField="Description" DataValueField="Code" runat="server" CssClass="field-medium form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblCurrency" runat="server" AssociatedControlID="ddlCurrency" Text="<%$ Resources:lbl_Currency%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <asp:DropDownList ID="ddlCurrency" DataTextField="Description" DataValueField="Code" runat="server" CssClass="field-medium form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblTPASettle" runat="server" AssociatedControlID="ddlTPASettle" Text="<%$ Resources:lbl_TPASettle %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <asp:DropDownList ID="ddlTPASettle" TabIndex="13" CssClass="field-medium form-control" runat="server">
                                                <asp:ListItem Value="" Text="<%$ Resources:ddlTPASettle_ListItem1 %>"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="<%$ Resources:ddlTPASettle_ListItem2 %>"></asp:ListItem>
                                                <asp:ListItem Value="0" Text="<%$ Resources:ddlTPASettle_ListItem3 %>"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="tab-editAddresses" class="tab-pane animated fadeIn" role="tabpanel">
                                <uc3:Addresses ID="Addresses" runat="server"></uc3:Addresses>

                            </div>
                            <div id="tab-editContacts" class="tab-pane animated fadeIn" role="tabpanel">
                                <uc10:Contacts ID="Contact" runat="server"></uc10:Contacts>

                            </div>
                            <div id="tab-editSupply" class="tab-pane animated fadeIn" role="tabpanel">
                                <uc8:SupplyPickList ID="SupplyPickList" runat="server"></uc8:SupplyPickList>
                                <uc9:SpecialityPickList ID="SpecialityPickList" runat="server"></uc9:SpecialityPickList>

                            </div>
                            <div id="tab-editConvictions" class="tab-pane animated fadeIn" role="tabpanel">
                                <uc4:Convictions ID="Conviction" runat="server"></uc4:Convictions>

                            </div>
                            <div id="tab-editAccidents" class="tab-pane animated fadeIn" role="tabpanel">
                                <uc6:Accidents ID="Accidents" runat="server"></uc6:Accidents>

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
                                <uc5:BankDetail ID="BankDetail" runat="server"></uc5:BankDetail>

                            </div>
                            <div id="tab-editBranch" class="tab-pane animated fadeIn" role="tabpanel">
                                <uc7:BranchPickList ID="BranchPickList" runat="server"></uc7:BranchPickList>

                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlViewOP" CssClass="card-body clearfix" runat="server" Visible="true" EnableViewState="true">
                <div class="md-whiteframe-z0 bg-white">
                    <ul class="nav nav-lines nav-tabs b-danger">
                        <li><a href="#tab-General" data-bs-toggle="tab" aria-expanded="true" class="active">General</a></li>
                        <li><a href="#tab-Addresses" data-bs-toggle="tab" aria-expanded="true">Addresses</a></li>
                        <li><a href="#tab-Contacts" data-bs-toggle="tab" aria-expanded="true">Contacts</a></li>
                        <li><a href="#tab-Supply" data-bs-toggle="tab" aria-expanded="true">Supply</a></li>
                        <li><a href="#tab-Convictions" data-bs-toggle="tab" aria-expanded="true">Convictions</a></li>
                        <li><a href="#tab-Accidents" data-bs-toggle="tab" aria-expanded="true">Accidents</a></li>
                        <li><a href="#tab-Tax" data-bs-toggle="tab" aria-expanded="true">Tax</a></li>
                        <li><a href="#tab-Bank" data-bs-toggle="tab" aria-expanded="true">Bank</a></li>
                        <li><a href="#tab-Branch" data-bs-toggle="tab" aria-expanded="true">Branch</a></li>
                    </ul>
                    <div class="tab-content clearfix p b-t b-t-2x">
                        <div id="tab-General" class="tab-pane animated fadeIn active" role="tabpanel">
                            <div class="form-horizontal">
                                <legend>
                                    <asp:Label ID="lblClientDetails" runat="server" Text="<%$ Resources:lblClientDetails %>"></asp:Label>

                                </legend>

                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblCodeView" runat="server" AssociatedControlID="lblCode_view" Text="<%$ Resources:lbl_Code %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:Label ID="lblCode_view" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblDOBView" runat="server" AssociatedControlID="lblDOB_View" Text="<%$ Resources:lbl_DOB %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:Label ID="lblDOB_View" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblNameView" runat="server" AssociatedControlID="lblName_View" Text="<%$ Resources:lbl_Name%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:Label ID="lblName_View" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblGenderView" runat="server" AssociatedControlID="lblGender_View" Text="<%$ Resources:lbl_Gender %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:Label ID="lblGender_View" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblLicenceTypeView" runat="server" AssociatedControlID="lblLicenceType_View" Text="<%$ Resources:lbl_LicenceType%>" class="col-md-4 col-sm-3 control-label" />
                                    <div class="col-md-8 col-sm-9">
                                        <asp:Label ID="lblLicenceType_View" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblLicenceNOView" runat="server" AssociatedControlID="lblLicenceNO_View" Text="<%$ Resources:lbl_LicenceNO %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:Label ID="lblLicenceNO_View" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblStatusView" runat="server" AssociatedControlID="lblStatus_View" Text="<%$ Resources:lbl_DriverStatus%>" class="col-md-4 col-sm-3 control-label" />
                                    <div class="col-md-8 col-sm-9">
                                        <asp:Label ID="lblStatus_View" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblRegistrationNOView" runat="server" AssociatedControlID="lblRegistrationNO_View" Text="<%$ Resources:lbl_RegistrationNO %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:Label ID="lblRegistrationNO_View" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div id="Div1" runat="server" visible="false" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblBranchView" runat="server" AssociatedControlID="lblBranch_View" Text="<%$ Resources:lbl_Branch%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:Label ID="lblBranch_View" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div id="Div2" runat="server" visible="false" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblSubBranchView" runat="server" AssociatedControlID="lblSubBranch_View" Text="<%$ Resources:lbl_SubBranch%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:Label ID="lblSubBranch_View" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblCurrencyView" runat="server" AssociatedControlID="lblCurrency_View" Text="<%$ Resources:lbl_Currency%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:Label ID="lblCurrency_View" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblTPASettleView" runat="server" AssociatedControlID="lblTPASettle_View" Text="<%$ Resources:lbl_TPASettle %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:Label ID="lblTPASettle_View" runat="server"></asp:Label>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <div id="tab-Addresses" class="tab-pane animated fadeIn" role="tabpanel">
                            <uc3:Addresses ID="ViewAddressesCntrl" runat="server"></uc3:Addresses>

                        </div>
                        <div id="tab-Contacts" class="tab-pane animated fadeIn" role="tabpanel">
                            <uc10:Contacts ID="ViewContactsCntrl" runat="server"></uc10:Contacts>

                        </div>
                        <div id="tab-Supply" class="tab-pane animated fadeIn" role="tabpanel">
                            <legend><asp:Literal ID="ltSupplyPLSelectedView" runat="server" Text="<%$ Resources:lbl_ltSupplyPLSelectedView %>"></asp:Literal></legend>
                            <asp:Label ID="lblSupplyPLSelected_View" runat="server"></asp:Label>
                            <br />
                            <legend><asp:Literal ID="ltSpecialityPLSelectedView" runat="server" Text="<%$ Resources:lbl_ltSpecialityPLSelectedView %>"></asp:Literal></legend>
                            <asp:Label ID="lblSpecialityPLSelected_View" runat="server"></asp:Label>

                        </div>
                        <div id="tab-Convictions" class="tab-pane animated fadeIn" role="tabpanel">
                            <uc4:Convictions ID="ViewConvictionsCntrl" runat="server"></uc4:Convictions>

                        </div>
                        <div id="tab-Accidents" class="tab-pane animated fadeIn" role="tabpanel">
                            <uc6:Accidents ID="Accidents1" runat="server"></uc6:Accidents>
                        </div>
                        <div id="tab-Tax" class="tab-pane animated fadeIn" role="tabpanel">
                            <div class="form-horizontal">
                                <legend>
                                    <asp:Label ID="lblHeadingTax_View" runat="server" Text="<%$ Resources:lbl_HeadingTax %>"></asp:Label>
                                </legend>

                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblTaxNumberView" runat="server" AssociatedControlID="lblTaxNumber_View" Text="<%$ Resources:lbl_TaxNumber %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:Label ID="lblTaxNumber_View" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblIsDomiciledForTaxView" runat="server" AssociatedControlID="chkIsDomiciledForTax_View" Text="<%$ Resources:lbl_IsDomiciledForTax  %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:CheckBox ID="chkIsDomiciledForTax_View" runat="server" Enabled="false" Text=" " CssClass="asp-check"></asp:CheckBox>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblTaxPercentageView" AssociatedControlID="lblTaxPercentage_View" runat="server" Text="<%$ Resources:lbl_TaxPercentage %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:Label ID="lblTaxPercentage_View" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblTaxExemptView" AssociatedControlID="chkTaxExempt_View" runat="server" Text="<%$ Resources:lbl_TaxExempt %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:CheckBox ID="chkTaxExempt_View" runat="server" Text=" " Enabled="false" CssClass="asp-check"></asp:CheckBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="tab-Bank" class="tab-pane animated fadeIn" role="tabpanel">
                            <uc5:BankDetail ID="ctrlBankDetail" runat="server"></uc5:BankDetail>

                        </div>
                        <div id="tab-Branch" class="tab-pane animated fadeIn" role="tabpanel">
                            <legend><asp:Literal ID="ltBranchPLSelectedView" runat="server" Text="<%$ Resources:lbl_ltBranchPLSelectedView %>"></asp:Literal></legend>
                            <asp:Label ID="lblBranchPLSelected_View" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <div class="card-footer">
                <asp:Label runat="server" ID="lblReviewText" Text="<%$ Resources:lbl_Review %>" EnableViewState="false" CssClass="label-margin-right"></asp:Label>
                <asp:LinkButton runat="server" ID="btnCancel" Text="<%$ Resources:lbl_Cancel %>" Visible="false" OnClick="BtnCancelClientClick" SkinID="btnSecondary" CausesValidation="false" ></asp:LinkButton>
                <asp:LinkButton runat="server" ID="btnSubmit" Text="<%$ Resources:lbl_Submit %>" EnableViewState="false" SkinID="btnPrimary"></asp:LinkButton>
                <asp:LinkButton runat="server" ID="btnEditClient" Visible="false" Text="<%$ Resources:lbl_btnEditOtherParty%>" EnableViewState="false" CausesValidation="False" OnClick="BtnEditClientClick" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
        <asp:CustomValidator ID="cusvldBranch" runat="server" Display="None" OnServerValidate="cusvldBranch_ServerValidate" ErrorMessage="<%$ Resources:err_VldBranchDDL %>"></asp:CustomValidator>
        <asp:CustomValidator ID="cusvldAddress" runat="server" Display="None" OnServerValidate="cusvldAddress_ServerValidate"></asp:CustomValidator>
        <asp:CustomValidator ID="VldBranchPickList" runat="server" Display="None" SetFocusOnError="true" CssClass="error" Enabled="false"></asp:CustomValidator>
        <asp:ValidationSummary ID="ValidationSummary" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
