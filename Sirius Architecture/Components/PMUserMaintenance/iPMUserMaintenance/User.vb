Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms

Imports SharedFiles
Partial Public Class frmUser
    Inherits System.Windows.Forms.Form

    Private Sub frmUser_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender
        End If
    End Sub


    Public Property EffectiveDate() As Date
        Get

            Dim result As Date = DateTime.FromOADate(0)
            If txtEffectiveDate.Text <> "" Then

                result = CDate(txtEffectiveDate.Text)

            End If

            Return result
        End Get
        Set(ByVal Value As Date)

            txtEffectiveDate.Text = Value
        End Set
    End Property


    Public Property UserName() As String
        Get
            Return txtUsername.Text
        End Get
        Set(ByVal Value As String)
            txtUsername.Text = Value
        End Set
    End Property

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        DirectCast(Me.Owner, frmUserMaintenance).NewUser = False

        Me.Hide()
    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Dim lReturn As gPMConstants.PMEReturnCode

        Dim sUsername As String = txtUsername.Text.Trim()

        If sUsername = "" Then

            MessageBox.Show("Username Must Be Entered", "Username", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

            txtUsername.Focus()

        Else

            lReturn = DirectCast(Me.Owner, frmUserMaintenance).CheckUsername(sUsername)


            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("User name " & sUsername & _
                                " already exists." & Strings.Chr(13) & Strings.Chr(10) & "Please Choose another user name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                txtUsername.Focus()

            Else

                If Not Information.IsDate(txtEffectiveDate.Text.Trim()) Then

                    MessageBox.Show("A Valid Date Must Be Entered", "Effective Date", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                    txtEffectiveDate.Focus()

                Else

                    With DirectCast(Me.Owner, frmUserMaintenance)
                        .NewUser = True
                        .UserName = txtUsername.Text.Trim()
                        .UserEffectiveDate = CDate(txtEffectiveDate.Text.Trim())
                    End With

                    Me.Hide()

                End If

            End If

        End If

    End Sub
End Class