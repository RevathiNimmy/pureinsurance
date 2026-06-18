Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Globalization
Imports System.Windows.Forms
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")>
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    Implements SSP.S4I.Interfaces.ILocalInterface
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: 11th July 1997
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History:
    ' ***************************************************************** '

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sNavigatorTitle As String = ""
    Private m_lPMAuthorityLevel As Integer

    ' {* USER DEFINED CODE (Begin) *}
    Private m_lCashlistID As Integer
    Private m_lCashListItemID As Integer
    Private m_lCashlistItemMode As FormShowConstants
    Private m_lCashListTypeID As Integer
    Private m_lAccountID As Integer
    Private m_iCurrencyID As Integer
    Private m_lAllocationID As Integer
    Private m_bAllowAllocateButton As Boolean
    Private m_sActionkey As String = ""

    Private m_vAllocationIDs() As Object

    Private m_sCashListRoadmap As String = ""
    Private m_lBatchID As Integer
    Private m_cAmount As Decimal
    Private m_cWriteOffAmount As Decimal
    Private m_bISWOFF As Boolean
    Private m_bViaInsurerPayment As Boolean

    Private m_bViaFinancePlan As Boolean
    Private m_bViaClaimPayment As Boolean
    Private m_sLedgerCode As String = ""
    Private m_iCompanyID As Integer
    Private m_vLetters(,) As Object
    Private m_lLetterTypeId As Integer
    Private m_lInsuranceFileCnt As Integer
    Private m_lGISSchemeID As Integer
    Private m_sGISDataModelCode As String = ""
    Private m_bLossSchedule As Boolean
    Private m_lCashlistitemPaymentStatusId As Integer
    Private m_sReceiptPolicyRef As String = ""
    Private m_lMediaTypeID As Integer
    Private m_sOurRef As String
    Private m_sTheirRef As String
    Private m_sPaymentName As String = ""
    Private m_iCashListItemPaymentTypeID As Integer
    Private m_iCashListStatusId As Integer
    Private m_lApprovalType As Integer
    Private m_sMediaRef As String = ""
    Private m_iSourceForm As Integer
    Private m_sDocumentRef As String = ""

    ' {* USER DEFINED CODE (End) *}

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode
    ' PRIVATE Data Members (End)

    Const ACSourceFindCashList As Integer = 1

    'AR20050125 - PN18271
    Private m_sPayeeName As String = ""
    Private m_sPayeeAccountCode As String = ""
    Private m_sPayeeSortCode As String = ""
    Private m_sPayeeComments As String = ""
    'AR20050310 - PN19332
    Private m_bAbortCashListProcess As Boolean
    Private m_vDocumentIds As Object
    Private m_sScreenType As String = ""
    Private m_lClaimPaymentId As Integer
    Private m_iDocumentsCurrencyID As Integer
    Private m_bSilentMultiCurrencyScreen As Boolean
    Private m_lCashTransDetailID As Integer
    Private m_bViaBanking As Boolean
    Private m_sForRecommendation As String = ""
    Private m_vClaimPaymentIDs As Object
    Private m_bdisplayCashPaymentProcess As Boolean
    Private m_iClaimWorkFlowID As Integer
    Private m_bUserAuthRunCashPayment As Boolean
    Private m_lPartyBankId As Integer

    'WPR12- Enhancement Quote Collection Process
    Private m_sCashListActualCalledFrom As String = ""
    Private m_bMultiplePoliciesSelected As Boolean
    Private m_lQuoteClientCnt As Integer
    Private m_lQuoteAgentCnt As Integer
    Private m_sQuoteAgentType As String = ""
    Private m_bLetterPrint As Boolean
    Private m_vInsurerPaymentInstArray As Object
    Private m_iCashListItemReceiptTypeID As Integer

    Private m_iDocumentID As Integer
    Private m_cUnallocatedAmountForPost As Decimal
    Private m_bIsUnallocatedAmountForPost As Boolean
    ' Start - Sankar - PN 56728

    Public Property PartyBankId() As Integer
        Get
            Return m_lPartyBankId
        End Get
        Set(ByVal Value As Integer)
            m_lPartyBankId = Value
        End Set
    End Property
    ' End - Sankar - PN 56728

    ' PUBLIC Property Procedures (Begin)

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the calling application name.
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
    Public Property Letters() As Object
        Get

            Return VB6.CopyArray(m_vLetters)

        End Get
        Set(ByVal Value As Object)

            m_vLetters = Value
        End Set
    End Property

    Public Property LetterPrint() As Boolean
        Get

            Return m_bLetterPrint

        End Get
        Set(ByVal Value As Boolean)

            m_bLetterPrint = Value
        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property

    Public ReadOnly Property Task() As Integer
        Get

            ' Standard Property.

            ' Return the task.
            Return m_iTask

        End Get
    End Property

    Public ReadOnly Property Navigate() As Integer
        Get

            ' Standard Property.

            ' Return the navigate flag.
            Return m_lNavigate

        End Get
    End Property

    Public ReadOnly Property ProcessMode() As Integer
        Get

            ' Standard Property.

            ' Return the process mode.
            Return m_lProcessMode

        End Get
    End Property

    Public ReadOnly Property TransactionType() As String
        Get

            ' Standard Property.

            ' Return the type of business.
            Return m_sTransactionType

        End Get
    End Property

    Public ReadOnly Property EffectiveDate() As Date
        Get

            ' Standard Property.

            ' Return the effective date.
            Return m_dtEffectiveDate

        End Get
    End Property

    ' {* USER DEFINED CODE (Begin) *}
    Public WriteOnly Property CashlistID() As Integer
        Set(ByVal Value As Integer)
            ' Set the Cash List ID
            m_lCashlistID = Value
        End Set
    End Property
    Public Property CashlistitemID() As Integer
        Get
            ' Return the Cash List Item ID
            Return m_lCashListItemID
        End Get
        Set(ByVal Value As Integer)
            ' Set the Cash List Item ID
            m_lCashListItemID = Value
        End Set
    End Property
    Public WriteOnly Property CashlistItemMode() As Integer
        Set(ByVal Value As Integer)
            ' Set the Cash List Item Mode
            m_lCashlistItemMode = Value
        End Set
    End Property
    Public WriteOnly Property AccountID() As Integer
        Set(ByVal Value As Integer)
            ' Set the Account ID
            m_lAccountID = Value
            g_bHasAccountContext = m_lAccountID <> 0

        End Set
    End Property

    Public Property CashlistTypeID() As Integer
        Get
            Return m_lCashListTypeID
        End Get
        Set(ByVal Value As Integer)
            m_lCashListTypeID = Value
        End Set
    End Property

    Public Property CurrencyID() As Integer
        Get
            Return m_iCurrencyID
        End Get
        Set(ByVal Value As Integer)
            m_iCurrencyID = Value
        End Set
    End Property

    'WPR12- Enhancement Quote Collection Process
    Public WriteOnly Property CashListActualCalledFrom() As String
        Set(ByVal Value As String)
            m_sCashListActualCalledFrom = Value
        End Set
    End Property

    Public WriteOnly Property MultipleQuoteSelected() As Boolean
        Set(ByVal Value As Boolean)
            m_bMultiplePoliciesSelected = Value
        End Set
    End Property

    Public WriteOnly Property QuotePartyCnt() As Integer
        Set(ByVal Value As Integer)
            m_lQuoteClientCnt = Value
        End Set
    End Property

    Public WriteOnly Property QuoteAgentCnt() As Integer
        Set(ByVal Value As Integer)

            m_lQuoteAgentCnt = Value

        End Set
    End Property

    Public WriteOnly Property QuoteAgentType() As String
        Set(ByVal Value As String)

            m_sQuoteAgentType = Value

        End Set
    End Property

    Public ReadOnly Property DocumentRef() As String
        Get
            Return m_sDocumentRef
        End Get
    End Property

    Public Property CashTransDetailID() As Integer
        Get
            Return m_lCashTransDetailID
        End Get
        Set(ByVal Value As Integer)
            m_lCashTransDetailID = Value
        End Set
    End Property

    Public Property DocumentID() As Integer
        Get
            Return m_iDocumentID
        End Get
        Set(ByVal value As Integer)
            m_iDocumentID = value
        End Set
    End Property

    ' {* USER DEFINED CODE (End) *}
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
    Public Function Initialise() As Integer Implements SSP.S4I.Interfaces.ILocalInterface.Initialise

        Dim result As Integer = 0
        Dim sHelpFile As String
        Dim m_lReturn As gPMConstants.PMEReturnCode
        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily
        'eck070201
        Dim sChequeProduction As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Set the object manager to nothing.
                g_oObjectManager = Nothing

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return result
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                'DD 21/10/2003 - pass through user ID
                g_iUserID = .UserID
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
                g_sUserName = .UserName
                g_iCurrencyID = .CurrencyID
                g_iCountryID = .CountryID
            End With

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = gPMConstants.PMEProductFamily.pmePFOrion
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

            'Find out from the registry where the Help File is
            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="HelpFile", r_sSettingValue:=sHelpFile), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to retrive Helpfile", Application.ProductName)
                Return result
            End If
            If sHelpFile <> "" Then

                'App.HelpFile = sHelpFile
            End If
            'eck070201
            ' Get the Cheque Production Option
            m_lReturn = CType(GetOption(v_iOptionNumber:=60, r_sOptionValue:=sChequeProduction), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to read system option for Cheque Production assuming Not Installed.", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=New Exception(Information.Err().Description))
                g_bChequeProduction = False
            End If

            g_bChequeProduction = Not (sChequeProduction = "0")

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

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
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: SetKeys (Standard Method)
    '
    ' Description: Stores all of the parameter members with the key
    '              array.
    '
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lInnerRow As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' default to true
            m_bAllowAllocateButton = True

            ' Check we have a vaild array.
            If Not Information.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Defalt the values
            m_sCashListRoadmap = "ACTRCTV22P"
            m_lBatchID = 0
            m_bViaInsurerPayment = False
            m_bViaFinancePlan = False
            'AR20050125 - PN18271
            m_bViaClaimPayment = False

            m_iCurrencyID = g_iCurrencyID

            ' Step through the key array.
            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)

                ' Assign the parameter member with the
                ' correct key array item.

                ' {* USER DEFINED CODE (Begin) *}

                Select Case Convert.ToString(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                    Case PMNavKeyConst.ACTKeyNameCashListId

                        m_lCashlistID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.ACTKeyNameCashListItemId

                        m_lCashListItemID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                        ' Setting this via a work task, the key is passed
                        ' gets converted from actionkey to ActionKey as stored in the
                        ' PMNav_Key table... so therefore need to add additional check
                        ' because i dont know how many places this key is used....
                    Case PMNavKeyConst.ACTKeyNameActionKey, "ActionKey"

                        m_sActionkey = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                        If m_sActionkey = gACTLibrary.ACTViewCheque Then
                            m_iTask = gPMConstants.PMEComponentAction.PMView
                        ElseIf m_sActionkey = gACTLibrary.ACTFindCashList Then
                            m_iSourceForm = ACSourceFindCashList
                        End If

                    Case PMNavKeyConst.ACTKeyNameCashListItemMode

                        m_lCashlistItemMode = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.ACTKeyNameAccountID

                        m_lAccountID = CInt(Conversion.Val(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))))

                        'AR20050210 - PN18698/PN18699
                        g_bHasAccountContext = m_lAccountID <> 0

                    Case PMNavKeyConst.ACTKeyNameCashListTypeId

                        m_lCashListTypeID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.ACTKeyNameCurrencyID

                        Dim dbNumericTemp As Double
                        If Double.TryParse(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                            m_iCurrencyID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        Else
                            m_iCurrencyID = g_iCurrencyID
                        End If

                    Case PMNavKeyConst.ACTKeyAllowAllocateButton

                        m_bAllowAllocateButton = CBool(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.ACTKeyNameCashListAllocationRoadmap

                        m_sCashListRoadmap = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNameBatchID

                        m_lBatchID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.ACTKeyNameInsurerPayment
                        m_bViaInsurerPayment = True
                        If IsArray(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) Then
                            For lInnerRow = LBound(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow), 2) To UBound(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow), 2)
                                Select Case Trim$(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lInnerRow)))
                                    Case ACTKeyNameInsurerPayment
                                        m_cAmount = CDec(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lInnerRow))
                                    Case ACTKeyNameCashListItemPaymentTypeID
                                        m_iCashListItemPaymentTypeID = ToSafeInteger(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lInnerRow))
                                    Case "cashlistitem_receipt_type_id"
                                        m_iCashListItemReceiptTypeID = CLng(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lInnerRow))
                                    Case "InstalmentArray"
                                        m_vInsurerPaymentInstArray = vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lInnerRow)
                                    Case ACTKeyNameUnallocatedAmountForPost
                                        m_cUnallocatedAmountForPost = ToSafeCurrency(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lInnerRow))
                                    Case ACTKeyNameIsUnallocatedAmountForPost
                                        m_bIsUnallocatedAmountForPost = ToSafeBoolean(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lInnerRow))
                                End Select
                            Next lInnerRow
                        Else
                            m_cAmount = vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)
                        End If

                    Case PMNavKeyConst.ACTKeyNameBranchID

                        m_iCompanyID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.ACTKeyNameFinanceDeposit
                        m_bViaFinancePlan = True

                        m_cAmount = CDec(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.ACTKeyNameCashListItemAmount
                        'AR20050125 - PN18271
                        If Not m_bViaClaimPayment Then

                            Dim dbNumericTemp2 As Double
                            If Double.TryParse(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then

                                m_cAmount = CDec(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                            Else
                                m_cAmount = 0
                            End If
                        End If

                    Case PMNavKeyConst.PMKeyNameGISSchemeId

                        Dim dbNumericTemp3 As Double
                        If Double.TryParse(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then

                            m_lGISSchemeID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        Else
                            m_lGISSchemeID = 0
                        End If

                    Case PMNavKeyConst.PMKeyNameDataModelCode

                        m_sGISDataModelCode = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNameLossSchedule

                        m_bLossSchedule = CBool(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.ACTKeyNameCashListItemPaymentStatusId

                        m_lCashlistitemPaymentStatusId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.ACTKeyNameReceiptPolicyRef

                        m_sReceiptPolicyRef = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.ACTKeyNamePaymentName

                        m_sPaymentName = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.ACTKeyNameMediaTypeID

                        m_lMediaTypeID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.ACTKeyNameCashListItemPaymentTypeID

                        m_iCashListItemPaymentTypeID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.ACTKeyNameCashListStatusId

                        m_iCashListStatusId = Conversion.Val(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))

                    Case PMNavKeyConst.PMKeyNameApprovalType

                        m_lApprovalType = CInt(Conversion.Val(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))))

                    Case PMNavKeyConst.PMKeyNameInsReference

                        m_sMediaRef = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNameInsuranceFileCnt
                        m_lInsuranceFileCnt = gPMFunctions.ToSafeLong(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        m_lCashlistItemMode = gACTLibrary.ACTUseListHidden

                    Case PMNavKeyConst.ACTKeyNameDocumentRef

                        m_sDocumentRef = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                        'AR20050125 - PN18271
                    Case PMNavKeyConst.PMKeyNamePayeeName

                        m_sPayeeName = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNamePayeeAccountCode

                        m_sPayeeAccountCode = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNamePayeeSortCode

                        m_sPayeeSortCode = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNamePayeeComments

                        m_sPayeeComments = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNameClaimPayment

                        If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)).ToUpper() <> "UNDERWRITING" Then
                            m_bViaClaimPayment = True

                            m_cAmount = CDec(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        End If
                        'AR20050310 - PN19332
                    Case PMNavKeyConst.ACTKeyNameCashListProcessAbort
                        m_bAbortCashListProcess = gPMFunctions.ToSafeLong(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow), 0) = 1

                    Case PMNavKeyConst.ACTKeyNameDocumentID

                        m_vDocumentIds = vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)

                    Case PMNavKeyConst.PMKeyNameScreenType

                        m_sScreenType = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNameClaimPaymentId

                        m_lClaimPaymentId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.ACTKeyNameTransactionCurrencyID

                        m_iDocumentsCurrencyID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case "SilentMultiCurrencyScreen"

                        m_bSilentMultiCurrencyScreen = CBool(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.ACTKeyNameWriteOffAmount

                        m_cWriteOffAmount = CDec(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        m_bISWOFF = True

                    Case PMNavKeyConst.ACTKeyNameLedgerCode

                        m_sLedgerCode = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                        m_sLedgerCode = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameBankingAmount
                        m_bViaBanking = True
                        m_cAmount = gPMFunctions.ToSafeCurrency(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNameRecommendation
                        m_sForRecommendation = gPMFunctions.ToSafeString(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNameClaimPaymentIDs

                        m_vClaimPaymentIDs = vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)
                    Case PMNavKeyConst.PMKeyNameDisplayCashPaymentProcess
                        m_bdisplayCashPaymentProcess = gPMFunctions.ToSafeBoolean(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameClaimWorkflowId
                        m_iClaimWorkFlowID = gPMFunctions.ToSafeInteger(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameUserAuthRunCashPayment
                        m_bUserAuthRunCashPayment = gPMFunctions.ToSafeBoolean(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyPartyBankId
                        m_lPartyBankId = gPMFunctions.ToSafeLong(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameOurRef
                        m_sOurRef = gPMFunctions.ToSafeString(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameTheirRef
                        m_sTheirRef = gPMFunctions.ToSafeString(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                End Select

                ' {* USER DEFINED CODE (End) *}

            Next lRow

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetKeys (Standard Method)
    '
    ' Description: Stores all of the key array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sTmp As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}

            ' Initialise the key array with the number of
            ' keys needed to be returned.
            ' Note: Remember arrays are zero based.
            ReDim vKeyArray(1, 4)

            ' Assign the key array with the parameter members.

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameCashListItemId

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lCashListItemID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameAccountID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = m_lAccountID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyNameAllocationId

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = m_lAllocationID

            m_lReturn = CType(gACTLibrary.ParseArray(vArray:=m_vAllocationIDs, sString:=sTmp, bArrayToString:=True), gPMConstants.PMEReturnCode)

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.ACTKeyNameAllocationIDs

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = sTmp

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.ACTKeyNameDocumentRef

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = m_sDocumentRef

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetSummary (Standard Method)
    '
    ' Description: Stores all of the summary array with the parameter
    '              members.
    '
    ' ***************************************************************** '

    Public Function GetSummary(ByRef vSummaryArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}

            ' Initialise the summary array with the number of
            ' items needed to be returned.
            ' Note: Remember arrays are zero based.
            Dim vKeyArray(1, 0) As Object

            ' Assign the key array with the parameter members.
            '    vSummaryArray(PMKeyName, 0) = PMKeyNameNavigatorTitle1
            '    vSummaryArray(PMKeyValue, 0) = m_sNavigatorTitle$
            vSummaryArray = Nothing
            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

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

            If Not Information.IsNothing(vTask) Then

                m_iTask = CType(CInt(vTask), gPMConstants.PMEComponentAction)
            End If

            If Not Information.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If

            If Not Information.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If

            If Not Information.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If

            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Start (Standard Method)
    '
    ' Description: Entry point for the object to start its processing.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'AR20050310 - PN19332 End here if Abort flag is set
            If m_bAbortCashListProcess Then
                m_lStatus = gPMConstants.PMEReturnCode.PMOK
                Return result
            End If

            '2005 Client Manager Security PN24911
            m_lReturn = CType(CheckSecurity(r_bPerformAllocationsAuthority:=g_bCanPerformAllocationsAuthority), gPMConstants.PMEReturnCode) 'PN24911

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Starts the interface processing.
            m_lReturn = CType(ProcessInterface(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: ProcessInterface (Standard Method)
    '
    ' Description: Calls the appropriate methods to process the
    '              interface.
    '
    ' ***************************************************************** '
    Private Function ProcessInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        m_ofrmList = New frmList
        If m_iClaimWorkFlowID >= 1 And m_iClaimWorkFlowID <= 3 And (Not m_bdisplayCashPaymentProcess Or Not m_bUserAuthRunCashPayment) Then
            Return result
        End If

        ' Set this before initialising !
        m_ofrmList.CashListAllocationRoadmap = m_sCashListRoadmap

        ' Call the List Form initialise method

        m_lReturn = m_ofrmList.Initialise()

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            m_ofrmList = Nothing
            Return result
        End If

        ' Assign the parameters to the interface properties.
        With m_ofrmList

            '***********
            ' MEvans : 14-05-2003 : CQ 709
            .ApprovalType = m_lApprovalType
            '***********

            .CallingAppName = m_sCallingAppName
            .Task = m_iTask
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate
            .PMAuthorityLevel = m_lPMAuthorityLevel

            ' {* USER DEFINED CODE (Begin) *}
            .CashlistID = m_lCashlistID
            .AccountID = m_lAccountID
            .CashlistTypeID = m_lCashListTypeID
            .CurrencyID = m_iCurrencyID
            .AllowAllocateButton = m_bAllowAllocateButton
            .ActionKey = m_sActionkey
            .CashlistitemID = m_lCashListItemID
            .SourceForm = m_iSourceForm
            .BatchID = m_lBatchID
            .Amount = m_cAmount
            .CompanyID = m_iCompanyID
            .ViaInsurerPayment = m_bViaInsurerPayment
            .ViaFinancePlan = m_bViaFinancePlan
            .ViaClaimPayment = m_bViaClaimPayment 'AR20050125 - PN18271
            .InsuranceFileCnt = m_lInsuranceFileCnt
            .GISSchemeID = m_lGISSchemeID
            .GISDataModelCode = m_sGISDataModelCode
            .LossSchedule = m_bLossSchedule
            .CashListItemPaymentStatusID = m_lCashlistitemPaymentStatusId
            .ReceiptPolicyRef = m_sReceiptPolicyRef
            .PaymentName = m_sPaymentName
            .MediaTypeID = m_lMediaTypeID
            .CashListItemPaymentTypeID = m_iCashListItemPaymentTypeID
            .CashListStatusId = m_iCashListStatusId
            .MediaRef = m_sMediaRef
            .DocumentRef = m_sDocumentRef
            .PayeeName = m_sPayeeName 'AR20050125 - PN18271
            .PayeeAccountCode = m_sPayeeAccountCode 'AR20050125 - PN18271
            .PayeeSortCode = m_sPayeeSortCode 'AR20050125 - PN18271
            .PayeeComments = m_sPayeeComments 'AR20050125 - PN18271

            .DocumentIds = m_vDocumentIds
            .ScreenType = m_sScreenType
            .ClaimPaymentId = m_lClaimPaymentId
            .DocumentsCurrencyID = m_iDocumentsCurrencyID
            .SilentMultiCurrencyScreen = m_bSilentMultiCurrencyScreen
            .WriteOffAmount = m_cWriteOffAmount
            .IsWOFF = m_bISWOFF
            .LedgerCode = m_sLedgerCode
            .ViaBanking = m_bViaBanking
            .ForRecommendation = m_sForRecommendation

            .ClaimPaymentIDs = m_vClaimPaymentIDs
            .PartyBankId = m_lPartyBankId

            'WPR12- Enhancement Quote Collection Process
            .CashListActualCalledFrom = m_sCashListActualCalledFrom
            .MultipleQuoteSelected = m_bMultiplePoliciesSelected
            .QuotePartyCnt = m_lQuoteClientCnt
            .QuoteAgentCnt = m_lQuoteAgentCnt
            .QuoteAgentType = m_sQuoteAgentType
            .DocumentID = m_iDocumentID
            .InsurerPaymentInstArray = m_vInsurerPaymentInstArray
            .Cashlistitemreceipttypeid = m_iCashListItemReceiptTypeID
            .UnallocatedAmountForPost = m_cUnallocatedAmountForPost
            .IsUnallocatedAmountForPost = m_bIsUnallocatedAmountForPost
            .OurRef = m_sOurRef
            .TheirRef = m_sTheirRef
        End With

        ' Call the Load method to setup the List Form details

        m_lReturn = m_ofrmList.Load_Renamed()

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            m_ofrmList = Nothing
            Return result
        End If

        ' Call the ShowForm method to show the form, allow user input etc.
        '

        If m_lCashlistItemMode = gACTLibrary.ACTUseListHidden Then
            m_lReturn = CType(m_ofrmList.ShowForm(lDisplayState:=gACTLibrary.ACTUseListHidden), gPMConstants.PMEReturnCode)
        Else
            m_lReturn = CType(m_ofrmList.ShowForm(lDisplayState:=FormShowConstants.Modal), gPMConstants.PMEReturnCode)
        End If

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            m_ofrmList = Nothing
            Return result
        End If

        ' Assign the property members from the interface parameters.
        With m_ofrmList
            m_lStatus = .Status
            ' {* USER DEFINED CODE (Begin) *}
            m_lCashListItemID = .CashlistitemID
            m_lAccountID = .AccountID
            m_lAllocationID = .AllocationID

            m_vAllocationIDs = .AllocationIDs
            m_vLetters = .Letters
            ' {* USER DEFINED CODE (End) *}
            'Deepak
            CashTransDetailID = .CashTransDetailID
            m_sDocumentRef = .DocumentRef
        End With
        If m_lStatus <> gPMConstants.PMEReturnCode.PMCancel Then
            If Not Information.IsArray(m_vLetters) Then
                ' Gets the Details of the Letters.
                m_lReturn = m_ofrmList.GetLetterDetails()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_ofrmList = Nothing
                    Return result
                End If
                m_vLetters = m_ofrmList.Letters
            End If
        End If
        If Information.IsArray(m_vLetters) Then
            m_sDocumentRef = CStr(m_vLetters(2, 0))
        End If
        'Document fails when Cancelled if Produce Document is checked
        'Also Checking Status before ProcessLetters
        If m_sForRecommendation <> "T" Then
            If Information.IsArray(m_vLetters) And m_ofrmList.CashDrawerID = 0 And Status <> gPMConstants.PMEReturnCode.PMCancel Then
                If Not m_bLetterPrint Then
                    m_lReturn = CType(m_ofrmList.ProcessLetters(), gPMConstants.PMEReturnCode)
                End If
            End If
        End If

        If Not Information.IsArray(m_vLetters) Then
            m_bLetterPrint = False
        End If
        ' Unload and destroy the instance of the interface
        ' from memory.
        m_ofrmList.Close()
        m_ofrmList = Nothing

        Return result

    End Function

    'PRIVATE Methods (End)

    Public Sub New()
        MyBase.New()

        ' Class Initialise Event.

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error Section.
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface entry class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    ' Name: CheckSecurity (Standard Method)
    '
    ' Description: Check whether the user has authority to view clients
    ' History:     2005 Client Security. Added as part of PN24378
    '
    ' ***************************************************************** '
    Private Function CheckSecurity(ByRef r_bPerformAllocationsAuthority As Boolean) As Integer

        Dim result As Integer = 0


        Dim sValue As String = ""
        result = gPMConstants.PMEReturnCode.PMTrue
        r_bPerformAllocationsAuthority = True

        Return result

    End Function
    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class