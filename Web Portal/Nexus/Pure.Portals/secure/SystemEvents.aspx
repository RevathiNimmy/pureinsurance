<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SystemEvents.aspx.vb" Inherits="Nexus.secure_SystemEvents"
    MasterPageFile="~/default.master" EnableViewState="true" ValidateRequest="false" %>

<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <asp:ScriptManager ID="smFindCllient" runat="server"></asp:ScriptManager>

    <script type="text/javascript">
        function beforeSubmit() {
            if (typeof (ValidatorOnSubmitCustom) == "function" && ValidatorOnSubmitCustom() == true) {
                debugger;
                if (theForm.__EVENTARGUMENT.value.indexOf("Page$") == -1 &&
                    theForm.__EVENTARGUMENT.value.indexOf("Sort$") == -1 &&
                    theForm.__EVENTTARGET.value == "") {
                    if (document.activeElement != null) {
                        document.activeElement.blur();
                    }
                    enableScreen();
                }
                else {
                    enableScreen();
                }
                return true;
            }
            else {
                Page_BlockSubmit = !Page_BlockSubmit;
                return false;
            }
        }


        function enableScreen() {
            $.unblockUI();

        }

    </script>



    <div id="Controls_EventList">
        <div class="card-heading">
            <h1>
                <asp:Literal ID="lblHeading" runat="server" Text="<%$ Resources:lblPageHeader %>"></asp:Literal></h1>
        </div>
        <div class="card-body clearfix">

            <asp:UpdatePanel ID="upSearchEvent" runat="server" UpdateMode="Conditional">
                <ContentTemplate>

                    <div class="form-horizontal">
                        <legend>
                            <asp:Label ID="lblPageheader" runat="server" Text="<%$ Resources:lbl_Page_header%>"></asp:Label>
                        </legend>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblEventType" runat="server" AssociatedControlID="ddlEventType" Text="<%$ Resources:lbl_EventType %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:DropDownList ID="ddlEventType" runat="server" AutoPostBack="false" CssClass="form-control" />
                            </div>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblUserName" runat="server" AssociatedControlID="ddlUserName" CssClass="col-md-4 col-sm-3 control-label">
                                <asp:Literal ID="litUserName" runat="server" Text="<%$ Resources:lbl_UserName %>" /></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:DropDownList ID="ddlUserName" runat="server" CssClass="form-control form-select" AutoPostBack="false" />

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
                </ContentTemplate>
            </asp:UpdatePanel>

            <div class="card-footer">
                <asp:LinkButton runat="server" ID="btnNewSearch" Text="<%$ Resources:btn_btnNewSearch %>" EnableViewState="false" CausesValidation="false" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton runat="server" ID="btnRefresh" Text="<%$ Resources:btn_Refresh %>" EnableViewState="false" ValidationGroup="EventListGroup" CausesValidation="true" SkinID="btnPrimary"></asp:LinkButton>

            </div>

            <asp:ValidationSummary ID="vldSummary" runat="server" DisplayMode="BulletList" ShowSummary="true" ValidationGroup="EventListGroup" HeaderText="<%$Resources:lbl_ValidationSummary %>" CssClass="validation-summary"></asp:ValidationSummary>
            <asp:UpdatePanel ID="updSearchEvents" runat="server" ChildrenAsTriggers="true" UpdateMode="Always">
                <ContentTemplate>
                    <div class="grid-card table-responsive no-margin">
                        <asp:GridView ID="gvEventList" runat="server" AutoGenerateColumns="False" GridLines="None" PageSize="15" AllowPaging="true" AllowSorting="true" PagerSettings-Mode="Numeric" OnPageIndexChanging="gvEventList_PageIndexChanging" EmptyDataText="<%$ Resources:ErrorMessage %>" EmptyDataRowStyle-CssClass="noData">
                            <Columns>
                                    <asp:TemplateField HeaderText="<%$ Resources:gv_level%>" SortExpression="ScreenDescription">
                                         <ItemTemplate>
                                             <asp:Label ID="lblScreenDescription" runat="server" Text='<%# Eval("ScreenDescription") %>'
                                                 Style="max-width: 300px; display: block; white-space: pre-wrap; word-wrap: break-word;">
                                             </asp:Label>
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                 <asp:TemplateField HeaderText="<%$ Resources:gv_Property%>" SortExpression="FieldDescription">
                                         <ItemTemplate>
                                             <asp:Label ID="lblFieldDescription" runat="server" Text='<%# Eval("FieldDescription") %>'
                                                 Style="max-width: 300px; display: block; white-space: pre-wrap; word-wrap: break-word;">
                                             </asp:Label>
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:gv_OldVal %>" SortExpression="OldValue">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOldValue" runat="server" Text='<%# Eval("OldValue") %>'
                                            Style="max-width: 300px; display: block; white-space: pre-wrap; word-wrap: break-word;">
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:gv_NewVal %>" SortExpression="NewValue">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNewValue" runat="server" Text='<%# Eval("NewValue") %>'
                                            Style="max-width: 300px; display: block; white-space: pre-wrap; word-wrap: break-word;">
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="UserName" ControlStyle-Width="20%" ItemStyle-Wrap="true" HeaderText="<%$ Resources:gv_ModifiedBy %>" SortExpression="UserName"></asp:BoundField>
                                <asp:BoundField DataField="ModifiedOn" ControlStyle-Width="20%" ItemStyle-Wrap="true" HeaderText="<%$ Resources:gv_ModifiedOn %>" SortExpression="ModifiedOn"></asp:BoundField>
                            </Columns>
                        </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="gvEventList" EventName="PageIndexChanging"></asp:AsyncPostBackTrigger>


                </Triggers>
            </asp:UpdatePanel>
        </div>

        <div class="card-footer">
            <asp:LinkButton runat="server" ID="btnExport" Text="<%$ Resources:btn_Export %>" Visible="false" SkinID="btnPrimary"></asp:LinkButton>



        </div>
    </div>
</asp:Content>
