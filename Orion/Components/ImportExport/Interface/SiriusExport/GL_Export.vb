Imports System.Collections.ObjectModel
Imports System.Xml
Imports SharedFiles
Public NotInheritable Class GL_Export : Inherits ExportBase 

#Region "Fields"
    ' Either a new or previously exported batch
    Private m_iBatchID As Integer
    Private m_iPeriodId As Integer
#End Region

#Region "Public Properties"
    ' Builds the export filename for this interface
    Public Overrides ReadOnly Property Filename() As String
        Get
            If (m_sFilename.Length = 0) Then
                If m_iPeriodId > 0 Then
                    m_sFilename = String.Format("{0}_{1}_{2}.xml", InterfaceName, m_iPeriodId, Now.ToString("yyyyMMddhhmm"))
                Else
                    m_sFilename = String.Format("{0}_{1}_{2}.xml", InterfaceName, m_iBatchID, Now.ToString("yyyyMMddhhmm"))
                End If
            End If
            Return m_sFilename
        End Get
    End Property

    ' Interface name
    Public Overrides ReadOnly Property InterfaceName() As String
        Get
            Return "GL_Export"
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
        OutputLine("SIRIUSEXPORT GL_Export [batchid]")
        OutputLine()
        OutputLine("  batchid     (optional) If specified a previous batch is recreated")
        OutputLine("  periodid    (optional) the ID value of the required Period")
        OutputLine()
    End Sub

    ' Process the command line for this interface
    Public Overrides Sub ProcessCommandLine(ByVal cArgs As Collection(Of String))
        'PLICO 50
        ' process tight structure
        If cArgs.Count > 0 Then
            Dim NoofCommandLineArgs As Integer
            Dim iItem As Integer = 0
            Dim sArgs As String
            Dim sArgsValues() As String
            ' get the number of commandline arguements passed
            NoofCommandLineArgs = cArgs.Count - 1
            ' for each command line arguement passed
            For iItem = 0 To NoofCommandLineArgs

                ' get the argument (should be in the format (argument_name = argument_value)
                sArgs = cArgs(iItem).ToString()

                ' split the argument into argument name / argument value
                sArgsValues = sArgs.Split(CChar("="))
                Try
                    ' determine which argument we are looking at
                    Select Case sArgsValues(0).ToUpper
                        Case "BATCH_ID"
                            ' Try and grab a batch id
                            m_iBatchID = Integer.Parse(sArgsValues(1))
                        Case "PERIOD_ID"
                            m_iPeriodId = Integer.Parse(sArgsValues(1))
                    End Select


                Catch ex As Exception
                    Throw New ArgumentException("Invalid argument" + sArgsValues(0).ToString, ex)
                End Try
            Next
        End If
    End Sub

    ' Process the export
    Public Overrides Sub ProcessExport()

        ' Write status line
        OutputLine("General Ledger Export")
        OutputLine()

        ' Existing batch or new one?

        If m_iBatchID > 0 Then
            OutputLine(String.Format("Recreating batch {0}", m_iBatchID))
        ElseIf m_iPeriodId > 0 And m_iBatchID = 0 Then
            OutputLine(String.Format("Exporting period {0}", m_iPeriodId))
        ElseIf m_iPeriodId = 0 And m_iBatchID = 0 Then
            OutputLine("Creating new batch...")
        End If

        If m_iBatchID = 0 Then
            CreateBatch()
            If m_iPeriodId = 0 Then ' this prompt should appear only when period has not been selected in the screen
                OutputLine(String.Format("Created batch {0}", m_iBatchID))
            End If
        End If

        ' Export the batch 
        ExportBatch()
        UpdateBatchTask(kBatchStatusComplete, m_iBatchID, Filename, 0, 0)
    End Sub
#End Region

#Region "Private Methods"
    Private Sub CreateBatch()
        Dim nReturn As PMEReturnCode = PMEReturnCode.PMTrue

        Try
            ' Add parameters
            AddParameterLite(m_oDatabase, "batch_id", System.DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMInteger, True)
            AddParameterLite(m_oDatabase, "period_id", m_iPeriodId, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, False)
            ' Execute command
            nReturn = m_oDatabase.SQLAction("spu_ACT_GLExport_CreateBatch", "Create Batch", True)
            If nReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to execute 'spu_ACT_GLExport_CreateBatch'")
            End If
            ' Get batch id
            m_iBatchID = m_oDatabase.Parameters.Item("batch_id").Value
        Catch ex As Exception
            Throw New Exception("Unable to create new export batch", ex)
        End Try
    End Sub

    Private Sub ExportBatch()
        Dim iReturn As Long

        'Dim oXML As New XmlDocument  ' MSXML2.DOMDocument = New MSXML2.DOMDocument


        Dim oPI As Xml.XmlProcessingInstruction ' MSXML2.IXMLDOMProcessingInstruction

        'Raise the timeout for the batch with higher no. of rows	
        m_oDatabase.QueryTimeout = 0

        ' Get batch XML
        Output("Retrieving batch...")
        AddParameterLite(m_oDatabase, "batch_id", m_iBatchID, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)
        Dim oDS As Data.DataSet
        iReturn = m_oDatabase.SQLSelectForXML("spu_ACT_GLExport_XML_Select", True, oDS)
        Dim iRowCount As Integer = 0

        If oDS IsNot Nothing Then
            If oDS.Tables(0) IsNot Nothing Then
                If oDS.Tables(0).Rows.Count > 0 Then
                    iRowCount = oDS.Tables(0).Rows.Count
                    Dim sFileName As String = FullPath
                    Dim oFS As IO.FileStream = IO.File.Create(sFileName)
                    oFS.Close()
                    Dim oSW As IO.StreamWriter = IO.File.AppendText(sFileName)
                    For iCnt As Integer = 0 To oDS.Tables(0).Rows.Count - 1

                        oSW.Write(oDS.Tables(0).Rows(iCnt)(0).ToString())

                    Next
                    oSW.Close()
                    oSW = Nothing
                End If
            End If
        End If
        If iRowCount = 0 Then
            OutputLine(String.Format("Batch {0} does not exist", m_iBatchID))
        Else
            OutputLine("Done")
            Output(String.Format("Writing output file...{0}...", Filename))

            ' Create the processing instruction (header) for this file.
            ' oPI = oXML.CreateProcessingInstruction("xml", "version=""1.0""")
            ' oXML.InsertBefore(oPI, oXML.FirstChild)

            ' Tidy up the header wrapper
            'GenerateSchemaHeader(oXML)

            ' Save XML
            'oXML.Save(FullPath)
            OutputLine("Done")
        End If
    End Sub

    Public Overrides Sub CleanUpInterops()

        ' clean up the database interop
'        DestroyCOMInterop(m_oDatabase, True)
		m_oDatabase = Nothing

    End Sub

#End Region

End Class
