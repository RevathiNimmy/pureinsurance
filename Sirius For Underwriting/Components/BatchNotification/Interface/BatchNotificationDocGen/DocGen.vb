Imports System.Threading
Imports System.Xml
Imports SharedFiles
Public NotInheritable Class DocGen 

#Region "Fields"
    Private m_sSAMURL As String
    Private m_sSAMUsername As String
    Private m_sSAMPassword As String
    Private m_sBranchCode As String
    Private m_iNumberOfThreads As Integer
    Private m_lTimeOut As Integer
    Private m_Operation As DocGenOperation
    Private m_sFilePath As String
    Private m_sDocCode As String
    Private m_sBatchRef As String
    Private m_iBatchId As Integer
    Private m_oProcessingQueue As Queue
    Protected m_oDatabase As Object = Nothing
    Private m_iThreadNumber As Integer
    Private m_sClientId As String
    Private m_sTenantId As String
    Private m_sTokenUrl As String

#End Region

#Region "Properties"
    Public ProcessingQueue As Queue
#End Region

#Region "Creator"
    Public Sub New(ByVal v_sSAMURL As String, ByVal v_sSAMUsername As String, ByVal v_sSAMPassword As String,
                   ByVal v_sBranchCode As String, ByVal v_iNumberOfThreads As Integer, ByVal v_oOperation As Integer,
                   ByVal v_sFilePath As String, ByVal v_sDocCode As String, ByVal v_sBatchRef As String, ByVal v_lTimeOut As Integer,
                   ByVal v_sClientId As String, ByVal v_sTenantId As String, ByVal v_sTokenUrl As String)

        m_sSAMURL = v_sSAMURL
        m_sSAMUsername = v_sSAMUsername
        m_sSAMPassword = v_sSAMPassword
        m_sBranchCode = v_sBranchCode
        m_iNumberOfThreads = v_iNumberOfThreads
        m_Operation = v_oOperation
        m_sFilePath = v_sFilePath
        m_sDocCode = v_sDocCode
        m_sBatchRef = v_sBatchRef
        m_lTimeOut = v_lTimeOut
        m_sClientId = v_sClientId
        m_sTenantId = v_sTenantId
        m_sTokenUrl = v_sTokenUrl
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

#End Region

#Region "Public Methods"
    Public Function ProcessDocumentGeneration() As Integer

        Try
            'Init and open m_oDatabase
            DBConnect(m_oDatabase)

            If m_Operation = DocGenOperation.Purge Then
                Purge()
                DBDisconnect(m_oDatabase)
                Exit Function
            End If

            If m_Operation = DocGenOperation.NewRun Then
                ProcessBatchCreation()
            Else
                ProcessBatchReactivation()
            End If

            GetBatch()

            ProcessQueue()

            GetBatchSummary()


        Catch ex As Exception
            Throw New ApplicationException(ex.Message, ex)
        Finally
            DBDisconnect(m_oDatabase)
        End Try
    End Function
#End Region

