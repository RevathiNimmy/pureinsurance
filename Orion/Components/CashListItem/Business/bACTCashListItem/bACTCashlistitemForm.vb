Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
'developer guide no.129
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Form_NET.Form")>
Public NotInheritable Class Form
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Form
    '
    ' Date: 07/04/1998
    '
    ' Description: Creatable Form class which contains all the
    '              methods, Form rules required to manipulate
    '              a Cashlistitem.
    '
    ' Edit History:
    ' ***************************************************************** '

    ' ************************************************
    ' Added to replace global variables 03/04/2007
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
    Private Const ACClass As String = "Form"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Collection of Cashlistitems (Private)
    Private m_oCashlistitems As bACTCashlistitem.Cashlistitems

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
    ' Navigate
    Private m_lNavigate As Integer
    ' Process Mode
    Private m_lProcessMode As Integer
    ' Type of Business
    Private m_sTransactionType As String = ""
    ' Effective
    Private m_dtEffectiveDate As Date

    ' Primary Keys to work with
    ' Source ID

    ' Insurance File ID
    Private m_lInsuranceFileID As Integer
    ' Risk ID
    Private m_lRiskID As Integer

    ' PM Lookup Business Component (Private)
    Private m_oLookup As BPMLOOKUP.Business

    'TN20010906
    Private m_sUnderwritingOrAgency As String = ""

    Private m_oSalvage As Object
    Private m_oRecovery As Object
    Private m_vCashListItemIds As Object

    Private Const ACCashListItemKeyName As String = "cashlistitem_id"
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    'TN20010906
    Public ReadOnly Property UnderwritingOrAgency() As String
        Get
            If m_sUnderwritingOrAgency = "" Then
                m_lReturn = CType(GetHiddenOption(r_sResult:=m_sUnderwritingOrAgency), gPMConstants.PMEReturnCode)
            End If

            If m_sUnderwritingOrAgency <> "U" Then
                m_sUnderwritingOrAgency = "A"
            End If

            Return m_sUnderwritingOrAgency
        End Get
    End Property

    Public Property CurrentRecord() As Integer
        Get

            Return m_lCurrentRecord

        End Get
        Set(ByVal Value As Integer)

            Select Case Value
                Case Is < 1
                    m_lCurrentRecord = 0
                Case Is > m_oCashlistitems.Count()
                    m_lCurrentRecord = m_oCashlistitems.Count()
                Case Else
                    m_lCurrentRecord = Value
            End Select

        End Set
    End Property

    Public ReadOnly Property RecordCount() As Integer
        Get

            ' Return Numner in Collection
            Return m_oCashlistitems.Count()

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

    Public ReadOnly Property TransactionType() As String
        Get

            Return m_sTransactionType

        End Get
    End Property

    Public ReadOnly Property EffectiveDate() As Date
        Get

            Return m_dtEffectiveDate

        End Get
    End Property

    Public Property SourceID() As Integer
        Get

            Return m_iSourceID

        End Get
        Set(ByVal Value As Integer)

            m_iSourceID = Value

        End Set
    End Property

    Public Property InsuranceFileID() As Integer
        Get

            Return m_lInsuranceFileID

        End Get
        Set(ByVal Value As Integer)

            m_lInsuranceFileID = Value

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

    Public WriteOnly Property CashListItemIds() As Object
        Set(ByVal Value As Object)
            m_vCashListItemIds = Value
        End Set
    End Property

    Public ReadOnly Property CashListItemsIds() As Object
        Get
            Return m_vCashListItemIds
        End Get
    End Property
    Public ReadOnly Property Details() As Cashlistitems
        Get
            Return m_oCashlistitems
        End Get
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFOrion
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


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialisation Code.

            ' Set Username and Password

            ' Set UserID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level


            ' Get Reference to Database

            m_lReturn = CType(gPMComponentServices.CheckDatabase(v_sUsername:=sUserName, v_iSourceID:=iSourceID, v_iLanguageID:=iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFOrion, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)
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

            ' Default RiskID to 0 (So we will work at Ins File Level by default)
            m_lRiskID = 0

            ' Create Cashlistitems Collection
            m_oCashlistitems = New bACTCashlistitem.Cashlistitems()

            ' Create PM Lookup Business Object
            m_oLookup = New BPMLOOKUP.Business()

            ' Initialise PM Lookup Business passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUsername:=sUserName, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFOrion





            ' Initialise the process modes.
            'm_iTask% = PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                    m_oLookup = Nothing
                End If
                m_oCashlistitems = Nothing
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
                End If
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



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetLookupValues (Public)
    '
    ' Description: Gets the Lookup values for a Cashlistitem.
    '
    '
    ' ***************************************************************** '
    'developer guide no. 101
    Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray As Object, ByRef iLanguageID As Integer, ByRef vResultArray As Object) As Integer

        Dim result As Integer = 0
        Const CMediaType As Integer = 0
        Const CAllocationStatus As Integer = 1
        Const CAddressCountry As Integer = 2
        Const CReceiptType As Integer = 3
        Const CBankType As Integer = 4
        Const CReverseType As Integer = 5
        Const CPaymentType As Integer = 6
        Const cPaymentStatus As Integer = 7
        Const cChequeType As Integer = 8
        Const cChequeClearingType As Integer = 9

        Dim oCashListItem As bACTCashlistitem.Cashlistitem = Nothing
        Dim dtEffectiveDate As Date

        ' {* USER DEFINED CODE (Begin) *}
        Dim vTabArray As Array = Array.CreateInstance(GetType(Object), New Integer() {4, cChequeClearingType - CMediaType + 1}, New Integer() {0, CMediaType})
        ' {* USER DEFINED CODE (End) *}

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset Result Array
            'developer guide no. 17
            vResultArray = Nothing

            ' Reset Table Array

            'developer guide no. 17
            vTableArray = Nothing

            ' {* USER DEFINED CODE (Begin) *}

            ' Setup Lookup Table Names

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, CMediaType) = gACTLibrary.ACTLookupMediaType

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, CAllocationStatus) = gACTLibrary.ACTLookupAllocationStatus

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, CAddressCountry) = gPMConstants.PMLookupCountry
            'Added Front Office receipting

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, CReceiptType) = gACTLibrary.ACTLookupReceiptType

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, CBankType) = gACTLibrary.ACTLookupBankType

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, CReverseType) = gACTLibrary.ACTLookupReverseType
            'added payment maintenance

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, CPaymentType) = gACTLibrary.ACTLookupPaymentTypeTable

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, cPaymentStatus) = gACTLibrary.ACTLookupPaymentStatus
            'WPR12- Enhancement Quote Collection Process

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, cChequeType) = gACTLibrary.ACTLookupChequeType

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, cChequeClearingType) = gACTLibrary.ACTLookupChequeClearingType
            ' {* USER DEFINED CODE (End) *}

            ' Do we have any records
            If m_lCurrentRecord < 1 Then
                ' No, we can only lookup all
                iLookupType = gPMConstants.PMELookupType.PMLookupAll
            Else
                ' Yes get current record
                oCashListItem = m_oCashlistitems.Item(m_lCurrentRecord)
            End If

            Select Case iLookupType
                Case gPMConstants.PMELookupType.PMLookupAll

                    ' Do not supply a key

                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, CMediaType) = ""

                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, CAllocationStatus) = ""

                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, CAddressCountry) = ""

                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, CReceiptType) = ""

                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, CBankType) = ""

                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, CReverseType) = ""

                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, CPaymentType) = ""

                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, cPaymentStatus) = ""
                    'WPR12- Enhancement Quote Collection Process

                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, cChequeType) = ""

                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, cChequeClearingType) = ""

                    ' Default Effective Date to current date/time
                    dtEffectiveDate = DateTime.Now

                Case gPMConstants.PMELookupType.PMLookupAllEffective

                    ' Use keys and effective date from current record
                    ' Note: The keys are not used for the select, but are used by
                    '       the iterface program to set the list index.
                    With oCashListItem

                        ' {* USER DEFINED CODE (Begin) *}

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, CMediaType) = .MediaTypeID

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, CAllocationStatus) = .AllocationstatusID

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, CAddressCountry) = .AddressCountry

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, CReceiptType) = .CashListItem_receipt_type_id

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, CBankType) = .CashListItem_bank_id

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, CReverseType) = .CashListItem_Reverse_Reason_id

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, CPaymentType) = .CashListItem_Payment_Type_id
                        'PSL 03/03/2003 Payment status not receipt status
                        '            vTabArray(PMLookupKey, cPaymentStatus) = .CashListItem_receipt_status_id

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, cPaymentStatus) = .CashListItem_Payment_Status_id

                        'BB dtEffectiveDate = .EffectiveDate
                        'BB Default Effective Date to current date/time when not present
                        dtEffectiveDate = DateTime.Now
                        ' {* USER DEFINED CODE (End) *}

                    End With

                Case gPMConstants.PMELookupType.PMLookupSingle

                    ' Set keys from current record
                    With oCashListItem

                        ' {* USER DEFINED CODE (Begin) *}

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, CMediaType) = .MediaTypeID

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, CAllocationStatus) = .AllocationstatusID

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, CAddressCountry) = .AddressCountry

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, CReceiptType) = .CashListItem_receipt_type_id

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, CBankType) = .CashListItem_bank_id

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, CReverseType) = .CashListItem_Reverse_Reason_id

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, CPaymentType) = .CashListItem_Payment_Type_id

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, cPaymentStatus) = .CashListItem_Payment_Status_id
                        ' {* USER DEFINED CODE (End) *}

                    End With
                    ' Default Effective Date to current date/time
                    dtEffectiveDate = DateTime.Now

            End Select

            ' Release Cashlistitem reference
            oCashListItem = Nothing

            ' Get the Lookup items
            m_lReturn = m_oLookup.GetLookupValues(iLookupType:=iLookupType, vTableArray:=vTabArray, iLanguageID:=iLanguageID, dtEffectiveDate:=dtEffectiveDate, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the Table Array

            vTableArray = vTabArray

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookupValues Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetMandatory (Public)
    '
    ' Description: Sets and returns the Mandy fields required.
    '
    ' ***************************************************************** '
    Public Function GetMandatory(Optional ByRef lContactNameMandy As Integer = 0, Optional ByRef lAddress1Mandy As Integer = 0, Optional ByRef lAddress2Mandy As Integer = 0, Optional ByRef lAddress3Mandy As Integer = 0, Optional ByRef lAddress4Mandy As Integer = 0, Optional ByRef lPostalCodeMandy As Integer = 0, Optional ByRef lAddressCountryMandy As Integer = 0, Optional ByRef lPaymentNameMandy As Integer = 0, Optional ByRef lPaymentAccountCodeMandy As Integer = 0, Optional ByRef lPaymentBranchCodeMandy As Integer = 0, Optional ByRef lPaymentExpiryDateMandy As Integer = 0, Optional ByRef lPaymentReference1Mandy As Integer = 0, Optional ByRef lPaymentReference2Mandy As Integer = 0, Optional ByRef lLetterMandy As Integer = 0, Optional ByRef lBatch_idMandy As Integer = 0, Optional ByRef lPMUser_idMandy As Integer = 0, Optional ByRef lTransaction_dateMandy As Integer = 0, Optional ByRef lOriginal_amountMandy As Integer = 0, Optional ByRef lAmount_tenderedMandy As Integer = 0, Optional ByRef lChangeMandy As Integer = 0, Optional ByRef lCashlistitem_receipt_type_idMandy As Integer = 0, Optional ByRef lCashlistitem_receipt_status_idMandy As Integer = 0, Optional ByRef lCashlistitem_bank_idMandy As Integer = 0, Optional ByRef lCheque_dateMandy As Integer = 0, Optional ByRef lCC_numberMandy As Integer = 0, Optional ByRef lCC_expiry_dateMandy As Integer = 0, Optional ByRef lCC_start_dateMandy As Integer = 0, Optional ByRef lCC_issueMandy As Integer = 0, Optional ByRef lCC_pinMandy As Integer = 0, Optional ByRef lCC_auth_codeMandy As Integer = 0, Optional ByRef lReceipt_detailsMandy As Integer = 0, Optional ByRef lCashlistitem_reverse_pmuser_idMandy As Integer = 0, Optional ByRef lCashlistitem_reverse_reason_idMandy As Integer = 0, Optional ByRef lCashlistitem_payment_type_idMandy As Integer = 0, Optional ByRef lCashlistitem_payment_method_idMandy As Integer = 0, Optional ByRef lCashlistitem_payment_status_idMandy As Integer = 0, Optional ByRef lDate_presentedMandy As Integer = 0, Optional ByRef lCheque_in_possessionMandy As Integer = 0, Optional ByRef lStop_requested_dateMandy As Integer = 0, Optional ByRef lStop_printed_dateMandy As Integer = 0, Optional ByRef lStop_confirmation_dateMandy As Integer = 0, Optional ByRef lReasonMandy As Integer = 0, Optional ByRef lReplaces_cashlistitem_idMandy As Integer = 0, Optional ByRef lXML_ObjectMandy As Integer = 0) As Integer

        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Mandy fields.

            lContactNameMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lAddress1Mandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lAddress2Mandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lAddress3Mandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lAddress4Mandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lPostalCodeMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lAddressCountryMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lPaymentNameMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lPaymentAccountCodeMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lPaymentBranchCodeMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lPaymentExpiryDateMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lPaymentReference1Mandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lPaymentReference2Mandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lLetterMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lBatch_idMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lPMUser_idMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lTransaction_dateMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lOriginal_amountMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lAmount_tenderedMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lChangeMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lCashlistitem_receipt_type_idMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lCashlistitem_receipt_status_idMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lCashlistitem_bank_idMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lCheque_dateMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lCC_numberMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lCC_expiry_dateMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lCC_start_dateMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lCC_issueMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lCC_pinMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lCC_auth_codeMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lReceipt_detailsMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lCashlistitem_reverse_pmuser_idMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lCashlistitem_reverse_reason_idMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lCashlistitem_payment_type_idMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lCashlistitem_payment_method_idMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lCashlistitem_payment_status_idMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lDate_presentedMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lCheque_in_possessionMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lStop_requested_dateMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lStop_printed_dateMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lStop_confirmation_dateMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lReasonMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lReplaces_cashlistitem_idMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            lXML_ObjectMandy = gPMConstants.PMEMandatoryStatus.PMNonMandatory

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetMandatory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMandatory", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectAdd (Public)
    '
    ' Description: Adds a single Cashlistitem directly into the database.
    '        Note: The Cashlistitem will NOT be added to the collection.
    '
    ' ***************************************************************** '
    Public Function DirectAdd(ByRef r_vCashListItem() As Object) As Integer

        Dim result As Integer = 0
        Dim oCashListItem As bACTCashlistitem.Cashlistitem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Create a new Cashlistitem
            oCashListItem = New bACTCashlistitem.Cashlistitem()

            'Populate Cashlistitem Attributes
            m_lReturn = CType(SetProperties(oCashListItem, gPMConstants.PMEComponentAction.PMAdd, r_vCashListItem), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            'Add the Cashlistitem to the Database
            m_lReturn = CType(AddItem(oCashListItem), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Return the ID of the Cashlistitem Added

            r_vCashListItem(gACTLibrary.eCashListItem.CashlistitemID) = oCashListItem.CashlistitemID

            oCashListItem = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectDelete (Public)
    '
    ' Description: Deletes a single Cashlistitem directly from the database.
    '        Note: The Cashlistitem will NOT be deleted from the collection.
    '
    ' ***************************************************************** '
    Public Function DirectDelete(ByVal v_vCashListItem() As Object) As Integer

        Dim result As Integer = 0
        Dim oCashListItem As bACTCashlistitem.Cashlistitem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new Cashlistitem
            oCashListItem = New bACTCashlistitem.Cashlistitem()

            ' Populate Cashlistitem Attributes
            m_lReturn = CType(SetProperties(oCashListItem, gPMConstants.PMEComponentAction.PMDelete, v_vCashListItem), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Cashlistitem to the Database
            m_lReturn = CType(DeleteItem(oCashListItem), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oCashListItem = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectDelete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetCaptions (Public)
    '
    ' Description: Get the requested caption fields for a record.
    '
    ' ***************************************************************** '
    Public Function GetCaptions(ByRef vID As Object, ByRef vFieldArray() As Object, ByRef vResultArray() As Object) As Integer
        Return GetCaptions(vID:=vID, vFieldArray:=vFieldArray, vResultArray:=vResultArray, vTable:=Nothing)
    End Function

    Public Function GetCaptions(ByRef vID As Object, ByRef vFieldArray() As Object, ByRef vResultArray() As Object, ByRef vTable As Object) As Integer

        Dim result As Integer = 0
        'Developer Guide No. 21
        Dim oFields As DataRow

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Informations.IsArray(vFieldArray) Then
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:="Parameter vFieldArray must be a Variant Array", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCaptions")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Captions ourself

            ' Check that this record exists
            m_lReturn = CType(CheckID(vID:=vID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Resize the Temporary Results Array
            Dim vResults(vFieldArray.GetUpperBound(0)) As Object

            ' Get a reference to the Fields returned
            'Developer Guide No. 111
            oFields = m_oDatabase.Records.Item(0).Fields()

            With oFields

                ' For Each Field requested
                For lSub As Integer = vFieldArray.GetLowerBound(0) To vFieldArray.GetUpperBound(0)

                    'PWF 230702 - scalability change - check for null value

                    If Not (Convert.IsDBNull(oFields(vFieldArray(lSub))) Or Informations.IsNothing(oFields(vFieldArray(lSub)))) Then
                        ' Store the results in the Temporary results array

                        vResults(lSub) = oFields(vFieldArray(lSub))
                    Else
                        Select Case oFields(vFieldArray(lSub)).Type
                            Case DbType.String, DbType.String, DbType.String, ADODB.DataTypeEnum.adLongVarWChar, DbType.String, DbType.String, ADODB.DataTypeEnum.adWChar

                                vResults(lSub) = ""
                            Case DbType.Date, ADODB.DataTypeEnum.adDBDate

                                vResults(lSub) = -1
                            Case Else

                                vResults(lSub) = 0
                        End Select
                    End If

                Next lSub

            End With

            ' Assign the results
            vResultArray = vResults

            ' Release the reference to the Fields
            oFields = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCaptions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCaptions", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the required CashListItems and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetDetails(Optional ByRef vCashlistitemID As Object = Nothing, Optional ByRef vCashListID As Object = Nothing, Optional ByRef vLockMode As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        Dim oCashListItem As bACTCashlistitem.Cashlistitem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Collection
            m_oCashlistitems.Clear()

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Default to No Lock if not supplied or not numeric
            Dim dbNumericTemp As Double

            If (Informations.IsNothing(vLockMode)) Or (Not Double.TryParse(CStr(vLockMode), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                vLockMode = 0
            End If

            ' Do we have a key

            If Not Informations.IsNothing(vCashlistitemID) Then

                ' Yes, Is the key valid

                Dim dbNumericTemp2 As Double
                If Not Double.TryParse(CStr(vCashlistitemID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error Message

                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vCashListItemID =" & CStr(vCashlistitemID), vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                    Return result

                End If

                ' Add the CashListItemID parameter (INPUT)

                m_lReturn = m_oDatabase.Parameters.Add(sName:="CashListItem_id", vValue:=CStr(vCashlistitemID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetDetailsSQL, sSQLName:=ACGetDetailsName, bStoredProcedure:=ACGetDetailsStored, lNumberRecords:=0)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else

                'BB No Item ID so we should have a Cash List ID
                ' to get all items belonging to a given Cash List

                'BB Check Cash List ID is present

                If Informations.IsNothing(vCashListID) Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Cash List ID is missing", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")
                    Return result
                End If

                'BB Check Cash List ID is numeric

                Dim dbNumericTemp3 As Double
                If Not Double.TryParse(CStr(vCashListID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message

                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vCashListID =" & CStr(vCashListID), vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")
                    Return result
                End If

                'BB Add the CashListID parameter (INPUT)

                m_lReturn = m_oDatabase.Parameters.Add(sName:="CashList_id", vValue:=CStr(vCashListID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllDetailsSQL, sSQLName:=ACGetAllDetailsName, bStoredProcedure:=ACGetAllDetailsStored, lNumberRecords:=0)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            ' Do we have any records ?

            If lRecordCount < 1 Then

                ' No Records, return PMFalse
                Return gPMConstants.PMEReturnCode.PMNotFound

            Else

                ' Yes, load them into the collection
                'Developer Guide No 162
                For lSub As Integer = 0 To lRecordCount - 1

                    ' Create New CashListItem
                    oCashListItem = New bACTCashlistitem.Cashlistitem()

                    m_lReturn = CType(SetPropertiesFromDB(oCashListItem:=oCashListItem, lRecordNumber:=lSub), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If m_oCashlistitems.Count = 0 Then
                        m_oCashlistitems.Add(Nothing)
                    End If
                    ' Add CashListItem to collection
                    m_lReturn = CType(m_oCashlistitems.Add(oNewCashlistitem:=oCashListItem), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oCashListItem = Nothing

                Next lSub

            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetNext (Public)
    '
    ' Description: Gets the required Cashlistitems and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetNext(ByRef r_vCashListItem() As Object) As Integer

        Dim result As Integer = 0
        Dim oCashListItem As bACTCashlistitem.Cashlistitem
        Dim iStatus As gPMConstants.PMEComponentAction

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oCashlistitems.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord += 1
            Else
                result = gPMConstants.PMEReturnCode.PMEOF
            End If

            oCashListItem = m_oCashlistitems.Item(m_lCurrentRecord)

            ' Get the Cashlistitem Property Values
            m_lReturn = CType(GetProperties(oCashListItem, iStatus, r_vCashListItem), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oCashListItem = Nothing


            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNext", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditAdd (Public)
    '
    ' Description: Adds the supplied Cashlistitem into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    '
    ' ***************************************************************** '
    Public Function EditAdd(ByRef lRow As Integer, ByRef r_vCashListItem() As Object) As Integer

        Dim result As Integer = 0
        Dim oCashListItem As bACTCashlistitem.Cashlistitem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oCashlistitems.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new Cashlistitem
            oCashListItem = New bACTCashlistitem.Cashlistitem()

            ' Populate Cashlistitem Attributes
            m_lReturn = CType(SetProperties(oCashListItem, gPMConstants.PMEComponentAction.PMAdd, r_vCashListItem), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oCashListItem = Nothing
                Return result
            End If

            If m_oCashlistitems.Count = 0 Then
                m_oCashlistitems.Add(Nothing)
            End If
            ' Add Cashlistitem to collection
            m_lReturn = CType(m_oCashlistitems.Add(oNewCashlistitem:=oCashListItem), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oCashListItem = Nothing
                Return result
            End If

            oCashListItem = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditUpdate (Public)
    '
    ' Description: Validates that this action is valid on the Cashlistitem
    '              specified and updates the Cashlistitem with the new values.
    '
    ' ***************************************************************** '
    Public Function EditUpdate(ByRef lRow As Integer, ByVal v_vCashListItem() As Object) As Integer

        Dim result As Integer = 0
        Dim oCashListItem As bACTCashlistitem.Cashlistitem
        Dim iStatus As gPMConstants.PMEComponentAction

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'SMJB CQ2155 01/09/03: If we've settled an instalment plan then, we've reloaded the
            'cashlistitems collection with only our added item

            If Not Object.Equals(v_vCashListItem(gACTLibrary.eCashListItem.CashlistitemID), Nothing) Then
                If m_oCashlistitems.Count() = 1 Then

                    If CInt(v_vCashListItem(gACTLibrary.eCashListItem.CashlistitemID)) = m_oCashlistitems.Item(1).CashlistitemID Then
                        lRow = 1
                    End If
                End If
            End If

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oCashlistitems.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oCashListItem = m_oCashlistitems.Item(lRow)

            ' Check the Status of the Cashlistitem

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oCashListItem.DatabaseStatus
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

            ' Update Cashlistitem Attributes
            m_lReturn = CType(SetProperties(oCashListItem, iStatus, v_vCashListItem), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oCashListItem = Nothing
                Return result
            End If

            ' Release reference to Cashlistitem
            oCashListItem = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditUpdate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditDelete (Public)
    '
    ' Description: Validate that the specified Cashlistitem can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oCashListItem As bACTCashlistitem.Cashlistitem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oCashlistitems.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oCashListItem = m_oCashlistitems.Item(lRow)

            ' Check the Status of the Cashlistitem

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oCashListItem.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oCashListItem.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                oCashListItem.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
            End If

            ' Release reference to Cashlistitem
            oCashListItem = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditDelete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            For lSub As Integer = 1 To m_oCashlistitems.Count()
                Select Case m_oCashlistitems.Item(lSub).DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing
                    Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMDelete
                        result = gPMConstants.PMEReturnCode.PMDataChanged
                        Exit For
                End Select
            Next lSub

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cancel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Cancel", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dim oCashListItem As bACTCashlistitem.Cashlistitem
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub = 1 To m_oCashlistitems.Count()
                'If lSub = 52 Then Stop
                oCashListItem = m_oCashlistitems.Item(lSub)


                Select Case oCashListItem.DatabaseStatus
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
                        m_lReturn = CType(AddItem(oCashListItem), gPMConstants.PMEReturnCode)
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
                        m_lReturn = CType(UpdateItem(oCashListItem), gPMConstants.PMEReturnCode)
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
                        If oCashListItem.Instalment_Array(0, 0) <> -1 Then
                            m_lReturn = CType(DeleteItem(oCashListItem), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                Exit For
                            End If
                        End If
                End Select

            Next lSub

            ' Release last reference
            oCashListItem = Nothing

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
                    Do While lSub <= m_oCashlistitems.Count()

                        ' With the item
                        With m_oCashlistitems.Item(lSub)


                            Select Case .DatabaseStatus
                                ' Delete or Dummy Delete
                                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                                    ' Remove from Collection
                                    m_oCashlistitems.Delete(lSub)

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



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Public Function GetDocumentIdOnInsuranceCnt(ByRef lInsuranceCnt As Integer, ByRef ldocumentId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetDocumentIdOnInsuranceCnt"
        Dim vArray(,) As Object = Nothing
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "insurance_file", lInsuranceCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetDocumentIdFromInsuranceFileSQL, sSQLName:="", bStoredProcedure:=True, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACGetDocumentIdFromInsuranceFileSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            ElseIf Informations.IsArray(vArray) Then

                ldocumentId = CInt(vArray(0, 0))
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
    ' Name: DeleteUserProperty
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function DeleteUserProperty(ByVal v_sPropertyName As String, ByVal v_bDeleteAll As Boolean) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If v_bDeleteAll Then

                ' Delete all the properties
                m_lReturn = CType(gPMComponentServices.NewDeleteAllUserProperties(v_sUsername:=m_sUsername), gPMConstants.PMEReturnCode)

            Else

                ' Delete just the one property
                m_lReturn = CType(gPMComponentServices.NewDeleteUserProperty(v_sUsername:=m_sUsername, v_sPropertyName:=v_sPropertyName), gPMConstants.PMEReturnCode)

            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteUserProperty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteUserProperty", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: UpdateUserProperty
    '
    ' Description: Updates a user value via server component services.
    '
    ' ***************************************************************** '
    Public Function UpdateUserProperty(ByVal v_sPropertyName As String, ByVal v_vPropertyValue As Object) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new instance of component services

            ' call the component services to udate the property
            m_lReturn = CType(gPMComponentServices.NewUpdateUserProperty(v_sUsername:=m_sUsername, v_sPropertyName:=v_sPropertyName, v_vPropertyValue:=v_vPropertyValue), gPMConstants.PMEReturnCode)

            ' Remove the instance


            Return m_lReturn

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateUserProperty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateUserProperty", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetUserProperty
    '
    ' Description: Gets a user property using service component services
    '
    ' ***************************************************************** '
    Public Function GetUserProperty(ByVal v_sPropertyName As String, ByRef r_vPropertyValue As Object) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new instance of component services

            ' call the component services to udate the property
            m_lReturn = CType(gPMComponentServices.GetUserProperty(v_sUsername:=m_sUsername, v_sPropertyName:=v_sPropertyName, r_vPropertyValue:=r_vPropertyValue), gPMConstants.PMEReturnCode)

            ' Remove the instance


            Return m_lReturn

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserProperty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserProperty", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: AddItem (Public)
    '
    ' Description: Adds to the Database from the Base Details.
    '
    ' ***************************************************************** '
    Private Function AddItem(ByRef oCashListItem As bACTCashlistitem.Cashlistitem) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' CTAF 191200 - Rearranged parameters

        ' Add CashlistitemID as an OUTPUT param for an insert
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Cashlistitem_id", vValue:=CStr(oCashListItem.CashlistitemID), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add the required INPUT parameters
        m_lReturn = CType(AddInputParam(oCashListItem:=oCashListItem), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored, lRecordsAffected:=lRecordsAffected)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the ID of the record inserted
        oCashListItem.CashlistitemID = gPMFunctions.NullToLong(m_oDatabase.Parameters.Item("Cashlistitem_id").Value)

        'added sw front office receipting 02-11-2002
        'Add any instalment records that relate to this cashlistitem
        If Informations.IsArray(oCashListItem.Instalment_Array) AndAlso oCashListItem.Instalment_Array(0, 0) <> -1 Then

            m_lReturn = CType(CreateCashlistItemInstalments(oCashListItem.Instalment_Array, oCashListItem.CashlistitemID), gPMConstants.PMEReturnCode)
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Informations.IsArray(oCashListItem.Salvage_Array) Then
            m_lReturn = CType(CreateCashlistItemSalvage(oCashListItem.Salvage_Array, oCashListItem.CashlistitemID, oCashListItem.AccountID), gPMConstants.PMEReturnCode)
        End If
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Informations.IsArray(oCashListItem.CLMUSRecovery_Array) Then
            m_lReturn = CType(CreateCashlistItemCLMUSRecovery(oCashListItem.CLMUSRecovery_Array, oCashListItem.CLMRVRecovery_Array, oCashListItem.CashlistitemID, oCashListItem.AccountID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        If Informations.IsArray(oCashListItem.BGPolicies) Then

            m_lReturn = CType(CreateCashlistForBankGuarantee(oCashListItem.BGPolicies, oCashListItem.CashlistitemID, oCashListItem.CashlistID), gPMConstants.PMEReturnCode)
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        'end sw

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UpdateItem (Public)
    '
    ' Description: UpdateItems a single record in the database from
    '              the Base Details.
    '
    ' ***************************************************************** '
    Private Function UpdateItem(ByRef oCashListItem As bACTCashlistitem.Cashlistitem) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add CashlistitemID as an INPUT param for an update
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Cashlistitem_id", vValue:=CStr(oCashListItem.CashlistitemID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add the Input Parameters to the Parameteres collection
        m_lReturn = CType(AddInputParam(oCashListItem:=oCashListItem), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateSQL, sSQLName:=ACUpdateName, bStoredProcedure:=ACUpdateStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Check to see that the record was updated OK

        If lRecordsAffected > 0 Then
            ' Updated No action required
        Else
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'now add with new instalment details
        If Informations.IsArray(oCashListItem.Instalment_Array) AndAlso oCashListItem.Instalment_Array(0, 0) <> -1 Then
            'first delete any existing instalment records
            m_lReturn = CType(DeleteCashlistItemInstalments(oCashListItem.CashlistitemID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(CreateCashlistItemInstalments(oCashListItem.Instalment_Array, oCashListItem.CashlistitemID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        If Informations.IsArray(oCashListItem.CLMUSRecovery_Array) Then

            'first delete any existing claim mrecovery records
            m_lReturn = CType(DeleteCashlistItemCLMUSRecovery(oCashListItem.CashlistitemID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'now add the new claim recovery records
            m_lReturn = CType(CreateCashlistItemCLMUSRecovery(oCashListItem.CLMUSRecovery_Array, oCashListItem.CLMRVRecovery_Array, oCashListItem.CashlistitemID, oCashListItem.AccountID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End If

        'end changes 02-11-2002

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DeleteItem (Public)
    '
    ' Description: Deletes a single record from the database.
    '
    '
    ' ***************************************************************** '
    Private Function DeleteItem(ByRef oCashListItem As bACTCashlistitem.Cashlistitem) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the CashlistitemID INPUT parameter
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Cashlistitem_id", vValue:=CStr(oCashListItem.CashlistitemID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteSQL, sSQLName:=ACDeleteName, bStoredProcedure:=ACDeleteStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' If record wasn't deleted, error
        If lRecordsAffected > 0 Then
            ' Deleted, No action required
        Else
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'first delete any existing instalment records
        m_lReturn = CType(DeleteCashlistItemInstalments(oCashListItem.CashlistitemID), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'first delete any existing salvage records
        m_lReturn = CType(DeleteCashlistItemSalvage(oCashListItem.CashlistitemID), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ''' <summary>
    ''' Sets the supplied Cashlistitem properties from a database record
    ''' </summary>
    ''' <param name="oCashListItem"></param>
    ''' <param name="lRecordNumber"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetPropertiesFromDB(ByRef oCashListItem As bACTCashlistitem.Cashlistitem,
                                         ByRef lRecordNumber As Integer) As Integer


        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim oFields As DataRow


        ' Set oFields to refer to Field Collection for Record 1
        oFields = m_oDatabase.Records.Item(lRecordNumber).Fields()

        With oCashListItem
            .CashlistitemID = NullToLong(oFields("cashlistitem_id"))
            .AllocationstatusID = NullToLong(oFields("allocationstatus_id"))
            .MediaTypeID = NullToLong(oFields("mediatype_id"))
            .CashlistID = NullToLong(oFields("cashlist_id"))
            .AccountID = NullToLong(oFields("account_id"))
            .MediaRef = NullToString(oFields("media_ref"))
            .OurRef = NullToString(oFields("our_ref"))
            .TheirRef = NullToString(oFields("their_ref"))
            .Amount = CDec(NullToDecimal(oFields("amount")))
            .TransdetailID = NullToLong(oFields("transdetail_id"))
            .ContactName = NullToString(oFields("contact_name"))
            .Address1 = NullToString(oFields("address1"))
            .Address2 = NullToString(oFields("address2"))
            .Address3 = NullToString(oFields("address3"))
            .Address4 = NullToString(oFields("address4"))
            .PostalCode = NullToString(oFields("postal_code"))
            .AddressCountry = NullToLong(oFields("address_country"))
            .PaymentName = NullToString(oFields("payment_name"))
            .PaymentAccountCode = NullToString(oFields("payment_account_code"))
            .PaymentBranchCode = NullToString(oFields("payment_branch_code"))
            .PaymentExpiryDate = NullToDate(oFields("payment_expiry_date"))
            .PaymentReference1 = NullToString(oFields("payment_reference1"))
            .PaymentReference2 = NullToString(oFields("payment_reference2"))
            .Letter = NullToBoolean(oFields("letter"))
            .Batch_id = NullToLong(oFields("batch_id"))
            .pmuser_id = NullToLong(oFields("pmuser_id"))
            .Transaction_Date = NullToDate(oFields("transaction_date"))
            .Original_Amount = NullToDouble(oFields("original_amount"))
            .Amount_Tendered = NullToDouble(oFields("amount_tendered"))
            .Change = NullToDouble(oFields("change"))
            .CashListItem_receipt_type_id = NullToLong(oFields("cashlistitem_receipt_type_id"))
            .CashListItem_receipt_status_id = NullToLong(oFields("cashlistitem_receipt_status_id"))
            .CashListItem_bank_id = NullToLong(oFields("cashlistitem_bank_id"))
            .Cheque_Date = NullToDate(oFields("cheque_date"))
            .CC_Number = NullToString(oFields("cc_number"))
            .CC_Expiry_Date = NullToString(oFields("cc_expiry_date"))
            .CC_Start_Date = NullToString(oFields("cc_start_date"))
            .CC_Issue = NullToString(oFields("cc_issue"))
            .CC_Pin = NullToString(oFields("cc_pin"))
            .CC_Auth_Code = NullToString(oFields("cc_auth_code"))
            .Receipt_Details = NullToString(oFields("receipt_details"))
            .CashListItem_Reverse_PMUser_id = NullToInteger(oFields("cashlistitem_reverse_pmuser_id"))
            .CashListItem_Reverse_Reason_id = NullToLong(oFields("cashlistitem_reverse_reason_id"))
            .CashListItem_Payment_Type_id = NullToLong(oFields("cashlistitem_payment_type_id"))
            .CashListItem_Payment_Status_id = NullToLong(oFields("cashlistitem_payment_status_id"))
            .Date_Presented = NullToDate(oFields("date_presented"))
            .Cheque_in_Possession = NullToInteger(oFields("cheque_in_possession"))
            .Stop_Requested_Date = NullToDate(oFields("stop_requested_date"))
            .Stop_Printed_Date = NullToDate(oFields("stop_printed_date"))
            .Stop_Confirmation_Date = NullToDate(oFields("stop_confirmation_date"))
            .Reason = NullToString(oFields("reason"))
            .Replaces_CashListItem_id = NullToLong(oFields("replaces_cashlistitem_id"))
            .XML_Object = NullToString(oFields("xml_object"))
            .UnderwritingYearID = NullToString(oFields("underwriting_year_id"))
            .CurrencyBaseDate = NullToDate(oFields("currency_base_date"))
            .CurrencyBaseXrate = NullToDouble(oFields("currency_base_xrate"))
            .AccountBaseDate = NullToDate(oFields("account_base_date"))
            .AccountBaseXrate = NullToDouble(oFields("account_base_xrate"))
            .SystemBaseDate = NullToDate(oFields("system_base_date"))
            .SystemBaseXrate = NullToDouble(oFields("system_base_xrate"))
            .OverrideReason = NullToLong(oFields("exchange_rate_override_reason_id"))
            .CC_Name = NullToString(oFields("cc_name"))
            .CC_Customer = NullToString(oFields("cc_customer"))
            .CC_Manual_Auth_Code = NullToString(oFields("CC_Manual_Auth_Code"))
            .CC_Transaction_Code = NullToString(oFields("CC_Transaction_Code"))
            .MediaTypeIssuerID = NullToLong(oFields("mediatype_issuer_id"))
            .PartyBankId = oFields("Party_bank_id")
            .CollectionDate = NullToDate(oFields("collection_date"))
            .Comments = NullToString(oFields("comments"))
            .DatabaseStatus = gPMConstants.PMEComponentAction.PMView
            .IsInstalment = NullToBoolean(oFields("is_instalment")) 'PN 56851
            'WPR12- Enhancement Quote Collection Process
            .BankLocation = NullToString(oFields("bank_location"))
            .BankBranch = NullToString(oFields("bank_branch"))
            .ChequeTypeId = CInt(NullToDouble(oFields("chequetype_id")))
            .CCBankId = CInt(NullToDouble(oFields("cc_bank_id")))
            .CardTypeId = CInt(NullToDouble(oFields("type_of_card_id")))
            .CardTransSlipNo = NullToString(oFields("cc_trans_slip_no"))
            .ChequeClearingTypeId = CInt(NullToDouble(oFields("Cheque_clearing_type_id")))
            .SplitTotal = CDec(NullToDecimal(oFields("split_total")))
            .IsLeadAccount = (NullToBoolean(oFields("is_lead")))
            .TaxAmount = CDec(NullToCurrency(oFields("taxamount")))
            .TaxBandID = (NullToInteger(oFields("tax_band_id")))
            .BIC = NullToString(oFields("business_identifier_code"))
            .IBAN = NullToString(oFields("international_bank_account_number"))
            .InsuranceRef = gPMFunctions.NullToString(oFields("Insurance_Ref"))
        End With

        Return nResult

    End Function


    ''' <summary>
    ''' Sets the supplied Cashlistitem property values.
    ''' </summary>
    ''' <param name="oCashListItem"></param>
    ''' <param name="iStatus"></param>
    ''' <param name="vCashListItemArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetProperties(ByRef oCashListItem As bACTCashlistitem.Cashlistitem,
                                   ByRef iStatus As Integer,
                                   ByRef vCashListItemArray() As Object) As Integer

        Dim bSetProperties As Boolean = False
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim bDataChanged As Boolean

        Try
            bSetProperties = True
            bDataChanged = False

            With oCashListItem

                If Not Object.Equals(vCashListItemArray(eCashListItem.CashlistitemID), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.CashlistitemID) Is DBNull.Value Then

                    If .CashlistitemID <> CDbl(vCashListItemArray(eCashListItem.CashlistitemID)) Then
                        .CashlistitemID = CInt(vCashListItemArray(eCashListItem.CashlistitemID))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.AllocationstatusID), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.AllocationstatusID) Is DBNull.Value Then

                    If .AllocationstatusID <> CDbl(vCashListItemArray(eCashListItem.AllocationstatusID)) Then
                        .AllocationstatusID = CInt(vCashListItemArray(eCashListItem.AllocationstatusID))
                        bDataChanged = True
                    End If
                End If
                If Not Object.Equals(vCashListItemArray(eCashListItem.MediaTypeID), Nothing) AndAlso
                    Not (vCashListItemArray(eCashListItem.MediaTypeID)) Is DBNull.Value Then

                    If .MediaTypeID <> CDbl(vCashListItemArray(eCashListItem.MediaTypeID)) Then
                        .MediaTypeID = CInt(vCashListItemArray(eCashListItem.MediaTypeID))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.CashlistID), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.CashlistID) Is DBNull.Value Then

                    If .CashlistID <> CDbl(vCashListItemArray(eCashListItem.CashlistID)) Then
                        .CashlistID = CInt(vCashListItemArray(eCashListItem.CashlistID))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.AccountID), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.AccountID) Is DBNull.Value Then

                    If .AccountID <> CDbl(vCashListItemArray(eCashListItem.AccountID)) Then
                        .AccountID = CInt(vCashListItemArray(eCashListItem.AccountID))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.MediaRef), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.MediaRef) Is DBNull.Value Then

                    If .MediaRef.Trim() <> CStr(vCashListItemArray(eCashListItem.MediaRef)).Trim() Then
                        .MediaRef = CStr(vCashListItemArray(eCashListItem.MediaRef))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.OurRef), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.OurRef) Is DBNull.Value Then

                    If .OurRef.Trim() <> CStr(vCashListItemArray(eCashListItem.OurRef)).Trim() Then
                        .OurRef = CStr(vCashListItemArray(eCashListItem.OurRef))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.TheirRef), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.TheirRef) Is DBNull.Value Then

                    If .TheirRef.Trim() <> CStr(vCashListItemArray(eCashListItem.TheirRef)).Trim() Then
                        .TheirRef = CStr(vCashListItemArray(eCashListItem.TheirRef))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.Amount), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.Amount) Is DBNull.Value Then

                    If .Amount <> vCashListItemArray(eCashListItem.Amount) Then
                        .Amount = CDec(vCashListItemArray(eCashListItem.Amount))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.TransdetailID), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.TransdetailID) Is DBNull.Value Then

                    If .TransdetailID <> CDbl(vCashListItemArray(eCashListItem.TransdetailID)) Then
                        .TransdetailID = CInt(vCashListItemArray(eCashListItem.TransdetailID))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.ContactName), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.ContactName) Is DBNull.Value Then

                    If .ContactName.Trim() <> CStr(vCashListItemArray(eCashListItem.ContactName)).Trim() Then
                        .ContactName = CStr(vCashListItemArray(eCashListItem.ContactName)).Trim()
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.Address1), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.Address1) Is DBNull.Value Then

                    If .Address1.Trim() <> CStr(vCashListItemArray(eCashListItem.Address1)).Trim() Then
                        .Address1 = CStr(vCashListItemArray(eCashListItem.Address1)).Trim()
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.Address2), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.Address2) Is DBNull.Value Then

                    If .Address2.Trim() <> CStr(vCashListItemArray(eCashListItem.Address2)).Trim() Then
                        .Address2 = CStr(vCashListItemArray(eCashListItem.Address2)).Trim()
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.Address3), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.Address3) Is DBNull.Value Then

                    If .Address3.Trim() <> CStr(vCashListItemArray(eCashListItem.Address3)).Trim() Then
                        .Address3 = CStr(vCashListItemArray(eCashListItem.Address3)).Trim()
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.Address4), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.Address4) Is DBNull.Value Then

                    If .Address4.Trim() <> CStr(vCashListItemArray(eCashListItem.Address4)).Trim() Then
                        .Address4 = CStr(vCashListItemArray(eCashListItem.Address4)).Trim()
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.PostalCode), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.PostalCode) Is DBNull.Value Then

                    If .PostalCode.Trim() <> CStr(vCashListItemArray(eCashListItem.PostalCode)).Trim() Then
                        .PostalCode = CStr(vCashListItemArray(eCashListItem.PostalCode)).Trim()
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.AddressCountry), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.AddressCountry) Is DBNull.Value Then

                    If .AddressCountry <> CDbl(vCashListItemArray(eCashListItem.AddressCountry)) Then
                        .AddressCountry = CInt(vCashListItemArray(eCashListItem.AddressCountry))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.PaymentName), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.PaymentName) Is DBNull.Value Then

                    If Convert.ToString(.PaymentName).Trim() <> CStr(vCashListItemArray(eCashListItem.PaymentName)).Trim() Then
                        .PaymentName = CStr(vCashListItemArray(eCashListItem.PaymentName)).Trim()
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.PaymentAccountCode), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.PaymentAccountCode) Is DBNull.Value Then

                    If .PaymentAccountCode.Trim() <> CStr(vCashListItemArray(eCashListItem.PaymentAccountCode)).Trim() Then
                        .PaymentAccountCode = CStr(vCashListItemArray(eCashListItem.PaymentAccountCode))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.PaymentBranchCode), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.PaymentBranchCode) Is DBNull.Value Then

                    If .PaymentBranchCode.Trim() <> CStr(vCashListItemArray(eCashListItem.PaymentBranchCode)).Trim() Then
                        .PaymentBranchCode = CStr(vCashListItemArray(eCashListItem.PaymentBranchCode)).Trim()
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.PaymentExpiryDate), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.PaymentExpiryDate) Is DBNull.Value Then

                    If .PaymentExpiryDate <> CDate(vCashListItemArray(eCashListItem.PaymentExpiryDate)) Then
                        .PaymentExpiryDate = CDate(vCashListItemArray(eCashListItem.PaymentExpiryDate))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.PaymentReference1), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.PaymentReference1) Is DBNull.Value Then

                    If .PaymentReference1.Trim() <> CStr(vCashListItemArray(eCashListItem.PaymentReference1)).Trim() Then
                        .PaymentReference1 = CStr(vCashListItemArray(eCashListItem.PaymentReference1)).Trim()
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.PaymentReference2), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.PaymentReference2) Is DBNull.Value Then

                    If .PaymentReference2.Trim() <> CStr(vCashListItemArray(eCashListItem.PaymentReference2)).Trim() Then
                        .PaymentReference2 = CStr(vCashListItemArray(eCashListItem.PaymentReference2)).Trim()
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.Letter), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.Letter) Is DBNull.Value Then

                    If CStr(.Letter).Trim() <> CStr(vCashListItemArray(eCashListItem.Letter)).Trim() Then
                        .Letter = CBool(vCashListItemArray(eCashListItem.Letter))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.Batch_id), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.Batch_id) Is String.Empty Then

                    If CStr(.Batch_id).Trim() <> CStr(vCashListItemArray(eCashListItem.Batch_id)).Trim() Then
                        .Batch_id = CInt(vCashListItemArray(eCashListItem.Batch_id))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.pmuser_id), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.pmuser_id) Is DBNull.Value Then

                    If CStr(.pmuser_id).Trim() <> CStr(vCashListItemArray(eCashListItem.pmuser_id)).Trim() Then
                        .pmuser_id = CInt(vCashListItemArray(eCashListItem.pmuser_id))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.Transaction_Date), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.Transaction_Date) Is DBNull.Value Then
                    'Developer Guide No. 40
                    If .Transaction_Date.ToString.Trim() <> CStr(vCashListItemArray(eCashListItem.Transaction_Date)).Trim() Then
                        .Transaction_Date = CDate(vCashListItemArray(eCashListItem.Transaction_Date))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.Original_Amount), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.Original_Amount) Is DBNull.Value Then

                    If CStr(.Original_Amount).Trim() <> CStr(vCashListItemArray(eCashListItem.Original_Amount)).Trim() Then
                        .Original_Amount = CDec(vCashListItemArray(eCashListItem.Original_Amount))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.Amount_Tendered), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.Amount_Tendered) Is DBNull.Value Then

                    If CStr(.Amount_Tendered).Trim() <> CStr(vCashListItemArray(eCashListItem.Amount_Tendered)).Trim() Then
                        .Amount_Tendered = CDec(vCashListItemArray(eCashListItem.Amount_Tendered))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.Change), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.Change) Is DBNull.Value Then

                    If CStr(.Change).Trim() <> CStr(vCashListItemArray(eCashListItem.Change)).Trim() Then
                        .Change = CDec(vCashListItemArray(eCashListItem.Change))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.CashListItem_receipt_type_id), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.CashListItem_receipt_type_id) Is DBNull.Value Then

                    If CStr(.CashListItem_receipt_type_id).Trim() <> CStr(vCashListItemArray(eCashListItem.CashListItem_receipt_type_id)).Trim() Then
                        .CashListItem_receipt_type_id = CInt(vCashListItemArray(eCashListItem.CashListItem_receipt_type_id))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.CashListItem_receipt_status_id), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.CashListItem_receipt_status_id) Is DBNull.Value Then

                    If CStr(.CashListItem_receipt_status_id).Trim() <> CStr(vCashListItemArray(eCashListItem.CashListItem_receipt_status_id)).Trim() Then
                        .CashListItem_receipt_status_id = CInt(vCashListItemArray(eCashListItem.CashListItem_receipt_status_id))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.CashListItem_bank_id), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.CashListItem_bank_id) Is DBNull.Value Then

                    If CStr(.CashListItem_bank_id).Trim() <> CStr(vCashListItemArray(eCashListItem.CashListItem_bank_id)).Trim() Then
                        .CashListItem_bank_id = CInt(vCashListItemArray(eCashListItem.CashListItem_bank_id))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.Cheque_Date), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.Cheque_Date) Is DBNull.Value Then
                    'Developer Guide No. 40
                    If .Cheque_Date.ToString.Trim() <> CStr(vCashListItemArray(eCashListItem.Cheque_Date)).Trim() Then
                        .Cheque_Date = CDate(vCashListItemArray(eCashListItem.Cheque_Date))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.CC_Number), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.CC_Number) Is DBNull.Value Then

                    If .CC_Number.Trim() <> CStr(vCashListItemArray(eCashListItem.CC_Number)).Trim() Then
                        .CC_Number = CStr(vCashListItemArray(eCashListItem.CC_Number))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.CC_Expiry_Date), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.CC_Expiry_Date) Is DBNull.Value Then

                    If Convert.ToString(.CC_Expiry_Date).Trim() <> CStr(vCashListItemArray(eCashListItem.CC_Expiry_Date)).Trim() Then
                        .CC_Expiry_Date = CStr(vCashListItemArray(eCashListItem.CC_Expiry_Date))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.CC_Start_Date), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.CC_Start_Date) Is DBNull.Value Then

                    If Convert.ToString(.CC_Start_Date).Trim() <> CStr(vCashListItemArray(eCashListItem.CC_Start_Date)).Trim() Then

                        .CC_Start_Date = CStr(vCashListItemArray(eCashListItem.CC_Start_Date))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.CC_Issue), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.CC_Issue) Is DBNull.Value Then

                    If .CC_Issue.Trim() <> CStr(vCashListItemArray(eCashListItem.CC_Issue)).Trim() Then

                        .CC_Issue = CStr(vCashListItemArray(eCashListItem.CC_Issue))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.CC_Pin), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.CC_Pin) Is DBNull.Value Then

                    If .CC_Pin.Trim() <> CStr(vCashListItemArray(eCashListItem.CC_Pin)).Trim() Then

                        .CC_Pin = CStr(vCashListItemArray(eCashListItem.CC_Pin))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.CC_Auth_Code), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.CC_Auth_Code) Is DBNull.Value Then

                    If .CC_Auth_Code.Trim() <> CStr(vCashListItemArray(eCashListItem.CC_Auth_Code)).Trim() Then

                        .CC_Auth_Code = CStr(vCashListItemArray(eCashListItem.CC_Auth_Code))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.Receipt_Details), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.Receipt_Details) Is DBNull.Value Then

                    If .Receipt_Details.Trim() <> CStr(vCashListItemArray(eCashListItem.Receipt_Details)).Trim() Then

                        .Receipt_Details = CStr(vCashListItemArray(eCashListItem.Receipt_Details))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.CashListItem_Reverse_PMUser_id), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.CashListItem_Reverse_PMUser_id) Is DBNull.Value Then

                    If CStr(.CashListItem_Reverse_PMUser_id).Trim() <> CStr(vCashListItemArray(eCashListItem.CashListItem_Reverse_PMUser_id)).Trim() Then

                        .CashListItem_Reverse_PMUser_id = CInt(vCashListItemArray(eCashListItem.CashListItem_Reverse_PMUser_id))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.CashListItem_Reverse_Reason_id), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.CashListItem_Reverse_Reason_id) Is DBNull.Value Then

                    If CStr(.CashListItem_Reverse_Reason_id).Trim() <> CStr(vCashListItemArray(eCashListItem.CashListItem_Reverse_Reason_id)).Trim() Then

                        .CashListItem_Reverse_Reason_id = CInt(vCashListItemArray(eCashListItem.CashListItem_Reverse_Reason_id))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.CashListItem_Payment_Type_id), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.CashListItem_Payment_Type_id) Is DBNull.Value Then

                    If CStr(.CashListItem_Payment_Type_id).Trim() <> CStr(vCashListItemArray(eCashListItem.CashListItem_Payment_Type_id)).Trim() Then

                        .CashListItem_Payment_Type_id = CInt(vCashListItemArray(eCashListItem.CashListItem_Payment_Type_id))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.CashListItem_Payment_Status_id), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.CashListItem_Payment_Status_id) Is DBNull.Value Then

                    If CStr(.CashListItem_Payment_Status_id).Trim() <> CStr(vCashListItemArray(eCashListItem.CashListItem_Payment_Status_id)).Trim() Then

                        .CashListItem_Payment_Status_id = CInt(vCashListItemArray(eCashListItem.CashListItem_Payment_Status_id))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.Date_Presented), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.Date_Presented) Is DBNull.Value Then

                    If .Date_Presented.ToString.Trim() <> CStr(vCashListItemArray(eCashListItem.Date_Presented)).Trim() Then

                        If Informations.IsDate(Convert.ToString(vCashListItemArray(eCashListItem.Date_Presented))) Then
                            .Date_Presented = CDate(vCashListItemArray(eCashListItem.Date_Presented))
                        End If
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.Cheque_in_Possession), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.Cheque_in_Possession) Is DBNull.Value Then

                    If CStr(.Cheque_in_Possession).Trim() <> CStr(vCashListItemArray(eCashListItem.Cheque_in_Possession)).Trim() Then
                        .Cheque_in_Possession = CInt(vCashListItemArray(eCashListItem.Cheque_in_Possession))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.Stop_Requested_Date), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.Stop_Requested_Date) Is DBNull.Value Then

                    If .Stop_Requested_Date.ToString.Trim() <> CStr(vCashListItemArray(eCashListItem.Stop_Requested_Date)).Trim() Then

                        If Informations.IsDate(Convert.ToString(vCashListItemArray(eCashListItem.Stop_Requested_Date))) Then
                            .Stop_Requested_Date = CDate(vCashListItemArray(eCashListItem.Stop_Requested_Date))
                        End If
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.Stop_Printed_Date), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.Stop_Printed_Date) Is DBNull.Value Then

                    If .Stop_Printed_Date.ToString.Trim() <> CStr(vCashListItemArray(eCashListItem.Stop_Printed_Date)).Trim() Then
                        .Stop_Printed_Date = CDate(vCashListItemArray(eCashListItem.Stop_Printed_Date))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.Stop_Confirmation_Date), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.Stop_Confirmation_Date) Is DBNull.Value Then

                    If .Stop_Confirmation_Date.ToString.Trim() <> CStr(vCashListItemArray(eCashListItem.Stop_Confirmation_Date)).Trim() Then

                        If Informations.IsDate(Convert.ToString(vCashListItemArray(eCashListItem.Stop_Confirmation_Date))) Then
                            .Stop_Confirmation_Date = CDate(vCashListItemArray(eCashListItem.Stop_Confirmation_Date))
                            bDataChanged = True
                        End If
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.Reason), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.Reason) Is DBNull.Value Then

                    If .Reason.Trim() <> CStr(vCashListItemArray(eCashListItem.Reason)).Trim() Then
                        .Reason = CStr(vCashListItemArray(eCashListItem.Reason))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.Replaces_CashListItem_id), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.Replaces_CashListItem_id) Is DBNull.Value Then

                    If CStr(.Replaces_CashListItem_id).Trim() <> CStr(vCashListItemArray(eCashListItem.Replaces_CashListItem_id)).Trim() Then
                        .Replaces_CashListItem_id = CInt(vCashListItemArray(eCashListItem.Replaces_CashListItem_id))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.XML_Object), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.XML_Object) Is DBNull.Value Then

                    If .XML_Object.Trim() <> CStr(vCashListItemArray(eCashListItem.XML_Object)).Trim() Then
                        .XML_Object = CStr(vCashListItemArray(eCashListItem.XML_Object))
                        bDataChanged = True
                    End If
                End If

                'check to see if the contents of the instalment array have changed
                If (Not Informations.IsArray(.Instalment_Array) AndAlso
                    Informations.IsArray(vCashListItemArray(eCashListItem.InstalmentArray))) OrElse
                (Informations.IsArray(.Instalment_Array) AndAlso
                 Informations.IsArray(vCashListItemArray(eCashListItem.InstalmentArray))) Then
                    'we are either adding or deleting instalment details

                    .Instalment_Array = vCashListItemArray(eCashListItem.InstalmentArray)
                    bDataChanged = True
                Else
                    If Informations.IsArray(.Instalment_Array) AndAlso
                        Informations.IsArray(vCashListItemArray(eCashListItem.InstalmentArray)) Then

                        If AreArraysDifferent(.Instalment_Array, vCashListItemArray(eCashListItem.InstalmentArray)) = gPMConstants.PMEReturnCode.PMTrue Then
                            .Instalment_Array = vCashListItemArray(eCashListItem.InstalmentArray)
                            bDataChanged = True
                        End If
                    End If
                End If

                'check to see if the salvage array has changed
                If (Not Informations.IsArray(.Salvage_Array) AndAlso
                    Informations.IsArray(vCashListItemArray(eCashListItem.SalvageArray))) OrElse
                (Informations.IsArray(.Salvage_Array) AndAlso
                 Not Informations.IsArray(vCashListItemArray(eCashListItem.SalvageArray))) Then

                    .Salvage_Array = vCashListItemArray(eCashListItem.SalvageArray)
                    bDataChanged = True
                Else
                    If Informations.IsArray(.Salvage_Array) AndAlso
                        Informations.IsArray(vCashListItemArray(eCashListItem.SalvageArray)) Then

                        If AreArraysDifferent(.Salvage_Array, vCashListItemArray(eCashListItem.SalvageArray)) = gPMConstants.PMEReturnCode.PMTrue Then
                            .Salvage_Array = vCashListItemArray(eCashListItem.SalvageArray)
                            bDataChanged = True
                        End If
                    End If
                End If

                If (Not Informations.IsArray(.CLMUSRecovery_Array) AndAlso
                    Informations.IsArray(vCashListItemArray(eCashListItem.CLMUSRecoveryArray))) OrElse
                (Informations.IsArray(.CLMUSRecovery_Array) AndAlso
                 Not Informations.IsArray(vCashListItemArray(eCashListItem.CLMUSRecoveryArray))) Then

                    .CLMUSRecovery_Array = vCashListItemArray(eCashListItem.CLMUSRecoveryArray)
                    bDataChanged = True
                Else
                    If Informations.IsArray(.CLMUSRecovery_Array) AndAlso
                        Informations.IsArray(vCashListItemArray(eCashListItem.CLMUSRecoveryArray)) Then

                        If AreArraysDifferent(.CLMUSRecovery_Array, vCashListItemArray(eCashListItem.CLMUSRecoveryArray)) = gPMConstants.PMEReturnCode.PMTrue Then
                            .CLMUSRecovery_Array = vCashListItemArray(eCashListItem.CLMUSRecoveryArray)
                            bDataChanged = True
                        End If
                    End If
                End If

                If (Not Informations.IsArray(.CLMRVRecovery_Array) AndAlso
                    Informations.IsArray(vCashListItemArray(eCashListItem.CLMRVRecoveryArray))) OrElse
                (Informations.IsArray(.CLMRVRecovery_Array) AndAlso
                 Not Informations.IsArray(vCashListItemArray(eCashListItem.CLMRVRecoveryArray))) Then

                    .CLMRVRecovery_Array = vCashListItemArray(eCashListItem.CLMRVRecoveryArray)
                    bDataChanged = True
                Else
                    If Informations.IsArray(.CLMRVRecovery_Array) AndAlso
                        Informations.IsArray(vCashListItemArray(eCashListItem.CLMRVRecoveryArray)) Then
                        If AreArraysDifferent(.CLMRVRecovery_Array, vCashListItemArray(eCashListItem.CLMRVRecoveryArray)) = gPMConstants.PMEReturnCode.PMTrue Then

                            .CLMRVRecovery_Array = vCashListItemArray(eCashListItem.CLMRVRecoveryArray)
                            bDataChanged = True
                        End If
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.UnderwritingYearID), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.UnderwritingYearID) Is DBNull.Value Then

                    If Convert.ToInt32(.UnderwritingYearID) <> (vCashListItemArray(eCashListItem.UnderwritingYearID)) Then
                        .UnderwritingYearID = vCashListItemArray(eCashListItem.UnderwritingYearID)
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.CurrencyBaseDate), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.CurrencyBaseDate) Is DBNull.Value Then

                    If Convert.ToDateTime(.CurrencyBaseDate) <> vCashListItemArray(eCashListItem.CurrencyBaseDate) Then
                        .CurrencyBaseDate = vCashListItemArray(eCashListItem.CurrencyBaseDate)
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.CurrencyBaseXrate), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.CurrencyBaseXrate) Is DBNull.Value Then

                    If .CurrencyBaseXrate Is Nothing Or .CurrencyBaseXrate <> vCashListItemArray(eCashListItem.CurrencyBaseXrate) Then
                        .CurrencyBaseXrate = vCashListItemArray(eCashListItem.CurrencyBaseXrate)
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.AccountBaseDate), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.AccountBaseDate) Is DBNull.Value Then

                    If Convert.ToDateTime(.AccountBaseDate) <> (vCashListItemArray(eCashListItem.AccountBaseDate)) Then
                        .AccountBaseDate = vCashListItemArray(eCashListItem.AccountBaseDate)
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.AccountBaseXrate), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.AccountBaseXrate) Is DBNull.Value Then

                    If .AccountBaseXrate Is Nothing Or .AccountBaseXrate <> (vCashListItemArray(eCashListItem.AccountBaseXrate)) Then
                        .AccountBaseXrate = vCashListItemArray(eCashListItem.AccountBaseXrate)
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.SystemBaseDate), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.SystemBaseDate) Is DBNull.Value Then

                    If Convert.ToDateTime(.SystemBaseDate) <> (vCashListItemArray(eCashListItem.SystemBaseDate)) Then
                        .SystemBaseDate = vCashListItemArray(eCashListItem.SystemBaseDate)
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.SystemBaseXrate), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.SystemBaseXrate) Is DBNull.Value Then

                    If .SystemBaseXrate Is Nothing Or .SystemBaseXrate <> (vCashListItemArray(eCashListItem.SystemBaseXrate)) Then
                        .SystemBaseXrate = vCashListItemArray(eCashListItem.SystemBaseXrate)
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.OverrideReason), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.OverrideReason) Is DBNull.Value Then

                    If Convert.ToString(.OverrideReason) <> Convert.ToString(vCashListItemArray(eCashListItem.OverrideReason)) Then
                        .OverrideReason = vCashListItemArray(eCashListItem.OverrideReason)
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.CC_Name), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.CC_Name) Is DBNull.Value Then

                    If .CC_Name <> CStr(vCashListItemArray(eCashListItem.CC_Name)) Then
                        .CC_Name = CStr(vCashListItemArray(eCashListItem.CC_Name))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.CC_Customer), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.CC_Customer) Is DBNull.Value Then

                    If .CC_Customer <> CStr(vCashListItemArray(eCashListItem.CC_Customer)) Then
                        .CC_Customer = CStr(vCashListItemArray(eCashListItem.CC_Customer))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.CC_Manual_Auth_Code), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.CC_Manual_Auth_Code) Is DBNull.Value Then

                    If .CC_Manual_Auth_Code <> CStr(vCashListItemArray(eCashListItem.CC_Manual_Auth_Code)) Then
                        .CC_Manual_Auth_Code = CStr(vCashListItemArray(eCashListItem.CC_Manual_Auth_Code))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.CC_Transaction_Code), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.CC_Transaction_Code) Is DBNull.Value Then

                    If .CC_Transaction_Code <> CStr(vCashListItemArray(eCashListItem.CC_Transaction_Code)) Then
                        .CC_Transaction_Code = CStr(vCashListItemArray(eCashListItem.CC_Transaction_Code))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.MediaTypeIssuerID), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.MediaTypeIssuerID) Is DBNull.Value Then

                    If .MediaTypeIssuerID <> CDbl(vCashListItemArray(eCashListItem.MediaTypeIssuerID)) Then
                        .MediaTypeIssuerID = CInt(vCashListItemArray(eCashListItem.MediaTypeIssuerID))
                        bDataChanged = True
                    End If
                End If

                'Party Bank Details
                If Not Object.Equals(vCashListItemArray(eCashListItem.CollectionDate), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.CollectionDate) Is DBNull.Value Then

                    If .CollectionDate <> CDate(vCashListItemArray(eCashListItem.CollectionDate)) Then
                        .CollectionDate = CDate(vCashListItemArray(eCashListItem.CollectionDate))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(gACTLibrary.eCashListItem.PartyBankId), Nothing) And Not vCashListItemArray(gACTLibrary.eCashListItem.PartyBankId) Is DBNull.Value Then

                    If (.PartyBankId Is DBNull.Value) OrElse (Convert.ToInt32(.PartyBankId) <> Convert.ToInt32(vCashListItemArray(gACTLibrary.eCashListItem.PartyBankId))) Then
                        'developer guide no.24
                        .PartyBankId = vCashListItemArray(gACTLibrary.eCashListItem.PartyBankId)
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.Comments), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.Comments) Is DBNull.Value Then

                    If .Comments <> CStr(vCashListItemArray(eCashListItem.Comments)) Then
                        .Comments = CStr(vCashListItemArray(eCashListItem.Comments))
                        bDataChanged = True
                    End If
                End If

                If (Not Informations.IsArray(.BGPolicies) AndAlso
                    Informations.IsArray(vCashListItemArray(eCashListItem.BGPolicies))) OrElse
                (Informations.IsArray(.BGPolicies) AndAlso
                 Not Informations.IsArray(vCashListItemArray(eCashListItem.BGPolicies))) Then

                    .BGPolicies = vCashListItemArray(eCashListItem.BGPolicies)
                    bDataChanged = True
                Else
                    If Informations.IsArray(.BGPolicies) AndAlso
                        Informations.IsArray(vCashListItemArray(eCashListItem.BGPolicies)) Then

                        If AreArraysDifferent(.BGPolicies, vCashListItemArray(eCashListItem.BGPolicies)) = gPMConstants.PMEReturnCode.PMTrue Then
                            .BGPolicies = vCashListItemArray(eCashListItem.BGPolicies)
                            bDataChanged = True
                        End If
                    End If
                End If

                'WPR12- Enhancement Quote Collection Process
                If Not Object.Equals(vCashListItemArray(eCashListItem.BankLocation), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.BankLocation) Is DBNull.Value Then

                    If .BankLocation <> CStr(vCashListItemArray(eCashListItem.BankLocation)) Then
                        .BankLocation = CStr(vCashListItemArray(eCashListItem.BankLocation))
                        bDataChanged = True
                    End If
                End If


                If Not Object.Equals(vCashListItemArray(eCashListItem.BankBranch), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.BankBranch) Is DBNull.Value Then

                    If .BankBranch <> CStr(vCashListItemArray(eCashListItem.BankBranch)) Then
                        .BankBranch = CStr(vCashListItemArray(eCashListItem.BankBranch))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.ChequeTypeId), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.ChequeTypeId) Is DBNull.Value Then

                    If .ChequeTypeId <> CDbl(vCashListItemArray(eCashListItem.ChequeTypeId)) Then
                        .ChequeTypeId = CInt(vCashListItemArray(eCashListItem.ChequeTypeId))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.CCBankId), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.CCBankId) Is DBNull.Value Then

                    If .CCBankId <> CDbl(vCashListItemArray(eCashListItem.CCBankId)) Then
                        .CCBankId = CInt(vCashListItemArray(eCashListItem.CCBankId))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.CardTypeId), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.CardTypeId) Is DBNull.Value Then

                    If .CardTypeId <> CDbl(vCashListItemArray(eCashListItem.CardTypeId)) Then
                        .CardTypeId = CInt(vCashListItemArray(eCashListItem.CardTypeId))
                        bDataChanged = True
                    End If
                End If


                If Not Object.Equals(vCashListItemArray(eCashListItem.CardTransSlipNo), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.CardTransSlipNo) Is DBNull.Value Then

                    If .CardTransSlipNo <> CStr(vCashListItemArray(eCashListItem.CardTransSlipNo)) Then
                        .CardTransSlipNo = CStr(vCashListItemArray(eCashListItem.CardTransSlipNo))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.ChequeClearingTypeId), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.ChequeClearingTypeId) Is DBNull.Value Then

                    If .ChequeClearingTypeId <> CDbl(vCashListItemArray(eCashListItem.ChequeClearingTypeId)) Then
                        .ChequeClearingTypeId = CInt(vCashListItemArray(eCashListItem.ChequeClearingTypeId))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.SplitTotal), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.SplitTotal) Is DBNull.Value Then

                    If .SplitTotal <> CDbl(vCashListItemArray(eCashListItem.SplitTotal)) Then
                        .SplitTotal = CDec(vCashListItemArray(eCashListItem.SplitTotal))
                        bDataChanged = True
                    End If

                End If

                If Not Object.Equals(vCashListItemArray(eCashListItem.IsLeadAccount), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.IsLeadAccount) Is DBNull.Value Then

                    .IsLeadAccount = vCashListItemArray(eCashListItem.IsLeadAccount)
                    bDataChanged = True
                End If

                If vCashListItemArray.GetUpperBound(0) > 83 AndAlso (Not Object.Equals(vCashListItemArray(eCashListItem.TaxBandId), Nothing)) Then
                    If .TaxBandID <> vCashListItemArray(eCashListItem.TaxBandId) Then
                        .TaxBandID = ToSafeInteger(vCashListItemArray(eCashListItem.TaxBandId))
                        bDataChanged = True
                    End If
                End If

                If vCashListItemArray.GetUpperBound(0) > 84 AndAlso Not Object.Equals(vCashListItemArray(eCashListItem.TaxAmount), Nothing) Then
                    If .TaxAmount <> vCashListItemArray(eCashListItem.TaxAmount) Then
                        .TaxAmount = ToSafeDouble(vCashListItemArray(eCashListItem.TaxAmount))
                        bDataChanged = True
                    End If
                End If

                If vCashListItemArray.GetUpperBound(0) > 85 AndAlso Not Object.Equals(vCashListItemArray(eCashListItem.BIC), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.BIC) Is DBNull.Value Then

                    If NullToString(.BIC).Trim() <> CStr(vCashListItemArray(eCashListItem.BIC)).Trim() Then
                        .BIC = CStr(vCashListItemArray(eCashListItem.BIC))
                        bDataChanged = True
                    End If
                End If

                If vCashListItemArray.GetUpperBound(0) > 86 AndAlso Not Object.Equals(vCashListItemArray(eCashListItem.IBAN), Nothing) AndAlso
                    Not vCashListItemArray(eCashListItem.IBAN) Is DBNull.Value Then

                    If NullToString(.IBAN).Trim() <> CStr(vCashListItemArray(eCashListItem.IBAN)).Trim() Then
                        .IBAN = CStr(vCashListItemArray(eCashListItem.IBAN))
                        bDataChanged = True
                    End If
                End If

                If Not Object.Equals(vCashListItemArray(gACTLibrary.eCashListItem.InsuranceRef), Nothing) And Not vCashListItemArray(gACTLibrary.eCashListItem.InsuranceRef) Is DBNull.Value Then

                    If Convert.ToString(.InsuranceRef).Trim() <> CStr(vCashListItemArray(gACTLibrary.eCashListItem.InsuranceRef)).Trim() Then

                        .InsuranceRef = CStr(vCashListItemArray(gACTLibrary.eCashListItem.InsuranceRef))
                        bDataChanged = True
                    End If
                End If

                ' If we have changed one of the properties, update the status
                If bDataChanged Then
                    .DatabaseStatus = iStatus
                End If

            End With

            Return nResult

        Catch excep As System.Exception
            If Not bSetProperties Then
                Throw excep
            End If
            If bSetProperties Then
                nResult = gPMConstants.PMEReturnCode.PMError
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProperties", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
                Return nResult
            End If
        End Try
        Return nResult
    End Function

    ' ***************************************************************** '
    ' Name: ArrayChanged (Private)
    ' Date: 02-11-2002
    ' Author: Steve Watton
    ' Description: Compares two, 2 dimensional arrays, option base = 0
    '               and returns PMTrue if they are different
    '
    ' ***************************************************************** '
    Private Function AreArraysDifferent(ByVal vArrayOne(,) As Object, ByVal vArrayTwo(,) As Object) As Integer
        Dim result As Integer = 0
        Dim lColOne, lColTwo, lRowOne, lRowTwo As Integer



        result = gPMConstants.PMEReturnCode.PMFalse

        'first check to see if one or both of the arrays are empty

        If Object.Equals(vArrayOne, Nothing) And Object.Equals(vArrayTwo, Nothing) Then Return result


        If Object.Equals(vArrayOne, Nothing) <> Object.Equals(vArrayTwo, Nothing) Then
            Return gPMConstants.PMEReturnCode.PMTrue
        End If

        'we now know that we are dealing with 2 arrays


        lColOne = vArrayOne.GetUpperBound(0)

        lColTwo = vArrayTwo.GetUpperBound(0)

        If lColOne <> lColTwo Then
            Return gPMConstants.PMEReturnCode.PMTrue
        End If


        lRowOne = vArrayOne.GetUpperBound(1)

        lRowTwo = vArrayTwo.GetUpperBound(1)

        If lRowOne <> lRowTwo Then
            Return gPMConstants.PMEReturnCode.PMTrue
        End If

        For lColCount As Integer = 0 To lColOne
            For lRowCount As Integer = 0 To lRowOne


                If Not vArrayOne(lColCount, lRowCount).Equals(vArrayTwo(lColCount, lRowCount)) Then
                    Return gPMConstants.PMEReturnCode.PMTrue
                End If
            Next
        Next

        Return result

    End Function

    ''' <summary>
    ''' Returns the supplied Cashlistitem property values.
    ''' </summary>
    ''' <param name="oCashListItem"></param>
    ''' <param name="iStatus"></param>
    ''' <param name="r_vCashListItem"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetProperties(ByRef oCashListItem As bACTCashlistitem.Cashlistitem,
                                   ByRef iStatus As Integer,
                                   ByRef r_vCashListItem() As Object) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue


        With oCashListItem
            ReDim r_vCashListItem(eCashListItem.LastItem)
            r_vCashListItem(eCashListItem.CashlistitemID) = .CashlistitemID
            r_vCashListItem(eCashListItem.AllocationstatusID) = .AllocationstatusID
            r_vCashListItem(eCashListItem.MediaTypeID) = .MediaTypeID
            r_vCashListItem(eCashListItem.CashlistID) = .CashlistID
            r_vCashListItem(eCashListItem.AccountID) = .AccountID
            r_vCashListItem(eCashListItem.MediaTypeIssuerID) = ToSafeLong(.MediaTypeIssuerID, 0)
            r_vCashListItem(eCashListItem.MediaRef) = .MediaRef
            r_vCashListItem(eCashListItem.OurRef) = .OurRef
            r_vCashListItem(eCashListItem.TheirRef) = .TheirRef
            r_vCashListItem(eCashListItem.Amount) = .Amount
            r_vCashListItem(eCashListItem.TransdetailID) = .TransdetailID
            r_vCashListItem(eCashListItem.ContactName) = .ContactName
            r_vCashListItem(eCashListItem.Address1) = .Address1
            r_vCashListItem(eCashListItem.Address2) = .Address2
            r_vCashListItem(eCashListItem.Address3) = .Address3
            r_vCashListItem(eCashListItem.Address4) = .Address4
            r_vCashListItem(eCashListItem.PostalCode) = .PostalCode
            r_vCashListItem(eCashListItem.AddressCountry) = .AddressCountry
            r_vCashListItem(eCashListItem.PaymentName) = .PaymentName
            r_vCashListItem(eCashListItem.PaymentAccountCode) = .PaymentAccountCode
            r_vCashListItem(eCashListItem.PaymentBranchCode) = .PaymentBranchCode
            r_vCashListItem(eCashListItem.PaymentExpiryDate) = .PaymentExpiryDate
            r_vCashListItem(eCashListItem.PaymentReference1) = .PaymentReference1
            r_vCashListItem(eCashListItem.PaymentReference2) = .PaymentReference2
            r_vCashListItem(eCashListItem.Letter) = .Letter
            r_vCashListItem(eCashListItem.Batch_id) = .Batch_id
            r_vCashListItem(eCashListItem.pmuser_id) = .pmuser_id
            r_vCashListItem(eCashListItem.Transaction_Date) = .Transaction_Date
            r_vCashListItem(eCashListItem.Original_Amount) = .Original_Amount
            r_vCashListItem(eCashListItem.Amount_Tendered) = .Amount_Tendered
            r_vCashListItem(eCashListItem.Change) = .Change
            r_vCashListItem(eCashListItem.CashListItem_receipt_type_id) = .CashListItem_receipt_type_id
            r_vCashListItem(eCashListItem.CashListItem_receipt_status_id) = .CashListItem_receipt_status_id
            r_vCashListItem(eCashListItem.CashListItem_bank_id) = .CashListItem_bank_id
            r_vCashListItem(eCashListItem.Cheque_Date) = .Cheque_Date
            r_vCashListItem(eCashListItem.CC_Number) = .CC_Number
            r_vCashListItem(eCashListItem.CC_Expiry_Date) = .CC_Expiry_Date
            r_vCashListItem(eCashListItem.CC_Start_Date) = .CC_Start_Date
            r_vCashListItem(eCashListItem.CC_Issue) = .CC_Issue
            r_vCashListItem(eCashListItem.CC_Pin) = .CC_Pin
            r_vCashListItem(eCashListItem.CC_Auth_Code) = .CC_Auth_Code
            r_vCashListItem(eCashListItem.CC_Name) = .CC_Name
            r_vCashListItem(eCashListItem.CC_Customer) = .CC_Customer
            r_vCashListItem(eCashListItem.CC_Manual_Auth_Code) = .CC_Manual_Auth_Code
            r_vCashListItem(eCashListItem.CC_Transaction_Code) = .CC_Transaction_Code
            r_vCashListItem(eCashListItem.Receipt_Details) = .Receipt_Details
            r_vCashListItem(eCashListItem.CashListItem_Reverse_PMUser_id) = .CashListItem_Reverse_PMUser_id
            r_vCashListItem(eCashListItem.CashListItem_Reverse_Reason_id) = .CashListItem_Reverse_Reason_id
            r_vCashListItem(eCashListItem.CashListItem_Payment_Type_id) = .CashListItem_Payment_Type_id
            r_vCashListItem(eCashListItem.CashListItem_Payment_Status_id) = .CashListItem_Payment_Status_id
            r_vCashListItem(eCashListItem.Date_Presented) = .Date_Presented
            r_vCashListItem(eCashListItem.Cheque_in_Possession) = .Cheque_in_Possession
            r_vCashListItem(eCashListItem.Stop_Requested_Date) = .Stop_Requested_Date
            r_vCashListItem(eCashListItem.Stop_Printed_Date) = .Stop_Printed_Date
            r_vCashListItem(eCashListItem.Stop_Confirmation_Date) = .Stop_Confirmation_Date
            r_vCashListItem(eCashListItem.Reason) = .Reason
            r_vCashListItem(eCashListItem.Replaces_CashListItem_id) = .Replaces_CashListItem_id
            r_vCashListItem(eCashListItem.XML_Object) = .XML_Object
            r_vCashListItem(eCashListItem.CurrencyBaseDate) = .CurrencyBaseDate
            r_vCashListItem(eCashListItem.CurrencyBaseXrate) = .CurrencyBaseXrate
            r_vCashListItem(eCashListItem.AccountBaseDate) = .AccountBaseDate
            r_vCashListItem(eCashListItem.AccountBaseXrate) = .AccountBaseXrate
            r_vCashListItem(eCashListItem.SystemBaseDate) = .SystemBaseDate
            r_vCashListItem(eCashListItem.SystemBaseXrate) = .SystemBaseXrate
            r_vCashListItem(eCashListItem.OverrideReason) = .OverrideReason
            r_vCashListItem(eCashListItem.PartyBankId) = .PartyBankId
            r_vCashListItem(eCashListItem.CollectionDate) = .CollectionDate
            r_vCashListItem(eCashListItem.Comments) = .Comments
            'WPR12- Enhancement Quote Collection Process
            r_vCashListItem(eCashListItem.BankLocation) = .BankLocation
            r_vCashListItem(eCashListItem.BankBranch) = .BankBranch
            r_vCashListItem(eCashListItem.ChequeTypeId) = .ChequeTypeId
            r_vCashListItem(eCashListItem.CCBankId) = .CCBankId
            r_vCashListItem(eCashListItem.CardTypeId) = .CardTypeId
            r_vCashListItem(eCashListItem.CardTransSlipNo) = .CardTransSlipNo
            r_vCashListItem(eCashListItem.ChequeClearingTypeId) = .ChequeClearingTypeId
            r_vCashListItem(eCashListItem.SplitTotal) = .SplitTotal
            r_vCashListItem(eCashListItem.IsLeadAccount) = .IsLeadAccount
            r_vCashListItem(eCashListItem.TaxAmount) = .TaxAmount
            r_vCashListItem(eCashListItem.TaxBandId) = .TaxBandID
            r_vCashListItem(eCashListItem.BIC) = .BIC
            r_vCashListItem(eCashListItem.IBAN) = .IBAN
            r_vCashListItem(gACTLibrary.eCashListItem.InsuranceRef) = .InsuranceRef
            iStatus = .DatabaseStatus

        End With

        Return nResult

    End Function

    ''' <summary>
    ''' Adds all of the INPUT parameters required for an Insert or Update.
    ''' </summary>
    ''' <param name="oCashListItem"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function AddInputParam(ByRef oCashListItem As bACTCashlistitem.Cashlistitem) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue

        With m_oDatabase

            If oCashListItem.AllocationstatusID < 1 Then

                m_lReturn = .Parameters.Add(sName:="allocationstatus_id",
                                            vValue:=DBNull.Value,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="allocationstatus_id",
                                            vValue:=CStr(oCashListItem.AllocationstatusID),
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMLong)
            End If

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            If oCashListItem.MediaTypeID < 1 Then

                m_lReturn = .Parameters.Add(sName:="mediatype_id",
                                            vValue:=DBNull.Value,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="mediatype_id",
                                            vValue:=CStr(oCashListItem.MediaTypeID),
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMLong)
            End If

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            If oCashListItem.CashlistID < 1 Then

                m_lReturn = .Parameters.Add(sName:="cashlist_id",
                                            vValue:=DBNull.Value,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="cashlist_id",
                                            vValue:=CStr(oCashListItem.CashlistID),
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMLong)
            End If

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            If oCashListItem.AccountID < 1 Then

                m_lReturn = .Parameters.Add(sName:="account_id",
                                            vValue:=DBNull.Value,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="account_id",
                                            vValue:=CStr(oCashListItem.AccountID),
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMLong)
            End If

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="media_ref",
                                        vValue:=Trim(oCashListItem.MediaRef),
                                        iDirection:=PMEParameterDirection.PMParamInput,
                                        iDataType:=PMEDataType.PMString)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="our_ref",
                                        vValue:=Trim(oCashListItem.OurRef),
                                        iDirection:=PMEParameterDirection.PMParamInput,
                                        iDataType:=PMEDataType.PMString)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="their_ref",
                                        vValue:=Trim(oCashListItem.TheirRef),
                                        iDirection:=PMEParameterDirection.PMParamInput,
                                        iDataType:=PMEDataType.PMString)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="amount",
                                        vValue:=CStr(oCashListItem.Amount),
                                        iDirection:=PMEParameterDirection.PMParamInput,
                                        iDataType:=PMEDataType.PMCurrency)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            If oCashListItem.TransdetailID < 1 Then

                m_lReturn = .Parameters.Add(sName:="transdetail_id",
                                            vValue:=DBNull.Value,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="transdetail_id",
                                            vValue:=CStr(oCashListItem.TransdetailID),
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMLong)
            End If

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="contact_name",
                                        vValue:=Trim(oCashListItem.ContactName),
                                        iDirection:=PMEParameterDirection.PMParamInput,
                                        iDataType:=PMEDataType.PMString)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="address1",
                                        vValue:=Trim(oCashListItem.Address1),
                                        iDirection:=PMEParameterDirection.PMParamInput,
                                        iDataType:=PMEDataType.PMString)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="address2",
                                        vValue:=Trim(oCashListItem.Address2),
                                        iDirection:=PMEParameterDirection.PMParamInput,
                                        iDataType:=PMEDataType.PMString)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="address3",
                                        vValue:=Trim(oCashListItem.Address3),
                                        iDirection:=PMEParameterDirection.PMParamInput,
                                        iDataType:=PMEDataType.PMString)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="address4",
                                        vValue:=Trim(oCashListItem.Address4),
                                        iDirection:=PMEParameterDirection.PMParamInput,
                                        iDataType:=PMEDataType.PMString)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="postal_code",
                                        vValue:=Trim(oCashListItem.PostalCode),
                                        iDirection:=PMEParameterDirection.PMParamInput,
                                        iDataType:=PMEDataType.PMString)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="address_country",
                                        vValue:=CStr(oCashListItem.AddressCountry),
                                        iDirection:=PMEParameterDirection.PMParamInput,
                                        iDataType:=PMEDataType.PMInteger)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="payment_name",
                                        vValue:=Trim(oCashListItem.PaymentName),
                                        iDirection:=PMEParameterDirection.PMParamInput,
                                        iDataType:=PMEDataType.PMString)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="payment_account_code",
                                        vValue:=Trim(oCashListItem.PaymentAccountCode),
                                        iDirection:=PMEParameterDirection.PMParamInput,
                                        iDataType:=PMEDataType.PMString)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="payment_branch_code",
                                        vValue:=Trim(oCashListItem.PaymentBranchCode),
                                        iDirection:=PMEParameterDirection.PMParamInput,
                                        iDataType:=PMEDataType.PMString)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If
            'developer guide no. 131
            If (DateTime.MinValue.Equals(oCashListItem.PaymentExpiryDate)) Then
                m_lReturn = .Parameters.Add(sName:="payment_expiry_date",
                                            vValue:=DBNull.Value,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMDate)
            Else
                'developer guide no. 40
                m_lReturn = .Parameters.Add(sName:="payment_expiry_date",
                                            vValue:=oCashListItem.PaymentExpiryDate,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMDate)
            End If


            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="payment_reference1",
                                        vValue:=Trim(oCashListItem.PaymentReference1),
                                        iDirection:=PMEParameterDirection.PMParamInput,
                                        iDataType:=PMEDataType.PMString)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="payment_reference2",
                                        vValue:=Trim(oCashListItem.PaymentReference2),
                                        iDirection:=PMEParameterDirection.PMParamInput,
                                        iDataType:=PMEDataType.PMString)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            If oCashListItem.Letter Then

                m_lReturn = .Parameters.Add(sName:="letter",
                                            vValue:=CStr(1),
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="letter",
                                            vValue:=CStr(0),
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMInteger)
            End If
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            If oCashListItem.Batch_id < 1 Then

                m_lReturn = .Parameters.Add(sName:="batch_id",
                                            vValue:=DBNull.Value,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="batch_id",
                                            vValue:=CStr(oCashListItem.Batch_id),
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMLong)
            End If

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            If oCashListItem.pmuser_id < 1 Then
                m_lReturn = .Parameters.Add(sName:="pmuser_id",
                                            vValue:=CStr(m_iUserID),
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="pmuser_id",
                                            vValue:=CStr(oCashListItem.pmuser_id),
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMLong)
            End If

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If
            'developer guide no. 131
            If (DateTime.MinValue.Equals(oCashListItem.Transaction_Date)) Then
                m_lReturn = .Parameters.Add(sName:="transaction_date",
                                            vValue:=DBNull.Value,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMDate)
            Else
                'developer guide no. 40
                m_lReturn = .Parameters.Add(sName:="transaction_date",
                                            vValue:=oCashListItem.Transaction_Date,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMDate)
            End If


            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="original_amount",
                                        vValue:=CStr(oCashListItem.Original_Amount),
                                        iDirection:=PMEParameterDirection.PMParamInput,
                                        iDataType:=PMEDataType.PMDouble)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="amount_tendered",
                                        vValue:=CStr(oCashListItem.Amount_Tendered),
                                        iDirection:=PMEParameterDirection.PMParamInput,
                                        iDataType:=PMEDataType.PMDouble)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="change",
                                        vValue:=CStr(oCashListItem.Change),
                                        iDirection:=PMEParameterDirection.PMParamInput,
                                        iDataType:=PMEDataType.PMDouble)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            If oCashListItem.CashListItem_receipt_type_id < 1 Then

                m_lReturn = .Parameters.Add(sName:="cashlistitem_receipt_type_id",
                                            vValue:=DBNull.Value,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="cashlistitem_receipt_type_id",
                                            vValue:=CStr(oCashListItem.CashListItem_receipt_type_id),
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMLong)
            End If

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            If oCashListItem.CashListItem_receipt_status_id < 1 Then

                m_lReturn = .Parameters.Add(sName:="cashlistitem_receipt_status_id",
                                            vValue:=DBNull.Value,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="cashlistitem_receipt_status_id",
                                            vValue:=CStr(oCashListItem.CashListItem_receipt_status_id),
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMLong)
            End If

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            If oCashListItem.CashListItem_bank_id < 1 Then

                m_lReturn = .Parameters.Add(sName:="cashlistitem_bank_id",
                                            vValue:=DBNull.Value,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="cashlistitem_bank_id",
                                            vValue:=CStr(oCashListItem.CashListItem_bank_id),
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMLong)
            End If

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If
            'developer guide no. 131
            If (DateTime.MinValue.Equals(oCashListItem.Cheque_Date)) Then
                m_lReturn = .Parameters.Add(sName:="cheque_date",
                                            vValue:=DBNull.Value,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMDate)
            Else
                'developer guide no. 40
                m_lReturn = .Parameters.Add(sName:="cheque_date",
                                            vValue:=oCashListItem.Cheque_Date,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMDate)
            End If


            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="cc_number",
                                        vValue:=Trim(oCashListItem.CC_Number),
                                        iDirection:=PMEParameterDirection.PMParamInput,
                                        iDataType:=PMEDataType.PMString)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If
            'developer guide no. 131
            If (DateTime.MinValue.Equals(oCashListItem.CC_Expiry_Date)) Then
                m_lReturn = .Parameters.Add(sName:="cc_expiry_date",
                                            vValue:=DBNull.Value,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMString)
            Else
                m_lReturn = .Parameters.Add(sName:="cc_expiry_date",
                                            vValue:=oCashListItem.CC_Expiry_Date,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMString)
            End If


            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            'developer guide no. 131
            If (DateTime.MinValue.Equals(oCashListItem.CC_Start_Date)) Then
                m_lReturn = .Parameters.Add(sName:="cc_start_date",
                                            vValue:=DBNull.Value,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMString)
            Else
                m_lReturn = .Parameters.Add(sName:="cc_start_date",
                                            vValue:=oCashListItem.CC_Start_Date,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMString)
            End If


            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="cc_issue",
                                        vValue:=Trim(oCashListItem.CC_Issue),
                                        iDirection:=PMEParameterDirection.PMParamInput,
                                        iDataType:=PMEDataType.PMString)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="cc_pin",
                                        vValue:=Trim(oCashListItem.CC_Pin),
                                        iDirection:=PMEParameterDirection.PMParamInput,
                                        iDataType:=PMEDataType.PMString)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="cc_auth_code",
                                        vValue:=Trim(oCashListItem.CC_Auth_Code),
                                        iDirection:=PMEParameterDirection.PMParamInput,
                                        iDataType:=PMEDataType.PMString)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="receipt_details",
                                        vValue:=Trim(oCashListItem.Receipt_Details),
                                        iDirection:=PMEParameterDirection.PMParamInput,
                                        iDataType:=PMEDataType.PMString)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            If oCashListItem.CashListItem_Reverse_PMUser_id < 1 Then
                'has a reverse reason been given
                If oCashListItem.CashListItem_Reverse_Reason_id < 1 Then

                    m_lReturn = .Parameters.Add(sName:="cashlistitem_reverse_pmuser_id",
                                                vValue:=DBNull.Value,
                                                iDirection:=PMEParameterDirection.PMParamInput,
                                                iDataType:=PMEDataType.PMInteger)
                Else
                    m_lReturn = .Parameters.Add(sName:="cashlistitem_reverse_pmuser_id",
                                                vValue:=CStr(m_iUserID),
                                                iDirection:=PMEParameterDirection.PMParamInput,
                                                iDataType:=PMEDataType.PMInteger)
                End If
            Else
                m_lReturn = .Parameters.Add(sName:="cashlistitem_reverse_pmuser_id",
                                            vValue:=CStr(oCashListItem.CashListItem_Reverse_PMUser_id),
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMInteger)
            End If

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            If oCashListItem.CashListItem_Reverse_Reason_id < 1 Then

                m_lReturn = .Parameters.Add(sName:="cashlistitem_reverse_reason_id",
                                            vValue:=DBNull.Value,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="cashlistitem_reverse_reason_id",
                                            vValue:=CStr(oCashListItem.CashListItem_Reverse_Reason_id),
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMLong)
            End If

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            If oCashListItem.CashListItem_Payment_Type_id < 1 Then

                m_lReturn = .Parameters.Add(sName:="cashlistitem_payment_type_id",
                                            vValue:=DBNull.Value,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="cashlistitem_payment_type_id",
                                            vValue:=CStr(oCashListItem.CashListItem_Payment_Type_id),
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMLong)
            End If

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            If oCashListItem.CashListItem_Payment_Status_id < 1 Then

                m_lReturn = .Parameters.Add(sName:="cashlistitem_payment_status_id",
                                            vValue:=DBNull.Value,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="cashlistitem_payment_status_id",
                                            vValue:=CStr(oCashListItem.CashListItem_Payment_Status_id),
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMLong)
            End If

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If
            'developer guide no. 131
            If (DateTime.MinValue.Equals(oCashListItem.Date_Presented)) Then
                m_lReturn = .Parameters.Add(sName:="date_presented",
                                            vValue:=DBNull.Value,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMDate)
            Else
                'developer guide no. 40
                m_lReturn = .Parameters.Add(sName:="date_presented",
                                            vValue:=oCashListItem.Date_Presented,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMDate)
            End If


            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            If oCashListItem.Cheque_in_Possession < 1 Then

                m_lReturn = .Parameters.Add(sName:="cheque_in_possession",
                                            vValue:=DBNull.Value,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="cheque_in_possession",
                                            vValue:=CStr(oCashListItem.Cheque_in_Possession),
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMInteger)
            End If

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If
            'developer guide no. 131
            If (DateTime.MinValue.Equals(oCashListItem.Stop_Requested_Date)) Then
                m_lReturn = .Parameters.Add(sName:="stop_requested_date",
                                            vValue:=DBNull.Value,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMDate)
            Else
                'developer guide no. 40
                m_lReturn = .Parameters.Add(sName:="stop_requested_date",
                                            vValue:=oCashListItem.Stop_Requested_Date,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMDate)
            End If


            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If
            'developer guide no. 131
            If (DateTime.MinValue.Equals(oCashListItem.Stop_Printed_Date)) Then
                m_lReturn = .Parameters.Add(sName:="stop_printed_date",
                                            vValue:=DBNull.Value,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMDate)
            Else
                'developer guide no. 40
                m_lReturn = .Parameters.Add(sName:="stop_printed_date",
                                            vValue:=oCashListItem.Stop_Printed_Date,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMDate)
            End If


            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If
            'developer guide no. 131
            If (DateTime.MinValue.Equals(oCashListItem.Stop_Confirmation_Date)) Then
                m_lReturn = .Parameters.Add(sName:="stop_confirmation_date",
                                            vValue:=DBNull.Value,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMDate)
            Else
                'developer guide no. 40
                m_lReturn = .Parameters.Add(sName:="stop_confirmation_date",
                                            vValue:=oCashListItem.Stop_Confirmation_Date,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMDate)
            End If


            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="reason",
                                        vValue:=Trim(oCashListItem.Reason),
                                        iDirection:=PMEParameterDirection.PMParamInput,
                                        iDataType:=PMEDataType.PMString)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            If oCashListItem.Replaces_CashListItem_id < 1 Then

                m_lReturn = .Parameters.Add(sName:="replaces_cashlistitem_id",
                                            vValue:=DBNull.Value,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="replaces_cashlistitem_id",
                                            vValue:=CStr(oCashListItem.Replaces_CashListItem_id),
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMLong)
            End If

            m_lReturn = .Parameters.Add(sName:="XML_Object",
                                        vValue:=Trim(oCashListItem.XML_Object),
                                        iDirection:=PMEParameterDirection.PMParamInput,
                                        iDataType:=PMEDataType.PMString)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            If gPMFunctions.NullToString(oCashListItem.UnderwritingYearID) = "" Then

                m_lReturn = .Parameters.Add(sName:="underwriting_year_id",
                                            vValue:=DBNull.Value,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="underwriting_year_id",
                                            vValue:=oCashListItem.UnderwritingYearID,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMLong)
            End If

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            'Developer Guide No. 251
            If oCashListItem.CurrencyBaseDate = DateTime.MinValue OrElse oCashListItem.CurrencyBaseDate = #12/29/1899# OrElse oCashListItem.CurrencyBaseDate = #12/30/1899# Then

                m_lReturn = .Parameters.Add(sName:="currency_base_date",
                                            vValue:=DBNull.Value,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMDate)
            Else
                m_lReturn = .Parameters.Add(sName:="currency_base_date",
                                            vValue:=oCashListItem.CurrencyBaseDate,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMDate)
            End If
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="currency_base_xrate",
                                        vValue:=oCashListItem.CurrencyBaseXrate,
                                        iDirection:=PMEParameterDirection.PMParamInput,
                                        iDataType:=PMEDataType.PMDouble)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            'Developer Guide No. 251
            If oCashListItem.AccountBaseDate = DateTime.MinValue OrElse oCashListItem.AccountBaseDate = #12/29/1899# OrElse oCashListItem.AccountBaseDate = #12/30/1899# Then

                m_lReturn = .Parameters.Add(sName:="account_base_date",
                                            vValue:=DBNull.Value,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMDate)
            Else
                m_lReturn = .Parameters.Add(sName:="account_base_date",
                                            vValue:=oCashListItem.AccountBaseDate,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMDate)
            End If
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="account_base_xrate",
                                        vValue:=oCashListItem.AccountBaseXrate,
                                        iDirection:=PMEParameterDirection.PMParamInput,
                                        iDataType:=PMEDataType.PMDouble)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            'Developer Guide No. 251
            If oCashListItem.SystemBaseDate = DateTime.MinValue OrElse oCashListItem.SystemBaseDate = #12/29/1899# OrElse oCashListItem.SystemBaseDate = #12/30/1899# Then

                m_lReturn = .Parameters.Add(sName:="system_base_date",
                                            vValue:=DBNull.Value,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMDate)
            Else
                m_lReturn = .Parameters.Add(sName:="system_base_date",
                                            vValue:=oCashListItem.SystemBaseDate,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMDate)
            End If
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="system_base_xrate",
                                        vValue:=oCashListItem.SystemBaseXrate,
                                        iDirection:=PMEParameterDirection.PMParamInput,
                                        iDataType:=PMEDataType.PMDouble)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            If oCashListItem.OverrideReason = 0 Then

                m_lReturn = .Parameters.Add(sName:="exchange_rate_override_reason_id",
                                            vValue:=DBNull.Value,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="exchange_rate_override_reason_id",
                                            vValue:=oCashListItem.OverrideReason,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMLong)
            End If
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            If oCashListItem.MediaTypeIssuerID = 0 Then

                m_lReturn = .Parameters.Add(sName:="mediatype_issuer_id",
                                            vValue:=DBNull.Value,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="mediatype_issuer_id",
                                            vValue:=CStr(oCashListItem.MediaTypeIssuerID),
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMLong)
            End If
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="cc_name",
                                        vValue:=Trim(oCashListItem.CC_Name),
                                        iDirection:=PMEParameterDirection.PMParamInput,
                                        iDataType:=PMEDataType.PMString)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="cc_manual_auth_code",
                                        vValue:=Trim(oCashListItem.CC_Manual_Auth_Code),
                                        iDirection:=PMEParameterDirection.PMParamInput,
                                        iDataType:=PMEDataType.PMString)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="cc_customer",
                                        vValue:=Trim(oCashListItem.CC_Customer),
                                        iDirection:=PMEParameterDirection.PMParamInput,
                                        iDataType:=PMEDataType.PMString)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="cc_transaction_code",
                                        vValue:=Trim(oCashListItem.CC_Transaction_Code),
                                        iDirection:=PMEParameterDirection.PMParamInput,
                                        iDataType:=PMEDataType.PMString)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            'Party Bank Details
            If Informations.IsDBNull(oCashListItem.PartyBankId) OrElse oCashListItem.PartyBankId = 0 Then

                m_lReturn = .Parameters.Add(sName:="Party_bank_Id",
                                            vValue:=DBNull.Value,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="Party_bank_Id",
                                            vValue:=oCashListItem.PartyBankId,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMInteger)
            End If
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If
            'Rahul
            'developer guide no. 40
            m_lReturn = .Parameters.Add(sName:="collection_date",
                                        vValue:=oCashListItem.CollectionDate,
                                        iDirection:=PMEParameterDirection.PMParamInput,
                                        iDataType:=PMEDataType.PMDate)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="comments",
                                        vValue:=Trim(oCashListItem.Comments),
                                        iDirection:=PMEParameterDirection.PMParamInput,
                                        iDataType:=PMEDataType.PMString)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            'WPR12- Enhancement Quote Collection Process
            m_lReturn = .Parameters.Add(sName:="bank_location",
                                        vValue:=Trim(oCashListItem.BankLocation),
                                        iDirection:=PMEParameterDirection.PMParamInput,
                                        iDataType:=PMEDataType.PMString)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="bank_branch",
                                        vValue:=Trim(oCashListItem.BankBranch),
                                        iDirection:=PMEParameterDirection.PMParamInput,
                                        iDataType:=PMEDataType.PMString)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            If oCashListItem.ChequeTypeId = -1 OrElse oCashListItem.ChequeTypeId = 0 Then

                m_lReturn = .Parameters.Add(sName:="chequetype_id",
                                            vValue:=DBNull.Value,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="chequetype_id",
                                            vValue:=CStr(oCashListItem.ChequeTypeId),
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMLong)
            End If

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            If oCashListItem.CCBankId = -1 OrElse oCashListItem.CCBankId = 0 Then

                m_lReturn = .Parameters.Add(sName:="cc_bank_id",
                                            vValue:=DBNull.Value,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="cc_bank_id",
                                            vValue:=CStr(oCashListItem.CCBankId),
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMLong)
            End If

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            If oCashListItem.CardTypeId = -1 OrElse oCashListItem.CardTypeId = 0 Then

                m_lReturn = .Parameters.Add(sName:="type_of_card_id",
                                            vValue:=DBNull.Value,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="type_of_card_id",
                                            vValue:=CStr(oCashListItem.CardTypeId),
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMLong)
            End If

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="cc_trans_slip_no",
                                        vValue:=Trim(oCashListItem.CardTransSlipNo),
                                        iDirection:=PMEParameterDirection.PMParamInput,
                                        iDataType:=PMEDataType.PMString)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            If oCashListItem.ChequeClearingTypeId = -1 OrElse oCashListItem.ChequeClearingTypeId = 0 Then

                m_lReturn = .Parameters.Add(sName:="Cheque_clearing_type_id",
                                            vValue:=DBNull.Value,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="Cheque_clearing_type_id",
                                            vValue:=CStr(oCashListItem.ChequeClearingTypeId),
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMLong)
            End If

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If


            If oCashListItem.IsLeadAccount = False Then
                m_lReturn = .Parameters.Add(sName:="is_lead",
                                            vValue:=0,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="is_lead",
                                            vValue:=1,
                                            iDirection:=PMEParameterDirection.PMParamInput,
                                            iDataType:=PMEDataType.PMInteger)
            End If


            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="split_total",
                                        vValue:=CStr(oCashListItem.SplitTotal),
                                        iDirection:=PMEParameterDirection.PMParamInput,
                                        iDataType:=PMEDataType.PMCurrency)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="taxamount",
                                        vValue:=oCashListItem.TaxAmount,
                                        iDirection:=PMEParameterDirection.PMParamInput,
                                        iDataType:=PMEDataType.PMDouble)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="tax_band_id", vValue:=oCashListItem.TaxBandID,
                                        iDirection:=PMEParameterDirection.PMParamInput,
                                        iDataType:=PMEDataType.PMLong)

            If (m_lReturn <> PMEReturnCode.PMTrue) Then
                Return PMEReturnCode.PMFalse
            End If


            m_lReturn = .Parameters.Add(sName:="sBusinessIdentifierCode",
                                        vValue:=Trim(oCashListItem.BIC),
                                        iDirection:=PMEParameterDirection.PMParamInput,
                                        iDataType:=PMEDataType.PMString)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="sInternationalBankAccountNumber",
                                        vValue:=Trim(oCashListItem.IBAN),
                                        iDirection:=PMEParameterDirection.PMParamInput,
                                        iDataType:=PMEDataType.PMString)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="insurance_ref", vValue:=oCashListItem.InsuranceRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        Return nResult

    End Function


    ' ***************************************************************** '
    ' Name: GetAllocationStatus
    '
    ' Description: Calculates if an allocation is partially, totally or
    '              not allocated.
    '
    ' ***************************************************************** '
    Public Function GetAllocationStatus(ByVal v_lAllocationID As Integer, ByVal v_cCashListAmt As Decimal, ByRef r_lAllocationStatus As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing
        Dim cAllocAmt As Decimal
        Dim sMsg As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT SUM(alloc_base_amount) " &
                   "FROM AllocationDetail " &
                   "WHERE cashlistitem_id = {cashlistitem_id}"

            ' Clear the parameters ...
            m_oDatabase.Parameters.Clear()

            ' ... and add the allocation_id parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="cashlistitem_id", vValue:=CStr(v_lAllocationID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Perform the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetAllocationSum", bStoredProcedure:=False, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed on SQL : " & sSQL, vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllocationStatus", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Check the return values

            If (Not Informations.IsArray(vResultArray)) Or (CStr(vResultArray(0, 0)) = "") Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the allocated amount

            cAllocAmt = CDec(vResultArray(0, 0))

            If cAllocAmt = 0 Then
                ' Totally unallocated
                r_lAllocationStatus = gACTLibrary.ACTAllocationStatusUnallocated
            ElseIf (cAllocAmt = v_cCashListAmt) Then
                ' Fully allocated
                r_lAllocationStatus = gACTLibrary.ACTAllocationStatusAllocated
            ElseIf (cAllocAmt < v_cCashListAmt) Then
                ' Partially allocated
                r_lAllocationStatus = gACTLibrary.ACTAllocationStatusPartial
            Else
                ' Dont know. Shouldn't be more!
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Construct the error message
                sMsg = "Failed to check allocated amount (" & cAllocAmt &
                       ") against cash list amount (" & CStr(v_cCashListAmt)
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMsg, vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllocationStatus", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllocationStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllocationStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    'made changes to this function so that is works,
    'pass in cashlistitem_ID not allocation_id
    'changed queries to sum amounts when workng out whether item is fully allocated
    'sw 16/12/2002
    Public Function GetMultiAllocationStatus(ByVal v_lCashListItemID As Integer, ByVal v_iCashListCurrency As Integer, ByVal v_cCashListAmt As Decimal, ByRef r_lAllocationStatus As Integer, ByRef r_cAllocAmt As Decimal) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing
        Dim sMsg As String = ""
        Dim cCurrDiff As Decimal

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get the most recent allocated amount
            sSQL = ""
            sSQL = sSQL & "SELECT" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    SUM(CASE" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "            WHEN original_currency = {original_currency} THEN alloc_ccy_amount" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "            ELSE alloc_base_amount" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        END)," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    SUM (loss_gain_amount)" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "FROM AllocationDetail" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE cashlistitem_id = {cashlistitem_id}" & Strings.ChrW(13) & Strings.ChrW(10)

            ' Clear the parameters ...
            m_oDatabase.Parameters.Clear()

            ' ... and add the allocation_id parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="cashlistitem_id", vValue:=CStr(v_lCashListItemID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="original_currency", vValue:=CStr(v_iCashListCurrency), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Perform the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetAllocationSum", bStoredProcedure:=False, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed on SQL : " & sSQL, vApp:=ACApp, vClass:=ACClass, vMethod:="GetMultiAllocationStatus", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Check the return values

            If (Not Informations.IsArray(vResultArray)) Or (CStr(vResultArray(0, 0)) = "") Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the FULL allocated amount

            r_cAllocAmt = CDec(vResultArray(0, 0))

            cCurrDiff = 0

            If vResultArray.GetUpperBound(0) <> 0 Then

                If CStr(vResultArray(1, 0)) <> "" Then

                    cCurrDiff = CDec(vResultArray(1, 0))
                End If
            End If


            If r_cAllocAmt = 0 Then
                ' Totally unallocated
                r_lAllocationStatus = gACTLibrary.ACTAllocationStatusUnallocated
            ElseIf (Math.Abs(r_cAllocAmt) = Math.Abs(v_cCashListAmt)) Then
                ' Fully allocated
                r_lAllocationStatus = gACTLibrary.ACTAllocationStatusAllocated
            ElseIf (r_cAllocAmt < v_cCashListAmt And r_cAllocAmt > 0) Or (r_cAllocAmt > v_cCashListAmt And r_cAllocAmt < 0) Then
                ' Partially allocated
                r_lAllocationStatus = gACTLibrary.ACTAllocationStatusPartial
            Else
                If cCurrDiff <> 0 Then
                    ' must have write off, so  Fully allocated
                    r_lAllocationStatus = gACTLibrary.ACTAllocationStatusAllocated
                Else
                    ' Dont know. Shouldn't be more!
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Construct the error message
                    sMsg = "Failed to check allocated amount (" & r_cAllocAmt &
                           ") against cash list amount (" & CStr(v_cCashListAmt) & ")"
                    ' Log Error Message
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMsg, vApp:=ACApp, vClass:=ACClass, vMethod:="GetMultiAllocationStatus", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetMultiAllocationStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMultiAllocationStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: BeginTrans (Public)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    Public Function BeginTrans() As Integer

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
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetLetterDetails
    '
    ' Description: Gets information required for
    '
    ' ***************************************************************** '
    Public Function GetLetterDetails(ByVal lAccountId As Integer, ByVal lTransdetailId As Integer, ByRef lPartyCnt As Integer, ByRef sShortName As String, ByRef sDocumentRef As String) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="account_id", vValue:=CStr(lAccountId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = .Parameters.Add(sName:="transdetail_id", vValue:=CStr(lTransdetailId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = .SQLSelect(sSQL:=ACGetLetterDetailsSQL, sSQLName:=ACGetLetterDetailsName, bStoredProcedure:=ACGetLetterDetailsStored, lNumberRecords:=0, vResultArray:=vResultArray)

            End With

            If (m_lReturn = gPMConstants.PMEReturnCode.PMNotFound) Or Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            lPartyCnt = CInt(vResultArray(0, 0))

            sShortName = CStr(vResultArray(1, 0))

            sDocumentRef = CStr(vResultArray(2, 0))


            Return m_lReturn

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLetterDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLetterDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (Public)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    Public Function CommitTrans() As Integer

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
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Public)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    Public Function RollbackTrans() As Integer

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
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
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
    ' Name: GetHiddenOption
    '
    ' Description: get value from hidden_option to see if we are underwriting or agency
    '
    ' History: 07/09/2001 TN - Created.
    '
    ' ***************************************************************** '
    Public Function GetHiddenOption(ByRef r_sResult As String) As Integer

        Dim result As Integer = 0
        Dim oDatabase As dPMDAO.Database = Nothing
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            'open sirius database
            If gPMComponentServices.NewDatabase(v_sUsername:=m_sUsername, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, r_oDatabase:=oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse

            End If


            sSQL = "SELECT value FROM hidden_options WHERE branch_id = {branch_id}" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "AND option_number = {option_number}"

            oDatabase.Parameters.Clear()

            m_lReturn = oDatabase.Parameters.Add(sName:="branch_id", vValue:=CStr(1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                m_lReturn = oDatabase.CloseDatabase()
                oDatabase = Nothing

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = oDatabase.Parameters.Add(sName:="option_number", vValue:=CStr(1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                m_lReturn = oDatabase.CloseDatabase()
                oDatabase = Nothing

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="Get Hidden Option", bStoredProcedure:=False, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = oDatabase.CloseDatabase()
                oDatabase = Nothing

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
                m_lReturn = oDatabase.CloseDatabase()
                oDatabase = Nothing
                Return result
            End If


            r_sResult = CStr(vResultArray(0, 0)).ToUpper()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetHiddenOption Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetHiddenOption", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name : AddTaskToWorkManager
    '
    ' Desc : Add task to work manager
    '
    ' History: 07/09/2001 TN - Created.
    '
    ' ***************************************************************** '
    Public Function AddTaskToWorkManager(ByRef r_lPMWrkTaskInstanceCnt As Integer, ByVal v_sCustomer As String, ByVal v_sDescription As String, ByVal v_dtTaskDueDate As Date, Optional ByVal v_lPMWrkTaskID As Integer = 0, Optional ByVal v_sTaskCode As String = "", Optional ByVal v_lPMWrkTaskGroupID As Integer = 0, Optional ByVal v_sTaskGroupCode As String = "", Optional ByVal v_iUserID As Integer = 0, Optional ByVal v_sUserGroupCode As String = "PURCLDGR", Optional ByVal v_vKeyArray As Object = Nothing, Optional ByVal v_iIsUrgent As Integer = 0, Optional ByVal v_iIsVisible As Integer = gPMConstants.PMEReturnCode.PMTrue) As Integer

        Dim result As Integer = 0
        Dim oDatabase As Object = Nothing
        Dim oWrkTaskInstance As bPMWrkTaskInstance.TaskControl

        Dim sSQL As String = ""
        Dim vResultArray As Object = Nothing
        Dim lUserGroupID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'check to see if we have task_id or task code
            If v_lPMWrkTaskID = 0 And v_sTaskCode = "" Then
                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Must supply either task id or task code", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            End If

            'check to see if we have task_group_id or task group code
            If v_lPMWrkTaskGroupID = 0 And v_sTaskGroupCode = "" Then
                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Must supply either task group id or task group code", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            End If


            'open architecture database

            If gPMComponentServices.NewDatabase(v_sUsername:=m_sUsername, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_oDatabase:=oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            oWrkTaskInstance = New bPMWrkTaskInstance.TaskControl
            If oWrkTaskInstance.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            '*******************************************************
            'get user group id
            '*******************************************************
            ' Clear the database parameters

            oDatabase.Parameters.Clear()

            If v_sUserGroupCode <> "" Then
                sSQL = "SELECT pmuser_group_id FROM pmuser_group WHERE code = {group_code}"

                ' Add the user_id parameter

                If oDatabase.Parameters.Add(sName:="group_code", vValue:=ToSafeString(v_sUserGroupCode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else
                sSQL = "SELECT pmuser_group_id FROM pmuser_group_user WHERE user_id = {user_id}"


                ' Add the user_id parameter

                If oDatabase.Parameters.Add(sName:="user_id", vValue:=ToSafeInteger(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If


            If oDatabase.SQLSelect(sSQL:=ToSafeString(sSQL), sSQLName:="GetGroupIDs", bStoredProcedure:=False, vResultArray:=vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'do we have any data
            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the user_group_id

            lUserGroupID = CInt(vResultArray(0, 0))

            '*******************************************************
            ' Get the task_id
            '*******************************************************
            If v_sTaskCode <> "" Then
                sSQL = "SELECT pmwrk_task_id FROM PMWrk_Task WHERE code = {task_code}"


                oDatabase.Parameters.Clear()


                If oDatabase.Parameters.Add(sName:="task_code", vValue:=ToSafeString(v_sTaskCode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                If oDatabase.SQLSelect(sSQL:=ToSafeString(sSQL), sSQLName:="GetTaskID", bStoredProcedure:=False, vResultArray:=vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Not Informations.IsArray(vResultArray) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                v_lPMWrkTaskID = CInt(vResultArray(0, 0))
            End If

            '*******************************************************
            'get task group id
            '*******************************************************
            If v_sTaskGroupCode <> "" Then
                sSQL = "SELECT pmwrk_task_group_id FROM PMWrk_Task_group WHERE code = {task_group_code}"


                oDatabase.Parameters.Clear()


                If oDatabase.Parameters.Add(sName:="task_group_code", vValue:=ToSafeString(v_sTaskGroupCode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                If oDatabase.SQLSelect(sSQL:=ToSafeString(sSQL), sSQLName:="GetGroupTaskID", bStoredProcedure:=False, vResultArray:=vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Not Informations.IsArray(vResultArray) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                v_lPMWrkTaskGroupID = CInt(vResultArray(0, 0))
            End If

            '*******************************************************
            'create task
            '*******************************************************

            If oWrkTaskInstance.CreateNew(v_lPMWrkTaskGroupID:=v_lPMWrkTaskGroupID, v_lPMWrkTaskID:=v_lPMWrkTaskID, v_sCustomer:=v_sCustomer, v_dtTaskDueDate:=v_dtTaskDueDate, v_lPMUserGroupID:=lUserGroupID, v_sDescription:=v_sDescription, v_iTaskStatus:=gPMConstants.PMEWrkManTaskStatus.pmeWMTSNew, v_iIsUrgent:=v_iIsUrgent, r_lPMWrkTaskInstanceCnt:=r_lPMWrkTaskInstanceCnt, v_iUserID:=v_iUserID, v_vKeyArray:=v_vKeyArray) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            'close database

            m_lReturn = oDatabase.CloseDatabase()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddTaskToWorkManager Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Public Function ProcessWTM(ByVal v_lCashListItemID As Integer) As Integer

        Dim result As Integer = 0
        Dim oDatabase As Object = Nothing
        Dim oWrkTaskInstance As bPMWrkTaskInstance.TaskControl = Nothing
        Try

            Dim vInstanceCnt(,) As Object = Nothing

            result = gPMConstants.PMEReturnCode.PMTrue

            'Let's get the Instance Cnt using the Key Name and the Key Value
            With m_oDatabase
                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="KeyName", vValue:=ToSafeString(ACCashListItemKeyName), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessWTM Failed to add Parameter", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessWTM", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                m_lReturn = .Parameters.Add(sName:="KeyValue", vValue:=CStr(v_lCashListItemID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessWTM Failed to add Parameter", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessWTM", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                m_lReturn = .SQLSelect(sSQL:=ACGetWTMInstanceCntSQL, sSQLName:=ACGetWTMInstanceCntName, bStoredProcedure:=ACGetWTMInstanceCntStored, vResultArray:=vInstanceCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessWTM Failed to run the stored proc", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessWTM", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

            End With

            If Informations.IsArray(vInstanceCnt) Then
                'open architecture database

                m_lReturn = CType(gPMComponentServices.NewDatabase(v_sUsername:=m_sUsername, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_oDatabase:=oDatabase), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessWTM Failed to Check NewDatabase", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessWTM", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                oWrkTaskInstance = New bPMWrkTaskInstance.TaskControl
                m_lReturn = CType(oWrkTaskInstance.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessWTM Failed to Create bPMWrkTaskInstance object", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessWTM", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If


                For lCount As Integer = 0 To vInstanceCnt.GetUpperBound(1)

                    m_lReturn = oWrkTaskInstance.SetStatusComplete(v_lPMWrkTaskInstanceCnt:=gPMFunctions.NullToLong(vInstanceCnt(0, lCount)))
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn
                        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessWTM Failed to Set the WTM status to complete", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessWTM", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Exit For
                    End If
                    '            PN28020 RKS - Don't Delete the Task now otherwise it will not show the task as completed in work manager
                    '            m_lReturn& = oWrkTaskInstance.Delete(v_lPMWrkTaskInstanceCnt:=NullToLong(vInstanceCnt(0, lCount)))
                    '            If m_lReturn& <> PMTrue Then
                    '                ProcessWTM = m_lReturn&
                    '                LogMessage sUserName:=m_sUsername$, iType:=PMLogOnError, _
                    ''                           sMsg:="ProcessWTM Failed to delete WTM", _
                    ''                           vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessWTM", _
                    ''                           vErrNo:=Err.Number, vErrDesc:=Err.Description
                    '                Exit For
                    '            End If
                Next lCount
                If Not (oWrkTaskInstance Is Nothing) Then

                    oWrkTaskInstance.Dispose()
                    oWrkTaskInstance = Nothing
                End If
                If Not (oDatabase Is Nothing) Then

                    m_lReturn = oDatabase.CloseDatabase()
                    oDatabase = Nothing
                End If
            Else
                'We must have at least one
                result = gPMConstants.PMEReturnCode.PMError
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            If Not (oWrkTaskInstance Is Nothing) Then

                oWrkTaskInstance.Dispose()
                oWrkTaskInstance = Nothing
            End If
            If Not (oDatabase Is Nothing) Then

                m_lReturn = oDatabase.CloseDatabase()
                oDatabase = Nothing
            End If
            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessWTM Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessWTM", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetCashListType
    '
    ' Description:
    '
    ' History: 10/09/2001 TN - Created.
    '
    ' ***************************************************************** '
    Public Function GetCashListType(ByVal v_lCashListTypeID As Integer, ByRef r_sCashListType As String) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT description FROM CashListType WHERE cashlisttype_id = {cashlisttypeid}"

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="cashlisttypeid", vValue:=CStr(v_lCashListTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetCashListType", bStoredProcedure:=False, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            r_sCashListType = CStr(vResultArray(0, 0))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCashListType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCashListType", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function ValidateMediaReference(ByVal v_lCashListID As Integer, ByVal v_lCashListItemID As Integer, ByVal v_lMediaTypeID As Integer, ByVal v_sMediaRef As String, ByRef r_bValid As Boolean, ByRef r_iPeriodMonths As Integer, ByRef r_bValidateUI As Boolean) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: ValidateMediaReference
        ' PURPOSE: Checks to see that a media reference has not been used
        ' within the last x months.
        ' AUTHOR: Danny Davis
        ' DATE: 17/05/2002, 10:02
        ' RETURNS: PMTrue for success
        ' SPECIAL INSTRUCTIONS:
        ' r_bValid is set to False for invalid and the PeriodMonths set for the
        ' message to the user.
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim lCashlistitemID As Integer = Nothing


        Try

            With m_oDatabase
                .Parameters.Clear()
                .Parameters.Add("cashlist_id", CStr(v_lCashListID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                'DD 22/08/2003: Added parameter for CashListItemID
                .Parameters.Add("cashlistitem_id", CStr(v_lCashListItemID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                .Parameters.Add("mediatype_id", CStr(v_lMediaTypeID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                .Parameters.Add("media_ref", v_sMediaRef, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Parameters.Add("period_months", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMInteger)
                'DD 28/08/2003: Added parameter to determine if a UI validation
                'is required

                .Parameters.Add("validate_ui", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMInteger)

                .Parameters.Add("valid", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMInteger)

                result = .SQLAction(sSQL:=ACValidateSQL, sSQLName:=ACValidateName, bStoredProcedure:=ACValidateStored)

                'If it all went okay then get the return values
                If result = gPMConstants.PMEReturnCode.PMTrue Then
                    r_bValid = (gPMFunctions.NullToLong(.Parameters.Item("valid").Value) = 1)
                    If Not r_bValid Then
                        r_iPeriodMonths = gPMFunctions.NullToLong(.Parameters.Item("period_months").Value)
                    End If
                    r_bValidateUI = (gPMFunctions.NullToLong(.Parameters.Item("validate_ui").Value) = 1)
                End If
            End With


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateMediaReference", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

        Finally



        End Try
        Return result
    End Function


    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: GetInstalmentDetails
    ' PURPOSE: gets instalment plan details for a given account
    ' AUTHOR: steve watton
    ' DATE: 23-10-2002
    ' RETURNS: PMTrue for success
    ' ---------------------------------------------------------------------------

    Public Function GetInstalmentDetails(ByVal v_lAccountID As Integer, ByRef r_vInstalArray(,) As Object, Optional ByVal v_sThirdPartyOnly As String = "") As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add accountid as an input param for select
            m_lReturn = m_oDatabase.Parameters.Add(sName:="account_id", vValue:=CStr(v_lAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add accountid as an input param for select
            m_lReturn = m_oDatabase.Parameters.Add(sName:="third_party", vValue:=v_sThirdPartyOnly, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelInstalmentsForAccountSQL, sSQLName:=ACSelInstalmentsForAccountName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vInstalArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInstalmentDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInstalmentDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Private Function CreateCashlistForBankGuarantee(ByVal vBGPolicyArray(,) As Object, ByVal lCashlistitemID As Integer, ByVal lCashlistID As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CreateCashlistForBankGuarantee"

        Dim lRowCount As Integer
        Dim oBankGuarantee As bSIRBankGuarantee.Business



        result = gPMConstants.PMEReturnCode.PMTrue

        'add the new instalment records

        lRowCount = vBGPolicyArray.GetUpperBound(1)

        If Informations.IsArray(vBGPolicyArray) Then
            oBankGuarantee = New bSIRBankGuarantee.Business
            m_lReturn = CType(oBankGuarantee.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "oBankGuarantee.UpdateCashListItemForBG Failed", gPMConstants.PMELogLevel.PMLogError)
            End If



            m_lReturn = oBankGuarantee.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "oBankGuarantee.Initialise Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            'populate the instalment plan cbo with the distinct Plan names
            For lRow As Integer = 0 To lRowCount

                m_lReturn = oBankGuarantee.UpdateCashListItemForBG(vBGPolicyArray(MainModule.ENBankGuarantee.BGId, lRow), lCashListId:=lCashlistID, lCashListitemId:=lCashlistitemID, lInsuranceFileCnt:=vBGPolicyArray(MainModule.ENBankGuarantee.InsuranceFileCnt, lRow), cAmtToBePosted:=vBGPolicyArray(MainModule.ENBankGuarantee.AmtTobePosted, lRow))

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "oBankGuarantee.UpdateCashListItemForBG Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            Next
        End If
        Return result

    End Function

    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: CreateCashListIteminstalments
    ' PURPOSE: creates cashlistitem instalment records
    ' AUTHOR: steve watton
    ' DATE: 23-10-2002
    ' RETURNS: PMTrue for success
    ' IMPORTANT: deletes existing cashlistiteminstalment records for given item and
    '               Creates cashlistiteminstalment records from the array passed,
    '              each row in the array contains a true/false flag to indicate whether
    '               the appropriate cashlistitem record needs to be created
    ' ---------------------------------------------------------------------------


    Public Function CreateCashlistItemInstalments(ByVal v_vInstalmentDetails As Object, ByVal v_lCashListItemID As Integer) As Integer

        Dim result As Integer = 0
        Dim lRowCount As Integer

        Const ACInstalmentFlagElement As Integer = 8

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'add the new instalment records

            lRowCount = v_vInstalmentDetails.GetUpperBound(1)

            'populate the instalment plan cbo with the distinct Plan names
            For lRow As Integer = 0 To lRowCount
                If v_vInstalmentDetails(ACInstalmentFlagElement, lRow) = gPMConstants.PMEReturnCode.PMTrue Then
                    'add a cashlistitem instalment record

                    ' Clear the Database Parameters Collection
                    m_oDatabase.Parameters.Clear()

                    ' Add cashlisttitemid as an input param for insert
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="cashlistitem_id", vValue:=CStr(v_lCashListItemID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Add pfprem_finance_cnt as an input param for insert

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="pfinstalments_id", vValue:=CStr(v_vInstalmentDetails(ACPFInstalmentsID, lRow)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Execute SQL Statement
                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCreateCashlistItemInstalmentsSQL, sSQLName:=ACCreateCashlistItemInstalmentsName, bStoredProcedure:=True)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            Next

            Return result

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateCashlistItemInstalments Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateCashlistItemInstalments", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function




    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: DeleteCashListIteminstalments
    ' PURPOSE: creates cashlistitem instalment records
    ' AUTHOR: steve watton
    ' DATE: 23-10-2002
    ' RETURNS: PMTrue for success
    ' IMPORTANT: deletes existing cashlistiteminstalment records for given item
    ' ---------------------------------------------------------------------------

    Private Function DeleteCashlistItemInstalments(ByVal v_lCashListItemID As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        'first of all delete any instalment records that relate to this cashlistitem

        m_oDatabase.Parameters.Clear()

        ' Add instalmentnumber as an input param for insert
        m_lReturn = m_oDatabase.Parameters.Add(sName:="cashlistitem_id", vValue:=CStr(v_lCashListItemID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACDeleteCashlistItemInstalmentsSQL, sSQLName:=ACDeleteCashlistItemInstalmentsName, bStoredProcedure:=True)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Return result

    End Function


    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: SelectCashListIteminstalments
    ' PURPOSE: selects cashlistitem instalment records
    ' AUTHOR: steve watton
    ' DATE: 01-11-2002
    ' RETURNS: PMTrue for success
    ' IMPORTANT: selects cashlistitem instalment records
    ' ---------------------------------------------------------------------------

    Public Function SelectCashlistItemInstalments(ByVal v_lCashListItemID As Integer, ByRef v_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add accountid as an input param for select
            m_lReturn = m_oDatabase.Parameters.Add(sName:="CashListItem_ID", vValue:=CStr(v_lCashListItemID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectCashlistItemInstalmentsSQL, sSQLName:=ACSelectCashlistItemInstalmentsName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=v_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectCashlistItemInstalments Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectCashlistItemInstalments", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function



    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: UpdateTablesForMediaRefChange
    ' PURPOSE: update the neccessary tables for a media reference change on a cashlistitem
    ' AUTHOR: steve watton
    ' DATE: 08-11-2002
    ' RETURNS: PMTrue for success
    ' ---------------------------------------------------------------------------

    Public Function UpdateDBForMediaRefChange(ByVal v_lCashListItemID As Integer, ByVal v_lPMUserID As Integer, ByVal v_sOldMediaRef As String, ByVal v_sNewMediaRef As String, ByVal v_sOurRef As String, ByVal v_sTheirRef As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add output param for select to store fail code

            m_lReturn = m_oDatabase.Parameters.Add(sName:="outputparam", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add cashlistitemid as an input param for select
            m_lReturn = m_oDatabase.Parameters.Add(sName:="cashlistitemid", vValue:=CStr(v_lCashListItemID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add pmuserid as an input param for select
            m_lReturn = m_oDatabase.Parameters.Add(sName:="pmuserid", vValue:=CStr(v_lPMUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add oldmediaref as an input param for select
            m_lReturn = m_oDatabase.Parameters.Add(sName:="oldmediaref", vValue:=v_sOldMediaRef.Trim(), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Add newmediaref as an input param for select
            m_lReturn = m_oDatabase.Parameters.Add(sName:="newmediaref", vValue:=v_sNewMediaRef.Trim(), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Add ourref as an input param for select
            m_lReturn = m_oDatabase.Parameters.Add(sName:="ourref", vValue:=v_sOurRef.Trim(), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Add theirref as an input param for select
            m_lReturn = m_oDatabase.Parameters.Add(sName:="theirref", vValue:=v_sTheirRef.Trim(), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateMediaRefSQL, sSQLName:=ACUpdateMediaRefName, bStoredProcedure:=ACUpdateStored)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or (m_oDatabase.Parameters.Item("outputparam").Value = gPMConstants.PMEReturnCode.PMFail) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateDBForMediaRefChange Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateDBForMediaRefChange", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function




    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: Update Tables For Stop Cheque
    ' PURPOSE: update the neccessary tables for a stop cheque request
    ' AUTHOR: steve watton
    ' DATE: 11-11-2002
    ' RETURNS: PMTrue for success
    ' ---------------------------------------------------------------------------

    Public Function UpdateDBForStopCheque(ByVal v_lCashListItemID As Integer, ByVal v_sStopReason As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add cashlistitemid as an input param for select
            m_lReturn = m_oDatabase.Parameters.Add(sName:="cashlistitemid", vValue:=CStr(v_lCashListItemID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add cashlistitemid as an input param for select
            m_lReturn = m_oDatabase.Parameters.Add(sName:="stopreason", vValue:=v_sStopReason, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateStopChequeSQL, sSQLName:=ACUpdateStopChequeName, bStoredProcedure:=ACUpdateStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateDBForStopCheque Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateDBForStopCheque", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function



    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: UpdateDBForStopChequeConfirm
    ' PURPOSE: Update cash List Item For Stop Cheque confirmation
    ' AUTHOR: steve watton
    ' DATE: 13-11-2002
    ' RETURNS: PMTrue for success
    ' ---------------------------------------------------------------------------

    Public Function UpdateDBForStopChequeConfirm(ByVal v_lCashListItemID As Integer, ByVal v_dBankConfirmDate As Date) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add cashlistitemid as an input param for select
            m_lReturn = m_oDatabase.Parameters.Add(sName:="cashlistitemid", vValue:=CStr(v_lCashListItemID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add cashlistitemid as an input param for select
            'Developer Guide No. 40
            m_lReturn = m_oDatabase.Parameters.Add(sName:="stopconfirmdate", vValue:=v_dBankConfirmDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateStopChequeConfirmSQL, sSQLName:=ACUpdateStopChequeConfirmName, bStoredProcedure:=ACUpdateStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateDBForStopChequeConfirm Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateDBForStopChequeConfirm", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: UpdateDBForCancelCheque
    ' PURPOSE: Update cash List Item For Cancel Cheque and Reverses Docuemnt
    ' AUTHOR: steve watton
    ' DATE: 13-11-2002
    ' RETURNS: PMTrue for success
    ' ---------------------------------------------------------------------------

    Public Function UpdateDBForCancelCheque(ByVal v_lTransdetailId As Integer, ByVal v_lCashListItemID As Integer, ByVal v_sCancellationReason As Object) As Integer
        Dim result As Integer = 0
        Dim oReversal As bACTDocumentReversal.Business

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Perform the reversal operations

            oReversal = New bACTDocumentReversal.Business()

            m_lReturn = oReversal.Initialise(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oReversal.TransDetailId = v_lTransdetailId

            m_lReturn = oReversal.Start()

            oReversal = Nothing

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'update the status on the cashlistitem to cancelled

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add cashlistitemid as an input param for select
            m_lReturn = m_oDatabase.Parameters.Add(sName:="cashlistitem_id", vValue:=CStr(v_lCashListItemID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Add cancellation reason as an input param for select

            m_lReturn = m_oDatabase.Parameters.Add(sName:="reason", vValue:=CStr(v_sCancellationReason), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateCancelCashListItemSQL, sSQLName:=ACUpdateCancelCashListItemName, bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateDBForCancelCheque Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateDBForcancelCheque", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function




    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: GetAccountAndUserGroupCode
    ' PURPOSE: Update cash List Item For Cancel Cheque and Reverses Docuemnt
    ' AUTHOR: steve watton
    ' DATE: 13-11-2002
    ' RETURNS: PMTrue for success
    ' ---------------------------------------------------------------------------
    Public Function GetAccountAndUserGroupCode(ByVal v_lAccountID As Integer, ByVal v_lUserGroupID As Integer, ByRef r_sAccountCode As String, ByRef r_sUsergroupCode As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add accountid as an input param for select
            m_lReturn = m_oDatabase.Parameters.Add(sName:="accountid", vValue:=CStr(v_lAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add usergroupid reason as an input param for select
            m_lReturn = m_oDatabase.Parameters.Add(sName:="usergroupid", vValue:=CStr(v_lUserGroupID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Add account code as an OUTPUT param
            m_lReturn = m_oDatabase.Parameters.Add(sName:="accountcode", vValue:=r_sAccountCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Add usergroupcode as an OUTPUT param
            m_lReturn = m_oDatabase.Parameters.Add(sName:="usergroupcode", vValue:=r_sUsergroupCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetAccountAndUserGroupCodeSQL, sSQLName:=ACGetAccountAndUserGroupCodeName, bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the account code
            r_sAccountCode = gPMFunctions.NullToString(m_oDatabase.Parameters.Item("accountcode").Value).Trim()

            ' Get the usergroup code
            r_sUsergroupCode = gPMFunctions.NullToString(m_oDatabase.Parameters.Item("usergroupcode").Value).Trim()

            Return result

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountAndUserGroupCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountAndUserGroupCode", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function GetUserGroupID(ByVal v_lUserId As Integer, ByRef r_lUserGroupID As Integer) As Integer

        Dim result As Integer = 0
        Try

            Dim oUserGroup As bPMUserGroup.Lookup
            Dim vUserGroupArray(,) As Object = Nothing

            Const ACSysAdminGroupCode As String = "SYSADMIN"

            result = gPMConstants.PMEReturnCode.PMTrue
            oUserGroup = New bPMUserGroup.Lookup
            m_lReturn = CType(oUserGroup.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserGroupID Failed to create bPMUUserGroup.Lookup object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserGroupID", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If


            m_lReturn = oUserGroup.GetUsersGroups(v_iUserID:=v_lUserId, v_dtEffectiveDate:=DateTime.Now, r_vUsersGroupsArray:=vUserGroupArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserGroupID Failed to GetUserGroups", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserGroupID", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            If Informations.IsArray(vUserGroupArray) Then
                'We want the user group that is not SYSADMIN but just in case the user is ONLY in sysadmin

                For iCount As Integer = 0 To vUserGroupArray.GetUpperBound(1)
                    Select Case gPMFunctions.NullToString(vUserGroupArray(1, iCount)).Trim().ToUpper()
                        Case ACSysAdminGroupCode
                            r_lUserGroupID = gPMFunctions.NullToLong(vUserGroupArray(0, iCount))
                        Case Else
                            r_lUserGroupID = gPMFunctions.NullToLong(vUserGroupArray(0, iCount))
                            Exit For
                    End Select
                Next iCount
            Else
                result = gPMConstants.PMEReturnCode.PMError
                r_lUserGroupID = 0
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserGroupID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserGroupID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function GetAdditionalFields(ByVal v_lCashListItemID As Integer, ByRef r_sXML As String) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetAdditionalFields
        ' PURPOSE: gets the xml data associated with a cashlistitem that belongs to a batch
        ' AUTHOR: steve watton
        ' DATE: 13-11-2002
        ' RETURNS: PMTrue for success
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            ' Add usergroupcode as an OUTPUT param
            m_lReturn = m_oDatabase.Parameters.Add(sName:="cashlistitemid", vValue:=CStr(v_lCashListItemID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAdditionalFieldsSQL, sSQLName:=ACGetAdditionalFieldsName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            r_sXML = CStr(vResultArray(0, 0))

            Return result

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAdditionalFields Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAdditionalFields", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    Public Function GetPostedTransaction(ByVal lTransdetailId As Integer, ByRef r_vResultArray(,) As Object) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetPostedTransaction
        ' PURPOSE:
        ' AUTHOR: Danny Davis
        ' DATE: 26/04/2004
        ' RETURNS: PMTrue for success
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            ' Add usergroupcode as an OUTPUT param
            m_lReturn = m_oDatabase.Parameters.Add(sName:="transdetail_id", vValue:=CStr(lTransdetailId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            'developer guide no. 39
            m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_ACT_Select_CashListItem_Posted_Transaction", sSQLName:="Get CashListItem Posted Transaction", bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPostedTransaction Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPostedTransaction", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: GetPaymentStatusIDFromCode
    ' PURPOSE: gets ID field from cashlistitem_payment_status for the code passed
    ' AUTHOR: steve watton
    ' DATE: 28/01/2003
    ' RETURNS: PMTrue for success
    ' ---------------------------------------------------------------------------

    Public Function GetPaymentStatusIDFromCode(ByVal v_sCode As String, ByRef r_lID As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            ' Add usergroupcode as an OUTPUT param
            m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=v_sCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPaymentStatusIDFromCodeSQL, sSQLName:=ACGetPaymentStatusIDFromCodeName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            r_lID = CInt(vResultArray(0, 0))

            Return result

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPaymentStatusIDFromCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPaymentStatusIDFromCode", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: CreateCashListItemSalvage
    ' PURPOSE: creates cashlistitem Salvage records
    ' AUTHOR: steve watton
    ' DATE: 23-10-2002
    ' RETURNS: PMTrue for success
    '
    ' THIS NEEDS TO BE IMPLEMENTED WHEN THE CLIENT PROJECT HAS BEEN FINISHED
    ' ---------------------------------------------------------------------------

    Private Function CreateCashlistItemSalvage(ByVal v_vSalvageDetails As Object, ByVal v_lCashListItemID As Integer, ByVal v_lAccountID As Integer) As Integer

        Dim result As Integer = 0
        Dim lPartyCnt As Integer
        Dim vDocIDs As Object = Nothing



        'fist find the perty count from the account_ID
        m_lReturn = CType(GetPartyCntFromAccountID(v_lAccountID, lPartyCnt), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or lPartyCnt = 0 Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'create the salvage business object

        If m_oSalvage Is Nothing Then
            If gPMComponentServices.CreateBusinessObject(r_oObject:=m_oSalvage, v_sClassName:="bCLMSalvageReceipt.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        m_lReturn = m_oSalvage.ProcessSalvageReceipts(vSalvageItemArray:=v_vSalvageDetails, lCashlistitemID:=ToSafeInteger(v_lCashListItemID), lPartyCnt:=ToSafeInteger(lPartyCnt), r_vDocument_ids:=vDocIDs)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Return gPMConstants.PMEReturnCode.PMTrue

    End Function

    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: DeleteCashListItemSalvage
    ' PURPOSE: Deletes cashlistitem Salvage records
    ' AUTHOR: steve watton
    ' DATE: 23-10-2002
    ' RETURNS: PMTrue for success
    '
    ' THIS NEEDS TO BE IMPLEMENTED WHEN THE CLIENT PROJECT HAS BEEN FINISHED
    ' ---------------------------------------------------------------------------

    Private Function DeleteCashlistItemSalvage(ByVal v_lCashListItemID As Integer) As Integer




        Return gPMConstants.PMEReturnCode.PMTrue

    End Function

    ' ***************************************************************** '
    ' Name: ApproveClaimPayment
    '
    ' Parameters: N/A
    '
    ' Description: approves the loss schedule items related to the
    '               specified cashlistitem
    '
    ' History:
    '           Created : MEvans : 23-05-2003 : CQ 709
    ' ---------------------------------------------------------------------------
    Function ApproveClaimPayment(ByVal v_lCashListItemID As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "ApproveClaimPayment"

        Dim oLossSched As Object = Nothing
        Dim bUpdatedLSItems As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the claims loss schedule business object
            If gPMComponentServices.CreateBusinessObject(r_oObject:=oLossSched, v_sClassName:="bCLMSalvageReceipt.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase) = gPMConstants.PMEReturnCode.PMTrue Then

                ' approve the loss schedule items relating to the cashlistitem...

                If oLossSched.ApproveLossScheduleItemPayments(v_lCashListItemID:=ToSafeInteger(v_lCashListItemID), v_bApprove:=True, r_sReturnMessage:="") Then

                    bUpdatedLSItems = True

                End If


                ' if unable to approve item log it...
                If Not bUpdatedLSItems Then

                    ' Log Error.
                    Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                    oDict.Add("v_lCashListItemID", v_lCashListItemID)
                    gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to approve loss schedule items", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)

                    ' log message failed to update loss schedule items
                    result = gPMConstants.PMEReturnCode.PMFalse

                End If
            Else

                ' Log Error.
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lCashListItemID", v_lCashListItemID)
                gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to create bCLMLossSchedule.Business", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)

                result = gPMConstants.PMEReturnCode.PMFalse

            End If

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lCashListItemID", v_lCashListItemID)
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
            '******************************


        Finally
            ' destroy object reference
        End Try



        Return result


        Return result
    End Function




    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: getPartyCntFromAccountID
    ' PURPOSE: gets party Count from AccountID
    ' AUTHOR: steve watton
    ' DATE: 28/01/2003
    ' RETURNS: PMTrue for success
    ' ---------------------------------------------------------------------------

    Public Function GetPartyCntFromAccountID(ByVal v_lAccountID As Integer, ByRef r_lPartyCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            ' Add usergroupcode as an OUTPUT param
            m_lReturn = m_oDatabase.Parameters.Add(sName:="accountid", vValue:=CStr(v_lAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPartyCntFromAccountIDSQL, sSQLName:=ACGetPartyCntFromAccountIDName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            r_lPartyCnt = CInt(vResultArray(0, 0))

            Return result

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPartyCntFromAccountID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartyCntFromAccountID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: ProcessSalvageAllocation
    ' PURPOSE: calls allocation routine on salvageReceipt component
    ' AUTHOR: jeremy formby
    ' DATE: 18-3-2003
    ' RETURNS: PMTrue for success
    '
    ' ---------------------------------------------------------------------------

    Public Function ProcessSalvageAllocation(ByVal lCashlistitemID As Integer, ByVal lTransdetailId As Integer, ByVal lAccountId As Integer) As Integer
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oSalvage.ProcessSalvageAllocation(ToSafeInteger(lCashlistitemID), ToSafeInteger(lTransdetailId), ToSafeInteger(lAccountId))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMError
            End If

            Return result

        Catch excep As System.Exception



            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessSalvageAllocation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessSalvageAllocation", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetLetterDetailsForInstalment
    '
    ' Description: Gets information required for
    '              printing leeters, for instalment payment
    '              in this case there is no transaction id in the cashlistitem
    '
    ' PSL 14/04/2003
    ' ***************************************************************** '
    Public Function GetLetterDetailsForInstalment(ByVal lCashlistitemID As Integer, ByRef lPartyCnt As Integer, ByRef sShortName As String, ByRef sDocumentRef As String, Optional ByRef lInsuranceFileCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="CashListItem_ID", vValue:=CStr(lCashlistitemID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                ' Execute SQL Statement
                m_lReturn = .SQLSelect(sSQL:=ACGetInstalmentLetterDetailsSQL, sSQLName:=ACGetInstalmentLetterDetailsName, bStoredProcedure:=ACGetInstalmentLetterDetailsStored, lNumberRecords:=0, vResultArray:=vResultArray)

            End With
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            lPartyCnt = ToSafeInteger(vResultArray(0, 0))

            sShortName = ToSafeString(vResultArray(1, 0))

            sDocumentRef = ToSafeString(vResultArray(2, 0))

            lInsuranceFileCnt = ToSafeInteger(vResultArray(3, 0))

            Return m_lReturn

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLetterDetailsForInstalment Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLetterDetailsForInstalment", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetReceiptTypeCode
    '
    ' Description: Gets the type code for ReceiptTypeID
    '
    ' PSL 14/04/2003
    ' ***************************************************************** '
    Public Function GetReceiptTypeCode(ByVal lReceiptTypeID As Integer, ByRef sReceiptTypeCode As String) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="cashlistitem_receipt_type_id", vValue:=CStr(lReceiptTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                ' Execute SQL Statement
                m_lReturn = .SQLSelect(sSQL:=ACGetReceiptTypeCodeSQL, sSQLName:=ACGetReceiptTypeCodeName, bStoredProcedure:=ACGetReceiptTypeCodeStored, lNumberRecords:=0, vResultArray:=vResultArray)

            End With
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            sReceiptTypeCode = CStr(vResultArray(0, 0))


            Return m_lReturn

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetReceiptTypeCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReceiptTypeCode", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetLetterPrinted
    '
    ' Description: Set Letter column to zero so that the letter doesb't
    ' print each time you go into that cash list
    '
    ' PSL 14/04/2003
    ' ***************************************************************** '
    Public Function SetLetterPrinted(ByVal lCashlistitemID As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="cashlistitem_id", vValue:=CStr(lCashlistitemID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACSetLetterPrintedSQL, sSQLName:=ACSetLetterPrintedName, bStoredProcedure:=ACSetLetterPrintedStored)

            End With
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return m_lReturn

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetLetterPrinted Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetLetterPrinted", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdStatusOfReversedInstalment
    '
    ' Description: Update status of reversed Instalment
    '
    ' Author : Kevin Grandison
    '
    ' Date : 11/09/2003
    '
    ' ***************************************************************** '
    Public Function UpdStatusOfReversedInstalment(ByVal lCashlistitemID As Integer, ByVal sReverseCode As String, ByRef r_sReverseReason As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="cashlistitem_id", vValue:=CStr(lCashlistitemID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Parameters.Add(sName:="reverse_code", vValue:=sReverseCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Parameters.Add(sName:="reverse_reason", vValue:="", iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACUpdStatusOfReversedInstalmentSQL, sSQLName:=ACUpdStatusOfReversedInstalmentName, bStoredProcedure:=True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'DD 16/09/2003: Did the item fail or just get manually reverse because of
                'a user error
                r_sReverseReason = gPMFunctions.NullToString(.Parameters.Item("reverse_reason").Value)
            End With


            Return m_lReturn

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdStatusOfReversedInstalment Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdStatusOfReversedInstalment", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    ' Name: GetInstalmentTransdetailIDs
    '
    ' Description: gets the instalment transdetail ids for a given receipt
    '
    ' SW 30/04/2003
    ' ***************************************************************** '
    Private Function GetInstalmentTransdetailIDs(ByVal v_lCashListItemID As Integer, ByRef r_vTransDetails(,) As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            .Parameters.Clear()

            m_lReturn = .Parameters.Add(sName:="cashlistitemid", vValue:=CStr(v_lCashListItemID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = .SQLSelect(sSQL:=ACGetInstalmentTransdetailIDsSQL, sSQLName:=ACGetInstalmentTransdetailIDsName, bStoredProcedure:=True, vResultArray:=r_vTransDetails)
        End With

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetReceiptTypeCodeAndTransDetailID
    '
    ' Description: gets the receipt type code and transdetail id of the cashlistitem
    '
    ' SW 30/04/2003
    ' ***************************************************************** '
    Public Function GetReceiptTypeCodeAndTransDetailID(ByVal v_lCashListItemID As Integer, ByRef r_vTransDetailID As Integer, ByRef r_sReceiptTypeCode As String, ByRef r_bIsInstalmentBased As Boolean) As Integer
        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Const kTransDetailID As Integer = 0
        Const kReceiptTypeCode As Integer = 1
        Const kReceiptTypeIsInstalmentBased As Integer = 2

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="cashlistitemid", vValue:=CStr(v_lCashListItemID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = .SQLSelect(sSQL:=ACGetReceiptTypeCodeAndTransDetailIDSQL, sSQLName:=ACGetReceiptTypeCodeAndTransDetailIDName, bStoredProcedure:=True, vResultArray:=vResultArray)

            End With

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If CStr(vResultArray(kTransDetailID, 0)) <> "" Then

                r_vTransDetailID = CInt(vResultArray(kTransDetailID, 0))
            Else
                r_vTransDetailID = 0
            End If


            r_sReceiptTypeCode = CStr(vResultArray(kReceiptTypeCode, 0))


            r_bIsInstalmentBased = gPMFunctions.ToSafeBoolean(CStr(vResultArray(kReceiptTypeIsInstalmentBased, 0)), False)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetReceiptTypeCodeAndTransDetailID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReceiptTypeCodeAndTransDetailID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetPaymentTypeCodeAndTransDetailID
    '
    ' Description: gets the payment type code and transdetail id of the cashlistitem
    '
    ' DC 101003 PN7393
    ' ***************************************************************** '
    Public Function GetPaymentTypeCodeAndTransDetailID(ByVal v_lCashListItemID As Integer, ByRef r_vTransDetailID As Integer, ByRef r_sPaymentTypeCode As String) As Integer
        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Const kTransDetailID As Integer = 0
        Const kPaymentTypeCode As Integer = 1

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="cashlistitemid", vValue:=CStr(v_lCashListItemID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = .SQLSelect(sSQL:=ACGetPaymentTypeCodeAndTransDetailIDSQL, sSQLName:=ACGetPaymentTypeCodeAndTransDetailIDName, bStoredProcedure:=True, vResultArray:=vResultArray)
            End With

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If CStr(vResultArray(kTransDetailID, 0)) <> "" Then

                r_vTransDetailID = CInt(vResultArray(kTransDetailID, 0))
            Else
                r_vTransDetailID = 0
            End If


            r_sPaymentTypeCode = CStr(vResultArray(kPaymentTypeCode, 0))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPaymentTypeCodeAndTransDetailID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPaymentTypeCodeAndTransDetailID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    'Steve Watton
    '28-10-2002
    'ReverseReceipt
    'Function to process a reversal request on a given receipt

    Public Function ReverseReceipt(ByVal v_lCashListItemID As Integer, ByRef r_sFailureReason As Object, ByVal v_vCashListDrawerID As Object, ByVal sReverseCode As String) As Integer

        ' KG 08/08/03 - CQ1030 - Branch/SubBranch.Pass CashListDrawerID

        Dim result As Integer = 0
        Dim oReversal As Object = Nothing
        Dim sReceiptTypeCode As String = ""
        Dim lTransdetailId As Integer
        Dim vTransDetails(,) As Object = Nothing
        'DD 16/09/2003
        Dim sCCValue As String = ""
        Dim oCreditControl As Object = Nothing
        Dim bReceiptTypeIsInstalmentBased As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If gPMComponentServices.CreateBusinessObject(r_oObject:=oReversal, v_sClassName:="bACTDocumentReversal.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMError
            End If

            m_lReturn = CType(GetReceiptTypeCodeAndTransDetailID(v_lCashListItemID:=v_lCashListItemID, r_vTransDetailID:=lTransdetailId, r_sReceiptTypeCode:=sReceiptTypeCode, r_bIsInstalmentBased:=bReceiptTypeIsInstalmentBased), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                oReversal.Dispose()
                oReversal = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'if we are dealing with instalments then we need to look up the approriate transdetails
            If bReceiptTypeIsInstalmentBased Then

                m_lReturn = CType(GetInstalmentTransdetailIDs(v_lCashListItemID:=v_lCashListItemID, r_vTransDetails:=vTransDetails), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Informations.IsArray(vTransDetails) Then

                    oReversal.Dispose()
                    oReversal = Nothing
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' KG 11/09/03
                ' CQ 2482 - Update status of reversed Instalment
                m_lReturn = CType(UpdStatusOfReversedInstalment(v_lCashListItemID, sReverseCode, r_sFailureReason), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    oReversal.Dispose()
                    oReversal = Nothing
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Update the Status of Instalment in ReverseReceipt function", vApp:=ACApp, vClass:=ACClass, vMethod:="ReverseReceipt", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                'Determine whether credit control is switched on
                m_lReturn = CType(bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=4, r_sOptionValue:=sCCValue, v_iSourceID:=1), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If sCCValue = "1" And r_sFailureReason <> "" Then
                    m_lReturn = CType(gPMComponentServices.CreateBusinessObject(r_oObject:=oCreditControl, v_sClassName:="bACTCreditControlItem.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

                'loop through the instalment transdetails reversing each one individually

                For lCount As Integer = 0 To vTransDetails.GetUpperBound(1)


                    oReversal.TransdetailID = CInt(vTransDetails(0, lCount))

                    'reset any other variables that could be left from the last time

                    oReversal.DocumentId = 0


                    m_lReturn = oReversal.Start(r_sFailureReason:=r_sFailureReason, v_bDisableTransactions:=True, v_vCashListDrawerID:=v_vCashListDrawerID)
                    ' KG 08/08/03 - CQ1030 - Branch/SubBranch.Pass CashListDrawerID

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        'm_lReturn = oReversal.RollbackTrans

                        oReversal.Dispose()
                        oReversal = Nothing
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'DD 16/09/2003: Add the items to Credit Control
                    'We only do this if Credit Control is enabled and the CCI
                    'failed due to an actual bank rejection rather than a user's
                    'correction

                    If sCCValue = "1" And r_sFailureReason <> "" Then



                        m_lReturn = oCreditControl.AddInstalment(v_lPFInstalmentsID:=CInt(vTransDetails(1, lCount)), v_sReason:=r_sFailureReason, v_cAmount:=CDec(vTransDetails(2, lCount)))
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                Next

                'Shutdown credit control
                If sCCValue = "1" And r_sFailureReason <> "" Then

                    oCreditControl.Dispose()
                    oCreditControl = Nothing
                End If
            Else
                'reverse the receipt

                oReversal.TransdetailID = lTransdetailId


                m_lReturn = oReversal.Start(r_sFailureReason:=r_sFailureReason, v_vCashListDrawerID:=v_vCashListDrawerID)
                ' KG 08/08/03 - CQ1030 - Branch/SubBranch.Pass CashListDrawerID

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    oReversal.Dispose()
                    oReversal = Nothing
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


            End If


            oReversal.Dispose()
            oReversal = Nothing

            Return result

        Catch excep As System.Exception



            ''    ' Error Section.
            ''    If bTransActive = True Then
            ''        oReversal.RollbackTrans
            ''    End If

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the ReverseReceipt function", vApp:=ACApp, vClass:=ACClass, vMethod:="ReverseReceipt", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: CreateCashListItemRecovery
    ' PURPOSE: creates cashlistitem Recovery records
    ' AUTHOR: steve watton
    ' DATE: 23-10-2002
    ' RETURNS: PMTrue for success
    '
    ' THIS NEEDS TO BE IMPLEMENTED WHEN THE CLIENT PROJECT HAS BEEN FINISHED
    ' ---------------------------------------------------------------------------

    Private Function CreateCashlistItemCLMUSRecovery(ByVal v_vCLMUSRecoveryDetails As Object, ByVal v_vCLMRVRecoveryDetails As Object, ByVal v_lCashListItemID As Integer, ByVal v_lAccountID As Integer) As Integer

        Dim result As Integer = 0
        Dim lPartyCnt As Integer



        'fist find the perty count from the account_ID
        m_lReturn = CType(GetPartyCntFromAccountID(v_lAccountID, lPartyCnt), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or lPartyCnt = 0 Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'create the Recovery business object

        If m_oRecovery Is Nothing Then
            If gPMComponentServices.CreateBusinessObject(r_oObject:=m_oRecovery, v_sClassName:="bCLMDebtAllocate.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        m_lReturn = m_oRecovery.ConfirmReceipt(r_vUnsavedRBIItems:=v_vCLMUSRecoveryDetails, r_vReversedRBIItems:=v_vCLMRVRecoveryDetails, v_lCashListItemID:=ToSafeInteger(v_lCashListItemID), v_sCustomerShortName:="", v_bPayByInstalments:=False) '???
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Return gPMConstants.PMEReturnCode.PMTrue

    End Function

    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: DeleteCashListItemSalvage
    ' PURPOSE: Deletes cashlistitem Salvage records
    ' AUTHOR: steve watton
    ' DATE: 23-10-2002
    ' RETURNS: PMTrue for success
    '
    ' THIS NEEDS TO BE IMPLEMENTED WHEN THE CLIENT PROJECT HAS BEEN FINISHED
    ' ---------------------------------------------------------------------------

    Private Function DeleteCashlistItemCLMUSRecovery(ByVal v_lCashListItemID As Integer) As Integer




        Return gPMConstants.PMEReturnCode.PMTrue

    End Function





    ' ***************************************************************** '
    ' Name: UnLockSalvageParty
    ' Description:
    ' History: 11/9/2003 JMF - Created.
    ' ***************************************************************** '
    Public Function UnLockSalvageParty(ByVal v_lPartyId As Integer) As Integer

        Dim result As Integer = 0
        Dim oPMLock As bpmlock.User
        Dim sCurrentlyLocked As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            oPMLock = New bpmlock.User
            m_lReturn = CType(oPMLock.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(sUsername:=m_sUsername, sMsg:="Failed to get PMLock", vMethod:="UnLockSalvageParty", iType:=gPMConstants.PMELogLevel.PMLogOnError, vApp:=ACApp, vClass:=ACClass, vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If


            m_lReturn = oPMLock.UnLockKey(sKeyName:="party_id", vKeyValue:=v_lPartyId, iUserID:=m_iUserID)


            oPMLock = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnLockSalvageParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnLockSalvageParty", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: DeclineClaimPayment
    '
    ' Parameters: N/A
    '
    ' Description: approves the loss schedule items related to the
    '               specified cashlistitem
    '
    ' History:
    '           Created : MEvans : 19-06-2003 : CQ 1745
    ' ---------------------------------------------------------------------------
    Function DeclineClaimPayment(ByVal v_lCashListItemID As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "DeclineClaimPayment"

        Dim oLossSched As Object = Nothing
        Dim bUpdatedLSItems As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the claims loss schedule business object

            If gPMComponentServices.CreateBusinessObject(r_oObject:=oLossSched, v_sClassName:="bCLMLossSchedule.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase) = gPMConstants.PMEReturnCode.PMTrue Then

                ' approve the loss schedule items relating to the cashlistitem...

                If oLossSched.ApproveLossScheduleItemPayments(v_lCashListItemID:=ToSafeInteger(v_lCashListItemID), v_bApprove:=False, r_sReturnMessage:="") Then

                    bUpdatedLSItems = True

                End If


                ' if unable to approve item log it...
                If Not bUpdatedLSItems Then

                    ' Log Error.
                    Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                    oDict.Add("v_lCashListItemID", v_lCashListItemID)
                    gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to decline loss schedule items", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)

                    ' log message failed to update loss schedule items
                    result = gPMConstants.PMEReturnCode.PMFalse

                End If
            Else

                ' Log Error.
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lCashListItemID", v_lCashListItemID)
                gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to create bCLMLossSchedule.Business", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)

                result = gPMConstants.PMEReturnCode.PMFalse

            End If

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lCashListItemID", v_lCashListItemID)
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
            '******************************


        Finally
            ' destroy object reference
        End Try



        Return result


        Return result
    End Function


    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: CreateBatchRecord
    ' PURPOSE: Creates a new record in the batch table.
    ' Taken from bACTFinanceSpoke.
    ' AUTHOR: Danny Davis
    ' DATE: 21/10/2003
    ' RETURNS: PMTrue for success
    ' ---------------------------------------------------------------------------

    Public Function CreateBatchRecord(Optional ByRef r_lBatchID As Integer = 0, Optional ByVal v_lBatchStatusID As Integer = 0, Optional ByVal v_lCompanyID As Integer = 0, Optional ByVal v_lUserId As Integer = 0, Optional ByVal v_sBatchRef As String = "", Optional ByVal v_dtCreatedDate As Date = #12/30/1899#, Optional ByVal v_dtAuthorisedDate As Date = #12/30/1899#, Optional ByVal v_dtAccountingDate As Date = #12/30/1899#, Optional ByVal v_sComment As String = "", Optional ByVal v_lBatchTypeID As Integer = 0, Optional ByVal v_lBatchSourceID As Integer = 0, Optional ByVal v_sXML As String = "", Optional ByVal v_dtExportDate As Date = #12/30/1899#, Optional ByVal v_dtReExportDate As Date = #12/30/1899#, Optional ByVal v_lMediaTypeID As Integer = 0, Optional ByVal v_cTotalAmount As Decimal = 0, Optional ByVal v_lTotalTransactions As Integer = 0, Optional ByVal v_dtImportedDate As Date = #12/30/1899#, Optional ByVal v_cRejectAmount As Decimal = 0, Optional ByVal v_lRejectTransactions As Integer = 0, Optional ByVal v_dtClosedDate As Date = #12/30/1899#, Optional ByVal v_sInterfaceCode As String = "", Optional ByVal v_iAutoClose As Integer = 0) As Integer

        Dim result As Integer = 0
        Const ACMethod As String = "CreateBatchRecord"
        Try


            result = gPMConstants.PMEReturnCode.PMFalse

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add(sName:="batch_id", vValue:=CStr(r_lBatchID), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


            If m_oDatabase.Parameters.Add(sName:="batchstatus_id", vValue:=If(v_lBatchStatusID = 0, DBNull.Value, CStr(v_lBatchStatusID)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


            If m_oDatabase.Parameters.Add(sName:="company_id", vValue:=If(v_lCompanyID = 0, DBNull.Value, CStr(v_lCompanyID)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


            If m_oDatabase.Parameters.Add(sName:="user_id", vValue:=If(v_lUserId = 0, DBNull.Value, CStr(v_lUserId)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            If m_oDatabase.Parameters.Add(sName:="batch_ref", vValue:=v_sBatchRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


            'developer guide no. 40
            If m_oDatabase.Parameters.Add(sName:="created_date", vValue:=If(v_dtCreatedDate = CDate("00:00:00"), DBNull.Value, v_dtCreatedDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


            'developer guide no. 40
            If m_oDatabase.Parameters.Add(sName:="authorised_date", vValue:=If(v_dtAuthorisedDate = CDate("00:00:00"), DBNull.Value, v_dtAuthorisedDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


            'developer guide no. 40
            If m_oDatabase.Parameters.Add(sName:="accounting_date", vValue:=If(v_dtAccountingDate = CDate("00:00:00"), DBNull.Value, v_dtAccountingDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


            If m_oDatabase.Parameters.Add(sName:="comment", vValue:=If(v_sComment = "", DBNull.Value, v_sComment), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


            If m_oDatabase.Parameters.Add(sName:="batch_type_id", vValue:=If(v_lBatchTypeID = 0, DBNull.Value, CStr(v_lBatchTypeID)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


            If m_oDatabase.Parameters.Add(sName:="batch_source_id", vValue:=If(v_lBatchSourceID = 0, DBNull.Value, CStr(v_lBatchSourceID)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


            If m_oDatabase.Parameters.Add(sName:="xml_object", vValue:=If(v_sXML = "", DBNull.Value, v_sXML), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


            'developer guide no. 40
            If m_oDatabase.Parameters.Add(sName:="exportdate", vValue:=If(v_dtExportDate = CDate("00:00:00"), DBNull.Value, v_dtExportDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


            'developer guide no. 40
            If m_oDatabase.Parameters.Add(sName:="reexportdate", vValue:=If(v_dtReExportDate = CDate("00:00:00"), DBNull.Value, v_dtReExportDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            'sw 14-04-2003 Changed this from mediavalidationid to mediaid.
            'table and sp changed accordingly

            If m_oDatabase.Parameters.Add(sName:="mediatypeid", vValue:=If(v_lMediaTypeID = 0, DBNull.Value, CStr(v_lMediaTypeID)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


            If m_oDatabase.Parameters.Add(sName:="totalamount", vValue:=If(Val(CStr(v_cTotalAmount)) = 0, DBNull.Value, CStr(v_cTotalAmount)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


            If m_oDatabase.Parameters.Add(sName:="totaltransactions", vValue:=If(v_lTotalTransactions = 0, DBNull.Value, CStr(v_lTotalTransactions)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


            'developer guide no. 40
            If m_oDatabase.Parameters.Add(sName:="importeddate", vValue:=If(v_dtImportedDate = CDate("00:00:00"), DBNull.Value, v_dtImportedDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


            If m_oDatabase.Parameters.Add(sName:="rejectamount", vValue:=If(Val(CStr(v_cRejectAmount)) = 0, DBNull.Value, CStr(v_cRejectAmount)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


            If m_oDatabase.Parameters.Add(sName:="rejecttransactions", vValue:=If(v_lRejectTransactions = 0, DBNull.Value, CStr(v_lRejectTransactions)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


            'developer guide no. 40
            If m_oDatabase.Parameters.Add(sName:="closeddate", vValue:=If(v_dtClosedDate = CDate("00:00:00"), DBNull.Value, v_dtClosedDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


            If m_oDatabase.Parameters.Add(sName:="interfacecode", vValue:=If(v_sInterfaceCode = "", DBNull.Value, v_sInterfaceCode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            If m_oDatabase.Parameters.Add(sName:="autoclose", vValue:=CStr(v_iAutoClose), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            ' Execute SQL Statement
            If m_oDatabase.SQLAction(sSQL:=ACCreateBatchRecordSQL, sSQLName:=ACCreateBatchRecordName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            r_lBatchID = gPMFunctions.NullToLong(m_oDatabase.Parameters.Item("batch_id").Value)

            result = gPMConstants.PMEReturnCode.PMTrue

            ' GoTo Finally_Renamed

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------
            'Resume
            Return result
        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Constants.vbObjectError
                    ' Log internal failure.
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Informations.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:=Informations.Err().Source, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' GoTo Finally_Renamed

                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACMethod & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError

                    ' GoTo Finally_Renamed

            End Select
            Return result
        End Try

    End Function


    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: SelectBatchRecord
    ' PURPOSE: Selects a Batch Record from the table using the Reference.
    ' AUTHOR: Danny Davis
    ' DATE: 21/10/2003
    ' RETURNS: PMTrue for success
    ' ---------------------------------------------------------------------------

    Public Function SelectBatchRecord(ByVal sBatchRef As String, ByRef r_lBatchID As Integer) As Integer

        Dim result As Integer = 0
        Const ACMethod As String = "SelectBatchRecord"



        result = gPMConstants.PMEReturnCode.PMFalse

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        If m_oDatabase.Parameters.Add(sName:="batch_ref", vValue:=sBatchRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


        If m_oDatabase.Parameters.Add(sName:="batch_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

        ' Execute SQL Statement
        If m_oDatabase.SQLAction(sSQL:=ACSelectBatchRecordSQL, sSQLName:=ACSelectBatchRecordName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

        r_lBatchID = gPMFunctions.NullToLong(m_oDatabase.Parameters.Item("batch_id").Value)

        result = gPMConstants.PMEReturnCode.PMTrue

        GoTo Finally_Renamed

        '----------------------------------------------------------------------------------------
        'Only for Debugging, the code will never execute this line
        '----------------------------------------------------------------------------------------
        ' Resume


        Select Case Informations.Err().Number
            Case Constants.vbObjectError
                ' Log internal failure.
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Informations.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:=Informations.Err().Source)

                result = gPMConstants.PMEReturnCode.PMFalse

                GoTo Finally_Renamed

            Case Else
                ' Log Error.
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACMethod & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                result = gPMConstants.PMEReturnCode.PMError

                GoTo Finally_Renamed

        End Select

Finally_Renamed:

        Return result

    End Function


    ' ***************************************************************** '
    '
    ' Name: GetMediaTypeIssuer
    '
    ' Description: Gets data required from the MediaType_Issuer combo.
    '
    ' ***************************************************************** '
    Public Function GetMediaTypeIssuer(ByVal lMediaTypeID As Integer, ByVal iIsClaimPayment As Integer, ByRef r_vOutputDetails(,) As Object) As Integer

        Dim result As Integer = 0
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="mediatype_id", vValue:=CStr(lMediaTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.Parameters.Add", "m_lReturn = " & m_lReturn & ", sCallingAppName = " & ACApp & ", mediatype_id:=" & CStr(lMediaTypeID))
            End If
            'Developer Guide No 98
            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_claim_type_payment", vValue:=iIsClaimPayment, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.Parameters.Add", "m_lReturn = " & m_lReturn & ", sCallingAppName = " & ACApp & ", is_claim_type_payment:=" & CStr(iIsClaimPayment))
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelMediaTypeIssuerSQL, sSQLName:=ACSelMediaTypeIssuerName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vOutputDetails)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "m_lReturn = " & m_lReturn & ", sCallingAppName = " & ACApp & ", sSQL = " & ACSelMediaTypeIssuerSQL)
            End If


        Catch ex As Exception
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetMediaTypeIssuer Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMediaTypeIssuer", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)
            result = gPMConstants.PMEReturnCode.PMFalse
        Finally
            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetClaimPaymentAccountsDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 24-01-2006 : Cheque Production Workflow (ATD16)
    ' ***************************************************************** '
    Public Function GetClaimPaymentAccountsDetails(ByVal v_lClaimPaymentId As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimPaymentAccountsDetails"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="claim_payment_id", v_vValue:=v_lClaimPaymentId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetClaimPaymentAccountsDetailsSQL, sSQLName:=kGetClaimPaymentAccountsDetailsName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kGetClaimPaymentAccountsDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
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

            lReturn = m_oDatabase.Parameters.Add(sName:=v_sName, vValue:=v_vValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iType)

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

    ' ***************************************************************** '
    ' Name: GetReceiptTypeDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 07-02-2006 : Receipt Type Maintenance
    ' ***************************************************************** '
    Public Function GetReceiptTypeDetails(ByVal v_lReceiptTypeId As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetReceiptTypeDetails"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            lReturn = CType(AddInputParameter(v_sName:="cashlistitem_receipt_type_id", v_vValue:=v_lReceiptTypeId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetReceiptTypeDetailsSQL, sSQLName:=kGetReceiptTypeDetailsName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kGetReceiptTypeDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
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
    ' Name:                 AddCashListItemClaimLink
    '
    ' Description:          Add a record in cashlistitem_claim_link
    '
    ' ***************************************************************** '

    Public Function AddCashListItemClaimLink(ByVal v_lClaim_payment_Id As Integer, ByVal v_lClaim_receipt_id As Integer, ByVal v_lCashListItem_id As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AddCashListItemClaimLink"
        Dim lReturn As gPMConstants.PMEReturnCode
        Try



            result = gPMConstants.PMEReturnCode.PMTrue
            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            If v_lClaim_payment_Id > 0 Then
                lReturn = CType(AddInputParameter(v_sName:="claim_payment_id", v_vValue:=v_lClaim_payment_Id, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            Else

                lReturn = CType(AddInputParameter(v_sName:="claim_payment_id", v_vValue:=DBNull.Value, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            End If
            If v_lClaim_receipt_id > 0 Then
                lReturn = CType(AddInputParameter(v_sName:="claim_receipt_id", v_vValue:=v_lClaim_receipt_id, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            Else

                lReturn = CType(AddInputParameter(v_sName:="claim_receipt_id", v_vValue:=DBNull.Value, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            End If
            lReturn = CType(AddInputParameter(v_sName:="cashlistitem_id", v_vValue:=v_lCashListItem_id, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            lReturn = m_oDatabase.SQLAction(sSQL:=kAddCashListItemClaimLinkSQL, sSQLName:=kAddCashListItemClaimLinkName, bStoredProcedure:=True)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kAddCashListItemClaimLinkSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
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
    ' Name:          GetBranchBaseCurrency
    '
    ' Task:          Account Function and CCY Cash Allocation
    '
    ' ***************************************************************** '

    Public Function GetBranchBaseCurrency(ByVal v_lSourceID As Integer, ByRef v_lBaseCurrencyID As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetBranchBaseCurrency"
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vResultArray(,) As Object = Nothing
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            v_lBaseCurrencyID = 0

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            lReturn = CType(AddInputParameter(v_sName:="Source_id", v_vValue:=v_lSourceID, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetSourceBaseCurrencySQL, sSQLName:=ACGetSourceBaseCurrencyName, bStoredProcedure:=ACGetSourceBaseCurrencyStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray, bKeepNulls:=False)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACGetSourceBaseCurrencyName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Informations.IsArray(vResultArray) Then
                v_lBaseCurrencyID = gPMFunctions.ToSafeLong(vResultArray(0, 0))
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

    Public Function UpdateCLIPaymentStatus(ByVal v_lCashListItem_id As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateCLIPaymentStatus"
        Dim lReturn As gPMConstants.PMEReturnCode
        Try



            result = gPMConstants.PMEReturnCode.PMTrue
            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            lReturn = CType(AddInputParameter(v_sName:="cashlistitem_id", v_vValue:=v_lCashListItem_id, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            lReturn = m_oDatabase.SQLAction(sSQL:=kUpdateCashListItemStatusPendingSQL, sSQLName:=kUpdateCashListItemStatusPendingName, bStoredProcedure:=True)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kUpdateCashListItemStatusPendingSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
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


    'Rahul

    Public Function GetCollectionDateOverrideAuthority(ByVal v_lUserId As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetCollectionDateOverrideAuthority"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            lReturn = CType(AddInputParameter(v_sName:="user_id", v_vValue:=v_lUserId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetCollectionDateOverrideAuthoritySQL, sSQLName:=ACGetCollectionDateOverrideAuthorityName, bStoredProcedure:=ACGetCollectionDateOverrideAuthorityStored, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACGetCollectionDateOverrideAuthoritySQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
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

    'Start - Sankar - PN 55288
    Public Function GetCashListReceiptTypeFromID(ByRef lCashListReceiptTypeId As Integer, ByRef r_sCashListReceiptType As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetCashListReceiptTypeFromID"
        Dim vArray(,) As Object = Nothing
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "table_name", "CashListItem_Receipt_Type", gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "field_to_return_name", "code", gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "field_to_validate_name", "cashlistitem_receipt_type_id", gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            'developer guide no. 252
            bPMAddParameter.AddParameterLite(m_oDatabase, "field_to_validate_value", lCashListReceiptTypeId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetCashListReceiptTypeFromIDSQL, sSQLName:=ACGetCashListReceiptTypeFromIDName, bStoredProcedure:=True, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACGetCashListReceiptTypeFromIDSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            ElseIf Informations.IsArray(vArray) Then

                r_sCashListReceiptType = CStr(vArray(0, 0))
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
    'Start - Sankar - PN 55288

    ' ************************************************************************* '
    ' Name: GetDocumentFromTransdetail
    '
    ' Parameters: TransdetailId
    '
    ' Description: Get document details from TransdetailId
    '
    ' History:
    '           Created : Gautam Poddar - 21 Oct 2008
    ' ************************************************************************* '

    Public Function GetDocumentFromTransdetail(ByVal v_lTransdetailId As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetDocumentFromTransdetail"
        Dim lReturn As gPMConstants.PMEReturnCode
        Try



            result = gPMConstants.PMEReturnCode.PMTrue
            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            lReturn = CType(AddInputParameter(v_sName:="transdetail_id", v_vValue:=v_lTransdetailId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetDocumentFromTransdetailSQL, sSQLName:=ACGetDocumentFromTransdetailName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACGetDocumentFromTransdetailSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
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
    ' Name: GetPolicyDetailsFromClaimPayment
    '
    ' Parameters: v_lClaimPaymentId
    '
    ' Description:
    '
    ' History:
    '           Created : Gautam Poddar : 09-Feb-2009
    ' ***************************************************************** '
    Public Function GetPolicyDetailsFromClaimPayment(ByVal v_lClaimPaymentId As Integer, ByRef r_lInsuranceFileCnt As Integer, ByRef r_sDocumentRef As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPolicyDetailsFromClaimPayment"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vResultArray(,) As Object = Nothing

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            lReturn = CType(AddInputParameter(v_sName:="claim_payment_id", v_vValue:=v_lClaimPaymentId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPolicyDetailsFromClaimPaymentSQL, sSQLName:=ACGetPolicyDetailsFromClaimPaymentName, bStoredProcedure:=True, vResultArray:=vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACGetPolicyDetailsFromClaimPaymentSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Informations.IsArray(vResultArray) Then
                r_lInsuranceFileCnt = gPMFunctions.ToSafeLong(vResultArray(0, 0))
                r_sDocumentRef = gPMFunctions.ToSafeString(vResultArray(1, 0))
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


    Public Function GetTransDetailsFromBatch(ByVal v_lBatchID As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetTransDetailsFromBatch"
        result = gPMConstants.PMEReturnCode.PMTrue
        Try
            Dim lReturn As gPMConstants.PMEReturnCode

            bPMAddParameter.AddParameterLite(m_oDatabase, "Batch_set_id", v_lBatchID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPMNavXMBatchTransactionDetailSQL, sSQLName:=ACGetPMNavXMBatchTransactionDetailName, bStoredProcedure:=True, vResultArray:=r_vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to execute spu_Get_PMNav_Batch_Transaction_Details", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally


        End Try
        Return result
    End Function


    Public Function UpdateWriteOffDocumentRef(ByVal v_lOldDocumentId As Integer, ByVal v_lNewDocumentId As Integer) As Integer


        Dim result As Integer = 0
        Const kMethodName As String = "UpdateWriteOffDocumentRef"
        result = gPMConstants.PMEReturnCode.PMTrue
        Try
            Dim lReturn As gPMConstants.PMEReturnCode

            bPMAddParameter.AddParameterLite(m_oDatabase, "iOldDocumentId", v_lOldDocumentId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "iNewDocumentID", v_lNewDocumentId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            lReturn = m_oDatabase.SQLAction(sSQL:=ACTUpdateWriteOffDocumentSQL, sSQLName:=ACTUpdateWriteOffDocumentSQL, bStoredProcedure:=True)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to execute spu_Get_PMNav_Batch_Transaction_Details", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally


        End Try
        Return result
    End Function

    Public Function UpdateCashListForSplitReceipt(ByVal v_iCashListId As Integer, ByVal v_bStatus As Boolean) As Integer

        Const kMethodName As String = "UpdateCashListForSplitReceipt"
        Dim result As Integer = 0
        Dim lReturn As Integer = 0

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            bPMAddParameter.AddParameterLite(m_oDatabase, "CashList_Id", v_iCashListId, PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Status", v_bStatus, PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMBoolean)


            lReturn = m_oDatabase.SQLAction(sSQL:=kUpdaetCashListForSplitReceiptSQL, sSQLName:=kTUpdaetCashListForSplitReceiptName, bStoredProcedure:=kUpdateCashListForSplitReceiptStored)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to execute ACTUpdaetCashListForSplitReceiptSQL", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            End If

        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            'bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)

        End Try

        Return result

    End Function

    Public Function GetSchemeCurrency(ByVal lPremiumFinanceCnt As Integer, ByVal lPremiumFinanceVersion As Integer,
                                       ByRef r_iCurrencyID As Integer, ByRef r_XRate As Double) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetSchemeCurrency
        ' PURPOSE: Returns the Base Currency used by the Finance Scheme
        ' AUTHOR: Danny Davis
        ' DATE: 04 August 2004, 09:50 AM
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim vResultArray As Object(,) = Nothing
        Try

            ' On Error GoTo Catch_Renamed

            result = gPMConstants.PMEReturnCode.PMFalse

            With m_oDatabase

                .Parameters.Clear()
                m_lReturn = .Parameters.Add(sName:="pfprem_finance_cnt", vValue:=CStr(lPremiumFinanceCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

                m_lReturn = .Parameters.Add(sName:="pfprem_finance_version", vValue:=CStr(lPremiumFinanceVersion), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

                'Developer Guide No 39
                m_lReturn = .SQLSelect(sSQL:="spu_PFScheme_GetCurrency", sSQLName:="spu_PFScheme_GetCurrency", bStoredProcedure:=True, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

                r_XRate = 1

                If Informations.IsArray(vResultArray) Then
                    r_iCurrencyID = ToSafeInteger(vResultArray(5, 0))

                    If ToSafeInteger(vResultArray(2, 0)) = 1 Then
                        r_iCurrencyID = ToSafeInteger(vResultArray(0, 0))
                        r_XRate = ToSafeDouble(vResultArray(1, 0))
                    End If
                End If

            End With

            result = gPMConstants.PMEReturnCode.PMTrue

            'GoTo Finally_Renamed

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------
            'Resume
            Return result
        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSchemeCurrency", vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

                    'GoTo Finally_Renamed
            End Select

            Return result
        End Try
    End Function


    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub

    ''' <summary>
    ''' UpdateTransMatchCashListItemID
    ''' </summary>
    ''' <param name="nCashListItemID"></param>
    ''' <param name="nCashListTransDetailsID"></param>
    ''' <param name="sProcessType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateTransMatchCashListItemID(ByVal nCashListItemID As Integer,
                                                   ByVal nCashListTransDetailsID As Integer,
                                                   ByVal sProcessType As String) As Integer
        Dim nReturn As Integer = PMEReturnCode.PMTrue

        Try
            AddParameterLite(m_oDatabase,
                             "sProcessType", sProcessType,
                             PMEParameterDirection.PMParamInput,
                             PMEDataType.PMString, True)
            AddParameterLite(m_oDatabase,
                             "nCashListItemID", nCashListItemID,
                             PMEParameterDirection.PMParamInput,
                             PMEDataType.PMInteger)
            AddParameterLite(m_oDatabase,
                             "nBatchID", nCashListTransDetailsID,
                             PMEParameterDirection.PMParamInput,
                             PMEDataType.PMInteger)


            nReturn = m_oDatabase.SQLAction(sSQLName:=kUpdateTransMatchCashListItemIdName,
                                            sSQL:=kUpdateTransMatchCashListItemIdSQL,
                                            bStoredProcedure:=kUpdateTransMatchCashListItemIdStored)

            If nReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Failed to execute spu_ACT_Update_TransMatch_CashListID")
            End If

            Return nReturn
        Catch ex As Exception
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="UpdateTransMatchCashListItemID", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateTransMatchCashListItemID", vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)
            Return PMEReturnCode.PMError
        End Try
    End Function

    ''' <summary>
    ''' UpdateCashListBatchID
    ''' </summary>
    ''' <param name="nBatchID"></param>
    ''' <param name="nCashListId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateCashListBatchID(ByVal nBatchID As Integer,
                                          ByVal nCashListId As Integer) As Integer

        Dim nReturn As Integer = PMEReturnCode.PMTrue
        Try
            AddParameterLite(m_oDatabase, "nBatchID", nBatchID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, True)
            AddParameterLite(m_oDatabase, "nCashListID", nCashListId, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)

            nReturn = m_oDatabase.SQLAction(sSQLName:=kUpdateCashListBatchIDName,
                                            sSQL:=kUpdateCashListBatchIDSQL,
                                            bStoredProcedure:=kUpdateCashListBatchIDStored)

            If nReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Failed to execute spu_ACT_Update_TransMatch_CashListID")
            End If

            Return nReturn
        Catch ex As Exception
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="UpdateCashListBatchID", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateCashListBatchID", vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)
            Return PMEReturnCode.PMError
        End Try
    End Function

    ''' <summary>
    ''' GetCashListBatchID
    ''' </summary>
    ''' <param name="nCashListID"></param>
    ''' <param name="r_nBatchID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCashListBatchID(ByVal nCashListID As Integer,
                                       ByRef r_nBatchID As Integer) As Integer

        Dim oResultArray(,) As Object = Nothing
        Dim nReturn As Integer = PMEReturnCode.PMTrue
        Try
            AddParameterLite(m_oDatabase,
                             "CashList_ID", nCashListID,
                             PMEParameterDirection.PMParamInput,
                             PMEDataType.PMInteger, True)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=kGetCashListBatchIDSQL,
                                               sSQLName:=kGetCashListBatchIDName,
                                               bStoredProcedure:=kGetCashListBatchIDStored,
                                               lNumberRecords:=0,
                                               vResultArray:=oResultArray)


            If (m_lReturn = PMEReturnCode.PMNotFound) OrElse Not Informations.IsArray(oResultArray) Then
                Return PMEReturnCode.PMNotFound
            End If

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Failed to execute ACGetCashListBatchIDSQL")
            End If

            If oResultArray(0, 0) IsNot Nothing Then
                r_nBatchID = ToSafeInteger(oResultArray(0, 0))
            Else
                Throw New Exception("Failed to get BatchID")
            End If


            Return nReturn
        Catch ex As Exception
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="GetCashListBatchID", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCashListBatchID", vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)
            Return PMEReturnCode.PMError
        End Try
    End Function


    ''' <summary>
    ''' CheckInsurerPaymentRoadMap
    ''' </summary>
    ''' <param name="nCashListItemID"></param>
    ''' <param name="r_bIsInsurerPaymentRoadMap"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CheckInsurerPaymentRoadMap(ByVal nCashListItemID As Integer,
                                               ByRef r_bIsInsurerPaymentRoadMap As Boolean) As Integer


        Dim oResultArray As Object(,) = Nothing

        Try
            AddParameterLite(m_oDatabase,
                             "CashListitem_ID", nCashListItemID,
                             PMEParameterDirection.PMParamInput,
                             PMEDataType.PMInteger, True)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=kCheckInsurerPaymentRoadMapSQL,
                                              sSQLName:=kCheckInsurerPaymentRoadMapName,
                                              bStoredProcedure:=kCheckInsurerPaymentRoadMapStored,
                                              lNumberRecords:=0,
                                              vResultArray:=oResultArray)

            If (m_lReturn = PMEReturnCode.PMNotFound) OrElse Not Informations.IsArray(oResultArray) Then
                Return PMEReturnCode.PMNotFound
            End If

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Failed to execute CheckInsurerPaymentRoadMap")
            End If

            If CLng(oResultArray(0, 0)) > 0 Then
                r_bIsInsurerPaymentRoadMap = True
            Else
                r_bIsInsurerPaymentRoadMap = False
            End If

            Return PMEReturnCode.PMTrue
        Catch ex As Exception
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="CheckInsurerPaymentRoadMap", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckInsurerPaymentRoadMap", vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)
            Return PMEReturnCode.PMError

        End Try
    End Function

    ''' <summary>
    ''' GetandUpdateBatchTransDetailID
    ''' </summary>
    ''' <param name="nBatchID"></param>
    ''' <param name="nTransDetailID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetandUpdateBatchTransDetailID(ByVal nBatchID As Integer,
                                                   ByVal nTransDetailID As Integer) As Integer


        Try

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="nBatchID",
                                                   vValue:=nBatchID,
                                                   iDirection:=PMEParameterDirection.PMParamInput,
                                                   iDataType:=PMEDataType.PMInteger)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Failed to add parameter BatchID.")
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="nTransdetailID",
                                                    vValue:=nTransDetailID,
                                                    iDirection:=PMEParameterDirection.PMParamInput,
                                                    iDataType:=PMEDataType.PMInteger)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Failed to add parameter TransdetailID.")
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=kGetandUpdateBatchTransDetailIDSQL,
                                              sSQLName:=kGetandUpdateBatchTransDetailIDName,
                                              bStoredProcedure:=True)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception(kGetandUpdateBatchTransDetailIDSQL + " Failed.")
            End If

            Return PMEReturnCode.PMTrue
        Catch ex As Exception
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="GetandUpdateBatchTransDetailID", vApp:=ACApp, vClass:=ACClass, vMethod:="GetandUpdateBatchTransDetailID", vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)
            Return PMEReturnCode.PMError

        End Try

    End Function

    ''' <summary>
    ''' GetAllocationDetailIDs
    ''' </summary>
    ''' <param name="nTransDetailID"></param>
    ''' <param name="r_oAllocationIDS"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAllocationDetailIDs(ByVal nTransDetailID As Integer,
                                            ByRef r_oAllocationIDS As Object(,)) As Integer


        Try

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="TransDetailID",
                                       vValue:=nTransDetailID,
                                       iDirection:=PMEParameterDirection.PMParamInput,
                                       iDataType:=PMEDataType.PMInteger)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Failed to add parameter TransDetailID.")
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=kGetAllocationDetailIDsSQL,
                                              sSQLName:=kGetAllocationDetailIDsName,
                                              bStoredProcedure:=True,
                                              lNumberRecords:=0,
                                              vResultArray:=r_oAllocationIDS)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception(kGetAllocationDetailIDsSQL + " Failed.")
            End If
            Return PMEReturnCode.PMTrue
        Catch ex As Exception
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="GetAllocationDetailIDs", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllocationDetailIDs", vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)
            Return PMEReturnCode.PMError
        End Try

    End Function

    ''' <summary>
    ''' GetReinsurerAndRIPaymentRecoveriesDetail
    ''' </summary>
    ''' <param name="nAccountID"></param>
    ''' <param name="r_bIsReinsurer"></param>
    ''' <param name="r_bIsRIPaymentsAndRecovery"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetReinsurerAndRIPaymentRecoveriesDetail(ByVal nAccountID As Integer,
                                                             ByRef r_bIsReinsurer As Boolean,
                                                             ByRef r_bIsRIPaymentsAndRecovery As Boolean) As Integer

        Dim oResultArray As Object(,) = Nothing
        Try

            m_oDatabase.Parameters.Clear()
            AddParameterLite(m_oDatabase,
                             "nAccountID", nAccountID,
                             PMEParameterDirection.PMParamInput,
                             PMEDataType.PMInteger, True)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=kGetReinsurerAndRIPaymentRecoveriesDetailSQL,
                                              sSQLName:=kGetReinsurerAndRIPaymentRecoveriesDetailName,
                                              bStoredProcedure:=kGetReinsurerAndRIPaymentRecoveriesDetailStored,
                                              lNumberRecords:=gPMConstants.PMAllRecords,
                                              vResultArray:=oResultArray)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception(kGetReinsurerAndRIPaymentRecoveriesDetailSQL + " failed.")
            End If

            If Informations.IsArray(oResultArray) Then
                r_bIsReinsurer = ToSafeBoolean(oResultArray(0, 0))
                r_bIsRIPaymentsAndRecovery = ToSafeBoolean(oResultArray(1, 0))
            End If

            Return PMEReturnCode.PMTrue
        Catch ex As Exception
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="GetReinsurerAndRIPaymentRecoveriesDetail", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReinsurerAndRIPaymentRecoveriesDetail", vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)
            Return PMEReturnCode.PMFail
        End Try


    End Function

    ''' <summary>
    ''' GetTaxbandDetailForPaymentRecoveries
    ''' </summary>
    ''' <param name="r_oResultArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetTaxbandDetailForPaymentRecoveries(ByRef r_oResultArray(,) As Object) As Integer

        Dim nReturn As Integer
        Dim oResultArray As Object(,) = Nothing
        Try
            m_oDatabase.Parameters.Clear()

            nReturn = m_oDatabase.SQLSelect(sSQL:=kGetTaxbandDetailForPaymentRecoveriesSQL,
                                            sSQLName:=kGetTaxbandDetailForPaymentRecoveriesName,
                                            bStoredProcedure:=kGetTaxbandDetailForPaymentRecoveriesStored,
                                            lNumberRecords:=gPMConstants.PMAllRecords,
                                            vResultArray:=oResultArray)

            If nReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception(kGetTaxbandDetailForPaymentRecoveriesSQL + " failed.")
            End If

            If Informations.IsArray(oResultArray) Then
                r_oResultArray = oResultArray
            End If

            Return PMEReturnCode.PMTrue
        Catch ex As Exception
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="GetTaxbandDetailForPaymentRecoveries", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTaxbandDetailForPaymentRecoveries", vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)
            Return PMEReturnCode.PMFail
        End Try

    End Function

    ''' <summary>
    ''' GetCashListDetails
    ''' </summary>
    ''' <param name="nCashListId"></param>
    ''' <param name="r_oResultArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCashListDetails(ByVal nCashListId As Integer,
                                       ByRef r_oResultArray(,) As Object) As Integer

        Dim nReturn As Integer
        Try

            AddParameterLite(m_oDatabase, "cashlist_id", nCashListId, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, True)

            nReturn = m_oDatabase.SQLSelect(sSQL:=kGetCashListDetailsSQL,
                                            sSQLName:=kGetCashListDetailsName,
                                            bStoredProcedure:=kGetCashListDetailsStored,
                                            vResultArray:=r_oResultArray,
                                            lNumberRecords:=gPMConstants.PMAllRecords)

            If nReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception(kGetCashListDetailsSQL + " Failed.")
            End If
            Return nReturn
        Catch ex As Exception
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="GetCashListDetails", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCashListDetails", vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)
            Return PMEReturnCode.PMFail

        End Try

    End Function

    ''' <summary>
    ''' GetClaimPaymentDetailsByCashListItem
    ''' </summary>
    ''' <param name="nCashListItemId"></param>
    ''' <param name="r_oResultArray"></param>
    ''' <returns></returns>
    Public Function GetClaimPaymentDetailsByCashListItem(ByVal nCashListItemId As Integer,
                                       ByRef r_oResultArray(,) As Object) As Integer
        Dim nReturn As Integer
        Try

            AddParameterLite(m_oDatabase, "CashListItem_id", nCashListItemId, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, True)

            nReturn = m_oDatabase.SQLSelect(sSQL:=kGetClaimPaymentDetailsByCashListItemIdSQL,
                                            sSQLName:=kGetClaimPaymentDetailsByCashListItemIdName,
                                            bStoredProcedure:=True,
                                            vResultArray:=r_oResultArray,
                                            lNumberRecords:=gPMConstants.PMAllRecords)

            If nReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception(kGetClaimPaymentDetailsByCashListItemIdSQL + " Failed.")
            End If
            Return nReturn
        Catch ex As Exception
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="GetClaimPaymentDetailsByCashListItem", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimPaymentDetailsByCashListItem", vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)
            Return PMEReturnCode.PMFail

        End Try
    End Function

    ''' <summary>
    ''' CheckWriteOffReason list count in Database table
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CheckWriteOffReason(ByRef drResultArray As DataRow()) As Integer
        Dim nReturn As Integer = PMEReturnCode.PMTrue
        Dim dtResultArray As New DataTable
        Try
            m_oDatabase.Parameters.Clear()
            nReturn = m_oDatabase.ExecuteDataTable(sSQL:=kCheckWriteOffReasonSQL,
                                              sSQLName:=kCheckWriteOffReason,
                                              bStoredProcedure:=kCheckWriteOffReasonStored,
                                                          oRecordset:=dtResultArray)
            If nReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Failed to execute CheckWriteOffReason")
            End If
            drResultArray = dtResultArray.[Select]("is_valid_for_instalments = 1")
        Catch ex As Exception
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="GetWriteOffReasonCountInTable", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckWriteOffReason", excep:=ex)
            Return PMEReturnCode.PMError

        End Try
        Return nReturn
    End Function

    Public Function GetPartyPolicies(ByVal v_lAccountID As Integer, ByRef r_vPolicyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add accountid as an input param for select
            m_lReturn = m_oDatabase.Parameters.Add(sName:="account_id", vValue:=CStr(v_lAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPoliciesForAccountSQL, sSQLName:=ACGetPoliciesForAccountName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vPolicyArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPartyPolicies Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartyPolicies", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function
End Class
