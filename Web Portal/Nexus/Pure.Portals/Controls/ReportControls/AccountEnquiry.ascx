<%@ control language="VB" autoeventwireup="false" inherits="Nexus.Controls_ReportControls_AccountEnquiry" Codefile="AccountEnquiry.ascx.vb" %>


<script language="javascript" type="text/javascript">
  
        $(document).ready(function () {

    });
     function setAccount(sClientName, sClientKey) {
       tb_remove();
      
           document.getElementById('<%= RP__short_code.ClientId%>').value = unescape(sClientName);
           document.getElementById('<%= txtClientKey.ClientId%>').value = sClientKey;
           document.getElementById('<%= RP__short_code.ClientId%>').focus();
       }
    
</script>

<div id="Controls_ReportControls_AccountEnquiry">
    <div class="card">
        <div class="card-body clearfix">
            <div class="form-horizontal">
                <legend>
                    <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:lbl_header %>"></asp:Label></legend>					
              
              <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                  <asp:Label runat="Server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="RP__short_code" Text="<%$ Resources:btn_Client %>" ID="lblbtnClient"></asp:Label>
                  <div class="col-md-8 col-sm-9">
                        <div class="input-group">
                            <asp:TextBox ID="RP__short_code" runat="server" CssClass="form-control" Text="ALL" onBlur="validate(this)"></asp:TextBox><span class="input-group-btn">
                                <asp:LinkButton ID="btnClient" runat="server" OnClientClick="tb_show(null , '../Modal/FindAccount.aspx?modal=true&RequestPage=BG&KeepThis=true&FromPage=PC&TB_iframe=true&height=500&width=750' , null);return false;" SkinID="btnModal">
                                <i class="fa fa-search"></i>
                                 <span class="btn-fnd-txt">Short Code</span>
                                </asp:LinkButton></span>
                        </div>
                    </div>
					<asp:HiddenField ID="txtClientKey" runat="server"></asp:HiddenField>
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
               
            </div>
        </div>
        <div class="card-footer">
            <asp:LinkButton ID="btnGenerateReport" runat="server" Text="<%$ Resources:btnGenerateReport %>" OnClick="GenerateReport" ValidationGroup="vldReportsControlsGroup" SkinID="btnPrimary"></asp:LinkButton>
        </div>
    </div>
</div>
