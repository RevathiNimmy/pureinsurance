Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Imports MSMAPI

Partial Friend Class frmEmail
    Inherits System.Windows.Forms.Form

    Const ACClass As String = "frmEmail"

    Private m_lReturn As DialogResult
    Private m_lStatus As gPMConstants.PMEReturnCode
     'WR77 Documaster Enhancements START
    Private m_bToClick As Boolean
    Private m_sAddress() As Object

     'WR77 Documaster Enhancements START
    Private Const SW_SHOWNORMAL As Integer = 1
    Private Declare Function ShellExecute Lib "shell32.dll" Alias "ShellExecuteA" (ByVal hwnd As Integer, ByVal lOperation As String, ByVal lpFile As String, ByVal lpParameters As String, ByVal lpDirectory As String, ByVal nShowEnd As Integer) As Integer

    Public Property Addresses() As Object
        Get

            Return VB6.CopyArray(m_sAddress)

        End Get
        Set(ByVal Value As Object)

            m_sAddress = Value

        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            Return m_lStatus

        End Get
    End Property


    Public Property SendTo() As String
        Get

            Return txtTo.Text

        End Get
        Set(ByVal Value As String)

            txtTo.Text = Value

        End Set
    End Property


    Public Property SendSubject() As String
        Get

            Return txtSubject.Text

        End Get
        Set(ByVal Value As String)

            txtSubject.Text = Value

        End Set
    End Property


    Public Property SendNote() As String
        Get

            Return txtNote.Text

        End Get
        Set(ByVal Value As String)

            txtNote.Text = Value

        End Set
    End Property


    Public Property SendFile() As String
        Get

            Return txtFile.Text

        End Get
        Set(ByVal Value As String)

            txtFile.Text = Value

        End Set
    End Property

    Private Sub cmdExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdExit.Click

        m_lReturn = MessageBox.Show("Are you sure you wish to cancel?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If m_lReturn <> System.Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        m_lStatus = gPMConstants.PMEReturnCode.PMCancel

        Me.Hide()

    End Sub

    Private Sub cmdSend_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSend.Click
        Const kMethodName As String = "cmdSend_Click"

        Dim v_arrTemp As Object

        Try


         'Validate entries
        If txtTo.Text = "" Then
            MessageBox.Show("Please enter an address to send to.", "Documaster Email", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            txtTo.Focus()
            Exit Sub
        End If
        If Not m_bToClick And txtTo.Text <> "" Then

            v_arrTemp = txtTo.Text.Split(";"c)
            If Information.IsArray(v_arrTemp) Then

                For iCnt As Integer = 0 To v_arrTemp.GetUpperBound(0)

                    If CStr(v_arrTemp(iCnt)).Trim() <> "" Then
                        If iCnt <> 0 Then
                            ReDim Preserve m_sAddress(m_sAddress.GetUpperBound(0) + 1)
                        Else
                            ReDim m_sAddress(0)
                        End If



                        m_sAddress(m_sAddress.GetUpperBound(0)) = v_arrTemp(iCnt)
                    End If
                Next iCnt
            End If
        End If

        m_lStatus = gPMConstants.PMEReturnCode.PMOK

        Me.Hide()

        Catch ex As Exception
         ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, excep:=ex)

        Finally

        End Try
        Exit Sub
    End Sub

    Private Sub cmdTo_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdTo.Click
        Const kMethodName As String = "cmdTo_Click"



        'Dim myMapiSession As AxMSMAPI.AxMAPISession = Me.MAPISession1

        'Dim myMapiMessages As AxMSMAPI.AxMAPIMessages = Me.MAPIMessages1

        Try




            'myMapiSession.SignOn()


            'myMapiMessages.SessionID = myMapiSession.SessionID


            'myMapiMessages.Compose()

            'myMapiMessages.MsgSubject = "N/A"

            'myMapiMessages.MsgNoteText = "N/A"

            'myMapiMessages.Show()


            'For iCnt As Integer = 0 To CInt(myMapiMessages.RecipCount - 1)

            'If iCnt <> 0 Then
            '    ReDim Preserve m_sAddress(m_sAddress.GetUpperBound(0) + 1)
            'Else
            '    ReDim m_sAddress(0)
            'End If


            'myMapiMessages.RecipIndex = iCnt


            'm_sAddress(m_sAddress.GetUpperBound(0)) = myMapiMessages.RecipDisplayName 'myMapiMessages.RecipAddress

            'If iCnt = 0 Then

            '    txtTo.Text = myMapiMessages.RecipDisplayName
            'Else

            '    txtTo.Text = txtTo.Text & "; " & myMapiMessages.RecipDisplayName
            'End If
            'm_bToClick = True
            'Next iCnt



        Catch ex As Exception
         ' DO Not Call any functions before here or the error will be lost
        If Information.Err().Number <> 32001 Then

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, excep:=ex)
        End If
        Finally

            'myMapiSession.SignOff()
            'myMapiSession = Nothing
            'myMapiMessages = Nothing


        End Try
    End Sub
End Class