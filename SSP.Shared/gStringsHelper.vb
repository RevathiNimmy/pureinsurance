Imports System.Runtime.InteropServices
Imports System.Text

Public NotInheritable Class StringsHelper
    Public Structure FixedLengthString
        Dim mValue As String
        Dim mSize As Short

        Public Sub New(Size As Integer)
            mSize = Size
            mValue = New String(" ", mSize)
        End Sub

        Public Property Value As String
            Get
                Value = mValue
            End Get

            Set(value As String)
                If value.Length < mSize Then
                    mValue = value & New String(" ", mSize - value.Length)
                Else
                    mValue = value.Substring(0, mSize)
                End If
            End Set
        End Property
    End Structure
    Public Shared Function Format(ByVal toFormat As Object, ByVal mask As Object) As String
        Dim toFormatString As String = Convert.ToString(toFormat)
        Dim maskString As String

        If String.IsNullOrEmpty(toFormatString) Then
            Return String.Empty
        End If

        Dim dateTimeValue As DateTime
        If (Not toFormatString.Contains(".")) AndAlso DateTime.TryParse(toFormatString, dateTimeValue) Then
            maskString = Convert.ToString(mask)
            ' Use .NET formatting for DateTime
            Return dateTimeValue.ToString(maskString)
        Else
            If mask = "standard" Then
                mask = "0.00"
            End If
            maskString = "{0:" & mask & "}"
            ' Handle non-DateTime formatting
            Return String.Format(maskString, Convert.ToDecimal(toFormatString))
        End If
    End Function
    Public Enum VbStrConvEnum
        vbFromUnicode = &H80
        vbHiragana = &H20
        vbKatakana = &H10
        vbLowerCase = &H2
        vbNarrow = &H8
        vbProperCase = &H3
        vbUnicode = &H4
        vbUpperCase = &H1
        vbWide = &H4
    End Enum
    Public Shared Function StrConv(ByVal str As String, ByVal Conversion As VbStrConvEnum) As String
        Return StrConv(str, Conversion, 0)
    End Function

    Public Shared Function StrConv(ByVal str As String, ByVal Conversion As VbStrConvEnum, ByVal LocaleID As Integer) As String
        Dim empty As String = String.Empty
        Dim zero As IntPtr = IntPtr.Zero

        Select Case Conversion
            Case VbStrConvEnum.vbFromUnicode
                zero = Marshal.StringToHGlobalAnsi(str)
                empty = Marshal.PtrToStringUni(zero)
                Marshal.FreeHGlobal(zero)
            Case VbStrConvEnum.vbUnicode
                Dim bytes As Byte() = Encoding.Convert(Encoding.[Default], Encoding.Unicode, Encoding.Unicode.GetBytes(str))
                empty = Encoding.Unicode.GetString(bytes)
                Exit Select
                'Case Else
                '    empty = Strings.StrConv(str, CType(Conversion, VbStrConv), LocaleID)
        End Select

        Return empty
    End Function
    Public Shared Function Replace(sPropertyName As String, sWhichWillBeReplaced As String, sWhatToBePlaced As String) As String
        Return sPropertyName.Replace(sWhichWillBeReplaced, sWhatToBePlaced)
    End Function
End Class
