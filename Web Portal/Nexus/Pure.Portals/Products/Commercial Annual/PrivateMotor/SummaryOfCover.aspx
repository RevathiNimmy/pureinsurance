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
            <Nexus:tabindex id="ctrlTabIndex" runat="server" cssclass="TabContainer" tabcontainerclass="page-progress" activetabclass="ActiveTab" disabledclass="DisabledTab" scrollable="false"></Nexus:tabindex>
            <div class="card-body clearfix">

                <div class="standard-form">
                    <%--  <h4>
                            <asp:Label runat="server" ID="lblRequiredStaffCategory" Text="Staff Catg" />
                        </h4>--%>
                    <div class="standard-form">
                        <div class="fieldset-wrapper">

                            <div class="form-horizontal">
                                <legend>
                                    <asp:Label ID="lblTitle" runat="server" Text="<%$ Resources:lblTitle %>"></asp:Label></legend>


                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <h4>
                                        <asp:Label ID="lblHeading" runat="server" Text="<%$ Resources:lblHeading %>" class="col-md-4 col-sm-3 control-label"></asp:Label></h4>
                                </div>
                                <div id="liPremium" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <%--<asp:Label ID="lblPremiumTotal" runat="server" AssociatedControlID="lblTotalPremium" />--%><%-- <asp:Literal ID="litTotalPremium" runat="server" Text="<%$ Resources:lblTotalPremium %>" />--%>
                                    <asp:Label ID="lblTotalPremium" runat="server" Text="<%$ Resources:lblTotalPremium %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <asp:Label ID="lblTotalPremiumValue" runat="server" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblClientName" runat="server" Text="<%$ Resources:lblClientName %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <asp:Label ID="lblClientNameValue" runat="server" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                </div>
                                <%--<li>
                                            <asp:Label ID="lblQuoteStatus" runat="server" Text="<%$ Resources:lblQuoteStatus %>" />
                                            <asp:label id="lblQuoteStatusValue" runat="server"></asp:label>
                                --%>
                            </div>

                        </div>
                        <h4>
                            <asp:Label ID="lblReferal" runat="server" Text="<%$ Resources:lblReferal %>" AssociatedControlID="Referral"></asp:Label></h4>
                        <uc7:referral id="Referral" cssclass="submit" runat="server"></uc7:referral>
                        <uc10:commission id="Commission" cssclass="submit" runat="server" shownetannualpremium="false" showexitbutton="false" showsavebutton="false" isleadagent="true" isretained="false" issubagent="false" showrecipient="false" showmaxcomm="false" shownetaprp="false" showgrossaprp="true"></uc10:commission>
                    </div>
                    <asp:Literal ID="lblRenewalMessage" Visible="false" Text="<%$ Resources:lbl_Renewal_Message%>" runat="server"></asp:Literal>
                    <asp:Literal ID="lblTotalPremiumRenewal" runat="server"></asp:Literal>
                    <asp:Literal ID="lblPremium" runat="server"></asp:Literal>
                    <%--commented Following code for issue 1788 --%>
                    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
                            <ContentTemplate>
                     
                                <asp:Label ID="lblPremiumDisplay" runat="server" AssociatedControlID="lblPremiumValue">
                                    <asp:Literal ID="litPremiumDisplay" runat="server" Text="<%$ Resources:lbl_PremiumDisplay %>" /><asp:label id="lblPremiumValue" runat="server"></asp:label>
                            
                    --%>
                    <asp:ValidationSummary ID="ValidationSummary1" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
                    <div class="clear">
                    </div>
                    <p class="left">
                        <asp:Literal ID="lblMessage" runat="server" Visible="false"></asp:Literal>
                    </p>
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
                            <asp:Label ID="lblDocuments" runat="server" Text="<%$ Resources:lblDocuments %>"></asp:Label></legend>
                        <uc:documentmanager runat="server" id="docMgr" autoarchiveselected="true"></uc:documentmanager>
                        <%--EnableArchive="false" EnableEmail="false"--%>
                    </div>
                    <%--</asp:Panel>--%>




                    <div>
                        <table id="ButtonTable2" class="buttontablecss">
                            <tr>
                                <td id="tdHomeButton" runat="server">
                                    <asp:LinkButton ID="btnHomePage" runat="server" Text="<%$ Resources:linAgentStartPage %>" CssClass="submit"></asp:LinkButton>
                                </td>
                                <td id="tdNewQuoteButton" runat="server">
                                    <asp:LinkButton ID="btnNewQuote" runat="server" Text="<%$ Resources:btnNewQuote %>" CssClass="submit"></asp:LinkButton>
                                </td>
                                <td id="tdBuyButton" runat="server">
                                    <asp:LinkButton ID="btnBuy" runat="server" Text="<%$ Resources:btnBuy %>" CssClass="submit"></asp:LinkButton>
                                </td>
                                <td id="tdDeclineButton" runat="server">
                                    <uc11:declinebutton id="btnDecline" cssclass="submit" runat="server" postback="true"></uc11:declinebutton>
                                </td>
                                <td id="tdDetailsButton" runat="server">
                                    <asp:LinkButton ID="btnDetails" runat="server" Text="<%$ Resources:btnDetails %>" OnClick="DetailsButton" CssClass="submit"></asp:LinkButton>
                                </td>
                                <td id="tdSaveButton" runat="server">
                                    <asp:LinkButton ID="btnSaveQuote" runat="server" Text="<%$ Resources:btnSaveQuote %>" CssClass="submit"></asp:LinkButton>
                                </td>
                                <td id="tdRequoteButton" runat="server">
                                    <asp:LinkButton ID="btnRequote" runat="server" Text="<%$ Resources:btn_Requote%>" OnClick="RequoteButton" CssClass="submit" CausesValidation="False"></asp:LinkButton>
                                </td>
                                <td visible="false" id="tdIssueButton" runat="server">
                                    <asp:LinkButton ID="btnIssue" runat="server" Text="<%$ Resources:btn_Issue %>" CausesValidation="False" CssClass="submit"></asp:LinkButton>
                                </td>
                                <td>
                                    <asp:LinkButton ID="btnEnquiry" runat="server" Text="Enquiry" PostBackUrl="~/secure/BrokerView.aspx" CssClass="submit"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                        <asp:LinkButton CssClass="submit" ID="btnLapse" runat="server" Text="<%$ Resources:btn_LapsePolicy%>" Visible="false" CausesValidation="False"></asp:LinkButton>
                        <asp:LinkButton ID="btnMarkQuoteForCollection" Width="130px" CssClass="submit" Visible="false" runat="server" Text="<%$ Resources:btn_MarkQuote%>" CausesValidation="False" OnClientClick="return MarkedConfirmation()"></asp:LinkButton>
                        <asp:LinkButton CssClass="submit" ID="btnPrint" runat="server" Text="<%$ Resources:btn_Print %>" Visible="false" CausesValidation="False"></asp:LinkButton>
                        <uc1:document id="Print_Renewaldocument" runat="server" documentname="RenewalInvite" pregenerate="false" visible="false" text="<%$ Resources:Print_Renewaldocument %>"></uc1:document>
                        <asp:CustomValidator ID="vldChkStatus" runat="server" Display="None" ClientValidationFunction="CheckStatus" ErrorMessage="<%$ Resources:lbl_Please_Check %>"></asp:CustomValidator><asp:HiddenField ID="HiddenField1" runat="server"></asp:HiddenField>
                    </div>
                </div>
            </div>
            <div class='card-footer'></div>
        </div>
    </div>
</asp:Content>
