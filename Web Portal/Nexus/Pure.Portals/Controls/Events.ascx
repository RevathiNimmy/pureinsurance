<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Events.ascx.vb" Inherits="Nexus.secure_Controls_Events" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>

<%--<asp:ScriptManager ID="smEventDetails" runat="server" />--%>
<div id="Controls_EventList">

    <asp:UpdatePanel ID="updEventList" runat="server" UpdateMode="Conditional" CssClass="card" ChildrenAsTriggers="true">
        <ContentTemplate>

            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblHeading" runat="server" Text="<%$ Resources:lblPageHeader %>"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblPageheader" runat="server" Text="<%$ Resources:lbl_Page_header%>"></asp:Label>
                    </legend>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblEventType" runat="server" AssociatedControlID="ddlEventType" Text="<%$ Resources:lbl_EventType %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:DropDownList ID="ddlEventType" runat="server" AutoPostBack="false" CssClass="form-control" />
                            <asp:RequiredFieldValidator ID="vldrqdEventType" runat="server" ControlToValidate="ddlEventType" Display="none" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_EventTypeRequiredMsg%>" InitialValue="" ValidationGroup="EventListGroup"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblUserName" runat="server" AssociatedControlID="ddlUserName" CssClass="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="litUserName" runat="server" Text="<%$ Resources:lbl_UserName %>" /></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:DropDownList ID="ddlUserName" runat="server" CssClass="form-control form-select" AutoPostBack="false"
                              />

                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12 clear">
                        <asp:Label ID="lbl_EventFromDate" runat="server" AssociatedControlID="txtEventFromDate" CssClass="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="litEventFromDate" runat="server" Text="<%$ Resources:lbl_EventFromDate%>" /></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="txtEventFromDate" runat="server" CssClass="form-control" />
                                <uc1:CalendarLookup ID="calEventFromDate" runat="server" LinkedControl="txtEventFromDate"
                                    HLevel="3" />
                                <asp:RequiredFieldValidator ID="vldrqdfromdate" runat="server" ControlToValidate="txtEventFromDate" Display="none" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_FromdateRequiredMsg%>" InitialValue="" ValidationGroup="EventListGroup"></asp:RequiredFieldValidator>

                            </div>
                            <asp:RangeValidator ID="rangevldFromDate" runat="server" Display="None" Type="date"
                                ErrorMessage="<%$ Resources:lbl_RanErrMsgInvalidFromDate %>" ControlToValidate="txtEventFromDate"
                                MinimumValue="01/01/1900" SetFocusOnError="True" Enabled="true" ValidationGroup="EventListGroup"
                                MaximumValue="01/12/9998" />


                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lbl_EventToDate" runat="server" AssociatedControlID="txtEventToDate" CssClass="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="litEventToDate" runat="server" Text="<%$ Resources:lbl_EventToDate %>" /></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="txtEventToDate" runat="server" CssClass="form-control" />
                                <uc1:CalendarLookup ID="calEventToDate" runat="server" LinkedControl="txtEventToDate"
                                    HLevel="3" />
                            </div>
                            <asp:RangeValidator ID="rangevldToDate" runat="server" Display="None" Type="date"
                                ErrorMessage="<%$ Resources:lbl_RanErrMsgInvalidToDate %>" ControlToValidate="txtEventToDate"
                                MinimumValue="01/01/1900" SetFocusOnError="True" Enabled="true" ValidationGroup="EventListGroup"
                                MaximumValue="01/12/9998" />
                            <asp:CustomValidator ID="CustVldDate" runat="server" Display="None" SetFocusOnError="True"
                                ValidationGroup="EventListGroup" />
                            <asp:RequiredFieldValidator ID="vlrqdTodate" runat="server" ControlToValidate="txtEventToDate" Display="none" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_TodateRequiredMsg%>" InitialValue="" ValidationGroup="EventListGroup"></asp:RequiredFieldValidator>

                        </div>
                    </div>
                    <asp:HiddenField ID="hfModuleKey" runat="server" />
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton runat="server" ID="btnNewSearch" Text="<%$ Resources:btn_btnNewSearch %>" EnableViewState="false" CausesValidation="false" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton runat="server" ID="btnRefresh" Text="<%$ Resources:btn_Refresh %>" EnableViewState="false" ValidationGroup="EventListGroup" CausesValidation="true" SkinID="btnPrimary"></asp:LinkButton>

            </div>

            <asp:ValidationSummary ID="vldSummary" runat="server" DisplayMode="BulletList" ShowSummary="true" ValidationGroup="EventListGroup" HeaderText="<%$Resources:lbl_ValidationSummary %>" CssClass="validation-summary"></asp:ValidationSummary>


            <div class="grid-card table-responsive no-margin">
                <asp:GridView ID="gvEventList" runat="server" AutoGenerateColumns="False" GridLines="None" PageSize="10" AllowPaging="true" AllowSorting="true" PagerSettings-Mode="Numeric" OnPageIndexChanging="gvEventList_PageIndexChanging" EmptyDataText="<%$ Resources:ErrorMessage %>" EmptyDataRowStyle-CssClass="noData">
                    <Columns>
                        <asp:BoundField DataField="ScreenDescription" HeaderText="<%$ Resources:gv_level %>" SortExpression="ScreenDescription"></asp:BoundField>
                        <asp:BoundField DataField="FieldDescription" HeaderText="<%$ Resources:gv_Property %>" SortExpression="FieldDescription"></asp:BoundField>
                        <asp:BoundField DataField="OldValue" HeaderText="<%$ Resources:gv_OldVal %>" SortExpression="OldValue"></asp:BoundField>
                        <asp:BoundField DataField="NewValue" HeaderText="<%$ Resources:gv_NewVal %>" SortExpression="NewValue"></asp:BoundField>
                        <asp:BoundField DataField="UserName" HeaderText="<%$ Resources:gv_ModifiedBy %>" SortExpression="UserName"></asp:BoundField>
                        <asp:BoundField DataField="ModifiedOn" HeaderText="<%$ Resources:gv_ModifiedOn %>" SortExpression="ModifiedOn"></asp:BoundField>

                    </Columns>
                </asp:GridView>
            </div>

            <div class="card-footer">
    <asp:LinkButton runat="server" ID="btnExport" Text="<%$ Resources:btn_Export %>" Visible="false" EnableViewState="false" ValidationGroup="EventListGroup" CausesValidation="true" SkinID="btnPrimary"></asp:LinkButton>

</div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gvEventList" EventName="PageIndexChanging"></asp:AsyncPostBackTrigger>
 </Triggers>
    </asp:UpdatePanel>
    <Nexus:ProgressIndicator ID="upEventList" OverlayCssClass="updating" AssociatedUpdatePanelID="updEventList" runat="server">
        <progresstemplate>
        </progresstemplate>
    </Nexus:ProgressIndicator>
</div>
