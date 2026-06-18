Option Strict Off
Option Explicit On
Imports System.IO
Imports System.Reflection
Imports System.Text
Imports SSP.Shared
Imports Microsoft.VisualBasic
Imports SharedQuoteEngine
Imports System.Runtime.ExceptionServices


<System.Runtime.InteropServices.ProgId("VBS_NET.VBS")>
Public NotInheritable Class VBS
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

    Private Const ACClass As String = "VBS"

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
    'Private m_sCompiledScript As String = ""
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
                oUserAuthority.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
                m_lReturn = CType(gPMComponentServices.CreateBusinessObject(r_oObject:=oUserAuthority, v_sClassName:="bACTUserAuthorities.Business", v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iUserID:=m_iUserID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise bACTUserAuthorities.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'UPGRADE_TODO: (1067) Member GetDetails is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
                m_lReturn = oUserAuthority.GetDetails(vUserID:=m_iUserID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise bACTUserAuthorities.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'UPGRADE_TODO: (1067) Member GetNext is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
                m_lReturn = oUserAuthority.GetNext(vParams:=vParams)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the details for the User Authority", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If gPMFunctions.ToSafeInteger(vParams(ACUserServerScriptsRunInDebug), 0) = 1 Then
                    m_bServerDebuggingEnabled = True
                End If

                'UPGRADE_TODO: (1067) Member Terminate is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
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

            'm_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="CompiledSCript", r_sSettingValue:=m_sCompiledScript, v_sSubKey:=sSubKey), gPMConstants.PMEReturnCode)

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
            ' Log Error Message
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
        Dim sDataModelCode As String = String.Empty, sBusinessType As String
        Dim dCurrentDate As Date
        Dim iActive As Integer

        Dim kSInsurerName As String = ""
        Dim kIInsurerNumber As Integer = 0
        Dim kGISInsurerId As Integer = 0

        Dim m_vInsurerDesc As Object = Nothing
        Dim vInsurerArray As Object = Nothing
        Dim sXMLDataSetDef As String = ""

        Dim vPartyId As String = ""
        Dim lQuoteType, lTransactionType As Integer

        Dim sSQL As String = ""
        Dim vArray(,) As Object = Nothing

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            If Not v_bAfterPRETriggerRules Then
                ' Clear any existing Quote Output
                lReturn = r_oDataset.ClearAllQuoteOutput()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If
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

            ' Get the GIS Insurer Name since we had the Polaris Insurer No only
            lReturn = CType(GetInsurers(vInsurerArray), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Getting Insurers Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NBQuote", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            If Informations.IsArray(vInsurerArray) Then

                vPartyId = CStr(vInsurerArray(1))
                lReturn = GetGISInsurerDetails(vPartyId, kGISInsurerId, m_vInsurerDesc)

                If lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    kIInsurerNumber = kGISInsurerId

                    kSInsurerName = CStr(m_vInsurerDesc)
                Else
                    ' Invalid Insurer
                    kIInsurerNumber = 9999
                    kSInsurerName = "Unknown Insurer"
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
                    Case PBQuoteTypeEncode.PBCQemQuoteTypeRenewalLapse
                        lReturn = CType(RenewalLapse(oDataSet:=r_oDataset, sRuleFile:=sAppRuleFile, sInsurerName:=kSInsurerName, InsurerId:=kIInsurerNumber, InstanceNo:=1, r_vAdditionalDataArray:=r_vAdditionalDataArray), gPMConstants.PMEReturnCode)
                    Case PBQuoteTypeEncode.PBCQemQuoteTypePreScreen, PBQuoteTypeEncode.PBCQemQuoteTypeDefault, PBQuoteTypeEncode.PBCQemQuoteTypeCopyRisk
                        ' Call the Default Method
                        lReturn = CType(Default_Renamed(oDataSet:=r_oDataset, sRuleFile:=sAppRuleFile, sInsurerName:=kSInsurerName, InsurerId:=kIInsurerNumber, InstanceNo:=1, lQuoteType:=lQuoteType, r_vAdditionalDataArray:=r_vAdditionalDataArray, v_bIsBackdatedMTA:=v_bIsBackdatedMTA), gPMConstants.PMEReturnCode)
                    Case Else
                        ' Call the Quote Method
                        lReturn = CType(Quote(oDataSet:=r_oDataset, sRuleFile:=sAppRuleFile, sInsurerName:=kSInsurerName, InsurerId:=kIInsurerNumber, InstanceNo:=1, v_lTransactionType:=lTransactionType, r_vAdditionalDataArray:=r_vAdditionalDataArray, v_bIsBackdatedMTA:=v_bIsBackdatedMTA), gPMConstants.PMEReturnCode)
                End Select

            End If

            Return lReturn

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NBQuoteFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="NBQuote", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try

    End Function

    Public Function IsToQuoteForTest() As Boolean
        Dim result As Boolean = False
        Dim sValue As String = String.Empty, sSubKey As String = String.Empty

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

    Public Sub New()
        MyBase.New()

        Try
            ' Ram-01-03-2000
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
        Dim vData As Object = Nothing, vKeyArray As Object = Nothing
        Dim sKey As String = ""

        ' These are keys to SINGLE instances
        Dim sPolicyBinderKey, sPolicyKey, sVehicleKey, sProposerKey, sNCDKey As String

        ' These are keys to multiple instances of a GIS object
        Dim vDriverKey, vClaimKey, vConvictionKey As Object

        Dim sTopLevelObject As String = String.Empty, sTopLevelTable As String = String.Empty
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


        vData = CStr((CDate(vData)).Day) & "/" & CStr(CDate(vData).Month) & "/" & CStr(CDate(vData).Year + 1)


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
    Public Function Quote(ByRef oDataSet As cGISDataSetControl.Application, Optional ByRef sRuleFile As Object = Nothing, Optional ByRef sInsurerName As Object = Nothing, Optional ByRef InsurerId As Object = Nothing, Optional ByRef InstanceNo As Object = Nothing, Optional ByRef r_vAdditionalDataArray As Object = Nothing, Optional ByVal v_lTransactionType As Integer = 4, Optional ByVal v_bIsBackdatedMTA As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim sFileName As String = ""
        Dim vArray(,) As Object = Nothing
        Dim dCurrentDate As Date ' RAM20030114 : Variable to hold Effective Date

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
            ' RAM20030131 : Check for an Effective Date - Start
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            dCurrentDate = GetEffectiveDate()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=dCurrentDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20030131 : Check for an Effective Date - End
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="type", vValue:="RT", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

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
                '        Quote = PMFalse
                Return result
            End If

            If CStr(vArray(0, 0)) = "" Then
                'It's still ok not to have a rule
                Return result
            End If

            sFileName = sFileName & CStr(vArray(0, 0))
            vArray = Nothing
            m_lReturn = CType(ExecuteRules(lQemQuoteType:=PBQuoteTypeEncode.PBCQemQuoteTypeQuote, oDataSet:=oDataSet, sFileName:=sFileName, sInsurerName:=sInsurerName, InsurerId:=InsurerId, InstanceNo:=InstanceNo, r_vAdditionalDataArray:=r_vAdditionalDataArray, v_bIsBackdatedMTA:=v_bIsBackdatedMTA), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Quote failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Quote", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
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
            ' Log Error Message
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
        Dim dCurrentDate As Date ' RAM20030114 : Variable to hold Effective Date



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
        ' RAM20030131 : Check for an Effective Date - Start
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        dCurrentDate = GetEffectiveDate()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=dCurrentDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' RAM20030131 : Check for an Effective Date - End
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
        Dim dCurrentDate As Date ' RAM20030114 : Variable to hold Effective Date

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
            ' RAM20030131 : Check for an Effective Date - Start
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            dCurrentDate = GetEffectiveDate()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=dCurrentDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20030131 : Check for an Effective Date - End
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


            sFileName = sFileName & CStr(vArray(0, 0))


            vArray = Nothing


            m_lReturn = CType(ExecuteRules(lQemQuoteType:=PBQuoteTypeEncode.PBCQemQuoteTypeRenewal, oDataSet:=oDataSet, sFileName:=sFileName, sInsurerName:=sInsurerName, InsurerId:=InsurerId, InstanceNo:=InstanceNo, r_vAdditionalDataArray:=r_vAdditionalDataArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
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

        Select Case lQemQuoteType
            Case PBQuoteTypeEncode.PBCQemQuoteTypeQuote
                sQuoteType = "PBCQemQuoteTypeQuote"
            Case PBQuoteTypeEncode.PBCQemQuoteTypeCopyRisk
                sQuoteType = "PBCQemQuoteTypeCopyRisk"
            Case PBQuoteTypeEncode.PBCQemQuoteTypeValidate
                sQuoteType = "PBCQemQuoteTypeValidate"
            Case PBQuoteTypeEncode.PBCQemQuoteTypeUal
                sQuoteType = "PBCQemQuoteTypeUal"
            Case PBQuoteTypeEncode.PBCQemQuoteTypeDefault
                sQuoteType = "PBCQemQuoteTypeDefault"
            Case PBQuoteTypeEncode.PBCQemQuoteTypeRenewal
                sQuoteType = "PBCQemQuoteTypeRenewal"
            Case PBQuoteTypeEncode.PBCQemQuoteTypePreScreen
                sQuoteType = "PBCQemQuoteTypePreScreen"
            Case PBQuoteTypeEncode.PBCQemQuoteTypeRenewalLapse
                sQuoteType = "PBCQemQuoteTypeRenewalLapse"
            Case Else
                sQuoteType = "PBCQemQuoteType Unknown"
        End Select

        Return ExecuteRulesVBScript(oDataSet, sFileName, sInsurerName, InsurerId, InstanceNo, lQemQuoteType, r_vAdditionalDataArray, v_bIsBackdatedMTA)

    End Function
    ''' <summary>
    ''' Readfile
    ''' </summary>
    ''' <param name="sFileName"></param>
    ''' <param name="sFileText"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ReadFile(ByVal sFileName As String, ByRef sFileText As String) As Integer
        Dim result As Integer = 1


        Using fs As New FileStream(sFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
            Using sr As New StreamReader(fs)
                sFileText = sr.ReadToEnd
            End Using
        End Using
        Return result
    End Function

     Private Function ExecuteRulesVBScript(ByRef oDataSet As cGISDataSetControl.Application, ByRef sFileName As String, Optional ByRef sInsurerName As Object = Nothing, Optional ByRef InsurerId As Object = Nothing, Optional ByRef InstanceNo As Object = Nothing, Optional ByRef lQemQuoteType As Integer = 0, Optional ByRef r_vAdditionalDataArray(,) As Object = Nothing, Optional ByVal v_bIsBackdatedMTA As Boolean = False) As Integer
        Const kKeyChildOIKey As String = "CHILD_OIKEY"
        Const kPosChildOIKey As Integer = 0
        Const kPosChildMaxItems As Integer = 0
        Const kCNCalledFromSTS As String = "CALLEDFROMSTS"

        Dim nResult As Integer = 0
        Dim oScriptControl As MSScriptControl.ScriptControl
        Dim sStr As New StringBuilder
        Dim vArray As Object = Nothing
        Dim oExtras As bGISPMUExtras.Business = Nothing

        Dim sTempstr As String = ""
        Dim dtCancellationDate As Date ' RAM20030205 : Added this variable
        Dim bIscalledViaSAM As Boolean = False
        Dim vbScriptCode As String = String.Empty
        Dim scriptExecutor As New VBQuoteEngine()
        nResult = gPMConstants.PMEReturnCode.PMTrue

        If Not IO.File.Exists(sFileName) Then
            Return nResult
        End If
        nResult = ReadFile(sFileName, sTempstr)

        'Ah, but what if it's empty?
        If sTempstr.Trim() = "" Then
            Return nResult
        End If

#If quoteTiming Then

			PerformanceCtr(cntrCntr, 0) = "Create bGISPMUExtras"
			QueryPerformanceCounter PerformanceCtr(cntrCntr, 1): cntrCntr = cntrCntr + 1
#End If

        oExtras = New bGISPMUExtras.Business
        m_lReturn = CType(oExtras.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

#If quoteTiming Then

			QueryPerformanceCounter PerformanceCtr(cntrCntr, 1): cntrCntr = cntrCntr + 1
#End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = oExtras.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        ' RVH 7/8/2003 : IAG - START: Check and read passed additional data array
        If Informations.IsArray(r_vAdditionalDataArray) Then
            ReDim vArray(kPosChildMaxItems)
            For iLoop As Integer = r_vAdditionalDataArray.GetLowerBound(1) To r_vAdditionalDataArray.GetUpperBound(1)

                Select Case CStr(r_vAdditionalDataArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iLoop)).ToUpper()
                    Case kKeyChildOIKey
                        vArray.SetValue(r_vAdditionalDataArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop), kPosChildOIKey)
                    Case kCNCalledFromSTS
                        bIscalledViaSAM = True ' PASS IT TRUE 
                End Select
            Next iLoop
        End If

        oExtras.GISDataModel = g_sDataModelCode
        oExtras.PolicyLinkId = oDataSet.PolicyLinkID()
        oExtras.DataSet = oDataSet.Risk
        dtCancellationDate = GetEffectiveDate()
        oExtras.CancellationDate = dtCancellationDate
        oExtras.IsCalledViaSAM = bIscalledViaSAM

#If quoteTiming Then

			PerformanceCtr(cntrCntr, 0) = "Create ScriptControl"
			QueryPerformanceCounter PerformanceCtr(cntrCntr, 1): cntrCntr = cntrCntr + 1
#End If

        Dim sScript As StringBuilder = New StringBuilder("")
        'create the script global definitions
        sStr.AppendLine("Option Explicit")
        sStr.AppendLine("Dim lReturn")
        sStr.AppendLine("Dim TransactionType")
        sStr.AppendLine("Dim vAdditionalData")
        sStr.AppendLine("Dim bIsBackdatedMTA")
        sStr.AppendLine("Const cAdditionalData_OIKey = 0")
        sStr.AppendLine(GetPublicConstantDeclarationScript(GetType(bGISPMUExtras.Business)))
        sStr.AppendLine(sTempstr)
        sStr.AppendLine("Sub SetDefaultValue(sTransactionType, vArray,v_bIsBackdatedMTA)")
        sStr.AppendLine("    TransactionType = sTransactionType")
        sStr.AppendLine("    vAdditionalData = vArray")
        sStr.AppendLine("    bIsBackdatedMTA = v_bIsBackdatedMTA")
        sStr.AppendLine("End Sub")

        
#If quoteTiming Then

			PerformanceCtr(cntrCntr, 0) = "Add code" & vbTab & vbTab
			QueryPerformanceCounter PerformanceCtr(cntrCntr, 1): cntrCntr = cntrCntr + 1
#End If
        If m_bServerDebuggingEnabled Then
            Select Case lQemQuoteType
                Case PBQuoteTypeEncode.PBCQemQuoteTypeDefault, PBQuoteTypeEncode.PBCQemQuoteTypeQuote, PBQuoteTypeEncode.PBCQemQuoteTypeValidate, PBQuoteTypeEncode.PBCQemQuoteTypeRenewal, PBQuoteTypeEncode.PBCQemQuoteTypeUal
                    sStr.Replace("Sub Start()" & Strings.ChrW(13) & Strings.ChrW(10), "Sub Start()" & Strings.ChrW(13) & Strings.ChrW(10) & "Stop" & Strings.ChrW(13) & Strings.ChrW(10))
                Case PBQuoteTypeEncode.PBCQemQuoteTypePreScreen
                    sStr.Replace("Sub Main()" & Strings.ChrW(13) & Strings.ChrW(10), "Sub Main()" & Strings.ChrW(13) & Strings.ChrW(10) & "Stop" & Strings.ChrW(13) & Strings.ChrW(10))
                Case PBQuoteTypeEncode.PBCQemQuoteTypeCopyRisk
                    sStr.Replace("Sub CopyRisk()" & Strings.ChrW(13) & Strings.ChrW(10), "Sub CopyRisk()" & Strings.ChrW(13) & Strings.ChrW(10) & "Stop" & Strings.ChrW(13) & Strings.ChrW(10))
            End Select
        End If

       
        
#If quoteTiming Then

			QueryPerformanceCounter PerformanceCtr(cntrCntr, 1): cntrCntr = cntrCntr + 1
			PerformanceCtr(cntrCntr, 0) = "Run SetDefaultValue code"
			QueryPerformanceCounter PerformanceCtr(cntrCntr, 1): cntrCntr = cntrCntr + 1
#End If
       
#If quoteTiming Then
            QueryPerformanceCounter PerformanceCtr(cntrCntr, 1): cntrCntr = cntrCntr + 1
#End If

        ' Run the Rule
#If quoteTiming Then

			PerformanceCtr(cntrCntr, 0) = "Run Start" & vbTab & vbTab
			QueryPerformanceCounter PerformanceCtr(cntrCntr, 1): cntrCntr = cntrCntr + 1
#End If
        vbScriptCode=sStr.ToString()
        
        Try

            Select Case lQemQuoteType
                Case PBQuoteTypeEncode.PBCQemQuoteTypeDefault, PBQuoteTypeEncode.PBCQemQuoteTypeQuote, PBQuoteTypeEncode.PBCQemQuoteTypeValidate, PBQuoteTypeEncode.PBCQemQuoteTypeRenewal, PBQuoteTypeEncode.PBCQemQuoteTypeUal, PBQuoteTypeEncode.PBCQemQuoteTypeRenewalLapse
                    scriptExecutor.ExecuteVBScript(vbScriptCode, "Start", oDataSet, oExtras, m_sTransactionType, vArray, v_bIsBackdatedMTA)
                Case PBQuoteTypeEncode.PBCQemQuoteTypePreScreen
                   scriptExecutor.ExecuteVBScript(vbScriptCode, "Main", oDataSet, oExtras, m_sTransactionType, vArray, v_bIsBackdatedMTA)
                Case PBQuoteTypeEncode.PBCQemQuoteTypeCopyRisk
                    scriptExecutor.ExecuteVBScript(vbScriptCode, "CopyRisk", oDataSet, oExtras, m_sTransactionType, vArray, v_bIsBackdatedMTA)
                    
            End Select
        Catch
            If Informations.Err().Number = 438 Then
            ElseIf Informations.Err().Number <> 0 Then
                GoTo Err_Error_In_Script
            End If
        End Try
         sStr = Nothing
#If quoteTiming Then

			QueryPerformanceCounter PerformanceCtr(cntrCntr, 1): cntrCntr = cntrCntr + 1
#End If

#If quoteTiming Then

			PerformanceCtr(cntrCntr, 0) = "Set scriptcontrol nothing    "
			QueryPerformanceCounter PerformanceCtr(cntrCntr, 1): cntrCntr = cntrCntr + 1
#End If
        oScriptControl = Nothing
#If quoteTiming Then

			QueryPerformanceCounter PerformanceCtr(cntrCntr, 1): cntrCntr = cntrCntr + 1
			sTotals = sFileName & vbCrLf
			cTotal = 0
			For iLoop = 0 To cntrCntr - 1 Step 2
			sTotals = sTotals & Format(iLoop / 2 + 1, "00") & vbTab & PerformanceCtr(iLoop, 0) & vbTab & Format((PerformanceCtr(iLoop + 1, 1) - PerformanceCtr(iLoop, 1)) / Freq, "0.0000") & vbTab & Format((PerformanceCtr(iLoop + 1, 1) - PerformanceCtr(0, 1)) / Freq, "0.0000") & vbCrLf
			Next
			MsgBox sTotals
#End If
        oExtras.Dispose()
        oExtras = Nothing

        Return nResult

Err_Error_In_Script:
        nResult = gPMConstants.PMEReturnCode.PMError

        ' Ram - 08-03-2000 (To Trap the error)

        'Err_No = oScriptControl.Error.Number
        'Err_Line = oScriptControl.Error.Line
        'Err_Col = oScriptControl.Error.Column
        'Err_Description = oScriptControl.Error.Description
        'Err_Text = oScriptControl.Error.Text

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="VBS ExecuteRules - Engine Failed. Error Running Rule." & Strings.ChrW(13) & Strings.ChrW(10) & "File         : " & sFileName & Strings.ChrW(13) & Strings.ChrW(10) & "Error No     : " & CStr(Err_No) & Strings.ChrW(13) & Strings.ChrW(10) & "Error Desc   : " & Err_Description & Strings.ChrW(13) & Strings.ChrW(10) & "Error Line   : " & CStr(Err_Line) & Strings.ChrW(13) & Strings.ChrW(10) & "Error Column : " & CStr(Err_Col) & Strings.ChrW(13) & Strings.ChrW(10) & "Error Text   : " & Err_Text & Strings.ChrW(13) & Strings.ChrW(10), vApp:=ACApp, vClass:=ACClass, vMethod:="ExecuteRules", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        oScriptControl = Nothing

        oExtras = Nothing

        Return nResult

Err_ExecuteRules:

        'No file, not an error...
        If Informations.Err().Number = 53 Then
            Return nResult
        End If

        nResult = gPMConstants.PMEReturnCode.PMError
        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="VBS ExecuteRulesVBScript - Engine Failed: " & sFileName, vApp:=ACApp, vClass:=ACClass, vMethod:="ExecuteRulesVBScript", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        'oScriptControl = Nothing

        oExtras = Nothing

        Return nResult

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
        ' RAM20030926 : Changed CInt to CLng. Noticed by Jes Westerman.
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
        Dim sStr As String = String.Empty
        Dim ff As Integer
        Dim sRecord As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue
        oScriptControl = New MSScriptControl.ScriptControl()
        oScriptControl.Language = "VBScript"

        sStr = ""

        'ff = FreeFile()
        'File.Open(ff, sInFile, OpenMode.Input)
        ff = FileSystem.FreeFile()
        FileSystem.FileOpen(ff, sInFile, OpenMode.Input)
        ' Ram 23-06-2000 (To Speed up the process. Commented above Lines)
        ' -----Start
        Dim sTempstr As String = ""
        'sTempstr = InputString(ff, LOF(ff), sInFile)
        sTempstr = FileSystem.InputString(ff, FileSystem.LOF(ff))
        sStr = sStr & sTempstr
        ' -----End

        'FileClose(ff)
        FileSystem.FileClose(ff)
        sStr = sStr.Trim()

        ' Test Output  (to be removed)
        Dim lFileNo As Integer
        'lFileNo = FreeFile()
        'File.Open(lFileNo, "C:\SETemp2.tmp", OpenMode.Output) ' Create filename.
        'File.WriteAllLines(lFileNo, sStr)
        'FileClose(lFileNo)
        lFileNo = FileSystem.FreeFile()
        FileSystem.FileOpen(lFileNo, "C:\SETemp2.tmp", OpenMode.Output) ' Create filename.
        FileSystem.WriteLine(lFileNo, sStr)
        FileSystem.FileClose(lFileNo)
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

            ' Ram (03-07-200).
            oScriptControl = Nothing

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Ram - 08-03-2000 (To Trap the error)
            Err_No = oScriptControl.Error.Number
            Err_Line = oScriptControl.Error.Line
            Err_Col = oScriptControl.Error.Column
            Err_Description = oScriptControl.Error.Description

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="VBS MergeData - Engine Failed. Error in Script." & Strings.ChrW(13) & Strings.ChrW(10) & "Error No     : " & CStr(Err_No) & Strings.ChrW(13) & Strings.ChrW(10) & "Error Desc   : " & Err_Description & Strings.ChrW(13) & Strings.ChrW(10) & "Error Line   : " & CStr(Err_Line) & Strings.ChrW(13) & Strings.ChrW(10) & "Error Column : " & CStr(Err_Col) & Strings.ChrW(13) & Strings.ChrW(10), vApp:=ACApp, vClass:=ACClass, vMethod:="MergeDataWithDoc", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            ' Ram (03-07-200).

            Return result

Err_MergeDataWithDoc:

            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
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

            ' Ram - 24-07-2000 (For Multiple DataModel)
            sSubKey = GISSharedConstants.ACOIMGISSubKey & "\" & g_sDataModelCode


            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="RuleFilePath", r_sSettingValue:=sRuleFilePath, v_sSubKey:=sSubKey), gPMConstants.PMEReturnCode)




            Return sRuleFilePath

        Catch excep As System.Exception


            result = ""

            ' Log Error Message
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
        Dim sSubKey As String = String.Empty, sInsurers As String = String.Empty

        Dim lStart, lEnd, lLength, lCounter As Integer

        result = CStr(gPMConstants.PMEReturnCode.PMTrue)

        ' Ram 24-07-2000 (For Multiple Datamodel)
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
            lEnd = sInsurers.IndexOf(",", lStart)
            If lEnd < lStart Then
                ' No '+' is found so only one task is available
                ReDim vInsurer(1)

                vInsurer(1) = sInsurers
            ElseIf lEnd > lStart Then
                'We have found more than one task
                lStart = 1
                lCounter = 1
                While lStart <= lLength
                    lEnd = sInsurers.IndexOf(",", lStart) + 1
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

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Set Rule File Location Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetRuleFileLocation", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
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
    ' RAM20030131   : Created
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
            Dim sXMLDef As String = String.Empty, sXMLData As String = String.Empty

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
            ' RAM20030131 : Check for an Effective Date - Start
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            dCurrentDate = GetEffectiveDate()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=dCurrentDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20030131 : Check for an Effective Date - End
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

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Renewal Lapsed Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Renewal", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    Function GetPublicConstantDeclarationScript(ByVal type As System.Type) As String
        Const kMethodName As String = "GetPublicConstantDeclarationScript"

        Dim sDeclarationScript As New StringBuilder
        Try

            Dim afiConstants As FieldInfo() = type.GetFields(BindingFlags.[Public] Or
                BindingFlags.[Static] Or BindingFlags.FlattenHierarchy)

            For Each fiConstant As FieldInfo In afiConstants
                If fiConstant.IsLiteral AndAlso Not fiConstant.IsInitOnly Then
                    sDeclarationScript.AppendLine("Public Const " & fiConstant.Name & " = " &
                                                If(fiConstant.FieldType.Name = "String",
                                                """" & fiConstant.GetValue(Nothing).ToString & """",
                                                fiConstant.GetValue(Nothing).ToString))

                End If
            Next
            Return sDeclarationScript.ToString
        Catch ex As Exception
            bPMFunc.LogMessage(sUsername:=m_sUsername,
                                  iType:=gPMConstants.PMELogLevel.PMLogError,
                                  sMsg:="Method Failed!", vClass:=ACClass,
                                  vMethod:=kMethodName,
                                  excep:=ex)
            Return String.Empty
        Finally
            sDeclarationScript = Nothing
        End Try

    End Function
End Class

