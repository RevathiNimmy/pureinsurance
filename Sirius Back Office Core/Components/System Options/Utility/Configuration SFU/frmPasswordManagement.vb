Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
Imports System.Text.RegularExpressions
Imports SharedFiles

Partial Friend Class frmPasswordManagement
    Inherits System.Windows.Forms.Form
    Private m_sUsername As String = ""
    Public ReadOnly Property DisplayOrder() As Integer
        Get
            Return IIf(CBool(ParentGroupID), GroupId Mod 10, GroupId \ 10)
        End Get
    End Property

    Public ReadOnly Property GroupId() As Integer
        Get
            Return 81
        End Get
    End Property

    Public ReadOnly Property GroupName() As String
        Get
            Return "Password Management"
        End Get
    End Property

    Public ReadOnly Property ParentGroupID() As String
        Get
            Return IIf(GroupId Mod 10, CStr((GroupId \ 10) * 10), CStr(0))
        End Get
    End Property


    Private Sub TextBox1_Leave(sender As Object, e As EventArgs) Handles txtPswrdStrRegx.Leave
        Dim m_lreturn As Integer = 0
        Try
            m_lreturn = gPMConstants.PMEReturnCode.PMTrue
            If txtPswrdStrRegx.Text <> "" Then
                m_lreturn = IsRegexPatternValid(txtPswrdStrRegx.Text)
            End If

        Catch excep As System.Exception
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Log that the regex supplied was invalid and rejected by the regex API Warn the user that the regex supplied was invalid and do not let them save", vApp:=ACApp, vClass:=ACClass, vMethod:="CalculatePremium", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
        End Try
    End Sub

    Public Function IsRegexPatternValid(ByVal pattern As String) As Boolean
        Dim result As Integer = 0
        Dim regex As Regex
        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            If Trim(pattern) <> "" Then
                regex = New Regex(pattern)
            End If
            Return True

        Catch excep As System.Exception
            ' Error.
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Log that the regex supplied was invalid and rejected by the regex API Warn the user that the regex supplied was invalid and do not let them save", vApp:=ACApp, vClass:=ACClass, vMethod:="Terminate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

End Class