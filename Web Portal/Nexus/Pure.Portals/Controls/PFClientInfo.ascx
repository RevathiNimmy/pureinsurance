<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PFClientInfo.ascx.vb"
    Inherits="Nexus.Controls_PFClientInfo" %>
<div id="Controls_PFClientInfo">
    <asp:PlaceHolder ID="PnlPFClientInfo" runat="server">
        <div class="card">
            <div class="card-body clearfix">
                
                <div class="form-horizontal">
                <legend>Client Information</legend>
                    
                                            
                        <div class="doublewidth form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblClientName" runat="server" AssociatedControlID="txtClientName" Text="<%$ Resources:lbl_Client_Name %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9"><asp:TextBox ID="txtClientName" runat="server" CssClass="field-extralarge field-mandatory form-control"></asp:TextBox></div>
                            <asp:RequiredFieldValidator ID="RqdDd1Status" runat="server" ControlToValidate="txtClientName" Display="none" Enabled="true" SetFocusOnError="true" ErrorMessage="<%$ Resources:ClientName_ErrorMessage%>"></asp:RequiredFieldValidator>
                        </div>
                        <div class="doublewidth form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblStreetNoName" runat="server" AssociatedControlID="txtStreetNoName" Text="<%$ Resources:lbl_StreetNo_Name %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9"><asp:TextBox ID="txtStreetNoName" runat="server" CssClass="field-extralarge form-control"></asp:TextBox></div>
                        </div>
                        <div class="doublewidth form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblLocality" runat="server" AssociatedControlID="txtLocality" Text="<%$ Resources:lbl_Locality %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9"><asp:TextBox ID="txtLocality" runat="server" CssClass="field-extralarge form-control"></asp:TextBox></div>
                        </div>
                        <div class="doublewidth form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblTown" runat="server" AssociatedControlID="txtTown" Text="<%$ Resources:lbl_Town %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9"><asp:TextBox ID="txtTown" runat="server" CssClass="field-extralarge form-control"></asp:TextBox></div>
                        </div>
                        <div class="doublewidth form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblCountry" runat="server" AssociatedControlID="dd1Country" Text="<%$ Resources:lbl_Country %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9"><asp:DropDownList ID="dd1Country" runat="server" CssClass="field-extralarge form-control form-select">
                                <asp:ListItem Text="United Kingdom"></asp:ListItem>
                            </asp:DropDownList></div>
                        </div>
                        <div class="doublewidth form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblPostCode" runat="server" AssociatedControlID="txtPostCode" Text="<%$ Resources:lbl_PostCode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9"><asp:TextBox ID="txtPostCode" runat="server" CssClass="field-extralarge form-control"></asp:TextBox></div>
                        </div>
                        </div>
                        <div class="form-horizontal">
                         <legend>Contact Information</legend>
                        <ol>
                        <div class="doublewidth phone-wrapper form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblAreaCode" runat="server" Text="<%$ Resources:lbl_AreaCode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <asp:Label ID="lblNumber" runat="server" Text="<%$ Resources:lbl_Number %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <asp:Label ID="lblExtension" runat="server" Text="<%$ Resources:lbl_Extension %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        </div>
                        <div class="doublewidth form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblPhoneNumber" runat="server" AssociatedControlID="txtPhAreaCode" Text="<%$ Resources:lbl_PhoneNumber %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9"><asp:TextBox ID="txtPhAreaCode" runat="server" CssClass="short form-control"></asp:TextBox></div>
                            <asp:TextBox ID="txtPhNumber" runat="server"></asp:TextBox>
                            <asp:TextBox ID="txtPhExt" runat="server"></asp:TextBox>
                        </div>
                        <div class="doublewidth form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblFaxNumber" runat="server" AssociatedControlID="txtFaxAreaCode" Text="<%$ Resources:lbl_FaxNumber %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9"><asp:TextBox ID="txtFaxAreaCode" runat="server" CssClass="short form-control"></asp:TextBox></div>
                            <asp:TextBox ID="txtFaxNumber" runat="server"></asp:TextBox>
                        </div>
                        <div class="doublewidth form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblEmailAddress" runat="server" AssociatedControlID="txtEmailAddress" Text="<%$ Resources:lbl_Email_Address %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9"><asp:TextBox ID="txtEmailAddress" runat="server" CssClass="field-extralarge form-control"></asp:TextBox></div>
                        </div>
                    </ol>
                </div>
            </div>
            
        </div>
    </asp:PlaceHolder>
</div>
