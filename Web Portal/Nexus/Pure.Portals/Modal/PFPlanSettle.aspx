<%@ Page Title="" Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="PFPlanSettle.aspx.vb" Inherits="Nexus.Modal_PFPlanSettle" %>

<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<asp:Content ID="Content4" ContentPlaceHolderID="cntMainBody" runat="Server">
    <asp:ScriptManager runat="server"></asp:ScriptManager>
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
    </script>
    <div id="Modal_PlanSettlement">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblTitle" runat="server" Text="<%$ Resources:lbl_Title %>"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div class="list-group-item md-whiteframe-z0 b-l-info b-l-3x text-info-dk">
                    <i class="fa fa-info-circle i-16 m-r-sm"></i>
                    <asp:Label ID="lblMessage" runat="server" Text="<%$ Resources:lbl_Message %>"></asp:Label>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnCancel" runat="server" Text="<%$ Resources:btn_Cancel %>" CausesValidation="true" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnOk" runat="server" Text="<%$ Resources:btn_Ok %>" CausesValidation="true" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
        <asp:ValidationSummary ID="ValidationSummary" runat="server" DisplayMode="BulletList" ShowSummary="true" HeaderText="<%$Resources:lbl_EditInstalment_ValidationSummary %>" CssClass="validation-summary"></asp:ValidationSummary>

    </div>
</asp:Content>
