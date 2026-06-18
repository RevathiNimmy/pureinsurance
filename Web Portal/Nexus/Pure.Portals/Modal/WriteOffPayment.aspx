<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/default.master" CodeFile="WriteOffPayment.aspx.vb" Inherits="Nexus.Modal_WriteOffPayment" %>


<%--<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>--%>


<asp:Content ContentPlaceHolderID="cntMainBody" runat="server" ID="cntMainBody">

    <div id="Modal_WriteOffPayment">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Label runat="server" Text="<%$ Resources:lbl_WriteOffHeading %>" ID="WriteOffHeader"></asp:Label>
                </h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend> <h5>
                        <asp:Label runat="server" ID="lblWriteOffHeading" Text="<%$ Resources:lblWriteOffHeading %>" ></asp:Label></h5>
                    </legend>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lbl_WriteOfftxt" runat="server" class="control-label" Text="<%$ Resources:lbl_WriteOfftxt %>"></asp:Label>
                         <div class="col-md-8 col-sm-9">
                             <asp:TextBox ID="txtWriteoff_amtVal" TabIndex="1" CssClass="form-control" MaxLength="9" runat="server"></asp:TextBox>
                        </div>
                    </div>
                         <div class="form-group form-group-sm col-lg-12 col-md-12 col-sm-12">
                    <asp:label ID="rvWriteoffRange" runat="server" Visible ="false" CssClass="error" SetFocusOnError="true"></asp:label>
                </div>
            </div>
        </div>   
         <div class="card-footer">
                <asp:LinkButton ID="WriteOffBtn_Cancel" runat="server" CausesValidation="false" Text="<%$ Resources:btnCancel %>" TabIndex="5" SkinID="btnSecondary"></asp:LinkButton>
             <asp:LinkButton ID="WriteOffBtn_Ok" runat="server" CausesValidation="True" ControlToValidate="txtWriteoff_amtVal" TabIndex="5" Text="<%$ Resources:btnOk %>" SkinID="btnPrimary"></asp:LinkButton>
       </div> 
    </div>
    </div>
</asp:Content>


