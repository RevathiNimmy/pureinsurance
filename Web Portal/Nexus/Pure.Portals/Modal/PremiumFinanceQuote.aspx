<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/default.master"
    CodeFile="PremiumFinanceQuote.aspx.vb" Inherits="Nexus.Modal_PremiumFinanceQuote" %>

<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<%@ Register Src="~/Controls/Instalments.ascx" TagName="Instalments" TagPrefix="uc8" %>

<asp:Content ContentPlaceHolderID="cntMainBody" runat="server" ID="cntMainBody">
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

    <div id="Modal_PremiumFinanceQuote">
        <div class="card">
            <div class="card-heading">
                <h1>Instalment Payment
                </h1>
            </div>
            <div class="card-body clearfix">
                <uc8:Instalments ID="ucInstalments" runat="server"></uc8:Instalments>
            </div>
            <asp:Panel ID="PanelButton" runat="server" CssClass="card-footer">
                <asp:LinkButton ID="btnSave" runat="server" ValidationGroup="grpOverRide" Text="<%$ Resources:btn_Save %>" SkinID="btnPrimary"></asp:LinkButton>
            </asp:Panel>
        </div>
        <asp:ValidationSummary ID="ValidationSummary1" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" ValidationGroup="grpOverRide" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
