
Imports System.Collections.ObjectModel
Imports System.IO
Imports System.Xml

Friend MustInherit Class ImportBase

#Region "Fields"
    ' Database connection
    Protected m_oDatabase As dPMDAO.Database = Nothing

    ' Flag to indicate if this module was created in bulk import mode
    Protected m_bIsBulkImport As Boolean = False
    Protected m_ElementCount As Integer
    ' Batch id for imports and ref for updates
    Protected m_iBatchID As Integer
    Protected m_sBatchRef As String

    ' Base file parser object
    Protected m_oXML As XmlDocument = Nothing

    ' Current working element
    Protected m_oElement As XmlElement = Nothing
    Private m_sFileName As String
    Protected Const kBatchStatusComplete As String = "C"
    Protected Const kBatchStatusFailed As String = "F"
#End Region

#Region "Properties"
    ''' <summary>
    ''' Specifies the batch code for this interface
    ''' </summary>
    Public MustOverride ReadOnly Property BatchCode() As String

    ''' <summary>
    ''' Specifies the active interface name
    ''' </summary>
    Public MustOverride ReadOnly Property InterfaceName() As String

    ''' <summary>
    ''' Indicates if this model was created during a bulk import
    ''' </summary>
    Public ReadOnly Property IsBulkImport() As Boolean
        Get
            Return m_bIsBulkImport
        End Get
    End Property
    Public Property ElementCount() As Integer
        Get
            Return m_ElementCount - 1
        End Get
        Set(ByVal value As Integer)
            m_ElementCount = value
        End Set
    End Property

    ''' <summary>
    ''' Indicates if this interface supports bulk imports
    ''' </summary>
    Public Overridable ReadOnly Property SupportsBulkImport() As Boolean
        Get
            Return True
        End Get
    End Property
    ''' <summary>
    ''' Specifies the number of records in batch for this interface
    ''' </summary>
    ''' <returns></returns>
    Public MustOverride Property NoOfTotalRecords() As Integer
    ''' <summary>
    ''' Specifies the no of rejected records in the batch for this interface
    ''' </summary>
    ''' <returns></returns>
    Public MustOverride Property NoOfRejections() As Integer

    ''' <summary>
    ''' Specifies the file name in batch for this Class
    ''' </summary>
    ''' <returns></returns>
    Public Property FileName() As String
        Get
            Return m_sFileName
        End Get
        Set(ByVal value As String)
            m_sFileName = value
        End Set
    End Property


#End Region

