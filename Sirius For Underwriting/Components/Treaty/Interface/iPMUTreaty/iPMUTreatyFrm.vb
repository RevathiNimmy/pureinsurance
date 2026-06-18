Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Friend Class frmTreaty
    Inherits System.Windows.Forms.Form


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmTreaty"
    Private Const vbFormCode As Integer = 0

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
    Private m_lTreatyID As Integer
    Private m_vTreatyParties(,) As Object
    Private m_dTotalShare As Double
    Private m_lReplacesTreatyID As Integer

    Private m_lReplacedByTreatyID As Integer
    Private m_bIsRi2007Enabled As Boolean
    'E005
    Private m_vBrokerParticipantArray(,) As Object
    Private m_vBrokerParticipantArrayForDisplay(,) As Object

    Private m_sUnderwritingType As String = ""
    ' Treaty array
    Private m_vTreaties(,) As Object
    Private m_vTreatyPartiesBrokerParticipants(,) As Object   'E005
    ' ***************************************************************** '
    '                         PUBLIC METHODS
    ' ***************************************************************** '
    '<Pankaj PN:38977>
    Public WriteOnly Property Treaties() As Object(,)
        Set(ByVal Value(,) As Object)
            m_vTreaties = Value
        End Set
    End Property
    '</Pankaj PN:38977>

    Public Function Clear() As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Const kMethodName As String = "Clear"


        Try

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear controls
        txtCode.Text = ""
        txtDescription.Text = ""
        dtpEffectiveDate.Value = DateTime.Today

        'dtpExpiryDate.Value = Nothing
        txtAgreementCode.Text = ""
        cboReinsuranceType.ItemId = -1
        cboReplacesTreaty.ItemId = -1

        
        ' Clear list view and adjust buttons
        lvwTreatyParty.Items.Clear()
        cmdAdd.Enabled = True
        cmdEdit.Enabled = False
        cmdDelete.Enabled = False

        dtpReplacedEffectivedt.Value = DateTime.Today
        cboReplacedByTreaty.ItemId = -1

        ' Hide Reinstatement Limit, Currency, and Reinstatements fields until XOL is selected
        If m_bIsRi2007Enabled Then
            txtTreatyLimit.Visible = False
            lblTreatyLimit.Visible = False
            uctCurrency.Visible = False
            lblCurrency.Visible = False
            txtReinstatements.Visible = False
            lblReinstatements.Visible = False
            txtTreatyLimit.Text = ""
            txtReinstatements.Text = ""
            fraTreaty.Top = 268
        End If

        ' Refresh treaties just to show total row
        RefreshTreatyParties()

        Catch ex As Exception
        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally




        End Try
        Return result	
    End Function

    Public Function GetProperties(ByRef lTreatyID As Integer, ByRef sCode As String, ByRef sDescription As String, ByRef dtEffectiveDate As Date, ByRef dtExpiryDate As Object, ByRef sAgreementCode As String, ByRef lReinsuranceTypeID As Integer, ByRef sReinsuranceType As String, ByRef lReplacesTreatyID As Object, ByRef lReplacesTreaty As String, ByRef vTreatyParties(,) As Object, ByRef dtReplacedEffectiveDt As Date, ByRef lReplacedByTreatyID As Object, ByRef lReplacedByTreaty As String, _
    Optional ByRef dTreatyLimit As Decimal =0, Optional ByRef lCurrencyID As Integer = 0, Optional ByRef lReinstatements As Integer = 0, _
     Optional ByRef vTreatyPartiesBrokerParticipants(,) As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Const kMethodName As String = "GetProperties"


        Try

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Return all detail data
        lTreatyID = m_lTreatyID
        sCode = txtCode.Text.Trim()
        sDescription = txtDescription.Text.Trim()

        dtEffectiveDate = dtpEffectiveDate.Value

        If dtpExpiryDate.Checked Then
            dtExpiryDate = dtpExpiryDate.Value
        End If
        sAgreementCode = txtAgreementCode.Text.Trim()
        lReinsuranceTypeID = cboReinsuranceType.ItemId
        sReinsuranceType = cboReinsuranceType.ItemCaption


        lReplacesTreatyID = IIf(cboReplacesTreaty.ItemId = 0, DBNull.Value, cboReplacesTreaty.ItemId)
        lReplacesTreaty = IIf(cboReplacesTreaty.ItemId = 0, "", cboReplacesTreaty.ItemCaption)
        vTreatyParties = VB6.CopyArray(m_vTreatyParties)


        dtReplacedEffectiveDt = dtpReplacedEffectivedt.Value


        lReplacedByTreatyID = IIf(cboReplacedByTreaty.ItemId = 0, DBNull.Value, cboReplacedByTreaty.ItemId)
        lReplacedByTreaty = IIf(cboReplacedByTreaty.ItemId = 0, "", cboReplacedByTreaty.ItemCaption)
        ' Get Treaty Limit, Currency, and Reinstatements
        If cboReinsuranceType.ItemCode.Trim() = "XOL" Then
            dTreatyLimit = If(txtTreatyLimit.Text.Trim() = "", 0, CDec(txtTreatyLimit.Text))
            lCurrencyID = If(uctCurrency.CurrencyId <= 0, 0, uctCurrency.CurrencyId)
            lReinstatements = If(txtReinstatements.Text.Trim() = "", 0, CInt(txtReinstatements.Text))
        End If
        vTreatyPartiesBrokerParticipants = m_vTreatyPartiesBrokerParticipants  'E005
        Catch ex As Exception
        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally



        End Try
        Return result	
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="lTreatyID"></param>
    ''' <param name="sCode"></param>
    ''' <param name="sDescription"></param>
    ''' <param name="dtEffectiveDate"></param>
    ''' <param name="dtExpiryDate"></param>
    ''' <param name="sAgreementCode"></param>
    ''' <param name="lReinsuranceTypeID"></param>
    ''' <param name="lReplacesTreatyID"></param>
    ''' <param name="dtReplacedEffectiveDt"></param>
    ''' <param name="lReplacedByTreatyID"></param>
    ''' <param name="vBrokerParticipantArrayForDisplay"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetProperties(ByVal lTreatyID As Integer, _
                                  ByVal sCode As String, _
                                  ByVal sDescription As String, _
                                  ByVal dtEffectiveDate As Date, _
                                  ByVal dtExpiryDate As Object, _
                                  ByVal sAgreementCode As String, _
                                  ByVal lReinsuranceTypeID As Integer, _
                                  ByVal lReplacesTreatyID As Object, _
                                  ByVal dtReplacedEffectiveDt As Date, _
                                  ByVal lReplacedByTreatyID As Integer, _
                                  Optional ByVal dTreatyLimit As Decimal = 0, _
                                  Optional ByVal lCurrencyID As Integer = 0, _
                                  Optional ByVal lReinstatements As Integer = 0, _
                                  Optional ByVal vBrokerParticipantArrayForDisplay As Object = Nothing) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim nReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "SetProperties"
        Try
            cboReinsuranceType.FirstItem = ""
            ' Set all detail data
            m_lTreatyID = lTreatyID
            txtCode.Text = sCode.Trim()
            txtDescription.Text = sDescription.Trim()
            dtpEffectiveDate.Value = dtEffectiveDate

            'If its coming NULL from DB show it equal to dtEffectiveDate as that what happens when a new treaty is made
            If Convert.IsDBNull(dtExpiryDate) OrElse String.IsNullOrEmpty(dtExpiryDate) Then
                dtpExpiryDate.Value = dtEffectiveDate
            ElseIf (Not (Convert.IsDBNull(dtExpiryDate))) AndAlso (Not (String.IsNullOrEmpty(dtExpiryDate))) Then
                dtpExpiryDate.Checked = True
                dtpExpiryDate.Value = CDate(gPMFunctions.BlankToNull(dtExpiryDate))
            End If

            txtAgreementCode.Text = sAgreementCode.Trim()
            cboReinsuranceType.ItemId = lReinsuranceTypeID
            cboReplacesTreaty.ItemId = gPMFunctions.ToSafeLong(lReplacesTreatyID)

            'If Not (Convert.IsDBNull(dtReplacedEffectiveDt) Or IsNothing(dtReplacedEffectiveDt)) Then
            If (Not (Convert.IsDBNull(dtReplacedEffectiveDt))) AndAlso (Not (String.IsNullOrEmpty(dtReplacedEffectiveDt))) Then
                dtpReplacedEffectivedt.Value = gPMFunctions.BlankToNull(dtReplacedEffectiveDt)
            End If

            If Not (Convert.IsDBNull(lReplacesTreatyID) Or IsNothing(lReplacesTreatyID)) Then
                cboReplacedByTreaty.ItemId = gPMFunctions.ToSafeLong(lReplacedByTreatyID)
            Else
                cboReplacedByTreaty.ItemId = -1
            End If
            If m_bIsRi2007Enabled Then
                Dim isXOL As Boolean = (cboReinsuranceType.ItemCode.Trim() = "XOL")
                txtTreatyLimit.Visible = isXOL
                lblTreatyLimit.Visible = isXOL
                uctCurrency.Visible = isXOL
                lblCurrency.Visible = isXOL
                txtReinstatements.Visible = isXOL
                lblReinstatements.Visible = isXOL
                fraTreaty.Top = If(isXOL, 320, 268)

                If isXOL Then
                    ' Set Treaty Limit, Currency, and Reinstatements
                    If Not Convert.IsDBNull(dTreatyLimit) AndAlso dTreatyLimit>0 Then
                        txtTreatyLimit.Text = gPMFunctions.ToSafeCurrency(dTreatyLimit)
                    Else
                        txtTreatyLimit.Text = ""
                    End If

                    If Not Convert.IsDBNull(lCurrencyID) AndAlso lCurrencyID > 0 Then
                        uctCurrency.CurrencyId = gPMFunctions.ToSafeLong(lCurrencyID)
                    Else
                        uctCurrency.ListIndex = -1
                    End If

                    If Not Convert.IsDBNull(lReinstatements) AndAlso lReinstatements >0 Then
                        txtReinstatements.Text = CInt(lReinstatements).ToString()
                    Else
                        txtReinstatements.Text = ""
                    End If
                End If
            End If
            ' Load treaty parties
            nReturn = GetTreatyParties()
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetTreatyParties", "Unable to retrieve Treaty Party information")
            End If

            ' Display treaty parties
            nReturn = RefreshTreatyParties()
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("RefreshTreatyParties", "Unable to display Treaty Party information")
            End If

            ' Refresh replaces treaty code
            m_lReplacesTreatyID = CInt(lReplacesTreatyID)
            m_lReplacedByTreatyID = lReplacedByTreatyID
            nReturn = RefreshReplacesList()
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("RefreshReplacesList", "Unable to refresh the replaces treaty list")
            End If

            cboReplacesTreaty.ItemId = CInt(lReplacesTreatyID)
            'PN33619
            cboReplacedByTreaty.ItemId = lReplacedByTreatyID
            m_vBrokerParticipantArrayForDisplay = vBrokerParticipantArrayForDisplay  'E005
            Return nResult
        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult)
            Return PMEReturnCode.PMFail
        End Try
    End Function


    ' ***************************************************************** '
    '                         PRIVATE METHODS
    ' ***************************************************************** '
    Private Function GetTreatyParties() As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "SetFieldValidation"

        'E005
        Dim vTreatyPartiesBrokerParticipantsTemp As Object
        Dim lCount As Long
        'E005

        Try

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get treaty party details from the business object.

        lReturn = Business.GetTreatyPartyList(v_lTreatyID:=m_lTreatyID, r_vTreatyParties:=m_vTreatyParties)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oBusiness.GetTreatyPartyList", "Unable to retrieve treaty parties")
        End If
        ' E005
        ' Add to m_vTreatyParties data come from spu_Treaty_Party_BrokerParticipants_saa
        ' With proper logic depend upon data in m_vTreatyParties at 9th position after redim
        lReturn = Business.GetTreatyPartyBrokerParticipantList( _
                                                        v_lTreatyID:=m_lTreatyID, _
                                                        r_vTreatyPartiesBrokerParticipant:=m_vTreatyPartiesBrokerParticipants)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            RaiseError("m_oBusiness.GetTreatyPartyBrokerParticipantList", "Unable to retrieve treaty parties broker participants")
        End If

        ' Need to manipulate array
        If IsArray(m_vTreatyPartiesBrokerParticipants) Then
            ReDim vTreatyPartiesBrokerParticipantsTemp(UBound(m_vTreatyPartiesBrokerParticipants, 2), UBound(m_vTreatyPartiesBrokerParticipants, 1))
            For lCount = 0 To UBound(m_vTreatyPartiesBrokerParticipants, 2)
                vTreatyPartiesBrokerParticipantsTemp(lCount, TreatyPartyBrokerParticipantEnum.DBTPBPParticipantonTreatyID) = _
                m_vTreatyPartiesBrokerParticipants(TreatyPartyBrokerParticipantEnum.DBTPBPParticipantonTreatyID, lCount)
                vTreatyPartiesBrokerParticipantsTemp(lCount, TreatyPartyBrokerParticipantEnum.DBTPBPTreatyID) = _
                m_vTreatyPartiesBrokerParticipants(TreatyPartyBrokerParticipantEnum.DBTPBPTreatyID, lCount)
                vTreatyPartiesBrokerParticipantsTemp(lCount, TreatyPartyBrokerParticipantEnum.DBTPBPTreatyTartyID) = _
                m_vTreatyPartiesBrokerParticipants(TreatyPartyBrokerParticipantEnum.DBTPBPTreatyTartyID, lCount)
                vTreatyPartiesBrokerParticipantsTemp(lCount, TreatyPartyBrokerParticipantEnum.DBTPBPassociatedPartyCnt) = _
                m_vTreatyPartiesBrokerParticipants(TreatyPartyBrokerParticipantEnum.DBTPBPassociatedPartyCnt, lCount)
                vTreatyPartiesBrokerParticipantsTemp(lCount, TreatyPartyBrokerParticipantEnum.DBTPBPPartyCnt) = _
                m_vTreatyPartiesBrokerParticipants(TreatyPartyBrokerParticipantEnum.DBTPBPPartyCnt, lCount)
                vTreatyPartiesBrokerParticipantsTemp(lCount, TreatyPartyBrokerParticipantEnum.DBTPBPParticipantPercent) = _
                m_vTreatyPartiesBrokerParticipants(TreatyPartyBrokerParticipantEnum.DBTPBPParticipantPercent, lCount)
                vTreatyPartiesBrokerParticipantsTemp(lCount, TreatyPartyBrokerParticipantEnum.DBTPBPShortCode) = _
                m_vTreatyPartiesBrokerParticipants(TreatyPartyBrokerParticipantEnum.DBTPBPShortCode, lCount)
                vTreatyPartiesBrokerParticipantsTemp(lCount, TreatyPartyBrokerParticipantEnum.DBTPBPName) = _
                m_vTreatyPartiesBrokerParticipants(TreatyPartyBrokerParticipantEnum.DBTPBPName, lCount)
                vTreatyPartiesBrokerParticipantsTemp(lCount, TreatyPartyBrokerParticipantEnum.RowID) = _
            m_vTreatyPartiesBrokerParticipants(TreatyPartyBrokerParticipantEnum.RowID, lCount)
            Next
            m_vTreatyPartiesBrokerParticipants = vTreatyPartiesBrokerParticipantsTemp
        End If
        ' E005

        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally

        End Try
        Return result
    End Function

    Private Function IsValidParty() As Boolean

        Dim result As Boolean = False
        Dim lReturn As Integer
        Const kMethodName As String = "IsValidParty"


        Try

            ' Check for active item
            If Not (lvwTreatyParty.FocusedItem Is Nothing) Then
                ' Check for total line
                If lvwTreatyParty.FocusedItem.Name <> "TOTAL" Then
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

    Private Function RefreshReplacesList() As Integer

        Dim result As Integer = 0
        Dim lOriginal, lReplacedOriginal, lReturn As Integer
        Const kMethodName As String = "RefreshReplacesList"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Save original value
            lOriginal = cboReplacesTreaty.ItemId
            lReplacedOriginal = cboReplacedByTreaty.ItemId

            ' Set filter criteria
            ' - We don't want this treaty!
            ' - We only want treaties older than this one,
            ' - or the original treaty
            cboReplacesTreaty.WhereClause = "treaty_id <> " & m_lTreatyID & " AND " & _
                                            "(treaty_id = " & CStr(m_lReplacesTreatyID) & " OR effective_date < '" & DateTime.FromOADate(dtpEffectiveDate.Value.ToOADate()).ToString("yyyy.MM.dd") & "')"
            cboReplacesTreaty.RefreshList()
            cboReplacesTreaty.ItemId = lOriginal

            'PN33619
            If gPMFunctions.ToSafeLong(m_lReplacedByTreatyID) > 0 Then
                cboReplacedByTreaty.WhereClause = "treaty_id <> " & m_lTreatyID
            End If

            cboReplacedByTreaty.RefreshList()
            cboReplacedByTreaty.ItemId = lReplacedOriginal


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally

        End Try
        Return result
    End Function

    Private Function RefreshTreatyParties(Optional ByVal v_lIndex As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem

        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "RefreshTreatyParties"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the list before we start
            cmdEdit.Enabled = False
            cmdDelete.Enabled = False
            lvwTreatyParty.Items.Clear()
            m_dTotalShare = 0

            ' Check for items (we may not have any yet)
            If Information.IsArray(m_vTreatyParties) Then
                ' Process all treaties
                For lCount As Integer = m_vTreatyParties.GetLowerBound(1) To m_vTreatyParties.GetUpperBound(1)
                    ' Don't add deleted (party_cnt = 0) parties
                    If CDbl(m_vTreatyParties(MainModule.TreatyPartyEnum.DBTPPartyCnt, lCount)) > 0 Then
                        ' Add the list item
                        oListItem = lvwTreatyParty.Items.Add("TP" & lCount, CStr(m_vTreatyParties(MainModule.TreatyPartyEnum.DBTPResolvedName, lCount)), "")

                        ListViewHelper.GetListViewSubItem(oListItem, 1).Text = "   " & gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, m_vTreatyParties(MainModule.TreatyPartyEnum.DBTPSharePercent, lCount), -5)
                        ListViewHelper.GetListViewSubItem(oListItem, 2).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, m_vTreatyParties(MainModule.TreatyPartyEnum.DBTPCommissionPercent, lCount), -5)
                        ListViewHelper.GetListViewSubItem(oListItem, 3).Text = CStr(m_vTreatyParties(MainModule.TreatyPartyEnum.DBTPTaxGroup, lCount))
                        ListViewHelper.GetListViewSubItem(oListItem, 4).Text = IIf(gPMFunctions.ToSafeBoolean(m_vTreatyParties(MainModule.TreatyPartyEnum.DBTPDomiciledForTax, lCount)), "Yes", "No")

                        ' Keep running total
                        m_dTotalShare += gPMFunctions.ToSafeDouble(m_vTreatyParties(MainModule.TreatyPartyEnum.DBTPSharePercent, lCount))

                        ' Store array index so we can find the original record
                        oListItem.Tag = CStr(lCount)

                        ' Check for selected item
                        If lCount = v_lIndex Then
                            ' If we are refreshing reselect the original item
                            lvwTreatyParty.FullRowSelect = True
                            lvwTreatyParty.Items(oListItem.Index).Selected = True
                            lvwTreatyParty.Select()
                            ' Click the item to refresh buttons
                            'Developer Guide No. 185
                            lvwTreatyParty_SelectedIndexChanged(lvwTreatyParty, New EventArgs())
                        End If
                    End If
                Next lCount
            End If

            ' Ignore errors this is only a cosmetic nicety
            'Developer Guide No. 178
            lReturn = CType(ListView6Func.ListViewAutoSize(lvwTreatyParty, True, True, Me), gPMConstants.PMEReturnCode)

            ' Refresh sort order (also adds total row)
            SortList(ListViewHelper.GetSortKeyProperty(lvwTreatyParty), True)


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

            lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboReinsuranceType, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oFormFields.AddNewFormField", "Failed to add cboReinsuranceType")
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

        result = gPMConstants.PMEReturnCode.PMTrue
        ' Remove an existing total row (ignore errors)
        lvwTreatyParty.Items.RemoveByKey("TOTAL")
        ' We may just be refreshing after a item edit or addition
        If Not bReSort Then
            ' Reverse sort order if column hasn't changed
            If ListViewHelper.GetSortKeyProperty(lvwTreatyParty) = lColumnIndex Then
                ListViewHelper.SetSortOrderProperty(lvwTreatyParty, IIf(ListViewHelper.GetSortOrderProperty(lvwTreatyParty) = SortOrder.Ascending, SortOrder.Descending, SortOrder.Ascending))
            Else
                ListViewHelper.SetSortOrderProperty(lvwTreatyParty, SortOrder.Ascending)
            End If
        End If

        ' Sort based on contents
        Select Case lColumnIndex
            Case 1, 2 ' Percentage
                'Developer Guide No. 178
                ListView6Func.ListViewSortByValue(lvwTreatyParty, lColumnIndex, ListViewHelper.GetSortOrderProperty(lvwTreatyParty), True, True)
            Case Else
                ListViewHelper.SetSortKeyProperty(lvwTreatyParty, lColumnIndex)
                ListViewHelper.SetSortedProperty(lvwTreatyParty, True)
                ListViewHelper.SetSortedProperty(lvwTreatyParty, False) ' Need to turn off sort to protect Total row
        End Select

        ' Add summary list item
        oListItem = lvwTreatyParty.Items.Add("TOTAL", "Total", "")
        ListViewHelper.GetListViewSubItem(oListItem, 1).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, m_dTotalShare, -5)
        ListViewHelper.GetListViewSubItem(oListItem, 2).Text = " "
        ListViewHelper.GetListViewSubItem(oListItem, 3).Text = " "

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



        Return result
    End Function

    Private Function Validate_Renamed() As Integer

        Dim result As Integer = 0
        Dim dShare As Double
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "Validate"


        Try

            ' Default to false, only set true if we get to the end
            result = gPMConstants.PMEReturnCode.PMFalse
            If m_oFormFields Is Nothing Then
                m_oFormFields = New iPMFormControl.FormFields
            End If
            ' Standard validation
            lReturn = m_oFormFields.CheckMandatoryControls()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            '<pankaj PN:38886>

            If Not (Convert.IsDBNull(dtpExpiryDate.Value) Or IsNothing(dtpExpiryDate.Value)) Then
                If dtpExpiryDate.Checked Then
                    If dtpExpiryDate.Value < dtpEffectiveDate.Value Then
                        MessageBox.Show("Expiry Date can not be less than Effective Date", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        dtpExpiryDate.Focus()
                        Return result
                    End If
                End If
            End If
            '<pankaj PN:38886>

            '<Pankaj PN:38977>
            If Information.IsArray(m_vTreaties) Then
                For lCount As Integer = m_vTreaties.GetLowerBound(1) To m_vTreaties.GetUpperBound(1)
                    If CStr(m_vTreaties(MainModule.TreatyEnum.DBTCode, lCount)).Trim().ToUpper() = txtCode.Text.Trim().ToUpper() And CDbl(m_vTreaties(MainModule.TreatyEnum.DBTTreatyID, lCount)) <> m_lTreatyID Then
                        MessageBox.Show("Treaty code already exists", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        txtCode.Focus()
                        Return result
                    End If
                Next lCount
            End If
            '</Pankaj PN:38977>

            ' Validate currency when treaty limit is entered
            If txtTreatyLimit.Text.Trim() <> "" AndAlso (uctCurrency.CurrencyId<= 0 OrElse uctCurrency.ListIndex = -1) Then
                MessageBox.Show("Please select the currency for Treaty Limit.", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                uctCurrency.Focus()
                Return result
            End If
            ' Check share total
            If Math.Round(m_dTotalShare, 5) <> 100 Then
                MessageBox.Show("Treaty party share must total 100%", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                lvwTreatyParty.Focus()
                lvwTreatyParty.FullRowSelect = True
                lvwTreatyParty.Items(0).Selected = True
                Return result
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
    Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click

        Dim oForm As frmTreatyParty

        Dim lPartyCnt As Integer
        Dim sResolvedName As String = ""
        Dim dSharePercent, dCommPercent As Double
        Dim sTaxGroup As String = ""
        Dim bIsDomiciled As Boolean
        'E016
        Dim lApproved As Long
        Dim vRIBrokerParticipant As Object 'E005
        Dim bRetrun As Boolean              'E005
        Dim lIndex As Integer
        Dim lReturn As gPMConstants.PMEReturnCode

        Dim sId As String
        Dim lCount As Long

        Const kMethodName As String = "cmdAdd_Click"


        Try

        ' Create treatyparty form
        oForm = New frmTreatyParty()
        oForm.Business = Business
        oForm.frmTreatyPartyLoad()
        ' Clear form
        lReturn = CType(oForm.Clear(m_dTotalShare), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("oForm.Clear", "Unable to set default properties on treaty party dialog")
        End If

        ' Show dialog
        oForm.ShowDialog()

        ' Check result
        If oForm.Status = gPMConstants.PMEReturnCode.PMOK Then
            ' Get results
            lReturn = CType(oForm.GetProperties(lPartyCnt, sResolvedName, dSharePercent, dCommPercent, sTaxGroup, bIsDomiciled, lApproved, _
            vRIBrokerParticipant), gPMConstants.PMEReturnCode)

            ' Increase array
            If Information.IsArray(m_vTreatyParties) Then
                ReDim Preserve m_vTreatyParties(DBTPMax, m_vTreatyParties.GetUpperBound(1) + 1)
            Else
                ReDim m_vTreatyParties(DBTPMax, 0)
            End If
            lIndex = m_vTreatyParties.GetUpperBound(1)

            ' Store results
            m_vTreatyParties(MainModule.TreatyPartyEnum.DBTPPartyCnt, lIndex) = lPartyCnt
            m_vTreatyParties(MainModule.TreatyPartyEnum.DBTPResolvedName, lIndex) = sResolvedName
            m_vTreatyParties(MainModule.TreatyPartyEnum.DBTPSharePercent, lIndex) = dSharePercent
            m_vTreatyParties(MainModule.TreatyPartyEnum.DBTPCommissionPercent, lIndex) = dCommPercent
            m_vTreatyParties(MainModule.TreatyPartyEnum.DBTPTaxGroup, lIndex) = sTaxGroup
            m_vTreatyParties(MainModule.TreatyPartyEnum.DBTPDomiciledForTax, lIndex) = bIsDomiciled

            'Start E016
            If m_bIsRi2007Enabled Then
                m_vTreatyParties(MainModule.TreatyPartyEnum.DBTPIsReinsurerApproved, lIndex) = lApproved
            End If
            'End E016

            'E005
            If IsArray(m_vTreatyParties) Then
                sId = "ID-" & (UBound(m_vTreatyParties, 2) + 1)
                m_vTreatyParties(MainModule.TreatyPartyEnum.DBTPRowID, lIndex) = sId
            End If

            If IsArray(vRIBrokerParticipant) Then
                ReDim Preserve vRIBrokerParticipant(UBound(vRIBrokerParticipant, 1), UBound(vRIBrokerParticipant, 2) + 1)
            End If

            If IsArray(vRIBrokerParticipant) Then
                For lCount = 0 To UBound(vRIBrokerParticipant, 1)
                    vRIBrokerParticipant(lCount, UBound(vRIBrokerParticipant, 2)) = sId
                Next
            End If

            ' Add Broker Array
            bRetrun = CombineArrays(v_vRIBrokerParticipant:=vRIBrokerParticipant)
            ' E005
            ' Refresh list
            lReturn = CType(RefreshTreatyParties(lIndex), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("RefreshTreatyParties(lIndex)", "Unable to refresh treaty party list")
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

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        ' Check the user wants to close
        If MessageBox.Show("Cancelling will lose all of your current details." & _
                           Strings.Chr(13) & Strings.Chr(10) & "Do you really wish to cancel?", Text, MessageBoxButtons.YesNo) = System.Windows.Forms.DialogResult.Yes Then
            ' Set status to cancel and close
            Status = gPMConstants.PMEReturnCode.PMCancel
            Me.Hide()
        End If
    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click

        Dim lIndex As Integer
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "cmdEdit_Click"


        Try

        ' Check valid party
        If IsValidParty() Then
            ' Get index of selected item
            lIndex = gPMFunctions.ToSafeLong(Convert.ToString(lvwTreatyParty.FocusedItem.Tag), -1)

            ' We can delete this line, set party_cnt to zero and refresh
            m_vTreatyParties(MainModule.TreatyPartyEnum.DBTPPartyCnt, lIndex) = 0

            ' Refresh list
            lReturn = CType(RefreshTreatyParties(lIndex), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("RefreshTreatyParties(lIndex)", "Unable to refresh treaty party list")
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

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click

        Dim oForm As frmTreatyParty

        Dim lPartyCnt As Integer
        Dim sResolvedName As String = ""
        Dim dSharePercent, dCommPercent As Double
        Dim sTaxGroup As String = ""
        Dim bIsDomiciled As Boolean
        'E016
        Dim lApproved As Long
        Dim vRIBrokerParticipant As Object     'E005
        Dim lIndex As Integer
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "cmdEdit_Click"
        Dim sID As String
        Dim lCount As Long

        Try

        ' Check valid party
        If IsValidParty() Then
            ' Get index of selected item
            lIndex = gPMFunctions.ToSafeLong(Convert.ToString(lvwTreatyParty.FocusedItem.Tag))

            ' Create treatyparty form
            oForm = New frmTreatyParty()
            oForm.Business = Business
            oForm.frmTreatyPartyLoad()
            ' Set properties
            lReturn = CType(oForm.SetProperties(CInt(m_vTreatyParties(MainModule.TreatyPartyEnum.DBTPPartyCnt, lIndex)), CStr(m_vTreatyParties(MainModule.TreatyPartyEnum.DBTPResolvedName, lIndex)), CDbl(m_vTreatyParties(MainModule.TreatyPartyEnum.DBTPSharePercent, lIndex)), CDbl(m_vTreatyParties(MainModule.TreatyPartyEnum.DBTPCommissionPercent, lIndex)), m_vTreatyParties(MainModule.TreatyPartyEnum.DBTPIsReinsurerApproved, lIndex), m_vTreatyPartiesBrokerParticipants), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("oForm.SetProperties", "Unable to set properties on treaty party dialog")
            End If

            ' Show dialog
            oForm.ShowDialog()

            ' Check result
            If oForm.Status = gPMConstants.PMEReturnCode.PMOK Then
                ' Get results
                lReturn = CType(oForm.GetProperties(lPartyCnt, sResolvedName, dSharePercent, dCommPercent, sTaxGroup, bIsDomiciled, _
                lApproved, _
                vRIBrokerParticipant), gPMConstants.PMEReturnCode)

                ' Store results
                m_vTreatyParties(MainModule.TreatyPartyEnum.DBTPPartyCnt, lIndex) = lPartyCnt
                m_vTreatyParties(MainModule.TreatyPartyEnum.DBTPResolvedName, lIndex) = sResolvedName
                m_vTreatyParties(MainModule.TreatyPartyEnum.DBTPSharePercent, lIndex) = dSharePercent
                m_vTreatyParties(MainModule.TreatyPartyEnum.DBTPCommissionPercent, lIndex) = dCommPercent
                m_vTreatyParties(MainModule.TreatyPartyEnum.DBTPTaxGroup, lIndex) = sTaxGroup
                m_vTreatyParties(MainModule.TreatyPartyEnum.DBTPDomiciledForTax, lIndex) = bIsDomiciled
                'Start E016
                If m_bIsRi2007Enabled Then
                    m_vTreatyParties(MainModule.TreatyPartyEnum.DBTPIsReinsurerApproved, lIndex) = lApproved
                End If
                'End E016

                m_lReturn = CombineArrays(v_vRIBrokerParticipant:=vRIBrokerParticipant)  'E005

                ' Refresh list
                lReturn = CType(RefreshTreatyParties(lIndex), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("RefreshTreatyParties(lIndex)", "Unable to refresh treaty party list")
                End If
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
        RefreshReplacesList()
    End Sub

    Public Sub frmTreatyLoad()

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vValue As String = ""


        Const kMethodName As String = "Form_Load"


        Try
        cboReinsuranceType.FirstItem = ""
        

        cboReplacedByTreaty.FirstItem = "(none)"
        cboReplacesTreaty.FirstItem = "(none)"
        uctCurrency.FirstItem = ""
        ' Set the mouse pointer to busy.
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
        
        m_lReturn = iPMFunc.GetSystemOption(5005, m_sUnderwritingType)

        m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableRI2007, v_vBranch:=g_iSourceID, r_vUnderwriting:=vValue)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("Form Load", "getProductOptionValue Failed for option " & gPMConstants.SIRHiddenOptions.SIROPTEnableRI2007, gPMConstants.PMELogLevel.PMLogError)
        End If

        m_bIsRi2007Enabled = vValue = "1"
        If m_bIsRi2007Enabled Then  
            AddHandler cboReinsuranceType.Click, AddressOf CboReinsuranceType_Click
        Else
            txtTreatyLimit.Visible = False
            lblTreatyLimit.Visible = False
            uctCurrency.Visible = False
            lblCurrency.Visible = False
            txtReinstatements.Visible = False
            lblReinstatements.Visible = False
            fraTreaty.Top = 268
        End If

        ' Validate fields using Forms Control
        lReturn = CType(SetFieldValidation(), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("SetFieldValidation", "Unable to set field validation")
        End If

        If m_sUnderwritingType = "1" Then
            lblReinsuranceType.Text = "Insurance Type:"
            lvwTreatyParty.Columns.Item(0).Text = "Insurer"
        End If

       

        Catch ex As Exception
        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally
        ' Set the mouse pointer to normal.
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)



        End Try
    End Sub
    Private Sub frmTreaty_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        Dim lReturn As Integer
        Const kMethodName As String = "Form_QueryUnload"


        Try

        ' Check if the interface has been terminated by means
        ' other than pressing the command buttons.


        If UnloadMode <> vbFormCode Then
            ' Check the user wants to close
            If MessageBox.Show("Cancelling will lose all of your current details." & _
                               Strings.Chr(13) & Strings.Chr(10) & "Do you really wish to cancel?", Text, MessageBoxButtons.YesNo) = System.Windows.Forms.DialogResult.No Then
                ' Do not procced with the interface termination.
                Cancel = 1
                eventArgs.cancel = True
            End If
        End If


        Catch ex As Exception
        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally


        End Try
        Exit Sub
    End Sub

    Private Sub lvwTreatyParty_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwTreatyParty.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwTreatyParty.Columns(eventArgs.Column)
        SortList(ColumnHeader.Index + 1 - 1)
    End Sub

    Private Sub lvwTreatyParty_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwTreatyParty.DoubleClick
        cmdEdit_Click(cmdEdit, New EventArgs())
    End Sub

    Private Sub txtTreatyLimit_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTreatyLimit.KeyPress
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso e.KeyChar <> "."c AndAlso e.KeyChar <> ","c Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtTreatyLimit_Leave(ByVal sender As Object, ByVal e As EventArgs) Handles txtTreatyLimit.Leave
        If txtTreatyLimit.Text.Trim() <> "" Then
            Dim dValue As Decimal
            If Decimal.TryParse(txtTreatyLimit.Text, dValue) AndAlso dValue > 0 Then
                txtTreatyLimit.Text = dValue.ToString("N2")
                txtTreatyLimit.Text = CStr(gPMFunctions.ToSafeCurrency(txtTreatyLimit.Text))
            Else
                txtTreatyLimit.Text = ""
            End If
        End If
    End Sub

    Private Sub txtReinstatements_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtReinstatements.KeyPress
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    
    Private Sub CboReinsuranceType_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboReinsuranceType.Click
        If m_bIsRi2007Enabled Then  
            Dim isExcessOfLoss As Boolean = (cboReinsuranceType.ItemCode.Trim() = "XOL")
            txtTreatyLimit.Visible = isExcessOfLoss
            lblTreatyLimit.Visible = isExcessOfLoss
            uctCurrency.Visible = isExcessOfLoss
            lblCurrency.Visible = isExcessOfLoss
            txtReinstatements.Visible = isExcessOfLoss
            lblReinstatements.Visible = isExcessOfLoss
        
            ' Adjust Treaty Parties position to collapse space when controls are hidden
            If isExcessOfLoss Then
            
            txtTreatyLimit.Text = ""
            uctCurrency.CurrencyId = 0
            uctCurrency.ListIndex = -1
            txtReinstatements.Text = ""

                fraTreaty.Top = 320
            Else
                fraTreaty.Top = 268
            End If
        End If

    End Sub
    

    Private Sub lvwTreatyParty_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwTreatyParty.SelectedIndexChanged
        Dim lReturn As Integer
        Const kMethodName As String = "lvwTreatyParty_ItemClick"
        If lvwTreatyParty.SelectedItems.Count < 1 Then
            Exit Sub
        End If

        Try

        ' Set enabled states
        cmdDelete.Enabled = (lvwTreatyParty.SelectedItems(0).Name <> "TOTAL")
        cmdEdit.Enabled = (lvwTreatyParty.SelectedItems(0).Name <> "TOTAL")


        Catch ex As Exception
        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally
        ' Reset the mouse pointer to normal.
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)



        End Try
        Exit Sub
    End Sub

    ' Used to manipulate broker participant array according to desired format
    'Created by : Santosh Singh for E005 requirement
    'Paralleled by : Shubhankar Singh for E005 requirement
    '*************************************************************************
    Private Function CombineArrays(ByVal v_vRIBrokerParticipant As Object) As Boolean
        Dim vTreatyPartiesBrokerParticipantsTemp As Object
        Dim iUbound As Integer
        Dim lCounter As Long
        Dim lCounterTemp As Long
        Const kMethodName As String = "CombineArrays"

        Try
 
            CombineArrays = True
            'Manipulate m_vTreatyPartiesBrokerParticipants
            lCounterTemp = 0

            If IsArray(v_vRIBrokerParticipant) Then
                iUbound = iUbound + (UBound(v_vRIBrokerParticipant))
            End If

            ReDim vTreatyPartiesBrokerParticipantsTemp(iUbound, 8)

            If IsArray(v_vRIBrokerParticipant) Then
                For lCounter = 0 To UBound(v_vRIBrokerParticipant)
                    vTreatyPartiesBrokerParticipantsTemp(lCounter, TreatyPartyBrokerParticipantEnum.DBTPBPParticipantonTreatyID) = "" ' Set participantontreaty_id
                    vTreatyPartiesBrokerParticipantsTemp(lCounter, TreatyPartyBrokerParticipantEnum.DBTPBPTreatyID) = m_lTreatyID
                    vTreatyPartiesBrokerParticipantsTemp(lCounter, TreatyPartyBrokerParticipantEnum.DBTPBPTreatyTartyID) = "" ' Set treaty_party_id
                    vTreatyPartiesBrokerParticipantsTemp(lCounter, TreatyPartyBrokerParticipantEnum.DBTPBPassociatedPartyCnt) = v_vRIBrokerParticipant(lCounter, 4)
                    vTreatyPartiesBrokerParticipantsTemp(lCounter, TreatyPartyBrokerParticipantEnum.DBTPBPPartyCnt) = v_vRIBrokerParticipant(lCounter, 3)
                    vTreatyPartiesBrokerParticipantsTemp(lCounter, TreatyPartyBrokerParticipantEnum.DBTPBPParticipantPercent) = v_vRIBrokerParticipant(lCounter, 2)
                    vTreatyPartiesBrokerParticipantsTemp(lCounter, TreatyPartyBrokerParticipantEnum.DBTPBPShortCode) = v_vRIBrokerParticipant(lCounter, 0) ' Set ShortCode
                    vTreatyPartiesBrokerParticipantsTemp(lCounter, TreatyPartyBrokerParticipantEnum.DBTPBPName) = v_vRIBrokerParticipant(lCounter, 1) ' Set Name
                    'vTreatyPartiesBrokerParticipantsTemp(lCounter, TreatyPartyBrokerParticipantEnum.RowID) = v_vRIBrokerParticipant(lCounter, 5)
                    lCounterTemp = lCounterTemp + 1
                Next
            End If

            If IsArray(vTreatyPartiesBrokerParticipantsTemp) Then
                m_vTreatyPartiesBrokerParticipants = vTreatyPartiesBrokerParticipantsTemp
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally
            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        End Try
    End Function
End Class
