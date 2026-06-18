<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/default.master"
    CodeFile="WrmTask.aspx.vb" Inherits="Nexus.Modal_WrmTask" %>

<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<asp:Content ContentPlaceHolderID="cntMainBody" runat="server" ID="cntMainBody">

    <script language="javascript" type="text/javascript">

        function pageLoad() {
            $('.nav-tabs a[href="#tab-Details"]').tab('show');
            //this is needed if the trigger is external to the update panel   
            var manager = Sys.WebForms.PageRequestManager.getInstance();
            manager.add_beginRequest(OnBeginRequest);
        }

        function OnBeginRequest(sender, args) {
            var postBackElement = args.get_postBackElement();
            if (postBackElement.id == 'hypView') {
                $get(uprogQuotes).style.display = "block";
            }
        }


    </script>

    <asp:ScriptManager runat="server"></asp:ScriptManager>
    <div id="Modal_WrmTask">
        <asp:Panel ID="pnlRiskDetls" runat="server" Visible="true" EnableViewState="true">
            <div class="card">
                <div class="card-heading">
                    <h1>
                        <asp:Label ID="lblTaskHeading" runat="server" Text="<%$ Resources:lblTaskHeading %>" EnableViewState="false"></asp:Label>
                    </h1>
                </div>
                <div class="card-body clearfix">
                    <div class="md-whiteframe-z0 bg-white">
                        <ul class="nav nav-lines nav-tabs b-danger">
                            <li><a href="#tab-Details" data-bs-toggle="tab" aria-expanded="true" class="active">Details</a></li>
                            <li><a href="#tab-Audit" data-bs-toggle="tab" aria-expanded="true">Audit</a></li>
                        </ul>
                        <div class="tab-content clearfix p b-t b-t-2x">
                            <div id="tab-Details" class="tab-pane animated fadeIn active" role="tabpanel">
                                <div class="form-horizontal">
                                    <legend>
                                        <asp:Label ID="ltHeading" runat="server" Text="<%$ Resources:lbl_TaskDetails %>"></asp:Label>
                                    </legend>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblTaskGroup" runat="server" AssociatedControlID="ddlTaskGroup" Text="<%$ Resources:lbl_TaskGroup %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <NexusProvider:LookupList ID="ddlTaskGroup" runat="server" CssClass="field-medium form-control form-select" OnSelectedIndexChange="ddlTaskGroup_SelectedIndexChange" ListCode="PMwrk_Task_Group" ListType="PMLookup" AutoPostBack="True"></NexusProvider:LookupList>
                                        </div>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblTask" runat="server" AssociatedControlID="ddlTask" Text="<%$ Resources:lbl_Task %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <asp:DropDownList ID="ddlTask" runat="server" CssClass="field-long form-control form-select"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblDueDateTime" runat="server" AssociatedControlID="ddlDueDateTime" CssClass="col-md-4 col-sm-3 control-label" Text="<%$ Resources:lbl_DueDate/Time %>"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <div class="row row-xs">
                                                <div class="col-sm-5">
                                                    <asp:DropDownList ID="ddlDueDateTime" runat="server" CssClass="form-control form-select" OnSelectedIndexChanged="ddlDueDateTime_SelectedIndexChanged" AutoPostBack="True">
                                                        <asp:ListItem Text="<%$ Resources:lbl_Select%>" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:lbl_Today %>" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:lbl_Tomorrow  %>" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:lbl_Within a Week %>" Value="3"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:lbl_Within a Month %>" Value="4"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:lbl_Within  a Quater %>" Value="5"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:lbl_Within a Year %>" Value="6"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-sm-4">
                                                    <asp:TextBox ID="txtDueDate" runat="server" CssClass="form-control field-mandatory"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="vldDueDateRequired" runat="server" ValidationGroup="AddTaskButtonGroup" SetFocusOnError="True" ErrorMessage="<%$ Resources:lbl_vldDueDateRequired %> " ControlToValidate="txtDueDate" Display="none"></asp:RequiredFieldValidator>
                                                    <asp:RangeValidator ID="rngvDueDate" runat="Server" Type="Date" Display="none" ControlToValidate="txtDueDate" SetFocusOnError="True" MinimumValue="01/01/1900" MaximumValue="01/12/9998" ErrorMessage="<%$ Resources:lbl_ErrorMessageforDueDate %>" ValidationGroup="AddTaskButtonGroup"></asp:RangeValidator>
                                                </div>
                                                <div class="col-sm-3">
                                                    <asp:TextBox ID="txtTime" runat="server" CssClass="form-control field-mandatory"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="vldTimeRequired" runat="server" ValidationGroup="AddTaskButtonGroup" SetFocusOnError="True" ErrorMessage=" <%$ Resources:lbl_vldTimeRequired %> " ControlToValidate="txtTime" Display="none"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblClient" runat="server" AssociatedControlID="txtClient" Text="<%$ Resources:lbl_Client %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <asp:TextBox ID="txtClient" runat="server" CssClass="field-mandatory form-control" MaxLength="255"></asp:TextBox>
                                        </div>
                                        <asp:RequiredFieldValidator ID="vldClientRequired" runat="server" ValidationGroup="AddTaskButtonGroup" SetFocusOnError="True" ErrorMessage="<%$ Resources:lbl_vldClientRequired %> " ControlToValidate="txtClient" Display="none"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblDescription" runat="server" AssociatedControlID="txtDescription" Text="<%$ Resources:lbl_Description %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <asp:TextBox ID="txtDescription" runat="server" CssClass="field-mandatory form-control" MaxLength="255" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                        <asp:RequiredFieldValidator ID="vldDescriptionRequired" runat="server" ValidationGroup="AddTaskButtonGroup" SetFocusOnError="True" ErrorMessage="<%$ Resources:lbl_vldDescriptionRequired %>" ControlToValidate="txtDescription" Display="none"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblUrgent" runat="server" AssociatedControlID="chkUrgent" Text="<%$ Resources:lbl_Urgent %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <asp:CheckBox ID="chkUrgent" runat="server" Text=" " CssClass="asp-check"></asp:CheckBox>
                                        </div>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblComplete" runat="server" AssociatedControlID="chkComplete" Text="<%$ Resources:lbl_Complete %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <asp:CheckBox ID="chkComplete" runat="server" Text=" " CssClass="asp-check"></asp:CheckBox>
                                        </div>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblReview" runat="server" AssociatedControlID="chkReview" Text="<%$ Resources:lbl_Review %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <asp:CheckBox ID="chkReview" runat="server" Text=" " CssClass="asp-check"></asp:CheckBox>
                                        </div>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblUserGroup" runat="server" AssociatedControlID="ddlUserGroup" Text="<%$ Resources:lbl_UserGroup %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <asp:DropDownList ID="ddlUserGroup" runat="server" OnSelectedIndexChanged="ddlUserGroup_SelectedIndexChanged" AutoPostBack="True" CssClass="form-control form-select">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="AlternatelistItem form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblUser" runat="server" AssociatedControlID="ddlUser" Text="<%$ Resources:lbl_User %> " class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <asp:DropDownList ID="ddlUser" runat="server" OnSelectedIndexChanged="ddlUser_SelectedIndexChanged" AutoPostBack="True" CssClass="form-control form-select">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="tab-Audit" class="tab-pane animated fadeIn" role="tabpanel">
                                <div class="form-horizontal">
                                    <legend>
                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lbl_AuditDetails %>"></asp:Label>
                                    </legend>

                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblloggedBy" runat="server" AssociatedControlID="txtLoggedBy" Text="<%$ Resources:lbl_LoggedBy %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <asp:TextBox ID="txtLoggedBy" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblAt" runat="server" AssociatedControlID="txtLoggedDate" CssClass="col-md-4 col-sm-3 control-label" Text="<%$ Resources:lbl_LoggedAt %>"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <div class="row row-xs">
                                                <div class="col-sm-8">
                                                    <asp:TextBox ID="txtLoggedDate" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                </div>
                                                <div class="col-sm-4">
                                                    <asp:TextBox ID="txtLoggedTime" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12" id="li_LastModifiedBy" runat="server">
                                        <asp:Label ID="lbl_LastModifiedBy" runat="server" AssociatedControlID="txtLastModifiedBy" Text="<%$ Resources:lbl_LastModifiedBy %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <asp:TextBox ID="txtLastModifiedBy" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div id="li_LastModifiedAt" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">

                                        <asp:Label ID="lblLastModifiedAt" runat="server" AssociatedControlID="txtLoggedBy" CssClass="col-md-4 col-sm-3 control-label" Text="<%$ Resources:lbl_ModifiedAt %>"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <div class="row row-xs">
                                                <div class="col-sm-8">
                                                    <asp:TextBox ID="txtLastModifiedDate" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                </div>
                                                <div class="col-sm-4">
                                                    <asp:TextBox ID="txtLastModifiedTime" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </div>
                            <asp:UpdatePanel ID="updWrmTask" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="True">
                                <ContentTemplate>
                                    <div class="grid-card table-responsive">
                                        <asp:GridView ID="gvTaskLog" runat="server" AutoGenerateColumns="False" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                                            <Columns>
                                                <asp:BoundField DataField="UserName" HeaderText="<%$ Resources:lbl_CreatedBy %>"></asp:BoundField>
                                                <asp:BoundField HeaderText="<%$ Resources:lbl_Date/Time %>" DataField="DateCreated"></asp:BoundField>
                                                <asp:BoundField HeaderText="<%$ Resources:lbl_Entry %>" DataField="LogText"></asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <div class="rowMenu">
                                                            <ol id='menu_<%# Eval("TaskInstanceKey") %>' class="list-inline no-margin">
                                                                <li>
                                                                    <asp:HyperLink ID="hypView" runat="server" SkinID="btnGrid">View</asp:HyperLink></li>
                                                            </ol>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                </Triggers>
                            </asp:UpdatePanel>
                            <nexus:ProgressIndicator ID="upWorkMgrTask" OverlayCssClass="updating" AssociatedUpdatePanelID="updWrmTask" runat="server">
                                <ProgressTemplate>
                                </ProgressTemplate>
                            </nexus:ProgressIndicator>
                            <asp:CustomValidator ID="CstVldData" runat="server" Display="None" ErrorMessage="<%$ Resources:msg_Completed %>" ValidationGroup="AddTaskButtonGroup"></asp:CustomValidator>
                            <asp:CustomValidator ID="CstVldMTA" runat="server" Display="None" ErrorMessage="<%$ Resources:lbl_FD_MTAExistMsg %>" Enabled="false"></asp:CustomValidator>
                        </div>
                    </div>
                </div>
                <div class="card-footer">
                    <asp:LinkButton ID="btnCancel" runat="server" Text="<%$ Resources:btnCancel %> " SkinID="btnSecondary"></asp:LinkButton>
                    <asp:LinkButton ID="btnDelete" runat="server" Text="<%$ Resources:btnDelete %>" SkinID="btnSecondary"></asp:LinkButton>
                    <asp:LinkButton ID="btnNewEntry" runat="server" Text="<%$ Resources:btnNewEntry %>" OnClientClick="tb_show(null , 'WmTaskLogEntry.aspx?Mode=NewTaskLog&modal=true&KeepThis=true&TB_iframe=true&height=300&width=750' , null);return false;" SkinID="btnPrimary"></asp:LinkButton>
                    <asp:LinkButton ID="btnEdit" runat="server" Text="<%$ Resources:btnEdit %>" SkinID="btnPrimary"></asp:LinkButton>
                    <asp:LinkButton ID="btnStart" runat="server" Text="<%$ Resources:btnStart %>" SkinID="btnPrimary"></asp:LinkButton>
                    <asp:LinkButton ID="btnAssign" runat="server" Text="<%$ Resources:btnAssign %>" SkinID="btnPrimary"></asp:LinkButton>
                    <asp:LinkButton ID="btnComplete" runat="server" Text="<%$ Resources:btnComplete %>" SkinID="btnPrimary"></asp:LinkButton>
                    <asp:LinkButton ID="btnInComplete" runat="server" Text="<%$ Resources:btnInComplete %>" SkinID="btnPrimary"></asp:LinkButton>
                    <asp:LinkButton ID="btnOK" runat="server" Text="<%$ Resources:btnOK %>" ValidationGroup="AddTaskButtonGroup" SkinID="btnPrimary"></asp:LinkButton>
                </div>
            </div>
        </asp:Panel>
        <asp:ValidationSummary ID="vldAddTaskButton" DisplayMode="Bulletlist" runat="server" ValidationGroup="AddTaskButtonGroup" ShowSummary="true" HeaderText="<%$ Resources:lbl_ValidationSummary %>" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
