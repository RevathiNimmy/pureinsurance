Imports System.Collections.ObjectModel
Imports System.Xml
Imports System.Xml.Schema
Imports SharedFiles

Public NotInheritable Class Instalment_Plan_Export : Inherits ExportBase 

#Region "Fields"

    Private m_iBatchID As Integer = 0
    Private m_sPFSchemeTypeCode As String = ""
    Private m_sBusinessType As String = ""
    Private m_iNewBatch As Byte = 0
    Private m_sExportPath As String = String.Empty
    Private m_nTotalRecords As Integer
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
            Return "Instalment_Plan_Export"
        End Get
    End Property

    Public Overrides Property BatchId As Integer
        Get
            Return m_iBatchID
        End Get
        Set(ByVal value As Integer)
            m_iBatchID = value
        End Set
    End Property
    Public Property TotalRecords() As Integer
        Get
            Return m_nTotalRecords
        End Get
        Set(ByVal value As Integer)
            m_nTotalRecords = value
        End Set
    End Property
#End Region

#Region "Public Methods"
    ' Display help for this interface
    Public Overrides Sub DisplayHelp()

        ' Write syntax help for this command
        'OutputLine("SIRIUSEXPORT Instalment_Plan_Export [batchid]")
        OutputLine("Example call : - SIRIUSEXPORT Instalment_Plan_Export Batch_ID =1 scheme_type_code=""TP""")
        OutputLine()
        OutputLine("Batch_ID     :If specified a previous batch is recreated")
        OutputLine("              If no batchid is specified a new batch is created")
        OutputLine()
        OutputLine("Scheme_Type_Code   - (optional) filter on the premium finance schemes type")
        OutputLine()
        OutputLine("Business_Type   - (optional) filter on business type type -- NB,MTA,MTC or REN")
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
                    Case "SCHEME_TYPE_CODE"
                        m_sPFSchemeTypeCode = sArgValues(1)
                    Case "BUSINESS_TYPE"
                        m_sBusinessType = sArgValues(1)

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
    Private Sub ProcessBatchID(ByVal v_bCreateBatchID As Boolean)
        ' Existing batch or new one?
        If m_iBatchID = 0 Or v_bCreateBatchID Then
            Output("Creating new batch...")
            CreateBatch()
            OutputLine(String.Format("Created batch {0}", m_iBatchID))
        End If
    End Sub
    ' Process the export
    Public Overrides Sub ProcessExport()
        Dim bCreateBatchID As Boolean
        ' Write status line
        OutputLine("Instalment Plan Export")
        OutputLine()

        If m_iBatchID > 0 Then
            bCreateBatchID = False
            OutputLine(String.Format("Recreating batch {0}", m_iBatchID))
        Else
            bCreateBatchID = True
        End If

        If m_sPFSchemeTypeCode = "TP" Then
            If m_sBusinessType = "" Or m_sBusinessType = "NB" Then
                ProcessBatchID(bCreateBatchID)
                ExportBatch(v_sTransactionType:="NB")
            End If
            If m_sBusinessType = "" Or m_sBusinessType = "MTA" Then
                ProcessBatchID(bCreateBatchID)
                ExportBatch(v_sTransactionType:="MTA")
            End If
            If m_sBusinessType = "" Or m_sBusinessType = "MTC" Then
                ProcessBatchID(bCreateBatchID)
                ExportBatch(v_sTransactionType:="MTC")
            End If
            If m_sBusinessType = "" Or m_sBusinessType = "REN" Then
                ProcessBatchID(bCreateBatchID)
                ExportBatch(v_sTransactionType:="REN")
            End If
        Else
            ProcessBatchID(bCreateBatchID)
            ExportBatch()
        End If

        Dim nReturn As Integer = PMEReturnCode.PMTrue
        nReturn = GetXMLFileInfo(Filename, TotalRecords)
        If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oBusiness.GetFileSummary", "Unable to get record count for policy export")
        End If
        UpdateBatchTask(kBatchStatusComplete, m_iBatchID, Filename, TotalRecords, 0)
    End Sub
