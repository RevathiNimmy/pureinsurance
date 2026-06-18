<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="BankGuaranteeDetails.aspx.vb" Inherits="Nexus.secure_BankGuaranteeDetails"
    Title="Bank Guarantee Details" %>

<asp:Content ID="Content4" ContentPlaceHolderID="cntMainBody" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <script language="javascript" type="text/javascript">
      
      function pageLoad()
    {   
    	//this is needed if the trigger is external to the update panel   
       var manager = Sys.WebForms.PageRequestManager.getInstance();
       manager.add_beginRequest(OnBeginRequest);
    }

    function OnBeginRequest(sender, args)
    {
          var postBackElement = args.get_postBackElement();
          if (postBackElement.id == 'chkAmtSelect')
          { 
     		    $get(uprogQuotes).style.display = "block";  
          }
    }

    </script>

    <div id="secure_payment_BankGuaranteeDetails">
        
    
            
        
                
                
                <asp:CustomValidator ID="CustAmountValidation" runat="server" ErrorMessage="<%$ Resources:CustSelectionValidation %>" Display="None"></asp:CustomValidator>
                <asp:ValidationSummary ID="ValidationSummary" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
                <div class="submit">
                    <asp:Button ID="btnSubmit" Text="Submit" runat="server" OnClientClick=""></asp:Button>
                </div>
            
                    
                    <div class="card">
                        <div class="card-body clearfix">
                            
                            <div class="form-horizontal">
                                <asp:RadioButtonList ID="radioUserType" runat="server" RepeatDirection="Horizontal" AutoPostBack="true">
                                    <asp:ListItem Text="Agent" Enabled="false"></asp:ListItem>
                                    <asp:ListItem Text="Client" Enabled="false"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            <div class="form-horizontal">
                                <ol>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblPartyCode" runat="server" AssociatedControlID="txtPartyCode" Text="<%$ Resources:lblPartyCode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9"><asp:TextBox ID="txtPartyCode" TabIndex="1" CssClass="field-medium form-control" runat="server" Enabled="false"></asp:TextBox></div>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblPartyName" runat="server" AssociatedControlID="txtPartyName" Text="<%$ Resources:lblPartyName %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9"><asp:TextBox ID="txtPartyName" TabIndex="2" CssClass="field-medium form-control" runat="server" Enabled="false"></asp:TextBox></div>
                                    </div>
                                </ol>
                            </div>
                            <asp:UpdatePanel ID="updateAmount" runat="server">
                                <ContentTemplate>
                                    <asp:Label ID="lblAmount" runat="server" AssociatedControlID="txtAmount">
                                        <asp:Literal ID="ltAmount" runat="server" Text="<%$ Resources:ltAmount %>"></asp:Literal></asp:Label>
                                    <asp:TextBox ID="txtAmount" runat="server" Enabled="false" CssClass="field-medium"></asp:TextBox>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <nexus:ProgressIndicator ID="upAmount" OverlayCssClass="updating" AssociatedUpdatePanelID="updateAmount" runat="server">
                                <ProgressTemplate>
                                </ProgressTemplate>
                            </nexus:ProgressIndicator>
                            <asp:GridView ID="grdvBGDebtDetails" runat="server" AlternatingRowStyle-BorderStyle="none" AutoGenerateColumns="False" DataKeyNames="BGKey" GridLines="None" CellPadding="0" CellSpacing="0" PageSize="10" AllowPaging="True" PagerSettings-Mode="Numeric" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                                <Columns>
                                    <asp:BoundField HeaderText="<%$ Resources:BankNameDescription %>" DataField="BankNameDescription"></asp:BoundField>
                                    <asp:BoundField HeaderText="<%$ Resources:Key %>" DataField="Key"></asp:BoundField>
                                    <asp:BoundField HeaderText="<%$ Resources:Key %>" DataField="BankGuaranteeReference"></asp:BoundField>
                                    <nexus:BoundField HeaderText="<%$ Resources:AvailableBalance %>" DataField="AvailableBalance" DataType="Currency"></nexus:BoundField>
                                    <asp:BoundField HeaderText="<%$ Resources:ExpiryDate %>" DataField="ExpiryDate"></asp:BoundField>
                                    <asp:BoundField HeaderText="<%$ Resources:DueDate %>" DataField="DueDate"></asp:BoundField>
                                    <asp:TemplateField HeaderText="SelectAll">
                                        <ItemTemplate>
                                            <asp:UpdatePanel ID="updateCheckAmount" runat="server" ChildrenAsTriggers="False" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:CheckBox ID="chkAmtSelect" runat="server" value='<%# Eval("Key") %>' AutoPostBack="True" OnCheckedChanged="chkAmtSelect_CheckedChanged" Text=" "></asp:CheckBox>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="chkAmtSelect" EventName="CheckedChanged"></asp:AsyncPostBackTrigger>
                                                </Triggers>
                                            </asp:UpdatePanel>
                                            <nexus:ProgressIndicator ID="upChkPayment" OverlayCssClass="updating" AssociatedUpdatePanelID="updateCheckAmount" runat="server">
                                                <ProgressTemplate>
                                                </ProgressTemplate>
                                            </nexus:ProgressIndicator>
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                        </HeaderTemplate>
                                        <ItemStyle CssClass="span-2"></ItemStyle>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        
                    </div>
                </div>
</asp:Content>
