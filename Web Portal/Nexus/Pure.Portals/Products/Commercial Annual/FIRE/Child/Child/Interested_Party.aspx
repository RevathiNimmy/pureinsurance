<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/default.master" CodeFile="Interested_Party.aspx.vb" Inherits="Nexus.BLDGSP_GENERAL" %>

<%@ Register Src="~/Controls/ProgressBar.ascx" TagName="ProgressBar" TagPrefix="NexusControl" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="NexusControl" %>
<%@ Register Src="~/controls/AddressCntrl.ascx" TagName="AddressCntrl" TagPrefix="NexusControl" %>
<%@ Register Src="~/controls/StandardWordings.ascx" TagName="StandardWording" TagPrefix="NexusControl" %>
<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>

<asp:content id="cntMainBody" contentplaceholderid="cntMainBody" runat="Server" xmlns:asp="remove" xmlns:nexus="remove" xmlns:nexuscontrol="remove" xmlns:nexusprovider="remove">
    <div class="risk-screen">
      <NexusControl:ProgressBar ID="ucProgressBar" runat="server"></NexusControl:ProgressBar>
      <div class="card">
        <nexus:tabindex id="ctrlTabIndex" runat="server" cssclass="TabContainer" tabcontainerclass="page-progress" activetabclass="ActiveTab" disabledclass="DisabledTab" scrollable="false"></nexus:tabindex>
        <div class="card-body clearfix">
              <div class="form-horizontal">
                <legend>Interested Party</legend>
                  <div id="liIPNAME__IPARTY_NAME" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                      <label class="col-md-4 col-sm-3 control-label">Name:</label>
                      <div class="col-md-8 col-sm-9">
                          <asp:textbox id="PNAME__IPARTY_NAME" runat="server" cssclass="form-control"></asp:textbox>

                      </div>
                  </div>
                </div>
        </div> 
          <div class='card-footer'>
             <asp:LinkButton id="btnBack" runat="server" Text="<i class='fa fa-chevron-left' aria-hidden='true'></i> Back" onclick="BackButton" SkinID="btnSecondary"></asp:LinkButton>
             <asp:LinkButton id="btnNext" runat="server" Text="Next <i class='fa fa-chevron-right' aria-hidden='true'></i>" onclick="NextButton" SkinID="btnPrimary"></asp:LinkButton>
            <asp:LinkButton id="btnFinish" runat="server" Text="<i class='fa fa-check' aria-hidden='true'></i> Finish" onclick="FinishButton" onprerender="PreRenderFinish" SkinID="btnPrimary"></asp:LinkButton>
          </div>
      </div>
          <asp:validationsummary id="vldsumSummary" displaymode="BulletList" headertext="Summary" runat="server" cssclass="validation-summary"></asp:validationsummary>
    </div>
</asp:content>
