<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RenewalManager.aspx.vb"
    Inherits="Nexus.secure_agent_RenewalManager" MasterPageFile="~/Default.master"
    EnableViewState="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/Controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<asp:Content ID="cntMainBody" runat="server" ContentPlaceHolderID="cntMainBody">

    <script language="javascript" type="text/javascript">

        $(document).ready(function () {
            EnableCalender(document.getElementById('<%= chkRenewalDate.ClientID%>').value)
        });

        function EnableCalender(status) {
            var RenewalDate = document.getElementById('<%= txtRenewalDate.ClientId%>').value;
            // var chkRenewalDate = document.getElementById('<%= chkRenewalDate.ClientId%>');
            //var imgCRenewalDate = document.getElementById('ctl00_cntMainBody_RenewalDate_CalendarLookup_imgCalendar');

            if (status == true) {
                document.getElementById('<%= txtRenewalDate.ClientId%>').value = RenewalDate
                $('#<%= txtRenewalDate.ClientId %>').removeAttr('disabled');
                $('#ctl00_cntMainBody_RenewalDate_CalendarLookup_lblCalenderIcon').removeAttr("style", "pointer-events:none!important");
                $('#ctl00_cntMainBody_RenewalDate_CalendarLookup_lblCalenderIcon').removeAttr("readonly", true);
            }
            else {
                document.getElementById('<%= txtRenewalDate.ClientId%>').value = RenewalDate
                $('#<%= txtRenewalDate.ClientId %>').attr('disabled', 'disabled');
                $('#ctl00_cntMainBody_RenewalDate_CalendarLookup_lblCalenderIcon').attr("style", "pointer-events:none!important");
                $('#ctl00_cntMainBody_RenewalDate_CalendarLookup_lblCalenderIcon').attr("readonly", true);

            }
        }

        //Will be called on page load
        //This will enable/disable calender controls on the basis of associated checkbox value
        onload = function () {
            if (!document.getElementById('<%= chkRenewalDate.ClientId %>').checked) {
                $('#ctl00_cntMainBody_RenewalDate_CalendarLookup_lblCalenderIcon').attr("style", "pointer-events:none!important");
                $('#ctl00_cntMainBody_RenewalDate_CalendarLookup_lblCalenderIcon').attr("readonly", true);
            }
        }

        function pageLoad() {
            var manager = Sys.WebForms.PageRequestManager.getInstance();
            manager.add_beginRequest(OnBeginRequest);
            manager.add_endRequest(OnEndRequest);
        }

        function OnBeginRequest(sender, args) {

            //disable the button (or whatever else we need to do) here
            var btnLapse = document.getElementById('<%= btnLapse.ClientId%>');
            var btnDelete = document.getElementById('<%= btnDelete.ClientId%>');
            var btnStatus = document.getElementById('<%= btnStatus.ClientId%>');


            //            if (btnTransfer != null) {
            //                btnTransfer.disabled = true;
            //            }

            if (btnLapse != null) {
                btnLapse.disabled = true;
            }

            if (btnDelete != null) {
                btnDelete.disabled = true;
            }

            if (btnStatus != null) {
                btnStatus.disabled = true;
            }
        }

        function OnEndRequest(sender, args) {
            //enable the button (or whatever else we need to do) here
            var btnLapse = document.getElementById('<%= btnLapse.ClientId%>');
            var btnDelete = document.getElementById('<%= btnDelete.ClientId%>');
            var btnStatus = document.getElementById('<%= btnStatus.ClientId%>');
            var rowscount = document.getElementByID(<%=grdvRenQuotes.ClientID%>).rows.length;


            if (rowscount > 0) {


                if (btnLapse != null) {
                    btnLapse.disabled = false;
                }

                if (btnDelete != null) {
                    btnDelete.disabled = false;
                }

                if (btnStatus != null) {
                    btnStatus.disabled = false;
                }
            }
        }

        //This will hold a count for selected migrated policy
        var iSelectedMigratedPolicies = 0;
        var vInsuranceFileRef = "";
        function CallMe(checkstate, sInsuranceFileKey, bIsMigratedPolicy, sInsuranceFolderKey) {
            iSelectedMigratedPolicies = parseInt(document.getElementById('<%= hfSelectedMigratedPolicy.ClientId%>').value);
            if (CheckBoxIDs != null) {
                for (var i = 0; i < CheckBoxIDs.length; i++) {
                    if (CheckBoxIDs[i] == checkstate) {
                        var cb = document.getElementById(CheckBoxIDs[i]);
                        if (cb != null) {
                            //debugger;
                            if ('<%= HttpContext.Current.Session.IsCookieless %>==True') {
                                //need to pass session id in request path
                                PageMethods.set_path('<%= System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot") %>(S(<%=Session.SessionID%>))/secure/RenewalManager.aspx');
                            }
                            if (bIsMigratedPolicy == 'True') {
                                if (cb.checked) {
                                    iSelectedMigratedPolicies = iSelectedMigratedPolicies + 1;
                                }
                                else {
                                    iSelectedMigratedPolicies = iSelectedMigratedPolicies - 1;
                                }
                                $('#<%= hfSelectedMigratedPolicy.ClientId%>').val(iSelectedMigratedPolicies);
                            }

                            PageMethods.SelectRecord(cb.checked, sInsuranceFileKey, sInsuranceFolderKey, CallSuccess, CallFailed);
                            //vInsuranceFileRef = sInsuranceFileRef;
                        }
                    }
                }
            }
        }
        // set the destination textbox value with the ContactName
        function CallSuccess(res, destCtrl) {
            var ErrorMessage = '<%= GetLocalResourceObject("lbl_Referred").ToString() %>';
            if (res.split(";")[0] == "IsReferred") {
                if (res.split(";")[1].split(",").length > 1) {
                    $("#ctl00_cntMainBody_lblPolicyReferred")[0].innerText = ErrorMessage.replace("#policy", "Policy Numbers " + res.split(";")[1] + " have")
                }
                else {
                    $("#ctl00_cntMainBody_lblPolicyReferred")[0].innerText = ErrorMessage.replace("#policy", "Policy Number " + res.split(";")[1] + " has")
                }
                $("#ctl00_cntMainBody_btnStatus").attr('disabled', 'true');
                $("#ctl00_cntMainBody_lblPolicyReferred")[0].style.display = "Block";
            }
            else if (res == "Success") {
                $("#ctl00_cntMainBody_btnStatus")[0].disabled = false;
                $("#ctl00_cntMainBody_lblPolicyReferred")[0].style.display = "none";
            }
            else { $("#ctl00_cntMainBody_lblPolicyReferred")[0].style.display = "none"; }

            if (res != undefined && res != "Success" && res.length > 0) {
                var RetArr = res.split(',');
                var LockUserName = RetArr[0];
                var cbId = RetArr[1];
                if (LockUserName != undefined && LockUserName.length > 0) {
                    alert('Policy currently locked by ' + LockUserName + ': \n Please try later.');
                    document.getElementById(CheckBoxIDs[cbId]).checked = false;
                }
            }
        }

        // alert message on some failure
        function CallFailed(res, destCtrl) {
            $("#ctl00_cntMainBody_lblPolicyReferred")[0].style.display = "none";
        }


        function setAgent(sName, sKey, sCode) {
            tb_remove();
            document.getElementById('<%= txtAgentCode.ClientId%>').value = unescape(sCode);
            document.getElementById('<%= txtAgentKey.ClientId%>').value = sKey;
            document.getElementById('<%= hdnAgentCode.ClientID%>').value = sCode;
        }

        function clearClient() {
            tb_init('a.thickbox');
        }
        function setClient(sClientName, sClientKey) {
            tb_remove();
            document.getElementById('<%= txtClient.ClientId%>').value = unescape(sClientName);
            document.getElementById('<%= txtClientKey.ClientId%>').value = sClientKey;
            document.getElementById('<%= txtClient.ClientId%>').focus();
        }

        function NewSearch() {
            document.getElementById('<%= txtRenewalDate.ClientId %>').value = "";
            document.getElementById('<%= chkRenewalDate.ClientId %>').checked = false;
        }

        $(document).ready(function () {
            if (document.getElementById('<%= hvChecked.ClientId %>').value != "true") {
                EnableDisableButtons(false);
            }
            $('input[id$=chkSelection]').click(function () {
                var isChecked = false;

                var grid = document.getElementById("<%= GridViewRenewals.ClientID %>").getElementsByTagName("tr");
                if (grid.length > 0) {
                    for (rowCount = 1; rowCount < grid.length; rowCount++) {
                        if (grid[rowCount].cells[0].getElementsByTagName("input")[0].checked == true) {
                            isChecked = true;
                        }
                    }
                }
                EnableDisableButtons(isChecked);
            });
        });

        function EnableDisableButtons(isChecked) {
            var btnLapse = $('#<%= btnLapse.ClientId%>');
            var btnStatus = $('#<%= btnStatus.ClientId%>');
            var btnDelete = $('#<%= btnDelete.ClientId%>');
            if (isChecked) {     //at least one checkbox checked }
                if (btnLapse != null) {
                    btnLapse.removeAttr('disabled');
                }
                if (btnStatus != null) {
                    btnStatus.removeAttr('disabled');
                }
                if (btnDelete != null) {
                    btnDelete.removeAttr('disabled');
                }
            }
            else {
                if (btnLapse != null) {
                    btnLapse.attr('disabled', 'disabled');

                }
                if (btnStatus != null) {
                    btnStatus.attr('disabled', 'disabled');
                }
                if (btnDelete != null) {
                    btnDelete.attr('disabled', 'disabled');
                }
            }
        }
    </script>

    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            //the below script will dynamically show and hide the newly added fields for logged in UserType
            if (document.getElementById('<%= hvIsAgent.ClientId %>').value == "1") {
                document.getElementById('liResults').style.display = 'block';

            }
            else {
                document.getElementById('liResults').style.display = 'none';

            }
        });

        //if any of the selected policy is migrated then we need to show a confirmation message
        function LapseConfirmation() {
            iSelectedMigratedPolicies = parseInt(document.getElementById('<%= hfSelectedMigratedPolicy.ClientId%>').value);
            if (iSelectedMigratedPolicies > 0) {
                var sMsgMigratedLapseConfirmation = '<%= sMsgMigratedLapseConfirmation %>';
                var LapseResponse = confirm(sMsgMigratedLapseConfirmation);
                if (LapseResponse == true) {
                    if (vInsuranceFileRef.length > 0) { publishMessage('POLICY', vInsuranceFileRef, 'LAPSE') };
                    tb_show(null, '../Modal/RenewalLapseReason.aspx?PostbackTo=' + '<%= UpdRenewal.ClientID.ToString %>' + '&modal=true&Type=All&KeepThis=true&TB_iframe=true&height=500&width=750', null); return false;
                }
            }
            else {
                tb_show(null, '../Modal/RenewalLapseReason.aspx?PostbackTo=' + '<%= UpdRenewal.ClientID.ToString %>' + '&modal=true&Type=All&KeepThis=true&TB_iframe=true&height=500&width=750', null); return false;
            }

        }
    </script>

    <asp:ScriptManager ID="ScriptManagerRenewalManager" runat="server" EnablePageMethods="true"></asp:ScriptManager>
    <div id="secure_RenewalManager">
        <asp:HiddenField ID="hvIsAgent" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hvRenewalTitle" runat="server" Value="<%$ Resources:lbl_Renewal_Title %>"></asp:HiddenField>
        <asp:HiddenField ID="hvChecked" runat="server"></asp:HiddenField>

        <asp:Panel ID="pnlsearch" runat="server" DefaultButton="btnFilter">
            <div class="card">
                <div class="card-heading">
                    <h1>
                        <asp:Literal ID="lblPageHeader" runat="server" Text="<%$ Resources:lblHeader%>"></asp:Literal></h1>
                </div>
                <div class="card-body clearfix">
                    <div class="form-horizontal">
                        <legend>
                            <asp:Label ID="lblSearchFormLegend" runat="server" Text="<%$ Resources:lbl_FilterHeading %>"></asp:Label></legend>

                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblRenewalDate" runat="server" AssociatedControlID="txtRenewalDate" Text="<%$ Resources:lbl_RenewalDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <div class="input-group">
                                    <span class="input-group-addon">
                                        <asp:CheckBox ID="chkRenewalDate" runat="server" OnClick="EnableCalender(this.checked)" Text=" " CssClass="asp-check"></asp:CheckBox>
                                    </span>
                                    <asp:TextBox ID="txtRenewalDate" runat="server" CssClass="field-medium form-control"></asp:TextBox>
                                    <uc1:CalendarLookup ID="RenewalDate_CalendarLookup" runat="server" LinkedControl="txtRenewalDate" HLevel="2"></uc1:CalendarLookup>
                                </div>
                            </div>
                            <asp:RangeValidator ID="rngRenewalDate" runat="server" ControlToValidate="txtRenewalDate" Display="None" ValidationGroup="RenewalStatusGroup" Type="Date" MinimumValue="01/01/1900" MaximumValue="01/12/9998" ErrorMessage="<%$ Resources:Err_InvalidRenewalDate %>"></asp:RangeValidator>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblStatusType" runat="server" AssociatedControlID="RenewalStatusType" Text="<%$ Resources:lbl_StatusType %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:DropDownList ID="RenewalStatusType" runat="server" CssClass="field-large form-control form-select">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblProduct" runat="server" AssociatedControlID="ddlProductType" Text="<%$ Resources:lbl_ProductType %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:DropDownList ID="ddlProductType" runat="server" CssClass="field-medium form-control form-select">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblBranchCode" runat="server" AssociatedControlID="BranchCode" Text="<%$ Resources:lbl_BranchCode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:DropDownList ID="BranchCode" runat="server" CssClass="form-control form-select">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div id="liResults" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblResults" runat="server" Text="<%$ Resources:lblResults %>" AssociatedControlID="ddlResults" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:DropDownList ID="ddlResults" runat="server" CssClass="form-control form-select">
                                    <asp:ListItem Text="<%$ Resources:li_User_Only %>" Value="UserOnly" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:li_All %>" Value="All"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div id="liPolicyNumber" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblPolicyNo" runat="server" Text="<%$Resources:lblPolicyNumber %>" AssociatedControlID="txtPolicyNo" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:TextBox ID="txtPolicyNo" runat="server" CssClass="field-medium form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div id="AgentPanel" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:UpdatePanel ID="POLICYHEADER__updPanelAgent" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Label runat="server" ID="lblAgent" AssociatedControlID="txtAgentCode" Text="Agent Code" class="col-md-4 col-sm-3 control-label">
                                    </asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <div class="input-group">
                                            <asp:TextBox ID="txtAgentCode" runat="server" ReadOnly="true" CssClass="field-medium form-control"></asp:TextBox>
                                            <span class="input-group-btn">
                                                <asp:LinkButton ID="btnAgentCode" runat="server" SkinID="btnModal">
                                                    <i class="fa fa-search"></i>
                                                     <span class="btn-fnd-txt">Agent Code</span>  
                                                </asp:LinkButton>
                                            </span>
                                        </div>
                                    </div>
                                    <asp:HiddenField ID="txtAgentKey" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="hdnAgentCode" runat="server"></asp:HiddenField>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <Nexus:ProgressIndicator ID="upSearchAgent" OverlayCssClass="updating" AssociatedUpdatePanelID="POLICYHEADER__updPanelAgent" runat="server" Visible="false">
                                <progresstemplate>
                                </progresstemplate>
                            </Nexus:ProgressIndicator>

                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:UpdatePanel ID="POLICYHEADER__updPanelClient" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Label runat="server" ID="lblClient" AssociatedControlID="txtClient" Text="<%$ Resources:btn_Client %>" class="col-md-4 col-sm-3 control-label">
                                    </asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <div class="input-group">
                                            <asp:TextBox ID="txtClient" runat="server" CssClass="form-control"></asp:TextBox>
                                            <span class="input-group-btn">
                                                <asp:LinkButton ID="btnClient" runat="server" SkinID="btnModal">
                                                    <i class="fa fa-search"></i>
                                                     <span class="btn-fnd-txt">Client</span>  
                                                </asp:LinkButton>
                                            </span>
                                        </div>
                                    </div>
                                    <asp:HiddenField ID="txtClientKey" runat="server"></asp:HiddenField>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <Nexus:ProgressIndicator ID="upSearchClient" OverlayCssClass="updating" AssociatedUpdatePanelID="POLICYHEADER__updPanelClient" runat="server" Visible="false">
                                <progresstemplate>
                                </progresstemplate>
                            </Nexus:ProgressIndicator>

                        </div>

                    </div>
                </div>
                <div class="card-footer">
                    <asp:LinkButton ID="btnNewSearch" runat="server" Text="<%$ Resources:lbl_btnNewSearch %>" OnClientClick="javascript:Page_ValidationActive = false;" SkinID="btnSecondary"></asp:LinkButton>
                    <asp:LinkButton ID="btnFilter" ValidationGroup="RenewalStatusGroup" runat="server" Text="<%$ Resources:lbl_Filter %>" SkinID="btnPrimary"></asp:LinkButton>

                </div>
            </div>
            <div id="GridViewRenewals" runat="server" class="card" visible="false">
                <div class="card-body clearfix">
                    <legend>
                        <asp:Literal ID="ltRenewalMessage" runat="server" Text="<%$ Resources:lbl_Renewal_Title %>" Visible="false"></asp:Literal>
                    </legend>


                    <asp:UpdatePanel ID="UpdRenewal" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <div class="grid-card table-responsive no-margin">
                                <asp:Panel ID="updpnlRenewal" runat="server">
                                    <asp:HiddenField ID="hfSelectedMigratedPolicy" runat="server" Value="0"></asp:HiddenField>
                                    <asp:GridView ID="grdvRenQuotes" runat="server" AutoGenerateColumns="False" DataKeyNames="InsuranceFolderKey,InsuranceFileKey,IsMarketPlacePolicy" GridLines="None" PageSize="10" AllowPaging="true" PagerSettings-Mode="Numeric" EmptyDataText="<%$ Resources:ErrorMessage %>" EmptyDataRowStyle-CssClass="noData">
                                        <Columns>
                                            <asp:BoundField DataField="InsuranceFileKey"></asp:BoundField>
                                            <asp:TemplateField ShowHeader="false">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelection" runat="server" CausesValidation="False" Text=" " CssClass="asp-check"></asp:CheckBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="PartyName" HeaderText="<%$ Resources:lbl_PartyName %>"></asp:BoundField>
                                            <asp:BoundField DataField="Reference" HeaderText="<%$ Resources:lbl_Reference %>"></asp:BoundField>
                                            <asp:BoundField DataField="ProductCode" HeaderText="<%$ Resources:lbl_ProductCode %>"></asp:BoundField>
                                            <Nexus:BoundField DataField="RenewalPremium" HeaderText="<%$ Resources:lbl_RenewalPremium %>" HtmlEncode="false" DataType="Currency"></Nexus:BoundField>
                                            <asp:BoundField DataField="CoverStartDate" HeaderText="<%$ Resources:lbl_RenewalDate %>" HtmlEncode="false" DataFormatString="{0:d}"></asp:BoundField>
                                            <asp:BoundField DataField="RenewalStatusTypeDescription" HeaderText="<%$ Resources:lbl_RenewalStatusTypeDescription %>" HtmlEncode="false" DataFormatString="{0:d}"></asp:BoundField>
                                            <asp:TemplateField HeaderText="<%$ Resources:lbl_AssociatedClients %>" SortExpression="AssociatedClients"
                                                ItemStyle-CssClass="span-4">
                                                <ItemTemplate>
                                                    <asp:Repeater ID="rptrAssociateClient" runat="server">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssociateClientName" runat="server" Text='<%#CType(Container.DataItem, System.Xml.XmlElement).GetAttribute("Name")%>'></asp:Label>
                                                            <br />
                                                        </ItemTemplate>
                                                        <HeaderTemplate>
                                                        </HeaderTemplate>
                                                        <SeparatorTemplate>
                                                        </SeparatorTemplate>
                                                    </asp:Repeater>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ShowHeader="false">
                                                <ItemTemplate>
                                                    <div class="rowMenu">
                                                        <ol id='menu_<%# Eval("InsuranceFileKey") %>' class="list-inline no-margin">
                                                            <li>
                                                                <asp:LinkButton ID="lnkbtnSelect" Text="<%$ Resources:lbl_lnkbtnSelect %>" runat="server" CausesValidation="False" CommandName="Details" SkinID="btnGrid"></asp:LinkButton>
                                                            </li>
                                                        </ol>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblInsuranceFolderKey" runat="server" CausesValidation="False" Text='<%#Eval("InsuranceFolderKey") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="grdvRenQuotes"></asp:PostBackTrigger>
                        </Triggers>
                    </asp:UpdatePanel>
                    <Nexus:ProgressIndicator ID="upRenewal" OverlayCssClass="updating" AssociatedUpdatePanelID="UpdRenewal" runat="server">
                        <progresstemplate>
                        </progresstemplate>
                    </Nexus:ProgressIndicator>
                </div>
                <asp:Panel ID="pnlButtons" runat="server" Visible="false">
                    <div class="card-footer">
                        <asp:LinkButton ID="btnLapse" runat="server" Text="<%$ Resources:lbl_lapse %>" SkinID="btnPrimary"></asp:LinkButton>
                        <asp:LinkButton ID="btnDelete" runat="server" Text="<%$ Resources:lbl_delete %>" SkinID="btnPrimary"></asp:LinkButton>
                        <asp:LinkButton ID="btnStatus" runat="server" Text="<%$ Resources:lbl_status %>" SkinID="btnPrimary"></asp:LinkButton><br>
                        <asp:Label ID="lblNoPoliciesSelected" CssClass="error" Visible="false" runat="server" Text="<%$ Resources:lbl_NoPoliciesSelected %>"></asp:Label>
                        <asp:Label ID="lblPolicyReferred" CssClass="error" Style="display: none" runat="server"
                            Text="<%$ Resources:lbl_Referred %>" />
                    </div>
                </asp:Panel>
            </div>

        </asp:Panel>
        <asp:ValidationSummary ID="vldRenewalManager" runat="server" DisplayMode="BulletList" ShowSummary="true" ValidationGroup="RenewalStatusGroup" HeaderText="<%$Resources:lbl_ValidationSummary %>" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
