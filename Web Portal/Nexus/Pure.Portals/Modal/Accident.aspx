<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Accident.aspx.vb" Inherits="Nexus.Modal_Accident"
    MasterPageFile="~/default.master" %>

<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script language="javascript" type="text/javascript">

        function UpdateAccidentData() {
            debugger;
            //to Fire the Client Validation first
            Page_ClientValidate();

            var AccidentData;
            if (Page_IsValid == true) {

                //Mode
                AccidentData = document.getElementById('<%=txtMode.ClientID %>').value + ";";
                //AccidentDate
                AccidentData += document.getElementById('<%=txtAccidentDate.ClientID %>').value + ";";
                //AccidentDescription
                AccidentData += document.getElementById('<%=txtAccidentDescription.ClientID %>').value + ";";
                //IsAtFault
                AccidentData += document.getElementById('<%=chkIsAtFault.ClientID %>').checked + ";";
                //AccidentKey
                AccidentData += document.getElementById('<%=txtAccidentKey.ClientID %>').value + ";";

                self.parent.tb_remove();
                self.parent.ReceiveAccidentData(AccidentData, document.getElementById('<%=txtPostBackTo.ClientID %>').value);
            }
        }
    </script>

    <div id="Modal_Accident">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblTitle" runat="server" Text="<%$ Resources:lbl_Accident_Title %>"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblHeading" runat="server" Text="<%$ Resources:lbl_Heading %>"></asp:Label>

                    </legend>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblAccidentDate" runat="server" Text="<%$ Resources:lbl_AccidentDate %>" AssociatedControlID="txtAccidentDate" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="txtAccidentDate" runat="server" CssClass="field-mandatory field-date form-control"></asp:TextBox><uc1:CalendarLookup ID="calEventFromDate" runat="server" LinkedControl="txtAccidentDate" HLevel="1"></uc1:CalendarLookup>
                            </div>
                        </div>
                        <asp:RequiredFieldValidator ID="reqvldAccidentDate" Display="None" ControlToValidate="txtAccidentDate" runat="server" ErrorMessage="<%$ Resources:err_AccidentDate %>" SetFocusOnError="True" ValidationGroup="AccidentGroup"></asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="rangevldAccidentDate" runat="server" Type="Date" ErrorMessage="<%$ Resources:lbl_InvalidAccidentDateFormat %>" ControlToValidate="txtAccidentDate" SetFocusOnError="True" ValidationGroup="AccidentGroup" Display="None" MinimumValue="01/01/1900"></asp:RangeValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblAccidentDescription" runat="server" AssociatedControlID="txtAccidentDescription" Text="<%$ Resources:lbl_AccidentDescription %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtAccidentDescription" runat="server" CssClass="field-mandatory form-control"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="reqvldAccidentDescription" runat="server" Display="none" ControlToValidate="txtAccidentDescription" ErrorMessage="<%$ Resources:err_AccidentDescription %>" SetFocusOnError="True" ValidationGroup="AccidentGroup"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblIsAtFault" runat="server" AssociatedControlID="chkIsAtFault" Text="<%$ Resources:lbl_AccidentIsAtFault %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:CheckBox ID="chkIsAtFault" runat="server" Text=" " CssClass="asp-check"></asp:CheckBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnAddAccident" runat="server" Text="<%$ Resources:btn_Add %>" ValidationGroup="AccidentGroup" CausesValidation="true" OnClientClick="UpdateAccidentData()" SkinID="btnPrimary" style="margin-right:25px;"></asp:LinkButton>
                <asp:LinkButton ID="btnUpdateAccident" runat="server" Text="<%$ Resources:btn_Update %>" Visible="false" ValidationGroup="AccidentGroup" CausesValidation="true" OnClientClick="UpdateAccidentData()" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
        <asp:HiddenField ID="txtPostBackTo" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="txtMode" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="txtAccidentKey" runat="server"></asp:HiddenField>
        <asp:ValidationSummary ID="ValidationSummary" ShowSummary="true" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" ValidationGroup="AccidentGroup" CssClass="validation-summary"></asp:ValidationSummary>

    </div>
</asp:Content>
