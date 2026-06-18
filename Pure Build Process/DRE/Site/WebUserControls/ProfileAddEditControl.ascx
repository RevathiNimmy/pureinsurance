<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    className="ProfileAddEditControl" 
    Inherits="WebUserControls_ProfileAddEditControl" 
    Codebehind="ProfileAddEditControl.ascx.cs" 
%>
<%@ Import Namespace="RulesEngine.Website"%>
<%@ Register 
    src="~/WebUserControls/XsdUpload.ascx" 
    tagname="XsdUpload" 
    tagprefix="uc1" 
%>
<asp:Panel ID="pnlProfileAddEditControl" runat="server" Width="100%">
    <asp:HiddenField ID="hfProfileKey" runat="server" />
    <asp:HiddenField ID="hfVersion" Value="0" runat="server" />
    
    <div class="question">
        <asp:Label AssociatedControlID="ddlSystem" runat="Server" ID="lblSystemName" Text="System Name:" />
        <asp:DropDownList ID="ddlSystem" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlSystem_SelectedIndexChanged" />
    </div>                    
    <div class="question">
        <asp:Label AssociatedControlID="ddlCustomer" runat="Server" ID="lblCustomerName" Text="Customer Name:" />
        <asp:DropDownList ID="ddlCustomer" runat="server" />
    </div>                            
    <div class="question">
        <asp:Label AssociatedControlID="tbToken" runat="Server" ID="lblToken" Text="Token:" />     
        <asp:TextBox ID="tbToken" runat="server" MaxLength="255" Width="200px" ReadOnly="true" />            
    </div>                    
    <div class="question">
        <asp:Label AssociatedControlID="tbName" runat="Server" ID="lblProfileName" Text="Profile Name:" />
        <asp:TextBox ID="tbName" runat="server" MaxLength="255" Width="400px" />
        <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="tbName"
            ValidationGroup="ProfileValidation" Display="Static" Text="*" ToolTip="Profile name is mandatory"  />
        <ajax:TextBoxWatermarkExtender ID="tbweName" runat="server"
            TargetControlID="tbName"
            WatermarkText="Type Profile Name Here"
            WatermarkCssClass="watermark" />                    
    </div>
    <div class="question">
        <asp:Label AssociatedControlID="tbDescription" runat="Server" ID="lblDescription" Text="Profile Description:" />
        <asp:TextBox ID="tbDescription" runat="server" MaxLength="2000" TextMode="MultiLine" Width="400px" Height="150px" />
        <ajax:TextBoxWatermarkExtender ID="tbwmDescription" runat="server"
            TargetControlID="tbDescription"
            WatermarkText="Type Profile Description Here"
            WatermarkCssClass="watermark" />                    
    </div>
    
    
    
    <div class="question">
        <asp:Label AssociatedControlID="tbEffectiveDate" runat="Server" ID="lblEffectiveDate" Text="Effective Date:" />
        <asp:TextBox ID="tbEffectiveDate" runat="server" Width="185px" />
        <asp:Image ID="imgEffectiveDate" runat="server" ImageUrl="~/App_Themes/Images/Calendar_scheduleHS.png" />
        <asp:RequiredFieldValidator ID="rfvEffectiveDate" runat="server" ControlToValidate="tbEffectiveDate"
            ValidationGroup="ProfileValidation" Display="Static" Text="*" ToolTip="Effective date is mandatory"  />
            
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
            
        <%--<asp:CompareValidator ID="cvEffectiveDate" runat="server" ControlToValidate="tbEffectiveDate" ValueToCompare="" Operator="GreaterThanEqual" Type="Date"
            ValidationGroup="ProfileValidation" Display="Static" Text="*" ToolTip="Effective date must be on or after today’s date" />        --%>
        
        <asp:CustomValidator 
            id="cvEffectiveDate" 
            runat="server" 
            ClientValidationFunction="cvEffectiveDate_ClientValidate"
            OnServerValidate="cvEffectiveDate_ServerValidate"
            Display="Dynamic"
            Text="*"
            ToolTip="Must be after today’s date and time" />    
        
        <ajax:CalendarExtender ID="ceEffectiveDate" runat="server" 
                 TargetControlID="tbEffectiveDate" PopupButtonID="imgEffectiveDate" Enabled="true" />
        <ajax:TextBoxWatermarkExtender ID="tbwmEffectiveDate" runat="server"
            TargetControlID="tbEffectiveDate"
            WatermarkText="Choose An Effective Date"
            WatermarkCssClass="watermark" />                    
    </div>        
    <div class="question">
        <asp:Label ID="tbEffectiveTime" runat="Server" Text="Effective Time:" AssociatedControlID="ddlEffectiveTimeHours" />
        <asp:DropDownList ID="ddlEffectiveTimeHours" runat="server" />
        <asp:Label ID="Label4" runat="Server" Text=":&nbsp;" Width="5px" AssociatedControlID="ddlEffectiveTimeMinutes" />
        <asp:DropDownList ID="ddlEffectiveTimeMinutes" runat="server"  />
        <br /><br />
    </div>
    <div class="question">
        <asp:Label AssociatedControlID="ddlStatus" runat="Server" ID="lblStatus" Text="Status:" />
        <asp:DropDownList ID="ddlStatus" runat="server" />
    </div>
    <div class="question">
        <asp:Label AssociatedControlID="cbSequential" runat="Server" ID="lblSequential" Text="Sequential:" />
        <asp:CheckBox ID="cbSequential" Checked="false" runat="server" /><!--onClick="javascript:setSeqRadioButtons(this.checked);"/>-->
    </div>
    <div class="question">
        <asp:Label AssociatedControlID="ddlUse" runat="Server" ID="lbUse" Text="Use:"   />
        <asp:DropDownList ID="ddlUse" runat="server" 
            onselectedindexchanged="ddlUse_SelectedIndexChanged" AutoPostBack="true" />
    </div>
    <div class="question">
        <asp:Label AssociatedControlID="cbDifferenceAnalysis" runat="Server" ID="lbDifferenceAnalysis" Text="Difference Analysis:" />
        <asp:CheckBox ID="cbDifferenceAnalysis" Checked="false" runat="server"/>
        <asp:HiddenField ID="hfDifferenceAnalysis" runat="server" />
    </div>
    <div id = "xsdInput" class="question">
        <uc1:XsdUpload AllowRemove="false" ID="xsdUploadInput" Label="Input XSD:" Mandatory="true" runat="server" />
    </div>
    <br />
    <div id = "xsdOutput" class="question">    
        <uc1:XsdUpload AllowRemove="true" ID="xsdUploadOutput" Label="Output XSD:" Mandatory="false" runat="server" />
    </div>
</asp:Panel>




