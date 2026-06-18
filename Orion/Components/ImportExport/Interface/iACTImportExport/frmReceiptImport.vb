Option Strict Off
Option Explicit On

Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports SharedFiles

Partial Friend Class frmReceiptImport
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
    Private Const ACClass As String = "frmReceiptImport"
    Private Const vbFormCode As Integer = 0
    ' Stores the return value for the a function call.
    Private m_lReturn As Integer

    ' Preview details
    Private m_vHeader() As Object
    Private m_vDetail(,) As Object
    Private m_bIsDirty As Boolean
    Private m_oBusiness As Object

    Private m_sPath As String = ""
    Private m_sFilename As String = ""

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As gPMConstants.PMEReturnCode
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_bXmlUpdate As Boolean

    Private Enum ReceiptPreviewHeaderEnum
        RPHBatchReference
        RPHBankAccount
        RPHDate
        RPHCurrency
        RPHReceiptTotal
        RPHExpectedTotal
        RPHTotalRecords
        RPHExpectedRecords
        RPHInvalidRecords
    End Enum


    ' ***************************************************************** '
    '                          PUBLIC METHODS
    ' ***************************************************************** '

    ' ***************************************************************** '
    ' Name: Start
    '
    ' Description: Starts the form and returns it's response
    ' ***************************************************************** '
    Public Function Start(ByVal v_oBusiness As Object, ByVal v_sPath As String, ByVal v_sFilename As String, Optional ByVal Index As Integer = -1) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "Start"


        Try

            ' Set default response
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Store details
            m_oBusiness = v_oBusiness
            m_sPath = v_sPath
            m_sFilename = v_sFilename

            ' Set interface defaults
            SetInterfaceDefaults()





            ' Get the record preview

            lReturn = m_oBusiness.GetRecordPreview(v_sPath:=v_sPath, v_sFilename:=v_sFilename, r_vHeader:=m_vHeader, r_vDetail:=m_vDetail)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oBusiness.GetRecordPreview", "Unable to get record preview details")
            End If

            ' Populate dialog
            lReturn = BusinessToInterface()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("BusinessToInterface", "Unable to display business details")
            End If

            ' Validate initial data
            ValidateImport()
            If Index = 1 Then
                cmdOK.Enabled = False
            End If
            ' Show dialog
            Me.ShowDialog()

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally
            ' Release this form
            Me.Close()

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    '                         PRIVATE METHODS
    ' ***************************************************************** '
    ' ***************************************************************** '
    ' Name: BusinessToInterface
    '
    ' Description: Updates all interface details from the business object.
    ' ***************************************************************** '
    Private Function BusinessToInterface() As Integer

        Dim result As Integer = 0
        Dim lCount As Integer
        Dim oListItem As ListViewItem
        Dim oListSubItem As ListViewItem.ListViewSubItem

        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "BusinessToInterface"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Populate header information
            If Information.IsArray(m_vHeader) Then
                txtBatchReference.Text = gPMFunctions.ToSafeString(CStr(m_vHeader(ReceiptPreviewHeaderEnum.RPHBatchReference))).Trim()
                txtDate.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateShort, CStr(m_vHeader(ReceiptPreviewHeaderEnum.RPHDate)).Substring(0, 10))
                txtBankAccount.Text = gPMFunctions.ToSafeString(CStr(m_vHeader(ReceiptPreviewHeaderEnum.RPHBankAccount))).Trim()
                txtCurrency.Text = gPMFunctions.ToSafeString(CStr(m_vHeader(ReceiptPreviewHeaderEnum.RPHCurrency))).Trim()
                txtReceiptTotal.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(m_vHeader(ReceiptPreviewHeaderEnum.RPHReceiptTotal)))
                txtExpectedTotal.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(m_vHeader(ReceiptPreviewHeaderEnum.RPHExpectedTotal)))
                txtTotalRecords.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatLong, CStr(m_vHeader(ReceiptPreviewHeaderEnum.RPHTotalRecords)))
                txtExpectedRecords.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatLong, CStr(m_vHeader(ReceiptPreviewHeaderEnum.RPHExpectedRecords)))
                txtInvalidRecords.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatLong, CStr(m_vHeader(ReceiptPreviewHeaderEnum.RPHInvalidRecords)))
            End If

            ' Populate the grid
            lReturn = CType(PopulateListView(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("PopulateListView", "Unable to display import folder details")
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally




        End Try
        Return result
    End Function


    Private Function PopulateListView() As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem
        Dim oListSubItem As ListViewItem.ListViewSubItem

        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "PopulateListView"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the list before we start
            lvwReceipts.Items.Clear()

            ' Process all items
            If Information.IsArray(m_vDetail) Then
                For lCount As Integer = m_vDetail.GetLowerBound(1) To m_vDetail.GetUpperBound(1)
                    ' Add the list item
                    oListItem = lvwReceipts.Items.Add(gPMFunctions.ToSafeString(CStr(m_vDetail(0, lCount))).Trim())
                    ' Populate sub items
                    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(m_vDetail(1, lCount)))
                    ListViewHelper.GetListViewSubItem(oListItem, 2).Text = gPMFunctions.ToSafeString(CStr(m_vDetail(2, lCount))).Trim()
                    ListViewHelper.GetListViewSubItem(oListItem, 3).Text = gPMFunctions.ToSafeString(CStr(m_vDetail(3, lCount))).Trim()

                    ' Check for invalid client codes
                    If gPMFunctions.ToSafeString(CStr(m_vDetail(0, lCount))).Trim() = gPMFunctions.ToSafeString(CStr(m_vDetail(4, lCount))).Trim() Then
                        ' Codes match, line is invalid
                        oListItem.ForeColor = Color.Red
                        oListItem.Font = VB6.FontChangeBold(oListItem.Font, True)
                    Else
                        ListViewHelper.GetListViewSubItem(oListItem, 4).Text = gPMFunctions.ToSafeString(CStr(m_vDetail(4, lCount))).Trim()
                    End If

                    ListViewHelper.GetListViewSubItem(oListItem, 5).Text = gPMFunctions.ToSafeString(CStr(m_vDetail(5, lCount))).Trim()
                    ListViewHelper.GetListViewSubItem(oListItem, 6).Text = gPMFunctions.ToSafeString(CStr(m_vDetail(6, lCount))).Trim()

                    ' Set tag so we can trace back to array line
                    oListItem.Tag = CStr(lCount)
                Next lCount
            End If

            ' Ignore errors this is only a cosmetic nicety

            lReturn = CType(ListView6Func.ListViewAutoSize(lvwReceipts, True, True, Me), gPMConstants.PMEReturnCode)

            ' Refresh sort order
            SortList(lvwReceipts, ListViewHelper.GetSortKeyProperty(lvwReceipts), True)

            ' Highlight first item in list
            If Not (lvwReceipts.FocusedItem Is Nothing) Then
                lvwReceipts.FocusedItem.Selected = False
            End If
            If lvwReceipts.Items.Count Then
                lvwReceipts.Items.Item(0).Selected = True
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally




        End Try
        Return result
    End Function

    Private Function ProcessImport() As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "ProcessImport"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Process the import file

            lReturn = m_oBusiness.ProcessManualImport(v_sFilename:=m_sFilename)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oBusiness.ProcessManualImport", "Unable to process import file")
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally




        End Try
        Return result
    End Function

    Private Function SaveChanges() As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Const kMethodName As String = "SaveChanges"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if we have any changes to save (just in case)
            If m_bIsDirty Or m_bXmlUpdate Then
                ' Return updated data to save back to xml

                lReturn = m_oBusiness.UpdateRecordPreview(v_sPath:=m_sPath, v_sFilename:=m_sFilename, v_vHeader:=m_vHeader, v_vDetail:=m_vDetail)

                ' Refresh dirty status and update command buttons
                m_bIsDirty = False
                ValidateImport()
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally




        End Try
        Return result
    End Function

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

            ' Set command states
            cmdLookupAccount.Enabled = False
            cmdFixExpected.Enabled = False

            cmdOK.Enabled = False
            cmdApply.Enabled = False


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally




        End Try
        Return result
    End Function

    Private Function SortList(ByVal oListView As ListView, ByVal lColumnIndex As Integer, Optional ByVal bReSort As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Const kMethodName As String = "SortList"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' We may just be refreshing after a item edit or addition
            If Not bReSort Then
                ' Reverse sort order if column hasn't changed
                If ListViewHelper.GetSortKeyProperty(oListView) = lColumnIndex Then
                    ListViewHelper.SetSortOrderProperty(oListView, IIf(ListViewHelper.GetSortOrderProperty(oListView) = SortOrder.Ascending, SortOrder.Descending, SortOrder.Ascending))
                Else
                    ListViewHelper.SetSortOrderProperty(oListView, SortOrder.Ascending)
                End If
            End If

            ' Sort based on contents
            Select Case lColumnIndex
                Case 1 ' Value

                    ListView6Func.ListViewSortByValue(oListView, lColumnIndex, ListViewHelper.GetSortOrderProperty(oListView), True)
                Case Else
                    ListViewHelper.SetSortKeyProperty(oListView, lColumnIndex)
                    ListViewHelper.SetSortedProperty(oListView, True)
            End Select


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally




        End Try
        Return result
    End Function

    Private Sub ValidateImport()


        Try

            ' Check if fix values button should be enabled
            cmdFixExpected.Enabled = (gPMFunctions.ToSafeCurrency(txtExpectedRecords.Text) <> gPMFunctions.ToSafeCurrency(txtTotalRecords.Text)) Or (gPMFunctions.ToSafeCurrency(txtExpectedTotal.Text) <> gPMFunctions.ToSafeCurrency(txtReceiptTotal.Text))

            ' Header must balance and invalid records must be 0
            If (gPMFunctions.ToSafeCurrency(txtExpectedRecords.Text) = gPMFunctions.ToSafeCurrency(txtTotalRecords.Text)) And (gPMFunctions.ToSafeCurrency(txtExpectedTotal.Text) = gPMFunctions.ToSafeCurrency(txtReceiptTotal.Text)) And (gPMFunctions.ToSafeLong(txtInvalidRecords.Text) = 0) Then
                cmdOK.Enabled = False
            Else
                cmdOK.Enabled = True
            End If
            cmdApply.Enabled = m_bIsDirty

        Catch exc As System.Exception
            NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try

    End Sub


    ' ***************************************************************** '
    '                             EVENTS
    ' ***************************************************************** '
    Private Sub cmdApply_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdApply.Click

        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "cmdApply_Click"


        Try

            ' Save any changes
            lReturn = CType(SaveChanges(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("SaveChanges()", "Unable to save changes to import file")
            End If

            ' Hide dialog
            Me.Hide()


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally


        End Try
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        Dim lReturn As Integer
        Const kMethodName As String = "cmdCancel_Click"


        Try

            ' Check if we have made any changes
            If m_bIsDirty Then
                ' We have, prompt...
                Select Case MessageBox.Show("You have made changes. Do you wish to save?", "Import Review", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
                    Case System.Windows.Forms.DialogResult.Yes
                        ' Delegate to cmdSave
                        cmdApply_Click(cmdApply, New EventArgs())
                    Case System.Windows.Forms.DialogResult.Cancel
                        ' Cancel, return to dialog
                        Exit Sub
                End Select
            End If

            ' Hide dialog
            Me.Hide()


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally


        End Try
    End Sub

    Private Sub cmdFixExpected_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFixExpected.Click
        ' Set expected values to actuals
        m_vHeader(ReceiptPreviewHeaderEnum.RPHExpectedRecords) = m_vHeader(ReceiptPreviewHeaderEnum.RPHTotalRecords)
        m_vHeader(ReceiptPreviewHeaderEnum.RPHExpectedTotal) = m_vHeader(ReceiptPreviewHeaderEnum.RPHReceiptTotal)
        m_bIsDirty = True

        ' Update interface
        txtExpectedTotal.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(m_vHeader(ReceiptPreviewHeaderEnum.RPHExpectedTotal)))
        txtExpectedRecords.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatLong, CStr(m_vHeader(ReceiptPreviewHeaderEnum.RPHExpectedRecords)))

        ' Check if we are ok to import now
        ValidateImport()
    End Sub

    Private Sub cmdLookupAccount_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdLookupAccount.Click
        Dim iACTFindAccount As Object

        Dim oSelectedItem As ListViewItem

        Dim oInterface As iACTFindAccount.Interface_Renamed
        Dim vKeyArray(,) As Object

        Dim lReturn As Integer
        Const kMethodName As String = "cmdLookupAccount_Click"


        Try

            ' Check for selected account
            If lvwReceipts.FocusedItem Is Nothing Then
                cmdLookupAccount.Enabled = False
                Exit Sub
            End If

            ' Ensure this item is currently invalid
            If Not lvwReceipts.FocusedItem.Font.Bold Then
                cmdLookupAccount.Enabled = False
                Exit Sub
            End If

            ' Get selected item
            oSelectedItem = lvwReceipts.FocusedItem

            ' Get an instance of Find Account
            Dim temp_oInterface As Object
            lReturn = g_oObjectManager.GetInstance(temp_oInterface, sClassName:="iACTFindAccount.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oInterface = temp_oInterface
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("g_oObjectManager.GetInstance", "Unable to get instance of iACTFindAccount.Interface")
            End If

            ' Populate key array

            ReDim vKeyArray(1, 1)

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyAllowStoppedAccounts

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = True

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameShortCode

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = oSelectedItem.Text

            ' Pass to find account

            m_lReturn = oInterface.SetKeys(vKeyArray)

            ' Setting the NotEditable property of FindAccount to avoid editing

            oInterface.NotEditable = True

            ' Start the find interface

            lReturn = oInterface.Start()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("oInterface.Start()", "Unable to start the Find Account interface")
            End If

            ' If they didn't cancel then store the new data

            If oInterface.Status <> gPMConstants.PMEReturnCode.PMCancel Then
                ' Write new account code back and reset formatting
                oSelectedItem.Font = VB6.FontChangeBold(oSelectedItem.Font, False)
                oSelectedItem.ForeColor = SystemColors.WindowText
                ListViewHelper.GetListViewSubItem(oSelectedItem, 4).Text = oSelectedItem.Text

                oSelectedItem.Text = oInterface.ShortCode

                ' Store back to array

                m_vDetail(0, gPMFunctions.ToSafeLong(Convert.ToString(oSelectedItem.Tag))) = oInterface.ShortCode

                ' Reset button status
                cmdLookupAccount.Enabled = False

                ' Decrement the invalid account counter
                txtInvalidRecords.Text = CStr(gPMFunctions.ToSafeLong(txtInvalidRecords.Text) - 1)
            End If

            ' Validate the current recordset
            ValidateImport()
            m_bXmlUpdate = True
            'm_bIsDirty = True


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally
            ' Terminate the find interface
            If Not (oInterface Is Nothing) Then

                oInterface.Dispose()
            End If
            oInterface = Nothing

        End Try
    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click ' Import

        Dim sPrompt As String = ""
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "cmdOK_Click"


        Try

            ' Check if we have made any changes
            If m_bIsDirty Then
                sPrompt = "Save and import this file?"
            Else
                sPrompt = "Import this file?"
            End If

            ' Set mouse pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)



            ' We have, prompt...
            Select Case MessageBox.Show(sPrompt, "Import Review", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                Case System.Windows.Forms.DialogResult.Yes
                    ' Delegate to cmdApply (this will only save if dirty)
                    cmdOK.Enabled = False
                    lReturn = CType(SaveChanges(), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("SaveChanges()", "Unable to save changes to import file")
                    End If

                    ' Import file
                    lReturn = CType(ProcessImport(), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("ProcessImport()", "Unable to process import file")
                    End If
                    cmdOK.Enabled = True
                    ' Hide dialog
                    Me.Hide()

                Case System.Windows.Forms.DialogResult.Cancel
                    ' Cancel, return to dialog
                    Exit Sub
            End Select


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally
            ' Reset mouse pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        End Try
    End Sub

    Private Sub Form_Initialize_Renamed()

        Dim lReturn As Integer
        Const kMethodName As String = "Form_Initialize"


        Try

            ' Show form in task bar
            iPMFunc.ShowFormInTaskBar_Attach()


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

            ' Set error code
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

        Finally


        End Try
    End Sub


    Private Sub frmReceiptImport_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Dim lReturn As Integer
        Const kMethodName As String = "Form_Load"


        Try

            ' Show form in task bar
            iPMFunc.ShowFormInTaskBar_Detach()


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally


        End Try
    End Sub

    Private Sub frmReceiptImport_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        Dim lReturn As Integer
        Const kMethodName As String = "Form_QueryUnload"


        Try

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.


            If UnloadMode <> vbFormCode Then

                ' Check if we have made any changes
                'If m_bIsDirty Then
                '    Select Case MsgBox("You have made changes. Do you wish to save?", vbYesNoCancel + vbQuestion, "Import Review")
                '        Case vbYes
                '            m_lStatus = ACDRSave
                '        Case vbNo
                '            m_lStatus = ACDRClose
                '        Case vbCancel
                '            ' Cancel, return to dialog
                '            Cancel = 1
                '            Exit Sub
                '    End Select
                'End If
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally
        End Try
    End Sub

    Private isInitializingComponent As Boolean
    Private Sub frmReceiptImport_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        ' Move the listview and buttons
        '    tabIE.Move 60, 60, ScaleWidth - 120, ScaleHeight - 510
        '    lvwImport.Move 90, 390, tabIE.Width - 180, tabIE.Height - 480
        '    lvwImported.Move 90, 390, tabIE.Width - 180, tabIE.Height - 480
        '    lvwExport.Move 90, 390, tabIE.Width - 180, tabIE.Height - 480
        '    cmdRefresh.Move 60, ScaleHeight - 390
        '    cmdClose.Move ScaleWidth - 1155, ScaleHeight - 390
        '
        'Catch exc As System.Exception
        'NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        'End Try
    End Sub

    Private Sub lvwReceipts_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwReceipts.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwReceipts.Columns(eventArgs.Column)
        SortList(lvwReceipts, ColumnHeader.Index + 1 - 1)
    End Sub

    Private Sub lvwReceipts_ItemClick(ByVal Item As ListViewItem)
        ' If the active item is bold it's invalid, allow it to be fixed..
        cmdLookupAccount.Enabled = Item.Font.Bold
    End Sub

    Private Sub txtExpectedRecords_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtExpectedRecords.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        If gPMFunctions.ToSafeCurrency(txtExpectedRecords.Text) <> gPMFunctions.ToSafeCurrency(txtTotalRecords.Text) Then
            txtExpectedRecords.Font = VB6.FontChangeBold(txtExpectedRecords.Font, True)
            txtExpectedRecords.ForeColor = Color.Red
        Else
            txtExpectedRecords.Font = VB6.FontChangeBold(txtExpectedRecords.Font, False)
            txtExpectedRecords.ForeColor = SystemColors.WindowText
        End If
    End Sub

    Private Sub txtExpectedTotal_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtExpectedTotal.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        If gPMFunctions.ToSafeCurrency(txtExpectedTotal.Text) <> gPMFunctions.ToSafeCurrency(txtReceiptTotal.Text) Then
            txtExpectedTotal.Font = VB6.FontChangeBold(txtExpectedTotal.Font, True)
            txtExpectedTotal.ForeColor = Color.Red
        Else
            txtExpectedTotal.Font = VB6.FontChangeBold(txtExpectedTotal.Font, False)
            txtExpectedTotal.ForeColor = SystemColors.WindowText
        End If
    End Sub

    Private Sub txtInvalidRecords_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInvalidRecords.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        If gPMFunctions.ToSafeLong(txtInvalidRecords.Text) <> 0 Then
            txtInvalidRecords.Font = VB6.FontChangeBold(txtInvalidRecords.Font, True)
            txtInvalidRecords.ForeColor = Color.Red
        Else
            txtInvalidRecords.Font = VB6.FontChangeBold(txtInvalidRecords.Font, False)
            txtInvalidRecords.ForeColor = SystemColors.WindowText
        End If
    End Sub
End Class
