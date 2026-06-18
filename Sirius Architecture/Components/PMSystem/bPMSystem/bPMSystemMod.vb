Option Strict Off
Option Explicit On
Module MainModule
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date:  17 October 1996
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "bPMSystem"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"



    'UserID

    Sub main_Renamed()

        ' Main entry point for the component

    End Sub

    '' ***************************************************************** '
    '' Name: LogMessage
    ''
    '' Description: Wrapper function to the log message method of the
    ''              message object.
    ''
    '' ***************************************************************** '
    'Public Sub LogMessage(iType As Integer, sMsg As String, Optional vApp As Variant, _
    ''        Optional vClass As Variant, Optional vMethod As Variant, _
    ''        Optional vErrNo As Variant, Optional vErrDesc As Variant)
    '
    'Dim lErrorValue As Long
    '
    '    On Error GoTo Err_LogMessage
    '
    '    ' Log Message to File
    '    LogMessagePopup _
    ''        iType:=iType%, _
    ''        sMsg:=sMsg$, _
    ''        vApp:=vApp, _
    ''        vClass:=vClass, _
    ''        vMethod:=vMethod, _
    ''        vErrNo:=vErrNo, _
    ''        vErrDesc:=vErrDesc
    '
    '    Exit Sub
    '
    'Err_LogMessage:
    '
    '    ' Error Section.
    '
    '    ' Failed to log message, so we must call the
    '    ' function to popup the message instead.
    '    LogMessagePopup _
    ''        iType:=iType%, _
    ''        sMsg:=sMsg$, _
    ''        vApp:=vApp, _
    ''        vClass:=vClass, _
    ''        vMethod:=vMethod, _
    ''        vErrNo:=vErrNo, _
    ''        vErrDesc:=vErrDesc
    '
    '    Exit Sub
    '
    'End Sub
    '
    '' ***************************************************************** '
    '' Name: Encrypt
    ''
    '' Description: Encrypts string passed and returns the result.
    ''
    '' ***************************************************************** '
    'Public Function Encrypt(sPassword As String, sEncryptedPassword) As Long
    '
    'Dim sAString As String
    'Dim sBString As String
    'Dim iCntr As Integer
    'Dim iCntr2 As Integer
    'Dim sChar1 As String * 1
    'Dim sChar2 As String * 1
    'Dim iSn As Integer
    'Dim sCodeString As String
    'Dim iClen As Integer
    '
    '    On Error GoTo Err_Encrypt
    '
    '    Encrypt = PMTrue
    '
    '    ' Encrypts the supplied string returning the encrypted
    '    ' result. Encrypted string will always be 2 characters
    '    ' longer than original (leave space!)
    '    '
    '    ' Encrypted string contains only ASCII characters in
    '    ' range 32-126
    '
    '    sCodeString$ = "aPCXADneGgH7khIJpjKtBMzmQLrRcqSEsbUv6yuVFW9xYZ2T3fd4w5N8"
    '    iClen% = Len(sCodeString$)
    '
    '    sAString$ = sPassword$
    '    iCntr% = Len(sAString$)
    '
    '    If (iCntr% < 1) Then
    '        Encrypt = PMFalse
    '
    '        sEncryptedPassword = ""
    '
    '        Exit Function
    '    End If
    '
    '    sChar1$ = Mid$(sCodeString$, (((Asc(Left$(sAString$, 1)) + iCntr%) Mod iClen%) + 1), 1)
    '    sChar2$ = Mid$(sCodeString$, ((Asc(Right$(sAString$, 1)) Mod iClen%) + 1), 1)
    '    iSn = ((Asc(sChar1$) + Asc(sChar2$)) Mod iClen%) + 1
    '    sBString$ = sChar2$
    '
    '    For iCntr2% = 1 To iCntr%
    '        sBString$ = sBString$ + Mid$(sCodeString$, ((Asc(Mid$(sAString$, iCntr2%, 1)) + _
    ''            iSn% + iCntr2%) Mod iClen%) + 1, 1)
    '    Next iCntr2%
    '
    '    sBString$ = sBString$ + sChar1$
    '
    '    ' Return the result.
    '    sEncryptedPassword = Trim$(sBString$)
    '
    '    Exit Function
    '
    'Err_Encrypt:
    '
    '    ' Error Section
    '
    '    Encrypt = PMError
    '
    '    sEncryptedPassword = ""
    '
    '    ' Log Error.
    '    LogMessage _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="Failed to encrypt the string", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="Encrypt", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    '
    'End Function
End Module