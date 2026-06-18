Option Strict Off
Option Explicit On

Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
'developer guide no. 129
Imports SSP.Shared
Friend NotInheritable Class SIRPartyIN
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: SIRPartyIN
    '
    ' Date: 25/06/1999
    '
    ' Description: Describes the SIRPartyIN attributes.
    '
    ' Edit History:
    ' RAW 18/12/2002 : PS187 : Added new data items (WHTaxType, WHTaxRate, TaxRegNo, TaxCode, PaymentMethod, PaymentFrequency , BankAccount)
    '                          Report validation errors correctly
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
    Private Const ACClass As String = "SIRPartyIN"

    ' ************************************************
    ' Added to replace global variables 09/02/2004



    ' ************************************************


    ' PRIVATE Data Members (Begin)

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' Instance of Data component
    Private m_dSIRPartyIN As dSIRPartyIN.SIRPartyIN ' was dSIRPartyIN.SIRPartyIN

    ' Instance of the Core SIRClaim object
    'Private m_bSIRParty As bSIRParty.Business
    Private m_bSIRParty As bSIRParty.Business

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
    Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUserName
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel



            ' Create instance of data class
            m_dSIRPartyIN = New dSIRPartyIN.SIRPartyIN()
            '    Set m_dSIRPartyIN = New dSIRPartyIN.SIRPartyIN

            m_lReturn = m_dSIRPartyIN.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=vDatabase)

            ' Create Core Business Object
            '    Set m_bSIRParty = New bSIRParty.Business




            m_bSIRParty = New bSIRParty.Business
            m_lReturn = m_bSIRParty.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=vDatabase)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
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
            Me.disposedValue = True
            If disposing Then
                If m_dSIRPartyIN IsNot Nothing Then
                    m_dSIRPartyIN.Dispose()
                End If
                m_dSIRPartyIN = Nothing
                If m_bSIRParty IsNot Nothing Then
                    m_bSIRParty.Dispose()
                End If
                m_bSIRParty = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetDefaults (Public)
    '
    ' Description: Returns the Default Values for the SIRPartyIN.
    '
    ' ***************************************************************** '
    ' RAW 18/12/2002 : PS187 : Added WH TaxType, WH Tax Rate, Tax Reg No, Tax Code & PaymentMethod, PaymentFrequency, BankAccount
    'DC150803 -PS254 -fsa compliance
    'ECK Datasure 10102005 Claims Rating Agency
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vAgencyNumber As Object = Nothing, Optional ByRef vBinderIndicator As Object = Nothing, Optional ByRef vReportIndicator As Object = Nothing, Optional ByRef vIsReinsurer As Object = Nothing, Optional ByRef vReinsuranceType As Object = Nothing, Optional ByRef vIsReinsuranceDebitCreditNo As Object = Nothing, Optional ByRef vDefaultCommRate As Object = Nothing, Optional ByRef vABICodeOn81 As Object = Nothing, Optional ByRef vTaxGroupID As Object = Nothing, Optional ByRef vPaymentMethod As Object = Nothing, Optional ByRef vPaymentFrequency As Object = Nothing, Optional ByRef vBankAccount As Object = Nothing, Optional ByRef vFSAInsurerStatus As Object = Nothing, Optional ByRef vFSARegistrationNumber As Object = Nothing, Optional ByRef vFSAInsurerCreditRating As Object = Nothing, Optional ByRef vIsRetained As Object = Nothing, Optional ByRef vClaimsRatingAgencyId As Object = Nothing, Optional ByRef vClaimsRatingGrading As Object = Nothing, Optional ByRef vClaimsRatingDate As Object = Nothing, Optional ByRef vClaimsRatingDescription As Object = Nothing, Optional ByRef vTermsOfPaymentId As Object = Nothing, Optional ByRef vDomiciledForTax As Object = Nothing, Optional ByRef vBrokerlinkSubaccount As Object = Nothing, Optional ByRef vBrokerlinkUnderwritingid As Object = Nothing, Optional ByRef vIsRIBroker As Object = Nothing, Optional ByRef vRiskTransferEditable As Object = Nothing, Optional ByRef vInsurerTypeId As Object = Nothing, Optional ByRef vBureauAccountParty As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults
            ' RAW 18/12/2002 : PS187 : Added WH TaxType, WH Tax Rate, Tax Reg No, Tax Code & PaymentMethod, PaymentFrequency, BankAccount
            'DC150803 -PS254 -fsa compliance

            'developer guide no.98
            m_lReturn = DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, _
            vPartyCnt:=vPartyCnt, _
            vAgencyNumber:=vAgencyNumber, _
            vBinderIndicator:=vBinderIndicator, _
            vReportIndicator:=vReportIndicator, _
            vIsReinsurer:=vIsReinsurer, _
            vReinsuranceType:=vReinsuranceType, _
            vIsReinsuranceDebitCreditNo:=vIsReinsuranceDebitCreditNo, _
            vDefaultCommRate:=vDefaultCommRate, _
            vTaxGroupID:=vTaxGroupID, _
            vPaymentMethod:=vPaymentMethod, _
            vPaymentFrequency:=vPaymentFrequency, _
            vBankAccount:=vBankAccount, _
            vFSAInsurerStatus:=vFSAInsurerStatus, _
            vFSARegistrationNumber:=vFSARegistrationNumber, _
            vFSAInsurerCreditRating:=vFSAInsurerCreditRating, _
            vIsRetained:=vIsRetained, _
            vClaimsRatingAgencyId:=vClaimsRatingAgencyId, _
            vClaimsRatingGrading:=vClaimsRatingGrading, _
            vClaimsRatingDate:=vClaimsRatingDate, _
            vClaimsRatingDescription:=vClaimsRatingDescription, _
            vTermsOfPaymentId:=vTermsOfPaymentId, _
            vDomiciledForTax:=vDomiciledForTax, _
            vBrokerlinkSubaccount:=vBrokerlinkSubaccount, _
            vBrokerlinkUnderwritingid:=vBrokerlinkUnderwritingid, vIsRIBroker:=vIsRIBroker)


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
    ' Description: Sets the supplied SIRPartyIN property values.
    '
    ' ***************************************************************** '
    ' RAW 18/12/2002 : PS187 : Added WH TaxType, WH Tax Rate, Tax Reg No, Tax Code & PaymentMethod, PaymentFrequency, BankAccount
    'DC150803 -PS254 -fsa compliance
    'ECK Datasure 10102005 Claims Rating Agency
    'developer guide no.33
    Public Function SetProperties(ByRef iStatus As Integer, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vAgencyNumber As Object = Nothing, Optional ByRef vBinderIndicator As Object = Nothing, Optional ByRef vReportIndicator As Object = Nothing, Optional ByRef vIsReinsurer As Object = Nothing, Optional ByRef vReinsuranceType As Object = Nothing, Optional ByRef vIsReinsuranceDebitCreditNo As Object = Nothing, Optional ByRef vDefaultCommRate As Object = Nothing, Optional ByRef vTaxGroupID As Object = Nothing, Optional ByRef vPaymentMethod As Object = Nothing, Optional ByRef vPaymentFrequency As Object = Nothing, Optional ByRef vBankAccount As Object = Nothing, Optional ByRef vFSAInsurerStatus As Object = Nothing, Optional ByRef vFSARegistrationNumber As Object = Nothing, Optional ByRef vFSAInsurerCreditRating As Object = Nothing, Optional ByRef vIsRetained As Object = Nothing, Optional ByRef vClaimsRatingAgencyId As Object = Nothing, Optional ByRef vClaimsRatingGrading As Object = Nothing, Optional ByRef vClaimsRatingDate As Object = Nothing, Optional ByRef vClaimsRatingDescription As Object = Nothing, Optional ByRef vTermsOfPaymentId As Object = Nothing, Optional ByRef vDomiciledForTax As Object = Nothing, Optional ByRef vRiskTransferAgreement As Object = Nothing, Optional ByRef vBrokerlinkSubaccount As Object = Nothing, Optional ByRef vBrokerlinkUnderwritingid As Object = Nothing, Optional ByRef vIsRIBroker As Object = Nothing, Optional ByRef vCboLockingTypeId As Object = Nothing, Optional ByRef vRiskTransferEditable As Object = Nothing, Optional ByRef vInsurerTypeId As Object = Nothing, Optional ByRef vBureauAccountParty As Object = Nothing, Optional ByVal sUniqueId As String = "", Optional ByVal sScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If Add Mode
            If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

                ' Default Any Missing Parameters
                ' RAW 18/12/2002 : PS187 : Added WH TaxType, WH Tax Rate, Tax Reg No, Tax Code & PaymentMethod, PaymentFrequency, BankAccount
                'DC150803 -PS254 -fsa compliance
                'developer guide no.98
                m_lReturn = DefaultParameters(bDefaultAll:=False,
                vPartyCnt:=vPartyCnt,
                vAgencyNumber:=vAgencyNumber,
                vBinderIndicator:=vBinderIndicator,
                vReportIndicator:=vReportIndicator,
                vIsReinsurer:=vIsReinsurer,
                vReinsuranceType:=vReinsuranceType,
                vIsReinsuranceDebitCreditNo:=vIsReinsuranceDebitCreditNo,
                vDefaultCommRate:=vDefaultCommRate,
                vTaxGroupID:=vTaxGroupID,
                vPaymentMethod:=vPaymentMethod,
                vPaymentFrequency:=vPaymentFrequency,
                vBankAccount:=vBankAccount,
                vFSAInsurerStatus:=vFSAInsurerStatus,
                vFSARegistrationNumber:=vFSARegistrationNumber,
                vFSAInsurerCreditRating:=vFSAInsurerCreditRating,
                vIsRetained:=vIsRetained,
                vDomiciledForTax:=vDomiciledForTax,
                vRiskTransferAgreement:=vRiskTransferAgreement, vIsRIBroker:=vIsRIBroker,
                vCboLockingTypeId:=vCboLockingTypeId,
                vRiskTransferEditable:=vRiskTransferEditable,
                vInsurerTypeId:=vInsurerTypeId, vBureauAccountParty:=vBureauAccountParty)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

            End If

            ' Validate Parameters
            ' RAW 18/12/2002 : PS187 : Added WH TaxType, WH Tax Rate, Tax Reg No, Tax Code & PaymentMethod, PaymentFrequency, BankAccount
            'DC150803 -PS254 -fsa compliance
            m_lReturn = CType(Validate(vPartyCnt:=vPartyCnt, vAgencyNumber:=vAgencyNumber, vBinderIndicator:=vBinderIndicator, vReportIndicator:=vReportIndicator, vIsReinsurer:=vIsReinsurer, vReinsuranceType:=vReinsuranceType, vIsReinsuranceDebitCreditNo:=vIsReinsuranceDebitCreditNo, vDefaultCommRate:=vDefaultCommRate, vTaxGroupID:=vTaxGroupID, vPaymentMethod:=vPaymentMethod, vPaymentFrequency:=vPaymentFrequency, vBankAccount:=vBankAccount, vIsRetained:=vIsRetained, vRiskTransferAgreement:=vRiskTransferAgreement, vCboLockingTypeId:=vCboLockingTypeId), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Set Data Changed Flag to False
            bDataChanged = False

            ' Set Property values.
            With m_dSIRPartyIN

                If (Not Informations.IsNothing(vPartyCnt)) AndAlso (Not vPartyCnt.Equals(0)) Then
                    .PartyCnt = vPartyCnt
                End If


                If (Not Informations.IsNothing(vAgencyNumber)) AndAlso (Not Object.Equals(vAgencyNumber, Nothing)) Then


                    'developer guide no. 24 (Latest Guide)
                    .AgencyNumber = vAgencyNumber
                End If




                If (Not Informations.IsNothing(vBinderIndicator)) AndAlso (Not Object.Equals(vBinderIndicator, Nothing)) Then


                    'developer guide no. 24 (Latest Guide)
                    .BinderIndicator = vBinderIndicator
                End If



                If (Not Informations.IsNothing(vReportIndicator)) AndAlso (Not Object.Equals(vReportIndicator, Nothing)) Then


                    'developer guide no. 24 (Latest Guide)
                    .ReportIndicator = vReportIndicator
                End If



                If (Not Informations.IsNothing(vIsReinsurer)) AndAlso (Not vIsReinsurer.Equals(0)) Then
                    .IsReinsurer = vIsReinsurer
                End If



                If (Not Informations.IsNothing(vReinsuranceType)) AndAlso (Not Object.Equals(vReinsuranceType, Nothing)) Then


                    'developer guide no. 24 (Latest Guide)
                    .ReinsuranceType = vReinsuranceType
                End If



                If (Not Informations.IsNothing(vIsReinsuranceDebitCreditNo)) Then
                    .IsReinsuranceDebitCreditNo = vIsReinsuranceDebitCreditNo
                End If



                If (Not Informations.IsNothing(vDefaultCommRate)) AndAlso (Not Object.Equals(vDefaultCommRate, Nothing)) Then


                    'developer guide no. 24 (Latest Guide)
                    .DefaultCommRate = vDefaultCommRate
                End If



                If (Not Informations.IsNothing(vTaxGroupID)) AndAlso (Not vTaxGroupID.Equals(0)) Then
                    .TaxGroupID = vTaxGroupID
                End If



                If (Not Informations.IsNothing(vPaymentMethod)) AndAlso (Not vPaymentMethod.Equals(0)) Then
                    .PaymentMethod = vPaymentMethod
                End If



                If (Not Informations.IsNothing(vPaymentFrequency)) AndAlso (Not vPaymentFrequency.Equals(0)) Then
                    .PaymentFrequency = vPaymentFrequency
                End If



                If (Not Informations.IsNothing(vBankAccount)) AndAlso (Not String.IsNullOrEmpty(vBankAccount)) Then
                    .BankAccount = vBankAccount
                End If

                ' RAW 18/12/2002 : PS187 : end

                'DC150803 -PS254 -fsa compliance


                If (Not Informations.IsNothing(vFSAInsurerStatus)) AndAlso (Not vFSAInsurerStatus.Equals(0)) Then
                    .FSAInsurerStatus = vFSAInsurerStatus
                End If



                If (Not Informations.IsNothing(vFSARegistrationNumber)) AndAlso (Not String.IsNullOrEmpty(vFSARegistrationNumber)) Then
                    .FSARegistrationNumber = vFSARegistrationNumber
                End If



                If (Not Informations.IsNothing(vFSAInsurerCreditRating)) AndAlso (Not vFSAInsurerCreditRating.Equals(0)) Then
                    .FSAInsurerCreditRating = vFSAInsurerCreditRating
                End If



                If (Not Informations.IsNothing(vIsRetained)) Then
                    .IsRetained = vIsRetained
                End If



                If (Not Informations.IsNothing(vClaimsRatingAgencyId)) AndAlso (Not vClaimsRatingAgencyId.Equals(0)) Then
                    .ClaimsRatingAgencyId = vClaimsRatingAgencyId
                End If



                If (Not Informations.IsNothing(vClaimsRatingGrading)) AndAlso (Not String.IsNullOrEmpty(vClaimsRatingGrading)) Then
                    .ClaimsRatingGrading = vClaimsRatingGrading
                End If



                If (Not Informations.IsNothing(vClaimsRatingDate)) AndAlso (Not vClaimsRatingDate.Equals(DateTime.FromOADate(0))) Then
                    .ClaimsRatingDate = vClaimsRatingDate
                End If



                If (Not Informations.IsNothing(vClaimsRatingDescription)) AndAlso (Not vClaimsRatingDate.Equals(DateTime.FromOADate(0))) Then
                    .ClaimsRatingDescription = vClaimsRatingDescription
                End If



                If (Not Informations.IsNothing(vTermsOfPaymentId)) AndAlso (Not vTermsOfPaymentId.Equals(0)) Then
                    .TermsOfPaymentId = vTermsOfPaymentId
                End If



                If (Not Informations.IsNothing(vDomiciledForTax)) AndAlso (Not vDomiciledForTax.Equals(False)) Then
                    .DomiciledForTax = vDomiciledForTax
                End If



                If (Not Informations.IsNothing(vRiskTransferAgreement)) AndAlso (Not vRiskTransferAgreement.Equals(False)) Then
                    .RiskTransferAgreement = vRiskTransferAgreement
                End If



                If (Not Informations.IsNothing(vBrokerlinkSubaccount)) AndAlso (Not Object.Equals(vBrokerlinkSubaccount, Nothing)) Then


                    'developer guide no. 24 (Latest Guide)
                    .BrokerlinkSubAccount = vBrokerlinkSubaccount
                End If



                If (Not Informations.IsNothing(vBrokerlinkUnderwritingid)) AndAlso (Not Object.Equals(vBrokerlinkUnderwritingid, Nothing)) Then


                    'developer guide no. 24 (Latest Guide)
                    .BrokerlinkUnderwritingID = vBrokerlinkUnderwritingid
                End If


                If (Not Informations.IsNothing(vIsRIBroker)) Then
                    .IsRIBroker = vIsRIBroker
                End If

                'Devlopment Work on Insurer Payment Locking


                If (Not Informations.IsNothing(vCboLockingTypeId)) AndAlso (Not vCboLockingTypeId.Equals(0)) Then
                    .CboLockingTypeId = vCboLockingTypeId
                End If



                If (Not Informations.IsNothing(vRiskTransferEditable)) AndAlso (Not vRiskTransferEditable.Equals(0)) Then
                    .RiskTransferEditable = vRiskTransferEditable
                End If
                'PVY Development


                If (Not Informations.IsNothing(vInsurerTypeId)) AndAlso (Not vInsurerTypeId.Equals(0)) Then
                    .InsurerTypeId = vInsurerTypeId
                End If


                If (Not Informations.IsNothing(vBureauAccountParty)) AndAlso (Not vBureauAccountParty.Equals(0)) Then
                    .BureauAccountParty = vBureauAccountParty
                End If

                If (Not String.IsNullOrEmpty(sUniqueId)) AndAlso (Not String.IsNullOrEmpty(sScreenHierarchy)) Then
                    .UniqueId = sUniqueId
                    .ScreenHierarchy = sScreenHierarchy
                End If
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
    ' Description: Returns the supplied SIRPartyIN property values.
    '
    ' ***************************************************************** '
    ' RAW 18/12/2002 : PS187 : Added WH TaxType, WH Tax Rate, Tax Reg No, Tax Code & PaymentMethod, PaymentFrequency, BankAccount
    'DC150803 -PS254 -fsa compliance
    'ECK Datasure 10102005 Claims Rating Agency
    'developer guide no.33
    Public Function GetProperties(ByRef iStatus As Integer, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vAgencyNumber As Object = Nothing, Optional ByRef vBinderIndicator As Object = Nothing, Optional ByRef vReportIndicator As Object = Nothing, Optional ByRef vIsReinsurer As Object = Nothing, Optional ByRef vReinsuranceType As Object = Nothing, Optional ByRef vIsReinsuranceDebitCreditNo As Object = Nothing, Optional ByRef vDefaultCommRate As Object = Nothing, Optional ByRef vTaxGroupID As Object = Nothing, Optional ByRef vPaymentMethod As Object = Nothing, Optional ByRef vPaymentFrequency As Object = Nothing, Optional ByRef vBankAccount As Object = Nothing, Optional ByRef vFSAInsurerStatus As Object = Nothing, Optional ByRef vFSARegistrationNumber As Object = Nothing, Optional ByRef vFSAInsurerCreditRating As Object = Nothing, Optional ByRef vIsRetained As Object = Nothing, Optional ByRef vClaimsRatingAgencyId As Object = Nothing, Optional ByRef vClaimsRatingGrading As Object = Nothing, Optional ByRef vClaimsRatingDate As Object = Nothing, Optional ByRef vClaimsRatingDescription As Object = Nothing, Optional ByRef vTermsOfPaymentId As Object = Nothing, Optional ByRef vDomiciledForTax As Object = Nothing, Optional ByRef vRiskTransferAgreement As Object = Nothing, Optional ByRef vBrokerlinkSubaccount As Object = Nothing, Optional ByRef vBrokerlinkUnderwritingid As Object = Nothing, Optional ByRef vIsRIBroker As Object = Nothing, Optional ByRef vCboLockingTypeId As Object = Nothing, Optional ByRef vRiskTransferEditable As Object = Nothing, Optional ByRef vInsurerTypeId As Object = Nothing, Optional ByRef vBureauAccountParty As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set Property values.
            With m_dSIRPartyIN


                'developer guide no. 118
                vPartyCnt = .PartyCnt

                vAgencyNumber = .AgencyNumber

                vBinderIndicator = .BinderIndicator


                vReportIndicator = .ReportIndicator

                vIsReinsurer = .IsReinsurer


                vReinsuranceType = .ReinsuranceType

                vIsReinsuranceDebitCreditNo = .IsReinsuranceDebitCreditNo


                vDefaultCommRate = .DefaultCommRate

                vTaxGroupID = .TaxGroupID

                vPaymentMethod = .PaymentMethod

                vPaymentFrequency = .PaymentFrequency

                vBankAccount = .BankAccount
                ' RAW 18/12/2002 : PS187 : end

                'DC150803 -PS254 -fsa compliance

                vFSAInsurerStatus = .FSAInsurerStatus

                vFSAInsurerCreditRating = .FSAInsurerCreditRating

                vFSARegistrationNumber = .FSARegistrationNumber

                vIsRetained = .IsRetained

                vClaimsRatingAgencyId = .ClaimsRatingAgencyId

                vClaimsRatingGrading = .ClaimsRatingGrading

                vClaimsRatingDate = .ClaimsRatingDate

                vClaimsRatingDescription = .ClaimsRatingDescription

                vTermsOfPaymentId = .TermsOfPaymentId

                vDomiciledForTax = .DomiciledForTax

                vRiskTransferAgreement = .RiskTransferAgreement

                vBrokerlinkSubaccount = .BrokerlinkSubAccount

                vBrokerlinkUnderwritingid = .BrokerlinkUnderwritingID


                'QBENZ005

                vIsRIBroker = .IsRIBroker

                vCboLockingTypeId = .CboLockingTypeId

                vRiskTransferEditable = .RiskTransferEditable

                'PVY Development

                vInsurerTypeId = .InsurerTypeId

                vBureauAccountParty = .BureauAccountParty

                iStatus = m_iDatabaseStatus

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
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

            With m_dSIRPartyIN

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

            With m_dSIRPartyIN

                ' Set Data object primary key
                .PartyCnt = PartyCnt
                ' Add a record to the database from the object
                m_lReturn = .Add()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Retain the Primary Key of the SIRPartyIN Added
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

            With m_dSIRPartyIN

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

            With m_dSIRPartyIN

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
    ' Description: Sets the Default Values for a SIRPartyIN.
    '
    ' ***************************************************************** '
    ' RAW 18/12/2002 : PS187 : Added WH TaxType, WH Tax Rate, Tax Reg No, Tax Code & PaymentMethod, PaymentFrequency, BankAccount
    'ECK Datasure 10102005 Claims Rating Agency
    'developer guide no.33
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vAgencyNumber As Object = Nothing, Optional ByRef vBinderIndicator As Object = Nothing, Optional ByRef vReportIndicator As Object = Nothing, Optional ByRef vIsReinsurer As Object = Nothing, Optional ByRef vReinsuranceType As Object = Nothing, Optional ByRef vIsReinsuranceDebitCreditNo As Object = Nothing, Optional ByRef vDefaultCommRate As Object = Nothing, Optional ByRef vTaxGroupID As Object = Nothing, Optional ByRef vPaymentMethod As Object = Nothing, Optional ByRef vPaymentFrequency As Object = Nothing, Optional ByRef vBankAccount As Object = Nothing, Optional ByRef vFSAInsurerStatus As Object = Nothing, Optional ByRef vFSARegistrationNumber As Object = Nothing, Optional ByRef vFSAInsurerCreditRating As Object = Nothing, Optional ByRef vIsRetained As Object = Nothing, Optional ByRef vClaimsRatingAgencyId As Object = Nothing, Optional ByRef vClaimsRatingGrading As Object = Nothing, Optional ByRef vClaimsRatingDate As Object = Nothing, Optional ByRef vClaimsRatingDescription As Object = Nothing, Optional ByRef vTermsOfPaymentId As Object = Nothing, Optional ByRef vDomiciledForTax As Object = Nothing, Optional ByRef vRiskTransferAgreement As Object = Nothing, Optional ByRef vBrokerlinkSubaccount As Object = Nothing, Optional ByRef vBrokerlinkUnderwritingid As Object = Nothing, Optional ByRef vIsRIBroker As Object = Nothing, Optional ByRef vCboLockingTypeId As Object = Nothing, Optional ByRef vRiskTransferEditable As Object = Nothing, Optional ByRef vInsurerTypeId As Object = Nothing, Optional ByRef vBureauAccountParty As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}
        'developer guide no.151
        If (Informations.IsNothing(vPartyCnt)) OrElse (vPartyCnt.Equals(0)) OrElse (bDefaultAll) Then
            vPartyCnt = 0
        End If



        If (Informations.IsNothing(vAgencyNumber)) OrElse (String.IsNullOrEmpty(vAgencyNumber)) OrElse (bDefaultAll) Then
            vAgencyNumber = ""
        End If



        If (Informations.IsNothing(vBinderIndicator)) OrElse (vBinderIndicator.Equals(0)) OrElse (bDefaultAll) Then
            vBinderIndicator = 0
        End If



        If (Informations.IsNothing(vReportIndicator)) OrElse (vReportIndicator.Equals(0)) OrElse (bDefaultAll) Then
            vReportIndicator = 0
        End If



        If (Informations.IsNothing(vIsReinsurer)) OrElse (vIsReinsurer.Equals(0)) OrElse (bDefaultAll) Then
            vIsReinsurer = 0
        End If



        If (Informations.IsNothing(vReinsuranceType)) OrElse (vReinsuranceType.Equals(0)) OrElse (bDefaultAll) Then
            vReinsuranceType = 0
        End If



        If (Informations.IsNothing(vIsReinsuranceDebitCreditNo)) OrElse (vIsReinsuranceDebitCreditNo.Equals(0)) OrElse (bDefaultAll) Then
            vIsReinsuranceDebitCreditNo = 0
        End If



        If (Informations.IsNothing(vDefaultCommRate)) OrElse (vDefaultCommRate.Equals(0)) OrElse (bDefaultAll) Then
            vDefaultCommRate = 0
        End If

        ' RAW 18/12/2002 : PS187 : added


        If (Informations.IsNothing(vTaxGroupID)) OrElse (vTaxGroupID.Equals(0)) OrElse (bDefaultAll) Then
            vTaxGroupID = 0
        End If



        If (Informations.IsNothing(vPaymentMethod)) OrElse (vPaymentMethod.Equals(0)) OrElse (bDefaultAll) Then
            vPaymentMethod = -1
        End If



        If (Informations.IsNothing(vPaymentFrequency)) OrElse (vPaymentFrequency.Equals(0)) OrElse (bDefaultAll) Then
            vPaymentFrequency = -1
        End If



        If (Informations.IsNothing(vBankAccount)) OrElse (String.IsNullOrEmpty(vBankAccount)) OrElse (bDefaultAll) Then
            vBankAccount = ""
        End If

        ' RAW 18/12/2002 : PS187 : end

        'DC150803 -PS254 -fsa compliance


        If (Informations.IsNothing(vFSAInsurerStatus)) OrElse (vFSAInsurerStatus.Equals(0)) OrElse (bDefaultAll) Then
            vFSAInsurerStatus = -1
        End If



        If (Informations.IsNothing(vFSARegistrationNumber)) OrElse (String.IsNullOrEmpty(vFSARegistrationNumber)) OrElse (bDefaultAll) Then
            vFSARegistrationNumber = ""
        End If



        If (Informations.IsNothing(vFSAInsurerCreditRating)) OrElse (vFSAInsurerCreditRating.Equals(0)) OrElse (bDefaultAll) Then
            vFSAInsurerCreditRating = -1
        End If



        If (Informations.IsNothing(vIsRetained)) OrElse (vIsRetained.Equals(False)) OrElse (bDefaultAll) Then
            vIsRetained = False
        End If



        If (Informations.IsNothing(vClaimsRatingAgencyId)) OrElse (vClaimsRatingAgencyId.Equals(0)) OrElse (bDefaultAll) Then
            vClaimsRatingAgencyId = 0
        End If



        If (Informations.IsNothing(vClaimsRatingGrading)) OrElse (String.IsNullOrEmpty(vClaimsRatingGrading)) OrElse (bDefaultAll) Then
            vClaimsRatingGrading = ""
        End If



        If (Informations.IsNothing(vClaimsRatingDate)) OrElse (vClaimsRatingDate.Equals(DateTime.FromOADate(0))) OrElse (bDefaultAll) Then
            vClaimsRatingDate = DateTime.Now
        End If

        'S4BDAT005


        If Informations.IsNothing(vClaimsRatingDescription) OrElse String.IsNullOrEmpty(vClaimsRatingDescription) OrElse bDefaultAll Then
            vClaimsRatingDescription = ""
        End If
        'S4BDAT004


        If Informations.IsNothing(vTermsOfPaymentId) OrElse vTermsOfPaymentId.Equals(0) OrElse bDefaultAll Then
            vTermsOfPaymentId = 0
        End If



        If Informations.IsNothing(vDomiciledForTax) OrElse vDomiciledForTax.Equals(0) OrElse bDefaultAll Then
            vDomiciledForTax = 0
        End If



        If (Informations.IsNothing(vRiskTransferAgreement)) OrElse (vRiskTransferAgreement.Equals(False)) OrElse (bDefaultAll) Then
            vRiskTransferAgreement = False
        End If



        If Informations.IsNothing(vBrokerlinkSubaccount) OrElse vBrokerlinkSubaccount.Equals(0) OrElse bDefaultAll Then
            vBrokerlinkSubaccount = 0
        End If



        If Informations.IsNothing(vBrokerlinkUnderwritingid) OrElse String.IsNullOrEmpty(vBrokerlinkUnderwritingid) OrElse bDefaultAll Then
            vBrokerlinkUnderwritingid = ""
        End If

        'QBENZ005


        If (Informations.IsNothing(vIsRIBroker)) OrElse (vIsRIBroker.Equals(False)) OrElse (bDefaultAll) Then
            vIsRIBroker = 0
        End If

        'Devlopment work on Insurer Payment Locking


        If (Informations.IsNothing(vCboLockingTypeId)) OrElse (vCboLockingTypeId.Equals(0)) OrElse (bDefaultAll) Then
            vCboLockingTypeId = 1
        End If



        If Informations.IsNothing(vRiskTransferEditable) OrElse vRiskTransferEditable.Equals(0) OrElse bDefaultAll Then
            vRiskTransferEditable = 0
        End If

        'PVY Development


        If Informations.IsNothing(vInsurerTypeId) OrElse vInsurerTypeId.Equals(0) OrElse bDefaultAll Then
            vInsurerTypeId = 0
        End If


        If Informations.IsNothing(vBureauAccountParty) OrElse vBureauAccountParty.Equals(0) OrElse bDefaultAll Then
            vBureauAccountParty = 0
        End If
        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the SIRPartyIN for Consistency.
    '
    ' ***************************************************************** '
    ' RAW 18/12/2002 : PS187 : Added WH TaxType, WH Tax Rate, Tax Reg No, Tax Code & PaymentMethod, PaymentFrequency, BankAccount
    Private Function Validate(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vAgencyNumber As Object = Nothing, Optional ByRef vBinderIndicator As Object = Nothing, Optional ByRef vReportIndicator As Object = Nothing, Optional ByRef vIsReinsurer As Object = Nothing, Optional ByRef vReinsuranceType As Object = Nothing, Optional ByRef vIsReinsuranceDebitCreditNo As Object = Nothing, Optional ByRef vDefaultCommRate As Object = Nothing, Optional ByRef vTaxGroupID As Object = Nothing, Optional ByRef vPaymentMethod As Object = Nothing, Optional ByRef vPaymentFrequency As Object = Nothing, Optional ByRef vBankAccount As Object = Nothing, Optional ByRef vIsRetained As Object = Nothing, Optional ByRef vRiskTransferAgreement As Object = Nothing, Optional ByRef vCboLockingTypeId As Object = Nothing) As Integer

        Dim result As Integer = 0

        Dim sMessage As String = "" ' RAW 18/12/2002 : PS187 : added



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Validate

        ' {* USER DEFINED CODE (Begin) *}


        If Not Informations.IsNothing(vPartyCnt) Then

            Dim dbNumericTemp As Double
            If Not Double.TryParse(CStr(vPartyCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                sMessage = sMessage & Strings.ChrW(13) & Constants.vbLf & "PartyCnt must be numeric" ' RAW 18/12/2002 : PS187 : replaced exit
            End If
        End If


        If Not Informations.IsNothing(vBinderIndicator) Then

            Dim dbNumericTemp2 As Double
            If Not Double.TryParse(CStr(vBinderIndicator), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                sMessage = sMessage & Strings.ChrW(13) & Constants.vbLf & "BinderIndicator must be numeric" ' RAW 18/12/2002 : PS187 : replaced exit
            End If
        End If


        If Not Informations.IsNothing(vReportIndicator) Then

            Dim dbNumericTemp3 As Double
            If Not Double.TryParse(CStr(vReportIndicator), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                sMessage = sMessage & Strings.ChrW(13) & Constants.vbLf & "ReportIndicator must be numeric" ' RAW 18/12/2002 : PS187 : replaced exit
            End If
        End If


        If Not Informations.IsNothing(vIsReinsurer) Then

            Dim dbNumericTemp4 As Double
            If Not Double.TryParse(CStr(vIsReinsurer), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                sMessage = sMessage & Strings.ChrW(13) & Constants.vbLf & "IsReinsurer must be numeric" ' RAW 18/12/2002 : PS187 : replaced exit
            End If
        End If


        If Not Informations.IsNothing(vReinsuranceType) Then

            Dim dbNumericTemp5 As Double
            If Not Double.TryParse(CStr(vReinsuranceType), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                sMessage = sMessage & Strings.ChrW(13) & Constants.vbLf & "ReinsuranceType must be numeric" ' RAW 18/12/2002 : PS187 : replaced exit
            End If
        End If


        If Not Informations.IsNothing(vIsReinsuranceDebitCreditNo) Then

            Dim dbNumericTemp6 As Double
            If Not Double.TryParse(CStr(vIsReinsuranceDebitCreditNo), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp6) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                sMessage = sMessage & Strings.ChrW(13) & Constants.vbLf & "IsReinsuranceDebitCreditNo must be numeric" ' RAW 18/12/2002 : PS187 : replaced exit
            End If
        End If


        If Not Informations.IsNothing(vDefaultCommRate) Then

            Dim dbNumericTemp7 As Double
            If Not Double.TryParse(CStr(vDefaultCommRate), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp7) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                sMessage = sMessage & Strings.ChrW(13) & Constants.vbLf & "DefaultCommRate must be numeric" ' RAW 18/12/2002 : PS187 : replaced exit
            End If
        End If

        ' RAW 18/12/2002 : PS187 : added

        If Not Informations.IsNothing(vTaxGroupID) Then

            Dim dbNumericTemp8 As Double
            If Not Double.TryParse(CStr(vTaxGroupID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp8) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                sMessage = sMessage & Strings.ChrW(13) & Constants.vbLf & "Tax Group must be numeric"
            End If
        End If

        ' Note - TaxRegNumber is optional and can hold any value
        ' Note - TaxCode is optional and can hold any value


        If Not Informations.IsNothing(vPaymentMethod) Then

            Dim dbNumericTemp9 As Double
            If Not Double.TryParse(CStr(vPaymentMethod), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp9) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                sMessage = sMessage & Strings.ChrW(13) & Constants.vbLf & "PaymentMethod must be numeric"
            End If
        End If


        If Not Informations.IsNothing(vPaymentFrequency) Then

            Dim dbNumericTemp10 As Double
            If Not Double.TryParse(CStr(vPaymentFrequency), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp10) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                sMessage = sMessage & Strings.ChrW(13) & Constants.vbLf & "PaymentFrequency must be numeric"
            End If
        End If


        If Not Informations.IsNothing(vIsRetained) Then

            'Dim dbNumericTemp11 As Double
            If (Informations.IsNumeric(vIsRetained)) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                sMessage = sMessage & Strings.ChrW(13) & Constants.vbLf & "IsRetained must be boolean"
            End If
        End If


        If Not Informations.IsNothing(vCboLockingTypeId) Then

            Dim dbNumericTemp12 As Double
            If Not Double.TryParse(CStr(vCboLockingTypeId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp12) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                sMessage = sMessage & Strings.ChrW(13) & Constants.vbLf & "Locking Type must be numeric"
            End If
        End If


        ' Note - BankAccount is optional and can hold any value

        'Handle any errors detected
        If sMessage <> "" Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="Validate")
            If result = gPMConstants.PMEReturnCode.PMFalse Then
                Return result
            End If
        End If

        ' RAW 18/12/2002 : PS187 : end

        ' {* USER DEFINED CODE (End) *}

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
