
      <%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/default.master" CodeFile="FIRE_OFFICECONTENTSX.aspx.vb" Inherits="Nexus.FIRE_OFFICECONTENTSX" %>
    
 <%@ Register Src="~/Controls/ProgressBar.ascx" TagName="ProgressBar" TagPrefix="NexusControl" %>
 <%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="NexusControl" %>
 <%@ Register Src="~/controls/AddressCntrl.ascx" TagName="AddressCntrl" TagPrefix="NexusControl" %>
 <%@ Register Src="~/controls/StandardWordings.ascx" TagName="StandardWording" TagPrefix="NexusControl" %>
 <%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
 <%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" Runat="Server" xmlns:asp="remove" xmlns:Nexus="remove" xmlns:NexusControl="remove" xmlns:NexusProvider="remove">
  <div class="risk-screen">
    
  
      <NexusControl:ProgressBar ID="ucProgressBar" runat="server"></NexusControl:ProgressBar>
      <div class="card">
        <nexus:tabindex id="ctrlTabIndex" runat="server" cssclass="TabContainer" tabcontainerclass="page-progress" activetabclass="ActiveTab" disabledclass="DisabledTab" scrollable="false"></nexus:tabindex>
        <div class="card-body clearfix">
          
          <div class="standard-form">
            <div class="fieldset-wrapper">
              
              <div class="form-horizontal">
                <legend><span>Reinsurance Exposure</span></legend>
                <div><label>Total Sum Insured</label></div>
                <div><label>Target Risk SI</label></div>
                <div><label>MPL %</label></div>
                <div><label>RI Exposure</label></div>
                
              
                  <div id="liOC__CONTENT_TOTAL_SI" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12"><label class="col-md-4 col-sm-3 control-label">Contents</label><div class="col-md-8 col-sm-9"><asp:textbox id="OC__CONTENT_TOTAL_SI" runat="server" cssclass="form-control"></asp:textbox></div>
                  </div>
                  <div id="liOC__CONTENT_TARGET_RISK_SI" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12"><label class="col-md-4 col-sm-3 control-label"></label><div class="col-md-8 col-sm-9"><asp:textbox id="OC__CONTENT_TARGET_RISK_SI" runat="server" cssclass="form-control"></asp:textbox></div>
                  </div>
                  <div id="liOC__CONTENT_MPL" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12"><label class="col-md-4 col-sm-3 control-label"></label><div class="col-md-8 col-sm-9"><asp:textbox id="OC__CONTENT_MPL" runat="server" cssclass="form-control"></asp:textbox></div>
                  </div>
                  <div id="liOC__CONTENT_RI_EXPOSURE" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12"><label class="col-md-4 col-sm-3 control-label"></label><div class="col-md-8 col-sm-9"><asp:textbox id="OC__CONTENT_RI_EXPOSURE" runat="server" cssclass="form-control"></asp:textbox></div>
                  </div>
                  <div id="liOC__RENT_TOTAL_SI" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12"><label class="col-md-4 col-sm-3 control-label">25% Rent</label><div class="col-md-8 col-sm-9"><asp:textbox id="OC__RENT_TOTAL_SI" runat="server" cssclass="form-control"></asp:textbox></div>
                  </div>
                  <div id="liOC__RENT_TARGET_RISK_SI" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12"><label class="col-md-4 col-sm-3 control-label"></label><div class="col-md-8 col-sm-9"><asp:textbox id="OC__RENT_TARGET_RISK_SI" runat="server" cssclass="form-control"></asp:textbox></div>
                  </div>
                  <div id="liOC__RENT_MPL" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12"><label class="col-md-4 col-sm-3 control-label"></label><div class="col-md-8 col-sm-9"><asp:textbox id="OC__RENT_MPL" runat="server" cssclass="form-control"></asp:textbox></div>
                  </div>
                  <div id="liOC__RENT_RI_EXPOSURE" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12"><label class="col-md-4 col-sm-3 control-label"></label><div class="col-md-8 col-sm-9"><asp:textbox id="OC__RENT_RI_EXPOSURE" runat="server" cssclass="form-control"></asp:textbox></div>
                  </div>
                  <div id="liOC__ICOST_SI" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12"><label class="col-md-4 col-sm-3 control-label">Increased in 
Cost of Working</label><div class="col-md-8 col-sm-9"><asp:textbox id="OC__ICOST_SI" runat="server" cssclass="form-control"></asp:textbox></div>
                  </div>
                  <div id="liOC__ICOST_TARGET_RISK_SI" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12"><label class="col-md-4 col-sm-3 control-label"></label><div class="col-md-8 col-sm-9"><asp:textbox id="OC__ICOST_TARGET_RISK_SI" runat="server" cssclass="form-control"></asp:textbox></div>
                  </div>
                  <div id="liOC__ICOST_MPL" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12"><label class="col-md-4 col-sm-3 control-label"></label><div class="col-md-8 col-sm-9"><asp:textbox id="OC__ICOST_MPL" runat="server" cssclass="form-control"></asp:textbox></div>
                  </div>
                  <div id="liOC__ICOST_RI_EXPOSURE" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12"><label class="col-md-4 col-sm-3 control-label"></label><div class="col-md-8 col-sm-9"><asp:textbox id="OC__ICOST_RI_EXPOSURE" runat="server" cssclass="form-control"></asp:textbox></div>
                  </div>
                  <div id="liOC__LI_TOTAL_SI" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12"><label class="col-md-4 col-sm-3 control-label">Liability Document
