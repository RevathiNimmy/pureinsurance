Option Strict Off
Option Explicit On
Imports System.IO
'developer guide no. 129
Imports System.Reflection
Imports SharedQuoteEngine
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable

    ' ***************************************************************** '
    ' Class Name: Communcation
    '
    ' Author: Steve Watton
    '
    ' Date: 11/10/2002
    '
    ' Description: Creatable class which is used to validate
    ' different media types for various locales
    '
    ' Edit History:
    ' ***************************************************************** '

    ' ************************************************
    ' Added to replace global variables 14/01/2004
    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    Private Const ACGetLocaleFromCountryIDName As String = "GetLocaleFromCountryID"

    'Developer Guide No 39
    Private Const ACGetLocaleFromCountryIDSQL As String = "spu_ACT_Sel_Locale"

    Private Const ACGetValidationCodeForMediaTypeName As String = "GetValCodeForMediaTypeID"
    'Developer Guide No 39
    Private Const ACGetValidationCodeForMediaTypeSQL As String = "spu_ACT_Sel_Media_Val_Code"

    Private Const ACGetMediaTypeName As String = "GetValCodeForMediaTypeID"
    'Developer Guide No 39
    Private Const ACGetMediaTypeSQL As String = "spu_ACT_Select_MediaType"

    Private Const ACGetMediaTypeIdForCodeName As String = "Get MediaTypeId From Code"
    Private Const ACGetMediaTypeIdForCodeSQL As String = "spu_Get_MediaType_Id_For_Code"

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database
    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean
    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode
    ' Process Mode Properties
    ' Task
    Private m_iTask As Integer
    ' Navigate
    Private m_lNavigate As Integer
    ' Process Mode
    Private m_lProcessMode As gPMConstants.PMEProcessMode
    ' Type of Business
    Private m_sTransactionType As String = ""
    ' Effective
    Private m_dtEffectiveDate As Date
    ' PM Lookup Business Component (Private)
    Private m_oLookup As BPMLOOKUP.Business
    Private m_bServerDebuggingEnabled As Boolean
    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFOrion
        End Get
    End Property
    Public ReadOnly Property Task() As Integer
        Get
            Return m_iTask
        End Get
    End Property

    Public ReadOnly Property Navigate() As Integer
        Get

            Return m_lNavigate

        End Get
    End Property

    Public ReadOnly Property ProcessMode() As Integer
        Get

            Return m_lProcessMode

        End Get
    End Property

    Public ReadOnly Property TransactionType() As String
        Get

            Return m_sTransactionType

        End Get
    End Property

    Public ReadOnly Property EffectiveDate() As Date
        Get

            Return m_dtEffectiveDate

        End Get
    End Property

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long

        Dim result As Integer = 0
        Dim oUserAuthority As bACTUserAuthorities.Business
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

            ' Get Reference to Database

            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the ProcessMode to Generic
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric

            ' Set the Type Of Business to New Business
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric

            ' Set the Effective Date to NOW
            m_dtEffectiveDate = DateTime.Now

            m_bServerDebuggingEnabled = False

            Dim sValue As String = ""

            bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iUserID:=m_iUserID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=5084, r_sOptionValue:=sValue)

            Dim vParams As Object = Nothing
            Const ACUserServerScriptsRunInDebug As Integer = 14
            If sValue = "1" Then


                oUserAuthority = New bACTUserAuthorities.Business
                m_lReturn = oUserAuthority.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise bACTUserAuthorities.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = oUserAuthority.GetDetails(vUserID:=m_iUserID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise bACTUserAuthorities.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = oUserAuthority.GetNext(vParams:=vParams)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the details for the User Authority", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If gPMFunctions.ToSafeInteger(vParams(ACUserServerScriptsRunInDebug), 0) = 1 Then
                    m_bServerDebuggingEnabled = True
                End If

                oUserAuthority.Dispose()
                oUserAuthority = Nothing
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
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
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.

            If Not Informations.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Informations.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Informations.IsNothing(vProcessMode) Then

                m_lProcessMode = CType(CInt(vProcessMode), gPMConstants.PMEProcessMode)
            End If


            If Not Informations.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Informations.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (BeginTrans) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function BeginTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLBeginTrans()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (CommitTrans) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CommitTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLCommitTrans()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (RollbackTrans) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function RollbackTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLRollbackTrans()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    Public Sub New()
        MyBase.New()

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        ' Class Initialise
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error.
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    ''' <summary>
    '''  Validates the Credit Card or Bank Account Number
    ''' </summary>
    ''' <param name="v_lMediaID"></param>
    ''' <param name="v_lCountryID"></param>
    ''' <param name="v_sNumber"></param>
    ''' <param name="r_bValid"></param>
    ''' <param name="r_sBankName"></param>
    ''' <param name="r_sAddress1"></param>
    ''' <param name="r_sAddress2"></param>
    ''' <param name="r_sAddress3"></param>
    ''' <param name="r_sAddress4"></param>
    ''' <param name="r_sPostalCode"></param>
    ''' <param name="r_vValidationMessage"></param>
    ''' <param name="r_bValidationOverridable"></param>
    ''' <param name="sMediaCode"></param>
    ''' <param name="sBIC"></param>
    ''' <param name="sIBAN"></param>
    ''' <remarks></remarks>
    Public Sub ValidateNumber(ByVal v_lMediaID As Integer,
                              ByVal v_lCountryID As Object,
                              ByVal v_sNumber As Object,
                              ByRef r_bValid As Boolean,
                              Optional ByRef r_sBankName As String = "",
                              Optional ByRef r_sAddress1 As String = "",
                              Optional ByRef r_sAddress2 As String = "",
                              Optional ByRef r_sAddress3 As String = "",
                              Optional ByRef r_sAddress4 As String = "",
                              Optional ByRef r_sPostalCode As String = "",
                              Optional ByRef r_vValidationMessage As Object = Nothing,
                              Optional ByRef r_bValidationOverridable As Boolean = False,
                              Optional ByVal sMediaCode As String = "",
                              Optional ByVal sBIC As String = "",
                              Optional ByVal sIBAN As String = "")

        Dim sScript As String = String.Empty
        Dim sMediaTypeValidationCode As String = String.Empty
        Dim sLocale As String = String.Empty
        Dim sOptionValue As String = String.Empty
        Dim bRunClaimRules As Boolean = False
        Try

            If sMediaCode = "" Then
                'TR - First Find out if Validation is Required
                If GetValidationSwitch(v_lMediaID) Then
                    sMediaTypeValidationCode = GetValidationCode(v_lMediaID)
                Else
                    r_bValid = True
                    Exit Sub
                End If
            ElseIf sMediaCode <> "" AndAlso Not sMediaCode.Contains("_CLMPAYABLE") Then
                sMediaTypeValidationCode = sMediaCode
            Else
                If sMediaCode.Contains("_CLMPAYABLE") Then
                    Dim sMediaRef As String() = sMediaCode.Split("_CLMPAYABLE")
                    If sMediaRef.Length > 0 Then
                        sMediaTypeValidationCode = sMediaRef(0)
                        bRunClaimRules = True
                    End If
                End If
            End If

            If Not v_lCountryID Is Nothing AndAlso v_lCountryID > 0 Then
                sLocale = GetLocaleFromCountry(v_lCountryID)
            End If

            m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iUserID:=m_iUserID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=GeneralConst.kSystemOptionMediaTypeIsCompliedRuleEnabled, r_sOptionValue:=sOptionValue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            If bRunClaimRules Then
                If Not CType(GetClaimFile(sLocale, sMediaTypeValidationCode, sScript, sOptionValue), Boolean) Then
                    'no valid script files, return true
                    r_bValid = True
                    Exit Sub
                End If
                If ToSafeInteger(sOptionValue) = 0 Then
                    'Call RunScript to run the script file (v_sScript), pass in the
                    'cc or bank account number, return true or false
                    RunScript(sScript,
                          v_iReference:=CStr(v_sNumber),
                          v_bValidDetails:=r_bValid,
                          sBankName:=r_sBankName,
                          sAddress1:=r_sAddress1,
                          sAddress2:=r_sAddress2,
                          sAddress3:=r_sAddress3,
                          sAddress4:=r_sAddress4,
                          sPostalCode:=r_sPostalCode,
                          vValidationMessage:=r_vValidationMessage,
                          bValidationOverridable:=r_bValidationOverridable,
                          v_sMediaTypeValidationCode:=sMediaTypeValidationCode,
                          sBIC:=sBIC,
                          sIBAN:=sIBAN)
                Else
                    RunCompiledRules(sAssemblyClassName:=sScript,
                                    sNumber:="",
                             sReference:=CStr(v_sNumber),
                             bValidDetails:=r_bValid,
                             oRounded:=Nothing,
                             sBankName:=r_sBankName,
                             sAddress1:=r_sAddress1,
                             sAddress2:=r_sAddress2,
                             sAddress3:=r_sAddress3,
                             sAddress4:=r_sAddress4,
                             sPostalCode:=r_sPostalCode,
                             oValidationMessage:=r_vValidationMessage,
                             bValidationOverridable:=r_bValidationOverridable,
                             sMediaTypeValidationCode:=sMediaTypeValidationCode,
                             sBIC:=sBIC,
                             sIBAN:=sIBAN)
                End If
            Else



                'if enable compiled rules checkbox is unchecked
                If ToSafeInteger(sOptionValue) = 0 Then
                    'Call GetScriptfile to return a valid script file for the given
                    'locale and media type validation code
                    If Not CType(GetScriptFile(sLocale, sMediaTypeValidationCode, sScript), Boolean) Then
                        'no valid script files, return true
                        r_bValid = True
                        Exit Sub
                    End If

                    'Call RunScript to run the script file (v_sScript), pass in the
                    'cc or bank account number, return true or false
                    RunScript(sScript,
                              v_iReference:=CStr(v_sNumber),
                              v_bValidDetails:=r_bValid,
                              sBankName:=r_sBankName,
                              sAddress1:=r_sAddress1,
                              sAddress2:=r_sAddress2,
                              sAddress3:=r_sAddress3,
                              sAddress4:=r_sAddress4,
                              sPostalCode:=r_sPostalCode,
                              vValidationMessage:=r_vValidationMessage,
                              bValidationOverridable:=r_bValidationOverridable,
                              v_sMediaTypeValidationCode:=sMediaTypeValidationCode,
                              sBIC:=sBIC,
                              sIBAN:=sIBAN)
                Else
                    'Call GetCompiledRuleFile to return a valid compiled rule file for the given
                    'locale and media type validation code
                    If Not CType(GetCompiledRuleFile(sLocale, sMediaTypeValidationCode, sScript), Boolean) Then
                        'no valid script files, return true
                        r_bValid = True
                        Exit Sub
                    End If

                    'Call RunCompiledrules to run the compiled rule file (sAssemblyClassName), pass in the
                    'cc or bank account number, return true or false
                    RunCompiledRules(sAssemblyClassName:=sScript,
                                     sNumber:="",
                              sReference:=CStr(v_sNumber),
                              bValidDetails:=r_bValid,
                              oRounded:=Nothing,
                              sBankName:=r_sBankName,
                              sAddress1:=r_sAddress1,
                              sAddress2:=r_sAddress2,
                              sAddress3:=r_sAddress3,
                              sAddress4:=r_sAddress4,
                              sPostalCode:=r_sPostalCode,
                              oValidationMessage:=r_vValidationMessage,
                              bValidationOverridable:=r_bValidationOverridable,
                              sMediaTypeValidationCode:=sMediaTypeValidationCode,
                              sBIC:=sBIC,
                              sIBAN:=sIBAN)
                End If
            End If
        Catch excep As System.Exception


            ' Log Error.
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateNumber", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub
        End Try
    End Sub

    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: GetClaimFile
    ' PURPOSE:locate the appropriate script file
    ' AUTHOR: Steve Watton
    ' DATE: 11 October 2002, 13:38:51
    ' RETURNS: PMTrue for success
    ' CHANGES:
    ' ---------------------------------------------------------------------------
    Private Function GetClaimFile(ByVal sLocale As String, ByVal sMediaTypeValidationCode As String, ByRef v_sScript As String, Optional ByVal SOptionValue As String = "0") As Integer

        Dim result As Integer = 0
        Dim sFullPath As String = ""
        Dim intFile As Integer
        Dim lFileLength As Integer
        Dim sPathName As String = ""
        Dim lFileNumber As gPMConstants.PMEReturnCode
        Dim sStr, sStr2 As String

        Dim sClassName As String = ""
        Dim sPurePath As String = ""
        Dim sAssemblyName As String = ""
        Dim slibraryPath As String
        Dim DLLAssembly As [Assembly]
        Dim nNumberOfTypes As Integer = 0
        Dim lstDlls As New List(Of String)
        Dim bClassFilePresent As Boolean = False

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If ToSafeInteger(SOptionValue) = 0 Then
                ' Rule File 
                lFileNumber = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="RulePath", v_sSubKey:="GIS", r_sSettingValue:=sPathName), gPMConstants.PMEReturnCode)

                ''Build the path to the script file
                'sFullPath = sPathName & "\" & "MEDIAVALIDATE_" & sMediaTypeValidationCode & "_" & sLocale.Trim() & ".rul"

                ''locate the file
                'If FileSystem.Dir(sFullPath, FileAttribute.Normal) = "" Then
                '    'file does not exist, try and find a generic one
                '   sFullPath = sPathName & "\" & "MEDIAVALIDATE_" & sMediaTypeValidationCode & "_GENERIC.rul"
                sFullPath = sPathName & "\" & "MEDIAVALIDATE_CLMPAY_GENERIC.rul"

                If FileSystem.Dir(sFullPath, FileAttribute.Normal) = "" Then
                    'generic file does not exist
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                '  End If

                intFile = FileSystem.FreeFile()

                FileSystem.FileOpen(intFile, sFullPath, OpenMode.Input)
                lFileLength = FileSystem.LOF(intFile)

                'read the basic into the string variable
                sStr2 = FileSystem.InputString(intFile, lFileLength)

                FileSystem.FileClose(intFile)

                'build the full script
                sStr = ""

                sStr = sStr & "Option Explicit" & Strings.ChrW(13) & Strings.ChrW(10)

                sStr = sStr & sStr2 & Strings.ChrW(13) & Strings.ChrW(10)

                'return the script
                v_sScript = sStr.Trim()
            Else
                ' Compiled Rule
                sAssemblyName = SOptionValue & ".dll"
                sClassName = sMediaTypeValidationCode & "_" & sLocale.Trim()

                sPurePath = New Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath
                sPurePath = Path.GetDirectoryName(sPurePath)
                slibraryPath = Path.Combine(sPurePath, sAssemblyName)

                Dim oTypes() As System.Type
                If IO.File.Exists(slibraryPath) Then
                    DLLAssembly = [Assembly].LoadFrom(slibraryPath)
                    oTypes = DLLAssembly.GetExportedTypes()
                    For Each oType As Type In oTypes
                        If oType.Name = sClassName Then
                            bClassFilePresent = True
                            Exit For
                        End If
                    Next
                End If
                If bClassFilePresent Then
                    v_sScript = "MediaValidate." & sClassName
                Else
                    v_sScript = "MediaValidate." & "CLMPAY" & "_GENERIC"
                End If
            End If


            Return result
        Catch excep As System.Exception



            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimFile", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                    Return gPMConstants.PMEReturnCode.PMFalse

            End Select

            Return result
        End Try
    End Function


    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: RoundCurrency
    ' PURPOSE:Rounds the currency dependant on the given locale
    ' AUTHOR: Steve Watton
    ' DATE: 11 October 2002, 13:58:05
    ' CHANGES:
    ' ---------------------------------------------------------------------------

    Public Sub RoundCurrency(ByVal v_lMediaID As Integer, ByVal v_lCountryID As Object, ByVal v_dAmount As Double, ByRef r_dRoundedAmount As Double)

        Dim v_sScript As String = String.Empty
        Dim sMediaTypeValidationCode As String = String.Empty
        Dim sLocale As String = String.Empty
        Dim sOptionValue As String = String.Empty

        Const TwoDP As Integer = 2

        Try
            m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iUserID:=m_iUserID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=GeneralConst.kSystemOptionMediaTypeIsCompliedRuleEnabled, r_sOptionValue:=sOptionValue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            sMediaTypeValidationCode = GetValidationCode(v_lMediaID)
            sLocale = GetLocaleFromCountry(v_lCountryID)

            If sOptionValue = "0" Then
                'get the correct script file
                If Not CType(GetScriptFile(sLocale, sMediaTypeValidationCode, v_sScript), Boolean) Then
                    'no script file exists, return the original amount
                    r_dRoundedAmount = v_dAmount
                    Exit Sub
                End If
            Else
                If Not CType(GetCompiledRuleFile(sLocale, sMediaTypeValidationCode, v_sScript), Boolean) Then
                    'no script file exists, return the original amount
                    r_dRoundedAmount = v_dAmount
                    Exit Sub
                End If
            End If

            'Just in case the number is to more than 2 dp
            'first check for existance of dp.
            If CStr(v_dAmount).Length - (CStr(v_dAmount).IndexOf("."c) + 1) > TwoDP Then
                v_dAmount = Math.Round(v_dAmount, TwoDP)
            End If

            If CStr(v_dAmount).IndexOf("."c) >= 0 Then
                If sOptionValue = "0" Then
                    'call runscript to run the script file pass in number to round, return the rounded value (r_dRoundedAmount)
                    RunScript(v_sScript, v_sNumber:=CStr(v_dAmount), v_dRounded:=r_dRoundedAmount)
                Else
                    RunCompiledRules(sAssemblyClassName:=v_sScript,
                                    sNumber:=CStr(v_dAmount),
                                    sReference:=CStr(v_dAmount),
                                    bValidDetails:=False,
                                    oRounded:=r_dRoundedAmount,
                                    sBankName:="",
                                    sAddress1:="",
                                    sAddress2:="",
                                    sAddress3:="",
                                    sAddress4:="",
                                    sPostalCode:="",
                                    oValidationMessage:=Nothing,
                                    bValidationOverridable:=False,
                                    sMediaTypeValidationCode:="",
                                    sBIC:="",
                                    sIBAN:="")
                End If
            Else
                r_dRoundedAmount = v_dAmount
            End If

        Catch excep As System.Exception



            Select Case Informations.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="RoundCurrency", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)


            End Select

        End Try

    End Sub


    ''' <summary>
    ''' Runs the appropriate validation script
    ''' </summary>
    ''' <param name="v_sScript"></param>
    ''' <param name="v_sNumber"></param>
    ''' <param name="v_iReference"></param>
    ''' <param name="v_bValidDetails"></param>
    ''' <param name="v_dRounded"></param>
    ''' <param name="sBankName"></param>
    ''' <param name="sAddress1"></param>
    ''' <param name="sAddress2"></param>
    ''' <param name="sAddress3"></param>
    ''' <param name="sAddress4"></param>
    ''' <param name="sPostalCode"></param>
    ''' <param name="vValidationMessage"></param>
    ''' <param name="bValidationOverridable"></param>
    ''' <param name="v_sMediaTypeValidationCode"></param>
    ''' <param name="sBIC"></param>
    ''' <param name="sIBAN"></param>
    ''' <remarks></remarks>
    Private Sub RunScript(ByVal v_sScript As String,
                          Optional ByVal v_sNumber As String = "",
                          Optional ByVal v_iReference As String = "",
                          Optional ByRef v_bValidDetails As Boolean = False,
                          Optional ByRef v_dRounded As Object = Nothing,
                          Optional ByRef sBankName As String = "",
                          Optional ByRef sAddress1 As String = "",
                          Optional ByRef sAddress2 As String = "",
                          Optional ByRef sAddress3 As String = "",
                          Optional ByRef sAddress4 As String = "",
                          Optional ByRef sPostalCode As String = "",
                          Optional ByRef vValidationMessage As Object = Nothing,
                          Optional ByRef bValidationOverridable As Boolean = False,
                          Optional ByVal v_sMediaTypeValidationCode As String = "",
                          Optional ByVal sBIC As String = "",
                          Optional ByVal sIBAN As String = "")

        Dim oSharedStorage As SharedStorage
        'Dim oScriptControl As MSScriptControl.ScriptControl
        Dim scriptExecutor As New VBQuoteEngine()

        Try

            'create script control object
           ' oScriptControl = New MSScriptControl.ScriptControl()

            'oScriptControl.Language = "VBScript"

            'create shared storage object, used to hold values that are read/writable from the VB script file
            oSharedStorage = New SharedStorage()

            'check for presense of v_snumber and store in SharedStorage object

            If Not Informations.IsNothing(v_sNumber) Then
                oSharedStorage.vAmount = v_sNumber.Trim()
            End If

            'check for presense of v_ireference and store in SharedStorage object

            If Not Informations.IsNothing(v_iReference) Then
                If v_sMediaTypeValidationCode = "CC" Then
                    oSharedStorage.vReference = v_iReference.Trim()
                Else
                    oSharedStorage.vReference = v_iReference.Trim()

                    Dim VREFSpilt As Array = v_iReference.Split("|"c)

                    oSharedStorage.sBankBranchCode = VREFSpilt(0)
                    oSharedStorage.sAccountNo = VREFSpilt(1)

                    If (VREFSpilt.Length > 2) Then
                        oSharedStorage.sAccountType = VREFSpilt(2)
                    End If
                End If
            End If

            oSharedStorage.sBIC = sBIC
            oSharedStorage.sIBAN = sIBAN
            If Not String.IsNullOrEmpty(sBankName) Then
                oSharedStorage.sBankName = sBankName
            End If
            'Add reference to sharedstorage object on the scriptcontrol object
            'oScriptControl.AddObject("oSharedStorage", oSharedStorage, False)


            If m_bServerDebuggingEnabled Then

                Dim find As String = "Sub Start()" & Strings.ChrW(13) & Strings.ChrW(10)
                Dim replace As String = "Sub Start()" & Strings.ChrW(13) & Strings.ChrW(10) & "Stop" & Strings.ChrW(13) & Strings.ChrW(10)
                v_sScript = v_sScript.Replace(find, replace)
                ' v_sScript = Replace(v_sScript, "Sub Start()" & Strings.ChrW(13) & Strings.ChrW(10), "Sub Start()" & Strings.ChrW(13) & Strings.ChrW(10) & "Stop" & Strings.ChrW(13) & Strings.ChrW(10), , , CompareMethod.Text)
            End If

            'read in the script and run it
            'oScriptControl.AddCode(v_sScript.Trim())
            'oScriptControl.Run("start")
            scriptExecutor.RunMediaTypeValidation(v_sScript.Trim(), "Start", oSharedStorage)
            'return valid flag is applicable

            If Not Informations.IsNothing(v_bValidDetails) Then
                v_bValidDetails = gPMFunctions.ToSafeBoolean(oSharedStorage.vValid)
            End If

            'return rounded currency if applicable

            If Not Informations.IsNothing(v_dRounded) Then


                v_dRounded = oSharedStorage.vRoundedAmount
            End If

            'return bankname if applicable
            If Not Informations.IsNothing(oSharedStorage.sBankName) Then
                sBankName = oSharedStorage.sBankName
            End If

            'return address1 if applicable
            If Not Informations.IsNothing(oSharedStorage.sAddress1) Then
                sAddress1 = oSharedStorage.sAddress1
            End If

            'return address2 if applicable
            If Not Informations.IsNothing(oSharedStorage.sAddress2) Then
                sAddress2 = oSharedStorage.sAddress2
            End If

            'return address3 if applicable
            If Not Informations.IsNothing(oSharedStorage.sAddress3) Then
                sAddress3 = oSharedStorage.sAddress3
            End If

            'return address4 if applicable
            If Not Informations.IsNothing(oSharedStorage.sAddress4) Then
                sAddress4 = oSharedStorage.sAddress4
            End If

            'return postalcode if applicable
            If Not Informations.IsNothing(oSharedStorage.sPostalCode) Then
                sPostalCode = oSharedStorage.sPostalCode
            End If

            'return validationmessage if applicable
            If Not Informations.IsNothing(oSharedStorage.vValidationMessage) Then
                vValidationMessage = oSharedStorage.vValidationMessage
            End If


            'return validationoverridable flag if applicable
            If Not Informations.IsNothing(oSharedStorage.bValidationOverridable) Then
                bValidationOverridable = gPMFunctions.ToSafeBoolean(oSharedStorage.bValidationOverridable)

            End If

            oSharedStorage = Nothing
           ' oScriptControl = Nothing
        Catch excep As System.Exception


            Select Case Informations.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="RunScript", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            End Select


        End Try

    End Sub

    ''' <summary>
    ''' Run compiled files
    ''' </summary>
    ''' <param name="sAssemblyClassName"></param>
    ''' <param name="sNumber"></param>
    ''' <param name="sReference"></param>
    ''' <param name="bValidDetails"></param>
    ''' <param name="oRounded"></param>
    ''' <param name="sBankName"></param>
    ''' <param name="sAddress1"></param>
    ''' <param name="sAddress2"></param>
    ''' <param name="sAddress3"></param>
    ''' <param name="sAddress4"></param>
    ''' <param name="sPostalCode"></param>
    ''' <param name="oValidationMessage"></param>
    ''' <param name="bValidationOverridable"></param>
    ''' <param name="sMediaTypeValidationCode"></param>
    ''' <param name="sBIC"></param>
    ''' <param name="sIBAN"></param>
    ''' <remarks></remarks>
    Private Sub RunCompiledRules(ByVal sAssemblyClassName As String,
                           ByVal sNumber As String,
                           ByVal sReference As String,
                           ByRef bValidDetails As Boolean,
                           ByRef oRounded As Object,
                           ByRef sBankName As String,
                           ByRef sAddress1 As String,
                           ByRef sAddress2 As String,
                           ByRef sAddress3 As String,
                           ByRef sAddress4 As String,
                           ByRef sPostalCode As String,
                           ByRef oValidationMessage As Object,
                           ByRef bValidationOverridable As Boolean,
                           ByVal sMediaTypeValidationCode As String,
                           ByVal sBIC As String,
                           ByVal sIBAN As String)

        Dim oSharedStorage As SharedStorage
        Dim oRules As Object

        Try
            'create shared storage object, used to hold values that are read/writable from the VB script file
            oSharedStorage = New SharedStorage()

            'check for presense of v_snumber and store in SharedStorage object
            If Not Informations.IsNothing(sNumber) Then
                oSharedStorage.vAmount = sNumber.Trim()
            End If

            'check for presense of v_ireference and store in SharedStorage object

            If Not Informations.IsNothing(sReference) Then
                If sMediaTypeValidationCode = "CC" Then
                    oSharedStorage.vReference = sReference.Trim()
                Else
                    oSharedStorage.vReference = sReference.Trim()

                    oSharedStorage.sBankBranchCode = sReference.Split("|"c)(0)
                    oSharedStorage.sAccountNo = sReference.Split("|"c)(1)
                End If
            End If

            oSharedStorage.sBIC = sBIC
            oSharedStorage.sIBAN = sIBAN

            oRules = CreateLateBoundObject_CompiledRules(sAssemblyClassName)
            If Not (oRules Is Nothing) Then
                oRules.oSharedStorage = oSharedStorage
                oRules.Start()

                'return valid flag is applicable
                If Not Informations.IsNothing(bValidDetails) Then
                    bValidDetails = gPMFunctions.ToSafeBoolean(oRules.oSharedStorage.vValid)
                End If

                'return rounded currency if applicable
                If Not Informations.IsNothing(oRounded) Then
                    oRounded = oRules.oSharedStorage.vRoundedAmount
                End If

                'return bankname if applicable
                If Not Informations.IsNothing(oRules.oSharedStorage.sBankName) Then
                    sBankName = oRules.oSharedStorage.sBankName
                End If

                'return address1 if applicable
                If Not Informations.IsNothing(oRules.oSharedStorage.sAddress1) Then
                    sAddress1 = oRules.oSharedStorage.sAddress1
                End If

                'return address2 if applicable
                If Not Informations.IsNothing(oRules.oSharedStorage.sAddress2) Then
                    sAddress2 = oRules.oSharedStorage.sAddress2
                End If

                'return address3 if applicable
                If Not Informations.IsNothing(oRules.oSharedStorage.sAddress3) Then
                    sAddress3 = oRules.oSharedStorage.sAddress3
                End If

                'return address4 if applicable
                If Not Informations.IsNothing(oRules.oSharedStorage.sAddress4) Then
                    sAddress4 = oRules.oSharedStorage.sAddress4
                End If

                'return postalcode if applicable
                If Not Informations.IsNothing(oRules.oSharedStorage.sPostalCode) Then
                    sPostalCode = oRules.oSharedStorage.sPostalCode
                End If

                'return validationmessage if applicable
                If Not Informations.IsNothing(oRules.oSharedStorage.vValidationMessage) Then
                    oValidationMessage = oRules.oSharedStorage.vValidationMessage
                End If


                'return validationoverridable flag if applicable
                If Not Informations.IsNothing(oRules.oSharedStorage.bValidationOverridable) Then
                    bValidationOverridable = gPMFunctions.ToSafeBoolean(oRules.oSharedStorage.bValidationOverridable)
                End If
            End If

            oSharedStorage = Nothing
        Catch excep As System.Exception
            ' Log Error.
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="RunCompiledRules", excep:=excep)
        Finally
            oRules = Nothing
            oSharedStorage = Nothing
        End Try
    End Sub

    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: GetScriptFile
    ' PURPOSE:locate the appropriate script file
    ' AUTHOR: Steve Watton
    ' DATE: 11 October 2002, 13:38:51
    ' RETURNS: PMTrue for success
    ' CHANGES:
    ' ---------------------------------------------------------------------------
    Private Function GetScriptFile(ByVal sLocale As String, ByVal sMediaTypeValidationCode As String, ByRef v_sScript As String) As Integer

        Dim result As Integer = 0
        Dim sFullPath As String = ""
        Dim intFile As Integer
        Dim lFileLength As Integer
        Dim sPathName As String = ""
        Dim lFileNumber As gPMConstants.PMEReturnCode
        Dim sStr, sStr2 As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            'get the path to the validation script from thje registry
            '
            lFileNumber = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="RulePath", v_sSubKey:="GIS", r_sSettingValue:=sPathName), gPMConstants.PMEReturnCode)

            'Build the path to the script file
            sFullPath = sPathName & "\" & "MEDIAVALIDATE_" & sMediaTypeValidationCode & "_" & sLocale.Trim() & ".rul"

            'locate the file
            If FileSystem.Dir(sFullPath, FileAttribute.Normal) = "" Then
                'file does not exist, try and find a generic one
                sFullPath = sPathName & "\" & "MEDIAVALIDATE_" & sMediaTypeValidationCode & "_GENERIC.rul"

                If FileSystem.Dir(sFullPath, FileAttribute.Normal) = "" Then
                    'generic file does not exist
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            intFile = FileSystem.FreeFile()

            FileSystem.FileOpen(intFile, sFullPath, OpenMode.Input)
            lFileLength = FileSystem.LOF(intFile)

            'read the basic into the string variable
            sStr2 = FileSystem.InputString(intFile, lFileLength)

            FileSystem.FileClose(intFile)

            'build the full script
            sStr = ""

            sStr = sStr & "Option Explicit" & Strings.Chrw(13) & Strings.Chrw(10)

            sStr = sStr & sStr2 & Strings.Chrw(13) & Strings.Chrw(10)

            'return the script
            v_sScript = sStr.Trim()

            Return result
        Catch excep As System.Exception



            Select Case Informations.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetScriptFile", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                    Return gPMConstants.PMEReturnCode.PMFalse

            End Select

            Return result
        End Try
    End Function

    ''' <summary>
    ''' Get compiled rule file based on media type and locale
    ''' </summary>
    ''' <param name="sLocale"></param>
    ''' <param name="sMediaTypeValidationCode"></param>
    ''' <param name="v_sScript"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetCompiledRuleFile(ByVal sLocale As String, ByVal sMediaTypeValidationCode As String, ByRef v_sScript As String) As Integer

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim sFullPath As String = ""
        Dim sClassName As String = ""
        Dim sPurePath As String = ""
        Dim sAssemblyName As String = ""
        Dim slibraryPath As String
        Dim DLLAssembly As [Assembly]
        Dim nNumberOfTypes As Integer = 0
        Dim lstDlls As New List(Of String)
        Dim bClassFilePresent As Boolean = False
        Dim sOptionValue As String = String.Empty

        nResult = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iUserID:=m_iUserID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=GeneralConst.kSystemOptionMediaTypeIsCompliedRuleEnabled, r_sOptionValue:=sOptionValue)

        If String.IsNullOrEmpty(sOptionValue) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        sAssemblyName = sOptionValue & ".dll"
        sClassName = sMediaTypeValidationCode & "_" & sLocale.Trim()

        sPurePath = New Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath
        sPurePath = Path.GetDirectoryName(sPurePath)
        slibraryPath = Path.Combine(sPurePath, sAssemblyName)

        Dim oTypes() As System.Type
        If IO.File.Exists(slibraryPath) Then
            DLLAssembly = [Assembly].LoadFrom(slibraryPath)
            oTypes = DLLAssembly.GetExportedTypes()
            For Each oType As Type In oTypes
                If oType.Name = sClassName Then
                    bClassFilePresent = True
                    Exit For
                End If
            Next
        End If
        If bClassFilePresent Then
            v_sScript = "MediaValidate." & sClassName
        Else
            v_sScript = "MediaValidate." & sMediaTypeValidationCode & "_GENERIC"
        End If

        Return nResult
    End Function

    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: GetLocaleFromCountry
    ' PURPOSE:gets the locale from a given country ID
    ' AUTHOR: Steve Watton
    ' DATE: 18 October 2002, 13:58:05
    ' CHANGES:
    ' ---------------------------------------------------------------------------

    Private Function GetLocaleFromCountry(ByVal lCountryID As Object) As String
        Dim vLocaleArray(,) As Object = Nothing
        Dim sCountryCode As String = String.Empty



        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add cashdrawerid as an input param for select

        m_lReturn = m_oDatabase.Parameters.Add(sName:="countryid", vValue:=CStr(lCountryID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetLocaleFromCountryIDSQL, sSQLName:=ACGetLocaleFromCountryIDName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vLocaleArray)



        Return CStr(vLocaleArray(0, 0)).Trim()

    End Function


    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: GetvalidationCode
    ' PURPOSE: gets the media type validation code from the media ID
    ' AUTHOR: Steve Watton
    ' DATE: 11 October 2002, 13:58:05
    ' CHANGES: 04-11-2002 Made Public
    ' ---------------------------------------------------------------------------

    Public Function GetValidationCode(ByVal v_lMediaID As Integer) As String

        Dim vMediaCodeArray(,) As Object = Nothing

        Try

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add cashdrawerid as an input param for select
            m_lReturn = m_oDatabase.Parameters.Add(sName:="mediaid", vValue:=CStr(v_lMediaID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetValidationCodeForMediaTypeSQL, sSQLName:=ACGetValidationCodeForMediaTypeName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vMediaCodeArray)


            If Informations.IsArray(vMediaCodeArray) Then

                Return CStr(vMediaCodeArray(0, 0)).Trim()
            Else
                Return ""
            End If

        Catch
        End Try




        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetValidationCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetValidationCode", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
        Return String.Empty
        Exit Function

    End Function

    '*************************************************************************
    ' Name :        GetValidationSwitch
    ' Description:  Gets the MediaType and checks to see if Validation is
    '               switched on or not
    ' Created:      Tracy Richards 02/07/03
    ' Histroy:
    '*************************************************************************
    Private Function GetValidationSwitch(ByVal v_lMediaTypeID As Integer) As Boolean

        Dim result As Boolean = False
        Dim vResultArray(,) As Object = Nothing



        'TR - Assume that the switch is OFF

        With m_oDatabase
            ' Clear the Database Parameters Collection
            .Parameters.Clear()

            'TR - Add MedaitTypeID
            m_lReturn = m_oDatabase.Parameters.Add("mediatype_id", CStr(v_lMediaTypeID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(ACGetMediaTypeSQL, ACGetMediaTypeName, True, gPMConstants.PMAllRecords, vResultArray)
        End With

        'TR - Make sure that this worked OK
        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            'PN: 48388
            If Informations.IsArray(vResultArray) Then
                '10th parameter is Is_Validation_enabled
                If gPMFunctions.NullToLong(vResultArray(9, 0)) = 1 Then
                    'TR - Only turn on Validation here if everything has
                    'succeeded and the Flag = 1
                    result = True
                End If
            End If
        End If

        Return result

    End Function

    '*************************************************************************
    ' Name :        GetMediaTypeIdForCode
    ' Description:  Gets the MediaType Id for the MediaType Code
    ' Created:      Gautam Poddar 07 Aug 2008
    ' Histroy:
    '*************************************************************************
    Public Function GetMediaTypeIdForCode(ByVal v_sCode As String, ByRef r_lMediaTypeId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetMediaTypeIdForCode"

        Dim vResultArray(,) As Object = Nothing

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add input parameter for select
            m_lReturn = m_oDatabase.Parameters.Add(sName:="MediaType_Code", vValue:=v_sCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetMediaTypeIdForCodeSQL, sSQLName:=ACGetMediaTypeIdForCodeName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACGetMediaTypeIdForCodeName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Informations.IsArray(vResultArray) Then

                r_lMediaTypeId = gPMFunctions.ToSafeLong(CStr(vResultArray(0, 0)))
            End If

            Return result

        Catch ex As System.Exception



            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here



            Return result



            Return result
        End Try
    End Function
    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class