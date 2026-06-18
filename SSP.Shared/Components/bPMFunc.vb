Option Strict Off
Option Explicit On
Imports System.Globalization
Imports System.IO
Imports System.Reflection
Imports System.Security.Cryptography
Imports System.Text

<System.Runtime.InteropServices.ProgId("bPMFunc_NET.bPMFunc")>
Public Module bPMFunc
    ' ***************************************************************** '
    '
    ' Business general functions module. Contains all of the global
    ' functions that might be useful when writing the business layer.
    '
    ' ***************************************************************** '
    ' ***************************************************************** '
    ' Edit History :
    '
    ' BB231097 -       Add ConvertWildCard function
    ' RFC 27/02/1998 - Encrypt from PMFunc added.
    ' RFC 27/02/1998 - GetCommandLine from PMFunc added.
    ' JK 11/05/99-     Validate SQL added
    ' DAK240100 -      Add new function LicenceEncrypt
    ' CJB 8/2/2000 -   Added new subroutine ShellSort
    ' BSJ 16/2/00 -    Added UniqueTableName function
    ' RFC 11/08/00 -   Added Get GUID function.
    ' RDT 28/09/00 -   Added ConvertBaseNToDec function.
    ' RDT 28/09/00 -   Added ConvertDecToBaseN function.
    ' SJP 18/06/02 -   Added retrieveProductOptions function
    ' RFC 12/10/04 -   Added LogError sub.
    ' RKS 15/12/04 -   Added GetSystemSecurityModel function
    '****************************************************************** '


    ' RDC 02062004
    <ThreadStatic()>
    Private m_lReturn As gPMConstants.PMEReturnCode
    <ThreadStatic()>
    Private vProductOptions(,) As Object
    ' Constant for the methods to identify which class this is.
    Private Const ACClass As String = "bPMFunc"
    ' Private DES As New TripleDESCryptoServiceProvider
    ' Private MD5 As New MD5CryptoServiceProvider

    ' BSJ 16/02/00
    ' BSJ 16/02/00

    ' RFC 11/08/00 - Get GUID function - Start
    'Private Structure GUID
    '    Dim Data1 As Integer
    '    Dim Data2 As Integer
    '    Dim Data3 As Integer
    '    <VBFixedArray(7)> _
    '    Dim Data4() As Byte
    '    Public Shared Function CreateInstance() As GUID
    '        Dim result As New GUID
    '        ReDim result.Data4(7)
    '        Return result
    '    End Function
    'End Structure


    'Private Declare Function CoCreateGuid Lib "OLE32.DLL" (ByRef pGuid As GUID) As Integer
    '' RFC 11/08/00 - Get GUID function - End

    ' ***************************************************************** '
    ' Name: UniqueTableName
    '
    ' Description: Function to retrieve a unique table name.
    ' Usage      : This can be used to get a name for unique temp tables
    '            : i.e. smytable = "##TMP" & UniqueTableName
    '
    ' ***************************************************************** '
    Public Function UniqueTableName() As String

        ' Get the name of the table
        Return Environment.TickCount.ToString("X")
    End Function

    ' ***************************************************************** '
    ' Name: LogError
    '
    ' Description: Called by the Catch Error Handlers
    '
    ' v_sUsername       The current sirius username.
    ' v_sClass          The class in which the error is occurring
    ' v_sMethod         The function/sub in which the error is occurring
    ' r_lFunctionReturn The will reutn the standard sirius return value
    '                   depending on the type of error.
    '                       PMFalse for calls to other methods etc.
    '                       PMError for VB exceptions.
    '                   If a specific (non PMTrue) value has already been assigned
    '                   as the function return value then that value will be preserved.
    ' ***************************************************************** '
    Public Sub LogError(ByVal v_sUsername As String, ByVal v_sClass As String, ByVal v_sMethod As String, ByRef r_lFunctionReturn As Integer)
        LogError(v_sUsername, v_sClass, v_sMethod, r_lFunctionReturn, Nothing)
    End Sub
    Public Sub LogError(ByVal v_sUsername As String, ByVal v_sClass As String, ByVal v_sMethod As String, ByRef r_lFunctionReturn As Integer, ByRef excep As Exception)

        Dim eErrLevel As gPMConstants.PMELogLevel
        Dim sMsg As String = ""

        'Grab the Error details first before they disappear.
        Dim llineNo As Integer = Information.Erl()
        Dim lErrNumber As gPMConstants.PMEReturnCode = Information.Err().Number
        v_sMethod = v_sMethod & Strings.ChrW(13) & Strings.ChrW(10) & "Line Number     : " & CStr(llineNo)
        Dim sErrDesc As String = Information.Err().Description & Strings.ChrW(13) & Strings.ChrW(10) & "Source          : " & Information.Err().Source
        Dim lLogLevel As Integer = Information.Err().HelpContext

        If excep IsNot Nothing AndAlso excep.Message IsNot Nothing Then
            sErrDesc = sErrDesc & Environment.NewLine & excep.Message
        End If

        Try

            'Subtract vbObjectError to see if its one of our errors Or a VB Native one
            lErrNumber = CType(lErrNumber - ObjectError, gPMConstants.PMEReturnCode)

            'What sort of error?

            Select Case lErrNumber
                ' One of Ours
                Case gPMConstants.PMEReturnCode.PMBackOfficeError

                    ' If the Function already has a non PMTrue return value then leave it alone, otherwise set to PMFalse
                    If r_lFunctionReturn = gPMConstants.PMEReturnCode.PMTrue Then
                        r_lFunctionReturn = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If lLogLevel > 0 Then
                        eErrLevel = lLogLevel
                    Else
                        eErrLevel = gPMConstants.PMELogLevel.PMLogDebug1
                    End If

                    sMsg = "Call to another method failed. See Error description for details."

                    ' VB Native
                Case Else

                    ' If the Function already has a non PMTrue return value then leave it alone, otherwise set to PMError
                    If r_lFunctionReturn = gPMConstants.PMEReturnCode.PMTrue Then
                        r_lFunctionReturn = gPMConstants.PMEReturnCode.PMError
                    End If

                    eErrLevel = gPMConstants.PMELogLevel.PMLogOnError
                    sMsg = "Internal Exception. See Error description for details."

                    ' Add it back on so that we get a VB error number that we recognise e.g. 429
                    lErrNumber = CType(lErrNumber + ObjectError, gPMConstants.PMEReturnCode)

            End Select

            LogMessage(v_sUsername, iType:=eErrLevel, sMsg:=sMsg, vApp:=ACApp, vClass:=v_sClass, vMethod:=v_sMethod, vErrNo:=lErrNumber, vErrDesc:=sErrDesc)

        Catch exc As System.Exception
            ' NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try


    End Sub    ' ***************************************************************** '
    ' Name: LogMessage
    '
    ' Description: Wrapper function to the log message method of the
    '              message object.
    '
    ' Changes:
    ' RDC 29072002 bPMMessage is no longer required.
    '              Use methods in gPMFunctions
    ' ***************************************************************** '
    Public Sub LogMessage(ByVal sUsername As String, ByVal iType As Integer, ByVal sMsg As String)
        LogMessage(sUsername, iType, sMsg, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
    End Sub
    Public Sub LogMessage(ByVal sUsername As String, ByVal iType As Integer, ByVal sMsg As String, ByRef vApp As Object)
        LogMessage(sUsername, iType, sMsg, vApp, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
    End Sub
    Public Sub LogMessage(ByVal sUsername As String, ByVal iType As Integer, ByVal sMsg As String, ByRef vApp As Object, ByRef vClass As Object)
        LogMessage(sUsername, iType, sMsg, vApp, vClass, Nothing, Nothing, Nothing, Nothing, Nothing)
    End Sub
    Public Sub LogMessage(ByVal sUsername As String, ByVal iType As Integer, ByVal sMsg As String, ByRef vApp As Object, ByRef vClass As Object, ByRef vMethod As Object)
        LogMessage(sUsername, iType, sMsg, vApp, vClass, vMethod, Nothing, Nothing, Nothing, Nothing)
    End Sub
    Public Sub LogMessage(ByVal sUsername As String, ByVal iType As Integer, ByVal sMsg As String, ByRef vApp As Object, ByRef vClass As Object, ByRef vMethod As Object, ByRef vErrNo As Object, ByRef vErrDesc As Object)
        LogMessage(sUsername, iType, sMsg, vApp, vClass, vMethod, vErrNo, vErrDesc, Nothing, Nothing)
    End Sub
    Public Sub LogMessage(ByVal sUsername As String, ByVal iType As Integer, ByVal sMsg As String, ByRef vApp As Object, ByRef vClass As Object, ByRef vMethod As Object, ByRef excep As Exception)
        LogMessage(sUsername, iType, sMsg, vApp, vClass, vMethod, Nothing, Nothing, excep, Nothing)
    End Sub
    Public Sub LogMessage(ByVal sUsername As String, ByVal iType As Integer, ByVal sMsg As String, ByRef vClass As Object, ByRef vMethod As Object, ByRef vErrNo As Object, ByRef vErrDesc As Object)
        LogMessage(sUsername, iType, sMsg, Nothing, vClass, vMethod, vErrNo, vErrDesc, Nothing, Nothing)
    End Sub
    Public Sub LogMessage(ByVal sUsername As String, ByVal iType As Integer, ByVal sMsg As String, ByRef vClass As Object, ByRef vMethod As Object, ByRef excep As Exception)
        LogMessage(sUsername, iType, sMsg, Nothing, vClass, vMethod, Nothing, Nothing, excep, Nothing)
    End Sub
    Public Sub LogMessage(ByVal sUsername As String, ByVal iType As Integer, ByVal sMsg As String, ByRef vClass As Object, ByRef vMethod As Object, ByRef vErrNo As Object, ByRef vErrDesc As Object, ByRef excep As Exception)
        LogMessage(sUsername, iType, sMsg, Nothing, vClass, vMethod, vErrNo, vErrDesc, excep, Nothing)
    End Sub
    Public Sub LogMessage(ByVal sUsername As String, ByVal iType As Integer, ByVal sMsg As String, ByRef vApp As Object, ByRef vClass As Object, ByRef vMethod As Object, ByRef vErrNo As Object, ByRef vErrDesc As Object, ByRef excep As Exception)
        LogMessage(sUsername, iType, sMsg, vApp, vClass, vMethod, vErrNo, vErrDesc, excep, Nothing)
    End Sub


    Public Sub LogMessage(ByVal iType As Integer, ByVal sMsg As String, ByRef vApp As Object, ByRef vClass As Object, ByRef vMethod As Object, ByRef vErrNo As Object, ByRef vErrDesc As Object, ByRef excep As Exception)
        LogMessage(iType, sMsg, vApp, vClass, vMethod, vErrNo, vErrDesc, excep, Nothing)

    End Sub
    Public Sub LogMessage(ByVal iType As Integer, ByVal sMsg As String, ByRef vApp As Object, ByRef vClass As Object, ByRef vMethod As Object, ByRef vErrNo As Object, ByRef vErrDesc As Object)

        LogMessage(iType:=iType, sMsg:=sMsg, vApp:=vApp, vClass:=vClass, vMethod:=vMethod, vErrNo:=vErrNo, vErrDesc:=vErrDesc)
        ' LogMessage(iType, sMsg, vApp, vClass, vMethod, vErrNo, vErrDesc)
    End Sub

    Public Sub LogMessage(ByVal iType As Integer, ByVal sMsg As String, ByRef vApp As Object, ByRef vClass As Object, ByRef vMethod As Object)

        LogMessage(iType:=iType, sMsg:=sMsg, vApp:=vApp, vClass:=vClass, vMethod:=vMethod)

    End Sub

    Public Sub LogMessage(ByVal sUsername As String, ByVal iType As Integer, ByVal sMsg As String, ByRef vApp As Object, ByRef vClass As Object, ByRef vMethod As Object, ByRef vErrNo As Object, ByRef vErrDesc As Object, ByRef excep As Exception, ByRef oDicParms As Dictionary(Of String, Object))
        Try
            Dim sb As New StringBuilder()
            If vErrDesc IsNot Nothing Then
                sb.AppendLine(vErrDesc)
            End If
            If excep IsNot Nothing AndAlso excep.Message IsNot Nothing Then
                sb.AppendLine("Message: " & excep.Message)
            End If
            If excep IsNot Nothing AndAlso excep.StackTrace IsNot Nothing Then
                sb.AppendLine("StackTrace: " & excep.StackTrace)
            End If

            vErrDesc = sb.ToString()
            If excep Is Nothing Then
                excep = New Exception(CStr(vErrDesc))
            End If
            gPMFunctions.LogMessageToFile(sUsername:=sUsername, iType:=iType, sMsg:=sMsg, vApp:=CStr(vApp), vClass:=CStr(vClass), vMethod:=CStr(vMethod), excep:=excep, oDicParms:=oDicParms)
        Catch
        End Try
    End Sub
    'NIIT: To Do : New method added as per proactive list for exception handling
    Public Sub LogExcepMessage(ByVal sUsername As String, ByVal iType As Integer, ByVal sMsg As String, Optional ByRef vApp As Object = Nothing, Optional ByRef vClass As Object = Nothing, Optional ByRef vMethod As Object = Nothing, Optional ByRef vErrNo As Object = Nothing, Optional ByRef vErrDesc As Object = Nothing, Optional ByRef oException As System.Exception = Nothing)
        LogMessage(sUsername, iType, sMsg, vApp, vClass, vMethod, vErrNo, vErrDesc, excep:=oException)
    End Sub
    ' ***************************************************************** '
    ' BB231097 - Add ConvertWildCard function
    '
    ' Name: ConvertWildCard
    '
    ' Description: Takes a string trims it and converts incoming wildcard char
    ' to outgoing wilcard char currently "*" to "%" for SQL. Converted string
    ' returned as Function value.
    '
    ' For further info see ConvertWildCard.doc in Sirius SourceSafe
    ' ***************************************************************** '
    Public Function ConvertWildCard(ByVal v_sSearchString As String) As String

        Const WildIn As String = "*"
        Const WildOut As String = "%"

        Dim sConvertString As String = ""

        Try

            ' Remove leading & trailing spaces
            sConvertString = v_sSearchString.Trim()

            ' If not a null string check for wildcard(s) and convert accordingly
            If sConvertString <> "" Then

                If (sConvertString.Substring(0, 1) = WildIn) And (sConvertString.Substring(sConvertString.Length - 1) = WildIn) Then

                    ' Could be "*" or "**" on their own
                    If sConvertString.Length < 3 Then
                        sConvertString = WildOut
                    Else
                        'sConvertString = WildOut & Mid(sConvertString, 2, sConvertString.Length - 2) & WildOut
                        sConvertString = WildOut & sConvertString.Substring(2, sConvertString.Length - 2) & WildOut
                    End If

                ElseIf (sConvertString.Substring(sConvertString.Length - 1) = WildIn) Then

                    sConvertString = sConvertString.Substring(0, sConvertString.Length - 1) & WildOut

                ElseIf (sConvertString.Substring(0, 1) = WildIn) Then

                    sConvertString = WildOut & sConvertString.Substring(sConvertString.Length - (sConvertString.Length - 1))

                End If

            End If

            ' Return the converted string.

            Return sConvertString

        Catch



            ' Error Section.

            ' Return the original value.

            Return v_sSearchString
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: Encrypt
    '
    ' Description: Encrypts string passed and returns the result.
    '
    ' ***************************************************************** '
    ' ************* If you change this function you MUST also change
    ' ************* the iPMFunc version.
    ' *************
    Public Function Encrypt(ByRef sPassword As String, ByRef sEncryptedPassword As String) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim salt As String = BCrypt.Net.BCrypt.GenerateSalt()
            'Encrypts the supplied string returning the encrypted
            ' result.
            sEncryptedPassword = BCrypt.Net.BCrypt.HashPassword(sPassword, salt)
            sEncryptedPassword = sPassword

            Return result

        Catch excep As System.Exception

            ' Error Section
            result = gPMConstants.PMEReturnCode.PMError
            sEncryptedPassword = ""
            ' Log Error.
            LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to encrypt the string", vApp:=ACApp, vClass:=ACClass, vMethod:="Encrypt", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: Decrypt
    '
    ' Description: Decrypts string passed and returns the result.
    '
    ' Originally by Chris Barnes; modified for business obj by CL090101
    ' DD 27/11/2001 Moved here from bSiriusLink.SiriusLink and made public
    '
    ' ***************************************************************** '
    ' ************* If you change this function you MUST also change
    ' ************* the iPMFunc version.
    ' *************
    Public Function Decrypt(ByVal v_sPassword As String, ByRef r_sDecryptedPassword As String) As Integer

        Dim result As Integer = 0
        Dim sAString As String = ""
        Dim sBString As New StringBuilder
        Dim iCntr As Integer
        Dim sChar1 As New StringBuilder(1)
        Dim sChar2 As New StringBuilder(1)
        Dim iSn As Integer
        Dim sCodeString As String = ""
        Dim iClen As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Decrypts the supplied string returning the Decrypted
            ' result. Decrypted string will always be 2 characters
            ' shorter than original
            '

            sCodeString = "aPCXADneGgH7khIJpjKtBMzmQLrRcqSEsbUv6yuVFW9xYZ2T3fd4w5N8"
            iClen = sCodeString.Length

            sAString = v_sPassword
            iCntr = sAString.Length - 2 'take 2 off as ignoring first and last characters in password

            If iCntr < 1 Then
                result = gPMConstants.PMEReturnCode.PMFalse

                r_sDecryptedPassword = ""

                Return result
            End If

            '1..Find out value for iSn
            'ASCII value of last character + ASCII value of first character, mod by 56, add 1 to result
            iSn = ((Strings.AscW(sAString.Substring(sAString.Length - 1)(0)) + Strings.AscW(sAString.Substring(0, 1)(0))) Mod iClen) + 1

            '2..Find value of sChar1$
            'Is simply the last character
            sChar1 = New StringBuilder(sAString.ToString.Substring(sAString.Length - 1))

            '3..Find value of sChar2$
            'Is simply the first character
            sChar2 = New StringBuilder(sAString.ToString.Substring(0, 1))

            '4..Now we have all the variable values used, plug the value into the loop for every char in
            '   the password. Note that we ignore the first and last characters in the password now
            sAString = sAString.Substring(1, sAString.Length - 2)

            Dim iPos, iTemp As Integer
            Dim sTemp As String = ""
            For iCntr2 As Integer = 1 To iCntr

                iPos = (sCodeString.IndexOf(sAString.Substring(iCntr2 - 1, 1)) + 1)
                iTemp = iPos - 1
                iTemp += (iClen * 2) 'this iClen * 2 is dodgy ! - could be * 1 or other???
                iTemp = iTemp - iSn - iCntr2

                If iTemp > 122 Then iTemp -= 56

                sTemp = Strings.ChrW(iTemp).ToString()

                sBString.Append(sTemp)
            Next

            ' Return the result.
            r_sDecryptedPassword = sBString.ToString().Trim()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to decrypt the string", vApp:=ACApp, vClass:=ACClass, vMethod:="Encrypt", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)


            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: LicenceEncrypt
    '
    ' Description: Encrypts string passed and returns the result.
    '              Copied from Encrypt but returns a string.
    '
    ' ************* If you change this function you MUST also change
    ' ************* the iPMFunc version.
    ' *************
    '
    ' History: 24/01/2000 DAK - Created.
    '
    ' ***************************************************************** '
    Public Function LicenceEncrypt(ByRef sLicence As String, ByRef sLicenceKey As String) As Integer
        Dim result As Integer = 0
        Dim sAString As String = ""
        Dim sBString As New StringBuilder
        Dim iCntr As Integer
        Dim sChar1 As New StringBuilder(1)
        Dim sChar2 As New StringBuilder(1)
        Dim iSn As Integer
        Dim sCodeString As String = ""
        Dim iClen As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Encrypts the supplied string returning the encrypted
            ' result. Encrypted string will always be 2 characters
            ' longer than original (leave space!)
            '
            ' Encrypted string contains only ASCII characters in
            ' range 32-126

            sCodeString = "aPCXADneGgH7khIJpjKtBMzmQLrRcqSEsbUv6yuVFW9xYZ2T3fd4w5N8"
            iClen = sCodeString.Length

            sAString = sLicence
            iCntr = sAString.Length

            If iCntr < 1 Then
                result = gPMConstants.PMEReturnCode.PMFalse

                sLicenceKey = ""

                Return result
            End If

            sChar1 = New StringBuilder(sCodeString.ToString.Substring((Strings.AscW(sAString.ToString.Substring(0, 1)(0)) + iCntr) Mod iClen, 1))
            sChar2 = New StringBuilder(sCodeString.ToString.Substring(Strings.AscW(sAString.ToString.Substring(sAString.Length - 1)(0)) Mod iClen, 1))
            iSn = ((Strings.AscW(sChar1.ToString) + Strings.AscW(sChar2.ToString)) Mod iClen) + 1
            sBString = sChar2

            For iCntr2 As Integer = 1 To iCntr
                sBString.Append(sCodeString.Substring((Strings.AscW(sAString.ToString.Substring(iCntr2 - 1, 1)(0)) + iSn + iCntr2) Mod iClen, 1))
            Next iCntr2

            sBString.Append(sChar1.ToString)

            ' Return the result.
            sLicenceKey = sBString.ToString().Trim()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LicenceEncrypt Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LicenceEncrypt", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetCommandLine
    '
    ' Description: Gets the command line passed and splits it down
    '              into the seporate arguments.
    '
    ' ***************************************************************** '
    ' ************* If you change this function you MUST also change
    ' ************* the iPMFunc version.
    ' *************
    Public Function GetCommandLine(ByRef vArgArray As Object, Optional ByRef vMaxArgs As Object = Nothing) As Integer

        'Declare variables.
        Dim result As Integer = 0
        Dim sChar As String
        Dim sCmdLine As String = ""
        Dim iCmdLineLen As Integer
        Dim bInArg As Boolean
        Dim iMaxArgs, iNumArgs As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'See if MaxArgs was provided.

            Dim dbNumericTemp As Double

            If (Not vMaxArgs Is Nothing) And (Double.TryParse(CStr(vMaxArgs), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then

                iMaxArgs = CInt(vMaxArgs)
            Else
                iMaxArgs = 100
            End If

            'Make array of the correct size.
            ReDim vArgArray(iMaxArgs)

            ' Initialise
            iNumArgs = 0
            bInArg = False

            'Get command line arguments.
            'sCmdLine = Interaction.Command()

            ' Get the length of the command line
            iCmdLineLen = sCmdLine.Length

            'Go thru command line one character at a time.
            For iSub As Integer = 1 To iCmdLineLen

                'sChar = Mid(sCmdLine, iSub, 1)
                sChar = sCmdLine.Substring(iSub, 1)

                'Test for space or tab.
                If (sChar <> " ") And (sChar <> Strings.ChrW(9)) Then

                    'Neither space nor tab.

                    'Test if already in argument.
                    If Not bInArg Then

                        'New argument begins.

                        'Test for too many arguments.
                        If iNumArgs >= iMaxArgs Then
                            Exit For
                        End If

                        iNumArgs += 1
                        bInArg = True

                    End If

                    'Concatenate character to current argument.


                    vArgArray(iNumArgs - 1) = CStr(vArgArray(iNumArgs - 1)) & sChar

                Else

                    'Found a space or tab.
                    'Set bInArg flag to False.
                    bInArg = False

                End If

            Next iSub

            'Resize array just enough to hold arguments.
            If iNumArgs > 0 Then
                ReDim Preserve vArgArray(iNumArgs - 1)
            Else

                vArgArray = ""
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.
            result = gPMConstants.PMELogLevel.PMLogOnError

            ' Log Error.
            LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Process the Command Line :- " & sCmdLine, vApp:=ACApp, vClass:=ACClass, vMethod:="GetCommandLine", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ValidateSQL
    '
    ' Description: Given a database type and an SQL string, this function
    ' validates the SQL for the given DB type and returns an appropriately
    ' ammended SQL string.
    '
    ' ***************************************************************** '
    '
    Public Function ValidateSQL(ByRef sSQLStatement As String) As Integer

        Dim result As Integer = 0
        Dim sTmpSQL As New StringBuilder


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'We are validating for SQL Server

            'Loop thru each character
            For i As Integer = 1 To sSQLStatement.Length

                'Should not have any characters that are before
                '<space> in the ascii character set
                If Strings.AscW(sSQLStatement.Substring(i - 1, 1)(0)) < Strings.AscW(" "c) Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    'message invalid SQL
                    'LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Invalid SQL - " & sSQLStatement, vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateSQL", vErrDesc:=EXP.Description)

                    Return result

                End If

                'Quotes must be doubled up
                If sSQLStatement.Substring(i - 1, 1) = "'" Then

                    sTmpSQL.Append("''")

                Else

                    sTmpSQL.Append(sSQLStatement.Substring(i - 1, 1))

                End If

            Next i



            'Everything's great
            sSQLStatement = sTmpSQL.ToString()

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateSQL", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetGUID

    ' Description: Generate a Globaly Unique Identifier.
    '              Copied from Technet Article.
    ' RFC 11/08/00 - Get GUID function
    ' ***************************************************************** '
    Public Function GetGUID() As String

        Dim newGuid As Guid = Guid.NewGuid

        Return newGuid.ToString

    End Function

    ' ***************************************************************** '
    ' Name: ConvertDecToBaseN

    ' Description: Accepts a decimal value and converts it to a specified Base
    '              and returns the result as a string.  e.g. Base 16 (Hex) will
    '              convert 10 to A etc.
    '
    ' RT 28/09/00 - Created
    ' ***************************************************************** '
    Public Function ConvertDecToBaseN(ByVal dValue As Double, Optional ByVal byBase As Byte = 16) As String

        Const BASENUMBERS As String = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ"

        Dim sResult As String = ""
        Dim dRemainder As Double

        Try

            sResult = ""

            ' Check that the base passed in is within the range of 2 to 36
            If Not ((byBase < 2) Or (byBase > 36)) Then

                ' Get the Absolute value
                dValue = Math.Abs(dValue)

                Do

                    ' Work out the position of the character in the constant.
                    dRemainder = dValue - (byBase * Math.Floor(dValue / byBase))
                    ' Reteieve the character out of the constant
                    sResult = BASENUMBERS.Substring(CInt(dRemainder + 1) - 1, 1) & sResult
                    ' Reset value to the remaining integer
                    dValue = Math.Floor(dValue / byBase)

                Loop While (dValue > 0)

            End If


            Return sResult

        Catch excep As System.Exception
            LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="ConvertDecToBaseN Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ConvertDecToBaseN", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
            Return Nothing
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ConvertBaseNToDec

    ' Description: Accepts a String value and converts it from a specified Base
    '              and returns the result as a string.  e.g. Using Base 16 (Hex) will
    '              convert A to 10 etc.
    '
    ' RT 28/09/00 - Created
    ' ***************************************************************** '
    Public Function ConvertBaseNToDec(ByVal dValue As String, Optional ByVal byBase As Byte = 16) As String

        Const BASENUMBERS As String = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ"

        Dim sResult As String = ""
        Dim lReturn As Double
        Dim n As Integer

        Try

            ' Check that the base passed in is within the range of 2 to 36
            If (byBase < 2) Or (byBase > 36) Then Return ""

            n = 0

            Do
                lReturn = ((BASENUMBERS.IndexOf(dValue.Substring(dValue.Length - n, 1))) * (byBase ^ n)) + lReturn
                n += 1
            Loop Until n = dValue.Length


            Return CStr(lReturn)

        Catch excep As System.Exception
            ' Log Error Message
            LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="ConvertBaseNToDec Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ConvertBaseNToDec", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
            Return ""
        End Try
    End Function

    Public Function CreateLateBoundObject(ByVal ClassName As String) As Object
        Dim sPurePath As String

        Try
            sPurePath = New Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath
            sPurePath = Path.GetDirectoryName(sPurePath)

            Dim libraryPath As String
            Dim DLLAssembly As [Assembly]

            libraryPath = Path.Combine(sPurePath, ClassName.Substring(0, ClassName.IndexOf(".")) & ".dll")
            If IO.File.Exists(libraryPath) Then
                DLLAssembly = [Assembly].LoadFrom(libraryPath)
                Return DLLAssembly.CreateInstance(ClassName, True)
            End If

            libraryPath = Path.Combine(sPurePath, ClassName.Substring(0, ClassName.IndexOf(".")) & ".exe")
            If IO.File.Exists(libraryPath) Then
                DLLAssembly = [Assembly].LoadFrom(libraryPath)
                Return DLLAssembly.CreateInstance(ClassName, True)
            End If

            Throw New IO.FileNotFoundException("Cannot find " & libraryPath & " in " & sPurePath & " as exe or dll")
        Catch excep As Exception
            LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="CreateLateBoundObject Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateLateBoundObject", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' Creates object of class at rule folder eg. C:/Pure/Rules
    ''' </summary>
    ''' <param name="ClassName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CreateLateBoundObject_CompiledRules(ByVal ClassName As String) As Object
        Dim sRulePath As String
        Dim sSubKey As String = "GIS"
        Dim result As Integer = 0

        Dim libraryPath As String
        Dim DLLAssembly As [Assembly]

        Try
            result = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="RulePath", r_sSettingValue:=sRulePath, v_sSubKey:=sSubKey), gPMConstants.PMEReturnCode)
            If result <> 1 Then
                Throw New IO.FileNotFoundException("Cannot find " & ClassName & " in " & sRulePath & " as exe or dll")
            End If
            If sRulePath <> "" Then
                If Not sRulePath.EndsWith("\") Then
                    sRulePath = sRulePath & "\"
                End If
            End If

            libraryPath = Path.Combine(sRulePath, ClassName.Substring(0, ClassName.IndexOf(".")) & ".dll")
            If IO.File.Exists(libraryPath) Then
                DLLAssembly = [Assembly].LoadFrom(libraryPath)
                Return DLLAssembly.CreateInstance(ClassName, True)
            End If

            libraryPath = Path.Combine(sRulePath, ClassName.Substring(0, ClassName.IndexOf(".")) & ".exe")
            If IO.File.Exists(libraryPath) Then
                DLLAssembly = [Assembly].LoadFrom(libraryPath)
                Return DLLAssembly.CreateInstance(ClassName, True)
            End If

            Throw New IO.FileNotFoundException("Cannot find " & libraryPath & " in " & sRulePath & " as exe or dll")
        Catch excep As Exception
            LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="CreateLateBoundObject_CompiledRules Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateLateBoundObject_CompiledRules", excep:=excep)


        End Try
        Return Nothing
    End Function

    ' ******************************************************'
    '
    ' Name: retrieveProductOptions
    '
    ' Description: This will create a business object to
    '               populate the array
    '
    ' History: 18/06/2002 SJP - Created.
    '
    '*******************************************************'
    ' REMOVE GLOBAL VARIABLES
    'Public Function retrieveProductOptions(ByRef r_vProductOptions) As Long
    Public Function retrieveProductOptions(ByVal v_sUsername As String, ByVal v_sPassword As String, ByVal v_iUserID As Integer, ByVal v_iSourceID As Integer, ByVal v_iLanguageID As Integer, ByVal v_iCurrencyID As Integer, ByVal v_iLogLevel As Integer, ByVal v_sCallingAppName As String, ByRef r_vProductOptions As Object) As Integer

        Dim result As Integer = 0
        Dim oProductOption As Object
        Dim lErrorValue As gPMConstants.PMEReturnCode

        Try

            ' Create Product Options object
            oProductOption = CreateLateBoundObject("bSIRProductOptions.Business")

            ' Initialise Product Options object
            lErrorValue = oProductOption.Initialise(sUsername:=v_sUsername.ToString, sPassword:=v_sPassword.ToString, iUserID:=CInt(v_iUserID), iSourceID:=CInt(v_iSourceID), iLanguageID:=CInt(v_iLanguageID), iCurrencyID:=CInt(v_iCurrencyID), iLogLevel:=CInt(v_iLogLevel), sCallingAppName:=v_sCallingAppName.ToString)

            If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then

                oProductOption = Nothing

                ' Error message
                LogMessage(sUsername:=v_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initiate Product Options object", vApp:=ACApp, vClass:=ACClass, vMethod:="getProductOption", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result

            End If

            '   Get the business object to populate the array
            lErrorValue = oProductOption.getAllHiddenOptions(r_vResultArray:=r_vProductOptions)

            '   Should find at least one value in the database
            If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lErrorValue

                oProductOption = Nothing

                LogMessage(sUsername:=v_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve product options", vApp:=ACApp, vClass:=ACClass, vMethod:="getProductOption", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            oProductOption = Nothing


            Return lErrorValue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="retrieveProductOptions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="retrieveProductOptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function




    ' ***************************************************************** '
    ' Name: RetrieveSingleSystemOption
    '
    ' Description:  gets the system option required for the current branch
    '               held in g_iSourceID
    '
    ' History: SW 07/04/2003
    '
    ' ***************************************************************** '
    ' REMOVE GLOBAL VARIABLES
    'Public Function RetrieveSingleSystemOption(v_iOptionNumber As Integer, _
    ''                                           r_sOptionValue As String, _
    ''                                           Optional v_iSourceID As Integer) As Long
    Public Function RetrieveSingleSystemOption(ByVal v_sUsername As String, ByVal v_sPassword As String, ByVal v_iUserID As Integer, ByVal v_iMainSourceID As Integer, ByVal v_iLanguageID As Integer, ByVal v_iCurrencyID As Integer, ByVal v_iLogLevel As Integer, ByVal v_sCallingAppName As String, ByRef v_iOptionNumber As Integer, ByRef r_sOptionValue As String, Optional ByRef v_iSourceID As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim oSystemOptions As Object
        Dim lResult As gPMConstants.PMEReturnCode
        ' Throw New NotImplementedException()

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Create the System Options Object
            oSystemOptions = CreateLateBoundObject("bSIROptions.Business")

            lResult = oSystemOptions.Initialise(sUsername:=ToSafeString(v_sUsername), sPassword:=ToSafeString(v_sPassword), iUserID:=ToSafeInteger(v_iUserID), iSourceID:=ToSafeInteger(v_iMainSourceID),
                                                iLanguageID:=ToSafeInteger(v_iLanguageID), iCurrencyID:=ToSafeInteger(v_iCurrencyID), iLogLevel:=ToSafeInteger(v_iLogLevel), sCallingAppName:=ToSafeString(ACApp))

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'get the system option
            lResult = oSystemOptions.GetOption(iOptionNumber:=ToSafeInteger(v_iOptionNumber), sValue:=ToSafeString(r_sOptionValue), v_iSourceID:=ToSafeInteger(v_iSourceID))

            r_sOptionValue = ToSafeString(oSystemOptions.SystemOptionValue)

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                oSystemOptions.Dispose()
                oSystemOptions = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                oSystemOptions.Dispose()
                oSystemOptions = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSystemOptions.Dispose()
            oSystemOptions = Nothing


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            ' sw 07/04/2003: Raise the error to the calling function so it can be logged correctly
            Throw New System.Exception(Information.Err().Number.ToString() + ", " + Information.Err().Source + ", " + Information.Err().Description + ", " + Information.Err().HelpFile + ", " + Information.Err().HelpContext)

        Finally


        End Try
        Return result

    End Function

    ' ***************************************************************************
    ' Moved from gSirLibrary as Part of the Global Data Changes
    ' ***************************************************************************

    ' ******************************************************'
    '
    ' Name: getProductOption
    '
    ' Description: This will retrieve all product options from
    '               the business option if not already retrieved
    '               It will then find a value for the option
    '
    ' History: 06/06/2002 SJP - Created.
    '
    ' RFC15102003 Moved from gSirLibrary as Part of the Global Data Changes
    '*******************************************************'
    Private Function getProductOption(ByVal v_sUsername As String, ByVal v_sPassword As String,
                                      ByVal v_iUserID As Integer,
                                      ByVal v_iMainSourceID As Integer,
                                      ByVal v_iLanguageID As Integer,
                                      ByVal v_iCurrencyID As Integer,
                                      ByVal v_iLogLevel As Integer,
                                      ByVal v_sCallingAppName As String,
                                      ByVal v_vOptionNumber As gPMConstants.SIRHiddenOptions,
                                      ByVal v_vBranch As Integer, ByRef r_vUnderwriting As String,
                                      ByVal v_bValue As Boolean) As Integer


        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode



        lReturn = gPMConstants.PMEReturnCode.PMTrue

        '   This will check whether array is empty
        '   If so then it will create a business object to retrieve
        '   into the array
        vProductOptions = Nothing

        ' REMOVE GLOBAL VARIABLES

        lReturn = CType(retrieveProductOptions(v_sUsername, v_sPassword, v_iUserID, v_iMainSourceID,
                                               v_iLanguageID, v_iCurrencyID, v_iLogLevel,
                                               v_sCallingAppName, vProductOptions), gPMConstants.PMEReturnCode)

        '   If product options have been found then will return value
        If lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            lReturn = CType(findValueInArray(v_vOptionNumber, v_vBranch, r_vUnderwriting, v_bValue), gPMConstants.PMEReturnCode)
        End If


        Return lReturn

    End Function

    ' ******************************************************'
    '
    ' Name: getProductOptionValue
    '
    ' Description: This provides a public interface for Product Options
    '
    ' History: 06/06/2002 SJP - Created.
    '
    ' RFC15102003 Moved from gSirLibrary as Part of the Global Data Changes
    '*******************************************************'
    Public Function getProductOptionValue(ByVal v_sUsername As String, ByVal v_sPassword As String, ByVal v_iUserID As Integer, ByVal v_iMainSourceID As Integer, ByVal v_iLanguageID As Integer, ByVal v_iCurrencyID As Integer, ByVal v_iLogLevel As Integer, ByVal v_sCallingAppName As String, ByVal v_vOptionNumber As gPMConstants.SIRHiddenOptions, ByVal v_vBranch As Integer, ByRef r_vUnderwriting As String) As Integer

        Dim result As Integer = 0
        Try


            Return getProductOption(v_sUsername, v_sPassword, v_iUserID, v_iMainSourceID, v_iLanguageID, v_iCurrencyID, v_iLogLevel, v_sCallingAppName, v_vOptionNumber, v_vBranch, r_vUnderwriting, True)

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="getProductOptionValue", Information.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    ' ******************************************************'
    '
    ' Name: getUnderwritingOrAgency
    '
    ' Description: This provides a replacement for the legacy
    '               UnderwritingOrAgency interface
    '
    ' History: 06/06/2002 SJP - Created.
    '
    ' RFC15102003 Moved from gSirLibrary as Part of the Global Data Changes
    '*******************************************************'
    Public Function getUnderwritingOrAgency(ByVal v_sUsername As String, ByVal v_sPassword As String, ByVal v_iUserID As Integer, ByVal v_iMainSourceID As Integer, ByVal v_iLanguageID As Integer, ByVal v_iCurrencyID As Integer, ByVal v_iLogLevel As Integer, ByVal v_sCallingAppName As String, ByRef r_vUnderwriting As String) As Integer

        Dim result As Integer = 1
        Try

            r_vUnderwriting = "U"

            'result = getProductOption(v_sUsername, v_sPassword, v_iUserID, v_iMainSourceID, v_iLanguageID, v_iCurrencyID, v_iLogLevel, v_sCallingAppName, gPMConstants.SIRHiddenOptions.SIROPTUnderwriting, gPMConstants.SIRBCHHeadOffice, r_vUnderwriting, True)

            'If result = gPMConstants.PMEReturnCode.PMTrue Then
            '    If (r_vUnderwriting <> "A") And (r_vUnderwriting <> "U") Then
            '        r_vUnderwriting = "A"
            '    End If
            'End If
            'gPMFunctions.LogMessageToFile(sUsername:="", iType:=4, sMsg:=getUnderwritingOrAgency, vApp:=CStr("Navneet"), vClass:=CStr("Third"), vMethod:=CStr("abc"), vErrNo:=CStr("vErrNo"), vErrDesc:=CStr("vErrDesc"))
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getUnderwritingOrAgency Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="getUnderwritingOrAgency", Information.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    ' ******************************************************'
    '
    ' Name: getUnderwritingType
    '
    ' Description: This provides a replacement for the legacy
    '               UnderwritingType interface
    '
    ' History: 06/06/2002 SJP - Created.
    '
    ' RFC15102003 Moved from gSirLibrary as Part of the Global Data Changes
    '*******************************************************'
    Public Function getUnderwritingType(ByVal v_sUsername As String, ByVal v_sPassword As String, ByVal v_iUserID As Integer, ByVal v_iMainSourceID As Integer, ByVal v_iLanguageID As Integer, ByVal v_iCurrencyID As Integer, ByVal v_iLogLevel As Integer, ByVal v_sCallingAppName As String, ByRef r_vUnderwriting As String) As Integer

        Dim result As Integer = 0
        Try

            r_vUnderwriting = "U"

            result = getProductOption(v_sUsername, v_sPassword, v_iUserID, v_iMainSourceID, v_iLanguageID, v_iCurrencyID, v_iLogLevel, v_sCallingAppName, gPMConstants.SIRHiddenOptions.SIROPTUnderwriting, gPMConstants.SIRBCHHeadOffice, r_vUnderwriting, False)

            If result = gPMConstants.PMEReturnCode.PMTrue Then
                If (r_vUnderwriting <> "A") And (r_vUnderwriting <> "U") Then
                    r_vUnderwriting = "U"
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getUnderwritingType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="getUnderwritingType", Information.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    ' ******************************************************'
    '
    ' Name: findValueInArray
    '
    ' Description: This will find the value in the product options array
    '
    ' History: 06/06/2002 SJP - Created.
    '
    ' RFC15102003 Moved from gSirLibrary as Part of the Global Data Changes
    '*******************************************************'
    Private Function findValueInArray(ByVal v_vOptionNumber As gPMConstants.SIRHiddenOptions, ByVal v_vBranch As Integer, ByRef r_vUnderwriting As String, ByVal v_bValue As Boolean) As Integer

        Dim result As Integer = 0
        Dim i As Integer



        result = gPMConstants.PMEReturnCode.PMTrue
        r_vUnderwriting = ""

        '   Find the option Number within the array
        If Not (vProductOptions Is Nothing) Then
            For i = 0 To vProductOptions.GetUpperBound(1)

                If CInt(vProductOptions(0, i)) = v_vOptionNumber Then

                    If CInt(vProductOptions(1, i)) = v_vBranch Then
                        Exit For
                    End If
                End If
            Next i

            '   If it was not found then find the option for the Head Office value

            If i > vProductOptions.GetUpperBound(1) Then
                v_vBranch = gPMConstants.SIRBCHHeadOffice

                For i = 0 To vProductOptions.GetUpperBound(1)

                    If CInt(vProductOptions(0, i)) = v_vOptionNumber Then

                        If CInt(vProductOptions(1, i)) = v_vBranch Then
                            Exit For
                        End If
                    End If
                Next i
            End If

            '   If found then get the value or UnderwritingType (only used by getUnderwritingType)

            Dim tempString As String = ""
            If i <= vProductOptions.GetUpperBound(1) Then
                If v_bValue Then

                    tempString = CStr(vProductOptions(2, i))
                Else

                    tempString = CStr(vProductOptions(3, i))
                End If

                If Not (Convert.IsDBNull(tempString) Or (tempString Is Nothing)) Then
                    r_vUnderwriting = tempString.Trim()
                End If
            End If
        End If
        Return result

    End Function


    ' ***************************************************************** '
    ' Name: GetSystemOption
    '
    ' Description:  gets the system option required for the Source passed
    '               if v_iSourceID is ommitted then the branch will be taken from the global
    '               variable g_iSourceID
    '
    ' History: SW 07/04/2003
    '
    ' RFC15102003 Moved from gPMFunctions as Part of the Global Data Changes
    ' ***************************************************************** '
    Public Function GetSystemOption(ByVal v_sUsername As String, ByVal v_sPassword As String, ByVal v_iUserID As Integer, ByVal v_iMainSourceID As Integer, ByVal v_iLanguageID As Integer, ByVal v_iCurrencyID As Integer, ByVal v_iLogLevel As Integer, ByVal v_sCallingAppName As String, ByRef v_iOptionNumber As Integer, ByRef r_sOptionValue As String, Optional ByRef v_iSourceID As Integer = 0) As Integer
        Dim result As Integer = 0
        Dim oSystemOptions As Object


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' REMOVE GLOBAL VARIABLES
            If RetrieveSingleSystemOption(v_sUsername, v_sPassword, v_iUserID, v_iMainSourceID, v_iLanguageID, v_iCurrencyID, v_iLogLevel, v_sCallingAppName, v_iOptionNumber, r_sOptionValue, v_iSourceID) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            ' sw 07/04/2003: Raise the error to the calling function so it can be logged correctly
            'Throw New System.Exception(Information.Err().Number.ToString() + ", " + Information.Err().Source + ", " + Information.Err().Description + ", " + Information.Err().HelpFile + ", " + Information.Err().HelpContext)

        Finally


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetBranchBaseCurrency
    '
    ' Description:  gets base currency for branch
    '
    ' History: RDC 15062004 created
    ' ***************************************************************** '
    'PN30098 - Datasure
    Public Function GetBranchBaseCurrency(ByVal v_lSourceID As Integer, ByVal v_oDatabase As dPMDAO.Database, ByRef r_iCurrencyID As Integer, Optional ByRef r_sCurSymbol As String = "") As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse
            'PN30098 - Datasure
            If Not True Then
                sSQL = "SELECT base_currency_id FROM source "
                sSQL = sSQL & "WHERE source_id = " & CStr(v_lSourceID)
            Else
                sSQL = "SELECT DISTINCT Source.base_currency_id AS base_currency_id,Currency.symbol AS Symbol From"
                sSQL = sSQL & " Source INNER JOIN Currency ON Source.base_currency_id = Currency.currency_id AND"
                sSQL = sSQL & " Currency.currency_id =(SELECT base_currency_id From Source WHERE source_id = " & CStr(v_lSourceID) & ")"
            End If

            With v_oDatabase


                .Parameters.Clear()


                m_lReturn = v_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetBranchBaseCurrency", bStoredProcedure:=False)


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

                'r_iCurrencyID = .Records.Fields("base_currency_id").Value
                r_iCurrencyID = ToSafeInteger(.Records.Fields("base_currency_id"))
                'PN30098 - Datasure
                If True Then
                    'r_sCurSymbol = gPMFunctions.ToSafeString(.Records.Fields("Symbol").Value)
                    r_sCurSymbol = gPMFunctions.ToSafeString(.Records.Fields("Symbol"))
                End If

            End With


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            'Log Error Message
            LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBranchBaseCurrency failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBranchBaseCurrency", Information.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetBranchCurrencies
    '
    ' Description:  gets currencies available to supplied branch
    '
    ' History: RDC 02062004 created
    ' VB 19/04/2005 PN19896 : New parameter added for restricting the deleted currency.
    ' ***************************************************************** '
    Public Function GetBranchCurrencies(ByVal v_iSourceID As Integer, ByVal v_oDatabase As dPMDAO.Database, ByRef r_vReturnArray(,) As Object, Optional ByVal v_bRestrictDeletedCurrency As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Deleted currencies displays with 'Deleted' suffix.
            sSQL = "SELECT cy.currency_id, CASE cy.is_deleted WHEN 1 THEN RTRIM(cy.[description])"
            sSQL = sSQL & "+ ' (Deleted)' WHEN 0 THEN cy.[description] END FROM CompanyCurrency cc "
            sSQL = sSQL & "INNER JOIN currency cy ON cc.currency_id=cy.currency_id "
            sSQL = sSQL & "WHERE company_id = " & CStr(v_iSourceID)

            If v_bRestrictDeletedCurrency Then
                sSQL = sSQL & " AND cy.is_deleted <>1"
            End If

            With v_oDatabase


                m_lReturn = v_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="RetrieveCurrenciesForBranch", bStoredProcedure:=False, vResultArray:=r_vReturnArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

            End With


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            'Log Error Message
            LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBranchCurrencies Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBranchCurrencies", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    Public Function GetCurrencyAuthorities(ByVal v_iUserID As Integer, ByVal v_oDatabase As dPMDAO.Database, ByRef r_bChangeDate As Boolean, ByRef r_bChangeRate As Boolean) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResult(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            sSQL = "SELECT can_change_exchange_date, can_change_exchange_rate" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "FROM user_authorities ua" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "JOIN PMUser pmu ON pmu.user_id = ua.user_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE pmu.user_id = " & CStr(v_iUserID)


            m_lReturn = v_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetCurrencyAuthorities", bStoredProcedure:=False, vResultArray:=vResult)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or Not (vResult Is Nothing) Then
                Return result
            End If


            r_bChangeDate = CStr(vResult(0, 0)) = "1"

            r_bChangeRate = CStr(vResult(1, 0)) = "1"


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCurrencyAuthorities Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCurrencyAuthorities", Information.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetSystemSecurityModel
    '
    ' Description:  Returns the SystemSecurityModel from Product Option
    '
    ' ***************************************************************** '
    Public Function GetSystemSecurityModel(ByRef vValue As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'SystemSecurityModel is NOT Branch specific.
            'It should always use the default Branch.

            m_lReturn = CType(getProductOptionValue(v_sUsername:="", v_sPassword:="", v_iUserID:=0, v_iMainSourceID:=0, v_iLanguageID:=0, v_iCurrencyID:=0, v_iLogLevel:=0, v_sCallingAppName:="", v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTAlternativeLogon, v_vBranch:=1, r_vUnderwriting:=vValue), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSystemSecurityModel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSystemSecurityModel", Information.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function
    'Added for DPMDAO
    Public Sub InitGlobalVars()
        m_lReturn = 0
        vProductOptions = Nothing
        m_lReturn = gPMConstants.PMEReturnCode.PMFalse
        vProductOptions = Nothing
    End Sub

    'Added by Deepak Sharma for Reinsurance
    Public Sub TransposeArray(ByRef vArray(,) As Object)
        Try
            Dim vArrayTemp(vArray.GetUpperBound(1), vArray.GetUpperBound(0)) As Object
            For iCount As Integer = 0 To vArray.GetUpperBound(0)
                For jCount As Integer = 0 To vArray.GetUpperBound(1)
                    vArrayTemp(jCount, iCount) = vArray(iCount, jCount)
                Next
            Next
            vArray = vArrayTemp
        Catch ex As Exception

        End Try
    End Sub
    Public Function CheckPassword(ByVal password As String, ByVal hashedpassword As String) As Integer
        Dim result As Integer = 0
        Try
            If BCrypt.Net.BCrypt.Verify(password, hashedpassword) = True Then
                result = gPMConstants.PMEReturnCode.PMTrue
                Return result
            Else
                result = gPMConstants.PMEReturnCode.PMFalse
            End If
        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMFalse
        End Try
        Return result
    End Function
    ''' <summary>
    ''' SHA256Hash
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SHA256Hash(ByVal value As String) As Byte()
        Using sha256 As SHA256 = SHA256.Create()
            Return sha256.ComputeHash(Encoding.UTF8.GetBytes(value))
        End Using
    End Function
    ''' <summary>
    ''' EncryptPassword
    ''' </summary>
    ''' <param name="sValueToEncrypt"></param>
    ''' <param name="sKey"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ' AES-CBC Encryption with random IV
    Public Function EncryptPassword(ByVal sValueToEncrypt As String, ByVal sKey As String) As String
        Dim key As Byte() = SHA256Hash(sKey)
        Using aes As Aes = Aes.Create()
            aes.Key = key
            aes.Mode = CipherMode.CBC
            aes.Padding = PaddingMode.PKCS7
            aes.GenerateIV() ' Random IV for each encryption

            Dim encryptor As ICryptoTransform = aes.CreateEncryptor()
            Dim plainBytes As Byte() = Encoding.UTF8.GetBytes(sValueToEncrypt)
            Dim encryptedBytes As Byte() = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length)

            ' Prepend IV to the encrypted data
            Dim resultBytes As Byte() = New Byte(aes.IV.Length + encryptedBytes.Length - 1) {}
            Buffer.BlockCopy(aes.IV, 0, resultBytes, 0, aes.IV.Length)
            Buffer.BlockCopy(encryptedBytes, 0, resultBytes, aes.IV.Length, encryptedBytes.Length)

            Return Convert.ToBase64String(resultBytes)
        End Using
    End Function
    ''' <summary>
    ''' DecryptPassword
    ''' </summary>
    ''' <param name="sEncryptedString"></param>
    ''' <param name="sKey"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ' AES-CBC Decryption
    Public Function DecryptPassword(ByVal sEncryptedString As String, ByVal sKey As String) As String
        Try
            Dim key As Byte() = SHA256Hash(sKey)
            Dim fullCipher As Byte() = Convert.FromBase64String(sEncryptedString)

            Using aes As Aes = Aes.Create()
                aes.Key = key
                aes.Mode = CipherMode.CBC
                aes.Padding = PaddingMode.PKCS7

                ' Extract IV (first 16 bytes) and cipher text
                Dim iv(15) As Byte
                Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length)
                aes.IV = iv

                Dim cipherText As Byte() = New Byte(fullCipher.Length - iv.Length - 1) {}
                Buffer.BlockCopy(fullCipher, iv.Length, cipherText, 0, cipherText.Length)

                Dim decryptor As ICryptoTransform = aes.CreateDecryptor()
                Dim decryptedBytes As Byte() = decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length)
                Return Encoding.UTF8.GetString(decryptedBytes)
            End Using
        Catch excep As Exception
            LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Decrypt Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Decrypt", Information.Err().Number, vErrDesc:=excep)
            Return String.Empty
        End Try
    End Function
    Public Function GetOVal(ByVal encryptedtext As String) As String
        Dim sRetVal As String = ""

        Dim TripleDes As New TripleDESCryptoServiceProvider
        Dim sKey As String = "!@$1R1U5"
        TripleDes.Key = TruncateHash(sKey, TripleDes.KeySize \ 8)
        TripleDes.IV = TruncateHash("", TripleDes.BlockSize \ 8)


        Try

            ' Convert the encrypted text string to a byte array. 
            Dim encryptedBytes() As Byte = Convert.FromBase64String(encryptedtext)

            ' Create the stream. 
            Dim ms As New System.IO.MemoryStream
            ' Create the decoder to write to the stream. 
            Dim decStream As New CryptoStream(ms,
                TripleDes.CreateDecryptor(),
                System.Security.Cryptography.CryptoStreamMode.Write)

            ' Use the crypto stream to write the byte array to the stream.
            decStream.Write(encryptedBytes, 0, encryptedBytes.Length)
            decStream.FlushFinalBlock()

            ' Convert the plaintext stream to a string. 
            sRetVal = System.Text.Encoding.Unicode.GetString(ms.ToArray)

            TripleDes = Nothing
        Catch ex As Exception
            Return ""
        End Try

        Return sRetVal
    End Function
    Private Function TruncateHash(ByVal key As String, ByVal length As Integer) As Byte()

        Dim sha1 As New SHA1CryptoServiceProvider

        ' Hash the key. 
        Dim keyBytes() As Byte =
            System.Text.Encoding.Unicode.GetBytes(key)
        Dim hash() As Byte = sha1.ComputeHash(keyBytes)

        ' Truncate or pad the hash. 
        ReDim Preserve hash(length - 1)
        Return hash
    End Function

    Public Function GetEVal(ByVal plaintext As String) As String
        Dim sKey As String = "!@$1R1U5"

        Dim TripleDes As New TripleDESCryptoServiceProvider

        TripleDes.Key = TruncateHash(sKey, TripleDes.KeySize \ 8)
        TripleDes.IV = TruncateHash("", TripleDes.BlockSize \ 8)

        Dim sRetVal As String = ""

        ' Convert the plaintext string to a byte array. 
        Dim plaintextBytes() As Byte =
            System.Text.Encoding.Unicode.GetBytes(plaintext)

        ' Create the stream. 
        Dim ms As New System.IO.MemoryStream
        ' Create the encoder to write to the stream. 
        Dim encStream As New CryptoStream(ms,
                                    TripleDes.CreateEncryptor(),
                                    System.Security.Cryptography.CryptoStreamMode.Write)

        ' Use the crypto stream to write the byte array to the stream.
        encStream.Write(plaintextBytes, 0, plaintextBytes.Length)
        encStream.FlushFinalBlock()

        ' Convert the encrypted stream to a printable string. 
        sRetVal = Convert.ToBase64String(ms.ToArray)

        TripleDes = Nothing
        Return sRetVal
    End Function

    Public Function GetSystemOption(ByRef v_iOptionNumber As Integer, ByRef r_sOptionValue As String, Optional ByRef v_iSourceID As Integer = 0) As Integer
        Dim result As Integer = 0
        Dim oSystemOptions As Object


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If RetrieveSingleSystemOption(v_iOptionNumber, r_sOptionValue, v_iSourceID) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            ' sw 07/04/2003: Raise the error to the calling function so it can be logged correctly
            Throw New System.Exception(Information.Err().Number.ToString() + ", " + Information.Err().ToString + ", " + Information.Err().Description + ", " + Information.Err().ToString + ", " + Information.Err().ToString)

        Finally


        End Try
        Return result
    End Function
    Public Function RetrieveSingleSystemOption(ByRef v_iOptionNumber As Integer, ByRef r_sOptionValue As String, Optional ByRef v_iSourceID As Integer = 0) As Integer
        Dim result As Integer = 0
        Dim bSIROptions As Object


        Dim oSystemOptions As Object
        Dim oObjectManager As Object
        Dim lResult As gPMConstants.PMEReturnCode


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the object manager and Initialise
            oObjectManager = CreateObject("bObjectManager.ObjectManager")

            lResult = oObjectManager.Initialise(sCallingAppName:=ACApp)

            '   If not initialised then call error handler
            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oObjectManager = Nothing
                Return result
            End If

            '   Find the Business Class
            Dim temp_oSystemOptions As Object = Nothing
            lResult = oObjectManager.GetInstance(temp_oSystemOptions, "bSIROptions.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oSystemOptions = temp_oSystemOptions

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oObjectManager = Nothing
                Return result
            End If

            'get the system option

            lResult = oSystemOptions.GetOption(iOptionNumber:=CInt(v_iOptionNumber), sValue:=r_sOptionValue.ToString, v_iSourceID:=CInt(v_iSourceID))

            If lResult = gPMConstants.PMEReturnCode.PMNotFound Then
                'Return 0 to stop errors occuring when option is not available
                r_sOptionValue = "0"
            ElseIf lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If


            oSystemOptions.Dispose()
            oSystemOptions = Nothing
            oObjectManager.Dispose()
            oObjectManager = Nothing


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            ' sw 07/04/2003: Raise the error to the calling function so it can be logged correctly
            Throw New System.Exception(Information.Err().Number.ToString() + ", " + Information.Err().ToString + ", " + Information.Err().Description + ", " + Information.Err().ToString + ", " + Information.Err().ToString)

        Finally


        End Try
        Return result
    End Function


End Module

