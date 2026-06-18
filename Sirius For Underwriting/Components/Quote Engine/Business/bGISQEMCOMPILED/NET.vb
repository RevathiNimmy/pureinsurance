Option Strict Off
Option Explicit On
Imports System.IO
Imports SSP.Shared
Imports System.Runtime.ExceptionServices
<System.Runtime.InteropServices.ProgId("VBS_NET.VBS")>
Public NotInheritable Class NET
    Implements IDisposable
#If quoteTiming Then
    	Private Declare Function QueryPerformanceCounter Lib "kernel32" (x As Currency) As Boolean
	Private Declare Function QueryPerformanceFrequency Lib "kernel32" (x As Currency) As Boolean
	Private PerformanceCtr(100, 2) As Variant, Freq As Currency
#End If
    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer

    Private Const ACClass As String = "NET"

    Dim m_iLevel As Integer
    Dim m_iInstance(6) As Integer
    Dim g_sDataModelCode As String = ""
    Private m_oDatabase As dPMDAO.Database
    Private m_bCloseDatabase As Boolean
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_oDataSet As cGISDataSetControl.Application
    Private m_sRulePath As String = ""
    Private m_sSchemePath As String = ""
    Private m_sDictionaryPath As String = ""
    Private Err_No As Integer
    Private Err_Line As Integer
    Private Err_Col As Integer
    Private Err_Description As String = ""
    Private Err_Text As String = ""

    Private m_lGISScreenId As Integer
    Private sRuleFileLocation As String = ""
    Private m_sCompiledScript As String = ""
    Private m_dtGISEffectiveDate As Object
    Private m_bServerDebuggingEnabled As Boolean

    Public Property EffectiveDate() As Date
        Get
            Return m_dtGISEffectiveDate
        End Get
        Set(ByVal Value As Date)

            m_dtGISEffectiveDate = CDate(Value)
        End Set
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    Public ReadOnly Property ScriptErrorLine() As Integer
        Get
            Return Err_Line
        End Get
    End Property

    Public ReadOnly Property ScriptErrorColumn() As Integer
        Get
            Return Err_Col
        End Get
    End Property

    Public ReadOnly Property ScriptErrorDescription() As String
        Get
            Return Err_Description
        End Get
    End Property

    Public ReadOnly Property ScriptErrorNo() As Integer
        Get
            Return Err_No
        End Get
    End Property

    Public Property RuleFile() As String
        Get
            Return sRuleFileLocation
        End Get
        Set(ByVal Value As String)

            sRuleFileLocation = CStr(Value)
        End Set
    End Property

    Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer

        Dim oUserAuthority As bACTUserAuthorities.Business
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

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
            m_bServerDebuggingEnabled = False

            Dim sValue As String = ""

            bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iUserID:=m_iUserID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=5084, r_sOptionValue:=sValue)

            Dim vParams As Object = Nothing
            Const ACUserServerScriptsRunInDebug As Integer = 14
            If sValue = "1" Then

                oUserAuthority = New bACTUserAuthorities.Business
                m_lReturn = CType(oUserAuthority.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

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

                'oUserAuthority.Terminate()
                oUserAuthority.Dispose()
                oUserAuthority = Nothing
            End If
            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try

    End Function

    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                m_oDataSet = Nothing
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    Public Function InitialiseEngine(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String) As Integer
        Dim result As Integer = 0
        Dim sSubKey As String = ""

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            g_sDataModelCode = v_sGisDataModelCode
            sSubKey = GISSharedConstants.ACOIMGISSubKey & "\" & v_sGisDataModelCode

            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="RulePath", r_sSettingValue:=m_sRulePath, v_sSubKey:=sSubKey), gPMConstants.PMEReturnCode)

            If Not m_sRulePath.EndsWith("\") Then
                m_sRulePath = m_sRulePath & "\"
            End If

            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="CompiledSCript", r_sSettingValue:=m_sCompiledScript, v_sSubKey:=sSubKey), gPMConstants.PMEReturnCode)

            If m_oDatabase Is Nothing Then
                m_lReturn = CType(gPMComponentServices.NewDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error Opening Database", vApp:=ACApp, vClass:=ACClass, vMethod:="InitialiseEngine", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result
                End If
            End If
            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InitialiseEngineFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="InitialiseEngine", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' RAW 22/07/2003 : CQ1672 : added
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Informations.IsNothing(vTask) Then
                m_iTask = CInt(vTask)
            End If

            If Not Informations.IsNothing(vNavigate) Then
                m_lNavigate = CInt(vNavigate)
            End If

            If Not Informations.IsNothing(vProcessMode) Then
                m_lProcessMode = CInt(vProcessMode)
            End If

            If Not Informations.IsNothing(vTransactionType) Then
                m_sTransactionType = CStr(vTransactionType)
            End If

            If Not Informations.IsNothing(vEffectiveDate) Then
                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Public Function NBQuote(ByVal v_lQuoteType As Integer,
                            ByRef r_oDataset As Object, ByVal v_dtEffectiveDate As Date,
                            Optional ByRef r_vAdditionalDataArray As Object = Nothing,
                            Optional ByVal v_vRatingInfo As Object = Nothing,
                            Optional ByVal v_bIsBackdatedMTA As Boolean = False,
                            Optional ByVal v_bAfterPRETriggerRules As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim sAppRuleFile As String = ""
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sDataModelCode, sBusinessType As String
        Dim dCurrentDate As Date
        Dim iActive As Integer

        Const kSInsurerName = "Unknown Insurer"
        Const kIInsurerNumber = "9999"

        Dim sXMLDataSetDef As String = ""
        Dim bQuotedOnce As Boolean

        Dim vPartyId As String = ""
        Dim lQuoteType, lTransactionType As Integer

        Dim sSQL As String = ""
        Dim vArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            bQuotedOnce = False

            ' Clear any existing Quote Output
            lReturn = r_oDataset.ClearAllQuoteOutput()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            sDataModelCode = r_oDataset.GISDataModelCode
            dCurrentDate = GetEffectiveDate()

            If IsToQuoteForTest() Then
                iActive = 99
            Else
                iActive = 1
            End If

            PBQuoteTypeEncode.decodeTransactionScreenAndType(v_lQuoteType, lTransactionType, m_lGISScreenId, lQuoteType)

            'User authority levels use different transactio types
            If (lTransactionType <> 0) And lQuoteType <> PBQuoteTypeEncode.PBCQemQuoteTypeUal Then
                sSQL = "Select code from transaction_type" & Strings.ChrW(13) & Strings.ChrW(10) &
                       "where transaction_type_id = " & CStr(lTransactionType)

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetTransactionType", bStoredProcedure:=False, vResultArray:=vArray)

                If Informations.IsArray(vArray) Then
                    m_sTransactionType = CStr(vArray(0, 0)).Trim()
                    vArray = Nothing
                End If
            End If

            If lQuoteType = PBQuoteTypeEncode.PBCQemQuoteTypeUal And lTransactionType = 0 Then
                lTransactionType = 1
            End If

            sBusinessType = "1"

            lReturn = r_oDataset.InitialiseLookups(v_sDataModelCode:=CStr(sDataModelCode), v_sBusinessTypeCode:=CStr(sBusinessType), v_dtProcessDate:=ToSafeDate(dCurrentDate), v_lStatus:=ToSafeInteger(iActive))

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialising Lookups Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NBQuote", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'Initialise Lookups.
            r_oDataset.LookupsRequiredInsurerNo = kIInsurerNumber

            Select Case lQuoteType
                Case PBQuoteTypeEncode.PBCQemQuoteTypeQuote
                    ' Call the Quote Method
                    lReturn = CType(Quote(oDataSet:=r_oDataset, sRuleFile:=sAppRuleFile, sInsurerName:=kSInsurerName, InsurerId:=kIInsurerNumber, InstanceNo:=1, v_lTransactionType:=lTransactionType, r_vAdditionalDataArray:=r_vAdditionalDataArray, v_bIsBackdatedMTA:=v_bIsBackdatedMTA), gPMConstants.PMEReturnCode)
                Case PBQuoteTypeEncode.PBCQemQuoteTypeValidate
                    ' Call the Validate Method
                    lReturn = CType(Validate(oDataSet:=r_oDataset, sRuleFile:=sAppRuleFile, sInsurerName:=kSInsurerName, InsurerId:=kIInsurerNumber, InstanceNo:=1, r_vAdditionalDataArray:=r_vAdditionalDataArray), gPMConstants.PMEReturnCode)
                Case PBQuoteTypeEncode.PBCQemQuoteTypeUal
                    ' Call the Underwriting Authority Limit Method
                    lReturn = CType(UnderwritingAuthorityLimits(oDataSet:=r_oDataset, sRuleFile:=sAppRuleFile, sInsurerName:=kSInsurerName, InsurerId:=kIInsurerNumber, InstanceNo:=1, lTransactionType:=lTransactionType, r_vAdditionalDataArray:=r_vAdditionalDataArray), gPMConstants.PMEReturnCode)
                Case PBQuoteTypeEncode.PBCQemQuoteTypeRenewal
                    'call renewal method
                    lReturn = CType(Renewal(oDataSet:=r_oDataset, sRuleFile:=sAppRuleFile, sInsurerName:=kSInsurerName, InsurerId:=kIInsurerNumber, InstanceNo:=1, r_vAdditionalDataArray:=r_vAdditionalDataArray), gPMConstants.PMEReturnCode)
                            'AK 150403
                Case PBQuoteTypeEncode.PBCQemQuoteTypeRenewalLapse
                    lReturn = CType(RenewalLapse(oDataSet:=r_oDataset, sRuleFile:=sAppRuleFile, sInsurerName:=kSInsurerName, InsurerId:=kIInsurerNumber, InstanceNo:=1, r_vAdditionalDataArray:=r_vAdditionalDataArray), gPMConstants.PMEReturnCode)

                Case PBQuoteTypeEncode.PBCQemQuoteTypePreScreen, PBQuoteTypeEncode.PBCQemQuoteTypeDefault, PBQuoteTypeEncode.PBCQemQuoteTypeCopyRisk
                    ' Call the Default Method
                    lReturn = CType(Default_Renamed(oDataSet:=r_oDataset, sRuleFile:=sAppRuleFile, sInsurerName:=kSInsurerName, InsurerId:=kIInsurerNumber, InstanceNo:=1, lQuoteType:=lQuoteType, r_vAdditionalDataArray:=r_vAdditionalDataArray, v_bIsBackdatedMTA:=v_bIsBackdatedMTA), gPMConstants.PMEReturnCode)
                Case Else
                    ' Call the Quote Method
                    lReturn = CType(Quote(oDataSet:=r_oDataset, sRuleFile:=sAppRuleFile, sInsurerName:=kSInsurerName, InsurerId:=kIInsurerNumber, InstanceNo:=1, v_lTransactionType:=lTransactionType, r_vAdditionalDataArray:=r_vAdditionalDataArray, v_bIsBackdatedMTA:=v_bIsBackdatedMTA), gPMConstants.PMEReturnCode)
            End Select

            If lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                bQuotedOnce = True
            Else
                result = lReturn
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Quoting Failed. For Insurer : " & kSInsurerName, vApp:=ACApp, vClass:=ACClass, vMethod:="NBQuote")
                Return result
            End If


            If bQuotedOnce Then
                result = gPMConstants.PMEReturnCode.PMTrue
            Else
                result = lReturn
            End If


            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NBQuoteFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="NBQuote", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Public Function IsToQuoteForTest() As Boolean
        Dim result As Boolean = False
        Dim sValue As String = ""
        Dim sSubKey As String = ""

        Try
            sSubKey = GISSharedConstants.ACOIMGISSubKey & "\" & g_sDataModelCode

            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="QuoteForTest", r_sSettingValue:=sValue, v_sSubKey:=sSubKey), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return False
            End If

            Select Case sValue.Trim().ToUpper()
                Case "1", "Y", "YES"
                    Return True
                Case Else
                    Return False
            End Select
        Catch ex As Exception
            result = False
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get QuoteForTest setting from registry.", vApp:=ACApp, vClass:=ACClass, vMethod:="IsToQuoteForTest", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)
            Return result
        End Try

    End Function

    Public Function PrintForm(ByVal v_vSchemeArray As Object, ByVal v_lFormNumber As Integer, ByVal v_sXMLDataSetDef As String, ByVal v_sXMLDataSet As String) As Integer
        Return gPMConstants.PMEReturnCode.PMTrue
    End Function

    ' RFC14121999
    ' ***************************************************************** '
    ' Name: NBPostQuoteProcess
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function NBPostQuoteProcess(ByVal v_vSchemeArray As Object, ByVal v_lProcessType As Integer, ByVal v_sXMLDataSetDef As String, ByRef r_sXMLDataSet As String) As Integer
        Return gPMConstants.PMEReturnCode.PMTrue
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sAssemblyClassName"></param>
    ''' <param name="nQuoteType"></param>
    ''' <param name="oDataset"></param>
    ''' <param name="dtEffectiveDate"></param>
    ''' <param name="bPrePRE"></param>
    ''' <param name="bPostPRE"></param>
    ''' <param name="oAdditionalDataArray"></param>
    ''' <param name="bIsBackdatedMTA"></param>
    ''' <returns></returns>
    Public Function PREProcessRules(ByVal sAssemblyClassName As String, ByVal nQuoteType As Integer,
                             ByRef oDataset As cGISDataSetControl.Application, ByVal dtEffectiveDate As Date,
                             ByVal bPrePRE As Boolean, ByVal bPostPRE As Boolean,
                             ByRef oAdditionalDataArray As Object,
                             ByVal bIsBackdatedMTA As Boolean) As Integer

        Dim nResult As Integer = 0
        Dim sStr As String = ""
        Dim oArray As Object = Nothing
        Dim oExtras As Object = Nothing
        Dim sTotals As String = ""
        Dim dtCancellationDate As Date
        Dim oRules As Object = Nothing
        Dim nClaimId As Integer

        ' RVH 7/8/2003 : IAG - START: Constants for name/value pairs passed in additional data array
        Const KeyChildOIKey As String = "CHILD_OIKEY"
        Const PosChildOIKey As Integer = 0
        Const PosChildMaxItems As Integer = 0

        nResult = gPMConstants.PMEReturnCode.PMTrue

        If sAssemblyClassName = "" Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = CType(gPMComponentServices.CreateBusinessObject(r_oObject:=oExtras, v_sClassName:="bGISPMUExtras.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = oExtras.SetProcessModes(vTask:=ToSafeInteger(m_iTask), vNavigate:=ToSafeInteger(m_lNavigate), vProcessMode:=ToSafeInteger(m_lProcessMode), vTransactionType:=CStr(m_sTransactionType), vEffectiveDate:=ToSafeDate(dtEffectiveDate))

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' RVH 7/8/2003 : IAG - START: Check and read passed additional data array
        If Informations.IsArray(oAdditionalDataArray) Then
            ReDim oArray(PosChildMaxItems)
            For iLoop As Integer = oAdditionalDataArray.GetLowerBound(1) To oAdditionalDataArray.GetUpperBound(1)

                Select Case CStr(oAdditionalDataArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iLoop)).ToUpper()
                    Case KeyChildOIKey
                        oArray(PosChildOIKey) = oAdditionalDataArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop)
                End Select
            Next iLoop
        End If

        'set the extra's properties
        oExtras.GISDataModel = g_sDataModelCode
        oExtras.PolicyLinkID = oDataset.PolicyLinkID()
        oExtras.DataSet = oDataset.Risk
        dtCancellationDate = GetEffectiveDate()
        oExtras.CancellationDate = dtCancellationDate

        '   RVH 16/9/2003 - Check to see if this is a claims dataset and if
        '                   so, set the Extras object so that it knows to use
        '                   the "work" tables for tasks and events...any tasks
        '                   or events created through VBScript as part of the
        '                   claims roadmap will be copied from these work
        '                   tables to the live tables at the end of the claim
        '                   roadmap.
        m_lReturn = CType(CheckIfClaim(oDataset, nClaimId), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMFalse Then
            oExtras.TaskEventUseWorkTables = gPMConstants.PMEReturnCode.PMTrue
            oExtras.CallingProcessKeyId = nClaimId
            oExtras.CallingProcessType = 1
        End If

        Try

            oRules = CreateLateBoundObject_CompiledRules(sAssemblyClassName)

            If Not (oRules Is Nothing) Then

                Try
                    oRules.SetDefaultValue(CStr(m_sTransactionType), oAdditionalDataArray, CBool(bIsBackdatedMTA))
                Catch ex As MissingMemberException
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="NET PREProcessRules - SetDefaultValue method does not exist: " & sAssemblyClassName,
                                       vApp:=ACApp, vClass:=ACClass, vMethod:="PREProcessRules", excep:=ex)
                    oRules.Dispose()
                    oExtras.Dispose()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End Try

                Try
                    oRules.SetAll(oDataset.Risk, DirectCast(oDataset, cGISDataSetControl.Application), oExtras)
                Catch ex As MissingMemberException
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="NET PREProcessRules - SetAll method does not exist: " & sAssemblyClassName,
                                       vApp:=ACApp, vClass:=ACClass, vMethod:="PREProcessRules", excep:=ex)
                    oRules.Dispose()
                    oExtras.Dispose()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End Try

                Try
                    'we should only every be hitting this at rating time, so NB, MTA/MTC, MTR, REN
                    Select Case nQuoteType

                        Case PBQuoteTypeEncode.PBCQemQuoteTypeQuote, PBQuoteTypeEncode.PBCQemQuoteTypeRenewal

                            If bPrePRE Then
                                oRules.PreRatingRules()
                            ElseIf bPostPRE Then
                                oRules.PostRatingRules()
                            End If

                    End Select
                Catch ex As MissingMemberException
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="NET PREProcessRules PreRatingRules or PostRatingRules method does not exist: " & sAssemblyClassName,
                                       vApp:=ACApp, vClass:=ACClass, vMethod:="PREProcessRules", excep:=ex)
                    Return gPMConstants.PMEReturnCode.PMFalse
                Finally
                    oRules.Dispose()
                    oExtras.Dispose()
                End Try
            End If

            Return nResult

        Catch ex As Exception

            nResult = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="NET PREProcessRules - Engine Failed: " & sAssemblyClassName,
                               vApp:=ACApp, vClass:=ACClass, vMethod:="PREProcessRules", excep:=ex)
            Return nResult
        Finally
            oRules.Dispose()
            oExtras.Dispose()
        End Try

    End Function

    Public Sub New()
        MyBase.New()

        Try

            Err_Line = -1
            Err_Col = -1
            Err_Description = ""
            Err_Text = ""
        Catch excep As System.Exception
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", excep:=excep)
            Exit Sub
        End Try
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    ' Name: AddExtrasToDataset
    '
    ' Description: Copy some properties into other properties or
    '              set defaults
    '
    ' Date: 9/12/99
    '
    ' ***************************************************************** '
    Private Function AddExtrasToDataset() As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vData As Object = Nothing
        Dim vKeyArray As Object = Nothing
        Dim sKey As String = ""

        ' These are keys to SINGLE instances
        Dim sPolicyBinderKey As String = ""
        Dim sPolicyKey As String = ""
        Dim sVehicleKey As String = ""
        Dim sProposerKey As String = ""
        Dim sNCDKey As String = ""

        ' These are keys to multiple instances of a GIS object
        Dim vDriverKey As Object = Nothing
        Dim vClaimKey As Object = Nothing
        Dim vConvictionKey As Object = Nothing

        Dim sTopLevelObject As String = ""
        Dim sTopLevelTable As String = ""
        Dim bIsAssumedInfo As Boolean



        result = gPMConstants.PMEReturnCode.PMTrue

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Get OI Keys first
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        lReturn = m_oDataSet.GetTopLevelRiskObject(r_sObjectName:=sTopLevelObject, r_sTableName:=sTopLevelTable)

        '
        ' XEM_Policy_binder
        '

        lReturn = m_oDataSet.GetAllOIKey(sTopLevelObject, vKeyArray)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        sPolicyBinderKey = CStr(vKeyArray(0))

        '
        ' XEM_Policy
        '

        lReturn = m_oDataSet.GetAllOIKey("XEM_Policy", vKeyArray)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        sPolicyKey = CStr(vKeyArray(0))

        '
        ' XEM_Proposer
        '

        lReturn = m_oDataSet.GetAllOIKey("XEM_Proposer", vKeyArray)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        sProposerKey = CStr(vKeyArray(0))

        '
        ' XEM_Vehicle
        '

        lReturn = m_oDataSet.GetAllOIKey("XEM_Vehicle", vKeyArray)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        sVehicleKey = CStr(vKeyArray(0))

        '
        ' XEM_NCD
        '

        lReturn = m_oDataSet.GetAllOIKey("XEM_NCD", vKeyArray)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        sNCDKey = CStr(vKeyArray(0))

        '
        ' XEM_Driver
        '

        lReturn = m_oDataSet.GetAllOIKey("XEM_Driver", vKeyArray)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        vDriverKey = vKeyArray ' 0..2

        '
        ' XEM_Claim
        '

        lReturn = m_oDataSet.GetAllOIKey("XEM_Claim", vKeyArray)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        vClaimKey = vKeyArray ' 0..9

        '
        ' XEM_Conviction
        '

        lReturn = m_oDataSet.GetAllOIKey("XEM_Claim", vKeyArray)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        vConvictionKey = vKeyArray ' 0..9

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Set reqd properties
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        '
        ' POLICY, Effective_End_Date
        '

        lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:="XEM_POLICY", v_sPropertyName:="Effective_Start_Date", v_sOIKey:=sPolicyKey, r_vPropertyValue:=CStr(vData), r_bIsAssumedInfo:=bIsAssumedInfo)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Make expiry date as start date plus 1 year

        vData = CStr(CDate(vData).Day) & "/" & CStr(CDate(vData).Month) & "/" & CStr(CDate(vData).Year + 1)

        lReturn = m_oDataSet.SetPropertyValue(v_sObjectName:="XEM_POLICY", v_sPropertyName:="Effective_End_Date", v_sOIKey:=sPolicyKey, v_vPropertyValue:=CStr(vData), v_bIsAssumedInfo:=False)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '
        ' POLICY, Effective_End_Time
        '

        lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:="XEM_POLICY", v_sPropertyName:="Effective_Start_Time", v_sOIKey:=sPolicyKey, r_vPropertyValue:=CStr(vData), r_bIsAssumedInfo:=bIsAssumedInfo)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        lReturn = m_oDataSet.SetPropertyValue(v_sObjectName:="XEM_POLICY", v_sPropertyName:="Effective_End_Time", v_sOIKey:=sPolicyKey, v_vPropertyValue:=CStr(vData), v_bIsAssumedInfo:=False)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '
        ' XEM_PROPOSER, legal_protection_taken_ind
        '

        lReturn = m_oDataSet.SetPropertyValue(v_sObjectName:="XEM_PROPOSER", v_sPropertyName:="legal_protection_taken_ind", v_sOIKey:=sProposerKey, v_vPropertyValue:="No", v_bIsAssumedInfo:=False)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '
        ' XEM_VEHICLE, OWNERSHIP
        '

        lReturn = m_oDataSet.SetPropertyValue(v_sObjectName:="XEM_VEHICLE", v_sPropertyName:="ownership", v_sOIKey:=sVehicleKey, v_vPropertyValue:="1", v_bIsAssumedInfo:=False)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '
        ' XEM_VEHICLE, KEEPER
        '

        lReturn = m_oDataSet.SetPropertyValue(v_sObjectName:="XEM_VEHICLE", v_sPropertyName:="keeper", v_sOIKey:=sVehicleKey, v_vPropertyValue:="1", v_bIsAssumedInfo:=False)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '
        ' XEM_VEHICLE, VEHICLE_PRN
        '

        lReturn = m_oDataSet.SetPropertyValue(v_sObjectName:="XEM_VEHICLE", v_sPropertyName:="VEHICLE_PRN", v_sOIKey:=sVehicleKey, v_vPropertyValue:="1", v_bIsAssumedInfo:=False)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '
        ' XEM_DRIVER, PRN
        '

        For i As Integer = vDriverKey.GetLowerBound(0) To vDriverKey.GetUpperBound(0)

            sKey = CStr(vDriverKey(i))

            lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:="XEM_DRIVER", v_sPropertyName:="Date_of_birth", v_sOIKey:=sKey, r_vPropertyValue:=CStr(vData), r_bIsAssumedInfo:=bIsAssumedInfo)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If CStr(vData).Trim() <> "" Then

                lReturn = m_oDataSet.SetPropertyValue(v_sObjectName:="XEM_DRIVER", v_sPropertyName:="PRN", v_sOIKey:=sKey, v_vPropertyValue:=CStr(i + 1), v_bIsAssumedInfo:=False)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

        Next i

        '
        ' XEM_CLAIM, PRN
        ' XEM_CLAIM, bodily_injury_caused_ind
        '

        If Informations.IsArray(vClaimKey) Then

            For i As Integer = vClaimKey.GetLowerBound(0) To vClaimKey.GetUpperBound(0)

                sKey = CStr(vClaimKey(i))

                lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:="XEM_CLAIM", v_sPropertyName:="claim_type", v_sOIKey:=sKey, r_vPropertyValue:=CStr(vData), r_bIsAssumedInfo:=bIsAssumedInfo)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If CStr(vData).Trim() <> "" Then

                    lReturn = m_oDataSet.SetPropertyValue(v_sObjectName:="XEM_CLAIM", v_sPropertyName:="PRN", v_sOIKey:=sKey, v_vPropertyValue:=CStr(i + 1), v_bIsAssumedInfo:=False)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    lReturn = m_oDataSet.SetPropertyValue(v_sObjectName:="XEM_CLAIM", v_sPropertyName:="bodily_injury_caused_ind", v_sOIKey:=sKey, v_vPropertyValue:="No", v_bIsAssumedInfo:=False)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

            Next i

        End If

        '
        ' XEM_CONVICTION, PRN
        ' XEM_CONVICTION, disqualified_ind
        '

        If Informations.IsArray(vConvictionKey) Then

            For i As Integer = 0 To 8

                sKey = CStr(vConvictionKey(i))

                lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:="XEM_CONVICTION", v_sPropertyName:="code", v_sOIKey:=sKey, r_vPropertyValue:=CStr(vData), r_bIsAssumedInfo:=bIsAssumedInfo)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If CStr(vData).Trim() <> "" Then

                    lReturn = m_oDataSet.SetPropertyValue(v_sObjectName:="XEM_CONVICTION", v_sPropertyName:="PRN", v_sOIKey:=sKey, v_vPropertyValue:=CStr(i + 1), v_bIsAssumedInfo:=False)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    lReturn = m_oDataSet.SetPropertyValue(v_sObjectName:="XEM_CONVICTION", v_sPropertyName:="disqualified_ind", v_sOIKey:=sKey, v_vPropertyValue:="No", v_bIsAssumedInfo:=False)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

            Next i

        End If

        '
        ' NCD, Claimed_years
        '

        lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:="XEM_NCD", v_sPropertyName:="Claimed_years_earned", v_sOIKey:=sNCDKey, r_vPropertyValue:=CStr(vData), r_bIsAssumedInfo:=bIsAssumedInfo)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '
        ' NCD, Claimed_protection_ind
        '

        lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:="XEM_NCD", v_sPropertyName:="Claimed_protection_reqd_ind", v_sOIKey:=sNCDKey, r_vPropertyValue:=CStr(vData), r_bIsAssumedInfo:=bIsAssumedInfo)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        lReturn = m_oDataSet.SetPropertyValue(v_sObjectName:="XEM_NCD", v_sPropertyName:="Claimed_protected_ind", v_sOIKey:=sNCDKey, v_vPropertyValue:=CStr(vData), v_bIsAssumedInfo:=False)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: Quote
    '
    ' Description: run the quote rule
    '
    ' History: 12/07/2001 TN - Created.
    '
    ' RVH07082003   : Add additional data array parameter to allow other info
    '                 to be passed down to the script.
    ' RAW 15/11/2004 : Pricing Changes : added v_lTransactionType param ( default to NB if missing)
    ' ***************************************************************** '
    Public Function Quote(ByRef oDataSet As cGISDataSetControl.Application, Optional ByRef sRuleFile As Object = Nothing, Optional ByRef sInsurerName As Object = Nothing, Optional ByRef InsurerId As Object = Nothing, Optional ByRef InstanceNo As Object = Nothing, Optional ByRef r_vAdditionalDataArray(,) As Object = Nothing, Optional ByVal v_lTransactionType As Integer = 4, Optional ByVal v_bIsBackdatedMTA As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim sFileName As String = ""

        Dim oArray(,) As Object = Nothing
        Dim dCurrentDate As Date

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'For now we're always passing in a blank rule file.
            sFileName = m_sRulePath

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_policy_link_id", vValue:=CStr(oDataSet.PolicyLinkID()), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            dCurrentDate = GetEffectiveDate()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=ToSafeDate(dCurrentDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="type", vValue:="RT", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="quote_type", vValue:=PBCQemQuoteTypeQuote, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRuleFileNameSQL, sSQLName:=ACGetRuleFileNameName, bStoredProcedure:=ACGetRuleFileNameStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=oArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            If oArray(0, 0) IsNot Nothing Then
                sFileName = CStr(oArray(0, 0))
            End If

            m_lReturn = CType(ExecuteRules(lQemQuoteType:=PBQuoteTypeEncode.PBCQemQuoteTypeQuote, oDataSet:=oDataSet, sFileName:=sFileName, sInsurerName:=sInsurerName, InsurerId:=InsurerId, InstanceNo:=InstanceNo, r_vAdditionalDataArray:=r_vAdditionalDataArray, v_bIsBackdatedMTA:=v_bIsBackdatedMTA), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Quote failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Quote", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        Finally
            oArray = Nothing

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: Validate
    '
    ' Description: run the validate rule
    '
    ' History: 12/07/2001 TN - Created.
    '
    ' RVH07082003   : Add additional data array parameter to allow other info
    '                 to be passed down to the script.
    ' ***************************************************************** '
    Public Function Validate(ByRef oDataSet As cGISDataSetControl.Application, Optional ByRef sRuleFile As Object = Nothing, Optional ByRef sInsurerName As Object = Nothing, Optional ByRef InsurerId As Object = Nothing, Optional ByRef InstanceNo As Object = Nothing, Optional ByRef r_vAdditionalDataArray As Object = Nothing) As Integer
        Dim result As Integer = 0
        Dim sFileName As String = ""
        Dim vArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_lGISScreenId = 0 Then
                Return result
            End If

            'For now we're always passing in a blank rule file.
            sFileName = m_sRulePath

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_screen_id", vValue:=CStr(m_lGISScreenId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=PBDatabaseConsts.ACGetAllScreenHeaderSQL, sSQLName:=PBDatabaseConsts.ACGetAllScreenHeaderName, bStoredProcedure:=PBDatabaseConsts.ACGetAllScreenHeaderStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vArray) Then
                'This had better be there...
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If CStr(vArray(PBDatabaseConsts.ACHCode, 0)) = "" Then
                'It's still ok not to have a rule
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sFileName = sFileName & CStr(vArray(PBDatabaseConsts.ACHCode, 0)).Trim() & "Val.Rul"

            vArray = Nothing

            'PN3601 Compiled scripts for validation and defaults cause error unless normal script file also exists
            'Remove check from here. Now only check for files in ExecuteRulesVBScript

            m_lReturn = CType(ExecuteRules(lQemQuoteType:=PBQuoteTypeEncode.PBCQemQuoteTypeValidate, oDataSet:=oDataSet, sFileName:=sFileName, sInsurerName:=sInsurerName, InsurerId:=InsurerId, InstanceNo:=InstanceNo, r_vAdditionalDataArray:=r_vAdditionalDataArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Validate failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Validate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UnderwritingAuthorityLimits
    '
    ' Description: run the UnderwritingAuthorityLimits rule
    '
    ' History: 12/07/2001 TN - Created.
    '
    ' sj16/12/2002  : Add transaction type
    ' RVH07082003   : Add additional data array parameter to allow other info
    '                 to be passed down to the script.
    ' ***************************************************************** '

    Private Function UnderwritingAuthorityLimits(ByRef oDataSet As cGISDataSetControl.Application, Optional ByRef sRuleFile As Object = Nothing, Optional ByRef sInsurerName As Object = Nothing, Optional ByRef InsurerId As Object = Nothing, Optional ByRef InstanceNo As Object = Nothing, Optional ByRef lTransactionType As Object = 1, Optional ByRef r_vAdditionalDataArray As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim sFileName As String = ""
        Dim vArray(,) As Object = Nothing
        Dim dCurrentDate As Date



        result = gPMConstants.PMEReturnCode.PMTrue

        'For now we're always passing in a blank rule file.
        sFileName = m_sRulePath

        'We need to identify the file.  This varies by so many things
        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_policy_link_id", vValue:=CStr(oDataSet.PolicyLinkID()), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'For now hard code this...
        m_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_type_id", vValue:=CStr(lTransactionType), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        dCurrentDate = GetEffectiveDate()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=ToSafeDate(dCurrentDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetUALRuleFileNameSQL, sSQLName:=ACGetUALRuleFileNameName, bStoredProcedure:=ACGetUALRuleFileNameStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Informations.IsArray(vArray) Then
            'It's ok not to have a rule
            '        UnderwritingAuthorityLimits = PMFalse
            Return result
        End If

        If CStr(vArray(0, 0)) = "" Then
            'It's still ok not to have a rule
            Return result
        End If

        sFileName = sFileName & CStr(vArray(0, 0))

        vArray = Nothing

        m_lReturn = CType(ExecuteRules(lQemQuoteType:=PBQuoteTypeEncode.PBCQemQuoteTypeUal, oDataSet:=oDataSet, sFileName:=sFileName, sInsurerName:=sInsurerName, InsurerId:=InsurerId, InstanceNo:=InstanceNo, r_vAdditionalDataArray:=r_vAdditionalDataArray), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: Default
    '
    ' Description: run the default rule
    '
    ' History: 12/07/2001 TN - Created.
    '
    ' AK 150403     : addl. paramamter v_bPreScreen
    ' RVH07082003   : Add additional data array parameter to allow other info
    '                 to be passed down to the script.
    ' ***************************************************************** '
    Public Function Default_Renamed(ByRef oDataSet As cGISDataSetControl.Application, Optional ByRef sRuleFile As Object = Nothing, Optional ByRef sInsurerName As Object = Nothing, Optional ByRef InsurerId As Object = Nothing, Optional ByRef InstanceNo As Object = Nothing, Optional ByRef lQuoteType As Object = Nothing, Optional ByRef r_vAdditionalDataArray As Object = Nothing, Optional ByVal v_bIsBackdatedMTA As Boolean = False) As Integer
        Dim result As Integer = 0
        Dim sFileName As String = ""
        Dim vArray(,) As Object = Nothing

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            sFileName = m_sRulePath

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_screen_id", vValue:=CStr(m_lGISScreenId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=PBDatabaseConsts.ACGetAllScreenHeaderSQL, sSQLName:=PBDatabaseConsts.ACGetAllScreenHeaderName, bStoredProcedure:=PBDatabaseConsts.ACGetAllScreenHeaderStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vArray) Then
                'This had better be there...
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If CStr(vArray(PBDatabaseConsts.ACHCode, 0)) = "" Then
                'It's still ok not to have a rule
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sFileName = sFileName & CStr(vArray(PBDatabaseConsts.ACHCode, 0)).Trim() & "def.Rul"
            vArray = Nothing

            m_lReturn = CType(ExecuteRules(lQemQuoteType:=CInt(lQuoteType), oDataSet:=oDataSet, sFileName:=sFileName, sInsurerName:=sInsurerName, InsurerId:=InsurerId, InstanceNo:=InstanceNo, r_vAdditionalDataArray:=r_vAdditionalDataArray, v_bIsBackdatedMTA:=v_bIsBackdatedMTA), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Default failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Default", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: Renewal
    '
    ' Description: run the renewal rule
    '
    ' History: 12/07/2001 TN - Created.
    '
    ' RVH07082003   : Add additional data array parameter to allow other info
    '                 to be passed down to the script.
    ' ***************************************************************** '
    Public Function Renewal(ByRef oDataSet As cGISDataSetControl.Application, Optional ByRef sRuleFile As Object = Nothing, Optional ByRef sInsurerName As Object = Nothing, Optional ByRef InsurerId As Object = Nothing, Optional ByRef InstanceNo As Object = Nothing, Optional ByRef r_vAdditionalDataArray As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim sFileName As String = ""
        Dim vArray(,) As Object = Nothing
        Dim dCurrentDate As Date

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'For now we're always passing in a blank rule file.
            sFileName = m_sRulePath

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_policy_link_id", vValue:=CStr(oDataSet.PolicyLinkID()), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            dCurrentDate = GetEffectiveDate()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=ToSafeDate(dCurrentDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="type", vValue:="RN", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="quote_type",
                                           vValue:=PBCQemQuoteTypeQuote,
                                           iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                           iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRuleFileNameSQL, sSQLName:=ACGetRuleFileNameName, bStoredProcedure:=ACGetRuleFileNameStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vArray) Then
                'It's ok not to have a rule
                Return result
            End If

            If CStr(vArray(0, 0)) = "" Then
                'It's still ok not to have a rule
                Return result
            End If

            sFileName = sFileName & ToSafeString(vArray(0, 0))
            vArray = Nothing

            m_lReturn = CType(ExecuteRules(lQemQuoteType:=PBQuoteTypeEncode.PBCQemQuoteTypeRenewal, oDataSet:=oDataSet, sFileName:=sFileName, sInsurerName:=sInsurerName, InsurerId:=InsurerId, InstanceNo:=InstanceNo, r_vAdditionalDataArray:=r_vAdditionalDataArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Renewal Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Renewal", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: ExecuteRules
    '
    ' Description: Checks the registry option to determine which type rule to call
    '
    ' History: 13/03/2002 CLG - Created.
    '          22/06/2004 CLG - PN3601 Compiled scripts for validation and defaults cause error unless normal script file also exists
    '          07/08/2003 RVH - add parameter for the additional data array
    '
    ' ***************************************************************** '
    'AK 150403 - an additional paramameter to differentiate between Defaults/PreScreen scripts

    Private Function ExecuteRules(ByVal lQemQuoteType As Integer, ByRef oDataSet As cGISDataSetControl.Application, ByRef sFileName As String, Optional ByRef sInsurerName As Object = Nothing, Optional ByRef InsurerId As Object = Nothing, Optional ByRef InstanceNo As Object = Nothing, Optional ByRef r_vAdditionalDataArray(,) As Object = Nothing, Optional ByVal v_bIsBackdatedMTA As Boolean = False) As Integer
        Dim sQuoteType As String = ""
        Dim sAssemblyClassName As String = ""
        Dim vArray(,) As Object = Nothing

        'get the screen code as we need this for the assembly name
        m_oDatabase.Parameters.Clear()
        m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_screen_id", vValue:=CStr(m_lGISScreenId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=PBDatabaseConsts.ACGetAllScreenHeaderSQL, sSQLName:=PBDatabaseConsts.ACGetAllScreenHeaderName, bStoredProcedure:=PBDatabaseConsts.ACGetAllScreenHeaderStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If (Not Informations.IsNothing(vArray) AndAlso (Informations.IsArray(vArray) AndAlso CStr(vArray(PBDatabaseConsts.ACHCode, 0)) <> "")) Or lQemQuoteType = PBQuoteTypeEncode.PBCQemQuoteTypeRenewal Then
            'sFileName = CStr(vArray(PBDatabaseConsts.ACHCode, 0)).Trim()
        ElseIf lQemQuoteType <> PBCQemQuoteTypeQuote Then
            'can't load the screen code for this code for some reason, so log it
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Could not get screen code for defaults / validation for screen ID : " & m_lGISScreenId,
                                       vApp:=ACApp, vClass:=ACClass, vMethod:="ExecuteRules", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Select Case lQemQuoteType
            Case PBQuoteTypeEncode.PBCQemQuoteTypeQuote
                sQuoteType = "PBCQemQuoteTypeQuote"
                sAssemblyClassName = sFileName
            Case PBQuoteTypeEncode.PBCQemQuoteTypeCopyRisk
                sQuoteType = "PBCQemQuoteTypeCopyRisk"
            Case PBQuoteTypeEncode.PBCQemQuoteTypeValidate
                sQuoteType = "PBCQemQuoteTypeValidate"
                sAssemblyClassName = CStr(vArray(PBDatabaseConsts.ACHCompiledRuleAssemblyValidation, 0)).Trim()
            Case PBQuoteTypeEncode.PBCQemQuoteTypeUal
                sQuoteType = "PBCQemQuoteTypeUal"
            Case PBQuoteTypeEncode.PBCQemQuoteTypeDefault, PBQuoteTypeEncode.PBCQemQuoteTypePreScreen
                sQuoteType = "PBCQemQuoteTypeDefault"
                sAssemblyClassName = CStr(vArray(PBDatabaseConsts.ACHCompiledRuleAssemblyDefaults, 0)).Trim()
            Case PBQuoteTypeEncode.PBCQemQuoteTypeRenewal
                sQuoteType = "PBCQemQuoteTypeRenewal"
                sAssemblyClassName = System.IO.Path.GetFileName(sFileName)
            Case PBQuoteTypeEncode.PBCQemQuoteTypeRenewalLapse
                sQuoteType = "PBCQemQuoteTypeRenewalLapse"
            Case Else
                sQuoteType = "PBCQemQuoteType Unknown"
        End Select

        Return ExecuteRulesCompiled(oDataSet, sAssemblyClassName, sInsurerName, InsurerId, InstanceNo, lQemQuoteType, r_vAdditionalDataArray)

    End Function

    'Ram - Function to return GIS Insurer Id, Name for a Party Cnt No from Party Table of SBO
    '14-07-2000
    Private Function GetGISInsurerDetails(ByRef v_PartyID As String, ByRef r_GISInsurerNumber As Integer, ByRef r_GISInsurerDesc As Object) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue
        ' Clear the Parameters
        m_oDatabase.Parameters.Clear()

        ' Add v_PartyCnt input Parameter

        m_lReturn = m_oDatabase.Parameters.Add(sName:="Party_Id", vValue:=CStr(CInt(v_PartyID)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add GIS Insurer ID output Parameter

        m_lReturn = m_oDatabase.Parameters.Add(sName:="GIS_Insurer_Id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add GIS Insurer Descriptiong output Parameter

        m_lReturn = m_oDatabase.Parameters.Add(sName:="GIS_Insurer_Desc", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Call the SQL
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetGISInsurerIDSQL, sSQLName:=ACGetGISInsurerIDName, bStoredProcedure:=ACGetGISInsurerID, lRecordsAffected:=lRecordsAffected)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Return the GIS Insurer Id
        r_GISInsurerNumber = m_oDatabase.Parameters.Item("GIS_Insurer_Id").Value

        ' Return the GIS Insurer  Desc

        r_GISInsurerDesc = m_oDatabase.Parameters.Item("GIS_Insurer_Desc").Value

        Return result

    End Function

    'Ram-22-05-2000 - Added to Execure Rule for the Document Template
	<HandleProcessCorruptedStateExceptions>
    Public Function MergeDataWithDoc(ByRef r_oXML As Object, ByRef oDocument As Object, ByRef vData As Object, ByRef sInFile As String) As Integer
        Dim result As Integer = 0
        Dim oScriptControl As MSScriptControl.ScriptControl
        Dim sStr As String = ""
        Dim ff As Integer
        Dim sRecord As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue
        oScriptControl = New MSScriptControl.ScriptControl()
        oScriptControl.Language = "VBScript"

        sStr = ""

        ff = FileSystem.FreeFile()
        FileSystem.FileOpen(ff, sInFile, OpenMode.Input)

        ' -----Start
        Dim sTempstr As String = ""
        sTempstr = FileSystem.InputString(ff, FileSystem.LOF(ff))
        sStr = sStr & sTempstr
        ' -----End

        FileSystem.FileClose(ff)
        sStr = sStr.Trim()

        ' Test Output  (to be removed)
        Dim lFileNo As Integer
        lFileNo = FreeFile()
        File.Open(lFileNo, "C:\SETemp2.tmp", OpenMode.Output) ' Create filename.
        File.WriteAllLines(lFileNo, sStr)
        FileClose(lFileNo)

        Try
            oScriptControl.AddCode(sStr)

            'Set the Engine
            oScriptControl.Run("SetEngine", r_oXML)

            'Set the Document Engine
            oScriptControl.Run("SetDocumentEngine", oDocument)

            'Set the Data
            oScriptControl.Run("SetData", vData)

            ' Run the Rule
            oScriptControl.Run("Main")

            'Get the Data back
            oScriptControl.Run("GetData", vData)

            oScriptControl = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            Err_No = oScriptControl.Error.Number
            Err_Line = oScriptControl.Error.Line
            Err_Col = oScriptControl.Error.Column
            Err_Description = oScriptControl.Error.Description

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="VBS MergeData - Engine Failed. Error in Script." & Strings.ChrW(13) & Strings.ChrW(10) & "Error No     : " & CStr(Err_No) & Strings.ChrW(13) & Strings.ChrW(10) & "Error Desc   : " & Err_Description & Strings.ChrW(13) & Strings.ChrW(10) & "Error Line   : " & CStr(Err_Line) & Strings.ChrW(13) & Strings.ChrW(10) & "Error Column : " & CStr(Err_Col) & Strings.ChrW(13) & Strings.ChrW(10), vApp:=ACApp, vClass:=ACClass, vMethod:="MergeDataWithDoc", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

Err_MergeDataWithDoc:

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="VBS MergeData - Engine Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MergeDataWithDoc", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetRuleFileLocation
    '
    ' Description:  Method to Fetch the Rule File Location from the
    '               Registry
    '
    ' Created by :  Ram Chandrbose
    ' Created on :  25-06-2000
    ' ***************************************************************** '
    Public Function GetRuleFileLocation() As String

        Dim result As String = String.Empty
        Dim sRuleFilePath, sSubKey As String

        Try

            sRuleFilePath = ""

            sSubKey = GISSharedConstants.ACOIMGISSubKey & "\" & g_sDataModelCode

            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="RuleFilePath", r_sSettingValue:=sRuleFilePath, v_sSubKey:=sSubKey), gPMConstants.PMEReturnCode)

            Return sRuleFilePath

        Catch excep As System.Exception

            result = ""

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Get Rule File Location Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRuleFileLocation", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Get Insurers
    '
    ' Description:  Method to Fetch the Insurers from the
    '               Registry
    '
    ' Created by :  Ram Chandrbose
    ' Created on :  14-07-2000
    ' ***************************************************************** '
    Private Function GetInsurers(ByRef vInsurer As Object) As String

        Dim result As String = String.Empty
        Dim sSubKey As String = ""
        Dim sInsurers As String = ""

        Dim lStart, lEnd, lLength, lCounter As Integer

        result = CStr(gPMConstants.PMEReturnCode.PMTrue)

        sSubKey = GISSharedConstants.ACOIMGISSubKey & "\" & g_sDataModelCode

        m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="Insurers", r_sSettingValue:=sInsurers, v_sSubKey:=sSubKey), gPMConstants.PMEReturnCode)

        ' Initialise
        sInsurers = sInsurers.Trim()
        lLength = sInsurers.Length
        lStart = 1

        If lLength = 0 Then
            ' No Tasks Available
            vInsurer = Nothing
            result = CStr(gPMConstants.PMEReturnCode.PMFalse)
        Else
            ' if No '+' is Available in the Task then
            lEnd = Informations.inStr(lStart, sInsurers, ",")
            If lEnd < lStart Then
                ' No '+' is found so only one task is available
                ReDim vInsurer(1)
                vInsurer(1) = sInsurers
            ElseIf lEnd > lStart Then
                'We have found more than one task
                lStart = 1
                lCounter = 1
                While lStart <= lLength
                    lEnd = Informations.inStr(lStart, sInsurers, ",")
                    If lEnd > lStart Then
                        ' We found A Task
                        'if we are in the first task only Redim else Redim Preserve
                        If lCounter = 1 Then
                            ReDim vInsurer(1)
                        ElseIf lCounter > 1 Then
                            ReDim Preserve vInsurer(vInsurer.GetUpperBound(0) + 1)
                        End If

                        vInsurer(vInsurer.GetUpperBound(0)) = sInsurers.Substring(lStart - 1, Math.Min(sInsurers.Length, lEnd - lStart))
                        lStart = lEnd + 1
                        lCounter += 1
                    ElseIf lEnd = 0 Then
                        ' We entered the last entry
                        ReDim Preserve vInsurer(vInsurer.GetUpperBound(0) + 1)
                        vInsurer(vInsurer.GetUpperBound(0)) = sInsurers.Substring(lStart - 1)
                        lStart = lLength + 1
                    End If
                End While
            End If

        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SetRuleFileLocation
    '
    ' Description:  Method to Set the Rule File Location In the
    '               Registry
    '
    ' Created by :  Ram Chandrbose
    ' Created on :  25-06-2000
    ' ***************************************************************** '
    Public Function SetRuleFileLocation(ByRef sFilePath As String) As Integer

        Dim result As Integer = 0
        Dim sRuleFilePath, sSubKey As String

        Try

            sRuleFilePath = ""

            'Ram 24-07-2000 (For Multiple Datamodel)
            sSubKey = GISSharedConstants.ACOIMGISSubKey & "\" & g_sDataModelCode

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="RuleFilePath", v_sSettingValue:=sFilePath, v_sSubKey:=sSubKey), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Set Rule File Location Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetRuleFileLocation", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ExecuteRulesCompiled
    '
    ' Description: Runs the compiled version of the rule
    '
    ' History: 13/03/2002 CLG - Created.
    ' RVH 16/9/2003 - Set Extras parameters when in "claim mode"
    ' ***************************************************************** '
    'AK 150403 - another parameter
    Private Function ExecuteRulesCompiled(ByRef oDataSet As cGISDataSetControl.Application, ByVal sAssemblyClassName As String, Optional ByRef sInsurerName As Object = Nothing, Optional ByRef InsurerId As Object = Nothing,
                                          Optional ByRef InstanceNo As Object = Nothing, Optional ByRef lQemQuoteType As Integer = 0, Optional ByRef r_vAdditionalDataArray As Object = Nothing, Optional ByVal v_bIsBackdatedMTA As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim sStr As String = ""
        Dim vArray As Object = Nothing
        Dim oExtras As Object = Nothing
        Dim sTotals As String = ""
        Dim dtCancellationDate As Date
        Dim oRules As Object = Nothing
        Dim lClaimId As Integer

        ' RVH 7/8/2003 : IAG - START: Constants for name/value pairs passed in additional data array
        Const KeyChildOIKey As String = "CHILD_OIKEY"
        Const PosChildOIKey As Integer = 0
        Const PosChildMaxItems As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue

        If sAssemblyClassName = "" Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = CType(gPMComponentServices.CreateBusinessObject(r_oObject:=oExtras, v_sClassName:="bGISPMUExtras.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = oExtras.SetProcessModes(vTask:=ToSafeInteger(m_iTask), vNavigate:=ToSafeInteger(m_lNavigate), vProcessMode:=ToSafeInteger(m_lProcessMode), vTransactionType:=CStr(m_sTransactionType), vEffectiveDate:=ToSafeDate(m_dtEffectiveDate))

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' RVH 7/8/2003 : IAG - START: Check and read passed additional data array
        If Informations.IsArray(r_vAdditionalDataArray) Then
            ReDim vArray(PosChildMaxItems)
            For iLoop As Integer = r_vAdditionalDataArray.GetLowerBound(1) To r_vAdditionalDataArray.GetUpperBound(1)

                Select Case CStr(r_vAdditionalDataArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iLoop)).ToUpper()
                    Case KeyChildOIKey
                        vArray(PosChildOIKey) = r_vAdditionalDataArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop)
                End Select
            Next iLoop
        End If

        'set the extra's properties
        oExtras.GISDataModel = g_sDataModelCode
        oExtras.PolicyLinkID = oDataSet.PolicyLinkID()
        oExtras.DataSet = oDataSet.Risk
        dtCancellationDate = GetEffectiveDate()
        oExtras.CancellationDate = dtCancellationDate

        '   RVH 16/9/2003 - Check to see if this is a claims dataset and if
        '                   so, set the Extras object so that it knows to use
        '                   the "work" tables for tasks and events...any tasks
        '                   or events created through VBScript as part of the
        '                   claims roadmap will be copied from these work
        '                   tables to the live tables at the end of the claim
        '                   roadmap.
        m_lReturn = CType(CheckIfClaim(oDataSet, lClaimId), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMFalse Then
            oExtras.TaskEventUseWorkTables = gPMConstants.PMEReturnCode.PMTrue
            oExtras.CallingProcessKeyId = lClaimId
            oExtras.CallingProcessType = 1
        End If

        Try

            oRules = CreateLateBoundObject_CompiledRules(sAssemblyClassName)

            If Not (oRules Is Nothing) Then

                Try
                    oRules.SetDefaultValue(CStr(m_sTransactionType), r_vAdditionalDataArray, CBool(v_bIsBackdatedMTA))
                Catch ex As MissingMemberException
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="VBS ExecuteRulesCompiled - SetDefaultValue method does not exist: " & sAssemblyClassName,
                                       vApp:=ACApp, vClass:=ACClass, vMethod:="ExecuteRulesCompiled", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                End Try

                Try
                    oRules.SetAll(oDataSet.Risk, DirectCast(oDataSet, cGISDataSetControl.Application), oExtras)
                Catch ex As MissingMemberException
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="VBS ExecuteRulesCompiled - SetAll method does not exist: " & sAssemblyClassName,
                                       vApp:=ACApp, vClass:=ACClass, vMethod:="ExecuteRulesCompiled", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                End Try

                Try
                    Select Case lQemQuoteType
                        Case PBQuoteTypeEncode.PBCQemQuoteTypeDefault, PBQuoteTypeEncode.PBCQemQuoteTypePreScreen
                            oRules.Def()
                        Case PBQuoteTypeEncode.PBCQemQuoteTypeValidate
                            oRules.Val()
                        Case PBQuoteTypeEncode.PBCQemQuoteTypeQuote, PBQuoteTypeEncode.PBCQemQuoteTypeRenewal
                            oRules.Quote()
                        Case PBQuoteTypeEncode.PBCQemQuoteTypeCopyRisk
                            oRules.CopyRisk()
                        Case PBQuoteTypeEncode.PBCQemQuoteTypeUal
                            oRules.UAL()
                        Case PBQuoteTypeEncode.PBCQemQuoteTypeRenewalLapse
                            oRules.RenewalLapse()
                    End Select
                Catch ex As MissingMemberException
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="VBS ExecuteRulesCompiled " + lQemQuoteType + " method does not exist: " & sAssemblyClassName,
                                       vApp:=ACApp, vClass:=ACClass, vMethod:="ExecuteRulesCompiled", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Finally
                    'ToDo: why does this line throw and invalidcastexception?????
                    'oRules = Nothing
                End Try

            End If

            oExtras = Nothing
            Return result

        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="VBS ExecuteRulesCompiled - Engine Failed: " & sAssemblyClassName, vApp:=ACApp, vClass:=ACClass, vMethod:="ExecuteRulesCompiled", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            oRules = Nothing
            oExtras = Nothing
            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name          : GetEffectiveDate
    '
    ' Description   : Function to get the effecetive date
    '
    ' Edit History  :

    ' ***************************************************************** '
    Private Function GetEffectiveDate() As Date

        Try

            Dim dCurrentDate As Date

            If Informations.IsDate(m_dtGISEffectiveDate) Then
                dCurrentDate = m_dtGISEffectiveDate
            Else
                ' Check if we have the value in the Property Manager
                ' and set the module level variable for future use
                m_lReturn = CType(gPMComponentServices.GetUserProperty(v_sUsername:=m_sUsername, v_sPropertyName:="effective_date", r_vPropertyValue:=m_dtGISEffectiveDate), gPMConstants.PMEReturnCode)

                ' If the value is not there or if the value is not a valid date, then
                ' default to current date
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue And Informations.IsDate(m_dtGISEffectiveDate) Then
                    ' Do nothing
                Else
                    m_dtGISEffectiveDate = DateTime.Now
                End If

                dCurrentDate = m_dtGISEffectiveDate
            End If

            'Return the Effective Date

            Return dCurrentDate

        Catch
            Return DateTime.Now
        End Try

    End Function

    ' ***************************************************************** '
    '
    ' Name          : CheckIfClaim
    '
    ' Description   : Function to check to see if the current datamodel
    '                 is for a claim - this is done by looking for the
    '                 "work_claim" element, which would not be present
    '                 for a risk datamodel.
    '
    ' Edit History  :
    ' RVH 16/09/2003: Created
    ' ***************************************************************** '
    Private Function CheckIfClaim(ByVal oDataSet As cGISDataSetControl.Application, ByRef v_lClaimID As Integer) As Integer
        Dim result As Integer = 0
        Try

            Dim vKeyArray As Object = Nothing
            Dim lReturn As gPMConstants.PMEReturnCode
            Dim sClaimKey As String = ""
            Dim vData As Object = Nothing
            Dim bIsAssumedInfo As Boolean
            Dim sXMLDef As String = ""
            Dim sXMLData As String = ""

            result = True

            If oDataSet Is Nothing Then
                Return False
            End If

            '   RVH 20/04/2004: Need to do a "pre-check" to see if the "work_claim" object exists
            '                   in the XML definition. Otherwise, expecting GetAllOIKey to do the check can
            '                   result in an error being written to the log if the object does not
            '                   exist.
            lReturn = oDataSet.ReturnAsXML(sXMLDef, sXMLData)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If (sXMLDef.ToUpper().IndexOf("<WORK_CLAIM ") + 1) <= 0 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '   Return all OIKEYS from the Work_Claim object

            lReturn = oDataSet.GetAllOIKey("work_claim", vKeyArray)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '   Get the OI key for the "work_claim" item

            sClaimKey = CStr(vKeyArray(0))

            '   Retrieve the "claim_id"

            lReturn = oDataSet.GetPropertyValue(v_sObjectName:="work_claim", v_sPropertyName:="Claim_Id", v_sOIKey:=sClaimKey, r_vPropertyValue:=CStr(vData), r_bIsAssumedInfo:=bIsAssumedInfo)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '   Return claim id to calling function

            v_lClaimID = CInt(vData)

            Return result

        Catch
            Return False
        End Try

    End Function
    Public Function RenewalLapse(ByRef oDataSet As cGISDataSetControl.Application, Optional ByRef sRuleFile As Object = Nothing, Optional ByRef sInsurerName As Object = Nothing, Optional ByRef InsurerId As Object = Nothing, Optional ByRef InstanceNo As Object = Nothing, Optional ByRef r_vAdditionalDataArray As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim sFileName As String = ""
        Dim vArray(,) As Object = Nothing
        Dim dCurrentDate As Date '  Variable to hold Effective Date 

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'For now we're always passing in a blank rule file.
            sFileName = m_sRulePath

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_policy_link_id", vValue:=CStr(oDataSet.PolicyLinkID()), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            dCurrentDate = GetEffectiveDate()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=ToSafeDate(dCurrentDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="type", vValue:="RL", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="quote_type",
                                           vValue:=PBCQemQuoteTypeQuote,
                                           iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                           iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRuleFileNameSQL, sSQLName:=ACGetRuleFileNameName, bStoredProcedure:=ACGetRuleFileNameStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vArray) Then
                'It's ok not to have a rule
                Return result
            End If

            If CStr(vArray(0, 0)) = "" Then
                'It's still ok not to have a rule
                Return result
            End If

            sFileName = sFileName & CStr(vArray(0, 0))

            vArray = Nothing

            m_lReturn = CType(ExecuteRules(lQemQuoteType:=PBQuoteTypeEncode.PBCQemQuoteTypeRenewalLapse, oDataSet:=oDataSet, sFileName:=sFileName, sInsurerName:=sInsurerName, InsurerId:=InsurerId, InstanceNo:=InstanceNo, r_vAdditionalDataArray:=r_vAdditionalDataArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Renewal Lapsed Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Renewal", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

End Class

