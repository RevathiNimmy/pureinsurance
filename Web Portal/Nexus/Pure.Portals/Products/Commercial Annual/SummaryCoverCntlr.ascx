<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SummaryCoverCntlr.ascx.vb"
    Inherits="Products_TestMOTOR_TestMOTOR_SummaryOfCover" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register Src="~/Controls/PolicyFees.ascx" TagName="PolicyFeesCntrl" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/PolicyTax.ascx" TagName="PolicyTaxCntrl" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/AgentCommission.ascx" TagName="AgentCommissionCntrl"
    TagPrefix="uc3" %>
<%@ Register Src="~/Controls/DocumentList.ascx" TagName="DocumentListCtrl" TagPrefix="uc4" %>
<%@ Register Src="~/Controls/PolicyDetails.ascx" TagName="PolicyDetails" TagPrefix="uc7" %>
<%@ Register Src="~/Controls/Document.ascx" TagName="Document" TagPrefix="uc5" %>
<%@ Register Src="~/Controls/RiskFees.ascx" TagName="RiskFeesCntrl" TagPrefix="uc5" %>
<%@ Register Src="~/Controls/RiskData.ascx" TagName="RiskData" TagPrefix="uc6" %>
<%@ Register Src="~/Controls/FileView.ascx" TagName="FileView" TagPrefix="uc13" %>
<%@ Register Src="~/Controls/EditTax.ascx" TagName="AllTax" TagPrefix="uc14" %>
<%@ Register Src="~/Controls/RiskTax.ascx" TagName="RiskTaxCntrl" TagPrefix="uc15" %>
<%@ Register Src="~/Controls/DocumentManager.ascx" TagName="DocumentManager" TagPrefix="uc10" %>
<uc1:PolicyFeesCntrl ID="PolicyFeesCntrl" runat="server"></uc1:PolicyFeesCntrl>
<uc2:PolicyTaxCntrl ID="PolicyTaxCntrl" runat="server"></uc2:PolicyTaxCntrl>
<uc3:AgentCommissionCntrl ID="AgentCommissionCntrl" runat="server"></uc3:AgentCommissionCntrl>
<uc7:PolicyDetails ID="ucPolicyDetails" runat="server"></uc7:PolicyDetails>