#Region "Methods"
    ''' <summary>
    ''' Begins a new database transaction
    ''' </summary>
    Protected Sub BeginTrans()
        Dim iReturn As PMEReturnCode = m_oDatabase.SQLBeginTrans()
        If iReturn <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to begin transaction")
        End If
    End Sub

    ''' <summary>
    ''' Commites the current database transaction
    ''' </summary>
    Protected Sub CommitTrans()
        Dim iReturn As PMEReturnCode = m_oDatabase.SQLCommitTrans()
        If iReturn <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to commit transaction")
        End If
    End Sub

    ''' <summary>
    ''' Creates a new batch header for this import
    ''' </summary>
    ''' <remarks>
    ''' This assumes a standard import header. If a non-standard header is used
    ''' this method should be overriden in the interface specific derived class.
    ''' </remarks>
    Protected Overridable Sub CreateBatch()
        Dim nReturn As PMEReturnCode = PMEReturnCode.PMTrue
        Dim nSupplierId, nSiteNumber As Integer
        If m_oXML.GetHeaderAttribute("supplier_id") Is Nothing OrElse Trim(m_oXML.GetHeaderAttribute("supplier_id")) = "" Then
            nSupplierId = 0
        Else
            nSupplierId = m_oXML.GetHeaderAttribute("supplier_id")
        End If
        If m_oXML.GetHeaderAttribute("site_number") Is Nothing OrElse Trim(m_oXML.GetHeaderAttribute("site_number")) = "" Then
            nSiteNumber = 0
        Else
            nSiteNumber = m_oXML.GetHeaderAttribute("site_number")
        End If
        Try
            ' Add parameters
            AddParameterLite(m_oDatabase, "batch_code", BatchCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString, True)
            AddParameterLite(m_oDatabase, "batch_ref", IIf(m_oXML.BatchReference = "", DBNull.Value, m_oXML.BatchReference), PMEParameterDirection.PMParamInputOutput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "interface_code", InterfaceName, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "batch_id", System.DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMInteger)
            AddParameterLite(m_oDatabase, "supplier_id", nSupplierId, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, False)
            AddParameterLite(m_oDatabase, "site_number", nSiteNumber, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, False)

            ' Execute command
            nReturn = m_oDatabase.SQLAction("spu_ACT_Import_CreateBatch", "Create Batch", True)
            If nReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to execute 'spu_ACT_Import_CreateBatch'")
            End If

            ' Get batch id
            m_iBatchID = Util.ToSafeInt(m_oDatabase.Parameters.Item("batch_id").Value, 0)
            m_sBatchRef = m_oDatabase.Parameters.Item("batch_ref").Value

        Catch ex As Exception

            Throw New Exception("Unable to create new import batch", ex)

        Finally

            ' Check for successful creation of batch
            If m_iBatchID = 0 Then
                Throw New Exception(String.Format("Batch '{0}' has already been imported", m_oXML.BatchReference))
            End If

        End Try

    End Sub
    Protected Overloads Function GetHeaderAttribute(ByVal sAttribute As String) As Object
        Return m_oXML.GetHeaderAttribute(sAttribute)
    End Function

    ''' <summary>
    ''' Creates a new batch header for this import
    ''' </summary>
    ''' <remarks>
    ''' This assumes a standard import header. If a non-standard header is used
    ''' this method should be overriden in the interface specific derived class.
    ''' </remarks>
    Protected Overridable Sub UpdateBatch(ByVal BatchStatusCode As String, ByVal TotalAmount As Decimal, ByVal TotalTransactions As Integer, ByVal RejectAmount As Decimal, ByVal RejectTransactions As Integer, ByVal ImportFilename As String)

        Dim iReturn As PMEReturnCode

        Try
            ' Add parameters
            AddParameterLite(m_oDatabase, "batch_id", m_iBatchID, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)
            AddParameterLite(m_oDatabase, "company_id", DBNull.Value, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
            AddParameterLite(m_oDatabase, "batchstatus_code", BatchStatusCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "user_id", DBNull.Value, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
            AddParameterLite(m_oDatabase, "batch_ref", m_sBatchRef, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "created_date", System.DateTime.Now, PMEParameterDirection.PMParamInput, PMEDataType.PMDate)
            AddParameterLite(m_oDatabase, "authorised_date", DBNull.Value, PMEParameterDirection.PMParamInput, PMEDataType.PMDate)
            AddParameterLite(m_oDatabase, "accounting_date", DBNull.Value, PMEParameterDirection.PMParamInput, PMEDataType.PMDate)
            AddParameterLite(m_oDatabase, "comment", DBNull.Value, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "batch_type_id", DBNull.Value, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(m_oDatabase, "batch_source_id", DBNull.Value, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(m_oDatabase, "xml_object", DBNull.Value, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "total_amount", TotalAmount, PMEParameterDirection.PMParamInput, PMEDataType.PMCurrency)
            AddParameterLite(m_oDatabase, "total_transactions", TotalTransactions, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(m_oDatabase, "imported_date", System.DateTime.Now, PMEParameterDirection.PMParamInput, PMEDataType.PMDate)
            AddParameterLite(m_oDatabase, "reject_amount", RejectAmount, PMEParameterDirection.PMParamInput, PMEDataType.PMCurrency)
            AddParameterLite(m_oDatabase, "reject_transactions", RejectTransactions, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(m_oDatabase, "import_file_name", ImportFilename, PMEParameterDirection.PMParamInput, PMEDataType.PMString)

            ' Execute command
            iReturn = m_oDatabase.SQLAction("spu_ACT_Update_Batch", "Update Batch", True)
            If iReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to execute 'spu_ACT_Update_Batch'")
            End If

        Catch ex As Exception

            Throw New Exception("Unable to update batch - " & m_iBatchID, ex)

        End Try

    End Sub
    ''' <summary>
    ''' Update Batch Status for Batch Process Status
    ''' </summary>
    ''' <param name="sBatchStatusCode"></param>
    ''' <param name="nTotalTransactions"></param>
    ''' <param name="nRejectTransactions"></param>
    Protected Sub UpdateImportBatchStatus(ByVal sBatchStatusCode As String, ByVal nTotalTransactions As Integer, ByVal nRejectTransactions As Integer)

        Dim iReturn As PMEReturnCode

        Try
            ' Add parameters
            AddParameterLite(m_oDatabase, "batch_id", m_iBatchID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, True)
            AddParameterLite(m_oDatabase, "batchstatusCode", sBatchStatusCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "total_transactions", nTotalTransactions, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(m_oDatabase, "reject_transactions", nRejectTransactions, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(m_oDatabase, "FileName", FileName, PMEParameterDirection.PMParamInput, PMEDataType.PMString)

            ' Execute command
            iReturn = m_oDatabase.SQLAction("spu_Update_BatchTask", "Update Batch", True)
            If iReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to execute 'spu_ACT_Update_Batch'")
            End If

        Catch ex As Exception

            Throw New Exception("Unable to update batch - " & m_iBatchID, ex)

        End Try

    End Sub

    ''' <summary>
    ''' Used to finalise the import process if required
    ''' </summary>
    Protected Overridable Sub FinaliseImport()
    End Sub
    ''' <summary>
    ''' Used to finalise the import process if required
    ''' </summary>
    Protected Overridable Sub UpdateBatchStatus()
    End Sub

    Protected Overloads Function GetAttribute(ByVal oElement As XmlElement, ByVal sAttribute As String) As Object
        ' Check for attribute
        If oElement.HasAttribute(sAttribute) Then
            Return oElement.Attributes(sAttribute).Value
        Else
            Return Nothing
        End If
    End Function

    Protected Overloads Function GetAttribute(ByVal sAttribute As String) As Object
        ' Check for attribute
        If m_oElement.HasAttribute(sAttribute) Then
            Return m_oElement.Attributes(sAttribute).Value
        Else
            Return Nothing
        End If
    End Function

    ''' <summary>
    ''' Processes a single element from the import file
    ''' </summary>
    Protected MustOverride Sub ProcessElement()


    ''' Used to process an import file header if required
    ''' </summary>
    Protected Overridable Sub ProcessHeader()

    End Sub

    ''' <summary>
    ''' Does any work that should take place after the import file has been processed
    ''' </summary>
    Protected Overridable Sub PostImportProcessing()

    End Sub


    ''' <summary>
    ''' Process the export
    ''' </summary>
    Public Overridable Sub ProcessImport()
        Try
            ' Create batch header
            CreateBatch()

            ' Process entire file in a single transaction

            BeginTrans()

            ' Process header
            m_oElement = m_oXML.HeaderNode
            ElementCount = m_oElement.ChildNodes.Count

            ProcessHeader()
            NoOfTotalRecords = 0
            NoOfRejections = 0
            ' Process each reference
            For Each m_oElement In m_oXML
                ProcessElement()
            Next

            ' Finalise the edit (back to header node)
            m_oElement = m_oXML.HeaderNode
            FinaliseImport()

            UpdateBatchStatus()
            ' Success, commit the transaction
            CommitTrans()

            ' do any post import processing
            PostImportProcessing()

            ' Update xml file with import details
            m_oXML.Update(m_iBatchID, m_sBatchRef, DateTime.Now)

        Catch ex As Exception

            ' We failed rollback the transaction and raise the error
            RollbackTrans()
            NoOfTotalRecords = 0
            NoOfRejections = 0
            UpdateImportBatchStatus(kBatchStatusFailed, 0, 0)

            ' Raise exception
            Throw New Exception("Unable to process import file.", ex)

        End Try
    End Sub

    ''' <summary>
    ''' Rolls back the current database transaction
    ''' </summary>
    Protected Sub RollbackTrans()
        Dim iReturn As PMEReturnCode = m_oDatabase.SQLRollbackTrans()
        If iReturn <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to rollback transaction")
        End If
    End Sub

    Protected Sub SetAttribute(ByVal oElement As XmlElement, ByVal sAttribute As String, ByVal vValue As Object)
        ' Check for attribute
        If oElement.HasAttribute(sAttribute) Then
            oElement.Attributes(sAttribute).Value = vValue
        Else
            ' Create a new attribute.
            Dim newAttr As XmlAttribute = m_oElement.OwnerDocument.CreateAttribute(sAttribute)
            ' Append to element
            oElement.Attributes.Append(newAttr)
            newAttr.Value = vValue
        End If
    End Sub

    Protected Sub SetAttribute(ByVal sAttribute As String, ByVal vValue As Object)
        ' Check for attribute
        If m_oElement.HasAttribute(sAttribute) Then
            m_oElement.Attributes(sAttribute).Value = vValue
        Else
            ' Create a new attribute.
            Dim newAttr As XmlAttribute = m_oElement.OwnerDocument.CreateAttribute(sAttribute)
            ' Append to element
            m_oElement.Attributes.Append(newAttr)
            newAttr.Value = vValue
        End If
    End Sub
#End Region

#Region "Creator"
    Public Sub New(ByVal oXML As XmlDocument)

        m_oXML = oXML
        m_sFileName = System.IO.Path.GetFileName(oXML.Filename)
        ' Connect to database
        DBConnect(m_oDatabase)
    End Sub

    Protected Overrides Sub Finalize()
        ' Close database
        DBDisconnect(m_oDatabase)

        MyBase.Finalize()
    End Sub
#End Region

End Class
