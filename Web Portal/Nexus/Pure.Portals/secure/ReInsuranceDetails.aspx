<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false" CodeFile="ReInsuranceDetails.aspx.vb" Inherits="Nexus.secure_ReInsuranceDetails" Title="Re Insurance" %>

<%@ Register Src="~/Controls/ProgressBar.ascx" TagName="ProgressBar" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Reinsurance.ascx" TagName="ReInsurance" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/Reinsurance2007.ascx" TagName="RI2007" TagPrefix="uc4" %>
<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cntMainBody" runat="Server">
    <asp:ScriptManager ID="smRatingDetail" runat="server"></asp:ScriptManager>
 
    <%--<nexus:TabIndex ID="ctrlTabIndex" runat="server" CssClass="TabContainer" TabContainerClass="page-progress"
        ActiveTabClass="ActiveTab" DisabledClass="DisabledTab" Scrollable="false" />--%>
    <div id="secure_ReInsurance">
        <div class="nexus-fluid-layout">
            <uc1:ProgressBar ID="ucProgressBar" runat="server" />
            <div class="page-container">
                <div class="page-container-content">
                    <div class="top-corners">
                    </div>
                    <asp:Panel ID="pnlReInsurance" runat="server" Visible="true" EnableViewState="true">
                        <div class="standard-form card">
                            <div class="nexus-tabs md-whiteframe-z0 bg-white">                                 
                                <ul class="tab-container nav nav-lines nav-tabs b-danger">                                    
                                    <li id="liReinsuranceCntrl" runat="server" class="active"><a href="#tab-ReinsuranceCntrl">Reinsurance
                                        Details</a></li>
                                </ul>
                                <div id="tab-ReinsuranceCntrl">
                                    <div class="top-right-padding">                                     
                                         <asp:Button runat="server" ID="btnBacktoSummary" Text="Back to Client Details" CssClass="btn btn-sm btn-primary float-right px-4" />
                                     </div>   
                                        
                                    <uc2:ReInsurance ID="ReInsuranceCntrl" runat="server" Visible="false" />
                                    <uc4:RI2007 ID="ReInsurance2007Cntrl" runat="server" Visible="false" />
                                    <div class="clear">
                                        &nbsp;
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>

                </div>
            </div>
            <div class="bottom-corners">
                <div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
