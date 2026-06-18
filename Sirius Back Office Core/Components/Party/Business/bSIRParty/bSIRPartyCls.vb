Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
'Developer Guide No. 129
Imports SSP.Shared
Friend NotInheritable Class SIRParty
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: SIRParty
    '
    ' Date: 12/10/1998
    '
    ' Description: Describes the SIRParty attributes.
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
    Private Const ACClass As String = "SIRParty"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' Instance of Data component
    Private m_dSIRParty As dSIRParty.SIRParty

    ' Error Code
    Private m_lReturn As Integer

    ' Primary Keys to work with
    Private m_lPartyCnt As Integer

    Private m_bEvent As Boolean

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

    Public Property FromEvent() As Boolean
        Get

            Return m_bEvent

        End Get
        Set(ByVal Value As Boolean)

            m_bEvent = Value

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
    Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
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
            m_dSIRParty = New dSIRParty.SIRParty()

            m_lReturn = m_dSIRParty.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=vDatabase)

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
                If m_dSIRParty IsNot Nothing Then
                    m_dSIRParty.Dispose()
                End If
                m_dSIRParty = Nothing
            End If
        End If
		Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetDefaults (Public)
    '
    ' Description: Returns the Default Values for the SIRParty.
    '
    ' ***************************************************************** '
    'DC 28/06/00 Added Correspondence Type Id
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyTypeID As Object = Nothing, Optional ByRef vPartyStructureID As Object = Nothing, Optional ByRef vSourceID As Object = Nothing, Optional ByRef vIsAlsoAgent As Object = Nothing, Optional ByRef vPartyID As Object = Nothing, Optional ByRef vShortname As Object = Nothing, Optional ByRef vName As Object = Nothing, Optional ByRef vResolvedName As Object = Nothing, Optional ByRef vCurrencyId As Object = Nothing, Optional ByRef vLanguageID As Object = Nothing, Optional ByRef vCollectTypeID As Object = Nothing, Optional ByRef vAccumTreatmentTypeID As Object = Nothing, Optional ByRef vStatsTreatmentTypeID As Object = Nothing, Optional ByRef vPartyCategoryID As Object = Nothing, Optional ByRef vAgentCnt As Object = Nothing, Optional ByRef vConsultantCnt As Object = Nothing, Optional ByRef vCreatedByID As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vLastModified As Object = Nothing, Optional ByRef vModifiedByID As Object = Nothing, Optional ByRef vPaymentMethodCode As Object = Nothing, Optional ByRef vPaymentTermCode As Object = Nothing, Optional ByRef vCreditCardCode As Object = Nothing, Optional ByRef vFileCode As Object = Nothing, Optional ByRef vABCCount As Object = Nothing, Optional ByRef vStatements As Object = Nothing, Optional ByRef vReminderTypeId As Object = Nothing, Optional ByRef vRenewals As Object = Nothing, Optional ByRef vStatus As Object = Nothing, Optional ByRef vLAstActionType As Object = Nothing, Optional ByRef vIsTravelAgent As Object = Nothing, Optional ByRef vIsProspect As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vABICodeOn406 As Object = Nothing, Optional ByRef vABICodeOn81 As Object = Nothing, Optional ByRef vABICodeList As Object = Nothing, Optional ByRef vAreaId As Object = Nothing, Optional ByRef vServiceLevelId As Object = Nothing, Optional ByRef vInvariantKey As Object = Nothing, Optional ByRef vRecordStatus As Object = Nothing, Optional ByRef vCCJs As Object = Nothing, Optional ByRef vUserDefinedDataId As Object = Nothing, Optional ByRef vSeasonalGiftID As Object = Nothing, Optional ByRef vCorrespondenceTypeId As Object = Nothing, Optional ByRef vRenewalStopCodeId As Object = Nothing, Optional ByRef vSwiftPartyId As Integer = 0) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults
            'DC 28/06/00 Added Correspondence Type Id
            'DC 16/08/00 PaymentTermCode was incorrectly set to PaymentMethodCode










































            'Developer Guide No. 98
            m_lReturn = DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vPartyCnt:=vPartyCnt, vPartyTypeID:=vPartyTypeID, vIsAlsoAgent:=vIsAlsoAgent, vPartyStructureID:=vPartyStructureID, vSourceID:=vSourceID, vPartyID:=vPartyID, vShortname:=vShortname, vName:=vName, vResolvedName:=vResolvedName, vCurrencyId:=vCurrencyId, vLanguageID:=vLanguageID, vCollectTypeID:=vCollectTypeID, vAccumTreatmentTypeID:=vAccumTreatmentTypeID, vStatsTreatmentTypeID:=vStatsTreatmentTypeID, vPartyCategoryID:=vPartyCategoryID, vAgentCnt:=vAgentCnt, vConsultantCnt:=vConsultantCnt, vCreatedByID:=vCreatedByID, vDateCreated:=vDateCreated, vLastModified:=vLastModified, vModifiedByID:=vModifiedByID, vPaymentMethodCode:=vPaymentMethodCode, vPaymentTermCode:=vPaymentTermCode, vCreditCardCode:=vCreditCardCode, vFileCode:=vFileCode, vABCCount:=vABCCount, vStatements:=vStatements, vReminderTypeId:=vReminderTypeId, vRenewals:=vRenewals, vStatus:=vStatus, vLAstActionType:=vLAstActionType, vIsTravelAgent:=vIsTravelAgent, vIsProspect:=vIsProspect, vIsDeleted:=vIsDeleted, vABICodeOn406:=vABICodeOn406, vABICodeOn81:=vABICodeOn81, vABICodeList:=vABICodeList, vAreaId:=vAreaId, vServiceLevelId:=vServiceLevelId, vInvariantKey:=vInvariantKey, vRecordStatus:=vRecordStatus, vCCJs:=vCCJs, vUserDefinedDataId:=vUserDefinedDataId, vSeasonalGiftID:=vSeasonalGiftID, vCorrespondenceTypeId:=vCorrespondenceTypeId, vRenewalStopCodeId:=vRenewalStopCodeId, vSwiftPartyId:=vSwiftPartyId)

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
    Private Function Validate(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyTypeID As Object = Nothing, Optional ByRef vPartyStructureID As Object = Nothing, Optional ByRef vSourceID As Object = Nothing, Optional ByRef vIsAlsoAgent As Object = Nothing, Optional ByRef vPartyID As Object = Nothing, Optional ByRef vShortname As Object = Nothing, Optional ByRef vName As Object = Nothing, Optional ByRef vResolvedName As Object = Nothing, Optional ByRef vCurrencyId As Object = Nothing, Optional ByRef vLanguageID As Object = Nothing, Optional ByRef vCollectTypeID As Object = Nothing, Optional ByRef vAccumTreatmentTypeID As Object = Nothing, Optional ByRef vStatsTreatmentTypeID As Object = Nothing, Optional ByRef vPartyCategoryID As Object = Nothing, Optional ByRef vAgentCnt As Object = Nothing, Optional ByRef vConsultantCnt As Object = Nothing, Optional ByRef vCreatedByID As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vLastModified As Object = Nothing, Optional ByRef vModifiedByID As Object = Nothing, Optional ByRef vPaymentMethodCode As Object = Nothing, Optional ByRef vPaymentTermCode As Object = Nothing, Optional ByRef vCreditCardCode As Object = Nothing, Optional ByRef vFileCode As Object = Nothing, Optional ByRef vABCCount As Object = Nothing, Optional ByRef vStatements As Object = Nothing, Optional ByRef vReminderTypeId As Object = Nothing, Optional ByRef vRenewals As Object = Nothing, Optional ByRef vStatus As Object = Nothing, Optional ByRef vLAstActionType As Object = Nothing, Optional ByRef vIsTravelAgent As Object = Nothing, Optional ByRef vIsProspect As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vABICodeOn406 As Object = Nothing, Optional ByRef vABICodeOn81 As Object = Nothing, Optional ByRef vABICodeList As Object = Nothing, Optional ByRef vAreaId As Object = Nothing, Optional ByRef vServiceLevelId As Object = Nothing, Optional ByRef vInvariantKey As Object = Nothing, Optional ByRef vRecordStatus As Object = Nothing, Optional ByRef vCCJs As Object = Nothing, Optional ByRef vUserDefinedDataId As Object = Nothing, Optional ByRef vSeasonalGiftID As Object = Nothing, Optional ByRef vCorrespondenceTypeId As Object = Nothing, Optional ByRef vRenewalStopCodeId As Object = Nothing, Optional ByRef vSwiftPartyId As Object = Nothing, Optional ByRef vLoyaltyNumber As Object = Nothing, Optional ByRef vAlternativeIdentifier As Object = Nothing, Optional ByRef vMarketingSegmentInd As Object = Nothing, Optional ByRef vTradingName As Object = Nothing, Optional ByRef vSubBranchId As Object = Nothing, Optional ByRef vSubBranchName As Object = Nothing, Optional ByRef vTobLetter As Object = Nothing, Optional ByRef vOverrideCommission As Object = Nothing, Optional ByRef vOverrideCommissionRenewal As Object = Nothing) As Integer

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


        If Not Informations.IsNothing(vPartyTypeID) Then

            Dim dbNumericTemp2 As Double
            If Not Double.TryParse(CStr(vPartyTypeID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vIsAlsoAgent) Then

            Dim dbNumericTemp3 As Double
            If Not Double.TryParse(CStr(vIsAlsoAgent), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vPartyStructureID) Then

            Dim dbNumericTemp4 As Double
            If Not Double.TryParse(CStr(vPartyStructureID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vSourceID) Then

            Dim dbNumericTemp5 As Double
            If Not Double.TryParse(CStr(vSourceID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vPartyID) Then

            Dim dbNumericTemp6 As Double
            If Not Double.TryParse(CStr(vPartyID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp6) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vCurrencyId) Then

            Dim dbNumericTemp7 As Double
            If Not Double.TryParse(CStr(vCurrencyId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp7) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vLanguageID) Then

            Dim dbNumericTemp8 As Double
            If Not Double.TryParse(CStr(vLanguageID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp8) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vCollectTypeID) Then

            Dim dbNumericTemp9 As Double
            If Not Double.TryParse(CStr(vCollectTypeID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp9) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vAccumTreatmentTypeID) Then

            Dim dbNumericTemp10 As Double
            If Not Double.TryParse(CStr(vAccumTreatmentTypeID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp10) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vStatsTreatmentTypeID) Then

            Dim dbNumericTemp11 As Double
            If Not Double.TryParse(CStr(vStatsTreatmentTypeID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp11) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vPartyCategoryID) Then

            Dim dbNumericTemp12 As Double
            If Not Double.TryParse(CStr(vPartyCategoryID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp12) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vAgentCnt) Then


            Dim dbNumericTemp13 As Double
            If (Not Double.TryParse(CStr(vAgentCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp13)) And (Not (Convert.IsDBNull(vAgentCnt) Or Informations.IsNothing(vAgentCnt))) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vConsultantCnt) Then


            Dim dbNumericTemp14 As Double
            If (Not Double.TryParse(CStr(vConsultantCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp14)) And (Not (Convert.IsDBNull(vConsultantCnt) Or Informations.IsNothing(vConsultantCnt))) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        If Not Informations.IsNothing(vCreatedByID) Then

            Dim dbNumericTemp15 As Double
            If Not Double.TryParse(CStr(vCreatedByID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp15) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vDateCreated) Then
            If Not Informations.IsDate(vDateCreated) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vLastModified) Then
            If Not Informations.IsDate(vLastModified) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vModifiedByID) Then

            Dim dbNumericTemp16 As Double
            If Not Double.TryParse(CStr(vModifiedByID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp16) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vABCCount) Then

            Dim dbNumericTemp17 As Double
            If Not Double.TryParse(CStr(vABCCount), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp17) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vStatements) Then

            Dim dbNumericTemp18 As Double
            If Not Double.TryParse(CStr(vStatements), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp18) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vReminderTypeId) Then

            Dim dbNumericTemp19 As Double
            If Not Double.TryParse(CStr(vReminderTypeId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp19) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vRenewals) Then

            Dim dbNumericTemp20 As Double
            If Not Double.TryParse(CStr(vRenewals), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp20) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vIsTravelAgent) Then

            Dim dbNumericTemp21 As Double
            If Not Double.TryParse(CStr(vIsTravelAgent), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp21) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vIsProspect) Then

            Dim dbNumericTemp22 As Double
            If Not Double.TryParse(CStr(vIsProspect), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp22) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vIsDeleted) Then

            Dim dbNumericTemp23 As Double
            If Not Double.TryParse(CStr(vIsDeleted), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp23) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vAreaId) Then

            Dim dbNumericTemp24 As Double
            If Not Double.TryParse(CStr(vAreaId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp24) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vServiceLevelId) Then

            Dim dbNumericTemp25 As Double
            If Not Double.TryParse(CStr(vServiceLevelId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp25) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vInvariantKey) Then

            Dim dbNumericTemp26 As Double
            If Not Double.TryParse(CStr(vInvariantKey), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp26) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vCCJs) Then

            Dim dbNumericTemp27 As Double
            If Not Double.TryParse(CStr(vCCJs), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp27) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vUserDefinedDataId) Then

            Dim dbNumericTemp28 As Double
            If Not Double.TryParse(CStr(vUserDefinedDataId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp28) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        'CT 07/12/00 should be null if coming from maintain commision accounts so
        'cater for null

        If Not Informations.IsNothing(vSwiftPartyId) Then
            'If (IsNumeric(vSwiftPartyID) = False) Then

            Dim dbNumericTemp29 As Double
            'Developer Guide No. 115
            If (Not (Convert.IsDBNull(vSwiftPartyId) Or Informations.IsNothing(vSwiftPartyId))) AndAlso (Not Double.TryParse(CStr(vSwiftPartyId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp29)) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: SetProperties (Public)
    '
    ' Description: Sets the supplied SIRParty property values.
    '
    ' ***************************************************************** '
    'DC 28/06/00 Added Correspondence Type Id
    'sj 12/06/2002 - Added LoyaltyNumber, AlternativeIdentifier,
    '                MarketingSegmentInd, TradingName and SubBranchId
    'FSA Phase III
    'Developer Guide No. 101
    Public Function SetProperties(ByRef iStatus As Integer, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyTypeID As Object = Nothing, Optional ByRef vPartyStructureID As Object = Nothing, Optional ByRef vSourceID As Object = Nothing, Optional ByRef vIsAlsoAgent As Object = Nothing, Optional ByRef vPartyID As Object = Nothing, Optional ByRef vShortname As Object = Nothing, Optional ByRef vName As Object = Nothing, Optional ByRef vResolvedName As Object = Nothing, Optional ByRef vCurrencyId As Object = Nothing, Optional ByRef vLanguageID As Object = Nothing, Optional ByRef vCollectTypeID As Object = Nothing, Optional ByRef vAccumTreatmentTypeID As Object = Nothing, Optional ByRef vStatsTreatmentTypeID As Object = Nothing, Optional ByRef vPartyCategoryID As Object = Nothing, Optional ByRef vAgentCnt As Object = Nothing, Optional ByRef vConsultantCnt As Object = Nothing, Optional ByRef vCreatedByID As Object = Nothing, Optional ByRef vDateCreated As Date = #12/30/1899#, Optional ByRef vLastModified As Date = #12/30/1899#, Optional ByRef vModifiedByID As Object = Nothing, Optional ByRef vPaymentMethodCode As Object = Nothing, Optional ByRef vPaymentTermCode As Object = Nothing, Optional ByRef vCreditCardCode As Object = Nothing, Optional ByRef vFileCode As Object = Nothing, Optional ByRef vABCCount As Object = Nothing, Optional ByRef vStatements As Object = Nothing, Optional ByRef vReminderTypeId As Object = Nothing, Optional ByRef vRenewals As Object = Nothing, Optional ByRef vStatus As Object = Nothing, Optional ByRef vLAstActionType As Object = Nothing, Optional ByRef vIsTravelAgent As Object = Nothing, Optional ByRef vIsProspect As Object = Nothing, Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vABICodeOn406 As Object = Nothing, Optional ByRef vABICodeOn81 As Object = Nothing, Optional ByRef vABICodeList As Object = Nothing, Optional ByRef vAreaId As Object = Nothing, Optional ByRef vServiceLevelId As Object = Nothing, Optional ByRef vInvariantKey As Object = Nothing, Optional ByRef vRecordStatus As Object = Nothing, Optional ByRef vCCJs As Object = Nothing, Optional ByRef vUserDefinedDataId As Object = Nothing, Optional ByRef vSeasonalGiftID As Object = Nothing, Optional ByRef vCorrespondenceTypeId As Object = Nothing, Optional ByRef vRenewalStopCodeId As Object = Nothing, Optional ByRef vSwiftPartyId As Object = Nothing, Optional ByRef vLoyaltyNumber As Object = Nothing, Optional ByRef vAlternativeIdentifier As Object = Nothing, Optional ByRef vMarketingSegmentInd As Object = Nothing, Optional ByRef vTradingName As Object = Nothing, Optional ByRef vSubBranchId As Object = Nothing, Optional ByRef vTobLetter As Object = Nothing, Optional ByRef vOverrideCommission As Object = Nothing, Optional ByRef vOverrideCommissionRenewal As Object = Nothing, Optional ByRef vParamArray As Object = Nothing, Optional ByVal vUniqueId As Object = Nothing, Optional ByVal vScreenHeirarchy As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim bDataChanged As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If Add Mode
            If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

                ' Default Any Missing Parameters
                'DC 28/06/00 Added Correspondence Type Id
                'sj 12/06/2002 - Added LoyaltyNumber, AlternativeIdentifier,
                '                MarketingSegmentInd, TradingName and SubBranchId
                'FSA Phase III
                'Developer Guide No. 67
                m_lReturn = DefaultParameters(bDefaultAll:=False, vPartyCnt:=vPartyCnt, vPartyTypeID:=vPartyTypeID, vIsAlsoAgent:=vIsAlsoAgent, vPartyStructureID:=vPartyStructureID, vSourceID:=vSourceID, vPartyID:=vPartyID, vShortname:=vShortname, vName:=vName, vResolvedName:=vResolvedName, vCurrencyId:=vCurrencyId, vLanguageID:=vLanguageID, vCollectTypeID:=vCollectTypeID, vAccumTreatmentTypeID:=vAccumTreatmentTypeID, vStatsTreatmentTypeID:=vStatsTreatmentTypeID, vPartyCategoryID:=vPartyCategoryID, vAgentCnt:=vAgentCnt, vConsultantCnt:=vConsultantCnt, vCreatedByID:=vCreatedByID, vDateCreated:=vDateCreated, vLastModified:=vLastModified, vModifiedByID:=vModifiedByID, vPaymentMethodCode:=vPaymentMethodCode, vPaymentTermCode:=vPaymentTermCode, vCreditCardCode:=vCreditCardCode, vFileCode:=vFileCode, vABCCount:=vABCCount, vStatements:=vStatements, vReminderTypeId:=vReminderTypeId, vRenewals:=vRenewals, vStatus:=vStatus, vLAstActionType:=vLAstActionType, vIsTravelAgent:=vIsTravelAgent, vIsProspect:=vIsProspect, vIsDeleted:=vIsDeleted, vABICodeOn406:=vABICodeOn406, vABICodeOn81:=vABICodeOn81, vABICodeList:=vABICodeList, vAreaId:=vAreaId, vServiceLevelId:=vServiceLevelId, vInvariantKey:=vInvariantKey, vRecordStatus:=vRecordStatus, vCCJs:=vCCJs, vUserDefinedDataId:=vUserDefinedDataId, vSeasonalGiftID:=vSeasonalGiftID, vCorrespondenceTypeId:=vCorrespondenceTypeId, vRenewalStopCodeId:=vRenewalStopCodeId, vSwiftPartyId:=vSwiftPartyId, vLoyaltyNumber:=vLoyaltyNumber, vAlternativeIdentifier:=vAlternativeIdentifier, vMarketingSegmentInd:=vMarketingSegmentInd, vTradingName:=vTradingName, vSubBranchId:=vSubBranchId, vTobLetter:=vTobLetter, vOverrideCommission:=vOverrideCommission, vOverrideCommissionRenewal:=vOverrideCommissionRenewal)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

            End If

            ' Validate Parameters
            'DC 28/06/00 Added Correspondence Type Id
            'sj 12/06/2002 - Added LoyaltyNumber, AlternativeIdentifier,
            '                MarketingSegmentInd, TradingName and SubBranchId
            'FSA Phase III
            m_lReturn = Validate(vPartyCnt, vPartyTypeID, vPartyStructureID, vSourceID, vIsAlsoAgent, vPartyID, vShortname, vName, vResolvedName, vCurrencyId, vLanguageID, vCollectTypeID, vAccumTreatmentTypeID, vStatsTreatmentTypeID, vPartyCategoryID, vAgentCnt, vConsultantCnt, vCreatedByID, vDateCreated, vLastModified, vModifiedByID, vPaymentMethodCode, vPaymentTermCode, vCreditCardCode, vFileCode, vABCCount, vStatements, vReminderTypeId, vRenewals, vStatus, vLAstActionType, vIsTravelAgent, vIsProspect, vIsDeleted, vABICodeOn406, vABICodeOn81, vABICodeList, vAreaId, vServiceLevelId, vInvariantKey, vRecordStatus, vCCJs, vUserDefinedDataId, vSeasonalGiftID, vCorrespondenceTypeId, vRenewalStopCodeId, vSwiftPartyId, vLoyaltyNumber, vAlternativeIdentifier, vMarketingSegmentInd, vTradingName, vSubBranchId, , vTobLetter, vOverrideCommission, vOverrideCommissionRenewal)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Set Data Changed Flag to False
            bDataChanged = False

            ' Set Property values.
            With m_dSIRParty



                If (Not Informations.IsNothing(vPartyCnt)) AndAlso (Not vPartyCnt.Equals(0)) Then
                    .PartyCnt = vPartyCnt
                End If



                If (Not Informations.IsNothing(vPartyTypeID)) AndAlso (Not vPartyTypeID.Equals(0)) Then
                    .PartyTypeID = vPartyTypeID
                End If



                If (Not Informations.IsNothing(vIsAlsoAgent)) AndAlso (Not Object.Equals(vIsAlsoAgent, Nothing)) Then
                    'Developer Guide No. 24
                    .IsAlsoAgent = vIsAlsoAgent
                End If
                If (Not Informations.IsNothing(vPartyStructureID)) AndAlso (Not vPartyStructureID.Equals(0)) Then
                    .PartyStructureID = vPartyStructureID
                End If



                If (Not Informations.IsNothing(vSourceID)) AndAlso (Not vSourceID.Equals(0)) Then
                    .SourceID = vSourceID
                End If



                If (Not Informations.IsNothing(vPartyID)) AndAlso (Not vPartyID.Equals(0)) Then
                    .PartyID = vPartyID
                End If



                If (Not Informations.IsNothing(vShortname)) AndAlso (Not String.IsNullOrEmpty(vShortname)) Then
                    .Shortname = vShortname
                End If



                If (Not Informations.IsNothing(vName)) AndAlso (Not String.IsNullOrEmpty(vName)) Then
                    .Name = vName
                End If



                If (Not Informations.IsNothing(vResolvedName)) AndAlso (Not Object.Equals(vResolvedName, Nothing)) Then
                    'Developer Guide No. 24 
                    .ResolvedName = vResolvedName
                End If



                If (Not Informations.IsNothing(vCurrencyId)) AndAlso (Not vCurrencyId.Equals(0)) Then
                    .CurrencyID = vCurrencyId
                End If



                If (Not Informations.IsNothing(vLanguageID)) AndAlso (Not vLanguageID.Equals(0)) Then
                    .LanguageID = vLanguageID
                End If



                If (Not Informations.IsNothing(vCollectTypeID)) AndAlso (Not Object.Equals(vCollectTypeID, Nothing)) Then
                    'Developer Guide No. 24
                    .CollectTypeID = vCollectTypeID
                End If



                If (Not Informations.IsNothing(vAccumTreatmentTypeID)) AndAlso (Not Object.Equals(vAccumTreatmentTypeID, Nothing)) Then
                    'Developer Guide No. 24
                    .AccumTreatmentTypeID = vAccumTreatmentTypeID
                End If



                If (Not Informations.IsNothing(vStatsTreatmentTypeID)) AndAlso (Not Object.Equals(vStatsTreatmentTypeID, Nothing)) Then
                    'Developer Guide No. 24
                    .StatsTreatmentTypeID = vStatsTreatmentTypeID
                End If



                If (Not Informations.IsNothing(vPartyCategoryID)) AndAlso (Not Object.Equals(vPartyCategoryID, Nothing)) Then

                    .PartyCategoryID = vPartyCategoryID
                End If



                If (Not Informations.IsNothing(vAgentCnt)) AndAlso (Not Object.Equals(vAgentCnt, Nothing)) Then
                    'Developer Guide No. 24
                    .AgentCnt = vAgentCnt
                End If



                If (Not Informations.IsNothing(vConsultantCnt)) AndAlso (Not Object.Equals(vConsultantCnt, Nothing)) Then
                    'Developer Guide No. 24
                    .ConsultantCnt = vConsultantCnt
                End If



                If (Not Informations.IsNothing(vCreatedByID)) AndAlso (Not vCreatedByID.Equals(0)) Then
                    .CreatedByID = vCreatedByID
                End If



                If (Not Informations.IsNothing(vDateCreated)) AndAlso (Not vDateCreated.Equals(#12:00:00 AM#)) Then
                    .DateCreated = vDateCreated
                End If



                If (Not Informations.IsNothing(vLastModified)) AndAlso (Not vDateCreated.Equals(#12:00:00 AM#)) Then
                    'Developer Guide No. 24
                    .LastModified = vLastModified
                End If



                If (Not Informations.IsNothing(vModifiedByID)) AndAlso (Not Object.Equals(vModifiedByID, Nothing)) Then
                    'Developer Guide No. 24
                    .ModifiedByID = vModifiedByID
                End If



                If (Not Informations.IsNothing(vPaymentMethodCode)) AndAlso (Not Object.Equals(vPaymentMethodCode, Nothing)) Then
                    'Developer Guide No. 24
                    .PaymentMethodCode = vPaymentMethodCode
                End If



                If (Not Informations.IsNothing(vServiceLevelId)) AndAlso (Not vServiceLevelId.Equals(0)) Then
                    .ServiceLevelId = vServiceLevelId
                Else
                    .ServiceLevelId = 0
                End If



                If (Not Informations.IsNothing(vPaymentTermCode)) AndAlso (Not Object.Equals(vPaymentTermCode, Nothing)) Then
                    .PaymentTermCode = vPaymentTermCode
                End If



                If (Not Informations.IsNothing(vCreditCardCode)) AndAlso (Not Object.Equals(vCreditCardCode, Nothing)) Then
                    'Developer Guide No. 24
                    .CreditCardCode = vCreditCardCode
                End If



                If (Not Informations.IsNothing(vFileCode)) AndAlso (Not Object.Equals(vFileCode, Nothing)) Then

                    'Developer Guide No. 24
                    .FileCode = vFileCode
                End If



                If (Not Informations.IsNothing(vABCCount)) AndAlso (Not Object.Equals(vABCCount, Nothing)) Then

                    'Developer Guide No. 24
                    .ABCCount = vABCCount
                End If



                If (Not Informations.IsNothing(vStatements)) AndAlso (Not vStatements.Equals(0)) Then
                    .Statements = vStatements
                End If



                If (Not Informations.IsNothing(vReminderTypeId)) AndAlso (Not Object.Equals(vReminderTypeId, Nothing)) Then


                    'Developer Guide No. 24
                    .ReminderTypeId = vReminderTypeId
                End If



                If (Not Informations.IsNothing(vRenewals)) AndAlso (Not vRenewals.Equals(0)) Then
                    .Renewals = vRenewals
                End If



                If (Not Informations.IsNothing(vStatus)) AndAlso (Not Object.Equals(vStatus, Nothing)) Then


                    'Developer Guide No. 24
                    .Status = vStatus
                End If



                If (Not Informations.IsNothing(vLAstActionType)) AndAlso (Not Object.Equals(vLAstActionType, Nothing)) Then


                    'Developer Guide No. 24
                    .LastActionType = vLAstActionType
                End If



                If (Not Informations.IsNothing(vIsTravelAgent)) AndAlso (Not vIsTravelAgent.Equals(0)) Then
                    .IsTravelAgent = vIsTravelAgent
                End If



                If (Not Informations.IsNothing(vIsProspect)) AndAlso (Not vIsProspect.Equals(0)) Then
                    .IsProspect = vIsProspect
                End If



                If (Not Informations.IsNothing(vIsDeleted)) Then
                    .IsDeleted = vIsDeleted
                End If



                If (Not Informations.IsNothing(vABICodeOn406)) AndAlso (Not Object.Equals(vABICodeOn406, Nothing)) Then


                    'Developer Guide No. 24
                    .ABICodeOn406 = vABICodeOn406
                End If



                If (Not Informations.IsNothing(vABICodeOn81)) AndAlso (Not Object.Equals(vABICodeOn81, Nothing)) Then


                    'Developer Guide No. 24
                    .ABICodeOn81 = vABICodeOn81
                End If



                If (Not Informations.IsNothing(vABICodeList)) AndAlso (Not Object.Equals(vABICodeList, Nothing)) Then


                    'Developer Guide No. 24
                    .ABICodeList = vABICodeList
                End If



                If (Not Informations.IsNothing(vAreaId)) AndAlso (Not Object.Equals(vAreaId, Nothing)) Then


                    'Developer Guide No. 24
                    .AreaId = vAreaId
                End If



                If (Not Informations.IsNothing(vInvariantKey)) AndAlso (Not vInvariantKey.Equals(0)) Then
                    .InvariantKey = vInvariantKey
                End If



                If (Not Informations.IsNothing(vRecordStatus)) AndAlso (Not Object.Equals(vRecordStatus, Nothing)) Then


                    'Developer Guide No. 24
                    .RecordStatus = vRecordStatus
                End If



                If (Not Informations.IsNothing(vCCJs)) AndAlso (Not vCCJs.Equals(0)) Then
                    .CCJs = vCCJs
                End If



                If (Not Informations.IsNothing(vUserDefinedDataId)) AndAlso (Not Object.Equals(vUserDefinedDataId, Nothing)) Then


                    'Developer Guide No. 24
                    .UserDefinedDataId = vUserDefinedDataId
                End If



                If (Not Informations.IsNothing(vSeasonalGiftID)) AndAlso (Not Object.Equals(vSeasonalGiftID, Nothing)) Then


                    'Developer Guide No. 24
                    .SeasonalGiftID = vSeasonalGiftID
                End If

                'DC 28/06/00


                If (Not Informations.IsNothing(vCorrespondenceTypeId)) AndAlso (Not Object.Equals(vCorrespondenceTypeId, Nothing)) Then


                    'Developer Guide No. 24
                    .CorrespondenceTypeId = vCorrespondenceTypeId
                Else
                    .CorrespondenceTypeId = 0
                End If

                'Tomo060700


                If (Not Informations.IsNothing(vRenewalStopCodeId)) AndAlso (Not Object.Equals(vRenewalStopCodeId, Nothing)) Then


                    'Developer Guide No. 24
                    .RenewalStopCodeId = vRenewalStopCodeId
                End If

                ' CTAF 250900


                If (Not Informations.IsNothing(vSwiftPartyId)) AndAlso (Not Object.Equals(vSwiftPartyId, Nothing)) Then


                    'Developer Guide No. 24
                    .SwiftPartyID = vSwiftPartyId
                End If

                'sj 13/06/2002 - start


                If (Not Informations.IsNothing(vLoyaltyNumber)) AndAlso (Not Object.Equals(vLoyaltyNumber, Nothing)) Then


                    'Developer Guide No. 24
                    .LoyaltyNumber = vLoyaltyNumber
                End If


                If (Not Informations.IsNothing(vAlternativeIdentifier)) AndAlso (Not Object.Equals(vAlternativeIdentifier, Nothing)) Then


                    'Developer Guide No. 24
                    .AlternativeIdentifier = vAlternativeIdentifier
                End If


                If (Not Informations.IsNothing(vMarketingSegmentInd)) AndAlso (Not Object.Equals(vMarketingSegmentInd, Nothing)) Then


                    'Developer Guide No. 24
                    .MarketingSegmentInd = vMarketingSegmentInd
                End If


                If (Not Informations.IsNothing(vTradingName)) AndAlso (Not Object.Equals(vTradingName, Nothing)) Then

                    .TradingName = vTradingName
                End If


                If (Not Informations.IsNothing(vSubBranchId)) AndAlso (Not Object.Equals(vSubBranchId, Nothing)) Then

                    .SubBranchId = vSubBranchId
                End If
                'FSA Phase III


                If (Not Informations.IsNothing(vTobLetter)) AndAlso (Not Object.Equals(vTobLetter, Nothing)) Then

                    .TobLetter = vTobLetter
                End If
                'Override Commission


                If (Not Informations.IsNothing(vOverrideCommission)) AndAlso (Not vOverrideCommission.Equals(0)) Then
                    .Override = vOverrideCommission
                End If

                If (Not String.IsNullOrEmpty(vUniqueId)) Then
                    .ScreenHeirarchy = vScreenHeirarchy
                    .UniqueId = vUniqueId
                End If

                If (Not Informations.IsNothing(vOverrideCommissionRenewal)) AndAlso (Not vOverrideCommissionRenewal.Equals(0)) Then
                    .OverrideRenewal = vOverrideCommissionRenewal
                End If

                ' If we have changed one of the properties, update the status
                m_iDatabaseStatus = iStatus

            End With

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, gPMConstants.PMELogLevel.PMLogOnError, "SetProperties Failed", ACApp, ACClass, "SetProperties", Informations.Err().Number, excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetProperties (Public)
    '
    ' Description: Returns the supplied SIRParty property values.
    '
    ' ***************************************************************** '
    'DC 28/06/00 Added Correspondence Type Id
    'sj 12/06/2002 - Added LoyaltyNumber, AlternativeIdentifier,
    '                MarketingSegmentInd, TradingName and SubBranchId
    'SD 17/07/2002 Added Sub Branch Name
    'JAS(CMG) 05/09/02 - Added vRecordStatus
    'FSA Phase III
    'Developer Guide No. 101
    Public Function GetProperties(ByRef iStatus As Integer, Optional ByRef vPartyCnt As Object = Nothing,
                                Optional ByRef vPartyTypeID As Object = Nothing, Optional ByRef vPartyStructureID As Object = Nothing,
                                Optional ByRef vSourceID As Object = Nothing, Optional ByRef vIsAlsoAgent As Object = Nothing,
                                Optional ByRef vPartyID As Object = Nothing, Optional ByRef vShortname As Object = Nothing,
                                Optional ByRef vName As Object = Nothing, Optional ByRef vResolvedName As Object = Nothing,
                                Optional ByRef vCurrencyId As Object = Nothing, Optional ByRef vLanguageID As Object = Nothing,
                                Optional ByRef vCollectTypeID As Object = Nothing, Optional ByRef vAccumTreatmentTypeID As Object = Nothing,
                                Optional ByRef vStatsTreatmentTypeID As Object = Nothing, Optional ByRef vPartyCategoryID As Object = Nothing,
                                Optional ByRef vAgentCnt As Object = 0, Optional ByRef vConsultantCnt As Object = 0,
                                Optional ByRef vCreatedByID As Object = Nothing, Optional ByRef vDateCreated As Date = #12/30/1899#,
                                Optional ByRef vLastModified As Object = Nothing, Optional ByRef vModifiedByID As Object = Nothing,
                                Optional ByRef vPaymentMethodCode As Object = Nothing, Optional ByRef vPaymentTermCode As Object = Nothing,
                                Optional ByRef vCreditCardCode As Object = Nothing, Optional ByRef vFileCode As Object = Nothing,
                                Optional ByRef vABCCount As Object = Nothing, Optional ByRef vStatements As Object = Nothing,
                                Optional ByRef vReminderTypeId As Object = Nothing, Optional ByRef vRenewals As Object = Nothing,
                                Optional ByRef vStatus As Object = Nothing, Optional ByRef vLAstActionType As Object = Nothing,
                                Optional ByRef vIsTravelAgent As Object = Nothing, Optional ByRef vIsProspect As Object = Nothing,
                                Optional ByRef vIsDeleted As Object = Nothing, Optional ByRef vABICodeOn406 As Object = Nothing,
                                Optional ByRef vABICodeOn81 As Object = Nothing, Optional ByRef vABICodeList As Object = Nothing,
                                Optional ByRef vAreaId As Object = Nothing, Optional ByRef vServiceLevelId As Object = Nothing,
                                Optional ByRef vInvariantKey As Object = Nothing, Optional ByRef vRecordStatus As String = "",
                                Optional ByRef vCCJs As Object = Nothing, Optional ByRef vUserDefinedDataId As Object = Nothing,
                                Optional ByRef vSeasonalGiftID As Object = Nothing, Optional ByRef vCorrespondenceTypeId As Object = Nothing,
                                Optional ByRef vRenewalStopCodeId As Object = Nothing, Optional ByRef vSwiftPartyId As Object = Nothing,
                                Optional ByRef vLoyaltyNumber As Object = Nothing, Optional ByRef vAlternativeIdentifier As Object = Nothing,
                                Optional ByRef vMarketingSegmentInd As Object = Nothing, Optional ByRef vTradingName As Object = Nothing,
                                Optional ByRef vSubBranchId As Object = Nothing, Optional ByRef vSubBranchName As Object = Nothing,
                                Optional ByRef vTobLetter As Object = Nothing, Optional ByRef vOverrideCommission As Object = Nothing,
                                Optional ByRef vOverrideCommissionRenewal As Object = Nothing, Optional ByRef vParamArray As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set Property values.
            With m_dSIRParty

                vPartyCnt = .PartyCnt.ToString

                vPartyTypeID = .PartyTypeID.ToString

                If Convert.IsDBNull(.IsAlsoAgent) Or Informations.IsNothing(.IsAlsoAgent) Then
                    vIsAlsoAgent = 0.ToString
                Else
                    vIsAlsoAgent = .IsAlsoAgent.ToString
                End If

                vPartyStructureID = .PartyStructureID.ToString
                vSourceID = .SourceID.ToString
                vPartyID = .PartyID.ToString
                vShortname = .Shortname.ToString
                vName = .Name.ToString

                If Convert.IsDBNull(.ResolvedName) Or Informations.IsNothing(.ResolvedName) Then
                    vResolvedName = ""
                Else
                    vResolvedName = .ResolvedName.ToString
                End If

                vCurrencyId = .CurrencyID.ToString
                vLanguageID = .LanguageID.ToString

                If Convert.IsDBNull(.CollectTypeID) Or Informations.IsNothing(.CollectTypeID) Then
                    vCollectTypeID = 0.ToString
                Else
                    vCollectTypeID = .CollectTypeID.ToString
                End If

                If Convert.IsDBNull(.AccumTreatmentTypeID) Or Informations.IsNothing(.AccumTreatmentTypeID) Then
                    vAccumTreatmentTypeID = 0.ToString
                Else
                    vAccumTreatmentTypeID = .AccumTreatmentTypeID.ToString
                End If

                If Convert.IsDBNull(.StatsTreatmentTypeID) Or Informations.IsNothing(.StatsTreatmentTypeID) Then
                    vStatsTreatmentTypeID = 0.ToString
                Else
                    vStatsTreatmentTypeID = .StatsTreatmentTypeID.ToString
                End If

                If Convert.IsDBNull(.PartyCategoryID) Or Informations.IsNothing(.PartyCategoryID) Then
                    vPartyCategoryID = 0.ToString
                Else
                    'Developer Guide No. 229(latest guide)
                    vPartyCategoryID = .PartyCategoryID.ToString
                End If
                'Developer Guide No. 143
                If Convert.IsDBNull(.AgentCnt) Or Informations.IsNothing(.AgentCnt) Then
                    'Developer Guide No. 229(latest guide)
                    vAgentCnt = 0.ToString
                Else
                    'Developer Guide No. 229(latest guide)
                    vAgentCnt = .AgentCnt.ToString
                End If
                'Developer Guide No. 143
                If Convert.IsDBNull(.ConsultantCnt) Or Informations.IsNothing(.ConsultantCnt) Then
                    'Developer Guide No. 229(latest guide)
                    vConsultantCnt = 0.ToString
                Else
                    'Developer Guide No. 229(latest guide)
                    vConsultantCnt = .ConsultantCnt.ToString
                End If
                'Developer Guide No. 143
                'Developer Guide No. 229(latest guide)
                vCreatedByID = .CreatedByID.ToString
                'Developer Guide No. 143
                'Developer Guide No. 229(latest guide)
                vDateCreated = .DateCreated.ToString

                'Developer Guide No. 143
                If Convert.IsDBNull(.LastModified) Or Informations.IsNothing(.LastModified) Then
                    'Developer Guide No. 229(latest guide)
                    vLastModified = 0.ToString
                Else
                    'Developer Guide No. 229(latest guide)
                    vLastModified = .LastModified.ToString
                End If
                'Developer Guide No. 143
                If Convert.IsDBNull(.ModifiedByID) Or Informations.IsNothing(.ModifiedByID) Then
                    'Developer Guide No. 229(latest guide)
                    vModifiedByID = 0.ToString
                Else
                    'Developer Guide No. 229(latest guide)
                    vModifiedByID = .ModifiedByID.ToString
                End If
                'Developer Guide No. 143
                If Convert.IsDBNull(.PaymentMethodCode) Or Informations.IsNothing(.PaymentMethodCode) Then
                    vPaymentMethodCode = ""
                Else
                    'Developer Guide No. 229(latest guide)
                    vPaymentMethodCode = .PaymentMethodCode.ToString
                End If
                'Developer Guide No. 143
                If Convert.IsDBNull(.PaymentTermCode) Or Informations.IsNothing(.PaymentTermCode) Then
                    vPaymentTermCode = 0
                Else
                    'Developer Guide No. 229(latest guide)
                    vPaymentTermCode = gPMFunctions.ToSafeInteger(.PaymentTermCode)
                End If
                'End If

                'Developer Guide No 143
                If Convert.IsDBNull(.CreditCardCode) Or Informations.IsNothing(.CreditCardCode) Then
                    vCreditCardCode = ""
                Else
                    'Developer Guide No. 229(latest guide)
                    vCreditCardCode = .CreditCardCode.ToString
                End If
                'End If

                'Developer Guide No 143
                If Convert.IsDBNull(.FileCode) Or Informations.IsNothing(.FileCode) Then
                    vFileCode = ""
                Else
                    'Developer Guide No. 229(latest guide)
                    vFileCode = .FileCode.ToString
                End If
                'End If

                'Developer Guide No 143
                If Convert.IsDBNull(.ABCCount) Or Informations.IsNothing(.ABCCount) Then
                    'Developer Guide No. 229(latest guide)
                    vABCCount = 0.ToString
                Else
                    'Developer Guide No. 229(latest guide)
                    vABCCount = .ABCCount.ToString
                End If
                'Developer Guide No 143
                If Convert.IsDBNull(.Statements) Or Informations.IsNothing(.Statements) Then
                    'Developer Guide No. 229(latest guide)
                    vStatements = 0.ToString
                Else
                    'Developer Guide No. 229(latest guide)
                    vStatements = .Statements.ToString
                End If
                'End If

                'Developer Guide No 143
                If Convert.IsDBNull(.ReminderTypeId) Or Informations.IsNothing(.ReminderTypeId) Then
                    'Developer Guide No. 229(latest guide)
                    vReminderTypeId = 0.ToString
                Else
                    'Developer Guide No. 229(latest guide)
                    vReminderTypeId = .ReminderTypeId.ToString
                End If
                'Developer Guide No 143
                If Convert.IsDBNull(.Renewals) Or Informations.IsNothing(.Renewals) Then
                    'Developer Guide No. 229(latest guide)
                    vRenewals = 0.ToString
                Else
                    'Developer Guide No. 229(latest guide)
                    vRenewals = .Renewals.ToString

                End If
                'End If

                'Developer Guide No 143
                If Convert.IsDBNull(.Status) Or Informations.IsNothing(.Status) Then
                    vStatus = ""
                Else
                    'Developer Guide No. 229(latest guide)
                    vStatus = .Status.ToString
                End If
                'End If

                'Developer Guide No 143
                If Convert.IsDBNull(.LastActionType) Or Informations.IsNothing(.LastActionType) Then
                    vLAstActionType = ""
                Else
                    'Developer Guide No. 229(latest guide)
                    vLAstActionType = .LastActionType.ToString
                End If
                'Developer Guide No 143
                If Convert.IsDBNull(.IsTravelAgent) Or Informations.IsNothing(.IsTravelAgent) Then
                    'Developer Guide No. 229(latest guide)
                    vIsTravelAgent = 0.ToString
                Else
                    'Developer Guide No. 229(latest guide)
                    vIsTravelAgent = .IsTravelAgent.ToString
                End If
                'Developer Guide No 143
                If Convert.IsDBNull(.IsProspect) Or Informations.IsNothing(.IsProspect) Then
                    'Developer Guide No. 229(latest guide)
                    vIsProspect = 0.ToString
                Else
                    'Developer Guide No. 229(latest guide)
                    vIsProspect = .IsProspect.ToString
                End If
                'Developer Guide No 143
                If Convert.IsDBNull(.IsDeleted) Or Informations.IsNothing(.IsDeleted) Then
                    'Developer Guide No. 229(latest guide)
                    vIsDeleted = 0.ToString
                Else
                    'Developer Guide No. 229(latest guide)
                    vIsDeleted = .IsDeleted.ToString
                End If
                'Developer Guide No 143
                If Convert.IsDBNull(.ABICodeOn406) Or Informations.IsNothing(.ABICodeOn406) Then
                    vABICodeOn406 = ""
                Else
                    'Developer Guide No. 229(latest guide)
                    vABICodeOn406 = .ABICodeOn406.ToString
                End If
                'Developer Guide No 143
                If Convert.IsDBNull(.ABICodeOn81) Or Informations.IsNothing(.ABICodeOn81) Then
                    vABICodeOn81 = ""
                Else
                    'Developer Guide No. 229(latest guide)
                    vABICodeOn81 = .ABICodeOn81.ToString
                End If
                'End If

                'Developer Guide No 143
                If Convert.IsDBNull(.ABICodeList) Or Informations.IsNothing(.ABICodeList) Then
                    vABICodeList = ""
                Else
                    'Developer Guide No. 229(latest guide)
                    vABICodeList = .ABICodeList.ToString
                End If
                'Developer Guide No 143
                If Convert.IsDBNull(.AreaId) Or Informations.IsNothing(.AreaId) Then
                    'Developer Guide No. 229(latest guide)
                    vAreaId = 0.ToString
                Else
                    'Developer Guide No. 229(latest guide)
                    vAreaId = .AreaId.ToString

                End If
                'End If

                'Developer Guide No 143
                If Convert.IsDBNull(.ServiceLevelId) Or Informations.IsNothing(.ServiceLevelId) Then
                    'Developer Guide No. 229(latest guide)
                    vServiceLevelId = 0.ToString
                Else
                    'Developer Guide No. 229(latest guide)
                    vServiceLevelId = .ServiceLevelId.ToString
                End If
                'Developer Guide No 143

                If Convert.IsDBNull(.InvariantKey) Or Informations.IsNothing(.InvariantKey) Then
                    'Developer Guide No. 229(latest guide)
                    vInvariantKey = 0.ToString
                Else
                    'Developer Guide No. 229(latest guide)
                    vInvariantKey = .InvariantKey.ToString
                End If
                'End If


                'Developer Guide No 143

                If Convert.IsDBNull(.RecordStatus) Or Informations.IsNothing(.RecordStatus) Then
                    vRecordStatus = ""
                Else
                    'Developer Guide No. 229(latest guide)
                    vRecordStatus = .RecordStatus.ToString
                End If
                'Developer Guide No 143

                If Convert.IsDBNull(.CCJs) Or Informations.IsNothing(.CCJs) Then
                    'Developer Guide No. 229(latest guide)
                    vCCJs = 0.ToString
                Else
                    'Developer Guide No. 229(latest guide)
                    vCCJs = .CCJs.ToString

                End If
                'End If


                'Developer Guide No 143

                If Convert.IsDBNull(.SeasonalGiftID) Or Informations.IsNothing(.SeasonalGiftID) Then
                    'Developer Guide No. 229(latest guide)
                    vSeasonalGiftID = 0.ToString

                Else
                    'Developer Guide No. 229(latest guide)
                    vSeasonalGiftID = .SeasonalGiftID.ToString
                End If
                'DC 28/06/00

                'Developer Guide No 143

                If Convert.IsDBNull(.CorrespondenceTypeId) Or Informations.IsNothing(.CorrespondenceTypeId) Then
                    'Developer Guide No. 229(latest guide)
                    vCorrespondenceTypeId = 0.ToString
                Else
                    'Developer Guide No. 229(latest guide)
                    vCorrespondenceTypeId = .CorrespondenceTypeId.ToString
                End If

                'Tomo060700

                'Developer Guide No 143

                If Convert.IsDBNull(.RenewalStopCodeId) Or Informations.IsNothing(.RenewalStopCodeId) Then
                    'Developer Guide No. 229(latest guide)
                    vRenewalStopCodeId = 0.ToString
                Else
                    'Developer Guide No. 229(latest guide)
                    vRenewalStopCodeId = .RenewalStopCodeId.ToString
                End If
                'Developer Guide No 143

                If Convert.IsDBNull(.SwiftPartyID) Or Informations.IsNothing(.SwiftPartyID) Then
                    'Developer Guide No. 229(latest guide)
                    vSwiftPartyId = 0.ToString
                Else
                    'Developer Guide No. 229(latest guide)
                    vSwiftPartyId = .SwiftPartyID.ToString
                End If

                If Convert.IsDBNull(.LoyaltyNumber) Or Informations.IsNothing(.LoyaltyNumber) Then
                    vLoyaltyNumber = ""
                Else
                    vLoyaltyNumber = .LoyaltyNumber.ToString
                End If

                If Convert.IsDBNull(.AlternativeIdentifier) Or Informations.IsNothing(.AlternativeIdentifier) Then
                    vAlternativeIdentifier = ""
                Else
                    vAlternativeIdentifier = .AlternativeIdentifier.ToString
                End If

                If Convert.IsDBNull(.MarketingSegmentInd) Or Informations.IsNothing(.MarketingSegmentInd) Then
                    vMarketingSegmentInd = ""
                Else
                    vMarketingSegmentInd = .MarketingSegmentInd.ToString
                End If

                If Convert.IsDBNull(.TradingName) Or Informations.IsNothing(.TradingName) Then
                    vTradingName = ""
                Else
                    vTradingName = .TradingName.ToString
                End If

                If Convert.IsDBNull(.SubBranchId) Or Informations.IsNothing(.SubBranchId) Then
                    vSubBranchId = 0.ToString
                Else
                    vSubBranchId = .SubBranchId.ToString
                End If


                If Convert.IsDBNull(.SubBranchName) Or Informations.IsNothing(.SubBranchName) Then
                    vSubBranchName = ""
                Else
                    vSubBranchName = .SubBranchName.ToString
                End If

                vTobLetter = .TobLetter.ToString

                vOverrideCommission = .Override.ToString
                vOverrideCommissionRenewal = .OverrideRenewal.ToString
                iStatus = m_iDatabaseStatus

                If (Informations.IsNothing(vParamArray) = False) Then

                    'Initialize array
                    ReDim vParamArray(2)

                    ' vParamArray(AC_PARTY_IsBordereauxAccount) = .IsBordereauxAccount
                    'vParamArray(AC_PARTY_PremiumManagerId) = .PremiumManagerId
                End If
            End With

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, gPMConstants.PMELogLevel.PMLogOnError, "GetProperties Failed", ACApp, ACClass, "GetProperties", Informations.Err().Number, excep.Message, excep:=excep)
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

            With m_dSIRParty

                ' Set Data object primary key
                .PartyCnt = PartyCnt

                'Adn if we're from events
                .FromEvent = FromEvent

                ' Select a record from the database
                m_lReturn = .SelectSingle()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
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

            With m_dSIRParty

                ' Add a record to the database from the object
                m_lReturn = .Add()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Retain the Primary Key of the SIRParty Added
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

            With m_dSIRParty

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

            With m_dSIRParty

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
    ' Description: Sets the Default Values for a SIRParty.
    '
    ' ***************************************************************** '
    'DC 28/06/00 Added Correspondence Type Id
    'sj 12/06/2002 - Added LoyaltyNumber, AlternativeIdentifier,
    '                MarketingSegmentInd, TradingName and SubBranchId
    'SD 17/07/2002 - Added SubBranchName
    'FSA Phase III TobLetter
    'Developer Guide No. 101
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vPartyCnt As Object = 0, Optional ByRef vPartyTypeID As Object = 0, Optional ByRef vPartyStructureID As Object = 0, Optional ByRef vSourceID As Integer = 0, Optional ByRef vIsAlsoAgent As Object = 0, Optional ByRef vPartyID As Object = 0, Optional ByRef vShortname As String = "", Optional ByRef vName As String = "", Optional ByRef vResolvedName As String = "", Optional ByRef vCurrencyId As Integer = 0, Optional ByRef vLanguageID As Integer = 0, Optional ByRef vCollectTypeID As Object = 0, Optional ByRef vAccumTreatmentTypeID As Object = 0, Optional ByRef vStatsTreatmentTypeID As Object = 0, Optional ByRef vPartyCategoryID As Object = 0, Optional ByRef vAgentCnt As Object = 0, Optional ByRef vConsultantCnt As Object = 0, Optional ByRef vCreatedByID As Integer = 0, Optional ByRef vDateCreated As Date = #12/30/1899#, Optional ByRef vLastModified As Date = #12/30/1899#, Optional ByRef vModifiedByID As Integer = 0, Optional ByRef vPaymentMethodCode As String = "", Optional ByRef vPaymentTermCode As String = "", Optional ByRef vCreditCardCode As String = "", Optional ByRef vFileCode As String = "", Optional ByRef vABCCount As Object = 0, Optional ByRef vStatements As Object = 0, Optional ByRef vReminderTypeId As Object = 0, Optional ByRef vRenewals As Object = 0, Optional ByRef vStatus As String = "", Optional ByRef vLAstActionType As String = "", Optional ByRef vIsTravelAgent As Object = 0, Optional ByRef vIsProspect As Object = 0, Optional ByRef vIsDeleted As Object = 0, Optional ByRef vABICodeOn406 As String = "", Optional ByRef vABICodeOn81 As String = "", Optional ByRef vABICodeList As String = "", Optional ByRef vAreaId As Object = 0, Optional ByRef vServiceLevelId As Object = 0, Optional ByRef vInvariantKey As Object = 0, Optional ByRef vRecordStatus As String = "", Optional ByRef vCCJs As Object = 0, Optional ByRef vUserDefinedDataId As Object = Nothing, Optional ByRef vSeasonalGiftID As Object = Nothing, Optional ByRef vCorrespondenceTypeId As Object = Nothing, Optional ByRef vRenewalStopCodeId As Object = Nothing, Optional ByRef vSwiftPartyId As Object = Nothing, Optional ByRef vLoyaltyNumber As Object = Nothing, Optional ByRef vAlternativeIdentifier As Object = Nothing, Optional ByRef vMarketingSegmentInd As Object = Nothing, Optional ByRef vTradingName As Object = Nothing, Optional ByRef vSubBranchId As Object = Nothing, Optional ByRef vSubBranchName As Object = Nothing, Optional ByRef vTobLetter As Object = Nothing, Optional ByRef vOverrideCommission As Object = Nothing, Optional ByRef vOverrideCommissionRenewal As Object = Nothing) As Integer


        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}

        'Developer Guide No. 44 
        If (Informations.IsNothing(vPartyCnt)) OrElse (vPartyCnt.Equals(0)) Or (bDefaultAll) Then
            vPartyCnt = 0
        End If


        'Developer Guide No. 44 
        If (Informations.IsNothing(vPartyTypeID)) OrElse (vPartyTypeID.Equals(0)) Or (bDefaultAll) Then
            vPartyTypeID = 0
        End If


        'Developer Guide No. 44
        If (Informations.IsNothing(vIsAlsoAgent)) OrElse (vIsAlsoAgent.Equals(0)) Or (bDefaultAll) Then
            vIsAlsoAgent = 0
        End If




        'Developer Guide No. 44
        If (Informations.IsNothing(vPartyStructureID)) OrElse (vPartyStructureID.Equals(0)) Or (bDefaultAll) Then
            'sp todo - review this - it should be a constant or reg setting
            vPartyStructureID = 1
        End If

        'Developer Guide No. 44 
        If (Informations.IsNothing(vSourceID)) OrElse (vSourceID.Equals(0)) Or (bDefaultAll) Then
            vSourceID = m_iSourceID
        End If


        'Developer Guide No. 44
        If (Informations.IsNothing(vPartyID)) OrElse (vPartyID.Equals(0)) Or (bDefaultAll) Then
            vPartyID = 0
        End If


        'Developer Guide No. 44
        If (Informations.IsNothing(vShortname)) Or (String.IsNullOrEmpty(vShortname)) Or (bDefaultAll) Then
            vShortname = ""
        End If



        'Developer Guide No. 44
        If (Informations.IsNothing(vName)) Or (String.IsNullOrEmpty(vName)) Or (bDefaultAll) Then
            vName = ""
        End If


        'Developer Guide No. 44
        If (Informations.IsNothing(vResolvedName)) Or (String.IsNullOrEmpty(vResolvedName)) Or (bDefaultAll) Then
            vResolvedName = ""
        End If
        'eck010600


        'Developer Guide No. 44
        If (Informations.IsNothing(vCurrencyId)) OrElse (vCurrencyId.Equals(0)) Or (bDefaultAll) Then
            vCurrencyId = m_iCurrencyID
        Else
            If vCurrencyId = 0 Then
                vCurrencyId = m_iCurrencyID
            End If
        End If


        'Developer Guide No. 44
        If (Informations.IsNothing(vLanguageID)) OrElse (vLanguageID.Equals(0)) Or (bDefaultAll) Then
            vLanguageID = m_iLanguageID
        End If


        'Developer Guide No. 44
        If (Informations.IsNothing(vCollectTypeID)) OrElse (vCollectTypeID.Equals(0)) Or (bDefaultAll) Then
            vCollectTypeID = 0
        End If


        'Developer Guide No. 44
        If (Informations.IsNothing(vAccumTreatmentTypeID)) OrElse (vAccumTreatmentTypeID.Equals(0)) Or (bDefaultAll) Then
            vAccumTreatmentTypeID = 0
        End If


        'Developer Guide No. 44
        If (Informations.IsNothing(vStatsTreatmentTypeID)) OrElse (vStatsTreatmentTypeID.Equals(0)) Or (bDefaultAll) Then
            vStatsTreatmentTypeID = 0
        End If


        'Developer Guide No. 44
        If (Informations.IsNothing(vPartyCategoryID)) OrElse (vPartyCategoryID.Equals(0)) Or (bDefaultAll) Then
            vPartyCategoryID = 0
        End If



        'Developer Guide No. 44
        If (Informations.IsNothing(vAgentCnt)) OrElse (vAgentCnt.Equals(0)) Or (bDefaultAll) Then
            vAgentCnt = 0
        End If


        'Developer Guide No. 44
        If (Informations.IsNothing(vConsultantCnt)) OrElse (vConsultantCnt.Equals(0)) Or (bDefaultAll) Then
            vConsultantCnt = 0
        End If


        'Developer Guide No. 44
        If (Informations.IsNothing(vCreatedByID)) OrElse (vCreatedByID.Equals(0)) Or (bDefaultAll) Then
            vCreatedByID = m_iUserID
        End If


        'Developer Guide No. 44
        If (Informations.IsNothing(vDateCreated)) OrElse (vDateCreated.Equals(DateTime.FromOADate(0))) Or (bDefaultAll) Then
            vDateCreated = DateTime.Now
        End If


        'Developer Guide No. 44
        If (Informations.IsNothing(vLastModified)) OrElse (vLastModified.Equals(DateTime.FromOADate(0))) Or (bDefaultAll) Then
            vLastModified = DateTime.Now
        End If

        'Developer Guide No. 44
        If (Informations.IsNothing(vModifiedByID)) OrElse (vModifiedByID.Equals(0)) Or (bDefaultAll) Then
            vModifiedByID = m_iUserID
        End If


        'Developer Guide No. 44
        If (Informations.IsNothing(vPaymentMethodCode)) Or (bDefaultAll) Then
            vPaymentMethodCode = "Cash"
        End If


        'Developer Guide No. 44
        If (Informations.IsNothing(vPaymentTermCode)) Or (bDefaultAll) Then
            vPaymentTermCode = "3"
        End If


        'Developer Guide No. 44
        If (Informations.IsNothing(vCreditCardCode)) Or (String.IsNullOrEmpty(vCreditCardCode)) Or (bDefaultAll) Then
            vCreditCardCode = ""
        End If


        'Developer Guide No. 44
        If (Informations.IsNothing(vFileCode)) Or (String.IsNullOrEmpty(vFileCode)) Or (bDefaultAll) Then
            vFileCode = ""
        End If


        'Developer Guide No. 44
        If (Informations.IsNothing(vABCCount)) OrElse (vABCCount.Equals(0)) Or (bDefaultAll) Then
            vABCCount = 0
        End If



        'Developer Guide No. 44
        If (Informations.IsNothing(vStatements)) OrElse (vStatements.Equals(0)) Or (bDefaultAll) Then
            vStatements = 0
        End If


        'Developer Guide No. 44
        If (Informations.IsNothing(vStatements)) OrElse (vStatements.Equals(0)) Or (bDefaultAll) Then
            vStatements = 0
        End If


        'Developer Guide No. 44
        If (Informations.IsNothing(vRenewals)) OrElse (vRenewals.Equals(0)) Or (bDefaultAll) Then
            vRenewals = 0
        End If


        'Developer Guide No. 44
        If (Informations.IsNothing(vStatus)) Or (String.IsNullOrEmpty(vStatus)) Or (bDefaultAll) Then
            vStatus = ""
        End If


        'Developer Guide No. 44
        If (Informations.IsNothing(vLAstActionType)) Or (String.IsNullOrEmpty(vLAstActionType)) Or (bDefaultAll) Then
            vLAstActionType = ""
        End If


        'Developer Guide No. 44
        If (Informations.IsNothing(vIsTravelAgent)) OrElse (vIsTravelAgent.Equals(0)) Or (bDefaultAll) Then
            vIsTravelAgent = 0
        End If


        'Developer Guide No. 44
        If (Informations.IsNothing(vIsProspect)) OrElse (vIsProspect.Equals(0)) Or (bDefaultAll) Then
            vIsProspect = 0
        End If


        'Developer Guide No. 44
        If (Informations.IsNothing(vIsDeleted)) OrElse (vIsDeleted.Equals(0)) Or (bDefaultAll) Then
            vIsDeleted = 0
        End If


        'Developer Guide No. 44
        If (Informations.IsNothing(vABICodeOn406)) Or (String.IsNullOrEmpty(vABICodeOn406)) Or (bDefaultAll) Then
            vABICodeOn406 = ""
        End If


        'Developer Guide No. 44
        If (Informations.IsNothing(vABICodeOn81)) Or (String.IsNullOrEmpty(vABICodeOn81)) Or (bDefaultAll) Then
            vABICodeOn81 = ""
        End If


        'Developer Guide No. 44
        If (Informations.IsNothing(vABICodeList)) Or (String.IsNullOrEmpty(vABICodeList)) Or (bDefaultAll) Then
            vABICodeList = ""
        End If


        'Developer Guide No. 44
        If (Informations.IsNothing(vAreaId)) OrElse (vAreaId.Equals(0)) Or (bDefaultAll) Then
            vAreaId = 0
        End If


        'Developer Guide No. 44
        If (Informations.IsNothing(vServiceLevelId)) OrElse (vServiceLevelId.Equals(0)) Or (bDefaultAll) Then
            vServiceLevelId = 0
        End If


        'Developer Guide No. 44
        If (Informations.IsNothing(vInvariantKey)) OrElse (vInvariantKey.Equals(0)) Or (bDefaultAll) Then
            vInvariantKey = 0
        End If


        'Developer Guide No. 44
        If (Informations.IsNothing(vRecordStatus)) Or (String.IsNullOrEmpty(vRecordStatus)) Or (bDefaultAll) Then
            vRecordStatus = ""
        End If


        'Developer Guide No. 44
        If (Informations.IsNothing(vCCJs)) OrElse (vCCJs.Equals(0)) Or (bDefaultAll) Then
            vCCJs = 0
        End If
        'MSS200901 - Changed from vbNull to Null.
        '               vbNull is vBad....actually equals 1 (which ain't null)
        'eck010600

        'Developer Guide No. 44
        If (Informations.IsNothing(vSeasonalGiftID)) Or (Object.Equals(vSeasonalGiftID, Nothing)) Or (bDefaultAll) Then


            vSeasonalGiftID = DBNull.Value
        End If

        'DC 28/06/00

        'Developer Guide No. 44
        If (Informations.IsNothing(vCorrespondenceTypeId)) Or (Object.Equals(vCorrespondenceTypeId, Nothing)) Or (bDefaultAll) Then


            vCorrespondenceTypeId = DBNull.Value
        End If

        'RWH 07/07/2000

        'Developer Guide No. 44
        If (Informations.IsNothing(vRenewalStopCodeId)) Or (Object.Equals(vRenewalStopCodeId, Nothing)) Or (bDefaultAll) Then


            vRenewalStopCodeId = DBNull.Value
        End If

        ' CTAF 250900

        'Developer Guide No. 44
        If (Informations.IsNothing(vSwiftPartyId)) Or (Object.Equals(vSwiftPartyId, Nothing)) Or (bDefaultAll) Then


            vSwiftPartyId = DBNull.Value
        End If


        'Developer Guide No. 44
        If (Informations.IsNothing(vSwiftPartyId)) Or (Object.Equals(vSwiftPartyId, Nothing)) Or (bDefaultAll) Then


            vSwiftPartyId = DBNull.Value
        End If

        'sj 12/06/2002 - start

        'Developer Guide No. 44
        If (Informations.IsNothing(vLoyaltyNumber)) Or (Object.Equals(vLoyaltyNumber, Nothing)) Or (bDefaultAll) Then


            vLoyaltyNumber = DBNull.Value
        End If


        'Developer Guide No. 44
        If (Informations.IsNothing(vAlternativeIdentifier)) Or (Object.Equals(vAlternativeIdentifier, Nothing)) Or (bDefaultAll) Then


            vAlternativeIdentifier = DBNull.Value
        End If


        'Developer Guide No. 44
        If (Informations.IsNothing(vMarketingSegmentInd)) Or (Object.Equals(vMarketingSegmentInd, Nothing)) Or (bDefaultAll) Then


            vMarketingSegmentInd = DBNull.Value
        End If


        'Developer Guide No. 44
        If (Informations.IsNothing(vTradingName)) Or (Object.Equals(vTradingName, Nothing)) Or (bDefaultAll) Then


            vTradingName = DBNull.Value
        End If

        'Developer Guide No. 44
        If (Informations.IsNothing(vSubBranchId)) Or (Object.Equals(vSubBranchId, Nothing)) Or (bDefaultAll) Then


            vSubBranchId = DBNull.Value
        End If

        'Developer Guide No. 44
        If (Informations.IsNothing(vSubBranchName)) Or (Object.Equals(vSubBranchName, Nothing)) Or (bDefaultAll) Then


            vSubBranchName = DBNull.Value
        End If
        'FSA Phase III

        'Developer Guide No. 44
        If (Informations.IsNothing(vTobLetter)) Or (Object.Equals(vTobLetter, Nothing)) Or (bDefaultAll) Then


            vTobLetter = DBNull.Value
        End If
        'FSA Phase III End


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

