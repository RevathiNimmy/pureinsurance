Option Strict Off
Option Explicit On
Imports System.IO
Imports Artinsoft.VB6.Utils
Imports SSP.Shared
'devloper guide no. 129
Friend NotInheritable Class DebugTimings

    Private Const ACClass As String = "DebugTimings"

    Private m_iUserID As Integer

    Private m_lStartTime(10) As Integer
    Private m_lEndTime(10) As Integer
    Private m_lLevel As Integer = 0

    Private m_sDebugFile As String = ""
    Private m_iDebugFile As Integer
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' UserId
    ' SaveAsFile
    Private m_bSaveAsFile As Boolean
    ' PrintToScreen
    Private m_bPrintToScreen As Boolean = True

    Private m_iGotRegSettings As Integer
    Private m_iRenewalInDebug As Integer
    Public WriteOnly Property PrintToScreen() As Boolean
        Set(ByVal Value As Boolean)
            m_bPrintToScreen = Value
        End Set
    End Property

    Public WriteOnly Property SaveAsFile() As Boolean
        Set(ByVal Value As Boolean)

            Try
                Dim lRenewalInDebug As Integer
                Dim sRenewalDebugFilePath As String = ""
                Dim i As Integer

                If Value Then

                    m_lReturn = CType(GetDebugSettingsFromRegistry(r_lRenewalInDebug:=lRenewalInDebug, r_sRenewalDebugFilePath:=sRenewalDebugFilePath), gPMConstants.PMEReturnCode)

                    If lRenewalInDebug = 1 Then
                        If sRenewalDebugFilePath.Trim() = "" Then
                            sRenewalDebugFilePath = "c:\temp\"
                        End If

                        m_iDebugFile = FreeFile()
                        m_lReturn = CType(OpenDebugFile(v_sRenewalDebugFilePath:=sRenewalDebugFilePath), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            m_bSaveAsFile = False
                        Else
                            m_bSaveAsFile = Value
                        End If
                    End If

                End If

            Catch exc As System.Exception
                'NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
            End Try

        End Set
    End Property
    Public WriteOnly Property UserId() As Integer
        Set(ByVal Value As Integer)
            m_iUserID = Value
        End Set
    End Property
    ' ***************************************************************** '
    '
    ' Name: OpenDebugFile
    '
    ' Description:
    '
    ' History: 20/11/2001 sj - Created.
    '
    ' ***************************************************************** '
    Private Function OpenDebugFile(ByVal v_sRenewalDebugFilePath As String) As Integer

        Dim result As Integer = 0
        Dim iCnt As Integer
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_sDebugFile = v_sRenewalDebugFilePath & "RenInfo" & CStr(m_iUserID) & ".txt"

            File.Open(m_iDebugFile, m_sDebugFile, OpenMode.Append)

            Return result

        Catch

            If Informations.Err().Number = 55 Then
                iCnt += 1
                m_sDebugFile = v_sRenewalDebugFilePath & "RenInfo" & CStr(m_iUserID) & "-" & CStr(iCnt) & ".txt"

            Else
                result = gPMConstants.PMEReturnCode.PMError
            End If

            Return result
        End Try

    End Function

    Public Sub StartBlock()

        Try
            Dim lRenewalInDebug As Integer
            Dim sRenewalDebugFilePath As String = ""

            If m_iGotRegSettings <> 1 Then
                m_lReturn = CType(GetDebugSettingsFromRegistry(r_lRenewalInDebug:=lRenewalInDebug, r_sRenewalDebugFilePath:=sRenewalDebugFilePath), gPMConstants.PMEReturnCode)

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    m_iRenewalInDebug = lRenewalInDebug
                End If
                m_iGotRegSettings = 1
            End If

            ' only if debug is required
            If m_iRenewalInDebug = 1 Then
                StartTiming()
            End If

        Catch exc As System.Exception
            ''NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try
    End Sub
    Public Sub EndBlock(ByVal v_sMessage As String)

        Try
            Dim lRenewalInDebug As Integer
            Dim sRenewalDebugFilePath As String = ""

            If m_iGotRegSettings <> 1 Then
                m_lReturn = CType(GetDebugSettingsFromRegistry(r_lRenewalInDebug:=lRenewalInDebug, r_sRenewalDebugFilePath:=sRenewalDebugFilePath), gPMConstants.PMEReturnCode)

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    m_iRenewalInDebug = lRenewalInDebug
                End If
                m_iGotRegSettings = 1
            End If

            ' only if debug is required
            If m_iRenewalInDebug = 1 Then
                DebugMessage(v_sMessage)
            End If

        Catch exc As System.Exception
            'NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try

    End Sub
    Public Sub PrintDebugMessage(ByVal v_sMessage As String)

        Try

            DebugMessage(v_sMessage)
            StartTiming()

        Catch exc As System.Exception
            'NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try
    End Sub
    Private Sub StartTiming()



        m_lStartTime(m_lLevel) = Environment.TickCount
        m_lLevel += 1


    End Sub

    Private Sub DebugMessage(ByVal v_sMessage As String)


        Dim sMessage As String = ""
        Dim lRenewalInDebug As Integer
        Dim sRenewalDebugFilePath As String = ""

        If m_iGotRegSettings <> 1 Then
            m_lReturn = CType(GetDebugSettingsFromRegistry(r_lRenewalInDebug:=lRenewalInDebug, r_sRenewalDebugFilePath:=sRenewalDebugFilePath), gPMConstants.PMEReturnCode)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                m_iRenewalInDebug = lRenewalInDebug
            End If
            m_iGotRegSettings = 1
        End If

        ' only if debug is required
        If m_iRenewalInDebug = 1 Then
            m_lLevel -= 1

            m_lEndTime(m_lLevel) = Environment.TickCount

            If m_bPrintToScreen Then
                Debug.WriteLine(v_sMessage & " - " & (m_lEndTime(m_lLevel) - m_lStartTime(m_lLevel)) / 1000)
            End If

            If m_bSaveAsFile Then
                sMessage = v_sMessage & " - " & CStr((m_lEndTime(m_lLevel) - m_lStartTime(m_lLevel)) / 1000)
                File.AppendAllLines(sRenewalDebugFilePath, sMessage.AsEnumerable)
                ' PrintLine(m_iDebugFile, sMessage)
            End If
        End If

    End Sub

    Protected Overrides Sub Finalize()
        If m_bSaveAsFile Then
            FileClose(m_iDebugFile)
        End If
    End Sub
    ' ***************************************************************** '
    '
    ' Name: GetDebugSettingsFromRegistry
    '
    ' Description:
    '
    ' History: 04/06/2001 sj - Created.
    '
    ' ***************************************************************** '
    Private Function GetDebugSettingsFromRegistry(ByRef r_lRenewalInDebug As Integer, ByRef r_sRenewalDebugFilePath As String) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim sValue As String = ""

        sValue = ""
        r_lRenewalInDebug = 0

        m_iGotRegSettings = 1
        m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="RenewalInDebug", r_sSettingValue:=sValue, v_sSubKey:=GISSharedConstants.ACOIMGISSubKey), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        sValue = sValue.Trim()
        If sValue = "1" Or sValue.ToUpper() = "Y" Then
            r_lRenewalInDebug = 1
            m_iRenewalInDebug = 1
        End If

        m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="RenewalDebugFilePath", r_sSettingValue:=sValue, v_sSubKey:=GISSharedConstants.ACOIMGISSubKey), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        sValue = sValue.Trim()
        If sValue <> "" Then
            If Not sValue.EndsWith("\") Then
                sValue = sValue & "\"
            End If
        End If

        r_sRenewalDebugFilePath = sValue

        Return result

    End Function
End Class

