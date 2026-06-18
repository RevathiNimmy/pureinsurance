Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Partial Friend Class frmLog
	Inherits System.Windows.Forms.Form
	
	
	Private m_sFileName As String = ""
	
	Private Sub BuildScriptFiles(ByRef sCategory As String, ByRef vArray() As Object)
		
        Dim iFilePtr As Integer
		

        Try
            File.Delete(m_sFileName)

            Try

                iFilePtr = FileSystem.FreeFile()

                'Create the script file
                FileSystem.FileOpen(iFilePtr, m_sFileName, OpenMode.Output)

                For lPtr As Integer = vArray.GetLowerBound(0) To vArray.GetUpperBound(0)

                    'Kill existing script file

                    If CStr(vArray(lPtr)) <> "" Then

                        'Output script file data
                        FileSystem.PrintLine(iFilePtr, vArray(lPtr))

                        'Clear Text

                        vArray(lPtr) = ""

                    End If

                Next lPtr

                'Close the script file
                FileSystem.FileClose(iFilePtr)

                'Release

                vArray = Nothing

                Exit Sub

            Catch excep As System.Exception



                Interaction.MsgBox("An Error occured while writing the script: " & Strings.Chr(13) & _
                                   Information.Err().Number & " : " & excep.Message, VariantType.Error, "Script Error")



            End Try

        Catch exc As System.Exception
            NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try
    End Sub

    Public Function Initialise(ByRef vLogText As Object, Optional ByRef vFileName As String = "") As Integer




        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Set up output file

            If Not Information.IsNothing(vFileName) Then
                m_sFileName = vFileName
            Else
                m_sFileName = "C:\InsertScript.sql"
            End If

            If Information.IsArray(vLogText) Then

                'If we have an array of strings, then pump to file
                BuildScriptFiles("PROCESS", vLogText)
                Return result

            Else
                'Display single string
                txtLog.Text = CStr(vLogText)

            End If

            Me.ShowDialog()

            Return result

        Catch


            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Me.Close()

    End Sub

    Private isInitializingComponent As Boolean
    Private Sub frmLog_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If


        Try
            txtLog.Width = Me.ClientRectangle.Width - VB6.TwipsToPixelsX(200)
            txtLog.Height = Me.ClientRectangle.Height - (cmdOK.Height + VB6.TwipsToPixelsY(400))

            cmdOK.Top = Me.ClientRectangle.Height - (cmdOK.Height + VB6.TwipsToPixelsY(200))
            cmdOK.Left = Me.ClientRectangle.Width - (cmdOK.Width + VB6.TwipsToPixelsX(200))

        Catch exc As System.Exception
            NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try

    End Sub
End Class
