Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports SharedFiles
Imports System.Runtime.InteropServices

<System.Runtime.InteropServices.ProgId("uctPMUFees_NET.uctPMUFees")> _
Partial Public Class uctPMUFees
    Inherits System.Windows.Forms.UserControl

    Private Const ACClass As String = "uctPMUFees"

    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iUserId As Integer
    Private m_lCurrencyID As Integer

    'Win32 API declarations to preserve list view horizontal scroll position after sort
    Const LVM_FIRST As Int32 = &H1000
    Const LVM_SCROLL As Int32 = LVM_FIRST + 20
    Const SBS_HORZ As Integer = 0

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

    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_iTask As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_lReturn As Integer
    Private m_oBusiness As Object
    Private m_oFormFields As iPMFormControl.FormFields
    Private m_ListViewArray As Object
    Private m_bStatus As Boolean
    Private m_sPaymentTerms As String
    Private m_sPaymentMethod As String

    Private hScrollValue As Integer = 0

    Public Event Change(ByVal Sender As Object, ByVal e As EventArgs)
    Public Event AboutToChange(ByVal Sender As Object, ByVal e As EventArgs)

    Private m_lInsuranceFileCnt As Integer
    Private m_lRiskCnt As Integer
    Private m_bReadOnly As Boolean
    Private m_crTotalTax As Decimal
    Private m_crTotalFees As Decimal
    Private m_sDescription As String = ""
    Private m_vFeeDetails(,) As Object

    Private m_crPremium As Decimal
    Private m_crAnnualPremium As Decimal
    Private m_dtPolicyCoverStartDate As Date
    Private m_dtPolicyExpiryDate As Date
    Private m_lTransTypeId As Integer
    Private m_lProductId As Integer

    Private m_vCurrency As Object
    Private m_vTaxGroup As Object
    Private m_sAppliesTo As String = ""
    Private m_bCancelAboutToChangeAction As Boolean
    Private m_sMakeLivePaymentDebitOrCash As String
    Private m_sMakeLivePaymentTerms As String

    <Browsable(False)> _
    Public WriteOnly Property CancelAboutToChangeAction() As Boolean
        Set(ByVal Value As Boolean)
            m_bCancelAboutToChangeAction = Value
        End Set
    End Property

    <Browsable(False)> _
    Public ReadOnly Property AppliesTo() As String
        Get
            Return m_sAppliesTo
        End Get
    End Property

    <Browsable(False)> _
    Public WriteOnly Property InsuranceFileCnt() As Integer
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property

    <Browsable(False)> _
    Public WriteOnly Property RiskCnt() As Integer
        Set(ByVal Value As Integer)
            m_lRiskCnt = Value
        End Set
    End Property

    <Browsable(False)> _
    Public WriteOnly Property ReadOnly_Renamed() As Boolean
        Set(ByVal Value As Boolean)
            m_bReadOnly = Value
        End Set
    End Property

    <Browsable(False)> _
    Public ReadOnly Property TotalTax() As Decimal
        Get
            Dim lReturn As gPMConstants.PMEReturnCode = GetTotalTax()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return 0
            Else
                Return m_crTotalTax
            End If
        End Get
    End Property

    <Browsable(False)> _
    Public ReadOnly Property TotalFees() As Decimal
        Get
            Dim lReturn As gPMConstants.PMEReturnCode = GetTotalFees()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return 0
            Else
                Return m_crTotalFees
            End If
        End Get
    End Property

    Private Sub cmdAddFees_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddFees.Click
        ReAddFees()
        SelectLastLVItem()
    End Sub

    Private Sub SelectLastLVItem()
        If lvwFees.Items.Count > 0 Then
            lvwFees.Items(lvwFees.Items.Count - 1).Selected = True
            cmdDeleteFees.Enabled = True
            cmdEditFees.Enabled = True
        Else
            cmdDeleteFees.Enabled = False
            cmdEditFees.Enabled = False
        End If
    End Sub

    Private Sub cmdDeleteFees_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteFees.Click
        If lvwFees.Items.Count > 0 Then
            DeletePolicyFee()
            SelectLastLVItem()
        End If
    End Sub

    Private Sub cmdEditFees_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEditFees.Click
        EditItem()
        SelectLastLVItem()
    End Sub
    <DllImport("user32.dll")> _
      Private Shared Function GetScrollPos(ByVal hWnd As System.IntPtr, ByVal nBar As Integer) As Integer

    End Function
    <DllImport("user32.dll")> _
       Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As UInteger, ByVal wParam As Integer, ByVal lParam As Integer) As Boolean

    End Function
    <DllImport("user32.dll")> _
   Private Shared Function LockWindowUpdate(ByVal Handle As IntPtr) As Boolean

    End Function
    'Store the horizontal scroll value.
    Private Sub StoreHScrollValue()
        hScrollValue = GetScrollPos(lvwFees.Handle, SBS_HORZ)
    End Sub
    'Recover the old scroll position
    Private Sub RecoverHorizontalScroll()
        LockWindowUpdate(lvwFees.Handle)
        'Calculate the value the scroll needs to scroll back.
        Dim dx As Integer = hScrollValue - GetScrollPos(lvwFees.Handle, SBS_HORZ)
        'Send the scroll message.
        Dim b As Boolean = SendMessage(lvwFees.Handle, LVM_SCROLL, dx, 0)
        LockWindowUpdate(IntPtr.Zero)

    End Sub
    Private Sub lvwFees_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwFees.ColumnClick
        'For storing the position of the horizontal scroll bar.
        StoreHScrollValue()
        ListViewFunc.SortListView(lvwFees, eventArgs)
        'For recovering the position of horizontal scroll bar. 
        RecoverHorizontalScroll()

    End Sub
    Private Sub UserControl_InitProperties()
        m_BackColor = m_def_BackColor
        m_ForeColor = m_def_ForeColor
        m_BackStyle = m_def_BackStyle
        m_BorderStyle = m_def_BorderStyle
        m_ShowEdit = m_def_ShowEdit
        m_Enabled = m_def_Enabled
        m_Visible = m_def_Visible

        'Developer Guide No solution 2 
        m_Font = Me.Font
    End Sub
    Private Sub UserControl_ReadProperties(ByRef PropBag As Object)


        m_BackColor = CInt(PropBag.ReadProperty("BackColor", m_def_BackColor))


        m_ForeColor = CInt(PropBag.ReadProperty("ForeColor", m_def_ForeColor))


        m_BackStyle = CInt(PropBag.ReadProperty("BackStyle", m_def_BackStyle))


        m_BorderStyle = CInt(PropBag.ReadProperty("BorderStyle", m_def_BorderStyle))


        m_ShowEdit = CBool(PropBag.ReadProperty("ShowEdit", m_def_ShowEdit))


        m_Enabled = CBool(PropBag.ReadProperty("Enabled", m_def_Enabled))


        m_Visible = CBool(PropBag.ReadProperty("Visible", m_def_Visible))
        m_Font = PropBag.ReadProperty("Font", Me.Font)
    End Sub

    Private Sub uctPMUFees_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize


        Dim lNewWidth As Integer = CInt(VB6.PixelsToTwipsX(Me.Width))
        Dim lNewHeight As Integer = CInt(VB6.PixelsToTwipsY(Me.Height) - (VB6.PixelsToTwipsY(cmdEditFees.Height) + 200))

        If lNewWidth > 0 Then
            lvwFees.Width = VB6.TwipsToPixelsX(lNewWidth)
        End If

        If lNewHeight > 0 Then
            lvwFees.Height = VB6.TwipsToPixelsY(lNewHeight)
        End If

        cmdEditFees.Top = lvwFees.Height + VB6.TwipsToPixelsY(100)
        cmdAddFees.Top = lvwFees.Height + VB6.TwipsToPixelsY(100)
        cmdDeleteFees.Top = lvwFees.Height + VB6.TwipsToPixelsY(100)
        cmdEditFees.Visible = True
        cmdAddFees.Visible = True
        cmdDeleteFees.Visible = True

    End Sub
    Private Sub UserControl_WriteProperties(ByRef PropBag As Object)

        PropBag.WriteProperty("BackColor", m_BackColor, m_def_BackColor)

        PropBag.WriteProperty("ForeColor", m_ForeColor, m_def_ForeColor)

        PropBag.WriteProperty("BackStyle", m_BackStyle, m_def_BackStyle)

        PropBag.WriteProperty("BorderStyle", m_BorderStyle, m_def_BorderStyle)

        PropBag.WriteProperty("ShowEdit", m_ShowEdit, m_def_ShowEdit)

        PropBag.WriteProperty("Enabled", m_Enabled, m_def_Enabled)

        PropBag.WriteProperty("Visible", m_Visible, m_def_Visible)
        PropBag.WriteProperty("Font", m_Font, Me.Font)
    End Sub

    ' ***************************************************************** '
    ' Name: Load
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 25-04-2005 : AUS005
    ' ***************************************************************** '
    Public Function Load_Renamed() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "Load"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



        result = gPMConstants.PMEReturnCode.PMTrue


        lReturn = SetupControl()
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "Setup Failed", gPMConstants.PMELogLevel.PMLogError)
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

        ' set up control accordingly
        lReturn = CType(SetupControl(v_lMode:=kModeRecalculate), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "SetupControl Failed", gPMConstants.PMELogLevel.PMLogError)
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
    ' Name: Initialise
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 25-04-2005 : AUS005
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
        If m_oBusiness Is Nothing Then
            Dim temp_m_oBusiness As Object
            lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRPartyFee.UBusiness", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetInstance of bSIRPartyFee.Business Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
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

