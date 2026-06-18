<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/default.master" CodeFile="FIRE_OVERVIEW.aspx.vb" Inherits="Nexus.FIRE_OVERVIEW" %>

<%@ Register Src="~/Controls/ProgressBar.ascx" TagName="ProgressBar" TagPrefix="NexusControl" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="NexusControl" %>
<%@ Register Src="~/controls/AddressCntrl.ascx" TagName="AddressCntrl" TagPrefix="NexusControl" %>
<%@ Register Src="~/controls/StandardWordings.ascx" TagName="StandardWording" TagPrefix="NexusControl" %>
<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <div class="risk-screen">
        <NexusControl:ProgressBar ID="ucProgressBar" runat="server"></NexusControl:ProgressBar>
        <div class="card">
            <Nexus:tabindex id="ctrlTabIndex" runat="server" cssclass="TabContainer" tabcontainerclass="page-progress" activetabclass="ActiveTab" disabledclass="DisabledTab" scrollable="false"></Nexus:tabindex>
            <div class="card-body clearfix">
                <legend>Financial Overview</legend>
                <div class="grid-card table-responsive">
                    <table class="grid-table">
                        <tr>
                            <th width="20%"></th>
                            <th width="20%">Total Sum Insured</th>
                            <th width="20%">Risk Premium</th>
                            <th width="20%">Extension Prem</th>
                            <th width="20%">Total Prem</th>

                        </tr>
                        <tr>
                            <td>Fire</td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__FIRE_TOTAL_SI" runat="server" CssClass="form-control e-num2"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__FIRE_RISK_PREM" runat="server" CssClass="form-control e-num2"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__FIRE_EXT_PREM" runat="server" CssClass="form-control e-num2"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__FIRE_TOTAL_PREM" runat="server" CssClass="form-control e-num2"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Building Combined</td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__BLDG_TOTAL_SI" runat="server" Text="0" ReadOnly="true" CssClass="form-control e-num2"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__BLDG_RISK_PREM" runat="server" Text="0" ReadOnly="true" CssClass="form-control e-num2"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__BLDG_EXT_PREM" runat="server" Text="0" ReadOnly="true" CssClass="form-control e-num2"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__BLDG_TOTAL_PREM" runat="server" Text="0" ReadOnly="true" CssClass="form-control e-num2"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Business Interruption</td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__BI_TOTAL_SI" runat="server" Text="0" ReadOnly="true" CssClass="form-control e-num2"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__BI_RISK_PREM" runat="server" Text="0" ReadOnly="true" CssClass="form-control e-num2"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__BI_EXT_PREM" runat="server" Text="0" ReadOnly="true" CssClass="form-control e-num2"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__BI_TOTAL_PREM" runat="server" Text="0" ReadOnly="true" CssClass="form-control e-num2"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Account Receivable</td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__ACCT_TOTAL_SI" runat="server" ReadOnly="true" Text="0" CssClass="form-control e-num2"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__ACCT_RISK_PREM" runat="server" ReadOnly="true" Text="0" CssClass="form-control e-num2"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__ACCT_EXT_PREM" runat="server" ReadOnly="true" Text="0" CssClass="form-control e-num2"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__ACCT_TOTAL_PREM" runat="server" ReadOnly="true" Text="0" CssClass="form-control e-num2"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Office Contents</td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__OFF_TOTAL_SI" runat="server" CssClass="form-control e-num2"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__OFF_RISK_PREM" runat="server" CssClass="form-control e-num2"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__OFF_EXT_PREM" runat="server" CssClass="form-control e-num2"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__OFF_TOTAL_PREM" runat="server" CssClass="form-control e-num2"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Accidental Damage</td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__ACC_TOTAL_SI" runat="server" ReadOnly="true" Text="0" CssClass="form-control e-num2"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__ACC_RISK_PREM" runat="server" ReadOnly="true" Text="0" CssClass="form-control e-num2"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__ACC_EXT_PREM" runat="server" ReadOnly="true" Text="0" CssClass="form-control e-num2"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__ACC_TOTAL_PREM" runat="server" ReadOnly="true" Text="0" CssClass="form-control e-num2"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__TSI_TOTAL_SUM_INSURED" runat="server" CssClass="form-control e-num2"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__TSI_RISK_PREMIUM" runat="server" CssClass="form-control e-num2"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__TSI_EXTENSION_PREM" runat="server" CssClass="form-control e-num2"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__TSI_TOTAL_PREMIUM" runat="server" CssClass="form-control e-num2"></asp:TextBox></td>
                        </tr>
                    </table>

                </div>
                <legend>Combined Reinsurance Exposure</legend>
                <div class="grid-card table-responsive">
                    <table class="grid-table">
                        <tr>
                            <th width="20%"></th>
                            <th width="20%">Total Sum Insured</th>
                            <th width="20%">Target Risk SI</th>
                            <th width="20%">RI Exposure</th>
                            <th width="20%">RI Exposure VAT Excl.</th>
                        </tr>
                        <tr>
                            <td>Fire</td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__RE_FIRE_TOTAL_SI" runat="server" CssClass="form-control e-num2"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__RE_FIRE_TARGET_RISK_SI" runat="server" CssClass="form-control e-num2"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__RE_FIRE_RI_EXPOSURE" runat="server" CssClass="form-control e-num2"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__RE_FIRE_RI_EXP_VAT_EXL" runat="server" CssClass="form-control e-num2"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Building Combined</td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__RE_BLDG_TOTAL_SI" runat="server" ReadOnly="true" Text="0" CssClass="form-control e-num2"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__RE_BLDG_TARGET_RISK_SI" runat="server" ReadOnly="true" Text="0" CssClass="form-control e-num2"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__RE_BLDG_RI_EXPOSURE" runat="server" Text="0" ReadOnly="true" CssClass="form-control e-num2"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__RE_BLDG_RI_EXP_VAT_EXL" runat="server" Text="0" ReadOnly="true" CssClass="form-control e-num2"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Office Contents</td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__RE_OFFC_TOTAL_SI" runat="server" CssClass="form-control e-num2"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__RE_OFFC_TARGET_RISK_SI" runat="server" CssClass="form-control e-num2"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__RE_OFFC_RI_EXPOSURE" runat="server" CssClass="form-control e-num2"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__RE_OFFC_EXP_VAT_EXL" runat="server" CssClass="form-control e-num2"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Business Interruption</td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__RE_BUS_TOTAL_SI" runat="server" Text="0" ReadOnly="true" CssClass="form-control e-num2"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__RE_BUS_TARGET_RISK_SI" runat="server" Text="0" ReadOnly="true" CssClass="form-control e-num2"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__RE_BUS_RI_EXPOSURE" runat="server" Text="0" ReadOnly="true" CssClass="form-control e-num2"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__RE_BUS_EXP_VAT_EXL" runat="server" Text="0" ReadOnly="true" CssClass="form-control e-num2"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Account Receivable</td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__RE_ACCT_REC_TOTAL_SI" runat="server" Text="0" ReadOnly="true" CssClass="form-control e-num2"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__RE_ACCT_REC_TARGET_RISK_SI" runat="server" ReadOnly="true" Text="0" CssClass="form-control e-num2"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__RE_ACCT_REC_RI_EXPOSURE" runat="server" ReadOnly="true" Text="0" CssClass="form-control e-num2"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__RE_ACCT_REC_EXP_VAT_EXL" runat="server" ReadOnly="true" Text="0" CssClass="form-control e-num2"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__CRI_TOTAL_SUM_INSURED" runat="server" ReadOnly="true" CssClass="form-control e-num2"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__CRI_TARGET_RISK_SI" runat="server" ReadOnly="true" CssClass="form-control e-num2"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__CRI_RI_EXPOSURE" runat="server" ReadOnly="true" CssClass="form-control e-num2"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="SEC_OVRW__CRI_RI_EXPOSURE_VAT_EXCL" runat="server" ReadOnly="true" CssClass="form-control e-num2"></asp:TextBox></td>
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
