Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Globalization
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Friend Class frmDetail
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmDetail
    '
    ' Date: 16th September 2000
    '
    ' Description: Detail from used for data entry.
    '
    ' Edit History:
    ' 24/10/2005 RKS Premium Override (Commission Override)
    ' ***************************************************************** '

    Private Const ACClass As String = "frmDetail"

    'Declare variables to hold the property values
    Private m_lAgentid As Integer
    Private m_lAgentTypeid As Integer
    Private m_lRiskTypeId As Integer
    Private m_lCommissionBandId As Integer

    Private m_lReturn As gPMConstants.PMEReturnCode

    Private m_bChange As Boolean


    Private m_oBusiness As bSirAgentCommission.Business

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    Public lvwAgentCommission As ListView
    Public InsuranceFileCnt As Integer
    Public ReadOnly_Renamed As Boolean
    Public SourceID As Integer
    Public CurrencyID As Integer

    Private m_iIsAmended As Integer
    Private m_cCalculatedCommissionValue As Decimal
    Private m_sOverrideReason As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    'Start (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1.1)
    Private m_iIsTaxAmended As Integer
    Private m_cCalculatedTaxValue As Decimal
    'End (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1.1)
    'Start - Renuka - (WPR64 Paralleling)
    Private m_cMaximumRateValue As Decimal
    Private m_bIsValue As Boolean

    'PN_68557 Start
    Private m_bisGetCursor As Boolean
    Private m_prevValue As Decimal
    'PN_68557 End
    Private m_dInitialTaxAmount As Decimal
    Private m_dInitialCommRate As Decimal
    Private m_nTaxGroupItemId As Integer
    Private m_dInitialCommValue As Decimal
    Private m_bTaxValueKeyPressed As Boolean = False
    Private m_bSuppressDecimalValues As Boolean

    Public Property InitialTaxAmount() As Decimal
        Get
            Return m_dInitialTaxAmount
        End Get
        Set(ByVal Value As Decimal)
            m_dInitialTaxAmount = Value
        End Set
    End Property

    Public Property InitialCommRate() As Decimal
        Get
            Return m_dInitialCommRate
        End Get
        Set(ByVal Value As Decimal)
            m_dInitialCommRate = Value
        End Set
    End Property

    Public Property InitialCommValue() As Decimal
        Get
            Return m_dInitialCommValue
        End Get
        Set(ByVal Value As Decimal)
            m_dInitialCommValue = Value
        End Set
    End Property

    Public Property TaxGroupItemId() As Integer
        Get
            Return m_nTaxGroupItemId
        End Get
        Set(ByVal Value As Integer)
            m_nTaxGroupItemId = Value
        End Set
    End Property

    Public Property MaximumRateValue() As Decimal
        Get
            Return m_cMaximumRateValue
        End Get
        Set(ByVal Value As Decimal)
            m_cMaximumRateValue = Value
        End Set
    End Property
    Public Property IsValue() As Boolean
        Get
            Return m_bIsValue
        End Get
        Set(ByVal Value As Boolean)
            m_bIsValue = Value
        End Set
    End Property
    'End - Renuka - (WPR64 Paralleling)

    'Start (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1.1)
    Public WriteOnly Property IsTaxAmended() As Integer
        Set(ByVal Value As Integer)
            m_iIsTaxAmended = Value
        End Set
    End Property
    'End (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1.1)

    Public WriteOnly Property OverrideReason() As String
        Set(ByVal Value As String)
            m_sOverrideReason = Value
        End Set
    End Property

    Public WriteOnly Property IsAmended() As Integer
        Set(ByVal Value As Integer)
            m_iIsAmended = Value
        End Set
    End Property

    Public WriteOnly Property CalculatedCommissionValue() As Decimal
        Set(ByVal Value As Decimal)
            m_cCalculatedCommissionValue = Value
        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get
            Return m_lStatus
        End Get
    End Property

    Public WriteOnly Property Business() As bSirAgentCommission.Business
        Set(ByVal Value As bSirAgentCommission.Business)
            m_oBusiness = Value
        End Set
    End Property

    'Start (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1.1.3.1)
    'UPGRADE_NOTE: (7001) The following declaration (cboTaxGroup_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    Private Sub cboTaxGroup_Click()
        'Do the following actions only if the form is visible
        If Me.Visible Then
            If m_nTaxGroupItemId = 0 Then
                m_nTaxGroupItemId = cboTaxGroup.ItemId
            End If

            'Recalculate the tax
            If m_prevValue <> CDec(cboTaxGroup.ItemId) Then
                RecalculateTax()
            End If
            m_cCalculatedTaxValue = CDec(gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_cCalculatedTaxValue, 2))
            If m_nTaxGroupItemId <> ToSafeInteger(cboTaxGroup.ItemId) Then
                m_iIsTaxAmended = 1
                txtOverrideReason.Enabled = True
                txtOverrideReason.Text = m_sOverrideReason
            Else
                m_iIsTaxAmended = 0
            End If
            If txtTaxValue.Text.Trim() = "" Then
                txtTaxValue.Text = "0.00"
            End If
        End If

        CheckCommissionTaxOverride()
    End Sub
    'End (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1.1.3.1)

    Private Sub cboTaxGroup_LostFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTaxGroup.LostFocus
        'To do list
        'cboTaxGroup_Click(cboTaxGroup, Nothing)
    End Sub

    Private Sub cboTaxGroup_Validating(ByVal eventSender As Object, ByVal eventArgs As CancelEventArgs) Handles cboTaxGroup.Validating
        Dim Cancel As Boolean = eventArgs.Cancel
        m_prevValue = cboTaxGroup.ItemId
        eventArgs.Cancel = Cancel
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        m_lStatus = gPMConstants.PMEReturnCode.PMCancel
        Me.Hide()
    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        'If _StatusBar1_Panel1.Text.Trim() = "WARNING: Commission has been amended" Or _StatusBar1_Panel2.Text.Trim() = "WARNING: Tax Value has been amended" Then
        'If txtOverrideReason.Text = "" Then
        'MessageBox.Show("WARNING: Override Required", " Amend Tax & Commission Value", MessageBoxButtons.OK, MessageBoxIcon.Error)
        'Exit Sub
        'End If

        'End If

        m_lReturn = CheckCommissionOverride()

        'Start - Renuka - (WPR64 Paralleling)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If
        'End - Renuka - (WPR64 Paralleling)
        'Check the commission


        Dim cCommissionValue As Decimal = Conversion.Val(CStr(m_oFormFields.UnformatControl(txtCommissionvalue)))

        Dim cPremium As Decimal = Conversion.Val(CStr(m_oFormFields.UnformatControl(txtPremium)))
        If Math.Abs(cCommissionValue) > Math.Abs(cPremium) Then
            MessageBox.Show("The Commission exceeds the Total Premium." & Strings.Chr(13) & Strings.Chr(10) & "Please amend before proceeding", " Amend Commission Value", MessageBoxButtons.OK, MessageBoxIcon.Information)
            'Start - Renuka - (WPR64 Paralleling)
            If txtCommissionvalue.Enabled Then
                txtCommissionvalue.Focus()
            End If
            'End - Renuka - (WPR64 Paralleling)
            Exit Sub
        End If

        'Update the selected item
        Dim nItem As ListViewItem = lvwAgentCommission.FocusedItem
        If txtCommissionrate.Text = "" Then
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtCommissionrate, vControlValue:=0)
            'Start (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1.1)
            txtCommissionrate.Tag = CStr(0)
            'End (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1.1)
        End If

        'Start (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1.1)

        'Changes done by: Krishna Nand
        'Dated: 12/02/2010
        'PN:68552
        'Purpose: convert Commission Rate upto 4 decimal places
        'nItem.SubItems(ACCommPercent) = FormatField(PMFormatPercent, txtCommissionrate.Tag, 2)
        ListViewHelper.GetListViewSubItem(nItem, ACCommPercent).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, Convert.ToString(txtCommissionrate.Tag), 10)
        'End of changes done by Krishna Nand on 12/02/2010 for PN: 68552

        'End (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1.1)
        ListViewHelper.GetListViewSubItem(nItem, ACCommValue).Text = txtCommissionvalue.Text
        ListViewHelper.GetListViewSubItem(nItem, ACRawCommValue).Text = CStr(m_oFormFields.UnformatControl(txtCommissionvalue))
        ListViewHelper.GetListViewSubItem(nItem, ACTaxAmount).Text = txtTaxValue.Text
        ListViewHelper.GetListViewSubItem(nItem, ACRawTaxAmount).Text = CStr(m_oFormFields.UnformatControl(txtTaxValue))
        ListViewHelper.GetListViewSubItem(nItem, ACIsAmended).Text = CStr(m_iIsAmended)
        ListViewHelper.GetListViewSubItem(nItem, ACOverrideReason).Text = m_sOverrideReason
        'Start (Saurabh Agrawal) PN54244
        ListViewHelper.GetListViewSubItem(nItem, ACCalculatedCommissionValue).Text = txtCommissionvalue.Text
        'End (Saurabh Agrawal) PN54244

        'Start (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1.1.3.3)
        ListViewHelper.GetListViewSubItem(nItem, ACIsTaxAmended).Text = CStr(m_iIsTaxAmended)

        ListViewHelper.GetListViewSubItem(nItem, ACTaxGroupID).Text = CStr(cboTaxGroup.ItemId)
        'End (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1.1.3.3)
        m_lStatus = gPMConstants.PMEReturnCode.PMOK

        'Unload Me
        Me.Hide()

    End Sub

    Private Sub frmDetail_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender
            txtCommissionvalue.Enabled = Not ReadOnly_Renamed
            txtCommissionrate.Enabled = Not ReadOnly_Renamed
            ' Enable cboTaxGroup and txtTaxValue to make them editable if the system option �Override Agent Tax Group Allowed� is set
            If CanOverrideAgentTaxGroup() Then
                cboTaxGroup.Enabled = Not ReadOnly_Renamed
                txtTaxValue.Enabled = Not ReadOnly_Renamed
            End If
            'End (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1.1)
        End If
    End Sub

    Public Sub frmDetailLoad()
        m_oFormFields = New iPMFormControl.FormFields()

        m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPremium, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
        m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCommissionrate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatPercent, lFieldType:=gPMConstants.PMEDataType.PMDecimal, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory, lDecimalPlaces:=10)
        m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtTaxValue, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

        m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCommissionvalue, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

        m_lReturn = CType(PopulatePartyCombo(), gPMConstants.PMEReturnCode)

        'PM034504 - Added
        txtOverrideReason.Text = m_sOverrideReason

        'Start (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1.1)
        ' Setup status bar panels
        With StatusBar1


            'TODO
            '.style = MSComctlLib.SbarStyleConstants.sbrNormal

            'TODO
            '.Items.Add(New ToolStripStatusLabel(2, "TaxAmended", "", ToolStripItemAlignment.Left))
            Dim tlStripLabel As ToolStripStatusLabel = New ToolStripStatusLabel()
            tlStripLabel.Name = "TaxAmended"
            tlStripLabel.TextAlign = ContentAlignment.MiddleLeft
            .Items.AddRange(New System.Windows.Forms.ToolStripItem() {tlStripLabel})
            With .Items.Item(0)

                'TODO
                '.MinWidth = 1500
                .Width = Me.Width / 2
                .TextImageRelation = TextImageRelation.ImageBeforeText
                .TextAlign = ContentAlignment.MiddleLeft
                .AutoSize = True
            End With

            With .Items.Item(1)

                'TODO
                '.MinWidth = 1500
                .Width = Me.Width / 2
                .TextImageRelation = TextImageRelation.ImageBeforeText
                .TextAlign = ContentAlignment.MiddleLeft
                .AutoSize = True
            End With
        End With
        'End (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1.1)

        'Get the Decimal Suppression flag
        Dim sTempOptionValue As String = ""
        iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTEnableDecimalsSuppression, v_vBranch:=1, r_vUnderwriting:=sTempOptionValue)
        If Trim(sTempOptionValue) <> "" AndAlso Trim(sTempOptionValue) = "1" Then
            m_bSuppressDecimalValues = True
        End If


        m_lStatus = gPMConstants.PMEReturnCode.PMCancel
    End Sub



    Private Sub frmDetail_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        m_oFormFields.Dispose()

        m_oFormFields = Nothing

        eventArgs.Cancel = Cancel <> 0
    End Sub


    Private isInitializingComponent As Boolean
    Private Sub txtCommissionrate_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCommissionrate.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        'Start (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1)
        ' Do the following actions only if the form is visible.
        Dim dCommissionRate As Double
        Dim cPremium As Decimal
        If Me.Visible Then
            'End (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1)

            If Not m_bChange Then
                'Set the change flag
                m_bChange = True

                'Start (Prakash Varghessiriuscomme) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1)
                If txtCommissionrate.Text.Trim() = "" Then
                    txtCommissionrate.Tag = CStr(0)
                Else

                    txtCommissionrate.Tag = CStr(m_oFormFields.UnformatControl(txtCommissionrate))
                End If

                'Get the commission rate and Premium from the textbox
                dCommissionRate = Conversion.Val(Convert.ToString(txtCommissionrate.Tag))
                'End (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1)


                cPremium = CDec(m_oFormFields.UnformatControl(txtPremium))
                ' PN_73361 Start

                If gPMFunctions.ToSafeCurrency(txtCommissionrate.Text.Trim()) >= 0 Then
                    'do not calculate Commission if Premium is ZERO
                    'Renuka - (WPR64 Paralleling) - Changed the IF condition
                    'If Not m_bIsValue Then
                    If m_bSuppressDecimalValues Then
                        'Round Uptp zero decimals
                        m_lReturn = m_oFormFields.FormatControl(txtCommissionvalue, gPMFunctions.ToSafeRound(cPremium * (dCommissionRate / 100), 2, m_bSuppressDecimalValues))
                    Else
                        m_lReturn = m_oFormFields.FormatControl(txtCommissionvalue, cPremium * (dCommissionRate / 100))
                    End If


                    'End If
                End If
                CheckCommissionOverride()
                'PN_73361 end

                m_bChange = False
        End If
        'Start (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1)
        End If
        'End (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1)
        Dim p_cCurCommissionRate As Decimal = gPMFunctions.ToSafeDecimal(txtCommissionrate.Text.Replace("%"c, "").Trim)

        If m_prevValue <> p_cCurCommissionRate Then
            txtOverrideReason.Enabled = True
        Else
            txtOverrideReason.Enabled = False
        End If
    End Sub

    Private Sub txtCommissionrate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCommissionrate.Enter
        'PN_68557 start
        m_bisGetCursor = True
        If txtCommissionrate.Text.IndexOf("%"c) > 0 Then
            m_prevValue = CDec(txtCommissionrate.Text.Trim().Substring(0, txtCommissionrate.Text.Trim().Length - 1))
        Else
            m_prevValue = CDec(txtCommissionrate.Text)
        End If

        'PN_68557 end

        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtCommissionrate)
    End Sub

    'PN_73361 Start
    Private Sub txtCommissionrate_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtCommissionrate.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        Dim sCommisionrate As String = String.Empty
        If KeyAscii = 8 Then
            If KeyAscii = 0 Then
                eventArgs.Handled = True
            End If
            Exit Sub
        End If
        If Not ((KeyAscii >= Strings.Asc(CStr(0)(0)) And KeyAscii <= Strings.Asc(CStr(9)(0))) Or KeyAscii = 46 Or KeyAscii = 45) Then
            KeyAscii = 0
        End If

        If txtCommissionrate.Text.IndexOf("-"c) >= 0 And KeyAscii = 45 Then KeyAscii = 0
        sCommisionrate = txtCommissionrate.Text
        sCommisionrate = CStr(gPMFunctions.UnFormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, gPMConstants.PMEFormatStyle.PMFormatString, sCommisionrate))
        If Strings.Len(sCommisionrate) > 13 Then
            If gPMFunctions.ToSafeCurrency(CStr(CDbl(txtCommissionrate.Text) * 100)) > 0 Then
            Else
                KeyAscii = 0
            End If
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)

    End Sub
    'PN_73361 End

    Private Sub txtCommissionrate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCommissionrate.Leave
        'Start - Renuka - (WPR64 Paralleling)
        If Not m_bIsValue Then
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtCommissionrate)
        End If
        'End  - Renuka - (WPR64 Paralleling)

        'PN_68557 Start Jeetendra



        'Recalculate the tax
        'Dim p_cCurCommissionRate As Decimal = IIf((txtCommissionrate.Text.IndexOf("%"c) + 1), CDec(txtCommissionrate.Text.Trim().Substring(0, txtCommissionrate.Text.Trim().Length - 1)), CDec(txtCommissionrate.Text))
        Dim p_cCurCommissionRate As Decimal = gPMFunctions.ToSafeDecimal(txtCommissionrate.Text.Replace("%"c, "").Trim)
        If m_prevValue <> p_cCurCommissionRate Then
            RecalculateTax()
        End If
        m_bisGetCursor = False
        m_cCalculatedTaxValue = CDec(gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(m_cCalculatedTaxValue), 2))
        If txtCommissionrate.Text.Trim() = "" Then
            txtCommissionrate.Text = "0.0000000000"
        End If
        If m_cCalculatedTaxValue <> CDec(txtTaxValue.Text) Or (m_cCalculatedTaxValue <> CDec(txtTaxValue.Text) And m_iIsTaxAmended = 1) Then
            m_iIsTaxAmended = 1
        Else
            If m_iIsAmended <> 1 Then
                m_iIsAmended = 0
            End If
        End If
        m_lReturn = CheckCommissionOverride()
        CheckCommissionTaxOverride()

    End Sub

    Private Sub txtCommissionvalue_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCommissionvalue.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        ' Do the following actions only if the form is visible.
        If m_bisGetCursor Then Exit Sub

        Dim dCommissionValue As Double
        Dim cPremium As Decimal
        If Me.Visible Then
            If Not m_bChange Then

                CheckCommissionOverride()
                'Set the rate as zero

                If txtCommissionvalue.Text.Trim() <> "" And Not (gPMFunctions.ToSafeDecimal(txtCommissionvalue.Text.Trim()) = 0) Then

                    cPremium = Conversion.Val(CStr(m_oFormFields.UnformatControl(txtPremium)))

                    dCommissionValue = Conversion.Val(CStr(m_oFormFields.UnformatControl(txtCommissionvalue)))
                    If cPremium <> 0 Then
                        txtCommissionrate.Tag = CStr(Math.Round((dCommissionValue * 100) / cPremium, 8))
                    End If

                    'Pn_73361 Start
                    If Not (gPMFunctions.ToSafeCurrency(txtPremium.Text.Trim()) = 0) Then
                        txtCommissionrate.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, Convert.ToString(txtCommissionrate.Tag), 10)
                    End If
                ElseIf gPMFunctions.ToSafeDecimal(txtCommissionvalue.Text.Trim()) = 0 Then
                    txtCommissionrate.Tag = 0
                    txtCommissionrate.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, Convert.ToString(txtCommissionrate.Tag), 10)
                End If


            End If
            'Recalculate Tax here since the lost focus event will not fire when txtCommissionValue is disabled
            RecalculateTax()

        End If
        'End (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1)
    End Sub

    Private Sub txtCommissionvalue_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCommissionvalue.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtCommissionvalue)

    End Sub

    'PN_69833 Start
    Private Sub txtCommissionvalue_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtCommissionvalue.KeyPress

        'Prevent to enter decimal digits
        If m_bSuppressDecimalValues Then
            gPMFunctions.NumPress(eventSender, eventArgs)
            Exit Sub
        End If

        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        If KeyAscii = 8 Then
            If KeyAscii = 0 Then
                eventArgs.Handled = True
            End If
            Exit Sub
        End If
        If Not ((KeyAscii >= Strings.Asc(CStr(0)(0)) And KeyAscii <= Strings.Asc(CStr(9)(0))) Or KeyAscii = 46 Or KeyAscii = 45) Then
            KeyAscii = 0
        End If
        If txtCommissionvalue.Text.IndexOf("-"c) >= 0 And KeyAscii = 45 Then KeyAscii = 0

        If Strings.Len(txtCommissionvalue.Text) > 13 Then
            If gPMFunctions.ToSafeCurrency(CStr(CDbl(txtCommissionvalue.Text) * 10)) > 0 Then
            Else
                KeyAscii = 0
            End If
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)

    End Sub
    'PN_69833 Start

    Private Sub txtCommissionvalue_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCommissionvalue.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtCommissionvalue)

        'Recalculate the tax
        RecalculateTax()

        'PN:28198
        If txtCommissionvalue.Text.Trim() = "" Then
            txtCommissionvalue.Text = "0.00"
        End If
        m_lReturn = CType(CheckCommissionOverride(), gPMConstants.PMEReturnCode)
        CheckCommissionTaxOverride()
    End Sub

    Public Function PopulatePartyCombo() As Integer
        ' ---------------------------------------------------------------------------
        ' NAME: PopulatePartyCombo
        ' DESCRIPTION:
        ' AUTHOR: Danny Davis
        ' DATE: 09 May 2005, 11:11:39
        ' HISTORY:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Const kMethodName As String = "PopulatePartyCombo"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Const nCAgent_id As Integer = 0
            Const nCAgent_Desc As Integer = 1
            Dim vntPartyDetails(,) As Object


            'Get the party details

            m_lReturn = m_oBusiness.GetAllParties(vntPartyDetails)

            'Clear the Agent combo box in the Detai form
            cboAgent.Items.Clear()


            For nCount As Integer = vntPartyDetails.GetLowerBound(1) To vntPartyDetails.GetUpperBound(1)
                Dim cboAgent_NewIndex2 As Integer = -1
                cboAgent_NewIndex2 = cboAgent.Items.Add(CStr(vntPartyDetails(nCAgent_Desc, nCount)))
                VB6.SetItemData(cboAgent, cboAgent_NewIndex2, CInt(vntPartyDetails(nCAgent_id, nCount)))
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
    'Start (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx)
    ' Modified the method to get the Recalculated tax without changing the txtTaxValue.Text
    Private Function RecalculateTax(Optional ByVal v_bUpdateTaxValueControl As Boolean = True) As Integer
        'End (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx)
        ' ---------------------------------------------------------------------------
        ' NAME: RecalculateTax
        ' DESCRIPTION:
        ' AUTHOR: Danny Davis
        ' DATE: 09 May 2005, 11:12:00
        ' HISTORY:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Const kMethodName As String = "RecalculateTax"

        'Start (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx)
        'removed the local variable cTax and used the newly added module variable m_cCalculatedTaxValue

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Gets the return value to the newly added module varaible

            m_lReturn = m_oBusiness.CalculateAgentTax(lInsuranceFileCnt:=InsuranceFileCnt, iSourceID:=SourceID, lTaxGroupID:=gPMFunctions.ToSafeLong(CStr(cboTaxGroup.ItemId)), lCurrencyID:=CurrencyID, cAmount:=m_oFormFields.UnformatControl(txtCommissionvalue), r_cTax:=m_cCalculatedTaxValue)

            '            'PN_68557 Start
            '            If txtTaxValue.Tag <> m_cCalculatedTaxValue Then
            '                m_iIsTaxAmended = 1
            '            Else
            '                m_iIsTaxAmended = 0
            '            End If
            'PN_68557 End

            'End (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(m_lReturn.ToString() + ", " + +", CalculateAgentTax failed")
            End If




        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
            'Start (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx)
            m_cCalculatedTaxValue = 0
            'End (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx)

        Finally
            'Start (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx)
            'Change txtTaxValue.Text only if parameter v_bUpdateTaxValueControl is true
            If v_bUpdateTaxValueControl Then
                txtTaxValue.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(m_cCalculatedTaxValue), 2)
            End If
            'End (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx)





        End Try
        Return result
    End Function


    ' ***************************************************************** '
    '
    ' Name: CheckCommissionOverride
    '
    ' Description:
    '
    ' History: 24/10/2005 RKS - Created.
    '
    ' ***************************************************************** '
    Public Function CheckCommissionOverride() As Integer

        'Start - Renuka - (WPR64 Paralleling)
        Dim result As Integer = 0
        Dim sCommisionrate As String = ""
        'End - Renuka - (WPR64 Paralleling)
        Try

            'Start (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1.1)
            Dim cCommissionValue, cPremium As Decimal
            'End (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1.1)

            result = gPMConstants.PMEReturnCode.PMFalse

            Dim dbNumericTemp As Double
            If Not Double.TryParse(txtCommissionvalue.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                Return result
            End If

            If m_cCalculatedCommissionValue <> CDec(txtCommissionvalue.Text) Then

                'Commission is overridden
                m_iIsAmended = 1
                txtOverrideReason.Enabled = True
                txtOverrideReason.Text = m_sOverrideReason

                'Start (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1.1)
                _StatusBar1_Panel1.Text = "WARNING: Commission has been amended" + Space(20)
                If _StatusBar1_Panel2.Text.Trim = String.Empty Then
                    _StatusBar1_Panel2.Text = Space(100)
                End If
                'StatusBar1.Items.Item(0).Text = "WARNING: Commission has been amended"
                'End (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1.1)
            Else
                ' If m_iIsAmended <> 1 Then
                m_iIsAmended = 0
                'End If
                txtOverrideReason.Enabled = False
                'PM034504 - Commented
                'txtOverrideReason.Text = ""

                'Start (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1.1)
                _StatusBar1_Panel1.Text = Space(100)
                'StatusBar1.Items.Item(0).Text = ""
                'End (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1.1)
            End If

            'Start - Renuka - (WPR64 Paralleling)
            If txtCommissionrate.Text.IndexOf("%"c) >= 0 Then
                sCommisionrate = txtCommissionrate.Text

                sCommisionrate = CStr(gPMFunctions.UnFormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, gPMConstants.PMEFormatStyle.PMFormatString, sCommisionrate))
            Else
                sCommisionrate = txtCommissionrate.Text
            End If
            Dim sCommisionValue As String = ""
            If txtCommissionvalue.Text.IndexOf("%"c) >= 0 Then
                sCommisionValue = txtCommissionvalue.Text

                sCommisionValue = CStr(gPMFunctions.UnFormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, gPMConstants.PMEFormatStyle.PMFormatString, sCommisionValue))
            Else
                sCommisionValue = txtCommissionvalue.Text
            End If
            If m_cMaximumRateValue <> 0 Then
                If m_bIsValue Then
                    If CDec(sCommisionValue) > m_cMaximumRateValue Then
                        MessageBox.Show("The Commission Value is more than the Maximum Rate defined as part of configuration." & Strings.Chr(13) & Strings.Chr(10) & "The Maximum Rate allowed is " & gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(m_cMaximumRateValue)), " Amend Commission Value", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        txtCommissionvalue.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(m_cMaximumRateValue))
                        Return result
                    End If
                Else
                    If CDec(sCommisionrate) > m_cMaximumRateValue Then
                        MessageBox.Show("The Commission Rate entered is more than the Maximum Rate defined as part of configuration." & Strings.Chr(13) & Strings.Chr(10) & "The Maximum Rate allowed is " & gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, CStr(m_cMaximumRateValue)), " Amend Commission Value", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        'Start (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx)
                        txtCommissionrate.Tag = CStr(Math.Round(m_cMaximumRateValue, 8))
                        'Changes done by: Krishna Nand
                        'Dated: 12/02/2010
                        'PN:68552
                        'Purpose: convert Commission Rate upto 4 decimal places
                        txtCommissionrate.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, Convert.ToString(txtCommissionrate.Tag), 10)
                        'txtCommissionrate.Text = FormatField(PMFormatPercent, txtCommissionrate.Tag, 2)
                        'End of changes done by Krishna Nand on 12/02/2010 for PN: 68552

                        'End (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx)
                        Return result
                    End If
                End If
            End If
            If m_cMaximumRateValue <> 0 Then
                If m_bIsValue Then
                    txtCommissionrate.Enabled = False
                    txtCommissionvalue.Enabled = True
                Else
                    txtCommissionvalue.Enabled = False
                    txtCommissionrate.Enabled = True
                End If
            End If
            'End  - Renuka - (WPR64 Paralleling)
            'This is to handle rounding error
            'Start (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1)
            If Convert.ToString(txtCommissionrate.Tag).Trim() = "" Then
                txtCommissionrate.Tag = CStr(0)
            End If
            If txtCommissionvalue.Text.Trim() = "" Then
                txtCommissionvalue.Text = CStr(0)
            End If

            cCommissionValue = Conversion.Val(CStr(m_oFormFields.UnformatControl(txtCommissionvalue)))

            cPremium = Conversion.Val(CStr(m_oFormFields.UnformatControl(txtPremium)))

            If cPremium <> 0 Then
                If gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr((CDbl(Convert.ToString(txtCommissionrate.Tag)) * cPremium) / 100), 2) <> txtCommissionvalue.Text Then
                    'Start - Renuka - (WPR64 Paralleling)
                    If m_bIsValue Then
                        'Recalculate commission percentage, in both cases - Commission is amended or not.
                        txtCommissionrate.Tag = CStr(Math.Round((cCommissionValue * 100) / cPremium, 8))
                        'Changes done by: Krishna Nand
                        'Dated: 12/02/2010
                        'PN:68552
                        'Purpose: convert Commission Rate upto 4 decimal places
                        'txtCommissionrate.Text = FormatField(PMFormatPercent, txtCommissionrate.Tag, 2)
                        txtCommissionrate.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, Convert.ToString(txtCommissionrate.Tag), 10)
                        'End of changes done by Krishna Nand on 12/02/2010 for PN: 68552

                    End If
                    'End - Renuka - (WPR64 Paralleling)
                End If
            End If
            'End (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1)


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckCommissionOverride Failed", vApp:=ACApp, vClass:=Me, vMethod:="CheckCommissionOverride", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Private Sub txtOverrideReason_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtOverrideReason.Leave
        m_sOverrideReason = txtOverrideReason.Text
    End Sub

    'Start (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1.1.4.1)
    ' ***************************************************************** '
    '
    ' Name: CheckCommissionTaxOverride
    '
    ' Description:
    '
    ' History: 18/12/2009
    '
    ' ***************************************************************** '
    Public Function CheckCommissionTaxOverride() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            Dim dbNumericTemp As Double
            If Not Double.TryParse(txtTaxValue.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                Return result
            End If

            m_cCalculatedTaxValue = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_cCalculatedTaxValue, 2)
            If m_iIsTaxAmended = 1 And m_dInitialTaxAmount <> CDec(txtTaxValue.Text) Then
                txtOverrideReason.Enabled = True
                txtOverrideReason.Text = m_sOverrideReason
            End If
            If m_iIsTaxAmended = 1 Then
                'Tax is overridden. Set warning message
                If _StatusBar1_Panel1.Text.Trim = String.Empty Then
                    _StatusBar1_Panel1.Text = Space(100)
                End If
                _StatusBar1_Panel2.Text = "WARNING: Tax Value has been amended" + Space(50)
                'StatusBar1.Items.Item(1).Text = "WARNING: Tax Value has been amended"

            Else
                _StatusBar1_Panel2.Text = ""
                'StatusBar1.Items.Item(1).Text = ""
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckCommissionTaxOverride Failed", vApp:=ACApp, vClass:=Me, vMethod:="CheckCommissionTaxOverride", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    'End (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1.1.4.1)

    'Start (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1.1.3.2)
    Private Sub txtTaxValue_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTaxValue.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtTaxValue)
    End Sub

    'PN_69833  Start
    Private Sub txtTaxValue_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtTaxValue.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        'Prevent to enter decimal digits
        If m_bSuppressDecimalValues Then
            gPMFunctions.NumPress(eventSender, eventArgs)
            Exit Sub
        End If

        If KeyAscii = 8 Then
            If KeyAscii = 0 Then
                eventArgs.Handled = True
            End If
            Exit Sub
        End If
        If Not ((KeyAscii >= Strings.Asc(CStr(0)(0)) And KeyAscii <= Strings.Asc(CStr(9)(0))) Or KeyAscii = 46 Or KeyAscii = 45) Then
            KeyAscii = 0
        End If
        If txtTaxValue.Text.IndexOf("-"c) >= 0 And KeyAscii = 45 Then KeyAscii = 0

        If Strings.Len(txtTaxValue.Text) > 10 Then
            If gPMFunctions.ToSafeCurrency(CStr(CDbl(txtTaxValue.Text) * 10)) > 0 Then
            Else
                KeyAscii = 0
            End If
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)

    End Sub
    'PN_69833  End

    Private Sub txtTaxValue_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTaxValue.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtTaxValue)

        If txtTaxValue.Text.Trim() = "" Then
            txtTaxValue.Text = "0.00"
        End If

        RecalculateTax(v_bUpdateTaxValueControl:=False)

        'PN_68557 Jeetendra
        m_cCalculatedTaxValue = CDec(gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(m_cCalculatedTaxValue), 2))
        'If m_cCalculatedTaxValue <> CCur(txtTaxValue.Text) Or (m_cCalculatedTaxValue <> CCur(txtTaxValue.Text) And m_iIsTaxAmended = 1) Then
        If m_cCalculatedTaxValue <> CDec(txtTaxValue.Text) Or (m_cCalculatedTaxValue <> CDec(txtTaxValue.Text) And m_iIsTaxAmended = 1) Then
            'If m_dInitialTaxAmount <> CDec(txtTaxValue.Text) Then
            m_iIsTaxAmended = 1
        Else
            If m_nTaxGroupItemId <> ToSafeInteger(cboTaxGroup.ItemId) Then
                m_iIsTaxAmended = 1
            Else
                m_iIsTaxAmended = 0
            End If
        End If
        CheckCommissionTaxOverride()
        'PN_68557 End

    End Sub
    'End (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1.1.3.2)

    'Start (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx)
    Private Function CanOverrideAgentTaxGroup() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CanOverrideAgentTaxGroup"
        Const kSystemOptionOverrideAgentTaxGroup As Integer = 5081

        Dim sValue As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue
        Try



            m_lReturn = CType(iPMFunc.GetSystemOption(v_iOptionNumber:=kSystemOptionOverrideAgentTaxGroup, r_sOptionValue:=sValue, v_iSourceID:=SourceID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get System Option for Override Agent Tax Group", gPMConstants.PMELogLevel.PMLogError)
            End If

            result = gPMFunctions.ToSafeLong(sValue, CInt("0"))



        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


        End Try
        Return result
    End Function
    'End (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx)


    Private Sub cboTaxGroup_ItemIdChange() Handles cboTaxGroup.ItemIdChange
        cboTaxGroup_Click()
    End Sub
    Private Sub FillCombo()
        'Developer Guide No. 220
        RemoveHandler cboTaxGroup.ItemIdChange, AddressOf cboTaxGroup_ItemIdChange
        Me.cboTaxGroup.FirstItem = "(none)"
        AddHandler cboTaxGroup.ItemIdChange, AddressOf cboTaxGroup_ItemIdChange
        Me.cboRiskType.FirstItem = ""
        Me.cboCommissionBand.FirstItem = ""
        Me.cboPartyAgentType.FirstItem = ""
    End Sub

    'Private Sub txtTaxValue_TextChanged(sender As Object, e As EventArgs) Handles txtTaxValue.TextChanged
    '    If m_bTaxValueKeyPressed Then
    '        If m_dInitialTaxAmount <> ToSafeDecimal(txtTaxValue.Text) Then
    '            m_iIsTaxAmended = 1
    '            txtOverrideReason.Enabled = True
    '            txtOverrideReason.Text = m_sOverrideReason
    '        Else
    '            m_iIsTaxAmended = 0
    '        End If
    '        If txtTaxValue.Text.Trim() = "" Then
    '            txtTaxValue.Text = "0.00"
    '        End If
    '        CheckCommissionTaxOverride()
    '        m_bTaxValueKeyPressed = False
    '    End If
    'End Sub
End Class
