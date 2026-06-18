Option Strict Off
Option Explicit On
Imports System
Imports SharedFiles

Module Module1

    Public Const ACApp As String = "GISListCreator"

    Sub Main()
        If Command() = "" Then
            Application.Run(New frmGISListCreator)
        Else
            Dim lResult As gPMConstants.PMEReturnCode = ProcessList(Command)
            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                MsgBox(Command() & " failed to process.", MsgBoxStyle.Exclamation, "Error")
            End If
        End If
    End Sub

    Function ProcessList(ByVal ListFile As String) As gPMConstants.PMEReturnCode
        Dim oGISList As New bGISList.bGISListCreate
        oGISList.InputFile = ListFile

        ' Create list DAT/IDX files
        Dim lResult As gPMConstants.PMEReturnCode = oGISList.Create()
        Return lResult
    End Function
End Module