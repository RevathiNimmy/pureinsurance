Attribute VB_Name = "MGeneratePassword"
' Module:   Password generator for use by setupdb
' Shared:   Yes
'
' THE VALUE OF THIS CODE DEPENDS ON THE ALGORITHM BEING KEPT
' SECRET. DO NOT SHARE THIS CODE INTO ANY PROJECTS OTHER THAN
' spgen AND setupdb!
'
Option Explicit

Public Const knPasswordAlgorithmVersion = 1

Public Function GeneratePassword(ByVal nSeed As Long) As Long

    Const kdMaxInt As Double = 2147483647
    Const kdBaseDate As Double = #1/1/2000#

    Dim dSeed As Double
    Dim dDate As Double
    Dim dTime As Double
    Dim dPassword As Double

    ' Make sure the seed value is positive.
    dSeed = Abs(CDbl(nSeed)) + 3

    ' Get the date as a number.
    dDate = Abs(CDbl(Date) - kdBaseDate) + 3

    ' Get the time as a number rounded to the nearest quarter-hour.
    dTime = Abs(Int(CDbl(Time) * 96# + 0.5)) + 3

    ' Throw all the values together and see what comes out.
    dPassword = dDate * dDate * dTime * dTime * dSeed * dSeed

    ' Quickly cut down the exponent if it's too high.
    dPassword = CDbl(Left$(Format$(dPassword, "#"), 15))

    ' Take the modulus of the value to make sure it fits into
    ' the correct range for a long integer.
    GeneratePassword = Int(dPassword - (Int(dPassword / kdMaxInt) * kdMaxInt))

End Function