'        Return result
'        Resume
'        Return result
        End Try
        Return result
    End Function


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
    ' Name: PopulateFees
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 25-04-2005 : AUS005
    ' ***************************************************************** '
    Private Function PopulateFees() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulateFees"

        Dim lReturn, llBound, lUBound As Integer
        Dim oListItem As ListViewItem
        Dim sResolvedName As String = ""
        Dim lProductId As Integer
        Dim sProductDescription As String = ""
        Dim lTransactionTypeId As Integer
        Dim sTransactionTypeDescription As String = ""
        Dim lFeeAmountId, lPartyCnt As Integer
        Dim crFeePercentage, crFeeAmount As Decimal
        Dim dtEffectiveDate As Date
        Dim lTaxGroupId, lCurrencyId As Integer
        Dim sCurrencyDesc, sAppliesTo As String
        Dim lPerilGroupId As Integer
        Dim crRate As Decimal
        Dim bRateIsPercentageOfPremium As Boolean
        Dim crFeeValue, crTaxAmount, crTotalAmount, crPremium As Decimal
        Dim sCurrencyCode As String = ""
        Dim lRiskGroupId, lCompanyID, lFeeCurrencyId As Integer
        Dim sTaxGroupDesc As String = ""
        Dim lIncludeIns, lSpread As Integer
        Dim sIncludeIns, sSpread As String
        Try



        result = gPMConstants.PMEReturnCode.PMTrue
        ListViewFunc.ListViewBatchStart(lvwFees)

        lvwFees.Items.Clear()

        If Information.IsArray(m_vFeeDetails) Then

            llBound = m_vFeeDetails.GetLowerBound(1)
            lUBound = m_vFeeDetails.GetUpperBound(1)

            ' Assign the details to the interface.
            For lFee As Integer = llBound To lUBound

                ' fee name
                sResolvedName = ReplaceNullWithDefault(CStr(m_vFeeDetails(kFeeItemResolvedName, lFee)), "NEW FEE ADDED").Trim()

                ' currency code
                sCurrencyCode = CStr(m_vFeeDetails(kFeeItemCurrencyIsoCode, lFee)).Trim()

                ' applies to
                'Developer Guide No.248
                If gPMFunctions.ToSafeDouble(m_vFeeDetails(kFeeItemProductId, lFee)) <> 0 Then
                    lProductId = CInt(ReplaceNullWithDefault(CStr(m_vFeeDetails(kFeeItemProductId, lFee)), CStr(-1)))
                End If

                lRiskGroupId = CInt(ReplaceNullWithDefault(CStr(m_vFeeDetails(kFeeItemRiskGroupId, lFee)), CStr(-1)))
                lPerilGroupId = CInt(ReplaceNullWithDefault(CStr(m_vFeeDetails(kFeeItemPerilGroupId, lFee)), CStr(-1)))

                If lProductId = 0 Then
                    sAppliesTo = "(All)"
                End If

                If lProductId > 0 Then
                    sAppliesTo = CStr(m_vFeeDetails(kFeeItemProductDesc, lFee)).Trim()
                ElseIf lRiskGroupId > 0 Then
                    sAppliesTo = CStr(m_vFeeDetails(kFeeItemRiskGroupDesc, lFee)).Trim()
                ElseIf lPerilGroupId > 0 Then
                    sAppliesTo = CStr(m_vFeeDetails(kFeeItemPerilGroupDesc, lFee)).Trim()
                End If

                m_sAppliesTo = sAppliesTo

                ' premium
                crPremium = CDec(ReplaceNullWithDefault(CStr(m_vFeeDetails(kFeeItemFeePremium, lFee)), CStr(0)))

                ' rate
                crFeePercentage = CDec(m_vFeeDetails(kFeeItemFeePercentage, lFee))
                crFeeValue = CDec(m_vFeeDetails(kFeeItemFeeAmount, lFee))

                If crFeePercentage <> 0 Then
                    crRate = crFeePercentage
                    bRateIsPercentageOfPremium = True
                Else
                    crRate = crFeeValue
                    bRateIsPercentageOfPremium = False
                End If

                ' actual fee amount
                crFeeAmount = CDec(ReplaceNullWithDefault(CStr(m_vFeeDetails(kFeeItemCurrencyAmount, lFee)), CStr(0)))

                ' tax amount
                crTaxAmount = CDec(ReplaceNullWithDefault(CStr(m_vFeeDetails(kFeeItemTaxAmount, lFee)), CStr(0)))

                ' total amount
                ' tax amount + fee amount
                crTotalAmount = crTaxAmount + crFeeAmount

                ' tax group
                sTaxGroupDesc = CStr(m_vFeeDetails(kFeeItemTaxGroupDescription, lFee))

                ' other required values
                lCompanyID = CInt(m_vFeeDetails(kFeeItemCompanyId, lFee))
                lFeeCurrencyId = CInt(m_vFeeDetails(kFeeItemCurrencyId, lFee))

                ' **************************************
                ' add item to fee list view
                ' **************************************
                'Two columns has been added (BPIS-Partial instalment)
                lIncludeIns = gPMFunctions.ToSafeLong(CStr(m_vFeeDetails(kFeeItemIncludeIns, lFee)), 0)
                lSpread = gPMFunctions.ToSafeLong(CStr(m_vFeeDetails(kFeeItemSpread, lFee)), 0)

                If lIncludeIns = 1 Then
                    sIncludeIns = "Yes"
                Else
                    sIncludeIns = "No"
                End If

                If lSpread = 1 Then
                    sSpread = "Yes"
                Else
                    sSpread = "No"
                End If

                ' Fee Name
                oListItem = lvwFees.Items.Add(sResolvedName)

                ' Currency Code
                ListViewHelper.GetListViewSubItem(oListItem, kFeeColHCurrency - 1).Text = sCurrencyCode

                ' Applied To
                ListViewHelper.GetListViewSubItem(oListItem, kFeeColHAppliedTo - 1).Text = sAppliesTo

                ' Premium
                ListViewHelper.GetListViewSubItem(oListItem, kFeeColHPremium - 1).Text = StringsHelper.Format(crPremium, "0.00")

                ' Rate
                If bRateIsPercentageOfPremium Then
                    ListViewHelper.GetListViewSubItem(oListItem, kFeeColHRate - 1).Text = StringsHelper.Format(crRate, "0.00") & "%"
                Else
                    ListViewHelper.GetListViewSubItem(oListItem, kFeeColHRate - 1).Text = StringsHelper.Format(crRate, "0.00")
                End If

                ' Fee Amount
                ListViewHelper.GetListViewSubItem(oListItem, kFeeColHFeeAmount - 1).Text = StringsHelper.Format(crFeeAmount, "0.00") & gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatString, CStr(m_vFeeDetails(kFeeItemTransCurrencyISOCode, lFee)))

                ' Tax Amount
                ListViewHelper.GetListViewSubItem(oListItem, kFeeColHTaxAmount - 1).Text = StringsHelper.Format(crTaxAmount, "0.00")

                ' Total Amount
                ListViewHelper.GetListViewSubItem(oListItem, kFeeColHTotalAmount - 1).Text = StringsHelper.Format(crTotalAmount, "0.00")

                ' Tax Group
                ListViewHelper.GetListViewSubItem(oListItem, kFeeColHTaxGroup - 1).Text = sTaxGroupDesc
                ' Fee included in instalment or spread across instalment
                ListViewHelper.GetListViewSubItem(oListItem, kFeeColHIncludeIns - 1).Text = sIncludeIns
                ListViewHelper.GetListViewSubItem(oListItem, kFeeColHSpread - 1).Text = sSpread

                ' Store the Fee_cnt
                oListItem.Tag = CStr(lFee)

            Next lFee

        End If
        ListViewFunc.ListViewBatchEnd()

        lvwFees.Refresh()

        lvwFees.Visible = True



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
    '           Created : MEvans : 25-04-2005 : AUS005
    ' ***************************************************************** '
    Private Function SetUpListView() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetUpListView"

        Dim lReturn As Integer

        Try



        result = gPMConstants.PMEReturnCode.PMTrue

        lvwFees.FullRowSelect = True

        lvwFees.Columns.Clear()

        'set header properties
        With lvwFees

            'clear the column headers
            .Columns.Clear()

            .Columns.Insert(kFeeColHFeeType - 1, "kFeeColHFeeType", "Fee Name", CInt(VB6.TwipsToPixelsX(1600)))
            .Columns.Insert(kFeeColHCurrency - 1, "kFeeColHCurrency", "Currency Code", CInt(VB6.TwipsToPixelsX(1600)))
            .Columns.Insert(kFeeColHAppliedTo - 1, "kFeeColHAppliedTo", "Applied To", CInt(VB6.TwipsToPixelsX(1400)))
            .Columns.Insert(kFeeColHPremium - 1, "kFeeColHPremium", "Premium", CInt(VB6.TwipsToPixelsX(1400)), HorizontalAlignment.Right, -1)
            .Columns.Insert(kFeeColHRate - 1, "kFeeColHRate", "Rate", CInt(VB6.TwipsToPixelsX(1400)), HorizontalAlignment.Right, -1)
            .Columns.Insert(kFeeColHFeeAmount - 1, "kFeeColHFeeAmount", "Fee Amount", CInt(VB6.TwipsToPixelsX(1400)), HorizontalAlignment.Right, -1)
            .Columns.Insert(kFeeColHTaxAmount - 1, "kFeeColHTaxAmount", "Tax Amount", CInt(VB6.TwipsToPixelsX(1400)), HorizontalAlignment.Right, -1)
            .Columns.Insert(kFeeColHTotalAmount - 1, "kFeeColHTotalAmount", "Total Amount", CInt(VB6.TwipsToPixelsX(1400)), HorizontalAlignment.Right, -1)
            .Columns.Insert(kFeeColHTaxGroup - 1, "kFeeColHTaxGroup", "Tax Group", CInt(VB6.TwipsToPixelsX(1400)))
            .Columns.Insert(kFeeColHIncludeIns - 1, "kFeeColHIncludeIns", "Include Fee in Instalment", CInt(VB6.TwipsToPixelsX(1400)))
            .Columns.Insert(kFeeColHSpread - 1, "kFeeColHSpread", "Spread the Fee across instalment", CInt(VB6.TwipsToPixelsX(1400)))
            .FullRowSelect = True

        End With



        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function
    ''' <summary>
    ''' SetProcessModes
    ''' </summary>
    ''' <param name="vTask"></param>
    ''' <param name="vNavigate"></param>
    ''' <param name="vProcessMode"></param>
    ''' <param name="vTransactionType"></param>
    ''' <param name="vEffectiveDate"></param>
    ''' <param name="oMakeLivePaymentTerms"></param>
    ''' <param name="oMakeLivePaymentDebitOrCash"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, _
                                    Optional oMakeLivePaymentTerms As Object = Nothing, Optional oMakeLivePaymentDebitOrCash As Object = Nothing) As Integer


        Dim nResult As Integer
        Const kMethodName As String = "SetProcessModes"
        Try
            nResult = PMEReturnCode.PMTrue

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

                Select Case m_sTransactionType
                    Case "NB"
                        m_lTransTypeId = 4
                    Case "MTC"
                        m_lTransTypeId = 7
                    Case "MTA"
                        m_lTransTypeId = 9
                    Case "MTR"
                        m_lTransTypeId = 20
                    Case Else '"REN"
                        m_lTransTypeId = 10
                End Select

            End If

            If Not Information.IsNothing(vEffectiveDate) Then
                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            If (Information.IsNothing(oMakeLivePaymentTerms) = False) Then
                m_sMakeLivePaymentTerms = CStr(oMakeLivePaymentTerms)
            End If

            If (Information.IsNothing(oMakeLivePaymentDebitOrCash) = False) Then
                m_sMakeLivePaymentDebitOrCash = CStr(oMakeLivePaymentDebitOrCash)
                If Len(Trim(m_sMakeLivePaymentDebitOrCash)) = 0 Then
                    m_sMakeLivePaymentDebitOrCash = "0"
                End If
            End If
            Return nResult
        Catch ex As Exception
            nResult = PMEReturnCode.PMError
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)
            Return nResult
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetupBusiness
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 25-04-2005 : AUS005
    ' ***************************************************************** '
    Private Function SetupBusiness() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupBusiness"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



        result = gPMConstants.PMEReturnCode.PMTrue

        If m_oBusiness Is Nothing Then
            Dim temp_m_oBusiness As Object
            lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRPartyFee.UBusiness", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetInstance of bSIRPartyFee.Business Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
        End If

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
    '           Created : MEvans : 25-04-2005 : AUS005
    ' ***************************************************************** '
    Private Function SetupButtons() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupButtons"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lItemId As Integer


        Try



        result = gPMConstants.PMEReturnCode.PMTrue

        If m_bReadOnly Then
            cmdEditFees.Text = "View"
        End If

        lReturn = CType(GetSelectedItem(lItemId, lvwFees), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "GetSelectedItem Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        If lItemId <> -1 Then
            cmdEditFees.Enabled = True
            cmdDeleteFees.Enabled = (Not m_bReadOnly)
        Else
            cmdEditFees.Enabled = False
            cmdDeleteFees.Enabled = False
        End If

        cmdAddFees.Enabled = (Not m_bReadOnly)



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

        Dim lReturn, lFeeAmountId As Integer
        Dim bItemSelected As Boolean

        Try



        result = gPMConstants.PMEReturnCode.PMTrue

        r_lItemId = -1

        ' determine if there are any selected items
        For lItem As Integer = 1 To oList.Items.Count
            If oList.Items.Item(lItem - 1).Selected Then

                r_lItemId = Convert.ToString(oList.Items.Item(lItem - 1).Tag)
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

    ''' <summary>
    ''' EditItem
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function EditItem() As Integer

        Dim nResult As Integer
        Const kMethodName As String = "EditItem"

        Dim nItemId As Integer
        Dim ofrmFeeDetail As frmFeeDetail
        Try

            nResult = PMEReturnCode.PMTrue

            RaiseEvent AboutToChange(Me, Nothing)
            If m_bCancelAboutToChangeAction Then
                m_bCancelAboutToChangeAction = False
                Return nResult
            End If

            nResult = CType(GetSelectedItem(nItemId, lvwFees), PMEReturnCode)
            If nResult <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "GetSelectedItem Failed", PMELogLevel.PMLogError)
            End If

            ofrmFeeDetail = New frmFeeDetail()

            ofrmFeeDetail.ReadOnly_Renamed = m_bReadOnly
            ofrmFeeDetail.SelectedItem = nItemId
            ofrmFeeDetail.FeeDetails = VB6.CopyArray(m_vFeeDetails)
            ofrmFeeDetail.InsuranceFileCnt = m_lInsuranceFileCnt
            ofrmFeeDetail.RiskCnt = m_lRiskCnt
            ofrmFeeDetail.ShowDialog()

            If ofrmFeeDetail.Status = PMEReturnCode.PMOK Then

                If ofrmFeeDetail.DataHasChanged Then

                    m_vFeeDetails = ofrmFeeDetail.FeeDetails

                    nResult = CType(UpdatePolicyFee(nItemId), PMEReturnCode)
                    If nResult <> PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "UpdatePolicyFee Failed", PMELogLevel.PMLogError)
                    End If

                    nResult = CType(GetFeeDetails(), PMEReturnCode)
                    If nResult <> PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "GetFeeDetails Failed", PMELogLevel.PMLogError)
                    End If

                    nResult = PopulateFees()
                    If nResult <> PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "PopulateTaxes Failed", PMELogLevel.PMLogError)
                    End If

                    RaiseEvent Change(Me, Nothing)

                End If
            End If

            m_vFeeDetails = ofrmFeeDetail.FeeDetails

            Return nResult

        Catch ex As Exception
            nResult = PMEReturnCode.PMError

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)
            Return nResult
        Finally
            ofrmFeeDetail = Nothing
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetTotalTax
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function GetTotalTax() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetTotalTax"

        Dim lReturn As Integer
        Dim crTotalTax As Decimal

        Try



        result = gPMConstants.PMEReturnCode.PMTrue

        ' determine if there are any selected items
        For lItem As Integer = 1 To lvwFees.Items.Count
            crTotalTax += CDbl(ListViewHelper.GetListViewSubItem(lvwFees.Items.Item(lItem - 1), kFeeColHTaxAmount - 1).Text)
        Next

        m_crTotalTax = crTotalTax



        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetTotalFees
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function GetTotalFees() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetTotalFees"

        Dim lReturn As Integer
        Dim crTotalFees As Decimal

        Try



        result = gPMConstants.PMEReturnCode.PMTrue

        ' determine if there are any selected items
        If Information.IsArray(m_vFeeDetails) Then
            For lItem As Integer = 0 To m_vFeeDetails.GetUpperBound(1)
                crTotalFees += CDbl(IIf(m_vFeeDetails(kFeeItemCurrencyAmount, lItem) = "", 0, m_vFeeDetails(kFeeItemCurrencyAmount, lItem)))
            Next
        End If

        m_crTotalFees = crTotalFees



        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetProductType
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function GetProductType() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetProductType"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



        result = gPMConstants.PMEReturnCode.PMTrue


        lReturn = m_oBusiness.GetProductId(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, r_lProductId:=m_lProductId)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "GetProductType Failed", gPMConstants.PMELogLevel.PMLogError)
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
    ''' GetFeeDetails
    ''' </summary>
    ''' <param name="v_lMode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetFeeDetails(Optional ByVal v_lMode As Integer = 0) As Integer

        Dim nResult As Integer
        Const kMethodName As String = "GetFeeDetails"

        Dim oPolicyDetails As Object = Nothing

        Try

            nResult = PMEReturnCode.PMTrue

            ' if the process is being called at risk level
            If m_lRiskCnt <> 0 Then

                If v_lMode = kModeRecalculate Then
                    ' get fee ready for recalculation
                    ' recalculate fees at risk level

                    nResult = m_oBusiness.RecalculateRiskFees(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_lTransactionTypeId:=m_lTransTypeId, v_lRiskCnt:=m_lRiskCnt, v_sTransactionType:=m_sTransactionType)
                    If nResult <> PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "RecalculateRiskFees Failed", PMELogLevel.PMLogError)
                    End If

                End If

                ' get saved risk fee details from policy_fee_u

                nResult = m_oBusiness.GetRiskFees(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_lRiskCnt:=m_lRiskCnt, r_vResults:=m_vFeeDetails)
                If nResult <> PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "GetRiskFees Failed", PMELogLevel.PMLogError)
                End If

            Else
                ' if the process is being shown at policy level

                ' get fees ready for recalculation
                If v_lMode = kModeRecalculate Then
                    Dim sPaymentTerm As String = ""
                    ' recalculate all fees at policy level
                    If m_sMakeLivePaymentTerms.Trim.ToLower = "direct debit" Then
                        sPaymentTerm = "INST"
                    Else
                        sPaymentTerm = m_sMakeLivePaymentTerms
                    End If

                    nResult = m_oBusiness.RecalculatePolicyFees(v_lTransactionTypeId:=m_lTransTypeId, v_lProductId:=m_lProductId, v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_sMakeLivePaymentTerms:=sPaymentTerm, v_sMakeLivePaymentDebitOrCash:=m_sMakeLivePaymentDebitOrCash, v_sTransactionType:=m_sTransactionType)

                    If nResult <> PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "RecalculateRiskFees Failed", PMELogLevel.PMLogError)
                    End If
                End If

                ' get saved polcy fee details from policy_fee_u

                nResult = m_oBusiness.GetPolicyFees(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, r_vResults:=m_vFeeDetails)

                If nResult <> PMEReturnCode.PMTrue AndAlso nResult <> PMEReturnCode.PMNotFound Then
                    RaiseError(kMethodName, "GetPolicyFees Failed", PMELogLevel.PMLogError)
                End If

            End If

            Return nResult
        Catch ex As Exception
            nResult = PMEReturnCode.PMError
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)
            Return nResult
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetLookups
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (GetLookups) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetLookups() As Integer
    '
    'Dim result As Integer = 0
    'Const kMethodName As String = "GetLookups"
    '
    'Dim lReturn As gPMConstants.PMEReturnCode
    '
    'On Error GoTo Catch_Renamed
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'lReturn = CType(GetLookupValues(kTableCurrency, m_vCurrency), gPMConstants.PMEReturnCode)
    'If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'gPMFunctions.RaiseError(kMethodName, "GetLookupValues Failed", gPMConstants.PMELogLevel.PMLogError)
    'End If
    '
    'lReturn = CType(GetLookupValues(kTableTaxGroup, m_vTaxGroup), gPMConstants.PMEReturnCode)
    'If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'gPMFunctions.RaiseError(kMethodName, "GetLookupValues Failed", gPMConstants.PMELogLevel.PMLogError)
    'End If
    '
    'GoTo Finally_Renamed
    '
    'Catch_Renamed: '
    '
    ' DO Not Call any functions before here or the error will be lost
    'iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)
    '
    ' If you want to rollback a transaction or something, do it here
    '
    'Finally_Renamed: '
    '
    'Return result
    'Resume 
    'Return result
    'End Function

    ' ***************************************************************** '
    ' Name: GetLookupValues
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 19-04-2005 : AUS005
    ' ***************************************************************** '
    Private Function GetLookupValues(ByVal v_sTableName As String, ByRef r_vArray(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetLookupValues"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            lReturn = m_oBusiness.GetTableLookupValues(v_sTableName:=v_sTableName, r_vResults:=r_vArray)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetLookupValues Failed for table :" & v_sTableName)
            End If

            If Not Information.IsArray(r_vArray) Then
                gPMFunctions.RaiseError(kMethodName, "GetLookupValues Failed to return any values for table :" & v_sTableName)
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
    ' Name: ReplaceNullWithDefault
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 24-06-2004 : CQ4740
    ' ***************************************************************** '
    Private Function ReplaceNullWithDefault(ByRef v_vValue As String, ByVal v_vDefault As String) As String

        Dim result As String = String.Empty
        Const sFunctionName As String = "ReplaceNullWithDefault"

        Try

            result = CStr(gPMConstants.PMEReturnCode.PMTrue)


            If v_vValue = "" Or v_vValue Is DBNull.Value.ToString() Or Convert.IsDBNull(v_vValue) Or IsNothing(v_vValue) Or StringsHelper.ToDoubleSafe(v_vValue) = 0 Then

                v_vValue = v_vDefault

            End If


            Return v_vValue

        Catch excep As System.Exception



            result = CStr(gPMConstants.PMEReturnCode.PMError)

            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetPolicyDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function GetPolicyDetails() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPolicyDetails"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vPolicyDetails As Object

        Try



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get Policy Details

        lReturn = m_oBusiness.GetEffectiveDate(vInsID:=m_lInsuranceFileCnt, vResultArray:=vPolicyDetails)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "GetEffectiveDate Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        If Not Information.IsArray(vPolicyDetails) Then
            gPMFunctions.RaiseError(kMethodName, "GEtEffectiveDate Returned No Data", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' get effective date returns
        '  kPolicyCoverStartDate = 0
        '  kPolicyThisPremium = 1
        '  kPolicyExpiryDate = 2
        '  kPolicyAnnualPremium = 3


        m_crPremium = CDec(ReplaceNullWithDefault(CStr(vPolicyDetails(kPolicyThisPremium, 0)), CStr(0)))

        m_crAnnualPremium = CDec(ReplaceNullWithDefault(CStr(vPolicyDetails(kPolicyAnnualPremium, 0)), CStr(0)))

        m_dtPolicyCoverStartDate = (CDate(vPolicyDetails(kPolicyCoverStartDate, 0)))
        m_dtPolicyCoverStartDate = CDate(m_dtPolicyCoverStartDate.ToString("yyyy-MM-dd"))

        m_dtPolicyExpiryDate = (CDate(vPolicyDetails(kPolicyExpiryDate, 0)))



        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: CalculateTaxAmount
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (CalculateTaxAmount) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CalculateTaxAmount(ByVal v_crFeeAmount As Decimal, ByVal v_lTaxGroupId As Integer, ByVal v_lCompanyId As Integer, ByVal v_lFeeCurrencyId As Integer, ByRef r_crTaxAmount As Decimal) As Integer
    '
    'Dim result As Integer = 0
    'Const kMethodName As String = "CalculateTaxAmount"
    '
    'Dim lReturn As gPMConstants.PMEReturnCode
    '
    'On Error GoTo Catch_Renamed
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '

    'lReturn = m_oBusiness.CalculateFeeTaxAmount(v_lTaxGroupId:=v_lTaxGroupId, v_lCompany_id:=v_lCompanyId, v_lFeeCurrencyId:=v_lFeeCurrencyId, v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_crFeeAmount:=v_crFeeAmount, r_crTaxAmount:=r_crTaxAmount)
    '
    'If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'gPMFunctions.RaiseError(kMethodName, "CalcualteFeeTaxAmount Failed", gPMConstants.PMELogLevel.PMLogError)
    'End If
    '
    '
    'GoTo Finally_Renamed
    '
    'Catch_Renamed: '
    '
    ' Do Not Call any functions before here or the error will be lost
    'iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)
    '
    ' If you want to rollback a transaction or something, do it here
    '
    'Finally_Renamed: '
    '
    'Return result
    'Resume 
    'Return result
    'End Function


    ' ***************************************************************** '
    ' Name: SetupControl
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function SetupControl(Optional ByVal v_lMode As Integer = 0) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupControl"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



        result = gPMConstants.PMEReturnCode.PMTrue

        If m_lInsuranceFileCnt = 0 And m_lRiskCnt = 0 Then
            gPMFunctions.RaiseError(kMethodName, "Invalid Properties Set", gPMConstants.PMELogLevel.PMLogError)
        End If

        lReturn = SetupBusiness()
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "SetupControl", gPMConstants.PMELogLevel.PMLogError)
        End If

        lReturn = SetInterfaceDefaults()
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "SetInterfaceDefaults Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        lReturn = GetPolicyDetails()
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "GetPolicyDetails Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        lReturn = GetProductType()
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "GetProductType Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        lReturn = CType(GetFeeDetails(v_lMode), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "GetFeeDetails Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        lReturn = PopulateFees()
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "PopulateTaxes Failed", gPMConstants.PMELogLevel.PMLogError)
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
    ' Name: DeletePolicyFee
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function DeletePolicyFee() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DeletePolicyFee"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lPolicyFeeUId, lItemId As Integer

        Try



        result = gPMConstants.PMEReturnCode.PMTrue

        RaiseEvent AboutToChange(Me, Nothing)
        If m_bCancelAboutToChangeAction Then
            m_bCancelAboutToChangeAction = False
            Return result
        End If

        ' get the selected fee index
        lReturn = CType(GetSelectedItem(lItemId, lvwFees), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "GetSelectedItem Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' get the policy fee u id
        lPolicyFeeUId = CInt(m_vFeeDetails(kFeeitemPolicyFeeUId, lItemId))

        ' delete record from the db..

        lReturn = m_oBusiness.DeletePolicyFee(v_lPolicyFeeUId:=lPolicyFeeUId)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "DeletePolicyFee Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        lReturn = CType(GetFeeDetails(), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "GetFeeDetails Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        lReturn = PopulateFees()
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "PopulateTaxes Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        RaiseEvent Change(Me, Nothing)



        Catch ex As Exception

        ' Do Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ReAddFees
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function ReAddFees() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ReAddFees"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



        result = gPMConstants.PMEReturnCode.PMTrue

        RaiseEvent AboutToChange(Me, Nothing)
        If m_bCancelAboutToChangeAction Then
            m_bCancelAboutToChangeAction = False
            Return result
        End If

        lReturn = Recalculate()
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "Recalculate Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        RaiseEvent Change(Me, Nothing)



        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: UpdatePolicyFee
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function UpdatePolicyFee(ByVal v_lSelectedItem As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdatePolicyFee"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim crFeePercentage, crFeeAmount As Decimal
        Dim lPolicyFeeUId As Integer

        Try



        result = gPMConstants.PMEReturnCode.PMTrue

        ' get selected items details
        lPolicyFeeUId = CInt(m_vFeeDetails(kFeeitemPolicyFeeUId, v_lSelectedItem))
        crFeeAmount = CDec(m_vFeeDetails(kFeeItemFeeAmount, v_lSelectedItem))
        crFeePercentage = CDec(m_vFeeDetails(kFeeItemFeePercentage, v_lSelectedItem))

        ' update policy fee item details..
        ' NB: this will also recalculate the fee amounts and the fee tax amounts

        lReturn = m_oBusiness.UpdatePolicyFee(v_lPolicyFeeUId:=lPolicyFeeUId, v_crFeePercentage:=crFeePercentage, v_crFeeAmount:=crFeeAmount)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "UpdatePolicyFee Failed", gPMConstants.PMELogLevel.PMLogError)
        End If



        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function
    Private Sub lvwFees_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwFees.Click
        SetupButtons()
    End Sub
End Class