#Region "Private Methods"
    Private Sub ProcessBatchCreation()
        Dim inputDocument As New XmlDocument
        Dim iReturn As Integer

        CreateBatch()

        Try
            inputDocument.Load(m_sFilePath)
        Catch ex As Exception
            Throw New ApplicationException("Provided file is not valid XML - " & ex.Message)
        End Try

        Try
            Const nodeListPath As String = "/ExportRows/ExportRow"
            Dim nodes As XmlNodeList

            nodes = inputDocument.SelectNodes(nodeListPath)

            If nodes Is Nothing Then
                Throw New ApplicationException("XML structure is incorrect. Following XPath cannot be evaluated -" & nodeListPath)
            End If

            Dim rowNum As Integer
            For Each node As XmlNode In nodes
                Dim partyKey As Integer
                Dim insuranceFileKey As Integer
                Dim insuranceFolderKey As Integer
                Dim claimKey As Integer

                rowNum += 1

                'Check columns exist and process
                Dim attributeNode As XmlAttribute

                attributeNode = node.Attributes.GetNamedItem("PartyKey")
                If attributeNode Is Nothing Then
                    Throw New ApplicationException("Row " & rowNum & " - PartyKey attribute does not exist")
                Else
                    If Not Integer.TryParse(attributeNode.Value, partyKey) Then
                        Throw New ApplicationException("Row " & rowNum & " - PartyKey is not an Integer")
                    End If
                End If

                attributeNode = node.Attributes.GetNamedItem("InsuranceFileKey")
                If attributeNode Is Nothing Then
                    Throw New ApplicationException("Row " & rowNum & " - InsuranceFileKey attribute does not exist")
                Else
                    If Not Integer.TryParse(attributeNode.Value, insuranceFileKey) Then
                        Throw New ApplicationException("Row " & rowNum & " - InsuranceFileKey is not an Integer")
                    End If
                End If

                attributeNode = node.Attributes.GetNamedItem("InsuranceFolderKey")
                If attributeNode Is Nothing Then
                    Throw New ApplicationException("Row " & rowNum & " - InsuranceFolderKey attribute does not exist")
                Else
                    If Not Integer.TryParse(attributeNode.Value, insuranceFolderKey) Then
                        Throw New ApplicationException("Row " & rowNum & " - InsuranceFolderKey is not an Integer")
                    End If
                End If

                attributeNode = node.Attributes.GetNamedItem("ClaimKey")
                If attributeNode Is Nothing Then
                    claimKey = 0
                Else
                    If Not Integer.TryParse(attributeNode.Value, insuranceFolderKey) Then
                        Throw New ApplicationException("Row " & rowNum & " - InsuranceFolderKey is not an Integer")
                    End If
                End If

                'Add to database
                AddParameterLite(m_oDatabase, "batch_id", m_iBatchId, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)
                AddParameterLite(m_oDatabase, "party_key", partyKey, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
                AddParameterLite(m_oDatabase, "insurance_file_key", insuranceFileKey, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
                AddParameterLite(m_oDatabase, "insurance_folder_key", insuranceFolderKey, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
                If claimKey <> 0 Then
                    AddParameterLite(m_oDatabase, "claim_key", claimKey, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
                End If

                ' Execute command
                iReturn = m_oDatabase.SQLAction("spu_BatchNotification_Batch_Item_add", "Batch Item Add", True)
                If iReturn <> PMEReturnCode.PMTrue Then
                    Throw New ApplicationException("Unable to execute 'spu_ACT_InstalmentPlanExport_CreateBatch'")
                End If

            Next
        Catch ex As Exception
            Throw New ApplicationException("Error Processing XML File - " & ex.Message)
        End Try

        OutputLine()
        OutputLine("Batch Created")


    End Sub

    Private Sub CreateBatch()
        Dim iReturn As PMEReturnCode

        Try
            ' Add parameters
            AddParameterLite(m_oDatabase, "batch_code", "BATNOT", PMEParameterDirection.PMParamInput, PMEDataType.PMString, True)
            AddParameterLite(m_oDatabase, "batch_ref", m_sBatchRef, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "interface_code", "Batch Doc Gen", PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "batch_id", m_iBatchId, PMEParameterDirection.PMParamOutput, PMEDataType.PMLong)

            ' Execute command
            iReturn = m_oDatabase.SQLSelect("spu_ACT_Import_CreateBatch", "Create Batch", True)
            If iReturn <> PMEReturnCode.PMTrue Then
                Throw New ApplicationException("Unable to execute 'spu_ACT_InstalmentPlanExport_CreateBatch'")
            End If

            ' Get batch id
            If Convert.IsDBNull(m_oDatabase.Parameters.Item("batch_id").Value) Then
                Throw New ApplicationException("Batch already exists")
            Else
                m_iBatchId = m_oDatabase.Parameters.Item("batch_id").Value
            End If

        Catch ex As Exception
            Throw New ApplicationException("Unable to create new export batch", ex)
        End Try
    End Sub

    Private Sub ProcessBatchReactivation()
        Dim iReturn As PMEReturnCode

        Try
            ' Add parameters
            AddParameterLite(m_oDatabase, "batch_ref", m_sBatchRef, PMEParameterDirection.PMParamInput, PMEDataType.PMString, True)
            AddParameterLite(m_oDatabase, "batch_id", m_iBatchId, PMEParameterDirection.PMParamOutput, PMEDataType.PMLong)

            ' Execute command
            iReturn = m_oDatabase.SQLSelect("spu_ACT_Select_Batch_FromBatchRef", "Select Batch", True)
            If iReturn <> PMEReturnCode.PMTrue Then
                Throw New ApplicationException("Unable to execute 'spu_ACT_InstalmentPlanExport_CreateBatch'")
            End If

            ' Get batch id
            If m_oDatabase.Parameters.Item("batch_id").Value Is Nothing Then
                Throw New ApplicationException("Batch does not exist")
            Else
                m_iBatchId = m_oDatabase.Parameters.Item("batch_id").Value
            End If

            AddParameterLite(m_oDatabase, "batch_id", m_iBatchId, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)
            iReturn = m_oDatabase.SQLAction("spu_BatchNotification_Batch_reactivate", "Select Batch", True)
            If iReturn <> PMEReturnCode.PMTrue Then
                Throw New ApplicationException("Unable to execute 'spu_BatchNotification_Batch_reactivate'")
            End If
            Console.WriteLine("Batch Reactivated")
        Catch ex As Exception
            Throw New ApplicationException("Unable to create new export batch", ex)
        End Try

    End Sub

    Private Sub GetBatch()
        Dim iReturn As PMEReturnCode
        Dim vResults As Object = Nothing
        m_oProcessingQueue = New Queue

        AddParameterLite(m_oDatabase, "batch_id", m_iBatchId, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)
        iReturn = m_oDatabase.SQLSelect("spu_BatchNotification_Batch_sel", "Select Batch", True, -1, vResults)
        If iReturn <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("Unable to execute 'spu_BatchNotification_Batch_sel'")
        End If

        If TypeOf vResults Is String Or vResults Is Nothing Then
            Throw New ApplicationException("Batch does not have any documents to generate")
        End If

        Dim iRow As Integer
        For iRow = LBound(vResults, 2) To UBound(vResults, 2)
            Dim oDocGenItem As DocGenItem
            oDocGenItem = New DocGenItem(vResults(0, iRow), vResults(1, iRow), vResults(2, iRow), vResults(3, iRow), IIf(vResults(4, iRow) = "", 0, vResults(4, iRow)), m_sDocCode)
            m_oProcessingQueue.Enqueue(oDocGenItem)
        Next
    End Sub

    Private Sub ProcessQueue()
        Dim threadCollection As New List(Of Thread)
        Dim totalQueueItems As Integer
        Dim worker As Thread
        Dim exited As Boolean

        totalQueueItems = m_oProcessingQueue.Count

        Console.WriteLine("Processing Batch")

        If m_iNumberOfThreads = 1 Then
            'Only 1 thread, just perform action
            SetupWorker()
        Else
            'Loop through and start each thread. Adding to collection
            For i As Integer = 0 To m_iNumberOfThreads - 1
                worker = New Thread(AddressOf SetupWorker)
                worker.IsBackground = True
                worker.Start()
                threadCollection.Add(worker)
            Next

            'Loop until all threads have stopped
            While True

                exited = False

                For Each th As Thread In threadCollection
                    If th.ThreadState = ThreadState.Stopped Then
                        threadCollection.Remove(th)
                        exited = True
                        Exit For
                    End If
                Next

                If Not exited Then
                    Thread.Sleep(5000)
                    SyncLock m_oProcessingQueue.SyncRoot
                        Console.WriteLine("Processing – " & m_oProcessingQueue.Count.ToString & " of " & _
                                           totalQueueItems.ToString & " remaining")
                    End SyncLock
                End If

                If threadCollection.Count = 0 Then
                    Exit While
                End If
            End While
        End If
        Console.WriteLine("Batch Processing Complete")

    End Sub

    Private Sub SetupWorker()
        m_iThreadNumber += 1
        Dim worker As DocGenWorker
        worker = New DocGenWorker(m_iThreadNumber, m_oProcessingQueue,
               m_sSAMURL, m_sSAMUsername, m_sSAMPassword,
               m_sBranchCode, m_lTimeOut, m_sClientId, m_sTenantId, m_sTokenUrl)
        worker.start()
    End Sub

    Private Sub GetBatchSummary()
        Dim iReturn As Integer
        Dim oResults As Object = Nothing
        AddParameterLite(m_oDatabase, "batch_id", m_iBatchId, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)
        iReturn = m_oDatabase.SQLSelect("spu_BatchNotification_Batch_summary", "BatchNotification Batch Summary", True, -1, oResults)
        If iReturn <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to execute 'spu_BatchNotification_Batch_summary'")
        End If
        OutputLine(oResults(1, 0) & " of " & oResults(0, 0) & " documents generated")

        If oResults(0, 0) > oResults(1, 0) Then
            OutputLine()
            Output("Failure Report:")
            OutputLine()
            AddParameterLite(m_oDatabase, "batch_id", m_iBatchId, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)
            iReturn = m_oDatabase.SQLSelect("spu_BatchNotification_Batch_Failure_sel", "BatchNotification Batch Summary", True, , oResults)
            If iReturn <> PMEReturnCode.PMTrue Then
                Throw New ApplicationException("Unable to execute 'Call spu_BatchNotification_Batch_Failure_sel'")
            End If

            Dim iRow As Integer

            For iRow = LBound(oResults, 2) To UBound(oResults, 2)
                Console.WriteLine("PK: " & oResults(1, iRow).ToString & _
                    ", IFILE: " & oResults(2, iRow).ToString & _
                    ", IFOLDER: " & oResults(3, iRow).ToString & _
                    ", Reason: " & oResults(5, iRow).ToString)
            Next
            Console.WriteLine("")
        End If
    End Sub

    Private Sub Purge()
        Dim iReturn As Integer
        AddParameterLite(m_oDatabase, "batch_ref", m_sBatchRef, PMEParameterDirection.PMParamInput, PMEDataType.PMString, True)
        iReturn = m_oDatabase.SQLAction("spu_BatchNotification_Purge", "sBatchNotification Purge", True)
        If iReturn <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("Unable to execute 'spu_BatchNotification_Purge'")
        End If
        OutputLine("Purge Successful")
    End Sub
#End Region

End Class
