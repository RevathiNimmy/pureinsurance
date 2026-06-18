<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/default.master" CodeFile="FIRE_FIRERATES.aspx.vb" Inherits="Nexus.FIRE_FIRERATES" %>

<%@ Register Src="~/Controls/ProgressBar.ascx" TagName="ProgressBar" TagPrefix="NexusControl" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="NexusControl" %>
<%@ Register Src="~/controls/AddressCntrl.ascx" TagName="AddressCntrl" TagPrefix="NexusControl" %>
<%@ Register Src="~/controls/StandardWordings.ascx" TagName="StandardWording" TagPrefix="NexusControl" %>
<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {


            if (!($('#<%= GROUP_FIRE__PRL_RATE_FIRE_EXPLOSION.ClientID %>')[0].checked)) {
                document.getElementById("<%= GROUP_FIRE__FIRE_EXPLOSION_BOOK_RATE.ClientID%>").disabled = true;
                document.getElementById("<%= GROUP_FIRE__FIRE_EXPLOSION_LD_RATE.ClientID%>").disabled = true;
                document.getElementById("<%= GROUP_FIRE__FIRE_EXPLOSION_AGREED_RATE.ClientID%>").disabled = true;
            }
            if (!($('#<%= GROUP_FIRE__PRL_SPECIAL_PERILS.ClientID %>')[0].checked)) {
                document.getElementById("<%= GROUP_FIRE__SPECIALS_PERILS_BOOK_RATE.ClientID%>").disabled = true;
                document.getElementById("<%= GROUP_FIRE__SPECIALS_PERILS_LD_RATE.ClientID%>").disabled = true;
                document.getElementById("<%= GROUP_FIRE__SPECIALS_PERILS_AGREED_RATE.ClientID%>").disabled = true;
            }
            if (!($('#<%= GROUP_FIRE__PRL_EARTHQUAKE.ClientID %>')[0].checked)) {
                document.getElementById("<%= GROUP_FIRE__EARTHQUAKE_BOOK_RATE.ClientID%>").disabled = true;
                document.getElementById("<%= GROUP_FIRE__EARTHQUAKE_LD_RATE.ClientID%>").disabled = true;
                document.getElementById("<%= GROUP_FIRE__EARTHQUAKE_AGREED_RATE.ClientID%>").disabled = true;
            }
            if (!($('#<%= GROUP_FIRE__PRL_MALICIOUS_DAMAGE.ClientID %>')[0].checked)) {
                document.getElementById("<%= GROUP_FIRE__MALICIOUS_DAMAGE_BOOK_RATE.ClientID%>").disabled = true;
                document.getElementById("<%= GROUP_FIRE__MALICIOUS_DAMAGE_LD_RATE.ClientID%>").disabled = true;
                document.getElementById("<%= GROUP_FIRE__MALICIOUS_DAMAGE_AGREED_RATE.ClientID%>").disabled = true;
            }
            document.getElementById("<%= GROUP_FIRE__FINAL_RATE_BOOK_RATE.ClientID%>").readOnly = true;
            document.getElementById("<%= GROUP_FIRE__FINAL_RATE_LD_RATE.ClientID%>").readOnly = true;
            //document.getElementById("<%= GROUP_FIRE__FINAL_RATE_AGREED_RATE.ClientID%>").readOnly = true;




            DoCalculations();

            if (document.getElementById("<%= GROUP_FIRE__FINAL_RATE_AGREED_RATE.ClientID%>").value == "" || document.getElementById("<%= GROUP_FIRE__FINAL_RATE_AGREED_RATE.ClientID%>").value == "0") {
                document.getElementById("<%= GROUP_FIRE__PRL_RATE_FIRE_EXPLOSION.ClientID%>").checked = true;
                document.getElementById("<%= GROUP_FIRE__PRL_SPECIAL_PERILS.ClientID%>").checked = true;
                document.getElementById("<%= GROUP_FIRE__PRL_EARTHQUAKE.ClientID%>").checked = true;
                document.getElementById("<%= GROUP_FIRE__PRL_MALICIOUS_DAMAGE.ClientID%>").checked = true;
                ToggleFireAndExplosion(true);
                ToggleSpecialPerils(true);
                ToggleEarthQuake(true);
                ToggleMaliciousDamage(true);
            }
            else {
                ToggleFireAndExplosion($('#<%= GROUP_FIRE__PRL_RATE_FIRE_EXPLOSION.ClientID%>')[0].checked);
                ToggleSpecialPerils($('#<%= GROUP_FIRE__PRL_SPECIAL_PERILS.ClientID%>')[0].checked);
                ToggleEarthQuake($('#<%= GROUP_FIRE__PRL_EARTHQUAKE.ClientID%>')[0].checked);
                ToggleMaliciousDamage($('#<%= GROUP_FIRE__PRL_MALICIOUS_DAMAGE.ClientID %>')[0].checked);
            }


        });

        function ToggleFireAndExplosion(flag) {
            document.getElementById("<%= GROUP_FIRE__FIRE_EXPLOSION_BOOK_RATE.ClientID%>").disabled = !flag;
                document.getElementById("<%= GROUP_FIRE__FIRE_EXPLOSION_BOOK_RATE.ClientID%>").value = document.getElementById("<%= hvBookRateFE.ClientID%>").value;
                document.getElementById("<%= GROUP_FIRE__FIRE_EXPLOSION_LD_RATE.ClientID%>").value = document.getElementById("<%= hvLDRate1.ClientID%>").value;
                if (document.getElementById("<%= hvLDRate1.ClientID%>").value != "") {
                    document.getElementById("<%= GROUP_FIRE__FIRE_EXPLOSION_AGREED_RATE.ClientID%>").value = document.getElementById("<%= hvLDRate1.ClientID%>").value;
            }
            else {
                document.getElementById("<%= GROUP_FIRE__FIRE_EXPLOSION_AGREED_RATE.ClientID%>").value = document.getElementById("<%= hvBookRateFE.ClientID%>").value;
            }
            document.getElementById("<%= GROUP_FIRE__FIRE_EXPLOSION_LD_RATE.ClientID%>").disabled = !flag;
                document.getElementById("<%= GROUP_FIRE__FIRE_EXPLOSION_AGREED_RATE.ClientID%>").disabled = !flag;
                if (!flag) {
                    document.getElementById("<%= GROUP_FIRE__FIRE_EXPLOSION_BOOK_RATE.ClientID%>").value = "";
                document.getElementById("<%= GROUP_FIRE__FIRE_EXPLOSION_LD_RATE.ClientID%>").value = "";
                document.getElementById("<%= GROUP_FIRE__FIRE_EXPLOSION_AGREED_RATE.ClientID%>").value = "";
            }
            DoCalculations();
        }

        function ToggleSpecialPerils(flag) {
            document.getElementById("<%= GROUP_FIRE__SPECIALS_PERILS_BOOK_RATE.ClientID%>").disabled = !flag;
            document.getElementById("<%= GROUP_FIRE__SPECIALS_PERILS_BOOK_RATE.ClientID%>").value = document.getElementById("<%= hvBookRateSP.ClientID%>").value;
            document.getElementById("<%= GROUP_FIRE__SPECIALS_PERILS_LD_RATE.ClientID%>").value = document.getElementById("<%= hvLDRate2.ClientID%>").value;
            if (document.getElementById("<%= hvLDRate2.ClientID%>").value != "") {
                document.getElementById("<%= GROUP_FIRE__SPECIALS_PERILS_AGREED_RATE.ClientID%>").value = document.getElementById("<%= hvLDRate2.ClientID%>").value;
            }
            else {
                document.getElementById("<%= GROUP_FIRE__SPECIALS_PERILS_AGREED_RATE.ClientID%>").value = document.getElementById("<%= hvBookRateSP.ClientID%>").value;
            }
            document.getElementById("<%= GROUP_FIRE__SPECIALS_PERILS_LD_RATE.ClientID%>").disabled = !flag;
            document.getElementById("<%= GROUP_FIRE__SPECIALS_PERILS_AGREED_RATE.ClientID%>").disabled = !flag;
            if (!flag) {
                document.getElementById("<%= GROUP_FIRE__SPECIALS_PERILS_BOOK_RATE.ClientID%>").value = "";
                document.getElementById("<%= GROUP_FIRE__SPECIALS_PERILS_LD_RATE.ClientID%>").value = "";
                document.getElementById("<%= GROUP_FIRE__SPECIALS_PERILS_AGREED_RATE.ClientID%>").value = "";
            }
            DoCalculations();
        }

        function ToggleEarthQuake(flag) {
            document.getElementById("<%= GROUP_FIRE__EARTHQUAKE_BOOK_RATE.ClientID%>").disabled = !flag;
            document.getElementById("<%= GROUP_FIRE__EARTHQUAKE_BOOK_RATE.ClientID%>").value = document.getElementById("<%= hvBookRateEQ.ClientID%>").value;
            document.getElementById("<%= GROUP_FIRE__EARTHQUAKE_LD_RATE.ClientID%>").disabled = !flag;
            document.getElementById("<%= GROUP_FIRE__EARTHQUAKE_AGREED_RATE.ClientID%>").disabled = !flag;
            document.getElementById("<%= GROUP_FIRE__EARTHQUAKE_LD_RATE.ClientID%>").value = document.getElementById("<%= hvLDRate3.ClientID%>").value;
            if (document.getElementById("<%= hvLDRate3.ClientID%>").value != "") {
                document.getElementById("<%= GROUP_FIRE__EARTHQUAKE_AGREED_RATE.ClientID%>").value = document.getElementById("<%= hvLDRate3.ClientID%>").value;
            }
            else {
                document.getElementById("<%= GROUP_FIRE__EARTHQUAKE_AGREED_RATE.ClientID%>").value = document.getElementById("<%= hvBookRateEQ.ClientID%>").value;
            }
            if (!flag) {
                document.getElementById("<%= GROUP_FIRE__EARTHQUAKE_BOOK_RATE.ClientID%>").value = "";
                document.getElementById("<%= GROUP_FIRE__EARTHQUAKE_LD_RATE.ClientID%>").value = "";
                document.getElementById("<%= GROUP_FIRE__EARTHQUAKE_AGREED_RATE.ClientID%>").value = "";
            }
            DoCalculations();
        }

        function ToggleMaliciousDamage(flag) {

            document.getElementById("<%= GROUP_FIRE__MALICIOUS_DAMAGE_BOOK_RATE.ClientID%>").disabled = !flag;
            document.getElementById("<%= GROUP_FIRE__MALICIOUS_DAMAGE_BOOK_RATE.ClientID%>").value = document.getElementById("<%= hvBookRateMD.ClientID%>").value;
            document.getElementById("<%= GROUP_FIRE__MALICIOUS_DAMAGE_LD_RATE.ClientID%>").disabled = !flag;
            document.getElementById("<%= GROUP_FIRE__MALICIOUS_DAMAGE_AGREED_RATE.ClientID%>").disabled = !flag;
            document.getElementById("<%= GROUP_FIRE__MALICIOUS_DAMAGE_LD_RATE.ClientID%>").value = document.getElementById("<%= hvLDRate4.ClientID%>").value;
            if (document.getElementById("<%= hvLDRate4.ClientID%>").value != "") {
                document.getElementById("<%= GROUP_FIRE__MALICIOUS_DAMAGE_AGREED_RATE.ClientID%>").value = document.getElementById("<%= hvLDRate4.ClientID%>").value;
            }
            else {
                document.getElementById("<%= GROUP_FIRE__MALICIOUS_DAMAGE_AGREED_RATE.ClientID%>").value = document.getElementById("<%= hvBookRateMD.ClientID%>").value;
            }
            if (!flag) {
                document.getElementById("<%= GROUP_FIRE__MALICIOUS_DAMAGE_BOOK_RATE.ClientID%>").value = "";
                document.getElementById("<%= GROUP_FIRE__MALICIOUS_DAMAGE_LD_RATE.ClientID%>").value = "";
                document.getElementById("<%= GROUP_FIRE__MALICIOUS_DAMAGE_AGREED_RATE.ClientID%>").value = "";
            }
            DoCalculations();
        }

        function DoCalculations() {
            document.getElementById("<%= GROUP_FIRE__FINAL_RATE_BOOK_RATE.ClientID%>").value = (parseFloat(parseFloat(document.getElementById("<%= GROUP_FIRE__FIRE_EXPLOSION_BOOK_RATE.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= GROUP_FIRE__SPECIALS_PERILS_BOOK_RATE.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= GROUP_FIRE__EARTHQUAKE_BOOK_RATE.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= GROUP_FIRE__MALICIOUS_DAMAGE_BOOK_RATE.ClientID%>").value) || 0)).toFixed(4);
            document.getElementById("<%= GROUP_FIRE__FINAL_RATE_LD_RATE.ClientID%>").value = (parseFloat(parseFloat(document.getElementById("<%= GROUP_FIRE__FIRE_EXPLOSION_LD_RATE.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= GROUP_FIRE__SPECIALS_PERILS_LD_RATE.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= GROUP_FIRE__EARTHQUAKE_LD_RATE.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= GROUP_FIRE__MALICIOUS_DAMAGE_LD_RATE.ClientID%>").value) || 0)).toFixed(4);
            document.getElementById("<%= GROUP_FIRE__FINAL_RATE_AGREED_RATE.ClientID%>").value = (parseFloat(parseFloat(document.getElementById("<%= GROUP_FIRE__FIRE_EXPLOSION_AGREED_RATE.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= GROUP_FIRE__SPECIALS_PERILS_AGREED_RATE.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= GROUP_FIRE__EARTHQUAKE_AGREED_RATE.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= GROUP_FIRE__MALICIOUS_DAMAGE_AGREED_RATE.ClientID%>").value) || 0)).toFixed(4);
        }

        function DoAgreedRateCalculations() {
            if (document.getElementById("<%= GROUP_FIRE__FINAL_RATE_LD_RATE.ClientID%>").value != "" || document.getElementById("<%= GROUP_FIRE__FINAL_RATE_LD_RATE.ClientID%>").value != "0") {
                document.getElementById("<%= GROUP_FIRE__FIRE_EXPLOSION_AGREED_RATE.ClientID%>").value = parseFloat(((parseFloat(document.getElementById("<%= GROUP_FIRE__FINAL_RATE_AGREED_RATE.ClientID%>").value) || 0) / (parseFloat(document.getElementById("<%= GROUP_FIRE__FINAL_RATE_LD_RATE.ClientID%>").value) || 0)) * (parseFloat(document.getElementById("<%= hvLDRate1.ClientID%>").value) || 0)).toFixed(4);
                document.getElementById("<%= GROUP_FIRE__SPECIALS_PERILS_AGREED_RATE.ClientID%>").value = parseFloat(((parseFloat(document.getElementById("<%= GROUP_FIRE__FINAL_RATE_AGREED_RATE.ClientID%>").value) || 0) / (parseFloat(document.getElementById("<%= GROUP_FIRE__FINAL_RATE_LD_RATE.ClientID%>").value) || 0)) * (parseFloat(document.getElementById("<%= hvLDRate2.ClientID%>").value) || 0)).toFixed(4);
                document.getElementById("<%= GROUP_FIRE__EARTHQUAKE_AGREED_RATE.ClientID%>").value = parseFloat(((parseFloat(document.getElementById("<%= GROUP_FIRE__FINAL_RATE_AGREED_RATE.ClientID%>").value) || 0) / (parseFloat(document.getElementById("<%= GROUP_FIRE__FINAL_RATE_LD_RATE.ClientID%>").value) || 0)) * (parseFloat(document.getElementById("<%= hvLDRate3.ClientID%>").value) || 0)).toFixed(4);
                document.getElementById("<%= GROUP_FIRE__MALICIOUS_DAMAGE_AGREED_RATE.ClientID%>").value = parseFloat(((parseFloat(document.getElementById("<%= GROUP_FIRE__FINAL_RATE_AGREED_RATE.ClientID%>").value) || 0) / (parseFloat(document.getElementById("<%= GROUP_FIRE__FINAL_RATE_LD_RATE.ClientID%>").value) || 0)) * (parseFloat(document.getElementById("<%= hvLDRate4.ClientID%>").value) || 0)).toFixed(4);
            }
            else {
                document.getElementById("<%= GROUP_FIRE__FIRE_EXPLOSION_AGREED_RATE.ClientID%>").value = parseFloat(((parseFloat(document.getElementById("<%= GROUP_FIRE__FINAL_RATE_AGREED_RATE.ClientID%>").value) || 0) / (parseFloat(document.getElementById("<%= GROUP_FIRE__FINAL_RATE_BOOK_RATE.ClientID%>").value) || 0)) * (parseFloat(document.getElementById("<%= hvBookRateFE.ClientID%>").value) || 0)).toFixed(4);
                document.getElementById("<%= GROUP_FIRE__SPECIALS_PERILS_AGREED_RATE.ClientID%>").value = parseFloat(((parseFloat(document.getElementById("<%= GROUP_FIRE__FINAL_RATE_AGREED_RATE.ClientID%>").value) || 0) / (parseFloat(document.getElementById("<%= GROUP_FIRE__FINAL_RATE_BOOK_RATE.ClientID%>").value) || 0)) * (parseFloat(document.getElementById("<%= hvBookRateSP.ClientID%>").value) || 0)).toFixed(4);
                document.getElementById("<%= GROUP_FIRE__EARTHQUAKE_AGREED_RATE.ClientID%>").value = parseFloat(((parseFloat(document.getElementById("<%= GROUP_FIRE__FINAL_RATE_AGREED_RATE.ClientID%>").value) || 0) / (parseFloat(document.getElementById("<%= GROUP_FIRE__FINAL_RATE_BOOK_RATE.ClientID%>").value) || 0)) * (parseFloat(document.getElementById("<%= hvBookRateEQ.ClientID%>").value) || 0)).toFixed(4);
                document.getElementById("<%= GROUP_FIRE__MALICIOUS_DAMAGE_AGREED_RATE.ClientID%>").value = parseFloat(((parseFloat(document.getElementById("<%= GROUP_FIRE__FINAL_RATE_AGREED_RATE.ClientID%>").value) || 0) / (parseFloat(document.getElementById("<%= GROUP_FIRE__FINAL_RATE_BOOK_RATE.ClientID%>").value) || 0)) * (parseFloat(document.getElementById("<%= hvBookRateMD.ClientID%>").value) || 0)).toFixed(4);
            }


        }
    </script>
    <div class="risk-screen">
        <NexusControl:ProgressBar ID="ucProgressBar" runat="server"></NexusControl:ProgressBar>
        <div class="card">
            <Nexus:tabindex id="ctrlTabIndex" runat="server" cssclass="TabContainer" tabcontainerclass="page-progress" activetabclass="ActiveTab" disabledclass="DisabledTab" scrollable="false"></Nexus:tabindex>
            <div class="card-body clearfix">
                <asp:HiddenField ID="hvBookRateFE" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="hvBookRateSP" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="hvBookRateEQ" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="hvBookRateMD" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="hvLDRate1" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="hvLDRate2" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="hvLDRate3" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="hvLDRate4" runat="server"></asp:HiddenField>
                <legend>Insured Perils</legend>
                <div class="grid-card table-responsive">
                    <table class="grid-table">
                        <tr>
                            <th style="width: 15%"></th>
                            <th style="width: 10%"></th>
                            <th style="width: 25%">
                                <label>Book Rate</label></th>
                            <th style="width: 25%">
                                <label>L and D Rate</label></th>
                            <th style="width: 25%">
                                <label>Agreed Rate</label></th>

                        </tr>

                        <tr>
                            <td style="width: 15%">
                                <label>Fire and Explosion</label></td>
                            <td style="width: 10%">
                                <asp:CheckBox ID="GROUP_FIRE__PRL_RATE_FIRE_EXPLOSION" runat="server" onclick="ToggleFireAndExplosion(this.checked);" Text=" " CssClass="asp-check"></asp:CheckBox></td>
                            <td style="width: 25%">
                                <asp:TextBox ID="GROUP_FIRE__FIRE_EXPLOSION_BOOK_RATE" CssClass="form-control e-num4" runat="server"></asp:TextBox></td>
                            <td style="width: 25%">
                                <asp:TextBox ID="GROUP_FIRE__FIRE_EXPLOSION_LD_RATE" CssClass="form-control e-num4" runat="server"></asp:TextBox></td>
                            <td style="width: 25%">
                                <asp:TextBox ID="GROUP_FIRE__FIRE_EXPLOSION_AGREED_RATE" CssClass="form-control e-num4" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 15%">
                                <label>Specials Perils</label></td>
                            <td style="width: 10%">
                                <asp:CheckBox ID="GROUP_FIRE__PRL_SPECIAL_PERILS" runat="server" onclick="ToggleSpecialPerils(this.checked);" Text=" " CssClass="asp-check"></asp:CheckBox></td>
                            <td style="width: 25%">
                                <asp:TextBox ID="GROUP_FIRE__SPECIALS_PERILS_BOOK_RATE" CssClass="form-control e-num4" runat="server"></asp:TextBox></td>
                            <td style="width: 25%">
                                <asp:TextBox ID="GROUP_FIRE__SPECIALS_PERILS_LD_RATE" CssClass="form-control e-num4" runat="server"></asp:TextBox></td>
                            <td style="width: 25%">
                                <asp:TextBox ID="GROUP_FIRE__SPECIALS_PERILS_AGREED_RATE" CssClass="form-control e-num4" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 15%">
                                <label>Earthquake</label></td>
                            <td style="width: 10%">
                                <asp:CheckBox ID="GROUP_FIRE__PRL_EARTHQUAKE" runat="server" onclick="ToggleEarthQuake(this.checked);" Text=" " CssClass="asp-check"></asp:CheckBox></td>
                            <td style="width: 25%">
                                <asp:TextBox ID="GROUP_FIRE__EARTHQUAKE_BOOK_RATE" CssClass="form-control e-num4" runat="server"></asp:TextBox></td>
                            <td style="width: 25%">
                                <asp:TextBox ID="GROUP_FIRE__EARTHQUAKE_LD_RATE" CssClass="form-control e-num4" runat="server"></asp:TextBox></td>
                            <td style="width: 25%">
                                <asp:TextBox ID="GROUP_FIRE__EARTHQUAKE_AGREED_RATE" CssClass="form-control e-num4" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 15%">
                                <label>Malicious Damage</label></td>
                            <td style="width: 10%">
                                <asp:CheckBox ID="GROUP_FIRE__PRL_MALICIOUS_DAMAGE" runat="server" onclick="ToggleMaliciousDamage(this.checked);" Text=" " CssClass="asp-check"></asp:CheckBox></td>
                            <td style="width: 25%">
                                <asp:TextBox ID="GROUP_FIRE__MALICIOUS_DAMAGE_BOOK_RATE" CssClass="form-control e-num4" runat="server"></asp:TextBox></td>
                            <td style="width: 25%">
                                <asp:TextBox ID="GROUP_FIRE__MALICIOUS_DAMAGE_LD_RATE" CssClass="form-control e-num4" runat="server"></asp:TextBox></td>
                            <td style="width: 25%">
                                <asp:TextBox ID="GROUP_FIRE__MALICIOUS_DAMAGE_AGREED_RATE" CssClass="form-control e-num4" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 15%">
                                <label>Final Rate</label></td>
                            <td style="width: 10%"></td>
                            <td style="width: 25%">
                                <asp:TextBox ID="GROUP_FIRE__FINAL_RATE_BOOK_RATE" CssClass="form-control e-num4" runat="server"></asp:TextBox></td>
                            <td style="width: 25%">
                                <asp:TextBox ID="GROUP_FIRE__FINAL_RATE_LD_RATE" CssClass="form-control e-num4" runat="server"></asp:TextBox></td>
                            <td style="width: 25%">
                                <asp:TextBox ID="GROUP_FIRE__FINAL_RATE_AGREED_RATE" CssClass="form-control field-mandatory e-num4" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvFRAR" runat="server" ControlToValidate="GROUP_FIRE__FINAL_RATE_AGREED_RATE" ErrorMessage="Final Rate Agreed Rate can not be 0." InitialValue="0.00" SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator>
                                <asp:RangeValidator ID="RangeGROUP_FIRE__FINAL_RATE_AGREED_RATE" Type="Double" runat="server" MinimumValue="0" MaximumValue="100" ControlToValidate="GROUP_FIRE__FINAL_RATE_AGREED_RATE" SetFocusOnError="true" Display="None" ErrorMessage="Rate must be less then 100%."></asp:RangeValidator>

                            </td>
                        </tr>

                    </table>
                </div>
            </div>
            <div class='card-footer'>
                <asp:LinkButton ID="btnBack" runat="server" Text="<i class='fa fa-chevron-left' aria-hidden='true'></i> Back" OnClick="BackButton" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnNext" runat="server" Text="Next <i class='fa fa-chevron-right' aria-hidden='true'></i>" OnClick="NextButton" SkinID="btnPrimary"></asp:LinkButton>
                <asp:LinkButton ID="btnFinish" runat="server" Text="<i class='fa fa-check' aria-hidden='true'></i> Finish" OnClick="FinishButton" OnPreRender="PreRenderFinish" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
        <asp:ValidationSummary ID="vldsumSummary" DisplayMode="BulletList" HeaderText="<h2>There are errors on this page</h2><p>Please review these errors and re-submit the form</p>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
