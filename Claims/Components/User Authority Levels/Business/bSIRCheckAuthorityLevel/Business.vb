Option Strict Off
Option Explicit On
Imports System.IO
Imports SSP.Shared
Imports SharedQuoteEngine
'Developer Guide No. 129
<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable

    ' ***************************************************************** '
    ' Module Name: Business
    '
    ' Date:  15-11-2002
    '
    ' Description: Business Object (Generic Interface to Rules)
    '
    ' ***************************************************************** '

    ' ************************************************
    ' Added to replace global variables 03/02/2004
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************


    Private Const ACClass As String = "Business"

    Private m_lAuthError As Integer ' Script Error Value
    Private m_lAuthUserID As Integer ' User who performed original action
    Private m_lCurrentUserID As Integer ' User Performing Operation
    Private m_crPaymentAmount As Double ' Amount of Payment to be authorised
    Private m_lPaymentType As Integer ' Type of Payment
    Private m_lProductID As Integer ' Product Id
    Private m_lReference As Integer ' Reference Id (Claim Id)
    Private m_lTransType As Integer ' Transaction Type
    Private m_lPaymentCurrencyID As Integer ' Payment Currency
    Private m_crOriginalCurrencyAmount As Double 'Amount of Payment to be authorised in Payment Currency
    Private m_lOriginalPaymentCurrencyID As Integer 'Payment Currency ISO Code of Amount
    Private m_lInsuranceFileCnt As Integer

    Private m_bInitialised As Boolean ' Initialised Status
    Private m_lReturn As gPMConstants.PMEReturnCode ' Error Code
    Private m_sLoadedRule As String = "" ' Loaded Rule stored as string
    Private m_bRuleLoaded As Boolean ' A rule is currently loaded
    Private m_sLoadedFileName As String = "" ' File Name including path

    Private m_bRuleIsCompiled As Boolean ' Rules are compiled indicator
    Private m_sDefaultRulePath As String = "" ' Default Rule Path from Registry

    Private m_lProcessMode As gPMConstants.PMEProcessMode '
    Private m_sTransactionType As String = "" ' Process Mode
    Private m_dtEffectiveDate As Date '  Indicators
    Private m_iTask As Integer '
    Private m_lNavigate As Integer '

    Private m_sTransTypeCode As String = "" ' Process Mode

    ' Local Copy of AuthLevelData Object
    Private m_oAuthLevelData As cAuthLevelData

    ' Database Classes
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flags
    Private m_bCloseDatabase As Boolean

    Private m_vDataDictionary As Object

    Private sLoadedCompiledRuleFileName As String = "" ' Compiled rule File Name

    Public Property AuthError() As Integer
        Get
            Return m_lAuthError
        End Get
        Set(ByVal Value As Integer)
            m_lAuthError = Value
        End Set
    End Property
    Public Property AuthUserID() As Integer
        Get
            Return m_lAuthUserID
        End Get
        Set(ByVal Value As Integer)
            m_lAuthUserID = Value
        End Set
    End Property
    Public Property CurrentUserID() As Integer
        Get
            Return m_lCurrentUserID
        End Get
        Set(ByVal Value As Integer)
            m_lCurrentUserID = Value
        End Set
    End Property
    Public Property PaymentAmount() As Double
        Get
            Return m_crPaymentAmount
        End Get
        Set(ByVal Value As Double)
            m_crPaymentAmount = Value
        End Set
    End Property
    Public Property OriginalPaymentAmount() As Double
        Get
            Return m_crOriginalCurrencyAmount
        End Get
        Set(ByVal Value As Double)
            m_crOriginalCurrencyAmount = Value
        End Set
    End Property
    Public Property PaymentType() As Integer
        Get
            Return m_lPaymentType
        End Get
        Set(ByVal Value As Integer)
            m_lPaymentType = Value
        End Set
    End Property
    Public Property ProductID() As Integer
        Get
            Return m_lProductID
        End Get
        Set(ByVal Value As Integer)
            m_lProductID = Value
        End Set
    End Property
    Public Property Reference() As Integer
        Get
            Return m_lReference
        End Get
        Set(ByVal Value As Integer)
            m_lReference = Value
        End Set
    End Property
    Public Property TransType() As Integer
        Get
            Return m_lTransType
        End Get
        Set(ByVal Value As Integer)
            m_lTransType = Value
        End Set
    End Property
    Public Property PaymentCurrencyID() As Integer
        Get
            Return m_lPaymentCurrencyID
        End Get
        Set(ByVal Value As Integer)
            m_lPaymentCurrencyID = Value
        End Set
    End Property
    Public Property OriginalPaymentCurrencyID() As Integer
        Get
            Return m_lOriginalPaymentCurrencyID
        End Get
        Set(ByVal Value As Integer)
            m_lOriginalPaymentCurrencyID = Value
        End Set
    End Property
    Public WriteOnly Property InsuranceFileCnt() As Integer
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property

    Public Property TransTypeCode() As String
        Get
            Return m_sTransTypeCode
        End Get
        Set(ByVal Value As String)
            m_sTransTypeCode = Value
        End Set
    End Property
    Private ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    ' ***************************************************************** '
    ' Name: LoadRule
    '
    ' Parameters:   N/A
    '
    ' Description: Locates and Loads the applicable rule file by
    '               examining the appropriate database rule
    '
    ' ***************************************************************** '
    Public Function LoadRule() As Integer

        Dim result As Integer = 0
        Dim sFileName As String = String.Empty

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' initialise - new rule - so reset defaults
            m_bRuleLoaded = False
            m_sLoadedRule = ""

            If m_bInitialised Then

                ' load rule for specified product and user
                If m_lProductID <> 0 And m_lAuthUserID <> 0 Then

                    ' determine whether or not the rules will be compiled
                    m_lReturn = CType(GetRuleCompiled(m_bRuleIsCompiled), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' find the rule name
                    m_lReturn = CType(GetRuleFileName(r_sFileName:=sFileName), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' if load rule hasnt already failed
                    If result <> gPMConstants.PMEReturnCode.PMFalse Then
                        ' check if the file is valid
                        m_lReturn = LoadAndValidateRule(m_sDefaultRulePath, sFileName)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadRule Failed for : " & m_sDefaultRulePath & sFileName, vApp:=ACApp, vClass:=ACClass, vMethod:="LoadRule", excep:=New Exception(Informations.Err().Description))

                            result = gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If

                Else
                    ' if the product id and user id havent been set up yet quit
                    result = gPMConstants.PMEReturnCode.PMFail
                End If

            Else
                result = gPMConstants.PMEReturnCode.PMFail
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadRule Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadRule", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: LoadRuleFile
    '
    ' Parameters:-  v_sRuleFileName   :- rule file name to load
    '
    '               v_sRuleFilePath   :- optional file path to find rule in
    '                  if empty function uses path provided in registry
    '
    ' Description: Loads the specified rule file
    '
    ' ***************************************************************** '
    Public Function LoadRuleFile(ByVal v_sRuleFileName As String, Optional ByVal v_sRuleFilePath As String = "") As Integer

        Dim result As Integer = 0
        Dim bGetRegistryRulePath As Boolean
        Dim sFilePath As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' initialise - new rule - so set defaults
            m_bRuleLoaded = False
            m_sLoadedRule = ""
            bGetRegistryRulePath = False

            ' if program has been initialised and rule file name passed
            If m_bInitialised And v_sRuleFileName <> "" Then

                sFilePath = v_sRuleFilePath

                ' check if the file path has been passed
                If False Then
                    bGetRegistryRulePath = True
                Else
                    If v_sRuleFilePath = "" Then
                        bGetRegistryRulePath = True
                    End If
                End If

                ' if no file path has been passed then retrieve it from the registry
                If bGetRegistryRulePath Then
                    sFilePath = m_sDefaultRulePath
                End If

                ' if load rule hasnt already failed
                If result <> gPMConstants.PMEReturnCode.PMFalse Then
                    ' check if the file is valid
                    m_lReturn = LoadAndValidateRule(sFilePath, v_sRuleFileName)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

            Else
                result = gPMConstants.PMEReturnCode.PMFail
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadRuleFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadRuleFile", excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ExecuteRule
    '
    ' Parameters:   r_bAuthorised := returns true if the user is
    '                   authorised to do the specified action
    '
    ' Description: Executes the loaded rule file (script or compiled)
    '               and returns a flag to indicate if the rules
    '                 determined that the user is or is not authorised
    '                   to perform the action
    ' ***************************************************************** '
    Public Function ExecuteRule(ByRef r_bAuthorised As Boolean) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If m_bRuleIsCompiled Then
                Return ExecuteRulesCompiled(r_bAuthorised:=r_bAuthorised)
            Else
                Return ExecuteRuleScript(r_bAuthorised:=r_bAuthorised)
            End If

        Catch
        End Try



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ExecuteRule Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ExecuteRule", excep:=New Exception(Informations.Err().Description))

        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ExecuteRuleScript
    '
    ' Parameters:   r_bAuthorised := returns true if the user is
    '                   authorised to do the specified action
    '
    ' Description: Executes the loaded rule "script" file and returns a flag to
    '               indicate if the rules determined that the user is or
    '                 is not authorised to perform the action
    ' ***************************************************************** '
    Function ExecuteRuleScript(ByRef r_bAuthorised As Boolean) As Integer

        Dim result As Integer = 0
        Dim oExtras As bGISPMUExtras.Business
        Dim oVBQuoteEngine As New VBQuoteEngine()
        Dim sRule As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_bInitialised Then

                ' check if there is a rule loaded
                If m_bRuleLoaded Then

                    '******************************
                    ' Set Up Data to Pass to Script
                    '******************************

                    ' load extras object ready to add to script

                    oExtras = New bGISPMUExtras.Business
                    m_lReturn = oExtras.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If result <> gPMConstants.PMEReturnCode.PMFalse Then

                        oExtras.InsuranceFileCnt = m_lInsuranceFileCnt

                        ' set up the authority level data
                        m_lReturn = SetUpAuthLevelData()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                        End If

                        If result <> gPMConstants.PMEReturnCode.PMFalse Then

                            'If option explicit missing from rule
                            ' then add it
                            If (m_sLoadedRule.IndexOf("Option Explicit") + 1) = 0 Then
                                sRule = "Option Explicit" & Strings.ChrW(13) & m_sLoadedRule
                                sRule = sRule.Replace("If cValue >= 100 Then", "If CCur(cValue) >= CCur(100) Then")
                            End If

                            oVBQuoteEngine = New VBQuoteEngine()

                            oVBQuoteEngine.ExecuteAuthorityRule(sRule, "Start", m_oAuthLevelData, oExtras)

                        End If
                    End If
                Else
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            If result <> gPMConstants.PMEReturnCode.PMFalse Then
                r_bAuthorised = m_oAuthLevelData.IsAuthorised
            End If

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

                ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ExecuteRuleScript Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ExecuteRuleScript", excep:=excep)

            


        Finally
            'destroy object references
        End Try



        Return result


        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ExecuteRuleCompiled
    '
    ' Parameters:   r_bAuthorised := returns true if the user is
    '                   authorised to do the specified action
    '
    ' Description: Executes the loaded rule "compiled" file and returns a flag to
    '               indicate if the rules determined that the user is or
    '                 is not authorised to perform the action
    ' ***************************************************************** '
    Private Function ExecuteRulesCompiled(ByRef r_bAuthorised As Boolean) As Integer

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim oRules As Object

        Try
            ' set up the authority rules data
            nResult = SetUpAuthLevelData()
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' create rule object
            oRules = CreateLateBoundObject_CompiledRules(sLoadedCompiledRuleFileName)
            If Not (oRules Is Nothing) Then
                oRules.AuthorityLevelData = m_oAuthLevelData
                oRules.Start()
                ' Get return value
                r_bAuthorised = oRules.AuthorityLevelData.IsAuthorised
            End If

        Catch ex As Exception
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Create Object :=" & sLoadedCompiledRuleFileName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ExecuteRulesCompiled", excep:=New Exception(Informations.Err().Description))
            nResult = gPMConstants.PMEReturnCode.PMError
        Finally
            oRules = Nothing
        End Try

        Return nResult
    End Function

    ' ***************************************************************** '
    ' Name: GetCompileRuleObjectName
    '
    ' Parameters:   r_sObjectName := returns true if the user is
    '                   authorised to do the specified action
    '
    ' Description: Executes the loaded rule "compiled" file and returns a flag to
    '               indicate if the rules determined that the user is or
    '                 is not authorised to perform the action
    ' ***************************************************************** '
    Private Function GetCompileRuleObjectName(ByVal v_sFileName As String, ByRef r_sObjectName As String) As Integer

        Dim result As Integer = 0
        Dim sTemp As String



        result = gPMConstants.PMEReturnCode.PMTrue

        ' strip path
        sTemp = v_sFileName.Substring(v_sFileName.Length - (v_sFileName.Length - (If(v_sFileName = "" And "\" = "", 0, (v_sFileName.LastIndexOf("\") + 1)))))

        ' strip file type
        If sTemp.IndexOf("."c) + 1 Then
            sTemp = sTemp.Substring(0, sTemp.IndexOf("."c))
        End If

        ' strip any spaces in name
        sTemp.Substring(0).Replace(" ", "")

        ' return object name
        r_sObjectName = "AuthLevel_" & sTemp & ".Sirius_Rule"

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SetUpAuthLevelData
    '
    ' Parameters:  None
    '
    ' Description: Copy local property values to AuthLeveData object
    ' ***************************************************************** '
    Private Function SetUpAuthLevelData() As Integer

        Dim result As Integer = 0
        Dim oCurrency As bACTCurrency.Form
        Dim sCurrencyCode As String = ""
        Dim sOriginalCurrencyCode As String = ""

        ' load currency object to convert currency ID to Code

        oCurrency = New bACTCurrency.Form
        m_lReturn = oCurrency.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New System.Exception(m_lReturn.ToString() + ", " + +", Cannot get reference to bACTCurrency.")
        End If


        m_lReturn = oCurrency.GetISOCodeFromCurrencyID(v_iCurrencyID:=m_lPaymentCurrencyID, r_sISOCode:=sCurrencyCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New System.Exception(m_lReturn.ToString() + ", " + +", Cannot get currency ISO Code for currency id.")
        End If

        m_lReturn = oCurrency.GetISOCodeFromCurrencyID(v_iCurrencyID:=m_lOriginalPaymentCurrencyID, r_sISOCode:=sOriginalCurrencyCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New System.Exception(m_lReturn.ToString() + ", " + +", Cannot get currency ISO Code for currency id.")
        End If

        result = gPMConstants.PMEReturnCode.PMTrue

        ' copy properties
        m_oAuthLevelData.AuthError = m_lAuthError
        m_oAuthLevelData.AuthUserID = m_lAuthUserID
        m_oAuthLevelData.CurrentUserID = m_lCurrentUserID
        m_oAuthLevelData.PaymentAmount = CDbl(m_crPaymentAmount)
        m_oAuthLevelData.PaymentType = m_lPaymentType
        m_oAuthLevelData.ProductID = m_lProductID
        m_oAuthLevelData.Reference = m_lReference
        m_oAuthLevelData.TransType = m_lTransType
        m_oAuthLevelData.CurrencyCode = sCurrencyCode.Trim()
        m_oAuthLevelData.PaymentCurrencyAmount = CDbl(m_crOriginalCurrencyAmount)
        m_oAuthLevelData.PaymentCurrencyCode = sOriginalCurrencyCode.Trim()

        oCurrency.Dispose()
        oCurrency = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetRuleCompiled
    '
    ' Parameters:  r_bRuleCompiled :- returns TRUE if rules are compiled
    '
    ' Description: Returns whether or not the rules we are executing are
    '                   going to be compiled or script based
    ' ***************************************************************** '
    Private Function GetRuleCompiled(ByRef r_bRuleCompiled As Boolean) As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing

        result = gPMConstants.PMEReturnCode.PMTrue

        ' clear down the database parameters
        m_oDatabase.Parameters.Clear()

        '***************
        ' Add Params
        '***************

        ' user id
        m_lReturn = CType(AddInputParameter(v_sName:="UserID", v_vValue:=m_lAuthUserID, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

        ' product id
        m_lReturn = CType(AddInputParameter(v_sName:="ProductID", v_vValue:=m_lProductID, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

        'AK 230503 - changed the id to code, which can now be worked out within the stored procedure
        ' transaction type code
        m_lReturn = CType(AddInputParameter(v_sName:="Transaction_Type_Code", v_vValue:=m_sTransTypeCode, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)

        '*****************
        ' Get Select Data
        '*****************

        If result <> gPMConstants.PMEReturnCode.PMFalse Then
            ' get the required rules script file name from the database
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRiskTypeRuleSetTypeSQL, sSQLName:=ACGetRiskTypeRuleSetTypeName, bStoredProcedure:=ACGetRiskTypeRuleSetTypeStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        '***************
        ' Assign Results
        '***************

        If result <> gPMConstants.PMEReturnCode.PMFalse Then
            ' return the file name from the results
            ' results should contain at most one result because there should
            ' only ever be one authority_level for a user per product.
            If Informations.IsArray(vArray) AndAlso Not String.IsNullOrEmpty(vArray(0, 0)) Then
                If CInt(vArray(0, 0)) = 3 Then
                    r_bRuleCompiled = True
                Else
                    r_bRuleCompiled = False
                End If

            End If

        End If

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: GetRulePath
    '
    ' Parameters:  r_sRulePath :- returns the rule file path
    '
    ' Description: Returns the path of the rule script files
    '                       from the registry
    ' ***************************************************************** '
    Private Function GetRulePath(ByRef r_sRulePath As String) As Integer

        Dim result As Integer = 0
        Dim sSubKey As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue

        sSubKey = "GIS\"

        ' Get rule path from registry
        m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="RulePath", r_sSettingValue:=r_sRulePath, v_sSubKey:=sSubKey), gPMConstants.PMEReturnCode)

        If r_sRulePath = "" Then
            result = gPMConstants.PMEReturnCode.PMFalse

            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to read RulePath registry setting for " & sSubKey, vApp:=ACApp, vClass:=ACClass, vMethod:="GetRulePath", excep:=New Exception(Informations.Err().Description))
        Else
            If Not r_sRulePath.EndsWith("\") Then
                r_sRulePath = r_sRulePath & "\"
            End If
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetRuleFileName
    '
    ' Parameters:  r_sFileName :- returns the required rules filename
    '
    ' Description: Returns the filename of the user defined script
    '                   for the specified user id and product id
    ' ***************************************************************** '
    Private Function GetRuleFileName(ByRef r_sFileName As String) As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing

        result = gPMConstants.PMEReturnCode.PMTrue

        ' clear down the database parameters
        m_oDatabase.Parameters.Clear()

        '***************
        ' Add Params
        '***************

        ' user id
        m_lReturn = CType(AddInputParameter(v_sName:="UserID", v_vValue:=m_lAuthUserID, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

        ' product id
        m_lReturn = CType(AddInputParameter(v_sName:="ProductID", v_vValue:=m_lProductID, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

        'AK 230503 - changed the id to code, which can now be worked out within the stored procedure
        ' transaction type code
        m_lReturn = CType(AddInputParameter(v_sName:="Transaction_Type_Code", v_vValue:=m_sTransTypeCode, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)

        '*****************
        ' Get Select Data
        '*****************

        If result <> gPMConstants.PMEReturnCode.PMFalse Then
            ' get the required rules script file name from the database
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRuleFileNameSQL, sSQLName:=ACGetRuleFileNameName, bStoredProcedure:=ACGetRuleFileNameStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        '***************
        ' Assign Results
        '***************

        If result <> gPMConstants.PMEReturnCode.PMFalse Then
            ' return the file name from the results
            ' results should contain at most one result because there should
            ' only ever be one authority_level for a user per product.
            If Informations.IsArray(vArray) Then
                r_sFileName = CStr(vArray(0, 0))
            End If

        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: LoadAndValidateRule
    '
    ' Parameters:  v_sFilePath :- set the required rules file path
    '
    '              v_sFileName := set the required rules file name
    '
    ' Description: Return whether the specified file is a valid rule file
    '               A valid file will be loaded and stored
    ' ***************************************************************** '
    Private Function LoadAndValidateRule(ByVal v_sFilePath As String, ByVal v_sFileName As String) As Integer

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim oRule As Object
        Dim sObjectName As String = ""
        Dim fso As Object
        Dim fsoFile As FileInfo
        Dim sFile As String = ""
        Dim sRight, sLeft As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sRight = v_sFilePath.Substring(v_sFilePath.Length - 1)
            sLeft = v_sFileName.Substring(0, 1)

            ' set up file name
            If sRight = "\" Or sRight = "/" Then
                If sLeft = "\" Or sLeft = "/" Then
                    ' need to strip one character
                    v_sFileName = v_sFileName.Substring(1)
                End If
            Else
                If sLeft <> "\" Or sLeft <> "/" Then
                    ' need to add backspace character
                    v_sFileName = "\" & v_sFileName
                End If
            End If

            ' combine file path and name
            sFile = v_sFilePath & v_sFileName

            ' check this isnt the file that is currently loaded
            If sFile.ToLower() <> m_sLoadedFileName.ToLower() Then
                If Not m_bRuleIsCompiled Then
                    ' create file system object
                    fso = New Object()

                    ' open rule file
                    If File.Exists(sFile) Then
                        '' store rule
                        Using fsoTxtStream As FileStream = New FileStream(sFile, FileMode.Open, FileAccess.Read)

                            Using r As StreamReader = New StreamReader(fsoTxtStream)
                                m_sLoadedRule = r.ReadToEnd()
                            End Using
                        End Using
                        ' validate script
                        m_lReturn = CType(ValidateScriptFile(m_sLoadedRule), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                        End If
                    Else
                        ' rule file doesnt exist
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If
                Else
                    sLoadedCompiledRuleFileName = v_sFileName
                End If
            Else
                ' dont bother to load as we have already done this
            End If

            If result = gPMConstants.PMEReturnCode.PMTrue Then
                m_bRuleLoaded = True
                m_sLoadedFileName = sFile
            End If

        Catch ex As Exception
            ' if we fail to create compiled rule
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadAndValidateRule Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAndValidateRule", excep:=New Exception(Informations.Err().Description))

        Finally
            'destroy references
            fso = Nothing
            fsoFile = Nothing
            oRule = Nothing
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ValidateScriptFile
    '
    ' Parameters:  v_sFile :- script file to validate
    '
    ' Description: Runs a series of validation checks against the
    '               specified script file, logs any error found
    ' ***************************************************************** '

    Private Function ValidateScriptFile(ByVal v_sFile As String) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        If v_sFile = "" Then

            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Validate Script Failed, Reason :=File is Empty", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateScriptFile", excep:=New Exception(Informations.Err().Description))


            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        ' there must be a start function present
        If (v_sFile.IndexOf(" Start()", StringComparison.CurrentCultureIgnoreCase) + 1) = 0 Then

            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Validate Script Failed, Reason :=No Start Procedure", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateScriptFile", excep:=New Exception(Informations.Err().Description))

            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddInputParameter
    '
    ' Parameters: v_sName   : Parameter Name
    '             v_vValue  : Parameter Value
    '             v_iType   : Parameter DataType
    '
    ' Description: Adds an input parameter to the database parameters
    '
    ' History:
    '           Created : MEvans : 18-12-2002 : 202
    ' ***************************************************************** '
    Private Function AddInputParameter(ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iType As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "AddInputParameter"



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Add Parameter to database object

        If m_oDatabase.Parameters.Add(sName:=v_sName, vValue:=CStr(v_vValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iType) <> gPMConstants.PMEReturnCode.PMTrue Then

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Informations.Err().Description))

        End If

        Return result

    End Function





    ' ***************************************************************** '
    ' ***************************************************************** '
    '                 START STANDARD BUSINESS METHODS
    ' ***************************************************************** '
    ' ***************************************************************** '

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceId As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceId
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel


            m_bInitialised = False

            m_oAuthLevelData = New cAuthLevelData()


            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password

            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now


            ' determine whether or not the rules will be compiled
            m_lReturn = CType(GetRuleCompiled(m_bRuleIsCompiled), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' get the default rule path
            m_lReturn = CType(GetRulePath(m_sDefaultRulePath), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_bInitialised = True
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("iUserID", iUserID)
            oDict.Add("iSourceId", iSourceId)
            oDict.Add("iLanguageID", iLanguageID)
            oDict.Add("iCurrencyID", iCurrencyID)
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep, oDicParms:=oDict)

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
                m_vDataDictionary = Nothing
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



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("vEffectiveDate", vEffectiveDate)
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", excep:=excep, oDicParms:=oDict)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' ***************************************************************** '
    '                 END STANDARD BUSINESS METHODS
    ' ***************************************************************** '
    ' ***************************************************************** '
    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class
