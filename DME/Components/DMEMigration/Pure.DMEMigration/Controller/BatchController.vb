Imports System.Collections.Concurrent
Imports System.Globalization
Imports System.Xml.Schema
Imports System.Configuration
Imports System.IO
Imports Pure.DMEMigration.Controller
Imports Pure.DMEMigration.Configuration
Imports System.Xml
Imports System.Threading.Tasks
Imports SharedFiles

Namespace Controller
    ''' <summary>
    ''' Batch controller for executing doc migration
    ''' </summary>
    Public NotInheritable Class BatchController
        Private ReadOnly m_Queue As ConcurrentQueue(Of JobDetail)
        Private ReadOnly m_Batches As ConcurrentQueue(Of Integer)
    Private ReadOnly m_Culture As CultureInfo
    Private ReadOnly m_Results As New ConcurrentBag(Of Result)
    Private m_oSharePoint As bSIRSharepoint.Business
    Private taskArray() As Task
    Private concurrentLimit As Integer
    Private appSettings As ConfigSection

    Public Event StatusUpdate(sender As Object, e As StatusUpdateEventArgs)

    ''' <summary>
    ''' BatchContoller Constructor.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        m_Queue = New ConcurrentQueue(Of JobDetail)()
        m_Culture = CultureInfo.CurrentCulture
    End Sub

    ''' <summary>
    ''' Starts the batch process.
    ''' </summary>
    ''' <param name="iBatchId"></param>
    ''' <param name="iFolderNum"></param>
    ''' <param name="iDocNums"></param>
    ''' <remarks></remarks>
    Public Sub Start(ByVal iBatchId As Integer?, ByVal iFolderNum As Integer?, ByVal iDocNums() As Integer)

        OnStatusUpdate("DME migration process has STARTED", StatusLevel.Information)
        Dim dMainStartTime As DateTime = Date.Now()
        Dim _SiriusUser As New SIRIUSUSER
        Dim sUserName As String = _SiriusUser.Username
        Dim sPassword As String = _SiriusUser.Password

        ' Get config.
        appSettings = CType(ConfigurationManager.GetSection("appSetting"), ConfigSection)
        If appSettings Is Nothing Then
            OnStatusUpdate(String.Format(m_Culture, "!!! Batch {0} aborted !!! There are no appSettings present.",
                                     iBatchId), StatusLevel.Fatal)
            Return
        End If

        ' Get the documents from the DB and push them into the queue.
        Dim dStartTime As DateTime = Date.Now()
        Dim iJobBatchId As Integer? = PopulateQueue(iBatchId, iFolderNum, iDocNums)

        If iJobBatchId.HasValue Then
            OnStatusUpdate(String.Format(m_Culture, "Batch {0} started.", iJobBatchId), StatusLevel.Fatal)
        End If

        Dim lReturn As Long
        Try
            concurrentLimit = appSettings.InstanceItems(0).ThreadLimit - 1
            If ToSafeInteger(concurrentLimit) < 0 Then
                concurrentLimit = 0
            End If
        Catch
            ' set default to 1
            concurrentLimit = 0
        End Try

        If Not m_Queue.IsEmpty Then
            If m_oSharePoint Is Nothing Then
                m_oSharePoint = New bSIRSharepoint.Business()

                    lReturn = m_oSharePoint.Initialise(sUserName, sPassword, _SiriusUser.UserID, _SiriusUser.SourceID, _SiriusUser.LanguageID, _SiriusUser.CurrencyID, _SiriusUser.LogLevel, "DMEMigration")
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    OnStatusUpdate(String.Format(m_Culture, "!!! Batch {0} aborted !!! Failed to initialise the m_oSharePoint object.",
                     iBatchId), StatusLevel.Fatal)
                    Return
                End If
            End If

            ' Get the number of cores.
            OnStatusUpdate(String.Format("Number of Processors: {0}", Environment.ProcessorCount), StatusLevel.Information)

            ' only 1 supported for now. can be expanded in future
            ReDim taskArray(concurrentLimit)

            For i As Integer = 0 To taskArray.Length - 1
                taskArray(i) = Task.Factory.StartNew(Sub() MigrateBatch(appSettings.InstanceItems(0)))
            Next
            Task.WaitAll(taskArray)

            taskArray = Nothing
            OnStatusUpdate(String.Format("Documents moved in {0}s", (Date.Now() - dMainStartTime).TotalSeconds), StatusLevel.Information)
        End If
    End Sub

    ''' <summary>
    ''' enqueue next batch 
    ''' </summary>
    ''' <param name="iBatchId"></param>
    ''' <param name="iFolderNum"></param>
    ''' <param name="iDocNums"></param>
    ''' <remarks></remarks>
    Public Sub EnqueueNextBatch(ByVal iBatchId As Integer?, ByVal iFolderNum As Integer?, ByVal iDocNums() As Integer)

        OnStatusUpdate("Next batch being queued.", StatusLevel.Information)
        Dim dMainStartTime As DateTime = Date.Now()
        Dim _SiriusUser As New SIRIUSUSER
        ' Get the documents from the DB and push them into the queue.
        Dim startTime As DateTime = Date.Now()

        If iBatchId.HasValue AndAlso m_Batches.Contains(iBatchId.Value) Then
            OnStatusUpdate(String.Format("Given batch {0} is already being migrated.", iBatchId.Value), StatusLevel.Information)
        Else
            Dim jobBatchId As Integer? = PopulateQueue(iBatchId, iFolderNum, iDocNums)
            If jobBatchId.HasValue Then
                OnStatusUpdate(String.Format("Next batch {0} queued.", jobBatchId), StatusLevel.Information)
            End If
        End If

        If Not m_Queue.IsEmpty And taskArray Is Nothing Then
            ReDim taskArray(concurrentLimit)

            For i As Integer = 0 To taskArray.Length - 1
                taskArray(i) = Task.Factory.StartNew(Sub() MigrateBatch(appSettings.InstanceItems(0)))
            Next
            Task.WaitAll(taskArray)

            taskArray = Nothing
            OnStatusUpdate(String.Format("Documents moved in {0}s", (Date.Now() - dMainStartTime).TotalSeconds), StatusLevel.Information)
        End If

    End Sub

    ''' <summary>
    ''' Populates the queue with the docs from the DB.
    ''' </summary>
    ''' <param name="iBatchId"></param>
    ''' <param name="iFolderNum"></param>
    ''' <param name="iDocNums"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function PopulateQueue(ByVal iBatchId As Integer?, iFolderNum As Integer?, ByVal iDocNums() As Integer) As Integer
        ' Check if it is a resume.
        If iBatchId.HasValue Then
            ' Populate the queue with the batch's existing data.
            Return ResumeBatchPopulateQueue(iBatchId.Value)
        ElseIf iFolderNum.HasValue Then
            ' Creates a batch from a folder num
            Return CreateBatchForFolder(iFolderNum)
        Else
            ' Create batch for passed docs
            Return CreateBatchForDocs(iDocNums)
        End If
    End Function

    ''' <summary>
    ''' Resumes a batch for records with 1 (Ready for processing) status.
    ''' </summary>
    ''' <param name="iBatchId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ResumeBatchPopulateQueue(ByVal iBatchId As Integer?) As Integer

        ' Check if a batch ID is provided.
        If iBatchId <= 0 Then
            OnStatusUpdate(String.Format(m_Culture,
                                     "!!! Batch {0} aborted !!! Doc migration was called to resume a batch but an invalid batch ID was provided.",
                                     iBatchId), StatusLevel.Fatal)
            Return Nothing
        End If

        Dim dataContext As New DataContext

        Dim listOfDocs As List(Of Integer)

        ' Get the batch details and push the documents into the queue.
        Try
            listOfDocs = dataContext.LoadOutstandingDocsForBatch(iBatchId)

            ' Check if the queue was populated with anything.
            If listOfDocs Is Nothing OrElse listOfDocs.Count = 0 Then
                OnStatusUpdate(String.Format(m_Culture,
                                        "Batch {0} aborted. No records were found to process for the batch.",
                                        iBatchId), StatusLevel.Fatal)
                Return Nothing
            End If

            For Each targetDoc As Integer In listOfDocs
                Dim jobDetails As New JobDetail
                jobDetails.BatchId = iBatchId
                jobDetails.DocNum = targetDoc
                If Not m_Queue.Contains(jobDetails) Then
                    m_Queue.Enqueue(jobDetails)
                End If
            Next

            Return iBatchId

        Catch ex As Exception

            OnStatusUpdate(ex.Message, StatusLevel.Fatal)
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="iFolderNum"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateBatchForFolder(iFolderNum As Integer?) As Integer
        Dim oDataConext As New DataContext
        Dim sBatchReference As String = Guid.NewGuid().ToString()
        Dim ibatchId As Integer?
        Dim iTargetdoc As List(Of Integer)
        Dim sDmeFolder As String = ""
        Try
            oDataConext.BeginTransaction()

            oDataConext.RetreiveFolderInfo(iFolderNum, sDmeFolder)

            ibatchId = oDataConext.ExecuteInsertBatch(sBatchReference)
            If Not ibatchId.HasValue Then
                oDataConext.RollBackTransaction()
                OnStatusUpdate(String.Format("Failed to create batch for the folder ""{0}"".", sDmeFolder), StatusLevel.Information)
                Return Nothing
            End If

            iTargetdoc = oDataConext.RetrieveTargetDocsForBatch(ibatchId.Value, iFolderNum.Value)
            If iTargetdoc Is Nothing OrElse iTargetdoc.Count = 0 Then
                oDataConext.RollBackTransaction()
                OnStatusUpdate(String.Format("No valid documents found for the folder ""{0}"" to migrate.", sDmeFolder), StatusLevel.Information)
                Return Nothing
            End If

            For Each iTarget As Integer In iTargetdoc.OrderBy(Function(p) iTarget)
                Dim jobDetails As New JobDetail
                jobDetails.BatchId = ibatchId
                jobDetails.DocNum = iTarget
                If Not m_Queue.Contains(jobDetails) Then
                    m_Queue.Enqueue(jobDetails)
                End If
            Next

            oDataConext.CommitTransaction()

            Return ibatchId

        Catch ex As Exception
            If Not oDataConext Is Nothing Then
                oDataConext.RollBackTransaction()
            End If
            OnStatusUpdate(ex.Message, StatusLevel.Fatal)
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="iDocNums"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateBatchForDocs(iDocNums() As Integer) As Integer
        Dim oDataConext As New DataContext

        Dim sBatchReference As String = Guid.NewGuid().ToString()
        Dim iBatchId As Integer?
        Dim iTargetdoc As List(Of Integer)

        Try
            oDataConext.BeginTransaction()

            iBatchId = oDataConext.ExecuteInsertBatch(sBatchReference)
            If Not iBatchId.HasValue Then
                oDataConext.RollBackTransaction()
                OnStatusUpdate(String.Format("Failled to create batch for selected documents."), StatusLevel.Information)
                Return Nothing
            End If

            iTargetdoc = oDataConext.RetrieveTargetDocsForBatch(iBatchId.Value, iDocNums)
            If iTargetdoc Is Nothing OrElse iTargetdoc.Count = 0 Then
                oDataConext.RollBackTransaction()
                OnStatusUpdate(String.Format("No valid records found for selected documents"), StatusLevel.Information)
                Return Nothing
            End If

            For Each iTarget As Integer In iTargetdoc.OrderBy(Function(p) p)
                Dim jobDetails As New JobDetail
                jobDetails.BatchId = iBatchId
                jobDetails.DocNum = iTarget
                If Not m_Queue.Contains(jobDetails) Then
                    m_Queue.Enqueue(jobDetails)
                End If
            Next

            oDataConext.CommitTransaction()

            Return iBatchId

        Catch ex As Exception
            If Not oDataConext Is Nothing Then
                oDataConext.RollBackTransaction()
            End If
            OnStatusUpdate(ex.Message, StatusLevel.Fatal)
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="instance"></param>
    ''' <remarks></remarks>
    Private Sub MigrateBatch(ByVal instance As InstanceElement)

        Dim oJobDetail As JobDetail = Nothing
        Dim sArchiveDoc As String = ""
        Dim sDestinationFilename As String = ""
        Dim sFullDMEPath As String = ""
        Dim sCreateBy As String = ""
            Dim sCreatedate As DateTime

            Dim lReturn As Integer
        Dim oDataContext As New DataContext

        While m_Queue.TryDequeue(oJobDetail)
            Dim oResult As New Result(instance, oJobDetail)

            m_Results.Add(oResult)

            ProcessDoc(oJobDetail.DocNum, sArchiveDoc, sDestinationFilename, sFullDMEPath, sCreateBy, sCreatedate, oResult.Message)

            If oResult.Message Is Nothing Then

                lReturn = m_oSharePoint.ArchiveDocument(PartyCnt:=0, InsuranceFileCnt:=0,
                                              ClaimID:=0, CaseID:=0, DocumentTemplateID:=0,
                                              TemplateGroupID:=0,
                                              TemplateSubGroupID:=0,
                                              SourceFile:=sArchiveDoc, InternalOnly:=False, SharepointPath:=sArchiveDoc,
                                              DestinationFilename:=sDestinationFilename,
                                              PartyCode:="", PolicyNumber:="",
                                              ClaimNumber:="", Background_Job_Id:=0,
                                              bIsDMEMigration:=True, sCreatedBy:=sCreateBy, sCreateddate:=sCreatedate)
            End If

            oResult.EndTime = DateTime.Now
            oDataContext.BeginTransaction()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If oResult.Message Is Nothing Then
                    If m_oSharePoint.ErrorString Is Nothing Or String.IsNullOrEmpty(m_oSharePoint.ErrorString) Or m_oSharePoint.ErrorString.Length = 0 Then ' if message not set, bSIRSharePoint failed with unknown reason; set one
                        oResult.Message = String.Format("{0}  Failed to upload to Sharepoint.", sFullDMEPath)
                    Else
                        oResult.Message = "[Document Name = " & sFullDMEPath & "] [Error = " & m_oSharePoint.ErrorString & "]"
                    End If
                End If
                oResult.isFailed = True
                oDataContext.UpdateMigrationStatus(oJobDetail.DocNum, "FAIL")
                oResult.Message = "[DateTime = " & DateAndTime.Now & "] [Batch Id = " & oJobDetail.BatchId & "] [Document Number = " & oJobDetail.DocNum & "] " & oResult.Message
                ' log the error
                My.Computer.FileSystem.WriteAllText(instance.LogFile, oResult.Message & vbCrLf, True)
            Else
                oResult.isFailed = False
                oDataContext.UpdateMigrationStatus(oJobDetail.DocNum, "COMPLETE")
            End If
            oDataContext.CommitTransaction()
        End While
    End Sub

    ''' <summary>
    ''' XSD Validator callback for validating the XML files.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ValidationCallBack(sender As Object, e As ValidationEventArgs)
        If e.Severity = XmlSeverityType.Error Then
            OnStatusUpdate(e.Message, StatusLevel.Fatal)
            Throw e.Exception
        End If
    End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="iDocNum"></param>
        ''' <param name="sArchiveDoc"></param>
        ''' <param name="sTargetDoc"></param>
        ''' <param name="sFullDMEPath"></param>
        ''' <param name="sCreateBy"></param>
        ''' <param name="sCreatedate"></param>
        ''' <param name="sMessage"></param>
        ''' <remarks></remarks>
        Private Sub ProcessDoc(iDocNum As Integer, ByRef sArchiveDoc As String, ByRef sTargetDoc As String, ByRef sFullDMEPath As String, ByRef sCreateBy As String, ByRef sCreatedate As DateTime, ByRef sMessage As String)
            Dim dataConext As New DataContext

            Try
                dataConext.RetreiveDocInfo(iDocNum, sArchiveDoc, sTargetDoc, sFullDMEPath, sCreateBy, sCreatedate)
                If sArchiveDoc.Length = 0 AndAlso sTargetDoc.Length = 0 Then
                    sMessage = String.Format("No information found for doc num: {0}", iDocNum)
                ElseIf sArchiveDoc.Length = 0 Then
                    sMessage = String.Format("{0}  No document found.", sFullDMEPath)
                ElseIf sTargetDoc.Length = 0 Then
                    ' very unlikely
                    sMessage = String.Format("No documents found for doc num: {0}", iDocNum)
                ElseIf sCreateBy.Length = 0 Then
                    sMessage = String.Format("No User found", sCreateBy)
                ElseIf sCreatedate = Nothing Then
                    sMessage = String.Format("No valid date", sCreatedate)
                End If
    Catch ex As Exception
                OnStatusUpdate(ex.Message, StatusLevel.Fatal)
            End Try
    End Sub

    ''' <summary>
    ''' Gets the current number of items in the queue
    ''' </summary>
    Public ReadOnly Property QueueDepth() As Integer
        Get
            Return m_Queue.Count()
        End Get
    End Property

    ''' <summary>
    ''' Gets a the current run statistics
    ''' </summary>
    Public ReadOnly Property CurrentStatistics() As IEnumerable(Of RunStatistic)
        Get
            If m_Results Is Nothing Then
                Return Nothing
            End If

            Return From n In m_Results Order By n.JobDetail.BatchId Descending
                   Group n By n.InstanceId, n.JobDetail.BatchId Into Group
                   Select New RunStatistic With {
                       .BatchId = BatchId,
                       .Processed = Group.Count(Function(p) p.EndTime.HasValue And Not p.isFailed),
                       .Failed = Group.Count(Function(p) p.isFailed),
                       .InProgress = Group.Count(Function(p) Not p.EndTime.HasValue),
                       .MinDuration = Group.Where(Function(p) p.EndTime.HasValue).DefaultIfEmpty().Min(Of Double)(Function(n) If(n IsNot Nothing, n.Duration.TotalMilliseconds, 0)),
                       .MaxDuration = Group.Where(Function(p) p.EndTime.HasValue).DefaultIfEmpty().Max(Of Double)(Function(n) If(n IsNot Nothing, n.Duration.TotalMilliseconds, 0)),
                       .AverageDuration = Group.Where(Function(p) p.EndTime.HasValue).DefaultIfEmpty().Average(Function(n) If(n IsNot Nothing, n.Duration.TotalMilliseconds, 0))}

        End Get
    End Property

    ''' <summary>
    ''' Status update method to raise event
    ''' </summary>
    ''' <param name="sStatus"></param>
    ''' <param name="oStateLevel"></param>
    ''' <remarks></remarks>
    Protected Sub OnStatusUpdate(ByVal sStatus As String, ByVal oStateLevel As StatusLevel)
        RaiseEvent StatusUpdate(Me, New StatusUpdateEventArgs(sStatus, oStateLevel))
    End Sub

    Protected Overrides Sub Finalize()
        If m_oSharePoint IsNot Nothing Then
                m_oSharePoint.Dispose()
                m_oSharePoint = Nothing
            End If

        MyBase.Finalize()
    End Sub
    End Class
End Namespace