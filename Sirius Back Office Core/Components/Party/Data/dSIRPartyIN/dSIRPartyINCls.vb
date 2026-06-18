Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
'developer guide no. 129
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("SIRPartyIN_NET.SIRPartyIN")>
Public NotInheritable Class SIRPartyIN
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: partyinsurer
    '
    ' Date: 25/06/1999
    '
    ' Description: Describes the partyinsurer attributes.
    '
    ' Edit History:
    ' RAW 18/12/2002 : PS187 : Added new data items (WHTaxType, WHTaxRate, TaxRegNo, TaxCode, PaymentMethod, PaymentFrequency , BankAccount)
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 06/02/2004
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
    Private Const ACClass As String = "partyinsurer"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag
    Private m_bCloseDatabase As Boolean

    ' DataBase Attributes
    Private m_lPartyCnt As Object
    Private m_vAgencyNumber As Object
    Private m_vBinderIndicator As Object
    Private m_vReportIndicator As Object
    Private m_iIsReinsurer As Integer
    Private m_vReinsuranceType As Object
    Private m_iIsReinsuranceDebitCreditNo As Integer
    Private m_vDefaultCommRate As Object
    Private m_vTaxGroupID As Object
    Private m_vPaymentMethod As Object
    Private m_vPaymentFrequency As Object
    Private m_vBankAccount As Object = Nothing
    Private m_vFSAInsurerStatus As Object
    Private m_vFSARegistrationNumber As Object = Nothing
    Private m_vFSAInsurerCreditRating As Object

    Private m_vIsRetained As Object = False
    'ECK Datasure 10102005 Claims Rating Agency
    Private m_vClaimsRatingAgencyId As Object
    Private m_vClaimsRatingGrading As Object = Nothing
    Private m_vClaimsRatingDate As Date
    Private m_vClaimsRatingDescription As Object = Nothing 'S4BDAT005 Datasure - Claims Rating Agency Description
    'S4BDAT004 Datasure
    Private m_vTermsOfPaymentId As Object
    ' Function Return Code
    'DC0050706 fix issue where tax flag not set correctly
    Private m_vDomiciledForTax As Boolean
    Private m_vRiskTransferAgreement As Object
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_vBrokerlinkSubAccount As Object
    Private m_vBrokerlinkUnderwritingID As Object
    'QBENZ005
    Private m_iIsRIBroker As Object
    Private m_vRiskTransferEditable As Object
    'Devlopment work on Inssurer Payment Locking
    Private m_vCboLockingTypeId As Object
    'PYV Development
    Private m_vInsurerTypeId As Object
    Private m_vBureauAccountParty As Object
    Private m_sUniqueId As String
    Private m_sScreenHierarchy As String

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


    Public Property PartyCnt() As Object
        Get

            Return m_lPartyCnt

        End Get
        Set(ByVal Value As Object)

            m_lPartyCnt = Value

        End Set
    End Property

    Public Property AgencyNumber() As Object
        Get

            Return m_vAgencyNumber

        End Get
        Set(ByVal Value As Object)



            m_vAgencyNumber = Value

        End Set
    End Property

    Public Property BinderIndicator() As Object
        Get

            Return m_vBinderIndicator

        End Get
        Set(ByVal Value As Object)



            m_vBinderIndicator = Value

        End Set
    End Property

    Public Property ReportIndicator() As Object
        Get

            Return m_vReportIndicator

        End Get
        Set(ByVal Value As Object)



            m_vReportIndicator = Value

        End Set
    End Property

    Public Property IsReinsurer() As Integer
        Get

            Return m_iIsReinsurer

        End Get
        Set(ByVal Value As Integer)

            m_iIsReinsurer = Value

        End Set
    End Property

    Public Property ReinsuranceType() As Object
        Get

            Return m_vReinsuranceType

        End Get
        Set(ByVal Value As Object)



            m_vReinsuranceType = Value

        End Set
    End Property

    Public Property IsReinsuranceDebitCreditNo() As Object
        Get

            Return m_iIsReinsuranceDebitCreditNo

        End Get
        Set(ByVal Value As Object)

            m_iIsReinsuranceDebitCreditNo = Value

        End Set
    End Property

    Public Property DefaultCommRate() As Object
        Get

            Return m_vDefaultCommRate

        End Get
        Set(ByVal Value As Object)



            m_vDefaultCommRate = Value

        End Set
    End Property

    Public Property TaxGroupID() As Object
        Get
            Return m_vTaxGroupID
        End Get
        Set(ByVal Value As Object)

            m_vTaxGroupID = Value
        End Set
    End Property

    Public Property PaymentMethod() As Object
        Get
            Return m_vPaymentMethod
        End Get
        Set(ByVal Value As Object)

            m_vPaymentMethod = Value
        End Set
    End Property

    Public Property PaymentFrequency() As Object
        Get
            Return m_vPaymentFrequency
        End Get
        Set(ByVal Value As Object)

            m_vPaymentFrequency = Value
        End Set
    End Property

    Public Property BankAccount() As Object
        Get
            Return m_vBankAccount
        End Get
        Set(ByVal Value As Object)

            m_vBankAccount = Value
        End Set
    End Property

    Public Property FSAInsurerStatus() As Object
        Get

            Return m_vFSAInsurerStatus

        End Get
        Set(ByVal Value As Object)


            m_vFSAInsurerStatus = Value

        End Set
    End Property

    Public Property FSARegistrationNumber() As Object
        Get

            Return m_vFSARegistrationNumber

        End Get
        Set(ByVal Value As Object)


            m_vFSARegistrationNumber = Value

        End Set
    End Property
    'DC150803 -PS254 -fsa compliance
    Public Property FSAInsurerCreditRating() As Object
        Get

            Return m_vFSAInsurerCreditRating

        End Get
        Set(ByVal Value As Object)


            m_vFSAInsurerCreditRating = Value

        End Set
    End Property
    ' PUBLIC Property Procedures (End)
    Public Property IsRetained() As Object
        Get
            Return m_vIsRetained
        End Get
        Set(ByVal Value As Object)


            m_vIsRetained = Value

        End Set
    End Property
    'ECK Datasure 10102005 Claims Rating Agency
    Public Property ClaimsRatingAgencyId() As Object
        Get
            Return m_vClaimsRatingAgencyId
        End Get
        Set(ByVal Value As Object)

            m_vClaimsRatingAgencyId = Value
        End Set
    End Property
    Public Property ClaimsRatingGrading() As Object
        Get
            Return m_vClaimsRatingGrading
        End Get
        Set(ByVal Value As Object)

            m_vClaimsRatingGrading = Value
        End Set
    End Property
    Public Property ClaimsRatingDate() As Object
        Get
            Return m_vClaimsRatingDate
        End Get
        Set(ByVal Value As Object)

            m_vClaimsRatingDate = Value
        End Set
    End Property
    Public Property ClaimsRatingDescription() As Object
        Get
            Return m_vClaimsRatingDescription
        End Get
        Set(ByVal Value As Object)

            m_vClaimsRatingDescription = Value
        End Set
    End Property
    Public Property TermsOfPaymentId() As Object
        Get
            Return m_vTermsOfPaymentId
        End Get
        Set(ByVal Value As Object)

            m_vTermsOfPaymentId = Value
        End Set
    End Property
    'DC0050706 fix issue where tax flag not set correctly
    Public Property DomiciledForTax() As Object
        Get
            Return m_vDomiciledForTax
        End Get
        Set(ByVal Value As Object)

            m_vDomiciledForTax = Value
        End Set
    End Property
    Public Property CboLockingTypeId() As Object
        Get
            Return m_vCboLockingTypeId
        End Get
        Set(ByVal Value As Object)

            m_vCboLockingTypeId = Value
        End Set
    End Property

    Public Property RiskTransferEditable() As Object
        Get
            Return m_vRiskTransferEditable
        End Get
        Set(ByVal Value As Object)

            m_vRiskTransferEditable = Value
        End Set
    End Property
    'PYV Development
    Public Property InsurerTypeId() As Object
        Get
            Return m_vInsurerTypeId
        End Get
        Set(ByVal Value As Object)

            m_vInsurerTypeId = Value
        End Set
    End Property
    Public Property BureauAccountParty() As Object
        Get
            Return m_vBureauAccountParty
        End Get
        Set(ByVal Value As Object)

            If CDbl(Value) = 0 Then


                'developer guide no. 85 (Latest Guide)
                Value = Nothing
            End If

            m_vBureauAccountParty = Value
        End Set
    End Property


    Public Property RiskTransferAgreement() As Object
        Get
            Return m_vRiskTransferAgreement
        End Get
        Set(ByVal Value As Object)

            Dim dbNumericTemp As Double
            If Double.TryParse(CStr(Value), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                m_vRiskTransferAgreement = Value
            End If
        End Set
    End Property

    Public Property BrokerlinkSubAccount() As Object
        Get
            Return m_vBrokerlinkSubAccount
        End Get
        Set(ByVal Value As Object)


            m_vBrokerlinkSubAccount = Value
        End Set
    End Property

    Public Property BrokerlinkUnderwritingID() As Object
        Get
            Return m_vBrokerlinkUnderwritingID
        End Get
        Set(ByVal Value As Object)


            m_vBrokerlinkUnderwritingID = Value
        End Set
    End Property
    'QBENZ005

    Public Property IsRIBroker() As Object
        Get
            Return m_iIsRIBroker
        End Get
        Set(ByVal Value As Object)

            m_iIsRIBroker = Value
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




            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)


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

                ' Add PrimaryKey as OUTPUT parameters
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

                ' Add PrimaryKey as INPUT parameters
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
    ' Description: Selects the required partyinsurer
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
                'developer guide no.162
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
    ' Description: Sets the supplied partyinsurer properties from a database
    '              record.
    ' ***************************************************************** '
    'ECK Datasure 10102005 Claims Rating Agency
    'developer guide no.21
    Public Function SetPropertiesFromDB(ByRef oFields As DataRow) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Populate Base Details

            With oFields

                PartyCnt = gPMFunctions.NullToLong(oFields("party_cnt"))

                If Convert.IsDBNull(oFields("agency_number")) Or Informations.IsNothing(oFields("agency_number")) Then

                    AgencyNumber = Nothing
                Else

                    AgencyNumber = oFields("agency_number")
                End If

                If Convert.IsDBNull(oFields("binder_indicator")) Or Informations.IsNothing(oFields("binder_indicator")) Then

                    BinderIndicator = Nothing
                Else

                    BinderIndicator = oFields("binder_indicator")
                End If

                If Convert.IsDBNull(oFields("report_indicator")) Or Informations.IsNothing(oFields("report_indicator")) Then

                    ReportIndicator = Nothing
                Else

                    ReportIndicator = oFields("report_indicator")
                End If
                IsReinsurer = gPMFunctions.NullToLong(oFields("is_reinsurer"))

                If Convert.IsDBNull(oFields("reinsurance_type")) Or Informations.IsNothing(oFields("reinsurance_type")) Then

                    ReinsuranceType = Nothing
                Else

                    ReinsuranceType = oFields("reinsurance_type")
                End If
                IsReinsuranceDebitCreditNo = gPMFunctions.NullToLong(oFields("is_reinsurance_debit_credit_no"))

                If Convert.IsDBNull(oFields("default_comm_rate")) Or Informations.IsNothing(oFields("default_comm_rate")) Then

                    DefaultCommRate = Nothing
                Else

                    DefaultCommRate = oFields("default_comm_rate")
                End If


                If Convert.IsDBNull(oFields("tax_group_id")) Or Informations.IsNothing(oFields("tax_group_id")) Then
                    TaxGroupID = 0
                Else

                    TaxGroupID = oFields("tax_group_id")

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

                BankAccount = gPMFunctions.NullToString(oFields("bank_account"))
                ' RAW 18/12/2002 : PS187 : end

                'DC150803 -PS254 -fsa compliance

                If Convert.IsDBNull(oFields("fsa_insurerstatus_id")) Or Informations.IsNothing(oFields("fsa_insurerstatus_id")) Then
                    FSAInsurerStatus = -1
                Else

                    FSAInsurerStatus = oFields("fsa_insurerstatus_id")

                End If
                FSARegistrationNumber = gPMFunctions.NullToString(oFields("fsa_registration_number"))

                If Convert.IsDBNull(oFields("fsa_insurercreditrating_id")) Or Informations.IsNothing(oFields("fsa_insurercreditrating_id")) Then
                    FSAInsurerCreditRating = -1
                Else

                    FSAInsurerCreditRating = oFields("fsa_insurercreditrating_id")

                End If


                If Convert.IsDBNull(oFields("is_retained")) Or Informations.IsNothing(oFields("is_retained")) Then
                    IsRetained = False
                Else

                    IsRetained = oFields("is_retained")

                End If

                If Convert.IsDBNull(oFields("claims_rating_agency_id")) Or Informations.IsNothing(oFields("claims_rating_agency_id")) Then
                    ClaimsRatingAgencyId = 0
                Else

                    ClaimsRatingAgencyId = oFields("claims_rating_agency_id")

                End If

                If Convert.IsDBNull(oFields("claims_rating_grading")) Or Informations.IsNothing(oFields("claims_rating_grading")) Then
                    ClaimsRatingGrading = ""
                Else

                    ClaimsRatingGrading = oFields("claims_rating_grading")

                End If

                If Convert.IsDBNull(oFields("claims_rating_date")) Or Informations.IsNothing(oFields("claims_rating_date")) Then
                    ClaimsRatingDate = DateTime.Now
                Else

                    ClaimsRatingDate = oFields("claims_rating_date")

                End If

                If Convert.IsDBNull(oFields("claims_rating_description")) Or Informations.IsNothing(oFields("claims_rating_description")) Then
                    ClaimsRatingDescription = ""
                Else

                    ClaimsRatingDescription = oFields("claims_rating_description")

                End If

                If Convert.IsDBNull(oFields("terms_of_payment_id")) Or Informations.IsNothing(oFields("terms_of_payment_id")) Then
                    TermsOfPaymentId = 0
                Else

                    TermsOfPaymentId = oFields("terms_of_payment_id")

                End If
                'DC0050706 fix issue where tax flag not set correctly

                If Convert.IsDBNull(oFields("domiciled_for_tax")) Or Informations.IsNothing(oFields("domiciled_for_tax")) Then
                    DomiciledForTax = 0
                Else

                    DomiciledForTax = oFields("domiciled_for_tax")

                End If

                RiskTransferAgreement = gPMFunctions.ToSafeBoolean(oFields("risk_transfer_agreement"), False)


                If Convert.IsDBNull(oFields("Brokerlink_Subaccount")) Or Informations.IsNothing(oFields("Brokerlink_Subaccount")) Then

                    BrokerlinkSubAccount = Nothing
                Else

                    BrokerlinkSubAccount = oFields("Brokerlink_Subaccount")
                End If


                If Convert.IsDBNull(oFields("Brokerlink_UW_ID")) Or Informations.IsNothing(oFields("Brokerlink_UW_ID")) Then

                    BrokerlinkUnderwritingID = Nothing
                Else

                    BrokerlinkUnderwritingID = oFields("Brokerlink_UW_ID")
                End If

                'QBENZ005

                If Convert.IsDBNull(oFields("is_ri_broker")) Or Informations.IsNothing(oFields("is_ri_broker")) Then
                    IsRIBroker = False
                Else

                    IsRIBroker = oFields("is_ri_broker")

                End If



                If Convert.IsDBNull(oFields("Insurer_locking_type_id")) Or Informations.IsNothing(oFields("Insurer_locking_type_id")) Then
                    CboLockingTypeId = 1
                Else

                    CboLockingTypeId = oFields("Insurer_locking_type_id")

                End If


                If Convert.IsDBNull(oFields("risk_transfer_editable")) Or Informations.IsNothing(oFields("risk_transfer_editable")) Then
                    RiskTransferEditable = 0
                Else
                    RiskTransferEditable = If(gPMFunctions.ToSafeBoolean(oFields("risk_transfer_editable")), 1, 0)
                End If

                'PYV Development

                If Convert.IsDBNull(oFields("insurer_type_id")) Or Informations.IsNothing(oFields("insurer_type_id")) Then
                    InsurerTypeId = 3
                Else

                    InsurerTypeId = oFields("insurer_type_id")

                End If

                If Convert.IsDBNull(oFields("bureauaccountparty")) Or Informations.IsNothing(oFields("bureauaccountparty")) Then
                    BureauAccountParty = Nothing
                Else

                    BureauAccountParty = oFields("bureauaccountparty")
                End If


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
    'ECK Datasure 10102005 Claims Rating Agency
    Private Function AddInputParam() As Integer

        Dim result As Integer = 0
        Dim vValue As Object ' RAW 18/12/2002 : PS187 : added



        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            m_lReturn = .Parameters.Add(sName:="agency_number", vValue:=AgencyNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="binder_indicator", vValue:=BinderIndicator, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="report_indicator", vValue:=ReportIndicator, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="is_reinsurer", vValue:=IsReinsurer, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="reinsurance_type", vValue:=ReinsuranceType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="is_reinsurance_debit_credit_n", vValue:=IsReinsuranceDebitCreditNo, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="default_comm_rate", vValue:=DefaultCommRate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' RAW 18/12/2002 : PS187 : added
            If TaxGroupID = 0 Then


                vValue = DBNull.Value ' convert 0 to null for optional parameter
            Else

                vValue = TaxGroupID
            End If

            'developer guide no.85
            m_lReturn = .Parameters.Add(sName:="tax_group_id", vValue:=vValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' RAW 18/12/2002 : PS187 : added
            If PaymentMethod = -1 Then


                vValue = DBNull.Value ' convert -1 to null for optional parameter
            Else

                vValue = PaymentMethod
            End If

            'developer guide no.85
            m_lReturn = .Parameters.Add(sName:="payment_method", vValue:=vValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If PaymentFrequency = -1 Then


                vValue = DBNull.Value ' convert -1 to null for optional parameter
            Else

                vValue = PaymentFrequency
            End If

            'developer guide no.85
            m_lReturn = .Parameters.Add(sName:="payment_frequency", vValue:=vValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If BankAccount = "" Then


                vValue = DBNull.Value ' convert "" to null for optional parameter
            Else

                vValue = BankAccount
            End If

            'developer guide no.85
            m_lReturn = .Parameters.Add(sName:="bank_account", vValue:=vValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' RAW 18/12/2002 : PS187 : end

            'DC150803 -PS254 -fsa compliance
            If FSAInsurerStatus = -1 Then


                vValue = DBNull.Value ' convert -1 to null for optional parameter
            Else

                vValue = FSAInsurerStatus
            End If

            'developer guide no.85
            m_lReturn = .Parameters.Add(sName:="fsa_insurerstatus_id", vValue:=vValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="fsa_registration_number", vValue:=FSARegistrationNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If FSAInsurerCreditRating = -1 Then


                vValue = DBNull.Value ' convert -1 to null for optional parameter
            Else

                vValue = FSAInsurerCreditRating
            End If

            'developer guide no.85
            m_lReturn = .Parameters.Add(sName:="fsa_insurercreditrating_id", vValue:=vValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="is_retained", vValue:=IsRetained, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If ClaimsRatingAgencyId = 0 Then


                vValue = DBNull.Value ' convert -1 to null for optional parameter
            Else

                vValue = ClaimsRatingAgencyId
            End If


            'developer guide no.85
            m_lReturn = .Parameters.Add(sName:="claims_rating_agency_id", vValue:=vValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="claims_rating_grading", vValue:=ClaimsRatingGrading, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If ClaimsRatingGrading = "" Then


                vValue = DBNull.Value
            Else

                vValue = ClaimsRatingDate
            End If

            'developer guide no.85
            m_lReturn = .Parameters.Add(sName:="claims_rating_date", vValue:=vValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="claims_rating_description", vValue:=ClaimsRatingDescription, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="terms_of_payment_id", vValue:=TermsOfPaymentId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DC0050706 fix issue where tax flag not set correctly
            m_lReturn = .Parameters.Add(sName:="domiciled_for_tax", vValue:=m_vDomiciledForTax, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="risk_transfer_agreement", vValue:=CStr(RiskTransferAgreement), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="Brokerlink_Subaccount", vValue:=BrokerlinkSubAccount, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="Brokerlink_UW_ID", vValue:=BrokerlinkUnderwritingID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'QBENZ005
            m_lReturn = .Parameters.Add(sName:="is_ri_broker", vValue:=IsRIBroker, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Devlopment work on Insurer Payment Locking
            m_lReturn = .Parameters.Add(sName:="Insurer_locking_type_id", vValue:=CboLockingTypeId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="risk_transfer_editable", vValue:=RiskTransferEditable, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="Insurer_type_id", vValue:=InsurerTypeId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="bureau_account_cnt", vValue:=CStr(BureauAccountParty), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="UserId", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="UniqueId", vValue:=m_sUniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="ScreenHierarchy", vValue:=m_sScreenHierarchy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
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