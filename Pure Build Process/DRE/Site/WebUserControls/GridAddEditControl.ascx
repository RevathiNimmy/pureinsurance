<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    Inherits="WebUserControls_GridAddEditControl" 
    className="GridAddEditControl" 
 Codebehind="GridAddEditControl.ascx.cs" %>

<div class="question">
    <asp:Label ID="lblName" AssociatedControlID="txtName" runat="server" Text="Name:"></asp:Label>
    <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
    <ajax:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" TargetControlID="txtName" runat="server" WatermarkText="Type Name Of Grid Here" WatermarkCssClass="watermark" />
    <asp:CustomValidator ID="cvUniqueGridname" runat="server" 
        ToolTip="Grid name must be unique" Text="*" 
        onservervalidate="cvUniqueGridname_ServerValidate" ValidationGroup="RuleSetValidation" />
</div>

<div class="question">
    <asp:Label ID="lblEffectiveDate" AssociatedControlID="tbEffectiveDate" runat="server" Text="Effective Date:"></asp:Label>
    <asp:TextBox ID="tbEffectiveDate" runat="server"></asp:TextBox>
    <ajax:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" TargetControlID="tbEffectiveDate" runat="server" WatermarkText="Enter Effective Date Here Or Select Calendar" WatermarkCssClass="watermark" />
    <asp:Image ID="imgEffectiveDate" runat="server" ImageUrl="~/App_Themes/Images/Calendar_scheduleHS.png" />
    <asp:RequiredFieldValidator ID="rfvEffectiveDate" runat="server" ControlToValidate="tbEffectiveDate" ValidationGroup="GridValidation" Display="Static" Text="*" ToolTip="Effective date is mandatory"  />
    <%--<asp:CustomValidator ID="cvEffectiveDate" runat="server" ControlToValidate="tbEffectiveDate" ValidationGroup="GridValidation" Display="Static" Text="*" onservervalidate="cvEffectiveDate_ServerValidate" />--%>
    
    <script type="text/javascript" language="javascript">
           function cvEffectiveDate_ClientValidate(source, args) {
               var effectiveDate = new Date(document.getElementById("<%= tbEffectiveDate.ClientID %>").getAttribute("value"));

               if (isNaN(effectiveDate)) {
                   args.IsValid = false;
                   return;
               }

               var hoursDDL = document.getElementById("<%= ddlEffectiveTimeHours.ClientID %>");
               var minutesDDL = document.getElementById("<%= ddlEffectiveTimeMinutes.ClientID %>");

               var hours = hoursDDL.options[hoursDDL.selectedIndex].text;
              
               var minutes = minutesDDL.options[minutesDDL.selectedIndex].text;

               effectiveDate.setHours(hours, minutes, 0, 0);

               if (effectiveDate < new Date()) {
                   args.IsValid = false;
               }
               else {
                   args.IsValid = true;
               }
           }
        </script>
            
        <asp:CustomValidator 
            id="cvEffectiveDate" 
            runat="server" 
            ClientValidationFunction="cvEffectiveDate_ClientValidate"
            OnServerValidate="cvEffectiveDate_ServerValidate"
            Display="Dynamic"
            Text="*"
            ToolTip="Must be after today’s date and time" />
    
    <ajax:CalendarExtender ID="ceEffectiveDate" runat="server" TargetControlID="tbEffectiveDate" PopupButtonID="imgEffectiveDate" Enabled="true" />
</div>
<div class="question">
        <asp:Label ID="tbEffectiveTime" runat="Server" Text="Effective Time:" AssociatedControlID="ddlEffectiveTimeHours" />
        <asp:DropDownList ID="ddlEffectiveTimeHours" runat="server" />
        <asp:Label ID="Label4" runat="Server" Text=":&nbsp;" Width="5px" AssociatedControlID="ddlEffectiveTimeMinutes" />
        <asp:DropDownList ID="ddlEffectiveTimeMinutes" runat="server"  />
        <br /><br />
    </div>
<div class="question">
    <asp:Label ID="lblFacets" AssociatedControlID="ddFacets" runat="server" Text="Dimension:"></asp:Label>
    <asp:DropDownList ID="ddFacets" runat="server"></asp:DropDownList>
</div>

<div class="question">
    <asp:Label ID="lblInputContents" AssociatedControlID="fuInputContents" runat="server" Text="Input Contents:"></asp:Label>
    <asp:FileUpload ID="fuInputContents" runat="server" />  
    <asp:CustomValidator ID="cvGridContents" runat="server" Text="*" />
</div>

<div class="buttons">
    <asp:HyperLink ID="btnViewGrid" Text="View Grid" runat="server"   />
</div>

<asp:HiddenField ID="hfRuleGridKey" runat="server" />
<asp:HiddenField ID="hfProfileToken" runat="server" />
<asp:HiddenField ID="hfProfileStatus" runat="server" />
<asp:HiddenField ID="hfProfileKey" runat="server" />
<asp:HiddenField ID="hfRuleSetKey" runat="server" />
<asp:HiddenField ID="hfRuleSetStatus" runat="server" />
<asp:HiddenField ID="hfDREItem" runat="server" /> 

<div style="clear:both"></div>