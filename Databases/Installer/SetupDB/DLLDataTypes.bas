Attribute VB_Name = "MDLLDataTypes"
' Module:   Safe generic data type conversion functions
' Shared:   Yes (RESTRICTED)
' Needs:    Nothing
'
' THIS CODE IMPLEMENTS CORRESPONDING FUNCTIONS IN THE DLL.
' IT IS SHARED *ONLY* TO SUPPORT SMALL UTILITIES THAT
' CANNOT REFERENCE THE DLL. *DO NOT* ALTER THIS CODE IN ANY
' WAY UNLESS YOU ARE CHANGING THE INTERNALS OF THE DLL.
'
Option Explicit

''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Public Enumerations
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

' Cannot use a real enumeration otherwise the DLL would not compile!
Public Const knXFUnspecified = 0
Public Const knXFBit = 1
Public Const knXFTrueFalseL = 2
Public Const knXFTrueFalseM = 3
Public Const knXFYesNoL = 4
Public Const knXFYesNoM = 5
Public Const knXFDateTime = 6
Public Const knXFDate = 7
Public Const knXFTime = 8
Public Const knXFYearMonth = 9
Public Const knXFMonthDay = 10
Public Const knXFYear = 11
Public Const knXFMonth = 12
Public Const knXFDay = 13

''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Private Declarations
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

' These are the true VarType() constants supported by COM, of which VB only defines about half.
' Valueless types
Private Const VT_EMPTY = 0      ' vbEmpty
Private Const VT_NULL = 1       ' vbNull
' Single values
Private Const VT_BOOL = 11      ' vbBoolean
Private Const VT_I1 = 16
Private Const VT_UI1 = 17       ' vbByte
Private Const VT_I2 = 2         ' vbInteger
Private Const VT_UI2 = 18
Private Const VT_I4 = 3         ' vbLong
Private Const VT_UI4 = 19
Private Const VT_INT = 22
Private Const VT_UINT = 23
Private Const VT_I8 = 20
Private Const VT_UI8 = 21
Private Const VT_R4 = 4         ' vbSingle
Private Const VT_R8 = 5         ' vbDouble
Private Const VT_CY = 6         ' vbCurrency
Private Const VT_DECIMAL = 14   ' vbDecimal
Private Const VT_DATE = 7       ' vbDate
Private Const VT_BSTR = 8       ' vbString
' Not interpretable
Private Const VT_DISPATCH = 9   ' vbObject
Private Const VT_ERROR = 10     ' vbError
Private Const VT_VARIANT = 12   ' vbVariant
Private Const VT_UNKNOWN = 13   ' vbDataObject
Private Const VT_VOID = 24
Private Const VT_HRESULT = 25
Private Const VT_PTR = 26
Private Const VT_SAFEARRAY = 27
Private Const VT_CARRAY = 28
Private Const VT_USERDEFINED = 29
Private Const VT_LPSTR = 30
Private Const VT_LPWSTR = 31
Private Const VT_RECORD = 36    ' vbUserDefinedType
Private Const VT_INT_PTR = 37
Private Const VT_UINT_PTR = 38
Private Const VT_FILETIME = 64
Private Const VT_BLOB = 65
Private Const VT_STREAM = 66
Private Const VT_STORAGE = 67
Private Const VT_STREAMED_OBJECT = 68
Private Const VT_STORED_OBJECT = 69
Private Const VT_BLOB_OBJECT = 70
Private Const VT_CF = 71
Private Const VT_CLSID = 72
Private Const VT_VERSIONED_STREAM = 73
' Structured data flags
Private Const VT_TYPEMASK = &HFFF
Private Const VT_VECTOR = &H1000
Private Const VT_ARRAY = &H2000 ' vbArray
Private Const VT_BYREF = &H4000

' In all procedures below, we first make a copy of the variant
' passed in. If an object was passed in, the code reads its
' default property many times. Making a copy of the variable
' reads the property only once and stores the result as a simple
' data type. This is particularly useful for reading Adaptive
' Server memo fields in rdoColumn objects, because they return
' Null the second time regardless of the actual value.

' Errors raised.
Private Const ksErrSource = "gSWLibrary.GDataTypes"
Private Const knErrSortConversion = 13
Private Const ksErrSortConversion = "Type Mismatch (cannot convert to sortcode)."
Private Const knErrCrystalConversion = 13
Private Const ksErrCrystalConversion = "Type Mismatch (cannot convert to Crystal)."
Private Const knErrSQLConversion = 13
Private Const ksErrSQLConversion = "Type Mismatch (cannot convert to SQL)."
Private Const knErrXPathConversion = 13
Private Const ksErrXPathConversion = "Type Mismatch (cannot convert to XPath)."

Public Function ToBoolean(ByVal vValue As Variant, _
    Optional ByVal vDefault As Variant = False) As Variant

    On Error GoTo EH_Conversion

    ' Copy input value (see module-level comment).
    Dim vCopy As Variant
    vCopy = vValue

    Select Case VarType(vCopy)
    Case vbEmpty, vbNull
        GoTo EH_Conversion
    Case vbString
        If vCopy = "" Then GoTo EH_Conversion
    End Select

    ToBoolean = CBool(vCopy)
    Exit Function

EH_Conversion:
    ToBoolean = vDefault

End Function

Public Function ToByte(ByVal vValue As Variant, _
    Optional ByVal vDefault As Variant = 0) As Variant

    On Error GoTo EH_Conversion

    ' Copy input value (see module-level comment).
    Dim vCopy As Variant
    vCopy = vValue

    Select Case VarType(vCopy)
    Case vbEmpty, vbNull
        GoTo EH_Conversion
    Case vbString
        If vCopy = "" Then GoTo EH_Conversion
    End Select

    ToByte = CByte(vCopy)
    Exit Function

EH_Conversion:
    ToByte = vDefault

End Function

Public Function ToInteger(ByVal vValue As Variant, _
    Optional ByVal vDefault As Variant = 0) As Variant

    On Error GoTo EH_Conversion

    ' Copy input value (see module-level comment).
    Dim vCopy As Variant
    vCopy = vValue

    Select Case VarType(vCopy)
    Case vbEmpty, vbNull
        GoTo EH_Conversion
    Case vbString
        If vCopy = "" Then GoTo EH_Conversion
    End Select

    ToInteger = CInt(vCopy)
    Exit Function

EH_Conversion:
    ToInteger = vDefault

End Function

