Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'refer Developer Guide No. 129
Imports SharedFiles
Partial Friend Class frmSearch
    Inherits System.Windows.Forms.Form

    Private Const ACClass As String = "frmInterface"

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_lFindNext As gPMConstants.PMEReturnCode


    ' ***************************************************************** '
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields.
    '
    ' ***************************************************************** '
    Public Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to assign all of the controls to
            ' PMFormControl
            '
            ' Example:-
            '
            '        ' Pass control and required settings to FormControl
            '        m_lReturn = m_oFormFields.AddNewFormField( _
            ''                       ctlControl:=<Control Name>, _
            ''                       lFieldType:=<PM field type>, _
            ''                       lFormat:=<PM format string>, _
            ''                       lMandatory:=<PMMandatory or PMNonMandatory)
            '
            '        'Error checking
            '        If (m_lReturn <> PMTrue) Then
            '          SetFieldValidation = PMFalse
            '          Exit Function
            '        End If
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtSearchValue, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboSearchColumn, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboSearchPosition, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' {* USER DEFINED CODE (End) *}


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cboSearchPosition_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboSearchPosition.SelectedIndexChanged
        If Convert.ToString(cboSearchPosition.Tag) <> "" Then
            If cboSearchPosition.SelectedIndex <> CDbl(Convert.ToString(cboSearchPosition.Tag)) Then
                cboSearchPosition.Tag = CStr(cboSearchPosition.SelectedIndex)
                m_lFindNext = gPMConstants.PMEReturnCode.PMFalse
            End If
        End If
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Dim lFound, lColumnStart, lColumnEnd, lRecordStart, lRecordEnd, lRecordStep As Integer
        Dim sSearchText As String = ""
        Dim lCurrentPos As Integer

        Dim oListItem As ListViewItem

        Try

            m_lReturn = m_oFormFields.CheckMandatoryControls()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            stbMain.Items.Item("Message").Text = "Searching..."
            stbMain.Items.Item("Found").Text = ""

            sSearchText = txtSearchValue.Text.Trim()
            'search columns
            If VB6.GetItemData(cboSearchColumn, cboSearchColumn.SelectedIndex) = -1 Then
                lColumnStart = 0
                lColumnEnd = m_ofrmInterface.lvwInstalment.Columns.Count - 1
            Else
                lColumnStart = VB6.GetItemData(cboSearchColumn, cboSearchColumn.SelectedIndex) - 1
                lColumnEnd = VB6.GetItemData(cboSearchColumn, cboSearchColumn.SelectedIndex) - 1
            End If

            'Developer Guide No 234
            If Not Information.IsNothing(m_ofrmInterface.lvwInstalment.FocusedItem) Then
                lCurrentPos = m_ofrmInterface.lvwInstalment.FocusedItem.Index + 1
            End If

            'search from
            Select Case cboSearchPosition.SelectedIndex
                Case 0 'current
                    If m_lFindNext = gPMConstants.PMEReturnCode.PMTrue Then
                        lRecordStart = lCurrentPos + 1
                    Else
                        lRecordStart = lCurrentPos
                    End If

                    lRecordEnd = m_ofrmInterface.lvwInstalment.Items.Count
                    lRecordStep = 1
                Case 1 'top
                    If m_lFindNext = gPMConstants.PMEReturnCode.PMTrue Then
                        lRecordStart = lCurrentPos + 1
                    Else
                        lRecordStart = 1
                    End If

                    lRecordEnd = m_ofrmInterface.lvwInstalment.Items.Count
                    lRecordStep = 1
                Case 2 'bottom
                    If m_lFindNext = gPMConstants.PMEReturnCode.PMTrue Then
                        lRecordStart = lCurrentPos - 1
                    Else
                        lRecordStart = m_ofrmInterface.lvwInstalment.Items.Count
                    End If

                    lRecordEnd = 1
                    lRecordStep = -1
            End Select


            'search thro records and columns

            lFound = -1
            For lRecordLoop As Integer = lRecordStart To lRecordEnd Step lRecordStep

                oListItem = m_ofrmInterface.lvwInstalment.Items.Item(lRecordLoop - 1)


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


                'stbMain.Items("Message").Text = "Ready"
                stbMain.Items("Found").Text = "Found"
                stbMain.Items("Message").Text = "Found" 'sanjay
                m_ofrmInterface.SetItem(Me, lFound)
            Else
                If m_lFindNext = gPMConstants.PMEReturnCode.PMTrue Then
                    m_ofrmInterface.SetItem(Me, lCurrentPos)
                End If

                stbMain.Items.Item("Found").Text = "Not Found"
                stbMain.Items.Item("Message").Text = "Not Found" 'sanjay
            End If

            oListItem = Nothing

        Catch excep As System.Exception



            stbMain.Items.Item("Message").Text = "Ready"
            stbMain.Items.Item("Found").Text = "Error"

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to do search", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub




        End Try

    End Sub


    Private Sub frmSearch_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load


        m_oFormFields = New iPMFormControl.FormFields()

        m_oFormFields.LanguageID = g_iLanguageID


        m_lReturn = CType(SetFieldValidation(), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        'load search column
        cboSearchColumn.Items.Clear()
        Dim cboSearchColumn_NewIndex As Integer = -1
        cboSearchColumn_NewIndex = cboSearchColumn.Items.Add("All")
        VB6.SetItemData(cboSearchColumn, cboSearchColumn_NewIndex, -1)

        For lCount As Integer = 1 To m_ofrmInterface.lvwInstalment.Columns.Count
            cboSearchColumn_NewIndex = cboSearchColumn.Items.Add(m_ofrmInterface.lvwInstalment.Columns.Item(lCount - 1).Text)
            VB6.SetItemData(cboSearchColumn, cboSearchColumn_NewIndex, lCount)
        Next

        cboSearchColumn.SelectedIndex = 0


        'load search position
        cboSearchPosition.Items.Clear()
        cboSearchPosition.Items.Add("Current")
        cboSearchPosition.Items.Add("Top")
        cboSearchPosition.Items.Add("Bottom")


        cboSearchPosition.SelectedIndex = 0
        cboSearchPosition.Tag = CStr(0)

        m_lFindNext = gPMConstants.PMEReturnCode.PMFalse

    End Sub

    Private Sub frmSearch_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
		m_oFormFields.Dispose()
        m_oFormFields = Nothing
        eventArgs.Cancel = Cancel <> 0
    End Sub


End Class
