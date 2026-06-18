Imports System.Collections.ObjectModel
Imports System.Xml
Imports System.Xml.Schema
Imports SharedFiles
Imports System.IO
Imports System.Xml.Xsl
Imports System.Xml.Serialization
Imports System.Text
Imports System.Xml.XPath

Public NotInheritable Class MID_Export : Inherits ExportBase 

#Region "Fields"

    Private m_nBatchID As Integer = 0
    Private m_sExportType As String
    Private m_nNewBatch As Integer
    Private m_nRuleID As Integer = 0
    Private m_nBranchID As Integer = 0
    Private m_sRuleFileName As String
    Private m_sXsltFilename As String = String.Empty
    Private m_sDestFileExtn As String = String.Empty
    Private m_sDestFolder As String = String.Empty
    Private m_sSupplierId As String = "0"
    Private m_sInsurer_id As String = "0"    
    Private m_bTestIndicator As Boolean
    Private m_sSiteNumber As String = String.Empty

#End Region

#Region "Public Properties"
    ' Builds the export filename for this interface
    Public Overrides ReadOnly Property Filename() As String
        Get
            If String.IsNullOrEmpty(m_sRuleFileName) AndAlso m_nRuleID = 0 Then
                Dim aoFileName(,) As Object = Nothing
                GetFileNameForReExport(aoFileName)
                If aoFileName IsNot Nothing Then
                    m_nRuleID = Convert.ToInt32(aoFileName(0, aoFileName.GetLowerBound(1)).ToString())
                    m_sRuleFileName = aoFileName(1, aoFileName.GetLowerBound(1)).ToString()
                    If Not String.IsNullOrEmpty(m_sRuleFileName) Then
                        m_sRuleFileName = m_sRuleFileName & ".xml"
                        m_sFilename = m_sRuleFileName
                    End If
                End If
            End If
            If (m_sFilename.Length = 0) Then
                m_sFilename = String.Format("{0}.{1}.{2}.{3}.xml", IIf(m_bTestIndicator = True, "UKMI.UAT", "UKMI.PRD"), "INS" & m_sInsurer_id, "SITE" & m_sSiteNumber, "INPUT")
            End If
            Return m_sFilename
        End Get
    End Property

    ' Interface name
    Public Overrides ReadOnly Property InterfaceName() As String
        Get
            Return "MID_Export"
        End Get
    End Property
    Public Overrides Property BatchId() As Integer
        Get
            Return m_nBatchID
        End Get
        Set(ByVal value As Integer)
            m_nBatchID = value
        End Set
    End Property
#End Region