Public Function ToLong(ByVal vValue As Variant, _
    Optional ByVal vDefault As Variant = 0&) As Variant

    On Error GoTo EH_Conversion

    ' Copy input value (see module-level comment).
    Dim vCopy As Variant
    vCopy = vValue

    Select Case VarType(vCopy)
    Case vbEmpty, vbNull
        GoTo EH_Conversion
    Case vbString
        If vCopy = "" Then GoTo EH_Conversion
    End Select

    ToLong = CLng(vCopy)
    Exit Function

EH_Conversion:
    ToLong = vDefault

End Function

Public Function ToDouble(ByVal vValue As Variant, _
    Optional ByVal vDefault As Variant = 0#) As Variant

    On Error GoTo EH_Conversion

    ' Copy input value (see module-level comment).
    Dim vCopy As Variant
    vCopy = vValue

    Select Case VarType(vCopy)
    Case vbEmpty, vbNull
        GoTo EH_Conversion
    Case vbString
        If vCopy = "" Then GoTo EH_Conversion
    End Select

    ToDouble = CDbl(vCopy)
    Exit Function

EH_Conversion:
    ToDouble = vDefault

End Function

Public Function ToCurrency(ByVal vValue As Variant, _
    Optional ByVal vDefault As Variant = 0@) As Variant

    On Error GoTo EH_Conversion

    ' Copy input value (see module-level comment).
    Dim vCopy As Variant
    vCopy = vValue

    Select Case VarType(vCopy)
    Case vbEmpty, vbNull
        GoTo EH_Conversion
    Case vbString
        If vCopy = "" Then GoTo EH_Conversion
    End Select

    ToCurrency = CCur(vCopy)
    Exit Function

EH_Conversion:
    ToCurrency = vDefault

End Function

Public Function ToDate(ByVal vValue As Variant, _
    Optional ByVal vDefault As Variant) As Variant

    If IsMissing(vDefault) Then vDefault = Null

    On Error GoTo EH_Conversion

    ' Copy input value (see module-level comment).
    Dim vCopy As Variant
    vCopy = vValue

    Select Case VarType(vCopy)
    Case vbEmpty, vbNull
        GoTo EH_Conversion
    Case vbString
        If vCopy = "" Then GoTo EH_Conversion
    End Select

    ToDate = DateValue(CDate(vCopy))
    Exit Function

EH_Conversion:
    ToDate = vDefault

End Function

Public Function ToTime(ByVal vValue As Variant, _
    Optional ByVal vDefault As Variant) As Variant

    If IsMissing(vDefault) Then vDefault = Null

    On Error GoTo EH_Conversion

    ' Copy input value (see module-level comment).
    Dim vCopy As Variant
    vCopy = vValue

    Select Case VarType(vCopy)
    Case vbEmpty, vbNull
        GoTo EH_Conversion
    Case vbString
        If vCopy = "" Then GoTo EH_Conversion
    End Select

    ToTime = TimeValue(CDate(vCopy))
    Exit Function

EH_Conversion:
    ToTime = vDefault

End Function

Public Function ToDateTime(ByVal vValue As Variant, _
    Optional ByVal vDefault As Variant) As Variant

    If IsMissing(vDefault) Then vDefault = Null

    On Error GoTo EH_Conversion

    ' Copy input value (see module-level comment).
    Dim vCopy As Variant
    vCopy = vValue

    Select Case VarType(vCopy)
    Case vbEmpty, vbNull
        GoTo EH_Conversion
    Case vbString
        If vCopy = "" Then GoTo EH_Conversion
    End Select

    ToDateTime = CDate(vCopy)
    Exit Function

EH_Conversion:
    ToDateTime = vDefault

End Function

Public Function ToString(ByVal vValue As Variant, _
    Optional ByVal vDefault As Variant = "") As Variant

    On Error GoTo EH_Conversion

    ' Copy input value (see module-level comment).
    Dim vCopy As Variant
    vCopy = vValue

    Select Case VarType(vCopy)
    Case vbEmpty, vbNull
        GoTo EH_Conversion
    Case vbString
        If vCopy = "" Then GoTo EH_Conversion
    End Select

    ToString = CStr(vCopy)
    Exit Function

EH_Conversion:
    ToString = vDefault

End Function

Public Function ToBooleanFixed(ByVal vValue As Variant, _
    Optional ByVal vDefault As Variant = False) As Variant

    On Error GoTo EH_Conversion

    ' Copy input value (see module-level comment).
    Dim vCopy As Variant
    vCopy = vValue

    Select Case VarType(vCopy)
    Case vbEmpty, vbNull
        GoTo EH_Conversion
    Case vbString
        If vCopy = "" Then GoTo EH_Conversion
    End Select

    ToBooleanFixed = (Val(vCopy) <> 0)
    Exit Function

EH_Conversion:
    ToBooleanFixed = vDefault

End Function

Public Function ToByteFixed(ByVal vValue As Variant, _
    Optional ByVal vDefault As Variant = 0) As Variant

    On Error GoTo EH_Conversion

    ' Copy input value (see module-level comment).
    Dim vCopy As Variant
    vCopy = vValue

    Select Case VarType(vCopy)
    Case vbEmpty, vbNull
        GoTo EH_Conversion
    Case vbString
        If vCopy = "" Then GoTo EH_Conversion
    End Select

    ToByteFixed = CByte(Val(vCopy))
    Exit Function

EH_Conversion:
    ToByteFixed = vDefault

End Function

Public Function ToIntegerFixed(ByVal vValue As Variant, _
    Optional ByVal vDefault As Variant = 0) As Variant

    On Error GoTo EH_Conversion

    ' Copy input value (see module-level comment).
    Dim vCopy As Variant
    vCopy = vValue

    Select Case VarType(vCopy)
    Case vbEmpty, vbNull
        GoTo EH_Conversion
    Case vbString
        If vCopy = "" Then GoTo EH_Conversion
    End Select

    ToIntegerFixed = CInt(Val(vCopy))
    Exit Function

EH_Conversion:
    ToIntegerFixed = vDefault

End Function

Public Function ToLongFixed(ByVal vValue As Variant, _
    Optional ByVal vDefault As Variant = 0&) As Variant

    On Error GoTo EH_Conversion

    ' Copy input value (see module-level comment).
    Dim vCopy As Variant
    vCopy = vValue

    Select Case VarType(vCopy)
    Case vbEmpty, vbNull
        GoTo EH_Conversion
    Case vbString
        If vCopy = "" Then GoTo EH_Conversion
    End Select

    ToLongFixed = CLng(Val(vCopy))
    Exit Function

EH_Conversion:
    ToLongFixed = vDefault

End Function

Public Function ToDoubleFixed(ByVal vValue As Variant, _
    Optional ByVal vDefault As Variant = 0#) As Variant

    On Error GoTo EH_Conversion

    ' Copy input value (see module-level comment).
    Dim vCopy As Variant
    vCopy = vValue

    Select Case VarType(vCopy)
    Case vbEmpty, vbNull
        GoTo EH_Conversion
    Case vbString
        If vCopy = "" Then GoTo EH_Conversion
    End Select

    ToDoubleFixed = CDbl(Val(vCopy))
    Exit Function

EH_Conversion:
    ToDoubleFixed = vDefault

End Function

Public Function ToCurrencyFixed(ByVal vValue As Variant, _
    Optional ByVal vDefault As Variant = 0@) As Variant

    On Error GoTo EH_Conversion

    ' Copy input value (see module-level comment).
    Dim vCopy As Variant
    vCopy = vValue

    Select Case VarType(vCopy)
    Case vbEmpty, vbNull
        GoTo EH_Conversion
    Case vbString
        If vCopy = "" Then GoTo EH_Conversion
    End Select

    ToCurrencyFixed = CCur(Val(vCopy))
    Exit Function

EH_Conversion:
    ToCurrencyFixed = vDefault

End Function

Public Function ToDateFixed(ByVal vValue As Variant, _
    Optional ByVal vDefault As Variant) As Variant

    If IsMissing(vDefault) Then vDefault = Null

    On Error GoTo EH_Conversion

    ' Copy input value (see module-level comment).
    Dim vCopy As Variant
    vCopy = vValue

    Select Case VarType(vCopy)
    Case vbEmpty, vbNull
        GoTo EH_Conversion
    Case vbString
        If vCopy = "" Then GoTo EH_Conversion
    Case Else
        ToDateFixed = DateValue(CDate(vCopy))
        Exit Function
    End Select

    Dim nYear As Integer
    Dim nMonth As Integer
    Dim nDay As Integer
    Dim nHour As Integer
    Dim nMinute As Integer
    Dim nSecond As Integer

    ParseDateTimeFixed vCopy, nYear, nMonth, nDay, nHour, nMinute, nSecond
    ToDateFixed = DateSerial(nYear, nMonth, nDay)
    Exit Function

EH_Conversion:
    ToDateFixed = vDefault

End Function

Public Function ToTimeFixed(ByVal vValue As Variant, _
    Optional ByVal vDefault As Variant) As Variant

    If IsMissing(vDefault) Then vDefault = Null

    On Error GoTo EH_Conversion

    ' Copy input value (see module-level comment).
    Dim vCopy As Variant
    vCopy = vValue

    Select Case VarType(vCopy)
    Case vbEmpty, vbNull
        GoTo EH_Conversion
    Case vbString
        If vCopy = "" Then GoTo EH_Conversion
    Case Else
        ToTimeFixed = TimeValue(CDate(vCopy))
        Exit Function
    End Select

    Dim nYear As Integer
    Dim nMonth As Integer
    Dim nDay As Integer
    Dim nHour As Integer
    Dim nMinute As Integer
    Dim nSecond As Integer

    ParseDateTimeFixed vCopy, nYear, nMonth, nDay, nHour, nMinute, nSecond
    ToTimeFixed = TimeSerial(nHour, nMinute, nSecond)
    Exit Function

EH_Conversion:
    ToTimeFixed = vDefault

End Function

Public Function ToDateTimeFixed(ByVal vValue As Variant, _
    Optional ByVal vDefault As Variant) As Variant

    If IsMissing(vDefault) Then vDefault = Null

    On Error GoTo EH_Conversion

    ' Copy input value (see module-level comment).
    Dim vCopy As Variant
    vCopy = vValue

    Select Case VarType(vCopy)
    Case vbEmpty, vbNull
        GoTo EH_Conversion
    Case vbString
        If vCopy = "" Then GoTo EH_Conversion
    Case Else
        ToDateTimeFixed = CDate(vCopy)
        Exit Function
    End Select

    Dim nYear As Integer
    Dim nMonth As Integer
    Dim nDay As Integer
    Dim nHour As Integer
    Dim nMinute As Integer
    Dim nSecond As Integer

    ParseDateTimeFixed vCopy, nYear, nMonth, nDay, nHour, nMinute, nSecond
    ToDateTimeFixed = DateSerial(nYear, nMonth, nDay) + TimeSerial(nHour, nMinute, nSecond)
    Exit Function

EH_Conversion:
    ToDateTimeFixed = vDefault

End Function

Public Function ToStringFixed(ByVal vValue As Variant, _
    Optional ByVal vDefault As Variant = "") As Variant

    On Error GoTo EH_Conversion

    ' Copy input value (see module-level comment).
    Dim vCopy As Variant
    vCopy = vValue

    Select Case VarType(vCopy)
    Case vbEmpty, vbNull
        GoTo EH_Conversion
    Case vbString
        If vCopy = "" Then GoTo EH_Conversion
    End Select

    ' Handle undocumented vartypes as well since ADO could pass them in.
    Select Case VarType(vCopy)
    Case VT_BOOL
        ' Stored as 0 or 1.
        ToStringFixed = IIf(vCopy, "1", "0")
    Case VT_I1, VT_UI1, VT_I2, VT_UI2, VT_I4, VT_UI4, VT_INT, VT_UINT, VT_I8, VT_UI8, VT_R4, VT_R8, VT_CY, VT_DECIMAL
        ' Locale-independent string conversion.
        ToStringFixed = LTrim$(Str$(vCopy))
    Case VT_DATE
        ' Locale-independent date/time format. This may contain
        ' a date or a time or both.
        Dim sFormat As String
        If vCopy = DateValue(vCopy) Then
            sFormat = "yyyy\/mm\/dd"
        ElseIf vCopy = TimeValue(vCopy) Then
            sFormat = "hh\:nn\:ss"
        Else
            sFormat = "yyyy\/mm\/dd hh\:nn\:ss"
        End If
        ToStringFixed = Format$(vCopy, sFormat)
    Case VT_BSTR
        ' No change for a string.
        ToStringFixed = vCopy
    Case Else ' unrecognised format
        GoTo EH_Conversion
    End Select
    Exit Function

EH_Conversion:
    ToStringFixed = vDefault

End Function

Public Function ToBooleanXML(ByVal vValue As Variant, _
    Optional ByVal vDefault As Variant = False) As Variant

    On Error GoTo EH_Handler

    ' Copy input value (see module-level comment).
    Dim vCopy As Variant
    vCopy = vValue

    Select Case VarType(vCopy)
    Case vbEmpty, vbNull
        GoTo EH_Handler
    Case vbString
        If vCopy = "" Then GoTo EH_Handler
    End Select

    Select Case LCase$(Trim$(CStr(vCopy)))
    Case "1", "yes", "true"
        ToBooleanXML = True
    Case "0", "no", "false"
        ToBooleanXML = False
    Case Else
        ToBooleanXML = vDefault
    End Select
    Exit Function

EH_Handler:
    ToBooleanXML = vDefault

End Function

Public Function ToByteXML(ByVal vValue As Variant, _
    Optional ByVal vDefault As Variant = 0) As Variant

    On Error GoTo EH_Handler

    ' Copy input value (see module-level comment).
    Dim vCopy As Variant
    vCopy = vValue

    Select Case VarType(vCopy)
    Case vbEmpty, vbNull
        GoTo EH_Handler
    Case vbString
        If vCopy = "" Then GoTo EH_Handler
    End Select

    ToByteXML = CByte(Val(vCopy))
    Exit Function

EH_Handler:
    ToByteXML = vDefault

End Function

Public Function ToIntegerXML(ByVal vValue As Variant, _
    Optional ByVal vDefault As Variant = 0) As Variant

    On Error GoTo EH_Handler

    ' Copy input value (see module-level comment).
    Dim vCopy As Variant
    vCopy = vValue

    Select Case VarType(vCopy)
    Case vbEmpty, vbNull
        GoTo EH_Handler
    Case vbString
        If vCopy = "" Then GoTo EH_Handler
    End Select

    ToIntegerXML = CInt(Val(vCopy))
    Exit Function

EH_Handler:
    ToIntegerXML = vDefault

End Function

Public Function ToLongXML(ByVal vValue As Variant, _
    Optional ByVal vDefault As Variant = 0&) As Variant

    On Error GoTo EH_Handler

    ' Copy input value (see module-level comment).
    Dim vCopy As Variant
    vCopy = vValue

    Select Case VarType(vCopy)
    Case vbEmpty, vbNull
        GoTo EH_Handler
    Case vbString
        If vCopy = "" Then GoTo EH_Handler
    End Select

    ToLongXML = CLng(Val(vCopy))
    Exit Function

EH_Handler:
    ToLongXML = vDefault

End Function

Public Function ToDoubleXML(ByVal vValue As Variant, _
    Optional ByVal vDefault As Variant = 0#) As Variant

    On Error GoTo EH_Handler

    ' Copy input value (see module-level comment).
    Dim vCopy As Variant
    vCopy = vValue

    Select Case VarType(vCopy)
    Case vbEmpty, vbNull
        GoTo EH_Handler
    Case vbString
        If vCopy = "" Then GoTo EH_Handler
    End Select

    ToDoubleXML = CDbl(Val(vCopy))
    Exit Function

EH_Handler:
    ToDoubleXML = vDefault

End Function

Public Function ToCurrencyXML(ByVal vValue As Variant, _
    Optional ByVal vDefault As Variant = 0@) As Variant

    On Error GoTo EH_Handler

    ' Copy input value (see module-level comment).
    Dim vCopy As Variant
    vCopy = vValue

    Select Case VarType(vCopy)
    Case vbEmpty, vbNull
        GoTo EH_Handler
    Case vbString
        If vCopy = "" Then GoTo EH_Handler
    End Select

    ToCurrencyXML = CCur(Val(vCopy))
    Exit Function

EH_Handler:
    ToCurrencyXML = vDefault

End Function

Public Function ToDateXML(ByVal vValue As Variant, _
    Optional ByVal vDefault As Variant) As Variant

    If IsMissing(vDefault) Then vDefault = Null

    On Error GoTo EH_Handler

    ' Copy input value (see module-level comment).
    Dim vCopy As Variant
    vCopy = vValue

    Select Case VarType(vCopy)
    Case vbEmpty, vbNull
        GoTo EH_Handler
    Case vbString
        If vCopy = "" Then GoTo EH_Handler
    Case Else
        ToDateXML = DateValue(CDate(vCopy))
        Exit Function
    End Select

    Dim nYear As Integer
    Dim nMonth As Integer
    Dim nDay As Integer
    Dim nHour As Integer
    Dim nMinute As Integer
    Dim nSecond As Integer

    ParseDateTimeXML vCopy, nYear, nMonth, nDay, nHour, nMinute, nSecond
    ToDateXML = DateSerial(nYear, nMonth, nDay)
    Exit Function

EH_Handler:
    ToDateXML = vDefault

End Function

Public Function ToTimeXML(ByVal vValue As Variant, _
    Optional ByVal vDefault As Variant) As Variant

    If IsMissing(vDefault) Then vDefault = Null

    On Error GoTo EH_Handler

    ' Copy input value (see module-level comment).
    Dim vCopy As Variant
    vCopy = vValue

    Select Case VarType(vCopy)
    Case vbEmpty, vbNull
        GoTo EH_Handler
    Case vbString
        If vCopy = "" Then GoTo EH_Handler
    Case Else
        ToTimeXML = TimeValue(CDate(vCopy))
        Exit Function
    End Select

    Dim nYear As Integer
    Dim nMonth As Integer
    Dim nDay As Integer
    Dim nHour As Integer
    Dim nMinute As Integer
    Dim nSecond As Integer

    ParseDateTimeXML vCopy, nYear, nMonth, nDay, nHour, nMinute, nSecond
    ToTimeXML = TimeSerial(nHour, nMinute, nSecond)
    Exit Function

EH_Handler:
    ToTimeXML = vDefault

End Function

Public Function ToDateTimeXML(ByVal vValue As Variant, _
    Optional ByVal vDefault As Variant) As Variant

    If IsMissing(vDefault) Then vDefault = Null

    On Error GoTo EH_Handler

    ' Copy input value (see module-level comment).
    Dim vCopy As Variant
    vCopy = vValue

    Select Case VarType(vCopy)
    Case vbEmpty, vbNull
        GoTo EH_Handler
    Case vbString
        If vCopy = "" Then GoTo EH_Handler
    Case Else
        ToDateTimeXML = CDate(vCopy)
        Exit Function
    End Select

    Dim nYear As Integer
    Dim nMonth As Integer
    Dim nDay As Integer
    Dim nHour As Integer
    Dim nMinute As Integer
    Dim nSecond As Integer

    ParseDateTimeXML vCopy, nYear, nMonth, nDay, nHour, nMinute, nSecond
    ToDateTimeXML = DateSerial(nYear, nMonth, nDay) + TimeSerial(nHour, nMinute, nSecond)
    Exit Function

EH_Handler:
    ToDateTimeXML = vDefault

End Function

Public Function ToStringXML(ByVal vValue As Variant, _
    Optional ByVal vDefault As Variant = "", _
    Optional ByVal nFormat As Long = knXFUnspecified) As Variant

    On Error GoTo EH_Handler

    ' Copy input value (see module-level comment).
    Dim vCopy As Variant
    vCopy = vValue

    Select Case VarType(vCopy)
    Case vbEmpty, vbNull
        GoTo EH_Handler
    Case vbString
        If vCopy = "" Then GoTo EH_Handler
    End Select

    ' Handle undocumented vartypes as well since ADO could pass them in.
    Select Case VarType(vCopy)
    Case VT_BOOL
        ' Convert according to user requirements.
        Select Case nFormat
        Case knXFBit
            ToStringXML = IIf(vCopy, "1", "0")
        Case knXFYesNoM
            ToStringXML = IIf(vCopy, "Yes", "No")
        Case knXFYesNoL
            ToStringXML = IIf(vCopy, "yes", "no")
        Case knXFTrueFalseM
            ToStringXML = IIf(vCopy, "True", "False")
        Case Else ' knXFTrueFalseL
            ToStringXML = IIf(vCopy, "true", "false")
        End Select
    Case VT_I1, VT_UI1, VT_I2, VT_UI2, VT_I4, VT_UI4, VT_INT, VT_UINT, VT_I8, VT_UI8, VT_R4, VT_R8, VT_CY, VT_DECIMAL
        ' XML number format.
        ToStringXML = LTrim$(Str$(vCopy))
    Case VT_DATE
        ' Convert according to user requirements.
        Dim sFormat As String
        Select Case nFormat
        Case knXFYear
            sFormat = "yyyy"
        Case knXFMonth
            sFormat = "\-\-mm\-\-"
        Case knXFDay
            sFormat = "\-\-\-dd"
        Case knXFYearMonth
            sFormat = "yyyy\-mm"
        Case knXFMonthDay
            sFormat = "\-\-mm\-dd"
        Case knXFDate
            sFormat = "yyyy\-mm\-dd"
        Case knXFTime
            sFormat = "hh\:nn\:ss"
        Case Else ' knXFDateTime
            sFormat = "yyyy\-mm\-dd\Thh\:nn\:ss"
        End Select
        ToStringXML = Format$(vCopy, sFormat)
    Case VT_BSTR
        ' No change for a string.
        ToStringXML = vCopy
    Case Else ' unrecognised format
        GoTo EH_Handler
    End Select
    Exit Function

EH_Handler:
    ToStringXML = vDefault

End Function

' Converts ID values from zero to null.
' NOT TO BE USED FOR ANY OTHER PURPOSE
Public Function ToIDType(ByVal lValue As Long) As Variant

    If lValue = 0 Then
        ToIDType = Null
    Else
        ToIDType = lValue
    End If

End Function

' Converts any value to a string suitable for sorting in ANSI
' order. Null and Empty values are sorted to the top.
Public Function ToSortable(ByVal vValue As Variant) As String

    Dim sFormat As String

    ' Copy input value (see module-level comment).
    Dim vCopy As Variant
    vCopy = vValue

    ' Handle undocumented vartypes as well since ADO could pass them in.
    Select Case VarType(vCopy)
    Case VT_EMPTY, VT_NULL
        ToSortable = ""
    Case VT_BOOL
        ' Sort as "F" or "T" (false first).
        ToSortable = IIf(vCopy, "T", "F")
    Case VT_I1, VT_UI1, VT_I2, VT_UI2, VT_I4, VT_UI4, VT_INT, VT_UINT, VT_I8, VT_UI8
        ' Sort right-justified, placing minus values first.
        sFormat = """P""000000000000;""M""000000000000"
        ToSortable = Format$(vCopy, sFormat)
    Case VT_R4, VT_R8, VT_CY, VT_DECIMAL
        ' Sort right-justified, placing minus values first.
        sFormat = """P""000000000000.0;""M""000000000000.0"
        ToSortable = Format$(vCopy, sFormat)
    Case VT_DATE
        ' Sort by year, month, day, hour, minute, second.
        sFormat = "yyyymmddhhnnss"
        ToSortable = Format$(vCopy, sFormat)
    Case VT_BSTR
        ' Sort case-independent and ignore leading spaces.
        ToSortable = UCase$(Trim$(vCopy))
    Case Else
        ' Unrecognised data type.
        Err.Raise knErrSortConversion, ksErrSource, ksErrSortConversion
    End Select

End Function

' Converts a variant into the best form for a Crystal value.
' NOT TO BE USED FOR ANY OTHER PURPOSE
Public Function ToCrystal(ByVal vValue As Variant, _
    Optional ByVal nDataType As VBA.VbVarType = vbEmpty) As String

    Dim sFormat As String

    ' Copy input value (see module-level comment).
    Dim vCopy As Variant
    vCopy = vValue

    ' Handle undocumented vartypes as well since ADO could pass them in.
    Select Case VarType(vCopy)
    Case VT_EMPTY, VT_NULL
        ' Return an empty value of the specified data type.
        Select Case nDataType
        Case vbBoolean
            ToCrystal = "False"
        Case vbByte, vbInteger, vbLong, vbSingle, vbDouble, vbCurrency
            ToCrystal = "0"
        Case vbDate
            ToCrystal = "Date(0,0,0)"
        Case Else ' vbEmpty, vbNull, vbString
            ToCrystal = """"""
        End Select
    Case VT_BOOL
        ToCrystal = IIf(vCopy, "True", "False")
    Case VT_I1, VT_UI1, VT_I2, VT_UI2, VT_I4, VT_UI4, VT_INT, VT_UINT, VT_I8, VT_UI8, VT_R4, VT_R8, VT_CY, VT_DECIMAL
        ' Locale-independent string conversion.
        ToCrystal = LTrim$(Str$(vCopy))
    Case VT_DATE
        ' Handle both dates and times, and use an unambiguous format.
        If vCopy = DateValue(vCopy) Then
            sFormat = """Date(""yyyy"",""mm"",""dd"")"""
        ElseIf vCopy = TimeValue(vCopy) Then
            sFormat = """Time(""hh"",""nn"",""ss"")"""
        Else
            sFormat = """DateTime(""yyyy"",""mm"",""dd"",""hh"",""nn"",""ss"")"""
        End If
        ToCrystal = Format$(vCopy, sFormat)
    Case VT_BSTR
        ' Translate quotes (Crystal doesn't recognise quote-doubling).
        ToCrystal = """" & Replace$(vCopy, """", "'") & """"
    Case Else
        Err.Raise knErrCrystalConversion, ksErrSource, ksErrCrystalConversion
    End Select

End Function

' Converts a variant into the best form for a Transact-SQL literal value.
' NOT TO BE USED FOR ANY OTHER PURPOSE
Public Function ToSQL(ByVal vValue As Variant) As String

    Dim sFormat As String
    Dim iByte As Long
    Dim sValue As String
    Dim iByteFirst As Long
    Dim iByteLast As Long

    ' Copy input value (see module-level comment).
    Dim vCopy As Variant
    vCopy = vValue

    ' Handle undocumented vartypes as well since ADO could pass them in.
    Select Case VarType(vCopy)
    Case VT_NULL, VT_EMPTY
        ToSQL = "null"
    Case VT_BOOL
        ' This matches the BOOLEAN type definition in the database.
        ToSQL = IIf(vCopy, "-1", "0")
    Case VT_I1, VT_UI1, VT_I2, VT_UI2, VT_I4, VT_UI4, VT_INT, VT_UINT, VT_I8, VT_UI8, VT_R4, VT_R8, VT_CY, VT_DECIMAL
        ' Locale-independent string conversion.
        ToSQL = LTrim$(Str$(vCopy))
    Case VT_DATE
        ' Handle both dates and times, and use locale-independent format.
        If vCopy = DateValue(vCopy) Then
            sFormat = "{\d'yyyy\-mm\-dd'}"
        ElseIf vCopy = TimeValue(vCopy) Then
            sFormat = "{\t'hh\:nn\:ss'}"
        Else
            sFormat = "{\t\s'yyyy\-mm\-dd hh\:nn\:ss'}"
        End If
        ToSQL = Format$(vCopy, sFormat)
    Case VT_BSTR
        ' Must handle quotes properly.
        ToSQL = "'" & Replace$(vCopy, "'", "''") & "'"
    Case VT_ARRAY + VT_I1, VT_ARRAY + VT_UI1
        ' Translate a byte array into a binary literal.
        On Error Resume Next
        iByteFirst = LBound(vCopy)
        iByteLast = UBound(vCopy)
        sValue = ""
        If Err = 0 Then
            sValue = "0x"
            For iByte = iByteFirst To iByteLast
                sValue = sValue & Right$("00" & Hex$(vCopy(iByte)), 2)
            Next
        Else
            Err.Clear
        End If
        ToSQL = sValue
    Case Else
        Err.Raise knErrSQLConversion, ksErrSource, ksErrSQLConversion
    End Select

End Function

' Converts a variant into the best form for a Jet-SQL literal value.
' NOT TO BE USED FOR ANY OTHER PURPOSE
Public Function ToJSQL(ByVal vValue As Variant) As String

    Dim sFormat As String
    Dim iByte As Long
    Dim sValue As String
    Dim iByteFirst As Long
    Dim iByteLast As Long

    ' Copy input value (see module-level comment).
    Dim vCopy As Variant
    vCopy = vValue

    ' Handle undocumented vartypes as well since ADO could pass them in.
    Select Case VarType(vCopy)
    Case VT_NULL, VT_EMPTY
        ToJSQL = "null"
    Case VT_BOOL
        ' This matches the BOOLEAN type definition in the database.
        ToJSQL = IIf(vCopy, "-1", "0")
    Case VT_I1, VT_UI1, VT_I2, VT_UI2, VT_I4, VT_UI4, VT_INT, VT_UINT, VT_I8, VT_UI8, VT_R4, VT_R8, VT_CY, VT_DECIMAL
        ' Locale-independent string conversion.
        ToJSQL = LTrim$(Str$(vCopy))
    Case VT_DATE
        ' Handle both dates and times, and use locale-independent format.
        If vCopy = DateValue(vCopy) Then
            sFormat = "\#dd mmm yyyy\#"
        ElseIf vCopy = TimeValue(vCopy) Then
            sFormat = "\#hh\:nn\:ss\#"
        Else
            sFormat = "\#dd mmm yyyy hh\:nn\:ss\#"
        End If
        ToJSQL = Format$(vCopy, sFormat)
    Case VT_BSTR
        ' Must handle quotes properly.
        ToJSQL = "'" & Replace$(vCopy, "'", "''") & "'"
    Case VT_ARRAY + VT_I1, VT_ARRAY + VT_UI1
        ' Translate a byte array into a binary literal.
        On Error Resume Next
        iByteFirst = LBound(vCopy)
        iByteLast = UBound(vCopy)
        sValue = ""
        If Err = 0 Then
            sValue = "0x"
            For iByte = iByteFirst To iByteLast
                sValue = sValue & Right$("00" & Hex$(vCopy(iByte)), 2)
            Next
        Else
            Err.Clear
        End If
        ToJSQL = sValue
    Case Else
        Err.Raise knErrSQLConversion, ksErrSource, ksErrSQLConversion
    End Select

End Function

' Converts a variant into the best form for a Crystal stored procedure parameter value.
' NOT TO BE USED FOR ANY OTHER PURPOSE
Public Function ToCrystalSQLParam(ByVal vValue As Variant) As String

    ' Copy input value (see module-level comment).
    Dim vCopy As Variant
    vCopy = vValue

    ' Handle undocumented vartypes as well since ADO could pass them in.
    Select Case VarType(vCopy)
    Case VT_NULL, VT_EMPTY
        ToCrystalSQLParam = "CRWNull"
    Case VT_BOOL
        ' This matches the BOOLEAN type definition in the database.
        ToCrystalSQLParam = IIf(vCopy, "-1", "0")
    Case VT_I1, VT_UI1, VT_I2, VT_UI2, VT_I4, VT_UI4, VT_INT, VT_UINT, VT_I8, VT_UI8, VT_R4, VT_R8, VT_CY, VT_DECIMAL
        ' Locale-independent string conversion.
        ToCrystalSQLParam = LTrim$(Str$(vCopy))
    Case VT_DATE
        ' Locale-independent format.
        ToCrystalSQLParam = Format$(vCopy, "yyyy\-mm\-dd hh\:nn\:ss\.\0\0\0")
    Case VT_BSTR
        ' No translation possible here.
        ToCrystalSQLParam = vCopy
    Case Else
        Err.Raise knErrCrystalConversion, ksErrSource, ksErrCrystalConversion
    End Select

End Function

' Converts a variant into the best form for an XPath literal value.
' NOT TO BE USED FOR ANY OTHER PURPOSE
Public Function ToXPath(ByVal vValue As Variant) As String

    ' Copy input value (see module-level comment).
    Dim vCopy As Variant
    vCopy = vValue

    ' Handle undocumented vartypes as well since ADO could pass them in.
    Select Case VarType(vCopy)
    Case VT_BOOL, VT_I1, VT_UI1, VT_I2, VT_UI2, VT_I4, VT_UI4, VT_INT, VT_UINT, VT_I8, VT_UI8, VT_R4, VT_R8, VT_CY, VT_DECIMAL
        ToXPath = ToStringXML(vCopy)
    Case VT_BSTR
        ' Replace illegal characters with entity references.
        ToXPath = "'" & EscapeXMLText(vCopy) & "'"
    Case Else
        Err.Raise knErrXPathConversion, ksErrSource, ksErrXPathConversion
    End Select

End Function

' Converts a variant into a useful format for debugging the contents.
Public Function ToDebug(ByVal vValue As Variant, _
    Optional ByVal bExtraSafe As Boolean = False) As String

    Dim vCopy As Variant
    Dim nVarType As VbVarType
    Dim sTypeName As String
    Dim sLBound As String
    Dim sUBound As String

    ' This function could be used to diagnose problems like an error occurs when reading
    ' a property value. In this situation, we must not throw any errors out to the calling
    ' code, but instead trap them and display semi-useful text.
    On Error GoTo EH_Handler

    ' This function could be used to diagnose problems like when the value changes when
    ' read more than once. In this situation, we must make exactly one copy of the value,
    ' and operate on that throughout.
    If bExtraSafe Then
        vCopy = vValue
    ElseIf IsObject(vValue) Then
        Set vCopy = vValue
    Else
        vCopy = vValue
    End If

    ' The VarType() function actually returns the value of the internal VARIANT structure
    ' type field. To accommodate code that passes in variants of a type unsupported by VB,
    ' we must interpret undocumented types as well.
    nVarType = VarType(vCopy)
    Select Case nVarType
    Case VT_EMPTY
        ToDebug = "Empty"
    Case VT_NULL
        ToDebug = "Null"
    Case VT_BOOL, VT_I1, VT_UI1, VT_I2, VT_UI2, VT_I4, VT_UI4, VT_INT, VT_UINT, VT_I8, VT_UI8, VT_R4, VT_R8, VT_CY, VT_DECIMAL
        ToDebug = CStr(vCopy)
    Case VT_DATE
        If vCopy = DateValue(vCopy) Then
            ToDebug = Format$(vCopy, "\#dd mmm yyyy#")
        ElseIf vCopy = TimeValue(vCopy) Then
            ToDebug = Format$(vCopy, "\#hh\:nn\:ss\#")
        Else
            ToDebug = Format$(vCopy, "\#dd mmm yyyy hh\:nn\:ss\#")
        End If
    Case VT_BSTR
        ToDebug = """" & Replace(vCopy, """", """""") & """"
    Case VT_ERROR
        ToDebug = "Error(" & CStr(CLng(vCopy)) & ")"
    Case Else
        sTypeName = TypeName(vCopy)
        If nVarType And VT_ARRAY Then
            On Error Resume Next
            sLBound = "?"
            sLBound = LBound(vCopy)
            Err.Clear
            sUBound = "?"
            sUBound = UBound(vCopy)
            Err.Clear
            On Error GoTo EH_Handler
            sTypeName = RemoveChars(sTypeName, "()") & "(" & sLBound & " To " & sUBound & ")"
        End If
        ToDebug = sTypeName
    End Select
    Exit Function

EH_Handler:
    ToDebug = "{" & Err.Description & "}"
    Exit Function

End Function

' Access an object's default property as if it was a primitive data type.
Public Property Get ObjValue(ByRef r_vObject As Variant) As Variant

    Dim oObject As Object

    If IsObject(r_vObject) Then
        Set oObject = r_vObject
        ObjValue = oObject
    Else
        ObjValue = r_vObject
    End If

End Property

' Access an object's default property as if it was a primitive data type.
Public Property Let ObjValue(ByRef r_vObject As Variant, ByVal vObjValue As Variant)

    Dim oObject As Object

    ' Copy input value (see module-level comment).
    Dim vCopy As Variant
    vCopy = vObjValue

    If IsObject(r_vObject) Then
        Set oObject = r_vObject
        oObject = vCopy
    Else
        r_vObject = vCopy
    End If

End Property

' Helper function - replaces illegal characters with XML entity references.
Public Function EscapeXMLText(ByVal sValue As String) As String

    sValue = Replace$(sValue, "&", "&amp;")
    sValue = Replace$(sValue, "'", "&apos;")
    sValue = Replace$(sValue, """", "&quot;")
    sValue = Replace$(sValue, "<", "&lt;")
    sValue = Replace$(sValue, ">", "&gt;")
    EscapeXMLText = sValue

End Function

' Helper function - returns the parsed date/time fragments from a string.
Public Sub ParseDateTimeFixed(ByVal sValue As String, _
    ByRef r_nYear As Integer, _
    ByRef r_nMonth As Integer, _
    ByRef r_nDay As Integer, _
    ByRef r_nHour As Integer, _
    ByRef r_nMinute As Integer, _
    ByRef r_nSecond As Integer)

    Dim sChar1 As String
    Dim nYearOrHour As Integer

    On Error GoTo EH_Handler

    r_nYear = 0
    r_nMonth = 0
    r_nDay = 0
    r_nHour = 0
    r_nMinute = 0
    r_nSecond = 0

    ' Positive year or hour
    nYearOrHour = Val(ParseDigits(sValue, sValue))
    sChar1 = Left$(sValue, 1)
    If sValue = "" Then
        ' Year
        r_nYear = nYearOrHour
    ElseIf sChar1 = "/" Then
        ' Year
        r_nYear = nYearOrHour
        ' Month
        sValue = Mid$(sValue, 2)
        r_nMonth = Val(ParseDigits(sValue, sValue))
        If Left$(sValue, 1) = "/" Then
            ' Day
            sValue = Mid$(sValue, 2)
            r_nDay = Val(ParseDigits(sValue, sValue))
            If Left$(sValue, 1) = " " Then
                ' Hour
                sValue = Mid$(sValue, 2)
                r_nHour = Val(ParseDigits(sValue, sValue))
                If Left$(sValue, 1) = ":" Then
                    ' Minute
                    sValue = Mid$(sValue, 2)
                    r_nMinute = Val(ParseDigits(sValue, sValue))
                    If Left$(sValue, 1) = ":" Then
                        ' Second
                        sValue = Mid$(sValue, 2)
                        r_nSecond = Val(ParseDigits(sValue, sValue))
                    End If
                End If
            End If
        End If
    ElseIf sChar1 = ":" Then
        ' Hour
        r_nHour = nYearOrHour
        ' Minute
        sValue = Mid$(sValue, 2)
        r_nMinute = Val(ParseDigits(sValue, sValue))
        If Left$(sValue, 1) = ":" Then
            ' Second
            sValue = Mid$(sValue, 2)
            r_nSecond = Val(ParseDigits(sValue, sValue))
        End If
    End If

EH_Handler:

End Sub

' Helper function - returns the parsed date/time fragments from a string.
Public Sub ParseDateTimeXML(ByVal sValue As String, _
    ByRef r_nYear As Integer, _
    ByRef r_nMonth As Integer, _
    ByRef r_nDay As Integer, _
    ByRef r_nHour As Integer, _
    ByRef r_nMinute As Integer, _
    ByRef r_nSecond As Integer)

    Dim sChar1 As String
    Dim sChar12 As String
    Dim sChar123 As String
    Dim nYearOrHour As Integer

    On Error GoTo EH_Handler

    r_nYear = 0
    r_nMonth = 0
    r_nDay = 0
    r_nHour = 0
    r_nMinute = 0
    r_nSecond = 0

    sChar1 = Left$(sValue, 1)
    sChar12 = Left$(sValue, 2)
    sChar123 = Left$(sValue, 3)

    If sChar123 = "---" Then
        ' Day
        sValue = Mid$(sValue, 4)
        r_nDay = Val(ParseDigits(sValue, sValue))
    ElseIf sChar12 = "--" Then
        ' Month
        sValue = Mid$(sValue, 3)
        r_nMonth = Val(ParseDigits(sValue, sValue))
        If Left$(sValue, 1) = "-" Then
            ' Day
            sValue = Mid$(sValue, 2)
            r_nDay = Val(ParseDigits(sValue, sValue))
        End If
    ElseIf sChar1 = "-" Then
        ' Negative year
        sValue = Mid$(sValue, 2)
        r_nYear = Val(ParseDigits(sValue, sValue))
        If Left$(sValue, 1) = "-" Then
            ' Month
            sValue = Mid$(sValue, 2)
            r_nMonth = Val(ParseDigits(sValue, sValue))
            If Left$(sValue, 1) = "-" Then
                ' Day
                sValue = Mid$(sValue, 2)
                r_nDay = Val(ParseDigits(sValue, sValue))
                If Left$(sValue, 1) = "T" Then
                    ' Hour
                    sValue = Mid$(sValue, 2)
                    r_nHour = Val(ParseDigits(sValue, sValue))
                    If Left$(sValue, 1) = ":" Then
                        ' Minute
                        sValue = Mid$(sValue, 2)
                        r_nMinute = Val(ParseDigits(sValue, sValue))
                        If Left$(sValue, 1) = ":" Then
                            ' Second
                            sValue = Mid$(sValue, 2)
                            r_nSecond = Val(ParseDigits(sValue, sValue))
                        End If
                    End If
                End If
            End If
        End If
    Else
        ' Positive year or hour
        nYearOrHour = Val(ParseDigits(sValue, sValue))
        sChar1 = Left$(sValue, 1)
        If sValue = "" Then
            ' Year
            r_nYear = nYearOrHour
        ElseIf sChar1 = "-" Then
            ' Year
            r_nYear = nYearOrHour
            ' Month
            sValue = Mid$(sValue, 2)
            r_nMonth = Val(ParseDigits(sValue, sValue))
            If Left$(sValue, 1) = "-" Then
                ' Day
                sValue = Mid$(sValue, 2)
                r_nDay = Val(ParseDigits(sValue, sValue))
                If Left$(sValue, 1) = "T" Then
                    ' Hour
                    sValue = Mid$(sValue, 2)
                    r_nHour = Val(ParseDigits(sValue, sValue))
                    If Left$(sValue, 1) = ":" Then
                        ' Minute
                        sValue = Mid$(sValue, 2)
                        r_nMinute = Val(ParseDigits(sValue, sValue))
                        If Left$(sValue, 1) = ":" Then
                            ' Second
                            sValue = Mid$(sValue, 2)
                            r_nSecond = Val(ParseDigits(sValue, sValue))
                        End If
                    End If
                End If
            End If
        ElseIf sChar1 = ":" Then
            ' Hour
            r_nHour = nYearOrHour
            ' Minute
            sValue = Mid$(sValue, 2)
            r_nMinute = Val(ParseDigits(sValue, sValue))
            If Left$(sValue, 1) = ":" Then
                ' Second
                sValue = Mid$(sValue, 2)
                r_nSecond = Val(ParseDigits(sValue, sValue))
            End If
        End If
    End If

EH_Handler:

End Sub

' If the string starts with digits, split them out.
Private Function ParseDigits(ByVal sLine As String, _
    ByRef r_sRemainder As String) As String

    Dim iPos As Long

    ParseDigits = sLine
    r_sRemainder = ""

    For iPos = 1 To Len(sLine)
        Select Case Mid$(sLine, iPos, 1)
        Case "0" To "9"
            ' continue
        Case Else
            ' stop looking any further
            ParseDigits = Left$(sLine, iPos - 1)
            r_sRemainder = Mid$(sLine, iPos)
            Exit For
        End Select
    Next

End Function
