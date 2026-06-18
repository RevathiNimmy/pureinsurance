<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Statements.aspx.vb" Inherits="Nexus.Statements"
    MasterPageFile="~/default.master" Title="Untitled Page" %>

<%@ Register Src="../Controls/ProgressBar.ascx" TagName="ProgressBar" TagPrefix="uc3" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script type="text/jscript" language="javascript">

        function PopupEditClientDetails() {
            window.open("../modal/FindAgent.aspx", "List", "scrollbars=yes,resizable=no,width=800,height=400");

            return false;
        }
        function statementUpdated() {

            tb_remove();
            document.getElementById('ctl00_cntMainBody_btnHiddenStatement').click();
        }

        function ConfirmDepositCollection(sMessage) {
            var IsConfirm;

            IsConfirm = window.confirm(sMessage);
            if (IsConfirm)
                document.getElementById('<%=hdnDepositCollection.ClientID%>').value = "1";
            else
                document.getElementById('<%=hdnDepositCollection.ClientID%>').value = "0";
        }
    </script>

    <div id="secure_Statements">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:HiddenField ID="hdnDepositCollection" runat="server" Value="0"></asp:HiddenField>
        <uc3:ProgressBar ID="ucProgressBar" runat="server"></uc3:ProgressBar>
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Label ID="lblStatementHeading" runat="server" Text="<%$ Resources:lbl_Statements_heading %>"></asp:Label></h1>
            </div>
            <div class="card-body clearfix">
                <p>
                    <asp:Literal ID="lblStatementtext" runat="server" Text="<%$ Resources:lbl_Statement_text %>"></asp:Literal>
                </p>
                <legend>
                    <asp:Literal ID="lblDeclarationHeading" runat="server" Text="<%$ Resources:lbl_Declaration_heading %>"></asp:Literal>
                </legend>
                <p class="text-dark dk">
                    <asp:Literal ID="lblDeclarationtext" runat="server" Text="<%$ Resources:lbl_Declaration_text %>"></asp:Literal>
                </p>
                <asp:Panel ID="pnlConfirmContactDetails" Visible="false" runat="server">
                    <div class="card-divider">
                        <h5>
                            <asp:Label ID="lblPageHeading" runat="server" Text="<%$ Resources:lbl_Page_header%>"></asp:Label></h5>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="False" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="card-body clearfix">
                                <div class="form-horizontal">
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblNameTitle" runat="server" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <p class="form-control-static font-bold">
                                                <asp:Literal ID="lblName" runat="server"></asp:Literal>
                                            </p>
                                        </div>
                                    </div>
                                    <div id="liMainContact" runat="server" visible="false" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblMainContactTitle" runat="server" Text="<%$ Resources:lbl_MainContact %>" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <p class="form-control-static font-bold">
                                                <asp:Literal ID="lblMainContact" runat="server"></asp:Literal>
                                            </p>
                                        </div>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblEmailTitle" runat="server" Text="<%$ Resources:lbl_EmailAddress %>" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <p class="form-control-static font-bold">
                                                <asp:Literal ID="LblEmail" runat="server"></asp:Literal>
                                            </p>
                                        </div>

                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblTelephoneTitle" runat="server" Text="<%$ Resources:lbl_Telephone %>" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <p class="form-control-static font-bold">
                                                <asp:Literal ID="LblTelephone" runat="server"></asp:Literal>
                                            </p>
                                        </div>

                                    </div>
                                    <div id="liAddress1Title" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblAddress1Title" runat="server" Text="<%$ Resources:lbl_Address1 %>" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <p class="form-control-static font-bold">
                                                <asp:Literal ID="LblAddress1" runat="server"></asp:Literal>
                                            </p>
                                        </div>
                                    </div>
                                    <div id="liAddress2Title" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblAddress2Title" runat="server" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <p class="form-control-static font-bold">
                                                <asp:Literal ID="LblAddress2" runat="server"></asp:Literal>
                                            </p>
                                        </div>
                                    </div>
                                    <div id="liAddress3Title" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblAddress3Title" runat="server" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <p class="form-control-static font-bold">
                                                <asp:Literal ID="LblAddress3" runat="server"></asp:Literal>
                                            </p>
                                        </div>
                                    </div>
                                    <div id="liAddress4Title" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblAddress4Title" runat="server" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <p class="form-control-static font-bold">
                                                <asp:Literal ID="LblAddress4" runat="server"></asp:Literal>
                                            </p>
                                        </div>
                                    </div>
                                    <div id="liPostcodeTitle" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblPostcodeTitle" runat="server" Text="<%$ Resources:lbl_Postcode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <p class="form-control-static font-bold">
                                                <asp:Literal ID="LblPostcode" runat="server"></asp:Literal>
                                            </p>
                                        </div>
                                    </div>
                                    <div id="licountry" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblCountryTitle" runat="server" Text="<%$ Resources:lbl_Country %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <p class="form-control-static font-bold">
                                                <asp:Literal ID="lblCountry" runat="server"></asp:Literal>
                                            </p>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnHiddenStatement" EventName="Click"></asp:AsyncPostBackTrigger>
                        </Triggers>
                    </asp:UpdatePanel>
                    <Nexus:ProgressIndicator ID="UpStatements" OverlayCssClass="updating" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
                        <progresstemplate>
                                        </progresstemplate>
                    </Nexus:ProgressIndicator>
                    <asp:Button ID="btnHiddenStatement" runat="server" Style="display: none;" CausesValidation="false"></asp:Button>
                    <p class="text-danger">
                        <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:lbl_Confirm_text %>"></asp:Literal>
                        <asp:CheckBox ID="chkConfirmContact" runat="server" Text=" " CssClass="asp-check"></asp:CheckBox>
                    </p>
                     <asp:CustomValidator ID="vldConfirmContact" runat="server" CssClass="error" Display="Dynamic" ErrorMessage="<%$ Resources:lbl_Please_Check %>" ClientValidationFunction="EnsureChecked"></asp:CustomValidator>
                </asp:Panel>
                <p class="text-danger">
                    <asp:Literal ID="lblCheckbox" runat="server" Text="<%$ Resources:lbl_Checkbox_text %>"></asp:Literal>
                    <asp:CheckBox ID="chkConfirmation" runat="server" Text=" " CssClass="asp-check"></asp:CheckBox>
                </p>
                 <asp:CustomValidator ID="vldConfirmation" runat="server" Display="Dynamic" CssClass="error" ErrorMessage="<%$ Resources:lbl_Please_Check %>" ClientValidationFunction="EnsureChecked"></asp:CustomValidator>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="lnkEditDetails" runat="server" Text="<%$ Resources:lbl_Edit %>" Visible="false" SkinID="btnPrimary"></asp:LinkButton>
                <asp:LinkButton ID="btnContinue" runat="server" Text="<%$ Resources:lbl_Continue_button %>" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
       
       
    </div>
</asp:Content>
