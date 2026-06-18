Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Text
Module NumberToWord

    Public Function NumToWord(ByVal v_cValue As Decimal, Optional ByVal v_bConvertDecimal As Boolean = True, Optional ByVal v_sIntegerPartCode As String = "Pounds", Optional ByVal v_sDecimalPartcode As String = "Pence") As String

        ' This function will only work upto Trillion

        Dim sNTW As New StringBuilder 'temp variable
        Dim sNText As String = "" ' string version of number
        Dim lDecPlace As Integer 'position of decimal place in number
        Dim sIntegerPart As String = "" 'integer part of number
        Dim lIntegerPartLen As Integer 'length of integer part
        Dim lCurrentIntegerPartLen As Integer 'length of integer part
        Dim sDecimalPart As String = "" 'decimal part of number
        Dim lSetOfThree As Integer 'number of set of three digit
        Dim lCount As Integer 'loop counter
        Dim sNumberWord, sDecimalPartWord As String

        Dim NumParts(5) As String 'Array for Amount (sets of three)
        Dim place(5) As String 'Array containing place holders

        Try

            place(2) = " Thousand "
            place(3) = " Million "

            ' uk definition of a billion = 10^12
            ' uk definition of a trillion = 10^18
            ' other countries (e.g. USA) define billion 10^9
            ' other countries (e.g. USA) define trillion 10^12
            ' for now only use uk definitions
            place(4) = " Thousand Million "
            place(5) = " Billion "

            sNTW = New StringBuilder("")
            sNText = Conversion.Str(round_currency(v_cValue, 2, True)).Trim() 'round up decimal
            lDecPlace = (sNText.Trim().IndexOf("."c) + 1) 'Position of decimal

            sIntegerPart = sNText.Substring(0, IIf(lDecPlace = 0, sNText.Length, lDecPlace - 1)).Trim()
            lIntegerPartLen = sIntegerPart.Length
            sDecimalPart = sNText.Substring(sNText.Length - (IIf(lDecPlace = 0, 0, CInt(Math.Abs(lDecPlace - sNText.Length))))).Trim()

            If sDecimalPart.Length = 1 Then
                sDecimalPart = sDecimalPart & "0"
            ElseIf sDecimalPart = "" Then
                sDecimalPart = 0
            End If

            If (lIntegerPartLen Mod 3) = 0 Then
                lSetOfThree = (lIntegerPartLen \ 3)
            Else
                lSetOfThree = (lIntegerPartLen \ 3) + 1
            End If

            lCount = 1
            lCurrentIntegerPartLen = lIntegerPartLen

            Do While lCurrentIntegerPartLen > 0
                NumParts(lCount) = IIf(lCurrentIntegerPartLen > 3, Right(sIntegerPart, 3), sIntegerPart.Trim())
                sIntegerPart = IIf(lCurrentIntegerPartLen > 3, Left(sIntegerPart, (IIf(lCurrentIntegerPartLen < 3, 3, lCurrentIntegerPartLen)) - 3), "")
                lCurrentIntegerPartLen = Len(sIntegerPart)
                lCount += 1
            Loop

            For lCount = lSetOfThree To 1 Step -1 'step through NumParts array
                sNumberWord = GetWord(NumParts(lCount)) 'convert 1 element of NumParts
                sNTW.Append(sNumberWord)
                If sNumberWord <> "" Then sNTW.Append(place(lCount))
            Next lCount

            If lIntegerPartLen > 0 Then
                'Modified by Sudhanshu Behera on 5/12/2010 7:17:52 PM refer developer guide no. 114 (guide)
                'sNTW = New StringBuilder(sNTW.ToString().Trim() & " " & v_sIntegerPartCode.Trim() & (IIf(v_bConvertDecimal, " and ", "")))
                sNTW.Replace(sNTW.ToString().Trim(), (sNTW.ToString().Trim() & " " & v_sIntegerPartCode.Trim() & (IIf(v_bConvertDecimal, " and ", ""))))
            Else
                'Modified by Sudhanshu Behera on 5/12/2010 7:18:17 PM refer developer guide no. 114 (guide)
                'sNTW = New StringBuilder(sNTW.ToString().Trim() & " Zero " & v_sIntegerPartCode.Trim() & (IIf(v_bConvertDecimal, " and ", "")))
                sNTW.Append(sNTW.ToString().Trim() & " Zero " & v_sIntegerPartCode.Trim() & (IIf(v_bConvertDecimal, " and ", "")))
            End If

            If v_bConvertDecimal Then
                sDecimalPartWord = GetTens(sDecimalPart)

                If sDecimalPartWord = "" Then sDecimalPartWord = "Zero"

                sNTW.Append(sDecimalPartWord.Trim() & " " & v_sDecimalPartcode.Trim())
            End If

            sNTW.Replace("  ", " ")
            Return sNTW.ToString()

        Catch


            Return ""
        End Try
    End Function

    Private Function GetDigit(ByVal sDigit As String) As String
        'assign numeric word value based on a single digit

        Select Case Conversion.Val(sDigit)
            Case 1 : Return "One"
            Case 2 : Return "Two"
            Case 3 : Return "Three"
            Case 4 : Return "Four"
            Case 5 : Return "Five"
            Case 6 : Return "Six"
            Case 7 : Return "Seven"
            Case 8 : Return "Eight"
            Case 9 : Return "Nine"
            Case Else : Return ""
        End Select
    End Function

    Private Function GetTens(ByVal v_stenstext As String) As String
        'convert number from 10 to 99 to word


        Dim sGT As String = "" 'null out the temporary function value


        If Conversion.Val(v_stenstext.Substring(0, 1)) = 1 Then ' If value between 10-19
            Select Case Conversion.Val(v_stenstext)
                Case 10 : sGT = "Ten" '
                Case 11 : sGT = "Eleven"
                Case 12 : sGT = "Twelve"
                Case 13 : sGT = "Thirteen"
                Case 14 : sGT = "Fourteen"
                Case 15 : sGT = "Fifteen"
                Case 16 : sGT = "Sixteen"
                Case 17 : sGT = "Seventeen"
                Case 18 : sGT = "Eighteen"
                Case 19 : sGT = "Nineteen"
            End Select
        Else
            ' If value between 20-99
            Select Case Conversion.Val(v_stenstext.Substring(0, 1))
                Case 2 : sGT = "Twenty "
                Case 3 : sGT = "Thirty "
                Case 4 : sGT = "Forty "
                Case 5 : sGT = "Fifty "
                Case 6 : sGT = "Sixty "
                Case 7 : sGT = "Seventy "
                Case 8 : sGT = "Eighty "
                Case 9 : sGT = "Ninety "
            End Select

            sGT = sGT & GetDigit(v_stenstext.Substring(v_stenstext.Length - 1)) 'Retrieve ones place

        End If

        Return sGT

    End Function

    Private Function GetWord(ByVal v_sNumText As String) As String
        ' The following function converts a number from 0 to 999 to text


        Dim sWordNumber As New StringBuilder

        If Conversion.Val(v_sNumText) > 0 Then
            For lCount As Integer = 1 To v_sNumText.Length 'loop the length of sNumText times
                Select Case v_sNumText.Length
                    Case 3
                        If Conversion.Val(v_sNumText) > 99 Then
                            sWordNumber = New StringBuilder(GetDigit(v_sNumText.Substring(0, 1)) & " Hundred ")
                        End If

                        v_sNumText = v_sNumText.Substring(v_sNumText.Length - 2)

                    Case 2
                        sWordNumber.Append(GetTens(v_sNumText))
                        v_sNumText = ""

                    Case 1
                        sWordNumber = New StringBuilder(GetDigit(v_sNumText))
                End Select
            Next lCount
        End If

        Return sWordNumber.ToString()

    End Function

    Private Function round_currency(ByVal v_cValue As Decimal, ByVal v_lNumberOfDigit As Integer, Optional ByVal v_bBankerRounding As Boolean = True) As Decimal

        Dim nPower As Double
        Dim lSign As Integer

        

            'dictate how manay digit we are rounding to
            nPower = 10 ^ v_lNumberOfDigit

            'get the sign -1 or 1
            lSign = Math.Sign(v_cValue)

            'make sure its positive
            v_cValue = Math.Abs(v_cValue)

            v_cValue = (v_cValue * nPower) + 0.5

            'round up for odd number and down for even number eg 100.135 = 100.14 and 100.145 = 100.14 (round to 2 dec)
            If v_bBankerRounding Then
                If Math.Floor(v_cValue) = v_cValue Then
                    If v_cValue Mod 2 = 1 Then
                        v_cValue -= 1
                    End If
                End If
            End If


            Return lSign * Math.Floor(v_cValue) / nPower


    End Function
End Module

