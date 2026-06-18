Imports System.Collections.ObjectModel
Imports System.Xml
Imports System.Xml.Schema
Imports SharedFiles

Public NotInheritable Class Instalment_Export : Inherits ExportBase

#Region "Fields"
    Private m_iBatchID As Integer = 0
    Private m_sBatchRef As String = ""
    Private m_iLeadDays As Integer = 0
    Private m_sBankAccountName As String = ""
    Private m_sMediaTypeCode As String = ""
    Private m_bAutoPost As Boolean = False
    Private m_iDueDay As Integer = 0
    Private m_bInstalmentNotification As Boolean
    Private m_bPaymentHubEnabled As Boolean = False
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
            Return "Instalment_Export"
        End Get
    End Property
    Public Overrides Property BatchId() As Integer
        Get
            Return m_iBatchID
        End Get
        Set(ByVal value As Integer)
            m_iBatchID = value
        End Set
    End Property

#End Region

#Region "Public Methods"
    ' Display help for this interface
    Public Overrides Sub DisplayHelp()
        ' Write syntax help for this command
        OutputLine("Example call : - SIRIUSEXPORT Instalment_Export media_type_code=""DD"" lead_days=10 bank_account_name=""Bank GB"" autopost=false Instalment_Notification=False")
        OutputLine()
        OutputLine("  media_type_code    - (mandatory)  e.g Cash -> CA")
        OutputLine("  lead_days          - (optional) numeric default(0 -> (instalments due today)), (1 -> (instalments due today + due tomorrow) etc)")
        OutputLine("  bank_account_name  - (optional) filter on schemes banks account name")
        OutputLine("  autopost           - (optional) defaults to false (dont post) true ( post )")
        OutputLine("  instalment_notification – (optional) process instalment notification documents. Defaults to false.")
        OutputLine()
    End Sub

    ' Process the command line for this interface
    Public Overrides Sub ProcessCommandLine(ByVal cArgs As Collection(Of String))

        ' process tight structure
        If cArgs.Count > 0 Then

            Dim NoofCommandLineArgs As Integer
            Dim lItem As Integer = 0
            Dim sArg As String
            Dim sArgValues() As String
            Dim bMediaTypeCodePassed As Boolean
            Dim bBatchIDPassed As Boolean

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

                        Case "MEDIA_TYPE_CODE"
                            m_sMediaTypeCode = sArgValues(1)
                            bMediaTypeCodePassed = True

                        Case "BANK_ACCOUNT_NAME"
                            m_sBankAccountName = sArgValues(1)

                        Case "AUTOPOST"
                            m_bAutoPost = CBool(sArgValues(1))

                        Case "LEAD_DAYS"
                            m_iLeadDays = CInt(sArgValues(1))

                        Case "BATCH_ID"
                            m_iBatchID = CInt(sArgValues(1))
                            bBatchIDPassed = True
                        Case "INSTALMENT_NOTIFICATION"
                            m_bInstalmentNotification = CBool(sArgValues(1))
                        Case "DUE_DATE"
                            m_iDueDay = CInt(sArgValues(1))
                    End Select

                Catch ex As Exception

                    Throw New ArgumentException("Invalid argument " + sArgValues(0).ToString, ex)

                End Try

            Next

            ' if the command line argument - media type code has not been found
            If bMediaTypeCodePassed = False And bBatchIDPassed = False Then
                Dim ex As Exception = Nothing
                ' raise an exception
                Throw New ArgumentException("Mandatory command line argument not found - media type code", ex)
            End If

            Dim sOptionValue As String = "0"

            sOptionValue = GetSystemOption(kSystemOptionPaymentHubEnabled)
            m_bPaymentHubEnabled = (sOptionValue = "1")

            If (m_sMediaTypeCode.ToUpper = "OCP" OrElse m_sMediaTypeCode.ToUpper = "CC") AndAlso m_bPaymentHubEnabled Then
                m_bAutoPost = True
            End If

        Else
            ' if the command line argument - media type code has not been found
            Dim ex As Exception = Nothing
            ' raise an exception
            Throw New ArgumentException("Mandatory command line argument not found - media type code", ex)
        End If
    End Sub

    ' Process the export
    ''' <summary>
    ''' Process Export
    ''' </summary>
    Public Overrides Sub ProcessExport()

        ' Write status line
        If m_iDueDay > 0 Then
            OutputLine(String.Format("Instalment Export media_type_code={0} leaddays={1} bank_account_name={2} autopost={3} duedate={4}", m_sMediaTypeCode, m_iLeadDays, m_sBankAccountName, m_bAutoPost, m_iDueDay))
        Else
            OutputLine(String.Format("Instalment Export media_type_code={0} leaddays={1} bank_account_name={2} autopost={3}", m_sMediaTypeCode, m_iLeadDays, m_sBankAccountName, m_bAutoPost))
        End If

        OutputLine()

        ' Existing batch or new one?
        If m_iBatchID > 0 Then
            OutputLine(String.Format("Recreating batch {0}", m_iBatchID))

            ' Export the old batch 
            ExportBatch(False)
        Else
            Output("Creating new batch...")
            CreateBatch()
            OutputLine(String.Format("Created batch {0}", m_iBatchID))
            ExportBatch(True)
            UpdateBatchTask(kBatchStatusComplete, m_iBatchID, Filename, 0, 0)
        End If

        ' if auto post has been requested
        If m_bAutoPost = True Then
            ' post the batch 
            PostBatch()
        End If

        If m_bInstalmentNotification Then
            InstalmentNotification()
        End If
    End Sub

#End Region

#Region "Private Methods"

    Private Sub PostBatch()

        Dim oPFInstalments As bSIRPFInstalments.Business = Nothing
        Dim iReturn As PMEReturnCode

        ' declare and initalise to defaults 
        Dim sUsername As String = "sirius"
        Dim sPassword As String = "sirius"
        Dim iUserID As Integer = 1
        Dim sCallingAppName As String = ACApp
        Dim iSourceID As Integer = 1
        Dim iLanguageID As Integer = 1
        Dim iCurrencyID As Integer = 26
        Dim iLogLevel As Integer = 6
        Dim bTransactionStarted As Boolean = False
        Dim lFailedPosting As Integer
        Dim lRecordsPosted As Integer

        Try

            ' create instance of the instalments business object
            oPFInstalments = New bSIRPFInstalments.Business

            ' initialise bSIRPFinstalments component
            iReturn = oPFInstalments.Initialise(sUsername, sPassword, iUserID, iSourceID, iLanguageID,
                                        iCurrencyID, iLogLevel, sCallingAppName, vDatabase:=m_oDatabase)
            If iReturn = PMEReturnCode.PMFalse Then
                Throw New ApplicationException("bSIRPFInstalments.Business.Initialise Failed")
            End If

            ' begin transaction
            iReturn = m_oDatabase.SQLBeginTrans
            If iReturn = PMEReturnCode.PMTrue Then

                'indicate transaction started
                bTransactionStarted = True

                If (m_sMediaTypeCode.ToUpper = "OCP" OrElse m_sMediaTypeCode.ToUpper = "CC") AndAlso m_bPaymentHubEnabled Then
                    oPFInstalments.CallingAppName = "SIRIUSEXPORT"
                End If
                'post instalments
                iReturn = oPFInstalments.PostInstalments(lBatchID:=m_iBatchID, r_lRecordsPosted:=lRecordsPosted, r_lFailedPosting:=lFailedPosting)
                If iReturn = PMEReturnCode.PMFalse Then
                    Throw New ApplicationException("bSIRPFInstalments.Business.PostInstalments Failed")
                End If

                ' commit transaction
                If bTransactionStarted Then
                    iReturn = m_oDatabase.SQLCommitTrans
                    If iReturn <> PMEReturnCode.PMTrue Then
                        Throw New ApplicationException("Commit Trans Failed")
                    End If
                End If

            Else

                ' begin transaction failed
                Throw New ApplicationException("BeginTrans Failed")

            End If

        Catch ex As Exception

            ' if a transaction has been started
            If bTransactionStarted Then

                ' rollback transaction
                iReturn = m_oDatabase.SQLRollbackTrans
                If iReturn <> PMEReturnCode.PMTrue Then
                    Throw New ApplicationException("Rollback Trans Failed")
                End If

            End If

            ' throw error to calling function
            Throw New ApplicationException("PostBatch Failed", ex)

        Finally
            oPFInstalments.Dispose()
        End Try

    End Sub


    Private Sub CreateBatch()
        Dim iReturn As PMEReturnCode = PMEReturnCode.PMTrue

        Try
            ' Add parameters
            m_oDatabase.Parameters.DeleteAll()
            AddParameterLite(m_oDatabase, "batch_id", System.DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMInteger, True)
            AddParameterLite(m_oDatabase, "MediaTypeCode", m_sMediaTypeCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "bank_account_name", m_sBankAccountName, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "lead_days", m_iLeadDays, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
            ' Execute command
            iReturn = m_oDatabase.SQLAction("spu_ACT_InstalmentExport_CreateBatch", "Create Batch", True)
            If iReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to execute 'spu_ACT_InstalmentExport_CreateBatch'")
            End If

            ' Get batch id
            m_iBatchID = m_oDatabase.Parameters.Item("batch_id").Value

        Catch ex As Exception
            Throw New Exception("Unable to create new instalment export batch", ex)
        End Try
    End Sub

    Private Sub ExportBatch(ByVal bNewBatch As Boolean)

        Dim iReturn As Long
        Dim oXML As New XmlDocument ' MSXML2.DOMDocument = New MSXML2.DOMDocument
        Dim oPI As Xml.XmlProcessingInstruction ' MSXML2.IXMLDOMProcessingInstruction
        Dim m_iStatus As Integer
        Dim oXMLReaderSettings As XmlReaderSettings = New XmlReaderSettings()
        Dim oXMLReader As XmlReader = Nothing
       ' Dim sFileName As String

        ' Get batch XML
        Output("Retrieving batch...")

        AddParameterLite(m_oDatabase, "media_type_code", m_sMediaTypeCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString, True)
        AddParameterLite(m_oDatabase, "bank_account_name", m_sBankAccountName, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        AddParameterLite(m_oDatabase, "lead_days", m_iLeadDays, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
        AddParameterLite(m_oDatabase, "batch_id", m_iBatchID, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
        AddParameterLite(m_oDatabase, "status", m_iStatus, PMEParameterDirection.PMParamOutput, PMEDataType.PMLong)
        AddParameterLite(m_oDatabase, "newbatch", CInt(bNewBatch), PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
        AddParameterLite(m_oDatabase, "due_day", m_iDueDay, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)

          '  iReturn = m_oDatabase.SQLSelectForXML("spu_ACT_InstalmentExport_XML_Select", True, oXML)

        Dim oDS As Data.DataSet
        iReturn = m_oDatabase.SQLSelectForXML("spu_ACT_InstalmentExport_XML_Select", True, oDS)
        Dim iRowCount As Integer = 0
        Dim sFileName As String = FullPath

        If oDS IsNot Nothing Then
            If oDS.Tables(0) IsNot Nothing Then
                If oDS.Tables(0).Rows.Count > 0 Then

                    iRowCount = oDS.Tables(0).Rows.Count
                    Dim oFS As IO.FileStream = IO.File.Create(sFileName)
                    oFS.Close()

                    Dim oSW As IO.StreamWriter = IO.File.AppendText(sFileName)
                    oSW.Write("<?xml version=" & ChrW(34) & "1.0" & ChrW(34) & "?>" & Environment.NewLine())

                    For iCnt As Integer = 0 To oDS.Tables(0).Rows.Count - 1
                        oSW.Write(oDS.Tables(0).Rows(iCnt)(0).ToString())
                    Next
                    oSW.Close()
                    oSW = Nothing
                End If
            End If
        End If

        If iRowCount = 0 Then
            ' Check for xml
            '  If oXML.InnerXml.Length = 0 Then
            OutputLine(String.Format("Batch {0} does not exist", m_iBatchID))
        Else
            'OutputLine("Done")
           ' Output(String.Format("Writing output file...{0}...", Filename))

            ' Create the processing instruction (header) for this file.
            'oPI = oXML.CreateProcessingInstruction("xml", "version=""1.0""")
            'oXML.InsertBefore(oPI, oXML.FirstChild)

            ' Tidy up the header wrapper
            'GenerateSchemaHeader(oXML)

            ' Save XML
            'sFileName = FullPath
            'oXML.Save(sFileName)
            OutputLine("Done")

            Try

                OutputLine("Validating Exported XML File Format")

                oXMLReaderSettings.Schemas.Add("http://www.siriusfs.com/SFI/Export/Instalment_Export/20060321", System.AppDomain.CurrentDomain.BaseDirectory() & "\" & "Instalment_Export.xsd")
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
        m_oDatabase = Nothing

    End Sub
    Private Function InstalmentNotification() As Long
        Dim iReturn As Integer
        Dim iNumberOfRecords As Integer = 0
        Dim oInstalments(,) As Object
        Dim oResultArray(,) As Object
        Dim lIndex As Long
        'developer Guide no.108
        ' Dim oDocManagerWrapper As bSIRDocManagerWrapper.Interface = Nothing
        Dim oDocManagerWrapper As bSIRDocManagerWrapper.Interface_Renamed = Nothing

        Dim sUsername As String = "sirius"
        Dim sPassword As String = "sirius"
        Dim iUserID As Integer = 1
        Dim sCallingAppName As String = ACApp
        Dim iSourceID As Integer = 1
        Dim iLanguageID As Integer = 1
        Dim iCurrencyID As Integer = 26
        Dim iLogLevel As Integer = 6

        m_oDatabase.Parameters.Clear()

        iReturn = m_oDatabase.SQLSelect("spu_ACT_InstalmentNotification_Select", "Select Instalments for which notification has to be sent", True, iNumberOfRecords, oResultArray)
        If iReturn <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to execute spu_ACT_InstalmentNotifiaction_select ")
        End If

        OutputLine(String.Format("Instalment Notification Started - {0} Notifications", iNumberOfRecords))
        OutputLine()

        If iNumberOfRecords > 0 Then
            oInstalments = CType(oResultArray, [Object](,))
            'Developer guide no. 108
            'oDocManagerWrapper = New bSIRDocManagerWrapper.Interface
            oDocManagerWrapper = New bSIRDocManagerWrapper.Interface_Renamed

            iReturn = oDocManagerWrapper.InitialiseBusiness(sUsername, sPassword, iUserID, iSourceID, iLanguageID,
                                        iCurrencyID, iLogLevel, sCallingAppName, vDatabase:=m_oDatabase)

            If iReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to execute spu_ACT_InstalmentNotification_Update ")
            End If

            Dim lBound As Integer = oInstalments.GetLowerBound(1)
            Dim uBound As Integer = oInstalments.GetUpperBound(1)

            For lIndex = lBound To uBound

                oDocManagerWrapper.DocumentTemplateId = 0
                oDocManagerWrapper.PartyCnt = oInstalments(3, lIndex)
                oDocManagerWrapper.Mode = 4
                oDocManagerWrapper.InsuranceFileCnt = oInstalments(2, lIndex)
                oDocManagerWrapper.DocumentTemplateCode = oInstalments(5, lIndex)

                iReturn = oDocManagerWrapper.Start()
                If iReturn = PMEReturnCode.PMTrue Then
                    AddParameterLite(m_oDatabase, "pfinstalments_id", oInstalments(0, lIndex), PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)
                    AddParameterLite(m_oDatabase, "notification_sent", 1, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)

                    iReturn = m_oDatabase.SQLAction("spu_ACT_InstalmentNotification_Update", "Update the status of notification sent for the instalments", True)

                    If iReturn <> PMEReturnCode.PMTrue Then
                        Throw New Exception("Unable to execute spu_ACT_InstalmentNotification_Update ")
                    End If
                End If
            Next
        End If

        OutputLine(String.Format("Instalment Notification Complete"))

    End Function
#End Region

End Class

