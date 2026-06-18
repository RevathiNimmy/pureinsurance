Imports System.Collections.ObjectModel
Imports System.IO

Friend Class ImportFile
#Region "Fields"
    ' Construct on first use
    Private m_sImportedPath As String = String.Empty
    Private m_sImportFullPath As String = String.Empty
    Private m_sRequiredInterface As String = String.Empty
    Private m_sInterfaceName As String = String.Empty

    ' Internal XML object for this file
    Private m_oXML As XmlDocument

    ' Is this a bulk import?
    Private m_bIsBulkImport As Boolean
#End Region

#Region "Properties"
    Public ReadOnly Property ImportedFullPath() As String
        Get
            ' Return an appropriate export filename, the imported path plus original filename
            Return Path.Combine(ImportedPath, ImportFilename)
        End Get
    End Property

    Public ReadOnly Property ImportedPath() As String
        Get
            ' If we haven't got the path yet, get it
            If (m_sImportedPath.Length = 0) Then
                m_sImportedPath = GetSystemOption(ACImportedPathOption)
            End If

            ' Did we find a path?
            If (m_sImportedPath.Length = 0) Then
                ' If not move imported files to a sub folder named "Imported"
                m_sImportedPath = Path.Combine(ImportPath, "Imported")
            End If

            ' Check it exists
            If Not Directory.Exists(m_sImportedPath) Then
                ' If we can create it do so, else raise error
                Directory.CreateDirectory(m_sImportedPath)
            End If

            ' If we made it this far return the path
            Return m_sImportedPath
        End Get
    End Property

    Public ReadOnly Property ImportFilename() As String
        Get
            Return Path.GetFileName(ImportFullPath)
        End Get
    End Property

    Public ReadOnly Property ImportFullPath() As String
        Get
            Return m_sImportFullPath
        End Get
    End Property

    Public ReadOnly Property ImportPath() As String
        Get
            Return Path.GetDirectoryName(m_sImportFullPath)
        End Get
    End Property

    Public ReadOnly Property RequiredInterface() As String
        Get
            Return m_sRequiredInterface
        End Get
    End Property

    Public WriteOnly Property InterfaceName() As String
        Set(ByVal value As String)
            m_sInterfaceName = value
        End Set
    End Property

#End Region


