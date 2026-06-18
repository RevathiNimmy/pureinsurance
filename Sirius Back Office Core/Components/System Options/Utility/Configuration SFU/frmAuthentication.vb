Option Strict Off
Option Explicit On
Public Class frmAuthentication
    Public ReadOnly Property DisplayOrder() As Integer
        Get
            Return IIf(CBool(ParentGroupID), GroupId Mod 10, GroupId \ 10)
        End Get
    End Property

    Public ReadOnly Property GroupId() As Integer
        Get
            Return 100
        End Get
    End Property
    Public ReadOnly Property GroupName() As String
        Get
            Return "Authentication Configuration"
        End Get
    End Property

    Public ReadOnly Property ParentGroupID() As String
        Get
            Return IIf(GroupId Mod 10, CStr((GroupId \ 10) * 10), CStr(0))
        End Get
    End Property
    Private Sub frmAuthentication_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class