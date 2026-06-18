Imports System.Web.Configuration.WebConfigurationManager
Imports System.Drawing
Imports Nexus.Constants
Imports Nexus.Library
Imports Nexus.Utils
Imports CMS.Library
Imports System.Collections.Generic
Imports System.Xml.Linq
Imports Nexus
Imports System.Web.UI.Page
Imports System.IO
Imports System.Linq

Partial Class Controls_DocumentManager
    Inherits System.Web.UI.UserControl

    Private _ProductCode As String
    Dim sFolder As String = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).TempFileLocation & "\" & Guid.NewGuid.ToString & "\"
    Dim bUpdateGridRequired As Boolean = False

#Region "Properties"

    'Declare private variables with default values where appropriate
    Private _Documents As String
    Private _EnableEmail As Boolean = True
    Private _EnableArchive As Boolean = True
    Private _EnableEditDocument As Boolean = True
    Private _AutoArchiveSelected As Boolean = False
    Private _EnableUploadDocument As Boolean = True
    Private _InsuranceFileKey As Integer
    Private _InsuranceFolderKey As Integer
    Private _ClaimKey As Integer
    Private _PartyKey As Integer
    Private _Reports As Boolean = False
    Private _Branch As String = String.Empty
    Private _PreGenerated As Boolean = True
    Private _ShowEmailPopUp As Boolean = False
    Private _ShowEmailTemplateList As Integer = 0
    Private nSessionKey As Integer = 0
    ''' <summary>
    ''' Sets the Insurance File Key to which documents will be archived. If not set will pick up from cache
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property InsuranceFileKey() As Integer
        Get
            Return _InsuranceFileKey
        End Get
        Set(ByVal value As Integer)
            _InsuranceFileKey = value
        End Set
    End Property

    ''' <summary>
    ''' Sets the Insurance Folder to which documents will be archived. If not set will pick up from cache
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property InsuranceFolderKey() As Integer
        Get
            Return _InsuranceFolderKey
        End Get
        Set(ByVal value As Integer)
            _InsuranceFolderKey = value
        End Set
    End Property

    ''' <summary>
    ''' Sets the claim record to which documents will be archived. If not set will pick up from cache
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ClaimKey() As Integer
        Get
            Return _ClaimKey
        End Get
        Set(ByVal value As Integer)
            _ClaimKey = value
        End Set
    End Property

    ''' <summary>
    ''' Sets the party record to which documents will be archived. If not set will pick up from cache
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PartyKey() As Integer
        Get
            Return _PartyKey
        End Get
        Set(ByVal value As Integer)
            _PartyKey = value
        End Set
    End Property

    ''' <summary>
    ''' Accepts string containing comma separated list of documents which should be displayed
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Documents() As String
        Get
            Return _Documents
        End Get
        Set(ByVal value As String)
            _Documents = value
        End Set
    End Property

    ''' <summary>
    ''' Boolean value whether documents which are selected by default are archived on page load
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AutoArchiveSelected() As Boolean
        Get
            Return _AutoArchiveSelected
        End Get
        Set(ByVal value As Boolean)
            _AutoArchiveSelected = value
        End Set
    End Property

    ''' <summary>
    ''' Boolean value controlling availability of email functionality
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property EnableEmail() As Boolean
        Get
            Return _EnableEmail
        End Get
        Set(ByVal value As Boolean)
            _EnableEmail = value
        End Set
    End Property

    ''' <summary>
    ''' Boolean value controlling availability of archive functionality
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property EnableArchive() As Boolean
        Get
            Return _EnableArchive
        End Get
        Set(ByVal value As Boolean)
            _EnableArchive = value
        End Set
    End Property

    ''' <summary>
    ''' Boolean value controlling availability of archive functionality
    ''' User must also have relevant permissions to edit documents
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property EnableEditDocument() As Boolean
        Get
            Return _EnableEditDocument
        End Get
        Set(ByVal value As Boolean)
            _EnableEditDocument = value
        End Set
    End Property

    ''' <summary>
    ''' Boolean value controlling whether uploading documents is available
    ''' User must also have relevant permissions to upload documents
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property EnableUploadDocument() As Boolean
        Get
            Return _EnableUploadDocument
        End Get
        Set(ByVal value As Boolean)
            _EnableUploadDocument = value
        End Set
    End Property

    ''' <summary>
    ''' A Property Reports flag to identify whether called for reports only
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Reports() As Boolean
        Get
            Return _Reports
        End Get
        Set(ByVal value As Boolean)
            _Reports = value
        End Set
    End Property

    '''' <summary>
    '''' A Property Branch to store the Value of the Report Path
    '''' </summary>
    '''' <value></value>
    '''' <returns></returns>
    '''' <remarks></remarks>
    Public Property Branch() As String
        Get
            Return _Branch
        End Get
        Set(ByVal Value As String)
            _Branch = Value
        End Set
    End Property

    ''' <summary>
    ''' A Property PreGenerated flag to generate document always or not
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PreGenerated() As Boolean
        Get
            Return _PreGenerated
        End Get
        Set(ByVal value As Boolean)
            _PreGenerated = value
        End Set
    End Property


    ''' <summary>
    ''' A Property ShowEmailPopUp flag to load Email screen automatically
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ShowEmailPopUp() As Boolean
        Get
            Return _ShowEmailPopUp
        End Get
        Set(ByVal value As Boolean)
            _ShowEmailPopUp = value
        End Set
    End Property

    ''' <summary>
    ''' A Property ShowEmailTemplateList flag to show Email Template selction list
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ShowEmailTemplateList() As Integer
        Get
            Return _ShowEmailTemplateList
        End Get
        Set(ByVal value As Integer)
            _ShowEmailTemplateList = value
        End Set
    End Property

