' NOTE: Code common to all background job types is located in this file. Code specific to one particular job type or
' processing step is located in other files for the same class.

Imports SharedFiles
Imports System.Data
Imports System.Configuration
Imports Sirius.Architecture.Data

''' <summary>
''' Each instance of this class represents one processing thread.
''' </summary>


Public NotInheritable Class BackgroundJobProcess

#Region "Private Constants"

    ' This is the background job type set in the xml @ \\BACKGROUND_JOB\JOB\[@jobtype="xxxxx"]
    Private Const BackgroundJobTypeDOCUPACK As String = "DOCUPACK"
    Private Const BackgroundJobTypeEXWRKITEM As String = "EXWRKITEM"

    'Printing Modes
    Private Const ACNormalMode = 0
    Private Const ACMergeMode = 1
    Private Const ACPrintMode = 2
    Private Const ACPrintSilentMode = 3
    Private Const ACSpoolDocMode = 4
    Private Const ACSpoolReportMode = 5
    Private Const ACViewMode = 6             ' To support view only mode
    Private Const ACSwiftEditMode = 7
    Private Const ACEmailMode = 8            ' To Support Email- Renewals Back Office    
    Private Const ACUserChoice = 15          ' To Support User-Choice
    ''' <summary>
    ''' this constant are used to hold the index for keyName in the Array
    ''' </summary>
    ''' <remarks></remarks>
    Public Const kPMENavLetGetKeyNameColPosition As Integer = 0
    ''' <summary>
    ''' this constant are used to hold the index for keyValue in the Array
    ''' </summary>
    ''' <remarks></remarks>
    Public Const kPMENavLetGetKeyColPosition As Integer = 1

#End Region

#Region "Private Shared Fields"

    ' Lock object for the _stopProcess variable.
    Private Shared ReadOnly _stopProcessLock As New Object

    ' Shared between all threads, access is synchronised.
    Private Shared _stopProcess As Boolean = False
    ''' <summary>
    ''' Used to set the max no of retry
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared _nServiceRetryLimitForEXWRKITEMValue As New Integer
    Private Shared _nWaitMiuntesBeforeRetry As New Integer


#End Region

#Region "Private Fields"

    ' Private to each thread.
    Private _info As BackgroundJobInfo

#End Region

#Region "Constructors"

    ''' <summary>
    ''' Create a new instance of this class, specifying all necessary configuration data.
    ''' </summary>
    ''' <param name="info">Configuration data to use.</param>
    Public Sub New(ByVal info As BackgroundJobInfo)

        ' Take a copy so that the data does not have to be synchronised between threads.
        _info = info.Clone()

    End Sub

#End Region

#Region "Public Shared Properties"

    ''' <summary>
    ''' Set this property to true to signal all running instances of this class to start the
    ''' shutdown process.
    ''' </summary>
    Public Shared Property StopProcess() As Boolean
        Get
            SyncLock _stopProcessLock
                Return _stopProcess
            End SyncLock
        End Get
        Set(ByVal value As Boolean)
            SyncLock _stopProcessLock
                _stopProcess = value
            End SyncLock
        End Set
    End Property
    ''' <summary>
    ''' Set this property to maximum no of retry limit.
    ''' </summary>
    Public Shared Property ServiceRetryLimitForEXWRKITEM() As Integer
        Get
            Return _nServiceRetryLimitForEXWRKITEMValue
        End Get
        Set(ByVal value As Integer)
            _nServiceRetryLimitForEXWRKITEMValue = value
        End Set
    End Property

    Public Shared Property WaitMiuntesBeforeRetry() As Integer
        Get
            Return _nWaitMiuntesBeforeRetry
        End Get
        Set(ByVal value As Integer)
            _nWaitMiuntesBeforeRetry = value
        End Set
    End Property

#End Region

