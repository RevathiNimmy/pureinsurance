Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports System.Windows.Forms
Imports SharedFiles

Partial Friend Class frmColumns
    Inherits System.Windows.Forms.Form

    ' ***************************************************************** '
    ' Module Name: frmColumns
    '
    ' Date: 28/06/2002
    '
    ' Description:  This will shows columns for import
    '
    ' Edit History:
    '   28/06/2002 SJP  - Tidied up after merge from Carole Nash
    ' ***************************************************************** '

    Private vData(,) As Object
    Private bLargeFileFlag As Boolean
    Private m_lFileEntries As Integer


    Private m_lSelectedItem As Integer

    Dim FrmImport As FrmImport
    Dim frmView As frmView
    Dim frmColumns As frmColumns
    ' ***************************************************************** '
    '
    ' Name: cmdCancel_Click()
    '
    ' Description:  This will cancel the button
    '
    ' History: 28/06/2002 SJP - tidied up
    '
    ' ***************************************************************** '
    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        Try

            Me.Close()

        Catch excep As System.Exception


            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cancel", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdcancel_click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    ' ***************************************************************** '
    '
    ' Name: PopListTypes()
    '
    ' Description:
    '
    ' History: 28/06/2002 SJP - tidied up
    '          07/01/2003 APS - Amended to handle large files
    '
    ' ***************************************************************** '
    Private Sub cmdImport_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdImport.Click

        Dim vFields As Object
        Dim strFileName, sFieldName As String

        Dim fso As Object
        Dim file As FileInfo
        Dim dStart, dEnd As Date
        Dim sTempFolder As String = "c:\temp\"

        Try

            'validate input
            For i As Integer = 1 To lvwColumns.Items.Count
                If lvwColumns.Items.Item(i - 1).Text.Substring(0, 1) = "?" Or lvwColumns.Items.Item(i - 1).Text = "" Then

                    MessageBox.Show("You must add columns names for the extra fields", "List Management", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub

                End If
            Next i

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            If vData.GetUpperBound(0) > 1 Then
                'get EXTRA column names from screen
                ReDim vFields(vData.GetUpperBound(0) - 2)

                With lvwColumns

                    For i As Integer = 3 To vData.GetUpperBound(0) + 1

                        sFieldName = .Items.Item(i - 1).Text.Replace(kSelectorChar, "").Trim()


                        vFields(i - 3) = "[" & sFieldName & "]"

                    Next i

                End With
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            'Reset audit trail flag for new list creation process
            m_oBusiness.AuditTrailCreated = False

            '   Creates lookup
            m_oBusiness.UniqueId = GetUniqueID()
            m_lReturn = m_oBusiness.createPMlookup("UDL_" & FrmImport.cboListType.Text.TrimEnd(), vFields)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception("1, Failed to create table")
            Else
                MessageBox.Show("Created Lookup Table", "List Management", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            dStart = DateTime.Now
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
            ProgressBar.Value = 0
            ProgressBar.Visible = True

            Dim lProcessedEntries As Integer
            If bLargeFileFlag Then



                fso = New Object()

                'reset the array
                vData = Nothing

                'get the next file
                strFileName = FileSystem.Dir(sTempFolder & "splitfile*.000", FileAttribute.Normal)
                'm_lReturn = m_oBusiness.ImportList("c:\temp\splitfile000.000", vData)

                Do
                    If m_bIsServer Then
                        'get the first block of data

                        m_lReturn = m_oBusiness.ImportList(sTempFolder & strFileName, vData)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                            Throw New System.Exception("1, cmdUpdate, Failed to read from file")
                        End If
                    Else
                        m_lReturn = CType(ImportList(sTempFolder & strFileName, vData), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                            Throw New System.Exception("1, cmdUpdate, Failed to read from file")
                        End If
                    End If

                    If Information.IsArray(vData) Then

                        For i As Integer = 0 To vData.GetUpperBound(1)
                            ProgressBar.Value = (100 * lProcessedEntries) \ m_lFileEntries
                            lProcessedEntries += 1

                            If Not (m_oBusiness.ListItemExists("UDL_" & FrmImport.cboListType.Text.TrimEnd(), CStr(vData(0, i)), ToSafeLong(FrmImport.cboListVersion.Text)) = 1) Then

                                m_lReturn = m_oBusiness.addlistentry("UDL_" & FrmImport.cboListType.Text.TrimEnd(), vData, i, CDate(FrmImport.txtEffectiveDate.Text), ToSafeLong(FrmImport.cboListVersion.Text))
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                                    Throw New System.Exception("1, cmdimport, Failed to add list entry")
                                End If


                                m_lReturn = m_oBusiness.addusage("UDL_" & FrmImport.cboListType.Text.TrimEnd(), CStr(vData(0, i)).TrimEnd(), CInt(FrmImport.cboListVersion.Text), CDate(FrmImport.txtEffectiveDate.Text))
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                                    Throw New System.Exception("1, cmdimport, Failed to add list entry usage")
                                End If
                            Else

                                m_lReturn = m_oBusiness.UpdateListEntry("UDL_" & FrmImport.cboListType.Text.TrimEnd(), vData, i, CDate(FrmImport.txtEffectiveDate.Text))
                                'add to usage table and rating structure

                                m_lReturn = m_oBusiness.addusage("UDL_" & FrmImport.cboListType.Text.TrimEnd(), CStr(vData(0, i)), CInt(FrmImport.cboListVersion.Text), CDate(FrmImport.txtEffectiveDate.Text))

                                'check for errors
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                                    Throw New System.Exception("1, cmdimport, Failed to add item to rating")
                                End If

                            End If

                        Next i
                    End If

                    file = New FileInfo(sTempFolder & strFileName)
                    file.Delete()

                    strFileName = FileSystem.Dir(sTempFolder & "splitfile*.000", FileAttribute.Normal)

                Loop While strFileName <> ""

                fso = Nothing
                file = Nothing


            Else
                For i As Integer = 0 To vData.GetUpperBound(1)
                    If vData.GetUpperBound(1) > 0 Then
                        ProgressBar.Value = (100 * i) \ vData.GetUpperBound(1)
                    Else
                        ProgressBar.Value = (100 * i) \ 1
                    End If



                    If Not (m_oBusiness.ListItemExists("UDL_" & FrmImport.cboListType.Text.TrimEnd(), CStr(vData(0, i))) = 1) Then


                        m_lReturn = m_oBusiness.addlistentry("UDL_" & FrmImport.cboListType.Text.TrimEnd(), vData, i, CDate(FrmImport.txtEffectiveDate.Text), ToSafeLong(FrmImport.cboListVersion.Text))
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                            Throw New System.Exception("1, cmdimport, Failed to add list entry")
                        End If


                        m_lReturn = m_oBusiness.addusage("UDL_" & FrmImport.cboListType.Text.TrimEnd(), CStr(vData(0, i)).TrimEnd(), CInt(FrmImport.cboListVersion.Text), CDate(FrmImport.txtEffectiveDate.Text))
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                            Throw New System.Exception("1, cmdimport, Failed to add list entry usage")
                        End If

                    Else

                        m_lReturn = m_oBusiness.UpdateListEntry("UDL_" & FrmImport.cboListType.Text.TrimEnd(), vData, i, CDate(FrmImport.txtEffectiveDate.Text))
                        'add to usage table and rating structure

                        m_lReturn = m_oBusiness.addusage("UDL_" & FrmImport.cboListType.Text.TrimEnd(), CStr(vData(0, i)), CInt(FrmImport.cboListVersion.Text), CDate(FrmImport.txtEffectiveDate.Text))

                        'check for errors
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                            Throw New System.Exception("1, cmdimport, Failed to add item to rating")
                        End If

                    End If

                Next i
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception("1, cmdImport, Failed to import data")
            End If

            dEnd = DateTime.Now
            MessageBox.Show("Imported Data. (" & DateTime.FromOADate((dEnd - dStart).TotalDays).Minute & " minutes " & CStr(DateTime.FromOADate((dEnd - dStart).TotalDays).Second) & " seconds)", "List Management", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Me.Close()

        Catch excep As System.Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to import list", vApp:=ACApp, vClass:=ACClass, vMethod:="CmdImport_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub
        End Try

    End Sub

    ' ***************************************************************** '
    '
    ' Name: cmdView_Click()
    '
    ' Description:
    '
    ' History: 28/06/2002 SJP - tidied up
    '
    ' ***************************************************************** '
    Private Sub cmdView_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdView.Click

        Try

            'check for errors
            If Information.IsArray(vData) Then
                frmView = New frmView()
                frmView.SetData(vData)
                frmView.ShowDialog()
            Else
                MessageBox.Show("No data found", "List Management", MessageBoxButtons.OK)
            End If

        Catch excep As System.Exception


            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Falied to view file", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdview_click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    ' ***************************************************************** '
    '
    ' Name: Form_Load()
    '
    ' Description:
    '
    ' History: 28/06/2002 SJP - tidied up
    '
    ' ***************************************************************** '


    Private Sub frmColumns_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Dim oListItem As ListViewItem
        Dim lFileLength As Integer
        Dim sTempFolder As String = "c:\temp\"
        'Dim FrmImport As New FrmImport
        Try
            FrmImport = Me.Owner
            'set UI
            ProgressBar.Visible = False

            'Import the list
            lFileLength = GetFileLength(FrmImport.txtfile.Text)
            If lFileLength > conMaxRecords Then
                bLargeFileFlag = True
                m_lFileEntries = lFileLength
                deleteSplitFiles()
                splitfile(FrmImport.txtfile.Text, lFileLength)

                If m_bIsServer Then
                    'get the first block of data

                    m_lReturn = m_oBusiness.ImportList(sTempFolder & "splitfile000.000", vData)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception("1, cmdUpdate, Failed to read from file - c:\temp\splitfile000.000")
                    End If
                Else
                    m_lReturn = CType(ImportList(sTempFolder & "splitfile000.000", vData), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception("1, cmdUpdate, Failed to read from file - c:\temp\splitfile000.000")
                    End If
                End If
            Else
                bLargeFileFlag = False
                'New code Added

                If m_bIsServer Then
                    'get the first block of data

                    m_lReturn = m_oBusiness.ImportList(FrmImport.txtfile.Text, vData)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception("1, cmdUpdate, Failed to read from file")
                    End If
                Else
                    m_lReturn = CType(ImportList(FrmImport.txtfile.Text, vData), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or (Not Information.IsArray(vData)) Then
                        Throw New System.Exception("1, cmdUpdate, Failed to read from file")
                    End If
                End If

            End If

            If Information.IsArray(vData) Then
                'do nothing
            Else
                MessageBox.Show("No data found", "List Management", MessageBoxButtons.OK)
            End If

            'setup columns
            With lvwColumns

                .Columns.Clear()
                .Columns.Add("Column Name", 94)
                .Columns.Add("Position", 94)
                .Columns.Add("Editable", CInt((0)))

                'add standard stuff
                oListItem = .Items.Add("Code")
                ListViewHelper.GetListViewSubItem(oListItem, 1).Text = "1"
                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(False)
                oListItem = .Items.Add("Description")
                ListViewHelper.GetListViewSubItem(oListItem, 1).Text = "2"
                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(False)


                .View = View.Details
                .GridLines = True

                'add extra fields
                For i As Integer = 2 To vData.GetUpperBound(0)
                    oListItem = .Items.Add("?" & New String(" "c, 100) & kSelectorChar)
                    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(i + 1)
                    ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(True)
                Next i

            End With

        Catch excep As System.Exception


            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load form", vApp:=ACApp, vClass:=ACClass, vMethod:="Form Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    Private Sub SelectItem()


        ' Const kGridTop As Integer = 480 / 15
        Const kGridAdjustment As Integer = 40 / 15
        Const kGridLeft As Integer = 120 / 15

        ' if no item is selected via click method
        ' must be via keyboard
        If m_lSelectedItem = 0 Then
            ' so get the selected item
            m_lSelectedItem = lvwColumns.FocusedItem.Index + 1
        End If

        ' if there is still no selected item then exit
        If m_lSelectedItem = 0 Then
            Exit Sub
        End If

        ' get the selected item
        Dim oListItem As ListViewItem = lvwColumns.Items.Item(m_lSelectedItem)

        ' get the items position


        'Developer Guide No. 221

        Dim lItemLeft As Integer = CInt(oListItem.Position.X)



        'Developer Guide No. 221

        Dim lItemTop As Integer = CInt(oListItem.Position.Y)

        Dim lWidth As Integer = CInt((lvwColumns.Columns.Item(0).Width))


        Dim lHeight As Integer = 255 / 15

        ' place text box on top of the listview
        ' to get details for new caption
        txtColumnHeader.Tag = CStr(oListItem.Index)
        txtColumnHeader.Left = (lItemLeft + kGridLeft + kGridAdjustment)
        txtColumnHeader.Top = (lItemTop + kGridAdjustment)
        txtColumnHeader.Width = (lWidth)
        txtColumnHeader.Height = (lHeight)

        If oListItem.Text.Substring(0, 1) = "?" Then
            txtColumnHeader.Text = "?"
        Else
            txtColumnHeader.Text = oListItem.Text.Replace(kSelectorChar, "").Trim()
        End If

        ' place the text box above the list view and select the contents
        ' ready for replacement
        txtColumnHeader.Visible = True
        txtColumnHeader.BringToFront()
        txtColumnHeader.SelectionLength = Strings.Len(txtColumnHeader.Text)
        txtColumnHeader.Focus()

        ' enable / disable data entry into the text box as appropriate
        'txtColumnHeader.ReadOnly = (Not CBool(ListViewHelper.GetListViewSubItem(lvwColumns.Items.Item(m_lSelectedItem - 1), 2).Text))
        txtColumnHeader.ReadOnly = (Not CBool(ListViewHelper.GetListViewSubItem(lvwColumns.Items.Item(m_lSelectedItem), 2).Text))

    End Sub

    Private Sub lvwColumns_ItemClick(ByVal Item As ListViewItem)
        'm_lSelectedItem = Item.Index + 1
        m_lSelectedItem = Item.Index
    End Sub

    Private Sub lvwColumns_KeyUp(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles lvwColumns.KeyUp
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        If eventArgs.KeyCode = Keys.Up Then
            SelectPreviousItem()
        End If

        If eventArgs.KeyCode = Keys.Down Then
            SelectNextItem()
        End If

    End Sub

    Private Sub lvwColumns_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwColumns.MouseUp
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = (eventArgs.X)
        Dim y As Single = (eventArgs.Y)
        ' if the item is editable
        SelectItem()
    End Sub

    Private Sub txtColumnHeader_KeyUp(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles txtColumnHeader.KeyUp
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        If eventArgs.KeyCode = Keys.Up Then
            SelectPreviousItem()
        End If

        If eventArgs.KeyCode = Keys.Down Then
            SelectNextItem()
        End If

    End Sub

    Private Sub txtColumnHeader_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtColumnHeader.Leave

        ' save the last item away
        RefreshListItem()

        ' reset the text box and hide it
        txtColumnHeader.Tag = CStr(0)
        txtColumnHeader.Visible = False
        txtColumnHeader.SendToBack()

    End Sub

    Private Sub RefreshListItem()

        ' save user entry back to list view
        If CDbl(Convert.ToString(txtColumnHeader.Tag)) <> 0 Then
            'lvwColumns.Items.Item(CInt(Convert.ToString(txtColumnHeader.Tag)) - 1).Text = txtColumnHeader.Text & New String(" "c, 100) & kSelectorChar
            lvwColumns.Items.Item(CInt(Convert.ToString(txtColumnHeader.Tag))).Text = txtColumnHeader.Text & New String(" "c, 100) & kSelectorChar
        End If

    End Sub

    Private Sub SelectPreviousItem()

        ' if the current item is the first item
        If m_lSelectedItem = 1 Then
            ' do nothing
        Else
            RefreshListItem()
            ' select the previous item in the list
            m_lSelectedItem -= 1
            SelectItem()
        End If

    End Sub

    Private Sub SelectNextItem()

        ' if the current item is the last item
        If m_lSelectedItem = lvwColumns.Items.Count Then
            ' do nothing
        Else

            RefreshListItem()
            ' select the next item in the list
            m_lSelectedItem += 1
            SelectItem()
        End If

    End Sub
End Class