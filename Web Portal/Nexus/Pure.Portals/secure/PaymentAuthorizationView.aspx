<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="PaymentAuthorizationView.aspx.vb" Inherits="Nexus.PaymentAuthorizationView" EnableEventValidation="false" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    
    <div id="Modal_Viewallocation">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblPageHeader" runat="server" Text="<%$ Resources:lblPageHeader%>"></asp:Literal>
                </h1>
            </div>
             <div class="card-body clearfix">
                 <div class="form-horizontal"> 
                   
                        <div class="row ">
                            <div class="col-3 ">            
                                 <asp:Label ID="lbl_NewComments"  style="margin-left:15px;margin-top: 70px;" AssociatedControlID ="txtNewComments" runat="server" Text="<%$ Resources:lblNewComments%>" class="col-md-3 col-sm-4 control-label"></asp:Label>                      
                           </div> 
                             <div class="col-8 ">
                                <asp:TextBox ID="txtNewComments" runat="server" TextMode="MultiLine" style="margin-top: 20px;margin-bottom:15px; height:100px;" CssClass="form-control"></asp:TextBox>
                            </div> 
                      
                        </div>                        
                    </div>
                      </div>
                         <div class="card-footer">
                            <%--<asp:LinkButton ID="ViewBtn_Back" runat="server" Text="<%$ Resources:btnBack %>"  SkinID="btnSecondary"></asp:LinkButton>--%>
                            <asp:LinkButton ID="ViewBtn_Next" runat="server"  TabIndex="5" Text="<%$ Resources:btnNext %>" SkinID="btnPrimary"></asp:LinkButton>
                         </div>
                     </div>
                 </div>    
</asp:Content>
