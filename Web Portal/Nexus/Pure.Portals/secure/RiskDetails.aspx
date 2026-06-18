<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="RiskDetails.aspx.vb" Inherits="Nexus.secure_RiskDetails" Title="Rating Details" %>

<%@ Register Src="~/Controls/ProgressBar.ascx" TagName="ProgressBar" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/RatingDetails.ascx" TagName="RatingDetails" TagPrefix="uc3" %>
<%@ Register Src="~/Controls/RiskFees.ascx" TagName="RiskFeesCntrl" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/RiskTax.ascx" TagName="RiskTaxCntrl" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cntMainBody" runat="Server">
    <asp:ScriptManager ID="smRatingDetail" runat="server"></asp:ScriptManager>
     <!-- Hidden field to track active tab -->
    <asp:HiddenField ID="hdnActiveTab" runat="server" />
    <script language="javascript" type="text/javascript">
        function ConfirmRIMsg(sMessage) {
            var ret = confirm(sMessage);
            return ret;
        }
        function clearTabAndSubmit() {
            localStorage.removeItem('activeMainTab');
            __doPostBack('<%= btnSubmit.UniqueID %>', '');
        }

       // Handle tab memory
        document.addEventListener("DOMContentLoaded", function () {
            const activeTabField = document.getElementById('<%= hdnActiveTab.ClientID %>');

           // Restore tab on load from localStorage
           let storedTab = localStorage.getItem("activeMainTab");
           if (storedTab) {
               const tabToActivate = document.querySelector('[href="' + storedTab + '"]');
               if (tabToActivate) {
                   new bootstrap.Tab(tabToActivate).show();
                   activeTabField.value = storedTab;
               }
           }

           // When tab is changed
           document.querySelectorAll('a[data-bs-toggle="tab"]').forEach(function (tabLink) {
               tabLink.addEventListener("shown.bs.tab", function (e) {
                   const tabId = e.target.getAttribute("href");
                   activeTabField.value = tabId;
                   localStorage.setItem("activeMainTab", tabId);
               });
           });
       });
    </script>
    <div id="secure_RiskDetails">
        <uc1:ProgressBar ID="ucProgressBar" runat="server" />
        <asp:Panel ID="pnlRiskDetls" runat="server" Visible="true" EnableViewState="true">
            <div class="card">
                
                <div class="md-whiteframe-z0 bg-white">
                    <ul class="nav nav-lines nav-tabs b-danger">
                        <li>
                            <a data-bs-toggle="tab" href="#tab-Ratingdetail" aria-expanded="true" class="active">Rating Details</a>
                        </li>
                    </ul>
                </div>
                <div class="tab-content clearfix p b-t b-t-2x">
                    <div role="tabpanel" class="tab-pane animated fadeIn active show" id="tab-Ratingdetail">
                        <div class="md-whiteframe-z0 bg-white">
                            <asp:Button runat="server" ID="btnBacktoSummary" Text="Back to Client Details" CssClass="btn btn-sm btn-primary float-right px-4" />
                            <ul class="nav nav-lines nav-tabs b-danger">
                                <li>
                                    <a href="#tab-Ratingdetails" aria-expanded="true" data-bs-toggle="tab" class="active" aria-selected="true" >
                                        <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:lbl_tab_Ratingdetails %>" /></a></li>
                                <li><a href="#tab-RiskFee" aria-expanded="false" data-bs-toggle="tab">
                                    <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:lbl_tab_RiskFee %>" /></a></li>
                                <li><a href="#tab-RiskTax" aria-expanded="false" data-bs-toggle="tab">
                                    <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:lbl_tab_RiskTax %>" /></a></li>
                            </ul>                                  
                        </div>                      
                        <div class="tab-content clearfix p b-t b-t-2x">
                            <div id="tab-Ratingdetails" class="tab-pane animated fadeIn show active" role="tabpanel">
                                <h5>
                                    <asp:Literal ID="litRatingDetails" runat="server" Text="<%$ Resources:lbl_RatingDetails %>" /></h5>

                                <div class="card-body no-padding clearfix">
                                    <uc3:RatingDetails ID="RatingDetails1" runat="server" DataType="Table" AttributeName="RatingSectionType,RATETYPE,AnnualRate,SumInsured,ThisPremium,AnnualPremium"
                                        ShowTableHeaderRow="True" TableRowHeaders="Rating Section Type,Rate Type,Rate ,Sum Insured([!Currency!]),Premium([!Currency!]), Annual Premium([!Currency!])"
                                        TotalAttributeName="SumInsured,ThisPremium,AnnualPremium"
                                        ShowOriginalPremium="false" />
                                    <uc3:RatingDetails ID="RatingDetails2" Visible="false" runat="server" DataType="Table" AttributeName="RatingSectionType,RATETYPE,AnnualRate,SumInsured,ThisPremium,AnnualPremium"
                                        ShowTableHeaderRow="True" TableRowHeaders="Rating Section Type,Rate Type,Rate ,Sum Insured([!Currency!]),Premium([!Currency!]),Annual Premium([!Currency!])"
                                        TotalAttributeName="SumInsured,ThisPremium,AnnualPremium"
                                        ShowOriginalPremium="true" />
                                </div>
                            </div>
                            <div id="tab-RiskFee" class="tab-pane animated fadeIn" role="tabpanel">
                                <asp:UpdatePanel ID="updFee" runat="server">
                                    <ContentTemplate>
                                        <uc1:RiskFeesCntrl ID="RiskFeesCntrl" runat="server" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div id="tab-RiskTax" class="tab-pane animated fadeIn" role="tabpanel">
                                <uc2:RiskTaxCntrl ID="RiskTaxCntrl" runat="server" />
                            </div>
                        </div>

                        <div id="tab-overview">
                            <div class="form-horizontal clear">
                                <div class="row">
                                    <div class="col-sm-6 col-md-3">
                                        <div class="md-list md-whiteframe-z0 m-b">
                                            <div class="md-list-item">
                                                <div class="md-list-item-left circle lt">
                                                    <i class="mdi-action-grade i-24"></i>
                                                </div>
                                                <div class="md-list-item-content">
                                                    <h3 class="text-md">
                                                        <asp:Label ID="lblNetTotal" AssociatedControlID="txtNetTotal" runat="server" Text="<%$ Resources:lbl_NetTotal %>"></asp:Label></h3>
                                                    <small class="font-thin">
                                                        <asp:Label ID="txtNetTotal" CssClass="field-medium" runat="server"></asp:Label></small>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-6 col-md-3">
                                        <div class="md-list md-whiteframe-z0 m-b">
                                            <div class="md-list-item">
                                                <div class="md-list-item-left circle lt">
                                                    <i class="mdi-action-grade i-24"></i>
                                                </div>
                                                <div class="md-list-item-content">
                                                    <h3 class="text-md">
                                                        <asp:Label ID="lblFeeTotal" AssociatedControlID="txtFeeTotal" runat="server" Text="<%$ Resources:lbl_FeeTotal %>"></asp:Label></h3>
                                                    <small class="font-thin">
                                                        <asp:Label ID="txtFeeTotal" CssClass="field-medium" runat="server"></asp:Label></small>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-6 col-md-3">
                                        <div class="md-list md-whiteframe-z0 m-b">
                                            <div class="md-list-item">
                                                <div class="md-list-item-left circle lt">
                                                    <i class="mdi-action-grade i-24"></i>
                                                </div>
                                                <div class="md-list-item-content">
                                                    <h3 class="text-md">
                                                        <asp:Label ID="lblTaxTotal" AssociatedControlID="txtTaxTotal" runat="server" Text="<%$ Resources:lbl_TaxTotal %>"></asp:Label></h3>
                                                    <small class="font-thin">
                                                        <asp:Label ID="txtTaxTotal" CssClass="field-medium" runat="server"></asp:Label></small>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-6 col-md-3">
                                        <div class="md-list md-whiteframe-z0 m-b">
                                            <div class="md-list-item">
                                                <div class="md-list-item-left circle lt">
                                                    <i class="mdi-action-grade i-24"></i>
                                                </div>
                                                <div class="md-list-item-content">
                                                    <h3 class="text-md">
                                                        <asp:Label ID="lblGrossTotal" AssociatedControlID="txtGrossTotal" runat="server"
                                                            Text="<%$ Resources:lbl_GrossTotal%>"></asp:Label></h3>
                                                    <small class="font-thin">
                                                        <asp:Label ID="txtGrossTotal" CssClass="field-medium" runat="server"></asp:Label></small>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="card-footer" id="divButton" runat="server">
                                <asp:Literal ID="lblReview" runat="server" Text="<%$ Resources:lbl_Review %>" />
                                <asp:LinkButton ID="btnCancel" SkinID="btnSecondary" runat="server" Text="<%$ Resources:btn_Cancel %>"
                                    CausesValidation="false" TabIndex="4" />
                                <asp:LinkButton ID="btnSubmit" SkinID="btnPrimary" runat="server" Text="<%$ Resources:btn_Submit%>"  OnClientClick="clearTabAndSubmit(); return false;"
                                    TabIndex="3" />

                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </asp:Panel>
        <div class="submitarea d-flex justify-content-end">
            <asp:Button runat="server" ID="btnOk" Text="Ok" CssClass="btn btn-sm btn-primary float-right px-4" />
        </div>
    </div>
</asp:Content>
