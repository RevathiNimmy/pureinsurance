<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="FindInsuranceFile.aspx.vb" Inherits="Modal_FindInsuranceFile" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ScriptIncludes" runat="Server"></asp:Content>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <asp:ScriptManager runat="server"></asp:ScriptManager>
    <div id="Modal_FindInsuranceFile">
        <asp:Panel ID="PnlFindInsuranceFile" runat="server" DefaultButton="btnFindNow" CssClass="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblPageHeader" runat="server" Text="<%$ Resources:lblPageHeader%>"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblSearchForm" runat="server" Text="<%$ Resources:lblSearchForm %>"></asp:Label></legend>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblReference" runat="server" AssociatedControlID="txtReference" Text="<%$ Resources:lbl_Reference %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtReference" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblRiskIndex" runat="server" AssociatedControlID="txtRiskIndex" Text="<%$ Resources:lbl_RiskIndex %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtRiskIndex" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label runat="server" ID="lblFileType" AssociatedControlID="drpFileType" Text="<%$ Resources:lblFileType %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:DropDownList runat="server" ID="drpFileType" CssClass="form-control form-select">
                                <asp:ListItem Text="<%$ Resources: liAllTypes%>" Value="ALL"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources: liNBQuote%>" Value="QUOTE"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources: liMTAQuote%>" Value="MTAQUOTE"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources: liPolicy%>" Value="POLICY"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources: liRenewal%>" Value="RENEWAL"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblClientCode" runat="server" AssociatedControlID="txtClientCode" Text="<%$ Resources:txtClientCode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtClientCode" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnNewSearch" runat="server" Text="<%$ Resources:btn_NewSearch%>" CausesValidation="false" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnFindNow" runat="server" Text="<%$ Resources:btn_FindNow%>" SkinID="btnPrimary"></asp:LinkButton>
                
            </div>
        </asp:Panel>

        <Nexus:WildCardValidator ID="vldWildCard" AllowWildCardAtEndErrorMessage="<%$Resources:lbl_WildCardAtEnd_Error %>" NoWildCardErrorMessage="<%$Resources:lbl_NoWildCard_Error %>" ControlsToValidate="txtReference,txtRiskIndex,txtClientCode" Condition="Auto" Display="none" runat="server" EnableClientScript="true">
        </Nexus:WildCardValidator>
        <asp:ValidationSummary ID="vldFindInsuranceFileSummary" runat="server" DisplayMode="BulletList" ShowSummary="true" HeaderText="<%$Resources:lbl_ValidationSummary %>" CssClass="validation-summary"></asp:ValidationSummary>

        <asp:UpdatePanel ID="updFindInsuranceFile" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="True">
            <ContentTemplate>
                <div class="grid-card table-responsive">
                    <asp:GridView ID="grdvSearchResults" runat="server" PageSize="10" AllowPaging="true" PagerSettings-Mode="Numeric" EnableViewState="false" AutoGenerateColumns="false" GridLines="None" EmptyDataText="<%$ Resources:ErrorMessage %>" EmptyDataRowStyle-CssClass="noData">
                        <Columns>
                            <asp:BoundField DataField="InsuranceRef" HeaderText="<%$ Resources:lbl_FolderReference%>"></asp:BoundField>
                            <asp:BoundField DataField="ProductDescription" HeaderText="<%$ Resources:lbl_Product %>"></asp:BoundField>
                            <asp:BoundField DataField="Status" HeaderText="<%$ Resources:lbl_Type %>"></asp:BoundField>
                            <asp:BoundField DataField="ClientName" HeaderText="<%$ Resources:lbl_InsuranceHolder %>" HtmlEncode="false"></asp:BoundField>
                            <asp:BoundField DataField="LastModifiedDate" DataFormatString="{0:d}" HeaderText="<%$ Resources:lbl_DateModified %>"></asp:BoundField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <div class="rowMenu">
                                        <ol id="menu_<%# Eval("InsuranceFileKey") %>" class="list-inline no-margin">
                                            <li>
                                                <asp:LinkButton ID="btnSelect" SkinID="btnGrid"  runat="server" OnClientClick=<%# "self.parent.setQuote('" + Eval("InsuranceRef").ToString() + "','" + Eval("InsuranceFileKey").ToString() + "','" + Eval("ClientShortName").ToString() + "');"%>><i class='fa fa-check' aria-hidden='true'></i> Select</asp:LinkButton>
                                                <%--<a href="#" onclick="self.parent.setQuote('<%# DataBinder.Eval(Container.DataItem,"InsuranceRef")%>','<%# DataBinder.Eval(Container.DataItem,"InsuranceFileKey")%>','<%# Nexus.Utils.PureEncode(DataBinder.Eval(Container.DataItem, "ClientShortName"))%>');">select</a>--%>
                                            </li>
                                        </ol>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="grdvSearchResults" EventName="PageIndexChanging"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="grdvSearchResults" EventName="RowDataBound"></asp:AsyncPostBackTrigger>
            </Triggers>
        </asp:UpdatePanel>
        <Nexus:ProgressIndicator ID="upFndInsuranceFile" OverlayCssClass="updating" AssociatedUpdatePanelID="updFindInsuranceFile" runat="server">
            <ProgressTemplate>
            </ProgressTemplate>
        </Nexus:ProgressIndicator>
    </div>
</asp:Content>
