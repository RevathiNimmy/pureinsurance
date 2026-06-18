Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Windows.Forms
'Developer Guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("uctCLMReceipt_NET.uctCLMReceipt")> _
Partial Public Class uctCLMReceipt
    Inherits System.Windows.Forms.UserControl
    'Developer Guide no. 50
    Dim frmReceiptDetails As frmReceiptDetails
    Private Const ACClass As String = "uctCLMReceipt"

    Public Event UnRecoverableError(ByVal Sender As Object, ByVal e As EventArgs)

    ' objects
    Private m_oObjectManager As bObjectManager.ObjectManager

    Private m_oCurrencyConvert As bACTCurrencyConvert.Form

    Private m_oPaymentMethod As bCLMPaymentMethod.Business

    Private m_oBusiness As bCLMPeril.Business

    ' generic interface details
    Private m_iTask As Integer
    Private m_iLanguageID As Integer
    Private m_iSourceID As Integer
    Private m_iUserId As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_dtEffectiveDate As Date
    Private m_sTransactionType As String = ""

    ' custom interface details
    Private m_bAdvancedTaxScriptingOption As Boolean
    Private m_iComponentMode As Integer
    Private m_lPrevClaimPayeeOption As Integer
    Private m_bUnrecoverableError As Boolean
    Private m_lRecoveryMode As Integer
    Private m_bIsSalvage As Boolean
    Private m_bATSSattlement As Boolean

    ' collection
    Private m_colReceiptItems As Collection

    ' array details
    Private m_vClaimReceiptItemDetails As Object
    Private m_vClaimReceiptDetails As Object
    Private m_vRecoveryDetails(,) As Object
    ' Start - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)
    Private m_vAttachPartyToRecovery(,) As Object
    Private m_bPartyAlreadyAttached As Boolean
    ' End - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)
    ' claim details
    Private m_vClaimDetails(,) As Object
    Private m_lClaimId As Integer
    Private m_lClaimPerilId As Integer
    Private m_lClaimPaymentId As Integer
    Private m_lProductId As Integer
    Private m_lLossCurrencyId As Integer
    Private m_lClaimSourceId As Integer
    Private m_sLossCurrency As String = ""
    Private m_dtLossDate As Date
    Private m_lClaimBaseCurrencyId As Integer
    Private m_lClassOfBusinessId As Integer
    Private m_sClassOfBusinessCode As String = ""
    Private m_lInsuranceFileCnt As Integer
    Private m_bPostClaimTax As Boolean
    Private m_sClaimNumber As String = ""
    Private m_sPerilDescription As String = ""

    ' receipt details
    Private m_lPaymentPartyTo As Integer
    Private m_sRisktype As String = ""
    Private m_vClaimPaymentTo As Object
    Private m_sClaimPaymentToCode As Object
    Private m_vSafeHarbour As Object
    Private m_sSafeHarbourCode As Object
    Private m_vMediaType(,) As Object
    Private m_vCountry(,) As Object
    Private m_crTotalThisReceiptGross As Decimal
    Private m_crTotalThisReceiptNet As Decimal
    Private m_crTotalTaxAmount As Decimal

    ' lookup details
    Private m_vCurrencyArray(,) As Object
    Private m_vTaxGroupTaxBandDetails(,) As Object
    Private m_vTaxBand As Object
    Private m_vTaxGroup(,) As Object
    Private m_vClassOfBusiness As Object

    ' receipt item details
    Private m_dCurrencyToBaseXRate As Double
    Private m_dAccountToBaseXRate As Double
    Private m_dSystemToBaseXRate As Double

    ' selected party details
    Private m_bSelectedPayeeDomiciledForTax As Boolean
    Private m_lSelectedPayeeDetailIndex As Integer
    Private m_lSelectedPayeeAccountCurrencyId As Integer
    Private m_sSelectedPayee As String = ""
    Private m_lSelectedPayeeId As Integer

    ' claim payable details
    Private m_lClaimReceivableAccountId As Integer
    Private m_lClaimReceivableCurrencyId As Integer

    ' other party details
    Private m_lOtherPartyCnt As Integer
    Private m_lOtherPartyCurrencyId As Integer
    Private m_bOtherPartyDomiciledForTax As Boolean

    ' client details
    Private m_lClientId As Integer
    Private m_sClientName As String = ""
    Private m_lClientCurrencyId As Integer
    Private m_bClientDomiciledForTax As Boolean
    Private m_sClientTaxNumber As String = ""
    Private m_crClientTaxPercentage As Decimal
    Private m_bClientTaxExempt As Boolean

    ' lead agent details
    Private m_lLeadAgentId As Integer
    Private m_sLeadAgentName As String = ""
    Private m_lLeadAgentCurrencyId As Integer
    Private m_bLeadAgentDomiciledForTax As Boolean
    Private m_sLeadAgentTaxNumber As String = ""
    Private m_crLeadAgentTaxPercentage As Decimal
    Private m_bLeadAgentTaxExempt As Boolean

    ' transfer agent details
    Private m_bAgentInTransfer As Boolean
    Private m_sAgentTransferBusinessType As String = ""
    Private m_lTransferAgentPartyCnt As Integer
    Private m_bTransferAgentDomiciledForTax As Boolean
    Private m_sTransferAgentTaxNumber As String = ""
    Private m_crTransferAgentTaxPercentage As Decimal
    Private m_bTransferAgentTaxExempt As Boolean
    Private m_lTransferAgentCurrencyId As Integer
    Private m_sTransferAgentName As String = ""

    ' insurer details
    Private m_vCoinsurance(,) As Object
    Private m_vReinsurance(,) As Object
    Private m_colReinsurers As Collection
    Private m_colCoinsurers As Collection

    'S4B Claim Enhancements R&D 2005
    Private m_bReceiptMade As Boolean
    Private m_lCashListAccountId As Integer
    Private m_lCashListPartyId As Integer
    Private m_crCashListAmount As Decimal
    Private m_sInsurer As String = ""

    ''Start(Saurabh Agrawal) Tech Spec QBENZCR004 Claim Recovery Reinsurance
    Private m_bRI2007Enabled As Boolean
    Private m_lClaimReceiptID As Integer
    Private m_vClaimReceiptItemDetail(,) As Object
    Private m_vClaimReceiptDetail(,) As Object
    Private m_bViewReceiptMode As Boolean
    Private m_bReceiptExcludeTax As Boolean
    ''End (Saurabh Agrawal) Tech Spec QBENZCR004 Claim Recovery Reinsurance

    Private m_bAutomateReceiptGeneration As Boolean
    Private m_oCashListItemReceiptType(,) As Object

    Private Function CheckMandatory() As Integer
        'ATS
        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue
        'Start - Sankar - (Tech Spec WR34 - Claims Recovery Party Link.doc)
        If CBool(CStr(txtParty.Text = "").Trim()) Then
            MessageBox.Show("Invalid Payee Name", "Payee Name Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            result = gPMConstants.PMEReturnCode.PMFalse
        End If
        'End - Sankar - (Tech Spec WR34 - Claims Recovery Party Link.doc)
        If m_bAdvancedTaxScriptingOption Then
            If chkITDomiciled.CheckState = CheckState.Checked And txtITPercentage.Text.Trim() = "" Then
                MessageBox.Show("Insured percentage is mandatory", "Percentage Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                result = gPMConstants.PMEReturnCode.PMFalse
            End If
        End If
        Return result
    End Function

    <Browsable(False)> _
    Public WriteOnly Property RecoveryMode() As Integer
        Set(ByVal Value As Integer)
            m_lRecoveryMode = Value

            If m_lRecoveryMode = kRecoveryModeSalvageReceipt Then
                m_bIsSalvage = True
            End If
        End Set
    End Property

    <Browsable(False)> _
    Public ReadOnly Property ClaimNumber() As String
        Get
            Return m_sClaimNumber
        End Get
    End Property


    <Browsable(True)> _
    Public Property ClaimID() As Integer
        Get
            Return m_lClaimId
        End Get
        Set(ByVal Value As Integer)
            m_lClaimId = Value
        End Set
    End Property


    <Browsable(True)> _
    Public Property ClaimPerilId() As Integer
        Get
            Return m_lClaimPerilId
        End Get
        Set(ByVal Value As Integer)
            m_lClaimPerilId = Value
        End Set
    End Property

    <Browsable(False)> _
    Public WriteOnly Property ClaimPaymentId() As Integer
        Set(ByVal Value As Integer)
            m_lClaimPaymentId = Value
        End Set
    End Property
    ''Start(Saurabh Agrawal) Tech Spec QBENZCR004 Claim Recovery Reinsurance
    <Browsable(False)> _
    Public WriteOnly Property ClaimReceiptId() As Integer
        Set(ByVal Value As Integer)
            m_lClaimReceiptID = Value
        End Set
    End Property
    ''End(Saurabh Agrawal) Tech Spec QBENZCR004 Claim Recovery Reinsurance
    'S4B Claim Enhancements R&D 2005
    <Browsable(False)> _
    Public ReadOnly Property ReceiptMade() As Boolean
        Get
            Return m_bReceiptMade
        End Get
    End Property

    ' ***************************************************************** '
    ' Name: Load
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 26-07-2005 : 360 - Taxes On Claims
    ' ***************************************************************** '
    Public Function Load_Renamed() As Integer

        Dim nResult As Integer = 0
        Const kMethodName As String = "Load_Renamed"

        Dim nReturn As gPMConstants.PMEReturnCode
        Dim sResult As String = "" ''Saurabh
        Try



            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' get required system options
            nReturn = GetSystemOptions()
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetSystemOptions Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            ''Start(Saurabh Agrawal) Tech Spec QBENZCR004 Claim Recovery Reinsurance
            nReturn = GetProductOptions()
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetProductOptions Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            nReturn = CType(iPMFunc.RetrieveSingleSystemOption(v_iOptionNumber:=kSIROPTReceiptExcludeTax, r_sOptionValue:=sResult), gPMConstants.PMEReturnCode)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "RetrieveSingleSystemOption Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_bReceiptExcludeTax = (Not String.IsNullOrEmpty(sResult) And sResult = "1")
            ''End(Saurabh Agrawal) Tech Spec QBENZCR004 Claim Recovery Reinsurance

            ' get claim specific details
            nReturn = GetClaimDetails()
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetClaimDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' populate user control with claim details
            nReturn = PopulateClaimDetails()
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "PopulateClaimDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' set up taxes list view
            nReturn = SetupTaxesListView()
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetupTaxesListView Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get lookup details...
            nReturn = GetLookups()
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetLookups Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' populate lookup combos
            nReturn = PopulateLookups()
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "PopulateLookups Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get latest version of all claim recoveries
            If m_lClaimReceiptID = 0 Then
                m_bViewReceiptMode = False
                nReturn = GetCurrentRecoveryDetails()
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    If nReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                        Return gPMConstants.PMEReturnCode.PMNotFound
                    Else
                        gPMFunctions.RaiseError(kMethodName, "GetCurrentRecoveryDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If

                ' setup receipt details list view
                nReturn = SetupReceiptDetailsListView()
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "SetupReceiptDetailsListView Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' populate receipt details list view
                nReturn = PopulateReceiptDetailsListView()
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "PopulateLookups Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            Else

                ''Start(Saurabh Agrawal) Tech Spec Claims Recovery Reinsurance
                m_bViewReceiptMode = True
                nReturn = GetClaimReceiptItemDetails()
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    If nReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                        Return gPMConstants.PMEReturnCode.PMNotFound
                    Else
                        gPMFunctions.RaiseError(kMethodName, "GetClaimReceiptItemDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If

                ' setup receipt details list view
                nReturn = SetupReceiptItemDetailsListView()
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "SetupReceiptItemDetailsListView Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' populate receipt details list view
                nReturn = PopulateReceiptItemDetailsListView()
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "PopulateReceiptItemDetailsListView Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                nReturn = GetClaimReceiptItemTaxDetails()
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetClaimReceiptItemTaxDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' populate this payment details
                nReturn = PopulateThisReceiptDetails()
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "PopulateThisPaymentDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
                nReturn = GetClaimReceiptDetails()
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetClaimReceiptDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
                nReturn = PopulateClaimReceiptDetails()
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "PopulateClaimReceiptDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                SSTabHelper.SetTabVisible(SSTab1, kTabThisReceipt, True)

            End If
            ''End(Saurabh Agrawal) Tech Spec QBENZCR004 - Claims Recovery Reinsurance
            ' get coinsurance details
            nReturn = GetCoinsurance()
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetCoinsurance Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' setup coinsurance listview
            nReturn = CType(SetupInsurerListView(v_bIsCoinsurance:=True), gPMConstants.PMEReturnCode)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetupInsurerListView Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' populate coinsurance
            nReturn = CType(PopulateInsurerCollection(v_bIsCoinsurance:=True), gPMConstants.PMEReturnCode)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "PopulateInsurerCollection Coinsurer Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            'S4B Claim Enhancements R&D 2005
            If g_sUnderwritingOrAgency = "U" Then
                ' get reinsurance details
                nReturn = GetReinsurance()
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetReinsurance Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' setup reinsurance listview
                nReturn = CType(SetupInsurerListView(v_bIsCoinsurance:=False), gPMConstants.PMEReturnCode)
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "SetupInsurerListView Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' populate reinsurance
                nReturn = CType(PopulateInsurerCollection(v_bIsCoinsurance:=False), gPMConstants.PMEReturnCode)
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "PopulateInsurerCollection Reinsurer Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            ' setup user control
            nReturn = SetUpUserControl()
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetUpUserControl Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' WPR085 Set media type as mandatory based on user option 5117 and default to EFT if exists
            Dim sAutoReceipt As String = ""
            nReturn = iPMFunc.GetSystemOption(5117, sAutoReceipt)
            If nReturn <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "GetSystemOption Failed", PMELogLevel.PMLogError)
            End If

            m_bAutomateReceiptGeneration = ToSafeInteger(sAutoReceipt, "0") = "1"

            If m_bAutomateReceiptGeneration And IsArray(m_vMediaType) Then

                lblMediaType.Font = VB6.FontChangeBold(lblMediaType.Font, True)

                Dim nlBound As Integer
                Dim nUBound As Integer
                Dim nItemIndex As Integer
                Dim nItemId As Integer
                Dim sDescription As String

                cboMediaType.SelectedIndex = -1
                nlBound = LBound(m_vMediaType, 2)
                nUBound = UBound(m_vMediaType, 2)

                If nlBound = nUBound Then
                    nItemId = m_vMediaType(kLookupItemId, nlBound)
                    Call SetComboBoxValue(cboMediaType, nItemId)
                End If
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally


        End Try
        Return nResult
    End Function

    ' ***************************************************************** '
    ' Name: Initialise
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 26-07-2005 : 360 - Taxes On Claims
    ' ***************************************************************** '
    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "Initialise"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            Static bIsInitialised As Boolean

            ' Check if already initialised
            If bIsInitialised Then
                Return result
            End If

            m_colReceiptItems = New Collection()
            m_colReinsurers = New Collection()
            m_colCoinsurers = New Collection()

            ' Create an instance of the object manager.
            m_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            lReturn = m_oObjectManager.Initialise(sCallingAppName:=ACApp)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "g_oOBjectManager.Initialise Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' If UserID is 0 assume that user cancelled logon
            If m_oObjectManager.UserID = 0 Then
                ' Exit application
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            ' Store the language ID from the object manager to the public variables,
            ' to enable us to use them throughout the object.
            With m_oObjectManager
                m_iLanguageID = .LanguageID
                m_iSourceID = .SourceID
                m_iUserId = .UserID
            End With

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Get an instance of the business object via the public object manager.
            Dim temp_m_oBusiness As Object
            lReturn = m_oObjectManager.GetInstance(temp_m_oBusiness, "bCLMPeril.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetInstance of bCLMPeril.Business Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            ' get instance of currency conversion component
            Dim temp_m_oCurrencyConvert As Object
            lReturn = m_oObjectManager.GetInstance(temp_m_oCurrencyConvert, "bACTCurrencyConvert.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oCurrencyConvert = temp_m_oCurrencyConvert
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetInstance of bACTCurrencyConvert.Form Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oPaymentMethod As Object
            lReturn = m_oObjectManager.GetInstance(temp_m_oPaymentMethod, "bCLMPaymentMethod.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oPaymentMethod = temp_m_oPaymentMethod
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetInstance of bCLMPaymentMethod.Business Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            'S4B Claim Enhancements R&D 2005
            lReturn = iPMFunc.getUnderwritingOrAgency(g_sUnderwritingOrAgency)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "getUnderwritingOrAgency function Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' hold Initialised status
            bIsInitialised = True


        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SetProcessModes
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 26-07-2005 : 360 - Taxes On Claims
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer


        Dim result As Integer = 0
        Const kMethodName As String = "SetProcessModes"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


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

            ' call the set process modes on the business component

            lReturn = m_oBusiness.SetProcessModes(vTransactionType:=m_sTransactionType)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "bCLMPeril.Business.SetProcessModes Failed", gPMConstants.PMELogLevel.PMLogError)
            End If




        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function

    ' ********************************************************************* '
    ' Name: GetCashListDetails
    '
    ' Parameters:
    '
    ' Description:
    '
    ' History:
    '           Created : A.Robinson - 06/02/2006 : S4B Claim Enhancements
    ' ********************************************************************* '
    Public Function GetCashListDetails(ByRef r_lAccountId As Integer, ByRef r_lPartyId As Integer, ByRef r_crCashListAmount As Decimal) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetCashListDetails"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_lAccountId = m_lCashListAccountId
            r_lPartyId = m_lCashListPartyId
            r_crCashListAmount = m_crCashListAmount

            Return result

        Catch ex As Exception


            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupsByEffectiveDate
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 26-07-2005 : 360 - Taxes On Claims
    ' ***************************************************************** '
    Private Function GetLookupsByEffectiveDate(ByVal v_sTableName As String, ByRef r_vResults As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetLookupsByEffectiveDate"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try
            result = gPMConstants.PMEReturnCode.PMTrue


            lReturn = m_oBusiness.GetLookupsByEffectiveDate(v_sTableName:=v_sTableName, r_vResults:=r_vResults)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetLookupsByEffectiveDate Failed for tablename:=" & v_sTableName, gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetLookups
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 26-07-2005 : 360 - Taxes On Claims
    ' ***************************************************************** '
    Public Function GetLookups() As Integer

        Dim nResult As Integer = 0
        Const kMethodName As String = "GetLookups"

        Dim nReturn As gPMConstants.PMEReturnCode

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' get class of business
            nReturn = CType(GetLookupsByEffectiveDate(v_sTableName:=kLookupTableNameClassOfBusiness, r_vResults:=m_vClassOfBusiness), gPMConstants.PMEReturnCode)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetLookupsByEffectiveDate Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get tax band
            nReturn = CType(GetLookupsByEffectiveDate(v_sTableName:=kLookupTableNameTaxBand, r_vResults:=m_vTaxBand), gPMConstants.PMEReturnCode)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetLookupsByEffectiveDate Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get media types
            nReturn = m_oBusiness.GetMediaTypeLookUpDetails(r_oResults:=m_vMediaType)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetMediaTypeLookUpDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get countries
            nReturn = CType(GetLookupsByEffectiveDate(v_sTableName:=kLookupTableNameCountry, r_vResults:=m_vCountry), gPMConstants.PMEReturnCode)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetLookupsByEffectiveDate Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get currency
            nReturn = CType(GetLookupsByEffectiveDate(v_sTableName:=kLookupTableNameCurrency, r_vResults:=m_vCurrencyArray), gPMConstants.PMEReturnCode)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetLookupsByEffectiveDate Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get tax group with withholding tax

            nReturn = CType(GetTaxGroupDetails(v_vIsWithHoldingTax:=DBNull.Value, r_vArray:=m_vTaxGroup), gPMConstants.PMEReturnCode)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetTaxGroupDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get tax group tax bands
            nReturn = CType(GetTaxGroupTaxBandDetails(), gPMConstants.PMEReturnCode)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetTaxGroupTaxBandDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception


            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
        End Try

        Return nResult
    End Function



    ' ***************************************************************** '
    ' Name: GetCurrentRecoveryDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 09-08-2005 : 360 - Taxes On Claims
    ' ***************************************************************** '
    Private Function GetCurrentRecoveryDetails() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetCurrentRecoveryDetails"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            lReturn = m_oBusiness.GetClaimPerilRecoveryDetails(v_lClaimPerilId:=m_lClaimPerilId, v_bIsSalvage:=m_bIsSalvage, r_vResults:=m_vRecoveryDetails, v_lClaimReceiptId:=m_lClaimReceiptID) ''Saurabh - Added ClaimReceiptId
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetCurrentRecoveryDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not Information.IsArray(m_vRecoveryDetails) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: PopulateClaimDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 08-08-2005 : 360 - Taxes On Claims
    ' ***************************************************************** '
    Private Function PopulateClaimDetails() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulateClaimDetails"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' claim details
            m_sRisktype = CStr(m_vClaimDetails(kClaimDetailRiskTypeDesc, 0))
            m_sLossCurrency = CStr(m_vClaimDetails(kClaimDetailLossCurrencyDesc, 0))
            m_dtLossDate = gPMFunctions.ToSafeDate(m_vClaimDetails(kClaimDetailLossFromDate, 0), DateTime.Today)
            m_lProductId = gPMFunctions.ToSafeLong(m_vClaimDetails(kClaimDetailProductId, 0), 0)
            m_lClaimSourceId = gPMFunctions.ToSafeLong(m_vClaimDetails(kClaimDetailClaimSourceId, 0), 0)
            m_lLossCurrencyId = gPMFunctions.ToSafeLong(m_vClaimDetails(kClaimDetailLossCurrencyId, 0), 0)
            m_sLossCurrency = CStr(m_vClaimDetails(kClaimDetailLossCurrencyDesc, 0))
            m_lClaimBaseCurrencyId = CInt(m_vClaimDetails(kClaimDetailBaseCurrencyId, 0))

            ' client details
            m_lClientId = gPMFunctions.ToSafeLong(m_vClaimDetails(kClaimDetailInsuredCnt, 0), 0)
            m_sClientName = CStr(m_vClaimDetails(kClaimDetailInsuredName, 0))
            m_lClientCurrencyId = gPMFunctions.ToSafeLong(m_vClaimDetails(kClaimDetailClientCurrencyId, 0), 0)
            m_bClientDomiciledForTax = gPMFunctions.ToSafeBoolean(m_vClaimDetails(kClaimDetailClientDomiciledForTax, 0), False)
            m_sClientTaxNumber = CStr(m_vClaimDetails(kClaimDetailClientTaxNumber, 0))
            m_crClientTaxPercentage = gPMFunctions.ToSafeCurrency(m_vClaimDetails(kClaimDetailClientTaxPercentage, 0), 0)
            m_bClientTaxExempt = gPMFunctions.ToSafeBoolean(m_vClaimDetails(kClaimDetailClientTaxExempt, 0), False)

            ' agent details
            m_lLeadAgentId = gPMFunctions.ToSafeLong(m_vClaimDetails(kClaimDetailLeadAgentCnt, 0), 0)
            m_sLeadAgentName = CStr(m_vClaimDetails(kClaimDetailAgentName, 0))
            m_lLeadAgentCurrencyId = gPMFunctions.ToSafeLong(m_vClaimDetails(kClaimDetailAgentCurrencyId, 0), 0)
            m_bLeadAgentDomiciledForTax = gPMFunctions.ToSafeBoolean(m_vClaimDetails(kClaimDetailAgentDomiciledForTax, 0), False)
            m_sLeadAgentTaxNumber = CStr(m_vClaimDetails(kClaimDetailAgentTaxNumber, 0))
            m_crLeadAgentTaxPercentage = gPMFunctions.ToSafeCurrency(m_vClaimDetails(kClaimDetailAgentTaxPercentage, 0), 0)
            m_bLeadAgentTaxExempt = gPMFunctions.ToSafeBoolean(m_vClaimDetails(kClaimDetailAgentTaxExempt, 0), False)

            ' agent in transfer details
            m_bAgentInTransfer = gPMFunctions.ToSafeBoolean(m_vClaimDetails(kClaimDetailAgentIsInTransferMode, 0), False)
            m_sAgentTransferBusinessType = CStr(m_vClaimDetails(kClaimDetailTransferToBusinessType, 0))
            m_lTransferAgentPartyCnt = gPMFunctions.ToSafeLong(m_vClaimDetails(kClaimDetailTransferToPartyCnt, 0), 0)
            m_bTransferAgentDomiciledForTax = gPMFunctions.ToSafeBoolean(m_vClaimDetails(kClaimDetailTransferAgentDomiciledForTax, 0), False)
            m_sTransferAgentTaxNumber = CStr(m_vClaimDetails(kClaimDetailTransferAgentTaxNumber, 0))
            m_crTransferAgentTaxPercentage = gPMFunctions.ToSafeCurrency(m_vClaimDetails(kClaimDetailTransferAgentTaxpercentage, 0), 0)
            m_bTransferAgentTaxExempt = gPMFunctions.ToSafeBoolean(m_vClaimDetails(kClaimDetailTransferAgentTaxExempt, 0), False)
            m_lTransferAgentCurrencyId = gPMFunctions.ToSafeLong(m_vClaimDetails(kClaimDetailTransferAgentCurrencyId, 0), 0)
            m_sTransferAgentName = CStr(m_vClaimDetails(kClaimDetailTransferAgentPartyName, 0))

            m_lClassOfBusinessId = gPMFunctions.ToSafeLong(m_vClaimDetails(kClaimDetailClassOfBusinessId, 0), 0)
            m_sClassOfBusinessCode = CStr(m_vClaimDetails(kClaimDetailClassOfBusinessCode, 0))
            m_lInsuranceFileCnt = gPMFunctions.ToSafeLong(m_vClaimDetails(kClaimDetailInsuranceFileCnt, 0), 0)

            m_bPostClaimTax = gPMFunctions.ToSafeLong(m_vClaimDetails(kClaimDetailPostClaimsTaxes, 0), 0)
            m_sClaimNumber = CStr(m_vClaimDetails(kClaimDetailClaimNumber, 0)).Trim()
            m_sPerilDescription = CStr(m_vClaimDetails(kClaimDetailPerilDescription, 0)).Trim()

            If g_sUnderwritingOrAgency = "A" Then
                m_sInsurer = gPMFunctions.ToSafeString(m_vClaimDetails(kClaimDetailInsurer, 0), "").Trim()
            End If

            txtRiskType.Text = m_sRisktype
            txtPeril.Text = m_sPerilDescription
            txtLossCurrency.Text = m_sLossCurrency
            txtLossDate.Text = m_dtLossDate.ToString("dd/MM/yyyy")



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function



    ' ***************************************************************** '
    ' Name: SetUpUserControl
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 08-08-2005 : 360 - Taxes On Claims
    ' ***************************************************************** '
    Private Function SetUpUserControl() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetUpUserControl"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim bVisible As Boolean

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' setup defaults for insured tax status frame
            lReturn = SetupDefaultInsuredTaxStatus()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "", gPMConstants.PMELogLevel.PMLogError)
            End If

            '   Start - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)
            ' setup default payment options
            '    lReturn = SetupPayeeInterfaceDefaults
            '    If lReturn <> PMTrue Then
            '        RaiseError kMethodName, "", PMLogError
            '    End If
            '   End - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)

            ' set up the advanced tax frames
            lReturn = SetupAdvancedTaxFrames()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetupAvailableFrames  Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' only display the this receipt tab
            ' when receipts have been made in this session
            lReturn = SetupThisReceiptInterface()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetupThisReceiptInterface", gPMConstants.PMELogLevel.PMLogError)
            End If

            'S4B Claim Enhancements R&D 2005
            If g_sUnderwritingOrAgency = "A" Then
                SSTabHelper.SetTabVisible(SSTab1, kTabReinsurance, False)
                SSTabHelper.SetTabCaption(SSTab1, kTabThisReceipt, "&3 - This Receipt")
            End If

            If g_sUnderwritingOrAgency = "A" Then
                If m_sInsurer.ToUpper() <> "MULTI" Then
                    SSTabHelper.SetTabVisible(SSTab1, kTabCoinsurance, False)
                End If
            End If
            ''Start(Saurabh Agrawal) TechSpec QBENZCR004 Claims Recovery Reinsurance
            If m_bRI2007Enabled Then
                SSTabHelper.SetTabVisible(SSTab1, kTabReinsurance, False)
                SSTabHelper.SetTabVisible(SSTab1, kTabCoinsurance, False)
            Else
                SSTabHelper.SetTabVisible(SSTab1, kTabReinsurance, True)
                SSTabHelper.SetTabVisible(SSTab1, kTabCoinsurance, True)
            End If
            ''End(Saurabh Agrawal) TechSpec QBENZCR004 Claims Recovery Reinsurance

            ' on load disable the edit and history buttons
            ' this should only be enabled when an item has been
            ' selected from the receipt details list
            cmdEdit.Enabled = False
            cmdDelete.Enabled = False



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SelectcboItem
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 08-08-2005 : 360 - Taxes On Claims
    ' ***************************************************************** '
    Private Function SelectcboItem(ByRef r_oCbo As ComboBox, ByVal v_lSelectedId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SelectcboItem"

        Dim bItemNotFound As Boolean

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            bItemNotFound = True

            ' if the item id is valid
            If v_lSelectedId <> 0 Then

                ' for each item in the list
                For lItem As Integer = 0 To r_oCbo.Items.Count

                    ' search the item data array for a match
                    If VB6.GetItemData(r_oCbo, lItem) = v_lSelectedId Then

                        ' found a match - select the item
                        r_oCbo.SelectedIndex = lItem
                        bItemNotFound = False
                        Exit For
                    End If

                Next lItem

                If bItemNotFound Then

                    ' log that we havent found the specified item
                    Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                    oDict.Add("v_lSelectedId", v_lSelectedId)
                    gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=kMethodName & " Failed to find item with id:" & CStr(v_lSelectedId) & " in :" & r_oCbo.Name, vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, oDicParms:=oDict)

                End If

            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetClaimDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 08-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function GetClaimDetails() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimDetails"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            lReturn = m_oBusiness.GetClaimDetails(v_lClaimId:=m_lClaimId, v_lClaimPerilId:=m_lClaimPerilId, r_vResults:=m_vClaimDetails)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetClaimDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not Information.IsArray(m_vClaimDetails) Then
                gPMFunctions.RaiseError(kMethodName, "GetClaimDetails Failed to return any data", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ActionEdit
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 08-08-2005 : 360 - Taxes On Claims
    ' ***************************************************************** '
    Private Function ActionEdit() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ActionEdit"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lReceiptItemArrayPos As Integer
        Dim ofrmReceiptDetails As frmReceiptDetails
        Dim oListItem As ListViewItem
        Dim oReceiptItem As cReceiptItem
        Dim sRecoveryId As String = ""
        Dim crTotalReserve, crReceivedToDate, crBalance, crThisReceiptInclTax, crThisReceiptTax, crCostToClaim As Decimal
        Dim dReceiptToLossXRate As Integer
        Dim sRecoveryTypeDesc As String = ""
        Dim ofrmEditWarning As frmEditWarning
        Dim lDisplayMode As Integer
        Dim bWarningingShown As Boolean
        Dim lPaymentCurrencyFilter, lRecoveryTypeId As Integer
        Dim oTaxItem As cTaxParameters
        Dim iPos As Integer
        Dim bPartyNewlyAdded As Boolean
        Dim lRecoverId As Integer
        Dim sResult As String = ""

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            ' get the selected list item
            oListItem = lvwRecovery.Items.Item(m_lSelectedPayeeDetailIndex - 1)

            'ATS
            If CheckMandatory() <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' get the payment items reserve details
            sRecoveryId = oListItem.Text
            sRecoveryTypeDesc = ListViewHelper.GetListViewSubItem(oListItem, kRecDetSubItemsRecoveryTypeDesc).Text
            lRecoveryTypeId = gPMFunctions.ToSafeLong(ListViewHelper.GetListViewSubItem(oListItem, kRecDetSubItemsRecoveryTypeId).Text, 0)

            ' get the payment item
            lReturn = CType(GetReceiptItem(v_sRecoveryId:=sRecoveryId, r_oReceiptItem:=oReceiptItem), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetReceiptItem Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get the default tax item
            lReturn = CType(GetDefaultTaxItem(r_oTaxItem:=oTaxItem), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetTaxItem", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' if this item doesnt already have a currency set
            ' i.e it is a new item not an existing one.
            ' get the payment currency filter if there is one
            ' and default the currency xchanges rates for the new item
            lReturn = CType(GetPaymentCurrencyFilter(lPaymentCurrencyFilter, oReceiptItem), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetPaymentCurrencyFilter Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' determine whether it is necessary to display the warning form
            If oReceiptItem.AdvancedTaxScript <> "" And m_bAdvancedTaxScriptingOption Then

                ' create new instance of warning form
                ofrmEditWarning = New frmEditWarning()

                ' display warning form
                ofrmEditWarning.ShowDialog()

                ' get the display mode to use
                lDisplayMode = ofrmEditWarning.DisplayMode

                ' show edit warning
                bWarningingShown = True
            Else

                lDisplayMode = gPMConstants.PMEComponentAction.PMEdit

            End If

            ' if the user hasnt specified to cancel then
            Dim counter As Integer
            If lDisplayMode <> gPMConstants.PMEReturnCode.PMCancel Then

                ' save the recovery details to the receipt item
                If oReceiptItem.RecoveryTypeDesc = "" Then
                    oReceiptItem.RecoveryTypeDesc = sRecoveryTypeDesc
                    oReceiptItem.RecoveryTypeId = lRecoveryTypeId
                End If

                ' create new instance of payment details form
                ofrmReceiptDetails = New frmReceiptDetails()

                '********************
                ' interface details
                '********************
                ofrmReceiptDetails.PaymentMethod = m_oPaymentMethod
                ofrmReceiptDetails.CurrencyConvert = m_oCurrencyConvert
                ofrmReceiptDetails.ReceiptItem = oReceiptItem
                ofrmReceiptDetails.TaxItem = oTaxItem
                ofrmReceiptDetails.Business = m_oBusiness

                ' lookups


                ofrmReceiptDetails.TaxBandLookup = m_vTaxBand
                ofrmReceiptDetails.TaxGroupLookup = VB6.CopyArray(m_vTaxGroup)
                ofrmReceiptDetails.TaxGroupTaxBandLookup = VB6.CopyArray(m_vTaxGroupTaxBandDetails)


                ofrmReceiptDetails.ClassOfBusinessLookup = m_vClassOfBusiness

                ' display mode
                ofrmReceiptDetails.IsSalvageRecovery = m_bIsSalvage
                ofrmReceiptDetails.ViewReceiptMode = (lDisplayMode = gPMConstants.PMEComponentAction.PMView)

                ' system options
                'ofrmReceiptDetails.AllowNegativeReserve = m_bAllowNegativeReserve
                ofrmReceiptDetails.AdvancedTaxScriptOptionOn = m_bAdvancedTaxScriptingOption

                ofrmReceiptDetails.PaymentCurrencyFilter = lPaymentCurrencyFilter
                ofrmReceiptDetails.UserId = m_iUserId
                ofrmReceiptDetails.TaxGroupArray = VB6.CopyArray(m_vTaxGroup)
                ofrmReceiptDetails.CurrencyArray = VB6.CopyArray(m_vCurrencyArray)
                ofrmReceiptDetails.ClaimPerilId = m_lClaimPerilId

                '********************
                ' Recovery Details
                '********************
                ofrmReceiptDetails.TotalReserve = CDec(ListViewHelper.GetListViewSubItem(oListItem, kRecDetSubItemsTotalReserve).Text)
                ofrmReceiptDetails.RecoveredToDate = CDec(ListViewHelper.GetListViewSubItem(oListItem, kRecDetSubItemsRecoveredTotal).Text)
                ofrmReceiptDetails.Balance = ofrmReceiptDetails.TotalReserve - ofrmReceiptDetails.RecoveredToDate

                'Start - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)
                ofrmReceiptDetails.PayeeDetails = VB6.CopyArray(m_vAttachPartyToRecovery)
                'End - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)

                ' also populate the payment items reserve totals
                oReceiptItem.TotalReserve = CDec(ListViewHelper.GetListViewSubItem(oListItem, kRecDetSubItemsTotalReserve).Text)
                oReceiptItem.ReceivedToDate = CDec(ListViewHelper.GetListViewSubItem(oListItem, kRecDetSubItemsRecoveredTotal).Text)
                oReceiptItem.Balance = ofrmReceiptDetails.TotalReserve - ofrmReceiptDetails.RecoveredToDate

                ' Start - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc) - (4.3.3.1.2)
                If Information.IsArray(m_vAttachPartyToRecovery) Then
                    iPos = m_vAttachPartyToRecovery.GetUpperBound(1)
                    counter = iPos
                    For iCounter As Integer = 0 To counter
                        If m_vAttachPartyToRecovery(kNewPartyRecoveryId, iCounter).Equals(m_vRecoveryDetails(kRecoveryDetailsRecoveryId, lvwRecovery.FocusedItem.Index + 1 - 2)) Then
                            iPos = iCounter
                            bPartyNewlyAdded = True
                            Exit For
                        End If
                    Next
                    If bPartyNewlyAdded Then
                        oReceiptItem.RecoveryPartyTypeId = CInt(m_vAttachPartyToRecovery(kNewPartyPartyTypeId, iPos))
                        oReceiptItem.RecoveryPartyCnt = CInt(m_vAttachPartyToRecovery(kNewPartyPartyID, iPos))
                    Else
                        If CStr(m_vRecoveryDetails(kRecoveryDetailsClaimsPartyType, lvwRecovery.FocusedItem.Index + 1 - 2)) <> "" Then
                            oReceiptItem.RecoveryPartyTypeId = CInt(m_vRecoveryDetails(kRecoveryDetailsClaimsPartyType, lvwRecovery.FocusedItem.Index + 1 - 2))
                        End If
                        If CStr(m_vRecoveryDetails(kRecoveryDetailsClaimsPartyCnt, lvwRecovery.FocusedItem.Index + 1 - 2)) <> "" Then
                            oReceiptItem.RecoveryPartyCnt = CInt(m_vRecoveryDetails(kRecoveryDetailsClaimsPartyCnt, lvwRecovery.FocusedItem.Index + 1 - 2))
                        End If
                    End If
                Else
                    If CStr(m_vRecoveryDetails(kRecoveryDetailsClaimsPartyType, lvwRecovery.FocusedItem.Index + 1 - 2)) <> "" Then
                        oReceiptItem.RecoveryPartyTypeId = CInt(m_vRecoveryDetails(kRecoveryDetailsClaimsPartyType, lvwRecovery.FocusedItem.Index + 1 - 2))
                    End If
                    If CStr(m_vRecoveryDetails(kRecoveryDetailsClaimsPartyCnt, lvwRecovery.FocusedItem.Index + 1 - 2)) <> "" Then
                        oReceiptItem.RecoveryPartyCnt = CInt(m_vRecoveryDetails(kRecoveryDetailsClaimsPartyCnt, lvwRecovery.FocusedItem.Index + 1 - 2))
                    End If
                End If
                ' End - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc) - (4.3.3.1.2)

                '********************
                ' Claim Details
                '********************
                ofrmReceiptDetails.LossCurrency = m_sLossCurrency
                ofrmReceiptDetails.LossCurrencyID = m_lLossCurrencyId
                ofrmReceiptDetails.RiskType = m_sRisktype
                ofrmReceiptDetails.RecoveryType = ListViewHelper.GetListViewSubItem(oListItem, kRecDetSubItemsRecoveryTypeDesc).Text
                ofrmReceiptDetails.ClaimSourceId = m_lClaimSourceId
                ofrmReceiptDetails.ClaimID = m_lClaimId
                ofrmReceiptDetails.ClaimReceivableAccountId = m_lClaimReceivableAccountId
                ofrmReceiptDetails.PayeePartyId = m_lSelectedPayeeId

                '********************
                ' Receipt Item details
                '********************
                ofrmReceiptDetails.RecoveryId = CInt(sRecoveryId)
                ofrmReceiptDetails.ReceiptToLossXRate = oReceiptItem.ReceiptToLossXRate
                ofrmReceiptDetails.CurrencyId = oReceiptItem.CurrencyId
                ofrmReceiptDetails.ThisReceipt = oReceiptItem.ThisReceipt
                ofrmReceiptDetails.TaxGroupId = oReceiptItem.TaxGroupId


                ofrmReceiptDetails.TaxBandRateArray = oReceiptItem.TaxBandRateArray
                ofrmReceiptDetails.ScriptedTaxAmount = oReceiptItem.ScriptedTaxAmount

                ofrmReceiptDetails.TaxAmount = oReceiptItem.TaxAmount - oReceiptItem.ScriptedTaxAmount

                ofrmReceiptDetails.TaxGroupDescription = oReceiptItem.TaxGroupDescription
                ofrmReceiptDetails.CurrencyDescription = oReceiptItem.CurrencyDescription

                ' reset the tax details
                ' if the warning form was shown and the user selected to continue
                ' with editing the item
                If lDisplayMode = gPMConstants.PMEComponentAction.PMEdit And bWarningingShown Then
                    ofrmReceiptDetails.TaxAmount = 0
                    ofrmReceiptDetails.ScriptedTaxAmount = 0
                    ofrmReceiptDetails.TaxGroupDescription = ""
                    ofrmReceiptDetails.TaxGroupId = 0

                    ofrmReceiptDetails.TaxBandRateArray = ""
                End If

                ' display form
                ofrmReceiptDetails.ShowDialog()

                ' if the user confirms the details on the payment item detail form
                If ofrmReceiptDetails.Status = gPMConstants.PMEReturnCode.PMOK Then

                    'Start - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)
                    If bPartyNewlyAdded Then
                        m_vRecoveryDetails(kRecoveryDetailsClaimsPartyType, lvwRecovery.FocusedItem.Index + 1 - 2) = oReceiptItem.RecoveryPartyTypeId
                        m_vRecoveryDetails(kRecoveryDetailsClaimsPartyCnt, lvwRecovery.FocusedItem.Index + 1 - 2) = oReceiptItem.RecoveryPartyCnt
                        m_vRecoveryDetails(kRecoveryDetailsClaimsShortName, lvwRecovery.FocusedItem.Index + 1 - 2) = m_vAttachPartyToRecovery(kNewPartyShortName, iPos)
                        m_vRecoveryDetails(kRecoveryDetailsClaimsResolvedName, lvwRecovery.FocusedItem.Index + 1 - 2) = m_vAttachPartyToRecovery(kNewPartyLongName, iPos)

                        lReturn = CType(RemoveAttachedParty(CInt(m_vAttachPartyToRecovery(kNewPartyRecoveryId, iPos))), gPMConstants.PMEReturnCode)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "RemoveAttachedParty Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                        'Developer Guide no. 52
                        lvwRecovery.FocusedItem.SubItems(kRecDetSubItemsRecoveryPartyName).Text = CStr(m_vRecoveryDetails(kRecoveryDetailsClaimsResolvedName, lvwRecovery.FocusedItem.Index + 1 - 2))
                        DisableControls()
                    End If
                    'End - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)

                    ' disable payee details frame
                    lReturn = CType(EnableDisablePayeeDetails(v_bEnabled:=False), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "EnableDisablePayeeDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    oReceiptItem.ThisReceipt = ofrmReceiptDetails.ThisReceipt
                    oReceiptItem.CurrencyCode = ofrmReceiptDetails.CurrencyCode
                    oReceiptItem.CurrencyId = ofrmReceiptDetails.CurrencyId
                    oReceiptItem.TaxGroupDescription = ofrmReceiptDetails.TaxGroupDescription
                    oReceiptItem.CurrencyDescription = ofrmReceiptDetails.CurrencyDescription

                    ' if this is the first item to be created then
                    ' get exchanges rates from the form
                    ' otherwise keep the default values set when adding the new item
                    If lPaymentCurrencyFilter = 0 Then
                        oReceiptItem.AccountToBaseDate = ofrmReceiptDetails.AccountToBaseDate
                        oReceiptItem.AccountToBaseXRate = ofrmReceiptDetails.AccountToBaseXRate
                        oReceiptItem.CurrencyToBaseDate = ofrmReceiptDetails.CurrencyToBaseDate
                        oReceiptItem.CurrencyToBaseXRate = ofrmReceiptDetails.CurrencyToBaseXRate
                        oReceiptItem.ReceiptToLossXRate = ofrmReceiptDetails.ReceiptToLossXRate
                        oReceiptItem.SystemToBaseDate = ofrmReceiptDetails.SystemToBaseDate
                        oReceiptItem.SystemToBaseXRate = ofrmReceiptDetails.SystemToBaseXRate
                        oReceiptItem.ExchangeRateOverrideReasonId = ofrmReceiptDetails.ExchangeRateOverrideReasonId
                    End If


                    oReceiptItem.RecoveryId = ofrmReceiptDetails.RecoveryId

                    ' save the details back to the payment item object
                    oReceiptItem.TaxGroupId = ofrmReceiptDetails.TaxGroupId



                    oReceiptItem.TaxBandRateArray = ofrmReceiptDetails.TaxBandRateArray

                    ' get the advanced tax script
                    oReceiptItem.AdvancedTaxScript = ofrmReceiptDetails.AdvancedTaxScript

                    oReceiptItem.TaxAmount = ofrmReceiptDetails.TaxAmount

                    lReturn = oReceiptItem.RecalculateTaxAmounts()
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "RecalculateTaxAmounts Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    ' update the receipt details on the the listview
                    lReturn = CType(UpdateReceiptDetailListView(oReceiptItem, oListItem), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "PopulatePaymentDetailThisReceipt Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    ' populate the "this receipt" tab details
                    lReturn = PopulateThisReceiptDetails()
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "PopulateThisReceiptDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    ' populate total line
                    lReturn = PopulateTotals()
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "PopulateTotals Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    ' recalculate coinsurance / reinsurance
                    lReturn = CType(RecalculateInsurance(v_oReceipt:=oReceiptItem), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "RecalculateCoReinsurance Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If



                    ' set up this payment form
                    lReturn = SetupThisReceiptInterface()
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "SetupThisReceiptInterface", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    lReturn = CType(ActionSelectReceiptItem(lvwRecovery.FocusedItem), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "ActionSelectReceiptItem Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                End If

            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' destroy payment details form
            '    Set ofrmEditWarning = Nothing
            '    Set ofrmReceiptDetails = Nothing



        End Try
        Return result
    End Function



    ' ***************************************************************** '
    ' Name: PopulateCombo
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 09-08-2005 : 260 - Taxes on Claims
    ' ***************************************************************** '
    Private Function PopulateCombo(ByRef r_oComboBox As ComboBox, ByVal v_vValuesArray(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulateCombo"

        Dim lReturn As Integer
        Dim sDescription As String = ""
        Dim lItemId As Integer
        Dim sCode As String = ""
        Dim llBound, lUBound As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            r_oComboBox.Items.Clear()

            If Not Information.IsArray(v_vValuesArray) Then
                gPMFunctions.RaiseError(kMethodName, "PopulateCombo Failed - Value Array not Array", gPMConstants.PMELogLevel.PMLogError)
            End If

            llBound = v_vValuesArray.GetLowerBound(1)
            lUBound = v_vValuesArray.GetUpperBound(1)

            For lItem As Integer = llBound To lUBound


                lItemId = CInt(v_vValuesArray(kLookupItemId, lItem))

                sDescription = CStr(v_vValuesArray(kLookupDescription, lItem))

                r_oComboBox.Items.Insert(lItem, sDescription)
                VB6.SetItemData(r_oComboBox, lItem, lItemId)

            Next



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: PopulateLookups
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 09-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function PopulateLookups() As Integer

        Dim nResult As Integer = 0
        Const kMethodName As String = "PopulateLookups"

        Dim nReturn As gPMConstants.PMEReturnCode

        Try


            nResult = gPMConstants.PMEReturnCode.PMTrue

            '    lReturn = PopulateCombo(cboClaimPaymentTo, m_vClaimPaymentTo)
            '    If lReturn <> PMTrue Then
            '        RaiseError kMethodName, "PopulateCombo = ClaimPaymentTo Failed", PMlogError
            '    End If

            nReturn = CType(PopulateCombo(cboCountry, m_vCountry), gPMConstants.PMEReturnCode)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "PopulateCombo - Country Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If IsArray(m_vMediaType) Then
                nReturn = CType(PopulateCombo(cboMediaType, m_vMediaType), gPMConstants.PMEReturnCode)
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "PopulateCombo - Media Type Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If
            '    lReturn = PopulateCombo(cboSafeHarbour, m_vSafeHarbour)
            '    If lReturn <> PMTrue Then
            '        RaiseError kMethodName, "PopulateCombo - Safe Harbour Failed", PMlogError
            '    End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
        End Try
        Return nResult
    End Function

    ' ***************************************************************** '
    ' Name: PopulateReceiptDetailsListView
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 09-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function PopulateReceiptDetailsListView() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulateReceiptDetailsListView"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lType, lTypeId, llBound, lUBound As Integer
        Dim oListItem As ListViewItem
        Dim lRecoveryId, lClaimPerilId, lRecoveryTypeId As Integer
        Dim sRecoveryTypeDescription As String = ""
        Dim lCurrencyId As Integer
        Dim sCurrencyDesc As String = ""
        Dim crInitialReserve, crRevisedReserve, crReceivedToDate As Decimal
        Dim lRevisionCount As Integer
        Dim crTaxAmount As Decimal
        Dim lClaimID, lClaimsIsPostTaxes As Integer
        Dim crTotalReserve As Decimal

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            lvwRecovery.Items.Clear()

            ' determine array boundaries
            llBound = m_vRecoveryDetails.GetLowerBound(1)
            lUBound = m_vRecoveryDetails.GetUpperBound(1)

            ' add default (All) item to coinsurance / reinsurance filters
            cboRecoveryFilter(kCoinsuranceFilter).Items.Insert(0, "(All)")
            cboRecoveryFilter(kReinsuranceFilter).Items.Insert(0, "(All)")

            ' for each reserve / recovery type
            For lItem As Integer = llBound To lUBound

                If lItem = llBound Then

                    ' add the total list item
                    oListItem = lvwRecovery.Items.Add(CStr(0))

                    ' populate reserve description
                    ListViewHelper.GetListViewSubItem(oListItem, kRecDetSubItemsRecoveryTypeDesc).Text = GetResString(kResDetailsTotal)

                End If

                ' get recovery details
                lRecoveryId = gPMFunctions.ToSafeLong(m_vRecoveryDetails(kRecoveryDetailsRecoveryId, lItem), 0)
                lClaimPerilId = gPMFunctions.ToSafeLong(m_vRecoveryDetails(kRecoveryDetailsClaimPerilId, lItem), 0)
                lRecoveryTypeId = gPMFunctions.ToSafeLong(m_vRecoveryDetails(kRecoveryDetailsRecoveryTypeId, lItem), 0)
                sRecoveryTypeDescription = CStr(m_vRecoveryDetails(kRecoveryDetailsRecoveryTypeDescription, lItem)).Trim()
                lCurrencyId = gPMFunctions.ToSafeLong(m_vRecoveryDetails(kRecoveryDetailsCurrencyId, lItem), 0)
                sCurrencyDesc = CStr(m_vRecoveryDetails(kRecoveryDetailsCurrencyDescription, lItem)).Trim()
                crInitialReserve = gPMFunctions.ToSafeCurrency(m_vRecoveryDetails(kRecoveryDetailsInitialReserve, lItem), 0)
                crRevisedReserve = gPMFunctions.ToSafeCurrency(m_vRecoveryDetails(kRecoveryDetailsRevisedReserve, lItem), 0)
                crReceivedToDate = gPMFunctions.ToSafeCurrency(m_vRecoveryDetails(kRecoveryDetailsReceivedToDate, lItem), 0)
                lRevisionCount = gPMFunctions.ToSafeLong(m_vRecoveryDetails(kRecoveryDetailsRevisionCount, lItem), 0)
                crTaxAmount = gPMFunctions.ToSafeCurrency(m_vRecoveryDetails(kRecoveryDetailsTaxAmount, lItem), 0)
                lClaimID = gPMFunctions.ToSafeLong(m_vRecoveryDetails(kRecoveryDetailsClaimId, lItem), 0)
                lClaimsIsPostTaxes = gPMFunctions.ToSafeLong(m_vRecoveryDetails(kRecoveryDetailsClaimsIsPostTaxes, lItem), 0)
                crTotalReserve = crInitialReserve + crRevisedReserve

                ' add list item
                oListItem = lvwRecovery.Items.Add(CStr(lRecoveryId))

                ' populate list sub item details
                ListViewHelper.GetListViewSubItem(oListItem, kRecDetSubItemsRecoveryTypeDesc).Text = sRecoveryTypeDescription
                'Start - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc) - (4.3.3.1.1)
                ListViewHelper.GetListViewSubItem(oListItem, kRecDetSubItemsRecoveryPartyName).Text = CStr(m_vRecoveryDetails(kRecoveryDetailsClaimsResolvedName, lItem))
                'End - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc) - (4.3.3.1.1)
                ListViewHelper.GetListViewSubItem(oListItem, kRecDetSubItemsTotalReserve).Text = StringsHelper.Format(crTotalReserve, "0.00")
                ListViewHelper.GetListViewSubItem(oListItem, kRecDetSubItemsRecoveredTotal).Text = StringsHelper.Format(crReceivedToDate, "0.00")
                ListViewHelper.GetListViewSubItem(oListItem, kRecDetSubItemsThisReceipt).Text = "0.00"
                ListViewHelper.GetListViewSubItem(oListItem, kRecDetSubItemsTaxAmount).Text = "0.00"
                ListViewHelper.GetListViewSubItem(oListItem, kRecDetSubItemsThisNet).Text = "0.00"
                ListViewHelper.GetListViewSubItem(oListItem, kRecDetSubItemsBalance).Text = StringsHelper.Format(crTotalReserve - crReceivedToDate, "0.00")
                ListViewHelper.GetListViewSubItem(oListItem, kRecDetSubItemsRecoveryTypeId).Text = CStr(lRecoveryTypeId)


                lRecoveryTypeId = gPMFunctions.ToSafeLong(m_vRecoveryDetails(kRecoveryDetailsRecoveryTypeId, lItem), 0)

                oListItem.Tag = CStr(lRecoveryId)

                ' add recovery to coinsurance and reinsurance filters
                'Developer Guide no. 153
                'start
                cboRecoveryFilter(kCoinsuranceFilter).Items.Add(New VB6.ListBoxItem(Trim(sRecoveryTypeDescription), lRecoveryId))
                cboRecoveryFilter(kReinsuranceFilter).Items.Add(New VB6.ListBoxItem(Trim(sRecoveryTypeDescription), lRecoveryId))
                'end
            Next
            'Start - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)
            DisableControls()
            'End - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)

            ' populate total line
            lReturn = PopulateTotals()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "PopulateTotals Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' default reinsurance filter to (All)
            cboRecoveryFilter(kCoinsuranceFilter).SelectedIndex = 0
            cboRecoveryFilter(kReinsuranceFilter).SelectedIndex = 0



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SetupReceiptDetailsListView
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 09-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function SetupReceiptDetailsListView() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupReceiptDetailsListView"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            lvwRecovery.Columns.Clear()

            ' hidden column
            lvwRecovery.Columns.Insert(kRecDetColHIndexRecoveryId - 1, kRecDetColHCodeRecoveryId, "", CInt(VB6.TwipsToPixelsX(0)), HorizontalAlignment.Left, -1)

            lvwRecovery.Columns.Insert(kRecDetColHIndexRecoveryTypeDesc - 1, kRecDetColHCodeRecoveryTypeDesc, GetResString(kResDetailsRecoveryType), CInt(VB6.TwipsToPixelsX(1920)), HorizontalAlignment.Left, -1)
            'Start - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)
            lvwRecovery.Columns.Insert(kRecDetColHIndexRecoveryPartyName - 1, kRecDetColHCodeRecoveryPartyName, GetResString(kResDetailsRecoveryParty), CInt(VB6.TwipsToPixelsX(1920)), HorizontalAlignment.Left, -1)
            'End - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)
            lvwRecovery.Columns.Insert(kRecDetColHIndexTotalReserve - 1, kRecDetColHCodeTotalReserve, GetResString(kResDetailsTotalReserve), CInt(VB6.TwipsToPixelsX(1766)), HorizontalAlignment.Right, -1)

            lvwRecovery.Columns.Insert(kRecDetColHIndexRecoveredTotal - 1, kRecDetColHCodeRecoveredTotal, GetResString(kResDetailsRecoveredTotal), CInt(VB6.TwipsToPixelsX(1766)), HorizontalAlignment.Right, -1)

            lvwRecovery.Columns.Insert(kRecDetColHIndexThisReceipt - 1, kRecDetColHCodeThisReceipt, GetResString(kResDetailsThisReceipt), CInt(VB6.TwipsToPixelsX(1766)), HorizontalAlignment.Right, -1)

            lvwRecovery.Columns.Insert(kRecDetColHIndexTaxAmount - 1, kRecDetColHCodeTaxAmount, GetResString(kResDetailsTaxAmount), CInt(VB6.TwipsToPixelsX(1766)), HorizontalAlignment.Right, -1)

            lvwRecovery.Columns.Insert(kRecDetColHIndexThisNet - 1, kRecDetColHCodeThisNet, GetResString(kResDetailsThisNet), CInt(VB6.TwipsToPixelsX(1766)), HorizontalAlignment.Right, -1)

            lvwRecovery.Columns.Insert(kRecDetColHIndexBalance - 1, kRecDetColHCodeBalance, GetResString(kResDetailsBalance), CInt(VB6.TwipsToPixelsX(1766)), HorizontalAlignment.Right, -1)

            ' hidden column
            lvwRecovery.Columns.Insert(kRecDetColHIndexRecoveryTypeId - 1, kRecDetColHCodeRecoveryTypeId, "", CInt(VB6.TwipsToPixelsX(0)), HorizontalAlignment.Right, -1)



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetResString
    '
    ' Parameters: n/a
    '
    ' Description: Returns string item from resource file
    '
    ' History:
    '           Created : MEvans : 09-08-2005 : 360 - Taxes On Claims
    ' ***************************************************************** '
    Private Function GetResString(ByVal v_lItemId As Integer) As String

        Const kMethodName As String = "SetupReceiptDetailsListView"

        Dim sReturn As String = ""

        Try



            ' always want to return a string

            sReturn = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=v_lItemId, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))




        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=SetupReceiptDetailsListView(), excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally


        End Try
        Return sReturn
    End Function

    Private Sub cboRecoveryFilter_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cboRecoveryFilter_1.SelectedIndexChanged, _cboRecoveryFilter_0.SelectedIndexChanged
        Dim Index As Integer = Array.IndexOf(cboRecoveryFilter, eventSender)
        PopulateInsurerListView(Index = 0, VB6.GetItemData(cboRecoveryFilter(Index), cboRecoveryFilter(Index).SelectedIndex))
    End Sub

    Private Sub chkITDomiciled_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkITDomiciled.CheckStateChanged
        SetupInsuredTaxAdjustments()
    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click
        ActionDelete()
    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click
        ActionEdit()
    End Sub

    Private Sub cmdEditPayee_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEditPayee.Click
        EnableDisablePayeeDetails(v_bEnabled:=True)
    End Sub

    Private Sub cmdParty_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdParty.Click
        ActionFindParty()
    End Sub

    Private Sub lvwTable_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwRecovery.SelectedIndexChanged
        ActionSelectReceiptItem(lvwRecovery.FocusedItem)
    End Sub
    ''' <summary>
    ''' ActionSelectReceiptItem
    ''' </summary>
    ''' <param name="r_oListItem"></param>
    ''' <returns></returns>
    ''' <remarks>Created : MEvans : 10-08-2005 : 360 - Taxes on Claims</remarks>
    Private Function ActionSelectReceiptItem(ByRef r_oListItem As ListViewItem) As Integer

        Dim nResult As Integer = 0
        Const kMethodName As String = "ActionSelectReceiptItem"

        Dim nReturn As gPMConstants.PMEReturnCode
        Dim sPartyType, sShortName As String
        Dim bNewlyAdded As Boolean

        Try



            nResult = gPMConstants.PMEReturnCode.PMTrue

            If Not m_bViewReceiptMode Then
                ' Start - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)
                If r_oListItem.Index + 1 > 1 Then

                    sPartyType = CStr(m_vRecoveryDetails(kRecoveryDetailsClaimsPartyType, lvwRecovery.FocusedItem.Index + 1 - 2))
                    sShortName = CStr(m_vRecoveryDetails(kRecoveryDetailsClaimsShortName, lvwRecovery.FocusedItem.Index + 1 - 2))
                    If sShortName <> "" Then
                        m_bPartyAlreadyAttached = True
                        txtParty.Text = sShortName
                        DisableControls()
                    Else
                        m_bPartyAlreadyAttached = False
                        EnableControls()
                    End If
                    If sPartyType = "" Then
                        '-------------------------------------------------------------------------
                        'The function is called to set the values in the form if the party is not
                        'attached during open claim/Maintain claim and the user have attached now.
                        '-------------------------------------------------------------------------
                        nReturn = CType(SetNewlyAttachedPartyValues(bNewlyAdded), gPMConstants.PMEReturnCode)
                        If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "SetNewlyAttachedPartyValues Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                        If Not bNewlyAdded Then
                            OptClaimReceivable.Checked = True
                            cmdParty.Enabled = False
                            m_sSelectedPayee = kAccountCLMRECEIVABLE
                            txtParty.Text = kAccountCLMRECEIVABLE

                            'The function is called to set the values of account id and currency id
                            nReturn = CType(GetAccountDetailsByShortCode(), gPMConstants.PMEReturnCode)
                            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError(kMethodName, "GetAccountDetailsByShortCode Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If
                        End If
                    ElseIf StringsHelper.ToDoubleSafe(sPartyType) = kPartyTypeAgent Then
                        OptAgent.Checked = True
                        m_sSelectedPayee = kAccountCLMAgent
                        m_lClaimReceivableCurrencyId = CInt(m_vRecoveryDetails(kRecoveryDetailsCurrencyId, lvwRecovery.FocusedItem.Index + 1 - 2))
                        m_lClaimReceivableAccountId = CInt(m_vRecoveryDetails(kRecoveryDetailsClaimsPartyCnt, lvwRecovery.FocusedItem.Index + 1 - 2))
                        If m_bAgentInTransfer Then
                            If m_sAgentTransferBusinessType <> "DIRECT" Then
                                m_lSelectedPayeeAccountCurrencyId = m_lTransferAgentCurrencyId
                                m_lSelectedPayeeId = m_lTransferAgentPartyCnt
                            End If
                        Else
                            m_lSelectedPayeeAccountCurrencyId = m_lLeadAgentCurrencyId
                            m_lSelectedPayeeId = m_lLeadAgentId
                        End If
                    ElseIf StringsHelper.ToDoubleSafe(sPartyType) = kPartyTypeClient Then
                        OptClient.Checked = True
                        m_sSelectedPayee = kAccountCLMClient
                        m_lClaimReceivableCurrencyId = CInt(m_vRecoveryDetails(kRecoveryDetailsCurrencyId, lvwRecovery.FocusedItem.Index + 1 - 2))
                        m_lClaimReceivableAccountId = CInt(m_vRecoveryDetails(kRecoveryDetailsClaimsPartyCnt, lvwRecovery.FocusedItem.Index + 1 - 2))
                        m_lSelectedPayeeAccountCurrencyId = m_lClientCurrencyId
                        m_lSelectedPayeeId = m_lClaimReceivableAccountId
                    ElseIf StringsHelper.ToDoubleSafe(sPartyType) = kPartyTypeInsurer Then
                        OptInsurer.Checked = True
                        m_sSelectedPayee = kAccountCLMInsurer
                        m_lClaimReceivableCurrencyId = CInt(m_vRecoveryDetails(kRecoveryDetailsCurrencyId, lvwRecovery.FocusedItem.Index + 1 - 2))
                        m_lClaimReceivableAccountId = CInt(m_vRecoveryDetails(kRecoveryDetailsClaimsPartyCnt, lvwRecovery.FocusedItem.Index + 1 - 2))
                        m_lSelectedPayeeAccountCurrencyId = m_lClientCurrencyId
                        m_lSelectedPayeeId = CInt(m_vRecoveryDetails(kRecoveryDetailsClaimsPartyCnt, lvwRecovery.FocusedItem.Index + 1 - 2))
                    ElseIf StringsHelper.ToDoubleSafe(sPartyType) = kPartyTypeParty Then
                        OptParty.Checked = True
                        m_sSelectedPayee = kAccountCLMParty
                        m_lClaimReceivableCurrencyId = CInt(m_vRecoveryDetails(kRecoveryDetailsCurrencyId, lvwRecovery.FocusedItem.Index + 1 - 2))
                        m_lClaimReceivableAccountId = CInt(m_vRecoveryDetails(kRecoveryDetailsClaimsPartyCnt, lvwRecovery.FocusedItem.Index + 1 - 2))
                        m_lSelectedPayeeAccountCurrencyId = m_lClientCurrencyId
                        m_lSelectedPayeeId = CInt(m_vRecoveryDetails(kRecoveryDetailsClaimsPartyCnt, lvwRecovery.FocusedItem.Index + 1 - 2))
                    End If
                ElseIf r_oListItem.Index + 1 = 1 Then
                    DisableControls()
                    txtParty.Text = ""
                End If
                ' End - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)
                If m_sSelectedPayee <> "" Then
                    m_lSelectedPayeeDetailIndex = r_oListItem.Index + 1

                    If ListViewHelper.GetListViewSubItem(r_oListItem, kRecDetSubItemsRecoveryTypeDesc).Text = "Total" Then
                        cmdEdit.Enabled = False
                        cmdDelete.Enabled = False
                    Else
                        cmdEdit.Enabled = txtParty.Text.Trim() <> ""

                        cmdDelete.Enabled = ListViewHelper.GetListViewSubItem(r_oListItem, kRecDetSubItemsThisReceipt).Text <> "0.00"
                    End If
                Else
                    cmdEdit.Enabled = False
                    cmdDelete.Enabled = False
                End If
            Else
                EnableControls()
                sPartyType = CStr(m_vClaimReceiptItemDetail(kClaimRecItemDetRecoveryPartyTypeId, 0))
                sShortName = CStr(m_vClaimReceiptItemDetail(kClaimRecItemDetShortName, 0))

                txtParty.Text = sShortName
                If sPartyType = "" Then
                    OptClaimReceivable.Checked = True
                    txtParty.Text = kAccountCLMRECEIVABLE
                ElseIf StringsHelper.ToDoubleSafe(sPartyType) = kPartyTypeAgent Then
                    OptAgent.Checked = True
                ElseIf StringsHelper.ToDoubleSafe(sPartyType) = kPartyTypeClient Then
                    OptClient.Checked = True
                ElseIf StringsHelper.ToDoubleSafe(sPartyType) = kPartyTypeParty Then
                    OptParty.Checked = True
                ElseIf StringsHelper.ToDoubleSafe(sPartyType) = kPartyTypeInsurer Then
                    OptInsurer.Checked = True

                End If

                DisableControls()
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return nResult
    End Function

    'Start - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)
    Private Sub DisableControls()
        OptClaimReceivable.Enabled = False
        OptAgent.Enabled = False
        OptClient.Enabled = False
        OptInsurer.Enabled = False
        OptParty.Enabled = False
        cmdParty.Enabled = False
    End Sub
    'End - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)

    'Start - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)
    Private Sub EnableControls()
        OptClaimReceivable.Enabled = True
        OptAgent.Enabled = True
        OptClient.Enabled = True
        OptInsurer.Enabled = True
        OptParty.Enabled = True
    End Sub
    'End - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)

    'Start - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)
    Private Function SetNewlyAttachedPartyValues(ByRef bPartyNewlyAdded As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetNewlyAttachedPartyValues"

        Dim iPos As Integer
        Dim vAccountDetails As Object
        Dim lReturn As gPMConstants.PMEReturnCode

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            Dim counter_2 As Integer
            If Information.IsArray(m_vAttachPartyToRecovery) Then
                iPos = m_vAttachPartyToRecovery.GetUpperBound(1)
                'Loop thru the array and find the position in which the selected recovery id is stored
                counter_2 = iPos
                For iCounter As Integer = 0 To counter_2
                    If m_vAttachPartyToRecovery(kNewPartyRecoveryId, iCounter).Equals(m_vRecoveryDetails(kRecoveryDetailsRecoveryId, lvwRecovery.FocusedItem.Index + 1 - 2)) Then
                        iPos = iCounter
                        bPartyNewlyAdded = True
                        Exit For
                    End If
                Next

                If bPartyNewlyAdded Then


                    lReturn = m_oBusiness.GetAccountDetailsByShortCode(v_sShortCode:=m_vAttachPartyToRecovery(kNewPartyShortName, iPos), r_vResults:=vAccountDetails)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "GetAccountDetailsByShortCode", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    If Information.IsArray(vAccountDetails) Then

                        m_lClaimReceivableCurrencyId = CInt(vAccountDetails(0, 0))

                        m_lClaimReceivableAccountId = CInt(vAccountDetails(1, 0))
                    End If

                    If CDbl(m_vAttachPartyToRecovery(kNewPartyPartyTypeId, iPos)) = kPartyTypeInsurer Then
                        m_sSelectedPayee = kAccountCLMInsurer
                        OptInsurer.Checked = True
                    ElseIf CDbl(m_vAttachPartyToRecovery(kNewPartyPartyTypeId, iPos)) = kPartyTypeParty Then
                        m_sSelectedPayee = kAccountCLMParty
                        OptParty.Checked = True
                    ElseIf CDbl(m_vAttachPartyToRecovery(kNewPartyPartyTypeId, iPos)) = kPartyTypeClient Then
                        m_sSelectedPayee = kAccountCLMClient
                        OptClient.Checked = True
                    ElseIf CDbl(m_vAttachPartyToRecovery(kNewPartyPartyTypeId, iPos)) = kPartyTypeAgent Then
                        m_sSelectedPayee = kAccountCLMAgent
                        OptAgent.Checked = True
                    End If
                    txtParty.Text = CStr(m_vAttachPartyToRecovery(kNewPartyShortName, iPos))
                End If
            End If
            Return result

        Catch ex As Exception


            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here



            Return result
        End Try
    End Function
    'End - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)

    'Start - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)
    Public Function CheckChangeOfParty(ByVal lPartyTypeId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CheckChangeOfParty"
        Dim lReturn As gPMConstants.PMEReturnCode

        Try


            result = gPMConstants.PMEReturnCode.PMTrue
            If Information.IsArray(m_vAttachPartyToRecovery) Then
                For iCounter As Integer = 0 To m_vAttachPartyToRecovery.GetUpperBound(1)
                    If m_vAttachPartyToRecovery(kNewPartyRecoveryId, iCounter).Equals(m_vRecoveryDetails(kRecoveryDetailsRecoveryId, lvwRecovery.FocusedItem.Index + 1 - 2)) Then
                        If CDbl(m_vAttachPartyToRecovery(kNewPartyPartyTypeId, iCounter)) <> lPartyTypeId Then

                            'The user have changed the party so delete that entry from the array
                            lReturn = CType(RemoveAttachedParty(CInt(m_vRecoveryDetails(kRecoveryDetailsRecoveryId, lvwRecovery.FocusedItem.Index + 1 - 2))), gPMConstants.PMEReturnCode)
                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError(kMethodName, "RemoveAttachedParty Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If
                            Exit For
                        End If
                    End If
                Next
            End If

            Return result

        Catch ex As Exception


            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here



            Return result
        End Try
    End Function
    'End - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)

    'Start - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)
    Public Function RemoveAttachedParty(ByVal lRecoveryId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "RemoveAttachedParty"

        Dim vTempPartyDetails As Object
        Dim iCount As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue
            If Information.IsArray(m_vAttachPartyToRecovery) Then
                iCount = 0
                'Loop thru the array and store the values except for the recovery id which is to be removed
                For iCounter As Integer = 0 To m_vAttachPartyToRecovery.GetUpperBound(1)
                    If CDbl(m_vAttachPartyToRecovery(kNewPartyRecoveryId, iCounter)) <> lRecoveryId Then
                        If Not Information.IsArray(vTempPartyDetails) Then
                            ReDim vTempPartyDetails(kNewPartyPartyTypeId, 0)
                        Else
                            ReDim Preserve vTempPartyDetails(kNewPartyPartyTypeId, iCount)
                        End If

                        vTempPartyDetails(kNewPartyPartyID, iCount) = m_vAttachPartyToRecovery(kNewPartyPartyID, iCounter)

                        vTempPartyDetails(kNewPartyShortName, iCount) = m_vAttachPartyToRecovery(kNewPartyShortName, iCounter)

                        vTempPartyDetails(kNewPartyLongName, iCount) = m_vAttachPartyToRecovery(kNewPartyLongName, iCounter)

                        vTempPartyDetails(kNewPartyRecoveryId, iCount) = m_vAttachPartyToRecovery(kNewPartyRecoveryId, iCounter)

                        vTempPartyDetails(kNewPartyPartyTypeId, iCount) = m_vAttachPartyToRecovery(kNewPartyPartyTypeId, iCounter)
                        iCount += 1
                    ElseIf CDbl(m_vAttachPartyToRecovery(kNewPartyRecoveryId, iCounter)) = lRecoveryId Then
                        lvwRecovery.FocusedItem.SubItems.Item(kRecDetSubItemsRecoveryPartyName - 1).Text = ""
                    End If
                Next

                m_vAttachPartyToRecovery = vTempPartyDetails
            End If
            Return result

        Catch ex As Exception



            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here



            Return result
        End Try
    End Function
    'End - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)

    'Start - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)
    Public Function SetNewlyAttachedParty(ByVal lPartyId As Integer, ByVal sShortName As String, ByVal sLongName As String, ByVal lRecoveryId As String, ByVal iPartyTypeId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetNewlyAttachedParty"

        Dim iCount As Integer

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            Dim bFlag As Boolean
            Dim counter_3 As Integer
            If Not Information.IsArray(m_vAttachPartyToRecovery) Then
                ReDim m_vAttachPartyToRecovery(4, 0)
                iCount = 0
            Else
                iCount = m_vAttachPartyToRecovery.GetUpperBound(1)
                counter_3 = iCount
                For iCounter As Integer = 0 To counter_3
                    If m_vAttachPartyToRecovery(kNewPartyRecoveryId, iCounter).Equals(m_vRecoveryDetails(kRecoveryDetailsRecoveryId, lvwRecovery.FocusedItem.Index + 1 - 2)) Then
                        iCount = iCounter
                        bFlag = True
                        Exit For
                    End If
                Next
                If Not bFlag Then
                    iCount += 1
                    ReDim Preserve m_vAttachPartyToRecovery(kNewPartyPartyTypeId, iCount)
                End If
            End If

            m_vAttachPartyToRecovery(kNewPartyPartyID, iCount) = lPartyId
            m_vAttachPartyToRecovery(kNewPartyShortName, iCount) = sShortName
            m_vAttachPartyToRecovery(kNewPartyLongName, iCount) = sLongName
            m_vAttachPartyToRecovery(kNewPartyRecoveryId, iCount) = lRecoveryId
            m_vAttachPartyToRecovery(kNewPartyPartyTypeId, iCount) = iPartyTypeId
            txtParty.Text = sShortName.Trim()
            lvwRecovery.FocusedItem.SubItems.Item(kRecDetSubItemsRecoveryPartyName - 1).Text = sLongName

            Return result

        Catch ex As Exception


            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here



            Return result
        End Try
    End Function
    'End - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)

    ' ***************************************************************** '
    ' Name: SetupInsuredTaxAdjustments
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 10-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function SetupInsuredTaxAdjustments() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupInsuredTaxAdjustments"

        Dim lReturn As Integer
        Dim bEnabled As Boolean

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            bEnabled = chkITDomiciled.CheckState = CheckState.Checked
            'ATS
            If m_bAdvancedTaxScriptingOption Then

                lblITPercentage.Enabled = bEnabled
                lblITPercentage.Font = VB6.FontChangeBold(lblITPercentage.Font, bEnabled)

                If gPMFunctions.ToSafeCurrency(txtITPercentage.Text) > 0 Then
                    lblITTaxNo.Enabled = bEnabled
                    txtITTaxNo.Enabled = bEnabled
                    'Start Arul PN56725
                    If chkITDomiciled.CheckState = CheckState.Unchecked Then
                        txtITPercentage.Enabled = False
                    End If
                    'End Arul PN56725
                Else
                    lblITTaxNo.Enabled = False
                    txtITTaxNo.Enabled = False
                    'Start Arul PN56725
                    If chkITDomiciled.CheckState = CheckState.Checked Then
                        txtITPercentage.Text = StringsHelper.Format(m_crClientTaxPercentage, "0.00")
                        txtITTaxNo.Text = m_sClientTaxNumber
                        lblITPercentage.Enabled = True
                        lblITPercentage.Font = VB6.FontChangeBold(lblITPercentage.Font, True)
                        lblITTaxNo.Enabled = True
                        txtITTaxNo.Enabled = True
                        txtITPercentage.Enabled = True
                    Else
                        txtITPercentage.Enabled = False
                        txtITTaxNo.Enabled = False
                        lblITTaxNo.Enabled = False
                        lblITPercentage.Enabled = False
                    End If
                    'End Arul PN56725
                End If
            Else
                txtITPercentage.Enabled = bEnabled
                txtITTaxNo.Enabled = bEnabled
            End If

            If Not bEnabled Then
                txtITPercentage.Text = ""
                txtITTaxNo.Text = ""
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ActionFindParty
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 10-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function ActionFindParty() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ActionFindParty"

        Dim lReturn As Integer
        'developer guide no.88
        Dim oFindParty As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' if we havent got an instance of find party
            If oFindParty Is Nothing Then

                ' get a new instance of find party
                Dim temp_oFindParty As Object
                lReturn = m_oObjectManager.GetInstance(temp_oFindParty, sClassName:="iPMBFindParty.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                oFindParty = temp_oFindParty

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed to GetInstnace of iPMBFindParty.Interface", gPMConstants.PMELogLevel.PMLogError)
                End If

            End If

            ' Set component properties and start interface

            oFindParty.CallingAppName = ACApp

            oFindParty.IgnoreDriversAndWitnesses = True

            oFindParty.NotEditable = gPMConstants.PMEReturnCode.PMTrue
            ' Start - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)

            oFindParty.EnableNewParty = True
            Dim iPartyTypeId As Integer
            If OptParty.Checked Then

                oFindParty.SpecialParty = "OT"
                iPartyTypeId = kPartyTypeParty
            ElseIf (OptInsurer.Checked) Then

                oFindParty.SpecialParty = "IN"
                iPartyTypeId = kPartyTypeInsurer
            End If
            ' End - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)

            ' start the find part component

            lReturn = oFindParty.Start()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "iPMBFindParty.Interface.Start Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get the returned partycnt

            If oFindParty.PartyCnt > 0 Then
                ' Start - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)
                '        m_lOtherPartyCnt = oFindParty.PartyCnt
                '        m_lSelectedPayeeId = m_lOtherPartyCnt

                txtParty.Text = oFindParty.ShortName
                m_sSelectedPayee = txtParty.Text




                lReturn = SetNewlyAttachedParty(lPartyId:=oFindParty.PartyCnt, sShortName:=oFindParty.ShortName, sLongName:=oFindParty.LongName, lRecoveryId:=CStr(m_vRecoveryDetails(kRecoveryDetailsRecoveryId, lvwRecovery.FocusedItem.Index + 1 - 2)), iPartyTypeId:=iPartyTypeId)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "SetNewlyAttachedParty Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
                ' End - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)

                '        lReturn = GetOtherPartyCurrencyId
                '        If lReturn <> PMTrue Then
                '            RaiseError kMethodName, "GetOtherPartyCurrencyId Failed", PMLogError
                '        End If
                lReturn = GetOtherPartyDetails()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "", gPMConstants.PMELogLevel.PMLogError)
                End If

            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            oFindParty = Nothing




        End Try
        Return result
    End Function


    Private isInitializingComponent As Boolean
    Private Sub OptAgent_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles OptAgent.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If

            Const kMethodName As String = "OptAgent_Click"


            Dim oBusiness As bCLMRecovery.Business
            Dim sAgentCode, sAgentName As String
            Dim lClientCnt As Integer
            Dim sClientCode, sClientName As String
            Dim lReturn As gPMConstants.PMEReturnCode
            Dim lAgentCnt As Integer

            'Call ActionPayeeOption(kPayeeOptAgent)
            'm_lPrevClaimPayeeOption = kPayeeOptAgent

            cmdParty.Enabled = False
            If Not m_bPartyAlreadyAttached Then
                '---------------------------------------------------------------
                ' This function is used to check whether the user have changed the
                'party which is attached to the recovery
                '---------------------------------------------------------------
                lReturn = CType(CheckChangeOfParty(kPartyTypeAgent), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "CheckChangeOfParty Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                Dim temp_oBusiness As Object
                lReturn = m_oObjectManager.GetInstance(temp_oBusiness, "bCLMRecovery.Business", vInstanceManager:="ClientManager")
                oBusiness = temp_oBusiness
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetInstance Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                'Get the details of the party which is newly attached to the recovery

                lReturn = oBusiness.GetAttachedParties(r_lAgentCnt:=lAgentCnt, r_sAgentCode:=sAgentCode, r_sAgentName:=sAgentName, r_lClientCnt:=lClientCnt, r_sClientCode:=sClientCode, r_sClientName:=sClientName, v_lClaim_Id:=m_lClaimId)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetAttachedParties Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
                If lAgentCnt <= 0 Then
                    'Start Renuka PN 61649
                    MessageBox.Show("No Agent is attached with this Claim Recovery.", "Agent Validation", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    'End Renuka PN 61649
                    OptClaimReceivable.Checked = True
                Else
                    txtParty.Text = sAgentCode

                    'Store the newly selected party details in the array
                    lReturn = CType(SetNewlyAttachedParty(lPartyId:=lAgentCnt, sShortName:=sAgentCode, sLongName:=sAgentName, lRecoveryId:=CStr(m_vRecoveryDetails(kRecoveryDetailsRecoveryId, lvwRecovery.FocusedItem.Index + 1 - 2)), iPartyTypeId:=kPartyTypeAgent), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "SetNewlyAttachedParty Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If
            End If
        End If
    End Sub
    'End - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)


    'Private Sub OptClaimPayable_Click()
    '    Call ActionPayeeOption(kPayeeOptClaimReceivable)
    '    m_lPrevClaimPayeeOption = kPayeeOptClaimReceivable
    'End Sub

    'Start - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)
    Private Sub OptClaimReceivable_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles OptClaimReceivable.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If

            Const kMethodName As String = "OptClaimReceivable_Click"

            '---------------------------------------------------------------
            ' This function is used to check whether the user have changed the
            'party which is attached to the recovery
            '---------------------------------------------------------------
            Dim lReturn As gPMConstants.PMEReturnCode = CType(CheckChangeOfParty(kPartyTypeClaimReceivable), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "CheckChangeOfParty Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            cmdParty.Enabled = False
            m_sSelectedPayee = kAccountCLMRECEIVABLE
            txtParty.Text = kAccountCLMRECEIVABLE

            lReturn = CType(GetAccountDetailsByShortCode(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetAccountDetailsByShortCode Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            '    Call ActionPayeeOption(kPayeeOptClaimReceivable)
            '    m_lPrevClaimPayeeOption = kPayeeOptClaimReceivable
        End If
    End Sub
    'End - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)

    'Start - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)
    Private Sub OptClient_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles OptClient.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If

            Const kMethodName As String = "OptClient_Click"

            Dim sShortName As String = ""
            Dim lReturn As gPMConstants.PMEReturnCode
            If Not m_bViewReceiptMode Then
                If Not m_bPartyAlreadyAttached Then
                    '---------------------------------------------------------------
                    ' This function is used to check whether the user have changed the
                    ' party which is attached to the recovery
                    '---------------------------------------------------------------
                    lReturn = CType(CheckChangeOfParty(kPartyTypeClient), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "CheckChangeOfParty Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If


                    lReturn = m_oBusiness.GetPartyName(v_lPartyCnt:=m_lClientId, v_sFieldName:="shortname", r_sResult:=sShortName)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "GetPartyName Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    'Store the newly selected party details in the array
                    lReturn = CType(SetNewlyAttachedParty(m_lClientId, sShortName.Trim(), m_sClientName, CStr(m_vRecoveryDetails(kRecoveryDetailsRecoveryId, lvwRecovery.FocusedItem.Index + 1 - 2)), kPartyTypeClient), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "SetNewlyAttachedParty Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                    cmdParty.Enabled = False
                End If
            End If

            '    Call ActionPayeeOption(kPayeeOptClient)
            '    m_lPrevClaimPayeeOption = kPayeeOptClient
        End If
    End Sub
    'End - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)

    'Start - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)
    Private Sub OptInsurer_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles OptInsurer.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If

            Const kMethodName As String = "OptInsurer_Click"

            Dim lReturn As gPMConstants.PMEReturnCode

            If Not m_bPartyAlreadyAttached Then
                txtParty.Text = ""
                cmdParty.Enabled = True

                '---------------------------------------------------------------
                ' This function is used to check whether the user have changed the
                ' party which is attached to the recovery
                '---------------------------------------------------------------
                lReturn = CType(CheckChangeOfParty(kPartyTypeInsurer), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "CheckChangeOfParty Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If
        End If
    End Sub
    'End - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)

    'Start - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)
    Private Sub OptParty_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles OptParty.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If

            Const kMethodName As String = "OptParty_Click"

            Dim lReturn As gPMConstants.PMEReturnCode

            If Not m_bPartyAlreadyAttached Then
                txtParty.Text = ""
                cmdParty.Enabled = True

                '---------------------------------------------------------------
                ' This function is used to check whether the user have changed the
                ' party which is attached to the recovery
                '---------------------------------------------------------------
                lReturn = CType(CheckChangeOfParty(kPartyTypeParty), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "CheckChangeOfParty Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If
            '    Call ActionPayeeOption(kPayeeOptParty)
            '    m_lPrevClaimPayeeOption = kPayeeOptParty
        End If
    End Sub
    'End - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)



    ' ***************************************************************** '
    ' Name: GetOtherPartyDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 11-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function GetOtherPartyDetails() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetOtherPartyDetails"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vAccountDetails As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get the specified other party account details

            lReturn = m_oBusiness.GetOtherPartyDetails(v_lPartyCnt:=m_lOtherPartyCnt, r_vResults:=vAccountDetails)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetOtherPartyAccountDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Information.IsArray(vAccountDetails) Then
                ' get the other parties account currency id

                m_lOtherPartyCurrencyId = CInt(vAccountDetails(0, 0))
                m_bOtherPartyDomiciledForTax = gPMFunctions.ToSafeBoolean(vAccountDetails(1, 0), False)

                m_bSelectedPayeeDomiciledForTax = m_bOtherPartyDomiciledForTax
                m_lSelectedPayeeAccountCurrencyId = m_lOtherPartyCurrencyId


            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function



    ' ***************************************************************** '
    ' Name: GetAccountDetailsByShortCode
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 11-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function GetAccountDetailsByShortCode() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetAccountDetailsByShortCode"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vAccountDetails As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            lReturn = m_oBusiness.GetAccountDetailsByShortCode(v_sShortCode:=kAccountCLMRECEIVABLE, r_vResults:=vAccountDetails)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetAccountDetailsByShortCode", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Information.IsArray(vAccountDetails) Then

                m_lClaimReceivableCurrencyId = CInt(vAccountDetails(0, 0))

                m_lClaimReceivableAccountId = CInt(vAccountDetails(1, 0))
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetReceiptItem
    '
    ' Parametseters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 12-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function GetReceiptItem(ByVal v_sRecoveryId As String, ByRef r_oReceiptItem As cReceiptItem) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetReceiptItem"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_colReceiptItems.Count = 0 Then

                ' add the payment item
                lReturn = CType(AddReceiptItem(v_sRecoveryId), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "AddReceiptItem Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            Else

                ' if the payment item doesnt exist yet
                r_oReceiptItem = m_colReceiptItems(v_sRecoveryId)

                If r_oReceiptItem Is Nothing Then

                    ' add the payment item
                    lReturn = CType(AddReceiptItem(v_sRecoveryId), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "AddReceiptItem Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If
            End If

            ' get the payment item - it should now exist so just return it
            r_oReceiptItem = m_colReceiptItems.Item(v_sRecoveryId)


        Catch ex As Exception
            result = PMEReturnCode.PMFalse
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        End Try

        Return result
    End Function

    ' ***************************************************************** '
    ' Name: AddReceiptItem
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 12-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function AddReceiptItem(ByVal v_sRecoveryId As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AddReceiptItem"

        Dim lReturn As Integer
        Dim oReceiptItem As cReceiptItem

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' create new version of payment item
            oReceiptItem = New cReceiptItem()

            ' add new payment item to the collection
            m_colReceiptItems.Add(oReceiptItem, v_sRecoveryId)

            ' set default parameters
            If m_colReceiptItems.Count > 1 Then
                ' default the currency exchange rates and dates from any existing item...

                oReceiptItem.AccountToBaseDate = m_colReceiptItems.Item(1).AccountToBaseDate

                oReceiptItem.AccountToBaseXRate = m_colReceiptItems.Item(1).AccountToBaseXRate

                oReceiptItem.CurrencyToBaseDate = m_colReceiptItems.Item(1).CurrencyToBaseDate

                oReceiptItem.CurrencyToBaseXRate = m_colReceiptItems.Item(1).CurrencyToBaseXRate

                oReceiptItem.ExchangeRateOverrideReasonId = m_colReceiptItems.Item(1).ExchangeRateOverrideReasonId

                oReceiptItem.SystemToBaseDate = m_colReceiptItems.Item(1).SystemToBaseDate

                oReceiptItem.SystemToBaseXRate = m_colReceiptItems.Item(1).SystemToBaseXRate

                oReceiptItem.ReceiptToLossXRate = m_colReceiptItems.Item(1).ReceiptToLossXRate
            Else
                ' default the currency xchange rates and dates from the original item...
                oReceiptItem.AccountToBaseDate = DateTime.Today
                oReceiptItem.AccountToBaseXRate = 0
                oReceiptItem.CurrencyToBaseDate = DateTime.Today
                oReceiptItem.CurrencyToBaseXRate = 0
                oReceiptItem.ExchangeRateOverrideReasonId = 0
                oReceiptItem.SystemToBaseDate = DateTime.Today
                oReceiptItem.SystemToBaseXRate = 0
                oReceiptItem.ReceiptToLossXRate = 0
            End If

            oReceiptItem.CurrencyId = 0
            oReceiptItem.RecoveryId = 0
            oReceiptItem.TaxAmount = 0
            oReceiptItem.TaxGroupId = 0
            oReceiptItem.ThisReceipt = 0



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: Save
    '
    ' Parameters: n/a
    '
    ' Description: saves claim payment / claim payment items / tax calculations
    '
    ' History:
    '           Created : MEvans : 12-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Public Function Save() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "Save"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim bTransStarted As Boolean

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' if an unrecoverable error has occurred then
            If m_bUnrecoverableError Then
                gPMFunctions.RaiseError(kMethodName, "Save Failed - Unrecoverable Error occurred.", gPMConstants.PMELogLevel.PMLogError)
            Else

                If m_crTotalThisReceiptNet <> 0 Then

                    ' start transaction

                    lReturn = m_oBusiness.BeginTrans
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "Begin Transaction Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    bTransStarted = True

                    ' save payment / payment items / tax calculations
                    lReturn = SaveClaimReceipt()
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "SaveClaimReceipt Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    ' commit transaction

                    lReturn = m_oBusiness.CommitTrans
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "Commit Transaction Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    'S4B Claim Enhancements R&D 2005
                    m_bReceiptMade = True

                End If

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' if a transaction was started and an error occurred then
            ' rollback all updates...
            If bTransStarted Then

                m_oBusiness.RollbackTrans()
            End If

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SetupAdvancedTaxFrames
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function SetupAdvancedTaxFrames() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupAdvancedTaxFrames"
        Dim lLayoutID As Integer

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' if the advanced tax scripting option is on
            ' then display the advanced tax frames
            If m_bAdvancedTaxScriptingOption And g_sUnderwritingOrAgency <> "A" Then
                lLayoutID = 1
            Else
                lLayoutID = 2
            End If


            Select Case lLayoutID
                Case 1

                    ' show the tax specific frames
                    fraReceivableTaxStatus.Visible = True
                    fraInsuredTaxAdjustment.Visible = True
                    fraSettlement.Visible = Not (m_bAdvancedTaxScriptingOption And Not m_bATSSattlement)

                    fraRecovery.Top = VB6.TwipsToPixelsY(1530)
                    fraRecovery.Height = VB6.TwipsToPixelsY(4095)
                    lvwRecovery.Height = VB6.TwipsToPixelsY(3195)

                    cmdEdit.Top = VB6.TwipsToPixelsY(3555)
                    cmdDelete.Top = VB6.TwipsToPixelsY(3555)

                    cmdEditPayee.Top = VB6.TwipsToPixelsY(3555)
                    cmdEditPayee.Visible = True

                Case 2

                    ' hide the tax specific frames
                    fraReceivableTaxStatus.Visible = False
                    fraInsuredTaxAdjustment.Visible = False
                    fraSettlement.Visible = False

                    fraRecovery.Top = fraReceivableTaxStatus.Top

                    fraRecovery.Height = VB6.TwipsToPixelsY(4935)
                    lvwRecovery.Height = VB6.TwipsToPixelsY(4155)

                    cmdEdit.Top = VB6.TwipsToPixelsY(4515)
                    cmdDelete.Top = VB6.TwipsToPixelsY(4515)

                    cmdEditPayee.Visible = False
                    cmdEditPayee.Top = VB6.TwipsToPixelsY(4515)

            End Select

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: PopulateTotals
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 15-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function PopulateTotals() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulateTotals"

        Dim lReturn As Integer
        Dim oListItem As ListViewItem
        Dim crTotalReserve, crTotalReceived, crTotalThisReciept, crTotalTaxAmount, crTotalThisNet, crTotalBalance As Decimal
        Dim lTotalItems As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            lTotalItems = lvwRecovery.Items.Count

            ' if there are list items
            If lTotalItems > 0 Then

                m_crTotalThisReceiptGross = 0

                ' then get totals for all reserve lines apart from total line
                ' so start from the second line i.e the first reserve line
                For lItem As Integer = kReceiptDetailsFirstReserveItemRow To lTotalItems

                    ' get totals from payment detail grid
                    crTotalReserve += gPMFunctions.ToSafeCurrency(ListViewHelper.GetListViewSubItem(lvwRecovery.Items.Item(lItem - 1), kRecDetSubItemsTotalReserve).Text, 0)
                    crTotalReceived += gPMFunctions.ToSafeCurrency(ListViewHelper.GetListViewSubItem(lvwRecovery.Items.Item(lItem - 1), kRecDetSubItemsRecoveredTotal).Text, 0)
                    crTotalThisReciept += gPMFunctions.ToSafeCurrency(ListViewHelper.GetListViewSubItem(lvwRecovery.Items.Item(lItem - 1), kRecDetSubItemsThisReceipt).Text, 0)
                    crTotalTaxAmount += gPMFunctions.ToSafeCurrency(ListViewHelper.GetListViewSubItem(lvwRecovery.Items.Item(lItem - 1), kRecDetSubItemsTaxAmount).Text, 0)
                    crTotalThisNet += gPMFunctions.ToSafeCurrency(ListViewHelper.GetListViewSubItem(lvwRecovery.Items.Item(lItem - 1), kRecDetSubItemsThisNet).Text, 0)
                    crTotalBalance += gPMFunctions.ToSafeCurrency(ListViewHelper.GetListViewSubItem(lvwRecovery.Items.Item(lItem - 1), kRecDetSubItemsBalance).Text, 0)

                Next

            End If

            ' store the total amount of this payment including tax
            m_crTotalThisReceiptGross = crTotalThisReciept

            ' store the total amount of this receipt
            m_crTotalThisReceiptNet = crTotalThisNet

            ' store the total tax amount
            m_crTotalTaxAmount = crTotalTaxAmount

            ' get the total row
            oListItem = lvwRecovery.Items.Item(0)

            ' save totals to total row
            ListViewHelper.GetListViewSubItem(oListItem, kRecDetSubItemsTotalReserve).Text = StringsHelper.Format(crTotalReserve, "0.00")
            ListViewHelper.GetListViewSubItem(oListItem, kRecDetSubItemsRecoveredTotal).Text = StringsHelper.Format(crTotalReceived, "0.00")
            ListViewHelper.GetListViewSubItem(oListItem, kRecDetSubItemsThisReceipt).Text = StringsHelper.Format(crTotalThisReciept, "0.00")
            ListViewHelper.GetListViewSubItem(oListItem, kRecDetSubItemsTaxAmount).Text = StringsHelper.Format(crTotalTaxAmount, "0.00")
            ListViewHelper.GetListViewSubItem(oListItem, kRecDetSubItemsThisNet).Text = StringsHelper.Format(crTotalThisNet, "0.00")
            ListViewHelper.GetListViewSubItem(oListItem, kRecDetSubItemsBalance).Text = StringsHelper.Format(crTotalBalance, "0.00")

            ' bold the total line
            For Each oListSubItem As ListViewItem.ListViewSubItem In oListItem.SubItems
                oListSubItem.Font = VB6.FontChangeBold(oListSubItem.Font, True)
            Next oListSubItem



        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetTaxGroupDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 26-07-2005 : 360 - Taxes On Claims
    ' ***************************************************************** '
    Private Function GetTaxGroupDetails(ByVal v_vIsWithHoldingTax As Object, ByRef r_vArray(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetTaxGroupDetails"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            lReturn = m_oBusiness.GetTaxGroupDetails(v_vIsWithHoldingTax:=v_vIsWithHoldingTax, r_vResults:=r_vArray)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetTaxGroupDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: SetupThisReceiptInterface
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function SetupThisReceiptInterface() As Integer

        Dim nResult As Integer = 0
        Const kMethodName As String = "SetupThisReceiptInterface"

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' if no payment has been made in this session then
            ' dont show the this payment tab.
            If m_crTotalThisReceiptGross > 0 Then
                SSTabHelper.SetTabVisible(SSTab1, kTabThisReceipt, True)
            ElseIf m_crTotalThisReceiptGross < 0 Then
                SSTabHelper.SetTabVisible(SSTab1, kTabThisReceipt, True)
            Else
                SSTabHelper.SetTabVisible(SSTab1, kTabThisReceipt, False)
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return nResult
    End Function
    ' ***************************************************************** '
    ' Name: PopulateThisReceiptDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 24-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function PopulateThisReceiptDetails() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulateThisReceiptDetails"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim crTotalThisReceipt, crTotalTaxAmount, crTotalThisNet As Decimal

        Dim oReceiptItem As cReceiptItem
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If m_colReceiptItems.Count <> 0 Then

                For lReceiptItem As Integer = 1 To m_colReceiptItems.Count

                    oReceiptItem = m_colReceiptItems.Item(lReceiptItem)

                    crTotalThisReceipt += (oReceiptItem.ThisReceipt * oReceiptItem.ReceiptToLossXRate)
                    crTotalTaxAmount += (oReceiptItem.TaxAmount * oReceiptItem.ReceiptToLossXRate)


                Next

                ' populate the "this payment summary"
                

                'PN-61621(Sushil Kumar)
                If m_bReceiptExcludeTax Then
                    crTotalThisNet += crTotalThisReceipt 
                Else
                    crTotalThisNet += crTotalThisReceipt - crTotalTaxAmount
                End If

               If m_bReceiptExcludeTax Then
                    txtGrossReceipt.Text = StringsHelper.Format(crTotalThisReceipt + crTotalTaxAmount, "0.00")
                    txtTotalTax.Text = StringsHelper.Format(crTotalTaxAmount, "0.00")
                    txtNetReceipt.Text = StringsHelper.Format(crTotalThisNet, "0.00")

                Else
                    txtGrossReceipt.Text = StringsHelper.Format(crTotalThisReceipt, "0.00")
                    txtTotalTax.Text = StringsHelper.Format(crTotalTaxAmount, "0.00")
                    txtNetReceipt.Text = StringsHelper.Format(crTotalThisNet, "0.00")

                End If

            End If

            ' populate the tax band rate breakdown list view
            lReturn = PopulateTaxesListView()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "PopulateTaxesListView Failed", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: ResetThisReceipt
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function ResetThisReceipt() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ResetThisReceipt"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If m_colReceiptItems.Count > 0 Then

                ' reset the payment item collection
                m_colReceiptItems = New Collection()

                ' clear down all this payment entries in the listview
                For lItem As Integer = 1 To lvwRecovery.Items.Count

                    ListViewHelper.GetListViewSubItem(lvwRecovery.Items.Item(lItem - 1), kRecDetSubItemsThisReceipt).Text = StringsHelper.Format(0, "0.00")
                    ListViewHelper.GetListViewSubItem(lvwRecovery.Items.Item(lItem - 1), kRecDetSubItemsTaxAmount).Text = StringsHelper.Format(0, "0.00")
                    ListViewHelper.GetListViewSubItem(lvwRecovery.Items.Item(lItem - 1), kRecDetSubItemsThisNet).Text = StringsHelper.Format(0, "0.00")

                Next

            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    Private Sub txtITPercentage_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtITPercentage.Enter
        ValidatePercentage(txtITPercentage)
    End Sub

    Private Sub txtITPercentage_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtITPercentage.Leave
        ValidatePercentage(txtITPercentage)
    End Sub

    ''' <summary>
    ''' ValidatePercentage
    ''' </summary>
    ''' <param name="oTxtBox"></param>
    ''' <returns></returns>
    ''' <remarks>Created : MEvans : Date : Process ID</remarks>
    Private Function ValidatePercentage(ByRef oTxtBox As TextBox) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ValidatePercentage"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If oTxtBox.Text <> "" Then
                oTxtBox.Text = CStr(gPMFunctions.ToSafeCurrency(oTxtBox.Text, 0))
                If CDbl(oTxtBox.Text) < 0 Or CDbl(oTxtBox.Text) > 100 Then
                    MessageBox.Show("Percentage must be between 0 - 100%", "Percentage Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    oTxtBox.Text = "0.00"
                    oTxtBox.Focus()
                Else
                    oTxtBox.Text = StringsHelper.Format(oTxtBox.Text, "0.00")
                End If
            End If

            If m_bAdvancedTaxScriptingOption And txtITPercentage.Text <> "" And gPMFunctions.ToSafeCurrency(txtITPercentage.Text) > 0 And chkITDomiciled.CheckState = CheckState.Checked Then
                lblITTaxNo.Enabled = True
                txtITTaxNo.Enabled = True
            Else
                lblITTaxNo.Enabled = False
                txtITTaxNo.Enabled = False
                txtITTaxNo.Text = ""
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetTaxGroupTaxBandDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 30-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function GetTaxGroupTaxBandDetails() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetTaxGroupTaxBandDetails"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get the tax group tax band details

            lReturn = m_oBusiness.GetTaxGroupTaxBandDetails(r_vResults:=m_vTaxGroupTaxBandDetails)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetTaxGroupTaxBandDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            '    If Not IsArray(m_vTaxGroupTaxBandDetails) Then
            '        RaiseError kMethodName, "GetTaxGroupTaxBandDetails Failed to return any data", PMLogError
            '    End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: DisableInterface
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 30-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function DisableInterface() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DisableInterface"

        Dim lReturn As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            cmdEdit.Enabled = False
            fraClaimInformation.Enabled = False
            fraReceivableTaxStatus.Enabled = False
            fraInsuredTaxAdjustment.Enabled = False
            fraPayee.Enabled = False
            fraRecovery.Enabled = False
            fraSettlement.Enabled = False
            fraTaxesOnReceipt.Enabled = False
            fraReceiptComments.Enabled = False
            fraReceiptDetails.Enabled = False
            fraReceiptSummary.Enabled = False

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupItemFromArray
    '
    ' Parameters: n/a
    '
    ' Description: Returns the code for a specifed item description
    '                  in a specified lookup table..
    '
    ' History:
    '           Created : MEvans : 06-06-2003 : 223
    ' ***************************************************************** '
    Private Function GetLookupItemFromArray(ByVal v_vArray(,) As Object, ByRef r_sItemDesc As String, ByRef r_sItemCode As String, ByRef r_lItemId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetLookupItemFromArray"

        Dim lRow As Integer
        Dim bFoundMatch As Boolean
        Dim sCode As String = ""
        Dim llBound, lUBound As Integer
        Dim v_vLookupItem As String = ""
        Dim lLookupItem As Integer

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Lookup value contants.
            Const ACValueTableName As Integer = 0
            Const ACValueID As Integer = 1
            Const ACValueStartPos As Integer = 2
            Const ACValueNumber As Integer = 3

            Const ACDetailKey As Integer = 0
            Const ACDetailDesc As Integer = 1
            Const ACDetailCode As Integer = 2

            If Information.IsArray(v_vArray) Then

                llBound = v_vArray.GetLowerBound(1)
                lUBound = v_vArray.GetUpperBound(1)

                ' set lookup properties
                If r_lItemId <> 0 Then
                    v_vLookupItem = CStr(r_lItemId)
                    lLookupItem = 0

                ElseIf r_sItemDesc <> "" Then
                    v_vLookupItem = r_sItemDesc
                    lLookupItem = 1

                ElseIf r_sItemCode <> "" Then
                    v_vLookupItem = r_sItemCode
                    lLookupItem = 2
                End If

                ' loop around the available items for the specified table
                For lCntr As Integer = llBound To lUBound

                    ' get the code for the specified lookup items key

                    If CStr(v_vArray(lLookupItem, lCntr)).Trim() = v_vLookupItem Then

                        ' return the requested code, id, description

                        r_sItemDesc = CStr(v_vArray(ACDetailDesc, lCntr)).Trim()

                        r_sItemCode = CStr(v_vArray(ACDetailCode, lCntr)).Trim()

                        r_lItemId = CInt(CStr(v_vArray(ACDetailKey, lCntr)).Trim())

                        bFoundMatch = True
                        Exit For
                    End If

                Next lCntr

            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=DisableInterface(), excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            If Not bFoundMatch Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If




        End Try
        Return result
    End Function


    Private Function ConvertToSafe(ByVal ConvertType As Integer, ByVal value As String, Optional ByRef Default_Renamed As Decimal = 0, Optional ByRef ErrorOccurred As Boolean = False) As Double

        Dim result As Double = 0
        Try

            ErrorOccurred = False

            Select Case ConvertType
                Case gPMConstants.PMEDataType.PMCurrency
                    result = CDec(value)
                Case gPMConstants.PMEDataType.PMLong, gPMConstants.PMEDataType.PMInteger
                    result = CInt(value)
                Case gPMConstants.PMEDataType.PMBoolean
                    result = CBool(value)
                Case gPMConstants.PMEDataType.PMDouble
                    result = CDbl(value)
            End Select

            Return result

        Catch

            ErrorOccurred = True

            Return Default_Renamed
        End Try

    End Function

    '***************************************************************** '
    ' Name: SetupTaxesListView
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    '***************************************************************** '
    Private Function SetupTaxesListView() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupTaxesListView"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            lvwTaxesOnThisReceipt.Columns.Clear()

            lvwTaxesOnThisReceipt.Columns.Insert(kTaxDetColHIndexRecoveryType - 1, kTaxDetColHCodeRecoveryType, "Reserve Type", CInt(VB6.TwipsToPixelsX(1500)), HorizontalAlignment.Left, -1)

            lvwTaxesOnThisReceipt.Columns.Insert(kTaxDetColHIndexTaxGroup - 1, kTaxDetColHCodeTaxGroup, "Tax Group", CInt(VB6.TwipsToPixelsX(1600)), HorizontalAlignment.Left, -1)

            lvwTaxesOnThisReceipt.Columns.Insert(kTaxDetColHIndexTaxBand - 1, kTaxDetColHCodeTaxBand, "Tax Band", CInt(VB6.TwipsToPixelsX(1600)), HorizontalAlignment.Left, -1)

            lvwTaxesOnThisReceipt.Columns.Insert(kTaxDetColHIndexPercentage - 1, kTaxDetColHCodePercentage, "Percentage", CInt(VB6.TwipsToPixelsX(1300)), HorizontalAlignment.Right, -1)

            lvwTaxesOnThisReceipt.Columns.Insert(kTaxDetColHIndexTaxAmount - 1, kTaxDetColHCodeTaxAmount, "Amount", CInt(VB6.TwipsToPixelsX(1300)), HorizontalAlignment.Right, -1)


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: PopulateTaxesListView
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 31-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function PopulateTaxesListView() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulateTaxesListView"

        Dim lReturn As Integer
        Dim vTaxBandRateArray(,) As Object
        Dim llBound, lUBound As Integer
        Dim sTaxGroupDesc, sTaxBandDesc As String
        Dim dPercentage As Double
        Dim crValue As Decimal
        Dim oListItem As ListViewItem

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' clear down list before populating
            lvwTaxesOnThisReceipt.Items.Clear()

            ' for each payment item
            For Each oReceiptItem As cReceiptItem In m_colReceiptItems

                ' get the tax band rate array


                vTaxBandRateArray = oReceiptItem.TaxBandRateArray

                ' if there is a tax band rate array
                If Information.IsArray(vTaxBandRateArray) Then


                    llBound = vTaxBandRateArray.GetLowerBound(1)

                    lUBound = vTaxBandRateArray.GetUpperBound(1)

                    ' for each item in the tax band rate array
                    For lTaxItem As Integer = llBound To lUBound

                        ' get details

                        sTaxGroupDesc = CStr(vTaxBandRateArray(kTaxArrayTaxGroupDescription, lTaxItem))

                        sTaxBandDesc = CStr(vTaxBandRateArray(kTaxArrayTaxBandDescription, lTaxItem))

                        dPercentage = CDbl(vTaxBandRateArray(kTaxArrayPercentage, lTaxItem))

                        crValue = CDec(vTaxBandRateArray(kTaxArrayValue, lTaxItem))

                        ' add list item
                        oListItem = lvwTaxesOnThisReceipt.Items.Add("")

                        ' populate list item
                        oListItem.Text = oReceiptItem.RecoveryTypeDesc
                        ListViewHelper.GetListViewSubItem(oListItem, kTaxDetailsSubItemsTaxGroup).Text = sTaxGroupDesc
                        ListViewHelper.GetListViewSubItem(oListItem, kTaxDetailsSubItemsTaxBand).Text = sTaxBandDesc
                        ListViewHelper.GetListViewSubItem(oListItem, kTaxDetailsSubItemsPercentage).Text = StringsHelper.Format(dPercentage, "0.00")
                        ListViewHelper.GetListViewSubItem(oListItem, kTaxDetailsSubItemsTaxAmount).Text = StringsHelper.Format(crValue * oReceiptItem.ReceiptToLossXRate, "0.00")

                    Next

                End If
            Next oReceiptItem

            ' resize the list view dependant on how many tax rows there are....
            If lvwTaxesOnThisReceipt.Items.Count <= 7 Then
                lvwTaxesOnThisReceipt.Columns.Item(kTaxDetColHIndexRecoveryType - 1).Width = CInt(VB6.TwipsToPixelsX(1800))
            Else
                lvwTaxesOnThisReceipt.Columns.Item(kTaxDetColHIndexRecoveryType - 1).Width = CInt(VB6.TwipsToPixelsX(1500))
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SaveClaimReceipt() As Integer

        Dim nResult As Integer = 0
        Const kMethodName As String = "SaveClaimReceipt"
        Const kClaimReceiptId As Integer = 0
        Const kClaimid As Integer = 1
        Const kClaimPerilId As Integer = 2
        Const kDateOfReceipt As Integer = 3
        Const kPartyCnt As Integer = 4
        Const kAmount As Integer = 5
        Const kTaxAmount As Integer = 6
        Const kComments As Integer = 7
        Const kCreatedBy As Integer = 8
        Const kInsuredDomiciled As Integer = 9
        Const kInsuredPercentage As Integer = 10
        Const kInsuredTaxNumber As Integer = 11
        Const kReceivableTaxPercentage As Integer = 12
        Const kReceivableIsTaxExempt As Integer = 13
        Const kIsSettlement As Integer = 14
        Const kPayeeMediaTypeId As Integer = 15
        Const kPayeeName As Integer = 16
        Const kBankName As Integer = 17
        Const kBankSortCode As Integer = 18
        Const kBankAccountNo As Integer = 19
        Const kPayeeCountryId As Integer = 20
        Const kPayeeComments As Integer = 21
        Const kPayeeMediaRef As Integer = 22
        Const kDocumentId As Integer = 23
        Const kCurrencyId As Integer = 24
        Const kIsLive As Integer = 25
        Const kLiveClaimReceiptId As Integer = 26

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oSelectedPayeeId, oITDomiciled, oITTaxNumber, oITPercentage, oReceivableIsTaxExempt, oReceivableTaxPercentage, oIsSettlement, oPayeeMediaTypeId, oPayeeCountryId As Object
        Dim oPayeeBankName, oPayeeBankAccountNo, oPayeeBankSortCode, oPayeeComments, oPayeeName, oMediaRef As Object
        Dim nClaimReceiptId As Integer = 0
        Dim crTotalThisReceiptInPaymentCurrency As Decimal
        Dim crTotalThisReceiptTaxInPaymentCurrency As Decimal
        Dim crTotalThisReceiptTaxWHTInPaymentCurrency As Decimal
        Dim nCurrencyId As Integer = 0
        Dim oClaimReceiptDetails As Object = Nothing
        Dim sResult As String = ""
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            lReturn = CType(GetFormData(r_vSelectedPayeeId:=oSelectedPayeeId, r_vITDomiciled:=oITDomiciled, r_vITTaxNumber:=oITTaxNumber, r_vITPercentage:=oITPercentage, r_vReceivableTaxPercentage:=oReceivableTaxPercentage, r_vReceivableIsTaxExempt:=oReceivableIsTaxExempt, r_vIsSettlement:=oIsSettlement, r_vPayeeMediaTypeId:=oPayeeMediaTypeId, r_vPayeeCountryId:=oPayeeCountryId, r_vPayeeName:=oPayeeName, r_vPayeeBankName:=oPayeeBankName, r_vPayeeBankAccountNo:=oPayeeBankAccountNo, r_vPayeeBankSortCode:=oPayeeBankSortCode, r_vPayeeComments:=oPayeeComments, r_vMediaRef:=oMediaRef), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetFormData Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' GetReceiptTotalsInPaymentCurrency
            For Each oReceiptItem As cReceiptItem In m_colReceiptItems
                If m_bReceiptExcludeTax Then
                    crTotalThisReceiptInPaymentCurrency += oReceiptItem.ThisNet
                Else
                    crTotalThisReceiptInPaymentCurrency += oReceiptItem.ThisReceipt
                End If
                crTotalThisReceiptTaxInPaymentCurrency += oReceiptItem.TaxAmount
                nCurrencyId = oReceiptItem.CurrencyId
            Next oReceiptItem

            ReDim oClaimReceiptDetails(kLiveClaimReceiptId)

            oClaimReceiptDetails(kClaimid) = m_lClaimId

            oClaimReceiptDetails(kClaimPerilId) = m_lClaimPerilId

            oClaimReceiptDetails(kDateOfReceipt) = DateTime.Today

            oClaimReceiptDetails(kAmount) = crTotalThisReceiptInPaymentCurrency
            ''Start(Saurabh Agrawal)Tech Spec QBE004 Claim Recovery Reinsurance

            oClaimReceiptDetails(kTaxAmount) = crTotalThisReceiptTaxInPaymentCurrency
            oClaimReceiptDetails(kPartyCnt) = oSelectedPayeeId
            oClaimReceiptDetails(kComments) = DBNull.Value
            oClaimReceiptDetails(kCreatedBy) = m_oObjectManager.UserID
            oClaimReceiptDetails(kPayeeMediaTypeId) = oPayeeMediaTypeId
            oClaimReceiptDetails(kPayeeName) = oPayeeName
            oClaimReceiptDetails(kBankName) = oPayeeBankName
            oClaimReceiptDetails(kBankSortCode) = oPayeeBankSortCode
            oClaimReceiptDetails(kBankAccountNo) = oPayeeBankAccountNo
            oClaimReceiptDetails(kPayeeCountryId) = oPayeeCountryId


            oClaimReceiptDetails(kPayeeComments) = oPayeeComments

            oClaimReceiptDetails(kPayeeMediaRef) = oMediaRef

            oClaimReceiptDetails(kInsuredDomiciled) = oITDomiciled

            oClaimReceiptDetails(kInsuredPercentage) = oITPercentage

            oClaimReceiptDetails(kInsuredTaxNumber) = oITTaxNumber

            oClaimReceiptDetails(kReceivableIsTaxExempt) = oReceivableIsTaxExempt

            oClaimReceiptDetails(kIsSettlement) = oIsSettlement

            oClaimReceiptDetails(kDocumentId) = DBNull.Value

            oClaimReceiptDetails(kIsLive) = DBNull.Value

            oClaimReceiptDetails(kLiveClaimReceiptId) = DBNull.Value

            oClaimReceiptDetails(kCurrencyId) = gPMFunctions.ToSafeLong(nCurrencyId, 0)

            lReturn = m_oBusiness.SaveClaimReceipt(v_vClaimReceiptDetails:=oClaimReceiptDetails, r_lClaimReceiptId:=nCurrencyId)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SaveClaimReceipt Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            lReturn = CType(SaveClaimReceiptItems(v_lClaimReceiptId:=nCurrencyId), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "bCLMPeril.SaveClaimReceiptItems Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If g_sUnderwritingOrAgency = "U" Then

                lReturn = UpdateClaimReinsurance()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "CreateClaimReinsurance", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If
        Catch ex As Exception
            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)
            nResult = gPMConstants.PMEReturnCode.PMFalse
        End Try

        Return nResult
    End Function

    ' ***************************************************************** '
    ' Name: SaveClaimReceiptItems
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 01-09-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function SaveClaimReceiptItems(ByVal v_lClaimReceiptId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SaveClaimReceiptItems"

        Const kClaimReceiptItemId As Integer = 0
        Const kClaimReceiptId As Integer = 1
        Const kRecoveryId As Integer = 2
        Const kReserveId As Integer = 3
        Const kRecoveryTypeId As Integer = 4
        Const kCurrencyId As Integer = 5
        Const kThisReceipt As Integer = 6
        Const kTaxGroupId As Integer = 7
        Const kTaxAmount As Integer = 8
        Const kExchangeRateOverrideReasonId As Integer = 10
        Const kCurrencyToBaseXrate As Integer = 11
        Const kCurrencyToBaseDate As Integer = 12
        Const kAccountToBaseXrate As Integer = 13
        Const kAccountToBaseDate As Integer = 14
        Const kSystemToBaseXrate As Integer = 15
        Const kSystemToBaseDate As Integer = 16
        Const kReceiptToLossXrate As Integer = 17
        Const kIsLive As Integer = 18
        Const kLiveClaimReceiptItemId As Integer = 19
        Const kLiveClaimReceiptId As Integer = 20
        Const kLiveReserveId As Integer = 21
        Const kLiveRecoveryId As Integer = 22

        Dim iCounter As Integer
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lClaimReceiptItemId As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue
            iCounter = 1

            ' for each payment item in this payment
            For Each oReceiptItem As cReceiptItem In m_colReceiptItems

                If oReceiptItem.ThisReceipt <> 0 Then

                    ' prepare claim payment item array
                    Dim vClaimReceiptItem(kLiveRecoveryId) As Object

                    ' populate claim payment item array


                    vClaimReceiptItem(kClaimReceiptItemId) = DBNull.Value

                    vClaimReceiptItem(kClaimReceiptId) = v_lClaimReceiptId

                    vClaimReceiptItem(kRecoveryId) = oReceiptItem.RecoveryId


                    vClaimReceiptItem(kReserveId) = DBNull.Value

                    vClaimReceiptItem(kRecoveryTypeId) = oReceiptItem.RecoveryTypeId

                    vClaimReceiptItem(kCurrencyId) = oReceiptItem.CurrencyId

                    If oReceiptItem.TaxGroupId = 0 Then


                        vClaimReceiptItem(kTaxGroupId) = DBNull.Value
                    Else

                        vClaimReceiptItem(kTaxGroupId) = oReceiptItem.TaxGroupId
                    End If


                    vClaimReceiptItem(kThisReceipt) = oReceiptItem.ThisReceipt

                    vClaimReceiptItem(kTaxAmount) = oReceiptItem.TaxAmount

                    If oReceiptItem.ExchangeRateOverrideReasonId = 0 Then


                        vClaimReceiptItem(kExchangeRateOverrideReasonId) = DBNull.Value
                    Else

                        vClaimReceiptItem(kExchangeRateOverrideReasonId) = oReceiptItem.ExchangeRateOverrideReasonId
                    End If


                    vClaimReceiptItem(kCurrencyToBaseXrate) = oReceiptItem.CurrencyToBaseXRate

                    vClaimReceiptItem(kCurrencyToBaseDate) = oReceiptItem.CurrencyToBaseDate

                    vClaimReceiptItem(kAccountToBaseXrate) = oReceiptItem.AccountToBaseXRate

                    vClaimReceiptItem(kAccountToBaseDate) = oReceiptItem.AccountToBaseDate

                    vClaimReceiptItem(kSystemToBaseXrate) = oReceiptItem.SystemToBaseXRate

                    vClaimReceiptItem(kSystemToBaseDate) = oReceiptItem.SystemToBaseDate

                    vClaimReceiptItem(kReceiptToLossXrate) = oReceiptItem.ReceiptToLossXRate


                    vClaimReceiptItem(kIsLive) = DBNull.Value


                    vClaimReceiptItem(kLiveClaimReceiptId) = DBNull.Value


                    vClaimReceiptItem(kLiveRecoveryId) = DBNull.Value


                    vClaimReceiptItem(kLiveRecoveryId) = DBNull.Value


                    vClaimReceiptItem(kLiveClaimReceiptItemId) = DBNull.Value

                    ' save the claim payment item details

                    lReturn = m_oBusiness.SaveClaimReceiptItem(v_vClaimReceiptItem:=vClaimReceiptItem, r_lClaimReceiptItemId:=lClaimReceiptItemId)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "SaveClaimReceiptItem Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If


                    m_colReceiptItems(iCounter).WorkClaimReceiptItemId = lClaimReceiptItemId 'Sankar - PN 61750
                    ' save the tax entries for the claim payment item
                    lReturn = CType(SaveTaxCalculations(v_lClaimReceiptId:=v_lClaimReceiptId, v_lClaimReceiptItemId:=lClaimReceiptItemId, v_oReceiptItem:=oReceiptItem), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "bCLMPeril.SaveClaimReceiptItemTaxItems Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    ' update the associated reserve with the payment details
                    lReturn = CType(UpdateRecovery(oReceiptItem), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    ' save any associated insurance payments
                    lReturn = CType(SaveInsurancePayments(oReceiptItem), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "", gPMConstants.PMELogLevel.PMLogError)
                    End If
                    iCounter += 1
                End If

            Next oReceiptItem



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally





        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SaveTaxCalculations
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 01-09-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function SaveTaxCalculations(ByVal v_lClaimReceiptId As Integer, ByVal v_lClaimReceiptItemId As Integer, ByVal v_oReceiptItem As cReceiptItem) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SaveTaxCalculations"

        Const kWorkTaxCalculationCnt As Integer = 0
        Const kClaimPerilId As Integer = 1
        Const kClaimPaymentId As Integer = 2
        Const kClaimReceiptId As Integer = 3
        Const kClaimPaymentItemId As Integer = 4
        Const kClaimReceiptItemId As Integer = 5
        Const kTaxBandId As Integer = 6
        Const kPremium As Integer = 7
        Const kPercentage As Integer = 8
        Const kValue As Integer = 9
        Const kIsValue As Integer = 10
        Const kCurrencyId As Integer = 11
        Const kClassOfBusinessId As Integer = 12
        Const kTaxGroupId As Integer = 13
        Const kSequence As Integer = 14
        Const kTransType As Integer = 15
        Const kIsManuallyChanged As Integer = 16

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vTaxCalculation, vTaxBandRateArray(,) As Object
        Dim llBound, lUBound, lTaxCalculationCnt As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get the tax band rate array from the payment item


            vTaxBandRateArray = v_oReceiptItem.TaxBandRateArray

            ' if there are tax calculation entries
            If Information.IsArray(vTaxBandRateArray) Then

                ' get tax array boundaries

                llBound = vTaxBandRateArray.GetLowerBound(1)

                lUBound = vTaxBandRateArray.GetUpperBound(1)

                ' for each tax item
                For lTaxItem As Integer = llBound To lUBound

                    ' prepare tax calculation array
                    ReDim vTaxCalculation(kIsManuallyChanged)

                    ' populate array


                    vTaxCalculation(kWorkTaxCalculationCnt) = DBNull.Value

                    vTaxCalculation(kClaimPerilId) = m_lClaimPerilId


                    vTaxCalculation(kClaimPaymentId) = DBNull.Value

                    vTaxCalculation(kClaimReceiptId) = v_lClaimReceiptId


                    vTaxCalculation(kClaimPaymentItemId) = DBNull.Value

                    vTaxCalculation(kClaimReceiptItemId) = v_lClaimReceiptItemId



                    vTaxCalculation(kTaxBandId) = vTaxBandRateArray(kTaxArrayTaxBandId, lTaxItem)

                    vTaxCalculation(kPremium) = v_oReceiptItem.ThisReceipt


                    vTaxCalculation(kPercentage) = vTaxBandRateArray(kTaxArrayPercentage, lTaxItem)


                    vTaxCalculation(kValue) = vTaxBandRateArray(kTaxArrayValue, lTaxItem)


                    vTaxCalculation(kIsValue) = vTaxBandRateArray(kTaxArrayIsValue, lTaxItem)

                    vTaxCalculation(kCurrencyId) = v_oReceiptItem.CurrencyId


                    If CStr(vTaxBandRateArray(kTaxArrayClassOfBusinessId, lTaxItem)).Trim() = "" Then


                        vTaxCalculation(kClassOfBusinessId) = DBNull.Value
                    Else


                        vTaxCalculation(kClassOfBusinessId) = vTaxBandRateArray(kTaxArrayClassOfBusinessId, lTaxItem)
                    End If



                    vTaxCalculation(kTaxGroupId) = vTaxBandRateArray(kTaxArrayTaxGroupId, lTaxItem)


                    vTaxCalculation(kSequence) = vTaxBandRateArray(kTaxArraySequence, lTaxItem)

                    ' need to determine whether this is salvage receipting or third party recovery
                    If m_bIsSalvage Then

                        vTaxCalculation(kTransType) = kTaxTransTypeClaimSalvageReceipt
                    Else

                        vTaxCalculation(kTransType) = kTaxTransTypeClaimThirdPartyRecoveryReceipt
                    End If



                    vTaxCalculation(kIsManuallyChanged) = vTaxBandRateArray(kTaxArrayIsManuallyChanged, lTaxItem)

                    ' save the tax item to the database

                    lReturn = m_oBusiness.SaveTaxCalculationItem(v_vTaxCalculation:=vTaxCalculation, r_lTaxCalculationCnt:=lTaxCalculationCnt)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "bCLMPeril.SaveWorkTaxCalculation Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                Next

            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetSelectedComboItemData
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 05-09-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function GetSelectedComboItemData(ByRef r_oCombo As ComboBox, ByRef r_vSelectedItemId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetSelectedComboItemData"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' if no item is selected return null
            If r_oCombo.SelectedIndex = -1 Then

                r_vSelectedItemId = Nothing
            Else
                ' otherwise return item data (selected items key id)
                r_vSelectedItemId = VB6.GetItemData(r_oCombo, r_oCombo.SelectedIndex)
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetFormData
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 05-09-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    'developer guide no.101
    Private Function GetFormData(ByRef r_vSelectedPayeeId As Object, ByRef r_vITDomiciled As Object, ByRef r_vITTaxNumber As Object, ByRef r_vITPercentage As Object, ByRef r_vReceivableTaxPercentage As Object, ByRef r_vReceivableIsTaxExempt As Object, ByRef r_vIsSettlement As Object, ByRef r_vPayeeMediaTypeId As Object, ByRef r_vPayeeCountryId As Object, ByRef r_vPayeeName As Object, ByRef r_vPayeeBankName As Object, ByRef r_vPayeeBankAccountNo As Object, ByRef r_vPayeeBankSortCode As Object, ByRef r_vPayeeComments As Object, ByRef r_vMediaRef As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetFormData"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            '*************************
            ' party receipt to
            '*************************
            If m_lSelectedPayeeId = 0 Then

                r_vSelectedPayeeId = Nothing
            Else
                r_vSelectedPayeeId = m_lSelectedPayeeId
            End If

            '*************************
            ' insured tax adjustment
            '*************************
            If fraInsuredTaxAdjustment.Enabled Then

                If chkITDomiciled.CheckState = CheckState.Checked Then
                    r_vITDomiciled = 1
                Else
                    r_vITDomiciled = 0
                End If

                If txtITPercentage.Text = "" Then

                    r_vITPercentage = Nothing
                Else
                    r_vITPercentage = txtITPercentage.Text
                End If

                If txtITTaxNo.Text.Trim() = "" Then
                    r_vITTaxNumber = txtITTaxNo.Text
                Else

                    r_vITTaxNumber = Nothing
                End If

            Else

                r_vITDomiciled = Nothing

                r_vITPercentage = Nothing

                r_vITTaxNumber = Nothing
            End If

            '*************************
            ' tax exemptions
            '*************************
            If fraReceivableTaxStatus.Enabled Then
                If chkTaxExempt.CheckState = CheckState.Checked Then
                    r_vReceivableIsTaxExempt = 1
                Else
                    r_vReceivableIsTaxExempt = 0
                End If
            Else

                r_vReceivableIsTaxExempt = Nothing
            End If

            If txtReceivableTaxPercentage.Text <> "" Then
                r_vReceivableTaxPercentage = txtReceivableTaxPercentage.Text
            Else

                r_vReceivableTaxPercentage = Nothing
            End If

            '*************************
            ' settlement
            '*************************
            If fraSettlement.Enabled Then
                If chkSettlement.CheckState = CheckState.Checked Then
                    r_vIsSettlement = 1
                Else
                    r_vIsSettlement = 0
                End If
            Else

                r_vIsSettlement = Nothing
            End If


            '*************************
            ' payee details
            '*************************
            GetSelectedComboItemData(cboMediaType, r_vPayeeMediaTypeId)
            GetSelectedComboItemData(cboCountry, r_vPayeeCountryId)

            If txtPayeeName.Text.Trim() = "" Then

                r_vPayeeName = Nothing
            Else
                r_vPayeeName = txtPayeeName.Text.Trim()
            End If

            If txtBankName.Text.Trim() = "" Then

                r_vPayeeBankName = Nothing
            Else
                r_vPayeeBankName = txtBankName.Text.Trim()
            End If

            If txtBankCode.Text.Trim() = "" Then

                r_vPayeeBankSortCode = Nothing
            Else
                r_vPayeeBankSortCode = txtBankCode.Text.Trim()
            End If

            If txtBankAccountNo.Text.Trim() = "" Then

                r_vPayeeBankAccountNo = Nothing
            Else
                r_vPayeeBankAccountNo = txtBankAccountNo.Text.Trim()
            End If

            If txtPayeeComments.Text = "" Then

                r_vPayeeComments = Nothing
            Else
                r_vPayeeComments = txtPayeeComments.Text
            End If

            If txtMediaRef.Text.Trim() = "" Then

                r_vMediaRef = Nothing
            Else
                r_vMediaRef = txtMediaRef.Text.Trim()
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function
    ' ***************************************************************** '
    ' Name: GetSystemOptions
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function GetSystemOptions() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetSystemOptions"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sOptionNo As String = ""
        Dim bAllow_Negative_Reserve, bIs_Advanced_Tax_Script_Enabled As Boolean

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            '******************************
            ' advanced tax scripting
            '******************************
            lReturn = CType(GetProductDetailsForClaim(r_bIs_Advanced_Tax_Script_Enabled:=bIs_Advanced_Tax_Script_Enabled, r_bAllow_Negative_Reserve:=bAllow_Negative_Reserve), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetProductDetailsForClaim  Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_bAdvancedTaxScriptingOption = bIs_Advanced_Tax_Script_Enabled

            sOptionNo = "   "
            '******************************
            ' ATS Sattlement : 5071
            '******************************
            lReturn = CType(iPMFunc.GetSystemOption(v_iOptionNumber:=kSysOptionATSSattlement, r_sOptionValue:=sOptionNo, v_iSourceID:=m_iSourceID), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetSystemOption AdvancedTax Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If sOptionNo = "1" Then
                ' store system option value
                m_bATSSattlement = True
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: UpdateRecovery
    '
    ' Parameters: n/a
    '
    ' Description: Updates the reserve associated with the claim
    '               payment item according to payment / revision
    '
    ' History:
    '           Created : MEvans : 08-09-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function UpdateRecovery(ByVal v_oReceiptItem As cReceiptItem) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateRecovery"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lRecoveryId As Integer
        Dim crThisRevision, crThisReceipt, crTaxAmount As Decimal

        Try


            '

            result = gPMConstants.PMEReturnCode.PMTrue

            ' get claim payment item reserve id
            lRecoveryId = v_oReceiptItem.RecoveryId

            ' always update the recovery in loss currency

            If m_bReceiptExcludeTax Then
                crThisReceipt = v_oReceiptItem.ThisReceipt * v_oReceiptItem.ReceiptToLossXRate
            Else
                crThisReceipt = v_oReceiptItem.ThisNet * v_oReceiptItem.ReceiptToLossXRate
            End If

            crTaxAmount = v_oReceiptItem.TaxAmount * v_oReceiptItem.ReceiptToLossXRate

            crThisRevision = 0

            ' update the claim receipt items associated recovery entry

            lReturn = m_oBusiness.UpdateClaimReceiptItemRecovery(v_lRecoveryId:=lRecoveryId, v_crThisRevision:=crThisRevision, v_crThisReceipt:=crThisReceipt, v_crTaxAmount:=crTaxAmount)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "UpdateRecovery Failed", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SaveReceiptToAccounts
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function SaveReceiptToAccounts(ByVal v_lClaimReceiptId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SaveReceiptToAccounts"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim crTotalPaymentInPaymentCurrency As Decimal
        Dim sShortName As String = ""

        Dim crReceiptAmountGross, crReceiptAmountNet, crReceiptTaxAmount As Decimal

        Dim lClientAccountId As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            '    For Each oReceiptItem In m_colReceiptItems
            '        ' NB: the amount are in payment currency so no conversion required
            '        crReceiptAmountGross = crReceiptAmountGross + oReceiptItem.ThisReceipt
            '        If m_bReceiptExcludeTax Then
            '        crReceiptAmountNet = crReceiptAmountNet + oReceiptItem.ThisNet
            '        Else
            '            crReceiptAmountNet = crReceiptAmountNet + oReceiptItem.ThisReceipt
            '        End If
            '        crReceiptTaxAmount = crReceiptTaxAmount + oReceiptItem.TaxAmount
            '    Next

            If m_sClassOfBusinessCode <> "" Then

                If m_lSelectedPayeeId <> 0 Then

                    'get party name

                    lReturn = m_oBusiness.GetPartyName(v_lPartyCnt:=m_lSelectedPayeeId, v_sFieldName:="shortname", r_sResult:=sShortName)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "GetPartyName Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                Else
                    sShortName = kAccountCLMRECEIVABLE
                End If

                '**************************
                '* post receipt and taxes *
                '**************************
                If g_sUnderwritingOrAgency = "U" Then
                    For Each oReceiptItem As cReceiptItem In m_colReceiptItems
                        'Start - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc) - (4.3.3.1.3)
                        If oReceiptItem.RecoveryPartyCnt > 0 Then

                            lReturn = m_oBusiness.GetPartyName(v_lPartyCnt:=oReceiptItem.RecoveryPartyCnt, v_sFieldName:="shortname", r_sResult:=sShortName)
                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError(kMethodName, "GetPartyName Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If
                        End If

                        '           Start - Sankar - PN 61750
                        ' NB: the amount are in payment currency so no conversion required
                        crReceiptAmountGross = oReceiptItem.ThisReceipt

                        If m_bReceiptExcludeTax Then
                            crReceiptAmountNet = oReceiptItem.ThisNet
                        Else
                            crReceiptAmountNet = oReceiptItem.ThisReceipt - oReceiptItem.TaxAmount
                        End If

                        crReceiptTaxAmount = oReceiptItem.TaxAmount
                        'End - Sankar - PN 61750


                        lReturn = m_oBusiness.PostReceiptToOrion(v_bIsSalvage:=m_bIsSalvage, v_lClaimReceiptId:=v_lClaimReceiptId, v_sClaimNumber:=m_sClaimNumber, v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_lClaimId:=m_lClaimId, v_lClaimPerilId:=m_lClaimPerilId, v_crGrossReceiptAmount:=crReceiptAmountGross, v_crNetReceiptAmount:=crReceiptAmountNet, v_crTaxAmount:=oReceiptItem.TaxAmount, v_sDebitAccountCode:=sShortName, v_sCOBCode:=m_sClassOfBusinessCode, v_lCOBId:=m_lClassOfBusinessId, v_bPostClaimTax:=m_bPostClaimTax, v_lPartyCnt:=oReceiptItem.RecoveryPartyCnt, v_lClaimReceiptItemId:=oReceiptItem.WorkClaimReceiptItemId)
                        'End - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc) - (4.3.3.1.3)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "PostReceiptToOrion Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                    Next oReceiptItem
                Else

                    lReturn = m_oBusiness.PostBrokingReceiptToOrion(v_lClaimReceiptId:=v_lClaimReceiptId, v_lClaimId:=m_lClaimId, v_sClaimNumber:=m_sClaimNumber, v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_cReceiptAmount:=crReceiptAmountGross, v_sDebitAccountCode:=sShortName.Trim(), v_bPostClaimTax:=m_bPostClaimTax, v_lMediaType:=0, v_sComments:="", v_lPartyCnt:=m_lSelectedPayeeId, v_vInsurerDetails:=m_vCoinsurance, r_lClientAccountId:=lClientAccountId)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "PostBrokingReceiptToOrion Failed", gPMConstants.PMELogLevel.PMLogError)
                    Else
                        m_lCashListAccountId = lClientAccountId
                        m_lCashListPartyId = m_lSelectedPayeeId
                        m_crCashListAmount = crReceiptAmountGross
                    End If
                End If
            Else
                gPMFunctions.RaiseError(kMethodName, "SaveReceiptsToAccounts Failed - No Class Of Business Code Available", gPMConstants.PMELogLevel.PMLogError)
            End If




        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetPaymentCurrencyFilter
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 09-09-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function GetPaymentCurrencyFilter(ByRef r_lPaymentCurrencyFilter As Integer, ByRef r_oReceiptItem As cReceiptItem) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPaymentCurrencyFilter"

        Dim lReturn As Integer
        Dim oReceiptItem As cReceiptItem

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If m_colReceiptItems.Count > 1 Then

                r_lPaymentCurrencyFilter = m_colReceiptItems.Item(1).CurrencyId



            End If







        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: ActionDelete
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 09-09-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function ActionDelete() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ActionDelete"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sRecoveryId As String = ""
        Dim oListItem As ListViewItem

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get the selected list item
            oListItem = lvwRecovery.Items.Item(m_lSelectedPayeeDetailIndex - 1)

            ' get the payment items reserve details
            sRecoveryId = oListItem.Text

            ' remove the item from the collection
            m_colReceiptItems.Remove(sRecoveryId)

            ' reset the selected receipt details on the grid
            ListViewHelper.GetListViewSubItem(oListItem, kRecDetSubItemsThisReceipt).Text = StringsHelper.Format(0, "0.00")
            ListViewHelper.GetListViewSubItem(oListItem, kRecDetSubItemsTaxAmount).Text = StringsHelper.Format(0, "0.00")
            ListViewHelper.GetListViewSubItem(oListItem, kRecDetSubItemsThisNet).Text = StringsHelper.Format(0, "0.00")
            ListViewHelper.GetListViewSubItem(oListItem, kRecDetSubItemsBalance).Text = StringsHelper.Format(0, "0.00")

            ' populate this payment details
            lReturn = PopulateThisReceiptDetails()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "PopulateThisReceiptDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' populate total line
            lReturn = PopulateTotals()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "PopulateTotals Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' set up this payment form
            lReturn = SetupThisReceiptInterface()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetupThisReceiptInterface", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SetupDefaultInsuredTaxStatus
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function SetupDefaultInsuredTaxStatus() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupDefaultInsuredTaxStatus"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            '******************************************************
            ' populate insured tax adjustments with client details
            If m_bClientDomiciledForTax Or m_bAdvancedTaxScriptingOption Then
                chkITDomiciled.CheckState = CheckState.Checked
            Else
                chkITDomiciled.CheckState = CheckState.Unchecked
            End If

            txtITPercentage.Text = StringsHelper.Format(m_crClientTaxPercentage, "0.00")
            txtITTaxNo.Text = m_sClientTaxNumber
            If m_crClientTaxPercentage > 0 And m_bAdvancedTaxScriptingOption Then
                lblITTaxNo.Enabled = True
                txtITTaxNo.Enabled = True
                txtITPercentage.Enabled = True
                lblITPercentage.Enabled = True
            End If
            '******************************************************


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: UpdateReceiptDetailListView
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function UpdateReceiptDetailListView(ByRef r_oReceiptItem As cReceiptItem, ByRef r_oListItem As ListViewItem) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateReceiptDetailListView"

        Dim lReturn As Integer
        Dim crThisReceipt, crTaxAmount, crThisNet, crBalance, crTotalReserve, crTotalRecievedToDate As Decimal
        Dim sResult As String = ""
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' get receipt details totals
            crThisReceipt = r_oReceiptItem.ThisReceipt * r_oReceiptItem.ReceiptToLossXRate
            crTaxAmount = r_oReceiptItem.TaxAmount * r_oReceiptItem.ReceiptToLossXRate

            'PN-61621 (Sushil Kumar)
            If m_bReceiptExcludeTax Then
                crThisNet = crThisReceipt + crTaxAmount
            Else
                crThisNet = crThisReceipt - crTaxAmount
            End If

            ' NB: Recovery Total Already In Loss Currency
            crTotalReserve = r_oReceiptItem.TotalReserve
            crTotalRecievedToDate = r_oReceiptItem.ReceivedToDate

            If m_bReceiptExcludeTax Then
                crBalance = crTotalReserve - crTotalRecievedToDate - crThisNet + crTaxAmount
            Else
                crBalance = crTotalReserve - crTotalRecievedToDate - crThisNet - crTaxAmount
            End If

            ' update the receipt details listview with the new values
            ListViewHelper.GetListViewSubItem(r_oListItem, kRecDetSubItemsThisReceipt).Text = StringsHelper.Format(crThisReceipt, "0.00")
            ListViewHelper.GetListViewSubItem(r_oListItem, kRecDetSubItemsTaxAmount).Text = StringsHelper.Format(crTaxAmount, "0.00")
            ListViewHelper.GetListViewSubItem(r_oListItem, kRecDetSubItemsThisNet).Text = StringsHelper.Format(crThisNet, "0.00")
            ListViewHelper.GetListViewSubItem(r_oListItem, kRecDetSubItemsBalance).Text = StringsHelper.Format(crBalance, "0.00")



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    Private Sub txtReceivableTaxPercentage_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtReceivableTaxPercentage.Enter
        ValidatePercentage(txtReceivableTaxPercentage)
    End Sub
    Private Sub txtReceivableTaxPercentage_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtReceivableTaxPercentage.Leave
        ValidatePercentage(txtReceivableTaxPercentage)
    End Sub

    ' ***************************************************************** '
    ' Name: EnableDisablePayeeDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 23-09-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Public Function EnableDisablePayeeDetails(ByVal v_bEnabled As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "EnableDisablePayeeDetails"

        Dim lReturn As Integer
        Dim bContinue As Boolean

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' this functionality is only supported when
            ' the advanced tax scripting option is switched on
            If m_bAdvancedTaxScriptingOption Then

                ' default
                bContinue = True

                If v_bEnabled Then
                    lReturn = MessageBox.Show("Changing the payee or tax details will result in all " & _
                              "receipts added in this session being cleared down. " & _
                              "Do you wish to continue?", "Edit Payee Details", MessageBoxButtons.YesNo)

                    If lReturn = System.Windows.Forms.DialogResult.No Then
                        bContinue = False
                    End If
                End If

                If bContinue Then

                    ' enable / disable all payee details which
                    ' can effect tax calculations
                    fraPayee.Enabled = v_bEnabled
                    fraSettlement.Enabled = v_bEnabled
                    fraReceivableTaxStatus.Enabled = v_bEnabled
                    fraInsuredTaxAdjustment.Enabled = v_bEnabled

                    ' if the payee details are to be reenabled
                    ' clear all payment details from this session
                    If v_bEnabled Then

                        ' reset all receipt lines
                        lReturn = ResetThisReceipt()
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "", gPMConstants.PMELogLevel.PMLogError)
                        End If

                        ' populate this receipt details
                        lReturn = PopulateThisReceiptDetails()
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "PopulateThisPaymentDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                        ' populate total line
                        lReturn = PopulateTotals()
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "PopulateTotals Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                        ' set up this receipt form
                        lReturn = SetupThisReceiptInterface()
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "SetupThisPaymentInterface", gPMConstants.PMELogLevel.PMLogError)
                        End If

                    End If

                    cmdEditPayee.Enabled = Not v_bEnabled

                End If
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetDefaultTaxItem
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function GetDefaultTaxItem(ByRef r_oTaxItem As cTaxParameters) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetDefaultTaxItem"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            r_oTaxItem = New cTaxParameters()

            ' set interface defaults

            r_oTaxItem.InsuredDomiciled = chkITDomiciled.CheckState = CheckState.Checked

            r_oTaxItem.InsuredPercentage = gPMFunctions.ToSafeCurrency(txtITPercentage.Text, 0)

            r_oTaxItem.InsuredTaxNumber = txtITTaxNo.Text

            r_oTaxItem.IsSettlement = chkSettlement.CheckState = CheckState.Checked

            r_oTaxItem.IsTaxExempt = chkTaxExempt.CheckState = CheckState.Checked

            r_oTaxItem.Payee = m_sSelectedPayee

            r_oTaxItem.ProcessType = "CLR"

            r_oTaxItem.ReceivablePercentage = gPMFunctions.ToSafeDouble(txtReceivableTaxPercentage.Text, 0)


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetCoinsurance
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 05-10-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function GetCoinsurance() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetCoinsurance"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            lReturn = m_oBusiness.GetCoinsurance(v_lClaimPerilId:=m_lClaimPerilId, v_bIsSalvage:=m_bIsSalvage, r_vResults:=m_vCoinsurance)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "bCLMPeril.GetCoinsurance Failed", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetReinsurance
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 05-10-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function GetReinsurance() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetReinsurance"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            lReturn = m_oBusiness.GetReinsurance(v_lClaimPerilId:=m_lClaimPerilId, v_bIsSalvage:=m_bIsSalvage, r_vResults:=m_vReinsurance)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "bCLMPeril.GetReinsurance Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Sort the array
            If Information.IsArray(m_vReinsurance) Then
                lReturn = CType(SortRIArray2007(m_vReinsurance), gPMConstants.PMEReturnCode)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function


    '***************************************************************** '
    ' Name: SetupInsurerListView
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    '***************************************************************** '
    Private Function SetupInsurerListView(ByVal v_bIsCoinsurance As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupInsurerListView"

        Dim lReturn As Integer
        Dim lvwInsurer As ListView
        Dim sInsurer As String = ""

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' determine if this is reinsurance or coinsurance were setting up
            ' and use appropriate listview
            If v_bIsCoinsurance Then
                lvwInsurer = lvwCoinsurance
                sInsurer = "Coinsurer"
            Else
                lvwInsurer = lvwReinsurance
                sInsurer = "Reinsurer"
            End If
            lvwInsurer.Columns.Clear()
            lvwInsurer.Columns.Insert(kInsuranceColHIndexRecoveryId - 1, kInsuranceColHCodeRecoveryId, "", 0, HorizontalAlignment.Left, -1)
            lvwInsurer.Columns.Insert(kInsuranceColHIndexRecoveryTypeDesc - 1, kInsuranceColHCodeRecoveryTypeDesc, "Recovery Type", 133, HorizontalAlignment.Left, -1)
            lvwInsurer.Columns.Insert(kInsuranceColHIndexPartyId - 1, kInsuranceColHCodePartyId, "", 0, HorizontalAlignment.Left, -1)
            lvwInsurer.Columns.Insert(kInsuranceColHIndexPartyName - 1, kInsuranceColHCodePartyName, sInsurer, 70, HorizontalAlignment.Left, -1)
            lvwInsurer.Columns.Insert(kInsuranceColHIndexSharePercent - 1, kInsuranceColHCodeSharePercent, "Share Percent", 105, HorizontalAlignment.Right, -1)
            lvwInsurer.Columns.Insert(kInsuranceColHIndexRecoveryToDate - 1, kInsuranceColHCodeRecoveryToDate, "Recovery To Date", 133, HorizontalAlignment.Right, -1)
            lvwInsurer.Columns.Insert(kInsuranceColHIndexThisRecovery - 1, kInsuranceColHCodeThisRecovery, "Total This Recovery", 133, HorizontalAlignment.Right, -1)
            lvwInsurer.Columns.Insert(kInsuranceColHIndexThisRecoverySplit - 1, kInsuranceColHCodeThisRecoverySplit, "This Recovery (Split)", 133, HorizontalAlignment.Right, -1)



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    '***************************************************************** '
    ' Name: PopulateInsurerCollection
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    '***************************************************************** '
    Private Function PopulateInsurerCollection(ByVal v_bIsCoinsurance As Boolean, Optional ByVal v_lApplyRecoveryTypeFilter As Integer = 0) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulateInsurerCollection"

        Dim lReturn As Integer
        Dim lvwInsurance As ListView
        Dim sInsurer As String = ""
        Dim vInsurer(,) As Object
        Dim llBound, lUBound, lRecoveryId As Integer
        Dim sRecoveryType As String = ""
        Dim lPartyCnt As Integer
        Dim sPartyName As String = ""
        Dim crSharePercent, crRecoveryToDate As Decimal
        Dim lIsTaxShared As Integer
        Dim oListItem As ListViewItem
        Dim lRIArrangementLine, lTreatyId As Integer
        Dim colInsurance As Collection
        Dim oInsurance As cInsurance

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' determine if this is reinsurance or coinsurance were setting up
            ' and use appropriate listview
            If v_bIsCoinsurance Then
                lvwInsurance = lvwCoinsurance
                colInsurance = m_colCoinsurers

                vInsurer = VB6.CopyArray(m_vCoinsurance)

            Else
                lvwInsurance = lvwReinsurance
                colInsurance = m_colReinsurers

                vInsurer = VB6.CopyArray(m_vReinsurance)
            End If

            If Information.IsArray(vInsurer) Then

                ' get array boundaries

                llBound = vInsurer.GetLowerBound(1)

                lUBound = vInsurer.GetUpperBound(1)

                ' clear down list
                lvwInsurance.Items.Clear()

                ' for each insurer (coinsurer / reinsurer)
                For lInsurer As Integer = llBound To lUBound

                    ' get details
                    lRecoveryId = gPMFunctions.ToSafeLong(vInsurer(kInsuranceDetailRecoveryId, lInsurer), 0)

                    sRecoveryType = CStr(vInsurer(kInsuranceDetailRecoveryTypeDesc, lInsurer)).Trim()
                    lPartyCnt = gPMFunctions.ToSafeLong(vInsurer(kInsuranceDetailPartyCnt, lInsurer), 0)

                    sPartyName = CStr(vInsurer(kInsuranceDetailPartyName, lInsurer)).Trim()
                    crSharePercent = gPMFunctions.ToSafeCurrency(vInsurer(kInsuranceDetailPartyShare, lInsurer), 0)
                    crRecoveryToDate = gPMFunctions.ToSafeCurrency(vInsurer(kInsuranceDetailRecoveryToDate, lInsurer), 0)
                    lIsTaxShared = gPMFunctions.ToSafeLong(vInsurer(kInsuranceDetailIsTaxShared, lInsurer), 0)

                    ' get additional details provided for reinsurance
                    If Not v_bIsCoinsurance Then
                        lRIArrangementLine = gPMFunctions.ToSafeLong(vInsurer(kInsuranceDetailRIArrangementLineId, lInsurer), 0)
                        lTreatyId = gPMFunctions.ToSafeLong(vInsurer(kInsuranceDetailTreatyId, lInsurer), 0)

                    End If

                    ' create insurance item
                    oInsurance = New cInsurance()

                    ' populate insurance details
                    oInsurance.RecoveryId = lRecoveryId
                    oInsurance.RecoveryTypeDescription = sRecoveryType
                    oInsurance.RecoveryToDateLC = crRecoveryToDate
                    oInsurance.SharePercentage = crSharePercent
                    oInsurance.IsTaxShared = lIsTaxShared
                    oInsurance.PartyCnt = lPartyCnt
                    oInsurance.PartyName = sPartyName
                    oInsurance.ThisRecoveryAmount = 0
                    oInsurance.ThisRecoveryTaxAmount = 0

                    If Not v_bIsCoinsurance Then
                        oInsurance.LowerLimit = gPMFunctions.ToSafeCurrency(vInsurer(kInsuranceDetailLowerLimit, lInsurer), 0)
                        oInsurance.UpperLimit = gPMFunctions.ToSafeCurrency(vInsurer(kInsuranceDetailUpperLimit, lInsurer), 0)

                        oInsurance.RIType = CStr(vInsurer(kInsuranceDetailType, lInsurer))
                        oInsurance.PaidToDate = gPMFunctions.ToSafeCurrency(vInsurer(kInsuranceDetailPayment, lInsurer), 0)
                    End If

                    ' save additional details provided for reinsurance
                    If Not v_bIsCoinsurance Then

                        oInsurance.RIArrangementLineId = lRIArrangementLine
                        ' NB: Faculative Reinsurance doesnt have a treaty id
                        ' this needs the treaty id to be reset to Null
                        If lTreatyId <> 0 Then

                            oInsurance.TreatyId = lTreatyId
                        Else

                            oInsurance.TreatyId = Nothing
                        End If
                    Else

                        oInsurance.RIArrangementLineId = Nothing

                        oInsurance.TreatyId = Nothing
                    End If

                    ' add insurance item to the collection
                    If v_bIsCoinsurance Then
                        colInsurance.Add(oInsurance, CStr(lRecoveryId) & ":C" & CStr(lPartyCnt))
                    Else
                        'colInsurance.Add oInsurance, CStr(lRecoveryId & ":" & lTreatyId)
                        colInsurance.Add(oInsurance, CStr(lRecoveryId) & ":" & CStr(lRIArrangementLine))
                    End If

                Next

            End If

            ' populate the list view
            PopulateInsurerListView(v_bIsCoinsurance)

            ' autosize the list view
            AutosizeListView(lvwInsurance)



        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: AutosizeListView
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function AutosizeListView(ByVal v_oListView As ListView) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AutosizeListView"

        Const CELL_PADDING As Single = 240
        Const ICON_PADDING As Single = 240

        Dim lReturn, lColumns, lWidth As Integer
        Dim vArray() As Object
        Dim lMaxWidth As Integer

        Dim oFind As Object

        Try

            'Developer Guide no. 50
            frmReceiptDetails = New frmReceiptDetails
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get column count
            lColumns = v_oListView.Columns.Count

            ' Make an array to store the widths in
            ReDim vArray(lColumns - 1)

            ' ensure that the minimum width is at least that of the header
            For Each oHeader As ColumnHeader In v_oListView.Columns

                vArray(oHeader.Index) = VB6.PixelsToTwipsX(frmReceiptDetails.CreateGraphics().MeasureString(oHeader.Text, frmReceiptDetails.Font).Width)
            Next oHeader

            ' if any items require a larger width than that
            ' of the header use the width of the item instead
            For Each oListItem As ListViewItem In v_oListView.Items

                ' Do the first column
                lWidth = CInt(VB6.PixelsToTwipsX(frmReceiptDetails.CreateGraphics().MeasureString(oListItem.Text, frmReceiptDetails.Font).Width))

                If lWidth > CDbl(vArray(1)) Then

                    vArray(1) = lWidth
                End If

                ' And the sub columns
                For lCount As Integer = 2 To lColumns
                    lWidth = CInt(VB6.PixelsToTwipsX(frmReceiptDetails.CreateGraphics().MeasureString(ListViewHelper.GetListViewSubItem(oListItem, lCount - 1).Text, frmReceiptDetails.Font).Width))

                    If lWidth > CDbl(vArray(lCount - 1)) Then

                        vArray(lCount - 1) = lWidth
                    End If
                Next lCount

            Next oListItem

            ' Add a little extra for the icon if there is one


            vArray(1) = CDbl(vArray(1)) + ICON_PADDING

            ' Now set the column header widths
            For Each oHeader As ColumnHeader In v_oListView.Columns

                ' only resize if the initial width is set to 1 (AUTO)
                If VB6.PixelsToTwipsX(oHeader.Width) >= 1 And VB6.PixelsToTwipsX(oHeader.Width) < 2 Then


                    lWidth = CInt(CDbl(vArray(oHeader.Index + 1)) + CELL_PADDING)

                    oHeader.Width = CInt(VB6.TwipsToPixelsX(lWidth))

                End If

            Next oHeader



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    '                       PRIVATE FUNCTIONS
    ' ***************************************************************** '
    '
    ' Recalculate coinsurance and reinsurance splits
    Private Function RecalculateInsurance(ByVal v_oReceipt As cReceiptItem) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "RecalculateInsurance"

        Dim crReinsuranceAmount, crReinsuranceTaxAmount, crTotalAmount, crTotalTaxAmount, crThisPayment, crThisTax As Decimal
        Dim lCount, lReturn As Integer
        Dim vValue As String = ""
        Dim cRunningAmount, cSharePercent As Decimal
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If we are posting taxes reinsure them seperately, else rollup into receipt
            If m_bPostClaimTax Then
                'Start - Sankar - PN 57829
                If m_bReceiptExcludeTax Then
                    crReinsuranceAmount = v_oReceipt.ThisReceipt
                Else
                    crReinsuranceAmount = v_oReceipt.ThisReceipt - v_oReceipt.TaxAmount
                End If
                'End - Sankar - PN 57829
                crTotalAmount = v_oReceipt.ThisNet
                crReinsuranceTaxAmount = v_oReceipt.TaxAmount
                crTotalTaxAmount = v_oReceipt.TaxAmount
            Else
                crReinsuranceAmount = v_oReceipt.ThisReceipt
                crTotalAmount = v_oReceipt.ThisReceipt
                crReinsuranceTaxAmount = v_oReceipt.TaxAmount
                crTotalTaxAmount = v_oReceipt.TaxAmount
            End If


            cRunningAmount = crReinsuranceAmount

            ' for each coinsurance item in the collection
            For Each oInsurance As cInsurance In m_colCoinsurers

                ' if its for the selected recovery
                If oInsurance.RecoveryId = v_oReceipt.RecoveryId Then

                    ' Calculate payment and adjust amount available for reinsurance
                    crThisPayment = crTotalAmount * (oInsurance.SharePercentage / 100)
                    crReinsuranceAmount -= crThisPayment

                    ' Set payment share and loss equvalent
                    oInsurance.ThisRecoveryAmount = crTotalAmount
                    oInsurance.ThisRecoverySplitAmount = crThisPayment
                    oInsurance.ReceiptToLossXRate = v_oReceipt.ReceiptToLossXRate

                    ' Calculate tax and adjust amount available for reinsurance
                    crThisTax = crTotalTaxAmount * (oInsurance.SharePercentage / 100)
                    crReinsuranceTaxAmount -= crThisTax

                    ' Set tax ... only if shared with CI
                    If oInsurance.IsTaxShared Or Not m_bPostClaimTax Then
                        ' Set tax share and loss equivalent
                        oInsurance.ThisRecoveryTaxAmount = crTotalTaxAmount
                        oInsurance.ThisRecoverySplitTaxAmount = crThisTax
                    End If

                End If

            Next oInsurance


            lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableRI2007, v_vBranch:=1, r_vUnderwriting:=vValue)



            ' for each reinsurance item in the collection
            'if Product Option is switched off then go with this code
            If StringsHelper.ToDoubleSafe(vValue) = 0 Or vValue = "" Then
                For Each oInsurance As cInsurance In m_colReinsurers

                    ' if its for the selected recovery
                    If oInsurance.RecoveryId = v_oReceipt.RecoveryId Then

                        ' Calculate payment
                        crThisPayment = crReinsuranceAmount * (oInsurance.SharePercentage / 100)

                        ' Set payment share and loss equvalent
                        oInsurance.ThisRecoveryAmount = crReinsuranceAmount
                        oInsurance.ThisRecoverySplitAmount = crThisPayment
                        oInsurance.ReceiptToLossXRate = v_oReceipt.ReceiptToLossXRate

                        ' Calculate tax
                        crThisTax = crReinsuranceTaxAmount * (oInsurance.SharePercentage / 100)

                        ' Set tax ... only if shared with RI
                        If oInsurance.IsTaxShared Or Not m_bPostClaimTax Then
                            ' Set tax share and loss equivalent
                            oInsurance.ThisRecoveryTaxAmount = crReinsuranceTaxAmount
                            oInsurance.ThisRecoverySplitTaxAmount = crThisTax
                        End If

                    End If

                Next oInsurance

            ElseIf StringsHelper.ToDoubleSafe(vValue) = 1 Then

                For Each oInsurance As cInsurance In m_colReinsurers

                    ' if its for the selected recovery
                    If oInsurance.RecoveryId = v_oReceipt.RecoveryId Then
                        If cRunningAmount > 0 Then
                            oInsurance.ThisRecoveryAmount = crReinsuranceAmount
                            oInsurance.ReceiptToLossXRate = v_oReceipt.ReceiptToLossXRate

                            If oInsurance.RIType = "TX" Or oInsurance.RIType = "FX" Then

                                '                            If oInsurance.PaidToDate >= cRunningAmount Then
                                '                                oInsurance.ThisRecoverySplitAmount = cRunningAmount - oInsurance.RecoveryToDateLC
                                '                                'oInsurance.ThisRecoveryTaxAmount = cRunningAmount
                                '                            ElseIf oInsurance.PaidToDate < cRunningAmount Then
                                '                                oInsurance.ThisRecoverySplitAmount = oInsurance.PaidToDate - oInsurance.RecoveryToDateLC
                                '                            End If

                                If oInsurance.PaidToDate - oInsurance.RecoveryToDateLC > cRunningAmount Then
                                    oInsurance.ThisRecoverySplitAmount = cRunningAmount
                                Else
                                    oInsurance.ThisRecoverySplitAmount = oInsurance.PaidToDate - oInsurance.RecoveryToDateLC
                                End If

                            ElseIf oInsurance.RIType = "R" Then
                                oInsurance.ThisRecoverySplitAmount = cRunningAmount
                            Else
                                crThisPayment = crReinsuranceAmount * (oInsurance.SharePercentage / 100)
                                ' Set payment share and loss equvalent
                                oInsurance.ThisRecoveryAmount = crReinsuranceAmount
                                oInsurance.ThisRecoverySplitAmount = crThisPayment
                                oInsurance.ReceiptToLossXRate = v_oReceipt.ReceiptToLossXRate
                            End If
                            If (crReinsuranceAmount <> 0) Then
                                cSharePercent = oInsurance.ThisRecoverySplitAmount / crReinsuranceAmount

                                ' Calculate tax
                                crThisTax = crReinsuranceTaxAmount * (cSharePercent)
                            End If
                            ' Set tax ... only if shared with RI
                            If oInsurance.IsTaxShared Or Not m_bPostClaimTax Then
                                ' Set tax share and loss equivalent
                                oInsurance.ThisRecoveryTaxAmount = crReinsuranceTaxAmount
                                oInsurance.ThisRecoverySplitTaxAmount = crThisTax
                            End If


                            ' Update the Ruuning Amount
                            cRunningAmount -= oInsurance.ThisRecoverySplitAmount
                        End If
                    End If

                Next oInsurance

            End If
            'Else
            'Call the SP to update the recoveries

            'End if

            ' rebuild coinsurance / reinsurance displays
            lReturn = PopulateInsurerListView(True, 0)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "PopulateInsurerListView Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            lReturn = PopulateInsurerListView(False, 0)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "PopulateInsurerListView Failed", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


            ' This is for debugging only



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: PopulateInsurerListView
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function PopulateInsurerListView(ByVal v_bIsCoinsurance As Boolean, Optional ByVal v_lApplyRecoveryTypeFilter As Integer = 0) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulateInsurerListView"

        Dim lReturn As Integer
        Dim colInsurance As Collection
        Dim lvwInsurance As ListView
        Dim oInsurance As cInsurance
        Dim oListItem As ListViewItem

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' if this is coinsurance
            If v_bIsCoinsurance Then
                lvwInsurance = lvwCoinsurance
                colInsurance = m_colCoinsurers
            Else
                ' else reinsurance
                lvwInsurance = lvwReinsurance
                colInsurance = m_colReinsurers
            End If

            ' clear down list
            lvwInsurance.Items.Clear()

            ' for each insurance item in the collection
            For lInsurance As Integer = 1 To colInsurance.Count

                ' get insurance object
                oInsurance = colInsurance.Item(lInsurance)

                ' if no filter has been applied or the filter matches the current objects recovery type
                If v_lApplyRecoveryTypeFilter = 0 Or v_lApplyRecoveryTypeFilter = oInsurance.RecoveryId Then

                    ' add an item to the listview
                    oListItem = lvwInsurance.Items.Add(CStr(oInsurance.RecoveryId))

                    ' populate the listitem's details
                    ListViewHelper.GetListViewSubItem(oListItem, kInsuranceSubItemsRecoveryTypeDesc).Text = oInsurance.RecoveryTypeDescription
                    ListViewHelper.GetListViewSubItem(oListItem, kInsuranceSubItemsPartyId).Text = CStr(oInsurance.PartyCnt)
                    ListViewHelper.GetListViewSubItem(oListItem, kInsuranceSubItemsPartyName).Text = oInsurance.PartyName
                    ListViewHelper.GetListViewSubItem(oListItem, kInsuranceSubItemsSharePercent).Text = StringsHelper.Format(oInsurance.SharePercentage, "0.00")
                    ListViewHelper.GetListViewSubItem(oListItem, kInsuranceSubItemsRecoveryToDate).Text = StringsHelper.Format(oInsurance.RecoveryToDateLC, "0.00")
                    ListViewHelper.GetListViewSubItem(oListItem, kInsuranceSubItemsThisRecovery).Text = StringsHelper.Format(oInsurance.ThisRecoveryAmountLC, "0.00")
                    ListViewHelper.GetListViewSubItem(oListItem, kInsuranceSubItemsThisRecoverySplit).Text = StringsHelper.Format(oInsurance.ThisRecoverySplitAmountLC, "0.00")

                End If

            Next



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: SaveInsurancePayments
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 07-10-2005 : Process ID
    ' ***************************************************************** '
    Private Function SaveInsurancePayments(ByVal v_oReceiptItem As cReceiptItem) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SaveInsurancePayments"

        Dim lReturn As gPMConstants.PMEReturnCode


        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            For Each oInsurance As cInsurance In m_colCoinsurers

                If oInsurance.RecoveryId = v_oReceiptItem.RecoveryId Then

                    ' if there is an amount to pay
                    lReturn = CType(SaveClaimPayment(v_oReceiptItem:=v_oReceiptItem, v_oInsurance:=oInsurance), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "SaveClaimPayment", gPMConstants.PMELogLevel.PMLogError)
                    End If

                End If

            Next oInsurance

            For Each oInsurance As cInsurance In m_colReinsurers

                If oInsurance.RecoveryId = v_oReceiptItem.RecoveryId Then

                    ' if there is an amount to pay
                    lReturn = CType(SaveClaimPayment(v_oReceiptItem:=v_oReceiptItem, v_oInsurance:=oInsurance), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "SaveClaimPayment", gPMConstants.PMELogLevel.PMLogError)
                    End If

                End If

            Next oInsurance



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ''' <summary>
    ''' SaveClaimPayment
    ''' </summary>
    ''' <param name="v_oReceiptItem"></param>
    ''' <param name="v_oInsurance"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SaveClaimPayment(ByVal v_oReceiptItem As cReceiptItem, ByVal v_oInsurance As cInsurance) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Const kMethodName As String = "SaveClaimPayment"
        Dim nReturn As PMEReturnCode
        Dim oPaymentpartyTo, oClaimPaymentToId, oSelectedPayeeId, oITDomiciled As Object
        Dim oITTaxNumber, oITPercentage, oIsTaxExempt, oIsSettlement, oPayeeMediaTypeId As Object
        Dim oPayeeCountryId, oPayeeName, oPayeeBankName, oPayeeBankAccountNo, oPayeeBankSortCode As Object
        Dim oPayeeComments, oMediaRef, vReceivableTaxPercentage, oReceivableIsTaxExempt As Object
        Dim nClaimPaymentId As Integer
        Dim oParty As Object

        Try

            nReturn = CType(GetFormData(r_vSelectedPayeeId:=oSelectedPayeeId, r_vITDomiciled:=oITDomiciled, _
                                        r_vITTaxNumber:=oITTaxNumber, r_vITPercentage:=oITPercentage, _
                                        r_vReceivableTaxPercentage:=vReceivableTaxPercentage, r_vReceivableIsTaxExempt:=oReceivableIsTaxExempt, _
                                        r_vIsSettlement:=oIsSettlement, r_vPayeeMediaTypeId:=oPayeeMediaTypeId, _
                                        r_vPayeeCountryId:=oPayeeCountryId, r_vPayeeName:=oPayeeName, _
                                        r_vPayeeBankName:=oPayeeBankName, r_vPayeeBankAccountNo:=oPayeeBankAccountNo, _
                                        r_vPayeeBankSortCode:=oPayeeBankSortCode, r_vPayeeComments:=oPayeeComments, _
                                        r_vMediaRef:=oMediaRef), PMEReturnCode)
            If nReturn <> PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetFormData Failed", PMELogLevel.PMLogError)
            End If

            If v_oInsurance.PartyCnt = 0 Then


                oParty = DBNull.Value
            Else

                oParty = v_oInsurance.PartyCnt
            End If

            Dim vClaimPaymentDetails(eClaimPayment.kLast) As Object
            vClaimPaymentDetails(eClaimPayment.kClaimid) = m_lClaimId
            vClaimPaymentDetails(eClaimPayment.kClaimPerilId) = m_lClaimPerilId
            vClaimPaymentDetails(eClaimPayment.kPaymentDate) = DateTime.Today
            vClaimPaymentDetails(eClaimPayment.kAmount) = v_oInsurance.ThisRecoverySplitAmount ' crTotalThisPaymentInPaymentCurrency
            vClaimPaymentDetails(eClaimPayment.kTaxAmount) = v_oInsurance.ThisRecoverySplitTaxAmount ' crTotalThisPaymentTaxInPaymentCurrency
            vClaimPaymentDetails(eClaimPayment.kTaxAmountWHT) = DBNull.Value ' crTotalThisPaymentTaxWHTInPaymentCurrency
            vClaimPaymentDetails(eClaimPayment.kPartyCnt) = oParty
            vClaimPaymentDetails(eClaimPayment.kComments) = DBNull.Value
            vClaimPaymentDetails(eClaimPayment.kIsReferred) = DBNull.Value
            vClaimPaymentDetails(eClaimPayment.kCreatedBy) = m_oObjectManager.UserID
            vClaimPaymentDetails(eClaimPayment.kPayeeMediaTypeId) = oPayeeMediaTypeId
            vClaimPaymentDetails(eClaimPayment.kPayeeName) = oPayeeName
            vClaimPaymentDetails(eClaimPayment.kBankName) = oPayeeBankName
            vClaimPaymentDetails(eClaimPayment.kBankSortCode) = oPayeeBankSortCode
            vClaimPaymentDetails(eClaimPayment.kBankAccountNo) = oPayeeBankAccountNo
            vClaimPaymentDetails(eClaimPayment.kPayeeCountryId) = oPayeeCountryId
            vClaimPaymentDetails(eClaimPayment.kPayeeComments) = oPayeeComments
            vClaimPaymentDetails(eClaimPayment.kSequenceNo) = DBNull.Value
            vClaimPaymentDetails(eClaimPayment.kTreatyId) = v_oInsurance.TreatyId
            vClaimPaymentDetails(eClaimPayment.kClaimPaymentTo) = oClaimPaymentToId
            vClaimPaymentDetails(eClaimPayment.kPaymentPartyTo) = oPaymentpartyTo
            vClaimPaymentDetails(eClaimPayment.kInsuredDomiciled) = oITDomiciled
            vClaimPaymentDetails(eClaimPayment.kInsuredPercentage) = oITPercentage
            vClaimPaymentDetails(eClaimPayment.kInsuredTaxNumber) = oITTaxNumber
            vClaimPaymentDetails(eClaimPayment.kPayeeDomiciled) = DBNull.Value
            vClaimPaymentDetails(eClaimPayment.kPayeePercentage) = DBNull.Value
            vClaimPaymentDetails(eClaimPayment.kPayeeTaxNumber) = DBNull.Value
            vClaimPaymentDetails(eClaimPayment.kSafeHarbourId) = DBNull.Value
            vClaimPaymentDetails(eClaimPayment.kSafeHarbourPercentage) = DBNull.Value
            vClaimPaymentDetails(eClaimPayment.kIsTaxExempt) = oIsTaxExempt
            vClaimPaymentDetails(eClaimPayment.kIsWHTExempt) = DBNull.Value
            vClaimPaymentDetails(eClaimPayment.kIsSettlement) = oIsSettlement
            vClaimPaymentDetails(eClaimPayment.kDocumentId) = DBNull.Value
            vClaimPaymentDetails(eClaimPayment.kIsLive) = DBNull.Value
            vClaimPaymentDetails(eClaimPayment.kLiveClaimPaymentId) = DBNull.Value
            vClaimPaymentDetails(eClaimPayment.kMediaRef) = oMediaRef
            vClaimPaymentDetails(eClaimPayment.kCurrencyId) = v_oReceiptItem.CurrencyId
            vClaimPaymentDetails(eClaimPayment.kExcessAmount) = DBNull.Value
            vClaimPaymentDetails(eClaimPayment.kPayeeAddress1) = ""
            vClaimPaymentDetails(eClaimPayment.kPayeeAddress2) = ""
            vClaimPaymentDetails(eClaimPayment.kPayeeAddress3) = ""
            vClaimPaymentDetails(eClaimPayment.kPayeeAddress4) = ""
            vClaimPaymentDetails(eClaimPayment.kPayeePostalCode) = ""
            vClaimPaymentDetails(eClaimPayment.kThirdPartyReference) = ""
            vClaimPaymentDetails(eClaimPayment.kChequeDate) = DBNull.Value
            vClaimPaymentDetails(eClaimPayment.kBankPaymentTypeId) = DBNull.Value
            vClaimPaymentDetails(eClaimPayment.kIsExGratia) = eClaimPayment.kWorkClaimPayment
            vClaimPaymentDetails(eClaimPayment.kBIC) = ""
            vClaimPaymentDetails(eClaimPayment.kIBAN) = ""

            nReturn = m_oBusiness.SaveClaimPayment(v_vClaimPayment:=vClaimPaymentDetails, r_lClaimPaymentId:=nClaimPaymentId)
            If nReturn <> PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SaveClaimPayment Failed", PMELogLevel.PMLogError)
            End If

            nReturn = CType(SaveClaimPaymentItems(v_oReceiptItem:=v_oReceiptItem, v_oInsurance:=v_oInsurance, v_lClaimPaymentId:=nClaimPaymentId), PMEReturnCode)
            If nReturn <> PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "bCLMPeril.SaveClaimPaymentItems Failed", PMELogLevel.PMLogError)
            End If

        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)
        Finally
        End Try
        Return nResult
    End Function

    ' ***************************************************************** '
    ' Name: SaveClaimPaymentItems
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 01-09-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function SaveClaimPaymentItems(ByVal v_oReceiptItem As cReceiptItem, ByVal v_oInsurance As cInsurance, ByVal v_lClaimPaymentId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SaveClaimPaymentItems"

        Const kClaimPaymentItemId As Integer = 0
        Const kClaimPaymentId As Integer = 1
        Const kReserveId As Integer = 2
        Const kRecoveryId As Integer = 3
        Const kRecoveryTypeId As Integer = 4
        Const kCurrencyId As Integer = 5
        Const kTaxGroupId As Integer = 6
        Const kThisPayment As Integer = 7
        Const kTaxAmount As Integer = 8
        Const kTaxAmountWHT As Integer = 9
        Const kExchangeRateOverrideReasonId As Integer = 10
        Const kCurrencyBaseXrate As Integer = 11
        Const kCurrencyBaseDate As Integer = 12
        Const kAccountBaseXRate As Integer = 13
        Const kAccountBaseDate As Integer = 14
        Const kSystemBaseXRate As Integer = 15
        Const kSystemBaseDate As Integer = 16
        Const kPaymentToLossXRate As Integer = 17
        Const kIsLive As Integer = 18
        Const kLiveClaimPaymentId As Integer = 19
        Const kLiveRecoveryId As Integer = 20
        Const kLiveReserveId As Integer = 21
        Const kLiveClaimPaymentItemId As Integer = 22
        Const kPaymentAdjustment As Integer = 23


        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lClaimPaymentItemId As Integer
        Dim oReceiptItem As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If v_oInsurance.ThisRecoverySplitAmount <> 0 Then

                ' prepare claim payment item array
                Dim vClaimPaymentItem(kPaymentAdjustment) As Object

                ' populate claim payment item array


                vClaimPaymentItem(kClaimPaymentItemId) = DBNull.Value

                vClaimPaymentItem(kClaimPaymentId) = v_lClaimPaymentId


                vClaimPaymentItem(kReserveId) = DBNull.Value

                vClaimPaymentItem(kRecoveryId) = v_oReceiptItem.RecoveryId

                vClaimPaymentItem(kRecoveryTypeId) = v_oReceiptItem.RecoveryTypeId

                vClaimPaymentItem(kCurrencyId) = v_oReceiptItem.CurrencyId
                If v_oReceiptItem.TaxGroupId = 0 Then


                    vClaimPaymentItem(kTaxGroupId) = DBNull.Value
                Else

                    vClaimPaymentItem(kTaxGroupId) = v_oReceiptItem.TaxGroupId
                End If

                vClaimPaymentItem(kThisPayment) = v_oInsurance.ThisRecoverySplitAmount

                vClaimPaymentItem(kTaxAmount) = v_oInsurance.ThisRecoverySplitTaxAmount


                vClaimPaymentItem(kTaxAmountWHT) = DBNull.Value

                vClaimPaymentItem(kExchangeRateOverrideReasonId) = v_oReceiptItem.ExchangeRateOverrideReasonId

                vClaimPaymentItem(kCurrencyBaseXrate) = v_oReceiptItem.CurrencyToBaseXRate

                vClaimPaymentItem(kCurrencyBaseDate) = v_oReceiptItem.CurrencyToBaseDate

                vClaimPaymentItem(kAccountBaseXRate) = v_oReceiptItem.AccountToBaseXRate

                vClaimPaymentItem(kAccountBaseDate) = v_oReceiptItem.AccountToBaseDate

                vClaimPaymentItem(kSystemBaseXRate) = v_oReceiptItem.SystemToBaseXRate

                vClaimPaymentItem(kSystemBaseDate) = v_oReceiptItem.SystemToBaseDate

                vClaimPaymentItem(kPaymentToLossXRate) = v_oReceiptItem.ReceiptToLossXRate


                vClaimPaymentItem(kIsLive) = DBNull.Value


                vClaimPaymentItem(kLiveClaimPaymentId) = DBNull.Value


                vClaimPaymentItem(kLiveRecoveryId) = DBNull.Value


                vClaimPaymentItem(kLiveReserveId) = DBNull.Value


                vClaimPaymentItem(kLiveClaimPaymentItemId) = DBNull.Value

                vClaimPaymentItem(kPaymentAdjustment) = 0

                ' save the claim payment item details

                lReturn = m_oBusiness.SaveClaimPaymentItem(v_vClaimPaymentItem:=vClaimPaymentItem, r_lClaimPaymentItemId:=lClaimPaymentItemId)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "SaveClaimPaymentItem Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally





        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: UpdateClaimReinsurance
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 07-10-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function UpdateClaimReinsurance() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateClaimReinsurance"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' updates claim reinsurance
            '  - claim ri arrangement
            '  - claim ri arrangement line

            lReturn = m_oBusiness.UpdateClaimReinsurance(v_lClaimPerilId:=m_lClaimPerilId)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "UpdateClaimReinsurance Failed", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function


    ' Routine Desined to find the rows in an Array
    ' that are required to be sorted
    Private Function SortRIArray2007(ByRef vArrayResult(,) As Object) As Integer

        Dim result As Integer = 0
        Dim vTemp As Object

        Const kMethodName As String = "SortRIArray2007"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For iCnt1 As Integer = 0 To vArrayResult.GetUpperBound(1) - 1
                For iCnt As Integer = 0 To vArrayResult.GetUpperBound(1) - iCnt1 - 1


                    If IsSortingRequired(CStr(vArrayResult(kInsuranceDetailType, iCnt)), CStr(vArrayResult(kInsuranceDetailType, iCnt + 1))) Then
                        For innerCnt As Integer = 0 To vArrayResult.GetUpperBound(0)


                            vTemp = vArrayResult(innerCnt, iCnt)


                            vArrayResult(innerCnt, iCnt) = vArrayResult(innerCnt, iCnt + 1)


                            vArrayResult(innerCnt, iCnt + 1) = vTemp
                        Next
                    End If
                Next
            Next
            'XOL Lines needs to be sorted
            'Re-sort the Array Based on Line-Limit
            For iCnt1 As Integer = 0 To vArrayResult.GetUpperBound(1) - 1
                For iCnt As Integer = 0 To vArrayResult.GetUpperBound(1) - iCnt1 - 1




                    If vArrayResult(11, iCnt).Equals(vArrayResult(11, iCnt + 1)) And vArrayResult(10, iCnt) < vArrayResult(10, iCnt) Then
                        For innerCnt As Integer = 0 To vArrayResult.GetUpperBound(0)


                            vTemp = vArrayResult(innerCnt, iCnt)


                            vArrayResult(innerCnt, iCnt) = vArrayResult(innerCnt, iCnt + 1)


                            vArrayResult(innerCnt, iCnt + 1) = vTemp
                        Next
                    End If
                Next
            Next


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            ' LogError _
            'v_sUsername:=m_sUsername, _
            'v_sClass:=ACClass, _
            'v_sMethod:=kMethodName, _
            'r_lFunctionReturn:=SortRIArray2007

        Finally




        End Try
        Return result
    End Function


    '*************************************************************************************
    '   Function        :        IsSortingRequired
    '   I/P             :        sRIType1,sRIType2  Denotes the RI Type
    '   Return          :        True/False
    '   Description     :        Returns True if sRIType1 priority is greater than sRIType2
    '                            Priority values are Hard Coded and calculated from index of vSortOrder.
    '   Example         :        Priority of 'T' is greater than 'F'
    '**************************************************************************************
    Private Function IsSortingRequired(ByVal sRIType1 As String, ByVal sRIType2 As String) As Boolean

        Dim iIndex1, iIndex2 As Integer

        Dim vSortOrder(4) As String
        vSortOrder(0) = "F"
        vSortOrder(1) = "FX"
        vSortOrder(2) = "TX"
        vSortOrder(3) = "T"
        vSortOrder(4) = "R"

        For iCounter As Integer = vSortOrder.GetLowerBound(0) To vSortOrder.GetUpperBound(0)
            If vSortOrder(iCounter) = sRIType1 Then
                iIndex1 = iCounter
            End If
            If vSortOrder(iCounter) = sRIType2 Then
                iIndex2 = iCounter
            End If
        Next
        Return iIndex1 > iIndex2

    End Function


    Private Function GetProductDetailsForClaim(ByRef r_bIs_Advanced_Tax_Script_Enabled As Boolean, ByRef r_bAllow_Negative_Reserve As Boolean) As Integer
        Dim result As Integer = 0
        Dim bSIRProduct As Object

        Const kMethodName As String = "GetProductDetailsForClaim"

        Dim lReturn As gPMConstants.PMEReturnCode

        Dim o_ProductBusiness As bSIRProduct.Business
        Dim vProductDetails As Object
        Dim oObjectManager As bObjectManager.ObjectManager

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            oObjectManager = New bObjectManager.ObjectManager()

            'lReturn = oObjectManager.Initialise(sCallingAppName:=ACApp)
            lReturn = oObjectManager.Initialise(sCallingAppName:="uctCLMReceiptControl")

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "oObjectManager.Initialise Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            Dim temp_o_ProductBusiness As Object
            lReturn = oObjectManager.GetInstance(temp_o_ProductBusiness, "bSIRProduct.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            o_ProductBusiness = temp_o_ProductBusiness
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetInstance of bSIRProduct.Business Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            lReturn = o_ProductBusiness.GetProductLevelOptionsForClaim(v_lClaimID:=m_lClaimId, r_bIs_Advanced_Tax_Script_Enabled:=r_bIs_Advanced_Tax_Script_Enabled, r_bAllow_Negative_Reserve:=r_bAllow_Negative_Reserve)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetProductDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally



        End Try
        Return result
    End Function

    ''Start(Saurabh Agrawal) Tech Spec QBENZCR004 Claim Recovery Reinsurance
    Private Function GetProductOptions() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetProductOptions"

        Dim lReturn As Integer
        Dim vValue As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            '*****************************************
            'developer guide no.98
            lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableRI2007, v_vBranch:=gPMConstants.SIRBCHHeadOffice, r_vUnderwriting:=vValue)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, "getProductOptionValue Failed " & _
                                        " to return value for Option:" & CStr(gPMConstants.SIRHiddenOptions.SIROPTEnableRI2007), gPMConstants.PMELogLevel.PMLogError)

            End If


            m_bRI2007Enabled = CStr(vValue) = "1"



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: GetClaimReceiptItemTaxDetails
    '
    ' Description: To Get the Clai Receipt Item Tax details
    '
    ' ***************************************************************** '
    Private Function GetClaimReceiptItemTaxDetails() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimReceiptItemTaxDetails"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lWorkClaimReceiptItemId As Integer
        Dim vTaxDetails As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue



            For Each oReceiptItem As cReceiptItem In m_colReceiptItems

                lWorkClaimReceiptItemId = oReceiptItem.WorkClaimReceiptItemId

                lReturn = m_oBusiness.GetClaimReceiptItemTaxDetails(v_lclaim_Receipt_Item_id:=lWorkClaimReceiptItemId, r_vResultArray:=vTaxDetails)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "bCLMPeril.GetClaimReceiptItemTaxDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' assign the tax details back to the payment item
                If Information.IsArray(vTaxDetails) Then


                    oReceiptItem.TaxBandRateArray = vTaxDetails
                End If

            Next oReceiptItem



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function


    Private Function GetClaimReceiptDetails() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimReceiptDetails"
        Try
            Dim lReturn As gPMConstants.PMEReturnCode
            result = gPMConstants.PMEReturnCode.PMTrue



            lReturn = m_oBusiness.GetClaimReceiptDetails(v_lClaimId:=m_lClaimId, v_lclaim_Receipt_id:=m_lClaimReceiptID, r_vResultArray:=m_vClaimReceiptDetail)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetClaimReceiptDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception
            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)


        Finally


        End Try
        Return result
    End Function
    Private Function GetClaimReceiptItemDetails() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimReceiptItemDetails"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            lReturn = m_oBusiness.GetClaimReceiptItemDetails(v_lClaimReceiptId:=m_lClaimReceiptID, r_vResults:=m_vClaimReceiptItemDetail)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetClaimReceiptItemDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not Information.IsArray(m_vClaimReceiptItemDetail) Then
                gPMFunctions.RaiseError(kMethodName, "GetClaimReceiptItemDetails Failed to return any data", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally





        End Try
        Return result
    End Function

    Private Function SetupReceiptItemDetailsListView() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupReceiptItemDetailsListView"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            lvwRecovery.Columns.Clear()

            lvwRecovery.Columns.Add("", CInt(VB6.TwipsToPixelsX(0)), HorizontalAlignment.Left)

            lvwRecovery.Columns.Insert(kRecDetItemColHIndexRecoveryId - 1, kRecDetColHCodeRecoveryId, "RecoveryId", CInt(VB6.TwipsToPixelsX(0)), HorizontalAlignment.Left, -1)

            lvwRecovery.Columns.Insert(kRecDetItemColHIndexRecoveryDesc - 1, kRecDetColHCodeRecoveryTypeDesc, "Recovery Type", CInt(VB6.TwipsToPixelsX(2000)), HorizontalAlignment.Left, -1)

            lvwRecovery.Columns.Insert(kRecDetItemColHIndexThisReceipt - 1, kRecDetColHCodeThisReceipt, GetResString(kResDetailsThisReceipt), CInt(VB6.TwipsToPixelsX(1800)), HorizontalAlignment.Right, -1)

            lvwRecovery.Columns.Insert(kRecDetItemColHIndexThisReceiptTax - 1, kRecDetColHCodeTaxAmount, GetResString(kResDetailsTaxAmount), CInt(VB6.TwipsToPixelsX(1800)), HorizontalAlignment.Right, -1)

            lvwRecovery.Columns.Insert(kRecDetItemColHIndexTotal - 1, kRecDetColHCodeRecoveredTotal, GetResString(kResDetailsTotal), CInt(VB6.TwipsToPixelsX(1800)), HorizontalAlignment.Right, -1)



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function
    Private Function PopulateReceiptItemDetailsListView() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulateReceiptItemDetailsListView"

        Dim lReturn, lType, lTypeId, llBound, lUBound As Integer
        Dim oListItem As ListViewItem
        Dim lRecoveryId, lClaimPerilId, lRecoveryTypeId As Integer
        Dim sRecoveryTypeDescription As String = ""
        Dim crThisReceipt As Decimal
        Dim lCurrencyId As Integer
        Dim sCurrencyDesc As String = ""
        Dim crReceivedToDate As Decimal
        Dim lRevisionCount As Integer
        Dim crTaxAmount As Decimal
        Dim lClaimID, lClaimsIsPostTaxes As Integer
        Dim crTotalReceipt As Decimal
        Dim lTaxGroupId As Integer
        Dim dCurrencyBaseXRate As Decimal
        Dim dReceiptToLossXRate As Double
        Dim sTaxGroupDescription As String = ""
        Dim bIsWithHoldingTax As Boolean
        Dim sAdvancedTaxScript As String = ""
        Dim lWorkClaimReceiptItemId As Integer
        Dim crTotal As Decimal
        Dim oReceiptItem As cReceiptItem
        Dim sResult As String = ""
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lvwRecovery.Items.Clear()

            ' determine array boundaries
            llBound = m_vClaimReceiptItemDetail.GetLowerBound(1)
            lUBound = m_vClaimReceiptItemDetail.GetUpperBound(1)


            ' for each reserve / recovery type
            For lItem As Integer = llBound To lUBound

                '' get recovery details
                lRecoveryId = gPMFunctions.ToSafeLong(m_vClaimReceiptItemDetail(kClaimRecItemDetRecoveryId, lItem), 0)
                sRecoveryTypeDescription = gPMFunctions.ToSafeString(m_vClaimReceiptItemDetail(kClaimRecItemDetRecoveryTypeDesc, lItem), CStr(0))
                crThisReceipt = gPMFunctions.ToSafeCurrency(m_vClaimReceiptItemDetail(kClaimRecItemDetThisReceipt, lItem), 0)
                crTaxAmount = gPMFunctions.ToSafeCurrency(m_vClaimReceiptItemDetail(kClaimRecItemDetTaxAmount, lItem), 0)
                lCurrencyId = gPMFunctions.ToSafeLong(m_vClaimReceiptItemDetail(kClaimRecItemDetCurrencyId, lItem), 0)
                dCurrencyBaseXRate = gPMFunctions.ToSafeCurrency(m_vClaimReceiptItemDetail(kClaimRecItemDetCurrencyBaseXRate, lItem), 0)
                lTaxGroupId = gPMFunctions.ToSafeLong(m_vClaimReceiptItemDetail(kClaimRecItemDetTaxGroupId, lItem), 0)
                crTotalReceipt = gPMFunctions.ToSafeCurrency(m_vClaimReceiptItemDetail(kClaimRecItemDetTotalRecovery, lItem), 0)
                crReceivedToDate = gPMFunctions.ToSafeCurrency(m_vClaimReceiptItemDetail(kClaimRecItemDetReceivedToDate, lItem), 0)
                dReceiptToLossXRate = gPMFunctions.ToSafeLong(m_vClaimReceiptItemDetail(kClaimRecItemDetReceiptToLossXRate, lItem), 0)
                sCurrencyDesc = gPMFunctions.ToSafeString(m_vClaimReceiptItemDetail(kClaimRecItemDetCurrencyDescription, lItem), "")
                sTaxGroupDescription = gPMFunctions.ToSafeString(m_vClaimReceiptItemDetail(kClaimRecItemDetTaxGroupDescription, lItem), "")
                bIsWithHoldingTax = gPMFunctions.ToSafeBoolean(m_vClaimReceiptItemDetail(kClaimRecItemDetIsWithHoldingTax, lItem), False)
                sAdvancedTaxScript = CStr(m_vClaimReceiptItemDetail(kClaimRecItemDetAdvancedTaxScript, lItem)).Trim()
                lWorkClaimReceiptItemId = gPMFunctions.ToSafeLong(m_vClaimReceiptItemDetail(kClaimRecItemDetWorkClaimReceiptItemId, lItem))

                oListItem = lvwRecovery.Items.Add(CStr(lRecoveryId))

                ' populate list sub item details
                ListViewHelper.GetListViewSubItem(oListItem, kRecDetItemColHIndexRecoveryId - 1).Text = CStr(lRecoveryId)
                ListViewHelper.GetListViewSubItem(oListItem, kRecDetItemColHIndexRecoveryDesc - 1).Text = sRecoveryTypeDescription
                ListViewHelper.GetListViewSubItem(oListItem, kRecDetItemColHIndexThisReceipt - 1).Text = StringsHelper.Format(crThisReceipt * dReceiptToLossXRate, "0.00")
                ListViewHelper.GetListViewSubItem(oListItem, kRecDetItemColHIndexThisReceiptTax - 1).Text = StringsHelper.Format((crTaxAmount) * dReceiptToLossXRate, "0.00")

                ''Saurabh
                'PN-61621(Sushil Kumar)
                If m_bReceiptExcludeTax Then
                    crTotal = (crThisReceipt * dReceiptToLossXRate) + (crTaxAmount * dReceiptToLossXRate)
                Else
                    crTotal = (crThisReceipt * dReceiptToLossXRate) - (crTaxAmount * dReceiptToLossXRate)
                End If

                ListViewHelper.GetListViewSubItem(oListItem, kRecDetItemColHIndexTotal - 1).Text = StringsHelper.Format(crTotal, "0.00")

                m_crTotalThisReceiptGross += crTotal

                '         m_crTotalTax = m_crTotalTax + crTaxAmount
                '         m_crNetAmount = m_crNetAmount + crThisReceipt
                oReceiptItem = New cReceiptItem()

                oReceiptItem.CurrencyId = lCurrencyId
                oReceiptItem.RecoveryTypeDesc = sRecoveryTypeDescription
                oReceiptItem.AdvancedTaxScript = sAdvancedTaxScript
                oReceiptItem.CurrencyDescription = sCurrencyDesc
                oReceiptItem.ThisReceipt = crThisReceipt
                oReceiptItem.TaxAmount = crTaxAmount
                oReceiptItem.ReceivedToDate = crReceivedToDate
                oReceiptItem.RecoveryId = lRecoveryId
                oReceiptItem.ReceiptToLossXRate = dReceiptToLossXRate

                oReceiptItem.WorkClaimReceiptItemId = lWorkClaimReceiptItemId

                If m_colReceiptItems Is Nothing Then
                    m_colReceiptItems = New Collection()
                End If

                m_colReceiptItems.Add(oReceiptItem, CStr(lRecoveryId))

            Next

            ' populate total line

            If lvwRecovery.Items.Count > 0 Then
                lvwRecovery.Items(0).Selected = True
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=PopulateReceiptDetailsListView(), excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function
    Private Function PopulateClaimReceiptDetails() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "PopulateClaimReceiptDetails"

        Dim lReturn, lClaimID As Integer
        Dim dtLossFromDate As Date
        Dim lWorkClaimReceiptId, lClaimPerilId As Integer
        Dim dtDateOfPayment As Date
        Dim crAmount, crTaxAmount As Decimal
        Dim lPartyCnt As Integer
        Dim sComments As String = ""
        Dim lCreatedById, lPayeeMediaType As Integer
        Dim sPayeeName, sPayeeBankName, sPayeeSortCode, sPayeeAccountNo, sPayeeCountry, sPayeeComments As String
        Dim bInsuredDomiciled As Boolean
        Dim crInsuredPercentage As Decimal
        Dim sInsuredTaxNumber As String = ""
        Dim bPayeeDomiciled As Boolean
        Dim crPayeePercentage As Decimal
        Dim sPayeeTaxNumber As String = ""
        Dim bIsTaxExempt, bIsSettlement As Boolean
        Dim lDocumentId As Integer


        Const kThisReceipt As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Information.IsNothing(m_vClaimReceiptDetail) Then
                ' fraPayee
                m_lClaimPerilId = gPMFunctions.ToSafeLong(m_vClaimReceiptDetail(kClaimPerilId, kThisReceipt), 0)


                SelectcboItem(cboMediaType, gPMFunctions.ToSafeLong(m_vClaimReceiptDetail(kPayeeMediaType, 0)))
                '        SelectcboItem cboCountry, ToSafeLong(m_vClaimPaymentDetails(kPayeeCountry, 0))

                txtMediaType.Text = CStr(m_vClaimReceiptDetail(kMediaTypeDesc, kThisReceipt))


                m_lClaimReceiptID = CInt(m_vClaimReceiptDetail(kWorkClaimReceiptId, kThisReceipt))


                ' fraInsuredTaxAdjustment
                chkITDomiciled.CheckState = gPMFunctions.ToSafeLong(m_vClaimReceiptDetail(kRecInsuredDomiciled, kThisReceipt))
                txtITPercentage.Text = StringsHelper.Format(gPMFunctions.ToSafeCurrency(m_vClaimReceiptDetail(kRecInsuredPercentage, kThisReceipt), 0), "0.00")
                txtITTaxNo.Text = CStr(m_vClaimReceiptDetail(kRecInsuredTaxNumber, kThisReceipt))


                ' fraSafeHarbour


                ' fraExemptions
                chkTaxExempt.CheckState = gPMFunctions.ToSafeLong(m_vClaimReceiptDetail(kRecIsTaxExempt, kThisReceipt), 0)

                ' fraSettlement
                chkSettlement.CheckState = gPMFunctions.ToSafeLong(m_vClaimReceiptDetail(kRecIsSettlement, kThisReceipt), 0)


                txtPayeeName.Text = CStr(m_vClaimReceiptDetail(kPayeeName, kThisReceipt))
                txtMediaRef.Text = CStr(m_vClaimReceiptDetail(kPayeeMediaRef, kThisReceipt))
                txtBankAccountNo.Text = CStr(m_vClaimReceiptDetail(kPayeeAccountNo, kThisReceipt))
                txtBankCode.Text = CStr(m_vClaimReceiptDetail(kPayeeSortCode, kThisReceipt))
                txtBankName.Text = CStr(m_vClaimReceiptDetail(kPayeeBankName, kThisReceipt))
                txtPayeeComments.Text = CStr(m_vClaimReceiptDetail(kComments, kThisReceipt))

            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        End Try
        Return result
    End Function

    ''End(Saurabh Agrawal) Tech Spec QBENZCR004 Claim Recovery Reinsurance
    ''' <summary>
    ''' To Validate the reciept
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ValidateReciept() As Integer

        Dim nResult As Integer
        Dim oDefaultBankAccount(,) As Object

        nResult = PMEReturnCode.PMTrue
        ' WPR085 media type is mandatory when automatic receipting is enabled.
        If m_crTotalThisReceiptGross <> 0 Then

            If m_bAutomateReceiptGeneration = True Then
                If cboMediaType.SelectedIndex = -1 Then
                    MsgBox("Please select a Media Type for automatic receipt generation.", vbExclamation, "Media Type Validation")
                    nResult = PMEReturnCode.PMFalse
                    cboMediaType.Focus()
                    Return nResult
                End If

                oDefaultBankAccount = Nothing
                ' WPR085 Get default bank account and cash list item receipt type.
                ' First, try to filter by product id - if that is not successful, ignore product.
                nResult = m_oBusiness.GetDefaultBankAccount(v_nSourceID:=m_lClaimSourceId, v_nMediaType:=m_vMediaType(kLookupItemId, cboMediaType.SelectedIndex), v_nProductId:=m_lProductId, r_oResults:=oDefaultBankAccount)

                If nResult <> PMEReturnCode.PMTrue Or Not IsArray(oDefaultBankAccount) Then
                    nResult = m_oBusiness.GetDefaultBankAccount(v_nSourceID:=m_lClaimSourceId, v_nMediaType:=m_vMediaType(kLookupItemId, cboMediaType.SelectedIndex), v_nProductId:=0, r_oResults:=oDefaultBankAccount)

                    If nResult <> PMEReturnCode.PMTrue Or Not IsArray(oDefaultBankAccount) Then
                        MsgBox("No default bank account exists for this Branch / Media Type /Product combination.Please contact your system administrator.", vbExclamation, "Default Bank Account")
                        nResult = PMEReturnCode.PMFalse
                        Return nResult
                    End If
                End If

                Dim nCurrencyMatches As Integer

                For i As Integer = 0 To UBound(oDefaultBankAccount, 2)
                    Dim oCurrencyId As Object
                    oCurrencyId = Nothing

                    nResult = m_oBusiness.GetCurrencyFromBankAccount(CInt(oDefaultBankAccount(0, i)), oCurrencyId)
                    If nResult <> PMEReturnCode.PMTrue Then
                        RaiseError("GetCurrencyFromBankAccount", "GetClaimReceiptItemDetails Failed", PMELogLevel.PMLogError)
                        Return nResult
                    End If

                    If oCurrencyId(0, 0) = m_lLossCurrencyId Then
                        nCurrencyMatches = nCurrencyMatches + 1
                    End If
                Next i

                If nCurrencyMatches > 1 Then
                    MsgBox("Configuration problem - there is more than 1 default Bank Accounts that can be used for this MediaType / Branch / Claim Receipt Type / Currency combination")
                    nResult = PMEReturnCode.PMFalse
                    Return nResult
                End If

                If nCurrencyMatches = 0 Then
                    MsgBox("There are no default Bank Accounts for this currency. Please contact your system administrator.", vbExclamation, "Default Bank Account")
                    nResult = PMEReturnCode.PMFalse
                    Return nResult
                End If

                nResult = m_oBusiness.GetDefaultCashListItemReceiptType(r_oResults:=m_oCashListItemReceiptType)

                If (IsArray(m_oCashListItemReceiptType)) Then
                    If String.IsNullOrEmpty(m_oCashListItemReceiptType(0, 0)) Then
                        MsgBox("Configuration problem - there is no default Claim Receipt Type setup for the System, Please contact your system administrator", vbExclamation, "Default CashList Item Receipt Type")
                        nResult = PMEReturnCode.PMFalse
                        Return nResult
                    End If
                ElseIf (nResult <> PMEReturnCode.PMTrue) Or (Not IsArray(m_oCashListItemReceiptType)) Then
                    MsgBox("Configuration problem - there is no default Claim Receipt Type setup for the System, Please contact your system administrator", vbExclamation, "Default CashList Item Receipt Type")
                    nResult = PMEReturnCode.PMFalse
                    Return nResult
                End If

                Dim oResult(,) As Object
                Dim dRecoveredTotal As Double
                Dim dThisReceipt As Double

                dRecoveredTotal = ToSafeDouble(lvwRecovery.Items(1).SubItems(kRecDetSubItemsRecoveredTotal), 0)
                dThisReceipt = ToSafeDouble(lvwRecovery.Items(1).SubItems(kRecDetSubItemsThisNet), 0)

                nResult = m_oBusiness.GetClaimPaymentTotal(r_oResults:=oResult, nClaimId:=ClaimID)

                If (nResult <> PMEReturnCode.PMTrue) Then
                    RaiseError("ValidateReciept", "GetClaimPaymentTotal", PMELogLevel.PMLogError)
                ElseIf (dThisReceipt > ToSafeDouble(oResult(0, 0))) Then
                    MessageBox.Show("Recovery cannot be more than the total Reserve.", "Recovery amount", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return PMEReturnCode.PMFalse
                End If



            End If
        End If

        Return nResult

    End Function

End Class
