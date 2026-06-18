<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/default.master"
    CodeFile="TemplatePage.aspx.vb" Inherits="Nexus.TemplatePage" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <div id="TemplatePage">
        
    
            
        
                
                
            
                    
                    <h1>
                        <asp:Literal ID="lblHeader" runat="server" Text="Template header"></asp:Literal></h1>
                    <div class="card">
                        <div class="card-body clearfix">
                            
                            <div class="form-horizontal">
                                <legend>
                                    <asp:Label ID="lblLegendHeader" runat="server" Text="Legend"></asp:Label></legend>
                                
                            
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblLabel" runat="server" AssociatedControlID="txtTextbox" Text="Label" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9"><asp:TextBox ID="txtTextbox" runat="server" cssclass="form-control"></asp:TextBox></div>
                                    </div>
                                </div>
                        </div>
                        
                    </div>
                    <asp:ValidationSummary ID="ValidationSummary1" DisplayMode="BulletList" HeaderText="Validation summary header" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
                    <div class="card-footer">
                        <asp:LinkButton ID="btnSubmit" runat="server" Text="Submit" SkinID="btnPrimary"></asp:LinkButton>
                    </div>
                </div>
</asp:Content>