#End Region

    Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        If Not Page.IsPostBack Then
            ViewState.Add("DocumentCollection", Guid.NewGuid.ToString())
        End If

        'Udatepanel returning default 90 sec time out. This need to be increase.Can't increase locally because script manager calling from Master page. Its only for current page
        Dim _scriptMan As ScriptManager = ScriptManager.GetCurrent(Me.Page)
        _scriptMan.AsyncPostBackTimeout = 600

        If Me.Visible = True Then
            'set visibility of buttons as per properties
            btnEmailSelected.Visible = _EnableEmail
            btnArchiveSelected.Visible = _EnableArchive
            'set upload panel visible only if user has UploadDOcument permission AND EnableUpload is true
            pnlUpload.Visible = UserCanDoTask("UploadDocument") And _EnableUploadDocument

            'On initial load we must form a collection to bind to the grid
            'This will be formed by calling the GetDocumentDefaults SAM method, passing in the collection of documents specified in the DocumentsProperty
            'The collection will be stored in session so that it can be modified should a document be generated
            'if we are not in a quote / claim or if we haven't specified any documents to generate then we will create an empty collection to give us storage for any docs which are uploaded
            Dim oDocumentCollection As NexusProvider.DocumentDefaultsCollection 'holds the collection of documents which we will bind to
            Dim oQuote As NexusProvider.Quote
            Dim oParty As NexusProvider.BaseParty
            Dim sCurrentPartyCode As String = String.Empty

            If Session(CNMode) = Mode.NewClaim Or Session(CNMode) = Mode.EditClaim Or Session(CNMode) = Mode.PayClaim Or Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Or Session(CNMode) = Mode.ViewClaim Then
                oQuote = Session(CNClaimQuote)
            Else
                oQuote = Session(CNQuote)
                'fetching the current logged user so that document cache can be made userwise.
                oParty = Session.Item(CNParty)

                If oParty IsNot Nothing Then
                    Select Case True
                        Case TypeOf oParty Is NexusProvider.CorporateParty
                            With CType(oParty, NexusProvider.CorporateParty)
                                If .ClientSharedData IsNot Nothing AndAlso String.IsNullOrEmpty(.ClientSharedData.ShortName) = False Then
                                    sCurrentPartyCode = .ClientSharedData.ShortName.Trim()
                                End If
                            End With
                        Case TypeOf oParty Is NexusProvider.PersonalParty
                            With CType(oParty, NexusProvider.PersonalParty)
                                If .ClientSharedData IsNot Nothing AndAlso String.IsNullOrEmpty(.ClientSharedData.ShortName) = False Then
                                    sCurrentPartyCode = .ClientSharedData.ShortName.Trim()
                                ElseIf String.IsNullOrEmpty(.UserName) = False Then
                                    sCurrentPartyCode = .UserName.Trim()
                                End If
                            End With

                    End Select
                End If
            End If

            If (Session(CNMode) = Mode.View OrElse Session(CNMode) = Mode.ViewClaim) And UserCanDoTask("UploadDocumentOnView") <> True Then
                pnlUpload.Visible = False
                If Session(CNMode) = Mode.View Then
                    btnEmailSelected.Visible = False
                    btnArchiveSelected.Visible = False
                End If
            End If

            Dim oClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
            If oClaim IsNot Nothing Then
                _ClaimKey = oClaim.ClaimKey
                _PartyKey = oClaim.Client.PartyKey
                nSessionKey = _ClaimKey
                If oQuote IsNot Nothing Then
                    _InsuranceFileKey = oQuote.InsuranceFileKey
                    _InsuranceFolderKey = oQuote.InsuranceFolderKey
                End If
            Else
                If (_InsuranceFileKey = 0 AndAlso
                  _InsuranceFolderKey = 0 AndAlso
                   _PartyKey = 0) Then

                    'we need to get the context as nothing has been specified so that we know which section 
                    'of config to look at for the template codes, and we know where to place uploaded documents etc

                    Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)

                    'store various values as we'll need it again
                    If oQuote IsNot Nothing Then
                        _ProductCode = oQuote.ProductCode
                        _InsuranceFileKey = oQuote.InsuranceFileKey
                        _InsuranceFolderKey = oQuote.InsuranceFolderKey
                        _PartyKey = oQuote.PartyKey
                        nSessionKey = _InsuranceFileKey
                    End If
                ElseIf (_InsuranceFileKey <> 0) Then
                    nSessionKey = _InsuranceFileKey
                ElseIf (_PartyKey <> 0) Then
                    nSessionKey = _PartyKey
                ElseIf _ClaimKey <> 0 Then
                    nSessionKey = _ClaimKey
                End If
            End If

            If (oParty IsNot Nothing AndAlso oParty.Key > 0 AndAlso _PartyKey = 0) Then
                _PartyKey = oParty.Key
                If (nSessionKey = 0) Then
                    nSessionKey = _PartyKey
                End If
            End If

            If (hdnKey.Value.Length <= 0) And CType(Cache.Item("hdnKeyDocMan" & nSessionKey.ToString), String) IsNot Nothing Then
                hdnKey.Value = CType(Cache.Item("hdnKeyDocMan" & nSessionKey.ToString), String)
            End If

            Dim bIsDocManagerPostBack As Boolean = Request("__EVENTTARGET") IsNot Nothing AndAlso Request("__EVENTTARGET").Contains(grdDocuments.UniqueID)
            If (Not IsPostBack AndAlso Request.QueryString("FromPage") Is Nothing) OrElse (bIsDocManagerPostBack AndAlso Request("__EVENTARGUMENT") <> "UpdateDocs" AndAlso CType(Cache.Item(hdnKey.Value), NexusProvider.DocumentDefaultsCollection) Is Nothing) OrElse (bIsDocManagerPostBack AndAlso Request("__EVENTARGUMENT") IsNot Nothing _
                    AndAlso (Request("__EVENTARGUMENT").Contains("Page$") OrElse Request("__EVENTTARGET").Contains("lnkbtnDelete"))) Then
                If hdnKey.Value = "" Then
                    hdnKey.Value = Guid.NewGuid.ToString()
                    'we've not set the cache key yet, so generate a new key and add it to the hidden field
                    Cache.Insert("hdnKeyDocMan" & nSessionKey.ToString, hdnKey.Value, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(25))
                End If
                Session(CNFileUploadError) = Nothing
                If oQuote IsNot Nothing And Not String.IsNullOrEmpty(Documents) Then
                    If String.IsNullOrEmpty(_ProductCode) Then
                        _ProductCode = oQuote.ProductCode
                    End If
                    Dim sDocNameNotFound As String = String.Empty
                    'we need to call SAM to get the defaults for the specified documents
                    'create a list of document tempalte codes from the list of document names
                    Dim oDocuments As Config.Documents = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork) _
                        .Portals.Portal(Portal.GetPortalID()).Products.Product(_ProductCode).Documents

                    Dim lDocTemplates As New List(Of String)
                    For Each sDocName As String In Split(_Documents, ",")
                        If oDocuments.DocTemplate(sDocName) IsNot Nothing Then
                            'add the document template to the list of templates
                            lDocTemplates.Add(oDocuments.DocTemplate(sDocName).Code.ToString)
                        End If
                    Next

                    If CType(Split(_Documents, ","), Array).Length <> lDocTemplates.Count Then
                        Dim bDocNameFound As Boolean
                        For Each sDocName As String In Split(_Documents, ",")
                            bDocNameFound = False
                            For i As Integer = 0 To oDocuments.Count - 1
                                If oDocuments.DocTemplate(i).Name = sDocName Then
                                    bDocNameFound = True
                                    Exit For
                                End If
                            Next
                            If bDocNameFound = False Then
                                sDocNameNotFound = sDocNameNotFound + sDocName + ","
                            End If
                        Next
                    End If

                    'get initial defaults from SAM
                    oDocumentCollection = oWebService.GetDocumentDefaults(lDocTemplates)

                    If oDocumentCollection.Count <> lDocTemplates.Count Then
                        Dim bDocCodeFound As Boolean
                        For Each sItem As String In lDocTemplates
                            bDocCodeFound = False
                            For i As Integer = 0 To oDocumentCollection.Count - 1
                                If oDocumentCollection(i).documentTemplateCode = sItem Then
                                    bDocCodeFound = True
                                    Exit For
                                End If
                            Next
                            If bDocCodeFound = False Then
                                For i As Integer = 0 To oDocuments.Count - 1
                                    If oDocuments.DocTemplate(i).Code = sItem Then
                                        sDocNameNotFound = sDocNameNotFound + oDocuments.DocTemplate(i).Name + ","
                                        Exit For
                                    End If
                                Next

                            End If
                        Next
                    End If

                    If sDocNameNotFound <> String.Empty Then
                        'take off the trailing ,
                        sDocNameNotFound = Left(sDocNameNotFound, Len(sDocNameNotFound) - 1)
                        ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "DocsNotFoundOK", "alert('Document " & sDocNameNotFound & " cannot be found. Please contact your supervisor');", True)

                    End If

                    Cache.Insert(hdnKey.Value, oDocumentCollection, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(25))

                    'we need to give every document in the collection a file type
                    'if the user can edit AND edit is enabled then this will be "WORD" if not then "PDF"
                    Dim sFileType As String
                    If EnableEditDocument And UserCanDoTask("EditDocument") Then
                        sFileType = "WORD"
                    Else
                        sFileType = "PDF"
                    End If
                    'loop through the collection and update the entries
                    For i As Integer = 0 To oDocumentCollection.Count - 1
                        oDocumentCollection(i).FileType = sFileType 'the file types will all be the same
                        'we capture the document names, but then lose them when fetching the defaults. We must populate the document collection with the document names

                        Dim sFileExtension As String
                        'figure out the appropriate file extension. we'll display the documents with the extension that they will be generated as
                        If EnableEditDocument And UserCanDoTask("EditDocument") Then
                            'we can edit the doc so return as docx
                            sFileExtension = ".docx"
                        Else
                            'not allowed to edit doc so we will return the PDF version
                            sFileExtension = ".pdf"
                        End If

                        For iDocIndex As Integer = 0 To oDocuments.Count - 1
                            If oDocuments.DocTemplate(iDocIndex).Code = oDocumentCollection(i).documentTemplateCode Then
                                oDocumentCollection(i).DocumentName = ReplaceMergedField(oDocuments.DocTemplate(iDocIndex).FileName, oDocumentCollection(i).IsTimeStampAppended, oQuote.CoverStartDate) & sFileExtension
                                oDocumentCollection(i).Key = i
                                Exit For
                            End If
                        Next

                    Next
                    ' Pre-generate documents and build final collection with splits replacing parents
                    Dim oFinalCollection As New NexusProvider.DocumentDefaultsCollection
                    For iPreGen As Integer = 0 To oDocumentCollection.Count - 1
                        Dim oPreGenDoc As NexusProvider.DocumentDefaults = oDocumentCollection(iPreGen)
                        If oPreGenDoc.FileLocation Is Nothing Then
                            Dim sPreGenLocation As String = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).TempFileLocation & "\" & Guid.NewGuid.ToString & "\" & oPreGenDoc.DocumentName
                            Dim oPreGenDocType As NexusProvider.DocumentType
                            Select Case Split(oPreGenDoc.DocumentName, ".")(Split(oPreGenDoc.DocumentName, ".").Length - 1).ToLower
                                Case "docx"
                                    oPreGenDocType = NexusProvider.DocumentType.DOCX
                                Case "pdf"
                                    oPreGenDocType = NexusProvider.DocumentType.PDF
                                Case Else
                                    oPreGenDocType = NexusProvider.DocumentType.PDF
                            End Select
                            Dim sPreGenFile As String = oWebService.GenerateDocument(v_iPartyKey:=_PartyKey, v_iInsuranceFileKey:=_InsuranceFileKey, v_iInsuranceFolderKey:=_InsuranceFolderKey, v_sDocumentCode:=oPreGenDoc.documentTemplateCode, v_oDocumentType:=oPreGenDocType, v_sDocumentExtractionDirectory:=sPreGenLocation, v_iClaimKey:=_ClaimKey, bIsSuppressArchive:=True, v_bSkipArchiveonEdit:=True)
                            oPreGenDoc.FileLocation = sPreGenLocation

                            ' Scan for split files (files in same directory that are NOT the parent document)
                            Dim oPreGenSplitFiles As String() = Nothing
                            Dim sPreGenDir As String = Path.GetDirectoryName(sPreGenLocation)
                            If Directory.Exists(sPreGenDir) Then
                                Dim sPreGenMainFile As String = Path.GetFileName(sPreGenLocation)
                                oPreGenSplitFiles = Directory.GetFiles(sPreGenDir).Where(Function(f)
                                                                                             Dim sFileName As String = Path.GetFileName(f)
                                                                                             ' Exclude the parent document file
                                                                                             If sFileName.Equals(sPreGenMainFile, StringComparison.OrdinalIgnoreCase) Then Return False
                                                                                             ' Exclude the sPreGenFile return value
                                                                                             If Not String.IsNullOrEmpty(sPreGenFile) AndAlso f.Equals(sPreGenFile, StringComparison.OrdinalIgnoreCase) Then Return False
                                                                                             Return True
                                                                                         End Function).OrderBy(Function(f) f).ToArray()
                            End If

                            If oPreGenSplitFiles IsNot Nothing AndAlso oPreGenSplitFiles.Length > 0 Then
                                ' Splits found - add only sub-documents, skip parent
                                Dim sSplitFT As String = oPreGenDoc.FileType
                                Dim sSplitExt As String = If(sSplitFT = "WORD", ".docx", ".pdf")
                                Dim sSplitBaseName As String = If(String.IsNullOrEmpty(oPreGenDoc.DocumentName), oPreGenDoc.documentTemplateDescription, Path.GetFileNameWithoutExtension(oPreGenDoc.DocumentName))
                                Dim nSplitIdx As Integer = 1
                                For Each sSplitPath As String In oPreGenSplitFiles
                                    Dim oSplit As New NexusProvider.DocumentDefaults()
                                    oSplit.FileLocation = sSplitPath
                                    oSplit.documentTemplateCode = oPreGenDoc.documentTemplateCode
                                    oSplit.documentTemplateDescription = oPreGenDoc.documentTemplateDescription
                                    oSplit.documentGroupID = oPreGenDoc.documentGroupID
                                    oSplit.documentGroupDescription = oPreGenDoc.documentGroupDescription
                                    oSplit.documentSubGroupID = oPreGenDoc.documentSubGroupID
                                    oSplit.documentSubGroupDescription = oPreGenDoc.documentSubGroupDescription
                                    oSplit.IsTimeStampAppended = oPreGenDoc.IsTimeStampAppended
                                    oSplit.Selected = oPreGenDoc.Selected
                                    oSplit.FileType = sSplitFT
                                    Dim sSplitCode As String = Path.GetFileNameWithoutExtension(sSplitPath)
                                    If sSplitCode.StartsWith(Path.GetFileNameWithoutExtension(sPreGenLocation), StringComparison.OrdinalIgnoreCase) Then
                                        sSplitCode = sSplitCode.Substring(Path.GetFileNameWithoutExtension(sPreGenLocation).Length).TrimStart("_"c)
                                    End If
                                    oSplit.DocumentName = sSplitBaseName & "_" & sSplitCode & sSplitExt
                                    oFinalCollection.Add(oSplit)
                                    nSplitIdx += 1
                                Next
                            Else
                                ' No splits - keep parent document
                                oFinalCollection.Add(oPreGenDoc)
                            End If
                        Else
                            ' Already has file location, keep as-is
                            oFinalCollection.Add(oPreGenDoc)
                        End If
                    Next

                    ' Replace collection and reassign keys
                    oDocumentCollection = oFinalCollection
                    For iReKey As Integer = 0 To oDocumentCollection.Count - 1
                        oDocumentCollection(iReKey).Key = iReKey
                    Next

                    Cache.Insert(hdnKey.Value, oDocumentCollection, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(25))
                    Cache.Item(ViewState("DocumentCollection")) = oDocumentCollection
                Else
                    'We're not in a quote / claim so we can only hold uploaded files in the grid. 
                    'No need to call SAM just create an empty object to hold details of uploaded documnets
                    hdnKey.Value = Guid.NewGuid.ToString()
                    oDocumentCollection = New NexusProvider.DocumentDefaultsCollection
                    Cache.Insert("hdnKeyDocMan" & nSessionKey.ToString, hdnKey.Value, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(25))

                End If

                'check if the update is triggered by a category being updated, if it is we'll need to rebind
                If (Request("__EVENTTARGET") IsNot Nothing AndAlso Request("__EVENTTARGET").Contains("drpCategory")) Then
                    Cache.Insert(hdnKey.Value, oDocumentCollection, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(25))
                End If

                grdDocuments.DataSource = oDocumentCollection
                grdDocuments.DataBind()

                If AutoArchiveSelected And oDocumentCollection.Count > 0 Then
                    'call method to archive the selected documents on load of the control
                    ArchiveSelectedDocuments()
                End If
                If Not Request("__EVENTARGUMENT") = "" AndAlso Request("__EVENTARGUMENT") <> "UpdateDocs" AndAlso (Request("__EVENTARGUMENT") IsNot Nothing AndAlso Request("__EVENTARGUMENT").StartsWith("Page") = False) Then
                    FillTemplateGrid()
                End If

                Session.Remove(CNCurrentDocumentCollection)

            ElseIf Not Request("__EVENTARGUMENT") = "" AndAlso Request("__EVENTARGUMENT") <> "UpdateDocs" AndAlso Request("__EVENTARGUMENT") <> "onchange" AndAlso (Request("__EVENTARGUMENT") IsNot Nothing AndAlso Request("__EVENTARGUMENT").StartsWith("Page") = False) Then
                FillTemplateGrid()
            End If

            If (hdnKey.Value.Length <= 0) And CType(Cache.Item("hdnKeyDocMan" & nSessionKey.ToString), String) IsNot Nothing Then
                hdnKey.Value = CType(Cache.Item("hdnKeyDocMan" & nSessionKey.ToString), String)
            End If

            'check if the update is triggered by a category being updated, if it is we'll need to rebind
            Dim bCategoryUpdated As Boolean = False
            If Request("__EVENTTARGET") IsNot Nothing Then
                bCategoryUpdated = Request("__EVENTTARGET").Contains("drpCategory")
                'check if the document collection has not been initialized
                If oDocumentCollection IsNot Nothing Then
                    'check if document collection is not blank i.e, row count is not zero
                    If (oDocumentCollection.Count = 0) Then
                        'if it is zero then assign the items from cache
                        oDocumentCollection = CType(Cache.Item(hdnKey.Value), NexusProvider.DocumentDefaultsCollection)
                    Else
                        'else insert the value of document collection to cache
                        Cache.Insert(hdnKey.Value, oDocumentCollection, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(25))
                    End If
                End If

            End If

            If (Request("__EVENTARGUMENT") = "UpdateDocs" Or bCategoryUpdated) OrElse (Request("__EVENTTARGET") IsNot Nothing AndAlso Request("__EVENTTARGET").Contains("btnArchiveSelected")) Then

                oDocumentCollection = FillDataInCollection(oDocumentCollection)

                'the upload service will store the uploaded docs in session so we need to merge the 2 collections
                'get the upload collection from session, then loop through it adding each item to the main collection
                Dim oUploadedDocumentCollection As NexusProvider.DocumentDefaultsCollection = CType(Session(CNCurrentDocumentCollection), NexusProvider.DocumentDefaultsCollection)
                If oUploadedDocumentCollection IsNot Nothing Then
                    For Each oDocumentDefault As NexusProvider.DocumentDefaults In oUploadedDocumentCollection
                        oDocumentCollection.Add(oDocumentDefault)
                    Next
                End If
                'clear the session up as we don't need it any more
                If Request("__EVENTARGUMENT") = "UpdateDocs" Then
                    Session.Remove(CNCurrentDocumentCollection)
                End If

                'we need to update the document collection that is held with any updates from the controls in the gridview
                'otherwise any changes will be lost when the grid is rebound, and we have to rebind to show the uploaded documents
                'we also want to rebind when we change the category so that we can enable the sub category vaidation correctly
                updateDocCollection(oDocumentCollection)
                'update triggered by upload of documents, so we need to rebind the grid to show the uploaded docs
                grdDocuments.DataSource = oDocumentCollection
                grdDocuments.DataBind()

            End If
            If Request.Form("__EVENTTARGET") IsNot Nothing Then
                If Request.Form("__EVENTTARGET").Contains("btnArchiveSelected") AndAlso grdDocuments.Rows.Count = 0 Then
                    'update triggered by upload of documents, so we need to rebind the grid to show the uploaded docs
                    oDocumentCollection = FillDataInCollection(oDocumentCollection)
                    grdDocuments.DataSource = oDocumentCollection
                    grdDocuments.DataBind()
                    pnlDocuments.Update()
                    'on the click on document link
                ElseIf Request.Form("__EVENTTARGET").Contains("lnkDocument") AndAlso grdDocuments.Rows.Count = 0 Then
                    'update triggered by upload of documents, so we need to rebind the grid to show the uploaded docs
                    oDocumentCollection = FillDataInCollection(oDocumentCollection)
                    grdDocuments.DataSource = oDocumentCollection
                    grdDocuments.DataBind()
                    pnlDocuments.Update()
                ElseIf Request.Form("__EVENTTARGET").Contains("rblPaymentMethods") AndAlso grdDocuments.Rows.Count = 0 Then
                    'update triggered by payment Method Selection, so we need to rebind the grid to show the uploaded docs
                    oDocumentCollection = FillDataInCollection(oDocumentCollection)
                    grdDocuments.DataSource = oDocumentCollection
                    grdDocuments.DataBind()
                End If
            End If

            If Session(CNFileUploadError) IsNot Nothing Then
                Dim sFileUploadPathErrorMessage As String = If(GetLocalResourceObject("FileUploadPathErrorMessage") IsNot Nothing, GetLocalResourceObject("FileUploadPathErrorMessage"), "This file cannot be uploaded as path is not configured.")
                Dim sFileUploadExtErrorMessage As String = If(GetLocalResourceObject("FileUploadExtErrorMessage") IsNot Nothing, GetLocalResourceObject("FileUploadExtErrorMessage"), "The type of file'" & Session(CNFileUploadError) & "' being uploaded is not configured, contact a system administrator.")
                Dim sFileUploadErrorMessage As String = If(GetLocalResourceObject("FileUploadErrorMessage") IsNot Nothing, GetLocalResourceObject("FileUploadErrorMessage"), "This file cannot be uploaded.")
                If Session(CNFileUploadError).ToString().Trim = "FilehandlerException The network path was not found." Then
                    ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "alert", "alert('" & sFileUploadPathErrorMessage & "');", True)
                ElseIf Session(CNFileUploadError).ToString().IndexOf("Extension") <> -1 Then
                    Dim sExtensions As String = Replace(Session(CNFileUploadError).ToString(), "Extension", "")
                    sFileUploadExtErrorMessage = Replace(sFileUploadExtErrorMessage, "[!EXTENSION!]", sExtensions.Remove(sExtensions.LastIndexOf(",")))
                    ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "alert", "alert('" & sFileUploadExtErrorMessage & "');", True)
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "alert", "alert('" & sFileUploadErrorMessage & "');", True)
                End If
                Session(CNFileUploadError) = Nothing
                Exit Sub
            End If

            If Request.QueryString("Mode") IsNot Nothing AndAlso Request.QueryString("Mode") = "LetterWriting" Then
                pnlUpload.Visible = False
            End If


            Dim oDocumentArchiveSetting As NexusProvider.OptionTypeSetting
            oWebService = New NexusProvider.ProviderManager().Provider
            oDocumentArchiveSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 10)
            If (oDocumentArchiveSetting.OptionValue = "1") Then
                btnArchiveSelected.Text = GetLocalResourceObject("btnArchiveSelectedDME")
            ElseIf (oDocumentArchiveSetting.OptionValue = "2") Then
                btnArchiveSelected.Text = GetLocalResourceObject("btnArchiveSelected")
            End If
        End If
        If Not Page.IsPostBack Then
            If grdDocuments.AllowPaging = True AndAlso grdDocuments.PageCount > grdDocuments.PageSize Then
                grdDocuments.AllowPaging = True
            Else
                grdDocuments.AllowPaging = False
            End If
        End If

        If Not Page.IsPostBack Then
            If _ShowEmailPopUp Then
                Me.Page.ClientScript.RegisterStartupScript(Me.GetType, "emailDocs", "emailDocs();", True)
            End If
        End If
    End Sub


    Private Sub FillTemplateGrid()


        Dim oDocumentCollection As NexusProvider.DocumentDefaultsCollection
        Dim oCurrentDocumentCollection As NexusProvider.DocumentDefaultsCollection
        Dim oListRequestCode As New List(Of String)
        Dim oListRequestKey As New List(Of String)

        If ViewState("DocumentCollection") IsNot Nothing AndAlso Cache.Item(ViewState("DocumentCollection")) IsNot Nothing Then
            oDocumentCollection = CType(Cache.Item(ViewState("DocumentCollection")), NexusProvider.DocumentDefaultsCollection)
        Else
            oDocumentCollection = New NexusProvider.DocumentDefaultsCollection
        End If


        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        Dim oArchiveTOPDFDocumentOptionSetting As NexusProvider.OptionTypeSetting = Nothing

        oArchiveTOPDFDocumentOptionSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5009)

        Dim sFileType As String = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).DocumentFormat.ToLower()

        If Session(CNTemplateCode) IsNot Nothing Then
            oListRequestKey.Add(Session(CNTemplateCode))

        End If

        oCurrentDocumentCollection = oWebService.GetDocumentDefaults(oListRequestCode, "", oListRequestKey)


        'Dim iDocumentCount As Integer = 0
        If oCurrentDocumentCollection IsNot Nothing AndAlso oCurrentDocumentCollection.Count > 0 Then
            oDocumentCollection.Clear()
            For iDocumentCount = 0 To oCurrentDocumentCollection.Count - 1
                oDocumentCollection.Add(oCurrentDocumentCollection.Item(iDocumentCount))
            Next
        End If

        Dim sFileExtension As String
        'figure out the appropriate file extension. we'll display the documents with the extension that they will be generated as
        If EnableEditDocument And UserCanDoTask("EditDocument") OrElse oArchiveTOPDFDocumentOptionSetting.OptionValue <> "1" Then
            'we can edit the doc so return as docx
            sFileExtension = ".docx"
            sFileType = "WORD"
        Else
            'not allowed to edit doc so we will return the PDF version
            sFileExtension = "." & sFileType
            sFileType = sFileType
        End If

        For iDocumentCount = 0 To oDocumentCollection.Count - 1
            If oDocumentCollection(iDocumentCount).DocumentName = "" Then
                oDocumentCollection(iDocumentCount).DocumentName = oDocumentCollection(iDocumentCount).documentTemplateDescription & sFileExtension
                oDocumentCollection(iDocumentCount).FileType = sFileType
            End If

            '/****************************************/


            Dim oParty As NexusProvider.BaseParty = Session.Item(CNParty)

            Select Case True
                Case TypeOf oParty Is NexusProvider.CorporateParty
                    With CType(oParty, NexusProvider.CorporateParty)
                        _PartyKey = .Key
                    End With
                Case TypeOf oParty Is NexusProvider.PersonalParty
                    With CType(oParty, NexusProvider.PersonalParty)
                        _PartyKey = .Key
                    End With
            End Select
            If Session(CNTemplateCode) IsNot Nothing AndAlso Session(CNInsuranceFileKey) IsNot Nothing Then
                _InsuranceFileKey = Session(CNInsuranceFileKey)
                _InsuranceFolderKey = Session(CNInsuranceFolderKey)
            ElseIf Session(CNQuote) IsNot Nothing Then
                Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                _InsuranceFileKey = oQuote.InsuranceFileKey
                _InsuranceFolderKey = oQuote.InsuranceFolderKey
            End If
            If Session(CNClaim) IsNot Nothing Then
                Dim oClaim As NexusProvider.Claim = Session(CNClaim)
                _ClaimKey = oClaim.ClaimKey
            End If
        Next
        If ViewState("DocumentCollection") IsNot Nothing Then
            Cache.Item(ViewState("DocumentCollection")) = oDocumentCollection
        End If

        If oDocumentCollection IsNot Nothing AndAlso oDocumentCollection.Count > 0 AndAlso oDocumentCollection.Count - grdDocuments.Rows.Count > 1 Then
            oDocumentCollection.Remove(oDocumentCollection.Count - 1)
        End If

        If oDocumentCollection IsNot Nothing Then
            Cache.Insert(hdnKey.Value, oDocumentCollection, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(25))
            grdDocuments.DataSource = oDocumentCollection
            grdDocuments.DataBind()
            pnlDocuments.Update()
            If oDocumentCollection.Count > 1 Then
                Dim sDocumentName As String = oDocumentCollection.Item(oDocumentCollection.Count - 1).documentTemplateDescription
                Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "SetTemplateName('" + sDocumentName + "');", True)
            End If
        End If
    End Sub


    Private Function FillDataInCollection(ByVal oDocumentCollection As NexusProvider.DocumentDefaultsCollection) As NexusProvider.DocumentDefaultsCollection
        'assign the document collection with cache/initialize the cache with a new instance if the cache is blank or grid has no document (page is loading for the first time)
        oDocumentCollection = CType(Cache.Item(hdnKey.Value), NexusProvider.DocumentDefaultsCollection)
        Dim oDocumentCollectionPreserveData As NexusProvider.DocumentDefaultsCollection
        oDocumentCollectionPreserveData = New NexusProvider.DocumentDefaultsCollection
        oDocumentCollectionPreserveData = CType(Cache.Item(hdnKey.Value), NexusProvider.DocumentDefaultsCollection)
        If oDocumentCollection Is Nothing Or grdDocuments.Rows.Count = 0 Then
            oDocumentCollection = New NexusProvider.DocumentDefaultsCollection
            If oDocumentCollectionPreserveData IsNot Nothing Then
                oDocumentCollection = oDocumentCollectionPreserveData
            End If
            Cache.Insert(hdnKey.Value, oDocumentCollection, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(25))
        End If
        Return oDocumentCollection
    End Function

    ''' <summary>
    ''' to move to next page
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grdDocuments_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdDocuments.PageIndexChanging
        Dim oDocumentCollection As NexusProvider.DocumentDefaultsCollection = CType(Cache.Item(hdnKey.Value), NexusProvider.DocumentDefaultsCollection)
        grdDocuments.PageIndex = e.NewPageIndex
        updateDocCollection(oDocumentCollection)
        grdDocuments.DataSource = oDocumentCollection
        grdDocuments.DataBind()
    End Sub

    ''' <summary>
    ''' Handles click event for the document link
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>Will trigger document generation if document has not already been generated.
    ''' If user has permissions to edit document they are then redirected onto the document, otherwise file is streamed to the browser</remarks>
    Protected Sub grdDocuments_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdDocuments.RowCommand
        'find the document in collection of docs stored in session
        Dim sExtension As String = String.Empty
        If e.CommandName <> "Page" Then
            Dim oDocumentCollection As NexusProvider.DocumentDefaultsCollection = CType(Cache.Item(hdnKey.Value), NexusProvider.DocumentDefaultsCollection)
            Dim iKey As Integer
            Integer.TryParse(e.CommandArgument, iKey)
            Dim sFileLocation As String
            Dim IsSuppressArchive As Boolean = False
            Dim oDocument As NexusProvider.DocumentDefaults = oDocumentCollection(iKey)

            If oDocument.FileLocation Is Nothing Then
                'we need to generate the document before we can redirect the user onto it or stream it out
                'Dim oDocuments As Config.Documents = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork) _
                '    .Portals.Portal(Portal.GetPortalID()).Products.Product(_ProductCode).Documents()

                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim sFile As String
                'set the location using an guid as the folder inside the temp file location from config
                sFileLocation = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).TempFileLocation & "\" & Guid.NewGuid.ToString & "\" & oDocument.DocumentName

                Dim oDocumentType As NexusProvider.DocumentType
                'set the document type for the call to GenerateDocument, use the file extensions that we added when setting up the document
                Select Case Split(oDocument.DocumentName, ".")(Split(oDocument.DocumentName, ".").Length - 1).ToLower
                    Case "docx"
                        oDocumentType = NexusProvider.DocumentType.DOCX
                    Case "pdf"
                        oDocumentType = NexusProvider.DocumentType.PDF
                End Select

                If Not AutoArchiveSelected Then
                    sFile = oWebService.GenerateDocument(v_iPartyKey:=_PartyKey, v_iInsuranceFileKey:=_InsuranceFileKey, v_iInsuranceFolderKey:=_InsuranceFolderKey, v_sDocumentCode:=oDocument.documentTemplateCode, v_oDocumentType:=oDocumentType, v_sDocumentExtractionDirectory:=sFileLocation, v_iClaimKey:=_ClaimKey, bIsSuppressArchive:=True, v_bSkipArchiveonEdit:=True)
                Else
                    sFile = oWebService.GenerateDocument(_PartyKey, _InsuranceFileKey, _InsuranceFolderKey, oDocument.documentTemplateCode, oDocumentType, sFileLocation, _ClaimKey, Nothing, Nothing)
                End If

                If PreGenerated Or Reports Then
                    'save the file location back to the document collection and save it back intp cache
                    'if the doc is requested again we'll retrieve it from disk
                    oDocument.FileLocation = sFileLocation
                End If

                oDocumentCollection.Item(e.CommandArgument) = oDocument
                ' Scan the extraction directory for split/sub-documents produced by GenerateDocument
                Dim sSplitDir As String = Path.GetDirectoryName(sFileLocation)
                Dim oSplitFiles As String() = Nothing
                If Directory.Exists(sSplitDir) Then
                    Dim sMainBaseName As String = Path.GetFileNameWithoutExtension(sFileLocation)
                    oSplitFiles = Directory.GetFiles(sSplitDir).Where(Function(f)
                                                                          If f.Equals(sFileLocation, StringComparison.OrdinalIgnoreCase) Then Return False
                                                                          If Not String.IsNullOrEmpty(sFile) AndAlso f.Equals(sFile, StringComparison.OrdinalIgnoreCase) Then Return False
                                                                          Dim sFileBaseName As String = Path.GetFileNameWithoutExtension(f)
                                                                          ' Exclude files whose base name matches the main document exactly or differs only by extension
                                                                          If sFileBaseName.Equals(sMainBaseName, StringComparison.OrdinalIgnoreCase) Then Return False
                                                                          ' Exclude files that are the main document with only a numeric suffix (e.g. _1, _2) but no other distinguishing text
                                                                          If sFileBaseName.StartsWith(sMainBaseName, StringComparison.OrdinalIgnoreCase) Then
                                                                              Dim sSuffix As String = sFileBaseName.Substring(sMainBaseName.Length)
                                                                              If System.Text.RegularExpressions.Regex.IsMatch(sSuffix, "^(_\d+)?$") Then Return False
                                                                          End If
                                                                          Return True
                                                                      End Function).OrderBy(Function(f) f).ToArray()
                End If
                If oSplitFiles IsNot Nothing AndAlso oSplitFiles.Length > 0 Then
                    ' Filter out split files that already exist in the collection
                    oSplitFiles = oSplitFiles.Where(Function(f) Not oDocumentCollection.Cast(Of NexusProvider.DocumentDefaults)().Any(Function(d) Not String.IsNullOrEmpty(d.FileLocation) AndAlso d.FileLocation.Equals(f, StringComparison.OrdinalIgnoreCase))).ToArray()
                End If
                If oSplitFiles IsNot Nothing AndAlso oSplitFiles.Length > 0 Then
                    Dim nOriginalCount As Integer = oDocumentCollection.Count
                    ' Add each split file as a new document entry
                    For Each sSplitFile As String In oSplitFiles
                        Dim oSplitDoc As New NexusProvider.DocumentDefaults()
                        oSplitDoc.FileLocation = sSplitFile
                        oSplitDoc.documentTemplateCode = oDocument.documentTemplateCode
                        oSplitDoc.documentTemplateDescription = oDocument.documentTemplateDescription
                        oSplitDoc.documentGroupID = oDocument.documentGroupID
                        oSplitDoc.documentGroupDescription = oDocument.documentGroupDescription
                        oSplitDoc.documentSubGroupID = oDocument.documentSubGroupID
                        oSplitDoc.documentSubGroupDescription = oDocument.documentSubGroupDescription
                        oSplitDoc.IsTimeStampAppended = oDocument.IsTimeStampAppended
                        oSplitDoc.Selected = oDocument.Selected
                        oDocumentCollection.Add(oSplitDoc)
                    Next
                    ' Apply file type, document name with merge fields, and _N suffix to split docs
                    Dim sSplitFileType As String
                    Dim sSplitFileExtension As String
                    If EnableEditDocument And UserCanDoTask("EditDocument") Then
                        sSplitFileType = "WORD"
                        sSplitFileExtension = ".docx"
                    Else
                        sSplitFileType = "PDF"
                        sSplitFileExtension = ".pdf"
                    End If
                    Dim oSplitQuote As NexusProvider.Quote = Nothing
                    If Session(CNMode) = Mode.NewClaim Or Session(CNMode) = Mode.EditClaim Or Session(CNMode) = Mode.PayClaim Or Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Or Session(CNMode) = Mode.ViewClaim Then
                        oSplitQuote = Session(CNClaimQuote)
                    Else
                        oSplitQuote = Session(CNQuote)
                    End If
                    ' Resolve template metadata once before the loop
                    Dim sSplitTemplateFileName As String = Nothing
                    If oSplitQuote IsNot Nothing AndAlso Not String.IsNullOrEmpty(oDocument.documentTemplateCode) Then
                        Dim oSplitDocuments As Config.Documents = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork) _
                            .Portals.Portal(Portal.GetPortalID()).Products.Product(oSplitQuote.ProductCode).Documents
                        For iDocIdx As Integer = 0 To oSplitDocuments.Count - 1
                            If oSplitDocuments.DocTemplate(iDocIdx).Code = oDocument.documentTemplateCode Then
                                sSplitTemplateFileName = oSplitDocuments.DocTemplate(iDocIdx).FileName
                                Exit For
                            End If
                        Next
                    End If
                    Dim nSplitIndex As Integer = 1
                    For i As Integer = nOriginalCount To oDocumentCollection.Count - 1
                        If String.IsNullOrEmpty(oDocumentCollection(i).FileType) Then
                            oDocumentCollection(i).FileType = sSplitFileType
                        End If
                        Dim sBaseName As String = String.Empty
                        If sSplitTemplateFileName IsNot Nothing Then
                            sBaseName = ReplaceMergedField(sSplitTemplateFileName, oDocumentCollection(i).IsTimeStampAppended, oSplitQuote.CoverStartDate)
                        End If
                        If String.IsNullOrEmpty(sBaseName) Then
                            sBaseName = If(String.IsNullOrEmpty(oDocumentCollection(i).documentTemplateDescription), "SplitDocument", oDocumentCollection(i).documentTemplateDescription)
                        End If
                        oDocumentCollection(i).DocumentName = sBaseName & "_" & nSplitIndex.ToString() & sSplitFileExtension
                        nSplitIndex += 1
                    Next
                End If
                Cache.Insert(hdnKey.Value, oDocumentCollection, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(25))
                grdDocuments.DataSource = oDocumentCollection
                grdDocuments.DataBind()
                pnlDocuments.Update()
            Else
                sFileLocation = oDocument.FileLocation
            End If
            Dim sRole As String = AppSettings("ExternalUserGroup")
            sExtension = Path.GetExtension(oDocument.FileLocation)
            If String.IsNullOrEmpty(sRole) = False Then
                If UserIsInRoles(sRole) = False AndAlso EnableEditDocument AndAlso UserCanDoTask("EditDocument") Then
                    If CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).EnableTXTextControl _
                        AndAlso Not String.IsNullOrEmpty(sExtension) AndAlso sExtension.ToUpper = ".DOCX" Then
                        Dim sURL As String
                        Dim sFileName As String
                        sFileName = Path.GetFileName(oDocument.FileLocation)
                        If HttpContext.Current.Session.IsCookieless Then
                            sURL = AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/OpenTXTextControl.aspx?DocPath=" & Server.UrlEncode(oDocument.FileLocation) & "&Pagename=" & sFileName & "&bSupportRiskLevel=True&modal=true&KeepThis=true&TB_iframe=true&height=500&width=700"
                        Else
                            sURL = AppSettings("WebRoot") & "Modal/OpenTXTextControl.aspx?DocPath=" & Server.UrlEncode(oDocument.FileLocation) & "&Pagename=" & sFileName & "&bSupportRiskLevel=True&modal=true&KeepThis=true&TB_iframe=true&height=500&width=700"
                        End If
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "tb_show",
                                  "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){tb_show( null,'" & sURL & "' , null);});</script>", False)
                    Else
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "EditDocument",
                                          "OpenDoc('file:///" & (oDocument.FileLocation.Replace("\\", "\").Replace("/", "\")).Replace("\", "\\") & "');", True)
                    End If
                Else
                    oDocument.FileLocation = sFileLocation
                    Session(CNDocumentToDownload) = oDocument
                    docFrame.Attributes.Add("src", "~/Secure/Download.aspx")
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UnblockUI", "$.unblockUI();", True)
                End If
            Else
                If EnableEditDocument And UserCanDoTask("EditDocument") Then
                    'we should redirect the user onto it so that they can edit and save back
                    'set the content type so that the xml doc is opened in word
                    'Response.Redirect("file:///" & sFileLocation)
                    If CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).EnableTXTextControl _
                        AndAlso Not String.IsNullOrEmpty(sExtension) AndAlso sExtension.ToUpper = ".DOCX" Then
                        Dim sURL As String
                        Dim sFileName As String
                        sFileName = Path.GetFileName(oDocument.FileLocation)
                        If HttpContext.Current.Session.IsCookieless Then
                            sURL = AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/OpenTXTextControl.aspx?DocPath=" & Server.UrlEncode(oDocument.FileLocation) & "&Pagename=" & sFileName & "&bSupportRiskLevel=True&modal=true&KeepThis=true&TB_iframe=true&height=500&width=700"
                        Else
                            sURL = AppSettings("WebRoot") & "Modal/OpenTXTextControl.aspx?DocPath=" & Server.UrlEncode(oDocument.FileLocation) & "&Pagename=" & sFileName & "&bSupportRiskLevel=True&modal=true&KeepThis=true&TB_iframe=true&height=500&width=700"
                        End If
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "tb_show",
                                  "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){tb_show( null,'" & sURL & "' , null);});</script>", False)
                    Else
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "EditDocument",
                                           "OpenDoc('file:///" & (oDocument.FileLocation.Replace("\\", "\").Replace("/", "\")).Replace("\", "\\") & "');", True)
                    End If
                Else
                    oDocument.FileLocation = sFileLocation
                    Session(CNDocumentToDownload) = oDocument
                    docFrame.Attributes.Add("src", "~/Secure/Download.aspx")
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UnblockUI", "$.unblockUI();", True)
                End If
            End If
        End If
        If grdDocuments.AllowPaging AndAlso grdDocuments.PageCount > grdDocuments.PageSize Then
            grdDocuments.AllowPaging = True
        Else
            grdDocuments.AllowPaging = False
        End If
    End Sub

    Protected Shadows Sub grdDocuments_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdDocuments.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim oDocumentCollection As NexusProvider.DocumentDefaultsCollection = CType(Cache.Item(hdnKey.Value), NexusProvider.DocumentDefaultsCollection)


            If Session(CNTemplateCode) IsNot Nothing AndAlso ViewState("DocumentCollection") IsNot Nothing AndAlso Cache.Item(ViewState("DocumentCollection")) IsNot Nothing Then
                oDocumentCollection = CType(Cache.Item(ViewState("DocumentCollection")), NexusProvider.DocumentDefaultsCollection)
            End If
            'set the link button in the first column to the name of the document
            Dim lnkDocument As LinkButton = CType(e.Row.FindControl("lnkDocument"), LinkButton)
            If lnkDocument IsNot Nothing Then
                lnkDocument.Text = oDocumentCollection(e.Row.DataItemIndex).DocumentName

                Dim oFileTypes As Config.FileTypes = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork) _
                .Portals.Portal(Portal.GetPortalID()).FileTypes
                'set the cssclass according to config
                If Not String.IsNullOrEmpty(oDocumentCollection(e.Row.DataItemIndex).FileType) AndAlso oFileTypes.FileType(oDocumentCollection(e.Row.DataItemIndex).FileType) IsNot Nothing Then
                    lnkDocument.CssClass = oFileTypes.FileType(oDocumentCollection(e.Row.DataItemIndex).FileType).CssClass
                End If
            End If

            'set the category label text and visibility
            Dim lblCategory As Label = CType(e.Row.FindControl("lblCategory"), Label)
            If lblCategory IsNot Nothing Then
                lblCategory.Text = oDocumentCollection(e.Row.DataItemIndex).documentGroupDescription
                'set visibile only if file is not an upload
                lblCategory.Visible = Not oDocumentCollection(e.Row.DataItemIndex).IsUpload
            End If

            'set the sub category label text and visibility
            Dim lblSubCategory As Label = CType(e.Row.FindControl("lblSubCategory"), Label)
            If lblSubCategory IsNot Nothing Then
                lblSubCategory.Text = oDocumentCollection(e.Row.DataItemIndex).documentSubGroupDescription
                'set visibile only if file is not an upload
                lblSubCategory.Visible = Not oDocumentCollection(e.Row.DataItemIndex).IsUpload
            End If

            'check or uncheck the internal only checkbox
            Dim chkInternalOnly As CheckBox = CType(e.Row.FindControl("chkInternalOnly"), CheckBox)
            If chkInternalOnly IsNot Nothing Then
                chkInternalOnly.Checked = oDocumentCollection(e.Row.DataItemIndex).InternalOnly
            End If

            Dim bSelected As Boolean = False 'we'll use this to determine whether to enable the validators 

            'check or uncheck the selected checkbox
            Dim chkDocumentSelected As CheckBox = CType(e.Row.FindControl("chkDocumentSelected"), CheckBox)
            If chkDocumentSelected IsNot Nothing Then
                chkDocumentSelected.Checked = oDocumentCollection(e.Row.DataItemIndex).Selected
                bSelected = oDocumentCollection(e.Row.DataItemIndex).Selected
                'add a class name containing the document id so that we can get this client side
                chkDocumentSelected.CssClass = "asp-check docID" & e.Row.DataItemIndex.ToString
            End If

            Dim bEnableSubCatValidation As Boolean = False

            'set the categories drop downs visibility to the value of "IsUpload" as they should be visible only for uploaded files
            Dim drpCategory As NexusProvider.LookupList = CType(e.Row.FindControl("drpCategory"), NexusProvider.LookupList)
            Dim drpCategoryHasValue As Boolean = False 'this will be used to determine whether to enable the sub category validator later on
            If drpCategory IsNot Nothing Then
                drpCategory.Visible = oDocumentCollection(e.Row.DataItemIndex).IsUpload
                'we only want to enable validation on the sub category drop down if the category dropdown is populate
                bEnableSubCatValidation = Not String.IsNullOrEmpty(oDocumentCollection(e.Row.DataItemIndex).documentGroupID)
                drpCategoryHasValue = String.IsNullOrEmpty(oDocumentCollection(e.Row.DataItemIndex).documentGroupID) 'determine whether a value has been set on the category drop down
                drpCategory.Value = oDocumentCollection(e.Row.DataItemIndex).documentGroupID
            End If

            Dim drpSubCategory As NexusProvider.LookupList = CType(e.Row.FindControl("drpSubCategory"), NexusProvider.LookupList)
            If drpCategory IsNot Nothing Then
                drpSubCategory.Visible = oDocumentCollection(e.Row.DataItemIndex).IsUpload
                drpSubCategory.Value = oDocumentCollection(e.Row.DataItemIndex).documentSubGroupID
            End If

            'if the document is an upload then we need to set the error message on the validators for that row so that the message includes the name of the document
            'if the document is not an upload then we need to disable the validators
            Dim reqCategory As RequiredFieldValidator = CType(e.Row.FindControl("reqCategory"), RequiredFieldValidator)

            If reqCategory IsNot Nothing Then
                reqCategory.ErrorMessage = Replace(reqCategory.ErrorMessage, "[!DOCUMENT_NAME!]", oDocumentCollection(e.Row.DataItemIndex).DocumentName)
                reqCategory.Enabled = oDocumentCollection(e.Row.DataItemIndex).IsUpload And bSelected 'only enable validator if it is an upload and it is selected
                reqCategory.Visible = oDocumentCollection(e.Row.DataItemIndex).IsUpload
            End If

            Dim reqSubCategory As RequiredFieldValidator = CType(e.Row.FindControl("reqSubCategory"), RequiredFieldValidator)
            If reqSubCategory IsNot Nothing Then
                reqSubCategory.ErrorMessage = Replace(reqSubCategory.ErrorMessage, "[!DOCUMENT_NAME!]", oDocumentCollection(e.Row.DataItemIndex).DocumentName)
                reqSubCategory.Enabled = oDocumentCollection(e.Row.DataItemIndex).IsUpload And bSelected 'only enable validator if it is an upload and it is selected
                'reqSubCategory.Enabled = oDocumentCollection(e.Row.DataItemIndex).IsUpload And bSelected And drpCategoryHasValue  'only enable validator if it is an upload and it is selected and the category has been selected
                reqSubCategory.Visible = oDocumentCollection(e.Row.DataItemIndex).IsUpload
            End If

            If chkDocumentSelected IsNot Nothing And reqSubCategory IsNot Nothing And reqCategory IsNot Nothing And oDocumentCollection(e.Row.DataItemIndex).IsUpload Then
                'add client script to enable / disable the validators when the row is selected
                chkDocumentSelected.Attributes.Add("onClick", "setCategoryValidators('" & reqCategory.ClientID & "' , '" & reqSubCategory.ClientID & "' , '" & drpCategory.ClientID & "' , this.checked );")

            End If
            If Request.QueryString("Mode") IsNot Nothing AndAlso Request.QueryString("Mode") = "LetterWriting" Then
                e.Row.Cells("2").Visible = False
                e.Row.Cells("3").Visible = False
                e.Row.Cells("4").Visible = False
                grdDocuments.HeaderRow.Cells(2).Visible = False
                grdDocuments.HeaderRow.Cells(3).Visible = False
                grdDocuments.HeaderRow.Cells(4).Visible = False
            End If

            'set the AddTaskButton visibility
            Dim AddTaskButton As AddTaskButtonCntrl = CType(e.Row.FindControl("AddTaskButton"), AddTaskButtonCntrl)
            If oDocumentCollection(e.Row.DataItemIndex).IsExternal = True Then
                AddTaskButton.Visible = True
            End If

        End If
    End Sub

    ''' <summary>
    ''' Handles the click event of the Archive Selected button
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnArchiveSelected_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnArchiveSelected.Click
        'add a new job which will archive the seleted documents
        'If Page.IsValid Then 'this particular line was added against defect #2068 to stop Archiving of documents when no Category or Sub-Categories are selected
        ArchiveSelectedDocuments()
        'End If
    End Sub

    ''' <summary>
    ''' Update the document collection from the gridview row passed in
    ''' </summary>
    ''' <param name="oDocumentCollection"></param>
    ''' <remarks></remarks>
    Private Sub updateDocCollection(ByRef oDocumentCollection As NexusProvider.DocumentDefaultsCollection)
        For Each oRow As GridViewRow In grdDocuments.Rows
            If oRow.RowType = DataControlRowType.DataRow Then
                'read the value of the internal only checkbox
                Dim chkDocumentSelected As CheckBox = CType(oRow.FindControl("chkDocumentSelected"), CheckBox)
                If chkDocumentSelected IsNot Nothing Then
                    oDocumentCollection(oRow.DataItemIndex).Selected = chkDocumentSelected.Checked
                End If

                'read the value of the internal only checkbox
                Dim chkInternalOnly As CheckBox = CType(oRow.FindControl("chkInternalOnly"), CheckBox)
                If chkInternalOnly IsNot Nothing Then
                    oDocumentCollection(oRow.DataItemIndex).InternalOnly = chkInternalOnly.Checked
                End If

                'only set the values for category and sub category if it is an uploaded document
                If oDocumentCollection(oRow.DataItemIndex).IsUpload Then
                    'we need to get the selected values for category and sub category, these will be defaulted if it's a generated document
                    Dim drpCategory As NexusProvider.LookupList = CType(oRow.FindControl("drpCategory"), NexusProvider.LookupList)
                    If drpCategory IsNot Nothing Then
                        If Not String.IsNullOrEmpty(drpCategory.Value) Then
                            oDocumentCollection(oRow.DataItemIndex).documentGroupID = drpCategory.Value
                            oDocumentCollection(oRow.DataItemIndex).documentGroupDescription = drpCategory.Text
                        End If
                    End If

                    Dim drpSubCategory As NexusProvider.LookupList = CType(oRow.FindControl("drpSubCategory"), NexusProvider.LookupList)
                    If drpSubCategory IsNot Nothing Then
                        If Not String.IsNullOrEmpty(drpSubCategory.Value) Then
                            oDocumentCollection(oRow.DataItemIndex).documentSubGroupID = drpSubCategory.Value
                            oDocumentCollection(oRow.DataItemIndex).documentSubGroupDescription = drpSubCategory.Text
                        End If
                    End If
                End If
            End If
        Next
    End Sub

    ''' <summary>
    ''' Call SAM to queue selected documents for archive
    ''' </summary>
    ''' <remarks>This may be called either in response to the archive button click, or else on load if AutoArchiveSelected is true</remarks>
    Private Sub ArchiveSelectedDocuments()

        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

        If _PartyKey = 0 Then
            'we have a policy or claim specified but the background job wants us to pass in the party key too, so we need to look it up
            If _ClaimKey <> 0 Then
                'try to get the party key from the claim
                'arch issue 268
                Dim oClaim As NexusProvider.ClaimDetails = GetClaimDetailsCall(_ClaimKey)
                'claim details don't include party info so get the quote associated with the claim
                Dim oQuote As NexusProvider.Quote = oWebService.GetHeaderAndSummariesByKey(oClaim.InsuranceFileKey)
                PartyKey = oQuote.PartyKey
            Else
                'no claim specified, see if we can get it from the file key
                If _InsuranceFileKey <> 0 Then
                    'get the party key from the insurance file
                    Dim oQuote As NexusProvider.Quote = oWebService.GetHeaderAndSummariesByKey(_InsuranceFileKey)
                    PartyKey = Trim(oQuote.PartyKey)
                End If
            End If

        End If

        If Session(CNTemplateCode) IsNot Nothing Then


            Dim oParty As NexusProvider.BaseParty = Session.Item(CNParty)

            Select Case True
                Case TypeOf oParty Is NexusProvider.CorporateParty
                    With CType(oParty, NexusProvider.CorporateParty)
                        _PartyKey = .Key
                    End With
                Case TypeOf oParty Is NexusProvider.PersonalParty
                    With CType(oParty, NexusProvider.PersonalParty)
                        _PartyKey = .Key
                    End With
            End Select
            If Session(CNTemplateCode) IsNot Nothing AndAlso Session(CNInsuranceFileKey) IsNot Nothing Then
                _InsuranceFileKey = Session(CNInsuranceFileKey)
                _InsuranceFolderKey = Session(CNInsuranceFolderKey)
            ElseIf Session(CNQuote) IsNot Nothing Then
                Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                _InsuranceFileKey = oQuote.InsuranceFileKey
                _InsuranceFolderKey = oQuote.InsuranceFolderKey
            Else
                _InsuranceFileKey = 0
            End If
            If Session(CNClaim) IsNot Nothing Then
                Dim oClaim As NexusProvider.Claim = Session(CNClaim)
                _ClaimKey = oClaim.ClaimKey
            End If
            If Session(CNQuote) IsNot Nothing Then
                Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                If oParty.Key <> oQuote.PartyKey Then
                    _PartyKey = oQuote.PartyKey
                End If
            End If

        End If

        'set up the initial XML for the job

        Dim xlJob As XElement =
           <BACKGROUND_JOB>
               <JOB jobtype="DOCUPACK">
                   <PARAMETERS>
                       <PARAMETER name="destination" value="archive"/>
                       <PARAMETER name="archive" value="true"/>
                       <PARAMETER name="PartyCnt" value=<%= _PartyKey %>/>
                       <PARAMETER name="ClaimID" value=<%= _ClaimKey %>/>
                       <PARAMETER name="InsuranceFileCnt" value=<%= _InsuranceFileKey %>/>
                   </PARAMETERS>
                   <ITEMS>

                   </ITEMS>
               </JOB>
           </BACKGROUND_JOB>

        Dim oDocumentCollection As NexusProvider.DocumentDefaultsCollection = CType(Cache.Item(hdnKey.Value), NexusProvider.DocumentDefaultsCollection)

        If oDocumentCollection Is Nothing AndAlso Cache.Item(ViewState("DocumentCollection")) IsNot Nothing Then
            oDocumentCollection = CType(Cache.Item(ViewState("DocumentCollection")), NexusProvider.DocumentDefaultsCollection)
        End If
        updateDocCollection(oDocumentCollection)
        'update the collection with the selection made in the grid (selected / internal only / categories), and add an item to the request to CreateBackgroundJob

        Dim reqCategory As New RequiredFieldValidator
        Dim drpCategory As New NexusProvider.LookupList
        Dim drpSubCategory As New NexusProvider.LookupList
        Dim reqSubCategory As New RequiredFieldValidator
        Dim reqdFlg As Boolean = False

        Dim bAddedFirst As Boolean
        For Each oRow As GridViewRow In grdDocuments.Rows
            If oRow.RowType = DataControlRowType.DataRow Then
                'If the item is selected then add an element to the xml 
                'check or uncheck the selected checkbox
                Dim chkDocumentSelected As CheckBox = CType(oRow.FindControl("chkDocumentSelected"), CheckBox)

                reqCategory = CType(oRow.FindControl("reqCategory"), RequiredFieldValidator)
                drpCategory = CType(oRow.FindControl("drpCategory"), NexusProvider.LookupList)
                reqSubCategory = CType(oRow.FindControl("reqSubCategory"), RequiredFieldValidator)
                drpSubCategory = CType(oRow.FindControl("drpSubCategory"), NexusProvider.LookupList)

                If chkDocumentSelected IsNot Nothing Then
                    If chkDocumentSelected.Checked Then
                        'And drpCategory IsNot Nothing Then
                        reqCategory.Enabled = True

                        If (drpCategory.Text = "" Or drpSubCategory.Text = "") AndAlso oDocumentCollection(oRow.DataItemIndex).IsUpload AndAlso Session(CNTemplateCode) Is Nothing Then
                            chkDocumentSelected.Attributes.Add("OnCheckedChanged", "setCategoryValidators('" & reqCategory.ClientID & "' , '" & reqSubCategory.ClientID & "' , '" & drpCategory.ClientID & "' , '" & chkDocumentSelected.Checked & "');")
                            reqCategory.Enabled = True
                            reqCategory.Visible = True
                            reqCategory.Validate()

                        Else
                            Dim sOutputFormat As String = String.Empty
                            If Not String.IsNullOrEmpty(oDocumentCollection(oRow.DataItemIndex).FileLocation) Then
                                'get the file extension so that we can use this as the OutputFormat in the xml
                                Dim sFileLocation As String = oDocumentCollection(oRow.DataItemIndex).FileLocation
                                sOutputFormat = Right(sFileLocation, Len(sFileLocation) - sFileLocation.LastIndexOf(".") - 1).ToUpper
                            End If
                            'set up the item to add to the main job xml
                            'first check if this is the first item to be added
                            If Not bAddedFirst Then
                                bAddedFirst = True
                                'add parameter elements to the parameters section
                                If Not String.IsNullOrEmpty(oDocumentCollection(oRow.DataItemIndex).FileLocation) Then
                                    'uploaded or previously generated document so specify the path to the file
                                    Dim xlPath As XElement = <PARAMETER name="Path" value=<%= oDocumentCollection(oRow.DataItemIndex).FileLocation %>/>
                                    xlJob.Element("JOB").Element("PARAMETERS").Add(xlPath)
                                    'we need to specify format
                                    xlJob.Element("JOB").Element("PARAMETERS").Add(New XElement(<PARAMETER name="OutputFormat" value=<%= sOutputFormat %>/>))
                                    'documents to generate so specify the document template code
                                    Dim xlCode As XElement = <PARAMETER name="code" value=<%= oDocumentCollection(oRow.DataItemIndex).documentTemplateCode %>/>
                                    xlJob.Element("JOB").Element("PARAMETERS").Add(xlCode)

                                Else
                                    'documents to generate so specify the document template code
                                    Dim xlCode As XElement = <PARAMETER name="code" value=<%= oDocumentCollection(oRow.DataItemIndex).documentTemplateCode %>/>
                                    xlJob.Element("JOB").Element("PARAMETERS").Add(xlCode)
                                End If

                                Dim xlInternalonly As XElement = <PARAMETER name="Internalonly" value=<%= oDocumentCollection(oRow.DataItemIndex).InternalOnly %>/>
                                xlJob.Element("JOB").Element("PARAMETERS").Add(xlInternalonly)
                                Dim xlDocumentTemplateGroupID As XElement = <PARAMETER name="DocumentTemplateGroupID" value=<%= oDocumentCollection(oRow.DataItemIndex).documentGroupID %>/>
                                xlJob.Element("JOB").Element("PARAMETERS").Add(xlDocumentTemplateGroupID)
                                Dim xlDocumentTemplateSubGroupID As XElement = <PARAMETER name="DocumentTemplateSubGroupID" value=<%= oDocumentCollection(oRow.DataItemIndex).documentSubGroupID %>/>
                                xlJob.Element("JOB").Element("PARAMETERS").Add(xlDocumentTemplateSubGroupID)
                                'when called from Reports Link
                                If Reports = True Then
                                    Dim sDesPath As String
                                    If String.IsNullOrEmpty(Branch) Then
                                        sDesPath = "General/Reports/" & Replace(Date.Today, "/", "-") & "/" & IO.Path.GetFileName(oDocumentCollection(oRow.DataItemIndex).FileLocation)
                                    Else
                                        sDesPath = Branch & "/General/Reports/" & Replace(Date.Today, "/", "-") & "/" & ReplaceSplCharacters(IO.Path.GetFileName(oDocumentCollection(oRow.DataItemIndex).FileLocation))
                                    End If
                                    Dim xlDestinationFileName As XElement = <PARAMETER name="DestinationFilename" value=<%= ReplaceSplCharacters(sDesPath) %>/>
                                    xlJob.Element("JOB").Element("PARAMETERS").Add(xlDestinationFileName)

                                    Dim xIsTimeStampAppended As XElement = <PARAMETER name="IsTimeStampAppended" value=<%= oDocumentCollection(oRow.DataItemIndex).IsTimeStampAppended %>/>
                                    xlJob.Element("JOB").Element("PARAMETERS").Add(xIsTimeStampAppended)

                                End If
                                'if we are specifying a document to generate, or a document which has been generated and editted
                                'we then need to specify a format in which to archive it, either docx or pdf depending on the setting in web.cofig
                                If Not oDocumentCollection(oRow.DataItemIndex).IsUpload Then
                                    'get the file type from config
                                    Dim sFileType As String = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).DocumentFormat.ToLower()
                                    'we need to pass in the file name, which may change according to file type (e.g. quote.docx may archive as quote.pdf)
                                    Dim sFileName As String = oDocumentCollection(oRow.DataItemIndex).DocumentName
                                    sFileName = Left(sFileName, sFileName.LastIndexOf(".")) & "." & sFileType.ToLower

                                    Dim xlDocumentFormat As XElement = <PARAMETER name="OutputFormat" value=<%= sFileType.ToUpper %>/>
                                    xlJob.Element("JOB").Element("PARAMETERS").Add(xlDocumentFormat)
                                    Dim xlDestinationFilename As XElement = <PARAMETER name="DestinationFilename" value=<%= ReplaceSplCharacters(sFileName) %>/>
                                    xlJob.Element("JOB").Element("PARAMETERS").Add(xlDestinationFilename)
                                    Dim xIsTimeStampAppended As XElement = <PARAMETER name="IsTimeStampAppended" value=<%= oDocumentCollection(oRow.DataItemIndex).IsTimeStampAppended %>/>
                                    xlJob.Element("JOB").Element("PARAMETERS").Add(xIsTimeStampAppended)
                                End If

                            Else
                                'add a new item to the items section
                                Dim xlItem As XElement
                                If Not String.IsNullOrEmpty(oDocumentCollection(oRow.DataItemIndex).FileLocation) Then
                                    'uploaded or previously generated document so specify the path to the file
                                    xlItem = <ITEM path=<%= oDocumentCollection(oRow.DataItemIndex).FileLocation %>/>
                                Else
                                    'documents to generate so specify the document template code
                                    xlItem = <ITEM code=<%= oDocumentCollection(oRow.DataItemIndex).documentTemplateCode %>/>
                                End If

                                'add parameters to the item
                                Dim xlParameters As XElement = New XElement("PARAMETERS")

                                If Not String.IsNullOrEmpty(oDocumentCollection(oRow.DataItemIndex).FileLocation) Then
                                    'we need to specify format
                                    xlParameters.Add(New XElement(<PARAMETER name="OutputFormat" value=<%= sOutputFormat %>/>))
                                End If

                                Dim xlInternalonly As XElement = <PARAMETER name="Internalonly" value=<%= oDocumentCollection(oRow.DataItemIndex).InternalOnly %>/>
                                xlParameters.Add(xlInternalonly)
                                Dim xlDocumentTemplateGroupID As XElement = <PARAMETER name="DocumentTemplateGroupID" value=<%= oDocumentCollection(oRow.DataItemIndex).documentGroupID %>/>
                                xlParameters.Add(xlDocumentTemplateGroupID)
                                Dim xlDocumentTemplateSubGroupID As XElement = <PARAMETER name="DocumentTemplateSubGroupID" value=<%= oDocumentCollection(oRow.DataItemIndex).documentSubGroupID %>/>
                                xlParameters.Add(xlDocumentTemplateSubGroupID)
                                Dim xlArchive As XElement = <PARAMETER name="archive" value="true"/>
                                xlParameters.Add(xlArchive)
                                'if we are specifying a document to generate, or a document which has been generated and editted
                                'we then need to specify a format in which to archive it, either docx or pdf depending on the setting in web.cofig
                                If Not oDocumentCollection(oRow.DataItemIndex).IsUpload And Reports = False Then
                                    'get the file type from config
                                    Dim sFileType As String = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).DocumentFormat.ToLower()
                                    'we need to pass in the file name, which may change according to file type (e.g. quote.docx may archive as quote.pdf)
                                    Dim sFileName As String = oDocumentCollection(oRow.DataItemIndex).DocumentName
                                    sFileName = Left(sFileName, sFileName.LastIndexOf(".")) & "." & sFileType.ToLower
                                    'add output format and file name params
                                    Dim xlDocumentFormat As XElement = <PARAMETER name="OutputFormat" value=<%= sFileType.ToUpper %>/>
                                    xlParameters.Add(xlDocumentFormat)
                                    Dim xlDestinationFilename As XElement = <PARAMETER name="DestinationFilename" value=<%= ReplaceSplCharacters(sFileName) %>/>
                                    xlParameters.Add(xlDestinationFilename)
                                End If

                                Dim xIsTimeStampAppended As XElement = <PARAMETER name="IsTimeStampAppended" value=<%= oDocumentCollection(oRow.DataItemIndex).IsTimeStampAppended %>/>
                                xlParameters.Add(xIsTimeStampAppended)


                                'when called from Reports Link
                                If Reports = True Then
                                    Dim sDesPath As String
                                    If String.IsNullOrEmpty(Branch) Then
                                        sDesPath = "General/Reports/" & Replace(Date.Today, "/", "-") & "/" & IO.Path.GetFileName(oDocumentCollection(oRow.DataItemIndex).FileLocation)
                                    Else
                                        sDesPath = Branch & "/General/Reports/" & Replace(Date.Today, "/", "-") & "/" & IO.Path.GetFileName(oDocumentCollection(oRow.DataItemIndex).FileLocation)
                                    End If
                                    Dim xlDestinationFileName As XElement = <PARAMETER name="DestinationFilename" value=<%= ReplaceSplCharacters(sDesPath) %>/>
                                    xlJob.Element("JOB").Element("PARAMETERS").Add(xlDestinationFileName)
                                    'documents to generate so specify the document template code
                                    Dim xlCode As XElement = <PARAMETER name="code" value=<%= oDocumentCollection(oRow.DataItemIndex).documentTemplateCode %>/>
                                    xlJob.Element("JOB").Element("PARAMETERS").Add(xlCode)

                                    xIsTimeStampAppended = <PARAMETER name="IsTimeStampAppended" value=<%= oDocumentCollection(oRow.DataItemIndex).IsTimeStampAppended %>/>
                                    xlParameters.Add(xIsTimeStampAppended)
                                End If
                                'add the parameters to the item
                                xlItem.Add(xlParameters)

                                'add the item to the job
                                xlJob.Element("JOB").Element("ITEMS").Add(xlItem)
                            End If
                        End If
                    Else
                        reqCategory.Enabled = False
                    End If
                End If

            End If
        Next
        If bAddedFirst Then
            Dim strJob As String = xlJob.ToString 'this will be used as input to the SAM call
            Dim sDescription As String = "Archive documents"
            'call SAM to queue the docs for Archiving
            Dim iBackgroundJobID As Integer = oWebService.CreateBackgroundJob(sDescription, strJob, Now.Date)
            ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "archiveOK", "alert('" & GetLocalResourceObject("archiveConfirm").ToString() & "');", True)
        End If

    End Sub

    ''' <summary>
    ''' Server side validation to ensure that a selection has been made for all category and sub category drop downs
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Sub ValidateCategories(ByVal source As Object, ByVal args As ServerValidateEventArgs)
        Dim bValid As Boolean = True

        For Each oRow As GridViewRow In grdDocuments.Rows
            Dim drpCategory As NexusProvider.LookupList = oRow.FindControl("drpCategory")
            If drpCategory IsNot Nothing Then
                If String.IsNullOrEmpty(drpCategory.Value) Then
                    'we have a category drop down which has no selected value so we fail validation
                    bValid = False
                    Exit For
                End If
            End If

            Dim drpSubCategory As NexusProvider.LookupList = oRow.FindControl("drpSubCategory")
            If drpSubCategory IsNot Nothing Then
                If String.IsNullOrEmpty(drpSubCategory.Value) Then
                    'we have a sub category drop down which has no selected value so we fail validation
                    bValid = False
                    Exit For
                End If
            End If
        Next
        args.IsValid = bValid
    End Sub

    Private Function ReplaceMergedField(ByVal sMergedDocumentName As String, ByRef bIsPortalTimeStampAppended As Boolean, Optional ByVal sCoverStartDate As Date = Nothing) As String
        Dim nPartyKey As Integer
        Dim sPolicyReference As String = String.Empty
        Dim oUserDetails As NexusProvider.UserDetails = Nothing
        Dim oQuote As NexusProvider.Quote = Nothing
        Dim oClaim As NexusProvider.ClaimOpen = Nothing
        Dim oWebService As NexusProvider.ProviderBase = Nothing
        Dim sNewDocumentName As String = String.Empty
        Dim sClientName As String = String.Empty
        Dim sBranchName As String = String.Empty
        Dim oDocuments As Config.Documents = Nothing
        Dim sDocumentDirName As String = String.Empty
        Dim sDocumentCode As String = String.Empty
        Dim sTagFileName As String = String.Empty
        Dim sTagFileFormat As String = String.Empty
        Dim oParty As NexusProvider.BaseParty = Nothing
        Dim sPolicyNo As String = String.Empty
        Dim sCurrentPartyCode As String = String.Empty

        Try
            If Session(CNMode) = Mode.NewClaim Or Session(CNMode) = Mode.EditClaim Or Session(CNMode) = Mode.PayClaim Or Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Or Session(CNMode) = Mode.ViewClaim Then
                oQuote = Session(CNClaimQuote)
                oClaim = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
            Else
                oQuote = Session(CNQuote)
                If Session.Item(CNPolicy_Summary) IsNot Nothing Then
                    Dim oPolicySummary As NexusProvider.PolicySummary = Session.Item(CNPolicy_Summary)
                    sPolicyReference = oPolicySummary.Reference
                Else
                    sPolicyReference = oQuote.InsuranceFileRef.Trim()
                End If
            End If


            oDocuments = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork) _
                .Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode).Documents
            sDocumentDirName = oDocuments.Location
            oUserDetails = Session(CNAgentDetails)
            oWebService = New NexusProvider.ProviderManager().Provider
            sDocumentDirName = Right(sDocumentDirName, (sDocumentDirName.Length - sDocumentDirName.LastIndexOf("\")))
            oParty = Session.Item(CNParty)
            sPolicyNo = oQuote.InsuranceFileRef.Trim()


            sNewDocumentName = sMergedDocumentName

            'if Oparty is nothing
            If oParty Is Nothing AndAlso oQuote.PartyKey > 0 Then
                Dim sBranchCode As String = Nothing
                If String.IsNullOrEmpty(oQuote.BranchCode) = False Then
                    sBranchCode = oQuote.BranchCode
                End If
                oParty = oWebService.GetParty(oQuote.PartyKey, sBranchCode)
            End If

            sPolicyNo = sPolicyNo.Replace("/", "-")
            sPolicyNo = sPolicyNo.Replace("\", "-")

            Dim sClaimNo As String = Nothing
            If Session(CNClaimNumber) IsNot Nothing Then
                sClaimNo = Session(CNClaimNumber)
                sClaimNo = sClaimNo.Replace("/", "-")
                sClaimNo = sClaimNo.Replace("\", "-")
            End If

            If oUserDetails IsNot Nothing Then
                sNewDocumentName = sNewDocumentName.Replace("[!UserID!]", oUserDetails.ResolvedName)
            ElseIf oParty IsNot Nothing Then
                sNewDocumentName = sNewDocumentName.Replace("[!UserID!]", oParty.TPIntroducer)
            End If

            If Session(CNMode) = Mode.NewClaim Or Session(CNMode) = Mode.EditClaim Or Session(CNMode) = Mode.PayClaim Or Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Or Session(CNMode) = Mode.ViewClaim Then
                nPartyKey = oClaim.Client.PartyKey
                If oParty IsNot Nothing Then
                    Select Case True
                        Case TypeOf oParty Is NexusProvider.CorporateParty
                            With CType(oParty, NexusProvider.CorporateParty)
                                If .ClientSharedData IsNot Nothing AndAlso String.IsNullOrEmpty(.ClientSharedData.ShortName) = False Then
                                    sCurrentPartyCode = .ClientSharedData.ShortName.Trim()
                                End If
                                If .ClientSharedData IsNot Nothing AndAlso String.IsNullOrEmpty(.ClientSharedData.ResolvedName) = False Then
                                    sClientName = DirectCast(oParty, NexusProvider.CorporateParty).ClientSharedData.ResolvedName
                                End If
                            End With
                        Case TypeOf oParty Is NexusProvider.PersonalParty
                            With CType(oParty, NexusProvider.PersonalParty)

                                If .ClientSharedData IsNot Nothing AndAlso String.IsNullOrEmpty(.ClientSharedData.ShortName) = False Then
                                    sCurrentPartyCode = .ClientSharedData.ShortName.Trim()
                                End If
                                If .ClientSharedData IsNot Nothing AndAlso String.IsNullOrEmpty(.ClientSharedData.ResolvedName) = False Then
                                    sClientName = DirectCast(oParty, NexusProvider.PersonalParty).ClientSharedData.ResolvedName
                                End If
                            End With
                    End Select
                    sNewDocumentName = sNewDocumentName.Replace("[!ClientCode!]", sCurrentPartyCode)
                    sNewDocumentName = sNewDocumentName.Replace("[!ClientName!]", sClientName)
                Else
                    sNewDocumentName = sNewDocumentName.Replace("[!ClientCode!]", oClaim.Client.ShortName)
                    sNewDocumentName = sNewDocumentName.Replace("[!ClientName!]", oClaim.Client.ClientName)
                End If


            ElseIf oParty IsNot Nothing Then
                Select Case True
                    Case TypeOf oParty Is NexusProvider.CorporateParty
                        With CType(oParty, NexusProvider.CorporateParty)
                            If .ClientSharedData IsNot Nothing AndAlso String.IsNullOrEmpty(.ClientSharedData.ShortName) = False Then
                                sCurrentPartyCode = .ClientSharedData.ShortName.Trim()
                            End If
                            If .ClientSharedData IsNot Nothing AndAlso String.IsNullOrEmpty(.ClientSharedData.ResolvedName) = False Then
                                sClientName = DirectCast(oParty, NexusProvider.CorporateParty).ClientSharedData.ResolvedName
                            End If
                        End With
                    Case TypeOf oParty Is NexusProvider.PersonalParty
                        With CType(oParty, NexusProvider.PersonalParty)

                            If .ClientSharedData IsNot Nothing AndAlso String.IsNullOrEmpty(.ClientSharedData.ShortName) = False Then
                                sCurrentPartyCode = .ClientSharedData.ShortName.Trim()
                            End If
                            If .ClientSharedData IsNot Nothing AndAlso String.IsNullOrEmpty(.ClientSharedData.ResolvedName) = False Then
                                sClientName = DirectCast(oParty, NexusProvider.PersonalParty).ClientSharedData.ResolvedName
                            End If
                        End With
                End Select
                sNewDocumentName = sNewDocumentName.Replace("[!ClientCode!]", sCurrentPartyCode)
                sNewDocumentName = sNewDocumentName.Replace("[!ClientName!]", sClientName)
            End If

            ' File Name concatenation
            sNewDocumentName = sNewDocumentName.Replace("[!ProductName!]", oQuote.ProductCode)
            sNewDocumentName = sNewDocumentName.Replace("[!PolicyNumber!]", sPolicyNo)
            If sClaimNo IsNot Nothing Then
                sNewDocumentName = sNewDocumentName.Replace("[!ClaimNumber!]", sClaimNo)
            Else
                sNewDocumentName = sNewDocumentName.Replace("[!ClaimNumber!]", "")
            End If

            sNewDocumentName = sNewDocumentName.Replace("[!BranchCode!]", oQuote.BranchCode)
            sNewDocumentName = sNewDocumentName.Replace("[!BranchName!]", oQuote.BranchName)
            sNewDocumentName = sNewDocumentName.Replace("[!TransactionType!]", oQuote.TransactionType.ToString())
            sNewDocumentName = sNewDocumentName.Replace("[!InsuranceFileKey!]", oQuote.InsuranceFileKey)
            sNewDocumentName = sNewDocumentName.Replace("[!CoverStartDate!]", sCoverStartDate.ToString("dd-MMMM-yyyy"))

            'Get Bo Setting For TimeStamp if The Setting persist in Bo then Portal Format will be overrided
            Dim oDocumentArchiveTimeStampSetting As NexusProvider.OptionTypeSetting
            Dim sDocumentAchiveWithTimeStamp As String = String.Empty
            Dim sDocumentAchiveWithTimeStampBOFormat As String = String.Empty

            oWebService = New NexusProvider.ProviderManager().Provider
            oDocumentArchiveTimeStampSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5145)
            'For BO Checkbox
            If oDocumentArchiveTimeStampSetting IsNot Nothing AndAlso oDocumentArchiveTimeStampSetting.OptionValue <> "" Then
                sDocumentAchiveWithTimeStamp = oDocumentArchiveTimeStampSetting.OptionValue
            End If

            If sNewDocumentName.Contains("[!CurrentDate!]") OrElse sNewDocumentName.Contains("[!CurrentDateTime!]") Then
                bIsPortalTimeStampAppended = True
            Else
                bIsPortalTimeStampAppended = False
            End If

            If Trim(sDocumentAchiveWithTimeStamp) = "1" Then
                'For BO Format
                oWebService = New NexusProvider.ProviderManager().Provider
                oDocumentArchiveTimeStampSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5146)
                If oDocumentArchiveTimeStampSetting IsNot Nothing AndAlso oDocumentArchiveTimeStampSetting.OptionValue <> "" Then

                    If Trim(oDocumentArchiveTimeStampSetting.OptionValue) = "0" Then
                        sDocumentAchiveWithTimeStampBOFormat = "yyyyMMdd hhmmss tt"
                    ElseIf Trim(oDocumentArchiveTimeStampSetting.OptionValue) = "1" Then
                        sDocumentAchiveWithTimeStampBOFormat = "MMddyyyy hhmmss tt"
                    End If
                End If
                If Trim(sDocumentAchiveWithTimeStampBOFormat) <> "" Then
                    sNewDocumentName = sNewDocumentName.Replace("[!CurrentDate!]", Date.Now().ToString(sDocumentAchiveWithTimeStampBOFormat))
                    sNewDocumentName = sNewDocumentName.Replace("[!CurrentDateTime!]", Date.Now().ToString(sDocumentAchiveWithTimeStampBOFormat))
                End If
            Else
                sNewDocumentName = sNewDocumentName.Replace("[!CurrentDate!]", Date.Now().ToString("dd-MMMM-yyyy"))
                sNewDocumentName = sNewDocumentName.Replace("[!CurrentDateTime!]", Date.Now().ToString("dd-MMMM-yyyy HH_mm"))
            End If

            sNewDocumentName = sNewDocumentName.Replace("/", "-")
            sNewDocumentName = sNewDocumentName.Replace("\", "-")
            sNewDocumentName = ReplaceSplCharacters(sNewDocumentName)
            Return sNewDocumentName

        Catch ex As NexusProvider.NexusException
            Throw
        Finally
            oUserDetails = Nothing
            oQuote = Nothing
            oClaim = Nothing
            oDocuments = Nothing
            oParty = Nothing
        End Try
        Return sNewDocumentName
    End Function

    Private Shared Function ReplaceSplCharacters(ByRef str As String) As String
        Dim illegalChars As Char() = ":~""#%&*<>?/\{}|.".ToCharArray()
        Dim ext As String
        Dim fName As String
        If str.LastIndexOf(".") = -1 Then
            fName = str
            ext = ""
        Else
            ext = str.Substring(str.LastIndexOf("."))
            fName = str.Substring(0, str.LastIndexOf("."))
        End If


        Dim sb As New System.Text.StringBuilder

        For Each ch As Char In fName
            If Array.IndexOf(illegalChars, ch) = -1 Then
                sb.Append(ch)
            End If
        Next
        Return sb.ToString() & IIf(ext.Length > 1, ext, "")
    End Function
    Private Sub OpenLink(ByVal oDocument As Object)

        Response.AddHeader("Content-Disposition", "attachment; filename=" & oDocument.DocumentName)
        Response.WriteFile(oDocument.FileLocation)
        Response.Flush()
        Response.End()
    End Sub

End Class