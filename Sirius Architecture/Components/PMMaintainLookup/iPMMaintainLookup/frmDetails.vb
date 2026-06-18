Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports System.Windows.Forms
' Developer Guide No. 129
Imports SharedFiles
Imports System.String
Imports Microsoft.VisualBasic.Compatibility.VB6

Partial Friend Class frmDetails
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 17/05/99
    '
    ' Description: Main interface.
    '
    ' Edit History:
    ' RVH240902 - If screen size would be too large, add scrolling
    ' DAK291099 - Improve tab sequencing
    ' DAK251199 - Prevent error when exiting using x button.
    '             Make description mandatory
    ' DAK011299 - Changed privilege levels
    ' DAK031299 - Set authority and object parameters in calling module
    '             Set SetKeyArray here
    ' DAK080200 - only update if in Add or Edit mode
    '           - allow for dates in input fields:
    '             MaxLength = column length + 2 to allow for delimiters
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmDetails"

    Private m_sIDColumnName As String = ""
    Private m_lIDColumnValue As Integer
    Private m_lCaptionID As Integer
    Private m_sCode As String = ""
    Private m_sDescription As String = ""
    Private m_iIsDeleted As Integer
    Private m_dtEffectiveDate As Date
    Private m_vExtras(,) As Object
    Private m_sUniqueId As String

    Private m_iStatus As gPMConstants.PMEReturnCode
    Private m_iTask As gPMConstants.PMEComponentAction

    Private m_iProductFamily As Integer

    ' Array containing what the extra controls are
    'Developer Guide No. 101
    Private m_vExtraTypes As Object

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' PMAuthorityLevel
    Private m_lPMAuthorityLevel As Integer
    ' PrivilegeLevel
    Private m_iPrivilegeLevel As Integer

    ' Return variable
    Private m_lReturn As Integer
    'MIPS
    Private m_sTableName As String = ""

    ' RVH 24092002 - Add constant for max height
    Private Const clMaxHeight As Integer = 400 '700 '350 '7000

    'Start - Sankar - (WPRvb64 Media Type Status) - Paralleling
    Private Const csMediaTypeStatusTable As String = "MediaType_Status"
    Private Const csMediaTypeTable As String = "MediaType"
    Private Const csIsReceiptCaption As String = "is_receipt"
    Private Const csMediaTypeStatusCaption As String = "Default Status at Receipt"
    Private Const csMediaTypeValidationTable As String = "MediaType_Validation"
    '' start 68551
    Private Const csMediaTypeValidationIDCaption As String = "mediatype_validation_id"
    Private Const csMediaTypeValidationIDCash As String = "Cash"
    Private Const csMediaTypeValidationIDCreditCard As String = "Credit Card"
    Private Const csIsAdditionalDetailsCaption As String = "is_additional_details"
    Private Const kIsPaymentTypeClaimPaymentCaption As String = "Is_Payment_Type_Claim_Payment"
    Private Const kRuleTypeCaption As String = "risk_type_rule_set_type"

    Dim m_iMediaTypeValidationIDIndex As Integer
    Dim m_iIsAdditionalDetailsIndex As Integer
    '' End 68551
    Dim m_bMediaTypeMaintenanceTask As Boolean
    Dim m_iIsReceiptControlIndex As Integer
    Dim m_iMediaTypeStatusControlIndex As Integer
    'End - Sankar - (WPRvb64 Media Type Status) - Paralleling
    Dim m_iIsPaymentTypeClaimPaymentIndex As Integer
    Private makeTextBox0 As Boolean = False
    Private udlVersionControlId As Integer = -1
    Dim nIsPaymentTypeClaimPaymentIndex As Integer
    ' Events
    'Developer Guide No. 101
    Public Event LaunchLinkedObject(ByVal v_vSetKeyArray As Object)

    Private DynamicCheckBoxCount As Integer
    Private DynamicTextboxCount As Integer
    Private DynamicLookupCount As Integer
    Private DynamicLabelCount As Integer
    Private m_bSomethingChanged As Boolean = False
    Private m_bIsImportedMarketplaceDM As Boolean = False

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMMaintainLookup.General

    'consts to match table risk_type_rule_set_type
    Private Const VBScriptRules As Integer = 1
    Private Const CompiledRules As Integer = 3
    Private lCount As Integer

    Private m_sRulePath As String = ""
    Private m_lErrorNumber As gPMConstants.PMEReturnCode
    Private m_oBusiness As bPMMaintainLookup.Business
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer

    Dim btnFile As New Button()

    Public Property ProductFamily() As Integer
        Get
            Return m_iProductFamily
        End Get
        Set(ByVal Value As Integer)
            m_iProductFamily = Value
        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get
            Return m_iStatus
        End Get
    End Property


    Public Property CaptionID() As Integer
        Get
            Return m_lCaptionID
        End Get
        Set(ByVal Value As Integer)
            m_lCaptionID = Value
        End Set
    End Property

    Public Property Code() As String
        Get
            Return m_sCode
        End Get
        Set(ByVal Value As String)
            m_sCode = Value
            'We're editing or viewing, so disable editing of Code field
            txtCode.Enabled = False
        End Set
    End Property

    Public Property UniqueId() As String
        Get
            Return m_sUniqueId
        End Get
        Set(ByVal Value As String)
            m_sUniqueId = Value
        End Set
    End Property

    Public Property Description() As String
        Get
            Return m_sDescription
        End Get
        Set(ByVal Value As String)
            m_sDescription = Value
        End Set
    End Property

    Public Property IsDeleted() As Integer
        Get
            Return m_iIsDeleted
        End Get
        Set(ByVal Value As Integer)
            m_iIsDeleted = Value
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

    Public Property Extras() As Object
        Get
            Return VB6.CopyArray(m_vExtras)
        End Get
        Set(ByVal Value As Object)
            m_vExtras = Value
        End Set
    End Property

    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)
            m_iTask = Value
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

    Public Property PrivilegeLevel() As Integer
        Get
            Return m_iPrivilegeLevel
        End Get
        Set(ByVal Value As Integer)
            m_iPrivilegeLevel = Value
        End Set
    End Property

    'DAK031299
    Public Property IDColumnName() As String
        Get
            Return m_sIDColumnName.Trim()
        End Get
        Set(ByVal Value As String)
            m_sIDColumnName = Value.Trim()
        End Set
    End Property

    Public Property IDColumnValue() As Integer
        Get
            Return m_lIDColumnValue
        End Get
        Set(ByVal Value As Integer)
            m_lIDColumnValue = Value
        End Set
    End Property

    Public WriteOnly Property TableName() As String
        Set(ByVal Value As String)
            m_sTableName = Value
        End Set
    End Property
    Private Sub AddControlArray(ByVal cControl As String, ByVal lIndex As Integer)
        '
        Dim lControlIndex As Integer
        lControlIndex = lIndex

        Select Case cControl

            Case "txtExtra"

                ReDim Preserve txtExtra(lControlIndex)
                txtExtra(lControlIndex) = New TextBox
                txtExtra(lControlIndex).Name = "_txtExtra_" & CStr(lControlIndex)
                txtExtra(lControlIndex).Left = txtEffectiveDate.Left
                txtExtra(lControlIndex).Width = txtEffectiveDate.Width
                txtExtra(lControlIndex).Visible = True
                Me.Controls.Add(txtExtra(lControlIndex))
                'DynamicTextboxCount += 1

            Case "chkExtra"

                ReDim Preserve chkExtra(lControlIndex)
                chkExtra(lControlIndex) = New CheckBox
                chkExtra(lControlIndex).Name = "_chkExtra_" & CStr(lControlIndex)
                chkExtra(lControlIndex).RightToLeft = System.Windows.Forms.RightToLeft.No
                chkExtra(lControlIndex).Left = _chkExtra_0.Left
                chkExtra(lControlIndex).Width = _chkExtra_0.Width
                chkExtra(lControlIndex).TextAlign = _chkExtra_0.TextAlign
                chkExtra(lControlIndex).CheckAlign = _chkExtra_0.CheckAlign
                chkExtra(lControlIndex).Height = _chkExtra_0.Height
                Me.Controls.Add(chkExtra(lControlIndex))

            Case "lblExtra"

                ReDim Preserve lblExtra(lControlIndex)
                lblExtra(lControlIndex) = New Label
                lblExtra(lControlIndex).Name = "_lblExtra_" & CStr(lControlIndex)
                lblExtra(lControlIndex).Left = lblEffectiveDate.Left
                lblExtra(lControlIndex).Width = 200
                Me.Controls.Add(lblExtra(lControlIndex))
                'DynamicLabelCount += 1

            Case "cboLookupExtra"

                ReDim Preserve cboLookupExtra(lControlIndex)
                cboLookupExtra(lControlIndex) = New PMLookupControl.cboPMLookup
                cboLookupExtra(lControlIndex).Name = "_cboLookupExtra_" & CStr(lControlIndex)
                cboLookupExtra(lControlIndex).Width = 200
                cboLookupExtra(lControlIndex).Left = txtEffectiveDate.Left
                cboLookupExtra(lControlIndex).Height = txtEffectiveDate.Height
                cboLookupExtra(lControlIndex).FirstItem = "(null)"
                Me.Controls.Add(cboLookupExtra(lControlIndex))
                'DynamicLookupCount += 1

        End Select

    End Sub
    ' ***************************************************************** '
    ' Name: SetDynamicControls
    '
    ' Description: Adds the extra labels and text boxes. Then resizes
    '              the form as needed.
    '
    ' ***************************************************************** '
    Private Function SetDynamicControls() As gPMConstants.PMEReturnCode
        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim NewLargeChange As Integer
        Dim lMinusCount As Long

        Dim lControlType As MaintainConst.ACExtraControlType
        Dim sCaption As String = ""
        Dim lNewHeight As Integer
        Dim bEnabled As Boolean
        'Developer Guide No. 101
        Dim vValue As Object
        Dim iCountStampDuty As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Resize the extras type array
            m_vExtraTypes = Array.CreateInstance(GetType(Object), New Integer() {m_vExtras.GetUpperBound(1) - m_vExtras.GetLowerBound(1) + 1}, New Integer() {m_vExtras.GetLowerBound(1)})

            'm_lReturn = iPMFunc.getUnderwritingOrAgency(r_vUnderwriting:=vValue)

            ' Loop through array
            For lCount As Integer = m_vExtras.GetLowerBound(1) To m_vExtras.GetUpperBound(1)

                '****************************************
                'Disable controls depending on lookup
                'Set bEnabled to False to disable control
                '****************************************
                bEnabled = True

                '*****************************************************
                'PN #28979
                'GIS Screen must be amendable against a party type
                'Hence commented out the following code
                '*****************************************************
                '        If m_sIDColumnName = "party_type_id" Then
                '            If m_vExtras(ACExtraLookupTable, lCount) = "GIS_Screen" Then
                '                If ToSafeLong(m_vExtras(ACExtraValue, lCount)) <> 0 Then
                '                    bEnabled = False
                '                End If
                '            End If
                '        End If

                '****************************************

                ' Decide which control type we are...
                If CStr(m_vExtras(ACExtraLookupTable, lCount)) <> "" Then
                    lControlType = MaintainConst.ACExtraControlType.ACExtraTypeLookup
                ElseIf CStr(m_vExtras(ACExtraCaption, lCount)).Length >= 3 AndAlso (CStr(m_vExtras(ACExtraCaption, lCount)).Substring(0, 3) = "is_" Or CStr(m_vExtras(ACExtraCaption, lCount)).Substring(0, 3) = "add") Then
                    lControlType = MaintainConst.ACExtraControlType.ACExtraTypeCheckBox
                    'not sure if we can just use tinyint here and get away with it without testing every single table in lookup maintenance
                ElseIf m_vExtras(ACExtraType, lCount) = "tinyint" AndAlso m_sTableName.ToUpper() = "GIS_DATA_MODEL" Then
                    lControlType = MaintainConst.ACExtraControlType.ACExtraTypeCheckBox
                ElseIf m_vExtras(ACExtraType, lCount) = "tinyint" AndAlso m_sTableName.ToUpper() = "RI_BAND" Then
                    lControlType = MaintainConst.ACExtraControlType.ACExtraTypeCheckBox
                Else
                    lControlType = MaintainConst.ACExtraControlType.ACExtraTypeTextBox
                End If

                ' Only load what we need, start with the caption
                If lControlType <> MaintainConst.ACExtraControlType.ACExtraTypeCheckBox Then
                    If lCount > 0 Then
                        'ContainerHelper.LoadControl(Me, "lblExtra", lCount)
                        AddControlArray("lblExtra", lCount) '
                    End If

                    ' Set position and style
                    'lblExtra(lCount).Text = ReformatText(CStr(m_vExtras(ACExtraCaption, lCount))) & ":"
                    'lblExtra(lCount).Font = VB6.FontChangeBold(lblExtra(lCount).Font, CInt(m_vExtras(ACExtraOffset, lCount)) > 0)
                    'lblExtra(lCount).Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(txtExtra(0).Top) + ((VB6.PixelsToTwipsY(txtExtra(0).Height) + 90) * lCount) + 30)
                    '               lblExtra(lCount).Visible = Not (((m_sTableName.ToLower() = "tax_band" And CStr(m_vExtras(ACExtraCaption, lCount)).ToLower() = "tax_type_id") Or (m_sTableName.ToLower() = "tax_group" And CStr(m_vExtras(ACExtraCaption, lCount)).ToLower() = "advanced_tax_script")) And vValue = "A")

                    'If (((m_sTableName.ToLower() = "tax_band" AndAlso CStr(m_vExtras(ACExtraCaption, lCount)).ToLower() = "tax_type_id") Or
                    '     (m_sTableName.ToLower() = "tax_group" AndAlso CStr(m_vExtras(ACExtraCaption, lCount)).ToLower() = "advanced_tax_script")) And vValue = "A") Then
                    '    lblExtra(lCount).Visible = False
                    'Else
                    '    lblExtra(lCount).Visible = True
                    'End If

                    lblExtra(lCount).Visible = True
                    lblExtra(lCount).Text = ReformatText(CStr(m_vExtras(ACExtraCaption, lCount))) & ":"
                    lblExtra(lCount).Font = VB6.FontChangeBold(lblExtra(lCount).Font, CInt(m_vExtras(ACExtraOffset, lCount)) > 0)
                    lblExtra(lCount).Top = txtExtra(0).Top + ((txtExtra(0).Height + 4) * lCount) + 2

                    If LCase(m_vExtras(ACExtraCaption, lCount)) = "udl_version" Then
                        lblExtra(lCount).Visible = False
                        lMinusCount = lMinusCount + 1
                    End If

                    lblExtra(lCount).Parent = picCanvas
                End If

                Select Case lControlType
                    Case MaintainConst.ACExtraControlType.ACExtraTypeCheckBox
                        If DynamicCheckBoxCount > 0 Then
                            'ContainerHelper.LoadControl(Me, "chkExtra", lCount)
                            AddControlArray("chkExtra", DynamicCheckBoxCount) '
                        End If

                        'AddHandler chkExtra(lCount).CheckStateChanged, AddressOf chkExtra_CheckStateChanged

                        AddHandler chkExtra(DynamicCheckBoxCount).CheckStateChanged, AddressOf chkExtra_CheckStateChanged

                        'Start - Sankar - (WPRvb64 Media Type Status) - Paralleling
                        If CStr(m_vExtras(ACExtraCaption, lCount)) = csIsReceiptCaption Then
                            m_iIsReceiptControlIndex = DynamicCheckBoxCount
                        ElseIf CStr(m_vExtras(ACExtraCaption, lCount)) = csIsAdditionalDetailsCaption Then  '' start 68551
                            m_iIsAdditionalDetailsIndex = DynamicCheckBoxCount
                        ElseIf UCase(m_vExtras(ACExtraCaption, lCount)) = UCase(kIsPaymentTypeClaimPaymentCaption) Then
                            nIsPaymentTypeClaimPaymentIndex = DynamicCheckBoxCount
                        End If   '' End 68551
                        'End - Sankar - (WPRvb64 Media Type Status) - Paralleling
                        If m_sTableName = "peril_type" And (CStr(m_vExtras(ACExtraCaption, lCount)) = "is_stamp_duty_insurer" Or CStr(m_vExtras(ACExtraCaption, lCount)) = "is_stamp_duty_insured") Then
                            If CStr(m_vExtras(ACExtraCaption, 2)) = "lead_commission_band" And gPMFunctions.ToSafeInteger(m_vExtras(ACExtraValue, lCount)) = 1 Then
                                lblExtra(2).Font = VB6.FontChangeBold(lblExtra(2).Font, False)
                                iCountStampDuty = 1
                            ElseIf CStr(m_vExtras(ACExtraCaption, 2)) = "lead_commission_band" And gPMFunctions.ToSafeInteger(m_vExtras(ACExtraValue, lCount)) <> 1 And iCountStampDuty = 0 Then
                                lblExtra(2).Font = VB6.FontChangeBold(lblExtra(2).Font, True)
                            End If
                        End If
                        ' Set position and style
                        chkExtra(DynamicCheckBoxCount).Text = ReformatText(CStr(m_vExtras(ACExtraCaption, lCount))) & ":"
                        chkExtra(DynamicCheckBoxCount).TabIndex = lCount
                        'chkExtra(lCount).Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(txtExtra(0).Top) + ((VB6.PixelsToTwipsY(txtExtra(0).Height) + 90) * lCount) + 30)
                        chkExtra(DynamicCheckBoxCount).Top = txtExtra(0).Top + ((txtExtra(0).Height + 4) * lCount) + 2
                        chkExtra(DynamicCheckBoxCount).Visible = True

                        If m_sTableName.Trim().ToUpper() = "PARTY_TYPE" AndAlso CStr(m_vExtras(ACExtraCaption, lCount)) = "is_on_numbering_scheme" Then
                            chkExtra(DynamicCheckBoxCount).Visible = False
                        End If

                        If m_sTableName.Trim().ToUpper() = "WRITE_OFF_REASON" And m_vExtras(ACExtraCaption, lCount) = "is_valid_for_instalments" Then
                            chkExtra(DynamicCheckBoxCount).Text = "Valid for Instalments"
                        End If

                        If m_sTableName = "Debtor_User_Groups" Then
                            If m_vExtras(ACExtraLookupTable, 0) = "Debtor_User_Groups_Type" Then
                                If m_vExtras(ACExtraValue, 0) <> "" AndAlso m_vExtras(ACExtraValue, 0) = 6 Then
                                    chkExtra(nIsPaymentTypeClaimPaymentIndex).Visible = True
                                    chkExtra(nIsPaymentTypeClaimPaymentIndex).Checked = True
                                Else
                                    chkExtra(nIsPaymentTypeClaimPaymentIndex).Visible = False
                                End If
                            End If
                        End If



                        chkExtra(DynamicCheckBoxCount).CheckState = IIf(ToSafeInteger(m_vExtras(ACExtraValue, lCount)) = 1, CheckState.Checked, CheckState.Unchecked)
                        chkExtra(DynamicCheckBoxCount).Enabled = bEnabled
                        chkExtra(DynamicCheckBoxCount).Parent = picCanvas

                        '************************************************************************
                        'PN28188 both option should be ON by default
                        '************************************************************************
                        If m_iTask = gPMConstants.PMEComponentAction.PMAdd And (CStr(m_vExtras(ACExtraCaption, lCount)) = "is_include_tax_in_instalments" Or CStr(m_vExtras(ACExtraCaption, lCount)) = "is_spread_tax_across_instalments") Then
                            chkExtra(DynamicCheckBoxCount).CheckState = CheckState.Checked
                        End If
                        'PN32370
                        If m_iTask = gPMConstants.PMEComponentAction.PMAdd And (CStr(m_vExtras(ACExtraCaption, lCount)) = "is_in_tp_commission_calculation" Or CStr(m_vExtras(ACExtraCaption, lCount)) = "is_in_tp_premium_calculation") Then
                            chkExtra(DynamicCheckBoxCount).CheckState = CheckState.Checked
                        End If

                        If ToSafeString(m_vExtras(ACExtraCaption, lCount)) = "is_imported_marketplace_data_model" Then
                            chkExtra(DynamicCheckBoxCount).Enabled = False
                            m_bIsImportedMarketplaceDM = chkExtra(DynamicCheckBoxCount).Checked
                        End If

                        If m_bIsImportedMarketplaceDM And ToSafeString(m_vExtras(ACExtraCaption, lCount)) = "is_marketplace_data_model" Then
                            chkExtra(DynamicCheckBoxCount).Enabled = False
                        End If
                        ' Set type
                        m_vExtraTypes(lCount) = MaintainConst.ACExtraControlType.ACExtraTypeCheckBox
                        'lNewHeight = CInt(VB6.PixelsToTwipsY(chkExtra(lCount).Top))
                        lNewHeight = CInt((chkExtra(DynamicCheckBoxCount).Top))
                        DynamicCheckBoxCount += 1
                    Case MaintainConst.ACExtraControlType.ACExtraTypeLookup
                        If lCount > 0 Then
                            'ContainerHelper.LoadControl(Me, "cboLookupExtra", lCount)
                            AddControlArray("cboLookupExtra", lCount) '
                        End If
                        AddHandler cboLookupExtra(lCount).Click, AddressOf cboLookupExtra_ClickEvent

                        ' Set position and style
                        cboLookupExtra(lCount).PMLookupProductFamily = ProductFamily
                        cboLookupExtra(lCount).TabIndex = lCount
                        cboLookupExtra(lCount).TableName = CStr(m_vExtras(ACExtraLookupTable, lCount))

                        cboLookupExtra(lCount).Top = txtExtra(0).Top + ((txtExtra(0).Height + 4) * lCount)
                        cboLookupExtra(lCount).Width = txtEffectiveDate.Width
                        If m_sTableName = "Tax_Group" Then
                            cboLookupExtra(lCount).FirstItem = "NONE"
                            cboLookupExtra(lCount).WhereClause = "code in ('SCRIPT','COMPILED')"
                        Else
                            cboLookupExtra(lCount).FirstItem = "(NULL)"
                        End If

                        cboLookupExtra(lCount).Parent = picCanvas
                        If (CStr(m_vExtras(ACExtraCaption, lCount)).Trim().ToLower() = "number_scheme" AndAlso m_sTableName.Trim().ToLower() = "party_type") AndAlso Code.Trim().ToUpper() <> "PC" AndAlso Code.Trim().ToUpper() <> "CC" AndAlso Code.Trim().ToUpper() <> "GC" Then
                            cboLookupExtra(lCount).Enabled = False
                        Else
                            cboLookupExtra(lCount).Enabled = bEnabled
                        End If
                        'Start - Sankar - (WPRvb64 Media Type Status) - Paralleling
                        If cboLookupExtra(lCount).TableName = csMediaTypeStatusTable Then
                            'Set the label caption as "Default Status at Receipt"
                            lblExtra(lCount).Text = csMediaTypeStatusCaption & ":"
                            m_iMediaTypeStatusControlIndex = lCount
                            cboLookupExtra(lCount).WhereClause = "Code<>'SRPB'"
                        End If
                        'End - Sankar - (WPRvb64 Media Type Status) - Paralleling

                        ' Refresh the list and set value
                        If cboLookupExtra(lCount).ListCount > 0 Then
                            cboLookupExtra(lCount).RefreshList()
                        End If
                        cboLookupExtra(lCount).ItemId = ToSafeLong(m_vExtras(ACExtraValue, lCount))

                        ' Lookup control
                        m_vExtraTypes(lCount) = MaintainConst.ACExtraControlType.ACExtraTypeLookup
                        'lNewHeight = CInt(VB6.PixelsToTwipsY(cboLookupExtra(lCount).Top))
                        lNewHeight = CInt((cboLookupExtra(lCount).Top))
                        If (m_sTableName.ToLower() = "tax_band" AndAlso CStr(m_vExtras(ACExtraCaption, lCount)).ToLower() = "tax_type_id" AndAlso vValue = "A") Then
                            cboLookupExtra(lCount).Visible = False
                        Else
                            cboLookupExtra(lCount).Visible = True
                        End If

                        '' Start 68551
                        If CStr(m_vExtras(ACExtraCaption, lCount)) = csMediaTypeValidationIDCaption Then
                            m_iMediaTypeValidationIDIndex = lCount
                        End If
                        '' End 68551
                    Case MaintainConst.ACExtraControlType.ACExtraTypeTextBox
                        If lCount > 0 Then
                            'ContainerHelper.LoadControl(Me, "txtExtra", lCount)
                            AddControlArray("txtExtra", lCount) '
                        End If
                        AddHandler txtExtra(lCount).Leave, AddressOf txtExtra_Leave
                        AddHandler txtExtra(lCount).KeyPress, AddressOf txtExtra_KeyPress
                        AddHandler txtExtra(lCount).Enter, AddressOf txtExtra_Enter

                        ' Set position and style
                        txtExtra(lCount).MaxLength = 0
                        txtExtra(lCount).TabIndex = lCount

                        'Workaround to handle when left blank(though its saved as CDate(-1)
                        'Since i should not change the gPMFunction so i did this
                        'Modified,changed it for runtime
                        'If CDate(m_vExtras(ACExtraValue, lCount)) = #12/29/1899# Then


                        'txtExtra(lCount).Top = txtExtra(0).Top + VB6.TwipsToPixelsY((VB6.PixelsToTwipsY(txtExtra(0).Height) + 90) * lCount)
                        txtExtra(lCount).Top = txtExtra(0).Top + ((txtExtra(0).Height + 4) * lCount)
                        picCanvas.Visible = True
                        If (m_sTableName.ToLower() = "tax_group" AndAlso CStr(m_vExtras(ACExtraCaption, lCount)).ToLower() = "advanced_tax_script" AndAlso vValue = "A") Then
                            txtExtra(lCount).Visible = False
                        Else
                            txtExtra(lCount).Visible = True
                            If lCount = 0 Then
                                makeTextBox0 = True
                            End If

                        End If

                        If CStr(m_vExtras(ACExtraValue, lCount)) = "" Then
                            txtExtra(lCount).Text = ""
                        Else
                            txtExtra(lCount).Text = CStr(m_vExtras(ACExtraValue, lCount))
                        End If
                        txtExtra(lCount).Width = txtEffectiveDate.Width
                        txtExtra(lCount).Enabled = bEnabled

                        txtExtra(lCount).Left = txtExtra(0).Left
                        txtExtra(lCount).Parent = picCanvas

                        If LCase(m_vExtras(ACExtraCaption, lCount)) = "udl_version" Then
                            txtExtra(lCount).Visible = False
                            If LCase(m_vExtras(ACExtraCaption, lCount)) = "udl_version" Then
                                udlVersionControlId = lCount
                            End If
                        End If

                        If LCase(m_vExtras(ACExtraCaption, lCount)) = "pie_guid" Or LCase(m_vExtras(ACExtraCaption, lCount)) = "pie_last_updated" Then
                            txtExtra(lCount).Enabled = False
                        End If

                        ' Set formfields validation.
                        Select Case CStr(m_vExtras(ACExtraType, lCount)).ToUpper()
                            Case "BIT"
                                m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtExtra(lCount), lFormat:=gPMConstants.PMEFormatStyle.PMFormatBoolean, lFieldType:=gPMConstants.PMEDataType.PMBoolean, lGridColumn:=lCount)
                            Case "TINYINT", "SMALLINT"
                                m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtExtra(lCount), lFormat:=gPMConstants.PMEFormatStyle.PMFormatInteger, lFieldType:=gPMConstants.PMEDataType.PMInteger, lGridColumn:=lCount)
                            Case "TINYINT", "SMALLINT", "INT"
                                m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtExtra(lCount), lFormat:=gPMConstants.PMEFormatStyle.PMFormatLong, lFieldType:=gPMConstants.PMEDataType.PMLong, lGridColumn:=lCount)
                            Case "FLOAT"
                                m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtExtra(lCount), lFormat:=gPMConstants.PMEFormatStyle.PMFormatDouble, lFieldType:=gPMConstants.PMEDataType.PMDouble, lGridColumn:=lCount)
                            Case "NUMERIC", "DECIMAL"
                                m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtExtra(lCount), lFormat:=gPMConstants.PMEFormatStyle.PMFormatDecimal, lFieldType:=gPMConstants.PMEDataType.PMDecimal, lGridColumn:=lCount)
                            Case "DATETIME"
                                m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtExtra(lCount), lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lFieldType:=gPMConstants.PMEDataType.PMDate, lGridColumn:=lCount)
                                'Case for CURRENCY added for checking min value and max value
                                'PN 22505
                            Case "CURRENCY"
                                m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtExtra(lCount), lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lGridColumn:=lCount)
                            Case Else
                                m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtExtra(lCount), lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lFieldType:=gPMConstants.PMEDataType.PMString, lGridColumn:=lCount)

                                ' Override maxlength with string size
                                txtExtra(lCount).MaxLength = ToSafeLong(m_vExtras(ACExtraLength, lCount))
                        End Select

                        ' Check result
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        'If m_vExtras(ACExtraCaption, lCount) <> "campaign_date" Then
                        ' Reformat for starters
                        m_lReturn = m_oFormFields.LostFocus(txtExtra(lCount), , lCount)
                        ' End If

                        ' Lookup control
                        m_vExtraTypes(lCount) = MaintainConst.ACExtraControlType.ACExtraTypeTextBox
                        'lNewHeight = CInt(VB6.PixelsToTwipsY(txtExtra(lCount).Top))
                        lNewHeight = CInt((txtExtra(lCount).Top))
                End Select

            Next lCount

            If m_sTableName.ToUpper() = "TAX_GROUP" AndAlso cboLookupExtra.Length > 1 Then
                cboLookupExtra_ClickEvent(cboLookupExtra(0), New EventArgs())
            End If
            ' RVH 16092002 - Compute the new height
            'lNewHeight = CInt(lNewHeight + VB6.PixelsToTwipsY(txtExtra(txtExtra.GetUpperBound(0)).Height))
            lNewHeight = CInt(lNewHeight + (txtExtra(txtExtra.GetUpperBound(0)).Height))

            ' RVH 24092002 - Check to see if the new height would be greater than the current height
            'If lNewHeight < clMaxHeight And lNewHeight > VB6.PixelsToTwipsY(picContainer.Height) Then
            If lNewHeight < clMaxHeight And lNewHeight > (picContainer.Height) Then
                ' Resize the tab
                'tabMainTab.Height += VB6.TwipsToPixelsY(lNewHeight) - picContainer.Height
                'picContainer.Height = VB6.TwipsToPixelsY(lNewHeight)

                tabMainTab.Height += (lNewHeight) - picContainer.Height
                picContainer.Height = (lNewHeight)
            ElseIf lNewHeight >= clMaxHeight Then
                ' Resize the tab
                'tabMainTab.Height = VB6.TwipsToPixelsY(clMaxHeight)
                'picContainer.Height = VB6.TwipsToPixelsY((VB6.PixelsToTwipsY(tabMainTab.Height) - VB6.PixelsToTwipsY(tabMainTab.ItemSize.Height)) - 205)

                tabMainTab.Height = (clMaxHeight)
                picContainer.Height = (lNewHeight)
            End If

            ' Move the command buttons
            'cmdOK.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(tabMainTab.Top) + VB6.PixelsToTwipsY(tabMainTab.Height) + 120)
            cmdOK.Top = (tabMainTab.Top) + (tabMainTab.Height) + 6
            cmdCancel.Top = cmdOK.Top
            cmdLinkObject.Top = cmdOK.Top

            ' Resize the form
            Me.Height = (cmdOK.Top) + (cmdOK.Height) + 46

            ' RVH 24092002 - Resize canvas and scroll bar
            ' picCanvas.Height = VB6.TwipsToPixelsY(lNewHeight)
            picCanvas.Height = (lNewHeight)
            VScroll1.Height = picContainer.Height

            ' RVH 16092002 - Determine if the "canvas" is now larger than the "container" if
            '                so, show a scrollbar, otherwise hide it
            'If VB6.PixelsToTwipsY(picCanvas.ClientRectangle.Height) > VB6.PixelsToTwipsY(picContainer.ClientRectangle.Height) Then
            If (picCanvas.ClientRectangle.Height) > (picContainer.ClientRectangle.Height) Then
                'VScroll1.Visible = True
                VScroll1.Minimum = 1
                VScroll1.Maximum = (((picCanvas.ClientRectangle.Height) - (picContainer.ClientRectangle.Height)) + VScroll1.LargeChange - 1)
                VScroll1.SmallChange = 1 '20
                NewLargeChange = (picContainer.ClientRectangle.Height)
                VScroll1.Maximum = VScroll1.Maximum + NewLargeChange - VScroll1.LargeChange
                VScroll1.LargeChange = NewLargeChange
            Else
                VScroll1.Visible = False
            End If

            ' Re-sort the tab order (all Extras will be moved up 3)
            txtCode.TabIndex = 0
            txtDescription.TabIndex = 1
            txtEffectiveDate.TabIndex = 2
            If m_sTableName = "rating_section_type" AndAlso cboLookupExtra.Length > 4 Then
                cboLookupExtra_ClickEvent(cboLookupExtra(4), New EventArgs())
                cboLookupExtra(5).ItemId = ToSafeLong(m_vExtras(ACExtraValue, 5))
            End If
            'picContainer.Height = picContainer.Height + m_vExtras.GetUpperBound(1) * 12
            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetDynamicControls Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetDynamicControls", excep:=excep)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: HideExtra
    '
    ' Description: Hides the extra label and text box. Also resizes
    '              the form
    '
    ' ***************************************************************** '
    Private Function HideExtra() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Hide the controls
            txtExtra(0).Visible = False
            lblExtra(0).Visible = False
            cboLookupExtra(0).Visible = False
            chkExtra(0).Visible = False

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="HideExtra Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="HideExtra", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="HideExtra Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="HideExtra", excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: SetProperties
    '
    ' Description: Set the text values from the properties
    '
    ' ***************************************************************** '
    Private Function SetProperties() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Start - Sankar - (WPRvb64 Media Type Status) - Paralleling
            If m_sTableName = csMediaTypeTable Then
                m_bMediaTypeMaintenanceTask = True
            End If
            'End - Sankar - (WPRvb64 Media Type Status) - Paralleling
            ' Code
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtCode, vControlValue:=m_sCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'If table name is State, Restrict length to 3 characters
            If m_sTableName = "State" Then
                txtCode.MaxLength = 3
            End If

            ' Description
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtDescription, vControlValue:=m_sDescription)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Effective Date
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtEffectiveDate, vControlValue:=m_dtEffectiveDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If we have extra columns, then display controls for those
            If g_bHasExtras Then
                m_lReturn = SetDynamicControls()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Start - Sankar - (WPRvb64 Media Type Status) - Paralleling
                If m_bMediaTypeMaintenanceTask Then
                    SetupMediaTypeStatusControl()
                    EnableDisableAdditionalDetailsMediaType() '' 68551
                End If
                'End - Sankar - (WPRvb64 Media Type Status) - Paralleling
            Else
                ' Otherwise hide the one thats on the form, and re-size the form
                m_lReturn = HideExtra()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProperties", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetProperties
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function GetProperties() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_sCode = CStr(m_oFormFields.UnformatControl(ctlControl:=txtCode))

            m_sDescription = CStr(m_oFormFields.UnformatControl(ctlControl:=txtDescription))

            m_dtEffectiveDate = CDate(m_oFormFields.UnformatControl(ctlControl:=txtEffectiveDate))

            ' Get the extra properties

            If g_bHasExtras Then

                DynamicCheckBoxCount = m_vExtraTypes.GetLowerBound(0)

                For lCount As Integer = m_vExtraTypes.GetLowerBound(0) To m_vExtraTypes.GetUpperBound(0)


                    Select Case (m_vExtraTypes(lCount))
                        ' Text box
                        Case MaintainConst.ACExtraControlType.ACExtraTypeTextBox
                            ' unformat the control
                            If Not (CStr(m_vExtras(ACExtraCaption, lCount)).ToLower() = "pie_guid" Or CStr(m_vExtras(ACExtraCaption, lCount)).ToLower() = "pie_last_updated") Then
                                If txtExtra(lCount).Text = "True" Or txtExtra(lCount).Text = "False" Then
                                    m_vExtras(ACExtraValue, lCount) = IIf(txtExtra(lCount).Text = "True", 1, 0)
                                Else
                                    If Not cboLookupExtra Is Nothing Then
                                        For Each cmblkp As PMLookupControl.cboPMLookup In cboLookupExtra
                                            If Not cmblkp Is Nothing Then
                                                If cmblkp.TableName = kRuleTypeCaption Then
                                                    Dim nSelectedItemValue As Integer = DirectCast(cmblkp.cboTypeTable.SelectedItem, Microsoft.VisualBasic.Compatibility.VB6.ListBoxItem).ItemData
                                                    If nSelectedItemValue = 3 Then
                                                        m_vExtras(ACExtraValue, lCount) = UctCompiledRule1.Text
                                                        m_vExtras(ACExtraValue, ACExtraType) = 1
                                                    ElseIf nSelectedItemValue = 1 Then
                                                        m_vExtras(ACExtraValue, lCount) = txtExtra(2).Text
                                                        m_vExtras(ACExtraValue, ACExtraType) = 2
                                                    End If
                                                Else
                                                    m_vExtras(ACExtraValue, lCount) = txtExtra(lCount).Text
                                                End If
                                            Else
                                                m_vExtras(ACExtraValue, lCount) = txtExtra(lCount).Text
                                            End If
                                        Next
                                    Else
                                        m_vExtras(ACExtraValue, lCount) = txtExtra(lCount).Text
                                    End If
                                End If
                            End If
                            ' Lookup
                        Case MaintainConst.ACExtraControlType.ACExtraTypeLookup
                            ' Else, get it from the combo box
                            If cboLookupExtra(lCount).ItemId = 0 Then
                                m_vExtras(ACExtraValue, lCount) = ""
                            Else
                                m_vExtras(ACExtraValue, lCount) = cboLookupExtra(lCount).ItemId
                            End If

                            ' Check box
                        Case MaintainConst.ACExtraControlType.ACExtraTypeCheckBox

                            'we need to get value from dynamic check box
                            m_vExtras(ACExtraValue, lCount) = chkExtra(DynamicCheckBoxCount).CheckState
                            DynamicCheckBoxCount += 1

                    End Select

                Next lCount

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            'LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProperties", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetFieldValidation
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function SetFieldValidation() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Code
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCode, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lFieldType:=gPMConstants.PMEDataType.PMString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Description
            'DAK251199
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDescription, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lFieldType:=gPMConstants.PMEDataType.PMString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DC 27/07/00 Do not set to Date & Time just Date only
            ' Effective Date
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtEffectiveDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lFieldType:=gPMConstants.PMEDataType.PMDate, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetFieldValidation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisableForm
    '
    ' Description: Disables controls on the form (for View mode).
    '              There's no need to be able re-enable them.
    '
    ' ***************************************************************** '
    Private Function DisableForm() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ' Developer Guide No. 288
            Dim arrControl As ArrayList = New ArrayList()
            ControlList(Me, arrControl)


            ' Developer Guide No. 288
            For oControl As Integer = 0 To arrControl.Count - 1

                If TypeOf arrControl(oControl) Is TextBox Then
                    arrControl(oControl).Enabled = False
                ElseIf TypeOf arrControl(oControl) Is ComboBox Then
                    arrControl(oControl).Enabled = False
                ElseIf TypeOf arrControl(oControl) Is Label Then
                    arrControl(oControl).Enabled = False
                ElseIf TypeOf arrControl(oControl) Is CheckBox Then
                    arrControl(oControl).Enabled = False
                End If


            Next oControl

            'DAK080200
            If cmdLinkObject.Visible Then
                cmdLinkObject.Enabled = True
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DisableForm Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableForm", excep:=excep)

            Return result

        End Try
    End Function
    'Get The all controls from the given control
    ' Developer Guide No.288 
    Private Sub ControlList(ByVal cControl As Control, ByRef arrControlList As ArrayList)

        If cControl.HasChildren Then
            For i As VariantType = 0 To cControl.Controls.Count - 1
                If Not cControl.Controls(i).HasChildren Then
                    arrControlList.Add(cControl.Controls(i))
                Else
                    For k As VariantType = 0 To cControl.Controls(i).Controls.Count - 1
                        ControlList(cControl.Controls(i).Controls(k), arrControlList)
                    Next
                End If
            Next
        Else
            arrControlList.Add(cControl)
        End If

    End Sub
    ' ***************************************************************** '
    '
    ' Name: CheckPrivileges
    '
    ' Description:
    '
    ' History: 01/12/1999 DAK - Created.
    '
    ' ***************************************************************** '
    Private Function CheckPrivileges() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Select Case PrivilegeLevel
                Case gPMConstants.PMELookupEditPrivlegeLevel.PMLookupNoEdit
                    m_lReturn = DisableForm()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New Exception()
                    End If
                    If cmdLinkObject.Visible Then
                        cmdLinkObject.Enabled = False
                    End If

                Case gPMConstants.PMELookupEditPrivlegeLevel.PMLookupViewOnly
                    m_lReturn = DisableForm()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New Exception()
                    End If

                Case gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAmendCaptions
                    m_lReturn = DisableForm()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New Exception()
                    End If
                    lblDescription.Enabled = True
                    txtDescription.Enabled = True

                Case gPMConstants.PMELookupEditPrivlegeLevel.PMLookupFullPrivileges
                    '
                Case gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAdminViewUserNone
                    m_lReturn = DisableForm()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New Exception()
                    End If
                    If PMAuthorityLevel <> gPMConstants.PMEAuthorityLevel.pmeALSysAdmin Then
                        If cmdLinkObject.Visible Then
                            cmdLinkObject.Enabled = False
                        End If
                    End If

                Case gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAdminCaptionsUserNone
                    m_lReturn = DisableForm()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New Exception()
                    End If
                    If PMAuthorityLevel = gPMConstants.PMEAuthorityLevel.pmeALSysAdmin Then
                        lblDescription.Enabled = True
                        txtDescription.Enabled = True
                    Else
                        If cmdLinkObject.Visible Then
                            cmdLinkObject.Enabled = False
                        End If
                    End If

                Case gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAdminCaptionsUserView
                    m_lReturn = DisableForm()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New Exception()
                    End If
                    If PMAuthorityLevel = gPMConstants.PMEAuthorityLevel.pmeALSysAdmin Then
                        lblDescription.Enabled = True
                        txtDescription.Enabled = True
                    End If

                Case gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAdminFullUserNone
                    If PMAuthorityLevel <> gPMConstants.PMEAuthorityLevel.pmeALSysAdmin Then
                        m_lReturn = DisableForm()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New Exception()
                        End If
                        If cmdLinkObject.Visible Then
                            cmdLinkObject.Enabled = False
                        End If
                    End If

                Case gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAdminFullUserView
                    If PMAuthorityLevel <> gPMConstants.PMEAuthorityLevel.pmeALSysAdmin Then
                        m_lReturn = DisableForm()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New Exception()
                        End If
                    End If

                Case gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAdminFullUserCaptions
                    If PMAuthorityLevel <> gPMConstants.PMEAuthorityLevel.pmeALSysAdmin Then
                        m_lReturn = DisableForm()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New Exception()
                        End If
                        lblDescription.Enabled = True
                        txtDescription.Enabled = True
                    End If

                Case Else
                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckPrivileges Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckPrivileges", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Start
    '
    ' Description: Sets the properties and displays the form
    '
    ' ***************************************************************** '
    Public Function Start() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try
            result = gPMConstants.PMEReturnCode.PMTrue


            'DAK251199
            m_iStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to the hourglass
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Create a new instance of form fields
            m_oFormFields = New iPMFormControl.FormFields()

            'DAK080500
            g_bHasExtras = Information.IsArray(m_vExtras)

            m_lReturn = SetFieldValidation()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Set the mouse pointer to the default
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            ' Set the properties on the form
            m_lReturn = SetProperties()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Set the mouse pointer to the hourglass
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            'DAK080200 - ensure privileges are checked
            m_lReturn = CheckPrivileges()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            ' Disable the controls if in view mode
            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                m_lReturn = DisableForm()
            End If

            ' Set the mouse pointer to the hourglass
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Display the form

            Me.ShowDialog()

            'DAK080200 - only update if in Add or Edit mode
            If m_iStatus = gPMConstants.PMEReturnCode.PMOK And (m_iTask = gPMConstants.PMEComponentAction.PMAdd Or m_iTask = gPMConstants.PMEComponentAction.PMEdit) Then
                ' Get the values off the form
                m_lReturn = GetProperties()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Unload the extra txt's and lbl's we may have
            If g_bHasExtras Then
                m_lReturn = UnloadDynamicControls()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Terminate form control
            m_oFormFields.Dispose()

            ' Remove the instance of form control
            m_oFormFields = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UnloadDynamicControls
    '
    ' Description: Unloads all the extra dynamic controls from memory
    '
    ' ***************************************************************** '
    Public Function UnloadDynamicControls() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Me.Controls.Count > 3 Then
                ' Loop through and unload all the extra controls
                For Each ctl As Control In Me.Controls(3).Controls(0).Controls(0).Controls(0).Controls
                    If (ctl.Name <> "lblCode") AndAlso (ctl.Name <> "lblDescription") AndAlso (ctl.Name <> "lblEffectiveDate") AndAlso (ctl.Name <> "_lblExtra_0") AndAlso (ctl.Name <> "_chkExtra_0") AndAlso (ctl.Name <> "_cboLookupExtra_0") AndAlso (ctl.Name <> "_txtExtra_0") AndAlso (ctl.Name <> "txtEffectiveDate") AndAlso (ctl.Name <> "txtDescription") AndAlso (ctl.Name <> "txtCode") AndAlso (ctl.Name <> "VScroll1") Then
                        Me.Controls(3).Controls(0).Controls(0).Controls(0).Controls.Remove(ctl)
                    End If
                Next
            End If
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnloadDynamicControls Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnloadDynamicControls", excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cboLookupExtra_ClickEvent(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cboLookupExtra_0.Click
        Dim Index As Integer = Array.IndexOf(cboLookupExtra, eventSender)
        If m_sTableName = "rating_section_type" Then
            If Index = 4 And cboLookupExtra.Length > 4 Then
                If cboLookupExtra.GetUpperBound(0) = 5 AndAlso (Not cboLookupExtra(Index + 1) Is Nothing) Then
                    cboLookupExtra(Index + 1).WhereClause = " country_id=" & cboLookupExtra(Index).ItemId
                    cboLookupExtra(Index + 1).TableName = "state"
                    cboLookupExtra(Index + 1).RefreshList()
                End If
            End If
        ElseIf m_sTableName = "Debtor_User_Groups" Then

            If nIsPaymentTypeClaimPaymentIndex >= 0 Then
                If cboLookupExtra(Index).TableName = "Debtor_User_Groups_Type" Then
                    If cboLookupExtra(Index).ItemCaption <> "Payments" Then
                        chkExtra(nIsPaymentTypeClaimPaymentIndex).Visible = False
                    Else
                        chkExtra(nIsPaymentTypeClaimPaymentIndex).Visible = True
                        chkExtra(nIsPaymentTypeClaimPaymentIndex).Checked = True
                    End If
                End If
            End If
        ElseIf m_sTableName = "Tax_Group" Then
            If Index = 1 Then
                Dim nSelectedItemValue As Integer = DirectCast(cboLookupExtra(Index).cboTypeTable.SelectedItem, Microsoft.VisualBasic.Compatibility.VB6.ListBoxItem).ItemData
                If nSelectedItemValue = 3 Then
                    btnFile.Visible = False
                    UctCompiledRule1.Visible = True
                    UctCompiledRule1.Load()
                    UctCompiledRule1.bEnterOnlyAssemblyName = False
                    If (txtExtra.Length > 2) Then
                        txtExtra(2).Visible = False
                        lblExtra(2).Text = "Compiled Rule Assembly:"
                        UctCompiledRule1.Text = ""
                    End If
                ElseIf nSelectedItemValue = 1 Then
                    btnFile.Visible = True
                    UctCompiledRule1.Visible = False
                    If (txtExtra.Length > 2) Then
                        txtExtra(2).Visible = True
                        txtExtra(2).Enabled = False
                        txtExtra(2).Text = "ATS.Rul"
                        lblExtra(2).Text = "Advanced Tax Script:"
                    End If
                Else
                    btnFile.Visible = False
                    UctCompiledRule1.Visible = False
                    If (txtExtra.Length > 2) Then
                        txtExtra(2).Visible = True
                        txtExtra(2).Enabled = False
                        lblExtra(2).Text = "Advanced Tax Script:"
                        txtExtra(2).Text = ""
                    End If
                End If
            End If

        End If
        '' start 68551
        If m_bMediaTypeMaintenanceTask And Index = m_iMediaTypeValidationIDIndex Then
            EnableDisableAdditionalDetailsMediaType()
        End If
        '' End 68551
    End Sub

    Private Sub chkExtra_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _chkExtra_0.CheckStateChanged
        Dim Index As Integer = Array.IndexOf(chkExtra, eventSender)
        '**************************************************************************************************************
        'PN28188If is include tax in instalment is unselected, is spread tax across instalment will be unselected and disabled
        '**************************************************************************************************************
        If chkExtra(Index).Text = "Is Include Tax In Instalments:" And chkExtra(Index).CheckState = CheckState.Unchecked Then
            chkExtra(Index + 1).Enabled = False
            chkExtra(Index + 1).CheckState = CheckState.Unchecked
        ElseIf chkExtra(Index).Text = "Is Include Tax In Instalments:" And chkExtra(Index).CheckState = CheckState.Checked Then
            If chkExtra.Length > Index + 1 Then
                chkExtra(Index + 1).Enabled = True
                chkExtra(Index + 1).CheckState = CheckState.Checked
            End If
            'Start - Sankar - (WR29 - Stamp Duty Process) - Paralleling
        ElseIf chkExtra(Index).Text = "Is Stamp Duty Insurer:" And chkExtra(Index).CheckState = CheckState.Checked Then
            For i As Integer = 0 To chkExtra.Length - 1
                If Not (chkExtra(i) Is Nothing) Then
                    If chkExtra(i).Text = "Is Stamp Duty Insured:" Or chkExtra(i).Text = "Is Levy Tax:" Or chkExtra(i).Text = "Is Taxed:" Or chkExtra(i).Text = "Is Sum Insured:" Or chkExtra(i).Text = "Is Premium:" Then
                        chkExtra(i).CheckState = CheckState.Unchecked
                    End If
                End If
            Next
            For i_2 As Integer = 0 To cboLookupExtra.Length - 1
                If Not (cboLookupExtra(i_2) Is Nothing) Then
                    If cboLookupExtra(i_2).TableName.ToUpper() = "TAX_GROUP" Then
                        cboLookupExtra(i_2).ListIndex = 0
                        Exit For
                    End If
                    If cboLookupExtra(i_2).TableName = "Commission_Band" Then
                        lblExtra(2).Font = VB6.FontChangeBold(lblExtra(2).Font, False)
                        Exit For
                    End If
                End If
            Next
        ElseIf chkExtra(Index).Text = "Is Stamp Duty Insured:" And chkExtra(Index).CheckState = CheckState.Checked Then
            For i_3 As Integer = 0 To chkExtra.Length - 1
                If Not (chkExtra(i_3) Is Nothing) Then
                    If chkExtra(i_3).Text = "Is Stamp Duty Insurer:" Or chkExtra(i_3).Text = "Is Levy Tax:" Or chkExtra(i_3).Text = "Is Taxed:" Or chkExtra(i_3).Text = "Is Sum Insured:" Or chkExtra(i_3).Text = "Is Premium:" Then
                        chkExtra(i_3).CheckState = CheckState.Unchecked
                    End If
                End If
            Next
            For i_4 As Integer = 0 To cboLookupExtra.Length - 1
                If Not (cboLookupExtra(i_4) Is Nothing) Then
                    If cboLookupExtra(i_4).TableName.ToUpper() = "TAX_GROUP" Then
                        cboLookupExtra(i_4).ListIndex = 0
                        Exit For
                    End If
                    If cboLookupExtra(i_4).TableName = "Commission_Band" Then
                        lblExtra(2).Font = VB6.FontChangeBold(lblExtra(2).Font, False)
                        Exit For
                    End If
                End If
            Next
        ElseIf chkExtra(Index).Text = "Is Levy Tax:" And chkExtra(Index).CheckState = CheckState.Checked Then
            For i_5 As Integer = 0 To chkExtra.Length - 1
                If Not (chkExtra(i_5) Is Nothing) Then
                    If chkExtra(i_5).Text = "Is Stamp Duty Insurer:" Or chkExtra(i_5).Text = "Is Stamp Duty Insured:" Then
                        chkExtra(i_5).CheckState = CheckState.Unchecked
                    End If
                End If
            Next
        ElseIf chkExtra(Index).Text = "Is Sum Insured:" And chkExtra(Index).CheckState = CheckState.Checked Then
            For i_6 As Integer = 0 To chkExtra.Length - 1
                If Not (chkExtra(i_6) Is Nothing) Then
                    If chkExtra(i_6).Text = "Is Stamp Duty Insurer:" Or chkExtra(i_6).Text = "Is Stamp Duty Insured:" Then
                        chkExtra(i_6).CheckState = CheckState.Unchecked
                    End If
                End If
            Next
        ElseIf chkExtra(Index).Text = "Is Premium:" And chkExtra(Index).CheckState = CheckState.Checked Then
            For i_7 As Integer = 0 To chkExtra.Length - 1
                If Not (chkExtra(i_7) Is Nothing) Then
                    If chkExtra(i_7).Text = "Is Stamp Duty Insurer:" Or chkExtra(i_7).Text = "Is Stamp Duty Insured:" Then
                        chkExtra(i_7).CheckState = CheckState.Unchecked
                    End If
                End If
            Next
        ElseIf chkExtra(Index).Text = "Is Taxed:" And chkExtra(Index).CheckState = CheckState.Checked Then
            For i_8 As Integer = 0 To chkExtra.Length - 1
                If Not (chkExtra(i_8) Is Nothing) Then
                    If chkExtra(i_8).Text = "Is Stamp Duty Insurer:" Or chkExtra(i_8).Text = "Is Stamp Duty Insured:" Then
                        chkExtra(i_8).CheckState = CheckState.Unchecked
                    End If
                End If
            Next
            'End - Sankar - (WR29 - Stamp Duty Process) - Paralleling
        ElseIf chkExtra(Index).Text = "Is Stamp Duty Insurer:" And chkExtra(Index).CheckState <> CheckState.Checked Then

            For i_9 As Integer = 0 To cboLookupExtra.Length - 1
                If Not (cboLookupExtra(i_9) Is Nothing) Then

                    If cboLookupExtra(i_9).TableName = "Commission_Band" Then
                        lblExtra(2).Font = VB6.FontChangeBold(lblExtra(2).Font, True)
                        Exit For
                    End If
                End If
            Next
        ElseIf chkExtra(Index).Text = "Is Stamp Duty Insured:" And chkExtra(Index).CheckState <> CheckState.Checked Then
            For i_10 As Integer = 0 To cboLookupExtra.Length - 1
                If Not (cboLookupExtra(i_10) Is Nothing) Then
                    If cboLookupExtra(i_10).TableName = "Commission_Band" Then
                        lblExtra(2).Font = VB6.FontChangeBold(lblExtra(2).Font, True)
                        Exit For
                    End If
                End If
            Next
        End If

        'Start - Sankar - (WPRvb64 Media Type Status) - Paralleling
        If m_bMediaTypeMaintenanceTask And Index = m_iIsReceiptControlIndex Then
            SetupMediaTypeStatusControl()
            EnableDisableAdditionalDetailsMediaType() ''68551
        End If
        'End - Sankar - (WPRvb64 Media Type Status) - Paralleling
    End Sub

    'Start - Sankar - (WPRvb64 Media Type Status) - Paralleling
    Private Sub SetupMediaTypeStatusControl()
        If chkExtra(m_iIsReceiptControlIndex).CheckState = CheckState.Checked Then
            lblExtra(m_iMediaTypeStatusControlIndex).Font = VB6.FontChangeBold(lblExtra(m_iMediaTypeStatusControlIndex).Font, True)
            lblExtra(m_iMediaTypeStatusControlIndex).Enabled = True
            cboLookupExtra(m_iMediaTypeStatusControlIndex).Enabled = True
            'Make the media type status combo mandatory
            m_vExtras(ACExtraOffset, m_iMediaTypeStatusControlIndex) = 1
        Else
            lblExtra(m_iMediaTypeStatusControlIndex).Font = VB6.FontChangeBold(lblExtra(m_iMediaTypeStatusControlIndex).Font, False)
            lblExtra(m_iMediaTypeStatusControlIndex).Enabled = False
            cboLookupExtra(m_iMediaTypeStatusControlIndex).Enabled = False
            cboLookupExtra(m_iMediaTypeStatusControlIndex).ListIndex = 0
            'Make the media type status combo optional
            m_vExtras(ACExtraOffset, m_iMediaTypeStatusControlIndex) = -1
        End If
    End Sub
    'End - Sankar - (WPRvb64 Media Type Status) - Paralleling

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        Me.Hide()

    End Sub

    ' ********************************************************************** '
    ' Name: CheckExtraControls
    '
    ' Description: An extension to Form Fields. Checks our own extra fields
    '
    ' ********************************************************************** '
    Private Function CheckExtraControls() As gPMConstants.PMEReturnCode

        Dim result As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim sText, sMsg As String
        'Developer Guide No. 101
        Dim vConvert As Object
        Dim bIsSourceAssociatedWithBank As Boolean
        Dim lBankAccountID, lSourceID As Integer
        Dim sObjectName As String = String.Empty
        Dim oRules As Object = Nothing
        Dim sRulePath As String = ""
        Dim sAssemblyName As String = ""
        Dim sSubKey As String = "GIS"

        If g_bHasExtras Then

            ' Check for empty fields

            For lCount As Integer = m_vExtraTypes.GetLowerBound(0) To m_vExtraTypes.GetUpperBound(0)

                If CInt(m_vExtras(ACExtraOffset, lCount)) >= 0 Then


                    Select Case m_vExtraTypes(lCount)
                        Case MaintainConst.ACExtraControlType.ACExtraTypeTextBox

                            sText = txtExtra(lCount).Text.Trim()
                            'TFS 6331 PIE GUid and PIE date should not be mandatory
                            If Not (CStr(m_vExtras(ACExtraCaption, lCount)).ToLower() = "pie_guid" Or CStr(m_vExtras(ACExtraCaption, lCount)).ToLower() = "pie_last_updated") And sText = "" Then
                                ' Message box
                                sMsg = "The field '" & _
                                       CStr(m_vExtras(ACExtraCaption, lCount)) & "' is mandatory."
                                MessageBox.Show(sMsg, "Mandatory Field", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                ' Set the focus to that control
                                txtExtra(lCount).Focus()
                                ' Exit

                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                        Case MaintainConst.ACExtraControlType.ACExtraTypeLookup
                            If cboLookupExtra(lCount).ItemId = 0 And CStr(m_vExtras(ACExtraCaption, lCount)) <> "lead_commission_band" Then
                                ' Show a message box
                                'Start - Sankar - (WPRvb64 Media Type Status) - Paralleling
                                If cboLookupExtra(lCount).TableName = csMediaTypeStatusTable Then
                                    'create the error message with customized field name
                                    sMsg = "The field '" & csMediaTypeStatusCaption & "' is mandatory."
                                Else
                                    sMsg = "The field '" & _
                                           CStr(m_vExtras(ACExtraCaption, lCount)) & "' is mandatory."
                                End If
                                'End - Sankar - (WPRvb64 Media Type Status) - Paralleling
                                MessageBox.Show(sMsg, "Mandatory Field", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                ' Set the focus to that control
                                cboLookupExtra(lCount).Focus()
                                ' Exit
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                        Case MaintainConst.ACExtraControlType.ACExtraTypeCheckBox
                            ' Dont do anything here, because empty checkboxes
                            ' are just as good as checked ones

                    End Select

                End If

            Next lCount

            ' Now check that text boxes match up to the correct type

            For lCount As Integer = m_vExtraTypes.GetLowerBound(0) To m_vExtraTypes.GetUpperBound(0)

                If m_vExtraTypes(lCount) = MaintainConst.ACExtraControlType.ACExtraTypeTextBox Then

                    sText = txtExtra(lCount).Text.Trim()

                    ' Check its in the right format (number, date etc...)

                    Select Case CStr(m_vExtras(ACExtraType, lCount)).ToUpper()
                        Case "TINYINT", "INT", "SMALLINT", "NUMERIC"

                            Dim dbNumericTemp As Double
                            If (Not Double.TryParse(sText, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) And (sText <> "") Then
                                sMsg = "The field '" & _
                                       CStr(m_vExtras(ACExtraCaption, lCount)) & "' must have a numeric value."
                                MessageBox.Show(sMsg, "Numeric Field", MessageBoxButtons.OK, MessageBoxIcon.Error)

                                ' Set to false and exist
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                        Case "BIT"
                            If sText <> "False" And sText <> "True" Then
                                sMsg = "The field '" & _
                                       CStr(m_vExtras(ACExtraCaption, lCount)) & "' must have a True/False value."
                                MessageBox.Show(sMsg, "Boolean Field", MessageBoxButtons.OK, MessageBoxIcon.Error)

                                ' Set to false and exist
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If


                        Case "DATETIME"
                            If Not Information.IsDate(gPMFunctions.UnFormatField(gPMConstants.PMEFormatStyle.PMFormatDateLong, gPMConstants.PMEDataType.PMDate, sText)) Then
                                sMsg = "The field '" & _
                                       CStr(m_vExtras(ACExtraCaption, lCount)) & "' must have a date/time value."

                                MessageBox.Show(sMsg, "DateTime Field", MessageBoxButtons.OK, MessageBoxIcon.Error)

                                ' Set to false and exist
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                    End Select

                    'DJM 08/03/2004
                    'Check that the number is valid for the SQL data type.
                    sMsg = ""
                    Select Case CStr(m_vExtras(ACExtraType, lCount)).ToUpper()
                        Case "TINYINT"
                            If sText <> "" Then

                                vConvert = Nothing
                                Try
                                    vConvert = CByte(sText)

                                Catch
                                End Try



                                If Convert.IsDBNull(vConvert) Or IsNothing(vConvert) Or vConvert <> CDbl(sText) Then
                                    sMsg = "The field '" & CStr(m_vExtras(ACExtraCaption, lCount)) & "' must have a whole number" & Strings.Chr(13) & Strings.Chr(10) & _
                                           "between (0) and (255)."
                                End If
                            End If
                        Case "INT"
                            If sText <> "" Then

                                vConvert = Nothing
                                Try
                                    vConvert = CInt(sText)

                                Catch
                                End Try



                                If Convert.IsDBNull(vConvert) Or IsNothing(vConvert) Or vConvert <> CDbl(sText) Then
                                    sMsg = "The field '" & CStr(m_vExtras(ACExtraCaption, lCount)) & "' must have a whole number" & Strings.Chr(13) & Strings.Chr(10) & _
                                           "between (-2,147,483,648) and (2,147,483,647)."
                                End If
                            End If
                        Case "BIT"
                            If sText <> "" Then

                                vConvert = Nothing
                                Try
                                    vConvert = ToSafeBoolean(sText)

                                Catch
                                End Try



                                If Convert.IsDBNull(vConvert) Or IsNothing(vConvert) Or (Not vConvert And vConvert) Then
                                    sMsg = "The field '" & CStr(m_vExtras(ACExtraCaption, lCount)) & "' must have a whole number" & Strings.Chr(13) & Strings.Chr(10) & _
                                           "between (0) and (1)."
                                End If
                            End If
                        Case "SMALLINT"
                            If sText <> "" Then

                                vConvert = Nothing
                                Try
                                    vConvert = CShort(sText)

                                Catch
                                    vConvert = Nothing
                                End Try

                                If Convert.IsDBNull(vConvert) Or IsNothing(vConvert) Or vConvert <> CDbl(sText) Then
                                    sMsg = "The field '" & CStr(m_vExtras(ACExtraCaption, lCount)) & "' must have a whole number" & Strings.Chr(13) & Strings.Chr(10) & _
                                           "between (-32,768) and (32,767)."
                                End If
                            End If
                        Case "NUMERIC"
                            If sText <> "" Then

                                vConvert = Nothing
                                Try
                                    vConvert = CDbl(sText)

                                Catch
                                End Try



                                If Convert.IsDBNull(vConvert) Or IsNothing(vConvert) Or CStr(IIf(vConvert > 0, Math.Floor(vConvert), Math.Ceiling(vConvert))).Length > (CDbl(CDbl(m_vExtras(ACExtraPrecision, lCount)) - CDbl(m_vExtras(ACExtraScale, lCount)))) Or CStr(vConvert).Length - (CStr(IIf(vConvert > 0, Math.Floor(vConvert), Math.Ceiling(vConvert))).Length + 1) > (CDbl(m_vExtras(ACExtraScale, lCount))) Then
                                    sMsg = "The field '" & CStr(m_vExtras(ACExtraCaption, lCount)) & "' must have a numeric value" & Strings.Chr(13) & Strings.Chr(10) & _
                                           "with no more than (" & (CStr(CDbl(m_vExtras(ACExtraPrecision, lCount)) - CDbl(m_vExtras(ACExtraScale, lCount)))) & _
                                           ") numbers before the decimal place and no more than (" & (CStr(m_vExtras(ACExtraScale, lCount))) & _
                                           ") numbers after the decimal place."
                                End If
                            End If

                    End Select

                    If sMsg <> "" Then
                        MessageBox.Show(sMsg, "Numeric Field", MessageBoxButtons.OK, MessageBoxIcon.Error)

                        ' Set to false and exist
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

            Next lCount
            If m_sTableName = "Tax_Group" Then

                For Each cmblkp As PMLookupControl.cboPMLookup In cboLookupExtra
                    If Not cmblkp Is Nothing Then
                        If cmblkp.TableName = kRuleTypeCaption Then
                            Dim nSelectedItemValue As Integer = DirectCast(cmblkp.cboTypeTable.SelectedItem, Microsoft.VisualBasic.Compatibility.VB6.ListBoxItem).ItemData
                            Select Case nSelectedItemValue
                                Case CompiledRules
                                    If UctCompiledRule1.Text = "" Then
                                        MessageBox.Show("Please enter the Rating assembly and class name.", "Compiled Rules", MessageBoxButtons.OK)
                                        Return gPMConstants.PMEReturnCode.PMFalse
                                    End If

                                    sObjectName = UctCompiledRule1.Text

                                    m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="RulePath", r_sSettingValue:=sRulePath, v_sSubKey:=sSubKey), gPMConstants.PMEReturnCode)

                                    If sRulePath <> "" Then
                                        If Not sRulePath.EndsWith("\") Then
                                            sRulePath = sRulePath & "\" & sObjectName
                                        End If
                                    End If

                                    If sRulePath.Length > 255 Then
                                        MessageBox.Show("The total length of rule folder path and Assembly.Class name should not exceed 255 characters.", "Compiled Rules", MessageBoxButtons.OK)
                                        Return gPMConstants.PMEReturnCode.PMFalse
                                    End If

                                    Try
                                        oRules = CreateLateBoundObject_CompiledRules(sObjectName)

                                        If Not IsNothing(oRules) Then
                                            MessageBox.Show(sObjectName + " validated successfully.", "Compiled Rules", MessageBoxButtons.OK)
                                        End If

                                    Catch ex As Exception
                                        oRules = Nothing
                                    End Try

                                    If Not String.IsNullOrEmpty(sObjectName) Then
                                        sAssemblyName = sObjectName.Split(".")(0)
                                    End If

                                    If IsNothing(oRules) Then
                                        MessageBox.Show("Unable to find compiled rule class " + sObjectName + ". Please ensure the " + sAssemblyName + " assembly is in the Rules folder, and the class name is correct. " +
                                       "The format should be assemblyname.classname.", "Compiled Rules", MessageBoxButtons.OK)
                                        Return gPMConstants.PMEReturnCode.PMFalse
                                    End If
                                Case VBScriptRules
                                    Try
                                        ' Process the next set of actions depending
                                        ' upon the interface task etc.
                                        m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

                                        ' Check the return value.
                                        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                                            ' Everything OK, so we can hide the interface.
                                            result = gPMConstants.PMEReturnCode.PMTrue
                                            Me.Hide()
                                        End If
                                    Catch ex As Exception
                                        oRules = Nothing
                                    End Try
                            End Select
                        End If
                    End If
                Next
            End If
        End If
        If m_sTableName = "PMWrk_websites" Then
            ' If (UCase(Left(Trim(txtCode.Text), 3)) <> "WEB") Then
            If (UCase(RTrim(LTrim(txtCode.Text)).Substring(0, 3)) <> "WEB") Then


                MsgBox("First three alphabets of the code should be WEB.", vbExclamation, "Wrong Code Entry")
                CheckExtraControls = gPMConstants.PMEReturnCode.PMFalse
                txtCode.Focus()
                Exit Function
            End If

        End If
        If m_sTableName = "Catastrophe_Code" Then 'PN 39288 and PN 39292
            If (Information.IsDate(txtExtra(0).Text) Or Information.IsDate(txtExtra(1).Text)) And (txtExtra(0).Text.Trim() = "" Or txtExtra(1).Text.Trim() = "") Then
                MessageBox.Show("Please enter both From and To Dates or blank dates will be accepted", "Date Field", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Information.IsDate(txtExtra(0).Text) And Information.IsDate(txtExtra(1).Text) Then
                If ToSafeDate(txtExtra(0).Text) > ToSafeDate(txtExtra(1).Text) Then
                    MessageBox.Show("From Date should be less than To Date", "Date Field", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If

        'Check Product Family. This is valid for product family pmePFOrion
        ' and table should be BankAccount_Default (Task: Account Function & CCY Cash Allocation)
        If ProductFamily <> gPMConstants.PMEProductFamily.pmePFOrion Then
            Return result
        End If

        If m_sTableName.ToUpper() <> ("BankAccount_Default").ToUpper() Then
            Return result
        End If

        For lCount As Integer = m_vExtras.GetLowerBound(1) To m_vExtras.GetUpperBound(1)
            If CStr(m_vExtras(0, lCount)).ToUpper() = ("bankaccount_id").ToUpper() Then
                lBankAccountID = cboLookupExtra(lCount).ItemId
                Exit For
            End If
        Next

        For lCount As Integer = m_vExtras.GetLowerBound(1) To m_vExtras.GetUpperBound(1)
            If CStr(m_vExtras(0, lCount)).ToUpper() = ("source_id").ToUpper() Then
                lSourceID = cboLookupExtra(lCount).ItemId

                m_lReturn = CheckBranchAssociatedWithBank(v_lBankAccountID:=lBankAccountID, v_lSourceID:=lSourceID, r_bIsSourceAssociatedWithBank:=bIsSourceAssociatedWithBank)

                If Not bIsSourceAssociatedWithBank Then
                    MessageBox.Show("Selected Branch is not Linked with Bank Account.", "Source Field", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    cboLookupExtra(lCount).Focus()
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If

                Exit For
            End If
        Next

        Return result

Err_CheckExtraControls:

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckExtraControls Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckExtraControls", excep:=New Exception(Information.Err().Description))

        Return result

    End Function

    Private Sub cmdLinkObject_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdLinkObject.Click
        Dim oSetKeyArray(,) As Object
        Dim nTxtExtra As Integer = 0
        Dim nLookupExtra As Integer = 0
        Dim iChkExtra As Integer = 0

        ReDim oSetKeyArray(1, 4)
        'DAK031299 - Set the key values
        oSetKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = IDColumnName
        oSetKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = IDColumnValue

        oSetKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = "code"
        oSetKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = Code

        oSetKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = "description"
        oSetKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = Description

        oSetKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = "Task" 'MKW010803 PN4514
        oSetKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = m_iTask 'MKW010803 PN4514

        oSetKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = "effective_date"
        oSetKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = EffectiveDate

        'Extras
        If g_bHasExtras Then
            For iCount As Integer = m_vExtras.GetLowerBound(1) To m_vExtras.GetUpperBound(1)

                ReDim Preserve oSetKeyArray(1, iCount + 5)
                oSetKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iCount + 5) = m_vExtras(ACExtraCaption, iCount)


                Select Case m_vExtraTypes(iCount)
                    Case MaintainConst.ACExtraControlType.ACExtraTypeTextBox
                        If txtExtra(nTxtExtra) IsNot Nothing Then
                            oSetKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iCount + 5) = txtExtra(nTxtExtra).Text
                        End If
                        nTxtExtra = nTxtExtra + 1

                    Case MaintainConst.ACExtraControlType.ACExtraTypeLookup
                        If cboLookupExtra(nLookupExtra) IsNot Nothing Then
                            oSetKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iCount + 5) = cboLookupExtra(nLookupExtra).ItemId
                        End If
                        nLookupExtra = nLookupExtra + 1

                    Case MaintainConst.ACExtraControlType.ACExtraTypeCheckBox
                        'Due to the way dynamic checkboxes are now added, they are always at -1 from the other dynamic controls' index
                        If chkExtra(iChkExtra) IsNot Nothing Then
                            oSetKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iCount + 5) = chkExtra(iChkExtra).CheckState
                        End If
                        iChkExtra = iChkExtra + 1

                    Case Else
                        '

                End Select

            Next iCount
        End If

        m_sUniqueId = GetUniqueID()
        ReDim Preserve oSetKeyArray(1, oSetKeyArray.GetUpperBound(1) + 2)
        oSetKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, oSetKeyArray.GetUpperBound(1) - 1) = "UniqueId"
        oSetKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, oSetKeyArray.GetUpperBound(1) - 1) = m_sUniqueId

        oSetKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, oSetKeyArray.GetUpperBound(1)) = "ScreenHierarchy"
        oSetKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, oSetKeyArray.GetUpperBound(1)) = ReformatText(m_sTableName) + " (" + Code.ToString + ")"

        RaiseEvent LaunchLinkedObject(oSetKeyArray)

    End Sub


    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
        'dim v_Num as Long
        ' PW030702 - add doevents in here. This gets rid of the problem where
        '            if the user uses the shorcut key to OK (Alt-O), the code
        '            crashes.
        '            This was happening because the lostfocus event of the
        '            current field was not being fired before the modal form
        '            surrendered control to the calling code. i.e the Start
        '            method.
        Application.DoEvents()

        Dim dbNumericTemp As Double
        If txtCode.Text.Trim().ToString <> "" And m_sTableName.ToLower = "gis_data_model" Then
            If Double.TryParse(txtCode.Text.Trim().Substring(0, 1), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                MessageBox.Show("The Code must not begin with a numeric for Data Models.", "Invalid Code", MessageBoxButtons.OK, MessageBoxIcon.Information)
                txtCode.SelectionStart = 0
                txtCode.SelectionLength = txtCode.Text.Trim().Length
                txtCode.Focus()
                Exit Sub
            End If
        End If

        ' Check mandatory fields
        m_lReturn = m_oFormFields.CheckMandatoryControls()

        Dim iCountStampDuty As Integer = 0
        For i As Integer = 0 To chkExtra.Length - 1
            If Not (chkExtra(i) Is Nothing) Then
                If chkExtra(i).Text = "Is Stamp Duty Insured:" Or chkExtra(i).Text = "Is Stamp Duty Insurer:" Then
                    For i_2 As Integer = 0 To cboLookupExtra.Length - 1
                        If Not (cboLookupExtra(i_2) Is Nothing) Then
                            If cboLookupExtra(i_2).TableName.ToUpper() = "TAX_GROUP" And cboLookupExtra(i_2).ListIndex <> 0 And chkExtra(i).CheckState = CheckState.Checked Then
                                MessageBox.Show("Cannot Select Any Tax Group When Stamp Duty is Enabled", "Peril Type", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                Exit Sub
                            End If
                            If cboLookupExtra(i_2).TableName.ToUpper() = "COMMISSION_BAND" Then
                                If (chkExtra(i).CheckState = CheckState.Checked And iCountStampDuty = 0) Or cboLookupExtra(i_2).ListIndex <> 0 Then
                                    iCountStampDuty = 1
                                End If
                            End If
                        End If
                    Next
                End If
            End If
        Next

        If iCountStampDuty <> 1 And m_sTableName.ToLower = "peril_type" Then
            MessageBox.Show("The field 'Lead Commission Band' is mandatory.", "Mandatory Field", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' If all ok then hide the form
        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

            ' Check any mandatory extra controls that arent controlled by
            ' form control
            m_lReturn = CheckExtraControls()

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                m_iStatus = gPMConstants.PMEReturnCode.PMOK
                Me.Hide()
            End If

        End If

    End Sub

    Private Sub txtCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCode.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtCode)
    End Sub

    Private Sub txtCode_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtCode.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)

        'Only allow numbers and letters for the code.
        Select Case KeyAscii
            Case 13, 10 'CR, LF
            Case 8 'Backspace
            Case 48 To 57 '0-9
            Case 65 To 90, 97 To 122 'A-Z
            Case Else
                KeyAscii = 0
        End Select

        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub txtCode_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCode.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtCode)
    End Sub

    Private Sub txtDescription_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDescription.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtDescription)
    End Sub


    Private Sub txtDescription_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDescription.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtDescription)
    End Sub

    Private Sub txtEffectiveDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtEffectiveDate.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtEffectiveDate)
    End Sub

    Private Sub txtEffectiveDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtEffectiveDate.Leave
        'If it is blank then don't call Lostfocus as it will change it to a stupid date.
        If txtEffectiveDate.Text <> "" Then
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtEffectiveDate)
        End If
    End Sub

    Private Sub txtExtra_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs)
        Dim Index As Integer = Array.IndexOf(txtExtra, eventSender)
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtExtra(Index), vCol:=Index)
    End Sub

    Private Sub txtExtra_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs)
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)

        'disallowing the entry of comma. PN 20133/20139
        If KeyAscii = 44 Then
            KeyAscii = 0
        End If

        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    'Private Sub txtExtra_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _txtExtra_0.Leave
    Private Sub txtExtra_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs)
        Dim Index As Integer = Array.IndexOf(txtExtra, eventSender)
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtExtra(Index), vCol:=Index)
    End Sub



    Private Sub VScroll1_Change(ByVal newScrollValue As Integer)
        picCanvas.Top = VB6.TwipsToPixelsY(-newScrollValue)
        If Me.Visible Then
            picCanvas.Focus()
        End If
    End Sub

    Private Sub VScroll1_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles VScroll1.Enter
        If Me.Visible Then
            picCanvas.Focus()
        End If
    End Sub


    Private Sub VScroll1_Scroll_Renamed(ByVal newScrollValue As Integer)
        picCanvas.Top = VB6.TwipsToPixelsY(-newScrollValue)
        If Me.Visible Then
            picCanvas.Focus()
        End If
    End Sub

    Private Function CheckBranchAssociatedWithBank(ByVal v_lBankAccountID As Integer, ByVal v_lSourceID As Integer, ByRef r_bIsSourceAssociatedWithBank As Boolean) As Integer
        Dim result As Integer = 0
        Dim bActBankAccount As Object

        Const kMethod As String = "CheckBranchAssociatedWithBank"

        Try


            Dim oBankAccount As Object
            Dim lCnt As Integer


            ' Get an instance of the bActBankAccount via
            ' the public object manager.
            Dim temp_oBankAccount As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oBankAccount, "bACTBankAccount.Form", vInstanceManager:=PMGetViaClientManager)
            oBankAccount = temp_oBankAccount

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(ACClass, kMethod & " Fails to check assoicated branch with bank account", gPMConstants.PMELogLevel.PMLogError)
            End If


            m_lReturn = oBankAccount.CheckSourceAssociatedWithBank(v_lBankAccountID:=v_lBankAccountID, v_lSourceID:=v_lSourceID, r_bIsSourceAssociatedWithBank:=r_bIsSourceAssociatedWithBank)

            ' Terminate the object

            oBankAccount.Dispose()
            ' Destroy the instance of the object from memory.
            oBankAccount = Nothing



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sUsername:=g_oObjectManager.UserName, v_sClass:=ACClass, v_sMethod:=kMethod, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally





        End Try
        Return result
    End Function


    'WPR12- Enhancement Quote Collection Process
    Private Sub EnableDisableAdditionalDetailsMediaType()

        Dim bDoEnable As Boolean

        If chkExtra(m_iIsReceiptControlIndex).CheckState = CheckState.Checked Then
            For i As Integer = 0 To cboLookupExtra.Length - 1
                If Not (cboLookupExtra(i) Is Nothing) Then
                    If cboLookupExtra(i).TableName.ToUpper() = csMediaTypeValidationTable.ToUpper() Then
                        If cboLookupExtra(i).ItemCaption(cboLookupExtra(i).ItemId).ToUpper() = "CHEQUE" Or cboLookupExtra(i).ItemCaption(cboLookupExtra(i).ItemId).ToUpper() = "CREDIT CARD" Then
                            'ENABLE CHECKBOX
                            chkExtra(m_iIsAdditionalDetailsIndex).Enabled = True
                            bDoEnable = True
                            Exit For
                        End If
                    End If
                End If
            Next
        End If

        'DISABLE CHECKBOX
        If Not bDoEnable Then
            chkExtra(m_iIsAdditionalDetailsIndex).CheckState = CheckState.Unchecked
            chkExtra(m_iIsAdditionalDetailsIndex).Enabled = False
        End If

    End Sub
    Private Sub frmDetails_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
        MemoryHelper.ReleaseMemory()
    End Sub
    Private Sub VScroll1_Scroll(ByVal eventSender As Object, ByVal eventArgs As ScrollEventArgs) Handles VScroll1.Scroll
        Select Case eventArgs.Type
            Case ScrollEventType.ThumbTrack
                VScroll1_Scroll_Renamed(eventArgs.NewValue)
            Case ScrollEventType.EndScroll
                VScroll1_Change(eventArgs.NewValue)
        End Select
    End Sub

    Private Sub frmDetails_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown

        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D Then
            tabMainTab.SelectedIndex = 0
        End If
    End Sub

    Private Sub frmDetails_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'New Code to make the dynamic text boxes visible
        If m_sTableName = "Tax_Group" Then
            txtExtra(2).Width = UctCompiledRule1.Width
            btnFile.Text = "..."
            btnFile.Name = "btnFile"
            btnFile.Top = txtExtra(2).Top
            btnFile.Left = txtExtra(2).Right + 5
            btnFile.Width = 20
            btnFile.Height = 20
            UctCompiledRule1.Top = txtExtra(2).Top
            UctCompiledRule1.Left = txtExtra(2).Left

            AddHandler btnFile.Click, AddressOf btnFile_Click
            picCanvas.Controls.Add(btnFile)

            Dim sAdvTaxScriptVal As String = String.Empty
            For lCount As Integer = m_vExtras.GetLowerBound(1) To m_vExtras.GetUpperBound(1)
                If m_vExtras(ACExtraCaption, lCount) = "advanced_tax_script" Then
                    sAdvTaxScriptVal = m_vExtras(ACExtraValue, lCount)
                End If
            Next
            For lCount As Integer = m_vExtras.GetLowerBound(1) To m_vExtras.GetUpperBound(1)
                If m_vExtras(ACExtraCaption, lCount) = "rule_type" Then
                    For Each cmblkp As PMLookupControl.cboPMLookup In cboLookupExtra
                        If Not cmblkp Is Nothing Then
                            If cmblkp.TableName = kRuleTypeCaption Then
                                cmblkp.cboTypeTable.SelectedItem = IIf(String.IsNullOrEmpty(ToSafeString(m_vExtras(ACExtraValue, lCount)).Trim()), _
                                                                        0, ToSafeInteger(m_vExtras(ACExtraValue, lCount)))
                                Dim nSelectedItemValue As Integer = DirectCast(cmblkp.cboTypeTable.SelectedItem, Microsoft.VisualBasic.Compatibility.VB6.ListBoxItem).ItemData
                                If nSelectedItemValue = 3 Then
                                    UctCompiledRule1.Text = sAdvTaxScriptVal
                                ElseIf nSelectedItemValue = 1 Then
                                    txtExtra(2).Text = "ATS.Rul"
                                End If
                            End If
                        End If
                    Next
                End If
            Next
        Else
            UctCompiledRule1.Visible = False
        End If
        Dim tabOrderValue As Integer = 0
        For Each ctl As Control In Me.Controls(3).Controls(0).Controls(0).Controls(0).Controls
            If TypeOf (ctl) Is TextBox Then
                If (ctl.Name <> "_txtExtra_0") Then
                    ctl.Visible = True
                Else
                    If makeTextBox0 Then
                        ctl.Visible = True
                    End If
                End If

            End If
            ctl.TabIndex = tabOrderValue
            tabOrderValue = tabOrderValue + 1
        Next
        If udlVersionControlId >= 0 Then
            txtExtra(udlVersionControlId).Visible = False
        End If

        tabMainTab.TabIndex = tabOrderValue + 2
        txtCode.Select()

        GetRulePath()
    End Sub

    Private Sub btnFile_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs)
        RuleEditor()
    End Sub

    ''' <summary>
    ''' Open rule file on clicking ellipsis button
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function RuleEditor() As Integer
        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim oRuleEditor As iPMURuleEditor.Interface_Renamed

        Dim temp_oRuleEditor As Object = Nothing
        m_lReturn = g_oObjectManager.GetInstance(temp_oRuleEditor, sClassName:="iPMURuleEditor.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
        oRuleEditor = temp_oRuleEditor

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            nResult = gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not (oRuleEditor Is Nothing) Then
            'set the default editor values
            oRuleEditor.RulePath = m_sRulePath
            oRuleEditor.FixedFile = True
            oRuleEditor.RuleFileName = "ATS.rul"
            oRuleEditor.Start()
            If oRuleEditor.Status <> gPMConstants.PMEReturnCode.PMCancel Then
                nResult = gPMConstants.PMEReturnCode.PMTrue
            End If
            oRuleEditor.Dispose()
            oRuleEditor = Nothing
        End If
        Return nResult
    End Function

    ''' <summary>
    ''' get business object
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetRulePath() As Integer
        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue

        Try
            ' Get the details from the business object.
            ' Get an instance of the business object via
            ' the public object manager.
            nResult = g_oObjectManager.GetInstance(m_oBusiness, "bPMMaintainLookup.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)

            ' Check for errors.
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                nResult = gPMConstants.PMEReturnCode.PMFalse
            End If

            nResult = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            ' Check for errors.
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                nResult = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")
            End If

            m_sRulePath = m_oBusiness.RulePath

            ' Terminate the business object

            m_oBusiness.Dispose()
            ' Destroy the instance of the business object
            ' from memory.
            m_oBusiness = Nothing

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
        End Try

        Return nResult
    End Function


End Class
