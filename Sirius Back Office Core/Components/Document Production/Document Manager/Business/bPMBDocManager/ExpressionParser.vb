Option Strict Off
Option Explicit On
Imports System.Globalization
Imports SSP.Shared
Module ExpressionParser
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    '
    ' Name          : ExpressionParser
    '
    ' Description   : Bas Module to Evaluate Mathematical Expression in
    '                 Document Production
    '
    ' Author        : Ram Chandrabose
    '
    ' Edit History  :
    ' RAM20030415   : Created
    '
    ' ***************************************************************** '

    Private Const ACClass As String = "ExpressionParser"

    Private Structure T_VTREC ' Variable Table Record
        Dim name As String ' name of the variable
        Dim value As Object '
        Public Shared Function CreateInstance() As T_VTREC
            Dim result As New T_VTREC
            result.name = String.Empty
            Return result
        End Function
    End Structure

    Private VT() As T_VTREC = Nothing ' Variable Table
    Private VTtop As Integer ' Variable Table's Upper bound

    ' Merge Field markers (These are inserted in Word
    'as "<@" and "@>" but when viewed as flat text appear as
    ' "&lt;@" and "@&gt;" respectively).
    Private m_sFieldStartMarker As String = ""
    Private m_sFieldEndMarker As String = ""
    Private m_iFieldMarkerLength As Integer


    ' ***************************************************************** '
    ' Name          : InitialiseExpressionParser
    ' Description   : Initialise Variable Table Array, and other initialisation
    ' Reference     : Document Issuance Changes.
    '                 Tech Spec 4.4 Scripting 4.4.1.2 Functions
    ' Edit History  :
    ' RAM20030417   : Created
    ' *****************************************************************



    ' ***************************************************************** '
    ' Name          : RemoveWhiteSpaces
    ' Description   : Removed unwanted white spaces
    ' Reference     : Document Issuance Changes.
    '                 Tech Spec 4.4 Scripting 4.4.1.2 Functions
    ' Edit History  :
    ' RAM20030417   : Created
    ' *****************************************************************
    Public Function RemoveWhiteSpaces(ByVal v_sInputString As String) As String


        Dim result As String = ""

        Try

            v_sInputString = v_sInputString.Replace(ChrW(148), ChrW(34))
            v_sInputString = v_sInputString.Replace(ChrW(147), ChrW(34))

            RemoveWhiteSpaces = v_sInputString

        Catch excep As System.Exception

            result = ""

            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RemoveWhiteSpaces Failed for : " & v_sInputString, vApp:=ACApp, vClass:=ACClass, vMethod:="RemoveWhiteSpaces", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name          : GetVariableValue
    ' Description   : Search if Variable already exists in  Variable Table,
    '                   if found, Return the value assigned to that variable
    ' Reference     : Document Issuance Changes.
    '                 Tech Spec 4.4 Scripting 4.4.1.2 Functions
    ' Edit History  :
    ' RAM20030417   : Created
    ' ***************************************************************** '
    Public Function GetVariableValue(ByVal sName As String) As String

        Dim result As String = String.Empty
        Dim iCounter As Integer
        Dim Found As Boolean

        Try

            result = ""

            If VTtop = -1 Then
                ' We dont' have any variable set
                Return result
            End If

            sName = sName.Trim().ToUpper()
            VTtop = VT.GetUpperBound(0)
            Found = False
            For iCounter = 0 To VTtop
                If VT(iCounter).name = sName Then
                    Found = True
                    Exit For
                End If
            Next

            If Found And iCounter >= 0 And iCounter <= VTtop Then

                result = CStr(VT(iCounter).value)
            End If

            Return result

        Catch excep As System.Exception



            result = ""

            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetVariableValue Failed for : " & sName, vApp:=ACApp, vClass:=ACClass, vMethod:="GetVariableValue", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name          : SetVariableValue
    ' Description   : Search if Variable already exists in  Variable Table,
    '                   if not found, add it
    ' Reference     : Document Issuance Changes.
    '                 Tech Spec 4.4 Scripting 4.4.1.2 Functions
    ' Edit History  :
    ' RAM20030417   : Created
    ' ***************************************************************** '
    Public Function SetVariableValue(ByVal sName As String, ByVal vValue As String) As Integer

        Dim result As Integer = 0
        Dim iCounter As Integer
        Dim Found As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If VTtop = -1 Then
                VTtop = 0
                ReDim VT(VTtop)
                VT(VTtop).name = sName

                VT(VTtop).value = vValue
            Else
                VTtop = VT.GetUpperBound(0)
                For iCounter = 0 To VTtop
                    If VT(iCounter).name = sName Then
                        Found = True
                        Exit For
                    End If
                Next

                If Found Then
                    ' We have an existing variable
                    ' We need to store it

                    VT(iCounter).value = vValue
                Else
                    VTtop += 1
                    ReDim Preserve VT(VTtop)
                    VT(VTtop).name = sName

                    VT(VTtop).value = vValue
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetVariableValue Failed for : " & sName, vApp:=ACApp, vClass:=ACClass, vMethod:="SetVariableValue", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name          : ParseFunctionNameAndParams
    ' Description   : Function to parse the input string for a function name
    '                   and its arguments
    ' Reference     : Document Issuance Changes.
    '                 Tech Spec 4.4 Scripting 4.4.1.2 Functions
    ' Notes         : 1. Currently Nesting of Functions are not supported
    '                 2. You can pass Varaiables as an argument to the function
    '                 3. This function assumes that the arguments are
    '                    a) comma separated
    '                    b) string arguments are enclosed in double quotes i.e.  ??
    '                    c) numeric arguments are not enclosed in double quotes
    '                    d) Varaible Expressions are enclosed in <@VAR_n@>
    ' Example Input : FUNC_LCase(?UPPERCASE String?)
    ' Edit History  :
    ' RAM20030416   : Created
    ' PW060404 - CQ5295 - do not replace the ChrW(147) and ChrW(148) with
    '                 quotation marks. There could be a quotation mark in
    '                 the field value, so we can't delimit with it.
    ' ***************************************************************** '
    Public Function ParseFunctionNameAndParams(ByVal v_vInputString As String, ByRef r_sFunctionName As String, ByRef r_vParamArray() As Object) As Integer
        Dim result As Integer = 0

        Dim iStart, iEnd, iLen As Integer
        Dim vArgumentValue As String = String.Empty
        Dim strFunctionName As String = String.Empty
        Dim strArguments As String = String.Empty
        Dim vArgumentsArray() As Object
        Dim iNoofArguments As Integer
        Dim strTemp As String
        Dim strVariableName As String = ""
        Dim vVariableValue As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            vArgumentsArray = Nothing

            If v_vInputString.StartsWith("FUNC_") Then
                'Strip off the leading FUNC_
                v_vInputString = v_vInputString.Substring(5)
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            iStart = 1
            iEnd = (v_vInputString.IndexOf("("c) + 1)
            If iEnd > 1 Then ' !!!! Function name can't be one character
                iEnd -= 1
            End If

            iLen = v_vInputString.Length

            ' Get the Function Name
            strFunctionName = v_vInputString.Substring(iStart - 1, System.Math.Min(v_vInputString.Length, iEnd))

            iStart = iStart + strFunctionName.Length + 1
            ' PW071003 - CQ2759 - Find last ')' because there could be
            ' one in the literal string
            iEnd = If(v_vInputString = "" And ")" = "", 0, (v_vInputString.LastIndexOf(")") + 1))
            If iEnd > 1 Then
                strArguments = v_vInputString.Substring(iStart - 1, System.Math.Min(v_vInputString.Length, iEnd - iStart))
            End If
            strArguments = RemoveDoubleQuotes(strArguments)
            ' Check if we have any arguments
            If strArguments.Length > 0 Then

                ' Note : Some FUNNY things are going on here with the arguments.
                '        If the HTML arguments contains spaces, they are stored either as
                '        ChrW(20) =  or ChrW(160),  which looks like space, but they are not.
                '        so we need to replace then with standard spaces - ChrW(32)
                strArguments = strArguments.Replace(Strings.ChrW(20).ToString(), " ")
                strArguments = strArguments.Replace(Strings.ChrW(160).ToString(), " ")

                ' Make it into Array (since arguments are comma separated)
                If strFunctionName = "CNUM" Or strFunctionName.ToUpper() = "LEN" Then
                    ReDim vArgumentsArray(0)
                    vArgumentsArray(0) = strArguments
                Else
                    vArgumentsArray = ProperSplit(strArguments, ",")
                End If
                ' Check all the arugments for values, it may be any <@VAR_1@>, if so, solve the variable


                iNoofArguments = vArgumentsArray.GetUpperBound(0)

                For iCounter As Integer = 0 To iNoofArguments

                    vArgumentsArray(iCounter) = RemoveDoubleQuotes(CStr(vArgumentsArray(iCounter)))
                    vArgumentValue = CStr(vArgumentsArray(iCounter))

                    ' Resolve the Variable Value

                    ' Check if we have a variable
                    strTemp = vArgumentValue
                    If strTemp.IndexOf(m_sFieldStartMarker) + 1 Then

                        ' Remove the m_sFieldStartMarker ( &lt;@  )
                        strTemp = strTemp.Replace(m_sFieldStartMarker, "")

                        ' Remove the  m_sFieldEndMarker (  @&gt;  )
                        strTemp = strTemp.Replace(m_sFieldEndMarker, "")

                        ' We got the Variable Name
                        strVariableName = strTemp

                        ' Get the Variable's value
                        vVariableValue = GetVariableValue(sName:=strVariableName)

                        ' Set the value back into the Arguments Array

                        vArgumentsArray(iCounter) = vVariableValue

                    End If

                Next iCounter
                Else
                ReDim vArgumentsArray(0)
                vArgumentsArray(0) = ""
            End If

            ' Return the Values
            r_sFunctionName = strFunctionName


            r_vParamArray = vArgumentsArray

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ParseFunctionNameAndParams Failed for : " & v_vInputString, vApp:=ACApp, vClass:=ACClass, vMethod:="ParseFunctionNameAndParams", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name          : CallBuiltInFunction
    ' Description   : Execute the built-in function, and return its result
    ' Reference     : Document Issuance Changes.
    '                 Tech Spec 4.4 Scripting 4.4.1.2 Functions
    ' Note          : The following functions are supported.
    '
    '    Concat(String1, String2)           Concatenates two strings
    '    Mod(Number1, Number2)              Returns the Modulus of two numbers
    '    ChrW(CharCode)                      Returned a character given the ASCII code
    '    InStr(String1, String2)            Returns the position of String2 within String1
    '    LCase(String)                      Converts a string to lowercase
    '    UCase(String)                      Converts a string to Uppercase
    '    Len(String)                        Returns the length of a string
    '    Mid(String, Start, Length)         Returns a portion of a string
    '    StrComp(String1, String2)          Returns 1 is String1 = String2, otherwise 0
    '    String(Number, Character)          Builds a string of a number of characters
    '    informations.Left(String, Length)               Returns a left hand section of a string
    '    Right(String, Length)              Returns a right hand section of a string
    '    LTrim (String)                     Trims the spaces from the left of a string
    '    RTrim (String)                     Trims the spaces from the right of a string
    '    Date()                             Returns the current date
    '    Time()                             Returns the current time
    '    DateAdd(Interval, Number, Date)    Adds an interval to a date
    '    DateDiff(Interval, Date1, Date2)   Returns the difference between two dates
    '    DatePart(Interval, Date)           Returns part of a date (e.g. day, month, year)
    '    FormatDateTime(Date,Format)        Formats a date to a given format
    '    WeekdayName (Day)                  Returns the name of a weekday number
    '    MonthName (Month)                  Returns the name of a month number
    '    IsDate(String)                     Returns 1 if string is a valid date, otherwise 0
    '    FormatCurrency (Value)             Formats a number into a currency value
    '    Replace(Expression, Find, Replace) Replaces part of a string
    '
    ' NOTE 2        : If more functions are to be added, add it in the select case
    '                   statement and also make sure that, IsBuiltInFunction is also
    '                   updated
    ' Edit History  :
    ' RAM20030416   : Created
    ' PW230603 - Added two new functions (CR103)
    '    CNum(String)                       Converts a string to a number
    '    CStr(Number)                       Converts a number to a string
    ' PW240604 - CQ5678 - add byref parameter, to indicate if function
    '                     result is a numeric value
    ' ***************************************************************** '
    Public Function CallBuiltinFunction(ByVal v_sFunctionName As String, ByVal v_vParamArray() As Object, ByRef r_bResultIsNumeric As Boolean) As String

        Dim result As String = String.Empty
        Dim strTemp As String = ""
        Dim vReturnValue As Object = Nothing
        Dim iNoOfArgumentNeeded As Integer
        Dim oval As Object
        Try

            result = ""
            r_bResultIsNumeric = False

            If Not IsBuiltInFunction(v_sFunctionName) Then
                Return ""
            End If

            strTemp = v_sFunctionName.Trim().ToUpper()

            Select Case strTemp
                Case "CONCAT"
                    '   Concat(String1, String2)            Concatenates two strings
                    iNoOfArgumentNeeded = 2
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        vReturnValue = ToSafeString(v_vParamArray(0)) & ToSafeString(v_vParamArray(1))
                    End If
                Case "MOD"
                    '    Mod(Number1, Number2)              Returns the Modulus of two numbers
                    iNoOfArgumentNeeded = 2
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        vReturnValue = ToSafeDouble(v_vParamArray(0)) Mod ToSafeDouble(v_vParamArray(1))
                    End If
                Case "CHR"
                    '    ChrW(CharCode)                      Returned a character given the ASCII code
                    iNoOfArgumentNeeded = 1
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        vReturnValue = Strings.ChrW(ToSafeInteger(v_vParamArray(0))).ToString()
                    End If
                Case "INSTR"
                    '    InStr(String1, String2)            Returns the position of String2 within String1
                    iNoOfArgumentNeeded = 2
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then

                        v_vParamArray(0) = RemoveDoubleQuotes(ToSafeString(v_vParamArray(0)))
                        v_vParamArray(1) = RemoveDoubleQuotes(ToSafeString(v_vParamArray(1)))

                        vReturnValue = ToSafeString(v_vParamArray(0)).IndexOf(ToSafeString(v_vParamArray(1))) + 1

                        r_bResultIsNumeric = True
                    End If
                Case "LCASE"
                    '    LCase(String)                      Converts a string to lowercase
                    iNoOfArgumentNeeded = 1
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        vReturnValue = ToSafeString(v_vParamArray(0)).ToLower()
                    End If
                Case "UCASE"
                    '    UCase(String)                      Converts a string to Uppercase
                    iNoOfArgumentNeeded = 1
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        v_vParamArray(0) = v_vParamArray(0).ToString.Replace(ChrW(34), "")
                        vReturnValue = ToSafeString(v_vParamArray(0)).ToUpper()
                    End If
                Case "LEN"
                    '    Len(String)                        Returns the length of a string
                    iNoOfArgumentNeeded = 1
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        v_vParamArray(0) = RemoveDoubleQuotes(v_vParamArray(0))
                        vReturnValue = Len(ToSafeString(v_vParamArray(0)))
                        r_bResultIsNumeric = True
                    End If
                Case "MID"
                    '    Mid(String, Start, Length)         Returns a portion of a string
                    iNoOfArgumentNeeded = 3
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        v_vParamArray(0) = v_vParamArray(0).ToString.Replace(ChrW(34), "")
                        v_vParamArray(1) = v_vParamArray(1).ToString.Replace(ChrW(34), "")
                        v_vParamArray(2) = v_vParamArray(2).ToString.Replace(ChrW(34), "")
                        If ToSafeInteger(v_vParamArray(1)) > 0 Then
                            vReturnValue = Mid(ToSafeString(v_vParamArray(0)), ToSafeInteger(v_vParamArray(1)), ToSafeInteger(v_vParamArray(2)))
                        End If
                    End If
                Case "STRCOMP"
                    '    StrComp(String1, String2)          Returns 1 is String1 = String2, otherwise 0
                    iNoOfArgumentNeeded = 2
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        If v_vParamArray(0).Equals(v_vParamArray(1)) Then
                            vReturnValue = 1
                        Else
                            vReturnValue = 0
                        End If
                    End If
                Case "STRING"
                    '    String(Number, Character)          Builds a string of a number of characters
                    iNoOfArgumentNeeded = 2
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        vReturnValue = New String(ChrW(v_vParamArray(1)), ToSafeInteger(v_vParamArray(0)))
                    End If
                Case "LEFT"
                    '    informations.Left(String, Length)               Returns a left hand section of a string
                    iNoOfArgumentNeeded = 2
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then

                        v_vParamArray(0) = RemoveDoubleQuotes(v_vParamArray(0))
                        v_vParamArray(1) = RemoveDoubleQuotes(v_vParamArray(1))

                        If ToSafeString(v_vParamArray(0)).Trim = "" OrElse ToSafeInteger(v_vParamArray(1)) < 0 Then
                            vReturnValue = ""
                        Else
                            vReturnValue = ToSafeString(v_vParamArray(0)).Substring(0, ToSafeInteger(v_vParamArray(1)))
                        End If

                    End If
                Case "RIGHT"
                    '    Right(String, Length)              Returns a right hand section of a string
                    iNoOfArgumentNeeded = 2
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then

                        If ToSafeString(v_vParamArray(0)).Trim = "" OrElse ToSafeInteger(v_vParamArray(1)) < 0 Then
                            vReturnValue = ""
                        Else
                            vReturnValue = ToSafeString(v_vParamArray(0)).Substring(ToSafeString(v_vParamArray(0)).Length - ToSafeInteger(v_vParamArray(1)))
                        End If

                    End If
                Case "LTRIM"
                    '    LTrim (String)                     Trims the spaces from the left of a string
                    iNoOfArgumentNeeded = 1
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        vReturnValue = ToSafeString(v_vParamArray(0)).TrimStart()
                    End If
                Case "RTRIM"
                    '    RTrim (String)                     Trims the spaces from the right of a string
                    iNoOfArgumentNeeded = 1
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        vReturnValue = ToSafeString(v_vParamArray(0)).TrimEnd()
                    End If
                Case "DATE"
                    '    Date()                             Returns the current date
                    iNoOfArgumentNeeded = 0

                    vReturnValue = DateTime.Today
                Case "TIME"
                    '    Time()                             Returns the current time
                    iNoOfArgumentNeeded = 0

                    vReturnValue = DateTimeHelper.Time
                Case "DATEADD"
                    '    DateAdd(Interval, Number, Date)    Adds an interval to a date
                    iNoOfArgumentNeeded = 3
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then

                        v_vParamArray(0) = v_vParamArray(0).ToString.Replace(ChrW(34), "")
                        v_vParamArray(1) = v_vParamArray(1).ToString.Replace(ChrW(34), "")
                        v_vParamArray(2) = v_vParamArray(2).ToString.Replace(ChrW(34), "")
                        vReturnValue = Informations.DateAdd(ToSafeString(v_vParamArray(0)), v_vParamArray(1), ToSafeDate(v_vParamArray(2)))
                    End If
                Case "DATEDIFF"
                    '    DateDiff(Interval, Date1, Date2)   Returns the difference between two dates
                    iNoOfArgumentNeeded = 3
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        v_vParamArray(0) = v_vParamArray(0).ToString.Replace(ChrW(34), "")
                        v_vParamArray(1) = v_vParamArray(1).ToString.Replace(ChrW(34), "")
                        v_vParamArray(2) = v_vParamArray(2).ToString.Replace(ChrW(34), "")
                        vReturnValue = Informations.DateDiff(v_vParamArray(0), v_vParamArray(1), v_vParamArray(2))
                    End If
                Case "DATEPART"
                    '    DatePart(Interval, Date)           Returns part of a date (e.g. day, month, year)
                    iNoOfArgumentNeeded = 2
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        oval = DirectCast([Enum].Parse(GetType(DateInterval), v_vParamArray(0)), DateInterval)
                        vReturnValue = Informations.DatePart(oval, ToSafeDate(v_vParamArray(1)), DayOfWeek.Sunday, FirstWeekOfYear.FirstJan1)
                    End If
                Case "FORMATDATETIME"
                    '    FormatDateTime(Date,Format)        Formats a date to a given format
                    iNoOfArgumentNeeded = 2
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        v_vParamArray(0) = v_vParamArray(0).ToString.Replace(ChrW(34), "")
                        ''Saj240224
                        vReturnValue = FormatDateTime(ToSafeDate(v_vParamArray(0)), val(ToSafeString(v_vParamArray(1))))
                    End If
                Case "WEEKDAYNAME"
                    '    WeekdayName (Day)                  Returns the name of a weekday number
                    iNoOfArgumentNeeded = 1
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        vReturnValue = DateTimeFormatInfo.CurrentInfo.GetDayName(ToSafeInteger(v_vParamArray(0)) - 1)
                    End If
                Case "MONTHNAME"
                    '    MonthName (Month)                  Returns the name of a month number
                    iNoOfArgumentNeeded = 1
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        vReturnValue = DateTimeFormatInfo.CurrentInfo.GetMonthName(ToSafeInteger(v_vParamArray(0)))
                    End If
                Case "ISDATE"
                    '    IsDate(String)                     Returns 1 if string is a valid date, otherwise 0
                    iNoOfArgumentNeeded = 1
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        If Informations.IsDate(v_vParamArray(0)) Then
                            vReturnValue = 1
                        Else
                            vReturnValue = 0
                        End If
                    End If
                Case "FORMATCURRENCY"
                    '    FormatCurrency (Value)             Formats a number into a currency value
                    iNoOfArgumentNeeded = 1
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        vReturnValue = ToSafeDouble(v_vParamArray(0)).ToString("C")
                    End If
                Case "REPLACE"
                    '    Replace(Expression, Find, Replace) Replaces part of a string
                    iNoOfArgumentNeeded = 3
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        v_vParamArray(0) = RemoveDoubleQuotes(v_vParamArray(0))
                        v_vParamArray(2) = RemoveDoubleQuotes(v_vParamArray(2))
                        vReturnValue = ToSafeString(v_vParamArray(0)).Replace(ToSafeString(v_vParamArray(1)), ToSafeString(v_vParamArray(2)))
                    End If
                    ' PW230603 - CR103: start
                Case "CSTR"
                    iNoOfArgumentNeeded = 1
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        v_vParamArray(0) = v_vParamArray(0).ToString.Replace(ChrW(34), "")
                        vReturnValue = ToSafeString(v_vParamArray(0))
                    End If
                Case "CNUM"
                    iNoOfArgumentNeeded = 1
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        vReturnValue = ToSafeDouble(v_vParamArray(0))
                    End If
                    r_bResultIsNumeric = True
                Case "CDATE"
                    iNoOfArgumentNeeded = 1
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        vReturnValue = ToSafeDate(v_vParamArray(0))
                    End If
                Case "MONTH"
                    iNoOfArgumentNeeded = 1
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        v_vParamArray(0) = v_vParamArray(0).ToString.Replace(ChrW(34), "")
                        vReturnValue = CDate(v_vParamArray(0)).Month
                    End If

                    ' PW230603 - CR103: end
                    'Being paralled from MItsui Against RACTI SSP-457
                Case "FORMATNUMBER"
                    iNoOfArgumentNeeded = 5
                    If IsValidArguments(v_vParamArray, iNoOfArgumentNeeded) Then
                        'Saj240224
                        'vReturnValue = FormatNumber(ToSafeDouble(Trim(v_vParamArray(0)).Replace(ChrW(34), "")),
                        '                          ToSafeInteger(v_vParamArray(1)),
                        '                          ToSafeInteger(v_vParamArray(2)),
                        '                          ToSafeInteger(v_vParamArray(3)),
                        '                          ToSafeInteger(v_vParamArray(4)))
                    Else
                        If CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator = "," Then
                            Dim sParam As String = String.Empty
                            Dim nLength As Integer = v_vParamArray.Length
                            For i As Integer = 0 To nLength - 5
                                sParam = sParam + v_vParamArray(i).ToString()
                            Next
                            v_vParamArray(0) = sParam
                            vReturnValue = FormatNumber(ToSafeDouble(Trim(v_vParamArray(0)).Replace(ChrW(34), "")),
                                                      ToSafeInteger(v_vParamArray(nLength - 4)),
                                                      ToSafeInteger(v_vParamArray(nLength - 3)),
                                                      ToSafeInteger(v_vParamArray(nLength - 2)),
                                                      ToSafeInteger(v_vParamArray(nLength - 1)))
                        End If
                    End If
                    r_bResultIsNumeric = True

            End Select

            ' Return the result


            Return ToSafeString(vReturnValue)

        Catch excep As System.Exception
            result = ""
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CallBuiltInFunction Failed for : " & v_sFunctionName, vApp:=ACApp, vClass:=ACClass, vMethod:="CallBuiltInFunction", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Private Function RemoveDoubleQuotes(ByVal v_sValue As String) As String


        If ToSafeString(v_sValue).Substring(0, 1) = ChrW(34) Then
            v_sValue = Mid(v_sValue, 2)
        End If

        If ToSafeString(v_sValue).Substring(ToSafeString(v_sValue).Length - 1, 1) = ChrW(34) Then
            v_sValue = Mid(ToSafeString(v_sValue), 1, ToSafeString(v_sValue).Length - 1)
        End If


        Return v_sValue
    End Function

    ' ***************************************************************** '
    ' Name          : IsBuiltInFunction
    ' Description   : Check if a string name does stand for a supported built-in function
    ' Reference     : Document Issuance Changes.
    '                 Tech Spec 4.4 Scripting 4.4.1.2 Functions
    ' Note          : The following functions are supported.
    ' Currently We are supporting the following functions
    '        Concat(String1, String2)
    '        Mod(Number1, Number2)
    '        Chr (CharCode)
    '        InStr(String1, String2)
    '        LCase(String)
    '        UCase(String)
    '        Len(String)
    '        Mid(String, Start, Length)
    '        StrComp(String1, String2)
    '        String(Number, Character)
    '        informations.Left(String, Length)
    '        Right(String, Length)
    '        LTrim (String)
    '        RTrim (String)
    '        Date()
    '        Time()
    '        DateAdd(Interval, Number, Date)
    '        DateDiff(Interval, Date1, Date2)
    '        DatePart(Interval, Date)
    '        FormatDateTime(Date,Format)
    '        WeekdayName (Day)
    '        MonthName (Month)
    '        IsDate(String)
    '        FormatCurrency (value)
    '        Replace(Expression, Find, Replace)
    ' Edit History  :
    ' RAM20030416   : Created
    ' PW230603 - Added two new functions (CR103)
    '    CNum(String)                       Converts a string to a number
    '    CStr(Number)                       Converts a number to a string
    ' ***************************************************************** '
    Public Function IsBuiltInFunction(ByVal v_sFunctionName As String) As Boolean


        Dim TempName As String = v_sFunctionName.Trim().ToUpper()

        If TempName = "CONCAT" Or TempName = "MOD" Or TempName = "CHR" Or TempName = "INSTR" Or TempName = "LCASE" Or TempName = "UCASE" Or TempName = "LEN" Or TempName = "MID" Or TempName = "STRCOMP" Or TempName = "STRING" Or TempName = "LEFT" Or TempName = "RIGHT" Or TempName = "LTRIM" Or TempName = "RTRIM" Or TempName = "DATE" Or TempName = "TIME" Or TempName = "DATEADD" Or TempName = "DATEDIFF" Or TempName = "DATEPART" Or TempName = "FORMATDATETIME" Or TempName = "WEEKDAYNAME" Or TempName = "MONTHNAME" Or TempName = "ISDATE" Or TempName = "FORMATCURRENCY" Or TempName = "CNUM" Or TempName = "CSTR" Or TempName = "REPLACE" Or TempName = "CDATE" Or TempName = "MONTH" Or TempName = "FORMATNUMBER" Then

            Return True
        Else
            Return False
        End If

    End Function


    ' ***************************************************************** '
    ' Name          : IsValidArguments
    ' Description   : Check if arguments array matches with the no of
    '                   arguments needed
    ' Reference     : Document Issuance Changes.
    '                 Tech Spec 4.4 Scripting 4.4.1.2 Functions
    ' Note          : The following functions are supported.
    ' Edit History  :
    ' RAM20030417   : Created
    ' ***************************************************************** '
    Public Function IsValidArguments(ByVal v_vInputArray() As Object, ByVal v_iNoOfParams As Integer) As Boolean

        Dim result As Boolean = False

        If Informations.IsArray(v_vInputArray) Then
            If v_vInputArray.GetUpperBound(0) = v_iNoOfParams - 1 Then ' Base Zero
                result = True
            End If
        End If

        Return result
    End Function


    ' ***************************************************************** '
    ' Name          : ProperSplit
    ' Description   : This function is for use when parsing(splitting) a
    '                   data string that has a comma delimiter.
    ' Reference     : Document Issuance Changes.
    '                 Tech Spec 4.4 Scripting 4.4.1.2 Functions
    ' Note          : This is a replacement for the VB6 Split Function
    ' Edit History  :
    ' RAM20030422   : Created
    ' PW060404 - CQ5295 - do not delimit with quotation marks.
    '                 There could be a quotation mark in
    '                 the field value, so we can't delimit with it. Use the
    '                 word quote marks instead (ChrW(147) and (148) like the
    '                 rest of doc production does.
    ' ***************************************************************** '
    Public Function ProperSplit(ByVal v_sInputString As String, Optional ByVal v_sDelimter As String = ",") As Object

        'This function is for use when parsing(splitting) a data string that
        'has a comma delimiter. The normal VB Split function does not take into
        'consideration of a comma embedded within a Fields' data string and
        'will parse the information incorrectly.
        '
        '
        'This function takes into consideration the a data field may contain
        'a comma and parses the data as entire string. The data string being defined
        'as the data between the two Double-Quote marks. This function also
        'prunes the leading and trailing double quote marks
        '
        ' Notes : Does NOT Correct improperly formatted Numeric amounts that
        ': contain a comma for the thousands placement, unless the number has
        ': leading and trailing Double-Quote marks.
        '
        ' Call : X() = ProperSplit(datastring to split.)
        '
        ' Returns: Single-Dimension array, same result that you get from the SPLIT Function.

        Dim result As Object = Nothing
        Dim iStringLength, iDelimPosition As Integer
        Dim sDoubleQuoteMark As String = ""
        Dim iIndex As Integer
        Dim aData1() As String = Nothing
        Dim sDatafield As String = ""
        Dim iDQPos1, iDQPos2 As Integer

        Try

            '
            '    v_sDelimter = ","
            iStringLength = v_sInputString.Length
            iIndex = -1 ' To Make the return array it Zero Base
            '
            ' if the length of the data string is greater than zero

            If iStringLength > 0 Then
                ' search for a v_sDelimteriter in the datastring
                iDelimPosition = (v_sInputString.IndexOf(v_sDelimter) + 1)
                '
                Do While iDelimPosition <> 0
                    ' do while there is a v_sDelimteriter
                    ' search for a quote-enclosure set.
                    iDQPos1 = (v_sInputString.IndexOf(Strings.ChrW(34).ToString()) + 1)
                    sDatafield = ""
                    '
                    If iDQPos1 <> 0 And iDQPos1 < iDelimPosition Then
                        ' found Double quote mark, and it is found BEFORE
                        ' the v_sDelimteriter. Search for matching Double Quote Mark
                        iDQPos2 = v_sInputString.IndexOf(Strings.ChrW(34).ToString(), iDQPos1 + 1) + 1

                        If iDQPos2 <> 0 Then

                            If iDQPos2 = v_sInputString.Length Then
                                ' this is the last field of data so we remove the
                                ' surrounding Double-Quote Marks.
                                v_sInputString = v_sInputString.Substring(v_sInputString.Length - (v_sInputString.Length - 1))
                                v_sInputString = v_sInputString.Substring(0, v_sInputString.Length - 1)
                                'exit the Do loop and
                                Exit Do
                            End If
                            ' Just found the Matching double Quote Mark
                            ' data field ends at iDQPos2, not iDelim Position
                            sDatafield = v_sInputString.Substring(0, iDQPos2)
                            v_sInputString = v_sInputString.Substring(v_sInputString.Length - (v_sInputString.Length - (sDatafield.Length + 1)))
                            sDatafield = sDatafield.Substring(sDatafield.Length - (sDatafield.Length - 1))
                            sDatafield = sDatafield.Substring(0, sDatafield.Length - 1)
                            iIndex += 1
                        Else
                            ' unmatched double quote usually specifies error with the
                            ' data being read in.
                        End If
                    Else

                        If iDQPos1 <> 0 Then
                            ' Quote mark is FOUND AFTER the v_sDelimteriter meaning the
                            ' data to the v_sDelimteriter is ok to use as a full field.
                            ' Data ends at the v_sDelimteriter.
                            sDatafield = v_sInputString.Substring(0, iDelimPosition - 1)
                            v_sInputString = v_sInputString.Substring(v_sInputString.Length - (v_sInputString.Length - (sDatafield.Length + 1)))
                            iIndex += 1
                        Else
                            ' there is NO double Quote Mark Found.
                            sDatafield = v_sInputString.Substring(0, iDelimPosition - 1)
                            v_sInputString = v_sInputString.Substring(v_sInputString.Length - (v_sInputString.Length - iDelimPosition))
                            iIndex += 1
                        End If
                    End If
                    ReDim Preserve aData1(iIndex)

                    ' Remove any Starting double quote and ending doube quote
                    If sDatafield.Substring(0, 1) = Strings.ChrW(147).ToString() Then
                        sDatafield = v_sInputString.Substring(1)
                    End If

                    If sDatafield.Substring(sDatafield.Length - 1) = Strings.ChrW(148).ToString() Then
                        sDatafield = sDatafield.Substring(0, sDatafield.Length - 1)
                    End If

                    aData1(iIndex) = sDatafield
                    iDelimPosition = (v_sInputString.IndexOf(v_sDelimter) + 1)
                Loop
                iIndex += 1
                ReDim Preserve aData1(iIndex)

                ' Remove any Starting double quote and ending doube quote
                If v_sInputString.Trim <> "" Then
                    If v_sInputString.Substring(0, 1) = Strings.ChrW(34).ToString() Then
                        v_sInputString = v_sInputString.Substring(1)
                    End If

                    If v_sInputString.Substring(v_sInputString.Length - 1) = Strings.ChrW(34).ToString() Then
                        v_sInputString = v_sInputString.Substring(0, v_sInputString.Length - 1)
                    End If
                End If


                aData1(iIndex) = v_sInputString
            Else
            End If

            Return aData1

        Catch excep As System.Exception




            result = ""

            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProperSplit Failed for : " & v_sInputString, vApp:=ACApp, vClass:=ACClass, vMethod:="ProperSplit", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name          : NoOfOccurences
    ' Description   : This function is for use to Count # of occurrences of
    '                   a search string with in a string.
    ' Reference     : Document Issuance Changes.
    '                 Tech Spec 4.4 Scripting 4.4.1.2 Functions
    ' Params        :(IN) v_sSource         to search,
    '                (IN) v_sStringToMatch  to look for.
    ' Returns       : # of occurrences.
    ' Edit History  :
    ' RAM20030423   : Created
    ' ***************************************************************** '
    Function NoOfOccurences(ByVal v_sSource As String, ByVal v_sStringToMatch As String) As Integer

        Dim result As Integer = 0
        Dim nPos, nCount, iOffset As Integer

        Try

            ' Check if we have valid strings
            If v_sSource.Length = 0 Or v_sStringToMatch.Length = 0 Then
                Return 0
            End If

            nCount = 0
            iOffset = v_sStringToMatch.Length
            nPos = (v_sSource.IndexOf(v_sStringToMatch) + 1)

            While nPos
                nCount += 1
                nPos = v_sSource.IndexOf(v_sStringToMatch, nPos + iOffset) + 1

            End While

            ' Return the no of occurences

            Return nCount

        Catch excep As System.Exception




            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NoOfOccurences Failed for : " & v_sSource & " : " & v_sStringToMatch, vApp:=ACApp, vClass:=ACClass, vMethod:="NoOfOccurences", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name          : NthInstanceOf
    ' Description   : Function to Return the Position Of the Nth Instance of the String
    '                  [sSearched] Within [sString].
    '                 If Instance = 0 then returns the Index of the Last Occurrence of the
    '                 String. Negative Numbers are calculated from the Last Match
    ' Reference     : Document Issuance Changes.
    '                 Tech Spec 4.4 Scripting 4.4.1.2 Functions
    ' Edit History  :
    ' RAM20030423   : Created
    ' ***************************************************************** '
    Public Function NthInstanceOf(ByVal sString As String, ByVal sSearched As String, Optional ByVal Instance As Integer = 1, Optional ByVal nStartFrom As Integer = 1, Optional ByVal eCompareMethod As String = "", Optional ByVal bFromLastMatch As Boolean = False) As Integer
        Dim result As Integer = 0
        Dim nOccurrences, nInitStartFrom As Integer

        Try

            If sString = "" Then Return result
            If sSearched = "" Then Return result

            If bFromLastMatch Then
                Instance = -1 * Instance
            End If


            Select Case System.Math.Sign(Instance)
                Case 0
                    result = If(sString = "" And sSearched = "", 0, (sString.LastIndexOf(sSearched) + 1))
                Case 1
                    nStartFrom = sString.IndexOf(sSearched, nStartFrom) + 1

                    Do While nStartFrom > 0
                        nOccurrences += 1

                        If nOccurrences = Instance Then
                            Return nStartFrom
                        End If

                        nStartFrom = sString.IndexOf(sSearched, nStartFrom + 1) + 1
                    Loop

                Case Else

                    nInitStartFrom = nStartFrom

                    nStartFrom = If(sString = "" And sSearched = "", 0, (sString.LastIndexOf(sSearched) + 1))

                    Do While nStartFrom >= nInitStartFrom
                        nOccurrences -= 1

                        If nOccurrences = Instance Then
                            Return nStartFrom
                        End If

                        nStartFrom = If(sString = "" And sSearched = "", 0, (sString.LastIndexOf(sSearched) + 1))
                    Loop

            End Select

            Return result

        Catch excep As System.Exception



            result = 0

            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NthInstanceOf Failed for : " & sString & " : " & sSearched, vApp:=ACApp, vClass:=ACClass, vMethod:="NthInstanceOf", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function

    'Saj240224
    Public Enum DateFormat
        GeneralDate
        LongDate
        ShortDate
        LongTime
        ShortTime
    End Enum

    Private Function FormatDateTime(Expression As DateTime, Optional NamedFormat As DateFormat = DateFormat.GeneralDate) As String
        Try
            Return Expression.ToString(If(NamedFormat = DateFormat.LongDate, "D",
                                           If(NamedFormat = DateFormat.ShortDate, "d",
                                              If(NamedFormat = DateFormat.LongTime, "T",
                                                 If(NamedFormat = DateFormat.ShortTime, "HH:mm",
                                                    If((Expression.TimeOfDay.Ticks <> Expression.Ticks),
                                                       If((Expression.TimeOfDay.Ticks <> 0L), "G", "d"), "T"))))), Nothing)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Module


