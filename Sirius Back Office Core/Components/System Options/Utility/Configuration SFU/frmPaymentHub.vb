Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
Partial Friend Class frmPaymentHub
    Inherits System.Windows.Forms.Form

    Public ReadOnly Property DisplayOrder() As Integer
        Get
            Return IIf(CBool(ParentGroupID), GroupId Mod 10, GroupId \ 10)
        End Get
    End Property

    Public ReadOnly Property GroupId() As Integer
        Get
            Return 90
        End Get
    End Property

    Public ReadOnly Property GroupName() As String
        Get
            Return "Payment HUB"
        End Get
    End Property

    Public ReadOnly Property ParentGroupID() As String
        Get
            Return IIf(GroupId Mod 10, CStr((GroupId \ 10) * 10), CStr(0))
        End Get
    End Property

    Private Sub frmPaymentHub_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class