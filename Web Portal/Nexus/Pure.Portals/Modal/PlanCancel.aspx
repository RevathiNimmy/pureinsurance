<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PlanCancel.aspx.vb" Inherits="Nexus.Modal_PlanCancel"
    MasterPageFile="~/default.master" %>

<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script type="text/javascript" language="javascript">
        function beforeSubmit() {
            if (typeof (ValidatorOnSubmitCustom) == "function" && ValidatorOnSubmitCustom() == true) {
                if (theForm.__EVENTARGUMENT.value.indexOf("Page$") == -1 &&
                    theForm.__EVENTARGUMENT.value.indexOf("Sort$") == -1 &&
                    theForm.__EVENTTARGET.value == "") {
                    if (document.activeElement != null) {
                        document.activeElement.blur();
                    }
                    disableScreen();
                }
                else {
                    disableScreen();
                }
                return true;
            }
            else {
                Page_BlockSubmit = !Page_BlockSubmit;
                return false;
            }
        }

        function BeginRequestHandlerForUpdatePanel(sender, args) {
            disableScreen();
        }

        function EndRequestHandlerForUpdatePanel(sender, args) {
            $.unblockUI();
            // Check to see if there's an error on this request.
            if (args.get_error() != undefined) {
                var msg = args.get_error().message.replace("Sys.WebForms.PageRequestManagerServerErrorException: ", "");
                msg = msg.replace("Sys.WebForms.PageRequestManagerParserErrorException: ", "");
                // Show the custom error. 
                // Here you can be creative and do whatever you want
                // with the exception (i.e. call a modalpopup and show 
                // a nicer error window). I will simply use 'alert'
                alert(msg);
                // Let the framework know that the error is handled, 
                //  so it doesn't throw the JavaScript alert.
                args.set_errorHandled(true);
            }
        }

        function disableScreen() {
            $.blockUI({
                message: '<div class="loader">' +
                    '<img id="loader" runat="server" src="~/App_Themes/Internal/images/loader.gif" alt="loader" />' +
                    '</div>',
                css: {
                    position: 'absolute',
                    border: '0px',
                    left: '50%',
                    top: '50%',
                    width: 'auto',
                    padding: '5px 10px',
                    height: 'auto',
                    background: '#fff',
                }
            });
        }

        function RedirectToPolicyCancel(sMessage) {
            var IsConfirm;

            IsConfirm = window.confirm(sMessage);

            if (IsConfirm) {

                self.parent.CheckResponseAndRedirect(IsConfirm);
            }
            else {


                __doPostBack('RedirectFinancePlan', 'RedirectFinancePlan');

            }
        }
    </script>
    <asp:ScriptManager runat="server"></asp:ScriptManager>
    <div id="Modal_PlanCancel">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Label ID="lblPlanCancel" runat="server" Text="<%$ Resources:lbl_PlanCancel %>"></asp:Label>
                </h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label runat="server" ID="lblPlanReason" Text="<%$ Resources:lbl_PlanReason %>" AssociatedControlID="ddlCancelReason" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <NexusProvider:LookupList ID="ddlCancelReason" runat="server" DataItemValue="Code" DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="PFPREMIUMFINANCE_CANCEL_REASON" DefaultText="<%$ Resources:lbl_None %>" CssClass="field-medium form-control form-select"></NexusProvider:LookupList>
                        </div>
                  <asp:RequiredFieldValidator ID="RqdCancelReason" runat="server" InitialValue="" ControlToValidate="ddlCancelReason"
                                    ErrorMessage="" Display="none" SetFocusOnError="true"
                                    ValidationGroup="CancelReasonGroup" Enabled="False" />
                    </div>
                </div>
                <div>
                    <asp:Label runat="server" CssClass="list-group-item md-whiteframe-z0 error b-l-3x" ID="lblConfirmMessage" Text="<%$ Resources:lbl_ConfirmMessage %>"></asp:Label>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnCancel" runat="server" Text="<%$ Resources:btnCancel %>" OnClientClick="javascript:self.parent.tb_remove();" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnOk" runat="server" Text="<%$ Resources:btnOk %>" SkinID="btnPrimary"></asp:LinkButton>
                
            </div>
        </div>
        <asp:ValidationSummary ID="ValidationSummary" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
