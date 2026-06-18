Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports System.Text
Imports System.Xml
'Developer Guide no.129
Imports SharedFiles
Imports System.Reflection

<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Business
    ' ***************************************************************** '

    ' ************************************************
    ' Added to replace global variables 22/12/2003
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    Private cloudHostingOptionEnabled As Boolean

    ' ************************************************

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' File system objectt
    Private m_oFS As Object

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As gPMConstants.PMEProcessMode
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_lPMAuthorityLevel As Integer


    ' ***************************************************************** '
    '                        PUBLIC PROPERTIES
    ' ***************************************************************** '
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = Value
        End Set
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


    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this object.
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel


            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Create file system object
            m_oFS = New Object()

            Dim cloudHostingOptionValue As String = ""
            SharedFiles.bPMFunc.getProductOptionValue(v_sUsername:="", v_sPassword:="", v_iUserID:=0, v_iMainSourceID:=0, v_iLanguageID:=0, v_iCurrencyID:=0, v_iLogLevel:=0, v_sCallingAppName:=ACApp, v_vOptionNumber:=SharedFiles.gPMConstants.SIRHiddenOptions.SIROPTEnableCloudHosting, v_vBranch:=1, r_vUnderwriting:=cloudHostingOptionValue)
            cloudHostingOptionEnabled = (cloudHostingOptionValue = "1")

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this object.
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                m_oFS = Nothing
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()

                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.

            If Not Information.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Information.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Information.IsNothing(vProcessMode) Then

                m_lProcessMode = CType(CInt(vProcessMode), gPMConstants.PMEProcessMode)
            End If


            If Not Information.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Scans the given folder and returns information about xml files
    ' ***************************************************************** '
    Public Function GetFileSummary(ByVal sPath As String, ByRef vResults As Object) As Integer

        Dim result As Integer = 0
        Dim oFolder As DirectoryInfo

        Dim dtDate As Date
        Dim sInterface, sReference, sRecords As String
        Dim lRow As Integer

        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "GetFileSummary"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Ensure results are empty

            vResults = Nothing

            If Directory.Exists(sPath) Then
                ' Get folder
                oFolder = New DirectoryInfo(sPath)

                ' Get all files in folder
                For Each oFile As FileInfo In oFolder.GetFiles
                    ' Only process XML
                    If Path.GetExtension(oFile.Name).Substring(1).ToUpper() = "XML" Then
                        ' Increase array size
                        If Information.IsArray(vResults) Then

                            ReDim Preserve vResults(MainModule.ACImportExportFileInfoEnum.ACIEMax, vResults.GetUpperBound(1) + 1)
                        Else
                            ReDim vResults(MainModule.ACImportExportFileInfoEnum.ACIEMax, 0)
                        End If

                        lRow = vResults.GetUpperBound(1)

                        ' Store filename and date

                        vResults(MainModule.ACImportExportFileInfoEnum.ACIEFilename, lRow) = oFile.Name

                        vResults(MainModule.ACImportExportFileInfoEnum.ACIEDate, lRow) = oFile.LastWriteTime
                        dtDate = oFile.LastWriteTime

                        ' Get rest of file information
                        lReturn = CType(GetXMLFileInfo(oFile.FullName, dtDate, sInterface, sReference, sRecords), gPMConstants.PMEReturnCode)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' If we failed still add the file set invalid state in record count
                            sRecords = "<Invalid>"
                        End If

                        ' Save results

                        vResults(MainModule.ACImportExportFileInfoEnum.ACIEInterface, lRow) = sInterface

                        vResults(MainModule.ACImportExportFileInfoEnum.ACIEReference, lRow) = sReference

                        vResults(MainModule.ACImportExportFileInfoEnum.ACIERecords, lRow) = sRecords
                    End If
                Next oFile
            Else

                vResults = "Warning: Path not configured"
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Gets file information for an import or export file
    ' ***************************************************************** '
    Private Function GetXMLFileInfo(ByVal v_sFilename As String, ByRef r_dtDate As Date, ByRef r_sInterface As String, ByRef r_sReference As String, ByRef r_sRecords As String) As Integer

        Dim result As Integer = 0
        Dim oXML As XmlDocument
        Dim oXMLReader As XmlReader = Nothing
        Dim oXMLReader1 As XmlReader = Nothing
        Dim oNode As XmlNode
        Dim sMainKey As String = ""
        Dim bOutOfmemoryError As Boolean = False
        Const kMethodName As String = "GetXMLFileInfo"

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set default responses
        r_sInterface = "<Unknown>"
        r_sReference = ""
        r_sRecords = "<Invalid>"

        ' Open as xml
        oXML = New XmlDocument()
        Dim temp_xml_result As Boolean

        Try
            oXML.Load(v_sFilename)
            temp_xml_result = True

        Catch parseError As System.Exception
            Try
                oXMLReader = XmlReader.Create(v_sFilename)
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
                        Case "IMPORT_HEADER"
                            ' Get header info
                            r_sInterface = TryGetAttribute(oNode, "interface_name", "<Unknown>")
                            r_sReference = TryGetAttribute(oNode, "batch_reference", "")
                            r_dtDate = gPMFunctions.ToSafeDate(TryGetAttribute(oNode, "date_imported", CStr(0)))
                            ' Get record count
                            r_sRecords = CStr(oNode.ChildNodes.Count)

                        Case "EXPORT_HEADER"
                            ' Get header info
                            r_sInterface = gPMFunctions.ToSafeString(TryGetAttribute(oNode, "interface_name", "<Unknown>"), "<Unknown>")
                            r_sReference = gPMFunctions.ToSafeString(TryGetAttribute(oNode, "batch_reference", ""), "None")
                            r_dtDate = gPMFunctions.ToSafeDate(TryGetAttribute(oNode, "date_exported", CStr(0)))
                            ' Get record count
                            If oNode.ChildNodes.Count > 0 Then
                                'Instalment export has a different 3-level structure
                                For iCnt As Integer = 0 To oNode.ChildNodes.Count - 1
                                    If oNode.ChildNodes(iCnt).Name = "SCHEME_HEADER" Then
                                        r_sRecords = oNode.ChildNodes(iCnt).ChildNodes.Count
                                    Else
                                        r_sRecords = oNode.ChildNodes.Count
                                    End If
                                Next iCnt
                            Else
                                r_sRecords = 0
                            End If
                        Case Else
                            If oXML.DocumentElement IsNot Nothing AndAlso oXML.DocumentElement.Name = "n:PremiumStatementLoad" Then
                                r_sInterface = "IMPORT"
                                r_sRecords = oXML.SelectNodes("//PremiumReconciliationRs").Count
                                r_dtDate = gPMFunctions.ToSafeDate(oXML.DocumentElement.SelectSingleNode("DateGenerated").Value)

                                Dim sSplitFileName As String() = Path.GetFileNameWithoutExtension(v_sFilename).Split("_")

                                For Each str As String In sSplitFileName
                                    If Not String.IsNullOrEmpty(str) AndAlso str.Trim.ToUpper.StartsWith("ARI") Then
                                        r_sReference = str
                                        Exit For
                                    End If
                                Next
                            End If
                    End Select
                Next oNode

            ElseIf Not IsNothing(oXMLReader) And oXMLReader.Value <> String.Empty Then

                While oXMLReader.Read()
                    Select Case oXMLReader.Name
                        Case "IMPORT_HEADER"
                            ' Get header info
                            r_sInterface = oXMLReader.GetAttribute("interface_name") 'TryGetAttribute(oNode, "interface_name", "<Unknown>")
                            r_sReference = oXMLReader.GetAttribute("batch_reference") 'TryGetAttribute(oNode, "batch_reference", "")
                            r_dtDate = gPMFunctions.ToSafeDate(oXMLReader.GetAttribute("date_imported")) 'gPMFunctions.ToSafeDate(TryGetAttribute(oNode, "date_imported", CStr(0)))

                            ' Get record count
                            While oXMLReader.Read()
                                If oXMLReader.Name <> "" Then
                                    sMainKey = oXMLReader.Name
                                    Exit While
                                End If
                            End While

                            Dim i As Integer = 0
                            oXMLReader1 = XmlReader.Create(v_sFilename)
                            While oXMLReader1.Read()
                                If oXMLReader1.NodeType = XmlNodeType.Element And oXMLReader1.Name = sMainKey Then
                                    i += 1
                                End If
                            End While
                            r_sRecords = CStr(i)
                            Exit While

                        Case "EXPORT_HEADER"
                            ' Get header info
                            r_sInterface = oXMLReader.GetAttribute("interface_name") ' gPMFunctions.ToSafeString(TryGetAttribute(oNode, "interface_name", "<Unknown>"), "<Unknown>")
                            r_sReference = oXMLReader.GetAttribute("batch_reference") 'gPMFunctions.ToSafeString(TryGetAttribute(oNode, "batch_reference", ""), "None")
                            r_dtDate = gPMFunctions.ToSafeDate(oXMLReader.GetAttribute("date_exported")) 'gPMFunctions.ToSafeDate(TryGetAttribute(oNode, "date_exported", CStr(0)))

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
                                r_sRecords = CStr(i)
                            End Try
                            Exit While

                    End Select
                End While
            End If
        Else
            gPMFunctions.RaiseError("oXML.Load()", "Unable to load xml stream")
        End If

        GoTo Finally_Renamed