#End Region

#Region "Private Methods"
    'Method used to create the batch if no batch id is supplied(m_batchID <=0)
    Private Sub CreateBatch()
        Dim nReturn As PMEReturnCode = PMEReturnCode.PMTrue

        Try
            ' Add parameters
            AddParameterLite(m_oDatabase, "batch_id", System.DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMInteger, True)
            AddParameterLite(m_oDatabase, "pfscheme_type_code", m_sPFSchemeTypeCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            ' Execute command
            nReturn = m_oDatabase.SQLAction("spu_ACT_InstalmentPlanExport_CreateBatch", "Create Batch", True)
            If nReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to execute 'spu_ACT_InstalmentPlanExport_CreateBatch'")
            End If

            ' Get batch id
            m_iBatchID = m_oDatabase.Parameters.Item("batch_id").Value
            m_iNewBatch = 1
        Catch ex As Exception
            Throw New Exception("Unable to create new export batch", ex)
        End Try
    End Sub

    Public Sub ExportBatch(Optional ByVal v_sTransactionType As String = "")

        'Method used to create an XML file in a format defined by Instalment_Plan_Export.XSD
        Dim iReturn As Long
        Dim oXML As New XmlDocument ' MSXML2.DOMDocument = New MSXML2.DOMDocument 
        Dim oPI As Xml.XmlProcessingInstruction ' MSXML2.IXMLDOMProcessingInstruction 
        Dim sFileName As String
        Dim vResult(,) As Object = Nothing
        Dim sSql As String
        Dim oXMLReaderSettings As XmlReaderSettings = New XmlReaderSettings()
        Dim oXMLReader As XmlReader = Nothing

        ' Validate whether batch no is Instalment Plan Export Batch No or Not
        sSql = "Select batch_ref from batch where batch_id=" & m_iBatchID
        iReturn = m_oDatabase.SQLSelect(sSql, "name", False, , vResultArray:=vResult)
        If vResult Is String.Empty Then
            OutputLine(String.Format("Invalid Batch ID for Instalment Plan Export.", m_iBatchID))
            Exit Sub
        End If

        If vResult.ToString() = "" Then
            OutputLine(String.Format("Invalid Batch ID for Instalment Plan Export.", m_iBatchID))
            Exit Sub
        End If

        If Left(vResult(0, 0), 3) <> "IPX" Then
            OutputLine(String.Format("Invalid Batch ID for Instalment Plan Export.", m_iBatchID))
            Exit Sub
        End If

        ' Add the parameters required for the SP's execution
        Output("Retrieving batch...")

        AddParameterLite(m_oDatabase, "batch_id", m_iBatchID, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)
        AddParameterLite(m_oDatabase, "pfscheme_type_code", m_sPFSchemeTypeCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        AddParameterLite(m_oDatabase, "new_batch", m_iNewBatch, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
        If m_sPFSchemeTypeCode = "TP" Then
            AddParameterLite(m_oDatabase, "transactiontype", v_sTransactionType, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        End If

        ' Execute the SP which will return the XML String and store it in DOMDocument
        iReturn = m_oDatabase.SQLSelectForXML("spu_ACT_InstalmentPlanExport_XML_Select", True, oXML)

        ' Save XML
        If (m_sPFSchemeTypeCode = "TP") Then
            m_sFilename = String.Format("{0}_{1}_{2}_{3}.xml", InterfaceName, m_iBatchID, v_sTransactionType, Now.ToString("yyyyMMddhhmm"))
        End If

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

            sFileName = FullPath
            oXML.Save(sFileName)
            OutputLine("Done")

            'Block Coded to validate the XML file against the XSD file
            Try

                OutputLine("Validating Exported XML File Format")

                oXMLReaderSettings.Schemas.Add("http://www.siriusfs.com/SFI/Export/instalment_plan_Export/20070816", System.AppDomain.CurrentDomain.BaseDirectory() & "\" & "Instalment_Plan_Export.xsd")
                oXMLReaderSettings.ValidationType = ValidationType.Schema
                oXMLReader = XmlReader.Create(sFileName, oXMLReaderSettings)

                While oXMLReader.Read()
                End While
                oXMLReader.Close()

                OutputLine("Validation Completed")

            Catch ex As XmlException

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
    ''' <summary>
    ''' Gets file information for an import or export file
    ''' </summary>
    ''' <param name="sFilename"></param>
    ''' <param name="dtDate"></param>
    ''' <param name="sInterface"></param>
    ''' <param name="sReference"></param>
    ''' <param name="sRecords"></param>
    ''' <param name="sParameters_Used"></param>
    ''' <returns></returns>
    Public Function GetXMLFileInfo(ByVal sFilename As String, ByRef sRecords As Integer) As Integer

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim oXML As XmlDocument
        Dim oXMLReader As XmlReader = Nothing
        Dim oXMLReader1 As XmlReader = Nothing
        Dim oNode As XmlNode
        Dim sMainKey As String = ""
        Dim bOutOfmemoryError As Boolean = False

        sRecords = 0
        ' Open as xml
        oXML = New XmlDocument()
        Dim temp_xml_result As Boolean

        Try
            m_sExportPath = GetSystemOption(ACExportPathOption)
            oXML.Load(m_sExportPath + "\" + sFilename)
            temp_xml_result = True

        Catch parseError As System.Exception
            Try
                oXMLReader = XmlReader.Create(sFilename)
                temp_xml_result = True
                bOutOfmemoryError = True
            Catch ex As System.Exception
                temp_xml_result = False
            End Try
        End Try
        If temp_xml_result Then
            If oXML.HasChildNodes Then
                For Each oNode In oXML.ChildNodes
                    Select Case oNode.Name
                        Case "EXPORT_HEADER"
                            sRecords = oNode.ChildNodes.Count
                    End Select
                Next oNode

            ElseIf Not IsNothing(oXMLReader) And oXMLReader.Value <> String.Empty Then

                While oXMLReader.Read()
                    Select Case oXMLReader.Name
                        Case "EXPORT_HEADER"
                            ' Get record count
                            Dim i As Integer = 0
                            Dim bSkipMainKey As Boolean = False
                            Dim SError As String = ""
                            Try
                                While oXMLReader.Read()
                                    If oXMLReader.Name <> "" AndAlso bSkipMainKey = False Then
                                        sMainKey = oXMLReader.Name
                                        bSkipMainKey = True
                                    End If
                                    If oXMLReader.NodeType = XmlNodeType.Element And oXMLReader.Name = sMainKey Then
                                        i += 1
                                    End If
                                End While
                            Catch ex As Exception
                                If bOutOfmemoryError Then
                                    'Do Nothing
                                Else
                                    gPMFunctions.RaiseError("GetXMLFileInfo", ex.Message)
                                End If

                            Finally
                                sRecords = i
                            End Try
                            Exit While

                    End Select
                End While
            End If
        Else
            gPMFunctions.RaiseError("oXML.Load()", "Unable to load xml stream")
        End If

        oXML = Nothing

        Return nResult

    End Function
    ''' <summary>
    ''' Get an attribute value
    ''' </summary>
    ''' <param name="oNode"></param>
    ''' <param name="sAttribute"></param>
    ''' <returns></returns>
    Public Function TryGetAttribute(ByVal oNode As XmlElement, ByVal sAttribute As String) As String
        Try
            Return CStr(oNode.GetAttribute(sAttribute))
        Catch
            Return String.Empty
        End Try
    End Function
    ''' <summary>
    ''' Get Path
    ''' </summary>
    ''' <returns></returns>
    'Private Function GetPaths() As Integer
    '    Dim nReturn As Integer = gPMConstants.PMEReturnCode.PMTrue
    '    nReturn = CType(iPMFunc.GetSystemOption(ACExportPathOption, m_sExportPath), gPMConstants.PMEReturnCode)
    '    If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '        gPMFunctions.RaiseError("GetSystemOption", "Unable to get system option for export path")
    '    End If
    '    Return nReturn
    'End Function
#End Region

End Class
