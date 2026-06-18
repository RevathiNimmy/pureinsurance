    <%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="SearchTransactions.aspx.vb" Inherits="Nexus.SearchTransactions" EnableEventValidation="false" %>

<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
      <head>
        <meta name="viewport" content="width=device-width, initial-scale=1">
    </head>

    <script language="javascript" type="text/javascript"> 
        

        $(document).ready(function () {
            $("#ctl00_cntMainBody_ColumnSelectorExtender")[0].childNodes['0'].href = "/localhost/Pure.Portal/secure/SearchTransactions.aspx#TB_inline?height=400&width=200&inlineId=ColumnSelector";
        });


        function setAccount(sShortCode, shiddenShortCode, sAccountName, iPartyKey, sCurrencyCode, sType) //setAccount
        {
            tb_remove();
            if (sType == 'IACC') {
                //SetInsuredAccount
                document.getElementById('<%= txtInsuredAccountCode.ClientId%>').value = unescape(sShortCode);
                document.getElementById('<%= txtInsuredAccountCode.ClientId%>').focus();
            }
            else {
                //SetAccount
                document.getElementById('<%= txtAccountCode.ClientId%>').value = unescape(sShortCode);
                document.getElementById('<%= hiddenAccountCode.ClientId%>').value = unescape(shiddenShortCode);
            }
            $("#<%=hdnIsAccountFound.ClientID%>").val("True");
        }

        function pageLoad() {
            //this is needed if the trigger is external to the update panel   
            var manager = Sys.WebForms.PageRequestManager.getInstance();
            manager.add_beginRequest(OnBeginRequest);
            manager.add_endRequest(OnEndRequest);
        }

        function OnBeginRequest(sender, args) {

            var postBackElement = args.get_postBackElement();
            if (postBackElement.id == 'btnAccount') {
                $get(piAccount).style.display = "block";
            }

            //disable the button (or whatever else we need to do) here
            var dSearchTransactions = document.getElementById('divButtons');

            if (dSearchTransactions != null) {
                dSearchTransactions.disabled = true;
            }

        }

        function OnEndRequest(sender, args) {
            //enable the button (or whatever else we need to do) here
            var dSearchTransactions = document.getElementById('divButtons');

            if (dSearchTransactions != null) {
                dSearchTransactions.disabled = false;
            }
        }

        function RefreshReverseAllocation() {
            tb_remove();
            __doPostBack('<%=UpdDocRefTransaction.ClientID %>', 'RefreshReverseAllocation');
        }

        function RefreshAllocation() {
            tb_remove();
            __doPostBack('<%=UpdDocRefTransaction.ClientID %>', 'RefreshAllocation');
        }

        function ReverseConfirmation() {
            return confirm("Do you want to reverse the selected cash item ?");
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    findAccount();
                }
            });
        };

        function findAccount() {
            var shortCode = escape($('#<%=txtAccountCode.ClientID%>').val());
            if ("<%=HttpContext.Current.Session.IsCookieless%>") {
                if ($("#<%=hdnIsAccountFound.ClientID%>").val() == "False") {
                    tb_show(null, '../Modal/FindAccount.aspx?modal=true&KeepThis=true&FromPage=ACC&TB_iframe=true&height=500&width=800&shortcode=' + shortCode, null);
                    return false;
                }
            }
            else {
                tb_show(null, '" & "<%=System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot")%>" & "(S(" & <%=HttpContext.Current.Session.SessionID.ToString() %>" + "))" & "/Modal/FindAccount.aspx?modal=true&KeepThis=true&FromPage=ACC&TB_iframe=true&height=500&width=800&shortcode=' + shortCode, null);
                return false;
            }

            if ($("#<%=txtAccountCode.ClientID%>").val().length == 0) {
                $("#<%=hdnIsAccountFound.ClientID%>").val("True");
            }
            if ($("#<%=hiddenAccountname.ClientID%>").val() == $("#<%=txtAccountCode.ClientID%>").val()) {
                $("#<%=hdnIsAccountFound.ClientID%>").val("True");
            }
        }
    </script>

    <script>
        function RedirectToDownload() {
            var frame = $("#<%=docFrame.ClientID%>");
            frame.attr("src", "Download.aspx");
        };
    </script>

    <asp:ScriptManager ID="smSearchTransactions" runat="server" AsyncPostBackTimeout="1200"></asp:ScriptManager>
    <div id="secure_SearchTransactions">
        <iframe id="docFrame" runat="server" width="0px" height="0px" class="hide"></iframe>
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblPageHeader" runat="server" Text="<%$ Resources:lblHeader%>"></asp:Literal>
                </h1>
            </div>
              <div class="accordion" id="accordionExample">
      <div class="accordion-item">
           <div class="card-body clearfix">
    <div class="md-whiteframe-z0 bg-white">
        <div id="accordionPanelsStayOpenExample">
            <div class="row">
                <div class="col-lg-6 col-md-6 col-sm-12">
                    <ul class="nav nav-lines nav-tabs b-danger">
                        <li><a href="#tab-accountdetails" data-bs-toggle="tab" aria-expanded="true" class="active">Account Details</a></li>
                        <li><a href="#tab-Currency" data-bs-toggle="tab" aria-expanded="true">Currency</a></li>
                        <li><a href="#tab-Document" data-bs-toggle="tab" aria-expanded="true">Document</a></li>
                        <li><a href="#tab-Amount" data-bs-toggle="tab" aria-expanded="true">Amount</a></li>
                        <li><a href="#tab-Reference" data-bs-toggle="tab" aria-expanded="true">Reference</a></li>
                    </ul>
                </div>
                <div class="col-lg-5 col-md-5 col-sm-5"></div>

            </div>
        </div>

        <div class="tab-content clearfix p b-t b-t-2x">

            <div id="tab-accountdetails" class="tab-pane animated fadeIn active" role="tabpanel">
                <div class="col-lg-1 col-md-1 col-sm-1 float-end">
                    <button class="accordion-button acc-style" type="button" data-bs-toggle="collapse" data-bs-target="#panelsStayOpen_collapseOne" aria-expanded="true" aria-controls="panelsStayOpen_collapseOne">
                    </button>
                </div>
                <asp:UpdatePanel ID="UpdAccountDetails" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="PnlSTAccount" runat="server" DefaultButton="btnFindNow">
                            <asp:HiddenField ID="hdnIsAccountFound" runat="server"></asp:HiddenField>
                            <div class="form-horizontal">
                                <div id="panelsStayOpen_collapseOne" class="accordion-collapse collapse show" aria-labelledby="headingOne">
                                    <div class="accordion-body">
                                        <div id="UserAgentSection" runat="server">
                                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                                <asp:Label ID="lblAgentCode" runat="server" AssociatedControlID="txtAgentCode" class="col-md-4 col-sm-3 control-label">
                                                    <asp:Literal ID="Literal15" runat="server" Text="<%$ Resources:lbl_AccountCode %>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                                        <asp:TextBox ID="txtAgentCode" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                            </div>
                                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                                <asp:Label ID="lblAgentName" runat="server" AssociatedControlID="txtAgentName" class="col-md-4 col-sm-3 control-label">
                                                    <asp:Literal ID="Literal16" runat="server" Text="<%$ Resources:lbl_AccountName%>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                                        <asp:TextBox ID="txtAgentName" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                    </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <%-- <asp:Button ID="btnAccountCode" runat="server" CssClass="md-btn md-raised p-xs p-h-sm grey-300 waves-effect" TabIndex="1" Text="<%$ Resources:lbl_AccountCode %>"
                                        CausesValidation="false" />--%>
                                            <asp:Label ID="lblAccountCode" runat="server" CssClass="col-lg-4 col-md-4 col-sm-3 control-label" AssociatedControlID="txtAccountCode">
                                                <asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:lbl_AccountCode %>"></asp:Literal></asp:Label>
                                            <div class="col-md-8 col-sm-9">
                                                <div class="row">
                                                    <div class="col-sm-8">
                                                        <div class="input-group flex-nowrap">
                                                            <asp:TextBox ID="txtAccountCode" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                                                            <span class="input-group-btn">
                                                                <asp:LinkButton ID="btnAccountCode" runat="server" SkinID="btnModal" TabIndex="2" CausesValidation="false">
                                                         <i class="fa fa-search"></i>
                                                         <span class="btn-fnd-txt">Account Code</span>  
                                                                </asp:LinkButton>
                                                            </span>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-4">
                                                        <asp:TextBox ID="txtActive" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <asp:HiddenField ID="hiddenAccountCode" runat="server"></asp:HiddenField>
                                                <asp:HiddenField ID="hiddenAccountname" runat="server"></asp:HiddenField>
                                            </div>
                                        </div>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblAccountName" runat="server" AssociatedControlID="txtAccountName" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="ltAccountName" runat="server" Text="<%$ Resources:lbl_AccountName%>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                                    <asp:TextBox ID="txtAccountName" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                </div>
                                        </div>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblContactName" runat="server" AssociatedControlID="txtContactName" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="ltContactName" runat="server" Text="<%$ Resources:lbl_ContactName%>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                                    <asp:TextBox ID="txtContactName" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                </div>
                                        </div>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblTelePhoneNumber" runat="server" AssociatedControlID="txtTelePhoneNumber" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="ltTelePhoneNumber" runat="server" Text="<%$ Resources:lbl_TelePhoneNumber%>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                                    <asp:TextBox ID="txtTelePhoneNumber" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                </div>
                                        </div>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblReason" runat="server" AssociatedControlID="txtReason" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="ltReason" runat="server" Text="<%$ Resources:lbl_Reason%>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                                    <asp:TextBox ID="txtReason" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                </div>
                                        </div>
                                        <div id="liBranch" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblBranchCode" runat="server" AssociatedControlID="ddlBranchCode" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="litBranchCode" runat="server" Text="<%$ Resources:lbl_BranchCode %>"></asp:Literal>
                                            </asp:Label><div class="col-md-8 col-sm-9">
                                                <asp:DropDownList ID="ddlBranchCode" TabIndex="2" runat="server" DataTextField="Description" DataValueField="Code" CssClass="field-medium form-control form-select"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblAccountBalance" runat="server" AssociatedControlID="txtAccountBalance" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="ltAccountBalance" runat="server" Text="<%$ Resources:lbl_AccountBalance%>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                                    <asp:TextBox ID="txtAccountBalance" runat="server" TabIndex="3" CssClass="form-control"></asp:TextBox>
                                                </div>
                                        </div>
                                    </div>
                                    <div class="form-horizontal">
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblShowOutStandingTransaction" runat="server" AssociatedControlID="chkShowOutStandingTransaction" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:lbl_ShowOutStandingTransaction%>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                                    <asp:CheckBox ID="chkShowOutStandingTransaction" TabIndex="4" runat="server" Checked="true" Text=" " CssClass="asp-check"></asp:CheckBox>
                                                </div>
                                        </div>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblIncludeFuturebalanceTransaction" runat="server" AssociatedControlID="chkIncludeFuturebalanceTransaction" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="ltIncludeFuturebalanceTransaction" runat="server" Text="<%$ Resources:lbl_IncludeFuturebalanceTransaction%>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                                    <asp:CheckBox ID="chkIncludeFuturebalanceTransaction" TabIndex="5" runat="server" Text=" " CssClass="asp-check"></asp:CheckBox>
                                                </div>
                                        </div>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblShowOnlyLatest500Transactions" runat="server" AssociatedControlID="chkShowOnlyLatest500Transactions" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="ltShowOnlyLatest500Transactions" runat="server" Text="<%$ Resources:lbl_ShowOnlyLatest500Transactions%>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                                    <asp:CheckBox ID="chkShowOnlyLatest500Transactions" TabIndex="6" runat="server" Checked="true" Text=" " CssClass="asp-check"></asp:CheckBox>
                                                </div>
                                        </div>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblIncludeReversedReversalTransactions" runat="server" AssociatedControlID="chkIncludeReversedReversalTransactions" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="ltIncludeReversedReversalTransactions" runat="server" Text="<%$ Resources:lbl_IncludeReversedReversalTransactions%>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                                    <asp:CheckBox ID="chkIncludeReversedReversalTransactions" TabIndex="7" runat="server" Checked="true" Text=" " CssClass="asp-check"></asp:CheckBox>
                                                </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnFindNow" EventName="Click"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="btnNewsearch" EventName="Click"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="btnReverse" EventName="Click"></asp:AsyncPostBackTrigger>
                    </Triggers>
                </asp:UpdatePanel>
                <nexus:ProgressIndicator ID="UpAccDetails" OverlayCssClass="updating" AssociatedUpdatePanelID="UpdAccountDetails" runat="server">
                    <progresstemplate>
                    </progresstemplate>
                </nexus:ProgressIndicator>
            </div>
            <div id="tab-Currency" class="tab-pane animated fadeIn" role="tabpanel">
                <div class="col-lg-1 col-md-1 col-sm-1 float-end">
                    <button class="accordion-button acc-style" type="button" data-bs-toggle="collapse" data-bs-target="#panelsStayOpen_collapsetwo" aria-expanded="true" aria-controls="panelsStayOpen_collapsetwo">
                    </button>
                </div>
                <asp:Panel ID="PnlAmountColCurrency" runat="server" DefaultButton="btnFindNow">
                    <div class="form-horizontal">
                        <div id="panelsStayOpen_collapsetwo" class="accordion-collapse collapse show" aria-labelledby="panelsStayOpen-headingOne">
                            <div class="accordion-body">
                                <legend><span>
                                    <asp:Literal ID="ltAmountColumn" runat="server" Text="<%$ Resources:lbl_AmountColumnCurrency %>"></asp:Literal>
                                </span></legend>

                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <div class="col-md-8 col-sm-9">
                                        <asp:RadioButtonList ID="rbtAmountColumn" RepeatDirection="Vertical" RepeatLayout="Flow" TabIndex="9" runat="server" CssClass="asp-radio">
                                            <asp:ListItem Text="Transaction" Value="TC"></asp:ListItem>
                                            <asp:ListItem Text="Base" Value="BC"></asp:ListItem>
                                            <asp:ListItem Text="Account" Value="AC"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </asp:Panel>
                <asp:Panel ID="PnlOCurrencyCol" runat="server" DefaultButton="btnFindNow">
                    <div class="form-horizontal">

                        <legend>
                            <asp:Literal ID="ltOutstandingColumnCurrency" runat="server" Text="<%$ Resources:lbl_OutstandingColumnCurrency %>"></asp:Literal>
                        </legend>

                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <div class="col-md-8 col-sm-9">
                                <asp:RadioButtonList ID="rbtOutStandingColumn" RepeatDirection="Vertical" RepeatLayout="Flow" TabIndex="11" runat="server" CssClass="asp-radio">
                                    <asp:ListItem Text="Base" Value="BC"></asp:ListItem>
                                    <asp:ListItem Text="Transaction" Value="TC"></asp:ListItem>
                                    <asp:ListItem Text="Account" Value="AC"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </div>
                    </div>


                </asp:Panel>
            </div>
            <div id="tab-Document" class="tab-pane animated fadeIn" role="tabpanel">
                <button class="accordion-button acc-style" type="button" data-bs-toggle="collapse" data-bs-target="#panelsStayOpen_collapsethird" aria-expanded="true" aria-controls="panelsStayOpen_collapsethird">
                </button>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="PnlDocumentCol" runat="server" DefaultButton="btnFindNow">
                            <div class="form-horizontal">
                                <div id="panelsStayOpen_collapsethird" class="accordion-collapse collapse show" aria-labelledby="panelsStayOpen-headingOne">
                                    <div class="accordion-body">
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblDocumentRef" runat="server" AssociatedControlID="txtDocumentRef" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="ltDocumentRef" runat="server" Text="<%$ Resources:lbl_DocumentRef%>"></asp:Literal>
                                            </asp:Label><div class="col-md-8 col-sm-9">
                                                <asp:TextBox ID="txtDocumentRef" runat="server" CssClass="form-control" TabIndex="13"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblDocGroup" runat="server" AssociatedControlID="DocumentGroup" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="ltDocGroup" runat="server" Text="<%$ Resources:lbl_DocGroup%>"></asp:Literal>
                                            </asp:Label><div class="col-md-8 col-sm-9">
                                                <NexusProvider:LookupList ID="DocumentGroup" runat="server" DataItemValue="Code" TabIndex="14" DataItemText="Description" Sort="ASC" ListType="pmlookup" ListCode="DOCTYPEGROUP" CssClass="field-medium form-control form-select" DefaultText="<%$ Resources:lbl_DocGroupdefaultText%>"></NexusProvider:LookupList>
                                            </div>
                                        </div>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblDocType" runat="server" AssociatedControlID="DocumentType" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="ltDocType" runat="server" Text="<%$ Resources:lbl_DocType%>"></asp:Literal>
                                            </asp:Label><div class="col-md-8 col-sm-9">
                                                <NexusProvider:LookupList ID="DocumentType" runat="server" DataItemValue="Code" DataItemText="Description" Sort="ASC" ListType="pmlookup" ListCode="DOCUMENTTYPE" CssClass="field-medium form-control form-select" TabIndex="15" DefaultText="<%$ Resources:lbl_DoctypedefaultText%>"></NexusProvider:LookupList>
                                            </div>
                                        </div>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblPeriod" runat="server" AssociatedControlID="ddlPeriod" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="ltperiod" runat="server" Text="<%$ Resources:lbl_Period%>"></asp:Literal>
                                            </asp:Label><div class="col-md-8 col-sm-9">
                                                <asp:DropDownList ID="ddlPeriod" runat="server" CssClass="field-medium form-control form-select" TabIndex="16">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblFrom" runat="server" AssociatedControlID="txtFrom" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="ltFrom" runat="server" Text="<%$ Resources:lbl_From%>"></asp:Literal>
                                            </asp:Label><div class="col-md-8 col-sm-9">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtFrom" runat="server" CssClass="form-control" TabIndex="17"></asp:TextBox>
                                                    <uc1:CalendarLookup ID="calFromDate" runat="server" LinkedControl="txtFrom" HLevel="4" TabIndex="18"></uc1:CalendarLookup>
                                                </div>
                                            </div>

                                        </div>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblTo" runat="server" AssociatedControlID="txtTo" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="ltTO" runat="server" Text="<%$ Resources:lbl_To%>"></asp:Literal>
                                            </asp:Label><div class="col-md-8 col-sm-9">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtTo" runat="server" CssClass="form-control" TabIndex="19"></asp:TextBox><uc1:CalendarLookup ID="calTodate" runat="server" LinkedControl="txtTo" HLevel="4" TabIndex="20"></uc1:CalendarLookup>
                                                </div>
                                            </div>

                                            <asp:CustomValidator ID="CustVldDate" runat="server" Display="None" SetFocusOnError="true"></asp:CustomValidator>
                                        </div>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblDueDateFrom" runat="server" AssociatedControlID="txtDueDateFrom" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="ltDueDateFrom" runat="server" Text="<%$ Resources:lbl_DueDateFrom%>"></asp:Literal>
                                            </asp:Label><div class="col-md-8 col-sm-9">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtDueDateFrom" runat="server" CssClass="form-control" TabIndex="22"></asp:TextBox><uc1:CalendarLookup ID="calDueDateFromDate" runat="server" LinkedControl="txtDueDateFrom" HLevel="4" TabIndex="23"></uc1:CalendarLookup>
                                                </div>
                                            </div>

                                        </div>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblDueDateTo" runat="server" AssociatedControlID="txtDueDateTo" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="ltDueDateTo" runat="server" Text="<%$ Resources:lbl_DueDateTo%>"></asp:Literal>
                                            </asp:Label><div class="col-md-8 col-sm-9">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtDueDateTo" runat="server" CssClass="form-control" TabIndex="24"></asp:TextBox><uc1:CalendarLookup ID="calDueDateTodate" runat="server" LinkedControl="txtDueDateTo" HLevel="4" TabIndex="25"></uc1:CalendarLookup>
                                                </div>
                                            </div>

                                            <asp:CustomValidator ID="CustVldDueDate" runat="server" Display="None" SetFocusOnError="true"></asp:CustomValidator>
                                        </div>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblAccountType" runat="server" AssociatedControlID="AccountType" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:lbl_AccType%>"></asp:Literal>
                                            </asp:Label><div class="col-md-8 col-sm-9">
                                                <asp:DropDownList ID="AccountType" runat="server" TabIndex="21" CssClass="field-medium form-control form-select">
                                                    <asp:ListItem Text="(all)" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Client" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="Insurer" Value="4"></asp:ListItem>
                                                    <asp:ListItem Text="Agent" Value="5"></asp:ListItem>
                                                    <asp:ListItem Text="Premium Finance" Value="6"></asp:ListItem>
                                                    <asp:ListItem Text="Fees" Value="7"></asp:ListItem>
                                                    <asp:ListItem Text="Discount" Value="8"></asp:ListItem>
                                                    <asp:ListItem Text="Commission" Value="9"></asp:ListItem>
                                                    <asp:ListItem Text="Sub Agent" Value="10"></asp:ListItem>
                                                    <asp:ListItem Text="Write Off" Value="21"></asp:ListItem>
                                                    <asp:ListItem Text="Extra" Value="22"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </div>

                        </asp:Panel>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnFindNow" EventName="Click"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="btnNewsearch" EventName="Click"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="btnReverse" EventName="Click"></asp:AsyncPostBackTrigger>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div id="tab-Amount" class="tab-pane animated fadeIn" role="tabpanel">
                <button class="accordion-button acc-style" type="button" data-bs-toggle="collapse" data-bs-target="#panelsStayOpen_collapseFour" aria-expanded="true" aria-controls="panelsStayOpen_collapseFour">
                </button>
                <div id="panelsStayOpen_collapseFour" class="accordion-collapse collapse show" aria-labelledby="headingOne" data-bs-parent="#accordionExample">
                    <div class="accordion-body">
                        <asp:Panel ID="PnlAmountCol" runat="server" DefaultButton="btnFindNow">

                            <div class="form-horizontal">

                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblCurrency" runat="server" AssociatedControlID="CurrencyType" class="col-md-4 col-sm-3 control-label">
                                        <asp:Literal ID="ltRecoveryTypeforReInsurance" runat="server" Text="<%$ Resources:lbl_Currency%>"></asp:Literal>
                                    </asp:Label><div class="col-md-8 col-sm-9">
                                        <NexusProvider:LookupList ID="CurrencyType" runat="server" DataItemValue="Code" DataItemText="Description" TabIndex="23" Sort="ASC" ListType="pmlookup" ListCode="CURRENCY" CssClass="field-medium form-control form-select" DefaultText="<%$ Resources:lbl_CurrencydeafultText%>"></NexusProvider:LookupList>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblTransactionCurrencyaccountbalance" runat="server" AssociatedControlID="txtTransactionCurrencyaccountbalance" class="col-md-4 col-sm-3 control-label">
                                        <asp:Literal ID="ltTransactionCurrencyaccountbalance" runat="server" Text="<%$ Resources:lbl_TransactionCurrencyaccountbalance%>"></asp:Literal>
                                    </asp:Label><div class="col-md-8 col-sm-9">
                                        <asp:TextBox ID="txtTransactionCurrencyaccountbalance" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblAmount" runat="server" AssociatedControlID="txtAmount" class="col-md-4 col-sm-3 control-label">
                                        <asp:Literal ID="ltAmount" runat="server" Text="<%$ Resources:lbl_Amount%>"></asp:Literal>
                                    </asp:Label><div class="col-md-8 col-sm-9">
                                        <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" TabIndex="24"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblTolerance" runat="server" AssociatedControlID="txtTolerance" class="col-md-4 col-sm-3 control-label">
                                        <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:lbl_Tolerance%>"></asp:Literal>
                                    </asp:Label><div class="col-md-8 col-sm-9">
                                        <asp:TextBox ID="txtTolerance" runat="server" CssClass="form-control" TabIndex="25"></asp:TextBox>
                                    </div>
                                </div>

                            </div>

                        </asp:Panel>
                    </div>
                </div>
            </div>
            <div id="tab-Reference" class="tab-pane animated fadeIn" role="tabpanel">
                <div class="col-lg-1 col-md-1 col-sm-1 float-end">
                    <button class="accordion-button acc-style" type="button" data-bs-toggle="collapse" data-bs-target="#panelsStayOpen_collapseFive" aria-expanded="true" aria-controls="panelsStayOpen_collapseOne">
                    </button>
                </div>
                <asp:Panel ID="PnlReferenceCol" runat="server" DefaultButton="btnFindNow">
                    <div class="form-horizontal">
                        <div id="panelsStayOpen_collapseFive" class="accordion-collapse collapse show" aria-labelledby="panelsStayOpen-headingOne">
                            <div class="accordion-body">
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblPolicyNumber" runat="server" AssociatedControlID="txtPolicyNumber" class="col-md-4 col-sm-3 control-label">
                                        <asp:Literal ID="ltPolicyNumber" runat="server" Text="<%$ Resources:lbl_PolicyNumber%>"></asp:Literal>
                                    </asp:Label><div class="col-md-8 col-sm-9">
                                        <asp:TextBox ID="txtPolicyNumber" runat="server" CssClass="form-control" TabIndex="27"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblPurchaseOrderNo" runat="server" AssociatedControlID="txtPurchaseOrderNo" class="col-md-4 col-sm-3 control-label">
                                        <asp:Literal ID="ltPurchaseOrderNo" runat="server" Text="<%$ Resources:lbl_PurchaseOrderNo%>"></asp:Literal>
                                    </asp:Label><div class="col-md-8 col-sm-9">
                                        <asp:TextBox ID="txtPurchaseOrderNo" runat="server" CssClass="form-control" TabIndex="28"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblPurChaseInvoice" runat="server" AssociatedControlID="txtPurChaseInvoice" class="col-md-4 col-sm-3 control-label">
                                        <asp:Literal ID="ltPurChaseInvoice" runat="server" Text="<%$ Resources:lbl_PurChaseInvoice%>"></asp:Literal>
                                    </asp:Label><div class="col-md-8 col-sm-9">
                                        <asp:TextBox ID="txtPurChaseInvoice" runat="server" CssClass="form-control" TabIndex="29"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblMediaRef" runat="server" AssociatedControlID="txtMediaRef" class="col-md-4 col-sm-3 control-label">
                                        <asp:Literal ID="ltMediaRef" runat="server" Text="<%$ Resources:lbl_MediaRef%>"></asp:Literal>
                                    </asp:Label><div class="col-md-8 col-sm-9">
                                        <asp:TextBox ID="txtMediaRef" runat="server" CssClass="form-control" TabIndex="30"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblClaimNumber" runat="server" AssociatedControlID="txtClaimNumber" class="col-md-4 col-sm-3 control-label">
                                        <asp:Literal ID="ltClaimNumber" runat="server" Text="<%$ Resources:lbl_ClaimNumber%>"></asp:Literal>
                                    </asp:Label><div class="col-md-8 col-sm-9">
                                        <asp:TextBox ID="txtClaimNumber" runat="server" CssClass="form-control" TabIndex="31"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblAlternateRef" runat="server" AssociatedControlID="txtAlternateRef" class="col-md-4 col-sm-3 control-label">
                                        <asp:Literal ID="ltAlternateRef" runat="server" Text="<%$ Resources:lbl_AlternateRef%>"></asp:Literal>
                                    </asp:Label><div class="col-md-8 col-sm-9">
                                        <asp:TextBox ID="txtAlternateRef" runat="server" CssClass="form-control" TabIndex="32"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblBGRef" runat="server" AssociatedControlID="txtBGRef" class="col-md-4 col-sm-3 control-label">
                                        <asp:Literal ID="ltBGRef" runat="server" Text="<%$ Resources:lbl_BGRef%>"></asp:Literal>
                                    </asp:Label><div class="col-md-8 col-sm-9">
                                        <asp:TextBox ID="txtBGRef" runat="server" CssClass="form-control" TabIndex="33"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblOperatorName" runat="server" AssociatedControlID="ddlOperatorName" class="col-md-4 col-sm-3 control-label">
                                        <asp:Literal ID="ltOperatorName" runat="server" Text="<%$ Resources:lbl_OperatorName%>"></asp:Literal>
                                    </asp:Label><div class="col-md-8 col-sm-9">
                                        <asp:DropDownList ID="ddlOperatorName" runat="server" TabIndex="34" CssClass="field-medium form-control form-select">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblDepartment" runat="server" AssociatedControlID="Department" class="col-md-4 col-sm-3 control-label">
                                        <asp:Literal ID="ltDepartment" runat="server" Text="<%$ Resources:lbl_Department%>"></asp:Literal>
                                    </asp:Label><div class="col-md-8 col-sm-9">
                                        <NexusProvider:LookupList ID="Department" runat="server" TabIndex="35" DataItemValue="Code" DataItemText="Description" Sort="ASC" ListType="pmlookup" ListCode="DEPARTMENT" CssClass="field-medium form-control form-select" DefaultText="<%$ Resources:lbl_departmentDefaultText%>"></NexusProvider:LookupList>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label runat="Server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtInsuredAccountCode" Text="<%$ Resources:lbl_InsuredAccountCode %>" ID="lblbtnInsuredAccountCode"></asp:Label><div class="col-md-8 col-sm-9">
                                        <div class="input-group">
                                            <asp:TextBox ID="txtInsuredAccountCode" runat="server" TabIndex="37" CssClass="form-control"></asp:TextBox><span class="input-group-btn"><asp:LinkButton ID="btnInsuredAccountCode" runat="server" TabIndex="36" OnClientClick="tb_show(null , '../Modal/FindAccount.aspx?modal=true&KeepThis=true&FromPage=IACC&TB_iframe=true&height=500&width=800' , null);return false;" SkinID="btnModal"><i class="fa fa-search"></i></asp:LinkButton></span>
                                        </div>
                                    </div>
                                    <asp:HiddenField ID="hiddenInsuredAccountCode" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="hiddenInsuredAccountKey" runat="server"></asp:HiddenField>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>
