Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Windows.Forms
Imports SharedFiles

<System.Runtime.InteropServices.ProgId("uctCLMPayment1_NET.uctCLMPayment1")>
Partial Public Class uctCLMPayment1
    Inherits System.Windows.Forms.UserControl
    Public Event ShowCoInsurersChange()

    Private Const ACOptionNumberAuthoriseClaimPayment As Integer = 2020
    Private Const ACClass As String = "uctCLMPayment1"

    Public Event UnRecoverableError(ByVal Sender As Object, ByVal e As EventArgs)

    'Party Bank Details
    Private Enum ENPartyBank
        RowStatus = 0
        RowIndex = 1
        PartyBankId = 2
        IsBank = 3
        AccountID = 4
        BankPaymentTypeId = 5
        BankAccountTypeId = 6
        AccountHolderName = 7
        AccountNumber = 8
        BankNameId = 9
        BankBranch = 10
        BankBranchCode = 11
        BankAdd1 = 12
        BankAdd2 = 13
        BankAdd3 = 15
        BankTown = 17
        BankPCode = 16
        BankRegion = 14
        BankCountry = 18
        CCNum = 19
        CCstartdate = 20
        CCexpirydate = 21
        CCIssueNum = 22
        CCpin1 = 23
        IsRegistered = 24
        CCAdd1 = 25
        CCAdd2 = 26
        CCAdd3 = 27
        CCTown = 28
        CCPCode = 29
        CCCountry = 30
        IsDeleted = 31
        CCNameOnCard = 32
        CCManualAuthorisationNum = 33
        PFLINKEXISTS = 34
        CLILINKEXISTS = 35
        CPLINKEXISTS = 36
        BIC = 37
        IBAN = 38
    End Enum

    Private Enum ENPMLookups
        Id = 0
        Description = 1
    End Enum

    ' objects
    Private m_oObjectManager As bObjectManager.ObjectManager

    Private m_oCurrencyConvert As Object

    Private m_oPaymentMethod As Object
    Private m_oBusiness As Object
    Private m_oWrkTaskInstanceTemp As Object 'eck 11/2005


    Private m_oSirMediaTypeValidation As Object

    ' generic interface details
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_iLanguageID As Integer
    Private m_iSourceID As Integer
    Private m_iUserId As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_dtEffectiveDate As Date
    Private m_sTransactionType As String = ""

    ' custom interface details
    Private m_bViewPaymentMode As Boolean
    Private m_iComponentMode As Integer
    Private m_lPrevClaimPayeeOption As Integer
    Private m_bUnrecoverableError As Boolean

    ' system options
    Private m_bAdvancedTaxScriptingOption As Boolean
    Private m_lDefaultClaimPaymentOption As Integer
    Private m_lClaimPaymentAuthorityOption As Integer
    Private m_bAllowNegativeReserve As Boolean
    Private m_bClaimPaymentIsGross As Boolean
    Private m_iQASType As Integer
    Private m_bATSSattlement As Boolean
    Private m_bPaymentATSSafeHarbour As Boolean

    ' product options
    'Private m_bPaymentAuthorityScriptsOn        As Boolean
    Private m_bClaimHasReferredPayments As Boolean

    ' collection
    Private m_colPaymentItems As Collection

    ' array details
    Private m_vClaimPaymentItemDetails(,) As Object
    Private m_vClaimPaymentDetails(,) As Object
    Private m_vReserveAndPaymentDetails(,) As Object

    ' claim details
    Private m_vClaimDetails(,) As Object
    Private m_lClaimId As Integer
    Private m_lClaimPerilId As Integer
    Private m_lWorkClaimID As Integer
    Private m_lWorkClaimPaymentId As Integer
    Private m_lWorkClaimPerilId As Integer
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
    Private m_lRiskID As Integer

    'DC090606 Add Coinsurer Details for Datasure
    Private m_bShowCoinsurers As Boolean
    Private m_vCoinsurerSplit As Object

    ' payment details
    Private m_lPaymentPartyTo As Integer
    Private m_sRisktype As String = ""
    Private m_vClaimPaymentTo(,) As Object
    Private m_sClaimPaymentToCode As Object
    Private m_vSafeHarbour(,) As Object
    Private m_sSafeHarbourCode As Object
    Private m_vMediaType(,) As Object
    Private m_vCountry As Object
    Private m_crTotalThisPaymentInclTax As Decimal
    Private m_crTotalThisPaymentInclTaxABS As Decimal 'PN 45182
    Private m_crTotalThisPayment As Decimal
    Private m_crTotalAmount As Decimal

    ' lookup details
    Private m_vCurrencyArray As Object
    Private m_vTaxGroupNotWithholdingTax(,) As Object
    Private m_vTaxGroupIsWithholdingTax(,) As Object
    Private m_vTaxGroupTaxBandDetails As Object
    Private m_vTaxBand As Object
    Private m_vTaxGroup As Object
    Private m_vClassOfBusiness As Object

    ' payment item details
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
    Private m_lClaimPayableAccountId As Integer
    Private m_lClaimPayableCurrencyId As Integer

    ' other party details
    Private m_lOtherPartyCnt As Integer
    Private m_lOtherPartyCurrencyId As Integer
    Private m_bOtherPartyDomiciledForTax As Boolean
    Private m_sOtherPartyTaxNumber As String = ""
    Private m_dOtherPartyTaxPercentage As Double

    ' client details
    Private m_lClientId As Integer
    Private m_sClientName As String = ""
    Private m_lClientCurrencyId As Integer
    Private m_bClientDomiciledForTax As Boolean
    Private m_sClientTaxNumber As String = ""
    Private m_crClientTaxPercentage As Decimal
    Private m_bClientTaxExempt As Boolean

    ' product details
    Private m_bProductPreventPaymentsToCancelledAgents As Boolean
    Private m_bProductMediaTypeMandatory As Boolean

    ' lead agent details
    Private m_lLeadAgentId As Integer
    Private m_sLeadAgentName As String = ""
    Private m_lLeadAgentCurrencyId As Integer
    Private m_bLeadAgentDomiciledForTax As Boolean
    Private m_sLeadAgentTaxNumber As String = ""
    Private m_crLeadAgentTaxPercentage As Decimal
    Private m_bLeadAgentTaxExempt As Boolean
    Private m_bLeadAgentDateCancelled As Boolean

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
    Private m_bTransferAgentDateCancelled As Boolean

    Private m_crExcessAmount As Decimal
    Private m_bExcessSupplied As Boolean
    Private m_bExcessDefaulted As Boolean
    Private m_bExcessRowExists As Boolean
    Private m_bExcessWarningDisplayed As Boolean
    Private m_bTaxWarningShown As Boolean

    Private m_lReturn As Integer
    Private m_oFormFields As iPMFormControl.FormFields
    Private m_bPaymentLock As Boolean

    '(RC) QBENZ001
    Private m_bCanChangeReserves As Boolean
    Private m_crTotalThisReserve As Decimal
    Private m_bOpenClaimNoTrans As Boolean
    Private m_sUnderwritingOrBroking As String = ""
    Private m_bRI2007Enabled As Boolean

    Private m_iNoofReferredPayments As Integer
    Private m_bMultipleClaimPayments As Boolean
    Private m_iMaxNoofUnAuthorisedClaimPayments As Integer
    Private m_cMaxUnAuthorisedClaimPaymentValue As Decimal
    Private m_cUnAuthorisedClaimPayment As Decimal
    Private m_bRunAuthorisationScriptsForClaimPayments As Boolean
    Private m_bCanDoClaimPayment As Boolean
    Private m_bViewThisPayment As Boolean

    'Party Bank Details
    Private m_vPartyBankDetails(,) As Object
    Private m_lAccountID As Integer
    Private m_vBankPaymentTypeId As Object


    Private m_oFindParty As Object
    Public Event DataHasChanged(ByVal Sender As Object, ByVal e As DataHasChangedEventArgs)
    Private m_bShowPaymentDetailsHiddenMode As Boolean
    Private m_bFastTrackEnabled As Boolean
    Private m_bCash_Payment_Process As Boolean
    Private m_bPaymentCannotExceedReserve As Boolean

    Private m_bClaimReserveIsGross as Boolean 
    Private m_oBusinessParty As Object
    Private m_lPartyBankCount As Long
    ' PN 76954
    Private m_crTotalCostToClaim As Decimal
    Private m_crTotalPaidToDate As Decimal
    Private m_crTotalReceivedToDate As Decimal
    Private m_bClaimHasXOLLines As Boolean 'WPR022
    Private sExGratiaAccount As String = ""

    Private m_bIsPaymentsReadOnly As Boolean?
    Private m_bIsThisPaymentMade As Boolean
    ''' <summary>
    ''' Holds the flag for IsPaymentsReadOnly configured in product maintenanc.
    ''' </summary>
    ''' <returns></returns>
    Private ReadOnly Property IsPaymentsReadOnly() As Boolean
        Get
            If Not m_bIsPaymentsReadOnly.HasValue Then
                m_bIsPaymentsReadOnly = GetIsPaymentsReadOnly()
            End If
            Return m_bIsPaymentsReadOnly
        End Get
    End Property
    ''' <summary>
    ''' Holds the flag for Paymentsmode configured in product maintenanc.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property IsThisPaymentMade() As Boolean
        Get
            Return IIf(SSTab1.TabCount > 1, True, False)
        End Get
    End Property

    <Browsable(False)>
    Public WriteOnly Property ViewPaymentMode() As Boolean
        Set(ByVal Value As Boolean)
            m_bViewPaymentMode = Value
        End Set
    End Property


    <Browsable(True)>
    Public Property WorkClaimID() As Integer
        Get
            Return m_lWorkClaimID
        End Get
        Set(ByVal Value As Integer)
            m_lWorkClaimID = Value
        End Set
    End Property


    <Browsable(True)>
    Public Property WorkClaimPerilId() As Integer
        Get
            Return m_lWorkClaimPerilId
        End Get
        Set(ByVal Value As Integer)
            m_lWorkClaimPerilId = Value
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property WorkClaimPaymentId() As Integer
        Set(ByVal Value As Integer)
            m_lWorkClaimPaymentId = Value
        End Set
    End Property


    <Browsable(True)>
    Public Property RI2007Enabled() As Boolean
        Get
            Return m_bRI2007Enabled
        End Get
        Set(ByVal Value As Boolean)
            m_bRI2007Enabled = Value
        End Set
    End Property


    'DC090606 Add Coinsurer Details for Datasure
    <Browsable(True)>
    Public Property ShowCoInsurers() As Boolean
        Get
            Return m_bShowCoinsurers
        End Get
        Set(ByVal Value As Boolean)

            m_bShowCoinsurers = Value

            ' show/hide the CoInsurers tab
            SSTabHelper.SetTabVisible(SSTab1, 2, Value)

            RaiseEvent ShowCoInsurersChange()

        End Set
    End Property

    <Browsable(True)>
    Public Property IsOpenClaimNoTrans() As Boolean
        Get
            Return m_bOpenClaimNoTrans
        End Get
        Set(ByVal Value As Boolean)
            m_bOpenClaimNoTrans = Value
        End Set
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

        Dim result As Integer = 0
        Const kMethodName As String = "Load"

        Const AC_MEDIA_TYPE_CHEQUE As String = "CQ"

        Dim lMediaTypeId As Integer
        Dim lReturn As gPMConstants.PMEReturnCode


        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' get required system options
            lReturn = GetSystemOptions()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetSystemOptions Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get required product options
            lReturn = GetProductOptions()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetProductOptions Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get claim specific details
            lReturn = GetClaimDetails()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetClaimDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' populate user control with claim details
            lReturn = PopulateClaimDetails()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "PopulateClaimDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' NB: This actually creates the reserve entries if they are not already there...
            lReturn = CreateReserveDetails()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "CreateReserveDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            lReturn = GetProductDetails()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetProductDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' set up taxes list view
            lReturn = SetupTaxesListView()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetupTaxesListView Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            'DC090606 Add Coinsurer Details for Datasure
            SSTabHelper.SetTabVisible(SSTab1, 2, False)

            dtpChequeDate.Value = DateTime.Now

            'If m_bViewPaymentMode Then
            ' get claim payment details
            ' First Check, is there any payment? only in that case populate this payment details
            lReturn = GetClaimPaymentDetails()
            m_bViewPaymentMode = Not (lReturn <> gPMConstants.PMEReturnCode.PMTrue)
            'RaiseError kMethodName, "GetClaimPaymentDetails Failed", PMLogError
            'End If
            If IsArray(m_vClaimPaymentDetails) Then
                m_lWorkClaimPaymentId = ToSafeLong(m_vClaimPaymentDetails(kClaimPaytDetWorkClaimPaymentId, 0))
            Else
                m_bViewPaymentMode = False
            End If

            uctPMAddressControl1.Size = New System.Drawing.Size(520, 152)
            uctPMAddressControl1.IsPostCodeRequired = uctPMAddressControl1.IsPostCodeRequired
            If Not m_bViewPaymentMode Then

                ' only get lookups when not in view only mode
                ' as values provided by lookups will be retrieved from db
                ' along with general call for data
                ' get lookup details...
                lReturn = GetLookups()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetLookups Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' populate lookup combos
                lReturn = PopulateLookups()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "PopulateLookups Failed", gPMConstants.PMELogLevel.PMLogError)
                End If


                ' get latest version of all claim reserve details.
                lReturn = GetCurrentPaymentDetails()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetCurrentClaimPaymentReserveDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' setup payment details list view
                lReturn = SetupPaymentDetailsListView()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "SetupPaymentDetailsListView Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' populate payment details list view
                lReturn = PopulatePaymentDetailsListView()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "PopulateLookups Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            Else


                ' get claim payment details
                lReturn = GetClaimPaymentDetails()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "GetClaimPaymentDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                End If


                ' populate user control with claim payment details
                lReturn = PopulateClaimPaymentDetails()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "PopulateClaimPaymentDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
                '
                ' if in view only mode get the saved payment item details for this payment
                lReturn = GetClaimPaymentItemDetails()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "GetClaimPaymentItemDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                If IsArray(m_vClaimPaymentItemDetails) Then
                    ' setup payment details list view
                    lReturn = SetupPaymentItemDetailsListView()
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "SetupPaymentDetailsListView Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    ' populate payment details list view
                    lReturn = PopulatePaymentItemDetailsListView()
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "PopulateLookups Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    ' get claim payment item tax
                    lReturn = GetClaimPaymentItemTaxDetails()
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "GetClaimPaymentItemTaxDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    ' populate this payment details
                    lReturn = PopulateThisPaymentDetails()
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "PopulateThisPaymentDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                Else
                    ' get latest version of all claim reserve details.
                    lReturn = GetCurrentPaymentDetails()
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "GetCurrentClaimPaymentReserveDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    ' setup payment details list view
                    lReturn = SetupPaymentDetailsListView()
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "SetupPaymentDetailsListView Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    ' populate payment details list view
                    lReturn = PopulatePaymentDetailsListView()
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "PopulateLookups Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                    m_bViewPaymentMode = False
                End If
            End If

            lReturn = SetFieldValidation()

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            End If



            'WR5
            lReturn = GetClaimWorkFlow()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetClaimWorkFlow Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            'PN45918 (RC)

            lReturn = m_oBusiness.GetUserCanChangeReserves(m_iUserId, m_bCanChangeReserves)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Load() - GetUserCanChangeReserves Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' setup user control
            lReturn = SetUpUserControl()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetClaimPaymentDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not IsArray(m_vClaimPaymentItemDetails) Then
                SSTabHelper.SetTabVisible(SSTab1, 1, False)
            End If

            ValidatePaymentReadOnly()

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
    'Start - (Sankar) - (Tech Spec - QBENZCR007 - Authorise Claim payments.doc)
    Public Function FocusTab(ByVal iIndex As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "FocusTab"

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            If SSTabHelper.GetTabVisible(SSTab1, iIndex) Then
                SSTabHelper.SetSelectedIndex(SSTab1, iIndex)
                result = gPMConstants.PMEReturnCode.PMTrue
            End If


        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=Initialise(), excep:=ex)
        Finally


        End Try
        Return result
    End Function
    'End - (Sankar) - (Tech Spec - QBENZCR007 - Authorise Claim payments.doc)

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

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            Static bIsInitialised As Boolean

            ' Check if already initialised
            If bIsInitialised Then
                Return result
            End If

            m_colPaymentItems = New Collection()

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
                gPMFunctions.RaiseError(kMethodName, "GetInstance of bSIRPartyFee.Business Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' retrieve the agency or broking indicator

            m_sUnderwritingOrBroking = m_oBusiness.UnderwritingOrAgency

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

            'Get the instance of bSirMediaTypeValidation
            Dim temp_m_oSirMediaTypeValidation As Object
            lReturn = m_oObjectManager.GetInstance(temp_m_oSirMediaTypeValidation, "bSIRMediaTypeValidation.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oSirMediaTypeValidation = temp_m_oSirMediaTypeValidation
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetInstance of bSirMediaTypeValidation.business Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            ' Create an instance of the form control object.
            m_oFormFields = New iPMFormControl.FormFields()

            ' Set language
            m_oFormFields.LanguageID = g_iLanguageID


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

    ' ***************************************************************** '
    ' Name: GetClaimPaymentToDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 26-07-2005 : 360 - Taxes On Claims
    ' ***************************************************************** '
    Private Function GetClaimPaymentToDetails() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimPaymentToDetails"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            lReturn = m_oBusiness.GetClaimPaymentToDetails(r_vResults:=m_vClaimPaymentTo)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetClaimPaymentToDetails", gPMConstants.PMELogLevel.PMLogError)
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
    Private Function GetLookups() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetLookups"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get class of business
            lReturn = CType(GetLookupsByEffectiveDate(v_sTableName:=kLookupTableNameClassOfBusiness, r_vResults:=m_vClassOfBusiness), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetLookupsByEffectiveDate Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            ' get tax group
            lReturn = CType(GetLookupsByEffectiveDate(v_sTableName:=kLookupTableNameTaxGroup, r_vResults:=m_vTaxGroup), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetLookupsByEffectiveDate Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get tax band
            lReturn = CType(GetLookupsByEffectiveDate(v_sTableName:=kLookupTableNameTaxBand, r_vResults:=m_vTaxBand), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetLookupsByEffectiveDate Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            '    ' get media types
            '    lReturn = GetLookupsByEffectiveDate(v_sTableName:=kLookupTableNameMediaType, r_vResults:=m_vMediaType)
            '    If lReturn <> PMTRue Then
            '        RaiseError kMethodName, "GetLookupsByEffectiveDate Failed", PMLogError
            '    End If

            ' get countries
            lReturn = CType(GetLookupsByEffectiveDate(v_sTableName:=kLookupTableNameCountry, r_vResults:=m_vCountry), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetLookupsByEffectiveDate Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get currency
            lReturn = CType(GetClaimBranchCurrencies(v_lSourceId:=m_lClaimSourceId, r_vResults:=m_vCurrencyArray), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetLookupsByEffectiveDate Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get tax group with withholding tax
            lReturn = CType(GetTaxGroupDetails(v_vIsWithHoldingTax:=1, r_vArray:=m_vTaxGroupIsWithholdingTax), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetTaxGroupDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get tax group details with out withholding tax
            lReturn = CType(GetTaxGroupDetails(v_vIsWithHoldingTax:=0, r_vArray:=m_vTaxGroupNotWithholdingTax), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetTaxGroupDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get safe harbour
            lReturn = CType(GetSafeHarbourDetails(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetTaxGroupDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get claim payment to
            lReturn = CType(GetClaimPaymentToDetails(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetClaimPaymentToDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get tax group tax bands
            lReturn = CType(GetTaxGroupTaxBandDetails(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetTaxGroupTaxBandDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get media types
            lReturn = CType(GetMediaType(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetMediaType Failed", gPMConstants.PMELogLevel.PMLogError)
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
    ' Name: GetClaimPaymentDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 26-07-2005 : 360 - Taxes On Claims
    ' ***************************************************************** '
    Private Function GetClaimPaymentDetails() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimPaymentDetails"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            lReturn = m_oBusiness.GetClaimPaymentDetails(v_lClaimId:=m_lWorkClaimID, v_lClaimPaymentId:=m_lWorkClaimPaymentId, r_vResults:=m_vClaimPaymentDetails)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetClaimPaymentDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not Information.IsArray(m_vClaimPaymentDetails) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                'RaiseError kMethodName, "GetClaimPaymentDetails Failed to return any date", PMLogError
            Else
                m_lWorkClaimPaymentId = m_vClaimPaymentDetails(kClaimPaytDetWorkClaimPaymentId, 0)
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
    ' Name: GetCurrentPaymentDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 09-08-2005 : 360 - Taxes On Claims
    ' ***************************************************************** '
    Private Function GetCurrentPaymentDetails() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetCurrentPaymentDetails"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            lReturn = m_oBusiness.GetCurrentClaimPaymentReserveDetails(v_lClaimPerilId:=m_lWorkClaimPerilId, r_vResults:=m_vReserveAndPaymentDetails)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetCurrentPaymentDetails Failed", gPMConstants.PMELogLevel.PMLogError)
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
        Dim dtDate As Date

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If Information.IsArray(m_vClaimDetails) Then
                ' product details
                m_bProductPreventPaymentsToCancelledAgents = gPMFunctions.ToSafeBoolean(m_vClaimDetails(kClaimDetailPreventCancelledAgents, 0), False)
                m_bProductMediaTypeMandatory = gPMFunctions.ToSafeBoolean(m_vClaimDetails(kClaimDetailProductMediaTypeMandatory, 0), False)

                ' claim details
                m_lRiskID = CInt(m_vClaimDetails(kClaimDetailRiskId, 0))
                m_sRisktype = CStr(m_vClaimDetails(kClaimDetailRiskTypeDesc, 0))
                m_sLossCurrency = CStr(m_vClaimDetails(kClaimDetailLossCurrencyDesc, 0))
                m_dtLossDate = gPMFunctions.ToSafeDate(m_vClaimDetails(kClaimDetailLossFromDate, 0), DateTime.Today)
                m_lProductId = gPMFunctions.ToSafeLong(m_vClaimDetails(kClaimDetailProductId, 0), 0)
                m_lClaimSourceId = gPMFunctions.ToSafeLong(m_vClaimDetails(kClaimDetailClaimSourceId, 0), 0)
                m_lLossCurrencyId = gPMFunctions.ToSafeLong(m_vClaimDetails(kClaimDetailLossCurrencyId, 0), 0)
                m_sLossCurrency = CStr(m_vClaimDetails(kClaimDetailLossCurrencyDesc, 0)).Trim()
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

                If gPMFunctions.ToSafeDate(m_vClaimDetails(kClaimDetailLeadAgentDateCancelled, 0), #12/29/1899#) <> #12/29/1899# Then

                    dtDate = gPMFunctions.ToSafeDate(m_vClaimDetails(kClaimDetailLeadAgentDateCancelled, 0), #12/29/1899#)

                    ' if the date isnt sql's default date value
                    If dtDate <> gPMFunctions.ToSafeDate("29-12-1899", #12/29/1899#) Then
                        ' only set the date cancelled indicator if the prevent claim payment to cancelled agents
                        ' flag is set as this is the only time we take "date cancelled"  into account
                        'If m_bProductPreventPaymentsToCancelledAgents Then
                        m_bLeadAgentDateCancelled = True 'PN 30836
                        'End If
                    End If

                End If

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

                If gPMFunctions.ToSafeDate(m_vClaimDetails(kClaimDetailTransferAgentDateCancelled, 0), #12/29/1899#) <> #12/29/1899# Then

                    dtDate = gPMFunctions.ToSafeDate(m_vClaimDetails(kClaimDetailTransferAgentDateCancelled, 0), #12/29/1899#)

                    ' if the date isnt sql's default date value
                    If dtDate <> gPMFunctions.ToSafeDate("29-12-1899", #12/29/1899#) Then
                        ' only set the data cancelled indicator if the prevent claim payment to cancelled agents
                        ' flag is set as this is the only time we take "date cancelled" into account
                        'If m_bProductPreventPaymentsToCancelledAgents Then
                        m_bTransferAgentDateCancelled = True
                        'End If
                    End If
                End If

                m_lClassOfBusinessId = gPMFunctions.ToSafeLong(m_vClaimDetails(kClaimDetailClassOfBusinessId, 0), 0)
                m_sClassOfBusinessCode = CStr(m_vClaimDetails(kClaimDetailClassOfBusinessCode, 0))
                m_lInsuranceFileCnt = gPMFunctions.ToSafeLong(m_vClaimDetails(kClaimDetailInsuranceFileCnt, 0), 0)

                m_bPostClaimTax = gPMFunctions.ToSafeLong(m_vClaimDetails(kClaimDetailPostClaimsTaxes, 0), 0)
                m_sClaimNumber = CStr(m_vClaimDetails(kClaimDetailClaimNumber, 0)).Trim()

                txtRiskType.Text = m_sRisktype
                txtLossCurrency.Text = m_sLossCurrency
                txtLossDate.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateShort, m_dtLossDate)
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
    ''' PopulateClaimPaymentDetails
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function PopulateClaimPaymentDetails() As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Const kMethodName As String = "PopulateClaimPaymentDetails"

        Dim sCurrencyDescription As String = String.Empty
        Dim sRiskTypeDescription As String = String.Empty
        Dim sComments As String = String.Empty
        Dim sPayeeName As String = String.Empty
        Dim sPayeeBankName As String = String.Empty
        Dim sPayeeSortCode As String = String.Empty
        Dim sPayeeAccountNo As String = String.Empty
        Dim sPayeeCountry As String = String.Empty
        Dim sPayeeComments As String = String.Empty
        Dim sInsuredTaxNumber As String = String.Empty
        Dim sPayeeTaxNumber As String = String.Empty

        Const kThisPayment As Integer = 0


        Try
            ' fraPayee
            m_lClaimPerilId = gPMFunctions.ToSafeLong(m_vClaimPaymentDetails(kClaimPaytDetClaimPerilId, kThisPayment), 0)

            If Not m_bViewPaymentMode Then
                SelectcboItem(cboClaimPaymentTo, gPMFunctions.ToSafeLong(m_vClaimPaymentDetails(kClaimPaytDetClaimPaymentToId, 0)))
                SelectcboItem(cboSafeHarbour, gPMFunctions.ToSafeLong(m_vClaimPaymentDetails(kClaimPaytDetSafeHarbourId, 0)))
                SelectcboItem(cboMediaType, gPMFunctions.ToSafeLong(m_vClaimPaymentDetails(kClaimPaytDetPayeeMediaType, 0)))
            End If

            txtClaimPaymentTo.Text = CStr(m_vClaimPaymentDetails(kClaimPaytDetClaimPaymentToDesc, kThisPayment))
            txtSafeHarbour.Text = CStr(m_vClaimPaymentDetails(kClaimPaytDetSafeHarbourDesc, kThisPayment))
            txtMediaType.Text = CStr(m_vClaimPaymentDetails(kClaimPaytDetMediaTypeDesc, kThisPayment))

            m_lPaymentPartyTo = gPMFunctions.ToSafeLong(m_vClaimPaymentDetails(kClaimPaytDetPaymentPartyTo, kThisPayment), 0)

            ' the actual calculation is in the sql so just set the returned values accordingly
            OptClaimPayable.Checked = gPMFunctions.ToSafeLong(m_vClaimPaymentDetails(kClaimPaytDetClaimPayable, kThisPayment)) > 0
            OptParty.Checked = gPMFunctions.ToSafeLong(m_vClaimPaymentDetails(kClaimPaytDetParty, kThisPayment)) > 0
            OptAgent.Checked = gPMFunctions.ToSafeLong(m_vClaimPaymentDetails(kClaimPaytDetAgent, kThisPayment)) > 0
            OptClient.Checked = gPMFunctions.ToSafeLong(m_vClaimPaymentDetails(kClaimPaytDetClient, kThisPayment)) > 0
            m_lOtherPartyCnt = gPMFunctions.ToSafeLong(m_vClaimPaymentDetails(kClaimPaytDetPartyCnt, kThisPayment))

            ' if this is view payment mode
            If m_bViewPaymentMode Then
                ' and none of the options have been set on
                If Not OptClient.Checked And Not OptAgent.Checked And Not OptParty.Checked Then
                    ' it is an historic payment to the claim payable account
                    OptClaimPayable.Checked = True
                End If
                m_lWorkClaimPaymentId = CInt(m_vClaimPaymentDetails(kClaimPaytDetWorkClaimPaymentId, kThisPayment))
            End If

            txtParty.Text = CStr(m_vClaimPaymentDetails(kClaimPaytDetPartyShortname, kThisPayment))

            ' fraInsuredTaxAdjustment
            chkITDomiciled.CheckState = gPMFunctions.ToSafeLong(m_vClaimPaymentDetails(kClaimPaytDetInsuredDomiciled, kThisPayment))
            txtITPercentage.Text = StringsHelper.Format(gPMFunctions.ToSafeCurrency(m_vClaimPaymentDetails(kClaimPaytDetInsuredPercentage, kThisPayment), 0), "0.00")
            txtITTaxNo.Text = CStr(m_vClaimPaymentDetails(kClaimPaytDetInsuredTaxNumber, kThisPayment))

            ' fraPayeeTaxAdjustment
            chkPTDomiciled.CheckState = gPMFunctions.ToSafeLong(m_vClaimPaymentDetails(kClaimPaytDetPayeeDomiciled, kThisPayment), 0)
            txtPTPercentage.Text = StringsHelper.Format(gPMFunctions.ToSafeCurrency(m_vClaimPaymentDetails(kClaimPaytDetPayeePercentage, kThisPayment), 0), "0.00")
            txtPTTaxNo.Text = CStr(m_vClaimPaymentDetails(kClaimPaytDetPayeeTaxNumber, kThisPayment))

            ' fraSafeHarbour
            txtSFPercentage.Text = StringsHelper.Format(gPMFunctions.ToSafeCurrency(m_vClaimPaymentDetails(kClaimPaytDetSafeHarbourPercentage, kThisPayment), 0), "0.00")

            ' fraExemptions
            chkTaxExempt.CheckState = gPMFunctions.ToSafeLong(m_vClaimPaymentDetails(kClaimPaytDetIsTaxExempt, kThisPayment), 0)
            chkWHTExempt.CheckState = gPMFunctions.ToSafeLong(m_vClaimPaymentDetails(kClaimPaytDetIsWHTExempt, kThisPayment), 0)

            ' fraSettlement
            chkSettlement.CheckState = gPMFunctions.ToSafeLong(m_vClaimPaymentDetails(kClaimPaytDetIsSettlement, kThisPayment), 0)
            chkIsExGratia.CheckState = gPMFunctions.ToSafeLong(m_vClaimPaymentDetails(kClaimPayDetIsExGratia, kThisPayment), 0)

            ' payment details
            txtPayeeName.Text = CStr(m_vClaimPaymentDetails(kClaimPaytDetPayeeName, kThisPayment))
            txtMediaRef.Text = CStr(m_vClaimPaymentDetails(kClaimPaytDetMediaRef, kThisPayment))
            txtBankAccountNo.Text = CStr(m_vClaimPaymentDetails(kClaimPaytDetPayeeAccountNo, kThisPayment))
            txtBankCode.Text = CStr(m_vClaimPaymentDetails(kClaimPaytDetPayeeSortCode, kThisPayment))
            txtBankName.Text = CStr(m_vClaimPaymentDetails(kClaimPaytDetPayeeBankName, kThisPayment))
            txtPayeeComments.Text = CStr(m_vClaimPaymentDetails(kClaimPaytDetPayeeComments, kThisPayment))
            txtBIC.Text = CStr(m_vClaimPaymentDetails(kClaimPayBIC, kThisPayment))
            txtIBAN.Text = CStr(m_vClaimPaymentDetails(kClaimPayIBAN, kThisPayment))
            txtOurReference.Text = CStr(m_vClaimPaymentDetails(kClaimPaytDetOurReference, kThisPayment))
            uctPMAddressControl1.AddressLine1 = CStr(m_vClaimPaymentDetails(kClaimPaytDetPayeeAddress1, kThisPayment))
            uctPMAddressControl1.AddressLine2 = CStr(m_vClaimPaymentDetails(kClaimPaytDetPayeeAddress2, kThisPayment))
            uctPMAddressControl1.AddressLine3 = CStr(m_vClaimPaymentDetails(kClaimPaytDetPayeeAddress3, kThisPayment))
            uctPMAddressControl1.AddressLine4 = CStr(m_vClaimPaymentDetails(kClaimPaytDetPayeeAddress4, kThisPayment))
            uctPMAddressControl1.PostCode = CStr(m_vClaimPaymentDetails(kClaimPaytDetPayeePostalCode, kThisPayment))
            uctPMAddressControl1.CountryId = gPMFunctions.ToSafeLong(m_vClaimPaymentDetails(kClaimPaytDetPayeeCountry, kThisPayment), 0)
            txtThirdPartyReference.Text = CStr(m_vClaimPaymentDetails(kClaimPaytDetThirdPartyReference, kThisPayment))
            If CStr(m_vClaimPaymentDetails(kClaimPaytDetChequeDate, kThisPayment)) <> "" Then
                dtpChequeDate.Value = CDate(m_vClaimPaymentDetails(kClaimPaytDetChequeDate, kThisPayment))
            End If
            'Party Bank Details
            m_vBankPaymentTypeId = gPMFunctions.ToSafeInteger(m_vClaimPaymentDetails(kClaimBankPaymentTypeId, kThisPayment))

            'if party cnt is not Zero and display mode should be zero
            If Val(m_vClaimPaymentDetails(kClaimDetailLeadAgentCnt, kThisPayment)) > 0 AndAlso m_iTask = PMEComponentAction.PMView Then
                uctPartyBankCombo1.Initialise()
                uctPartyBankCombo1.PartyCnt = m_vClaimPaymentDetails(kClaimDetailLeadAgentCnt, kThisPayment)
                uctPartyBankCombo1.PopulateScreen()
                uctPartyBankCombo1.EnableCombo = False
                uctPartyBankCombo1.SelectedPaymentID = Val(m_vClaimPaymentDetails(kClaimBankPaymentTypeId, kThisPayment))
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
        Dim bVisible, bStatus As Boolean

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' if the claim payment authorisation scripts option is switched on...
            'If m_bPaymentAuthorityScriptsOn Then

            ' only check if were actually in the payment roadmap
            ' otherwise this call is pointless
            If m_sTransactionType = "C_CP" Then
                ' and the user has referred payments

                lReturn = m_oBusiness.CheckReferredPayment(m_lWorkClaimID, bStatus, m_iNoofReferredPayments, m_cUnAuthorisedClaimPayment)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue And lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    gPMFunctions.RaiseError(kMethodName, "CheckReferredPayment", gPMConstants.PMELogLevel.PMLogError)
                End If

                If bStatus Then
                    m_bClaimHasReferredPayments = True
                End If
            End If
            'End If

            '********************************************
            '**  there are 3 modes for this component  **
            '********************************************
            ' 1. View A Specific Payment
            ' 2. View All Payments (In Maintain or Open Claim)
            ' 3. View All Payment (in Pay Claim Mode)

            '    ' determine which component mode we are in
            '    If m_bViewPaymentMode Then
            '        m_iComponentMode = kModeViewPayment
            '    ElseIf m_sTransactionType = "C_CP" Then
            '        m_iComponentMode = kModeNewPayment
            '    ElseIf m_sTransactionType = "C_CO" Or m_sTransactionType = "C_CR" Then
            '        m_iComponentMode = kModeHistoricPayment
            '    End If

            '    ' setup component based on component mode
            '    Select Case m_iComponentMode
            '
            '        Case kViewPaymentMode
            '            lReturn = SetupInterface(kModeViewPayment)
            '            If lReturn <> PMTrue Then
            '                RaiseError kMethodName, "SetupInterface", PMLogError
            '            End If
            '
            '        Case kNewPaymentMode
            '            lReturn = SetupInterface(kModeNewPayment)
            '            If lReturn <> PMTrue Then
            '                RaiseError kMethodName, "SetupInterface", PMLogError
            '            End If
            '
            '        Case kHistoricPaymentMode
            '            lReturn = SetupInterface(kModeHistoricPayment)
            '            If lReturn <> PMTrue Then
            '                RaiseError kMethodName, "SetupInterface", PMLogError
            '            End If
            '
            '    End Select

            '******************************************************************************
            '******************************************************************************
            '******************************************************************************
            '******************************************************************************
            '******************************************************************************
            '******************************************************************************

            ' setup default payment options
            lReturn = SetupDefaultClaimPaymentOptions()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetupDefaultClaimPaymentOptions Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' setup default payment options
            lReturn = SetupPayeeInterfaceDefaults()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetupPayeeInterfaceDefaults Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' set up the advanced tax frames
            lReturn = SetupFormLayout()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetupAvailableFrames  Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' set up view payment mode
            If m_bViewPaymentMode Then
                lReturn = SetupViewPaymentInterface()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "SetupViewPaymentOnly Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                lReturn = SetupThisPaymentInterface()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "SetupThisPaymentInterface", gPMConstants.PMELogLevel.PMLogError)
                End If
            Else
                lReturn = SetupThisPaymentInterface()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "SetupThisPaymentInterface", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            If m_sTransactionType = "C_CR" Then
                fraPayee.Enabled = False
                fraClaimInformation.Enabled = False
                cmdEditPayee.Visible = False
            End If

            ' on load disable the edit and history buttons
            ' this should only be enabled when an item has been
            ' selected from the payment details list
            cmdReserveEdit.Enabled = False '(RC) QBENZ001

            cmdEdit.Enabled = m_bViewPaymentMode

            cmdHistory.Enabled = False
            cmdPaymentLock.Enabled = False
            If RI2007Enabled Then
                cmdPaymentLock.Enabled = False
            End If
            If Not Information.IsArray(m_vReserveAndPaymentDetails) And Not m_bViewPaymentMode Then
                lReturn = DisableInterface()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "DisableInterface Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            '******************************************************************************
            '******************************************************************************
            '******************************************************************************
            '******************************************************************************
            '******************************************************************************
            '******************************************************************************



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

            Return result


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


            lReturn = m_oBusiness.GetClaimDetails(v_lClaimId:=m_lWorkClaimID, v_lClaimPerilId:=m_lWorkClaimPerilId, r_vResults:=m_vClaimDetails)
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
        Dim lPaymentItemArrayPos As Integer
        Dim ofrmPaymentDetails As frmPaymentDetails
        Dim oListItem As ListViewItem
        Dim oPaymentItem As cPaymentItem
        Dim sReserveId As String = ""
        Dim crTotalReserve, crPaidToDate, crBalance, crThisPaymentInclTax, crThisPaymentTax, crThisPaymentExcess, crCostToClaim As Decimal
        Dim dPaymentToLossXRate As Integer
        Dim sReserveDesc As String = ""
        Dim ofrmEditWarning As frmEditWarning
        Dim lDisplayMode As gPMConstants.PMEReturnCode
        Dim bWarningShown As Boolean
        Dim lPaymentCurrencyFilter As Integer
        Dim bIsExcess As Boolean
        Dim oTaxItem As cTaxParameters
        Dim bPayeeDetailsValid As Boolean
        Dim lNoOfPayments As Integer
        Dim bClaimHasXOLLines As Boolean
        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            ' check that the payee details are all okay
            ' prior to creating a payment
            lReturn = CType(ValidatePayeeDetails(r_bPayeeDetailsValid:=bPayeeDetailsValid), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "ValidatePayeeDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If bPayeeDetailsValid Then

                ' get the payment item
                lReturn = CType(GetPaymentItem(r_oPaymentItem:=oPaymentItem), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetPaymentItem Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' determine the display mode to use
                lReturn = CType(GetDisplayMode(v_sAdvancedTaxScript:=oPaymentItem.AdvancedTaxScript, r_lDisplayMode:=lDisplayMode, r_bWarningShown:=bWarningShown), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetDisplayMode Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' if the user hasnt specified to cancel then
                If lDisplayMode <> gPMConstants.PMEReturnCode.PMCancel Then

                    ' if this is an excess item
                    If oPaymentItem.IsExcess Then

                        ' get the number of payments lines added in this session excluding excess lines
                        lReturn = CType(GetNoOfPaymentsItems(lNoOfPayments), gPMConstants.PMEReturnCode)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "GetNoOfPaymentsItems Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                        If lNoOfPayments > 0 Then

                            ' confirm the user wants to edit the excess and acknowledges the conseqences
                            If MessageBox.Show("Editing the excess row will result in all payments made in this session being cleared down." &
                                               Strings.Chr(13) & Strings.Chr(10) & "Do you want to continue?", "Excess Line Deletion", MessageBoxButtons.YesNo) = System.Windows.Forms.DialogResult.No Then
                                lDisplayMode = gPMConstants.PMEReturnCode.PMCancel
                            Else
                                ' erase any payments that have been made in this session
                                lReturn = ResetPaymentsOnly()
                                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    gPMFunctions.RaiseError(kMethodName, "ResetPaymentsOnly Failed", gPMConstants.PMELogLevel.PMLogError)
                                End If

                            End If

                        End If

                    End If

                End If

                ' if the user hasnt specified to cancel then
                If lDisplayMode <> gPMConstants.PMEReturnCode.PMCancel Then

                    ' get the default tax item
                    lReturn = CType(GetDefaultTaxItem(r_oTaxItem:=oTaxItem), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "GetTaxItem", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    ' create new instance of payment details form
                    ofrmPaymentDetails = New frmPaymentDetails()
                    If m_bShowPaymentDetailsHiddenMode Then
                        ofrmPaymentDetails.ShowPaymentDetailsHiddenMode = True
                    End If
                    ofrmPaymentDetails.IsOpenClaimNoTrans = m_bOpenClaimNoTrans
                    ofrmPaymentDetails.RI2007Enabled = m_bRI2007Enabled
                    ' load and setup the edit form
                    lReturn = CType(LoadEditForm(v_lDisplayMode:=lDisplayMode, v_bWarningShown:=bWarningShown, v_oPaymentItem:=oPaymentItem, v_oTaxItem:=oTaxItem, r_ofrmPaymentDetails:=ofrmPaymentDetails), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "LoadEditForm Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    m_lReturn = iPMFunc.SetWindowPlacement(ofrmPaymentDetails.Handle.ToInt32(), True)

                    ' Display Form
                    If m_bShowPaymentDetailsHiddenMode Then
                        ofrmPaymentDetails.ActionTaxGroupSelection(True)
                        ofrmPaymentDetails.ActionOk()

                    Else
                        ofrmPaymentDetails.ShowDialog()
                    End If


                    ' only if this is not view mode does anything extra need to be done here
                    If Not m_bViewPaymentMode Then

                        ' if the user confirms the details on the payment item detail form
                        If ofrmPaymentDetails.status = gPMConstants.PMEReturnCode.PMOK Then

                            lReturn = CType(ProcessEditForm(r_oPaymentItem:=oPaymentItem, r_ofrmPaymentDetails:=ofrmPaymentDetails), gPMConstants.PMEReturnCode)

                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError(kMethodName, "SaveEditFormDetails", gPMConstants.PMELogLevel.PMLogError)
                            End If
                            ''Start(Saurabh Agrawal) Tech SPec LOA010 Claim Payment Improvement
                            m_lReturn = EnableDisablePartyCombo()

                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError(kMethodName, "GetPartyBanks", gPMConstants.PMELogLevel.PMLogError)
                            End If

                            m_lReturn = FillPartyBankDetails()

                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError(kMethodName, "FillPartyBankDetails", gPMConstants.PMELogLevel.PMLogError)
                            End If
                            ''End(Saurabh Agrawal) Tech SPec LOA010 Claim Payment Improvement
                        End If
                    End If

                End If

                ' reset available actions for the selected item
                lReturn = CType(ActionSelectPaymentItem(lvwPaymentDetails.SelectedItems.Item(0)), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "ActionSelectPaymentItem Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            End If




        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' destroy payment details form
            ofrmEditWarning = Nothing
            ofrmPaymentDetails = Nothing




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ActionReserveEdit
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : Rajesh Choudhary : 15 Jan 2007 : QBENZ001
    ' ***************************************************************** '
    Private Function ActionReserveEdit() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ActionReserveEdit"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim ofrmReserve As frmReserve
        Dim l_oPaymentItem As cPaymentItem
        Dim vNewData(,) As Object
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get the payment item
            lReturn = CType(GetPaymentItem(r_oPaymentItem:=l_oPaymentItem), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetPaymentItem Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' create new instance of Reserve details form
            ofrmReserve = New frmReserve()

            'set form properties
            ofrmReserve.PaymentLine = l_oPaymentItem.ReserveDesc
            ofrmReserve.InitialReserve = l_oPaymentItem.TotalReserve - l_oPaymentItem.PaidToDate
            ofrmReserve.LossCurrency = m_sLossCurrency
            ofrmReserve.IsOpenClaimNoTrans = m_bOpenClaimNoTrans
            ofrmReserve.TransactionType = m_sTransactionType ' PN 76954
            ofrmReserve.RI2007Enabled = m_bRI2007Enabled ' PN 76954
            ofrmReserve.RevisedReserve = l_oPaymentItem.ThisReserveRevision
            'If l_oPaymentItem.ThisReserveRevision = 0 Then
            '    ofrmReserve.RevisedReserve = l_oPaymentItem.TotalReserve
            'Else
            '    ofrmReserve.RevisedReserve = l_oPaymentItem.ThisReserveRevision
            'End If

            m_lReturn = iPMFunc.SetWindowPlacement(ofrmReserve.Handle.ToInt32(), True)


            ' display form
            ofrmReserve.ShowDialog()

            If ofrmReserve.status = gPMConstants.PMEReturnCode.PMOK Then
                If m_bOpenClaimNoTrans Then
                    l_oPaymentItem.TotalReserve -= l_oPaymentItem.ThisReserveRevision
                End If
                l_oPaymentItem.ThisReserveRevision = ofrmReserve.RevisedReserve
                '' Update Reserve
                'If ofrmReserve.RevisedReserve <> ofrmReserve.InitialReserve Then

                'Else
                '	l_oPaymentItem.ThisReserveRevision = 0
                'End If

                l_oPaymentItem.ThisReserveRevision = ofrmReserve.RevisedReserve
                'UPDATE LIST VIEW
                If m_bOpenClaimNoTrans Then
                    ListViewHelper.GetListViewSubItem(lvwPaymentDetails.Items.Item(m_lSelectedPayeeDetailIndex - 1), kPayDetailsThisReserveRevision).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, l_oPaymentItem.ThisReserveRevision)
                    ListViewHelper.GetListViewSubItem(lvwPaymentDetails.Items.Item(m_lSelectedPayeeDetailIndex - 1), kPayDetailsSubItemsTotalReserve).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, l_oPaymentItem.TotalReserve + l_oPaymentItem.ThisReserveRevision)
                    ListViewHelper.GetListViewSubItem(lvwPaymentDetails.Items.Item(m_lSelectedPayeeDetailIndex - 1), kPayDetailsSubItemsCurrentReserve).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, (l_oPaymentItem.TotalReserve + l_oPaymentItem.ThisReserveRevision) - (l_oPaymentItem.ThisPayment))
                Else
                    ListViewHelper.GetListViewSubItem(lvwPaymentDetails.Items.Item(m_lSelectedPayeeDetailIndex - 1), kPayDetailsThisReserveRevision).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, l_oPaymentItem.ThisReserveRevision)
                    ListViewHelper.GetListViewSubItem(lvwPaymentDetails.Items.Item(m_lSelectedPayeeDetailIndex - 1), kPayDetailsSubItemsTotalReserve).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, l_oPaymentItem.TotalReserve + l_oPaymentItem.ThisReserveRevision)
                    ListViewHelper.GetListViewSubItem(lvwPaymentDetails.Items.Item(m_lSelectedPayeeDetailIndex - 1), kPayDetailsSubItemsCurrentReserve).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, (ofrmReserve.InitialReserve + l_oPaymentItem.ThisReserveRevision) - (l_oPaymentItem.ThisPayment))
                End If

                ' populate total line
                lReturn = PopulateTotals()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "PopulateTotals Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                If m_bOpenClaimNoTrans Then
                    ReDim vNewData(2, 0)

                    vNewData(0, 0) = l_oPaymentItem.ReserveId

                    vNewData(1, 0) = l_oPaymentItem.ThisPayment

                    vNewData(2, 0) = l_oPaymentItem.ThisReserveRevision

                    RaiseEvent DataHasChanged(Me, New DataHasChangedEventArgs(vNewData))
                End If
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' destroy payment details form
            l_oPaymentItem = Nothing
            ofrmReserve = Nothing




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ActionHistory
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 08-08-2005 : 360 - Taxes On Claims
    ' ***************************************************************** '
    Private Function ActionHistory() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ActionHistory"

        Dim lReturn As Integer

        'Developer Guide No. 88
        Dim oPaymentList As Object
        Dim oListItem As ListViewItem
        Dim lRow As Integer

        'Developer Guide No. 88
        Dim oReceiptList As Object


        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If m_lSelectedPayeeDetailIndex <> 0 Then



                ' get the list item
                oListItem = lvwPaymentDetails.Items.Item(m_lSelectedPayeeDetailIndex - 1)

                If oListItem.Text = kTypeRecovery Then

                    Dim temp_oReceiptList As Object
                    lReturn = m_oObjectManager.GetInstance(temp_oReceiptList, sClassName:="iCLMListReceipts.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                    oReceiptList = temp_oReceiptList

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "GetInstance of iCLMListREceipts.Interface Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    ' set required parameters

                    oReceiptList.ClaimId = m_lWorkClaimID

                    oReceiptList.ClaimPerilId = m_lWorkClaimPerilId

                    oReceiptList.RecoveryText = ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsTypeDesc).Text

                    oReceiptList.RecoveryType = m_vReserveAndPaymentDetails(kPaymentDetailsTypeId, m_lSelectedPayeeDetailIndex - 2)


                    ' start interface

                    lReturn = oReceiptList.Start()
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "iCLMPayments.Interface.Start", gPMConstants.PMELogLevel.PMLogError)
                    End If
                Else
                    Dim temp_oPaymentList As Object
                    lReturn = m_oObjectManager.GetInstance(temp_oPaymentList, sClassName:="iCLMListPayments.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                    oPaymentList = temp_oPaymentList

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "GetInstance of iCLMListPayments.Interface Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    ' set required parameters

                    oPaymentList.ClaimId = m_lWorkClaimID

                    oPaymentList.WorkClaimPerilId = m_lWorkClaimPerilId


                    oPaymentList.ReserveId = Convert.ToString(oListItem.Tag)

                    oPaymentList.ReserveText = ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsTypeDesc).Text

                    ' start interface

                    lReturn = oPaymentList.Start()
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "iCLMPayments.Interface.Start", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            oPaymentList = Nothing




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetClaimPaymentItemDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 08-08-2005 : 360 - Taxes On Claims
    ' ***************************************************************** '
    Private Function GetClaimPaymentItemDetails() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimPaymentItemDetails"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            lReturn = m_oBusiness.GetClaimPaymentItemDetails(v_lClaimPaymentId:=m_lWorkClaimPaymentId, r_vResults:=m_vClaimPaymentItemDetails)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetClaimPaymentItemDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not Information.IsArray(m_vClaimPaymentItemDetails) Then
                'RaiseError kMethodName, "GetClaimPaymentItemDetails Failed to return any data", PMLogError
            End If



        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally





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

            If Information.IsArray(v_vValuesArray) Then

                llBound = v_vValuesArray.GetLowerBound(1)
                lUBound = v_vValuesArray.GetUpperBound(1)

                For lItem As Integer = llBound To lUBound


                    lItemId = CInt(v_vValuesArray(kLookupItemId, lItem))

                    sDescription = CStr(v_vValuesArray(kLookupDescription, lItem))

                    r_oComboBox.Items.Insert(lItem, sDescription)
                    VB6.SetItemData(r_oComboBox, lItem, lItemId)

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

        Dim result As Integer = 0
        Const kMethodName As String = "PopulateLookups"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = CType(PopulateCombo(cboClaimPaymentTo, m_vClaimPaymentTo), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "PopulateCombo = ClaimPaymentTo Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            '    lReturn = PopulateCombo(cboCountry, m_vCountry)
            '    If lReturn <> PMTrue Then
            '        RaiseError kMethodName, "PopulateCombo - Country Failed", PMLogError
            '    End If

            lReturn = CType(PopulateCombo(cboMediaType, m_vMediaType), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "PopulateCombo - Media Type Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            lReturn = CType(PopulateCombo(cboSafeHarbour, m_vSafeHarbour), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "PopulateCombo - Safe Harbour Failed", gPMConstants.PMELogLevel.PMLogError)
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
    ' Name: PopulatePaymentDetailsListView
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 09-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function PopulatePaymentDetailsListView() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulatePaymentDetailsListView"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lType, lTypeId, llBound, lUBound As Integer
        Dim sTypeDesc As String = ""
        Dim crTotalReserve, crPaidToDate, crPaidToDateTax, crPaidToDateTaxWHT, crCurrentReserve, crThisPayment, crThisPaymentTax, crThisPaymentTaxWHT, crThisPaymentExcess, crCostToClaim As Decimal
        Dim oListItem As ListViewItem
        Dim bIsExcess As Boolean
        Dim lIsHistory As Integer

        ''Start(Saurabh Agrawal)Tech Spec QBENZCR004 Claims Recovery Reinsurance
        Dim crReceivedToDate, crReceivedToDateTax As Decimal
        ''End(Saurabh Agrawal)Tech Spec QBENZCR004 Claims Recovery Reinsurance


        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            lvwPaymentDetails.Items.Clear()

            If Information.IsArray(m_vReserveAndPaymentDetails) Then

                ' determine array boundaries
                llBound = m_vReserveAndPaymentDetails.GetLowerBound(1)
                lUBound = m_vReserveAndPaymentDetails.GetUpperBound(1)

                ' for each reserve / recovery type
                For lItem As Integer = llBound To lUBound

                    If lItem = llBound Then

                        ' add the total list item
                        oListItem = lvwPaymentDetails.Items.Add(kLVWTotal)

                        ' populate reserve description
                        ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsTypeDesc).Text = GetResString(kResDetailsTotal)

                    End If

                    ' get reserve / recovery details
                    lType = gPMFunctions.ToSafeLong(m_vReserveAndPaymentDetails(kPaymentDetailsType, lItem), 0)
                    lTypeId = gPMFunctions.ToSafeLong(m_vReserveAndPaymentDetails(kPaymentDetailsTypeId, lItem), 0)
                    sTypeDesc = CStr(m_vReserveAndPaymentDetails(kPaymentDetailsReserveDescription, lItem))
                    crTotalReserve = gPMFunctions.ToSafeCurrency(m_vReserveAndPaymentDetails(kPaymentDetailsTotalReserve, lItem), 0)

                    ''Start(Saurabh Agrawal)Tech Spec QBENZCR004 Claims Recovery Reinsurance
                    '' Check for RI 2007 if enabled then display the new columns added else
                    ''Display the values as existing
                    If m_bRI2007Enabled Then
                        If lType = StringsHelper.ToDoubleSafe("2") Then
                            crReceivedToDate = gPMFunctions.ToSafeCurrency(m_vReserveAndPaymentDetails(kPaymentDetailsPaidToDate, lItem), 0)
                            crReceivedToDateTax = gPMFunctions.ToSafeCurrency(m_vReserveAndPaymentDetails(kPaymentDetailsPaidToDateTax, lItem), 0)
                            crPaidToDate = 0
                            crPaidToDateTax = 0
                        Else
                            crPaidToDate = gPMFunctions.ToSafeCurrency(m_vReserveAndPaymentDetails(kPaymentDetailsPaidToDate, lItem), 0)
                            crPaidToDateTax = gPMFunctions.ToSafeCurrency(m_vReserveAndPaymentDetails(kPaymentDetailsPaidToDateTax, lItem), 0)
                            crReceivedToDate = 0
                            crReceivedToDateTax = 0

                        End If
                        ''End(Saurabh Agrawal)Tech Spec QBENZCR004 Claims Recovery Reinsurance
                    Else
                        crPaidToDate = gPMFunctions.ToSafeCurrency(m_vReserveAndPaymentDetails(kPaymentDetailsPaidToDate, lItem), 0)
                        crPaidToDateTax = gPMFunctions.ToSafeCurrency(m_vReserveAndPaymentDetails(kPaymentDetailsPaidToDateTax, lItem), 0)
                    End If

                    crPaidToDateTaxWHT = gPMFunctions.ToSafeCurrency(m_vReserveAndPaymentDetails(kPaymentDetailsPaidToDateTaxWHT, lItem), 0)
                    If m_bOpenClaimNoTrans Then
                        crCurrentReserve = crTotalReserve - crPaidToDate
                    Else
                        crCurrentReserve = crTotalReserve - crPaidToDate - crPaidToDateTax - crPaidToDateTaxWHT
                    End If
                    crThisPayment = gPMFunctions.ToSafeCurrency(m_vReserveAndPaymentDetails(kPaymentDetailsThisPayment, lItem), 0)
                    crThisPaymentTax = gPMFunctions.ToSafeCurrency(m_vReserveAndPaymentDetails(kPaymentDetailsThisPaymentTax, lItem), 0)
                    crThisPaymentTaxWHT = gPMFunctions.ToSafeCurrency(m_vReserveAndPaymentDetails(kPaymentDetailsThisPaymentTaxWHT, lItem), 0)
                    crCostToClaim = gPMFunctions.ToSafeCurrency(m_vReserveAndPaymentDetails(kPaymentDetailsCostToClaim, lItem), 0)
                    bIsExcess = gPMFunctions.ToSafeBoolean(m_vReserveAndPaymentDetails(kPaymentDetailsIsExcess, lItem), False)

                    ''Start(Saurabh Agrawal)Tech Spec QBENZCR004 Claims Recovery Reinsurance
                    ''If RI 2007 is Enabled then make lIsHistory 1 for Recovery rows also
                    If m_bRI2007Enabled And crReceivedToDate <> 0 Then
                        lIsHistory = 1
                    Else
                        lIsHistory = gPMFunctions.ToSafeLong(m_vReserveAndPaymentDetails(kPaymentDetailsIsHistory + 1, lItem), 0)
                    End If
                    ''End(Saurabh Agrawal)Tech Spec QBENZCR004 Claims Recovery Reinsurance
                    If bIsExcess Then
                        ' store the fact that there is at least one excess row
                        m_bExcessRowExists = True
                        m_bExcessDefaulted = True
                    End If

                    'PN53193
                    If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                        ''PN56294  Saurabh - Added the If condition the following code should run only for normal reserves and not for recovery lines
                        If lType <> StringsHelper.ToDoubleSafe("2") Then
                            crPaidToDate += crCostToClaim
                            crCurrentReserve = crTotalReserve - crPaidToDate
                            crPaidToDateTax = (crPaidToDateTax + crPaidToDateTaxWHT) + (crThisPaymentTax + crThisPaymentTaxWHT)
                        End If
                    End If

                    ' add list item
                    oListItem = lvwPaymentDetails.Items.Add(CStr(lType))

                    ' populate list sub item details
                    ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsTypeId).Text = CStr(lTypeId)
                    ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsTypeDesc).Text = sTypeDesc
                    ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsTotalReserve).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, crTotalReserve)
                    'ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsTotalReserve).Text = StringsHelper.Format(crTotalReserve, "0.00")
                    If m_bOpenClaimNoTrans Then
                        ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsThisReserveRevision).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_vReserveAndPaymentDetails(kPaymentDetailsCurrentReserve, lItem))
                        'ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsThisReserveRevision).Text = StringsHelper.Format(gPMFunctions.ToSafeCurrency(m_vReserveAndPaymentDetails(kPaymentDetailsCurrentReserve, lItem), 0), "0.00")
                        m_vReserveAndPaymentDetails(kPaymentDetailsCurrentReserve, lItem) = "0.00"
                    Else
                        ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsThisReserveRevision).Text = "0.00" '(RC) QBENZ001
                    End If
                    ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsPaidToDate).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, crPaidToDate)
                    'ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsPaidToDate).Text = StringsHelper.Format(crPaidToDate, "0.00")
                    ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsCurrentReserve).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, crCurrentReserve)
                    'ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsCurrentReserve).Text = StringsHelper.Format(crCurrentReserve, "0.00")
                    ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsCostToClaim).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, crCostToClaim)
                    'ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsCostToClaim).Text = StringsHelper.Format(crCostToClaim, "0.00")
                    ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsIsExcess).Text = CStr(bIsExcess)
                    ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsIsHistory).Text = CStr(lIsHistory)
                    If bIsExcess Then
                        ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsThisPaymentInclTax).Text = "0.00" ' kLVSCExcessNotSet
                        ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsPaidToDateTax).Text = kLVSCNotApplicable
                        ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsThisPaymentTax).Text = kLVSCNotApplicable
                    Else
                        ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsThisPaymentInclTax).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, crThisPayment)
                        'ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsThisPaymentInclTax).Text = StringsHelper.Format(crThisPayment, "0.00")
                        ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsPaidToDateTax).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, IIf(lType <> 2, crPaidToDateTax, crPaidToDateTax + crPaidToDateTaxWHT))
                        'ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsPaidToDateTax).Text = StringsHelper.Format(IIf(lType <> 2, crPaidToDateTax, crPaidToDateTax + crPaidToDateTaxWHT), "0.00")  'PN 71693
                        ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsThisPaymentTax).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, crThisPaymentTax + crThisPaymentTaxWHT)
                        'ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsThisPaymentTax).Text = StringsHelper.Format(crThisPaymentTax + crThisPaymentTaxWHT, "0.00")
                    End If

                    oListItem.Tag = CStr(lTypeId)
                    ''Start(Saurabh Agrawal)Tech Spec QBENZCR004 Claims Recovery Reinsurance
                    ''If RI 2007 is Enabled then display the values in new columns added
                    If m_bRI2007Enabled Then
                        ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsReceivedToDate).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, crReceivedToDate * -1)
                        'ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsReceivedToDate).Text = StringsHelper.Format(crReceivedToDate * -1, "0.00")
                        ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsReceivedToDateTax).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, crReceivedToDateTax * -1)
                        'ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsReceivedToDateTax).Text = StringsHelper.Format(crReceivedToDateTax * -1, "0.00")
                        ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsThisReceipt).Text = StringsHelper.Format(0, "0.00")
                        ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsThisReceiptTax).Text = StringsHelper.Format(0, "0.00")
                    End If
                    ''End(Saurabh Agrawal)Tech Spec QBENZCR004 Claims Recovery Reinsurance

                Next
                ''Start(Saurabh Agrawal)Tech Spec QBENZCR004 Claims Recovery Reinsurance
                '' If RI 2007 is not Enabled then hide the new columns.
                If Not m_bRI2007Enabled Then
                    lvwPaymentDetails.Columns.Item(kPayDetColHIndexReceivedToDate - 1).Width = CInt(0)
                    lvwPaymentDetails.Columns.Item(kPayDetColHIndexReceivedToDateTax - 1).Width = CInt(0)
                    lvwPaymentDetails.Columns.Item(kPayDetColHIndexThisReceiptInclTax - 1).Width = CInt(0)
                    lvwPaymentDetails.Columns.Item(kPayDetColHIndexThisReceiptTax - 1).Width = CInt(0)
                End If
                ''End(Saurabh Agrawal)Tech Spec QBENZCR004 Claims Recovery Reinsurance
                ' populate total line
                lReturn = PopulateTotals()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "PopulateTotals Failed", gPMConstants.PMELogLevel.PMLogError)
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
    ' Name: SetupPaymentDetailsListView
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 09-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function SetupPaymentDetailsListView() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupPaymentDetailsListView"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            lvwPaymentDetails.Columns.Clear()

            lvwPaymentDetails.Columns.Insert(kPayDetColHIndexType - 1, kPayDetColHCodeType, "Type", CInt(VB6.TwipsToPixelsX(0)), HorizontalAlignment.Left, -1)

            lvwPaymentDetails.Columns.Insert(kPayDetColHIndexTypeId - 1, kPayDetColHCodeTypeId, "Type", CInt(VB6.TwipsToPixelsX(0)), HorizontalAlignment.Left, -1)

            lvwPaymentDetails.Columns.Insert(kPayDetColHIndexTypeDesc - 1, kPayDetColHCodeTypeDesc, "", CInt(VB6.TwipsToPixelsX(2000)), HorizontalAlignment.Left, -1)

            lvwPaymentDetails.Columns.Insert(kPayDetColHIndexTotalReserve - 1, kPayDetColHCodeTotalReserve, GetResString(kResDetailsTotalReserve), CInt(VB6.TwipsToPixelsX(1420)), HorizontalAlignment.Right, -1)

            '(RC) QBENZ001
            lvwPaymentDetails.Columns.Insert(kPayDetColHIndexRevisedReserve - 1, "ThisRevision", "This Revision", CInt(VB6.TwipsToPixelsX(1420)), HorizontalAlignment.Right, -1)

            lvwPaymentDetails.Columns.Insert(kPayDetColHIndexPaidToDate - 1, kPayDetColHCodePaidToDate, GetResString(kResDetailsPaidToDate), CInt(VB6.TwipsToPixelsX(1300)), HorizontalAlignment.Right, -1)

            lvwPaymentDetails.Columns.Insert(kPayDetColHIndexPaidToDateTax - 1, kPayDetColHCodePaidToDateTax, GetResString(kResDetailsPaidToDateTax), CInt(VB6.TwipsToPixelsX(1700)), HorizontalAlignment.Right, -1)
            ''Start(Saurabh Agrawal) Tech Spec QBENZCR004 Claim Recovery Reinsurance
            lvwPaymentDetails.Columns.Insert(kPayDetColHIndexReceivedToDate - 1, kPayDetColHCodeReceivedToDate, GetResString(kResDetailsReceivedToDate), CInt(VB6.TwipsToPixelsX(1700)), HorizontalAlignment.Right, -1)

            lvwPaymentDetails.Columns.Insert(kPayDetColHIndexReceivedToDateTax - 1, kPayDetColHCodeReceivedToDateTax, GetResString(kResDetailsReceivedToDateInclTax), CInt(VB6.TwipsToPixelsX(1700)), HorizontalAlignment.Right, -1)

            ''End(Saurabh Agrawal) Tech Spec QBENZCR004 Claim Recovery Reinsurance
            lvwPaymentDetails.Columns.Insert(kPayDetColHIndexCurrentReserve - 1, kPayDetColHCodeCurrentReserve, GetResString(kResDetailsCurrentReserve), CInt(VB6.TwipsToPixelsX(1600)), HorizontalAlignment.Right, -1)

            lvwPaymentDetails.Columns.Insert(kPayDetColHIndexThisPayment - 1, kPayDetColHCodeThisPayment, GetResString(kResDetailsThisPaymentIncludingTax), CInt(VB6.TwipsToPixelsX(2100)), HorizontalAlignment.Right, -1)

            lvwPaymentDetails.Columns.Insert(kPayDetColHIndexThisPaymentTax - 1, kPayDetColHCodeTax, GetResString(kResDetailsThisPaymentTax), CInt(VB6.TwipsToPixelsX(1750)), HorizontalAlignment.Right, -1)

            lvwPaymentDetails.Columns.Insert(kPayDetColHIndexThisReceiptInclTax - 1, kPayDetColHCodeThisReceipt, GetResString(kResDetailsThisReceipt), CInt(VB6.TwipsToPixelsX(1700)), HorizontalAlignment.Right, -1)
            lvwPaymentDetails.Columns.Insert(kPayDetColHIndexThisReceiptTax - 1, kPayDetColHCodeThisReceiptIncTax, GetResString(kResDetailsThisReceiptInclusiveTax), CInt(VB6.TwipsToPixelsX(1700)), HorizontalAlignment.Right, -1)

            lvwPaymentDetails.Columns.Insert(kPayDetColHIndexCostToClaim - 1, kPayDetColHCodeCostToClaim, GetResString(kResDetailsCostToClaim), CInt(VB6.TwipsToPixelsX(1400)), HorizontalAlignment.Right, -1)


            lvwPaymentDetails.Columns.Insert(kPayDetColHIndexIsExcess - 1, kPayDetColHCodeIsExcess, "", CInt(VB6.TwipsToPixelsX(0)), HorizontalAlignment.Right, -1)

            lvwPaymentDetails.Columns.Insert(kPayDetColHIndexIsHistory - 1, kPayDetColHCodeIsHistory, "", CInt(VB6.TwipsToPixelsX(0)), HorizontalAlignment.Right, -1)




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

        Const kMethodName As String = "SetupPaymentDetailsListView"

        Dim sReturn As String = ""

        Try



            ' always want to return a string

            sReturn = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=v_lItemId, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))




        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=SetupPaymentDetailsListView(), excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally


        End Try
        Return sReturn
    End Function

    Private Sub cboClaimPaymentTo_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboClaimPaymentTo.SelectionChangeCommitted
        SetupAvailablePayeeOptions()
    End Sub





    'Private Sub cboCountry_Change()
    '
    'End Sub

    Private Sub cboMediaType_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboMediaType.SelectedIndexChanged
        ProcessMediaTypeValidation()
    End Sub

    Private Sub cboSafeHarbour_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboSafeHarbour.SelectionChangeCommitted
        SetupSafeHarbour()
    End Sub

    Private Sub chkITDomiciled_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkITDomiciled.CheckStateChanged
        SetupInsuredTaxAdjustments()
    End Sub

    Private Sub chkPTDomiciled_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkPTDomiciled.CheckStateChanged
        SetupPayeeTaxAdjustments()
        m_bTaxWarningShown = False
    End Sub

    Private Sub chkTaxExempt_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkTaxExempt.CheckStateChanged
        If Not m_bAdvancedTaxScriptingOption Then 'ATS
            ActionExemptions(kTaxExempt)
        End If
    End Sub

    Private Sub chkWHTExempt_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkWHTExempt.CheckStateChanged
        If Not m_bAdvancedTaxScriptingOption Then 'ATS
            ActionExemptions(kWHTExempt)
        End If
    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click
        ActionDelete()
    End Sub


    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click
        m_bShowPaymentDetailsHiddenMode = False

        ActionEdit()
        'developer guide no.(For the focus to remain on the parent form)
        MyBase.ParentForm.Focus()
    End Sub

    Private Sub cmdHistory_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHistory.Click
        ActionHistory()
        'developer guide no.(For the focus to remain on the parent form)
        MyBase.ParentForm.Focus()
    End Sub

    Private Sub cmdParty_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdParty.Click
        ActionFindParty()
        'developer guide no.(For the focus to remain on the parent form)
        MyBase.ParentForm.Focus()
    End Sub

    Private Sub cmdEditPayee_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEditPayee.Click
        EnableDisablePayeeDetails(v_bEnabled:=True)
    End Sub

    Private Sub cmdPaymentLock_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPaymentLock.Click
        Actionpaymentlock()
    End Sub

    '(RC) QBENZ001
    Private Sub cmdReserveEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdReserveEdit.Click
        ActionReserveEdit()
        'developer guide no.(For the focus to remain on the parent form)
        MyBase.ParentForm.Focus()
    End Sub

    Private Function Actionpaymentlock() As Integer
        m_bPaymentLock = gPMConstants.PMEReturnCode.PMTrue
        If m_bCanChangeReserves And m_iTask <> gPMConstants.PMEComponentAction.PMView Then
            cmdReserveEdit.Enabled = True
        End If
        cmdEdit.Enabled = False
        cmdDelete.Enabled = False
        cmdPaymentLock.Enabled = False
    End Function
    ' ***************************************************************** '
    ' Name: ActionSelectPaymentItem
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 10-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function ActionSelectPaymentItem(ByRef r_oListItem As ListViewItem) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ActionSelectPaymentItem"

        Dim lReturn As Integer
        Dim bExcessSelected As Boolean

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            m_lSelectedPayeeDetailIndex = r_oListItem.Index + 1

            If m_bViewPaymentMode Then
                cmdDelete.Visible = False
                cmdEdit.Left = cmdHistory.Left

                '(RC) QBENZ001
                If m_bCanChangeReserves And m_iTask <> gPMConstants.PMEComponentAction.PMView And m_bPaymentLock Then
                    cmdReserveEdit.Enabled = True
                End If

                cmdEdit.Enabled = m_bViewPaymentMode
                cmdEdit.Text = "&View"
                cmdHistory.Visible = False
                cmdEdit.Width = VB6.TwipsToPixelsX(1000)
                cmdReserveEdit.Visible = False
                cmdPaymentLock.Visible = False
            Else

                ' if this isnt a spacer or a total row
                If r_oListItem.Text <> kLVWSpacer And r_oListItem.Text <> kLVWTotal Then
                    'ECK Oct 2005 Broking doesn't use  Transaction Type   "C_CP"
                    If m_sTransactionType = "C_CP" And m_bCanDoClaimPayment Then

                        cmdDelete.Visible = True

                        If gPMFunctions.ToSafeCurrency(ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsThisPaymentInclTax).Text, 0) <> 0 And Not m_bPaymentLock Then
                            cmdDelete.Enabled = True
                            'If RI2007Enabled = True Then
                            '    cmdPaymentLock.Enabled = False
                            'Else
                            cmdPaymentLock.Enabled = True
                            'End If
                        Else
                            cmdDelete.Enabled = False
                            cmdPaymentLock.Enabled = False
                        End If

                        ' if some payments have been made previously and this is not
                        ' the total row then enable the payment history button
                        cmdHistory.Enabled = StringsHelper.ToDoubleSafe(ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsIsHistory).Text) <> 0

                        ' disable edit and history functionality for recovery entries
                        If r_oListItem.Text = kTypeRecovery Or txtParty.Text = "" Then
                            cmdEdit.Enabled = False
                            cmdReserveEdit.Enabled = False '(RC) QBENZ001
                        Else
                            If Not m_bPaymentLock Then
                                cmdEdit.Enabled = True
                            End If

                            'PN 45918 (RC)
                            If m_bCanChangeReserves And m_iTask <> gPMConstants.PMEComponentAction.PMView And m_bPaymentLock Then
                                cmdReserveEdit.Enabled = True
                            End If

                        End If


                        ' if advanced tax scripting switched on
                        If m_bAdvancedTaxScriptingOption Then
                            ' is this an excess row that has been selected
                            bExcessSelected = CBool(ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsIsExcess).Text)
                            ' if this isnt an excess row and all the excess rows have not yet been set
                            ' only history functionality should be enabled.
                        End If

                    Else

                        ' if this is maintain or open claim
                        ' or the claim has referred payments then
                        'Start Renuka PN 61509
                        cmdEdit.Enabled = False
                        'End Renuka PN 61509

                        cmdDelete.Visible = False
                        cmdReserveEdit.Enabled = False '(RC) QBENZ001
                        cmdPaymentLock.Enabled = False
                        ' if some payments have been made previously and this is not
                        ' the total row or a recovery row then enable the payment history button

                        ''Start(Saurabh Agrawal) Tech Spec QBENZCR004 Claim Recovery Reinsurance
                        ''Check For RI 2007 if Enabled then Enable the History Button for Recovery row also Else
                        ''use the existing functionality.

                        If m_bRI2007Enabled Then
                            cmdHistory.Enabled = StringsHelper.ToDoubleSafe(ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsIsHistory).Text) <> 0

                        Else
                            ''End (Saurabh Agrawal) Tech Spec QBENZCR004 Claim Recovery Reinsurance
                            cmdHistory.Enabled = StringsHelper.ToDoubleSafe(ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsIsHistory).Text) <> 0 And r_oListItem.Text <> kTypeRecovery
                        End If

                    End If

                    If gPMFunctions.ToSafeCurrency(ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsThisPaymentInclTax).Text, 0) <> 0 And Not m_bPaymentLock Then
                        cmdDelete.Enabled = True
                        'If RI2007Enabled = True Then
                        '    cmdPaymentLock.Enabled = False
                        'Else
                        cmdPaymentLock.Enabled = True
                        'End If
                    Else
                        cmdDelete.Enabled = False
                        cmdPaymentLock.Enabled = False
                    End If

                Else

                    ' total row or spacer selected
                    ' so disable all list view functionality
                    cmdReserveEdit.Enabled = False '(RC) QBENZ001
                    cmdEdit.Enabled = False
                    cmdDelete.Enabled = False
                    cmdHistory.Enabled = False

                End If

                If m_bOpenClaimNoTrans And m_sTransactionType = "C_CO" And r_oListItem.Text <> kLVWSpacer And r_oListItem.Text <> kLVWTotal And Not m_bPaymentLock Then
                    cmdEdit.Visible = True
                    cmdEdit.Enabled = True

                    'PN 45918 (RC)
                    If m_bCanChangeReserves And m_iTask <> gPMConstants.PMEComponentAction.PMView And m_bPaymentLock Then
                        cmdReserveEdit.Visible = True
                        cmdReserveEdit.Enabled = True
                    End If

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
    ' Name: SetupAvailablePayeeOptions
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 10-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function SetupAvailablePayeeOptions() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupAvailablePayeeOptions"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lClaimPaymentToId, llBound, lUBound As Integer
        Dim bClaimPayableEnabled, bPartyEnabled, bAgentEnabled, bClientEnabled As Boolean

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If cboClaimPaymentTo.SelectedIndex <> -1 Then


                ' get the selected claim payment to option
                lClaimPaymentToId = VB6.GetItemData(cboClaimPaymentTo, cboClaimPaymentTo.SelectedIndex)

                ' get array boundaries
                llBound = m_vClaimPaymentTo.GetLowerBound(1)
                lUBound = m_vClaimPaymentTo.GetUpperBound(1)

                ' for each claim payment to option
                For lItem As Integer = llBound To lUBound

                    ' if the option matches the selected one
                    If CDbl(m_vClaimPaymentTo(kLookupItemId, lItem)) = lClaimPaymentToId Then

                        ' get the payee option details

                        m_sClaimPaymentToCode = m_vClaimPaymentTo(kLookupCode, lItem)
                        bClaimPayableEnabled = CBool(m_vClaimPaymentTo(kLookupClaimPaymentToClaimPayable, lItem))
                        bAgentEnabled = CBool(m_vClaimPaymentTo(kLookupClaimPaymentToAgent, lItem))
                        bPartyEnabled = CBool(m_vClaimPaymentTo(kLookupClaimPaymentToParty, lItem))
                        bClientEnabled = CBool(m_vClaimPaymentTo(kLookupClaimPaymentToClient, lItem))

                        Exit For
                    End If

                Next

                ' reset the selection
                OptClaimPayable.Checked = False
                OptParty.Checked = False
                OptAgent.Checked = False
                OptClient.Checked = False

                ' enable / disable the payee options
                OptClaimPayable.Enabled = bClaimPayableEnabled
                OptParty.Enabled = bPartyEnabled
                OptAgent.Enabled = bAgentEnabled
                OptClient.Enabled = bClientEnabled

                'ATS
                ' reset payee tax details when a new payee is selected
                chkPTDomiciled.CheckState = CheckState.Unchecked
                txtPTPercentage.Text = ""
                txtPTTaxNo.Text = ""

                'Reset Insured tax details when a new payee is selected
                chkITDomiciled.CheckState = CheckState.Unchecked
                txtITPercentage.Text = ""
                txtITTaxNo.Text = ""

                txtParty.Text = ""
                'Reset Safe harbour Group, Exemption group and Settlement group
                cboSafeHarbour.SelectedIndex = -1
                txtSFPercentage.Text = ""

                chkTaxExempt.CheckState = CheckState.Unchecked
                chkWHTExempt.CheckState = CheckState.Unchecked
                chkSettlement.CheckState = CheckState.Unchecked
                If Not (lvwPaymentDetails.FocusedItem Is Nothing) Then
                    lReturn = CType(ActionSelectPaymentItem(lvwPaymentDetails.FocusedItem), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "SetupAvailablePayeeOptions Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If
                'ATS

            End If

            ' if the lead agent is not set this is direct business
            ' and as such the agent should not be available
            If m_lLeadAgentId = 0 Then
                OptAgent.Enabled = False
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
    ''' SetupTaxAdjustmentFrames
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>Created : MEvans : 10-08-2005 : 360 - Taxes on Claims</remarks>
    Private Function SetupTaxAdjustmentFrames() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupTaxAdjustmentFrames"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If m_bAdvancedTaxScriptingOption Then 'ATS


                'developer guide no.248
                Select Case Convert.ToString(m_sClaimPaymentToCode).Trim().ToUpper()
                    Case "INSURED"
                        fraInsuredTaxAdjustment.Enabled = True
                        fraPayeeTaxAdjustments.Enabled = False
                    Case "SUPPLIER", "EXPENSES"
                        fraInsuredTaxAdjustment.Enabled = False
                        fraPayeeTaxAdjustments.Enabled = True
                    Case "3RDPARTY", "3PARTY"
                        fraInsuredTaxAdjustment.Enabled = Not OptClient.Checked

                        fraPayeeTaxAdjustments.Enabled = Not OptClaimPayable.Checked

                End Select

            Else

                If OptAgent.Checked Then

                    fraPayeeTaxAdjustments.Enabled = True
                    fraInsuredTaxAdjustment.Enabled = True

                ElseIf OptClaimPayable.Checked Or OptParty.Checked Or OptClient.Checked Then

                    fraPayeeTaxAdjustments.Enabled = True
                    fraInsuredTaxAdjustment.Enabled = False

                Else

                    fraPayeeTaxAdjustments.Enabled = False
                    fraInsuredTaxAdjustment.Enabled = False

                End If
            End If

            lReturn = SetupPayeeTaxAdjustments()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetupPayeeTaxAdjustments Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            lReturn = SetupInsuredTaxAdjustments()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetupInsuredTaxAdjustments Failed", gPMConstants.PMELogLevel.PMLogError)
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
    ''' SetupInsuredTaxAdjustments
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>MEvans : 10-08-2005 : 360 - Taxes on Claims</remarks>
    Private Function SetupInsuredTaxAdjustments() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupInsuredTaxAdjustments"

        Dim lReturn As Integer
        Dim bEnabled As Boolean

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            bEnabled = (chkITDomiciled.CheckState = CheckState.Checked)

            If m_bAdvancedTaxScriptingOption Then 'ATS

                'developer guide no.248
                Select Case Convert.ToString(m_sClaimPaymentToCode).Trim().ToUpper()
                    Case "INSURED", "3RDPARTY", "3PARTY"
                        lblITTaxNo.Font = VB6.FontChangeBold(lblITTaxNo.Font, False)
                    Case Else
                        lblITTaxNo.Font = VB6.FontChangeBold(lblITTaxNo.Font, bEnabled)
                End Select
            Else
                lblITTaxNo.Font = VB6.FontChangeBold(lblITTaxNo.Font, bEnabled)
            End If

            txtITPercentage.Enabled = bEnabled
            txtITTaxNo.Enabled = bEnabled

            lblITPercentage.Font = VB6.FontChangeBold(lblITPercentage.Font, bEnabled)

            If bEnabled Then
                lblITPercentage.Left = VB6.TwipsToPixelsX(1440)
                lblITTaxNo.Left = VB6.TwipsToPixelsX(3450)
            Else
                lblITPercentage.Left = VB6.TwipsToPixelsX(1560)
                lblITTaxNo.Left = VB6.TwipsToPixelsX(3480)
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
    ' Name: SetupPayeeTaxAdjustments
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 10-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function SetupPayeeTaxAdjustments() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupPayeeTaxAdjustments"

        Dim lReturn As Integer
        Dim bEnabled As Boolean

        Try



            result = gPMConstants.PMEReturnCode.PMTrue
            If m_lClaimPayableAccountId <> 0 And m_sTransactionType = "" Then
                m_bSelectedPayeeDomiciledForTax = True
            Else
                m_bSelectedPayeeDomiciledForTax = (chkPTDomiciled.CheckState = CheckState.Checked)
            End If


            bEnabled = (chkPTDomiciled.CheckState = CheckState.Checked)

            If m_bAdvancedTaxScriptingOption Then 'ATS

                'developer guide no.248
                Select Case Convert.ToString(m_sClaimPaymentToCode).Trim().ToUpper()
                    Case "SUPPLIER", "EXPENSES"
                        lblPTTaxNo.Font = VB6.FontChangeBold(lblPTTaxNo.Font, False)
                        txtPTPercentage.Enabled = bEnabled
                        lblPTPercentage.Font = VB6.FontChangeBold(lblPTPercentage.Font, bEnabled)
                    Case "INSURED"
                        lblPTTaxNo.Font = VB6.FontChangeBold(lblPTTaxNo.Font, bEnabled)
                        txtPTPercentage.Enabled = bEnabled
                        lblPTPercentage.Font = VB6.FontChangeBold(lblPTPercentage.Font, bEnabled)
                    Case "3RDPARTY", "3PARTY"
                        lblPTTaxNo.Font = VB6.FontChangeBold(lblPTTaxNo.Font, False)
                        txtPTPercentage.Enabled = False
                        lblPTPercentage.Font = VB6.FontChangeBold(lblPTPercentage.Font, False)
                End Select
            Else
                lblPTTaxNo.Font = VB6.FontChangeBold(lblPTTaxNo.Font, bEnabled)
                lblPTPercentage.Font = VB6.FontChangeBold(lblPTPercentage.Font, bEnabled)
                txtPTPercentage.Enabled = bEnabled
            End If

            txtPTTaxNo.Enabled = bEnabled

            If bEnabled Then
                lblPTPercentage.Left = VB6.TwipsToPixelsX(1440)
                lblPTTaxNo.Left = VB6.TwipsToPixelsX(3450)
            Else
                lblPTPercentage.Left = VB6.TwipsToPixelsX(1560)
                lblPTTaxNo.Left = VB6.TwipsToPixelsX(3480)
            End If

            If Not bEnabled Then
                txtPTPercentage.Text = ""
                txtPTTaxNo.Text = ""
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
    ' Name: ActionExemptions
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 10-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function ActionExemptions(ByVal v_lOption As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ActionExemptions"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If v_lOption = kWHTExempt Then
                If chkWHTExempt.CheckState = CheckState.Checked Then
                    chkTaxExempt.CheckState = CheckState.Unchecked
                End If
            End If

            If v_lOption = kTaxExempt Then
                If chkTaxExempt.CheckState = CheckState.Checked Then
                    chkWHTExempt.CheckState = CheckState.Unchecked
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
    ' Name: SetupSafeHarbour
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 10-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function SetupSafeHarbour() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupSafeHarbour"

        Dim lReturn, lSafeHarbourId As Integer
        Dim crPercentage As Decimal
        Dim llBound, lUBound As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get the selected claim payment to option
            If cboSafeHarbour.SelectedIndex > -1 Then
                lSafeHarbourId = VB6.GetItemData(cboSafeHarbour, cboSafeHarbour.SelectedIndex)
            End If

            ' get array boundaries
            llBound = m_vSafeHarbour.GetLowerBound(1)
            lUBound = m_vSafeHarbour.GetUpperBound(1)


            m_sSafeHarbourCode = ""
            crPercentage = 0
            ' for each claim payment to option
            For lItem As Integer = llBound To lUBound

                ' if the option matches the selected one
                If CDbl(m_vSafeHarbour(kLookupItemId, lItem)) = lSafeHarbourId Then

                    ' get the payee option details

                    m_sSafeHarbourCode = m_vSafeHarbour(kLookupCode, lItem)
                    crPercentage = gPMFunctions.ToSafeCurrency(m_vSafeHarbour(kLookupSafeHarbourPercentage, lItem), 0)

                    Exit For
                End If

            Next

            ' enable / disable the payee options
            txtSFPercentage.Text = StringsHelper.Format(crPercentage, "0.00")



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


        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' if we havent got an instance of find party
            If m_oFindParty Is Nothing Then

                ' get a new instance of find party
                Dim temp_m_oFindParty As Object
                lReturn = m_oObjectManager.GetInstance(temp_m_oFindParty, sClassName:="iPMBFindParty.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oFindParty = temp_m_oFindParty

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed to GetInstnace of iPMBFindParty.Interface", gPMConstants.PMELogLevel.PMLogError)
                End If

            End If

            ' Set component properties and start interface

            m_oFindParty.CallingAppName = ACApp

            m_oFindParty.IgnoreDriversAndWitnesses = True

            m_oFindParty.NotEditable = gPMConstants.PMEReturnCode.PMTrue

            m_oFindParty.SpecialParty = "OT"

            Dim vKeyArray(1, 0) As Object

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = "keep_window_on_top"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = 1


            m_lReturn = m_oFindParty.SetKeys(vKeyArray)

            ' start the find part component

            lReturn = m_oFindParty.Start()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "iPMBFindParty.Interface.Start Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get the returned partycnt

            If m_oFindParty.PartyCnt > 0 Then

                txtParty.Text = m_oFindParty.LongName

                m_lOtherPartyCnt = m_oFindParty.PartyCnt

                m_lSelectedPayeeId = m_lOtherPartyCnt
                m_sSelectedPayee = txtParty.Text

                '        lReturn = GetOtherPartyCurrencyId
                '        If lReturn <> PMTrue Then
                '            RaiseError kMethodName, "GetOtherPartyCurrencyId Failed", PMLogError
                '        End If

                lReturn = GetOtherPartyDetails()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetOtherPartyDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Do your tidy up here. i.e. Terminate and set = Nothing object referenes
            If Not (m_oFindParty Is Nothing) Then

                m_oFindParty.Dispose()
                m_oFindParty = Nothing
            End If




        End Try
        Return result
    End Function

    ''' <summary>
    ''' ActionPayeeOption
    ''' </summary>
    ''' <param name="v_lPayeeOption"></param>
    ''' <returns></returns>
    ''' <remarks>Created : MEvans : 10-08-2005 : 360 - Taxes on Claims</remarks>
    Private Function ActionPayeeOption(ByVal v_lPayeeOption As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ActionPayeeOption"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim bNewPayeeSelected As Boolean

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            ' reset payee tax details when a new payee is selected
            chkPTDomiciled.CheckState = CheckState.Unchecked
            txtPTPercentage.Text = ""
            txtPTTaxNo.Text = ""

            'Reset Insured tax details when a new payee is selected
            'ATS
            chkITDomiciled.CheckState = CheckState.Unchecked
            txtITPercentage.Text = ""
            txtITTaxNo.Text = ""

            'Reset Safe harbour Group, Exemption group and Settlement group
            cboSafeHarbour.SelectedIndex = -1
            txtSFPercentage.Text = ""

            chkTaxExempt.CheckState = CheckState.Unchecked
            chkWHTExempt.CheckState = CheckState.Unchecked
            chkSettlement.CheckState = CheckState.Unchecked

            ' disable other party selection
            cmdParty.Enabled = False
            uctPartyBankCombo1.EnableCombo = Not (v_lPayeeOption = kPayeeOptClaimPayable)

            Select Case v_lPayeeOption
                Case kPayeeOptClaimPayable
                    If OptClaimPayable.Checked Then
                        txtParty.Text = kAccountCLMPAYABLE
                    End If

                    '******************************************************
                    'developer guide no.248
                    If Convert.ToString(m_sClaimPaymentToCode) <> "" Then
                        If m_bAdvancedTaxScriptingOption Then 'ATS
                            'developer guide no.248
                            If Convert.ToString(m_sClaimPaymentToCode).Trim().ToUpper() <> "SUPPLIER" And Convert.ToString(m_sClaimPaymentToCode).Trim().ToUpper() <> "EXPENSES" Then
                                chkITDomiciled.CheckState = CheckState.Checked
                                txtITPercentage.Text = StringsHelper.Format(m_crClientTaxPercentage, "0.00")
                                txtITTaxNo.Text = m_sClientTaxNumber
                                'developer guide no.248
                                If Convert.ToString(m_sClaimPaymentToCode).Trim().ToUpper() = "3RDPARTY" Then
                                    chkPTDomiciled.CheckState = CheckState.Checked
                                Else
                                    chkPTDomiciled.CheckState = CheckState.Unchecked
                                End If
                            Else
                                chkPTDomiciled.CheckState = CheckState.Checked
                            End If
                        Else
                            ' populate payee tax adjustments with client details
                            chkPTDomiciled.CheckState = CheckState.Checked
                        End If
                    End If

                    '******************************************************

                    ' get the claim payable curreny id
                    If m_lClaimPayableCurrencyId = 0 Then
                        lReturn = GetAccountDetailsByShortCode()
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "GetAccountDetailsByShortCode Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                    End If

                    m_lSelectedPayeeAccountCurrencyId = m_lClaimPayableCurrencyId
                    m_bSelectedPayeeDomiciledForTax = True
                    m_sSelectedPayee = "CLMPAYABLE"


                    ''********************************************************************
                    ' UNDER NO CIRCUMSTANCES ALLOW THIS TO BE SET IF THIS IS UNDERWRITING
                    ' BECAUSE IT BREAKS THE STATS POSTINGS
                    m_lSelectedPayeeId = 0
                '********************************************************************

                Case kPayeeOptParty
                    cmdParty.Enabled = True
                    m_sSelectedPayee = "PARTY"

                    If m_bAdvancedTaxScriptingOption Then 'ATS
                        'developer guide no.248
                        If Convert.ToString(m_sClaimPaymentToCode).Trim().ToUpper() <> "SUPPLIER" And Convert.ToString(m_sClaimPaymentToCode).Trim().ToUpper() <> "EXPENSES" Then
                            chkITDomiciled.CheckState = CheckState.Checked
                            txtITPercentage.Text = StringsHelper.Format(m_crClientTaxPercentage, "0.00")
                            txtITTaxNo.Text = m_sClientTaxNumber
                        End If
                        'developer guide no.248
                        If Convert.ToString(m_sClaimPaymentToCode).Trim().ToUpper() <> "INSURED" Then
                            chkPTDomiciled.CheckState = CheckState.Checked
                        End If
                    End If

                Case kPayeeOptAgent
                    If m_bAgentInTransfer Then

                        ' if the agent isnt being transferred onto direct business
                        If m_sAgentTransferBusinessType <> "DIRECT" Then

                            ' use the transfer agents details
                            txtParty.Text = m_sTransferAgentName

                            '******************************************************
                            ' populate payee tax adjustments with agent details
                            txtPTPercentage.Text = StringsHelper.Format(m_crTransferAgentTaxPercentage, "0.00")
                            txtPTTaxNo.Text = m_sTransferAgentTaxNumber
                            chkTaxExempt.CheckState = m_bTransferAgentTaxExempt

                            If m_bTransferAgentDomiciledForTax Then
                                chkPTDomiciled.CheckState = CheckState.Checked
                            Else
                                chkPTDomiciled.CheckState = CheckState.Unchecked
                            End If

                            If m_bTransferAgentTaxExempt Then
                                chkTaxExempt.CheckState = CheckState.Checked
                            Else
                                chkTaxExempt.CheckState = CheckState.Unchecked
                            End If
                            '******************************************************

                            '******************************************************
                            ' populate insured tax adjustments with client details
                            If m_bClientDomiciledForTax Then
                                chkITDomiciled.CheckState = CheckState.Checked
                            Else
                                chkITDomiciled.CheckState = CheckState.Unchecked
                            End If

                            txtITPercentage.Text = StringsHelper.Format(m_crClientTaxPercentage, "0.00")
                            txtITTaxNo.Text = m_sClientTaxNumber
                            '******************************************************

                            m_lSelectedPayeeAccountCurrencyId = m_lTransferAgentCurrencyId
                            m_sSelectedPayee = "AGENT"
                            m_lSelectedPayeeId = m_lTransferAgentPartyCnt

                        End If

                    Else

                        txtParty.Text = m_sLeadAgentName

                        If Not m_bAdvancedTaxScriptingOption Then 'ATS
                            '******************************************************
                            ' populate payee tax adjustments with agent details
                            txtPTPercentage.Text = StringsHelper.Format(m_crLeadAgentTaxPercentage, "0.00")
                            txtPTTaxNo.Text = m_sLeadAgentTaxNumber
                            chkTaxExempt.CheckState = m_bLeadAgentTaxExempt
                        End If


                        If m_bLeadAgentDomiciledForTax Then
                            chkPTDomiciled.CheckState = CheckState.Checked
                        Else
                            chkPTDomiciled.CheckState = CheckState.Unchecked
                        End If

                        If m_bLeadAgentTaxExempt Then
                            chkTaxExempt.CheckState = CheckState.Checked
                        Else
                            chkTaxExempt.CheckState = CheckState.Unchecked
                        End If
                        '******************************************************

                        '******************************************************
                        ' populate insured tax adjustments with client details
                        If m_bClientDomiciledForTax Then
                            chkITDomiciled.CheckState = CheckState.Checked
                        Else
                            chkITDomiciled.CheckState = CheckState.Unchecked
                        End If

                        txtITPercentage.Text = StringsHelper.Format(m_crClientTaxPercentage, "0.00")
                        txtITTaxNo.Text = m_sClientTaxNumber
                        '******************************************************

                        m_lSelectedPayeeAccountCurrencyId = m_lLeadAgentCurrencyId
                        m_sSelectedPayee = "AGENT"
                        m_lSelectedPayeeId = m_lLeadAgentId

                    End If

                Case kPayeeOptClient

                    txtParty.Text = m_sClientName


                    m_lSelectedPayeeAccountCurrencyId = m_lClientCurrencyId
                    m_bSelectedPayeeDomiciledForTax = m_bClientDomiciledForTax
                    m_sSelectedPayee = "CLIENT"
                    m_lSelectedPayeeId = m_lClientId

                    If m_bAdvancedTaxScriptingOption Then 'ATS
                        'developer guide no.248
                        If Convert.ToString(m_sClaimPaymentToCode).Trim().ToUpper() <> "SUPPLIER" Then
                            chkITDomiciled.CheckState = CheckState.Checked
                            txtITPercentage.Text = StringsHelper.Format(m_crClientTaxPercentage, "0.00")
                            txtITTaxNo.Text = m_sClientTaxNumber

                            If m_bClientTaxExempt Then
                                chkTaxExempt.CheckState = CheckState.Checked
                            Else
                                chkTaxExempt.CheckState = CheckState.Unchecked
                            End If
                        End If
                    Else
                        '******************************************************
                        ' populate payee tax adjustments with client details
                        txtPTPercentage.Text = StringsHelper.Format(m_crClientTaxPercentage, "0.00")
                        txtPTTaxNo.Text = m_sClientTaxNumber

                        If m_bClientDomiciledForTax Then
                            chkPTDomiciled.CheckState = CheckState.Checked
                        Else
                            chkPTDomiciled.CheckState = CheckState.Unchecked
                        End If

                        If m_bClientTaxExempt Then
                            chkTaxExempt.CheckState = CheckState.Checked
                        Else
                            chkTaxExempt.CheckState = CheckState.Unchecked
                        End If
                        '******************************************************
                    End If

                Case Else
                    txtParty.Text = ""

            End Select

            ' setup tax adjustments frames
            lReturn = SetupTaxAdjustmentFrames()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetupTaxAdjustmentFrames Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Reset tax details when frame is disabled
            If m_bAdvancedTaxScriptingOption Then 'ATS
                If Not fraInsuredTaxAdjustment.Enabled Then
                    chkITDomiciled.CheckState = CheckState.Unchecked
                    txtITPercentage.Text = ""
                    txtITTaxNo.Text = ""

                End If
                If Not fraPayeeTaxAdjustments.Enabled Then
                    chkPTDomiciled.CheckState = CheckState.Unchecked
                    txtPTPercentage.Text = ""
                    txtPTTaxNo.Text = ""
                End If
            End If

            If Not (lvwPaymentDetails.FocusedItem Is Nothing) Then
                lReturn = CType(ActionSelectPaymentItem(lvwPaymentDetails.FocusedItem), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "ActionSelectPaymentItem Failed", gPMConstants.PMELogLevel.PMLogError)
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
    ' Name: SetupDefaultClaimPaymentOptions
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 11-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function SetupDefaultClaimPaymentOptions() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupDefaultClaimPaymentOptions"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lPayeeOption As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' set up default claim payment options - system option value

            Select Case m_lDefaultClaimPaymentOption
                Case kDefaultClaimPaymentOptionSuspense
                    lPayeeOption = kPayeeOptClaimPayable

                Case kDefaultClaimPaymentOptionThirdParty
                    lPayeeOption = kPayeeOptParty

                Case kDefaultClaimPaymentOptionUsersChoice
                    lPayeeOption = kPayeeOptNone

                Case kDefaultClaimPaymentOptionClient
                    lPayeeOption = kPayeeOptClient

                Case Else
                    lPayeeOption = kPayeeOptNone
            End Select

            m_lPrevClaimPayeeOption = 0
            ' perform any actions associated with the specified selections
            lReturn = CType(ActionPayeeOption(lPayeeOption), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "ActionPayeeOption failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lPrevClaimPayeeOption = lPayeeOption




        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    Private isInitializingComponent As Boolean
    Private Sub OptAgent_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles OptAgent.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            ActionPayeeOption(kPayeeOptAgent)
            m_lPrevClaimPayeeOption = kPayeeOptAgent
        End If
    End Sub

    Private Sub OptClaimPayable_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles OptClaimPayable.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            ActionPayeeOption(kPayeeOptClaimPayable)
            m_lPrevClaimPayeeOption = kPayeeOptClaimPayable
        End If
    End Sub

    Private Sub OptClient_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles OptClient.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            ActionPayeeOption(kPayeeOptClient)
            m_lPrevClaimPayeeOption = kPayeeOptClient
        End If
    End Sub

    Private Sub OptParty_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles OptParty.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            If Not IsPaymentsReadOnly Then
                txtParty.Text = ""
            End If
            ActionPayeeOption(kPayeeOptParty)
            m_lPrevClaimPayeeOption = kPayeeOptParty
        End If
    End Sub

    ' ***************************************************************** '
    ' Name: SetupPayeeInterfaceDefaults
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 11-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function SetupPayeeInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupPayeeInterfaceDefaults"

        Dim lReturn As Integer
        Dim bEnabled As Boolean

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' if there are "claim payment to" entries then
            ' enable the "claim payment to" combo and disable
            ' all payee options until a claim payment to
            ' entry is selected
            bEnabled = Information.IsArray(m_vClaimPaymentTo)

            ' if there are claim payment to entries
            If bEnabled Then

                ' if the advanced tax scripting option is switched
                ' on the display the claim payment to option
                ' otherwise just display the options
                ' for user selection
                bEnabled = m_bAdvancedTaxScriptingOption

            End If

            ' if in view payment mode and no claim payment option was
            ' previously selected then hide it...
            If bEnabled Then
                If m_bViewPaymentMode And m_lPaymentPartyTo Then
                    bEnabled = False
                End If
            End If

            ' if the claimpaymentto combo is not to be displayed
            ' then only display the options
            If Not bEnabled Then
                OptClaimPayable.Left = VB6.TwipsToPixelsX(190)
                OptParty.Left = VB6.TwipsToPixelsX(2525)
                OptAgent.Left = VB6.TwipsToPixelsX(4375)
                OptClient.Left = VB6.TwipsToPixelsX(5815)
                chkIsExGratia.Left = VB6.TwipsToPixelsX(7540)

            Else
                OptClaimPayable.Left = VB6.TwipsToPixelsX(4000)
                OptParty.Left = VB6.TwipsToPixelsX(5440)
                OptAgent.Left = VB6.TwipsToPixelsX(6360)
                OptClient.Left = VB6.TwipsToPixelsX(7150)
                chkIsExGratia.Left = VB6.TwipsToPixelsX(8080)

            End If

            cboClaimPaymentTo.Visible = bEnabled
            txtClaimPaymentTo.Visible = bEnabled

            OptClaimPayable.Enabled = Not bEnabled
            OptParty.Enabled = Not bEnabled
            OptAgent.Enabled = Not bEnabled
            OptClient.Enabled = Not bEnabled

            ' if there is no agent on the policy then
            ' the agent option is not an available choice
            If m_lLeadAgentId <> 0 Then
                'PN 33316
                ' if the lead agent is in transfer mode then new Agent will take precedence
                OptAgent.Enabled = True
                If m_bAgentInTransfer Then
                    If m_sAgentTransferBusinessType.Trim() = "DIRECT" Then
                        OptAgent.Enabled = False
                    ElseIf m_bTransferAgentDateCancelled Then
                        OptAgent.Text = "Agent Cancelled"
                        If m_bProductPreventPaymentsToCancelledAgents Then
                            OptAgent.Enabled = False
                        End If
                    End If
                Else
                    If m_bLeadAgentDateCancelled Then
                        OptAgent.Text = "Agent Cancelled"
                        If m_bProductPreventPaymentsToCancelledAgents Then
                            OptAgent.Enabled = False
                        End If
                    End If
                End If
            Else
                OptAgent.Enabled = False
            End If

            'ATS
            If m_bAdvancedTaxScriptingOption Then
                OptAgent.Enabled = False
            End If
            lReturn = CType(iPMFunc.GetSystemOption(v_iOptionNumber:=kSysOptionPaymentPExGratiaAccount, r_sOptionValue:=sExGratiaAccount, v_iSourceID:=m_iSourceID), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetupPayeeInterface Defaults Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            If sExGratiaAccount <> "" Then
                chkIsExGratia.Enabled = True
            Else
                chkIsExGratia.Enabled = False
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

            m_lOtherPartyCurrencyId = 0
            m_bOtherPartyDomiciledForTax = False
            m_sOtherPartyTaxNumber = ""
            m_dOtherPartyTaxPercentage = 0

            ' get the specified other party account details

            lReturn = m_oBusiness.GetOtherPartyDetails(v_lPartyCnt:=m_lOtherPartyCnt, r_vResults:=vAccountDetails)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetOtherPartyAccountDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Information.IsArray(vAccountDetails) Then

                ' get the other parties account details

                m_lOtherPartyCurrencyId = CInt(vAccountDetails(0, 0))
                m_bOtherPartyDomiciledForTax = gPMFunctions.ToSafeBoolean(vAccountDetails(1, 0), False)

                m_sOtherPartyTaxNumber = CStr(vAccountDetails(2, 0))
                m_dOtherPartyTaxPercentage = gPMFunctions.ToSafeDouble(vAccountDetails(3, 0), 0)

                ' set selected payee fields
                m_bSelectedPayeeDomiciledForTax = m_bOtherPartyDomiciledForTax
                m_lSelectedPayeeAccountCurrencyId = m_lOtherPartyCurrencyId

            End If

            ' set party tax details back to the interface
            If m_bAdvancedTaxScriptingOption Then 'ATS
                If fraPayeeTaxAdjustments.Enabled Then
                    txtPTPercentage.Text = StringsHelper.Format(m_dOtherPartyTaxPercentage, "0.00")
                    txtPTTaxNo.Text = m_sOtherPartyTaxNumber
                    chkPTDomiciled.CheckState = CheckState.Checked
                    m_bSelectedPayeeDomiciledForTax = True
                End If
            Else
                txtPTPercentage.Text = StringsHelper.Format(m_dOtherPartyTaxPercentage, "0.00")
                txtPTTaxNo.Text = m_sOtherPartyTaxNumber

                If m_bOtherPartyDomiciledForTax Then
                    chkPTDomiciled.CheckState = CheckState.Checked
                    m_bSelectedPayeeDomiciledForTax = True
                Else
                    chkPTDomiciled.CheckState = CheckState.Unchecked
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


            lReturn = m_oBusiness.GetAccountDetailsByShortCode(v_sShortCode:=kAccountCLMPAYABLE, r_vResults:=vAccountDetails)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetAccountDetailsByShortCode", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Information.IsArray(vAccountDetails) Then

                m_lClaimPayableCurrencyId = CInt(vAccountDetails(0, 0))

                m_lClaimPayableAccountId = CInt(vAccountDetails(1, 0))
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
    ' Name: SetupPaymentDetailsListView
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 09-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function SetupPaymentItemDetailsListView() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupPaymentDetailsListView"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            lvwPaymentDetails.Columns.Clear()

            lvwPaymentDetails.Columns.Add("", CInt(VB6.TwipsToPixelsX(0)), HorizontalAlignment.Left)

            lvwPaymentDetails.Columns.Insert(kPayDetItemColHIndexReserveId - 1, kPayDetItemColHCodeReserveId, "ReserveId", CInt(VB6.TwipsToPixelsX(0)), HorizontalAlignment.Left, -1)

            lvwPaymentDetails.Columns.Insert(kPayDetItemColHIndexReserveDesc - 1, kPayDetItemColHCodeReserveDesc, "", CInt(VB6.TwipsToPixelsX(2000)), HorizontalAlignment.Left, -1)

            lvwPaymentDetails.Columns.Insert(kPayDetItemColHIndexThisPayment - 1, kPayDetItemColHCodeThisPayment, GetResString(kResDetailsThisPayment), CInt(VB6.TwipsToPixelsX(1800)), HorizontalAlignment.Right, -1)

            lvwPaymentDetails.Columns.Insert(kPayDetItemColHIndexThisPaymentTax - 1, kPayDetItemColHCodeThisPaymentTax, GetResString(kResDetailsThisPaymentTax), CInt(VB6.TwipsToPixelsX(1800)), HorizontalAlignment.Right, -1)

            lvwPaymentDetails.Columns.Insert(kPayDetItemColHIndexTotal - 1, kPayDetItemColHCodeTotal, GetResString(kResDetailsTotal), CInt(VB6.TwipsToPixelsX(1800)), HorizontalAlignment.Right, -1)



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: PopulatePaymentItemDetailsListView
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 09-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function PopulatePaymentItemDetailsListView() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulatePaymentItemDetailsListView"

        Dim lReturn, lReserveID As Integer
        Dim sReserveDesc As String = ""
        Dim crThisPayment, crThisPaymentTax, crThisPaymentTaxWHT, crTotal, crExcess As Decimal
        Dim lCurrencyId As Integer
        Dim dCurrencyBaseXRate As Decimal
        Dim lTaxGroupId As Integer
        Dim crTotalReserve, crThisReserveRevision As Decimal '(RC) QBENZ001
        Dim crPaidToDate, crBalance As Decimal
        Dim llBound, lUBound As Integer
        Dim oListItem As ListViewItem
        Dim oPaymentItem As cPaymentItem
        Dim dPaymentToLossXRate As Double
        Dim sCurrencyDescription, sTaxGroupDescription As String
        Dim bIsWithHoldingTax As Boolean
        Dim sAdvancedTaxScript As String = ""
        Dim lWorkClaimPaymentItemId As CheckState

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lvwPaymentDetails.Items.Clear()

            ' determine array boundaries
            llBound = m_vClaimPaymentItemDetails.GetLowerBound(1)
            lUBound = m_vClaimPaymentItemDetails.GetUpperBound(1)

            ' for each reserve / recovery type
            For lItem As Integer = llBound To lUBound

                ' get reserve / recovery details
                lReserveID = gPMFunctions.ToSafeLong(m_vClaimPaymentItemDetails(kClaimPayItemDetReserveId, lItem), 0)
                sReserveDesc = CStr(m_vClaimPaymentItemDetails(kClaimPayItemDetReserveTypeDesc, lItem))
                crThisPayment = gPMFunctions.ToSafeCurrency(m_vClaimPaymentItemDetails(kClaimPayItemDetThisPayment, lItem), 0)
                crThisPaymentTax = gPMFunctions.ToSafeCurrency(m_vClaimPaymentItemDetails(kClaimPayItemDetTaxAmount, lItem), 0)
                crThisPaymentTaxWHT = gPMFunctions.ToSafeCurrency(m_vClaimPaymentItemDetails(kClaimPayItemDetTaxAmountWHT, lItem), 0)
                lCurrencyId = CInt(gPMFunctions.ToSafeCurrency(m_vClaimPaymentItemDetails(kClaimPayItemDetCurrencyId, lItem), 0))
                dCurrencyBaseXRate = gPMFunctions.ToSafeDouble(m_vClaimPaymentItemDetails(kClaimPayItemDetCurrencyBaseXRate, lItem), 0)
                lTaxGroupId = gPMFunctions.ToSafeLong(m_vClaimPaymentItemDetails(kClaimPayItemDetTaxGroupId, lItem), 0)
                crTotalReserve = gPMFunctions.ToSafeCurrency(m_vClaimPaymentItemDetails(kClaimPayItemDetTotalReserve, lItem), 0)
                crPaidToDate = gPMFunctions.ToSafeCurrency(m_vClaimPaymentItemDetails(kClaimPayItemDetPaidToDate, lItem), 0)
                crBalance = gPMFunctions.ToSafeCurrency(m_vClaimPaymentItemDetails(kClaimPayItemDetBalance, lItem), 0)
                dPaymentToLossXRate = gPMFunctions.ToSafeDouble(m_vClaimPaymentItemDetails(kClaimPayItemDetPaymentToLossXRate, lItem), 0)
                sCurrencyDescription = CStr(m_vClaimPaymentItemDetails(kClaimPayItemDetCurrencyDescription, lItem)).Trim()
                sTaxGroupDescription = CStr(m_vClaimPaymentItemDetails(kClaimPayItemDetTaxGroupDescription, lItem)).Trim()
                bIsWithHoldingTax = gPMFunctions.ToSafeBoolean(m_vClaimPaymentItemDetails(kClaimPayItemDetIsWithHoldingTax, lItem), False)
                sAdvancedTaxScript = CStr(m_vClaimPaymentItemDetails(kClaimPayItemDetAdvancedTaxScript, lItem)).Trim()
                lWorkClaimPaymentItemId = gPMFunctions.ToSafeLong(m_vClaimPaymentItemDetails(kClaimPayItemDetWorkClaimPaymentItemId, lItem))
                crThisReserveRevision = gPMFunctions.ToSafeCurrency(m_vClaimPaymentItemDetails(17, lItem), 0) '(RC) QBENZ001

                ' add list item
                oListItem = lvwPaymentDetails.Items.Add(CStr(lReserveID))

                ' populate list sub item details
                ListViewHelper.GetListViewSubItem(oListItem, kPayItemDetailsSubItemsReserveId).Text = CStr(lReserveID)
                ListViewHelper.GetListViewSubItem(oListItem, kPayItemDetailsSubItemsReserveDesc).Text = sReserveDesc
                ListViewHelper.GetListViewSubItem(oListItem, kPayItemDetailsSubItemsThisPayment).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, crThisPayment * dPaymentToLossXRate)
                ListViewHelper.GetListViewSubItem(oListItem, kPayItemDetailsSubItemsThisPaymentTax).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, (crThisPaymentTax + crThisPaymentTaxWHT) * dPaymentToLossXRate)

             '   If m_bClaimPaymentIsGross Then
               '     crTotal = (crThisPayment * dPaymentToLossXRate) '+ ((crThisPaymentTax + crThisPaymentTaxWHT) * dPaymentToLossXRate)
              '  Else
                    crTotal = (crThisPayment * dPaymentToLossXRate) + ((crThisPaymentTax + crThisPaymentTaxWHT) * dPaymentToLossXRate)
              '  End If

                ListViewHelper.GetListViewSubItem(oListItem, kPayItemDetailsSubItemsTotal).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, crTotal)

                'set up payment item and add it to the collection
                oPaymentItem = New cPaymentItem()

                oPaymentItem.CurrencyId = lCurrencyId
                oPaymentItem.PaymentToLossXRate = dPaymentToLossXRate
                oPaymentItem.ReserveId = lReserveID
                oPaymentItem.TaxAmount = crThisPaymentTax
                oPaymentItem.TaxAmountWHT = crThisPaymentTaxWHT
                oPaymentItem.TaxGroupId = lTaxGroupId
                oPaymentItem.ThisPayment = crThisPayment
                oPaymentItem.TaxGroupDescription = sTaxGroupDescription
                oPaymentItem.CurrencyDescription = sCurrencyDescription
                oPaymentItem.Balance = crBalance
                oPaymentItem.TotalReserve = crTotalReserve
                oPaymentItem.ThisReserveRevision = crThisReserveRevision '(RC) QBENZ001
                oPaymentItem.PaidToDate = crPaidToDate
                oPaymentItem.IsWithHoldingTax = bIsWithHoldingTax
                oPaymentItem.AdvancedTaxScript = sAdvancedTaxScript
                oPaymentItem.ReserveDesc = sReserveDesc
                oPaymentItem.WorkClaimPaymentId = lWorkClaimPaymentItemId

                If m_colPaymentItems Is Nothing Then
                    m_colPaymentItems = New Collection()
                End If
                m_colPaymentItems.Add(oPaymentItem, CStr(lReserveID))
                m_crTotalAmount += crTotal

            Next
            If lvwPaymentDetails.Items.Count > 0 Then
                lvwPaymentDetails.Items(0).Selected = True
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
    ' Name: GetPaymentItem
    '
    ' Parametseters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 12-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function GetPaymentItem(ByRef r_oPaymentItem As cPaymentItem) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPaymentItem"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim bIsExcess As Boolean
        Dim oListItem As ListViewItem
        Dim sReserveId, sReserveDesc As String

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' get the selected payment list item

            'oListItem = lvwPaymentDetails.Items.Item(m_lSelectedPayeeDetailIndex - 1)
            oListItem = lvwPaymentDetails.SelectedItems(0)


            ' get required details from the selected list item
            sReserveId = ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsTypeId).Text
            sReserveDesc = ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsTypeDesc).Text

            ' if this isnt payment mode then it is possible that
            ' the payment doesnt exist yet
            If Not m_bViewPaymentMode Then

                ' is this an excess row
                bIsExcess = CBool(ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsIsExcess).Text)

                If m_colPaymentItems.Count = 0 Then
                    ' add the payment item
                    lReturn = CType(AddPaymentItem(sReserveId), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "AddPaymentItem Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                Else

                    ' if the payment item doesnt exist yet
                    r_oPaymentItem = If(m_colPaymentItems.Contains(sReserveId), CType(m_colPaymentItems(sReserveId), uctCLMPaymentControl.cPaymentItem), Nothing)

                    If r_oPaymentItem Is Nothing Then
                        ' add the payment item
                        lReturn = CType(AddPaymentItem(sReserveId), gPMConstants.PMEReturnCode)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "AddPaymentItem Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                    End If
                End If

            End If

            ' get the payment item - it should now exist so just return it
            r_oPaymentItem = m_colPaymentItems.Item(sReserveId)

            ' save the is excess indicator
            r_oPaymentItem.IsExcess = bIsExcess

            ' save the reserve description to the payment item
            ' if it hasnt already been set
            If r_oPaymentItem.ReserveDesc = "" Then
                r_oPaymentItem.ReserveDesc = sReserveDesc
            End If


            'save Paid To Date
            If r_oPaymentItem.PaidToDate = 0 Then
                r_oPaymentItem.PaidToDate = CDec(ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsPaidToDate).Text)
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        End Try

        Return result
    End Function

    ' ***************************************************************** '
    ' Name: AddPaymentItem
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 12-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function AddPaymentItem(ByVal v_sReserveId As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AddPaymentItem"

        Dim lReturn As Integer
        Dim oPaymentItem As cPaymentItem

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' create new version of payment item
            oPaymentItem = New cPaymentItem()

            ' add new payment item to the collection
            m_colPaymentItems.Add(oPaymentItem, v_sReserveId)
            ' set default parameters
            If m_colPaymentItems.Count > 1 Then
                ' default the currency exchange rates and dates from any existing item...

                oPaymentItem.AccountToBaseDate = m_colPaymentItems.Item(1).AccountToBaseDate

                oPaymentItem.AccountToBaseXRate = m_colPaymentItems.Item(1).AccountToBaseXRate

                oPaymentItem.CurrencyToBaseDate = m_colPaymentItems.Item(1).CurrencyToBaseDate

                oPaymentItem.CurrencyToBaseXRate = m_colPaymentItems.Item(1).CurrencyToBaseXRate

                oPaymentItem.ExchangeRateOverrideReasonId = m_colPaymentItems.Item(1).ExchangeRateOverrideReasonId

                oPaymentItem.SystemToBaseDate = m_colPaymentItems.Item(1).SystemToBaseDate

                oPaymentItem.SystemToBaseXRate = m_colPaymentItems.Item(1).SystemToBaseXRate

                oPaymentItem.PaymentToLossXRate = m_colPaymentItems.Item(1).PaymentToLossXRate
            Else
                ' default the currency xchange rates and dates from the original item...
                oPaymentItem.AccountToBaseDate = DateTime.Today
                oPaymentItem.AccountToBaseXRate = 0
                oPaymentItem.CurrencyToBaseDate = DateTime.Today
                oPaymentItem.CurrencyToBaseXRate = 0
                oPaymentItem.ExchangeRateOverrideReasonId = 0
                oPaymentItem.SystemToBaseDate = DateTime.Today
                oPaymentItem.SystemToBaseXRate = 0
                oPaymentItem.PaymentToLossXRate = 0
            End If

            oPaymentItem.PostClaimTax = m_bPostClaimTax
            oPaymentItem.CurrencyId = 0
            oPaymentItem.ReserveId = gPMFunctions.ToSafeLong(v_sReserveId, 0)
            oPaymentItem.TaxAmount = 0
            oPaymentItem.TaxAmountWHT = 0
            oPaymentItem.TaxGroupId = -1
            oPaymentItem.ThisPayment = 0
            oPaymentItem.WorkClaimPaymentId = 0
            oPaymentItem.IsWithHoldingTax = False

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
        Const kCOL_CLAIMPERIL_DESC As Integer = 3
        Const kEVENT_TYPE_UPDATECLAIM As Integer = 6

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim bTransStarted As Boolean
        Dim sEventDesc As String = ""
        Dim vResultArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' if an unrecoverable error has occurred then
            If m_bUnrecoverableError Then
                gPMFunctions.RaiseError(kMethodName, "Save Failed - Unrecoverable Error occurred.", gPMConstants.PMELogLevel.PMLogError)
            Else

                If m_crTotalThisPayment <> 0 Then

                    ' start transaction

                    lReturn = m_oBusiness.BeginTrans
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "Begin Transaction Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    bTransStarted = True

                    ' save payment / payment items / tax calculations
                    lReturn = SaveClaimPayment()
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "SaveClaimPayment Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                    lReturn = ValidateClaimRIXOL()
                    If m_bClaimHasXOLLines = True Then
                        ' rollback transaction
                        lReturn = m_oBusiness.RollbackTrans
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "RollBack Transaction Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                        If MsgBox("You cannot add any further payments onto this claim as it has pending authorisations with Excess of Loss Reinsurance." & vbCrLf & vbCrLf & "The transaction will be changed to a Maintain Claim transaction if you continue?", vbOKCancel + vbExclamation, "Claim Payment") = vbOK Then
                            m_lReturn = m_oBusiness.UpdateClaimTransactionType(v_lClaimId:=m_lClaimId, v_sTransactionType:="C_CR")
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                RaiseError(kMethodName, "GetClaimXOLineCount Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If
                        Else
                            result = gPMConstants.PMEReturnCode.PMCancel
                        End If
                    Else
                        ' commit transaction

                        lReturn = m_oBusiness.CommitTrans
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "Commit Transaction Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                    End If

                ElseIf m_crTotalThisReserve <> 0 Then

                    '(RC) QBENZ001

                    For Each oPaymentItem As cPaymentItem In m_colPaymentItems

                        If oPaymentItem.ThisReserveRevision <> 0 Then

                            ' update the associated reserve with the payment details
                            lReturn = CType(UpdateReserve(oPaymentItem), gPMConstants.PMEReturnCode)
                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError(kMethodName, "Call to UpdateReserve Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If

                        End If

                    Next oPaymentItem

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

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SetupViewPaymentInterface
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 12-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function SetupViewPaymentInterface() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupViewPaymentInterface"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' if in view payment mode only show the text box versions
            ' as no further selection can be made from the cbo's
            ' and the historic values may no longer be available from the cbo's
            '    cboCountry.Visible = False
            cboMediaType.Visible = False
            cboSafeHarbour.Visible = False

            '    txtCountry.Visible = True
            txtMediaType.Visible = True
            txtSafeHarbour.Visible = True

            cmdEdit.Text = "&View"
            cmdEdit.Width = VB6.TwipsToPixelsX(1000)
            cmdReserveEdit.Visible = False 'PN 45918 (RC)
            cmdHistory.Visible = False
            cmdEditPayee.Visible = False
            cmdEdit.Left = cmdHistory.Left

            fraInsuredTaxAdjustment.Enabled = False
            fraPayeeTaxAdjustments.Enabled = False
            fraExemptions.Enabled = False
            fraSettlement.Enabled = False
            fraClaimInformation.Enabled = False
            fraPayee.Enabled = False
            fraSafeHarbour.Enabled = False

            fraThisPaymentComments.Enabled = False
            'fraThisPaymentDetails.Enabled = False
            txtMediaType.Enabled = False
            txtMediaRef.Enabled = False
            txtPayeeName.Enabled = False
            dtpChequeDate.Enabled = False
            txtBankAccountNo.Enabled = False
            txtBankCode.Enabled = False
            txtBankName.Enabled = False
            txtThirdPartyReference.Enabled = False
            txtBIC.Enabled = False
            txtIBAN.Enabled = False
            'Start (Renuka) PN 61323
            txtOurReference.Enabled = False
            'End (Renuka) PN 61323

            fraThisPaymentSummary.Enabled = False
            fraTaxesOnPayments.Enabled = False
            uctPMAddressControl1.Enabled = False



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SetupFormLayout
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function SetupFormLayout() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupFormLayout"

        Dim lReturn As Integer
        Dim bVisible As Boolean
        Dim lLayoutID As Integer

        Try


            m_bCanDoClaimPayment = True
            result = gPMConstants.PMEReturnCode.PMTrue

            If m_bClaimHasReferredPayments Then
                If m_bMultipleClaimPayments Then
                    If m_iMaxNoofUnAuthorisedClaimPayments = 0 Or m_iMaxNoofUnAuthorisedClaimPayments > m_iNoofReferredPayments Then
                        m_bCanDoClaimPayment = True
                    Else
                        MessageBox.Show("This payment exceeds the maximum unauthorised claim payment entries for your product", "Claim Payment", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        m_bCanDoClaimPayment = False
                    End If
                Else
                    m_bCanDoClaimPayment = False
                End If
            End If

            ' if in the process of making a payment or
            ' we are viewing a historic payment then display extra
            ' payment tax details
            If (m_sTransactionType = "C_CP" Or m_bViewPaymentMode) And m_bAdvancedTaxScriptingOption And m_bCanDoClaimPayment And m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                lLayoutID = 1

            ElseIf (m_sTransactionType = "C_CP" Or m_bViewPaymentMode) And Not m_bAdvancedTaxScriptingOption And m_bCanDoClaimPayment And m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                lLayoutID = 2

                ' if in open claim or maintain claim mode or the advanced tax scripting system
                ' option is turned off then dont display specific
                ' payment details until the user drills down using viewpaymentmode
            ElseIf m_sTransactionType = "C_CO" Or m_sTransactionType = "C_CR" Or Not m_bCanDoClaimPayment Or m_iTask = gPMConstants.PMEComponentAction.PMView Then
                lLayoutID = 3
            End If


            Select Case lLayoutID
                Case 1
                    ' ahow the payment specific frames
                    fraPayee.Visible = True
                    fraPayeeTaxAdjustments.Visible = True
                    fraSettlement.Visible = Not (m_bAdvancedTaxScriptingOption And Not m_bATSSattlement)
                    fraInsuredTaxAdjustment.Visible = True
                    fraSafeHarbour.Visible = Not (m_bAdvancedTaxScriptingOption And Not m_bPaymentATSSafeHarbour)
                    fraExemptions.Visible = True

                    fraPaymentDetails.Top = VB6.TwipsToPixelsY(3100)
                    fraPaymentDetails.Height = VB6.TwipsToPixelsY(3055)
                    lvwPaymentDetails.Height = VB6.TwipsToPixelsY(2335)
                    cmdEdit.Top = VB6.TwipsToPixelsY(2680)
                    cmdPaymentLock.Top = VB6.TwipsToPixelsY(2680) '(RK)
                    cmdReserveEdit.Top = VB6.TwipsToPixelsY(2680) '(RC) QBENZ001
                    cmdHistory.Top = VB6.TwipsToPixelsY(2680)
                    cmdEditPayee.Top = VB6.TwipsToPixelsY(2680)
                    cmdEditPayee.Visible = True
                    cmdDelete.Top = VB6.TwipsToPixelsY(2680)
                    cmdReserveEdit.Enabled = False '(RC) QBENZ001
                    cmdEdit.Enabled = False
                    cmdHistory.Enabled = False
                    cmdDelete.Enabled = False
                Case 2

                    ' dont show the advanced tax payment specific frames
                    ' but do show the payee.
                    fraPayee.Visible = True
                    fraPayeeTaxAdjustments.Visible = False
                    fraSettlement.Visible = False
                    fraInsuredTaxAdjustment.Visible = False
                    fraSafeHarbour.Visible = False
                    fraExemptions.Visible = False

                    fraPaymentDetails.Top = fraInsuredTaxAdjustment.Top
                    fraPaymentDetails.Height = VB6.TwipsToPixelsY(4700)
                    lvwPaymentDetails.Height = VB6.TwipsToPixelsY(4000)
                    cmdEdit.Top = VB6.TwipsToPixelsY(4300)
                    cmdReserveEdit.Top = VB6.TwipsToPixelsY(4300) '(RC) QBENZ001
                    cmdHistory.Top = VB6.TwipsToPixelsY(4300)
                    cmdDelete.Top = VB6.TwipsToPixelsY(4300)
                    cmdPaymentLock.Top = VB6.TwipsToPixelsY(4300)
                    cmdReserveEdit.Enabled = False '(RC) QBENZ001
                    cmdEdit.Enabled = False
                    cmdHistory.Enabled = False
                    cmdDelete.Enabled = False
                    cmdEditPayee.Visible = False
                Case 3
                    ' dont show the advanced tax specific frames
                    ' or the payee specific frames
                    fraPayee.Visible = False
                    fraPayeeTaxAdjustments.Visible = False
                    fraSettlement.Visible = False
                    fraInsuredTaxAdjustment.Visible = False
                    fraSafeHarbour.Visible = False
                    fraExemptions.Visible = False

                    fraPaymentDetails.Top = fraPayee.Top
                    fraPaymentDetails.Height = VB6.TwipsToPixelsY(4700 + 735)
                    lvwPaymentDetails.Height = VB6.TwipsToPixelsY(4000 + 735)
                    cmdEdit.Top = VB6.TwipsToPixelsY(4300 + 735)
                    cmdReserveEdit.Top = VB6.TwipsToPixelsY(4300 + 735) '(RC) QBENZ001
                    cmdHistory.Top = VB6.TwipsToPixelsY(4300 + 735)
                    cmdDelete.Top = VB6.TwipsToPixelsY(4300 + 735)
                    cmdEditPayee.Top = VB6.TwipsToPixelsY(4300 + 735)
                    cmdPaymentLock.Top = VB6.TwipsToPixelsY(4300 + 735)

                    'PN 45918 (RC)
                    If m_bCanChangeReserves And m_iTask <> gPMConstants.PMEComponentAction.PMView And m_bPaymentLock Then
                        cmdReserveEdit.Visible = True
                        cmdReserveEdit.Enabled = True
                    End If
                    cmdPaymentLock.Visible = False

                    cmdEdit.Visible = True
                    cmdDelete.Visible = False
                    cmdEditPayee.Visible = False

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
        Dim crTotalThisReserve, crTotalPaidToDate, crTotalPaidToDateTax, crTotalCurrentReserve, crTotalThisPaymentInclTax, crTotalThisPaymentTax, crTotalThisPaymentExcess, crTotalCostToClaim As Decimal
        'DC090606 Add Coinsurer Details for Datasure
        Dim cAmount, cPerc, cPerAllocated, cTotAllocated As Decimal
        Dim iRow As Integer
        Dim oListPayItem As ListViewItem
        Dim crThisReserveRevision As Decimal '(RC) QBENZ001
        Dim crTotalThisPaymentInclTaxABS As Decimal ' PN 45182

        ''Start(Saurabh Agrawal)Tech Spec QBENZCR004 Claims Recovery Reinsurance
        Dim crTotalReceivedToDate, crTotalReceivedToDateTax As Decimal
        ''End(Saurabh Agrawal)Tech Spec QBENZCR004 Claims Recovery Reinsurance

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' if there are list items
            If lvwPaymentDetails.Items.Count > 0 Then

                m_crTotalThisPaymentInclTax = 0
                m_crTotalThisPaymentInclTaxABS = 0

                ' then get totals for all reserve lines apart from total line
                ' so start from the second line i.e the first reserve line
                For lItem As Integer = kPaymentDetailsFirstReserveItemRow To lvwPaymentDetails.Items.Count

                    ' only include reserves in the totals
                    ' salvage and third party recovery values should not be included.
                    If lvwPaymentDetails.Items.Item(lItem - 1).Text = kTypeReserve Then

                        ' get totals from payment detail grid
                        crTotalThisReserve += gPMFunctions.ToSafeCurrency(ListViewHelper.GetListViewSubItem(lvwPaymentDetails.Items.Item(lItem - 1), kPayDetailsSubItemsTotalReserve).Text, 0)
                        crThisReserveRevision += gPMFunctions.ToSafeCurrency(ListViewHelper.GetListViewSubItem(lvwPaymentDetails.Items.Item(lItem - 1), kPayDetailsThisReserveRevision).Text, 0) '(RC) QBENZ001
                        crTotalPaidToDate += gPMFunctions.ToSafeCurrency(ListViewHelper.GetListViewSubItem(lvwPaymentDetails.Items.Item(lItem - 1), kPayDetailsSubItemsPaidToDate).Text, 0)
                        crTotalPaidToDateTax += gPMFunctions.ToSafeCurrency(ListViewHelper.GetListViewSubItem(lvwPaymentDetails.Items.Item(lItem - 1), kPayDetailsSubItemsPaidToDateTax).Text, 0)
                        crTotalCurrentReserve += gPMFunctions.ToSafeCurrency(ListViewHelper.GetListViewSubItem(lvwPaymentDetails.Items.Item(lItem - 1), kPayDetailsSubItemsCurrentReserve).Text, 0)
                        crTotalThisPaymentInclTax += gPMFunctions.ToSafeCurrency(ListViewHelper.GetListViewSubItem(lvwPaymentDetails.Items.Item(lItem - 1), kPayDetailsSubItemsThisPaymentInclTax).Text, 0)
                        crTotalThisPaymentInclTaxABS += Math.Abs(gPMFunctions.ToSafeCurrency(ListViewHelper.GetListViewSubItem(lvwPaymentDetails.Items.Item(lItem - 1), kPayDetailsSubItemsThisPaymentInclTax).Text, 0)) ' PN 45182
                        crTotalThisPaymentTax += gPMFunctions.ToSafeCurrency(ListViewHelper.GetListViewSubItem(lvwPaymentDetails.Items.Item(lItem - 1), kPayDetailsSubItemsThisPaymentTax).Text, 0)
                        crTotalCostToClaim += gPMFunctions.ToSafeCurrency(ListViewHelper.GetListViewSubItem(lvwPaymentDetails.Items.Item(lItem - 1), kPayDetailsSubItemsCostToClaim).Text, 0)

                    End If
                    If m_bRI2007Enabled And lvwPaymentDetails.Items.Item(lItem - 1).Text = kTypeRecovery Then
                        ''Start(Saurabh Agrawal)Tech Spec QBENZCR004 Claims Recovery Reinsurance
                        crTotalReceivedToDate += gPMFunctions.ToSafeCurrency(ListViewHelper.GetListViewSubItem(lvwPaymentDetails.Items.Item(lItem - 1), kPayDetailsSubItemsReceivedToDate).Text, 0)
                        crTotalReceivedToDateTax += gPMFunctions.ToSafeCurrency(ListViewHelper.GetListViewSubItem(lvwPaymentDetails.Items.Item(lItem - 1), kPayDetailsSubItemsReceivedToDateTax).Text, 0)
                        ''End(Saurabh Agrawal)Tech Spec QBENZCR004 Claims Recovery Reinsurance

                    End If

                Next

            End If

            ' store the total amount of this payment including tax
            m_crTotalThisPaymentInclTax = crTotalThisPaymentInclTax
            m_crTotalThisPaymentInclTaxABS = crTotalThisPaymentInclTaxABS 'PN 45182

            ' store the total amount of this payment
            If m_bOpenClaimNoTrans Then
                m_crTotalThisPayment = crTotalPaidToDate
            Else
                m_crTotalThisPayment = crTotalThisPaymentInclTax - crTotalThisPaymentTax
            End If
            ' PN 76954
            m_crTotalCostToClaim = crTotalCostToClaim
            m_crTotalPaidToDate = crTotalPaidToDate
            m_crTotalReceivedToDate = crTotalReceivedToDate

            '(RC) QBENZ001
            ' store the total amount of ThisReserve
            m_crTotalThisReserve = crTotalThisReserve

            ' get the total row
            oListItem = lvwPaymentDetails.Items.Item(0)

            ' save totals to total row
            ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsTotalReserve).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, crTotalThisReserve)
            'ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsTotalReserve).Text = StringsHelper.Format(crTotalThisReserve, "0.00")
            ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsThisReserveRevision).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, crThisReserveRevision)
            'ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsThisReserveRevision).Text = StringsHelper.Format(crThisReserveRevision, "0.00") '(RC) QBENZ001
            ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsPaidToDate).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, crTotalPaidToDate)
            'ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsPaidToDate).Text = StringsHelper.Format(crTotalPaidToDate, "0.00")
            ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsPaidToDateTax).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, crTotalPaidToDateTax)
            'ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsPaidToDateTax).Text = StringsHelper.Format(crTotalPaidToDateTax, "0.00")
            ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsCurrentReserve).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, crTotalCurrentReserve)
            'ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsCurrentReserve).Text = StringsHelper.Format(crTotalCurrentReserve, "0.00")
            ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsThisPaymentInclTax).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, crTotalThisPaymentInclTax)
            'ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsThisPaymentInclTax).Text = StringsHelper.Format(crTotalThisPaymentInclTax, "0.00")
            ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsThisPaymentTax).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, crTotalThisPaymentTax)
            'ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsThisPaymentTax).Text = StringsHelper.Format(crTotalThisPaymentTax, "0.00")
            ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsCostToClaim).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, crTotalCostToClaim)
            'ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsCostToClaim).Text = StringsHelper.Format(crTotalCostToClaim, "0.00")

            ''Start(Saurabh Agrawal)Tech Spec QBENZCR004 Claims Recovery Reinsurance
            ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsReceivedToDate).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, crTotalReceivedToDate)
            'ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsReceivedToDate).Text = StringsHelper.Format(crTotalReceivedToDate, "0.00")
            ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsReceivedToDateTax).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, crTotalReceivedToDateTax)
            'ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsReceivedToDateTax).Text = StringsHelper.Format(crTotalReceivedToDateTax, "0.00")
            ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsThisReceipt).Text = StringsHelper.Format(0, "0.00")
            ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsThisReceiptTax).Text = StringsHelper.Format(0, "0.00")
            ''End(Saurabh Agrawal)Tech Spec QBENZCR004 Claims Recovery Reinsurance

            ' bold the total line
            For Each oListSubItem As ListViewItem.ListViewSubItem In oListItem.SubItems
                oListSubItem.Font = VB6.FontChangeBold(oListSubItem.Font, True)
            Next oListSubItem

            'DC090606 Add Coinsurer Details for Datasure



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
    ' Name: GetSafeHarbourDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 26-07-2005 : 360 - Taxes On Claims
    ' ***************************************************************** '
    Private Function GetSafeHarbourDetails() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetSafeHarbourDetails"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            lReturn = m_oBusiness.GetSafeHarbourDetails(r_vResults:=m_vSafeHarbour)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetSafeHarbourDetails", gPMConstants.PMELogLevel.PMLogError)
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
    ' Name: SetupThisPaymentInterface
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function SetupThisPaymentInterface() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupThisPaymentInterface"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' if no payment has been made in this session then
            ' dont show the this payment tab.
            If m_crTotalThisPaymentInclTaxABS <> 0 Or m_crTotalAmount <> 0 Then
                SSTabHelper.SetTabVisible(SSTab1, kTabThisPayment, True)
            Else
                If m_sTransactionType = "C_CP" Or m_sTransactionType = "C_CO" Or m_sTransactionType = "C_CR" Then
                    SSTabHelper.SetTabVisible(SSTab1, kTabThisPayment, False)
                End If
            End If

            If m_bProductMediaTypeMandatory Then
                lblMediaType.Font = VB6.FontChangeBold(lblMediaType.Font, True)
                lblPayeeName.Font = VB6.FontChangeBold(lblMediaType.Font, True)
            Else
                lblMediaType.Font = VB6.FontChangeBold(lblMediaType.Font, False)
                lblPayeeName.Font = VB6.FontChangeBold(lblMediaType.Font, False)
            End If

            lblMediaType.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cboMediaType.Left) - VB6.PixelsToTwipsX(lblMediaType.Width) - 75)



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function



    ' ***************************************************************** '
    ' Name: PopulateThisPaymentDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 24-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function PopulateThisPaymentDetails() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulateThisPaymentDetails"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim crTotalThisPayment, crTotalThisPaymentTax, crTotalThisPaymentTaxWHT, crTotalThisPaymentExcess As Decimal
        Dim oPaymentItem As cPaymentItem
        Dim crTotalPaidToDate, crTotalPaidToDateTax As Decimal
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If m_colPaymentItems.Count <> 0 Then

                For lPaymentItem As Integer = 1 To m_colPaymentItems.Count

                    oPaymentItem = m_colPaymentItems.Item(lPaymentItem)

                    crTotalThisPaymentTax += (oPaymentItem.TaxAmount * oPaymentItem.PaymentToLossXRate)
                    crTotalThisPaymentTaxWHT += (oPaymentItem.TaxAmountWHT * oPaymentItem.PaymentToLossXRate)
                    crTotalThisPayment += (oPaymentItem.ThisPayment * oPaymentItem.PaymentToLossXRate)
                    If m_bOpenClaimNoTrans Then
                        crTotalPaidToDate += crTotalThisPayment
                        crTotalPaidToDateTax += crTotalThisPaymentTax
                    End If
                Next

                ' populate the "this payment summary"
                txtGrossPayment.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, crTotalThisPayment + crTotalThisPaymentTax + crTotalThisPaymentTaxWHT)
                'txtGrossPayment.Text = StringsHelper.Format(crTotalThisPayment + crTotalThisPaymentTax + crTotalThisPaymentTaxWHT, "0.00")
                txtTotalTax.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, crTotalThisPaymentTax)
                'txtTotalTax.Text = StringsHelper.Format(crTotalThisPaymentTax, "0.00")
                txtTotalWHTax.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, crTotalThisPaymentTaxWHT)
                'txtTotalWHTax.Text = StringsHelper.Format(crTotalThisPaymentTaxWHT, "0.00")
                txtNetPayment.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, crTotalThisPayment)
                'txtNetPayment.Text = StringsHelper.Format(crTotalThisPayment, "0.00")

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

    ''' <summary>
    ''' PopulatePaymentDetailThisPayment
    ''' </summary>
    ''' <param name="r_oPaymentItem"></param>
    ''' <param name="r_oListItem"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function PopulatePaymentDetailThisPayment(ByRef r_oPaymentItem As cPaymentItem, ByRef r_oListItem As ListViewItem) As Integer

        Dim nResult As Integer = 0
        Const kMethodName As String = "PopulatePaymentDetailThisPayment"

        Dim crThisPaymentTax, crThisPaymentInclTax, crCostToClaim As Decimal
        Try
            nResult = PMEReturnCode.PMTrue

            ' update the payment detail listview with the new values
            crThisPaymentTax = (r_oPaymentItem.TaxAmount + r_oPaymentItem.TaxAmountWHT) * r_oPaymentItem.PaymentToLossXRate
            crThisPaymentInclTax = (r_oPaymentItem.ThisPayment * r_oPaymentItem.PaymentToLossXRate) + crThisPaymentTax
            crCostToClaim = crThisPaymentInclTax - crThisPaymentTax


            ' populate this payment details back to the grid
            ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsThisPaymentInclTax).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, crThisPaymentInclTax)
            'ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsThisPaymentInclTax).Text = StringsHelper.Format(crThisPaymentInclTax, "0.00")
            If Not r_oPaymentItem.IsExcess Then
                ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsThisPaymentTax).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, crThisPaymentTax)
                'ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsThisPaymentTax).Text = StringsHelper.Format(crThisPaymentTax, "0.00")
            End If

            ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsCostToClaim).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, crCostToClaim)
            'ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsCostToClaim).Text = StringsHelper.Format(crCostToClaim, "0.00")
            If m_bOpenClaimNoTrans Then
                If m_bClaimPaymentIsGross Then
                    ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsCurrentReserve).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CDbl(ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsTotalReserve).Text) - (crThisPaymentInclTax - crThisPaymentTax))
                    'ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsCurrentReserve).Text = StringsHelper.Format(CDbl(ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsTotalReserve).Text) - (crThisPaymentInclTax - crThisPaymentTax), "0.00")
                Else
                    ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsCurrentReserve).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CDbl(ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsTotalReserve).Text) - crCostToClaim)
                    'ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsCurrentReserve).Text = StringsHelper.Format(CDbl(ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsTotalReserve).Text) - crCostToClaim, "0.00")
                End If
            Else
             ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsCurrentReserve).Text = StringsHelper.Format(CDbl(ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsTotalReserve).Text) - (crThisPaymentInclTax) - CDbl(ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsPaidToDate).Text) - CDbl(ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsPaidToDateTax).Text), "0.00")
               ' If m_bClaimReserveIsGross Then
                 '    ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsCurrentReserve).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CDbl(ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsTotalReserve).Text) - (crThisPaymentInclTax) - CDbl(ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsPaidToDate).Text))
               ' Else
               '     ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsCurrentReserve).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CDbl(ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsTotalReserve).Text) - (crCostToClaim - crThisPaymentTax) - CDbl(ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsPaidToDate).Text))
             '   End If
              '  If m_bClaimPaymentIsGross Then
                    ' Commented for 907 Arch
                    'If m_bPostClaimTax Then
                    'ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsCurrentReserve).Text = StringsHelper.Format(CDbl(ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsTotalReserve).Text) - (crThisPaymentInclTax - crThisPaymentTax) - CDbl(ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsPaidToDate).Text), "0.00")
                    'Else
                  '  ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsCurrentReserve).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CDbl(ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsTotalReserve).Text) - (crThisPaymentInclTax - crThisPaymentTax) - CDbl(ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsPaidToDate).Text))
                    'ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsCurrentReserve).Text = StringsHelper.Format(CDbl(ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsTotalReserve).Text) - (crThisPaymentInclTax) - CDbl(ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsPaidToDate).Text), "0.00")
                    'End If
              '  Else
                  '  ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsCurrentReserve).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CDbl(ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsTotalReserve).Text) - crCostToClaim - CDbl(ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsPaidToDate).Text))
                    'ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsCurrentReserve).Text = StringsHelper.Format(CDbl(ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsTotalReserve).Text) - crCostToClaim - CDbl(ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsPaidToDate).Text), "0.00")
               ' End If
            End If

            If m_bOpenClaimNoTrans Then
                ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsPaidToDate).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, crThisPaymentInclTax)
                'ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsPaidToDate).Text = StringsHelper.Format(crThisPaymentInclTax, "0.00")
                ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsPaidToDateTax).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, crThisPaymentTax)
                'ListViewHelper.GetListViewSubItem(r_oListItem, kPayDetailsSubItemsPaidToDateTax).Text = StringsHelper.Format(crThisPaymentTax, "0.00")
            End If

            Return nResult
        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult)
            Return PMEReturnCode.PMError
        End Try
    End Function


    Private Sub SSTab1_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles SSTab1.SelectedIndexChanged
        'Party Bank Details
        If SSTab1PreviousTab = 1 And m_lPartyBankCount <> 0 Then
            m_lPartyBankCount = 1
        End If

        If OptAgent.Checked = True Then
            ActionPayeeOption(kPayeeOptAgent)
        ElseIf OptClient.Checked = True Then
            ActionPayeeOption(kPayeeOptClient)
        ElseIf OptParty.Checked = True Then
            ActionPayeeOption(kPayeeOptParty)
        ElseIf OptClaimPayable.Checked = True Then
            ActionPayeeOption(kPayeeOptClaimPayable)
        End If

        EnableDisablePartyCombo()
        SSTab1PreviousTab = SSTab1.SelectedIndex
    End Sub

    Private Sub txtITPercentage_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtITPercentage.Enter
        ValidatePercentage(txtITPercentage)
    End Sub

    Private Sub txtITPercentage_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtITPercentage.Leave
        ValidatePercentage(txtITPercentage)
    End Sub

    Private Sub txtPTPercentage_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPTPercentage.Enter
        ValidatePercentage(txtPTPercentage)
    End Sub

    Private Sub txtPTPercentage_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPTPercentage.Leave
        ValidatePercentage(txtPTPercentage)
    End Sub

    ' ***************************************************************** '
    ' Name: ValidatePercentage
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function ValidatePercentage(ByRef oTxtBox As TextBox) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ValidatePercentage"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If oTxtBox.Text <> "" Then
                oTxtBox.Text = CStr(gPMFunctions.ToSafeCurrency(oTxtBox.Text, 0))
                If CDbl(oTxtBox.Text) < 0 Or CDbl(oTxtBox.Text) > 100 Then
                    MessageBox.Show("InsuredPercent' and 'PayeePercent' must reflect a value >=0% and <=100%.", "Percentage Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    oTxtBox.Text = "0.00"
                    oTxtBox.Focus()
                Else
                    oTxtBox.Text = StringsHelper.Format(oTxtBox.Text, "0.00")
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

            cmdReserveEdit.Enabled = False '(RC) QBENZ001
            cmdEdit.Enabled = False
            cmdHistory.Enabled = False
            cmdPaymentLock.Enabled = False
            fraClaimInformation.Enabled = False
            fraExemptions.Enabled = False
            fraInsuredTaxAdjustment.Enabled = False
            fraPayee.Enabled = False
            fraPayeeTaxAdjustments.Enabled = False
            fraPaymentDetails.Enabled = False
            fraSafeHarbour.Enabled = False
            fraSettlement.Enabled = False
            fraTaxesOnPayments.Enabled = False
            fraThisPaymentComments.Enabled = False

            'fraThisPaymentDetails.Enabled = False
            txtMediaType.Enabled = False
            txtMediaRef.Enabled = False
            txtPayeeName.Enabled = False
            dtpChequeDate.Enabled = False
            txtBankAccountNo.Enabled = False
            txtBankCode.Enabled = False
            txtBankName.Enabled = False
            txtThirdPartyReference.Enabled = False
            txtBIC.Enabled = False
            txtIBAN.Enabled = False
            'Start (Renuka) PN 61323
            txtOurReference.Enabled = False
            'End (Renuka) PN 61323

            fraThisPaymentSummary.Enabled = False



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
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

            lvwTaxesOnThisPayment.Columns.Clear()

            lvwTaxesOnThisPayment.Columns.Insert(kTaxDetColHIndexReserveType - 1, kTaxDetColHCodeReserveType, "Reserve Type", CInt(VB6.TwipsToPixelsX(1500)), HorizontalAlignment.Left, -1)

            lvwTaxesOnThisPayment.Columns.Insert(kTaxDetColHIndexTaxGroup - 1, kTaxDetColHCodeTaxGroup, "Tax Group", CInt(VB6.TwipsToPixelsX(1600)), HorizontalAlignment.Left, -1)

            lvwTaxesOnThisPayment.Columns.Insert(kTaxDetColHIndexTaxBand - 1, kTaxDetColHCodeTaxBand, "Tax Band", CInt(VB6.TwipsToPixelsX(1600)), HorizontalAlignment.Left, -1)

            lvwTaxesOnThisPayment.Columns.Insert(kTaxDetColHIndexPercentage - 1, kTaxDetColHCodePercentage, "Percentage", CInt(VB6.TwipsToPixelsX(1300)), HorizontalAlignment.Right, -1)

            lvwTaxesOnThisPayment.Columns.Insert(kTaxDetColHIndexTaxAmount - 1, kTaxDetColHCodeTaxAmount, "Amount", CInt(VB6.TwipsToPixelsX(1300)), HorizontalAlignment.Right, -1)


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
            lvwTaxesOnThisPayment.Items.Clear()

            ' for each payment item
            For Each oPaymentItem As cPaymentItem In m_colPaymentItems

                ' get the tax band rate array

                If oPaymentItem.TaxBandRateArray IsNot Nothing AndAlso TypeOf oPaymentItem.TaxBandRateArray Is Object(,) Then
                    vTaxBandRateArray = oPaymentItem.TaxBandRateArray

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


                            oListItem = lvwTaxesOnThisPayment.Items.Add("")

                            ' populate list item
                            oListItem.Text = oPaymentItem.ReserveDesc
                            ListViewHelper.GetListViewSubItem(oListItem, kTaxDetailsSubItemsTaxGroup).Text = sTaxGroupDesc
                            ListViewHelper.GetListViewSubItem(oListItem, kTaxDetailsSubItemsTaxBand).Text = sTaxBandDesc
                            ListViewHelper.GetListViewSubItem(oListItem, kTaxDetailsSubItemsPercentage).Text = StringsHelper.Format(dPercentage, "0.00")
                            'ListViewHelper.GetListViewSubItem(oListItem, kTaxDetailsSubItemsTaxAmount).Text = StringsHelper.Format(crValue * oPaymentItem.PaymentToLossXRate, "0.00")
                            ListViewHelper.GetListViewSubItem(oListItem, kTaxDetailsSubItemsTaxAmount).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, crValue * oPaymentItem.PaymentToLossXRate)
                        Next

                    End If
                Else
                    oPaymentItem.TaxBandRateArray = Nothing
                End If
            Next oPaymentItem

            ' resize the list view dependant on how many tax rows there are....
            If lvwTaxesOnThisPayment.Items.Count <= 7 Then
                lvwTaxesOnThisPayment.Columns.Item(kTaxDetColHIndexReserveType - 1).Width = CInt(VB6.TwipsToPixelsX(1800))
            Else
                lvwTaxesOnThisPayment.Columns.Item(kTaxDetColHIndexReserveType - 1).Width = CInt(VB6.TwipsToPixelsX(1500))
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
    ''' SaveClaimPayment
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SaveClaimPayment() As Integer

        Dim result As Integer = PMEReturnCode.PMTrue
        Const kMethodName As String = "SaveClaimPayment"
        Dim nReturn As gPMConstants.PMEReturnCode
        Dim oPaymentpartyTo, oClaimPaymentToId, oSelectedPayeeId As Object
        Dim oITDomiciled, oITTaxNumber, oITPercentage, oPTDomiciled As Object
        Dim oPTTaxNumber, oPTPercentage, oSafeHarbourId, oSFPercentage As Object
        Dim oIsTaxExempt, oIsTaxWHTExempt, oIsSettlement, oPayeeMediaTypeId As Object
        Dim oPayeeBankName, oPayeeBankAccountNo, oPayeeBankSortCode, oPayeeComments, oPayeeName, oMediaRef As Object
        Dim nClaimPaymentId As Integer
        Dim oPayeeAddressDetails, oPayeeAddress1, oPayeeAddress2, oPayeeAddress3, oPayeeAddress4 As Object
        Dim oPayeePostalCode, oPayeeCountryId, oThirdPartyReference, oChequeDate, oBankPaymentTypeId As Object
        Dim vOurReference As Object
        Dim crTotalThisPaymentInPaymentCurrency, crTotalThisPaymentTaxInPaymentCurrency, crTotalThisPaymentTaxWHTInPaymentCurrency As Decimal
        Dim nCurrencyId, nIsExGratia As Integer
        Dim sBIC As String = String.Empty
        Dim sIBAN As String = String.Empty


        Try
            nReturn = GetFormData(r_vPaymentPartyTo:=oPaymentpartyTo,
                                  r_vClaimPaymentToId:=oClaimPaymentToId,
                                  r_vSelectedPayeeId:=oSelectedPayeeId,
                                  r_vITDomiciled:=oITDomiciled,
                                  r_vITTaxNumber:=oITTaxNumber,
                                  r_vITPercentage:=oITPercentage,
                                  r_vPTDomiciled:=oPTDomiciled,
                                  r_vPTTaxNumber:=oPTTaxNumber,
                                  r_vPTPercentage:=oPTPercentage,
                                  r_vSafeHarbourId:=oSafeHarbourId,
                                  r_vSFPercentage:=oSFPercentage,
                                  r_vIsTaxExempt:=oIsTaxExempt,
                                  r_vIsTaxWHTExempt:=oIsTaxWHTExempt,
                                  r_vIsSettlement:=oIsSettlement,
                                  r_vPayeeMediaTypeId:=oPayeeMediaTypeId,
                                  r_vPayeeName:=oPayeeName,
                                  r_vPayeeBankName:=oPayeeBankName,
                                  r_vPayeeBankAccountNo:=oPayeeBankAccountNo,
                                  r_vPayeeBankSortCode:=oPayeeBankSortCode,
                                  r_vPayeeComments:=oPayeeComments,
                                  r_vMediaRef:=oMediaRef,
                                  r_vPayeeAddressDetails:=oPayeeAddressDetails,
                                  r_vThirdPartyReference:=oThirdPartyReference,
                                  r_vChequeDate:=oChequeDate,
                                  r_vOurReference:=vOurReference,
                                  r_bIsExGratia:=nIsExGratia,
                                  r_sBIC:=sBIC,
                                  r_sIBAN:=sIBAN)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetFormData Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' GetPaymentTotalsInPaymentCurrency
            For Each oPaymentItem As cPaymentItem In m_colPaymentItems
                ' dont include any totals from any excess rows on the payment header
                ' as this amount is not actually posted just recorded
                crTotalThisPaymentInPaymentCurrency += oPaymentItem.ThisPayment
                crTotalThisPaymentTaxWHTInPaymentCurrency += oPaymentItem.TaxAmount
                crTotalThisPaymentTaxInPaymentCurrency += oPaymentItem.TaxAmountWHT
                If oPaymentItem.CurrencyId > 0 Then
                    nCurrencyId = oPaymentItem.CurrencyId
                End If
            Next oPaymentItem

            '*******************************
            '* data to claim payment array *
            '*******************************
            Dim vClaimPaymentDetails(eClaimPayment.kLast) As Object

            vClaimPaymentDetails(eClaimPayment.kClaimid) = m_lWorkClaimID
            vClaimPaymentDetails(eClaimPayment.kClaimPerilId) = m_lWorkClaimPerilId
            vClaimPaymentDetails(eClaimPayment.kPaymentDate) = DateTime.Today
            vClaimPaymentDetails(eClaimPayment.kAmount) = crTotalThisPaymentInPaymentCurrency
            vClaimPaymentDetails(eClaimPayment.kTaxAmount) = crTotalThisPaymentTaxInPaymentCurrency
            vClaimPaymentDetails(eClaimPayment.kTaxAmountWHT) = crTotalThisPaymentTaxWHTInPaymentCurrency
            vClaimPaymentDetails(eClaimPayment.kPartyCnt) = oSelectedPayeeId
            vClaimPaymentDetails(eClaimPayment.kComments) = DBNull.Value
            vClaimPaymentDetails(eClaimPayment.kIsReferred) = DBNull.Value
            vClaimPaymentDetails(eClaimPayment.kCreatedBy) = m_oObjectManager.UserID
            vClaimPaymentDetails(eClaimPayment.kPayeeMediaTypeId) = oPayeeMediaTypeId
            vClaimPaymentDetails(eClaimPayment.kPayeeName) = oPayeeName
            vClaimPaymentDetails(eClaimPayment.kBankName) = oPayeeBankName
            vClaimPaymentDetails(eClaimPayment.kBankSortCode) = oPayeeBankSortCode
            vClaimPaymentDetails(eClaimPayment.kBankAccountNo) = oPayeeBankAccountNo
            vClaimPaymentDetails(eClaimPayment.kPayeeComments) = oPayeeComments
            vClaimPaymentDetails(eClaimPayment.kSequenceNo) = DBNull.Value
            vClaimPaymentDetails(eClaimPayment.kTreatyId) = DBNull.Value
            vClaimPaymentDetails(eClaimPayment.kClaimPaymentTo) = oClaimPaymentToId
            vClaimPaymentDetails(eClaimPayment.kPaymentPartyTo) = oPaymentpartyTo
            vClaimPaymentDetails(eClaimPayment.kInsuredDomiciled) = oITDomiciled
            vClaimPaymentDetails(eClaimPayment.kInsuredPercentage) = oITPercentage
            vClaimPaymentDetails(eClaimPayment.kInsuredTaxNumber) = oITTaxNumber
            vClaimPaymentDetails(eClaimPayment.kPayeeDomiciled) = oPTDomiciled
            vClaimPaymentDetails(eClaimPayment.kPayeePercentage) = oPTPercentage
            vClaimPaymentDetails(eClaimPayment.kPayeeTaxNumber) = oPTTaxNumber
            vClaimPaymentDetails(eClaimPayment.kSafeHarbourId) = oSafeHarbourId
            vClaimPaymentDetails(eClaimPayment.kSafeHarbourPercentage) = oSFPercentage
            vClaimPaymentDetails(eClaimPayment.kIsTaxExempt) = oIsTaxExempt
            vClaimPaymentDetails(eClaimPayment.kIsWHTExempt) = oIsTaxWHTExempt
            vClaimPaymentDetails(eClaimPayment.kIsSettlement) = oIsSettlement
            vClaimPaymentDetails(eClaimPayment.kDocumentId) = DBNull.Value
            vClaimPaymentDetails(eClaimPayment.kIsLive) = DBNull.Value
            vClaimPaymentDetails(eClaimPayment.kLiveClaimPaymentId) = DBNull.Value
            vClaimPaymentDetails(eClaimPayment.kMediaRef) = oMediaRef
            vClaimPaymentDetails(eClaimPayment.kCurrencyId) = gPMFunctions.ToSafeLong(nCurrencyId, 0)
            vClaimPaymentDetails(eClaimPayment.kExcessAmount) = gPMFunctions.ToSafeLong(m_crExcessAmount, 0)
            vClaimPaymentDetails(eClaimPayment.kPayeeAddress1) = oPayeeAddressDetails(0)
            vClaimPaymentDetails(eClaimPayment.kPayeeAddress2) = oPayeeAddressDetails(1)
            vClaimPaymentDetails(eClaimPayment.kPayeeAddress3) = oPayeeAddressDetails(2)
            vClaimPaymentDetails(eClaimPayment.kPayeeAddress4) = oPayeeAddressDetails(3)
            vClaimPaymentDetails(eClaimPayment.kPayeePostalCode) = oPayeeAddressDetails(4)
            vClaimPaymentDetails(eClaimPayment.kPayeeCountryId) = gPMFunctions.ToSafeLong(oPayeeAddressDetails(5), 0)
            vClaimPaymentDetails(eClaimPayment.kThirdPartyReference) = oThirdPartyReference
            vClaimPaymentDetails(eClaimPayment.kChequeDate) = oChequeDate
            vClaimPaymentDetails(eClaimPayment.kBankPaymentTypeId) = m_vBankPaymentTypeId
            vClaimPaymentDetails(eClaimPayment.kOurReference) = vOurReference
            vClaimPaymentDetails(eClaimPayment.kIsExGratia) = nIsExGratia
            vClaimPaymentDetails(eClaimPayment.kBIC) = sBIC
            vClaimPaymentDetails(eClaimPayment.kIBAN) = sIBAN

            nReturn = m_oBusiness.SaveClaimPayment(v_vClaimPayment:=vClaimPaymentDetails, r_lClaimPaymentId:=nClaimPaymentId)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SaveClaimPayment Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            nReturn = CType(SaveClaimPaymentItems(v_lClaimPaymentId:=nClaimPaymentId), gPMConstants.PMEReturnCode)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "bCLMPeril.SaveClaimPaymentItems Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            Return result
        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally
        End Try
        Return result
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
    Private Function SaveClaimPaymentItems(ByVal v_lClaimPaymentId As Integer) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "SaveClaimPaymentItems"

        Const kWorkClaimPaymentItemId As Integer = 0
        Const kWorkClaimPaymentId As Integer = 1
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

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' for each payment item in this payment
            For Each oPaymentItem As cPaymentItem In m_colPaymentItems

                If oPaymentItem.ThisPayment <> 0 OrElse oPaymentItem.ThisReserveRevision <> 0 Then

                    ' prepare claim payment item array
                    Dim vClaimPaymentItem(kPaymentAdjustment) As Object

                    ' populate claim payment item array


                    vClaimPaymentItem(kWorkClaimPaymentItemId) = DBNull.Value

                    vClaimPaymentItem(kWorkClaimPaymentId) = v_lClaimPaymentId

                    vClaimPaymentItem(kReserveId) = oPaymentItem.ReserveId


                    vClaimPaymentItem(kRecoveryId) = DBNull.Value


                    vClaimPaymentItem(kRecoveryTypeId) = DBNull.Value

                    vClaimPaymentItem(kCurrencyId) = oPaymentItem.CurrencyId
                    'PN: 53820
                    If oPaymentItem.TaxGroupId < 1 Then


                        vClaimPaymentItem(kTaxGroupId) = DBNull.Value
                    Else

                        vClaimPaymentItem(kTaxGroupId) = oPaymentItem.TaxGroupId
                    End If

                    vClaimPaymentItem(kThisPayment) = oPaymentItem.ThisPayment

                    vClaimPaymentItem(kTaxAmount) = oPaymentItem.TaxAmount

                    vClaimPaymentItem(kTaxAmountWHT) = oPaymentItem.TaxAmountWHT

                    vClaimPaymentItem(kExchangeRateOverrideReasonId) = oPaymentItem.ExchangeRateOverrideReasonId

                    vClaimPaymentItem(kCurrencyBaseXrate) = oPaymentItem.CurrencyToBaseXRate

                    vClaimPaymentItem(kCurrencyBaseDate) = oPaymentItem.CurrencyToBaseDate

                    vClaimPaymentItem(kAccountBaseXRate) = oPaymentItem.AccountToBaseXRate

                    vClaimPaymentItem(kAccountBaseDate) = oPaymentItem.AccountToBaseDate

                    vClaimPaymentItem(kSystemBaseXRate) = oPaymentItem.SystemToBaseXRate

                    vClaimPaymentItem(kSystemBaseDate) = oPaymentItem.SystemToBaseDate

                    vClaimPaymentItem(kPaymentToLossXRate) = oPaymentItem.PaymentToLossXRate


                    vClaimPaymentItem(kIsLive) = DBNull.Value


                    vClaimPaymentItem(kLiveClaimPaymentId) = DBNull.Value


                    vClaimPaymentItem(kLiveRecoveryId) = DBNull.Value


                    vClaimPaymentItem(kLiveReserveId) = DBNull.Value


                    vClaimPaymentItem(kLiveClaimPaymentItemId) = DBNull.Value

                    vClaimPaymentItem(kPaymentAdjustment) = oPaymentItem.PaymentAdjustment

                    ' save the claim payment item details
                    If oPaymentItem.ThisPayment <> 0 Then
                        lReturn = m_oBusiness.SaveClaimPaymentItem(v_vClaimPaymentItem:=vClaimPaymentItem, r_lClaimPaymentItemId:=lClaimPaymentItemId)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "SaveClaimPaymentItem Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If


                        ' save the tax entries for the claim payment item
                        lReturn = CType(SaveTaxCalculations(v_lClaimPaymentId:=v_lClaimPaymentId, v_lClaimPaymentItemId:=lClaimPaymentItemId, v_oPaymentItem:=oPaymentItem), gPMConstants.PMEReturnCode)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "bCLMPeril.SaveClaimPaymentItemTaxItems Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                    End If
                    ' update the associated reserve with the payment details
                    lReturn = CType(UpdateReserve(oPaymentItem), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "UpdateReserve Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                End If

            Next oPaymentItem



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
    Private Function SaveTaxCalculations(ByVal v_lClaimPaymentId As Integer, ByVal v_lClaimPaymentItemId As Integer, ByVal v_oPaymentItem As cPaymentItem) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SaveTaxCalculations"

        Const kWorkTaxCalculationCnt As Integer = 0
        Const kClaimPerilId As Integer = 1
        Const kWorkClaimPaymentId As Integer = 2
        Const kWorkClaimReceiptId As Integer = 3
        Const kWorkClaimPaymentItemId As Integer = 4
        Const kWorkClaimReceiptItemId As Integer = 5
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

            If v_oPaymentItem.TaxBandRateArray IsNot Nothing AndAlso TypeOf v_oPaymentItem.TaxBandRateArray Is Object(,) Then
                vTaxBandRateArray = v_oPaymentItem.TaxBandRateArray

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

                        vTaxCalculation(kClaimPerilId) = m_lWorkClaimPerilId

                        vTaxCalculation(kWorkClaimPaymentId) = v_lClaimPaymentId


                        vTaxCalculation(kWorkClaimReceiptId) = DBNull.Value

                        vTaxCalculation(kWorkClaimPaymentItemId) = v_lClaimPaymentItemId


                        vTaxCalculation(kWorkClaimReceiptItemId) = DBNull.Value


                        vTaxCalculation(kTaxBandId) = vTaxBandRateArray(kTaxArrayTaxBandId, lTaxItem)

                        vTaxCalculation(kPremium) = v_oPaymentItem.ThisPayment


                        vTaxCalculation(kPercentage) = vTaxBandRateArray(kTaxArrayPercentage, lTaxItem)


                        vTaxCalculation(kValue) = vTaxBandRateArray(kTaxArrayValue, lTaxItem)


                        vTaxCalculation(kIsValue) = vTaxBandRateArray(kTaxArrayIsValue, lTaxItem)

                        vTaxCalculation(kCurrencyId) = v_oPaymentItem.CurrencyId


                        If CStr(vTaxBandRateArray(kTaxArrayClassOfBusinessId, lTaxItem)).Trim() = "" Then


                            vTaxCalculation(kClassOfBusinessId) = DBNull.Value
                        Else


                            vTaxCalculation(kClassOfBusinessId) = vTaxBandRateArray(kTaxArrayClassOfBusinessId, lTaxItem)
                        End If



                        vTaxCalculation(kTaxGroupId) = vTaxBandRateArray(kTaxArrayTaxGroupId, lTaxItem)


                        vTaxCalculation(kSequence) = vTaxBandRateArray(kTaxArraySequence, lTaxItem)

                        vTaxCalculation(kTransType) = kTaxTransTypeClaimPayment


                        vTaxCalculation(kIsManuallyChanged) = vTaxBandRateArray(kTaxArrayIsManuallyChanged, lTaxItem)

                        ' save the tax item to the database

                        lReturn = m_oBusiness.SaveTaxCalculationItem(v_vTaxCalculation:=vTaxCalculation, r_lTaxCalculationCnt:=lTaxCalculationCnt)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "bCLMPeril.SaveWorkTaxCalculation Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                    Next

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
    ' Name: GetSelectedComboItemData
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 05-09-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    'developer guide no.101
    Private Function GetSelectedComboItemData(ByRef r_oCombo As ComboBox, ByRef r_vSelectedItemId As Object) As Integer

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
    Private Function GetFormData(ByRef r_vPaymentPartyTo As Object,
                                 ByRef r_vClaimPaymentToId As Object,
                                 ByRef r_vSelectedPayeeId As Object,
                                 ByRef r_vITDomiciled As Object,
                                 ByRef r_vITTaxNumber As Object,
                                 ByRef r_vITPercentage As Object,
                                 ByRef r_vPTDomiciled As Object,
                                 ByRef r_vPTTaxNumber As Object,
                                 ByRef r_vPTPercentage As Object,
                                 ByRef r_vSafeHarbourId As Object,
                                 ByRef r_vSFPercentage As Object,
                                 ByRef r_vIsTaxExempt As Object,
                                 ByRef r_vIsTaxWHTExempt As Object,
                                 ByRef r_vIsSettlement As Object,
                                 ByRef r_vPayeeMediaTypeId As Object,
                                 ByRef r_vPayeeName As Object,
                                 ByRef r_vPayeeBankName As Object,
                                 ByRef r_vPayeeBankAccountNo As Object,
                                 ByRef r_vPayeeBankSortCode As Object,
                                 ByRef r_vPayeeComments As Object,
                                 ByRef r_vMediaRef As Object,
                                 ByRef r_vPayeeAddressDetails As Object,
                                 ByRef r_vThirdPartyReference As Object,
                                 ByRef r_vChequeDate As Object,
                                 ByRef r_vOurReference As Object,
                                 ByRef r_bIsExGratia As Object,
                                 ByRef r_sBIC As String,
                                 ByRef r_sIBAN As String) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Const kMethodName As String = "GetFormData"

        Try
            '*************************
            ' party payment to
            '*************************
            GetSelectedComboItemData(cboClaimPaymentTo, r_vClaimPaymentToId)

            If m_lSelectedPayeeId = 0 Then

                r_vSelectedPayeeId = Nothing
            Else
                r_vSelectedPayeeId = m_lSelectedPayeeId
            End If

            r_vPaymentPartyTo = kPayeeOptNone

            If OptClaimPayable.Checked Then
                r_vPaymentPartyTo = kPayeeOptClaimPayable
            ElseIf OptParty.Checked Then
                r_vPaymentPartyTo = kPayeeOptParty
            ElseIf OptAgent.Checked Then
                r_vPaymentPartyTo = kPayeeOptAgent
            ElseIf OptClient.Checked Then
                r_vPaymentPartyTo = kPayeeOptClient
            End If

            '*************************
            ' insured tax adjustment
            '*************************
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

                r_vITTaxNumber = Nothing
            Else
                r_vITTaxNumber = txtITTaxNo.Text
            End If

            '*************************
            ' payee tax adjustment
            '*************************
            If chkPTDomiciled.CheckState = CheckState.Checked Then
                r_vPTDomiciled = 1
            Else
                r_vPTDomiciled = 0
            End If

            If txtPTPercentage.Text = "" Then

                r_vPTPercentage = Nothing
            Else
                r_vPTPercentage = txtPTPercentage.Text
            End If

            If txtPTTaxNo.Text.Trim() = "" Then

                r_vPTTaxNumber = Nothing
            Else
                r_vPTTaxNumber = txtPTTaxNo.Text
            End If

            '*************************
            ' safe harbour details
            '*************************
            If fraSafeHarbour.Enabled Then

                GetSelectedComboItemData(cboSafeHarbour, r_vSafeHarbourId)

                If txtSFPercentage.Text = "" Then

                    r_vSFPercentage = Nothing
                Else
                    r_vSFPercentage = txtSFPercentage.Text
                End If
            Else

                r_vSafeHarbourId = Nothing

                r_vSFPercentage = Nothing
            End If

            '*************************
            ' tax exemptions
            '*************************
            If fraExemptions.Enabled Then

                If chkTaxExempt.CheckState = CheckState.Checked Then
                    r_vIsTaxExempt = 1
                Else
                    r_vIsTaxExempt = 0
                End If

                If chkWHTExempt.CheckState = CheckState.Checked Then
                    r_vIsTaxWHTExempt = 1
                Else
                    r_vIsTaxWHTExempt = 0
                End If
            Else

                r_vIsTaxExempt = Nothing

                r_vIsTaxWHTExempt = Nothing
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

            ' size payee address details array
            ReDim r_vPayeeAddressDetails(5)

            If uctPMAddressControl1.AddressLine1.Trim() = "" Then


                r_vPayeeAddressDetails(0) = DBNull.Value
            Else

                r_vPayeeAddressDetails(0) = uctPMAddressControl1.AddressLine1
            End If

            If uctPMAddressControl1.AddressLine2.Trim() = "" Then


                r_vPayeeAddressDetails(1) = DBNull.Value
            Else

                r_vPayeeAddressDetails(1) = uctPMAddressControl1.AddressLine2
            End If

            If uctPMAddressControl1.AddressLine3.Trim() = "" Then


                r_vPayeeAddressDetails(2) = DBNull.Value
            Else

                r_vPayeeAddressDetails(2) = uctPMAddressControl1.AddressLine3
            End If

            If uctPMAddressControl1.AddressLine4.Trim() = "" Then


                r_vPayeeAddressDetails(3) = DBNull.Value
            Else

                r_vPayeeAddressDetails(3) = uctPMAddressControl1.AddressLine4
            End If

            If uctPMAddressControl1.PostCode.Trim() = "" Then


                r_vPayeeAddressDetails(4) = DBNull.Value
            Else

                r_vPayeeAddressDetails(4) = uctPMAddressControl1.PostCode
            End If

            If StringsHelper.ToDoubleSafe(CStr(uctPMAddressControl1.CountryId).Trim()) = 0 Then


                r_vPayeeAddressDetails(5) = DBNull.Value
            Else

                r_vPayeeAddressDetails(5) = uctPMAddressControl1.CountryId
            End If

            If txtThirdPartyReference.Text.Trim() = "" Then

                r_vThirdPartyReference = Nothing
            Else
                r_vThirdPartyReference = txtThirdPartyReference.Text
            End If

            'Start(Saurabh Agrawal) LOA010 Tech Spec Claim Payment Improvement
            If txtOurReference.Text.Trim() = "" Then

                r_vOurReference = Nothing
            Else
                r_vOurReference = txtOurReference.Text
            End If
            'End(Saurabh Agrawal) LOA010 Tech Spec Claim Payment Improvement

            If chkIsExGratia.CheckState = CheckState.Checked Then
                r_bIsExGratia = 1
            Else
                r_bIsExGratia = 0
            End If

            r_vChequeDate = dtpChequeDate.Value

            If Not txtBIC.Text.Trim() = "" Then
                r_sBIC = txtBIC.Text.Trim()
            End If

            If Not txtIBAN.Text.Trim() = "" Then
                r_sIBAN = txtIBAN.Text.Trim()
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
    ' Name: GetClaimPaymentItemTaxDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 06-09-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function GetClaimPaymentItemTaxDetails() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimPaymentItemTaxDetails"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lWorkClaimPaymentItemId As CheckState
        Dim vTaxDetails As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' for each claim payment item
            For Each oPaymentItem As cPaymentItem In m_colPaymentItems

                ' get the item id
                lWorkClaimPaymentItemId = oPaymentItem.WorkClaimPaymentId

                ' get tax detail array for the specified claim payment item id

                lReturn = m_oBusiness.GetClaimPaymentItemTax(v_lClaimPaymentItemId:=lWorkClaimPaymentItemId, r_vResults:=vTaxDetails)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "bCLMPeril.GetClaimPaymentTax Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' assign the tax details back to the payment item
                If Information.IsArray(vTaxDetails) Then


                    oPaymentItem.TaxBandRateArray = vTaxDetails
                End If

            Next oPaymentItem



        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost
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

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            '*********************************
            ' default claim payment system option
            '*********************************

            m_lDefaultClaimPaymentOption = kPayeeOptNone

            '*********************************
            '   claim payment authority system option
            '*********************************
            lReturn = CType(iPMFunc.GetSystemOption(v_iOptionNumber:=kSysOptionClaimPaymentAuthority, r_sOptionValue:=sOptionNo, v_iSourceID:=m_iSourceID), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetSystemOption Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' set the default claim payment option
            m_lClaimPaymentAuthorityOption = gPMFunctions.ToSafeLong(sOptionNo, 0)

            'DC150606 PN28938 only check fo system option if Underwriting


            sOptionNo = ""

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

            sOptionNo = ""

            '********************************
            ' Payment ATS Safe Harbour : 5072
            '********************************
            lReturn = CType(iPMFunc.GetSystemOption(v_iOptionNumber:=kSysOptionPaymentATSSafeHarbour, r_sOptionValue:=sOptionNo, v_iSourceID:=m_iSourceID), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetSystemOption AdvancedTax Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            If sOptionNo = "1" Then
                ' store system option value
                m_bPaymentATSSafeHarbour = True
            End If

        '********************************
        ' Current Reserve Is Gross : 5239
        '********************************
        sOptionNo = ""
        lReturn = CType(iPMFunc.GetSystemOption(v_iOptionNumber:=kSysOptionCurrentReserveIsGross, r_sOptionValue:=sOptionNo, v_iSourceID:=m_iSourceID), gPMConstants.PMEReturnCode)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "GetSystemOption CurrentReserveIsGross Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        If sOptionNo = "1" Then
            ' store system option value
             m_bClaimReserveIsGross = True
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
    ' Name: UpdateReserve
    '
    ' Parameters: n/a
    '
    ' Description: Updates the reserve associated with the claim
    '               payment item according to payment / revision
    '
    ' History:
    '           Created : MEvans : 08-09-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function UpdateReserve(ByVal v_oPaymentItem As cPaymentItem) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateReserve"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lReserveID As Integer
        Dim crThisRevision, crThisPayment, crThisTaxAmount, crTotalPayment As Decimal

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get claim payment item reserve id
            lReserveID = v_oPaymentItem.ReserveId
            crThisPayment = v_oPaymentItem.ThisPayment * v_oPaymentItem.PaymentToLossXRate
            crThisTaxAmount = (v_oPaymentItem.TaxAmount + v_oPaymentItem.TaxAmountWHT) * v_oPaymentItem.PaymentToLossXRate

            'Commented form 907 Arch
            If m_bPostClaimTax Then
                ' if taxes are to be posted seperately then
                ' just post the payment amount against the reserve
                crTotalPayment = crThisPayment
            Else
                ' if taxes are "NOT" to be posted seperately then
                ' post the payment amount plus the tax amount against the reserve
                crTotalPayment = crThisPayment + crThisTaxAmount
            End If

            crThisRevision = 0

            ' if negative reserves are not allowed
            If Not m_bAllowNegativeReserve Then

                ' ensure post claim tax flag set prior to calculation
                v_oPaymentItem.PostClaimTax = m_bPostClaimTax

                'NB: The Excess rows calculation used to determine the
                ' revision amount is different to the one used for non excess reserve types
                crThisRevision = v_oPaymentItem.ThisRevisionInLossCurrency

            Else
                crThisRevision = v_oPaymentItem.ThisReserveRevision 'QBENZ001 (RC)
            End If

            ' update the claim payment item reserve

            lReturn = m_oBusiness.UpdateClaimPaymentItemReserve(v_lReserveId:=lReserveID, v_crThisRevision:=crThisRevision, v_crThisPayment:=crTotalPayment)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "UpdateReserve Failed", gPMConstants.PMELogLevel.PMLogError)
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
    ' Name: SavePaymentToAccounts
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function SavePaymentToAccounts(ByVal v_lClaimPaymentId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SavePaymentToAccounts"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim crTotalPaymentInPaymentCurrency As Decimal
        Dim sShortName As String = ""
        Dim crPaymentAmount, crTaxAmount As Decimal

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get the total payment amount in payment currency
            For Each oPaymentItem As cPaymentItem In m_colPaymentItems

                ' NB: this payment and excess amount are in payment currency
                ' so no conversion required
                crPaymentAmount = oPaymentItem.ThisPayment

                ' the tax amount and tax amount WHT are in payment currency
                ' so no conversion required
                crTaxAmount = oPaymentItem.TaxAmountWHT + oPaymentItem.TaxAmount

                If m_bPostClaimTax Then
                    ' calculate the total amount
                    crTotalPaymentInPaymentCurrency += crPaymentAmount
                Else
                    ' calculate the total amount
                    crTotalPaymentInPaymentCurrency = crTotalPaymentInPaymentCurrency + crPaymentAmount + crTaxAmount
                End If

            Next oPaymentItem

            If m_sClassOfBusinessCode <> "" Then

                If m_lSelectedPayeeId <> 0 Then

                    'get party name

                    lReturn = m_oBusiness.GetPartyName(v_lPartyCnt:=m_lSelectedPayeeId, v_sFieldName:="shortname", r_sResult:=sShortName)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "GetPartyName Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                Else
                    sShortName = "CLMPAYABLE"
                End If

                '**************************
                '* post payment and taxes *
                '**************************



                lReturn = m_oBusiness.PostPaymentToOrion(v_lClaimPaymentId:=v_lClaimPaymentId, v_sClaimNumber:=m_sClaimNumber, v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_lClaimId:=m_lWorkClaimID, v_lClaimPerilId:=m_lWorkClaimPerilId, v_cPayAmount:=crTotalPaymentInPaymentCurrency, v_sCreditAccountCode:=sShortName, v_sCOBCode:=m_sClassOfBusinessCode, v_lCOBId:=m_lClassOfBusinessId, v_bPostClaimTax:=m_bPostClaimTax, v_lPartyCnt:=m_lSelectedPayeeId)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "PostPaymentToOrion Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            Else
                gPMFunctions.RaiseError(kMethodName, "SavePaymentsToAccounts Failed - No Class Of Business Code Available", gPMConstants.PMELogLevel.PMLogError)
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
    ' Name: CreateChequePaymentTask
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : EKnott : Date xx/11/2005: Process ID
    ' ***************************************************************** '


    'Private Function CreateAuthorisationTask(ByVal v_lPartyCnt As Integer, ByVal v_lClaimPaymentId As Integer) As Integer
    'Dim result As Integer = 0
    'Dim lReturn As gPMConstants.PMEReturnCode
    'Dim vKeyArray As Object
    'Dim lPMWrkTaskInstanceCnt As Integer
    'Dim sTaskGroupCode, sPartyShortName As String
    '
    'Const c_sTaskCode As String = "AUTHCLMCHQ"
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '




    Private Function CreateWrkTaskInstances() As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oWrkTaskInstanceTemp Is Nothing Then

                Dim temp_m_oWrkTaskInstanceTemp As Object
                lReturn = m_oObjectManager.GetInstance(temp_m_oWrkTaskInstanceTemp, sClassName:="iPMWrkTaskInstanceTemp.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oWrkTaskInstanceTemp = temp_m_oWrkTaskInstanceTemp
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to get instance of iPMWrkTaskInstanceTemp.Interface")
                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateWrkTaskInstances Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateWrkTaskInstances", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Private Function DestroyWrkTaskInstances() As Integer
        Dim result As Integer = 0
        Dim lReturn As Integer
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not (m_oWrkTaskInstanceTemp Is Nothing) Then

                m_oWrkTaskInstanceTemp.Dispose()
                m_oWrkTaskInstanceTemp = Nothing
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DestroyWrkTaskInstances Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DestroyWrkTaskInstances", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
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
    Private Function GetPaymentCurrencyFilter(ByRef r_lPaymentCurrencyFilter As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPaymentCurrencyFilter"

        Dim lReturn As Integer
        Dim oPaymentItem As cPaymentItem

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' if there is more than one item in the collection
            If m_colPaymentItems.Count > 1 Then '

                r_lPaymentCurrencyFilter = m_colPaymentItems.Item(1).CurrencyId
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
        Dim sReserveId As String = ""
        Dim oListItem As ListViewItem
        Dim bIsExcess, bDeleteExcess As Boolean
        Dim lNoOfPayments As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get the selected list item
            oListItem = lvwPaymentDetails.Items.Item(m_lSelectedPayeeDetailIndex - 1)

            ' get the payment items reserve details
            sReserveId = ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsTypeId).Text
            bIsExcess = CBool(ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsIsExcess).Text)

            If bIsExcess Then

                bDeleteExcess = True

                ' get the number of payments lines added in this session excluding excess lines
                lReturn = CType(GetNoOfPaymentsItems(lNoOfPayments), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetNoOfPaymentsItems Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                If lNoOfPayments > 0 Then
                    ' confirm that the user actually wants to change the party
                    If MessageBox.Show("Deleting the excess row will result in all payments made in the session being cleared down." &
                                       Strings.Chr(13) & Strings.Chr(10) & "Do you want to continue?", "Excess Line Deletion", MessageBoxButtons.YesNo) = System.Windows.Forms.DialogResult.No Then
                        bDeleteExcess = False
                    End If
                End If

                If bDeleteExcess Then
                    ' erase any payments that have been made in this session
                    'developer guide no.98
                    lReturn = CType(ResetPaymentsOnly(v_lIncludeExcessItemId:=ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsTypeId).Text), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "ResetPaymentsOnly Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If
            Else

                ' remove the item from the collection
                m_colPaymentItems.Remove(sReserveId)

                ' populate this payment details back to the grid
                ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsThisPaymentInclTax).Text = StringsHelper.Format(0, "0.00")
                ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsThisPaymentTax).Text = StringsHelper.Format(0, "0.00")
                ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsCostToClaim).Text = StringsHelper.Format(0, "0.00")
                ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsCurrentReserve).Text = StringsHelper.Format(CDbl(ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsTotalReserve).Text) - CDbl(ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsPaidToDate).Text), "0.00")


            End If

            ' populate this payment details
            lReturn = PopulateThisPaymentDetails()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "PopulateThisPaymentDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' populate total line
            lReturn = PopulateTotals()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "PopulateTotals Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' set up this payment form
            lReturn = SetupThisPaymentInterface()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetupThisPaymentInterface", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' reset available actions for the selected item
            lReturn = CType(ActionSelectPaymentItem(lvwPaymentDetails.FocusedItem), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "ActionSelectPaymentItem Failed", gPMConstants.PMELogLevel.PMLogError)
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
    ' Name: SaveReserveAdjustmentsToAccounts
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 12-09-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function SaveReserveAdjustmentsToAccounts(ByVal v_lWorkClaimPaymentId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SaveReserveAdjustmentsToAccounts"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim crTotalRevisionAmount As Decimal

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' if negative reserves are not allowed
            'If Not m_bAllowNegativeReserve Then

            ' for each payment item
            For Each oPaymentItem As cPaymentItem In m_colPaymentItems

                ' ensure that the post claim tax flag is set prior to calculation
                oPaymentItem.PostClaimTax = m_bPostClaimTax

                ' calculate the total revision amount
                crTotalRevisionAmount += oPaymentItem.ThisRevisionInLossCurrency

            Next oPaymentItem

            'End If

            ' if there is a revision amount
            If crTotalRevisionAmount <> 0 Then

                ' post it to accounts

                lReturn = m_oBusiness.PostReserveAdjustmentToOrion(v_crRevisionAmount:=crTotalRevisionAmount, v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_lClaimId:=m_lWorkClaimID, v_sClaimNo:=m_sClaimNumber, v_lPerilID:=m_lWorkClaimPerilId, v_sCOBCode:=m_sClassOfBusinessCode, v_lCOBId:=m_lClassOfBusinessId)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "PostClaimPaymentReserveAdjustmentToOrion Failed", gPMConstants.PMELogLevel.PMLogError)
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
    ' Name: GetProductOptions
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 14-09-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function GetProductOptions() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetProductOptions"

        Dim lReturn As Integer
        Dim vValue As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            '*****************************************

            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableRI2007, v_vBranch:=gPMConstants.SIRBCHHeadOffice, r_vUnderwriting:=vValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, "getProductOptionValue Failed " &
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
    ' Name: EnableDisablePayeeDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 23-09-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function EnableDisablePayeeDetails(ByVal v_bEnabled As Boolean) As Integer

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
                    lReturn = MessageBox.Show("Changing the payee or tax details will result in all " &
                              "payments added in this session being cleared down. " &
                              "Do you wish to continue?", "Edit Payee Details", MessageBoxButtons.YesNo)

                    If lReturn = System.Windows.Forms.DialogResult.No Then
                        bContinue = False
                    End If
                End If

                If bContinue Then

                    ' enable / disable all payee details which
                    ' can effect tax calculations
                    fraPayee.Enabled = v_bEnabled

                    fraSafeHarbour.Enabled = v_bEnabled

                    If Not v_bEnabled Then
                        fraInsuredTaxAdjustment.Enabled = v_bEnabled
                        fraPayeeTaxAdjustments.Enabled = v_bEnabled
                    Else
                        lReturn = SetupTaxAdjustmentFrames()
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "SetupTaxAdjustmentFrames Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                    End If

                    fraExemptions.Enabled = v_bEnabled
                    fraSettlement.Enabled = v_bEnabled

                    ' if the payee details are to be reenabled
                    ' clear all payment details from this session
                    If v_bEnabled Then

                        ' reset all payment lines
                        lReturn = ResetPaymentsOnly()
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "ResetPaymentsOnly failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                        ' populate this payment details
                        lReturn = PopulateThisPaymentDetails()
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "PopulateThisPaymentDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                        ' populate total line
                        lReturn = PopulateTotals()
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "PopulateTotals Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                        ' set up this payment form
                        lReturn = SetupThisPaymentInterface()
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




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetExcessDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 23-09-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function GetExcessDetails() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetExcessDetails"

        Dim lReturn As Integer
        Dim oListItem As ListViewItem
        Dim crTotalExcess As Decimal

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            'm_bExcessSupplied = True

            If m_bAdvancedTaxScriptingOption Then

                ' for each payment detail item
                For lItem As Integer = 1 To lvwPaymentDetails.Items.Count

                    ' get the item
                    oListItem = lvwPaymentDetails.Items.Item(lItem - 1)

                    ' ignore any spacers and the total line
                    If oListItem.Text <> kLVWSpacer And oListItem.Text <> kLVWTotal Then

                        ' if the item is an excess row
                        If CBool(ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsIsExcess).Text) Then

                            ' if the excess row hasnt been supplied with a value then
                            'If oListItem.SubItems(kPayDetailsSubItemsThisPaymentInclTax) = kLVSCExcessNotSet Then

                            ' flag not all excess rows have been filled in
                            'm_bExcessSupplied = False
                            'End If

                            ' calculate total amount of excess to date
                            crTotalExcess += gPMFunctions.ToSafeCurrency(ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsThisPaymentInclTax).Text, 0)

                        End If

                    End If
                Next

                ' store total amount of excess specified
                m_crExcessAmount = crTotalExcess

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
    ' Name: GetDefaultTaxItem
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function GetDefaultTaxItem(ByRef r_oTaxItem As cTaxParameters) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetDefaultTaxItem"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            r_oTaxItem = New cTaxParameters()

            ' set interface defaults

            r_oTaxItem.ExcessAmount = m_crExcessAmount

            r_oTaxItem.InsuredDomiciled = chkITDomiciled.CheckState = CheckState.Checked

            r_oTaxItem.InsuredPercentage = gPMFunctions.ToSafeCurrency(txtITPercentage.Text, 0)

            r_oTaxItem.InsuredTaxNumber = txtITTaxNo.Text

            r_oTaxItem.IsSettlement = chkSettlement.CheckState = CheckState.Checked

            r_oTaxItem.IsTaxExempt = chkTaxExempt.CheckState = CheckState.Checked

            r_oTaxItem.IsWHTExempt = chkWHTExempt.CheckState = CheckState.Checked

            r_oTaxItem.Payee = m_sSelectedPayee

            r_oTaxItem.PayeeDomiciled = chkPTDomiciled.CheckState = CheckState.Checked

            r_oTaxItem.PayeePercentage = gPMFunctions.ToSafeCurrency(txtPTPercentage.Text, 0)

            r_oTaxItem.PayeeTaxNumber = txtPTTaxNo.Text


            r_oTaxItem.PaymentToCode = m_sClaimPaymentToCode

            r_oTaxItem.ProcessType = "CLP"


            r_oTaxItem.SafeHarbourCode = m_sSafeHarbourCode

            r_oTaxItem.SafeHarbourPercentage = gPMFunctions.ToSafeCurrency(txtSFPercentage.Text, 0)



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: CreateReserveDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function CreateReserveDetails() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CreateReserveDetails"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' create the relevant reserves if they do not already exist.

            lReturn = m_oBusiness.CreateClaimReserves(v_lClaimPerilId:=m_lWorkClaimPerilId, v_lRiskId:=m_lRiskID, v_lInsuranceFileCnt:=m_lInsuranceFileCnt)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "CreateClaimReserves Failed", gPMConstants.PMELogLevel.PMLogError)
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
    ' Name: ValidPayment
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 26-09-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Public Function ValidPayment() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ValidPayment"

        Dim lReturn As Integer
        Dim crPaymentAmount, crTotalPaymentInPaymentCurrency As Decimal
        Dim sStrippedString As String = ""
        Try
            Dim m_lReturn As gPMConstants.PMEReturnCode
            'No Need to Validate the Payment Just Save the current Changes
            If IsPaymentsReadOnly AndAlso IsThisPaymentMade Then
                Return SaveThisPaymentChanges()
            End If




            result = gPMConstants.PMEReturnCode.PMTrue

            'Need to validate Net Payment, If Zero then STOP - PN 54478
            'Start Renuka PN 61509
            If m_crTotalThisPayment = 0 And m_bCanDoClaimPayment Then
                'End Renuka PN 61509
                MessageBox.Show("Claim Payment cannot be Zero", "Claim Payment Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '<Pankaj PN:39033 validate for m_bOpenClaimNoTrans also >
            If (m_sTransactionType = "C_CP") Or m_bOpenClaimNoTrans Then

                ' only validate if an actual payment has been made in this session
                If m_crTotalThisPayment <> 0 Then

                    ' get the total payment amount in payment currency
                    For Each oPaymentItem As cPaymentItem In m_colPaymentItems
                        ' NB: this payment and excess amount are in payment currency
                        ' so no conversion required
                        crPaymentAmount = oPaymentItem.ThisPayment

                        ' calculate the total amount
                        crTotalPaymentInPaymentCurrency += crPaymentAmount

                    Next oPaymentItem

                    ' Apparently negative payments are allowed so dont stop them being made
                    '            If crTotalPaymentInPaymentCurrency < 0 Then
                    '                MsgBox "Invalid Payment Amount specified", vbExclamation, "Claim Payment Validation"
                    '                ValidPayment = PMFalse
                    '            Else
                    '                ValidPayment = PMTrue
                    '            End If

                    ' if this is a valid payment
                    If result = gPMConstants.PMEReturnCode.PMTrue Then

                        ' if the product the claim is based on requires
                        ' media type to be mandatory
                        If m_bProductMediaTypeMandatory Then

                            ' if a media type hasnt been specified
                            If cboMediaType.SelectedIndex = -1 Then
                                MessageBox.Show("A media type must be specified", "Claim Payment Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                SSTabHelper.SetSelectedIndex(SSTab1, kTabThisPayment)
                                result = gPMConstants.PMEReturnCode.PMFalse
                            End If

                        End If

                    End If
                    If RI2007Enabled And result = gPMConstants.PMEReturnCode.PMTrue Then
                        'Now recovery can be more than payment
                        If (gPMFunctions.ToSafeCurrency(m_crTotalThisPayment) + gPMFunctions.ToSafeCurrency(m_crTotalPaidToDate)) < (gPMFunctions.ToSafeCurrency(m_crTotalReceivedToDate)) Then
                        MessageBox.Show("This payment is less than the total amount received against this claim.", "Cost to claim", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    End If
                    ' if this is a valid payment
                    If result = gPMConstants.PMEReturnCode.PMTrue Then

                        ' if a media type has been specified
                        If cboMediaType.SelectedIndex <> -1 Then

                            ' if the bank details are mandatory
                            If lblBankAccountNo.Font.Bold Then

                                ' ensure all bank details have been set
                                If txtBankName.Text.Trim() = "" Then
                                    MessageBox.Show("Bank Name is required", "Claim Payment Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                    SSTabHelper.SetSelectedIndex(SSTab1, kTabThisPayment)
                                    result = gPMConstants.PMEReturnCode.PMFalse
                                    'Start(Prakash Varghese)PN 59093
                                    If txtBankName.Enabled Then
                                        txtBankName.Focus()
                                    End If
                                    'End(Prakash Varghese)PN 59093
                                    Return result
                                End If

                                If txtBankCode.Text.Trim() = "" Then
                                    MessageBox.Show("Bank Code is required", "Claim Payment Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                    SSTabHelper.SetSelectedIndex(SSTab1, kTabThisPayment)
                                    result = gPMConstants.PMEReturnCode.PMFalse
                                    'Start(Prakash Varghese)PN 59093
                                    If txtBankCode.Enabled Then
                                        txtBankCode.Focus()
                                    End If
                                    'End(Prakash Varghese)PN 59093
                                    Return result
                                End If

                                If txtBankAccountNo.Text.Trim() = "" Then
                                    MessageBox.Show("Bank Account No. is required", "Claim Payment Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                    SSTabHelper.SetSelectedIndex(SSTab1, kTabThisPayment)
                                    result = gPMConstants.PMEReturnCode.PMFalse
                                    'Start(Prakash Varghese)PN 59093
                                    If txtBankAccountNo.Enabled Then
                                        txtBankAccountNo.Focus()
                                    End If
                                    'End(Prakash Varghese)PN 59093
                                    Return result
                                End If


                            End If

                            ' if Payee details are mandatory
                            If lblPayeeName.Font.Bold Then

                                If txtPayeeName.Text.Trim() = "" Then

                                    MessageBox.Show("The Payee Name must be entered.", "Claim Payment Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                                    SSTabHelper.SetSelectedIndex(SSTab1, kTabThisPayment)
                                    result = gPMConstants.PMEReturnCode.PMFalse
                                End If
                            End If

                            'If Cheque Date is mandatory
                            If lblChequeDate.Font.Bold Then
                                If Not Information.IsDate(dtpChequeDate.Value) Then

                                    MessageBox.Show("Cheque Date must be entered.", "Claim Payment Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                                    SSTabHelper.SetSelectedIndex(SSTab1, kTabThisPayment)
                                    result = gPMConstants.PMEReturnCode.PMFalse
                                End If
                            End If

                            'Make sure that account number field is populated
                            If Strings.Len(txtBankAccountNo.Text) > 0 Then
                                m_lReturn = CType(ValidateAccountNumber(), gPMConstants.PMEReturnCode)

                                If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                                    Return gPMConstants.PMEReturnCode.PMFalse
                                End If
                            End If
                            'PK Code End

                        End If
                    End If
                End If
            Else

                If SSTabHelper.GetTabVisible(SSTab1, 1) And m_crTotalThisPayment <> 0 Then

                    m_lReturn = m_oFormFields.CheckMandatoryControls()

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        SSTabHelper.SetSelectedIndex(SSTab1, kTabThisPayment)
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If
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
    ' Name: GetDisplayMode
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function GetDisplayMode(ByVal v_sAdvancedTaxScript As String, ByRef r_lDisplayMode As Integer, ByRef r_bWarningShown As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetDisplayMode"

        Dim lReturn As Integer
        Dim ofrmEditWarning As frmEditWarning

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' if this is not view payment mode
            If Not m_bViewPaymentMode Then

                ' With advanced scripting tax switched on
                ' all manner of issues arise when trying to edit an item
                ' which already has had the advanced tax script ran against
                ' it. Get around these issues by clearing down any tax values
                ' if a payment item with advanced tax is edited.

                ' determine the display mode to be used
                ' the user is given the choice  of whether to view the payment item
                ' or edit it and warned that editing the item will result in all
                ' applied taxes being cleared down
                If v_sAdvancedTaxScript <> "" And m_bAdvancedTaxScriptingOption Then

                    ' create new instance of warning form
                    ofrmEditWarning = New frmEditWarning()

                    ' display warning form
                    ofrmEditWarning.ShowDialog()

                    ' get the display mode to use
                    r_lDisplayMode = ofrmEditWarning.DisplayMode

                    ' show edit warning
                    r_bWarningShown = True

                Else

                    r_lDisplayMode = gPMConstants.PMEComponentAction.PMEdit

                End If

            Else
                ' in view mode always display in view mode
                r_lDisplayMode = gPMConstants.PMEComponentAction.PMView
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
    ''' LoadEditForm
    ''' </summary>
    ''' <param name="v_lDisplayMode"></param>
    ''' <param name="v_bWarningShown"></param>
    ''' <param name="v_oPaymentItem"></param>
    ''' <param name="v_oTaxItem"></param>
    ''' <param name="r_ofrmPaymentDetails"></param>
    ''' <returns></returns>
    ''' <remarks>Created : MEvans : Date : Process ID</remarks>
    Private Function LoadEditForm(ByVal v_lDisplayMode As Integer, ByVal v_bWarningShown As Boolean, ByVal v_oPaymentItem As cPaymentItem, ByVal v_oTaxItem As cTaxParameters, ByRef r_ofrmPaymentDetails As frmPaymentDetails) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "LoadEditForm"
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lPaymentCurrencyFilter As Integer
        Dim oListItem As ListViewItem


        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            ' if this is not view payment mode
            If Not m_bViewPaymentMode Then

                ' only one payment currency is allowed per session
                ' determine if a payment currency filter is required and return it
                lReturn = CType(GetPaymentCurrencyFilter(lPaymentCurrencyFilter), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetPaymentCurrencyFilter Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' set the payment currency filter
                r_ofrmPaymentDetails.PaymentCurrencyFilter = lPaymentCurrencyFilter

            End If


            ' get the selected payment list item
            'oListItem = lvwPaymentDetails.Items.Item(m_lSelectedPayeeDetailIndex - 1)
            oListItem = lvwPaymentDetails.SelectedItems(0)


            '********************
            ' claim risk details
            '********************
            r_ofrmPaymentDetails.IsPostClaimTaxes = m_bPostClaimTax

            '********************
            ' lookup details
            '********************


            r_ofrmPaymentDetails.TaxBandLookup = m_vTaxBand


            r_ofrmPaymentDetails.TaxGroupLookup = m_vTaxGroup

            r_ofrmPaymentDetails.TaxGroupTaxBandLookup = m_vTaxGroupTaxBandDetails


            r_ofrmPaymentDetails.ClassOfBusinessLookup = m_vClassOfBusiness

            '********************
            ' system options
            '********************
            r_ofrmPaymentDetails.AllowNegativeReserve = m_bAllowNegativeReserve
            r_ofrmPaymentDetails.AdvancedTaxScriptOptionOn = m_bAdvancedTaxScriptingOption
            r_ofrmPaymentDetails.ClaimPaymentIsGross = m_bClaimPaymentIsGross
            r_ofrmPaymentDetails.PaymentCannotExceedReserve = m_bPaymentCannotExceedReserve

            '********************
            ' interface details
            '********************
            r_ofrmPaymentDetails.PaymentMethod = m_oPaymentMethod
            r_ofrmPaymentDetails.CurrencyConvert = m_oCurrencyConvert
            r_ofrmPaymentDetails.Business = m_oBusiness
            r_ofrmPaymentDetails.PaymentItem = v_oPaymentItem
            r_ofrmPaymentDetails.TaxItem = v_oTaxItem

            ' either view mode or view chosen by user on warning form
            r_ofrmPaymentDetails.ViewPaymentMode = (v_lDisplayMode = gPMConstants.PMEComponentAction.PMView)
            r_ofrmPaymentDetails.UserId = m_iUserId

            ' use payee domiciled override indicator
            If m_bAdvancedTaxScriptingOption Then
                If m_bSelectedPayeeDomiciledForTax And Trim(txtPTTaxNo.Text) = "" And ToSafeDouble(txtPTPercentage.Text, 0) > 0 Or
                        ((Not m_bSelectedPayeeDomiciledForTax) And Trim(txtITTaxNo.Text) = "" And ToSafeDouble(txtITPercentage.Text, 0) > 0 And UCase(Trim(m_sClaimPaymentToCode)) = "INSURED") Or
                         m_bOpenClaimNoTrans = True Then
                    If UCase(Trim(m_sClaimPaymentToCode)) = "INSURED" Then
                        r_ofrmPaymentDetails.TaxGroupArray = m_vTaxGroupNotWithholdingTax
                    Else
                        r_ofrmPaymentDetails.TaxGroupArray = m_vTaxGroupIsWithholdingTax
                    End If
                Else
                    If UCase(Trim(m_sClaimPaymentToCode)) = "SUPPLIER" _
                                And Trim(txtPTTaxNo.Text) = "" And ToSafeDouble(txtPTPercentage.Text, 0) = 0 _
                                And chkWHTExempt.CheckState = CheckState.Unchecked Then
                        r_ofrmPaymentDetails.TaxGroupArray = m_vTaxGroupIsWithholdingTax
                    Else
                        r_ofrmPaymentDetails.TaxGroupArray = m_vTaxGroupNotWithholdingTax
                    End If
                End If
            Else
                If m_bSelectedPayeeDomiciledForTax Then
                    r_ofrmPaymentDetails.TaxGroupArray = m_vTaxGroupIsWithholdingTax
                Else
                    r_ofrmPaymentDetails.TaxGroupArray = m_vTaxGroupNotWithholdingTax
                End If
            End If


            r_ofrmPaymentDetails.CurrencyArray = m_vCurrencyArray
            r_ofrmPaymentDetails.ClaimPerilId = m_lWorkClaimPerilId

            '********************
            ' payment details
            '********************
            If m_bViewPaymentMode Then
                r_ofrmPaymentDetails.TotalReserve = v_oPaymentItem.TotalReserve
                r_ofrmPaymentDetails.PaidToDate = v_oPaymentItem.PaidToDate
                r_ofrmPaymentDetails.Balance = v_oPaymentItem.Balance
            Else
                r_ofrmPaymentDetails.TotalReserve = CDec(ListViewHelper.GetListViewSubItem(oListItem,
                                                                                           kPayDetailsSubItemsTotalReserve).Text)
                If m_bPostClaimTax Then
                    r_ofrmPaymentDetails.PaidToDate = gPMFunctions.ToSafeCurrency(
                                                      ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsPaidToDate).Text)
                Else
                    r_ofrmPaymentDetails.PaidToDate = gPMFunctions.ToSafeCurrency _
                                                      (ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsPaidToDate).Text) + gPMFunctions.ToSafeCurrency(ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsPaidToDateTax).Text)
                End If

                r_ofrmPaymentDetails.Balance = r_ofrmPaymentDetails.TotalReserve - r_ofrmPaymentDetails.PaidToDate

                ' populate the payment items reserve totals
                v_oPaymentItem.TotalReserve = CDec(ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsTotalReserve).Text)
                v_oPaymentItem.PaidToDate = CDec(ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsPaidToDate).Text)
                v_oPaymentItem.Balance = r_ofrmPaymentDetails.TotalReserve - r_ofrmPaymentDetails.PaidToDate
            End If


            If CStr(IIf(m_sClaimPaymentToCode = Nothing, "", m_sClaimPaymentToCode)).Trim() = "INSURED" And
               chkITDomiciled.CheckState = CheckState.Checked Then
                v_oPaymentItem.IsWithHoldingTax = False
                r_ofrmPaymentDetails.IsWithHoldingTax = False
            ElseIf (CStr(IIf(m_sClaimPaymentToCode = Nothing, "", m_sClaimPaymentToCode)).Trim() = "3RDPARTY" Or
                    CStr(IIf(m_sClaimPaymentToCode = Nothing, "", m_sClaimPaymentToCode)).Trim() = "3PARTY") Then
                v_oPaymentItem.IsWithHoldingTax = Not (chkITDomiciled.CheckState = CheckState.Checked)
                r_ofrmPaymentDetails.IsWithHoldingTax = Not (chkITDomiciled.CheckState = CheckState.Checked)
                Else
                v_oPaymentItem.IsWithHoldingTax = Not m_bSelectedPayeeDomiciledForTax
                r_ofrmPaymentDetails.IsWithHoldingTax = Not m_bSelectedPayeeDomiciledForTax
            End If

            r_ofrmPaymentDetails.LossCurrency = m_sLossCurrency
            r_ofrmPaymentDetails.LossCurrencyID = m_lLossCurrencyId
            r_ofrmPaymentDetails.RiskType = m_sRisktype
            r_ofrmPaymentDetails.ReserveType = ListViewHelper.GetListViewSubItem(oListItem,
                                                                                 kPayDetailsSubItemsTypeDesc).Text
            r_ofrmPaymentDetails.ClaimSourceId = m_lClaimSourceId
            r_ofrmPaymentDetails.ClaimId = m_lWorkClaimID
            r_ofrmPaymentDetails.ClaimPayableAccountId = m_lClaimPayableAccountId
            r_ofrmPaymentDetails.PayeePartyId = m_lSelectedPayeeId

            '********************
            ' payment item details
            '********************
            r_ofrmPaymentDetails.AdvancedTaxScript = v_oPaymentItem.AdvancedTaxScript
            r_ofrmPaymentDetails.ReserveId = CInt(ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsTypeId).Text)
            r_ofrmPaymentDetails.PaymentToLossXRate = v_oPaymentItem.PaymentToLossXRate
            r_ofrmPaymentDetails.CurrencyId = v_oPaymentItem.CurrencyId
            r_ofrmPaymentDetails.ThisPayment = v_oPaymentItem.ThisPayment
            r_ofrmPaymentDetails.TaxGroupId = v_oPaymentItem.TaxGroupId

            If v_oPaymentItem.TaxBandRateArray IsNot Nothing AndAlso TypeOf v_oPaymentItem.TaxBandRateArray Is Object(,) Then
                r_ofrmPaymentDetails.TaxBandRateArray = v_oPaymentItem.TaxBandRateArray
            Else
                r_ofrmPaymentDetails.TaxBandRateArray = Nothing
            End If
            r_ofrmPaymentDetails.IsExcess = v_oPaymentItem.IsExcess

            'PN: 49813
            r_ofrmPaymentDetails.TaxAmountWHT = v_oPaymentItem.TaxAmountWHT
            r_ofrmPaymentDetails.TaxAmount = v_oPaymentItem.TaxAmount

            r_ofrmPaymentDetails.TaxGroupDescription = v_oPaymentItem.TaxGroupDescription
            r_ofrmPaymentDetails.CurrencyDescription = v_oPaymentItem.CurrencyDescription
            r_ofrmPaymentDetails.TotalTaxPaid = 0

            r_ofrmPaymentDetails.TotalUnAuthorisedClaimPayment = m_cUnAuthorisedClaimPayment
            r_ofrmPaymentDetails.MaxUnAuthorisedClaimPayment = m_cMaxUnAuthorisedClaimPaymentValue

            If Not v_oPaymentItem.IsExcess Then
                ' reset the tax details
                ' if the warning form was shown and the user selected to continue
                ' with editing the item
                If v_lDisplayMode = gPMConstants.PMEComponentAction.PMEdit And v_bWarningShown Then
                    r_ofrmPaymentDetails.TotalTaxPaid = gPMFunctions.ToSafeCurrency(r_ofrmPaymentDetails.TaxAmount) + gPMFunctions.ToSafeCurrency(r_ofrmPaymentDetails.TaxAmountWHT)
                    r_ofrmPaymentDetails.TaxAmount = 0
                    r_ofrmPaymentDetails.TaxAmountWHT = 0
                    r_ofrmPaymentDetails.TaxGroupDescription = ""
                    r_ofrmPaymentDetails.TaxGroupId = 0

                    r_ofrmPaymentDetails.TaxBandRateArray = Nothing
                End If
            End If


        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



            ' DO Not Call any functions before here or the error will be lost
        End Try
        Return result


    End Function

    ' ***************************************************************** '
    ' Name: ProcessEditForm
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function ProcessEditForm(ByRef r_oPaymentItem As cPaymentItem, ByRef r_ofrmPaymentDetails As frmPaymentDetails) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessEditForm"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lNoOfPaymentItems As Integer
        Dim vNewData(,) As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' if this is the first item being added to the collection
            lReturn = CType(GetNoOfPaymentsItems(lNoOfPaymentItems), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "", gPMConstants.PMELogLevel.PMLogError)
            End If

            If lNoOfPaymentItems = 1 Then
                ' disable payee details frame
                ' so none of the payment or tax details can be
                ' changed once a payment item has been added;
                ' as changing these details could invalidate the
                ' tax amount calculated on this item.
                lReturn = CType(EnableDisablePayeeDetails(v_bEnabled:=False), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "EnableDisablePayeeDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            ' save form details back to the payment
            lReturn = CType(SaveFormDetailsToPayment(r_oPaymentItem:=r_oPaymentItem, r_ofrmPaymentDetails:=r_ofrmPaymentDetails), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SaveFormDetailsToPayment Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' process interface changes
            lReturn = CType(ProcessInterfaceUpdates(r_oPaymentItem:=r_oPaymentItem), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "ProcessInterfaceUpdates Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If m_bOpenClaimNoTrans Then
                ReDim vNewData(2, 0)

                vNewData(0, 0) = r_oPaymentItem.ReserveId

                vNewData(1, 0) = r_oPaymentItem.ThisPayment

                vNewData(2, 0) = r_oPaymentItem.ThisReserveRevision

                RaiseEvent DataHasChanged(Me, New DataHasChangedEventArgs(vNewData))
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
    ' Name: SaveFormDetailsToPayment
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function SaveFormDetailsToPayment(ByRef r_oPaymentItem As cPaymentItem, ByRef r_ofrmPaymentDetails As frmPaymentDetails) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SaveFormDetailsToPayment"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' save the details from the form back to the payment item
            r_oPaymentItem.ThisPayment = r_ofrmPaymentDetails.ThisPayment '+ r_ofrmPaymentDetails.PaymentAdjustment
            r_oPaymentItem.CurrencyCode = r_ofrmPaymentDetails.CurrencyCode
            r_oPaymentItem.CurrencyId = r_ofrmPaymentDetails.CurrencyId
            r_oPaymentItem.IsExcess = r_ofrmPaymentDetails.IsExcess
            r_oPaymentItem.TaxGroupDescription = r_ofrmPaymentDetails.TaxGroupDescription
            r_oPaymentItem.CurrencyDescription = r_ofrmPaymentDetails.CurrencyDescription
            r_oPaymentItem.ReserveId = r_ofrmPaymentDetails.ReserveId
            r_oPaymentItem.TaxGroupId = r_ofrmPaymentDetails.TaxGroupId

            If r_ofrmPaymentDetails.TaxBandRateArray IsNot Nothing AndAlso TypeOf r_ofrmPaymentDetails.TaxBandRateArray Is Object(,) Then
                r_oPaymentItem.TaxBandRateArray = r_ofrmPaymentDetails.TaxBandRateArray
            Else
                r_oPaymentItem.TaxBandRateArray = Nothing
            End If

            r_oPaymentItem.AdvancedTaxScript = r_ofrmPaymentDetails.AdvancedTaxScript
            r_oPaymentItem.PaymentAdjustment = r_ofrmPaymentDetails.PaymentAdjustment

            ' if this is the first item to be created then
            ' get exchanges rates from the form
            ' otherwise keep the default values set when adding the new item
            If m_colPaymentItems.Count = 1 Or r_oPaymentItem.PaymentToLossXRate = 0 Then
                r_oPaymentItem.AccountToBaseDate = r_ofrmPaymentDetails.AccountToBaseDate
                r_oPaymentItem.AccountToBaseXRate = r_ofrmPaymentDetails.AccountToBaseXRate
                r_oPaymentItem.CurrencyToBaseDate = r_ofrmPaymentDetails.CurrencyToBaseDate
                r_oPaymentItem.CurrencyToBaseXRate = r_ofrmPaymentDetails.CurrencyToBaseXRate
                r_oPaymentItem.PaymentToLossXRate = r_ofrmPaymentDetails.PaymentToLossXRate
                r_oPaymentItem.SystemToBaseDate = r_ofrmPaymentDetails.SystemToBaseDate
                r_oPaymentItem.SystemToBaseXRate = r_ofrmPaymentDetails.SystemToBaseXRate
                r_oPaymentItem.ExchangeRateOverrideReasonId = r_ofrmPaymentDetails.ExchangeRateOverrideReasonId
            Else
                If r_oPaymentItem.PaymentToLossXRate = 0 Then
                    r_oPaymentItem.PaymentToLossXRate = 1
                End If
            End If

            ' excess rows dont have any associated tax entries
            ' so ignore tax fields for excess rows
            If Not r_oPaymentItem.IsExcess Then
                ' if advanced tax script is not set then get the tax amounts
                ' from the form otherwise they are detemined by a calculation
                ' of the tax band rate array items...
                r_oPaymentItem.TaxAmount = r_ofrmPaymentDetails.TaxAmount
                r_oPaymentItem.TaxAmountWHT = r_ofrmPaymentDetails.TaxAmountWHT
                r_oPaymentItem.RecalculateTaxAmounts()
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
    ' Name: ProcessInterfaceUpdates
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function ProcessInterfaceUpdates(ByRef r_oPaymentItem As cPaymentItem) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessInterfaceUpdates"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oListItem As ListViewItem

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get the selected payment list item
            oListItem = lvwPaymentDetails.Items.Item(m_lSelectedPayeeDetailIndex - 1)

            ' populate "this payment" details back to the the payment details listview
            lReturn = CType(PopulatePaymentDetailThisPayment(r_oPaymentItem, oListItem), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "PopulatePaymentDetailThisPayment Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' if this is an excess row - get the new values for the modular level excess fields
            ' e.g total excess, excess specified, excess supplied etc
            If r_oPaymentItem.IsExcess Then
                ' get excess details to modular fields
                lReturn = GetExcessDetails()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetExcessDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            ' populate this payment details
            lReturn = PopulateThisPaymentDetails()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "PopulateThisPaymentDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' populate total line
            lReturn = PopulateTotals()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "PopulateTotals Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' set up this payment form
            lReturn = SetupThisPaymentInterface()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetupThisPaymentInterface", gPMConstants.PMELogLevel.PMLogError)
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
    ' Name: GetMediaType
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 17-10-2005 : Process ID
    ' ***************************************************************** '
    Private Function GetMediaType() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetMediaType"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get media types

            lReturn = m_oBusiness.GetMediaTypes(r_vResults:=m_vMediaType, iPaymentsOnly:=1)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetMediaTypes", gPMConstants.PMELogLevel.PMLogError)
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
    ' Name: ProcessMediaTypeValidation
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function ProcessMediaTypeValidation() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessMediaTypeValidation"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lMediaTypeId As Integer
        Dim sMediaTypeValidationCode As String = ""
        Dim bBankFieldsMandatory As Boolean
        Dim llBound, lUBound As Integer
        Dim bIsValidationEnabled As Boolean

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get the selected claim payment to option
            lMediaTypeId = VB6.GetItemData(cboMediaType, cboMediaType.SelectedIndex)

            ' get array boundaries
            llBound = m_vMediaType.GetLowerBound(1)
            lUBound = m_vMediaType.GetUpperBound(1)

            ' for each claim payment to option
            For lItem As Integer = llBound To lUBound

                ' if the option matches the selected one
                If CDbl(m_vMediaType(kLookupItemId, lItem)) = lMediaTypeId Then

                    ' get the payee option details
                    sMediaTypeValidationCode = CStr(m_vMediaType(kLookupMediaTypeValidationCode, lItem)).Trim()
                    If CStr(m_vMediaType(kLookupMediaTypeIsValidationEnabled, lItem)).Trim() = "1" Then
                        bIsValidationEnabled = True
                    End If
                    Exit For
                End If

            Next

            ' setup bank or Cheque fields
            If bIsValidationEnabled Then
                lReturn = CType(SetupMandatoryFields(sMediaTypeValidationCode), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "SetupBankFields Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            Else
                lblBankAccountNo.Font = VB6.FontChangeBold(lblBankAccountNo.Font, gPMConstants.PMEReturnCode.PMFalse)
                lblBankCode.Font = VB6.FontChangeBold(lblBankCode.Font, gPMConstants.PMEReturnCode.PMFalse)
                lblBankName.Font = VB6.FontChangeBold(lblBankName.Font, gPMConstants.PMEReturnCode.PMFalse)

                lblBankAccountNo.Left = txtBankAccountNo.Left - lblBankAccountNo.Width - VB6.TwipsToPixelsX(75)
                lblBankCode.Left = txtBankCode.Left - lblBankCode.Width - VB6.TwipsToPixelsX(75)
                lblBankName.Left = txtBankName.Left - lblBankName.Width - VB6.TwipsToPixelsX(75)
                lblBIC.Left = txtBIC.Left - lblBIC.Width - VB6.TwipsToPixelsX(75)
                lblPayeeName.Font = VB6.FontChangeBold(lblPayeeName.Font, False)
                lblChequeDate.Font = VB6.FontChangeBold(lblChequeDate.Font, gPMConstants.PMEReturnCode.PMFalse)
                lblPayeeName.Left = txtPayeeName.Left - lblPayeeName.Width - VB6.TwipsToPixelsX(75)
                lblChequeDate.Left = dtpChequeDate.Left - lblChequeDate.Width - VB6.TwipsToPixelsX(75)

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
    ' Name: SetupBankFields
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function SetupMandatoryFields(ByVal sMediaTypeValidation As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupMandatoryFields"

        Dim lReturn As Integer
        Dim bBankMandatory, bChequeMandatory As Boolean

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            bBankMandatory = (sMediaTypeValidation = "BANK")
            bChequeMandatory = (sMediaTypeValidation = "CHEQUE")


            lblBankAccountNo.Font = VB6.FontChangeBold(lblBankAccountNo.Font, bBankMandatory)
            lblBankCode.Font = VB6.FontChangeBold(lblBankCode.Font, bBankMandatory)
            lblBankName.Font = VB6.FontChangeBold(lblBankName.Font, bBankMandatory)

            lblBankAccountNo.Left = txtBankAccountNo.Left - lblBankAccountNo.Width - VB6.TwipsToPixelsX(75)
            lblBankCode.Left = txtBankCode.Left - lblBankCode.Width - VB6.TwipsToPixelsX(75)
            lblBankName.Left = txtBankName.Left - lblBankName.Width - VB6.TwipsToPixelsX(75)
            lblBIC.Left = txtBIC.Left - lblBIC.Width - VB6.TwipsToPixelsX(75)
            lblPayeeName.Font = VB6.FontChangeBold(lblPayeeName.Font, bChequeMandatory)
            lblChequeDate.Font = VB6.FontChangeBold(lblChequeDate.Font, bChequeMandatory)
            lblPayeeName.Left = txtPayeeName.Left - lblPayeeName.Width - VB6.TwipsToPixelsX(75)
            lblChequeDate.Left = dtpChequeDate.Left - lblChequeDate.Width - VB6.TwipsToPixelsX(75)



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: GetClaimBranchCurrencies
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 17-11-2005 : Process ID
    ' ***************************************************************** '
    Private Function GetClaimBranchCurrencies(ByVal v_lSourceId As Integer, ByRef r_vResults As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimBranchCurrencies"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            lReturn = m_oBusiness.GetClaimBranchCurrencies(v_lSourceId:=v_lSourceId, r_vResults:=r_vResults)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetClaimBranchCurrencies Failed", gPMConstants.PMELogLevel.PMLogError)
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
    ' Name: ValidatePayeeDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 06-12-2005 : PN26185
    ' ***************************************************************** '
    Public Function ValidatePayeeDetails(ByRef r_bPayeeDetailsValid As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ValidatePayeeDetails"

        Dim lReturn As Integer
        Dim bPayeeDetailsValid As Boolean

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' assume all details are present and correct
            bPayeeDetailsValid = True

            ' if advanced tax scripting is switched on then
            ' perform advanced tax scripting field validation
            If m_bAdvancedTaxScriptingOption Then

                If chkITDomiciled.CheckState = CheckState.Checked Then

                    If bPayeeDetailsValid Then
                        If txtITPercentage.Text = "" Then
                            MessageBox.Show("Percentage not supplied", "Insurer Tax Adjustment Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            txtITPercentage.Focus()
                            bPayeeDetailsValid = False
                        End If
                    End If

                    If bPayeeDetailsValid Then
                        If txtITTaxNo.Text = "" And Conversion.Val(txtPTPercentage.Text) > 0 Then
                            MessageBox.Show("Tax number not supplied", "Insurer Tax Adjustment Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            txtITTaxNo.Focus()
                            bPayeeDetailsValid = False
                        End If
                    End If
                End If

                If chkPTDomiciled.CheckState = CheckState.Checked Then

                    If bPayeeDetailsValid Then
                        If txtPTPercentage.Text = "" Then
                            MessageBox.Show("Percentage not supplied", "Payee Tax Adjustment Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            txtPTPercentage.Focus()
                            bPayeeDetailsValid = False
                        End If
                    End If

                    If bPayeeDetailsValid Then
                        If txtPTTaxNo.Text = "" And Conversion.Val(txtPTPercentage.Text) > 0 And Not m_bTaxWarningShown Then
                            If MessageBox.Show("The Tax number has not been supplied so withholding tax will be " &
                                               "applied." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & "Continue?", "Payee Tax " &
                                               "Adjustment Validation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = System.Windows.Forms.DialogResult.No Then
                                txtPTTaxNo.Focus()
                                bPayeeDetailsValid = False
                            Else
                                m_bTaxWarningShown = True
                            End If

                        End If
                    End If
                End If
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            r_bPayeeDetailsValid = bPayeeDetailsValid

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetNoOfPaymentsItems
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function GetNoOfPaymentsItems(ByRef r_lNoOfPaymentItems As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetNoOfPaymentsItems"

        Dim lReturn, nItems As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' ensure passed value is initialised
            r_lNoOfPaymentItems = 0

            ' no of items in the collection
            nItems = m_colPaymentItems.Count

            ' for each item in the collection
            For lItem As Integer = 1 To nItems

                ' determine if it is not an excess item
                ' and therefore is a payment item

                If Not m_colPaymentItems(lItem).IsExcess Then

                    ' increment item counter
                    r_lNoOfPaymentItems += 1
                End If
            Next


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
    ' Name: ResetPayments
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 12-01-2006 : Process ID
    ' ***************************************************************** '
    Private Function ResetPaymentsOnly(Optional ByVal v_lIncludeExcessItemId As Integer = 0) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ResetPayments"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lNoOfPaymentItems As Integer
        Dim oListItem As ListViewItem

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' determine how many payment lines there are
            lReturn = CType(GetNoOfPaymentsItems(lNoOfPaymentItems), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetNoOfPaymentsItems Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' if there is at least one payment line
            If lNoOfPaymentItems > 0 Or v_lIncludeExcessItemId <> 0 Then

                ' for each item in the collection
                For Each oPayment As cPaymentItem In m_colPaymentItems

                    ' if the item is not an excess line
                    If Not oPayment.IsExcess Or oPayment.ReserveId = v_lIncludeExcessItemId Then

                        m_colPaymentItems.Remove(CStr(oPayment.ReserveId))

                    End If

                Next oPayment


                ' clear down all this payment entries in the listview
                For lItem As Integer = 1 To lvwPaymentDetails.Items.Count

                    oListItem = lvwPaymentDetails.Items.Item(lItem - 1)

                    ' if the item is not a spacer
                    If oListItem.Text <> kLVWSpacer Then

                        ' if the item is not an excess line
                        ' or its an excess line we have specified that we want to clear down
                        If Not gPMFunctions.ToSafeBoolean(ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsIsExcess).Text, False) Or gPMFunctions.ToSafeLong(ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsTypeId).Text, 0) = v_lIncludeExcessItemId Then

                            ' set excess items details
                            If gPMFunctions.ToSafeBoolean(ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsIsExcess).Text, False) Then
                                ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsThisPaymentInclTax).Text = "0.00" 'kLVSCExcessNotSet
                                ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsThisPaymentTax).Text = kLVSCNotApplicable
                                ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsCostToClaim).Text = StringsHelper.Format(0, "0.00")
                            Else
                                ' reset this payment details
                                ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsThisPaymentInclTax).Text = StringsHelper.Format(0, "0.00")
                                ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsThisPaymentTax).Text = StringsHelper.Format(0, "0.00")
                                ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsCostToClaim).Text = StringsHelper.Format(0, "0.00")
                            End If

                        End If

                    End If

                    lReturn = PopulateTotals()
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "PopulateTotals Failed", gPMConstants.PMELogLevel.PMLogError)
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
    Private Sub uctPartyBankCombo1_ComboChange(ByVal Sender As Object, ByVal e As uctPartyBank.uctPartyBankCombo.ComboChangeEventArgs) Handles uctPartyBankCombo1.ComboChange
        Dim lSelItemID As Integer = e.lSelItemID
        If lSelItemID > 0 Then
            FillPartyBankDetails()
            If uctPartyBankCombo1.SelectedPartyCnt > 0 Then

                m_vBankPaymentTypeId = uctPartyBankCombo1.SelectedPartyCnt
            Else


                m_vBankPaymentTypeId = DBNull.Value
            End If
            LockBankControls(False)
        Else
            If lSelItemID = 0 And m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                m_lReturn = ClearFields(1)
            End If

            m_vBankPaymentTypeId = ""
            LockBankControls(True)
        End If
        If m_iTask = gPMConstants.PMEComponentAction.PMView Then
            LockBankControls(False)
        End If
    End Sub

    Private Sub uctPartyBankCombo1_EditPartyBankItem(ByVal Sender As Object, ByVal e As uctPartyBank.uctPartyBankCombo.EditPartyBankItemEventArgs) Handles uctPartyBankCombo1.EditPartyBankItem
        Dim vBankDetails As Object = e.vBankDetails
        If Information.IsArray(vBankDetails) Then
            m_vPartyBankDetails = vBankDetails
            FillPartyBankDetails()
        End If
    End Sub

    'Private Sub uctPartyBankCombo1_AddPartyBankItem(ByVal Sender As Object, ByVal e As uctPartyBankCombo.AddPartyBankItemEventArgs) Handles uctPartyBankCombo1.AddPartyBankItem
    Private Sub uctPartyBankCombo1_AddPartyBankItem(ByVal Sender As Object, ByVal e As uctPartyBank.uctPartyBankCombo.AddPartyBankItemEventArgs) Handles uctPartyBankCombo1.AddPartyBankItem
        m_lReturn = GetPartyBanks()

        FillPartyBankDetails()
    End Sub
    Public Function GetPartyBanks() As Integer
        Dim result As Integer = 0
        Dim bSIRPartyBank As Object

        Const kMethodName As String = "GetPartyBanks"

        Dim lReturn As Integer

        Dim oPartyBank As Object
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oPartyBank As Object
            m_lReturn = m_oObjectManager.GetInstance(temp_oPartyBank, "bSIRPartyBank.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPartyBank = temp_oPartyBank

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Raise Error.
                gPMFunctions.RaiseError("FillPartyBankCombo", "Unable to get instance of  bSIRPartyBank.Business")
            End If



            'Developer Guide No. 105
            m_lReturn = oPartyBank.GetPartyBankDetails(vPartyCnt:=m_lSelectedPayeeId, vPartyBankDetails:=m_vPartyBankDetails, vAccountID:=Nothing)

            If m_lReturn = gPMConstants.PMEReturnCode.PMError Then

            End If

            m_lAccountID = ENPartyBank.AccountID


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

    Private Sub UserControl_Terminate()
        If Not (m_oSirMediaTypeValidation Is Nothing) Then

            m_oSirMediaTypeValidation.Dispose()
            m_oSirMediaTypeValidation = Nothing
        End If
    End Sub


    ' ***************************************************************** '
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields.
    '
    ' ***************************************************************** '
    Public Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            'Payee Name
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPayeeName, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Media Type
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboMediaType, lFieldType:=gPMConstants.PMEDataType.PMLong, lFormat:=gPMConstants.PMEFormatStyle.PMFormatLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function SetUserControlDefaults() As gPMConstants.PMEReturnCode
        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            ' setup user control
            m_lReturn = SetUpUserControl()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("SetUserControlDefaults", "SetUserControlDefaults Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetUserControlDefaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetUserControlDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateReserveItems
    ' Description: Updates On-Screen reserves items from Reserve control
    ' History:
    '           Created : Prabodh : 31-10-2007 : Open Claim No Trans
    ' ***************************************************************** '
    Public Sub UpdateReserveItems(ByRef NewData(,) As Object)

        Const kMethodName As String = "UpdateReserveItems"

        Dim lReturn As gPMConstants.PMEReturnCode

        Dim llBound, lUBound As Integer
        Dim bFound As Boolean
        Dim lReserveIndex As Integer
        Dim r_oPaymentItem As cPaymentItem = Nothing

        Try


            If Information.IsArray(m_vReserveAndPaymentDetails) And Information.IsArray(NewData) Then

                ' determine array boundaries
                llBound = m_vReserveAndPaymentDetails.GetLowerBound(1)
                lUBound = m_vReserveAndPaymentDetails.GetUpperBound(1)

                ' for each reserve / recovery type
                For lItem As Integer = llBound To lUBound
                    bFound = False
                    For lReserveIndex = NewData.GetLowerBound(1) To NewData.GetUpperBound(1)

                        If NewData(0, lReserveIndex).Equals(m_vReserveAndPaymentDetails(kPaymentDetailsTypeId, lItem)) Then
                            bFound = True
                            Exit For
                        End If
                    Next lReserveIndex

                    If bFound Then


                        m_vReserveAndPaymentDetails(kPaymentDetailsTotalReserve, lItem) = CDbl(NewData(2, lReserveIndex)) + CDbl(NewData(5, lReserveIndex))

                        m_vReserveAndPaymentDetails(kPaymentDetailsCurrentReserve, lItem) = NewData(5, lReserveIndex)

                        If m_colPaymentItems.Count = 0 Then

                            lReturn = CType(AddPaymentItem(CStr(NewData(0, lReserveIndex))), gPMConstants.PMEReturnCode)
                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError(kMethodName, "AddPaymentItem Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If

                            r_oPaymentItem = m_colPaymentItems.Item(NewData(0, lReserveIndex))
                        Else
                            Try
                                r_oPaymentItem = m_colPaymentItems.Item(NewData(0, lReserveIndex))
                            Catch ex As Exception


                            End Try
                            If r_oPaymentItem Is Nothing Then
                                ' add the payment item

                                lReturn = CType(AddPaymentItem(NewData(0, lReserveIndex)), gPMConstants.PMEReturnCode)
                                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    gPMFunctions.RaiseError(kMethodName, "AddPaymentItem Failed", gPMConstants.PMELogLevel.PMLogError)
                                End If

                                r_oPaymentItem = m_colPaymentItems.Item(NewData(0, lReserveIndex))
                            End If
                        End If

                        r_oPaymentItem.ThisReserveRevision = CDec(NewData(5, lReserveIndex))

                            If r_oPaymentItem.PaymentToLossXRate = 0 Then
            r_oPaymentItem.PaymentToLossXRate = 1
        End If
        If r_oPaymentItem.AccountToBaseXRate = 0 Then
            r_oPaymentItem.AccountToBaseXRate = 1
        End If
        If r_oPaymentItem.CurrencyToBaseXRate = 0 Then
            r_oPaymentItem.CurrencyToBaseXRate = 1
        End If
        If r_oPaymentItem.SystemToBaseXRate = 0 Then
            r_oPaymentItem.SystemToBaseXRate = 1
        End If

        m_vReserveAndPaymentDetails(kPaymentDetailsPaidToDate, lItem) = r_oPaymentItem.ThisPayment
        'm_vReserveAndPaymentDetails(kPaymentDetailsPaidToDateTax, lItem) = r_oPaymentItem.TaxAmount
        'm_vReserveAndPaymentDetails(kPaymentDetailsPaidToDateTaxWHT, lItem) = r_oPaymentItem.TaxAmountWHT
        m_vReserveAndPaymentDetails(kPaymentDetailsCostToClaim, lItem) = r_oPaymentItem.ThisPayment
        r_oPaymentItem = Nothing

        '    crThisPaymentTax = (r_oPaymentItem.TaxAmount + r_oPaymentItem.TaxAmountWHT) * r_oPaymentItem.PaymentToLossXRate
        '    crThisPaymentInclTax = (r_oPaymentItem.ThisPayment * r_oPaymentItem.PaymentToLossXRate) + crThisPaymentTax
        '    crCostToClaim = crThisPaymentInclTax - crThisPaymentTax

        End If
        Next lItem
        PopulatePaymentDetailsListView()
        End If


        Catch ex As Exception

        If Information.Err().Number = 5 Then
            'Resume Next
        End If
        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lReturn, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally


        End Try
    End Sub

    Private Function GetProductDetails() As Integer
        Dim result As Integer = 0
        Dim bSIRProduct As Object

        Const kMethodName As String = "GetProductDetails"

        Dim lReturn As gPMConstants.PMEReturnCode

        Dim o_ProductBusiness As Object
        Dim vProductDetails As Object
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_o_ProductBusiness As Object
            lReturn = m_oObjectManager.GetInstance(temp_o_ProductBusiness, "bSIRProduct.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            o_ProductBusiness = temp_o_ProductBusiness
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetInstance of bSIRProduct.Business Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            'lReturn = o_ProductBusiness.GetProductDetails(v_lProductID:=m_lProductId, _
            'r_vResultArray:=vProductDetails)
            If m_lClaimId = 0 And m_lWorkClaimID <> 0 Then
                m_lClaimId = m_lWorkClaimID
            End If

            lReturn = o_ProductBusiness.GetProductLevelOptionsForClaim(v_lClaimID:=m_lClaimId, r_bIs_Multiple_claims_payments:=m_bMultipleClaimPayments, r_cMax_unauthorised_claim_value:=m_cMaxUnAuthorisedClaimPaymentValue, r_iMax_unauthorised_no_claim_payments:=m_iMaxNoofUnAuthorisedClaimPayments, r_bRun_authorisation_scripts_claim_payments:=m_bRunAuthorisationScriptsForClaimPayments, r_bIs_Advanced_Tax_Script_Enabled:=m_bAdvancedTaxScriptingOption, r_bAllow_Negative_Reserve:=m_bAllowNegativeReserve, r_bIs_Gross_Claim_Payment_Amount:=m_bClaimPaymentIsGross, r_bPaymentCannotExceedReserve:=m_bPaymentCannotExceedReserve)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetProductDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally




        End Try
        Return result
    End Function

    'Party Bank Details
    Private Function EnableDisablePartyCombo() As Integer
        Dim result As Integer = 0
        Dim bSIRPartyBank As Object
        Const kMethodName As String = "EnableDisablePartyCombo"

        Dim oPartyBank As Object
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                'developer guide no.248
                If Convert.ToString(m_vBankPaymentTypeId) = "" Then
                    m_vBankPaymentTypeId = 0
                End If
                If CDbl(m_vBankPaymentTypeId) > 0 Then
                    'PN: 48518
                    uctPartyBankCombo1.SelectedPaymentID = gPMFunctions.ToSafeLong(m_vBankPaymentTypeId)
                End If
            End If
            ''Saurabh
            If txtParty.Text <> "" And m_iTask <> PMEComponentAction.PMView Then

                Dim temp_oPartyBank As Object
                m_lReturn = m_oObjectManager.GetInstance(temp_oPartyBank, "bSIRPartyBank.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oPartyBank = temp_oPartyBank

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Raise Error.
                    gPMFunctions.RaiseError("FillPartyBankCombo", "Unable to get instance of  bSIRPartyBank.Business")
                End If

                'Developer Guide No. 105
                m_lReturn = oPartyBank.GetPartyBankDetails(vPartyCnt:=m_lSelectedPayeeId, vPartyBankDetails:=m_vPartyBankDetails, vAccountID:=Nothing)

                If m_lReturn = gPMConstants.PMEReturnCode.PMError Then
                    gPMFunctions.RaiseError(kMethodName, "GetPartyBankDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                'If m_lAccountID <> m_lSelectedPayeeId Then

                uctPartyBankCombo1.IsBank = 1

                If m_lSelectedPayeeId = 0 And m_iTask = gPMConstants.PMEComponentAction.PMView Then
                    ' Start - Sankar - PN 61335
                    If OptAgent.Checked Then

                        uctPartyBankCombo1.PartyCnt = m_lLeadAgentId
                    ElseIf OptParty.Checked Then

                        uctPartyBankCombo1.PartyCnt = m_lOtherPartyCnt
                    Else

                        uctPartyBankCombo1.PartyCnt = m_lClientId
                    End If
                    ' End - Sankar - PN 61335
                Else

                    uctPartyBankCombo1.PartyCnt = m_lSelectedPayeeId
                End If
                uctPartyBankCombo1.BankPaymentTypeCode = "CLM"
                If m_sTransactionType = "C_CP" Then
                    uctPartyBankCombo1.Task = 1
                Else
                    uctPartyBankCombo1.Task = 0
                End If

                'Developer Guide No. 10
                uctPartyBankCombo1.Initialise()

                uctPartyBankCombo1.PopulateScreen()
                m_lPartyBankCount = 1

            End If

        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally

        End Try
        Return result
    End Function

    ''' <summary>
    ''' FillPartyBankDetails
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function FillPartyBankDetails() As Integer
        Dim result As Integer = PMEReturnCode.PMTrue
        Const kMethodName As String = "FillPartyBankDetails"

        Dim vPartyBankId, vIsBank, vAccountId As Object
        Dim vBankPaymentTypeId As String = ""
        Dim vBankAccountType, vAccountHolderName, vAccountNumber As Object
        Dim vBankNameId As String = ""
        Dim vBankName As String = ""
        Dim vBankBranch, vBankBranchCode, vBankAddress1, vBankAddress2, vBankAddress3, vBankTown, vBankPostCode, vBankRegion As Object
        Dim vBankCountry As String = ""
        Dim vCardNumber, vCardStart, vCardExpiryDate, vCardIssueNumber, vCardPin, vCardIsRegistered, vCardAddress1, vCardAddress2, vCardAddress3, vCardTown, vCardPostCode As Object
        Dim vCardCountry As String = ""
        Dim vIsDeleted As Object
        Dim iMatchFound As Boolean
        Dim lPartyBankID As Integer
        Dim sBIC As String = String.Empty
        Dim sIBAN As String = String.Empty

        Try
            If (Information.IsArray(m_vPartyBankDetails)) And m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                lPartyBankID = uctPartyBankCombo1.SelectedPartyCnt

                If Information.IsArray(m_vPartyBankDetails) Then
                    For lPaymentCount As Integer = 0 To m_vPartyBankDetails.GetUpperBound(1)
                        If CDbl(m_vPartyBankDetails(ENPartyBank.PartyBankId, lPaymentCount)) = lPartyBankID Then

                            vPartyBankId = m_vPartyBankDetails(ENPartyBank.PartyBankId, lPaymentCount)

                            vIsBank = m_vPartyBankDetails(ENPartyBank.IsBank, lPaymentCount)

                            vAccountId = m_vPartyBankDetails(ENPartyBank.AccountID, lPaymentCount)
                            vBankPaymentTypeId = CStr(m_vPartyBankDetails(ENPartyBank.BankPaymentTypeId, lPaymentCount)(ENPMLookups.Id))
                            vBankAccountType = m_vPartyBankDetails(ENPartyBank.BankAccountTypeId, lPaymentCount)

                            vAccountHolderName = m_vPartyBankDetails(ENPartyBank.AccountHolderName, lPaymentCount)

                            vAccountNumber = m_vPartyBankDetails(ENPartyBank.AccountNumber, lPaymentCount)
                            If m_vPartyBankDetails(ENPartyBank.BankNameId, lPaymentCount).Length = 0 Then
                                vBankNameId = ""
                                vBankName = ""
                            Else
                                vBankNameId = CStr(m_vPartyBankDetails(ENPartyBank.BankNameId, lPaymentCount)(ENPMLookups.Id))
                                vBankName = CStr(m_vPartyBankDetails(ENPartyBank.BankNameId, lPaymentCount)(ENPMLookups.Description))
                            End If

                            vBankBranch = m_vPartyBankDetails(ENPartyBank.BankBranch, lPaymentCount)

                            vBankBranchCode = m_vPartyBankDetails(ENPartyBank.BankBranchCode, lPaymentCount)
                            sBIC = m_vPartyBankDetails(ENPartyBank.BIC, lPaymentCount)
                            sIBAN = m_vPartyBankDetails(ENPartyBank.IBAN, lPaymentCount)
                            vBankAddress1 = m_vPartyBankDetails(ENPartyBank.BankAdd1, lPaymentCount)

                            vBankAddress2 = m_vPartyBankDetails(ENPartyBank.BankAdd2, lPaymentCount)

                            vBankAddress3 = m_vPartyBankDetails(ENPartyBank.BankAdd3, lPaymentCount)

                            vBankTown = m_vPartyBankDetails(ENPartyBank.BankTown, lPaymentCount)

                            vBankPostCode = m_vPartyBankDetails(ENPartyBank.BankPCode, lPaymentCount)

                            vBankRegion = m_vPartyBankDetails(ENPartyBank.BankRegion, lPaymentCount)
                            vBankCountry = CStr(m_vPartyBankDetails(ENPartyBank.BankCountry, lPaymentCount)(ENPMLookups.Id))

                            vCardNumber = m_vPartyBankDetails(ENPartyBank.CCNum, lPaymentCount)

                            vCardStart = m_vPartyBankDetails(ENPartyBank.CCstartdate, lPaymentCount)

                            vCardExpiryDate = m_vPartyBankDetails(ENPartyBank.CCexpirydate, lPaymentCount)

                            vCardIssueNumber = m_vPartyBankDetails(ENPartyBank.CCIssueNum, lPaymentCount)

                            vCardPin = m_vPartyBankDetails(ENPartyBank.CCpin1, lPaymentCount)

                            vCardIsRegistered = m_vPartyBankDetails(ENPartyBank.IsRegistered, lPaymentCount)

                            vCardAddress1 = m_vPartyBankDetails(ENPartyBank.CCAdd1, lPaymentCount)

                            vCardAddress2 = m_vPartyBankDetails(ENPartyBank.CCAdd2, lPaymentCount)

                            vCardAddress3 = m_vPartyBankDetails(ENPartyBank.CCAdd3, lPaymentCount)

                            vCardTown = m_vPartyBankDetails(ENPartyBank.CCTown, lPaymentCount)

                            vCardPostCode = m_vPartyBankDetails(ENPartyBank.CCPCode, lPaymentCount)
                            vCardCountry = CStr(m_vPartyBankDetails(ENPartyBank.CCCountry, lPaymentCount)(ENPMLookups.Id))

                            vIsDeleted = m_vPartyBankDetails(ENPartyBank.IsDeleted, lPaymentCount)
                            iMatchFound = True
                        End If
                    Next

                    If iMatchFound Then


                        If CStr(vIsBank) = "1" Then
                            m_lReturn = ClearFields(1)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError(kMethodName, "ClearFields Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If

                            txtPayeeName.Text = CStr(vAccountHolderName)

                            txtBankAccountNo.Text = CStr(vAccountNumber)
                            txtBankName.Text = vBankName

                            txtBankCode.Text = CStr(vBankBranchCode)
                            txtBIC.Text = NullToString(sBIC)
                            txtIBAN.Text = NullToString(sIBAN)
                            uctPMAddressControl1.AddressLine1 = CStr(vBankAddress1)

                            uctPMAddressControl1.AddressLine2 = CStr(vBankAddress2)

                            uctPMAddressControl1.AddressLine3 = CStr(vBankAddress3)

                            uctPMAddressControl1.AddressLine4 = CStr(vBankTown)

                            uctPMAddressControl1.PostCode = CStr(vBankPostCode)
                            uctPMAddressControl1.CountryId = CInt(vBankCountry)
                            LockBankControls(False)
                        End If
                    End If
                Else
                    txtPayeeName.Text = ""

                    txtBankAccountNo.Text = ""
                    txtBankName.Text = ""

                    txtBankCode.Text = ""

                    uctPMAddressControl1.AddressLine1 = ""

                    uctPMAddressControl1.PostCode = ""
                    uctPMAddressControl1.CountryId = 0
                    LockBankControls(False)
                End If
                uctPMAddressControl1.AddressLine2 = ""

                uctPMAddressControl1.AddressLine3 = ""

                uctPMAddressControl1.AddressLine4 = ""

            End If



        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally




        End Try
        Return result
    End Function

    Private Function ClearFields(ByRef bClearBankFields As Boolean) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "ClearFields"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue
            'For Bank
            If bClearBankFields Then
                txtPayeeName.Text = ""
                txtBankAccountNo.Text = ""
                txtBankName.Text = ""
                txtBankCode.Text = ""
                txtBIC.Text = ""
                txtIBAN.Text = ""
                uctPMAddressControl1.AddressLine1 = ""
                uctPMAddressControl1.AddressLine2 = ""
                uctPMAddressControl1.AddressLine3 = ""
                uctPMAddressControl1.AddressLine4 = ""
                uctPMAddressControl1.PostCode = ""
            End If



        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetPaymentGridInArray
    ' Description: Returns Payment Details to be manipulated through logic scripts
    ' vPaymentArray 0 Payment_id (for matching)
    '               1 Payment Type
    '               2 Revision
    ' ***************************************************************** '
    Public Function GetPaymentGridInArray(ByRef vPaymentArray(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPaymentGridInArray"
        Dim iNoofRecoveryLines As Integer
        Try


            result = gPMConstants.PMEReturnCode.PMTrue
            'Do we Have Recoveries
            For cnt As Integer = 0 To m_vReserveAndPaymentDetails.GetUpperBound(1)
                If CDbl(m_vReserveAndPaymentDetails(0, cnt)) = 2 Then
                    iNoofRecoveryLines += 1
                End If
            Next

            If Information.IsArray(m_vReserveAndPaymentDetails) Then
                ReDim vPaymentArray(8, m_vReserveAndPaymentDetails.GetUpperBound(1) - iNoofRecoveryLines)
                For cnt As Integer = 0 To m_vReserveAndPaymentDetails.GetUpperBound(1)
                    'filter out recoveries
                    If CDbl(m_vReserveAndPaymentDetails(kPaymentDetailsType, cnt)) = 1 Then

                        vPaymentArray(0, cnt) = m_vReserveAndPaymentDetails(kPaymentDetailsTypeId, cnt)

                        vPaymentArray(1, cnt) = m_vReserveAndPaymentDetails(kPaymentDetailsReserveDescription, cnt)
                    End If
                Next cnt
            End If

            Return result

        Catch ex As Exception


            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)


            'Tidy Up code goes here
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SaveScriptArrayToPayment
    ' Description: Save Payment Details to Payment control (for logic scripts)
    ' vPaymentArray 0 Payment_id (for matching)
    '               1 Payment Type
    '               2 Revision
    ' ***************************************************************** '
    Public Function SaveScriptArrayToPayment(ByVal vPaymentArray(,) As Object) As Integer
        Dim iPAyeeType, iReadOnly As Integer
        Dim vTaxArray(,) As Object

        Dim result As Integer = 0
        Const kMethodName As String = "SaveScriptArrayToPayment"
        Dim r_oPaymentItem As cPaymentItem
        Dim bExists As Boolean
        Dim sAccountType As String = ""
        Try


            result = gPMConstants.PMEReturnCode.PMTrue
            If Not m_bFastTrackEnabled Then
                Return result
            End If

            If Information.IsArray(vPaymentArray) And m_sTransactionType = "C_CP" Then
                'Update listview array
                For cntr As Integer = 0 To vPaymentArray.GetUpperBound(1)
                    If gPMFunctions.ToSafeCurrency(vPaymentArray(2, cntr)) <> 0 Then

                        ItemExistsIncollection(bExists, gPMFunctions.ToSafeLong(vPaymentArray(kPaymentDetailsType, cntr)))
                        If bExists Then Return result

                        AddPaymentItem(CStr(vPaymentArray(kPaymentDetailsType, cntr)))
                        r_oPaymentItem = m_colPaymentItems(vPaymentArray(kPaymentDetailsType, cntr))
                        r_oPaymentItem.ThisPayment = gPMFunctions.ToSafeLong(vPaymentArray(2, cntr))
                        r_oPaymentItem.PaymentToLossXRate = 1


                        If m_bSelectedPayeeDomiciledForTax And txtPTTaxNo.Text.Trim() = "" And gPMFunctions.ToSafeDouble(txtPTPercentage.Text, 0) > 0 Then

                            vTaxArray = VB6.CopyArray(m_vTaxGroupIsWithholdingTax)
                        Else

                            vTaxArray = VB6.CopyArray(m_vTaxGroupNotWithholdingTax)
                        End If
                        If Information.IsArray(vTaxArray) Then

                            For iVar As Integer = 0 To vTaxArray.GetUpperBound(1)

                                If CStr(vTaxArray(2, iVar)) = gPMFunctions.ToSafeString(vPaymentArray(3, 0)) Then
                                    r_oPaymentItem.TaxGroupId = gPMFunctions.ToSafeInteger(vTaxArray(0, iVar))
                                End If
                            Next
                        End If
                        'Media Type
                        For iVar As Integer = 0 To m_vMediaType.GetUpperBound(1)
                            If CStr(m_vMediaType(2, iVar)).Trim() = gPMFunctions.ToSafeString(vPaymentArray(6, 0)) Then
                                SelectcboItem(cboMediaType, CInt(m_vMediaType(0, iVar)))
                                Exit For
                            End If
                        Next

                        iPAyeeType = gPMFunctions.ToSafeInteger(vPaymentArray(4, 0))
                        Select Case iPAyeeType
                            Case 1
                                OptClaimPayable.Checked = 1
                            Case 2
                                OptParty.Checked = 1
                                txtParty.Text = gPMFunctions.ToSafeString(vPaymentArray(5, 0))
                                GetPartyDetails()
                            Case 3
                                If OptAgent.Enabled Then
                                    OptAgent.Checked = 1
                                    'txtParty.Text = ToSafeString(vPaymentArray(5, 0))
                                End If
                            Case 4
                                OptClient.Checked = 1
                        End Select

                        m_lSelectedPayeeDetailIndex = cntr + 2
                        ProcessInterfaceUpdates(r_oPaymentItem)

                        m_bShowPaymentDetailsHiddenMode = True

                        If CStr(vPaymentArray(kPaymentDetailsType, cntr)) = ListViewHelper.GetListViewSubItem(lvwPaymentDetails.Items.Item(cntr + 1), kPaymentDetailsTypeId).Text Then
                            ActionEdit()
                        End If
                        If SSTabHelper.GetTabVisible(SSTab1, kTabThisPayment) And SSTabHelper.GetSelectedIndex(SSTab1) <> kTabThisPayment Then
                            SSTabHelper.SetSelectedIndex(SSTab1, kTabThisPayment)
                        End If



                        sAccountType = CStr(vPaymentArray(7, 0))

                        If gPMFunctions.ToSafeLong(m_vBankPaymentTypeId) > 0 Then

                            uctPartyBankCombo1.SelectedPaymentID = CInt(m_vBankPaymentTypeId)
                        End If
                        If sAccountType <> "" Then
                            If Information.IsArray(m_vPartyBankDetails) Then
                                For iVar As Integer = 0 To m_vPartyBankDetails.GetUpperBound(1)
                                    If CStr(m_vPartyBankDetails(6, iVar)).Trim() = sAccountType Then
                                        uctPartyBankCombo1.SelectedPaymentID = CInt(m_vPartyBankDetails(2, iVar))
                                        Exit For
                                    End If
                                Next
                            End If
                        End If
                        'Read Only
                        iReadOnly = gPMFunctions.ToSafeInteger(vPaymentArray(8, 0))
                        If iReadOnly = 1 Then
                            fraPayee.Enabled = False
                            fraPaymentDetails.Enabled = False
                            fraInsuredTaxAdjustment.Enabled = False
                            SSTab2.Enabled = False
                        End If
                    End If
                Next cntr
                If SSTabHelper.GetTabVisible(SSTab1, kTabThisPayment) Then
                    SSTabHelper.SetSelectedIndex(SSTab1, kTabThisPayment)
                End If

            End If

            Return result

        Catch ex As Exception


            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)


            'Tidy Up code goes here
            Return result
        End Try
    End Function

    Private Function ItemExistsIncollection(ByRef bExists As Boolean, ByRef lReserveID As Integer) As Integer
        Dim result As Integer = 0
        Try


            bExists = False
            For iVar As Integer = 1 To m_colPaymentItems.Count

                If m_colPaymentItems(1).ReserveId = lReserveID Then
                    bExists = True
                    Exit For
                End If
            Next


        Catch ex As Exception
            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="kMethodName", r_lFunctionReturn:=result, excep:=ex)

        Finally

        End Try
        Return result
    End Function
    Private Function GetClaimWorkFlow() As Integer
        Dim result As Integer = 0
        Dim bSIRProduct As Object

        Const kMethodName As String = "GetClaimWorkFlow"

        Dim lReturn As Integer


        Dim oProductBusiness As Object
        Dim vResults As Object
        Dim lWorkflowId As Integer
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If m_lProductId > 0 Then
                If lWorkflowId = 0 Then
                    'set workflowId if not already set to push into roadmap
                    If m_sTransactionType = "C_CO" Then
                        lWorkflowId = gPMConstants.PMWorkflowOpenClaim
                    ElseIf m_sTransactionType = "C_CR" Then
                        lWorkflowId = gPMConstants.PMWorkflowMaintainClaim
                    ElseIf m_sTransactionType = "C_CP" Then
                        lWorkflowId = gPMConstants.PMWorkflowPayClaim
                    Else
                        'View Claim

                        Return result
                    End If
                End If
            Else
                gPMFunctions.RaiseError(kMethodName, "SetupClaimWorkflow Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            Dim temp_oProductBusiness As Object
            lReturn = m_oObjectManager.GetInstance(temp_oProductBusiness, "bSIRProduct.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oProductBusiness = temp_oProductBusiness
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetInstance of bSIRProduct.Business Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            lReturn = oProductBusiness.GetClaimWorkflow(r_vResults:=vResults, v_lProductId:=m_lProductId, v_lWorkflowID:=lWorkflowId)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetupClaimWorkflow Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not Information.IsArray(vResults) Then
                gPMFunctions.RaiseError(kMethodName, "SetupClaimWorkflow Failed to return any data", gPMConstants.PMELogLevel.PMLogError)
            Else
                m_bFastTrackEnabled = gPMFunctions.ToSafeBoolean(vResults(gPMConstants.EClaimWorkflowId.EFast_Track_Claims, 0))
                m_bCash_Payment_Process = gPMFunctions.ToSafeBoolean(vResults(gPMConstants.EClaimWorkflowId.ECash_Payment_process, 0))

            End If



        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally
            If Not (oProductBusiness Is Nothing) Then
                ' terminate this instance of the navigator process

                oProductBusiness.Dispose()
                ' clean up the object instances
                oProductBusiness = Nothing
            End If




        End Try
        Return result
    End Function
    Private Sub LockBankControls(ByRef bLock As Boolean)
        txtPayeeName.Enabled = bLock
        txtBankAccountNo.Enabled = (bLock)
        txtBankName.Enabled = (bLock)
        txtBankCode.Enabled = (bLock)
        txtBIC.Enabled = (bLock)
        txtIBAN.Enabled = (bLock)
        uctPMAddressControl1.Enabled = bLock
    End Sub

    Private Function GetPartyDetails() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "GetPartyDetails"
        Const iMaxSearchDetails As Integer = 500
        Dim vResultArray(,) As Object

        Try



            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Get an instance of the business object via the public object manager.
            Dim temp_m_oBusinessParty As Object
            m_lReturn = m_oObjectManager.GetInstance(temp_m_oBusinessParty, "bSIRFindParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusinessParty = temp_m_oBusinessParty
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetInstance of bSIRFindParty.Business Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            m_lReturn = m_oBusinessParty.SearchByQuery(r_lNumberOfRecords:=iMaxSearchDetails, r_vResultArray:=vResultArray, v_vShortName:=txtParty.Text)

            If Information.IsArray(vResultArray) Then

                txtParty.Text = CStr(vResultArray(2, 0))

                m_lOtherPartyCnt = CInt(vResultArray(0, 0))

                m_lSelectedPayeeId = m_lOtherPartyCnt
                m_sSelectedPayee = txtParty.Text

                m_lReturn = GetOtherPartyDetails()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetOtherPartyDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            Else
                txtParty.Text = ""
                OptClaimPayable.Checked = True
            End If




        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)




        End Try
        Return result
    End Function

    ''' <summary>
    ''' ValidateAccountNumber
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ValidateAccountNumber() As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim bSirMediaTypeValidation As Object

        Const kMethodName As String = "ValidateAccountNumber"

        Try

            If AlphanumericValidation(Trim(txtBIC.Text)) <> PMEReturnCode.PMTrue Then
                MessageBox.Show("Only alphanumeric characters allowed in BIC field.", "Bank Validation",
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return PMEReturnCode.PMFalse
            End If

            If AlphanumericValidation(Trim(txtIBAN.Text)) <> PMEReturnCode.PMTrue Then
                MessageBox.Show("Only alphanumeric characters allowed in IBAN field.", "Bank Validation",
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return PMEReturnCode.PMFalse
            End If

            Dim oSirMediaTypeValidation As Object
            Dim bValid As Boolean
            Dim sStrippedString As String = ""

            Dim sBankName, sAddress1, sAddress2, sAddress3, sAddress4, sPostalCode As String
            Dim vValidationMessage As Object
            Dim bValidationOverridable As Boolean
            Dim oObjectManager As bObjectManager.ObjectManager
            Dim vlMediaID As Integer

            ' Create an instance of the object manager and Initialise
            oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = oObjectManager.Initialise(sCallingAppName:=ACApp)

            Dim temp_oSirMediaTypeValidation As Object
            'developer guide no.(Changed instance name according to the dll)
            m_lReturn = oObjectManager.GetInstance(temp_oSirMediaTypeValidation, "bSIRMediaTypeValidation.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oSirMediaTypeValidation = temp_oSirMediaTypeValidation
            If cboMediaType.SelectedIndex >= 0 Then
                vlMediaID = VB6.GetItemData(cboMediaType, cboMediaType.SelectedIndex)
            End If
            'TR PN5080- Append the Account Number to the Sort Code field. Do
            'not check the Sort code field as some customers will put the sort code into
            'the account field (IAG), but for other (i.e. GB) customers the sort
            'code goes into it's own field. So sort code can be blank.
            'Strip the Spaces from the SortCode & AccountNumber before Validation
            sStrippedString = txtBankCode.Text.Replace(" ", "") & "|" &
                              txtBankAccountNo.Text.Replace(" ", "")
            'TR - Perform the validation

            oSirMediaTypeValidation.ValidateNumber(vlMediaID, oObjectManager.CountryID,
                                                   sStrippedString, bValid, sBankName,
                                                   sAddress1, sAddress2, sAddress3, sAddress4,
                                                   sPostalCode, vValidationMessage, bValidationOverridable, "",
                                                   sBIC:=Trim(txtBIC.Text), sIBAN:=Trim(txtIBAN.Text))


            Dim sMessage, IsValid As String
            If Not bValid Then
                If Information.IsArray(vValidationMessage) Then

                    For iErrCount As Integer = 0 To vValidationMessage.GetUpperBound(0)

                        sMessage = sMessage & Strings.Chr(13) & Strings.Chr(10) & CStr(vValidationMessage(iErrCount))
                    Next
                Else
                    'if there is no message then store the generic message
                    sMessage = "Bank details have failed validation"
                End If

                'if validation are overridable then show the message with vbYesNo
                If bValidationOverridable Then
                    sMessage = sMessage & Strings.Chr(13) & Strings.Chr(10) & "Do you wish to override the bank validation?"
                    IsValid = CStr(MessageBox.Show(sMessage, "Bank Validation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
                    If IsValid = System.Windows.Forms.DialogResult.Yes Then
                        txtBankName.Text = sBankName
                        uctPMAddressControl1.AddressLine1 = sAddress1
                        uctPMAddressControl1.AddressLine2 = sAddress2
                        uctPMAddressControl1.AddressLine3 = sAddress3
                        uctPMAddressControl1.AddressLine4 = sAddress4
                        uctPMAddressControl1.PostCode = sPostalCode
                        uctPMAddressControl1.CountryId = oObjectManager.CountryID
                        nResult = gPMConstants.PMEReturnCode.PMTrue
                    Else
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If
                ElseIf Not bValidationOverridable Then
                    MessageBox.Show(sMessage, "Bank Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    txtBankName.Text = sBankName
                    uctPMAddressControl1.AddressLine1 = sAddress1
                    uctPMAddressControl1.AddressLine2 = sAddress2
                    uctPMAddressControl1.AddressLine3 = sAddress3
                    uctPMAddressControl1.AddressLine4 = sAddress4
                    uctPMAddressControl1.PostCode = sPostalCode
                    uctPMAddressControl1.CountryId = oObjectManager.CountryID
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    'cmdOk.Enabled = False
                End If
            ElseIf bValid Then
                'developer guide no.(Added check for nothing)
                If Not sPostalCode Is Nothing Then
                    If sPostalCode.Trim() <> "" Then
                        txtBankName.Text = sBankName
                        uctPMAddressControl1.AddressLine1 = sAddress1
                        uctPMAddressControl1.AddressLine2 = sAddress2
                        uctPMAddressControl1.AddressLine3 = sAddress3
                        uctPMAddressControl1.AddressLine4 = sAddress4
                        uctPMAddressControl1.PostCode = sPostalCode
                        uctPMAddressControl1.CountryId = oObjectManager.CountryID
                    End If
                End If
                nResult = gPMConstants.PMEReturnCode.PMTrue
            End If

            oSirMediaTypeValidation = Nothing
            '   Terminate the object Manager
            oObjectManager.Dispose()
            ' Destroy the instance of the object manager from memory
            oObjectManager = Nothing

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return nResult
    End Function

    Private Sub _SSTab1_TabPage0_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _SSTab1_TabPage0.Click

    End Sub
    'developer guide no.11
    Private Sub lvwPaymentDetails_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwPaymentDetails.SelectedIndexChanged
        If Not IsNothing(lvwPaymentDetails.SelectedItems) Then
            If lvwPaymentDetails.SelectedItems.Count > 0 Then
                ActionSelectPaymentItem(lvwPaymentDetails.SelectedItems.Item(0))
            End If
        End If
    End Sub


    Private Function ValidateClaimRIXOL() As Long

        Const kMethodName As String = "ValidateClaimRIXOL"
        Dim iresult As Integer = 0
        Dim oClaimRI As Object
        Dim bClaimHasXOLLines As Boolean
        Try
            iresult = gPMConstants.PMEReturnCode.PMTrue

            If m_bRI2007Enabled = True And m_iNoofReferredPayments > 0 Then
                iresult = m_oObjectManager.GetInstance(oObject:=oClaimRI,
                        sClassName:="bCLMReinsuranceRI2007.Form",
                        vInstanceManager:=PMGetViaClientManager)

                If (iresult <> gPMConstants.PMEReturnCode.PMTrue) Then
                    ' Raise Error.
                    gPMFunctions.RaiseError(kMethodName, "Unable to get instance of  bCLMReinsuranceRI2007.Form")
                End If

                iresult = oClaimRI.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate, bOpenClaimNoTrans:=m_bOpenClaimNoTrans)
                If iresult <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed to set the process modes for the m_oClaimRI object")
                End If

                oClaimRI.Recovery = False
                oClaimRI.ActualRecovery = False
                oClaimRI.ClaimId = m_lClaimId

                iresult = oClaimRI.CalculateRI
                If iresult <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("ValidateClaimRIXOL", "Unable to auto calculate reinsurance")
                End If

                iresult = m_oBusiness.GetClaimXOLineCount(v_lClaimId:=m_lClaimId, r_bHaveXOLLines:=bClaimHasXOLLines)
                If iresult <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetClaimXOLineCount Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
                If bClaimHasXOLLines = True Then
                    m_bClaimHasXOLLines = True
                Else
                    m_bClaimHasXOLLines = False
                End If

                oClaimRI = Nothing
            End If
            Return iresult
        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=iresult, excep:=ex)
            Return iresult
            ' If you want to rollback a transaction or something, do it here
        End Try

    End Function

    Private Sub chkIsExGratia_CheckedChanged(sender As Object, e As EventArgs) Handles chkIsExGratia.CheckedChanged
        Dim bIsExists As Boolean
        Dim result As Integer = 0

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            If chkIsExGratia.Checked Then
                m_lReturn = CType(iPMFunc.GetSystemOption(v_iOptionNumber:=kSysOptionPaymentPExGratiaAccount, r_sOptionValue:=sExGratiaAccount, v_iSourceID:=m_iSourceID), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = m_oBusiness.IsAccountExists(sExGratiaAccount, bIsExists)

                If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If

                If bIsExists <> True Then
                    MessageBox.Show("Ex-gratia Expense Account does not exist. Please contact your System Administrator.", "Incorrect Ex-gratia Account", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    chkIsExGratia.Checked = False
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="chkIsExGratia_CheckedChanged", r_lFunctionReturn:=result, excep:=ex)
        End Try

    End Sub

    ''' <summary>
    ''' Called on KeyPress event of textox to validate only alphanumeric input
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub AlphanumericValidation(sender As Object, e As KeyPressEventArgs) Handles txtBIC.KeyPress, txtIBAN.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(e.KeyChar)
        If (KeyAscii >= 48 AndAlso KeyAscii <= 57) OrElse
            (KeyAscii >= 65 AndAlso KeyAscii <= 90) OrElse
            (KeyAscii >= 97 And KeyAscii <= 122) _
            OrElse KeyAscii = 8 OrElse KeyAscii = 127 Then
        Else
            KeyAscii = 0
        End If
        If KeyAscii = 0 Then
            e.Handled = True
        End If
        e.KeyChar = Convert.ToChar(KeyAscii)
    End Sub


    ''' <summary>
    ''' Called to validate alphanumeric validation via Copy/Paste 
    ''' </summary>
    ''' <param name="sInput"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function AlphanumericValidation(ByVal sInput As String) As Integer
        If System.Text.RegularExpressions.Regex.IsMatch(sInput, "^[a-zA-Z0-9]*$") Then
            Return PMEReturnCode.PMTrue
        Else
            Return PMEReturnCode.PMFalse
        End If
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ValidatePaymentReadOnly()
        'THIS SUBROUTINE WILL WORK ONLY IF PAYMENT IS SET READ ONLY IN PRODUCT OPTIONS
        If Not (IsPaymentsReadOnly AndAlso m_sTransactionType = "C_CP") Then
            Exit Sub
        End If

        fraClaimInformation.Enabled = False
        fraPayee.Enabled = False
        fraInsuredTaxAdjustment.Enabled = False
        fraPayeeTaxAdjustments.Enabled = False
        fraSafeHarbour.Enabled = False
        fraExemptions.Enabled = False
        fraSettlement.Enabled = False
        cmdDelete.Enabled = False
        cmdReserveEdit.Enabled = False
        cmdEdit.Enabled = False

        'IF PAYMENT HAS BEEN DONE
        If IsThisPaymentMade Then
            SSTab1.SelectTab(1)

            txtMediaType.Visible = False
            cboMediaType.Visible = True
            cboMediaType.Enabled = True
            txtMediaRef.Enabled = True
            txtPayeeName.Enabled = True
            SSTab2.Enabled = True
            fraTaxesOnPayments.Enabled = True
            If GetMediaType() = gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(PopulateCombo(cboMediaType, m_vMediaType), gPMConstants.PMEReturnCode)
            End If

            If IsPaymentsReadOnly AndAlso m_sTransactionType = "C_CP" Then
                SelectcboItem(cboMediaType, gPMFunctions.ToSafeLong(m_vClaimPaymentDetails(kClaimPaytDetPayeeMediaType, 0)))
            End If
        End If

    End Sub
    ''' <summary>
    ''' GetIsPaymentsReadOnly
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetIsPaymentsReadOnly() As Boolean
        Dim nProductID As Integer
        Dim oProduct As bSIRProduct.Business = Nothing
        Dim oIsPaymentsReadonly(,) As Object = Nothing
        Dim bReturnValue As Boolean = False
        If m_lInsuranceFileCnt = 0 Then
            Return bReturnValue
        End If

        m_lReturn = m_oObjectManager.GetInstance(oProduct, "bSIRProduct.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to get Product Business  object", ACApp, ACClass, "GetIsPaymentsReadOnly", Information.Err().Number, Information.Err().Description)
            Return False
        End If

        m_lReturn = oProduct.GetProductid(ifilecnt:=m_lInsuranceFileCnt, vProduct_id:=nProductID)
        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            m_lReturn = oProduct.GetProductValue(nProductID, "is_Payments_read_only", oIsPaymentsReadonly)
        End If

        If IsArray(oIsPaymentsReadonly) AndAlso ToSafeString(oIsPaymentsReadonly(0, 0)) = "1" Then
            bReturnValue = True
        End If

        oProduct.Dispose()
        oProduct = Nothing

        Return bReturnValue

    End Function

    ''' <summary>
    ''' SaveThisPaymentChanges
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SaveThisPaymentChanges() As Integer
        Dim nReturn As Integer = gPMConstants.PMEReturnCode.PMTrue
        If m_lWorkClaimID = 0 AndAlso m_lWorkClaimPaymentId = 0 Then
            Return nReturn
        End If
        nReturn = m_oBusiness.UpdateThisClaimPaymentDetails(nClaimID:=m_lWorkClaimID, nClaimPaymentID:=m_lWorkClaimPaymentId,
                                        nPayeeMediaType:=cboMediaType.SelectedIndex,
                                        smedia_ref:=txtMediaRef.Text,
                                        sPayeeName:=txtPayeeName.Text,
                                        sPayeeBankName:=txtBankName.Text,
                                        sPayeeSortCode:=txtBankCode.Text,
                                        sPayeeAccountNo:=txtBankAccountNo.Text,
                                        nPayeeCountry:=uctPMAddressControl1.CountryId,
                                        sPayeeComments:=txtPayeeComments.Text,
                                        sPayeeAddress1:=uctPMAddressControl1.AddressLine1,
                                        sPayeeAddress2:=uctPMAddressControl1.AddressLine2,
                                        sPayeeAddress3:=uctPMAddressControl1.AddressLine3,
                                        sPayeeAddress4:=uctPMAddressControl1.AddressLine4,
                                        sPayeePostalCode:=uctPMAddressControl1.PostCode,
                                        sThirdPartyReference:=txtThirdPartyReference.Text,
                                        sOur_ref:=txtOurReference.Text)

        Return nReturn

    End Function

End Class
