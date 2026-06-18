Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Module MainModule
    ' Main public constant for all functions
    ' to identify which application this is.

    Public Const ACApp As String = "iACTUserControls"
    ' Constant for the functions to identify
    ' which class this is.

    Public Const ACDefaultFirstItem As Integer = 0

    Private Const ACClass As String = "MainModule"

    Private m_lReturn As Integer

    ' Public source and language ID's from the
    ' Object Manager.
    Public g_iSourceID As Integer
    Public g_iLanguageID As Integer
    Public g_iCurrencyID As Integer


    '' Legal values for TableName
    'Public Enum actTableName
    '  actDocumentType
    '  actPostingStatus
    '  actLedgerType
    '  actPurgeFrequency
    '  actAccountType
    'End Enum

    Public Function Terminate() As Integer


    End Function

    ' Initialise the Object Manager Etc
    Public Function Initialise() As Integer

        Dim result As Integer = 0

        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

            Return result

        End Try
    End Function

    ' Function to return the position of an Item in ItemData
    Public Function IndexOfItem(ByRef r_cboCombo As ComboBox, ByVal v_lItemId As Integer) As Integer
        With r_cboCombo
            For nIndex As Integer = 0 To .Items.Count - 1
                If VB6.GetItemData(r_cboCombo, nIndex) = v_lItemId Then
                    Return nIndex
                End If
            Next nIndex
        End With
        Return -1
    End Function

    Public Function IsBuildMachine() As Boolean
        Static vIsRunning, bIsBuildMachine As Boolean

        Dim sValue As String = ""

        'DD 26/5/2004
        'Returns true if this machine is a build machine
        'PMLookup will always try to populate the combo if embedded on another control
        'eg. Address control. Which means that a build machine would have to be logged into
        'Sirius to compile other projects. If this registry setting is on then
        'it will never load any data which is fine for just compiling.


        If vIsRunning.Equals(False) Then
            gPMFunctions.GetPMRegSetting(gPMConstants.HKEY_LOCAL_MACHINE, gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, gPMConstants.PMERegSettingLevel.pmeRSLClient, "IsBuildMachine", sValue)

            vIsRunning = True
            bIsBuildMachine = (sValue <> "")
        End If

        Return bIsBuildMachine
    End Function
End Module