Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no 129. 
Imports SharedFiles

Partial Friend Class frmDetailsUW
    Inherits System.Windows.Forms.Form

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmDetailsUW"
    Private Const vbFormControlMenu As Integer = 1
    'variables declared for the Mode of the screen and the type of the screen
    Private m_iTask As gPMConstants.PMEComponentAction
    'Private m_sScreenMode As String

    Private m_sTransactionType As String = ""
    Private m_InitialReserve As Decimal
    Private m_RevisionAmount As Decimal
    Private m_cReserveAdjustment As Decimal
    Private m_PaidToDate As Decimal

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    Private m_lReturn As gPMConstants.PMEReturnCode
    Private sTitle As String = ""
    Private sMessage As String = ""
    Private m_lRevisionHasBeenAmended As Integer
    Private m_lInitialReserveHasBeenSet As Integer
    Private m_sLossCurrency As String = ""
    Private m_bAllowNegativeReserves As Boolean
    Private m_bOpenClaimNoTrans As Boolean
    Private m_sUnderwritingorAgency As String = ""

    Public WriteOnly Property LossCurrency() As String
        Set(ByVal Value As String)
            m_sLossCurrency = Value
        End Set
    End Property

    Public ReadOnly Property InitialReserve() As Decimal
        Get

            Dim result As Decimal = 0
            If Strings.Len(txtinitialreserve.Text) Then
                If IsValidCurrency(txtinitialreserve.Text) = gPMConstants.PMEReturnCode.PMTrue Then
                    result = CDec(txtinitialreserve.Text)
                End If
            End If
            Return result
        End Get
    End Property

    Public WriteOnly Property UnderwritingorAgency() As String
        Set(ByVal Value As String)
            m_sUnderwritingorAgency = Value
        End Set
    End Property


    Public ReadOnly Property InitialReserveHasBeenSet() As Integer
        Get
            Return m_lInitialReserveHasBeenSet
        End Get
    End Property

    Public ReadOnly Property RevisionHasBeenAmended() As Integer
        Get
            Return m_lRevisionHasBeenAmended
        End Get
    End Property


    Public ReadOnly Property Revision() As Decimal
        Get

            Dim result As Decimal = 0
            If Strings.Len(txtThisRevision.Text) Then
                If IsValidCurrency(txtThisRevision.Text) = gPMConstants.PMEReturnCode.PMTrue Then
                    result = CDec(txtThisRevision.Text)
                End If
            End If
            Return result
        End Get
    End Property

    ' Public Let and Get Functions

    'TN20010409 Start
    'Public Property Let TransactionType(sTransactionType As String)
    '    m_sTransactionType$ = sTransactionType
    'End Property
    'TN20010409 End

    'Public Property Get ScreenMode() As String
    '    ScreenMode = m_sScreenMode
    'End Property

    'Public Property Let ScreenMode(sScreenMode As String)
    '    m_sScreenMode = sScreenMode
    'End Property

    'Public Property Let Task(iTask As Integer)
    '
    '    m_iTask% = iTask%
    '
    'End Property
    Public ReadOnly Property Task() As Integer
        Get

            Return m_iTask

        End Get
    End Property

    Public WriteOnly Property AllowNegativeReserve() As Boolean
        Set(ByVal Value As Boolean)
            m_bAllowNegativeReserves = Value
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

    Public Function Initialise(ByVal sTransactionType As String, ByVal iTask As Integer, ByVal sRiskType As String, ByVal iInitialReserve As Decimal, ByVal iRevisionAmount As Decimal, ByVal iPaidToDate As Decimal, Optional ByVal iThisRevisionAmount As Decimal = 0) As Integer

        Try

            m_sTransactionType = sTransactionType
            txtinitialreserve.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(iInitialReserve))

            If m_bOpenClaimNoTrans Then
                txtrevisedreserve.Text = "0.00"
                txtThisRevision.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(iThisRevisionAmount))
            Else
                txtrevisedreserve.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(iRevisionAmount))
                txtThisRevision.Text = "0.00"
            End If

            Combo1.Text = sRiskType
            m_lRevisionHasBeenAmended = gPMConstants.PMEReturnCode.PMFalse
            m_lInitialReserveHasBeenSet = gPMConstants.PMEReturnCode.PMFalse
            m_PaidToDate = iPaidToDate

        Catch


            Return gPMConstants.PMEReturnCode.PMFalse
        End Try


    End Function

    ' ***************************************************************** '
    ' Name: cmdCancel_Click
    '
    ' Description: Unload the form when the Cancel Button is clicked
    '
    ' ***************************************************************** '
    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        m_lRevisionHasBeenAmended = False
        m_lInitialReserveHasBeenSet = False
        Me.Hide()
    End Sub

    ' ***************************************************************** '
    ' Name: cmdOk_Click
    '
    ' Description: Change the data in the List View when the Ok button is
    '              clicked and unload the form
    '
    ' ***************************************************************** '
    Private Sub cmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click

        Dim cchk As Decimal

        Try

            iPMFunc.ForceLostFocus(cmdOk)

            Application.DoEvents()

            'Start Renuka PN 61408
            ' Check mandatory controls have been entered into.
            m_lReturn = m_oFormFields.CheckMandatoryControls()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If
            'End Renuka PN 61408


            'TN20010409 Start
            If txtinitialreserve.Text = "" Then
                txtinitialreserve.Text = CStr(0)
            End If

            If m_bAllowNegativeReserves <> True Then
                If txtinitialreserve.Text < 0 Then
                    MsgBox("Initial Reserve cannot be Negative!", vbCritical, "Reserve Details")
                    SharedFiles.iPMFunc.SelectText(txtinitialreserve)
                    txtinitialreserve.Focus()
                    Exit Sub
                End If
                If txtinitialreserve.Enabled = False And _
                    (CDec(txtThisRevision.Text) + CDec(txtinitialreserve.Text) + CDec(txtrevisedreserve.Text)) < 0 Then
                    MsgBox("Total reserve cannot be Negative!", vbCritical, "Reserve Details")
                    SharedFiles.iPMFunc.SelectText(txtThisRevision)
                    txtThisRevision.Focus()
                    Exit Sub

                End If
            End If

            If txtrevisedreserve.Text = "" Then
                txtrevisedreserve.Text = CStr(0)
            End If

            If txtThisPayment.Text = "" Then
                txtThisPayment.Text = CStr(0)
            End If

            If txtThisRevision.Text = "" Then
                txtThisRevision.Text = CStr(0)
            End If

            'Start Renuka PN 61408
            '    cchk = CCur(txtinitialreserve.Text)
            '    cchk = CCur(txtrevisedreserve.Text)
            '    cchk = CCur(txtThisPayment.Text)
            '    cchk = CCur(txtThisRevision.Text)

            cchk = gPMFunctions.ToSafeCurrency(txtinitialreserve.Text)
            cchk = gPMFunctions.ToSafeCurrency(txtrevisedreserve.Text)
            cchk = gPMFunctions.ToSafeCurrency(txtThisPayment.Text)
            cchk = gPMFunctions.ToSafeCurrency(txtThisRevision.Text)
            'End Renuka PN 61408

            'TN20010409 End

            'Start Renuka PN 61408
            ' Check mandatory controls have been entered into.
            '    m_lReturn = m_oFormFields.CheckMandatoryControls()
            '
            '    ' Check for errors
            '    If m_lReturn <> PMTrue Then
            '      Exit Sub
            '    End If
            'End Renuka PN 61408

            '    With frmInterface
            '        Select Case ScreenMode
            '        Case "reserve"
            '            With .lstviewReserve
            '                If txtinitialreserve.Enabled Then
            '                    'initial reserve - reserve tab
            '                    .ListItems(.SelectedItem.Index).SubItems(1) = FormatField(PMFormatCurrency, txtinitialreserve.Text)
            '                Else
            '                    'TN20010405 Start
            '                    'this revision - reserve tab
            '                    .ListItems(.SelectedItem.Index).SubItems(3) = FormatField(PMFormatCurrency, txtThisRevision.Text)
            '                    '.ListItems(.SelectedItem.Index).SubItems(2) = FormatField(PMFormatCurrency, txtrevisedreserve.Text)
            '                    'TN20010405 End
            '                End If

            '            End With

            '            With .lstviewPayment
            '                If txtinitialreserve.Enabled Then
            '                    .ListItems(frmInterface.lstviewReserve.SelectedItem.Index).SubItems(1) = FormatField(PMFormatCurrency, txtinitialreserve.Text)
            '                End If
            '            End With
            '        End Select

            'TN20010405 Start - put in comments and repositioning listview columns
            '        If ScreenMode = "reserve" Then

            'incurred = initial reserve + revise reserve + this revision (reserve tab)
            '            .lstviewReserve.ListItems(.lstviewReserve.SelectedItem.Index).SubItems(5) = FormatField(PMFormatCurrency, CCur(.lstviewReserve.ListItems(.lstviewReserve.SelectedItem.Index).SubItems(1)) _
            ''                                                                                      + CCur(.lstviewReserve.ListItems(.lstviewReserve.SelectedItem.Index).SubItems(2)) _
            ''                                                                                      + CCur(.lstviewReserve.ListItems(.lstviewReserve.SelectedItem.Index).SubItems(3)))
            '            'incurred = initial reserve + revise reserve + this revision (payment tab)
            '            .lstviewPayment.ListItems(.lstviewReserve.SelectedItem.Index).SubItems(6) = FormatField(PMFormatCurrency, CCur(.lstviewReserve.ListItems(.lstviewReserve.SelectedItem.Index).SubItems(1)) _
            ''                                                                                      + CCur(.lstviewReserve.ListItems(.lstviewReserve.SelectedItem.Index).SubItems(2)) _
            ''                                                                                      + CCur(.lstviewReserve.ListItems(.lstviewReserve.SelectedItem.Index).SubItems(3)))
            'current reserve - reseve tab
            'current reserve = initial reserve + revise reserve + this revision - this payment - paid to date
            '            .lstviewReserve.ListItems(frmInterface.lstviewReserve.SelectedItem.Index).SubItems(4) = _
            ''                                FormatField(PMFormatCurrency, CCur(frmInterface.lstviewReserve.ListItems(frmInterface.lstviewReserve.SelectedItem.Index).SubItems(1)) _
            ''                                + CCur(frmInterface.lstviewReserve.ListItems(frmInterface.lstviewReserve.SelectedItem.Index).SubItems(2)) _
            ''                                + CCur(txtThisRevision.Text) _
            ''                                - CCur(frmInterface.lstviewPayment.ListItems(frmInterface.lstviewReserve.SelectedItem.Index).SubItems(4)) _
            ''                                - CCur(frmInterface.lstviewPayment.ListItems(frmInterface.lstviewReserve.SelectedItem.Index).SubItems(3)))

            '            'current reserve - payment tab
            '            'current reserve = initial reserve + revise reserve + this revision - this payment - paid to date
            '            .lstviewPayment.ListItems(frmInterface.lstviewReserve.SelectedItem.Index).SubItems(5) = _
            ''                                FormatField(PMFormatCurrency, CCur(frmInterface.lstviewReserve.ListItems(frmInterface.lstviewReserve.SelectedItem.Index).SubItems(1)) _
            ''                                + CCur(frmInterface.lstviewReserve.ListItems(frmInterface.lstviewReserve.SelectedItem.Index).SubItems(2)) _
            ''                                + CCur(txtThisRevision.Text) _
            ''                                - CCur(frmInterface.lstviewPayment.ListItems(frmInterface.lstviewReserve.SelectedItem.Index).SubItems(4)) _
            ''                                - CCur(frmInterface.lstviewPayment.ListItems(frmInterface.lstviewReserve.SelectedItem.Index).SubItems(3)))


            '        m_lReturn = frmInterface.GetTotalValuesUW()
            '
            '        If m_lReturn <> PMTrue Then
            '            GoTo Err_cmdOK
            '        End If
            '
            '    End With

            Me.Hide()

        Catch excep As System.Exception


            ' Log Error.
            Select Case Information.Err().Number
                Case 6 ' overflow
                    ' Get description from the resource file.
                    '        sTitle$ = iPMFunc.GetResData( _
                    ''            iLangID:=g_iLanguageID%, _
                    ''            lID:=ACInvalidDataTitle, _
                    ''            iDataType:=PMResString)
                    '
                    '        sMessage$ = iPMFunc.GetResData( _
                    ''            iLangID:=g_iLanguageID%, _
                    ''            lID:=ACInvalidIntegerData, _
                    ''            iDataType:=PMResString)

                    ' Display message.
                    MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                    Exit Sub

                Case Else
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process OK button of frmDetailsUW", vApp:=ACApp, vClass:=ACClass, vMethod:="cmd_OK Click of frmDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            End Select

            Exit Sub



        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: Form_load
    '
    ' Description: Loads the form with the minimum set of information required for the
    '              details screen depending on the modes that are set for the screen
    '
    ' Hist : 04/09/2001 Tinny - renumbering column, add revision amount to payment
    ' ***************************************************************** '


    Private Sub frmDetailsUW_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        Try

            m_cReserveAdjustment = 0.0#

            '    Combo1.Text = m_sRisktype
            Combo1.Enabled = False

            ' Create an instance of the form control object.
            m_oFormFields = New iPMFormControl.FormFields()

            ' Set language
            m_oFormFields.LanguageID = g_iLanguageID

            Me.Text = "Reserve Details Screen"
            'fraReserveDetails.Caption = "Reserve Details"

            '        txtinitialreserve.Text = frmInterface.lstviewReserve.ListItems(frmInterface.lstviewReserve.SelectedItem.Index).SubItems(1)
            'If frmInterface.lstviewReserve.ListItems(frmInterface.lstviewReserve.SelectedItem.Index).SubItems(1) = "0.00" Then

            txtLossCurrency.Visible = True
            lblLossCurrency.Visible = True
            txtLossCurrency.Enabled = False
            txtLossCurrency.ReadOnly = True
            txtLossCurrency.Text = m_sLossCurrency

            'TN20010409 Start  If m_iTask = PMAdd Then
            If m_sTransactionType = "C_CO" And Not m_bOpenClaimNoTrans Then

                lblInitialReserve.Visible = True
                txtinitialreserve.Visible = True

                lblRevisedReserve.Visible = False
                txtrevisedreserve.Visible = False

                lblThisPayment.Visible = False
                txtThisPayment.Visible = False

                ' Pass control and required settings to FormControl
                m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtinitialreserve, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

                'Error checking
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New Exception()
                End If

                txtLossCurrency.Top = txtrevisedreserve.Top
                lblLossCurrency.Top = lblRevisedReserve.Top

                fraReserveDetails.Height = VB6.TwipsToPixelsY(1650)
                SSTab1.Height = VB6.TwipsToPixelsY(2085)
                cmdOk.Top = VB6.TwipsToPixelsY(2290)
                cmdCancel.Top = VB6.TwipsToPixelsY(2290)
                Me.Height = VB6.TwipsToPixelsY(3330)

                iPMFunc.SelectText(txtinitialreserve)


                'TN20010409 Start ElseIf m_iTask = PMEdit Or m_iTask = PMView Then
            ElseIf m_sTransactionType = "C_CR" Or m_iTask = gPMConstants.PMEComponentAction.PMView Or m_bOpenClaimNoTrans Then

                lblInitialReserve.Visible = True
                txtinitialreserve.Visible = True

                If Not m_bOpenClaimNoTrans Then
                    txtinitialreserve.Enabled = False
                    txtThisRevision.Enabled = True
                Else
                    txtinitialreserve.Enabled = True
                    txtThisRevision.Enabled = False
                End If

                lblThisRevision.Visible = True
                txtThisRevision.Visible = True

                txtThisPayment.Visible = False
                lblThisPayment.Visible = False

                lblRevisedReserve.Visible = True
                txtrevisedreserve.Visible = True

                'TN20010405 Start
                txtrevisedreserve.Enabled = False

                txtLossCurrency.Top = txtThisRevision.Top
                lblLossCurrency.Top = lblThisRevision.Top

                txtThisRevision.Top = txtThisPayment.Top
                lblThisRevision.Top = lblThisPayment.Top

                txtThisRevision.Text = CStr(0)

                If Not m_bOpenClaimNoTrans Then
                    ' Pass control and required settings to FormControl
                    m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtThisRevision, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

                    'Error checking
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New Exception()
                    End If
                End If

                fraReserveDetails.Height = VB6.TwipsToPixelsY(2505)
                SSTab1.Height = VB6.TwipsToPixelsY(2925)
                cmdOk.Top = VB6.TwipsToPixelsY(3150)
                cmdCancel.Top = VB6.TwipsToPixelsY(3150)
                Me.Height = VB6.TwipsToPixelsY(4005)

                iPMFunc.SelectText(txtThisRevision)

                'TN20010405 End
            End If

        Catch excep As System.Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try
    End Sub

    Private Sub frmDetailsUW_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing

        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        If UnloadMode = vbFormControlMenu Then
            ' mimic user pressing cancel button
            m_lRevisionHasBeenAmended = False
            m_lInitialReserveHasBeenSet = False
            ' don't kill the form here, just hide it
            ' so control returns to the calling form
            Cancel = True
            Me.Hide()
        End If

        eventArgs.Cancel = Cancel <> 0
    End Sub

    Private isInitializingComponent As Boolean
    Private Sub txtinitialreserve_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtinitialreserve.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_lInitialReserveHasBeenSet = gPMConstants.PMEReturnCode.PMTrue
    End Sub

    ' ***************************************************************** '
    ' Name: txtinitialreserve_LostFocus
    '
    ' Description: Change the display format for the value in the text box
    '
    ' ***************************************************************** '

    Private Sub txtinitialreserve_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtinitialreserve.Leave

        Try

            If txtinitialreserve.Text.Trim() <> "" Then
                txtinitialreserve.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, txtinitialreserve.Text)
                '    If CCur(txtinitialreserve.Text) < 0 Then
                '        txtinitialreserve.Text = ""
                '        txtinitialreserve.SetFocus
                '        GoTo Err_txtInitialReserve
                '
                '        Exit Sub
                '    End If
            End If

        Catch



            ' Get description from the resource file.

            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInvalidDataTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInvalidIntegerData, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Display message.
            MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try


    End Sub



    Private Sub txtThisRevision_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtThisRevision.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_lRevisionHasBeenAmended = gPMConstants.PMEReturnCode.PMTrue
    End Sub

    Private Sub txtThisRevision_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtThisRevision.Leave

        'do we have data
        If txtThisRevision.Text = "" Then
            Exit Sub
        End If
        txtThisRevision.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, txtThisRevision.Text)

        'Start Renuka PN 61408
        'cAmount = CCur(txtrevisedreserve.Text) + CCur(txtThisRevision.Text) _
        ''   + CCur(txtinitialreserve.Text) - CCur(m_PaidToDate)
        Dim cAmount As Decimal = gPMFunctions.ToSafeCurrency(txtrevisedreserve.Text) + gPMFunctions.ToSafeCurrency(txtThisRevision.Text) + gPMFunctions.ToSafeCurrency(txtinitialreserve.Text) - gPMFunctions.ToSafeCurrency(CStr(m_PaidToDate))
        'End Renuka PN 61408

        'check to see if this revision will make current reserve negative
        
            If m_bAllowNegativeReserves <> True And m_bOpenClaimNoTrans = False Then
            If cAmount < 0 Then
                MsgBox("Current Reserve minus This Revision cannot be less than zero!", vbCritical, ACApp)
                SharedFiles.iPMFunc.SelectText(txtThisRevision)
                txtThisRevision.Focus()
                Exit Sub
            End If
        End If
    End Sub

    ' ******************************************************************* '
    ' Name:         EnableDisableControls
    '
    ' Description:  Enable/Disable controls depending on
    '               whether initial reserve is zero or not.
    ' ******************************************************************* '

    'Private Function EnableDisableControls(ByVal v_iInitialReserve As Integer) As Integer
    'Dim result As Integer = 0
    'Const kMethodName As String = "EnableDisableControls"
    '
    'On Error GoTo Catch_Renamed
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'If v_iInitialReserve = 0 Then
    'lblInitialReserve.Visible = True
    'txtinitialreserve.Visible = True
    'txtinitialreserve.Enabled = True
    '
    'lblRevisedReserve.Visible = True
    'txtrevisedreserve.Visible = True
    '
    'txtThisPayment.Visible = False
    'lblThisPayment.Visible = False
    '
    'lblThisRevision.Visible = True
    'txtThisRevision.Visible = True
    'txtThisRevision.Enabled = False
    '
    'txtrevisedreserve.Enabled = False
    '
    'lblInitialReserve.Font = VB6.FontChangeBold(lblInitialReserve.Font, True)
    'lblThisRevision.Font = VB6.FontChangeBold(lblThisRevision.Font, False)
    '
    'txtinitialreserve.Text = "0.00"
    '
    'fraReserveDetails.Height = VB6.TwipsToPixelsY(2505)
    'SSTab1.Height = VB6.TwipsToPixelsY(2925)
    'cmdOk.Top = VB6.TwipsToPixelsY(3150)
    'cmdCancel.Top = VB6.TwipsToPixelsY(3150)
    'Me.Height = VB6.TwipsToPixelsY(4005)
    '
    'iPMFunc.SelectText(txtinitialreserve)
    '
    'Else
    'lblInitialReserve.Visible = True
    'txtinitialreserve.Visible = True
    'txtinitialreserve.Enabled = False
    '
    'lblRevisedReserve.Visible = True
    'txtrevisedreserve.Visible = True
    '
    'txtThisPayment.Visible = False
    'lblThisPayment.Visible = False
    '
    'lblThisRevision.Visible = True
    'txtThisRevision.Visible = True
    'txtThisRevision.Enabled = True
    '
    'txtrevisedreserve.Enabled = False
    '
    'lblInitialReserve.Font = VB6.FontChangeBold(lblInitialReserve.Font, False)
    'lblThisRevision.Font = VB6.FontChangeBold(lblThisRevision.Font, True)
    '
    'txtThisRevision.Text = "0.00"
    '
    'fraReserveDetails.Height = VB6.TwipsToPixelsY(2505)
    'SSTab1.Height = VB6.TwipsToPixelsY(2925)
    'cmdOk.Top = VB6.TwipsToPixelsY(3150)
    'cmdCancel.Top = VB6.TwipsToPixelsY(3150)
    'Me.Height = VB6.TwipsToPixelsY(4005)
    '
    'iPMFunc.SelectText(txtThisRevision)
    '
    'End If
    '
    'GoTo Finally_Renamed
    '
    'Catch_Renamed: '
    '
    ' DO Not Call any functions before here or the error will be lost
    'iPMFunc.LogError(v_sUsername:=g_oObjectManager.UserName, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)
    '
    ' If you want to rollback a transaction or something, do it here
    '
    'Finally_Renamed: '
    '
    ' Do any tidy up, e.g. Set x = Nothing here
    'Return result
    '
    ' This is for debugging only
    'Resume 
    '
    'Return result
    'End Function
End Class
