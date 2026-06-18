Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Public Class frmSearch
    Inherits System.Windows.Forms.Form

    Private m_oSearchList As ListView
    Private m_lStatus As gPMConstants.PMEReturnCode

    Private m_lReturn As Integer
    Private m_lFindNext As gPMConstants.PMEReturnCode


    Public WriteOnly Property SearchList() As ListView
        Set(ByVal Value As ListView)
            m_oSearchList = Value
        End Set
    End Property

    Public Property Status() As Integer
        Get
            Return m_lStatus
        End Get
        Set(ByVal Value As Integer)
            m_lStatus = Value
        End Set
    End Property

    Private Sub cmdExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

    Private Sub cmdSelect_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSelect.Click
        If Not IsNothing(m_oSearchList.FocusedItem) Then
            m_oSearchList.FocusedItem.Checked = Not m_oSearchList.FocusedItem.Checked
        End If
    End Sub

    Private Sub cmdSearch_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSearch.Click

        Dim lFound, lColumnStart, lColumnEnd, lRecordStart, lRecordEnd, lRecordStep As Integer
        Dim sSearchText As String = ""
        Dim lCurrentPos As Integer

        Dim oListItem As ListViewItem

        Try

            If m_oSearchList.Items.Count < 1 Then
                stbMain.Items.Item("Message").Text = "Ready"
                stbMain.Items.Item("Found").Text = "Not Found"
                Exit Sub
            End If

            If Me.txtSearchValue.Text = "" Then
                MessageBox.Show("Please enter search value", "Transaction", MessageBoxButtons.OK)
                Exit Sub
            End If

            If Me.cboSearchColumn.Text = "" Then
                MessageBox.Show("Please select search column", "Transaction", MessageBoxButtons.OK)
                Exit Sub
            End If

            stbMain.Items.Item("Message").Text = "Searching..."
            stbMain.Items.Item("Found").Text = ""

            sSearchText = txtSearchValue.Text.Trim()
            'search columns
            If VB6.GetItemData(cboSearchColumn, cboSearchColumn.SelectedIndex) = -1 Then
                lColumnStart = 0

                lColumnEnd = m_oSearchList.Columns.Count - 1
            Else
                lColumnStart = VB6.GetItemData(cboSearchColumn, cboSearchColumn.SelectedIndex) - 1
                lColumnEnd = VB6.GetItemData(cboSearchColumn, cboSearchColumn.SelectedIndex) - 1
            End If

            lCurrentPos = m_oSearchList.FocusedItem.Index + 1

            'are we searching up or down
            If optSearchDirection(0).Checked Then
                'searching down-wards
                If m_lFindNext = gPMConstants.PMEReturnCode.PMTrue Then
                    lRecordStart = lCurrentPos + 1
                Else
                    lRecordStart = lCurrentPos
                End If

                lRecordEnd = m_oSearchList.Items.Count

                lRecordStep = 1
            Else
                'searching up-wards
                If m_lFindNext = gPMConstants.PMEReturnCode.PMTrue Then
                    lRecordStart = lCurrentPos - 1
                Else
                    lRecordStart = lCurrentPos
                End If

                lRecordEnd = 1

                lRecordStep = -1
            End If

            'search thro records and columns
            lFound = -1
            For lRecordLoop As Integer = lRecordStart To lRecordEnd Step lRecordStep

                oListItem = m_oSearchList.Items.Item(lRecordLoop - 1)

                For lColumnLoop As Integer = lColumnStart To lColumnEnd

                    If lColumnLoop = 0 Then
                        If chkPerfectMatch.CheckState Then
                            If oListItem.Text.Trim() = sSearchText Then
                                lFound = lRecordLoop
                                Exit For
                            End If
                        Else
                            If (oListItem.Text.IndexOf(sSearchText, StringComparison.CurrentCultureIgnoreCase) + 1) <> 0 Then
                                lFound = lRecordLoop
                                Exit For
                            End If
                        End If
                    Else
                        If chkPerfectMatch.CheckState Then
                            If ListViewHelper.GetListViewSubItem(oListItem, lColumnLoop).Text.Trim() = sSearchText Then
                                lFound = lRecordLoop
                                Exit For
                            End If
                        Else
                            If (ListViewHelper.GetListViewSubItem(oListItem, lColumnLoop).Text.IndexOf(sSearchText, StringComparison.CurrentCultureIgnoreCase) + 1) <> 0 Then
                                lFound = lRecordLoop
                                Exit For
                            End If
                        End If
                    End If
                Next

                If lFound <> -1 Then
                    Exit For
                End If
            Next

            If lFound <> -1 Then
                m_lFindNext = gPMConstants.PMEReturnCode.PMTrue
                stbMain.Items.Item("Message").Text = "Ready"
                stbMain.Items.Item("Found").Text = "Found"

                m_oSearchList.Items.Item(lFound - 1).Selected = True

                oListItem.EnsureVisible()
            Else
                '        If m_lFindNext = pmtrue Then
                '            m_oSearchList.ListItems(lCurrentPos).Selected = True
                '
                '            Set oListItem = m_oSearchList.ListItems(lCurrentPos)
                '            oListItem.EnsureVisible
                '        End If

                stbMain.Items.Item("Message").Text = "Ready"
                stbMain.Items.Item("Found").Text = "Not Found"
            End If

            m_oSearchList.Refresh()

            If (lFound > -1 And lFound < m_oSearchList.Items.Count) Then
                m_oSearchList.FocusedItem = m_oSearchList.Items.Item(lFound - 1)
            End If
            oListItem = Nothing

        Catch
            Status = gPMConstants.PMEReturnCode.PMFalse

            stbMain.Items.Item("Message").Text = "Ready"
            stbMain.Items.Item("Found").Text = "Error"
        End Try
    End Sub


    Private Sub frmSearch_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load


        Status = gPMConstants.PMEReturnCode.PMTrue

        'load search column
        cboSearchColumn.Items.Clear()
        Dim cboSearchColumn_NewIndex As Integer = -1
        cboSearchColumn_NewIndex = cboSearchColumn.Items.Add("All")
        VB6.SetItemData(cboSearchColumn, cboSearchColumn_NewIndex, -1)


        For lCount As Integer = 1 To m_oSearchList.Columns.Count
            cboSearchColumn_NewIndex = cboSearchColumn.Items.Add(m_oSearchList.Columns.Item(lCount - 1).Text)
            VB6.SetItemData(cboSearchColumn, cboSearchColumn_NewIndex, lCount)
        Next

        cboSearchColumn.SelectedIndex = 0

        m_lFindNext = gPMConstants.PMEReturnCode.PMFalse

    End Sub
End Class