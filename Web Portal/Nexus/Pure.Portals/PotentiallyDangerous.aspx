<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PotentiallyDangerous.aspx.vb" Inherits="Nexus.PotentiallyDangerous" MasterPageFile="~/default.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <ajaxToolkit:ToolkitScriptManager runat="server" ID="ajaxScriptManager" EnablePartialRendering="true" CombineScripts="false"></ajaxToolkit:ToolkitScriptManager>
    <div id="Error">
        
    
            
        
                
                
            
                    
                    <fieldset>
                        <ol>
                            <li class="doublewidth">
                               <asp:Literal ID="ltErrorTitle" runat="server" Text="<%$ Resources:lbl_ErrorTitle%>"></asp:Literal>
                            </li>
                        </ol>
                        <ol>
                            <li class="doublewidth">
                                <asp:Literal ID="ltErrorDetail" runat="server" Text="<%$ Resources:lbl_ErrorDetail%>"></asp:Literal>
                            </li>
                        </ol>
                    </fieldset>
                    <div id="divSubmitArea" runat="server" class="card-footer">
                        <asp:LinkButton ID="btnBack" runat="server" Text="<%$ Resources:lbl_btnBack%>" SkinID="btnPrimary"></asp:LinkButton>
                    </div>
                </div>
</asp:Content>
