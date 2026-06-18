Imports NexusProvider.SAMForInsurance.PureService
Imports System.ServiceModel
Imports System.Configuration.Provider
Imports System.Web
Imports System.Web.HttpContext
Imports System.Web.Configuration.WebConfigurationManager
Imports Microsoft.Web.Services3.Security.Tokens
Imports SiriusFS.SAM.Client.Security
Imports System.Xml.Serialization
Imports Microsoft.Practices.EnterpriseLibrary.Logging
Imports System.Diagnostics
Imports System.Text
Imports System.Xml
Imports System.IO
Imports Ionic.Zip
Partial Public Class ProviderSAMForInsuranceV2
    Public Overrides Function CreateBackgroundJob(ByVal v_sDescription As String,
                                                     ByVal v_sJobXML As String,
                                                     ByVal v_dJobWhenToStart As Date,
                                                             Optional ByVal v_sBranchCode As String = Nothing) As Integer

        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oCreateBackgroundJobRequest As CreateBackgroundJobRequestType
            Dim oCreateBackgroundJobResponse As CreateBackgroundJobResponseType
            Dim iBackgroundJobID As Integer
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oCreateBackgroundJobRequest = New CreateBackgroundJobRequestType
                oCreateBackgroundJobResponse = New CreateBackgroundJobResponseType
                sbLogMessage = New StringBuilder


                With oCreateBackgroundJobRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode

                    End If

                    .Description = v_sDescription
                    .JobWhenToStart = v_dJobWhenToStart
                    .JobXML = v_sJobXML

                End With


                Using trace As New Tracer(Category.Trace)
                    oCreateBackgroundJobResponse = oSAM.CreateBackgroundJob(oCreateBackgroundJobRequest)

                End Using

                With oCreateBackgroundJobResponse

                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)

                    Else
                        iBackgroundJobID = oCreateBackgroundJobResponse.BackgroundJobID

                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("CreateBackgroundJob executed ok" & vbCrLf)

                    sbLogMessage.AppendLine("Input : " & vbCrLf)

                    If IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("Branch Code : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Branch Code : " & v_sBranchCode.ToString & vbCrLf)
                    End If

                    If IsNothing(v_sDescription) Then
                        sbLogMessage.AppendLine("Description : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Description : " & v_sDescription.ToString & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Output : " & vbCrLf)

                    If IsNothing(iBackgroundJobID) Then
                        sbLogMessage.AppendLine("Background Job ID : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Background Job ID : " & iBackgroundJobID.ToString & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oCreateBackgroundJobRequest = Nothing
                oCreateBackgroundJobResponse = Nothing
            End Try


            Return iBackgroundJobID
        End SyncLock
    End Function

    ''' <summary>
    ''' Returns the defaults for a given document template
    ''' </summary>
    ''' <param name="DocumentTemplateCodes">Comma separated list of document template codes</param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetDocumentDefaults(ByVal DocumentTemplateCodes As List(Of String),
                                        Optional ByVal v_sBranchCode As String = Nothing) As DocumentDefaultsCollection
        Dim oSAM As PureServiceClient
        Dim oGetDocumentDefaultsRequest As GetDocumentDefaultsRequestType
        Dim oGetDocumentDefaultsResponse As GetDocumentDefaultsResponseType
        Dim sDocumentTemplateCodes As String = String.Empty
        Dim oDocumentDefaultsCollection As DocumentDefaultsCollection
        Dim sbLogMessage As StringBuilder

        Try
            oSAM = InitializeServiceMethod()
            oGetDocumentDefaultsRequest = New GetDocumentDefaultsRequestType
            oGetDocumentDefaultsResponse = New GetDocumentDefaultsResponseType
            oDocumentDefaultsCollection = New DocumentDefaultsCollection
            sbLogMessage = New StringBuilder


            'need to convert the list of strings into a comma separated list

            For Each sItem As String In DocumentTemplateCodes
                sDocumentTemplateCodes += sItem & ","
            Next
            'take off the trailing ,
            If sDocumentTemplateCodes <> String.Empty Then
                sDocumentTemplateCodes = Left(sDocumentTemplateCodes, Len(sDocumentTemplateCodes) - 1)
            End If
            'create object to hold the collection returned


            'nothing in cache so make the call to sam
            With oGetDocumentDefaultsRequest
                If String.IsNullOrEmpty(v_sBranchCode) Then
                    'if the branch code is NOT in session 
                    If String.IsNullOrEmpty(sBranchCode) Then
                        'Use the default branch code
                        .BranchCode = sDefaultBranchCode
                    Else
                        'Use the branch code in session 
                        .BranchCode = sBranchCode

                    End If
                Else
                    'use the passed parameter v_sBranchCode
                    .BranchCode = v_sBranchCode

                End If

                .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                .DocumentTemplateCodes = sDocumentTemplateCodes
            End With


            Using trace As New Tracer(Category.Trace)
                oGetDocumentDefaultsResponse = oSAM.GetDocumentDefaults(oGetDocumentDefaultsRequest)
            End Using




            With oGetDocumentDefaultsResponse
                If .Errors IsNot Nothing Then
                    'Process the error object if errors, and throw as a single exception
                    Throw New NexusException(.Errors)
                Else
                    If .DocumentTemplates IsNot Nothing Then
                        For Each oDocumentTemplates As BaseGetDocumentDefaultsResponseTypeDocumentTemplates In .DocumentTemplates
                            ' make sure Document Template Code not nothing
                            If oDocumentTemplates.DocumentTemplateCode IsNot Nothing AndAlso DocumentTemplateCodes.Contains(oDocumentTemplates.DocumentTemplateCode) Then
                                oDocumentDefaultsCollection.Add(New DocumentDefaults With {
                                                                .documentTemplateCode = oDocumentTemplates.DocumentTemplateCode,
                                                                .documentTemplateDescription = oDocumentTemplates.DocumentTemplateDescription,
                                                                .documentTemplateID = oDocumentTemplates.DocumentTemplateID,
                                                                .documentGroupCode = oDocumentTemplates.DocumentGroupCode,
                                                                .documentGroupDescription = oDocumentTemplates.DocumentGroupDescription,
                                                                .documentGroupID = oDocumentTemplates.DocumentGroupID,
                                                                .documentSubGroupCode = oDocumentTemplates.DocumentSubGroupCode,
                                                                .documentSubGroupDescription = oDocumentTemplates.DocumentSubGroupDescription,
                                                                .documentSubGroupID = oDocumentTemplates.DocumentSubGroupID,
                                                                .InternalOnly = oDocumentTemplates.InternalOnly,
                                                                .Selected = oDocumentTemplates.SelectedByDefault
                                                                })
                            End If
                        Next
                    End If
                End If
            End With

            If Logger.IsLoggingEnabled Then
                sbLogMessage.AppendLine("GetDocumentDefaults executed ok" & vbCrLf)

                sbLogMessage.AppendLine("Input : " & vbCrLf)

                If IsNothing(v_sBranchCode) Then
                    sbLogMessage.AppendLine("Branch Code : nothing" & vbCrLf)
                Else
                    sbLogMessage.AppendLine("Branch Code : " & v_sBranchCode.ToString & vbCrLf)
                End If

                sbLogMessage.AppendLine("Output : " & vbCrLf)

                'If IsNothing(iBackgroundJobID) Then
                '    sbLogMessage.AppendLine("Background Job ID : nothing" & vbCrLf)
                'Else
                '    sbLogMessage.AppendLine("Background Job ID : " & iBackgroundJobID.ToString & vbCrLf)
                'End If

                LogMessageEntry(sbLogMessage)
            End If

            'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

            'FaultErrorHandler(ex) ' handling fault error messages 

        Catch ex As Exception
            Throw
        Finally
            oSAM.Close()
            oGetDocumentDefaultsRequest = Nothing
            oGetDocumentDefaultsResponse = Nothing
        End Try


        Return oDocumentDefaultsCollection

    End Function

    ''' <summary>
    ''' This Method is created to Get the SharePointFile List
    ''' </summary>
    ''' <param name="v_sPartyShortName"></param>
    ''' <param name="v_sPolicyNumber"></param>
    ''' <param name="v_sClaimNumber"></param>
    ''' <param name="v_sFolderPath"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <param name="v_bCreateFolder"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetSharePointFileList(ByVal v_sPartyShortName As String,
                                                   Optional ByVal v_sPolicyNumber As String = Nothing,
                                                   Optional ByVal v_sClaimNumber As String = Nothing,
                                                   Optional ByVal v_sFolderPath As String = Nothing,
                                                   Optional ByVal v_sBranchCode As String = Nothing,
                                                   Optional ByVal v_bCreateFolder As Boolean = False) As SharepointFileList

        Dim oSAM As PureServiceClient 'SAMForInsuranceV2's Object
        Dim oGetSharepointFileListRequest As GetSharepointFileListRequestType    ' Request Type
        Dim oGetSharepointFileListResponse As GetSharepointFileListResponseType   ' Response Type
        Dim oSPFileListItemCollection As SharepointFileListResponseTypeItemListCollection
        Dim oSharepointFileList As SharepointFileList
        Dim oSharepointFolderPath As SharepointFolderPath
        Dim oSharepointFileListResponseTypeItemList As SharepointFileListResponseTypeItemList
        Dim oListItem As NexusProvider.SharepointFileListResponseTypeItemList
        Dim sbLogMessage As StringBuilder

        Try
            oSAM = InitializeServiceMethod()
            oGetSharepointFileListRequest = New GetSharepointFileListRequestType
            oGetSharepointFileListResponse = New GetSharepointFileListResponseType
            oListItem = New NexusProvider.SharepointFileListResponseTypeItemList
            sbLogMessage = New StringBuilder


            With oGetSharepointFileListRequest
                .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                'if the passed parameter v_sBranchCode is empty 
                If String.IsNullOrEmpty(v_sBranchCode) Then
                    'if the branch code is NOT in session 
                    If String.IsNullOrEmpty(sBranchCode) Then
                        'Use the default branch code
                        .BranchCode = sDefaultBranchCode
                    Else
                        'Use the branch code in session 
                        .BranchCode = sBranchCode
                    End If
                Else
                    'use the passed parameter v_sBranchCode
                    .BranchCode = v_sBranchCode

                End If

                .PartyShortname = v_sPartyShortName
                .PolicyNumber = v_sPolicyNumber
                .ClaimNumber = v_sClaimNumber
                .FolderPath = v_sFolderPath
                .CreateFolder = v_bCreateFolder 'pass this flag to automatically create folders in sharepoint

            End With


            'Calling the SAM Method with Request Type
            'add trace to allow us to debug slow SAM calls
            Using trace As New Tracer(Category.Trace)
                oGetSharepointFileListResponse = oSAM.GetSharepointFileList(oGetSharepointFileListRequest)
            End Using

            'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

            ' Disposing the SAM's object







            With oGetSharepointFileListResponse  'With Response Type
                If .Errors IsNot Nothing Then
                    'Process the error object if errors, and throw as a single exception
                    Throw New NexusException(.Errors)
                Else
                    'WorkManager Response
                    'Fetching from the  SubAgents Response Collection 
                    'v_sFolderPath = .FolderPath
                    oSharepointFileList = New SharepointFileList

                    oSharepointFolderPath = New SharepointFolderPath
                    oSharepointFolderPath.FolderPath = .FolderPath

                    oSharepointFileList.FolderPath = oSharepointFolderPath

                    If .ItemList IsNot Nothing Then

                        oSPFileListItemCollection = New SharepointFileListResponseTypeItemListCollection

                        'For Each oBaseSharepointItemList As ArrayOfBaseGetSharepointFileListResponseTypeItemListBaseGetSharepointFileListResponseTypeItemList In .ItemList
                        For iCt As Integer = 0 To .ItemList.Count - 1
                            oSharepointFileListResponseTypeItemList = New SharepointFileListResponseTypeItemList
                            oSharepointFileListResponseTypeItemList.Title = .ItemList(iCt).Title
                            oSharepointFileListResponseTypeItemList.URL = .ItemList(iCt).URL
                            oSharepointFileListResponseTypeItemList.ItemType = .ItemList(iCt).ItemType
                            oSharepointFileListResponseTypeItemList.InternalOnly = .ItemList(iCt).InternalOnly
                            oSharepointFileListResponseTypeItemList.PureUser = .ItemList(iCt).PureUser
                            oSharepointFileListResponseTypeItemList.Filename = .ItemList(iCt).Filename
                            oSharepointFileListResponseTypeItemList.CreatedDate = .ItemList(iCt).CreatedDate
                            oSharepointFileListResponseTypeItemList.LastModifiedDate = .ItemList(iCt).LastModifiedDate
                            oSharepointFileListResponseTypeItemList.DocumentTemplateGroup = .ItemList(iCt).DocumentTemplateGroup
                            oSharepointFileListResponseTypeItemList.DocumentTemplateSubGroup = .ItemList(iCt).DocumentTemplateSubGroup

                            oSPFileListItemCollection.Add(oSharepointFileListResponseTypeItemList)
                        Next
                        oSharepointFileList.ItemList = oSPFileListItemCollection
                    End If
                End If
            End With
            'oSharepointFileList.FolderPath = oSharepointFolderPath

            If Logger.IsLoggingEnabled Then
                sbLogMessage.AppendLine("SharePoint Item List executed ok" & vbCrLf)
                sbLogMessage.AppendLine("Input:" & oSPFileListItemCollection.Print() & vbCrLf)
                If Not IsNothing(v_sBranchCode) Then
                    sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                Else
                    sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                End If

                sbLogMessage.AppendLine("Returned " & oSPFileListItemCollection.Print() & "results" & vbCrLf)

                LogMessageEntry(sbLogMessage)
            End If

            'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

            'FaultErrorHandler(ex) ' handling fault error messages 

        Catch ex As Exception
            Throw
        Finally
            oSAM.Close()
            oGetSharepointFileListRequest = Nothing
            oGetSharepointFileListResponse = Nothing
        End Try


        Return oSharepointFileList

    End Function


    ''' <summary>
    ''' Requests document via SAM, saves the file to the specified location and returns this location
    ''' </summary>
    ''' <param name="v_iPartyKey"></param>
    ''' <param name="v_iInsuranceFileKey"></param>
    ''' <param name="v_iInsuranceFolderKey"></param>
    ''' <param name="v_sDocumentCode"></param>
    ''' <param name="v_oDocumentType"></param>
    ''' <param name="v_sDocumentExtractionDirectory"></param>
    ''' <param name="v_iClaimKey"></param>
    ''' <param name="v_bSpoolDocumentOnly"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <param name="v_sDocumentRef"></param>
    ''' <returns>String giving the location of the saved file</returns>
    ''' <remarks></remarks>
    Public Overrides Function GenerateDocument(ByVal v_iPartyKey As Integer,
                                            ByVal v_iInsuranceFileKey As Integer,
                                            ByVal v_iInsuranceFolderKey As Integer,
                                            ByVal v_sDocumentCode As String,
                                            ByVal v_oDocumentType As DocumentType,
                                            ByVal v_sDocumentExtractionPath As String,
                                            Optional ByVal v_iClaimKey As Integer = 0,
                                            Optional ByVal v_bSpoolDocumentOnly As Boolean = False,
                                            Optional ByVal v_sBranchCode As String = Nothing,
                                            Optional ByVal v_sDocumentRef As String = Nothing,
                                            Optional ByVal bIsSuppressArchive As Boolean = False
                                            ) As String
        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oGenerateDocumentRequest As GenerateDocumentRequestType
            Dim oGenerateDocumentResponse As GenerateDocumentResponseType
            Dim sDirectoryName As String = Left(v_sDocumentExtractionPath, v_sDocumentExtractionPath.LastIndexOf("\"))

            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oGenerateDocumentRequest = New GenerateDocumentRequestType
                oGenerateDocumentResponse = New GenerateDocumentResponseType
                sbLogMessage = New StringBuilder


                With oGenerateDocumentRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    If String.IsNullOrEmpty(v_sDocumentCode) Then
                    Throw New ArgumentNullException("DocumentCode")
                Else
                    .DocumentTemplateCode = v_sDocumentCode
                End If

                If v_iPartyKey > 0 Then
                    .PartyKey = v_iPartyKey
                Else
                    Throw New ArgumentNullException("PartyKey")
                End If

                If v_iInsuranceFileKey > 0 Then
                    .InsuranceFileKey = v_iInsuranceFileKey
                Else
                    Throw New ArgumentNullException("InsuranceFileKey")
                End If

                If v_iInsuranceFolderKey > 0 Then
                    .InsuranceFolderKey = v_iInsuranceFolderKey
                Else
                    Throw New ArgumentNullException("InsuranceFolderKey")
                End If

                If String.IsNullOrEmpty(v_sDocumentExtractionPath) Then
                    Throw New ArgumentNullException("DocumentExtractionPath")
                Else
                    If Not IO.Directory.Exists(sDirectoryName) Then
                        'create the directory that the file will be written to
                        IO.Directory.CreateDirectory(sDirectoryName)
                    End If

                    .SpoolDocumentOnly = v_bSpoolDocumentOnly
                    .SpoolDocumentOnlySpecified = v_bSpoolDocumentOnly
                End If

                Select Case v_oDocumentType
                    Case DocumentType.None
                        Throw New ArgumentException("Can not be DocumentType.None", "DocumentType")
                    Case DocumentType.HTML
                        'changes as of Pure 2.01 mean that
                        Throw New ArgumentException("Can not be DocumentType.HTML, no longer supported", "DocumentType")
                    Case DocumentType.PDF
                        .OutputAsPDF = True
                        .OutputAsHTML = False
                    Case DocumentType.DOCX
                        .OutputAsPDF = False
                        .OutputAsHTML = False
                End Select

                'Sure what these represent, but they've always be set to 4 and nothing
                .Mode = 4
                .ParameterXML = Nothing
                If v_iClaimKey > 0 Then
                    .ClaimKey = v_iClaimKey
                    .ClaimKeySpecified = True
                Else
                    .ClaimKeySpecified = False
                End If

                    If String.IsNullOrEmpty(v_sDocumentRef) = False Then
                        .DocumentRef = v_sDocumentRef
                    End If
                    .IsSuppressArchive = bIsSuppressArchive
                End With


                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                oGenerateDocumentResponse = oSAM.GenerateDocument(oGenerateDocumentRequest)
            End Using





            With oGenerateDocumentResponse

                If .Errors IsNot Nothing Then
                    'Process the error object if errors, and throw as a single exception
                    Throw New NexusException(.Errors)
                Else
                    'write the file, returned as a byte array in SpooledZipFile to disk
                    Dim fsOutputFile As IO.FileStream = IO.File.OpenWrite(v_sDocumentExtractionPath)
                    fsOutputFile.Write(.SpooledZipFile, 0, .SpooledZipFile.Length)
                    fsOutputFile.Close()
                End If

            End With

            If Logger.IsLoggingEnabled Then
                sbLogMessage.AppendLine("GenerateDocument executed ok" & vbCrLf)
                sbLogMessage.AppendLine("Input:" & vbCrLf)
                sbLogMessage.AppendLine("v_iPartyKey = " & v_iPartyKey.ToString & vbCrLf)
                sbLogMessage.AppendLine("v_iInsuranceFileKey = " & v_iInsuranceFileKey.ToString & vbCrLf)
                sbLogMessage.AppendLine("v_iInsuranceFolderKey = " & v_iInsuranceFolderKey.ToString & vbCrLf)
                sbLogMessage.AppendLine("v_sDocumentCode = " & v_sDocumentCode.ToString & vbCrLf)
                sbLogMessage.AppendLine("v_oDocumentType = " & v_oDocumentType.ToString & vbCrLf)
                sbLogMessage.AppendLine("v_sDocumentExtractionDirectory = " & v_sDocumentExtractionPath.ToString & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Returned " & v_sDocumentExtractionPath & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
                Finally
                oSAM.Close()
                oGenerateDocumentRequest = Nothing
                oGenerateDocumentResponse = Nothing
                sDirectoryName = Nothing
                End Try


                Return v_sDocumentExtractionPath 'todo - should be a sub, we're just passing back an input

        End SyncLock

    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_sDocumentTemplateCode"></param>
    ''' <param name="v_iDocumentTemplateKey"></param>
    ''' <param name="v_sTemplateExtractionDirectory"></param>
    ''' <param name="v_oDocumentFormatType"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetStandardWordingsTemplate(ByVal v_sDocumentTemplateCode As String,
                                                      ByVal v_iDocumentTemplateKey As Integer,
                                                      ByVal v_sTemplateExtractionDirectory As String,
                                                      ByVal v_oDocumentFormatType As NexusProvider.DocumentFormatType,
                                                      Optional ByVal v_sBranchCode As String = Nothing) As String

        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oGetStandardWordingTemplateRequest As GetStandardWordingTemplateRequestType
            Dim oGetStandardWordingTemplateResponse As GetStandardWordingTemplateResponseType
            Dim sFileName As String = Right(v_sTemplateExtractionDirectory, (v_sTemplateExtractionDirectory.Length - v_sTemplateExtractionDirectory.LastIndexOf("\")))
            sFileName = sFileName.Remove(0, 1)

            'Dim sFileName As String = Right(v_sTemplateExtractionDirectory, (v_sTemplateExtractionDirectory.Length - v_sTemplateExtractionDirectory.LastIndexOf("\")))
            Dim oDocumentTemplatesCollection As DocumentTemplateCollection
            Dim sReturnString As String
            Dim sZipFilePath As String = v_sTemplateExtractionDirectory & ".zip"
            Dim fsOutputFile As IO.FileStream = IO.File.OpenWrite(sZipFilePath)
            'Dim sTemp As String = v_sTemplateExtractionDirectory '& "" & oGetStandardWordingTemplateResponse.MergedFilePath
            'Dim file As System.IO.FileInfo = Nothing
            'Dim sourceDir As DirectoryInfo = New DirectoryInfo(sTemp) 'duz1.Destination
            'Dim sFileToCopy As String = Nothing
            'Dim sFileTargetLocation As String = Nothing
            'Dim bIsXMLFile As Boolean = False
            Dim Comletepath As String
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oGetStandardWordingTemplateRequest = New GetStandardWordingTemplateRequestType
                oGetStandardWordingTemplateResponse = New GetStandardWordingTemplateResponseType
                oDocumentTemplatesCollection = New DocumentTemplateCollection
                sbLogMessage = New StringBuilder

                sFileName = sFileName.Remove(0, 1)
                With oGetStandardWordingTemplateRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    .DocumentTemplateCode = RTrim(v_sDocumentTemplateCode)

                    If v_iDocumentTemplateKey <> 0 Then
                        .DocumentTemplateKey = v_iDocumentTemplateKey
                        .DocumentTemplateKeySpecified = True
                    Else
                        .DocumentTemplateKeySpecified = False
                    End If
                    'Checking the Document Type
                    Select Case v_oDocumentFormatType
                        Case DocumentFormatType.HTML
                            .DocumentTemplateFormat = DocumentFormatType.HTML
                            .DocumentTemplateFormatSpecified = True
                        Case DocumentFormatType.PDF
                            .DocumentTemplateFormat = DocumentFormatType.PDF
                            .DocumentTemplateFormatSpecified = True
                        Case Else
                            .DocumentTemplateFormatSpecified = False
                    End Select
                End With


                Using trace As New Tracer(Category.Trace)
                    oGetStandardWordingTemplateResponse = oSAM.GetStandardWordingTemplate(oGetStandardWordingTemplateRequest)
                End Using

                With oGetStandardWordingTemplateResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)

                    Else



                        'Create unique zip directory on the web server
                        IO.Directory.CreateDirectory(v_sTemplateExtractionDirectory)

                        'Create a unique zip file from the byte array retrieved from the Web Service

                        fsOutputFile.Write(.DocumentTemplate, 0, .DocumentTemplate.Length)
                        fsOutputFile.Close()

                        ''Invoke the unzip method

                        Using zip As ZipFile = ZipFile.Read(sZipFilePath)
                            zip.ExtractAll(v_sTemplateExtractionDirectory, Ionic.Zip.ExtractExistingFileAction.OverwriteSilently)
                            zip.Dispose()
                        End Using

                        'Remove zip file, as it been unzipped
                        IO.File.Delete(sZipFilePath)

                        'Remove the drive letter, and colon from the file path
                        .MergedFilePath = .MergedFilePath.Remove(0, 2)


                        Dim sTemp As String = v_sTemplateExtractionDirectory & .MergedFilePath

                        Dim file As System.IO.FileInfo = Nothing
                        'name of the source directory
                        Dim sourceDir As DirectoryInfo = New DirectoryInfo(sTemp) 'duz1.Destination
                        'for copy the unzip file
                        Dim sFileToCopy As String = Nothing
                        Dim sFileTargetLocation As String = Nothing
                        Dim bIsXMLFile As Boolean = False

                        'find the name of the file which has been extracted, we need to return this
                        For Each file In sourceDir.GetFiles()
                            If file.Extension.ToUpper = ".XML" Or file.Extension.ToUpper = ".PDF" Or file.Extension.ToUpper = ".HTML" Or file.Extension.ToUpper = ".HTM" Then
                                'set sFileName to the name of the htm file 
                                sFileName = file.Name
                                If file.Extension.ToUpper = ".XML" Then
                                    bIsXMLFile = True
                                End If
                                Exit For
                            End If
                        Next

                        Comletepath = sTemp & "\" & sFileName
                        If bIsXMLFile Then
                            sReturnString = Comletepath.Replace(".xml", ".doc")
                            IO.File.Move(Comletepath, sReturnString)
                        Else
                            sReturnString = Comletepath
                        End If

                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetStandardPolicyWordings executed ok" & vbCrLf)

                    sbLogMessage.AppendLine("Input : " & vbCrLf)

                    If IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("Branch Code : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Branch Code : " & v_sBranchCode.ToString & vbCrLf)
                    End If

                    If IsNothing(v_iDocumentTemplateKey) Then
                        sbLogMessage.AppendLine("Document Template Key : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Document Template Key : " & v_iDocumentTemplateKey.ToString & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Output : " & vbCrLf)
                    sbLogMessage.AppendLine(oDocumentTemplatesCollection.Print().Replace("<br />", vbCrLf))

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oGetStandardWordingTemplateRequest = Nothing
                oGetStandardWordingTemplateResponse = Nothing
                sFileName = Nothing
            End Try


            Return sReturnString
        End SyncLock
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_sDocumentTemplateCode"></param>
    ''' <param name="v_iDocumentTemplateKey"></param>
    ''' <param name="v_sUpdatedTemplateLocation"></param>
    ''' <param name="v_sTempFileLocation"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function UpdateStandardWordingsTemplate(ByVal v_sDocumentTemplateCode As String,
                                                             ByVal v_iDocumentTemplateKey As Integer,
                                                             ByVal v_sUpdatedTemplateLocation As String,
                                                             ByVal v_sTempFileLocation As String,
                                                              ByVal v_oDocumentFormatType As NexusProvider.DocumentFormatType,
                                                             Optional ByVal v_sBranchCode As String = Nothing) As DocumentTemplate

        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oUpdateStandardWordingTemplateRequest As UpdateStandardWordingTemplateRequestType
            Dim oUpdateStandardWordingTemplateResponse As UpdateStandardWordingTemplateResponseType
            Dim filepathName1 As String = v_sUpdatedTemplateLocation
            Dim dir1 As New DirectoryInfo(filepathName1)
            Dim Folder1Files As FileInfo() = dir1.GetFiles()
            Dim sfilename As String

            Dim oDocumentTemplate As DocumentTemplate
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oUpdateStandardWordingTemplateRequest = New UpdateStandardWordingTemplateRequestType
                oUpdateStandardWordingTemplateResponse = New UpdateStandardWordingTemplateResponseType
                oDocumentTemplate = New DocumentTemplate
                sbLogMessage = New StringBuilder


                With oUpdateStandardWordingTemplateRequest
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If




                    If IO.Directory.Exists(v_sUpdatedTemplateLocation) Then

                        If Folder1Files.Length > 0 Then
                            For Each aFile As FileInfo In Folder1Files
                                sfilename = aFile.ToString()
                            Next
                        End If

                        If sfilename <> "" Then
                            v_sUpdatedTemplateLocation = v_sUpdatedTemplateLocation + "\" + sfilename
                        End If

                        Dim sourceDir As DirectoryInfo = New DirectoryInfo(v_sUpdatedTemplateLocation)
                        'for copy the unzip file
                        Dim sFileToCopy As String = Nothing
                        Dim sFileTargetLocation As String = Nothing
                        Dim sTempFileLocation As String = v_sTempFileLocation & "\"
                        Dim strText As String
                        strText = Replace(v_sUpdatedTemplateLocation, sTempFileLocation, "")
                        Dim astrSplitItems As String() = Split(strText, "\")
                        Dim sOldGUID As String
                        Dim intX As Integer
                        For intX = 0 To UBound(astrSplitItems)
                            If intX = 0 Then
                                sOldGUID = astrSplitItems(intX)
                                Exit For
                            End If
                        Next
                        sTempFileLocation = sTempFileLocation & sOldGUID

                        Dim sGUID As String = Guid.NewGuid.ToString

                        'Use the UniqueID for the Zip folder and file names
                        Dim sTempFilePath As String = sTempFileLocation & "\"
                        Dim sZipFilePath As String = v_sTempFileLocation & "\" & sGUID & ".zip"

                        Dim zip As New ZipFile

                        zip.AddDirectory(sTempFilePath)
                        zip.TempFileFolder = sTempFileLocation & "\"
                        zip.Save(sZipFilePath)
                        zip.Dispose()


                        ''Use the UniqueID for the Zip folder and file names
                        'duz1.ZipSubOptions = CDZipNET.ZIPSUBOPTION.ZSO_RELATIVEPATHFLAG
                        'duz1.TempPath = sTempFileLocation & "\"
                        'duz1.ZIPFile = v_sTempFileLocation & "\" & sGUID & ".zip"
                        'duz1.ItemList = sTempFileLocation & "\*.*"
                        'duz1.ExcludeFollowing = sTempFileLocation
                        'duz1.ExcludeFollowingFlag = True
                        'duz1.RecurseFlag = True
                        'duz1.LargeZIPFilesFlag = True
                        'duz1.NoDirectoryEntriesFlag = False
                        'duz1.NoDirectoryNamesFlag = False
                        ''Invoke the zip method
                        'duz1.ActionDZ = CDZipNET.DZACTION.ZIP_ADD

                        'Create a unique zip file from the byte array retrieved from the Web Service
                        Try
                            Dim fsInputFile As IO.FileStream = New IO.FileStream(sZipFilePath, FileMode.Open, FileAccess.Read)
                            Dim bytes() As Byte = New Byte((fsInputFile.Length) - 1) {}
                            fsInputFile.Read(bytes, 0, fsInputFile.Length)
                            fsInputFile.Close()

                            ''Remove zip file, as it been unzipped
                            IO.File.Delete(sZipFilePath)

                            .DocumentTemplate = bytes
                        Catch ex As Exception

                        End Try
                    End If

                    .DocumentTemplateCode = RTrim(v_sDocumentTemplateCode)

                    If v_iDocumentTemplateKey <> 0 Then
                        .DocumentTemplateKey = v_iDocumentTemplateKey
                        .DocumentTemplateKeySpecified = True
                    Else
                        .DocumentTemplateKeySpecified = False
                    End If
                    'Checking the Document Type
                    Select Case v_oDocumentFormatType
                        Case DocumentFormatType.HTML
                            .DocumentTemplateFormat = DocumentFormatType.HTML
                            .DocumentTemplateFormatSpecified = True
                        Case DocumentFormatType.PDF
                            .DocumentTemplateFormat = DocumentFormatType.PDF
                            .DocumentTemplateFormatSpecified = True
                        Case Else
                            .DocumentTemplateFormatSpecified = False
                    End Select

                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                End With


                Using trace As New Tracer(Category.Trace)
                    oUpdateStandardWordingTemplateResponse = oSAM.UpdateStandardWordingTemplate(oUpdateStandardWordingTemplateRequest)
                End Using


                With oUpdateStandardWordingTemplateResponse

                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        oDocumentTemplate.Code = .NewDocumentTemplateCode
                        oDocumentTemplate.DocumentTemplateId = .NewDocumentTemplateKey
                        oDocumentTemplate.Description = .NewDocumentTemplateDescription
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("UpdateStandardWordingTemplate executed ok" & vbCrLf)

                    sbLogMessage.AppendLine("Input:" & vbCrLf)

                    If IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("Branch Code : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Branch Code : " & v_sBranchCode.ToString & vbCrLf)
                    End If

                    If IsNothing(v_iDocumentTemplateKey) Then
                        sbLogMessage.AppendLine("Document Template Key : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Document Template Key : " & v_iDocumentTemplateKey.ToString & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Output : " & vbCrLf)

                    If IsNothing(oDocumentTemplate.DocumentTemplateId) Then
                        sbLogMessage.AppendLine("Document Template Key : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Document Template Key : " & oDocumentTemplate.DocumentTemplateId.ToString & vbCrLf)
                    End If

                    If IsNothing(oDocumentTemplate.Code) Then
                        sbLogMessage.AppendLine("Document Template Code : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Document Template Code : " & oDocumentTemplate.Code & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oUpdateStandardWordingTemplateRequest = Nothing
                oUpdateStandardWordingTemplateResponse = Nothing
            End Try


            Return oDocumentTemplate
        End SyncLock
    End Function
End Class
