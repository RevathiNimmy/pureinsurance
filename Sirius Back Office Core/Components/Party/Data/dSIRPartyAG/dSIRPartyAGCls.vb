Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
'Developer Guide No. 129
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("SIRPartyAG_NET.SIRPartyAG")> _
Public NotInheritable Class SIRPartyAG
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: SIRPartyAG
    '
    ' Date: 04/09/1998
    '
    ' Description: Describes the SIRPartyAG attributes.
    '
    ' Edit History:
    ' PW100702 - add fields required for additioal details tab
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 01/03/2004
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************
    Private m_bAllowReallocationInstalmentDebt As Boolean

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "SIRPartyAG"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag
    Private m_bCloseDatabase As Boolean

    ' DataBase Attributes
    Private m_lPartyCnt As Integer
    Private m_lPartyAgentTypeID As Integer
    Private m_lPartyAgentOriginID As Integer
    'Developer Guide No. 101
    Private m_vAgencyAgreementDate As Object
    Private m_vAgencyNextReviewDate As Object
    'developer guide no.101
    Private m_vAgencyAccountNumber As Object
    Private m_iIsBranch As Integer
    Private m_iIsHeadOffice As Integer
    Private m_sgDefaultCommissionPercent As Single
    'developer guide no.101
    Private m_vTradingName As Object
    Private m_lBinderIndicator As Integer
    Private m_lReportIndicator As Integer
    Private m_lConsultantCnt As Integer
    Private m_lAgentGroupCnt As Integer
    'developer guide no.101
    'start
    Private m_vPaymentMethod As Object
    Private m_vPaymentFrequency As Object
    Private m_vAddressOnNotice As Object
    Private m_vTypeOfStatement As Object
    Private m_vSource As Object
    Private m_sTitle As String = ""
    Private m_iMultipac As Integer
    Private m_sContactPerson As String = ""
    Private m_sFirstName As String = ""
    Private m_sBankAccount As String = ""
    Private m_bAllowConsolidate As Object
    Private m_vDateCancelled As Object
    'developer guide no.101
    'start
    Private m_vAgentStatus As Object
    Private m_sRegistrationNumber As Object
    Private m_vBrokerAbiId As Object
    Private m_vExpenseAccountId As Object
    Private m_vIsInTransferMode As Object
    Private m_vTransferToBusinessTypeID As Object
    Private m_vTransferToPartyCnt As Object
    Private m_vOverride As Object
    Private m_vOverrideRenewal As Object
    Private m_vDomiciledForTax As Object
    'Float Balance and Pre-Payment (RC)
    Private m_vMakeLiveInvoice As Object
    Private m_vMakeLiveInstallments As Object
    Private m_vMakeLivePayNow As Object
    Private m_vMakeLiveBankGuarantee As Object
    Private m_vMakeLiveCashDeposit As Object

    Private m_vIsGrossagent As Object
    Private m_iBankAccount As Integer
    Private m_vIsStandardAccount As Object
    Private m_vIsFloatBalanceAccount As Object
    Private m_vIsPrepaymentAccount As Object
    Private m_vIsOverdraftAccount As Object
    Private m_vFloatBalanceLimit As Object
    Private m_vExpectedDailyPremium As Object
    Private m_vOverdraftLimit As Object
    Private m_vDaysAllowed As Object
    Private m_vOverdraftExpiry As Object
    Private m_vAltRefMandatory As Object
    Private m_vAltRefRequiredForEachTrans As Object
    Private m_vCommissionPostingType As Object
    Private m_bIsViewableOnly As Boolean 'AR - NEXUS MTA

    Private m_vIsSingleInstalmentPlanOnly As Object
    Private m_vCommonRenewalDate As Object
    ' Function Return Code
    Private m_lReturn As gPMConstants.PMEReturnCode
    'Batch Renewal
    Private m_vIsProduceAgentRenewalList As Object
    'end
    'set chosen commission level
    Private m_lCommissionLevel As Integer
    'Float Balance and Pre-Payment (RC)
    'developer guide no.101

    Private m_ReceivesClientCorrespondence As Object
    Private m_sUniqueId As String
    Private m_sScreenHierarchy As String

    Public Property MakeLiveInvoice() As Object
        Get
            Return m_vMakeLiveInvoice
        End Get
        Set(ByVal Value As Object)

            m_vMakeLiveInvoice = Value
        End Set
    End Property
    'developer guide no.101
    Public Property MakeLiveInstallments() As Object
        Get
            Return m_vMakeLiveInstallments
        End Get
        Set(ByVal Value As Object)

            m_vMakeLiveInstallments = Value
        End Set
    End Property
    'developer guide no.101
    Public Property MakeLivePayNow() As Object
        Get
            Return m_vMakeLivePayNow
        End Get
        Set(ByVal Value As Object)

            m_vMakeLivePayNow = Value
        End Set
    End Property
    Public Property AllowReallocationInstalmentDebt() As Boolean
        Get

            AllowReallocationInstalmentDebt = m_bAllowReallocationInstalmentDebt

        End Get
        Set(ByVal Value As Boolean)

            m_bAllowReallocationInstalmentDebt = Value

        End Set
    End Property
    'developer guide no.101
    Public Property MakeLiveBankGuarantee() As Object
        Get
            Return m_vMakeLiveBankGuarantee
        End Get
        Set(ByVal Value As Object)

            m_vMakeLiveBankGuarantee = Value
        End Set
    End Property
    'Start - Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling
    'developer guide no.101
    Public Property MakeLiveCashDeposit() As Object
        Get
            Return m_vMakeLiveCashDeposit
        End Get
        Set(ByVal Value As Object)

            m_vMakeLiveCashDeposit = Value
        End Set
    End Property
    'End - Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling
    'developer guide no.101
    Public Property IsStandardAccount() As Object
        Get
            Return m_vIsStandardAccount
        End Get
        Set(ByVal Value As Object)

            m_vIsStandardAccount = Value
        End Set
    End Property
    'developer guide no.101
    Public Property IsFloatBalanceAccount() As Object
        Get
            Return m_vIsFloatBalanceAccount
        End Get
        Set(ByVal Value As Object)

            m_vIsFloatBalanceAccount = Value
        End Set
    End Property
    'developer guide no.101
    Public Property IsPrepaymentAccount() As Object
        Get
            Return m_vIsPrepaymentAccount
        End Get
        Set(ByVal Value As Object)

            m_vIsPrepaymentAccount = Value
        End Set
    End Property
    'developer guide no.101
    Public Property IsOverdraftAccount() As Object
        Get
            Return m_vIsOverdraftAccount
        End Get
        Set(ByVal Value As Object)

            m_vIsOverdraftAccount = Value
        End Set
    End Property
    'developer guide no.101
    Public Property FloatBalanceLimit() As Object
        Get
            Return m_vFloatBalanceLimit
        End Get
        Set(ByVal Value As Object)

            m_vFloatBalanceLimit = Value
        End Set
    End Property
    'developer guide no.101
    Public Property ExpectedDailyPremium() As Object
        Get
            Return m_vExpectedDailyPremium
        End Get
        Set(ByVal Value As Object)

            m_vExpectedDailyPremium = Value
        End Set
    End Property
    'developer guide no.101
    Public Property OverdraftLimit() As Object
        Get
            Return m_vOverdraftLimit
        End Get
        Set(ByVal Value As Object)

            m_vOverdraftLimit = Value
        End Set
    End Property
    'develoepr guide no.101
    Public Property DaysAllowed() As Object
        Get
            Return m_vDaysAllowed
        End Get
        Set(ByVal Value As Object)

            m_vDaysAllowed = Value
        End Set
    End Property
    'developer guide no.101
    Public Property OverdraftExpiry() As Object
        Get
            Return m_vOverdraftExpiry
        End Get
        Set(ByVal Value As Object)

            m_vOverdraftExpiry = Value
        End Set
    End Property

    '(RC) QBENZ014
    'developer guide no.101
    Public Property AltRefMandatory() As Object
        Get
            Return m_vAltRefMandatory
        End Get
        Set(ByVal Value As Object)

            m_vAltRefMandatory = Value
        End Set
    End Property
    'developer guide no.101
    Public Property AltRefRequiredForEachTrans() As Object
        Get
            Return m_vAltRefRequiredForEachTrans
        End Get
        Set(ByVal Value As Object)

            m_vAltRefRequiredForEachTrans = Value
        End Set
    End Property

    '(RC) PLICO 9-10
    'developer guide no.101
    Public Property CommissionPostingType() As Object
        Get
            Return m_vCommissionPostingType
        End Get
        Set(ByVal Value As Object)

            m_vCommissionPostingType = Value
        End Set
    End Property




    Public Property ConsultantCnt() As Integer
        Get

            Return m_lConsultantCnt

        End Get
        Set(ByVal Value As Integer)

            m_lConsultantCnt = Value

        End Set
    End Property

    Public Property AgentGroupCnt() As Integer
        Get

            Return m_lAgentGroupCnt

        End Get
        Set(ByVal Value As Integer)

            m_lAgentGroupCnt = Value

        End Set
    End Property

    'developer guide no.101
    Public Property PaymentFrequency() As Object
        Get

            Return m_vPaymentFrequency

        End Get
        Set(ByVal Value As Object)


            m_vPaymentFrequency = Value

        End Set
    End Property
    'developer guide no.101
    Public Property PaymentMethod() As Object
        Get

            Return m_vPaymentMethod

        End Get
        Set(ByVal Value As Object)


            m_vPaymentMethod = Value

        End Set
    End Property

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public WriteOnly Property Database() As dPMDAO.Database
        Set(ByVal Value As dPMDAO.Database)

            m_oDatabase = Value

        End Set
    End Property



    Public Property PartyCnt() As Integer
        Get

            Return m_lPartyCnt

        End Get
        Set(ByVal Value As Integer)

            m_lPartyCnt = Value

        End Set
    End Property

    Public Property PartyAgentOriginID() As Integer
        Get

            Return m_lPartyAgentOriginID

        End Get
        Set(ByVal Value As Integer)

            m_lPartyAgentOriginID = Value

        End Set
    End Property


    Public Property PartyAgentTypeID() As Integer
        Get

            Return m_lPartyAgentTypeID

        End Get
        Set(ByVal Value As Integer)

            m_lPartyAgentTypeID = Value

        End Set
    End Property
    'Developer Guide No 101
    Public Property AgencyAgreementDate() As Object
        Get

            Return m_vAgencyAgreementDate

        End Get
        Set(ByVal Value As Object)


            m_vAgencyAgreementDate = Value

        End Set
    End Property
    'Developer Guide No 101
    Public Property AgencyNextReviewDate() As Object
        Get

            Return m_vAgencyNextReviewDate

        End Get
        'Set(ByVal Value As Byte)
        Set(ByVal Value As Object)


            m_vAgencyNextReviewDate = Value

        End Set
    End Property

    'developer guide no.101
    Public Property AgencyAccountNumber() As Object
        Get

            Return m_vAgencyAccountNumber

        End Get
        Set(ByVal Value As Object)


            m_vAgencyAccountNumber = Value

        End Set
    End Property

    Public Property IsBranch() As Integer
        Get

            Return m_iIsBranch

        End Get
        Set(ByVal Value As Integer)

            m_iIsBranch = Value

        End Set
    End Property


    Public Property IsHeadOffice() As Integer
        Get

            Return m_iIsHeadOffice

        End Get
        Set(ByVal Value As Integer)

            m_iIsHeadOffice = Value

        End Set
    End Property
    Public Property DefaultCommissionPercent() As Single
        Get

            Return m_sgDefaultCommissionPercent

        End Get
        Set(ByVal Value As Single)

            m_sgDefaultCommissionPercent = Value

        End Set
    End Property

    ' CF 280699 - Added - Begin
    'developer guide no.101
    Public Property TradingName() As Object
        Get
            Return m_vTradingName
        End Get
        Set(ByVal Value As Object)

            m_vTradingName = Value
        End Set
    End Property

    Public Property BinderIndicator() As Integer
        Get
            Return m_lBinderIndicator
        End Get
        Set(ByVal Value As Integer)
            m_lBinderIndicator = Value
        End Set
    End Property

    Public Property ReportIndicator() As Integer
        Get
            Return m_lReportIndicator
        End Get
        Set(ByVal Value As Integer)
            m_lReportIndicator = Value
        End Set
    End Property
    'developer guide no,.101
    Public Property BrokerAbiId() As Object
        Get
            Return m_vBrokerAbiId
        End Get
        Set(ByVal Value As Object)

            m_vBrokerAbiId = Value
        End Set
    End Property

    Public Property ExpenseAccountId() As Object
        Get
            Return m_vExpenseAccountId
        End Get
        Set(ByVal Value As Object)


            m_vExpenseAccountId = Value
        End Set
    End Property
    'developer guide no.101
    Public Property IsInTransferMode() As Object
        Get
            Return m_vIsInTransferMode
        End Get
        Set(ByVal Value As Object)

            m_vIsInTransferMode = Value
        End Set
    End Property
    'developer guide no.101
    Public Property TransferToBusinessTypeID() As Object
        Get
            Return m_vTransferToBusinessTypeID
        End Get
        Set(ByVal Value As Object)

            m_vTransferToBusinessTypeID = Value
        End Set
    End Property
    'developer guide no.101
    Public Property TransferToPartyCnt() As Object
        Get
            Return m_vTransferToPartyCnt
        End Get
        Set(ByVal Value As Object)

            m_vTransferToPartyCnt = Value
        End Set
    End Property
    'developer guide no.101
    Public Property DomiciledForTax() As Object
        Get
            Return m_vDomiciledForTax
        End Get
        Set(ByVal Value As Object)

            m_vDomiciledForTax = Value
        End Set
    End Property


    Public Property IsViewableOnly() As Boolean
        Get
            Return m_bIsViewableOnly
        End Get
        Set(ByVal Value As Boolean)
            m_bIsViewableOnly = Value
        End Set
    End Property
    'developer guide no.101
    Public Property IsSingleInstalmentPlanOnly() As Object
        Get
            Return m_vIsSingleInstalmentPlanOnly
        End Get
        Set(ByVal Value As Object)
            m_vIsSingleInstalmentPlanOnly = Value
        End Set
    End Property
    'developer guide no.101
    Public Property CommonRenewalDate() As Object
        Get
            Return m_vCommonRenewalDate
        End Get
        Set(ByVal Value As Object)
            m_vCommonRenewalDate = Value
        End Set
    End Property
    'Batch Renewal
    'developer guide no.101
    Public Property IsProduceAgentRenewalList() As Object
        Get
            Return m_vIsProduceAgentRenewalList
        End Get
        Set(ByVal Value As Object)
            m_vIsProduceAgentRenewalList = Value
        End Set
    End Property

    'developer guide no.101
    Public Property AddressOnNotice() As Object
        Get
            Return m_vAddressOnNotice
        End Get
        Set(ByVal Value As Object)
            m_vAddressOnNotice = Value
        End Set
    End Property


    Public Property TypeOfStatement() As Object
        Get
            Return m_vTypeOfStatement
        End Get
        Set(ByVal Value As Object)
            m_vTypeOfStatement = Value
        End Set
    End Property


    Public Property Source() As Object
        Get
            Return m_vSource
        End Get
        Set(ByVal Value As Object)
            m_vSource = Value
        End Set
    End Property


    Public Property Title() As String
        Get
            Return m_sTitle
        End Get
        Set(ByVal Value As String)
            m_sTitle = Value
        End Set
    End Property


    Public Property Multipac() As Integer
        Get
            Return m_iMultipac
        End Get
        Set(ByVal Value As Integer)
            m_iMultipac = Value
        End Set
    End Property


    Public Property ContactPerson() As String
        Get
            Return m_sContactPerson
        End Get
        Set(ByVal Value As String)
            m_sContactPerson = Value
        End Set
    End Property


    Public Property FirstName() As String
        Get
            Return m_sFirstName
        End Get
        Set(ByVal Value As String)
            m_sFirstName = Value
        End Set
    End Property


    Public Property AllowConsolidate() As Object
        Get
            Return m_bAllowConsolidate
        End Get
        Set(ByVal Value As Object)
            m_bAllowConsolidate = Value
        End Set
    End Property

    Public Property DateCancelled() As Object
        Get
            Return m_vDateCancelled
        End Get
        Set(ByVal Value As Object)
            m_vDateCancelled = Value
        End Set
    End Property

    Public Property AgentStatus() As Object
        Get
            Return m_vAgentStatus
        End Get
        Set(ByVal Value As Object)
            m_vAgentStatus = Value
        End Set
    End Property

    Public Property RegistrationNumber() As Object
        Get
            Return m_sRegistrationNumber
        End Get
        Set(ByVal Value As Object)
            m_sRegistrationNumber = Value
        End Set
    End Property
    Public Property Override() As Object
        Get
            Return m_vOverride
        End Get
        Set(ByVal Value As Object)
            m_vOverride = Value
        End Set
    End Property
    Public Property OverrideRenewal() As Object
        Get
            Return m_vOverrideRenewal
        End Get
        Set(ByVal Value As Object)
            m_vOverrideRenewal = Value
        End Set
    End Property
    Public Property CommissionLevel() As Integer
        Get
            CommissionLevel = m_lCommissionLevel
        End Get
        Set(ByVal Value As Integer)
            m_lCommissionLevel = Value
        End Set
    End Property
    Public Property IsGrossAgent() As Object
        Get
            IsGrossAgent = m_vIsGrossagent
        End Get
        Set(ByVal vIsGrossAgent As Object)
            m_vIsGrossagent = vIsGrossAgent
        End Set
    End Property

    Public Property BankAccount() As Integer
        Get
            BankAccount = m_iBankAccount
        End Get
        Set(ByVal iBankAccount As Integer)

            m_iBankAccount = iBankAccount
        End Set
    End Property

    Public Property ReceivesClientCorrespondence As Object
        Get
            Return m_ReceivesClientCorrespondence
        End Get
        Set(value As Object)
            m_ReceivesClientCorrespondence = value
        End Set
    End Property

    Public Property UniqueId() As String
        Get

            Return m_sUniqueId

        End Get
        Set(ByVal Value As String)

            m_sUniqueId = Value

        End Set
    End Property

    Public Property ScreenHierarchy() As String
        Get

            Return m_sScreenHierarchy

        End Get
        Set(ByVal Value As String)

            m_sScreenHierarchy = Value

        End Set
    End Property


    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer
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
            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
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
    ' Name: Add (Public)
    '
    ' Description: Adds to the Database from the Base Details.
    '
    ' ***************************************************************** '
    Public Function Add() As Integer

        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            With m_oDatabase
                ' Clear the Database Parameters Collection
                .Parameters.Clear()
                m_lReturn = CType(AddKeyInputParam(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                ' Add the required INPUT parameters
                m_lReturn = CType(AddInputParam(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                ' Get the Primary Key of the record inserted
                m_lReturn = CType(GetNewPrimaryKeyID(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With
            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Add Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Update (Public)
    '
    ' Description: Updates a single record in the database from
    '              the Base Details.
    '
    ' ***************************************************************** '
    Public Function Update() As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            With m_oDatabase
                ' Clear the Database Parameters Collection
                .Parameters.Clear()
                m_lReturn = CType(AddKeyInputParam(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                ' Add the required INPUT parameters
                m_lReturn = CType(AddInputParam(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACUpdateSQL, sSQLName:=ACUpdateName, bStoredProcedure:=ACUpdateStored, lRecordsAffected:=lRecordsAffected)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                ' Check to see that the record was updated OK
                If lRecordsAffected > 0 Then
                    ' Updated No action required
                Else
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With
            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Delete (Public)
    '
    ' Description: Deletes a single record from the database.
    '
    ' ***************************************************************** '
    Public Function Delete() As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            With m_oDatabase
                ' Clear the Database Parameters Collection
                .Parameters.Clear()
                ' Add PrimaryKey as INPUT parameters
                m_lReturn = CType(AddKeyInputParam(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACDeleteSQL, sSQLName:=ACDeleteName, bStoredProcedure:=ACDeleteStored, lRecordsAffected:=lRecordsAffected)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                ' If record wasn't deleted, error
                If lRecordsAffected > 0 Then
                    ' Deleted, No action required
                Else
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With
            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Delete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Delete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SelectSingle (Public)
    '
    ' Description: Selects the required SIRPartyAG
    '
    ' ***************************************************************** '
    Public Function SelectSingle(Optional ByRef vLockMode As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Default to No Lock if not supplied or not numeric
                Dim dbNumericTemp As Double

                If (Informations.IsNothing(vLockMode)) Or (Not Double.TryParse(CStr(vLockMode), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                    vLockMode = gPMConstants.PMELockMode.PMNoLock
                End If

                ' Add PrimaryKey as INPUT parameters
                m_lReturn = CType(AddKeyInputParam(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = .SQLSelect(sSQL:=ACSelectSingleSQL, sSQLName:=ACSelectSingleName, bStoredProcedure:=ACSelectSingleStored, bKeepNulls:=True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' How many records were selected
                lRecordCount = .Records.Count()

                ' Do we have any records ?
                If lRecordCount = 1 Then
                    ' Selected, No action required
                Else
                    Return gPMConstants.PMEReturnCode.PMNotFound
                End If

                ' Set properties
                m_lReturn = CType(SetPropertiesFromDB(oFields:= .Records.Item(0).Fields()), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectSingle Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectSingle", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetPropertiesFromDB (Public)
    '
    ' Description: Sets the supplied SIRPartyAG properties from a database
    '              record.
    ' ***************************************************************** '
    'Developer Guide No. 112
    Public Function SetPropertiesFromDB(ByRef oFields As DataRow) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ' Populate Base Details
            With oFields
                PartyCnt = gPMFunctions.NullToLong(oFields("party_cnt"))
                If Convert.IsDBNull(PartyAgentTypeID = oFields("party_agent_type_id")) Or Informations.IsNothing(PartyAgentTypeID = oFields("party_agent_type_id")) Then
                    PartyAgentTypeID = 0
                Else
                    PartyAgentTypeID = oFields("party_agent_type_id")
                End If

                PartyAgentOriginID = gPMFunctions.NullToLong(oFields("party_agent_origin_id"))
                If Convert.IsDBNull(oFields("agency_agreement_date")) Or Informations.IsNothing(oFields("agency_agreement_date")) Then
                    '            AgencyAgreementDate = Null
                    AgencyAgreementDate = 0
                Else
                    AgencyAgreementDate = oFields("agency_agreement_date")
                End If
                If Convert.IsDBNull(oFields("agency_next_review_date")) Or Informations.IsNothing(oFields("agency_next_review_date")) Then
                    '           AgencyNextReviewDate = Null
                    AgencyNextReviewDate = 0
                Else
                    AgencyNextReviewDate = oFields("agency_next_review_date")
                End If
                '02082002 CMG/PB Scalability 'NullTo' Changes
                IsBranch = gPMFunctions.NullToLong(oFields("is_branch"))
                IsHeadOffice = gPMFunctions.NullToLong(oFields("is_head_office"))
                DefaultCommissionPercent = gPMFunctions.NullToDecimal(oFields("default_commission_percent"))
                AgencyAccountNumber = gPMFunctions.NullToString(oFields("agency_account_number"))
                TradingName = gPMFunctions.NullToString(oFields("trading_name"))
                If Convert.IsDBNull(oFields("binder_indicator")) Or Informations.IsNothing(oFields("binder_indicator")) Then
                    BinderIndicator = -1
                Else
                    BinderIndicator = oFields("binder_indicator")
                End If
                If Convert.IsDBNull(oFields("report_indicator")) Or Informations.IsNothing(oFields("report_indicator")) Then
                    'BinderIndicator = -1
                    ReportIndicator = -1
                Else
                    ReportIndicator = oFields("report_indicator")
                End If
                If Convert.IsDBNull(oFields("linked_account_executive_id")) Or Informations.IsNothing(oFields("linked_account_executive_id")) Then
                    ConsultantCnt = -1
                Else
                    ConsultantCnt = oFields("linked_account_executive_id")
                End If
                If Convert.IsDBNull(oFields("linked_account_group")) Or Informations.IsNothing(oFields("linked_account_group")) Then
                    AgentGroupCnt = -1
                Else
                    AgentGroupCnt = oFields("linked_account_group")
                End If
                If Convert.IsDBNull(oFields("payment_method")) Or Informations.IsNothing(oFields("payment_method")) Then
                    PaymentMethod = -1
                Else
                    PaymentMethod = oFields("payment_method")
                End If
                If Convert.IsDBNull(oFields("payment_frequency")) Or Informations.IsNothing(oFields("payment_frequency")) Then
                    PaymentFrequency = -1
                Else
                    PaymentFrequency = oFields("payment_frequency")
                End If
                If Convert.IsDBNull(oFields("address_on_notice")) Or Informations.IsNothing(oFields("address_on_notice")) Then
                    AddressOnNotice = -1
                Else
                    AddressOnNotice = oFields("address_on_notice")
                End If
                If Convert.IsDBNull(oFields("title")) Or Informations.IsNothing(oFields("title")) Then
                    Title = ""
                Else
                    Title = oFields("title")
                End If
                If Convert.IsDBNull(oFields("multipac")) Or Informations.IsNothing(oFields("multipac")) Then
                    Multipac = 0
                Else
                    Multipac = oFields("multipac")
                End If
                If Convert.IsDBNull(oFields("contact_person")) Or Informations.IsNothing(oFields("contact_person")) Then
                    ContactPerson = ""
                Else
                    ContactPerson = oFields("contact_person")
                End If
                If Convert.IsDBNull(oFields("first_name")) Or Informations.IsNothing(oFields("first_name")) Then
                    FirstName = ""
                Else
                    FirstName = oFields("first_name")
                End If
                If Convert.IsDBNull(oFields("date_cancelled")) Or Informations.IsNothing(oFields("date_cancelled")) Then
                    DateCancelled = 0
                Else
                    DateCancelled = oFields("date_cancelled")
                End If
                If Convert.IsDBNull(oFields("agent_status_id")) Or Informations.IsNothing(oFields("agent_status_id")) Then
                    AgentStatus = -1
                Else
                    AgentStatus = oFields("agent_status_id")
                End If
                'DC021203 -PN8727 -fsa compliance -registration number
                If Convert.IsDBNull(oFields("fsa_registration_number")) Or Informations.IsNothing(oFields("fsa_registration_number")) Then
                    RegistrationNumber = ""
                Else
                    RegistrationNumber = oFields("fsa_registration_number")
                End If
                If Convert.IsDBNull(oFields("broker_abi_id")) Or Informations.IsNothing(oFields("broker_abi_id")) Then
                    BrokerAbiId = ""
                Else
                    BrokerAbiId = oFields("broker_abi_id")
                End If
                'DC141204
                If Convert.IsDBNull(oFields("expense_account_id")) Or Informations.IsNothing(oFields("expense_account_id")) Then
                    ExpenseAccountId = Nothing
                Else
                    ExpenseAccountId = oFields("expense_account_id")
                End If
                AllowConsolidate = gPMFunctions.ToSafeBoolean(oFields("allow_consolidated_commission"))
                IsInTransferMode = gPMFunctions.ToSafeInteger(oFields("is_in_transfer_mode"), 0)

                TransferToBusinessTypeID = gPMFunctions.ToSafeLong(oFields("transfer_to_business_type_id"), 0)

                TransferToPartyCnt = gPMFunctions.ToSafeLong(oFields("transfer_to_party_cnt"), 0)

                Override = gPMFunctions.ToSafeInteger(oFields("use_override_commission_rate"))
                OverrideRenewal = gPMFunctions.ToSafeInteger(oFields("use_override_commission_renewal"))
                'DC130706 Datasure - change to boolean
                DomiciledForTax = gPMFunctions.ToSafeBoolean(oFields("domiciled_for_tax"))
                'Float Balance and Pre-Payment (RC)
                MakeLiveInvoice = gPMFunctions.ToSafeBoolean(oFields("can_make_live_invoice"))
                MakeLiveInstallments = gPMFunctions.ToSafeBoolean(oFields("can_make_live_instalments"))
                MakeLivePayNow = gPMFunctions.ToSafeBoolean(oFields("can_make_live_paynow"))
                MakeLiveBankGuarantee = gPMFunctions.ToSafeBoolean(oFields("can_make_live_bankguarantee")) ' Gaurav
                MakeLiveCashDeposit = gPMFunctions.ToSafeBoolean(oFields("can_make_live_cashdeposit"))

                IsStandardAccount = gPMFunctions.ToSafeBoolean(oFields("is_standard_account"))
                IsFloatBalanceAccount = gPMFunctions.ToSafeBoolean(oFields("is_float_balance_account"))
                IsPrepaymentAccount = gPMFunctions.ToSafeBoolean(oFields("is_prepayment_account"))
                IsOverdraftAccount = gPMFunctions.ToSafeBoolean(oFields("is_overdraft_account"))
                FloatBalanceLimit = gPMFunctions.ToSafeCurrency(oFields("float_balance_limit"))
                ExpectedDailyPremium = gPMFunctions.ToSafeCurrency(oFields("expected_daily_premium"))
                OverdraftLimit = gPMFunctions.ToSafeCurrency(oFields("overdraft_limit"))
                DaysAllowed = gPMFunctions.ToSafeInteger(oFields("days_allowed"))
                If Informations.IsDate(oFields("overdraft_expiry")) Then
                    OverdraftExpiry = gPMFunctions.ToSafeDate(oFields("overdraft_expiry"))
                Else
                    OverdraftExpiry = Nothing
                End If
                AltRefMandatory = gPMFunctions.ToSafeBoolean(oFields("alternate_reference_mandatory"))
                AltRefRequiredForEachTrans = gPMFunctions.ToSafeBoolean(oFields("alternate_reference_for_each_transaction"))
                CommissionPostingType = gPMFunctions.ToSafeLong(oFields("commission_posting_type_id"))
                IsViewableOnly = gPMFunctions.ToSafeBoolean(oFields("is_viewable_only"))
                IsSingleInstalmentPlanOnly = gPMFunctions.NullToLong(oFields("is_single_instalment_plan"))
                If Informations.IsDate(oFields("common_renewal_date")) Then
                    CommonRenewalDate = gPMFunctions.ToSafeDate(oFields("common_renewal_date"))
                Else
                    CommonRenewalDate = Nothing
                End If
                'Batch Renewal
                IsProduceAgentRenewalList = gPMFunctions.ToSafeBoolean(oFields("produce_agent_renewal_list"))
                m_lCommissionLevel = gPMFunctions.ToSafeLong(oFields("commission_level_id"))

                IsGrossAgent = NullToLong(.Item("is_gross_agent"))

                If Informations.IsNothing(.Item("bankaccount_id")) Then
                    BankAccount = 0
                Else
                    BankAccount = .Item("bankaccount_id")
                End If
                ReceivesClientCorrespondence = gPMFunctions.ToSafeBoolean(oFields("receives_client_correspondence"))
            End With
            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetPropertiesFromDB Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetPropertiesFromDB", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function
    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)
    ' ***************************************************************** '
    ' Name: AddInputParam (Private)
    '
    ' Description: Adds all of the NON-KEY INPUT parameters
    '              required for an Insert or Update.
    '
    ' ***************************************************************** '
    Private Function AddInputParam() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            If PartyAgentTypeID <= 0 Then

                'Developer Guide No. 85
                m_lReturn = .Parameters.Add("party_agent_type_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="party_agent_type_id", vValue:=PartyAgentTypeID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="party_agent_origin_id", vValue:=PartyAgentOriginID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="agency_agreement_date", vValue:=AgencyAgreementDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="agency_next_review_date", vValue:=AgencyNextReviewDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="agency_account_number", vValue:=AgencyAccountNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="is_branch", vValue:=IsBranch, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="is_head_office", vValue:=IsHeadOffice, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Changed from PMDecimal to PMDouble
            m_lReturn = .Parameters.Add(sName:="default_commission_percent", vValue:=DefaultCommissionPercent, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="trading_name", vValue:=TradingName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If its -1 then nothing was selected in the list, so add a null
            If (Not True) Or (BinderIndicator < 0) Then

                'Developer Guide No. 85
                m_lReturn = .Parameters.Add("binder_indicator", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="binder_indicator", vValue:=BinderIndicator, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If its -1 then nothing was selected in the list, so add a null
            If (Not True) Or (ReportIndicator < 0) Then

                'Developer Guide No. 85
                m_lReturn = .Parameters.Add(sName:="report_indicator", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="report_indicator", vValue:=ReportIndicator, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="linked_account_executive_id", vValue:=ConsultantCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="linked_account_group", vValue:=AgentGroupCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="payment_method", vValue:=PaymentMethod, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="payment_frequency", vValue:=PaymentFrequency, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="address_on_notice", vValue:=AddressOnNotice, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            m_lReturn = .Parameters.Add(sName:="type_of_statement", vValue:=TypeOfStatement, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="source", vValue:=Source, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="title", vValue:=Title, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'm_lReturn = .Parameters.Add(sName:="multipac", vValue:=CStr(Multipac), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            m_lReturn = .Parameters.Add(sName:="multipac", vValue:=Multipac, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="contact_person", vValue:=ContactPerson, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="first_name", vValue:=FirstName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'm_lReturn = .Parameters.Add(sName:="bank_account", vValue:=BankAccount, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '    Return gPMConstants.PMEReturnCode.PMFalse
            'End If

            'm_lReturn = .Parameters.Add(sName:="allow_consolidated_commission", vValue:=CStr(AllowConsolidate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)
            m_lReturn = .Parameters.Add(sName:="allow_consolidated_commission", vValue:=AllowConsolidate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="date_cancelled", vValue:=DateCancelled, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DC220803 -PS253 -fsa compliance
            m_lReturn = .Parameters.Add(sName:="agent_status_id", vValue:=AgentStatus, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DC021203 -PN8727 -fsa compliance -registration number
            m_lReturn = .Parameters.Add(sName:="fsa_registration_number", vValue:=RegistrationNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="broker_abi_id", vValue:=BrokerAbiId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DC141204
            m_lReturn = .Parameters.Add(sName:="expense_account_id", vValue:=ExpenseAccountId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add("is_in_transfer_mode", If(m_vIsInTransferMode = 0, DBNull.Value, m_vIsInTransferMode), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'Developer Guide No. 85
            m_lReturn = .Parameters.Add("transfer_to_business_type_id", If(m_vTransferToBusinessTypeID = 0, DBNull.Value, m_vTransferToBusinessTypeID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'Developer Guide No. 85
            m_lReturn = .Parameters.Add("transfer_to_party_cnt", If(m_vTransferToPartyCnt = 0, DBNull.Value, m_vTransferToPartyCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="use_override_commission_rate", vValue:=Override, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="use_override_commission_renewal", vValue:=OverrideRenewal, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DC130706 Datasure - change to boolean
            'm_lReturn = .Parameters.Add(sName:="domiciled_for_tax", vValue:=CStr(DomiciledForTax), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            m_lReturn = .Parameters.Add(sName:="domiciled_for_tax", vValue:=DomiciledForTax, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'Float Balance and Pre-Payment (RC)
            m_lReturn = .Parameters.Add(sName:="can_make_live_invoice", vValue:=MakeLiveInvoice, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = .Parameters.Add(sName:="can_make_live_instalments", vValue:=MakeLiveInstallments, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'm_lReturn = .Parameters.Add(sName:="can_make_live_paynow", vValue:=CStr(MakeLivePayNow), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            m_lReturn = .Parameters.Add(sName:="can_make_live_paynow", vValue:=MakeLivePayNow, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'm_lReturn = .Parameters.Add(sName:="is_standard_account", vValue:=CStr(IsStandardAccount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            m_lReturn = .Parameters.Add(sName:="is_standard_account", vValue:=IsStandardAccount, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'm_lReturn = .Parameters.Add(sName:="is_float_balance_account", vValue:=CStr(IsFloatBalanceAccount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            m_lReturn = .Parameters.Add(sName:="is_float_balance_account", vValue:=IsFloatBalanceAccount, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'm_lReturn = .Parameters.Add(sName:="is_overdraft_account", vValue:=CStr(IsOverdraftAccount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            m_lReturn = .Parameters.Add(sName:="is_overdraft_account", vValue:=IsOverdraftAccount, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'm_lReturn = .Parameters.Add(sName:="is_prepayment_account", vValue:=CStr(IsPrepaymentAccount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            m_lReturn = .Parameters.Add(sName:="is_prepayment_account", vValue:=IsPrepaymentAccount, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="expected_daily_premium", vValue:=ExpectedDailyPremium, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="days_allowed", vValue:=DaysAllowed, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="float_balance_limit", vValue:=FloatBalanceLimit, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="overdraft_limit", vValue:=OverdraftLimit, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Developer Guide No. 40
            m_lReturn = .Parameters.Add(sName:="overdraft_expiry", vValue:=OverdraftExpiry, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '(RC) QBENZ014
            'm_lReturn = .Parameters.Add(sName:="alternate_reference_mandatory", vValue:=CStr(AltRefMandatory), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            m_lReturn = .Parameters.Add(sName:="alternate_reference_mandatory", vValue:=AltRefMandatory, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'm_lReturn = .Parameters.Add(sName:="alternate_reference_for_each_transaction", vValue:=CStr(AltRefRequiredForEachTrans), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            m_lReturn = .Parameters.Add(sName:="alternate_reference_for_each_transaction", vValue:=AltRefRequiredForEachTrans, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '(RC) PLICO 9-10
            m_lReturn = .Parameters.Add(sName:="commission_posting_type", vValue:=CommissionPostingType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'm_lReturn = .Parameters.Add(sName:="is_single_instalment_plan", vValue:=CStr(IsSingleInstalmentPlanOnly), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            m_lReturn = .Parameters.Add(sName:="is_single_instalment_plan", vValue:=IsSingleInstalmentPlanOnly, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Developer Guide No. 40
            m_lReturn = .Parameters.Add(sName:="common_renewal_date", vValue:=CommonRenewalDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Batch Renewal
            'm_lReturn = .Parameters.Add(sName:="produce_agent_renewal_list", vValue:=CStr(IsProduceAgentRenewalList), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)
            m_lReturn = .Parameters.Add(sName:="produce_agent_renewal_list", vValue:=IsProduceAgentRenewalList, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Gaurav
            'm_lReturn = .Parameters.Add(sName:="can_make_live_bankguarantee", vValue:=MakeLiveBankGuarantee, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            m_lReturn = .Parameters.Add(sName:="can_make_live_bankguarantee", vValue:=MakeLiveBankGuarantee, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'Start - Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling
            'm_lReturn = .Parameters.Add(sName:="can_make_live_cashdeposit", vValue:=CStr(MakeLiveCashDeposit), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            m_lReturn = .Parameters.Add(sName:="can_make_live_cashdeposit", vValue:=MakeLiveCashDeposit, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'End - Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling
            'GK : 03/08/10: WPR - 14 SAGICOR
            If m_lCommissionLevel > 0 Then
                m_lReturn = .Parameters.Add(sName:="commission_level_id", vValue:=m_lCommissionLevel, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    AddInputParam = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If
            End If

            m_lReturn = .Parameters.Add(sName:="is_gross_agent", vValue:=IsGrossAgent, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                AddInputParam = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If


            If (BankAccount = 0) Then
                m_lReturn = .Parameters.Add(sName:="bankaccount_id", vValue:=Nothing, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="bankaccount_id", vValue:=BankAccount, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                AddInputParam = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = .Parameters.Add(sName:="receives_client_correspondence", vValue:=ReceivesClientCorrespondence, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                AddInputParam = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = .Parameters.Add(sName:="user_id", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                AddInputParam = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = .Parameters.Add(sName:="unique_id", vValue:=m_sUniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                AddInputParam = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = .Parameters.Add(sName:="screen_hierarchy", vValue:=m_sScreenHierarchy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                AddInputParam = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddKeyInputParam (Private)
    '
    ' Description: Adds all of the PRIMARY KEY INPUT parameters
    '              required for a Select, Delete or Update.
    '
    ' ***************************************************************** '
    Private Function AddKeyInputParam() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase
            m_lReturn = .Parameters.Add(sName:="party_cnt", vValue:=CStr(PartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        End With
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: AddKeyOutputParam (Private)
    '
    ' Description: Adds all of the PRIMARY KEY OUTPUT parameters
    '              required for an Add.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (AddKeyOutputParam) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function AddKeyOutputParam() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'With m_oDatabase
    '
    'm_lReturn = .Parameters.Add(sName:="party_cnt", vValue:=CStr(0), idirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
    '
    'End With
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddKeyOutputParam Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddKeyOutputParam", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: GetNewPrimaryKeyID (Private)
    '
    ' Description: Returns the new PRIMARY KEY values from an Add.
    '
    ' ***************************************************************** '
    Private Function GetNewPrimaryKeyID() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            PartyCnt = .Parameters.Item("party_cnt").Value

        End With

        Return result

    End Function
    ' PRIVATE Methods (End)

    Public Sub New()
        MyBase.New()

        ' Class Initialise

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error.
        '
        ' Log Error Message
        'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class

