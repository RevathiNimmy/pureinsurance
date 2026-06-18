Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Public Module iValidateFunc
    Private Const ACClass As String = "iValidateFunc"
	
	Public Sub CheckDateGotFocus(ByRef ctlControl As Control)
        Try
            If ctlControl.Text.Trim() <> "" Then
                ctlControl.Text = GeneralFunc.FormatField(GeneralConst.PMFormatDateShort, ctlControl.Text)
            End If
        Catch excep As System.Exception
            iGeneralFunc.LogMessage(iType:=GeneralConst.PMLogOnError, sMsg:="Failed to check the date on the got focus", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckDateGotFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        End Try
    End Sub

	Public Sub CheckDateLostFocus(ByRef ctlControl As Control)
        Dim sDate As String = ""
        Try
            If ctlControl.Text.Trim() <> "" Then
                sDate = GeneralFunc.FormatField(GeneralConst.PMFormatDateLong, ctlControl.Text)
                If sDate = "" Then
                    MessageBox.Show("Invalid date", Application.ProductName)
                    ctlControl.Focus()
                Else
                    ctlControl.Text = sDate
                End If
            End If
        Catch excep As System.Exception
            iGeneralFunc.LogMessage(iType:=GeneralConst.PMLogOnError, sMsg:="Failed to check the date on the lost focus", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckDateLostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        End Try
    End Sub
	
    Public Sub CheckTimeGotFocus(ByRef ctlControl As Control)
        Try
            If ctlControl.Text.Trim() <> "" Then
                ctlControl.Text = GeneralFunc.FormatField(GeneralConst.PMFormatTimeShort, ctlControl.Text)
            End If
        Catch excep As System.Exception
            iGeneralFunc.LogMessage(iType:=GeneralConst.PMLogOnError, sMsg:="Failed to check the time on the got focus", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckTimeGotFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        End Try
    End Sub

	Public Sub CheckTimeLostFocus(ByRef ctlControl As Control)
        Dim sDate As String = ""
        Try
            If ctlControl.Text.Trim() <> "" Then
                sDate = GeneralFunc.FormatField(GeneralConst.PMFormatTimeLong, ctlControl.Text)
                If sDate = "" Then
                    MessageBox.Show("Invalid date", Application.ProductName)
                    ctlControl.Focus()
                Else
                    ctlControl.Text = sDate
                End If
            End If
        Catch excep As System.Exception
            iGeneralFunc.LogMessage(iType:=GeneralConst.PMLogOnError, sMsg:="Failed to check the time on the lost focus", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckTimeLostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        End Try
    End Sub

	Public Sub CheckIntegerLostFocus(ByRef ctlControl As Control)
        Dim sInteger As String = ""
        Try
            If ctlControl.Text.Trim() <> "" Then
                sInteger = GeneralFunc.FormatField(GeneralConst.PMFormatInteger, ctlControl.Text)
                If sInteger = "" Then
                    MessageBox.Show("Invalid number", Application.ProductName)
                    ctlControl.Focus()
                Else
                    ctlControl.Text = sInteger
                End If
            End If
        Catch excep As System.Exception
            iGeneralFunc.LogMessage(iType:=GeneralConst.PMLogOnError, sMsg:="Failed to check the integer on the lost focus", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckIntegerLostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        End Try
    End Sub
	
	Public Function IsChar(ByRef sCharacter As String) As Integer
        Dim result As Integer = 0
        Try
            result = GeneralConst.PMFalse
            If (Strings.Asc(sCharacter(0))) >= 48 And (Strings.Asc(sCharacter(0))) <= 57 Then
                'char is a digit
                Return GeneralConst.PMTrue
            End If

            If (Strings.Asc(sCharacter(0))) >= 65 And (Strings.Asc(sCharacter(0))) <= 90 Then
                'char is between A and Z
                Return GeneralConst.PMTrue
            End If

            If Strings.Asc(sCharacter(0)) >= 97 And Strings.Asc(sCharacter(0)) <= 122 Then
                'char is between A and Z
                Return GeneralConst.PMTrue
            End If
            Return result
        Catch excep As System.Exception
            result = GeneralConst.PMFalse
            iGeneralFunc.LogMessage(iType:=GeneralConst.PMLogOnError, sMsg:="Failed to check character", vApp:=ACApp, vClass:=ACClass, vMethod:="IsChar", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function
End Module