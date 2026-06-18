Option Strict Off
Option Explicit On
<System.Runtime.InteropServices.ProgId("SharedStorage_NET.SharedStorage")>
Public NotInheritable Class SharedStorage
    ' ***************************************************************** '
    ' Class Name: SharedStorage
    '
    ' Author: Samrendu Bhushan
    '
    ' Date: 25/02/2008
    '
    ' Description: private class that is created from the communication class.
    '              used to hold values that are accessible from the VB ScriptControl.
    ' ***************************************************************** '


    Public vValid As Boolean
    Public vPostCode As String = "" 'will be used to pass post code to search
    Public vAddressLine1 As String = "" 'will be used to pass optional value to search
    Public vAddressArray(,) As Object
End Class
