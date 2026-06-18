Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Commission_NET.Commission")>
Partial Public Class Commission
    Inherits System.Windows.Forms.UserControl
    ' ***************************************************************** '
    ' Object Name: uctPMURITax
    '
    ' Date: 25-04-2005
    '
    ' Description: Main User Control
    '
    ' Edit History:
    '   NB: All Initial functionality has been ripped from iSIRAgentCommission
    ' 24/10/2005 RKS Premium Override
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Commission"

    'For process mode
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_dtEffectiveDate As Date

    'Start - Renuka - (WPR64 Paralleling)
    Private m_bUserHasAuthority As Boolean
    'End - Renuka - (WPR64 Paralleling)
    'For scalability
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iUserId As Integer
    Private m_lCurrencyID As Integer

    'Default Property Values:
    Private Const m_def_BackColor As Integer = 0
    Private Const m_def_ForeColor As Integer = 0
    Private Const m_def_BackStyle As Integer = 0
    Private Const m_def_BorderStyle As Integer = 0
    Private Const m_def_ShowEdit As Boolean = True
    Private Const m_def_Enabled As Boolean = True
    Private Const m_def_Visible As Boolean = True

    'Property Variables:
    Private m_BackColor As Integer
    Private m_ForeColor As Integer
    Private m_BackStyle As Integer
    Private m_BorderStyle As Integer
    Private m_ShowEdit As Boolean
    Private m_Enabled As Boolean
    Private m_Visible As Boolean
    Private m_Font As Font

    Private m_lInsuranceFileCnt As Integer
    Private m_bReadOnly As Boolean
    Private m_cTotalCommission As Decimal
    Private m_cTotalTax As Decimal
    Private m_cTotalPremium As Decimal
    Private m_cLeadAgentTC As Decimal
    Private m_cLeadAgentTT As Decimal
    Private m_vAgentCommission(,) As Object
    Private m_sTransactionType As String = ""

    Private m_sInsuranceHolderShortName As String = ""
    Private m_sInsuranceHolderName As String = ""
    Private m_sInsuranceHolderResolvedName As String = ""
    Private m_sInsuranceRef As String = ""
    Private m_sInsuranceFolderDescription As String = ""
    Private m_sInsuranceCurrencyCode As String = ""
    Private m_sInsuranceCurrencyCaption As String = ""
    Private m_iInsuranceFileSourceID As Integer
    Private m_iInsuranceFileCurrencyID As Integer
    Private m_cGrossTotal As Decimal
    Private m_bRecalc As Boolean
    ' Stores the return value for the
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Declare an instance of the Business object.

    Private m_oBusiness As bSirAgentCommission.Business
    'Developer Guide. 50

    Private m_dInitialTaxAmount As Decimal
    Private m_dInitialCommRate As Decimal
    Private m_nTaxGroupItemId As Integer
    Private m_dInitialCommValue As Decimal

    Dim objfrmDetail As frmDetail
    Public Event Change(ByVal Sender As Object, ByVal e As EventArgs)

    <Browsable(False)>
    Public WriteOnly Property InsuranceFileCnt() As Integer
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property ReadOnly_Renamed() As Boolean
        Set(ByVal Value As Boolean)
            m_bReadOnly = Value
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)
            m_sTransactionType = Value
        End Set
    End Property

    <Browsable(False)>
    Public ReadOnly Property TotalCommission() As Decimal
        Get
            Return m_cTotalCommission
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property TotalTax() As Decimal
        Get
            Return m_cTotalTax
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property InsuranceHolderShortName() As String
        Get
            Return m_sInsuranceHolderShortName
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property InsuranceHolderName() As String
        Get
            Return m_sInsuranceHolderName
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property InsuranceHolderResolvedName() As String
        Get
            Return m_sInsuranceHolderResolvedName
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property InsuranceRef() As String
        Get
            Return m_sInsuranceRef
        End Get
    End Property
    'PN 35553
    <Browsable(False)>
    Public ReadOnly Property LeadAgentCommission() As String
        Get
            Return CStr(m_cLeadAgentTC)
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property LeadAgentTax() As String
        Get
            Return CStr(m_cLeadAgentTT)
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property InsuranceFolderDescription() As String
        Get
            Return m_sInsuranceFolderDescription
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property InsuranceCurrencyCode() As String
        Get
            Return m_sInsuranceCurrencyCode
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property InsuranceCurrencyCaption() As String
        Get
            Return m_sInsuranceCurrencyCaption
        End Get
    End Property

    <Browsable(False)>
    Public WriteOnly Property IsAmended() As Integer
        Set(ByVal Value As Integer)
            objfrmDetail.IsAmended = Value
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property GrossTotal() As Decimal
        Set(ByVal Value As Decimal)
            m_cGrossTotal = Value
            CalculateTotal()
            '    If m_bRecalc = False Then
            '    m_bRecalc = True
            '        Recalculate
            '
            '    End If
        End Set
    End Property

    Private Sub lvwAgentCommission_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwAgentCommission.Click

        'Start - Renuka - (WPR64 Paralleling)
        Dim lItemId As Integer
        'End - Renuka - (WPR64 Paralleling)
        cmdEdit.Enabled = False

        If Not (lvwAgentCommission.FocusedItem Is Nothing) Then
            If lvwAgentCommission.FocusedItem.Text.Trim() <> "SSPSUBAGENT" Then
                cmdEdit.Enabled = True
            End If
        End If
        'Start - Renuka - (WPR64 Paralleling)
        Dim kMethodName As String = "lvwAgentCommission_Click"
        Dim lReturn As gPMConstants.PMEReturnCode = CType(GetSelectedItem(lItemId, lvwAgentCommission), gPMConstants.PMEReturnCode)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "GetSelectedItem Failed", gPMConstants.PMELogLevel.PMLogError)
        End If
        'Fix for 5433 (Munich Re Issue no : 45 )
        If cmdEdit.Text = "View" Then
            cmdEdit.Enabled = m_bReadOnly And lItemId <> -1
        Else
            cmdEdit.Enabled = m_bUserHasAuthority And Not m_bReadOnly And lItemId <> -1
        End If
        'End - Renuka - (WPR64 Paralleling)
    End Sub


    Private Sub UserControl_InitProperties()
        m_BackColor = m_def_BackColor
        m_ForeColor = m_def_ForeColor
        m_BackStyle = m_def_BackStyle
        m_BorderStyle = m_def_BorderStyle
        m_ShowEdit = m_def_ShowEdit
        m_Enabled = m_def_Enabled
        m_Visible = m_def_Visible

        'Developer Guide No 2(no solution)
        'm_Font = Ambient.Font
        m_Font = Me.Font
    End Sub
    Private Sub Commission_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize

        Dim lNewWidth As Integer = CInt(VB6.PixelsToTwipsX(Me.Width))
        Dim lNewHeight As Integer = CInt(VB6.PixelsToTwipsY(Me.Height) - (VB6.PixelsToTwipsY(cmdEdit.Height) + 100 + VB6.PixelsToTwipsY(frmAgentTotal.Height)))

        If lNewWidth > 0 Then
            lvwAgentCommission.Width = VB6.TwipsToPixelsX(lNewWidth)
            'Pk
            frmAgentTotal.Width = MyBase.Width
        End If

        If lNewHeight > 0 Then
            lvwAgentCommission.Height = VB6.TwipsToPixelsY(lNewHeight)
        End If

        frmAgentTotal.Top = lvwAgentCommission.Height + VB6.TwipsToPixelsY(100)
        frmAgentTotal.Left = lvwAgentCommission.Left


        cmdEdit.Left = 0
        cmdEdit.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(lvwAgentCommission.Height) + VB6.PixelsToTwipsY(frmAgentTotal.Height) + 100)
        cmdEdit.Visible = True

        lblStatus.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cmdEdit.Left) + VB6.PixelsToTwipsX(cmdEdit.Width) + 300)
        lblStatus.Top = cmdEdit.Top + VB6.TwipsToPixelsY(100)

        'Start (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1.2)
        lblTaxAmendedStatus.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(lblStatus.Left) + VB6.PixelsToTwipsX(lblStatus.Width) + 300)
        lblTaxAmendedStatus.Top = lblStatus.Top
        'End (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1.2)

    End Sub




    'Developer Guide No 1(No solution)
    Private Sub UserControl_ReadProperties(ByRef PropBag As Object)


        m_BackColor = CInt(PropBag.ReadProperty("BackColor", m_def_BackColor))


        m_ForeColor = CInt(PropBag.ReadProperty("ForeColor", m_def_ForeColor))


        m_BackStyle = CInt(PropBag.ReadProperty("BackStyle", m_def_BackStyle))


        m_BorderStyle = CInt(PropBag.ReadProperty("BorderStyle", m_def_BorderStyle))


        m_ShowEdit = CBool(PropBag.ReadProperty("ShowEdit", m_def_ShowEdit))


        m_Enabled = CBool(PropBag.ReadProperty("Enabled", m_def_Enabled))


        m_Visible = CBool(PropBag.ReadProperty("Visible", m_def_Visible))


        'Developer Guide No 2(no solution)
        m_Font = PropBag.ReadProperty("Font", Me.Font)
    End Sub



    'Developer Guide No 1(no solution)
    Private Sub UserControl_WriteProperties(ByRef PropBag As Object)

        PropBag.WriteProperty("BackColor", m_BackColor, m_def_BackColor)

        PropBag.WriteProperty("ForeColor", m_ForeColor, m_def_ForeColor)

        PropBag.WriteProperty("BackStyle", m_BackStyle, m_def_BackStyle)

        PropBag.WriteProperty("BorderStyle", m_BorderStyle, m_def_BorderStyle)

        PropBag.WriteProperty("ShowEdit", m_ShowEdit, m_def_ShowEdit)

        PropBag.WriteProperty("Enabled", m_Enabled, m_def_Enabled)

        PropBag.WriteProperty("Visible", m_Visible, m_def_Visible)


        'Developer Guide No 2(No solution)
        PropBag.WriteProperty("Font", m_Font, Me.Font)
    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click
        'Developer Guide No. 69
        objfrmDetail = New frmDetail

        'Get the Rating Type Id
        Dim nItem As Integer = lvwAgentCommission.FocusedItem.Index + 1
        Dim lvItem As ListViewItem = lvwAgentCommission.FocusedItem

        'Update the details in the Detail form and show the detail for for editing
        objfrmDetail.Business = m_oBusiness
        'Start - Renuka - (WPR64 Paralleling)
        objfrmDetail.IsValue = gPMFunctions.ToSafeBoolean(ListViewHelper.GetListViewSubItem(lvItem, ACIsValue).Text)
        'End - Renuka - (WPR64 Paralleling)
        objfrmDetail.txtPremium.Text = ListViewHelper.GetListViewSubItem(lvItem, ACPremium).Text
        objfrmDetail.txtCommissionvalue.Text = ListViewHelper.GetListViewSubItem(lvItem, ACCommValue).Text
        'Start (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1.2)
        objfrmDetail.txtCommissionrate.Tag = ListViewHelper.GetListViewSubItem(lvItem, ACCommPercent).Text.Substring(0).Replace("%", "")
        If m_dInitialCommRate = 0 Then
            objfrmDetail.InitialCommRate = ToSafeDecimal(ListViewHelper.GetListViewSubItem(lvItem, ACCommPercent).Text.Substring(0).Replace("%", ""))
            m_dInitialCommRate = ToSafeDecimal(ListViewHelper.GetListViewSubItem(lvItem, ACCommPercent).Text.Substring(0).Replace("%", ""))
        Else
            objfrmDetail.InitialCommRate = m_dInitialCommRate
        End If
        If m_dInitialCommValue = 0 Then
            objfrmDetail.InitialCommValue = ToSafeDecimal(ListViewHelper.GetListViewSubItem(lvItem, ACCommValue).Text)
            m_dInitialCommValue = ToSafeDecimal(ListViewHelper.GetListViewSubItem(lvItem, ACCommValue).Text)
        Else
            objfrmDetail.InitialCommValue = m_dInitialCommValue
        End If

        'Changes done by: Krishna Nand
        'Dated: 12/02/2010
        'PN:68552
        'Purpose: convert Commission Rate upto 4 decimal places
        'End of changes done by Krishna Nand on 12/02/2010 for PN: 68552
        'End (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1.2)
        If objfrmDetail.IsValue Then
            'Recalculate commission percentage, in both cases - Commission is amended or not.
            objfrmDetail.txtCommissionrate.Tag = CStr(Math.Round((objfrmDetail.InitialCommValue * 100) / objfrmDetail.txtPremium.Text, 8))
            objfrmDetail.txtCommissionrate.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, Convert.ToString(objfrmDetail.txtCommissionrate.Tag), 10)
            'End of changes done by Krishna Nand on 12/02/2010 for PN: 68552
        Else
            objfrmDetail.txtCommissionrate.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, Convert.ToString(objfrmDetail.txtCommissionrate.Tag), 10)
        End If
        objfrmDetail.cboPartyAgentType.ItemId = CInt(ListViewHelper.GetListViewSubItem(lvItem, ACAgentTypeId).Text)
        objfrmDetail.cboRiskType.ItemId = CInt(ListViewHelper.GetListViewSubItem(lvItem, ACRiskTypeId).Text)
        objfrmDetail.cboCommissionBand.ItemId = CInt(ListViewHelper.GetListViewSubItem(lvItem, ACCommissionBandId).Text)

        objfrmDetail.cboTaxGroup.ItemId = gPMFunctions.ToSafeLong(ListViewHelper.GetListViewSubItem(lvItem, ACTaxGroupID).Text)
        If m_nTaxGroupItemId = 0 Then
            objfrmDetail.TaxGroupItemId = gPMFunctions.ToSafeLong(ListViewHelper.GetListViewSubItem(lvItem, ACTaxGroupID).Text)
            m_nTaxGroupItemId = gPMFunctions.ToSafeLong(ListViewHelper.GetListViewSubItem(lvItem, ACTaxGroupID).Text)
        Else
            objfrmDetail.TaxGroupItemId = m_nTaxGroupItemId
        End If

        objfrmDetail.txtTaxValue.Text = ListViewHelper.GetListViewSubItem(lvItem, ACTaxAmount).Text
        If m_dInitialTaxAmount = 0 Then
            objfrmDetail.InitialTaxAmount = ToSafeDecimal(ListViewHelper.GetListViewSubItem(lvItem, ACTaxAmount).Text)
            m_dInitialTaxAmount = ToSafeDecimal(ListViewHelper.GetListViewSubItem(lvItem, ACTaxAmount).Text)

        Else
            objfrmDetail.InitialTaxAmount = m_dInitialTaxAmount
        End If

        objfrmDetail.InsuranceFileCnt = m_lInsuranceFileCnt
        objfrmDetail.SourceID = m_iInsuranceFileSourceID
        objfrmDetail.CurrencyID = m_iInsuranceFileCurrencyID

        'PN_68557 Start Jeetendra
        objfrmDetail.CalculatedCommissionValue = CDec(ListViewHelper.GetListViewSubItem(lvItem, ACOrigCommissionRate).Text)
        objfrmDetail.txtTaxValue.Tag = ListViewHelper.GetListViewSubItem(lvItem, ACOrigTaxValue).Text
        objfrmDetail.cboTaxGroup.Tag = CStr(gPMFunctions.ToSafeLong(ListViewHelper.GetListViewSubItem(lvItem, ACOrigTaxGroup).Text))
        'PN_68557 End

        'frmDetail.CalculatedCommissionValue = ToSafeCurrency(lvItem.SubItems(ACCalculatedCommissionValue))

        objfrmDetail.OverrideReason = gPMFunctions.ToSafeString(ListViewHelper.GetListViewSubItem(lvItem, ACOverrideReason).Text)
        'Start - Renuka - (WPR64 Paralleling)
        objfrmDetail.MaximumRateValue = gPMFunctions.ToSafeCurrency(ListViewHelper.GetListViewSubItem(lvItem, ACMaximumRate).Text)
        'End - Renuka - (WPR64 Paralleling)
        m_lReturn = CType(objfrmDetail.CheckCommissionOverride(), gPMConstants.PMEReturnCode)
        'Start - Renuka - (WPR64 Paralleling)
        If gPMFunctions.ToSafeCurrency(ListViewHelper.GetListViewSubItem(lvItem, ACMaximumRate).Text) > 0 Then
            If gPMFunctions.ToSafeBoolean(ListViewHelper.GetListViewSubItem(lvItem, ACIsValue).Text) Then
                objfrmDetail.txtCommissionvalue.Enabled = True
                objfrmDetail.txtCommissionrate.Enabled = False
            Else
                objfrmDetail.txtCommissionvalue.Enabled = False
                objfrmDetail.txtCommissionrate.Enabled = True
            End If
        End If
        'End - Renuka - (WPR64 Paralleling)

        'Start (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1.2)
        objfrmDetail.IsTaxAmended = CInt(gPMFunctions.ToSafeString(ListViewHelper.GetListViewSubItem(lvItem, ACIsTaxAmended).Text))
        m_lReturn = CType(objfrmDetail.CheckCommissionTaxOverride(), gPMConstants.PMEReturnCode)
        'End (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1.2)


        objfrmDetail.lvwAgentCommission = lvwAgentCommission
        objfrmDetail.ReadOnly_Renamed = m_bReadOnly


        iPMFunc.CenterForm(objfrmDetail)

        objfrmDetail.frmDetailLoad()
        m_lReturn = CType(SetComboItem(objfrmDetail.cboAgent, CInt(ListViewHelper.GetListViewSubItem(lvItem, ACAgentCnt).Text)), gPMConstants.PMEReturnCode)
        'Show the Details form in Edit mode
        objfrmDetail.ShowDialog()

        'PM 11-Dec-2006
        'PN#31954: Recalculate only if Ok pressed
        If objfrmDetail.Status = gPMConstants.PMEReturnCode.PMOK And Not m_bReadOnly Then
            'Save immediately
            Save()

            'Recalculate the total
            CalculateTotal()
            'Start (Saurabh Agrawal) PN54244
            RaiseEvent Change(Me, Nothing)
            'End (Saurabh Agrawal) PN54244
            'PN#31954: Remove the check from here because we can have more than one sub-agents attached
            'PN:28198 Don't allow changes if Total Commission > Premium
            '        If CCur(txtTotalCommission.Text) > CCur(lvItem.SubItems(ACPremium)) Then
            '            MsgBox "Total Commission exceeds the Total Premium." & vbCrLf & "Please amend before proceeding", vbInformation, " Amend Commission Value"
            '            Call cmdEdit_Click
            '        End If
        Else
            'Either Canceled or view mode come out of commision form
            'All cancel and view mode form related code will go here
        End If
    End Sub

    Private Function SetComboItem(ByRef cboCombo As ComboBox, ByVal v_lItemData As Integer) As Integer

        For nCount As Integer = 0 To cboCombo.Items.Count - 1
            If VB6.GetItemData(cboCombo, nCount) = v_lItemData Then
                cboCombo.SelectedIndex = nCount
            End If
        Next
    End Function

    ' ***************************************************************** '
    ' Name: Initialise
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
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

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "g_oOBjectManager.Initialise Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' If UserID is 0 assume that user cancelled logon
            If g_oObjectManager.UserID = 0 Then
                ' Exit application
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            ' Store the language ID from the object manager to the public variables,
            ' to enable us to use them throughout the object.
            With g_oObjectManager
                m_iLanguageID = .LanguageID
                m_iSourceID = .SourceID
                m_iUserId = .UserID
            End With

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Get an instance of the business object via the public object manager.
            Dim temp_m_oBusiness As Object
            lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSirAgentCommission.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetInstance of bSIRRITax.Business Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_bReadOnly = False

            ' hold Initialised status
            bIsInitialised = True


        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: Load
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    ' ***************************************************************** '
    Public Function Load_Renamed(Optional ByVal v_iTask As Integer = gPMConstants.PMEComponentAction.PMView) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "Load"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            m_iTask = v_iTask
            m_bReadOnly = (m_iTask = gPMConstants.PMEComponentAction.PMView)

            If m_lInsuranceFileCnt = 0 Then
                gPMFunctions.RaiseError(kMethodName, "Invalid Properties Set", gPMConstants.PMELogLevel.PMLogError)
            End If

            lReturn = SetupBusiness()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetupBusiness Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            lReturn = SetInterfaceDefaults()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetInterfaceDefaults Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            lReturn = CType(GetCommission(m_bReadOnly), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetCommission Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            lReturn = PopulateCommission()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "PopulateTaxes Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Bubble the event up
            RaiseEvent Change(Me, Nothing)



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

    Private Function GetCommission(ByRef bReadOnly As Boolean) As Integer
        ' ---------------------------------------------------------------------------
        ' NAME: GetCommission
        ' DESCRIPTION:
        ' AUTHOR: Danny Davis
        ' DATE: 04 May 2005, 11:21:16
        ' HISTORY:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Const kMethodName As String = "GetCommission"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_oBusiness.InsuranceFileCnt = m_lInsuranceFileCnt

            'Retrieve or Calculate Commission
            If bReadOnly Then
                m_lReturn = m_oBusiness.GetAgentCommission(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, r_vntResult:=m_vAgentCommission)
            Else
                m_lReturn = m_oBusiness.CalculateAgentCommission(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_sTransactionType:=m_sTransactionType, r_vntResult:=m_vAgentCommission)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetCommission Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oBusiness.GetInsuranceHeaderDetails(r_sInsuranceHolderShortName:=m_sInsuranceHolderShortName, r_sInsuranceHolderName:=m_sInsuranceHolderName, r_sInsuranceHolderResolvedName:=m_sInsuranceHolderResolvedName, r_sInsuranceRef:=m_sInsuranceRef, r_sInsuranceFolderDescription:=m_sInsuranceFolderDescription, r_sInsuranceCurrencyCode:=m_sInsuranceCurrencyCode, r_sInsuranceCurrencyCaption:=m_sInsuranceCurrencyCaption, r_iInsuranceSourceID:=m_iInsuranceFileSourceID, r_iInsuranceCurrencyID:=m_iInsuranceFileCurrencyID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetCommission Failed", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally





        End Try
        Return result
    End Function

    Private Function PopulateCommission() As Integer
        ' ---------------------------------------------------------------------------
        ' NAME: PopulateCommission
        ' DESCRIPTION:
        ' AUTHOR: Danny Davis
        ' DATE: 04 May 2005, 11:21:51
        ' HISTORY:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Const kMethodName As String = "PopulateCommission"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Forms load event.
            Dim sTitle, sShortName, sInsHolderName, sResolvedName, sInsRef, sInsFolderDesc, sInsCurrencyCode, sInsCurrencyCaption As String
            Dim cThisPremium As Decimal
            Dim lvItem As ListViewItem

            'Fill the retrieved data into the List view
            lvwAgentCommission.Items.Clear()

            If Information.IsArray(m_vAgentCommission) Then

                If Information.IsArray(m_vAgentCommission) Then
                    'Store the Premium from the first entry
                    m_cTotalPremium = Conversion.Val(CStr(m_vAgentCommission(ACPremium, m_vAgentCommission.GetLowerBound(1))))

                    For iCount As Integer = m_vAgentCommission.GetLowerBound(1) To m_vAgentCommission.GetUpperBound(1)
                        lvItem = lvwAgentCommission.Items.Add(CStr(m_vAgentCommission(ACAgent, iCount)))

                        ListViewHelper.GetListViewSubItem(lvItem, ACAgentType).Text = CStr(m_vAgentCommission(ACAgentType, iCount))
                        ListViewHelper.GetListViewSubItem(lvItem, ACRiskType).Text = CStr(m_vAgentCommission(ACRiskType, iCount))
                        ListViewHelper.GetListViewSubItem(lvItem, ACCommissionBand).Text = CStr(m_vAgentCommission(ACCommissionBand, iCount))

                        ListViewHelper.GetListViewSubItem(lvItem, ACCurrency).Text = CStr(m_vAgentCommission(ACCurrency, iCount)).Trim()

                        'Format the premium
                        ListViewHelper.GetListViewSubItem(lvItem, ACPremium).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatMoney, CStr(m_vAgentCommission(ACPremium, iCount)), 2)
                        'Start - Renuka - (WPR64 Paralleling)
                        'Format the Commission percentage
                        'lvItem.SubItems(ACCommValue) = FormatField(PMFormatMoney, m_vAgentCommission(ACCommValue, iCount))
                        If Not String.IsNullOrEmpty(m_vAgentCommission(ACIsValue, iCount)) AndAlso m_vAgentCommission(ACIsValue, iCount) = 1 Then
                            ListViewHelper.GetListViewSubItem(lvItem, ACCommValue).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatMoney, CStr(m_vAgentCommission(ACCommPercent, iCount)))
                            If m_vAgentCommission(ACCommPercent, iCount) = 0 Or m_vAgentCommission(ACPremium, iCount) = 0 Then
                                ListViewHelper.GetListViewSubItem(lvItem, ACCommPercent).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(0), 10)
                            Else
                                ListViewHelper.GetListViewSubItem(lvItem, ACCommPercent).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr((CDbl(m_vAgentCommission(ACCommPercent, iCount))) / (CDbl(m_vAgentCommission(ACPremium, iCount))) * 100), 10)
                            End If
                        Else
                            ListViewHelper.GetListViewSubItem(lvItem, ACCommValue).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatMoney, CStr(m_vAgentCommission(ACCommValue, iCount)))
                            ListViewHelper.GetListViewSubItem(lvItem, ACCommPercent).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, CStr(m_vAgentCommission(ACCommPercent, iCount)), 10)
                        End If
                        'End - Renuka - (WPR64 Paralleling)
                        'Format the Commission value

                        ListViewHelper.GetListViewSubItem(lvItem, ACCommValue).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatMoney, CStr(m_vAgentCommission(ACCommValue, iCount)))
                        ListViewHelper.GetListViewSubItem(lvItem, ACRawCommValue).Text = ToSafeDouble(m_vAgentCommission(ACCommValue, iCount))
                        If Not String.IsNullOrEmpty(m_vAgentCommission(ACIsLeadAgent, iCount)) AndAlso m_vAgentCommission(ACIsLeadAgent, iCount) = 1 Then
                            ListViewHelper.GetListViewSubItem(lvItem, ACIsLeadAgent).Text = "Yes"
                        Else
                            ListViewHelper.GetListViewSubItem(lvItem, ACIsLeadAgent).Text = "No"
                        End If

                        ListViewHelper.GetListViewSubItem(lvItem, ACIsAmended).Text = CStr(m_vAgentCommission(ACIsAmended, iCount))
                        ListViewHelper.GetListViewSubItem(lvItem, ACAgentCnt).Text = CStr(m_vAgentCommission(ACAgentCnt, iCount))
                        ListViewHelper.GetListViewSubItem(lvItem, ACAgentTypeId).Text = CStr(m_vAgentCommission(ACAgentTypeId, iCount))
                        ListViewHelper.GetListViewSubItem(lvItem, ACRiskTypeId).Text = CStr(m_vAgentCommission(ACRiskTypeId, iCount))
                        ListViewHelper.GetListViewSubItem(lvItem, ACCommissionBandId).Text = CStr(m_vAgentCommission(ACCommissionBandId, iCount))
                        ListViewHelper.GetListViewSubItem(lvItem, ACCurrency).Text = CStr(m_vAgentCommission(ACCurrency, iCount))
                        ListViewHelper.GetListViewSubItem(lvItem, ACTaxGroupID).Text = CStr(m_vAgentCommission(ACTaxGroupID, iCount))
                        ListViewHelper.GetListViewSubItem(lvItem, ACTaxGroupDescription).Text = CStr(m_vAgentCommission(ACTaxGroupDescription, iCount))
                        ListViewHelper.GetListViewSubItem(lvItem, ACTaxAmount).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatMoney, CStr(m_vAgentCommission(ACTaxAmount, iCount)), 2)
                        ListViewHelper.GetListViewSubItem(lvItem, ACRawTaxAmount).Text = ToSafeDouble(m_vAgentCommission(ACTaxAmount, iCount))
                        ListViewHelper.GetListViewSubItem(lvItem, ACCalculatedCommissionValue).Text = CStr(m_vAgentCommission(ACCalculatedCommissionValue, iCount))
                        ListViewHelper.GetListViewSubItem(lvItem, ACOverrideReason).Text = CStr(m_vAgentCommission(ACOverrideReason, iCount))
                        'Start - Renuka - (WPR64 Paralleling)
                        ListViewHelper.GetListViewSubItem(lvItem, ACMaximumRate).Text = CStr(m_vAgentCommission(ACMaximumRate, iCount))
                        ListViewHelper.GetListViewSubItem(lvItem, ACIsValue).Text = CStr(m_vAgentCommission(ACIsValue, iCount))
                        'End - Renuka - (WPR64 Paralleling)
                        'Start (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1.2)
                        'As per section 6.2 of tech spec, isTaxAmended flag should be false initially
                        ListViewHelper.GetListViewSubItem(lvItem, ACIsTaxAmended).Text = CStr(0)
                        'End (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1.2)

                        'PN_68557 Start Jeetendra
                        ListViewHelper.GetListViewSubItem(lvItem, ACOrigCommissionRate).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatMoney, CStr(m_vAgentCommission(ACCommValue, iCount)))
                        ListViewHelper.GetListViewSubItem(lvItem, ACOrigTaxGroup).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatMoney, CStr(m_vAgentCommission(ACTaxGroupID, iCount)))
                        ListViewHelper.GetListViewSubItem(lvItem, ACOrigTaxValue).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatMoney, CStr(m_vAgentCommission(ACTaxAmount, iCount)))
                        'PN_68557 End ACOrigTaxGroup

                        ListViewHelper.GetListViewSubItem(lvItem, ACPerilTypeId).Text = CStr(m_vAgentCommission(ACOrgPerilTypeId, iCount))
                        ListViewHelper.GetListViewSubItem(lvItem, ACClassOfBusinessId).Text = CStr(m_vAgentCommission(ACOrgClassOfBusinessId, iCount))
                    Next

                    'Recalculate the total field
                    CalculateTotal()
                End If
            End If

            ' Set the interface default values.
            m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return result
            End If




        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally





        End Try
        Return result
    End Function


    Private Function Save() As Integer
        ' ---------------------------------------------------------------------------
        ' NAME: Save
        ' DESCRIPTION: Save the Commission back to the database
        ' AUTHOR: Danny Davis
        ' DATE: 04 May 2005, 11:36:23
        ' HISTORY:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Const kMethodName As String = "Save"


        Try

            Dim nIsLeadAgent, nIsAmended As Integer
            Dim lPartyCnt, lRiskTypeId, lCommissionBandId, nPerilTypeId, nClassOfBusinessId As Integer
            Dim cPremium, cCommissionPercentage, cCommissionValue As Decimal
            Dim lTaxGroupID As Integer

            Dim cCalculatedCommissionValue As Decimal
            Dim sOverrideReason As String = ""

            'Start - Renuka - (WPR64 Paralleling)
            Dim cMaximumRate As Decimal
            Dim lIsValue As Integer
            'End - Renuka - (WPR64 Paralleling)
            'Start (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1.2)
            Dim nIsTaxAmended As Integer
            Dim cAmendedTaxValue As Decimal
            'End (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1.2)

            Dim lvItem As ListViewItem

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_bReadOnly Then
                Return result
            End If

            lblStatus.Text = ""
            'Start (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1.2)
            lblTaxAmendedStatus.Text = ""
            'End (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1.2)




            m_lReturn = m_oBusiness.DeleteAgentCommission(v_lInsuranceFileCnt:=m_lInsuranceFileCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "DeleteAgentCommission Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            For iCount As Integer = 1 To lvwAgentCommission.Items.Count
                lvItem = lvwAgentCommission.Items.Item(iCount - 1)

                lPartyCnt = CInt(ListViewHelper.GetListViewSubItem(lvItem, ACAgentCnt).Text)
                lRiskTypeId = CInt(ListViewHelper.GetListViewSubItem(lvItem, ACRiskTypeId).Text)

                Dim auxVar As String = ListViewHelper.GetListViewSubItem(lvItem, ACCommissionBandId).Text

                If Convert.IsDBNull(auxVar) Or IsNothing(auxVar) Then
                    MessageBox.Show("Error - commission band id is null.", "Invalid Commission Band", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    lCommissionBandId = 1
                Else
                    lCommissionBandId = CInt(ListViewHelper.GetListViewSubItem(lvItem, ACCommissionBandId).Text)
                End If

                nIsLeadAgent = IIf(ListViewHelper.GetListViewSubItem(lvItem, ACIsLeadAgent).Text = "Yes", 1, 0)

                cPremium = gPMFunctions.ConvertCurrencyStringToValue(ListViewHelper.GetListViewSubItem(lvItem, ACPremium).Text)
                cCommissionValue = gPMFunctions.ConvertCurrencyStringToValue(ListViewHelper.GetListViewSubItem(lvItem, ACCommValue).Text)

                If ListViewHelper.GetListViewSubItem(lvItem, ACCommPercent).Text = "0.00%" Then
                    cCommissionPercentage = 0
                Else
                    cCommissionPercentage = Conversion.Val(ListViewHelper.GetListViewSubItem(lvItem, ACCommPercent).Text.Substring(0).Replace("%", ""))
                End If

                lTaxGroupID = CInt(Conversion.Val(ListViewHelper.GetListViewSubItem(lvItem, ACTaxGroupID).Text))


                nIsAmended = gPMFunctions.ToSafeInteger(ListViewHelper.GetListViewSubItem(lvItem, ACIsAmended).Text)
                If nIsAmended = 1 Then
                    lblStatus.Text = "WARNING: Commission has been amended"
                End If
                cCalculatedCommissionValue = gPMFunctions.ToSafeCurrency(ListViewHelper.GetListViewSubItem(lvItem, ACCalculatedCommissionValue).Text)
                sOverrideReason = gPMFunctions.ToSafeString(ListViewHelper.GetListViewSubItem(lvItem, ACOverrideReason).Text)
                'Start - Renuka - (WPR64 Paralleling)
                cMaximumRate = gPMFunctions.ToSafeCurrency(ListViewHelper.GetListViewSubItem(lvItem, ACMaximumRate).Text)
                lIsValue = gPMFunctions.ToSafeInteger(ListViewHelper.GetListViewSubItem(lvItem, ACIsValue).Text)
                'End - Renuka - (WPR64 Paralleling)

                'Start (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1.2)
                nIsTaxAmended = gPMFunctions.ToSafeInteger(ListViewHelper.GetListViewSubItem(lvItem, ACIsTaxAmended).Text)
                If nIsTaxAmended = 1 Then
                    lblTaxAmendedStatus.Text = "WARNING: Tax Value has been amended"
                    cAmendedTaxValue = gPMFunctions.ConvertCurrencyStringToValue(ListViewHelper.GetListViewSubItem(lvItem, ACTaxAmount).Text)
                    lTaxGroupID = gPMFunctions.ToSafeLong(ListViewHelper.GetListViewSubItem(lvItem, ACTaxGroupID).Text)
                Else
                    cAmendedTaxValue = 0
                End If

                nPerilTypeId = gPMFunctions.ToSafeInteger(ListViewHelper.GetListViewSubItem(lvItem, ACPerilTypeId).Text)
                nClassOfBusinessId = gPMFunctions.ToSafeInteger(ListViewHelper.GetListViewSubItem(lvItem, ACClassOfBusinessId).Text)

                'Start - Renuka - (WPR64 Paralleling) - Pass cMaximumRate and lIsValue to the database
                'Add the record in the database

                m_lReturn = m_oBusiness.AddAgentCommission(m_lInsuranceFileCnt, nIsLeadAgent, lPartyCnt, lRiskTypeId, lCommissionBandId, cPremium, cCommissionPercentage, cCommissionValue, nIsAmended, lTaxGroupID, cCalculatedCommissionValue, sOverrideReason, cMaximumRate, lIsValue, nIsTaxAmended, cAmendedTaxValue, nPerilTypeId, nClassOfBusinessId)
                'End - Renuka - (WPR64 Paralleling)
                'End (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1.2)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'Quit the processing if the add/edit failed.
                    Exit For
                End If
            Next

            'Update the lead commission records for the insurancefile
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                m_lReturn = m_oBusiness.UpdateLeadCommission(m_lInsuranceFileCnt)
            End If

            'Set the return Value
            result = m_lReturn



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally





        End Try
        Return result
    End Function

    Private Sub lvwAgentCommission_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwAgentCommission.DoubleClick

        If Not (lvwAgentCommission.FocusedItem Is Nothing) Then
            'Renuka - (WPR64 Paralleling) - Added one more condition in IF clause
            If lvwAgentCommission.FocusedItem.Text.Trim() <> "SSPSUBAGENT" And m_bUserHasAuthority Then
                cmdEdit_Click(cmdEdit, New EventArgs())
            End If
        End If

    End Sub

    Private Sub CalculateTotal()
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: CalculateTotal
        ' PURPOSE: Calculates the total commission
        ' AUTHOR: Danny Davis
        ' DATE: 11 February 2004, 17:19:11
        ' CHANGES:
        ' ---------------------------------------------------------------------------


        Try

            Dim cTotal, cTotalTax As Decimal

            Dim cleadAgentTC, cleadAgentTT, cSubAgentTC, cSubAgentTT As Decimal

            cTotal = 0
            cTotalTax = 0
            For Each oRow As ListViewItem In lvwAgentCommission.Items
                cTotal += CDec(ListViewHelper.GetListViewSubItem(oRow, ACRawCommValue).Text)
                cTotalTax += CDec(ListViewHelper.GetListViewSubItem(oRow, ACRawTaxAmount).Text)

                'PK
                If ListViewHelper.GetListViewSubItem(oRow, ACIsLeadAgent).Text = "Yes" Then
                    cleadAgentTT += CDec(ListViewHelper.GetListViewSubItem(oRow, ACRawTaxAmount).Text)
                    cleadAgentTC += CDec(ListViewHelper.GetListViewSubItem(oRow, ACRawCommValue).Text)

                Else
                    cSubAgentTT += CDec(ListViewHelper.GetListViewSubItem(oRow, ACRawTaxAmount).Text)
                    cSubAgentTC += CDec(ListViewHelper.GetListViewSubItem(oRow, ACRawCommValue).Text)
                End If

            Next oRow

            m_cTotalCommission = Math.Round(cTotal, 2)
            m_cTotalTax = Math.Round(cTotalTax, 2)

            'PK

            txtLeadAgentTotalCommission.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(cleadAgentTC))
            txtLeadAgentTotalTax.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(cleadAgentTT))
            txtLeadAgentNet.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(m_cGrossTotal - (cleadAgentTT + cleadAgentTC)))
            m_cLeadAgentTC = CDec(gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(cleadAgentTC)))
            m_cLeadAgentTT = CDec(gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(cleadAgentTT)))

            txtSubAgentTotalCommission.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(cSubAgentTC))
            txtSubAgentTotalTax.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(cSubAgentTT))

            txtSubAgentNet.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(cSubAgentTT + cSubAgentTC))

            'txtTotalComm.Text = FormatField(PMFormatCurrency, (cleadAgentTC + cSubAgentTC))
            'txtTotalTax.Text = FormatField(PMFormatCurrency, (cleadAgentTT + cSubAgentTT))
            'txtTotalNetCommission.Text = FormatField(PMFormatCurrency, (CCur(txtLeadAgentNet.Text) + CCur(txtSubAgentNet.Text)))


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="CalculateTotal", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    Exit Sub
            End Select

        Finally



        End Try
        Exit Sub
    End Sub


    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 25-04-2005 : AUS005
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetInterfaceDefaults"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = SetupButtons()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetupButtons Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            lReturn = SetUpListView()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetUpListView Failed", gPMConstants.PMELogLevel.PMLogError)
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
    ' Name: SetProcessModes
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer


        Dim result As Integer = 0
        Const kMethodName As String = "SetProcessModes"

        Dim lReturn As Integer

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
    ' Name: SetupBusiness
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    ' ***************************************************************** '
    Private Function SetupBusiness() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupBusiness"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the process modes for the busines object.

            lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetupBusiness Failed", gPMConstants.PMELogLevel.PMLogError)
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
    ' Name: SetupButtons
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    ' ***************************************************************** '
    Private Function SetupButtons() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupButtons"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lItemId As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_bReadOnly Then
                cmdEdit.Text = "View"
            End If

            lReturn = CType(GetSelectedItem(lItemId, lvwAgentCommission), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetSelectedItem Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            'Start - Renuka - (WPR64 Paralleling)
            lReturn = CType(GetEditDefaultCommissionAuthority(m_bUserHasAuthority), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetEditDefaultCommissionAuthority Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            cmdEdit.Enabled = m_bUserHasAuthority And Not m_bReadOnly And lItemId <> -1
            'End - Renuka - (WPR64 Paralleling)


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SetUpListView
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    ' ***************************************************************** '
    Private Function SetUpListView() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetUpListView"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            lvwAgentCommission.FullRowSelect = True

            lvwAgentCommission.Columns.Clear()

            lvwAgentCommission.Columns.Add("kCommissionColHeaderAgent", "Agent", CInt(VB6.TwipsToPixelsX(1500)))
            lvwAgentCommission.Columns.Add("kCommissionColHeaderAgentType", "Agent type", CInt(VB6.TwipsToPixelsX(1050)))
            lvwAgentCommission.Columns.Add("kCommissionColHeaderRiskType", "Risk Type", CInt(VB6.TwipsToPixelsX(1500)))
            lvwAgentCommission.Columns.Add("kCommissionColHeaderCommissionBand", "Commission Band", CInt(VB6.TwipsToPixelsX(1300)), HorizontalAlignment.Right, -1)
            lvwAgentCommission.Columns.Add("kCommissionColHeaderPremium", "Premium", CInt(VB6.TwipsToPixelsX(1800)))
            lvwAgentCommission.Columns.Add("kCommissionColHeaderCommissionRate", "Commission rate", CInt(VB6.TwipsToPixelsX(1000)), HorizontalAlignment.Right, -1)
            lvwAgentCommission.Columns.Add("kCommissionColHeaderCommissionValue", "Commission Value", CInt(VB6.TwipsToPixelsX(1800)))
            lvwAgentCommission.Columns.Add("kCommissionColHeaderIsLeadAgent", "Is Lead Agent", CInt(VB6.TwipsToPixelsX(1800)))
            lvwAgentCommission.Columns.Add("kCommissionColHeaderIsAmended", "Is Amended?", CInt(VB6.TwipsToPixelsX(0)))
            lvwAgentCommission.Columns.Add("kCommissionColHeaderIsPartyID", "Party Id", CInt(VB6.TwipsToPixelsX(0)))
            lvwAgentCommission.Columns.Add("kCommissionColHeaderIsAgentTypeID", "Agent Type Id", CInt(VB6.TwipsToPixelsX(0)))
            lvwAgentCommission.Columns.Add("kCommissionColHeaderIsRiskTypeID", "Risk Type ID", CInt(VB6.TwipsToPixelsX(0)))
            lvwAgentCommission.Columns.Add("kCommissionColHeaderIsCommissionBandID", "Commission Band ID", CInt(VB6.TwipsToPixelsX(0)))
            lvwAgentCommission.Columns.Add("kCommissionColHeaderIsCurrency", "Currency", CInt(VB6.TwipsToPixelsX(0)))
            lvwAgentCommission.Columns.Add("kCommissionColHeaderIsTaxGroup", "Tax Group ID", CInt(VB6.TwipsToPixelsX(0)))
            lvwAgentCommission.Columns.Add("kCommissionColHeaderIsTaxGroupDescription", "Tax Group", CInt(VB6.TwipsToPixelsX(1500)))
            lvwAgentCommission.Columns.Add("kCommissionColHeaderIsTaxValue", "Tax Value", CInt(VB6.TwipsToPixelsX(1500)))
            lvwAgentCommission.Columns.Add("kCommissionColHeaderCalcualtedCommissionValue", "Calculated Commission Value", CInt(VB6.TwipsToPixelsX(0)))
            lvwAgentCommission.Columns.Add("kCommissionColHeaderOverrideReason", "Override Reason", CInt(VB6.TwipsToPixelsX(0)))
            'Start - Renuka - (WPR64 Paralleling)
            lvwAgentCommission.Columns.Add("kCommissionColHeaderMaximumRate", "Maximum Rate", CInt(VB6.TwipsToPixelsX(0)))
            lvwAgentCommission.Columns.Add("kCommissionColHeaderIsValue", "IsValue", CInt(VB6.TwipsToPixelsX(0)))
            'End - Renuka - (WPR64 Paralleling)
            'Start (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1.2)
            lvwAgentCommission.Columns.Add("kCommissionColHeaderIsTaxAmended", "IsTaxAmended", CInt(VB6.TwipsToPixelsX(0)))
            'End (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1.2)

            'PN_68557 Start
            lvwAgentCommission.Columns.Add("OrigCommissionRateValue", "OrigCommissionRateValue", CInt(VB6.TwipsToPixelsX(0)))
            lvwAgentCommission.Columns.Add("OrigTaxGroup", "OrigTaxGroup", CInt(VB6.TwipsToPixelsX(0)))
            lvwAgentCommission.Columns.Add("OrigTaxValue", "OrigTaxValue", CInt(VB6.TwipsToPixelsX(0)))
            'PN_68557 End

            lvwAgentCommission.Columns.Add("PerilTypeId", "PerilTypeId", CInt(VB6.TwipsToPixelsX(0)))
            lvwAgentCommission.Columns.Add("ClassOfBusinessId", "ClassOfBusinessId", CInt(VB6.TwipsToPixelsX(0)))
            lvwAgentCommission.Columns.Add("RawCommValue", "RawCommValue", CInt(VB6.TwipsToPixelsX(0)))
            lvwAgentCommission.Columns.Add("RawTaxAmount", "RawTaxAmount", CInt(VB6.TwipsToPixelsX(0)))

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetSelectedItem
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 25-04-2005 : AUS005
    ' ***************************************************************** '
    Private Function GetSelectedItem(ByRef r_lItemId As Integer, ByVal oList As ListView) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetSelectedItem"

        Dim lReturn As Integer
        Dim bItemSelected As Boolean

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            r_lItemId = -1

            ' determine if there are any selected items
            For lItem As Integer = 1 To oList.Items.Count
                If oList.Items.Item(lItem - 1).Selected Then
                    If oList.Items.Item(lItem - 1).Text.Trim() <> "SSPSUBAGENT" Then
                        r_lItemId = CInt(Conversion.Val(Convert.ToString(oList.Items.Item(lItem - 1).Tag)))
                    Else
                        r_lItemId = -1
                    End If
                    Exit For
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
    ' Name: Recalculate
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 25-04-2005 : AUS005
    ' ***************************************************************** '
    Public Function Recalculate() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "Recalculate"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            lblStatus.Text = ""

            'Start (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1.2)
            lblTaxAmendedStatus.Text = ""
            'End (Prakash Varghese) - (Tech Spec - TRAC 2789 Agent Commission Tax.docx) - (6.1.2)

            ' NB : mode passed in via set process modes is ignored.
            ' Recalculate just calls "Load" in edit mode
            ' (this will regenerate the tax entries for either risk or policy)
            lReturn = CType(Load_Renamed(v_iTask:=gPMConstants.PMEComponentAction.PMEdit), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Load Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost
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
    ' Name: CheckCommissionAgainstPremium
    '
    ' Parameters: n/a
    '
    ' Description: Check if any rows in the list view have Commission Values
    '              greater than the premium.
    '
    ' History:
    '           Created : HG05092005 PN
    ' ***************************************************************** '
    Public Function CheckCommissionAgainstPremium() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CheckCommissionAgainstPremium"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            With lvwAgentCommission
                If .Items.Count = 0 Then
                    Return result
                End If

                For lCount As Integer = 1 To .Items.Count
                    If Math.Abs(gPMFunctions.ToSafeCurrency(ListViewHelper.GetListViewSubItem(.Items.Item(lCount - 1), ACCommValue).Text, 0)) > Math.Abs(gPMFunctions.ToSafeCurrency(ListViewHelper.GetListViewSubItem(.Items.Item(lCount - 1), ACPremium).Text, 0)) Then
                        MessageBox.Show("The Commission exceeds the total Premium for Agent " & .Items.Item(lCount - 1).Text & Strings.Chr(13) & Strings.Chr(10) & "Please amend before Proceeding", "Amend Commission Value", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        result = False
                        Return result
                    End If
                Next lCount
            End With


        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ''' <summary>
    ''' gets default commission authority
    ''' </summary>
    ''' <param name="r_bUserHasAuthority"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetEditDefaultCommissionAuthority(ByRef r_bUserHasAuthority As Boolean) As Integer
        Dim nResult As Integer


        Const kMethodName As String = "GetEditDefaultCommissionAuthority"

        Dim sResult As String = ""
        Dim oObjectManager As bObjectManager.ObjectManager

        Dim oBusinessDefault As bACTUserAuthorities.Business
        Dim sColumnName As String
        Try


            r_bUserHasAuthority = False

            nResult = gPMConstants.PMEReturnCode.PMTrue


            oObjectManager = New bObjectManager.ObjectManager()

            nResult = oObjectManager.Initialise(sCallingAppName:=ACApp)

            '   If not initialised then call error handler
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then

                nResult = gPMConstants.PMEReturnCode.PMError
                oObjectManager = Nothing
                gPMFunctions.RaiseError(kMethodName, "ObjectManager Initialise Failed",
                                        gPMConstants.PMELogLevel.PMLogError)

                Return nResult

            End If

            '   Find the Business Class
            Dim temp_oBusinessDefault As Object
            nResult = oObjectManager.GetInstance(temp_oBusinessDefault, "bACTUserAuthorities.Business",
                                                 vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oBusinessDefault = temp_oBusinessDefault

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMError
                oObjectManager = Nothing
                gPMFunctions.RaiseError(kMethodName, "ObjectManager Get instance Failed",
                                        gPMConstants.PMELogLevel.PMLogError)
                Return nResult
            End If
            If m_sTransactionType = "MTA" Then
                sColumnName = "edit_default_commission_MTA"
            ElseIf m_sTransactionType = "MTC" Then
                sColumnName = "edit_default_commission_MTC"
            ElseIf m_sTransactionType = "MTR" Then
                sColumnName = "edit_default_commission_MTR"
            ElseIf m_sTransactionType = "NB" Or m_sTransactionType = "REN" Then
                sColumnName = "edit_default_commission_NB_RN"
            Else
                sColumnName = "edit_default_commission"
            End If
            nResult = oBusinessDefault.GetValueFromTable("User_Authorities", sColumnName, "user_id",
                                                         v_sKeyValue:=g_oObjectManager.UserID,
                                                         v_iDataType:=gPMConstants.PMEDataType.PMInteger,
                                                         r_vResult:=sResult)

            If nResult = gPMConstants.PMEReturnCode.PMTrue Then
                r_bUserHasAuthority = (gPMFunctions.ToSafeLong(sResult, 0) = 1)
                m_bUserHasAuthority = r_bUserHasAuthority
            Else
                nResult = gPMConstants.PMEReturnCode.PMError
                oObjectManager = Nothing
                gPMFunctions.RaiseError(kMethodName, "GetValueFromTable Failed", gPMConstants.PMELogLevel.PMLogError)

            End If

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMFalse

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetEditDefaultCommissionAuthority", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function
End Class
