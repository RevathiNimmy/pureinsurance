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
<System.Runtime.InteropServices.ProgId("uctPMURITax_NET.uctPMURITax")> _
Partial Public Class uctPMURITax
    Inherits System.Windows.Forms.UserControl

    ' ***************************************************************** '
    ' Object Name: uctPMURITax
    '
    ' Date: 25-04-2005
    '
    ' Description: Main User Control
    '
    ' Edit History:
    '   NB: All Initial functionality has been ripped from iPMURITax
    ' ***************************************************************** '

    Private Const ACClass As String = "uctPMURITax"

    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iUserId As Integer
    Private m_lCurrencyID As Integer
    Private m_lInsurance_file_cnt As Integer

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
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_lReturn As Integer

    Private m_oBusiness As bSIRRITax.Business
    Private m_oFormFields As iPMFormControl.FormFields
    Private m_ListViewArray As Object
    Private m_bStatus As Boolean

    Public Event AboutToChange(ByVal Sender As Object, ByVal e As EventArgs)
    Public Event Change(ByVal Sender As Object, ByVal e As EventArgs)

    Private m_lInsuranceFileCnt As Integer
    Private m_lRiskCnt As Integer
    Private m_bReadOnly As Boolean
    Private m_crTotalTax As Decimal
    Private m_vRITax(,) As Object
    Private m_sDescription As String = ""
    Private m_bCancelAboutToChangeAction As Boolean
    Private m_bLoadWithoutCalculation As Boolean
    Private m_sApplyMTATaxRatesonRen As String = ""

    <Browsable(False)>
    Public WriteOnly Property LoadWithoutCalculation() As Boolean
        Set(ByVal Value As Boolean)
            m_bLoadWithoutCalculation = Value
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property CancelAboutToChangeAction() As Boolean
        Set(ByVal Value As Boolean)
            m_bCancelAboutToChangeAction = Value
        End Set
    End Property

    <Browsable(False)>
    Public ReadOnly Property Description() As String
        Get
            Return m_sDescription
        End Get
    End Property

    <Browsable(False)>
    Public WriteOnly Property InsuranceFileCnt() As Integer
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property RiskCnt() As Integer
        Set(ByVal Value As Integer)
            m_lRiskCnt = Value
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property ReadOnly_Renamed() As Boolean
        Set(ByVal Value As Boolean)
            m_bReadOnly = Value
        End Set
    End Property

    <Browsable(False)>
    Public Property CurrencyId() As Integer
        Get
            Return m_lCurrencyID
        End Get
        Set(ByVal Value As Integer)
            m_lCurrencyID = Value
        End Set
    End Property
    <Browsable(False)>
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

    <Browsable(False)>
    Public Property ApplyMTATaxRatesonRen() As String
        Get
            Return m_sApplyMTATaxRatesonRen
        End Get
        Set(ByVal Value As String)
            m_sApplyMTATaxRatesonRen = Value
        End Set
    End Property

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click
        If (lvwRITax.SelectedItems.Count > 0) Then
            EditItem()
        End If
    End Sub

    Private Sub lvwRITax_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwRITax.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwRITax.Columns(eventArgs.Column)

        With lvwRITax
            ' If current sort column header is
            ' pressed.
            If ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwRITax) Then
                ' Set sort order opposite of
                ' current direction.
                ListViewHelper.SetSortOrderProperty(lvwRITax, (ListViewHelper.GetSortOrderProperty(lvwRITax) + 1) Mod 2)
            Else
                ' Sort by this column (ascending).
                ListViewHelper.SetSortedProperty(lvwRITax, False)

                ' Turn off sorting so that the list
                ' is not sorted twice
                ListViewHelper.SetSortOrderProperty(lvwRITax, SortOrder.Ascending)
                ListViewHelper.SetSortKeyProperty(lvwRITax, ColumnHeader.Index + 1 - 1)
                ListViewHelper.SetSortedProperty(lvwRITax, True)
            End If
        End With

    End Sub

    Private Sub lvwRITax_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwRITax.DoubleClick
        ' If the edit button is enabled call it
        If cmdEdit.Enabled Then
            cmdEdit_Click(cmdEdit, New EventArgs())
        End If
    End Sub

    'Private Sub lvwRITax_ItemClick(ByVal Item As ListViewItem)
    '	SetupButtons()
    'End Sub


    Private Sub UserControl_InitProperties()
        m_BackColor = m_def_BackColor
        m_ForeColor = m_def_ForeColor
        m_BackStyle = m_def_BackStyle
        m_BorderStyle = m_def_BorderStyle
        m_ShowEdit = m_def_ShowEdit
        m_Enabled = m_def_Enabled
        m_Visible = m_def_Visible

        'Developer Guide No. 2 (no solution)

        m_Font = Me.Font
    End Sub




    'Developer Guide No. 1 (no solution)
    Private Sub UserControl_ReadProperties(ByRef PropBag As Object)


        m_BackColor = CInt(PropBag.ReadProperty("BackColor", m_def_BackColor))


        m_ForeColor = CInt(PropBag.ReadProperty("ForeColor", m_def_ForeColor))


        m_BackStyle = CInt(PropBag.ReadProperty("BackStyle", m_def_BackStyle))


        m_BorderStyle = CInt(PropBag.ReadProperty("BorderStyle", m_def_BorderStyle))


        m_ShowEdit = CBool(PropBag.ReadProperty("ShowEdit", m_def_ShowEdit))


        m_Enabled = CBool(PropBag.ReadProperty("Enabled", m_def_Enabled))


        m_Visible = CBool(PropBag.ReadProperty("Visible", m_def_Visible))



        'Developer Guide No. 2 (no solution)
        m_Font = PropBag.ReadProperty("Font", Me.Font)
    End Sub

    Private Sub uctPMURITax_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize


        Dim lNewWidth As Integer = CInt(VB6.PixelsToTwipsX(Me.Width))
        Dim lNewHeight As Integer = CInt(VB6.PixelsToTwipsY(Me.Height) - (VB6.PixelsToTwipsY(cmdEdit.Height) + 200))

        If lNewWidth > 0 Then
            lvwRITax.Width = VB6.TwipsToPixelsX(lNewWidth)
        End If

        If lNewHeight > 0 Then
            lvwRITax.Height = VB6.TwipsToPixelsY(lNewHeight)
        End If

        cmdEdit.Top = lvwRITax.Height + VB6.TwipsToPixelsY(100)
        cmdEdit.Visible = True

    End Sub




    'Developer Guide No. 1 (no solution)

    Private Sub UserControl_WriteProperties(ByRef PropBag As Object)

        PropBag.WriteProperty("BackColor", m_BackColor, m_def_BackColor)

        PropBag.WriteProperty("ForeColor", m_ForeColor, m_def_ForeColor)

        PropBag.WriteProperty("BackStyle", m_BackStyle, m_def_BackStyle)

        PropBag.WriteProperty("BorderStyle", m_BorderStyle, m_def_BorderStyle)

        PropBag.WriteProperty("ShowEdit", m_ShowEdit, m_def_ShowEdit)

        PropBag.WriteProperty("Enabled", m_Enabled, m_def_Enabled)

        PropBag.WriteProperty("Visible", m_Visible, m_def_Visible)



        'Developer Guide No 2(no solution)
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
    Public Function Load_Renamed(Optional ByVal v_iTask As Integer = gPMConstants.PMEComponentAction.PMView) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "Load"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



        result = gPMConstants.PMEReturnCode.PMTrue

        m_iTask = v_iTask

        If m_lInsuranceFileCnt = 0 And m_lRiskCnt = 0 Then
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

        lReturn = GetRITaxes()
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "GetRITaxes Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        lReturn = PopulateTaxes()
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "PopulateTaxes Failed", gPMConstants.PMELogLevel.PMLogError)
        End If


        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(ACClass, kMethodName, result, excep:=ex)

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

        ' NB : mode passed in via set process modes is ignored.
        ' Recalculate just calls "Load" in edit mode
        ' (this will regenerate the tax entries for either risk or policy)
        lReturn = CType(Load_Renamed(gPMConstants.PMEComponentAction.PMEdit), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "Load Failed", gPMConstants.PMELogLevel.PMLogError)
        End If


        Catch ex As Exception

        ' Do Not Call any functions before here or the error will be lost
        iPMFunc.LogError(ACClass, kMethodName, result, excep:=ex)

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
        lReturn = g_oObjectManager.Initialise(ACApp)
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
        lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRRITax.Business", gPMConstants.PMGetViaClientManager)
        m_oBusiness = temp_m_oBusiness
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "GetInstance of bSIRRITax.Business Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' hold Initialised status
        bIsInitialised = True


        Catch ex As Exception

        ' Do Not Call any functions before here or the error will be lost
        iPMFunc.LogError(ACClass, kMethodName, result, excep:=ex)

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
    ' Name: GetRITaxes
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 25-04-2005 : AUS005
    ' ***************************************************************** '
    Private Function GetRITaxes() As Integer

        Dim nResult As Integer = 0
        Const kMethodName As String = "GetRITaxes"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim bApplyTaxes, bTaxesSwitchedOff As Boolean

        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue

        lReturn = m_oBusiness.ApplyTaxes(v_lInsFileCnt:=m_lInsuranceFileCnt, v_lRiskCnt:=m_lRiskCnt, r_bApplyTaxes:=bApplyTaxes, r_bTaxesSwitchedOff:=bTaxesSwitchedOff)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "ApplyTax Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' If we are not applying taxes delete any that may already exist
        If bTaxesSwitchedOff Then

            m_lReturn = m_oBusiness.DeleteAllTaxes(m_lInsuranceFileCnt)
                Return nResult
        ElseIf Not bApplyTaxes Then

            m_lReturn = m_oBusiness.DeleteTaxes(m_lInsuranceFileCnt, m_lRiskCnt)
                Return nResult
        End If
        m_oBusiness.ApplyMTATaxRatesonRen = m_sApplyMTATaxRatesonRen
        If m_lRiskCnt <> 0 Then

            m_oBusiness.RiskCnt = m_lRiskCnt
            
            lReturn = m_oBusiness.GetRiskTax(r_vRiskTax:=m_vRITax, r_sDescription:=m_sDescription, iTask:=m_iTask)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetRiskTax Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        ElseIf m_lInsuranceFileCnt <> 0 Then

            m_oBusiness.InsuranceFileCnt = m_lInsuranceFileCnt

            ' generally taxes should always be recalculated
            If Not m_bLoadWithoutCalculation Then

                lReturn = m_oBusiness.GetInsuranceFileTax(r_vInsuranceFileTax:=m_vRITax, r_sDescription:=m_sDescription, iTask:=m_iTask)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetInsuranceFileTax Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            Else
                ' however for policy discount we want to load without recalculation

                lReturn = m_oBusiness.GetInsuranceFileTaxWithoutRecalculation(r_vInsuranceFileTax:=m_vRITax, r_sDescription:=m_sDescription, iTask:=m_iTask)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetInsuranceFileTax Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If
        End If
            If Information.IsArray(m_vRITax) Then
                m_lCurrencyID = m_vRITax(ACRCurrencyID, 0)
            End If


        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(ACClass, kMethodName, nResult, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return nResult
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
        iPMFunc.LogError(ACClass, kMethodName, result, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: PopulateTaxes
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 25-04-2005 : AUS005
    ' ***************************************************************** '
    Private Function PopulateTaxes() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulateTaxes"

        Dim lReturn, llBound, lUBound As Integer
        Dim oListItem As ListViewItem
        Dim lNotApp, lIncludeIns, lSpread As Integer
        Dim sNotApp, sIncludeIns, sSpread As String
        Try



        result = gPMConstants.PMEReturnCode.PMTrue
        ListViewFunc.ListViewBatchStart(lvwRITax)

        ' Clear list
        lvwRITax.Items.Clear()

        'm_lReturn = m_oBusiness.GetTaxesTotalDetails(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, vArray:=m_vArray)

        ' if there are any tax entries
        If Information.IsArray(m_vRITax) Then

            ' get array boundaries
            llBound = m_vRITax.GetLowerBound(1)
            lUBound = m_vRITax.GetUpperBound(1)

            ' for each tax group entry
            For lTax As Integer = llBound To lUBound

                ' Tax Group
                oListItem = lvwRITax.Items.Add("Tax" & lTax, m_vRITax(ACRTaxGroup, lTax), "")

                With oListItem

                    ' Group sequence
                    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_vRITax(ACRSequence, lTax)

                    ' Tax band description
                    ListViewHelper.GetListViewSubItem(oListItem, 2).Text = m_vRITax(ACRDescription, lTax)

                    ' Tax Amount
                    ListViewHelper.GetListViewSubItem(oListItem, 3).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_vRITax(ACRTaxValue, lTax))

                    ' Tax basis
                    Select Case m_vRITax(ACRCalcBasis, lTax)
                        Case CStr(ACCalcBasisRunningTotal)
                            ListViewHelper.GetListViewSubItem(oListItem, 4).Text = "Running Total"
                        Case CStr(ACCalcBasisSumInsuredChange)
                            ListViewHelper.GetListViewSubItem(oListItem, 4).Text = "Sum Insured Change"
                        Case CStr(ACCalcBasisSumInsured)
                            ListViewHelper.GetListViewSubItem(oListItem, 4).Text = "Sum Insured"
                        Case Else
                            ListViewHelper.GetListViewSubItem(oListItem, 4).Text = "Premium"
                    End Select
                    ' Three more columns has been added. (BPIS-Partial Instalment Work)
                    lNotApp = CInt(m_vRITax(ACRIsAppliedToClnt, lTax))
                    lIncludeIns = CInt(m_vRITax(ACRIncludeIns, lTax))
                    lSpread = CInt(m_vRITax(ACRSpread, lTax))
                    ' Rate
                    If CBool(m_vRITax(ACRIsValue, lTax)) Then
                            ListViewHelper.GetListViewSubItem(oListItem, 5).Text = m_vRITax(ACRTaxRate, lTax)
                        Else
                        ListViewHelper.GetListViewSubItem(oListItem, 5).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, m_vRITax(ACRTaxRate, lTax), -5)
                    End If

                    ' Class of Business
                    ListViewHelper.GetListViewSubItem(oListItem, 6).Text = m_vRITax(ACRClassOfBusiness, lTax)

                    ' Country
                    ListViewHelper.GetListViewSubItem(oListItem, 7).Text = m_vRITax(ACRCountry, lTax)

                    ' State
                    ListViewHelper.GetListViewSubItem(oListItem, 8).Text = m_vRITax(ACRState, lTax)
                    ' Is not applied to client
                    If lNotApp = 1 Then
                        sNotApp = "Yes"
                    Else
                        sNotApp = "No"
                    End If
                    ListViewHelper.GetListViewSubItem(oListItem, 9).Text = sNotApp

                    'Include Tax in instalment
                    If lIncludeIns = 1 Then
                        sIncludeIns = "Yes"
                    Else
                        sIncludeIns = "No"
                    End If

                    ListViewHelper.GetListViewSubItem(oListItem, 10).Text = sIncludeIns

                    'Spread Tax across instalment
                    If lSpread = 1 Then
                        sSpread = "Yes"
                    Else
                        sSpread = "No"
                    End If

                    ListViewHelper.GetListViewSubItem(oListItem, 11).Text = sSpread

                    'Apply tax By (RC)
                    If StringsHelper.ToDoubleSafe(m_vRITax(ACRApplyTaxBy, lTax)) = 0 Then
                        ListViewHelper.GetListViewSubItem(oListItem, 12).Text = "Transaction date"
                    ElseIf StringsHelper.ToDoubleSafe(m_vRITax(ACRApplyTaxBy, lTax)) = 1 Then
                        ListViewHelper.GetListViewSubItem(oListItem, 12).Text = "Effective date"
                    ElseIf StringsHelper.ToDoubleSafe(m_vRITax(ACRApplyTaxBy, lTax)) = 2 Then
                        ListViewHelper.GetListViewSubItem(oListItem, 12).Text = "Inception date"
                    End If


                    ' Row ID

                    .Tag = CStr(lTax)

                End With
            Next lTax
        End If
        
        ListViewFunc.ListViewBatchEnd()

        lvwRITax.Refresh()
        lvwRITax.Visible = True



        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(ACClass, kMethodName, result, excep:=ex)

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

        lvwRITax.FullRowSelect = True

        lvwRITax.Columns.Clear()

        lvwRITax.Columns.Add("kLRITaxColHeaderTaxGRoup", "Tax Group", CInt(VB6.TwipsToPixelsX(1500)))
        lvwRITax.Columns.Add("kRITaxColHeaderSequence", "Sequence", CInt(VB6.TwipsToPixelsX(1050)))
        lvwRITax.Columns.Add("kRITaxColHeaderTaxBand", "Tax Band", CInt(VB6.TwipsToPixelsX(1500)))
        lvwRITax.Columns.Add("kRITaxColHeaderTaxAmount", "Tax Amount", CInt(VB6.TwipsToPixelsX(1300)), HorizontalAlignment.Right, -1)
        lvwRITax.Columns.Add("kRITaxColHeaderCalculationBasis", "Calculation Basis", CInt(VB6.TwipsToPixelsX(1800)))
        lvwRITax.Columns.Add("kRITaxColHeaderRate", "Rate", CInt(VB6.TwipsToPixelsX(1000)), HorizontalAlignment.Right, -1)
        lvwRITax.Columns.Add("kRITaxColHeaderClassOfBusiness", "Class of Business", CInt(VB6.TwipsToPixelsX(1800)))
        lvwRITax.Columns.Add("kRITaxColHeaderCountry", "Country", CInt(VB6.TwipsToPixelsX(1800)))
        lvwRITax.Columns.Add("kRITaxColHeaderState", "State", CInt(VB6.TwipsToPixelsX(1800)))
        ' Three more columns has been added. (BPIS-Partial Instalment Work)
        lvwRITax.Columns.Add("kRITaxColHeaderIsNotInclude", "Is not applied to client", CInt(VB6.TwipsToPixelsX(1800)))
        lvwRITax.Columns.Add("kRITaxColHeaderIncludeIns", "Include Tax in instalment", CInt(VB6.TwipsToPixelsX(1800)))
        lvwRITax.Columns.Add("kRITaxColHeaderSpread", "Spread Tax across instalment", CInt(VB6.TwipsToPixelsX(1800)))
        lvwRITax.Columns.Add("kRITaxColHeaderApplyTaxBy", "Apply Tax by", CInt(VB6.TwipsToPixelsX(1800))) '(RC)



        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(ACClass, kMethodName, result, excep:=ex)

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
    '           Created : MEvans : 25-04-2005 : AUS005
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer


        Dim result As Integer = 0
        Const kMethodName As String = "SetProcessModes"

        Dim lReturn As Integer

        Try



        result = gPMConstants.PMEReturnCode.PMTrue


        If Not Information.IsNothing(vTask) Then

            m_iTask = CType(CInt(vTask), gPMConstants.PMEComponentAction)
            LoadWithoutCalculation = (m_iTask = gPMConstants.PMEComponentAction.PMView)
        End If


        If Not Information.IsNothing(vNavigate) Then

            m_lNavigate = CInt(vNavigate)
        End If


        If Not Information.IsNothing(vProcessMode) Then

            m_lProcessMode = CInt(vProcessMode)
        End If


        If Not Information.IsNothing(vTransactionType) Then

            m_sTransactionType = CStr(vTransactionType)

            If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                LoadWithoutCalculation = (m_sTransactionType = "")
            End If
        End If


        If Not Information.IsNothing(vEffectiveDate) Then

            m_dtEffectiveDate = CDate(vEffectiveDate)
        End If


        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(ACClass, kMethodName, result, excep:=ex)

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
    '           Created : MEvans : 25-04-2005 : AUS005
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
        iPMFunc.LogError(ACClass, kMethodName, result, excep:=ex)

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
            cmdEdit.Text = "View"
        End If

        lReturn = CType(GetSelectedItem(lItemId, lvwRITax), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "GetSelectedItem Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        cmdEdit.Enabled = lItemId <> -1



        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(ACClass, kMethodName, result, excep:=ex)

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
        iPMFunc.LogError(ACClass, kMethodName, result, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: EditItem
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 25-04-2005 : AUS005
    ' ***************************************************************** '
    Private Function EditItem() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "EditItem"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lItemId As Integer
        Dim ofrmTaxDetail As frmTaxDetail

        Try



        result = gPMConstants.PMEReturnCode.PMTrue

        RaiseEvent AboutToChange(Me, Nothing)
        If m_bCancelAboutToChangeAction Then
            m_bCancelAboutToChangeAction = False
            Return result
        End If

        lReturn = CType(GetSelectedItem(lItemId, lvwRITax), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "GetSelectedItem Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        ofrmTaxDetail = New frmTaxDetail()

        ofrmTaxDetail.ReadOnly_Renamed = m_bReadOnly
        ofrmTaxDetail.SelectedItem = lItemId
        ofrmTaxDetail.RITax = VB6.CopyArray(m_vRITax)


        ofrmTaxDetail.Business = m_oBusiness
        ofrmTaxDetail.TransactionType = m_sTransactionType '(RC)AUS Tax
        ofrmTaxDetail.ShowDialog()

        If ofrmTaxDetail.Status = gPMConstants.PMEReturnCode.PMOK And Not m_bReadOnly Then

            If ofrmTaxDetail.DataHasChanged Then

                m_vRITax = VB6.CopyArray(ofrmTaxDetail.RITax)

                ofrmTaxDetail = Nothing

                ' Now we have stored the new values we need to refresh the entire tax list to account
                ' for possible changes in sequential taxes...


                lReturn = m_oBusiness.CalculateTaxes(vTaxArray:=m_vRITax)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "CalculateTaxes Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                lReturn = PopulateTaxes()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "PopulateTaxes Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                lReturn = Save()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "", gPMConstants.PMELogLevel.PMLogError)
                End If

                RaiseEvent Change(Me, Nothing)

            End If

        End If



        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(ACClass, kMethodName, result, excep:=ex)

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
    ' Description: Saves all data back to the database
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function Save() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "Save"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Check if we are saving at risk level or at insurance file level
        If m_lRiskCnt <> 0 Then

            If Information.IsArray(m_vRITax) Then

                m_oBusiness.RiskCnt = m_lRiskCnt


                lReturn = m_oBusiness.UpdateRiskTax(v_vRiskTax:=m_vRITax)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "UpdateRiskTax Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

        ElseIf m_lInsuranceFileCnt <> 0 Then

            If Information.IsArray(m_vRITax) Then

                m_oBusiness.InsuranceFileCnt = m_lInsuranceFileCnt


                lReturn = m_oBusiness.UpdateInsuranceFileTax(v_vInsuranceFileTax:=m_vRITax)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "UpdateInsuranceFileTax Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

        End If



        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(ACClass, kMethodName, result, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
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
        For lItem As Integer = 1 To lvwRITax.Items.Count
            crTotalTax += CDbl(ListViewHelper.GetListViewSubItem(lvwRITax.Items.Item(lItem - 1), kRITaxColHeaderTaxAmount - 1).Text)
        Next

        m_crTotalTax = crTotalTax



        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(ACClass, kMethodName, result, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function
    Private Sub lvwRITax_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwRITax.Click
        SetupButtons()
    End Sub
End Class
