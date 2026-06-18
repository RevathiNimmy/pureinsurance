Option Strict Off
Option Explicit On

Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Windows.Forms
Imports SharedFiles
Imports System.IO

<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness

    ' Constants form gPMConstants
    Private Const PMFalse As Integer = 0
    Private Const PMTrue As Integer = 1
    Private Const PMFail As Integer = 10
    Private Const PMError As Integer = 11
    Private Const PMSucceed As Integer = 12
    Private Const PMOk As Integer = 20
    Private Const PMCancel As Integer = 21
    Private Const PMNotFound As Integer = 811

    Private m_oObjectManager As bObjectManager.ObjectManager

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise



        Dim result As Integer = 0
        Try

            m_oObjectManager = New bObjectManager.ObjectManager()
            result = CInt(m_oObjectManager.Initialise("bSIRCacheController"))
            Return PMTrue

        Catch excep As System.Exception

            result = PMError

            LogMessage("Terminate", "Initialise Failed" & Strings.Chr(13) & Strings.Chr(10) & excep.Message)

            Return result

        End Try
    End Function

    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
            End If
        End If
        Me.disposedValue = True
    End Sub


    Private Function LogMessage(ByVal v_sMethod As String, ByVal v_sMessage As String) As Integer

        Dim sMessage As String = "Method       : " & v_sMethod & Strings.Chr(13) & Strings.Chr(10) & _
                                 "Error Detail : " & v_sMessage

        If sMessage.Trim().Length > 0 Then
            MessageBox.Show(sMessage, "Sirius Cachce Controller - Business", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Function

    Private Function LogMessageToFile(ByVal v_sMessage As String) As Integer

        Dim sFileName As String = Interaction.Environ("TEMP") & "\SiriusCacheController.log"

        ' Load the file
        Dim ff As Integer = FileSystem.FreeFile()
        FileSystem.FileOpen(ff, sFileName, OpenMode.Append)

        FileSystem.WriteLine(ff, v_sMessage)
        FileSystem.FileClose(ff)

    End Function

    Public Function GetCacheKeyArray(ByRef r_vCacheKeyArray() As Object) As Integer

        Dim result As Integer = 0
        Dim vKeyArray() As String
        Dim sFileNames() As String
        Dim sCachePath As String = String.Empty
        Dim i As Integer
        Dim iReturn As Integer

        result = PMTrue

        Try

            iReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, _
                                                   v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, _
                                                   v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, _
                                                   v_sSettingName:=gPMConstants.PMRegKeyCachePath, r_sSettingValue:=sCachePath)

            If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Right(sCachePath, 1) <> "\" Then
                sCachePath += "\"
            End If

            result = PMTrue
            sFileNames = Directory.GetFiles(sCachePath, "*.xml")
            ' if nothing is passed, clear all the cache

            ' No Key is suppled, so delete all
            ReDim vKeyArray(sFileNames.GetUpperBound(0))
            For i = 0 To sFileNames.GetUpperBound(0)
                vKeyArray(i) = sFileNames(i)
            Next

            'if the key array is not nothing then pass it back to calling code
            If Not Object.Equals(vKeyArray, Nothing) Then
                r_vCacheKeyArray = vKeyArray
            End If

            Return result

        Catch excep As System.Exception

            result = PMError
            LogMessage("GetCacheKeyArray", "GetCacheKeyArray Failed" & Strings.Chr(13) & Strings.Chr(10) & excep.Message)
            Return result

        End Try
    End Function

    Public Function ClearCache(Optional ByVal v_sKey As Object = Nothing) As Integer

        Dim result As Integer = 0
        'Dim oCache As VariantCacheLib.Cache
        'Dim vKeyArray, vNewKeyArray As Object
        'Dim lOldArraySize, lNewArraySize, lRow As Integer
        Dim sKey As String ', sTempKey
        Dim sCachePath As String = String.Empty
        Dim sFileNames() As String
        Dim i As Integer
        Try

            result = PMTrue
            'oCache = New VriantCacheLib.Cache()

            ' if nothing is passed, clear all the cache
            If Information.IsNothing(v_sKey) Then
                ' No Key is suppled, so delete all
                'we need a threadlock on this
                'oCache.Clear()

                result = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, _
                                                   v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, _
                                                   v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, _
                                                   v_sSettingName:=gPMConstants.PMRegKeyCachePath, r_sSettingValue:=sCachePath)

                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Right(sCachePath, 1) <> "\" Then
                    sCachePath += "\"
                End If

                sFileNames = Directory.GetFiles(sCachePath, "*.xml")
                For i = 0 To sFileNames.GetUpperBound(0)
                    File.Delete(sFileNames(i))
                Next

            Else

                sKey = CStr(v_sKey)
                ' If a key is passed in just, the we need to clear that key
                'oCache.Remove(sKey)
                File.Delete(sKey)
                '' Also, we need to upadate the  ' SIRIUS_CACHE_KEYS  Array

                '' SIRIUS_CACHE_KEYS is the name of the key, we used in the components to store the keys

                'vKeyArray = oCache.Item("SIRIUS_CACHE_KEYS")

                '' Create the array without the one we deleted

                'If Object.Equals(vKeyArray, Nothing) Then
                '    LogMessageToFile("ClearCache :  Key Array is Empty")
                'Else

                '    lOldArraySize = vKeyArray.GetUpperBound(0)
                '    lNewArraySize = lOldArraySize - 1
                '    ' Check if the values are valid
                '    If lNewArraySize > -1 Then
                '        ReDim vNewKeyArray(lNewArraySize)
                '    End If

                '    lRow = 0
                '    For lCounter As Integer = 0 To lOldArraySize - 1

                '        sTempKey = CStr(vKeyArray(lCounter))
                '        sTempKey = sTempKey.Trim().ToUpper()
                '        sKey = sKey.Trim().ToUpper()

                '        If CStr(vKeyArray(lCounter)) <> sKey Then

                '            vNewKeyArray(lRow) = sTempKey
                '            lRow += 1
                '        End If
                '    Next lCounter

                '    ' First Remvoed the cache Key Array
                '    oCache.Remove("SIRIUS_CACHE_KEYS")

                '    ' Add the Updated Key
                '    oCache.Add("SIRIUS_CACHE_KEYS", vNewKeyArray)

                'End If                
            End If

            Return result

        Catch excep As System.Exception

            result = PMError
            LogMessage("ClearCache", "ClearCache Failed" & Strings.Chr(13) & Strings.Chr(10) & excep.Message)
            Return result

        End Try
    End Function
End Class
