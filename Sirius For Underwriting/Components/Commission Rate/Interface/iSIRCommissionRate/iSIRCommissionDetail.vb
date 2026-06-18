Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no.129
Imports SharedFiles
'Friend Partial Class frmDetail
Partial Public Class frmDetail
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmDetail
    '
    ' Date: 5th September 2000
    '
    ' Description: Detail from used for data entry.
    '
    ' Edit History:
    ' CMG / PB 23072002 New Commission Grouping functionality
    ' ***************************************************************** '
    'replaced iPMFunc.GetResData with GetResData in the whole document

    Public Const vbFormCode As Integer = 0
    'Declare variables to hold the property values
    Dim m_lPartyTypeId As Integer
    Dim m_lPartyId As Integer
    Dim m_lProductId As Integer
    Dim m_lRiskTypeId As Integer
    Dim m_lTransactionTypeId As Integer
    Dim m_lCommissionBandId As Integer
    Dim m_lCommissionGroupId As Integer
    Dim m_cRate As Decimal
    Dim m_dEffectiveDate As Date
    Dim m_bIsValue As Boolean
    Dim m_lMode As gPMConstants.PMEComponentAction
    Dim m_lReturn As gPMConstants.PMEReturnCode
    Dim m_lCommissionArrangementId As Integer
    Dim m_lTaxGroupID As Integer
    Private m_bSuppressDecimalValues As Boolean
    'SAGICOR WPR14
    Private m_lCommissionLevel As Integer
    Private m_sUniqueId As String = ""
    Private m_sScreenHierarchy As String = ""

    Public dtOldDate As Date

    Public m_oFormfields As iPMFormControl.FormFields

    'Private m_oInterface As ClassInterface
    Private m_oInterface As Interface_Renamed
    ''' <summary>
    ''' Holds The decimal suppress configuration.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsSuppressDecimalValues() As Boolean
        Get
            Return m_bSuppressDecimalValues
        End Get
        Set(ByVal Value As Boolean)
            m_bSuppressDecimalValues = Value
        End Set
    End Property

    ' CMG / PB 23072002 New Commission Grouping functionality
    Private Sub cboCommissionGroup_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCommissionGroup.SelectedIndexChanged

        CommissionGroupId = VB6.GetItemData(cboCommissionGroup, cboCommissionGroup.SelectedIndex)

    End Sub

    Private Sub cboCommissionband_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCommissionband.SelectedIndexChanged

        CommissionBandId = VB6.GetItemData(cboCommissionband, cboCommissionband.SelectedIndex)

    End Sub

    Private Sub cboParty_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboParty.SelectedIndexChanged

        PartyId = VB6.GetItemData(cboParty, cboParty.SelectedIndex)
        If m_lMode = gPMConstants.PMEComponentAction.PMAdd Then
            If PartyId > 0 Then
                cboCommissionLevel.Enabled = False
                'cboCommissionLevel.ListIndex = 0
                m_lReturn = CType(MainModule.frmInterface.SetCommissionLevel(cboCommissionLevel, PartyId), gPMConstants.PMEReturnCode)
            Else
                If m_lMode = gPMConstants.PMEComponentAction.PMAdd And cboCommissionLevel.Enabled = False Then
                    cboCommissionLevel.Enabled = True
                    cboCommissionLevel.SelectedIndex = 0
                Else
                    cboCommissionLevel.Enabled = True
                End If

            End If
        End If
    End Sub

    Private Sub cboPartyType_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPartyType.SelectedIndexChanged

        'CMG / PB 15/7/2002 Only enable the commission group if party type selected
        If cboPartyType.SelectedIndex = 0 Then
            cboCommissionGroup.Enabled = gPMConstants.PMEReturnCode.PMFalse
        Else
            cboCommissionGroup.Enabled = gPMConstants.PMEReturnCode.PMTrue
        End If

        'Get the newly selected party type
        PartyTypeId = VB6.GetItemData(cboPartyType, cboPartyType.SelectedIndex)

        m_lReturn = CType(MainModule.frmInterface.PopulatePartyCombo(cboParty, PartyTypeId, , m_lCommissionLevel), gPMConstants.PMEReturnCode)
        If cboParty.SelectedIndex < 0 Then
            cboParty.SelectedIndex = 0
        End If
        ' m_lReturn = PopulatePartyCombo(cboParty, PartyTypeId)
        If cboPartyType.SelectedIndex > 1 Then
            cmdFindParty.Enabled = gPMConstants.PMEReturnCode.PMTrue
        Else
            cmdFindParty.Enabled = gPMConstants.PMEReturnCode.PMFalse
        End If
    End Sub

    Private Sub cboProduct_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboProduct.SelectedIndexChanged

        ProductId = VB6.GetItemData(cboProduct, cboProduct.SelectedIndex)

    End Sub

    Private Sub cboRiskType_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboRiskType.SelectedIndexChanged

        RiskTypeId = VB6.GetItemData(cboRiskType, cboRiskType.SelectedIndex)

    End Sub

    Private Sub cboTaxGroup_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTaxGroup.SelectedIndexChanged
        If cboTaxGroup.SelectedIndex = 0 Then
            m_lTaxGroupID = 0
        Else
            m_lTaxGroupID = VB6.GetItemData(cboTaxGroup, cboTaxGroup.SelectedIndex)
        End If
    End Sub

    Private Sub cboTransactionType_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTransactionType.SelectedIndexChanged

        TransactionTypeId = VB6.GetItemData(cboTransactionType, cboTransactionType.SelectedIndex)

    End Sub

    Private Sub chkIsvalue_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkIsvalue.CheckStateChanged
        'Start - Renuka - (WPR64 Paralleling)
        'End - Renuka - (WPR64 Paralleling)

        'Get the value in the text box

        Dim sValue As String = CStr(m_oFormfields.UnformatControl(ctlControl:=txtRate))
        'Start - Renuka - (WPR64 Paralleling)

        Dim sMaxValue As String = CStr(m_oFormfields.UnformatControl(ctlControl:=txtMaximumRate))
        'End - Renuka - (WPR64 Paralleling)
        If chkIsvalue.CheckState = CheckState.Checked Then

            'start
            'm_oFormfields.Item(CStr(1)).set_FieldType(gPMConstants.PMEDataType.PMCurrency)
            m_oFormfields.Item(1).FieldType = gPMConstants.PMEDataType.PMCurrency
            'm_oFormfields.Item(CStr(1)).set_FieldFormat(gPMConstants.PMEFormatStyle.PMFormatCurrency)
            m_oFormfields.Item(1).FieldFormat = gPMConstants.PMEFormatStyle.PMFormatCurrency
            m_oFormfields.Item(1).DecimalPlaces = 0
            'Start (Sriram P )Tech Spec - UIIC WPR64 - Enhancement Commission Maintenance.doc section(5.1.3.2)
            'm_oFormfields.Item(CStr(3)).set_FieldFormat(gPMConstants.PMEFormatStyle.PMFormatCurrency)
            m_oFormfields.Item(3).FieldFormat = gPMConstants.PMEFormatStyle.PMFormatCurrency
            'End - Renuka - (WPR64 Paralleling)
        Else
            'm_oFormfields.Item(CStr(1)).set_FieldType(gPMConstants.PMEDataType.PMDouble)
            m_oFormfields.Item(1).FieldType = gPMConstants.PMEDataType.PMDouble
            'm_oFormfields.Item(CStr(1)).set_FieldFormat(gPMConstants.PMEFormatStyle.PMFormatPercent)
            m_oFormfields.Item(1).FieldFormat = gPMConstants.PMEFormatStyle.PMFormatPercent
            m_oFormfields.Item(1).DecimalPlaces = 10
            'Start (Sriram P )Tech Spec - UIIC WPR64 - Enhancement Commission Maintenance.doc section(5.1.3.2)
            'm_oFormfields.Item(CStr(3)).set_FieldFormat(gPMConstants.PMEFormatStyle.PMFormatPercent)
            m_oFormfields.Item(3).FieldFormat = gPMConstants.PMEFormatStyle.PMFormatPercent
            'end
            'End - Renuka - (WPR64 Paralleling)

        End If
        'Put back the content
        m_lReturn = m_oFormfields.FormatControl(ctlControl:=txtRate, vControlValue:=sValue)
        'Start - Renuka - (WPR64 Paralleling)
        m_lReturn = m_oFormfields.FormatControl(ctlControl:=txtMaximumRate, vControlValue:=sMaxValue)
        'End - Renuka - (WPR64 Paralleling)
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        Me.Hide()
    End Sub

    Private Sub cmdFindParty_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFindParty.Click
        Dim vCnt As Integer
        Dim vShortName As String = ""
        Dim vName As Object
        Dim lRows As Integer
        Dim iPartyTypeID As Integer
        Dim vCommissionLevelID As Object

        Try

            iPartyTypeID = VB6.GetItemData(cboPartyType, cboPartyType.SelectedIndex)

            'SAGICOR WPR14
            If cboCommissionLevel.SelectedIndex > 0 Then
                'UPGRADE_WARNING: Couldn't resolve default property of object vCommissionLevelID. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                vCommissionLevelID = VB6.GetItemData(cboCommissionLevel, cboCommissionLevel.SelectedIndex)
            Else
                'UPGRADE_WARNING: Couldn't resolve default property of object vCommissionLevelID. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                vCommissionLevelID = 0
            End If

            m_lReturn = m_oInterface.SelectParty(vPartyCnt:=vCnt, vShortName:=vShortName, vName:=CStr(vName), vPartyTypeID:=iPartyTypeID, vCommissionLevelID:=vCommissionLevelID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            lRows = cboParty.Items.Count - 1
            If vShortName <> "" Then
                m_lReturn = MainModule.frmInterface.PopulatePartyCombo(cboCombo:=cboParty, v_lPartytypeId:=PartyTypeId, v_lCommissionLevelID:=vCommissionLevelID)
                lRows = cboParty.Items.Count - 1
                For lRow As Integer = 0 To lRows
                    If vCnt = CInt(VB6.GetItemData(cboParty, lRow)) Then

                        cboParty.SelectedIndex = lRow
                        Exit For

                    End If
                Next lRow
            End If

        Catch excep As System.Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:="frmDetail", vMethod:="cmdFindParty_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
        End Try

    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
        ' call help

        'm_lReturn = CType(PMHelpFunc.ShowHelp(dlgHelp:=dlgHelp, lContextID:=ScreenhelpID), gPMConstants.PMEReturnCode)

        m_lReturn = CType(PMHelpFunc.ShowHelp(cmdHelp, ScreenhelpID), gPMConstants.PMEReturnCode)
    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
        Dim dRate As Double
        Dim dtEffectiveDate As Date
        Dim sMessage, sTitle As String
        Dim cMaxRate As Decimal
        'Change the Mouse pointer
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        'Validate the mandatory control
        m_lReturn = m_oFormfields.CheckMandatoryControls()

        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

            'Get the details from the form

            dRate = CDbl(m_oFormfields.UnformatControl(txtRate))

            dtEffectiveDate = ToSafeDate(m_oFormfields.UnformatControl(txtEffectiveDate))
            'Start - Renuka - (WPR64 Paralleling)
            If txtMaximumRate.Text.Trim() <> "" Or txtMaximumRate.Text.Trim().Length > 0 Then

                cMaxRate = CDec(m_oFormfields.UnformatControl(txtMaximumRate))
            End If
            If cMaxRate < dRate And cMaxRate <> 0 Then
                MessageBox.Show("The Maximum Rate can be equal to or greater than Rate but not less.", "Commission rate details", MessageBoxButtons.OK, MessageBoxIcon.Information)
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If
            'End - Renuka - (WPR64 Paralleling)
            m_sUniqueId = GetUniqueID()

            Select Case m_lMode
                Case gPMConstants.PMEComponentAction.PMAdd

                    'Validate for Duplicate
                    ' CMG / PB 23072002 New Commission Grouping functionality
                    'Thinh Nguyen 01/07/2003 - add effective date
                    If MainModule.frmInterface.IsDuplicateExists(v_lPartytypeId:=PartyTypeId, v_lPartyId:=PartyId, v_lRiskTypeId:=RiskTypeId, v_lProductId:=ProductId, v_lTransactionTypeId:=TransactionTypeId, v_lCommissionBandId:=CommissionBandId, v_lCommissionGroupId:=CommissionGroupId, v_dtEffectiveDate:=dtEffectiveDate, v_lCommissionLevelID:=Commissionlevel) = gPMConstants.PMEReturnCode.PMFalse Then

                        'Call the method to add the details into the Commission Arrangement table
                        'Start - Renuka - (WPR64 Paralleling)
                        If cMaxRate <= 0 Then
                            m_lReturn = CType(MainModule.frmInterface.AddcommissionArrangement(PartyTypeId, PartyId, RiskTypeId, ProductId, TransactionTypeId, CommissionBandId, CommissionGroupId, dtEffectiveDate, dRate, chkIsvalue.CheckState, m_lTaxGroupID, , Commissionlevel, m_sUniqueId), gPMConstants.PMEReturnCode)
                        Else
                            m_lReturn = CType(MainModule.frmInterface.AddcommissionArrangement(PartyTypeId, PartyId, RiskTypeId, ProductId, TransactionTypeId, CommissionBandId, CommissionGroupId, dtEffectiveDate, dRate, chkIsvalue.CheckState, m_lTaxGroupID, cMaxRate, Commissionlevel, m_sUniqueId), gPMConstants.PMEReturnCode)

                        End If
                        'End - Renuka - (WPR64 Paralleling)

                    Else

                        'Get the Error message and the title from the Resource file and display it

                        sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDuplicateItemTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDuplicateItem, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        ' Display message.
                        MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                        m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                        Me.Hide()

                    End If

                Case gPMConstants.PMEComponentAction.PMEdit

                    ' Alix - 26/01/2004
                    ' Also check for duplicates when updating
                    If MainModule.frmInterface.IsDuplicateExists(v_lPartytypeId:=PartyTypeId, v_lPartyId:=PartyId, v_lRiskTypeId:=RiskTypeId, v_lProductId:=ProductId, v_lTransactionTypeId:=TransactionTypeId, v_lCommissionBandId:=CommissionBandId, v_lCommissionGroupId:=CommissionGroupId, v_dtEffectiveDate:=dtEffectiveDate, v_lCommissionLevelID:=Commissionlevel) = gPMConstants.PMEReturnCode.PMFalse Or dtOldDate = dtEffectiveDate Then

                        'Call the method to Modify the details in the currently selected commission arrangement
                        ' CMG / PB 23072002 New Commission Grouping functionality
                        'PSL 29/07/2003 5616 need old date and new date for changing primary key
                        'Start - Renuka - (WPR64 Paralleling)
                        If cMaxRate <= 0 Then
                            m_lReturn = CType(MainModule.frmInterface.EditCommissionArrangement(PartyTypeId, PartyId, RiskTypeId, ProductId, TransactionTypeId, CommissionBandId, CommissionGroupId, dRate, chkIsvalue.CheckState, dtEffectiveDate, dtOldDate, m_lTaxGroupID, , Commissionlevel, m_sUniqueId), gPMConstants.PMEReturnCode)
                        Else
                            m_lReturn = CType(MainModule.frmInterface.EditCommissionArrangement(PartyTypeId, PartyId, RiskTypeId, ProductId, TransactionTypeId, CommissionBandId, CommissionGroupId, dRate, chkIsvalue.CheckState, dtEffectiveDate, dtOldDate, m_lTaxGroupID, cMaxRate, Commissionlevel, m_sUniqueId), gPMConstants.PMEReturnCode)

                        End If
                        'End - Renuka - (WPR64 Paralleling)
                    Else

                        'Get the Error message and the title from the Resource file and display it

                        sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDuplicateItemTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDuplicateItem, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        ' Display message.
                        MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                        m_lReturn = gPMConstants.PMEReturnCode.PMFalse

                    End If

            End Select

            'Repopulate the listview
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                Me.Hide()

                'Repopulate the listview
                m_lReturn = CType(MainModule.frmInterface.FilterCommissionRatings(), gPMConstants.PMEReturnCode)

            End If

        End If

        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

    End Sub
    Public WriteOnly Property Mode() As Integer
        Set(ByVal Value As Integer)

            m_lMode = Value

            If m_lMode = gPMConstants.PMEComponentAction.PMAdd Then

                'Enable the combos
                cboPartyType.Enabled = True
                cboParty.Enabled = True
                cboProduct.Enabled = True
                cboRiskType.Enabled = True
                cboTransactionType.Enabled = True
                cboCommissionband.Enabled = True

                'CMG / PB 15/7/2002 Only enable the commission group if party type selected
                If cboPartyType.SelectedIndex = 0 Then
                    cboCommissionGroup.Enabled = gPMConstants.PMEReturnCode.PMFalse
                    cmdFindParty.Enabled = gPMConstants.PMEReturnCode.PMFalse
                Else
                    cboCommissionGroup.Enabled = gPMConstants.PMEReturnCode.PMTrue
                    cmdFindParty.Enabled = gPMConstants.PMEReturnCode.PMTrue
                End If
                cboCommissionLevel.Enabled = True
            Else

                'Disable all the combos
                cboPartyType.Enabled = False
                cboParty.Enabled = False
                cboProduct.Enabled = False
                cboRiskType.Enabled = False
                cboTransactionType.Enabled = False
                cboCommissionband.Enabled = False
                cboCommissionGroup.Enabled = False
                cboCommissionLevel.Enabled = False
            End If
            cmdFindParty.Enabled = False
        End Set
    End Property

    Public Property PartyTypeId() As Integer
        Get
            Return m_lPartyTypeId
        End Get
        Set(ByVal Value As Integer)
            m_lPartyTypeId = Value
        End Set
    End Property

    Public Property PartyId() As Integer
        Get
            Return m_lPartyId
        End Get
        Set(ByVal Value As Integer)
            m_lPartyId = Value
        End Set
    End Property

    Public Property ProductId() As Integer
        Get
            Return m_lProductId
        End Get
        Set(ByVal Value As Integer)
            m_lProductId = Value
        End Set
    End Property

    Public Property RiskTypeId() As Integer
        Get
            Return m_lRiskTypeId
        End Get
        Set(ByVal Value As Integer)
            m_lRiskTypeId = Value
        End Set
    End Property

    Public Property TransactionTypeId() As Integer
        Get
            Return m_lTransactionTypeId
        End Get
        Set(ByVal Value As Integer)
            m_lTransactionTypeId = Value
        End Set
    End Property

    Public Property CommissionBandId() As Integer
        Get
            Return m_lCommissionBandId
        End Get
        Set(ByVal Value As Integer)
            m_lCommissionBandId = Value
        End Set
    End Property

    ' CMG / PB 23072002 New Commission Grouping functionality

    Public Property CommissionGroupId() As Integer
        Get
            Return m_lCommissionGroupId
        End Get
        Set(ByVal Value As Integer)
            m_lCommissionGroupId = Value
        End Set
    End Property
    'SAGICOR WPR14

    Public Property Commissionlevel() As Integer
        Get
            Commissionlevel = m_lCommissionLevel
        End Get
        Set(ByVal Value As Integer)
            m_lCommissionLevel = Value
        End Set
    End Property

    Function ValidateListView() As Integer
        Try

            '    For nCount = 1 To frmInterface.lvwRatingSection.ListItems.Count
            '
            '        Set lvItem = frmInterface.lvwRatingSection.ListItems(nCount)
            '
            '        If lvItem.SubItems(3) = cboRatingSectionType.ItemData(cboRatingSectionType.ListIndex) Then
            '
            '                ValidateListView = PMFalse
            '
            '                Exit For
            '        End If
            '
            '
            '    Next

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch

            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function
    ' CMG / PB 23072002 End

    Private Sub frmDetail_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender
            'Clear the rate field, so the user is forced to enter the mandatory data when first creating
            If m_lMode = gPMConstants.PMEComponentAction.PMAdd Then
                txtRate.Text = ""
                'Start - Renuka - (WPR64 Paralleling)
                txtMaximumRate.Text = ""
                'End - Renuka - (WPR64 Paralleling)
            End If
        End If
    End Sub

    Private Sub frmDetail_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        'Add the controls in the Detail form to the formfields collection
        'm_oFormfields = New iPMFormControl.FormFields()

        'm_oInterface = New ClassInterface()
        m_oInterface = New Interface_Renamed()
        'Get the Decimal Suppression flag
        Dim sTempOptionValue As String = ""
        iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTEnableDecimalsSuppression, v_vBranch:=1, r_vUnderwriting:=sTempOptionValue)

        If Trim(sTempOptionValue) <> "" AndAlso Trim(sTempOptionValue) = "1" Then
            IsSuppressDecimalValues = True
        End If

    End Sub

    Private Sub frmDetail_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        If UnloadMode <> vbFormCode Then
            Cancel = 1
            Me.Hide()
        Else
            'Commented the code as m_oFormfields cannot be assinged nothing here.
            'start
            'm_lReturn = m_oFormfields.Terminate()

            'm_oFormfields = Nothing
            'end
            m_oInterface = Nothing
        End If
        eventArgs.Cancel = Cancel <> 0
    End Sub

    Private Sub txtEffectiveDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtEffectiveDate.Enter
        m_lReturn = m_oFormfields.GotFocus(ctlControl:=txtEffectiveDate)
    End Sub

    Private Sub txtEffectiveDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtEffectiveDate.Leave
        m_lReturn = m_oFormfields.LostFocus(ctlControl:=txtEffectiveDate)
    End Sub

    Private Sub txtRate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRate.Enter
        m_lReturn = m_oFormfields.GotFocus(ctlControl:=txtRate)

    End Sub

    Private Sub txtRate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRate.Leave
        Dim bIsValueZero As Boolean
        If Val(txtRate.Text.TrimEnd("%")) = 0 Then
            bIsValueZero = True
        End If

        m_lReturn = m_oFormfields.LostFocus(ctlControl:=txtRate)
        'Start - Renuka - (WPR64 Paralleling)
        Dim sCommisionrate As String = ""
        If txtRate.Text.IndexOf("%"c) >= 0 And Not bIsValueZero Then
            sCommisionrate = txtRate.Text
            sCommisionrate = CStr(gPMFunctions.UnFormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, gPMConstants.PMEFormatStyle.PMFormatString, sCommisionrate))
        Else
            sCommisionrate = txtRate.Text
        End If
        If sCommisionrate.Length <= 0 Or sCommisionrate <= "0" Or sCommisionrate <= "0.00" Or bIsValueZero Then
            txtMaximumRate.Enabled = False
            txtMaximumRate.Text = ""
        Else
            txtMaximumRate.Enabled = True
        End If
        'End - Renuka - (WPR64 Paralleling)
    End Sub

    'Start - Renuka - (WPR64 Paralleling)
    Private Sub txtMaximumRate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMaximumRate.Enter
        m_lReturn = m_oFormfields.GotFocus(ctlControl:=txtMaximumRate)
    End Sub
    Private Sub txtMaximumRate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMaximumRate.Leave
        m_lReturn = m_oFormfields.LostFocus(ctlControl:=txtMaximumRate)
    End Sub
    'End - Renuka - (WPR64 Paralleling)

    Private Sub frmDetail_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown

        If e.Alt And e.KeyCode = Keys.D1 Then
            SSTab1.SelectedIndex = 0
        End If
    End Sub

    Private Sub _SSTab1_TabPage0_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _SSTab1_TabPage0.Click

    End Sub

    'SAGICOR WPR14
    'UPGRADE_WARNING: Event cboCommissionLevel.SelectedIndexChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    Private Sub cboCommissionLevel_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboCommissionLevel.SelectedIndexChanged

        If cboCommissionLevel.SelectedIndex = 0 Then
            m_lCommissionLevel = 0
        Else
            m_lCommissionLevel = VB6.GetItemData(cboCommissionLevel, cboCommissionLevel.SelectedIndex)
        End If

        If cboCommissionLevel.Enabled = True Then
            If cboPartyType.SelectedIndex = 0 Then
                PartyTypeId = 0
                'm_lCommissionLevel = 0
            Else
                PartyTypeId = VB6.GetItemData(cboPartyType, cboPartyType.SelectedIndex)
                'm_lCommissionLevel = cboCommissionLevel.ItemData(cboCommissionLevel.ListIndex)
            End If

            'Get the newly selected party type
            'PartyTypeId = cboPartyType.ItemData(cboPartyType.ListIndex)

            m_lReturn = MainModule.frmInterface.PopulatePartyCombo(cboCombo:=cboParty, v_lPartytypeId:=PartyTypeId, v_lCommissionLevelID:=m_lCommissionLevel)
            If cboParty.SelectedIndex < 0 Then
                cboParty.SelectedIndex = 0
            End If
            ' m_lReturn = PopulatePartyCombo(cboParty, PartyTypeId)
        End If

    End Sub

    Public Sub AddHandlers()

        AddHandler Me.cboParty.SelectedIndexChanged, AddressOf Me.cboParty_SelectedIndexChanged
        AddHandler Me.cboPartyType.SelectedIndexChanged, AddressOf Me.cboPartyType_SelectedIndexChanged
        AddHandler Me.cboProduct.SelectedIndexChanged, AddressOf Me.cboProduct_SelectedIndexChanged
        AddHandler Me.cboRiskType.SelectedIndexChanged, AddressOf Me.cboRiskType_SelectedIndexChanged
        AddHandler Me.cboTransactionType.SelectedIndexChanged, AddressOf Me.cboTransactionType_SelectedIndexChanged
        AddHandler Me.cboCommissionband.SelectedIndexChanged, AddressOf Me.cboCommissionband_SelectedIndexChanged
        AddHandler Me.cboCommissionGroup.SelectedIndexChanged, AddressOf Me.cboCommissionGroup_SelectedIndexChanged
        AddHandler Me.cboTaxGroup.SelectedIndexChanged, AddressOf Me.cboTaxGroup_SelectedIndexChanged
        AddHandler Me.cboCommissionLevel.SelectedIndexChanged, AddressOf Me.cboCommissionLevel_SelectedIndexChanged

    End Sub

    Public Sub RemoveHandlers()

        RemoveHandler Me.cboParty.SelectedIndexChanged, AddressOf Me.cboParty_SelectedIndexChanged
        RemoveHandler Me.cboPartyType.SelectedIndexChanged, AddressOf Me.cboPartyType_SelectedIndexChanged
        RemoveHandler Me.cboProduct.SelectedIndexChanged, AddressOf Me.cboProduct_SelectedIndexChanged
        RemoveHandler Me.cboRiskType.SelectedIndexChanged, AddressOf Me.cboRiskType_SelectedIndexChanged
        RemoveHandler Me.cboTransactionType.SelectedIndexChanged, AddressOf Me.cboTransactionType_SelectedIndexChanged
        RemoveHandler Me.cboCommissionband.SelectedIndexChanged, AddressOf Me.cboCommissionband_SelectedIndexChanged
        RemoveHandler Me.cboCommissionGroup.SelectedIndexChanged, AddressOf Me.cboCommissionGroup_SelectedIndexChanged
        RemoveHandler Me.cboTaxGroup.SelectedIndexChanged, AddressOf Me.cboTaxGroup_SelectedIndexChanged
        RemoveHandler Me.cboCommissionLevel.SelectedIndexChanged, AddressOf Me.cboCommissionLevel_SelectedIndexChanged

    End Sub
    Private Sub chkIsvalue_CheckedChanged(sender As Object, e As EventArgs) Handles chkIsvalue.CheckedChanged
        If IsSuppressDecimalValues AndAlso chkIsvalue.CheckState = CheckState.Checked Then
            txtRate.Text = 0
            txtMaximumRate.Text = 0
        End If
    End Sub
    Private Sub txtRate_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtRate.KeyPress, txtMaximumRate.KeyPress
        If IsSuppressDecimalValues Then
            'Disallow the decimals
            gPMFunctions.NumPress(sender, e)
        End If
    End Sub

End Class
