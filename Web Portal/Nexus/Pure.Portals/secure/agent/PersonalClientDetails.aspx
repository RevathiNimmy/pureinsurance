<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PersonalClientDetails.aspx.vb"
    Inherits="Nexus.secure_PersonalClientDetails" Title="Personal Client Details"
    MasterPageFile="~/default.master" EnableViewState="true" %>

<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register Src="~/Controls/ProgressBar.ascx" TagName="ProgressBar" TagPrefix="uc3" %>
<%@ Register Src="~/Controls/NewQuote.ascx" TagName="NewQuote" TagPrefix="uc6" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<%@ Register Src="~/controls/AddressCntrl.ascx" TagName="AddressCntrl" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/Addresses.ascx" TagName="Addresses" TagPrefix="uc7" %>

<%@ Register Src="~/Controls/Convictions.ascx" TagName="Convictions" TagPrefix="uc7" %>
<%@ Register Src="~/Controls/Associates.ascx" TagName="Associates" TagPrefix="uc7" %>
<%@ Register Src="~/Controls/LifeStyles.ascx" TagName="LifeStyles" TagPrefix="uc7" %>
<%@ Register Src="~/Controls/Loyalty.ascx" TagName="Loyalty" TagPrefix="uc7" %>
<%@ Register Src="~/Controls/ProspectPolicy.ascx" TagName="ProspectPolicy" TagPrefix="uc7" %>
<%@ Register Src="~/Controls/ClientClaims.ascx" TagName="ClientClaims" TagPrefix="uc8" %>
<%@ Register Src="~/Controls/EventList.ascx" TagName="ClientEvents" TagPrefix="uc9" %>
<%@ Register Src="~/Controls/ClientAccounts.ascx" TagName="ClientAccounts" TagPrefix="uc10" %>
<%@ Register Src="~/Controls/Contacts.ascx" TagName="Contacts" TagPrefix="uc11" %>
<%@ Register Src="~/Controls/ClientSummary.ascx" TagName="ClientSummary" TagPrefix="uc12" %>
<%@ Register Src="~/Controls/BankDetails.ascx" TagName="BankDetail" TagPrefix="uc13" %>
<%@ Register Src="~/Controls/NewQuoteImproved.ascx" TagName="NewQuoteImproved" TagPrefix="uc14" %>
<%@ Register Src="~/Controls/PolicyManager.ascx" TagName="PolicyManager" TagPrefix="uc15" %>
<%@ Register Src="~/Controls/CtrlLetterWriting.ascx" TagName="LetterWriting" TagPrefix="uc16" %>
<%@ Register Src="~/Controls/CtrlDashboard.ascx" TagName="Dashboard" TagPrefix="uc17" %>