#Region "Methods"
    Protected Sub CreateWorkManagerTask(ByVal sDescription As String)
        Dim iReturn As PMEReturnCode
        Dim oDatabase As dPMDAO.Database = Nothing

        Try
            ' Connect to db
            DBConnect(oDatabase)

            ' Add parameters
            AddParameterLite(oDatabase, "pmwrk_task_instance_cnt", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMLong, True)
            AddParameterLite(oDatabase, "pmwrk_task_group_id", 5, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(oDatabase, "pmwrk_task_id", 18, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(oDatabase, "customer", "System adminstration", PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(oDatabase, "task_due_date", DateTime.Now, PMEParameterDirection.PMParamInput, PMEDataType.PMDate)
            AddParameterLite(oDatabase, "pmuser_group_id", 1, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(oDatabase, "user_id", DBNull.Value, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(oDatabase, "description", sDescription, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(oDatabase, "task_status", 0, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(oDatabase, "is_urgent", 1, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(oDatabase, "date_created", DateTime.Now, PMEParameterDirection.PMParamInput, PMEDataType.PMDate)
            AddParameterLite(oDatabase, "created_by_id", 1, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(oDatabase, "last_modified", DBNull.Value, PMEParameterDirection.PMParamInput, PMEDataType.PMDate)
            AddParameterLite(oDatabase, "modified_by_id", DBNull.Value, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(oDatabase, "is_visible", 1, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(oDatabase, "workflow_information", DBNull.Value, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)

            ' Execute command
            iReturn = oDatabase.SQLAction("spe_PMWrk_Task_Instance_add", "Create WMTask", True)
            If iReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to execute 'spe_PMWrk_Task_Instance_add'")
            End If
        Catch ex As Exception
            Throw New Exception("Unable to create work manager task", ex)
        Finally
            ' Disconnect from db
            DBDisconnect(oDatabase)
        End Try
    End Sub

    ' Process the export routine
    Public Overridable Sub ProcessFile()
        Dim oImport As ImportBase

        Try

            ' Validate the file against our known schemas
            m_oXML.Validate()

            If m_oXML.IsAutoRecProcess Then

                oImport = New Agent_Reconciliation_Import(m_oXML)
                oImport.ProcessImport()
            Else
                If UCase(m_sInterfaceName) <> UCase(m_oXML.InterfaceName) And Len(Trim(m_sInterfaceName)) > 0 Then Exit Sub

                ' Keep user up to date information
                OutputLine(String.Format("Processing '{0}'...", ImportFilename))

                ' Get the approriate interface to process file
                oImport = GetInterface(m_oXML.InterfaceName)

                ' Process the import
                oImport.ProcessImport()

                ' Release import object to free file
                oImport = Nothing
            End If
            Try
                If Not m_oXML.IsAutoRecProcess Then
                    ' Move file to processed directory
                    File.Move(ImportFullPath, ImportedFullPath)
                End If

                If CloudHostingEnabled Then
                    Dim targetFileName As String = ImportedFullPath.Substring(ConfiguredImportedPath.Length).Replace("\", "/")
                    Dim result As Integer
                    Using fileStream As Stream = New FileStream(ImportedFullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                        Dim fileBytes(0 To fileStream.Length - 1) As Byte
                        fileStream.Read(fileBytes, 0, fileBytes.Length)

                        result = CloudRepository.UploadFile(Environment.GetEnvironmentVariable("AWS_IMPORTEXPORT_FOLDER_NAME") & "/" & CloudProcessedFolder & targetFileName, fileBytes).Result
                    End Using

                    If result = gPMConstants.PMEReturnCode.PMTrue Then
                        Dim s3ImportFile As String = Environment.GetEnvironmentVariable("AWS_IMPORTEXPORT_FOLDER_NAME") & "/" & CloudImportFolder & ImportFullPath.Substring(ConfiguredImportPath.Length).Replace("\", "/")
                        CloudRepository.DeleteFile(s3ImportFile)
                    End If
                End If
            Catch ex As System.IO.IOException

                ' Create a work manager task to indicate this file failed
                CreateWorkManagerTask(String.Format("Import Completed but unable to move import file from {0} to {1}.", ImportFullPath, ImportedFullPath))

            End Try

            ' Output success
            OutputLine(String.Format("SUCCEEDED"))

        Catch ex As Exception

            If Not String.IsNullOrEmpty(ex.InnerException.ToString) Then
                ' Create a work manager task to indicate this file failed
                CreateWorkManagerTask(String.Format("Import Failed for {0}. {1}", ImportFilename, ex.InnerException.ToString()))
            Else
                ' Create a work manager task to indicate this file failed
                CreateWorkManagerTask(String.Format("Import Failed for {0}. {1}", ImportFilename, ex.Message.ToString()))
            End If

            ' An error or import exception occurred, report the message and the log
            OutputLine("FAILED:")
            OutputError(ex)
        End Try
    End Sub

    Private Function GetInterface(ByVal sInterfaceName As String) As ImportBase
        ' Check the required interface type

        Select Case sInterfaceName
            Case "INSTALMENTS_IMPORT"
                Return New Instalment_Import(m_oXML)
            Case "PAYMENT_IMPORT"
                Return New Payment_Import(m_oXML)
            Case "RECEIPT_IMPORT"
                Return New Receipt_Import(m_oXML)
            Case "REFERENCE_IMPORT"
                Return New Reference_Import(m_oXML)
            Case "BANK_RECONCILIATION_IMPORT"
                Return New Bank_Reconciliation_Import(m_oXML)
            Case "COVER_NOTE_IMPORT"
                Return New Cover_Note_Import(m_oXML)
            Case "EXCHANGE_RATES_IMPORT"
                Return New Exchange_Rates_Import(m_oXML)
            Case "POLICY_BDX_IMPORT"
                Return New Policy_BDX_Import(m_oXML)
            Case "MID_IMPORT"
                Return New MID_Import(m_oXML)
            Case "MID2_IMPORT"
                Return New MID2_Import(m_oXML)
            Case "CASH_ALLOCATION_IMPORT"
                Return New Cash_Allocation_Import(m_oXML)
            Case Else
                Throw New ArgumentException(String.Format("Interface '{0}' not supported", sInterfaceName), "sInterfaceName")
        End Select
    End Function
#End Region

#Region "Creator"
    Public Sub New(ByVal sFilename As String, ByVal bIsBulkImport As Boolean)
        ' Store filename and bulk import flag
        m_sImportFullPath = sFilename
        m_bIsBulkImport = bIsBulkImport

        ' Create xml document reference
        m_oXML = New XmlDocument(sFilename)
    End Sub
#End Region

End Class