#Region "Public Methods"
    ' Display help for this interface
    Public Overrides Sub DisplayHelp()
        OutputLine("Example call : - SIRIUSEXPORT MID_Export")
        OutputLine()
        OutputLine("  batch_id    - (optional)  batch id to re-export a batch")
        OutputLine()
    End Sub

    ' Process the command line arguments supplied for this interface
    ' This Procedure is used to segregate the arguments supplied from commandLine
    Public Overrides Sub ProcessCommandLine(ByVal cArgs As Collection(Of String))
        ' process tight structure
        'If cArgs.Count >= 0 Then
        ' Debugger.Break()
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
                        m_nBatchID = CInt(sArgValues(1))

                    Case "EXPORT_TYPE"
                        m_sExportType = sArgValues(1)

                    Case "XSLT_FILE_NAME"
                        m_sXsltFilename = CStr(sArgValues(1))

                    Case "DEST_FILE_EXTN"
                        m_sDestFileExtn = CStr(sArgValues(1))

                    Case "DEST_FOLDER"
                        m_sDestFolder = CStr(sArgValues(1))

                End Select

            Catch ex As Exception

                Throw New ArgumentException("Invalid argument " + sArgValues(0).ToString, ex)
            End Try
        Next
    End Sub

    ' Process the export
    Public Overrides Sub ProcessExport()

        Dim nResult As PMEReturnCode = PMEReturnCode.PMTrue
        Dim aoRules(,) As Object = Nothing

        ' Write status line
        OutputLine("MID Export")
        OutputLine()

        If m_nNewBatch = 0 And m_nBatchID <> 0 Then
            ExportBatch()
            UpdateBatchTask(kBatchStatusComplete, m_nBatchID, Filename, 0, 0)

            ProcessTransform()
            'reset batchid for new batch creation
            m_nBatchID = 0
        Else
            nResult = GetMIDActiveRulesForExport(aoRules)
            If nResult <> PMEReturnCode.PMTrue OrElse aoRules Is Nothing OrElse aoRules Is String.Empty Then
                OutputLine(String.Format("No active MID1 rule exists for any of the branches", m_nBatchID))
                Exit Sub
            End If
            For iParam As Integer = aoRules.GetLowerBound(1) To aoRules.GetUpperBound(1)
                Output(String.Format("Process MID1 Export for branch '{0}' ...", aoRules(1, iParam).ToString()))
                m_nRuleID = Convert.ToInt32(aoRules(0, iParam).ToString())
                m_nBranchID = Convert.ToInt32(aoRules(1, iParam).ToString())
                m_sRuleFileName = aoRules(2, iParam).ToString()
                If Not String.IsNullOrEmpty(m_sRuleFileName) Then
                    m_sRuleFileName = m_sRuleFileName & ".xml"
                    m_sFilename = m_sRuleFileName
                End If
                m_sSupplierId = CastTo3Characters(aoRules(3, iParam).ToString())
                m_sInsurer_id = CastTo3Characters(aoRules(4, iParam).ToString())
                m_sSiteNumber = CastTo3Characters(aoRules(5, iParam).ToString())
                m_bTestIndicator = aoRules(6, iParam).ToString()
                ' Existing batch or new one?
                If m_nBatchID > 0 Then
                    OutputLine(String.Format("Recreating batch {0}", m_nBatchID))
                Else
                    Output("Creating new batch...")
                    CreateBatch()
                    OutputLine(String.Format("Created batch {0}", m_nBatchID))
                End If

                ' Export the batchs 
                ExportBatch()
                UpdateBatchTask(kBatchStatusComplete, m_nBatchID, Filename, 0, 0)

                ProcessTransform()
                'reset batchid for new batch creation
                m_nBatchID = 0
            Next iParam
        End If

    End Sub
#End Region

