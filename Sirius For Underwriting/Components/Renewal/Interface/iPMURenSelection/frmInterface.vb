Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.Windows.Forms
Imports SharedFiles
Imports bSIRRenSelection

Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Class Name: frmInterface
    '
    ' Date:
    '
    ' Description:
    '
    ' Edit History:
    '       MEvans : 01-12-2004 : PN16797
    '               Updated so that all rules (renewal, authority, nb) are run
    '                regardless of any referral/decline reasons.
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"
    Private Const ACSilentRenewal As Integer = 1
    Private Const vbFormCode As Integer = 0
    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sStepStatus As String = ""
    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMURenSelection.General
    ' Declare an instance of the Business object.
    Private m_oBusiness As Object
    Private m_oInsuranceFile As Object
    'RKS PN13438
    Private m_oInsuranceFileBusiness As Object
    Private m_oRenewalSelectionBusiness As Object
    'Start(Saurabh Agrawal) Tech Spec VAL P14 Policy numbering (5.3.2)
    Private m_oPolicyNumMaint As Object
    'End(Saurabh Agrawal) Tech Spec VAL P14 Policy numbering (5.3.2)

    Private m_oReinsurance As Object
    Private m_oTax As Object
    Private m_oRiskData As Object
    Private m_oPerilAllocation As Object
    Private m_oAgentCommission As Object
    Private m_oChangePolicyStatus As Object
    Private m_bManualRenewalsExist As Boolean
    Private m_bAutoRenewalsExist As Boolean
    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields
    ' Control array to store the first and last
    ' text box controls for each tab.
    'Private m_ctlTabFirstLast( ,  ) As Control
    Private m_ctlTabFirstLast(,) As Control
    ' Stores the return value for the a function call.
    Private m_lReturn As Integer

    Private m_vProductID As Integer

    'Report
    Private m_iPrintMode As Integer
    Private m_lRenewalMode As Integer
    Private m_lInsFolderCnt As Integer 'RWH(15/05/01)
    Private m_bCalledFromLocalForm As Boolean
    'Thinh Nguyen 19/03/2002 (start)
    Private m_vSourceID As Integer
    'Thinh Nguyen 19/03/2002 (end)
    Private m_bExtraRiskDetails As Boolean 'SET (10/6/02)
    'RKS PN13438
    Private m_bUnderwritingYearID As Boolean 'Is option switched on/off

    Private m_lPolicyVersionIncrement As Integer

    Private m_obSIRRenewal As Object
    Private bolActivated As Boolean
    Private bIsValid As Boolean
    Private oBatchRenewalBusiness As bSIRRenewalBusiness
    ' ***************************************************************** '
    '
    ' Name: UsesExtraRiskData
    '
    ' Description: Reads registry value for 'ExtraRiskDetails' in
    '              'HKEY_LOCAL_MACHINE\SOFTWARE\PM\SiriusSolutions\Server'
    '              and sets m_bExtraRiskDetails to true if this value is 1.
    '              This is used to decide whether to execute RSA specific code.
    '
    ' Hist : SET 10/06/2002 - function created
    '
    ' ***************************************************************** '
    Private Function UseExtraRiskData() As Integer
        Dim result As Integer = 0
        Dim sFile As String = ""
        Dim m_lReturn As gPMConstants.PMEReturnCode

        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily

        Try

            ' set default values
            m_bExtraRiskDetails = False
            result = gPMConstants.PMEReturnCode.PMTrue
            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = g_sProductFamily
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLServer

            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="ExtraRiskData", r_sSettingValue:=sFile), gPMConstants.PMEReturnCode)

            'if we have a valid return
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                If Conversion.Val(sFile) = 1 Then
                    m_bExtraRiskDetails = True
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error in UseExtraRiskData", vApp:=ACApp, vClass:=ACClass, vMethod:="UseExtraRiskData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public WriteOnly Property RenewalMode() As Integer
        Set(ByVal Value As Integer)
            m_lRenewalMode = Value
        End Set
    End Property

    Public ReadOnly Property ErrorNumber() As Integer
        Get

            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber

        End Get
    End Property
    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property


    Public Property Status() As Integer
        Get

            ' Return the interface exit status.
            Return m_lStatus

        End Get
        Set(ByVal Value As Integer)

            ' set the interface exit status.
            m_lStatus = Value

        End Set
    End Property

    Public Property Task() As Integer
        Get

            Return m_iTask

        End Get
        Set(ByVal Value As Integer)

            m_iTask = Value

        End Set
    End Property

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)

            m_lNavigate = Value

        End Set
    End Property

    Public Property StepStatus() As String
        Get

            Return m_sStepStatus

        End Get
        Set(ByVal Value As String)

            m_sStepStatus = Value

        End Set
    End Property
    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)

            m_lProcessMode = Value

        End Set
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)

            m_sTransactionType = Value

        End Set
    End Property

    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)

            m_dtEffectiveDate = Value

        End Set
    End Property

    ' ***************************************************************** '
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields.
    '
    ' ***************************************************************** '
    Public Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse


            Return m_oFormFields.AddNewFormField(ctlControl:=cboProductCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: BusinessToInterface
    '
    ' Description: Updates all interface details from the business
    '              object.
    '
    ' ***************************************************************** '
    Public Function BusinessToInterface() As Integer


        Dim result As Integer = 0
        Return gPMConstants.PMEReturnCode.PMTrue



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: InterfaceToBusiness
    '
    ' Description: Updates all business members from the interface
    '              details.
    '
    ' ***************************************************************** '
    Public Function InterfaceToBusiness() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the details from the interface to the data storage.
            If InterfaceToData() <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            StatusBar1.Items.Item("Count").Text = ""
            StatusBar1.Items.Item("Policy").Text = ""
            StatusBar1.Items.Item("Message").Text = " Processing Renewals"
            Me.StatusBar1.Refresh()
            Application.DoEvents()


            If Not (dtStartDate.Checked) Then
                m_lReturn = ProcessRenewalSelection(v_dtEndDate:=CDate(DateTime.FromOADate(dtEndDate.Value.ToOADate()).ToString("dd-MMM-yyyy") & " 23:59:59"), v_vProductID:=m_vProductID, v_vSourceID:=m_vSourceID)
            Else
                m_lReturn = ProcessRenewalSelection(v_dtEndDate:=CDate(DateTime.FromOADate(dtEndDate.Value.ToOADate()).ToString("dd-MMM-yyyy") & " 23:59:59"), v_vProductID:=m_vProductID, v_vSourceID:=m_vSourceID, v_vStartDate:=CDate(DateTime.FromOADate(dtStartDate.Value.ToOADate()).ToString("dd-MMM-yyyy") & " 00:00:00"))
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
            End If

            StatusBar1.Items.Item("Count").Text = ""
            Me.StatusBar1.Refresh()
            StatusBar1.Items.Item("Policy").Text = ""
            Me.StatusBar1.Refresh()
            StatusBar1.Items.Item("Message").Text = " Complete"
            Me.StatusBar1.Refresh()
            Application.DoEvents()

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ProcessRenewalSelection
    '
    ' Description:
    '
    ' History: 23/07/2001 TN - Created.
    ' 19/03/2002 Thinh Nguyen - add source id'
    ' 13/02/2004 Thinh Nguyen - change compare date to end_date and add optional start date
    ' ***************************************************************** '

    Private Function ProcessRenewalSelection(ByVal v_dtEndDate As Date, ByVal v_vProductID As Object, ByVal v_vSourceID As Object, Optional ByVal v_vStartDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim vRenewalList(,) As Object
        Dim sInsuranceRef As String = "" 'new insurance ref

        Dim lNumberOfPolicies As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            StatusBar1.Items.Item("Message").Text = "Creating business objects"
            Me.StatusBar1.Refresh()
            m_lReturn = CreateBusinessObject()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                m_lReturn = CloseBusinessObject()

                Return result
            End If

            StatusBar1.Items.Item("Message").Text = "Preparing database for renewals"
            Me.StatusBar1.Refresh()
            'prepare data for renewal selection

            m_lReturn = m_oBusiness.DelRenewalStatusPolicies(v_lRenewalStatusTypeID:=gPMConstants.PMBRenewalStatusTypePolicyChanged, v_dtCompareDate:=v_dtEndDate, v_vStartDate:=v_vStartDate)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            StatusBar1.Items.Item("Message").Text = "Selecting policies for renewals"
            Me.StatusBar1.Refresh()
            'Thinh Nguyen 19/03/2002 (start) - add source id
            'get all policy that needs renewal

            m_lReturn = m_oBusiness.GetRenewalSelection(v_vProductID:=v_vProductID, v_vBranchID:=m_vSourceID, v_dtCompareDate:=v_dtEndDate, r_vResultArray:=vRenewalList, v_vStartDate:=v_vStartDate)
            'Thinh Nguyen 19/03/2002 (end) - add source id

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' TB - return the fail code
                Return m_lReturn
            End If

            StatusBar1.Items.Item("Message").Text = "Preparing report table for Renewals"
            Me.StatusBar1.Refresh()
            'delete all in renewal_report table ready for new data

            If m_oBusiness.DelRenewalReport() <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'do we have any data
            If Not Information.IsArray(vRenewalList) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            lNumberOfPolicies = vRenewalList.GetUpperBound(1) + 1

            'loop through and process each policy
            For lCount As Integer = 0 To lNumberOfPolicies - 1

                m_lPolicyVersionIncrement = 0

                StatusBar1.Items.Item("Count").Text = CStr(lCount + 1) & " / " &
                                                      lNumberOfPolicies
                Me.StatusBar1.Refresh()

                StatusBar1.Items.Item("Message").Text = "Copying Policy"
                Me.StatusBar1.Refresh()

                m_lReturn = CreateRenewalPolicyWrapper(r_vRenewalList:=vRenewalList, v_lCount:=lCount, v_lDispStatusBar:=gPMConstants.PMEReturnCode.PMTrue)


                m_lReturn = CreateTMPAnniversaryRenewal(r_vRenewalList:=vRenewalList, v_lCount:=lCount, v_lDispStatusBar:=gPMConstants.PMEReturnCode.PMTrue)

            Next

            'Generate Agent Renewal Email


            m_lReturn = m_obSIRRenewal.GenerateAgentRenewalEmail(v_sType:="selection")

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception()
            End If
            StatusBar1.Items.Item("Message").Text = "Closing down business objects"
            Me.StatusBar1.Refresh()
            'close down business objects
            m_lReturn = CloseBusinessObject()


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessRenewalSelection Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRenewalSelection", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' Change current policy status to Renewal
    ''' Create new policy of type Renewal
    ''' </summary>
    ''' <param name="r_vRenewalList"></param>
    ''' <param name="v_lCount"></param>
    ''' <param name="v_lDispStatusBar"></param>
    ''' <param name="v_bTMPAnniversary"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateRenewalPolicy(ByRef r_vRenewalList(,) As Object, ByVal v_lCount As Object, Optional ByVal v_lDispStatusBar As Object = Nothing, Optional ByVal v_bTMPAnniversary As Object = Nothing) As Integer
        Dim nResult As Integer
        Dim nRenewalStatusTypeID As Integer 'renewal status to go on the Renewal Status table
        Dim sFailureCriterion As String = ""
        Dim nNewInsuranceFileCnt As Integer
        Dim sInsuranceRef As String = ""
        Dim lEligibleForRenewal As gPMConstants.PMEReturnCode
        Dim lProductId, lInsuredCnt As Integer
        Dim nSourceID As Integer = 0
        Dim bMidnightRenewal As Boolean
        Dim vKeyArray(,) As Object
        Dim bIsPremiumZero As Boolean
        Dim lIsQuoted As gPMConstants.PMEReturnCode
        Dim oArray(,) As Object = Nothing
        Dim oInsuranceFileTax As Object = Nothing
        Dim sDescription, sDateInteval As String
        Dim lDateIntervalNumber As Integer
        Dim lIsAgentCancelled As gPMConstants.PMEReturnCode
        Dim oRenewalFrequency As Object = Nothing
        Dim nUnderwritingYearID As Integer
        Dim sFailureMessage As String = ""
        Dim oBrokerTransferPorfolio(,) As Object = Nothing
        Dim nBrokerXferStatusTypeID As Integer = 0
        ' True monthly policy changes
        Dim bIsTrueMonthlyPolicy As Boolean
        Dim nAnniversaryCopy As Integer = 0
        Dim nRenewalDayNumber As Integer = 0
        Dim dtTMPCoverStartDate As Date
        Dim dtTMPExpiryDate As Date
        Dim dtTMPRenewalDate As Date
        Dim dtTMPAnniversaryDate As Date
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim bPutOnnextInstalmentRenewal As Boolean
        Dim nOriginalInsuranceFileCnt As Integer
        Dim bContinue As Boolean
        Dim bRenewalStatusAutomatic As Boolean
        Dim oListRisksBusiness As bSIRListRisks.Business
        Dim bInvoiceEnabled, bInstalmentsEnabled, bPayNowEnabled As Boolean
        Dim sPayment_Method As String = ""
        Dim nRenewalProductId As Integer = 0
        Dim sInstFreqency As String = ""
        Dim bisOriginalProductTMP, bSwapProducts As Boolean
        Dim vDefaultRenewalMths As String = ""
        Dim oResultsInst(,) As Object = Nothing
        Dim oProduct As bSIRProduct.Business
        Dim oUseNbPaymentTermAtRenSelection(,) As Object = Nothing
        Dim lInstalmentInsFileCnt As Integer
        Dim bChanged As Boolean
        Const kPolicyBusinessType As Integer = 2
        Dim bCashDepositEnabled As Boolean
        Dim bisOnlyAgentTransfer As Boolean
        Dim bIsReferred As Boolean
        Dim oGracePeriod As Object
        Dim iPartyCnt As Integer

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            'Start(Saurabh Agrawal) Tech Spec VAL p14 Policy Numbering (5.3.1)
            m_lReturn = CreateBusinessObject()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'End(Saurabh Agrawal) Tech Spec VAL p14 Policy Numbering (5.3.1)

            vDefaultRenewalMths = ""
            If v_lDispStatusBar = gPMConstants.PMEReturnCode.PMTrue Then
                StatusBar1.Items.Item("Message").Text = "Copying Policy"
                Me.StatusBar1.Refresh()
            End If

            lEligibleForRenewal = gPMConstants.PMEReturnCode.PMTrue

            If v_lDispStatusBar = gPMConstants.PMEReturnCode.PMTrue Then

                StatusBar1.Items.Item("Policy").Text = " " & CStr(r_vRenewalList(PMFieldPosInsuranceRef, v_lCount))
                Me.StatusBar1.Refresh()
                StatusBar1.Items.Item("Message").Text = "Copying Policy - Get " & "current policy details"
                Me.StatusBar1.Refresh()
            End If

            'assign current InsuranceFileCnt to object


            m_oInsuranceFile.InsuranceFileCnt = r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount)

            'get details of current policy

            If m_oInsuranceFile.Getdetails() <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'set policy status to Renewal

            m_oInsuranceFile.InsuranceFileStatus = "REN"


            m_oInsuranceFile.LapsedReasonID = Nothing


            m_oInsuranceFile.LapsedDate = Nothing

            'PN 30936 ------------------------------------------ START
            Dim temp_oListRisksBusiness As Object
            lReturn = g_oObjectManager.GetInstance(temp_oListRisksBusiness, "bSIRListRisks.Business",
                                                   vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oListRisksBusiness = temp_oListRisksBusiness
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                sFailureCriterion = "Failed to get instance of bSIRListRisks.Business"
            End If
            'Get current PaymentTerms
            'Start - Prakash - WPR85_Paralleling
            'Prakash Varghese - Added the parameter r_bCashDepositEnabled as part of Tech Spec -UIIC_WPR85_Cash_Deposit_Process-Part 2 work


            lReturn = oListRisksBusiness.GetPaymentTerms(v_lInsuranceFileCnt:=m_oInsuranceFile.InsuranceFileCnt,
                                                         v_lPMUserID:=g_oObjectManager.UserID,
                                                         r_bInvoiceEnabled:=bInvoiceEnabled,
                                                         r_bInstalmentsEnabled:=bInstalmentsEnabled,
                                                         r_bPayNowEnabled:=bPayNowEnabled,
                                                         r_bCashDepositEnabled:=bCashDepositEnabled)
            'End - Prakash - WPR85_Paralleling

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                sFailureCriterion = "bSIRListRisks.Business.GetPaymentTerms Failed"
            End If


            ' Tech Spec PGR 8.8 Renewals ---------------------------------
            Dim temp_oProduct As Object
            lReturn = g_oObjectManager.GetInstance(temp_oProduct, "bSIRProduct.Business",
                                                   vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oProduct = temp_oProduct
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                sFailureCriterion = "Failed to get instance of bSIRProduct.Business"
            End If

            'Get current PaymentTerms


            lReturn = oProduct.GetProductValue(v_lProductId:=m_oInsuranceFile.ProductID,
                                               v_sColumnName:="use_nb_payment_term_at_renselection",
                                               r_vProductArray:=oUseNbPaymentTermAtRenSelection)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                sFailureCriterion = "bSIRProduct.Business.GetProductValue Failed"
            End If
            ' End ----------------------------------------------------------

            Dim sOptionValue As String = "0"

            m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=SharedFiles.gPMConstants.kSystemOptionAutoInstalment, r_sOptionValue:=sOptionValue, v_iSourceID:=g_iSourceID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                sFailureCriterion = "iPMFunc.GetSystemOption Failed"
            End If

            oBatchRenewalBusiness = New bSIRRenewalBusiness()
            oBatchRenewalBusiness.Initialise(sUsername:="", sPassword:="", iUserID:=0, iSourceID:=1, iLanguageID:=0, iCurrencyID:=0, iLogLevel:=0, sCallingAppName:=ACApp)
            If oBatchRenewalBusiness Is Nothing Then
                Throw New ApplicationException("Failed to create instance of bSIRRenewalBusiness.Business")
            End If
            If Information.IsArray(oUseNbPaymentTermAtRenSelection) Then

                If Not (Not (CDbl(oUseNbPaymentTermAtRenSelection(0, 0)) = 0)) Then
                    If (sOptionValue.ToString = "1") Then
                        lReturn = oBatchRenewalBusiness.HasInstalmentPlanOnCurrentTerm(nInsuranceFileCnt:=m_oInsuranceFile.InsuranceFileCnt,
                                                                  sPaymentMethod:=sPayment_Method,
                                                                  nLatestInstalmentInsuranceFileCnt:=
                                                                     lInstalmentInsFileCnt)
                        sPayment_Method = sPayment_Method
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            nResult = gPMConstants.PMEReturnCode.PMFalse
                            sFailureCriterion = "GetInitialPolicyDetails Failed"
                        Else

                            m_oInsuranceFile.PaymentMethod = sPayment_Method

                        End If
                    Else
                        sPayment_Method = gPMFunctions.ToSafeString(m_oInsuranceFile.PaymentMethod).ToLower().Trim()
                        lInstalmentInsFileCnt = m_oInsuranceFile.InsuranceFileCnt
                    End If
                Else
                    'Check Auto Instalment is checked or not
                    If (sOptionValue.ToString = "1") Then
                        lReturn = oBatchRenewalBusiness.HasInstalmentPlanOnCurrentTerm(nInsuranceFileCnt:=m_oInsuranceFile.InsuranceFileCnt,
                                                                  sPaymentMethod:=sPayment_Method,
                                                                  nLatestInstalmentInsuranceFileCnt:=
                                                                     lInstalmentInsFileCnt)
                    Else
                        lReturn = m_oBusiness.GetInitialPolicyDetails(lInsuranceFileCnt:=m_oInsuranceFile.InsuranceFileCnt,
                                                                  sPaymentMethod:=sPayment_Method,
                                                                  lLatestInstalmentInsuranceFileCnt:=
                                                                     lInstalmentInsFileCnt)
                    End If

                    sPayment_Method = sPayment_Method.ToLower()
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                        sFailureCriterion = "GetInitialPolicyDetails Failed"

                    End If
                End If
            Else

                sPayment_Method = gPMFunctions.ToSafeString(m_oInsuranceFile.PaymentMethod).ToLower().Trim()
                lInstalmentInsFileCnt = nOriginalInsuranceFileCnt
            End If
            If (oBatchRenewalBusiness IsNot Nothing) Then
                oBatchRenewalBusiness.Dispose()
                oBatchRenewalBusiness = Nothing
            End If
            'if Payment_Method is not PayNow and Instalments then it must be Invoice
            'Start - Prakash - WPR85_Paralleling
            If _
                sPayment_Method <> "instalments" AndAlso sPayment_Method <> "direct debit" AndAlso sPayment_Method <> "PayNow" AndAlso
                sPayment_Method <> "credit card" AndAlso sPayment_Method <> "cashdeposit" Then
                sPayment_Method = "invoice"
            End If
            'End - Prakash - WPR85_Paralleling

            'Check if the Payment_Method is Invalid
            If sPayment_Method = "invoice" And Not bInvoiceEnabled Then
                nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                sFailureCriterion = "Payment Type - (Invoice) is Invalid"
            ElseIf _
                (sPayment_Method = "instalments" Or sPayment_Method = "direct debit" Or sPayment_Method = "credit card") And
                Not bInstalmentsEnabled Then
                nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                sFailureCriterion = "Payment Type - (Instalments) is Invalid"
            ElseIf sPayment_Method = "PayNow" And Not bPayNowEnabled Then
                nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                sFailureCriterion = "Payment Type - (PayNow) is Invalid"
                'Start - Prakash - WPR85_Paralleling
            ElseIf sPayment_Method = "cashdeposit" And Not bCashDepositEnabled Then
                'CreateRenewalPolicy = PMFalse
                nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                sFailureCriterion = "Payment Type - (CashDeposit) is Invalid"
            End If

            If sPayment_Method = "cashdeposit" Then
                'If payment method is cash deposit, always set for manual review.
                nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                'Start - Renuka -  Changes according to the WPR85 process sheet updation
                sFailureCriterion = "Manual Review - Payment option cash depsoit"
                'End - Renuka -  Changes according to the WPR85 process sheet updation
            End If
            'End - Prakash - WPR85_Paralleling


            If sFailureCriterion <> "" Then

                'If there's an Err, Add it to Renewal_report table

                m_lReturn = m_oBusiness.AddRenewalReport(
                    v_sReportType:=IIf(bRenewalStatusAutomatic, "AutoRenewal", "ManualRenewal"),
                    v_vClientName:=r_vRenewalList(PMFieldPosClientName, v_lCount),
                    v_vPolicyNumber:=r_vRenewalList(PMFieldPosInsuranceRef, v_lCount),
                    v_vAgentCode:=r_vRenewalList(PMFieldPosAgentName, v_lCount),
                    v_vCoverStartDate:=r_vRenewalList(PMFieldPosCoverStartDate, v_lCount),
                    v_vCoverEndDate:=r_vRenewalList(PMFieldPosCoverEndDate, v_lCount),
                    v_vProductCode:=r_vRenewalList(PMFieldPosProductCode, v_lCount),
                    v_vFailureCriterion:=sFailureCriterion,
                    v_vFailureDetail:="",
                    v_vInsuranceFileCnt:=r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount))
                sFailureCriterion = ""
            End If
            'PN 30936 ------------------------------------------ END
            If _
                sPayment_Method = "PayNow" AndAlso
                (Trim(m_oInsuranceFile.businesstype) = "COIN FOLL" OrElse Trim(m_oInsuranceFile.businesstype) = "IN FAC") _
                Then

                sFailureCriterion = "Manual Review Required for Co-Insurance Follow / Inward Facultative"

                'Add Error to Renewal_report table
                m_lReturn = m_oBusiness.AddRenewalReport(
                    v_sReportType:=IIf(bRenewalStatusAutomatic,
                                         "AutoRenewal", "ManualRenewal"),
                    v_vClientName:=r_vRenewalList(PMFieldPosClientName, v_lCount),
                    v_vPolicyNumber:=r_vRenewalList(PMFieldPosInsuranceRef, v_lCount),
                    v_vAgentCode:=r_vRenewalList(PMFieldPosAgentName, v_lCount),
                    v_vCoverStartDate:=r_vRenewalList(PMFieldPosCoverStartDate, v_lCount),
                    v_vCoverEndDate:=r_vRenewalList(PMFieldPosCoverEndDate, v_lCount),
                    v_vProductCode:=r_vRenewalList(PMFieldPosProductCode, v_lCount),
                    v_vFailureCriterion:=sFailureCriterion)

            End If

            'PN 30909 ------------------------------------------ START

            If m_oInsuranceFile.BusinessType.Trim() = "COIN FOLL" Or m_oInsuranceFile.BusinessType.Trim() = "IN FAC" _
                Then

                sFailureCriterion = "Manual Review Required for Co-Insurance Follow / Inward Facultative"

                'Add Error to Renewal_report table

                m_lReturn = m_oBusiness.AddRenewalReport(v_sReportType:="ManualRenewal",
                                                         v_vClientName:=r_vRenewalList(PMFieldPosClientName, v_lCount),
                                                         v_vPolicyNumber:=
                                                            r_vRenewalList(PMFieldPosInsuranceRef, v_lCount),
                                                         v_vAgentCode:=r_vRenewalList(PMFieldPosAgentName, v_lCount),
                                                         v_vCoverStartDate:=
                                                            r_vRenewalList(PMFieldPosCoverStartDate, v_lCount),
                                                         v_vCoverEndDate:=
                                                            r_vRenewalList(PMFieldPosCoverEndDate, v_lCount),
                                                         v_vProductCode:=
                                                            r_vRenewalList(PMFieldPosProductCode, v_lCount),
                                                         v_vFailureCriterion:=sFailureCriterion,
                                                         v_vFailureDetail:="",
                                                         v_vInsuranceFileCnt:=
                                                            r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount))


            End If

            If v_lDispStatusBar = gPMConstants.PMEReturnCode.PMTrue Then
                StatusBar1.Items.Item("Message").Text = "Copying Policy - Update " & "current policy status"
                Me.StatusBar1.Refresh()
            End If

            '01/08/2003 Tracy Richards - Add a transaction to umbrella all these
            'calls, to make sure that records are not left in a mid-state.

            m_lReturn = m_oInsuranceFile.BeginTrans
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'update current policy to status Renewal

            m_lReturn = m_oInsuranceFile.UpdatePolicy
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                m_oInsuranceFile.RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'set policy type to Renewal

            m_oInsuranceFile.InsuranceFileType = "RENEWAL"
            'set policy to Live (i.e. status = Null).


            m_oInsuranceFile.InsuranceFileStatus = Nothing

            lProductId = m_oInsuranceFile.ProductID

            If v_lDispStatusBar = gPMConstants.PMEReturnCode.PMTrue Then
                StatusBar1.Items.Item("Message").Text = "Copying Policy - Is policy" & " midnight renewal?"
                Me.StatusBar1.Refresh()
            End If


            m_lReturn = m_oBusiness.GetMidnightRenewal(v_lProductId:=lProductId, r_bMidnight:=bMidnightRenewal)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                m_oInsuranceFile.RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'get renewal frequency number (number of months)


            m_lReturn = m_oBusiness.GetRenewalFrequencyDetail(v_lFrequencyID:=m_oInsuranceFile.RenewalFrequencyID,
                                                              r_vResult:=oRenewalFrequency)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                m_oInsuranceFile.RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' determine which if any policy discount details need
            ' to be applied to the renewal....

            ' if the original policy has a recurring type of discount then

            If m_oInsuranceFile.DiscountRecurringTypeId = kDiscountRecurringTypeIdPolicy Then
                ' retain the existing policy discount information from the original policy
            Else
                ' otherwise clear down this information

                m_oInsuranceFile.DiscountPercentage = 0

                m_oInsuranceFile.DiscountReasonID = 0

                m_oInsuranceFile.MatchDiscountedPremiumFlag = 0

                m_oInsuranceFile.DiscountedPremium = 0

                m_oInsuranceFile.DiscountRecurringTypeId = 0
            End If


            Dim oAltRefMandetory As Object = Nothing


            If gPMFunctions.ToSafeLong(m_oInsuranceFile.LeadAgentCnt, 0) <> 0 Then

                '        m_lReturn = m_oInsuranceFileBusiness.GetFromTable("party_agent", "alternate_reference_for_each_transaction", "party_cnt", m_oInsuranceFile.LeadAgentCnt, vAltRefForEachTrans)


                m_lReturn = m_oInsuranceFileBusiness.GetFromTable("party_agent", "alternate_reference_mandatory",
                                                                  "party_cnt", m_oInsuranceFile.LeadAgentCnt,
                                                                  oAltRefMandetory)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If gPMFunctions.ToSafeBoolean(oAltRefMandetory, 0) And m_oInsuranceFile.AlternateReference = "" Then

                    'm_oInsuranceFile.AlternateReference = ""

                    nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                    sFailureCriterion = "The Alternate Reference must be entered for this renewal policy." &
                                        "You must amend the renewal before the renewal can be accepted."
                End If
            End If
            'PN 33800 (RC) ----------------------------End


            sDateInteval = "m"

            lDateIntervalNumber = CInt(oRenewalFrequency(2, 0))
            'check if original product is tmp


            m_lReturn = m_oBusiness.IsTrueMonthlyPolicyProduct(m_oInsuranceFile.ProductID, bisOriginalProductTMP)

            'get the instalmment scheme frequency
            If _
                sPayment_Method = "instalments" Or sPayment_Method = "direct debit" Or
                sPayment_Method <> "paynow" And sPayment_Method <> "credit card" Then


                m_lReturn =
                    m_oBusiness.GetInstalmentFrequency(
                        gPMFunctions.ToSafeLong(IIf(CDbl(oUseNbPaymentTermAtRenSelection(0, 0)) = 0,
                                                    (r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount)),
                                                    lInstalmentInsFileCnt)), sInstFreqency)
            End If

            bSwapProducts = False
            If _
                bisOriginalProductTMP AndAlso
                gPMFunctions.ToSafeDate(r_vRenewalList(PMFieldPosRenewalDate, v_lCount)) >=
                gPMFunctions.ToSafeDate(r_vRenewalList(PMFieldPosAnniversaryDate, v_lCount)) Then
                bSwapProducts = True
            ElseIf Not bisOriginalProductTMP AndAlso (sInstFreqency <> "Q" AndAlso sInstFreqency <> "A") Then
                bSwapProducts = True
            End If

            '1.12 Wr25
            If (gPMFunctions.ToSafeString(r_vRenewalList(PMFieldPosRenewalProductId, v_lCount)) <> "") And bSwapProducts _
                Then


                m_oInsuranceFile.OriginalProductID = r_vRenewalList(PMFieldPosProductID, v_lCount)


                m_oInsuranceFile.ProductID = r_vRenewalList(PMFieldPosRenewalProductId, v_lCount)


                m_oInsuranceFile.RenewalProductID = r_vRenewalList(PMFieldPosRenewalProductId, v_lCount)

                If _
                    bisOriginalProductTMP And
                    gPMFunctions.ToSafeDate(r_vRenewalList(PMFieldPosRenewalDate, v_lCount)) >=
                    gPMFunctions.ToSafeDate(r_vRenewalList(PMFieldPosAnniversaryDate, v_lCount)) Then


                    m_lReturn = m_oInsuranceFileBusiness.GetFromTable("PRODUCT", "default_renewal_months", "product_id",
                                                                      m_oInsuranceFile.ProductID, vDefaultRenewalMths)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If

            ' determine whether the associated policies product is a "True Monthly Policy"
            'bIsTrueMonthlyPolicy = CBool(ToSafeLong(r_vRenewalList(PMFieldPosIsTrueMonthlyPolicy, v_lCount)) = 1)


            m_lReturn = m_oBusiness.IsTrueMonthlyPolicyProduct(m_oInsuranceFile.ProductID, bIsTrueMonthlyPolicy)

            ' Switch off the TMP flag on a policy as its getting moved to another product
            If bisOriginalProductTMP AndAlso bSwapProducts AndAlso Not bIsTrueMonthlyPolicy Then

                r_vRenewalList(PMFieldPosIsTrueMonthlyPolicy, v_lCount) = 0
            End If

            ' if the associated product is "True Monthly Policy"
            If bIsTrueMonthlyPolicy Then
                If Not v_bTMPAnniversary Then

                    v_bTMPAnniversary =
                        gPMFunctions.ToSafeBoolean(
                            r_vRenewalList(PMFieldPosRenewalDate, v_lCount).Equals(
                                r_vRenewalList(PMFieldPosAnniversaryDate, v_lCount)))
                End If
                ' get the appropriate dates for the true monthly policy
                lReturn = CType(GetTrueMonthlyPolicyDates(v_bMidnightRenewal:=bMidnightRenewal,
                                                          v_bTMPAnniversary:=v_bTMPAnniversary, v_lCount:=v_lCount,
                                                          r_vRenewalList:=r_vRenewalList,
                                                          r_dtCoverStartDate:=dtTMPCoverStartDate,
                                                          r_dtExpiryDate:=dtTMPExpiryDate,
                                                          r_dtRenewalDate:=dtTMPRenewalDate,
                                                          r_dtAnniversaryDate:=dtTMPAnniversaryDate,
                                                          r_lRenewalDayNumber:=nRenewalDayNumber,
                                                          r_lAnniversaryCopy:=nAnniversaryCopy), 
                                gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    m_oInsuranceFile.RollbackTrans()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' save the data back to the insurance file

                m_oInsuranceFile.CoverStartDate = dtTMPCoverStartDate

                m_oInsuranceFile.ExpiryDate = dtTMPExpiryDate

                m_oInsuranceFile.RenewalDAte = dtTMPRenewalDate

                m_oInsuranceFile.InceptionTPI = dtTMPCoverStartDate

                m_oInsuranceFile.AnniversaryDate = dtTMPAnniversaryDate

                m_oInsuranceFile.AnniversaryCopy = nAnniversaryCopy

                m_oInsuranceFile.RenewalDayNumber = nRenewalDayNumber

                m_oInsuranceFile.PutOnNextInstalmentRenewal = 0


                bPutOnnextInstalmentRenewal =
                    gPMFunctions.ToSafeLong(CDbl(r_vRenewalList(PMFieldPosPutOnNextInstalmentRenewal, v_lCount)) = 1)

                ' if this is the anniversary copy of the policy then
                ' reset the inception date to be that of the new cover start date
                If v_bTMPAnniversary Then

                    m_oInsuranceFile.InceptionDate = dtTMPCoverStartDate
                End If

            Else

                'set new cover period
                'The problem here is that cover start date is that of this version of
                'the policy not of the policy as a whole.  So let's use the renewal
                'date instead, as that won't have changed

                If gPMFunctions.ToSafeInteger(vDefaultRenewalMths) > 0 Then
                    lDateIntervalNumber = gPMFunctions.ToSafeInteger(vDefaultRenewalMths)
                End If

                If bMidnightRenewal Then

                    'Thinh Nguyen 08/10/2002 - add one day to expiry date for next start date


                    m_oInsuranceFile.CoverStartDate = CDate(r_vRenewalList(PMFieldPosCoverEndDate, v_lCount)).AddDays(1)


                    m_oInsuranceFile.ExpiryDate =
                        DateAndTime.DateAdd(sDateInteval, lDateIntervalNumber, m_oInsuranceFile.CoverStartDate).AddDays(
                            -1)

                Else


                    m_oInsuranceFile.CoverStartDate = r_vRenewalList(PMFieldPosCoverEndDate, v_lCount)


                    m_oInsuranceFile.ExpiryDate = DateAndTime.DateAdd(sDateInteval, lDateIntervalNumber,
                                                                      m_oInsuranceFile.CoverStartDate)

                End If

                'Thinh Nguyen 13/01/2003 - renewal date is cover from date plus period of the policy


                m_oInsuranceFile.RenewalDAte = DateAndTime.DateAdd(sDateInteval, lDateIntervalNumber,
                                                                   m_oInsuranceFile.CoverStartDate)


                m_oInsuranceFile.InceptionTPI = m_oInsuranceFile.CoverStartDate

                ' store anniversary date as renewal date


                m_oInsuranceFile.AnniversaryDate = m_oInsuranceFile.RenewalDAte


                m_oInsuranceFile.RenewalDayNumber = DateAndTime.Day(m_oInsuranceFile.RenewalDAte)

            End If

            Dim sValue As String
            m_lReturn = iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTUnderwritingYear, 1, sValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            If sValue = "1" Then
                m_bUnderwritingYearID = True
            Else
                m_bUnderwritingYearID = False
            End If

            'RKS PN13438 and DD 14537
            If m_bUnderwritingYearID Then


                m_lReturn = m_oInsuranceFileBusiness.GetUnderwritingYear(m_oInsuranceFile.CoverStartDate,
                                                                         nUnderwritingYearID)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get Underwriting Year for " & m_sTransactionType & ".", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateRenewalPolicy")
                ElseIf nUnderwritingYearID = 0 Then
                    'PN14537 - Log the problem, don't prompt the user
                    nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review

                    sFailureCriterion = "No Underwriting Year exists for " &
                                        StringsHelper.Format(m_oInsuranceFile.CoverStartDate, "General Date")

                    m_oInsuranceFile.UnderwritingYearID = nUnderwritingYearID
                    lEligibleForRenewal = gPMConstants.PMEReturnCode.PMFalse
                Else

                    m_oInsuranceFile.UnderwritingYearID = nUnderwritingYearID
                End If
            End If
            'RKS PN13438


            m_oInsuranceFile.EventDescription = "Policy Copied To Renewal"

            If v_lDispStatusBar = gPMConstants.PMEReturnCode.PMTrue Then
                StatusBar1.Items.Item("Message").Text = "Copying Policy - Create renewal policy"
                Me.StatusBar1.Refresh()
            End If

            If v_bTMPAnniversary Then
                ' if this is an anniversary policy it is possible that
                ' a normal renewal (non anniversary) has already been run
                ' so a further increment of the Policy Version is required

                m_oInsuranceFile.PolicyVersion = m_oInsuranceFile.PolicyVersion + 1 + m_lPolicyVersionIncrement
            Else

                m_oInsuranceFile.PolicyVersion += 1
            End If

            'Start(Saurabh Agrawal) Tech Spec VAL p14 Policy Numbering (5.3.1)

            sInsuranceRef = m_oInsuranceFile.InsuranceRef
            'Start - Renuka - (WPR87 Paralleling)


            iPartyCnt = m_oInsuranceFile.InsuredCnt

            m_lReturn = m_oPolicyNumMaint.GenerateRenewalPolicyNumber(v_iPolicy_cnt:=gPMFunctions.ToSafeInteger(CStr(r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount))), v_lBusinessType:=kPolicyBusinessType, v_iBranch:=gPMFunctions.ToSafeInteger(m_oInsuranceFile.SourceID), v_lProductId:=gPMFunctions.ToSafeLong(r_vRenewalList(PMFieldPosProductID, v_lCount)), v_lAgent:=gPMFunctions.ToSafeLong(r_vRenewalList(PMFieldPosLeadAgentCnt, v_lCount)), r_sGeneratedPolicyNumber:=sInsuranceRef, r_bChanged:=bChanged, v_dtTransactionDate:=m_oInsuranceFile.CoverStartDate, v_lPartyCnt:=iPartyCnt)
            'End - Renuka - (WPR87 Paralleling)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'create new policy of type Renewal
            If bChanged Then

                m_oInsuranceFile.InsuranceRef = sInsuranceRef
                'Start(Sriram P)PN55579

                r_vRenewalList(PMFieldPosInsuranceRef, v_lCount) = sInsuranceRef
                'End(Sriram P)PN55579
            End If

            m_oInsuranceFile.PaymentMethod = sPayment_Method

            'End(Saurabh Agrawal) Tech Spec VAL p14 Policy Numbering (5.3.1)
            m_oInsuranceFile.QuoteExpiryDate = Nothing
            'Update Renewal Quote Expiry date
            lReturn = oProduct.GetProductValue(v_lProductId:=m_oInsuranceFile.ProductID, v_sColumnName:="grace_period", r_vProductArray:=oGracePeriod)
            If Information.IsArray(oGracePeriod) Then
                Dim iGracePeriod As Integer = CInt(oGracePeriod.GetValue(0, 0))
                If iGracePeriod > 0 Then
                    m_oInsuranceFile.QuoteExpirydate = DateTime.Today.AddDays(iGracePeriod)
                End If
            End If
            Dim oPartyDefaultCorrespondance(,) As Object = Nothing
            m_oInsuranceFileBusiness.GetDefaultPreferredCorrespondence(m_oInsuranceFile.InsuredCnt, oPartyDefaultCorrespondance)
            If Information.IsArray(oPartyDefaultCorrespondance) Then
                m_oInsuranceFile.DefaultPreferredCorrespondence = oPartyDefaultCorrespondance(0, 0)
            End If
            oPartyDefaultCorrespondance = Nothing
            'create new policy of type Renewal

            m_lReturn = m_oInsuranceFile.CreatePolicy
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                m_oInsuranceFile.RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
                'Tracy Richards - Commit trans here as all essential processing has been done.
            Else

                m_oInsuranceFile.CommitTrans()
            End If

            'get client and branch

            lInsuredCnt = m_oInsuranceFile.InsuredCnt

            nSourceID = m_oInsuranceFile.SourceID

            'get new insurance file cnt
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(oObject:=temp_m_oBusiness, sClassName:="bSIRRenSelection.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oRenewalSelectionBusiness = temp_m_oBusiness
            nNewInsuranceFileCnt = m_oInsuranceFile.InsuranceFileCnt

            sInsuranceRef = m_oInsuranceFile.InsuranceRef
            m_lReturn = m_oRenewalSelectionBusiness.UpdateInsuranceFileSystem(nNewInsuranceFileCnt)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                sFailureCriterion = "Failed to update insurance file system, insurance file count :" & nNewInsuranceFileCnt
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            If v_lDispStatusBar = gPMConstants.PMEReturnCode.PMTrue Then
                StatusBar1.Items.Item("Message").Text = "Copying Policy - Copying standard wording"
                Me.StatusBar1.Refresh()
            End If


            m_lReturn = m_oBusiness.CopyPolicyStandardWordings(v_lOldInsuranceFileCnt:=r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount), v_lNewInsuranceFileCnt:=nNewInsuranceFileCnt, v_dtEffectiveDate:=CDate(r_vRenewalList(PMFieldPosCoverEndDate, v_lCount)).AddYears(1))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                m_oInsuranceFile.RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oBusiness.CopyPolicyAssociates(nOldInsuranceFileCnt:=CInt(r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount)),
                                                                nNewInsuranceFileCnt:=nNewInsuranceFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                m_oInsuranceFile.RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If v_lDispStatusBar = gPMConstants.PMEReturnCode.PMTrue Then
                StatusBar1.Items.Item("Message").Text = "Copying Policy - Copying coinsurance"
                Me.StatusBar1.Refresh()
            End If

            'RWH(14/06/01) Copy coinsurance.

            m_lReturn = m_oBusiness.CopyCoinsurance(
                v_lCurrentInsFileCnt:=r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount),
                v_lNewInsFileCnt:=nNewInsuranceFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                sFailureCriterion = "Failed to copy coinsurance"
            End If

            ''Tomo160801
            'If v_lDispStatusBar = gPMConstants.PMEReturnCode.PMTrue Then
            '    StatusBar1.Items.Item("Message").Text = "Copying Policy - Copying agent commission"
            '    Me.StatusBar1.Refresh()
            'End If

            'Copy agent commission.


            'If _
            '    Not _
            '    (bIsTrueMonthlyPolicy AndAlso
            '     CDbl(r_vRenewalList(PMFieldPosLeadAllowConsolidatedCommission, v_lCount)) = 1 And
            '     CDbl(r_vRenewalList(PMFieldPosRenewalCount, v_lCount)) < 11) Then

            '    m_lReturn =
            '    m_oBusiness.CopyAgentCommission(
            '        v_lCurrentInsFileCnt:=r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount),
            '        v_lNewInsFileCnt:=nNewInsuranceFileCnt)
            '    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '        nResult = gPMConstants.PMEReturnCode.PMFalse
            '        nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
            '        sFailureCriterion = "Failed to copy agent commission"
            '    End If
            'End If

            'Tomo170801
            If v_lDispStatusBar = gPMConstants.PMEReturnCode.PMTrue Then
                StatusBar1.Items.Item("Message").Text = "Copying Policy - Copying policy agents"
                Me.StatusBar1.Refresh()
            End If

            'Copy insurance file agents.

            m_lReturn =
                m_oBusiness.CopyInsuranceFileAgent(
                    v_lCurrentInsFileCnt:=r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount),
                    v_lNewInsFileCnt:=nNewInsuranceFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                sFailureCriterion = "Failed to copy policy agents"
            End If

            If v_lDispStatusBar = gPMConstants.PMEReturnCode.PMTrue Then
                StatusBar1.Items.Item("Message").Text = "Copying Risks"
                Me.StatusBar1.Refresh()
            End If

            'RWH(22/08/01) Must set the Task in Tax as this is passed into stored procedures
            'as Mode. If it is wrong the new tax records will not be created.

            m_lReturn = m_oTax.SetProcessModes(vTask:=m_iTask, vTransactionType:="REN")

            'copy risk details to new policy
            m_lReturn = CopyRiskData(r_vRenewalList:=r_vRenewalList, v_lCount:=v_lCount,
                                     v_lNewInsuranceFileCnt:=nNewInsuranceFileCnt,
                                     r_sFailureReason:=sFailureCriterion, r_lEligibleForRenewal:=lEligibleForRenewal,
                                     v_bIsTrueMonthlyPolicy:=bIsTrueMonthlyPolicy,
                                     v_bTMPAnniversary:=v_bTMPAnniversary, r_bIsReferred:=bIsReferred)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Always update policy premium to avoid magical figures during manual review

                m_lReturn = m_oChangePolicyStatus.UpdatePolicyPremium(v_lInsuranceFileCnt:=nNewInsuranceFileCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'Leave alone. Failure is anyway due to copy risk data
                End If

                m_lReturn = m_oAgentCommission.CalculateAgentCommission(v_lInsuranceFileCnt:=nNewInsuranceFileCnt, v_sTransactionType:="REN", r_vntResult:=oArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                    sFailureCriterion = "Failed to do Agent Commission"
                End If

                If sFailureCriterion = "" Then
                    sFailureCriterion = "Failed to copy risk data"
                Else
                    sFailureCriterion = "Failed to copy risk data (" & sFailureCriterion & ")"
                End If
                If nRenewalStatusTypeID = 0 Then
                    nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                End If
            Else

                ' default to continue with process
                bContinue = True
                'PN 67489 -Always update policy premium to avoid magical figures during manual review

                m_lReturn = m_oChangePolicyStatus.UpdatePolicyPremium(v_lInsuranceFileCnt:=nNewInsuranceFileCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'Leave alone. Failure is anyway due to copy risk data
                End If

                ' if the renewal policy is subject to a policy discount

                If m_oInsuranceFile.DiscountRecurringTypeId = kDiscountRecurringTypeIdPolicy Then

                    ' indicate to the user that we are applying the discount
                    If v_lDispStatusBar = gPMConstants.PMEReturnCode.PMTrue Then
                        StatusBar1.Items.Item("Message").Text = "Copying Policy - Applying Policy Discount"
                        Me.StatusBar1.Refresh()
                    End If

                    ' apply any discount that is defined


                    m_lReturn = ApplyPolicyDiscount(m_oInsuranceFile.InsuranceFileCnt, m_oInsuranceFile.ProductID)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        ' if the apply discount process failed then we dont want to continue
                        ' so drop out of the process
                        bContinue = False

                        ' indicate this item needs to be manually reviewed.
                        If sFailureCriterion = "" Then
                            sFailureCriterion = "Failed to apply policy discount"
                        Else
                            sFailureCriterion = "Failed to apply policy discount (" & sFailureCriterion & ")"
                        End If

                        If nRenewalStatusTypeID = 0 Then
                            nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                        End If

                    End If

                End If

                If bContinue Then

                    If v_lDispStatusBar = gPMConstants.PMEReturnCode.PMTrue Then
                        StatusBar1.Items.Item("Message").Text = "Copying Policy - Checking renewal criteria"
                        Me.StatusBar1.Refresh()
                    End If

                    'Check Auto-renewal eligibility.

                    If _
                        m_oBusiness.CheckRenewalCriteria(r_vRenewalList, v_lCount, bisOnlyAgentTransfer) =
                        gPMConstants.PMEReturnCode.PMFalse Then
                        lEligibleForRenewal = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'TN20010719 - start
                    If lEligibleForRenewal = gPMConstants.PMEReturnCode.PMTrue Then
                        If v_lDispStatusBar = gPMConstants.PMEReturnCode.PMTrue Then
                            StatusBar1.Items.Item("Message").Text = "Copying Policy - Checking quote status"
                            Me.StatusBar1.Refresh()
                        End If


                        m_lReturn = m_oBusiness.IsQuoted(v_lInsuranceFileCnt:=nNewInsuranceFileCnt,
                                                         r_lIsQuoted:=lIsQuoted)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            lIsQuoted = gPMConstants.PMEReturnCode.PMFalse
                        End If

                        If lIsQuoted = gPMConstants.PMEReturnCode.PMFalse Then
                            If v_lDispStatusBar = gPMConstants.PMEReturnCode.PMTrue Then
                                StatusBar1.Items.Item("Message").Text = "Copying Policy - Adding renewal report " &
                                                                        "record"
                                Me.StatusBar1.Refresh()
                            End If

                            lEligibleForRenewal = gPMConstants.PMEReturnCode.PMFalse


                            m_lReturn = m_oBusiness.AddRenewalReport(v_sReportType:="ManualRenewal",
                                                                     v_vClientName:=
                                                                        r_vRenewalList(PMFieldPosClientName, v_lCount),
                                                                     v_vPolicyNumber:=
                                                                        r_vRenewalList(PMFieldPosInsuranceRef, v_lCount),
                                                                     v_vAgentCode:=
                                                                        r_vRenewalList(PMFieldPosAgentName, v_lCount),
                                                                     v_vCoverStartDate:=
                                                                        r_vRenewalList(PMFieldPosCoverStartDate,
                                                                                       v_lCount),
                                                                     v_vCoverEndDate:=
                                                                        r_vRenewalList(PMFieldPosCoverEndDate, v_lCount),
                                                                     v_vProductCode:=
                                                                        r_vRenewalList(PMFieldPosProductCode, v_lCount),
                                                                     v_vFailureCriterion:=PMIsQuotedDesc,
                                                                     v_vFailureDetail:="",
                                                                     v_vInsuranceFileCnt:=
                                                                        r_vRenewalList(PMFieldPosInsuranceFileCnt,
                                                                                       v_lCount))

                        End If
                    End If

                    'If not eligible for renewal, either because a risk failed rating OR
                    'policy level auto-renewal criteria were failed, then set RenewalStatusType
                    'and message appropriately.
                    If lEligibleForRenewal = gPMConstants.PMEReturnCode.PMFalse Then
                        If bisOnlyAgentTransfer Then
                            nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeAutoRated
                        Else
                            nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview
                            m_oAgentCommission.InsuranceFileCnt = nNewInsuranceFileCnt

                            'Tomo17102001 - The Get deletes the existing records, and recalculates them
                            'but does _not_ write them back to the database.  The calculate does...
                            'Enhancement 35643 Populate Agent Commission on Renewal Version in 'Manual Review' status
                            If Not (bIsTrueMonthlyPolicy And CDbl(r_vRenewalList(PMFieldPosLeadAllowConsolidatedCommission, v_lCount)) = 1 And CDbl(r_vRenewalList(PMFieldPosRenewalCount, v_lCount)) < 11) Then

                                m_lReturn = m_oAgentCommission.CalculateAgentCommission(v_lInsuranceFileCnt:=nNewInsuranceFileCnt, v_sTransactionType:="REN", r_vntResult:=oArray)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    nResult = gPMConstants.PMEReturnCode.PMFalse
                                    nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                                    sFailureCriterion = "Failed to do Agent Commission"
                                End If
                            End If
                        End If
                        If sFailureCriterion = "" Then
                            sFailureCriterion = "Manual Review"
                        End If
                        m_lReturn = m_oBusiness.IsQuoted(v_lInsuranceFileCnt:=nNewInsuranceFileCnt,
                                                         r_lIsQuoted:=lIsQuoted)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            lIsQuoted = gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                    If lIsQuoted = gPMConstants.PMEReturnCode.PMTrue Then
                        nResult = gPMConstants.PMEReturnCode.PMTrue

                        If bIsTrueMonthlyPolicy And Not v_bTMPAnniversary Then
                            'Set status to say we are ready to print the renewal notice.
                            nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeAwaitUpdate
                        ElseIf nRenewalStatusTypeID <> gPMConstants.PMBRenewalStatusTypeManualReview Then
                            'Set status to say we are ready to print the renewal notice.
                            nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeAutoRated

                        End If
                        'DO POLICY LEVEL STUFF
                        If v_lDispStatusBar = gPMConstants.PMEReturnCode.PMTrue Then
                            StatusBar1.Items.Item("Message").Text = "Copying Policy - Policy level reinsurance"
                            Me.StatusBar1.Refresh()
                        End If

                        'Reset RiskId to ensure Policy level reinsurance is done.

                        m_oReinsurance.InsuranceFileCnt = nNewInsuranceFileCnt

                        m_oReinsurance.RiskId = 0


                        m_lReturn = m_oReinsurance.Getdetails

                        'Do policy taxes.
                        If v_lDispStatusBar = gPMConstants.PMEReturnCode.PMTrue Then
                            StatusBar1.Items.Item("Message").Text = "Copying Policy - Policy level tax"
                            Me.StatusBar1.Refresh()
                        End If


                        m_oTax.InsuranceFileCnt = nNewInsuranceFileCnt


                        m_lReturn = m_oTax.GetInsuranceFileTax(oInsuranceFileTax, sDescription)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            nResult = gPMConstants.PMEReturnCode.PMFalse
                            nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                            sFailureCriterion = "Failed to do Policy Taxes"
                        End If

                        ' create any fees that apply for the renewal policy

                        m_lReturn = m_oBusiness.CreateRenewalFees(nNewInsuranceFileCnt)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            nResult = gPMConstants.PMEReturnCode.PMFalse
                            nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                            sFailureCriterion = "Failed to calculate renewal fees"
                        End If

                        'Update policy premium.
                        If v_lDispStatusBar = gPMConstants.PMEReturnCode.PMTrue Then
                            StatusBar1.Items.Item("Message").Text = "Copying Policy - Update policy premium"
                            Me.StatusBar1.Refresh()
                        End If


                        m_lReturn =
                            m_oChangePolicyStatus.UpdatePolicyPremium(v_lInsuranceFileCnt:=nNewInsuranceFileCnt)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            nResult = gPMConstants.PMEReturnCode.PMFalse
                            nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                            sFailureCriterion = "Failed to update Policy Premium"
                        End If

                        'Do agent commission.
                        If v_lDispStatusBar = gPMConstants.PMEReturnCode.PMTrue Then
                            StatusBar1.Items.Item("Message").Text = "Copying Policy - Agent commission"
                            Me.StatusBar1.Refresh()
                        End If


                        m_oAgentCommission.InsuranceFileCnt = nNewInsuranceFileCnt

                        'Tomo17102001 - The Get deletes the existing records, and recalculates them
                        'but does _not_ write them back to the database.  The calculate does...


                        If _
                            Not _
                            (bIsTrueMonthlyPolicy AndAlso
                             CDbl(r_vRenewalList(PMFieldPosLeadAllowConsolidatedCommission, v_lCount)) = 1 AndAlso
                             CDbl(r_vRenewalList(PMFieldPosRenewalCount, v_lCount)) < 11) Then

                            m_lReturn = m_oBusiness.CopyAgentCommission(v_lCurrentInsFileCnt:=r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount),
                                                                        v_lNewInsFileCnt:=nNewInsuranceFileCnt)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                nResult = gPMConstants.PMEReturnCode.PMFalse
                                nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                                sFailureCriterion = "Failed to copy agent commission"
                            End If

                            m_lReturn =
                                m_oAgentCommission.CalculateAgentCommission(v_lInsuranceFileCnt:=nNewInsuranceFileCnt,
                                                                            v_sTransactionType:="REN",
                                                                            r_vntResult:=oArray)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                nResult = gPMConstants.PMEReturnCode.PMFalse
                                nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview _
                                'Awaiting Manual Review
                                sFailureCriterion = "Failed to do Agent Commission"
                            End If
                        End If

                        'TMP
                        If Not bIsTrueMonthlyPolicy Then
                            ' Is the premium zero ?

                            m_lReturn = m_oBusiness.IsPremiumZero(v_lInsuranceFileCnt:=nNewInsuranceFileCnt,
                                                                  r_bIsPremiumZero:=bIsPremiumZero)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                nResult = gPMConstants.PMEReturnCode.PMFalse
                                nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview _
                                'Awaiting Manual Review
                                sFailureCriterion = "Failed to check IsPremiumZero"
                            Else
                                'If the premium is zero then fail and let the user know why in the report
                                If bIsPremiumZero Then
                                    nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview _
                                    'Awaiting Manual Review
                                    sFailureCriterion = "Premium Is Zero"
                                End If
                            End If
                        End If

                        'Thinh Nguyen 15/08/2003 (start) - set to manual review if lead agent is cancelled


                        If _
                            CStr(r_vRenewalList(PMFieldPosLeadAgentCnt, v_lCount)) <> "" Or
                            r_vRenewalList(PMFieldPosLeadAgentCnt, v_lCount) Is DBNull.Value Then


                            m_lReturn =
                                m_oBusiness.IsAgentCancelled(
                                    v_lpartyCnt:=CInt(r_vRenewalList(PMFieldPosLeadAgentCnt, v_lCount)),
                                    r_lIsCancelled:=lIsAgentCancelled)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                nResult = gPMConstants.PMEReturnCode.PMFalse
                                nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview _
                                'Awaiting Manual Review
                                sFailureCriterion = "IsAgentCancelled Failed"
                            ElseIf lIsAgentCancelled = gPMConstants.PMEReturnCode.PMTrue Then
                                nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview _
                                'Awaiting Manual Review
                                sFailureCriterion = "Agent has been cancelled"

                            End If
                        End If
                        'Thinh Nguyen 15/08/2003 (start) - set to manual review if lead agent is cancelled
                    End If
                End If
            End If

            sFailureMessage = ""
            oBatchRenewalBusiness = New bSIRRenewalBusiness()
            oBatchRenewalBusiness.Initialise(sUsername:="", sPassword:="", iUserID:=0, iSourceID:=1, iLanguageID:=0, iCurrencyID:=0, iLogLevel:=0, sCallingAppName:=ACApp)
            If oBatchRenewalBusiness Is Nothing Then
                Throw New ApplicationException("Failed to create instance of bSIRRenewalBusiness.Business")
            End If
            If bPutOnnextInstalmentRenewal Then
                m_lReturn = gPMConstants.PMEReturnCode.PMTrue
                nOriginalInsuranceFileCnt =
                    gPMFunctions.ToSafeLong(r_vRenewalList(PMFieldPosLatestInstalmentPlanInsuranceFileCnt, v_lCount), 0)
            Else

                nOriginalInsuranceFileCnt = CInt(r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount))
                'do we have instalment plan on this policy

                If (sOptionValue.ToString = "1") Then

                    m_lReturn = oBatchRenewalBusiness.HasInstalmentPlanOnCurrentTerm(nInsuranceFileCnt:=m_oInsuranceFile.InsuranceFileCnt,
                                                                  sPaymentMethod:=sPayment_Method,
                                                                  nLatestInstalmentInsuranceFileCnt:=
                                                                     lInstalmentInsFileCnt)
                    If ToSafeInteger(lInstalmentInsFileCnt) = 0 Then
                        m_lReturn = gPMConstants.PMEReturnCode.PMNotFound
                    Else
                        m_lReturn = gPMConstants.PMEReturnCode.PMTrue
                    End If
                Else
                    If (CDbl(oUseNbPaymentTermAtRenSelection(0, 0)) = 0) Then
                        m_lReturn = m_oBusiness.IsInstalment(v_lInsuranceFileCnt:=(r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount)))
                    Else
                        m_lReturn = m_oBusiness.IsInstalment(v_lInsuranceFileCnt:=lInstalmentInsFileCnt)
                    End If
                End If

            End If

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                'create quote plan for the renewal version

                m_lReturn = m_oBusiness.CreateInstalmentQuote(v_lOriginalInsuranceFileCnt:=lInstalmentInsFileCnt,
                                                              v_lRenewalInsuranceFileCnt:=nNewInsuranceFileCnt,
                                                              v_lpartyCnt:=lInsuredCnt,
                                                              r_sFailureMessage:=sFailureMessage,
                                                              v_lProductId:=
                                                                 gPMFunctions.ToSafeLong(
                                                                     r_vRenewalList(PMFieldPosProductID, v_lCount)))

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'mark it as manual renewal
                    nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview
                End If


                m_lReturn = m_oBusiness.IsInstalment(v_lInsuranceFileCnt:=IIf(CDbl(oUseNbPaymentTermAtRenSelection(0, 0)) = 0, (r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount)), nNewInsuranceFileCnt))
                If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound AndAlso (sPayment_Method = "direct debit" Or sPayment_Method = "credit card" Or sPayment_Method = "instalments") Then
                    m_lReturn = m_oBusiness.UpdatePaymentMethod(nNewInsuranceFileCnt)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        sFailureMessage = "Failed to update payment method"
                    End If
                End If

            ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then

                sFailureMessage = "Check for existing instalment plan failed for Policy ID " &
                                  CStr(r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount))
            End If

            If bPutOnnextInstalmentRenewal Then
                m_lReturn = gPMConstants.PMEReturnCode.PMTrue
                nOriginalInsuranceFileCnt =
                    gPMFunctions.ToSafeLong(r_vRenewalList(PMFieldPosLatestInstalmentPlanInsuranceFileCnt, v_lCount), 0)
            Else

                nOriginalInsuranceFileCnt = CInt(r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount))
                'do we have instalment plan and Active Party Bank on this policy

                m_lReturn =
                    m_oBusiness.IsInstalmentAndActivePartyBank(
                        v_lInsuranceFileCnt:=r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount),
                        r_vResults:=oResultsInst)
            End If

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                If Information.IsArray(oResultsInst) Then


                    If CStr(oResultsInst(0, 0)) = "1" AndAlso CStr(oResultsInst(1, 0)) = "0" Then
                        'mark it as manual renewal
                        nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview

                        sFailureMessage = "Party Bank Account is inactive for Policy ID " &
                                          CStr(r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount))
                    End If
                End If

            End If

            m_oInsuranceFileBusiness.ValidateProductBranch(m_oInsuranceFile.ProductId, m_oInsuranceFile.SourceId,
                                                           bIsValid)
            If bIsValid <> True Then
                nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                sFailureCriterion = " The Product is not available through the selected Branch. " &
                                    "You must amend the renewal before the renewal can be accepted."
                sFailureMessage = sFailureCriterion
            End If

            'if we didn't fail then check broker transfer portfolio
            If sFailureMessage = "" Then


                m_lReturn =
                    m_oBusiness.GetBrokerTransferPortfolioDetail(
                        v_lInsuranceFileCnt:=CInt(r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount)),
                        r_vResultArray:=oBrokerTransferPorfolio)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    sFailureMessage = "Failed to check broker transfer portfolio"
                Else
                    If Information.IsArray(oBrokerTransferPorfolio) Then
                        If gPMFunctions.ToSafeLong(oBrokerTransferPorfolio(1, 0), 0) = 1 Then
                            nBrokerXferStatusTypeID = nRenewalStatusTypeID
                            nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeAwaitBrokerTransfer
                        End If
                    End If
                End If
            End If

            If sFailureMessage <> "" Then

                m_lReturn = m_oBusiness.AddRenewalReport(v_sReportType:="ManualRenewal",
                                                         v_vClientName:=r_vRenewalList(PMFieldPosClientName, v_lCount),
                                                         v_vPolicyNumber:=
                                                            r_vRenewalList(PMFieldPosInsuranceRef, v_lCount),
                                                         v_vAgentCode:=r_vRenewalList(PMFieldPosAgentName, v_lCount),
                                                         v_vCoverStartDate:=
                                                            r_vRenewalList(PMFieldPosCoverStartDate, v_lCount),
                                                         v_vCoverEndDate:=
                                                            r_vRenewalList(PMFieldPosCoverEndDate, v_lCount),
                                                         v_vProductCode:=
                                                            r_vRenewalList(PMFieldPosProductCode, v_lCount),
                                                         v_vFailureCriterion:="", v_vFailureDetail:=sFailureMessage,
                                                         v_vInsuranceFileCnt:=
                                                            r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount))

            End If

            If v_lDispStatusBar = gPMConstants.PMEReturnCode.PMTrue Then
                StatusBar1.Items.Item("Message").Text = "Copying Policy - Adding renewal status " &
                                                        "record"
                Me.StatusBar1.Refresh()
            End If


            m_lReturn = m_oBusiness.AddRenewalStatus(v_lProductId:=r_vRenewalList(PMFieldPosProductID, v_lCount),
                                                     v_lRenewalStatusTypeID:=nRenewalStatusTypeID,
                                                     v_lInsuranceHolderCnt:=
                                                        r_vRenewalList(PMFieldPosInsuranceHolderCnt, v_lCount),
                                                     v_lInsuranceFileCnt:=
                                                        r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount),
                                                     v_vLeadAgentCnt:=r_vRenewalList(PMFieldPosLeadAgentCnt, v_lCount),
                                                     v_lRenewalInsuranceFileCnt:=nNewInsuranceFileCnt,
                                                     v_lBrokerXferStatusTypeID:=nBrokerXferStatusTypeID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'as far as Silent Renewal is concerned its now in renewal
                nResult = gPMConstants.PMEReturnCode.PMFalse
            End If

            '1.12 Wr25
            ' get the Product Id of New Insurancefile cnt

            m_lReturn = m_oBusiness.SelectRenewalProduct(nNewInsuranceFileCnt, nRenewalProductId)

            If _
                gPMFunctions.ToSafeLong(m_oInsuranceFile.RenewalProductID) <> gPMFunctions.ToSafeLong(nRenewalProductId) And
                gPMFunctions.ToSafeLong(nRenewalProductId) > 0 Then
                ' update the renewal product in the base insurancefile then delete the renewal version

                m_lReturn =
                    m_oBusiness.UpdateRenewalProduct(
                        v_lInsuranceFileCnt:=r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount),
                        v_lProductId:=nRenewalProductId)

                m_lReturn = m_oBusiness.DeleteRenewal(nNewInsuranceFileCnt)
                Return nResult
            End If


            'RWH(16/11/2000) if the policy is not elligible for renewal then we have already
            'added a record to the report.
            If _
                (m_lReturn = gPMConstants.PMEReturnCode.PMTrue) AndAlso
                (lEligibleForRenewal = gPMConstants.PMEReturnCode.PMTrue) Then
                'add to Renewal_Report table
                If v_lDispStatusBar = gPMConstants.PMEReturnCode.PMTrue Then
                    StatusBar1.Items.Item("Message").Text = "Copying Policy - Adding renewal report " &
                                                            "record"
                    Me.StatusBar1.Refresh()
                End If

                'If (lRenewalStatusTypeID <>  PMBRenewalStatusTypeAutoRated)  AND (lRenewalStatusTypeID  <> PMBRenewalStatusTypeAwaitUpdate) And
                'bRenewalStatusAutomatic = ((lRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeAutoRated) Or ((lRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeAwaitUpdate) And (Not v_bTMPAnniversary And bIsTrueMonthlyPolicy)))
                bRenewalStatusAutomatic = ((nRenewalStatusTypeID = PMBRenewalStatusTypeAutoRated) Or
                                           ((nRenewalStatusTypeID = PMBRenewalStatusTypeAwaitUpdate) And
                                            (v_bTMPAnniversary = False And bIsTrueMonthlyPolicy = True)) Or
                                           (nRenewalStatusTypeID = PMBRenewalStatusTypeAwaitBrokerTransfer))
                If sFailureCriterion <> "" OrElse bRenewalStatusAutomatic Then

                    m_lReturn = m_oBusiness.AddRenewalReport(
                        v_sReportType:=IIf(bRenewalStatusAutomatic, "AutoRenewal", "ManualRenewal"),
                        v_vClientName:=r_vRenewalList(PMFieldPosClientName, v_lCount),
                        v_vPolicyNumber:=r_vRenewalList(PMFieldPosInsuranceRef, v_lCount),
                        v_vAgentCode:=r_vRenewalList(PMFieldPosAgentName, v_lCount),
                        v_vCoverStartDate:=m_oInsuranceFile.CoverStartDate,
                        v_vCoverEndDate:=m_oInsuranceFile.ExpiryDate,
                        v_vProductCode:=r_vRenewalList(PMFieldPosProductCode, v_lCount),
                        v_vFailureCriterion:=sFailureCriterion, v_vFailureDetail:="", v_vInsuranceFileCnt:=nNewInsuranceFileCnt)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If

            If sFailureCriterion <> "" Then
                sFailureCriterion = "Renewal - " & sInsuranceRef & " - " & sFailureCriterion

                If v_lDispStatusBar = gPMConstants.PMEReturnCode.PMTrue Then
                    StatusBar1.Items.Item("Message").Text = "Copying Policy - Add task to work manager"
                    Me.StatusBar1.Refresh()
                End If

                ReDim vKeyArray(1, 1)


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = "insurance_file_cnt"

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = nNewInsuranceFileCnt

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameRunMode

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = 1


                m_lReturn = m_oBusiness.AddTaskToWorkManager(
                    v_sClientName:=r_vRenewalList(PMFieldPosClientName, v_lCount), v_sDescription:=sFailureCriterion,
                    v_dtDueDate:=DateAndTime.DateAdd("ww", 2, DateTime.Today), v_vKeyArray:=vKeyArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                End If
                Return nResult

            Else

                If Not (m_obSIRRenewal Is Nothing) Then


                    m_lReturn = m_obSIRRenewal.GenerateCustomerRenewalEmail(v_lpartyCnt:=lInsuredCnt,
                                                                            v_lInsuranceFileCnt:=nNewInsuranceFileCnt,
                                                                            v_sType:="selection")
                    If _
                        m_lReturn <> gPMConstants.PMEReturnCode.PMTrue AndAlso
                        m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                        ' Call m_oInsuranceFile.RollbackTrans
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

            End If


            If m_oInsuranceFile.BusinessType.Trim() = "AGENCY" Then

                m_lReturn = ValidateCertificateYear(bIsValid, nNewInsuranceFileCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to getCertificate Year for " & m_sTransactionType & ".", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateRenewalPolicy")
                End If
            End If
            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateRenewalPolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateRenewalPolicy", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")
            Return nResult
        End Try
    End Function

    ''' <summary>
    ''' copy all Risks attached to OldInsuranceFileCnt to NewInsuranceFileCnt
    ''' copy all GIS details attached to each risk to NewInsuranceFileCnt
    ''' </summary>
    ''' <param name="r_vRenewalList"></param>
    ''' <param name="v_lCount"></param>
    ''' <param name="v_lNewInsuranceFileCnt"></param>
    ''' <param name="r_lEligibleForRenewal"></param>
    ''' <param name="r_sFailureReason"></param>
    ''' <param name="v_bIsTrueMonthlyPolicy"></param>
    ''' <param name="v_bTMPAnniversary"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CopyRiskData(ByRef r_vRenewalList(,) As Object, ByVal v_lCount As Object, ByVal v_lNewInsuranceFileCnt As Object, ByRef r_lEligibleForRenewal As Integer, ByRef r_sFailureReason As String, Optional ByVal v_bIsTrueMonthlyPolicy As Object = Nothing, Optional ByVal v_bTMPAnniversary As Object = Nothing, Optional ByRef r_bIsReferred As Boolean = False) As Integer

        Dim nResult As Integer
        Dim nNewGisPolicyLinkID As Integer
        Dim oGisPolicyLinkArray(,) As Object = Nothing
        Dim oRiskArray(,) As Object = Nothing
        Dim nNewRiskCnt As Integer
        Dim sXMLDataSetDef As String = String.Empty
        Dim sXMLDataSet As String = String.Empty
        Dim nOldPolicyBinderId As Integer = 0
        Dim nNewPolicyBinderId As Integer = 0
        Dim sFailureDetail As String = String.Empty
        Dim sFailureCriterion As String = String.Empty
        Dim oRiskTax As Object
        Dim sDescription As String = ""
        Dim nTransactionType As Integer = 0
        Dim nQuoteType As Integer = 0
        ' Changes for new reinsurance
        Dim nReinsPremiumOrSumInsured As Integer = 0
        Dim nReinsBand As Integer = 0
        Dim bIsRIValid As Boolean
        Const kFieldPosRiskID As Integer = 0
        Const kFieldPosGisScreenID As Integer = 21

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue
            r_lEligibleForRenewal = gPMConstants.PMEReturnCode.PMTrue

            StatusBar1.Items.Item("Message").Text = "Copying Risks - Get relevant risks"
            Me.StatusBar1.Refresh()

            m_oRiskData.TransactionType = "REN"

            'get all risks associate with OldInsuranceFileCnt

            If _
                m_oRiskData.GetRisk(v_lInsuranceFileCnt:=r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount),
                                    r_vResultArray:=oRiskArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sFailureReason = "Getting Risk"
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'do we have any risks
            If Not Information.IsArray(oRiskArray) Then
                'RWH(16/11/2000) We do not need to report an error if there are no risks.
                r_sFailureReason = "No risks found"
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'loop thro and copy each risk details

            For iCount As Integer = 0 To oRiskArray.GetUpperBound(1)
                StatusBar1.Items.Item("Message").Text = "Copying Risk Data"
                'copy risk to NewInsuranceFileCnt

                m_lReturn = m_oRiskData.CopyRisk(v_lNewInsuranceFileCnt:=v_lNewInsuranceFileCnt,
                                                 v_vRiskDetail:=oRiskArray, v_lPosNo:=iCount,
                                                 r_lRiskCnt:=nNewRiskCnt,
                                                 v_lResetStatus:=gPMConstants.PMEReturnCode.PMTrue)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sFailureReason = "Copy Risk"
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'prepare details to copy GIS Stuff attached to current risk
                'get policy link detail
                ' Pass folder_cnt instead of file_cnt.
                m_lReturn =
                    m_oRiskData.GetGISPolicyLink(
                        v_lInsuranceFolderCnt:=r_vRenewalList(PMFieldPosInsuranceFolderCnt, v_lCount),
                        v_lRiskID:=oRiskArray(ACRiskPosCnt, iCount), r_vResultArray:=oGisPolicyLinkArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sFailureReason = "GetGISPolicyLink"
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'do we have any data
                Dim auxVar_2 As Object = oGisPolicyLinkArray(0, 0)


                If Not (Convert.IsDBNull(auxVar_2) OrElse IsNothing(auxVar_2)) Then
                    'Make sure GIS object present.
                    m_lReturn = m_oBusiness.GIS_LoadFromDB(CStr(oGisPolicyLinkArray(4, 0)).Trim(),
                                                           r_vRenewalList(PMFieldPosInsuranceFolderCnt, v_lCount),
                                                           oGisPolicyLinkArray(0, 0), oRiskArray(0, iCount)) _
                    'copy GIS details to NewInsuranceFileCnt
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sFailureReason = "LoadFromDB"
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'RWH(20/11/2000) REMEMBER we are storing folder_cnt in file_cnt field now !!!!!
                    'So we pass existing folder_cnt in for old and new file_cnt.


                    m_lReturn = m_oBusiness.CopyDataSet(v_sDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim(),
                                                        r_lNewGISPolicyLinkId:=nNewGisPolicyLinkID,
                                                        r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataSet:=sXMLDataSet,
                                                        v_vOldGISPolicyLinkId:=oGisPolicyLinkArray(0, 0),
                                                        v_vOldInsuranceFileCnt:=
                                                           r_vRenewalList(PMFieldPosInsuranceFolderCnt, v_lCount),
                                                        v_vOldRiskID:=oRiskArray(0, iCount),
                                                        v_vNewInsuranceFileCnt:=
                                                           r_vRenewalList(PMFieldPosInsuranceFolderCnt, v_lCount),
                                                        v_vNewRiskID:=nNewRiskCnt)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sFailureReason = "CopyDataSet"
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Initialise the Data Set with the Object/Properties
                    m_lReturn = m_oBusiness.LoadFromXML(v_sXMLDataSetDef:=sXMLDataSetDef, v_sXMLDataSet:=sXMLDataSet)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sFailureReason = "LoadFromXML"
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oBusiness.GIS_SaveToDB(v_sGisDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim())
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sFailureReason = "SaveToDB"
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Get Policy Binder Ids
                    m_lReturn = m_oBusiness.GetPolicyBinderId(v_sDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim(),
                                                              v_lGISPolicyLinkId:=nNewGisPolicyLinkID,
                                                              r_lPolicyBinderId:=nNewPolicyBinderId)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sFailureReason = "GetPolicyBinderId"
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = m_oBusiness.GetPolicyBinderId(v_sDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim(),
                                                              v_lGISPolicyLinkId:=oGisPolicyLinkArray(0, 0),
                                                              r_lPolicyBinderId:=nOldPolicyBinderId)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sFailureReason = "GetPolicyBinderId"
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' start (run the renewal scripts)
                    m_lReturn = m_oBusiness.DeleteOutputTable(v_sDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim(),
                                                              v_lPolicyBinderId:=nNewPolicyBinderId)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sFailureReason = "DeleteOutputTable"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vTransactionType:="REN")
                    nTransactionType = m_oBusiness.TransactionType
                    EncodeTransactionScreenAndType(nQuoteType, nTransactionType, 0, 5)

                    'run renewal script


                    m_lReturn = m_oBusiness.GIS_NBQuote(v_sGisDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim(),
                                                        v_lQuoteType:=nQuoteType, r_sXMLDataSet:=sXMLDataSet,
                                                        r_sXMLDataSetDef:=sXMLDataSetDef)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.Chr(13) & Strings.Chr(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "Failed Renewal Script"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = m_oBusiness.GIS_SaveToDB(v_sGisDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim())
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.Chr(13) & Strings.Chr(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "SaveToDB"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Clear rating output variable before new test.
                    sFailureDetail = ""
                    'Check Output table to see if risk has been referred or declined.


                    m_lReturn = m_oBusiness.CheckOutputTable(v_sDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim(),
                                                             v_lPolicyBinderId:=nNewPolicyBinderId,
                                                             r_sReasons:=sFailureDetail)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.Chr(13) & Strings.Chr(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "CheckOutputTable"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If


                    StatusBar1.Items.Item("Message").Text = "Copying Risk Data - Standard Wordings"
                    Me.StatusBar1.Refresh()


                    m_lReturn = m_oBusiness.CopyRiskStandardWordings(v_lOldPolicyBinderId:=nOldPolicyBinderId, v_lNewPolicyBinderId:=nNewPolicyBinderId, v_sDataModelCode:=CStr(oGisPolicyLinkArray.GetValue(4, 0)).Trim(), v_dtEffectiveDate:=CDate(r_vRenewalList(PMFieldPosCoverEndDate, v_lCount)).AddYears(1))

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.Chr(13) & Strings.Chr(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "CopyRiskStandardWordings"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    StatusBar1.Items.Item("Message").Text = "Copying risk data - Index linking"
                    Me.StatusBar1.Refresh()
                    If (v_bTMPAnniversary AndAlso v_bIsTrueMonthlyPolicy) OrElse (Not v_bIsTrueMonthlyPolicy) Then
                        ' INDEX LINKING GIS STUFF 
                        Dim auxVar As Object = oRiskArray(kFieldPosGisScreenID, iCount)


                        If Not (Convert.IsDBNull(auxVar) Or IsNothing(auxVar)) Then
                            'index link GIS
                            m_lReturn = m_oBusiness.GisIndexLink(v_lInsuranceFileCnt:=v_lNewInsuranceFileCnt,
                                                                 v_lRiskID:=oRiskArray(kFieldPosRiskID, iCount),
                                                                 v_vGisScreenID:=
                                                                    oRiskArray(kFieldPosGisScreenID, iCount),
                                                                 v_dtEffectiveDate:=
                                                                    CDate(r_vRenewalList(PMFieldPosCoverStartDate,
                                                                                         v_lCount)).AddYears(1),
                                                                 v_sGisDataModelCode:=
                                                                    CStr(oGisPolicyLinkArray(4, 0)).Trim())
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                If r_sFailureReason <> "" Then
                                    r_sFailureReason = r_sFailureReason & Strings.Chr(13) & Strings.Chr(10)
                                End If
                                r_sFailureReason = r_sFailureReason & "Index Link"
                                nResult = gPMConstants.PMEReturnCode.PMFalse
                            End If
                        End If
                    End If

                    ' check to see if this option is in use
                    If m_bExtraRiskDetails Then
                        StatusBar1.Items.Item("Message").Text = "Copying risk data - Sum insured"
                        Me.StatusBar1.Refresh()
                        'copy RSA_Sum_Insured

                        m_lReturn = m_oRiskData.CopyRSASumInsured(v_lOldPolicyLinkID:=oGisPolicyLinkArray(0, 0),
                                                                  v_lNewPolicyLinkID:=nNewGisPolicyLinkID)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            If r_sFailureReason <> "" Then
                                r_sFailureReason = r_sFailureReason & Strings.Chr(13) & Strings.Chr(10)
                            End If
                            r_sFailureReason = r_sFailureReason & "CopyRSASumInsured"
                            nResult = gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If

                    StatusBar1.Items.Item("Message").Text = "Copying risk data - Prepare output table"
                    Me.StatusBar1.Refresh()

                    m_lReturn = m_oBusiness.DeleteOutputTable(v_sDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim(),
                                                              v_lPolicyBinderId:=nNewPolicyBinderId)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.Chr(13) & Strings.Chr(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "DeleteOutputTable"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    StatusBar1.Items.Item("Message").Text = "Copying risk data - Underwriting authority " &
                                                            "limits"
                    Me.StatusBar1.Refresh()
                    EncodeTransactionScreenAndType(nQuoteType, nTransactionType, 0, 3)

                    'Check Underwriting Authority Limits.


                    m_lReturn = m_oBusiness.GIS_NBQuote(v_sGisDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim(),
                                                        v_lQuoteType:=nQuoteType, r_sXMLDataSet:=sXMLDataSet,
                                                        r_sXMLDataSetDef:=sXMLDataSetDef)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.Chr(13) & Strings.Chr(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "Check Underwriting Authority Limits"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = m_oBusiness.GIS_SaveToDB(v_sGisDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim())
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.Chr(13) & Strings.Chr(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "SaveToDB"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Check Output table to see if risk has been referred or declined.
                    m_lReturn = m_oBusiness.CheckOutputTable(v_sDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim(),
                                                             v_lPolicyBinderId:=nNewPolicyBinderId,
                                                             r_sReasons:=sFailureDetail)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.Chr(13) & Strings.Chr(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "CheckOutputTable"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    StatusBar1.Items.Item("Message").Text = "Copying risk data - Prepare output table"
                    Me.StatusBar1.Refresh()


                    m_lReturn = m_oBusiness.DeleteOutputTable(v_sDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim(),
                                                              v_lPolicyBinderId:=nNewPolicyBinderId)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.Chr(13) & Strings.Chr(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "DeleteOutputTable2"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    StatusBar1.Items.Item("Message").Text = "Copying risk data - Quote risk"
                    Me.StatusBar1.Refresh()
                    EncodeTransactionScreenAndType(nQuoteType, nTransactionType, 0, 1)

                    'Quote risk.
                    m_lReturn = m_oBusiness.GIS_NBQuote(v_sGisDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim(),
                                                        v_lQuoteType:=nQuoteType, r_sXMLDataSet:=sXMLDataSet,
                                                        r_sXMLDataSetDef:=sXMLDataSetDef)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.Chr(13) & Strings.Chr(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "Failed to Quote"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = m_oBusiness.GIS_SaveToDB(v_sGisDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim())
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.Chr(13) & Strings.Chr(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "SaveToDB"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    StatusBar1.Items.Item("Message").Text = "Copying risk data - Check output table"
                    Me.StatusBar1.Refresh()
                    'Check Output table to see if risk has been referred or declined.


                    m_lReturn = m_oBusiness.CheckOutputTable(v_sDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim(),
                                                             v_lPolicyBinderId:=nNewPolicyBinderId,
                                                             r_sReasons:=sFailureDetail)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.Chr(13) & Strings.Chr(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "CheckOutputTable"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If


                    'PerilAllocation - Rating Sections
                    StatusBar1.Items.Item("Message").Text = "Copying risk data - Peril allocation"
                    Me.StatusBar1.Refresh()
                    'Set required params for PerilAllocation

                    m_oPerilAllocation.InsuranceFileCnt = v_lNewInsuranceFileCnt


                    m_oPerilAllocation.InsuranceFolderCnt = r_vRenewalList(PMFieldPosInsuranceFolderCnt, v_lCount)

                    m_oPerilAllocation.RiskId = nNewRiskCnt

                    'RWH(24/08/01) Change put in for Tom.

                    m_oPerilAllocation.TransactionType = "REN"

                    'Do PerilAllocation/Rating Sections stuff.

                    m_lReturn = m_oPerilAllocation.PopulateRatingSections

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.Chr(13) & Strings.Chr(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "PopulateRatingSections"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'RWH(24/08/01) Change put in for Tom.
                    'Update the risk premium

                    m_lReturn = m_oPerilAllocation.UpdateRisk


                    'RWH(13/08/01) Moved the reinsurance part from within condition above.
                    'Reinsurance should be copied whether rating has succeeded or not.
                    StatusBar1.Items.Item("Message").Text = "Copying risk data - Risk reinsurance"
                    Me.StatusBar1.Refresh()
                    ' Set RI properties

                    m_oReinsurance.InsuranceFileCnt = v_lNewInsuranceFileCnt

                    m_oReinsurance.RiskId = nNewRiskCnt

                    If _
                        ((v_bIsTrueMonthlyPolicy AndAlso nTransactionType = 10) AndAlso Not v_bTMPAnniversary) AndAlso
                        ToSafeInteger(r_vRenewalList(PMFieldPosTMPAutoRenFAC, v_lCount)) = 1 Then
                        m_oReinsurance.TMPRiskCntUnderRenewal = ToSafeLong(oRiskArray(ACRiskPosCnt, iCount))
                    End If

                    ' Calculate RI

                    m_lReturn = m_oReinsurance.CalculateRI
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue AndAlso m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.Chr(13) & Strings.Chr(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "Calculating Reinsurance"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Load details to validate

                    m_lReturn = m_oReinsurance.Getdetails
                    If _
                        m_lReturn <> gPMConstants.PMEReturnCode.PMTrue AndAlso
                        m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.Chr(13) & Strings.Chr(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "Getting Reinsurance Details"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Update details, this will ensure minor rounding is handled

                    m_lReturn = m_oReinsurance.Update
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.Chr(13) & Strings.Chr(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "Updating Reinsurance"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Validate the reinsurance is all OK
                    StatusBar1.Items.Item("Message").Text = "Copying risk data - Validating reinsurance"
                    Me.StatusBar1.Refresh()
                    m_lReturn = m_oReinsurance.ValidateBands(nReinsPremiumOrSumInsured, nReinsBand)

                    ' Must return true AND a zero for lReinsPremiumOrSumInsured
                    bIsRIValid = (m_lReturn = gPMConstants.PMEReturnCode.PMTrue) AndAlso (nReinsPremiumOrSumInsured = 0)
                    If Not bIsRIValid Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.Chr(13) & Strings.Chr(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "Validating Reinsurance"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    StatusBar1.Items.Item("Message").Text = "Copying risk data - Risk tax"
                    Me.StatusBar1.Refresh()

                    m_oTax.RiskCnt = nNewRiskCnt
                    m_oTax.InsuranceFileCnt = v_lNewInsuranceFileCnt

                    m_lReturn = m_oTax.GetRiskTax(oRiskTax, sDescription)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.Chr(13) & Strings.Chr(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "GetRiskTax"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'set risk status to QUOTED if reinsurance is complete, Unquoted otherwise
                    If sFailureDetail <> "" Then

                        If sFailureDetail.Substring(0, 8) = "DECLINED" Then
                            m_lReturn = m_oRiskData.UpdateRiskStatus(v_lRiskCnt:=nNewRiskCnt,
                                                                     v_lRiskStatusID:=2)
                        ElseIf sFailureDetail.Substring(0, 8) = "REFERRED" Then

                            m_lReturn = m_oRiskData.UpdateRiskStatus(v_lRiskCnt:=nNewRiskCnt,
                                                                    v_lRiskStatusID:=1)
                        End If
                    Else
                        m_lReturn = m_oRiskData.UpdateRiskStatus(v_lRiskCnt:=nNewRiskCnt,
                                                                 v_lRiskStatusID:=IIf(bIsRIValid = True, 3, 4))
                    End If
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.Chr(13) & Strings.Chr(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "Failed to update risk status"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    IF r_sFailureReason.Contains("Failed to Quote") then
                         m_lReturn = m_oRiskData.UpdateRiskStatus(v_lRiskCnt:=nNewRiskCnt,
                                                                    v_lRiskStatusID:=4)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then                         
                            r_sFailureReason = r_sFailureReason & "Failed to update risk status"
                            nResult = gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If


                    If sFailureDetail <> "" Then
                        StatusBar1.Items.Item("Message").Text = "Copying risk data - Add renewal report " &
                                                                "record"
                        Me.StatusBar1.Refresh()

                        r_lEligibleForRenewal = gPMConstants.PMEReturnCode.PMFalse
                        sFailureCriterion = PMBConst.PMFailedReRateDesc
                        If sFailureDetail.Contains("REFERRED") Then
                            r_bIsReferred = True
                        End If

                        m_lReturn = m_oBusiness.AddRenewalReport(v_sReportType:="ManualRenewal",
                                                                 v_vClientName:=
                                                                    r_vRenewalList(PMFieldPosClientName, v_lCount),
                                                                 v_vPolicyNumber:=
                                                                    r_vRenewalList(PMFieldPosInsuranceRef, v_lCount),
                                                                 v_vAgentCode:=
                                                                    r_vRenewalList(PMFieldPosAgentName, v_lCount),
                                                                 v_vCoverStartDate:=
                                                                    r_vRenewalList(PMFieldPosCoverStartDate, v_lCount),
                                                                 v_vCoverEndDate:=
                                                                    r_vRenewalList(PMFieldPosCoverEndDate, v_lCount),
                                                                 v_vProductCode:=
                                                                    r_vRenewalList(PMFieldPosProductCode, v_lCount),
                                                                 v_vFailureCriterion:=sFailureCriterion,
                                                                 v_vFailureDetail:=sFailureDetail,
                                                                 v_vInsuranceFileCnt:=
                                                                    r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount))

                    End If

                Else
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    r_sFailureReason = "No Gis detail"

                    ' Log Error Message


                    Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                    oDict.Add("v_lNewInsuranceFileCnt", v_lNewInsuranceFileCnt)
                    gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                  sMsg:="No Gis detail for InsuranceFileCnt:" &
                                                  CStr(r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount)) &
                                                  " RiskID:" & CStr(oRiskArray(1, iCount)), vApp:=ACApp,
                                                  vClass:=ACClass, vMethod:="CopyRiskData", oDicParms:=oDict)

                End If
            Next iCount

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyRiskData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")
            Return nResult
        End Try
    End Function

    ' ***************************************************************** '
    ' Name : SilentRenewal
    '
    ' Desc : 1. delete all renewal versions of this policy
    '           2. get correct version of policy for renewal
    '           3. create renewal version of policy
    '           4. index linking renewal version of policy
    '
    ' Hist :    13/02/2001 Created - Tinny
    '           12/08/2003 Tracy Richards - optionally returns
    '           r_bIsProductAutoRenewable to caller
    ' ***************************************************************** '

    Public Function SilentRenewal(ByVal v_lInsuranceFolderCnt As Object, Optional ByRef r_bIsProductAutoRenewable As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim vRenewalPolicy As Object 'should only have one policy in it
        Dim sInsuranceRef As String = ""
        Dim bRenewalsExist As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If v_lInsuranceFolderCnt = 0 Then
                m_lReturn = SelectPolicy()
            End If

            'RWH(15/05/2001) Put in extra shenaneghans to use this function for single
            'policy selected on this renewal form.
            If m_bCalledFromLocalForm Then
                StatusBar1.Items.Item("Message").Text = "Checking for existing versions in " &
                                                        "renewal..."
                Me.StatusBar1.Refresh()
                StatusBar1.Items.Item("Count").Text = "1/1"
                Me.StatusBar1.Refresh()


                m_lReturn = m_oBusiness.GetRenewalsForPolicy(v_lInsFolderCnt:=v_lInsuranceFolderCnt, r_bRenewalsExist:=bRenewalsExist)
                If bRenewalsExist Then
                    If MessageBox.Show("A version of this policy is already in renewal." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & "Do you wish to re-select the policy?", "Renewals", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) = System.Windows.Forms.DialogResult.Yes Then

                        StatusBar1.Items.Item("Message").Text = "Deleting existing versions in renewal..."
                        Me.StatusBar1.Refresh()
                        m_lReturn = m_oBusiness.DeleteRenewalsForPolicy()
                    Else
                        StatusBar1.Items.Item("Policy").Text = ""
                        Me.StatusBar1.Refresh()
                        StatusBar1.Items.Item("Message").Text = "Ready"
                        Me.StatusBar1.Refresh()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            Else
                'delete all versions of this policy from renewal

                m_lReturn = m_oBusiness.DeleteRenewalPolicy(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed To Delete Existing Renewal For Selected Policy", ACApp, MessageBoxButtons.OK)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'delete all in renewal_report table ready for new data

            If m_oBusiness.DelRenewalReport() <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed To Delete Previous Renewal Report", ACApp, MessageBoxButtons.OK)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_bCalledFromLocalForm Then
                StatusBar1.Items.Item("Message").Text = "Retrieving policy details"
                Me.StatusBar1.Refresh()
            End If

            'get correct version of this policy for renewal

            m_lReturn = m_oBusiness.GetPolicyForRenewal(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, r_vResultArray:=vRenewalPolicy)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed To Get Correct Version Of Policy For Renewal", ACApp, MessageBoxButtons.OK)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'create relevant business objects
            m_lReturn = CreateBusinessObject()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = CloseBusinessObject()
                Return result
            End If

            'Tracy Richards - Get the Product ID for this Policy

            r_bIsProductAutoRenewable = CDbl(vRenewalPolicy(9, 0)) = 1
            '3547- Priya
            If Not ToSafeBoolean(vRenewalPolicy(33, 0)) Then
                MessageBox.Show("Policy is not renewable. Kindly check the product configuration.", ACApp, MessageBoxButtons.OK)
                m_lReturn = CloseBusinessObject()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' default policy version additional increment
            m_lPolicyVersionIncrement = 0

            'create renewal version for this policy
            m_lReturn = CreateRenewalPolicyWrapper(r_vRenewalList:=vRenewalPolicy, v_lCount:=0, v_lDispStatusBar:=IIf(m_bCalledFromLocalForm, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed To Create Renewal Version Of Policy", ACApp, MessageBoxButtons.OK)
                m_lReturn = CloseBusinessObject()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' if this is a renewal policy on a true monthly policy
            ' determine whether or not we need to create an anniversary renewal policy

            m_lReturn = CreateTMPAnniversaryRenewal(r_vRenewalList:=vRenewalPolicy, v_lCount:=0, v_lDispStatusBar:=IIf(m_bCalledFromLocalForm, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("CreateTMPAnniversaryRenewal Failed", ACApp, MessageBoxButtons.OK)
                m_lReturn = CloseBusinessObject()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If m_bCalledFromLocalForm Then
                StatusBar1.Items.Item("Policy").Text = ""
                Me.StatusBar1.Refresh()
                StatusBar1.Items.Item("Message").Text = "Complete"
                Me.StatusBar1.Refresh()
            End If

            m_lReturn = CloseBusinessObject()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SilentRenewal Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SilentRenewal", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)
    ' ***************************************************************** '
    ' Name: PrintRenewalReport
    '
    ' Description: Print Renewal Reports
    '
    ' ***************************************************************** '


    Private Function PrintRenewalReport() As Integer
        Dim result As Integer = 0
        Dim oReport As iPMBReportPrint.Interface_Renamed

        Dim vReportKeys As Object

        Const PMReportAutoRenewal As String = "AutomaticRenewal"
        Const PMReportManualRenewal As String = "ManualRenewal"

        m_bManualRenewalsExist = False
        m_bAutoRenewalsExist = False

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oReport As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oReport, sClassName:="iPMBReportPrint.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oReport = temp_oReport

            If Not (oReport Is Nothing) Then

                StatusBar1.Items.Item("Message").Text = "Generating Manual Renewal Reports"
                Me.StatusBar1.Refresh()
                'RWH(24/05/2001) Check whether report records exist before displaying reports.

                m_lReturn = m_oBusiness.RenewalsReportExists("ManualRenewal", m_bManualRenewalsExist)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse

                End If

                If m_bManualRenewalsExist Then
                    ReDim vReportKeys(1, 2)

                    vReportKeys(0, 0) = "report_name"

                    vReportKeys(1, 0) = PMReportManualRenewal

                    vReportKeys(0, 1) = "report_print_options"

                    vReportKeys(1, 1) = m_iPrintMode
                    vReportKeys(0, 2) = "param_name1"
                    vReportKeys(1, 2) = g_oObjectManager.UserName

                    m_lReturn = oReport.SetKeys(vReportKeys)

                    m_lReturn = oReport.Start

                End If

                StatusBar1.Items.Item("Message").Text = "Generating Automatic Renewal Reports"
                Me.StatusBar1.Refresh()

                m_lReturn = m_oBusiness.RenewalsReportExists("AutoRenewal", m_bAutoRenewalsExist)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse

                End If

                If m_bAutoRenewalsExist Then

                    ReDim vReportKeys(1, 2)

                    vReportKeys(0, 0) = "report_name"

                    vReportKeys(1, 0) = PMReportAutoRenewal

                    vReportKeys(0, 1) = "report_print_options"

                    vReportKeys(1, 1) = m_iPrintMode
                    vReportKeys(0, 2) = "param_name1"
                    vReportKeys(1, 2) = g_oObjectManager.UserName

                    m_lReturn = oReport.SetKeys(vReportKeys)

                    m_lReturn = oReport.Start

                End If

            End If



            ''m_lReturn = oReport.Terminate
            ''oReport = Nothing

            'RWH(08/06/01) Inform user if no data found.
            If Not (m_bManualRenewalsExist Or m_bAutoRenewalsExist) Then
                MessageBox.Show("No data found for current criteria.", "Renewal Selection", MessageBoxButtons.OK, MessageBoxIcon.Information)
                'PN: 46877
                StatusBar1.Items.Item("Message").Text = "No Reports generated"
                Me.StatusBar1.Refresh()
            Else
                'PN: 46877
                StatusBar1.Items.Item("Message").Text = "Reports generated"
                Me.StatusBar1.Refresh()
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: InterfaceToData
    '
    ' Description: Updates the data storage from the interface details.
    '
    ' ***************************************************************** '
    Private Function InterfaceToData() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the data storage.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to assign all of the details from the
            ' interface to the data storage.
            '
            ' Example:-
            '
            '    m_DName$ = trim$(txtName.Text)
            '    m_DDate = CDate(txtDate.Text)
            '    m_iDCodeID% = cmbCode.ItemData(cmbCode.ListIndex)
            '    m_lReturn& = m_oFormFields.UnformatControl(txtName)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            'all product
            If VB6.GetItemData(cboProductCode, cboProductCode.SelectedIndex) = 0 Then

                m_vProductID = Nothing
            Else
                m_vProductID = VB6.GetItemData(cboProductCode, cboProductCode.SelectedIndex)
            End If

            'Thinh Nguyen 19/03/2002 (start)
            If VB6.GetItemData(cboSource, cboSource.SelectedIndex) = 0 Then

                m_vSourceID = Nothing
            Else
                m_vSourceID = VB6.GetItemData(cboSource, cboSource.SelectedIndex)
            End If
            'Thinh Nguyen 19/03/2002 (end)

            If optPrint(0).Checked Then
                m_iPrintMode = MainModule.AC_PRINT_ONLY
            Else
                m_iPrintMode = MainModule.AC_VIEW_ONLY
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        'RKS PN13438
        Dim result As Integer = 0
        Dim vValue As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            'default to first tab
            If SSTabHelper.GetTabVisible(Me.tabMainTab, 0) = True Then
                SSTabHelper.SetSelectedIndex(Me.tabMainTab, 0)
            Else
                SSTabHelper.SetSelectedIndex(Me.tabMainTab, 1)
            End If


            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()


            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = SetFirstLastControls()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            'Thinh Nguyen 19/03/2002 (start) - add extra param for table name
            m_lReturn = GetComboDetails(Me.cboProductCode, "Product")

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            result = GetComboDetails(Me.cboSource, "Source")
            'Thinh Nguyen 19/03/2002 (start) - add extra param for table name


            'SET 10/06/2002 Set the Extra Risk details option
            m_lReturn = UseExtraRiskData()
            'SET 10/06/2002

            'default renewal compare date to today
            dtEndDate.Value = DateTime.Today

            'RKS PN13438
            m_lReturn = iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTUnderwritingYear, 1, vValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_bUnderwritingYearID = vValue = "1"
            'RKS PN13438


            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetFirstLastControls
    '
    ' Description: Sets the first and last data entry controls for
    '              each tab to the control array, for use with the
    '              keyboard navigation.
    '
    ' ***************************************************************** '
    Private Function SetFirstLastControls() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise the control array with the number of
            ' tabs which contain data entry fields on (Remember
            ' that arrays start from zero, therefore you must
            ' subtract one from the number of tabs).
            ReDim m_ctlTabFirstLast(1, 0)

            ' Set the first and last data entry controls for
            ' all of the tabs.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to set the first and last data entry
            ' controls for all of the tabs.
            '
            ' Example:-
            '
            '    Set m_ctlTabFirstLast(ACControlStart, 0) = txtName
            '    Set m_ctlTabFirstLast(ACControlEnd, 0) = txtAge
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************
            'dtStartDate.Text = String.Format("{0: dd/MM/yyyy} ", CDate(dtStartDate.Text))
            m_ctlTabFirstLast(ACControlStart, 0) = dtStartDate
            m_ctlTabFirstLast(ACControlEnd, 0) = cboProductCode

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFirstLastControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayCaptions
    '
    ' Description: Display all language specific captions.
    '
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.

            'RWH(20/02/2001)
            StatusBar1.Items.Item("Message").Text = " Ready"
            Me.StatusBar1.Refresh()

            'Developer Guide No. 243
            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() &
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdRePrint.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACReprintButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to display all language specific
            ' captions.
            ' The iPMFunc.GetResData function will allow you to do this.
            '
            ' Example:-
            '
            '    lblDesc.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACDesc, _
            ''        iDataType:=PMResString)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            '    lblRenewalDate.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lId:=ACRenewalDate, _
            ''        iDataType:=PMResString)


            lblProductCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACProductCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblPolicyRef.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPolicyRef, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ValidateForm
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function ValidateForm() As Integer
        Dim result As Integer = 0
        Dim lResponse As DialogResult

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'RWH(15/05/01) Are we using Date/Product selection tab ?
            If SSTabHelper.GetTabVisible(tabMainTab, 0) Then 'Date/Product.
                'RWH(20/02/2001)
                If cboProductCode.Text.Trim().ToUpper() = "ALL" Then
                    lResponse = MessageBox.Show("Are you sure you wish to select ALL products ?", "Renewal Selection", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2)
                    If lResponse = System.Windows.Forms.DialogResult.Yes Then
                        result = gPMConstants.PMEReturnCode.PMTrue
                    Else
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
                If cboSource.Text.Trim().ToUpper() = "ALL" Then
                    lResponse = MessageBox.Show("Are you sure you wish to select ALL branches ?", "Renewal Selection", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2)
                    If lResponse = System.Windows.Forms.DialogResult.Yes Then
                        result = gPMConstants.PMEReturnCode.PMTrue
                    Else
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

            End If



            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateForm Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetComboDetails
    '
    ' Description: get details from numbering scheme and add to combobox
    '
    ' Hist : Thinh Nguyen 19/03/2002 - add extra param and code for table name
    '        SET 07/06/2002 - changed intial return value to PMTrue
    '
    ' ***************************************************************** '
    Private Function GetComboDetails(ByRef r_cboControl As ComboBox, ByVal v_sTableName As String) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object

        result = gPMConstants.PMEReturnCode.PMTrue 'PMFalse

        Try

            'make sure combobox is empty
            r_cboControl.Items.Clear()

            'add in non applicable value with ID of 0
            Dim r_cboControl_NewIndex As Integer = -1
            r_cboControl_NewIndex = r_cboControl.Items.Add("All")
            VB6.SetItemData(r_cboControl, r_cboControl_NewIndex, 0)

            Select Case v_sTableName.ToUpper()
                Case "PRODUCT"

                    m_lReturn = m_oBusiness.GetLookUp(v_sTableName:="Product", v_sKeyIDFieldName:="product_id", v_sDescFieldName:="description", r_vResultArray:=vResultArray)

                Case "SOURCE"

                    m_lReturn = m_oBusiness.GetLookUp(v_sTableName:="Source", v_sKeyIDFieldName:="source_id", v_sDescFieldName:="description", r_vResultArray:=vResultArray)

            End Select

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Information.IsArray(vResultArray) Then

                For icount As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

                    r_cboControl_NewIndex = r_cboControl.Items.Add(CStr(vResultArray(1, icount)))

                    VB6.SetItemData(r_cboControl, r_cboControl_NewIndex, CInt(vResultArray(0, icount)))
                Next
            End If

            'default to all products
            r_cboControl.SelectedIndex = 0

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetComboDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetComboDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: EncodeTransactionScreenAndType
    '
    ' Description: Encodes Transaction, Screen id and tYpe from encoded value
    '              Originally TTTSSYY
    '              Now        1TTTSSSSYY
    '
    ' History: 19/12/2001 CLG - Created.
    '
    ' ***************************************************************** '
    Public Sub EncodeTransactionScreenAndType(ByRef r_lEncoded As Integer, ByRef r_lTransactionType As Integer, ByRef r_lGISScreenId As Byte, ByRef r_lQuoteType As Byte)

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass &
                        ".EncodeTransactionScreenAndType")

        Try

            'new format 1TTTSSSSYY
            r_lEncoded = 1000000000 + (r_lTransactionType * 1000000) + (r_lGISScreenId * 100) + r_lQuoteType

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass &
                            ".EncodeTransactionScreenAndType")

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass &
                            ".EncodeTransactionScreenAndType")


            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EncodeTransactionScreenAndType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EncodeTransactionScreenAndType", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    'End Private methods

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click

        'm_lReturn = PMHelpFunc.ShowHelp(dlgHelp:=dlgHelp, lContextID:=ScreenhelpID)
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = PMHelpFunc.ShowHelp(cmdHelp, lContextID:=ScreenhelpID)
    End Sub

    Private Sub cmdRePrint_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRePrint.Click
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        'RWH(25/05/01) Set report options.
        If optPrint(0).Checked Then
            m_iPrintMode = MainModule.AC_PRINT_ONLY
        Else
            m_iPrintMode = MainModule.AC_VIEW_ONLY
        End If

        m_lReturn = PrintRenewalReport()

        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
    End Sub

    Private Sub cmdSelectPolicy_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSelectPolicy.Click
        m_lReturn = SelectPolicy()
    End Sub

    Private Sub dtStartDate_ValueChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles dtStartDate.ValueChanged

        'lblStartDateDesc.Visible = Convert.IsDBNull(dtStartDate.Value) Or IsNothing(dtStartDate.Value)
        lblStartDateDesc.Visible = Not (dtStartDate.Checked)

        lblStartDate.Enabled = Not (Convert.IsDBNull(dtStartDate.Value) Or IsNothing(dtStartDate.Value))
    End Sub

    Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not bolActivated Then
            bolActivated = True

            If Not (ActivateHelper.myActiveForm Is eventSender) Then
                ActivateHelper.myActiveForm = eventSender

                Dim sOptionValue As String = ""

                m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=155, r_sOptionValue:=sOptionValue, v_iSourceID:=g_iSourceID)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    sOptionValue = ""

                    gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to retrieve system option", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Activate")

                    Exit Sub
                End If

                If sOptionValue = "" Then
                    ' option not populated, no action
                    Exit Sub
                End If

                ' prompt user
                m_lReturn = Interaction.MsgBox(sOptionValue & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & "Do you wish to proceed?", CStr(MsgBoxStyle.Question) & CStr(MsgBoxStyle.YesNo), "Renewal Selection")

                If m_lReturn = System.Windows.Forms.DialogResult.Yes Then
                    ' proceed
                    Exit Sub
                End If

                ' don't proceed, close the form
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel

                Me.Hide()

            End If
        End If
    End Sub

    ' PRIVATE Methods (End)

    Private Sub Form_Initialize_Renamed()

        Dim sMessage, sTitle As String

        ' Forms initialise event.
        Try

            iPMFunc.ShowFormInTaskBar_Attach()

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue


            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(oObject:=temp_m_oBusiness, sClassName:="bSIRRenSelection.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub
            End If

            ' Create an instance of the general interface object.
            m_oGeneral = New iPMURenSelection.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Create an instance of the form control object.
            m_oFormFields = New iPMFormControl.FormFields()

            ' Set language
            m_oFormFields.LanguageID = g_iLanguageID


            ' Get an instance of the business object via
            ' the public object manager.

            Dim temp_m_obSIRRenewal As Object
            m_lReturn = g_oObjectManager.GetInstance(oObject:=temp_m_obSIRRenewal, sClassName:="bSIRRenewal.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_obSIRRenewal = temp_m_obSIRRenewal

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub
            End If

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Forms load event.

        Try

            iPMFunc.ShowFormInTaskBar_Detach()

            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the business keys.
            ' {* USER DEFINED CODE (Begin) *}

            ' {* USER DEFINED CODE (End) *}

            If m_lRenewalMode <> ACSilentRenewal Then
                ' Validate fields using Forms Control
                m_lReturn = SetFieldValidation()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If

                ' Set the interface default values.
                m_lReturn = SetInterfaceDefaults()

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If

                ' Gets the interface details to be displayed.
                m_lReturn = m_oGeneral.GetInterfaceDetails()

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to get the interface details.
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            If SSTabHelper.GetTabVisible(tabMainTab, 1) Then

                _tabMainTab_TabPage1.Select()

            Else
                cboProductCode.Select()
            End If
        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            m_lReturn = CloseBusinessObject()

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = m_oGeneral.ProcessCommand()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    eventArgs.Cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If

            ' Terminate the general object.
            m_oGeneral.Dispose()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing

            ' Terminate the business object


            ' Terminate the form control object.
            m_oFormFields.Dispose()
            ' Destroy the instance of the form control object
            ' from memory.
            m_oFormFields = Nothing

            If Not (m_oBusiness Is Nothing) Then

                m_oBusiness.Dispose()
                m_oBusiness = Nothing
            End If

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    'Private Sub frmInterface_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
    '    Dim KeyCode As Integer = eventArgs.KeyCode
    '    Dim Shift As Integer = eventArgs.KeyData \ &H10000

    '    Dim iCtrlDown As Integer

    '    Const ACCtrlMask As Integer = 2

    '    Try

    '        ' Set the control key value.
    '        iCtrlDown = (Shift And ACCtrlMask) > 0

    '        With tabMainTab
    '            ' Check the key pressed.
    '            Select Case KeyCode
    '                Case Keys.PageUp
    '                    ' Page Up key has been pressed.

    '                    ' Check if the control key has also
    '                    ' been pressed.
    '                    If iCtrlDown Then
    '                        ' Display the first tab.
    '                        SSTabHelper.SetSelectedIndex(tabMainTab, 0)
    '                    Else
    '                        ' Check we are not on the
    '                        ' first tab.
    '                        If SSTabHelper.GetSelectedIndex(tabMainTab) > 0 Then
    '                            ' Display the previous tab.
    '                            SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) - 1)
    '                        End If
    '                    End If

    '                Case Keys.PageDown
    '                    ' Page Down key has been pressed.

    '                    ' Check if the control key has also
    '                    ' been pressed.
    '                    If iCtrlDown Then
    '                        ' Display the last tab.
    '                        SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetTabCount(tabMainTab) - 1)
    '                    Else
    '                        ' Check we are not on the
    '                        ' last tab.
    '                        If SSTabHelper.GetSelectedIndex(tabMainTab) < (SSTabHelper.GetTabCount(tabMainTab) - 1) Then
    '                            ' Display the next tab.
    '                            SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) + 1)
    '                        End If
    '                    End If

    '                Case Keys.Home
    '                    ' Home key has been pressed.

    '                    ' Check if the control key has also
    '                    ' been pressed.
    '                    If iCtrlDown Then
    '                        ' Set focus the the start control on
    '                        ' the tab.
    '                        If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
    '                            m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
    '                        End If
    '                    End If

    '                Case Keys.End
    '                    ' End key has been pressed.

    '                    ' Check if the control key has also
    '                    ' been pressed.
    '                    If iCtrlDown Then
    '                        ' Set focus the the start control on
    '                        ' the tab.
    '                        If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
    '                            m_ctlTabFirstLast(ACControlEnd, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
    '                        End If
    '                    End If
    '            End Select
    '        End With
    '        'Developer Guide No 293
    '        If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
    '            tabMainTab.SelectedIndex = 0
    '        End If
    '        If eventArgs.Alt And eventArgs.KeyCode = Keys.D2 Then
    '            tabMainTab.SelectedIndex = 1
    '            _tabMainTab_TabPage1.Select()

    '        End If
    '    Catch



    '        ' Error Section.

    '        Exit Sub
    '    End Try


    'End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Static bAlreadyRun As Boolean
        Dim sLockedBy As String = ""
        Dim bProductAutoRenew As Boolean
        Dim lInsurancefolderCnt As Long
        Dim vArray(,) As Object
        ' Click event of the OK button.
        Try

            If bAlreadyRun Then
                If MessageBox.Show("Are you sure you wish to run Renewal Selection again?", "Renewal Selection", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) = System.Windows.Forms.DialogResult.No Then
                    Exit Sub
                End If
            End If

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            m_lReturn = CheckJobBatchRenewalInProcess()

            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                Exit Sub
            End If

            'RWH(15/05/2001) Do our own mandatory check.
            If CheckMandatoryControls() = gPMConstants.PMEReturnCode.PMTrue Then
                If ValidateForm() = gPMConstants.PMEReturnCode.PMTrue Then
                    If SSTabHelper.GetTabVisible(tabMainTab, 0) Then 'Date/Product.
                        'RWH(22/02/2001) Lock Product for Renewal.

                        m_lReturn = m_oBusiness.LockProductForRenewal(VB6.GetItemData(cboProductCode, cboProductCode.SelectedIndex), sLockedBy)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            MessageBox.Show(sLockedBy & Strings.Chr(13) & Strings.Chr(10) & "Please try again later.", "Renewal " &
                                            "Selection", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                            'RKS PN14778
                            m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                            Exit Sub
                        End If
                        ' Process the next set of actions depending
                        ' upon the interface task etc.
                        m_lReturn = m_oGeneral.ProcessCommand()
                    Else
                        m_lReturn = m_oBusiness.GetInsFolderFromInsRef(v_sInsuranceRef:=Trim(txtPolicyRef.Text),
                                                         r_vResultArray:=vArray)

                        If IsArray(vArray) Then
                            lInsurancefolderCnt = ToSafeLong(vArray(0, 0))
                        Else
                            lInsurancefolderCnt = 0
                        End If
                        If lInsurancefolderCnt = 0 Then
                            MsgBox("No data found for current criteria", vbOKOnly, "Renewal " &
                                    "Selection")
                            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                            m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                            Exit Sub
                        Else
                            m_lInsFolderCnt = lInsurancefolderCnt
                        End If
                        If LockPolicy() <> gPMConstants.PMEReturnCode.PMTrue Then
                            Exit Sub
                        End If
                        m_bCalledFromLocalForm = True
                        m_lReturn = SilentRenewal(m_lInsFolderCnt, bProductAutoRenew)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            StatusBar1.Items.Item("Message").Text = " Ready"
                            Me.StatusBar1.Refresh()
                            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                            m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                            UnLockPolicy()
                            Exit Sub
                        End If
                        UnLockPolicy()
                    End If

                    bAlreadyRun = True

                    'turn off reprint - data in the renewal_report table may be empty
                    Me.cmdRePrint.Enabled = False
                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                        If Not (Status = gPMConstants.PMEReturnCode.PMCancel) Then
                            StatusBar1.Items.Item("Message").Text = " Generate reports..."
                            Me.StatusBar1.Refresh()
                            '***************PRINT OUT REPORTS*****************
                            PrintRenewalReport()
                        End If
                    ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                        MessageBox.Show("No data found for current criteria", "Renewal Reports", MessageBoxButtons.OK)
                    Else
                        MessageBox.Show("Failed Renewal Selection", ACApp, MessageBoxButtons.OK)
                    End If

                    'Do it here instead
                    cmdRePrint.Enabled = True

                    If SSTabHelper.GetTabVisible(tabMainTab, 0) Then 'Date/Product.
                        'RWH(22/02/2001) Unlock Product.
                        m_lReturn = UnlockProductForRenewal()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            MessageBox.Show("Failed to unlock Product", "Renewal Selection", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        End If
                    Else

                        'RWH(16/05/01) Give option to launch Renewal Amendment if we have done
                        'select on single policy and it is a manual renewal.
                        If Not bProductAutoRenew Then
                            If MessageBox.Show("Is policy to be amended now ?", "Renewals", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = System.Windows.Forms.DialogResult.Yes Then
                                m_lReturn = LaunchRenewalAmendment()
                            End If
                        End If
                    End If

                End If

                StatusBar1.Items.Item("Message").Text = " Ready"
                Me.StatusBar1.Refresh()

            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            'RKS PN14778
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

        Catch excep As System.Exception




            'RKS PN14778
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions depending
            ' upon the interface task etc.
            '    If m_oGeneral.ProcessCommand() = PMTrue Then
            ' Everything OK, so we can hide the interface.
            Me.Hide()
            '    End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    '
    ' Name: UnlockProductForRenewal
    '
    ' Description:
    '
    ' History: 22/02/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function UnlockProductForRenewal() As Integer
        Dim result As Integer = 0

        Dim oPMLock As bPMLock.User

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get bPMLock
            Dim temp_oPMLock As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPMLock = temp_oPMLock

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get PMLock", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockProductForRenewal", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return result
            End If


            m_lReturn = oPMLock.UnLockKey(sKeyName:="renewal", vKeyValue:=VB6.GetItemData(cboProductCode, cboProductCode.SelectedIndex), iUserID:=g_oObjectManager.UserID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to lock the risk", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockDataModel", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result

            End If

            oPMLock = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="UnlockProductForRenewal Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockProductForRenewal", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: SelectPolicy
    '
    ' Description:
    '
    ' History: 10/05/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function SelectPolicy() As Integer
        Dim result As Integer = 0

        Dim oFindInsurance As iPMBFindInsurance.Interface_Renamed
        Dim iStatus As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create Policy interface object
            Dim temp_oFindInsurance As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oFindInsurance, sClassName:="iPMBFindInsurance.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFindInsurance = temp_oFindInsurance

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            oFindInsurance.InsFileType = gSIRLibrary.SIRInsFileTypePolicy

            oFindInsurance.FindMode = 1
            ' Alix - Include policies attached to closed branch

            oFindInsurance.IncludeClosedBranches = True


            m_lReturn = oFindInsurance.Start
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                iStatus = oFindInsurance.Status

                'Retrieve BusinessTypeId to pass into coinsurance.
                If iStatus = gPMConstants.PMEReturnCode.PMOK Then

                    txtPolicyRef.Text = oFindInsurance.InsReference.Trim()

                    m_lInsFolderCnt = oFindInsurance.InsuranceFolderCnt
                End If
            Else

                oFindInsurance.Dispose()
                oFindInsurance = Nothing
                ' Failed to get an instance of the object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            oFindInsurance.Dispose()
            oFindInsurance = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectPolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectPolicy", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: CheckMandatoryControls
    '
    ' Description:
    '
    ' History: 15/05/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function CheckMandatoryControls() As Integer
        Dim result As Integer = 0
        Dim sMsg As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sMsg = ""

            Select Case (SSTabHelper.GetSelectedIndex(tabMainTab))
                Case 0 'Date/Product.
                    If cboProductCode.SelectedIndex = -1 Then
                        sMsg = sMsg & "Product Code" & Strings.Chr(13) & Strings.Chr(10)
                    End If

                Case 1 'Policy
                    If txtPolicyRef.Text.Trim() = "" Then
                        sMsg = sMsg & "Policy Ref" & Strings.Chr(13) & Strings.Chr(10)
                    End If

            End Select

            If sMsg <> "" Then
                MessageBox.Show("The following fields must be entered :" & Strings.Chr(13) & Strings.Chr(10) & sMsg, "Mandatory Fields", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckMandatoryControls Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMandatoryControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: LaunchRenewalAmendment
    '
    ' Description:
    '
    ' History: 16/05/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function LaunchRenewalAmendment() As Integer
        Dim result As Integer = 0

        Dim oRenewalAmendment As iPMURenewal.NavigatorV3

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oRenewalAmendment As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oRenewalAmendment, "iPMURenewal.NavigatorV3", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oRenewalAmendment = temp_oRenewalAmendment

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display message.
                MessageBox.Show("Failed to launch Renewal Amendment", "Renewal Selection", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'Developer Guide No. 9
            oRenewalAmendment.NavigatorV3_CallingAppName = ACApp

            Dim vkey(1, 0) As Object
            vkey(0, 0) = "Run_Mode"

            vkey(1, 0) = 3
            m_lReturn = oRenewalAmendment.NavigatorV3_SetKeys(vkey)

            m_lReturn = oRenewalAmendment.NavigatorV3_Start()


            oRenewalAmendment.Dispose()

            oRenewalAmendment = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LaunchRenewalAmendment Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LaunchRenewalAmendment", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CreateBusinessObject
    '
    ' Description: create required business objects to run renewal
    '
    ' History: 24/07/2001 TN - Created.
    '
    ' ***************************************************************** '
    Private Function CreateBusinessObject() As Integer

        Dim result As Integer = 0
        Dim vIsRI2007 As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_m_oInsuranceFile As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oInsuranceFile, "bSIRInsuranceFile.Services", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oInsuranceFile = temp_m_oInsuranceFile

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bSIRInsuranceFile.Services", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBusinessObject", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            'RKS PN13438
            Dim temp_m_oInsuranceFileBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oInsuranceFileBusiness, "bSIRInsuranceFile.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oInsuranceFileBusiness = temp_m_oInsuranceFileBusiness


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bSirInsuranceFile.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBusinessObject", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            'RKS PN13438

            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableRI2007, v_vBranch:=g_iSourceID, r_vUnderwriting:=vIsRI2007)

            If ToSafeInteger(vIsRI2007) = 1 Then
                Dim temp_m_oReinsurance As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oReinsurance, "bSIRReinsuranceRI2007.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                m_oReinsurance = temp_m_oReinsurance
            Else
                Dim temp_m_oReinsurance2 As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oReinsurance2, "bSIRReinsurance.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                m_oReinsurance = temp_m_oReinsurance2

            End If


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bSirReinsurance.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBusinessObject", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If


            m_lReturn = m_oReinsurance.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd, vTransactionType:="REN")

            Dim temp_m_oTax As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oTax, "bSIRRITax.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oTax = temp_m_oTax

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bSIRRITax.business", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBusinessObject", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            Dim temp_m_oRiskData As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oRiskData, "bSIRRiskData.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oRiskData = temp_m_oRiskData

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bSirRiskData.business", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBusinessObject", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            Dim temp_m_oPerilAllocation As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oPerilAllocation, "bSirPerilAllocation.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oPerilAllocation = temp_m_oPerilAllocation

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bSirPerilAllocation.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBusinessObject", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            Dim temp_m_oAgentCommission As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oAgentCommission, "bSirAgentCommission.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oAgentCommission = temp_m_oAgentCommission

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bSirAgentCommission.business", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBusinessObject", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            Dim temp_m_oChangePolicyStatus As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oChangePolicyStatus, "bSIRChangePolicyStatus.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oChangePolicyStatus = temp_m_oChangePolicyStatus

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bSirChangePolicyStatus.business", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBusinessObject", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If
            'Start(Saurabh Agrawal) Tech Spec VAL P14 Policy Numbering(5.3.2)
            Dim temp_m_oPolicyNumMaint As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oPolicyNumMaint, "bSIRPolicyNumMaint.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oPolicyNumMaint = temp_m_oPolicyNumMaint

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bSirPolicyNumMaint.business", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBusinessObject", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If
            'End(Saurabh Agrawal) Tech Spec VAL P14 Policy Numbering(5.3.2)
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateBusinessObject Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBusinessObject", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CloseBusinessObject
    '
    ' Description: close down business objects required to run renewal
    '
    ' History: 24/07/2001 TN - Created.
    '
    ' ***************************************************************** '
    Private Function CloseBusinessObject() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not (m_oInsuranceFile Is Nothing) Then

                m_oInsuranceFile.Dispose()
                m_oInsuranceFile = Nothing
            End If

            'RKS PN13438
            If Not (m_oInsuranceFileBusiness Is Nothing) Then

                m_oInsuranceFileBusiness.Dispose()
                m_oInsuranceFileBusiness = Nothing
            End If


            If Not (m_oReinsurance Is Nothing) Then

                m_oReinsurance.Dispose()
                m_oReinsurance = Nothing
            End If

            If Not (m_oTax Is Nothing) Then

                m_oTax.Dispose()
                m_oTax = Nothing
            End If

            If Not (m_oRiskData Is Nothing) Then

                m_oRiskData.Dispose()
                m_oRiskData = Nothing
            End If

            If Not (m_oPerilAllocation Is Nothing) Then

                m_oPerilAllocation.Dispose()
                m_oPerilAllocation = Nothing
            End If

            If Not (m_oAgentCommission Is Nothing) Then

                m_oAgentCommission.Dispose()
                m_oAgentCommission = Nothing
            End If

            If Not (m_oChangePolicyStatus Is Nothing) Then

                m_oChangePolicyStatus.Dispose()
                m_oChangePolicyStatus = Nothing
            End If
            'Start(Saurabh Agrawal) Tech Spec VAL P14 Policy Numbering (5.3.2)
            If Not (m_oPolicyNumMaint Is Nothing) Then

                m_oPolicyNumMaint.Dispose()
                m_oPolicyNumMaint = Nothing
            End If
            'End(Saurabh Agrawal) Tech Spec VAL P14 Policy Numbering (5.3.2)
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CloseBusinessObject Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CloseBusinessObject", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CreateTMPAnniversaryRenewal
    '
    ' Parameters: n/a
    '
    ' Description: Creates
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function CreateTMPAnniversaryRenewal(ByRef r_vRenewalList(,) As Object, ByVal v_lCount As Integer, Optional ByVal v_lDispStatusBar As Integer = 0) As Integer


        Dim result As Integer = 0
        Const kMethodName As String = "CreateTMPAnniversaryRenewal"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim bIsTrueMonthlyPolicy As Boolean
        Dim lAnniversaryRenewalWeeks As Integer
        Dim sInsuranceRef As String = ""
        Dim dtCoverStartDate As Date
        Dim vAnniversaryCopy As Object
        Dim lAnniversaryCopyCount As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' determine whether the associated policies product is a "True Monthly Policy"
            bIsTrueMonthlyPolicy = gPMFunctions.ToSafeLong(r_vRenewalList(PMFieldPosIsTrueMonthlyPolicy, v_lCount)) = 1

            ' if the associated product is "True Monthly Policy"
            If bIsTrueMonthlyPolicy Then

                ' get the anniversary renewal weeks
                ' this is the number of weeks prior to the anniversary date that the
                ' that the anniversary renewal version of the policy should be created

                lAnniversaryRenewalWeeks = CInt(r_vRenewalList(PMFieldPosAnniversaryRenewalWeeks, v_lCount))

                ' if the anniversary renewal version of the policy should have been created
                ' by now

                If gPMFunctions.ToSafeDate(r_vRenewalList(PMFieldPosRenewalDate, v_lCount)) >= gPMFunctions.ToSafeDate(DateAndTime.DateAdd("ww", -lAnniversaryRenewalWeeks, ToSafeDate(r_vRenewalList(PMFieldPosAnniversaryDate, v_lCount)))) Then


                    sInsuranceRef = CStr(r_vRenewalList(PMFieldPosInsuranceRef, v_lCount)).Trim()

                    ' cover start date for the anniversary copy is the anniversary date of the
                    ' original policy

                    dtCoverStartDate = ToSafeDate(r_vRenewalList(PMFieldPosAnniversaryDate, v_lCount))

                    'Debug.Print m_oBusiness.TransactionType

                    ' check if it has been created

                    lReturn = m_oBusiness.FindAnniversaryCopy(v_sinsuranceRef:=sInsuranceRef, v_dtCoverStartDAte:=dtCoverStartDate, r_vResults:=vAnniversaryCopy)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "FindAnniversaryCopy Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    ' if there is an array we have an anniversary copy
                    ' if not go ahead and create the anniversary copy
                    If Not Information.IsArray(vAnniversaryCopy) Then
                        lAnniversaryCopyCount = 0
                    Else

                        lAnniversaryCopyCount = CInt(vAnniversaryCopy(0, 0))
                    End If

                    If lAnniversaryCopyCount = 0 Then

                        ' create an anniversary renewal version of the policy
                        lReturn = CType(CreateRenewalPolicy(r_vRenewalList:=r_vRenewalList, v_lCount:=v_lCount, v_lDispStatusBar:=v_lDispStatusBar, v_bTMPAnniversary:=True), gPMConstants.PMEReturnCode)

                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "CreateRenewalPolicy Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                    End If

                End If

            Else
                ' do nothing
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
    ' Name: GetTrueMonthlyPolicyDates
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function GetTrueMonthlyPolicyDates(ByVal v_bMidnightRenewal As Boolean, ByVal v_bTMPAnniversary As Boolean, ByVal v_lCount As Integer, ByRef r_vRenewalList(,) As Object, ByRef r_dtCoverStartDate As Date, ByRef r_dtExpiryDate As Date, ByRef r_dtRenewalDate As Date, ByRef r_dtAnniversaryDate As Date, ByRef r_lRenewalDayNumber As Integer, ByRef r_lAnniversaryCopy As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetTrueMonthlyPolicyDates"

        Dim dtOriginalRenewalDate, dtOriginalAnniversaryDate As Date
        Dim lOriginalRenewalDayNumber As Integer
        Dim lDay, lYear, lMonth As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            dtOriginalRenewalDate = CDate(r_vRenewalList(PMFieldPosRenewalDate, v_lCount))
            dtOriginalAnniversaryDate = gPMFunctions.ToSafeDate(r_vRenewalList(PMFieldPosAnniversaryDate, v_lCount))

            lOriginalRenewalDayNumber = CInt(r_vRenewalList(PMFieldPosRenewalDayNumber, v_lCount))
            r_lRenewalDayNumber = lOriginalRenewalDayNumber

            If Not v_bTMPAnniversary Then

                ' cover start date
                r_dtCoverStartDate = dtOriginalRenewalDate

                lDay = DateAndTime.Day(r_dtCoverStartDate)
                lMonth = r_dtCoverStartDate.Month
                lYear = r_dtCoverStartDate.Year
                ' New Renewal Date = Renewal Date + 1 Month Aligned to Day Number
                If lOriginalRenewalDayNumber > DateAndTime.Day(DateAndTime.DateSerial(lYear, lMonth + 2, 0)) Then
                    r_dtRenewalDate = DateAndTime.DateSerial(lYear, lMonth + 2, 0)
                Else
                    r_dtRenewalDate = GetClosestDate(lOriginalRenewalDayNumber, dtOriginalRenewalDate.Month + 1, dtOriginalRenewalDate.Year)
                End If
                If v_bMidnightRenewal Then
                    r_dtExpiryDate = r_dtRenewalDate.AddDays(-1)
                Else
                    r_dtExpiryDate = r_dtRenewalDate
                End If

                ' The new renewals anniversary date = polcies anniversary date
                r_dtAnniversaryDate = dtOriginalAnniversaryDate

                ' Anniversary Copy = 0
                r_lAnniversaryCopy = 0

            Else

                ' Cover Start Date = Anniversary Date
                r_dtCoverStartDate = dtOriginalAnniversaryDate
                lDay = DateAndTime.Day(r_dtCoverStartDate)
                lMonth = r_dtCoverStartDate.Month
                lYear = r_dtCoverStartDate.Year
                ' Expiry Date = Anniversary Date + 1 Month Aligned to Renewal Day Number
                If lOriginalRenewalDayNumber > DateAndTime.Day(DateAndTime.DateSerial(lYear, lMonth + 2, 0)) Then
                    r_dtRenewalDate = DateAndTime.DateSerial(lYear, lMonth + 2, 0)
                Else
                    r_dtRenewalDate = GetClosestDate(lOriginalRenewalDayNumber, dtOriginalAnniversaryDate.Month + 1, dtOriginalAnniversaryDate.Year)
                End If
                ' If This Is a Midnight Renewal then the Expiry Date (Cover To Date) = Renewal Date - 1 Day
                If v_bMidnightRenewal Then
                    r_dtExpiryDate = r_dtRenewalDate.AddDays(-1)
                Else
                    ' Expiry Date (Cover To Date) = Renewal Date
                    r_dtExpiryDate = r_dtRenewalDate
                End If

                ' Anniversary Date = Anniversary Date + 1 Year
                r_dtAnniversaryDate = dtOriginalAnniversaryDate.AddYears(1)

                ' Anniversary Copy = 1
                r_lAnniversaryCopy = 1

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
    ' Name: GetClosestDate
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 12-10-2005 : Process ID
    ' ***************************************************************** '
    Public Function GetClosestDate(ByVal v_lDay As Integer, ByVal v_lMonth As Integer, ByVal v_lYear As Integer) As Date

        Dim result As Date = DateTime.FromOADate(0)
        Const kMethodName As String = "GetClosestDate"

        Dim lReturn, lNewMonth As Integer
        Dim dtSerial As Date
        Dim dtMonthStart As Date



        Try



            result = DateTime.FromOADate(gPMConstants.PMEReturnCode.PMTrue)

            If v_lMonth > 12 Then
                v_lMonth = 1
                v_lYear += 1
            End If

            ' serialise the date from the passed data
            dtSerial = DateAndTime.DateSerial(v_lYear, v_lMonth, v_lDay)

            ' get the month from the newly serialised date
            lNewMonth = dtSerial.Month

            ' if the month of the new date doesnt match the
            ' month passed in then
            If lNewMonth <> v_lMonth Then

                dtMonthStart = DateAndTime.DateSerial(v_lYear, lNewMonth, 1)

                dtSerial = dtMonthStart.AddDays(-1)

            End If

            ' return the serialised date
            ' or the last day in the specified month
            result = dtSerial


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lReturn, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function



    ' ***************************************************************** '
    ' Name: CreateRenewalPolicyWrapper
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function CreateRenewalPolicyWrapper(ByRef r_vRenewalList As Object, ByVal v_lCount As Integer, Optional ByVal v_lDispStatusBar As Integer = 0) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CreateRenewalPolicyWrapper"

        Dim lReturn As gPMConstants.PMEReturnCode
        '    Dim bIsTrueMonthlyPolicy        As Boolean
        '    Dim bCreateRenewalPolicy        As Boolean
        '    Dim dtOriginalRenewalDate       As Date
        '    Dim dtOriginalAnniversaryDate   As Date
        '    Dim vRenewalPolicy              As Variant

        Try



            result = gPMConstants.PMEReturnCode.PMTrue
            '
            '    ' default action is to create the renewal policy
            '    bCreateRenewalPolicy = True
            '
            '    ' determine whether the associated policies product is a "True Monthly Policy"
            '    bIsTrueMonthlyPolicy = CBool(ToSafeLong(r_vRenewalList(PMFieldPosIsTrueMonthlyPolicy, v_lCount)) = 1)
            '
            '    ' if the associated product is "True Monthly Policy"
            '    If bIsTrueMonthlyPolicy Then
            '
            '        ' get dates
            '        dtOriginalRenewalDate = r_vRenewalList(PMFieldPosRenewalDate, v_lCount)
            '        dtOriginalAnniversaryDate = r_vRenewalList(PMFieldPosAnniversaryDate, v_lCount)
            '
            '        ' check whether or not it should be the anniversary renewal
            '        ' is its cover start date >= original anniversary date
            '        If dtOriginalRenewalDate >= dtOriginalAnniversaryDate Then
            '
            '            ' flag that a renewal policy should not be created at this point
            '            ' NB: the following procedure "CreateTMPAnniversaryRenewal" code will
            '            ' create the anniversary version of the renewal if it is required
            '            bCreateRenewalPolicy = False
            '        End If
            '
            '    End If
            '
            '    ' if a renewal should be created
            '    If bCreateRenewalPolicy = True Then

            ' set additional policy version increment
            m_lPolicyVersionIncrement = 1

            ' create the renewal


            lReturn = CType(CreateRenewalPolicy(r_vRenewalList:=r_vRenewalList, v_lCount:=v_lCount, v_lDispStatusBar:=v_lDispStatusBar), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "CreateRenewalPolicy Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            '   End If


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
    ' Name: ApplyPolicyDiscount
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 10-01-2006 : Discount / Loading
    ' ***************************************************************** '
    Public Function ApplyPolicyDiscount(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lProductId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ApplyPolicyDiscount"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' apply policy discount

            lReturn = m_oBusiness.ApplyPolicyDiscount(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lProductId:=v_lProductId)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "ApplyPolicyDiscount Failed", gPMConstants.PMELogLevel.PMLogError)
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

    Private Sub dtStartDate_EnabledChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtStartDate.EnabledChanged
        'lblStartDateDesc.Visible = Convert.IsDBNull(dtStartDate.Value) Or IsNothing(dtStartDate.Value)

        'lblStartDate.Enabled = Not (Convert.IsDBNull(dtStartDate.Value) Or IsNothing(dtStartDate.Value))
    End Sub
    Private Sub frmInterface_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            tabMainTab.SelectedIndex = 0
            tabMainTab.Focus()
        End If
        If e.Alt And e.KeyCode = Keys.D2 Then
            tabMainTab.SelectedIndex = 1
            tabMainTab.Focus()
        End If
    End Sub
    Public Function ValidateCertificateYear(ByRef bIsValid As Boolean, ByVal lNewInsuranceFileCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim sValue As String = ""
        Dim r_sMessage As String
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTSubAgentCertificateYears, 1, sValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get product option " & gPMConstants.SIRHiddenOptions.SIROPTHoldCoverExpiryDate, vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            If sValue = "1" Then
                m_lReturn = m_obSIRRenewal.GetAndValidateSubAgentDetailsViaInsFile(bIsValid, lNewInsuranceFileCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
                If bIsValid = False Then
                    m_obSIRRenewal.UpdateRenewalStatus(gPMConstants.PMBRenewalStatusTypeManualReview, r_sMessage)
                    System.Windows.Forms.MessageBox.Show("You Cannot Make This Transaction Live- Please check the Certificate Year Configuration of Sub Agent", ACApp, MessageBoxButtons.OK)
                    Return result
                End If

            End If
            Return result
        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="ValidateCertificateYear", r_lFunctionReturn:=result, excep:=ex)
            Return gPMConstants.PMEReturnCode.PMFalse
        End Try

    End Function

    ''' <summary>
    ''' Locks the Current Policy so that other users are unable to make changes while it is under process by current user.
    ''' </summary>
    ''' <returns>Returns gPMConstants.PMEReturnCode which is of base type long</returns>
    ''' <remarks></remarks>
    Public Function LockPolicy() As Long

        Dim sLockedBy As String = String.Empty
        Dim oPMLock As Object = Nothing
        Const kMethodName As String = "LockPolicy"

        Try
            '   Find the Business Class
            m_lReturn = g_oObjectManager.GetInstance(
                    oObject:=oPMLock,
                    sClassName:="bPMLock.User",
                    vInstanceManager:=PMGetViaClientManager)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = oPMLock.LOCKKEY("insurance_folder_cnt",
                    vKeyValue:=m_lInsFolderCnt,
                    iUserID:=g_oObjectManager.UserID,
                    v_bOtherUserOnly:=False,
                    sCurrentlyLockedBy:=sLockedBy$)

            Select Case m_lReturn

                Case gPMConstants.PMEReturnCode.PMTrue
                    Return gPMConstants.PMEReturnCode.PMTrue

                Case gPMConstants.PMEReturnCode.PMFalse
                    'Locked or error
                    If (sLockedBy$ = "ERROR") Then
                        gPMFunctions.RaiseError(kMethodName, "Error trying to lock record", gPMConstants.PMELogLevel.PMLogError)
                    Else

                        MsgBox("Policy currently locked by " & sLockedBy$ &
                                vbCrLf & "Please try later", , "Policy Lock")
                        ' Return false but dont raise exception rather we will like to show message and use PMFalse status to handle code from calling Subroutine
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Case Else
                    gPMFunctions.RaiseError(kMethodName, "Failed to lock the policy", gPMConstants.PMELogLevel.PMLogError)
            End Select

        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, excep:=ex)
            Return gPMConstants.PMEReturnCode.PMFalse
        Finally
            'Terminate the business object
            oPMLock.Dispose()
            oPMLock = Nothing
        End Try

    End Function

    'Author: Tariq Rashid
    ''' <summary>
    ''' Unlocks Current Policy so that it is available to other users.
    ''' </summary>
    ''' <returns>Returns gPMConstants.PMEReturnCode which is of base type long</returns>
    ''' <remarks></remarks>
    Private Function UnLockPolicy() As Long

        Dim oPMLock As Object = Nothing
        Const kMethodName As String = "UnLockPolicy"

        Try
            '   Find the Business Class
            m_lReturn = g_oObjectManager.GetInstance(
                    oObject:=oPMLock,
                    sClassName:="bPMLock.User",
                    vInstanceManager:=PMGetViaClientManager)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'UnLock the Key
            m_lReturn = oPMLock.UNLOCKKEY("insurance_folder_cnt",
                                          vKeyValue:=m_lInsFolderCnt,
                                          iUserID:=g_oObjectManager.UserID)

            'Check for any error
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMTrue
            Else
                gPMFunctions.RaiseError(kMethodName, "Failed to unlock the policy", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, excep:=ex)
            Return gPMConstants.PMEReturnCode.PMFalse
        Finally
            'Terminate the business object
            oPMLock.Dispose()
            oPMLock = Nothing
        End Try


    End Function

    Private Function CheckJobBatchRenewalInProcess() As Long
        Const kMethodName As String = "CheckJobBatchRenewalInProcess"

        Dim lReturn As Long
        Dim bIsJobBatchRenewalInProcess As Boolean
        Dim oRenewal As Object
        Try

            CheckJobBatchRenewalInProcess = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oRenewal As Object
            m_lReturn = g_oObjectManager.GetInstance(
                oObject:=temp_oRenewal,
                sClassName:="bSIRRenewal.Business",
                vInstanceManager:=PMGetViaClientManager)
            oRenewal = temp_oRenewal

            lReturn = oRenewal.CheckJobBatchRenewalInProcess(v_sKey:="SEL",
                                                        r_bIsJobBatchRenewalInProcess:=bIsJobBatchRenewalInProcess)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If bIsJobBatchRenewalInProcess Then
                MessageBox.Show("There is a Batch Renewal Selection Run in progress. Please try later.", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                CheckJobBatchRenewalInProcess = gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(
                 v_sClass:=ACClass,
                 v_sMethod:=kMethodName,
                 r_lFunctionReturn:=CheckJobBatchRenewalInProcess)
            CheckJobBatchRenewalInProcess = gPMConstants.PMEReturnCode.PMFalse
            ' If you want to rollback a transaction or something, do it here
        Finally

            If Not oRenewal Is Nothing Then
                m_lReturn = oRenewal.Dispose()
                oRenewal = Nothing
            End If

        End Try

    End Function

End Class
