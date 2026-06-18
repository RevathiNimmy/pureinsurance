<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false" EnableEventValidation="false"
    CodeFile="CashListNew.aspx.vb" Inherits="Nexus.secure_CashListTab" Title="SSP - Pure Insurance" %>

<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>

<%@ Register Src="~/Controls/CashListItem.ascx" TagName="CashListItem" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/CashListItems.ascx" TagName="CashListItems" TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/Controls/Allocation.ascx" TagName="Allocate" TagPrefix="uc5" %>
<%@ Register Src="~/Controls/CashList.ascx" TagName="CashList" TagPrefix="uc4" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cntMainBody" runat="Server">
    <asp:ScriptManager ID="smRatingDetail" runat="server"></asp:ScriptManager>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            $('#divTabs a[href="#tab-ReceiptType"]').tab('show');
            var modeType = document.getElementById('<%= hdnModeType.ClientId%>');            
            var mtaAmountToPay = document.getElementById('<%= hdnAmountToPay.ClientId%>').value;            
            var IPTotalAmount = document.getElementById('<%= hdnTotalAmountForIP.ClientId%>').value;            
            var bTotalAmount = (modeType.value == "PayNow" && parseFloat(mtaAmountToPay).toFixed(2) > parseFloat("0.0").toFixed(2))
            var bIPTotalAmount = (modeType.value == "PayNow" && parseFloat(IPTotalAmount).toFixed(2) > parseFloat("0.0").toFixed(2))
            var bInsurerPaymentType = (modeType.value == "IP" && parseFloat(IPTotalAmount).toFixed(2) > parseFloat("0.0").toFixed(2))
            if (modeType.value == "Receipt" || modeType.value == "INSDEPOSIT" || bTotalAmount == true || bIPTotalAmount == true || bInsurerPaymentType == true|| modeType.value == "ReverseReceipt") {

                $('#lblReceiptTypeHeading').text('Receipt Type');
            } else {
                $('#lblReceiptTypeHeading').text('Payment Type');
                }
        });
        function CancelConfirmation() {
            var sTakeConfirmation = confirm("Cancelling will lose any changes. Do you want to cancel?");
            if (sTakeConfirmation == false) {
                return false;
            }
            else {
                return true;
            }
        }
        $(function () {

            var btnID = '<%=btnCashListNext.UniqueID %>';
            var queryStringModeValue = '<%: HttpUtility.JavaScriptStringEncode(Session("ModeValue")) %>';
            var addMoreCashList = $('#hdnAddMoreCashList').val();
            var hfViewOption = $('#hfViewOption').val();

            var tabName = $("[id*=hdnTabName]").val() != "" ? $("[id*=hdnTabName]").val() : "tab-CashList";

            $('#divTabs a[href="#' + tabName + '"]').tab('show');

            
            if (tabName == "tab-CashList" && queryStringModeValue != 'CR') {
                $('#hdnTabName').val('tab-CashList');
                $('#divTabs a[href="#tab-ReceiptType"]').attr('disabled', 'disabled');
                $('#divTabs a[href="#tab-CashListItems"]').attr('disabled', 'disabled');
                $('#divTabs a[href="#tab-Allocate"]').attr('disabled', 'disabled');
                <% if (Request.QueryString.HasKeys()) Then%>

                <%Else%>
                   $('#btnCashListCancel').css("display", "none");
                <%End If%>
            }

            else if (tabName == "tab-ReceiptType" && (queryStringModeValue == 'CR' || queryStringModeValue == "PayNow") && (addMoreCashList == "No" || addMoreCashList == "")) {
                $('#hdnTabName').val('tab-CashListItem');

                $('#divTabs a[href="#tab-CashListItems"]').attr('disabled', 'disabled');
                $('#divTabs a[href="#tab-Allocate"]').attr('disabled', 'disabled');
            }
            else if (tabName == "tab-ReceiptType" && queryStringModeValue == 'CR' && addMoreCashList == "Yes") {
                __doPostBack('btnCashListItemsAdd', 'CRAddCashListItem');
                $('#divTabs a[href="#tab-CashListItems"]').attr('disabled', 'disabled');
                $('#divTabs a[href="#tab-Allocate"]').attr('disabled', 'disabled');

            }
            else if (tabName == "tab-ReceiptType")
            {
                <% if (Request.QueryString.HasKeys()) Then%>

                <%Else%>
                    $('#btnCashListItemCancel').css("display", "none");
                <%End If%>
            }
            else if (tabName == "tab-CashListItem" && hfViewOption == "ViewReceiptAllocation") {
                $('#divTabs a[href="#tab-Allocate"]').attr('disabled', 'disabled');
            }
           
            else if (tabName == "tab-CashListItem") {
                //on cashlist view enable tab-CashListItems
                $('#divTabs a[href="#tab-CashListItems"]').attr('disabled', 'disabled');
                $('#divTabs a[href="#tab-Allocate"]').attr('disabled', 'disabled');
            }
            
            else if (tabName == "tab-CashListItems") {
                 $('#divTabs a[href="#tab-Allocate"]').attr('disabled', 'disabled');

            }
            else if (tabName == "tab-Allocate" && hfViewOption == "ViewReceiptAllocation") {
                $('#divTabs a[href="#tab-ReceiptType"]').attr('disabled', 'disabled')
            }
            else if (tabName == "tab-Allocate" && hfViewOption == "ViewAllocation") {
                //hfViewOption.val = "";
                $('#divTabs a[href="#tab-ReceiptType"]').attr('disabled', 'disabled')
                $('#divTabs a[href="#tab-CashListItems"]').attr('disabled', 'disabled');
            }
            else if (tabName == "tab-Allocate" && queryStringModeValue == 'Allocation') {
                $('#divTabs a[href="#tab-CashList"]').addClass('');
                $('#divTabs a[href="#tab-CashListItems"]').addClass('');
                $('#divTabs a[href="#tab-ReceiptType"]').addClass('');
            }
           

            $("#divTabs a").click(function () {
                $("[id*=tabName]").val($(this).attr("href").replace("#", ""));
                $('#divTabs a[href="#' + $(this).attr("href").replace("#", "") + '"]');
                var tabNameFrom = $("[id*=hdnTabName]").val() != "" ? $("[id*=hdnTabName]").val() : "tab-CashList";
                var control = $(this).attr("href").replace("#", "");
                if (tabNameFrom == control || (tabNameFrom == "tab-CashListItem" && control == "tab-ReceiptType")) {

                } else {
                    if (control == 'tab-CashList') {
                        $('#btnCashListNext').css("display", "none");
                        $('#btnCashListCancel').css("display", "none");
                        var hdnTabName = $("[id*=hdnTabName]").val();                         
                        if (hdnTabName == "tab-CashListItems" || hdnTabName == "tab-CashListItem" || hdnTabName == "tab-Allocate") {                            
                            $("#ctl00_cntMainBody_ucCashList_GISLookup_BankAccount").prop("disabled", "disabled");
                            $("#ctl00_cntMainBody_ucCashList_ddlBranchCode").prop("disabled", "disabled");
                            $("#ctl00_cntMainBody_ucCashList_CashList_Currencies").prop("disabled", "disabled");
                        }
                    }
                    else if (control == 'tab-ReceiptType') {
                       
                        <% if (Request.QueryString.HasKeys()) Then%>
                     
                        var modeValue = '<%: HttpUtility.JavaScriptStringEncode(Request.QueryString("Mode").ToLower()) %>';
                            if (modeValue != "payment") {
                                document.getElementById("tab-payment").style.display = "none";
                                document.getElementById("liPaymentTab").style.display = "none";
                            }
                            ShowHideInstalTab();
                         <%End If %>
                    }
                    else if (control == 'tab-CashListItems') {
                        $('#btnCashListNext').css("display", "none");
                        $('#btnCashListCancel').css("display", "none");
                        if (hfViewOption == "ViewReceiptAllocation" || hfViewOption == "ViewAllocation") {
                            $('#divTabs a[href="#tab-ReceiptType"]').attr('disabled', 'disabled')
                            $('#divTabs a[href="#tab-Allocate"]').attr('disabled', 'disabled');
                        }
                        if (tabNameFrom == "tab-Allocate") 
                        {
                            $('#divTabs a[href="#tab-Allocate"]').removeAttr("disabled");
                        }
                    }

                }
               

            });

        });

        function loadRefresh(addMoreCashList) {

            var tabName = $('#hdnTabName').val();

            $('#divTabs a[href="#' + tabName + '"]').tab('show');
            if (addMoreCashList == "Yes") {
                $('#divTabs a[href="#tab-ReceiptType"]').tab('show');
                __doPostBack("btnCashListItemsAdd", "RefreshIP");
            }

        }
        function disableButton(value){
            if (value == "Yes"){
                $('#btnCashListItemNext').css("display", "none");
                        $('#btnCashListItemCancel').css("display", "none");
            }
        }

        function DisableTabOnAllocateCancel() {
            $('#divTabs a[href="#tab-Allocate"]').attr('disabled', 'disabled');
            $('#divTabs a[href="#tab-CashListItems"]').removeAttr("disabled");
        }
        function HideTab(mode) {
            if (mode == "IP") {
                $('#divTabs a[href="#tab-Allocate"]').attr('hidden', 'hidden');
            }
            else if (mode = "PayNow") {
                $('#divTabs a[href="#tab-Allocate"]').attr('hidden', 'hidden');
                $('#divTabs a[href="#tab-CashListItems"]').attr('hidden', 'hidden');
            }
        }
        function changeToTab2() {
            $('.tab-cashlist li:eq(2) a').tab('show');
        }
        function DisableTab() {
            $('#divTabs a[href="#tab-Allocate"]').attr('disabled', 'disabled');
        }
        
        function PostClick() {
            $('#divTabs a[href="#tab-ReceiptType"]').attr('disabled', 'disabled');
        }
    </script>
    <asp:HiddenField ID="hdnTabName" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnAddMoreCashList" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hfMode" runat="server" ClientIDMode="Static" Value="" />
    <asp:HiddenField ID="hfCashListItemID" runat="server" ClientIDMode="Static" Value="" />
    <asp:HiddenField ID="hfType" runat="server" ClientIDMode="Static" Value="" />
    <asp:HiddenField ID="hfTransdetailKey" runat="server" ClientIDMode="Static" Value="" />
    
    <div id="secure_RiskDetails">
        <asp:Panel ID="pnlRiskDetls" runat="server" Visible="true" EnableViewState="true">
            <div id="tabs" class="card">
                <div id="divTabs" class="md-whiteframe-z0 bg-white">
                    <ul class="nav nav-lines nav-tabs b-danger tab-cashlist">
                        <li>
                            <a data-bs-toggle="tab" href="#tab-CashList" aria-expanded="true" class="active"><asp:Literal ID="lblCashListTabTitle" runat="server" Text="<%$ Resources:lbl_CashListTabTitle %>"></asp:Literal></a>
                        </li>
                        <li runat="server">
                            <a data-bs-toggle="tab" href="#tab-ReceiptType" id="lblReceiptTypeHeading" aria-expanded="false"><asp:Literal ID="lblReceiptTypeTabTitle" runat="server" Text="<%$ Resources:lbl_ReceiptTypeTabTitle %>"></asp:Literal></a>
                        </li>
                        <li id="liCashList_Item" runat="server">
                            <a data-bs-toggle="tab" href="#tab-CashListItems" aria-expanded="false"><asp:Literal ID="lblCashListItemsTabTitle" runat="server" Text="<%$ Resources:lbl_CashListItemsTabTitle %>"></asp:Literal></a>
                        </li>
                        <li runat="server">
                            <a data-bs-toggle="tab" href="#tab-Allocate" aria-expanded="false"><asp:Literal ID="lblAllocateTabTitle" runat="server" Text="<%$ Resources:lbl_AllocateTabTitle %>"></asp:Literal></a>
                        </li>
                    </ul>
                </div>
                <div class="tab-content clearfix p b-t b-t-2x">
                    <div role="tabpanel" class="tab-pane animated fadeIn active" id="tab-CashList">
                        <asp:UpdatePanel ID="updCashList" runat="server">
                            <ContentTemplate>
                                <uc4:CashList ID="ucCashList" runat="server"></uc4:CashList>
                            </ContentTemplate>
                            <Triggers>
                                <%--<asp:AsyncPostBackTrigger ControlID="rblPayee" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>--%>
                            </Triggers>
                        </asp:UpdatePanel>
                        <div class="text-end">
                            <asp:LinkButton ID="btnCashListCancel" ClientIDMode="Static" runat="server" Text="Cancel" SkinID="btnSecondary" CausesValidation="false"></asp:LinkButton>
                            <asp:LinkButton ID="btnCashListNext" runat="server" ClientIDMode="Static" Text="Next" SkinID="btnPrimary" ValidationGroup="grpvalcashlist"></asp:LinkButton>
                        </div>

                        <Nexus:ProgressIndicator ID="piCashList" OverlayCssClass="updating" AssociatedUpdatePanelID="updCashList" runat="server">
                            <progresstemplate></progresstemplate>
                        </Nexus:ProgressIndicator>

                    </div>
                    <div role="tabpanel" class="tab-pane animated fadeIn " id="tab-ReceiptType">

                        <asp:UpdatePanel ID="updCashList_Item" runat="server">
                            <ContentTemplate>
                                <uc2:CashListItem ID="ucCashListItem" runat="server"></uc2:CashListItem>
                                <%--                                <asp:PlaceHolder ID="phCashlistitem" runat="server"></asp:PlaceHolder>--%>
                            </ContentTemplate>
                            <%-- <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="GISLookup_MediaType" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
                            </Triggers>--%>
                        </asp:UpdatePanel>
                        <div class="text-end">
                            <asp:LinkButton ID="btnCashListItemCancel" runat="server" ClientIDMode="Static" Text="Cancel" SkinID="btnSecondary" CausesValidation="false"></asp:LinkButton>
                            <asp:LinkButton ID="btnCashListItemNext" ClientIDMode="Static" runat="server" Text="Next" SkinID="btnPrimary" ValidationGroup="grpvalcashlistitem"></asp:LinkButton>
                            <asp:LinkButton ID="btnClose2" runat="server" Text="Close" SkinID="btnSecondary" CausesValidation="false" Visible="false"  OnClientClick="changeToTab2();PostClick(); return false;"></asp:LinkButton>

                        </div>
                        <Nexus:ProgressIndicator ID="ProgressIndicator1" OverlayCssClass="updating" AssociatedUpdatePanelID="updCashList_Items" runat="server">
                            <progresstemplate></progresstemplate>
                        </Nexus:ProgressIndicator>
                    </div>
                    <div role="tabpanel" class="tab-pane animated fadeIn" id="tab-CashListItems">

                        <asp:UpdatePanel ID="updCashList_Items" runat="server">
                            <ContentTemplate>
                                <uc3:CashListItems ID="ucCashListItems" runat="server"></uc3:CashListItems>
                                <%--                                <asp:PlaceHolder ID="phCashlistitems" runat="server"></asp:PlaceHolder>--%>
                                <div class="text-end">
                                    <asp:LinkButton ID="btnCashListItemsCancel" runat="server" Text="Cancel" SkinID="btnSecondary" CausesValidation="false"></asp:LinkButton>
                                    <asp:LinkButton ID="btnCashListItemsAdd" runat="server" Text="Add" SkinID="btnPrimary" CausesValidation="false" OnClientClick="AddClick();"></asp:LinkButton>
                                </div>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <Nexus:ProgressIndicator ID="ProgressIndicator2" OverlayCssClass="updating" AssociatedUpdatePanelID="updCashList_Item" runat="server">
                            <progresstemplate></progresstemplate>
                        </Nexus:ProgressIndicator>
                    </div>

                    <div role="tabpanel" class="tab-pane animated fadeIn" id="tab-Allocate">

                        <asp:UpdatePanel ID="updAllocation" runat="server">
                            <ContentTemplate>
                                <uc5:Allocate ID="ucAllocate" runat="server"></uc5:Allocate>
                                <div class="text-end">
                                    <asp:LinkButton ID="btnAllocateCancel" runat="server" Text="Cancel" SkinID="btnSecondary" CausesValidation="false"></asp:LinkButton>
                                    <asp:LinkButton ID="btnAllocateOK" runat="server" Text="OK" SkinID="btnPrimary" CausesValidation="false" ValidationGroup="grpvalAllocate"></asp:LinkButton>
                                    <asp:LinkButton ID="btnClose" runat="server" Text="Close" SkinID="btnSecondary" CausesValidation="false" Visible="false"  OnClientClick="changeToTab2();DisableTab(); return false;"></asp:LinkButton>

                                </div>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <Nexus:ProgressIndicator ID="ProgressIndicator3" OverlayCssClass="updating" AssociatedUpdatePanelID="updAllocation" runat="server">
                            <progresstemplate></progresstemplate>
                        </Nexus:ProgressIndicator>
                    </div>

                </div>
            </div>
            <asp:HiddenField ID="hfModeValueee" Value="0" runat="server" />
            <asp:HiddenField ID="hdnModeType" runat="server" />
            <asp:HiddenField ID="hdnAmountToPay" Value="-1" runat="server" />
            <asp:HiddenField ID="hdnTotalAmountForIP" Value="-1" runat="server" />
        </asp:Panel>
    </div>
</asp:Content>
