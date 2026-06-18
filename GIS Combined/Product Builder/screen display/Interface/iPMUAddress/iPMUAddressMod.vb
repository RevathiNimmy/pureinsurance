Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Text
'Developer Guide No. 129
Imports SharedFiles
Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date: 02/07/1998
    '
    ' Description: Main module containing public variable/constants.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "iPMUAddress"


    ' Public interface constants used when
    ' retrieving data from the resource file.

    ' {* USER DEFINED CODE (Begin) *}

    ' General Icons


    ' Form
    Public Const ACInterfaceTitle As Integer = 100
    Public Const ACTabTitle1 As Integer = 101

    ' Buttons
    Public Const ACOKButton As Integer = 200
    Public Const ACCancelButton As Integer = 201
    Public Const ACHelpButton As Integer = 202
    Public Const ACNavigateButton As Integer = 203

    ' Messages
    Public Const ACCancelDetailsTitle As Integer = 300
    Public Const ACCancelDetails As Integer = 301
    Public Const ACBusinessFailTitle As Integer = 302
    Public Const ACBusinessFail As Integer = 303

    ' Menus


    ' {* USER DEFINED CODE (End) *}

    ' Public contants used for the start
    ' and end control indexes.
    Public Const ACControlStart As Integer = 0
    Public Const ACControlEnd As Integer = 1

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' Public source and language ID's from the
    ' Object Manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iCountryID As Integer 'RWH(15/09/2000) RSAIB Process 06.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_sCountryCode As String = "" 'eck030101
    ' Public instance of the object manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager


    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

    Public Const ScreenHelpID As Integer = 22

    'RWH(09/06/2000)
    Public g_oGIS As Object


    ' ************************************************************************ '
    ' Name: FormatPostCode
    '
    ' Description: Formats the post code passed in.
    '              Example...   A12 3BC     -> A12 3BC
    '                           A 12 3BC    -> A12 3BC
    '                           A 1 2 3BC   -> A12 3BC
    '               Basically it removes all spaces, except for the last one.
    '
    ' CTAF 200300 - Moved from frmInterface of iPMBAddress into PMBGeneralFunc.
    '
    ' ************************************************************************ '
    Public Function FormatPostCode(ByVal v_sInString As String, ByRef r_sOutString As String) As Integer

        Dim result As Integer = 0
        Dim iSpaceCounter, iCurrentSpaces As Integer
        'Developer Guide No. 128
        Dim sChar As Char

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            iSpaceCounter = 0
            iCurrentSpaces = 0

            r_sOutString = ""

            ' find out how many spaces are in the "in" string
            For iLoop1 As Integer = 1 To v_sInString.Length
                If v_sInString.Substring(iLoop1 - 1, 1) = " " Then
                    iSpaceCounter += 1
                End If
            Next iLoop1

            ' if theres just one space, then that's ok
            If iSpaceCounter = 1 Then
                r_sOutString = v_sInString
                Return result
            End If

            ' loop through the in string and
            'Developer Guide No. 128
            For Each sChar In v_sInString
                ' if its a space, then check if
                'Developer Guide No. 128
                If sChar = " " Then
                    iCurrentSpaces += 1
                    ' we arent at the last space
                    If iCurrentSpaces >= iSpaceCounter Then
                        ' if we are, then its ok to add this space
                        'Developer Guide No. 128
                        r_sOutString = r_sOutString & sChar
                    End If
                Else
                    ' not a space so just add it
                    'Developer Guide No. 128
                    r_sOutString = r_sOutString & sChar
                End If
                'Developer Guide No. 128
                'Next sChar.Value
            Next sChar

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to format the Postcode : " & v_sInString, vApp:=ACApp, vClass:=ACClass, vMethod:="FormatPostCode", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CheckValidPostCode
    '
    ' Description: Checks to see if a postcode is in one of the following
    '              formats :
    '                           X9 9XX
    '                           X99 9XX
    '                           XX9 9XX
    '                           XX99 9XX
    '                           XX9X 9XX
    '                           X9X 9XX
    '
    ' History: 20/03/2000 CTAF - Created.
    '          17/10/2000 CTAF - Added X9X 9XX
    '
    ' Notes: This is different to ValidatePostcodeFormat in GIIFunc.bas
    '        as it takes a string input.
    '
    ' ***************************************************************** '
    Public Function CheckValidPostCode(ByVal v_sPostCode As String, Optional ByVal v_bSpaceRequired As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim iLen As Integer
        Dim sPostCode As String = ""
        Dim sChar As New FixedLengthString(1)
        Dim sNewPostCode As New StringBuilder

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Store the post code, so we can modify it later on
            sPostCode = v_sPostCode

            ' Check for a minimum length
            iLen = sPostCode.Length

            If iLen < 5 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If v_bSpaceRequired Then
                If (sPostCode.IndexOf(" "c) + 1) = 0 Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            sNewPostCode = New StringBuilder("")

            For iLoop1 As Integer = 1 To iLen

                ' Grab the next letter
                sChar.Value = sPostCode.Substring(iLoop1 - 1, 1)

                If sChar.Value.ToUpper() Like "[A-Z]" Then
                    ' Convert any letters to X
                    sNewPostCode.Append("X")
                ElseIf (sChar.Value Like "[0-9]") Then
                    ' Convert any numbers
                    sNewPostCode.Append("9")
                ElseIf (sChar.Value = " ") Then
                    ' Do nothing
                Else
                    ' Leave anything else as it is
                    sNewPostCode.Append(sChar.Value)
                End If

            Next iLoop1


            Select Case sNewPostCode.ToString()
                Case "X99XX", "X999XX", "XX99XX", "XX999XX", "XX9X9XX", "X9X9XX"
                    ' Return True
                    Return gPMConstants.PMEReturnCode.PMTrue
                Case Else
                    ' Return False
                    Return gPMConstants.PMEReturnCode.PMFalse
            End Select

        Catch
        End Try



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckValidPostCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckValidPostCode", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function
End Module