Imports System.Collections.ObjectModel
Imports System.Xml
Imports System.Xml.Schema
Imports SharedFiles

Public NotInheritable Class Payment_Export : Inherits ExportBase 

#Region "Fields"

    Private m_iBatchID As Integer = 0
    Private m_sBankAccountName As String = ""
    Private m_sMediaTypeCode As String = ""
    Private m_sLeadDays As Integer = 0
    Private m_iNewBatch As Byte = 0
    Private m_sCashListItemPaymentType As String = Nothing

#End Region

#Region "Public Properties"
    ' Builds the export filename for this interface
    Public Overrides ReadOnly Property Filename() As String
        Get
            If (m_sFilename.Length = 0) Then
                m_sFilename = String.Format("{0}_{1}_{2}.xml", InterfaceName, m_iBatchID, Now.ToString("yyyyMMddhhmm"))
            End If
            Return m_sFilename
        End Get
    End Property

    ' Interface name
    Public Overrides ReadOnly Property InterfaceName() As String
        Get
            Return "Payment_Export"
        End Get
    End Property
    Public Overrides Property BatchId() As Integer
        Get
            Return m_iBatchId
        End Get
        Set(ByVal value As Integer)
            m_iBatchId = value
        End Set
    End Property
#End Region

#Region "Public Methods"
    ' Display help for this interface
    Public Overrides Sub DisplayHelp()

        ' Write syntax help for this command
        'OutputLine("SIRIUSEXPORT Payment_Export [batchid]")
        OutputLine("Example call : - SIRIUSEXPORT Payment_Export Batch_ID =1 bank_account_name=""Bank GB"" media_type_code=""CA"" lead_days=10")
        OutputLine()
        OutputLine("Batch_ID     :If specified a previous batch is recreated")
        OutputLine("              If no batchid is specified a new batch is created")
        OutputLine()
        OutputLine("Bank_account_name  - (optional) filter on schemes banks account name")
        OutputLine("Media_type_code    - (optional)  e.g. Cash -> CA")
        OutputLine("lead_days          - (optional) Numeric")
        OutputLine("Payment_Type_Code  - (optional) String")

    End Sub

    ' Process the command line arguments supplied for this interface
    ' This Procedure is used to segregate the arguments supplied from commandLine
    Public Overrides Sub ProcessCommandLine(ByVal cArgs As Collection(Of String))
        ' process tight structure
        'If cArgs.Count >= 0 Then

        Dim NoofCommandLineArgs As Integer
        Dim lItem As Integer = 0
        Dim sArg As String
        Dim sArgValues() As String

        ' get the number of command line arguments passed
        NoofCommandLineArgs = cArgs.Count - 1

        ' for each command line argument passed
        For lItem = 0 To NoofCommandLineArgs

            ' get the argument (should be in the format (argument_name = argument_value)
            sArg = cArgs(lItem).ToString()

            ' split the argument into argument name / argument value
            sArgValues = sArg.Split(CChar("="))

            Try

                ' determine which argument we are looking at
                Select Case sArgValues(0).ToUpper

                    Case "BATCH_ID"
                        m_iBatchID = CInt(sArgValues(1))

                    Case "BANK_ACCOUNT_NAME"
                        m_sBankAccountName = sArgValues(1)

                    Case "MEDIA_TYPE_CODE"
                        m_sMediaTypeCode = sArgValues(1)

                    Case "LEAD_DAYS"
                        m_sLeadDays = sArgValues(1)

                    Case "PAYMENT_TYPE_CODE"
                        m_sCashListItemPaymentType = sArgValues(1)

                End Select

            Catch ex As Exception

                Throw New ArgumentException("Invalid argument " + sArgValues(0).ToString, ex)
            End Try
        Next
        'Else
        ' if the command line argument - media type code has not been found
        'Dim ex As Exception = Nothing
        ' raise an exception
        'Throw New ArgumentException("Mandatory command line argument not found - media type code", ex)
        'End If
    End Sub

    ' Process the export
    Public Overrides Sub ProcessExport()

        ' Write status line
        OutputLine("Payments Export")
        OutputLine()

        ' Existing batch or new one?
        If m_iBatchID > 0 Then
            OutputLine(String.Format("Recreating batch {0}", m_iBatchID))
        Else
            Output("Creating new batch...")
            CreateBatch()
            OutputLine(String.Format("Created batch {0}", m_iBatchID))
        End If

        ' Export the batch 
        ExportBatch()
        UpdateBatchTask(kBatchStatusComplete, m_iBatchID, Filename, 0, 0)
    End Sub
#End Region

