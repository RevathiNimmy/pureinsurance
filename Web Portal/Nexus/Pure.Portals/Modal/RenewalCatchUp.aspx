<%@ Page Title="" Language="VB" AutoEventWireup="false" CodeFile="RenewalCatchUp.aspx.vb" Inherits="Nexus.Modal_RenewalCatchUp" MasterPageFile="~/default.master"  %>


<%@ Register Src="../Controls/ProgressBar.ascx" TagName="ProgressBar" TagPrefix="uc3" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
     <script type="text/javascript"  src="../js/jquery.blockUI.js"></script>
      <script type="text/javascript">
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
    <div id="Modal_PolicyVersion">
        <asp:ScriptManager ID="smPolicyVersions" AsyncPostBackTimeout ="6000" runat="server"></asp:ScriptManager>
        <uc3:ProgressBar ID="ucProgressBar" runat="server"></uc3:ProgressBar>
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal runat="server" Text="Renewal Catch-Up" ID="ltPageHeading"></asp:Literal>
                </h1>
            </div>
            <div class="card-body clearfix">
                 <asp:UpdatePanel ID="updPolicyVersion" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="True">
                    <ContentTemplate>
                        <asp:Label ID="lblWantToRenew" runat="server"  Text="The monthly policy is pending for renewals. Do you want to renew?" class="h4 m-v-lg text-u"></asp:Label>
                        <asp:LinkButton ID="btnRenewYes" runat="server" CausesValidation="false" Text="Yes" SkinID="btnPrimary"></asp:LinkButton>
                        <asp:LinkButton ID="btnRenewNo" runat="server" CausesValidation="false" Text="No" SkinID="btnSecondary" OnClientClick="self.parent.tb_remove();" ></asp:LinkButton>
                        <br />
                        <br />
                        <div class="grid-card table-responsive">
                            <asp:GridView ID="grdPolicyVersions" runat="server" AutoGenerateColumns="False" GridLines="None" PageSize="15" AllowPaging="true" AllowSorting="false" PagerSettings-Mode="Numeric">
                                <Columns>
                                    <%--<asp:BoundField DataField="PolicyStatus" SortExpression="PolicyStatus" HeaderText="<%$ Resources:gvh_PolicyStatus %>" />--%>
                                    <asp:BoundField DataField="CoverFrom" SortExpression="CoverFrom" HeaderText="Cover From" DataFormatString="{0:d}"></asp:BoundField>
                                    <asp:BoundField DataField="RenewalDate" SortExpression="RenewalDate" HeaderText="Renewal Date" DataFormatString="{0:d}"></asp:BoundField>
                                    <asp:BoundField DataField="BillingMethod" SortExpression="BillingMethod" HeaderText="Billing Method"></asp:BoundField>
                                    <asp:BoundField DataField="Amount" SortExpression="Amount" HeaderText="Amount" ></asp:BoundField>
                                    <asp:BoundField DataField="Status" SortExpression="Status" HeaderText="Status" ></asp:BoundField>
                                    <%--<asp:BoundField DataField="TransactionDate" SortExpression="TransactionDate" HeaderText="" DataFormatString="{0:d}"></asp:BoundField>--%>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <br />
                        <br />
                        <asp:Label ID="lblRenewed" runat="server" Text="The Policy is successfully renewed upto the current period." class="h4 m-v-lg text-u"></asp:Label>
                        <asp:LinkButton ID="btnRenewedOk" runat="server" CausesValidation="false" Text="OK" SkinID="btnPrimary" OnClientClick="self.parent.tb_remove();" ></asp:LinkButton>

                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnRenewYes" EventName="Click"></asp:AsyncPostBackTrigger>
                        <%--<asp:AsyncPostBackTrigger ControlID="grdPolicyVersions" EventName="RowCommand"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="grdPolicyVersions" EventName="RowDataBound"></asp:AsyncPostBackTrigger>--%>
                    </Triggers>
                </asp:UpdatePanel>
                <nexus:ProgressIndicator ID="upPolicyVersion" OverlayCssClass="updating" AssociatedUpdatePanelID="updPolicyVersion" runat="server">
                        <ProgressTemplate>
                          </ProgressTemplate>
                </nexus:ProgressIndicator>
            </div>
           <%-- <div class="card-footer">
                <asp:LinkButton ID="btnCancel" runat="server" Text="<%$ Resources:lbl_btnBack%>" OnClientClick="self.parent.tb_remove();" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnNewQuote" runat="server" Text="<%$ Resources:lbl_btnNewQuote %>" Visible="false" SkinID="btnPrimary"></asp:LinkButton>
            </div>--%>
        </div>
        
    </div>
</asp:Content>

