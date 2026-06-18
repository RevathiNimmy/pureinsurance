Option Strict Off
Option Explicit On
<System.Runtime.InteropServices.ProgId("SharedStorage_NET.SharedStorage")>
Public NotInheritable Class SharedStorage
    ' ***************************************************************** '
    ' Class Name: SharedStorage
    '
    ' Author: Steve Watton
    '
    ' Date: 11/10/2002
    '
    ' Description: private class that is created from the communication class.
    '              used to hold values that are accessible from the VB ScriptControl.
    '
    ' Edit History:
    ' ***************************************************************** '


    Public vAmount As String = ""
    Public vReference As String = ""
    Public vValid As Boolean
    Public vRoundedAmount As Object
    Public sBankName As String = ""
    Public sAddress1 As String = ""
    Public sAddress2 As String = ""
    Public sAddress3 As String = ""
    Public sAddress4 As String = ""
    Public sPostalCode As String = ""
    Public vValidationMessage As Object
    Public bValidationOverridable As Boolean
    Public sAccountNo As String = ""
    Public sBankBranchCode As String = ""
    Public sBIC As String = ""
    Public sIBAN As String = ""
    Public sAccountType As String = ""
    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class
