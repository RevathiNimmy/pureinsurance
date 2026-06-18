Imports System.Text.RegularExpressions

Public Class Formatting

    Public Shared Function StripAlphaNumeric(ByVal sText As String) As String

        Dim myRegExp As Regex = New Regex("[^A-Za-z0-9_ ]")
        Dim newString As String = String.Empty

        If sText <> "" Then
            newString = myRegExp.Replace(sText, "")
        End If

        Return newString

    End Function

    Public Shared Function StripInvalidFileNameCharacters(ByVal sText As String) As String

        Dim sCharactersToRemove As String = "\/:*?""<>|"

        Dim x As Int32
        For x = 1 To sCharactersToRemove.Length

            sText.Replace(sCharactersToRemove(x), "")

        Next

        Return sText

    End Function

    Public Shared Function BooleanToBit(ByVal BoolVal As Boolean) As Int16

        If BoolVal = True Then
            Return 1
        Else
            Return 0
        End If

    End Function

    Public Shared Function BitToYesNo(ByVal BoolVal As Boolean) As String
        If BoolVal = True Then
            Return "Yes"
        Else
            Return "No"
        End If
    End Function

    Public Shared Function NullToZero(ByVal ntzInt As Object) As Integer

        If IsNothing(ntzInt) Or IsDBNull(ntzInt) Or ntzInt = "" Then
            Return 0
        Else
            Return ntzInt
        End If

    End Function

    Public Shared Function ReplaceQuotes(ByVal InStr As String) As String

        'replace quotes and other troublesome characters from a string
        Dim OutStr As String
        OutStr = Replace(InStr, "'", "''")
        OutStr = Replace(OutStr, Chr(34), Chr(34) & Chr(34))
        Return (OutStr)

    End Function

    Public Shared Function DoQuotes(ByVal InStr As String) As String

        Dim OutStr As String
        'replace double quotes with single
        OutStr = Replace(InStr, "''", "'")
        OutStr = Replace(OutStr, Chr(34) & Chr(34), Chr(34))
        'html encode it too....
        Return (OutStr)

    End Function

    Public Shared Function JSEncodeQuotes(ByVal InStr As String) As String

        Dim OutStr As String
        OutStr = Replace(InStr, Chr(34), "\" & Chr(34)) 'quote mark'
        OutStr = Replace(OutStr, Chr(39), "\" & Chr(39)) 'single quote'
        Return (OutStr)

    End Function

    Public Shared Function FormatNumber(ByVal thisPage As System.Web.UI.Page) As String
        If thisPage.ClientScript.IsClientScriptBlockRegistered(thisPage.GetType(), "FormatNumber") = False Then
            thisPage.ClientScript.RegisterClientScriptBlock(thisPage.GetType(), "FormatNumber", GetFormatNumberScript())
        End If

        If thisPage.ClientScript.IsClientScriptBlockRegistered(thisPage.GetType(), "FormatClean") = False Then
            thisPage.ClientScript.RegisterClientScriptBlock(thisPage.GetType(), "FormatClean", GetFormatCleanScript())
        End If

        Return "this.value=FormatNumber(this.value)"
    End Function 'FormatNumber

    Public Shared Function FormatCurrency(ByVal thisPage As System.Web.UI.Page) As String
        If thisPage.ClientScript.IsClientScriptBlockRegistered(thisPage.GetType(), "FormatCurrency") = False Then
            thisPage.ClientScript.RegisterClientScriptBlock(thisPage.GetType(), "FormatCurrency", GetFormatCurrencyScript())
        End If

        If thisPage.ClientScript.IsClientScriptBlockRegistered(thisPage.GetType(), "FormatClean") = False Then
            thisPage.ClientScript.RegisterClientScriptBlock(thisPage.GetType(), "FormatClean", GetFormatCleanScript())
        End If

        If thisPage.ClientScript.IsClientScriptBlockRegistered(thisPage.GetType(), "FormatPounds") = False Then
            thisPage.ClientScript.RegisterClientScriptBlock(thisPage.GetType(), "FormatPounds", GetFormatPoundsScript())
        End If

        If thisPage.ClientScript.IsClientScriptBlockRegistered(thisPage.GetType(), "FormatPence") = False Then
            thisPage.ClientScript.RegisterClientScriptBlock(thisPage.GetType(), "FormatPence", GetFormatPenceScript())
        End If

        Return "this.value=FormatCurrency(this.value)"
    End Function 'FormatCurrency

    Private Shared Function GetFormatCurrencyScript() As String
        Dim sScript As String = ""

        sScript = "<script language='JavaScript'>" + vbCrLf + _
        "function FormatCurrency(num)" + vbCrLf + _
        "{" + vbCrLf + _
        "var minus='';" + vbCrLf + _
        "var DecimalDelimiter='.';" + vbCrLf + _
        "var CommaDelimiter=',';" + vbCrLf + _
        "if (num == '') { return '';}" + vbCrLf + _
        "if (num.lastIndexOf(""-"") == 0) { minus='-'; }" + vbCrLf + _
        "if (num.lastIndexOf(DecimalDelimiter) < 0) { num = num + '00'; }" + vbCrLf + _
        "num = FormatClean(num);" + vbCrLf + _
        "sVal = minus + FormatPounds(num,CommaDelimiter) + DecimalDelimiter + FormatPence(num); " + vbCrLf + _
        "return sVal;" + vbCrLf + _
        "}" + vbCrLf + _
        "</script>" + vbCrLf

        Return sScript
    End Function 'GetFormatCurrencyScript

    Private Shared Function GetFormatPoundsScript() As String
        Dim sScript As String = ""

        sScript = "<script language='JavaScript'>" + vbCrLf + _
        "function FormatPounds(amount,CommaDelimiter)" + vbCrLf + _
        "{" + vbCrLf + _
        "try " + vbCrLf + _
        "{" + vbCrLf + _
        "amount = parseInt(amount);" + vbCrLf + _
        "var samount = new String(amount);" + vbCrLf + _
        "if (samount.length < 3) { return 0; }  " + vbCrLf + _
        "samount =  samount.substring(0,samount.length -2);" + vbCrLf + _
        "for (var i = 0; i < Math.floor((samount.length-(1+i))/3); i++)" + vbCrLf + _
        "{" + vbCrLf + _
        "samount = samount.substring(0,samount.length-(4*i+3)) + CommaDelimiter + samount.substring(samount.length-(4*i+3));" + vbCrLf + _
        "}" + vbCrLf + _
        "}" + vbCrLf + _
        "catch (exception) {}" + vbCrLf + _
        "return samount;" + vbCrLf + _
        "}" + vbCrLf + _
        "</script>" + vbCrLf

        Return sScript
    End Function 'GetFormatPoundsScript

    Private Shared Function GetFormatPenceScript() As String
        Dim sScript As String = ""

        sScript = "<script language='JavaScript'>" + vbCrLf + _
        "function FormatPence(amount)" + vbCrLf + _
        "{" + vbCrLf + _
        "var pence = '';" + vbCrLf + _
        "try" + vbCrLf + _
        "{" + vbCrLf + _
        "amount = parseInt(amount);" + vbCrLf + _
        "var samount = new String(amount);" + vbCrLf + _
        "if (samount.length == 0) { return '00'; }" + vbCrLf + _
        "if (samount.length == 1) { return '0' + samount; }" + vbCrLf + _
        "if (samount.length == 2) { return samount; }" + vbCrLf + _
        "pence =  samount.substring(samount.length -2,samount.length);" + vbCrLf + _
        "}" + vbCrLf + _
        "catch (exception) {}" + vbCrLf + _
        "return pence;" + vbCrLf + _
        "}" + vbCrLf + _
        "</script>" + vbCrLf

        Return sScript
    End Function 'GetFormatPenceScript

    Private Shared Function GetFormatNumberScript() As String
        Dim sScript As String = ""

        sScript = "<script language='JavaScript'>" + vbCrLf + _
        "function FormatNumber(num)" + vbCrLf + _
        "{" + vbCrLf + _
        "var sVal='';" + vbCrLf + _
        "var minus='';" + vbCrLf + _
        "var CommaDelimiter=',';" + vbCrLf + _
        "var DecimalDelimiter='.';" + vbCrLf + _
        "var Numlength=0;" + vbCrLf + _
        "var DecimalIndex=0;" + vbCrLf + _
        "try" + vbCrLf + _
        "{" + vbCrLf + _
        "if (num == '') { return '';}" + vbCrLf + _
        "Numlength = num.length;" + vbCrLf + _
        "DecimalIndex = num.indexOf(""."");" + vbCrLf + _
        "if (DecimalIndex != -1)" + vbCrLf + _
        "{" + vbCrLf + _
        "num = num.slice(0, DecimalIndex);" + vbCrLf + _
        "}" + vbCrLf + _
        "if (num.lastIndexOf(""-"") == 0) { minus='-'; }" + vbCrLf + _
        "num = FormatClean(num);" + vbCrLf + _
        "num = parseInt(num);" + vbCrLf + _
        "var samount = new String(num);" + vbCrLf + _
        "for (var i = 0; i < Math.floor((samount.length-(1+i))/3); i++)" + vbCrLf + _
        "{" + vbCrLf + _
        "samount = samount.substring(0,samount.length-(4*i+3)) + CommaDelimiter + samount.substring(samount.length-(4*i+3));" + vbCrLf + _
        "}" + vbCrLf + _
        "}" + vbCrLf + _
        "catch (exception) {}" + vbCrLf + _
        "return minus + samount;" + vbCrLf + _
        "}" + vbCrLf + _
        "</script>" + vbCrLf

        Return sScript
    End Function 'GetFormatNumberScript

    Private Shared Function GetFormatCleanScript() As String
        Dim sScript As String = ""

        sScript = "<script language='JavaScript'>" + vbCrLf + _
        "function FormatClean(num)" + vbCrLf + _
        "{" + vbCrLf + _
        "var sVal='';" + vbCrLf + _
        "var nVal = num.length;" + vbCrLf + _
        "var sChar='';" + vbCrLf + _
        "try" + vbCrLf + _
        "{" + vbCrLf + _
        "for(i=0;i<nVal;i++)" + vbCrLf + _
        "{" + vbCrLf + _
        "sChar = num.charAt(i);" + vbCrLf + _
        "nChar = sChar.charCodeAt(0);" + vbCrLf + _
        "if ((nChar >=48) && (nChar <=57))  { sVal += num.charAt(i);   }" + vbCrLf + _
        "}" + vbCrLf + _
        "}" + vbCrLf + _
        "catch (exception) {}" + vbCrLf + _
        "return sVal;" + vbCrLf + _
        "}" + vbCrLf + _
        "</script>" + vbCrLf

        Return sScript
    End Function 'GetFormatCleanScript

End Class

