Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Globalization
Imports System.Windows.Forms
'developer guide no.129
Imports SharedFiles
Imports Artinsoft.VB6.Utils
Imports System.Reflection

<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    Implements SSP.S4I.Interfaces.ILocalInterface
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: 10th September 1997
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History:
    ' ***************************************************************** '
    'developer guide no.69
    Dim frmBanking As frmBanking
    Dim frmInterface As frmInterface
    Dim frmList As frmList
    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"
    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)
    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sNavigatorTitle As String = ""

    ' Status members
    Private m_sProcessStatus As New FixedLengthString(2)
    Private m_sMapStatus As New FixedLengthString(2)
    Private m_sStepStatus As New FixedLengthString(2)

    ' {* USER DEFINED CODE (Begin) *}
    Private m_lCashlistID As Integer
    Private m_lCashlistTypeID As Integer
    Private m_iCurrencyID As Integer
    Private m_iDepositCurrencyID As Integer
    Private m_sCashListRef As New FixedLengthString(25)

    Private m_sCashListRoadmap As String = ""
    'eck090500
    Private m_iCompanyID As Integer

    Private m_sDebitCredit As String = ""
    ' {* USER DEFINED CODE (End) *}

    ' Stores the exit status of the interface.
    Private m_lStatus As gPMConstants.PMEReturnCode

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    Private m_lPMAuthorityLevel As Integer
    'todolist(As per VB Code)
    'Private m_frmInterface As frmBanking
    'Private m_frmInterface As Form
    Private m_frmInterface As Object

    '06/11/2002 - PWC - Added to set start form
    Private m_sStartForm As String = ""
    Private Const m_ksStartDefault As String = ""
    Private Const m_ksStartBanking As String = "start_banking"
    Private Const m_ksStartList As String = "start_list"

    'PSL 15/04/2003 Screen mode if you are using form just to select a drawer
    Private m_sScreenMode As String = ""
    'DJM 08/12/2003
    Private m_cAmount As Decimal
    'Steve Watton 19/05/2004 Store the Account_ID & document ref PN 12009 PN 12023
    Private m_lAccountId As Integer
    Private m_sDocumentRef As String = ""
    Private m_lPartyCount As Integer

    Private m_oBusiness As bACTCashList.Form
    Private m_sCashListAllocRoadmap As String = ""

    Private m_sWMTask As String = ""
    Private m_sPaymentMethod As String = ""

    Private m_bHasDocumentContext As Boolean 'AR20050310 - PN19332
    Private m_bAbortCashListProcess As Boolean 'AR20050310 - PN19332

    'SMJB CQ1966
    Const cPendingBanking As String = "B"

    Private m_lMediaTypeId As Integer
    Private m_bMediaTypeSpecifiedInKeys As Boolean
    'Developer Guide No 101
    'Private m_vDocumentIds As String = ""
    Private m_vDocumentIds As Object
    Private m_bdisplayCashPaymentProcess As Boolean
    Private m_iClaimWorkFlowID As Integer
    Private m_bUserAuthRunCashPayment As Boolean
    Private m_iDocumentsCurrencyID As Integer

    Private m_bMulitCurrencyFlag As Boolean

    Public Property MulitCurrencyFlag() As Boolean
        Get
            Return m_bMulitCurrencyFlag
        End Get
        Set(ByVal value As Boolean)
            m_bMulitCurrencyFlag = value
        End Set
    End Property

    Public Property ScreenMode() As String
        Get
            Return m_sScreenMode
        End Get
        Set(ByVal Value As String)
            m_sScreenMode = Value
        End Set
    End Property

    ' PRIVATE Data Members (End)
    ' PUBLIC Property Procedures (Begin)

    Public Property PMAuthorityLevel() As Integer
        Get
            Return m_lPMAuthorityLevel
        End Get
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = Value
        End Set
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the calling application name.
            m_sCallingAppName = Value

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

    Public ReadOnly Property StepStatus() As String
        Get

            ' Standard Property.

            ' Return the Steps Status
            Return m_sStepStatus.Value

        End Get
    End Property

    ' {* USER DEFINED CODE (Begin) *}
    Public Property CashlistID() As Integer
        Get

            ' Return the Cash List ID
            Return m_lCashlistID

        End Get
        Set(ByVal Value As Integer)

            ' Set the Cash List ID
            m_lCashlistID = Value

        End Set
    End Property

    Public Property CashlistTypeID() As Integer
        Get

            ' Return the Cash List Type ID
            Return m_lCashlistTypeID

        End Get
        Set(ByVal Value As Integer)

            ' Set the Cash List Type ID
            m_lCashlistTypeID = Value

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
        Dim sHelpFile As String = ""
        Dim m_lReturn As gPMConstants.PMEReturnCode
        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily

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
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
                'sj 13/09/2002 - start
                g_iCurrencyID = .CurrencyID
                g_iUserID = .UserID
                'sj 13/09/2002 - end
            End With

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Initialise the Status settings
            m_sProcessStatus.Value = gPMConstants.PMNavStatusUnknown
            m_sMapStatus.Value = gPMConstants.PMNavStatusUnknown
            m_sStepStatus.Value = gPMConstants.PMNavStatusUnknown


            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = g_sProductFamily
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

            'Find out from the registry where the Help File is
            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="HelpFile", r_sSettingValue:=sHelpFile), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to retrive Helpfile", Application.ProductName)
                Return result
            End If
            If sHelpFile <> "" Then
                'developer guide no.39(No Solution)
                'App.HelpFile = sHelpFile
            End If

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
    '   ECK 4/12/2000 Call from Premium Finance Transaction Processing
    '
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim bIsClaimPayment As Boolean
        Dim sUnderwritingorAgency As String = ""
        Dim lInnerRow As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check we have a vaild array.
            If Not Information.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Step through the key array.
            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)
                ' Assign the parameter member with the
                ' correct key array item.

                ' {* USER DEFINED CODE (Begin) *}


                'developer guide no.248
                'Select Case CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                Select Case Convert.ToString(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                    Case PMNavKeyConst.ACTKeyNameMediaTypeID

                        m_lMediaTypeId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        If m_lMediaTypeId <> 0 Then
                            m_bMediaTypeSpecifiedInKeys = True
                        End If

                    Case PMNavKeyConst.ACTKeyNameCashListId
                        m_lCashlistID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.ACTKeyNameCashListTypeId
                        m_lCashlistTypeID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.ACTKeyNameCashListRoadmap
                        m_sCashListRoadmap = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                        'Use keys to set start mode
                    Case PMNavKeyConst.PMKeyNameStartForm
                        m_sStartForm = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                        'add debit_credit key
                    Case gPMConstants.PMKeyNameDebitCredit
                        m_sDebitCredit = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        If m_sDebitCredit.ToUpper() = "DEBIT" Then
                            m_lCashlistTypeID = gACTLibrary.ACTCashListTypeReceipts
                            m_sCashListRoadmap = "RECEIPT"
                        ElseIf m_sDebitCredit.ToUpper() = "CREDIT" Then
                            m_lCashlistTypeID = gACTLibrary.ACTCashListTypePayments
                            m_sCashListRoadmap = "PAYMENT"
                        End If

                    Case PMNavKeyConst.PMKeyNameScreenType
                        m_sScreenMode = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)).Trim()

                        'Passed through to avoid asking for Branch
                    Case PMNavKeyConst.PMKeyNameSourceId

                        m_iCompanyID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.ACTKeyNameCurrencyID

                        Dim dbNumericTemp As Double
                        If Double.TryParse(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                            m_iCurrencyID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        Else
                            m_iCurrencyID = g_iCurrencyID
                        End If

                        'Store the document reference from the previous posting
                    Case PMNavKeyConst.ACTKeyNameDocumentRef

                        m_sDocumentRef = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                        'Store the party
                    Case PMNavKeyConst.PMKeyNamePartyCnt

                        Dim dbNumericTemp2 As Double
                        If Double.TryParse(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then

                            m_lPartyCount = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        End If

                        'Set as can be passed via keys
                    Case PMNavKeyConst.ACTKeyNameAccountID

                        Dim dbNumericTemp3 As Double
                        If Double.TryParse(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then

                            m_lAccountId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        End If

                        'Take in the amount
                    Case PMNavKeyConst.ACTKeyNameFinanceDeposit, PMNavKeyConst.ACTKeyNameTotalPremium

                        m_cAmount = CDec(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNameCurrencyKey
                        m_iDepositCurrencyID = CDec(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.ACTKeyNameInsurerPayment
                        If IsArray(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) Then
                            For lInnerRow = LBound(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow), 2) To UBound(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow), 2)
                                Select Case Trim$(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lInnerRow)))
                                    Case ACTKeyNameInsurerPayment
                                        m_cAmount = CDec(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lInnerRow))
                                End Select
                            Next lInnerRow
                        Else
                            m_cAmount = CDec(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        End If
                    Case PMNavKeyConst.PMKeyNameClaimPayment

                        If gPMFunctions.ToSafeString(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))) <> "UNDERWRITING" Then
                            bIsClaimPayment = True

                            m_cAmount = CDec(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        End If

                    Case PMNavKeyConst.PMKeyNameWMTask

                        m_sWMTask = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                        'AR20050311 - PN19332 Use PMNavKeyConst constant
                    Case PMNavKeyConst.ACTKeyNamePaymentMethod

                        m_sPaymentMethod = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.ACTKeyNameDocumentID

                        'Developer Guide No 98
                        'm_vDocumentIds = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        m_vDocumentIds = vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)
                    Case PMNavKeyConst.PMKeyNameDisplayCashPaymentProcess

                        m_bdisplayCashPaymentProcess = gPMFunctions.ToSafeBoolean(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))
                    Case PMNavKeyConst.PMKeyNameClaimWorkflowId

                        m_iClaimWorkFlowID = gPMFunctions.ToSafeInteger(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))
                    Case PMNavKeyConst.PMKeyNameUserAuthRunCashPayment

                        m_bUserAuthRunCashPayment = gPMFunctions.ToSafeBoolean(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))

                    Case PMNavKeyConst.PMKeyNameMultiCurrencyFlag
                        m_bMulitCurrencyFlag = gPMFunctions.ToSafeBoolean(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))

                    Case PMNavKeyConst.PMKeyNameCashlistRef
                        m_sCashListRef.Value = vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)

                    Case PMNavKeyConst.ACTKeyNameCashListAllocationRoadmap
                        'This is called from Insurer payment  
                        m_sScreenMode = ToSafeString(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                End Select

                ' {* USER DEFINED CODE (End) *}
            Next lRow
            'Determine the account_id and amount of the transaction
            'These 2 values will be returned in Get Keys



            If m_lPartyCount <> 0 Then

                ' Get an instance of the business object via
                ' the public object manager.
                Dim temp_m_oBusiness As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bACTCashList.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                m_oBusiness = temp_m_oBusiness

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to get an instance of the business object.
                    result = gPMConstants.PMEReturnCode.PMFalse

                    gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to create the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Terminate")

                    Return result
                End If


                m_lReturn = m_oBusiness.GetAccountFromParty(v_lPartyCnt:=m_lPartyCount, v_lSourceID:=m_iCompanyID, r_lAccountID:=m_lAccountId)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the Account ID from the Party Count", vApp:=ACApp, vClass:=ACClass, vMethod:="Terminate")
                    Return result
                End If

                If m_sDocumentRef <> "" Then
                    'AR20050310 - PN19332
                    m_bHasDocumentContext = True

                    'DC030305 : PN19191 : added parameter for company

                    m_lReturn = m_oBusiness.GetOSCashForDebit(v_lAccountId:=m_lAccountId, v_sDocumentRef:=m_sDocumentRef, v_lSourceID:=m_iCompanyID, r_iCurrencyID:=m_iDocumentsCurrencyID, r_cCash:=m_cAmount)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Log Error.
                        gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the receipt amount from the document reference", vApp:=ACApp, vClass:=ACClass, vMethod:="Terminate")
                        Return result
                    End If
                End If


                m_oBusiness.Dispose()
                m_oBusiness = Nothing
            End If

            'DC140105 made speific check on amount as even if amount 0 then automatically
            'sets to PAYMENT, not suitable if the amount is not being passed through as a key
            'Now determine what sort of CashList we need to add

            If m_cAmount = 0 And m_sCashListRoadmap$ = "" Then
                m_sCashListRoadmap$ = "PAYMENT"
            End If

            If m_cAmount > 0 Then
                m_lCashlistTypeID = gACTLibrary.ACTCashListTypeReceipts
                m_sCashListRoadmap = "RECEIPT"
            End If
            If m_cAmount < 0 Then
                If m_lCashlistTypeID = 0 Then
                    If m_sScreenMode = "CLP" Then
                        m_lCashlistTypeID = gACTLibrary.ACTCashListTypeClaimPayments
                    Else
                        m_lCashlistTypeID = gACTLibrary.ACTCashListTypePayments
                    End If
                End If
                m_sCashListRoadmap = "PAYMENT"
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}

            ' Initialise the key array with the number of
            ' keys needed to be returned.
            ' Note: Remember arrays are zero based.
            'eck090500
            ReDim vKeyArray(1, 8)

            ' Assign the key array with the parameter members.

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameCashListId

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lCashlistID


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameCashListTypeId

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = m_lCashlistTypeID


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyNameCurrencyID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = m_iCurrencyID
            'eck090500

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.ACTKeyNameBranchID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = m_iCompanyID

            'Steve Watton 19/05/2004 PN 12009 & 12023, pass in amount and account id for CLI

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.ACTKeyNameCashListItemAmount

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = m_cAmount


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMNavKeyConst.ACTKeyNameAccountID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = m_lAccountId

            'AR20050310 - PN19332 Add abort cash list process flag

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 6) = PMNavKeyConst.ACTKeyNameCashListProcessAbort

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 6) = IIf(m_bAbortCashListProcess, 1, 0)


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 7) = PMNavKeyConst.ACTKeyNameMediaTypeID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 7) = m_lMediaTypeId


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 8) = PMNavKeyConst.ACTKeyNameTransactionCurrencyID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 8) = m_iDocumentsCurrencyID

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
            ReDim vSummaryArray(2, 0)

            ' Assign the key array with the parameter members.
            '    vSummaryArray(PMKeyName, 0) = PMKeyNameNavigatorTitle1
            '    vSummaryArray(PMKeyValue, 0) = m_sNavigatorTitle$

            ' CF151298 - Added some summary information

            vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummHeading, 0) = "Cash List Reference"

            vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummValue, 0) = m_sCashListRef.Value

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

                m_iTask = CInt(vTask)
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
    ' Name: SetStatus (Standard Method)
    '
    ' Description: Set the Process, Map and Step status.
    ' Note:        A Property Get is provided for the Step Status only
    '              as this is the only one which this component can
    '              alter directly.
    ' ***************************************************************** '
    Public Function SetStatus(ByRef sProcessStatus As String, ByRef sMapStatus As String, ByRef sStepStatus As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the current Status settings.
            m_sProcessStatus.Value = sProcessStatus.Trim()
            m_sMapStatus.Value = sMapStatus.Trim()
            m_sStepStatus.Value = sStepStatus.Trim()

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            Dim sMessage As String = "" 'AR20050311 - PN19332

            result = gPMConstants.PMEReturnCode.PMTrue

            'Do not continue if this is the Generic NB roadmap and we are paying by instalments.
            If m_sWMTask = "WMPMBNB" And m_sPaymentMethod = "Instalments" Then
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                m_lReturn = gPMConstants.PMEReturnCode.PMTrue
                Return result
            End If

            'AR20050310 - PN19332 If the document has an outstanding amount of 0 (for this client)
            '                     then abort cash list process
            If m_bHasDocumentContext Then
                If m_cAmount = 0 Then
                    MessageBox.Show("The policy holder has no outstanding amounts for this transaction." & Strings.Chr(13) & Strings.Chr(10) & "The cash list process will be bypassed.", "Cash List", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    m_bAbortCashListProcess = True
                    m_lStatus = gPMConstants.PMEReturnCode.PMOK
                    Return result
                Else
                    If m_sPaymentMethod.ToUpper() = "DIRECT DEBIT" Or m_sPaymentMethod.ToUpper() = "CREDIT CARD TO INSURER" Or m_sPaymentMethod.ToUpper() = "DIRECT TO INSURER" Then
                        sMessage = "The payment method for this transaction is " & m_sPaymentMethod & "."
                        sMessage = sMessage & Strings.Chr(13) & Strings.Chr(10) & "Therefore the cash list amount is only for the additional disbursements."
                        MessageBox.Show(sMessage, "Cash List", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If
            Else
                m_bAbortCashListProcess = False
            End If

            ' Starts the interface processing.
            m_lReturn = ProcessInterface()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                Return gPMConstants.PMEReturnCode.PMFalse
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
    ' PUBLIC Methods (End)


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


        Dim lCashlistID As Integer
        Dim sStatusCode As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue
        If m_iClaimWorkFlowID >= 1 And m_iClaimWorkFlowID <= 3 And (Not m_bdisplayCashPaymentProcess Or Not m_bUserAuthRunCashPayment) Then
            Return result
        End If

        ' Load the interface into memory.
        m_lReturn = LoadInterface()

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to load the interface.
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If m_sScreenMode = "SELECT" Then


            If m_frmInterface.lvwCashListDrawers.ListItems.Count = 1 Then

                lCashlistID = m_frmInterface.lvwCashListDrawers.ListItems(1).SubItems(2)
                ' KG 30/06/03 - Chk/Create, if a Cashlist has not been created
                If lCashlistID = 0 Then

                    m_lReturn = m_frmInterface.ChkCreateCashList(True, lCashlistID)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        'fail
                        Return result
                    End If
                End If
                m_frmInterface.CashlistID = lCashlistID
                m_frmInterface.Status = gPMConstants.PMEReturnCode.PMOK

                'Single user check required - drawer may be in the banking process
                'If the lock already exists, sCurrentlyLockedBy will tell us by who

                m_lReturn = m_frmInterface.CheckLock()
                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Drawer is locked
                    m_frmInterface.Status = gPMConstants.PMEReturnCode.PMCancel
                End If

                'SMJB CQ1966 06/08/03: Must also check the status of the cash list
                'it may be pending approval

                m_lReturn = m_frmInterface.Business.GetCashListStatusCode(lCashlistID, sStatusCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If sStatusCode = cPendingBanking Then
                    MessageBox.Show("This cash drawer is currently in Banking, receipts cannot be added until Banking is complete.", "Cash Drawer Unavailable", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else
                ' Display the interface.
                m_lReturn = ShowInterface(lDisplayState:=FormShowConstants.Modal)
            End If

        ElseIf m_sScreenMode = "CLP" Then

            ' if there is an amount to pay
            If m_cAmount <> 0 Then


                m_frmInterface.ScreenMode = "CLP"

                ' display interface to create cashlist item
                m_lReturn = ShowInterface(lDisplayState:=FormShowConstants.Modal)

            Else
                m_frmInterface.Status = gPMConstants.PMEReturnCode.PMCancel
                Return result

            End If

        Else
            ' Display the interface.
            m_lReturn = ShowInterface(lDisplayState:=FormShowConstants.Modal)

        End If

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to display the inteface.
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        'PSL 15/04/2003
        If m_sScreenMode = "SELECT" Then
            'NIIT - Replaced with the Migrated code 1144
            'm_lCashlistID = m_frmInterface.CashlistID
            m_lCashlistID = ReflectionHelper.GetMember(m_frmInterface, "CashlistID")
        End If

        ' Destroy the interface from memory.
        m_lReturn = UnLoadInterface()

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to unload the interface.
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        If m_bHasDocumentContext Then
            If m_iDocumentsCurrencyID <> m_iCurrencyID Then
                'We are receiving the payment in a different currency than the one the transaction was raised in.
                m_lReturn = ConvertCashListAmount()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                End If
            End If
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: LoadInterface (Standard Method)
    '
    ' Description: Loads the instance of the interface into memory and
    '              passes the parameters in.
    '
    ' ***************************************************************** '
    Private Function LoadInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        'Set the start up form - if set, will have been passed in SetKeys
        Select Case m_sStartForm
            Case m_ksStartBanking
                frmBanking = New frmBanking
                m_frmInterface = frmBanking
            Case m_ksStartList
                frmList = New frmList
                m_frmInterface = frmList
                'PSL 15/04/2003

                m_frmInterface.ScreenMode = m_sScreenMode
            Case Else
                frmInterface = New frmInterface
                m_frmInterface = frmInterface
        End Select

        ' Assign the parameters to the interface properties.
        With m_frmInterface
            'DD 02/09/2003: Ask for Branch if it wasn't passed in
            If m_iCompanyID = 0 Then
                m_lReturn = .GetCompany(m_iCompanyID:=m_iCompanyID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            .CallingAppName = m_sCallingAppName
            .Task = m_iTask
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate
            .ForceDepositeCurrencyID = m_iDepositCurrencyID
            'DJM 08/12/2003
            .CurrencyID = m_iCurrencyID
            .CashListRef = m_sCashListRef.Value

            ' {* USER DEFINED CODE (Begin) *}
            Select Case m_sStartForm
                Case m_ksStartDefault




                    .CashlistID = m_lCashlistID
                    .CashlistTypeID = m_lCashlistTypeID

                    .CashListRoadmap = m_sCashListRoadmap

                    .DebitCredit = m_sDebitCredit

                    .ScreenMode = m_sScreenMode

                    .MediaTypeSpecifiedInKeys = m_bMediaTypeSpecifiedInKeys

                    .MediaTypeID = m_lMediaTypeId

                    .DocumentIds = m_vDocumentIds

                    .AccountId = m_lAccountId
                    .MulitCurrencyFlag = m_bMulitCurrencyFlag
                Case m_ksStartBanking
                    .CashlistID = m_lCashlistID
                    '.CurrencyID = g_iCurrencyID  'needs cash drawer currency, not default currency

                Case Else

            End Select
            .CompanyID = m_iCompanyID

        End With

        ' Load the instance of the interface into memory.

        'developer guide no.68
        'Load(m_frmInterface)

        ' Check if we have had an error so far.
        If m_frmInterface.ErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
            ' We have already encountered an error,
            ' so we MUST return the error.
            result = m_frmInterface.ErrorNumber
        End If

        ' Set the status in the interface.
        m_lReturn = m_frmInterface.SetStatus(sProcessStatus:=m_sProcessStatus.Value, sMapStatus:=m_sMapStatus.Value, sStepStatus:=m_sStepStatus.Value)
        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to set the status.
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: UnLoadInterface (Standard Method)
    '
    ' Description: Unloads the instance of the interface from memory.
    '
    ' ***************************************************************** '
    Private Function UnLoadInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the property members from the interface parameters.
        With m_frmInterface
            m_lStatus = .Status
            m_sStepStatus.Value = .StepStatus
            ' {* USER DEFINED CODE (Begin) *}
            If m_sStartForm = m_ksStartDefault Then

                m_lCashlistID = .CashlistID
                m_iCurrencyID = .CurrencyID
                m_lCashlistTypeID = .CashlistTypeID
                m_sCashListRef.Value = .CashListRef
                m_lMediaTypeId = .MediaTypeID
                If .Amount <> 0 Then
                    m_cAmount = .Amount
                End If
            End If

            ' {* USER DEFINED CODE (End) *}
        End With

        ' Unload and destroy the instance of the interface
        ' from memory.
        m_frmInterface.Close()
        m_frmInterface = Nothing

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: CheckSecurity (Standard Method)
    '
    ' Description: Check whether the user has authority to view clients
    ' History:     2005 Client Security  20/04/2005
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (CheckSecurity) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CheckSecurity(ByRef r_bRaiseCashAuthority As Boolean) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    'Dim sUnderwritingorAgency, sValue As String
    'Dim iIsRaiseCash As Integer
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'r_bRaiseCashAuthority = True
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error Section.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckSecurity", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: ShowInterface (Standard Method)
    '
    ' Description: Displays the instance of the interface using the
    '              display state.
    '
    ' ***************************************************************** '
    Private Function ShowInterface(ByRef lDisplayState As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        'MKW190104 PN9787 Do Not Allow User into Form (if no banks or currencies defined).


        If (m_frmInterface.uctBankAccount.ListCount <= 0) Or (m_frmInterface.uctCurrency.ListCount <= 0) Then
            MessageBox.Show("No Currencies and/or Banks Defined." & Strings.Chr(13) & Strings.Chr(10) & "Please rectify.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Else
            ' Display the interface.
            VB6.ShowForm(m_frmInterface, lDisplayState)

            If lDisplayState = FormShowConstants.Modal Then
                ' Check for any form errors.
                If m_frmInterface.ErrorNumber <> 0 Then
                    result = m_frmInterface.ErrorNumber
                End If
            End If
        End If

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


    Private Function ConvertCashListAmount() As Integer
        Dim result As Integer = 0
        Dim bACTCurrencyConvert As Object

        Const kMethodName As String = "ConvertCashListAmount"

        Dim iBaseCurrencyID As Integer

        Dim oCurrencyConvert As bACTCurrencyConvert.Form
        Dim cOldAmount, cNewAmount As Decimal

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            Dim temp_oCurrencyConvert As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oCurrencyConvert, "bACTCurrencyConvert.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oCurrencyConvert = temp_oCurrencyConvert
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("g_oObjectManager.GetInstance", "sClassName:=bACTCurrencyConvert.Form", gPMConstants.PMELogLevel.PMLogError)
            End If


            m_lReturn = oCurrencyConvert.GetBaseCurrency(v_lCompanyId:=m_iCompanyID, r_iBaseCurrencyID:=iBaseCurrencyID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("oCurrencyConvert.GetBaseCurrency", "v_lCompanyId:=" & m_iCompanyID, gPMConstants.PMELogLevel.PMLogError)
            End If

            cOldAmount = m_cAmount
            cNewAmount = m_cAmount

            If m_iDocumentsCurrencyID <> iBaseCurrencyID Then

                m_lReturn = oCurrencyConvert.Convert(v_bConvertToBase:=True, v_lCurrencyID:=m_iDocumentsCurrencyID, v_lCompanyId:=m_iCompanyID, r_cOriginalAmount:=cOldAmount, r_cConvertedAmount:=cNewAmount)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("oCurrencyConvert.Convert", "v_bConvertToBase:=True", gPMConstants.PMELogLevel.PMLogError)
                End If
                cOldAmount = cNewAmount
            End If

            If m_iCurrencyID <> iBaseCurrencyID Then

                m_lReturn = oCurrencyConvert.Convert(v_bConvertToBase:=False, v_lCurrencyID:=m_iCurrencyID, v_lCompanyId:=m_iCompanyID, r_cOriginalAmount:=cOldAmount, r_cConvertedAmount:=cNewAmount)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'PN32234
                    If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                        MessageBox.Show("No currency rate is set for the selected currency type." & Strings.Chr(13) & Strings.Chr(10) & "Please set the currency rate", "Cash List", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                        Return result
                    Else
                        gPMFunctions.RaiseError("oCurrencyConvert.Convert", "v_bConvertToBase:=False", gPMConstants.PMELogLevel.PMLogError)
                    End If

                End If
            End If

            m_cAmount = cNewAmount


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
            ' Do any tidy up, e.g. Set x = Nothing here
            If Not (oCurrencyConvert Is Nothing) Then

                oCurrencyConvert.Dispose()
                oCurrencyConvert = Nothing
            End If

        End Try

        Return result
    End Function
End Class
