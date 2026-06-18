<%@ Control Language="VB" AutoEventWireup="false" CodeFile="DocumentManager.ascx.vb"
    Inherits="Controls_DocumentManager" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register Src="~/Controls/AddTaskButton.ascx" TagPrefix="uc1" TagName="AddTaskButton" %>
<script type="text/javascript">
    //add handler for completion of a partial postback
    var nTotalFilesUploaded = 0;
    var nFilesToUpload = 0;

    $(document).ready(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(partialComplete);

        var oDragDropPanel = $("#<%= pnlUpload.ClientID%>");

        oDragDropPanel.on('dragenter', function (e) {
            e.stopPropagation();
            e.preventDefault();
            $(this).css('border', '2px solid #0B85A1');
        });
        oDragDropPanel.on('dragover', function (e) {
            e.stopPropagation();
            e.preventDefault();
        });
        oDragDropPanel.on('drop', function (e) {
            $(this).css('border', '1px solid #CCCCCC');
            e.preventDefault();
            var files = e.originalEvent.dataTransfer.files;
            //We need to send dropped files to Server
            UploadFiles(files, oDragDropPanel);
            files = null;
        });


        //This will block to drag any document to browser - may be, by mistake user will drag the document outside drag panel
        $(document).on('dragenter', function (e) {
            e.stopPropagation();
            e.preventDefault();
        });
        $(document).on('dragover', function (e) {
            e.stopPropagation();
            e.preventDefault();
            oDragDropPanel.css('border', '1px solid #CCCCCC');
        });
        $(document).on('drop', function (e) {
            e.stopPropagation();
            e.preventDefault();
        });


        //Event for category/sub-category
        $('select[id$=drpCategory]').change(function () {
            var controlPrefix = this.id.substring(0, this.id.length - 11);
            ValidatorEnable(document.getElementById(controlPrefix + 'reqSubCategory'), false);
            $('#' + controlPrefix + 'drpSubCategory').find('option').remove().end().append('<option value="">Loading......</option>').attr('disabled', 'disabled');;
            //__doPostBack(controlPrefix +  'drpCategory', 'onchange');
        });
        $('select[id$=drpSubCategory]').change(function () {
            var controlPrefix = this.id.substring(0, this.id.length - 11);
            //ValidatorEnable(document.getElementById(controlPrefix + 'reqSubCategory'), false);
            //$('#' + controlPrefix + 'drpSubCategory').find('option').remove().end().append('<option value="">Loading......</option>').attr('disabled', 'disabled'); ;
            __doPostBack(controlPrefix + 'drpCategory', 'onchange');
        });
    });

    function emailDocs() {
        //launch email dialog, passing the selected document ids in the query string
        var docIDs = "";
        $('span[class^="asp-check docID"]').each(function () {
            if ($(this).find('input:first').is(':checked')) {
                docIDs += $(this).attr('class');
            }
        });
        var key = document.getElementById('<%= hdnKey.ClientId%>').value;
        show_modal('<%= ResolveUrl("~/modal/SendEmail.aspx") %>?Docs=' + docIDs + '&key=' + key + '&PartyKey=' + <%=PartyKey %> + '&InsuranceFileKey=' + <%=InsuranceFileKey %> + '&ClaimKey=' + <%=ClaimKey %> + '&ShowEmailTemplateList=' + <%=ShowEmailTemplateList %> + '&modal=true&loc=docm&n=p', null, false);
    }

    function partialComplete() {
        //if we are a modal then rezise following an upload so that we don't introduce scrolling
        //if (document.URL.indexOf('modal=true') != -1) { self.parent.resizeDialog($('.page-container')); }
    }

    function refreshFilelist() {
        //trigger partial postback on the update panel, which will refresh contents of file table
        __doPostBack('<%= pnlDocuments.ClientID %>', 'UpdateDocs');
    }

    function setCategoryValidators(catValidatorID, subcatValidatorID, drpCategoryID, bChecked) {

        var catValidator = document.getElementById(catValidatorID)
        ValidatorEnable(catValidator, bChecked);

        var subcatValidator = document.getElementById(subcatValidatorID)
        ValidatorEnable(subcatValidator, (bChecked && document.getElementById(drpCategoryID).value != ''));
    }

    function HideValidators() {
        //Hide all validation errors  
        if (window.Page_Validators)
            for (var vI = 0; vI < Page_Validators.length; vI++) {
                var vValidator = Page_Validators[vI];
                vValidator.isvalid = true;
                ValidatorUpdateDisplay(vValidator);
            }
        //Hide all validaiton summaries  
        if (typeof (Page_ValidationSummaries) != "undefined") { //hide the validation summaries  
            for (sums = 0; sums < Page_ValidationSummaries.length; sums++) {
                summary = Page_ValidationSummaries[sums];
                summary.style.display = "none";
            }
        }
    }

    function OpenDoc(sDocPath) {
        var url = sDocPath;
        if (navigator.userAgent.indexOf("Chrome") != -1) {
             url = '<%= ResolveUrl("~/secure/FileHandler.ashx")%>?filename=' + encodeURIComponent(sDocPath);
        }
       
        window.open(url, '_newtab');
    }

    function UploadFiles(files, obj) {
        nFilesToUpload = files.length;
        for (var i = 0; i < files.length; i++) {
            var fd = new FormData();
            fd.append('file', files[i]);
            //create progress bar
            var status = new createStatusbar(obj);
            //set current file details for progressbar
            status.setFileNameSize(files[i].name, files[i].size);
            //Upload file to server and update the progress bar
            sendFileToServer(fd, status, files[i].name);
        }
    }

    function sendFileToServer(formData, status, fileName) {
        var sUrl = window.location.protocol + "//" + window.location.host + '<%=System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot")%>';
        if ('<%= HttpContext.Current.Session.IsCookieless%>' == 'True') {
            sUrl = sUrl + "(S(" + "<%=Session.SessionID.ToString()%>" + "))" + "/";
        }
        sUrl = sUrl + "services/FileUpload.svc/UploadFiles?fileName=" + fileName;
        var uploadURL = sUrl;
        var extraData = {}; //Extra Data.
        var jqXHR = $.ajax({
            xhr: function () {
                var xhrobj = $.ajaxSettings.xhr();
                if (xhrobj.upload) {
                    xhrobj.upload.addEventListener('progress', function (event) {
                        var percent = 0;
                        var position = event.loaded || event.position;
                        var total = event.total;
                        if (event.lengthComputable) {
                            percent = Math.ceil(position / total * 100);
                        }
                        //Set progress
                        status.setProgress(percent);
                    }, false);
                }
                return xhrobj;
            },
            url: uploadURL,
            type: "POST",
            contentType: false,
            processData: false,
            cache: false,
            data: formData,
            async: true,
            success: function (data) {
                status.setProgress(100);
                nTotalFilesUploaded = nTotalFilesUploaded + 1;
                if (nFilesToUpload == nTotalFilesUploaded) {
                    nFilesToUpload = 0;
                    nTotalFilesUploaded = 0;
                    status.setFinished();
                    refreshFilelist();
                }
            },
            error: function (data) {
                status.setFailed();
            }
        });
    }

    function createStatusbar(obj) {
        this.textToDisplay = "";
        this.setFileNameSize = function (name, size) {
            this.textToDisplay = "Uploading file - " + name;
            var sizeStr = "";
            var sizeKB = size / 1024;
            if (parseInt(sizeKB) > 1024) {
                var sizeMB = sizeKB / 1024;
                sizeStr = sizeMB.toFixed(2) + " MB";
            }
            else {
                sizeStr = sizeKB.toFixed(2) + " KB";
            }
            this.textToDisplay = this.textToDisplay + "(" + sizeStr + ")";
        }
        this.setProgress = function (progress) {
            obj.text(this.textToDisplay + " - " + progress + "% ");
        }
        this.setFinished = function () {
            obj.text("Drag Files Here to Upload");
        }
        this.setFailed = function () {
            obj.text(this.textToDisplay + " - " + "failed");
        }
    }

