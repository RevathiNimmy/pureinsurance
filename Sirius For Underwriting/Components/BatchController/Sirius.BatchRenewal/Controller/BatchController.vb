Imports System.Collections.Concurrent
Imports System.Globalization
Imports System.Xml.Schema
Imports System.Configuration
Imports System.IO
Imports Sirius.Architecture.Security
Imports Sirius.BatchRenewal.SamConfiguration
Imports System.Xml
Imports System.Threading.Tasks
Imports SSP.PureInsuranceRestAPIHandler

Namespace Controller
    ''' <summary>
    ''' Batch controller for executing renewals.
    ''' </summary>
    Public NotInheritable Class BatchController
        Private ReadOnly mQueue As ConcurrentQueue(Of Integer)
        Private ReadOnly mCulture As CultureInfo
        Private ReadOnly mResults As New ConcurrentBag(Of Result)

        Public Event StatusUpdate(sender As Object, e As StatusUpdateEventArgs)
        Protected Const BatchStatusComplete As String = "C"
        Protected Const BatchStatusFailed As String = "F"

        Protected dictPolicies As New Dictionary(Of Integer, Integer)

        ''' <summary>
        ''' BatchContoller Constructor.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            mQueue = New ConcurrentQueue(Of Integer)()
            mCulture = CultureInfo.CurrentCulture
        End Sub

        ''' <summary>
        ''' Starts the batch renewal process.
        ''' </summary>
        ''' <param name="batchId">The batch ID</param>
        ''' <param name="xmlPath">Loads the batch XML file if a path is provided. The XSD is specified in the app.config file</param>
        ''' <param name="batchResume">Resumes an existing bath if True</param>
        ''' <remarks></remarks>
        Public Sub Start(ByVal batchId As Integer?, ByVal xmlPath As String,
                         ByVal batchResume As Boolean, ByVal jobCode As String)

            OnStatusUpdate("Batch Renewal process has STARTED", StatusLevel.Information)
            Dim mainStartTime As DateTime = Date.Now()

            ' Get the list of configured SAM servers.
            Dim servers As SamServersConfigSection = CType(ConfigurationManager.GetSection("SamServers"),
                                                           SamServersConfigSection)
            If servers Is Nothing Then
                OnStatusUpdate(String.Format(mCulture, "!!! Batch {0} aborted !!! There are no configured SAM servers.",
                                         batchId), StatusLevel.Fatal)
                Return
            End If

            ' Get the insurance folder cnts from the DB and push them into the queue.
            Dim startTime As DateTime = Date.Now()
            Dim jobDetail As JobDetail = PopulateQueue(batchId, xmlPath, batchResume, jobCode)
            OnStatusUpdate(String.Format("Queue and/or DB population took {0}s", (Date.Now - startTime).TotalSeconds), StatusLevel.Information)

            If jobDetail IsNot Nothing Then

                ' Get the number of cores.
                OnStatusUpdate(String.Format("Number of Processors: {0}", Environment.ProcessorCount), StatusLevel.Information)

                Dim taskArray() As Task
                ReDim taskArray(servers.InstanceItems.Count - 1)

                For i As Integer = 0 To taskArray.Length - 1
                    Dim instanceRef As InstanceElement = servers.InstanceItems(i)
                    taskArray(i) = Task.Factory.StartNew(Sub() ActionSamInstance(instanceRef, jobDetail))
                Next

                Task.WaitAll(taskArray)

            End If

            OnStatusUpdate(String.Format("Batch Renewals FINISHED in {0}s", (Date.Now() - mainStartTime).TotalSeconds), StatusLevel.Information)
        End Sub

        ''' <summary>
        ''' Populates the queue with the insurance folder cnts from the DB.
        ''' </summary>
        ''' <remarks></remarks>
        Private Function PopulateQueue(ByVal batchId As Integer?, ByVal xmlPath As String, ByVal batchResume As Boolean,
                                       ByVal jobCode As String) _
            As JobDetail

            ' Check if it is a resume.
            If batchResume AndAlso String.IsNullOrEmpty(xmlPath) AndAlso batchId.HasValue Then
                ' Populate the queue with the batch's existing data.
                Return ResumeBatchPopulateQueue(batchId.Value)
            ElseIf Not String.IsNullOrEmpty(jobCode) Then
                ' Creates a batch from a job code
                Return CreateBatchFromJob(jobCode)
            Else
                ' Load the DB while populating the queue.
                Return LoadXmlPopulateQueue(xmlPath)
            End If
        End Function

        ''' <summary>
        ''' Resumes a batch for records with 1 (Ready for processing) status.
        ''' </summary>
        ''' <param name="batchId">Batch ID to get the details</param>
        ''' <remarks>Populates the queue with the insurance folder record ID</remarks>
        Private Function ResumeBatchPopulateQueue(ByVal batchId As Integer) As JobDetail

            ' Check if a batch ID is provided.
            If batchId <= 0 Then
                OnStatusUpdate(String.Format(mCulture,
                                         "!!! Batch {0} aborted !!! BatchRenewal was called to resume a batch but an invalid batch ID was provided.",
                                         batchId), StatusLevel.Fatal)
                Return Nothing
            End If

            Dim dataContext As New DataContext
            Dim jobDetail As JobDetail = Nothing
            Dim listOfInsuranceFolders As List(Of Integer)

            ' Get the batch details and push the insurance folder IDs of the 1 (Ready for processing) records into the queue.
            Try
                listOfInsuranceFolders = dataContext.LoadInsuranceFoldersStillOutstandingForBatch(batchId)

                ' Check if the queue was populated with anything.
                If listOfInsuranceFolders Is Nothing OrElse listOfInsuranceFolders.Count = 0 Then
                    OnStatusUpdate(String.Format(mCulture,
                                            "Batch {0} aborted. No records were found to process for the batch.",
                                            batchId), StatusLevel.Fatal)
                    Return Nothing
                End If

                listOfInsuranceFolders.ForEach(Sub(i) mQueue.Enqueue(i))

                Return jobDetail

            Catch ex As Exception

                OnStatusUpdate(ex.Message, StatusLevel.Fatal)
                Return Nothing

            End Try
        End Function

        ''' <summary>
        ''' Loads the XML from the given file name to the DB and the queue.
        ''' </summary>
        ''' <param name="xmlPath">Path to XML file</param>
        ''' <remarks>Populates the queue with the insurance folder record ID</remarks>
        Private Function LoadXmlPopulateQueue(ByVal xmlPath As String) As JobDetail

            'Dim connection As SiriusConnection = Nothing
            Dim dataContext As New DataContext
            Dim jobDetail As JobDetail

            ' Check if the path is valid.
            If String.IsNullOrEmpty(xmlPath) OrElse Not File.Exists(xmlPath) Then
                OnStatusUpdate(String.Format(mCulture,
                                         "!!! Batch aborted !!! The path to the XML file is invalid. Path provided: {0}",
                                         xmlPath), StatusLevel.Fatal)
                Return Nothing
            End If

            Try

                Dim schemaSet As New XmlSchemaSet

                ' Add the schema to the collection.
                schemaSet.Add("http://www.siriusfs.com/SFI/Batch/Renewals/20120815", "RenewalList.xsd")

                ' Set the validation settings.
                Dim settings As New XmlReaderSettings()
                settings.ValidationType = ValidationType.Schema
                settings.Schemas = schemaSet
                AddHandler(settings.ValidationEventHandler), AddressOf ValidationCallBack

                ' Create the XmlReader object.
                Using reader As XmlReader = XmlReader.Create(xmlPath, settings)

                    reader.Read()

                    'Get Attributes from HEAD Element it is always the first element
                    Dim insuranceFolderCount As Integer = CInt(reader.GetAttribute("insurance_folder_count"))
                    Dim batchRenewalJobCode As String = reader.GetAttribute("batch_renewal_job_code")
                    Dim batchReference As String = reader.GetAttribute("batch_reference")
                    Dim batchId As Integer? = CType(IIf(reader.GetAttribute("batch_id") Is Nothing, Nothing,
                                                        Convert.ToInt32(reader.GetAttribute("batch_id"), mCulture)),
                                                    Integer?)
                    batchReference = CStr(IIf(String.IsNullOrEmpty(batchReference), Guid.NewGuid().ToString(),
                                              batchReference))

                    OnStatusUpdate("Batch Details:", StatusLevel.Information)
                    OnStatusUpdate(String.Format("    Insurance Folders in File: {0}", insuranceFolderCount), StatusLevel.Information)
                    OnStatusUpdate(String.Format("    Batch Id                 : {0}", batchId), StatusLevel.Information)
                    OnStatusUpdate(String.Format("    Batch Renewal Job Code   : {0}", batchRenewalJobCode), StatusLevel.Information)
                    OnStatusUpdate(String.Format("    Batch Reference          : {0}", batchReference), StatusLevel.Information)

                    'If the batch id is provided, select the batch's data from the DB and ignore the rest of the XML file.
                    If batchId.HasValue Then
                        ' Populate the queue with the batch's existing data.
                        Return ResumeBatchPopulateQueue(batchId.Value)
                    End If

                    dataContext.BeginTransaction()
                    batchId = dataContext.ExecuteInsertBatch(insuranceFolderCount, batchReference, "RENEWAL_JOB")

                    jobDetail = dataContext.RetrieveRenewalJobFromJobCode(batchRenewalJobCode)
                    If jobDetail Is Nothing Then
                        Return Nothing
                    End If

                    jobDetail.BatchId = batchId.Value

                    'Process Insurance_folder nodes
                    Dim count As Integer = 0
                    While reader.Read
                        If reader.IsStartElement Then 'INSURANCE FOLDER
                            Dim insuranceFolderCnt As Integer = CInt(reader.GetAttribute("insurance_folder_cnt"))
                            Dim recalculateCommission As Boolean = Convert.ToBoolean(reader.GetAttribute("recalculate_commission"), mCulture)
                            Dim recalculateFees As Boolean = Convert.ToBoolean(reader.GetAttribute("recalculate_fees"), mCulture)
                            Dim recalculateTaxes As Boolean = Convert.ToBoolean(reader.GetAttribute("recalculate_taxes"), mCulture)

                            dataContext.ExecuteInsertInsuranceFolder(batchId.Value, insuranceFolderCnt, recalculateCommission,
                                                         recalculateFees, recalculateTaxes, Nothing, jobDetail.JobId)
                            mQueue.Enqueue(insuranceFolderCnt)
                            While reader.Read
                                'This is the closing tag for INSURANCE FOLDER
                                If Not reader.IsStartElement Then
                                    Exit While
                                End If

                                Dim riskFolderCnt As Integer = CInt(reader.GetAttribute("risk_folder_cnt"))
                                Dim rerate As Boolean = Convert.ToBoolean(reader.GetAttribute("rerate"), mCulture)
                                Dim recalculateReinsurance As Boolean = Convert.ToBoolean(reader.GetAttribute("recalculate_reinsurance"),
                                                                                          mCulture)
                                Dim recalculateRiskFees As Boolean = Convert.ToBoolean(reader.GetAttribute("recalculate_fees"), mCulture)
                                Dim recalculateRiskTaxes As Boolean = Convert.ToBoolean(reader.GetAttribute("recalculate_taxes"), mCulture)

                                dataContext.InsertRisk(riskFolderCnt, rerate, recalculateReinsurance, recalculateRiskFees, recalculateRiskTaxes, batchId.Value, insuranceFolderCnt)
                            End While

                            count += 1
                        End If
                    End While

                    If insuranceFolderCount <> count Then
                        Dim message As String = String.Format(mCulture,
                                                 "Batch aborted. Number of records processed ({0}) did not match specified number of records in batch header ({1}).",
                                                 count, insuranceFolderCount)

                        OnStatusUpdate(message, StatusLevel.Fatal)
                        dataContext.RollBackTransaction()
                        Return Nothing
                    End If

                    dataContext.CommitTransaction()
                End Using

                ' Check if the queue was populated with anything.
                If mQueue.Count = 0 Then
                    OnStatusUpdate(String.Format(mCulture,
                                            "Batch aborted. No records were processed from the XML file. Path provided: {0}",
                                            xmlPath), StatusLevel.Information)
                    Return Nothing
                End If

                Return jobDetail

            Catch ex As Exception
                If Not dataContext Is Nothing Then
                    dataContext.RollBackTransaction()
                End If

                OnStatusUpdate(ex.Message, StatusLevel.Fatal)
                Return Nothing
            End Try
        End Function

        ''' <summary>
        ''' Called for each configured SAM server to set up the amount of calls
        ''' to be made to the server.
        ''' </summary>
        ''' <param name="instance">SAM server settings.</param>
        ''' <param name="jobDetail">Details of the job</param>
        Private Sub ActionSamInstance(ByVal instance As InstanceElement, jobDetail As JobDetail)
            Dim startTime As DateTime = Date.Now()
            Dim taskArray() As Task

            ReDim taskArray(instance.ConcurrentLimit - 1)

            For i As Integer = 0 To taskArray.Length - 1
                taskArray(i) = Task.Factory.StartNew(Sub() ActionSamCall(instance, jobDetail))
            Next
            Task.WaitAll(taskArray)

            OnStatusUpdate(String.Format("Instance {0} finished in {1}s", instance.InstanceId, (Date.Now() - startTime).TotalSeconds), StatusLevel.Information)
        End Sub

        ''' <summary>
        ''' Gets an item from the queue and calls SAM.
        ''' </summary>
        ''' <param name="instance">SAM server settings.</param>
        ''' <param name="jobDetail">The jobs details</param>
        Private Sub ActionSamCall(ByVal instance As InstanceElement, jobDetail As JobDetail)
            Dim insuranceFolderCnt As Integer
            While mQueue.TryDequeue(insuranceFolderCnt)
                Dim result As New Result(instance, jobDetail)
                mResults.Add(result)
                result.InsuranceFolderCnt = insuranceFolderCnt

                Dim request As New BaseClasses.RunBatchRenewalCommand
                Dim bRunRenewalAcceptance As Boolean
                request.BatchId = jobDetail.BatchId
                request.InsuranceFolderKey = insuranceFolderCnt
                request.BranchCode = "HEADOFF"
                request.LoginUserName = instance.UserName

                Dim response As New BaseClasses.RunBatchRenewalCommandResponse
                If Cast.ToBoolean(jobDetail.Run_Renewal_Extended_Rule) AndAlso jobDetail.JobTypeCode = "ACC" Then
                    bRunRenewalAcceptance = CheckServiceLevel(jobDetail.Code, jobDetail.JobTypeCode, insuranceFolderCnt, jobDetail.JobId)
                    If bRunRenewalAcceptance Then
                        ApiClient._tokenModel = GetApiTokendetails(instance)
                        response = ApiClient.DeserializeJson(Of BaseClasses.RunBatchRenewalCommandResponse)(CStr(ApiClient.Post($"/policies/runBatchRenewal", request))) 'RunBatchRenewal(request)
                    End If
                Else
                    ApiClient._tokenModel = GetApiTokendetails(instance)
                    response = ApiClient.DeserializeJson(Of BaseClasses.RunBatchRenewalCommandResponse)(CStr(ApiClient.Post($"/policies/runBatchRenewal", request))) 'RunBatchRenewal(request)
                End If
                result.EndTime = DateTime.Now
                result.Message = response.Message
            End While
        End Sub
        Private Function GetApiTokendetails(ByVal instance As InstanceElement) As TokenModel
            Dim apiTokenDetails As TokenModel = New TokenModel()
            apiTokenDetails = GenerateToken.GetJwtTokenForBatchProcess(instance.ClientID, instance.TokenUrl)
            Dim address As String = instance.Address
            If address.EndsWith("/") Then
                address = address.Substring(0, address.Length - 1)
            End If
            apiTokenDetails.ApiBaseUrl = address
            apiTokenDetails.TokenUrl = instance.TokenUrl
            Return apiTokenDetails
        End Function
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
        ''' Creates a batch job based on the job code
        ''' </summary>
        ''' <param name="jobCode">Target Job Code</param>
        Private Function CreateBatchFromJob(jobCode As String) As JobDetail
            Dim dataConext As New DataContext
            Dim jobDetail As JobDetail
            Dim batchReference As String = Guid.NewGuid().ToString()
            Dim batchId As Integer?
            Dim targetInsuranceFiles As List(Of TargetInsuranceFile)
            Dim sInterfaceCode As String
            Try
                dataConext.BeginTransaction()
                jobDetail = dataConext.RetrieveRenewalJobFromJobCode(jobCode)
                If jobDetail Is Nothing Then
                    dataConext.RollBackTransaction()
                    OnStatusUpdate(String.Format("No active jobs found for job code: {0}", jobCode), StatusLevel.Information)
                    Return Nothing
                End If

                targetInsuranceFiles = dataConext.RetrieveTargetInsuranceFilesForJob(jobDetail.Code, jobDetail.JobTypeCode)
                If targetInsuranceFiles Is Nothing OrElse targetInsuranceFiles.Count = 0 Then
                    dataConext.RollBackTransaction()
                    OnStatusUpdate(String.Format("No policies found for job code: {0}", jobCode), StatusLevel.Information)
                    Return Nothing
                End If

                sInterfaceCode = "Renewal " + jobDetail.Batch_Job_Type_Description
                batchId = dataConext.ExecuteInsertBatch(targetInsuranceFiles.Count, batchReference, sInterfaceCode)

                If Not batchId.HasValue Then
                    dataConext.RollBackTransaction()
                    OnStatusUpdate(String.Format("Failled to create batch for job code: {0}", jobCode), StatusLevel.Information)
                    Return Nothing
                End If

                jobDetail.BatchId = batchId.Value

                For Each target As TargetInsuranceFile In targetInsuranceFiles '.OrderBy(Function(p) p.InsuranceFileCnt)
                    If Not mQueue.Contains(target.InsuranceFolderCnt) Then
                        dataConext.ExecuteInsertInsuranceFolder(batchId.Value, target.InsuranceFolderCnt, True, True, True,
                                                     target.InsuranceFileCnt, jobDetail.JobId)
                        dictPolicies.Add(target.InsuranceFolderCnt, target.InsuranceFileCnt)

                        mQueue.Enqueue(target.InsuranceFolderCnt)
                    Else
                        OnStatusUpdate(String.Format("More than one insurance file found for job code: {0}, insurance_folder: {1}, insurance_file_cnt: {2}", jobCode, target.InsuranceFolderCnt, target.InsuranceFileCnt), StatusLevel.Information)
                    End If
                Next

                dataConext.CommitTransaction()
                dataConext.UpdateBatchTask(BatchStatusComplete, batchId.Value, String.Empty, jobCode + "_" + jobDetail.JobDescription)

                Return jobDetail

            Catch ex As Exception
                If Not dataConext Is Nothing Then
                    dataConext.RollBackTransaction()
                End If
                OnStatusUpdate(ex.Message, StatusLevel.Fatal)
                Return Nothing
            End Try
        End Function

        ''' <summary>
        ''' Gets the current number of items in the queue
        ''' </summary>
        Public ReadOnly Property QueueDepth() As Integer
            Get
                Return mQueue.Count()
            End Get
        End Property

        ''' <summary>
        ''' Gets a the current run statistics
        ''' </summary>
        Public ReadOnly Property CurrentStatistics() As IEnumerable(Of RunStatistic)
            Get
                If mResults Is Nothing Then
                    Return Nothing
                End If

                Return From n In mResults
                       Group n By n.InstanceId, n.Address Into Group
                       Select New RunStatistic With {
                           .Address = Address,
                           .Processed = Group.Count(Function(p) p.EndTime.HasValue),
                           .InProgress = Group.Count(Function(p) Not p.EndTime.HasValue),
                           .MinDuration = Group.Where(Function(p) p.EndTime.HasValue).DefaultIfEmpty().Min(Of Double)(Function(n) If(n IsNot Nothing, n.Duration.TotalMilliseconds, 0)),
                           .MaxDuration = Group.Where(Function(p) p.EndTime.HasValue).DefaultIfEmpty().Max(Of Double)(Function(n) If(n IsNot Nothing, n.Duration.TotalMilliseconds, 0)),
                           .AverageDuration = Group.Where(Function(p) p.EndTime.HasValue).DefaultIfEmpty().Average(Function(n) If(n IsNot Nothing, n.Duration.TotalMilliseconds, 0))}
            End Get
        End Property

        ''' <summary>
        ''' Status update method to raise event
        ''' </summary>
        ''' <param name="status">Status message</param>
        ''' <param name="level">Level of the message</param>
        Protected Sub OnStatusUpdate(ByVal status As String, ByVal level As StatusLevel)
            RaiseEvent StatusUpdate(Me, New StatusUpdateEventArgs(status, level))
        End Sub

        ''' <summary>
        ''' It will read WCFSecurityToken from config file then return encrypted token as string
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SecuirtyToken() As String
            Dim sSecurityToken As String = String.Empty
            If System.Configuration.ConfigurationManager.AppSettings("WCFSecurityToken") IsNot Nothing Then
                sSecurityToken = System.Configuration.ConfigurationManager.AppSettings("WCFSecurityToken").ToString
            End If

            If sSecurityToken.Length > 0 Then
                Return BCrypt.Net.BCrypt.HashPassword(sSecurityToken, 10)
            Else
                Return String.Empty
            End If
        End Function

        Public Function CheckServiceLevel(ByVal jobCode As String, ByVal jobTypeCode As String, ByVal insuranceFolderCnt As Integer, ByVal jobId As Integer) As Boolean
            Dim obSIRRenewalAcc As bSIRAutomaticRenewalsAccept.Business = Nothing
            Dim m_lReturn As Long
            Dim bIsRenewalAcceptanceAllowed As Boolean = False
            Dim m_oDatabase As Object = Nothing
            ' Dim targetInsuranceFiles As List(Of TargetInsuranceFile)
            ' Dim dataConext As New DataContext
            Dim iInsuranceFileCnt As Integer
            Try


                obSIRRenewalAcc = New bSIRAutomaticRenewalsAccept.Business
                m_lReturn = obSIRRenewalAcc.Initialise(
                                                sUsername:="",
                                                sPassword:="",
                                                iUserID:=1,
                                                iSourceID:=1,
                                                iLanguageID:=1,
                                               iCurrencyID:=26,
                                                iLogLevel:=1,
                                                sCallingAppName:="BatchRenewalWinController",
                                                vDatabase:=m_oDatabase)





                If dictPolicies.ContainsKey(insuranceFolderCnt) Then
                    iInsuranceFileCnt = dictPolicies.Item(insuranceFolderCnt)
                    bIsRenewalAcceptanceAllowed = obSIRRenewalAcc.CheckServiceLevelForRenewalAcceptance(iInsuranceFileCnt, jobId)
                End If

                obSIRRenewalAcc.Dispose()
                obSIRRenewalAcc = Nothing
            Catch ex As Exception
                OnStatusUpdate(ex.Message, StatusLevel.Fatal)
                Return False
            End Try
            Return bIsRenewalAcceptanceAllowed
        End Function
    End Class
End Namespace