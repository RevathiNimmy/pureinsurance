Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms

Imports SharedFiles
Partial Public Class frmUserStatus
    Inherits System.Windows.Forms.Form
    ' Status
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_bPassedExam As Boolean
    Private m_vDatePassedExam As String = ""

    Private m_oFormFields As iPMFormControl.FormFields

    Public Property PassedExam() As Boolean
        Get
            m_bPassedExam = (Me.chkPassedExam.CheckState = CheckState.Checked)
            Return m_bPassedExam
        End Get
        Set(ByVal Value As Boolean)
            m_bPassedExam = Value
            Me.chkPassedExam.CheckState = IIf(m_bPassedExam, CheckState.Checked, CheckState.Unchecked)

            If Me.chkPassedExam.CheckState = CheckState.Checked Then
                Me.txtDatePassedExam.Enabled = True
                Me.txtDatePassedExam.BackColor = Color.White
                m_oFormFields.FormatControl(txtDatePassedExam, DateTime.Now)
            Else
                Me.txtDatePassedExam.Enabled = False
                Me.txtDatePassedExam.BackColor = SystemColors.Control
                Me.txtDatePassedExam.Text = ""
            End If

        End Set
    End Property

    Public Property DatePassedExam() As String
        Get
            If Information.IsDate(txtDatePassedExam.Text) Then

                m_vDatePassedExam = CStr(m_oFormFields.UnformatControl(txtDatePassedExam))
            Else

                m_vDatePassedExam = Nothing
            End If
            Return m_vDatePassedExam
        End Get
        Set(ByVal Value As String)

            m_vDatePassedExam = CStr(Value)

            m_oFormFields.FormatControl(txtDatePassedExam, IIf(Convert.IsDBNull(m_vDatePassedExam) Or IsNothing(m_vDatePassedExam), "", m_vDatePassedExam))
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


    Public WriteOnly Property RiskGroup() As String
        Set(ByVal Value As String)
            Text = Value
        End Set
    End Property



    Public Property UserStatus() As String
        Get
            If cboUserStatus.SelectedIndex > -1 Then
                Return VB6.GetItemString(cboUserStatus, cboUserStatus.SelectedIndex)
            Else
                Return ""
            End If
        End Get
        Set(ByVal Value As String)

            cboUserStatus.SelectedIndex = 0
            For i As Integer = 1 To cboUserStatus.Items.Count - 1
                If VB6.GetItemString(cboUserStatus, i) = Value Then
                    cboUserStatus.SelectedIndex = i
                    Exit For
                End If
            Next i
        End Set
    End Property

    Public ReadOnly Property UserStatusId() As Integer
        Get
            'MKW111103 PN8249
            Return VB6.GetItemData(cboUserStatus, cboUserStatus.SelectedIndex)
        End Get
    End Property

    Private Sub chkPassedExam_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkPassedExam.CheckStateChanged

        If Me.chkPassedExam.CheckState = CheckState.Checked Then
            Me.txtDatePassedExam.Enabled = True
            Me.txtDatePassedExam.BackColor = Color.White
            m_oFormFields.FormatControl(txtDatePassedExam, DateTime.Now)
        Else
            Me.txtDatePassedExam.Enabled = False
            Me.txtDatePassedExam.BackColor = SystemColors.Control
            Me.txtDatePassedExam.Text = ""
        End If

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        UserStatus = ""
        Me.Hide()
    End Sub


    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
        m_lStatus = gPMConstants.PMEReturnCode.PMOK
        Me.Hide()
    End Sub


    Private Function PopCombo() As Integer
        ' Created MKW111103 PN8249

        Dim result As Integer = 0
        Dim vUserStatus As Object

        Select Case Information.Err().Number
            Case Is < 0
                Conversion.ErrorToString(5)
            Case 1
                GoTo err_PopCombo
        End Select

        result = gPMConstants.PMEReturnCode.PMTrue


        Dim lReturn As gPMConstants.PMEReturnCode = g_oBusiness.GetUserStatus(r_vUserStatus:=vUserStatus)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        cboUserStatus.Items.Clear()

        If Not Information.IsArray(vUserStatus) Then
            Return result
        End If

        ' Loop through the data array

        For lRow As Integer = vUserStatus.GetLowerBound(1) To vUserStatus.GetUpperBound(1)
            'Set the data in the combobox

            Dim listIndex As Integer = cboUserStatus.Items.Add(New VB6.ListBoxItem(CStr(vUserStatus(1, lRow)), CInt(vUserStatus(0, lRow))))
        Next lRow

        Return result

err_PopCombo:

        'Error Section

        result = gPMConstants.PMEReturnCode.PMError

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to populate the User Status box", vApp:=ACApp, vClass:="frmUserStatus", vMethod:="PopCombo", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result
    End Function


    Private Sub frmUserStatus_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        m_lStatus = gPMConstants.PMEReturnCode.PMCancel

        m_oFormFields = New iPMFormControl.FormFields()
        m_oFormFields.AddNewFormField(txtDatePassedExam, gPMConstants.PMEFormatStyle.PMFormatDateLong, gPMConstants.PMEDataType.PMDate)

        'MKW111103 PN8249 START
        Dim lReturn As gPMConstants.PMEReturnCode = PopCombo()
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If
        'MKW111103 PN8249 END
    End Sub

    Private Sub frmUserStatus_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed

        If Not (m_oFormFields Is Nothing) Then
		m_oFormFields.Dispose()
            m_oFormFields = Nothing
        End If

    End Sub

    Private Sub txtDatePassedExam_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDatePassedExam.Enter
        m_oFormFields.GotFocus(txtDatePassedExam)
    End Sub

    Private Sub txtDatePassedExam_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDatePassedExam.Leave
        m_oFormFields.LostFocus(txtDatePassedExam)
    End Sub
End Class