Catch_Renamed:
        ' DO Not Call any functions before here or the error will be lost
        bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)

        ' This is our invalid marker so ensure it is set
        r_sRecords = "<Invalid>"

Finally_Renamed:
        ' Release object
        oXML = Nothing

        Return result


    End Function

    ' ***************************************************************** '
    ' Gets an array of named fields from the specified file
    ' ***************************************************************** '
    Public Function GetRecordPreview(ByVal v_sPath As String, ByVal v_sFilename As String, ByRef r_vHeader() As Object, ByRef r_vDetail(,) As Object) As Integer

        Dim result As Integer = 0
        Dim oXML As XmlDocument
        Dim oNode As XmlNode
        Dim sInterface As String = ""
        Dim oPreview As PreviewBase

        Const kMethodName As String = "GetRecordPreview"
        result = gPMConstants.PMEReturnCode.PMTrue

        ' Open as xml
        oXML = New XmlDocument()
        Dim temp_xml_result As Boolean

        Try
            oXML.Load(Path.Combine(v_sPath, v_sFilename))
            temp_xml_result = True

        Catch parseError As System.Exception
            temp_xml_result = False
        End Try
        If temp_xml_result Then
            ' Find interface type
            For Each oNode2 As XmlNode In oXML.ChildNodes
                oNode = oNode2
                If oNode.Name = "IMPORT_HEADER" Then
                    ' Get interface name and exit loop retaining current node

                    sInterface = TryGetAttribute(oNode, "interface_name", "<Unknown>")
                    Exit For
                End If
            Next oNode2

            ' Check for supported interfaces
            Select Case sInterface
                Case "<Unknown>"
                    ' Interface not found
                    result = gPMConstants.PMEReturnCode.PMNotFound
                    GoTo Finally_Renamed

                Case "RECEIPT_IMPORT"
                    oPreview = New ReceiptImport()

                Case Else
                    ' Interface is not supported
                    result = gPMConstants.PMEReturnCode.PMFalse
                    GoTo Finally_Renamed
            End Select

            ' Return interface preview
            If Not (oPreview Is Nothing) Then
                result = oPreview.GetPreview(oNode, r_vHeader, r_vDetail)
            End If
        Else
            gPMFunctions.RaiseError("oXML.Load()", "Unable to load xml stream")
        End If

        GoTo Finally_Renamed