<asp:Content ID="cntMainBody" runat="server" ContentPlaceHolderID="cntMainBody">
    <script type="text/javascript">
        function IsAlphaNumeric(e) {
            var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
            var ret = ((keyCode >= 65 && keyCode <= 90) || (keyCode >= 97 && keyCode <= 122) || (keyCode == 44 || keyCode == 39 || keyCode == 32 || keyCode == 45));
            return ret;
        }

        $(document).ready(function () {
            if ($("#<%=hdnManualTransfer.ClientID%>").val() == "NB") {
                $("a[href='#tab-policies']").attr("disabled", "disabled");
                $("a[href='#tab-claims']").attr("disabled", "disabled");
                $("a[href='#tab-policies']").off('click');
                $("a[href='#tab-claims']").off('click');
            }
            else {
                $("a[href='#tab-policies']").removeAttr("disabled");
                $("a[href='#tab-claims']").removeAttr("disabled");
            }
            var currentURL = location.toString();
            if (currentURL.indexOf("#tab-policies") !== -1) {
                var tabLink = $('.nav-tabs a[href="#tab-policies"]');
                if (tabLink.length) {
                    tabLink.tab('show');
                }
            }

            if (new URLSearchParams(window.location.search).get("mode") === "add") {
                // Find the dashboard control by ID pattern and disable it
                $('[id*="ctrlDashboard"]').prop('disabled', true).css({
                'pointer-events': 'none',
                'opacity': '0.5'
            }).on('click', function (e) { e.preventDefault(); });
            }
        });
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="personal-client-details">
        <asp:HiddenField ID="hdnManualTransfer" runat="server"></asp:HiddenField>
        <uc3:ProgressBar ID="ucProgressBar" runat="server"></uc3:ProgressBar>
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Label ID="lblNewPCClient" runat="server" Text="<%$ Resources:lbl_AddPersonalClient_header %>" EnableViewState="false"></asp:Label>
                    <asp:Label ID="lblEditClient" runat="server" Visible="false" Text="<%$ Resources:lbl_EditClient_header %>" EnableViewState="false"></asp:Label>
                    <asp:Label ID="lblViewClient" runat="server" Visible="false" Text="<%$ Resources:lbl_ViewClient_header %>" EnableViewState="false"></asp:Label>
                </h1>
            </div>
            <asp:Panel ID="PnlRegisterPC" runat="server">
                <asp:Panel ID="pnlAddCC" runat="server" Visible="false" EnableViewState="false" CssClass="pnlClient">
                    <div class="card-divider">
                        <h5>
                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lbl_AddCClient_message %>"></asp:Label>
                            <asp:HyperLink ID="hypAddCorporateClient" runat="server" CssClass="font-bold text-dark-dk text-u-l" NavigateUrl="~/secure/agent/CorporateClientDetails.aspx?mode=add" Text="<%$ Resources:lbl_AddCClient_text %>"></asp:HyperLink>
                        </h5>
                    </div>
                </asp:Panel>
                <div id="personalclient-control" class="card-body clearfix">
                    <div class="md-whiteframe-z0 bg-white">
                        <ul class="nav nav-lines nav-tabs b-danger">
                            <li><a href="#tab-basicdetails" data-bs-toggle="tab" aria-expanded="true" class="active">Basic Details</a></li>
                            <li><a href="#tab-editaddresses" data-bs-toggle="tab" aria-expanded="true">Addresses</a></li>
                            <li><a href="#tab-editcontacts" data-bs-toggle="tab" aria-expanded="true">Contacts</a></li>
                            <li><a href="#tab-editconvictions" data-bs-toggle="tab" aria-expanded="true">Convictions</a></li>
                            <li><a href="#tab-editassociates" data-bs-toggle="tab" aria-expanded="true">Associates</a></li>
                            <li><a href="#tab-editlifestyles" data-bs-toggle="tab" aria-expanded="true">Lifestyles</a></li>
                            <li><a href="#tab-editloyalty" data-bs-toggle="tab" aria-expanded="true">Loyalty</a></li>
                            <li><a href="#tab-editprospectpolicy" data-bs-toggle="tab" aria-expanded="true">Prospect</a></li>
                            <li><a href="#tab-editbank" data-bs-toggle="tab" aria-expanded="true">Bank Details</a></li>
                        </ul>
                        <div class="tab-content clearfix p b-t b-t-2x">

                            <div id="tab-basicdetails" class="tab-pane animated fadeIn active" role="tabpanel">
                                <div class="form-horizontal">
                                    <legend>
                                        <asp:Label ID="lblHeading" runat="server" Text="<%$ Resources:lbl_Heading %>"></asp:Label></legend>

                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblTitle" runat="server" AssociatedControlID="ddlTitle" Text="<%$ Resources:lbl_Title %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <NexusProvider:LookupList ID="ddlTitle" runat="server" DataItemValue="description" DataItemText="description" ListType="UserDefined" ListCode="131085" DefaultText="Please Select" CssClass="field-medium field-mandatory form-control"></NexusProvider:LookupList>
                                        </div>
                                        <asp:RequiredFieldValidator ID="vldTitleRequired" runat="server" Display="none" ControlToValidate="ddlTitle" ErrorMessage="<%$ Resources:lbl_Title %>" InitialValue="" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblGender" runat="server" AssociatedControlID="ddlGender" Text="<%$ Resources:lbl_Gender %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <NexusProvider:LookupList ID="ddlGender" runat="server" DataItemValue="description" DataItemText="description" ListType="UserDefined" ListCode="131091" DefaultText="(Please select)" CssClass="field-medium form-control form-select"></NexusProvider:LookupList>
                                        </div>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblFirstName" runat="server" AssociatedControlID="txtFirstName" Text="<%$ Resources:lbl_FirstName%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <asp:TextBox ID="txtFirstName" runat="server" onkeypress="return IsAlphaNumeric(event);" onpaste="return false;" CssClass="field-mandatory form-control"></asp:TextBox>
                                        </div>
                                        <asp:RequiredFieldValidator ID="vldFirstNameRequired" runat="server" Display="none" ControlToValidate="txtFirstName" ErrorMessage="<%$ Resources:lbl_FirstName %>" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegexForename" runat="server" Display="none" ControlToValidate="txtFirstName" ErrorMessage="<%$ Resources:lbl_RegexFirstName %>" ValidationExpression="[^{}:~#%*:<>?'’^/\\|\u2022,\u2023,\u25E6,\u2043,\u2219]+" SetFocusOnError="True"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblLastname" runat="server" AssociatedControlID="txtLastname" Text="<%$ Resources:lbl_Lastname %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <asp:TextBox ID="txtLastname" runat="server" onkeypress="return IsAlphaNumeric(event);" onpaste="return false;" CssClass="field-mandatory form-control"></asp:TextBox>
                                        </div>
                                        <asp:RequiredFieldValidator ID="vldLastnameRequired" runat="server" Display="none" ControlToValidate="txtLastname" ErrorMessage="<%$ Resources:lbl_Lastname %>" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegexLastName" runat="server" Display="none" ControlToValidate="txtLastname" ErrorMessage="<%$ Resources:lbl_RegexLastName %>" SetFocusOnError="True" ValidationExpression="[^{}:~#%*:<>?'’^/\\|\u2022,\u2023,\u25E6,\u2043,\u2219]+"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="AlternateListItem form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblInitials" runat="server" AssociatedControlID="txtInitials" Text="<%$ Resources:lbl_Initials %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <asp:TextBox ID="txtInitials" runat="server" onkeypress="return IsAlphaNumeric(event);" onpaste="return false;" CssClass="field-mandatory form-control"></asp:TextBox>
                                        </div>
                                        <asp:RequiredFieldValidator ID="vldInitialsRequired" runat="server" Display="none" ControlToValidate="txtInitials" ErrorMessage="<%$ Resources:lbl_Initials %>" InitialValue="" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblDateOfBirth" runat="server" AssociatedControlID="txtDOB" Text="<%$ Resources:lbl_DOB %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <asp:Label ID="lblDateOfBirthRequired" runat="server" AssociatedControlID="txtDOB" Text="<%$ Resources:lbl_DOB %>" Visible="false" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <div class="input-group">
                                                <asp:TextBox ID="txtDOB" runat="server" CssClass="field-date form-control"></asp:TextBox><uc1:CalendarLookup ID="calDateOfBirth" runat="server" LinkedControl="txtDOB" HLevel="2"></uc1:CalendarLookup>
                                            </div>
                                        </div>

                                        <asp:CustomValidator ID="custrngvldDateOfBirth" runat="server" Display="None" ErrorMessage="<%$ Resources:lbl_InvalidDOB %>" OnServerValidate="custrngvldDateOfBirth_ServerValidate"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cusvldAddress" runat="server" Display="None" OnServerValidate="cusvldAddress_ServerValidate"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cusvlPreferredCorrespondence" runat="server" Display="None" OnServerValidate="cusvlPreferredCorrespondence_ServerValidate"></asp:CustomValidator>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblNationality" runat="server" AssociatedControlID="GISCorporate_Nationality" Text="<%$ Resources:lbl_Nationality%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <NexusProvider:LookupList ID="GISCorporate_Nationality" runat="server" DataItemValue="Code" DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="Nationality" DefaultText="(Please Select)" CssClass="field-medium form-control form-select"></NexusProvider:LookupList>
                                        </div>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblServiceLevel" runat="server" AssociatedControlID="ddlServiceLevel" Text="<%$ Resources:lbl_ServiceLevel%>" class="col-md-4 col-sm-3 control-label" />
                                        <div class="col-md-8 col-sm-9">
                                            <NexusProvider:LookupList ID="ddlServiceLevel" runat="server" DataItemValue="Code" DataItemText="Description"
                                                Sort="ASC" ListType="PMLookup" ListCode="Service_Level" DefaultText=" " CssClass="field-medium form-control" />
                                        </div>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblTaxRegistrationNO" runat="server" AssociatedControlID="txtTaxRegistrationNO" Text="<%$ Resources:lbl_TaxRegistrationNO %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <asp:TextBox ID="txtTaxRegistrationNO" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div id="liFileCode" runat="server" visible="false" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblFileCode" runat="server" AssociatedControlID="txtFileCode" Text="<%$ Resources:lbl_FileCode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <asp:TextBox ID="txtFileCode" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblMaritalStatus" runat="server" AssociatedControlID="ddlMaritalStatus" Text="<%$ Resources:lbl_MaritalStatus %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <NexusProvider:LookupList ID="ddlMaritalStatus" runat="server" DataItemValue="description" DataItemText="description" ListType="UserDefined" ListCode="131107" CssClass="field-medium form-control form-select"></NexusProvider:LookupList>
                                        </div>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblPrimaryOccupation" runat="server" AssociatedControlID="ddlPrimaryOccupation" Text="<%$ Resources:lbl_PrimaryOccupation %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <NexusProvider:LookupList ID="ddlPrimaryOccupation" runat="server" DataItemValue="description" DataItemText="description" ListType="UserDefined" ListCode="2228226" DefaultText="(Please select)" CssClass="field-medium form-control form-select"></NexusProvider:LookupList>
                                        </div>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblSecondaryOccupation" runat="server" AssociatedControlID="ddlSecondaryOccupation" Text="<%$ Resources:lbl_SecondaryOccupation %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <NexusProvider:LookupList ID="ddlSecondaryOccupation" runat="server" DataItemValue="description" DataItemText="description" ListType="UserDefined" ListCode="2228226" DefaultText="(Please select)" CssClass="field-medium form-control form-select"></NexusProvider:LookupList>
                                        </div>
                                    </div>
                                    <div id="liBranchCode" runat="server" visible="false" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblSelectBranch" runat="server" AssociatedControlID="ddlBranchCode" Text="<%$ Resources:lbl_Branch%>" class="col-md-4 col-sm-3 control-label"> </asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <asp:DropDownList ID="ddlBranchCode" DataTextField="Description" DataValueField="Code" runat="server" CssClass="field-medium form-control form-select" OnSelectedIndexChanged="ddlBranchCode_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="liCurrency" runat="server" AssociatedControlID="ddlCurrency" Text="<%$ Resources:lbl_Currency%>" class="col-md-4 col-sm-3 control-label"> </asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <asp:DropDownList ID="ddlCurrency" DataTextField="Description" DataValueField="Code" runat="server" CssClass="field-medium form-control form-select"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblBlacklistingReason" runat="server" AssociatedControlID="ddlBlacklistingReason" Text="Blacklisting Reason" class="col-md-4 col-sm-3 control-label"/>
                                        <div class="col-md-8 col-sm-9">
                                            <NexusProvider:LookupList ID="ddlBlacklistingReason" runat="server" DataItemValue="Code" DataItemText="Description" 
                                            Sort="ASC" ListType="PMLookup" ListCode="Blacklist_Reason" DefaultText=" " CssClass="form-control" />
                                        </div>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblRenewalStopCode" runat="server" AssociatedControlID="ddlRenewalStopCode" Text="Renewal Stop Code" class="col-md-4 col-sm-3 control-label"/>
                                        <div class="col-md-8 col-sm-9">
                                            <NexusProvider:LookupList ID="ddlRenewalStopCode" runat="server" DataItemValue="Code" DataItemText="Description" 
                                            Sort="ASC" ListType="PMLookup" ListCode="Renewal_Stop_Code" DefaultText=" " CssClass="form-control" />
                                        </div>
                                    </div>

                                </div>
                            </div>
                            <div id="tab-editaddresses" class="tab-pane animated fadeIn" role="tabpanel">
                                <uc7:Addresses ID="Addresses" runat="server"></uc7:Addresses>


                            </div>
                            <div id="tab-editcontacts" class="tab-pane animated fadeIn" role="tabpanel">
                                <uc11:Contacts ID="Contact" runat="server"></uc11:Contacts>


                            </div>
                            <div id="tab-editconvictions" class="tab-pane animated fadeIn" role="tabpanel">
                                <uc7:Convictions ID="Conviction" runat="server"></uc7:Convictions>


                            </div>
                            <div id="tab-editassociates" class="tab-pane animated fadeIn" role="tabpanel">
                                <uc7:Associates ID="Associate" runat="server"></uc7:Associates>


                            </div>
                            <div id="tab-editlifestyles" class="tab-pane animated fadeIn" role="tabpanel">
                                <uc7:LifeStyles ID="LifeStyle" runat="server"></uc7:LifeStyles>


                            </div>
                            <div id="tab-editloyalty" class="tab-pane animated fadeIn" role="tabpanel">
                                <uc7:Loyalty ID="Loyalty" runat="server"></uc7:Loyalty>


                            </div>
                            <div id="tab-editprospectpolicy" class="tab-pane animated fadeIn" role="tabpanel">
                                <uc7:ProspectPolicy ID="ProspectPolicy" runat="server"></uc7:ProspectPolicy>


                            </div>
                            <div id="tab-editbank" class="tab-pane animated fadeIn" role="tabpanel">
                                <uc13:BankDetail ID="BankDetail" runat="server"></uc13:BankDetail>


                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlViewPC" CssClass="card-body clearfix" runat="server" Visible="true" EnableViewState="true">
                <div class="md-whiteframe-z0 bg-white">
                    <ul class="nav nav-lines nav-tabs b-danger">
                        
                        <li><a href="#tab-overview" data-bs-toggle="tab" aria-expanded="true" class="active">Overview</a></li>

                        <li><a href="#tab-addresses" data-bs-toggle="tab" aria-expanded="true">Addresses</a></li>
                        <li><a href="#tab-contacts" data-bs-toggle="tab" aria-expanded="true">Contacts</a></li>
                        <li><a href="#tab-convictions" data-bs-toggle="tab" aria-expanded="true">Convictions</a></li>
                        <li><a href="#tab-associates" data-bs-toggle="tab" aria-expanded="true">Associates</a></li>
                        <li><a href="#tab-lifestyles" data-bs-toggle="tab" aria-expanded="true">Lifestyles</a></li>
                        <li><a href="#tab-loyalty" data-bs-toggle="tab" aria-expanded="true">Loyalty</a></li>
                        <li><a href="#tab-prospectpolicy" data-bs-toggle="tab" aria-expanded="true">Prospect</a></li>
                        <li><a href="#tab-policies" data-bs-toggle="tab" aria-expanded="true">Quotes/Policies</a></li>
                        <li><a href="#tab-claims" data-bs-toggle="tab" aria-expanded="true">Claims</a></li>
                        <li><a href="#tab-events" data-bs-toggle="tab" aria-expanded="true">Events</a></li>
                        <% With CType(Session.Item("AGENT_DETAILS"), NexusProvider.UserDetails)
                                If String.IsNullOrEmpty(.PartyType) Then%>
                        <li><a href="#tab-accounts" data-bs-toggle="tab" aria-expanded="true">Accounts</a></li>
                        <% End If
                            End With%>
                        <li><a href="#tab-bankdetails" data-bs-toggle="tab" aria-expanded="true">Bank Details</a></li>

                    </ul>
                    <div class="tab-content clearfix p b-t b-t-2x">
                        
                        <div id="tab-overview" class="tab-pane animated fadeIn active" role="tabpanel">
                            <div class="form-horizontal">
                                <legend>
                                    <asp:Label ID="lblClientDetails" runat="server" Text="<%$ Resources:lblClientDetails %>"></asp:Label></legend>

                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblCodeTitle" AssociatedControlID="lblCode_view" runat="server" Text="<%$ Resources:lbl_ClientCode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <p class="form-control-static font-bold">
                                            <asp:Label ID="lblCode_view" runat="server"></asp:Label>
                                        </p>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblGenderTitle" AssociatedControlID="lblGender_view" runat="server" Text="<%$ Resources:lbl_Gender %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <p class="form-control-static font-bold">
                                            <asp:Label ID="lblGender_view" runat="server"></asp:Label>
                                        </p>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblNationalityTitle" AssociatedControlID="lblNationality_view" runat="server" Text="<%$ Resources:lbl_Nationality %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <p class="form-control-static font-bold">
                                            <asp:Label ID="lblNationality_view" runat="server"></asp:Label>
                                        </p>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblServiceLevelTitle" AssociatedControlID="lblServiceLevel_view" runat="server" Text="<%$ Resources:lbl_ServiceLevel %>" class="col-md-4 col-sm-3 control-label" />
                                    <div class="col-md-8 col-sm-9">
                                        <p class="form-control-static font-bold">
                                            <asp:Label ID="lblServiceLevel_view" runat="server" />
                                        </p>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblMaritalStatusTitle" AssociatedControlID="lblMaritalStatus_view" runat="server" Text="<%$ Resources:lbl_MaritalStatus %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <p class="form-control-static font-bold">
                                            <asp:Label ID="lblMaritalStatus_view" runat="server"></asp:Label>
                                        </p>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblNameTitle" AssociatedControlID="lblName_view" runat="server" Text="<%$ Resources:lbl_ClientName %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <p class="form-control-static font-bold">
                                            <asp:Label ID="lblName_view" runat="server"></asp:Label>
                                        </p>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblPrimaryOccupationTitle" AssociatedControlID="lblPrimaryOccupation_view" runat="server" Text="<%$ Resources:lbl_PrimaryOccupation %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <p class="form-control-static font-bold">
                                            <asp:Label ID="lblPrimaryOccupation_view" runat="server"></asp:Label>
                                        </p>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblSecondaryOccupationTitle" AssociatedControlID="lblSecondaryOccupation_view" runat="server" Text="<%$ Resources:lbl_SecondaryOccupation %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <p class="form-control-static font-bold">
                                            <asp:Label ID="lblSecondaryOccupation_view" runat="server"></asp:Label>
                                        </p>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblDOBTitle" AssociatedControlID="lblDOB_view" runat="server" Text="<%$ Resources:lbl_DOB %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <p class="form-control-static font-bold">
                                            <asp:Label ID="lblDOB_view" runat="server"></asp:Label>
                                        </p>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblTaxRegistrationNoTitle" AssociatedControlID="lblTaxRegistrationNo_view" runat="server" Text="<%$ Resources:lbl_TaxRegistrationNO %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <p class="form-control-static font-bold">
                                            <asp:Label ID="lblTaxRegistrationNo_view" runat="server"></asp:Label>
                                        </p>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblBlacklistingReasonTitle" AssociatedControlID="lblBlacklistingReason_view" runat="server" Text="Blacklisting Reason" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <p class="form-control-static font-bold">
                                            <asp:Label ID="lblBlacklistingReason_view" runat="server"></asp:Label>
                                        </p>
                                    </div>
                                </div>
                                <div id="liFileCodeView" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblFileCodeView" AssociatedControlID="lblFileCode_view" runat="server" Text="<%$ Resources:lbl_FileCode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <p class="form-control-static font-bold">
                                            <asp:Label ID="lblFileCode_view" runat="server"></asp:Label>
                                        </p>
                                    </div>
                                </div>
                            </div>
                            <uc12:ClientSummary ID="ClientSummaryCntrl" runat="server"></uc12:ClientSummary>
                        </div>
                        <div id="tab-addresses" class="tab-pane animated fadeIn" role="tabpanel">
                            <uc7:Addresses ID="ViewAddressesCntrl" runat="server"></uc7:Addresses>


                        </div>

                        <div id="tab-contacts" class="tab-pane animated fadeIn" role="tabpanel">
                            <uc11:Contacts ID="ViewContactsCntrl" runat="server"></uc11:Contacts>


                        </div>
                        <div id="tab-associates" class="tab-pane animated fadeIn" role="tabpanel">
                            <uc7:Associates ID="ViewAssociatesCntrl" runat="server"></uc7:Associates>


                        </div>
                        <div id="tab-convictions" class="tab-pane animated fadeIn" role="tabpanel">
                            <uc7:Convictions ID="ViewConvictionsCntrl" runat="server"></uc7:Convictions>


                        </div>
                        <div id="tab-lifestyles" class="tab-pane animated fadeIn" role="tabpanel">
                            <uc7:LifeStyles ID="ViewLifeStyles" runat="server"></uc7:LifeStyles>


                        </div>
                        <div id="tab-loyalty" class="tab-pane animated fadeIn" role="tabpanel">
                            <uc7:Loyalty ID="ViewLoyalty" runat="server"></uc7:Loyalty>


                        </div>
                        <div id="tab-prospectpolicy" class="tab-pane animated fadeIn" role="tabpanel">
                            <uc7:ProspectPolicy ID="ViewProspectPolicy" runat="server"></uc7:ProspectPolicy>


                        </div>
                        <div id="tab-policies" class="tab-pane animated fadeIn" role="tabpanel">
                            <uc15:PolicyManager ID="ClientQuotesPolicies" runat="server"></uc15:PolicyManager>


                        </div>
                        <div id="tab-claims" class="tab-pane animated fadeIn" role="tabpanel">
                            <uc8:ClientClaims ID="ctrlClientClaims" runat="server"></uc8:ClientClaims>


                        </div>
                        <div id="tab-events" class="tab-pane animated fadeIn" role="tabpanel">
                            <uc9:ClientEvents ID="ctrlClientEvents" runat="server"></uc9:ClientEvents>


                        </div>
                        <div id="tab-accounts" class="tab-pane animated fadeIn" role="tabpanel">
                            <uc10:ClientAccounts ID="ctrlClientAccounts" runat="server"></uc10:ClientAccounts>


                        </div>
                        <div id="tab-bankdetails" class="tab-pane animated fadeIn" role="tabpanel">
                            <uc13:BankDetail ID="ctrlBankDetail" runat="server"></uc13:BankDetail>


                        </div>
                    </div>
                </div>
            </asp:Panel>
            <div class="card-footer">
                <uc6:NewQuote ID="ctrlNewQuote" Visible="false" runat="server"></uc6:NewQuote>
                <asp:LinkButton runat="server" ID="btnAccountStatement" Text="<%$ Resources:lbl_Statement %>" Visible="false" OnClick="BtnAccountStatementClick"   SkinID="btnPrimary"></asp:LinkButton>
                <uc14:NewQuoteImproved ID="ctrlNewQuoteImproved" runat="server"></uc14:NewQuoteImproved>
                <asp:Label runat="server" ID="lblReviewText" Text="<%$ Resources:lbl_Review %>" EnableViewState="false" CssClass="label-margin-right"></asp:Label>
                <asp:LinkButton runat="server" ID="btnCancel" Text="<%$ Resources:lbl_Cancel %>" Visible="false" OnClick="BtnCancelClientClick" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton runat="server" ID="btnSubmit" Text="<%$ Resources:lbl_Submit %>" EnableViewState="false" OnClick="BtnSubmitClientClick" SkinID="btnPrimary"></asp:LinkButton>
                <asp:LinkButton runat="server" ID="btnEditClient" Visible="false" Text="<%$ Resources:lbl_EditClient %>" EnableViewState="false" CausesValidation="False" OnClick="BtnEditClientClick" SkinID="btnPrimary"></asp:LinkButton>
                <uc17:Dashboard ID="ctrlDashboard" runat="server" CustomSkinID="btnSecondary"></uc17:Dashboard>
                <uc16:LetterWriting ID="ctrlLetterWriting" runat="server" CustomSkinID="btnPrimary"></uc16:LetterWriting>
                
                <asp:LinkButton ID="cmdExtractClientData" runat="server" SkinID="btnPrimary" Text="<%$ Resources:lbl_ExtractClientData %>" EnableViewState="false" CausesValidation="false"
                    Style="width: auto !important; float: left; margin-left: 10px;" />
            </div>
        </div>
        <asp:ValidationSummary ID="ValidationSummary" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
