<%@ Control Language="VB" AutoEventWireup="false" CodeFile="CausationAnalysis.ascx.vb" Inherits="Nexus.Controls_ReportControls_CausationAnalysis" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>

<div id="Controls_ReportControls_AgentAnalysis">
    <div class="card">
        <div class="card-body clearfix">
            <div class="form-horizontal">
                <legend>
                    <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:lbl_header %>"></asp:Label></legend>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblPrimaryCause" runat="server" AssociatedControlID="RP__Primary_cause" Text="<%$ Resources:lbl_PrimaryCause %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__Primary_cause" runat="server" CssClass="field-medium form-control form-select">
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_ALL %>" Value="ALL"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_AccidentalDamage %>" Value="Accidental Damage"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_AccidentalLoss %>" Value="Accidental Loss"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_BlowOut %>" Value="Blow Out"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_Cancellation %>" Value="Cancellation"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_CharterersLiability %>" Value="Charterers Liability"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_CivilUnrest %>" Value="Civil Unrest"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_ClassAction %>" Value="Class Action"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_Collapse %>" Value="Collapse"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_Contamination %>" Value="Contamination"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_ContractualLiability %>" Value="Contractual Liability"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_CrewNegligence %>" Value="Crew Negligence"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_Curtailment %>" Value="Curtailment"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_Delay %>" Value="Delay"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_DJAction %>" Value="DJ Action"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_Earthquake %>" Value="Earthquake"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_EmployersLiability %>" Value="Employers Liability"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_EPL %>" Value="EPL"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_EscapeofOil %>" Value="Escape of Oil"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_EscapeofWater %>" Value="Escape of Water"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_Explosion %>" Value="Explosion"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_Fire %>" Value="Fire"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_Flood %>" Value="Flood"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_Freezing %>" Value="Freezing"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_GeneralLiability %>" Value="General Liability"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_Grounding %>" Value="Grounding"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_HeavyWeather %>" Value="Heavy Weather"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_Hijacking %>" Value="Hijacking"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_ImpactCollision %>" Value="Impact/Collision"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_Infestation %>" Value="Infestation"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_Injury %>" Value="Injury"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_JettiWashoff %>" Value="Jetti/Washoff"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_Landslide %>" Value="Landslide"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_Lightning %>" Value="Lightning"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_MachineryBreakdown %>" Value="Machinery Breakdown"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_MaliciousDamage %>" Value="Malicious Damage"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_MedicalExpenses %>" Value="Medical Expenses"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_Misselling %>" Value="Mis-selling"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_Negligence %>" Value="Negligence"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_OccupiersLiability %>" Value="Occupiers Liability"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_Piracy %>" Value="Piracy"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_Pollution %>" Value="Pollution"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_ProductsLiability %>" Value="Products Liability"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_PublicLiability %>" Value="Public Liability"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_PublicOfficials %>" Value="Public Officials"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_Regulatory %>" Value="Regulatory"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_Riot %>" Value="Riot"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_Sickness %>" Value="Sickness"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_Sinking %>" Value="Sinking"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_Storm %>" Value="Storm"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_SubsidenceHeave %>" Value="Subsidence/Heave"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_Terrorism %>" Value="Terrorism"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_Theft %>" Value="Theft"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_Tsunami %>" Value="Tsunami"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_War %>" Value="War"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_PrimaryCause_WaterIngress %>" Value="Water Ingress"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblFromDate" runat="server" AssociatedControlID="RP__Start_Date" Text="<%$ Resources:lbl_FromDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <div class="input-group">
                            <asp:TextBox ID="RP__Start_Date" runat="server" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="calFromDate" runat="server" LinkedControl="RP__Start_Date" HLevel="1"></uc1:CalendarLookup>
                        </div>
                    </div>

                    <asp:RequiredFieldValidator ID="reqdvldFromDate" Display="None" ControlToValidate="RP__Start_Date" runat="server" ErrorMessage="<%$ Resources:lbl_req_FromDate %>" SetFocusOnError="True" ValidationGroup="vldReportsControlsGroup"> </asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="comvldFromDate" runat="server" Display="None" ControlToValidate="RP__Start_Date" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_invalid_FromDate %>" Operator="DataTypeCheck" Type="Date" ValidationGroup="vldReportsControlsGroup"></asp:CompareValidator>
                    <asp:RangeValidator ID="rngvldStartDate" runat="server" ErrorMessage="<%$ Resources:lbl_invalidrange_StartDate %>" ControlToValidate="RP__START_DATE" Display="None" Enabled="true" ValidationGroup="vldReportsControlsGroup" Type="Date">
                    </asp:RangeValidator>
                </div>


                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblToDate" runat="server" AssociatedControlID="RP__End_Date" Text="<%$ Resources:lbl_ToDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <div class="input-group">
                            <asp:TextBox ID="RP__End_Date" runat="server" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="CalToDate" runat="server" LinkedControl="RP__End_Date" HLevel="1"></uc1:CalendarLookup>
                        </div>
                    </div>

                    <asp:RequiredFieldValidator ID="reqdvldToDate" Display="None" ControlToValidate="RP__End_Date" runat="server" ErrorMessage="<%$ Resources:lbl_req_ToDate %>" SetFocusOnError="True" ValidationGroup="vldReportsControlsGroup"> </asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="comvldToDate" runat="server" Display="None" ControlToValidate="RP__End_Date" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_invalid_ToDate %>" Operator="DataTypeCheck" Type="Date" ValidationGroup="vldReportsControlsGroup"></asp:CompareValidator>
                    <asp:RangeValidator ID="rngvldEndDate" runat="server" ErrorMessage="<%$ Resources:lbl_invalidrange_EndDate %>" ControlToValidate="RP__END_DATE" Display="None" ValidationGroup="vldReportsControlsGroup" Type="Date">
                    </asp:RangeValidator>
                    <asp:CompareValidator ID="cmpvldStartEndDates" ForeColor="Red" runat="server" ControlToValidate="RP__START_DATE" ControlToCompare="RP__END_DATE" Operator="LessThanEqual" Type="Date" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_invalidGreater_StartDate %>" Display="None" ValidationGroup="vldReportsControlsGroup"></asp:CompareValidator>
                </div>



                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblDateRange" runat="server" AssociatedControlID="RP__DateRange" Text="<%$ Resources:lbl_DateRange %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__DateRange" runat="server" CssClass="field-medium form-control form-select">
                            <asp:ListItem Text="<%$ Resources:li_SelectDateRange_DefaultText %>" Value="Specify Dates"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_SelectDateRange_Yesterday %>" Value="Yesterday"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_SelectDateRange_Today %>" Value="Today"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_SelectDateRange_LastFullWeek%>" Value="Last Full Week"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_SelectDateRange_ThisWeek %>" Value="This Week"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_SelectDateRange_LastFullMonth %>" Value="Last Full Month"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_SelectDateRange_ThisMonth %>" Value="This Month"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>


                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblDateFilter" runat="server" AssociatedControlID="RP__DateRange" Text="<%$ Resources:lblDateFilter %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__DateType" runat="server" CssClass="field-medium form-control form-select">
                            <asp:ListItem Text="<%$ Resources:li_DateFilter_LossDate %>" Value="Loss Date"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_DateFilter_ReportedDate %>" Value="Reported Date"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>



                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblCurrencyType" runat="server" AssociatedControlID="RP__TypeOfCurrency" Text="<%$ Resources:lbl_CurrencyType %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__TypeOfCurrency" runat="server" CssClass="field-medium form-control form-select">
                            <asp:ListItem Text="<%$ Resources:li_CurrencyType_System %>" Value="System"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_CurrencyType_Base %>" Value="Base"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_CurrencyType_Transaction%>" Value="Transaction"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>


                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblGroupBy" runat="server" AssociatedControlID="RP__GroupByCode" Text="<%$ Resources:lbl_GroupBy %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__GroupByCode" runat="server" CssClass="field-medium form-control form-select">
                            <asp:ListItem Text="<%$ Resources:li_GroupBy_NoGrouping %>" Value="No Grouping"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_GroupBy_Branch %>" Value="Branch"></asp:ListItem>

                        </asp:DropDownList>
                    </div>
                </div>
                <asp:HiddenField ID="RP__TPACode" runat="server"></asp:HiddenField>

            </div>
        </div>
        <div class="card-footer">
            <asp:LinkButton ID="btnGenerateReport" runat="server" Text="<%$ Resources:btnGenerateReport %>" OnClick="GenerateReport" ValidationGroup="vldReportsControlsGroup" SkinID="btnPrimary"></asp:LinkButton>
        </div>
    </div>

</div>
