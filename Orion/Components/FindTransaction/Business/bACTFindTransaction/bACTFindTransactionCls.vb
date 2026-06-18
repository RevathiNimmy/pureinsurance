Option Strict Off
Option Explicit On
Imports System.Globalization
Imports System.Text
'Developer Guide no: 129
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable

    ' ************************************************
    ' Added to replace global variables 19/04/2004
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************

    ' ***************************************************************** '
    ' Class Name: FindTrans
    '
    ' Date: 28 August 1997
    '
    ' Description: Creatable class used by the FindTrans interface.
    '
    ' Edit History:
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    'DC 12/10/00
    Private m_oArcDatabase As dPMDAO.Database
    Private m_bArcCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lError As gPMConstants.PMEReturnCode

    ' Task
    Private m_iTask As Integer

    ' Navigate
    Private m_lNavigate As Integer

    ' Process Mode
    Private m_lProcessMode As Integer

    ' Type of Business
    Private m_sTypeOfBusiness As New StringsHelper.FixedLengthString(10)

    ' Effective
    Private m_dtEffectiveDate As Date

    ' Error Code (Private)
    Private m_lReturn As Integer

    ' Component Sub Type
    Private m_sSubType As New StringsHelper.FixedLengthString(20)
    ' Variable Data Business Component (Private)

    ' PM Lookup Business Component (Private)
    Private m_oLookup As bPMLookup.Business

    ' NavigatorV3 variables
    Private m_lPMAuthorityLevel As Integer
    Private m_lStatus As Integer

    'Business objects
    Private m_oAllocationCalculate As bACTAllocationCalculate.Form
    Private m_oPeriod As bACTPeriod.Form
    Private m_oAccount As bACTAccount.Form
    Private m_oSolutionConfig As bSIRSolutionConfig.Business
    Private m_oAuditSet As bACTAuditSet.Form
    Private m_oCurrencyConvert As bACTCurrencyConvert.Form

    Private m_iHasUnrestrictedEnquiry As Integer
    Private m_iHasUnrestrictedUpdate As Integer


    Private m_bDrillCompany As Boolean
    Private m_bRestrictByCompany As Boolean

    Private m_sUnderwritingOrAgency As String = ""

    Private m_bIsSingleInstalmentPlan As Boolean
    'developer guide no.39
    Private Const ACSelectViewAllocationSQL As String = "spu_ACT_Select_View_Allocation"
    Private Const ACSelectViewAllocationName As String = "SelectlViewAllocation"

    Private Const ACSelectRollupTransactionSQL As String = "spu_ACT_Select_RollupTransactions"
    Private Const ACSelectRollupTransactionName As String = "SelectRollupTransaction"

    Private Const ACSelectMarkTransactionSQL As String = "spu_ACT_Select_Mark_Transaction"
    Private Const ACSelectMarkTransactionName As String = "SelectlMarkTransaction"
    Private Const ACSelectAccountTypeSQL As String = "spu_ACT_Get_Account_Types"
    Private Const ACSelectAccountTypeName As String = "SelectAccountTypes"
    Private Const ACSelectGetPolicyBalanceSQL As String = "spu_Get_Policy_Balance"
    Private Const ACSelectGetPolicyBalanceName As String = "GetPolicyBalance"
    Private Const ACSelectGetPremiumFinanceBalanceSQL As String = "spu_Get_Premium_Finance_Balance "
    Private Const ACSelectGetPremiumFinanceBalanceName As String = "GetPremiumFinanceBalance"

    Private Const ACAccountTypeClient As Integer = 2
    Private Const ACAccountTypeInsurer As Integer = 4
    Private Const ACAccountTypeAgent As Integer = 5
    Private Const ACAccountTypePremiumFinance As Integer = 6
    Private Const ACAccountTypeFees As Integer = 7
    Private Const ACAccountTypeDiscount As Integer = 8
    Private Const ACAccountTypeCommission As Integer = 9
    Private Const ACAccountTypeSubAgent As Integer = 10
    Private Const ACAccountTypeWriteOff As Integer = 21
    Private Const ACAccountTypeExtra As Integer = 22
    Private Const ACAccountCodeWriteOff1 As String = "N4092"
    Private Const ACAccountCodeWriteOff2 As String = "N5092"

    Private Const ACPartyTypeInsurer As Integer = 7
    Private Const ACPartyTypeExtra As Integer = 10

    Public Property CallingAppName() As String
        Get
            Return m_sCallingAppName
        End Get
        Set(ByVal Value As String)
            m_sCallingAppName = Value
        End Set
    End Property

    Public Property PMAuthorityLevel() As Integer
        Get
            Return m_lPMAuthorityLevel
        End Get
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = Value
        End Set
    End Property

    Public Property Status() As Integer
        Get
            Return m_lStatus
        End Get
        Set(ByVal Value As Integer)
            m_lStatus = Value
        End Set
    End Property

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

    Public ReadOnly Property TypeOfBusiness() As String
        Get

            Return m_sTypeOfBusiness.Value

        End Get
    End Property

    Public ReadOnly Property EffectiveDate() As Date
        Get

            Return m_dtEffectiveDate

        End Get
    End Property

    Public WriteOnly Property DrillCompany() As Boolean
        Set(ByVal Value As Boolean)

            m_bDrillCompany = Value

        End Set
    End Property

    Public WriteOnly Property RestrictByCompany() As Boolean
        Set(ByVal Value As Boolean)

            m_bRestrictByCompany = Value

        End Set
    End Property

    Public ReadOnly Property UnderwritingOrAgency() As String
        Get

            If m_sUnderwritingOrAgency = "" Then
                m_lReturn = getUnderwritingOrAgency()
            End If

            Return m_sUnderwritingOrAgency

        End Get
    End Property
    Public WriteOnly Property IsSingleInstalmentPlan() As Boolean
        Set(ByVal Value As Boolean)

            m_bIsSingleInstalmentPlan = Value

        End Set
    End Property


    ' PUBLIC Property Procedures (End)

    ' PUBLIC Methods (Begin)


    ' ***************************************************************** '
    ' Name: SetKeys
    '
    ' Description: Navigator SetKeys function.
    '
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeysFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetKeys
    '
    ' Description: Navigator GetKeys function.
    '
    ' ***************************************************************** '
    'Developer Guide No. 101
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            vKeyArray = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeysFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetSummary
    '
    ' Description: GetSummary Navigator function.
    '
    ' ***************************************************************** '
    Public Function GetSummary(ByRef vSummaryArray As Object) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummaryFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Start
    '
    ' Description: Navigator Start function. Entry point into Navigator.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StartFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTypeOfBusiness As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

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

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Informations.IsNothing(vTypeOfBusiness) Then

                m_sTypeOfBusiness.Value = CStr(vTypeOfBusiness)
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
    ' Name: GetLookupValues (Public)
    '
    ' Description: Gets the Lookup values as defined by vTableArray.
    '
    '
    ' ***************************************************************** '
    'developer guide no.33
    Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset Result Array
            vResultArray = Nothing

            ' Get the Lookup items from the Business Component

            m_lReturn = m_oLookup.GetLookupValues(iLookupType:=iLookupType, vTableArray:=vTableArray, iLanguageID:=iLanguageID, dtEffectiveDate:=m_dtEffectiveDate, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookupValues Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long


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
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel


            ' Initialisation Code.

            ' Set Username and Password

            ' Set User ID
            m_iUserID = iUserID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level

            ' Get Reference to Database
            'ECK 20/7/99 Use Component Services


            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DC 12/10/00

            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_bNewInstanceCreated:=m_bArcCloseDatabase, r_oCheckedDatabase:=m_oArcDatabase, v_vDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create PM Lookup Business Object
            m_oLookup = New BPMLOOKUP.Business()

            ' Initialise PM Lookup Business Object passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFOrion

            ' CTAF 291099 - Create instances of extra business objects

            ' Allocation Calculate

            m_oAllocationCalculate = New bACTAllocationCalculate.Form
            m_lReturn = m_oAllocationCalculate.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserId:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyId:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Remove instance of Component Services
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of bACTAllocationCalculate.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Currency Convert

            m_oCurrencyConvert = New bACTCurrencyConvert.Form
            m_lReturn = m_oCurrencyConvert.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Remove instance of Component Services
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of bACTCurrencyConvert.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Period

            m_oPeriod = New bACTPeriod.Form
            m_lReturn = m_oPeriod.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Remove instance of Component Services
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of bACTPeriod.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Account

            m_oAccount = New bACTAccount.Form
            m_lReturn = m_oAccount.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Remove instance of Component Services
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of bACTAccount.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Solution Config

            m_oSolutionConfig = New bSIRSolutionConfig.Business
            m_lReturn = m_oSolutionConfig.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Remove instance of Component Services
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of bSIRSolutionConfig.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' AMB 13/02/2003: PS220 - added auditset object for Manage Debtors development
            ' Auditset

            m_oAuditSet = New bACTAuditset.Form
            m_lReturn = m_oAuditSet.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Remove instance of Component Services
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of bACTAuditSet.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = GetUserAuthorities()

            ' CTAF 291099 - Remove instance of Component Services

            Return result

        Catch excep As System.Exception



            ' Error Section.
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
            Me.disposedValue = True
            If disposing Then
                m_oLookup = Nothing
                If m_oAllocationCalculate IsNot Nothing Then
                    m_oAllocationCalculate.Dispose()
                    m_oAllocationCalculate = Nothing
                End If
                If m_oCurrencyConvert IsNot Nothing Then
                    m_oCurrencyConvert.Dispose()
                    m_oCurrencyConvert = Nothing
                End If
                If m_oPeriod IsNot Nothing Then
                    m_oPeriod.Dispose()
                    m_oPeriod = Nothing
                End If
                If m_oAccount IsNot Nothing Then
                    m_oAccount.Dispose()
                    m_oAccount = Nothing
                End If
                If m_oSolutionConfig IsNot Nothing Then
                    m_oSolutionConfig.Dispose()
                    m_oSolutionConfig = Nothing
                End If
                If m_oAuditSet IsNot Nothing Then
                    m_oAuditSet.Dispose()
                    m_oAuditSet = Nothing
                End If

                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()

                End If
                m_oDatabase = Nothing
                If m_bArcCloseDatabase Then
                    ' Close the Database
                    m_oArcDatabase.CloseDatabase()


                End If
                m_oArcDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub



    'developer guide no.33
    Public Function SelectTransQuery(ByRef r_lNumberOfRecords As Integer, ByRef r_vResultArray(,) As Object, ByVal v_iCompanyID As Integer,
                                     Optional ByVal v_vAccountID As Object = Nothing, Optional ByVal v_vDocumentRef As Object = Nothing,
                                     Optional ByVal v_vCurrencyID As Object = Nothing, Optional ByVal v_vCurrencyAmount As Object = Nothing,
                                     Optional ByVal v_vTolerance As Object = Nothing, Optional ByVal v_vDocTypeGroupId As Object = Nothing,
                                     Optional ByVal v_vDocumentTypeID As Object = Nothing, Optional ByVal v_vPeriodID As Object = Nothing,
                                     Optional ByVal v_vDateFrom As Object = Nothing, Optional ByVal v_vDateTo As Object = Nothing,
                                     Optional ByVal v_vInsuranceRef As Object = Nothing, Optional ByVal v_vUsername As Object = Nothing,
                                     Optional ByVal v_vPurchaseInvoiceNo As Object = Nothing, Optional ByVal v_vPurchaseOrderNo As Object = Nothing,
                                     Optional ByVal v_vDepartment As Object = Nothing, Optional ByVal v_vSpare As Object = Nothing,
                                     Optional ByVal v_vOutstandingOnly As Object = Nothing, Optional ByVal v_vIsNewPF As Object = Nothing,
                                     Optional ByVal v_vInsuredAccountID As Object = Nothing, Optional ByVal v_bRollup As Boolean = False,
                                     Optional ByVal v_vCashListId As Object = Nothing, Optional ByVal v_bOrderBySpare As Boolean = False,
                                     Optional ByVal v_lDocumentID As Integer = 0, Optional ByVal v_lFinancePlanCnt As Integer = 0,
                                     Optional ByVal v_vUnderwritingYearID As Object = Nothing, Optional ByVal v_vSourceArray As Object = Nothing,
                                     Optional ByVal v_vTransDetailIDs As Object = Nothing, Optional ByVal v_bDisplay500 As Boolean = True,
                                     Optional ByVal v_sAltReference As String = "", Optional ByVal v_bIncludeReversedTrans As Boolean = False,
                                     Optional ByVal v_iAccountTypeID As Integer = 0, Optional ByVal v_bInsuredAccountView As Boolean = False,
                                     Optional ByVal v_sBGREf As String = "", Optional ByVal v_vDueDateFrom As Object = Nothing, Optional ByVal v_vDueDateTo As Object = Nothing,
                                     Optional ByVal v_sCaseNumber As String = "",
                                     Optional ByVal v_bIncludeExcludedTaxRows As Boolean = True) As Integer
        Return SelectTransQuery(r_lNumberOfRecords:=r_lNumberOfRecords, r_vResultArray:=r_vResultArray, v_iCompanyID:=v_iCompanyID, v_iAgentCnt:=0, v_vAccountID:=v_vAccountID, v_vDocumentRef:=v_vDocumentRef,
                    v_vCurrencyID:=v_vCurrencyID, v_vCurrencyAmount:=v_vCurrencyAmount, v_vTolerance:=v_vTolerance, v_vDocTypeGroupId:=v_vDocTypeGroupId, v_vDocumentTypeID:=v_vDocumentTypeID, v_vPeriodID:=v_vPeriodID,
                    v_vDateFrom:=v_vDateFrom, v_vDateTo:=v_vDateTo, v_vInsuranceRef:=v_vInsuranceRef, v_vUsername:=v_vUsername, v_vPurchaseInvoiceNo:=v_vPurchaseInvoiceNo, v_vPurchaseOrderNo:=v_vPurchaseOrderNo,
                    v_vDepartment:=v_vDepartment, v_vSpare:=v_vSpare, v_vOutstandingOnly:=v_vOutstandingOnly, v_vIsNewPF:=v_vIsNewPF, v_vInsuredAccountID:=v_vInsuredAccountID, v_bRollup:=v_bRollup,
                    v_vCashListId:=v_vCashListId, v_bOrderBySpare:=v_bOrderBySpare, v_lDocumentID:=v_lDocumentID, v_lFinancePlanCnt:=v_lFinancePlanCnt, v_vUnderwritingYearID:=v_vUnderwritingYearID,
                    v_vSourceArray:=v_vSourceArray, v_vTransDetailIDs:=v_vTransDetailIDs, v_bDisplay500:=v_bDisplay500, v_sAltReference:=v_sAltReference, v_bIncludeReversedTrans:=v_bIncludeReversedTrans,
                    v_iAccountTypeID:=v_iAccountTypeID, v_bInsuredAccountView:=v_bInsuredAccountView, v_sBGREf:=v_sBGREf, v_vDueDateFrom:=v_vDueDateFrom, v_vDueDateTo:=v_vDueDateTo,
                    v_sCaseNumber:=v_sCaseNumber, v_bIncludeExcludedTaxRows:=v_bIncludeExcludedTaxRows)

    End Function

    'developer guide no.33
    Public Function SelectTransQuery(ByRef r_lNumberOfRecords As Integer, ByRef r_vResultArray(,) As Object, ByVal v_iCompanyID As Integer, ByVal v_iAgentCnt As Integer,
                                     Optional ByVal v_vAccountID As Object = Nothing, Optional ByVal v_vDocumentRef As Object = Nothing,
                                     Optional ByVal v_vCurrencyID As Object = Nothing, Optional ByVal v_vCurrencyAmount As Object = Nothing,
                                     Optional ByVal v_vTolerance As Object = Nothing, Optional ByVal v_vDocTypeGroupId As Object = Nothing,
                                     Optional ByVal v_vDocumentTypeID As Object = Nothing, Optional ByVal v_vPeriodID As Object = Nothing,
                                     Optional ByVal v_vDateFrom As Object = Nothing, Optional ByVal v_vDateTo As Object = Nothing,
                                     Optional ByVal v_vInsuranceRef As Object = Nothing, Optional ByVal v_vUsername As Object = Nothing,
                                     Optional ByVal v_vPurchaseInvoiceNo As Object = Nothing, Optional ByVal v_vPurchaseOrderNo As Object = Nothing,
                                     Optional ByVal v_vDepartment As Object = Nothing, Optional ByVal v_vSpare As Object = Nothing,
                                     Optional ByVal v_vOutstandingOnly As Object = Nothing, Optional ByVal v_vIsNewPF As Object = Nothing,
                                     Optional ByVal v_vInsuredAccountID As Object = Nothing, Optional ByVal v_bRollup As Boolean = False,
                                     Optional ByVal v_vCashListId As Object = Nothing, Optional ByVal v_bOrderBySpare As Boolean = False,
                                     Optional ByVal v_lDocumentID As Integer = 0, Optional ByVal v_lFinancePlanCnt As Integer = 0,
                                     Optional ByVal v_vUnderwritingYearID As Object = Nothing, Optional ByVal v_vSourceArray As Object = Nothing,
                                     Optional ByVal v_vTransDetailIDs As Object = Nothing, Optional ByVal v_bDisplay500 As Boolean = True,
                                     Optional ByVal v_sAltReference As String = "", Optional ByVal v_bIncludeReversedTrans As Boolean = False,
                                     Optional ByVal v_iAccountTypeID As Integer = 0, Optional ByVal v_bInsuredAccountView As Boolean = False,
                                     Optional ByVal v_sBGREf As String = "", Optional ByVal v_vDueDateFrom As Object = Nothing, Optional ByVal v_vDueDateTo As Object = Nothing,
                                     Optional ByVal v_sCaseNumber As String = "",
                                     Optional ByVal v_bIncludeExcludedTaxRows As Boolean = True) As Integer


        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: SelectTransQuery
        ' PURPOSE: Return array of transactions according to parameters
        ' AUTHOR: Peter Finney
        ' DATE: 26-Sep-02, 11:07 AM
        ' RETURNS: PMTrue for success
        ' CHANGES: Complete rebuild of sql and parameters
        '          AMB 11/02/2003 - changes for IAG PS220 Manage Debtors
        '          DD 11/04/2003 - changed for IAG PS220 Insured Account
        '          Thinh Nguyen 01/02/2004 add insurance file cnt to param list
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim sSQLWhere As String = ""

        Dim sSQLSelect As String = ""
        Dim sSQLGroupBy As String = ""
        Dim sSQLHaving As String = ""
        Dim sOrderBy As String = ""
        Dim sSQLExcludeTaxRows As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue
        Try
            ' Get the base query
            m_lReturn = SelectTransQueryWhere(sSQLWhere, v_iCompanyID, v_vAccountID, v_vDocumentRef, v_vCurrencyID, v_vCurrencyAmount, v_vTolerance, v_vDocTypeGroupId,
                                              v_vDocumentTypeID, v_vPeriodID, v_vDateFrom, v_vDateTo, v_vInsuranceRef, v_vUsername, v_vPurchaseInvoiceNo,
                                              v_vPurchaseOrderNo, v_vDepartment, v_vSpare, v_vOutstandingOnly, v_vCashListId, v_iAgentCnt, v_vIsNewPF, v_vInsuredAccountID,
                                              v_bRollup, v_lDocumentID, v_lFinancePlanCnt, v_vUnderwritingYearID, v_vSourceArray, v_vTransDetailIDs, ToSafeString(v_sAltReference).Replace("'", "''"),
                                              v_bIncludeReversedTrans, v_iAccountTypeID, v_bInsuredAccountView, v_sBGREf, v_vDueDateFrom, v_vDueDateTo, v_sCaseNumber)

            '27/05/2003 - PWC - 186 - Debt Rollup
            If Not v_bRollup Then
                'PSL 01/07/2003 Different select for broking and underwriting

                If v_bDisplay500 Then
                    sSQLSelect = ACTransFromQuerySelect500Underwriting
                Else
                    sSQLSelect = ACTransFromQuerySelectUnderwriting
                End If
                sSQLSelect = sSQLSelect & ACTransFromQuerySelectListUnderwriting

                sSQLGroupBy = ""
                sSQLHaving = ""
            Else

                If v_bDisplay500 Then
                    sSQLSelect = ACRollupTransFromQuerySelect500
                Else
                    sSQLSelect = ACRollupTransFromQuerySelect
                End If
                sSQLSelect = sSQLSelect & ACRollupTransFromQuerySelectList

                'Get the having clause
                If SelectTransQueryHaving(r_sSQL:=sSQLHaving, v_vCurrencyAmount:=v_vCurrencyAmount, v_vTolerance:=v_vTolerance) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Throw New System.Exception((ObjectError + m_lReturn).ToString() + ", [local], Failed: SelectTransQueryHaving")
                End If

                sSQLGroupBy = ACRollupTransFromQueryGroupBy
            End If

            If v_bOrderBySpare Then
                sOrderBy = ACTransFromQueryOrderBySpare
                sSQLSelect = sSQLSelect & ACTransFromQuerySelectSpareType
            Else
                sOrderBy = ACTransFromQueryOrderBy
            End If

            ' The final sql statement just need and order by

            'PSL 25/02/2003 Issue 2443 Different from clause for new Premium Finance search

            If v_bIncludeExcludedTaxRows = False Then
                sSQLExcludeTaxRows = ACIncludeExcludedTaxRows
            Else
                sSQLExcludeTaxRows = ""
            End If


            If Not Informations.IsNothing(v_vIsNewPF) Then
                If v_vIsNewPF = 1 Then
                    sSQL = sSQLSelect &
                          ACTransFromQueryFromNewPF &
                          GetTransFromQueryAdditional() &
                          ACTransFromAdditionalSAMQuery &
                          sSQLWhere &
                          sSQLExcludeTaxRows &
                          sSQLGroupBy &
                          sSQLHaving &
                          sOrderBy
                Else
                    sSQL = sSQLSelect &
                           ACTransFromQueryFrom &
                           GetTransFromQueryAdditional() &
                           ACTransFromAdditionalSAMQuery &
                           sSQLWhere &
                           sSQLGroupBy &
                           sSQLHaving &
                           sOrderBy
                End If
            Else
                sSQL = sSQLSelect &
                      ACTransFromQueryFrom &
                      GetTransFromQueryAdditional() &
                      ACTransFromAdditionalSAMQuery &
                      sSQLWhere &
                      sSQLGroupBy &
                      sSQLHaving &
                      sOrderBy
            End If

            ' Call the procedure
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:=ACTransFromQueryName, bStoredProcedure:=ACTransFromQueryStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception((ObjectError + m_lReturn).ToString() + ", [local], Failed to execute query")
            End If

            ' Check for results
            If Not Informations.IsArray(r_vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch ex As Exception

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="General failure in method", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectTransQuery", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)
            result = gPMConstants.PMEReturnCode.PMFalse
            Return result

        End Try

    End Function


    Public Function SelectTransQueryFiltered(ByRef r_lNumberOfRecords As Integer, ByRef r_vResultArray(,) As Object, ByVal v_iCompanyID As Integer, Optional ByVal v_vAccountID As Object = Nothing, Optional ByVal v_vDocumentRef As Object = Nothing, Optional ByVal v_vCurrencyID As Object = Nothing, Optional ByVal v_vCurrencyAmount As Object = Nothing, Optional ByVal v_vTolerance As Object = Nothing, Optional ByVal v_vDocTypeGroupId As Object = Nothing, Optional ByVal v_vDocumentTypeID As Object = Nothing, Optional ByVal v_vPeriodID As Object = Nothing, Optional ByVal v_vDateFrom As Object = Nothing, Optional ByVal v_vDateTo As Object = Nothing, Optional ByVal v_vInsuranceRef As Object = Nothing, Optional ByVal v_vUsername As Object = Nothing, Optional ByVal v_vPurchaseInvoiceNo As Object = Nothing, Optional ByVal v_vPurchaseOrderNo As Object = Nothing, Optional ByVal v_vDepartment As Object = Nothing, Optional ByVal v_vSpare As Object = Nothing, Optional ByVal v_vOutstandingOnly As Object = Nothing, Optional ByVal v_vIsNewPF As Object = Nothing, Optional ByVal v_vInsuredAccountID As Object = Nothing, Optional ByVal v_bRollup As Boolean = False, Optional ByVal v_vCashListId As Object = Nothing, Optional ByVal v_bOrderBySpare As Boolean = False, Optional ByVal v_lDocumentID As Integer = 0, Optional ByVal v_vSourceArray As Object = Nothing, Optional ByVal v_bDisplay500 As Boolean = True, Optional ByVal v_bIncludeReversedTrans As Boolean = False, Optional ByVal v_iAccountTypeID As Integer = 0, Optional ByVal v_bInsuredAccountView As Boolean = False, Optional ByVal v_sCaseNumber As String = "") As Integer
        Return SelectTransQueryFiltered(r_lNumberOfRecords:=r_lNumberOfRecords, r_vResultArray:=r_vResultArray, v_iCompanyID:=v_iCompanyID, v_iAgentCnt:=0, v_vAccountID:=v_vAccountID, v_vDocumentRef:=v_vDocumentRef,
                                        v_vCurrencyID:=v_vCurrencyID, v_vCurrencyAmount:=v_vCurrencyAmount, v_vTolerance:=v_vTolerance, v_vDocTypeGroupId:=v_vDocTypeGroupId,
                                        v_vDocumentTypeID:=v_vDocumentTypeID, v_vPeriodID:=v_vPeriodID, v_vDateFrom:=v_vDateFrom, v_vDateTo:=v_vDateTo, v_vInsuranceRef:=v_vInsuranceRef,
                                        v_vUsername:=v_vUsername, v_vPurchaseInvoiceNo:=v_vPurchaseInvoiceNo, v_vPurchaseOrderNo:=v_vPurchaseOrderNo, v_vDepartment:=v_vDepartment,
                                        v_vSpare:=v_vSpare, v_vOutstandingOnly:=v_vOutstandingOnly, v_vIsNewPF:=v_vIsNewPF, v_vInsuredAccountID:=v_vInsuredAccountID, v_bRollup:=v_bRollup,
                                        v_vCashListId:=v_vCashListId, v_bOrderBySpare:=v_bOrderBySpare, v_lDocumentID:=v_lDocumentID, v_vSourceArray:=v_vSourceArray, v_bDisplay500:=v_bDisplay500,
                                        v_bIncludeReversedTrans:=v_bIncludeReversedTrans, v_iAccountTypeID:=v_iAccountTypeID, v_bInsuredAccountView:=v_bInsuredAccountView, v_sCaseNumber:=v_sCaseNumber)
    End Function

    Public Function SelectTransQueryFiltered(ByRef r_lNumberOfRecords As Integer, ByRef r_vResultArray(,) As Object, ByVal v_iCompanyID As Integer, ByVal v_iAgentCnt As Integer, Optional ByVal v_vAccountID As Object = Nothing, Optional ByVal v_vDocumentRef As Object = Nothing, Optional ByVal v_vCurrencyID As Object = Nothing, Optional ByVal v_vCurrencyAmount As Object = Nothing, Optional ByVal v_vTolerance As Object = Nothing, Optional ByVal v_vDocTypeGroupId As Object = Nothing, Optional ByVal v_vDocumentTypeID As Object = Nothing, Optional ByVal v_vPeriodID As Object = Nothing, Optional ByVal v_vDateFrom As Object = Nothing, Optional ByVal v_vDateTo As Object = Nothing, Optional ByVal v_vInsuranceRef As Object = Nothing, Optional ByVal v_vUsername As Object = Nothing, Optional ByVal v_vPurchaseInvoiceNo As Object = Nothing, Optional ByVal v_vPurchaseOrderNo As Object = Nothing, Optional ByVal v_vDepartment As Object = Nothing, Optional ByVal v_vSpare As Object = Nothing, Optional ByVal v_vOutstandingOnly As Object = Nothing, Optional ByVal v_vIsNewPF As Object = Nothing, Optional ByVal v_vInsuredAccountID As Object = Nothing, Optional ByVal v_bRollup As Boolean = False, Optional ByVal v_vCashListId As Object = Nothing, Optional ByVal v_bOrderBySpare As Boolean = False, Optional ByVal v_lDocumentID As Integer = 0, Optional ByVal v_vSourceArray As Object = Nothing, Optional ByVal v_bDisplay500 As Boolean = True, Optional ByVal v_bIncludeReversedTrans As Boolean = False, Optional ByVal v_iAccountTypeID As Integer = 0, Optional ByVal v_bInsuredAccountView As Boolean = False, Optional ByVal v_sCaseNumber As String = "") As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: SelectTransQueryFiltered
        ' PURPOSE: Return array of transactions according to parameters
        ' AUTHOR: Peter Finney
        ' DATE: 26-Sep-02, 11:07 AM
        ' RETURNS: PMTrue for success
        ' CHANGES: Complete rebuild of sql and parameters
        '          AMB 11/02/2003 - changes for IAG PS220 Manage Debtors
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim sSQLWhere As String = ""
        Dim sSQLHaving As String = ""
        Dim sOrderBy As String = ""
        Dim sSQLSelectSpareType As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear parameters
            m_oDatabase.Parameters.Clear()

            m_lReturn = SelectTransQueryWhere(r_sSQL:=sSQLWhere, v_iCompanyID:=v_iCompanyID, v_vAccountID:=v_vAccountID, v_vDocumentRef:=v_vDocumentRef, v_vCurrencyID:=v_vCurrencyID, v_vCurrencyAmount:=v_vCurrencyAmount, v_vTolerance:=v_vTolerance, v_vDocTypeGroupId:=v_vDocTypeGroupId, v_vDocumentTypeID:=v_vDocumentTypeID, v_vPeriodID:=v_vPeriodID, v_vDateFrom:=v_vDateFrom, v_vDateTo:=v_vDateTo, v_vInsuranceRef:=v_vInsuranceRef, v_vUsername:=v_vUsername, v_vPurchaseInvoiceNo:=v_vPurchaseInvoiceNo, v_vPurchaseOrderNo:=v_vPurchaseOrderNo, v_vDepartment:=v_vDepartment, v_vSpare:=v_vSpare, v_vOutstandingOnly:=v_vOutstandingOnly, v_vCashListId:=v_vCashListId, v_vIsNewPF:=v_vIsNewPF, v_vInsuredAccountID:=v_vInsuredAccountID, v_bRollup:=v_bRollup, v_lDocumentID:=v_lDocumentID, v_vSourceArray:=v_vSourceArray, bIncludeReversedTrans:=v_bIncludeReversedTrans, v_iAccountTypeID:=v_iAccountTypeID, v_bInsuredAccountView:=v_bInsuredAccountView, v_sCaseNumber:=v_sCaseNumber)

            If v_bOrderBySpare Then
                sSQLSelectSpareType = ACTransFromQuerySelectSpareType
                sOrderBy = ACTransFromQueryOrderBySpare
            Else
                sSQLSelectSpareType = ""
                sOrderBy = ACTransFromQueryOrderBy
            End If

            ' The final sql statement
            '27/05/2003 - PWC - 186 - Debt Rollup
            If Not v_bRollup Then
                ' Add the filtering parameter
                AddWhereClause(sSQLWhere, "uga.has_unrestricted_enquiry", gPMConstants.PMEDataType.PMLong, 1)

                'PSL 01/07/2003 Different select for broking and underwriting

                sSQL = ACTransFilterGroups &
                       ACTransFromQuerySelectUnderwriting &
                       ACTransFilterFields &
                       sSQLSelectSpareType &
                       ACTransFromQueryFrom &
                       GetTransFromQueryAdditional() &
                       ACTransFromAdditionalSAMQuery &
                       ACTransFilterJoin &
                       sSQLWhere &
                       sOrderBy &
                       ACTransFilterCleanup
            Else
                ' Add the filtering parameter
                AddWhereClause(sSQLWhere, "@has_unrestricted_enquiry", gPMConstants.PMEDataType.PMLong, 1)

                'Get the having clause

                If SelectTransQueryHaving(r_sSQL:=sSQLHaving, v_vCurrencyAmount:=CDbl(v_vCurrencyAmount), v_vTolerance:=v_vTolerance) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Throw New System.Exception((ObjectError + m_lReturn).ToString() + ", [local], Failed: SelectTransQueryHaving")
                End If

                sSQL = ACRollupTransFilterGroups &
                       ACTransFilterCleanup

                If v_bDisplay500 Then
                    sSQL = sSQL & ACRollupTransFromQuerySelect500
                Else
                    sSQL = sSQL & ACRollupTransFromQuerySelect
                End If

                sSQL = sSQL &
                       ACRollupTransFromQuerySelectList &
                       ACRollupTransFilterFields &
                       sSQLSelectSpareType &
                       ACTransFromQueryFrom &
                       GetTransFromQueryAdditional() &
                       ACTransFromAdditionalSAMQuery &
                       sSQLWhere &
                       ACRollupTransFromQueryGroupBy &
                       sSQLHaving &
                       sOrderBy


                m_oDatabase.Parameters.Add(sName:="rollup_account_id", vValue:=CStr(v_vAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            ' AMB 11/02/2003: PS220 - added ACTransQueryAdditional

            With m_oDatabase
                .Parameters.Add(sName:="user_id", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                'developer guide no.40
                .Parameters.Add(sName:="effective_date", vValue:=DateTime.Today, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
                .Parameters.Add(sName:="language_id", vValue:=CStr(m_iLanguageID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                ' Call the procedure
                m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:=ACTransFromQueryName, bStoredProcedure:=ACTransFromQueryStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray)
            End With

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception((ObjectError + m_lReturn).ToString() + ", [local], Failed to execute query")
            End If

            ' Check for results
            If Not Informations.IsArray(r_vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="General failure in method", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectTransQueryFiltered", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

        Finally


        End Try
        Return result
    End Function

    Private Function SelectTransQueryWhere(ByRef r_sSQL As String, ByVal v_iCompanyID As Integer, ByVal v_vAccountID As Object, ByVal v_vDocumentRef As Object,
                                           ByVal v_vCurrencyID As Object, ByVal v_vCurrencyAmount As Double, ByVal v_vTolerance As Object, ByVal v_vDocTypeGroupId As Object,
                                           ByVal v_vDocumentTypeID As Object, ByVal v_vPeriodID As Object, ByVal v_vDateFrom As Object, ByVal v_vDateTo As Object,
                                           ByVal v_vInsuranceRef As Object, ByVal v_vUsername As Object, ByVal v_vPurchaseInvoiceNo As Object,
                                           ByVal v_vPurchaseOrderNo As Object, ByVal v_vDepartment As Object, ByVal v_vSpare As Object, ByVal v_vOutstandingOnly As String,
                                           ByVal v_vCashListId As Object, Optional ByVal v_vIsNewPF As Object = Nothing, Optional ByVal v_vInsuredAccountID As Object = Nothing,
                                           Optional ByVal v_bRollup As Boolean = False, Optional ByVal v_lDocumentID As Integer = 0,
                                           Optional ByVal v_lFinancePlanCnt As Integer = 0, Optional ByVal v_vUnderwritingYearID As Object = Nothing,
                                           Optional ByVal v_vSourceArray As Object = Nothing, Optional ByVal v_vTransDetailIDs As String = "",
                                           Optional ByVal v_sAltReference As String = "", Optional ByVal bIncludeReversedTrans As Boolean = False,
                                           Optional ByVal v_iAccountTypeID As Integer = 0, Optional ByVal v_bInsuredAccountView As Boolean = False,
                                           Optional ByVal v_sBGREf As String = "", Optional ByVal v_vDueDateFrom As Object = Nothing,
                                           Optional ByVal v_vDueDateTo As Object = Nothing, Optional ByVal v_sCaseNumber As String = "") As Integer
        Return SelectTransQueryWhere(r_sSQL:=r_sSQL, v_iCompanyID:=v_iCompanyID, v_vAccountID:=v_vAccountID, v_vDocumentRef:=v_vDocumentRef, v_vCurrencyID:=v_vCurrencyID,
                                v_vCurrencyAmount:=v_vCurrencyAmount, v_vTolerance:=v_vTolerance, v_vDocTypeGroupId:=v_vDocTypeGroupId,
                                v_vDocumentTypeID:=v_vDocumentTypeID, v_vPeriodID:=v_vPeriodID, v_vDateFrom:=v_vDateFrom, v_vDateTo:=v_vDateTo, v_vInsuranceRef:=v_vInsuranceRef,
                                v_vUsername:=v_vUsername, v_vPurchaseInvoiceNo:=v_vPurchaseInvoiceNo, v_vPurchaseOrderNo:=v_vPurchaseOrderNo, v_vDepartment:=v_vDepartment,
                                v_vSpare:=v_vSpare, v_vOutstandingOnly:=v_vOutstandingOnly, v_vCashListId:=v_vCashListId, v_vAgentCnt:=0, v_vIsNewPF:=v_vIsNewPF,
                                v_vInsuredAccountID:=v_vInsuredAccountID, v_bRollup:=v_bRollup, v_lDocumentID:=v_lDocumentID,
                                v_lFinancePlanCnt:=v_lFinancePlanCnt, v_vUnderwritingYearID:=v_vUnderwritingYearID, v_vSourceArray:=v_vSourceArray,
                                v_vTransDetailIDs:=v_vTransDetailIDs, v_sAltReference:=v_sAltReference, bIncludeReversedTrans:=bIncludeReversedTrans,
                                v_iAccountTypeID:=v_iAccountTypeID, v_bInsuredAccountView:=v_bInsuredAccountView, v_sBGREf:=v_sBGREf, v_vDueDateFrom:=v_vDueDateFrom,
                                v_vDueDateTo:=v_vDueDateTo, v_sCaseNumber:=v_sCaseNumber)

    End Function

    Private Function SelectTransQueryWhere(ByRef r_sSQL As String, ByVal v_iCompanyID As Integer, ByVal v_vAccountID As Object, ByVal v_vDocumentRef As Object,
                                         ByVal v_vCurrencyID As Object, ByVal v_vCurrencyAmount As Double, ByVal v_vTolerance As Object, ByVal v_vDocTypeGroupId As Object,
                                         ByVal v_vDocumentTypeID As Object, ByVal v_vPeriodID As Object, ByVal v_vDateFrom As Object, ByVal v_vDateTo As Object,
                                         ByVal v_vInsuranceRef As Object, ByVal v_vUsername As Object, ByVal v_vPurchaseInvoiceNo As Object,
                                         ByVal v_vPurchaseOrderNo As Object, ByVal v_vDepartment As Object, ByVal v_vSpare As Object, ByVal v_vOutstandingOnly As String,
                                         ByVal v_vCashListId As Object, ByVal v_vAgentCnt As Integer, Optional ByVal v_vIsNewPF As Object = Nothing, Optional ByVal v_vInsuredAccountID As Object = Nothing,
                                         Optional ByVal v_bRollup As Boolean = False, Optional ByVal v_lDocumentID As Integer = 0,
                                         Optional ByVal v_lFinancePlanCnt As Integer = 0, Optional ByVal v_vUnderwritingYearID As Object = Nothing,
                                         Optional ByVal v_vSourceArray As Object = Nothing, Optional ByVal v_vTransDetailIDs As String = "",
                                         Optional ByVal v_sAltReference As String = "", Optional ByVal bIncludeReversedTrans As Boolean = False,
                                         Optional ByVal v_iAccountTypeID As Integer = 0, Optional ByVal v_bInsuredAccountView As Boolean = False,
                                         Optional ByVal v_sBGREf As String = "", Optional ByVal v_vDueDateFrom As Object = Nothing,
                                         Optional ByVal v_vDueDateTo As Object = Nothing, Optional ByVal v_sCaseNumber As String = "") As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: SelectTransQueryWhere
        ' PURPOSE: Build base query according to parameters
        ' AUTHOR: Peter Finney
        ' DATE: 26-Sep-02, 11:07 AM
        ' RETURNS: PMTrue for success
        ' CHANGES: Complete rebuild of sql and parameters
        ' ---------------------------------------------------------------------------
        Dim result As Integer = 0
        Const kMultiCurrencyOption As Integer = 5058

        Dim sSQLWhere As String = ""
        Dim vValue As String = ""
        Dim dLower, dUpper As Double
        Dim sTempSQL As String = ""
        Dim lLower, lUpper As Integer
        Dim sTempStr As New StringBuilder
        Dim lCurrencyID As Integer

        ' will be used For Account type when Write off or Extra is Selected
        Dim lAccountIDWriteOff1, lAccountIDWriteOff2 As Integer
        Dim vResultArray(,) As Object = Nothing
        Dim sCurrency As New StringBuilder
        Dim bMultiCurrency As Boolean
        Dim sOptionValue As String = ""

        Dim sLinkedAccounts As New StringBuilder
        Dim bLinkedAccountsPresent As Boolean
        Dim vLinkedAccounts(,) As Object = Nothing

        result = gPMConstants.PMEReturnCode.PMTrue


        bMultiCurrency = False

        'Account Funcion & CCY Cash Allocation
        'This function retreive those currencies which are not specifically associated with branch via bankaccount
        'Plus the base currency. But available for multicurrency option

        m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=kMultiCurrencyOption, r_sOptionValue:=sOptionValue)

        If sOptionValue = "1" Then
            bMultiCurrency = True
        End If

        If bMultiCurrency Then
            If Not Informations.IsArray(v_vCurrencyID) Then
                If Not Informations.IsArray(v_vCurrencyID) Then
                    If (v_iCompanyID > 0) And (gPMFunctions.ToSafeLong(v_vCurrencyID) > 0) Then
                        lCurrencyID = gPMFunctions.ToSafeLong(v_vCurrencyID)
                        With m_oDatabase

                            .Parameters.Clear()
                            .Parameters.Add("Currency_Id", CStr(lCurrencyID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                            .Parameters.Add("Source_id", CStr(v_iCompanyID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                            m_lReturn = .SQLSelect(sSQL:=ACGetCurrecnyNotLinkedWithSourceSQL, sSQLName:=ACGetCurrecnyNotLinkedWithSourceName, bStoredProcedure:=ACGetCurrecnyNotLinkedWithSourceStored, vResultArray:=vResultArray)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Procedure Failed : " & ACGetCurrecnyNotLinkedWithSourceName, vApp:=ACApp, vClass:=ACClass, vMethod:="SelectTransQueryWhere ", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                                Return result
                            End If

                            If Informations.IsArray(vResultArray) Then

                                For iCnt As Integer = 0 To vResultArray.GetUpperBound(1)
                                    If iCnt = 0 Then
                                        sCurrency = New StringBuilder(gPMFunctions.ToSafeString(vResultArray(0, iCnt)))
                                    Else
                                        sCurrency.Append("," & gPMFunctions.ToSafeString(vResultArray(0, iCnt)))
                                    End If
                                Next
                                bMultiCurrency = True
                            Else
                                bMultiCurrency = False
                            End If
                        End With
                    End If
                End If
            End If
        End If

        'Get Linked Accounts
        If gPMFunctions.ToSafeInteger(v_vAccountID) > 0 Then
            m_lReturn = GetLinkedAccounts(v_lAccountID:=gPMFunctions.ToSafeInteger(v_vAccountID), r_vLinkedAccounts:=vLinkedAccounts)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("SelectTransQuerywhere", "Failed to Get Linked Accounts", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Informations.IsArray(vLinkedAccounts) Then
                sLinkedAccounts = New StringBuilder(CStr(v_vAccountID))

                For iCnt As Integer = 0 To vLinkedAccounts.GetUpperBound(1)
                    sLinkedAccounts.Append("," & CStr(vLinkedAccounts(0, iCnt)))
                Next

                bLinkedAccountsPresent = True
            Else
                bLinkedAccountsPresent = False
            End If
        End If

        ' Add restrictions by document
        AddWhereClause(sSQLWhere, "d.documenttype_id", gPMConstants.PMEDataType.PMLong, v_vDocumentTypeID)

        If v_lFinancePlanCnt <> 0 Then
            sTempSQL = ""
            sTempSQL = sTempSQL & "(" & Strings.ChrW(13) & Strings.ChrW(10)
            sTempSQL = sTempSQL & "    i.insurance_folder_cnt IS NULL" & Strings.ChrW(13) & Strings.ChrW(10)
            sTempSQL = sTempSQL & "    OR" & Strings.ChrW(13) & Strings.ChrW(10)
            sTempSQL = sTempSQL & "    i.insurance_folder_cnt IN" & Strings.ChrW(13) & Strings.ChrW(10)
            sTempSQL = sTempSQL & "        (" & Strings.ChrW(13) & Strings.ChrW(10)
            sTempSQL = sTempSQL & "            SELECT" & Strings.ChrW(13) & Strings.ChrW(10)
            sTempSQL = sTempSQL & "                i.insurance_folder_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sTempSQL = sTempSQL & "            FROM pftransaction_id t" & Strings.ChrW(13) & Strings.ChrW(10)
            sTempSQL = sTempSQL & "            JOIN insurance_file i" & Strings.ChrW(13) & Strings.ChrW(10)
            sTempSQL = sTempSQL & "                ON i.insurance_file_cnt = t.insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sTempSQL = sTempSQL & "            WHERE t.pfprem_finance_cnt = " & CStr(v_lFinancePlanCnt) & Strings.ChrW(13) & Strings.ChrW(10)
            sTempSQL = sTempSQL & "            AND t.insurance_file_cnt IS NOT NULL" & Strings.ChrW(13) & Strings.ChrW(10)
            sTempSQL = sTempSQL & "        )" & Strings.ChrW(13) & Strings.ChrW(10)
            sTempSQL = sTempSQL & ")" & Strings.ChrW(13) & Strings.ChrW(10)

            AddWhereClause(sSQLWhere, sTempSQL, gPMConstants.PMEDataType.PMCode)
        End If


        'DD 21/08/2003: Add the document ID for better document drill-down.
        If v_lDocumentID <> 0 Then
            AddWhereClause(sSQLWhere, "d.document_id", gPMConstants.PMEDataType.PMLong, v_lDocumentID)
        Else
            If AddWhereClause(sSQLWhere, "d.document_ref", gPMConstants.PMEDataType.PMString, v_vDocumentRef) Then
                ' Only filter company if specifically passed
                If m_bDrillCompany Then
                    AddWhereClause(sSQLWhere, "d.company_id", gPMConstants.PMEDataType.PMString, v_iCompanyID)
                End If
            End If
        End If

        If m_bRestrictByCompany Then
            'RDT PN 18041 - Removed the Else clause so that the search returns all branches
            '               regardless of whether or not the branch is closed or not.
            'VB 01/03/2005 PN19032 - Else part added for search all branch Transactions that are related to
            '                        branches, those are accessible by logged user.
            'VB 15/03/2005 Else part removed, search clause on branch will only be
            'included in case m_bRestrictByCompany is set to True

            'v_vSourceArray will supercede the v_iCompanyID parameter inclusion in where clause

            If Not Informations.IsNothing(v_vSourceArray) Then
                If Informations.IsArray(v_vSourceArray) Then

                    lLower = v_vSourceArray.GetLowerBound(1)

                    lUpper = v_vSourceArray.GetUpperBound(1)
                    For lSub As Integer = lLower To lUpper

                        'developer guide no 162
                        sTempStr.Append("," & CStr(v_vSourceArray(0, lSub)))
                    Next lSub
                    sTempStr = New StringBuilder(sTempStr.ToString().Substring(sTempStr.ToString().Length - (sTempStr.ToString().Length - 1)))
                    AddWhereClause(sSQLWhere, "d.company_id", gPMConstants.PMEDataType.PMString, sTempStr.ToString(), "IN")
                Else
                    AddWhereClause(sSQLWhere, "d.company_id", gPMConstants.PMEDataType.PMString, v_vSourceArray)
                End If
            Else
                AddWhereClause(sSQLWhere, "d.company_id", gPMConstants.PMEDataType.PMString, v_iCompanyID)
            End If
        End If

        ' Add restrictions by documenttype
        AddWhereClause(sSQLWhere, "dt.doctypegroup_id", gPMConstants.PMEDataType.PMLong, v_vDocTypeGroupId)

        ' Add restrictions by user
        AddWhereClause(sSQLWhere, "pmu.username", gPMConstants.PMEDataType.PMString, v_vUsername)

        ' Add restrictions by transdetail
        If bMultiCurrency Then
            AddWhereClause(sSQLWhere, "t.currency_id", gPMConstants.PMEDataType.PMString, sCurrency.ToString(), "IN")
        Else
            AddWhereClause(sSQLWhere, "t.currency_id", gPMConstants.PMEDataType.PMLong, v_vCurrencyID)
        End If
        AddWhereClause(sSQLWhere, "t.purchase_order_no", gPMConstants.PMEDataType.PMString, v_vPurchaseOrderNo)
        AddWhereClause(sSQLWhere, "t.purchase_invoice_no", gPMConstants.PMEDataType.PMString, v_vPurchaseInvoiceNo)
        AddWhereClause(sSQLWhere, "t.department", gPMConstants.PMEDataType.PMString, v_vDepartment)
        If bIncludeReversedTrans Then
            AddWhereClause(sSQLWhere, "Isnull(t.spare,'')", gPMConstants.PMEDataType.PMString, v_vSpare)
        Else
            AddWhereClause(sSQLWhere, "(t.spare NOT LIKE 'Revers%' OR fully_matched<>1)", gPMConstants.PMEDataType.PMCode)
            AddWhereClause(sSQLWhere, "Isnull(t.spare,'')", gPMConstants.PMEDataType.PMString, v_vSpare, " LIKE ")
        End If

        AddWhereClause(sSQLWhere, "t.period_id", gPMConstants.PMEDataType.PMLong, v_vPeriodID)
        AddWhereClause(sSQLWhere, "t.accounting_date", gPMConstants.PMEDataType.PMDate, v_vDateFrom, ">=")
        AddWhereClause(sSQLWhere, "t.accounting_date", gPMConstants.PMEDataType.PMDate, v_vDateTo, "<=")

        If Not m_bIsSingleInstalmentPlan Then
            AddWhereClause(sSQLWhere, "t.insurance_ref", gPMConstants.PMEDataType.PMString, v_vInsuranceRef)
        End If
        'Start - Prakash - WPR85 Parelleling
        'AddWhereClause sSQLWhere, "t.account_id", PMLOng, v_vAccountID
        If bLinkedAccountsPresent Then
            AddWhereClause(sSQLWhere, "t.account_id", gPMConstants.PMEDataType.PMString, sLinkedAccounts.ToString(), "IN")
        Else
            AddWhereClause(sSQLWhere, "t.account_id", gPMConstants.PMEDataType.PMLong, v_vAccountID)
        End If
        'End - Prakash - WPR85 Parelleling

        'DD 11/04/2003: Added for PS220.
        AddWhereClause(sSQLWhere, "acc2.account_id", gPMConstants.PMEDataType.PMLong, v_vInsuredAccountID)
        '11/08/2008 : Added by Sankar to include BGRef
        AddWhereClause(sSQLWhere, "Bank_Guarantee.BG_ref", gPMConstants.PMEDataType.PMString, v_sBGREf)

        If Not Informations.IsNothing(v_vInsuredAccountID) And CInt(v_vInsuredAccountID) > 0 And v_bInsuredAccountView Then
            AddWhereClause(sSQLWhere, " (a.ledger_id = 2 or a.ledger_id=5) ", gPMConstants.PMEDataType.PMCode)
        End If

        ' Filter by company Id if Multibranch accounting enabled
        bPMFunc.getProductOptionValue(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, m_iSourceID, vValue)

        If gPMFunctions.NullToString(vValue) = "1" Then
            AddWhereClause(sSQLWhere, "t.company_id", gPMConstants.PMEDataType.PMLong, m_iSourceID)
        End If

        ' Filter by company Id if Multibranch accounting enabled
        bPMFunc.getProductOptionValue(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, gPMConstants.SIRHiddenOptions.SIROPTAccountSegregation, m_iSourceID, vValue)

        If gPMFunctions.NullToString(vValue) = "1" Then
            AddWhereClause(sSQLWhere, "(t.company_id NOT IN (SELECT source_id FROM pmuser_source WHERE user_id = " & m_iUserID & "))", gPMConstants.PMEDataType.PMCode)
        End If

        sOptionValue = String.Empty
        m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=gPMConstants.PMSysOptionRestricteduserbranchOption, r_sOptionValue:=sOptionValue)

        If sOptionValue = "1" Then
            'AddWhereClause(sSQLWhere, " t.company_id IN(SELECT s.source_id  FROM Source s left join PMUser_Source u ON s.source_id=u.source_id   AND u.user_id=" & m_iUserID & "  WHERE u.source_id is	null)", gPMConstants.PMEDataType.PMCode)
            '31779 - System account to exclude branch access system option
            Dim sSqlBranchAccessQuery = ""
            sSqlBranchAccessQuery = " ((t.company_id IN(SELECT s.source_id  FROM Source s left join PMUser_Source u ON s.source_id=u.source_id   And u.user_id = " & m_iUserID & " "
            sSqlBranchAccessQuery = sSqlBranchAccessQuery + " WHERE u.source_id Is null) )"
            sSqlBranchAccessQuery = sSqlBranchAccessQuery + " Or  (t.account_id in ( select account_id from account where a.short_code in (select value from system_options where option_number in(150,151,152,153,5028,5061)))))"
            AddWhereClause(sSQLWhere, sSqlBranchAccessQuery, gPMConstants.PMEDataType.PMCode)
        End If

        '27/05/2003 - PWC - 186 - Debt Rollup
        'Remove the filter on currency from the WHERE clause.  We will add it
        'to a HAVING clause at the end
        If Not v_bRollup Then
            'Changes as per Vb code
            'If Not False Then
            If Not Informations.IsNothing(v_vCurrencyAmount) Then
                Select Case v_vCurrencyAmount
                    Case 0.01
                        AddWhereClause(sSQLWhere, "(t.currency_amount>=" & v_vCurrencyAmount & ")", gPMConstants.PMEDataType.PMCode)
                    Case -0.01
                        AddWhereClause(sSQLWhere, "(t.currency_amount<=" & v_vCurrencyAmount & ")", gPMConstants.PMEDataType.PMCode)
                    Case 0
                        ' Not needed, just speeds up the select as with all hard coded values
                        ' it can use a jump table rather than effective if, elseif's
                    Case Else
                        If gPMFunctions.NullToLong(v_vTolerance) > 0 Then
                            ' Get tolerances

                            dLower = v_vCurrencyAmount - (v_vCurrencyAmount * (CDbl(v_vTolerance) / 100))

                            dUpper = v_vCurrencyAmount + (v_vCurrencyAmount * (CDbl(v_vTolerance) / 100))

                            ' Add in correct order (lowest must be first!!!)
                            If dLower < dUpper Then
                                AddWhereClause(sSQLWhere, "(t.currency_amount BETWEEN " & dLower & " AND " & CStr(dUpper) & ")", gPMConstants.PMEDataType.PMCode)
                            Else
                                AddWhereClause(sSQLWhere, "(t.currency_amount BETWEEN " & dUpper & " AND " & CStr(dLower) & ")", gPMConstants.PMEDataType.PMCode)
                            End If
                        Else
                            AddWhereClause(sSQLWhere, "(round(t.currency_amount,2)=" & v_vCurrencyAmount & ")", gPMConstants.PMEDataType.PMCode)
                        End If
                End Select
            End If
        End If

        ' Check for restricted enquiries
        If m_iHasUnrestrictedEnquiry = 0 Then
            AddWhereClause(sSQLWhere, "(a.restrict_enquiry = 0)", gPMConstants.PMEDataType.PMCode)
        End If

        ' If we only want outstanding items add the restriction

        If Not Informations.IsNothing(v_vOutstandingOnly) Then
            If v_vOutstandingOnly <> "0" Then
                Dim sCommissionTaxClause As String
                If v_bRollup Then
                    sCommissionTaxClause = "((t.outstanding_amount <> 0) OR (t.amount = 0 AND t.spare = 'COMM' AND t.amount_updated IS NULL)"
                    sCommissionTaxClause = sCommissionTaxClause & " OR (t.outstanding_amount = 0 AND EXISTS (SELECT NULL FROM TransDetail tx WHERE tx.document_id = T.document_id AND tx.account_id = T.account_id and tx.outstanding_amount > 0)))"
                Else
                    sCommissionTaxClause = "((t.outstanding_amount <> 0) OR (t.amount = 0 AND t.spare = 'COMM' AND t.amount_updated IS NULL))"
                End If

                AddWhereClause(sSQLWhere, sCommissionTaxClause, gPMConstants.PMEDataType.PMCode)

            End If
        End If

        If Not Informations.IsNothing(v_vAgentCnt) Then
            If v_vAgentCnt <> 0 Then
                If Not (Informations.IsNothing(v_vInsuranceRef) OrElse String.IsNullOrEmpty(v_vInsuranceRef)) AndAlso Not (Informations.IsNothing(v_vAccountID) OrElse v_vAccountID = 0) _
                    OrElse ((Informations.IsNothing(v_vInsuranceRef) OrElse String.IsNullOrEmpty(v_vInsuranceRef)) AndAlso v_vAccountID = 0) Then
                    AddWhereClause(sSQLWhere, "EXISTS (select NULL from insurance_file ifl where ifl.insurance_ref= t.insurance_ref and ifl.lead_agent_cnt = " & CStr(v_vAgentCnt) & Strings.ChrW(13) & Strings.ChrW(10) & ")", gPMConstants.PMEDataType.PMCode)
                End If
            End If
        End If
        If Not Informations.IsNothing(v_vIsNewPF) Then
            If v_vIsNewPF Then
                AddWhereClause(sSQLWhere, "(t.transdetail_id NOT IN (SELECT pftransaction_id FROM pftransaction_id ))", gPMConstants.PMEDataType.PMCode)
                AddWhereClause(sSQLWhere, "(dt.code  IN ('SND','SRD','SED','SEC','SNC','SRC','SHC','SHD','TRC','TRD','FEE','SID','SIC'))", gPMConstants.PMEDataType.PMCode)
            End If
        End If

        'PSL 24/06/2003 Iss4919 added cashLIst ID so it only excludes this cash list, not all of them
        If Not False And CInt(v_vCashListId) > 0 Then

            AddWhereClause(sSQLWhere, CStr(v_vCashListId) &
                           " NOT IN" &
                           " (SELECT cl.cashlist_id FROM cashlist cl" &
                           " JOIN cashlistitem cli" &
                           " ON cl.cashlist_id = cli.cashlist_id" &
                           " WHERE cli.transdetail_id = t.transdetail_id AND cli.cashlist_id = " & CStr(v_vCashListId) & ")", gPMConstants.PMEDataType.PMCode)
        End If

        AddWhereClause(sSQLWhere, "t.underwriting_year_id", gPMConstants.PMEDataType.PMLong, v_vUnderwritingYearID)

        If Not Informations.IsNothing(v_vDueDateFrom) Then
            AddWhereClause(sSQLWhere, "t.due_date", gPMConstants.PMEDataType.PMDate, v_vDueDateFrom, ">=")
        End If

        If Not Informations.IsNothing(v_vDueDateTo) Then
            AddWhereClause(sSQLWhere, "t.due_date", gPMConstants.PMEDataType.PMDate, v_vDueDateTo, "<=")
        End If

        If Not Informations.IsNothing(v_vTransDetailIDs) Then
            If v_vTransDetailIDs.Trim().Length > 0 Then
                AddWhereClause(sSQLWhere, "(t.transdetail_id IN (" & v_vTransDetailIDs & "))", gPMConstants.PMEDataType.PMCode)
            End If
        End If

        If Not False And v_iAccountTypeID > 0 Then
            If v_iAccountTypeID = ACAccountTypeWriteOff Then
                m_lReturn = GetAccountID(v_sShortCode:=ACAccountCodeWriteOff1, r_lAccountID:=lAccountIDWriteOff1)
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = GetAccountID(v_sShortCode:=ACAccountCodeWriteOff2, r_lAccountID:=lAccountIDWriteOff2)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("SelectTransQuerywhere", "Failed to Get Write off AccountID", gPMConstants.PMELogLevel.PMLogError)
                    End If
                    AddWhereClause(sSQLWhere, "(a.account_id in (" & lAccountIDWriteOff1 & "," & CStr(lAccountIDWriteOff2) & " ))", gPMConstants.PMEDataType.PMCode)
                End If
            ElseIf v_iAccountTypeID = ACAccountTypeInsurer Then
                AddWhereClause(sSQLWhere, "(a.ledger_id = (" & v_iAccountTypeID & ")) and (party_type.party_type_id = " & CStr(ACPartyTypeInsurer) & ")", gPMConstants.PMEDataType.PMCode)
            ElseIf v_iAccountTypeID = ACAccountTypeExtra Then
                AddWhereClause(sSQLWhere, "(party_type.party_type_id = " & ACPartyTypeExtra & ")", gPMConstants.PMEDataType.PMCode)
            ElseIf v_iAccountTypeID <> ACAccountTypeExtra And v_iAccountTypeID <> ACAccountTypeWriteOff Then
                AddWhereClause(sSQLWhere, "(a.ledger_id = (" & v_iAccountTypeID & "))", gPMConstants.PMEDataType.PMCode)
            End If
        End If

        '(RC) QBENZ014
        If Not Informations.IsNothing(v_sAltReference) And gPMFunctions.ToSafeString(v_sAltReference, "") <> "" Then

            If Informations.Left(v_vDocumentRef, 1) <> "C" Then
                AddWhereClause(sSQLWhere, "(t.reference  ", gPMConstants.PMEDataType.PMString, v_sAltReference)
                AddWhereClause(sSQLWhere, " d.Document_ref Not Like 'C%')", gPMConstants.PMEDataType.PMCode)
            Else
                AddWhereClause(sSQLWhere, "(t.reference  )", gPMConstants.PMEDataType.PMString, v_sAltReference)
            End If
        End If

        If v_bInsuredAccountView Then
            AddWhereClause(sSQLWhere, " d.Document_ref Not Like 'C%' ", gPMConstants.PMEDataType.PMCode)
        End If

        m_lReturn = CType(bPMFunc.ValidateSQL(v_sCaseNumber), gPMConstants.PMEReturnCode)

        If v_sCaseNumber <> "" Then
            AddWhereClause(sSQLWhere, "([case].Case_number='" & v_sCaseNumber & "')", gPMConstants.PMEDataType.PMCode)
        End If

        ' Build the final sql statement
        r_sSQL = sSQLWhere

        Return result



    End Function


    Private Function SelectTransQueryHaving(ByRef r_sSQL As String, Optional ByVal v_vCurrencyAmount As Double = 0, Optional ByVal v_vTolerance As Object = Nothing) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: SelectTransQueryHaving
        ' PURPOSE: Build base query according to parameters
        ' AUTHOR: Peter Finney
        ' DATE: 26-Sep-02, 11:07 AM
        ' RETURNS: PMTrue for success
        ' CHANGES: Complete rebuild of sql and parameters
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim sSQLHaving As String = ""
        Dim dLower, dUpper As Double

        result = gPMConstants.PMEReturnCode.PMTrue

        If Not Informations.IsNothing(v_vCurrencyAmount) Then
            Select Case v_vCurrencyAmount
                Case 0.01
                    AddHavingClause(sSQLHaving, "(SUM(t.currency_amount) >=" & v_vCurrencyAmount & ")", gPMConstants.PMEDataType.PMCode)
                Case -0.01
                    AddHavingClause(sSQLHaving, "(SUM(t.currency_amount) <=" & v_vCurrencyAmount & ")", gPMConstants.PMEDataType.PMCode)
                Case 0
                    ' Not needed, just speeds up the select as with all hard coded values
                    ' it can use a jump table rather than effective if, elseif's
                Case Else
                    If gPMFunctions.NullToLong(v_vTolerance) > 0 Then
                        ' Get tolerances

                        dLower = v_vCurrencyAmount - (v_vCurrencyAmount * (CDbl(v_vTolerance) / 100))

                        dUpper = v_vCurrencyAmount + (v_vCurrencyAmount * (CDbl(v_vTolerance) / 100))

                        ' Add in correct order (lowest must be first!!!)
                        If dLower < dUpper Then
                            AddHavingClause(sSQLHaving, "(SUM(t.currency_amount) BETWEEN " & dLower & " AND " & CStr(dUpper) & ")", gPMConstants.PMEDataType.PMCode)
                        Else
                            AddHavingClause(sSQLHaving, "(SUM(t.currency_amount) BETWEEN " & dUpper & " AND " & CStr(dLower) & ")", gPMConstants.PMEDataType.PMCode)
                        End If
                    Else
                        AddHavingClause(sSQLHaving, "(SUM(t.currency_amount) =" & v_vCurrencyAmount & ")", gPMConstants.PMEDataType.PMCode)
                    End If
            End Select
        End If


        ' Build the final sql statement
        r_sSQL = sSQLHaving


        Return result

    End Function


    ' ***************************************************************** '
    ' Name: CheckResults (Private)
    '
    ' Description: Checks the result array after a query
    '              If records found returns PMTrue
    '              If no records found returns PMNotFound
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (CheckResults) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CheckResults(ByRef vResultArray As Object) As Integer
    '
    'Dim result As Integer = 0
    'Dim iCount2 As Integer
    'Dim iCount1 As Integer
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' If NO records were found return PMNotFound
    'If Not informations.IsArray(vResultArray) Then
    'result = gPMConstants.PMEReturnCode.PMNotFound
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error Section.
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    'MessageBox.Show(excep.Message, Application.ProductName)
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckResults Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckResults", vErrNo:=informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function
    ' PRIVATE Methods (End)


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
        ' Error Section.
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(informations.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    '
    ' Name: GetAllocationDetails
    '
    ' Description: Pass call through to the allocation calculate object.
    '
    ' History: 29/10/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function GetAllocationDetails(ByVal v_lAllocationId As Integer) As Integer

        Dim result As Integer = 0
        Try



            Return m_oAllocationCalculate.GetAllocationDetails(v_lAllocationId:=v_lAllocationId)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllocationDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllocationDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: IsTransInAllocation
    '
    ' Description: Pass call through to the allocation calculate object.
    '
    ' History: 29/10/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function IsTransInAllocation(ByVal v_lTransactionId As Integer) As Integer

        Dim result As Integer = 0
        Try



            Return m_oAllocationCalculate.IsTransInAllocation(v_lTransactionId:=v_lTransactionId)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsTransInAllocation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsTransInAllocation", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetPeriodForDate
    '
    ' Description: Pass the call through to the period object.
    '
    ' History: 29/10/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function GetPeriodForDate(ByRef dtDateInPeriod As Date, ByRef lPeriodId As Integer, ByRef vYearName As Object) As Integer

        Dim result As Integer = 0
        Try



            Return m_oPeriod.GetPeriodForDate(dtDateInPeriod:=dtDateInPeriod, lPeriodID:=lPeriodId, vYearName:=vYearName)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPeriodForDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPeriodForDate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetDetails
    '
    ' Description: Pass the call through to the period object.
    '
    ' History: 29/10/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function GetDetails(ByRef vYearName As Object) As Integer

        Dim result As Integer = 0
        Try



            Return m_oPeriod.GetDetails(vYearName:=vYearName)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetNext
    '
    ' Description: Pass the call through to the period object.
    '
    ' History: 29/10/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function GetNext(ByRef vPeriodId As Object, ByRef vCompanyID As Object, ByRef vYearName As Object, ByRef vPeriodName As Object, ByRef vPeriodEndDate As Object) As Integer

        Dim result As Integer = 0
        Try



            Return m_oPeriod.GetNext(vPeriodID:=vPeriodId, vCompanyID:=vCompanyID, vYearName:=vYearName, vPeriodName:=vPeriodName, vPeriodEndDate:=vPeriodEndDate)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNext", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetAccountDetails
    '
    ' Description: Pass the call through to the account object.
    '
    ' History: 29/10/1999 CTAF - Created.
    '
    ' eck180901 add extra return parameter for Account Currency
    ' AMB 12/02/2003 - PS220 - Added ledger_id
    ' ***************************************************************** '
    Public Function GetAccountDetails(ByRef r_lAccountID As Integer, ByRef sAccountName As String, ByRef sContactName As String, ByRef sPhoneAreaCode As String, ByRef sPhoneNumber As String, ByRef sPhoneExtension As String, ByRef r_vdAccountBalance As Object, ByRef r_sAccountCode As String, ByRef r_vlAccountCurrencyId As Integer, Optional ByVal v_dtAccountingDate As Object = Nothing, Optional ByRef r_lLedgerID As Integer = 0, Optional ByRef r_sLedgerShortName As String = "", Optional ByRef r_vdAccountDebt As Object = Nothing, Optional ByRef r_vdInstalmentDebt As Object = Nothing, Optional ByVal v_bCalculateAccountBalance As Boolean = True) As Integer
        'MKW110403 PN3414 Added Return values r_lLedgerID and r_sLedgerShortName


        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing
        Dim vAccountingDate As Date
        ' AMB 12/02/2003 - PS220 - Added ledger_id

        Try

            'DD 03/06/2003: Recoded into a Stored Procedure Call
            ' Get the details of the account
            'sSQL$ = "SELECT a.account_name, a.contact_name, " & _
            '"a.phone_area_code, a.phone_number, " & _
            '"a.phone_extension, acs.code, " & _
            '"a.ledger_id, l.ledger_short_name " & _
            '"FROM Account a " & _
            '"INNER JOIN Ledger l ON l.ledger_id=a.ledger_id " & _
            '"INNER JOIN AccountStatus acs ON acs.accountstatus_id=a.accountstatus_id " & _
            '"WHERE account_id = " & CStr(r_lAccountID&)
            ' AMB 12/02/2003 - PS220 - Added ledger_id

            With m_oDatabase
                .Parameters.Clear()

                .Parameters.Add("AccountID", CStr(r_lAccountID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                m_lReturn = .SQLSelect(sSQL:=ACGetAccountDetailsSQL, sSQLName:=ACGetAccountDetailsName, bStoredProcedure:=ACGetAccountDetailsStored, vResultArray:=vResultArray)

                If (Not Informations.IsArray(vResultArray)) Or (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SQL Failed : " & sSQL, vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If
            End With


            sAccountName = gPMFunctions.ToSafeString(vResultArray(0, 0))

            sContactName = gPMFunctions.ToSafeString(vResultArray(1, 0))

            sPhoneAreaCode = gPMFunctions.ToSafeString(vResultArray(2, 0))

            sPhoneNumber = gPMFunctions.ToSafeString(vResultArray(3, 0))

            sPhoneExtension = gPMFunctions.ToSafeString(vResultArray(4, 0))

            r_sAccountCode = gPMFunctions.ToSafeString(vResultArray(5, 0))
            ' AMB 12/02/2003 - PS220 - Added ledger_id

            r_lLedgerID = gPMFunctions.ToSafeInteger(vResultArray(6, 0))

            r_sLedgerShortName = gPMFunctions.ToSafeString(vResultArray(7, 0))

            If v_dtAccountingDate.Equals(Date.MinValue) Then
                vAccountingDate = DateTime.Today
            Else
                vAccountingDate = v_dtAccountingDate
            End If

            If v_bCalculateAccountBalance Then
                'eck180901 Return Currency Id

                m_lReturn = m_oAccount.GetAccountBalance(r_vdAccountBalance:=r_vdAccountBalance, v_vAccountID:=r_lAccountID, v_vAccountingDate:=vAccountingDate, r_vlAccountCurrencyId:=r_vlAccountCurrencyId)

                r_vdAccountDebt = 0
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            ' KG 08/07/03 - Only if r_vdInstalmentDebt is passed thru'

            If Not Informations.IsNothing(r_vdInstalmentDebt) Then
                r_vdInstalmentDebt = 0
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetAccountID
    '
    ' Description:
    '
    ' History: 05/01/2000 CTAF - Created.
    '
    ' ECK PN6169 Need to use company id as well
    '
    ' ***************************************************************** '
    Public Function GetAccountID(ByVal v_sShortCode As String, ByRef r_lAccountID As Integer, Optional ByVal sLedgerCode As String = "") As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim sSQL As String = ""
        Dim vValue As String = ""
        Dim sOptionValue As String = String.Empty

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ' Filter by Resticted branch
            m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=gPMConstants.PMSysOptionRestricteduserbranchOption, r_sOptionValue:=sOptionValue)

            ' Filter by company Id if Multibranch accounting enabled
            bPMFunc.getProductOptionValue(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, m_iSourceID, vValue)

            sSQL = "SELECT account_id FROM Account WHERE short_code = '" & v_sShortCode & "'"

            If sOptionValue = "1" Then
                'sSQL = sSQL & " AND  company_id IN (SELECT S.Source_id FROM Source S LEFT JOIN PMUSER_Source U " & Strings.ChrW(13) & Strings.ChrW(10)
                'sSQL = sSQL & " ON S.source_id = U.source_id AND U.user_id = " & CStr(m_iUserID) & Strings.ChrW(13) & Strings.ChrW(10)
                'sSQL = sSQL & " WHERE U.source_id IS NULL) "

                '31779 - System account to exclude branch access system option
                sSQL = sSQL & " AND ((  company_id IN (SELECT S.Source_id FROM Source S LEFT JOIN PMUSER_Source U " & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & " ON S.source_id = U.source_id AND U.user_id = " & CStr(m_iUserID) & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & " WHERE U.source_id IS NULL) )"
                sSQL = sSQL & " OR (short_code in (select value from system_options where option_number in(150,151,152,153,5028,5061))))"
            End If

            If gPMFunctions.ToSafeString(sLedgerCode).Trim.ToUpper = "CLIENT" Then
                sSQL = sSQL & "AND (a.ledger_id = 2 ) "
            End If

            'Only do this for Multi-Company
            If gPMFunctions.NullToString(vValue) = "1" Then
                'eck PN6169
                sSQL = sSQL & " AND company_id = " & CStr(m_iSourceID)
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetAccountID", bStoredProcedure:=False, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountID", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            If Not Informations.IsArray(vResultArray) Then

            Else

                r_lAccountID = gPMFunctions.ToSafeInteger(vResultArray(0, 0))
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: GetFinancePlanDetails
    '
    ' Description:
    '
    ' History: PW271102 - created (PS209)
    '
    ' ***************************************************************** '
    Public Function GetFinancePlanDetails(ByVal v_sDocumentReference As String, ByVal v_lCompanyID As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add parameters
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="document_ref", vValue:=v_sDocumentReference, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=CStr(v_lCompanyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute the stored procedure
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetFinancePlanDetailsSQL, sSQLName:=ACGetFinancePlanDetailsName, bStoredProcedure:=ACGetFinancePlanDetailsStored, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetFinancePlanDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFinancePlanDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: GetAccountAmounts
    '
    ' Description: Return the transdetail.amount and currency_match_amount
    '              for a given account_id.
    '
    ' History: AMB 18/02/2003: PS220 - created for Manage Debtors development
    '
    ' ***************************************************************** '
    Public Function GetAccountAmounts(ByVal v_lAccountID As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add parameters
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="account_id", vValue:=CStr(v_lAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute the stored procedure
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAccountAmountsSQL, sSQLName:=ACGetAccountAmountsName, bStoredProcedure:=ACGetAccountAmountsStored, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountAmounts Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountAmounts", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function




    ' ***************************************************************** '
    '
    ' Name: GetAccountCodeFromID
    '
    ' Description:
    '
    ' History: 05/01/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function GetAccountCodeFromID(ByRef r_sShortCode As String, ByVal v_lAccountID As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT short_code FROM Account WHERE account_id = " & v_lAccountID

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetAccountCode", bStoredProcedure:=False, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vResultArray) Then

                r_sShortCode = gPMFunctions.ToSafeString(vResultArray(0, 0))
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountCodeFromI Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountCodeFromID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: GetAccountKeyFromId
    '
    ' Description:
    '
    ' History: 05/01/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function GetAccountKeyFromId(ByRef r_lAccountKey As Integer, ByVal v_lAccountID As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT account_key FROM Account WHERE account_id = " & v_lAccountID

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetAccountKey", bStoredProcedure:=False, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vResultArray) Then

                r_lAccountKey = gPMFunctions.ToSafeInteger(vResultArray(0, 0))
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountCodeFromI Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountKeyFromId", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function ViewAllocationDetails(ByVal v_lTransDetailId As Integer, ByRef r_vResultArray(,) As Object) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: ViewAllocationDetails
        ' PURPOSE: Get allocation details for a transaction
        ' AUTHOR: Paul Cunnigham
        ' DATE: 18 November 2002, 10:38:52
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_oDatabase.Parameters.Clear()

            ' Add the ComponentNameID INPUT parameter
            If m_oDatabase.Parameters.Add(sName:="lTransDetailId", vValue:=CStr(v_lTransDetailId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            ' Execute SQL Statement
            If m_oDatabase.SQLSelect(sSQL:=ACSelectViewAllocationSQL, sSQLName:=ACSelectViewAllocationName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            result = gPMConstants.PMEReturnCode.PMTrue


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="ViewAllocationDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

        Finally



        End Try
        Return result
    End Function

    Public Function GetRollupTransactions(ByVal v_lDocumentID As Integer, ByVal v_lAccountID As Integer, ByRef r_vResultArray(,) As Object) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetRollupTransactions
        ' PURPOSE: Get allocation details for a rolled up document
        ' AUTHOR: Paul Cunnigham
        ' DATE: 18 November 2002, 10:38:52
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Const ACMethod As String = "GetRollupTransactions"


        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            With m_oDatabase
                .Parameters.Clear()

                'Add parameters..

                If .Parameters.Add(sName:="lDocumentId", vValue:=CStr(v_lDocumentID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Throw New System.Exception(ObjectError.ToString() + ", " + ACMethod + ", Unable to add parameter: @lDocumentId")
                End If

                If .Parameters.Add(sName:="lAccountId", vValue:=CStr(v_lAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Throw New System.Exception(ObjectError.ToString() + ", " + ACMethod + ", Unable to add parameter: @lAccountId")
                End If

                ' Execute SQL Statement
                If m_oDatabase.SQLSelect(sSQL:=ACSelectRollupTransactionSQL, sSQLName:=ACSelectRollupTransactionName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then

                End If

            End With

            result = gPMConstants.PMEReturnCode.PMTrue


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            Select Case Informations.Err().Number
                Case ObjectError
                    ' Log internal failure.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Informations.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:=Informations.Err().Source, excep:=ex)

                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACMethod & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

            End Select

        Finally



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetRegSettings
    '
    ' Description: Pass the call through to the solution config object.
    '
    ' History: 29/10/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function GetRegSettings(ByRef sResult As String, ByRef sAppName As String, ByRef sSection As String, ByRef sKey As String, ByRef vDefault As Object) As Integer

        Dim result As Integer = 0
        Try


            Return m_oSolutionConfig.GetRegSettings(sResult:=sResult, sAppName:=sAppName, sSection:=sSection, sKey:=sKey, vDefault:=vDefault)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRegSettings Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRegSettings", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UnderwritingOrAgency
    '
    ' Description:  Finds out if Underwriting or Agency business
    '
    ' ***************************************************************** '
    Private Function getUnderwritingOrAgency() As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim oDatabase As dPMDAO.Database = Nothing

        result = gPMConstants.PMEReturnCode.PMTrue

        If gPMComponentServices.NewDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, r_oDatabase:=oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_sUnderwritingOrAgency = "A"

        m_oDatabase.Parameters.Clear()

        sSQL = "SELECT value FROM hidden_options WHERE option_number = 1"

        m_lReturn = oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetHiddenOption", bStoredProcedure:=False)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
            ' Carry on without default set
        End If

        If oDatabase.Records.Count() = 1 Then
            ' select first letter of the return field
            'developer guide no.111
            m_sUnderwritingOrAgency = gPMFunctions.NullToString(oDatabase.Records.Item(0).Fields()("value")).Substring(0, 1)
        End If

        If (m_sUnderwritingOrAgency <> "A") And (m_sUnderwritingOrAgency <> "U") Then
            m_sUnderwritingOrAgency = "A"
        End If

        m_lReturn = oDatabase.CloseDatabase()

        oDatabase = Nothing

        Return result

    End Function


    Private Function AddWhereClause(ByRef r_sSQLWhere As String, ByVal v_sField As String, ByVal v_lDatatype As gPMConstants.PMEDataType, Optional ByVal v_vValue As Object = Nothing, Optional ByVal v_sOperator As String = "=", Optional ByVal v_bAllowEmpty As Boolean = False) As Boolean
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: AddWhereClause
        ' PURPOSE: Add a where clause to the current string. Used by SelectTransQuery
        ' AUTHOR: Peter Finney
        ' DATE: 26-Sep-02, 11:28 AM
        ' ---------------------------------------------------------------------------

        Dim result As Boolean = False
        Dim sClause As String = ""


        ' Validate and build the clause
        Select Case v_lDatatype
            Case gPMConstants.PMEDataType.PMLong
                ' Add a long parameter

                If Not Informations.IsNothing(v_vValue) Then

                    Dim dbNumericTemp As Double
                    If Double.TryParse(CStr(v_vValue), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                        If (CDbl(v_vValue) > 0) Or (v_bAllowEmpty) Then

                            sClause = "(" & v_sField & v_sOperator & CStr(CDbl(v_vValue)) & ")"
                        End If
                    End If
                End If

            Case gPMConstants.PMEDataType.PMDate
                ' Add a date parameter

                If Not Informations.IsNothing(v_vValue) Then
                    If Informations.IsDate(v_vValue) Then
                        'developer guide no.101
                        If (v_vValue > #12/30/1899#) Or (v_bAllowEmpty) Then

                            sClause = "(" & v_sField & v_sOperator & "Convert(datetime, '" & CDate(v_vValue).ToString("yyyy.MM.dd HH:mm:ss") & "'))"
                        End If
                    End If
                End If

            Case gPMConstants.PMEDataType.PMCode
                ' This clause is coded as is, just add to list
                sClause = v_sField

            Case Else
                ' Assume this is just a string

                If Not Informations.IsNothing(v_vValue) Then

                    If ((CStr(v_vValue).Length) > 0) Or (v_bAllowEmpty) Then

                        If (CStr(v_vValue).IndexOf("%"c) + 1) And v_sOperator = "=" Then

                            sClause = "(" & v_sField & " LIKE '" & CStr(v_vValue) & "')"

                        ElseIf v_sOperator.ToUpper() = "IN" Then  'VB 01/03/2005 PN19032

                            sClause = "(" & v_sField & " " & v_sOperator & " (" & CStr(v_vValue) & "))"
                        Else

                            sClause = "(" & v_sField & v_sOperator & "'" & CStr(v_vValue) & "')"
                        End If
                    End If
                End If
        End Select

        ' If we have a clause add it
        If sClause.Length Then
            ' Return true if we have done something
            result = True

            If r_sSQLWhere.Length Then
                r_sSQLWhere = r_sSQLWhere & "AND " & sClause & " "
            Else
                r_sSQLWhere = "WHERE " & sClause & " "
            End If
        End If


        Return result
    End Function

    Private Function AddHavingClause(ByRef r_sSQLWhere As String, ByVal v_sField As String, ByVal v_lDatatype As gPMConstants.PMEDataType, Optional ByVal v_vValue As String = "", Optional ByVal v_sOperator As String = "=", Optional ByVal v_bAllowEmpty As Boolean = False) As Boolean
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: AddHavingClause
        ' PURPOSE: Add a HAVING clause to the current string. Used by SelectTransQuery
        '         based on AddWhereClause
        ' AUTHOR: Paul Cunningham
        ' DATE: 27/05/2003
        ' ---------------------------------------------------------------------------

        Dim result As Boolean = False
        Dim sClause As String = ""



        ' Validate and build the clause
        Select Case v_lDatatype
            Case gPMConstants.PMEDataType.PMLong
                ' Add a long parameter

                If Not Informations.IsNothing(v_vValue) Then
                    Dim dbNumericTemp As Double
                    If Double.TryParse(v_vValue, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                        If (ToSafeDouble(v_vValue) > 0) Or (v_bAllowEmpty) Then
                            sClause = "(" & v_sField & v_sOperator & CStr(CDbl(v_vValue)) & ")"
                        End If
                    End If
                End If

            Case gPMConstants.PMEDataType.PMDate
                ' Add a date parameter

                If Not Informations.IsNothing(v_vValue) Then
                    If Informations.IsDate(v_vValue) Then
                        If (ToSafeDouble(v_vValue) > 0) Or (v_bAllowEmpty) Then
                            Dim TempDate As Date
                            sClause = "(" & v_sField & v_sOperator & "Convert(datetime, '" & (If(DateTime.TryParse(v_vValue, TempDate), TempDate.ToString("yyyy.MM.dd HH:mm:ss"), v_vValue)) & "'))"
                        End If
                    End If
                End If

            Case gPMConstants.PMEDataType.PMCode
                ' This clause is coded as is, just add to list
                sClause = v_sField

            Case Else
                ' Assume this is just a string

                If Not Informations.IsNothing(v_vValue) Then
                    If (v_vValue.Length > 0) Or (v_bAllowEmpty) Then
                        If v_vValue.IndexOf("%"c) + 1 Then
                            sClause = "(" & v_sField & " LIKE '" & v_vValue & "')"
                        Else
                            sClause = "(" & v_sField & v_sOperator & "'" & v_vValue & "')"
                        End If
                    End If
                End If
        End Select

        ' If we have a clause add it
        If sClause.Length Then
            ' Return true if we have done something
            result = True

            If r_sSQLWhere.Length Then
                r_sSQLWhere = r_sSQLWhere & "AND " & sClause & " "
            Else
                r_sSQLWhere = " HAVING " & sClause & " "
            End If
        End If


        Return result
    End Function

    Public Function AddAuditSet(ByVal v_lDocumentID As Integer, ByVal v_lAuditSetID As Integer, Optional ByVal v_lCashListItemID As Integer = 0, Optional ByRef r_lAuditSetID As Integer = 0) As Integer

        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: AddAuditSet
        ' PURPOSE: Add an auditset record
        ' AUTHOR: Andrew Bibby
        ' DATE: 13/02/2003, 11:38
        ' RETURNS: PMTrue for success
        ' CHANGES: AMB 13/02/2003: PS220 - created
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim lAuditSetID As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            If v_lDocumentID = 0 Then
                Return result
            End If

            ' has the cashlistitemid been passed in?
            If False Then

                ' add the auditset record

                m_lReturn = m_oAuditSet.DirectAdd(vAuditsetID:=lAuditSetID, vCompanyID:=m_iSourceID, vUserID:=m_iUserID, vPostedDate:=DateTime.Now, vComment:="", vDocumentID:=v_lDocumentID, vAuditSetTypeID:=v_lAuditSetID)

            Else
                ' add the auditset record

                m_lReturn = m_oAuditSet.DirectAdd(vAuditsetID:=lAuditSetID, vCompanyID:=m_iSourceID, vUserID:=m_iUserID, vPostedDate:=DateTime.Now, vComment:="", vDocumentID:=v_lDocumentID, vAuditSetTypeID:=v_lAuditSetID, vCashListItemID:=v_lCashListItemID)

            End If

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception((ObjectError + m_lReturn).ToString() + ", AddAuditSet, Failed to add Auditset record")
            End If

            ' return the auditset_id
            r_lAuditSetID = lAuditSetID

            result = gPMConstants.PMEReturnCode.PMTrue


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="General failure in method", vApp:=ACApp, vClass:=ACClass, vMethod:="AddAuditSet", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

        Finally


        End Try
        Return result
    End Function
    ' ***************************************************************** '
    ' Name : AddTaskToWorkManager
    '
    ' Desc : Add task to work manager
    '
    ' History: 07/09/2001 TN - Created.
    '          13/02/2003 AMB - Modified for PS220 Manage Debtors development
    ' ***************************************************************** '
    Public Function AddTaskToWorkManager(ByRef r_lPMWrkTaskInstanceCnt As Integer, ByVal v_sCustomer As String, ByVal v_sDescription As String, ByVal v_dtTaskDueDate As Date, Optional ByVal v_lPMWrkTaskID As Integer = 0, Optional ByVal v_sTaskCode As String = "", Optional ByVal v_lPMWrkTaskGroupID As Integer = 0, Optional ByVal v_sTaskGroupCode As String = "", Optional ByVal v_iUserID As Integer = 0, Optional ByVal v_lUserGroupID As Integer = 0, Optional ByVal v_vKeyArray As Object = Nothing, Optional ByVal v_iIsUrgent As Integer = 0, Optional ByVal v_iIsVisible As Integer = 1) As Integer

        Dim result As Integer = 0
        Dim oDatabase As Object = Nothing
        Dim oWrkTaskInstance As bPMWrkTaskInstance.TaskControl
        Dim sSQL As String = ""
        Dim vResultArray As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'check to see if we have task_id or task code
            If v_lPMWrkTaskID = 0 And v_sTaskCode = "" Then
                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Must supply either task ID or task code", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            End If

            'check to see if we have task_group_id or task group code
            If v_lPMWrkTaskGroupID = 0 And v_sTaskGroupCode = "" Then
                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Must supply either task group ID or task group code", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            End If

            'open architecture database

            If gPMComponentServices.NewDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_oDatabase:=oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            oWrkTaskInstance = New bPMWrkTaskInstance.TaskControl
            If oWrkTaskInstance.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'get task group id
            If v_sTaskGroupCode <> "" Then
                sSQL = "SELECT pmwrk_task_group_id FROM PMWrk_Task_group WHERE code = {task_group_code}"


                oDatabase.Parameters.Clear()


                If oDatabase.Parameters.Add(sName:="task_group_code", vValue:=CStr(v_sTaskGroupCode), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                If oDatabase.SQLSelect(sSQL:=CStr(sSQL), sSQLName:="GetGroupTaskID", bStoredProcedure:=False, vResultArray:=vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Not Informations.IsArray(vResultArray) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                v_lPMWrkTaskGroupID = gPMFunctions.ToSafeInteger(vResultArray(0, 0))
            End If

            'create task using the task code

            m_lReturn = oWrkTaskInstance.CreateNewByCode(v_lPMWrkTaskGroupID:=v_lPMWrkTaskGroupID, v_sPMWrkTaskCode:=v_sTaskCode, v_sCustomer:=v_sCustomer, v_dtTaskDueDate:=v_dtTaskDueDate, v_lPMUserGroupID:=v_lUserGroupID, v_sDescription:=v_sDescription, v_iTaskStatus:=gPMConstants.PMEWrkManTaskStatus.pmeWMTSNew, v_iIsUrgent:=v_iIsUrgent, r_lPMWrkTaskInstanceCnt:=r_lPMWrkTaskInstanceCnt, v_vKeyArray:=v_vKeyArray, v_iIsVisible:=gPMConstants.PMEReturnCode.PMTrue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Create Work Manager Task", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager")
                Return result
            End If

            'close database

            oWrkTaskInstance.Dispose()



            oWrkTaskInstance = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddTaskToWorkManager Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '*************************************************************************
    'Name:          AddTransferDocument
    'Description:   Creates a New Document for a Transfer
    'History:       14/02/2003 - TR - Created for TS220 Manage Debtor Accounts
    '*************************************************************************
    Public Function AddTransferDocument(ByVal v_crTransferAmount As Decimal, ByVal v_iCompanyID As Integer, ByVal v_lOriginatingAccount As Integer, ByRef r_lDocumentID As Integer) As Integer


        Dim result As Integer = 0
        Dim obACTDocumentPost As bACTDocumentPost.Form
        Dim obACTAutoNumber As bACTAutoNumber.Business
        Dim lNumberRangeID, lDocumentTypeID As Integer
        Dim sGroupCode As String = ""
        Dim sRangeCode As String = ""
        Dim sDocumentRef As String = ""
        Dim sComment As String = ""

        Try

            'TR - Assume Success
            result = gPMConstants.PMEReturnCode.PMTrue

            'TR - Which kind of Document to create
            'DD 06/05/2003: Negative is a transferred credit
            If v_crTransferAmount < 0 Then
                'TR - Transferred Credits
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeTrc
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef36
                lDocumentTypeID = 36
            Else
                'TR - Transferred Debits
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeTrd
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef35
                lDocumentTypeID = 35
            End If

            'TR - Create the number generator object

            obACTAutoNumber = New bACTAutoNumber.Business
            m_lReturn = obACTAutoNumber.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                obACTAutoNumber = Nothing
                Return result
            End If

            'TR - Get the number range

            m_lReturn = obACTAutoNumber.GetNumberRange(sGroupCode, sRangeCode, lNumberRangeID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                obACTAutoNumber = Nothing
                Return result
            End If

            'TR - Generate the Unique ID for this Doc
            'Start(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
            'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber

            m_lReturn = obACTAutoNumber.GenerateDocumentReferenceNumber(v_lNumberRangeID:=lNumberRangeID, v_iUserID:=m_iUserID, v_iCompanyID:=m_iSourceID, r_sDocumentRef:=sDocumentRef, v_sRangeCode:=sRangeCode)
            'End(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                obACTAutoNumber = Nothing
                Return result
            End If

            'TR - Destroy the object
            obACTAutoNumber = Nothing

            'TR - Create the DocumentRef from the Generated No & Range
            '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
            sDocumentRef = sRangeCode & sDocumentRef

            'TR - Create the Comment
            sComment = "Transferred from Acc No: " & v_lOriginatingAccount

            'TR - Create the Posting Business Object

            obACTDocumentPost = New bACTDocumentPost.Form
            m_lReturn = obACTDocumentPost.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                obACTDocumentPost = Nothing
                Return result
            End If

            'TR - Post the Transfer Document

            m_lReturn = obACTDocumentPost.AddDocument(lDocumentTypeID, sDocumentRef, DateTime.Now, sComment, r_vDocumentID:=r_lDocumentID, r_vDocSourceID:=v_iCompanyID)

            'TR - Return Succes / Failure of Add

            'TR - Detro the objects
            Return m_lReturn

        Catch excep As System.Exception


            bPMFunc.LogMessage(m_sUsername, gPMConstants.PMELogLevel.PMLogOnError, "AddTransferDocument Failed", ACApp, ACClass, "AddTransferDocument", Informations.Err().Number, excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    '*************************************************************************
    'Name:          DeleteTransferDocument
    'Description:   Creates a New Document for a Transfer
    'History:       14/02/2003 - TR - Created for TS220 Manage Debtor Accounts
    '*************************************************************************
    Public Function DeleteTransferDocument(ByVal v_lDocumentID As Integer) As Integer

        Dim result As Integer = 0
        Dim obACTDocument As bACTDocument.Form

        Try

            'TR - Assume Success
            result = gPMConstants.PMEReturnCode.PMTrue

            'TR - Create the Posting Business Object
            obACTDocument = New bACTDocument.Form
            m_lReturn = obACTDocument.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                obACTDocument = Nothing
                Return result
            End If

            'TR - Post the Transfer Document

            m_lReturn = obACTDocument.DirectDelete(vDocumentID:=v_lDocumentID)

            'TR - Return Succes / Failure of Add

            'TR - Detro the objects
            Return m_lReturn

        Catch excep As System.Exception


            bPMFunc.LogMessage(m_sUsername, gPMConstants.PMELogLevel.PMLogOnError, "DeleteTransferDocument Failed", ACApp, ACClass, "DeleteTransferDocument", Informations.Err().Number, excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateUserProperty
    '
    ' Description: Updates a Property in Property Manager
    ' History:     sj 14/05/2003 - Created


    ' ***************************************************************** '
    Public Function UpdateUserProperty(ByVal v_sPropertyName As String, ByVal v_vPropertyValue As Object) As Integer

        Dim result As Integer = 0
        Try


            Return gPMComponentServices.UpdateUserProperty(v_sUsername:=m_sUsername, v_sPropertyName:=v_sPropertyName, v_vPropertyValue:=v_vPropertyValue)

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Update Property Named - " & v_sPropertyName, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateUserProperty", excep:=excep)
            Return result
        End Try
    End Function


    'DJM 07/10/2003
    Public Function IsInsurer(ByVal v_lAccountID As Integer) As Boolean

        Dim result As Boolean = False
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try


            sSQL = ""
            sSQL = sSQL & "SELECT l.ledger_short_name" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "FROM ledger l" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "JOIN account a" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ON a.ledger_id = l.ledger_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE account_id = " & CStr(v_lAccountID)


            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetAccountID", bStoredProcedure:=False, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vResultArray) Then

                If gPMFunctions.ToSafeString(vResultArray(0, 0)).Trim() = "IN" Then
                    result = True
                End If
            End If

            Return result

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsInsurer Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsInsurer", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function IsAgent(ByVal v_lAccountID As Integer) As Boolean

        Dim result As Boolean = False
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try


            sSQL = ""
            sSQL = sSQL & "SELECT l.ledger_short_name" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "FROM ledger l" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "JOIN account a" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ON a.ledger_id = l.ledger_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE account_id = " & CStr(v_lAccountID)


            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetAccountID", bStoredProcedure:=False, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vResultArray) Then

                If gPMFunctions.ToSafeString(vResultArray(0, 0)).Trim() = "AG" Then
                    result = True
                End If
            End If

            Return result

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsAgent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsAgent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'DJM 07/10/2003
    Public Function GetAllTransdetails(ByVal v_lTransDetailId As Integer, ByRef r_vTransdetails(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = ""
            sSQL = sSQL & "SELECT tdall.transdetail_id, ROUND(tdall.amount,2) - (SELECT isnull(SUM(ROUND(base_match_amount,2)),0) FROM transmatch WHERE transdetail_id = tdall.transdetail_id)" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "FROM transdetail td" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "JOIN transdetail tdall" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ON tdall.document_id = td.document_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "AND tdall.account_id = td.account_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE td.transdetail_id = " & CStr(v_lTransDetailId)


            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetAllTransdetails", bStoredProcedure:=False, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vResultArray) Then
                r_vTransdetails = vResultArray
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllTransdetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllTransdetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: UpdateComment
    '
    ' Description: Update the comment (on TransDetail table) for a given
    '              transaction.
    '
    ' History: CJB 26/03/2004: Created
    '
    ' ***************************************************************** '
    Public Function UpdateComment(ByVal v_lTransDetailId As Integer, ByVal v_sComment As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add parameters
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="transdetail_id", vValue:=CStr(v_lTransDetailId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add Failed for transdetail_id", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateComment", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="comment", vValue:=v_sComment, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add Failed for comment", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateComment", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Execute the stored procedure
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateCommentSQL, sSQLName:=ACUpdateCommentName, bStoredProcedure:=ACUpdateCommentStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.SQLAction Failed for " & ACUpdateCommentSQL, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateComment", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateComment Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateComment", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateNotReported
    '
    ' Description: Update the not_reported flag (on TransDetail table)
    ' for a given transaction.
    '
    ' History: CJB 30/03/2004: Created
    '
    ' ***************************************************************** '
    Public Function UpdateNotReported(ByVal v_lTransDetailId As Integer, ByVal v_boNotReported As Boolean) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add parameters
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="transdetail_id", vValue:=CStr(v_lTransDetailId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add Failed for transdetail_id", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateNotReported", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="not_reported", vValue:=CStr(v_boNotReported), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add Failed for not_reported", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateNotReported", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Execute the stored procedure
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateNotReportedSQL, sSQLName:=ACUpdateNotReportedName, bStoredProcedure:=ACUpdateNotReportedStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.SQLAction Failed for " & ACUpdateNotReportedSQL, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateNotReported", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateNotReported Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateNotReported", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: FormatCurrency
    '
    ' Description:  Pass the call through to the currency convert object.
    '
    ' ***************************************************************** '
    Public Function FormatCurrency(ByRef vCurrencyID As Object, ByRef vCurrencyAmount As Object, ByRef vFormattedCurrency As Object, Optional ByVal vConversionDate As Object = Nothing, Optional ByVal vConvertToBase As Object = Nothing) As Integer

        'vConversionDate & vConvertToBase are no longer used. Kept in for compatibility.

        Dim result As Integer = 0
        Try



            Return m_oCurrencyConvert.FormatCurrency(vCurrencyID:=vCurrencyID, vCurrencyAmount:=vCurrencyAmount, vFormattedCurrency:=vFormattedCurrency)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FormatCurrency Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FormatCurrency", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'DC220305 : PN19114: get all write off reasons
    Public Function GetWriteOffReasons(ByRef v_vWriteOffReasons As Object) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetWriteOffReasons
        ' PURPOSE: Get Write Off Reasons
        ' AUTHOR: David Cleaver
        ' DATE: MAR 2005
        ' REMARKS:
        ' ---------------------------------------------------------------------------
        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .SQLSelect(sSQL:=ACGetWriteOffReasonsSQL, sSQLName:=ACGetWriteOffReasonsName, bStoredProcedure:=ACGetWriteOffReasonsStored, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(ObjectError.ToString() + ", " + +", bACTDocumentReversal.Business - GetAssocIntroDocuments -  Failed")
                End If




                v_vWriteOffReasons = vResultArray

            End With



            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetWriteOffReasons", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

        Finally


        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: GetInsuranceFolderDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function GetInsuranceFolderDetails(ByVal v_lInsuranceFolderCnt As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetInsuranceFolderDetails"

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = AddInputParameter(v_sName:="insurance_folder_cnt", v_vValue:=v_lInsuranceFolderCnt, v_iType:=gPMConstants.PMEDataType.PMLong)

            ' Execute selection Query
            If m_oDatabase.SQLSelect(sSQL:=kGetInsuranceFolderDetailsSQL, sSQLName:=kGetInsuranceFolderDetailsName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetInsuranceFolderDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: AddInputParameter
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function AddInputParameter(ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iType As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AddInputParameter"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add Parameter to database object

            lReturn = m_oDatabase.Parameters.Add(sName:=v_sName, vValue:=CStr(v_vValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iType)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, " Failed to add parameter name:" & v_sName &
                                        ", values :" & CStr(v_vValue) & ", type:" & CStr(v_iType), gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    Private Function GetUserAuthorities() As Integer

        Dim result As Integer = 0
        'Const kMethodName As String = "GetUserAuthorities"

        Dim oUserAuthorities As bACTUserAuthorities.Business

        result = gPMConstants.PMEReturnCode.PMTrue

        'Set default values
        m_iHasUnrestrictedEnquiry = 0
        m_iHasUnrestrictedUpdate = 0

        oUserAuthorities = New bACTUserAuthorities.Business
        m_lReturn = oUserAuthorities.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("gPMComponentServices.CreateBusinessObject", "v_sClassName:=bACTUserAuthorities.Business", gPMConstants.PMELogLevel.PMLogError)
        End If


        m_lReturn = oUserAuthorities.GetDetails(vUserID:=m_iUserID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("oUserAuthorities.GetDetails", "vUserId:=" & m_iUserID, gPMConstants.PMELogLevel.PMLogError)
        End If


        m_lReturn = oUserAuthorities.GetNext(vHasUnrestrictedEnquiry:=m_iHasUnrestrictedEnquiry, vHasUnrestrictedUpdate:=m_iHasUnrestrictedUpdate)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("oUserAuthorities.GetNext", "Failed", gPMConstants.PMELogLevel.PMLogError)
        End If



        ' Do any tidy up, e.g. Set x = Nothing here
        If Not (oUserAuthorities Is Nothing) Then

            oUserAuthorities.Dispose()
            oUserAuthorities = Nothing
        End If

        Return result


    End Function

    Public Function GetEventInsuranceFileForDocument(ByVal v_sDocumentRef As String, ByVal v_lSourceID As Integer, ByRef r_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Const kMETHOD_NAME As String = "GetEventInsuranceFileForDocument"
        Try

            Dim vResultArray(,) As Object = Nothing

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()
                .Parameters.Add(sName:="document_ref", vValue:=v_sDocumentRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                .Parameters.Add(sName:="source_id", vValue:=CStr(v_lSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .SQLSelect(sSQL:=ACGetEventInsuranceFileDocumentSQL, sSQLName:=ACGetEventInsuranceFileDocumentName, bStoredProcedure:=ACGetEventInsuranceFileDocumentSp, vResultArray:=vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Informations.Err().Number.ToString() + ", " + Informations.Err().Source + ", " + "Failed to execute stored procedure " & ACGetEventInsuranceFileDocumentSQL)
                End If

            End With

            If Informations.IsArray(vResultArray) Then
                r_lInsuranceFileCnt = gPMFunctions.ToSafeLong(vResultArray(0, 0), -1)
            Else
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch ex As Exception



            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMETHOD_NAME, r_lFunctionReturn:=result, excep:=ex)

            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetTransFromQueryAdditional
    '
    ' Parameters: n/a
    '
    ' Description: Check if the Multi company is switched on then change the criteria
    '               of the query.
    ' History:
    '           Created : Deepak Mittal : Date : 23/11/2006
    ' ***************************************************************** '
    Public Function GetTransFromQueryAdditional() As String

        Dim result As String = String.Empty
        Dim vValue As String = ""

        Try
            'If any of the three hidden options are not enabled then it means it is not a multicompany
            bPMFunc.getProductOptionValue(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, m_iSourceID, vValue)

            If gPMFunctions.NullToString(vValue) <> "1" Then
                result = "LEFT JOIN account AS acc2 WITH(NOLOCK) ON acc2.account_key = i.insured_cnt " &
                     " LEFT JOIN AuditSet AS auds WITH(NOLOCK) ON auds.document_id = d.document_id AND ISNULL(auds.rejected,0)=0 " &
                     "LEFT JOIN AuditSet_Type AS audst WITH(NOLOCK) ON audst.auditset_type_id = auds.auditset_type_id " &
                     " LEFT JOIN Stats_Folder AS sf WITH(NOLOCK) ON sf.document_ref = d.document_ref " &
                     "LEFT JOIN Claim AS cl1 WITH(NOLOCK) ON cl1.claim_id = sf.loss_id " &
                     "LEFT JOIN [case] WITH(NOLOCK) ON [case].case_id = cl1.base_case_id " & Strings.ChrW(13) & Strings.ChrW(10)

                Return result
            End If

            bPMFunc.getProductOptionValue(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, gPMConstants.SIRHiddenOptions.SIROPTEnableBranchSelectAtLogon, m_iSourceID, vValue)

            If gPMFunctions.NullToString(vValue) <> "1" Then
                result = "LEFT JOIN account AS acc2 WITH(NOLOCK) ON acc2.account_key = i.insured_cnt " &
                     " LEFT JOIN AuditSet AS auds WITH(NOLOCK) ON auds.document_id = d.document_id AND ISNULL(auds.rejected,0)=0 " &
                     "LEFT JOIN AuditSet_Type AS audst WITH(NOLOCK) ON audst.auditset_type_id = auds.auditset_type_id " &
                     " LEFT JOIN Stats_Folder AS sf WITH(NOLOCK) ON sf.document_ref = d.document_ref " &
                     "LEFT JOIN Claim AS cl1 WITH(NOLOCK) ON cl1.claim_id = sf.loss_id " &
                     "LEFT JOIN [case] WITH(NOLOCK) ON [case].case_id = cl1.base_case_id " & Strings.ChrW(13) & Strings.ChrW(10)

                Return result
            End If

            result = "LEFT JOIN account AS acc2 WITH(NOLOCK) ON acc2.account_key = i.insured_cnt " &
                     " AND acc2.company_id = i.source_id " &
                     "LEFT JOIN AuditSet AS auds WITH(NOLOCK) ON auds.document_id = d.document_id AND ISNULL(auds.rejected,0)=0 " &
                     "LEFT JOIN AuditSet_Type AS audst WITH(NOLOCK) ON audst.auditset_type_id = auds.auditset_type_id " &
                     " LEFT JOIN Stats_Folder AS sf WITH(NOLOCK) ON sf.document_ref = d.document_ref " &
                     "LEFT JOIN Claim AS cl1 WITH(NOLOCK) ON cl1.claim_id = sf.loss_id " &
                     "LEFT JOIN [case] WITH(NOLOCK) ON [case].case_id = cl1.base_case_id " & Strings.ChrW(13) & Strings.ChrW(10)
            Return result
        Catch

            result = "LEFT JOIN account AS acc2 ON acc2.account_key = i.insured_cnt " &
                     " AND acc2.company_id = i.source_id " &
                     "LEFT JOIN AuditSet AS auds ON auds.document_id = d.document_id AND ISNULL(auds.rejected,0)=0 " &
                     "LEFT JOIN AuditSet_Type AS audst ON audst.auditset_type_id = auds.auditset_type_id " &
                     " LEFT JOIN Stats_Folder AS sf WITH(NOLOCK) ON sf.document_ref = d.document_ref " &
                     "LEFT JOIN Claim AS cl1 WITH(NOLOCK) ON cl1.claim_id = sf.loss_id " &
                     "LEFT JOIN [case] WITH(NOLOCK) ON [case].case_id = cl1.base_case_id " & Strings.ChrW(13) & Strings.ChrW(10)
            Return result
        End Try

    End Function

    Public Function GetMarkTransdetails(ByVal v_lDocumentID As Integer, ByVal v_lTransDetailId As Integer, ByVal v_lAccountID As Integer, ByRef r_bTransdetailIds As Boolean) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim r_vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            ' Add the ComponentNameID INPUT parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="lDocumentId", vValue:=CStr(v_lDocumentID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the ComponentNameID INPUT parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="lTransDetailId", vValue:=CStr(v_lTransDetailId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Add the ComponentNameID INPUT parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="lAccountId", vValue:=CStr(v_lAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectMarkTransactionSQL, sSQLName:=ACSelectMarkTransactionName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(r_vResultArray) Then
                r_bTransdetailIds = True
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetMarkTransdetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMarkTransdetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '****************************************************************** '
    ' Name: GetCashListTypeCode (Public)
    '
    ' Description: Gets CashListType Code as when supplied Trans Id
    '
    '****************************************************************** '
    Public Function GetCashListTypeCode(ByVal v_iTransDetailId As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetCashListTypeCode"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="transdetail_id", vValue:=CStr(v_iTransDetailId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to Add parameter transdetail_id")
                Return result
            End If

            'Execute SQL Statement
            m_lError = m_oDatabase.SQLSelect(sSQL:=ACGetCashListTypeCodeSQL, sSQLName:=ACGetCashListTypeCodeName, bStoredProcedure:=ACGetCashListTypeCodeSp, vResultArray:=vResultArray)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                'Raise Error
                gPMFunctions.RaiseError(kMethodName, "m_oDatabase.SQLSelect failed")
                Return result
            End If


        Catch ex As Exception

            ' Error Section.
            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="GetCashListTypeCode", r_lFunctionReturn:=result, v_sUsername:=m_sUsername, excep:=ex)

        Finally

            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: GetAccountTypes
    ' PURPOSE: Gets List of Account Types from Ledger Table in Database
    ' AUTHOR: Vineet
    ' DATE: 18 MAR 2008
    ' REMARKS:
    ' ---------------------------------------------------------------------------
    Public Function GetAccountTypes(ByRef r_vResultArray As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetAccountTypes"
        Dim r_vResult(,) As Object = Nothing
        Dim sErrorMessage As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectAccountTypeSQL, sSQLName:=ACSelectAccountTypeName, bStoredProcedure:=True, vResultArray:=r_vResult)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sErrorMessage = "Failed to Get List of  Account Types"
                gPMFunctions.RaiseError(kMethodName, sErrorMessage, gPMConstants.PMELogLevel.PMLogError)
            End If

            If Informations.IsArray(r_vResult) Then


                r_vResultArray = r_vResult
            End If
            result = gPMConstants.PMEReturnCode.PMTrue


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountTypes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountTypes", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)
            Return result

        Finally


        End Try
        Return result

    End Function

    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: GetPolicyBalance
    ' PURPOSE: Gets Policy Balance for Selected Transaction
    ' AUTHOR: Vineet
    ' DATE: 18 MAR 2008
    ' REMARKS:
    ' ---------------------------------------------------------------------------
    Public Function GetPolicyBalance(ByVal v_lAccountID As Integer, ByVal v_dAccountingDate As Date, ByVal v_sInsurance_ref As String, ByRef r_vResultArray As Object, Optional ByVal v_lInsuranceFileCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPolicyBalance"
        Dim r_vResult(,) As Object = Nothing
        Dim sErrorMessage As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.Parameters.Add(sName:="lAccount_ID", vValue:=CStr(v_lAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sErrorMessage = "Failed to Add Parameter lAccount_ID"
                gPMFunctions.RaiseError(kMethodName, sErrorMessage, gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="lInsurance_File_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sErrorMessage = "Failed to Add Parameter Insurance_file_cnt"
                gPMFunctions.RaiseError(kMethodName, sErrorMessage, gPMConstants.PMELogLevel.PMLogError)
            End If
            'developer guide no.40
            m_lReturn = m_oDatabase.Parameters.Add(sName:="daccounting_date", vValue:=v_dAccountingDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sErrorMessage = "Failed to Add Parameter AccountingDate"
                gPMFunctions.RaiseError(kMethodName, sErrorMessage, gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="sInsurance_ref", vValue:=v_sInsurance_ref, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sErrorMessage = "Failed to Add Parameter Insurance_Ref"
                gPMFunctions.RaiseError(kMethodName, sErrorMessage, gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectGetPolicyBalanceSQL, sSQLName:=ACSelectGetPolicyBalanceName, bStoredProcedure:=True, vResultArray:=r_vResult)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sErrorMessage = "Failed to Execute Procedure " & ACSelectGetPolicyBalanceSQL
                gPMFunctions.RaiseError(kMethodName, sErrorMessage, gPMConstants.PMELogLevel.PMLogError)
            End If

            If Informations.IsArray(r_vResult) Then


                r_vResultArray = r_vResult
            End If

            result = gPMConstants.PMEReturnCode.PMTrue


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyBalance Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyBalance", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

        Finally


        End Try
        Return result

    End Function

    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: GetPremiumFinanceBalance
    ' PURPOSE: Gets Premium Finance Balance for Selected Transaction
    ' AUTHOR: Vineet
    ' DATE: 19 MAR 2008
    ' REMARKS:
    ' ---------------------------------------------------------------------------

    Public Function GetPremiumFinanceBalance(ByVal v_lAccountID As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_dAccountingDate As Date, ByRef r_vResultArray As Object, ByVal v_sInsurance_ref As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPremiumFinanceBalance"
        Dim r_vResult(,) As Object = Nothing
        Dim sErrorMessage As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="lAccount_ID", vValue:=CStr(v_lAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sErrorMessage = "Failed to Add Parameter lAccountID"
                gPMFunctions.RaiseError(kMethodName, sErrorMessage, gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="lInsurance_File_Cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sErrorMessage = "Failed to Add Parameter Insurance_file_cnt"
                gPMFunctions.RaiseError(kMethodName, sErrorMessage, gPMConstants.PMELogLevel.PMLogError)
            End If
            'developer guide no.40
            m_lReturn = m_oDatabase.Parameters.Add(sName:="dAccounting_Date", vValue:=v_dAccountingDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sErrorMessage = "Failed to Add Parameter dAccounting_Date"
                gPMFunctions.RaiseError(kMethodName, sErrorMessage, gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not Informations.IsNothing(v_sInsurance_ref) Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="sInsurance_ref", vValue:=v_sInsurance_ref, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    sErrorMessage = "Failed to Add Parameter sInsurance_Ref"
                    gPMFunctions.RaiseError(kMethodName, sErrorMessage, gPMConstants.PMELogLevel.PMLogError)
                End If

            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectGetPremiumFinanceBalanceSQL, sSQLName:=ACSelectGetPremiumFinanceBalanceName, bStoredProcedure:=True, vResultArray:=r_vResult)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sErrorMessage = "Failed to Execute Procedure " & ACSelectGetPremiumFinanceBalanceSQL
                gPMFunctions.RaiseError(kMethodName, sErrorMessage, gPMConstants.PMELogLevel.PMLogError)
            End If

            If Informations.IsArray(r_vResult) Then


                r_vResultArray = r_vResult
            End If

            result = gPMConstants.PMEReturnCode.PMTrue


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyBalance Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyBalance", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

        Finally


        End Try
        Return result

    End Function
    'Start - Prakash - WPR85 Parelleling
    Private Function GetLinkedAccounts(ByVal v_lAccountID As Integer, ByRef r_vLinkedAccounts(,) As Object) As Integer


        Dim result As Integer = 0
        Const kMethodName As String = "GetLinkedAccounts"




        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="Account_ID", vValue:=CStr(v_lAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "Failed to Add Parameter Account_ID", gPMConstants.PMELogLevel.PMLogError)
        End If

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectLinkedAccountIDsSQL, sSQLName:=ACSelectLinkedAccountIDsName, bStoredProcedure:=True, vResultArray:=r_vLinkedAccounts)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, ACSelectLinkedAccountIDsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
        End If


        Return result
    End Function
    'End - Prakash - WPR85 Parelleling
    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub

    Public Function GetSubAgentDetails(ByVal v_iInsuranceFileCnt As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_iInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectSubAgentsSQL, sSQLName:=ACSelectSubAgentsName, bStoredProcedure:=ACSelectSubAgentsStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSubAgentDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSubAgents", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

        Return result

    End Function

    Public Function GetCaseNumber(ByRef r_sCaseNumber As String) As Long

        Dim vResultArray(,) As Object = Nothing
        Dim sSQL As String
        Dim vValue As String = ""

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Filter by company Id if Multibranch accounting enabled
            bPMFunc.getProductOptionValue(m_sUsername,
                                  m_sPassword,
                                  m_iUserID,
                                  m_iSourceID,
                                  m_iLanguageID,
                                  m_iCurrencyID,
                                  m_iLogLevel,
                                  m_sCallingAppName,
                                  gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting,
                                  m_iSourceID,
                                  vValue)
            m_lReturn = CType(bPMFunc.ValidateSQL(r_sCaseNumber), gPMConstants.PMEReturnCode)
            sSQL$ = "SELECT case_number FROM [case] WHERE case_number = '" & r_sCaseNumber & "'"

            'Only do this for Multi-Company
            If NullToString(vValue) = "1" Then
                'eck PN6169
                sSQL$ = sSQL$ & " AND company_id = " & m_iSourceID
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL$,
                                  sSQLName:="GetCaseNumber",
                                  bStoredProcedure:=False,
                                  vResultArray:=vResultArray)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                   sMsg:="GetCaseNumber Failed",
                   vApp:=ACApp,
                   vClass:=ACClass,
                   vMethod:="GetCaseNumber",
                   vErrNo:=Informations.Err.Number,
                   vErrDesc:=Informations.Err.Description)
                Return result
            End If

            If (Informations.IsArray(vResultArray) = False) Then
                r_sCaseNumber = ""
            Else
                r_sCaseNumber = CStr(vResultArray(0, 0))
            End If

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCaseNumber Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCaseNumber", vErrNo:=Informations.Err.Number, vErrDesc:=Informations.Err.Description, excep:=excep)

        End Try

        Return result

    End Function

    ''' <summary>
    ''' Returns user authority for receipt reversal
    ''' </summary>
    ''' <param name="nUserID"></param>
    ''' <param name="vResultArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetReceiptReversalUserAuthority(ByVal nUserID As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Const sMethodName As String = "GetReceiptReversalUserAuthority"
        Try

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(nUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.RaiseError(sMethodName, "Failed to Add parameter user_id")
                Return nResult
            End If

            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetReceiptReversalSQL, sSQLName:=ACGetReceiptReversalName, bStoredProcedure:=ACGetReceiptReversalStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Return nResult
            End If

        Catch excep As Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetReceiptReversalUserAuthority Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReceiptReversal", excep:=excep)
        End Try

        Return nResult
    End Function

    ''' <summary>
    ''' updates cashlistitem details
    ''' </summary>
    ''' <param name="nCashlistItemID"></param>
    ''' <param name="dtReversedDate"></param>
    ''' <param name="nCashlistitemReversePMuserID"></param>
    ''' <param name="nCashlistitemReverseReasonID"></param>
    ''' <param name="nCashlistitemReversalTransdetailID"></param>
    ''' <param name="nIsReceiptReversal"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetCashListItemFlags(ByVal nCashlistItemID As Integer, ByVal dtReversedDate As Date,
                                         ByVal nCashlistitemReversePMuserID As Integer,
                                         ByVal nCashlistitemReverseReasonID As Integer,
                                         ByVal nCashlistitemReversalTransdetailID As Integer,
                                         ByVal nIsReceiptReversal As Integer) As Integer

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Const sMethodName As String = "SetCashListItemFlags"


        Try

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="cashlistitem_id", vValue:=CStr(nCashlistItemID),
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.RaiseError(sMethodName, "Failed to add parameter cashlistitem_id")
                Return nResult
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="reversed_date", vValue:=dtReversedDate,
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.RaiseError(sMethodName, "Failed to add parameter reversed_date")
                Return nResult
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="cashlistitem_reverse_pmuser_id",
                                                   vValue:=CStr(nCashlistitemReversePMuserID),
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.RaiseError(sMethodName, "Failed to add parameter cashlistitem_reverse_pmuser_id")
                Return nResult
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="cashlistitem_reverse_reason_id",
                                                   vValue:=CStr(nCashlistitemReverseReasonID),
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.RaiseError(sMethodName, "Failed to add parameter cashlistitem_reverse_reason_id")
                Return nResult
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="cashlistitem_reversal_transdetail_id",
                                                   vValue:=CStr(nCashlistitemReversalTransdetailID),
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.RaiseError(sMethodName, "Failed to add parameter cashlistitem_reversal_transdetail_id")
                Return nResult
            End If
            m_lReturn = m_oDatabase.Parameters.Add(
                sName:="nIsReceiptReversal",
                vValue:=nIsReceiptReversal,
                iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                RaiseError(sMethodName, "Failed to add parameter cashlistitem_reversal_transdetail_id")
                Return nResult
            End If
            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateCashListItemQuerySQL,
                                             sSQLName:=ACUpdateCashListItemQueryName,
                                             bStoredProcedure:=ACUpdateCashListItemQueryStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Raise Error
                nResult = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.RaiseError(sMethodName, "m_oDatabase.SQLSelect failed")
                Return nResult
            End If

        Catch ex As Exception
            ' Error Section.
            nResult = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="SetCashListItemFlags", r_lFunctionReturn:=nResult, v_sUsername:=m_sUsername, excep:=ex)

        Finally

        End Try
        Return nResult
    End Function
    ''' <summary>
    ''' CheckIsThirdParty
    ''' </summary>
    ''' <param name="v_sPlanRef"></param>
    ''' <param name="r_bIsThirdParty"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CheckIsThirdParty(ByVal v_sPlanRef As String, ByRef r_bIsThirdParty As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CheckIsThirdParty"
        Dim vResultArray As Object = Nothing
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="PlanRef", vValue:=v_sPlanRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to Add parameter PlanRef")
                Return result
            End If

            'Execute SQL Statement
            m_lError = m_oDatabase.SQLSelect(sSQL:=ACCheckThirdPartyInstalmentSQL, sSQLName:=ACCheckThirdPartyInstalmentName, bStoredProcedure:=ACCheckThirdPartyInstalmentStored, vResultArray:=vResultArray)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                'Raise Error
                gPMFunctions.RaiseError(kMethodName, "m_oDatabase.SQLSelect failed")
                Return result
            End If

            If Informations.IsArray(vResultArray) Then
                r_bIsThirdParty = True
            Else
                r_bIsThirdParty = False
            End If

        Catch ex As Exception

            ' Error Section.
            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="CheckIsThirdParty", r_lFunctionReturn:=result, v_sUsername:=m_sUsername, excep:=ex)

        Finally

            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function
    ''' <summary>
    ''' PlanStatusUpdate
    ''' </summary>
    ''' <param name="v_sPlanRef"></param>
    ''' <param name="vStatusInd"></param>
    ''' <returns></returns>
    Public Function PlanStatusUpdate(ByVal v_sPlanRef As String, ByVal vStatusInd As String) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set variables for update call

            With m_oDatabase
                .Parameters.Clear()

                m_lReturn = m_oDatabase.Parameters.Add(sName:="PlanRef", vValue:=gPMFunctions.ToSafeString(v_sPlanRef), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                m_lReturn = m_oDatabase.Parameters.Add(sName:="status", vValue:=vStatusInd, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                ' Update existing record
                m_lReturn = m_oDatabase.SQLAction(ACPFPremiumFinanceUpdateStatusSQL, ACPFPremiumFinanceUpdateStatusName, ACPFPremiumFinanceUpdateStatusStored)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If
            End With

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StatusUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="StatusUpdate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Public Function UpdateCashListItemForAllocationStatus(ByVal v_iTransDetailId As Integer) As Integer

        Dim nResult As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMTrue

        Try

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ntransdetail_id", vValue:=CStr(v_iTransDetailId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateSQL, sSQLName:=ACUpdateName, bStoredProcedure:=ACUpdateStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update CashListItem Failed for Allocation Status", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateCashListItemForAllocationStatus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
        End Try

        Return nResult
    End Function
End Class
