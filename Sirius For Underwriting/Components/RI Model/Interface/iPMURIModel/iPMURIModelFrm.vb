Option Strict Off
Option Explicit Off
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Imports System.Runtime.InteropServices
Imports System.Collections.Generic
Imports System.Linq
Imports System.Windows

Partial Friend Class frmRIModel
    Inherits System.Windows.Forms.Form

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmTreaty"

    ' ***************************************************************** '
    '                        PUBLIC PROPERTIES
    ' ***************************************************************** '
    Public Status As gPMConstants.PMEReturnCode

    ' Declare an instance of the Business object.
    Public Business As Object

    ' ***************************************************************** '
    '                        PRIVATE PROPERTIES
    ' ***************************************************************** '
    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Stores the return value for the a function call.
    Private m_lReturn As Integer

    ' Properties for treaty
    Private m_lRIModelID As Integer
    Private m_vRIModelLines(,) As Object
    Private m_vRIModelCurrRates(,) As Object
    Private m_vRIModelLinesVariableQuotaShare(,) As Object
    ' Original XOL ID's
    Private m_lXOLClmID As Integer
    Private m_lXOLCatID As Integer

    ' Current total limit for ri lines
    Private m_cTotalLimit As Decimal

    ' Priority summary for editing lines
    Private m_oPriorities As PriorityCollection
    Private m_sUnderwritingType As String = ""
    Private m_tabVariableQuotaShare As System.Windows.Forms.TabPage = Nothing
    Private m_bHasVariableQuotaShareTab As Boolean = False

    Private m_bIsRI2007Enabled As Boolean
    Private m_bIsMultipleRetainedTreaty As Boolean
    Private m_iRIConst As Integer
    Private m_vRIModels(,) As Object
    Private m_vRIModelAuditTrailArray(,) As Object
    'Developer Guide No.7
    Private Const vbFormCode As Integer = 0

    Private m_dtotalShare As Double

    Private m_bIsRIRegenerationEnabled As Boolean
    Private hScrollValue As Integer = 0
    Private m_sUniqueId As String
    Private m_iTreatyPremiumType As Integer = 0
    'Win32 API declarations to preserve list view horizontal scroll position after sort
    Const LVM_FIRST As Int32 = &H1000
    Const LVM_SCROLL As Int32 = LVM_FIRST + 20
    Const SBS_HORZ As Integer = 0

    Public WriteOnly Property UniqueId() As String
        Set(ByVal Value As String)
            m_sUniqueId = Value
        End Set
    End Property

    ' *******************************************************************************
    ' PUBLIC PROPERTIES
    ' *******************************************************************************
    <DllImport("user32.dll")>
    Private Shared Function GetScrollPos(ByVal hWnd As System.IntPtr, ByVal nBar As Integer) As Integer

    End Function
    <DllImport("user32.dll")>
    Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As UInteger, ByVal wParam As Integer, ByVal lParam As Integer) As Boolean

    End Function
    <DllImport("user32.dll")>
    Private Shared Function LockWindowUpdate(ByVal Handle As IntPtr) As Boolean

    End Function
    'Store the horizontal scroll value.
    Private Sub StoreHScrollValue()
        hScrollValue = GetScrollPos(lvwRIModelLine.Handle, SBS_HORZ)
    End Sub
    'Recover the old scroll position
    Private Sub RecoverHorizontalScroll()
        LockWindowUpdate(lvwRIModelLine.Handle)
        'Calculate the value the scroll needs to scroll back.
        Dim dx As Integer = hScrollValue - GetScrollPos(lvwRIModelLine.Handle, SBS_HORZ)
        'Send the scroll message.
        Dim b As Boolean = SendMessage(lvwRIModelLine.Handle, LVM_SCROLL, dx, 0)
        LockWindowUpdate(IntPtr.Zero)
    End Sub

    ' ***************************************************************** '
    '                         PUBLIC METHODS
    ' ***************************************************************** '
    Public Function Clear() As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Const kMethodName As String = "Clear"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear controls
            m_lRIModelID = 0
            txtCode.Text = ""
            txtDescription.Text = ""

            dtpEffectiveDate.Value = DateTime.Today

            dtpExpiryDate.Value = New DateTime(2005, 8, 10)
            optRIModelType(0).Checked = True
            optFACPremiums(0).Checked = True
            optClaimAllocation(0).Checked = True
            optTreatyPremium(0).Checked = True
            m_iTreatyPremiumType = 0
            chkClmXOL.CheckState = CheckState.Unchecked
            m_oFormFields.FormatControl(txtClmXOLLimit, 0)
            chkCatXOL.CheckState = CheckState.Unchecked
            m_oFormFields.FormatControl(txtCatXOLLimit, 0)
            chkCatXOLAutoReins.CheckState = CheckState.Unchecked
            m_oFormFields.FormatControl(txtCatXOLReinstatements, 0)

            ' Force click events on checkboxes
            chkClmXOL_CheckStateChanged(chkClmXOL, New EventArgs())
            chkCatXOL_CheckStateChanged(chkCatXOL, New EventArgs())

            ' Clear list view and adjust buttons
            lvwRIModelLine.Items.Clear()
            'Developer Guide No.9
            uctSummary.Initialise()
            uctSummary.SetProperties(0)
            cmdAdd.Enabled = True
            cmdEdit.Enabled = False
            cmdDelete.Enabled = False

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


        End Try
        Return result
    End Function

    Public Function GetProperties(ByRef lRIModelID As Integer, ByRef sCode As String, ByRef sDescription As String, ByRef dtEffectiveDate As Date, ByRef dtExpiryDate As Object, ByRef iRIModelType As Integer, ByRef iFACPremiumType As Integer, ByRef iClaimAllocationType As Integer, ByRef lCurrencyID As Integer, ByRef sCurrency As String, ByRef lXOLClmRIModelID As Integer, ByRef cXOLClmLimit As Decimal, ByRef lXOLCatRIModelID As Integer, ByRef cXOLCatLimit As Decimal, ByRef iXOLCatReinstatements As Integer, ByRef vRIModelLines(,) As Object, ByRef iTreatyPremiumType As Integer, ByRef vRIModelLinesVariableQuotaShare(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Const kMethodName As String = "GetProperties"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Return all detail data
            lRIModelID = m_lRIModelID
            sCode = txtCode.Text.Trim()
            sDescription = txtDescription.Text.Trim()

            dtEffectiveDate = dtpEffectiveDate.Value

            If dtpExpiryDate.Checked Then
                dtExpiryDate = dtpExpiryDate.Value
            End If
            iRIModelType = iPMFunc.GetOptionValue(optRIModelType)
            iFACPremiumType = iPMFunc.GetOptionValue(optFACPremiums)
            iClaimAllocationType = iPMFunc.GetOptionValue(optClaimAllocation)
            lCurrencyID = cboCurrency.CurrencyId
            lXOLClmRIModelID = IIf(chkClmXOL.CheckState = CheckState.Checked, cboClmXOLModel.ItemId, 0)

            cXOLClmLimit = IIf(chkClmXOL.CheckState = CheckState.Checked, CDec(m_oFormFields.UnformatControl(txtClmXOLLimit)), 0)
            lXOLCatRIModelID = IIf(chkCatXOL.CheckState = CheckState.Checked, cboCatXOLModel.ItemId, 0)

            cXOLCatLimit = IIf(chkCatXOL.CheckState = CheckState.Checked, CDec(m_oFormFields.UnformatControl(txtCatXOLLimit)), 0)

            iXOLCatReinstatements = IIf(chkCatXOL.CheckState = CheckState.Checked And chkCatXOLAutoReins.CheckState = CheckState.Checked, CInt(m_oFormFields.UnformatControl(txtCatXOLReinstatements)), 0)
            iTreatyPremiumType = m_iTreatyPremiumType
            vRIModelLines = VB6.CopyArray(m_vRIModelLines)
            vRIModelLinesVariableQuotaShare = IIf(m_vRIModelLinesVariableQuotaShare IsNot Nothing, VB6.CopyArray(m_vRIModelLinesVariableQuotaShare), Nothing)
        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally


        End Try
        Return result
    End Function

    ' Treaty Premium radio button event handlers
    Private Sub optTreatyPremium_CheckedChanged(sender As Object, e As EventArgs) Handles _optTreatyPremium_0.CheckedChanged, _optTreatyPremium_1.CheckedChanged
        Dim optButton As RadioButton = DirectCast(sender, RadioButton)
        If optButton.Checked Then
            If optButton Is _optTreatyPremium_0 Then
                m_iTreatyPremiumType = TreatyPremiumTypeEnum.Standard
            ElseIf optButton Is _optTreatyPremium_1 Then
                m_iTreatyPremiumType = TreatyPremiumTypeEnum.VariableCessionOrder
            End If
        End If
    End Sub

    ' Property to expose treaty premium type to child forms
    Public ReadOnly Property TreatyPremiumType() As Integer
        Get
            Return m_iTreatyPremiumType
        End Get
    End Property

    Public Function SetProperties(ByVal lRIModelID As Integer, ByVal sCode As String, ByVal sDescription As String, ByVal dtEffectiveDate As Date, ByVal dtExpiryDate As Object, ByVal iRIModelType As Integer, ByVal iFACPremiumType As Integer, ByVal iClaimAllocationType As Integer, ByVal lCurrencyID As Integer, ByRef lXOLClmRIModelID As Integer, ByRef cXOLClmLimit As Decimal, ByRef lXOLCatRIModelID As Integer, ByRef cXOLCatLimit As Decimal, ByRef iXOLCatReinstatements As Integer, Optional ByVal iTreatyPremiumType As Integer = 0, Optional ByVal vRIModelLinesVariableQuotaShare(,) As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "SetProperties"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set all detail data
            m_lRIModelID = lRIModelID
            txtCode.Text = sCode.Trim()
            txtDescription.Text = sDescription.Trim()
            dtpEffectiveDate.Value = dtEffectiveDate
            If (Not (Convert.IsDBNull(dtExpiryDate))) AndAlso (Not (String.IsNullOrEmpty(dtExpiryDate))) Then
                dtpExpiryDate.Checked = True
                dtpExpiryDate.Value = CDate(gPMFunctions.BlankToNull(dtExpiryDate))
            End If
            optRIModelType(iRIModelType).Checked = True
            optFACPremiums(iFACPremiumType).Checked = True
            optClaimAllocation(iClaimAllocationType).Checked = True

            ' Set treaty premium type
            m_iTreatyPremiumType = iTreatyPremiumType
            If iTreatyPremiumType >= 0 And iTreatyPremiumType <= 1 Then
                optTreatyPremium(iTreatyPremiumType).Checked = True
                ' Show Variable Quota Share tab if Variable Cession Order is selected

            Else
                optTreatyPremium(0).Checked = True

            End If

            cboCurrency.CurrencyId = lCurrencyID
            chkClmXOL.CheckState = IIf(lXOLClmRIModelID > 0, CheckState.Checked, CheckState.Unchecked)
            m_oFormFields.FormatControl(txtClmXOLLimit, cXOLClmLimit)
            chkCatXOL.CheckState = IIf(lXOLCatRIModelID > 0, CheckState.Checked, CheckState.Unchecked)
            m_oFormFields.FormatControl(txtCatXOLLimit, cXOLCatLimit)
            chkCatXOLAutoReins.CheckState = IIf(iXOLCatReinstatements > 0, CheckState.Checked, CheckState.Unchecked)
            m_oFormFields.FormatControl(txtCatXOLReinstatements, iXOLCatReinstatements)

            ' Force click events on checkboxes
            chkClmXOL_CheckStateChanged(chkClmXOL, New EventArgs())
            chkCatXOL_CheckStateChanged(chkCatXOL, New EventArgs())

            ' Load ri model lines
            lReturn = GetRIModelLines()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetRIModelLines", "Unable to retrieve Ri Model Line information")
            End If

            ' Load quotashare config
            lReturn = GetRIModelLineQuotaShareConfig()
            ' Display ri model lines
            lReturn = RefreshRILines()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("RefreshRILines", "Unable to display Ri Model Line information")
            End If

            ' Refresh xol models
            m_lXOLClmID = lXOLClmRIModelID
            m_lXOLCatID = lXOLCatRIModelID
            lReturn = RefreshXOLModelList()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("RefreshXOLModelList", "Unable to refresh the xol model lists")
            End If
            cboClmXOLModel.ItemId = lXOLClmRIModelID
            cboCatXOLModel.ItemId = lXOLCatRIModelID

            ' Initialise the RI Model summary
            lReturn = uctSummary.Initialise()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("uctSummary.Initialise", "Unable to initialise ri model summary control")
            End If

            ' Update the RI Model summary
            lReturn = uctSummary.SetProperties(m_lRIModelID)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("uctSummary.SetProperties(m_lRIModelID)", "Unable to populate ri model summary control")
            End If

            If m_bIsRI2007Enabled Then
                lReturn = CType(SetRIModelAuditTrail(), gPMConstants.PMEReturnCode)
            End If
        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    '                         PRIVATE METHODS
    ' ***************************************************************** '
    Private Function GetRIModelLines() As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "SetFieldValidation"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get treaty party details from the business object.

            lReturn = Business.GetRIModelLines(v_lRIModelID:=m_lRIModelID, r_vRIModelLines:=m_vRIModelLines)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oBusiness.GetRIModelLines", "Unable to retrieve ri model lines")
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


        End Try
        Return result
    End Function

    Private Function GetRIModelLineQuotaShareConfig() As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "SetFieldValidation"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ' Get treaty party details from the business object.
            lReturn = Business.GetRIModelVariableQuotaShareConfig(v_lRIModelID:=m_lRIModelID, v_vRIModelLinesVariableQuotaShare:=m_vRIModelLinesVariableQuotaShare)
        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally

        End Try
        Return result
    End Function




    ' ***************************************************************** '
    '                         PRIVATE METHODS
    ' ***************************************************************** '
    Private Function GetRIModelCurrencyRates() As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "GetRIModelCurrencyRates"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get treaty party details from the business object.

            lReturn = Business.GetRIModelCurrencyRates(v_lRIModelID:=m_lRIModelID, r_vRIModelCurrRates:=m_vRIModelCurrRates)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oBusiness.GetRIModelCurrencyRates", "Unable to retrieve ri model lines")
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


        End Try
        Return result
    End Function

    Private Function IsValidRILine() As Boolean

        Dim result As Boolean = False
        Dim lReturn As Integer
        Const kMethodName As String = "IsValidRILine"

        Try

            ' Check for active item
            If Not (lvwRIModelLine.FocusedItem Is Nothing) Then
                ' Check for total line
                If lvwRIModelLine.FocusedItem.Name <> "TOTAL" Then
                    ' Valid party line
                    result = True
                    Return result
                End If
            End If

            ' Not a valid party
            cmdEdit.Enabled = False
            cmdDelete.Enabled = False


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally


        End Try
        Return result
    End Function

    Private Function RefreshPriorities(ByVal lPriority As Integer, ByVal lLines As Decimal, ByVal cLineLimit As Decimal) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Const kMethodName As String = "RefreshPriorities"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Only if we have an array (which we should else why would this be called :-s)
            If Information.IsArray(m_vRIModelLines) Then
                For lCount As Integer = m_vRIModelLines.GetLowerBound(1) To m_vRIModelLines.GetUpperBound(1)
                    ' Check for matching priority
                    If CDbl(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLPriority, lCount)) = lPriority Then
                        ' Surplus lines decimals - do not overwrite number_of_lines for surplus types
                        ' as each surplus line has its own independent decimal value
                        Dim iRIType As Integer = gPMFunctions.ToSafeInteger(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRITypeID, lCount))
                        Dim isSurplus As Boolean = iRIType = ACSurplus OrElse iRIType = ACSecondSurplus OrElse iRIType = ACThirdSurplus
                        If Not isSurplus Then
                            m_vRIModelLines(MainModule.RIModelLineEnum.DBMLNumberOfLines, lCount) = lLines
                        End If
                    End If
                Next lCount
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


        End Try
        Return result
    End Function

    Private Function RefreshRILines(Optional ByVal v_lIndex As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim cLimit As Decimal
        Dim oListItem As ListViewItem
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "RefreshRILines"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the list before we start
            cmdEdit.Enabled = False
            cmdDelete.Enabled = False
            lvwRIModelLine.Items.Clear()
            m_cTotalLimit = 0

            If Information.IsArray(m_vRIModelLines) Then
                ' Process all treaties
                For lCount As Integer = m_vRIModelLines.GetLowerBound(1) To m_vRIModelLines.GetUpperBound(1)
                    ' Don't add deleted (party_cnt = 0) parties
                    If CDbl(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyID, lCount)) > 0 Then
                        ' Calculate this lines share and keep running total
                        If m_bIsRI2007Enabled And CDbl(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyTypeId, lCount)) = 2 Then

                            cLimit = gPMFunctions.ToSafeCurrency(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLineLimit, lCount)) - gPMFunctions.ToSafeCurrency(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLowerLimit, lCount))
                        Else
                            cLimit = (CDbl(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLNumberOfLines, lCount)) * CDbl(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLineLimit, lCount))) * (CDbl(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLSharePercent, lCount)) / 100)
                        End If

                        If ToSafeInteger(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLCedePremiumOnly, lCount), 0) = 1 Then
                            cLimit = 0
                        End If
                        m_cTotalLimit += cLimit

                        ' Add the list item
                        oListItem = lvwRIModelLine.Items.Add("ML" & lCount, CStr(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLPriority, lCount)), "")

                        ' Surplus lines decimals - use DBMLRITypeID to determine if surplus type
                        Dim iRIType As Integer = gPMFunctions.ToSafeInteger(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRITypeID, lCount))
                        Dim isSurplusType As Boolean = iRIType = ACSurplus OrElse iRIType = ACSecondSurplus OrElse iRIType = ACThirdSurplus
                        If isSurplusType Then
                            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDecimal, CDec(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLNumberOfLines, lCount)), 2)
                        Else
                            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(CInt(Math.Floor(CDec(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLNumberOfLines, lCount)))))
                        End If
                        If m_bIsRI2007Enabled Then ListViewHelper.GetListViewSubItem(oListItem, 2).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, gPMFunctions.ToSafeCurrency(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLowerLimit, lCount)))
                        ListViewHelper.GetListViewSubItem(oListItem, 2 + m_iRIConst).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLineLimit, lCount))
                        ListViewHelper.GetListViewSubItem(oListItem, 3 + m_iRIConst).Text = CStr(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLDescription, lCount))
                        ListViewHelper.GetListViewSubItem(oListItem, 4 + m_iRIConst).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, m_vRIModelLines(MainModule.RIModelLineEnum.DBMLSharePercent, lCount), -5)
                        ListViewHelper.GetListViewSubItem(oListItem, 5 + m_iRIConst).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, cLimit)

                        ' Store array index so we can find the original record
                        oListItem.Tag = CStr(lCount)

                        ' Check for selected item
                        If lCount = v_lIndex Then
                            ' If we are refreshing reselect the original item
                            lvwRIModelLine.FullRowSelect = True
                            lvwRIModelLine.Items(oListItem.Index).Selected = True
                            lvwRIModelLine.Select()
                            If lvwRIModelLine.Visible Then
                                lvwRIModelLine.Focus()
                            End If
                            lvwRIModelLine_SelectedIndexChanged(lvwRIModelLine, New EventArgs())
                        End If
                    End If
                Next lCount
            End If

            ' Ignore errors this is only a cosmetic nicetytaruy
            lReturn = CType(ListView6Func.ListViewAutoSize(lvwRIModelLine, True, True, Me), gPMConstants.PMEReturnCode)

            ' Refresh sort order (also adds total row)
            SortList(ListViewHelper.GetSortKeyProperty(lvwRIModelLine), True)


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally

        End Try
        Return result
    End Function

    Private Function RefreshXOLModelList() As Integer

        Dim result As Integer = 0
        Dim lOriginal, lReturn As Integer
        Const kMethodName As String = "RefreshXOLModelList"
        Dim maxDefaultDate As Date = Date.MaxValue
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set filter criteria
            ' - We only want xol type models!
            ' - We only want treaties older than this one,
            ' - or the originally selected model
            If dtpExpiryDate.Checked Then
                lOriginal = cboClmXOLModel.ItemId
                cboClmXOLModel.WhereClause = "(ri_model_id = " & m_lXOLClmID & ") OR " &
                                     "(ri_model_type = 3" &
                                     " AND effective_date < '" & gPMFunctions.ToSafeDate(dtpExpiryDate.Value, maxDefaultDate).ToString("yyyy.MM.dd") & "'" &
                                     " AND IsNull(expiry_date,'" & maxDefaultDate.ToString("yyyy.MM.dd") & "') > '" & DateTime.FromOADate(dtpEffectiveDate.Value.ToOADate()).ToString("yyyy.MM.dd") & "')"
                lOriginal = cboCatXOLModel.ItemId
                cboCatXOLModel.WhereClause = "(ri_model_id = " & m_lXOLCatID & ") OR " &
                                         "(ri_model_type = 3" &
                                         " AND effective_date < '" & gPMFunctions.ToSafeDate(dtpExpiryDate.Value, maxDefaultDate).ToString("yyyy.MM.dd") & "'" &
                                         " AND IsNull(expiry_date,'" & maxDefaultDate.ToString("yyyy.MM.dd") & "') > '" & DateTime.FromOADate(dtpEffectiveDate.Value.ToOADate()).ToString("yyyy.MM.dd") & "')"
            Else
                ' Set filter criteria
                ' - We only want xol type models!
                ' - We only want treaties older than this one,
                ' - or the originally selected model
                lOriginal = cboClmXOLModel.ItemId
                cboClmXOLModel.WhereClause = "(ri_model_id = " & m_lXOLClmID & ") OR " & "(ri_model_type = 3)"
                lOriginal = cboCatXOLModel.ItemId
                cboCatXOLModel.WhereClause = "(ri_model_id = " & m_lXOLCatID & ") OR " & "(ri_model_type = 3)"
            End If
            cboClmXOLModel.RefreshList()
            cboClmXOLModel.ItemId = lOriginal


            cboCatXOLModel.RefreshList()
            cboCatXOLModel.ItemId = lOriginal


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally


        End Try
        Return result
    End Function

    Private Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "SetFieldValidation"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the form control object.
            m_oFormFields = New iPMFormControl.FormFields()
            m_oFormFields.LanguageID = g_iLanguageID

            lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCode, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oFormFields.AddNewFormField", "Failed to add txtCode")
            End If

            lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDescription, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oFormFields.AddNewFormField", "Failed to add txtDescription")
            End If

            lReturn = m_oFormFields.AddNewFormField(ctlControl:=dtpEffectiveDate, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oFormFields.AddNewFormField", "Failed to add dtpEffectiveDate")
            End If

            lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oFormFields.AddNewFormField", "Failed to add cboCurrency")
            End If

            ' XOL per Claim
            lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboClmXOLModel, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oFormFields.AddNewFormField", "Failed to add cboClmXOLModel")
            End If

            lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtClmXOLLimit, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oFormFields.AddNewFormField", "Failed to add txtClmXOLLimit")
            End If

            ' XOL Catastrophe
            lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboCatXOLModel, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oFormFields.AddNewFormField", "Failed to add cboCatXOLModel")
            End If

            lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCatXOLLimit, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oFormFields.AddNewFormField", "Failed to add txtCatXOLLimit")
            End If

            lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCatXOLReinstatements, lFormat:=gPMConstants.PMEFormatStyle.PMFormatInteger, lFieldType:=gPMConstants.PMEDataType.PMInteger, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oFormFields.AddNewFormField", "Failed to add txtCatXOLReinstatements")
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


        End Try
        Return result
    End Function

    Private Function SortList(ByVal lColumnIndex As Integer, Optional ByVal bReSort As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem
        Const kMethodName As String = "SortList"

        ' Remove an existing total row (ignore errors)
        lvwRIModelLine.Items.RemoveByKey("TOTAL")

        result = gPMConstants.PMEReturnCode.PMTrue

        ' We may just be refreshing after a item edit or addition
        If Not bReSort And lColumnIndex <> 4 Then
            ' Reverse sort order if column hasn't changed
            If ListViewHelper.GetSortKeyProperty(lvwRIModelLine) = lColumnIndex Then
                ListViewHelper.SetSortOrderProperty(lvwRIModelLine, IIf(ListViewHelper.GetSortOrderProperty(lvwRIModelLine) = SortOrder.Ascending, SortOrder.Descending, SortOrder.Ascending))
            Else
                ListViewHelper.SetSortOrderProperty(lvwRIModelLine, SortOrder.Ascending)
            End If
        End If

        ' Sort based on contents
        Select Case lColumnIndex
            Case 0, 1 ' Numeric
                ListView6Func.ListViewSortByValue(lvwRIModelLine, lColumnIndex, ListViewHelper.GetSortOrderProperty(lvwRIModelLine), True)
            Case 2, 5 ' Percentage or currency
                ListView6Func.ListViewSortByValue(lvwRIModelLine, lColumnIndex, ListViewHelper.GetSortOrderProperty(lvwRIModelLine), True, True)
            Case 4
                ListViewHelper.SetSortedProperty(lvwRIModelLine, True)
                ' Sort by this column (ascending).
                ListViewHelper.SetSortedProperty(lvwRIModelLine, False)

                ' Turn off sorting so that the list
                ' is not sorted twice
                If ListViewHelper.GetSortOrderProperty(lvwRIModelLine) = SortOrder.Ascending Then
                    ListViewHelper.SetSortOrderProperty(lvwRIModelLine, SortOrder.Descending)
                Else
                    ListViewHelper.SetSortOrderProperty(lvwRIModelLine, SortOrder.Ascending)
                End If
                ListViewHelper.SetSortKeyProperty(lvwRIModelLine, lColumnIndex + 1 - 1)

            Case Else
                ListViewHelper.SetSortKeyProperty(lvwRIModelLine, lColumnIndex)
                ListViewHelper.SetSortedProperty(lvwRIModelLine, True)
                ListViewHelper.SetSortedProperty(lvwRIModelLine, False) ' Need to turn off sort to protect Total row
        End Select

        ' Add summary list item
        oListItem = lvwRIModelLine.Items.Add("TOTAL", "", "")
        ListViewHelper.GetListViewSubItem(oListItem, 1).Text = " "
        ListViewHelper.GetListViewSubItem(oListItem, 2).Text = " "
        ListViewHelper.GetListViewSubItem(oListItem, 3).Text = " "
        If m_bIsRI2007Enabled Then ListViewHelper.GetListViewSubItem(oListItem, 4).Text = " "
        ListViewHelper.GetListViewSubItem(oListItem, 4 + m_iRIConst).Text = "Total"
        ListViewHelper.GetListViewSubItem(oListItem, 5 + m_iRIConst).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_cTotalLimit)

        ' Set totals as bold
        oListItem.Font = VB6.FontChangeBold(oListItem.Font, True)
        For Each oListSubItem As ListViewItem.ListViewSubItem In oListItem.SubItems
            oListSubItem.Font = VB6.FontChangeBold(oListSubItem.Font, True)
        Next oListSubItem

        GoTo Finally_Renamed
Catch_Renamed:
        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)