<asp:Label ID="lbl_errmessage" runat="server" Text="<%$ Resources:lbl_errormessage%>" CssClass="error"></asp:Label>
<asp:Label ID="lbl_Writtenerrmessage" runat="server" Text="<%$ Resources:lbl_Writtenerrmessage%>" CssClass="error" Visible="false"></asp:Label>
<asp:Label ID="lbl_mtareinserrmessage" runat="server" Text="<%$ Resources:lbl_mtareinserrmessage%>" CssClass="error" Visible="false"></asp:Label>
<div id="divDME" runat="server">
    <uc10:DocumentManager ID="docMgr1" runat="server" Visible="true" Documents="Quotation" EnableEditDocument="true"></uc10:DocumentManager>
    <div class="card-body clearfix no-padding m-b-sm">
        <div class="form-horizontal">

            <legend>
                <asp:Literal ID="ltHeading" runat="server" Text="<%$ Resources:lbl_ltHeading%>"></asp:Literal></legend>

            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                <asp:Label ID="lblPremiumDisplay" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="lblPremiumValue">
                    <asp:Literal ID="litPremiumDisplay" runat="server" Text="<%$ Resources:lbl_PremiumDisplay%>"></asp:Literal></asp:Label>
                <div class="col-md-8 col-sm-9">
                    <p class="form-control-static font-bold">
                        <asp:Label ID="lblPremiumValue" runat="server"></asp:Label>
                    </p>
                </div>

            </div>
            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                <asp:Label ID="lblTotalTax" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="lblTotalTaxValue">
                    <asp:Literal ID="litTotalTax" runat="server" Text="<%$ Resources:lbl_Tax%>"></asp:Literal></asp:Label>
                <div class="col-md-8 col-sm-9">
                    <p class="form-control-static font-bold">
                        <asp:Label ID="lblTotalTaxValue" runat="server"></asp:Label>
                    </p>
                </div>

            </div>
            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                <asp:Label ID="lblTotalCommission" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="lblTotalCommissionValue">
                    <asp:Literal ID="litTotalCommission" runat="server" Text="<%$ Resources:lbl_Commission%>"></asp:Literal>

                </asp:Label>
                <div class="col-md-8 col-sm-9">
                    <p class="form-control-static font-bold">
                        <asp:Label ID="lblTotalCommissionValue" runat="server"></asp:Label>
                    </p>
                </div>

            </div>
            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                <asp:Button CssClass="submit" ID="Btn_PreviousYear" runat="server" Text="<%$ Resources:btn_PreviousYear%>" CausesValidation="False" Visible="false"></asp:Button>
                <asp:Button CssClass="submit" ID="Btn_CurrentYear" runat="server" Text="<%$ Resources:btn_CurrentYear%>" CausesValidation="False" Visible="false"></asp:Button>
            </div>

            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12" style="display: none;">
                <asp:Label runat="server" ID="lblPolicyNo" Text="<%$ Resources:lbl_PolicyNo%>" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtPolicyNo"></asp:Label>
                <div class="col-md-8 col-sm-9">
                    <p class="form-control-static font-bold">
                        <asp:Label runat="server" ID="txtPolicyNo" Font-Bold="true"></asp:Label>
                    </p>
                </div>
 
            </div>
			
		
            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                <asp:Label runat="server" ID="lbl_PolicyHolder" Text="<%$ Resources:lbl_PolicyHolder%>" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtPolicyHolder"></asp:Label>
                <div class="col-md-8 col-sm-9">
                    <p class="form-control-static font-bold">
                        <asp:Label runat="server" ID="txtPolicyHolder" Height="50"></asp:Label>
                    </p>
                </div>

            </div>
            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                <asp:Label runat="server" ID="lbl_InceptionDate" Text="<%$ Resources:lbl_InceptionDate%>" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtInceptionDate"></asp:Label>
                <div class="col-md-8 col-sm-9">
                    <p class="form-control-static font-bold">
                        <asp:Label runat="server" ID="txtInceptionDate"></asp:Label>
                    </p>
                </div>

            </div>
            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                <asp:Label runat="server" ID="lbl_BrokerName" Text="<%$ Resources:lbl_BrokerName%>" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="PCATLIN__HIDDENBROKERNAME"></asp:Label>
                <div class="col-md-8 col-sm-9">
                    <p class="form-control-static font-bold">
                        <asp:Label runat="server" ID="PCATLIN__HIDDENBROKERNAME" Height="50"></asp:Label>
                    </p>
                </div>

            </div>
            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12" style="display: none;">
                <asp:Label runat="server" ID="Label4" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="PCATLIN__COMMISSION"></asp:Label>
                <div class="col-md-8 col-sm-9">
                    <p class="form-control-static font-bold">
                        <asp:Label runat="server" ID="PCATLIN__COMMISSION"></asp:Label>
                    </p>
                </div>

            </div>
            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12" style="display: none;">
                <asp:Label runat="server" ID="Label10" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="PCATLIN__STRCOMMISSION"></asp:Label>
                <div class="col-md-8 col-sm-9">
                    <p class="form-control-static font-bold">
                        <asp:Label runat="server" ID="PCATLIN__STRCOMMISSION"></asp:Label>
                    </p>
                </div>

            </div>
            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12" style="display: none;">
                <asp:Label runat="server" ID="Label6" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="PCATLIN__LASTMODIFYDATE"></asp:Label>
                <div class="col-md-8 col-sm-9">
                    <p class="form-control-static font-bold">
                        <asp:Label runat="server" ID="PCATLIN__LASTMODIFYDATE"></asp:Label>
                    </p>
                </div>

            </div>
            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                <asp:Label runat="server" ID="lbl_InceptiontTime" Text="<%$ Resources:lbl_InceptionTime%>" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="PCATLIN__INCEPTIONTIME"></asp:Label>
                <div class="col-md-8 col-sm-9">
                    <p class="form-control-static font-bold">
                        <asp:Label runat="server" ID="PCATLIN__INCEPTIONTIME"></asp:Label>
                    </p>
                </div>

            </div>
            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                <asp:Label runat="server" ID="lbl_BrokerCode" Text="<%$ Resources:lbl_BrokerCode%>" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtBrokerCode"></asp:Label>
                <div class="col-md-8 col-sm-9">
                    <p class="form-control-static font-bold">
                        <asp:Label runat="server" ID="txtBrokerCode"></asp:Label>
                    </p>
                </div>

            </div>
            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                <asp:Label runat="server" ID="lbl_Status" Text="<%$ Resources:lbl_Status%>" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtStatus"></asp:Label>
                <div class="col-md-8 col-sm-9">
                    <p class="form-control-static font-bold">
                        <asp:Label runat="server" ID="txtStatus"></asp:Label>
                    </p>
                </div>

            </div>
            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12" style="display: none">
                <NexusProvider:LookupList ID="lkList_POLICYSTATUS" runat="server" DataItemValue="Key" DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="Policy_Status" CssClass="field-medium" DefaultText="(Please Select)"></NexusProvider:LookupList>
                <asp:Label runat="server" ID="PCATLIN__POLICYSTATUS"></asp:Label>
            </div>


            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                <asp:Label runat="server" ID="lbl_AddressLine1" Text="<%$ Resources:lbl_AddressLine1%>" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtAddressLine1"></asp:Label>
                <div class="col-md-8 col-sm-9">
                    <p class="form-control-static font-bold">
                        <asp:Label runat="server" ID="txtAddressLine1"></asp:Label>
                    </p>
                </div>

            </div>
            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12" style="display: none">
                <NexusProvider:LookupList ID="lkList_UNDERWRITER" runat="server" DataItemValue="key" DataItemText="Description" Sort="ASC" ListType="UserDefined" ListCode="UNDERWRITR" DefaultText="(Please Select)"></NexusProvider:LookupList>
                <asp:Label runat="server" ID="PCATLIN__UNDERWRITER"></asp:Label>
            </div>
            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                <asp:Label runat="server" ID="lbl_Underwriter" Text="<%$ Resources:lbl_Underwriter%>" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtUnderwriter"></asp:Label>
                <div class="col-md-8 col-sm-9">
                    <p class="form-control-static font-bold">
                        <asp:Label runat="server" ID="txtUnderwriter"></asp:Label>
                    </p>
                </div>

            </div>


            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                <asp:Label runat="server" ID="lbl_AddressLine2" Text="<%$ Resources:lbl_AddressLine2%>" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtAddressLine2"></asp:Label>
                <div class="col-md-8 col-sm-9">
                    <p class="form-control-static font-bold">
                        <asp:Label runat="server" ID="txtAddressLine2"></asp:Label>
                    </p>
                </div>

            </div>
            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                <asp:Label runat="server" ID="lbl_BrokerContact" Text="<%$ Resources:lbl_BrokerContact%>" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="PCATLIN__BROKERCONTACT"></asp:Label>
                <div class="col-md-8 col-sm-9">
                    <p class="form-control-static font-bold">
                        <asp:Label runat="server" ID="PCATLIN__BROKERCONTACT"></asp:Label>
                    </p>
                </div>

            </div>


            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                <asp:Label runat="server" ID="lbl_AddressLine3" Text="<%$ Resources:lbl_AddressLine3%>" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtAddressLine3"></asp:Label>
                <div class="col-md-8 col-sm-9">
                    <p class="form-control-static font-bold">
                        <asp:Label runat="server" ID="txtAddressLine3"></asp:Label>
                    </p>
                </div>

            </div>


            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                <asp:Label runat="server" ID="lbl_AddressLine4" Text="<%$ Resources:lbl_AddressLine4%>" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtAddressLine4"></asp:Label>
                <div class="col-md-8 col-sm-9">
                    <p class="form-control-static font-bold">
                        <asp:Label runat="server" ID="txtAddressLine4"></asp:Label>
                    </p>
                </div>

            </div>
            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                <asp:Label runat="server" ID="Label8" Text="<%$ Resources:lbl_midclientmanaged %>" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="MIDCLIENTMANAGED"></asp:Label>
                <div class="col-md-8 col-sm-9">

                    <asp:CheckBox ID="MIDCLIENTMANAGED" runat="server" Enabled="false" Text=" " CssClass="asp-check"></asp:CheckBox>

                </div>

            </div>


            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                <asp:Label runat="server" ID="lbl_PostCode" Text="<%$ Resources:lbl_PostCode%>" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtPostCode"></asp:Label>
                <div class="col-md-8 col-sm-9">
                    <p class="form-control-static font-bold">
                        <asp:Label runat="server" ID="txtPostCode"></asp:Label>
                    </p>
                </div>

            </div>
            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                <asp:Label runat="server" ID="Label7" Text="Premium Credit" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="PREMIUMCREDIT"></asp:Label>
                <div class="col-md-8 col-sm-9">

                    <asp:CheckBox ID="PREMIUMCREDIT" runat="server" Enabled="false" Text=" " CssClass="asp-check"></asp:CheckBox>

                </div>

            </div>
            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                <asp:Label ID="Label17" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="PCATLIN__VEHICLE_COUNT" Text="<%$ Resources:lbl_Vehiclecount %>"></asp:Label>
                <div class="col-md-8 col-sm-9">
                    <p class="form-control-static font-bold">
                        <asp:Label runat="server" ID="PCATLIN__VEHICLE_COUNT"></asp:Label>
                    </p>
                </div>

            </div>
            <div id="PremDetails" runat="server" visible="false">

                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label runat="server" ID="lbl_EarnedPremium" Text="<%$ Resources:lbl_EarnedPremium%>" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtEarnedPremium"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <p class="form-control-static font-bold">
                            <asp:Label runat="server" ID="txtEarnedPremium"></asp:Label>
                        </p>
                    </div>

                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label runat="server" ID="lbl_IncurredTodate" Text="<%$ Resources:lbl_IncurredToDate%>" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtIncurredTodate"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <p class="form-control-static font-bold">
                            <asp:Label runat="server" ID="txtIncurredTodate"></asp:Label>
                        </p>
                    </div>

                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label runat="server" ID="lbl_NetLossRatio" Text="<%$ Resources:lbl_NetLossRatio%>" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtNetLossRatio"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <p class="form-control-static font-bold">
                            <asp:Label runat="server" ID="txtNetLossRatio"></asp:Label>
                        </p>
                    </div>

                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label runat="server" ID="lbl_NoOfClaimty" Text="<%$ Resources:lbl_NoOfClaimTY%>" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="hypNoOfClaimty"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:HyperLink ID="hypNoOfClaimty" runat="server"></asp:HyperLink>
                    </div>

                </div>

            </div>
        </div>
    </div>
    <div class="card-body clearfix no-padding">
        <legend><span>Document Details</span></legend>
        <div class="card-footer no-padding m-v-sm">
            <asp:HyperLink ID="HyperLink1" runat="server" SkinID="btnHSM"><i class="fa fa-external-link" aria-hidden="true"></i> Go to DME Page</asp:HyperLink>
        </div>
        <uc13:FileView ID="cnt_FileView" runat="server" EnableFileUpload="true"></uc13:FileView>
    </div>
