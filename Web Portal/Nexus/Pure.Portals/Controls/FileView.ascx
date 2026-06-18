<%@ Control Language="VB" AutoEventWireup="false" CodeFile="FileView.ascx.vb" Inherits="Controls_FileView"
    EnableViewState="true" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>

<script type="text/javascript">

    function toggleUploadControls(fileInput) {
        var hasFile = fileInput.value && fileInput.value.length > 0;
        var btn = document.getElementById('<%= btnUploadDocument.ClientID%>');
        var cat = document.getElementById('<%= drpUploadCategory.ClientID%>');
        var subCat = document.getElementById('<%= drpUploadSubCategory.ClientID%>');
        if (btn) btn.disabled = !hasFile;
        if (cat) cat.disabled = !hasFile;
        if (subCat) subCat.disabled = true;
        var msg = document.getElementById('UploadStatusMessage');
        if (msg) msg.innerHTML = '';
    }

    function onCategoryChange(catSelect) {
        var subCat = document.getElementById('<%= drpUploadSubCategory.ClientID%>');
        if (subCat) {
            subCat.disabled = (catSelect.value === '' || catSelect.selectedIndex === 0);
        }
    }

    $(document).ready(function () {
        // Dropdowns and upload button disabled until file is chosen
        var btn = document.getElementById('<%= btnUploadDocument.ClientID%>');
        var cat = document.getElementById('<%= drpUploadCategory.ClientID%>');
        var subCat = document.getElementById('<%= drpUploadSubCategory.ClientID%>');
        if (btn) btn.disabled = true;
        if (cat) cat.disabled = true;
        if (subCat) subCat.disabled = true;
    });

    //$(document).ready(function () {
    //    $("#document-filter-toggle").click(function () {
    //        $("#document-filter").show(300, function () { $("#document-filter-toggle").removeClass('docexpand'); });
    //    });
    //    $("#document-filter-title").click(function () {
    //        $("#document-filter").hide(300, function () { $("#document-filter-toggle").addClass('docexpand'); });
    //    });

    //    $("#document-upload-toggle").click(function () {
    //        $("#UploadStatusMessage").text('');
    //        $("#document-upload").show(300, function () { $("#document-upload-toggle").removeClass('docexpand'); });
    //    });
    //    $("#document-upload-title").click(function () {
    //        $("#document-upload").hide(300, function () { $("#document-upload-toggle").addClass('docexpand'); });
    //    });
    //    resizeDocManager();
    //});

    //$(window).resize(resizeDocManager);

    //function resizeDocManager() {
    //    var subHeight = 173;
    //    $("#document-browser").height($(window).height() - subHeight);
    //    $("#document-viewer").height($(window).height() - subHeight);
    //}

    function setFolder(sCurrentFolder, sFolderNum) {
        var col_array = sCurrentFolder.split("|");
        var part_num = col_array.length;
        //this should change the CurrentFolder property and repopulate the control asynchronously
        document.getElementById('<%= HidCurrentFolder.ClientId%>').value = sCurrentFolder.replace('+',' ');
        document.getElementById('<%= HidFolderNum.ClientId%>').value = sFolderNum;
        // Show/hide upload section based on whether a valid folder is selected
        var uploadDiv = document.getElementById('<%= liUpload.ClientID%>');
        if (uploadDiv) {
            if (sFolderNum && parseInt(sFolderNum) > 0) {
                uploadDiv.style.display = '';
            } else {
                uploadDiv.style.display = 'none';
            }
        }
        // Clear upload status message and reset dropdowns on folder change
        var msg = document.getElementById('UploadStatusMessage');
        if (msg) msg.innerHTML = '';
        var cat = document.getElementById('<%= drpUploadCategory.ClientID%>');
        var subCat = document.getElementById('<%= drpUploadSubCategory.ClientID%>');
        if (cat) cat.selectedIndex = 0;
        if (subCat) subCat.selectedIndex = 0;
        // if Depth is greater then one
        if (part_num > 1) {
            __doPostBack('updDocumentResults', 'RefreshDocumentList');
        }
    }

    function toggleAllCheckboxes(headerChk) {
        var grid = document.getElementById('<%= grdvDocumentResults.ClientID%>');
        if (grid) {
            var checkboxes = grid.querySelectorAll('input[type="checkbox"]');
            for (var i = 0; i < checkboxes.length; i++) {
                checkboxes[i].checked = headerChk.checked;
            }
        }
        updateDownloadButtonState();
    }

    function updateDownloadButtonState() {
        var grid = document.getElementById('<%= grdvDocumentResults.ClientID%>');
        var btn = document.getElementById('btnDownloadSelected');
        if (!grid || !btn) return;
        var rows = grid.querySelectorAll('tr');
        var hasChecked = false;
        for (var i = 1; i < rows.length; i++) {
            var chk = rows[i].querySelector('input[type="checkbox"]');
            if (chk && chk.checked) { hasChecked = true; break; }
        }
        btn.style.pointerEvents = hasChecked ? '' : 'none';
        btn.style.opacity = hasChecked ? '' : '0.65';
    }

    // Use event delegation on the document-viewer container so checkbox events
    // are captured even after UpdatePanel replaces the grid HTML on folder change
    document.addEventListener('click', function (e) {
        var el = e.target;
        if (el && el.type === 'checkbox') {
            var grid = document.getElementById('<%= grdvDocumentResults.ClientID%>');
            if (grid && grid.contains(el)) {
                setTimeout(updateDownloadButtonState, 0);
            }
        }
    });

    // Also reset button state after every UpdatePanel async postback completes
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_endRequest(function () {
        updateDownloadButtonState();
    });

    function collectSelectedDocs() {
        var grid = document.getElementById('<%= grdvDocumentResults.ClientID%>');
        var selectedIds = [];
        var selectedNames = [];
        if (grid) {
            var rows = grid.querySelectorAll('tr');
            for (var i = 1; i < rows.length; i++) {
                var chk = rows[i].querySelector('input[type="checkbox"]');
                var hdn = rows[i].querySelector('input[type="hidden"]');
                if (chk && chk.checked && hdn) {
                    selectedIds.push(hdn.value);
                    var nameHdn = rows[i].querySelector('.hdnDocName');
                    selectedNames.push(nameHdn ? nameHdn.value : 'document');
                }
            }
        }
        if (selectedIds.length === 0) {
            alert('Please select at least one document to download.');
            return false;
        }
        // Use a hidden iframe to download so the main page doesn't show loading
        var folderName = document.getElementById('<%= HidCurrentFolder.ClientId%>').value;
        if (folderName) {
            var parts = folderName.split('|');
            folderName = parts[parts.length - 1];
        }
        var url = '<%= ResolveUrl("~/secure/DownloadDocuments.aspx") %>?docNums=' + encodeURIComponent(selectedIds.join(',')) + '&docNames=' + encodeURIComponent(selectedNames.join('|')) + '&folderName=' + encodeURIComponent(folderName || '');
        var iframe = document.getElementById('downloadFrame');
        if (!iframe) {
            iframe = document.createElement('iframe');
            iframe.id = 'downloadFrame';
            iframe.name = 'downloadFrame';
            iframe.style.display = 'none';
            document.body.appendChild(iframe);
        }
        iframe.src = url;
        return false;
    }

  
