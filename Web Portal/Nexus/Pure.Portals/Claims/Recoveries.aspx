<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="Recoveries.aspx.vb" Inherits="Nexus.Claims_Recoveries" Title="Recoveries" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/Controls/ClaimsProgressBar.ascx" TagName="ProgressBar" TagPrefix="uc3" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="Claims_Recoveries">
        <uc3:ProgressBar ID="ucProgressBar" runat="server"></uc3:ProgressBar>
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal runat="server" Text="<%$ Resources:ltPageHeading %>" ID="ltPageHeading"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div class="md-whiteframe-z0 bg-white">
                    <ul class="nav nav-lines nav-tabs b-danger">
                        <li><a href="#tab-Salvgaedetails" data-bs-toggle="tab" aria-expanded="true"  class="active">
                            <asp:Literal ID="lbltab_Salvgaedetails" runat="server" Text="<%$Resources:lbltab_Salvgaedetails %>"></asp:Literal></a></li>
                        <li><a href="#tab-TPDetails" data-bs-toggle="tab" aria-expanded="true">
                            <asp:Literal ID="lbltab_TPDetails" runat="server" Text="<%$Resources:lbltab_TPDetails %>"></asp:Literal></a></li>
                    </ul>
                    <div class="tab-content clearfix p b-t b-t-2x">
                        <div id="tab-Salvgaedetails" class="tab-pane animated fadeIn active" role="tabpanel">
                            <div id="divSalvageRecovery" runat="server">
                                <legend>
                                    <asp:Label ID="lblSalvageRecoveryReserve" runat="server" Text="<%$ Resources:lbl_SalvageRecoveryReserve %>"></asp:Label></legend>
                                <asp:UpdatePanel ID="updateSalvageRecovery" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                                    <ContentTemplate>
                                        <div class="grid-card table-responsive">
                                            <asp:GridView ID="gvRecovery" runat="server" AllowSorting="true" AutoGenerateColumns="false" GridLines="None" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                <Columns>
                                                    <asp:BoundField HeaderText="<%$ Resources:lbl_RecoveryType %>" DataField="TypeCode"></asp:BoundField>
                                                    <asp:BoundField HeaderText="<%$ Resources:lbl_RecoveryParty %>" DataField="PartyShortName"></asp:BoundField>
                                                    <Nexus:BoundField HeaderText="<%$ Resources:lbl_RecoveredToDate %>" DataField="ReceiptedAmount" DataType="Currency"></Nexus:BoundField>
                                                    <Nexus:BoundField HeaderText="<%$ Resources:lbl_InitialReserve %>" DataField="InitialRecovery" DataType="Currency"></Nexus:BoundField>
                                                    <Nexus:BoundField HeaderText="<%$ Resources:lbl_RevisedReserve %>" DataField="RevisedRecovery" DataType="Currency"></Nexus:BoundField>
                                                    <Nexus:BoundField HeaderText="<%$ Resources:lbl_ThisReserve %>" DataField="RevisionAmount" DataType="Currency"></Nexus:BoundField>
                                                    <Nexus:TemplateField HeaderText="<%$ Resources:lbl_TotalReserve %>" DataType="Currency">
                                                        <itemtemplate>
                                                            <asp:Literal ID="lbl_TotalReserve" runat="server"></asp:Literal>
                                                        </itemtemplate>
                                                    </Nexus:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <div class="rowMenu">
                                                                <ol class="list-inline no-margin">
                                                                    <li class="dropdown no-padding"><a href="#" title="Action Menu" md-ink-ripple="" data-bs-toggle="dropdown" class="md-btn grey-100 md-flat md-btn-circle"><i class="fa fa-ellipsis-v" aria-hidden="true"></i></a>
                                                                        <ol id="menu_<%# Eval("BaseRecoveryKey") %>" class="dropdown-menu dropdown-menu-scale pull-right pull-up top text-color">
                                                                            <li>
                                                                                <asp:LinkButton ID="lblhypEdit" runat="server" Text="<%$ Resources:btnEdit %>"></asp:LinkButton>
                                                                            </li>
                                                                            <li>
                                                                                <asp:LinkButton ID="lblhypDelete" runat="server" CommandName="Delete" Text="<%$ Resources:btnDelete %>" CausesValidation="false" Visible="true"></asp:LinkButton>
                                                                            </li>
                                                                        </ol>
                                                                    </li>
                                                                </ol>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                        <div class="m-b-sm">
                                            <asp:LinkButton ID="btnAdd" runat="server" SkinID="btnSM" TabIndex="4" Text="<%$ Resources:btnAdd %>"></asp:LinkButton>
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="gvRecovery" EventName="RowDeleting"></asp:AsyncPostBackTrigger>
                                    </Triggers>
                                </asp:UpdatePanel>
                                <Nexus:ProgressIndicator ID="upSalRecovery" OverlayCssClass="updating" AssociatedUpdatePanelID="updateSalvageRecovery" runat="server">
                                    <progresstemplate>
                                    </progresstemplate>
                                </Nexus:ProgressIndicator>
                            </div>
                            <div class="form-horizontal">
                                <legend>
                                    <asp:Label ID="lblCoInsuranceRecovery" runat="server" Text="<%$ Resources:lbl_CoInsuranceRecovery %>"></asp:Label></legend>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblRecoveryType" runat="server" AssociatedControlID="txtRecoveryType" class="col-md-4 col-sm-3 control-label">
                                        <asp:Literal ID="ltRecoveryType" runat="server" Text="<%$ Resources:lbl_RecoveryType%>"></asp:Literal>
                                    </asp:Label><div class="col-md-8 col-sm-9">
                                        <asp:TextBox ID="txtRecoveryType" runat="server" CssClass="field-medium form-control" Enabled="False"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblRecoveryAmount" runat="server" AssociatedControlID="txtRecoveryAmount" class="col-md-4 col-sm-3 control-label">
                                        <asp:Literal ID="ltRecoveryAmount" runat="server" Text="<%$ Resources:lbl_RecoveryAmount%>"></asp:Literal>
                                    </asp:Label><div class="col-md-8 col-sm-9">
                                        <asp:TextBox ID="txtRecoveryAmount" runat="server" CssClass="field-medium form-control" Enabled="False"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="grid-card table-responsive">
                                <asp:GridView ID="gvCoInsurance" runat="server" AutoGenerateColumns="False" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                                    <Columns>
                                        <asp:BoundField DataField="RecoveryType" HeaderText="<%$ Resources:lblCoInsRecoveryType %>" Visible="false"></asp:BoundField>
                                        <asp:BoundField DataField="Coinsurer" HeaderText="<%$ Resources:lblCoinsurer %>"></asp:BoundField>
                                        <Nexus:BoundField DataField="SharePercent" HeaderText="<%$ Resources:lblCoInsSharePercent %>" DataType="Percentage"></Nexus:BoundField>
                                        <Nexus:BoundField DataField="RecoveryToDate" HeaderText="<%$ Resources:lblCoInsRecoveryToDate %>" DataType="Currency"></Nexus:BoundField>
                                        <Nexus:BoundField DataField="TotalThisRecovery" DataType="Currency" HeaderText="<%$ Resources:lblTotalThisRecovery %>"></Nexus:BoundField>
                                        <Nexus:BoundField DataField="ThisRecovery" DataType="Currency" HeaderText="<%$ Resources:lblThisRecovery %>" Visible="false"></Nexus:BoundField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div class="form-horizontal">
                                <legend>
                                    <asp:Label ID="lblReinsuranceRecovery" runat="server" Text="<%$ Resources:lbl_ReinsuranceRecovery %>"></asp:Label></legend>

                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblRecoveryTypeforReInsurance" runat="server" AssociatedControlID="txtRecoveryTypeforReInsurance" class="col-md-4 col-sm-3 control-label">
                                        <asp:Literal ID="ltRecoveryTypeforReInsurance" runat="server" Text="<%$ Resources:lbl_RecoveryType%>"></asp:Literal>
                                    </asp:Label><div class="col-md-8 col-sm-9">
                                        <asp:TextBox ID="txtRecoveryTypeforReInsurance" runat="server" CssClass="field-medium form-control" Enabled="False"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblRecoveryAmountforReinsurance" runat="server" AssociatedControlID="txtRecoveryAmountforReinsurance" class="col-md-4 col-sm-3 control-label">
                                        <asp:Literal ID="ltRecoveryAmountforReinsurance" runat="server" Text="<%$ Resources:lbl_RecoveryAmount%>"></asp:Literal>
                                    </asp:Label><div class="col-md-8 col-sm-9">
                                        <asp:TextBox ID="txtRecoveryAmountforReinsurance" runat="server" CssClass="field-medium form-control" Enabled="False"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="grid-card table-responsive">
                                <asp:GridView ID="gvReInsurance" runat="server" AutoGenerateColumns="False" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                                    <Columns>
                                        <asp:BoundField DataField="RecoveryType" HeaderText="<%$ Resources:lblRecoveryType %>" Visible="false"></asp:BoundField>
                                        <asp:BoundField DataField="Reinsurer" HeaderText="<%$ Resources:lblReinsurer %>"></asp:BoundField>
                                        <Nexus:BoundField DataField="SharePercent" HeaderText="<%$ Resources:lblSharePercent %>" DataType="Percentage"></Nexus:BoundField>
                                        <Nexus:BoundField DataField="Salvage" HeaderText="<%$ Resources:lblRecoveryToDate %>" DataType="Currency"></Nexus:BoundField>
                                            <nexus:BoundField DataField="RecoveryToDate" HeaderText="<%$ Resources:lblRecoveryToDate %>"
                                                DataType="Currency" />
                                        <Nexus:BoundField DataField="TotalThisRecovery" DataType="Currency" HeaderText="<%$ Resources:lblTotalThisRecovery %>"></Nexus:BoundField>
                                        <Nexus:BoundField DataField="ThisSalvage" DataType="Currency" HeaderText="<%$ Resources:lblThisRecovery %>" Visible="false"></Nexus:BoundField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                        <div id="tab-TPDetails" class="tab-pane animated fadeIn" role="tabpanel">
                            <div id="divRecoveryAmountForTP" runat="server">
                                <legend>
                                    <asp:Label ID="lblThirdPartyRecovery" runat="server" Text="<%$ Resources:lbl_ThirdPartyRecovery %>"></asp:Label></legend>
                                <asp:UpdatePanel ID="updateThirdPartyRecovery" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="grid-card table-responsive">
                                            <asp:GridView ID="gvRecoveryAmountForTP" runat="server" AllowSorting="true" AutoGenerateColumns="false" GridLines="None" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                                                <Columns>
                                                    <asp:BoundField HeaderText="<%$ Resources:lbl_RecoveryType %>" DataField="TypeCode"></asp:BoundField>
                                                    <asp:BoundField HeaderText="<%$ Resources:lbl_RecoveryParty %>" DataField="PartyShortName"></asp:BoundField>
                                                    <Nexus:BoundField HeaderText="<%$ Resources:lbl_RecoveredToDate %>" DataField="ReceiptedAmount" DataType="Currency"></Nexus:BoundField>
                                                    <Nexus:BoundField HeaderText="<%$ Resources:lbl_InitialReserve %>" DataField="InitialRecovery" DataType="Currency"></Nexus:BoundField>
                                                    <Nexus:BoundField HeaderText="<%$ Resources:lbl_RevisedReserve %>" DataField="RevisedRecovery" DataType="Currency"></Nexus:BoundField>
                                                    <Nexus:BoundField HeaderText="<%$ Resources:lbl_ThisReserve %>" DataField="RevisionAmount" DataType="Currency"></Nexus:BoundField>
                                                    <Nexus:TemplateField HeaderText="<%$ Resources:lbl_TotalReserve %>" DataType="Currency">
                                                        <itemtemplate>
                                                            <asp:Literal ID="lbl_TotalReserve" runat="server"></asp:Literal>
                                                        </itemtemplate>
                                                    </Nexus:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <div class="rowMenu">
                                                                <ol class="list-inline no-margin">
                                                                    <li class="dropdown no-padding"><a href="#" title="Action Menu" md-ink-ripple="" data-bs-toggle="dropdown" class="md-btn grey-100 md-flat md-btn-circle"><i class="fa fa-ellipsis-v" aria-hidden="true"></i></a>
                                                                        <ol id="menu_<%# Eval("BaseRecoveryKey") %>" class="dropdown-menu dropdown-menu-scale pull-right pull-up top text-color">
                                                                            <li>
                                                                                <asp:LinkButton ID="lblhypEdit" runat="server" Text="<%$ Resources:btnEdit %>"></asp:LinkButton>
                                                                            </li>
                                                                            <li>
                                                                                <asp:LinkButton ID="lblhypDelete" runat="server" Text="<%$ Resources:btnDelete %>" CommandName="Delete" CausesValidation="false" Visible="true"></asp:LinkButton>
                                                                            </li>
                                                                        </ol>
                                                                    </li>
                                                                </ol>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>

                                        </div>
                                        <div class="m-b-sm">
                                            <asp:LinkButton ID="btnAddForTP" runat="server" SkinID="btnSM" TabIndex="4" Text="<%$ Resources:btnAdd %>"></asp:LinkButton>
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="gvRecoveryAmountForTP" EventName="RowDeleting"></asp:AsyncPostBackTrigger>
                                    </Triggers>
                                </asp:UpdatePanel>
                                <Nexus:ProgressIndicator ID="upTPRecovery" OverlayCssClass="updating" AssociatedUpdatePanelID="updateThirdPartyRecovery" runat="server">
                                    <progresstemplate>
                                    </progresstemplate>
                                </Nexus:ProgressIndicator>
                            </div>

                            <div class="form-horizontal">
                                <legend>
                                    <asp:Label ID="lbl_TPCoInsuranceRecovery" runat="server" Text="<%$ Resources:lbl_TPCoInsuranceRecovery %>"></asp:Label></legend>

                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lbl_TPRecoveryType" runat="server" AssociatedControlID="txtTPRecoveryType" class="col-md-4 col-sm-3 control-label">
                                        <asp:Literal ID="li_TPRecoveryType" runat="server" Text="<%$ Resources:lbl_TPRecoveryType%>"></asp:Literal>
                                    </asp:Label><div class="col-md-8 col-sm-9">
                                        <asp:TextBox ID="txtTPRecoveryType" runat="server" CssClass="field-medium form-control" Enabled="False"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lbl_TPRecoveryAmount" runat="server" AssociatedControlID="txtTPRecoveryAmount" class="col-md-4 col-sm-3 control-label">
                                        <asp:Literal ID="li_TPRecoveryAmount" runat="server" Text="<%$ Resources:lbl_TPRecoveryAmount%>"></asp:Literal>
                                    </asp:Label><div class="col-md-8 col-sm-9">
                                        <asp:TextBox ID="txtTPRecoveryAmount" runat="server" CssClass="field-medium form-control" Enabled="False"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="grid-card table-responsive">
                                <asp:GridView ID="gvTPCoInsurance" runat="server" AutoGenerateColumns="False" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                                    <Columns>
                                        <asp:BoundField DataField="RecoveryType" HeaderText="<%$ Resources:lblCoInsRecoveryType %>" Visible="false"></asp:BoundField>
                                        <asp:BoundField DataField="Coinsurer" HeaderText="<%$ Resources:lblCoinsurer %>"></asp:BoundField>
                                        <asp:BoundField DataField="SharePercent" HeaderText="<%$ Resources:lblCoInsSharePercent %>" DataFormatString="{0:N2}%"></asp:BoundField>
                                        <asp:BoundField DataField="RecoveryToDate" HeaderText="<%$ Resources:lblCoInsRecoveryToDate %>" DataFormatString="{0:N2}"></asp:BoundField>
                                        <asp:BoundField DataField="TotalThisRecovery" DataFormatString="{0:N2}" HeaderText="<%$ Resources:lblTotalThisRecovery %>"></asp:BoundField>
                                        <asp:BoundField DataField="ThisRecovery" DataFormatString="{0:N2}" HeaderText="<%$ Resources:lblThisRecovery %>" Visible="false"></asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div class="form-horizontal">
                                <legend>
                                    <asp:Label ID="lbl_TPReinsuranceRecovery" runat="server" Text="<%$ Resources:lbl_TPReinsuranceRecovery %>"></asp:Label></legend>

                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lbl_TPReInsuranceRecoveryType" runat="server" AssociatedControlID="txtTPRecoveryTypeforReInsurance" class="col-md-4 col-sm-3 control-label">
                                        <asp:Literal ID="li_TPReInsuranceRecoveryType" runat="server" Text="<%$ Resources:lbl_TPReInsuranceRecoveryType%>"></asp:Literal>
                                    </asp:Label><div class="col-md-8 col-sm-9">
                                        <asp:TextBox ID="txtTPRecoveryTypeforReInsurance" runat="server" CssClass="field-medium form-control" Enabled="False"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lbl_TPReInsuranceRecoveryAmount" runat="server" AssociatedControlID="txtTPRecoveryAmountforReinsurance" class="col-md-4 col-sm-3 control-label">
                                        <asp:Literal ID="li_TPReinsuranceRecoveryAmount" runat="server" Text="<%$ Resources:lbl_TPReInsuranceRecoveryAmount%>"></asp:Literal>
                                    </asp:Label><div class="col-md-8 col-sm-9">
                                        <asp:TextBox ID="txtTPRecoveryAmountforReinsurance" runat="server" CssClass="field-medium form-control" Enabled="False"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="grid-card table-responsive">
                                <asp:GridView ID="gvTPReInsurance" runat="server" AutoGenerateColumns="False" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                                    <Columns>
                                        <asp:BoundField DataField="RecoveryType" HeaderText="<%$ Resources:lblRecoveryType %>" Visible="false"></asp:BoundField>
                                        <asp:BoundField DataField="Reinsurer" HeaderText="<%$ Resources:lblReinsurer %>"></asp:BoundField>
                                        <asp:BoundField DataField="SharePercent" HeaderText="<%$ Resources:lblSharePercent %>" DataFormatString="{0:N2}%"></asp:BoundField>
                                        <asp:BoundField DataField="Recovery" HeaderText="<%$ Resources:lblRecoveryToDate %>" DataFormatString="{0:N2}"></asp:BoundField>
                                        <asp:BoundField DataField="TotalThisRecovery" DataFormatString="{0:N2}" HeaderText="<%$ Resources:lblTotalThisRecovery %>"></asp:BoundField>
                                        <asp:BoundField DataField="ThisRecovery" DataFormatString="{0:N2}" HeaderText="<%$ Resources:lblThisRecovery %>" Visible="false"></asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                            </div>

                        </div>
                    </div>

                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnNext" runat="server" Text="<%$ Resources:btnNext %>" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
    </div>
</asp:Content>