</div>
<div id="TransDetails" runat="server" visible="false">
    <div class="card-body clearfix no-padding">
        <legend>
            <asp:Label ID="ltPremiumTrans" runat="server" Text="<%$ Resources:lbl_PremiumTransactions%>"></asp:Label>
        </legend>
        <div class="grid-card table-responsive">
            <asp:GridView ID="gvPremiumTransactions" runat="server" AllowSorting="true" AutoGenerateColumns="false" GridLines="None" AllowPaging="True">
                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                <Columns>
                    <asp:BoundField HeaderText="<%$ Resources:bf_TransactionDate%>" DataField="TransDate" HtmlEncode="False" DataFormatString="{0:d}"></asp:BoundField>
                    <asp:BoundField HeaderText="<%$ Resources:bf_EffectiveDate%>" DataField="EffectiveDate" HtmlEncode="False" DataFormatString="{0:d}"></asp:BoundField>
                    <asp:BoundField HeaderText="<%$ Resources:bf_Details%>" DataField="DocumentTypeCode"></asp:BoundField>
                    <asp:BoundField HeaderText="<%$ Resources:bf_Premium%>" DataField="Amount"></asp:BoundField>
                    <asp:BoundField HeaderText="<%$ Resources:bf_Tax%>" DataField="Tax"></asp:BoundField>
                    <asp:BoundField HeaderText="<%$ Resources:bf_Comm%>" DataField="Comm"></asp:BoundField>
                    <asp:BoundField HeaderText="<%$ Resources:bf_AmountDue%>" DataField="OutstandingAmount"></asp:BoundField>
                    <asp:BoundField HeaderText="<%$ Resources:bf_Settled%>" DataField="Settled"></asp:BoundField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>
