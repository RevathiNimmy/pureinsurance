
Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Data.SqlClient
Imports System.Globalization
'Developer Guide No. 129
Imports SharedFiles
Imports System.Text

<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Form
    '
    ' Date: 27/10/1997
    '
    ' Description: Creatable Form class which contains all the
    '              methods, Form rules required to manipulate
    '              a SirPerilAllocation.
    '
    ' Edit History: TF27101997 - Created
    ' 24/10/2005 RKS Premium Override
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 16/12/2003
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************

    Private m_nProductId As Integer = 0
    Private m_bIsTrueMonthlyPolicy As Boolean = False

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    ' Task
    Private m_iTask As Integer
    ' Process Mode
    Private m_lProcessMode As Integer
    ' Type of Business
    Private m_sTransactionType As String = ""
    ' Effective
    Private m_dtEffectiveDate As Date

    ' Primary Keys to work with
    Private m_lInsuranceFolderCnt As Integer
    ' Insurance File Cnt
    Private m_lInsuranceFileCnt As Integer
    ' Risk ID
    Private m_lRiskID As Integer

    Private m_sDataModel As String = ""

    Private m_sDeclineReasons As String = ""
    Private m_sReferReasons As String = ""
    Private m_sMessages As String = ""

    Private m_iInsuranceFileNoOfDp As Integer

    Private m_iNBProRata As Integer
    Private m_iMTAProRata As Integer
    Private m_dProRataRate As Double
    Private m_sProRataMessage As String = ""

    Private m_bExtensionOfExpiryDate As Boolean

    ' ISS1377 - PWF 25/11/2002 - Short Period Rates
    Private m_iShortPeriodRated As Integer

    Private m_lOriginalRiskCnt As Integer

    Private m_lTransactionType As Integer
    ' JMK 26/07/2001 RoundPremium
    Private m_iRoundPremium As Integer
    Private m_lRoundingSection As Integer
    Private m_sRoundingSectionCode As String = ""

    ' UW Product Options
    Private m_lIsMidnightRenewal As Integer
    Private m_lAllowPositiveCancellation As Integer
    Private m_lUnifiedRenewalDay As Integer

    Private m_lInsuranceFileTypeId As Integer

    'Constants to define Listview column positions
    Const ACRatingSectionTypeCol As Integer = 0
    Const ACPolicySectionTypeCol As Integer = 1
    Const ACRateTypeCol As Integer = 2
    Const ACRateCol As Integer = 3
    Const ACSumInsuredCol As Integer = 4
    Const ACThisPremiumCol As Integer = 5
    Const ACPremiumCol As Integer = 6
    Const ACCountry As Integer = 7
    Const ACState As Integer = 8
    Const ACRatingSectionIdCol As Integer = 9
    Const ACRatingSectionTypeIdCol As Integer = 10
    Const ACPolicySectionTypeIdCol As Integer = 11
    Const ACRateTypeIdCol As Integer = 12
    Const ACOriginalFlagCol As Integer = 13

    ' RDT 16/04/2004 - Constant for the Terms Array
    Private Const ACColPosTermsFor As Integer = 0
    Private Const ACColPosExcessType As Integer = 1
    Private Const ACColPosExcess As Integer = 2
    Private Const ACColPosSpecialExcess As Integer = 3
    Private Const ACColPosExcessCategory As Integer = 4
    Private Const ACColPosExcluded As Integer = 5
    Private Const ACColPosConditions As Integer = 6

    Private m_cAnnualTaxTotal As Decimal

    Private Const kIsTemporaryQuotedMTA As Integer = 6
    Private Const kIsTemporaryMTA As Integer = 7

    Private m_bIsTemporaryMTA As Boolean
    Private m_bQuestionUnanswered As Boolean = False
    Private m_bIsMTCRatingRulesEnabled As Boolean

    ' HG181203 CQ598 Pass through Annual Tax Total

    Public Property AnnualTaxTotal() As Decimal
        Get
            Return m_cAnnualTaxTotal
        End Get
        Set(ByVal Value As Decimal)
            m_cAnnualTaxTotal = Value
        End Set
    End Property

    Public ReadOnly Property IsTrueMonthlyPolicy() As Boolean
        Get
            Return m_bIsTrueMonthlyPolicy
        End Get
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    Public ReadOnly Property Task() As Integer
        Get
            Return m_iTask
        End Get
    End Property

    Public ReadOnly Property ProcessMode() As Integer
        Get
            Return m_lProcessMode
        End Get
    End Property


    'TN20010514 start
    Public Property TransactionType() As String
        Get
            Return m_sTransactionType
        End Get
        Set(ByVal Value As String)
            m_sTransactionType = Value
        End Set
    End Property
    'TN20010514 end

    Public ReadOnly Property AllowPositiveCancellation() As Integer
        Get
            Return m_lAllowPositiveCancellation
        End Get
    End Property

    Public ReadOnly Property EffectiveDate() As Date
        Get
            Return m_dtEffectiveDate
        End Get
    End Property

    Public Property InsuranceFolderCnt() As Integer
        Get
            Return m_lInsuranceFolderCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFolderCnt = Value
        End Set
    End Property

    Public Property InsuranceFileCnt() As Integer
        Get
            Return m_lInsuranceFileCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property

    Public Property RiskID() As Integer
        Get
            Return m_lRiskID
        End Get
        Set(ByVal Value As Integer)
            m_lRiskID = Value
        End Set
    End Property

    Public Property DeclineReasons() As String
        Get
            Return m_sDeclineReasons
        End Get
        Set(ByVal Value As String)
            m_sDeclineReasons = Value
        End Set
    End Property

    Public Property ReferReasons() As String
        Get
            Return m_sReferReasons
        End Get
        Set(ByVal Value As String)
            m_sReferReasons = Value
        End Set
    End Property

    Public Property Messages() As String
        Get
            Return m_sMessages
        End Get
        Set(ByVal Value As String)
            m_sMessages = Value
        End Set
    End Property


    Public Property ProRataRate() As Double
        Get
            Return m_dProRataRate
        End Get
        Set(ByVal Value As Double)
            m_dProRataRate = Value
        End Set
    End Property

    Public ReadOnly Property ProRata() As Integer
        Get
            Select Case m_sTransactionType
                Case "NB"
                    Return m_iNBProRata
                Case Else
                    Return m_iMTAProRata
            End Select
        End Get
    End Property

    Public ReadOnly Property ProRataMessage() As String
        Get
            Return m_sProRataMessage
        End Get
    End Property
    'JMK 26/07/2001
    Public Property RoundPremium() As Integer
        Get
            Return m_iRoundPremium
        End Get
        Set(ByVal Value As Integer)
            m_iRoundPremium = Value
        End Set
    End Property
    'JMK 26/07/2001
    Public Property RoundingSectionID() As Integer
        Get
            Return m_lRoundingSection
        End Get
        Set(ByVal Value As Integer)
            m_lRoundingSection = Value
        End Set
    End Property
    'QBENZ022
    Public ReadOnly Property OriginalRiskCnt() As Integer
        Get
            Return m_lOriginalRiskCnt
        End Get
    End Property


    Public Property InsuranceFileNoOfDp() As Integer
        Get

            Return m_iInsuranceFileNoOfDp

        End Get
        Set(ByVal Value As Integer)

            ' Validate passed value
            If Value < 0 Then Value = 0
            If Value > 4 Then Value = 4

            m_iInsuranceFileNoOfDp = Value

        End Set
    End Property

    Public ReadOnly Property TransactionTypeId() As Integer
        Get
            If m_lTransactionType = 0 Then
                m_lReturn = CType(GetTransactionType(), gPMConstants.PMEReturnCode)
            End If

            Return m_lTransactionType
        End Get
    End Property


    Public Property IsTemporaryMTA() As Boolean
        Get
            Return m_bIsTemporaryMTA
        End Get
        Set(ByVal Value As Boolean)
            m_bIsTemporaryMTA = Value
        End Set
    End Property
    Public Property QuestionUnanswered() As Boolean
        Get
            Return m_bQuestionUnanswered
        End Get
        Set(ByVal Value As Boolean)
            m_bQuestionUnanswered = Value
        End Set
    End Property


    ' WPR24
    Public Property ExtensionOfExpiryDate() As Boolean
        Get
            Return m_bExtensionOfExpiryDate
        End Get
        Set(ByVal value As Boolean)
            m_bExtensionOfExpiryDate = value
        End Set
    End Property

    ' PUBLIC Property Procedures (End)


    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise


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


            ' Check that we have the right Database for our
            ' product Family

            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Set the ProcessMode to Generic
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric

            ' Set the Type Of Business to New Business
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric

            ' Set the Effective Date to NOW
            m_dtEffectiveDate = DateTime.Now

            'Set the default number of decimal places to 2
            m_iInsuranceFileNoOfDp = 2

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
                    m_oDatabase = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    Public Function CalculatePremium(ByVal v_lRatingSectionTypeId As Integer, ByVal v_cSumInsured As Decimal, ByRef v_cAnnualPremium As Decimal, ByRef v_cThisPremium As Decimal, ByRef v_cAnnualRate As Decimal) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bPMAddParameter.AddParameterLite(m_oDatabase, "rating_section_type_id", CStr(v_lRatingSectionTypeId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "sum_insured", CStr(v_cSumInsured), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
            bPMAddParameter.AddParameterLite(m_oDatabase, "annual_rate", CStr(v_cAnnualRate), gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency)
            bPMAddParameter.AddParameterLite(m_oDatabase, "annual_premium", CStr(v_cAnnualPremium), gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency)
            bPMAddParameter.AddParameterLite(m_oDatabase, "this_premium", CStr(v_cThisPremium), gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCalculatePremiumSQL, sSQLName:=ACCalculatePremiumName, bStoredProcedure:=ACCalculatePremiumStored)

            ' Commit or Rollback trans
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                'Get the return parameteres
                v_cAnnualPremium = m_oDatabase.Parameters.Item("annual_premium").Value
                v_cAnnualRate = m_oDatabase.Parameters.Item("Annual_Rate").Value
                v_cThisPremium = m_oDatabase.Parameters.Item("This_Premium").Value
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CalculatePremium Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CalculatePremium", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddSectionAndPerils (Public)
    '
    ' Description: Adds Section and Perils to database.
    '
    ' S. Rajan          21st August 2000
    ' ***************************************************************** '
    Public Function AddSectionAndPerils(ByVal v_lRatingSectionTypeId As Integer, ByVal v_lPolicySectionTypeId As Integer, ByVal v_cAnnualPremium As Decimal, ByVal v_cThisPremium As Decimal, ByVal v_cAnnualRate As Double, ByVal v_cSumInsured As Decimal, ByVal v_lRateTypeId As Integer, ByVal v_lOriginalFlag As Integer, ByVal v_iDefinedCurrencyID As Integer, ByVal v_lCountryID As Integer, ByVal v_lStateID As Integer, ByVal v_iIsAmended As Integer, ByVal v_cCalculatedPremium As Decimal, ByVal v_sOverrideReason As String, ByVal v_lEarningPatternId As Integer) As Integer
        Return AddSectionAndPerils(v_lRatingSectionTypeId:=v_lRatingSectionTypeId, v_lPolicySectionTypeId:=v_lPolicySectionTypeId, v_cAnnualPremium:=v_cAnnualPremium, v_cThisPremium:=v_cThisPremium, v_cAnnualRate:=v_cAnnualRate, v_cSumInsured:=v_cSumInsured, v_lRateTypeId:=v_lRateTypeId, v_lOriginalFlag:=v_lOriginalFlag, v_iDefinedCurrencyID:=v_iDefinedCurrencyID, v_lCountryID:=v_lCountryID, v_lStateID:=v_lStateID, v_iIsAmended:=v_iIsAmended, v_cCalculatedPremium:=v_cCalculatedPremium, v_sOverrideReason:=v_sOverrideReason, v_iAutoCalculated:=1, v_lEarningPatternId:=v_lEarningPatternId)
    End Function

    Public Function AddSectionAndPerils(ByVal v_lRatingSectionTypeId As Integer, ByVal v_lPolicySectionTypeId As Integer, ByVal v_cAnnualPremium As Decimal, ByVal v_cThisPremium As Decimal, ByVal v_cAnnualRate As Double, ByVal v_cSumInsured As Decimal, ByVal v_lRateTypeId As Integer, ByVal v_lOriginalFlag As Integer, ByVal v_iDefinedCurrencyID As Integer, ByVal v_lCountryID As Integer, ByVal v_lStateID As Integer, ByVal v_iIsAmended As Integer, ByVal v_cCalculatedPremium As Decimal, ByVal v_sOverrideReason As String, ByVal v_iAutoCalculated As Integer, ByVal v_lEarningPatternId As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bPMAddParameter.AddParameterLite(m_oDatabase, "rating_section_type_id", CStr(v_lRatingSectionTypeId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            'Developer Guide No. 85 (guide)
            bPMAddParameter.AddParameterLite(m_oDatabase, "policy_section_type_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "insurance_file_cnt", CStr(m_lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_id", CStr(m_lRiskID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "sum_insured", CStr(v_cSumInsured), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
            bPMAddParameter.AddParameterLite(m_oDatabase, "annual_rate", CStr(v_cAnnualRate), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)
            bPMAddParameter.AddParameterLite(m_oDatabase, "annual_premium", CStr(v_cAnnualPremium), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
            bPMAddParameter.AddParameterLite(m_oDatabase, "this_premium", CStr(v_cThisPremium), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
            bPMAddParameter.AddParameterLite(m_oDatabase, "rate_type_id", CStr(v_lRateTypeId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "insurance_file_no_of_dp", CStr(m_iInsuranceFileNoOfDp), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "original_flag", CStr(v_lOriginalFlag), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            'Developer Guide No.85
            If v_iDefinedCurrencyID = 0 Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "currency_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            Else
                bPMAddParameter.AddParameterLite(m_oDatabase, "currency_id", CStr(v_iDefinedCurrencyID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            End If


            ' Developer Guide No.85
            If v_lCountryID = 0 Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "country_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            Else
                bPMAddParameter.AddParameterLite(m_oDatabase, "country_id", CStr(v_lCountryID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            End If



            'Developer Guide No.85
            If v_lStateID = 0 Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "state_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            Else
                bPMAddParameter.AddParameterLite(m_oDatabase, "state_id", CStr(v_lStateID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            End If


            bPMAddParameter.AddParameterLite(m_oDatabase, "is_amended", CStr(v_iIsAmended), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "calculated_premium", CStr(v_cCalculatedPremium), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
            bPMAddParameter.AddParameterLite(m_oDatabase, "override_reason", v_sOverrideReason, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Auto_calculated", CStr(v_iAutoCalculated), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

            If Not False And v_lEarningPatternId <> 0 Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "earning_pattern_id", CStr(v_lEarningPatternId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            End If

            ' Begin transaction
            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add Section & Perils
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSectionAndPerilsSQL, sSQLName:=ACAddSectionAndPerilsName, bStoredProcedure:=ACAddSectionAndPerilsStored)

            ' Commit or Rollback trans
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)
            Else
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddSectionAndPerils Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddSectionAndPerils", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteSectionAndPerils (Public)
    '
    ' Description: Deletes Section and Perils for the Insurance file cnt and Risk Id
    '
    ' S. Rajan          24th August 2000
    ' ***************************************************************** '

    Public Function DeleteSectionAndPerils() As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(m_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .Parameters.Add(sName:="risk_id", vValue:=CStr(m_lRiskID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                'Begin the Transaction
                m_lReturn = .SQLBeginTrans()

                'Call the Stored procedure to delete the perils first
                m_lReturn = .SQLAction(sSQL:="spu_sir_peril_del", sSQLName:=ACDeletePerilsName, bStoredProcedure:=ACDeletePerilsStored)

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                    'Call the Stored procedure to Delete the Rating section also
                    m_lReturn = .SQLAction(sSQL:="spu_sir_rating_section_del", sSQLName:=ACDeleteRatingSectionName, bStoredProcedure:=ACDeleteRatingSectionStored)
                End If

                'Store this, otherwise we'll just return that the rollback was successful
                lReturn = m_lReturn

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = .SQLCommitTrans()
                Else
                    m_lReturn = .SQLRollbackTrans()
                End If

            End With

            'Set the result

            Return lReturn

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteSectionAndPerils Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteSectionAndPerils", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Public Function DeleteAndCreateNewSectionAndPerilsForUtility() As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()
              
                m_lReturn = .Parameters.Add(sName:="risk_cnt", vValue:=CStr(m_lRiskID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                'Begin the Transaction
                m_lReturn = .SQLBeginTrans()

                m_lReturn = .SQLAction(sSQL:="spu_sir_rating_section_peril_DataFix", sSQLName:="DeleteAndCreateNewSectionAndPerilsForUtility", bStoredProcedure:=True)

                'Store this, otherwise we'll just return that the rollback was successful
                lReturn = m_lReturn

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = .SQLCommitTrans()
                Else
                    m_lReturn = .SQLRollbackTrans()
                End If

            End With

            'Set the result

            Return lReturn

        Catch excep As System.Exception

            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteAndCreateNewSectionAndPerilsForUtility Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteAndCreateNewSectionAndPerilsForUtility", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function




    ' ***************************************************************** '
    ' Name: GetInsuranceHeaderDetails (Public)
    '
    ' Description: gets the insurance header details
    '
    ' ***************************************************************** '
    Public Function GetInsuranceHeaderDetails(ByRef r_sInsuranceHolderShortName As String, ByRef r_sInsuranceHolderName As String, ByRef r_sInsuranceHolderResolvedName As String, ByRef r_sInsuranceRef As String, ByRef r_sInsuranceFolderDescription As String, ByRef r_sInsuranceCurrencyCode As String, ByRef r_sInsuranceCurrencyCaption As String, ByRef r_iInsuranceCurrencyID As Integer, ByRef r_lInsuranceCompanyID As Integer) As Integer

        Dim result As Integer = 0
        Dim oPMLookup As bPMLookup.Business
        Dim lInsuranceHolderCnt As Integer
        Dim sInsuranceRef As String = ""
        Dim lInsuranceFolderCnt As Integer
        Dim iCurrencyID As Integer
        Dim vLookupResult As Object

        Dim sSQL As String = ""
        Dim vArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_lInsuranceFileCnt < 1 Then
                ' no ins file cnt supplied, log error and exit
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="No Insurance File Cnt supplied", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInsuranceHeaderDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            ' set the required components

            oPMLookup = New bPMLookup.Business()

            m_lReturn = CType(oPMLookup, SSP.S4I.Interfaces.IBusiness).Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMError
            End If

            sSQL = "Select ifo.insurance_holder_cnt, ifi.insurance_ref, ifi.insurance_folder_cnt, ifi.currency_id, ifi.source_id " & _
                   "from insurance_file ifi, insurance_folder ifo where ifi.insurance_file_cnt = " & CStr(m_lInsuranceFileCnt) & _
                   " and ifi.insurance_folder_cnt = ifo.insurance_folder_cnt"

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetInsuranceFile", bStoredProcedure:=False, vResultArray:=vArray)


            lInsuranceHolderCnt = CInt(vArray(0, 0))

            sInsuranceRef = CStr(vArray(1, 0))

            lInsuranceFolderCnt = CInt(vArray(2, 0))

            iCurrencyID = CInt(vArray(3, 0))


            r_lInsuranceCompanyID = CInt(vArray(4, 0))
            r_sInsuranceRef = sInsuranceRef

            sSQL = "Select last_trans_description " & _
                   "from insurance_file_system where insurance_file_cnt = " & CStr(m_lInsuranceFileCnt)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetInsuranceFileSystem", bStoredProcedure:=False, vResultArray:=vArray)


            r_sInsuranceFolderDescription = CStr(vArray(0, 0))

            sSQL = "Select name, shortname, resolved_name " & _
                   "from party where party_cnt = " & CStr(lInsuranceHolderCnt)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetParty", bStoredProcedure:=False, vResultArray:=vArray)


            r_sInsuranceHolderShortName = CStr(vArray(0, 0)).Trim()

            r_sInsuranceHolderName = CStr(vArray(1, 0)).Trim()

            r_sInsuranceHolderResolvedName = CStr(vArray(2, 0)).Trim()


            r_sInsuranceFolderDescription = CStr(vArray(0, 0))




            ' set the table array
            Dim vTableArray(3, 0) As Object

            ' Column positions for output array.

            ' table name

            vTableArray(0, 0) = "Currency"
            ' key

            vTableArray(1, 0) = iCurrencyID
            ' start position

            vTableArray(2, 0) = 1
            ' number of items

            vTableArray(3, 0) = 1

            ' get the code and caption for currency
            m_lReturn = oPMLookup.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupSingle, vTableArray:=vTableArray, iLanguageID:=m_iLanguageID, dtEffectiveDate:=m_dtEffectiveDate, vResultArray:=vLookupResult)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMError
            End If

            ' Set the returned values to the referenced variables

            r_sInsuranceCurrencyCode = CStr(vLookupResult(2, 0)).Trim()

            r_sInsuranceCurrencyCaption = CStr(vLookupResult(1, 0)).Trim()
            r_iInsuranceCurrencyID = iCurrencyID

            Return result

        Catch excep As System.Exception

            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInsuranceHeaderDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInsuranceHeaderDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        Finally
            oPMLookup.Dispose()
            oPMLookup = Nothing
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetDataModel
    '
    ' Description:
    '
    ' History: 24/09/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function GetDataModel(ByRef sGISDataModel As String) As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object
        Dim lPolicyBinder As Integer
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(m_lInsuranceFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_id", vValue:=CStr(m_lRiskID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectGISDataModelSQL, sSQLName:=ACSelectGISDataModelName, bStoredProcedure:=ACSelectGISDataModelStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Information.IsArray(vArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            sGISDataModel = CStr(vArray(0, 0)).Trim()

            lPolicyBinder = CInt(vArray(1, 0))


            'Developer Guide No. 12
            vArray = Nothing

            'Now delete the output records
            sSQL = "DELETE" & Strings.Chr(13) & Strings.Chr(10) & _
                   "FROM " & sGISDataModel & "_Output" & Strings.Chr(13) & Strings.Chr(10) & _
                   "WHERE " & sGISDataModel & "_policy_binder_id IN (" & Strings.Chr(13) & Strings.Chr(10) & _
                   "SELECT " & sGISDataModel & "_policy_binder_id" & Strings.Chr(13) & Strings.Chr(10) & _
                   "FROM " & sGISDataModel & "_policy_binder" & Strings.Chr(13) & Strings.Chr(10) & _
                   "WHERE gis_policy_link_id = " & CStr(lPolicyBinder) & ")" & Strings.Chr(13) & Strings.Chr(10)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="GetGISOutput", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDataModel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDataModel", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' PopulateRatingSections
    ''' </summary>
    ''' <param name="r_vResultArray"></param>
    ''' <param name="v_bIsBackdatedMTA"></param>
    ''' <param name="r_lPostChangeRiskCnt"></param>
    ''' <param name="v_dtMTADateCurrent"></param>
    ''' <param name="v_bExistsPreAndPost"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>

    Public Function PopulateRatingSections(ByRef r_vResultArray As Object) As Integer
        Return PopulateRatingSections(r_vResultArray:=r_vResultArray, _
                                    v_bIsBackdatedMTA:=False, _
                                    r_lPostChangeRiskCnt:=0, _
                                    v_dtMTADateCurrent:=#12/30/1899#, _
                                    v_bExistsPreAndPost:=False, _
                                    v_sRiskMergeStatus:="")
    End Function

    Public Function PopulateRatingSections(ByRef r_vResultArray As Object, _
                                           ByRef v_bIsBackdatedMTA As Boolean) As Integer
        Return PopulateRatingSections(r_vResultArray:=r_vResultArray, _
                                    v_bIsBackdatedMTA:=v_bIsBackdatedMTA, _
                                    r_lPostChangeRiskCnt:=0, _
                                    v_dtMTADateCurrent:=#12/30/1899#, _
                                    v_bExistsPreAndPost:=False, _
                                    v_sRiskMergeStatus:="")
    End Function

    Public Function PopulateRatingSections() As Integer
        Return PopulateRatingSections(r_vResultArray:=Nothing, _
                                    v_bIsBackdatedMTA:=False, _
                                    r_lPostChangeRiskCnt:=0, _
                                    v_dtMTADateCurrent:=#12/30/1899#, _
                                    v_bExistsPreAndPost:=False, _
                                    v_sRiskMergeStatus:="")
    End Function

    Public Function PopulateRatingSections(ByRef r_vResultArray As Object, _
                                         ByRef v_bIsBackdatedMTA As Boolean, _
                                         ByRef r_lPostChangeRiskCnt As Integer, _
                                         ByRef v_dtMTADateCurrent As Date, _
                                         ByRef v_bExistsPreAndPost As Boolean, _
                                         v_sRiskMergeStatus As String) As Integer
        Dim nResult As Integer = 0
        Dim oArray(,) As Object
        Dim nPolicyBinder As Integer
        Dim sSQL As String = ""
        Dim nPolicyRatingSectionId As Integer
        Dim nRiskRatingSectionId As Integer
        Dim dAnnualRate As Double
        Dim nRateTypeId As Integer
        Dim nRiskStatus As Integer
        Dim nInsuranceFileCnt As Integer
        Dim dtOldCoverStartDate As Date
        Dim dtOldExpiryDate As Date
        Dim dtCoverStartDate As Date
        Dim dtExpiryDate As Date
        Dim dOriginalAnnualPremium As Decimal
        Dim dAnnualPremium As Decimal
        Dim dThisPremium As Decimal
        Dim bAllAnswered As Boolean
        Dim nProductId As Integer
        Dim oRatingSectionArray(,) As Object
        Dim oMergedArray As Object
        Dim sStatusFlag As String = ""
        Dim bShortPeriodRated As Boolean
        Dim nCurrencyId As Integer
        Dim dRoundingAnnualPremium As Decimal
        Dim dRoundingThisPremium As Decimal

        Dim dOriginalRiskProrataRate As Double
        Dim dtInceptionDate As Date

        Dim dtRiskInceptionDate As Date

        Dim otempArray(,) As Object
        Dim bIsMandatoryRisk As Boolean
        Dim dtOriginalCoverStartDate As Date
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            'TN20010514 start
            ' ISS1377 - PWF 25/11/2002 - Short Period Rates
            m_lReturn = CType(GetProRataFlag(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, r_iNBProrata:=m_iNBProRata,
                                             r_iMTAProrata:=m_iMTAProRata, r_iShortPeriodRate:=m_iShortPeriodRated), 
                              gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'TN20010514 end
            If m_sTransactionType = "MTC" Then
                m_lReturn = CType(CheckMTCRatingRules(m_lInsuranceFileCnt, m_bIsMTCRatingRulesEnabled), 
                                  gPMConstants.PMEReturnCode)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Insurance File Cnt parameter (INPUT)
            bPMAddParameter.AddParameterLite(m_oDatabase, "nInsuranceFileCnt", CInt(m_lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            ' Execute SQL Statement   
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGETInsuranceFileTypeIDAndCodeNameSQL, sSQLName:=ACGETInsuranceFileTypeIDAndCodeName, bStoredProcedure:=ACGETInsuranceFileTypeIDAndCodeStored, vResultArray:=oArray)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            Else
                If IsArray(oArray) Then
                    If Trim(CStr(oArray(0, 0))).ToUpper() = "MTAQCAN" Then
                        m_sTransactionType = "MTC"
                    ElseIf Trim(CStr(oArray(0, 0))).ToUpper() = "MTAQREINS" Then
                        m_sTransactionType = "MTR"
                    End If
                    dtOriginalCoverStartDate = ToSafeDate(oArray(2, 0))
                    oArray = Nothing
                End If
            End If

            'Get the original risk cnt
            bPMAddParameter.AddParameterLite(m_oDatabase, "insurance_file_cnt", CStr(m_lInsuranceFileCnt),
                                             gPMConstants.PMEParameterDirection.PMParamInput,
                                             gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_cnt", CStr(m_lRiskID),
                                             gPMConstants.PMEParameterDirection.PMParamInput,
                                             gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectIFRLSQL, sSQLName:=ACSelectIFRLName,
                                              bStoredProcedure:=ACSelectIFRLStored,
                                              lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=oArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Information.IsArray(oArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CheckMandatoryRisk(m_lRiskID, bIsMandatoryRisk)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'Renewals puts 0 into this instead of null

            If _
                (gPMFunctions.NullToString(CStr(oArray(3, 0))) = "") OrElse
                (gPMFunctions.NullToString(CStr(oArray(3, 0))) = "0") Then
                m_lOriginalRiskCnt = -1
            Else

                m_lOriginalRiskCnt = CInt(oArray(3, 0))
            End If


            sStatusFlag = CStr(oArray(2, 0))

            ' Wipe out the array

            oArray = Nothing

            'JMK 02/08/2001 start
            ' moved to AFTER we've got the original risk id
            bPMAddParameter.AddParameterLite(m_oDatabase, "insurance_file_cnt", CStr(m_lInsuranceFolderCnt),
                                             gPMConstants.PMEParameterDirection.PMParamInput,
                                             gPMConstants.PMEDataType.PMLong, True)

            ' if cancelling, use original risk id
            If m_sTransactionType = "MTC" AndAlso Not m_bIsMTCRatingRulesEnabled AndAlso Not bIsMandatoryRisk Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "risk_id", CStr(m_lOriginalRiskCnt),
                                                 gPMConstants.PMEParameterDirection.PMParamInput,
                                                 gPMConstants.PMEDataType.PMLong)
            Else
                bPMAddParameter.AddParameterLite(m_oDatabase, "risk_id", CStr(m_lRiskID),
                                                 gPMConstants.PMEParameterDirection.PMParamInput,
                                                 gPMConstants.PMEDataType.PMLong)
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectGISDataModelSQL, sSQLName:=ACSelectGISDataModelName,
                                              bStoredProcedure:=ACSelectGISDataModelStored,
                                              lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=oArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Information.IsArray(oArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_sDataModel = CStr(oArray(0, 0)).Trim()

            nPolicyBinder = CInt(oArray(1, 0))

            oArray = Nothing

            'JMK 02/08/2001 end

            'Now we get the duration of the risk - this gives us the pro-rata % when we get
            'the number of days / 365

            'Longer term we need to check the proper pro-rata table as used for cancellations

            'We find the original insurance file record for the risk we're on

            If m_lOriginalRiskCnt = -1 Then
                nInsuranceFileCnt = m_lInsuranceFileCnt
            Else
                m_oDatabase.Parameters.Clear()

                m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=CStr(m_lOriginalRiskCnt),
                                                       iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                       iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = m_oDatabase.Parameters.Add(sName:="cover_start_date", _
                      vValue:=dtOriginalCoverStartDate, _
                      iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, _
                      iDataType:=gPMConstants.PMEDataType.PMDate)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' If this is an MTA Reinstatement then get the dates from the cancellation version.
                If m_sTransactionType = "MTR" Then
                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectInsuranceFileCntMTRSQL,
                                                      sSQLName:=ACSelectInsuranceFileCntName,
                                                      bStoredProcedure:=ACSelectInsuranceFileCntMTRStored,
                                                      lNumberRecords:=gPMConstants.PMAllRecords,
                                                      vResultArray:=oArray)
                ElseIf (m_sTransactionType = "PT") Then
                    m_lReturn = m_oDatabase.SQLSelect( _
                        sSQL:=kSelectInsuranceFileCntForPTSQL, _
                        sSQLName:=kSelectInsuranceFileCntForPTName, _
                        bStoredProcedure:=kSelectInsuranceFileCntForPTStored, _
                        lNumberRecords:=gPMConstants.PMAllRecords,
                        vResultArray:=oArray)
                Else
                    m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_select_Insurance_FileCnt_DataFix",
                                                      sSQLName:=ACSelectInsuranceFileCntName,
                                                      bStoredProcedure:=ACSelectInsuranceFileCntStored,
                                                      lNumberRecords:=gPMConstants.PMAllRecords,
                                                      vResultArray:=oArray)
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Not Information.IsArray(oArray) Then
                    m_lOriginalRiskCnt = -1
                    nInsuranceFileCnt = m_lInsuranceFileCnt
                Else

                    nInsuranceFileCnt = CInt(oArray(0, 0))

                    dtOldCoverStartDate = CDate(oArray(1, 0))

                    dtOldExpiryDate = CDate(oArray(2, 0))
                End If

                oArray = Nothing

            End If


            'Now we get the insurance file record and get the cover start and expiry dates

            'We can get both from the current insurance file record - vital
            'if it's a temporary MTA
            bPMAddParameter.AddParameterLite(m_oDatabase, "insurance_file_cnt", CStr(m_lInsuranceFileCnt),
                                             gPMConstants.PMEParameterDirection.PMParamInput,
                                             gPMConstants.PMEDataType.PMLong, True)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectInsuranceFileSQL, sSQLName:=ACSelectInsuranceFileName,
                                              bStoredProcedure:=ACSelectInsuranceFileStored,
                                              lNumberRecords:=gPMConstants.PMAllRecords)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If _
                Convert.IsDBNull(m_oDatabase.Records.Item(0).Fields()("cover_start_date")) OrElse
                IsNothing(m_oDatabase.Records.Item(0).Fields()("cover_start_date")) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                dtCoverStartDate = m_oDatabase.Records.Item(0).Fields()("cover_start_date")
            End If


            If _
                Convert.IsDBNull(m_oDatabase.Records.Item(0).Fields()("expiry_date")) OrElse
                IsNothing(m_oDatabase.Records.Item(0).Fields()("expiry_date")) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                dtExpiryDate = m_oDatabase.Records.Item(0).Fields()("expiry_date")
            End If


            If _
                Not _
                (Convert.IsDBNull(m_oDatabase.Records.Item(0).Fields()("inception_date_tpi")) OrElse
                 IsNothing(m_oDatabase.Records.Item(0).Fields()("inception_date_tpi"))) Then
                dtInceptionDate = m_oDatabase.Records.Item(0).Fields()("inception_date_tpi")
            End If

            If m_lOriginalRiskCnt = -1 Then
                dtOldCoverStartDate = dtCoverStartDate
                dtOldExpiryDate = dtExpiryDate
            End If

            ' WPR24
            If (dtExpiryDate > dtOldExpiryDate) AndAlso (m_sTransactionType = "MTA" AndAlso m_iMTAProRata = 1) Then
                m_bExtensionOfExpiryDate = True
            Else
                If (dtExpiryDate > dtOldExpiryDate) AndAlso Not (v_bIsBackdatedMTA) Then
                    'This will never be the case...
                    If m_sTransactionType = "NB" AndAlso m_iNBProRata = 0 Then
                        m_iNBProRata = 0
                        m_sProRataMessage = "Cannot pro rata a policy extension - pro rata disabled"
                    End If
                    If m_sTransactionType = "MTA" Then
                        If m_iMTAProRata = 0 Then
                            m_sProRataMessage = "Cannot pro rata a policy extension - pro rata disabled"
                        End If
                    End If
                End If
            End If

            'm_dProRataRate = (dtExpiryDate - dtCoverStartDate) / 365#
            ' PW080102 - scalability changes
            nProductId = gPMFunctions.NullToLong(m_oDatabase.Records.Item(0).Fields()("product_id"))
            m_nProductId = gPMFunctions.NullToLong(m_oDatabase.Records.Item(0).Fields()("product_id"))


            If _
                Convert.IsDBNull(m_oDatabase.Records.Item(0).Fields()("insurance_file_type_id")) OrElse
                IsNothing(m_oDatabase.Records.Item(0).Fields()("insurance_file_type_id")) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                m_lInsuranceFileTypeId = m_oDatabase.Records.Item(0).Fields()("insurance_file_type_id")
            End If

            If m_lInsuranceFileTypeId = kIsTemporaryQuotedMTA OrElse m_lInsuranceFileTypeId = kIsTemporaryMTA Then
                IsTemporaryMTA = True
            End If

            ' Get the options associated with this underwriting product
            m_lReturn = CType(GetUWProductOptions(v_lProductID:=nProductId), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' JMK 26/07/2001 start
            ' find out if this product specifies Rounding
            m_lReturn = CType(GetRoundingInfo(v_lProductID:=nProductId, r_iRoundPremium:=m_iRoundPremium,
                                              r_lRoundingSectionID:=m_lRoundingSection,
                                              r_sRoundingSectionCode:=m_sRoundingSectionCode), 
                              gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' ISS1377 - PWF 25/11/2002 - Short Period Rates
            If m_iShortPeriodRated = 1 AndAlso (m_sTransactionType <> "MTC" OrElse Not m_bIsMTCRatingRulesEnabled) Then
                ' Find short period rate
                ' Store the rate as if it is a standard pro-rata rate as it will be
                ' used in the same way.
                m_lReturn = CType(GetShortPeriodRate(v_lProductID:=nProductId, v_dtOldStartDate:=dtOldCoverStartDate,
                                                     v_dtOldEndDate:=dtOldExpiryDate,
                                                     v_dtStartDate:=dtCoverStartDate, v_dtEndDate:=dtExpiryDate,
                                                     r_dProRataRate:=m_dProRataRate), 
                                  gPMConstants.PMEReturnCode)

                ' Check return
                Select Case m_lReturn
                    Case gPMConstants.PMEReturnCode.PMTrue
                        bShortPeriodRated = True
                    Case gPMConstants.PMEReturnCode.PMNotFound
                        bShortPeriodRated = False
                    Case Else
                        bShortPeriodRated = False

                        ' Log Error Message and continue without SPR
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                           sMsg:="Unable to determine short period rating", vApp:=ACApp,
                                           vClass:=ACClass, vMethod:="PopulateRatingSections")
                End Select
            Else
                bShortPeriodRated = False
            End If

            'TN20010514 start - put in if statement (only get prorata rate when set in product)
            'JMK 01/08/2001 ... and always get prorata rate if Cancelling
            ' ISS1377 - PWF 25/11/2002 - Only pro-rata if no SPR's
            If Not bShortPeriodRated Then
                ' Peter Finney 10/09/2003 - Use property as this will do any checks for us
                If ProRata AndAlso (m_sTransactionType <> "MTC" OrElse Not m_bIsMTCRatingRulesEnabled) Then
                    'get pro rata rate
                    If v_bIsBackdatedMTA AndAlso dtOldExpiryDate > dtExpiryDate Then
                        'Fetch pro rata on the basis of original cover expiry date
                        m_lReturn = CType(GetProRataRate(v_lProductID:=nProductId,
                                                         v_dtOldStartDate:=dtOldCoverStartDate,
                                                         v_dtOldEndDate:=dtOldExpiryDate,
                                                         v_dtStartDate:=dtCoverStartDate,
                                                         v_dtEndDate:=dtOldExpiryDate,
                                                         r_dProRataRate:=m_dProRataRate), 
                                          gPMConstants.PMEReturnCode)
                    Else
                        m_lReturn = CType(GetProRataRate(v_lProductID:=nProductId,
                                                         v_dtOldStartDate:=dtOldCoverStartDate,
                                                         v_dtOldEndDate:=dtOldExpiryDate,
                                                         v_dtStartDate:=dtCoverStartDate, v_dtEndDate:=dtExpiryDate,
                                                         r_dProRataRate:=m_dProRataRate,
                                                         v_dtInceptionDate:=dtInceptionDate), 
                                          gPMConstants.PMEReturnCode)
                    End If
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return nResult
                    End If

                Else
                    m_dProRataRate = 1
                End If
            End If
            'TN20010417 End

            If sStatusFlag <> "D" Then
                ' If we're in portfolio transfer we should always use to original rating sections
                ' Do NOT get data from the output tables as it may have been amended!!
                If m_sTransactionType <> "PT" And m_sTransactionType <> "DRI" Then
                    'Now get the output records
                    bPMAddParameter.AddParameterLite(m_oDatabase, "PolicyBinderId", CStr(nPolicyBinder),
                                                     gPMConstants.PMEParameterDirection.PMParamInput,
                                                     gPMConstants.PMEDataType.PMLong, True)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "DataModel", m_sDataModel,
                                                     gPMConstants.PMEParameterDirection.PMParamInput,
                                                     gPMConstants.PMEDataType.PMString)
                    If gPMFunctions.ToSafeString(m_sRoundingSectionCode, "").Trim() <> "" Then
                        bPMAddParameter.AddParameterLite(m_oDatabase, "RoundingSectionCode",
                                                         m_sRoundingSectionCode.Trim(),
                                                         gPMConstants.PMEParameterDirection.PMParamInput,
                                                         gPMConstants.PMEDataType.PMString)
                    End If

                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=kGetGISOutputSQL, sSQLName:=kGetGISOutputName,
                                                      bStoredProcedure:=kGetGISOutputStored,
                                                      lNumberRecords:=gPMConstants.PMAllRecords,
                                                      vResultArray:=oArray)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If ' m_sTransactionType <> "PT"

                If (m_sTransactionType = "MTC" AndAlso Information.IsArray(oArray) AndAlso m_bIsMTCRatingRulesEnabled) OrElse
                   (m_sTransactionType = "MTC" AndAlso Information.IsArray(oArray) AndAlso bIsMandatoryRisk) Then

                    For lTemp As Integer = oArray.GetLowerBound(1) To oArray.GetUpperBound(1)


                        'oArray(ACOOriginalPremium, lTemp) = CDbl(oArray(ACOPremium, lTemp)) * -1
                        oArray(ACOOriginalPremium, lTemp) = gPMFunctions.ToSafeDouble(oArray(ACOPremium, lTemp), 0) * -1
                    Next
                End If

                ' *******************************************************************************************
                ' NOTE: This is getting the perils that WILL be used to calculate MTA default rating sections
                ' *******************************************************************************************
                If Not Information.IsArray(oArray) Then
                    If m_lOriginalRiskCnt <> -1 Then
                        'Now get the rating sections from the original risk
                        If m_sTransactionType = "MTR" Then
                            bPMAddParameter.AddParameterLite(m_oDatabase, "type", CStr(4),
                                                             gPMConstants.PMEParameterDirection.PMParamInput,
                                                             gPMConstants.PMEDataType.PMLong, True)
                        ElseIf (m_sTransactionType = "MTC") Then
                            bPMAddParameter.AddParameterLite(m_oDatabase, "type", CStr(6),
                                                             gPMConstants.PMEParameterDirection.PMParamInput,
                                                             gPMConstants.PMEDataType.PMLong, True)
                        Else
                            bPMAddParameter.AddParameterLite(m_oDatabase, "type", CStr(1),
                                                             gPMConstants.PMEParameterDirection.PMParamInput,
                                                             gPMConstants.PMEDataType.PMLong, True)
                        End If
                        bPMAddParameter.AddParameterLite(m_oDatabase, "insurancefilecnt", CStr(m_lInsuranceFileCnt),
                                                         gPMConstants.PMEParameterDirection.PMParamInput,
                                                         gPMConstants.PMEDataType.PMLong)
                        If r_lPostChangeRiskCnt > 0 AndAlso v_bIsBackdatedMTA Then
                            bPMAddParameter.AddParameterLite(m_oDatabase, "riskcnt", CStr(r_lPostChangeRiskCnt),
                                                             gPMConstants.PMEParameterDirection.PMParamInput,
                                                             gPMConstants.PMEDataType.PMLong)
                        Else
                            bPMAddParameter.AddParameterLite(m_oDatabase, "riskcnt", CStr(m_lOriginalRiskCnt),
                                                             gPMConstants.PMEParameterDirection.PMParamInput,
                                                             gPMConstants.PMEDataType.PMLong)
                        End If
                        bPMAddParameter.AddParameterLite(m_oDatabase, "roundingsectioncode",
                                                         m_sRoundingSectionCode.Trim(),
                                                         gPMConstants.PMEParameterDirection.PMParamInput,
                                                         gPMConstants.PMEDataType.PMString)

                        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetOriginalRatingSectionsSQL,
                                                          sSQLName:=ACGetOriginalRatingSectionsName,
                                                          bStoredProcedure:=ACGetOriginalRatingSectionsStored,
                                                          lNumberRecords:=gPMConstants.PMAllRecords,
                                                          vResultArray:=oArray)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                End If
            End If
            'End If

            'This now gets heinous.  There's already code later on to take account of rating sections
            'that were on the last version but not this one.  BUT it turns out that the order is
            'important.  If A is replaced by B we need to have A followed by B in the list.

            'The safest way I can think of is to reverse out the old data and then process the
            'new...

            'Of course, if we've no original risk, this is unnecessary...
            ' **************************************************************************************
            ' NOTE: This is getting the perils that WILL be used to calculate RETURN rating sections
            ' **************************************************************************************
            If m_lOriginalRiskCnt <> -1 Then
                'Now get the rating sections from the original risk
                If m_sTransactionType = "MTR" Then
                    bPMAddParameter.AddParameterLite(m_oDatabase, "type", CStr(5),
                                                     gPMConstants.PMEParameterDirection.PMParamInput,
                                                     gPMConstants.PMEDataType.PMLong, True)
                Else
                    bPMAddParameter.AddParameterLite(m_oDatabase, "type", CStr(2),
                                                     gPMConstants.PMEParameterDirection.PMParamInput,
                                                     gPMConstants.PMEDataType.PMLong, True)
                End If
                bPMAddParameter.AddParameterLite(m_oDatabase, "insurancefilecnt", CStr(m_lInsuranceFileCnt),
                                                 gPMConstants.PMEParameterDirection.PMParamInput,
                                                 gPMConstants.PMEDataType.PMLong)
                bPMAddParameter.AddParameterLite(m_oDatabase, "riskcnt", CStr(m_lOriginalRiskCnt),
                                                 gPMConstants.PMEParameterDirection.PMParamInput,
                                                 gPMConstants.PMEDataType.PMLong)
                bPMAddParameter.AddParameterLite(m_oDatabase, "roundingsectioncode", m_sRoundingSectionCode.Trim(),
                                                 gPMConstants.PMEParameterDirection.PMParamInput,
                                                 gPMConstants.PMEDataType.PMString)

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetOriginalRatingSectionsSQL,
                                                  sSQLName:=ACGetOriginalRatingSectionsName,
                                                  bStoredProcedure:=ACGetOriginalRatingSectionsStored,
                                                  lNumberRecords:=gPMConstants.PMAllRecords,
                                                  vResultArray:=oRatingSectionArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                If m_lOriginalRiskCnt <> -1 AndAlso v_bIsBackdatedMTA AndAlso r_lPostChangeRiskCnt <> 0 Then
                    If m_sTransactionType = "MTC" AndAlso Not m_bIsMTCRatingRulesEnabled Then

                        bPMAddParameter.AddParameterLite(m_oDatabase, "risk_cnt", CStr(m_lOriginalRiskCnt),
                                                         gPMConstants.PMEParameterDirection.PMParamInput,
                                                         gPMConstants.PMEDataType.PMLong, True)
                    Else

                        bPMAddParameter.AddParameterLite(m_oDatabase, "risk_cnt", CStr(m_lRiskID),
                                                         gPMConstants.PMEParameterDirection.PMParamInput,
                                                         gPMConstants.PMEDataType.PMLong, True)
                    End If
                    ' Call AddParameterLite(m_oDatabase, "risk_cnt", m_lRiskID, PMParamInput, PMLong, True)

                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRiskInceptionDateSQL,
                                                      sSQLName:=ACGetRiskInceptionDateName,
                                                      bStoredProcedure:=ACGetRiskInceptionDateStored)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    If _
                        IsNothing(m_oDatabase.Records.Item(0).Fields()("inception_date")) = False AndAlso
                        Convert.IsDBNull(m_oDatabase.Records.Item(0).Fields()("inception_date")) = False Then
                        '    Return gPMConstants.PMEReturnCode.PMFalse
                        'Else
                        dtRiskInceptionDate = m_oDatabase.Records.Item(0).Fields()("inception_date")
                    End If

                    If v_dtMTADateCurrent < dtRiskInceptionDate Then
                        If Not v_bExistsPreAndPost Then
                            'Developer Guide No. 12
                            oRatingSectionArray = Nothing
                        End If
                    End If
                    'End If
                End If
                'pooja
                'If m_sTransactionType = "MTA" Then
                '    oArray = Nothing
                'End If

                m_lReturn = CType(MergeArrays(vMTAArray:=oArray, vOriginalArray:=oRatingSectionArray,
                                              vMergedArray:=oMergedArray), 
                                  gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                oArray = oMergedArray
            End If

            'Now we worry about the decline or refer
            m_sDeclineReasons = ""
            m_sReferReasons = ""
            m_sMessages = ""

            If Information.IsArray(oArray) Then
                'It's automatically rated, so _now_ we have to clear it down
                'First remove the contents of the rating section and peril tables

                m_lReturn = CType(DeleteSectionAndPerils(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                For lTemp As Integer = oArray.GetLowerBound(1) To oArray.GetUpperBound(1)


                    If CStr(oArray(ACODeclineReason, lTemp)) <> "" Then

                        m_sDeclineReasons = m_sDeclineReasons & CStr(oArray(ACODeclineReason, lTemp)) & Strings.Chr(13) &
                                            Strings.Chr(10)

                    ElseIf (CStr(oArray(ACOReferReason, lTemp)) <> "") Then

                        m_sReferReasons = m_sReferReasons & CStr(oArray(ACOReferReason, lTemp)) & Strings.Chr(13) &
                                          Strings.Chr(10)

                    ElseIf (CStr(oArray(ACOMessage, lTemp)) <> "") Then

                        m_sMessages = m_sMessages & CStr(oArray(ACOMessage, lTemp)) & Strings.Chr(13) & Strings.Chr(10)

                    Else

                        nPolicyRatingSectionId = -1
                        nRiskRatingSectionId = -1
                        dAnnualRate = 0


                        If CStr(oArray(ACOPolicyRatingSectionType, lTemp)) <> "" Then

                            m_lReturn = CType(GetPolicyIdFromCode(
                                sCode:=CStr(oArray(ACOPolicyRatingSectionType, lTemp)), lId:=nPolicyRatingSectionId,
                                cAnnualRate:=dAnnualRate, lRateTypeId:=nRateTypeId), 
                                              gPMConstants.PMEReturnCode)
                        End If


                        If CStr(oArray(ACORiskRatingSectionType, lTemp)) <> "" Then

                            m_lReturn = CType(GetRatingIdFromCode(sCode:=CStr(oArray(ACORiskRatingSectionType, lTemp)),
                                                                  lId:=nRiskRatingSectionId,
                                                                  cAnnualRate:=dAnnualRate, lRateTypeId:=nRateTypeId,
                                                                  iCurrencyID:=nCurrencyId), 
                                              gPMConstants.PMEReturnCode)
                        End If


                        Dim dbNumericTemp As Decimal

                        If oArray(ACORate, lTemp) = "" OrElse oArray(ACORate, lTemp) Is Nothing Then ' 2819
                            dAnnualRate = 0
                        Else
                            dAnnualRate = IIf(
                               Decimal.TryParse(CStr(oArray(ACORate, lTemp)), NumberStyles.Number,
                                                CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp),
                               CDec(oArray(ACORate, lTemp)), 0)

                        End If

                        'Now we return both rate and type, type = "" or 0 means there wasn't one
                        '"" = we didn't look, 0 means we looked but there wasn't one

                        If CStr(oArray(ACORateTypeId, lTemp)) <> "" Then

                            If CDbl(oArray(ACORateTypeId, lTemp)) <> 0 Then

                                nRateTypeId = CInt(oArray(ACORateTypeId, lTemp))
                            End If
                        End If

                        If (nPolicyRatingSectionId <> -1) OrElse (nRiskRatingSectionId <> -1) Then
                            'We need to pro-rata the 'this premium' value.
                            'We've already got the original risk cnt and the pro-rata percentage.

                            'We get the rating section record of the proper type for the original risk
                            'Then, this premium = (annual premium - original annual premium) * pro rata %

                            'Tracy Richards - 04/09/03 Protect against blank strings

                            If CStr(oArray(ACOOriginalPremium, lTemp)) = "" Then
                                dOriginalAnnualPremium = 0
                            Else

                                dOriginalAnnualPremium = CDec(oArray(ACOOriginalPremium, lTemp))
                            End If

                            'JMK 01/08/2001 - make current Annual premium zero for Cancellation
                            If m_sTransactionType = "MTC" AndAlso Not bIsMandatoryRisk Then
                                dAnnualPremium = 0
                            Else
                                'Tracy Richards - 04/09/03 Protect against blank strings

                                If CStr(oArray(ACOPremium, lTemp)) = "" Then
                                    dAnnualPremium = 0
                                Else

                                    dAnnualPremium = CDec(oArray(ACOPremium, lTemp))
                                End If
                            End If

                            ' 05/08/2003 Peter Finney - make current premium zero for calculation
                            '   but not storage on MTA original sections
                            ' 12/09/2003 Peter Finney - don't multiply by the ProRate "flag"! only
                            '   the rate. If pro-rata is off the rate will be 1!

                            If _
                                (m_sTransactionType = "MTC" AndAlso bIsMandatoryRisk) OrElse
                                (m_sTransactionType = "MTA" OrElse m_sTransactionType = "MTR" OrElse m_sTransactionType = "DRI" OrElse
                                 m_sTransactionType = "PT") AndAlso CDbl(oArray(ACOOriginalFlag, lTemp)) = 1 Then
                                If _
                                    (dtExpiryDate < dtOldExpiryDate AndAlso m_bExtensionOfExpiryDate) OrElse
                                     Not v_bIsBackdatedMTA Then
                                    'fetch the proratarate applied on the original risk
                                    If m_iMTAProRata = 1 Then
                                        If Not IsTemporaryMTA Then
                                            m_lReturn = CType(GetProRataRate(v_lProductID:=nProductId,
                                                                             v_dtOldStartDate:=dtOldCoverStartDate,
                                                                             v_dtOldEndDate:=dtOldExpiryDate,
                                                                             v_dtStartDate:=dtCoverStartDate,
                                                                             v_dtEndDate:=dtOldExpiryDate,
                                                                             r_dProRataRate:=dOriginalRiskProrataRate,
                                                                             v_dtInceptionDate:=dtInceptionDate), 
                                                              gPMConstants.PMEReturnCode)
                                        Else
                                            m_lReturn = CType(GetProRataRate(v_lProductID:=nProductId,
                                                                             v_dtOldStartDate:=dtOldCoverStartDate,
                                                                             v_dtOldEndDate:=dtOldExpiryDate,
                                                                             v_dtStartDate:=dtCoverStartDate,
                                                                             v_dtEndDate:=dtExpiryDate,
                                                                             r_dProRataRate:=dOriginalRiskProrataRate,
                                                                             v_dtInceptionDate:=dtInceptionDate), 
                                                              gPMConstants.PMEReturnCode)
                                        End If
                                    Else
                                        dOriginalRiskProrataRate = 1
                                    End If
                                    ' If original prorata disabled
                                    If ToSafeInteger(oArray(ACODisableOriginalProRata, lTemp), 0) = 1 Then
                                        dThisPremium = Math.Round(-(dOriginalAnnualPremium), 4)
                                    Else
                                        dThisPremium = Math.Round(-(dOriginalAnnualPremium) * dOriginalRiskProrataRate, 4)
                                    End If
                                Else
                                    ' if original prorata disabled
                                    If ToSafeInteger(oArray(ACODisableOriginalProRata, lTemp), 0) = 1 Then
                                        dThisPremium = Math.Round(-(dOriginalAnnualPremium), 4)
                                    Else
                                        dThisPremium = Math.Round(-(dAnnualPremium) * m_dProRataRate, 4)
                                    End If
                                End If
                            Else
                                ' if new prorata disabled
                                If ToSafeInteger(oArray(ACODisableNewProRata, lTemp), 0) = 1 Then
                                    m_dProRataRate = 1
                                End If
                                dThisPremium = Math.Round((dAnnualPremium - dOriginalAnnualPremium) * m_dProRataRate, 4)
                            End If

                            ' ensure that the state and country are set to valid values
                            ' before add section and perils is called

                            If CStr(oArray(ACOCountryId, lTemp)) = "" Then

                                oArray(ACOCountryId, lTemp) = 0
                            End If


                            If CStr(oArray(ACOStateId, lTemp)) = "" Then

                                oArray(ACOStateId, lTemp) = 0
                            End If


                            If _
                                m_iRoundPremium = 1 AndAlso
                                m_sRoundingSectionCode = CStr(oArray(ACORiskRatingSectionType, lTemp)) Then


                                sSQL = "SELECT rs.risk_cnt" & Strings.Chr(13) & Strings.Chr(10) &
                                       "FROM rating_section rs" & Strings.Chr(13) & Strings.Chr(10) &
                                       "WHERE rs.risk_cnt = " & CStr(m_lRiskID) & Strings.Chr(13) & Strings.Chr(10) &
                                       "AND rs.rating_section_type_id = " & CStr(m_lRoundingSection) & Strings.Chr(13) &
                                       Strings.Chr(10) &
                                       "AND rs.original_flag = " & CStr(oArray(ACOOriginalFlag, lTemp)) &
                                       Strings.Chr(13) & Strings.Chr(10)


                                m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL,
                                                                  sSQLName:="Check rounding already there",
                                                                  bStoredProcedure:=False,
                                                                  lNumberRecords:=gPMConstants.PMAllRecords,
                                                                  vResultArray:=otempArray)

                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    Return gPMConstants.PMEReturnCode.PMFalse
                                End If

                                If Not Information.IsArray(otempArray) Then

                                    If _
                                        m_sTransactionType = "REN" OrElse m_sTransactionType = "MTA" OrElse
                                        m_sTransactionType = "MTC" OrElse m_sTransactionType = "MTR" Then

                                        m_lReturn =
                                            CType(GetRoundingSectionAmounts(r_cAnnualPremium:=dRoundingAnnualPremium,
                                                                            r_cThisPremium:=dRoundingThisPremium,
                                                                            v_lOriginal_Flag:=
                                                                               CInt(oArray(ACOOriginalFlag, lTemp))), 
                                                  gPMConstants.PMEReturnCode)

                                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                            Return gPMConstants.PMEReturnCode.PMFalse
                                        End If

                                    Else
                                        dRoundingAnnualPremium = 0
                                        dRoundingThisPremium = 0
                                    End If

                                    dAnnualPremium = Math.Round(dRoundingAnnualPremium, 4)
                                    dThisPremium = Math.Round(dRoundingThisPremium, 4)

                                End If
                            End If
                            If m_sTransactionType = "MTC" Then
                                m_lReturn = CType(AddSectionAndPerils(v_lRatingSectionTypeId:=nRiskRatingSectionId,
                                                                      v_lPolicySectionTypeId:=nPolicyRatingSectionId,
                                                                      v_cAnnualPremium:=dOriginalAnnualPremium,
                                                                      v_cThisPremium:=dThisPremium,
                                                                      v_cAnnualRate:=dAnnualRate,
                                                                      v_cSumInsured:=
                                                                         gPMFunctions.ToSafeCurrency(oArray(ACOSumInsured,
                                                                                                            lTemp)),
                                                                      v_lRateTypeId:=nRateTypeId,
                                                                      v_lOriginalFlag:=
                                                                         gPMFunctions.ToSafeLong(oArray(ACOOriginalFlag,
                                                                                                        lTemp)),
                                                                      v_iDefinedCurrencyID:=nCurrencyId,
                                                                      v_lCountryID:=
                                                                         gPMFunctions.ToSafeLong(oArray(ACOCountryId, lTemp)),
                                                                      v_lStateID:=
                                                                         gPMFunctions.ToSafeLong(oArray(ACOStateId, lTemp)),
                                                                      v_iAutoCalculated:=1,
                                                                      v_iIsAmended:=0, v_cCalculatedPremium:=0, v_sOverrideReason:="",
                                                                      v_lEarningPatternId:=
                                                                         gPMFunctions.ToSafeLong(oArray(ACOEarningPatternID,
                                                                                                        lTemp))), 
                                                  gPMConstants.PMEReturnCode)
                            Else
                                m_lReturn = CType(AddSectionAndPerils(v_lRatingSectionTypeId:=nRiskRatingSectionId,
                                                                    v_lPolicySectionTypeId:=nPolicyRatingSectionId,
                                                                    v_cAnnualPremium:=dAnnualPremium,
                                                                    v_cThisPremium:=dThisPremium,
                                                                    v_cAnnualRate:=dAnnualRate,
                                                                    v_cSumInsured:=
                                                                       gPMFunctions.ToSafeCurrency(oArray(ACOSumInsured,
                                                                                                          lTemp)),
                                                                    v_lRateTypeId:=nRateTypeId,
                                                                    v_lOriginalFlag:=
                                                                       gPMFunctions.ToSafeLong(oArray(ACOOriginalFlag,
                                                                                                      lTemp)),
                                                                    v_iDefinedCurrencyID:=nCurrencyId,
                                                                    v_lCountryID:=
                                                                       gPMFunctions.ToSafeLong(oArray(ACOCountryId, lTemp)),
                                                                    v_lStateID:=
                                                                       gPMFunctions.ToSafeLong(oArray(ACOStateId, lTemp)),
                                                                    v_iAutoCalculated:=1,
                                                                    v_iIsAmended:=0, v_cCalculatedPremium:=0, v_sOverrideReason:="",
                                                                    v_lEarningPatternId:=
                                                                       gPMFunctions.ToSafeLong(oArray(ACOEarningPatternID,
                                                                                                      lTemp))), 
                                                gPMConstants.PMEReturnCode)

                            End If

                        End If

                    End If

                Next lTemp
            Else
                If m_sTransactionType = "REN" Then
                    'We've copied the rating section records, but we need to regenerate the perils
                    'Let's reuse some code...
                    bPMAddParameter.AddParameterLite(m_oDatabase, "type", CStr(3),
                                                     gPMConstants.PMEParameterDirection.PMParamInput,
                                                     gPMConstants.PMEDataType.PMLong, True)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "insurancefilecnt", CStr(m_lInsuranceFileCnt),
                                                     gPMConstants.PMEParameterDirection.PMParamInput,
                                                     gPMConstants.PMEDataType.PMLong)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "riskcnt", CStr(m_lRiskID),
                                                     gPMConstants.PMEParameterDirection.PMParamInput,
                                                     gPMConstants.PMEDataType.PMLong)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "roundingsectioncode", m_sRoundingSectionCode.Trim(),
                                                     gPMConstants.PMEParameterDirection.PMParamInput,
                                                     gPMConstants.PMEDataType.PMString)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "renewalproratarate", CStr(m_dProRataRate),
                                                     gPMConstants.PMEParameterDirection.PMParamInput,
                                                     gPMConstants.PMEDataType.PMDouble)

                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetOriginalRatingSectionsSQL,
                                                      sSQLName:=ACGetOriginalRatingSectionsName,
                                                      bStoredProcedure:=ACGetOriginalRatingSectionsStored,
                                                      lNumberRecords:=gPMConstants.PMAllRecords,
                                                      vResultArray:=oArray)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If Information.IsArray(oArray) Then
                        'First remove the contents of the rating section and peril tables
                        m_lReturn = CType(DeleteSectionAndPerils(), gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If


                        For lTemp As Integer = oArray.GetLowerBound(1) To oArray.GetUpperBound(1)

                            nPolicyRatingSectionId = -1
                            nRiskRatingSectionId = -1
                            dAnnualRate = 0


                            If CStr(oArray(ACOPolicyRatingSectionType, lTemp)) <> "" Then

                                m_lReturn = CType(GetPolicyIdFromCode(
                                    sCode:=CStr(oArray(ACOPolicyRatingSectionType, lTemp)),
                                    lId:=nPolicyRatingSectionId, cAnnualRate:=dAnnualRate,
                                    lRateTypeId:=nRateTypeId), 
                                                  gPMConstants.PMEReturnCode)
                            End If


                            If CStr(oArray(ACORiskRatingSectionType, lTemp)) <> "" Then

                                m_lReturn = CType(GetRatingIdFromCode(
                                    sCode:=CStr(oArray(ACORiskRatingSectionType, lTemp)), lId:=nRiskRatingSectionId,
                                    cAnnualRate:=dAnnualRate, lRateTypeId:=nRateTypeId, iCurrencyID:=nCurrencyId), 
                                                  gPMConstants.PMEReturnCode)
                            End If

                            'Override it with that from the script

                            If CStr(oArray(ACORate, lTemp)) <> "" Then

                                dAnnualRate = CDec(oArray(ACORate, lTemp))
                            End If

                            If (nPolicyRatingSectionId <> -1) OrElse (nRiskRatingSectionId <> -1) Then


                                dThisPremium = Math.Round(CDec(oArray(ACOPremium, lTemp)), 4)
                                dAnnualPremium = Math.Round(CDec(oArray(ACOOriginalPremium, lTemp)), 4)
                                nRateTypeId = gPMFunctions.ToSafeLong(oArray(ACORateTypeId, lTemp))

                                ' ensure that the state and country are set to valid values
                                ' before add section and perils is called

                                If CStr(oArray(ACOCountryId, lTemp)) = "" Then

                                    oArray(ACOCountryId, lTemp) = 0
                                End If


                                If CStr(oArray(ACOStateId, lTemp)) = "" Then

                                    oArray(ACOStateId, lTemp) = 0
                                End If


                                m_lReturn = CType(AddSectionAndPerils(v_lRatingSectionTypeId:=nRiskRatingSectionId,
                                                                      v_lPolicySectionTypeId:=nPolicyRatingSectionId,
                                                                      v_cAnnualPremium:=dAnnualPremium,
                                                                      v_cThisPremium:=dThisPremium,
                                                                      v_cAnnualRate:=dAnnualRate,
                                                                      v_cSumInsured:=
                                                                         CDec(oArray(ACOSumInsured, lTemp)),
                                                                      v_lRateTypeId:=nRateTypeId,
                                                                      v_lOriginalFlag:=
                                                                         CInt(oArray(ACOOriginalFlag, lTemp)),
                                                                      v_iDefinedCurrencyID:=nCurrencyId,
                                                                      v_lCountryID:=CInt(oArray(ACOCountryId, lTemp)),
                                                                      v_lStateID:=CInt(oArray(ACOStateId, lTemp)),
                                                                      v_iIsAmended:=0, v_cCalculatedPremium:=0, v_sOverrideReason:="",
                                                                      v_iAutoCalculated:=1,
                                                                      v_lEarningPatternId:=
                                                                         CInt(oArray(ACOEarningPatternID, lTemp))), 
                                                  gPMConstants.PMEReturnCode)

                            End If

                        Next lTemp

                    End If

                End If

            End If

            'call new proc for peril and rating section insertion 

            If (m_sTransactionType = "MTA") Then

                m_lReturn = CType(DeleteAndCreateNewSectionAndPerilsForUtility(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
           

            If m_iRoundPremium = 1 Then
                'we will be needing to round this premium total
                'so create a rounding rating section ready for later

                sSQL = "SELECT rs.risk_cnt" & Strings.Chr(13) & Strings.Chr(10) &
                       "FROM rating_section rs" & Strings.Chr(13) & Strings.Chr(10) &
                       "WHERE rs.risk_cnt = " & CStr(m_lRiskID) & Strings.Chr(13) & Strings.Chr(10) &
                       "AND rs.rating_section_type_id = " & CStr(m_lRoundingSection) & Strings.Chr(13) & Strings.Chr(10) &
                       "AND rs.original_flag = 0" & Strings.Chr(13) & Strings.Chr(10)

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="Check rounding already there",
                                                  bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords,
                                                  vResultArray:=oArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Not Information.IsArray(oArray) Then

                    dRoundingAnnualPremium = 0
                    dRoundingThisPremium = 0

                    'We only want to specify rounding amounts for renewals as these can
                    'skip the peril allocation screen (where rounding is worked out) if
                    'done automatically.

                    ' PN60341 - Even if its NB - of ShortTerm - AMIT - ' Did we miss any other m_sTransactionType ??? -;
                    If _
                        m_sTransactionType = "NB" OrElse m_sTransactionType = "REN" OrElse m_sTransactionType = "MTA" OrElse
                        m_sTransactionType = "MTC" OrElse m_sTransactionType = "MTR" Then
                        'Get the rounding amounts and the currency of t
                        m_lReturn = CType(GetRoundingSectionAmounts(r_cAnnualPremium:=dRoundingAnnualPremium,
                                                                    r_cThisPremium:=dRoundingThisPremium), 
                                          gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If

                    m_lReturn = CType(AddSectionAndPerils(v_lRatingSectionTypeId:=m_lRoundingSection,
                                                          v_lPolicySectionTypeId:=-1,
                                                          v_cAnnualPremium:=Math.Round(dRoundingAnnualPremium, 2),
                                                          v_cThisPremium:=Math.Round(dRoundingThisPremium, 2),
                                                          v_cAnnualRate:=0, v_cSumInsured:=0, v_lRateTypeId:=1,
                                                          v_lOriginalFlag:=0, v_iDefinedCurrencyID:=nCurrencyId,
                                                          v_lCountryID:=0, v_lStateID:=0, v_iAutoCalculated:=1,
                                                          v_iIsAmended:=0, v_cCalculatedPremium:=0, v_sOverrideReason:="", v_lEarningPatternId:=0), 
                                      gPMConstants.PMEReturnCode)
                End If

            End If
            'JMK 26/07/2001 end

            'Now let's update the Risk Status...
            nRiskStatus = 0

            If m_sDeclineReasons <> "" Then
                nRiskStatus = 2
            ElseIf (m_sReferReasons <> "") Then
                nRiskStatus = 1
            End If

            If nRiskStatus = 0 Then
                m_lReturn = CType(CheckQuestions(bAllAnswered:=bAllAnswered, sRequirement:="pre_quote"), 
                                  gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Not bAllAnswered Then
                    nRiskStatus = 7
                    m_bQuestionUnanswered = True
                End If
            End If

            If nRiskStatus = 0 Then
                m_lReturn = CType(CheckQuestions(bAllAnswered:=bAllAnswered, sRequirement:="post_quote"), 
                                  gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Not bAllAnswered Then
                    nRiskStatus = 6
                    m_bQuestionUnanswered = True
                End If
            End If

            If nRiskStatus = 0 Then
                m_lReturn = CType(CheckQuestions(bAllAnswered:=bAllAnswered, sRequirement:="purchase"), 
                                  gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Not bAllAnswered Then
                    nRiskStatus = 5
                    m_bQuestionUnanswered = True
                End If
            End If

            If nRiskStatus = 0 Then
                nRiskStatus = 8
            End If

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=CStr(m_lRiskID),
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_status_id", vValue:=CStr(nRiskStatus),
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateRiskStatusSQL, sSQLName:=ACUpdateRiskStatusName,
                                              bStoredProcedure:=ACUpdateRiskStatusStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If the Result Array parameter has been passed then
            ' return the rating section details


            ' get the data for returning back to the caller
            m_lReturn = CType(GetRatingSections(vResultArray:=r_vResultArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return nResult

        Catch excep As System.Exception


            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PopulateRatingSections Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateRatingSections", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult



        End Try
    End Function

    Public Function GetOriginalAnnualPremium(ByRef lRiskRatingSectionId As Integer, ByRef lPolicyRatingSectionId As Integer, ByRef cOriginalAnnualPremium As Decimal) As Integer 'PN40959 (RC)
        Return GetOriginalAnnualPremium(lRiskRatingSectionId:=lRiskRatingSectionId, lPolicyRatingSectionId:=lPolicyRatingSectionId, cOriginalAnnualPremium:=cOriginalAnnualPremium, lRatingSectionCnt:=0)
    End Function

    Public Function GetOriginalAnnualPremium(ByRef lRiskRatingSectionId As Integer, ByRef lPolicyRatingSectionId As Integer, ByRef cOriginalAnnualPremium As Decimal, ByRef lRatingSectionCnt As Integer) As Integer 'PN40959 (RC)

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            cOriginalAnnualPremium = 0

            If m_lOriginalRiskCnt = -1 Then
                Return result
            End If

            sSQL = "SELECT CASE is_amended WHEN 1 THEN this_premium ELSE annual_premium END" & Strings.Chr(13) & Strings.Chr(10) & _
                   "FROM rating_section" & Strings.Chr(13) & Strings.Chr(10) & _
                   "WHERE risk_cnt = " & CStr(m_lOriginalRiskCnt) & Strings.Chr(13) & Strings.Chr(10)

            If lRiskRatingSectionId = -1 Then
                sSQL = sSQL & "AND rating_section_type_id IS NULL" & Strings.Chr(13) & Strings.Chr(10)
            Else
                sSQL = sSQL & "AND rating_section_type_id = " & CStr(lRiskRatingSectionId) & Strings.Chr(13) & Strings.Chr(10)
            End If

            If lPolicyRatingSectionId = -1 Then
                sSQL = sSQL & "AND policy_section_type_id IS NULL" & Strings.Chr(13) & Strings.Chr(10)
            Else
                sSQL = sSQL & "AND policy_section_type_id = " & CStr(lPolicyRatingSectionId) & Strings.Chr(13) & Strings.Chr(10)
            End If
            sSQL = sSQL & "AND rating_section_id = " & CStr(lRatingSectionCnt) & Strings.Chr(13) & Strings.Chr(10) 'PN40959 (RC)
            sSQL = sSQL & "AND original_flag=0 " & Strings.Chr(13) & Strings.Chr(10)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetOriginalAnnualPremium", bStoredProcedure:=False, vResultArray:=vArray, lNumberRecords:=gPMConstants.PMAllRecords)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Information.IsArray(vArray) Then
                Return result
            End If


            cOriginalAnnualPremium = CDec(vArray(0, 0))


            'Developer Guide No. 12
            vArray = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOriginalAnnualPremium Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOriginalAnnualPremium", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetRatingSections(ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            m_oDatabase.Parameters.Clear()

            'Add the necessary parameters

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(m_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_id", vValue:=CStr(m_lRiskID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


            'Fetch the records
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectRatingSectionSQL, sSQLName:=ACSelectRatingSectionName, bStoredProcedure:=ACSelectRatingSectionStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

            'Return the result

            Return m_lReturn

        Catch excep As System.Exception



            'Error
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInsuranceHeaderDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInsuranceHeaderDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    Public Function GetRatingSectionTypes(ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            'Clear all the parameters
            m_oDatabase.Parameters.Clear()

            'Add the necessary parameters

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(m_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


            'Fetch the records
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectRatingSectionTypeSQL, sSQLName:=ACSelectRatingSectionTypeName, bStoredProcedure:=ACSelectRatingSectionTypeStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

            'Return the result

            Return m_lReturn

        Catch excep As System.Exception



            'Error
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRatingSectionTypes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRatingSectionTypes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    Public Function GetRateTypes(ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            'Clear all the parameters
            m_oDatabase.Parameters.Clear()

            'Fetch the records
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectRateTypesSQL, sSQLName:=ACSelectRateTypesName, bStoredProcedure:=ACSelectRateTypesStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

            'Return the result

            Return m_lReturn

        Catch excep As System.Exception



            'Error
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRateTypes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRateTypes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateRisk (Public)
    '
    ' Description: Updates the risk with the premium and sum insured.
    '
    ' ***************************************************************** '
    Public Function UpdateRisk() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=CStr(m_lRiskID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' 01/08/2003 Peter Finney - Add actual proratarate to the risk
            ' Note: At the moment it appears this value is always used. We
            ' may need to tighten up on it later.
            m_lReturn = m_oDatabase.Parameters.Add(sName:="pro_rata_rate", vValue:=CStr(m_dProRataRate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateRiskSQL, sSQLName:=ACUpdateRiskName, bStoredProcedure:=ACUpdateRiskStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRisk", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name :ApplyCoinsurance
    '
    ' Desc : apply coinsurance shares to values in peril table
    '
    ' Hist : 11 June 2001 Created - Tinny
    '
    ' ***************************************************************** '
    Public Function ApplyCoinsurance(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = BeginTrans()

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = m_oDatabase.SQLAction(sSQL:=ACApplyCoinsuranceSQL, sSQLName:=ACApplyCoinsuranceName, bStoredProcedure:=ACApplyCoinsuranceStored)


            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return result
            End If

            result = CommitTrans()

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ApplyCoinsurance Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ApplyCoinsurance", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name :ApplyCoinsuranceToRisk
    '
    ' Desc : apply coinsurance shares to values in peril table
    '
    ' Hist : 23 July 2001 - Based on dodgy Tinny code
    '
    ' ***************************************************************** '
    Public Function ApplyCoinsuranceToRisk() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(m_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=CStr(m_lRiskID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACApplyCoinsuranceToRiskSQL, sSQLName:=ACApplyCoinsuranceToRiskName, bStoredProcedure:=ACApplyCoinsuranceToRiskStored)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return result
            End If

            m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ApplyCoinsuranceToRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ApplyCoinsuranceToRisk", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    Private Function CheckQuestions(ByRef bAllAnswered As Boolean, ByRef sRequirement As String) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vArray(,) As Object
        Dim sTable As String = ""
        Dim sDataType As New StringBuilder
        Dim sColumnType As New StringBuilder
        Dim sColumns As New StringBuilder



        result = gPMConstants.PMEReturnCode.PMTrue

        bAllAnswered = False

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="que_type", vValue:=sRequirement, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)


        m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=CStr(m_lRiskID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckMandatoryQueSQL, sSQLName:=ACCheckMandatoryQueName, bStoredProcedure:=ACCheckMandatoryQueStored, vResultArray:=vArray, lNumberRecords:=gPMConstants.PMAllRecords)


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Information.IsArray(vArray) Then
            bAllAnswered = True
            Return result
        End If

        sTable = ""
        sColumns = New StringBuilder("")
        sColumnType = New StringBuilder("")
        sDataType = New StringBuilder("")

        For lTemp As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)

            If CStr(vArray(0, lTemp)) <> sTable Then
                If sTable <> "" Then
                    m_lReturn = CType(LookForNulls(sTable:=sTable, sColumns:=sColumns.ToString(), bAllAnswered:=bAllAnswered, sColumnType:=sColumnType.ToString(), sDataType:=sDataType.ToString()), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If Not bAllAnswered Then
                        Return result
                    End If
                End If


                sTable = CStr(vArray(0, lTemp))
                sColumns = New StringBuilder("")
                sColumnType = New StringBuilder("")
                sDataType = New StringBuilder("")
            End If


            sColumns.Append(CStr(vArray(1, lTemp)) & ",")

            sColumnType.Append(CStr(vArray(2, lTemp)) & ",")

            sDataType.Append(CStr(vArray(3, lTemp)) & ",")
        Next lTemp

        If sTable <> "" Then
            m_lReturn = CType(LookForNulls(sTable:=sTable, sColumns:=sColumns.ToString(), bAllAnswered:=bAllAnswered, sColumnType:=sColumnType.ToString(), sDataType:=sDataType.ToString()), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not bAllAnswered Then
                Return result
            End If
        End If

        Return result

    End Function

    Private Function LookForNulls(ByRef sTable As String, ByRef sColumns As String, ByRef bAllAnswered As Boolean, Optional ByRef sColumnType As String = "", Optional ByRef sDataType As String = "") As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vArray(,) As Object
        Dim sColType As String = ""
        Dim sTempDataType As String = ""
        Dim iPos As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        bAllAnswered = True

        If (sTable = "") Or (sColumns = "") Then
            Return result
        End If

        If sColumns.EndsWith(",") Then
            sColumns = sColumns.Substring(0, sColumns.Length - 1)
        End If

        sSQL = "SELECT " & sColumns & Strings.Chr(13) & Strings.Chr(10) & _
               "FROM " & sTable & " o," & Strings.Chr(13) & Strings.Chr(10) & _
               m_sDataModel & "_policy_binder pb," & Strings.Chr(13) & Strings.Chr(10) & _
               "gis_policy_link pl" & Strings.Chr(13) & Strings.Chr(10) & _
               "WHERE pl.gis_policy_link_id = pb.gis_policy_link_id" & Strings.Chr(13) & Strings.Chr(10) & _
               "AND o." & m_sDataModel & "_policy_binder_id = pb." & m_sDataModel & "_policy_binder_id" & Strings.Chr(13) & Strings.Chr(10) & _
               "AND pl.insurance_file_cnt = " & CStr(m_lInsuranceFolderCnt) & Strings.Chr(13) & Strings.Chr(10) & _
               "AND pl.risk_id = " & CStr(m_lRiskID) & Strings.Chr(13) & Strings.Chr(10)

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="LookForNulls", bStoredProcedure:=False, vResultArray:=vArray, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords)


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Information.IsArray(vArray) Then
            bAllAnswered = True
            Return result
        End If


        For lTemp As Integer = vArray.GetLowerBound(0) To vArray.GetUpperBound(0)
            iPos = (sColumnType.IndexOf(","c) + 1)
            If iPos > 0 Then
                sColType = Mid(sColumnType, 1, iPos - 1)
                sColumnType = Mid(sColumnType, iPos + 1)
            Else
                sColType = sColumnType
            End If

            iPos = (sDataType.IndexOf(","c) + 1)
            If iPos > 0 Then
                sTempDataType = Mid(sDataType, 1, iPos - 1)
                sDataType = Mid(sDataType, iPos + 1)
            Else
                sColType = sColumnType
            End If


            For lTemp2 As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)
                Dim auxVar As Object = vArray(lTemp, lTemp2)



                If (Convert.IsDBNull(auxVar) Or IsNothing(auxVar)) And sTempDataType <> "20" Then
                    'Check Nulls for anything apart from check boxes
                    bAllAnswered = False
                    Return result
                ElseIf Convert.ToString(vArray(lTemp, lTemp2)).trim() = "2" And sColType = "13" And sTempDataType = "20" Then
                    'Check if tri-state check box is in UNKNOWN state i.e., value = 2
                    bAllAnswered = False
                    Return result
                End If
            Next
        Next lTemp

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetPolicyIdFromCode (Private)
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function GetPolicyIdFromCode(ByRef sCode As String, ByRef lId As Integer, ByRef cAnnualRate As Double, ByRef lRateTypeId As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="tablename", vValue:="policy_section_type", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=sCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        'Developer Guide No. 39
        m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=DateTime.Today, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="id", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACSelectIdFromCodeSQL, sSQLName:=ACSelectIdFromCodeName, bStoredProcedure:=ACSelectIdFromCodeStored)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        If Convert.IsDBNull(m_oDatabase.Parameters.Item("id").Value) Or IsNothing(m_oDatabase.Parameters.Item("id").Value) Then
            lId = -1
        Else
            lId = m_oDatabase.Parameters.Item("id").Value
        End If

        cAnnualRate = 0
        lRateTypeId = -1

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetRatingIdFromCode (Private)
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function GetRatingIdFromCode(ByRef sCode As String, ByRef lId As Integer, ByRef cAnnualRate As Double, ByRef lRateTypeId As Integer, ByRef iCurrencyID As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vArray(,) As Object



        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="tablename", vValue:="rating_section_type", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=sCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        'Developer Guide No. 39
        m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=DateTime.Today, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="id", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACSelectIdFromCodeSQL, sSQLName:=ACSelectIdFromCodeName, bStoredProcedure:=ACSelectIdFromCodeStored)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        If Convert.IsDBNull(m_oDatabase.Parameters.Item("id").Value) Or IsNothing(m_oDatabase.Parameters.Item("id").Value) Then
            lId = -1
            cAnnualRate = 0
            lRateTypeId = -1
            Return result
        End If

        lId = m_oDatabase.Parameters.Item("id").Value

        'Now let's get the rate and type

        sSQL = "SELECT rate, rate_type_id, currency_id" & Strings.Chr(13) & Strings.Chr(10) & _
               "FROM rating_section_type" & Strings.Chr(13) & Strings.Chr(10) & _
               "WHERE rating_section_type_id = " & CStr(lId) & Strings.Chr(13) & Strings.Chr(10)

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetRateAndType", bStoredProcedure:=False, vResultArray:=vArray, lNumberRecords:=gPMConstants.PMAllRecords)


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Information.IsArray(vArray) Then
            cAnnualRate = gPMFunctions.ToSafeDouble(vArray(0, 0), 0)
            lRateTypeId = gPMFunctions.ToSafeLong(vArray(1, 0), 1)
            iCurrencyID = gPMFunctions.ToSafeInteger(vArray(2, 0), 0)
            vArray = Nothing
        Else
            cAnnualRate = 0
            lRateTypeId = 1
            iCurrencyID = 0
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetShortPeriodRate
    '
    ' Description: Check for and return an available short period rate
    '
    ' History:
    '   25/11/2002 PWF - Created ISS1377
    ' ***************************************************************** '
    Private Function GetShortPeriodRate(ByVal v_lProductID As Integer, ByVal v_dtOldStartDate As Date, ByVal v_dtOldEndDate As Date, ByVal v_dtStartDate As Date, ByVal v_dtEndDate As Date, ByRef r_dProRataRate As Double) As Integer
        Dim Catch_Renamed As Boolean = False

        Dim result As Integer = 0
        Dim oBusiness As bSIRShortPeriodRate.Business


        Try
            Catch_Renamed = True

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Note: We may need to take into account midnight renewals, although
            ' this would be better done during the GetRefundRate call.

            ' Create a reference to the business object

            oBusiness = New bSIRShortPeriodRate.Business
            m_lReturn = oBusiness.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", GetShortPeriodRate, Unable to create business object 'bSIRShortPeriodRate.Business'")
            End If

            ' Get the rate based on new policy details.

            m_lReturn = oBusiness.GetShortPeriodRate(v_lProductID:=v_lProductID, v_sType:=IIf(m_sTransactionType = "MTC", "C", "P"), v_dtStartDate:=v_dtStartDate, v_dtEndDate:=v_dtEndDate, v_dtTransactDate:=v_dtStartDate, r_dRefundRate:=r_dProRataRate)
            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMNotFound
                    result = m_lReturn
                Case Else
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", GetShortPeriodRate, Unable to retrieve short period rates")
            End Select

            ' Terminate the business object.

            'oBusiness.Terminate()
            oBusiness.Dispose()
            oBusiness = Nothing

            Return result

        Catch excep As System.Exception
            If Not Catch_Renamed Then
                Throw excep
            End If

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------



            If Catch_Renamed Then

                Select Case Information.Err().Number
                    Case Constants.vbObjectError
                        ' Log internal failure.
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=excep.Message, vApp:=ACApp, vClass:=ACClass, vMethod:=excep.Source, excep:=excep)

                        Return gPMConstants.PMEReturnCode.PMFalse

                    Case Else
                        ' Log internal error.
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to calculate short period rate", vApp:=ACApp, vClass:=ACClass, vMethod:="GetShortPeriodRate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)


                        Return gPMConstants.PMEReturnCode.PMFalse
                End Select



                Return result
            End If
        End Try
    End Function

    ''' <summary>
    ''' pro rata rate = number of days (this policy) divides number of days in this year
    ''' </summary>
    ''' <param name="v_lProductID"></param>
    ''' <param name="v_dtOldStartDate"></param>
    ''' <param name="v_dtOldEndDate"></param>
    ''' <param name="v_dtStartDate"></param>
    ''' <param name="v_dtEndDate"></param>
    ''' <param name="r_dProRataRate"></param>
    ''' <param name="v_dtInceptionDate"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Public Function GetProRataRate(ByVal v_lProductID As Integer, ByVal v_dtOldStartDate As Date, ByVal v_dtOldEndDate As Date, ByVal v_dtStartDate As Date, ByVal v_dtEndDate As Date, ByRef r_dProRataRate As Double) As Integer
        Return GetProRataRate(v_lProductID:=v_lProductID, v_dtOldStartDate:=v_dtOldStartDate, v_dtOldEndDate:=v_dtOldEndDate, v_dtStartDate:=v_dtStartDate, v_dtEndDate:=v_dtStartDate, r_dProRataRate:=r_dProRataRate, v_dtInceptionDate:=#12/30/1899#)
    End Function

    Public Function GetProRataRate(ByVal v_lProductID As Integer, ByVal v_dtOldStartDate As Date, ByVal v_dtOldEndDate As Date, ByVal v_dtStartDate As Date, ByVal v_dtEndDate As Date, ByRef r_dProRataRate As Double, ByRef v_dtInceptionDate As Date) As Integer

        Dim nResult As Integer = 0
        Dim oResultArray(,) As Object = Nothing
        Dim dProRataRate, dProRataRateFractionVal, dProrata As Double
        Dim dtStartDate As Date
        Dim bIsTrueMonthlyPolicy As Boolean = False
        Dim dtInceptionDate As Date
        Dim dtEndDate As Date
        Dim dtTmpDate As Date
        Dim dtTmpDate1 As Date
        Dim dtLastDateofMonth As Date = Nothing
        Dim dtDate As Date = Nothing
        Dim nMonthCount As Integer = 0
        Dim nTotalDaysInMonth As Integer = 0
        Dim lBaseLength As Integer
        Dim lPeriodLength As Integer


        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            'Check whether its a TMP
            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add("Prod_id", CStr(v_lProductID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetTMPStatusSQL, sSQLName:=ACGetTMPStatusName, bStoredProcedure:=False, vResultArray:=oResultArray, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Information.IsArray(oResultArray) Then

                bIsTrueMonthlyPolicy = IIf(CDbl(oResultArray(0, 0)) = 1, 1, 0)

                If bIsTrueMonthlyPolicy Then
                    dtDate = v_dtStartDate
                    dProrata = 0

                    nMonthCount = DateAndTime.DateDiff("m", v_dtStartDate, v_dtEndDate, FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1)

                    If (m_lIsMidnightRenewal = 0) Then
                        dtEndDate = DateAdd("d", -1, v_dtEndDate)
                    Else
                        dtEndDate = v_dtEndDate
                    End If

                    Select Case Month(dtDate)
                        Case 1, 3, 5, 7, 8, 10, 12
                            nTotalDaysInMonth = 31
                        Case Else
                            nTotalDaysInMonth = DateDiff("d", dtDate, DateAdd("m", 1, dtDate))
                    End Select

                    If DateDiff("d", v_dtStartDate, dtEndDate) + 1 = nTotalDaysInMonth Then
                        r_dProRataRate = 1
                        Return nResult
                        Exit Function
                    End If

                    Do While dtDate <= v_dtEndDate
                        Select Case dtDate.Month
                            Case 1, 3, 5, 7, 8, 10, 12
                                nTotalDaysInMonth = 31
                            Case Else
                                nTotalDaysInMonth = DateAndTime.DateDiff("d", dtDate, dtDate.AddMonths(1), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1)
                        End Select

                        Dim dtTempDate As Date = Nothing
                        dtLastDateofMonth = New DateTime(dtDate.Year, dtDate.Month, 1).AddMonths(1).AddDays(-1)

                        Dim nPolicyDays As Integer = 0

                        If dtDate.Month = v_dtEndDate.Month Then
                            ''PN 61501
                            nPolicyDays = DateAndTime.DateDiff("d", dtDate, v_dtEndDate, FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) + 1
                        Else
                            nPolicyDays = DateAndTime.DateDiff("d", dtDate, dtLastDateofMonth, FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) + 1
                        End If

                        dProrata += (nPolicyDays / nTotalDaysInMonth)
                        If nMonthCount > 0 Then
                            If dtDate.Month < 12 Then
                                Dim dtTempDate2 As Date = Nothing
                                dtDate = CDate(IIf(DateTime.TryParse("01/" & dtDate.Month + 1 & "/" & CStr(dtDate.Year), dtTempDate2), dtTempDate2.ToString("dd/MM/yyyy"), "01/" & dtDate.Month + 1 & "/" & CStr(dtDate.Year)))
                            Else
                                Dim dtTempDate3 As Date = Nothing
                                dtDate = CDate(IIf(DateTime.TryParse("01/01/" & dtDate.Year + 1, dtTempDate3), dtTempDate3.ToString("dd/MM/yyyy"), "01/01/" & dtDate.Year + 1))
                            End If
                        Else : Exit Do
                        End If
                    Loop

                    r_dProRataRate = dProrata
                    Return nResult
                Else
                    dtStartDate = CDate(v_dtStartDate).Date
                    dtInceptionDate = v_dtInceptionDate
                    dtEndDate = v_dtEndDate

                    dtTmpDate = v_dtInceptionDate


                    'If v_dtOldEndDate > v_dtEndDate And IsTemporaryMTA = False Then
                    '    dtStartDate = v_dtOldStartDate
                    'End If

                    If m_lIsMidnightRenewal = 0 Then
                        dtEndDate = v_dtEndDate.AddDays(-1)
                    End If

                    lBaseLength = 365

                    If (Month(dtInceptionDate) = 2 And Day(dtInceptionDate) = 29) Or (Month(dtEndDate) = 2 And Day(dtEndDate) = 29) Then
                        lBaseLength = 366
                    Else
                        lBaseLength = DateDiff(DateInterval.Day, dtInceptionDate, dtInceptionDate.AddYears(1))
                        'Do While CDate(dtTmpDate) < CDate(dtEndDate)
                        '    If (Day(dtTmpDate) = 1 And Month(dtTmpDate) <> 2) Or (Year(dtTmpDate) Mod 4 <> 0) Then
                        '        dtTmpDate = DateAdd("m", 1, dtTmpDate)
                        '    Else
                        '        dtTmpDate = DateAdd("d", 1, dtTmpDate)
                        '    End If

                        '    If Month(dtTmpDate) = 2 And Day(dtTmpDate) = 29 Then
                        '        lBaseLength = 366
                        '        Exit Do
                        '    End If
                        'Loop
                    End If

                    lPeriodLength = DateDiff("d", dtStartDate, dtEndDate) + 1
                    dProRataRate = lPeriodLength / lBaseLength

                    r_dProRataRate = dProRataRate
                End If
            End If
            Return nResult
        Catch ex As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetProRataRate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProRataRate", vErrNo:=Information.Err().Number, vErrDesc:=ex, excep:=ex)
            Return nResult
        End Try
    End Function

    ' ***************************************************************** '
    ' Name : GetProRataFlag
    '
    ' Desc : get flag to see if we need to prorata at new business or MTA
    '
    ' Hist : 14 may 2001 Created - Tinny
    '
    ' ***************************************************************** '
    Private Function GetProRataFlag(ByVal v_lInsuranceFileCnt As Integer, ByRef r_iNBProrata As Integer, ByRef r_iMTAProrata As Integer, Optional ByRef r_iShortPeriodRate As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object



        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetProrataFlagSQL, sSQLName:=ACGetProrataFlagName, bStoredProcedure:=ACGetProrataFlagStored, vResultArray:=vResultArray, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords)


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If

        If Not Information.IsArray(vResultArray) Then
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If

        'store new business prorata flag
        Dim auxVar As Object = vResultArray(0, 0)


        If Convert.IsDBNull(auxVar) Or IsNothing(auxVar) Then
            r_iNBProrata = 0
        Else

            r_iNBProrata = CInt(vResultArray(0, 0))
        End If

        'store MTA prorata flag
        Dim auxVar_2 As Object = vResultArray(1, 0)


        If Convert.IsDBNull(auxVar_2) Or IsNothing(auxVar_2) Then
            r_iMTAProrata = 0
        Else

            r_iMTAProrata = CInt(vResultArray(1, 0))
        End If

        'store short period rated flag
        Dim auxVar_3 As Object = vResultArray(2, 0)


        If Convert.IsDBNull(auxVar_3) Or IsNothing(auxVar_3) Then
            r_iShortPeriodRate = 0
        Else

            r_iShortPeriodRate = CInt(vResultArray(2, 0))
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name : GetRoundingInfo
    '
    ' Desc : find out if Product requires This premium to be rounded
    '
    ' Hist : 26/07/2001 Created - JMK
    '
    ' ***************************************************************** '
    Private Function GetRoundingInfo(ByVal v_lProductID As Integer, ByRef r_iRoundPremium As Integer, ByRef r_lRoundingSectionID As Integer, ByRef r_sRoundingSectionCode As String) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object



        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="product_id", vValue:=CStr(v_lProductID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRoundingInfoSQL, sSQLName:=ACGetRoundingInfoName, bStoredProcedure:=ACGetRoundingInfoStored, vResultArray:=vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Information.IsArray(vResultArray) Then
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If

        'store rounding flag
        Dim auxVar As Object = vResultArray(0, 0)


        If Convert.IsDBNull(auxVar) Or IsNothing(auxVar) Then
            r_iRoundPremium = 0
        Else

            r_iRoundPremium = CInt(vResultArray(0, 0))
        End If

        'store rounding rating section ID
        Dim auxVar_2 As Object = vResultArray(1, 0)


        If Convert.IsDBNull(auxVar_2) Or IsNothing(auxVar_2) Then
            r_lRoundingSectionID = 0
        Else

            r_lRoundingSectionID = CInt(vResultArray(1, 0))
        End If

        'store rounding rating section code??
        Dim auxVar_3 As Object = vResultArray(2, 0)


        If Convert.IsDBNull(auxVar_3) Or IsNothing(auxVar_3) Then
            r_sRoundingSectionCode = ""
        Else

            r_sRoundingSectionCode = CStr(vResultArray(2, 0))
        End If

        Return result

    End Function
    ' ***************************************************************** '
    '
    ' Name: GetTransactionType
    '
    ' Description:
    '
    ' History: 03/07/2001 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function GetTransactionType() As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=m_sTransactionType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetTransactionTypeSQL, sSQLName:=ACGetTransactionTypeName, bStoredProcedure:=ACGetTransactionTypeStored, vResultArray:=vArray, lNumberRecords:=gPMConstants.PMAllRecords)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Information.IsArray(vArray) Then

                m_lTransactionType = CInt(vArray(0, 0))
                vArray = Nothing
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTransactionType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTransactionType", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: MergeArrays
    '
    ' Description:
    '
    ' History: 07/08/2001 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function MergeArrays(ByRef vMTAArray(,) As Object, ByRef vOriginalArray(,) As Object, ByRef vMergedArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lTemp2, lHowMany As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        If Information.IsArray(vMTAArray) Then
            If Information.IsArray(vOriginalArray) Then
                'We need to merge
            Else
                'Nowt to do
                vMergedArray = VB6.CopyArray(vMTAArray)
                Return result
            End If
        Else
            If Information.IsArray(vOriginalArray) Then
                'Use this one
                vMergedArray = VB6.CopyArray(vOriginalArray)
                Return result
            Else
                'Nowt to do
                Return result
            End If
        End If

        'One thing, if we have _any_ decline or refer reasons we'll be requoting anyway, so
        'let's get that out of the way

        For lTemp As Integer = vMTAArray.GetLowerBound(1) To vMTAArray.GetUpperBound(1)

            If CStr(vMTAArray(ACODeclineReason, lTemp)) <> "" Then
                vMergedArray = VB6.CopyArray(vMTAArray)
                Return result
            End If


            If CStr(vMTAArray(ACOReferReason, lTemp)) <> "" Then
                vMergedArray = VB6.CopyArray(vMTAArray)
                Return result
            End If

        Next lTemp

        For lTemp As Integer = LBound(vOriginalArray, 2) To UBound(vOriginalArray, 2)
            For lTemp2 = LBound(vMTAArray, 2) To UBound(vMTAArray, 2)
                If Trim(vOriginalArray(ACORiskRatingSectionType, lTemp)) = Trim(vMTAArray(ACORiskRatingSectionType, lTemp2)) Then
                    vOriginalArray(ACODisableOriginalProRata, lTemp) = vMTAArray(ACODisableOriginalProRata, lTemp2)
                    vOriginalArray(ACODisableNewProRata, lTemp) = vMTAArray(ACODisableNewProRata, lTemp2)
                End If
            Next
        Next

        'Put the original first, tack on the MTA

        ReDim vMergedArray(vMTAArray.GetUpperBound(0), vMTAArray.GetUpperBound(1) + vOriginalArray.GetUpperBound(1) + 1)

        For lTemp As Integer = vOriginalArray.GetLowerBound(1) To vOriginalArray.GetUpperBound(1)


            vMergedArray(ACOPolicyBinderId, lTemp) = vOriginalArray(ACOPolicyBinderId, lTemp)



            vMergedArray(ACOOutputId, lTemp) = vOriginalArray(ACOOutputId, lTemp)



            vMergedArray(ACODeclineReason, lTemp) = vOriginalArray(ACODeclineReason, lTemp)



            vMergedArray(ACOReferReason, lTemp) = vOriginalArray(ACOReferReason, lTemp)



            vMergedArray(ACOMessage, lTemp) = vOriginalArray(ACOMessage, lTemp)



            vMergedArray(ACOPolicyRatingSectionType, lTemp) = vOriginalArray(ACOPolicyRatingSectionType, lTemp)



            vMergedArray(ACORiskRatingSectionType, lTemp) = vOriginalArray(ACORiskRatingSectionType, lTemp)



            vMergedArray(ACOSumInsured, lTemp) = vOriginalArray(ACOSumInsured, lTemp)



            vMergedArray(ACOPremium, lTemp) = vOriginalArray(ACOPremium, lTemp)



            vMergedArray(ACORate, lTemp) = vOriginalArray(ACORate, lTemp)



            vMergedArray(ACOOriginalPremium, lTemp) = vOriginalArray(ACOOriginalPremium, lTemp)



            vMergedArray(ACOOriginalFlag, lTemp) = vOriginalArray(ACOOriginalFlag, lTemp)



            vMergedArray(ACORateTypeId, lTemp) = vOriginalArray(ACORateTypeId, lTemp)



            vMergedArray(ACOCountryId, lTemp) = vOriginalArray(ACOCountryId, lTemp)



            vMergedArray(ACOStateId, lTemp) = vOriginalArray(ACOStateId, lTemp)



            vMergedArray(ACOAutoCalculated, lTemp) = vOriginalArray(ACOAutoCalculated, lTemp)



            vMergedArray(ACOEarningPatternID, lTemp) = vOriginalArray(ACOEarningPatternID, lTemp)


            vMergedArray(ACODisableOriginalProRata, lTemp) = vOriginalArray(ACODisableOriginalProRata, lTemp)


            vMergedArray(ACODisableNewProRata, lTemp) = vOriginalArray(ACODisableNewProRata, lTemp)

        Next lTemp

        lHowMany = vOriginalArray.GetUpperBound(1) + 1

        For lTemp As Integer = vMTAArray.GetLowerBound(1) To vMTAArray.GetUpperBound(1)


            vMergedArray(ACOPolicyBinderId, lHowMany + lTemp) = vMTAArray(ACOPolicyBinderId, lTemp)



            vMergedArray(ACOOutputId, lHowMany + lTemp) = vMTAArray(ACOOutputId, lTemp)



            vMergedArray(ACODeclineReason, lHowMany + lTemp) = vMTAArray(ACODeclineReason, lTemp)



            vMergedArray(ACOReferReason, lHowMany + lTemp) = vMTAArray(ACOReferReason, lTemp)



            vMergedArray(ACOMessage, lHowMany + lTemp) = vMTAArray(ACOMessage, lTemp)



            vMergedArray(ACOPolicyRatingSectionType, lHowMany + lTemp) = vMTAArray(ACOPolicyRatingSectionType, lTemp)



            vMergedArray(ACORiskRatingSectionType, lHowMany + lTemp) = vMTAArray(ACORiskRatingSectionType, lTemp)



            vMergedArray(ACOSumInsured, lHowMany + lTemp) = vMTAArray(ACOSumInsured, lTemp)



            vMergedArray(ACOPremium, lHowMany + lTemp) = vMTAArray(ACOPremium, lTemp)



            vMergedArray(ACORate, lHowMany + lTemp) = vMTAArray(ACORate, lTemp)



            vMergedArray(ACOOriginalPremium, lHowMany + lTemp) = vMTAArray(ACOOriginalPremium, lTemp)



            vMergedArray(ACOOriginalFlag, lHowMany + lTemp) = vMTAArray(ACOOriginalFlag, lTemp)



            vMergedArray(ACORateTypeId, lHowMany + lTemp) = vMTAArray(ACORateTypeId, lTemp)



            vMergedArray(ACOCountryId, lHowMany + lTemp) = vMTAArray(ACOCountryId, lTemp)



            vMergedArray(ACOStateId, lHowMany + lTemp) = vMTAArray(ACOStateId, lTemp)



            vMergedArray(ACOAutoCalculated, lHowMany + lTemp) = vMTAArray(ACOAutoCalculated, lTemp)



            vMergedArray(ACOEarningPatternID, lHowMany + lTemp) = vMTAArray(ACOEarningPatternID, lTemp)



            vMergedArray(ACODisableOriginalProRata, lHowMany + lTemp) = vMTAArray(ACODisableOriginalProRata, lTemp)



            vMergedArray(ACODisableNewProRata, lHowMany + lTemp) = vMTAArray(ACODisableNewProRata, lTemp)

        Next lTemp


        Return result

    End Function


    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    Private Function BeginTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLBeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    Private Function CommitTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLCommitTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    Private Function RollbackTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLRollbackTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
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
        ' Error.
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub



    ' **************************************************************************
    '   Get all options from the product in one place!
    ' **************************************************************************
    Public Function GetUWProductOptions(ByVal v_lProductID As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add parameter
            bPMAddParameter.AddParameterLite(m_oDatabase, "product_id", CStr(v_lProductID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            ' Execute call
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectUWProductOptionsSQL, sSQLName:=ACSelectUWProductOptionsName, bStoredProcedure:=ACSelectUWProductOptionsStored, vResultArray:=vResultArray, lNumberRecords:=gPMConstants.PMAllRecords, bKeepNulls:=True)

            ' Check results
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Information.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' Store options
            m_lIsMidnightRenewal = gPMFunctions.NullToLong(vResultArray(0, 0))
            m_lAllowPositiveCancellation = gPMFunctions.NullToLong(vResultArray(1, 0))
            m_lUnifiedRenewalDay = gPMFunctions.NullToLong(vResultArray(2, 0))

            Return result

        Catch excep As System.Exception


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUWProductOptions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUWProductOptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMFalse

        End Try
    End Function

    'Calculates the amounts needed to round the premium to a whole number
    Public Function GetRoundingSectionAmounts(ByRef r_cAnnualPremium As Decimal, ByRef r_cThisPremium As Decimal) As Integer
        Return GetRoundingSectionAmounts(r_cAnnualPremium:=r_cAnnualPremium, r_cThisPremium:=r_cThisPremium, v_lOriginal_Flag:=0)
    End Function

    Public Function GetRoundingSectionAmounts(ByRef r_cAnnualPremium As Decimal, ByRef r_cThisPremium As Decimal, ByVal v_lOriginal_Flag As Integer) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Default the amounts
            r_cAnnualPremium = 0
            r_cThisPremium = 0

            'Add the parameters
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add("risk_cnt", CStr(m_lRiskID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add("original_flag", CStr(v_lOriginal_Flag), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            'Developer Guide No. 85 (guide)
            m_lReturn = m_oDatabase.Parameters.Add("annual_premium", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency)

            'Developer Guide No. 85 (guide)
            m_lReturn = m_oDatabase.Parameters.Add("this_premium", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency)

            'Execute the stored procedure
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetRoundingSectionAmountsSQL, sSQLName:=ACGetRoundingSectionAmountsName, bStoredProcedure:=ACGetRoundingSectionAmountsStored)

            ' Commit or Rollback trans
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If Convert.IsDBNull(m_oDatabase.Parameters.Item("annual_premium").Value) Or IsNothing(m_oDatabase.Parameters.Item("annual_premium").Value) Then
                r_cAnnualPremium = 0
            Else
                r_cAnnualPremium = m_oDatabase.Parameters.Item("annual_premium").Value
            End If


            If Convert.IsDBNull(m_oDatabase.Parameters.Item("this_premium").Value) Or IsNothing(m_oDatabase.Parameters.Item("this_premium").Value) Then
                r_cThisPremium = 0
            Else
                r_cThisPremium = m_oDatabase.Parameters.Item("this_premium").Value
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRoundingSectionAmounts Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRoundingSectionAmounts", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         RecalculatePremium
    '
    ' Description:  Accepts an array containing the Rating section data and
    '               recalculates the rating section premiums, taxes and running totals etc.
    '
    ' History    :  Richard Taylor - Created on 12/02/2004
    ' RAW 05/05/2004 : CQ753 : make params optional
    ' ***************************************************************** '
    Public Function RecalculatePremium(ByRef r_vRatingSection As Object, ByRef r_cTotalAnnualTax As Decimal) As Integer
        Return RecalculatePremium(r_vRatingSection:=r_vRatingSection, v_vRateTypes:=Nothing, r_cReturnPremium:=0, r_cPremiumDueNet:=0, r_cPremiumDueTax:=0, r_cPremiumDueGross:=0, r_cNewPremiumNet:=0, r_cNewPremiumTax:=0, r_cNewPremiumGross:=0, r_cNewAnnPremNet:=0, r_cNewAnnPremTax:=0, r_cNewAnnPremGross:=0, r_cOldPremiumNet:=0, r_cOldPremiumTax:=0, r_cOldPremiumGross:=0, r_cOldAnnPremNet:=0, r_cOldAnnPremTax:=0, r_cOldAnnPremGross:=0, r_cTotalAnnualTax:=r_cTotalAnnualTax, v_vOriginal:=Nothing, r_bCancelledPolicy:=False)
    End Function

    Public Function RecalculatePremium(ByRef r_vRatingSection As Object, ByVal v_vRateTypes As Object, ByRef r_cReturnPremium As Decimal, ByRef r_cPremiumDueNet As Decimal, ByRef r_cPremiumDueTax As Decimal, ByRef r_cPremiumDueGross As Decimal, ByRef r_cNewPremiumNet As Decimal, ByRef r_cNewPremiumTax As Decimal, ByRef r_cNewPremiumGross As Decimal, ByRef r_cNewAnnPremNet As Decimal, ByRef r_cNewAnnPremTax As Decimal, ByRef r_cNewAnnPremGross As Decimal, ByRef r_cOldPremiumNet As Decimal, ByRef r_cOldPremiumTax As Decimal, ByRef r_cOldPremiumGross As Decimal, ByRef r_cOldAnnPremNet As Decimal, ByRef r_cOldAnnPremTax As Decimal, ByRef r_cOldAnnPremGross As Decimal, ByRef r_cTotalAnnualTax As Decimal, ByVal v_vOriginal(,) As Object, ByRef r_bCancelledPolicy As Boolean) As Integer

        Dim result As Integer = 0
        Dim lUBound, lLbound, lUBoundColumn As Integer

        Dim cPremium, cRate, cRunningTotal As Decimal
        Dim lRateTypeId As Integer
        Dim sRateTypeCode, sTemp As String

        Dim cThisPremium, cRunningTotalThisPremium As Decimal

        'JMK 27/07/2001
        Dim cRoundedThisPremium, cRoundAmount As Decimal

        Dim iRoundPremium, lRoundingSectionId, lRatingSectionTypeId As Integer

        Dim sText As String = ""
        Dim sSubItems() As String
        Dim sTag As String = ""

        Dim dTaxRate As Double
        Dim lCurrentRatingSectionTypeID As Integer

        Dim cTax, cThisTax, cReturnTax, cRunningTotalTax, cRunningTotalThisTax, cTotalTaxOld As Decimal

        Dim cTotalPremiumOld, cOldTax As Decimal

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            iRoundPremium = RoundPremium
            lRoundingSectionId = RoundingSectionID

            cRunningTotal = 0

            cRunningTotalThisPremium = 0

            ' HG181203 CQ598 - Running Total for Annual Premium Tax
            r_cTotalAnnualTax = 0


            ' RAW 05/05/2004 : CQ753 : added

            If Information.IsNothing(v_vRateTypes) Then

                m_lReturn = CType(GetRateTypes(vResultArray:=v_vRateTypes), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRateTypes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RecalculatePremium")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            ' RAW 05/05/2004 : CQ753 : end


            'There's a problem that came to light when manual rating takes place, rounding goes
            'belly-up when rating sections are added.
            'The safest thing to do is to reshuffle the list and put the rounding section at
            'the bottom...

            If iRoundPremium = 1 Then

                'Ignore the end one...
                lLbound = r_vRatingSection.GetLowerBound(1)
                lUBound = r_vRatingSection.GetUpperBound(1) - 1
                lUBoundColumn = r_vRatingSection.GetUpperBound(0)

                ' Redim the temp array to match the RatingSection Array
                ReDim sSubItems(lUBoundColumn)

                For iLine As Integer = lLbound To lUBound


                    lRatingSectionTypeId = CInt(r_vRatingSection(ACRatingSectionTypeIdCol, iLine))

                    If lRatingSectionTypeId = lRoundingSectionId Then
                        'Move it to the end of the array.

                        'So let's swap them
                        'iLine goes to temporary storage
                        For lTemp As Integer = 0 To lUBoundColumn

                            sSubItems(lTemp) = CStr(r_vRatingSection(lTemp, iLine))
                        Next lTemp

                        ' The very last entry in the array goes to iLine
                        For lTemp As Integer = 0 To lUBoundColumn


                            r_vRatingSection(lTemp, iLine) = r_vRatingSection(lTemp, lUBound + 1)
                        Next lTemp

                        'temporary storage goes to very last entry
                        For lTemp As Integer = 0 To lUBoundColumn

                            r_vRatingSection(lTemp, lUBound + 1) = sSubItems(lTemp)
                        Next lTemp

                        ' And we're finished.
                        Exit For

                    End If

                Next iLine
            End If

            If Information.IsArray(v_vOriginal) Then

                r_cReturnPremium = 0

                lUBound = v_vOriginal.GetUpperBound(1)
                lLbound = v_vOriginal.GetLowerBound(1)

                For lTemp As Integer = lLbound To lUBound

                    ' PW191102 - get the rating section type for the current item
                    ' PS411

                    lRatingSectionTypeId = CInt(v_vOriginal(ACRatingSectionTypeIdCol, lTemp))
                    ' PW191102 - get the tax rate for the current
                    ' rating section type, if not already got
                    ' PS411
                    If lCurrentRatingSectionTypeID <> lRatingSectionTypeId Then

                        m_lReturn = CType(GetRatingSectionTypeTax(v_lRatingSectionTypeId:=lRatingSectionTypeId, r_dTaxRate:=dTaxRate), gPMConstants.PMEReturnCode)
                        ' Check for errors.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' Log Error Message
                            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the tax rate for the rating section " & lRatingSectionTypeId, vApp:=ACApp, vClass:=ACClass, vMethod:="RecalculatePremium")
                            Return result
                        End If

                        lCurrentRatingSectionTypeID = lRatingSectionTypeId
                    End If


                    cThisPremium = CDec(v_vOriginal(ACThisPremiumCol, lTemp))
                    r_cReturnPremium += cThisPremium

                    ' PW191102 - Calculate the tax amount and keep a total
                    ' PS411
                    cThisTax = cThisPremium * dTaxRate
                    cReturnTax += cThisTax

                    ' PW191102 - Get the annual premium and keep a total
                    ' - and the tax


                    cPremium = CDec(v_vOriginal(ACThisPremiumCol, lTemp))

                    ' HG131103 - Populate Old Annual Premium Using Top List View. Note: The
                    ' List View Constants Seem Odd (It Needs To Point to Original Annual Premium)
                    ' And Work Out Tax..

                    Dim dbNumericTemp As Double
                    If Double.TryParse(CStr(v_vOriginal(ACPremiumCol, lTemp)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                        cTotalPremiumOld += CDec(v_vOriginal(ACPremiumCol, lTemp))

                        cOldTax = CDec(v_vOriginal(ACPremiumCol, lTemp)) * dTaxRate
                        cTotalTaxOld += cOldTax
                    End If
                    cTax = cPremium * dTaxRate


                Next lTemp

            End If

            If Information.IsArray(r_vRatingSection) Then
                lUBound = r_vRatingSection.GetUpperBound(1)
                lLbound = r_vRatingSection.GetLowerBound(1)

                For lTemp As Integer = lLbound To lUBound

                    ' PW290904 - exclude the original rating sections from the
                    ' calculations

                    If CStr(r_vRatingSection(ACOriginalFlagCol, lTemp)) = "0" Or r_bCancelledPolicy Then


                        lRatingSectionTypeId = CInt(r_vRatingSection(ACRatingSectionTypeIdCol, lTemp))

                        ' Get the tax rate for the current
                        ' rating section type, if not already got
                        If lCurrentRatingSectionTypeID <> lRatingSectionTypeId Then

                            m_lReturn = CType(GetRatingSectionTypeTax(v_lRatingSectionTypeId:=lRatingSectionTypeId, r_dTaxRate:=dTaxRate), gPMConstants.PMEReturnCode)
                            ' Check for errors.
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                ' Log Error Message
                                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the tax rate for the rating section " & lRatingSectionTypeId, vApp:=ACApp, vClass:=ACClass, vMethod:="RecalculatePremium")
                                Return result
                            End If

                            lCurrentRatingSectionTypeID = lRatingSectionTypeId
                        End If

                        'Don't include the rounding item if we're rounding...
                        If (iRoundPremium = 0) Or (lRatingSectionTypeId <> lRoundingSectionId) Then


                            cPremium = CDec(r_vRatingSection(ACPremiumCol, lTemp))


                            cThisPremium = CDec(r_vRatingSection(ACThisPremiumCol, lTemp))


                            sTemp = CStr(r_vRatingSection(ACRateCol, lTemp))
                            If sTemp.EndsWith("%") Then
                                sTemp = sTemp.Substring(0, sTemp.Length - 1)
                            End If

                            Dim dbNumericTemp2 As Double
                            If Not Double.TryParse(sTemp, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                                cRate = 0
                            Else
                                cRate = CDec(sTemp)
                            End If


                            lRateTypeId = CInt(r_vRatingSection(ACRateTypeIdCol, lTemp))


                            m_lReturn = CType(GetRateType(v_vRateTypes:=v_vRateTypes, v_lRateTypeId:=lRateTypeId, r_sRateTypeCode:=sRateTypeCode), gPMConstants.PMEReturnCode)

                            If sRateTypeCode <> "T" Then
                                cRunningTotal += cPremium
                                cRunningTotalThisPremium += cThisPremium
                                ' PW191102 - calculate taxes
                                cTax = cPremium * dTaxRate
                                cThisTax = cThisPremium * dTaxRate
                                cRunningTotalTax += cTax
                                cRunningTotalThisTax += cThisTax

                            Else
                                cPremium = cRunningTotal * cRate / 100.0#

                                cThisPremium = cRunningTotalThisPremium * cRate / 100.0#

                                ' May need formatting

                                r_vRatingSection(ACPremiumCol, lTemp) = cPremium

                                cRunningTotal += cPremium

                                'this premium stuff

                                r_vRatingSection(ACThisPremiumCol, lTemp) = cThisPremium

                                cRunningTotalThisPremium += cThisPremium

                                ' PW191102 - calculate taxes
                                cTax = cPremium * dTaxRate
                                cThisTax = cThisPremium * dTaxRate
                                cRunningTotalTax += cTax
                                cRunningTotalThisTax += cThisTax

                            End If

                            ' HG181203 CQ598 - Calculate Annual Premium Tax
                            r_cTotalAnnualTax += cTax

                        End If
                    End If
                Next lTemp
            End If

            'Round it, don't just round it up...
            If CLngRounding(cValue:=cRunningTotalThisPremium + r_cReturnPremium, cReturn:=cRoundedThisPremium) <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            cRoundAmount = cRoundedThisPremium - cRunningTotalThisPremium - r_cReturnPremium

            If (iRoundPremium = 1) And (cRoundAmount <> 0) Then

                'Can't just do this as the rounding section can be deleted...
                lUBound = r_vRatingSection.GetUpperBound(1)
                lLbound = r_vRatingSection.GetLowerBound(1)
                For lTemp As Integer = lLbound To lUBound


                    lRatingSectionTypeId = CInt(r_vRatingSection(ACRatingSectionTypeIdCol, lTemp))

                    If lRatingSectionTypeId = lRoundingSectionId Then


                        r_vRatingSection(ACThisPremiumCol, lTemp) = cRoundAmount

                        cRunningTotalThisPremium += cRoundAmount

                    End If
                Next lTemp
            End If

            'Add in the return...
            cRunningTotalThisPremium += r_cReturnPremium
            'Add in the return...
            cRunningTotalThisTax += cReturnTax

            ' Net amounts...
            r_cNewPremiumNet = CDec(StringsHelper.Format(cRunningTotalThisPremium, "standard"))

            r_cPremiumDueNet = CDec(StringsHelper.Format(cRunningTotalThisPremium, "standard"))

            r_cOldPremiumNet = CDec(StringsHelper.Format(r_cReturnPremium, "standard"))

            r_cOldAnnPremNet = CDec(StringsHelper.Format(cTotalPremiumOld, "standard"))

            r_cNewAnnPremNet = CDec(StringsHelper.Format(cRunningTotal, "standard"))

            r_cNewPremiumTax = CDec(StringsHelper.Format(cRunningTotalThisTax, "standard"))

            r_cPremiumDueTax = CDec(StringsHelper.Format(cRunningTotalThisTax, "standard"))

            r_cOldPremiumTax = CDec(StringsHelper.Format(cReturnTax, "standard"))

            r_cOldAnnPremTax = CDec(StringsHelper.Format(cTotalTaxOld, "standard"))

            r_cNewAnnPremTax = CDec(StringsHelper.Format(cRunningTotalTax, "standard"))

            r_cNewPremiumGross = CDec(StringsHelper.Format(r_cNewPremiumNet + r_cNewPremiumTax, "standard"))

            r_cPremiumDueGross = CDec(StringsHelper.Format(cRunningTotalThisPremium + cRunningTotalThisTax, "standard"))

            r_cOldPremiumGross = CDec(StringsHelper.Format(r_cReturnPremium + cReturnTax, "standard"))

            r_cOldAnnPremGross = CDec(StringsHelper.Format(cTotalPremiumOld + cTotalTaxOld, "standard"))

            r_cNewAnnPremGross = CDec(StringsHelper.Format(cRunningTotal + cRunningTotalTax, "standard"))

            r_cTotalAnnualTax = CDec(StringsHelper.Format(r_cTotalAnnualTax, "standard"))

            Return result

        Catch excep As System.Exception


            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Calculate Premium", vApp:=ACApp, vClass:="", vMethod:="RecalculatePremium", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetRatingSectionTypeTax
    '
    ' Description: Return the tax rate associated with a Rating Section Type
    '
    ' History: PW151102 - created (PS411)
    '
    ' ***************************************************************** '
    Public Function GetRatingSectionTypeTax(ByVal v_lRatingSectionTypeId As Integer, ByRef r_dTaxRate As Double) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object
        Dim dRate As Double

        Try

            m_oDatabase.Parameters.Clear()

            ' Add the necessary parameters
            m_lReturn = m_oDatabase.Parameters.Add(sName:="rating_section_type_id", vValue:=CStr(v_lRatingSectionTypeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            ' Execute the stored procedure
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRatingSectionTypeTaxSQL, sSQLName:=ACGetRatingSectionTypeTaxName, bStoredProcedure:=ACGetRatingSectionTypeTaxStored, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Check if anything returned
            If Not Information.IsArray(vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMTrue
                r_dTaxRate = 0
                ' bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="No tax records returned for Rating Section Type " & v_lRatingSectionTypeId, vApp:=ACApp, vClass:=ACClass, vMethod:="GetRatingSectionTypeTax", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            '
            ' Calculate the tax rate as there could be more than one
            ' peril type per group with different rates
            '
            ' Loop through the result array

            For i As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)
                ' Add the percentage share to the running total


                dRate += (CDbl(vResultArray(ACRTaxPercentShare, i)) / 100) * CDbl(vResultArray(ACRTaxRate, i))
            Next

            ' Return the result
            ' dividing by 100 changes it from a percentage rate to a more useable
            ' multiply-able rate, e.g. amount * rate = tax amount
            r_dTaxRate = dRate / 100

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            'Error
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRatingSectionTypeTax Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRatingSectionTypeTax", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function GetRateType(ByVal v_vRateTypes(,) As Object, ByVal v_lRateTypeId As Integer, ByRef r_sRateTypeCode As String) As Integer

        Dim result As Integer = 0
        Dim lLbound, lUBound As Integer




        result = gPMConstants.PMEReturnCode.PMTrue

        r_sRateTypeCode = "O"

        If Information.IsArray(v_vRateTypes) Then
            lLbound = v_vRateTypes.GetLowerBound(1)
            lUBound = v_vRateTypes.GetUpperBound(1)
            For nCount As Integer = lLbound To lUBound

                If CDbl(v_vRateTypes(0, nCount)) = v_lRateTypeId Then

                    r_sRateTypeCode = CStr(v_vRateTypes(1, nCount)).Trim()
                    Exit For
                End If
            Next

        End If

        Return result

    End Function

    '*************************************************************************8
    '
    ' Name : CLngRounding
    '
    ' Desc : round to whole number
    '        +ve 0.5 and -ve 0.5 = 0
    '        +ve 1.5 = 2 and -ve 1.5 = -2
    '
    '        this is the same as CLng() - CLng() will fail if number is too big
    '
    ' History : Thinh Nguyen 31/03/2003
    '*************************************************************************8
    Public Function CLngRounding(ByVal cValue As Decimal, ByRef cReturn As Decimal) As Integer

        Dim result As Integer = 0
        Dim cDecimal As Decimal
        Dim lPos As Integer
        Dim cWholeNumber As Decimal

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            cWholeNumber = 0.0#

            If cValue <> 0.0# Then
                lPos = (CStr(cValue).IndexOf("."c) + 1)

                If lPos <> 0 Then
                    cDecimal = CDec(Mid(CStr(cValue), lPos))
                End If

                cWholeNumber = IIf(cValue > 0, Math.Floor(cValue), Math.Ceiling(cValue))

                If cWholeNumber > 0 And cDecimal > 0.5 Then
                    cWholeNumber += 1
                ElseIf cWholeNumber < 0 And cDecimal > 0.5 Then
                    cWholeNumber -= 1
                ElseIf cDecimal = 0.5 And (cWholeNumber Mod 2) = 0 Then
                    If cWholeNumber > 0 Then
                        cWholeNumber += 1
                    ElseIf cWholeNumber < 0 Then
                        cWholeNumber -= 1
                    End If
                ElseIf cDecimal > 0.5 Then
                    If cValue < 0 Then
                        cWholeNumber = -1
                    ElseIf cValue > 0 Then
                        cWholeNumber = 1
                    End If
                End If

            End If

            cReturn = cWholeNumber

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed rounding to whole number", vApp:=ACApp, vClass:=ACClass, vMethod:="CLngRounding", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: IsRiskACopy
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function IsRiskACopy(ByVal v_lRiskCnt As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "IsRiskACopy"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_cnt", CStr(v_lRiskCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kIsRiskACopySQL, sSQLName:=kIsRiskACopyName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kIsRiskACopySQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
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
    '
    ' Name: PopulateRatingSectionsFromExistingSections
    '
    ' Description:
    '
    ' History: 24/09/2000 Tomo - Created.
    ' RAW 05/05/2004 : CQ753 : added r_vResultArray param
    ' RDC 13/09/2005 : added from Sirius 1.9.1 code
    ' ***************************************************************** '
    Public Function PopulateRatingSectionsFromExistingSections(ByVal v_lPreviousDeletedRiskInsuranceFileCnt As Integer, ByRef r_vResultArray As Object) As Integer
        Return PopulateRatingSectionsFromExistingSections(v_lPreviousDeletedRiskInsuranceFileCnt:=v_lPreviousDeletedRiskInsuranceFileCnt, r_vResultArray:=r_vResultArray, v_bIsBackdatedMTA:=False)
    End Function

    Public Function PopulateRatingSectionsFromExistingSections(ByVal v_lPreviousDeletedRiskInsuranceFileCnt As Integer, ByRef r_vResultArray As Object, ByVal v_bIsBackdatedMTA As Boolean) As Integer
        Dim result As Integer = 0
        Dim vArray(,) As Object
        Dim lPolicyBinder As Integer

        Dim lPolicyRatingSectionId, lRiskRatingSectionId As Integer
        Dim cAnnualRate As Double
        Dim lRateTypeId, lRiskStatus As Integer
        Dim cAnnualPremium, cThisPremium As Decimal
        Dim sSQL As String = ""
        'sj 24/02/2003 - start
        'PS104
        Dim sOriginalStatusFlag As String = ""
        Dim lDummyRisk As Integer
        'sj 24/02/2003 - end
        ' RDC 13/05/2005
        Dim iCurrencyID As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get the original risk cnt
            m_lReturn = CType(GetOriginalRiskCnt(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_lRiskCnt:=m_lRiskID, r_lOriginalRiskCnt:=m_lOriginalRiskCnt), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'sj 24/02/2003 - start
            'PS104
            If m_sTransactionType = "MTCR" Or m_sTransactionType = "MTACR" Then
                'See if the original risk was a deleted risk
                'If it was then treat this like a reinstatement
                m_lReturn = CType(GetOriginalRiskCnt(v_lInsuranceFileCnt:=v_lPreviousDeletedRiskInsuranceFileCnt, v_lRiskCnt:=m_lOriginalRiskCnt, r_lOriginalRiskCnt:=lDummyRisk, r_sStatusFlag:=sOriginalStatusFlag), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            'sj 24/02/2003 - end
            m_lReturn = GetProRataForOriginalRisk(nOriginalRiskCnt:=m_lOriginalRiskCnt, _
                                   dProRataRate:=m_dProRataRate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            Select Case m_sTransactionType
                Case "MTR"
                    m_lReturn = CType(GetRatingSectionForReinstatement(r_vArray:=vArray), gPMConstants.PMEReturnCode)
                    'sj 24/02/2003 - start
                    'PS104
                Case "MTACR"

                    If sOriginalStatusFlag = "D" Then ' And v_bIsBackdatedMTA = False
                        m_lReturn = CType(GetRatingSectionForReinstatement(r_vArray:=vArray), gPMConstants.PMEReturnCode)
                    Else
                        m_lReturn = CType(GetOriginalRatingSections(r_vArray:=vArray), gPMConstants.PMEReturnCode)
                    End If
                    'sj 24/02/2003 - end
                Case "MTCR"

                    m_lReturn = CType(GetOriginalRatingSections(r_vArray:=vArray), gPMConstants.PMEReturnCode)

                Case "MTADR"

                    m_lReturn = CType(GetDeletedRatingSections(v_lPreviousDeletedRiskInsuranceFileCnt:=v_lPreviousDeletedRiskInsuranceFileCnt, r_vArray:=vArray), gPMConstants.PMEReturnCode)
            End Select

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Information.IsArray(vArray) Then
                'It's automatically rated, so _now_ we have to clear it down
                'First remove the contents of the rating section and peril tables

                m_lReturn = CType(DeleteSectionAndPerils(), gPMConstants.PMEReturnCode)


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                For lTemp As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)


                    lPolicyRatingSectionId = -1
                    lRiskRatingSectionId = -1


                    If CStr(vArray(ACOPolicyRatingSectionType, lTemp)) <> "" Then

                        m_lReturn = CType(GetPolicyIdFromCode(sCode:=CStr(vArray(ACOPolicyRatingSectionType, lTemp)), lId:=lPolicyRatingSectionId, cAnnualRate:=cAnnualRate, lRateTypeId:=lRateTypeId), gPMConstants.PMEReturnCode)
                    End If


                    If CStr(vArray(ACORiskRatingSectionType, lTemp)) <> "" Then

                        m_lReturn = CType(GetRatingIdFromCode(sCode:=CStr(vArray(ACORiskRatingSectionType, lTemp)), lId:=lRiskRatingSectionId, cAnnualRate:=cAnnualRate, lRateTypeId:=lRateTypeId, iCurrencyID:=iCurrencyID), gPMConstants.PMEReturnCode)
                    End If


                    If (lPolicyRatingSectionId <> -1) Or (lRiskRatingSectionId <> -1) Then

                        If m_sTransactionType = "MTCR" Or m_sTransactionType = "MTR" Then

                            cThisPremium = CDec(vArray(ACOPremium, lTemp)) * -1
                        Else

                            cThisPremium = CDec(vArray(ACOPremium, lTemp))
                        End If
                        Dim nIsOriginal As Integer
                        If m_sTransactionType = "MTCR" Then
                            nIsOriginal = IIf(CInt(vArray(ACOOriginalFlag, lTemp)) = 0, 1, 0)
                        End If

                        cAnnualPremium = CDec(vArray(ACOOriginalPremium, lTemp))

                        m_lReturn = CType(AddSectionAndPerils(v_lRatingSectionTypeId:=lRiskRatingSectionId, v_lPolicySectionTypeId:=lPolicyRatingSectionId, v_cAnnualPremium:=cAnnualPremium, v_cThisPremium:=cThisPremium, v_cAnnualRate:=cAnnualRate, v_cSumInsured:=CDec(vArray(ACOSumInsured, lTemp)), v_lRateTypeId:=lRateTypeId, v_lOriginalFlag:=nIsOriginal, v_iDefinedCurrencyID:=iCurrencyID, v_lCountryID:=CInt(vArray(ACOCountryId, lTemp)), v_lStateID:=CInt(vArray(ACOStateId, lTemp)), v_iAutoCalculated:=1, v_iIsAmended:=0, v_cCalculatedPremium:=0, v_sOverrideReason:="", v_lEarningPatternId:=0), gPMConstants.PMEReturnCode)

                    End If

                Next lTemp
            End If


            If m_iRoundPremium = 1 Then
                'we will be needing to round this premium total
                'so create a rounding rating section ready for later

                sSQL = "SELECT rs.risk_cnt" & Strings.Chr(13) & Strings.Chr(10) & _
                       "FROM rating_section rs" & Strings.Chr(13) & Strings.Chr(10) & _
                       "WHERE rs.risk_cnt = " & CStr(m_lRiskID) & Strings.Chr(13) & Strings.Chr(10) & _
                       "AND rs.rating_section_type_id = " & CStr(m_lRoundingSection) & Strings.Chr(13) & Strings.Chr(10) & _
                       "AND rs.original_flag = 0" & Strings.Chr(13) & Strings.Chr(10)

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="Check rounding already there", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Not Information.IsArray(vArray) Then
                    m_lReturn = CType(AddSectionAndPerils(v_lRatingSectionTypeId:=m_lRoundingSection, v_lPolicySectionTypeId:=-1, v_cAnnualPremium:=0, v_cThisPremium:=0, v_cAnnualRate:=0, v_cSumInsured:=0, v_lRateTypeId:=1, v_lOriginalFlag:=0, v_iDefinedCurrencyID:=iCurrencyID, v_lCountryID:=0, v_lStateID:=0, v_iAutoCalculated:=1, v_iIsAmended:=0, v_cCalculatedPremium:=0, v_sOverrideReason:="", v_lEarningPatternId:=0), gPMConstants.PMEReturnCode)
                End If

            End If


            'Now let's update the Risk Status...
            If lRiskStatus = 0 Then
                lRiskStatus = 8
            End If

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=CStr(m_lRiskID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_status_id", vValue:=CStr(lRiskStatus), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateRiskStatusSQL, sSQLName:=ACUpdateRiskStatusName, bStoredProcedure:=ACUpdateRiskStatusStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' RAW 05/05/2004 : CQ753 : added

            If Not Information.IsNothing(r_vResultArray) Then

                ' get the data for returning back to the caller
                m_lReturn = CType(GetRatingSections(vResultArray:=r_vResultArray), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            ' RAW 05/05/2004 : CQ753 : end

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PopulateRatingSectionsFromExistingSections Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateRatingSectionsFromExistingSections", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: GetOriginalRiskCnt
    '
    ' Description:
    '
    ' History: 29/01/2003 sj - Created.
    '
    ' RDC 13/09/2005 : Added from Sirius 1.9.1
    ' ***************************************************************** '
    'sj 24/02/2003 - start
    'PS104
    Private Function GetOriginalRiskCnt(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lRiskCnt As Integer, ByRef r_lOriginalRiskCnt As Integer, Optional ByRef r_sStatusFlag As String = "") As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim vArray(,) As Object

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=CStr(v_lRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectIFRLSQL, sSQLName:=ACSelectIFRLName, bStoredProcedure:=ACSelectIFRLStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Information.IsArray(vArray) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Renewals puts 0 into this instead of null
        Dim auxVar As Object = vArray(3, 0)


        If Convert.IsDBNull(auxVar) Or IsNothing(auxVar) Then
            r_lOriginalRiskCnt = -1
        Else
            'Hence...

            If (CStr(vArray(3, 0)) = "") Or (CStr(vArray(3, 0)) = "0") Then
                r_lOriginalRiskCnt = -1
            Else

                r_lOriginalRiskCnt = CInt(vArray(3, 0))
            End If
            'sj 24/02/2003 - start
            'PS104

            r_sStatusFlag = CStr(vArray(2, 0))
            'sj 24/02/2003 - end
        End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: GetRatingSectionForReinstatement
    '
    ' Description:
    '
    ' History: 29/01/2003 sj - Created.
    '
    ' RDC 13/09/2005 : Added from Sirius 1.9.1
    ' ***************************************************************** '
    Private Function GetRatingSectionForReinstatement(ByRef r_vArray(,) As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim sSQL As String = ""

        'Now get the rating sections from the original risk
        sSQL = "SELECT rs.risk_cnt," & Strings.Chr(13) & Strings.Chr(10) & _
               "rs.rating_section_id," & Strings.Chr(13) & Strings.Chr(10) & _
               "null Decline_reason," & Strings.Chr(13) & Strings.Chr(10) & _
               "null Refer_reason," & Strings.Chr(13) & Strings.Chr(10) & _
               "null Message," & Strings.Chr(13) & Strings.Chr(10) & _
               "null policy_rating_section," & Strings.Chr(13) & Strings.Chr(10) & _
               "rst.code," & Strings.Chr(13) & Strings.Chr(10) & _
               "rs.sum_insured," & Strings.Chr(13) & Strings.Chr(10) & _
               "rs.this_premium," & Strings.Chr(13) & Strings.Chr(10) & _
               "rs.annual_rate," & Strings.Chr(13) & Strings.Chr(10) & _
               "rs.annual_premium original_premium," & Strings.Chr(13) & Strings.Chr(10) & _
               "0 flag," & Strings.Chr(13) & Strings.Chr(10) & _
               "rst.rate_type_id," & Strings.Chr(13) & Strings.Chr(10) & _
               "ISNULL(rs.country_id, 0)," & Strings.Chr(13) & Strings.Chr(10) & _
               "ISNULL(rs.state_id, 0)" & Strings.Chr(13) & Strings.Chr(10) & _
               "FROM rating_section rs," & Strings.Chr(13) & Strings.Chr(10) & _
               "rating_section_type rst" & Strings.Chr(13) & Strings.Chr(10) & _
               "WHERE rs.risk_cnt = " & CStr(m_lOriginalRiskCnt) & Strings.Chr(13) & Strings.Chr(10) & _
               "AND rs.rating_section_type_id = rst.rating_section_type_id" & Strings.Chr(13) & Strings.Chr(10) & _
               "AND rs.original_flag = 1" & Strings.Chr(13) & Strings.Chr(10)

        'Ignore the rounding sections
        If m_sRoundingSectionCode <> "" Then
            '   sSQL = sSQL & "AND rst.code <> '" & Trim$(m_sRoundingSectionCode) & "'" & vbCrLf
        End If

        If UCase(Trim(m_sTransactionType)) = "MTCR" Then
            sSQL = sSQL & "ORDER BY rs.original_flag ASC, rs.rating_section_id" & vbCrLf
        Else
            sSQL = sSQL & "ORDER BY rs.original_flag DESC, rs.rating_section_id" & Strings.Chr(13) & Strings.Chr(10)
        End If
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="Get Manual Rating Sections", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: GetOriginalRatingSections
    '
    ' Description:
    '
    ' History: 29/01/2003 sj - Created.
    '
    ' RDC 13/09/2005 : Added from Sirius 1.9.1
    ' ***************************************************************** '
    Private Function GetOriginalRatingSections(ByRef r_vArray(,) As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim sSQL As String = ""

        'Now get the rating sections from the original risk
        sSQL = "SELECT rs.risk_cnt," & Strings.Chr(13) & Strings.Chr(10) & _
               "rs.rating_section_id," & Strings.Chr(13) & Strings.Chr(10) & _
               "null Decline_reason," & Strings.Chr(13) & Strings.Chr(10) & _
               "null Refer_reason," & Strings.Chr(13) & Strings.Chr(10) & _
               "null Message," & Strings.Chr(13) & Strings.Chr(10) & _
               "null policy_rating_section," & Strings.Chr(13) & Strings.Chr(10) & _
               "rst.code," & Strings.Chr(13) & Strings.Chr(10) & _
               "rs.sum_insured," & Strings.Chr(13) & Strings.Chr(10) & _
               "rs.this_premium," & Strings.Chr(13) & Strings.Chr(10) & _
               "rs.annual_rate," & Strings.Chr(13) & Strings.Chr(10) & _
               "rs.annual_premium original_premium," & Strings.Chr(13) & Strings.Chr(10) & _
               "rs.original_flag flag," & Strings.Chr(13) & Strings.Chr(10) & _
               "rst.rate_type_id," & Strings.Chr(13) & Strings.Chr(10) & _
               "ISNULL(rs.country_id, 0)," & Strings.Chr(13) & Strings.Chr(10) & _
               "ISNULL(rs.state_id, 0)" & Strings.Chr(13) & Strings.Chr(10) & _
               "FROM rating_section rs," & Strings.Chr(13) & Strings.Chr(10) & _
               "rating_section_type rst" & Strings.Chr(13) & Strings.Chr(10) & _
               "WHERE rs.risk_cnt = " & CStr(m_lOriginalRiskCnt) & Strings.Chr(13) & Strings.Chr(10) & _
               "AND rs.rating_section_type_id = rst.rating_section_type_id" & Strings.Chr(13) & Strings.Chr(10) & _
               "AND rs.original_flag in (0,1)" & Strings.Chr(13) & Strings.Chr(10)

        'Ignore the rounding sections
        If m_sRoundingSectionCode <> "" Then
            ' sSQL = sSQL & "AND rst.code <> '" & Trim$(m_sRoundingSectionCode) & "'" & vbCrLf
        End If

        If UCase(Trim(m_sTransactionType)) = "MTCR" Then
            sSQL = sSQL & "ORDER BY rs.original_flag ASC, rs.rating_section_id" & Strings.Chr(13) & Strings.Chr(10)
        Else
            sSQL = sSQL & "ORDER BY rs.original_flag DESC, rs.rating_section_id" & Strings.Chr(13) & Strings.Chr(10)
        End If

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="Get Manual Rating Sections", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: GetDeletedRatingSections
    '
    ' Description:
    '
    ' History: 29/01/2003 sj - Created.
    '
    ' RDC 13/09/2005 : Added from Sirius 1.9.1
    ' ***************************************************************** '
    Private Function GetDeletedRatingSections(ByVal v_lPreviousDeletedRiskInsuranceFileCnt As Object, ByRef r_vArray(,) As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Const ACDeletedPremium As Integer = 0

        Dim sSQL As String = ""
        Dim lOriginalDeletedRiskCnt As Integer
        Dim vArray(,) As Object

        'Get the premium from the deleted risk itself
        sSQL = "SELECT rs.this_premium" & Strings.Chr(13) & Strings.Chr(10) & _
               "FROM rating_section rs," & Strings.Chr(13) & Strings.Chr(10) & _
               "rating_section_type rst" & Strings.Chr(13) & Strings.Chr(10) & _
               "WHERE rs.risk_cnt = " & CStr(m_lOriginalRiskCnt) & Strings.Chr(13) & Strings.Chr(10) & _
               "AND rs.rating_section_type_id = rst.rating_section_type_id" & Strings.Chr(13) & Strings.Chr(10) & _
               "AND rs.original_flag = 1" & Strings.Chr(13) & Strings.Chr(10)

        'Ignore the rounding sections
        If m_sRoundingSectionCode <> "" Then
            sSQL = sSQL & "AND rst.code <> '" & m_sRoundingSectionCode.Trim() & "'" & Strings.Chr(13) & Strings.Chr(10)
        End If

        sSQL = sSQL & "ORDER BY rs.original_flag DESC, rs.rating_section_id" & Strings.Chr(13) & Strings.Chr(10)

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="Get Manual Rating Sections", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        If Not Information.IsArray(vArray) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Get the original risk cnt of the risk that was deleted

        m_lReturn = CType(GetOriginalRiskCnt(v_lInsuranceFileCnt:=CInt(v_lPreviousDeletedRiskInsuranceFileCnt), v_lRiskCnt:=m_lOriginalRiskCnt, r_lOriginalRiskCnt:=lOriginalDeletedRiskCnt), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Now get the rating section from the original risk
        '(i.e. the one the deletion was created from)
        sSQL = "SELECT rs.risk_cnt," & Strings.Chr(13) & Strings.Chr(10) & _
               "rs.rating_section_id," & Strings.Chr(13) & Strings.Chr(10) & _
               "null Decline_reason," & Strings.Chr(13) & Strings.Chr(10) & _
               "null Refer_reason," & Strings.Chr(13) & Strings.Chr(10) & _
               "null Message," & Strings.Chr(13) & Strings.Chr(10) & _
               "null policy_rating_section," & Strings.Chr(13) & Strings.Chr(10) & _
               "rst.code," & Strings.Chr(13) & Strings.Chr(10) & _
               "rs.sum_insured," & Strings.Chr(13) & Strings.Chr(10) & _
               "0," & Strings.Chr(13) & Strings.Chr(10) & _
               "rs.annual_rate," & Strings.Chr(13) & Strings.Chr(10) & _
               "rs.annual_premium original_premium," & Strings.Chr(13) & Strings.Chr(10) & _
               "rs.original_flag flag" & Strings.Chr(13) & Strings.Chr(10) & _
               "FROM rating_section rs," & Strings.Chr(13) & Strings.Chr(10) & _
               "rating_section_type rst" & Strings.Chr(13) & Strings.Chr(10) & _
               "WHERE rs.risk_cnt = " & CStr(lOriginalDeletedRiskCnt) & Strings.Chr(13) & Strings.Chr(10) & _
               "AND rs.rating_section_type_id = rst.rating_section_type_id" & Strings.Chr(13) & Strings.Chr(10) & _
               "AND rs.original_flag = 0" & Strings.Chr(13) & Strings.Chr(10)

        'Ignore the rounding sections
        If m_sRoundingSectionCode <> "" Then
            sSQL = sSQL & "AND rst.code <> '" & m_sRoundingSectionCode.Trim() & "'" & Strings.Chr(13) & Strings.Chr(10)
        End If

        sSQL = sSQL & "ORDER BY rs.original_flag DESC, rs.rating_section_id" & Strings.Chr(13) & Strings.Chr(10)

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="Get Manual Rating Sections", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Information.IsArray(r_vArray) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        For i As Integer = 0 To r_vArray.GetUpperBound(1)
            'Reverse the deleted premium and insert it into the section


            r_vArray(ACOPremium, i) = Conversion.Val(CStr(vArray(ACDeletedPremium, i))) * -1
        Next i

        Return result

    End Function

    Public Function GetRatingSectionType_ForRiskType(ByVal lMode As Integer, ByVal lRiskCnt As Integer, ByVal lRatingSectionTypeId As Integer, ByVal lOriginalRiskCnt As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            'Clear all the parameters
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add("mode", CStr(lMode), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add("risk_cnt", CStr(lRiskCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add("rating_section_type_id", CStr(lRatingSectionTypeId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add("original_risk_cnt", CStr(lOriginalRiskCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'Fetch the records
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectRatingSectionTypeForRiskTypeTypeSQL, sSQLName:=ACSelectRatingSectionTypeForRiskTypeName, bStoredProcedure:=ACSelectRatingSectionTypeForRiskTypeStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResults)

            'Return the result

            Return m_lReturn

        Catch excep As System.Exception



            'Error
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Err_GetRatingSectionType_ForRiskType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Err_GetRatingSectionType_ForRiskType", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    Public Function GetPerilAllocationSecurity(ByVal lRiskCnt As Integer, ByVal iUserID As Integer, ByRef r_bUserAllowRatingSectionAddDelete As Boolean, ByRef r_bUserAllowRatingSectionEdit As Boolean, ByRef r_bAllowRatingSectionAdd As Boolean, ByRef r_bAllowRatingSectionEdit As Boolean, ByRef r_bAllowRatingSectionDelete As Boolean, ByRef r_bAllowEditRatingSectionRateType As Boolean, ByRef r_bAllowEditRatingSectionRate As Boolean, ByRef r_bAllowEditRatingSectionSumInsured As Boolean, ByRef r_bAllowEditRatingSectionThisPremium As Boolean) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=CStr(lRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_allow_ratingsection_adddelete", vValue:=CStr(r_bUserAllowRatingSectionAddDelete), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_allow_ratingsection_editing", vValue:=CStr(r_bUserAllowRatingSectionEdit), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="allow_add_ratingsection", vValue:=CStr(r_bAllowRatingSectionAdd), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="allow_edit_ratingsection", vValue:=CStr(r_bAllowRatingSectionEdit), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="allow_delete_ratingsection", vValue:=CStr(r_bAllowRatingSectionDelete), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="allow_edit_ratingsection_ratetype", vValue:=CStr(r_bAllowEditRatingSectionRateType), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="allow_edit_ratingsection_rate", vValue:=CStr(r_bAllowEditRatingSectionRate), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="allow_edit_ratingsection_suminsured", vValue:=CStr(r_bAllowEditRatingSectionSumInsured), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="allow_edit_ratingsection_thispremium", vValue:=CStr(r_bAllowEditRatingSectionThisPremium), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACSelectPerilAllocationSecuritySQL, sSQLName:=ACSelectPerilAllocationSecurityName, bStoredProcedure:=ACSelectPerilAllocationSecurityStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If Convert.IsDBNull(m_oDatabase.Parameters.Item("user_allow_ratingsection_adddelete").Value) Or IsNothing(m_oDatabase.Parameters.Item("user_allow_ratingsection_adddelete").Value) Then
                r_bUserAllowRatingSectionAddDelete = False
            Else
                r_bUserAllowRatingSectionAddDelete = m_oDatabase.Parameters.Item("user_allow_ratingsection_adddelete").Value
            End If


            If Convert.IsDBNull(m_oDatabase.Parameters.Item("user_allow_ratingsection_editing").Value) Or IsNothing(m_oDatabase.Parameters.Item("user_allow_ratingsection_editing").Value) Then
                r_bUserAllowRatingSectionEdit = False
            Else
                r_bUserAllowRatingSectionEdit = m_oDatabase.Parameters.Item("user_allow_ratingsection_editing").Value
            End If


            If Convert.IsDBNull(m_oDatabase.Parameters.Item("allow_add_ratingsection").Value) Or IsNothing(m_oDatabase.Parameters.Item("allow_add_ratingsection").Value) Then
                r_bAllowRatingSectionAdd = False
            Else
                r_bAllowRatingSectionAdd = m_oDatabase.Parameters.Item("allow_add_ratingsection").Value
            End If


            If Convert.IsDBNull(m_oDatabase.Parameters.Item("allow_edit_ratingsection").Value) Or IsNothing(m_oDatabase.Parameters.Item("allow_edit_ratingsection").Value) Then
                r_bAllowRatingSectionEdit = False
            Else
                r_bAllowRatingSectionEdit = m_oDatabase.Parameters.Item("allow_edit_ratingsection").Value
            End If



            If Convert.IsDBNull(m_oDatabase.Parameters.Item("allow_delete_ratingsection").Value) Or IsNothing(m_oDatabase.Parameters.Item("allow_delete_ratingsection").Value) Then
                r_bAllowRatingSectionDelete = False
            Else
                r_bAllowRatingSectionDelete = m_oDatabase.Parameters.Item("allow_delete_ratingsection").Value
            End If


            If Convert.IsDBNull(m_oDatabase.Parameters.Item("allow_edit_ratingsection_ratetype").Value) Or IsNothing(m_oDatabase.Parameters.Item("allow_edit_ratingsection_ratetype").Value) Then
                r_bAllowEditRatingSectionRateType = False
            Else
                r_bAllowEditRatingSectionRateType = m_oDatabase.Parameters.Item("allow_edit_ratingsection_ratetype").Value
            End If


            If Convert.IsDBNull(m_oDatabase.Parameters.Item("allow_edit_ratingsection_rate").Value) Or IsNothing(m_oDatabase.Parameters.Item("allow_edit_ratingsection_rate").Value) Then
                r_bAllowEditRatingSectionRate = False
            Else
                r_bAllowEditRatingSectionRate = m_oDatabase.Parameters.Item("allow_edit_ratingsection_rate").Value
            End If


            If Convert.IsDBNull(m_oDatabase.Parameters.Item("allow_edit_ratingsection_suminsured").Value) Or IsNothing(m_oDatabase.Parameters.Item("allow_edit_ratingsection_suminsured").Value) Then
                r_bAllowEditRatingSectionSumInsured = False
            Else
                r_bAllowEditRatingSectionSumInsured = m_oDatabase.Parameters.Item("allow_edit_ratingsection_suminsured").Value
            End If


            If Convert.IsDBNull(m_oDatabase.Parameters.Item("allow_edit_ratingsection_thispremium").Value) Or IsNothing(m_oDatabase.Parameters.Item("allow_edit_ratingsection_thispremium").Value) Then
                r_bAllowEditRatingSectionThisPremium = False
            Else
                r_bAllowEditRatingSectionThisPremium = m_oDatabase.Parameters.Item("allow_edit_ratingsection_thispremium").Value
            End If

            Return result

        Catch excep As System.Exception

            ''--Call spu_SIR_GetPerilAllocationSecurity setting return variables


            'Error
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Err_GetPerilAllocationSecurity Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Err_GetPerilAllocationSecurity", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetRisksBilledPremium
    '
    ' Parameters: n/a
    '
    ' Description:
    ' ***************************************************************** '
    Public Function GetRisksBilledPremium(ByVal v_lRiskCnt As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetRisksBilledPremium"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_cnt", CStr(v_lRiskCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetRisksBilledPremiumSQL, sSQLName:=kGetRisksBilledPremiumName, bStoredProcedure:=kGetRisksBilledPremiumStored, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kGetRisksBilledPremiumSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
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

    Public Function CheckMTCRatingRules(ByVal v_lInsuranceFileCnt As Integer, ByRef r_bIsMTCRatingRulesEnabled As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CheckMTCRatingRules"

        Dim vResultArray(,) As Object


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add Parameter
            bPMAddParameter.AddParameterLite(m_oDatabase, "insurance_file_cnt", CStr(v_lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckMTCRatingRulesSQL, sSQLName:=ACCheckMTCRatingRulesName, bStoredProcedure:=ACCheckMTCRatingRulesStored, vResultArray:=vResultArray, lNumberRecords:=gPMConstants.PMAllRecords, bKeepNulls:=True)

            'Check Results
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception
            End If

            'Store Options

            m_bIsMTCRatingRulesEnabled = CDbl(vResultArray(0, 0)) = 1
            r_bIsMTCRatingRulesEnabled = m_bIsMTCRatingRulesEnabled


        Catch ex As Exception

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


        End Try
        Return result
    End Function
    Public Function CheckMandatoryRisk(ByVal m_lRiskCnt As Long, ByRef r_bIsMandatoryRisk As Boolean) As Integer

        Const kMethodName As String = "CheckMandatoryRisk"
        Dim dtResult As New DataTable
        Dim result As gPMConstants.PMEReturnCode
        Dim sqlCmd As New SqlCommand
        Dim sqlDA As New SqlDataAdapter
        Dim oParameter As SqlParameter

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            sqlCmd.CommandText = ACCheckMandatoryRiskSQL

            ' Add Parameter
            oParameter = New SqlParameter
            oParameter.ParameterName = "@risk_cnt"
            oParameter.Value = m_lRiskCnt
            oParameter.Direction = ParameterDirection.Input
            sqlCmd.Parameters.Add(oParameter)

            sqlCmd.CommandType = CommandType.StoredProcedure


            m_lReturn = m_oDatabase.ExecuteDataTable(command:=sqlCmd, adapter:=sqlDA, results:=dtResult)
            'Check Results
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Store Options
            r_bIsMandatoryRisk = IIf(CInt(dtResult.Rows(0)(0).ToString) = 1, True, False)

            Return result

        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=CheckMandatoryRisk, excep:=ex)
            Return result
        End Try

    End Function

    Public Function UpdateRiskStatus(ByVal v_lRiskCnt As Integer, ByVal v_lRiskStatusId As Integer) As Integer

        Const kMethodName As String = "UpdateRiskStatus"
        Dim result As Integer

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="risk_cnt", _
                                                   vValue:=v_lRiskCnt, _
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, _
                                                   iDataType:=gPMConstants.PMEDataType.PMInteger)



            result = m_oDatabase.Parameters.Add(sName:="risk_status_id", _
                                                   vValue:=v_lRiskStatusId, _
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, _
                                                   iDataType:=gPMConstants.PMEDataType.PMInteger)



            result = m_oDatabase.SQLAction(sSQL:=ACUpdateRiskStatusSQL, _
                                              sSQLName:=ACUpdateRiskStatusName, _
                                              bStoredProcedure:=ACUpdateRiskStatusStored)




        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=UpdateRiskStatus, excep:=ex)
            Return result
        End Try
        Return result
    End Function

    Private Function GetProRataForOriginalRisk(ByVal nOriginalRiskCnt As Integer, _
                                               ByRef dProRataRate As Double) As Integer
        Const kMethodName As String = "GetProRataForOriginalRisk"
        Dim nReturn As Integer
        Dim r_vResults(,) As Object


        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        AddParameterLite(m_oDatabase, "risk_cnt", nOriginalRiskCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

        ' Execute selection Query
        nReturn = m_oDatabase.SQLSelect( _
                                sSQL:=kGetProRataForOriginalRiskSQL, _
                                sSQLName:=kGetProRataForOriginalRiskName, _
                                bStoredProcedure:=kGetProRataForOriginalRiskStored, _
                                vResultArray:=r_vResults, _
                                lNumberRecords:=gPMConstants.PMAllRecords)

        If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        Else
            If IsArray(r_vResults) Then
                dProRataRate = ToSafeDouble(r_vResults(0, 0), 1)
            End If
        End If
        Return nReturn

    End Function

    Public Function GetTMPStatus() As Boolean
        Dim oResultArray(,) As Object = Nothing
        If m_nProductId = 0 Then
            bPMAddParameter.AddParameterLite(m_oDatabase, "insurance_file_cnt", CStr(m_lInsuranceFileCnt),
                                               gPMConstants.PMEParameterDirection.PMParamInput,
                                               gPMConstants.PMEDataType.PMLong, True)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectInsuranceFileSQL, sSQLName:=ACSelectInsuranceFileName,
                                              bStoredProcedure:=ACSelectInsuranceFileStored,
                                              lNumberRecords:=gPMConstants.PMAllRecords)

            m_nProductId = gPMFunctions.NullToLong(m_oDatabase.Records.Item(0).Fields()("product_id"))
        End If

        m_oDatabase.Parameters.Clear()
        m_lReturn = m_oDatabase.Parameters.Add("Prod_id", CStr(m_nProductId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetTMPStatusSQL, sSQLName:=ACGetTMPStatusName, bStoredProcedure:=False, vResultArray:=oResultArray, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords)

        If Information.IsArray(oResultArray) Then
            m_bIsTrueMonthlyPolicy = IIf(CDbl(oResultArray(0, 0)) = 1, True, False)
        End If
        Return m_bIsTrueMonthlyPolicy
    End Function

End Class
