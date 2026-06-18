Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Windows.Forms
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
    Private m_oGeneral As iPMURIModel.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Stores the return value for the a function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' RI Model array
    Private m_vRIModels(,) As Object
    'Start( Sriram )Tech Spec - Calliden WR3.2.1.2 (1) - RI Model Line Priority.doc sec(6.2.3.1)
    Private m_vRIModelLines As Object
    'End( Sriram )Tech Spec - Calliden WR3.2.1.2 (1) - RI Model Line Priority.doc sec(6.2.3.1)
    Private m_vRIModelLinesVariableQuotaShare As Object


    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As gPMConstants.PMEReturnCode
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As gPMConstants.PMENavigateButtonStatus
    Private m_lProcessMode As gPMConstants.PMEProcessMode
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sUniqueId As String
    Private m_sScreenHierarchy As String

    'Developer Guide No.7
    Private Const vbFormCode As Integer = 0

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
            lvwRIModel.Items.Clear()

            ' Check for items (we may not have any yet)
            If Information.IsArray(m_vRIModels) Then
                ' Process all treaties
                For lCount As Integer = m_vRIModels.GetLowerBound(1) To m_vRIModels.GetUpperBound(1)
                    If (chkHideDeleted.CheckState = CheckState.Unchecked Or Not gPMFunctions.ToSafeBoolean(m_vRIModels(MainModule.RIModelEnum.DBMIsDeleted, lCount))) And (chkHideExpired.CheckState = CheckState.Unchecked Or gPMFunctions.ToSafeDate(m_vRIModels(MainModule.RIModelEnum.DBMExpiryDate, lCount), DateTime.Today.AddDays(1)) > DateTime.Today) Then
                        ' Add the list item
                        oListItem = lvwRIModel.Items.Add("M" & CStr(m_vRIModels(MainModule.RIModelEnum.DBMRIModelID, lCount)), CStr(m_vRIModels(MainModule.RIModelEnum.DBMCode, lCount)).Trim(), "")

                        ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vRIModels(MainModule.RIModelEnum.DBMDescription, lCount))
                        ListViewHelper.GetListViewSubItem(oListItem, 2).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateShort, m_vRIModels(MainModule.RIModelEnum.DBMEffectiveDate, lCount))
                        ListViewHelper.GetListViewSubItem(oListItem, 3).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateShort, m_vRIModels(MainModule.RIModelEnum.DBMExpiryDate, lCount))
                        ListViewHelper.GetListViewSubItem(oListItem, 4).Text = CStr(Interaction.Choose(CInt(CDbl(m_vRIModels(MainModule.RIModelEnum.DBMRIModelType, lCount)) + 1), "Standard", "Default", "Deferred", "Excess of Loss", "Cloned"))

                        ' Gray out deleted items
                        If gPMFunctions.ToSafeBoolean(m_vRIModels(MainModule.RIModelEnum.DBMIsDeleted, lCount)) Then

                            'Developer Guide No. 12 - no solutions
                            oListItem.ForeColor = Color.Gray
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
                            lvwRIModel.FullRowSelect = True
                            lvwRIModel.Items(oListItem.Index).Selected = True
                            lvwRIModel.Select()
                            lvwRIModel_SelectedIndexChanged(lvwRIModel, New EventArgs())

                        End If
                    End If
                Next lCount
            End If

            ' Ignore errors this is only a cosmetic nicety
            lReturn = CType(ListView6Func.ListViewAutoSize(lvwRIModel, True, True, Me), gPMConstants.PMEReturnCode)

            ' Refresh sort order
            SortList(ListViewHelper.GetSortKeyProperty(lvwRIModel), True)

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

            lReturn = m_oBusiness.GetRIModels(r_vRIModel:=m_vRIModels)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oBusiness.GetRIModels", "Unable to get ri model list")
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
                If ListViewHelper.GetSortKeyProperty(lvwRIModel) = lColumnIndex Then
                    ListViewHelper.SetSortOrderProperty(lvwRIModel, IIf(ListViewHelper.GetSortOrderProperty(lvwRIModel) = SortOrder.Ascending, SortOrder.Descending, SortOrder.Ascending))
                Else
                    ListViewHelper.SetSortOrderProperty(lvwRIModel, SortOrder.Ascending)
                End If
            End If

            ' Sort based on contents
            Select Case lColumnIndex
                Case 2, 3 ' Date
                    ListView6Func.ListViewSortByValue(lvwRIModel, lColumnIndex, ListViewHelper.GetSortOrderProperty(lvwRIModel), True)
                Case Else
                    ListViewHelper.SetSortKeyProperty(lvwRIModel, lColumnIndex)
                    ListViewHelper.SetSortedProperty(lvwRIModel, True)
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
        If lvwRIModel.FocusedItem Is Nothing Then
            m_lReturn = BusinessToInterface()
        Else
            m_lReturn = CType(BusinessToInterface(gPMFunctions.ToSafeLong(Convert.ToString(lvwRIModel.FocusedItem.Tag))), gPMConstants.PMEReturnCode)
        End If
    End Sub

    Private Sub chkHideExpired_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkHideExpired.CheckStateChanged
        ' Refresh the list
        If lvwRIModel.FocusedItem Is Nothing Then
            m_lReturn = BusinessToInterface()
        Else
            m_lReturn = CType(BusinessToInterface(gPMFunctions.ToSafeLong(Convert.ToString(lvwRIModel.FocusedItem.Tag))), gPMConstants.PMEReturnCode)
        End If
    End Sub

    Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click

        Dim oForm As frmRIModel
        Dim oListItem As ListViewItem

        Dim lRIModelID As Integer
        Dim sCode, sDescription As String
        Dim dtEffectiveDate As Date
        Dim dtExpiryDate As Object
        Dim iRIModelType, iFACPremiumType, iClaimAllocationType As Integer
        Dim lCurrencyID As Integer
        Dim sCurrency As String = ""
        Dim lXOLClmRIModelID As Integer
        Dim cXOLClmLimit As Decimal
        Dim lXOLCatRIModelID As Integer
        Dim cXOLCatLimit As Decimal
        Dim iXOLCatReinstatements As Integer
        Dim vRIModelLines As Object
        Dim vRIModelLinesVariableQuotaShare As Object
        Dim iTreatyPremiumType As Integer
        Dim lIndex As Integer
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "cmdAdd_Click"


        Try

            ' Create ri model form
            oForm = New frmRIModel()
            oForm.Business = m_oBusiness

            'Load(oForm)
            oForm.LoadForm()
            ' Set properties
            lReturn = oForm.Clear()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("oForm.Clear", "Unable to set default properties on ri model dialog")
            End If

            ' Show dialog
            oForm.RIModels = VB6.CopyArray(m_vRIModels)
            oForm.ShowDialog()

            ' Check result
            If oForm.Status = gPMConstants.PMEReturnCode.PMOK Then
                ' Get results

                lReturn = CType(oForm.GetProperties(lRIModelID, sCode, sDescription, dtEffectiveDate, dtExpiryDate, iRIModelType, iFACPremiumType, iClaimAllocationType, lCurrencyID, sCurrency, lXOLClmRIModelID, cXOLClmLimit, lXOLCatRIModelID, cXOLCatLimit, iXOLCatReinstatements, vRIModelLines, iTreatyPremiumType, vRIModelLinesVariableQuotaShare), gPMConstants.PMEReturnCode)

                ' Save data
                m_sUniqueId = GetUniqueID()
                m_sScreenHierarchy = $"RI Model({sCode})"

                lReturn = m_oBusiness.AddRIModel(r_lRIModelID:=lRIModelID, v_sCode:=sCode, v_sDescription:=sDescription, v_bIsDeleted:=0, v_dtEffectiveDate:=dtEffectiveDate, v_dtExpiryDate:=dtExpiryDate, v_iRIModelType:=iRIModelType, v_iFACPremiumType:=iFACPremiumType, v_iClaimAllocationType:=iClaimAllocationType, v_lCurrencyID:=lCurrencyID, v_lXOLClmRIModelID:=lXOLClmRIModelID, v_cXOLClmLimit:=cXOLClmLimit, v_lXOLCatRIModelID:=lXOLCatRIModelID, v_cXOLCatLimit:=cXOLCatLimit, v_iXOLCatReinstatements:=iXOLCatReinstatements, v_vRIModelLines:=vRIModelLines, sUniqueId:=m_sUniqueId, sScreenHierarchy:=m_sScreenHierarchy, v_iTreatyPremiumType:=iTreatyPremiumType, v_vRIModelLinesVariableQuotaShare:=vRIModelLinesVariableQuotaShare)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oBusiness.AddRIModel", "Failed to add new ri model details")
                End If

                ' Increase array
                If Information.IsArray(m_vRIModels) Then
                    ReDim Preserve m_vRIModels(DBMMax, m_vRIModels.GetUpperBound(1) + 1)
                Else
                    ReDim m_vRIModels(DBMMax, 1)
                End If
                lIndex = m_vRIModels.GetUpperBound(1)

                ' Store results
                m_vRIModels(MainModule.RIModelEnum.DBMRIModelID, lIndex) = lRIModelID
                m_vRIModels(MainModule.RIModelEnum.DBMCode, lIndex) = sCode
                m_vRIModels(MainModule.RIModelEnum.DBMDescription, lIndex) = sDescription
                m_vRIModels(MainModule.RIModelEnum.DBMIsDeleted, lIndex) = 0
                m_vRIModels(MainModule.RIModelEnum.DBMEffectiveDate, lIndex) = dtEffectiveDate

                m_vRIModels(MainModule.RIModelEnum.DBMExpiryDate, lIndex) = dtExpiryDate
                m_vRIModels(MainModule.RIModelEnum.DBMRIModelType, lIndex) = iRIModelType
                m_vRIModels(MainModule.RIModelEnum.DBMFACPremiumType, lIndex) = iFACPremiumType
                m_vRIModels(MainModule.RIModelEnum.DBMClaimAllocationType, lIndex) = iClaimAllocationType
                m_vRIModels(MainModule.RIModelEnum.DBMCurrencyID, lIndex) = lCurrencyID
                m_vRIModels(MainModule.RIModelEnum.DBMCurrencyDescription, lIndex) = sCurrency
                m_vRIModels(MainModule.RIModelEnum.DBMXOLClmRIModelID, lIndex) = lXOLClmRIModelID
                m_vRIModels(MainModule.RIModelEnum.DBMXOLClmLimit, lIndex) = cXOLClmLimit
                m_vRIModels(MainModule.RIModelEnum.DBMXOLCatRIModelID, lIndex) = lXOLCatRIModelID
                m_vRIModels(MainModule.RIModelEnum.DBMXOLCatLimit, lIndex) = cXOLCatLimit
                m_vRIModels(MainModule.RIModelEnum.DBMXOLCatReinstatements, lIndex) = iXOLCatReinstatements
                m_vRIModels(MainModule.RIModelEnum.DBMTreatyPremiumType, lIndex) = iTreatyPremiumType

                ' Refresh list
                lReturn = CType(BusinessToInterface(lIndex), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("BusinessToInterface(lIndex)", "Unable to refresh ri model list")
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
        Dim index As Integer = 0
        Dim lIndex As Integer
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "cmdDelete_Click"


        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check for active item
            If lvwRIModel.FocusedItem Is Nothing Then
                cmdDelete.Enabled = False
                cmdEdit.Enabled = False
            Else
                ' Get index of selected item
                oListItem = lvwRIModel.FocusedItem
                index = oListItem.Index
                lIndex = gPMFunctions.ToSafeLong(Convert.ToString(oListItem.Tag))

                ' Delete or undelete the active ri model
                m_sUniqueId = GetUniqueID()
                m_sScreenHierarchy = $"RI Model({m_vRIModels(MainModule.RIModelEnum.DBMCode, lIndex)})"
                lReturn = m_oBusiness.DeleteRIModel(v_lRIModelID:=gPMFunctions.ToSafeLong(m_vRIModels(MainModule.RIModelEnum.DBMRIModelID, lIndex)), v_bIsDeleted:=Not gPMFunctions.ToSafeBoolean(m_vRIModels(MainModule.RIModelEnum.DBMIsDeleted, lIndex)), sUniqueId:=m_sUniqueId, sScreenHierarchy:=m_sScreenHierarchy)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oBusiness.DeleteRIModel", "Unable to delete/undelete ri model")
                End If

                ' We have updated the ri model so toggle the active ri model and refresh the list
                m_vRIModels(MainModule.RIModelEnum.DBMIsDeleted, lIndex) = Not gPMFunctions.ToSafeBoolean(m_vRIModels(MainModule.RIModelEnum.DBMIsDeleted, lIndex))
                lReturn = CType(BusinessToInterface(lIndex), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("BusinessToInterface", "Unable to refresh interface")
                End If
                lvwRIModel.FullRowSelect = True
                lvwRIModel.Items(index).Selected = True
                lvwRIModel.Select()
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

        Dim oForm As frmRIModel
        Dim oFormRI As frmRIModelLine
        Dim oListItem As ListViewItem

        Dim lRIModelID As Integer
        Dim sCode, sDescription As String
        Dim dtEffectiveDate As Date
        Dim dtExpiryDate As Object
        Dim iRIModelType, iFACPremiumType, iClaimAllocationType As Integer
        Dim lCurrencyID As Integer
        Dim sCurrency As String = ""
        Dim lXOLClmRIModelID As Integer
        Dim cXOLClmLimit As Decimal
        Dim lXOLCatRIModelID As Integer
        Dim cXOLCatLimit As Decimal
        Dim iXOLCatReinstatements As Integer
        Dim vRIModelLines As Object
        Dim iTreatyPremiumType As Integer
        Dim vRIModelLinesVariableQuotaShare As Object
        Dim lIndex As Integer
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "cmdEdit_Click"


        Try

            ' Check for active item
            If lvwRIModel.SelectedItems.Count < 0 Then
                cmdEdit.Enabled = False
                cmdDelete.Enabled = False
                Exit Sub
            End If

            ' Get index of selected item
            oListItem = lvwRIModel.SelectedItems(0)
            lIndex = gPMFunctions.ToSafeLong(Convert.ToString(oListItem.Tag))

            ' Check for deleted
            If gPMFunctions.ToSafeBoolean(m_vRIModels(MainModule.RIModelEnum.DBMIsDeleted, lIndex)) Then
                cmdEdit.Enabled = False
                Exit Sub
            End If

            ' Create ri model form


            oForm = New frmRIModel()
            oForm.Business = m_oBusiness
            oForm.RIModels = VB6.CopyArray(m_vRIModels)

            'Load(oForm)

            oForm.LoadForm()

            ' Set properties
            'lReturn = CType(oForm.SetProperties(CInt(m_vRIModels(MainModule.RIModelEnum.DBMRIModelID, lIndex)), CStr(m_vRIModels(MainModule.RIModelEnum.DBMCode, lIndex)), CStr(m_vRIModels(MainModule.RIModelEnum.DBMDescription, lIndex)), CDate(m_vRIModels(MainModule.RIModelEnum.DBMEffectiveDate, lIndex)), gPMFunctions.ToSafeDate(m_vRIModels(MainModule.RIModelEnum.DBMExpiryDate, lIndex)), CInt(m_vRIModels(MainModule.RIModelEnum.DBMRIModelType, lIndex)), CInt(m_vRIModels(MainModule.RIModelEnum.DBMFACPremiumType, lIndex)), CInt(m_vRIModels(MainModule.RIModelEnum.DBMClaimAllocationType, lIndex)), CInt(m_vRIModels(MainModule.RIModelEnum.DBMCurrencyID, lIndex)), gPMFunctions.ToSafeLong(m_vRIModels(MainModule.RIModelEnum.DBMXOLClmRIModelID, lIndex)), gPMFunctions.ToSafeCurrency(m_vRIModels(MainModule.RIModelEnum.DBMXOLClmLimit, lIndex)), gPMFunctions.ToSafeLong(m_vRIModels(MainModule.RIModelEnum.DBMXOLCatRIModelID, lIndex)), gPMFunctions.ToSafeCurrency(m_vRIModels(MainModule.RIModelEnum.DBMXOLCatLimit, lIndex)), gPMFunctions.ToSafeInteger(m_vRIModels(MainModule.RIModelEnum.DBMXOLCatReinstatements, lIndex))), gPMConstants.PMEReturnCode)
            lReturn = CType(oForm.SetProperties(CInt(m_vRIModels(MainModule.RIModelEnum.DBMRIModelID, lIndex)), CStr(m_vRIModels(MainModule.RIModelEnum.DBMCode, lIndex)), CStr(m_vRIModels(MainModule.RIModelEnum.DBMDescription, lIndex)), CDate(m_vRIModels(MainModule.RIModelEnum.DBMEffectiveDate, lIndex)), m_vRIModels(MainModule.RIModelEnum.DBMExpiryDate, lIndex), CInt(m_vRIModels(MainModule.RIModelEnum.DBMRIModelType, lIndex)), CInt(m_vRIModels(MainModule.RIModelEnum.DBMFACPremiumType, lIndex)), CInt(m_vRIModels(MainModule.RIModelEnum.DBMClaimAllocationType, lIndex)), CInt(m_vRIModels(MainModule.RIModelEnum.DBMCurrencyID, lIndex)), gPMFunctions.ToSafeLong(m_vRIModels(MainModule.RIModelEnum.DBMXOLClmRIModelID, lIndex)), gPMFunctions.ToSafeCurrency(m_vRIModels(MainModule.RIModelEnum.DBMXOLClmLimit, lIndex)), gPMFunctions.ToSafeLong(m_vRIModels(MainModule.RIModelEnum.DBMXOLCatRIModelID, lIndex)), gPMFunctions.ToSafeCurrency(m_vRIModels(MainModule.RIModelEnum.DBMXOLCatLimit, lIndex)), gPMFunctions.ToSafeInteger(m_vRIModels(MainModule.RIModelEnum.DBMXOLCatReinstatements, lIndex)), gPMFunctions.ToSafeInteger(m_vRIModels(MainModule.RIModelEnum.DBMTreatyPremiumType, lIndex))), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("oForm.SetProperties", "Unable to set properties on ri model dialog")
            End If

            ' Show dialog
            m_sUniqueId = GetUniqueID()
            oForm.UniqueId = m_sUniqueId
            oForm.ShowDialog()

            ' Check result
            If oForm.Status = gPMConstants.PMEReturnCode.PMOK Then
                ' Get results

                lReturn = CType(oForm.GetProperties(lRIModelID, sCode, sDescription, dtEffectiveDate, dtExpiryDate, iRIModelType, iFACPremiumType, iClaimAllocationType, lCurrencyID, sCurrency, lXOLClmRIModelID, cXOLClmLimit, lXOLCatRIModelID, cXOLCatLimit, iXOLCatReinstatements, vRIModelLines, iTreatyPremiumType, vRIModelLinesVariableQuotaShare), gPMConstants.PMEReturnCode)
                m_sScreenHierarchy = $"RI Model({sCode})"
                ' Save data
                ' Save data

                lReturn = m_oBusiness.UpdateRIModel(v_lRIModelID:=lRIModelID, v_sCode:=sCode, v_sDescription:=sDescription, v_bIsDeleted:=0, v_dtEffectiveDate:=dtEffectiveDate, v_dtExpiryDate:=dtExpiryDate, v_iRIModelType:=iRIModelType, v_iFACPremiumType:=iFACPremiumType, v_iClaimAllocationType:=iClaimAllocationType, v_lCurrencyID:=lCurrencyID, v_lXOLClmRIModelID:=lXOLClmRIModelID, v_cXOLClmLimit:=cXOLClmLimit, v_lXOLCatRIModelID:=lXOLCatRIModelID, v_cXOLCatLimit:=cXOLCatLimit, v_iXOLCatReinstatements:=iXOLCatReinstatements, v_vRIModelLines:=vRIModelLines, sUniqueId:=m_sUniqueId, sScreenHierarchy:=m_sScreenHierarchy, v_iTreatyPremiumType:=iTreatyPremiumType, v_vRIModelLinesVariableQuotaShare:=vRIModelLinesVariableQuotaShare)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oBusiness.UpdateRIModel", "Failed to update ri model details")
                End If

                ' Store results
                m_vRIModels(MainModule.RIModelEnum.DBMRIModelID, lIndex) = lRIModelID
                m_vRIModels(MainModule.RIModelEnum.DBMCode, lIndex) = sCode
                m_vRIModels(MainModule.RIModelEnum.DBMDescription, lIndex) = sDescription
                m_vRIModels(MainModule.RIModelEnum.DBMIsDeleted, lIndex) = 0
                m_vRIModels(MainModule.RIModelEnum.DBMEffectiveDate, lIndex) = dtEffectiveDate

                m_vRIModels(MainModule.RIModelEnum.DBMExpiryDate, lIndex) = dtExpiryDate
                m_vRIModels(MainModule.RIModelEnum.DBMRIModelType, lIndex) = iRIModelType
                m_vRIModels(MainModule.RIModelEnum.DBMFACPremiumType, lIndex) = iFACPremiumType
                m_vRIModels(MainModule.RIModelEnum.DBMClaimAllocationType, lIndex) = iClaimAllocationType
                m_vRIModels(MainModule.RIModelEnum.DBMCurrencyID, lIndex) = lCurrencyID
                m_vRIModels(MainModule.RIModelEnum.DBMCurrencyDescription, lIndex) = sCurrency
                m_vRIModels(MainModule.RIModelEnum.DBMXOLClmRIModelID, lIndex) = lXOLClmRIModelID
                m_vRIModels(MainModule.RIModelEnum.DBMXOLClmLimit, lIndex) = cXOLClmLimit
                m_vRIModels(MainModule.RIModelEnum.DBMXOLCatRIModelID, lIndex) = lXOLCatRIModelID
                m_vRIModels(MainModule.RIModelEnum.DBMXOLCatLimit, lIndex) = cXOLCatLimit
                m_vRIModels(MainModule.RIModelEnum.DBMXOLCatReinstatements, lIndex) = iXOLCatReinstatements
                m_vRIModels(MainModule.RIModelEnum.DBMTreatyPremiumType, lIndex) = iTreatyPremiumType

                ' Refresh list
                lReturn = CType(BusinessToInterface(lIndex), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("BusinessToInterface(lIndex)", "Unable to refresh ri model list")
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
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRRIModel.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("g_oObjectManager.GetInstance", "Unable to get instance of business object")
            End If

            ' Create an instance of the general interface object.
            m_oGeneral = New iPMURIModel.General()

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


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

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
            If lvwRIModel.Items.Count > 0 Then
                cmdEdit.Enabled = True
                cmdDelete.Enabled = True
            End If
            chkHideDeleted.Focus()
            chkHideDeleted.Select()

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

            ' Set error code
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

        Finally
            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)



        End Try
        Exit Sub
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
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                lReturn = m_oGeneral.ProcessCommand()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    'Developer Guide No. 7
                    eventArgs.Cancel = True
                    Cancel = 1
                    Exit Sub
                End If
            End If

            ' Terminate the general object.
            m_oGeneral.Dispose()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
            End If
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



            eventArgs.Cancel = Cancel <> 0
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
            lvwRIModel.SetBounds(VB6.TwipsToPixelsX(60), VB6.TwipsToPixelsY(405), ClientRectangle.Width - VB6.TwipsToPixelsX(120), ClientRectangle.Height - VB6.TwipsToPixelsY(855))
            cmdAdd.SetBounds(VB6.TwipsToPixelsX(60), ClientRectangle.Height - VB6.TwipsToPixelsY(390), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            cmdEdit.SetBounds(VB6.TwipsToPixelsX(120 + 1095), ClientRectangle.Height - VB6.TwipsToPixelsY(390), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            cmdDelete.SetBounds(VB6.TwipsToPixelsX(180 + 2190), ClientRectangle.Height - VB6.TwipsToPixelsY(390), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            cmdClose.SetBounds(ClientRectangle.Width - VB6.TwipsToPixelsX(1155), ClientRectangle.Height - VB6.TwipsToPixelsY(390), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)

        Catch exc As System.Exception
            NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try
    End Sub

    Private Sub lvwRIModel_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwRIModel.ColumnClick
        'Dim ColumnHeader As ColumnHeader = lvwRIModel.Columns(eventArgs.Column)
        'SortList(ColumnHeader.Index + 1 - 1)
        ListViewFunc.SortListView(lvwRIModel, eventArgs)
    End Sub

    Private Sub lvwRIModel_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwRIModel.DoubleClick
        cmdEdit_Click(cmdEdit, New EventArgs())
    End Sub

    Private Sub lvwRIModel_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwRIModel.SelectedIndexChanged
        Dim lIndex, lReturn As Integer
        Const kMethodName As String = "lvwRIModel_ItemClick"


        Try
            If lvwRIModel.FocusedItem Is Nothing Then
                Exit Sub
            End If


            ' Get index of selected item
            lIndex = gPMFunctions.ToSafeLong(Convert.ToString(lvwRIModel.FocusedItem.Tag))

            ' Set appropriate caption on delete button
            cmdDelete.Text = IIf(gPMFunctions.ToSafeBoolean(m_vRIModels(MainModule.RIModelEnum.DBMIsDeleted, lIndex)), "Un&delete", "&Delete")

            ' Set enabled states
            cmdEdit.Enabled = Not gPMFunctions.ToSafeBoolean(m_vRIModels(MainModule.RIModelEnum.DBMIsDeleted, lIndex))
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
