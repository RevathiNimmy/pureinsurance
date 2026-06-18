Imports System.Collections
Imports System.IO
Imports System.Xml.Xsl
Imports SharedFiles
Imports Sspi.Common.Aws.S3

Module MainModule

#Region "Application Constants"
    Public Const ACApp As String = "SiriusImport"

    ' System option numbers
    Public Const ACExportPathOption As Integer = 5015
    Public Const ACImportPathOption As Integer = 5013
    Public Const ACImportedPathOption As Integer = 5014

    Public Const ACTKeyNameTransDetailID As String = "trans_detail_id"
    Public Const ACTKeyNameTransDetailIDs As String = "trans_detail_ids"
    Public Const ACTKeyNameCashListItemId As String = "cashlistitem_id"
    Public Const kKeyNameWriteOffReasonId As String = "writeoff_reason_id"
    Public Const ACTKeyNameTransactionDate As String = "transaction_date"

    Public Const CloudProcessedFolder As String = "ImportExport/Processed/"
    Public Const CloudImportFolder As String = "ImportExport/Import/"
#End Region

#Region "Fields"
    ' Basic command details
    Private m_bIsHelp As Boolean = False
    Private m_bIsList As Boolean = False
    Private m_sInterface As String = String.Empty
    Private m_sFilename As String = String.Empty
    Private m_cArgs As System.Collections.ObjectModel.Collection(Of String) = Nothing
    Private m_sBDXFolder As String = String.Empty
    Private refreshImportFolder As Boolean = True
    Private cloudHostingOptionEnabled As Boolean
    Private s3Repository As IS3Repository
    Private configuredImportFolderPath As String = String.Empty
    Private configuredImportedFolderPath As String = String.Empty
#End Region

