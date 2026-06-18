<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="RenewalLapseReason.aspx.vb" Inherits="Nexus.Modal_RenewalLapseReason"
    Title="Renewal Lapse Reason" %>

<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">  
    <script src="../js/jquery.blockUI.js"></script>
  <script type="text/javascript">      
        function LapseConfirmation() {
           var sMsg_LapseConfirmation = '<%= sLapseConfirmation %>';
            Page_ClientValidate();
            if (Page_IsValid == true) {
                var answer = confirm(sMsg_LapseConfirmation)
                if (answer) {
                    return true;
                }
                else {
                    return false;

                }
            }
        }
    
      function beforeSubmit() {
          
            if (typeof (ValidatorOnSubmitCustom) == "function" && ValidatorOnSubmitCustom() == true) {
                if (theForm.__EVENTARGUMENT.value.indexOf("Page$") == -1 &&
                    theForm.__EVENTARGUMENT.value.indexOf("Sort$") == -1 &&
                    theForm.__EVENTTARGET.value == "") {
                    document.activeElement.blur();
                    disableScreen();
                    removeNumricFormatting();
                }
                else {
                    disableScreen();
                }
            }
        }

        function disableScreen() {
            $.blockUI({ message: '<div><img src="<%# ResolveClientUrl("~/App_themes/Internal/images/ajax-loader3.gif") %>" /></div>',
                css: {
                    position: 'absolute',
                    border: '0px',
                    left: '50%',
                    top: '50%',
                    width: '32',
                    height: '32'
                }
            });
        }
      
    </script>
  
    <div id="Modal_RenewalLapseReason">
        <div class="card">
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblReason" runat="server" Text="<%$ Resources:lbl_ReasonHeading %>"></asp:Label></legend>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblLapseReason" runat="server" AssociatedControlID="RenewalReasonDescription" Text="<%$ Resources:lbl_LapseReason %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <NexusProvider:LookupList ID="RenewalReasonDescription" runat="server" DataItemValue="Code" DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="Lapsed_Reason" DefaultText="(Please Select)" CssClass="field-medium form-control form-select"></NexusProvider:LookupList>
                        </div>
                        <asp:RequiredFieldValidator ID="RqdLapseReason" runat="server" InitialValue="" ControlToValidate="RenewalReasonDescription" ErrorMessage="<%$ Resources:lbl_ErrMsg_LapseReason %>" Display="none" SetFocusOnError="true" ValidationGroup="RenewalReasonGroup" Enabled="true"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnLapse" runat="server" Text="<%$ Resources:btn_Lapse %>" ValidationGroup="RenewalReasonGroup"  SkinID="btnPrimary"></asp:LinkButton>
            </div>

        </div>
        <asp:ValidationSummary ID="ValidationSummary" ShowSummary="true" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" ValidationGroup="RenewalReasonGroup" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
