<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Conviction.aspx.vb" Inherits="Nexus.Modal_Conviction"
    MasterPageFile="~/default.master" %>

<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script language="javascript" type="text/javascript">
        function ConvertFindToDecimal() {
            var ctrl = document.getElementById('<%=txtConvFine.ClientID %>').value;
            var myNum = new Number(ctrl);
            if (myNum.toFixed(2) != 'NaN') {
                document.getElementById('<%=txtConvFine.ClientID %>').value = myNum.toFixed(2);
            }
        }

        function UpdateConvictionData() {
            var ConvictionData;
            var oDDL;
            //to Fire teh Client Validation first
            Page_ClientValidate();

            if (Page_IsValid == true) {
                //Mode
               
                ConvictionData = document.getElementById('<%=txtMode.ClientID %>').value + ";";
                //ConvictionType
                oDDL = document.getElementById('<%=ConvictionType.ClientID %>');
                ConvictionData += oDDL.options[oDDL.selectedIndex].value + ";";
                //ConvFine
                ConvictionData += document.getElementById('<%=txtConvFine.ClientID %>').value + ";";
                //ConvStatus
                oDDL = document.getElementById('<%=ConvictionStatus.ClientID %>');
                ConvictionData += oDDL.options[oDDL.selectedIndex].value + ";";
                //ConvDescription
                ConvictionData += document.getElementById('<%=txtConvDescription.ClientID %>').value + ";";
                //ConvDate
                ConvictionData += document.getElementById('<%=DATE__CONVICTION.ClientID %>').value + ";";
                //SentenceType
                oDDL = document.getElementById('<%=SentenceType.ClientID %>');
                ConvictionData += oDDL.options[oDDL.selectedIndex].value + ";";
                //SentenceDate
                ConvictionData += document.getElementById('<%=txtSentenceDate.ClientID %>').value + ";";
                //SentenceDesc
                ConvictionData += document.getElementById('<%=txtSentenceDescription.ClientID %>').value + ";";
                //SentenceDuration
                ConvictionData += document.getElementById('<%=txtSentenceDuration.ClientID %>').value + ";";
                //TimeUnit
                oDDL = document.getElementById('<%=TimeUnit.ClientID %>');
                ConvictionData += oDDL.options[oDDL.selectedIndex].value + ";";
                //AlcohalMsrMethod
                oDDL = document.getElementById('<%=AlcoholMsrMethod.ClientID %>');
                ConvictionData += oDDL.options[oDDL.selectedIndex].value + ";";
                //AlcohalLevel
                ConvictionData += document.getElementById('<%=txtAlcoholLevel.ClientID %>').value + ";";
                //PenaltyPoints
                ConvictionData += document.getElementById('<%=txtPenaltyPoints.ClientID %>').value + ";";
                //Key
                ConvictionData += document.getElementById('<%=txtKey.ClientID %>').value + ";";

                self.parent.tb_remove();
                self.parent.ReceiveConvictionData(ConvictionData, document.getElementById('<%=txtPostBackTo.ClientID %>').value);
            }
        }
    </script>

    <div id="Modal_Conviction">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblTitle" runat="server" Text="<%$ Resources:lbl_Conviction_Title %>"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblHeading1" runat="server" Text="<%$ Resources:lbl_Heading1 %>"></asp:Label></legend>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblConvType" runat="server" AssociatedControlID="ConvictionType" Text="<%$ Resources:lbl_ConvType %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <NexusProvider:LookupList ID="ConvictionType" runat="server" DataItemValue="Description" DataItemText="Description" Sort="ASC" ListType="Userdefined" ListCode="1114113" DefaultText="(Please Select)" CssClass="field-medium field-mandatory form-control form-select"></NexusProvider:LookupList>
                        </div>
                        <asp:RequiredFieldValidator ID="RqdConvictionType" runat="server" InitialValue="" ControlToValidate="ConvictionType" ErrorMessage="<%$ Resources:lbl_ErrMsg_ConvictionType %>" Display="none" SetFocusOnError="true" ValidationGroup="ConvictionGroup" Enabled="true"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblFine" runat="server" AssociatedControlID="txtConvFine" Text="<%$ Resources:lbl_Fine %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtConvFine" runat="server" CssClass="form-control" onblur="ConvertFindToDecimal()"></asp:TextBox>
                        </div>
                        <asp:RangeValidator ID="RngConvFine" runat="server" Type="Currency" ErrorMessage="<%$ Resources:lbl_InvalidConvFine %>" ControlToValidate="txtConvFine" SetFocusOnError="True" ValidationGroup="ConvictionGroup" Display="None" MinimumValue="0.00" MaximumValue="9999999999.00"></asp:RangeValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblConvStatus" runat="server" AssociatedControlID="ConvictionStatus" Text="<%$ Resources:lbl_ConvStatus %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <NexusProvider:LookupList ID="ConvictionStatus" runat="server" DataItemValue="Description" DataItemText="Description" Sort="ASC" ListType="Userdefined" ListCode="1114124" DefaultText="(Please Select)" CssClass="field-medium field-mandatory form-control form-select"></NexusProvider:LookupList>
                        </div>
                        <asp:RequiredFieldValidator ID="RqdConvictionStatus" runat="server" InitialValue="" ControlToValidate="ConvictionStatus" ErrorMessage="<%$ Resources:lbl_ErrMsg_ConvictionStatus %>" Display="none" SetFocusOnError="true" ValidationGroup="ConvictionGroup" Enabled="true"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblDescription" runat="server" AssociatedControlID="txtConvDescription" Text="<%$ Resources:lbl_Description %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtConvDescription" runat="server" CssClass="field-mandatory form-control"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="vldDescriptionNoRequired" runat="server" Display="none" ControlToValidate="txtConvDescription" ErrorMessage="<%$ Resources:lbl_DescriptionNo %>" SetFocusOnError="True" ValidationGroup="ConvictionGroup"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblConvDate" runat="server" Text="<%$ Resources:lbl_StartDate %>" AssociatedControlID="DATE__CONVICTION" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="DATE__CONVICTION" runat="server" CssClass="field-mandatory field-date form-control"></asp:TextBox><uc1:CalendarLookup ID="calEventFromDate" runat="server" LinkedControl="DATE__CONVICTION" HLevel="1"></uc1:CalendarLookup>
                            </div>
                        </div>
                        <asp:RequiredFieldValidator ID="reqvldConvDate" Display="None" ControlToValidate="DATE__CONVICTION" runat="server" ErrorMessage="<%$ Resources:lbl_Conv_Date %>" SetFocusOnError="True" ValidationGroup="ConvictionGroup"></asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="rangevldConvDate" runat="server" Type="Date" ErrorMessage="<%$ Resources:lbl_InvalidConvDateFormat %>" ControlToValidate="DATE__CONVICTION" SetFocusOnError="True" ValidationGroup="ConvictionGroup" Display="None" MinimumValue="01/01/1900"></asp:RangeValidator>
                    </div>
                </div>
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblHeading2" runat="server" Text="<%$ Resources:lbl_Heading2 %>"></asp:Label></legend>

                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblSentenceType" runat="server" AssociatedControlID="SentenceType" Text="<%$ Resources:lbl_SentenceType %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <NexusProvider:LookupList ID="SentenceType" runat="server" DataItemValue="Description" DataItemText="Description" Sort="ASC" ListType="UserDefined" ListCode="1114119" DefaultText="(Please Select)" CssClass="field-medium form-control form-select"></NexusProvider:LookupList>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblSentenceDate" runat="server" Text="<%$ Resources:lbl_StartDate %>" AssociatedControlID="txtSentenceDate" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="txtSentenceDate" runat="server" CssClass="field-date form-control"></asp:TextBox><uc1:CalendarLookup ID="calSentenceDate" runat="server" LinkedControl="txtSentenceDate" HLevel="1"></uc1:CalendarLookup>
                            </div>
                        </div>

                        <asp:RangeValidator ID="rangevldSentenceDate" runat="server" Type="Date" ErrorMessage="<%$ Resources:lbl_InvalidSentenceDate %>" ControlToValidate="txtSentenceDate" SetFocusOnError="True" ValidationGroup="ConvictionGroup" Display="None" MinimumValue="01/01/1900" MaximumValue="01/12/9999"></asp:RangeValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblSentenceDescription" runat="server" AssociatedControlID="txtSentenceDescription" Text="<%$ Resources:lbl_SentenceDescription %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtSentenceDescription" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblDuration" runat="server" AssociatedControlID="txtSentenceDuration" Text="<%$ Resources:lbl_Duration %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtSentenceDuration" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <asp:RangeValidator ID="rangevldDuration" runat="server" Type="Currency" ErrorMessage="<%$ Resources:lbl_InvalidDuration %>" ControlToValidate="txtSentenceDuration" SetFocusOnError="True" ValidationGroup="ConvictionGroup" Display="None" MinimumValue="0" MaximumValue="9999999999"></asp:RangeValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblTimeUnit" runat="server" AssociatedControlID="TimeUnit" Text="<%$ Resources:lbl_TimeUnit %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <NexusProvider:LookupList ID="TimeUnit" runat="server" DataItemValue="Description" DataItemText="Description" Sort="ASC" ListType="UserDefined" ListCode="1114122" DefaultText="(Please Select)" CssClass="field-medium form-control form-select"></NexusProvider:LookupList>
                        </div>
                    </div>
                </div>
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblHeading3" runat="server" Text="<%$ Resources:lbl_Heading3 %>"></asp:Label></legend>

                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblAlcoholMsrMethod" runat="server" AssociatedControlID="AlcoholMsrMethod" Text="<%$ Resources:lbl_AlcoholMsrMethod %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <NexusProvider:LookupList ID="AlcoholMsrMethod" runat="server" DataItemValue="Description" DataItemText="Description" Sort="ASC" ListType="UserDefined" ListCode="1114126" DefaultText="(Please Select)" CssClass="field-medium form-control form-select"></NexusProvider:LookupList>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblAlcoholLevel" runat="server" AssociatedControlID="txtAlcoholLevel" Text="<%$ Resources:lbl_AlcoholLevel %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtAlcoholLevel" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <asp:RangeValidator ID="RngtxtAlcoholLevel" runat="server" Type="integer" ErrorMessage="<%$ Resources:lbl_InvalidAlcoholLevel %>" ControlToValidate="txtAlcoholLevel" SetFocusOnError="True" ValidationGroup="ConvictionGroup" Display="None" MinimumValue="0" MaximumValue="999999999"></asp:RangeValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPenaltyPoints" runat="server" AssociatedControlID="txtPenaltyPoints" Text="<%$ Resources:lbl_PenaltyPoints %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtPenaltyPoints" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <asp:RangeValidator ID="RngtxtPenaltyPoints" runat="server" Type="Integer" ErrorMessage="<%$ Resources:lbl_InvalidPenaltyPoints %>" ControlToValidate="txtPenaltyPoints" SetFocusOnError="True" ValidationGroup="ConvictionGroup" Display="None" MinimumValue="0" MaximumValue="999999999"></asp:RangeValidator>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnAddConviction" runat="server" Text="<%$ Resources:btn_Add %>" ValidationGroup="ConvictionGroup" CausesValidation="true" OnClientClick="UpdateConvictionData()" SkinID="btnPrimary" style="margin-right:25px;"></asp:LinkButton>
                <asp:LinkButton ID="btnUpdateConviction" runat="server" Text="<%$ Resources:btn_Update %>" Visible="false" ValidationGroup="ConvictionGroup" CausesValidation="true" OnClientClick="UpdateConvictionData()" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
        <asp:HiddenField ID="txtPostBackTo" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="txtMode" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="txtKey" runat="server"></asp:HiddenField>
        <asp:ValidationSummary ID="ValidationSummary" ShowSummary="true" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" ValidationGroup="ConvictionGroup" CssClass="validation-summary"></asp:ValidationSummary>

    </div>
</asp:Content>
