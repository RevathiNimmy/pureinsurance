Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports System.Windows.Forms

Imports SharedFiles
Partial Friend Class frmUInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmFeeChargesEX
    '
    ' Date: 19/05/2004
    '
    ' Description: Interface for Fee Charge entry
    '
    ' Edit History:
    '             VB 12/04/2005 PN19992: IF condition added for invalid percentage.
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmUInterface"

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_dtEffectiveDate As Date
    Private m_sStepStatus As String = ""
    Private m_lPartyCnt As Integer
    Private m_dFeePercentage As Double
    Private m_cFeeAmount As Decimal
    Private m_lTransactionTypeID As Integer
    Private m_lProductType As Integer
    Private m_sProductTypeDesc As String = ""
    Private m_sTransactionType As String = ""
    Private m_nIsTaxable As Integer
    Private m_lFeeAmountID As Integer
    Private m_lRMStepEdit As Integer

    'persist currency values
    Private m_lCurrencyID As Integer
    Private m_sCurrencyName As String = ""

    ' Declare an instance of the Business object.

    'TODO: iteration3,declare Private m_oBusiness As Object
    'Private m_oBusiness As bSIRPartyFee.UBusiness
    Private m_oBusiness As Object
    'Private m_oBusiness As bSIRPFee.Business
    Private m_oUBusiness As Object

    Private m_oGeneral As General

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Variables to store the lookup values/details.
    Private m_vLookupValues(,) As Object
    Private m_vLookupDetails(,) As Object

    'variables to check for mutiple entries if the same
    Private m_vFeeDetails(,) As Object
    Private m_iFeeElements As Integer
    Private m_vCheckValues(,) As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    ' AMB 13-Oct-03: 1.8.6 Accident Management development
    Private m_vExtraSchemes As Object
    Private m_lExtraSchemeID As Integer
    Private m_sExtraSchemeDesc As String = ""

    Private m_lIsAmmended As Integer

    Public Property IsAmmended() As Integer
        Get
            Return m_lIsAmmended
        End Get
        Set(ByVal Value As Integer)
            m_lIsAmmended = Value
        End Set
    End Property
    Public Property FeeDetails() As Object
        Get
            Return VB6.CopyArray(m_vFeeDetails)
        End Get
        Set(ByVal Value As Object)
            m_vFeeDetails = Value
        End Set
    End Property
    Public Property RMStepEdit() As Integer
        Get
            Return m_lRMStepEdit
        End Get
        Set(ByVal Value As Integer)
            m_lRMStepEdit = Value
        End Set
    End Property
    Public Property FeeAmountID() As Integer
        Get
            Return m_lFeeAmountID
        End Get
        Set(ByVal Value As Integer)
            m_lFeeAmountID = Value
        End Set
    End Property

    Public Property CurrencyID() As Integer
        Get
            Return m_lCurrencyID
        End Get
        Set(ByVal Value As Integer)
            m_lCurrencyID = Value
        End Set
    End Property

    Public Property CurrencyName() As String
        Get
            Return m_sCurrencyName
        End Get
        Set(ByVal Value As String)
            m_sCurrencyName = Value
        End Set
    End Property

    Public Property TransactionType() As Integer
        Get
            Return m_lTransactionTypeID
        End Get
        Set(ByVal Value As Integer)
            m_lTransactionTypeID = Value
        End Set
    End Property
    Public Property TransactionTypeDesc() As String
        Get
            Return m_sTransactionType
        End Get
        Set(ByVal Value As String)
            m_sTransactionType = Value
        End Set
    End Property
    Public Property ProductType() As Integer
        Get
            Return m_lProductType
        End Get
        Set(ByVal Value As Integer)
            m_lProductType = Value
        End Set
    End Property
    Public Property ProductTypeDesc() As String
        Get
            Return m_sProductTypeDesc
        End Get
        Set(ByVal Value As String)
            m_sProductTypeDesc = Value
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
    Public Property PartyCnt() As Integer
        Get
            Return m_lPartyCnt
        End Get
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property
    Public Property FeeAmount() As Decimal
        Get
            Return m_cFeeAmount
        End Get
        Set(ByVal Value As Decimal)
            m_cFeeAmount = Value
        End Set
    End Property
    Public Property FeePercentage() As Double
        Get
            Return m_dFeePercentage
        End Get
        Set(ByVal Value As Double)
            m_dFeePercentage = Value
        End Set
    End Property
    Public Property IsTaxable() As Integer
        Get
            Return m_nIsTaxable
        End Get
        Set(ByVal Value As Integer)
            m_nIsTaxable = Value
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

    Public Sub RMControlFormat()

        ' Check if the form has been loaded from the road map step

        If m_lRMStepEdit = 1 Then
            'form has been loaded from road map
            cboProduct.Enabled = False
            cboTransType.Enabled = False

            'if this is not a new add task
            If Me.Task <> gPMConstants.PMEComponentAction.PMAdd Then
                txtEffectiveDate.Enabled = False
                chkIsTaxable.Enabled = False
                'JT 15-10-2004
                'PN 15603
                'Desc-As per Tech. Specs. the currency combo shud be disable while editing
                cboCurrency.Enabled = False
            End If

        End If

    End Sub

    Private Sub MapTransactionTypes()

        ' This function maps the transaction types from the DB to the combo box

        Select Case m_lTransactionTypeID
            Case 4
                cboTransType.SelectedIndex = ACTransTypeNewBusiness
            Case 7
                cboTransType.SelectedIndex = ACTransTypeCancel
            Case 9
                cboTransType.SelectedIndex = ACTransTypeMTA
            Case 10
                cboTransType.SelectedIndex = ACTransTypeRenewal
            Case 20
                cboTransType.SelectedIndex = ACTransTypeReInstatement
            Case Else
                cboTransType.SelectedIndex = ACTransTypeBlank
        End Select

    End Sub

    Private Sub MapTransactionTypesToDB()

        ' This function maps the transaction types from the DB to the
        ' combo box (after the lookup function is called index's do not
        ' match

        Select Case cboTransType.SelectedIndex
            Case ACTransTypeCancel
                m_lTransactionTypeID = 7
                m_sTransactionType = ACTransTypeCaptionCancel
            Case ACTransTypeMTA
                m_lTransactionTypeID = 9
                m_sTransactionType = ACTransTypeCaptionMTA
            Case ACTransTypeNewBusiness
                m_lTransactionTypeID = 4
                m_sTransactionType = ACTransTypeCaptionNewBusiness
            Case ACTransTypeRenewal
                m_lTransactionTypeID = 10
                m_sTransactionType = ACTransTypeCaptionRenewal
            Case ACTransTypeReInstatement
                m_lTransactionTypeID = 20
                m_sTransactionType = ACTransTypeCaptionReInstatement
            Case Else
                m_lTransactionTypeID = 0
                m_sTransactionType = ""
        End Select

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            m_lReturn = CType(m_oGeneral.UInitialise(frmUInterface:=Me, oBusiness:=m_oBusiness), gPMConstants.PMEReturnCode)

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = CType(m_oGeneral.UProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Dim vKey As Integer

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Check mandatory controls have been entered into.
            m_lReturn = m_oFormFields.CheckMandatoryControls()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            m_lReturn = CType(ValidateForm(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' Create instance of interface
            m_lReturn = CType(m_oGeneral.UInitialise(frmUInterface:=Me, oBusiness:=m_oBusiness), gPMConstants.PMEReturnCode)

            ' If this form has been loaded from the road map check
            ' that the same fee is not being re-added
            If (m_lRMStepEdit = 1) And (m_iTask = gPMConstants.PMEComponentAction.PMAdd) Then

                m_iFeeElements = m_vFeeDetails.GetUpperBound(1)

                For i As Integer = m_vFeeDetails.GetLowerBound(1) To m_vFeeDetails.GetUpperBound(1)

                    If CInt(m_vFeeDetails(7, i)) = CInt(txtPercentage.Text.Substring(0, 4)) Then
                        If CInt(m_vFeeDetails(8, i)) = CInt(txtAmount.Text) Then
                            'user is trying to add the same fee twice
                            MessageBox.Show("You cannot add the same Fee twice", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            Exit Sub
                        End If
                    End If
                Next i
            End If

            ' Process the next set of actions depending upon the interface task etc.
            m_lReturn = CType(m_oGeneral.UProcessCommand(vKey:=vKey), gPMConstants.PMEReturnCode)


            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try

    End Sub

    Private Sub Form_Initialize_Renamed()

        Dim sMessage, sTitle As String

        ' Forms initialise event.
        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRPartyFee.UBusiness", vInstanceManager:=gPMConstants.PMGetViaClientManager)
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

            '    Set m_oUBusiness = New iPMBPartyFee.UInterface
            '
            '    ' Create an instance of the general interface object.
            '    Set m_oGeneral = New iPMBPartyFee.General

            'Call the initialise method passing this interface
            'and the business object as parameters.
            m_lReturn = CType(m_oGeneral.UInitialise(frmUInterface:=Me, oBusiness:=m_oBusiness), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Create an instance of the form control object.
            m_oFormFields = New iPMFormControl.FormFields()

            ' Set language
            m_oFormFields.LanguageID = g_iLanguageID

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


    Private Sub frmUInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Forms load event.

        Try

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

            m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")

                Exit Sub
            End If

            ' Validate fields using Forms Control
            m_lReturn = CType(SetFieldValidation(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Set the interface default values.
            m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            m_oGeneral = New iPMBPartyFee.General()

            m_lReturn = CType(DisplayLookupDetails(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Check the task.
            If Me.Task = gPMConstants.PMEComponentAction.PMEdit Or Me.Task = gPMConstants.PMEComponentAction.PMView Then
                ' Get the interface details from the
                ' business object.
                m_lReturn = CType(GetBusiness(), gPMConstants.PMEReturnCode)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to get the details.
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If

                ' Assign the details from the business object
                ' to the interface.
                m_lReturn = CType(BusinessToInterface(), gPMConstants.PMEReturnCode)

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to assign the details.
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Exit Sub
                End If
            End If

            ' Check the task.
            If Me.Task = gPMConstants.PMEComponentAction.PMView Then
                ' Disable the interface to only allow viewing.
                m_lReturn = CType(DisableForm(lDisabled:=True), gPMConstants.PMEReturnCode)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to assign the details.
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                End If
            End If

            If Me.Task = gPMConstants.PMEComponentAction.PMAdd Then
                ' Set currency
                cboCurrency.CurrencyId = m_lCurrencyID
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Now re-map the tranaction types to new index list
            MapTransactionTypes()

            ' Now disable controls for road map step
            RMControlFormat()

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            iPMFunc.CenterForm(Me)
            'JT     14-10-2004
            'PN     15340
            cboCurrency.RestrictTo = UserControls.CurrencyLookup.RestrictToCurrency.actCompanyCurrencies
            ' Display all language specific captions.
            ' m_lReturn& = DisplayCaptions()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(SetFirstLastControls(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayCaptions
    '
    ' Description: Display all language specific captions.
    '
    ' ***************************************************************** '

    'Private Function DisplayCaptions() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Display all language specific captions.
    '

    'Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
    '
    ' Check for an error.
    'If Me.Text = "" Then
    ' Failed to get data from the resource file.
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() &  _
    '                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")
    '
    'Return result
    'End If
    '

    'cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
    '

    'cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
    '

    'cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
    '
    '
    ' {* USER DEFINED CODE (Begin) *}
    '
    ' ************************************************************
    ' Enter your code here to display all language specific
    ' captions.
    ' The GetResData function will allow you to do this.
    ''
    ' Example:-
    ''
    '    lblDesc.Caption = iPMFunc.GetResData( _
    ''        iLangID:=g_iLanguageID%, _
    ''        lID:=ACDesc, _
    ''        iDataType:=PMResString)
    ''
    ' NOTE: Replace this section with your new code.
    ' ************************************************************
    '
    '    lblType.Caption = iPMFunc.GetResData( _
    ''        iLangID:=g_iLanguageID%, _
    ''        lID:=ACCaptionType, _
    ''        iDataType:=PMResString)
    '

    'lblPercentage.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionPercentage, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
    '

    'lblAmount.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionAmount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
    '

    'lblProduct.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionProduct, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
    '

    'lblEffectiveDate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionEffectiveDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
    '

    'lblIsTaxable.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionIsTaxable, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
    '

    'lblTransType.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionTransactionType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
    '

    'lblCurrency.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionCurrency, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMerror
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function
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

            m_ctlTabFirstLast(ACControlStart, 0) = cboProduct
            'PSA 22/06/00
            'Set m_ctlTabFirstLast(ACControlEnd, 0) = txtCommissionAmount
            m_ctlTabFirstLast(ACControlEnd, 0) = chkIsTaxable
            'PSA 22/06/00


            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMerror

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFirstLastControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayLookupDetails
    '
    ' Description: Displays all of the lookup details using the lookup
    '              values/details.
    '
    ' ***************************************************************** '
    Public Function DisplayLookupDetails() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.

            m_lReturn = CType(GetLookupValues(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' Get all of the lookup details.

            ' {* USER DEFINED CODE (Begin) *}
            'MK 991007
            m_lReturn = CType(GetLookupDetails(sLookupTable:="Product", ctlLookup:=Me.cboProduct), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Populate the transaction type
            cboTransType.Items.Clear()
            cboTransType.Items.Add("")
            cboTransType.Items.Add(ACTransTypeCaptionCancel)
            cboTransType.Items.Add(ACTransTypeCaptionMTA)
            cboTransType.Items.Add(ACTransTypeCaptionNewBusiness)
            cboTransType.Items.Add(ACTransTypeCaptionRenewal)
            cboTransType.Items.Add(ACTransTypeCaptionReInstatement)

            Return result

        Catch excep As System.Exception



            ' Error Section
            result = gPMConstants.PMEReturnCode.PMerror

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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



            result = gPMConstants.PMEReturnCode.PMerror

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

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' Assign the details from the business object to the data storage.
            m_lReturn = CType(BusinessToData(), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Assign the details to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to assign the all of the interface
            ' details from the business object, using the FormatField
            ' function for any type conversion.
            '
            ' Example:-
            '
            '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtName, vControlValue:=m_sName$)
            '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=optChoice, vControlValue:=m_iDChoice%)
            '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtDate, vControlValue:=m_dtDDate)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************


            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtPercentage, vControlValue:=m_dFeePercentage)
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtAmount, vControlValue:=m_cFeeAmount)
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtEffectiveDate, vControlValue:=m_dtEffectiveDate)
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=chkIsTaxable, vControlValue:=m_nIsTaxable)

            For lLoop As Integer = 0 To cboProduct.Items.Count - 1
                If m_lProductType = VB6.GetItemData(cboProduct, lLoop) Then
                    cboProduct.SelectedIndex = lLoop
                    Exit For
                End If
            Next

            ' Currency
            'JT  14-10-2004
            'PN-15340
            'Passing the Correct SourceId so that Currency list could be updated in combo
            cboCurrency.CompanyId = g_iSourceID
            cboCurrency.RefreshList()

            cboCurrency.CurrencyId = m_lCurrencyID

            ' Map types to DB values
            MapTransactionTypes()

            If m_nIsTaxable = 1 Then
                Me.chkIsTaxable.CheckState = CheckState.Checked
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMerror

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: BusinessToData
    '
    ' Description: Updates the data storage from the business object.
    '
    ' ***************************************************************** '
    Private Function BusinessToData() As Integer

        Dim result As Integer = 0
        Try


            ' Assign the details to the data storage.

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMerror

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields.
    '
    ' ***************************************************************** '
    Public Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to assign all of the controls to
            ' PMFormControl
            '
            ' Example:-
            '
            '        ' Pass control and required settings to FormControl
            '        m_lReturn = m_oFormFields.AddNewFormField( _
            ''                       ctlControl:=<Control Name>, _
            ''                       lFieldType:=<PM field type>, _
            ''                       lFormat:=<PM format string>, _
            ''                       lMandatory:=<PMMandatory or PMNonMandatory)
            '
            '        'Error checking
            '        If m_lReturn <> PMTrue Then
            '          SetFieldValidation = PMFalse
            '          Exit Function
            '        End If
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboProduct, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboTransType, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPercentage, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatPercent, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAmount, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtEffectiveDate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateShort, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=chkIsTaxable, lFieldType:=gPMConstants.PMEDataType.PMInteger, lFormat:=gPMConstants.PMEFormatStyle.PMFormatInteger, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMerror

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisableForm
    '
    ' Description: Sets all of the interface details to the disable
    '              state passed.
    '
    ' ***************************************************************** '
    Private Function DisableForm(ByRef lDisabled As Integer) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set all of the forms controls to the disable state.

            For Each ctlFormControl As Control In ContainerHelper.Controls(Me)
                ' Check the type of the control.
                If TypeOf ctlFormControl Is TextBox Then
                    ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
                ElseIf (TypeOf ctlFormControl Is ComboBox) Then
                    ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
                ElseIf (TypeOf ctlFormControl Is CheckBox) Then
                    ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
                ElseIf (TypeOf ctlFormControl Is UserControls.CurrencyLookup) Then
                    ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
                ElseIf (TypeOf ctlFormControl Is PMLookupControl.cboPMLookup) Then
                    ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
                End If
            Next ctlFormControl

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMerror

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the form disabling", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupValues
    '
    ' Description: Gets all of the lookup values, ready to be used by
    '              the lookup function.
    '
    ' ***************************************************************** '
    Private Function GetLookupValues() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Gets all of the lookup values.

            ' Check the task.

            Select Case (m_iTask)
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Get all of the lookup values.

                    m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Get all of the lookup values with the correct
                    ' effective date.

                    m_oBusiness.FeeAmountID = m_lFeeAmountID

                    m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)

                Case gPMConstants.PMEComponentAction.PMView
                    ' Get lookup values for viewing only.
                    '            m_oBusiness.RiskGroupID = m_lRiskGroupID
                    '            m_lReturn& = m_oBusiness.GetLookupValues( _
                    ''                iLookupType:=PMLookupSingle, _
                    ''                vTableArray:=m_vLookupValues, _
                    ''                iLanguageID:=g_iLanguageID%, _
                    ''                vResultArray:=m_vLookupDetails)
            End Select

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")

                Return result
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMerror

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupDetails
    '
    ' Description: Gets all of the lookup details using the lookup
    '              values, then assigns them to the control passed.
    '
    ' ***************************************************************** '
    'changes as Control does not support the properties of combobox
    'Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As Control) As Integer
    Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As ComboBox) As Integer

        Dim result As Integer = 0
        Dim lRow As Integer
        Dim bFoundMatch As Boolean

        ' Lookup value contants.
        Const ACValueTableName As Integer = 0
        Const ACValueID As Integer = 1
        Const ACValueStartPos As Integer = 2
        Const ACValueNumber As Integer = 3

        ' Lookup detail contants.
        Const ACDetailKey As Integer = 0
        Const ACDetailDesc As Integer = 1

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.

            bFoundMatch = False

            For lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
                ' Check for a match of the table name.
                If CStr(m_vLookupValues(ACValueTableName, lRow)).Trim() = sLookupTable.Trim() Then
                    ' Found a match
                    bFoundMatch = True
                    Exit For
                End If
            Next lRow

            ' Check if there has been a table match.
            If Not bFoundMatch Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for the table - " & sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails")

                Return result
            End If

            ' Using the lookup values, populate the control with
            ' the details from the lookup details array.
            Dim newIndex As Integer = 0
            For lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)
                ' Add the details to the control.



                newIndex = ctlLookup.Items.Add(m_vLookupDetails(ACDetailDesc, lCntr))




                ctlLookup.Items(newIndex) = CInt(m_vLookupDetails(ACDetailKey, lCntr))


                ' Check if this is the selected index.
                If CStr(m_vLookupValues(ACValueID, lRow)) <> "" Then
                    If CDbl(m_vLookupValues(ACValueID, lRow)) = CInt(m_vLookupDetails(ACDetailKey, lCntr)) Then


                        'dveloper guide no. 28
                        ctlLookup.SelectedIndex = newIndex
                    End If
                End If

            Next lCntr

            ' Set list to blank

            'dveloper guide no. 28
            ctlLookup.SelectedIndex = -1
            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMerror

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: InterfaceToBusiness
    '
    ' Description: Updates all business members from the interface
    '              details.
    '
    ' ***************************************************************** '
    Public Function InterfaceToBusiness(Optional ByRef vKey As Integer = 0) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the business object.

            ' Assign the details from the interface to the data storage.
            m_lReturn = CType(InterfaceToData(vKey:=vKey), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
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
    Private Function InterfaceToData(Optional ByRef vKey As Integer = 0) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the data storage.

            ' Product
            m_lProductType = VB6.GetItemData(cboProduct, cboProduct.SelectedIndex)
            m_sProductTypeDesc = VB6.GetItemString(cboProduct, cboProduct.SelectedIndex)

            ' Percentage

            m_dFeePercentage = CDbl(m_oFormFields.UnformatControl(txtPercentage))

            ' Amount

            m_cFeeAmount = CDec(m_oFormFields.UnformatControl(txtAmount))

            ' Is taxable

            m_nIsTaxable = CInt(m_oFormFields.UnformatControl(chkIsTaxable))

            ' Effective date

            m_dtEffectiveDate = CDate(m_oFormFields.UnformatControl(txtEffectiveDate))

            ' Currency ID
            m_lCurrencyID = cboCurrency.CurrencyId
            m_sCurrencyName = cboCurrency.CurrencyName


            m_lReturn = m_oBusiness.Validate(vPartyCnt:=PartyCnt, vProductType:=m_lProductType, vTransactionType:=m_lTransactionTypeID, vFeePercentage:=m_dFeePercentage, vFeeAmount:=m_cFeeAmount, vEffectiveDate:=DateTime.Parse(m_dtEffectiveDate).ToString("d"), vIsTaxable:=m_nIsTaxable)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Map the transaction id's and descriptions to correct DB values
            MapTransactionTypesToDB()

            ' Call business object and assign properties
            With m_oBusiness


                .PartyCnt = PartyCnt

                .ProductType = m_lProductType

                .TransactionType = m_lTransactionTypeID

                .EffectiveDate = DateTime.Parse(m_dtEffectiveDate).ToString("d")

                .FeePercentage = m_dFeePercentage

                .FeeAmount = m_cFeeAmount

                .IsTaxable = m_nIsTaxable

                .Status = m_lStatus

                .ProductTypeDesc = m_sProductTypeDesc

                .TransactionTypeDesc = m_sTransactionType

                .CurrencyID = m_lCurrencyID

                .CurrencyName = m_sCurrencyName

                .FeeAmountID = m_lFeeAmountID
            End With

            If Task = gPMConstants.PMEComponentAction.PMEdit Then
                'get current fee amount/fee percentage values from DB

                m_lReturn = m_oBusiness.CheckAmmended(vResultArray:=m_vCheckValues)

                If (Me.FeePercentage <> CInt(m_vCheckValues(0, 0))) Or (Me.FeeAmount <> CInt(m_vCheckValues(1, 0))) Then
                    'set IsAmmended property to one for DB update
                    With m_oBusiness

                        .IsAmmended = 1
                    End With

                    With m_oUBusiness
                        'now set property on Interface

                        .IsAmmended = 1
                    End With
                End If
            End If

            If Task = gPMConstants.PMEComponentAction.PMEdit And RMStepEdit <> 1 Then
                ' Now properties have been set at business layer call add function

                m_lReturn = m_oBusiness.Update()
            End If

            If Task = gPMConstants.PMEComponentAction.PMAdd Then
                If RMStepEdit = 1 Then
                    ' Fees will not be added to fee table
                    ' they will be added to policy fees table
                    Return gPMConstants.PMEReturnCode.PMTrue
                Else
                    ' Now properties have been set at business layer call add function

                    m_lReturn = m_oBusiness.DirectAdd(vKey:=vKey)
                End If
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function ValidateForm() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If cboProduct.SelectedIndex = -1 Then
                MessageBox.Show("No Product type entered", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            If cboTransType.SelectedIndex <= 0 Then
                MessageBox.Show("No Transaction type entered", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Dim dbNumericTemp2 As Double
            Dim dbNumericTemp As Double
            If (Not Double.TryParse(txtAmount.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) And (Not Double.TryParse(txtAmount.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2)) Then
                MessageBox.Show("Either a Percentage OR an Amount must be entered", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            If (txtAmount.Text = "0.00") And (txtPercentage.Text = "0.00%") Then
                MessageBox.Show("Either a Percentage OR an Amount must be entered", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Information.IsDate(txtEffectiveDate.Text) Then
                MessageBox.Show("Please ennter a valid date", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to validate the form", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub txtAmount_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAmount.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtAmount)
    End Sub

    Private Sub txtAmount_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAmount.Leave


        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtAmount)


        Dim cTemp As Decimal = CDec(m_oFormFields.UnformatControl(ctlControl:=txtAmount))

        If cTemp <> 0 Then
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtPercentage, vControlValue:=0)
        End If

    End Sub

    Private Sub txtEffectiveDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtEffectiveDate.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtEffectiveDate)
    End Sub

    Private Sub txtEffectiveDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtEffectiveDate.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtEffectiveDate)
    End Sub

    Private Sub txtPercentage_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPercentage.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtPercentage)
    End Sub

    Private Sub txtPercentage_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPercentage.Leave
        'Check for valid Percentage value
        If Conversion.Val(txtPercentage.Text) > 999.99 Or Conversion.Val(txtPercentage.Text) < 0 Then
            MessageBox.Show("Invalid percentage entered", "Invalid Percentage", MessageBoxButtons.OK, MessageBoxIcon.Error)
            txtPercentage.Text = CStr(0)
        End If
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtPercentage)

        If txtPercentage.Text.Trim() = "" Then
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtPercentage, vControlValue:=0)
        End If


        Dim cTemp As Decimal = CDec(m_oFormFields.UnformatControl(ctlControl:=txtPercentage))

        If cTemp <> 0 Then
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtAmount, vControlValue:=0)
        End If

    End Sub


    Private Sub frmUInterface_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown

        If e.Alt And e.KeyCode = Keys.D1 Then
            tabMainTab.SelectedIndex = 0
        End If
    End Sub
End Class
