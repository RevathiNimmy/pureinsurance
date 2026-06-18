<%@ control language="VB" autoeventwireup="false" inherits="Nexus.Controls_ReportControls_OustandingClaimsGrossToNet" Codefile="OustandingClaimsGrossToNet.ascx.vb"%>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<%@ Register TagPrefix="uc6" TagName="FindParty" Src="~/Controls/FindParty.ascx" %>


<script language="javascript" type="text/javascript">
   
</script>

<div id="Controls_ReportControls_OustandingClaimsGrossToNet">
    <div class="card">
        <div class="card-body clearfix">
            <div class="form-horizontal">
                <legend>
                    <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:lbl_header %>"></asp:Label></legend>
               
               
                 <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblSTP" runat="server" AssociatedControlID="RP__SalvageAndTPRecovery" Text="<%$ Resources:lbl_STP %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__SalvageAndTPRecovery" runat="server" CssClass="field-medium form-control">
                           <asp:ListItem Text="<%$ Resources:li_STP_Include%>" Value="Include"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_STP_Exclude %>" Value="Exclude"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_STP_Only %>" Value="Only"></asp:ListItem>  
                        </asp:DropDownList>
                    </div>
                                   
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
                    <asp:Label ID="lblCatastrophe" runat="server" AssociatedControlID="RP__Catastrophe" Text="<%$ Resources:lbl_Catastrophe %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__Catastrophe" runat="server" CssClass="field-medium form-control">
                           <asp:ListItem Text="<%$ Resources:li_Catastrophe_Include%>" Value="Include"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_Catastrophe_Exclude %>" Value="Exclude"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                                   
                </div>	
               
                 <div id="liBranch" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblBatch" runat="server" AssociatedControlID="RP__BRANCH_ID" Text="<%$ Resources:lbl_Batch %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__BRANCH_ID" runat="server" CssClass="field-medium form-control">
                        </asp:DropDownList>
						
                    </div>
                </div>
                
                
                 <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblTypeOfCurrency" runat="server" AssociatedControlID="RP__TypeOfCurrency" Text="<%$ Resources:lbl_TypeOfCurrency %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__TypeOfCurrency" runat="server" CssClass="field-medium form-control">
                            <asp:ListItem Text="<%$ Resources:li_TypeOfCurrency_System %>" Value="System"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_TypeOfCurrency_Account %>" Value="Account"></asp:ListItem>  
                            <asp:ListItem Text="<%$ Resources:li_TypeOfCurrency_Base %>" Value="Base"></asp:ListItem>  
                            <asp:ListItem Text="<%$ Resources:li_TypeOfCurrency_Transaction %>" Value="Transaction"></asp:ListItem>  
                        </asp:DropDownList>
                    </div>
                    </div>
                    
           			 <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblGroupByCode" runat="server" AssociatedControlID="RP__GROUPBYCODE" Text="<%$ Resources:lbl_GroupByCode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__GROUPBYCODE" runat="server" CssClass="field-medium form-control">
                            <asp:ListItem Text="<%$ Resources:li_DetailSummary_Summary %>" Value="No Grouping"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_DetailSummary_Detail %>" Value="Branch"></asp:ListItem>  
                        </asp:DropDownList>
                    </div>
                </div>
				
				  <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblGroupOn" runat="server" AssociatedControlID="RP__GROUPON" Text="<%$ Resources:lbl_GroupOn %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__GROUPON" runat="server" CssClass="field-medium form-control">
                           <asp:ListItem Text="<%$ Resources:li_GroupByAgent_Client%>" Value="Client"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_GroupByAgent_Policy %>" Value="Policy"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_GroupByAgent_Agent %>" Value="Agent"></asp:ListItem>  
                        </asp:DropDownList>
                    </div>
                
                   </div>        
                   
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12" style="display: none;">
                    <asp:Label ID="lblBranch" runat="server" AssociatedControlID="RP__BRANCH" Text="<%$ Resources:lbl_Branch %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:TextBox ID="RP__BRANCH" runat="server" CssClass="field-medium form-control">                           
                        </asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="card-footer">
            <asp:LinkButton ID="btnGenerateReport" runat="server" Text="<%$ Resources:btnGenerateReport %>" OnClick="GenerateReport" ValidationGroup="vldReportsControlsGroup" SkinID="btnPrimary"></asp:LinkButton>
        </div>
    </div>
</div>