Catch_Renamed:
        ' DO Not Call any functions before here or the error will be lost
        bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)

Finally_Renamed:
        ' Release object

        Return result

    End Function

    ' ***************************************************************** '
    ' Triggers the import application to manually import the specified file
    ' ***************************************************************** '
    Public Function ProcessManualImport(ByVal v_sFilename As String) As Integer

        Dim result As Integer = 0
        Dim sCommand As String = ""
        Dim sPurePath As String = ""

        Dim lReturn As Integer
        Const kMethodName As String = "ProcessManualImport"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If sPurePath = "" Then
                gPMFunctions.GetPMRegSetting(gPMConstants.HKEY_LOCAL_MACHINE, 0, gPMConstants.PMERegSettingLevel.pmeRSLBase, "PMDIR", sPurePath)
                If sPurePath.Contains("\") Then
                    sPurePath &= "Pure\Application\"
                Else
                    sPurePath &= "\Pure\Application\"
                End If
            End If

            ' If there is no file specified the process all files
            If v_sFilename = "" Then
                sCommand = sPurePath & "SIRIUSIMPORT.EXE"
                ' Check for spaces in filename
            ElseIf v_sFilename.IndexOf(" "c) >= 0 Then
                sCommand = sPurePath & "SIRIUSIMPORT.EXE RECEIPT_IMPORT /F:""" & v_sFilename & """"
            Else
                sCommand = sPurePath & "SIRIUSIMPORT.EXE RECEIPT_IMPORT /F:" & v_sFilename
            End If

            If CloudHostingEnabled Then
                sCommand &= " /NoRefresh"
            End If

            ' Fire off the import and wait for completion
            ShellWait(sCommand)

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Triggers the import application to manually export the specified file
    ' ***************************************************************** '
    Public Function ProcessManualExport(ByVal v_iInterface As Integer, ByVal v_lBatchID As Integer, ByVal v_vParamArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sCommand As String = ""
        Dim sPurePath As String = ""
        Dim sInterface As String = ""
        Dim sCommandLine As New StringBuilder


        Const kMethodName As String = "ProcessManualExport"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If sPurePath = "" Then
                gPMFunctions.GetPMRegSetting(gPMConstants.HKEY_LOCAL_MACHINE, 0, gPMConstants.PMERegSettingLevel.pmeRSLBase, "PMDIR", sPurePath)
                sPurePath &= "\Pure\Application\"
            End If

            Dim lLbnd, lUbnd As Integer

            If Information.IsArray(v_vParamArray) Then

                lLbnd = v_vParamArray.GetLowerBound(1)
                lUbnd = v_vParamArray.GetUpperBound(1)

                sCommandLine = New StringBuilder("")

                For lCnt As Integer = lLbnd To lUbnd
                    If Not Object.Equals(v_vParamArray(0, lCnt), Nothing) And Not Object.Equals(v_vParamArray(1, lCnt), Nothing) Then
                        sCommandLine.Append(CStr(v_vParamArray(0, lCnt)).TrimEnd() & "=""" & CStr(v_vParamArray(1, lCnt)).TrimEnd() & """ ")
                    End If
                Next lCnt
            End If

            Select Case v_iInterface
                Case gPMConstants.PMEExportInterface.pmeIEIGLExport
                    sInterface = "GL_EXPORT"
                    If sCommandLine.ToString = "" Then
                        sCommandLine = New StringBuilder(IIf(v_lBatchID = 0, "", CStr(v_lBatchID)).ToString.Trim.ToUpper())
                    End If
                Case gPMConstants.PMEExportInterface.pmeIEIInstalmentExport
                    sInterface = "INSTALMENT_EXPORT"
                Case gPMConstants.PMEExportInterface.pmeIEIClaimExport
                    sInterface = "CLAIMS_EXPORT"
                Case gPMConstants.PMEExportInterface.pmeIEIReceiptExport
                    sInterface = "RECEIPT_EXPORT"
                    If sCommandLine.ToString = "" Then
                        sCommandLine = New StringBuilder(IIf(v_lBatchID = 0, "", CStr(v_lBatchID)).ToString.Trim.ToUpper())
                    End If
                Case gPMConstants.PMEExportInterface.pmeIEIPaymentExport
                    sInterface = "PAYMENT_EXPORT"
                Case gPMConstants.PMEExportInterface.pmeIEIInstalmentPlanExport
                    sInterface = "INSTALMENT_PLAN_EXPORT"
                Case gPMConstants.PMEExportInterface.pmeIEIPolicyExport
                    sInterface = "POLICY_EXPORT"
                Case gPMConstants.PMEExportInterface.pmeIEIMessageExport
                    sInterface = "MESSAGE_EXPORT"
                Case gPMConstants.PMEExportInterface.pmeIEIDocumentExport
                    sInterface = "DOCUMENT_EXPORT"
                Case gPMConstants.PMEExportInterface.pmeIEIPolicyBatchExport
                    sInterface = "POLICY_BATCH_EXPORT"
                    'WPR14-MID
                Case gPMConstants.PMEExportInterface.pmeIEIMIDExport
                    sInterface = "MID_EXPORT"
                    'END WPR14-MID
                Case gPMConstants.PMEExportInterface.pmeIEIMID2Export
                    sInterface = "MID2_EXPORT"
                Case gPMConstants.PMEExportInterface.pmeIEICommissionExport
                    sInterface = "COMMISSION_EXPORT"
            End Select

            sCommand = sPurePath & "SIRIUSEXPORT.EXE " & sInterface & " " & sCommandLine.ToString()

            ' Fire off the import and wait for completion
            'ShellWait(sCommand)
            Dim noProcess As Process() = Process.GetProcessesByName("SIRIUSEXPORT")
            If noProcess.Length > 0 Then
                MessageBox.Show("There is already a Sirius Export instance running.", "Information")
            Else
                Dim pHelp As New ProcessStartInfo
                pHelp.FileName = sPurePath & "SIRIUSEXPORT.EXE"
                pHelp.Arguments = sInterface & " " & sCommandLine.ToString()
                pHelp.UseShellExecute = True
                pHelp.WindowStyle = ProcessWindowStyle.Minimized
                Dim proc As Process = Process.Start(pHelp)
            End If

            Return result
        Catch ex As System.Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Updates the source XML with data modified in the review
    ' ***************************************************************** '

    Public Function UpdateRecordPreview(ByVal v_sPath As String, ByVal v_sFilename As String, ByVal v_vHeader() As Object, ByVal v_vDetail(,) As Object) As Integer


        Dim result As Integer = 0
        Dim oXML As XmlDocument
        Dim oNode As XmlNode
        Dim sInterface As String = ""
        Dim oPreview As PreviewBase

        Const kMethodName As String = "UpdateRecordPreview"


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Open as xml
        oXML = New XmlDocument()
        Dim temp_xml_result As Boolean

        Try
            oXML.Load(Path.Combine(v_sPath, v_sFilename))
            temp_xml_result = True

        Catch parseError As System.Exception
            temp_xml_result = False
        End Try
        If temp_xml_result Then
            ' Find interface type
            For Each oNode2 As XmlNode In oXML.ChildNodes
                oNode = oNode2
                If oNode.Name = "IMPORT_HEADER" Then
                    ' Get interface name and exit loop retaining current node

                    sInterface = TryGetAttribute(oNode, "interface_name", "<Unknown>")
                    Exit For
                End If
            Next oNode2

            ' Check for supported interfaces
            Select Case sInterface
                Case "<Unknown>"
                    ' Interface not found
                    result = gPMConstants.PMEReturnCode.PMNotFound
                    GoTo Finally_Renamed

                Case "RECEIPT_IMPORT"
                    oPreview = New ReceiptImport()

                Case Else
                    ' Interface is not supported
                    result = gPMConstants.PMEReturnCode.PMFalse
                    GoTo Finally_Renamed
            End Select

            ' Pass update to appropriate interface processor
            If Not (oPreview Is Nothing) Then
                result = oPreview.Update(oNode, v_vHeader, v_vDetail)
            End If

            ' Save data back
            oXML.Save(Path.Combine(v_sPath, v_sFilename))
        Else
            gPMFunctions.RaiseError("oXML.Load()", "Unable to load xml stream")
        End If

        GoTo Finally_Renamed
Catch_Renamed:
        ' DO Not Call any functions before here or the error will be lost
        bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)

Finally_Renamed:
        ' Release object

        Return result
    End Function

    Public Function GetAllPeriods(ByRef r_vPeriodArray(,) As Object) As Integer

        Try

            r_vPeriodArray = Nothing
            With m_oDatabase

                .Parameters.Clear()
                m_lReturn = .SQLSelect(sSQL:=ACSelectPeriodSQL, sSQLName:=ACSelectPeriodName, bStoredProcedure:=ACSelectPeriodStored, vResultArray:=r_vPeriodArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(r_vPeriodArray) Then
                    r_vPeriodArray = Nothing
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSuspenseDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSuspenseDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return gPMConstants.PMEReturnCode.PMError
        End Try
    End Function

    Public Function GetPeriods(ByRef r_vPeriod(,) As Object) As Integer

        Try

            r_vPeriod = Nothing
            With m_oDatabase

                .Parameters.Clear()
                m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=CStr(m_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllDetailsSQL, sSQLName:=ACGetAllDetailsName, bStoredProcedure:=ACGetAllDetailsStored, vResultArray:=r_vPeriod)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCurrentPeriods Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCurrentPeriods", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return gPMConstants.PMEReturnCode.PMError
        End Try
    End Function

    Public Function GetCurrentPeriods(ByVal r_vPeriod As String, ByRef r_voCurrentPeriod(,) As Object) As Integer

        Try

            With m_oDatabase

                .Parameters.Clear()

                ' Add the PeriodID parameter (INPUT)

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Period_id", vValue:=CStr(r_vPeriod), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetDetailsSQL, sSQLName:=ACGetDetailsName, bStoredProcedure:=ACGetDetailsStored, vResultArray:=r_voCurrentPeriod)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCurrentPeriods Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCurrentPeriods", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return gPMConstants.PMEReturnCode.PMError
        End Try
    End Function

    Public Function GetBranchDetails(ByRef branchDetails(,) As Object) As Integer
        Try
            branchDetails = Nothing
            With m_oDatabase

                .Parameters.Clear()
                m_lReturn = m_oDatabase.Parameters.Add(sName:="Source_id", vValue:=CStr(m_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetBranchDetailsSQL, sSQLName:=ACGetBranchDetailsName, bStoredProcedure:=ACGetBranchDetailsStored, vResultArray:=branchDetails)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCurrentPeriods Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCurrentPeriods", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return gPMConstants.PMEReturnCode.PMError

        End Try

    End Function

    ' ***************************************************************** '
    '                           CLASS EVENTS
    ' ***************************************************************** '
    Public Sub New()
        MyBase.New()
        Exit Sub
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class

