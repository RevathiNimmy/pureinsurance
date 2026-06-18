<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="FindInsuranceFile.aspx.vb" Inherits="Nexus.Claims_FindInsuranceFile"
    Title="Untitled Page" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/Controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<asp:Content ID="cntProgressBar1" ContentPlaceHolderID="cntProgressBar" runat="Server">
</asp:Content>
<asp:Content ID="cntMainBody1" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script type="text/javascript">

        $(document).ready(function () {
            //var today = new Date();
            //var dd = today.getDate();
            //var mm = today.getMonth() + 1; //January is 0!
            //var yyyy = today.getFullYear();
            var Policyno = unescape(getQuerystring('Policyno'));
            var submit = getQuerystring('submit');
            var hpageloadflag = document.getElementById('<%=hpageloadflag.ClientId %>').value;
            //if (dd < 10) { dd = '0' + dd }
            //if (mm < 10) { mm = '0' + mm }
            //if (document.getElementById('<%=Claims_FindInsuranceFile__Claim_Date.ClientId%>').value == 'DD/MM/YYYY') {
            //    document.getElementById('<%=Claims_FindInsuranceFile__Claim_Date.ClientId%>').value = dd + '/' + mm + '/' + yyyy;
            //}

            if (Policyno != '' && hpageloadflag == '0') {
                document.getElementById('ctl00_cntMainBody_hpageloadflag').value = '1';
                document.getElementById('<%=txtPolicyNumber.ClientId%>').value = Policyno;
                if (submit != "false") {
                    document.getElementById('ctl00_cntMainBody_btnFindNow').click();
                }
            }

            if (document.getElementById('<%=hvMakePolicyNumberReadOnly.ClientID%>').value != null && document.getElementById('<%=hvMakePolicyNumberReadOnly.ClientID%>').value == "1") {
                document.getElementById('<%=txtPolicyNumber.ClientID%>').readOnly = true;
                document.getElementById('<%=btnNewSearch.ClientID%>').style.display = 'none';

            }
        });

        function getQuerystring(key, default_) {

            if (default_ == null) default_ = "";
            key = key.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
            var regex = new RegExp("[\\?&]" + key + "=([^&#]*)");
            var qs = regex.exec(window.location.href);
            if (qs == null)
                return default_;
            else
                return qs[1];

        }

        function isInteger(e) {
            var key = window.event ? e.keyCode : e.which;
            var keychar = String.fromCharCode(key);
            reg = /\d/;
            return reg.test(keychar);

        }

        function setClient(sClientName, sClientKey) {
            tb_remove();
            document.getElementById('<%=txtShortName.ClientId %>').value = unescape(sClientName);
            document.getElementById('<%=txtShortName.ClientId %>').focus();
        }

    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div id="Claims_FindInsuranceFile">

        <asp:Panel ID="PnlFindInsuranceGeneral" runat="server" DefaultButton="btnFindNow" CssClass="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblTitle" runat="server" Text="<%$ Resources:lbl_Title %>"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="ltGeneralHeading" runat="server" Text="<%$ Resources:lbl_GeneralHeading %>"></asp:Label></legend>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPolicyNumber" runat="server" AssociatedControlID="txtPolicyNumber" Text="<%$ Resources:lbl_PolicyNumber %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtPolicyNumber" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <asp:HiddenField ID="hvMakePolicyNumberReadOnly" runat="server"></asp:HiddenField>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblRiskIndex" runat="server" AssociatedControlID="txtRiskIndex" Text="<%$ Resources:lbl_RiskIndex %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtRiskIndex" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div id="liCoverNote" runat="server" visible="true" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblCoverNote" runat="server" AssociatedControlID="txtCoverNote" Text="<%$ Resources:lbl_CoverNote %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtCoverNote" runat="server" CssClass="form-control" MaxLength="9"></asp:TextBox>
                        </div>
                        <asp:CompareValidator ID="cmpCoverNoteSheet" runat="server" Type="Integer" Operator="DataTypeCheck" SetFocusOnError="True" Display="none" ControlToValidate="txtCoverNote" ErrorMessage="<%$ Resources:Invalid_CoverNoteSheet_err %>"></asp:CompareValidator>
                    </div>
                    <div id="liClaimDate" runat="server" visible="true" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblClaimDate" runat="server" AssociatedControlID="Claims_FindInsuranceFile__Claim_Date" Text="<%$ Resources:lbl_ClaimDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="Claims_FindInsuranceFile__Claim_Date" runat="server" CssClass="field-mandatory form-control"></asp:TextBox>
                                <uc1:CalendarLookup ID="ClaimDate_CalendarLookup" runat="server" LinkedControl="Claims_FindInsuranceFile__Claim_Date" HLevel="3"></uc1:CalendarLookup>
                            </div>
                        </div>
                        <asp:RegularExpressionValidator ID="regexVldClaimDate" runat="server" ValidationExpression="(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[012])/\d{4}" ControlToValidate="Claims_FindInsuranceFile__Claim_Date" ErrorMessage="<%$ Resources:lbl_LossDate_Regx_Val %>" Display="None"></asp:RegularExpressionValidator>

                        <asp:RequiredFieldValidator ID="rqdClaimDate" runat="server" ControlToValidate="Claims_FindInsuranceFile__Claim_Date" SetFocusOnError="true" Display="None" ErrorMessage="<%$ Resources:lbl_ClaimDate_err %>"></asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="rngClaimDate" runat="Server" Type="Date" Display="none" ControlToValidate="Claims_FindInsuranceFile__Claim_Date" SetFocusOnError="True" MinimumValue="01/01/1900" ErrorMessage="<%$ Resources:lbl_ClaimDate_Range_err %>"></asp:RangeValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:UpdatePanel ID="updateShortName" runat="server" ChildrenAsTriggers="False" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblShortName" runat="server" AssociatedControlID="txtShortName" Text="<%$ Resources:btn_ShortName %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtShortName" runat="server" CssClass="form-control"></asp:TextBox>
                                        <span class="input-group-btn">
                                            <asp:LinkButton ID="btnShortName" runat="server" SkinID="btnModal"><i class="fa fa-search"></i><span class="btn-fnd-txt">Short Name</span></asp:LinkButton>
                                        </span>
                                    </div>
                                </div>
                                <asp:Button ID="btnRefreshShortName" runat="server" Text="<%$ Resources:btn_RefreshShortName %>" Style="display: none" CausesValidation="False"></asp:Button>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnRefreshShortName" EventName="Click"></asp:AsyncPostBackTrigger>
                            </Triggers>
                        </asp:UpdatePanel>
                        <Nexus:ProgressIndicator ID="upShortName" OverlayCssClass="updating" AssociatedUpdatePanelID="updateShortName" runat="server">
                            <progresstemplate>
                                </progresstemplate>
                        </Nexus:ProgressIndicator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPostCode" runat="server" AssociatedControlID="txtPostCode" Text="<%$ Resources:lbl_PostCode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtPostCode" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div id="liForcedFrom" runat="server" visible="true" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblForceFrom" runat="server" AssociatedControlID="Claims_FindInsuranceFile__ForceFrom" Text="<%$ Resources:lbl_ForceFrom %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="Claims_FindInsuranceFile__ForceFrom" runat="server" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="ForceFrom_CalendarLookup" runat="server" LinkedControl="Claims_FindInsuranceFile__ForceFrom" HLevel="3"></uc1:CalendarLookup>
                            </div>
                        </div>

                    </div>
                    <div id="liForcedTo" runat="server" visible="true" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblForceTo" runat="server" AssociatedControlID="Claims_FindInsuranceFile_ForceTo" Text="<%$ Resources:lbl_ForceTo %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="Claims_FindInsuranceFile_ForceTo" runat="server" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="ForceTo_CalendarLookup" runat="server" LinkedControl="Claims_FindInsuranceFile_ForceTo" HLevel="3"></uc1:CalendarLookup>
                            </div>
                        </div>

                        <asp:CustomValidator ID="custVldForceDate" runat="server" Display="none"></asp:CustomValidator>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnNewSearch" runat="server" Text="<%$Resources:btn_NewSearch %>" CausesValidation="false" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnFindNow" runat="server" Text="<%$ Resources:btn_FindNow %>" SkinID="btnPrimary"></asp:LinkButton>

            </div>
        </asp:Panel>

        <Nexus:WildCardValidator ID="vldWildCard" AllowWildCardAtEndErrorMessage="<%$ Resources:lbl_NoWildCardAtEnd_Error %>" NoWildCardErrorMessage="<%$ Resources:lbl_NoWildCard_Error %>" ControlsToValidate="txtPolicyNumber,txtShortName,txtPostCode" Condition="Auto" Display="none" runat="server" EnableClientScript="true">
        </Nexus:WildCardValidator>
        <asp:CustomValidator ID="vldRiskIndex" runat="server" CssClass="error" ErrorMessage="<%$ Resources:lbl_InValidRiskIndex_Error %>" Display="none"></asp:CustomValidator>
        <asp:CustomValidator ID="IsValidDate" runat="server" CssClass="error" ErrorMessage="<%$ Resources:lbl_ValidDate_error %>" Display="none"></asp:CustomValidator>
        <asp:UpdatePanel runat="server" ID="Upd_Validator" UpdateMode="Always" ChildrenAsTriggers="true">
            <ContentTemplate>
                <asp:CustomValidator runat="server" ID="IsPolicyExist" CssClass="error" ErrorMessage="<%$ Resources:lbl_PolicyExist_error %>" Display="none"></asp:CustomValidator>
                <asp:CustomValidator ID="IsPolicyCancelled" runat="server" CssClass="error" ErrorMessage="<%$ Resources:lbl_PolicyCalcel_Error %>" Display="none"></asp:CustomValidator>
                <asp:ValidationSummary ID="vldFindInsuranceFileSummary" runat="server" DisplayMode="BulletList" ShowSummary="true" HeaderText="<%$Resources:lbl_ValidationSummary %>" CssClass="validation-summary"></asp:ValidationSummary>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel runat="server" ID="updInsuranceFile" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div class="grid-card table-responsive">
                    <asp:GridView ID="grdvInsuranceFile" runat="server" PageSize="10" AllowPaging="True" PagerSettings-Mode="Numeric" AutoGenerateColumns="False" GridLines="None" DataKeyNames="InsuranceRef,InsuranceFileKey" EmptyDataText="<%$ Resources:ErrorMessage %>" EmptyDataRowStyle-CssClass="noData" AllowSorting="true">
                        <Columns>
                            <asp:BoundField DataField="ClientShortName" HeaderText="<%$ Resources:lbl_ClientCode_g %>"></asp:BoundField>
                            <asp:BoundField DataField="ClientName" HeaderText="<%$ Resources:lbl_Name_g %>"></asp:BoundField>
                            <asp:BoundField DataField="ClientAddressLine1" HeaderText="<%$ Resources:lbl_AddressLine1_g %>"></asp:BoundField>
                            <asp:BoundField DataField="InsuranceRef" HeaderText="<%$ Resources:lbl_PolicyReference_g %>"></asp:BoundField>
                            <asp:BoundField DataField="RiskIndex" HeaderText="<%$ Resources:lbl_RiskIndex_g %>"></asp:BoundField>
                            <asp:BoundField DataField="ProductCode" HeaderText="<%$ Resources:lbl_ProductCode_g %>"></asp:BoundField>
                            <asp:BoundField DataField="RenewalDate" HeaderText="<%$ Resources:lbl_RenewalDate_g %>" DataFormatString="{0:d}" ItemStyle-CssClass="span-4" HtmlEncode="false" SortExpression="RenewalDate"></asp:BoundField>
                            <asp:BoundField DataField="PostCode" HeaderText="<%$ Resources:lbl_PostCode_g %>"></asp:BoundField>
                                      <asp:BoundField DataField="FileCode" HeaderText="<%$ Resources:lbl_FileCode_g %>" />
                                      <asp:BoundField DataField="DOBirth" HeaderText="<%$ Resources:lbl_DOBirth_g %>"
                                        DataFormatString="{0:d}" ItemStyle-CssClass="span-4" HtmlEncode="false" />
                            <asp:TemplateField HeaderText="<%$ Resources:lbl_AssociatedClients %>" SortExpression="AssociatedClients"
                                ItemStyle-CssClass="span-4">
                                        <ItemTemplate>
                                            <asp:Repeater ID="rptrAssociateClient" runat="server">
                                                <ItemTemplate>
                                            <asp:Label ID="lblAssociateClientName" runat="server" Text='<%#CType(Container.DataItem, System.Xml.XmlElement).GetAttribute("Name")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                                    </br></HeaderTemplate>
                                        <SeparatorTemplate>
                                                    </br>
                                        </SeparatorTemplate>
                                    </asp:Repeater>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <div class="rowMenu">
                                        <ol id="menu_<%# Eval("InsuranceFileKey") %>" class="list-inline no-margin">
                                            <li>
                                                <asp:LinkButton ID="lnkDetails" runat="server" CommandName="Select" Text="<%$ Resources:lbl_Select_g %>" SkinID="btnGrid"></asp:LinkButton></li>
                                        </ol>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="grdvInsuranceFile" EventName="DataBound"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="grdvInsuranceFile" EventName="PageIndexChanging"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="grdvInsuranceFile" EventName="RowDataBound"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="grdvInsuranceFile" EventName="RowCommand"></asp:AsyncPostBackTrigger>
            </Triggers>
        </asp:UpdatePanel>
        <Nexus:ProgressIndicator ID="upInsuranceFile" OverlayCssClass="updating" AssociatedUpdatePanelID="updInsuranceFile" runat="server">
            <progresstemplate>
            </progresstemplate>
        </Nexus:ProgressIndicator>

        <asp:HiddenField ID="hpageloadflag" runat="server" Value="0"></asp:HiddenField>

    </div>
</asp:Content>
