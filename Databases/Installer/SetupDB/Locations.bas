Attribute VB_Name = "MLocations"
' Module:   Utility functions for the Locations dialogs
' Shared:   No
'
Option Explicit

Public Sub DisableQuestion(ByVal lblLabel As Label, _
    ByVal cboCombo As ComboBox, _
    Optional ByVal cmdButton As CommandButton = Nothing)

    lblLabel.Enabled = False

    cboCombo.Locked = True
    cboCombo.BackColor = vbButtonFace
    cboCombo.ForeColor = vbGrayText 'vbButtonText

    If Not cmdButton Is Nothing Then
        cmdButton.Enabled = False
    End If

End Sub

Public Function IsQuestionDisabled(ByVal cboCombo As ComboBox) As Boolean

    IsQuestionDisabled = cboCombo.Locked

End Function

' Enable folder browse buttons only if the server name refers to this computer.
Public Function EnableFolderBrowseButtons(ByVal sServerName As String) As Boolean

    Dim sComputerName As String

    sComputerName = ParseSep(sServerName, "", "\")
    EnableFolderBrowseButtons = False
    If sComputerName <> "" Then
        Select Case LCase$(sComputerName)
        Case ".", "(local)", LCase$(GetWindowsComputerName())
            EnableFolderBrowseButtons = True
        End Select
    End If

End Function

Public Function PromptToExitApp() As Boolean

    PromptToExitApp = (MsgBox( _
        "Are you sure that you want to abandon this installation? " & _
        "Abandoning now may result in your database being left in a corrupt or unusable state. " & _
        "Contact SSP for more information.", _
        vbExclamation + vbYesNo + vbDefaultButton2) = vbYes)

End Function
