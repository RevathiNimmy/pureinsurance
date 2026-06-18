<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/default.master" CodeFile="MOTORCC_DRIVERDETAILS.aspx.vb" Inherits="Nexus.MOTORCC_DRIVERDETAILS" %>

<%@ Register Src="~/Controls/ProgressBar.ascx" TagName="ProgressBar" TagPrefix="NexusControl" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="NexusControl" %>
<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>

<asp:content id="cntMainBody" contentplaceholderid="cntMainBody" runat="Server" xmlns:asp="remove" xmlns:nexus="remove" xmlns:nexuscontrol="remove" xmlns:nexusprovider="remove">
    <div class="risk-screen">
      <NexusControl:ProgressBar ID="ucProgressBar" runat="server"></NexusControl:ProgressBar>
      <div class="card">
        <nexus:tabindex id="ctrlTabIndex" runat="server" cssclass="TabContainer" tabcontainerclass="page-progress" activetabclass="ActiveTab" disabledclass="DisabledTab" scrollable="false"></nexus:tabindex>
           <div class="card-body clearfix">
                <legend><span>Driver Details</span></legend>
                <div class="grid-card table-responsive">
                  <nexus:itemgrid id="MS__DRIVER" runat="server" screencode="DRIVERDETA" autogeneratecolumns="false" gridlines="None" childpage="Child/MemDetails_General.aspx">
                      <columns>
                          <nexus:riskattribute headertext="Name" datafield="DD_NAME"></nexus:riskattribute>
                          <nexus:riskattribute headertext="Date of Birth" datafield="DD_DOB" dataformatstring="{0:d}"></nexus:riskattribute>
                          <nexus:riskattribute headertext="Licence Number" datafield="DD_DRILIC_NO"></nexus:riskattribute>
                      </columns>
                  </nexus:itemgrid>
                </div>
                <asp:textbox runat="server" id="txtDriverDetails" style="display:none;"></asp:textbox>
                <asp:requiredfieldvalidator id="rqdDriverDetails" runat="server" controltovalidate="txtDriverDetails" display="none" enabled="true" setfocusonerror="true" errormessage="Please provide atleast 1 Driver Detail."></asp:requiredfieldvalidator>
          </div>
          <div class='card-footer'>
            <asp:LinkButton id="btnBack" runat="server" Text="<i class='fa fa-chevron-left' aria-hidden='true'></i> Back" onclick="BackButton" causesvalidation="false" SkinID="btnSecondary"></asp:LinkButton>
            <asp:LinkButton id="btnNext" runat="server" Text="Next <i class='fa fa-chevron-right' aria-hidden='true'></i>" onclick="NextButton" SkinID="btnPrimary"></asp:LinkButton>
            <asp:LinkButton id="btnFinish" runat="server" Text="<i class='fa fa-check' aria-hidden='true'></i> Finish" onclick="FinishButton" onprerender="PreRenderFinish" SkinID="btnPrimary"></asp:LinkButton>
          </div>
      </div>
        <asp:validationsummary id="vldsumSummary" displaymode="BulletList" headertext="Summary" runat="server" cssclass="validation-summary"></asp:validationsummary>
    </div>
</asp:content>