<div>
    <uc14:AllTax runat="server" ID="uc_Edit_All_Tax"></uc14:AllTax>
		<uc15:RiskTaxCntrl ID="RiskTaxCntrl" runat="server" />
</div>
<asp:HiddenField ID="Frequency" runat="server"></asp:HiddenField>
<div class="card-body clearfix no-padding">
    <legend>
        <asp:Label ID="ltEndorsements" runat="server" Text="<%$ Resources:lbl_Endorsements %>"></asp:Label>
    </legend>
    <div class="grid-card table-responsive">
        <asp:GridView ID="grdStdWording" runat="server" AllowSorting="true" AutoGenerateColumns="false" GridLines="None" AllowPaging="True" Visible="false">
            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
            <Columns>
                <asp:BoundField HeaderText="Code" DataField="Code"></asp:BoundField>
                <asp:BoundField HeaderText="Description" DataField="Description"></asp:BoundField>
            </Columns>
        </asp:GridView>
    </div>
</div>
<uc3:AgentCommissionCntrl runat="server" ID="ctrlAgentCommission" Visible="false"></uc3:AgentCommissionCntrl>
<div id="QuoteDocument" runat="server" visible="true">
    <div class="card-body clearfix no-padding">
        <legend>
            <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lbl_DocumentsHeader %>"></asp:Label></legend>
        <asp:Label runat="server" ID="Label5" Text="<%$ Resources:lbl_DocumentsHeader1 %>"></asp:Label>
        <uc5:Document ID="li_QuoteDocument" runat="server" DocumentName="Quotation - [!ClientName!]_[!TransactionType!]_[!InsuranceFileKey!]" PreGenerate="false" Visible="true" Text="<%$ Resources:lbl_QuoteDocument %>"></uc5:Document>

    </div>
