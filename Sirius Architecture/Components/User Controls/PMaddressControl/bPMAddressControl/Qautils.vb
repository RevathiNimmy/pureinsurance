Option Strict Off
Option Explicit On
Imports System.Text
Imports SSP.Shared
Module QASFunc
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '


    ' Bring up a dialog box based on the Error Number passed to it
    '
    Public Sub ErrorMessage(ByVal ErrorNo As Integer)
        'Commented message bxo 
        'Dim MB_OK As MsgBoxStyle

        Dim RetBuff As New StringsHelper.FixedLengthString(100)

        If ErrorNo < 0 Then
            'developer guide no. 69(Guide)
            Select Case (New Business).QASDatabaseID
                Case 1
                    R_QAErrorMessage(ErrorNo, RetBuff.Value, 100)
                Case 3
                    N_QAErrorMessage(ErrorNo, RetBuff.Value, 100)
                Case Else
                    QAErrorMessage(ErrorNo, RetBuff.Value, 100)
            End Select

            '	MessageBox.Show("Error: " & (ErrorNo.ToString) & Strings.ChrW(13).ToString() & UnMakeCString(RetBuff.Value), MB_OK, "QuickAddress API Error")

        End If

    End Sub

    ' Make a string suitable for use by the DLLs
    ' (not necessary really)
    '
    Public Function MakeCString(ByVal arg As String) As String
        Return arg & Strings.ChrW(0).ToString()
    End Function

    ' Convert the newlines returned by the DLLs into something
    ' that can be used in VB text boxes
    '
    Public Function SplitAddress(ByVal arg As String) As String


        Dim Ret As New StringBuilder

        For Each Temp As Char In arg

            'developer guide no. 136 (Guide)
            If Temp.ToString.Length > 0 Then
                Dim asciiValue As Integer = Encoding.ASCII.GetBytes(Temp)(0)
                If asciiValue = 10 Then
                    Ret.Append(Strings.ChrW(13).ToString() & Strings.ChrW(10).ToString())
                Else
                    Ret.Append(Temp)
                End If
            End If
        Next Temp

        Return Ret.ToString()

    End Function

    ' Removes the 0 character off the end of the string returned
    ' from the DLLs
    Public Function UnMakeCString(ByVal arg As String) As String


        Dim NulIndex As Integer = (arg.IndexOf(Strings.ChrW(0).ToString()) + 1)

        If NulIndex > 0 Then
            Return arg.Substring(0, Math.Min(arg.Length, NulIndex - 1))
        Else
            Return arg
        End If

    End Function
End Module