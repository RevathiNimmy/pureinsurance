Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Drawing
Imports System.Globalization
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Imports uctCLMReserveControl
Imports uctCLMPaymentControl

Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 23 Aug 2000
    '
    ' Description: Main interface.
    '
    ' Edit History:
    ' RAM20021018 : NRMA Changes (Sirius Process No 126)
    '               Added 2 tool bar buttons to view Party Summary Details &
    '               Policy Summary Details
    ' 01/06/2005 : MKR : PN 21215 : Made txtComments uneditable while it is
    '              in view or disabled mode. Also made it scrollable.
    ' ***************************************************************** '
    'developer guide no.7
    Public Const vbFormCode As Integer = 0

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Declare an instance of the Lock object.
    Private m_oPMLock As Object

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iCLMPeril.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer
    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control
    ' variable declared for list item
    Private m_lstitem As ListViewItem
    Private m_lstitem2 As ListViewItem
    ' variables for Message boxes
    Private sMessage As String = ""
    Private sTitle As String = ""
    Private m_lDisableScreen As gPMConstants.PMEReturnCode 'set to pmview to disable all control
    Private m_sNegativeReserve As String = "" 'store value from system option (option_number = 1016)
    Private m_lInsuranceFolderCnt As Integer
    Private m_sInsuranceRef As String = ""
    Private m_bClientPolicyDetailsLoaded As Boolean
    Private m_oPartySummary As iSIRPartySummary.Interface_Renamed
    Private m_oPolicySummary As iSIRPolicySummary.Interface_Renamed

    Private m_oRisk As iPMURiskWrapper.Interface_Renamed
    Private m_cThisPayment As Decimal
    Private m_bStatus As Boolean
    'S4B Claim Enhancements R&D 2005
    Private m_sSiriusProduct As String = ""

    'developer guide no. 50
    Dim frmDetailsUW As frmDetailsUW
    Dim frmDetailsBR As frmDetailsBR
    'DC080606
    Private m_bShowCoinsurers As Boolean
    Private m_vCoinsurers As Object
    Private m_bIsRI2007Enabled As Boolean
    Private m_bOpenClaimNoTrans As Boolean
    'Start(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.6)
    Private m_sScreenCaption As String = ""
    'End(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.6)
    Private m_bReserveLimitExceeded As Boolean
    Private m_dExceededReserve As Decimal

    Public Property DisableScreen() As Integer
        Get
            Return m_lDisableScreen
        End Get
        Set(ByVal Value As Integer)
            m_lDisableScreen = Value
        End Set
    End Property
    Public ReadOnly Property NegativeReserve() As String
        Get
            Return m_sNegativeReserve
        End Get
    End Property

    ' Stores the details from the business object.
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

    'Private Sub Status(ByVal Value As Integer)
    ' Set the interface exit status.
    'm_lStatus = Value
    'End Sub
    Public ReadOnly Property Status() As Integer
        Get
            ' Return the interface exit status.
            Return m_lStatus
        End Get
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
    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)
            m_lProcessMode = Value
        End Set
    End Property
    Public Property TransactionType() As String
        Get
            Return m_sTransactionType
        End Get
        Set(ByVal Value As String)
            m_sTransactionType = Value
        End Set
    End Property
    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)
            m_dtEffectiveDate = Value
        End Set
    End Property

    Public ReadOnly Property ThisPayment() As Decimal
        Get
            Return m_cThisPayment
        End Get
    End Property

    'Start(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.6)

    Public WriteOnly Property ScreenCaption() As String
        Set(ByVal Value As String)
            m_sScreenCaption = Value
        End Set
    End Property


    Public Property IsOpenClaimNoTrans() As Boolean
        Get
            Return m_bOpenClaimNoTrans
        End Get
        Set(ByVal Value As Boolean)
            m_bOpenClaimNoTrans = Value
        End Set
    End Property
    'End(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.6)

    Public Property ReserveLimitExceeded() As Boolean
        Get
            Return m_bReserveLimitExceeded
        End Get
        Set(ByVal Value As Boolean)
            m_bReserveLimitExceeded = Value
        End Set
    End Property

    Public Property ExceededReserve() As Decimal
        Get
            Return m_dExceededReserve
        End Get
        Set(ByVal Value As Decimal)
            m_dExceededReserve = Value
        End Set
    End Property

    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer
        Dim result As Integer = 0
        Dim bCheckPaymentAuthorisation As Boolean
        Dim sPolicyType As String = ""
        Dim oRiskDetails As bCLMRiskDetails.Business
        Dim vRiskDetails As Object

        Const ACGeminiIIMotor As String = "GEMINI IIM"
        Const ACGeminiIIHouseHold As String = "GEMINI IIH"
        Const ACCommercialVehicle As String = "CV"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'payment can only be made from payment roadmap
            'DC280302 added check for Underwriting

            'Get the risk type
            ' Get an instance of the business object via the public object manager.
            Dim temp_oRiskDetails As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oRiskDetails, "bCLMRiskDetails.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oRiskDetails = temp_oRiskDetails

            ' initialise this new object

            m_lReturn = oRiskDetails.Initialise(sUsername:=g_oObjectManager.UserName, sPassword:=g_oObjectManager.Password, iUserID:=g_oObjectManager.UserID, iSourceID:=g_iSourceID, iLanguageID:=g_iLanguageID, iCurrencyID:=g_oObjectManager.CurrencyID, iLogLevel:=g_oObjectManager.LogLevel, sCallingAppName:=ACApp)

            ' get the text description of the risk type

            m_lReturn = oRiskDetails.GetRiskDetails(v_lRisk:=m_lRiskID, v_lPolicyId:=m_lInsurance_file_cnt, r_vDataArray:=vRiskDetails)

            oRiskDetails.Dispose()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the Risk Details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")
            End If

            If Information.IsArray(vRiskDetails) Then

                g_bIsPostTaxes = gPMFunctions.ToSafeBoolean(CStr(vRiskDetails(2, 0)), True)
            Else
                g_bIsPostTaxes = True
            End If

            'TN20010605 start

            cmdCheckList.Visible = True

            If DisableScreen = gPMConstants.PMEReturnCode.PMTrue Then
                cmdCheckList.Enabled = False
            End If

            'TF011003 - PN7035 - To identify GII policies and suppress RiskDetails button

            m_lReturn = g_oBusiness.GetPolicyType(v_lPolicyId:=m_lInsurance_file_cnt, r_sType:=sPolicyType)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process g_oBusiness.GetPolicyType.", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If (sPolicyType = ACGeminiIIMotor) Or (sPolicyType = ACGeminiIIHouseHold) Or (sPolicyType = ACCommercialVehicle) Then
                Toolbar1.Items.Item("Risk").Visible = False
            End If


            'DC080606
            m_bShowCoinsurers = False


            ' check to see if the authorisation scripts for claim payments is switched on...
            m_lReturn = UseAuthorisedScriptsForClaimPayments(bCheckPaymentAuthorisation)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' can't get hidden option so assume it is switched off...
                bCheckPaymentAuthorisation = False
                ' log warning...
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:="Failed during call to UseAuthorisedScriptsForClaimPayments - assuming authorisation scripts are disabled", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults")
            End If

            ' only if authorisation scripts for claim payments are on do we need to check for referred payments...
            If bCheckPaymentAuthorisation Then
                'AK 130503 - stop the user from adding any payments, if any existing payment is outstanding - start

                m_lReturn = g_oBusiness.CheckReferredPayment(m_lClaimID, m_bStatus)

                'Edit button needs to be disabled if there are outstanding payment authorisations...
                '        If m_bStatus = True Then
                '            cmdEdit(ACPaymentEdit).Enabled = False
                '        End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayCaptions
    '
    ' Description: Display all language specific captions.
    '
    ' ***************************************************************** '
    Public Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ' Display all language specific captions.
            'Developer Guide No.: 243
            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() & _
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If

            'Developer Guide No.: 243
            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No.: 243
            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No.: 243
            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            '    cmdEdit(0).Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lId:=ACEditButton, _
            ''        iDataType:=PMResString)
            '
            '    cmdEdit(1).Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lId:=ACEditButton, _
            ''        iDataType:=PMResString)
            '

            'Developer Guide No.: 243
            SSTabHelper.SetTabCaption(SSTab1, ACGeneral, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACGeneralTab, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No.: 243
            SSTabHelper.SetTabCaption(SSTab1, ACDriver, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDriverTab, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No.: 243
            SSTabHelper.SetTabCaption(SSTab1, ACThirdParty, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACThirdPartyTab, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No.: 243
            SSTabHelper.SetTabCaption(SSTab1, ACRepairer, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRepairerTab, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No.: 243
            SSTabHelper.SetTabCaption(SSTab1, ACWitness, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACWitnessTab, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No.: 243
            SSTabHelper.SetTabCaption(SSTab1, ACReserve, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACReserveTab, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No.: 243
            SSTabHelper.SetTabCaption(SSTab1, ACPayment, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPaymentTab, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No.: 243
            SSTabHelper.SetTabCaption(SSTab1, ACComments, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCommentsTab, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            Return result

            'Developer Guide No.: 243
            fraDriver.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDriverFrame, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No.: 243
            fraThirdParty.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACThirdPartyFrame, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No.: 243
            fraRepairer.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRepairerFrame, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No.: 243
            fraWitness.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACWitnessFrame, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            '    fraReserveDetails.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lId:=ACReserveFrame, _
            ''        iDataType:=PMResString)
            '
            '    fraPaymentDetails.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lId:=ACPaymentFrame, _
            ''        iDataType:=PMResString)


            'Developer Guide No.: 243
            fraComments.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCommentsFrame, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            ' captions for the Details screen

            'Developer Guide No.: 243
            SSTabHelper.SetTabCaption(frmDetailsUW.SSTab1, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACReserveTab, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' captions for the Details screen

            'Developer Guide No.: 243
            SSTabHelper.SetTabCaption(frmDetailsBR.SSTab1, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACReserveTab, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No.: 243
            frmDetailsUW.fraReserveDetails.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACReserveFrame, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No.: 243
            frmDetailsBR.fraReserveDetails.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACReserveFrame, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No.: 243
            frmDetailsUW.cmdOk.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No.: 243
            frmDetailsBR.cmdOk.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No.: 243
            frmDetailsUW.cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No.: 243
            frmDetailsBR.cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No.: 243
            frmDetailsUW.lblRiskType.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRiskType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No.: 243
            frmDetailsBR.lblRiskType.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRiskType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No.: 243
            frmDetailsUW.lblInitialReserve.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInitialReserve, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No.: 243
            frmDetailsBR.lblInitialReserve.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInitialReserve, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No.: 243
            frmDetailsUW.lblRevisedReserve.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRevisedReserve, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No.: 243
            frmDetailsBR.lblRevisedReserve.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRevisedReserve, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No.: 243
            frmDetailsUW.lblThisPayment.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACThispayment, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No.: 243
            frmDetailsBR.lblThisPayment.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACThispayment, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    Private Sub cmdCheckList_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCheckList.Click
        Dim oObject As iCLMInfoChklst.Interface_Renamed
        Dim vKeyArray(1, 4) As Object
        Dim sClaimRef As String = ""

        Try

            m_lReturn = g_oBusiness.GetClaimNumber(v_lClaimId:=m_lClaimID, r_sClaimRef:=sClaimRef)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed To Get Claim Reference", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)

                Exit Sub
            End If

            Dim temp_oObject As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oObject, sClassName:="iCLMInfoChklst.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oObject = temp_oObject

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create object 'iCLMInfoChklst.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCheckList_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Exit Sub
            End If


            vKeyArray(0, 0) = "claim_ref"

            vKeyArray(1, 0) = sClaimRef

            vKeyArray(0, 1) = "risk_type_id"

            vKeyArray(1, 1) = m_lRiskID

            vKeyArray(0, 2) = "claim_cnt"

            vKeyArray(1, 2) = m_lClaimID

            vKeyArray(0, 3) = "claim_mode"

            vKeyArray(1, 3) = gPMConstants.PMEComponentAction.PMEdit

            vKeyArray(0, 4) = "DeleteWorkTableFlag"

            vKeyArray(1, 4) = gPMConstants.PMEReturnCode.PMFalse


            m_lReturn = oObject.SetProcessModes(vTask:=m_iTask, vTransactionType:=m_sTransactionType)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                m_lReturn = oObject.SetKeys(vKeyArray:=vKeyArray)
            End If

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                m_lReturn = oObject.Start()
            End If


            oObject.Dispose()
            oObject = Nothing


        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdCheckList_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCheckList_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try

    End Sub


    Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender
            If m_sTransactionType = "C_CP" Then
                SSTabHelper.SetSelectedIndex(SSTab1, ACPayment) '(RC)
            ElseIf m_sTransactionType = "C_CR" Or m_sTransactionType = "C_CO" Then
                SSTabHelper.SetSelectedIndex(SSTab1, ACReserve)
            End If
        End If
    End Sub

    ' ***************************************************************** '
    ' Name: Form_Initialise
    '
    ' Description: calls the initialise event of the form
    '
    ' ***************************************************************** '

    Private Sub Form_Initialize_Renamed()

        ' Forms initialise event.
        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' pass the parameters to the Business Layer

            g_oBusiness.PerilID = m_lPerilID

            g_oBusiness.PerilTypeID = m_lPerilTypeID

            g_oBusiness.ClaimID = m_lClaimID

            g_oBusiness.Partycnt = m_lPartycnt

            ' Create an instance of the general interface object.
            m_oGeneral = New iCLMPeril.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=g_oBusiness)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Create an instance of the form control object.
            m_oFormFields = New iPMFormControl.FormFields()

            ' Set language
            m_oFormFields.LanguageID = g_iLanguageID

            'Initialise the control
            'uctCLMPayment.Initialise     'PN 35485
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



    ' ***************************************************************** '
    ' Name: Form_Load
    '
    ' Description: Calls the form load event which calls the Business Layer functions
    '              loads the controls at the run-time and the values for these controls
    '
    ' Edit History  :
    ' RAM20021022   : NRMA Changes - Sirius Process No: 126 - Start
    ' ***************************************************************** '

    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        Dim bAllowNegativeReserve As Boolean

        ' Forms load event.

        Try

            'S4B Claim Enhancements R&D 2005

            m_sSiriusProduct = g_oBusiness.UnderwritingOrAgency

            'TN20010510 Start
            m_lReturn = GetProductDetailsForClaim(r_bAllowNegativeReserve:=bAllowNegativeReserve)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get claim system option (Allow negative reserve)", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")

                Exit Sub
            End If

            If bAllowNegativeReserve Then
                m_sNegativeReserve = "1"
            Else
                m_sNegativeReserve = "0"
            End If

            'TN20010510 End

            'sj 02/10/2002 - start
            With Toolbar1

                .ImageList = ImageList1

                '.Items.Item("Event").ImageIndex = 11
                .Items.Item("TSB_Event").ImageIndex = 10

                '.Items.Item("Risk Details").Visible = False
                .Items.Item("Risk_Details").Visible = False

                '.Items.Item("Information Checklist").Visible = False
                .Items.Item("Information_Checklist").Visible = False

                'RAM20021022 : NRMA Changes - Sirius Process No: 126 - Start
                .Items.Item("Party").ImageIndex = 16 ' Party Summary  ICON
                .Items.Item("Policy").ImageIndex = 17 ' Policy Summary ICON
                .Items.Item("Risk").ImageIndex = 14 ' Risk Details ICON
                'RAM20021022 : NRMA Changes - Sirius Process No: 126 - End
                'DJM 27/11/2003 : Disable the buutons if screen is meant to be disabled.

                '.Items.Item("Event").Enabled = Not (m_lDisableScreen = gPMConstants.PMEReturnCode.PMTrue)
                .Items.Item("TSB_Event").Enabled = Not (m_lDisableScreen = gPMConstants.PMEReturnCode.PMTrue)

                .Items.Item("Party").Enabled = Not (m_lDisableScreen = gPMConstants.PMEReturnCode.PMTrue)
                .Items.Item("Policy").Enabled = Not (m_lDisableScreen = gPMConstants.PMEReturnCode.PMTrue)
                .Items.Item("Risk").Enabled = Not (m_lDisableScreen = gPMConstants.PMEReturnCode.PMTrue)

            End With
            'sj 02/10/2002 - end
            'developer guide no.(Shifted from loadinterface of interface_renamed)
            'Diable the Toolbar Button if there are No event for the Claim
            'PN 29146
            If m_sTransactionType = "C_CO" Then
                Toolbar1.Items.Item(0).Enabled = False
            End If

            SSTabHelper.SetTabVisible(SSTab1, ACGeneral, False)
            SSTabHelper.SetTabVisible(SSTab1, ACDriver, False)
            SSTabHelper.SetTabVisible(SSTab1, ACThirdParty, False)
            SSTabHelper.SetTabVisible(SSTab1, ACRepairer, False)
            SSTabHelper.SetTabVisible(SSTab1, ACWitness, False)
            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the process modes for the busines object.

            m_lReturn = g_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")

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

            m_lReturn = GetComments()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            For lCount As Integer = 0 To SSTabHelper.GetTabCount(SSTab1) - 1
                If SSTabHelper.GetTabVisible(SSTab1, lCount) Then
                    SSTabHelper.SetSelectedIndex(SSTab1, lCount)
                    Exit For
                End If
            Next lCount

            ' Set any other default values to the interface.


            ' ****************************
            ' set up the claim reserve control
            ' ****************************
            If m_lPerilID = 0 And m_lClaimID = 0 And m_lRiskID = 0 Then
                SSTabHelper.SetTabVisible(SSTab1, 9, False)
            Else
                'developer guide no.9
                uctCLMReserve.Initialise()
                'DC080606
                uctCLMReserve.ShowCoInsurers = m_bShowCoinsurers
                uctCLMReserve.SetProcessModes(vTask:=m_iTask, vTransactionType:=m_sTransactionType)
                uctCLMReserve.IsOpenClaimNoTrans = m_bOpenClaimNoTrans
                uctCLMReserve.LoadControl()
                uctCLMReserve.GetDetails(lPerilID:=m_lPerilID, lClaimID:=m_lClaimID, lRiskID:=m_lRiskID, lInsurance_File_Cnt:=m_lInsurance_file_cnt, lPerilTypeID:=m_lPerilTypeID)
            End If
            ' ****************************
            ' ****************************


            ' ****************************
            ' set up the claim payment control....
            ' ****************************
            If m_lClaimID = 0 And m_lPerilID = 0 Then
                'This is for view mode so hide it.
                SSTabHelper.SetTabVisible(SSTab1, 10, False)
                'Developer guide no. 9
                m_lReturn = uctCLMPayment.Initialise()
                If m_lReturn <> 1 Then
                    Throw New System.Exception("1, , initialise failed")
                End If
                uctCLMPayment.SetUserControlDefaults()
            Else
                'developer guide no.9
                m_lReturn = uctCLMPayment.Initialise()
                If m_lReturn <> 1 Then
                    Throw New System.Exception("1, , initialise failed")
                End If
                'DC090606
                uctCLMPayment.ShowCoInsurers = m_bShowCoinsurers
                '        '<Pankaj PN 39106>
                '        If m_iTask = PMView Then
                '            uctCLMPayment.ViewPaymentMode = True
                '        End If
                '        '</Pankaj PN 39106

                m_lReturn = uctCLMPayment.SetProcessModes(vTask:=m_iTask, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)
                If m_lReturn <> 1 Then
                    Throw New System.Exception("1, , SetProcessModes failed")
                End If

                uctCLMPayment.WorkClaimID = m_lClaimID
                uctCLMPayment.WorkClaimPerilId = m_lPerilID
                uctCLMPayment.IsOpenClaimNoTrans = m_bOpenClaimNoTrans
                'TODOLIST:Changed as Per requirement
                m_lReturn = uctCLMPayment.Load_Renamed()
                If m_lReturn <> 1 Then
                    Throw New System.Exception("1, , Load failed")
                End If
            End If
            ' ****************************
            ' ****************************

            'Making it enable so that user can scroll if the text is large...
            txtComments.Enabled = True

            If m_iTask = gPMConstants.PMEComponentAction.PMView Or DisableScreen = gPMConstants.PMEReturnCode.PMTrue Then
                txtComments.ReadOnly = True
                txtComments.ForeColor = SystemColors.GrayText
            End If

            If m_sTransactionType = "C_CP" AndAlso SSTabHelper.GetTabVisible(SSTab1, ACPayment) Then
                SSTabHelper.SetSelectedIndex(SSTab1, ACPayment)
            End If
            'To display the title bar caption. (Shifted from Load Interface)
            Me.Text = Me.Text & " " & m_sScreenCaption

        Catch excep As System.Exception
            Select Case Information.Err().Number
                Case 380
                    ' do nothing
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                    Exit Sub
            End Select
        End Try


    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = m_oGeneral.ProcessCommand()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    'developer guide no.7
                    eventArgs.Cancel = True
                    Cancel = 1

                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If

            'AR20050204 - PN18518 Release the Risk screen
            If Not (m_oRisk Is Nothing) Then
                m_oRisk.Dispose()
                m_oRisk = Nothing
            End If

            ' Terminate the general object.
            m_oGeneral.Dispose()

            

            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing

            ' Destroy the instance of the business object
            ' from memory.
            g_oBusiness = Nothing

         

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

    Private Sub frmInterface_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        Dim iCtrlDown As Integer

        Const ACCtrlMask As Integer = 2
        Try

            ' Set the control key value.
            iCtrlDown = (Shift And ACCtrlMask) > 0

            With SSTab1
                ' Check the key pressed.
                Select Case KeyCode


                    Case Keys.PageUp
                        ' Page Up key has been pressed.
                        ' Check if the control key has also been pressed.
                        If iCtrlDown Then
                            ' Display the first tab.
                            SSTabHelper.SetSelectedIndex(SSTab1, 0)
                        Else
                            ' Check we are not on the first tab.
                            If SSTabHelper.GetSelectedIndex(SSTab1) > 0 Then
                                ' Display the previous tab.
                                SSTabHelper.SetSelectedIndex(SSTab1, SSTabHelper.GetSelectedIndex(SSTab1) - 1)
                            End If
                        End If

                    Case Keys.PageDown
                        ' Page Down key has been pressed.
                        ' Check if the control key has also been pressed.
                        If iCtrlDown Then
                            ' Display the last tab.
                            SSTabHelper.SetSelectedIndex(SSTab1, SSTabHelper.GetTabCount(SSTab1) - 1)
                        Else
                            ' Check we are not on the
                            ' last tab.
                            If SSTabHelper.GetSelectedIndex(SSTab1) < (SSTabHelper.GetTabCount(SSTab1) - 1) Then
                                ' Display the next tab.
                                SSTabHelper.SetSelectedIndex(SSTab1, SSTabHelper.GetSelectedIndex(SSTab1) + 1)
                            End If
                        End If

                    Case Keys.Home
                        ' Home key has been pressed.
                        ' Check if the control key has also been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on the tab.
                            If SSTabHelper.GetSelectedIndex(SSTab1) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                                m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(SSTab1)).Focus()
                            End If
                        End If

                    Case Keys.End
                        ' End key has been pressed.
                        ' Check if the control key has also been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on the tab.
                            If SSTabHelper.GetSelectedIndex(SSTab1) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                                m_ctlTabFirstLast(ACControlEnd, SSTabHelper.GetSelectedIndex(SSTab1)).Focus()
                            End If
                        End If
                End Select
            End With

        Catch
            Exit Sub
        End Try


        If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
            SSTab1.SelectedIndex = 0
        End If
        If eventArgs.Alt And eventArgs.KeyCode = Keys.D2 Then
            SSTab1.SelectedIndex = 1
        End If
        If eventArgs.Alt And eventArgs.KeyCode = Keys.D3 Then
            SSTab1.SelectedIndex = 2
        End If
        If eventArgs.Alt And eventArgs.KeyCode = Keys.D4 Then
            SSTab1.SelectedIndex = 11
        End If
        If eventArgs.Alt And eventArgs.KeyCode = Keys.D5 Then
            SSTab1.SelectedIndex = 4
        End If
        If eventArgs.Alt And eventArgs.KeyCode = Keys.D6 Then
            SSTab1.SelectedIndex = 5
        End If
        If eventArgs.Alt And eventArgs.KeyCode = Keys.D7 Then
            SSTab1.SelectedIndex = 6
        End If
        If eventArgs.Alt And eventArgs.KeyCode = Keys.D8 Then
            SSTab1.SelectedIndex = 7
        End If
        If eventArgs.Alt And eventArgs.KeyCode = Keys.D9 Then
            SSTab1.SelectedIndex = 8
        End If
    End Sub
    Private Sub SSTab1_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles SSTab1.SelectedIndexChanged

        Try

            With SSTab1
                ' Now I know this is crap, this goes against
                ' all my principles, but for some reason when
                ' using the mouse to select a tab the setfocus
                ' code below doesn't work. The cursor sticks,
                ' and you can't tab off. Therefore I've used
                ' this to get around the problem.
                Application.DoEvents()

                ' Set focus to the first control on the tab.
                If SSTabHelper.GetSelectedIndex(SSTab1) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                    m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(SSTab1)).Focus()
                End If
            End With

        Catch
            SStab1PreviousTab = SSTab1.SelectedIndex
        End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
        ' Click event of the OK button.
        Application.DoEvents()
                Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' validate data before continuing
            m_lReturn = ValidateOk()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            'DC280302 -start -check for underwriting/broking
            If m_sTransactionType <> "C_CP" And m_lDisableScreen <> gPMConstants.PMEReturnCode.PMTrue Then

                If SSTabHelper.GetTabVisible(SSTab1, ACGeneral) Then
                    m_lReturn = CheckMandatory()

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Set the mouse pointer to busy.
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                        Exit Sub
                    End If
                End If

                For lCount As Integer = ACDriver To ACWitness
                    If SSTabHelper.GetTabVisible(SSTab1, lCount) Then
                        Select Case lCount
                            Case ACDriver
                                If CDbl(Convert.ToString(uctDriver.Tag)) <> 0 Then
                                    If uctDriver.PartyCount = 0 Then
                                        SSTabHelper.SetSelectedIndex(SSTab1, ACDriver)
                                        ' show error message
                                        ' Display error stating the problem.

                                        ' Get description from the resource file.


                                        'Developer Guide No.: 243
                                        sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMandatoryTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                                        ''Developer Guide No.: 243
                                        sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMandatory, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                                        ' Display message.
                                        MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                                        ' Set the mouse pointer to busy.
                                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                                        Exit Sub
                                    End If
                                End If
                            Case ACThirdParty
                                If CDbl(Convert.ToString(uctThirdParty.Tag)) <> 0 Then
                                    If uctThirdParty.PartyCount = 0 Then
                                        SSTabHelper.SetSelectedIndex(SSTab1, ACThirdParty)
                                        ' show error message
                                        ' Display error stating the problem.

                                        ' Get description from the resource file.

                                        ''Developer Guide No.: 243
                                        sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMandatoryTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                                        ''Developer Guide No.: 243
                                        sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMandatory, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                                        ' Display message.
                                        ' Set the mouse pointer to busy.
                                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                                        MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                                        Exit Sub
                                    End If
                                End If
                            Case ACRepairer
                                If CDbl(Convert.ToString(uctRepairer.Tag)) <> 0 Then
                                    If uctRepairer.PartyCount = 0 Then
                                        SSTabHelper.SetSelectedIndex(SSTab1, ACRepairer)
                                        ' show error message
                                        ' Display error stating the problem.
                                        ' Get description from the resource file.
                                        'Developer Guide No.: 243
                                        sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMandatoryTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                                        'Developer Guide No.: 243
                                        sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMandatory, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                                        ' Display message.
                                        MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                                        ' Set the mouse pointer to busy.
                                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                                        Exit Sub
                                    End If
                                End If
                            Case ACWitness
                                If CDbl(Convert.ToString(uctWitness.Tag)) <> 0 Then
                                    If uctWitness.PartyCount = 0 Then
                                        SSTabHelper.SetSelectedIndex(SSTab1, ACWitness)
                                        ' show error message
                                        ' Display error stating the problem.

                                        ' Get description from the resource file.

                                        'Developer Guide No.: 243
                                        sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMandatoryTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                                        'Developer Guide No.: 243
                                        sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMandatory, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                                        ' Display message.
                                        MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                                        ' Set the mouse pointer to busy.
                                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                                        Exit Sub
                                    End If
                                End If
                            Case Else
                                ' do nothing
                        End Select
                    End If
                Next lCount
            End If

            'AJM 14/08/2001 - Save other party details
            m_lReturn = SaveOtherParty()

            If m_lDisableScreen <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = m_oGeneral.ProcessCommand()
            End If

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception

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
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.

                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ' RAM20021023 : Close the Party  Policy Summary Screen, if they
                '               are loaded
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                m_lReturn = ClosePartyPolicySummary()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                End If
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                Me.Hide()
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    'Private Sub cmdNavigate_Click()
    '
    ' Click event of the Navigate button.
    '
    'Try 
    '
    ' Set the interface status.
    'm_lStatus = gPMConstants.PMEReturnCode.PMNavigate
    '
    ' Process the next set of actions depending
    ' upon the interface task etc.
    'm_lReturn = m_oGeneral.ProcessCommand()
    '
    ' Check the return value.
    'If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
    ' Everything OK, so we can hide the interface.
    'Me.Hide()
    'End If
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Exit Sub
    '
    'End Try
    '
    'End Sub


    'Private Sub cmdNext_Click(ByRef Index As Integer)
    '
    'Try 
    '
    ' Change to the next tab.
    'If SSTabHelper.GetSelectedIndex(SSTab1) < SSTabHelper.GetTabCount(SSTab1) - 1 Then
    'SSTabHelper.SetSelectedIndex(SSTab1, Index + 1)
    'End If
    '
    ' Set focus to the first control on the tab.
    'If SSTabHelper.GetSelectedIndex(SSTab1) <= m_ctlTabFirstLast.GetUpperBound(1) Then
    'm_ctlTabFirstLast(ACControlStart, Index + 1).Focus()
    'End If
    '
    'Catch 
    '
    '
    '
    '
    'Exit Sub
    'End Try
    '
    '
    'End Sub

    ' ***************************************************************** '
    ' Name: GetPos
    '
    ' Description: calls the GetPos Event for the controls that have to
    '              be aligned at run time
    '
    ' ***************************************************************** '

    Public Function GetPos() As Integer
        Dim lLasttop, lTop As Integer
        Dim lctrlcount As Integer

        Try
            lLasttop = 0
            lctrlcount = 0

            For Each ctrl As Control In ContainerHelper.Controls(Me)

                If (TypeOf ctrl Is TextBox) Or (TypeOf ctrl Is ComboBox) Or (ctrl.Name = "chkBox") Then
                    If ctrl.Name <> "txtComments" Then
                        lctrlcount += 1
                    End If
                End If
            Next ctrl

            If lctrlcount > 5 Then

                For Each ctrl As Control In ContainerHelper.Controls(Me)
                    If (TypeOf ctrl Is TextBox) Or (TypeOf ctrl Is ComboBox) Or (ctrl.Name = "chkBox") Then
                        If ctrl.Name <> "txtComments" Then
                            lTop = CInt(VB6.PixelsToTwipsY(ctrl.Top))
                            If lTop < lLasttop Then
                                lTop = lLasttop
                            Else
                                lLasttop = lTop
                            End If
                        End If
                    End If
                Next ctrl
            ElseIf lctrlcount = 3 Then
                Return CInt(VB6.PixelsToTwipsY(cmbBox(0).Top))
            ElseIf lctrlcount = 4 Then
                Return CInt(VB6.PixelsToTwipsY(txtBox(0).Top))
            ElseIf lctrlcount = 5 Then
                Return CInt(VB6.PixelsToTwipsY(chkBox(0).Top))
            End If

            Return CInt(lTop + VB6.PixelsToTwipsY(cmbBox(0).Height) + ((VB6.PixelsToTwipsY(txtBox(0).Top) - VB6.PixelsToTwipsY(cmbBox(0).Top)) - VB6.PixelsToTwipsY(cmbBox(0).Height)))

        Catch excep As System.Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Positions for the Controls on the Form", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPos", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Function
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetReserveType
    '
    ' Description: Gets the Types of reserves
    '
    ' RWH(07/02/2001) RSAB #218 Remove Salvage/Third Party from Total
    '                 Initial Reserve for Underwriting.
    ' ***************************************************************** '

    Public Function GetReserveType() As Integer
        Dim result As Integer = 0
        Dim r_vReserveTypeArray As Object

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            'DC280302 -end


            m_lReturn = g_oBusiness.GetReserveType(r_vReserveTypeArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the GetReserveType", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReserveType", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: UpdateGeneral
    '
    ' Description: updates the general details for the Particular Peril
    '
    ' ***************************************************************** '

    Public Function UpdateGeneral() As Integer
        Dim result As Integer = 0
        Dim v_vGeneralDetailsArray(,) As Object
        Dim iCount As Integer
        Dim sText As String = ""
        'Const AC_COL_CLAIMPERIL_DESC As Integer = 3
        Const AC_EVENT_TYPE_UPDATECLAIM As Integer = 6
        Dim sEventDescription As String = ""

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            ' focus on the first tab
            SSTabHelper.SetSelectedIndex(SSTab1, 0)
            iCount = 0
            'Developer Guide No. NIIT-Modified the loop as per the requirement

            For Each ctlFormControl As Control In ContainerHelper.Controls(Me)
                Dim ctlArray As ArrayList = New ArrayList()
                If ctlFormControl.HasChildren Then
                    'ctlArray = GetControl(ctlControl:=ctlFormControl)
                    ControlList(ctlFormControl, ctlArray)
                End If
                For i As Integer = 0 To ctlArray.Count - 1
                    Dim chkControl As Control = CType(ctlArray(i), Control)
                    If (TypeOf chkControl Is TextBox) Or (TypeOf chkControl Is CheckBox) Or (TypeOf chkControl Is ComboBox) Then
                        If chkControl.Name <> "txtComments" And Convert.ToString(ControlHelper.GetTag(chkControl)) <> "" Then
                            If Information.IsArray(v_vGeneralDetailsArray) Then
                                ReDim Preserve v_vGeneralDetailsArray(1, iCount)
                            Else
                                ReDim v_vGeneralDetailsArray(1, iCount)
                            End If

                            v_vGeneralDetailsArray(0, iCount) = Convert.ToString(ControlHelper.GetTag(chkControl))
                            If chkControl.Name.ToLower.Contains("chkBox".ToLower) Then


                                'Developer Guide No. TODOLIST
                                v_vGeneralDetailsArray(1, iCount) = CType(chkControl, CheckBox).CheckState
                            Else
                                v_vGeneralDetailsArray(1, iCount) = chkControl.Text
                            End If

                            sText = CStr(v_vGeneralDetailsArray(1, iCount))
                            iCount += 1

                        End If
                    End If
                Next
            Next ctlFormControl

            m_lReturn = g_oBusiness.UpdateGeneral(v_vGeneralDetailsArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If
            If m_oGeneral.GeneralText <> sText Then
                sEventDescription = Interaction.InputBox("Enter the Event Description", "Event Log", sEventDescription)

                m_lReturn = g_oBusiness.CreateEvent(AC_EVENT_TYPE_UPDATECLAIM, sEventDescription)
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the UpdateGeneral", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateGeneral", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: UpdatePartyDetails
    ' Description: Updates the Party's assosciated with a particular peril and claim
    ' ***************************************************************** '
    Public Function UpdatePartyDetails() As Integer
        Return gPMConstants.PMEReturnCode.PMTrue
        'Commented out for ages, so Removed 01/10/03
    End Function

    ' ***************************************************************** '
    ' Name: AddComments
    ' Description: Updating the comments value in the peril table
    ' ***************************************************************** '
    Public Function AddComments() As Integer
        Dim result As Integer = 0
        Dim v_vComments As String = ""
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' get the values in an array
            v_vComments = txtComments.Text

            ' pass the values to the business Array

            m_lReturn = g_oBusiness.AddCommentsUW(v_vComments)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            '    Else
            '        m_lReturn = g_oBusiness.AddComments(v_vComments)
            '        If m_lReturn <> PMTrue Then
            '            GoTo Err_AddComments
            '        End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the AddComments", vApp:=ACApp, vClass:=ACClass, vMethod:="AddComments", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: Toolbar1_ButtonClick
    ' Description:
    ' History: 03/10/2002 SJ - Created.
    ' RAM20021022   : 1. Added two toolbar buttons for Party & Policy Summary
    '                    (NRMA Changes - Sirius Process No: 126 - Start)
    '                 2. Added  Party Short Name Parameter
    ' Kevin Renshaw (CMG) 26/3/2003 - using m_oBusiness instead of g_oBusiness
    ' ***************************************************************** '
    Private Sub Toolbar1_ButtonClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles TSB_Event.Click, _Toolbar1_Button2.Click, Risk_Details.Click, _Toolbar1_Button4.Click, Information_Checklist.Click, _Toolbar1_Button6.Click, Party.Click, Policy.Click, Risk.Click
        Dim Button As ToolStripItem = CType(eventSender, ToolStripItem)

        Try

            Dim lClaimID As Integer
            Dim sClaimRef As String = ""


            Select Case Button.Name
                Case "TSB_Event"

                    'sj 02/10/2002 - start
                    If Not m_bClientPolicyDetailsLoaded Then

                        m_lReturn = g_oBusiness.GetClientPolicyDetails(v_lInsuranceFileCnt:=m_lInsurance_file_cnt, r_lPartyCnt:=m_lPartycnt, r_sPartyShortName:=m_sPartyShortName, r_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, r_sInsuranceRef:=m_sInsuranceRef)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="m_oBusiness.GetClientPolicyDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Toolbar1_ButtonClick")
                            Exit Sub
                        End If

                        m_bClientPolicyDetailsLoaded = True
                    End If



                    If m_sTransactionType = "C_CO" Then
                        lClaimID = 0 'we don't have any event for this claim yet
                    Else
                        'get original claim id

                        m_lReturn = g_oBusiness.GetOriginalClaimID(v_lClaimId:=m_lClaimID, r_lOriginalClaimID:=lClaimID)
                    End If



                    m_lReturn = g_oBusiness.GetClaimNumberFromClaim(v_lClaimId:=lClaimID, r_sClaimRef:=sClaimRef)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="g_oBusiness.GetClaimNumber Failed for claimid " & lClaimID, vApp:=ACApp, vClass:=ACClass, vMethod:="Toolbar1_ButtonClick")
                        Exit Sub
                    End If
                    'developer guide no.294
                    SharedFiles.iPMBListEvents.g_oObjectManager = g_oObjectManager
                    m_lReturn = SharedFiles.iPMBListEvents.ShowEvents(v_lPartyCnt:=m_lPartycnt, v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_lInsuranceFileCnt:=m_lInsurance_file_cnt, v_lClaimCnt:=lClaimID, v_sInsuranceRef:=m_sInsuranceRef, v_sClaimRef:=sClaimRef, v_sTransactionType:=m_sTransactionType)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="ShowEvents Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Toolbar1_ButtonClick")
                        Exit Sub
                    End If

                Case "Party"
                    ' RAM20021022 : NRMA Changes (Sirius Process No 126)
                    m_lReturn = ShowPartySummaryDetails()
                Case "Policy"
                    ' RAM20021022 : NRMA Changes (Sirius Process No 126)
                    m_lReturn = ShowPolicySummaryDetails()
                Case "Risk"
                    ' RAM20021024 : NRMA Changes (Sirius Process No 126)
                    m_lReturn = ShowRiskDetails()
            End Select

        Catch excep As System.Exception
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Toolbar1_ButtonClick Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Toolbar1_ButtonClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub txtBox_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _txtBox_0.Enter
        Dim Index As Integer = Array.IndexOf(txtBox, eventSender)
        Dim lTagValue As Integer

        Try
            lTagValue = CInt(Convert.ToString(txtBox(Index).Tag))

            For lCount As Integer = vDataTypeArray.GetLowerBound(1) To vDataTypeArray.GetUpperBound(1)

                If CDbl(vDataTypeArray(1, lCount)) = lTagValue Then
                    Select Case vDataTypeArray(0, lCount)
                        Case "Text", "Integer"
                            ' do nothing
                        Case "Date"
                            If txtBox(Index).Text.Trim() <> "" Then
                                txtBox(Index).Text = StringsHelper.Format(txtBox(Index).Text, ACShortDate)
                                iPMFunc.SelectText(txtBox(Index))
                            End If
                    End Select
                    Exit For
                End If
            Next lCount

        Catch

            Exit Sub
        End Try
    End Sub

    Private Sub txtBox_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _txtBox_0.Leave
        Dim Index As Integer = Array.IndexOf(txtBox, eventSender)
        Dim lTagValue As Integer

        Try
            lTagValue = CInt(Convert.ToString(txtBox(Index).Tag))

            For lCount As Integer = vDataTypeArray.GetLowerBound(1) To vDataTypeArray.GetUpperBound(1)

                If CDbl(vDataTypeArray(1, lCount)) = lTagValue Then
                    Select Case vDataTypeArray(0, lCount)
                        Case "Text"
                            ' do nothing
                        Case "Integer"

                            'developer guide no.243
                            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInvalidIntegerData, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                            If txtBox(Index).Text.Trim() <> "" Then
                                txtBox(Index).Text = CStr(CInt(txtBox(Index).Text))
                            End If
                        Case "Date"

                            'developer guide no.243
                            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInvalidDateData, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                            If txtBox(Index).Text.Trim() <> "" Then
                                If CBool(CDate(txtBox(Index).Text).ToOADate) Then
                                    txtBox(Index).Text = StringsHelper.Format(txtBox(Index).Text, ACDateDisplay)
                                End If
                            End If

                    End Select
                    Exit For
                End If
            Next lCount

        Catch

            ' Get description from the resource file.
            'developer guide no.243
            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInvalidDataTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Display message.
            MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
            txtBox(Index).Text = ""
            txtBox(Index).Focus()
        End Try
    End Sub

    Private Sub uctCLMPayment_UnRecoverableError(ByVal Sender As Object, ByVal e As EventArgs) Handles uctCLMPayment.UnRecoverableError
        'ECK Oct 2005 Add Broking Check
        If m_sTransactionType = "C_CP" Then
            ' if the payment control blows up
            ' disable ok so user has to cancel
            ' from peril screen
            ' ensuring no changes are saved....
            cmdOK.Enabled = False
        End If
    End Sub

    Private Sub uctCLMReserve_DataHasChanged(ByVal Sender As Object, ByVal e As uctCLMReserve.DataHasChangedEventArgs) Handles uctCLMReserve.DataHasChanged
        'developer guide no. As per VB Code
        Dim NewData As Object = e.NewData
        If m_bOpenClaimNoTrans Then
            uctCLMPayment.UpdateReserveItems(NewData)
        End If
    End Sub

    Private Sub VScroll_Change(ByRef Index As Integer, ByVal newScrollValue As Integer)

        Picture1(Index).Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Picture1(Index).Height) * ((-VScroll_Renamed(Index).Value) * 0.01))
    End Sub

    Private Sub VScroll_Scroll_Renamed(ByRef Index As Integer, ByVal newScrollValue As Integer)

        Picture1(Index).Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Picture1(Index).Height) * ((-VScroll_Renamed(Index).Value) * 0.01))
    End Sub

    Private Function CheckMandatory() As Integer
        Dim result As Integer = 0
        Dim lCount As Integer
        Dim chkControl As Control
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' check the mandatory controls in the general tab

            For Each ctrl As Control In ContainerHelper.Controls(Me)
                Dim ctlArray As ArrayList = New ArrayList()
                If ctrl.HasChildren Then
                    'ctlArray = GetControl(ctlControl:=ctlFormControl)
                    ControlList(ctrl, ctlArray)
                End If
                For i As Integer = 0 To ctlArray.Count - 1
                    chkControl = CType(ctlArray(i), Control)
                    If TypeOf chkControl Is Label Then
                        If Convert.ToString(ControlHelper.GetTag(chkControl)) <> "0" Then
                            lCount = gPMFunctions.ToSafeInteger(Mid(Convert.ToString(ControlHelper.GetTag(chkControl)), 7))
                            'If Convert.ToString(ControlHelper.GetTag(chkControl)).IndexOf("txtBox") >= 0 Then
                            If InStr(1, ControlHelper.GetTag(chkControl), "txtBox") > 0 Then
                                If txtBox(lCount).Text.Trim() = "" Then
                                    txtBox(lCount).Focus()
                                    Throw New Exception()
                                End If
                                'ElseIf Convert.ToString(ControlHelper.GetTag(chkControl)).IndexOf("cmbBox") >= 0 Then
                            ElseIf InStr(1, ControlHelper.GetTag(chkControl), "cmbBox") > 0 Then
                                If cmbBox(lCount).Text.Trim() = "" Then
                                    cmbBox(lCount).Focus()
                                    Throw New Exception()
                                End If
                                'ElseIf Convert.ToString(ControlHelper.GetTag(chkControl)).IndexOf("chkBox") >= 0 Then
                            ElseIf InStr(1, ControlHelper.GetTag(chkControl), "chkBox") > 0 Then
                                If StringsHelper.ToDoubleSafe(CStr(chkBox(lCount).CheckState).Trim()) = 0 Then
                                    chkBox(lCount).Focus()
                                    Throw New Exception()
                                End If
                            End If
                        End If
                    End If
                Next
            Next ctrl

            Return result

        Catch

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Display error stating the problem.
            SSTabHelper.SetSelectedIndex(SSTab1, ACGeneral)
            ' Get description from the resource file.

            'developer guide no.243
            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMandatoryTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'developer guide no.243
            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMandatory, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Display message.
            MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetComments
    '
    ' Description: Getting the comments from the claim peril table
    '
    ' ***************************************************************** '

    Public Function GetComments() As Integer

        'DC240402
        Dim result As Integer = 0
        Dim r_vComments As Object
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            txtComments.Text = ""

            'DC240402 -Start

            m_lReturn = g_oBusiness.GetCommentsUW(r_vComments)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            ' get the values in an array
            If Information.IsArray(r_vComments) Then

                txtComments.Text = CStr(r_vComments(0, 0))
            End If



            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the GetComments", vApp:=ACApp, vClass:=ACClass, vMethod:="GetComments", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetPaymentPartyid
    '
    ' Description: Instance PaymentMethod to retrieve Policyholder
    '
    ' Date : 15/07/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Public Function GetPaymentPartyid(ByRef lPaymentPartyId As Integer, ByVal cAmount As Decimal, ByRef sOComments As String, ByRef lButtonClicked As Integer, Optional ByVal sIComments As String = "", Optional ByVal iCurrencyID As Integer = 0) As Integer
        Dim result As Integer = 0
        Dim oPaymentMethod As iCLMPaymentMethod.Interface_Renamed

        Dim r_vClaimClientAndAgent As Object
        Const ACPaymentMethod As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create Find Party object
            Dim temp_oPaymentMethod As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPaymentMethod, sClassName:="iCLMPaymentMethod.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oPaymentMethod = temp_oPaymentMethod

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create object 'iPMBFindParty.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPaymentPartyid", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Set component properties and start interface

            oPaymentMethod.CallingAppName = ACApp

            oPaymentMethod.ScreenMethod = ACPaymentMethod

            oPaymentMethod.Amount = m_cTotalPayment

            oPaymentMethod.Comments = sIComments

            oPaymentMethod.CurrencyID = iCurrencyID

            oPaymentMethod.ClaimID = m_lClaimID

            oPaymentMethod.InsuranceFileCnt = m_lInsurance_file_cnt

            oPaymentMethod.LossCurrencyAmount = m_cTotalPayment * g_dPaymentLossRate

            oPaymentMethod.LossCurrencyID = m_lCurrencyID

            'set agent and party default properties

            m_lReturn = g_oBusiness.GetClaimClientAndAgent(m_lClaimID, r_vClaimClientAndAgent)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Claim Client and Agent details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPaymentPartyid", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If


            If CStr(r_vClaimClientAndAgent(1, 0)) = "" Then

                oPaymentMethod.ClientID = 0
            Else


                oPaymentMethod.ClientID = r_vClaimClientAndAgent(1, 0)
            End If



            oPaymentMethod.ClientName = r_vClaimClientAndAgent(3, 0)



            If CStr(r_vClaimClientAndAgent(0, 0)) = "" Then

                oPaymentMethod.AgentID = 0
            Else


                oPaymentMethod.AgentID = r_vClaimClientAndAgent(0, 0)
            End If



            oPaymentMethod.AgentName = r_vClaimClientAndAgent(2, 0)

            'Also pass product id

            If CStr(r_vClaimClientAndAgent(4, 0)) = "" Then

                oPaymentMethod.ProductID = 0
            Else


                oPaymentMethod.ProductID = r_vClaimClientAndAgent(4, 0)
            End If




            m_lReturn = oPaymentMethod.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process object 'iCLMPaymentMethod.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPaymentPartyid", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Retrieve Party Shortname and set as Agent

            lPaymentPartyId = oPaymentMethod.Partyid

            lButtonClicked = oPaymentMethod.ButtonClicked

            sOComments = oPaymentMethod.Comments

            g_lPayeeMediaType = oPaymentMethod.PayeeMediaType

            g_sPayeeName = oPaymentMethod.PayeeName

            g_sPayeeBankName = oPaymentMethod.PayeeBankName

            g_sPayeeSortCode = oPaymentMethod.PayeeSortCode

            g_sPayeeAccountNo = oPaymentMethod.PayeeAccountNo

            g_lPayeeCountry = oPaymentMethod.PayeeCountry

            g_sPayeeComments = oPaymentMethod.PayeeComments

            ' Destroy Find Party object

            oPaymentMethod.Dispose()
            oPaymentMethod = Nothing


            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPaymentPartyid Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPaymentPartyid", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: SaveOtherParty
    '
    ' Description: save driver, repairer etc
    '
    ' History: 14/08/2001 AJM - Created.
    '
    ' ***************************************************************** '
    Private Function SaveOtherParty() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If SSTabHelper.GetTabVisible(SSTab1, ACDriver) Then
                m_lReturn = uctDriver.OKClick()
            End If

            If SSTabHelper.GetTabVisible(SSTab1, ACThirdParty) Then
                m_lReturn = uctThirdParty.OKClick()
            End If

            If SSTabHelper.GetTabVisible(SSTab1, ACRepairer) Then
                m_lReturn = uctRepairer.OKClick()
            End If

            If SSTabHelper.GetTabVisible(SSTab1, ACWitness) Then
                m_lReturn = uctWitness.OKClick()
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveOtherParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveOtherParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name          : ShowPartySummaryDetails
    ' Description   : This function will create the Party Summary Interface,
    '                   set it keys and show the interface
    ' Edit History  :
    ' RAM20021021   : Created  - NRMA Changes (Sirius Process No 126)
    ' ***************************************************************** '
    Public Function ShowPartySummaryDetails() As Integer


        Dim result As Integer = 0
        Dim ACShowFormModal As FormShowConstants = FormShowConstants.Modal ' Show the Form vbModal

        Dim ACShowFormModeLess As FormShowConstants = FormShowConstants.Modeless ' Show the Form vbModeless

        ' The following are PMNavKey Constants.
        ' Note : These constants should be deleted, if we include the PMNavKeyConst.bas file
        Const PMKeyNamePartyCnt As String = "party_cnt"
        Const PMKeyNameShortName As String = "shortname"
        Const PMKeyNameDisplayMode As String = "display_mode"

        Dim vKeyArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' First fetch the basic details for Party / Policy
            If Not m_bClientPolicyDetailsLoaded Then

                m_lReturn = g_oBusiness.GetClientPolicyDetails(v_lInsuranceFileCnt:=m_lInsurance_file_cnt, r_lPartyCnt:=m_lPartycnt, r_sPartyShortName:=m_sPartyShortName, r_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, r_sInsuranceRef:=m_sInsuranceRef)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="GetClientPolicyDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPartySummaryDetails")
                    Return result
                End If

                m_bClientPolicyDetailsLoaded = True
            End If

            ReDim vKeyArray(1, 2) ' Set the Navigator Keys


            vKeyArray(0, 0) = PMKeyNamePartyCnt ' Sent in the Party Cnt

            vKeyArray(1, 0) = m_lPartycnt


            vKeyArray(0, 1) = PMKeyNameShortName

            vKeyArray(1, 1) = m_sPartyShortName.Trim() ' Sent in the Party Short Name


            vKeyArray(0, 2) = PMKeyNameDisplayMode
            ' SET 13/08/2004 ISS14119 - display modally

            vKeyArray(1, 2) = ACShowFormModal

            If m_oPartySummary Is Nothing Then

                ' Create the Interface if not available
                'developer guide no.108
                m_oPartySummary = New iSIRPartySummary.Interface_Renamed()
                'developer guide no.9
                m_lReturn = m_oPartySummary.Initialise()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oPartySummary.Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPartySummaryDetails")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_oPartySummary.CallingAppName = ACApp


                m_lReturn = m_oPartySummary.SetKeys(vKeyArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oPartySummary.SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPartySummaryDetails")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oPartySummary.Start()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oPartySummary.Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPartySummaryDetails")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else
                ' Swith to the Party Summary Interface (i.e show it on top of all interface)
                m_lReturn = m_oPartySummary.switchTo()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oPartySummary.switchTo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPartySummaryDetails")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            ' SET 13/08/2004 ISS14119 - clean up the interface
            m_lReturn = ClosePartyPolicySummary()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
            End If
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowPartySummaryDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPartySummaryDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name          : ShowPolicySummaryDetails
    '
    ' Description   : This function will create the Policy Summary Interface,
    '                   set it keys and show the interface
    '
    ' Edit History  :
    ' RAM20021021   : Created  - NRMA Changes (Sirius Process No 126)
    ' ***************************************************************** '
    Public Function ShowPolicySummaryDetails() As Integer


        Dim result As Integer = 0
        Dim ACShowFormModal As FormShowConstants = FormShowConstants.Modal ' Show the Form vbModal

        Dim ACShowFormModeLess As FormShowConstants = FormShowConstants.Modeless ' Show the Form vbModeless

        ' The following are PMNavKey Constants.
        ' Note : These constants should be deleted, if we include the PMNavKeyConst.bas file
        Const PMKeyNamePartyCnt As String = "party_cnt"
        Const PMKeyNameShortName As String = "shortname"
        Const PMKeyNameInsuranceFolderCnt As String = "insurance_folder_cnt"
        Const PMKeyNameInsuranceFileCnt As String = "insurance_file_cnt"
        Const PMKeyNameInsReference As String = "insurance_ref"
        Const PMKeyNameDisplayMode As String = "display_mode"

        Dim vKeyArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' First fetch the basic details for Party / Policy
            If Not m_bClientPolicyDetailsLoaded Then

                m_lReturn = g_oBusiness.GetClientPolicyDetails(v_lInsuranceFileCnt:=m_lInsurance_file_cnt, r_lPartyCnt:=m_lPartycnt, r_sPartyShortName:=m_sPartyShortName, r_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, r_sInsuranceRef:=m_sInsuranceRef)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="GetClientPolicyDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPolicySummaryDetails")
                    Return result
                End If

                m_bClientPolicyDetailsLoaded = True
            End If

            ReDim vKeyArray(1, 5) ' Set the Navigator Keys


            vKeyArray(0, 0) = PMKeyNamePartyCnt ' Sent in the Party Cnt

            vKeyArray(1, 0) = m_lPartycnt


            vKeyArray(0, 1) = PMKeyNameShortName

            vKeyArray(1, 1) = m_sPartyShortName.Trim()


            vKeyArray(0, 2) = PMKeyNameInsuranceFolderCnt

            vKeyArray(1, 2) = m_lInsuranceFolderCnt


            vKeyArray(0, 3) = PMKeyNameInsuranceFileCnt

            vKeyArray(1, 3) = m_lInsurance_file_cnt


            vKeyArray(0, 4) = PMKeyNameInsReference

            vKeyArray(1, 4) = m_sInsuranceRef.Trim()


            vKeyArray(0, 5) = PMKeyNameDisplayMode
            ' SET 13/08/2004 ISS14119 - display modally

            vKeyArray(1, 5) = ACShowFormModal

            If m_oPolicySummary Is Nothing Then

                ' Create the Interface if not available


                m_oPolicySummary = New iSIRPolicySummary.Interface_Renamed()

                'developer guide no.9
                m_lReturn = m_oPolicySummary.Initialise()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oPolicySummary.Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPolicySummaryDetails")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_oPolicySummary.CallingAppName = ACApp

                m_lReturn = m_oPolicySummary.SetKeys(vKeyArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oPolicySummary.SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPolicySummaryDetails")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oPolicySummary.Start()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oPolicySummary.Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPolicySummaryDetails")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else
                ' Swith to the Policy Summary Interface  (i.e show it on top of all interface)

                m_lReturn = m_oPolicySummary.switchTo()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oPolicySummary.switchTo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPolicySummaryDetails")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If
            ' SET 13/08/2004 ISS14119 - clean up the interface
            m_lReturn = ClosePartyPolicySummary()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
            End If
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowPolicySummaryDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPolicySummaryDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name          : ShowRiskDetails
    '
    ' Description   : This function will Show the Risk Detail Iterface
    '
    ' Edit History  :
    ' RAM20021023   : Created  - NRMA Changes (Sirius Process No 126)
    ' ***************************************************************** '
    Private Function ShowRiskDetails() As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oRisk Is Nothing Then


                m_lReturn = g_oBusiness.GetRiskDetails(v_lClaimId:=m_lClaimID, r_vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                        MessageBox.Show("No risk screen exists for this policy record.", "Risk Details Missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    End If
                    Return m_lReturn
                End If

                m_oRisk = New iPMURiskWrapper.Interface_Renamed()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If
                'developer guide no.9
                m_lReturn = m_oRisk.Initialise()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

                With m_oRisk
                    .InsuranceFolderCnt = CInt(vResultArray(0, 0))
                    .InsuranceFileCnt = CInt(vResultArray(1, 0))
                    .ProductId = CInt(vResultArray(2, 0))
                    .RiskId = CInt(vResultArray(3, 0))
                    .RiskTypeId = CInt(vResultArray(4, 0))
                    .ScreenId = CInt(vResultArray(5, 0))
                    m_lReturn = .Start()
                End With
            Else
                m_lReturn = m_oRisk.SwithTo()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oRisk.SwithTo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowRiskDetails")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowRiskDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowRiskDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name          : ClosePartyPolicySummary
    ' Description   : This function will Close the Party & Policy Summary
    '                   Interfaces, if they are loaded
    ' Edit History  :
    ' RAM20021023   : Created  - NRMA Changes (Sirius Process No 126)
    ' ***************************************************************** '
    Private Function ClosePartyPolicySummary() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Terminate the objects if necessary and available
            If m_oPartySummary Is Nothing Then
                ' Do nothing
            Else
                ' Terminate
                m_oPartySummary.Dispose()
                'Clear it
                m_oPartySummary = Nothing
            End If

            If m_oPolicySummary Is Nothing Then
                ' Do nothing
            Else
                ' Terminate
                m_oPolicySummary.Dispose()
                'Clear it
                m_oPolicySummary = Nothing
            End If

            If m_oRisk Is Nothing Then
                ' Do nothing
            Else
                m_oRisk.Dispose()
                'Clear it
                m_oRisk = Nothing
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ClosePartyPolicySummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ClosePartyPolicySummary", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    Private Function UseAuthorisedScriptsForClaimPayments(ByRef r_bCheckAuthorisation As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UseAuthorisedScriptsForClaimPayments"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = CType(GetProductDetailsForClaim(r_bCheckAuthorisation), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetProductDetailsForClaim Failed ", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function


    Private Sub uctCLMPayment_DataHasChanged(ByVal Sender As Object, ByVal e As uctCLMPayment1.DataHasChangedEventArgs) Handles uctCLMPayment.DataHasChanged

        Const clPaidToDateColumn As Integer = 1
        Const clPaymentAmountColumn As Integer = 2
        Const clReserveIDColumn As Integer = 3
        Const clReserveAdjustmentColumn As Integer = 5
        Dim NewData(,) As Object = e.NewData
        Dim udtPay As uctCLMReserveControl.uctCLMReserve.udtPaymentDetails

        Try

            Dim dbNumericTemp As Double
            If m_bOpenClaimNoTrans Then

                udtPay.lReserveId = NewData(0, 0)
                udtPay.cTotalPayment = NewData(1, 0)

                udtPay.cReserveAdjustment = NewData(2, 0)
                uctCLMReserve.UpdatePaymentValue(udtPay)
            ElseIf Double.TryParse(Convert.ToString(uctCLMReserve.Tag), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                For iCnt As Integer = NewData.GetLowerBound(1) To NewData.GetUpperBound(1)
                    ' update the reserve control
                    With udtPay
                        .lReserveId = NewData(clReserveIDColumn, iCnt)
                        .cTotalPayment = CDbl(NewData(clPaymentAmountColumn, iCnt)) + CDbl(NewData(clPaidToDateColumn, iCnt))
                        .cReserveAdjustment = NewData(clReserveAdjustmentColumn, iCnt)
                    End With
                    ' send the new values to the control
                    uctCLMReserve.UpdatePaymentValue(udtPay)
                Next iCnt

            End If

        Catch excep As System.Exception


            MessageBox.Show(CStr(Information.Err().Number) & Strings.Chr(13) & Strings.Chr(10) & excep.Message, Application.ProductName)

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: SaveReserveAndPaymentDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 09-09-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Public Function SaveReserveAndPaymentDetails() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SaveReserveAndPaymentDetails"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vOptionValue As Object
        Try



            result = gPMConstants.PMEReturnCode.PMTrue
            'ECK Oct 2005 Add Broking Check
            'developer guide no.98
            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableRI2007, v_vBranch:=g_oObjectManager.SourceID, r_vUnderwriting:=vOptionValue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SaveReserveAndPaymentDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If gPMFunctions.NullToString(vOptionValue) = "1" Then
                m_bIsRI2007Enabled = True
            End If



            If m_bOpenClaimNoTrans And m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                ' save reserve details
                lReturn = uctCLMReserve.Save()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "SaveReserveAndPaymentDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
                ' so save payment details
                lReturn = uctCLMPayment.Save()
                If lReturn <> gPMConstants.PMEReturnCode.PMCancel Then
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "SaveReserveAndPaymentDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                Else
                    result = lReturn
                End If
            Else
                ' if this is not claim payment
                If m_sTransactionType <> "C_CP" Then
                    ' save reserve details
                    lReturn = uctCLMReserve.Save()
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "SaveReserveAndPaymentDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                Else
                    ' else this is claim payment
                    ' so save payment details
                    lReturn = uctCLMPayment.Save()
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "SaveReserveAndPaymentDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '            Return result
            '            Resume
            '            Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: ValidateOk
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function ValidateOk() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ValidateOk"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue
            'ECK Oct 2005 Add Broking Check
            '<Pankaj PN:39033 validate for m_bOpenClaimNoTrans also >
            If (m_sTransactionType = "C_CP") Or m_bOpenClaimNoTrans Then

                If uctCLMPayment.ValidPayment() <> gPMConstants.PMEReturnCode.PMTrue Then
                    'MsgBox "Invalid Payment Amount specified", vbExclamation, "Claim Payment Validation"
                    SSTabHelper.SetSelectedIndex(SSTab1, ACPayment)
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
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

    Private Function GetProductDetailsForClaim(Optional ByRef r_bRunAuthorisationScriptsForClaimPayments As Boolean = False, Optional ByRef r_bAllowNegativeReserve As Boolean = False) As Integer
        Dim result As Integer = 0

        Const kMethodName As String = "GetProductDetailsForClaim"

        Dim lReturn As gPMConstants.PMEReturnCode

        Dim o_ProductBusiness As bSIRProduct.Business
        Dim bAllow_Negative_Reserve, bRun_authorisation_scripts_claim_payments As Boolean
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_o_ProductBusiness As Object
            lReturn = g_oObjectManager.GetInstance(temp_o_ProductBusiness, "bSIRProduct.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            o_ProductBusiness = temp_o_ProductBusiness
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetInstance of bSIRProduct.Business Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            lReturn = o_ProductBusiness.GetProductLevelOptionsForClaim(v_lClaimID:=m_lClaimID, r_bRun_authorisation_scripts_claim_payments:=bRun_authorisation_scripts_claim_payments, r_bAllow_Negative_Reserve:=bAllow_Negative_Reserve)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetProductDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not False Then
                r_bRunAuthorisationScriptsForClaimPayments = bRun_authorisation_scripts_claim_payments
            End If
            If Not False Then
                r_bAllowNegativeReserve = bAllow_Negative_Reserve
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
    'Modified by Sudhanshu Behera on 6/10/2010 3:37:20 PM refer developer guide no. Todo List
    Private Sub VScroll_Renamed_Scroll(ByVal eventSender As Object, ByVal eventArgs As ScrollEventArgs)
        Dim Index As Integer = Array.IndexOf(VScroll_Renamed, eventSender)
        Select Case eventArgs.Type
            Case ScrollEventType.ThumbTrack
                VScroll_Scroll_Renamed(Index, eventArgs.NewValue)
            Case ScrollEventType.EndScroll
                VScroll_Change(Index, eventArgs.NewValue)
        End Select
    End Sub




    Public Function ControlList(ByVal root As Control, ByRef resultArray As ArrayList) As ArrayList

        If root.HasChildren Then
            For Each cControl As Control In root.Controls
                resultArray.Add(cControl)
                If cControl.HasChildren Then
                    If Not cControl.Name.Contains("uct") Then
                        ControlList(cControl, resultArray)
                    End If
                End If
            Next cControl
        End If
        Return resultArray
    End Function

End Class
