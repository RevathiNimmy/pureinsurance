Option Strict Off
Option Explicit On
Imports System.Text
'developer guide no. 129
Imports SSP.Shared

Module ACTAmountInWords
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    Const ACClass As String = "AmountInWords"


    '**********************************************************
    ' Functions to construct a language independent string
    ' of amount in words together with currency name
    '
    '**********************************************************

    Public Function AmountInWords(ByVal v_vdAmount As Double, ByRef r_sMajor As String, ByRef r_sMinor As String) As Integer

        Dim result As Integer = 0

        Dim vdDivisor(4) As Object ' 10000s, 1000s, 100s etc
        Dim sDivisorName(4) As String
        Dim sUnitName(99) As String
        Dim vdAmount As Double

        Dim vdUnits As Double
        Dim sWords As New StringBuilder

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            sWords = New StringBuilder("")

            vdAmount = Math.Floor(Math.Abs(v_vdAmount))

            SetUpConstants(vdDivisor, sDivisorName, sUnitName)

            r_sMinor = sUnitName(CInt(Math.Floor((v_vdAmount - vdAmount) * 100))).Trim()

            If vdAmount < 100 Then
                r_sMajor = sUnitName(CInt(vdAmount))
                Return result
            End If

            For i As Integer = vdDivisor.GetUpperBound(0) To 0 Step -1

                vdUnits = Math.Floor(vdAmount / CDbl(vdDivisor(i)))

                vdAmount -= vdUnits * CDbl(vdDivisor(i))
                If vdUnits > 0 Then
                    If i = 0 And sWords.ToString().Length > 0 Then
                        sWords.Append("and ")
                    End If

                    AmountInWords(vdUnits, r_sMajor, "")
                    sWords.Append(r_sMajor & sDivisorName(i))
                End If
            Next i

            r_sMajor = sWords.ToString().Trim()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage("", iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="Failed to construct amount in words", vApp:=ACApp, vClass:=ACClass, vMethod:="AmountInWords", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub SetUpConstants(ByRef r_vdDivisor() As Object, ByRef r_sDivisorName() As String, ByRef r_sUnitName() As String)

        Dim vdOne As Decimal = 1
        Dim vdTen As Decimal = 10
        Dim vdHundred As Decimal = 100
        Dim vdThousand As Decimal = 1000
        Dim vdMillion As Decimal = 1000000

        ' Set up the named units above 100

        r_vdDivisor(0) = vdOne
        r_sDivisorName(0) = ""

        r_vdDivisor(1) = vdHundred
        r_sDivisorName(1) = "hundred "

        r_vdDivisor(2) = vdThousand
        r_sDivisorName(2) = "thousand "

        r_vdDivisor(3) = vdMillion
        r_sDivisorName(3) = "million "

        r_vdDivisor(4) = vdThousand * vdMillion
        r_sDivisorName(4) = "billion "

        r_sUnitName(0) = "zero "
        r_sUnitName(1) = "one "
        r_sUnitName(2) = "two "
        r_sUnitName(3) = "three "
        r_sUnitName(4) = "four "
        r_sUnitName(5) = "five "
        r_sUnitName(6) = "six "
        r_sUnitName(7) = "seven "
        r_sUnitName(8) = "eight "
        r_sUnitName(9) = "nine "
        r_sUnitName(10) = "ten "
        r_sUnitName(11) = "eleven "
        r_sUnitName(12) = "twelve "
        r_sUnitName(13) = "thirteen "
        r_sUnitName(14) = "fourteen "
        r_sUnitName(15) = "fiveteen "
        r_sUnitName(16) = "sixteen "
        r_sUnitName(17) = "seventeen "
        r_sUnitName(18) = "eighteen "
        r_sUnitName(19) = "nineteen "
        r_sUnitName(20) = "twenty "
        r_sUnitName(21) = "twenty one "
        r_sUnitName(22) = "twenty two "
        r_sUnitName(23) = "twenty three "
        r_sUnitName(24) = "twenty four "
        r_sUnitName(25) = "twenty five "
        r_sUnitName(26) = "twenty six "
        r_sUnitName(27) = "twenty seven "
        r_sUnitName(28) = "twenty eight "
        r_sUnitName(29) = "twenty nine "
        r_sUnitName(30) = "thirty "
        r_sUnitName(31) = "thirty one "
        r_sUnitName(32) = "thirty two "
        r_sUnitName(33) = "thirty three "
        r_sUnitName(34) = "thirty four "
        r_sUnitName(35) = "thirty five "
        r_sUnitName(36) = "thirty six "
        r_sUnitName(37) = "thirty seven "
        r_sUnitName(38) = "thirty eight "
        r_sUnitName(39) = "thirty nine "
        r_sUnitName(40) = "forty "
        r_sUnitName(41) = "forty one "
        r_sUnitName(42) = "forty two "
        r_sUnitName(43) = "forty three "
        r_sUnitName(44) = "forty four "
        r_sUnitName(45) = "forty five "
        r_sUnitName(46) = "forty six "
        r_sUnitName(47) = "forty seven "
        r_sUnitName(48) = "forty eight "
        r_sUnitName(49) = "forty nine "
        r_sUnitName(50) = "fifty "
        r_sUnitName(51) = "fifty one "
        r_sUnitName(52) = "fifty two "
        r_sUnitName(53) = "fifty three "
        r_sUnitName(54) = "fifty four "
        r_sUnitName(55) = "fifty five "
        r_sUnitName(56) = "fifty six "
        r_sUnitName(57) = "fifty seven "
        r_sUnitName(58) = "fifty eight "
        r_sUnitName(59) = "fifty nine "
        r_sUnitName(60) = "sixty "
        r_sUnitName(61) = "sixty one "
        r_sUnitName(62) = "sixty two "
        r_sUnitName(63) = "sixty three "
        r_sUnitName(64) = "sixty four "
        r_sUnitName(65) = "sixty five "
        r_sUnitName(66) = "sixty six "
        r_sUnitName(67) = "sixty seven "
        r_sUnitName(68) = "sixty eight "
        r_sUnitName(69) = "sixty nine "
        r_sUnitName(70) = "seventy "
        r_sUnitName(71) = "seventy one "
        r_sUnitName(72) = "seventy two "
        r_sUnitName(73) = "seventy three "
        r_sUnitName(74) = "seventy four "
        r_sUnitName(75) = "seventy five "
        r_sUnitName(76) = "seventy six "
        r_sUnitName(77) = "seventy seven "
        r_sUnitName(78) = "seventy eight "
        r_sUnitName(79) = "seventy nine "
        r_sUnitName(80) = "eighty "
        r_sUnitName(81) = "eighty one "
        r_sUnitName(82) = "eighty two "
        r_sUnitName(83) = "eighty three "
        r_sUnitName(84) = "eighty four "
        r_sUnitName(85) = "eighty five "
        r_sUnitName(86) = "eighty six "
        r_sUnitName(87) = "eighty seven "
        r_sUnitName(88) = "eighty eight "
        r_sUnitName(89) = "eighty nine "
        r_sUnitName(90) = "ninety "
        r_sUnitName(91) = "ninety one "
        r_sUnitName(92) = "ninety two "
        r_sUnitName(93) = "ninety three "
        r_sUnitName(94) = "ninety four "
        r_sUnitName(95) = "ninety five "
        r_sUnitName(96) = "ninety six "
        r_sUnitName(97) = "ninety seven "
        r_sUnitName(98) = "ninety eight "
        r_sUnitName(99) = "ninety nine "

    End Sub
    'developer guide no. 29(No Solutions)
    Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Module
