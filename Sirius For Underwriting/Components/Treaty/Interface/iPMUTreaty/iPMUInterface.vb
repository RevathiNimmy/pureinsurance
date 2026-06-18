Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 09/06/1999
    '
    ' Description: Main interface.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmInterface"

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMUTreaty.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Stores the return value for the a function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Treaty array
    Private m_vTreaties(,) As Object


    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As gPMConstants.PMEReturnCode
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As gPMConstants.PMENavigateButtonStatus
    Private m_lProcessMode As gPMConstants.PMEProcessMode
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private Const vbFormCode As Integer = 0
    Private m_sUniqueId As String
    Private m_sScreenHierarchy As String


    ' ***************************************************************** '
    '                         PUBLIC PROPERTIES
    ' ***************************************************************** '
    Public ReadOnly Property ErrorNumber() As Integer
        Get
            ' Return any error number that might have occurred on the interface.
            Return m_lErrorNumber
        End Get
    End Property
    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            ' Set the calling application name.
            m_sCallingAppName = Value
        End Set
    End Property
    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)
            m_dtEffectiveDate = Value
        End Set
    End Property
    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)
            m_lNavigate = Value
        End Set
    End Property
    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)
            m_lProcessMode = Value
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
    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)
            m_sTransactionType = Value
        End Set
    End Property



    ' ***************************************************************** '
    '                          PUBLIC METHODS
    ' ***************************************************************** '

    ' ***************************************************************** '
    ' Name: BusinessToInterface
    '
    ' Description: Updates all interface details from the business object.
    ' ***************************************************************** '
    Public Function BusinessToInterface(Optional ByVal v_lIndex As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem

        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "BusinessToInterface"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the list before we start
            cmdEdit.Enabled = False
            cmdDelete.Enabled = False
            lvwTreaties.Items.Clear()

            ' Check for items (we may not have any yet)
            If Information.IsArray(m_vTreaties) Then
                ' Process all treaties
                For lCount As Integer = m_vTreaties.GetLowerBound(1) To m_vTreaties.GetUpperBound(1)
                    If (chkHideDeleted.CheckState = CheckState.Unchecked Or Not gPMFunctions.ToSafeBoolean(m_vTreaties(MainModule.TreatyEnum.DBTIsDeleted, lCount))) And (chkHideExpired.CheckState = CheckState.Unchecked Or gPMFunctions.ToSafeDate(m_vTreaties(MainModule.TreatyEnum.DBTExpiryDate, lCount), DateTime.Today.AddDays(1)) > DateTime.Today) Then
                        ' Add the list item
                        oListItem = lvwTreaties.Items.Add("T" & CStr(m_vTreaties(MainModule.TreatyEnum.DBTTreatyID, lCount)), CStr(m_vTreaties(MainModule.TreatyEnum.DBTCode, lCount)).Trim(), "")

                        ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vTreaties(MainModule.TreatyEnum.DBTDescription, lCount))
                        ListViewHelper.GetListViewSubItem(oListItem, 2).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateShort, m_vTreaties(MainModule.TreatyEnum.DBTEffectiveDate, lCount))
                        ListViewHelper.GetListViewSubItem(oListItem, 3).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateShort, m_vTreaties(MainModule.TreatyEnum.DBTExpiryDate, lCount))
                        ListViewHelper.GetListViewSubItem(oListItem, 4).Text = CStr(m_vTreaties(MainModule.TreatyEnum.DBTAgreementCode, lCount))
                        ListViewHelper.GetListViewSubItem(oListItem, 5).Text = CStr(m_vTreaties(MainModule.TreatyEnum.DBTReinsuranceType, lCount))
                        ListViewHelper.GetListViewSubItem(oListItem, 6).Text = CStr(m_vTreaties(MainModule.TreatyEnum.DBTReplacesTreaty, lCount))

                        ' Gray out deleted items
                        If gPMFunctions.ToSafeBoolean(m_vTreaties(MainModule.TreatyEnum.DBTIsDeleted, lCount)) Then

                            'Developer Guide No. 12 (No Solution)
                            oListItem.ForeColor = SystemColors.GrayText
                            For Each oListSubItem As ListViewItem.ListViewSubItem In oListItem.SubItems
                                oListSubItem.ForeColor = SystemColors.GrayText
                            Next oListSubItem
                        End If

                        ' Store array index so we can find the original record
                        oListItem.Tag = CStr(lCount)

                        ' Check for selected item

                        If lCount = v_lIndex Then
                            ' If we are refreshing reselect the original item
                            lvwTreaties.FullRowSelect = True
                            lvwTreaties.Items(oListItem.Index).Selected = True
                            lvwTreaties.Select()
                            If lvwTreaties.Visible Then
                                lvwTreaties.Focus()
                            End If

                            ' Click the item to refresh buttons
                            'lvwTreaties_SelectedIndexChanged(lvwTreaties, New EventArgs())
                        End If
                    End If
                Next lCount
            End If

            ' Ignore errors this is only a cosmetic nicety
            'Developer Guide No. 178
            lReturn = CType(ListView6Func.ListViewAutoSize(lvwTreaties, True, True, Me), gPMConstants.PMEReturnCode)

            ' Refresh sort order
            SortList(ListViewHelper.GetSortKeyProperty(lvwTreaties), True)

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "GetBusiness"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the details from the business object.

            lReturn = m_oBusiness.GetTreatyList(r_vTreaties:=m_vTreaties)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oBusiness.GetTreatyList", "Unable to get treaty list")
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

    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Const kMethodName As String = "SetInterfaceDefaults"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            cmdAdd.Enabled = (Task <> gPMConstants.PMEComponentAction.PMView)
            cmdEdit.Enabled = False
            cmdDelete.Enabled = False

            cmdClose.Enabled = True


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally


        End Try
        Return result
    End Function

    Private Function SortList(ByVal lColumnIndex As Integer, Optional ByVal bReSort As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Const kMethodName As String = "SortList"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' We may just be refreshing after a item edit or addition
            If Not bReSort Then
                ' Reverse sort order if column hasn't changed
                If ListViewHelper.GetSortKeyProperty(lvwTreaties) = lColumnIndex Then
                    ListViewHelper.SetSortOrderProperty(lvwTreaties, IIf(ListViewHelper.GetSortOrderProperty(lvwTreaties) = SortOrder.Ascending, SortOrder.Descending, SortOrder.Ascending))
                Else
                    ListViewHelper.SetSortOrderProperty(lvwTreaties, SortOrder.Ascending)
                End If
            End If

            ' Sort based on contents
            Select Case lColumnIndex
                Case 2, 3 ' Date
                    'Developer Guide No. 178
                    ListView6Func.ListViewSortByDate(lvwTreaties, lColumnIndex, ListViewHelper.GetSortOrderProperty(lvwTreaties), True)
                Case Else
                    ListViewHelper.SetSortKeyProperty(lvwTreaties, lColumnIndex)
                    ListViewHelper.SetSortedProperty(lvwTreaties, True)
            End Select


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

    Private Sub chkHideDeleted_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkHideDeleted.CheckStateChanged
        ' Refresh the list
        If lvwTreaties.FocusedItem Is Nothing Then
            m_lReturn = BusinessToInterface()
        Else
            m_lReturn = CType(BusinessToInterface(gPMFunctions.ToSafeLong(Convert.ToString(lvwTreaties.FocusedItem.Tag))), gPMConstants.PMEReturnCode)
        End If
    End Sub

    Private Sub chkHideExpired_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkHideExpired.CheckStateChanged
        ' Refresh the list
        If lvwTreaties.FocusedItem Is Nothing Then
            m_lReturn = BusinessToInterface()
        Else
            m_lReturn = CType(BusinessToInterface(gPMFunctions.ToSafeLong(Convert.ToString(lvwTreaties.FocusedItem.Tag))), gPMConstants.PMEReturnCode)
        End If
    End Sub

    Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click

        Dim oForm As frmTreaty
        Dim oListItem As ListViewItem
        Dim lTreatyID As Integer
        Dim sCode, sDescription As String
        Dim dtEffectiveDate As Date
        Dim dtExpiryDate As Object
        Dim sAgreementCode As String = ""
        Dim lReinsuranceTypeID As Integer
        Dim sReinsuranceType As String = ""
        Dim lReplacesTreatyID As Object
        Dim sReplacesTreaty As String = ""
        Dim vTreatyParties As Object
        Dim dtReplacedEffectiveDt As Date
        Dim lReplacedByTreatyID As Object
        Dim lReplacedByTreaty As String = ""
        Dim vTreatyPartiesBrokerParticipants(,) As Object 'E005
        Dim lIndex As Integer
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "cmdAdd_Click"
        Dim dTreatylimit As Decimal = 0
        Dim lCurrencyId As Integer,lReinstatements As Integer
        

        Try

            ' Create treaty form
            oForm = New frmTreaty()
            oForm.Business = m_oBusiness
            oForm.frmTreatyLoad()
            ' Set properties
            lReturn = oForm.Clear()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("oForm.Clear", "Unable to set default properties on treaty dialog")
            End If
            'Developer Guide No. 24
            oForm.Treaties = m_vTreaties

            ' Show dialog
            oForm.ShowDialog()

            ' Check result
            If oForm.Status = gPMConstants.PMEReturnCode.PMOK Then
                ' Get results

                lReturn = CType(oForm.GetProperties(lTreatyID, sCode, sDescription, dtEffectiveDate, dtExpiryDate, sAgreementCode, lReinsuranceTypeID, sReinsuranceType, lReplacesTreatyID, sReplacesTreaty, vTreatyParties, dtReplacedEffectiveDt, lReplacedByTreatyID, lReplacedByTreaty,dTreatylimit, lCurrencyId, lReinstatements, vTreatyPartiesBrokerParticipants), gPMConstants.PMEReturnCode)

                ' Save data
                m_sUniqueId = GetUniqueID()
                m_sScreenHierarchy = $"Treaty({sCode})"
                lReturn = m_oBusiness.AddTreaty(r_lTreatyID:=lTreatyID, v_sCode:=sCode, v_sDescription:=sDescription, v_bIsDeleted:=0, v_dtEffectiveDate:=dtEffectiveDate, v_dtExpiryDate:=dtExpiryDate, v_sAgreementCode:=sAgreementCode, v_lReinsuranceTypeID:=lReinsuranceTypeID, v_lReplacesTreatyID:=lReplacesTreatyID, v_vTreatyParties:=vTreatyParties, v_dtReplacedEffectiveDt:=dtReplacedEffectiveDt, v_lReplacedByTreatyID:=lReplacedByTreatyID,v_dTreatyLimit:=dTreatylimit, v_lCurrencyID:=lCurrencyId,v_lReinstatements:=lReinstatements, v_vTreatyPartiesBrokerParticipants:=vTreatyPartiesBrokerParticipants, sUniqueId:=m_sUniqueId, sScreenHierarchy:=m_sScreenHierarchy)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oBusiness.AddTreaty", "Failed to add new treaty details")
                End If

                ' Increase array
                If Information.IsArray(m_vTreaties) Then
                    ReDim Preserve m_vTreaties(DBTMax, m_vTreaties.GetUpperBound(1) + 1)
                Else
                    ReDim m_vTreaties(DBTMax, 0)
                End If
                lIndex = m_vTreaties.GetUpperBound(1)

                ' Store results
                m_vTreaties(MainModule.TreatyEnum.DBTTreatyID, lIndex) = lTreatyID
                m_vTreaties(MainModule.TreatyEnum.DBTCode, lIndex) = sCode
                m_vTreaties(MainModule.TreatyEnum.DBTDescription, lIndex) = sDescription
                m_vTreaties(MainModule.TreatyEnum.DBTEffectiveDate, lIndex) = dtEffectiveDate

                m_vTreaties(MainModule.TreatyEnum.DBTExpiryDate, lIndex) = dtExpiryDate
                m_vTreaties(MainModule.TreatyEnum.DBTAgreementCode, lIndex) = sAgreementCode
                m_vTreaties(MainModule.TreatyEnum.DBTReinsuranceTypeID, lIndex) = lReinsuranceTypeID
                m_vTreaties(MainModule.TreatyEnum.DBTReinsuranceType, lIndex) = sReinsuranceType

                m_vTreaties(MainModule.TreatyEnum.DBTReplacesTreatyID, lIndex) = lReplacesTreatyID
                m_vTreaties(MainModule.TreatyEnum.DBTReplacesTreaty, lIndex) = sReplacesTreaty
                m_vTreaties(MainModule.TreatyEnum.DBTReplacedEffectiveDate, lIndex) = dtReplacedEffectiveDt

                m_vTreaties(MainModule.TreatyEnum.DBTTreatyLimit, lIndex) = dTreatylimit
                m_vTreaties(MainModule.TreatyEnum.DBTCurrencyID, lIndex) = lCurrencyId
                m_vTreaties(MainModule.TreatyEnum.DBTReinstatements, lIndex) = lReinstatements
                ' Refresh list
                lReturn = CType(BusinessToInterface(lIndex), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("BusinessToInterface(lIndex)", "Unable to refresh treaty list")
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

    Private Sub cmdClose_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdClose.Click

        Dim lReturn As Integer
        Const kMethodName As String = "cmdClose_Click"


        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions depending upon the interface task etc.
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)
        Finally



        End Try
        Exit Sub
    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click

        Dim oListItem As ListViewItem
        Dim lIndex As Integer
        Dim index As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "cmdDelete_Click"


        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check for active item
            If lvwTreaties.FocusedItem Is Nothing Then
                cmdDelete.Enabled = False
                cmdEdit.Enabled = False
            Else
                ' Get index of selected item
                If lvwTreaties.SelectedItems.Count > 0 Then
                    oListItem = lvwTreaties.SelectedItems(0)
                    index = oListItem.Index
                    lIndex = gPMFunctions.ToSafeLong(Convert.ToString(oListItem.Tag))
                Else
                    Exit Sub
                End If
                ' Delete or undelete the active treaty
                m_sUniqueId = GetUniqueID()
                m_sScreenHierarchy = $"Treaty({m_vTreaties(MainModule.TreatyEnum.DBTCode, lIndex)})"
                lReturn = m_oBusiness.DeleteTreaty(v_lTreatyID:=gPMFunctions.ToSafeLong(m_vTreaties(MainModule.TreatyEnum.DBTTreatyID, lIndex)), v_bIsDeleted:=Not gPMFunctions.ToSafeBoolean(m_vTreaties(MainModule.TreatyEnum.DBTIsDeleted, lIndex)), sUniqueId:=m_sUniqueId, sScreenHierarchy:=m_sScreenHierarchy)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oBusiness.DeleteTreaty", "Unable to delete/undelete treaty")
                End If

                ' We have updated the treaty so toggle the active treaty and refresh the list
                m_vTreaties(MainModule.TreatyEnum.DBTIsDeleted, lIndex) = Not gPMFunctions.ToSafeBoolean(m_vTreaties(MainModule.TreatyEnum.DBTIsDeleted, lIndex))
                lReturn = CType(BusinessToInterface(lIndex), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("BusinessToInterface", "Unable to refresh interface")
                End If
                lvwTreaties.FullRowSelect = True
                lvwTreaties.Items(index).Selected = True
                lvwTreaties.Select()
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

        Dim oForm As frmTreaty
        Dim oListItem As ListViewItem

        Dim lTreatyID As Integer
        Dim sCode, sDescription As String
        Dim dtEffectiveDate As Date
        Dim dtExpiryDate As Object
        Dim sAgreementCode As String = ""
        Dim lReinsuranceTypeID As Integer
        Dim sReinsuranceType As String = ""
        Dim lReplacesTreatyID As Object
        Dim sReplacesTreaty As String = ""
        Dim vTreatyParties As Object

        Dim lReplacedByTreatyID As Object
        Dim dtReplacedEffectiveDate As Date
        Dim lReplacedByTreaty As String = ""
        Dim vTreatyPartiesBrokerParticipants As Object  'E005
        Dim lIndex As Integer
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "cmdEdit_Click"
        Dim dTreatylimit As Decimal=0
        Dim lCurrencyId As Integer,lReinstatements As Integer

        Try
            ' Check for active item
            If lvwTreaties.SelectedItems.Count <= 0 Then
                cmdEdit.Enabled = False
                cmdDelete.Enabled = False
                Exit Sub
            End If

            ' Get index of selected item
            oListItem = lvwTreaties.SelectedItems(0)
            lIndex = gPMFunctions.ToSafeLong(Convert.ToString(oListItem.Tag))

            ' Check for deleted
            If gPMFunctions.ToSafeBoolean(m_vTreaties(MainModule.TreatyEnum.DBTIsDeleted, lIndex)) Then
                cmdEdit.Enabled = False
                Exit Sub
            End If

            ' Create treaty form
            oForm = New frmTreaty()
            oForm.Business = m_oBusiness

            oForm.frmTreatyLoad()

            ' Set properties
            lReturn = CType(oForm.SetProperties(m_vTreaties(MainModule.TreatyEnum.DBTTreatyID, lIndex), _
                                                m_vTreaties(MainModule.TreatyEnum.DBTCode, lIndex), _
                                                m_vTreaties(MainModule.TreatyEnum.DBTDescription, lIndex), _
                                                gPMFunctions.ToSafeDate(m_vTreaties(MainModule.TreatyEnum.DBTEffectiveDate, lIndex)), _
                                                m_vTreaties(MainModule.TreatyEnum.DBTExpiryDate, lIndex), _
                                                m_vTreaties(MainModule.TreatyEnum.DBTAgreementCode, lIndex), _
                                                gPMFunctions.ToSafeLong(m_vTreaties(MainModule.TreatyEnum.DBTReinsuranceTypeID, lIndex)), _
                                                gPMFunctions.ToSafeLong(m_vTreaties(MainModule.TreatyEnum.DBTReplacesTreatyID, lIndex)), _
                                                gPMFunctions.ToSafeDate(m_vTreaties(MainModule.TreatyEnum.DBTReplacedEffectiveDate, lIndex)), _
                                                gPMFunctions.ToSafeLong(m_vTreaties(MainModule.TreatyEnum.DBTReplacedByTreatyID, lIndex)), _
                                                gPMFunctions.ToSafeCurrency(m_vTreaties(MainModule.TreatyEnum.DBTTreatyLimit, lIndex)), _ 
                                                gPMFunctions.ToSafeInteger(m_vTreaties(MainModule.TreatyEnum.DBTCurrencyID, lIndex)), _  
                                                gPMFunctions.ToSafeLong(m_vTreaties(MainModule.TreatyEnum.DBTReinstatements, lIndex))), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("oForm.SetProperties", "Unable to set properties on treaty dialog")
            End If
            'Developer Guide No. 24
            oForm.Treaties = m_vTreaties

            ' Show dialog
            oForm.ShowDialog()

            ' Check result
            If oForm.Status = gPMConstants.PMEReturnCode.PMOK Then
                ' Get results

                lReturn = CType(oForm.GetProperties(lTreatyID, sCode, sDescription, _
                                                    dtEffectiveDate, dtExpiryDate, sAgreementCode, _
                                                    lReinsuranceTypeID, sReinsuranceType, lReplacesTreatyID, _
                                                    sReplacesTreaty, vTreatyParties, dtReplacedEffectiveDate, _
                                                    lReplacedByTreatyID, lReplacedByTreaty,dTreatylimit, lCurrencyId, lReinstatements, vTreatyPartiesBrokerParticipants), gPMConstants.PMEReturnCode)

                ' Save data
                m_sUniqueId = GetUniqueID()
                m_sScreenHierarchy = $"Treaty({sCode})"

                lReturn = m_oBusiness.UpdateTreaty(v_lTreatyID:=lTreatyID, v_sCode:=sCode, v_sDescription:=sDescription,
                                                   v_bIsDeleted:=0, v_dtEffectiveDate:=dtEffectiveDate, v_dtExpiryDate:=dtExpiryDate,
                                                   v_sAgreementCode:=sAgreementCode, v_lReinsuranceTypeID:=lReinsuranceTypeID, v_lReplacesTreatyID:=lReplacesTreatyID,
                                                   v_vTreatyParties:=vTreatyParties, v_dtReplacedEffectiveDate:=dtReplacedEffectiveDate, v_vlReplacedByTreatyID:=lReplacedByTreatyID,
                                                   v_dTreatyLimit:=dTreatylimit, v_lCurrencyID:= lCurrencyId, v_lReinstatements:= lReinstatements,
                                                   v_vTreatyPartiesBrokerParticipants:=vTreatyPartiesBrokerParticipants, sUniqueId:=m_sUniqueId, sScreenHierarchy:=m_sScreenHierarchy)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oBusiness.UpdateTreaty", "Failed to update treaty details")
                End If

                ' Store results
                m_vTreaties(MainModule.TreatyEnum.DBTTreatyID, lIndex) = lTreatyID
                m_vTreaties(MainModule.TreatyEnum.DBTCode, lIndex) = sCode
                m_vTreaties(MainModule.TreatyEnum.DBTDescription, lIndex) = sDescription
                m_vTreaties(MainModule.TreatyEnum.DBTEffectiveDate, lIndex) = dtEffectiveDate

                m_vTreaties(MainModule.TreatyEnum.DBTExpiryDate, lIndex) = dtExpiryDate
                m_vTreaties(MainModule.TreatyEnum.DBTAgreementCode, lIndex) = sAgreementCode
                m_vTreaties(MainModule.TreatyEnum.DBTReinsuranceTypeID, lIndex) = lReinsuranceTypeID
                m_vTreaties(MainModule.TreatyEnum.DBTReinsuranceType, lIndex) = sReinsuranceType

                m_vTreaties(MainModule.TreatyEnum.DBTReplacesTreatyID, lIndex) = lReplacesTreatyID
                m_vTreaties(MainModule.TreatyEnum.DBTReplacesTreaty, lIndex) = sReplacesTreaty
                m_vTreaties(MainModule.TreatyEnum.DBTReplacedEffectiveDate, lIndex) = dtReplacedEffectiveDate

                m_vTreaties(MainModule.TreatyEnum.DBTReplacedByTreatyID, lIndex) = lReplacedByTreatyID
                m_vTreaties(MainModule.TreatyEnum.DBTTreatyLimit, lIndex) = dTreatylimit
                m_vTreaties(MainModule.TreatyEnum.DBTCurrencyID, lIndex) = lCurrencyId
                m_vTreaties(MainModule.TreatyEnum.DBTReinstatements, lIndex) = lReinstatements

                ' Refresh list
                lReturn = CType(BusinessToInterface(lIndex), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("BusinessToInterface(lIndex)", "Unable to refresh treaty list")
                End If
            End If
        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn)
        Finally
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
        End Try
    End Sub

    Private Sub Form_Initialize_Renamed()

        Dim lReturn As Integer
        Const kMethodName As String = "Form_Initialize"


        Try

            ' Show form in task bar
            iPMFunc.ShowFormInTaskBar_Attach()

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRTreaty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("g_oObjectManager.GetInstance", "Unable to get instance of business object")
            End If

            ' Create an instance of the general interface object.
            m_oGeneral = New iPMUTreaty.General()

            ' Call the initialise method passing this interface and the business object as parameters.
            m_lReturn = CType(m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oGeneral.Initialise", "Unable to initialise General object")
            End If

            ' Set the interface status to cancelled. This is done so that any
            ' interface termination will be noted as cancelled except in the
            ' event of accepting the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

            ' Set error code
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

        Finally

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)



        End Try
        Exit Sub
    End Sub

    Public Sub frmInterfaceLoad()

        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "Form_Load"


        Try

            ' Show form in task bar
            iPMFunc.ShowFormInTaskBar_Detach()

            ' Check if we have had an error so far. Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error, so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the process modes for the busines object.

            lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oBusiness.SetProcessModes", "Failed to set the process modes for the business object")
            End If

            ' Set the interface default values.
            lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("SetInterfaceDefaults", "Failed to set interface default values")
            End If

            ' Gets the interface details to be displayed.
            lReturn = CType(m_oGeneral.GetInterfaceDetails(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oGeneral.GetInterfaceDetails", "Failed to get interface details")
            End If
            If lvwTreaties.Items.Count > 0 Then
                cmdEdit.Enabled = True
                cmdDelete.Enabled = True
                lvwTreaties.Items(0).Selected = True
            End If
        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

            ' Set error code
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

        Finally
            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)



        End Try
    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        Dim lReturn As Integer
        Const kMethodName As String = "Form_QueryUnload"


        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.


            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending upon the interface task etc.
                lReturn = m_oGeneral.ProcessCommand()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    eventArgs.cancel = True
                    Exit Sub
                End If
            End If

            ' Terminate the general object.
            m_oGeneral.Dispose()
            m_oGeneral = Nothing

            ' Terminate the business object

            m_oBusiness.Dispose()
            m_oBusiness = Nothing


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally
            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)




        End Try
        Exit Sub
    End Sub

    Private isInitializingComponent As Boolean
    Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If

        Try

            ' Move the listview and buttons
            chkHideExpired.Left = ClientRectangle.Width - (chkHideExpired.Width + VB6.TwipsToPixelsX(60))
            chkHideDeleted.Left = chkHideExpired.Left - (chkHideDeleted.Width + VB6.TwipsToPixelsX(240))
            lvwTreaties.SetBounds(VB6.TwipsToPixelsX(60), VB6.TwipsToPixelsY(405), ClientRectangle.Width - VB6.TwipsToPixelsX(120), ClientRectangle.Height - VB6.TwipsToPixelsY(855))
            cmdAdd.SetBounds(VB6.TwipsToPixelsX(60), ClientRectangle.Height - VB6.TwipsToPixelsY(390), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            cmdEdit.SetBounds(VB6.TwipsToPixelsX(120 + 1095), ClientRectangle.Height - VB6.TwipsToPixelsY(390), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            cmdDelete.SetBounds(VB6.TwipsToPixelsX(180 + 2190), ClientRectangle.Height - VB6.TwipsToPixelsY(390), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            cmdClose.SetBounds(ClientRectangle.Width - VB6.TwipsToPixelsX(1155), ClientRectangle.Height - VB6.TwipsToPixelsY(390), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)

        Catch exc As System.Exception
        End Try
    End Sub

    Private Sub lvwTreaties_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwTreaties.ColumnClick
        ListViewFunc.SortListView(lvwTreaties, eventArgs)
    End Sub

    Private Sub lvwTreaties_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwTreaties.DoubleClick
        cmdEdit_Click(cmdEdit, New EventArgs())
    End Sub



    Private Sub lvwTreaties_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwTreaties.SelectedIndexChanged
        Dim lIndex
        Const kMethodName As String = "lvwTreaties_ItemClick"


        Try
            If lvwTreaties.SelectedItems.Count <= 0 Then
                Exit Sub
            End If
            ' Get index of selected item
            lIndex = gPMFunctions.ToSafeLong(Convert.ToString(lvwTreaties.SelectedItems(0).Tag))

            ' Set appropriate caption on delete button
            cmdDelete.Text = IIf(gPMFunctions.ToSafeBoolean(m_vTreaties(MainModule.TreatyEnum.DBTIsDeleted, lIndex)), "Un&delete", "&Delete")

            ' Set enabled states
            cmdEdit.Enabled = Not gPMFunctions.ToSafeBoolean(m_vTreaties(MainModule.TreatyEnum.DBTIsDeleted, lIndex))
            cmdDelete.Enabled = True


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally
            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)



        End Try
        Exit Sub
    End Sub
End Class
