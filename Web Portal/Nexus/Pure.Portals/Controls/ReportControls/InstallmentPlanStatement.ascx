<%@ control language="VB" autoeventwireup="false" inherits="Nexus.Controls_ReportControls_InstalmentPlanStatement" Codefile="InstallmentPlanStatement.ascx.vb" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<%@ Register TagPrefix="uc6" TagName="FindParty" Src="~/Controls/FindParty.ascx" %>


<script language="javascript" type="text/javascript">
  
     function setAgent(sName, sKey, sCode, sAgentType) {
        tb_remove();
        document.getElementById('<%= RP__AgentShortName.ClientId%>').value = sCode;
        document.getElementById('<%= txtAgentKey.ClientId%>').value = sAgentType;
        document.getElementById('<%= RP__AgentShortName.ClientId%>').focus
    }


    }
    
</script>

<div id="Controls_ReportControls_InstalmentPlanStatement">
    <div class="card">
        <div class="card-body clearfix">
            <div class="form-horizontal">
                <legend>
                    <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:lbl_header %>"></asp:Label></legend>					
					
					<div id="liBranch" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblBatch" runat="server" AssociatedControlID="RP__BRANCH_ID" Text="<%$ Resources:lbl_Batch %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__BRANCH_ID" runat="server" CssClass="field-medium form-control">
                        </asp:DropDownList>
						
                    </div>
                </div>
				
               <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblAgentCode" AssociatedControlID="RP__AgentShortName" runat="server" Text="<%$ Resources:lbl_Agent %>" class="col-md-4 col-sm-3 control-label">
                    </asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <div class="input-group">
                            <asp:TextBox ID="RP__AgentShortName" runat="server" CssClass="form-control" Text="ALL"></asp:TextBox>
                            <span class="input-group-btn">
                                <asp:LinkButton ID="btnAgentCode" runat="server" SkinID="btnModal" OnClientClick="tb_show(null , '../Modal/FindAgent.aspx?AgentType=Broker&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;">
                                    <i class="fa fa-search"></i>
                                     <span class="btn-fnd-txt">Agent</span>
                                </asp:LinkButton>
                            </span>
                        </div>
                    </div>
                    <asp:HiddenField ID="txtAgentKey" runat="server"></asp:HiddenField>
                    <asp:RequiredFieldValidator ID="rqdClient" runat="server" ControlToValidate="RP__AgentShortName" Display="None" ErrorMessage="<%$ Resources:lbl_req_Agent %>" SetFocusOnError="true" ValidationGroup="vldReportsControlsGroup"></asp:RequiredFieldValidator>
                </div>
				
				<div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblPeriodStartDate" runat="server" AssociatedControlID="RP__START_DATE" Text="<%$ Resources:lbl_PeriodStartDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <div class="input-group">
                            <asp:TextBox ID="RP__START_DATE" runat="server" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="calPeriodStartDate" runat="server" LinkedControl="RP__START_DATE" HLevel="1"></uc1:CalendarLookup>
                        </div>
                    </div>

                    <asp:RequiredFieldValidator ID="reqdvldPeriodStartDate" Display="None" ControlToValidate="RP__START_DATE" runat="server" ErrorMessage="<%$ Resources:lbl_req_PeriodStartDate %>" SetFocusOnError="True" ValidationGroup="vldReportsControlsGroup"> </asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="comvldPeriodStartDate" runat="server" Display="None" ControlToValidate="RP__START_DATE" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_invalid_PeriodStartDate %>" Operator="DataTypeCheck" Type="Date" ValidationGroup="vldReportsControlsGroup"></asp:CompareValidator>
                    <asp:RangeValidator ID="rngvldPeriodStartDate" runat="server" ErrorMessage="<%$ Resources:lbl_invalidrange_PeriodStartDate %>" ControlToValidate="RP__START_DATE" Display="None" ValidationGroup="vldReportsControlsGroup">
                    </asp:RangeValidator>
                </div>
				<div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblPeriodEndDate" runat="server" AssociatedControlID="RP__END_DATE" Text="<%$ Resources:lbl_PeriodEndDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <div class="input-group">
                            <asp:TextBox ID="RP__END_DATE" runat="server" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="calPeriodEndDate" runat="server" LinkedControl="RP__END_DATE" HLevel="1"></uc1:CalendarLookup>
                        </div>
                    </div>

                    <asp:RequiredFieldValidator ID="reqdvldPeriodEndDate" Display="None" ControlToValidate="RP__END_DATE" runat="server" ErrorMessage="<%$ Resources:lbl_req_PeriodEndDate %>" SetFocusOnError="True" ValidationGroup="vldReportsControlsGroup"> </asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="comvldPeriodEndDate" runat="server" Display="None" ControlToValidate="RP__END_DATE" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_invalid_PeriodEndDate %>" Operator="DataTypeCheck" Type="Date" ValidationGroup="vldReportsControlsGroup"></asp:CompareValidator>
                    <asp:RangeValidator ID="rngvldPeriodEndDate" runat="server" ErrorMessage="<%$ Resources:lbl_invalidrange_PeriodEndDate %>" ControlToValidate="RP__END_DATE" Display="None" ValidationGroup="vldReportsControlsGroup">
                    </asp:RangeValidator>
                </div>
				
				<div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblDateBasis" runat="server" AssociatedControlID="RP__Basis" Text="<%$ Resources:lbl_DateBasis %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__Basis" runat="server" CssClass="field-medium form-control form-select">
                            <asp:ListItem Text="<%$ Resources:li_DateBasis_TransactionDate %>" Value="Transaction Date"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_DateBasis_EffectiveDate %>" Value="Effective Date"></asp:ListItem>                            
                        </asp:DropDownList>
                    </div>
                </div>
				
				<div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblUnderwritingYear" runat="server" AssociatedControlID="RP__Underwriting_Year" Text="<%$ Resources:lbl_UnderwritingYear %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:TextBox ID="RP__Underwriting_Year" runat="server" CssClass="field-medium form-control">                           
                        </asp:TextBox>
                    </div>
                </div>			
				               
			
				<div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblTypeOfCurrency" runat="server" AssociatedControlID="RP__TypeOfCurrency" Text="<%$ Resources:lbl_TypeOfCurrency %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__TypeOfCurrency" runat="server" CssClass="field-medium form-control">
						   <asp:ListItem Text="<%$ Resources:li_TypeOfCurrency_Account %>" Value="Account"></asp:ListItem> 
                            <asp:ListItem Text="<%$ Resources:li_TypeOfCurrency_System %>" Value="System"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_TypeOfCurrency_Base %>" Value="Base"></asp:ListItem>  
							<asp:ListItem Text="<%$ Resources:li_TypeOfCurrency_Transaction %>" Value="Transaction"></asp:ListItem> 
                        </asp:DropDownList>
                    </div>
                </div>
				 <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblGroupByCode" runat="server" AssociatedControlID="RP__GroupByCode" Text="<%$ Resources:lbl_GroupByCode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__GroupByCode" runat="server" CssClass="field-medium form-control">
                            <asp:ListItem Text="<%$ Resources:li_DetailSummary_Summary %>" Value="No Grouping"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_DetailSummary_Detail %>" Value="Branch"></asp:ListItem>  
                        </asp:DropDownList>
                    </div>
                </div>
				
				<div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblIncludeBalAcc" runat="server" AssociatedControlID="RP__IncludeBalanceAccount" Text="<%$ Resources:lbl_IncludeBalAcc %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__IncludeBalanceAccount" runat="server" CssClass="field-medium form-control">
                            <asp:ListItem Text="<%$ Resources:li_Detail_Yes %>" Value="Yes"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_Detail_No %>" Value="No"></asp:ListItem>  
                        </asp:DropDownList>
                    </div>
                </div>
				
				<div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblTransactionType" runat="server" AssociatedControlID="RP__TRANSACTIONTYPE" Text="<%$ Resources:lbl_TransactionType %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__TransactionType" runat="server" CssClass="field-medium form-control">
                            <asp:ListItem Text="<%$ Resources:li_TransactionType_PCT %>" Value="Premium & Claim Transactions"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_TransactionType_PTO %>" Value="Premium Transactions Only"></asp:ListItem>  
							<asp:ListItem Text="<%$ Resources:li_TransactionType_CTO %>" Value="Claim Transaction Only"></asp:ListItem>  
                        </asp:DropDownList>
                    </div>
                </div>
				
				<div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblProduct" runat="server" AssociatedControlID="RP__ProductCode" Text="<%$ Resources:lbl_Product %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__ProductCode" runat="server" CssClass="field-medium form-control">
							<asp:ListItem Text="ALL" Value="ALL" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_Product_HEALTH %>" Value="Health"></asp:ListItem>  
							<asp:ListItem Text="<%$ Resources:li_Product_MOTOR %>" Value="Motor"></asp:ListItem>  
							 <asp:ListItem Text="<%$ Resources:li_Product_PCAP %>" Value="Pure Commercial Annual Product"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_Product_PCMP %>" Value="Pure Commercial Monthly Product"></asp:ListItem>  
							<asp:ListItem Text="<%$ Resources:li_Product_TMPP %>" Value="TMPProduct"></asp:ListItem> 
                        </asp:DropDownList>
                    </div>
                </div>  
				
              <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblAgeDisplayUnAllocatedCash" runat="server" AssociatedControlID="RP__AgeAndUnalloc" Text="<%$ Resources:lbl_AgeDisplayUnAllocatedCash %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__AgeAndUnalloc" runat="server" CssClass="field-medium form-control form-select">
                            <asp:ListItem Text="<%$ Resources:li_AgeDisplayUnAllocatedCash_Yes %>" Value="Yes"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_AgeDisplayUnAllocatedCash_No %>" Value="No"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                         
              
               
				
				
				
               
            </div>
        </div>
        <div class="card-footer">
            <asp:LinkButton ID="btnGenerateReport" runat="server" Text="<%$ Resources:btnGenerateReport %>" OnClick="GenerateReport" ValidationGroup="vldReportsControlsGroup" SkinID="btnPrimary"></asp:LinkButton>
        </div>
    </div>
</div>
