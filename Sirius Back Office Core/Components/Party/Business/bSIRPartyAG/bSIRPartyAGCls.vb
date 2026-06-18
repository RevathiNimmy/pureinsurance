Option Strict Off
Option Explicit On
Imports System.Globalization
'Developer Guide No. 129
Imports SSP.Shared
Friend NotInheritable Class SIRPartyAG
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: SIRPartyAG
    '
    ' Date: 12/10/1998
    '
    ' Description: Describes the SIRPartyAG attributes.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 27/11/2003
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
    Private Const ACClass As String = "SIRPartyAG"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' Instance of Data component
    Private m_dSIRPartyAG As dSIRPartyAG.SIRPartyAG ' was dSIRPartyAG.SIRPartyAG

    Private m_bSIRParty As Object ' bSIRParty.Business

    ' Error Code
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Primary Keys to work with
    Private m_lPartyCnt As Integer
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public Property DatabaseStatus() As Integer
        Get

            Return m_iDatabaseStatus

        End Get
        Set(ByVal Value As Integer)

            m_iDatabaseStatus = Value

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

    Public ReadOnly Property bSIRParty() As Object
        Get

            Return m_bSIRParty

        End Get
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

            ' Create instance of data class
            m_dSIRPartyAG = New dSIRPartyAG.SIRPartyAG()
            '    Set m_dSIRPartyAG = New dSIRPartyAG.SIRPartyAG

            m_lReturn = m_dSIRPartyAG.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=vDatabase)



            '  m_bSIRParty = New bSIRParty.Business
            m_bSIRParty = Nothing
            If m_bSIRParty Is Nothing Then
                result = gPMComponentServices.CreateBusinessObject(r_oObject:=m_bSIRParty, v_sClassName:="bSIRParty.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=vDatabase)
                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    Dim r_sMessage As String = "Failed to create an instance of bSIRParty.Business"
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="ReprocessClaim", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
                    Return result
                End If
            End If
            m_lReturn = m_bSIRParty.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ACApp, vDatabase:=vDatabase)


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
            Me.disposedValue = True
            If disposing Then
                If m_dSIRPartyAG IsNot Nothing Then
                    m_dSIRPartyAG.Dispose()
                    m_dSIRPartyAG = Nothing
                End If
                If m_bSIRParty IsNot Nothing Then
                    m_bSIRParty.Dispose()
                    m_bSIRParty = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetDefaults (Public)
    '
    ' Description: Returns the Default Values for the SIRPartyAG.
    '
    ' ***************************************************************** '
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyAgentOriginID As Object = Nothing, Optional ByRef vIsBranch As Object = Nothing, Optional ByRef vIsHeadOffice As Object = Nothing, Optional ByRef vAgencyAgreementDate As Object = Nothing, Optional ByRef vAgencyNextReviewDate As Object = Nothing, Optional ByRef vAgencyAccountNumber As Object = Nothing, Optional ByRef vDefaultCommissionPercent As Object = Nothing, Optional ByRef vTradingName As Object = Nothing, Optional ByRef vBinderIndicator As Object = Nothing, Optional ByRef vReportIndicator As Object = Nothing, Optional ByRef vIsInTransferMode As Object = Nothing, Optional ByRef vTransferToBusinessTypeID As Object = Nothing, Optional ByRef vTransferToPartyCnt As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults










            'Developer Guide No. 101
            m_lReturn = CType(DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vPartyCnt:=vPartyCnt, vPartyAgentOriginID:=vPartyAgentOriginID, vIsBranch:=vIsBranch, vIsHeadOffice:=vIsHeadOffice, vAgencyAgreementDate:=vAgencyAgreementDate, vAgencyNextReviewDate:=vAgencyNextReviewDate, vAgencyAccountNumber:=vAgencyAccountNumber, vDefaultCommissionPercent:=vDefaultCommissionPercent, vTradingName:=vTradingName, vBinderIndicator:=vBinderIndicator, vReportIndicator:=vReportIndicator, vIsInTransferMode:=vIsInTransferMode, vTransferToBusinessTypeID:=vTransferToBusinessTypeID, vTransferToPartyCnt:=vTransferToPartyCnt), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefaults Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaults", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetProperties (Public)
    '
    ' Description: Sets the supplied SIRPartyAG property values.
    '
    ' ***************************************************************** '
    ' PW100702 - add consultant/agent/others
    'DC180803 -added agent status id
    'DC021203 -PN8727 -fsa compliance -registration number
    'DC141204 -added expense account id
    'Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling - Added vMakeLiveCashDeposit
    'Developer Guide No. 101
    Public Function SetProperties(ByRef iStatus As Integer, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyAgentTypeID As Object = Nothing, Optional ByRef vPartyAgentOriginID As Object = Nothing, Optional ByRef vIsBranch As Object = Nothing, Optional ByRef vIsHeadOffice As Object = Nothing, Optional ByRef vAgencyAgreementDate As Object = Nothing, Optional ByRef vAgencyNextReviewDate As Object = Nothing, Optional ByRef vAgencyAccountNumber As Object = Nothing, Optional ByRef vDefaultCommissionPercent As Object = Nothing, Optional ByRef vTradingName As Object = Nothing, Optional ByRef vBinderIndicator As Object = Nothing, Optional ByRef vReportIndicator As Object = Nothing, Optional ByRef vConsultantCnt As Object = Nothing, Optional ByRef vAgentGroupCnt As Object = Nothing, Optional ByRef vPaymentMethod As Object = Nothing, Optional ByRef vPaymentFrequency As Object = Nothing, Optional ByRef vAddressOnNotice As Object = Nothing, Optional ByRef vTypeOfStatement As Object = Nothing, Optional ByRef vSource As Object = Nothing, Optional ByRef vTitle As Object = Nothing, Optional ByRef vMultipac As Object = Nothing, Optional ByRef vContactPerson As Object = Nothing, Optional ByRef vFirstName As Object = Nothing, Optional ByRef vDateCancelled As Object = Nothing, Optional ByRef vAgentStatus As Object = Nothing, Optional ByRef vRegistrationNumber As Object = Nothing, Optional ByRef vBrokerAbiId As Object = Nothing, Optional ByRef vExpenseAccountId As Object = Nothing, Optional ByRef vIsInTransferMode As Object = Nothing, Optional ByRef vTransferToBusinessTypeID As Object = Nothing, Optional ByRef vTransferToPartyCnt As Object = Nothing, Optional ByRef vOverrideCommission As Object = Nothing, Optional ByRef vOverrideCommissionRenewal As Object = Nothing, Optional ByRef vDomiciledForTax As Object = Nothing, Optional ByRef vAllowConsolidate As Object = Nothing, Optional ByRef vParamArray As Object = Nothing, Optional ByRef vBankAccount As Object = Nothing, Optional ByRef vMakeLiveInstallments As Object = Nothing, Optional ByRef vMakeLivePayNow As Object = Nothing, Optional ByRef vIsStandardAccount As Object = Nothing, Optional ByRef vIsFloatBalanceAccount As Object = Nothing, Optional ByRef vIsPrepaymentAccount As Object = Nothing, Optional ByRef vIsOverdraftAccount As Object = Nothing, Optional ByRef vFloatBalanceLimit As Object = Nothing, Optional ByRef vExpectedDailyPremium As Object = Nothing, Optional ByRef vOverdraftLimit As Object = Nothing, Optional ByRef vDaysAllowed As Object = Nothing, Optional ByRef vOverdraftExpiry As Object = Nothing, Optional ByRef vAltRefMandatory As Object = Nothing, Optional ByRef vAltRefRequiredForEachTrans As Object = Nothing, Optional ByRef vCommissionPostingType As Object = Nothing, Optional ByRef vIsSingleInstalmentPlanOnly As Object = Nothing, Optional ByRef vCommonRenewalDate As Object = Nothing, Optional ByRef vIsProduceAgentRenewalList As Object = Nothing, Optional ByRef vMakeLiveBankGuarantee As Object = Nothing, Optional ByRef vMakeLiveCashDeposit As Object = Nothing, Optional ByRef v_lCommissionlevel As Integer = 0, Optional ByRef vReceivesClientCorrespondence As Object = Nothing, Optional vUniqueId As String = "", Optional vScreenHierarchy As String = "") As Integer '(RC) QBENZ014  '(RC) PLICO 9-1
        Dim result As Integer = 0
        Dim bDataChanged As Boolean
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' If Add Mode
            If (iStatus = gPMConstants.PMEComponentAction.PMAdd) Then

                ' Default Any Missing Parameters
                ' PW100702 - add consultant/agent
                'DC021203 -PN8727 -fsa compliance -registration number
                'DC141204 -added expense account id
                m_lReturn = DefaultParameters(bDefaultAll:=False, vPartyCnt:=vPartyCnt, vPartyAgentTypeID:=vPartyAgentTypeID, vPartyAgentOriginID:=vPartyAgentOriginID, vIsBranch:=vIsBranch, vIsHeadOffice:=vIsHeadOffice, vAgencyAgreementDate:=vAgencyAgreementDate, vAgencyNextReviewDate:=vAgencyNextReviewDate, vAgencyAccountNumber:=vAgencyAccountNumber, vDefaultCommissionPercent:=vDefaultCommissionPercent, vTradingName:=vTradingName, vBinderIndicator:=vBinderIndicator, vReportIndicator:=vReportIndicator, vConsultantCnt:=vConsultantCnt, vAgentGroupCnt:=vAgentGroupCnt, vPaymentMethod:=vPaymentMethod, vPaymentFrequency:=vPaymentFrequency, vAddressOnNotice:=vAddressOnNotice, vTypeOfStatement:=vTypeOfStatement, vSource:=vSource, vTitle:=vTitle, vMultipac:=vMultipac, vContactPerson:=vContactPerson, vFirstName:=vFirstName, vDateCancelled:=vDateCancelled, vAgentStatus:=vAgentStatus, vRegistrationNumber:=vRegistrationNumber, vBrokerAbiId:=vBrokerAbiId, vExpenseAccountId:=vExpenseAccountId, vIsInTransferMode:=vIsInTransferMode, vTransferToBusinessTypeID:=vTransferToBusinessTypeID, vTransferToPartyCnt:=vTransferToPartyCnt, vOverrideCommission:=vOverrideCommission, vOverrideCommissionRenewal:=vOverrideCommissionRenewal, vDomiciledForTax:=vDomiciledForTax, vAllowConsolidate:=vAllowConsolidate)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

            End If

            ' Validate Parameters
            ' PW100702 - add consultant/agent
            m_lReturn = CType(Validate(vPartyCnt:=vPartyCnt, vPartyAgentTypeID:=vPartyAgentTypeID, vPartyAgentOriginID:=vPartyAgentOriginID, vIsBranch:=vIsBranch, vIsHeadOffice:=vIsHeadOffice, vAgencyAgreementDate:=vAgencyAgreementDate, vAgencyNextReviewDate:=vAgencyNextReviewDate, vAgencyAccountNumber:=vAgencyAccountNumber, vDefaultCommissionPercent:=vDefaultCommissionPercent, vTradingName:=vTradingName, vBinderIndicator:=vBinderIndicator, vReportIndicator:=vReportIndicator, vConsultantCnt:=vConsultantCnt, vAgentGroupCnt:=vAgentGroupCnt, vIsInTransferMode:=vIsInTransferMode, vTransferToBusinessTypeID:=vTransferToBusinessTypeID, vTransferToPartyCnt:=vTransferToPartyCnt), gPMConstants.PMEReturnCode)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return m_lReturn
            End If

            ' Set Data Changed Flag to False
            bDataChanged = False

            ' Set Property values.
            With m_dSIRPartyAG

                'developer guide no.115
                'start change
                If (Not Informations.IsNothing(vPartyCnt)) AndAlso (Not vPartyCnt.Equals(0)) Then
                    .PartyCnt = vPartyCnt
                End If



                If (Not Informations.IsNothing(vPartyAgentOriginID)) AndAlso (Not vPartyAgentOriginID.Equals(0)) Then
                    .PartyAgentOriginID = vPartyAgentOriginID
                End If



                If (Not Informations.IsNothing(vIsBranch)) AndAlso (Not vIsBranch.Equals(0)) Then
                    .IsBranch = vIsBranch
                End If



                If (Not Informations.IsNothing(vPartyAgentTypeID)) AndAlso (Not vPartyAgentTypeID.Equals(0)) Then
                    .PartyAgentTypeID = vPartyAgentTypeID
                End If



                If (Not Informations.IsNothing(vIsHeadOffice)) AndAlso (Not vIsHeadOffice.Equals(0)) Then
                    .IsHeadOffice = vIsHeadOffice
                End If



                If (Not Informations.IsNothing(vAgencyAgreementDate)) AndAlso (Not vAgencyAgreementDate.Equals(0)) Then
                    .AgencyAgreementDate = vAgencyAgreementDate
                End If



                If (Not Informations.IsNothing(vAgencyNextReviewDate)) AndAlso (Not vAgencyNextReviewDate.Equals(0)) Then
                    .AgencyNextReviewDate = vAgencyNextReviewDate
                End If



                If (Not Informations.IsNothing(vAgencyAccountNumber)) AndAlso (Not String.IsNullOrEmpty(vAgencyAccountNumber)) Then
                    .AgencyAccountNumber = vAgencyAccountNumber
                End If



                If (Not Informations.IsNothing(vDefaultCommissionPercent)) AndAlso (Not vDefaultCommissionPercent.Equals(0)) Then
                    .DefaultCommissionPercent = vDefaultCommissionPercent
                End If



                If (Not Informations.IsNothing(vTradingName)) AndAlso (Not String.IsNullOrEmpty(vTradingName)) Then
                    .TradingName = vTradingName
                End If



                If (Not Informations.IsNothing(vBinderIndicator)) Then
                    .BinderIndicator = vBinderIndicator
                End If



                If (Not Informations.IsNothing(vReportIndicator)) Then
                    .ReportIndicator = vReportIndicator
                End If



                If (Not Informations.IsNothing(vConsultantCnt)) AndAlso (Not vConsultantCnt.Equals(0)) Then
                    .ConsultantCnt = vConsultantCnt
                End If



                ' If (Not Informations.IsNothing(vAgentGroupCnt)) AndAlso (Not vAgentGroupCnt.Equals(0)) Then
                .AgentGroupCnt = vAgentGroupCnt
                ' End If



                If (Not Informations.IsNothing(vPaymentMethod)) AndAlso (Not vPaymentMethod.Equals(0)) Then
                    .PaymentMethod = vPaymentMethod
                End If



                If (Not Informations.IsNothing(vPaymentFrequency)) AndAlso (Not vPaymentFrequency.Equals(0)) Then
                    .PaymentFrequency = vPaymentFrequency
                End If



                If (Not Informations.IsNothing(vAddressOnNotice)) AndAlso (Not vAddressOnNotice.Equals(0)) Then
                    .AddressOnNotice = vAddressOnNotice
                End If



                If (Not Informations.IsNothing(vTypeOfStatement)) AndAlso (Not Object.Equals(vTypeOfStatement, Nothing)) Then


                    .TypeOfStatement = vTypeOfStatement
                End If



                If (Not Informations.IsNothing(vSource)) AndAlso (Not Object.Equals(vSource, Nothing)) Then


                    .Source = vSource
                End If



                If (Not Informations.IsNothing(vTitle)) AndAlso (Not String.IsNullOrEmpty(vTitle)) Then
                    .Title = vTitle
                End If



                If (Not Informations.IsNothing(vMultipac)) AndAlso (Not vMultipac.Equals(0)) Then
                    .Multipac = vMultipac
                End If



                If (Not Informations.IsNothing(vContactPerson)) AndAlso (Not String.IsNullOrEmpty(vContactPerson)) Then
                    .ContactPerson = vContactPerson
                End If



                If (Not Informations.IsNothing(vFirstName)) AndAlso (Not String.IsNullOrEmpty(vFirstName)) Then
                    .FirstName = vFirstName
                End If

                'If (Not Informations.IsNothing(vBankAccount)) AndAlso (Not String.IsNullOrEmpty(vBankAccount)) Then
                '    .BankAccount = vBankAccount
                'End If



                If (Not Informations.IsNothing(vDateCancelled)) AndAlso (Not vDateCancelled.Equals(0)) Then
                    .DateCancelled = vDateCancelled
                End If

                'DC180803


                If (Not Informations.IsNothing(vAgentStatus)) AndAlso (Not vAgentStatus.Equals(0)) Then
                    .AgentStatus = vAgentStatus
                End If

                'DC021203 -PN8727 -fsa compliance -registration number


                If (Not Informations.IsNothing(vRegistrationNumber)) AndAlso (Not String.IsNullOrEmpty(vRegistrationNumber)) Then
                    .RegistrationNumber = vRegistrationNumber
                End If



                If (Not Informations.IsNothing(vBrokerAbiId)) AndAlso (Not String.IsNullOrEmpty(vBrokerAbiId)) Then
                    .BrokerAbiId = vBrokerAbiId
                End If

                'DC141204 -added expense account id


                If (Not Informations.IsNothing(vExpenseAccountId)) AndAlso (Not Object.Equals(vExpenseAccountId, Nothing)) Then


                    'Developer Guide No. 24
                    .ExpenseAccountId = vExpenseAccountId
                End If



                If (Not Informations.IsNothing(vIsInTransferMode)) AndAlso (Not vIsInTransferMode.Equals(0)) Then
                    .IsInTransferMode = vIsInTransferMode
                End If



                If (Not Informations.IsNothing(vTransferToBusinessTypeID)) AndAlso (Not vTransferToBusinessTypeID.Equals(0)) Then
                    .TransferToBusinessTypeID = vTransferToBusinessTypeID
                End If



                If (Not Informations.IsNothing(vTransferToPartyCnt)) AndAlso (Not vTransferToPartyCnt.Equals(0)) Then
                    .TransferToPartyCnt = vTransferToPartyCnt
                End If


                If (Not Informations.IsNothing(vOverrideCommission)) Then
                    .Override = vOverrideCommission
                End If

                If (Not Informations.IsNothing(vOverrideCommissionRenewal)) Then
                    .OverrideRenewal = vOverrideCommissionRenewal
                End If

                If (Not Informations.IsNothing(vDomiciledForTax)) Then
                    .DomiciledForTax = vDomiciledForTax
                End If
                'TMP


                If (Not Informations.IsNothing(vAllowConsolidate)) Then
                    .AllowConsolidate = vAllowConsolidate
                End If

                If (Not Informations.IsNothing(vReceivesClientCorrespondence)) Then
                    .ReceivesClientCorrespondence = vReceivesClientCorrespondence
                End If

                'Float Balance and Pre-Payment (RC)

                If Informations.IsArray(vParamArray) Then


                    .MakeLiveInvoice = vParamArray(AC_PARTYAG_MakeLiveInvoice)


                    .MakeLiveInstallments = vParamArray(AC_PARTYAG_MakeLiveInstallments)


                    .MakeLivePayNow = vParamArray(AC_PARTYAG_MakeLivePayNow)


                    .IsStandardAccount = vParamArray(AC_PARTYAG_IsStandardAccount)


                    .IsFloatBalanceAccount = vParamArray(AC_PARTYAG_IsFloatBalanceAccount)


                    .IsPrepaymentAccount = vParamArray(AC_PARTYAG_IsPrepaymentAccount)


                    .IsOverdraftAccount = vParamArray(AC_PARTYAG_IsOverdraftAccount)


                    .FloatBalanceLimit = vParamArray(AC_PARTYAG_FloatBalanceLimit)


                    .ExpectedDailyPremium = vParamArray(AC_PARTYAG_ExpectedDailyPremium)


                    .OverdraftLimit = vParamArray(AC_PARTYAG_OverdraftLimit)


                    .DaysAllowed = vParamArray(AC_PARTYAG_DaysAllowed)


                    .OverdraftExpiry = vParamArray(AC_PARTYAG_OverdraftExpiry)


                    .AltRefMandatory = vParamArray(AC_PARTYAG_AltRefMandatory)
                    .AltRefRequiredForEachTrans = vParamArray(AC_PARTYAG_AltRefRequiredForEachTrans)
                    .CommissionPostingType = vParamArray(AC_PARTYAG_CommissionPostingType)
                    .IsSingleInstalmentPlanOnly = vParamArray(AC_PARTYAG_IsSingleInstalmentPlanOnly)


                    .CommonRenewalDate = vParamArray(AC_PARTYAG_CommonRenewalDate)


                    .IsProduceAgentRenewalList = vParamArray(AC_PARTYAG_IsProduceAgentRenewalList)


                    .MakeLiveBankGuarantee = vParamArray(AC_PARTYAG_MakeLiveBankGuarantee)


                    .MakeLiveCashDeposit = vParamArray(AC_PARTYAG_MakeLiveCashDeposit)
                    .IsGrossAgent = vParamArray(AC_PARTYAG_MakeLiveIsGrossAgent)
                End If


                If (Not Informations.IsNothing(vBankAccount)) Then
                    .BankAccount = vBankAccount
                End If
                If (Not Informations.IsNothing(vCommissionPostingType)) AndAlso (Not vCommissionPostingType.Equals(0)) Then
                    .CommissionPostingType = vCommissionPostingType
                End If



                If (Not Informations.IsNothing(vIsSingleInstalmentPlanOnly)) AndAlso (Not vIsSingleInstalmentPlanOnly.Equals(0)) Then
                    .IsSingleInstalmentPlanOnly = vIsSingleInstalmentPlanOnly
                End If



                If (Not Informations.IsNothing(vCommonRenewalDate)) AndAlso (Not vCommonRenewalDate.Equals(DateTime.FromOADate(0))) Then
                    .CommonRenewalDate = vCommonRenewalDate
                End If

                'Batch Renewal


                If (Not Informations.IsNothing(vIsProduceAgentRenewalList)) Then
                    .IsProduceAgentRenewalList = vIsProduceAgentRenewalList
                End If
                'end change

                'SAGICOR WPR 14
                If ((Informations.IsNothing(v_lCommissionlevel) = False) And (Informations.IsNothing(v_lCommissionlevel) = False)) Then
                    .CommissionLevel = v_lCommissionlevel
                End If

                If (Not String.IsNullOrEmpty(vUniqueId)) Then
                    .UniqueId = vUniqueId
                    .ScreenHierarchy = vScreenHierarchy
                End If

                ' If we have changed one of the properties, update the status
                m_iDatabaseStatus = iStatus

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProperties", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetProperties (Public)
    '
    ' Description: Returns the supplied SIRPartyAG property values.
    '
    ' ***************************************************************** '
    ' PW110702 - add new fields required for additional details
    'DC180803 added Agent Status Id
    'DC021203 -PN8727 -fsa compliance -registration number
    'DC141204 -added expense account id
    'Developer Guide No 101
    Public Function GetProperties(ByRef iStatus As Integer, Optional ByRef vPartyCnt As Integer = 0, Optional ByRef vPartyAgentTypeID As Integer = 0,
                                  Optional ByRef vPartyAgentOriginID As Integer = 0, Optional ByRef vIsBranch As Integer = 0,
                                  Optional ByRef vIsHeadOffice As Integer = 0, Optional ByRef vAgencyAgreementDate As Object = Nothing,
                                  Optional ByRef vAgencyNextReviewDate As Object = Nothing, Optional ByRef vAgencyAccountNumber As String = "",
                                  Optional ByRef vDefaultCommissionPercent As Single = 0, Optional ByRef vTradingName As String = "",
                                  Optional ByRef vBinderIndicator As Integer = 0, Optional ByRef vReportIndicator As Integer = 0,
                                  Optional ByRef vLinkedAccountExecutiveID As Integer = 0, Optional ByRef vLinkedAccountGroup As Integer = 0,
                                  Optional ByRef vPaymentMethod As Integer = 0, Optional ByRef vPaymentFrequency As Integer = 0,
                                  Optional ByRef vAddressOnNotice As Integer = 0, Optional ByRef vTypeOfStatement As Object = Nothing,
                                  Optional ByRef vSource As Object = Nothing, Optional ByRef vTitle As String = "", Optional ByRef vMultipac As Integer = 0,
                                  Optional ByRef vContactPerson As String = "", Optional ByRef vFirstName As String = "", Optional ByRef vDateCancelled As Object = Nothing,
                                  Optional ByRef vAgentStatus As Integer = 0, Optional ByRef vRegistrationNumber As String = "",
                                  Optional ByRef vBrokerAbiId As String = "", Optional ByRef vExpenseAccountId As Object = Nothing,
                                  Optional ByRef vIsInTransferMode As Integer = 0, Optional ByRef vTransferToBusinessTypeID As Integer = 0,
                                  Optional ByRef vTransferToPartyCnt As Integer = 0, Optional ByRef vOverrideCommission As Object = Nothing, Optional ByRef vOverrideCommissionRenewal As Object = Nothing,
                                  Optional ByRef vDomiciledForTax As Boolean = False, Optional ByRef vAllowConsolidate As Boolean = False, Optional ByRef vAllowReallocationInstalmentDebt As Object = Nothing,
                                  Optional ByRef vParamArray As Object = Nothing, Optional ByRef vIsViewableOnly As Boolean = False,
                                  Optional ByRef vCommissionlevel As Object = Nothing, Optional ByRef vEDIMailBox As Object = Nothing,
                                  Optional ByRef vBankAccount As Object = Nothing, Optional ByRef vReceivesClientCorrespondence As Object = Nothing) As Integer
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set Property values.
            With m_dSIRPartyAG

                vPartyCnt = .PartyCnt
                vPartyAgentTypeID = .PartyAgentTypeID
                vPartyAgentOriginID = .PartyAgentOriginID
                vIsBranch = .IsBranch
                vIsHeadOffice = .IsHeadOffice
                vAgencyAgreementDate = .AgencyAgreementDate
                vAgencyNextReviewDate = .AgencyNextReviewDate
                vAgencyAccountNumber = .AgencyAccountNumber
                vDefaultCommissionPercent = .DefaultCommissionPercent
                vTradingName = .TradingName
                vBinderIndicator = .BinderIndicator
                vReportIndicator = .ReportIndicator
                vLinkedAccountExecutiveID = .ConsultantCnt
                vLinkedAccountGroup = .AgentGroupCnt
                vPaymentMethod = .PaymentMethod
                vPaymentFrequency = .PaymentFrequency
                vAddressOnNotice = .AddressOnNotice
                vTypeOfStatement = .TypeOfStatement
                vSource = .Source
                vTitle = .Title
                vMultipac = .Multipac
                vContactPerson = .ContactPerson
                vFirstName = .FirstName
                vDateCancelled = .DateCancelled
                vAgentStatus = .AgentStatus
                vRegistrationNumber = .RegistrationNumber
                vBrokerAbiId = .BrokerAbiId
                vExpenseAccountId = .ExpenseAccountId
                vIsInTransferMode = .IsInTransferMode
                vTransferToBusinessTypeID = .TransferToBusinessTypeID
                vTransferToPartyCnt = .TransferToPartyCnt
                vDomiciledForTax = .DomiciledForTax
                vAllowConsolidate = .AllowConsolidate
                vReceivesClientCorrespondence = .ReceivesClientCorrespondence

                'Initialize array
                ReDim vParamArray(23)
                vParamArray(AC_PARTYAG_MakeLiveInvoice) = .MakeLiveInvoice
                vParamArray(AC_PARTYAG_MakeLiveInstallments) = .MakeLiveInstallments
                vParamArray(AC_PARTYAG_MakeLivePayNow) = .MakeLivePayNow
                vParamArray(AC_PARTYAG_MakeLiveBankGuarantee) = .MakeLiveBankGuarantee ' gaurav
                vParamArray(AC_PARTYAG_IsStandardAccount) = .IsStandardAccount
                vParamArray(AC_PARTYAG_IsFloatBalanceAccount) = .IsFloatBalanceAccount
                vParamArray(AC_PARTYAG_IsPrepaymentAccount) = .IsPrepaymentAccount
                vParamArray(AC_PARTYAG_IsOverdraftAccount) = .IsOverdraftAccount
                vParamArray(AC_PARTYAG_FloatBalanceLimit) = .FloatBalanceLimit
                vParamArray(AC_PARTYAG_ExpectedDailyPremium) = .ExpectedDailyPremium
                vParamArray(AC_PARTYAG_OverdraftLimit) = .OverdraftLimit
                vParamArray(AC_PARTYAG_DaysAllowed) = .DaysAllowed
                vParamArray(AC_PARTYAG_OverdraftExpiry) = .OverdraftExpiry
                '(RC) QBENZ014
                vParamArray(AC_PARTYAG_AltRefMandatory) = .AltRefMandatory
                vParamArray(AC_PARTYAG_AltRefRequiredForEachTrans) = .AltRefRequiredForEachTrans
                '(RC) PLICO 9-10
                vParamArray(AC_PARTYAG_CommissionPostingType) = .CommissionPostingType
                vParamArray(AC_PARTYAG_IsSingleInstalmentPlanOnly) = .IsSingleInstalmentPlanOnly
                vParamArray(AC_PARTYAG_CommonRenewalDate) = .CommonRenewalDate
                'Batch Renewal
                vParamArray(AC_PARTYAG_IsProduceAgentRenewalList) = .IsProduceAgentRenewalList
                vParamArray(AC_PARTYAG_MakeLiveCashDeposit) = .MakeLiveCashDeposit
                vParamArray(AC_PARTYAG_MakeLiveIsGrossAgent) = .IsGrossAgent
                vIsViewableOnly = .IsViewableOnly

                iStatus = m_iDatabaseStatus

                vBankAccount = .BankAccount
                vCommissionlevel = .CommissionLevel
            End With

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProperties", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try

    End Function

    ' ***************************************************************** '
    ' Name: SelectItem (Public)
    '
    ' Description: Reads the Base Details from the Database .
    '
    ' ***************************************************************** '
    Public Function SelectItem() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_dSIRPartyAG

                ' Set Data object primary key
                .PartyCnt = PartyCnt

                ' Select a record from the database
                m_lReturn = .SelectSingle()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectItem", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddItem (Public)
    '
    ' Description: Adds to the Database from the Base Details.
    '
    ' ***************************************************************** '
    Public Function AddItem() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_dSIRPartyAG

                ' Set Data object primary key
                .PartyCnt = PartyCnt

                ' Add a record to the database from the object
                m_lReturn = .Add()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Retain the Primary Key of the SIRPartyAG Added
                PartyCnt = .PartyCnt

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddItem", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteItem (Public)
    '
    ' Description: Deletes a single record from the database.
    '
    '
    ' ***************************************************************** '
    Public Function DeleteItem() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_dSIRPartyAG

                ' Set Data object primary key
                .PartyCnt = PartyCnt

                ' Update the record on the database from the object
                m_lReturn = .Delete()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteItem", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateItem (Public)
    '
    ' Description: UpdateItems a single record in the database from
    '              the Base Details.
    '
    ' ***************************************************************** '
    Public Function UpdateItem() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_dSIRPartyAG

                ' Set Data object primary key
                .PartyCnt = PartyCnt

                ' Update the record on the database from the object
                m_lReturn = .Update()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateItem", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: DefaultParameters (Private)
    '
    ' Description: Sets the Default Values for a SIRPartyAG.
    '
    ' ***************************************************************** '
    ' PW100702 - add consultant/agent
    'DC021203 -PN8727 -fsa compliance -registration number
    'DC141204 -added expense account id
    'Developer Guide No. 101
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyAgentTypeID As Object = Nothing, Optional ByRef vPartyAgentOriginID As Object = Nothing, Optional ByRef vIsBranch As Object = Nothing, Optional ByRef vIsHeadOffice As Object = Nothing, Optional ByRef vAgencyAgreementDate As Object = Nothing, Optional ByRef vAgencyNextReviewDate As Object = Nothing, Optional ByRef vAgencyAccountNumber As Object = Nothing, Optional ByRef vDefaultCommissionPercent As Object = Nothing, Optional ByRef vTradingName As Object = Nothing, Optional ByRef vBinderIndicator As Object = Nothing, Optional ByRef vReportIndicator As Object = Nothing, Optional ByRef vConsultantCnt As Object = Nothing, Optional ByRef vAgentGroupCnt As Object = Nothing, Optional ByRef vPaymentMethod As Object = Nothing, Optional ByRef vPaymentFrequency As Object = Nothing, Optional ByRef vAddressOnNotice As Object = Nothing, Optional ByRef vTypeOfStatement As Object = Nothing, Optional ByRef vSource As Object = Nothing, Optional ByRef vTitle As Object = Nothing, Optional ByRef vMultipac As Object = Nothing, Optional ByRef vContactPerson As Object = Nothing, Optional ByRef vFirstName As Object = Nothing, Optional ByRef vDateCancelled As Object = Nothing, Optional ByRef vAgentStatus As Object = Nothing, Optional ByRef vRegistrationNumber As Object = Nothing, Optional ByRef vBrokerAbiId As Object = Nothing, Optional ByRef vExpenseAccountId As Object = Nothing, Optional ByRef vIsInTransferMode As Object = Nothing, Optional ByRef vTransferToBusinessTypeID As Object = Nothing, Optional ByRef vTransferToPartyCnt As Object = Nothing, Optional ByRef vOverrideCommission As Object = Nothing, Optional ByRef vOverrideCommissionRenewal As Object = Nothing, Optional ByRef vDomiciledForTax As Object = Nothing, Optional ByRef vAllowConsolidate As Object = Nothing) As Integer
        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        'developer guide no.151
        'start change
        If (Informations.IsNothing(vPartyCnt)) OrElse (vPartyCnt.Equals(0)) OrElse (bDefaultAll) Then
            vPartyCnt = 0
        End If
        'EK 27/9/99 set defaults


        If (Informations.IsNothing(vPartyAgentTypeID)) OrElse (vPartyAgentTypeID.Equals(0)) OrElse (bDefaultAll) Then
            vPartyAgentTypeID = 1
        End If



        If (Informations.IsNothing(vPartyAgentOriginID)) OrElse (vPartyAgentOriginID.Equals(0)) OrElse (bDefaultAll) Then
            vPartyAgentOriginID = 1
        End If



        If (Informations.IsNothing(vIsBranch)) OrElse (vIsBranch.Equals(0)) OrElse (bDefaultAll) Then
            vIsBranch = 1
        End If



        If (Informations.IsNothing(vIsHeadOffice)) OrElse (vIsHeadOffice.Equals(0)) OrElse (bDefaultAll) Then
            vIsHeadOffice = 0
        End If



        If (Informations.IsNothing(vAgencyAgreementDate)) OrElse (vAgencyAgreementDate.Equals(DateTime.FromOADate(0))) OrElse (bDefaultAll) Then
            vAgencyAgreementDate = DateTime.Now
        End If



        If (Informations.IsNothing(vAgencyNextReviewDate)) OrElse (vAgencyNextReviewDate.Equals(DateTime.FromOADate(0))) OrElse (bDefaultAll) Then
            vAgencyNextReviewDate = DateTime.Now
        End If



        If (Informations.IsNothing(vDefaultCommissionPercent)) OrElse (vDefaultCommissionPercent.Equals(0)) OrElse (bDefaultAll) Then
            vDefaultCommissionPercent = 10
        End If



        If (Informations.IsNothing(vConsultantCnt)) OrElse (vConsultantCnt.Equals(0)) OrElse (bDefaultAll) Then
            vConsultantCnt = 0
        End If



        If (Informations.IsNothing(vAgentGroupCnt)) OrElse (vAgentGroupCnt.Equals(0)) OrElse (bDefaultAll) Then
            vAgentGroupCnt = 0
        End If



        If (Informations.IsNothing(vPaymentMethod)) OrElse (vPaymentMethod.Equals(0)) OrElse (bDefaultAll) Then
            vPaymentMethod = 0
        End If



        If (Informations.IsNothing(vPaymentFrequency)) OrElse (vPaymentFrequency.Equals(0)) OrElse (bDefaultAll) Then
            vPaymentFrequency = 0
        End If



        If (Informations.IsNothing(vAddressOnNotice)) OrElse (vAddressOnNotice.Equals(0)) OrElse (bDefaultAll) Then
            vAddressOnNotice = 0
        End If



        If (Informations.IsNothing(vTypeOfStatement)) OrElse (vTypeOfStatement.Equals(0)) OrElse (bDefaultAll) Then
            vTypeOfStatement = 0
        End If



        If (Informations.IsNothing(vSource)) OrElse (vSource.Equals(0)) OrElse (bDefaultAll) Then
            vSource = 0
        End If



        If (Informations.IsNothing(vTitle)) OrElse (vTitle.Equals(0)) OrElse (bDefaultAll) Then
            vTitle = 0
        End If



        If (Informations.IsNothing(vMultipac)) OrElse (vMultipac.Equals(0)) OrElse (bDefaultAll) Then
            vMultipac = 0
        End If



        If (Informations.IsNothing(vContactPerson)) OrElse (String.IsNullOrEmpty(vContactPerson)) OrElse (bDefaultAll) Then
            vContactPerson = ""
        End If



        If (Informations.IsNothing(vFirstName)) OrElse (String.IsNullOrEmpty(vFirstName)) OrElse (bDefaultAll) Then
            vFirstName = ""
        End If

        '    If ((IsMissing(vBankAccount) = True) _
        ''    Or (IsEmpty(vBankAccount) = True) _
        ''    Or (bDefaultAll = True)) Then
        '        vBankAccount = ""
        '    End If

        If (Informations.IsNothing(vDateCancelled)) OrElse (vDateCancelled.Equals(0)) OrElse (bDefaultAll) Then
            ' vDateCancelled = 0
            vDateCancelled = #12/29/1899#
        End If

        'PN12253
        'DC180803
        '    If ((IsMissing(vAgentStatus) = True) _
        ''    Or (IsEmpty(vAgentStatus) = True) _
        ''    Or (bDefaultAll = True)) Then
        '        vAgentStatus = 0
        '    End If
        If (Informations.IsNothing(vAgentStatus)) OrElse (Object.Equals(vAgentStatus, Nothing)) OrElse (bDefaultAll) Then


            vAgentStatus = DBNull.Value
        End If

        'PN12253End

        'DC021203 -PN8727 -fsa compliance -registration number


        If (Informations.IsNothing(vRegistrationNumber)) OrElse (String.IsNullOrEmpty(vRegistrationNumber)) OrElse (bDefaultAll) Then
            vRegistrationNumber = ""
        End If



        If (Informations.IsNothing(vBrokerAbiId)) OrElse (String.IsNullOrEmpty(vBrokerAbiId)) OrElse (bDefaultAll) Then
            vBrokerAbiId = ""
        End If

        'DC141204 -added expense account id


        If (Informations.IsNothing(vExpenseAccountId)) OrElse (Object.Equals(vExpenseAccountId, Nothing)) OrElse (bDefaultAll) Then


            vExpenseAccountId = DBNull.Value
        End If



        If (Informations.IsNothing(vIsInTransferMode)) OrElse (vIsInTransferMode.Equals(0)) OrElse (bDefaultAll) Then
            vIsInTransferMode = 0
        End If



        If (Informations.IsNothing(vTransferToBusinessTypeID)) OrElse (vTransferToBusinessTypeID.Equals(0)) OrElse (bDefaultAll) Then
            vTransferToBusinessTypeID = 0
        End If



        If (Informations.IsNothing(vTransferToPartyCnt)) OrElse (vTransferToPartyCnt.Equals(0)) OrElse (bDefaultAll) Then
            vTransferToPartyCnt = 0
        End If



        If Informations.IsNothing(vDomiciledForTax) OrElse vDomiciledForTax.Equals(False) OrElse bDefaultAll Then
            vDomiciledForTax = False
        End If

        Return result


    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the SIRPartyAG for Consistency.

    '
    ' ***************************************************************** '
    ' PW100702 - add consultant/agent
    Private Function Validate(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyAgentTypeID As Object = Nothing, Optional ByRef vPartyAgentOriginID As Object = Nothing, Optional ByRef vIsBranch As Object = Nothing, Optional ByRef vIsHeadOffice As Object = Nothing, Optional ByRef vAgencyAgreementDate As Object = Nothing, Optional ByRef vAgencyNextReviewDate As Object = Nothing, Optional ByRef vAgencyAccountNumber As Object = Nothing, Optional ByRef vDefaultCommissionPercent As Object = Nothing, Optional ByRef vTradingName As Object = Nothing, Optional ByRef vBinderIndicator As Object = Nothing, Optional ByRef vReportIndicator As Object = Nothing, Optional ByRef vConsultantCnt As Object = Nothing, Optional ByRef vAgentGroupCnt As Object = Nothing, Optional ByRef vDateCancelled As Object = Nothing, Optional ByRef vIsInTransferMode As Object = Nothing, Optional ByRef vTransferToBusinessTypeID As Object = Nothing, Optional ByRef vTransferToPartyCnt As Object = Nothing) As Integer

        Dim result As Integer = 0




        result = gPMConstants.PMEReturnCode.PMTrue

        ' Validate

        ' {* USER DEFINED CODE (Begin) *}


        If Not Informations.IsNothing(vPartyCnt) Then

            Dim dbNumericTemp As Double
            If Not Double.TryParse(CStr(vPartyCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vPartyAgentOriginID) Then

            Dim dbNumericTemp2 As Double
            If Not Double.TryParse(CStr(vPartyAgentOriginID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vAgencyAgreementDate) Then
            If Not Informations.IsDate(vAgencyAgreementDate) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vAgencyNextReviewDate) Then
            If Not Informations.IsDate(vAgencyNextReviewDate) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vDateCancelled) Then
            If Not Informations.IsDate(vDateCancelled) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vIsInTransferMode) Then

            Dim dbNumericTemp3 As Double
            If Not Double.TryParse(CStr(vIsInTransferMode), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vTransferToBusinessTypeID) Then

            Dim dbNumericTemp4 As Double
            If Not Double.TryParse(CStr(vTransferToBusinessTypeID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vTransferToPartyCnt) Then

            Dim dbNumericTemp5 As Double
            If Not Double.TryParse(CStr(vTransferToPartyCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function
    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        ' Class Initialise


        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
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