#Region "Public Methods - Processing Entry Points"

    ''' <summary>
    ''' Start processing data. This method runs in an infinite loop and can only be stopped by
    ''' setting the shared <see cref="StopProcess"/> property to true. When this is done, the code
    ''' will detect it at the first available opportunity, then proceed to shut itself down
    ''' and exit this method.
    ''' </summary>
    ''' <remarks>
    ''' This method is intended to be the thread start method of a background worker thread.
    ''' It will hang the thread that calls it, so it can only be stopped from a different thread.
    ''' </remarks>
    Public Sub StartProcessing()

        Log.WriteInfo("Thread is starting.")

        'System.Diagnostics.Debugger.Break()

        Try
            ' Run continously until the thread is told to stop.
            Do While ProcessAllBackgroundJobs()
            Loop
            Log.WriteInfo("Thread is terminating.")
        Catch ex As Exception
            If ExceptionPolicy.HandleException(ex, "Log and Absorb") Then
                Throw
            End If
        End Try

    End Sub

    ''' <summary>
    ''' Carry out all background jobs processing.
    ''' </summary>
    ''' <returns>True if the thread should continue processing, or false if a stop request has been received.</returns>
    ''' <remarks>
    ''' IMPORTANT: This method would normally be private. It is exposed as public ONLY so that
    ''' test harnesses can call it in order to perform internal operations and see internal state.
    ''' </remarks>
    <EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function ProcessAllBackgroundJobs() As Boolean

        ' Process all background jobs
        If Not ProcessAllJobs(_info.Database) Then
            Return False
        End If

        ' Sleep until it time to poll again.
        Log.WriteDebug("ProcessAllBackgroundJobs - Waiting for next polling period.")
        Dim timePeriod As Date = Date.Now + _info.PollingDelay
        While timePeriod > Date.Now
            ' During the wait period need to check to see if the service has been told to stop
            Thread.Sleep(1000)
            If BackgroundJobProcess.StopProcess Then
                Return False
            End If
        End While

        Return True

    End Function

    ''' <summary>
    ''' Carry out all processing of outstanding background jobs in the specified database exactly once.
    ''' </summary>
    ''' <param name="database">Database to act on.</param>
    ''' <returns>True if the thread should continue processing, or false if a stop request has been received.</returns>
    ''' <remarks>
    ''' IMPORTANT: This method would normally be private. It is exposed as public ONLY so that
    ''' test harnesses can call it in order to perform internal operations and see internal state.
    ''' </remarks>
    <EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function ProcessAllJobs(ByVal database As Database) As Boolean
        Log.WriteDebug(database, "ProcessAllJobs - start")

        ' Has the thread been told to stop?
        If BackgroundJobProcess.StopProcess Then
            Log.WriteDebug(database, "ProcessAllJobs - StopProcess = True")
            Return False
        End If

        ' Obtain the next bacth of outstanding jobs info from the database.
        Dim backgroundJob As BackgroundJobData = Nothing
        Dim bReturn As Boolean = False

        'Initializing the AppConfic value
        Try

            Try
                Log.WriteDebug(database, "Stored Procedure spu_SIR_Background_Job_GetNext - before")
                backgroundJob = GetNextJobToProcess(database, _info.Source, WaitMiuntesBeforeRetry)
                Log.WriteDebug(database, "Stored Procedure spu_SIR_Background_Job_GetNext - after")
            Catch ex As Exception
                ' Log the error then absorb it. Errors in one sub-process should not affect other sub-processes.
                Log.WriteDebug(database, "Stored Procedure spu_SIR_Background_Job_GetNext - error")
                If ExceptionPolicy.HandleException(ex, "Log and Absorb") Then
                    Throw
                End If
            End Try
            If backgroundJob Is Nothing Then
                ' There is no job available in the table, so just return true and exit safely
                Log.WriteDebug(database, "ProcessAllJobs - No jobs exits. So exit")
                Return True
            End If

            ' Process the background job.
            ' FUTURE_EXPANSION: Add new background job types here.
            Select Case backgroundJob.Type
                Case BackgroundJobTypeDOCUPACK
                    Log.WriteDebug(database, "ProcessAllJobs.ProcessBackgroundJobDOCUPACK - before")
                    bReturn = ProcessBackgroundJobDOCUPACK(database, backgroundJob)
                    Log.WriteDebug(database, "ProcessAllJobs.ProcessBackgroundJobDOCUPACK - after")
                    If bReturn = True And backgroundJob.ErrorDetails Is Nothing Then
                        Log.WriteDebug(database, "ProcessAllJobs.SetBackgroundJobAsComplete - before")
                        SetBackgroundJobAsComplete(database, backgroundJob.ID)
                        Log.WriteDebug(database, "ProcessAllJobs.SetBackgroundJobAsComplete - after")
                    Else
                        Log.WriteDebug(database, "ProcessAllJobs.ProcessBackgroundJobDOCUPACK - failed")
                        Log.WriteDebug(database, "ProcessAllJobs.SetBackgroundJobAsFailed - before")
                        SetBackgroundJobAsFailed(database, backgroundJob.ID, backgroundJob.ErrorDetails)
                        Log.WriteDebug(database, "ProcessAllJobs.SetBackgroundJobAsFailed - after")
                    End If
                Case BackgroundJobTypeEXWRKITEM
                    Log.WriteDebug(database, "ProcessAllJobs.ProcessBackgroundJobDOCUPACK - before")
                    bReturn = ProcessBackgroundJobEXTWRKITEM(database, backgroundJob)
                    Log.WriteDebug(database, "ProcessAllJobs.ProcessBackgroundJobDOCUPACK - after")

                    If bReturn = True And backgroundJob.ErrorDetails Is Nothing Then
                        Log.WriteDebug(database, "ProcessAllJobs.SetBackgroundJobAsComplete - before")
                        SetBackgroundJobAsComplete(database, backgroundJob.ID)
                        Log.WriteDebug(database, "ProcessAllJobs.SetBackgroundJobAsComplete - after")
                    Else

                        Log.WriteDebug(database, "ProcessAllJobs.ProcessBackgroundJobDOCUPACK - failed")
                        Log.WriteDebug(database, "ProcessAllJobs.SetBackgroundJobAsFailed - before")

                        'it checks  the  no of retry limit to be ecall the service
                        If ValidateRetryCount(database, backgroundJob.ID) <> 1 Then
                            SetBackgroundJobAsFailed(database, backgroundJob.ID, backgroundJob.ErrorDetails)
                        End If

                        Log.WriteDebug(database, "ProcessAllJobs.SetBackgroundJobAsFailed - after")
                    End If
                Case Else
                    Log.WriteDebug(database, "ProcessAllJobs.SetBackgroundJobAsFailed as unknown JobType - before")
                    SetBackgroundJobAsFailed(database, backgroundJob.ID, backgroundJob.Type + " not supported.")
                    Log.WriteDebug(database, "ProcessAllJobs.SetBackgroundJobAsFailed as unknown JobType - after")
            End Select

        Catch ex As Exception
            ' Log the error then absorb it. Errors in one job should not affect other background jobs in the database.
            ' The same error will probably occur every time the thread cycles round again, but that's no
            ' better or worse than aborting the thread entirely.
            Log.WriteDebug(database, "ProcessAllJobs - error")

            backgroundJob.ErrorDetails = ex.Message
            Log.WriteDebug(database, "ProcessAllJobs.SetBackgroundJobAsFailed - before")
            SetBackgroundJobAsFailed(database, backgroundJob.ID, backgroundJob.ErrorDetails)
            Log.WriteDebug(database, "ProcessAllJobs.SetBackgroundJobAsFailed - after")

            If ExceptionPolicy.HandleException(ex, "Log and Absorb") Then
                Throw
            End If

        Finally
            Log.WriteDebug(database, "ProcessAllJobs - exiting")
        End Try

        Return True

    End Function
#End Region

#Region "Private Methods"

    ''' <summary>
    ''' Process a DOCUPACK background Job.
    ''' </summary>
    ''' <param name="database">Database to act on.</param>
    ''' <param name="backgroundJob">Background Job row to act on.</param>
    ''' <returns>True if the thread should continue processing, or false if a stop request has been received.</returns>
    Private Function ProcessBackgroundJobDOCUPACK(ByVal database As Database,
                                                  ByRef backgroundJob As BackgroundJobData) As Boolean

        Dim bReturnValue As Boolean = False
        Dim typeOfAttachment As String = String.Empty
        Dim sEmailFilePath As String = String.Empty
        Log.WriteDebug(database, "ProcessBackgroundJobDOCUPACK - start")

        If BackgroundJobProcess.StopProcess Then
            RollbackBackgroundJob(database, backgroundJob)
            ' Exit the main loop allowing the thread to terminate.
            Return bReturnValue
        End If

        ' Process the job
        ' Add code to do the processing here
        Dim _SiriusUser As New SIRIUSUSER
        bReturnValue = GetJobUserDetails(backgroundJob.JobUserID, _SiriusUser)

        Dim TempFiles As New List(Of String)
        bReturnValue = GetJobUserDetails(backgroundJob.JobUserID, _SiriusUser)
        Try

            ' Parse the job xml to get all the required parameters such as
            '   Destination         - EMail / Spooler / Archive
            '   PartyCnt            - Integer
            '   InsuranceFileCnt    - Integer
            '   ClaimID             - Integer
            '   EMailTo             - String
            '   EmailCC             - String
            '   EmailBCC            - String
            '   EMailSubject        - String
            '   mainTemplate   - Document Template code which needs to be resolved
            '   Lists of (Item\Code)- Document Template code(s) which should be resolved & attached as attachments.
            Log.WriteDebug(database, "ProcessBackgroundJobDOCUPACK.ParseDOCUPACKJobXML - before")
            Dim destination As String = String.Empty
            Dim partyCnt As Integer = 0
            Dim insuranceFileCnt As Integer = 0
            Dim claimID As Integer = 0
            Dim emailTo As String = String.Empty
            Dim emailCC As String = String.Empty
            Dim emailBCC As String = String.Empty
            Dim emailSubject As String = String.Empty
            Dim mainTemplate As String = String.Empty
            Dim mainPath As String = String.Empty
            Dim emailAttachments As New List(Of String)
            Dim emailBodyHTMLLocation As String = String.Empty

            Dim mainArchive As Boolean = False
            Dim mainInternalOnly As Boolean = False
            Dim mainDocumentTemplateGroupID As Integer = 0
            Dim mainDocumentTemplateSubGroupID As Integer = 0
            Dim mainDocumentOutputFormat As String = String.Empty
            Dim mainDocumentDestinationFilename As String = String.Empty
            Dim bCreateFolderStructure As Boolean = False
            Dim lMode As Integer
            Dim archiveWithNoMerge As Boolean = False
            Dim bIsDMEMigration As Boolean = False
            Dim sCreatedDate As String = String.Empty
            Dim sDocumentRef As String = String.Empty

            Dim settings As New XmlReaderSettings
            settings.ValidationType = ValidationType.None

            Dim doc As XmlDocument = New XmlDocument()
            doc.Load(XmlReader.Create(New StringReader(backgroundJob.Xml), settings))

            'Create an XmlNamespaceManager for resolving namespaces.
            Dim nsmgr As XmlNamespaceManager = New XmlNamespaceManager(doc.NameTable)
            nsmgr.AddNamespace("xs", String.Empty)

            Dim nodeList As XmlNodeList
            Dim root As XmlElement = doc.DocumentElement
            Dim paramterNode As XmlNode = Nothing
            Dim parameterName As String = String.Empty
            Dim parameterValue As String = String.Empty

            ' Get all the DOCUPACKJob Parameters
            nodeList = root.SelectNodes("/BACKGROUND_JOB/JOB/PARAMETERS/PARAMETER", nsmgr)
            For Each paramterNode In nodeList
                parameterValue = ""
                If Not paramterNode.Attributes.ItemOf("name") Is Nothing Then
                    parameterName = paramterNode.Attributes.ItemOf("name").Value.ToLower
                End If
                If Not paramterNode.Attributes.ItemOf("value") Is Nothing Then
                    parameterValue = paramterNode.Attributes.ItemOf("value").Value
                End If
                Select Case parameterName
                    Case "destination"
                        destination = parameterValue
                    Case "emailto"
                        emailTo = parameterValue
                    Case "emailcc"
                        emailCC = parameterValue
                    Case "emailbcc"
                        emailBCC = parameterValue
                    Case "emailsubject"
                        emailSubject = parameterValue
                    Case "code"
                        mainTemplate = parameterValue
                    Case "path"
                        mainPath = parameterValue
                    Case "partycnt"
                        partyCnt = XmlSafeConvert.ToInt32(parameterValue, 0)
                    Case "insurancefilecnt"
                        insuranceFileCnt = XmlSafeConvert.ToInt32(parameterValue, 0)
                    Case "claimid"
                        claimID = XmlSafeConvert.ToInt32(parameterValue, 0)
                    Case "archive"
                        mainArchive = XmlSafeConvert.ToBoolean(parameterValue, 0)
                    Case "internalonly"
                        mainInternalOnly = XmlSafeConvert.ToBoolean(parameterValue, 0)
                    Case "documenttemplategroupid"
                        mainDocumentTemplateGroupID = XmlSafeConvert.ToInt32(parameterValue, 0)
                    Case "documenttemplatesubgroupid"
                        mainDocumentTemplateSubGroupID = XmlSafeConvert.ToInt32(parameterValue, 0)
                    Case "destinationfilename"
                        'Document wrapper check length to avoid failure where i stored "0"
                        If parameterValue = "0" Then
                            mainDocumentDestinationFilename = ""
                        Else
                            mainDocumentDestinationFilename = parameterValue
                        End If

                    Case "outputformat"
                        mainDocumentOutputFormat = parameterValue.ToUpper
                    Case "type"
                        typeOfAttachment = parameterValue
                    Case "createfolderstructure"
                        bCreateFolderStructure = True
                    Case "bisdmemigration"
                        If parameterValue.ToUpper = "TRUE" Then
                            bIsDMEMigration = True
                        Else
                            bIsDMEMigration = False
                        End If
                    Case "screateddate"
                        sCreatedDate = parameterValue
                    Case "documentref"
                        sDocumentRef = parameterValue
                    Case Else       ' Add more parameters (if any here)
                        ' Add error handler code
                End Select
            Next

            Select Case destination.ToLower
                Case "email"
                    lMode = ACEmailMode
                Case "spooler"
                    lMode = ACSpoolDocMode
                Case "archive"
                    lMode = ACUserChoice
                Case Else
                    ' Handle the mode here...
            End Select

            ' Get all the DOCUPACKJob Item(s)
            Dim itemNode As XmlNode
            Dim documentTemplateCode As String = String.Empty
            Dim documentTemplatePath As String = String.Empty
            Dim documentInternalOnly As Boolean = False
            Dim documentArchive As Boolean = False
            Dim documentDestinationFilename As String = String.Empty

            Dim parameterList As XmlNodeList
            Dim iNode As Integer

            If bCreateFolderStructure Then

                Dim oSharePointOnline As Object = CreateLateBoundObject("bSIRSharePointOnline.BusinessSharepointOnline")

                Dim nReturn As Integer = oSharePointOnline.Initialise(sUsername:=_SiriusUser.Username,
                                           sPassword:=_SiriusUser.Password,
                                           iUserID:=_SiriusUser.UserID,
                                           iSourceID:=_SiriusUser.SourceID,
                                           iLanguageID:=_SiriusUser.LanguageID,
                                           iCurrencyID:=_SiriusUser.CurrencyID,
                                           iLogLevel:=SiriusUserDefaults.LogLevel,
                                           sCallingAppName:=SiriusUserDefaults.AppName)

                nReturn = oSharePointOnline.GenerateDefaultPathForSharePointOnline(nPartyCnt:=partyCnt, nInsuranceFileCnt:=insuranceFileCnt, nClaimID:=claimID, nCaseID:=0)

                Return nReturn
            End If



            nodeList = root.SelectNodes("/BACKGROUND_JOB/JOB/ITEMS/ITEM", nsmgr)

            For Each itemNode In nodeList
                Dim documentTemplateGroupID As Integer = mainDocumentTemplateGroupID
                Dim documentTemplateSubGroupID As Integer = mainDocumentTemplateSubGroupID
                Dim documentOutputFormat As String = mainDocumentOutputFormat
                Dim partyCntLocal As Integer = partyCnt
                Dim insuranceFileCntLocal As Integer = insuranceFileCnt
                Dim claimIDLocal As Integer = claimID
                Dim bIsDMEMigrationLocal As Boolean = False
                Dim sCreatedDateLocal As String = String.Empty

                documentTemplatePath = String.Empty
                documentTemplateCode = String.Empty
                documentInternalOnly = False
                documentArchive = False
                documentDestinationFilename = String.Empty

                If Not itemNode.Attributes.ItemOf("code") Is Nothing Then
                    documentTemplateCode = itemNode.Attributes.ItemOf("code").Value
                End If
                If Not itemNode.Attributes.ItemOf("path") Is Nothing Then
                    documentTemplatePath = itemNode.Attributes.ItemOf("path").Value
                End If

                ' Resolve the document using the supplied parameters if the path is not supplied
                ' This means, this is a document which needs just attaching, doesn't required resolving.
                partyCntLocal = partyCnt                    ' default to the supplied value
                insuranceFileCntLocal = insuranceFileCnt    ' default to the supplied value
                claimIDLocal = claimID                      ' default to the supplied value

                ' Get document specific parameters (if any)
                parameterList = itemNode.SelectNodes("PARAMETERS/PARAMETER", nsmgr)
                For iNode = 0 To parameterList.Count - 1
                    If Not parameterList(iNode).Attributes.ItemOf("name") Is Nothing Then
                        parameterName = parameterList(iNode).Attributes.ItemOf("name").Value.ToLower
                    End If
                    If Not parameterList(iNode).Attributes.ItemOf("value") Is Nothing Then
                        parameterValue = parameterList(iNode).Attributes.ItemOf("value").Value
                    End If
                    Select Case parameterName
                        Case "partycnt"
                            partyCntLocal = XmlSafeConvert.ToInt32(parameterValue, 0)
                        Case "insurancefilecnt"
                            insuranceFileCntLocal = XmlSafeConvert.ToInt32(parameterValue, 0)
                        Case "claimid"
                            claimIDLocal = XmlSafeConvert.ToInt32(parameterValue, 0)
                        Case "internalonly"
                            documentInternalOnly = XmlSafeConvert.ToBoolean(parameterValue, 0)
                        Case "archive"
                            documentArchive = XmlSafeConvert.ToBoolean(parameterValue, 0)
                        Case "documenttemplategroupid"
                            documentTemplateGroupID = XmlSafeConvert.ToInt32(parameterValue, 0)
                        Case "documenttemplatesubgroupid"
                            documentTemplateSubGroupID = XmlSafeConvert.ToInt32(parameterValue, 0)
                        Case "destinationfilename"
                            documentDestinationFilename = parameterValue
                        Case "outputformat"
                            documentOutputFormat = parameterValue.ToUpper
                        Case "bIsDMEMigration"
                            If parameterValue.ToUpper = "TRUE" Then
                                bIsDMEMigrationLocal = True
                            Else
                                bIsDMEMigrationLocal = False
                            End If
                        Case "screateddate"
                            sCreatedDateLocal = parameterValue
                    End Select
                Next

                ' Check if the document template code exists first
                If documentTemplatePath = "" And documentTemplateCode <> "" Then
                    If lMode <> ACUserChoice AndAlso Not IsDocumentTemplateExists(database, documentTemplateCode, DateTime.Now, insuranceFileCntLocal, claimIDLocal) Then
                        backgroundJob.ErrorDetails = "ProcessBackgroundJobDOCUPACK - Missing document template in the database. DocumentTemplateCode [" + documentTemplateCode + "]."
                        Log.WriteDebug(database, backgroundJob.ErrorDetails)
                        Return bReturnValue
                    End If
                End If
                bReturnValue = True

                If lMode <> ACEmailMode OrElse (lMode = ACEmailMode AndAlso documentTemplateCode.Trim <> "") Then
                    ' Resolve the document template as pdf
                    Log.WriteDebug(database, "ProcessBackgroundJobDOCUPACK.GenerateDocument - " + documentTemplateCode + " - before")
                    bReturnValue = GenerateDocument(database:=database, _SiriusUser:=_SiriusUser, generateMode:=lMode,
                                    documentTemplateCode:=documentTemplateCode, partyCnt:=partyCntLocal,
                                    insuranceFileCnt:=insuranceFileCntLocal, claimID:=claimIDLocal,
                                    backgroundJob:=backgroundJob,
                                    mergedPath:=documentTemplatePath,
                                    documentTemplateGroupID:=documentTemplateGroupID,
                                    documentTemplateSubGroupID:=documentTemplateSubGroupID,
                                    archiveWithNoMerge:=documentArchive,
                                    internalOnly:=documentInternalOnly,
                                    outputFormat:=documentOutputFormat,
                                        documentDestinationFilename:=documentDestinationFilename,
                                        IsDMEMigration:=bIsDMEMigrationLocal,
                                        sCreatedDate:=sCreatedDateLocal,
                                        documentRef:=sDocumentRef)
                End If


                If bReturnValue Then
                    Log.WriteDebug(database, "ProcessBackgroundJobDOCUPACK.GenerateDocument - " + documentTemplateCode + " - after")

                    If lMode = ACEmailMode Then
                        ' Append it to the emailAttachments arrayList
                        If documentDestinationFilename <> "" And IO.Path.GetExtension(documentDestinationFilename) = IO.Path.GetExtension(documentTemplatePath) Then
                            Dim renamedPath As String = String.Empty

                            renamedPath = IO.Path.Combine(IO.Path.GetDirectoryName(documentTemplatePath), documentDestinationFilename)
                            If Not IO.File.Exists(renamedPath) Then
                                IO.File.Move(documentTemplatePath, renamedPath)
                                documentTemplatePath = renamedPath
                            End If
                        ElseIf documentDestinationFilename <> "" Then
                            'Convert to the requested format
                            Dim convertedPath As String
                            convertedPath = documentTemplatePath.Substring(0, documentTemplatePath.Length - IO.Path.GetExtension(documentTemplatePath).Length) & IO.Path.GetExtension(documentDestinationFilename)

                            ConvertDocumentUsingSiriusDocumentUtility(v_sSourceDocument:=documentTemplatePath, v_sDestDocument:=convertedPath)
                            documentTemplatePath = convertedPath

                        End If

                        emailAttachments.Add(documentTemplatePath)
                        TempFiles.Add(documentTemplatePath)
                    End If
                Else
                    backgroundJob.ErrorDetails = backgroundJob.ErrorDetails & " - ProcessBackgroundJobDOCUPACK.GenerateDocument - " + documentTemplateCode + " - failed."
                    Log.WriteDebug(database, backgroundJob.ErrorDetails)
                    Return bReturnValue
                End If
            Next
            Log.WriteDebug(database, "ProcessBackgroundJobDOCUPACK.ParseDOCUPACKJobXML - after")

            ' Resolve the Main body template using document production system
            ' Get back the HTML path
            Log.WriteDebug(database, "ProcessBackgroundJobDOCUPACK.GenerateDocument - " + mainTemplate + " - before")
            If GenerateDocument(database:=database, _SiriusUser:=_SiriusUser, generateMode:=lMode,
                                    documentTemplateCode:=mainTemplate, partyCnt:=partyCnt,
                                    insuranceFileCnt:=insuranceFileCnt, claimID:=claimID,
                                    backgroundJob:=backgroundJob,
                                    mergedPath:=mainPath,
                                    documentTemplateGroupID:=mainDocumentTemplateGroupID,
                                    documentTemplateSubGroupID:=mainDocumentTemplateSubGroupID,
                                    archiveWithNoMerge:=mainArchive,
                                    internalOnly:=mainInternalOnly,
                                    outputFormat:=mainDocumentOutputFormat,
                                    documentDestinationFilename:=mainDocumentDestinationFilename,
                                    IsDMEMigration:=bIsDMEMigration,
                                    sCreatedDate:=sCreatedDate) Then
                Log.WriteDebug(database, "ProcessBackgroundJobDOCUPACK.GenerateDocument - " + mainTemplate + " - after")

                If lMode = ACEmailMode Then
                    Dim lReturn As Integer


                    Log.WriteDebug(database, String.Format("ProcessBackgroundJobDOCUPACK.Invoking SendEmail for background job ID {0}.", backgroundJob.ID))
                    lReturn = SendEmail(database:=database,
                                        _SiriusUser:=_SiriusUser,
                                        emailTo:=emailTo,
                                        emailCC:=emailCC,
                                        emailBCC:=emailBCC,
                                        emailSubject:=emailSubject,
                                        emailBodyHTMLLocation:=mainPath,
                                        emailAttachments:=emailAttachments,
                                        emailCreateArchive:=mainArchive,
                                        emailGeneratedFile:=sEmailFilePath,
                                        iInsuranceFileCnt:=insuranceFileCnt)

                    If (lReturn = PMEReturnCode.PMTrue) Then
                        Log.WriteDebug(database, String.Format("ProcessBackgroundJobDOCUPACK.SendEmail method succeeded for background job ID {0}.", backgroundJob.ID))

                        'Do we archive?
                        If mainArchive Then
                            Log.WriteDebug(database, String.Format("ProcessBackgroundJobDOCUPACK.Archive of Email Started for background job ID {0}.", backgroundJob.ID))
                            If GenerateDocument(database:=database, _SiriusUser:=_SiriusUser,
                                                     generateMode:=ACUserChoice, partyCnt:=partyCnt,
                                                     insuranceFileCnt:=insuranceFileCnt, claimID:=claimID,
                                                     backgroundJob:=backgroundJob,
                                                     documentTemplateCode:="",
                                                     mergedPath:=sEmailFilePath,
                                                     documentTemplateGroupID:=mainDocumentTemplateGroupID,
                                                     documentTemplateSubGroupID:=mainDocumentTemplateSubGroupID,
                                                     internalOnly:=mainInternalOnly, archiveWithNoMerge:=True, isGeneratedEmail:=True,
                                                     IsDMEMigration:=bIsDMEMigration,
                                                     sCreatedDate:=sCreatedDate) Then


                                Log.WriteDebug(database, String.Format("ProcessBackgroundJobDOCUPACK.Archive of Email Started for background job ID {0}.", backgroundJob.ID))
                            Else
                                backgroundJob.ErrorDetails = backgroundJob.ErrorDetails & String.Format(" - ProcessBackgroundJobDOCUPACK.SendEmail Failed for background job id {0}.", backgroundJob.ID)
                                Log.WriteDebug(database, backgroundJob.ErrorDetails)
                                Return bReturnValue
                            End If
                        End If
                        'Folder clean up
                        Log.WriteDebug(database, String.Format("ProcessBackgroundJobDOCUPACK.CleanUpTemporaryFolders Started for background job ID {0}.", backgroundJob.ID))
                        TempFiles.Add(mainPath)
                        bReturnValue = True ' Everything is fine

                    Else
                        backgroundJob.ErrorDetails = backgroundJob.ErrorDetails & String.Format(" - ProcessBackgroundJobDOCUPACK.SendEmail Failed for background job id {0}.", backgroundJob.ID)
                        Log.WriteDebug(database, backgroundJob.ErrorDetails)
                        Return bReturnValue
                    End If
                Else
                    bReturnValue = True
                End If
            Else
                backgroundJob.ErrorDetails = "ProcessBackgroundJobDOCUPACK.GenerateDocument - " + mainTemplate + " - failed."
                Log.WriteDebug(database, backgroundJob.ErrorDetails)
                Return bReturnValue
            End If

        Catch ex As Exception

            backgroundJob.ErrorDetails = Formatter.Format(ex)

            ' Log the error then absorb it. Errors in one message should not affect other messages.
            Log.WriteDebug(database, String.Format("ProcessBackgroundJobDOCUPACK failed to process background job id {0}.", backgroundJob.ID))

            If ExceptionPolicy.HandleException(ex, "Log and Absorb") Then
                Throw
            End If

        Finally

            ' Cleanup directories
            If typeOfAttachment <> "report" Then
                If CleanUpTemporaryFolders(database, TempFiles) Then
                    Log.WriteDebug(database, String.Format("ProcessBackgroundJobDOCUPACK.CleanUpTemporaryFolders succeeded for background job ID {0}.", backgroundJob.ID))
                Else
                    Log.WriteDebug(database, String.Format("ProcessBackgroundJobDOCUPACK.CleanUpTemporaryFolders failed for background job ID {0}.", backgroundJob.ID))
                End If
                'If email Path exist then Delete it too.
                If IsFileExists(sEmailFilePath) Then
                    bReturnValue = bPMDocFunctions.DeleteFile(sEmailFilePath)
                    If bReturnValue <> True Then
                        Log.WriteDebug(database, String.Format("ProcessBackgroundJobDOCUPACK.DeleteFile failed for background job ID {0}.", backgroundJob.ID))
                    End If
                End If
            End If
            Log.WriteDebug(database, "ProcessBackgroundJobDOCUPACK - exiting")

        End Try

        Return bReturnValue

    End Function

    ''' <summary>
    ''' Roll back the background job status.
    ''' </summary>
    ''' <param name="database">Database to act on.</param>
    ''' <param name="backgroundJob">Background job row to act on.</param>
    Private Sub RollbackBackgroundJob(ByVal database As Database,
                                      ByVal backgroundJob As BackgroundJobData)

        Log.WriteDebug(database, "RollbackBackgroundJob - Thread is rolling back the background job row.")

        ' reset its status, ready to be processed next time.

        Try
            SetBackgroundJobStatus(database, backgroundJob.ID, "W")
        Catch ex As Exception
            ' Log the error then absorb it. Errors in one message should not affect other messages.
            Log.WriteDebug(database, "RollbackBackgroundJob - Failed to update the status to 'W' for background job ID {0}.", backgroundJob.ID)
            If ExceptionPolicy.HandleException(ex, "Log and Absorb") Then
                Throw
            End If
        End Try

    End Sub

    Private Function GenerateDocument(ByVal database As Database,
                                      ByVal _SiriusUser As SIRIUSUSER,
                                      ByVal generateMode As Integer,
                                      ByVal documentTemplateCode As String,
                                      ByVal partyCnt As Integer,
                                      ByVal insuranceFileCnt As Integer,
                                      ByVal claimID As Integer,
                                      ByVal backgroundJob As BackgroundJobData,
                                      ByRef mergedPath As String,
                                      Optional ByVal parameterXML As String = "",
                                      Optional ByVal outputAsHTML As Boolean = False,
                                      Optional ByVal outputFormat As String = "",
                                      Optional ByVal documentRef As String = "",
                                      Optional ByVal documentTemplateGroupID As Integer = 0,
                                      Optional ByVal documentTemplateSubGroupID As Integer = 0,
                                      Optional ByVal internalOnly As Boolean = False,
                                      Optional ByVal archiveWithNoMerge As Boolean = False,
                                      Optional ByVal documentDestinationFilename As String = "",
                                      Optional ByVal isGeneratedEmail As Boolean = False,
                                      Optional ByVal IsDMEMigration As Boolean = False,
                                      Optional ByVal sCreatedDate As String = "") As Boolean


        Log.WriteDebug(database, "GenerateDocument - Start")

        Dim oDocWrapper As bsirdocmanagerwrapper.Interface_Renamed = Nothing
        Dim iRet As Int32
        Dim bInitialised As Boolean = False

        Dim sZipFilePath As String = String.Empty
        Dim sMergedFilePath As String = String.Empty

        If generateMode = ACEmailMode And mergedPath.ToUpper.EndsWith(".HTML") Then
            'Skip the actual email body
            Return True
        End If

        Try
            oDocWrapper = New bsirdocmanagerwrapper.Interface_Renamed
        Catch ex As Exception
            ' Log the error then absorb it. Errors in one message should not affect other messages.
            Log.WriteDebug(database, "GenerateDocument - Failed to create bSIRDocManagerWrapper.Interface_Renamed Component.")
            If ExceptionPolicy.HandleException(ex, "Log and Absorb") Then
                Throw
            End If
        End Try

        Try
            Log.WriteDebug(database, "GenerateDocument.bSIRDocManagerWrapper.Interface_Renamed.Initialise - before")
            iRet = oDocWrapper.InitialiseBusiness(sUsername:=backgroundJob.LoggedUserName,
                                   sPassword:=backgroundJob.LoggedUserPassword,
                                   iUserID:=backgroundJob.LoggedUserID,
                                   iSourceID:=_SiriusUser.SourceID,
                                   iLanguageID:=_SiriusUser.LanguageID,
                                   iCurrencyID:=_SiriusUser.CurrencyID,
                                   iLogLevel:=SiriusUserDefaults.LogLevel,
                                   sCallingAppName:=SiriusUserDefaults.AppName)
            bInitialised = (iRet = PMEReturnCode.PMTrue)


            Dim mergeFilePathObject As Object = sMergedFilePath
            Dim zipFilePathObject As Object = sZipFilePath
            Dim convertedPath As String = String.Empty

            Dim insuranceFolderCnt As Integer = GetInsuranceFolderCnt(database, insuranceFileCnt)

            Log.WriteDebug(database, "GenerateDocument.bSIRDocManagerWrapper.Interface_Renamed.GenerateDocument - before")

            oDocWrapper.InsuranceFileCnt = insuranceFileCnt
            oDocWrapper.InsuranceFolderCnt = insuranceFolderCnt
            oDocWrapper.PartyCnt = partyCnt
            oDocWrapper.ClaimCnt = claimID
            oDocWrapper.DocumentRef = documentRef
            oDocWrapper.PartyCode = backgroundJob.PartyCode
            oDocWrapper.InsuranceFileRef = backgroundJob.InsuranceRef
            oDocWrapper.ClaimRef = backgroundJob.ClaimNumber
            oDocWrapper.Background_Job_Id = backgroundJob.ID
            oDocWrapper.RetainTempFiles = True
            'No need to to archive the document during the email process.
            If generateMode = ACEmailMode Then
                oDocWrapper.SkipArchiveOnEdit = True
            End If
            oDocWrapper.Mode = ACSpoolDocMode
            If String.IsNullOrEmpty(mergedPath) Then
                oDocWrapper.ArchiveDoc = archiveWithNoMerge
            Else
                oDocWrapper.ArchiveWithNoMerge = archiveWithNoMerge
            End If
            oDocWrapper.DocumentTemplateCode = documentTemplateCode
            oDocWrapper.DocumentTemplateGroupID = documentTemplateGroupID
            oDocWrapper.DocumentTemplateSubGroupID = documentTemplateSubGroupID
            oDocWrapper.ParameterXML = parameterXML
            oDocWrapper.OutputAsHTML = outputAsHTML
            oDocWrapper.OutputAsPDF = (outputFormat = "PDF")
            oDocWrapper.MergedFilePath = mergedPath
            oDocWrapper.DestinationFilename = documentDestinationFilename
            oDocWrapper.IsGeneratedMail = isGeneratedEmail
            oDocWrapper.IsCalledFromBackGroundJob = True
            oDocWrapper.IsDMEMigration = IsDMEMigration
            oDocWrapper.CreatedDate = sCreatedDate
            oDocWrapper.InternalOnly = internalOnly
            'Merge/Archive the document
            If documentTemplateCode <> "" Or generateMode <> ACEmailMode Then
                iRet = oDocWrapper.Start()
                If iRet <> PMEReturnCode.PMTrue Then
                    mergedPath = ""
                    Return False
                End If
                mergedPath = oDocWrapper.MergedFilePath
            End If




            'Convert to the requested format
            'If outputFormat <> "" And IO.Path.GetExtension(mergedPath).ToUpper <> "." & outputFormat Then
            '    convertedPath = mergedPath.Substring(0, mergedPath.Length - IO.Path.GetExtension(mergedPath).Length) & "." & outputFormat.ToLower

            '    ConvertDocumentUsingSiriusDocumentUtility(v_sSourceDocument:=mergedPath, v_sDestDocument:=convertedPath)
            '    mergedPath = convertedPath
            'End If
            Log.WriteDebug(database, "GenerateDocument.bSIRDocManagerWrapper.Interface_Renamed.GenerateDocument - after")

            Log.WriteDebug(database, "GenerateDocument.bSIRDocManagerWrapper.Interface_Renamed.GenerateDocument - Success")
            'sMergedFilePath = Cast.ToString(mergeFilePathObject, String.Empty)
            'mergedPath = sMergedFilePath    ' Return to the calling cdoe


        Catch ex As Exception
            If bInitialised = False Then
                Log.WriteDebug(database, "GenerateDocument - Failed to initialise bSIRDocManagerWrapper.Interface_Renamed Component.")
            End If
            Log.WriteDebug(database, "GenerateDocument - error." & ex.Message)
            backgroundJob.ErrorDetails = "GenerateDocument - error." & ex.Message
            If ExceptionPolicy.HandleException(ex, "Log and Absorb") Then
                Throw
            End If

        Finally
            ' Dispose of the business component safely.
            If oDocWrapper IsNot Nothing Then
                oDocWrapper.Dispose()
                oDocWrapper = Nothing
                Log.WriteDebug(database, "GenerateDocument.Dispose object bSIRDocManagerWrapper.Interface_Renamed - before")
            End If
        End Try

        If mergedPath.Length > 0 Then
            Log.WriteDebug(database, "GenerateDocument - Exit with Success")
            Return True ' everything is fine
        Else
            Log.WriteDebug(database, "GenerateDocument - Exit with Fail.")
            Return False
        End If

    End Function

    Private Function SendEmail(ByVal database As Database,
                               ByVal _SiriusUser As SIRIUSUSER,
                               ByVal emailTo As String,
                               ByVal emailCC As String,
                               ByVal emailSubject As String,
                               ByVal emailBodyHTMLLocation As String,
                               ByVal emailAttachments As List(Of String),
                               ByVal emailBCC As String,
                               ByVal emailCreateArchive As Boolean,
                               ByRef emailGeneratedFile As String,
                               ByVal iInsuranceFileCnt As Integer) As Integer

        Log.WriteDebug(database, "SendEmail - Start")

        Dim oDocTemplate As bSIRDocTemplate.Business = Nothing
        Dim iRet As Int32
        Dim iReturnValue As Int32
        Dim bInitialised As Boolean = False
        Dim sOptionValue As String = String.Empty
        Dim sZipFilePath As String = String.Empty
        Dim sMergedFilePath As String = String.Empty

        Try
            ' Instantiate the business component.
            Log.WriteDebug(database, "SendEmail - Create object bSIRDocTemplate.Business - before")
            oDocTemplate = New bSIRDocTemplate.Business
            ' Instantiate the business component.
            Log.WriteDebug(database, "SendEmail - Create object bSIRDocTemplate.Business - after")
        Catch ex As Exception
            ' Log the error then absorb it. Errors in one message should not affect other messages.
            Log.WriteDebug(database, "SendEmail - Failed to create bSIRDocTemplate.Business Component.")
            If ExceptionPolicy.HandleException(ex, "Log and Absorb") Then
                Throw
            End If
        End Try

        Try
            Log.WriteDebug(database, "SendEmail -  bSIRDocTemplate.Business.Initialise - before")
            iRet = oDocTemplate.Initialise(sUsername:=_SiriusUser.Username,
                                           sPassword:=_SiriusUser.Password,
                                           iUserID:=_SiriusUser.UserID,
                                           iSourceID:=_SiriusUser.SourceID,
                                           iLanguageID:=_SiriusUser.LanguageID,
                                           iCurrencyID:=_SiriusUser.CurrencyID,
                                           iLogLevel:=SiriusUserDefaults.LogLevel,
                                           sCallingAppName:=SiriusUserDefaults.AppName)
            bInitialised = (iRet = PMEReturnCode.PMTrue)

            oDocTemplate.IsNonBatchProcess = True

            iRet = oDocTemplate.GetPolicyLevelEmailAddress(iInsuranceFileCnt)
            If iRet = gPMConstants.PMEReturnCode.PMFalse Then
                Log.WriteDebug(database, "SendEmail - bSIRDocTemplate.Business.GetPolicyLevelEmailAddress - failed")
            End If

            oDocTemplate.IsNonBatchProcess = True

            Log.WriteDebug(database, "SendEmail - bSIRDocTemplate.Business.SendEmail - before")
            iReturnValue = oDocTemplate.SendEMail(v_sTo:=emailTo,
                                          sCC:=emailCC,
                                          v_sSubject:=emailSubject,
                                          v_sMessagePath:=emailBodyHTMLLocation,
                                          v_sAttachment:=emailAttachments.ToArray,
                                          sBCC:=emailBCC,
                                          bSaveEMLFile:=emailCreateArchive,
                                          sEMLFile:=emailGeneratedFile)

            If iReturnValue = PMEReturnCode.PMTrue Then
                Log.WriteDebug(database, "SendEmail - bSIRDocTemplate.Business.SendEmail - finished")
            Else
                Log.WriteDebug(database, "SendEmail - bSIRDocTemplate.Business.SendEmail - failed")
            End If

        Catch ex As Exception
            If bInitialised = False Then
                Log.WriteDebug(database, "SendEmail - Failed to initialise bSIRDocTemplate.Business Component.")
            End If
            If ExceptionPolicy.HandleException(ex, "Log and Absorb") Then
                Throw
            End If

        Finally
            ' Dispose of the business component safely.
            If oDocTemplate IsNot Nothing Then
                oDocTemplate.Dispose()
                'oDocTemplate = Nothing
                Log.WriteDebug(database, "SendEmail - Dispose object bSIRDocTemplate.Business - before")
            End If
        End Try

        Log.WriteDebug(database, "SendEmail - Exit")

        Return iReturnValue ' Return the value from the Send EMail function

    End Function

    Private Function CleanUpTemporaryFolders(ByVal database As Database,
                                             ByVal TempFiles As List(Of String)) As Boolean

        Log.WriteDebug(database, "CleanUpTemporaryFolders - Start")

        Dim sPath As String = String.Empty
        Dim bReturnValue As Boolean = False
        Dim oFileInfo As FileInfo

        Try

            For Each sPath In TempFiles
                My.Computer.FileSystem.DeleteFile(sPath)
            Next

            bReturnValue = True

        Catch ex As Exception

        End Try

        Log.WriteDebug(database, "CleanUpTemporaryFolders - Exit")

        Return bReturnValue

    End Function
    ''' <summary>
    ''' GetJobUserDetails
    ''' </summary>
    ''' <param name="NewUserID"></param>
    ''' <param name="_SiriusUser"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetJobUserDetails(ByVal NewUserID As Integer, ByRef _SiriusUser As SIRIUSUSER) As Integer

        Dim result As Integer

        Dim oBusiness As New Bpmuser.Business
        result = oBusiness.Initialise(sUsername:=_SiriusUser.Username,
                                   sPassword:=_SiriusUser.Password,
                                   iUserID:=_SiriusUser.UserID,
                                   iSourceID:=_SiriusUser.SourceID,
                                   iLanguageID:=_SiriusUser.LanguageID,
                                   iCurrencyID:=_SiriusUser.CurrencyID,
                                   iLogLevel:=SiriusUserDefaults.LogLevel,
                                   sCallingAppName:=SiriusUserDefaults.AppName)

        _SiriusUser.UserID = NewUserID

        result = oBusiness.GetDetails(vUserId:=_SiriusUser.UserID)

        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        result = oBusiness.GetNext(vUserId:=_SiriusUser.UserID, vUsername:=_SiriusUser.Username, vPassword:=_SiriusUser.Password, vPartyCnt:=_SiriusUser.PartyCnt)

        Return result

    End Function

    ''' <summary>
    ''' Process a EXTWRKITEM background Job.
    ''' </summary>
    ''' <param name="database">Database to act on.</param>
    ''' <param name="backgroundJob">Background Job row to act on.</param>
    ''' <returns>True if the thread should continue processing, or false if a stop request has been received.</returns>
    Private Function ProcessBackgroundJobEXTWRKITEM(ByVal database As Database,
                                                  ByRef backgroundJob As BackgroundJobData) As Boolean

        Dim bReturn As Boolean = False
        Dim nReturn As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim oa_KeyArray(,) As Object

        Log.WriteDebug(database, "ProcessBackgroundJobEXTWRKITEM - start")

        If BackgroundJobProcess.StopProcess Then
            RollbackBackgroundJob(database, backgroundJob)
            ' Exit the main loop allowing the thread to terminate.
            Return bReturn
        End If

        ' Process the job
        ' Add code to do the processing here
        Dim _SiriusUser As New SIRIUSUSER
        Dim TempFiles As New List(Of String)
        Try


            ' jobtype="EXWRKITEM">
            '   Parse the job xml to get all the required parameters such as
            '   WrkTaskGroupId       - Integer
            '   WrkTaskId            - Integer
            '   WrkCustomer          - String
            '   WrkDueDate           - Date
            '   WrkUserGroupId       - Integer
            '   WrkDescription       - Integer
            '   WrkStatus            - String
            '   WrkIsUrgent          - Boolean
            '   WrkFlowInformation   - String
            '   WrkUserId            - Integer
            '   WrkIsVisible         - Boolean
            '   WrkIsReview          - Boolean
            '   WrkIsExternal        - Boolean
            '   WrkParentTaskId      - Inetegr
            '   WrkExternalCategoryId- Integer
            '   WrkLockKeyName       - String
            '   WrkLockKeyValue      - String
            '   PMWrkTaskInstanceCnt - Integer
            '   WrkActionType        - String

            '=====================Key Data============================
            '<KEY DATA>
            '   NAME 
            '   VALUE
            '</KEY DATA>
            '=========================================================

            Log.WriteDebug(database, "ProcessBackgroundJobEXTWRKITEM.ParseEXTWRKITEMJobXML - before")


            Dim settings As New XmlReaderSettings
            settings.ValidationType = ValidationType.None

            Dim doc As XmlDocument = New XmlDocument()
            doc.Load(XmlReader.Create(New StringReader(backgroundJob.Xml), settings))

            'Create an XmlNamespaceManager for resolving namespaces.
            Dim nsmgr As XmlNamespaceManager = New XmlNamespaceManager(doc.NameTable)
            nsmgr.AddNamespace("xs", String.Empty)

            Dim nodeList As XmlNodeList
            Dim root As XmlElement = doc.DocumentElement
            Dim paramterNode As XmlNode = Nothing
            Dim parameterName As String = String.Empty
            Dim parameterValue As String = String.Empty

            '=====================Node Variables [Start]==========================
            Dim nWrkTaskGroupId As Integer
            Dim nWrkTaskId As Integer
            Dim sWrkCustomer As String = String.Empty
            Dim dtWrkDueDate As Date
            Dim nWrkUserGroupId As Integer
            Dim sWrkDescription As String = String.Empty
            Dim nWrkStatus As Integer
            Dim nWrkIsUrgent As Integer
            Dim sWrkFlowInformation As String = String.Empty
            Dim nWrkUserId As Integer
            Dim nWrkIsVisible As Integer
            Dim nWrkIsReview As Integer
            Dim bWrkIsExternal As Boolean
            Dim nWrkParentTaskId As Integer
            Dim sWrkExternalCategoryCode As String
            Dim sWrkLockKeyName As String = String.Empty
            Dim nWrkLockKeyValue As Integer
            Dim nWrkTaskInstanceCnt As Integer = 0
            Dim uGuidPMExternalItem As String = String.Empty
            Dim sWrkActionType As String = String.Empty
            Dim nExternalTaskStatus As Integer = -1
            Dim bIsExternalChildTask As Boolean = False
            '=====================End Node variable[End] ==========================

            ' Get all the EXTWRKITEM JOB Parameters
            nodeList = root.SelectNodes("/BACKGROUND_JOB/JOB/PARAMETERS/PARAMETER", nsmgr)
            For Each paramterNode In nodeList
                If Not paramterNode.Attributes.ItemOf("name") Is Nothing Then
                    parameterName = paramterNode.Attributes.ItemOf("name").Value.ToLower
                End If
                If Not paramterNode.Attributes.ItemOf("value") Is Nothing Then
                    parameterValue = paramterNode.Attributes.ItemOf("value").Value
                End If
                '============Initializing the Variables  for the XML node Valuess

                Select Case Microsoft.VisualBasic.UCase(parameterName)
                    Case "WRKTASKGROUPID"
                        If ValidateXmlParameterValue(parameterValue, "INTEGER") Then
                            nWrkTaskGroupId = XmlSafeConvert.ToInt32((parameterValue))
                        End If
                    Case "WRKTASKID"
                        If ValidateXmlParameterValue(parameterValue, "INTEGER") Then
                            nWrkTaskId = XmlSafeConvert.ToInt32(parameterValue)
                        End If

                    Case "WRKCUSTOMER"
                        sWrkCustomer = parameterValue
                    Case "WRKDUEDATE"
                        If Not parameterValue Is Nothing AndAlso parameterValue.Trim <> "" Then
                            dtWrkDueDate = CDate(parameterValue)
                        End If

                    Case "WRKUSERGROUPID"
                        If ValidateXmlParameterValue(parameterValue, "INTEGER") Then
                            nWrkUserGroupId = XmlSafeConvert.ToInt32(parameterValue)
                        End If
                    Case "WRKDESCRIPTION"
                        sWrkDescription = parameterValue
                    Case "WRKSTATUS"
                        If ValidateXmlParameterValue(parameterValue, "INTEGER") Then
                            nWrkStatus = XmlSafeConvert.ToInt32(parameterValue, 0)
                        End If
                    Case "WRKISURGENT"
                        If ValidateXmlParameterValue(parameterValue, "INTEGER") Then
                            nWrkIsUrgent = XmlSafeConvert.ToInt32(parameterValue, 0)
                        End If
                    Case "WRKFLOWINFORMATION"
                        sWrkFlowInformation = parameterValue
                    Case "WRKUSERID"
                        If ValidateXmlParameterValue(parameterValue, "INTEGER") Then
                            nWrkUserId = XmlSafeConvert.ToInt32(parameterValue, 0)
                        End If
                    Case "WRKISVISIBLE"
                        If ValidateXmlParameterValue(parameterValue, "INTEGER") Then
                            nWrkIsVisible = XmlSafeConvert.ToInt32(parameterValue, 0)
                        End If

                    Case "WRKISREVIEW"
                        If ValidateXmlParameterValue(parameterValue, "INTEGER") Then
                            nWrkIsReview = XmlSafeConvert.ToInt32(parameterValue, 0)
                        End If

                    Case "WRKISEXTERNAL"
                        If ValidateXmlParameterValue(parameterValue, "BOOLEAN") Then
                            If parameterValue.ToUpper.Trim = "TRUE" Then
                                bWrkIsExternal = True
                            Else
                                bWrkIsExternal = True
                            End If
                        End If

                    Case "WRKPARENTTASKCode"
                        If ValidateXmlParameterValue(parameterValue, "INTEGER") Then
                            nWrkParentTaskId = XmlSafeConvert.ToInt32(parameterValue, 0)
                        End If

                    Case "WRKEXTERNALCATEGORYCODE"
                        sWrkExternalCategoryCode = parameterValue.ToString()

                    Case "WRKLOCKKEYNAME"
                        If parameterValue <> "" Then
                            sWrkLockKeyName = parameterValue
                        End If

                    Case "WRKLOCKKEYVALUE"
                        If ValidateXmlParameterValue(parameterValue, "INTEGER") Then
                            nWrkLockKeyValue = XmlSafeConvert.ToInt32(parameterValue, 0)
                        End If

                    Case "WRKTASKINSTANCECNT"
                        If ValidateXmlParameterValue(parameterValue, "INTEGER") Then
                            nWrkTaskInstanceCnt = XmlSafeConvert.ToInt32(parameterValue, 0)
                        End If

                    Case "WRKGUIDEXTERNALITEM"
                        If parameterValue <> "" Then
                            uGuidPMExternalItem = parameterValue
                        End If

                    Case "WRKACTIONTYPE"
                        If parameterValue.ToString = "" Then
                            Log.WriteDebug(database, "ProcessBackgroundJobEXTWRKITEM Action type Is Missing - exiting")
                            Return False
                        End If
                        sWrkActionType = parameterValue.ToString.ToUpper
                    Case "WRKEXTERNALTASKSTATUS"
                        If ValidateXmlParameterValue(parameterValue, "INTEGER") Then
                            nExternalTaskStatus = XmlSafeConvert.ToInt32(parameterValue, -1)
                        End If
                    Case "WRKISEXTERNALCHILDTASK"
                        If ValidateXmlParameterValue(parameterValue, "BOOLEAN") Then
                            If parameterValue.ToUpper.Trim = "TRUE" Then
                                bIsExternalChildTask = True
                            Else
                                bIsExternalChildTask = False
                            End If
                        End If

                    Case Else       ' Add more parameters (if any here)
                        ' Add error handler code
                End Select
            Next


            '============Initializing the key array from the xml nodes
            nodeList = root.SelectNodes("//KEY")
            Dim icount As Integer = 0
            ReDim oa_KeyArray(1, nodeList.Count - 1)
            parameterName = ""
            parameterValue = 0

            For Each paramterNode In nodeList

                If Not paramterNode.Attributes.ItemOf("NAME") Is Nothing Then
                    parameterName = paramterNode.Attributes.ItemOf("NAME").Value.ToLower
                End If
                If Not paramterNode.Attributes.ItemOf("VALUE") Is Nothing Then
                    parameterValue = paramterNode.Attributes.ItemOf("VALUE").Value
                End If

                '============Initializing the Variables  for the XML node Valuess
                If Not parameterName Is Nothing AndAlso CStr(parameterName) <> "" Then
                    oa_KeyArray(kPMENavLetGetKeyNameColPosition, icount) = parameterName
                    oa_KeyArray(kPMENavLetGetKeyColPosition, icount) = parameterValue

                    icount = icount + 1
                End If

            Next

            Log.WriteDebug(database, "ProcessBackgroundJobEXTWRKITEM.CreateNewExternalTask  before")

            'create the external task
            bReturn = CreateNewExternalTask(database:=database,
                                    _SiriusUser:=_SiriusUser,
                                    r_sBranchCode:=_SiriusUser.SourceID,
                                    r_lCategoryID:=Nothing,
                                    r_lPMWrkTaskGroupID:=nWrkTaskGroupId,
                                    r_lPMWrkTaskID:=nWrkTaskId,
                                    r_sCustomer:=sWrkCustomer,
                                    r_dtTaskDueDate:=dtWrkDueDate,
                                    r_lPMUserGroupID:=nWrkUserGroupId,
                                    r_sDescription:=sWrkDescription,
                                    r_nTaskStatus:=nWrkStatus,
                                    r_nIsUrgent:=nWrkIsUrgent,
                                    r_lPMWrkTaskInstanceCnt:=nWrkTaskInstanceCnt,
                                    r_sWorkflowInformation:=sWrkFlowInformation,
                                    r_nUserID:=nWrkUserId,
                                    r_oaKeyArray:=oa_KeyArray,
                                    r_nIsVisible:=nWrkIsVisible,
                                    r_nIsReview:=nWrkIsReview,
                                    r_bIsExternalItem:=bWrkIsExternal,
                                    r_sGuidPMExternalItem:=uGuidPMExternalItem,
                                    r_nParentTaskId:=nWrkParentTaskId,
                                    r_sExternalTaskCategorycode:=sWrkExternalCategoryCode,
                                    r_sActionType:=sWrkActionType,
                                    r_sLockKeyName:=sWrkLockKeyName,
                                    r_nLockKeyValue:=nWrkLockKeyValue,
                                    r_nExternalTaskStatus:=nExternalTaskStatus,
                                    bIsExternalChildTask:=bIsExternalChildTask)


            Log.WriteDebug(database, "ProcessBackgroundJobEXTWRKITEM.CreateNewExternalTask after")

        Catch ex As Exception

            backgroundJob.ErrorDetails = Formatter.Format(ex)

            ' Log the error then absorb it. Errors in one message should not affect other messages.
            Log.WriteDebug(database, String.Format("ProcessBackgroundJobEXTWRKITEM failed to process background job id {0}.", backgroundJob.ID))

            If ExceptionPolicy.HandleException(ex, "Log and Absorb") Then
                Throw
            End If

        Finally

            Log.WriteDebug(database, "ProcessBackgroundJobEXTWRKITEM - exiting")

        End Try

        Return bReturn

    End Function
    ''' <summary>
    ''' This methos is Used to Create External item In the E5
    ''' </summary>
    ''' <param name="database"></param>
    ''' <param name="_SiriusUser"></param>
    ''' <param name="r_lCategoryID"></param>
    ''' <param name="r_sBranchCode"></param>
    ''' <param name="r_lPMWrkTaskGroupID"></param>
    ''' <param name="r_lPMWrkTaskID"></param>
    ''' <param name="r_sCustomer"></param>
    ''' <param name="r_dtTaskDueDate"></param>
    ''' <param name="r_lPMUserGroupID"></param>
    ''' <param name="r_sDescription"></param>
    ''' <param name="r_nTaskStatus"></param>
    ''' <param name="r_nIsUrgent"></param>
    ''' <param name="r_lPMWrkTaskInstanceCnt"></param>
    ''' <param name="r_sWorkflowInformation"></param>
    ''' <param name="r_nUserID"></param>
    ''' <param name="r_oaKeyArray"></param>
    ''' <param name="r_nIsVisible"></param>
    ''' <param name="r_nIsReview"></param>
    ''' <param name="r_bIsExternalItem"></param>
    ''' <param name="r_sGuidPMExternalItem"></param>
    ''' <param name="r_nParentTaskId"></param>
    ''' <param name="r_sExternalTaskCategorycode"></param>
    ''' <param name="r_sLockKeyName"></param>
    ''' <param name="r_nLockKeyValue"></param>
    ''' <param name="r_nExternalTaskStatus"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CreateNewExternalTask(ByVal database As Database,
                                        ByVal _SiriusUser As SIRIUSUSER,
                                        ByVal r_lCategoryID As Integer,
                                        ByVal r_sBranchCode As String,
                                        ByVal r_lPMWrkTaskGroupID As Integer,
                                        ByVal r_lPMWrkTaskID As Integer,
                                        ByVal r_sCustomer As String,
                                        ByVal r_dtTaskDueDate As Date,
                                        ByVal r_lPMUserGroupID As Integer,
                                        ByVal r_sDescription As String,
                                        ByVal r_nTaskStatus As Integer,
                                        ByVal r_nIsUrgent As Integer,
                                        ByVal r_lPMWrkTaskInstanceCnt As Integer,
                                        ByVal r_sWorkflowInformation As String,
                                        ByVal r_nUserID As String,
                                        ByVal r_oaKeyArray As Object,
                                        ByVal r_nIsVisible As Integer,
                                        ByVal r_nIsReview As Integer,
                                        ByVal r_bIsExternalItem As Boolean,
                                        ByRef r_sGuidPMExternalItem As String,
                                        ByVal r_nParentTaskId As Integer,
                                        ByVal r_sExternalTaskCategorycode As String,
                                        ByVal r_sActionType As String,
                                        ByVal r_sLockKeyName As String,
                                        ByVal r_nLockKeyValue As Integer,
                                        ByVal r_nExternalTaskStatus As Integer,
                                        ByVal bIsExternalChildTask As Integer) As Integer

        Dim nResult As Integer = 0
        Log.WriteDebug(database, "CreateNewExternalTask - Start")

        Dim oPMWrkTaskInstance As bPMWrkTaskInstance.TaskControl = Nothing
        Dim bInitialised As Boolean = False
        Dim sErrorMessage As String = String.Empty

        Try
            oPMWrkTaskInstance = New bPMWrkTaskInstance.TaskControl
        Catch ex As Exception
            ' Log the error then absorb it. Errors in one message should not affect other messages.
            Log.WriteDebug(database, "CreateNewExternalTask - Failed to create bPMWrkTaskInstance.TaskControl Component.")
            If ExceptionPolicy.HandleException(ex, "Log and Absorb") Then
                Throw
            End If
        End Try

        Try
            Log.WriteDebug(database, "CreateNewExternalTask.bPMWrkTaskInstance.TaskControl.Initialise - before")


            nResult = oPMWrkTaskInstance.Initialise(sUsername:=_SiriusUser.Username,
                                                 sPassword:=_SiriusUser.Password,
                                                 iUserID:=_SiriusUser.UserID,
                                                 iSourceID:=_SiriusUser.SourceID,
                                                 iLanguageID:=_SiriusUser.LanguageID,
                                                 iCurrencyID:=_SiriusUser.CurrencyID,
                                                 iLogLevel:=SiriusUserDefaults.LogLevel,
                                                sCallingAppName:=SiriusUserDefaults.AppName)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Log.WriteDebug(database, "Initialise - Failed to initialise CreateNewExternalTask.TaskControl Component.")
                Return nResult

            End If

            'validate the value before  Make the calling
            If r_oaKeyArray Is Nothing Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Log.WriteDebug(database, "Key Array is Nothing in While Creating External task")
                Return nResult
            End If

            '===============Calling the oPMWrkTaskInstance.CreateNewExternalTask Function to create it  And Update the status if succeded
            If r_sActionType.Trim.ToUpper = "UPDATETASK" Then

                Dim SGuid As Guid
                'other wise actiontype would be CreateTask
                nResult = oPMWrkTaskInstance.UpdateTaskStatus(r_lPMWrkTaskInstanceCnt, r_nTaskStatus, r_sGuidPMExternalItem, True, r_nExternalTaskStatus, sErrorMessage)

                '======================================Call update Task===================================================
            Else
                nResult = oPMWrkTaskInstance.CreateNew(v_lPMWrkTaskGroupID:=r_lPMWrkTaskGroupID,
                                                       v_lPMWrkTaskID:=r_lPMWrkTaskID,
                                                       v_sCustomer:=r_sCustomer,
                                                       v_dtTaskDueDate:=r_dtTaskDueDate,
                                                       v_lPMUserGroupID:=r_lPMUserGroupID,
                                                       v_sDescription:=r_sDescription,
                                                       v_iTaskStatus:=r_nTaskStatus,
                                                       v_iIsUrgent:=r_nIsUrgent,
                                                       r_lPMWrkTaskInstanceCnt:=r_lPMWrkTaskInstanceCnt,
                                                       v_sWorkflowInformation:=r_sWorkflowInformation,
                                                       v_iUserID:=r_nUserID,
                                                       v_vKeyArray:=r_oaKeyArray,
                                                       v_iIsVisible:=r_nIsVisible,
                                                       v_iIsTaskReview:=r_nIsReview,
                                                       bIsExternalWorkItem:=r_bIsExternalItem,
                                                       r_sGuidPMExternalItem:=r_sGuidPMExternalItem,
                                                       nParentTaskId:=r_nParentTaskId,
                                                       sExternalTaskCategoryCode:=r_sExternalTaskCategorycode,
                                                       sLockKeyName:=SharedFiles.LockNameStringToEnum(r_sLockKeyName),
                                                       nLockKeyValue:=r_nLockKeyValue,
                                                       bViaBackgrounJobProcess:=True,
                                                       nExternalTaskStatus:=r_nExternalTaskStatus,
                                                       bIsExternalChildTask:=bIsExternalChildTask)
            End If

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Log.WriteDebug(database, "CreateNewExternalTask - Failed to Create  the External task Component.")
                Return nResult

            End If

            Log.WriteDebug(database, "CreateNewExternalTask.bPMWrkTaskInstance.Create - after")

            If nResult = PMEReturnCode.PMTrue Then
                Log.WriteDebug(database, "Creation bPMWrkTaskInstance.TaskControl.CreateNewExternalTask - Success")
            End If

            nResult = gPMConstants.PMEReturnCode.PMTrue

        Catch ex As Exception
            If Not String.IsNullOrEmpty(sErrorMessage) Then
                Log.WriteDebug(database, sErrorMessage)
            Else
                If bInitialised = False Then
                    Log.WriteDebug(database, "CreateNewExternalTask - Failed to initialise bPMWrkTaskInstance.TaskControl Component.")
                End If

                Log.WriteDebug(database, "CreateNewExternalTask - error." & ex.Message)
            End If

            If ExceptionPolicy.HandleException(ex, "Log and Absorb") Then
                Throw
            End If

        Finally
            Dim nRet As Integer
            ' Dispose of the business component safely.
            If oPMWrkTaskInstance IsNot Nothing Then
                oPMWrkTaskInstance.Dispose()
                oPMWrkTaskInstance = Nothing
                Log.WriteDebug(database, "CreateNewExternalTask.Dispose object bPMWrkTaskInstance.TaskControl - before")
            End If
        End Try

        Return nResult
    End Function
    ''' <summary>
    ''' Method to Omit the Casting into wrong datatype
    ''' </summary>
    ''' <param name="r_oParameterValue"></param>
    ''' <param name="r_oParameterDatatype"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ValidateXmlParameterValue(ByVal r_oParameterValue As Object,
                                      ByVal r_oParameterDatatype As String
                                       ) As Integer
        Dim bReturn As Boolean = True

        Try




            Select Case Microsoft.VisualBasic.UCase(r_oParameterDatatype)
                Case "INTEGER"
                    If Not Microsoft.VisualBasic.IsNumeric(r_oParameterValue) Then
                        bReturn = False
                        Return bReturn
                    End If
                Case "BOOLEAN"
                    If Microsoft.VisualBasic.UCase(r_oParameterValue) <> "TRUE" AndAlso Microsoft.VisualBasic.UCase(r_oParameterValue) <> "FALSE" Then
                        bReturn = False
                        Return bReturn
                    End If
                Case "STRING"
                    bReturn = True
                    Return bReturn
            End Select


        Catch ex As Exception

        End Try
        Return bReturn
    End Function