Finally_Renamed:
        Return result
    End Function

    Private Function Validate_Renamed() As Integer

        Dim result As Integer = 0
        Const kTreatyPercentage As Integer = 100
        Dim oPriority As Priority
        Dim bOverlapLimit, bDuplicateRILine As Boolean
        Dim bIsRetainedReinsurer As Boolean
        Dim iCountRetained As Integer
        Dim bModelHasSurplus As Boolean
        Dim bModelHasQS As Boolean
        Dim dLineLimit As Double
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "Validate"
        Dim priority As New List(Of Integer)
        Dim dPriorityShare As Double

        Try

            ' Default to false, only set true if we get to the end
            result = gPMConstants.PMEReturnCode.PMFalse

            m_dtotalShare = 0
            ' Standard validation
            lReturn = m_oFormFields.CheckMandatoryControls()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If dtpExpiryDate.Value < dtpEffectiveDate.Value AndAlso dtpExpiryDate.Checked Then
                MessageBox.Show("Expiry date can't be less than Effective date", Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return result
            End If

            ' Validate priorities
            lReturn = CType(m_oPriorities.Refresh(m_vRIModelLines, , , m_bIsRI2007Enabled), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("BuildPriorities(lIndex, vPriorities)", "Unable to build priority array")
            End If

            'Check that the code already Exists or not
            If Information.IsArray(m_vRIModels) Then
                For lCount As Integer = m_vRIModels.GetLowerBound(1) To m_vRIModels.GetUpperBound(1)
                    If CStr(m_vRIModels(MainModule.RIModelEnum.DBMCode, lCount)).Trim() = txtCode.Text.Trim() And CDbl(m_vRIModels(MainModule.RIModelEnum.DBMRIModelID, lCount)) <> m_lRIModelID Then
                        MessageBox.Show("RI Model Code already exists.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        SSTabHelper.SetSelectedIndex(tabRIModel, 0)
                        txtCode.Focus()
                        Return result
                    End If
                Next
            End If

            If Not m_bIsRI2007Enabled Then

                ' If next available priority is 0% allocated then all preceeding priorities are fully allocated
                oPriority = m_oPriorities.NextAvailable
                If oPriority.Share > 0 And oPriority.Share < 100 Then
                    MessageBox.Show("RI Lines at priority " & oPriority.Priority & " are not 100% allocated.", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return result
                ElseIf oPriority.Share > 100 Then
                    MessageBox.Show("RI Lines at priority " & oPriority.Priority & " can not allocated more than 100%", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return result
                End If

            Else
                'lReturn = CType(m_oPriorities.Refresh(m_vRIModelLines, , , m_bIsRI2007Enabled), gPMConstants.PMEReturnCode)
                ' If next available priority is 0% allocated then all preceeding priorities are fully allocated
                oPriority = m_oPriorities.NextAvailable
                If oPriority.Share > 100 Then
                    MessageBox.Show("RI Lines at priority " & oPriority.Priority & " can not allocated more than 100%", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return result
                End If
                If Information.IsArray(m_vRIModelLines) Then
                    For lCount As Integer = m_vRIModelLines.GetLowerBound(1) To m_vRIModelLines.GetUpperBound(1)
                        If CDbl(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRITypeID, lCount)) <> ACQuotaShare Then

                            If (CDbl(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRITypeID, lCount)) = ACSurplus) Then
                                bModelHasSurplus = True
                                dLineLimit = ToSafeCurrency(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLineLimit, lCount))
                            End If
                        ElseIf ((m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRITypeID, lCount) = ACQuotaShare)) Then
                            If ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRIModelLineID, lCount), 0) >= 0 And m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyTypeId, lCount) = ACProportional Then
                                m_dtotalShare = m_dtotalShare + ToSafeDouble(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLSharePercent, lCount), 0)
                                bModelHasQS = True
                            End If

                        End If
                    Next lCount
                End If

                If m_dtotalShare > 100 Then
                    MsgBox("Allocated % must be in range of 0.01% and 100.00% when Reinsurance Type is Quota Share", vbInformation, "RI Model")
                    Return result
                End If
            End If

            If Information.IsArray(m_vRIModelLines) Then
                bOverlapLimit = False
                bDuplicateRILine = False
                For lCount As Integer = m_vRIModelLines.GetLowerBound(1) To m_vRIModelLines.GetUpperBound(1)
                    If ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyTypeId, lCount)) = 2 And ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyID, lCount)) <> 0 And
                                ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRITypeID, lCount)) = 5 Then 'E005 Part 2
                        For lCount1 As Integer = LBound(m_vRIModelLines, 2) To UBound(m_vRIModelLines, 2)
                            If m_bIsRI2007Enabled = True Then
                                If ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyTypeId, lCount1)) = 2 And (lCount <> lCount1) And ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyID, lCount1)) <> 0 And ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRITypeID, lCount1)) = 5 And ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLPriority, lCount)) = ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLPriority, lCount1)) Then
                                    If (ToSafeCurrency(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLineLimit, lCount)) > ToSafeCurrency(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLowerLimit, lCount1)) And ToSafeCurrency(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLineLimit, lCount)) <= ToSafeCurrency(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLineLimit, lCount1))) Or
                                    (ToSafeCurrency(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLowerLimit, lCount)) >= ToSafeCurrency(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLowerLimit, lCount1)) And ToSafeCurrency(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLowerLimit, lCount)) < ToSafeCurrency(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLineLimit, lCount1))) Then
                                        bOverlapLimit = True
                                        Exit For
                                    End If
                                End If
                            Else
                                If ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyTypeId, lCount1)) = 2 And (lCount <> lCount1) And ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyID, lCount1)) <> 0 And ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRITypeID, lCount1)) = 5 Then
                                    If (ToSafeCurrency(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLineLimit, lCount)) > ToSafeCurrency(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLowerLimit, lCount1)) And ToSafeCurrency(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLineLimit, lCount)) <= ToSafeCurrency(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLineLimit, lCount1))) Or
                                    (ToSafeCurrency(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLowerLimit, lCount)) >= ToSafeCurrency(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLowerLimit, lCount1)) And ToSafeCurrency(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLowerLimit, lCount)) < ToSafeCurrency(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLineLimit, lCount1))) Then
                                        bOverlapLimit = True
                                        Exit For
                                    End If
                                End If
                            End If

                        Next lCount1
                        If bOverlapLimit = True Then Exit For

                    ElseIf ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyTypeId, lCount)) = 2 And ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyID, lCount)) <> 0 And ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRITypeID, lCount)) = 12 Then
                        For lCount1 As Integer = LBound(m_vRIModelLines, 2) To UBound(m_vRIModelLines, 2)
                            If m_bIsRI2007Enabled = False Then
                                If ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyTypeId, lCount1)) = 2 And (lCount <> lCount1) And ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyID, lCount1)) <> 0 And ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRITypeID, lCount1)) = 12 Then
                                    If (ToSafeCurrency(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLineLimit, lCount)) > ToSafeCurrency(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLowerLimit, lCount1)) And ToSafeCurrency(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLineLimit, lCount)) <= ToSafeCurrency(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLineLimit, lCount1))) Or
                                    (ToSafeCurrency(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLowerLimit, lCount)) >= ToSafeCurrency(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLowerLimit, lCount1)) And ToSafeCurrency(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLowerLimit, lCount)) < ToSafeCurrency(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLineLimit, lCount1))) Then
                                        bOverlapLimit = True
                                        Exit For
                                    End If
                                End If
                            Else
                                If ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyTypeId, lCount1)) = 2 And (lCount <> lCount1) And ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyID, lCount1)) <> 0 And ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRITypeID, lCount1)) = 12 And ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLPriority, lCount)) = ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLPriority, lCount1)) Then
                                    If (ToSafeCurrency(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLineLimit, lCount)) > ToSafeCurrency(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLowerLimit, lCount1)) And ToSafeCurrency(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLineLimit, lCount)) <= ToSafeCurrency(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLineLimit, lCount1))) Or
                                    (ToSafeCurrency(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLowerLimit, lCount)) >= ToSafeCurrency(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLowerLimit, lCount1)) And ToSafeCurrency(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLowerLimit, lCount)) < ToSafeCurrency(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLineLimit, lCount1))) Then
                                        bOverlapLimit = True
                                        Exit For
                                    End If
                                End If
                            End If

                        Next lCount1
                        If bOverlapLimit = True Then Exit For
                    ElseIf (gPMFunctions.ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyTypeId, lCount)) = 1 Or Not m_bIsRI2007Enabled) And gPMFunctions.ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyID, lCount)) <> 0 Then  ' Quota share and other treaties
                        For lCount1 As Integer = m_vRIModelLines.GetLowerBound(1) To m_vRIModelLines.GetUpperBound(1)
                            If (gPMFunctions.ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyTypeId, lCount1)) = 1 Or Not m_bIsRI2007Enabled) And (lCount <> lCount1) And gPMFunctions.ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyID, lCount1)) <> 0 Then
                                If (gPMFunctions.ToSafeCurrency(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLineLimit, lCount)) = gPMFunctions.ToSafeCurrency(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLineLimit, lCount1))) And (gPMFunctions.ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyID, lCount)) = gPMFunctions.ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyID, lCount1))) Then
                                    bDuplicateRILine = True
                                    Exit For
                                End If
                            End If
                        Next
                        If bDuplicateRILine Then Exit For
                    End If
                    Dim value = m_vRIModelLines(MainModule.RIModelLineEnum.DBMLPriority, lCount)
                    If IsNumeric(value) Then
                        priority.Add(Convert.ToInt32(value))
                    End If
                Next lCount
                If m_bIsRI2007Enabled = True Then
                    If priority IsNot Nothing Then
                        Dim uniqueSorted = priority.Where(Function(n) n <> 0).Distinct().OrderBy(Function(n) n).ToList()

                        If uniqueSorted IsNot Nothing Then
                            Dim endValue = uniqueSorted.Count

                            If endValue > 0 Then
                                For i As Integer = 1 To endValue
                                    If Not uniqueSorted.Contains(i) Then
                                        MessageBox.Show("Some treaty priorities are missing. Please review and ensure that all priority values are in a continuous sequence without gaps.", "Priority", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                        Return result
                                    End If
                                Next
                            End If
                        End If
                    End If
                End If

                If bOverlapLimit Then
                    MessageBox.Show("Treaty Line Limits cannot overlap.", "Line Limits Overlaps", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return result
                End If
                If bDuplicateRILine Then
                    MessageBox.Show("Reinsurance Lines cannot be duplicated.", "Duplicate Check", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return result
                End If

                'Check for same upper limit for Quota Share treaties at same priority
                If IsArray(m_vRIModelLines) And m_bIsRI2007Enabled = True Then
                    For iCount As Integer = m_vRIModelLines.GetLowerBound(1) To m_vRIModelLines.GetUpperBound(1)
                        If m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRITypeID, iCount) = ACQuotaShare AndAlso ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyID, iCount)) <> 0 Then
                            For jCount As Integer = m_vRIModelLines.GetLowerBound(1) To m_vRIModelLines.GetUpperBound(1)
                                If iCount <> jCount AndAlso m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRITypeID, jCount) = ACQuotaShare AndAlso ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyID, jCount)) <> 0 Then
                                    If ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLPriority, iCount)) = ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLPriority, jCount)) Then
                                        If ToSafeDouble(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLineLimit, iCount)) <> ToSafeDouble(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLineLimit, jCount)) Then
                                            MsgBox("For Quota share at same priority, line/upper limit must be equal.", vbCritical, "Line Limits Not Equal")
                                            Return result
                                        End If
                                    End If
                                End If
                            Next
                        End If

                    Next iCount
                End If

                'Check for same upper limit for Surplus and quotaShare
                If bModelHasQS AndAlso bModelHasSurplus Then
                    If IsArray(m_vRIModelLines) Then
                        For iCount As Integer = m_vRIModelLines.GetLowerBound(1) To m_vRIModelLines.GetUpperBound(1)
                            If (m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRITypeID, iCount) = ACQuotaShare) OrElse (m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRITypeID, iCount) = ACSurplus) Then
                                If m_bIsRI2007Enabled = False Then
                                    If Not ToSafeBoolean(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRIIsObligatory, iCount)) AndAlso (ToSafeDouble(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLineLimit, iCount)) <> dLineLimit) Then
                                        MsgBox("Surplus And QuotaShare Upper Limits must be equal.", vbCritical, "Line Limits Not Equal")
                                        Return result
                                    End If
                                Else
                                    For jCount As Integer = m_vRIModelLines.GetLowerBound(1) To m_vRIModelLines.GetUpperBound(1)
                                        If iCount <> jCount AndAlso ((m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRITypeID, jCount) = ACQuotaShare) OrElse (m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRITypeID, jCount) = ACSurplus)) Then
                                            If ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLPriority, iCount)) = ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLPriority, jCount)) Then
                                                If Not ToSafeBoolean(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRIIsObligatory, iCount)) AndAlso (ToSafeDouble(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLineLimit, iCount)) <> ToSafeDouble(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLineLimit, jCount))) Then
                                                    MsgBox("Surplus And QuotaShare Upper Limits must be equal.", vbCritical, "Line Limits Not Equal")
                                                    Return result
                                                End If
                                            End If
                                        End If
                                    Next
                                End If
                            End If
                        Next iCount
                    End If
                End If

                If m_bIsMultipleRetainedTreaty = False Or m_bIsRI2007Enabled = False Then
                    'Check for Retained lines
                    iCountRetained = 0
                    For lCount As Integer = m_vRIModelLines.GetLowerBound(1) To m_vRIModelLines.GetUpperBound(1)
                        If CDbl(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyID, lCount)) <> 0 Then
                            If CDbl(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRITypeID, lCount)) = ACRetained Then
                                bIsRetainedReinsurer = False

                                lReturn = Business.CheckRetainedReinsurer(gPMFunctions.ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyID, lCount)), bIsRetainedReinsurer)

                                If bIsRetainedReinsurer Then
                                    iCountRetained += 1
                                End If
                            End If
                        End If
                    Next

                    If iCountRetained > 1 Then
                        MessageBox.Show("You cannot have more than ONE treaty that contains RETAINED, edit the treaties to correct.", "Invalid Treaty", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return result
                    End If

                    ' Also check that any Retained RI type line has a retained reinsurer party
                    For lCount As Integer = m_vRIModelLines.GetLowerBound(1) To m_vRIModelLines.GetUpperBound(1)
                        If CDbl(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyID, lCount)) <> 0 AndAlso
                           CDbl(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRITypeID, lCount)) = ACRetained Then
                            bIsRetainedReinsurer = False
                            lReturn = Business.CheckRetainedReinsurer(gPMFunctions.ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyID, lCount)), bIsRetainedReinsurer)
                            If Not bIsRetainedReinsurer Then
                                MessageBox.Show("A treaty with Reinsurance Type 'Retained' must have a retained reinsurer party. Edit the treaties to correct.", "Invalid Treaty", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                Return result
                            End If
                        End If
                    Next

                End If

            End If


            'Validate treaty dependencies (missing types And circular references)
            If m_iTreatyPremiumType = TreatyPremiumTypeEnum.VariableCessionOrder AndAlso m_bIsRI2007Enabled Then
                If ValidateTreatyDependencies() <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If
            End If

            ' Validate that all Quota Share Retained treaties share the same priority as the Retained treaty
            If Information.IsArray(m_vRIModelLines) AndAlso m_bIsRI2007Enabled Then
                Dim iRetainedPriority As Integer = -1
                Dim qsRetainedPriorities As New List(Of Integer)
                For lCount As Integer = m_vRIModelLines.GetLowerBound(1) To m_vRIModelLines.GetUpperBound(1)
                    If gPMFunctions.ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyID, lCount)) > 0 Then
                        Dim iRIType As Integer = gPMFunctions.ToSafeInteger(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRITypeID, lCount))
                        Dim iPriority As Integer = gPMFunctions.ToSafeInteger(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLPriority, lCount))
                        If iRIType = ACQuotaShareRetained Then qsRetainedPriorities.Add(iPriority)
                        If iRIType = ACRetained Then iRetainedPriority = iPriority
                    End If
                Next lCount
                If iRetainedPriority > -1 AndAlso qsRetainedPriorities.Count > 0 Then
                    If qsRetainedPriorities.Any(Function(p) p <> iRetainedPriority) Then
                        MsgBox("Quota Share Retained Treaty and the Retained Treaty should be on the same priority", vbExclamation, "Invalid Priority Configuration")
                        Return result
                    End If
                End If
            End If

            ' All validation passed return True
            result = gPMConstants.PMEReturnCode.PMTrue


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


        End Try
        Return result
    End Function


    ' ***************************************************************** '
    '                             EVENTS
    ' ***************************************************************** '
    Private Sub chkCatXOL_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkCatXOL.CheckStateChanged

        ' Enable/disable catastrophe xol
        Dim bEnable As Boolean = (chkCatXOL.CheckState = CheckState.Checked)
        lblCatXOLModel.Enabled = bEnable
        cboCatXOLModel.Enabled = bEnable
        lblCatXOLLimit.Enabled = bEnable
        txtCatXOLLimit.Enabled = bEnable
        chkCatXOLAutoReins.Enabled = bEnable
        ' Additional rules for reinstatements
        lblCatXOLReinstatements.Enabled = bEnable And (chkCatXOLAutoReins.CheckState = CheckState.Checked)
        txtCatXOLReinstatements.Enabled = bEnable And (chkCatXOLAutoReins.CheckState = CheckState.Checked)

        ' Update field validation
        m_oFormFields.Item("cboCatXOLModel-0").IsMandatory = bEnable
        m_oFormFields.Item("txtCatXOLLimit-0").IsMandatory = bEnable
        m_oFormFields.Item("txtCatXOLReinstatements-0").IsMandatory = txtCatXOLReinstatements.Enabled

    End Sub

    Private Sub chkCatXOLAutoReins_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkCatXOLAutoReins.CheckStateChanged

        ' Enable/disable reinstatement text
        Dim bEnable As Boolean = (chkCatXOLAutoReins.CheckState = CheckState.Checked)
        lblCatXOLReinstatements.Enabled = bEnable
        txtCatXOLReinstatements.Enabled = bEnable

        ' Update field validation
        m_oFormFields.Item("txtCatXOLReinstatements-0").IsMandatory = bEnable

    End Sub

    Private Sub chkClmXOL_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkClmXOL.CheckStateChanged

        ' Enable/disable per claim xol
        Dim bEnable As Boolean = (chkClmXOL.CheckState = CheckState.Checked)
        lblClmXOLModel.Enabled = bEnable
        cboClmXOLModel.Enabled = bEnable
        lblClmXOLLimit.Enabled = bEnable
        txtClmXOLLimit.Enabled = bEnable

        ' Update field validation
        m_oFormFields.Item("cboClmXOLModel-0").IsMandatory = bEnable
        m_oFormFields.Item("txtClmXOLLimit-0").IsMandatory = bEnable
    End Sub

    Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click

        Dim oForm As frmRIModelLine

        Dim lPriority As Integer
        Dim lLines As Decimal
        Dim cLineLimit As Decimal
        Dim lTreatyID As Integer
        Dim sTreatyName As String = ""
        Dim dShare As Double
        Dim cLowerLimit As Decimal
        Dim lTreatyTypeID As Integer
        Dim dCedingrate As Double
        Dim lRITypeID As Integer
        Dim iCedePremiumOnly As Integer
        Dim iPremiumCalculationBasis As Integer
        Dim iIsVariableQuotaShare As Integer
        Dim m_vTreatyVariableQuotaShare As Object
        Dim lIndex As Integer
        Dim vIndex As Integer
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "cmdAdd_Click"
        Try

            ' Build priority array
            lReturn = CType(m_oPriorities.Refresh(m_vRIModelLines, , , m_bIsRI2007Enabled), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("BuildPriorities(lIndex, vPriorities)", "Unable to build priority array")
            End If

            ' Create ri model form
            oForm = New frmRIModelLine()
            oForm.Business = Business
            oForm.RIModelLines = VB6.CopyArray(m_vRIModelLines)
            oForm.Task = gPMConstants.PMEComponentAction.PMAdd
            oForm.TreatyPremiumType = m_iTreatyPremiumType
            If m_bIsRI2007Enabled Then
                oForm.RIModelLinesVariableQuotaShare = IIf(m_vRIModelLinesVariableQuotaShare IsNot Nothing, VB6.CopyArray(m_vRIModelLinesVariableQuotaShare), Nothing)
            End If
            'Load(oForm)
            oForm.frmRIModelLineLoad()
            ' Clear form
            lReturn = CType(oForm.Clear(m_oPriorities), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("oForm.Clear", "Unable to set default properties on ri model line dialog")
            End If

            ' Show dialog
            oForm.ShowDialog()
            Dim bIsObligatory, bDoIncrementPriority  As Boolean
            Dim iCalculationOrder As Integer = 0
            Dim iCalculationBase As Integer = 0
            ' Check result
            If oForm.Status = gPMConstants.PMEReturnCode.PMOK Then
                ' Get results
                If m_bIsRI2007Enabled Then
                    lReturn = CType(oForm.GetProperties(lPriority, lLines, cLineLimit, lTreatyID, sTreatyName, dShare, cLowerLimit, lTreatyTypeID, dCedingrate, lRITypeID, bIsObligatory, bDoIncrementPriority, iCedePremiumOnly, iCalculationOrder, iCalculationBase, iIsVariableQuotaShare, iPremiumCalculationBasis:=iPremiumCalculationBasis, m_vTreatyVariableQuotaShare:=m_vTreatyVariableQuotaShare), gPMConstants.PMEReturnCode)
                Else
                    lReturn = CType(oForm.GetProperties(lPriority, lLines, cLineLimit, lTreatyID, sTreatyName, dShare, cLowerLimit, lTreatyTypeID, dCedingrate, lRITypeID, bIsObligatory, bDoIncrementPriority, iCedePremiumOnly, iCalculationOrder, iCalculationBase, iIsVariableQuotaShare, iPremiumCalculationBasis, m_vTreatyVariableQuotaShare:=m_vTreatyVariableQuotaShare), gPMConstants.PMEReturnCode)
                End If

                ' Increase array
                If Information.IsArray(m_vRIModelLines) Then
                    ReDim Preserve m_vRIModelLines(UBound(m_vRIModelLines, 1), UBound(m_vRIModelLines, 2) + 1)
                Else
                    ReDim m_vRIModelLines(DBMLMax, 0)
                End If
                lIndex = m_vRIModelLines.GetUpperBound(1)

                ' Get updated Variable Quota Share array from child form
                If Information.IsArray(m_vRIModelLinesVariableQuotaShare) Then
                    ReDim Preserve m_vRIModelLinesVariableQuotaShare(UBound(m_vRIModelLinesVariableQuotaShare, 1), UBound(m_vRIModelLinesVariableQuotaShare, 2) + 1)
                ElseIf oForm.RIModelLinesVariableQuotaShare IsNot Nothing Then
                    m_vRIModelLinesVariableQuotaShare = oForm.RIModelLinesVariableQuotaShare
                ElseIf Not Information.IsArray(m_vRIModelLinesVariableQuotaShare) Then
                    ReDim m_vRIModelLinesVariableQuotaShare(DBMVMax, 0)
                End If
                vIndex = m_vRIModelLinesVariableQuotaShare.GetUpperBound(1)

                ' Store results
                m_vRIModelLines(MainModule.RIModelLineEnum.DBMLPriority, lIndex) = lPriority
                m_vRIModelLines(MainModule.RIModelLineEnum.DBMLNumberOfLines, lIndex) = lLines
                m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLineLimit, lIndex) = cLineLimit
                m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyID, lIndex) = lTreatyID
                m_vRIModelLines(MainModule.RIModelLineEnum.DBMLDescription, lIndex) = sTreatyName
                m_vRIModelLines(MainModule.RIModelLineEnum.DBMLSharePercent, lIndex) = dShare
                m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLowerLimit, lIndex) = cLowerLimit
                m_vRIModelLines(MainModule.RIModelLineEnum.DBMLCedingrate, lIndex) = dCedingrate
                m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyTypeId, lIndex) = lTreatyTypeID
                m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRITypeID, lIndex) = lRITypeID
                m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRIIsObligatory, lIndex) = bIsObligatory
                ' Update priority values
                m_vRIModelLines(MainModule.RIModelLineEnum.DBMLCedePremiumOnly, lIndex) = iCedePremiumOnly  'E005 Part1
                m_vRIModelLines(MainModule.RIModelLineEnum.DBMLPremiumCalculationBasis, lIndex) = iPremiumCalculationBasis
                m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRIIsVariableQuotaShare, lIndex) = iIsVariableQuotaShare
                '' Copy back updated premium calculation basis for other treaties from form's array
                Dim formArray(,) As Object = oForm.GetUpdatedRIModelLines
                If Information.IsArray(formArray) Then
                    For i As Integer = formArray.GetLowerBound(1) To formArray.GetUpperBound(1)
                        If i <> lIndex And gPMFunctions.ToSafeLong(formArray(MainModule.RIModelLineEnum.DBMLTreatyID, i)) > 0 Then
                            m_vRIModelLines(MainModule.RIModelLineEnum.DBMLPremiumCalculationBasis, i) = formArray(MainModule.RIModelLineEnum.DBMLPremiumCalculationBasis, i)
                        End If
                    Next
                End If
                If m_vTreatyVariableQuotaShare IsNot Nothing Then
                    'for variablequotashare
                    For i As Integer = 0 To m_vTreatyVariableQuotaShare.GetUpperBound(1)
                        m_vRIModelLinesVariableQuotaShare(MainModule.RIModelLineQuotaShareEnum.DBMLVariableQuotaShareId, vIndex) = m_vTreatyVariableQuotaShare(MainModule.RIModelLineQuotaShareEnum.DBMLVariableQuotaShareId, i)
                        m_vRIModelLinesVariableQuotaShare(MainModule.RIModelLineQuotaShareEnum.DBMLSALowerLimit, vIndex) = m_vTreatyVariableQuotaShare(MainModule.RIModelLineQuotaShareEnum.DBMLSALowerLimit, i)
                        m_vRIModelLinesVariableQuotaShare(MainModule.RIModelLineQuotaShareEnum.DBMLSAUpperLimit, vIndex) = m_vTreatyVariableQuotaShare(MainModule.RIModelLineQuotaShareEnum.DBMLSAUpperLimit, i)
                        m_vRIModelLinesVariableQuotaShare(MainModule.RIModelLineQuotaShareEnum.DBMLSharePercent, vIndex) = m_vTreatyVariableQuotaShare(MainModule.RIModelLineQuotaShareEnum.DBMLSharePercent, i)
                        m_vRIModelLinesVariableQuotaShare(MainModule.RIModelLineQuotaShareEnum.DBMLTreatyLimit, vIndex) = m_vTreatyVariableQuotaShare(MainModule.RIModelLineQuotaShareEnum.DBMLTreatyLimit, i)
                        m_vRIModelLinesVariableQuotaShare(MainModule.RIModelLineQuotaShareEnum.DBMLTreatyId, vIndex) = m_vTreatyVariableQuotaShare(MainModule.RIModelLineQuotaShareEnum.DBMLTreatyId, i)
                        m_vRIModelLinesVariableQuotaShare(MainModule.RIModelLineQuotaShareEnum.DBMLRIModelLineID, vIndex) = m_vTreatyVariableQuotaShare(MainModule.RIModelLineQuotaShareEnum.DBMLRIModelLineID, i)
                    Next
                End If
                'If Not m_bIsRI2007Enabled Then
                lReturn = CType(RefreshPriorities(lPriority, lLines, cLineLimit), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("RefreshPriorities(lPriority, lLines, cLineLimit)", "Unable to refresh priority limits")
                    End If
                    'End If
                    If Not bDoIncrementPriority Then
                        For lCount As Integer = m_vRIModelLines.GetLowerBound(1) To m_vRIModelLines.GetUpperBound(1)
                            If gPMFunctions.ToSafeBoolean(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRIIsObligatory, lCount)) Or gPMFunctions.ToSafeInteger(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRIIsObligatory, lCount)) = 1 Then
                                bDoIncrementPriority = True
                                Exit For
                            End If
                        Next
                    End If

                    lReturn = CType(IncrementPriorities(v_bIsObligatory:=bDoIncrementPriority, lIndex), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "Unable to Increment priorities", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    ' Refresh list
                    lReturn = CType(RefreshRILines(lIndex), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("RefreshRILines(lIndex)", "Unable to refresh ri model lines")
                    End If
                End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally
            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)



        End Try
        Exit Sub
    End Sub
    'Note:- This method is used to increment the priorities if necessary
    Private Function IncrementPriorities(ByVal v_bIsObligatory As Boolean, lIndex As Integer) As Integer

        Dim result As Integer = 0
        Dim bObligatoryPriorityOne As Boolean
        Const kMethodName As String = "IncrementPriorities"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'RI 2007 Disable
            If v_bIsObligatory Then
                If Information.IsArray(m_vRIModelLines) Then
                    bObligatoryPriorityOne = True
                    For lCount As Integer = m_vRIModelLines.GetLowerBound(1) To m_vRIModelLines.GetUpperBound(1)
                        If gPMFunctions.ToSafeBoolean(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRIIsObligatory, lCount)) Then
                            If gPMFunctions.ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLPriority, lCount)) = 1 Then
                                For lCountObligatory As Integer = m_vRIModelLines.GetLowerBound(1) To m_vRIModelLines.GetUpperBound(1)
                                    If Not gPMFunctions.ToSafeBoolean(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRIIsObligatory, lCountObligatory)) Then
                                        If gPMFunctions.ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLPriority, lCountObligatory)) = 1 Then
                                            bObligatoryPriorityOne = False
                                        End If
                                    End If
                                Next lCountObligatory
                            End If
                        End If
                    Next lCount
                    If Not bObligatoryPriorityOne Then
                        For lCount As Integer = m_vRIModelLines.GetLowerBound(1) To m_vRIModelLines.GetUpperBound(1)
                            If gPMFunctions.ToSafeBoolean(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRIIsObligatory, lCount)) Then
                                m_vRIModelLines(MainModule.RIModelLineEnum.DBMLPriority, lCount) = 1
                            Else
                                If lCount < lIndex Then
                                    m_vRIModelLines(MainModule.RIModelLineEnum.DBMLPriority, lCount) = CDbl(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLPriority, lCount)) + 1
                                End If

                            End If
                        Next lCount
                    End If
                End If
            End If



        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally

        End Try
        Return result
    End Function

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        ' Check the user wants to close
        If MessageBox.Show("Cancelling will lose all of your current details." &
                           Strings.Chr(13) & Strings.Chr(10) & "Do you really wish to cancel?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Information) = System.Windows.Forms.DialogResult.Yes Then
            ' Set status to cancel and close
            Status = gPMConstants.PMEReturnCode.PMCancel
            Me.Hide()
        End If
    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click

        Dim lIndex As Integer
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "cmdDelete_Click"


        Try

            ' Check valid party
            If IsValidRILine() Then
                ' Get index of selected item
                lIndex = gPMFunctions.ToSafeLong(Convert.ToString(lvwRIModelLine.FocusedItem.Tag), -1)

                ' Check if deleting a Retained treaty while Quota Share Retained treaties exist
                If m_bIsRI2007Enabled AndAlso gPMFunctions.ToSafeInteger(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRITypeID, lIndex)) = ACRetained Then
                    Dim bHasQSR As Boolean = False
                    If Information.IsArray(m_vRIModelLines) Then
                        For lQSR As Integer = m_vRIModelLines.GetLowerBound(1) To m_vRIModelLines.GetUpperBound(1)
                            If gPMFunctions.ToSafeInteger(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRITypeID, lQSR)) = ACQuotaShareRetained AndAlso
                               gPMFunctions.ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyID, lQSR)) > 0 Then
                                bHasQSR = True
                                Exit For
                            End If
                        Next
                    End If
                    If bHasQSR Then
                        If MessageBox.Show("Deleting the Retained treaty will automatically delete the Quota Share Retained Treaties as well", Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Information) <> DialogResult.OK Then
                            Exit Sub
                        End If
                        ' Delete all Quota Share Retained treaties
                        For lQSR As Integer = m_vRIModelLines.GetLowerBound(1) To m_vRIModelLines.GetUpperBound(1)
                            If gPMFunctions.ToSafeInteger(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRITypeID, lQSR)) = ACQuotaShareRetained AndAlso
                               gPMFunctions.ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyID, lQSR)) > 0 Then
                                m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyID, lQSR) = 0
                                m_vRIModelLines(MainModule.RIModelLineEnum.DBMLPriority, lQSR) = 0
                                m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRITypeID, lQSR) = 0
                                m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRIIsObligatory, lQSR) = 0
                            End If
                        Next
                    End If
                End If

                ' We can delete this line, set party_cnt to zero and refresh
                m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyID, lIndex) = 0
                m_vRIModelLines(MainModule.RIModelLineEnum.DBMLPriority, lIndex) = 0

                m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRITypeID, lIndex) = 0

                m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRIIsObligatory, lIndex) = 0

                ' Refresh list
                lReturn = CType(RefreshRILines(lIndex), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("RefreshRILines(lIndex)", "Unable to refresh ri model lines")
                End If
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally


        End Try
        Exit Sub
    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click

        Dim oForm As frmRIModelLine

        Dim vPriorities As Object
        Dim lPriority As Integer
        Dim lLines As Decimal
        Dim cLineLimit As Decimal
        Dim lTreatyID As Integer
        Dim sTreatyName As String = ""
        Dim dShare As Double
        Dim cLowerLimit As Decimal
        Dim lTreatyTypeID As Integer
        Dim dCeding As Double
        Dim dCedingrate As Double
        Dim lRITypeID As Integer
        Dim bIsObligatory As Boolean
        Dim bDoIncrementPriority As Boolean
        Dim iCedePremiumOnly As Integer
        Dim iPremiumCalculationBasis As Integer
        Dim iIsVariableQuotaShare As Integer
        Dim m_vTreatyVariableQuotaShare As Object
        Dim lIndex As Integer
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "cmdEdit_Click"
        Try

            ' Check valid party
            If IsValidRILine() Then
                ' Get index of selected item
                lIndex = gPMFunctions.ToSafeLong(Convert.ToString(lvwRIModelLine.FocusedItem.Tag))

                ' Build priority array
                lReturn = CType(m_oPriorities.Refresh(m_vRIModelLines, lIndex, , m_bIsRI2007Enabled), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("BuildPriorities(lIndex, vPriorities)", "Unable to build priority array")
                End If

                ' Create ri model form
                oForm = New frmRIModelLine()
                oForm.RIModelLines = VB6.CopyArray(m_vRIModelLines)
                oForm.SelectedIndex = lIndex
                oForm.Business = Business
                oForm.Task = gPMConstants.PMEComponentAction.PMEdit
                oForm.TreatyPremiumType = m_iTreatyPremiumType
                If m_bIsRI2007Enabled AndAlso m_vRIModelLinesVariableQuotaShare IsNot Nothing Then
                    oForm.RIModelLinesVariableQuotaShare = VB6.CopyArray(m_vRIModelLinesVariableQuotaShare)
                End If
                'Load(oForm)
                oForm.frmRIModelLineLoad()

                ' Set properties
                If m_bIsRI2007Enabled Then
                    'Note:- Two Parameters are added "bIsObligatory,cLineLimit"
                    Dim vqsConfig As Object = IIf((m_vRIModelLinesVariableQuotaShare IsNot Nothing AndAlso m_vRIModelLinesVariableQuotaShare.Length > 0), m_vRIModelLinesVariableQuotaShare, Nothing)
                    Dim hasVQSConfig As Boolean = vqsConfig IsNot Nothing AndAlso Information.IsArray(vqsConfig) AndAlso DirectCast(vqsConfig, Array).GetLength(1) > 0
                    lReturn = CType(oForm.SetProperties(CInt(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLPriority, lIndex)), CInt(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyID, lIndex)), CDbl(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLSharePercent, lIndex)), m_oPriorities, gPMFunctions.ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyTypeId, lIndex)), cLowerLimit:=m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLowerLimit, lIndex), bIsObligatory:=gPMFunctions.ToSafeBoolean(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRIIsObligatory, lIndex)), cLineLimit:=CDec(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLineLimit, lIndex)), iCedePremiumOnly:=ToSafeInteger(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLCedePremiumOnly, lIndex)), dCedePercentage:=ToSafeDouble(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLCedingrate, lIndex)), iIsVariableQuotaShare:=ToSafeInteger(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRIIsVariableQuotaShare, lIndex)), iPremiumCalculationBasis:=ToSafeInteger(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLPremiumCalculationBasis, lIndex)), m_vTreatyVariableQuotaShare:=m_vRIModelLinesVariableQuotaShare), gPMConstants.PMEReturnCode)
                Else
                    'Note:- One Parameter is added "ToSafeBoolean(m_vRIModelLines(DBMLRIIsObligatory, lIndex))"
                    Dim vqsConfig As Object = IIf((m_vRIModelLinesVariableQuotaShare IsNot Nothing AndAlso m_vRIModelLinesVariableQuotaShare.Length > 0), m_vRIModelLinesVariableQuotaShare, Nothing)
                    Dim hasVQSConfig As Boolean = vqsConfig IsNot Nothing AndAlso Information.IsArray(vqsConfig) AndAlso DirectCast(vqsConfig, Array).GetLength(1) > 0
                    lReturn = CType(oForm.SetProperties(CInt(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLPriority, lIndex)), CInt(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyID, lIndex)), CDbl(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLSharePercent, lIndex)), m_oPriorities, gPMFunctions.ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyTypeId, lIndex)), CDec(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLineLimit, lIndex)), bIsObligatory:=gPMFunctions.ToSafeBoolean(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRIIsObligatory, lIndex)), iIsVariableQuotaShare:=hasVQSConfig, iPremiumCalculationBasis:=ToSafeInteger(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLPremiumCalculationBasis, lIndex)), m_vTreatyVariableQuotaShare:=m_vRIModelLinesVariableQuotaShare), gPMConstants.PMEReturnCode)
                End If

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("oForm.SetProperties", "Unable to set properties on ri model line dialog")
                End If

                If m_bIsRI2007Enabled Then

                    lPriority = CInt(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLPriority, lIndex))
                    'm_vRIModelLines(MainModule.RIModelLineEnum.DBMLPriority, lIndex) = 0

                    lTreatyID = CInt(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyID, lIndex))
                    'm_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyID, lIndex) = 0

                    lTreatyTypeID = CInt(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyTypeId, lIndex))
                    'm_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyTypeId, lIndex) = 0

                    lRITypeID = CInt(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRITypeID, lIndex))
                    'm_vRIModelLines(MainModule.RIModelLineEnum.DBMLRITypeID, lIndex) = 0

                    iCedePremiumOnly = ToSafeInteger(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLCedePremiumOnly, lIndex), 0)  'E005 Part1
                    iIsVariableQuotaShare = ToSafeInteger(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRIIsVariableQuotaShare, lIndex), 0)
                    oForm.RIModelLines = VB6.CopyArray(m_vRIModelLines)
                    If m_vRIModelLinesVariableQuotaShare IsNot Nothing Then
                        oForm.RIModelLinesVariableQuotaShare = VB6.CopyArray(m_vRIModelLinesVariableQuotaShare)
                    End If
                End If

                ' Show dialog
                oForm.ShowDialog()

                ' Check result
                If oForm.Status = gPMConstants.PMEReturnCode.PMOK Then
                    ' Get updated RIModelLines array with VQS config BEFORE calling GetProperties
                    m_vRIModelLinesVariableQuotaShare = oForm.RIModelLinesVariableQuotaShare
                    m_vTreatyVariableQuotaShare = oForm.RIModelLinesVariableQuotaShare

                    ' Get results
                    If m_bIsRI2007Enabled Then

                        lReturn = CType(oForm.GetProperties(lPriority, lLines, cLineLimit, lTreatyID, sTreatyName, dShare, cLowerLimit, lTreatyTypeID, dCeding, lRITypeID, bIsObligatory, bDoIncrementPriority, iCedePremiumOnly:=iCedePremiumOnly, iIsVariableQuotaShare:=iIsVariableQuotaShare, iPremiumCalculationBasis:=iPremiumCalculationBasis, m_vTreatyVariableQuotaShare:=m_vTreatyVariableQuotaShare), gPMConstants.PMEReturnCode)
                    Else

                        lReturn = CType(oForm.GetProperties(lPriority, lLines, cLineLimit, lTreatyID, sTreatyName, dShare, bIsObligatory:=bIsObligatory, bDoIncrementPriority:=bDoIncrementPriority, lReinsuranceTypeID:=lRITypeID, iIsVariableQuotaShare:=iIsVariableQuotaShare, iPremiumCalculationBasis:=iPremiumCalculationBasis, m_vTreatyVariableQuotaShare:=m_vTreatyVariableQuotaShare), gPMConstants.PMEReturnCode)
                    End If

                    ' Get updated RIModelLines array with VQS config
                    m_vRIModelLinesVariableQuotaShare = oForm.RIModelLinesVariableQuotaShare
                    ' Store results

                    m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRIIsObligatory, lIndex) = bIsObligatory

                    m_vRIModelLines(MainModule.RIModelLineEnum.DBMLPriority, lIndex) = lPriority
                    m_vRIModelLines(MainModule.RIModelLineEnum.DBMLNumberOfLines, lIndex) = lLines
                    m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLineLimit, lIndex) = cLineLimit
                    m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyID, lIndex) = lTreatyID
                    m_vRIModelLines(MainModule.RIModelLineEnum.DBMLDescription, lIndex) = sTreatyName
                    m_vRIModelLines(MainModule.RIModelLineEnum.DBMLSharePercent, lIndex) = dShare
                    m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLowerLimit, lIndex) = cLowerLimit
                    m_vRIModelLines(MainModule.RIModelLineEnum.DBMLCedingrate, lIndex) = dCeding
                    m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyTypeId, lIndex) = lTreatyTypeID
                    m_vRIModelLines(MainModule.RIModelLineEnum.DBMLCedePremiumOnly, lIndex) = iCedePremiumOnly
                    m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRITypeID, lIndex) = lRITypeID
                    m_vRIModelLines(MainModule.RIModelLineEnum.DBMLPremiumCalculationBasis, lIndex) = iPremiumCalculationBasis
                    m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRIIsVariableQuotaShare, lIndex) = iIsVariableQuotaShare
                    '' Copy back updated premium calculation basis for other treaties from form's array
                    Dim formArray(,) As Object = oForm.GetUpdatedRIModelLines
                    If Information.IsArray(formArray) Then
                        For i As Integer = formArray.GetLowerBound(1) To formArray.GetUpperBound(1)
                            If i <> lIndex And gPMFunctions.ToSafeLong(formArray(MainModule.RIModelLineEnum.DBMLTreatyID, i)) > 0 Then
                                m_vRIModelLines(MainModule.RIModelLineEnum.DBMLPremiumCalculationBasis, i) = formArray(MainModule.RIModelLineEnum.DBMLPremiumCalculationBasis, i)
                            End If
                        Next
                    End If

                    'for variablequotashare
                    If m_vTreatyVariableQuotaShare IsNot Nothing AndAlso Information.IsArray(m_vTreatyVariableQuotaShare) Then
                        For i As Integer = 0 To m_vTreatyVariableQuotaShare.GetUpperBound(1)
                            m_vRIModelLinesVariableQuotaShare(MainModule.RIModelLineQuotaShareEnum.DBMLVariableQuotaShareId, i) = m_vTreatyVariableQuotaShare(MainModule.RIModelLineQuotaShareEnum.DBMLVariableQuotaShareId, i)
                            m_vRIModelLinesVariableQuotaShare(MainModule.RIModelLineQuotaShareEnum.DBMLSALowerLimit, i) = m_vTreatyVariableQuotaShare(MainModule.RIModelLineQuotaShareEnum.DBMLSALowerLimit, i)
                            m_vRIModelLinesVariableQuotaShare(MainModule.RIModelLineQuotaShareEnum.DBMLSAUpperLimit, i) = m_vTreatyVariableQuotaShare(MainModule.RIModelLineQuotaShareEnum.DBMLSAUpperLimit, i)
                            m_vRIModelLinesVariableQuotaShare(MainModule.RIModelLineQuotaShareEnum.DBMLSharePercent, i) = m_vTreatyVariableQuotaShare(MainModule.RIModelLineQuotaShareEnum.DBMLSharePercent, i)
                            m_vRIModelLinesVariableQuotaShare(MainModule.RIModelLineQuotaShareEnum.DBMLTreatyLimit, i) = m_vTreatyVariableQuotaShare(MainModule.RIModelLineQuotaShareEnum.DBMLTreatyLimit, i)
                            m_vRIModelLinesVariableQuotaShare(MainModule.RIModelLineQuotaShareEnum.DBMLTreatyId, i) = m_vTreatyVariableQuotaShare(MainModule.RIModelLineQuotaShareEnum.DBMLTreatyId, i)
                            m_vRIModelLinesVariableQuotaShare(MainModule.RIModelLineQuotaShareEnum.DBMLRIModelLineID, i) = m_vTreatyVariableQuotaShare(MainModule.RIModelLineQuotaShareEnum.DBMLRIModelLineID, i)
                        Next
                    End If
                    ' When Retained treaty is edited, update all Quota Share Retained treaties' Line/Upper Limit
                    If m_bIsRI2007Enabled AndAlso lRITypeID = ACRetained Then
                        Dim bHasQSREdit As Boolean = False
                        If Information.IsArray(m_vRIModelLines) Then
                            For lQSR As Integer = m_vRIModelLines.GetLowerBound(1) To m_vRIModelLines.GetUpperBound(1)
                                If gPMFunctions.ToSafeInteger(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRITypeID, lQSR)) = ACQuotaShareRetained AndAlso
                                   gPMFunctions.ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyID, lQSR)) > 0 Then
                                    bHasQSREdit = True
                                    Exit For
                                End If
                            Next
                        End If
                        If bHasQSREdit Then
                            If MessageBox.Show("Any changes done to the Treaty Limit will also change the Line/Upper Limit of the Quota Share Retained Treaties", Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Information) = DialogResult.OK Then
                                Dim dNewRetainedTreatyLimit As Decimal = lLines * cLineLimit * CDec(dShare / 100)
                                For lQSR As Integer = m_vRIModelLines.GetLowerBound(1) To m_vRIModelLines.GetUpperBound(1)
                                    If gPMFunctions.ToSafeInteger(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRITypeID, lQSR)) = ACQuotaShareRetained AndAlso
                                       gPMFunctions.ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyID, lQSR)) > 0 Then
                                        m_vRIModelLines(MainModule.RIModelLineEnum.DBMLLineLimit, lQSR) = dNewRetainedTreatyLimit
                                    End If
                                Next
                            Else
                                ' User cancelled - revert the Retained treaty changes
                                m_vRIModelLines(MainModule.RIModelLineEnum.DBMLPriority, lIndex) = lPriority
                                m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyID, lIndex) = lTreatyID
                                m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyTypeId, lIndex) = lTreatyTypeID
                                m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRITypeID, lIndex) = lRITypeID
                                lReturn = CType(RefreshRILines(lIndex), gPMConstants.PMEReturnCode)
                                Exit Sub
                            End If
                        End If
                    End If

                    ' Update priority values
                    If Not m_bIsRI2007Enabled Then
                            lReturn = CType(RefreshPriorities(lPriority, lLines, cLineLimit), gPMConstants.PMEReturnCode)
                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError("RefreshPriorities(lPriority, lLines, cLineLimit)", "Unable to refresh priority limits")
                            End If
                        End If

                        If Not bDoIncrementPriority Then
                            For lCount As Integer = m_vRIModelLines.GetLowerBound(1) To m_vRIModelLines.GetUpperBound(1)
                                If gPMFunctions.ToSafeBoolean(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRIIsObligatory, lCount)) Or gPMFunctions.ToSafeInteger(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRIIsObligatory, lCount)) = 1 Then
                                    bDoIncrementPriority = True
                                    Exit For
                                End If
                            Next
                        End If

                        lReturn = CType(IncrementPriorities(v_bIsObligatory:=bDoIncrementPriority, lIndex), gPMConstants.PMEReturnCode)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(" IncrimentPriorities", "Unable to Incriment priorities", gPMConstants.PMELogLevel.PMLogError)
                        End If

                        ' Refresh list
                        lReturn = CType(RefreshRILines(lIndex), gPMConstants.PMEReturnCode)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("RefreshRILines(lIndex)", "Unable to refresh treaty party list")
                        End If

                    ElseIf oForm.Status = gPMConstants.PMEReturnCode.PMCancel Then
                        m_vRIModelLines(MainModule.RIModelLineEnum.DBMLPriority, lIndex) = lPriority
                    m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyID, lIndex) = lTreatyID
                    m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyTypeId, lIndex) = lTreatyTypeID
                    m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRITypeID, lIndex) = lRITypeID

                End If
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally
            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)



        End Try
        Exit Sub
    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Dim dShare As Double
        Dim lReturn As Integer
        Const kMethodName As String = "cmdOK_Click"

        Try

            ' Validate data
            If Validate_Renamed() = gPMConstants.PMEReturnCode.PMTrue Then
                ' Set status to OK and close
                Status = gPMConstants.PMEReturnCode.PMOK
                Me.Hide()
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally


        End Try
        Exit Sub
    End Sub

    Private Sub dtpEffectiveDate_ValueChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles dtpEffectiveDate.ValueChanged
        ' Refresh the replaces list based on the date
        'RefreshXOLModelList()
    End Sub

    Private Sub frmRIModel_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        If eventArgs.KeyCode = Keys.Tab Then
            If Shift And ShiftConstants.CtrlMask Then
                If Shift And ShiftConstants.ShiftMask Then
                    iPMFunc.SSTabMovePrevious(tabRIModel)
                Else
                    iPMFunc.SSTabMoveNext(tabRIModel)
                End If
            End If
        End If
    End Sub
    Public Sub LoadForm()

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vValue As String = ""
        Const kMethodName As String = "Form_Load"

        Try
            cboClmXOLModel.FirstItem = ""
            cboCatXOLModel.FirstItem = ""
            dtpExpiryDate.Checked = False
            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            m_lReturn = iPMFunc.GetSystemOption(5005, m_sUnderwritingType)

            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableRI2007, v_vBranch:=g_iSourceID, r_vUnderwriting:=vValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("Form Load", "getProductOptionValue Failed for option " & gPMConstants.SIRHiddenOptions.SIROPTEnableRI2007, gPMConstants.PMELogLevel.PMLogError)
            End If

            m_bIsRI2007Enabled = vValue = "1"

            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableRIRegeneration, v_vBranch:=g_iSourceID, r_vUnderwriting:=vValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("Form Load", "getProductOptionValue Failed for option " & gPMConstants.SIRHiddenOptions.SIROPTEnableRIRegeneration, gPMConstants.PMELogLevel.PMLogError)
            End If
            If vValue = "1" Then
                m_bIsRIRegenerationEnabled = True
            Else
                m_bIsRIRegenerationEnabled = False
            End If

            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTMultipleRetainedTreaty, v_vBranch:=g_iSourceID, r_vUnderwriting:=vValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("Form Load", "getProductOptionValue Failed for option " & gPMConstants.SIRHiddenOptions.SIROPTMultipleRetainedTreaty, gPMConstants.PMELogLevel.PMLogError)
            End If

            m_bIsMultipleRetainedTreaty = vValue = "1"

            m_lReturn = SetRI2007InterfaceDefaults()

            ' Create priority cache
            m_oPriorities = New PriorityCollection()

            ' Validate fields using Forms Control
            lReturn = CType(SetFieldValidation(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("SetFieldValidation", "Unable to set field validation")
            End If

            If m_sUnderwritingType = "1" Then
                lvwRIModelLine.Columns.Item(4 + m_iRIConst - 1).Text = "Binding Authority"
                lvwRIModelLine.Columns.Item(6 + m_iRIConst - 1).Text = "Binding Authority Limit"
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally
            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)



        End Try
    End Sub

    Private Sub frmRIModel_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        Dim lReturn As Integer
        Const kMethodName As String = "Form_QueryUnload"


        Try

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            If UnloadMode <> vbFormCode Then
                ' Check the user wants to close
                If MessageBox.Show("Cancelling will lose all of your current details." &
                                   Strings.Chr(13) & Strings.Chr(10) & "Do you really wish to cancel?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = System.Windows.Forms.DialogResult.No Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    'Developer Guide No. 7
                    eventArgs.Cancel = True
                Else
                    Status = gPMConstants.PMEReturnCode.PMCancel
                End If
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally


            eventArgs.Cancel = Cancel <> 0
        End Try
        Exit Sub
    End Sub

    Private Sub lvwRIModelLine_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwRIModelLine.ColumnClick

        Dim ColumnHeader As ColumnHeader = lvwRIModelLine.Columns(eventArgs.Column)
        StoreHScrollValue()
        SortList(ColumnHeader.Index)
        'ListViewFunc.SortListView(lvwRIModelLine, eventArgs)
        RecoverHorizontalScroll()
    End Sub

    Private Sub lvwRIModelLine_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwRIModelLine.DoubleClick
        cmdEdit_Click(cmdEdit, New EventArgs())
    End Sub

    Private isInitializingComponent As Boolean
    Private Sub optRIModelType_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _optRIModelType_0.CheckedChanged, _optRIModelType_1.CheckedChanged, _optRIModelType_2.CheckedChanged, _optRIModelType_3.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            Dim Index As Integer = Array.IndexOf(optRIModelType, eventSender)
            ' Set fax options
            optFACPremiums(0).Enabled = (Index <> 3)
            optFACPremiums(1).Enabled = (Index <> 3)

            ' Set claim options
            optClaimAllocation(0).Enabled = (Index <> 3)
            optClaimAllocation(1).Enabled = (Index <> 3)
            optClaimAllocation(2).Enabled = (Index <> 3)
            If Index = 3 Then
                optClaimAllocation(2).Checked = 2
            End If
        End If
    End Sub

    Private Sub tabRIModel_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabRIModel.SelectedIndexChanged

        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "tabRIModel_Click"

        Try

            ' Update summary control if we are moving to that tab

            If dtpExpiryDate.Checked Then

                lReturn = uctSummary.UpdateRIModel(txtDescription.Text.Trim(), dtpEffectiveDate.Value, dtpExpiryDate.Value.ToOADate(), iPMFunc.GetOptionValue(optClaimAllocation), cboCurrency.CurrencyName, IIf(chkClmXOL.CheckState = CheckState.Checked, cboClmXOLModel.ItemId, 0), IIf(chkClmXOL.CheckState = CheckState.Checked, CDec(m_oFormFields.UnformatControl(txtClmXOLLimit)), 0), IIf(chkCatXOL.CheckState = CheckState.Checked, cboCatXOLModel.ItemId, 0), IIf(chkCatXOL.CheckState = CheckState.Checked, CDec(m_oFormFields.UnformatControl(txtCatXOLLimit)), 0), IIf(chkCatXOL.CheckState = CheckState.Checked And chkCatXOLAutoReins.CheckState = CheckState.Checked, CInt(m_oFormFields.UnformatControl(txtCatXOLReinstatements)), 0), m_vRIModelLines)
            Else
                lReturn = uctSummary.UpdateRIModel(txtDescription.Text.Trim(), dtpEffectiveDate.Value, Nothing, iPMFunc.GetOptionValue(optClaimAllocation), cboCurrency.CurrencyName, IIf(chkClmXOL.CheckState = CheckState.Checked, cboClmXOLModel.ItemId, 0), IIf(chkClmXOL.CheckState = CheckState.Checked, CDec(m_oFormFields.UnformatControl(txtClmXOLLimit)), 0), IIf(chkCatXOL.CheckState = CheckState.Checked, cboCatXOLModel.ItemId, 0), IIf(chkCatXOL.CheckState = CheckState.Checked, CDec(m_oFormFields.UnformatControl(txtCatXOLLimit)), 0), IIf(chkCatXOL.CheckState = CheckState.Checked And chkCatXOLAutoReins.CheckState = CheckState.Checked, CInt(m_oFormFields.UnformatControl(txtCatXOLReinstatements)), 0), m_vRIModelLines)
            End If
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("uctSummary.UpdateRIModel", "Unable to update reinsurance model summary")
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally


            tabRIModelPreviousTab = tabRIModel.SelectedIndex
        End Try
        Exit Sub
    End Sub

    Private Sub txtCatXOLLimit_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCatXOLLimit.Enter
        m_oFormFields.GotFocus(txtCatXOLLimit)
    End Sub

    Private Sub txtCatXOLLimit_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCatXOLLimit.Leave
        m_oFormFields.LostFocus(txtCatXOLLimit)
    End Sub

    Private Sub txtCatXOLReinstatements_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCatXOLReinstatements.Enter
        m_oFormFields.GotFocus(txtCatXOLReinstatements)
    End Sub

    Private Sub txtCatXOLReinstatements_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCatXOLReinstatements.Leave
        m_oFormFields.LostFocus(txtCatXOLReinstatements)
    End Sub

    Private Sub txtClmXOLLimit_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtClmXOLLimit.Enter
        m_oFormFields.GotFocus(txtClmXOLLimit)
    End Sub

    Private Sub txtClmXOLLimit_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtClmXOLLimit.Leave
        m_oFormFields.LostFocus(txtClmXOLLimit)
    End Sub

    Private Function SetRI2007InterfaceDefaults() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_bIsRI2007Enabled Then
                SSTabHelper.SetTabCaption(tabRIModel, 1, "Treaty")
                SSTabHelper.SetTabCaption(tabRIModel, 2, "Catastrophe")
                optRIModelType(3).Visible = False
                optRIModelType(1).Visible = False
                optRIModelType(2).Left = optRIModelType(1).Left
                chkClmXOL.Visible = False
                fraClmXOL.Visible = False
                fraCatXOL.Top = fraClmXOL.Top
                chkCatXOL.Top = chkClmXOL.Top

                lvwRIModelLine.Columns.Insert(0, "", "Priority", CInt(VB6.TwipsToPixelsX(1500)), HorizontalAlignment.Left, -1)
                lvwRIModelLine.Columns.Insert(1, "", "Lines", CInt(VB6.TwipsToPixelsX(1500)), HorizontalAlignment.Left, -1)
                lvwRIModelLine.Columns.Insert(2, "", "Lower Limit", CInt(VB6.TwipsToPixelsX(2400)), HorizontalAlignment.Right, -1)
                lvwRIModelLine.Columns.Insert(3, "", "Line/Upper Limit", CInt(VB6.TwipsToPixelsX(2400)), HorizontalAlignment.Right, -1)
                lvwRIModelLine.Columns.Insert(4, "", "Treaty", CInt(VB6.TwipsToPixelsX(3000)), HorizontalAlignment.Left, -1)
                lvwRIModelLine.Columns.Insert(5, "", "Share %", CInt(VB6.TwipsToPixelsX(1500)), HorizontalAlignment.Left, -1)
                lvwRIModelLine.Columns.Insert(6, "", "Treaty Limit", CInt(VB6.TwipsToPixelsX(1800)), HorizontalAlignment.Right, -1)
                m_iRIConst = 1
                SSTabHelper.SetTabVisible(tabRIModel, 4, True)
                lvwHistory.Columns.Insert(0, "", "Date Amended", CInt(VB6.TwipsToPixelsX(2500)), HorizontalAlignment.Left, -1)
                lvwHistory.Columns.Insert(1, "", "User Name", CInt(VB6.TwipsToPixelsX(1800)), HorizontalAlignment.Left, -1)

                optRIModelType(4).Left = optRIModelType(2).Left
                If m_bIsRIRegenerationEnabled = True Then
                    optRIModelType(4).BringToFront()
                Else
                    optRIModelType(2).BringToFront()
                End If
                cmdConverionRates.Visible = True
                lblTreatyPremium.Visible = True
                Picture4.Visible = True
                optTreatyPremium(0).Visible = True
                optTreatyPremium(1).Visible = True

            Else
                ' Hide Treaty Premium Type controls when RI2007 is disabled
                lblTreatyPremium.Visible = False
                Picture4.Visible = False
                optTreatyPremium(0).Visible = False
                optTreatyPremium(1).Visible = False

                lvwRIModelLine.Columns.Insert(0, "", "Priority", CInt(VB6.TwipsToPixelsX(1500)), HorizontalAlignment.Left, -1)
                lvwRIModelLine.Columns.Insert(1, "", "Lines", CInt(VB6.TwipsToPixelsX(1500)), HorizontalAlignment.Left, -1)
                lvwRIModelLine.Columns.Insert(2, "", "Line Limit", CInt(VB6.TwipsToPixelsX(2400)), HorizontalAlignment.Right, -1)
                lvwRIModelLine.Columns.Insert(3, "", "Treaty", CInt(VB6.TwipsToPixelsX(3000)), HorizontalAlignment.Left, -1)
                lvwRIModelLine.Columns.Insert(4, "", "Share %", CInt(VB6.TwipsToPixelsX(1500)), HorizontalAlignment.Left, -1)
                lvwRIModelLine.Columns.Insert(5, "", "Treaty Limit", CInt(VB6.TwipsToPixelsX(1800)), HorizontalAlignment.Right, -1)
                m_iRIConst = 0
                SSTabHelper.SetTabVisible(tabRIModel, 4, False)
                cmdConverionRates.Visible = False
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set RI 2007 interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetRI2007InterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public WriteOnly Property RIModels() As Object(,)
        Set(ByVal Value As Object(,))
            m_vRIModels = Value
        End Set
    End Property
    Private Function SetRIModelAuditTrail() As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oListItem As ListViewItem

        Const kMethodName As String = "SetRIModelAuditTrail"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            lReturn = Business.GetRIModelAuditTrail(v_lRIModelID:=m_lRIModelID, v_vRIModelAuditTrailArray:=m_vRIModelAuditTrailArray)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oBusiness.GetRIModelAuditTrail", "Unable to retrieve Ri Model Audit Info.")
            End If

            'Add Audit details
            If Information.IsArray(m_vRIModelAuditTrailArray) Then
                For iCount As Integer = 0 To m_vRIModelAuditTrailArray.GetUpperBound(1)
                    oListItem = lvwHistory.Items.Add("Al" & iCount, CStr(m_vRIModelAuditTrailArray(0, iCount)), "")
                    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vRIModelAuditTrailArray(1, iCount))
                Next
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally

        End Try
        Return result
    End Function


    Private Sub dtpEffectiveDate_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtpEffectiveDate.Leave
        RefreshXOLModelList()
    End Sub

    Private Sub lvwRIModelLine_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwRIModelLine.SelectedIndexChanged
        Dim lReturn As Integer
        Const kMethodName As String = "lvwRIModelLine_ItemClick"


        Try
            If lvwRIModelLine.SelectedItems.Count < 1 Then
                Exit Sub
            End If
            ' Set enabled states
            cmdDelete.Enabled = (lvwRIModelLine.SelectedItems(0).Name <> "TOTAL")
            cmdEdit.Enabled = (lvwRIModelLine.SelectedItems(0).Name <> "TOTAL")


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally


        End Try
        Exit Sub
    End Sub

    Private Sub cmdConverionRates_Click(sender As Object, e As EventArgs) Handles cmdConverionRates.Click
        Dim oForm As frmRIConversionRates
        oForm = New frmRIConversionRates()
        GetRIModelCurrencyRates()
        oForm.RIModelCurrRates = VB6.CopyArray(m_vRIModelCurrRates)
        oForm.RIModelID = m_lRIModelID
        oForm.UniqueId = m_sUniqueId
        oForm.ScreenHierarchy = $"RIModel({txtCode.Text.Trim()})"
        oForm.ModelCurrencyDescription = cboCurrency.CurrencyName
        oForm.Business = Business

        oForm.GetRIModelCurrencyRates()
        oForm.ShowDialog(Me)
    End Sub
    Private Function ValidateTreatyDependencies() As Integer
        Dim result As Integer = gPMConstants.PMEReturnCode.PMTrue

        If Information.IsArray(m_vRIModelLines) Then
            ' Collect all RI type -> calc basis mappings (one per RI type, since same type must have same basis)
            Dim existingCalculations As New Dictionary(Of Integer, Integer)

            For lCount As Integer = m_vRIModelLines.GetLowerBound(1) To m_vRIModelLines.GetUpperBound(1)
                If gPMFunctions.ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyID, lCount)) > 0 Then
                    Dim riType As Integer = gPMFunctions.ToSafeInteger(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRITypeID, lCount))
                    Dim calcBasis As Integer = gPMFunctions.ToSafeInteger(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLPremiumCalculationBasis, lCount))

                    If calcBasis > 0 Then
                        ' Check for missing required reinsurance types
                        Dim missingTypes As String = GetMissingRequiredReinsuranceTypes(calcBasis)
                        If missingTypes <> "" Then
                            MessageBox.Show("The premium calculation for some treaties is dependent on other treaties. Please choose an alternative premium calculation basis", "Premium calculation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        ' Build dictionary for circular dependency check (one entry per RI type)
                        If riType > 0 And Not existingCalculations.ContainsKey(riType) Then
                            existingCalculations.Add(riType, calcBasis)
                        End If
                    End If
                End If
            Next
            ' Check for circular dependencies (including transitive 3-way cycles)
            If DetectCircularDependency(existingCalculations) Then
                MessageBox.Show("The premium calculation for some treaties is dependent on other treaties. Please choose an alternative premium calculation basis", "Premium calculation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        Return result
    End Function

    Private Function GetMissingRequiredReinsuranceTypes(ByVal iPremiumCalculationBasis As Integer) As String
        Dim missingTypes As New List(Of String)

        Select Case iPremiumCalculationBasis
            Case PremiumCalculationBasisEnum.PROPRETND
                If Not HasReinsuranceTypeInModel(ACRetained) Then missingTypes.Add("Retained")
            Case PremiumCalculationBasisEnum.PRGRFACCAT
                If Not HasReinsuranceTypeInModel(ACCat) Then missingTypes.Add("CAT")
            Case PremiumCalculationBasisEnum.PRGRFACXOL
                If Not HasXOLTypeInModel() Then missingTypes.Add("XOL")
            Case PremiumCalculationBasisEnum.PRGFACXCAT
                If Not HasXOLTypeInModel() Then missingTypes.Add("XOL")
                If Not HasReinsuranceTypeInModel(ACCat) Then missingTypes.Add("CAT")
            Case PremiumCalculationBasisEnum.XOLFACPRI
                If Not HasPropTypeInModel() Then missingTypes.Add("PROP")
            Case PremiumCalculationBasisEnum.XOLPRICAT
                If Not HasPropTypeInModel() Then missingTypes.Add("PROP")
                If Not HasReinsuranceTypeInModel(ACCat) Then missingTypes.Add("CAT")
            Case PremiumCalculationBasisEnum.XOLFACCAT
                If Not HasReinsuranceTypeInModel(ACCat) Then missingTypes.Add("CAT")
            Case PremiumCalculationBasisEnum.CATFACPRI
                If Not HasPropTypeInModel() Then missingTypes.Add("PROP")
            Case PremiumCalculationBasisEnum.CATPRIXOL
                If Not HasPropTypeInModel() Then missingTypes.Add("PROP")
                If Not HasReinsuranceTypeInModel(ACExcessofLoss) Then missingTypes.Add("XOL")
            Case PremiumCalculationBasisEnum.CATFACXOL
                If Not HasReinsuranceTypeInModel(ACExcessofLoss) Then missingTypes.Add("XOL")
            Case PremiumCalculationBasisEnum.PROPNTXOPX
                If Not HasXOLTypeInModel() Then missingTypes.Add("XOL")
                If Not HasReinsuranceTypeInModel(ACProportionalXOL) Then missingTypes.Add("PX")
            Case PremiumCalculationBasisEnum.PROPNTXPC
                If Not HasXOLTypeInModel() Then missingTypes.Add("XOL")
                If Not HasReinsuranceTypeInModel(ACProportionalXOL) Then missingTypes.Add("PX")
                If Not HasReinsuranceTypeInModel(ACCat) Then missingTypes.Add("CAT")
            Case PremiumCalculationBasisEnum.CATNTPRXO
                If Not HasPropTypeInModel() Then missingTypes.Add("PROP")
                If Not HasReinsuranceTypeInModel(ACExcessofLoss) Then missingTypes.Add("XOL")
                If Not HasReinsuranceTypeInModel(ACProportionalXOL) Then missingTypes.Add("PX")
            Case PremiumCalculationBasisEnum.CATNTXOPX
                If Not HasXOLTypeInModel() Then missingTypes.Add("XOL")
                If Not HasReinsuranceTypeInModel(ACProportionalXOL) Then missingTypes.Add("PX")
            Case PremiumCalculationBasisEnum.PXFACPRP
                If Not HasPropTypeInModel() Then missingTypes.Add("PROP")
            Case PremiumCalculationBasisEnum.PXFPRPXOL
                If Not HasPropTypeInModel() Then missingTypes.Add("PROP")
                If Not HasXOLTypeInModel() Then missingTypes.Add("XOL")
            Case PremiumCalculationBasisEnum.PXFPRPCAT
                If Not HasPropTypeInModel() Then missingTypes.Add("PROP")
                If Not HasReinsuranceTypeInModel(ACCat) Then missingTypes.Add("CAT")
            Case PremiumCalculationBasisEnum.PXFPRCATXL
                If Not HasPropTypeInModel() Then missingTypes.Add("PROP")
                If Not HasReinsuranceTypeInModel(ACCat) Then missingTypes.Add("CAT")
                If Not HasXOLTypeInModel() Then missingTypes.Add("XOL")
            Case PremiumCalculationBasisEnum.PXFACCAT
                If Not HasReinsuranceTypeInModel(ACCat) Then missingTypes.Add("CAT")
            Case PremiumCalculationBasisEnum.PXFACCATXL
                If Not HasReinsuranceTypeInModel(ACCat) Then missingTypes.Add("CAT")
                If Not HasXOLTypeInModel() Then missingTypes.Add("XOL")
            Case PremiumCalculationBasisEnum.T9PRGRFCCT
                If Not HasReinsuranceTypeInModel(ACCat) Then missingTypes.Add("CAT")
            Case PremiumCalculationBasisEnum.T9PRGRFCXL
                If Not HasXOLTypeInModel() Then missingTypes.Add("XOL")
            Case PremiumCalculationBasisEnum.T9PRGFXCAT
                If Not HasXOLTypeInModel() Then missingTypes.Add("XOL")
                If Not HasReinsuranceTypeInModel(ACCat) Then missingTypes.Add("CAT")
            Case PremiumCalculationBasisEnum.T9PRNTXOPX
                If Not HasXOLTypeInModel() Then missingTypes.Add("XOL")
                If Not HasReinsuranceTypeInModel(ACProportionalXOL) Then missingTypes.Add("PX")
            Case PremiumCalculationBasisEnum.T9PRNTXPCT
                If Not HasXOLTypeInModel() Then missingTypes.Add("XOL")
                If Not HasReinsuranceTypeInModel(ACProportionalXOL) Then missingTypes.Add("PX")
                If Not HasReinsuranceTypeInModel(ACCat) Then missingTypes.Add("CAT")
            Case PremiumCalculationBasisEnum.T9PRGRFCPR
                If Not HasPropTypeInModel() Then missingTypes.Add("PROP")
                If Not HasReinsuranceTypeInModel(ACRetained) Then missingTypes.Add("Retained")
            Case PremiumCalculationBasisEnum.T9PRFCPRXL
                If Not HasPropTypeInModel() Then missingTypes.Add("PROP")
                If Not HasXOLTypeInModel() Then missingTypes.Add("XOL")
            Case PremiumCalculationBasisEnum.T9PRFCPRCT
                If Not HasPropTypeInModel() Then missingTypes.Add("PROP")
                If Not HasXOLTypeInModel() Then missingTypes.Add("XOL")
                If Not HasReinsuranceTypeInModel(ACCat) Then missingTypes.Add("CAT")
            Case PremiumCalculationBasisEnum.T9PRFCPRPX
                If Not HasPropTypeInModel() Then missingTypes.Add("PROP")
                If Not HasXOLTypeInModel() Then missingTypes.Add("XOL")
                If Not HasReinsuranceTypeInModel(ACCat) Then missingTypes.Add("CAT")
                If Not HasReinsuranceTypeInModel(ACProportionalXOL) Then missingTypes.Add("PX")
        End Select

        Return String.Join(", ", missingTypes)
    End Function

    Private Function HasReinsuranceTypeInModel(ByVal iReinsuranceTypeID As Integer) As Boolean
        If Information.IsArray(m_vRIModelLines) Then
            For lCount As Integer = m_vRIModelLines.GetLowerBound(1) To m_vRIModelLines.GetUpperBound(1)
                If gPMFunctions.ToSafeInteger(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRITypeID, lCount)) = iReinsuranceTypeID Then
                    If gPMFunctions.ToSafeLong(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLTreatyID, lCount)) > 0 AndAlso
                       Not gPMFunctions.ToSafeBoolean(m_vRIModelLines(MainModule.RIModelLineEnum.DBMLRIIsObligatory, lCount)) Then
                        Return True
                    End If
                End If
            Next
        End If
        Return False
    End Function

    ''' <summary>
    ''' Checks if any proportional type treaty (QS, Surplus, SecondSurplus, ThirdSurplus, Retained) exists in the model
    ''' </summary>
    Private Function HasPropTypeInModel() As Boolean
        Return HasReinsuranceTypeInModel(ACQuotaShare) OrElse
               HasReinsuranceTypeInModel(ACSurplus) OrElse
               HasReinsuranceTypeInModel(ACSecondSurplus) OrElse
               HasReinsuranceTypeInModel(ACThirdSurplus) OrElse
               HasReinsuranceTypeInModel(ACQuotaShareRetained)
        'HasReinsuranceTypeInModel(ACRetained) OrElse
    End Function

    ''' <summary>
    ''' Checks if any XOL type treaty (ExcessOfLoss) exists in the model.
    ''' Proportional XOL produces factor PX, not TX — must not be treated as XOL.
    ''' </summary>
    Private Function HasXOLTypeInModel() As Boolean
        Return HasReinsuranceTypeInModel(ACExcessofLoss)
    End Function
    Private Function DetectCircularDependency(ByVal calculations As Dictionary(Of Integer, Integer)) As Boolean
        ' Skip circular dependency check for calculation basis values with no dependencies
        Dim noDependencyValues As Integer() = {
            PremiumCalculationBasisEnum.PROPGRSS,
            PremiumCalculationBasisEnum.PROPGRFAC,
            PremiumCalculationBasisEnum.PROPRETND,
            PremiumCalculationBasisEnum.XOLRATEGRO,
            PremiumCalculationBasisEnum.XOLGRSFAC,
            PremiumCalculationBasisEnum.CATRATEGRO,
            PremiumCalculationBasisEnum.CATGRSFAC,
            PremiumCalculationBasisEnum.PXGRS,
            PremiumCalculationBasisEnum.PXGRSFAC,
            PremiumCalculationBasisEnum.T9PROPGRSS,
            PremiumCalculationBasisEnum.T9PROPGRFC,
            PremiumCalculationBasisEnum.T9PRGRFCPR
        }

        ' Filter out calculations that have no dependencies
        Dim filteredCalculations As New Dictionary(Of Integer, Integer)
        For Each kvp As KeyValuePair(Of Integer, Integer) In calculations
            If Not noDependencyValues.Contains(kvp.Value) Then
                filteredCalculations.Add(kvp.Key, kvp.Value)
            End If
        Next

        ' If no calculations with dependencies remain, no circular dependency possible
        If filteredCalculations.Count = 0 Then
            Return False
        End If

        ' Build a dependency graph: RI type category -> set of RI type categories it depends on
        ' Categories: "PROP" (QS/Surplus types), "XOL" (ExcessOfLoss/ProportionalXOL), "CAT"
        Dim dependencyGraph As New Dictionary(Of String, HashSet(Of String))

        For Each kvp As KeyValuePair(Of Integer, Integer) In filteredCalculations
            Dim sourceCategory As String = GetRITypeCategory(kvp.Key)
            Dim dependsOn As List(Of String) = GetDependenciesFromCalcBasis(kvp.Value)

            If sourceCategory <> "" AndAlso dependsOn.Count > 0 Then
                If Not dependencyGraph.ContainsKey(sourceCategory) Then
                    dependencyGraph.Add(sourceCategory, New HashSet(Of String))
                End If
                For Each dep As String In dependsOn
                    dependencyGraph(sourceCategory).Add(dep)
                Next
            End If
        Next

        ' Detect cycles using DFS with visited/recursion stack
        Dim visited As New HashSet(Of String)
        Dim recStack As New HashSet(Of String)

        For Each node As String In dependencyGraph.Keys
            If HasCycleDFS(node, dependencyGraph, visited, recStack) Then
                Return True
            End If
        Next

        Return False
    End Function

    ''' <summary>
    ''' DFS-based cycle detection in the dependency graph
    ''' </summary>
    Private Function HasCycleDFS(ByVal node As String, ByVal graph As Dictionary(Of String, HashSet(Of String)), ByVal visited As HashSet(Of String), ByVal recStack As HashSet(Of String)) As Boolean
        If recStack.Contains(node) Then Return True
        If visited.Contains(node) Then Return False

        visited.Add(node)
        recStack.Add(node)

        If graph.ContainsKey(node) Then
            For Each neighbor As String In graph(node)
                If HasCycleDFS(neighbor, graph, visited, recStack) Then
                    Return True
                End If
            Next
        End If

        recStack.Remove(node)
        Return False
    End Function

    ''' <summary>
    ''' Maps an RI type ID to a category string for dependency graph
    ''' </summary>
    Private Function GetRITypeCategory(ByVal riTypeId As Integer) As String
        Select Case riTypeId
            Case ACQuotaShare, ACSurplus, ACSecondSurplus, ACThirdSurplus, ACQuotaShareRetained
                Return "PROP"
            Case ACRetained
                Return "RET"
            Case ACExcessofLoss
                Return "XOL"
            Case ACProportionalXOL
                Return "PX"
            Case ACCat
                Return "CAT"
            Case Else
                Return ""
        End Select
    End Function

    ''' <summary>
    ''' Returns the list of RI type categories that a given premium calculation basis depends on
    ''' </summary>
    Private Function GetDependenciesFromCalcBasis(ByVal calcBasis As Integer) As List(Of String)
        Dim deps As New List(Of String)

        Select Case calcBasis
            ' PROP depends on CAT
            Case PremiumCalculationBasisEnum.PRGRFACCAT
                deps.Add("CAT")
            ' PROP depends on XOL
            Case PremiumCalculationBasisEnum.PRGRFACXOL
                deps.Add("XOL")
            ' PROP depends on XOL and CAT
            Case PremiumCalculationBasisEnum.PRGFACXCAT
                deps.Add("XOL")
                deps.Add("CAT")
            ' PROP depends on XOL and PX
            Case PremiumCalculationBasisEnum.PROPNTXOPX
                deps.Add("XOL")
                deps.Add("PX")
            ' PROP depends on XOL, PX and CAT
            Case PremiumCalculationBasisEnum.PROPNTXPC
                deps.Add("XOL")
                deps.Add("PX")
                deps.Add("CAT")
            ' XOL depends on PROP
            Case PremiumCalculationBasisEnum.XOLFACPRI
                deps.Add("PROP")
            ' XOL depends on CAT
            Case PremiumCalculationBasisEnum.XOLPRICAT, PremiumCalculationBasisEnum.XOLFACCAT
                deps.Add("CAT")
            ' CAT depends on PROP
            Case PremiumCalculationBasisEnum.CATFACPRI
                deps.Add("PROP")
            ' CAT depends on XOL
            Case PremiumCalculationBasisEnum.CATPRIXOL, PremiumCalculationBasisEnum.CATFACXOL
                deps.Add("XOL")
            ' CAT depends on XOL and PX
            Case PremiumCalculationBasisEnum.CATNTXOPX
                deps.Add("XOL")
                deps.Add("PX")
            ' CAT depends on PROP and XOL
            Case PremiumCalculationBasisEnum.CATNTPRXO
                deps.Add("PROP")
                deps.Add("XOL")
            ' PX depends on PROP
            Case PremiumCalculationBasisEnum.PXFACPRP
                deps.Add("PROP")
            ' PX depends on PROP and XOL
            Case PremiumCalculationBasisEnum.PXFPRPXOL
                deps.Add("PROP")
                deps.Add("XOL")
            ' PX depends on PROP and CAT
            Case PremiumCalculationBasisEnum.PXFPRPCAT
                deps.Add("PROP")
                deps.Add("CAT")
            ' PX depends on PROP, CAT and XOL
            Case PremiumCalculationBasisEnum.PXFPRCATXL
                deps.Add("PROP")
                deps.Add("CAT")
                deps.Add("XOL")
            ' PX depends on CAT
            Case PremiumCalculationBasisEnum.PXFACCAT
                deps.Add("CAT")
            ' PX depends on CAT and XOL
            Case PremiumCalculationBasisEnum.PXFACCATXL
                deps.Add("CAT")
                deps.Add("XOL")
            ' RET depends on CAT
            Case PremiumCalculationBasisEnum.T9PRGRFCCT
                deps.Add("CAT")
            ' RET depends on XOL
            Case PremiumCalculationBasisEnum.T9PRGRFCXL
                deps.Add("XOL")
            ' RET depends on XOL and CAT
            Case PremiumCalculationBasisEnum.T9PRGFXCAT
                deps.Add("XOL")
                deps.Add("CAT")
            ' RET depends on XOL and PX
            Case PremiumCalculationBasisEnum.T9PRNTXOPX
                deps.Add("XOL")
                deps.Add("PX")
            ' RET depends on XOL, PX and CAT
            Case PremiumCalculationBasisEnum.T9PRNTXPCT
                deps.Add("XOL")
                deps.Add("PX")
                deps.Add("CAT")
            ' RET depends on PROP
            Case PremiumCalculationBasisEnum.T9PRGRFCPR
                deps.Add("PROP")
            ' RET depends on PROP and XOL
            Case PremiumCalculationBasisEnum.T9PRFCPRXL
                deps.Add("PROP")
                deps.Add("XOL")
            ' RET depends on PROP, XOL and CAT
            Case PremiumCalculationBasisEnum.T9PRFCPRCT
                deps.Add("PROP")
                deps.Add("XOL")
                deps.Add("CAT")
            ' RET depends on PROP, XOL, CAT and PX
            Case PremiumCalculationBasisEnum.T9PRFCPRPX
                deps.Add("PROP")
                deps.Add("XOL")
                deps.Add("CAT")
                deps.Add("PX")
        End Select

        Return deps
    End Function
End Class