</script>

<div id="Controls_FileView">
    <div id="document-viewer">
        <div id="document-upload" class="card card-secondary">
            <div class="card-heading">
                <h4>
                    <asp:Literal ID="lblHeading" runat="server" Text="<%$ Resources:lblPageHeader %>" /></h4>
            </div>
            <div class="card-body clearfix">
                <div id="liUpload" runat="server">
                    <div class="form-horizontal p-sm">
                        <div style="display:flex; flex-wrap:nowrap; align-items:center; gap:10px; width:100%;">
                            <asp:FileUpload ID="fuDocumentUpload" runat="server"
                                CssClass="form-control" style="flex:1; min-width:0;"
                                onchange="toggleUploadControls(this);" />
                            <asp:UpdatePanel runat="server" ID="updCategoryDropdowns" UpdateMode="Conditional" ChildrenAsTriggers="true" RenderMode="Inline">
                                <ContentTemplate>
                                    <span style="display:flex; flex-wrap:nowrap; align-items:center; gap:10px; flex:2; min-width:0;">
                                        <span style="white-space:nowrap; font-size:12px;">Category</span>
                                        <NexusProvider:LookupList ID="drpUploadCategory" runat="server"
                                            ListType="PMLookup"
                                            DefaultText="- - Please Select - -"
                                            CssClass="form-control form-select" style="flex:1; min-width:0;"
                                            ListCode="Document_Template_Group"
                                            DataItemValue="Key"
                                            DataItemText="Description"
                                            CausesValidation="false" />
                                        <span style="white-space:nowrap; font-size:12px;">Sub-Category</span>
                                        <NexusProvider:LookupList ID="drpUploadSubCategory" runat="server"
                                            ListType="PMLookup"
                                            DefaultText="- - Please Select - -"
                                            CssClass="form-control form-select" style="flex:1; min-width:0;"
                                            ParentLookupListID="drpUploadCategory"
                                            ParentFieldName="Document_Template_Group_Id"
                                            ListCode="Document_Template_Sub_Group"
                                            DataItemValue="Key"
                                            DataItemText="Description"
                                            CausesValidation="false" />
                                    </span>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="row">
                            <div class="col-lg-12 p-sm">
                                <asp:Button ID="btnUploadDocument" runat="server"
                                    Text="<%$ Resources:lbl_Upload %>"
                                    CssClass="btn btn-sm btn-primary"
                                    style="background-color:#009688; border-color:#009688;"
                                    OnClick="btnUploadDocument_Click" />
                                <span id="UploadStatusMessage" style="margin-left:5px;">
                                    <asp:Literal ID="lblFileUploadMessage" runat="server" />
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <iframe id="docFrame1" runat="server" width="0px" height="0px" class="hide"></iframe>
        <asp:UpdatePanel runat="server" ID="updDocumentResults" UpdateMode="Conditional"
            ChildrenAsTriggers="true">
            <ContentTemplate>
                <div id="document-list" class="card card-secondary">
                    <div class="card-heading">
                        <h4>
                            <asp:Literal ID="lblFilesHeader" runat="server" Text="<%$ Resources:lblFilesHeader %>" /></h4>
                    </div>
                    <div class="card-body clearfix">
                        <legend>
                            <asp:Label ID="lblSubHeader" runat="server" Text="<%$ Resources:lbl_DeselectedHeader %>"></asp:Label></legend>
                        <div class="form-horizontal p-sm">
                            <div class="row">
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblFilterDocName" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtFilterDocName">
                                        <asp:Literal ID="litFilterDocName" runat="server" Text="<%$ Resources:lbl_FilterDocName %>" /></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:TextBox ID="txtFilterDocName" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblFilterCategory" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="drpFilterCategory">
                                        <asp:Literal ID="litFilterCategory" runat="server" Text="<%$ Resources:lbl_FilterCategory %>" /></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <NexusProvider:LookupList ID="drpFilterCategory" runat="server"
                                            ListType="PMLookup"
                                            DefaultText="- - All - -"
                                            CssClass="form-control form-select"
                                            ListCode="Document_Template_Group"
                                            DataItemValue="Key"
                                            DataItemText="Description"
                                            CausesValidation="false" />
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblFilterSubCategory" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="drpFilterSubCategory">
                                        <asp:Literal ID="litFilterSubCategory" runat="server" Text="<%$ Resources:lbl_FilterSubCategory %>" /></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <NexusProvider:LookupList ID="drpFilterSubCategory" runat="server"
                                            ListType="PMLookup"
                                            DefaultText="- - All - -"
                                            CssClass="form-control form-select"
                                            ListCode="Document_Template_Sub_Group"
                                            DataItemValue="Key"
                                            DataItemText="Description"
                                            CausesValidation="false" />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-12 text-right p-sm">
                                    <asp:LinkButton ID="btnFilter" runat="server" Text="<%$ Resources:lbl_FilterSearch %>" SkinID="btnPrimary" OnClick="btnFilter_Click" />
                                    <asp:LinkButton ID="btnClearFilter" runat="server" Text="<%$ Resources:lbl_FilterClear %>" SkinID="btnSecondary" OnClick="btnClearFilter_Click" />
                                </div>
                            </div>
                        </div>
                        <div class="grid-card table-responsive no-margin">
                            <asp:GridView ID="grdvDocumentResults" runat="server" PageSize="10" AllowPaging="true"
                                AllowSorting="true"
                                PagerSettings-Mode="Numeric" EmptyDataText="<%$ Resources:ErrorMessage %>"
                                AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkSelectAll" runat="server" onclick="toggleAllCheckboxes(this);" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server" CssClass="doc-select-chk" />
                                            <asp:HiddenField ID="hdnDocNum" runat="server" Value='<%# Eval("DocNum") %>' />
                                            <input type="hidden" class="hdnDocName" value='<%# Server.HtmlEncode(Convert.ToString(Eval("DocDescription"))) %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:lbl_Name%>" SortExpression="DocDescription">
                                        <ItemTemplate>
                                            <div class="rowMenu">
                                                <ol id='menu_<%# Eval("DocNum") %>' class="list-inline no-margin">
                                                    <li class="no-padding">
                                                       <asp:LinkButton ID="hypDocDescription" SkinID="btnHGrid" runat="server" OnCommand="lnkDocDescription_Command" ></asp:LinkButton></li>
                                                     </li>
                                                </ol>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="CreateDate" HeaderText="<%$ Resources:lbl_CreatedBy%>" SortExpression="CreateDate" />
                                    <asp:TemplateField HeaderText="<%$ Resources:lbl_Type%>" SortExpression="DocumentType">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDocumentType" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:lbl_UploadedBy%>" SortExpression="UploadedBy">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUploadedBy" runat="server" Text='<%# Eval("UploadedBy") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:lbl_Category%>" SortExpression="Category">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCategory" runat="server" Text='<%# Eval("Category") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:lbl_SubCategory%>" SortExpression="SubCategory">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubCategory" runat="server" Text='<%# Eval("SubCategory") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <asp:HiddenField ID="HidCurrentFolder" runat="server" />
                        <asp:HiddenField ID="HidFolderNum" runat="server" />
                        <asp:HiddenField ID="HidImportFile" runat="server" />
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="grdvDocumentResults" EventName="DataBound" />
                <asp:AsyncPostBackTrigger ControlID="grdvDocumentResults" EventName="PageIndexChanging" />
                <asp:AsyncPostBackTrigger ControlID="grdvDocumentResults" EventName="RowCommand" />
                <asp:AsyncPostBackTrigger ControlID="grdvDocumentResults" EventName="RowDataBound" />
                <asp:AsyncPostBackTrigger ControlID="grdvDocumentResults" EventName="RowEditing" />
                <asp:AsyncPostBackTrigger ControlID="grdvDocumentResults" EventName="Sorting" />
            </Triggers>
        </asp:UpdatePanel>
        <div class="p-sm">
            <button type="button" id="btnDownloadSelected"
                class="btn btn-sm btn-primary waves-effect" style="background-color:#009688; border-color:#009688; pointer-events:none; opacity:0.65;"
                onclick="collectSelectedDocs();">
                <asp:Literal ID="litDownloadSelected" runat="server" Text="<%$ Resources:lblDownloadSelected %>" />
            </button>
        </div>
        <nexus:ProgressIndicator ID="upSearchFileView" OverlayCssClass="updating" AssociatedUpdatePanelID="updDocumentResults"
            runat="server">
            <ProgressTemplate>
            </ProgressTemplate>
        </nexus:ProgressIndicator>
    </div>
</div>