</div>
            <div id="divButtons" class="card-footer">
                <div id="panelsStayOpen-collapseOne" class="accordion-collapse collapse show" aria-labelledby="panelsStayOpen-headingOne">
                                             <div class="accordion-body">
                <asp:UpdatePanel ID="UpdSearchTransaction" runat="server">
                    <ContentTemplate>
                        <asp:LinkButton ID="btnNewsearch" runat="server" TabIndex="40" Text="<%$ Resources:btnNewsearch %>" CausesValidation="false" SkinID="btnSecondary"></asp:LinkButton>
                        <asp:LinkButton ID="btnCancel" runat="server" TabIndex="41" Text="<%$ Resources:btnCancel %>" SkinID="btnSecondary"></asp:LinkButton>
                        <asp:LinkButton ID="btnReverse" runat="server" TabIndex="38" Text="<%$ Resources:btnReverse %>" Enabled="false" SkinID="btnPrimary"></asp:LinkButton>
                        <asp:LinkButton ID="btnFindNow" runat="server" TabIndex="39" Text="<%$ Resources:btnFindNow %>" SkinID="btnPrimary"></asp:LinkButton>
                        <asp:LinkButton ID="btnAllocate" runat="server" TabIndex="42" Text="<%$ Resources:btnAllocate %>" SkinID="btnPrimary"></asp:LinkButton>
                        <asp:LinkButton ID="btnAddTrasaction" runat="server" TabIndex="43" Text="<%$ Resources:btnAddTrasaction %>" Visible="false" CausesValidation="false" SkinID="btnPrimary"></asp:LinkButton>

                        <asp:Label ID="lblInstallmentTransectionAllocationError" Text="<%$ Resources:lblInstallmentTransectionAllocationError%>" CssClass="error" runat="server" Visible="false"></asp:Label>
                    </ContentTemplate>

                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAllocate" EventName="Click"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="btnAddTrasaction" EventName="Click"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="btnFindNow" EventName="Click"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="btnNewsearch" EventName="Click"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="btnReverse" EventName="Click"></asp:AsyncPostBackTrigger>
                    </Triggers>
                </asp:UpdatePanel>
                <nexus:ProgressIndicator ID="UpSearchTransaction" OverlayCssClass="updating" AssociatedUpdatePanelID="UpdSearchTransaction" runat="server">
                    <progresstemplate>
                                </progresstemplate>
                </nexus:ProgressIndicator>
             </div> </div></div>
        </div>  

              </div> 

        </div>
        <nexus:WildCardValidator ID="vldWildCard" AllowWildCardAtEndErrorMessage="<%$ Resources:lbl_WildCardAtEnd_Error %>" NoWildCardErrorMessage="<%$ Resources:lbl_NoWildCard_Error %>" ControlsToValidate="txtAccountCode,txtAccountBalance,txtDocumentRef,txtAmount,txtPolicyNumber,txtPurchaseOrderNo,txtPurChaseInvoice,txtMediaRef,txtClaimNumber,txtAlternateRef,txtBGRef,txtInsuredAccountCode" Condition="Auto" Display="none" runat="server" EnableClientScript="true">
        </nexus:WildCardValidator>
        <asp:ValidationSummary ID="ValidationSummary" HeaderText="<%$ Resources:lbl_ValidationSummary %>" DisplayMode="BulletList" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
        <nexus:ColumnSelectorExtender ID="ColumnSelectorExtender" runat="server" CssClass="p-v-sm" ControlToExtend="gvGetTransactiondetails,gvDocRefTransactions,gvTransactionForAccount" LinkText="<%$ Resources:lbl_SelectColumns_LinkText %>" ButtonText="<%$ Resources:lbl_SelectColumns_ButtonText %>" EnableViewState="true" Style="display: none;"></nexus:ColumnSelectorExtender>
        <div class="grid-card table-responsive">
            <asp:UpdatePanel ID="UpdDocRefTransaction" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                <ContentTemplate>
                    <asp:GridView ID="gvGetTransactiondetails" runat="server" AllowSorting="true" AutoGenerateColumns="false" GridLines="None" AllowPaging="true" DataKeyNames="TransDetailKeys" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:UpdatePanel ID="UpdTransactionDetails" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False">
                                        <ContentTemplate>
                                            <asp:CheckBox ID="chkselectedTransaction" runat="server"  AutoPostBack="true" Checked='<%# Eval("IsSelected") %>' OnCheckedChanged="chkselectedTransaction_CheckedChanged" Text=" " CssClass="asp-check"></asp:CheckBox>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="chkselectedTransaction" EventName="CheckedChanged"></asp:AsyncPostBackTrigger>
                                            <asp:AsyncPostBackTrigger ControlID="btnFindNow" EventName="Click"></asp:AsyncPostBackTrigger>
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    <nexus:ProgressIndicator ID="UpTransactionDetails" OverlayCssClass="updating" AssociatedUpdatePanelID="UpdTransactionDetails" runat="server">
                                        <progresstemplate>
                                                        </progresstemplate>
                                    </nexus:ProgressIndicator>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_TransDetailKeys %>" DataField="TransDetailKeys" SortExpression="TransDetailKeys" HeaderStyle-CssClass="TransDetailKey" ItemStyle-CssClass="TransDetailKey"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_BranchKey %>" Visible="false" DataField="BranchKey" SortExpression="BranchKey"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_BalanceType %>" Visible="false" DataField="BalanceType" SortExpression="BalanceType"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_shortname %>" DataField="Account" SortExpression="Account"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_DocRef %>" DataField="DocRef" SortExpression="DocRef"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_AltRef %>" Visible="false" DataField="AltRef" SortExpression="AltRef"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_EffectiveDate %>" Visible="false" DataField="EffectiveDate" HtmlEncode="False" DataFormatString="{0:d}" SortExpression="EffectiveDate"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_TransDate %>" DataField="TransDate" HtmlEncode="False" DataFormatString="{0:d}" SortExpression="TransDate"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_DueDate %>" DataField="DueDate" HtmlEncode="False" DataFormatString="{0:d}" SortExpression="DueDate"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_MediaType %>" Visible="false" DataField="MediaType" SortExpression="MediaType"></asp:BoundField>
                            <nexus:BoundField HeaderText="<%$ Resources:lbl_AccountAmount %>" DataField="CurrencyAmount" DataType="Currency" SortExpression="CurrencyAmount"></nexus:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_PrimarySettled %>" Visible="false" DataField="PrimarySettled" SortExpression="PrimarySettled"></asp:BoundField>
                            <nexus:BoundField HeaderText="<%$ Resources:lbl_OutstandingAmount %>" DataField="OutstandingAmount" DataType="Currency" SortExpression="OutstandingAmount"></nexus:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_PaidDate %>" Visible="false" DataField="PaidDate" HtmlEncode="False" DataFormatString="{0:d}" SortExpression="PaidDate"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_DocTypeId %>" Visible="false" DataField="DocumentTypeCode" SortExpression="DocumentTypeCode"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_Ref %>" DataField="Reference" SortExpression="Reference"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_OperaName %>" Visible="false" DataField="OperatorName" SortExpression="OperatorName"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_Duration %>" Visible="false" DataField="Period" SortExpression="Period"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_DocumentGroupId %>" Visible="false" DataField="DocTypeGroupCode" SortExpression="DocTypeGroupCode"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_Client %>" Visible="false" DataField="Client" SortExpression="Client"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_ClientCode %>" DataField="ClientCode" SortExpression="ClientCode"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_MediaReference %>" DataField="MediaRef" SortExpression="MediaRef"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_AccountKey %>" Visible="false" DataField="AccountKey" SortExpression="AccountKey"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_PayeeName %>" Visible="false" DataField="PayeeName" SortExpression="PayeeName"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_UnderwritingYear %>" Visible="false" DataField="UnderwritingYear" SortExpression="UnderwritingYear"></asp:BoundField>
                            <nexus:BoundField HeaderText="<%$ Resources:lbl_AccountOutStandingAmount %>" Visible="false" DataField="AccountOutStandingAmount" DataType="Currency" SortExpression="AccountOutStandingAmount"></nexus:BoundField>
                            <nexus:BoundField HeaderText="<%$ Resources:lbl_CurrencyAmount %>" Visible="false" DataField="CurrencyAmount" DataType="Currency" SortExpression="CurrencyAmount"></nexus:BoundField>
                            <nexus:BoundField HeaderText="<%$ Resources:lbl_OutStandingCurrencyAmount %>" Visible="false" DataField="OutStandingCurrencyAmount" DataType="Currency" SortExpression="OutStandingCurrencyAmount"></nexus:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_BGReference %>" Visible="false" DataField="BGRef" SortExpression="BGRef"></asp:BoundField>
                            <%--wpr34-CR1--%>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_CashListKey %>" Visible="false" DataField="CashListKey"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_IsLeadAgent %>" Visible="false" DataField="IsLeadAgent"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_IsSplitReceipt %>" Visible="false" DataField="IsSplitReceipt"></asp:BoundField>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <div class="rowMenu">
                                        <ol class="list-inline no-margin">
                                            <li class="dropdown no-padding"><a href="#" title="Action Menu" md-ink-ripple="" data-bs-toggle="dropdown" class="md-btn grey-100 md-flat md-btn-circle"><i class="fa fa-ellipsis-v" aria-hidden="true"></i></a>
                                                <ol id="menu_<%# Eval("TransdetailId") %>" class="dropdown-menu dropdown-menu-scale pull-right pull-up top text-color">
                                                    <li id="liDrillDoc" runat="server">
                                                        <asp:LinkButton ID="lnkbtnDrillDoc" runat="server" CausesValidation="False" Text="<%$ Resources:lbl_DrillDoc %>"></asp:LinkButton>
                                                    </li>
                                                    <li id="liExpand" runat="server">
                                                        <asp:LinkButton ID="lnkbtnExpand" runat="server" CausesValidation="False" Text="<%$ Resources:lbl_Expand %>" CommandName="Expand"></asp:LinkButton>
                                                    </li>
                                                    <li id="liViewAllocation" runat="server">
                                                        <asp:LinkButton ID="lblhypViewAllocation" runat="server" Text="<%$ Resources:btnAllocation %>"></asp:LinkButton>
                                                    </li>
                                                    <li id="li1" runat="server">
                                                        <asp:LinkButton ID="lblDocument" runat="server" Text="<%$ Resources:btnReceiptDocument %>" CommandName="GenerateDocument" ></asp:LinkButton>
                                                    </li>
                                                    <li id="li2" runat="server">
                                                        <asp:LinkButton ID="lnkEmail" runat="server" Text="<%$ Resources:btnEmailReceipt %>" CommandName="EMAILLETTER" ></asp:LinkButton>
                                                    </li>
                                                </ol>
                                            </li>
                                        </ol>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:GridView ID="gvDocRefTransactions" runat="server" AllowSorting="true" AutoGenerateColumns="false" GridLines="None" AllowPaging="True" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>" Visible="false">
                        <Columns>

                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:UpdatePanel ID="UpdTransactionDetails1" UpdateMode="Conditional" ChildrenAsTriggers="False">
                                        <ContentTemplate>
                                            <asp:CheckBox ID="chkselectedTransaction1" runat="server" Checked='<%# Eval("IsSelected") %>' Visible="false" Text=" " CssClass="asp-check"></asp:CheckBox>
                                        </ContentTemplate>

                                    </asp:UpdatePanel>
                                    <nexus:ProgressIndicator ID="UpTransactionDetails" OverlayCssClass="updating" AssociatedUpdatePanelID="UpdTransactionDetails" runat="server">
                                        <progresstemplate>
                                                        </progresstemplate>
                                    </nexus:ProgressIndicator>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_TransDetailKeys %>" DataField="TransDetailKeys"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_BranchKey %>" Visible="false" DataField="BranchKey"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_BalanceType %>" Visible="false" DataField="BalanceType"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_shortname %>" DataField="Account"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_DocRef %>" DataField="DocRef"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_AltRef %>" Visible="false" DataField="AltRef"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_EffectiveDate %>" DataField="EffectiveDate" Visible="false" HtmlEncode="False" DataFormatString="{0:d}"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_TransDate %>" DataField="TransDate" HtmlEncode="False" DataFormatString="{0:d}"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_DueDate %>" DataField="DueDate" HtmlEncode="False" DataFormatString="{0:d}" SortExpression="DueDate"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_MediaType %>" Visible="false" DataField="MediaType"></asp:BoundField>
                            <nexus:BoundField HeaderText="<%$ Resources:lbl_AccountAmount %>" DataField="CurrencyAmount" DataType="Currency"></nexus:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_PrimarySettled %>" Visible="false" DataField="PrimarySettled"></asp:BoundField>
                            <nexus:BoundField HeaderText="<%$ Resources:lbl_OutstandingAmount %>" DataField="OutstandingAmount" DataType="Currency"></nexus:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_PaidDate %>" DataField="PaidDate" HtmlEncode="False" Visible="false" DataFormatString="{0:d}"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_DocTypeId %>" Visible="false" DataField="DocumentTypeCode"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_Ref %>"  DataField="Reference"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_OperaName %>" Visible="false" DataField="OperatorName"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_Duration %>" Visible="false" DataField="Period"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_DocumentGroupId %>" Visible="false" DataField="DocTypeGroupCode"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_Client %>" Visible="false" DataField="Client"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_ClientCode %>" DataField="ClientCode"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_MediaReference %>" DataField="MediaRef"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_AccountKey %>" Visible="false" DataField="AccountKey"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_PayeeName %>" Visible="false" DataField="PayeeName"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_UnderwritingYear %>" Visible="false" DataField="UnderwritingYear"></asp:BoundField>
                            <nexus:BoundField HeaderText="<%$ Resources:lbl_AccountOutStandingAmount %>" Visible="false" DataField="AccountOutStandingAmount" DataType="Currency"></nexus:BoundField>
                            <nexus:BoundField HeaderText="<%$ Resources:lbl_CurrencyAmount %>" Visible="false" DataField="CurrencyAmount" DataType="Currency"></nexus:BoundField>
                            <nexus:BoundField HeaderText="<%$ Resources:lbl_OutStandingCurrencyAmount %>" Visible="false" DataField="OutStandingCurrencyAmount" DataType="Currency"></nexus:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_BGReference %>" Visible="false" DataField="BGRef"></asp:BoundField>
                            <%--wpr34-CR1--%>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_CashListKey %>" Visible="false" DataField="CashListKey"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_IsLeadAgent %>" Visible="false" DataField="IsLeadAgent"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_IsSplitReceipt %>" Visible="false" DataField="IsSplitReceipt"></asp:BoundField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <div class="rowMenu">
                                        <ol class="list-inline no-margin">
                                            <li class="dropdown no-padding"><a href="#" title="Action Menu" md-ink-ripple="" data-bs-toggle="dropdown" class="md-btn grey-100 md-flat md-btn-circle"><i class="fa fa-ellipsis-v" aria-hidden="true"></i></a>
                                                <ol id="menu_<%# Eval("TransdetailId") %>" class="dropdown-menu dropdown-menu-scale pull-right pull-up top text-color">
                                                    <li id="liDrillDoc" runat="server">
                                                        <asp:LinkButton ID="lnkbtnDrillDoc" runat="server" CausesValidation="False" Text="<%$ Resources:lbl_DrillDoc %>"></asp:LinkButton>
                                                    </li>
                                                    <li id="liDrillAccount" runat="server">
                                                        <asp:LinkButton ID="lnkbtnDrillAccount" runat="server" CausesValidation="False"></asp:LinkButton>
                                                    </li>
                                                    <li id="liViewAllocation" runat="server">
                                                        <asp:LinkButton ID="lblhypViewAllocation" runat="server" Text="<%$ Resources:btnAllocation %>"></asp:LinkButton>
                                                    </li>
                                                </ol>
                                            </li>
                                        </ol>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:GridView ID="gvTransactionForAccount" runat="server" AllowSorting="true" AutoGenerateColumns="false" GridLines="None" AllowPaging="True" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>" Visible="false">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:UpdatePanel ID="UpdTransactionDetails1" UpdateMode="Conditional" ChildrenAsTriggers="False">
                                        <ContentTemplate>
                                            <asp:CheckBox ID="chkselectedTransaction1" runat="server" Checked='<%# Eval("IsSelected") %>' Visible="false" Text=" " CssClass="asp-check"></asp:CheckBox>
                                        </ContentTemplate>

                                    </asp:UpdatePanel>
                                    <nexus:ProgressIndicator ID="UpTransactionDetails" OverlayCssClass="updating" AssociatedUpdatePanelID="UpdTransactionDetails" runat="server">
                                        <progresstemplate>
                                        </progresstemplate>
                                    </nexus:ProgressIndicator>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_TransDetailKeys %>" Visible="false" DataField="TransDetailKeys" SortExpression="TransDetailKeys"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_BranchKey %>" Visible="false" DataField="BranchKey" SortExpression="BranchKey"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_shortname %>" DataField="Account" SortExpression="Account"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_DocRef %>" DataField="DocRef" SortExpression="DocRef"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_AltRef %>" Visible="false" DataField="AltRef" SortExpression="AltRef"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_EffectiveDate %>" Visible="false" DataField="EffectiveDate" HtmlEncode="False" DataFormatString="{0:d}" SortExpression="EffectiveDate"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_TransDate %>" DataField="TransDate" HtmlEncode="False" DataFormatString="{0:d}" SortExpression="TransDate"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_DueDate %>" DataField="DueDate" HtmlEncode="False" DataFormatString="{0:d}" SortExpression="DueDate"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_MediaType %>" Visible="false" DataField="MediaType" SortExpression="MediaType"></asp:BoundField>
                            <nexus:BoundField HeaderText="<%$ Resources:lbl_AccountAmount %>" DataField="CurrencyAmount" DataType="Currency" SortExpression="CurrencyAmount"></nexus:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_PrimarySettled %>" Visible="false" DataField="PrimarySettled" SortExpression="PrimarySettled"></asp:BoundField>
                            <nexus:BoundField HeaderText="<%$ Resources:lbl_OutstandingAmount %>" DataField="OutstandingAmount" DataType="Currency" SortExpression="OutstandingAmount"></nexus:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_PaidDate %>" Visible="false" DataField="PaidDate" HtmlEncode="False" DataFormatString="{0:d}" SortExpression="PaidDate"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_DocTypeId %>" Visible="false" DataField="DocumentTypeCode" SortExpression="DocumentTypeCode"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_Ref %>"  DataField="Reference" SortExpression="Reference"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_OperaName %>" Visible="false" DataField="OperatorName" SortExpression="OperatorName"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_Duration %>" Visible="false" DataField="Period" SortExpression="Period"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_DocumentGroupId %>" Visible="false" DataField="DocTypeGroupCode" SortExpression="DocTypeGroupCode"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_Client %>" Visible="false" DataField="Client" SortExpression="Client"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_ClientCode %>" DataField="ClientCode" SortExpression="ClientCode"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_MediaReference %>"  DataField="MediaRef" SortExpression="MediaRef"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_AccountKey %>" Visible="false" DataField="AccountKey" SortExpression="AccountKey"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_PayeeName %>" Visible="false" DataField="PayeeName" SortExpression="PayeeName"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_UnderwritingYear %>" Visible="false" DataField="UnderwritingYear" SortExpression="UnderwritingYear"></asp:BoundField>
                            <nexus:BoundField HeaderText="<%$ Resources:lbl_AccountOutStandingAmount %>" Visible="false" DataField="AccountOutStandingAmount" DataType="Currency" SortExpression="AccountOutStandingAmount"></nexus:BoundField>
                            <nexus:BoundField HeaderText="<%$ Resources:lbl_CurrencyAmount %>" Visible="false" DataField="CurrencyAmount" DataType="Currency" SortExpression="CurrencyAmount"></nexus:BoundField>
                            <nexus:BoundField HeaderText="<%$ Resources:lbl_OutStandingCurrencyAmount %>" Visible="false" DataField="OutStandingCurrencyAmount" DataType="Currency" SortExpression="OutStandingCurrencyAmount"></nexus:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_BGReference %>" Visible="false" DataField="BGRef" SortExpression="BGRef"></asp:BoundField>
                            <%--wpr34-CR1--%>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_CashListKey %>" Visible="false" DataField="CashListKey"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_IsLeadAgent %>" Visible="false" DataField="IsLeadAgent"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_IsSplitReceipt %>" Visible="false" DataField="IsSplitReceipt"></asp:BoundField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <div class="rowMenu">
                                        <ol id="menu_<%# Eval("TransdetailId") %>" class="list-inline no-margin">
                                            <li id="liViewAllocation" runat="server">
                                                <asp:LinkButton ID="lblhypViewAllocation" runat="server" Text="<%$ Resources:btnAllocation %>" SkinID="btnGrid"></asp:LinkButton>
                                            </li>
                                        </ol>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="gvGetTransactiondetails" EventName="PageIndexChanging"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="gvGetTransactiondetails" EventName="RowCommand"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="gvGetTransactiondetails" EventName="RowCreated"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="gvGetTransactiondetails" EventName="RowDataBound"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="gvDocRefTransactions" EventName="RowCommand"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="gvDocRefTransactions" EventName="RowCreated"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="gvDocRefTransactions" EventName="RowDataBound"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="gvDocRefTransactions" EventName="PageIndexChanging"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="gvTransactionForAccount" EventName="PageIndexChanging"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="gvTransactionForAccount" EventName="RowCreated"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="gvTransactionForAccount" EventName="RowDataBound"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="gvTransactionForAccount" EventName="RowCommand"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="btnFindNow" EventName="Click"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click"></asp:AsyncPostBackTrigger>
                </Triggers>
            </asp:UpdatePanel>
            <nexus:ProgressIndicator ID="UpAccTransaction" OverlayCssClass="updating" AssociatedUpdatePanelID="UpdDocRefTransaction" runat="server">
                <progresstemplate>
                </progresstemplate>
            </nexus:ProgressIndicator>
        </div>

    </div>
</asp:Content>
