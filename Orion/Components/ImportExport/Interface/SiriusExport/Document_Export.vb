'Option Strict On

Imports System.Collections.ObjectModel
Imports System.Xml
Imports System.Xml.Schema
Imports System.Xml.XPath
Imports SharedFiles

Public Class Document_Export : Inherits ExportBase

#Region "Fields"
    Private m_iBatchID As Integer = 0
    Private m_dStartDate As Nullable(Of DateTime)
    Private m_dEndDate As Nullable(Of DateTime)
    Private m_Exportmetadata As Boolean = True

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
            Return "Document_Export"
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
        OutputLine("Example call : - SIRIUSEXPORT Document_Export StartDate=""01/01/2008"" EndDate=""01/01/2009""")
        OutputLine()
        OutputLine("  batch_id    - (optional)  batch id to re-export a batch")
        OutputLine("  start_date  - (optional) optional date filter")
        OutputLine("  end_date    - (optional) optional date filter")
        OutputLine("  EXPORT_METADATA    - (optional)  Default Value True")
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
                            m_iBatchID = Convert.ToInt32(sArgValues(1))

                        Case "START_DATE"
                            m_dStartDate = Convert.ToDateTime(sArgValues(1))

                        Case "END_DATE"
                            m_dEndDate = Convert.ToDateTime(sArgValues(1))
                        Case "EXPORT_METADATA"

                            m_Exportmetadata = Boolean.Parse(sArgValues(1))


                    End Select

                Catch ex As Exception

                    Throw New ArgumentException("Invalid argument " + sArgValues(0).ToString, ex)

                End Try

            Next
        End If
    End Sub

    ' Process the export
    Public Overrides Sub ProcessExport()

        ' Write status line
        OutputLine(String.Format("Document Export start_date={0} end_date={1}", m_dStartDate, m_dEndDate))
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

    End Sub
#End Region

#Region "Private Methods"

    Private Sub CreateBatch()
        Dim iReturn As PMEReturnCode = PMEReturnCode.PMTrue

        Try
            ' Add parameters
            AddParameterLite(m_oDatabase, "batch_id", System.DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMInteger, True)
            AddParameterLite(m_oDatabase, "StartDate", m_dStartDate, PMEParameterDirection.PMParamInput, PMEDataType.PMDate)
            AddParameterLite(m_oDatabase, "EndDate", m_dEndDate, PMEParameterDirection.PMParamInput, PMEDataType.PMDate)
            ' Execute command
            iReturn = m_oDatabase.SQLAction("spu_DOC_DocumentExport_CreateBatch", "Create Batch", True)
            If iReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to execute 'spu_DOC_DocumentExport_CreateBatch'")
            End If

            ' Get batch id
            m_iBatchID = m_oDatabase.Parameters.Item("batch_id").Value

        Catch ex As Exception
            Throw New Exception("Unable to create new document export batch", ex)
        End Try
    End Sub

    Private Sub ExportBatch(ByVal bNewBatch As Boolean)
        Dim m_nodelist As XmlNodeList
        Dim xmlnode As XmlNode
        Dim xmlnode1 As XmlNode
        Dim iReturn As Long
        Dim oXML As New XmlDocument ' MSXML2.DOMDocument = New MSXML2.DOMDocument
        Dim oPI As Xml.XmlProcessingInstruction ' MSXML2.IXMLDOMProcessingInstruction
        Dim oXMLReaderSettings As XmlReaderSettings = New XmlReaderSettings()
        Dim oXMLReader As XmlReader = Nothing
        Dim sFileName As String

        Dim vResult(,) As Object = Nothing
        Dim sSql As String
        ' Validate whether batch no is Document Export Batch No or Not
        sSql = "Select batch_ref from batch where batch_id=" & m_iBatchID
        iReturn = m_oDatabase.SQLSelect(sSql, "name", False, , vResultArray:=vResult)
        If vResult Is String.Empty Then
            OutputLine(String.Format("Invalid Batch ID for Document Export.", m_iBatchID))
            Exit Sub
        End If

        If vResult.ToString() = "" Then
            OutputLine(String.Format("Invalid Batch ID for Document Export.", m_iBatchID))
            Exit Sub
        End If

        If Left(vResult(0, 0), 3) <> "DOC" Then
            OutputLine(String.Format("Invalid Batch ID for Document Export.", m_iBatchID))
            Exit Sub
        End If
        ' Get batch XML
        Output("Retrieving batch...")

        AddParameterLite(m_oDatabase, "start_date", m_dStartDate, PMEParameterDirection.PMParamInput, PMEDataType.PMDate, True)
        AddParameterLite(m_oDatabase, "end_date", m_dEndDate, PMEParameterDirection.PMParamInput, PMEDataType.PMDate)
        AddParameterLite(m_oDatabase, "batch_id", m_iBatchID, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
        'AddParameterLite(m_oDatabase, "status", m_iStatus, PMEParameterDirection.PMParamOutput, PMEDataType.PMLong)
        AddParameterLite(m_oDatabase, "newbatch", Convert.ToInt16(bNewBatch), PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)

        iReturn = m_oDatabase.SQLSelectForXML("spu_DOC_DocumentExport_XML_Select", True, oXML)

        Dim xmlDoc As XmlDocument = New XmlDocument
        xmlDoc.LoadXml(oXML.InnerXml)
        If m_Exportmetadata Then
            Dim namespaceManager As New XmlNamespaceManager(xmlDoc.NameTable)
            namespaceManager.AddNamespace("rss", "http://www.siriusfs.com/SFI/Export/document_Export/20091101")

            m_nodelist = xmlDoc.SelectNodes("/rss:EXPORT_HEADER/rss:document", namespaceManager)
            Dim navigator As XPathNavigator = xmlDoc.CreateNavigator()
            navigator.MoveToRoot()
            navigator.MoveToFirstChild()
            navigator.MoveToFirstChild()

            For Each xmlnode In m_nodelist

                xmlnode1 = xmlnode

                Dim filename = Convert.ToString(xmlnode1.Attributes("file_path").Value)

                If System.IO.File.Exists(filename) And System.IO.Path.GetExtension(filename).ToUpper() = ".XML" Then
                    Dim ResolvedXmlDoc As XmlDocument = New XmlDocument
                    ResolvedXmlDoc.Load(filename)
                    Dim root As XmlElement
                    root = ResolvedXmlDoc.DocumentElement
                    If root.Name = "ResolvedXML" Then
                        navigator.AppendChild(ResolvedXmlDoc.InnerXml)
                    End If
                End If
		navigator.MoveToNext()
            Next
        End If
        ' Check for xml
        If oXML.InnerXml.Length = 0 Then
            OutputLine(String.Format("Batch {0} does not exist", m_iBatchID))
        Else
            OutputLine("Done")
            Output(String.Format("Writing output file...{0}...", Filename))

            ' Create the processing instruction (header) for this file.
            oPI = xmlDoc.CreateProcessingInstruction("xml", "version=""1.0""")
            xmlDoc.InsertBefore(oPI, xmlDoc.FirstChild)

            ' Tidy up the header wrapper
            GenerateSchemaHeader(oXML)

            ' Save XML
            sFileName = FullPath

            xmlDoc.Save(sFileName)
            OutputLine("Done")

            Try

                OutputLine("Validating Exported XML File Format")

                oXMLReaderSettings.Schemas.Add("http://www.siriusfs.com/SFI/Export/document_Export/20091101", System.AppDomain.CurrentDomain.BaseDirectory() & "\" & "Document_Export.xsd")
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
