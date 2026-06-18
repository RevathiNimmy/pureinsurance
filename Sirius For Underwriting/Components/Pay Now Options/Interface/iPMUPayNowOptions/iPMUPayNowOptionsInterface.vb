Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.IO
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: 31 July 2006
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Interface"

    Private m_lInsuranceFileCnt As Integer
    Private m_crAmountDue As Decimal
    Private m_sPaymentOption As String = ""
    Private m_lPaymentAccountId As Integer
    Private m_iDebitAgainst As Integer
    Private m_vCreditTransactions As Object
    Private m_vLetters(,) As Object

    Private m_lCashListID As Integer
    Private m_lCashListItemID As Integer

    ' To Check that OK is Clicked
    Private m_bOkClick As Boolean
    Private m_lStatus As Integer


    Private m_sCallingAppName As String = ""
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sNavigatorTitle As String = ""
    Private m_sStepStatus As String = ""

    Private m_oBusiness As bSIRPayNowOptions.Business

    ' Stores the return value for the a function call.
    Private m_lReturn As Integer
    Private m_bMultiplePoliciesSelected As Boolean
    Private m_lAgentCnt As Integer

    Dim m_frmInterface As frmInterface
    Dim m_lCashTransDetailID As Integer
    ' Start - Sankar - PN 56728
    Private m_lMediaTypeId As Integer
    Private m_lPartyBankId As Integer
    ' End - Sankar - PN 56728

    'Rahul
    Private m_dCoverStartDate As Date
    'WPR12- Enhancement Quote Collection Process
    Private m_lClientCnt As Integer

    Private m_bLetterPrint As Boolean
    Private m_iPrePayment As Integer

    Public Property LetterPrint() As Boolean
        Get

            Return m_bLetterPrint

        End Get
        Set(ByVal Value As Boolean)

            m_bLetterPrint = Value

        End Set
    End Property
    Public Property PrePayment() As Integer
        Get
            Return m_iPrePayment
        End Get
        Set(ByVal Value As Integer)
            m_iPrePayment = Value
        End Set
    End Property


    Public WriteOnly Property CoverStartDate() As Date
        Set(ByVal Value As Date)

            m_dCoverStartDate = Value

        End Set
    End Property

    'End
    ' Start - Sankar - PN 56728

    Public Property MediaTypeId() As Integer
        Get
            Return m_lMediaTypeId
        End Get
        Set(ByVal Value As Integer)
            m_lMediaTypeId = Value
        End Set
    End Property


    Public Property PartyBankId() As Integer
        Get
            Return m_lPartyBankId
        End Get
        Set(ByVal Value As Integer)
            m_lPartyBankId = Value
        End Set
    End Property
    ' End - Sankar - PN 56728

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property


    Public Property EffectiveDate() As Date
        Get
            Return m_dtEffectiveDate
        End Get
        Set(ByVal Value As Date)
            m_dtEffectiveDate = Value
        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property


    Public Property StepStatus() As String
        Get

            Return m_sStepStatus

        End Get
        Set(ByVal Value As String)

            m_sStepStatus = Value

        End Set
    End Property

    'Properties Begin
    Public WriteOnly Property InsuranceFileCnt() As Integer
        Set(ByVal Value As Integer)

            m_lInsuranceFileCnt = Value
        End Set
    End Property

    Public WriteOnly Property AgentCnt() As Integer
        Set(ByVal Value As Integer)

            m_lAgentCnt = Value

        End Set
    End Property
    Public WriteOnly Property MultiplePoliciesSelected() As Boolean
        Set(ByVal Value As Boolean)

            m_bMultiplePoliciesSelected = Value

        End Set
    End Property
    Public WriteOnly Property AmountDue() As Decimal
        Set(ByVal Value As Decimal)

            m_crAmountDue = Value

        End Set
    End Property

    Public WriteOnly Property PaymentOption() As String
        Set(ByVal Value As String)

            m_sPaymentOption = Value

        End Set
    End Property

    Public ReadOnly Property PaymentAccountID() As Integer
        Get

            Return m_lPaymentAccountId

        End Get
    End Property

    Public ReadOnly Property DebitAgainst() As Integer
        Get

            Return m_iDebitAgainst

        End Get
    End Property

    Public ReadOnly Property OKClick() As Boolean
        Get

            Return m_bOkClick

        End Get
    End Property

    Public ReadOnly Property CreditTransactions() As Object
        Get

            Return m_vCreditTransactions

        End Get
    End Property

    Public ReadOnly Property Letters() As Object
        Get

            Return VB6.CopyArray(m_vLetters)

        End Get
    End Property

    Public ReadOnly Property CashListID() As Integer
        Get

            Return m_lCashListID

        End Get
    End Property
    Public ReadOnly Property CashListItemID() As Integer
        Get

            Return m_lCashListItemID

        End Get
    End Property

    Public ReadOnly Property CashTransDetailID() As Integer
        Get

            Return m_lCashTransDetailID

        End Get
    End Property

    'WPR12- Enhancement Quote Collection Process

    Public WriteOnly Property ClientCnt() As Integer
        Set(ByVal Value As Integer)
            m_lClientCnt = Value
        End Set
    End Property

    Public Function Initialise() As Integer

        Dim result As Integer = 0
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

            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRPayNowOptions.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
            End With

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMEdit
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

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

            ' Starts the interface processing.
            m_lReturn = ProcessInterface()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = m_lReturn

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
        Dim m_lAccountId As Integer
        Dim vValue As String = ""
        Dim m_lAgentAccountId As Integer
        'WPR12- Enhancement Quote Collection Process
        Dim m_lClientAccountId As Integer

        m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnablePayNowOptions, v_vBranch:=g_iSourceID, r_vUnderwriting:=vValue)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = m_lReturn
            gPMFunctions.RaiseError("ProcessInterface", "Process Interface Failed", gPMConstants.PMELogLevel.PMLogError)
            Return result
        End If

        If vValue = "1" Or m_iPrePayment = "1" Then  'Ashwani - (RFC_Enable_PrePayment_functionality)
            ' "Enable PayNow on Make Live" =1
            ' Payment Option="PayNow" Both Cash List and Pre-Payment Screen will be shown
            ' Payment Option="Invoice" Both Cash List and Pre-Payment Screen will be shown
            'Run CashList Process
            If m_bMultiplePoliciesSelected Then

                If gPMFunctions.ToSafeLong(m_lClientCnt) > 0 Then
                    m_lReturn = GetAccountID(m_lClientCnt, m_lClientAccountId)
                    CreateCashList(0, m_lClientAccountId, m_crAmountDue)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return m_lReturn
                    End If

                    m_iDebitAgainst = 0
                    m_lPaymentAccountId = m_lClientAccountId
                Else

                    If m_sPaymentOption.ToLower() = "paynow" Then

                        m_lReturn = GetAccountID(m_lAgentCnt, m_lAgentAccountId)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                        End If
                        CreateCashList(0, m_lAgentAccountId, m_crAmountDue)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return m_lReturn
                        End If

                        m_iDebitAgainst = 0
                        m_lPaymentAccountId = m_lAgentAccountId

                    End If

                End If
            Else

                If gPMFunctions.ToSafeLong(m_lClientCnt) > 0 Then
                    m_lReturn = GetAccountID(m_lClientCnt, m_lClientAccountId)
                    CreateCashList(0, m_lClientAccountId, m_crAmountDue)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return m_lReturn
                    End If

                    m_iDebitAgainst = 0
                    m_lPaymentAccountId = m_lClientAccountId
                Else

                    If m_sPaymentOption.ToLower() = "paynow" Then
                        m_lReturn = GetAccountIDFromInsuranceFile(m_lInsuranceFileCnt, m_lAccountId)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                        End If

                        CreateCashList(m_lInsuranceFileCnt, m_lAccountId, m_crAmountDue)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return m_lReturn
                        End If

                        m_iDebitAgainst = 0
                        m_lPaymentAccountId = m_lAccountId
                    End If
                End If
            End If
            'Now Show PayNow

            ' Load the interface into memory.
            m_lReturn = LoadInterface()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to load the interface.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Display the interface.
            m_lReturn = ShowInterface(lDisplayState:=FormShowConstants.Modal)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to display the inteface.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_bOkClick = m_frmInterface.OKClick
            If m_frmInterface.OKClick Then
                m_bOkClick = m_frmInterface.OKClick
                m_lPaymentAccountId = m_frmInterface.PaymentAccountID
                m_iDebitAgainst = m_frmInterface.DebitAgainst


                m_vCreditTransactions = m_frmInterface.CreditTransactions
                m_vLetters = m_frmInterface.Letters
            End If
            '     Check if we have had an error so far.
            If m_frmInterface.ErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST return the error.
                result = m_frmInterface.ErrorNumber
            End If

            ' Destroy the interface from memory.
            m_lReturn = UnLoadInterface()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to unload the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

        Else
            'Run CashList Process
            If m_sPaymentOption.ToLower() = "paynow" Then
                If m_bMultiplePoliciesSelected Then

                    If gPMFunctions.ToSafeLong(m_lClientCnt) > 0 Then
                        m_lReturn = GetAccountID(m_lClientCnt, m_lClientAccountId)
                        CreateCashList(0, m_lClientAccountId, m_crAmountDue)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return m_lReturn
                        End If

                        m_iDebitAgainst = 0
                        m_lPaymentAccountId = m_lClientAccountId
                    Else


                        m_lReturn = GetAccountID(m_lAgentCnt, m_lAgentAccountId)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                        End If
                        CreateCashList(0, m_lAgentAccountId, m_crAmountDue)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return m_lReturn
                        End If
                        m_iDebitAgainst = 0
                        m_lPaymentAccountId = m_lAgentAccountId
                    End If
                Else

                    If gPMFunctions.ToSafeLong(m_lClientCnt) > 0 Then
                        m_lReturn = GetAccountID(m_lClientCnt, m_lClientAccountId)
                        CreateCashList(0, m_lClientAccountId, m_crAmountDue)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return m_lReturn
                        End If

                        m_iDebitAgainst = 0
                        m_lPaymentAccountId = m_lClientAccountId
                    Else

                        m_lReturn = GetAccountIDFromInsuranceFile(m_lInsuranceFileCnt, m_lAccountId)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                        End If

                        CreateCashList(m_lInsuranceFileCnt, m_lAccountId, m_crAmountDue)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = m_lReturn
                        End If

                        m_iDebitAgainst = 0
                        m_lPaymentAccountId = m_lAccountId
                    End If

                End If
            End If
        End If


        Return result

    End Function


    Private Function GetAccountIDFromInsuranceFile(ByRef lInsuranceFileCnt As Integer, ByRef lAccountId As Integer) As Integer

        Dim result As Integer = 0


        Dim vResultArray(,) As Object
        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get the details from the business object.
        Dim temp_m_oBusiness As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRPayNowOptions.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        m_oBusiness = temp_m_oBusiness


        m_lReturn = m_oBusiness.GetAccountIDFromInsuranceFile(lInsuranceFileCnt, vResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Information.IsArray(vResultArray) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        lAccountId = gPMFunctions.ToSafeLong(ToSafeInteger(vResultArray(0, 0)))


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

        m_frmInterface = New frmInterface()
        ' Assign the parameters to the interface properties.
        With m_frmInterface

            .CallingAppName = m_sCallingAppName
            .Task = m_iTask
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate

            ' {* USER DEFINED CODE (Begin) *}

            .InsuranceFileCnt = m_lInsuranceFileCnt
            .AmountDue = m_crAmountDue
            'Rahul
            .CoverStartDate = m_dCoverStartDate
            If m_bMultiplePoliciesSelected Then
                .MultiplePoliciesSelected = True
                .AgentCnt = m_lAgentCnt
                'WPR12- Enhancement Quote Collection Process
                .ClientId = m_lClientCnt
            End If
            ' {* USER DEFINED CODE (End) *}

        End With

        ' Load the instance of the interface into memory.

        'Developer Guide No. 68(latest guide) 
        'Load(m_frmInterface)

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
            m_sStepStatus = .StepStatus


        End With

        ' Unload and destroy the instance of the interface
        ' from memory.
        m_frmInterface.Close()
        m_frmInterface = Nothing

        Return result

    End Function

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

        ' Display the interface.
        VB6.ShowForm(m_frmInterface, lDisplayState)

        If lDisplayState = FormShowConstants.Modal Then
            ' Check for any form errors.
            'Developer Guide No. 50 (guide)
            If m_frmInterface.ErrorNumber <> 0 Then
                'Developer Guide No. 50 (guide)
                result = m_frmInterface.ErrorNumber
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
    ' Name: CreateCashList
    '
    ' Description: Runs CashList and CashListItem Process
    '
    ' History:
    '           Created : Rajesh Choudhary : Date : 01 Sep 2006
    ' ***************************************************************** '
    Private Function CreateCashList(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lAccountId As Integer, ByVal v_cGrossTotal As Decimal) As Integer

        Dim result As Integer = 0
        Dim lCashListType As Integer
        Dim vResultArray(,) As Object
        Dim lCurrencyID As Integer




        result = gPMConstants.PMEReturnCode.PMTrue

        Const kMethodName As String = "CreateCashList"

        lCurrencyID = g_oObjectManager.CurrencyID

        If v_cGrossTotal > 0 Then
            lCashListType = gACTLibrary.ACTCashListTypeReceipts
        Else
            lCashListType = gACTLibrary.ACTCashListTypePayments
        End If

        If v_lInsuranceFileCnt <> 0 Then

            m_lReturn = m_oBusiness.GetInsuranceRef(v_lInsuranceFileCnt, vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Information.IsArray(vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lInsuranceFileCnt", v_lInsuranceFileCnt)
                oDict.Add("v_lAccountId", v_lAccountId)
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to execute GetInsuranceRef", vApp:=ACApp, vClass:=ACClass, vMethod:="Terminate", oDicParms:=oDict)
                Return result
            End If

            lCurrencyID = gPMFunctions.ToSafeInteger(vResultArray(18, 0))
        End If

        m_lReturn = OpenCashList(m_lCashListID, lCashListType, g_iSourceID, lCurrencyID)


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Start - Sankar - PN 56728

        m_lReturn = m_oBusiness.GetPaymentDetails(lInsuranceFileCnt:=m_lInsuranceFileCnt, r_lMediaTypeId:=m_lMediaTypeId, r_lPartyBankId:=m_lPartyBankId)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "GetPaymentDetails Failed")
        End If

        'Developer Guide No. 98
        m_lReturn = OpenCashListItem(r_lCashListItemID:=m_lCashListItemID, v_lCashListID:=m_lCashListID, v_lCurrencyID:=lCurrencyID, v_lSourceID:=g_iSourceID, v_dPaymentAmount:=v_cGrossTotal, v_lAccountId:=v_lAccountId, v_lMediaTypeID:=m_lMediaTypeId, v_lCashListType:=lCashListType, v_lPartyBankId:=m_lPartyBankId)
        'End - Sankar - PN56728

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            Return m_lReturn
        End If

        Return result
    End Function


    ' ***************************************************************** '
    ' Name: OpenCashList
    '
    ' Description: Opens CashList
    '
    ' History:
    '           Created : Rajesh Choudhary : Date : 10 Aug 2006
    ' ***************************************************************** '
    Function OpenCashList(ByRef r_lCashListId As Integer, ByVal v_lCashListType As Integer, ByVal v_lSourceID As Integer, ByRef v_lTransCurrency As Integer) As Integer
        Dim result As Integer = 0
        Dim iACTCashList As Object

        Const kMethodName As String = "OpenCashList"


        Dim oACTCashList As iACTCashList.Interface_Renamed

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' open up the cashlist form
            Dim temp_oACTCashList As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oACTCashList, sClassName:="iACTCashList.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oACTCashList = temp_oACTCashList

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMError
            End If

            ' Set up keys
            Dim vKeys(1, 3) As Object

            vKeys(0, 0) = PMNavKeyConst.ACTKeyNameCashListTypeId

            vKeys(1, 0) = v_lCashListType


            vKeys(0, 1) = PMNavKeyConst.PMKeyNameSourceId

            vKeys(1, 1) = v_lSourceID


            vKeys(0, 2) = PMNavKeyConst.ACTKeyNameCurrencyID

            vKeys(1, 2) = v_lTransCurrency


            vKeys(0, 3) = PMNavKeyConst.ACTKeyNameCashListRoadmap

            vKeys(1, 3) = "PAYNOW"

            ' Create a cash list

            m_lReturn = oACTCashList.Initialise
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error.
                result = gPMConstants.PMEReturnCode.PMError

                oACTCashList.Dispose()
                oACTCashList = Nothing
                Return result
            End If


            m_lReturn = oACTCashList.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error.
                result = gPMConstants.PMEReturnCode.PMError

                oACTCashList.Dispose()
                oACTCashList = Nothing
                Return result
            End If


            m_lReturn = oACTCashList.SetKeys(vKeyArray:=vKeys)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error.
                result = gPMConstants.PMEReturnCode.PMError

                oACTCashList.Dispose()
                oACTCashList = Nothing
                Return result
            End If


            m_lReturn = oACTCashList.Start

            v_lTransCurrency = oACTCashList.CurrencyID
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error.
                result = gPMConstants.PMEReturnCode.PMError

                oACTCashList.Dispose()
                oACTCashList = Nothing
                Return result
            End If

            ' If form not OK ie Cancelled return status

            If oACTCashList.Status = gPMConstants.PMEReturnCode.PMOK Then
                ' Return newly created Cash List ID

                r_lCashListId = oACTCashList.CashListID
                m_bOkClick = True
            Else
                result = gPMConstants.PMEReturnCode.PMCancel
                r_lCashListId = 0
            End If

            ' Kill the Cash List object

            oACTCashList.Dispose()
            oACTCashList = Nothing



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="OpenCashList", r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: OpenCashListItem
    '
    ' Description: Opens CashListItem
    '
    ' History:
    '           Created : Rajesh Choudhary : Date : 18 Aug 2006
    ' ***************************************************************** '
    Function OpenCashListItem(ByRef r_lCashListItemID As Integer, ByVal v_lCashListID As Integer, ByVal v_lCurrencyID As Integer, ByVal v_lSourceID As Integer, ByVal v_dPaymentAmount As Double, ByVal v_lAccountId As Integer, ByVal v_lMediaTypeID As Integer, ByVal v_lCashListType As String, Optional ByVal v_lPartyBankId As Integer = 0) As Integer
        Dim result As Integer = 0
        Dim iACTCashListItem As Object


        Dim oACTCashListItem As iACTCashListItem.Interface_Renamed

        Dim vKeys(,) As Object
        Dim lPaymentTypeID, lPaymentStatusID As Integer
        Dim vResultArray(,) As Object
        Dim sPartyName, sAgentType As String

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_oACTCashListItem As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oACTCashListItem, sClassName:="iACTCashListItem.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oACTCashListItem = temp_oACTCashListItem

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Failed to get an instance of the business object.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create object 'iACTCashListItem.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="OpenCashListItem", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If


            'WPR12- Enhancement Quote Collection Process
            'get Agent Type
            If m_lAgentCnt > 0 Then
                'Agent Exists
                If m_bMultiplePoliciesSelected Then

                    m_lReturn = m_oBusiness.GetAgentDetailsFromAgentID(m_lAgentCnt, vResultArray)
                Else

                    m_lReturn = m_oBusiness.GetAgentType(m_lInsuranceFileCnt, vResultArray)
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Agent Type.", vApp:=ACApp, vClass:=ACClass, vMethod:="OpenCashListItem", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If

                If Not Information.IsArray(vResultArray) Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="No Agent Type Found", vApp:=ACApp, vClass:=ACClass, vMethod:="OpenCashListItem", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If

                sAgentType = Convert.ToString(vResultArray(0, 0)).Trim()
            End If

            With oACTCashListItem


                m_lReturn = .Initialise()


                .CallingAppName = ACApp
                'WPR12- Enhancement Quote Collection Process

                .CashListActualCalledFrom = m_sCallingAppName

                .MultipleQuoteSelected = m_bMultiplePoliciesSelected

                .QuotePartyCnt = m_lClientCnt

                .QuoteAgentCnt = m_lAgentCnt

                .QuoteAgentType = sAgentType

                'create the keys here
                'Set the start up options


                m_lReturn = .SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeNBLive, vEffectiveDate:=DateTime.Now)

                'PMProcessModeGeneric, _
                '
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to OpenCashListItem", vApp:=ACApp, vClass:=ACClass, vMethod:="OpenCashListItem", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If

                ReDim vKeys(1, 11) ' Sankar - PN 56728 - 10 to 11

                ' cashlist id

                vKeys(0, 0) = PMNavKeyConst.ACTKeyNameCashListId

                vKeys(1, 0) = v_lCashListID

                ' cash LIST type

                vKeys(0, 1) = PMNavKeyConst.ACTKeyNameCashListTypeId

                vKeys(1, 1) = v_lCashListType

                '(RC) PN32961
                If StringsHelper.ToDoubleSafe(v_lCashListType) = gACTLibrary.ACTCashListTypePayments Then

                    ' ActionKey

                    vKeys(0, 2) = PMNavKeyConst.ACTKeyNameActionKey

                    vKeys(1, 2) = gACTLibrary.ACTRefund

                    ' PaymentTypeID

                    vKeys(0, 3) = PMNavKeyConst.ACTKeyNameCashListItemPaymentTypeID

                    vKeys(1, 3) = 4 'CashListItemPaymentTypeID - REFUND

                End If

                ' currency

                vKeys(0, 4) = PMNavKeyConst.ACTKeyNameCurrencyID

                vKeys(1, 4) = v_lCurrencyID

                ' sourceID

                vKeys(0, 5) = PMNavKeyConst.ACTKeyNameBranchID

                vKeys(1, 5) = v_lSourceID

                ' payment amount

                vKeys(0, 6) = PMNavKeyConst.ACTKeyNameCashListItemAmount

                vKeys(1, 6) = v_dPaymentAmount

                ' account id

                vKeys(0, 7) = PMNavKeyConst.ACTKeyNameAccountID

                vKeys(1, 7) = v_lAccountId

                ' media type id

                vKeys(0, 8) = PMNavKeyConst.ACTKeyNameMediaTypeID

                vKeys(1, 8) = v_lMediaTypeID

                ' mode

                vKeys(0, 9) = PMNavKeyConst.ACTKeyNameCashListItemMode

                vKeys(1, 9) = gACTLibrary.ACTUseListHidden

                ' Screen Type

                vKeys(0, 10) = PMNavKeyConst.PMKeyNameScreenType

                vKeys(1, 10) = "PAYNOW"

                ' Start - Sankar - PN 56728
                ' Party Bank ID

                vKeys(0, 11) = PMNavKeyConst.PMKeyPartyBankId

                vKeys(1, 11) = v_lPartyBankId
                ' End - Sankar - PN 56728


                .SetKeys(vKeys)



                .LetterPrint = m_bLetterPrint


                m_lReturn = .Start()


                If .Status = gPMConstants.PMEReturnCode.PMCancel Then
                    Return gPMConstants.PMEReturnCode.PMCancel
                End If
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                r_lCashListItemID = .CashListItemID

                m_lCashTransDetailID = .CashTransDetailID

                m_bLetterPrint = .LetterPrint


                ReDim m_vLetters(1, 0)

                m_vLetters(1, 0) = .DocumentRef
                If m_lClientCnt <> 0 Then
                    m_vLetters(0, 0) = m_lClientCnt
                Else
                    m_vLetters(0, 0) = m_lAgentCnt
                End If

            End With

            result = gPMConstants.PMEReturnCode.PMTrue


            oACTCashListItem.Dispose()
            oACTCashListItem = Nothing

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="OpenCashListItem", r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

            MessageBox.Show(Information.Err().Description, Application.ProductName)

        Finally
        End Try
        Return result
    End Function


    Private Function GetAccountID(ByRef lAgentCnt As Integer, ByRef lAccountId As Integer) As Integer
        Dim result As Integer = 0



        Dim vResultArray(,) As Object
        result = gPMConstants.PMEReturnCode.PMTrue

        '     m_lReturn& = g_oObjectManager.GetInstance( _
        ''        oObject:=m_oBusiness, _
        ''        sClassName:="bSIRPayNowOptions.Business", _
        ''        vInstanceManager:=PMGetViaClientManager)

        ' Get the details from the business object.

        m_lReturn = m_oBusiness.GetAccountID(lAgentCnt, vResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Information.IsArray(vResultArray) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        lAccountId = gPMFunctions.ToSafeLong(ToSafeInteger(vResultArray(0, 0)))


        Return result

    End Function
End Class

