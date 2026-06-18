Imports System.IO
Imports Microsoft.Win32
Public Class General
    Public Shared Sub LogErrorMessage(ByVal sUserName As String, _
            ByVal iType As Integer, _
            ByVal sMsg As String, _
   Optional ByVal vApp As String = "", _
   Optional ByVal vClass As String = "", _
   Optional ByVal vMethod As String = "", _
   Optional ByVal vErrNo As String = "", _
   Optional ByVal vErrDesc As String = "")

        Dim sNewMessage As String
        Dim sFileName As String



        sFileName = GetPMRegSetting()

        If Not String.IsNullOrEmpty(sFileName) Then
            Dim oFileStream = New StreamWriter(sFileName, True)

            sNewMessage = "Date / Time     : " & Date.Now
            sNewMessage = sNewMessage & vbNewLine & "Type            : " & DREConstants.PMELogLevel.PMLogError
            sNewMessage = sNewMessage & vbNewLine & "Username        : " & sUserName
            ' Add on the optional parameters if they are present
            If Not (String.IsNullOrEmpty(vApp)) Then
                sNewMessage = sNewMessage & vbNewLine & "Application     : " & vApp
            End If

            If Not (String.IsNullOrEmpty(vClass)) Then
                sNewMessage = sNewMessage & vbNewLine & "Class           : " & vClass
            End If

            If Not (String.IsNullOrEmpty(vMethod)) Then
                sNewMessage = sNewMessage & "." & vMethod
            End If
            ' Add the message to the end
            sNewMessage = sNewMessage & vbNewLine & "Message         : " & sMsg

            ' Add VB Error number and description if we have them
            If Not (String.IsNullOrEmpty(vErrNo)) Then
                sNewMessage = sNewMessage & vbNewLine & "Err.No          : " & vErrNo
            End If

            If Not (String.IsNullOrEmpty(vErrDesc)) Then
                sNewMessage = sNewMessage & vbNewLine & "Err.Description : " & vErrDesc
            End If

            sNewMessage = sNewMessage & vbNewLine & "********************************************************************************"

            oFileStream.WriteLine(sNewMessage)

            oFileStream.Close()
        End If

    End Sub

    Public Shared Function GetPMRegSetting() As String

        Dim sKeyString As String
        Dim v_sSettingName As String
        Dim vSettingValue As String
        Dim regVersion As RegistryKey

        ' Build up the key String
        sKeyString = DREConstants.ACRegRoot
        sKeyString = sKeyString & DREConstants.ACRegSiriusArchitecture
        sKeyString = sKeyString & DREConstants.ACRegCommon

        v_sSettingName = DREConstants.PMRegKeyLogFile

        regVersion = Registry.CurrentUser.OpenSubKey(sKeyString, False)
        vSettingValue = regVersion.GetValue(v_sSettingName).ToString

        Return vSettingValue

    End Function

End Class