</div>
<div id="CvrDtls" runat="server" visible="false">
    <div class="card-body clearfix no-padding">
        <legend>
            <asp:Label ID="lblDocument" runat="server" Text="<%$ Resources:lbl_DocumentsHeader %>"></asp:Label></legend>

        <asp:Label runat="server" ID="lblHeader" Text="<%$ Resources:lbl_DocumentsHeader1 %>"></asp:Label>
        <br />
        <uc5:Document ID="Document1" runat="server" DocumentName="Quotation" PreGenerate="false" Visible="true" Text="<%$ Resources:lbl_QuoteDocument %>"></uc5:Document>
        <uc5:Document ID="li_Scheduledocument" runat="server" DocumentName="Schedule" PreGenerate="false" Visible="true" Text="<%$ Resources:lbl_Scheduledocument %>"></uc5:Document>
        <br />
        <br />
        <asp:Label ID="Label1" runat="server" Font-Size="Small" Font-Bold="true">Certificate: </asp:Label>  
        <br />   
        <uc5:Document ID="li_Certificatedocument" runat="server" DocumentName="Standard" PreGenerate="false" Text="Standard"></uc5:Document>
        <uc5:Document ID="li_Certificatedocument1" runat="server" DocumentName="DOC" PreGenerate="false" Text="DOC"></uc5:Document>
        <uc5:Document ID="li_Certificatedocument2" runat="server" DocumentName="Movement" PreGenerate="false" Text="Movement"></uc5:Document>
        <uc5:Document ID="li_Certificatedocument3" runat="server" DocumentName="OBU" PreGenerate="false" Text="OBU"></uc5:Document>
        <uc5:Document ID="li_Certificatedocument4" runat="server" DocumentName="HP" PreGenerate="false" Text="HP"></uc5:Document>
        <uc5:Document ID="li_Certificatedocument5" runat="server" DocumentName="Haulage" PreGenerate="false" Text="Haulage"></uc5:Document>
        <uc5:Document ID="li_Certificatedocument6" runat="server" DocumentName="Taxi Form B" PreGenerate="false" Text="Taxi Form B"></uc5:Document>
        <uc5:Document ID="lbl_debitdocument" runat="server" DocumentName="DEBIT" PreGenerate="false" Visible="true" Text="<%$ Resources:lbl_debitdocument %>"></uc5:Document>
    </div>
</div>
<div style="display: none">
    <asp:TextBox ID="PCATLIN__MTADESC" runat="server" CssClass="text"></asp:TextBox>
    <asp:HiddenField ID="HidTotalCommissionValue" runat="server"></asp:HiddenField>
</div>
