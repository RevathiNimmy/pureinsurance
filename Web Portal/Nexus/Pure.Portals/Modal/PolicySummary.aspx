<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="PolicySummary.aspx.vb" Inherits="Nexus.Modal_PolicySummary" Title="Policy Summary" %>

<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ScriptIncludes" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cntProgressBar" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cntMainBody" runat="Server">
    <div id="Modal_PolicySummary">
        <div class="card">
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblHeading" runat="server" Text="<%$ Resources:lbl_PolicySummary_Header %>"></asp:Label>
                    </legend>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblInsuredName" runat="server" AssociatedControlID="POLICYHEADER__INSUREDNAME" Text="<%$ Resources:lbl_InsuredName %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <p class="form-control-static font-bold">
                                <asp:Label ID="POLICYHEADER__INSUREDNAME" runat="server" CssClass="field-medium" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            </p>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lbAlternateRef" runat="server" AssociatedControlID="POLICYHEADER__ALTERNATEREF" Text="<%$ Resources:lbl_AlternateReferance %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <p class="form-control-static font-bold">
                                <asp:Label ID="POLICYHEADER__ALTERNATEREF" runat="server" CssClass="field-medium" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            </p>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12" id="liPolicyAssociate">
                        <asp:Label ID="lblSecondaryAssociated" runat="server" AssociatedControlID="lbPolicyAssociates"
                            Text="<%$ Resources:lbl_SecondaryAssociated %>" class="col-md-4 col-sm-3 control-label" />
                        <div class="col-md-8 col-sm-9">
                            <asp:ListBox runat="server" ID="lbPolicyAssociates" Rows="4" Height="100px" CssClass="form-control" />
                            <asp:Repeater ID="rptrPolicyAssociate" runat="server" Visible="false">
                                <HeaderTemplate>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPolicyAssociate" runat="server" Text='<%# eval("PartyName") %>'
                                        CausesValidation="false" />
                                </ItemTemplate>
                                <SeparatorTemplate>
                                    </br>
                                </SeparatorTemplate>
                                <FooterTemplate>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPolicyNumber" runat="server" AssociatedControlID="POLICYHEADER__POLICYNUMBER" Text="<%$ Resources:lbl_PolicyNo %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <p class="form-control-static font-bold">
                                <asp:Label ID="POLICYHEADER__POLICYNUMBER" runat="server" CssClass="field-medium" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            </p>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblSelectProduct" runat="server" AssociatedControlID="POLICYHEADER__PRODUCT" Text="<%$ Resources:lbl_SelectProduct %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <p class="form-control-static font-bold">
                                <asp:Label ID="POLICYHEADER__PRODUCT" runat="server" CssClass="field-medium" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            </p>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblBranchCode" runat="server" AssociatedControlID="POLICYHEADER__BRANCH" Text="<%$ Resources:lbl_Branchcode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <p class="form-control-static font-bold">
                                <asp:Label ID="POLICYHEADER__BRANCH" runat="server" CssClass="field-medium" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            </p>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblAnalysisCode" runat="server" AssociatedControlID="POLICYHEADER__ANALYSISCODE" Text="<%$ Resources:lbl_AnalysisCode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <p class="form-control-static font-bold">
                                <asp:Label ID="POLICYHEADER__ANALYSISCODE" runat="server" CssClass="field-medium" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            </p>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblSubBranchCode" runat="server" AssociatedControlID="POLICYHEADER__SUBBRANCH" Text="<%$ Resources:lbl_SubBranchCode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <p class="form-control-static font-bold">
                                <asp:Label ID="POLICYHEADER__SUBBRANCH" runat="server" CssClass="field-medium" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            </p>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblBusinessSource" runat="server" AssociatedControlID="POLICYHEADER__BUSINESSTYPE" Text="<%$ Resources:lbl_BusinessSource %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <p class="form-control-static font-bold">
                                <asp:Label ID="POLICYHEADER__BUSINESSTYPE" runat="server" CssClass="field-medium" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            </p>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label runat="server" ID="lblAgent" Text="<%$ Resources:lbl_AgentCode %>" AssociatedControlID="POLICYHEADER__AGENTCODE" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <p class="form-control-static font-bold">
                                <asp:Label ID="POLICYHEADER__AGENTCODE" runat="server" CssClass="field-medium" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            </p>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label runat="server" ID="lblHandler" AssociatedControlID="POLICYHEADER__HANDLER" Text="<%$ Resources:lbl_AccountHandler %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <p class="form-control-static font-bold">
                                <asp:Label ID="POLICYHEADER__HANDLER" runat="server" CssClass="field-medium" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            </p>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblCurrency" runat="server" AssociatedControlID="POLICYHEADER__CURRENCY" Text="<%$ Resources:lbl_Currency %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <p class="form-control-static font-bold">
                                <asp:Label ID="POLICYHEADER__CURRENCY" runat="server" CssClass="field-medium" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            </p>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label runat="server" ID="lblRegarding" AssociatedControlID="POLICYHEADER__REGARDING" Text="<%$ Resources:lbl_Regarding %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <p class="form-control-static font-bold">
                                <asp:Label ID="POLICYHEADER__REGARDING" runat="server" CssClass="field-medium" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            </p>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label runat="server" ID="lblQuote" AssociatedControlID="POLICYHEADER__QUOTE" Text="<%$ Resources:lbl_Quote %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <p class="form-control-static font-bold">
                                <asp:CheckBox ID="POLICYHEADER__QUOTE" runat="server" Enabled="false" Text=" " CssClass="asp-check"></asp:CheckBox>
                            </p>
                        </div>
                    </div>
                </div>
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblCoverDateHeading" runat="server" Text="<%$ Resources:lbl_CoverDateHeading%>"></asp:Label>
                    </legend>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblCoverStartDate" runat="server" AssociatedControlID="POLICYHEADER__COVERSTARTDATE" Text="<%$ Resources:lbl_CoverStartDate%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <p class="form-control-static font-bold">
                                <asp:Label ID="POLICYHEADER__COVERSTARTDATE" runat="server" CssClass="field-date" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            </p>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lbl_CoverEndDate" runat="server" AssociatedControlID="POLICYHEADER__COVERENDDATE" Text="<%$ Resources:lbl_CoverEndDate%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <p class="form-control-static font-bold">
                                <asp:Label ID="POLICYHEADER__COVERENDDATE" runat="server" CssClass="field-date" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            </p>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblInception" runat="server" AssociatedControlID="POLICYHEADER__INCEPTION" Text="<%$ Resources:lbl_InceptionDate%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <p class="form-control-static font-bold">
                                <asp:Label ID="POLICYHEADER__INCEPTION" runat="server" CssClass="field-date" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            </p>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblRenewal" runat="server" AssociatedControlID="POLICYHEADER__RENEWAL" Text="<%$ Resources:lbl_RenewalDate%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <p class="form-control-static font-bold">
                                <asp:Label ID="POLICYHEADER__RENEWAL" runat="server" CssClass="field-date" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            </p>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblInceptionTPI" runat="server" AssociatedControlID="POLICYHEADER__INCEPTIONTPI" Text="<%$ Resources:lbl_InceptionTPIDate%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <p class="form-control-static font-bold">
                                <asp:Label ID="POLICYHEADER__INCEPTIONTPI" runat="server" CssClass="field-date" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            </p>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblProposalDate" runat="server" AssociatedControlID="POLICYHEADER__PROPOSALDATE" Text="<%$ Resources:lbl_ProposalDate%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <p class="form-control-static font-bold">
                                <asp:Label ID="POLICYHEADER__PROPOSALDATE" runat="server" CssClass="field-date" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            </p>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblHCExpiry" runat="server" AssociatedControlID="POLICYHEADER__HCEXPIRY" Text="<%$ Resources:lbl_HCExpiryDate%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <p class="form-control-static font-bold">
                                <asp:Label ID="POLICYHEADER__HCEXPIRY" runat="server" CssClass="field-date" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            </p>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblQuoteExpiryDate" runat="server" AssociatedControlID="POLICYHEADER__QUOTEEXPIRYDATE" Text="<%$ Resources:lbl_QuoteExpiryDate%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <p class="form-control-static font-bold">
                                <asp:Label ID="POLICYHEADER__QUOTEEXPIRYDATE" runat="server" CssClass="field-date" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            </p>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPolicyDeductible" runat="server" AssociatedControlID="POLICYHEADER__DEDUCTIBLES" Text="<%$ Resources:lbl_PolicyDeductible %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <p class="form-control-static font-bold">
                                <asp:Label ID="POLICYHEADER__DEDUCTIBLES" runat="server" CssClass="field-medium" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            </p>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblUnderwritingYear" runat="server" AssociatedControlID="POLICYHEADER__UNDERWRITINGYEAR" Text="<%$ Resources:lbl_Underwritingyear %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <p class="form-control-static font-bold">
                                <asp:Label ID="POLICYHEADER__UNDERWRITINGYEAR" runat="server" CssClass="field-medium" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            </p>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPolicyLimits" runat="server" AssociatedControlID="POLICYHEADER__POLICYLIMITS" Text="<%$ Resources:lbl_PolicyLimits %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <p class="form-control-static font-bold">
                                <asp:Label ID="POLICYHEADER__POLICYLIMITS" runat="server" CssClass="field-medium" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            </p>
                        </div>
                    </div>
                </div>
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblPremiumHeading" runat="server" Text="<%$ Resources:lbl_PremiumHeading%>"></asp:Label>
                    </legend>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblNetTotal" AssociatedControlID="txtNetTotal" runat="server" Text="<%$ Resources:lbl_NetTotal %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <p class="form-control-static font-bold">
                                <asp:Label ID="txtNetTotal" CssClass="field-medium" runat="server" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            </p>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblFeeTotal" AssociatedControlID="txtFeeTotal" runat="server" Text="<%$ Resources:lbl_FeeTotal %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <p class="form-control-static font-bold">
                                <asp:Label ID="txtFeeTotal" CssClass="field-medium" runat="server" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            </p>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblTaxTotal" AssociatedControlID="txtTaxTotal" runat="server" Text="<%$ Resources:lbl_TaxTotal %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <p class="form-control-static font-bold">
                                <asp:Label ID="txtTaxTotal" CssClass="field-medium" runat="server" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            </p>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblGrossTotal" AssociatedControlID="txtGrossTotal" runat="server" Text="<%$ Resources:lbl_GrossTotal%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <p class="form-control-static font-bold">
                                <asp:Label ID="txtGrossTotal" CssClass="field-medium" runat="server" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            </p>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnOk" runat="server" Text="<%$ Resources:btn_Ok %>" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
    </div>
</asp:Content>
