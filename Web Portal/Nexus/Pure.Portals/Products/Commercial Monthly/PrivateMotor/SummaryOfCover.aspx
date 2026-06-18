<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/default.master" CodeFile="SummaryOfCover.aspx.vb" Inherits="Products_PAPRODUCT_SummaryOfCoverCntrl" %>

<%@ Register Src="~/Controls/Document.ascx" TagName="Document" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/DocumentList.ascx" TagName="DocumentListCtrl" TagPrefix="uc4" %>
<%@ Register Src="~/Controls/NewQuote.ascx" TagName="NewQuote" TagPrefix="uc6" %>
<%@ Register Src="~/Controls/Referral.ascx" TagName="Referral" TagPrefix="uc7" %>
<%@ Register Src="~/Controls/PremiumSummary.ascx" TagName="PremiumSummary" TagPrefix="uc8" %>
<%@ Register Src="~/Controls/PremiumConfirmation.ascx" TagName="PremiumConfirmation"
    TagPrefix="uc9" %>
<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register Src="~/Controls/ProgressBar.ascx" TagName="ProgressBar" TagPrefix="NexusControl" %>
<%@ Register Src="~/Controls/StandardWordings.ascx" TagName="StandardWording" TagPrefix="NexusControl" %>
<%@ Register Src="~/Controls/DocumentManager.ascx" TagName="DocumentManager" TagPrefix="uc" %>
<%@ Register Src="~/Controls/Commission.ascx" TagName="Commission" TagPrefix="uc10" %>
<%@ Register Src="~/Controls/DeclineButton.ascx" TagName="DeclineButton" TagPrefix="uc11" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script type="text/javascript" language="javascript">
        function ConfirmAction(path) {
            //            var r = confirm("The agent will not be able to view this quote version until you Issue it. Are you sure you wish to continue?");
            //            if (r == true) {

            //if (confirm('The agent will not be able to view this quote version until you Issue it. Are you sure you wish to continue?')) {
            //tb_show(null, path + '/Products/GPA/SAGICORPA/StdWordingSubjectivity.aspx?modal=true&TB_iframe=true&height=600&width=750', null);
            //                tb_show(null, path, null);
            //                return false;
            //            }
            //            else
            //                return false;
        }



        //        function showMessageUnderWriter() {
        //            alert('You cannot re-quote this version as the Agent already has a pending quote version.');
        //            return false;
        //        }
        //        function showMessageBroker() {
        //            alert('You cannot re-quote this version as the Underwriter already has a pending quote version.');
        //            return false;
        //        }

        //show_modal needs to call tb_show, adding paramaters and also the thickbox specific query parameters
        function show_modal(sUrl, sParams, bInline) {
            $(thisModal).dialog('destroy'); //destroy anything that already exists

            var sHref = sUrl + '?newmodal=true&' + sParams;
            if (sHref.indexOf('modal=true') == -1) { sHref += "&modal=true"; } //only add modal=true if it's not there already
            if (bInline == true) {
                thisModal = $(sUrl);
                thisModal.dialog({
                    autoOpen: false,
                    width: $(sUrl).width() + 50,
                    height: $(sUrl).height() + 50,
                    modal: true,
                    resizable: true,
                    autoResize: false,
                    overlay: {
                        opacity: 0.5,
                        background: "black"
                    },
                    close: function(event, ui) {
                        //need to move the form elements back inside the form so that they can be accessed
                        $(this).appendTo('form');
                        //hide it so that it doesn't display whilst the form is posting back
                        $(this).hide();
                    }
                }).width(850 - horizontalPadding).height(100 - verticalPadding);
                thisModal.dialog('open');
                return false;
            }
            else //open inline modal, i.e. not an iFrame
            {
                //add lines to resize parent IF parent is a modal
                if (document.URL.indexOf('modal=true') != -1) {
                    //I'm a modal, so maximise me
                    maximiseParent();
                }

                thisModal = $('<iframe id="modalDialog" class="modalDialog" src="' + sHref + '" marginheight="0" marginwidth="0" frameborder="0" name="TB_iframeContent"></iframe>');
                thisModal.dialog({
                    title: 'Loading.....',
                    autoOpen: true,
                    width: 850,
                    height: 100,
                    modal: true,
                    resizable: true,
                    autoResize: false,
                    close: function(event, ui) { resizeParent(); },
                    open: function(event, ui) { if (sUrl.indexOf("StdWordingSubjectivity.aspx") >= 0 || sUrl.indexOf("Excesses.aspx") >= 0) { $(".ui-dialog-titlebar-close").hide(); } },
                    overlay: {
                        opacity: 0.5,
                        background: "black"
                    }
                }).width(850 - horizontalPadding).height(100 - verticalPadding);
            }
        }
        function CheckStatus(oSrc, args) {
            var HiddenValue = document.getElementById('<%=HiddenField1.ClientID%>');
            var grid = document.getElementById('ctl00_cntMainBody_MultiRisk1_grdvRisk');  //Retrieve the grid  
            if (grid != null) {
                var inputs = grid.getElementsByTagName("input"); //Retrieve all the input elements from the grid
                var isValid = false;
                if (HiddenValue.value != 1) {

                    for (var i = 0; i < inputs.length; i += 1) {
                        if (inputs[i].type === "checkbox") { //if the current element's type is checkbox
                            if (inputs[i].checked === true) { //if the current checkbox is true, then atleast one checkbox is ticked, so break the loop
                                isvalid = true;
                                args.isvalid = true;
                                return true;
                                break;
                            }
                        }
                    }
                    if (!isvalid) {
                        args.isvalid = false;
                        return false;
                    }
                }
                else {
                    args.isvalid = true;
                }
            }
        }
     inputs.length;="" i="" +="1)" {="" if="" (inputs[i].type="==" "checkbox")="" {="" if="" the="" current="" element's="" type="" is="" checkbox="" if="" (inputs[i].checked="==" true)="" {="" if="" the="" current="" checkbox="" is="" true,="" then="" atleast="" one="" checkbox="" is="" ticked,="" so="" break="" the="" loop="" isvalid="true;" args.isvalid="true;" return="" true;="" break;="" }="" }="" }="" if="" (!isvalid)="" {="" args.isvalid="false;" return="" false;="" }="" }="" else="" {="" args.isvalid="true;" }="" }="" }=""></ inputs.length; i += 1) {
                        if (inputs[i].type === "checkbox") { //if the current element's type is checkbox
                            if (inputs[i].checked === true) { //if the current checkbox is true, then atleast one checkbox is ticked, so break the loop
                                isvalid = true;
                                args.isvalid = true;
                                return true;
                                break;
                            }
                        }
                    }
                    if (!isvalid) {
                        args.isvalid = false;
                        return false;
                    }
                }
                else {
                    args.isvalid = true;
                }
            }
        }
    </script>

    <asp:ScriptManager runat="server"></asp:ScriptManager>
    <div class="risk-screen">
        
    
            <NexusControl:ProgressBar ID="ucProgressBar" runat="server"></NexusControl:ProgressBar>
            <div class="card">
                <nexus:tabindex id="ctrlTabIndex" runat="server" cssclass="TabContainer" tabcontainerclass="page-progress" activetabclass="ActiveTab" disabledclass="DisabledTab" scrollable="false"></nexus:tabindex>
                <div class="card-body clearfix">
                    
                    <div class="standard-form">
                        <%--  <h4>
                            <asp:Label runat="server" ID="lblRequiredStaffCategory" Text="Staff Catg" />
                        </h4>--%>
                        <div class="standard-form">
                            <div class="fieldset-wrapper">
                                
                                <div class="form-horizontal">
                                    <legend>
                                        <asp:label id="lblTitle" runat="server" text="<%$ Resources:lblTitle %>"></asp:label></legend>
                                    
                                
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <h4>
                                                <asp:label id="lblHeading" runat="server" text="<%$ Resources:lblHeading %>" class="col-md-4 col-sm-3 control-label"></asp:label></h4>
                                        </div>
                                        <div id="liPremium" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <%--<asp:Label ID="lblPremiumTotal" runat="server" AssociatedControlID="lblTotalPremium" />--%><%-- <asp:Literal ID="litTotalPremium" runat="server" Text="<%$ Resources:lblTotalPremium %>" />--%>
                                            <asp:label id="lblTotalPremium" runat="server" text="<%$ Resources:lblTotalPremium %>" class="col-md-4 col-sm-3 control-label"></asp:label>
                                            <asp:label id="lblTotalPremiumValue" runat="server" class="col-md-4 col-sm-3 control-label"></asp:label>
                                        </div>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:label id="lblClientName" runat="server" text="<%$ Resources:lblClientName %>" class="col-md-4 col-sm-3 control-label"></asp:label>
                                            <asp:label id="lblClientNameValue" runat="server" class="col-md-4 col-sm-3 control-label"></asp:label>
                                        </div>
                                        <%--<li>
                                            <asp:Label ID="lblQuoteStatus" runat="server" Text="<%$ Resources:lblQuoteStatus %>" />
                                            <asp:label id="lblQuoteStatusValue" runat="server"></asp:label>
                                        --%>
                                    </div>
                                
                            </div>
                            <h4>
                                <asp:label id="lblReferal" runat="server" text="<%$ Resources:lblReferal %>" associatedcontrolid="Referral"></asp:label></h4>
                            <uc7:referral id="Referral" cssclass="submit" runat="server"></uc7:referral>
                            <uc10:commission id="Commission" cssclass="submit" runat="server" shownetannualpremium="false" showexitbutton="false" showsavebutton="false" isleadagent="true" isretained="false" issubagent="false" showrecipient="false" showmaxcomm="false" shownetaprp="false" showgrossaprp="true"></uc10:commission>
                        </div>
                        <asp:literal id="lblRenewalMessage" visible="false" text="<%$ Resources:lbl_Renewal_Message%>" runat="server"></asp:literal>
                        <asp:literal id="lblTotalPremiumRenewal" runat="server"></asp:literal>
                        <asp:literal id="lblPremium" runat="server"></asp:literal>
                        <%--commented Following code for issue 1788 --%>
                        <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
                            <ContentTemplate>
                     
                                <asp:Label ID="lblPremiumDisplay" runat="server" AssociatedControlID="lblPremiumValue">
                                    <asp:Literal ID="litPremiumDisplay" runat="server" Text="<%$ Resources:lbl_PremiumDisplay %>" /><asp:label id="lblPremiumValue" runat="server"></asp:label>
                            
                        --%>
                        <asp:validationsummary id="ValidationSummary1" displaymode="BulletList" headertext="<%$ Resources:lbl_ValidationSummary %>" runat="server" cssclass="validation-summary"></asp:validationsummary>
                        <div class="clear">
                        </div>
                        <p class="left">
                            <asp:literal id="lblMessage" runat="server" visible="false"></asp:literal></p>
                        <div>
                        
                        <!---commented since not part of WPR 63 -->
                        
                          <!--  <table id="ButtonTable1"   class="buttontable1css">
                                
                                
                                <tr>
                                    <td id="tdPremiumSummary" runat="server">
                                        <uc8:PremiumSummary ID="PremiumSumary" CssClass="submit" runat="server" />
                                    </td>
                                    <td id="tdSubjEnd" runat="server">
                                        <asp:LinkButton ID="btnSubjEnd" CssClass="submit" runat="server" Text="<%$ Resources:btnSubjEnd %>" />
                                    </td>
                                    <td id="tdExcesses" runat="server">
                                        <asp:LinkButton ID="btnExcesses" CssClass="submit" runat="server" Text="<%$ Resources:btnExcesses %>" />
                                        <%--OnClientClick="tb_show(null , '../SAGICORPA/Excesses.aspx?modal=true&Page=IP&KeepThis=true&TB_iframe=true&height=500&width=650' , null);return false;" />--%>
                                    </td>
                                    <td id="tdPremiumConfirmation" runat="server">
                                        <uc9:PremiumConfirmation ID="PremiumConfirmation" CssClass="submit" runat="server" />
                                    </td>
                                </tr>
                                
                                
                                
                            </table> -->
                        </div>
                        <%--<asp:Panel ID="Document_Link" runat="server">--%>
                        <div class="grid-card table-responsive">
                            <legend>
                                <asp:label id="lblDocuments" runat="server" text="<%$ Resources:lblDocuments %>"></asp:label></legend>
                            <uc:documentmanager runat="server" id="docMgr" autoarchiveselected="true"></uc:documentmanager>
                            <%--EnableArchive="false" EnableEmail="false"--%>
                        </div> 
                        <%--</asp:Panel>--%>
                       
                       
                        
                       
                        <div>
                            <table id="ButtonTable2" class="buttontablecss">
                                <tr>
                                    <td id="tdHomeButton" runat="server">
                                        <asp:LinkButton id="btnHomePage" runat="server" text="<%$ Resources:linAgentStartPage %>" cssclass="submit"></asp:LinkButton>
                                    </td>
                                    <td id="tdNewQuoteButton" runat="server">
                                        <asp:LinkButton id="btnNewQuote" runat="server" text="<%$ Resources:btnNewQuote %>" cssclass="submit"></asp:LinkButton>
                                    </td>
                                    <td id="tdBuyButton" runat="server">
                                        <asp:LinkButton id="btnBuy" runat="server" text="<%$ Resources:btnBuy %>" cssclass="submit"></asp:LinkButton>
                                    </td>
                                    <td id="tdDeclineButton" runat="server">
                                        <uc11:declinebutton id="btnDecline" cssclass="submit" runat="server" postback="true"></uc11:declinebutton>
                                    </td>
                                    <td id="tdDetailsButton" runat="server">
                                        <asp:LinkButton id="btnDetails" runat="server" text="<%$ Resources:btnDetails %>" onclick="DetailsButton" cssclass="submit"></asp:LinkButton>
                                    </td>
                                    <td id="tdSaveButton" runat="server">
                                        <asp:LinkButton id="btnSaveQuote" runat="server" text="<%$ Resources:btnSaveQuote %>" cssclass="submit"></asp:LinkButton>
                                    </td>
                                    <td id="tdRequoteButton" runat="server">
                                        <asp:LinkButton id="btnRequote" runat="server" text="<%$ Resources:btn_Requote%>" onclick="RequoteButton" cssclass="submit" causesvalidation="False"></asp:LinkButton>
                                    </td>
                                    <td visible="false" id="tdIssueButton" runat="server">
                                        <asp:LinkButton id="btnIssue" runat="server" text="<%$ Resources:btn_Issue %>" causesvalidation="False" cssclass="submit"></asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton id="btnEnquiry" runat="server" text="Enquiry" postbackurl="~/secure/BrokerView.aspx" cssclass="submit"></asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                            <asp:LinkButton cssclass="submit" id="btnLapse" runat="server" text="<%$ Resources:btn_LapsePolicy%>" visible="false" causesvalidation="False"></asp:LinkButton>
                            <asp:LinkButton id="btnMarkQuoteForCollection" width="130px" cssclass="submit" visible="false" runat="server" text="<%$ Resources:btn_MarkQuote%>" causesvalidation="False" onclientclick="return MarkedConfirmation()"></asp:LinkButton>
                            <asp:LinkButton cssclass="submit" id="btnPrint" runat="server" text="<%$ Resources:btn_Print %>" visible="false" causesvalidation="False"></asp:LinkButton>
                            <uc1:document id="Print_Renewaldocument" runat="server" documentname="RenewalInvite" pregenerate="false" visible="false" text="<%$ Resources:Print_Renewaldocument %>"></uc1:document>
                                <asp:customvalidator id="vldChkStatus" runat="server" display="None" clientvalidationfunction="CheckStatus" errormessage="<%$ Resources:lbl_Please_Check %>"></asp:customvalidator><asp:hiddenfield id="HiddenField1" runat="server"></asp:hiddenfield>
                        </div>
                    </div>
                </div><div class='card-footer'></div>
            </div>
        </div>
</asp:Content>