#Region "Private Methods"
    'Method used to create the batch if no batch id is supplied(m_batchID <=0)
    Private Sub CreateBatch()
        Dim nReturn As PMEReturnCode = PMEReturnCode.PMTrue
        Try
            ' Add parameters
            AddParameterLite(m_oDatabase, "batch_id", System.DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMInteger, True)
            AddParameterLite(m_oDatabase, "branch_id", m_nBranchID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, False)

            ' Execute command
            nReturn = m_oDatabase.SQLAction("spu_MIDExport_CreateBatch", "Create Batch", True)
            If nReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to execute 'spu_MIDExport_CreateBatch'")
            End If

            ' Get batch id
            m_nBatchID = m_oDatabase.Parameters.Item("batch_id").Value
            m_nNewBatch = 1
        Catch ex As Exception
            Throw New Exception("Unable to create new export batch", ex)
        End Try
    End Sub

    Public Sub ExportBatch()

        'Method used to create an XML file in a format defined by MID_Export.XSD
        Dim nReturn As PMEReturnCode = PMEReturnCode.PMTrue
        Dim oXML As Xml.XmlDocument = New Xml.XmlDocument
        Dim oPI As Xml.XmlProcessingInstruction
        Dim sFileName As String
        Dim oXMLReaderSettings As XmlReaderSettings
        Dim oXMLReader As XmlReader = Nothing

        ' Validate whether batch no is MID Export Batch No or Not
        nReturn = ValidateBatchForMIDExport()
        If nReturn <> PMEReturnCode.PMTrue Then
            OutputLine(String.Format("Invalid Batch ID for MID Export.", m_nBatchID))
            Exit Sub
        End If

        'Setting file name to blank for it to create new filename for eachs
        'm_sFilename = String.Empty

        ' Add the parameters required for the SP's execution
        m_oDatabase.Parameters.Clear()
        AddParameterLite(m_oDatabase, "batch_id", m_nBatchID, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)
        AddParameterLite(m_oDatabase, "rule_id", m_nRuleID, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, False)
        AddParameterLite(m_oDatabase, "branch_id", m_nBranchID, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, False)
        AddParameterLite(m_oDatabase, "new_batch", Convert.ToInt16(m_nNewBatch), PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, False)

        nReturn = m_oDatabase.SQLSelectForXML("spu_MID_Export_XML_Select", True, oXML)

        'Check for xml
        If oXML.InnerXml.Length = 0 Then
            OutputLine(String.Format("Batch {0} does not exist", m_nBatchID))
        Else
            OutputLine("Done")
            Output(String.Format("Writing output file...{0}...", Filename))

            'Create the processing instruction (header) for this file.
            oPI = oXML.CreateProcessingInstruction("xml", "version=""1.0""")
            oXML.InsertBefore(oPI, oXML.FirstChild)

            ' Save XML
            sFileName = FullPath
            oXML.Save(sFileName)
            OutputLine("Done")

            'Block Coded to validate the XML file against the XSD file
            Try

                OutputLine("Validating Exported XML File Format")
                oXMLReaderSettings = New XmlReaderSettings()
                oXMLReaderSettings.Schemas.Add("http://www.siriusfs.com/SFI/Export/MID_Export/20060420", System.AppDomain.CurrentDomain.BaseDirectory() & "\" & "MID_Export.xsd")
                oXMLReaderSettings.ValidationType = ValidationType.Schema
                oXMLReader = XmlReader.Create(sFileName, oXMLReaderSettings)

                While oXMLReader.Read()
                    Select Case oXMLReader.NodeType

                    End Select
                End While

                oXMLReader.Close()

                OutputLine("Validation Completed")

            Catch ex As XmlException
                UpdateBatchTask(kBatchStatusFailed, m_nBatchID, Filename, 0, 0)
                OutputLine("Invalid XML File Format")
                Throw New ApplicationException("Export file has an invalid XML File Format", ex)

            Finally

                oXMLReader = Nothing
                oXMLReaderSettings = Nothing

            End Try

        End If
    End Sub

    Private Function ValidateBatchForMIDExport()
        Dim nResult As PMEReturnCode = PMEReturnCode.PMTrue
        Dim aoResult(,) As Object = Nothing

        ' Add parameters
        AddParameterLite(m_oDatabase, "batch_id", m_nBatchID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, True)
        AddParameterLite(m_oDatabase, "mid_type", "MID1", PMEParameterDirection.PMParamInput, PMEDataType.PMString, False)

        ' Execute command
        nResult = m_oDatabase.SQLSelect("spu_MID_ConfirmBatchForExport", "Confirm Batch", True, vResultArray:=aoResult)
        If nResult <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to execute 'spu_MID_ConfirmBatchForExport'")
        End If

        If aoResult Is Nothing OrElse aoResult Is String.Empty Then
            nResult = PMEReturnCode.PMFalse
        End If

        Return nResult

    End Function

    Private Function GetMIDActiveRulesForExport(ByRef r_aoResultArray(,) As Object)

        Dim nResult As PMEReturnCode = PMEReturnCode.PMTrue

        ' Add parameters
        AddParameterLite(m_oDatabase, "mid_type", "MID1", PMEParameterDirection.PMParamInput, PMEDataType.PMString, True)

        ' Execute command
        nResult = m_oDatabase.SQLSelect("spu_MID_GetActiveRulesForExport", "Confirm Batch", True, vResultArray:=r_aoResultArray)
        If nResult <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to execute 'spu_MID_GetActiveRulesForExport'")
        End If

        Return nResult

    End Function

    Private Function GetFileNameForReExport(ByRef r_aoResultArray(,) As Object)
        Dim nResult As PMEReturnCode = PMEReturnCode.PMTrue

        ' Add parameters
        AddParameterLite(m_oDatabase, "batch_id", m_nBatchID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, True)
        AddParameterLite(m_oDatabase, "mid_type", "MID1", PMEParameterDirection.PMParamInput, PMEDataType.PMString, False)

        ' Execute command
        nResult = m_oDatabase.SQLSelect("spu_MID_GetFileNameForReExport", "Confirm Batch", True, vResultArray:=r_aoResultArray)
        If nResult <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to execute 'spu_MID_GetFileNameForReExport'")
        End If

        Return nResult

    End Function

    Public Overrides Sub CleanUpInterops()

        ' clean up the database interop
        m_oDatabase = Nothing

    End Sub

    Public Overloads Sub ProcessTransform()

        Try
            ' Check the XSLT file exists
            If File.Exists(m_sXsltFilename) = False Then
                Throw New ArgumentException("XSLT file does not exist - " & m_sXsltFilename, "batchid")
            End If

            OutputLine("XSL transform beginning")

            ' Construct the sResultFile path from the source filename, dest folder (if provided) and dest extension.  
            Dim sResultFilename As String

            If CloudHostingEnabled Then
                sResultFilename = FullPath
            Else
                If m_sDestFolder.Length > 0 Then
                    m_sDestFolder = IIf(m_sDestFolder.EndsWith("\"), m_sDestFolder, m_sDestFolder & "\").ToString
                    If Directory.Exists(m_sDestFolder) = False Then
                        Throw New ArgumentException("Destination Folder does not exist - " & m_sDestFolder, "batchid")
                    End If
                    sResultFilename = m_sDestFolder & Filename
                Else
                    sResultFilename = FullPath
                End If
            End If

            ' Change the Extension
            If m_sDestFileExtn.Length > 0 Then
                sResultFilename = sResultFilename.Replace(".xml", "." & m_sDestFileExtn)
            End If

            ' Check the source and destination files are not the same
            If FullPath = sResultFilename Then
                sResultFilename = sResultFilename.Replace(".xml", "(XSLT Output).xml")
            End If

            Dim oTransform As XslCompiledTransform = New XslCompiledTransform()
            Dim oXslSettings As New XsltSettings(False, True)
            oTransform.Load(m_sXsltFilename, oXslSettings, Nothing)
            'transform.OutputSettings. XsltSettings.EnableScript
            Dim oXmlwriter As New XmlTextWriter(sResultFilename, System.Text.Encoding.Default)
            oTransform.Transform(FullPath, oXmlwriter)
            oXmlwriter.Close()

            TransformedFile = sResultFilename

            OutputLine("XSL transform complete")

        Catch ex As ArgumentException
            Throw
        Catch ex As XmlException
            OutputLine("Invalid XML File Format")
            Throw New ApplicationException("Export file has an invalid XML File Format", ex)
        Catch ex As Exception
            Throw New ApplicationException("Export file could not be transformed using the transform file - " & m_sXsltFilename, ex)
        End Try

    End Sub

    Private Function CastTo3Characters(ByVal v_sInputString As String) As String
        Dim sReturnStr As String = String.Empty
        If (v_sInputString.Length < 3) Then
            For index As Integer = 1 To (3 - v_sInputString.Length)
                sReturnStr = sReturnStr + "0"
            Next
        End If
        sReturnStr = sReturnStr + v_sInputString
        Return sReturnStr
    End Function

#End Region

End Class
