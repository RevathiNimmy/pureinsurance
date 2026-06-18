<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Loyalty.aspx.vb" Inherits="Nexus.Modal_Loyalty"
    MasterPageFile="~/default.master" %>

<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script language="javascript" type="text/javascript">
        $(document).ready(function () {

            if (document.getElementById('<%=txtEndDate.ClientId %>').value == '00:00:00') {
                document.getElementById('<%=txtEndDate.ClientId %>').value = '';
            }
        });

        String.prototype.trim = function () {
            return this.replace(/^\s*/, "").replace(/\s*$/, "");
        }

        function CheckDuplicateRecord(src, args) {
            var oDDL;
            args.IsValid = true;
            var iCount = 0;
            var LoyaltyID = document.getElementById('<%=txtKey.ClientID %>').value;
            oDDL = document.getElementById('<%=LoyaltyScheme.ClientID %>');
            var sLoyaltyCollection = document.getElementById('<%=LoyaltyType.ClientID %>').value.split(";");

            if (sLoyaltyCollection.length > 0) {

                //Need this function to find the duplecate values in case of "Update"
                if (document.getElementById('<%=txtMode.ClientID %>').value == "Update") {
                    //Update Mode
                    //it replaces the value temporarily then count
                    sLoyaltyCollection[LoyaltyID] = oDDL.options[oDDL.selectedIndex].value;
                }

                for (var CountVar = 0; CountVar < sLoyaltyCollection.length; CountVar++) {
                    //Add Mode
                    if (document.getElementById('<%=txtMode.ClientID %>').value == "Add") {
                        if (oDDL.options[oDDL.selectedIndex].value.length != 0 && sLoyaltyCollection[CountVar] == oDDL.options[oDDL.selectedIndex].value) {
                            args.IsValid = false;
                        }
                    }
                    else {
                        //Update Mode
                        if (oDDL.options[oDDL.selectedIndex].value.length != 0 && sLoyaltyCollection[CountVar] == oDDL.options[oDDL.selectedIndex].value) {
                            iCount = iCount + 1;
                        }
                    }
                }
                if (iCount > 1 && document.getElementById('<%=txtMode.ClientID %>').value == "Update") {
                    //For Update Mode if validation fails
                    args.IsValid = false;
                }
            }
        }
        function UpdateLoyaltyData() {
            var LoyaltyData;
            var oDDL;
            //to Fire teh Client Validation first
            Page_ClientValidate();

            if (Page_IsValid == true) {
                //Mode
                LoyaltyData = document.getElementById('<%=txtMode.ClientID %>').value + ";";
                //LoyaltyScheme
                oDDL = document.getElementById('<%=LoyaltyScheme.ClientID %>');
                LoyaltyData += oDDL.options[oDDL.selectedIndex].value + ";";
                //MembershipNo
                LoyaltyData += document.getElementById('<%=txtMemberShipNo.ClientID %>').value + ";";
                //OtherRef
                LoyaltyData += document.getElementById('<%=txtOtherRef.ClientID %>').value + ";";
                //StartDate
                LoyaltyData += document.getElementById('<%=txtStartDate.ClientID %>').value + ";";
                //EndDate
                LoyaltyData += document.getElementById('<%=txtEndDate.ClientID %>').value + ";";
                //MainMember
                LoyaltyData += document.getElementById('<%=txtMainMember.ClientID %>').value + ";";
                //Active 
                LoyaltyData += document.getElementById('<%=chkActive.ClientID %>').checked + ";";
                //Key
                LoyaltyData += document.getElementById('<%=txtKey.ClientID %>').value + ";";

                self.parent.tb_remove();
                self.parent.ReceiveLoyaltyData(LoyaltyData, document.getElementById('<%=txtPostBackTo.ClientID %>').value);
            }
        }
    </script>

    <div id="Modal_Loyalty">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblTitle" runat="server" Text="<%$ Resources:lbl_Loyalty_Title %>"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblHeading1" runat="server" Text="<%$ Resources:lbl_Heading %>"></asp:Label></legend>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblLoyaltyScheme" runat="server" AssociatedControlID="LoyaltyScheme" Text="<%$ Resources:lbl_LoyaltyScheme %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <NexusProvider:LookupList ID="LoyaltyScheme" runat="server" DataItemValue="Code" DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="Loyalty_scheme" DefaultText="(Please Select)" CssClass="field-medium field-mandatory form-control form-select"></NexusProvider:LookupList>
                        </div>
                        <asp:RequiredFieldValidator ID="ReqLoyaltyScheme" runat="server" Display="none" ControlToValidate="LoyaltyScheme" ErrorMessage="<%$ Resources:lbl_ReqLoyaltyScheme %>" ValidationGroup="LoyaltyGroup" SetFocusOnError="True" InitialValue=""></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblMemberShipNo" runat="server" AssociatedControlID="txtMemberShipNo" Text="<%$ Resources:lbl_MemberShipNo %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtMemberShipNo" runat="server" CssClass="field-mandatory form-control"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="RqdMemberShipNoRequired" runat="server" Display="none" ControlToValidate="txtMemberShipNo" ErrorMessage="<%$ Resources:lbl_MemberShipNo %>" ValidationGroup="LoyaltyGroup" SetFocusOnError="True"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblOtherRef" runat="server" AssociatedControlID="txtOtherRef" Text="<%$ Resources:lbl_OtherRef %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtOtherRef" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lbl_StartDate" runat="server" Text="<%$ Resources:lbl_StartDate %>" AssociatedControlID="txtStartDate" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="txtStartDate" runat="server" CssClass="field-date field-mandatory form-control"></asp:TextBox><uc1:CalendarLookup ID="calStartDate" runat="server" LinkedControl="txtStartDate" HLevel="1"></uc1:CalendarLookup>
                            </div>
                        </div>
                        <asp:RequiredFieldValidator ID="vldStartDate" Display="None" ControlToValidate="txtStartDate" runat="server" ErrorMessage="<%$ Resources:lbl_EnterStartDate %>" SetFocusOnError="True" ValidationGroup="LoyaltyGroup"></asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="rangevldStartDate" runat="server" Display="none" ErrorMessage="<%$ Resources:lbl_InvalidStartDateFormat %>" Type="Date" MinimumValue="01/01/1900" ControlToValidate="txtStartDate" SetFocusOnError="True" ValidationGroup="LoyaltyGroup"></asp:RangeValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lbl_EndDate" runat="server" Text="<%$ Resources:lbl_EndDate %>" AssociatedControlID="txtEndDate" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="calEndDate" runat="server" LinkedControl="txtEndDate" HLevel="1"></uc1:CalendarLookup>
                            </div>
                        </div>
                        <asp:CompareValidator ID="cmpLoyaltiStartDate" runat="server" Type="Date" ControlToValidate="txtEndDate" ControlToCompare="txtStartDate" Display="None" ErrorMessage="<%$ Resources:lbl_InvalidEndDateFormat %>" ValidationGroup="LoyaltyGroup" SetFocusOnError="true" Operator="GreaterThanEqual"></asp:CompareValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblMainMember" runat="server" AssociatedControlID="txtMainMember" Text="<%$ Resources:lbl_MainMember %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtMainMember" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <div class="col-md-8 col-sm-9">
                            <asp:CheckBox ID="chkActive" runat="server" Text="Active" CssClass="asp-check"></asp:CheckBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblAddLoyalty" runat="server" AssociatedControlID="btnAddLoyalty" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="card-footer">

                <asp:LinkButton ID="btnUpdateLoyalty" runat="server" Text="<%$ Resources:lbl_btnUpdateLoyalty %>" Visible="false" ValidationGroup="LoyaltyGroup" CausesValidation="true" OnClientClick="UpdateLoyaltyData()" SkinID="btnPrimary"></asp:LinkButton>
                <asp:LinkButton ID="btnAddLoyalty" runat="server" Text="<%$ Resources:lbl_btnAddLoyalty %>" ValidationGroup="LoyaltyGroup" CausesValidation="true" OnClientClick="UpdateLoyaltyData()" SkinID="btnPrimary"></asp:LinkButton>

            </div>
        </div>
        <asp:CustomValidator ID="custValidator" runat="server" ErrorMessage="<%$ Resources:lbl_CustValidation %>" Display="None" ValidationGroup="LoyaltyGroup" ClientValidationFunction="CheckDuplicateRecord"></asp:CustomValidator>
        <asp:HiddenField ID="txtPostBackTo" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="txtMode" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="txtKey" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="LoyaltyType" runat="server"></asp:HiddenField>
        <asp:ValidationSummary ID="ValidationSummary" ShowSummary="true" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" ValidationGroup="LoyaltyGroup" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
