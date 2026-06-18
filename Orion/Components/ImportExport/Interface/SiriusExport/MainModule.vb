Imports System.Collections
Imports System.IO
Imports Sspi.Common.Aws.S3

Module MainModule

#Region "Application Constants"
    Public Const ACApp As String = "SiriusExport"

    ' System option numbers
    Public Const ACExportPathOption As Integer = 5015
    Public Const ACImportPathOption As Integer = 5013
    Public Const ACImportedPathOption As Integer = 5014
    Public Const ACSMSMessagePathOption As Integer = 5056
#End Region

#Region "Constants"
    Private Const CloudExportFolder As String = "ImportExport/Exported/"
#End Region

#Region "Fields"
    ' Basic command details
    Private m_bIsHelp As Boolean = False
    Private m_bIsList As Boolean = False
    Private m_sInterface As String = String.Empty
    Private m_cArgs As System.Collections.ObjectModel.Collection(Of String) = Nothing
#End Region

#Region "Main Method"
    Sub Main()
        Dim oInterface As ExportBase = Nothing
        'Debugger.Break()
        ' Encapsulate entire app in error loop
        Try
            ' Strip command line
            ProcessCommandLine()

            ' List interface switch takes precendence
            If m_bIsList Then
                OutputInterfaceList()
            ElseIf m_bIsHelp And (m_sInterface.Length = 0) Then
                OutputSyntax()
            Else
                ' We need to resolve the interface now
                Select Case m_sInterface.ToUpper()
                    Case "GL_EXPORT"
                        oInterface = New GL_Export()
                    Case "INSTALMENT_EXPORT"
                        oInterface = New Instalment_Export()
                    Case "CLAIMS_EXPORT"
                        oInterface = New Claims_Export()
                    Case "PAYMENT_EXPORT"
                        oInterface = New Payment_Export()
                    Case "INSTALMENT_PLAN_EXPORT"
                        oInterface = New Instalment_Plan_Export()
                    Case "POLICY_EXPORT"
                        oInterface = New Policy_Export()
                    Case "MESSAGE_EXPORT"
                        oInterface = New Message_Export
                        'Start (Girija chokkalingam) - (Tech Spec - PGR022 - Financial Interfaces.doc) - (5..5.1.1)
                    Case "RECEIPT_EXPORT"
                        oInterface = New Receipt_Export()
                        'Start (Girija chokkalingam) - (Tech Spec - PGR022 - Financial Interfaces.doc) - (5.5.1.1)
                    Case "DOCUMENT_EXPORT"
                        oInterface = New Document_Export()
                        ' Start (KNaseem) - (Tech Spec - 8.6 Premium Claims Analysis.doc)
                    Case "POLICY_BATCH_EXPORT"
                        oInterface = New Policy_Batch_Export()
                        'WPR14-MID
                    Case "MID_EXPORT"
                        oInterface = New MID_Export()
                        'END WPR14-MID
                    Case "MID2_EXPORT"
                        oInterface = New MID2_Export()
                    Case "MID_BATCH_EXPORT"
                        oInterface = New MID_Batch_Export()
                    Case "COMMISSION_EXPORT"
                        oInterface = New Commission_Export()
                    Case Else
                        OutputInterfaceList()
                        Exit Sub
                End Select

                ' If no interface output warning and interface list
                If oInterface Is Nothing Then
                    OutputLine("Interface not supported" & Environment.NewLine)
                    OutputInterfaceList()
                Else
                    'Should we display interface specific syntax?
                    If m_bIsHelp Then
                        oInterface.DisplayHelp()
                    Else
                        ' Process interface parameters
                        oInterface.ProcessCommandLine(m_cArgs)

                        ' Process export
                        oInterface.ProcessExport()

                        ' Call the Transform Method
                        oInterface.ProcessTransform(m_cArgs)

                        ' close database connection
                        oInterface.CloseDBConnection()

                        ' clean up any interops used by the export process
                        oInterface.CleanUpInterops()

                        If IO.File.Exists(oInterface.FullPath) Then
                            FormatXMLFile(oInterface.FullPath)
                        End If

                        If oInterface.CloudHostingEnabled Then
                            Dim s3FileName As String = Environment.GetEnvironmentVariable("AWS_IMPORTEXPORT_FOLDER_NAME") & "/" & CloudExportFolder & oInterface.Filename
                            Dim repository As IS3Repository = New S3Repository(Environment.GetEnvironmentVariable("AWS_IMPORTEXPORT_BUCKET_NAME"),
                                                                               Environment.GetEnvironmentVariable("AWS_REGION"), "")
                            Using fileStream As Stream = New FileStream(oInterface.FullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                                Dim fileBytes(0 To fileStream.Length - 1) As Byte
                                fileStream.Read(fileBytes, 0, fileBytes.Length)

                                repository.UploadFile(s3FileName, fileBytes)
                            End Using


                            If Not String.IsNullOrEmpty(oInterface.TransformedFile) Then
                                Dim s3TransformedFileName As String = Environment.GetEnvironmentVariable("AWS_IMPORTEXPORT_FOLDER_NAME") & "/" & CloudExportFolder & oInterface.TransformedFile.Substring(oInterface.TransformedFile.LastIndexOf("\") + 1)
                                Using fileStream As Stream = New FileStream(oInterface.TransformedFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                                    Dim fileBytes(0 To fileStream.Length - 1) As Byte
                                    fileStream.Read(fileBytes, 0, fileBytes.Length)

                                    repository.UploadFile(s3TransformedFileName, fileBytes)
                                End Using

                            End If
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            oInterface.UpdateBatchTask(oInterface.kBatchStatusFailed, oInterface.BatchId, String.Empty, 0, 0)
            OutputLine("Sirius Export FAILED" & Environment.NewLine & Environment.NewLine & ex.ToString())
        End Try
    End Sub
#End Region

#Region "Public Methods"
    Public Function GetSystemOption(ByVal iOptionNumber As Integer) As String
        Dim lResult As Integer = 0
        Dim oSystemOptions As bSIROptions.Business = Nothing
        Dim sOptionValue As String = String.Empty

        Try

            ' Create the System Options Object
            oSystemOptions = New bSIROptions.Business
            If (oSystemOptions Is Nothing) Then
                Throw New Exception("Unable to create bSIROptions.Business")
            End If

            ' Initialise
            lResult = oSystemOptions.Initialise( _
                sUsername:="", _
                sPassword:="", _
                iUserID:=0, _
                iSourceID:=1, _
                iLanguageID:=1, _
                iCurrencyID:=26, _
                iLogLevel:=PMELogLevel.PMLogError, _
                sCallingAppName:=ACApp)
            If lResult <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to initialise bSIROptions.Business")
            End If

            ' Get the system option
            lResult = oSystemOptions.GetOption( _
                iOptionNumber:=CShort(iOptionNumber), _
                sValue:=sOptionValue, _
                v_iSourceID:=CShort(1))
            If lResult <> PMEReturnCode.PMTrue Then
                Throw New Exception(String.Format("Unable to retrieve system option '{0}'", iOptionNumber))
            End If

            ' Return the option value
            Return sOptionValue

        Catch ex As Exception
            Throw New Exception("Unable to retrieve system option", ex)

        Finally
            If Not oSystemOptions Is Nothing Then
                oSystemOptions.Dispose()
            End If
            oSystemOptions = Nothing
        End Try
    End Function

    Public Sub GetProductOption( _
    ByVal m_oDatabase As dPMDAO.Database, _
    ByVal productOption As Integer, _
    ByVal branchId As Integer, _
    ByRef optionValue As String)

        If m_oDatabase IsNot Nothing Then

            AddParameterLite(m_oDatabase, "option_number", productOption, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)
            AddParameterLite(m_oDatabase, "branch_id", branchId, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(m_oDatabase, "option_value", optionValue, PMEParameterDirection.PMParamOutput, PMEDataType.PMString)

            m_oDatabase.SQLSelect("spu_SAM_Get_Product_option", "spu_SAM_Get_Product_option", True)

            optionValue = m_oDatabase.Parameters.Item("option_value").Value.ToString()

        End If

    End Sub

    ' Outputs feedback, currently to the console
    Public Sub OutputLine(ByVal message As String)
        ' Write message with carriage return
        Console.WriteLine(message)
    End Sub

    Public Sub OutputLine()
        ' Write carriage return (or new line)
        Console.WriteLine()
    End Sub

    Public Sub Output(ByVal message As String)
        ' Write message without carriage return
        Console.Write(message)
    End Sub


    ''' <summary>
    ''' It will set QueryTimeOut from config file
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function QueryTimeOut() As Integer
        Dim nQueryTimeOut As Integer = 0
        If System.Configuration.ConfigurationManager.AppSettings("QueryTimeOut") IsNot Nothing Then
            If Not String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings("QueryTimeOut").ToString.Trim) Then
                nQueryTimeOut = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings("QueryTimeOut").ToString)
            End If
        End If

        Return nQueryTimeOut

    End Function
#End Region

#Region "Private Methods"

    'Method to indent all export files fully and to remove the "1 line" xml currently exported
    Private Sub FormatXMLFile(ByVal FileName As String)
        Dim xDoc As Xml.XmlDocument
        Dim xmlTWriter As Xml.XmlTextWriter
        Try
            xDoc = New Xml.XmlDocument()
            xDoc.Load(FileName)

            xmlTWriter = New Xml.XmlTextWriter(FileName, System.Text.Encoding.Unicode)
            xmlTWriter.Formatting = Xml.Formatting.Indented

            xDoc.Save(xmlTWriter)

        Catch ex As Exception

        Finally
            xDoc = Nothing
        End Try

    End Sub

    ' Displays basic command syntax
    Private Sub OutputSyntax()
        ' Write basic syntax
        OutputLine("Syntax: SIRIUSEXPORT [/?] [/L] interface [parameters]")
        OutputLine()
        OutputLine("  /?              Displays syntax help")
        OutputLine("  /L              List supported interfaces")
        OutputLine("  interface       Specifies the export interface to invoke")
        OutputLine("  parameters      Specifies any interface specific parameters")
        OutputLine("  XSLT_FILE_NAME  (Optional) Specifies the filename of an XSLT file to apply after the XML is exported")
        OutputLine("  DEST_FILE_EXTN  (Optional) Specifies the extension to be applied filename of the output file from the XSLT")
        OutputLine("  DEST_FOLDER     (Optional) Specifies an alternative output folder for the XSLT output file")
        OutputLine()
        OutputLine("For help on a specific interface use:")
        OutputLine("  SIRIUSEXPORT /? interface")
    End Sub

    ' Displays currently supported interface list
    Private Sub OutputInterfaceList()
        OutputLine("SIRIUSEXPORT Interface List")
        OutputLine()
        OutputLine("  GL_EXPORT                 General Ledger Export")
        OutputLine("  INSTALMENT_EXPORT         Instalment Export")
        OutputLine("  CLAIMS_EXPORT             Claims Export")
        OutputLine("  PAYMENT_EXPORT            Payments Export")
        OutputLine("  INSTALMENT_PLAN_EXPORT    Instalment Plan Export")
        OutputLine("  POLICY_EXPORT             Live Client, Policy and Risk Export")
        OutputLine("  MESSAGE_EXPORT            SMS MESSAGE Export")
        OutputLine("  RECEIPT_EXPORT            Receipt Export")
        OutputLine("  DOCUMENT_EXPORT           Document Export")
        OutputLine("  POLICY_BATCH_EXPORT       Policy Batch Export")
        OutputLine("  MID_Export                MID 1 Database Export")
        OutputLine("  MID2_Export               MID 2 Database Export")
        OutputLine("  COMMISSION_EXPORT         Commission Export")
    End Sub

    ' Process command line for flags and commands
    Private Sub ProcessCommandLine()

        ' Check for parameters
        If My.Application.CommandLineArgs.Count = 0 Then
            ' No parameters so default to plain help
            m_bIsHelp = True
        End If

        ' Create interface argument collection
        m_cArgs = New System.Collections.ObjectModel.Collection(Of String)()

        ' Process args
        For Each sArg As String In My.Application.CommandLineArgs
            ' Process and argument switches
            Select Case sArg.ToUpper
                Case "/?"
                    m_bIsHelp = True
                Case "/L"
                    m_bIsList = True
                Case Else
                    ' If not a switch, the first is our interface, anything
                    ' else is an interface specific parameter
                    If m_sInterface.Length = 0 Then
                        m_sInterface = sArg.Trim
                    Else
                        m_cArgs.Add(sArg.Trim)
                    End If
            End Select
        Next
    End Sub

#End Region

#Region "DestroyInteropComObject"
    'Friend Sub DestroyCOMInterop( _
    '                               ByRef oObject As Object, _
    '                       Optional ByVal bIgnoreTerminate As Boolean = False)

    '    Dim iRet As Int32

    '    ' call terminate on the object before releasing it
    '    If bIgnoreTerminate = False Then
    '        oObject.Dispose()
    '    End If
    '    ' Destroy the object reference

    '    'If (oObject.GetType.IsCOMObject) Then
    '    '    System.Runtime.InteropServices.Marshal.ReleaseComObject(oObject)
    '    'End If
    '    oObject = Nothing
    'End Sub
#End Region

End Module