#End Region



#Region "Private Shared Methods - Data Access"
    ''' <summary>
    ''' GetNextJobToProcess
    ''' </summary>
    ''' <param name="database"></param>
    ''' <param name="jobSource"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function GetNextJobToProcess(ByVal database As Database,
                                                ByVal jobSource As String,
                                                ByVal nWaitMiuntesBeforeRetry As Integer) As BackgroundJobData

        Dim id As Nullable(Of Integer)
        Dim xml As String
        Dim partyCode As String
        Dim insuranceRef As String
        Dim claimNumber As String
        Dim nUserID As Nullable(Of Integer)
        Dim nLoggedUserID As Nullable(Of Integer)
        Dim sLoggedUserName As String
        Dim sLoggedUserPassword As String

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SIR_Background_Job_GetNext")
            cmd.AddOutParameter("@background_job_id", SqlDbType.Int)
            cmd.AddOutParameter("@job_xml", SqlDbType.VarChar, -1)  ' -1 represents max size
            cmd.AddOutParameter("@party_code", SqlDbType.VarChar, -1)  ' -1 represents max size
            cmd.AddOutParameter("@insurance_ref", SqlDbType.VarChar, -1)  ' -1 represents max size
            cmd.AddOutParameter("@claim_number", SqlDbType.VarChar, -1) ' -1 represents max size
            cmd.AddInParameter("@job_service", SqlDbType.NVarChar, 255).Value = jobSource
            cmd.AddInParameter("@wait_minutes_before_retry", SqlDbType.Int).Value = nWaitMiuntesBeforeRetry
            cmd.AddOutParameter("@loggedin_userid", SqlDbType.Int, -1) ' -1 represents max size
            cmd.AddOutParameter("@loggedin_username", SqlDbType.VarChar, -1) ' -1 represents max size
            cmd.AddOutParameter("@loggedin_userpassword", SqlDbType.VarChar, -1) ' -1 represents max size

            cmd.AddOutParameter("@ljob_user_id", SqlDbType.Int)
            database.Connection.ExecuteNonQuery(cmd)

            id = Cast.ToInt32(cmd.Parameters("@background_job_id"))
            xml = Cast.ToString(cmd.Parameters("@job_xml"))
            partyCode = Cast.ToString(cmd.Parameters("@party_code"))
            insuranceRef = Cast.ToString(cmd.Parameters("@insurance_ref"))
            claimNumber = Cast.ToString(cmd.Parameters("@claim_number"))
            nUserID = Cast.ToInt32(cmd.Parameters("@ljob_user_id"))
            nLoggedUserID = Cast.ToInt32(cmd.Parameters("@loggedin_userid"))
            sLoggedUserName = Cast.ToString(cmd.Parameters("@loggedin_username"))
            sLoggedUserPassword = Cast.ToString(cmd.Parameters("@loggedin_userpassword"))
        End Using

        If id.HasValue And Not String.IsNullOrEmpty(xml) Then
            ' Process the message.
            Dim backgroundJob As BackgroundJobData = New BackgroundJobData(id, xml, partyCode, insuranceRef, claimNumber, nLoggedUserID, sLoggedUserName, sLoggedUserPassword, nUserID)
            Return backgroundJob
        Else
            Return Nothing
        End If

    End Function

    Private Shared Sub SetBackgroundJobAsComplete(ByVal database As Database,
                                                  ByVal id As Integer)

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SIR_Background_Job_complete")

            cmd.AddInParameter("@background_job_id", SqlDbType.Int).Value = id

            database.Connection.ExecuteNonQuery(cmd)
        End Using

    End Sub

    Private Shared Sub SetBackgroundJobAsFailed(ByVal database As Database,
                                                ByVal id As Integer,
                                                ByVal failureReason As String)

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SIR_Background_Job_failed")

            cmd.AddInParameter("@background_job_id", SqlDbType.Int).Value = id
            Dim truncatedFailureReason As String = If(failureReason, String.Empty)
            If truncatedFailureReason.Length > 1000 Then
                truncatedFailureReason = truncatedFailureReason.Substring(0, 1000)
            End If
            cmd.AddInParameter("@failure_description", SqlDbType.NVarChar, 1000).Value = truncatedFailureReason

            database.Connection.ExecuteNonQuery(cmd)
        End Using
    End Sub

    Private Shared Sub SetBackgroundJobStatus(ByVal database As Database,
                                              ByVal id As Integer,
                                              ByVal jobstatus As String)

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SIR_Background_Job_update_status")

            cmd.AddInParameter("@background_job_id", SqlDbType.Int).Value = id
            cmd.AddInParameter("@job_status", SqlDbType.NVarChar, 1).Value = jobstatus

            database.Connection.ExecuteNonQuery(cmd)

        End Using

    End Sub

    Private Shared Function GetInsuranceFolderCnt(ByVal database As Database,
                                                  ByVal insuranceFileCnt As Integer) As Integer

        Dim insuranceFolderCnt As Integer = 0
        Dim dsInsuranceFile As DataSet = Nothing

        If insuranceFileCnt > 0 Then
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_get_Insurance_Folder")
                cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = insuranceFileCnt
                dsInsuranceFile = database.Connection.ExecuteDataSet(cmd, "dsInsuranceFile")
            End Using
            If dsInsuranceFile.Tables(0).Rows.Count > 0 Then
                insuranceFolderCnt = Convert.ToInt32(dsInsuranceFile.Tables(0).Rows(0).Item("insurance_Folder_cnt"))
            End If
        End If

        Return insuranceFolderCnt

    End Function

    Private Shared Function IsDocumentTemplateExists(ByVal database As Database,
                                                     ByVal documentTemplateCode As String, Optional ByVal v_dtEffectiveDate As Date = Nothing, Optional ByVal v_lInsuranceFileCnt As Integer = 0, Optional ByVal v_lClaimCnt As Integer = 0) As Boolean


        Dim bReturnValue As Boolean = False
        Dim nDocumentTemplateID As Nullable(Of Integer)
        Dim nDocumentTypeID As Nullable(Of Integer)
        Dim sDescription As String

        If documentTemplateCode.Length > 0 And documentTemplateCode.Length <= 10 Then

            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_get_document_template_saa")

                cmd.AddInParameter("@code", SqlDbType.Char, 10).Value = documentTemplateCode
                cmd.AddOutParameter("@document_template_id", SqlDbType.Int)
                cmd.AddOutParameter("@document_type_id", SqlDbType.Int)
                cmd.AddOutParameter("@description", SqlDbType.Char, 255)  ' -1 represents max size

                cmd.AddInParameter("@effective_date", SqlDbType.DateTime).Value = v_dtEffectiveDate
                cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = v_lInsuranceFileCnt
                cmd.AddInParameter("@claim_cnt", SqlDbType.Int).Value = v_lClaimCnt

                database.Connection.ExecuteNonQuery(cmd)

                nDocumentTemplateID = Cast.ToInt32(cmd.Parameters("@document_template_id"))
                nDocumentTypeID = Cast.ToInt32(cmd.Parameters("@document_type_id"))
                sDescription = Cast.ToString(cmd.Parameters("@description"))

            End Using

            If nDocumentTemplateID.HasValue Then
                If nDocumentTemplateID > 0 Then
                    ' The template code is valid
                    bReturnValue = True
                End If
            End If

        End If

        Return bReturnValue

    End Function
    ''' <summary>
    ''' This Function Is Used to Validate the Service Retry limit if any Case is gets fail.
    ''' </summary>
    ''' <param name="oDatabase"></param>
    ''' <param name="lbackground_job_id"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ValidateRetryCount(ByVal oDatabase As Database,
                                        ByVal lbackground_job_id As Integer) As Integer

        Dim nResult As Integer = 0

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SIR_Background_Job_Reinstate")

            cmd.AddInParameter("@nbackground_job_id", SqlDbType.Int).Value = lbackground_job_id
            cmd.AddInParameter("@nAllowRetryCnt", SqlDbType.Int).Value = ServiceRetryLimitForEXWRKITEM

            cmd.AddOutParameter("@nReturnflag", SqlDbType.Int).Value = 0
            oDatabase.Connection.ExecuteNonQuery(cmd)
            nResult = Cast.ToInt32(cmd.Parameters("@nReturnflag"))

        End Using

        Return nResult

    End Function

#End Region

End Class