#Region "Properties"
    ''' <summary>
    ''' Configured path of Import folder.
    ''' </summary>
    Public ReadOnly Property ConfiguredImportPath() As String
        Get
            Return configuredImportFolderPath
        End Get
    End Property

    ''' <summary>
    ''' Configured path of Imported (processed) folder.
    ''' </summary>
    Public ReadOnly Property ConfiguredImportedPath() As String
        Get
            Return configuredImportedFolderPath
        End Get
    End Property

    ''' <summary>
    ''' To perform file operations when cloud hosting is enabled.
    ''' </summary>
    Public ReadOnly Property CloudRepository() As IS3Repository
        Get
            Return s3Repository
        End Get
    End Property

    ''' <summary>
    ''' Indicates if cloud hosting is enabled.
    ''' </summary>
    ''' <returns>True if cloud hosting is enabled otherwise False.</returns>
    Public ReadOnly Property CloudHostingEnabled() As Boolean
        Get
            Return cloudHostingOptionEnabled
        End Get
    End Property

#End Region

#Region "Main Method"
    Sub Main()
        Dim oInterface As ImportBase = Nothing

        ' Encapsulate entire app in error loop
        Try

            InitialiseCloudProcessing()

            'Console.ReadLine()

            ' Strip command line
            ProcessCommandLine()

            If CloudHostingEnabled Then
                If refreshImportFolder Then
                    Dim result As Integer = CloudRepository.DownloadFolderAsync(Environment.GetEnvironmentVariable("AWS_IMPORTEXPORT_FOLDER_NAME") & "/" & CloudImportFolder, ConfiguredImportPath).Result
                    If result = gPMConstants.PMEReturnCode.PMTrue Then
                        ' refreshed import folder successfully , no action required 
                    End If
                End If
            End If

            ' List interface switch takes precendence
            If m_bIsList Then
                OutputInterfaceList()
            ElseIf m_bIsHelp Then
                OutputSyntax()
            ElseIf Not m_sFilename.Equals(String.Empty) Then
                If Directory.Exists(m_sFilename) Then
                    ' Process a single directory.
                    ProcessImportDirectory(m_sFilename)
                Else
                    ' Process a single import file
                    ProcessSingleFile()
                End If
            Else
                ' Walk the import folder for import files
                ProcessImportDirectory()
            End If

        Catch ex As Exception
            OutputLine("Sirius Import FAILED" & Environment.NewLine & Environment.NewLine & ex.ToString())

        Finally


        End Try
    End Sub
#End Region

#Region "Methods"
    Public Sub CloseBusiness(ByRef oBusiness As Object)
        Try
            ' Terminate and release reference
            If Not (oBusiness Is Nothing) Then
                oBusiness.Dispose()
            End If
            oBusiness = Nothing
        Catch ex As Exception
            ' Do nothing, for now
        End Try
    End Sub

    Public Function CreateBusiness(ByVal sProgID As String) As Object
        Return CreateBusiness(sProgID, Nothing)
    End Function

    Public Function CreateBusiness(ByVal sProgID As String, ByVal vDatabase As dPMDAO.Database) As Object
        Dim iResult As Integer = 0
        Dim oBusiness As Object = Nothing
        Dim m_lReturn As Integer = 1
        Try
            ' Create the System Options Object
            m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oBusiness, v_sClassName:=sProgID, v_sCallingAppName:=ACApp, v_sUsername:="", v_sPassword:="", v_iUserID:=0, v_iSourceID:=1, v_iLanguageID:=0, v_iCurrencyID:=0, v_iLogLevel:=0)
            If (oBusiness Is Nothing) Then
                Throw New Exception("Unable to create " & sProgID)
            End If

            ' Initialise
            iResult = oBusiness.Initialise( _
                sUsername:="", _
                sPassword:="", _
                iUserID:=1, _
                iSourceID:=1, _
                iLanguageID:=1, _
                iCurrencyID:=26, _
                iLogLevel:=PMELogLevel.PMLogError, _
                sCallingAppName:=ACApp, _
                vDatabase:=vDatabase)
            If iResult <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to initialise " & sProgID)
            End If

            ' Return the object
            Return oBusiness

        Catch ex As Exception
            Throw New Exception("Failed to create " & sProgID, ex)
        End Try

    End Function

    Public Function GetSystemOption(ByVal iOptionNumber As Integer) As String
        Dim iResult As Integer = 0
        Dim oSystemOptions As bSIROptions.Business = Nothing
        Dim sOptionValue As String = String.Empty

        Try
            ' Create the System Options Object
            oSystemOptions = New bSIROptions.Business
            oSystemOptions.Initialise(sUsername:="", sPassword:="", iUserID:=1, iSourceID:=1, iLanguageID:=1, iCurrencyID:=26, iLogLevel:=PMELogLevel.PMLogError, sCallingAppName:=ACApp, vDatabase:=Nothing)

            ' Get the system option
            iResult = oSystemOptions.GetOption( _
                iOptionNumber:=CShort(iOptionNumber), _
                sValue:=sOptionValue, _
                v_iSourceID:=CShort(1))
            If iResult <> PMEReturnCode.PMTrue Then
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

    ''' <summary>
    ''' Sends the supplied error information to the current output stream
    ''' </summary>
    Public Sub OutputError(ByVal ex As Exception)
        Dim nextEx As Exception = ex

        While Not nextEx Is Nothing
            OutputLine("  " & nextEx.Message)
            nextEx = nextEx.InnerException
        End While
    End Sub

    ''' <summary>
    ''' Displays currently supported interface list
    ''' </summary>
    Private Sub OutputInterfaceList()
        OutputLine("SIRIUSIMPORT Interface List")
        OutputLine()
        OutputLine("  PAYMENT_IMPORT     Cash List Payment Import")
        OutputLine("  RECEIPT_IMPORT     Cash List Receipt Import (Manual Import)")
        OutputLine("  REFERENCE_IMPORT   Cheque Reference Import")
        OutputLine("  BANK_RECONCILIATION_IMPORT   Bank Reconciliation Import")
        OutputLine("  COVER_NOTE_IMPORT   Cover Note Import")
        OutputLine("  EXCHANGE_RATES_IMPORT   Exchange Rate Import")
        OutputLine("  INSTALMENT_IMPORT  ")
        OutputLine("  MID_IMPORT  ")
        OutputLine("  MID2_IMPORT  ")
        OutputLine("  CASH_ALLOCATION_IMPORT     Cash List Import")
    End Sub

    ''' <summary>
    ''' Outputs feedback, currently to the console
    ''' </summary>
    Public Sub OutputLine(ByVal message As String)
        ' Write message with carriage return
        Console.WriteLine(message)
    End Sub

    ''' <summary>
    ''' Outputs a blank line
    ''' </summary>
    Public Sub OutputLine()
        ' Write carriage return (or new line)
        Console.WriteLine()
    End Sub

    ''' <summary>
    ''' Displays basic command syntax
    ''' </summary>
    Private Sub OutputSyntax()
        ' Write basic syntax
        OutputLine("Syntax: SIRIUSIMPORT [/?] [/L] [/F:path] [/manual] [username] [password]")
        OutputLine()
        OutputLine("  /?        Displays syntax help")
        OutputLine("  /L        List supported interfaces")
        OutputLine("  /F:path   Only import the file or folder specified by the path")

    End Sub

    ''' <summary>
    ''' Process command line for flags and commands
    ''' </summary>
    Private Sub ProcessCommandLine()
        ' Note: Some of the following code is held over from the SiriusExport 
        ' project, beyond the basic help and list switches it retrieves the 
        ' first non-switch arg as the interface name and any further args
        ' as interface specific. For now this project is a folder parser
        ' so these args are unnecessary but they may be useful in the future
        ' and they have no negative side effects so still process them.

        ' Note: The command line parser is very friendly and will automatically
        ' sort out quotes for us, rather than assuming spaces.
        ' Thus:     /F:"My File.xml"   =    /F:My File.xml

        ' Create interface argument collection
        m_cArgs = New System.Collections.ObjectModel.Collection(Of String)()
        m_sBDXFolder = "Scheduled"
        ' Process args
        For Each sArg As String In My.Application.CommandLineArgs
            ' Process and argument switches
            Select Case sArg.ToUpper.Substring(0, 2)
                Case "/?"
                    m_bIsHelp = True
                Case "/L"
                    m_bIsList = True
                Case "/F"
                    ' Get the filename for this argument
                    m_sFilename = sArg.Substring(3)
                Case Else
                    ' If not a switch, the first is our interface, anything
                    ' else is an interface specific parameter
                    If sArg.ToUpper = "/MANUAL" Then
                        m_sBDXFolder = "Manual"
                    ElseIf sArg.ToUpper = "/NOREFRESH" Then
                        ' Import process is called from UI, when cloudhosting is enabled we have all files available in import folder
                        ' No need to refresh the import folder
                        refreshImportFolder = False
                    ElseIf m_sInterface.Length = 0 Then
                        m_sInterface = sArg.Trim
                        m_cArgs.Add(sArg.Trim)
                    Else
                        m_cArgs.Add(sArg.Trim)
                    End If
            End Select
        Next
    End Sub

    Private Sub ProcessImportDirectory(Optional ByVal sImportPath As String = "")
        Dim oParser As ImportFile
        Dim ImportNamedFolder As Boolean = True
        If (sImportPath.Length = 0) Then
            ' Get import path
            sImportPath = GetSystemOption(ACImportPathOption)
            ImportNamedFolder = False
        End If

        ' Have we got a configured path?
        If (sImportPath.Length = 0) Then
            OutputLine("Import directory has not been configured")
        Else
            ' Check directory exists
            If Directory.Exists(sImportPath) Then
                ' Get directory object
                Dim oDir As New DirectoryInfo(sImportPath)

                If ImportNamedFolder = True Then
                    Call ProcessSubDirectory(oDir, sImportPath)
                Else
                    ' Process any subfolders which may contain XSL Transforms
                    For Each oSubDir As DirectoryInfo In oDir.GetDirectories("*")
                        Call ProcessSubDirectory(oSubDir, sImportPath)
                    Next
                End If

                ' Process all xml files directly in the import folder
                For Each oFile As FileInfo In oDir.GetFiles("*.xml", SearchOption.TopDirectoryOnly)
                    ' Use the import base to find the appropriate import interface
                    oParser = New ImportFile(oFile.FullName, True)
                    If m_sInterface <> String.Empty Then
                        oParser.InterfaceName = m_sInterface
                        oParser.ProcessFile()
                    Else
                        oParser.ProcessFile()
                    End If
                Next

            Else
                OutputLine(String.Format("Import directory '{0}' does not exist", sImportPath))
            End If
        End If
    End Sub

    Private Sub ProcessAllXSLTFilesInFolder(ByVal oDir As DirectoryInfo, ByVal importPath As String, ByVal importedPath As String, ByVal xsltFile As FileInfo)
        ' Process all files directly in the folder
        For Each oFile As FileInfo In oDir.GetFiles("*.*", SearchOption.TopDirectoryOnly)
            ' Ignore the XSLT file
            If oFile.Extension <> ".xslt" Then

                Dim tmpXMLPath As String = oFile.FullName

                ' If it's not an XML file then wrap it in CDATA so that the XSLT can process it.
                If oFile.Extension <> ".xml" Then

                    Dim xmlDoc As New Xml.XmlDocument
                    Dim xmlStr As String = "<?xml version=""1.0"" encoding=""UTF-8""?><root></root>"
                    ' Load the source file into a string
                    Dim fileText As String = IO.File.ReadAllText(oFile.FullName)

                    xmlDoc.LoadXml(xmlStr)

                    'Set the Root element to the content of the other file
                    xmlDoc.Item("root").InnerXml = "<![CDATA[" & fileText & "]]>"

                    ' Store the file as a temp file
                    tmpXMLPath = oFile.DirectoryName & "\" & IO.Path.GetRandomFileName & ".xml"
                    xmlDoc.Save(tmpXMLPath)

                End If

                ' Construct the Reult filename
                Dim resultFilename As String = String.Empty
                resultFilename = importPath & oFile.Name
                resultFilename = resultFilename.Replace(oFile.Extension, ".xml")

                ' If the result file already exists then we don't handle it
                If File.Exists(resultFilename) Then
                    OutputLine(String.Format("The destination file '{0}' already exists and cannot be overwritten.", resultFilename))
                Else
                    ' Transform the source file using the xslt
                    Dim transform As XslCompiledTransform = New XslCompiledTransform()
                    Dim xslSettings As New XsltSettings(False, True)
                    transform.Load(xsltFile.FullName, xslSettings, Nothing)
                    transform.Transform(tmpXMLPath, resultFilename)

                    ' Delete the temp file
                    If oFile.Extension <> ".xml" Then
                        IO.File.Delete(tmpXMLPath)
                    End If

                    ' Move and rename the source file to the processed folder
                    'JP 24 June - trap the error if file can not move due to any previlleges
                    Try
                        Dim targetFileName As String = "(Source)" & oFile.Name
                        Dim processedFilePath As String = importedPath & targetFileName
                        If File.Exists(processedFilePath) Then
                            File.Delete(processedFilePath)
                        End If

                        File.Move(oFile.FullName, processedFilePath)

                        If CloudHostingEnabled Then
                            Dim result As Integer
                            Using fileStream As Stream = New FileStream(processedFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                                Dim fileBytes(0 To fileStream.Length - 1) As Byte
                                fileStream.Read(fileBytes, 0, fileBytes.Length)

                                result = CloudRepository.UploadFile(Environment.GetEnvironmentVariable("AWS_IMPORTEXPORT_FOLDER_NAME") & "/" & CloudProcessedFolder & processedFilePath.Substring(ConfiguredImportedPath.Length).Replace("\", "/"),
                                                                    fileBytes).Result
                            End Using

                            If result = gPMConstants.PMEReturnCode.PMTrue Then
                                Dim s3ImportFile As String = Environment.GetEnvironmentVariable("AWS_IMPORTEXPORT_FOLDER_NAME") & "/" & CloudImportFolder & oFile.FullName.Substring(ConfiguredImportPath.Length).Replace("\", "/")
                                CloudRepository.DeleteFile(s3ImportFile)
                            End If
                        End If
                    Catch ex As Exception
                        OutputLine(ex.Message)
                        Throw New Exception(ex.Message)
                    End Try
                End If

            End If
        Next
    End Sub

    Private Sub ProcessAllXMLFilesInFolder(ByVal oDir As DirectoryInfo, ByVal importPath As String, ByVal importedPath As String, ByVal xmlFile As FileInfo)

        If oDir.FullName.ToUpper.Contains(m_sBDXFolder.ToUpper & "\" & oDir.Name.ToUpper) Then

            Dim xmlDoc As New Xml.XmlDocument

            xmlDoc.Load(xmlFile.FullName)

            If (xmlDoc("IMPORT_HEADER") IsNot Nothing) AndAlso (xmlDoc("IMPORT_HEADER").Attributes("interface_name") IsNot Nothing) Then

                If xmlDoc("IMPORT_HEADER").Attributes("interface_name").Value = "POLICY_BDX_IMPORT" Then

                    ' Process all files directly in the folder
                    For Each oFile As FileInfo In oDir.GetFiles("*.*", SearchOption.TopDirectoryOnly)
                        ' Ignore the XSLT file
                        If (oFile.Extension = ".xls") Or (oFile.Extension = ".xlsx") Then

                            Dim oXML As New XmlDocument(xmlFile.FullName)
                            Dim oParser As New Policy_BDX_Import(oXML)
                            If m_cArgs.Count = 3 Then
                                oParser.UserName = m_cArgs.Item(1)
                            ElseIf m_cArgs.Count = 2 Then
                                oParser.UserName = m_cArgs.Item(0)
                            End If
                            oParser.ProcessImport(xmlFile, oFile)

                        End If
                    Next
                ElseIf xmlDoc("IMPORT_HEADER").Attributes("interface_name").Value = "PREMIUM_BDX_IMPORT" Then

                    ' Process all files directly in the folder
                    For Each oFile As FileInfo In oDir.GetFiles("*.*", SearchOption.TopDirectoryOnly)
                        ' Ignore the XSLT file
                        If (oFile.Extension.ToUpper = ".XLS") Or (oFile.Extension.ToUpper = ".XLSX") Then

                            Dim oXML As New XmlDocument(xmlFile.FullName)
                            Dim oParser As New Premium_BDX_Import(oXML)
                            If m_cArgs.Count = 2 Then
                                oParser.UserName = m_cArgs.Item(0)
                                oParser.Password = m_cArgs.Item(1)
                            End If
                            oParser.ProcessImport(xmlFile, oFile)

                        End If
                    Next
                ElseIf xmlDoc("IMPORT_HEADER").Attributes("interface_name").Value = "CLAIM_BDX_IMPORT" Then

                    ' Process all files directly in the folder
                    For Each oFile As FileInfo In oDir.GetFiles("*.*", SearchOption.TopDirectoryOnly)
                        ' Ignore the XSLT file
                        If (oFile.Extension = ".xls") Or (oFile.Extension = ".xlsx") Then

                            Dim oXML As New XmlDocument(xmlFile.FullName)
                            Dim oParser As New Claim_BDX_Import(oXML)
                            If m_cArgs.Count = 3 Then
                                oParser.UserName = m_cArgs.Item(1)
                                oParser.Password = m_cArgs.Item(2)
                            ElseIf m_cArgs.Count = 2 Then
                                oParser.UserName = m_cArgs.Item(0)
                                oParser.Password = m_cArgs.Item(1)
                            End If
                            oParser.ProcessImport(xmlFile, oFile)

                        End If
                    Next

                End If
            End If
        End If
    End Sub

    Private Sub ProcessSubDirectory(ByVal oDir As DirectoryInfo, ByVal importPath As String)

        Dim importedPath As String = GetSystemOption(ACImportedPathOption)

        importedPath = IIf(importedPath.EndsWith("\"), importedPath, importedPath & "\")
        importPath = IIf(importPath.EndsWith("\"), importPath, importPath & "\")

        ' Recursively call this same procedure to process the sub folders
        For Each subDir As DirectoryInfo In oDir.GetDirectories("*", SearchOption.TopDirectoryOnly)
            Call ProcessSubDirectory(subDir, importPath)
        Next

        ' Check that there is only one xslt file in the sub folder
        If oDir.GetFiles("*.xslt", SearchOption.TopDirectoryOnly).GetUpperBound(0) > 1 Then
            OutputLine(String.Format("There is more than one XSLT file in the folder '{0}'. This is not allowed.", oDir.FullName))
        End If

        ' Process all xslt files directly in the import folder
        For Each xsltFile As FileInfo In oDir.GetFiles("*.xslt", SearchOption.TopDirectoryOnly)

            ProcessAllXSLTFilesInFolder(oDir, importPath, importedPath, xsltFile)

        Next

        ' Process all xslt files directly in the import folder
        For Each xmlFile As FileInfo In oDir.GetFiles("*.xml", SearchOption.TopDirectoryOnly)

            ProcessAllXMLFilesInFolder(oDir, importPath, importedPath, xmlFile)

        Next


    End Sub


    Private Sub ProcessSingleFile()
        Dim oParser As ImportFile
        ' Get import path
        Dim sImportPath As String = GetSystemOption(ACImportPathOption)
        Dim sImportFile As String = Path.Combine(sImportPath, m_sFilename)

        ' Have we got a configured path?
        If (sImportPath.Length = 0) Then
            OutputLine("Import directory has not been configured")
        Else
            ' Check directory exists
            If Not Directory.Exists(sImportPath) Then
                OutputLine(String.Format("Import directory '{0}' does not exist", sImportPath))
            Else
                ' Check file exists
                If Not File.Exists(sImportFile) Then
                    OutputLine(String.Format("Import file '{0}' does not exist", sImportFile))
                Else
                    ' Use the import base to find the appropriate import interface
                    oParser = New ImportFile(sImportFile, False)
                    oParser.ProcessFile()
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' It will read WCFSecurityToken from config file then return encrypted token as string
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SecurityToken() As String
        Dim sSecurityToken As String = String.Empty
        If System.Configuration.ConfigurationManager.AppSettings("WCFSecurityToken") IsNot Nothing Then
            sSecurityToken = System.Configuration.ConfigurationManager.AppSettings("WCFSecurityToken").ToString
        End If

        If sSecurityToken.Length > 0 Then
            Return BCrypt.Net.BCrypt.HashPassword(sSecurityToken, 5)
        Else
            Return String.Empty
        End If
    End Function

    Private Sub InitialiseCloudProcessing()
        Dim cloudHostingOptionValue As String = ""
        bPMFunc.getProductOptionValue(v_sUsername:="", v_sPassword:="", v_iUserID:=0, v_iMainSourceID:=0, v_iLanguageID:=0, v_iCurrencyID:=0, v_iLogLevel:=0, v_sCallingAppName:=ACApp, v_vOptionNumber:=SharedFiles.gPMConstants.SIRHiddenOptions.SIROPTEnableCloudHosting, v_vBranch:=1, r_vUnderwriting:=cloudHostingOptionValue)
        cloudHostingOptionEnabled = (cloudHostingOptionValue = "1")

        configuredImportFolderPath = GetSystemOption(ACImportPathOption)
        configuredImportFolderPath = IIf(configuredImportFolderPath.EndsWith("\"), configuredImportFolderPath, configuredImportFolderPath & "\")
        configuredImportedFolderPath = GetSystemOption(ACImportedPathOption)
        configuredImportedFolderPath = IIf(configuredImportedFolderPath.EndsWith("\"), configuredImportedFolderPath, configuredImportedFolderPath & "\")

        If cloudHostingOptionEnabled Then
            s3Repository = New S3Repository(Environment.GetEnvironmentVariable("AWS_IMPORTEXPORT_BUCKET_NAME"), Environment.GetEnvironmentVariable("AWS_REGION"), "")
        End If
    End Sub

#End Region

#Region "DestroyInteropComObject"
    'Friend Sub DestroyCOMInterop( _
    '                          ByRef oObject As Object, _
    '                      Optional ByVal bIgnoreTerminate As Boolean = False)
    '    If oObject IsNot Nothing Then
    '        ' call terminate on the object before releasing it
    '        If bIgnoreTerminate = False Then
    '            oObject.Terminate()
    '        End If

    '        ' Destroy the object reference

    '        ' System.Runtime.InteropServices.Marshal.ReleaseComObject(oObject)

    '        oObject = Nothing
    '    End If
    'End Sub
#End Region

End Module
