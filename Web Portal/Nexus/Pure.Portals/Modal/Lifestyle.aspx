<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Lifestyle.aspx.vb" Inherits="Nexus.Modal_Lifestyle"
    MasterPageFile="~/default.master" %>

<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script language="javascript" type="text/javascript">

        function UpdateLifeStyleData() {
            //debugger;
            var LifeStyleData;
            var oDDL;
            //to Fire teh Client Validation first
            Page_ClientValidate();

            if (Page_IsValid == true) {
                //Mode
                LifeStyleData = document.getElementById('<%=txtMode.ClientID %>').value + ";";
                //Name
                LifeStyleData += document.getElementById('<%=txtName.ClientID %>').value + ";";
                //DOB
                LifeStyleData += document.getElementById('<%=txtDOB.ClientID %>').value + ";";
                //Category
                oDDL = document.getElementById('<%=GISLifestyle_Category.ClientID %>');
                LifeStyleData += oDDL.options[oDDL.selectedIndex].value + ";";
                //Gender Code
                oDDL = document.getElementById('<%=GISLifestyle_Gender.ClientID %>');
                LifeStyleData += oDDL.options[oDDL.selectedIndex].value + ";";
                //Occupation Code
                oDDL = document.getElementById('<%=GISLifestyle_Occupation.ClientID %>');
                LifeStyleData += oDDL.options[oDDL.selectedIndex].value + ";";
                //SecOccupationCode
                oDDL = document.getElementById('<%=GISLifestyle_SecOccupation.ClientID %>');
                LifeStyleData += oDDL.options[oDDL.selectedIndex].value + ";";
                //Smoker
                LifeStyleData += document.getElementById('<%=chkSmoker.ClientID %>').checked + ";";
                //Key
                LifeStyleData += document.getElementById('<%=txtKey.ClientID %>').value + ";";

                self.parent.tb_remove();
                self.parent.ReceiveLifeStyleData(LifeStyleData, document.getElementById('<%=txtPostBackTo.ClientID %>').value);
            }
        }
    </script>

    <div id="Modal_Lifestyle">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblTitle" runat="server" Text="<%$ Resources:lbl_Lifestyle_Title %>"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblHeading1" runat="server" Text="<%$ Resources:lbl_Heading %>"></asp:Label></legend>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblName" runat="server" AssociatedControlID="txtName" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="ltName" runat="server" Text="<%$ Resources:lbl_Name %>"></asp:Literal>
                        </asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblDOB" runat="server" AssociatedControlID="txtDOB" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="ltDOB" runat="server" Text="<%$ Resources:lbl_DOB %>"></asp:Literal>
                        </asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="txtDOB" runat="server" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="calEventFromDate" runat="server" LinkedControl="txtDOB" HLevel="1"></uc1:CalendarLookup>
                            </div>
                        </div>

                        <asp:RangeValidator ID="rvDOB" runat="server" ControlToValidate="txtDOB" Display="None" ValidationGroup="LifestyleGroup" ErrorMessage="<%$ Resources:lbl_InvalidDOB %>" Type="Date"></asp:RangeValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblCategory" runat="server" AssociatedControlID="GISLifestyle_Category" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="ltCategory" runat="server" Text="<%$ Resources:lbl_Category %>"></asp:Literal></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <NexusProvider:LookupList ID="GISLifestyle_Category" runat="server" DataItemValue="Code" DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="Lifestyle_Category" DefaultText="(Please Select)" CssClass="field-medium form-control form-select"></NexusProvider:LookupList>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblGenderCode" runat="server" AssociatedControlID="GISLifestyle_Gender" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="ltGenderCode" runat="server" Text="<%$ Resources:lbl_GenderCode %>"></asp:Literal></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <NexusProvider:LookupList ID="GISLifestyle_Gender" runat="server" DataItemValue="description" DataItemText="description" ListType="UserDefined" ListCode="131091" DefaultText="(Please select)" CssClass="field-medium form-control form-select"></NexusProvider:LookupList>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblOccupationCode" runat="server" AssociatedControlID="GISLifestyle_Occupation" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="ltOccupationCode" runat="server" Text="<%$ Resources:lbl_OccupationCode %>"></asp:Literal></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <NexusProvider:LookupList ID="GISLifestyle_Occupation" runat="server" DataItemValue="description" DataItemText="description" ListType="UserDefined" ListCode="2228226" DefaultText="(Please select)" CssClass="field-medium form-control form-select"></NexusProvider:LookupList>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblSecOccupationCode" runat="server" AssociatedControlID="GISLifestyle_SecOccupation" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="ltSecOccupationCode" runat="server" Text="<%$ Resources:lbl_SecOccupationCode %>"></asp:Literal></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <NexusProvider:LookupList ID="GISLifestyle_SecOccupation" runat="server" DataItemValue="description" DataItemText="description" ListType="UserDefined" ListCode="2228226" DefaultText="(Please select)" CssClass="field-medium form-control form-select"></NexusProvider:LookupList>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblSmoker" runat="server" AssociatedControlID="chkSmoker" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="ltSmoker" runat="server" Text="<%$ Resources:lbl_Smoker %>"></asp:Literal></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:CheckBox ID="chkSmoker" runat="server" Text=" " CssClass="asp-check"></asp:CheckBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblAddLifestyle" runat="server" AssociatedControlID="btnAddLifestyle" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="card-footer">

                <asp:LinkButton ID="btnUpdateLifestyle" runat="server" Text="<%$ Resources:lbl_btnUpdateLifestyle %>" Visible="false" ValidationGroup="LifestyleGroup" CausesValidation="true" OnClientClick="UpdateLifeStyleData()" SkinID="btnPrimary"></asp:LinkButton>
                <asp:LinkButton ID="btnAddLifestyle" runat="server" Text="<%$ Resources:lbl_btnAddLifestyle %>" ValidationGroup="LifestyleGroup" CausesValidation="true" OnClientClick="UpdateLifeStyleData()" SkinID="btnPrimary"></asp:LinkButton>

            </div>
        </div>
        <asp:HiddenField ID="txtPostBackTo" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="txtMode" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="txtKey" runat="server"></asp:HiddenField>
        <asp:ValidationSummary ID="ValidationSummary" ShowSummary="true" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" ValidationGroup="LifestyleGroup" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
