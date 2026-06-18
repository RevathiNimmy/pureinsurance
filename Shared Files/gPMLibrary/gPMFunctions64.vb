Imports System.Runtime.InteropServices
Imports System.Text
Imports System


Public Enum RegSAM
        QueryValue = &H1
        SetValue = &H2
        CreateSubKey = &H4
        EnumerateSubKeys = &H8
        Notify = &H10
        CreateLink = &H20
        WOW64_32Key = &H200
        WOW64_64Key = &H100
        WOW64_Res = &H300
        Read = &H20019
        Write = &H20006
        Execute = &H20019
        AllAccess = &HF003F
    End Enum




    Public Module gPMRegistryFunctionsWOW6432
#Region "Member Variables"
#Region "Read 64bit Reg from 32bit app"
        <DllImport("Advapi32.dll")>
        Private Function RegOpenKeyEx(ByVal hKey As UIntPtr, ByVal lpSubKey As String, ByVal ulOptions As UInteger, ByVal samDesired As Integer, <Out> ByRef phkResult As Integer) As UInteger
        End Function

        <DllImport("Advapi32.dll")>
        Private Function RegCloseKey(ByVal hKey As Integer) As UInteger
        End Function

        <DllImport("advapi32.dll", EntryPoint:="RegQueryValueEx")>
        Public Function RegQueryValueEx(ByVal hKey As Integer, ByVal lpValueName As String, ByVal lpReserved As Integer, ByRef lpType As UInteger, ByVal lpData As StringBuilder, ByRef lpcbData As UInteger) As Integer
        End Function
#End Region
#End Region

#Region "Functions"
        Public Function GetRegKey64(ByVal inHive As UIntPtr, ByVal inKeyName As String, ByVal inPropertyName As String) As String
            Return GetRegKey64(inHive, inKeyName, RegSAM.WOW64_64Key, inPropertyName)
        End Function

        Public Function GetRegKey32(ByVal inHive As UIntPtr, ByVal inKeyName As String, ByVal inPropertyName As String) As String
            Return GetRegKey64(inHive, inKeyName, RegSAM.WOW64_32Key, inPropertyName)
        End Function

        Public Function GetRegKey64(ByVal inHive As UIntPtr, ByVal inKeyName As String, ByVal in32or64key As RegSAM, ByVal inPropertyName As String) As String
            'UIntPtr HKEY_LOCAL_MACHINE = (UIntPtr)0x80000002;
            Dim hkey = 0

            Try
            Dim lResult = RegOpenKeyEx(gPMRegConst.HKEY_LOCAL_MACHINE, inKeyName, 0, CInt(RegSAM.QueryValue) Or CInt(in32or64key), hkey)
            If 0 <> lResult Then Return Nothing
                Dim lpType As UInteger = 0
                Dim lpcbData As UInteger = 1024
                Dim AgeBuffer As StringBuilder = New StringBuilder(1024)
                RegQueryValueEx(hkey, inPropertyName, 0, lpType, AgeBuffer, lpcbData)
                Dim Age As String = AgeBuffer.ToString()
                Return Age
            Finally
                If 0 <> hkey Then RegCloseKey(hkey)
            End Try
        End Function
#End Region

#Region "Enums"
#End Region
    End Module