</script>

<div id="Controls_DocumentManager">
    <asp:UpdatePanel ID="pnlDocuments" runat="server" UpdateMode="conditional" EnableViewState="false">
        <ContentTemplate>
            <div class="card-body clearfix no-padding">
                <iframe id="docFrame" runat="server" width="0px" height="0px" class="hide"></iframe>
                <div class="grid-card table-responsive no-margin">
                    <asp:GridView ID="grdDocuments" runat="server" AllowPaging="True" PageSize="10" PagerSettings-Mode="Numeric" AutoGenerateColumns="False" GridLines="None" DataKeyNames="Key" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                        <Columns>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                            <asp:LinkButton ID="lnkDocument" runat="server" Text="Undefined" CausesValidation="False" CommandArgument='<%# Container.DataItemIndex%>' SkinID="btnGrid"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Select">
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="chkDocumentSelected" Text=" " CssClass="asp-check"></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Internal Only">
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="chkInternalOnly" Text=" " CssClass="asp-check"></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Category">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCategory"></asp:Label>
                                    <NexusProvider:LookupList ID="drpCategory" runat="server" ListType="PMLookup" DefaultText="- - Please Select - -" CssClass="form-control field-mandatory form-select" ListCode="Document_Template_Group" DataItemValue="Key" DataItemText="Description" CausesValidation="false"></NexusProvider:LookupList>
                                    <asp:RequiredFieldValidator runat="server" EnableClientScript="true" ControlToValidate="drpCategory" ID="reqCategory" Display="None" ErrorMessage="<%$ Resources:reqCategory %>" ValidationGroup="valDocumentManager"></asp:RequiredFieldValidator>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sub Category">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblSubCategory"></asp:Label>
                                    <NexusProvider:LookupList ID="drpSubCategory" runat="server" ListType="PMLookup" DefaultText="- - Please Select - -" CssClass="form-control field-mandatory form-select" ParentLookupListID="drpCategory" ParentFieldName="Document_Template_Group_Id" ListCode="Document_Template_Sub_Group" DataItemValue="Key" DataItemText="Description" CausesValidation="false"></NexusProvider:LookupList>
                                    <asp:RequiredFieldValidator runat="server" EnableClientScript="true" ControlToValidate="drpSubCategory" ID="reqSubCategory" Display="None" ErrorMessage="<%$ Resources:reqSubCategory %>" ValidationGroup="valDocumentManager"></asp:RequiredFieldValidator>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <uc1:AddTaskButton runat="server" ID="AddTaskButton" CallingApp="DocumentManager" Visible="false"></uc1:AddTaskButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <Nexus:ProgressIndicator ID="upDocuments" OverlayCssClass="updating" AssociatedUpdatePanelID="pnlDocuments" runat="server">
        <progresstemplate>
                        </progresstemplate>
    </Nexus:ProgressIndicator>
    <div class="card-footer no-padding m-v-sm">
        <asp:LinkButton ID="btnEmailSelected" runat="server" Text="<%$ Resources:btnEmailSelected %>" OnClientClick="emailDocs();return false;" ValidationGroup="valDocumentManager" SkinID="btnSM"></asp:LinkButton>
        <asp:LinkButton ID="btnArchiveSelected" runat="server" UseSubmitBehavior="false" Text="<%$ Resources:btnArchiveSelected %>" ValidationGroup="valDocumentManager" SkinID="btnSM"></asp:LinkButton>
    </div>
    <div id="pnlUpload" class="p-lg grey-100 text-lg text-dark-lt b-a b-dark" runat="server">
       <i>Drag Files Here to Upload</i> 
    </div>
    <asp:HiddenField runat="server" ID="hdnKey"></asp:HiddenField>
    <asp:ValidationSummary ID="ValidationSummary" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" ValidationGroup="valDocumentManager" CssClass="validation-summary"></asp:ValidationSummary>
</div>

