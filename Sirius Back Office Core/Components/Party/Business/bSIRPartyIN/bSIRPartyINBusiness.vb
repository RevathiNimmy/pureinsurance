Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
'developer guide no. 129
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable

    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 25/06/1999
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a SIRPartyIN.
    '
    ' Edit History:
    ' SJP14062002 - getUnderWritingOrAgency and getUnderwritingType
    '               use new product options scheme
    ' RAW 18/12/2002 : PS187 : Added new data items (WHTaxRate, TaxRegNo, TaxCode, PaymentMethod, PaymentFrequency , BankAccount)
    '                          and new lookup tables (PaymentMethod, PaymentFrequency)
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 11/02/2004
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************


    ' ************************************************
    ' Added to replace global variables 09/02/2004



    ' ************************************************


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Collection of SIRPartyINs (Private)
    Private m_oSIRPartyINs As bSIRPartyIN.SIRPartyINs

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private lPMAuthorityLevel As Integer

    Private m_bGeminiIILink As Boolean

    Private m_oGIIDatabase As dPMDAO.Database

    ' Primary Keys to work with
    Private m_lPartyCnt As Integer

    ' PM Lookup Business Component (Private)
    Private m_oLookup As BPMLOOKUP.Business

    ' TN10072000 Finds out if Underwriting or Agency business
    Private m_sUnderwritingOrAgency As String = ""

    'JMK 19/10/2001 - another hidden option
    Private m_sUnderwritingType As String = ""
    Private m_oSIRParty As bSIRParty.Business

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)

            Value = Value

        End Set
    End Property

    Public Property CurrentRecord() As Integer
        Get

            Return m_lCurrentRecord

        End Get
        Set(ByVal Value As Integer)

            Select Case Value
                Case Is < 1
                    m_lCurrentRecord = 0
                Case Is > m_oSIRPartyINs.Count()
                    m_lCurrentRecord = m_oSIRPartyINs.Count()
                Case Else
                    m_lCurrentRecord = Value
            End Select

        End Set
    End Property

    Public ReadOnly Property RecordCount() As Integer
        Get

            ' Return Number in Collection
            Return m_oSIRPartyINs.Count()

        End Get
    End Property


    Public Property PartyCnt() As Integer
        Get

            Return m_lPartyCnt

        End Get
        Set(ByVal Value As Integer)

            m_lPartyCnt = Value

        End Set
    End Property

    Public ReadOnly Property GeminiIILink() As Boolean
        Get

            Return m_bGeminiIILink

        End Get
    End Property

    ' TN 10072000 return "A" for Agency and "U" for Underwriting
    Public ReadOnly Property UnderwritingOrAgency() As String
        Get

            If m_sUnderwritingOrAgency = "" Then
                m_lReturn = getUnderwritingOrAgency()
            End If

            Return m_sUnderwritingOrAgency

        End Get
    End Property

    ' JMK 19/10/2001 "A" for Underwriting Agency and "U" for Reinsurance
    Public ReadOnly Property UnderwritingType() As String
        Get

            If m_sUnderwritingType = "" Then
                m_lReturn = getUnderwritingType()
            End If

            Return m_sUnderwritingType

        End Get
    End Property

    ' PUBLIC Property Procedures (End)

    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)


    ' ***************************************************************** '
    ' Name: UnderwritingOrAgency
    '
    ' Description:  Finds out if Underwriting or Agency business
    '
    ' History:
    ' 06/06/2002 SP - moved to uniform Product Options scheme
    ' ***************************************************************** '
    Public Function getUnderwritingOrAgency() As Integer

        Dim result As Integer = 0
        Try


            Return bPMFunc.getUnderwritingOrAgency(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, m_sUnderwritingOrAgency)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnderwritingOrAgencyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnderwritingOrAgency", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetUnderwritingType
    '
    ' Description:  Finds out Underwriting type - U or A
    '               For labelling: A - Insurer. U - Reinsurer
    '
    ' JMK 19/10/2001    Created
    ' 06/06/2002 SP - moved to uniform Product Options scheme
    ' ***************************************************************** '
    Private Function getUnderwritingType() As Integer

        Dim result As Integer = 0



        Return bPMFunc.getUnderwritingType(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, m_sUnderwritingType)

    End Function

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUserName As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long


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




            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '***************************************

            m_oSIRParty = New bSIRParty.Business
            m_lReturn = m_oSIRParty.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            '***************************************

            'Tomo040400
            'Is GII there?

            m_lReturn = CType(gPMComponentServices.CheckPMProductInstalled(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFGeminiII, r_bInstalled:=m_bGeminiIILink), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_bGeminiIILink Then
                m_lReturn = CType(gPMComponentServices.NewDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFGeminiII, r_oDatabase:=m_oGIIDatabase), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If


            ' Set Username and Password

            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Create SIRPartyINs Collection
            m_oSIRPartyINs = New bSIRPartyIN.SIRPartyINs()

            ' Create PM Lookup Business Object
            m_oLookup = New BPMLOOKUP.Business()

            ' Initialise PM Lookup Business passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUsername:=sUserName, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions


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
                If m_oLookup IsNot Nothing Then
                    m_oLookup.Dispose()
                End If
                m_oLookup = Nothing
                m_oSIRParty = Nothing
                m_oSIRPartyINs = Nothing
                If Not (m_oGIIDatabase Is Nothing) Then
                    m_oGIIDatabase.CloseDatabase()


                    m_oGIIDatabase = Nothing
                End If
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

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupValues (Public)
    '
    ' Description: Gets the Lookup values for a SIRPartyIN.
    '
    ' ***************************************************************** '
    Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim oSIRPartyIN As bSIRPartyIN.SIRPartyIN = Nothing
        Dim dtEffectiveDate As Date
        Dim vTabArray(4, 6) As Object

        Dim vCurrencyID As Object = Nothing
        Dim vPaymentMethod As Object = Nothing
        Dim vPaymentFrequency As Object = Nothing
        Dim vFSAInsurerStatus As Object = Nothing
        Dim vFSAInsurerCreditRating As Object = Nothing
        Dim vPaymentTermCode As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            vResultArray = Nothing
            vTableArray = Nothing

            ' Setup Lookup Table Names
            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = gSIRLibrary.SIRLookupCurrency
            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 1) = gSIRLibrary.SIRLookupRiskGroup
            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 2) = gSIRLibrary.SIRLookupPaymentMethod
            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 3) = gSIRLibrary.SIRLookupPaymentFrequency
            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 4) = gSIRLibrary.SIRLookupFSAInsurerStatus
            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 5) = gSIRLibrary.SIRLookupFSAInsurerCreditRating
            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 6) = gSIRLibrary.SIRLookupPFFrequency
            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupWhereClause, 6) = "is_available_on_client_screen = 1"

            ' Do we have any records
            If m_lCurrentRecord < 1 Then
                ' No, we can only lookup all
                iLookupType = gPMConstants.PMELookupType.PMLookupAll
            Else
                ' Yes get current record
                oSIRPartyIN = m_oSIRPartyINs.Item(m_lCurrentRecord)
            End If

            Select Case iLookupType
                Case gPMConstants.PMELookupType.PMLookupAll

                    ' Do not supply a key
                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = ""
                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 1) = ""
                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 2) = ""
                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 3) = ""
                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 4) = ""
                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 5) = ""
                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 6) = ""

                Case gPMConstants.PMELookupType.PMLookupAllEffective

                    ' Use keys and effective date from current record
                    ' Note: The keys are not used for the select, but are used by
                    '       the interface program to set the list index.
                    With oSIRPartyIN

                        m_lReturn = CType(.GetProperties(iStatus:=gPMConstants.PMEComponentAction.PMView, vPaymentMethod:=vPaymentMethod, vPaymentFrequency:=vPaymentFrequency, vFSAInsurerStatus:=vFSAInsurerStatus, vFSAInsurerCreditRating:=vFSAInsurerCreditRating), gPMConstants.PMEReturnCode)

                        m_lReturn = .bSIRParty.GetLookupProperties(vCurrentRecord:=ToSafeInteger(m_lCurrentRecord), vAreaId:=Nothing, vCurrencyId:=vCurrencyID, vReminderTypeId:=Nothing,
                           vServiceLevelId:=Nothing, vSeasonalGiftID:=Nothing, vCorrespondenceTypeId:=Nothing,
                           vRenewalStopCodeId:=Nothing, vPaymentTermCode:=vPaymentTermCode)

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = vCurrencyID
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 2) = vPaymentMethod
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 3) = vPaymentFrequency
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 4) = vFSAInsurerStatus
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 5) = vFSAInsurerCreditRating
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 6) = vPaymentTermCode

                    End With

                Case gPMConstants.PMELookupType.PMLookupSingle

                    ' Set keys from current record
                    With oSIRPartyIN

                        m_lReturn = CType(.GetProperties(iStatus:=gPMConstants.PMEComponentAction.PMView, vPaymentMethod:=vPaymentMethod, vPaymentFrequency:=vPaymentFrequency, vFSAInsurerStatus:=vFSAInsurerStatus, vFSAInsurerCreditRating:=vFSAInsurerCreditRating), gPMConstants.PMEReturnCode)

                        m_lReturn = .bSIRParty.GetLookupProperties(vCurrentRecord:=ToSafeInteger(m_lCurrentRecord), vAreaId:=Nothing, vCurrencyId:=vCurrencyID, vReminderTypeId:=Nothing,
                           vServiceLevelId:=Nothing, vSeasonalGiftID:=Nothing, vCorrespondenceTypeId:=Nothing,
                           vRenewalStopCodeId:=Nothing, vPaymentTermCode:=vPaymentTermCode)

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = vCurrencyID

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 2) = vPaymentMethod
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 3) = vPaymentFrequency
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 4) = vFSAInsurerStatus
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 5) = vFSAInsurerCreditRating
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 6) = vPaymentTermCode

                    End With
            End Select

            ' Default Effective Date to current date/time
            dtEffectiveDate = DateTime.Now

            ' Release SIRPartyIN reference
            oSIRPartyIN = Nothing

            ' Get the Lookup items
            m_lReturn = m_oLookup.GetLookupValues(iLookupType:=iLookupType, vTableArray:=vTabArray, iLanguageID:=iLanguageID, dtEffectiveDate:=dtEffectiveDate, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the Table Array
            vTableArray = vTabArray

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookupValues Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectAdd (Public)
    '
    ' Description: Adds a single SIRPartyIN directly into the database.
    '        Note: The SIRPartyIN will NOT be added to the collection.
    '
    ' ***************************************************************** '
    'DC 15/08/00 Added Invariant Key
    'DC 16/11/00 Added Resolved Name
    ' RAW 18/12/2002 : PS187 : Added WH TaxType, WH Tax Rate, Tax Reg N, Tax Code & PaymentMethod, PaymentFrequency, BankAccount
    'DC150803 -PS254 -fsa compliance
    'ECK Datasure 10102005 Claims Rating Agency
    Public Function DirectAdd(Optional ByRef vPartyCnt As Integer = 0, Optional ByRef vAgencyNumber As Object = Nothing, Optional ByRef vBinderIndicator As Object = Nothing, Optional ByRef vReportIndicator As Object = Nothing, Optional ByRef vIsReinsurer As Object = Nothing, Optional ByRef vReinsuranceType As Object = Nothing, Optional ByRef vIsReinsuranceDebitCreditNo As Object = Nothing, Optional ByRef vDefaultCommRate As Object = Nothing, Optional ByRef vShortName As Object = Nothing, Optional ByRef vName As Object = Nothing, Optional ByRef vResolvedName As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vPaymentTermCode As Object = Nothing, Optional ByRef vStatements As Object = Nothing, Optional ByRef vFileCode As Object = Nothing, Optional ByRef vABCCount As Object = Nothing, Optional ByRef vLastModified As Object = Nothing, Optional ByRef vLastActionType As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vABICodeOn406 As Object = Nothing, Optional ByRef vABICodeOn81 As Object = Nothing, Optional ByRef vABICodeList As Object = Nothing, Optional ByRef vInvariantKey As Object = Nothing, Optional ByRef vTaxGroupID As Object = Nothing, Optional ByRef vPaymentMethod As Object = Nothing, Optional ByRef vPaymentFrequency As Object = Nothing, Optional ByRef vBankAccount As Object = Nothing, Optional ByRef vFSAInsurerStatus As Object = Nothing, Optional ByRef vFSARegistrationNumber As Object = Nothing, Optional ByRef vFSAInsurerCreditRating As Object = Nothing, Optional ByRef vIsRetained As Object = Nothing, Optional ByRef vClaimsRatingAgencyId As Object = Nothing, Optional ByRef vClaimsRatingGrading As Object = Nothing, Optional ByRef vClaimsRatingDate As Object = Nothing, Optional ByRef vClaimsRatingDescription As Object = Nothing, Optional ByRef vTermsOfPaymentId As Object = Nothing, Optional ByRef vDomiciledForTax As Object = Nothing, Optional ByRef vRiskTransferAgreement As Object = Nothing, Optional ByRef vIsRIBroker As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oSIRPartyIN As bSIRPartyIN.SIRPartyIN
        Dim lPartyTypeId As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'EK 22/9/99
            'Get party type id for a corporate client
            m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:=gSIRLibrary.SIRLookupPartyType, v_sCode:=gSIRLibrary.SIRPartyTypeInsurer, v_dtEffectiveDate:=DateTime.Now, r_lID:=lPartyTypeId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Create a new SIRPartyIN
            oSIRPartyIN = New bSIRPartyIN.SIRPartyIN()
            m_lReturn = CType(oSIRPartyIN.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            'DC 15/08/00 Added Invariant Key
            'DC 25/09/00 Set Resolved Name as Name

            m_lReturn = oSIRPartyIN.bSIRParty.DirectAdd(vPartyTypeId:=ToSafeInteger(lPartyTypeId), vShortName:=vShortName, vName:=vName, vResolvedName:=vName, vCurrencyID:=vCurrencyID, vPaymentTermCode:=vPaymentTermCode, vStatements:=vStatements, vFileCode:=vFileCode, vABCCount:=vABCCount, vLastModified:=vLastModified, vLastActionType:=vLastActionType, vDateCreated:=vDateCreated, vABICodeOn406:=vABICodeOn406, vABICodeOn81:=vABICodeOn81, vABICodeList:=vABICodeList, vInvariantKey:=vInvariantKey)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyIN = Nothing
                Return result
            End If
            ' Retrieve Primary Key of new Core record

            vPartyCnt = oSIRPartyIN.bSIRParty.PartyCnt
            'EK 22/9/99
            oSIRPartyIN.PartyCnt = vPartyCnt

            'developer guide no.98
            m_lReturn = oSIRPartyIN.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMAdd,
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
            vClaimsRatingAgencyId:=vClaimsRatingAgencyId,
            vClaimsRatingGrading:=vClaimsRatingGrading,
            vClaimsRatingDate:=vClaimsRatingDate,
            vClaimsRatingDescription:=vClaimsRatingDescription,
            vTermsOfPaymentId:=vTermsOfPaymentId,
            vDomiciledForTax:=vDomiciledForTax,
            vRiskTransferAgreement:=vRiskTransferAgreement, vIsRIBroker:=vIsRIBroker)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyIN = Nothing
                Return result
            End If

            ' Add the SIRPartyIN to the Database
            m_lReturn = CType(oSIRPartyIN.AddItem(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyIN = Nothing
                Return result
            End If

            ' Retain the Primary Key of the SIRPartyIN Added
            With oSIRPartyIN
                PartyCnt = .PartyCnt
            End With

            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}

            oSIRPartyIN = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectDelete (Public)
    '
    ' Description: Deletes a single SIRPartyIN directly from the database.
    '        Note: The SIRPartyIN will NOT be deleted from the collection.
    '
    ' ***************************************************************** '
    Public Function DirectDelete(Optional ByRef vPartyCnt As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oSIRPartyIN As bSIRPartyIN.SIRPartyIN

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new SIRPartyIN
            oSIRPartyIN = New bSIRPartyIN.SIRPartyIN()
            m_lReturn = CType(oSIRPartyIN.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            ' Set SIRPartyIN Primary Key

            m_lReturn = CType(oSIRPartyIN.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMDelete, vPartyCnt:=CInt(vPartyCnt)), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyIN = Nothing
                Return result
            End If

            ' Delete the SIRPartyIN from the Database
            m_lReturn = CType(oSIRPartyIN.DeleteItem(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyIN = Nothing
                Return result
            End If


            m_lReturn = oSIRPartyIN.bSIRParty.DirectDelete(vPartyCnt:=vPartyCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyIN = Nothing
                Return result
            End If

            oSIRPartyIN = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectDelete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckID (Public)
    '
    ' Description: Checks to see if the supplied ID is a valid record.
    '
    ' ***************************************************************** '
    Public Function CheckID(ByRef vID As Object) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the ID parameter (INPUT)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="id", vValue:=CStr(vID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckIDSQL, sSQLName:=ACCheckIDName, bStoredProcedure:=ACCheckIDStored, lNumberRecords:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            ' Do we have any records ?
            If lRecordCount < 1 Then
                ' No record found
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the required SIRPartyINs and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetDetails(Optional ByRef vLockMode As Object = 0, Optional ByRef vPartyCnt As Object = 0) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        'developer guide no. 112
        Dim oFields As DataRow
        Dim oSIRPartyIN As bSIRPartyIN.SIRPartyIN

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Collection
            m_oSIRPartyINs.Clear()

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Default to No Lock if not supplied or not numeric
            Dim dbNumericTemp As Double

            If (Informations.IsNothing(vLockMode)) Or (Not Double.TryParse(CStr(vLockMode), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                vLockMode = gPMConstants.PMELockMode.PMNoLock
            End If

            ' Check for Valid Primary Key
            Dim dbNumericTemp2 As Double

            If (Not Informations.IsNothing(vPartyCnt)) And (Not Double.TryParse(CStr(vPartyCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2)) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vPartyCnt=" & vPartyCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                Return result
            End If


            If Not Informations.IsNothing(vPartyCnt) Then

                ' Create New SIRPartyIN
                oSIRPartyIN = New bSIRPartyIN.SIRPartyIN()
                m_lReturn = CType(oSIRPartyIN.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

                ' Set component primary keys
                With oSIRPartyIN
                    .PartyCnt = vPartyCnt

                    m_lReturn = CType(.SelectItem(), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return m_lReturn
                    End If
                End With

                ' Add SIRPartyIN to collection
                If m_oSIRPartyINs.Count = 0 Then
                    m_oSIRPartyINs.Add(Nothing)
                End If
                m_lReturn = CType(m_oSIRPartyINs.Add(oNewSIRPartyIN:=oSIRPartyIN), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Call the core Business method

                m_lReturn = oSIRPartyIN.bSIRParty.GetDetails(vLockMode:=vLockMode, vPartyCnt:=vPartyCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                oSIRPartyIN = Nothing

            Else

                ' No Key, Get All Records

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllDetailsSQL, sSQLName:=ACGetAllDetailsName, bStoredProcedure:=ACGetAllDetailsStored, lNumberRecords:=0)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' How many records were selected
                lRecordCount = m_oDatabase.Records.Count()

                ' Do we have any records ?
                If lRecordCount < 1 Then
                    ' No Records, return PMFalse
                    Return gPMConstants.PMEReturnCode.PMNotFound
                End If

                ' Yes, load them into the collection
                For lSub As Integer = 1 To lRecordCount

                    ' Create New
                    oSIRPartyIN = New bSIRPartyIN.SIRPartyIN()
                    m_lReturn = CType(oSIRPartyIN.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

                    ' Set oFields to refer to one Record
                    'developer guide no. 111
                    oFields = m_oDatabase.Records.Item(lSub - 1).Fields()

                    ' Set component primary keys from current record
                    With oSIRPartyIN
                        'SD 02/08/2002
                        .PartyCnt = gPMFunctions.NullToLong(oFields("party_cnt"))

                        m_lReturn = CType(.SelectItem(), gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        m_lReturn = .bSIRParty.GetDetails(vLockMode:=vLockMode, vPartyCnt:=ToSafeInteger(.PartyCnt))

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End With

                    ' Add SIRPartyIN to collection
                    If m_oSIRPartyINs.Count = 0 Then
                        m_oSIRPartyINs.Add(Nothing)
                    End If
                    m_lReturn = CType(m_oSIRPartyINs.Add(oNewSIRPartyIN:=oSIRPartyIN), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oSIRPartyIN = Nothing
                Next lSub
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetNext (Public)
    '
    ' Description: Gets the required SIRPartyINs and populate the Collection
    '
    ' eck270901 Add resolved name
    '
    ' ***************************************************************** '
    ' RAW 18/12/2002 : PS187 : Added WH TaxType, WH Tax Rate, Tax Reg No, Tax Code & PaymentMethod, PaymentFrequency, BankAccount
    'DC150803 -PS254 -fsa compliance
    'ECK Datasure 10102005 Claims Rating Agency
    Public Function GetNext(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyId As Object = Nothing, Optional ByRef vAgencyNumber As Object = Nothing, Optional ByRef vBinderIndicator As Object = Nothing, Optional ByRef vReportIndicator As Object = Nothing, Optional ByRef vIsReinsurer As Object = Nothing, Optional ByRef vReinsuranceType As Object = Nothing, Optional ByRef vIsReinsuranceDebitCreditNo As Object = Nothing, Optional ByRef vDefaultCommRate As Object = Nothing, Optional ByRef vSourceId As Object = Nothing, Optional ByRef vShortName As Object = Nothing, Optional ByRef vName As Object = Nothing, Optional ByRef vResolvedName As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vPaymentTermCode As Object = Nothing, Optional ByRef vStatements As Object = Nothing, Optional ByRef vABICodeOn81 As Object = Nothing, Optional ByRef vSubBranchID As Object = Nothing, Optional ByRef vTaxGroupID As Object = Nothing, Optional ByRef vPaymentMethod As Object = Nothing, Optional ByRef vPaymentFrequency As Object = Nothing, Optional ByRef vBankAccount As Object = Nothing, Optional ByRef vFSAInsurerStatus As Object = Nothing, Optional ByRef vFSARegistrationNumber As Object = Nothing, Optional ByRef vFSAInsurerCreditRating As Object = Nothing, Optional ByRef vIsRetained As Object = Nothing, Optional ByRef vClaimsRatingAgencyId As Object = Nothing, Optional ByRef vClaimsRatingGrading As Object = Nothing, Optional ByRef vClaimsRatingDate As Object = Nothing, Optional ByRef vClaimsRatingDescription As Object = Nothing, Optional ByRef vTermsOfPaymentId As Object = Nothing, Optional ByRef vDomiciledForTax As Object = Nothing, Optional ByRef vRiskTransferAgreement As Object = Nothing, Optional ByRef vBrokerlinkSubaccount As Object = Nothing, Optional ByRef vBrokerlinkUnderwritingid As Object = Nothing, Optional ByRef vIsRIBroker As Object = Nothing, Optional ByRef vCboLockingTypeId As Object = Nothing, Optional ByRef vRiskTransferEditable As Object = Nothing, Optional ByRef vInsurerTypeId As Object = Nothing, Optional ByRef vBureauAccountParty As Object = Nothing) As Integer


        Dim result As Integer = 0

        Dim oSIRPartyIN As bSIRPartyIN.SIRPartyIN
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oSIRPartyINs.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord += 1
            Else
                result = gPMConstants.PMEReturnCode.PMEOF
            End If

            oSIRPartyIN = m_oSIRPartyINs.Item(m_lCurrentRecord)

            ' Get the SIRPartyIN Property Values
            ' RAW 18/12/2002 : PS187 : Added WH TaxType, WH Tax Rate, Tax Reg No, Tax Code & PaymentMethod, PaymentFrequency, BankAccount
            'DC150803 -PS254 -fsa compliance

            'developer guide no.98
            m_lReturn = oSIRPartyIN.GetProperties(iStatus,
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
        vIsRetained:=vIsRetained, vClaimsRatingAgencyId:=vClaimsRatingAgencyId,
        vClaimsRatingGrading:=vClaimsRatingGrading,
        vClaimsRatingDate:=vClaimsRatingDate,
        vClaimsRatingDescription:=vClaimsRatingDescription,
        vTermsOfPaymentId:=vTermsOfPaymentId, vDomiciledForTax:=vDomiciledForTax,
        vRiskTransferAgreement:=vRiskTransferAgreement,
        vBrokerlinkSubaccount:=vBrokerlinkSubaccount,
        vBrokerlinkUnderwritingid:=vBrokerlinkUnderwritingid, vIsRIBroker:=vIsRIBroker, vCboLockingTypeId:=vCboLockingTypeId, vRiskTransferEditable:=vRiskTransferEditable,
        vInsurerTypeId:=vInsurerTypeId, vBureauAccountParty:=vBureauAccountParty)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get Core details

            m_lReturn = oSIRPartyIN.bSIRParty.GetDetails(vPartyCnt:=vPartyCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'eck270901 Added resolved name

            m_lReturn = oSIRPartyIN.bSIRParty.GetNext(vSourceId:=vSourceId, vPartyId:=vPartyId, vShortName:=vShortName, vName:=vName, vResolvedName:=vResolvedName, vCurrencyID:=vCurrencyID, vPaymentTermCode:=vPaymentTermCode, vStatements:=vStatements, vABICodeOn81:=vABICodeOn81, vSubBranchID:=vSubBranchID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRPartyIN = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNext", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditAdd (Public)
    '
    ' Description: Adds the supplied SIRPartyIN into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    '
    ' ***************************************************************** '
    ' RAW 18/12/2002 : PS187 : Added WH TaxType, WH Tax Rate, Tax Reg No, Tax Code & PaymentMethod, PaymentFrequency, BankAccount
    'DC150803 -PS254 -fsa compliance
    'ECK Datasure 10102005 Claims Rating Agency
    Public Function EditAdd(ByRef lRow As Object, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vAgencyNumber As Object = Nothing, Optional ByRef vBinderIndicator As Object = Nothing, Optional ByRef vReportIndicator As Object = Nothing, Optional ByRef vIsReinsurer As Object = Nothing, Optional ByRef vReinsuranceType As Object = Nothing, Optional ByRef vIsReinsuranceDebitCreditNo As Object = Nothing, Optional ByRef vDefaultCommRate As Object = Nothing, Optional ByRef vShortName As Object = Nothing, Optional ByRef vName As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vPaymentTermCode As Object = Nothing, Optional ByRef vStatements As Object = Nothing, Optional ByRef vABICodeOn81 As Object = Nothing, Optional ByRef vTaxGroupID As Object = Nothing, Optional ByRef vPaymentMethod As Object = Nothing, Optional ByRef vPaymentFrequency As Object = Nothing, Optional ByRef vBankAccount As Object = Nothing, Optional ByRef vFSAInsurerStatus As Object = Nothing, Optional ByRef vFSARegistrationNumber As Object = Nothing, Optional ByRef vFSAInsurerCreditRating As Object = Nothing, Optional ByRef vIsRetained As Object = Nothing, Optional ByRef vBranchID As Object = Nothing, Optional ByRef vSubBranchID As Object = Nothing, Optional ByRef vClaimsRatingAgencyId As Object = Nothing, Optional ByRef vClaimsRatingGrading As Object = Nothing, Optional ByRef vClaimsRatingDate As Object = Nothing, Optional ByRef vClaimsRatingDescription As Object = Nothing, Optional ByRef vTermsOfPaymentId As Object = Nothing, Optional ByRef vDomiciledForTax As Object = Nothing, Optional ByRef vRiskTransferAgreement As Object = Nothing, Optional ByRef vBrokerlinkSubaccount As Object = Nothing, Optional ByRef vBrokerlinkUnderwritingid As Object = Nothing, Optional ByRef vIsRIBroker As Object = Nothing, Optional ByRef vCboLockingTypeId As Object = Nothing, Optional ByRef vRiskTransferEditable As Object = Nothing, Optional ByRef vInsurerTypeId As Object = Nothing, Optional ByRef vBureauAccountParty As Object = Nothing, Optional sUniqueId As String = "", Optional sScreenHierarchy As String = "") As Integer


        Dim result As Integer = 0
        Dim oSIRPartyIN As bSIRPartyIN.SIRPartyIN
        Dim lPartyTypeId As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get party type id for an agent
            m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:=gSIRLibrary.SIRLookupPartyType, v_sCode:=gSIRLibrary.SIRPartyTypeInsurer, v_dtEffectiveDate:=DateTime.Now, r_lID:=lPartyTypeId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If 'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oSIRPartyINs.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new SIRPartyIN
            oSIRPartyIN = New bSIRPartyIN.SIRPartyIN()
            m_lReturn = CType(oSIRPartyIN.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            ' Populate SIRPartyIN Attributes
            ' RAW 18/12/2002 : PS187 : Added WH TaxType, WH Tax Rate, Tax Reg No, Tax Code & PaymentMethod, PaymentFrequency, BankAccount
            'DC150803 -PS254 -fsa compliance

            'developer guide no.98
            m_lReturn = oSIRPartyIN.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMAdd,
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
            vClaimsRatingAgencyId:=vClaimsRatingAgencyId,
            vClaimsRatingGrading:=vClaimsRatingGrading,
            vClaimsRatingDate:=vClaimsRatingDate,
            vClaimsRatingDescription:=vClaimsRatingDescription,
            vTermsOfPaymentId:=vPaymentTermCode,
            vDomiciledForTax:=vDomiciledForTax,
            vRiskTransferAgreement:=vRiskTransferAgreement,
            vBrokerlinkSubaccount:=vBrokerlinkSubaccount, vBrokerlinkUnderwritingid:=vBrokerlinkUnderwritingid, vIsRIBroker:=vIsRIBroker, vCboLockingTypeId:=vCboLockingTypeId, vRiskTransferEditable:=vRiskTransferEditable, vInsurerTypeId:=vInsurerTypeId, vBureauAccountParty:=vBureauAccountParty,
                sUniqueId:=sUniqueId, sScreenHierarchy:=sScreenHierarchy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oSIRPartyIN = Nothing
                Return result
            End If

            ' Add SIRPartyIN to collection
            If m_oSIRPartyINs.Count = 0 Then
                m_oSIRPartyINs.Add(Nothing)
            End If
            m_lReturn = CType(m_oSIRPartyINs.Add(oNewSIRPartyIN:=oSIRPartyIN), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyIN = Nothing
                Return result
            End If

            ' CTAF 10800 - Added ABICodeOn81
            'Added resolved name

            m_lReturn = oSIRPartyIN.bSIRParty.EditAdd(lRow:=lRow, vPartyTypeId:=ToSafeInteger(lPartyTypeId), vShortName:=vShortName, vName:=vName, vResolvedName:=vName, vCurrencyID:=vCurrencyID, vPFFrequencyID:=vPaymentTermCode, vStatements:=vStatements, vABICodeOn81:=vABICodeOn81, vSourceId:=vBranchID, vSubBranchID:=vSubBranchID, sUniqueId:=ToSafeString(sUniqueId), sScreenHierarchy:=ToSafeString(sScreenHierarchy))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyIN = Nothing
                Return result
            End If
            oSIRPartyIN = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditUpdate (Public)
    '
    ' Description: Validates that this action is valid on the SIRPartyIN
    '              specified and updates the SIRPartyIN with the new values.
    '
    ' ***************************************************************** '
    ' RAW 18/12/2002 : PS187 : Added WH TaxType, WH Tax Rate, Tax Reg No, Tax Code & PaymentDetails
    'DC150803 -PS254 -fsa compliance
    'ECK Datasure 10102005 Claims Rating Agency
    Public Function EditUpdate(ByRef lRow As Object, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vAgencyNumber As Object = Nothing, Optional ByRef vBinderIndicator As Object = Nothing, Optional ByRef vReportIndicator As Object = Nothing, Optional ByRef vIsReinsurer As Object = Nothing, Optional ByRef vReinsuranceType As Object = Nothing, Optional ByRef vIsReinsuranceDebitCreditNo As Object = Nothing, Optional ByRef vDefaultCommRate As Object = Nothing, Optional ByRef vShortName As Object = Nothing, Optional ByRef vName As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vPaymentTermCode As Object = Nothing, Optional ByRef vStatements As Object = Nothing, Optional ByRef vABICodeOn81 As Object = Nothing, Optional ByRef vTaxGroupID As Object = Nothing, Optional ByRef vPaymentMethod As Object = Nothing, Optional ByRef vPaymentFrequency As Object = Nothing, Optional ByRef vBankAccount As Object = Nothing, Optional ByRef vFSAInsurerStatus As Object = Nothing, Optional ByRef vFSARegistrationNumber As Object = Nothing, Optional ByRef vFSAInsurerCreditRating As Object = Nothing, Optional ByRef vIsRetained As Object = Nothing, Optional ByRef vBranchID As Object = Nothing, Optional ByRef vSubBranchID As Object = Nothing, Optional ByRef vClaimsRatingAgencyId As Object = Nothing, Optional ByRef vClaimsRatingGrading As Object = Nothing, Optional ByRef vClaimsRatingDate As Object = Nothing, Optional ByRef vClaimsRatingDescription As Object = Nothing, Optional ByRef vTermsOfPaymentId As Object = Nothing, Optional ByRef vDomiciledForTax As Object = Nothing, Optional ByRef vRiskTransferAgreement As Object = Nothing, Optional ByRef vBrokerlinkSubaccount As Object = Nothing, Optional ByRef vBrokerlinkUnderwritingid As Object = Nothing, Optional ByRef vIsRIBroker As Object = Nothing, Optional ByRef vCboLockingTypeId As Object = Nothing, Optional ByRef vRiskTransferEditable As Object = Nothing, Optional ByRef vInsurerTypeId As Object = Nothing, Optional ByRef vBureauAccountParty As Object = Nothing, Optional sUniqueId As String = "", Optional sScreenHierarchy As String = "") As Integer


        Dim result As Integer = 0
        Dim oSIRPartyIN As bSIRPartyIN.SIRPartyIN
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oSIRPartyINs.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oSIRPartyIN = m_oSIRPartyINs.Item(lRow)

            ' Check the Status of the SIRPartyIN

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oSIRPartyIN.DatabaseStatus
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Leave Status as Add
                    iStatus = gPMConstants.PMEComponentAction.PMAdd
                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                    ' Error
                    result = gPMConstants.PMEReturnCode.PMFalse
                Case Else
                    ' Set Edit (Update) Status
                    iStatus = gPMConstants.PMEComponentAction.PMEdit
            End Select

            ' Update SIRPartyIN Attributes
            ' RAW 18/12/2002 : PS187 : Added WH TaxType, WH Tax Rate, Tax Reg No, Tax Code & PaymentMethod, PaymentFrequency, BankAccount
            'DC150803 -PS254 -fsa compliance

            'developer guide no.98
            m_lReturn = oSIRPartyIN.SetProperties(iStatus:=iStatus,
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
        vClaimsRatingAgencyId:=vClaimsRatingAgencyId,
        vClaimsRatingGrading:=vClaimsRatingGrading,
        vClaimsRatingDate:=vClaimsRatingDate,
        vClaimsRatingDescription:=vClaimsRatingDescription,
        vTermsOfPaymentId:=vPaymentTermCode,
        vDomiciledForTax:=vDomiciledForTax,
        vRiskTransferAgreement:=vRiskTransferAgreement,
        vBrokerlinkSubaccount:=vBrokerlinkSubaccount, vBrokerlinkUnderwritingid:=vBrokerlinkUnderwritingid, vIsRIBroker:=vIsRIBroker, vCboLockingTypeId:=vCboLockingTypeId, vRiskTransferEditable:=vRiskTransferEditable, vInsurerTypeId:=vInsurerTypeId, vBureauAccountParty:=vBureauAccountParty, sUniqueId:=sUniqueId, sScreenHierarchy:=sScreenHierarchy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oSIRPartyIN = Nothing
                Return result
            End If

            'Added resolved name

            m_lReturn = oSIRPartyIN.bSIRParty.EditUpdate(lRow:=lRow, vShortName:=vShortName, vName:=vName, vResolvedName:=vName, vCurrencyID:=vCurrencyID, vPFFrequencyID:=vPaymentTermCode, vStatements:=vStatements, vABICodeOn81:=vABICodeOn81, vSourceId:=vBranchID, vSubBranchID:=vSubBranchID, sUniqueId:=ToSafeString(sUniqueId), sScreenHierarchy:=ToSafeString(sScreenHierarchy))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyIN = Nothing
                Return result
            End If
            ' Release reference to SIRPartyIN
            oSIRPartyIN = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditUpdate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditDelete (Public)
    '
    ' Description: Validate that the specified SIRPartyIN can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oSIRPartyIN As bSIRPartyIN.SIRPartyIN

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oSIRPartyINs.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oSIRPartyIN = m_oSIRPartyINs.Item(lRow)

            ' Check the Status of the SIRPartyIN

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oSIRPartyIN.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oSIRPartyIN.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                oSIRPartyIN.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
            End If


            m_lReturn = oSIRPartyIN.bSIRParty.EditDelete(lRow:=ToSafeInteger(lRow))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyIN = Nothing
                Return result
            End If
            ' Release reference to SIRPartyIN
            oSIRPartyIN = Nothing

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditDelete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Cancel (Public)
    '
    ' Description: Checks the Collection to see if Cancel is OK.
    '              i.e. Do we need any Adding, Deleting or Updating.
    '
    '              Returns PMTrue if all items are clean
    '                      (PMView or PMDummyDelete)
    '              Otherwise returns PMDataChanged.
    ' ***************************************************************** '
    Public Function Cancel() As Integer
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Loop round Collection
            For lSub As Integer = 1 To m_oSIRPartyINs.Count()
                Select Case m_oSIRPartyINs.Item(lSub).DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing
                    Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMDelete
                        result = gPMConstants.PMEReturnCode.PMDataChanged
                        Exit For
                End Select
            Next lSub

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cancel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Cancel", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Update (Public)
    '
    ' Description: Loops round the collection, doing any required
    '              Adds, Deletes or Updates.
    '
    ' ***************************************************************** '
    Public Function Update() As Integer

        Dim result As Integer = 0
        Dim lSub As Integer
        Dim oSIRPartyIN As bSIRPartyIN.SIRPartyIN = Nothing
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub = 1 To m_oSIRPartyINs.Count()
                oSIRPartyIN = m_oSIRPartyINs.Item(lSub)


                Select Case oSIRPartyIN.DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing

                    Case gPMConstants.PMEComponentAction.PMAdd

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Add Item

                        m_lReturn = oSIRPartyIN.bSIRParty.Update()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                        ' Retrieve Primary Key of Core Item added

                        PartyCnt = oSIRPartyIN.bSIRParty.PartyCnt
                        oSIRPartyIN.PartyCnt = PartyCnt

                        m_lReturn = CType(oSIRPartyIN.AddItem(), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                    Case gPMConstants.PMEComponentAction.PMEdit

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Update Item
                        m_lReturn = CType(oSIRPartyIN.UpdateItem(), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If


                        m_lReturn = oSIRPartyIN.bSIRParty.Update()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                    Case gPMConstants.PMEComponentAction.PMDelete

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Delete Item
                        m_lReturn = CType(oSIRPartyIN.DeleteItem(), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If


                        m_lReturn = oSIRPartyIN.bSIRParty.Update()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If
                End Select

            Next lSub

            ' Retain the Primary Key of the SIRPartyIN
            With oSIRPartyIN
                PartyCnt = .PartyCnt
            End With

            ' Release last reference
            oSIRPartyIN = Nothing

            ' Check to see if we started a Transaction (i.e. Were any updates done)
            If bTransStarted Then

                ' Commit if OK, or Rollback any errors
                If result = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Refresh the Collection Items D/B Status
                    lSub = 1

                    ' For each item in the collection
                    Do While lSub <= m_oSIRPartyINs.Count()

                        ' With the item
                        With m_oSIRPartyINs.Item(lSub)


                            Select Case .DatabaseStatus
                                ' Delete or Dummy Delete
                                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                                    ' Remove from Collection
                                    m_oSIRPartyINs.Delete(lSub)

                                    ' Anything Else
                                Case Else
                                    ' Set Status to view
                                    .DatabaseStatus = gPMConstants.PMEComponentAction.PMView
                                    lSub += 1

                            End Select

                        End With

                    Loop

                Else

                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' Name: CheckReference (Public)
    '
    ' Description: Checks if the passed shortname (reference) already exists
    '
    ' ***************************************************************** '
    Public Function CheckReference(ByRef sReference As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CheckReference"

        Dim oParty As bSIRParty.Business
        Dim bExists As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Create an instance of the main party business object
            oParty = New bSIRParty.Business()
            m_lReturn = oParty.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("oParty.Initialise", "oParty = bSIRParty.Business", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Pass in the account code to see if it already exists
            m_lReturn = oParty.CheckIfAccountCodeExists(v_sAccountCode:=sReference, r_bExists:=bExists)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("oParty.CheckIfAccountCodeExists", "v_sAccountCode:=" & sReference, gPMConstants.PMELogLevel.PMLogError)
            End If

            'If the Reference was found then return empty string
            If bExists Then
                sReference = ""
            End If

            Return result

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here

            '		Return result

            ' This is for debugging only
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function


    ' PRIVATE Methods (Begin)

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

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetAddressDetails
    '
    ' Description: Get address details for party.
    '
    ' ***************************************************************** '
    Public Function GetAddressDetails(ByRef vAddresses As Object) As Integer

        Dim result As Integer = 0
        Dim oSIRPartyIN As bSIRPartyIN.SIRPartyIN


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyIN - need to hit core for address stuff
            oSIRPartyIN = New bSIRPartyIN.SIRPartyIN()

            m_lReturn = CType(oSIRPartyIN.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)



            m_lReturn = oSIRPartyIN.bSIRParty.GetAddressDetails(vPartyCnt:=ToSafeInteger(m_lPartyCnt), vAddresses:=vAddresses)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRPartyIN = Nothing


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetAddressDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAddressDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetAddressTypeLookups
    '
    ' Description: Get address type lookups.
    '
    ' ***************************************************************** '
    Public Function GetAddressTypeLookups(ByRef vAddressTypes As Object) As Integer

        Dim result As Integer = 0
        Dim oSIRPartyIN As bSIRPartyIN.SIRPartyIN


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyIN - need to hit core for address stuff
            oSIRPartyIN = New bSIRPartyIN.SIRPartyIN()

            m_lReturn = CType(oSIRPartyIN.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)



            m_lReturn = oSIRPartyIN.bSIRParty.GetAddressTypeLookups(vAddressTypes:=vAddressTypes)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRPartyIN = Nothing


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetAddressTypeLookups Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAddressTypeLookups", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetContactDetails
    '
    ' Description: Get contact details for party.
    '
    ' ***************************************************************** '
    Public Function GetContactDetails(ByRef vContacts As Object) As Integer

        Dim result As Integer = 0
        Dim oSIRPartyIN As bSIRPartyIN.SIRPartyIN


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyIN - need to hit core for address stuff
            oSIRPartyIN = New bSIRPartyIN.SIRPartyIN()

            m_lReturn = CType(oSIRPartyIN.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)



            m_lReturn = oSIRPartyIN.bSIRParty.GetContactDetails(vPartyCnt:=ToSafeInteger(m_lPartyCnt), vContacts:=vContacts)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRPartyIN = Nothing


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetContactDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetContactDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: UpdateAddresses
    '
    ' Description: Update the party_address usage table with old
    ' and new addresses for the party.
    '
    ' ***************************************************************** '
    Public Function UpdateAddresses(ByRef vPartyCnt As Object, Optional ByRef vAddAddresses As Object = Nothing, Optional ByRef vDeleteAddresses As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oSIRPartyIN As bSIRPartyIN.SIRPartyIN


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyIN - need to hit core for address updates
            oSIRPartyIN = New bSIRPartyIN.SIRPartyIN()

            m_lReturn = CType(oSIRPartyIN.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)



            m_lReturn = oSIRPartyIN.bSIRParty.UpdateAddresses(vPartyCnt:=ToSafeInteger(m_lPartyCnt), vAddAddresses:=vAddAddresses, vDeleteAddresses:=vDeleteAddresses)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRPartyIN = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateAddresses Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateAddresses", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateGemini
    '
    ' Description: Update Gemini
    '
    ' ***************************************************************** '
    Public Function UpdateGemini(ByRef vPartyCnt As Object, ByRef vTask As Object) As Integer

        Dim result As Integer = 0
        Dim oSIRPartyIN As bSIRPartyIN.SIRPartyIN


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyPC - need to hit core to update gemini
            oSIRPartyIN = New bSIRPartyIN.SIRPartyIN()

            m_lReturn = CType(oSIRPartyIN.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oSIRPartyIN.bSIRParty.UpdateGemini(vPartyCnt:=vPartyCnt, vTask:=vTask)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRPartyIN = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateGemini Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateGemini", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    ' Name: UpdateContacts
    '
    ' Description: Update the party_contact usage table with old
    ' and new contacts for the party.
    '
    ' ***************************************************************** '
    Public Function UpdateContacts(ByRef vPartyCnt As Object, Optional ByRef vAddContacts As Object = Nothing, Optional ByRef vDeleteContacts As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oSIRPartyIN As bSIRPartyIN.SIRPartyIN


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyIN - need to hit core for address updates
            oSIRPartyIN = New bSIRPartyIN.SIRPartyIN()

            m_lReturn = CType(oSIRPartyIN.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)



            m_lReturn = oSIRPartyIN.bSIRParty.UpdateContacts(vPartyCnt:=ToSafeInteger(m_lPartyCnt), vAddContacts:=vAddContacts, vDeleteContacts:=vDeleteContacts)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRPartyIN = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateContacts Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateContacts", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetABIDetails
    '
    ' Description: Get ABI details from GII.
    '
    ' ***************************************************************** '
    Public Function GetABIDetails(ByRef vABIValues(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oGIIDatabase.SQLSelect(sSQL:=ACGetABICodesSQL, sSQLName:=ACGetABICodesName, bStoredProcedure:=ACGetABICodesStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vABIValues)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetABIDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetABIDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function




    Public Sub New()
        MyBase.New()

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    ' Name: GetDefaultCountryCode
    '
    ' Description: Get Country Code (eg - 'GBR') from system home_country_id.
    '
    ' Author: RWH
    '
    ' CTAF 141200 - Changed to use vResultArray to get the result.
    '               This was stopping reports from working for some reason.
    '
    ' ***************************************************************** '

    Public Function GetDefaultCountryCode(ByVal v_iCountryID As Integer, ByRef r_sCountryCode As String) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Construct the SQL
            sSQL = "SELECT code FROM Country WHERE country_id = " & v_iCountryID

            ' Perform the query
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetDefaultCountryCode", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the result
            If Informations.IsArray(vResultArray) Then

                r_sCountryCode = CStr(vResultArray(0, 0)).Trim()
            Else
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefaultCountryCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaultCountryCode", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetStargateInsurerID
    '
    ' Description: Get the GISInsurerID by linking the Party table with the GISInsurer
    '              table using abi_81_insurer
    '
    ' Author: DP 12/11/2002 - for Stargate
    ' Change: DP 30/01/2003 - Now uses the new Party_nt column in sg_configuration
    '
    ' ***************************************************************** '
    Public Function GetStargateInsurerID(ByVal v_lPartyCnt As Integer, ByRef r_lSGInsurerID As Integer) As Integer
        Dim result As Integer = 0
        Dim lRecordCount, lInsurerID As Integer
        Dim oFields As DataRow


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        'For Stargate 1.1 sites ...
        Dim sSQL As String = "SELECT insurer_id FROM sg_configuration " & Strings.ChrW(13) & Strings.ChrW(10) &
                             "WHERE party_cnt = " & CStr(v_lPartyCnt)

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetStargateInsurerID", bStoredProcedure:=False, lNumberRecords:=0)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            'Could be that we're on a 1.2 site, so ...
            m_oDatabase.Parameters.Clear()

            sSQL = "SELECT sg_insurer_id FROM sg_insurer " & Strings.ChrW(13) & Strings.ChrW(10) &
                   "WHERE party_cnt = " & CStr(v_lPartyCnt)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetStargateInsurerID", bStoredProcedure:=False, lNumberRecords:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                lInsurerID = 0
                Return result
            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            ' Do we have any records ?
            If lRecordCount > 0 Then
                'developer guide no. 111
                oFields = m_oDatabase.Records.Item(0).Fields()
                lInsurerID = oFields("sg_insurer_id")

            Else
                lInsurerID = 0
            End If
        Else
            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            ' Do we have any records ?
            If lRecordCount > 0 Then
                'developer guide no. 111
                oFields = m_oDatabase.Records.Item(0).Fields()
                lInsurerID = oFields("insurer_id")
            Else
                lInsurerID = 0
            End If
        End If

        r_lSGInsurerID = lInsurerID

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: GetPartyCnt
    '
    ' Description: Get party count for a given reference (ie shortname)
    '
    ' ***************************************************************** '
    Public Function GetPartyCnt(Optional ByRef vPartyRef As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing) As Integer

        Dim result As Integer = 0

        Dim oSIRPartyIN As bSIRPartyIN.SIRPartyIN

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyPC - need to for to core for shortname
            oSIRPartyIN = New bSIRPartyIN.SIRPartyIN()

            m_lReturn = CType(oSIRPartyIN.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)



            m_lReturn = oSIRPartyIN.bSIRParty.GetPartyCnt(vPartyRef:=vPartyRef, vPartyCnt:=vPartyCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRPartyIN = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetPartyCnt Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartyCnt", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetSubBranches
    '
    ' Description:
    '
    ' History: 11/06/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function GetSubBranches(ByVal v_lSourceID As Integer, ByRef r_vSubBranchArray As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Return SiriusCoreFunc.GetSubBranches(v_oDatabase:=m_oDatabase, v_lSourceID:=v_lSourceID, r_vSubBranchArray:=r_vSubBranchArray)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSubBranches Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSubBranches", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetPartyDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 19-08-2005 : 360 = Taxes on Claims
    ' ***************************************************************** '
    Public Function GetPartyDetails(ByVal v_lPartyCnt As Integer, ByRef r_vResults As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPartyDetails"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue



            lReturn = m_oSIRParty.GetPartyDetails(v_lPartyCnt:=v_lPartyCnt, r_vResults:=r_vResults)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetPartyDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            Return result
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
    ' Name: UpdatePartyDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function UpdatePartyDetails(ByVal v_lPartyCnt As Integer, ByVal v_vPartyDetails As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdatePartyDetails"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            lReturn = m_oSIRParty.UpdatePartyDetails(v_lPartyCnt:=v_lPartyCnt, v_vPartyDetails:=v_vPartyDetails)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "UpdatePartyDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            Return result
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
    ' Name: GetTermsOfPayment
    '
    ' Description:
    '
    ' History: 02/05/2006 Deepak - Created.
    '
    ' ***************************************************************** '
    Public Function GetTermsOfPayment(ByVal v_lTermsOfPaymentId As Integer, ByRef r_vResult(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim lNumberOfRecords As Integer

            lNumberOfRecords = 0

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="terms_of_payment_id", vValue:=CStr(v_lTermsOfPaymentId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .SQLSelect(sSQL:=ACGetTermsOfPaymentSQL, sSQLName:=ACGetTermsOfPaymentName, bStoredProcedure:=ACGetTermsOfPayment, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResult)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed to GetTermsOfPayment", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTermsOfPayment")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTermsOfPayment Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTermsOfPayment", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetRiskCodeDetails(ByRef r_vRiskCodes(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetRiskCodeDetails"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(m_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.Parameters.Add", "sName:=party_cnt, vValue:=" & m_lPartyCnt, gPMConstants.PMELogLevel.PMLogError)
            End If


            'developer guide no. 85 (Latest Guide)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_code_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.Parameters.Add", "sName:=risk_code_id", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRiskCodeDetailsSQL, sSQLName:=ACGetRiskCodeDetailsName, bStoredProcedure:=ACGetRiskCodeDetailsStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vRiskCodes)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "sSQL:=ACGetRiskCodeDetailsSQL", gPMConstants.PMELogLevel.PMLogError)
            End If

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here

            '        Return result

            ' This is for debugging only
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    Public Function UpdateRiskCodes(ByVal v_lPartyCnt As Integer, ByVal v_vRiskCodes As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateRiskCodes"


        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            'Loop through each risk code and update it on the database

            For lLoop As Integer = v_vRiskCodes.GetLowerBound(1) To v_vRiskCodes.GetUpperBound(1)

                m_oDatabase.Parameters.Clear()

                m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(v_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oDatabase.Parameters.Add", "sName:=party_cnt, vValue:=" & m_lPartyCnt, gPMConstants.PMELogLevel.PMLogError)
                End If


                m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_code_id", vValue:=CStr(v_vRiskCodes(0, lLoop)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oDatabase.Parameters.Add", "sName:=risk_code_id", gPMConstants.PMELogLevel.PMLogError)
                End If


                m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_transfer_agreement", vValue:=CStr(v_vRiskCodes(2, lLoop)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oDatabase.Parameters.Add", "sName:=risk_transfer_agreement", gPMConstants.PMELogLevel.PMLogError)
                End If


                m_lReturn = m_oDatabase.Parameters.Add(sName:="delegated_authority", vValue:=CStr(v_vRiskCodes(3, lLoop)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oDatabase.Parameters.Add", "sName:=delegated_authority", gPMConstants.PMELogLevel.PMLogError)
                End If

                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateRiskCodeDetailsSQL, sSQLName:=ACUpdateRiskCodeDetailsName, bStoredProcedure:=ACUpdateRiskCodeDetailsStored)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oDatabase.SQLAction", "sSQL:=ACUpdateRiskCodeDetailsSQL", gPMConstants.PMELogLevel.PMLogError)
                End If

            Next

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here

            '        Return result

            ' This is for debugging only
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    Public Function GetRiskTransferForInsurerRisk(ByVal v_lPartyCnt As Integer, ByVal v_lRiskCodeId As Integer, ByRef r_bRiskTransferAgreement As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetRiskTransferForInsurerRisk"

        Dim vRiskCodes(,) As Object = Nothing

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(v_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.Parameters.Add", "sName:=party_cnt, vValue:=" & v_lPartyCnt, gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_code_id", vValue:=CStr(v_lRiskCodeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.Parameters.Add", "sName:=risk_code_id, vValue:=" & v_lRiskCodeId, gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRiskCodeDetailsSQL, sSQLName:=ACGetRiskCodeDetailsName, bStoredProcedure:=ACGetRiskCodeDetailsStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vRiskCodes)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "sSQL:=ACGetRiskCodeDetailsSQL", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Informations.IsArray(vRiskCodes) Then
                r_bRiskTransferAgreement = gPMFunctions.ToSafeBoolean(vRiskCodes(2, 0))
            End If

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here

            '        Return result

            ' This is for debugging only
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    Public Function GetCertificateYearDetails(ByRef vCertYear(,) As Object, Optional ByVal vPartyCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the party count parameter

            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(vPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetCertYearSQL, sSQLName:=ACGetCertYearName, bStoredProcedure:=ACGetCertYearStored, vResultArray:=vCertYear)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetGetCertYearFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSuppressedDocs", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function UpdateCertYear(ByRef vPartyCnt As Object, Optional ByRef vUpdateCertYear As Object = Nothing, Optional ByVal sUniqueId As String = "", Optional ByVal sScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim ScreenHierarchy As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = BeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Delete old CertYear for party if supplied

            If Not Informations.IsNothing(vUpdateCertYear) Then

                For i As Integer = vUpdateCertYear.GetLowerBound(1) To vUpdateCertYear.GetUpperBound(1)

                    If vUpdateCertYear(0, i).ToString.Trim <> "" Then
                        m_oDatabase.Parameters.Clear()

                        ' Add the party count parameter
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(vPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="Code", vValue:=CStr(vUpdateCertYear(0, i).ToString.Trim), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="Description", vValue:=CStr(vUpdateCertYear(1, i).ToString.Trim), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="StartDate", vValue:=CStr(vUpdateCertYear(2, i).ToString.Trim), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="EndDate", vValue:=CStr(vUpdateCertYear(3, i).ToString.Trim), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="UpdateStatus", vValue:=CStr(vUpdateCertYear(4, i).ToString.Trim), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="UserId", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="UniqueId", vValue:=sUniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        If Not String.IsNullOrEmpty(sUniqueId) Then
                            ScreenHierarchy = sScreenHierarchy & $"\Certificate Year({CStr(vUpdateCertYear(0, i).ToString.Trim)})"
                        End If

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="ScreenHierarchy", vValue:=ScreenHierarchy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        ' Execute SQL Statement
                        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateCertYearSQL, sSQLName:=ACUpdateCertYearName, bStoredProcedure:=ACUpdateCertYearStored)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                m_lReturn = RollbackTrans()
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                        End If
                Next i
            End If


            m_lReturn = CommitTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            m_lReturn = RollbackTrans()


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateCertYearFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateCertYear", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Public Function GetAndValidateSubAgentDetails(ByVal v_sPartyCode As String, ByVal v_dtCoverStartDate As Date, ByVal v_dtCoverEndDate As Date, ByRef r_bIsValid As Boolean) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_code", vValue:=CStr(v_sPartyCode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="CoverStartDate", vValue:=CStr(v_dtCoverStartDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="CoverEndDate", vValue:=CStr(v_dtCoverEndDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetSubAgentDetailSQL, sSQLName:=ACGetSubAgentDetailName, bStoredProcedure:=ACGetSubAgentDetailStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If vResultArray IsNot Nothing AndAlso Informations.IsArray(vResultArray) Then
                If CStr(vResultArray(0, 0)).Trim() = "VALID" Then
                    r_bIsValid = True
                Else
                    r_bIsValid = False
                End If
            Else
                r_bIsValid = True
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSubAgents Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSubAgents", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function DeleteAddress(ByVal party_cnt As Integer, ByVal address_cnt As Integer) As Integer
        Dim m_lReturn As Integer = gPMConstants.PMEReturnCode.PMTrue

        Try
            With m_oDatabase

                .Parameters.Clear()
                'Developer Guide No. 40
                .Parameters.Add("is_deleted", 1, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                .Parameters.Add("party_cnt", party_cnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                .Parameters.Add("address_cnt", address_cnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                m_lReturn = .SQLAction(sSQL:=ACDelAddressSQL, sSQLName:=ACDelAddressName, bStoredProcedure:=ACDelAddressStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                Return m_lReturn
            End With
        Catch excep As System.Exception

            m_lReturn = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Delete Addresses Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteAddress", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return m_lReturn
        End Try
    End Function
End Class