Loss of Document</label><div class="col-md-8 col-sm-9"><asp:textbox id="OC__LI_TOTAL_SI" runat="server" cssclass="form-control"></asp:textbox></div>
                  </div>
                  <div id="liOC__LI_TARGET_RISK_SI" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12"><label class="col-md-4 col-sm-3 control-label"></label><div class="col-md-8 col-sm-9"><asp:textbox id="OC__LI_TARGET_RISK_SI" runat="server" cssclass="form-control"></asp:textbox></div>
                  </div>
                  <div id="liOC__LI_MPL" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12"><label class="col-md-4 col-sm-3 control-label"></label><div class="col-md-8 col-sm-9"><asp:textbox id="OC__LI_MPL" runat="server" cssclass="form-control"></asp:textbox></div>
                  </div>
                  <div id="liOC__LI_RI_EXPOSURE" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12"><label class="col-md-4 col-sm-3 control-label"></label><div class="col-md-8 col-sm-9"><asp:textbox id="OC__LI_RI_EXPOSURE" runat="server" cssclass="form-control"></asp:textbox></div>
                  </div>
                  <div id="liOC__CL_TOTAL_SI" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12"><label class="col-md-4 col-sm-3 control-label">Claims Preparation 
Costs</label><div class="col-md-8 col-sm-9"><asp:textbox id="OC__CL_TOTAL_SI" runat="server" cssclass="form-control"></asp:textbox></div>
                  </div>
                  <div id="liOC__CL_TARGET_RISK_SI" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12"><label class="col-md-4 col-sm-3 control-label"></label><div class="col-md-8 col-sm-9"><asp:textbox id="OC__CL_TARGET_RISK_SI" runat="server" cssclass="form-control"></asp:textbox></div>
                  </div>
                  <div id="liOC__CL_MPL" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12"><label class="col-md-4 col-sm-3 control-label"></label><div class="col-md-8 col-sm-9"><asp:textbox id="OC__CL_MPL" runat="server" cssclass="form-control"></asp:textbox></div>
                  </div>
                  <div id="liOC__CL_RI_EXPOSURE" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12"><label class="col-md-4 col-sm-3 control-label"></label><div class="col-md-8 col-sm-9"><asp:textbox id="OC__CL_RI_EXPOSURE" runat="server" cssclass="form-control"></asp:textbox></div>
                  </div>
                  <div id="liOC__AIW_TOTAL_SI" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12"><label class="col-md-4 col-sm-3 control-label">Additional Increase in
 Cost of Working</label><div class="col-md-8 col-sm-9"><asp:textbox id="OC__AIW_TOTAL_SI" runat="server" cssclass="form-control"></asp:textbox></div>
                  </div>
                  <div id="liOC__AIW_TARGET_RISK_SI" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12"><label class="col-md-4 col-sm-3 control-label"></label><div class="col-md-8 col-sm-9"><asp:textbox id="OC__AIW_TARGET_RISK_SI" runat="server" cssclass="form-control"></asp:textbox></div>
                  </div>
                  <div id="liOC__AIW_MPL" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12"><label class="col-md-4 col-sm-3 control-label"></label><div class="col-md-8 col-sm-9"><asp:textbox id="OC__AIW_MPL" runat="server" cssclass="form-control"></asp:textbox></div>
                  </div>
                  <div id="liOC__AIW_RI_EXPOSURE" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12"><label class="col-md-4 col-sm-3 control-label"></label><div class="col-md-8 col-sm-9"><asp:textbox id="OC__AIW_RI_EXPOSURE" runat="server" cssclass="form-control"></asp:textbox></div>
                  </div>
                  <div id="liOC__TRE_TOTAL_SI" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12"><label class="col-md-4 col-sm-3 control-label">Total RI Exposure</label><div class="col-md-8 col-sm-9"><asp:textbox id="OC__TRE_TOTAL_SI" runat="server" cssclass="form-control"></asp:textbox></div>
                  </div>
                  <div id="liOC__TRE_TARGET_RISK_SI" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12"><label class="col-md-4 col-sm-3 control-label"></label><div class="col-md-8 col-sm-9"><asp:textbox id="OC__TRE_TARGET_RISK_SI" runat="server" cssclass="form-control"></asp:textbox></div>
                  </div>
                  <div id="liOC__TRE_RI_EXPOSURE" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12"><label class="col-md-4 col-sm-3 control-label"></label><div class="col-md-8 col-sm-9"><asp:textbox id="OC__TRE_RI_EXPOSURE" runat="server" cssclass="form-control"></asp:textbox></div>
                  </div>
                </div>
            </div>
            
          </div>
          <asp:validationsummary id="vldsumSummary" displaymode="BulletList" headertext="Summary" runat="server" cssclass="validation-summary"></asp:validationsummary>
          
        </div><div class='card-footer'>
            <asp:LinkButton id="btnBack" runat="server" Text="<i class='fa fa-chevron-left' aria-hidden='true'></i> Back" onclick="BackButton" SkinID="btnSecondary"></asp:LinkButton>
            <asp:LinkButton id="btnNext" runat="server" Text="Next <i class='fa fa-chevron-right' aria-hidden='true'></i>" onclick="NextButton" SkinID="btnSecondary"></asp:LinkButton>
            <asp:LinkButton id="btnFinish" runat="server" Text="<i class='fa fa-check' aria-hidden='true'></i> Finish" onclick="FinishButton" onprerender="PreRenderFinish" SkinID="btnSecondary"></asp:LinkButton>
          </div>
        
      </div>
    </div>
</asp:Content>