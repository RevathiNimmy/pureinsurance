<%@ Page Language="C#" EnableEventValidation="false" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="CompileError.aspx.cs" Inherits="WebPages_RuleSets_CompileError" ValidateRequest="false" %>
<asp:Content ID="cntError" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:Panel runat="server" ID="cpe">
        
            <asp:Label runat="server" ID="lblDataEntry" CssClass="dataentrylabel">Publish Errors</asp:Label>
            
            <div class="dataentry" runat="server">
            <asp:Panel runat="server" ID="pnlDataEntry" Height="258px">
                
                <asp:TextBox runat="server" id="errorText" TextMode="MultiLine" ReadOnly="True" 
                    Width="100%" Height="104px"></asp:TextBox>
                
                <div style="clear:both" />
                
            </asp:Panel> 
            </div> 
                     
            <asp:Label runat="server" ID="Label1" CssClass="dataentrylabel">Generated Code</asp:Label>   
            <div id="Div1" class="dataentry" runat="server">
            <asp:Panel runat="server" ID="Panel1">
                
                <asp:TextBox runat="server" id="generatedCodeText" TextMode="MultiLine" ReadOnly="True" 
                    Width="100%" Height="250px"></asp:TextBox>
                
                <div style="clear:both" />
                
            </asp:Panel> 
            </div>                       
        </asp:Panel>
</asp:Content>
