<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="XsdUpload.ascx.cs" 
    Inherits="WebUserControls_XsdUpload" 
%>
<asp:Label AssociatedControlID="fuXSD" runat="Server" ID="lblXSD" Text="" />
<asp:FileUpload ID="fuXSD" runat="server" Width="475px" />
<asp:RequiredFieldValidator 
    ID="rfvXSD" 
    runat="server" 
    ControlToValidate="tbXSD"
    ValidationGroup="ProfileValidation" 
    Display="Static" 
    Text="*" 
    ToolTip="XSD is mandatory"  
/>    
<asp:Label 
    ID="lblInvalidXSD" 
    Visible="false" 
    runat="server" 
    Text="*" 
    ToolTip="Invalid XSD file"
    ForeColor="Red" />
<div class="nolabelinput">
    <asp:Button 
        ID="btnUploadXSD" 
        runat="server" 
        Text="Upload" 
        onclick="btnUploadXSD_Click" 
    />
    <asp:DropDownList runat="server" ID="ddlAvailableObjects" Visible="false"></asp:DropDownList>
    <asp:LinkButton ID="lbViewXSD" runat="server">View</asp:LinkButton>
    <asp:LinkButton ID="lbRemoveXSD" runat="server" onclick="lbRemoveXSD_Click">Remove</asp:LinkButton>
    <asp:TextBox ID="tbXSD" CssClass="hiddenTextBox" runat="server" />
    <%--<ajax:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" TargetControlID="tbXSD" runat="server" WatermarkText="Browse To Select XSD And Upload" WatermarkCssClass="watermark" />--%>
    
</div>