#Region "Private Methods"
    'Method used to create the batch if no batch id is supplied(m_batchID <=0)
    Private Sub CreateBatch()
        Dim nReturn As PMEReturnCode = PMEReturnCode.PMTrue

        Try
            ' Add parameters
            AddParameterLite(m_oDatabase, "batch_id", System.DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMInteger, True)
            AddParameterLite(m_oDatabase, "MediaTypeCode", m_sMediaTypeCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "bank_account_name", m_sBankAccountName, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            ' Execute command
            nReturn = m_oDatabase.SQLAction("spu_ACT_PaymentExport_CreateBatch", "Create Batch", True)
            If nReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to execute 'spu_ACT_PaymentsExport_CreateBatch'")
            End If

            ' Get batch id
            m_iBatchID = m_oDatabase.Parameters.Item("batch_id").Value
            m_iNewBatch = 1
        Catch ex As Exception
            Throw New Exception("Unable to create new export batch", ex)
        End Try
    End Sub

    Public Sub ExportBatch()

        'Method used to create an XML file in a format defined by Payment_Export.XSD
        Dim iReturn As Long
        Dim oXML As New XmlDocument ' MSXML2.DOMDocument = New MSXML2.DOMDocument
        Dim oPI As Xml.XmlProcessingInstruction ' MSXML2.IXMLDOMProcessingInstruction
        Dim sFileName As String
        Dim vResult(,) As Object = Nothing
        Dim sSql As String
        Dim oXMLReaderSettings As XmlReaderSettings = New XmlReaderSettings()
        Dim oXMLReader As XmlReader = Nothing
        Dim nQueryTimeOut As Integer = 0

        ' Validate whether batch no is Payment Export Batch No or Not
        sSql = "Select batch_ref from batch where batch_id=" & m_iBatchID
        iReturn = m_oDatabase.SQLSelect(sSql, "name", False, , vResultArray:=vResult)
        If vResult Is String.Empty Then
            OutputLine(String.Format("Invalid Batch ID for Payment Export.", m_iBatchID))
            Exit Sub
        End If

        If vResult Is Nothing OrElse vResult.ToString() = "" Then
            OutputLine(String.Format("Invalid Batch ID for Payment Export.", m_iBatchID))
            Exit Sub
        End If

        If Left(vResult(0, 0), 4) <> "SPYX" Then
            OutputLine(String.Format("Invalid Batch ID for Payment Export.", m_iBatchID))
            Exit Sub
        End If

        ' Add the parameters required for the SP's execution
        Output("Retrieving batch...")
        AddParameterLite(m_oDatabase, "batch_id", m_iBatchID, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)
        AddParameterLite(m_oDatabase, "bank_account_name", m_sBankAccountName, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        AddParameterLite(m_oDatabase, "media_type_code", m_sMediaTypeCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        'Developer Guide no. 252
        'AddParameterLite(m_oDatabase, "lead_days", m_sLeadDays, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        AddParameterLite(m_oDatabase, "lead_days", m_sLeadDays, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
        AddParameterLite(m_oDatabase, "new_batch", m_iNewBatch, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)

        If Not String.IsNullOrEmpty(m_sCashListItemPaymentType) Then
            AddParameterLite(m_oDatabase, "cashlistitem_payment_type", m_sCashListItemPaymentType, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        End If
        nQueryTimeOut = QueryTimeOut()

        If nQueryTimeOut > 0 Then
            m_oDatabase.QueryTimeout = nQueryTimeOut
        End If
        ' Execute the SP which will return the XML String and store it in DOMDocument
        iReturn = m_oDatabase.SQLSelectForXML("spu_ACT_PaymentExport_XML_Select", True, oXML)

        ' Check for xml
        If oXML.InnerXml.Length = 0 Then
            OutputLine(String.Format("Batch {0} does not exist", m_iBatchID))
        Else
            OutputLine("Done")
            Output(String.Format("Writing output file...{0}...", Filename))

            ' Create the processing instruction (header) for this file.
            oPI = oXML.CreateProcessingInstruction("xml", "version=""1.0""")
            oXML.InsertBefore(oPI, oXML.FirstChild)

            ' Tidy up the header wrapper
            GenerateSchemaHeader(oXML)

            ' Save XML
            sFileName = FullPath
            oXML.Save(sFileName)
            OutputLine("Done")

            'Block Coded to validate the XML file against the XSD file
            Try

                OutputLine("Validating Exported XML File Format")

                oXMLReaderSettings.Schemas.Add("http://www.siriusfs.com/SFI/Export/Payment_Export/20060420", System.AppDomain.CurrentDomain.BaseDirectory() & "\" & "Payment_Export.xsd")
                oXMLReaderSettings.ValidationType = ValidationType.Schema
                oXMLReader = XmlReader.Create(sFileName, oXMLReaderSettings)

                While oXMLReader.Read()
                End While
                oXMLReader.Close()

                OutputLine("Validation Completed")

            Catch ex As XmlException
                UpdateBatchTask(kBatchStatusFailed, m_iBatchID, Filename, 0, 0)
                OutputLine("Invalid XML File Format")
                Throw New ApplicationException("Export file has an invalid XML File Format", ex)

            Finally

                oXMLReader = Nothing
                oXMLReaderSettings = Nothing

            End Try

        End If
    End Sub

    Public Overrides Sub CleanUpInterops()

        ' clean up the database interop
'        DestroyCOMInterop(m_oDatabase, True)
		m_oDatabase = Nothing

    End Sub

#End Region

End Class
