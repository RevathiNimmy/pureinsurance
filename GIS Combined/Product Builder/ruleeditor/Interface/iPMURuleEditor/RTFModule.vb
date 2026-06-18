Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Artinsoft.VB6.VB
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Drawing
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Module RTFModule
    ' ************************************************************
    ' * Author           : Ram Chandrabose
    ' * Comments         : RichTextBox Functions
    ' * Purpose          : Used to hold the general variables,
    '                      subs and functions related to
    '                      a) Tag coloring
    '                      b) Printing the Content of Rich Text Box
    '                      c) Find and Highlight
    ' *
    ' ***************************************************************

    Private Declare Function GetDeviceCaps Lib "gdi32" (ByVal hdc As Integer, ByVal nIndex As Integer) As Integer

    Private Declare Function LockWindowUpdate Lib "user32" (ByVal hwndLock As Integer) As Integer
    Private Declare Function SendMessage Lib "user32" Alias "SendMessageA" (ByVal hwnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer

    Private Declare Function SendMessageLong Lib "user32" Alias "SendMessageA" (ByVal hwnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer

    Private Const EM_GETSEL As Integer = &HB0S
    Private Const EM_SETSEL As Integer = &HB1S
    Private Const EM_LINELENGTH As Integer = &HC1S

    Private Const WM_USER As Integer = &H400S
    Private Const EM_FORMATRANGE As Integer = WM_USER + 57

    Private Const PHYSICALOFFSETX As Integer = 112
    Private Const PHYSICALOFFSETY As Integer = 113

    Private Structure RECT
        Dim Left As Integer
        Dim Top As Integer
        Dim Right As Integer
        Dim Bottom As Integer
    End Structure

    Private Structure CharRange
        Dim cpMin As Integer
        Dim cpMax As Integer
    End Structure

    Private Structure FormatRange
        Dim hdc As Integer
        Dim hdcTarget As Integer
        Dim rc As RECT
        Dim rcPage As RECT
        Dim chrg As CharRange
        Public Shared Function CreateInstance() As FormatRange
            Dim result As New FormatRange
            Return result
        End Function
    End Structure

    Private Structure POINTAPI
        Dim x As Integer
        Dim Y As Integer
    End Structure

    Public Const EM_CHARFROMPOS As Integer = &HD7S
    Public Const EM_GETFIRSTVISIBLELINE As Integer = &HCES '(0&,pt)
    Public Const EM_FMTLINES As Integer = &HC8S
    Public Const EM_GETLINE As Integer = &HC4S '(line num,pt)=len
    Public Const EM_GETLINECOUNT As Integer = &HBAS
    Public Const EM_LINEINDEX As Integer = &HBBS '(char num,0&)
    Public Const EM_LINEFROMCHAR As Integer = &HC9S '(char num,0&)

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "RTFModule"


    '********************************************************************
    ' * Author      :   Ram Chandrabose
    ' * Comments    :   Findit takes three arguments
    '                   Two required  - a) Box   - A RichTextBox Object
    '                                   b) scch  - String to Search
    '                   Two optional  - a) Start - Long Integer
    '                                   b) HighlighColor - Long Integer
    '                                      (Color Code for Hightlight Color)
    '********************************************************************
    Public Function FindIt(ByRef Box As RichTextBox, ByRef Srch As String, Optional ByRef Start As Integer = 0, Optional ByRef HighLightColor As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim retval As Integer 'Instr returns a long
        Dim Source As String = "" 'variable used in Instr

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Source = Box.Text.ToUpper() 'put the text to search into the variable
            Srch = Srch.ToUpper()

            Box.Enabled = False

            If Start = 0 Then Start = 1 'the initial call doesn't pass a value
            'for Start, so it will equal 0

            retval = Strings.InStr(Start, Source, Srch) 'do the first search,
            'starting at the beginning
            'of the text

            If retval <> 0 Then 'there is at least one more occurrence of
                'the string

                'the RichTextBox doesn't support multiple active selections, so
                'this section marks the occurrences of the search string by
                'making them Bold and Red
                ' For Rich Text Box

                With Box
                    .SelectionStart = retval - 1
                    .SelectionLength = Srch.Length
                    .ScrollToCaret()
                    .SelectionColor = ColorTranslator.FromOle(HighLightColor)

                    .SelectionFont = VB6.FontChangeBold(.SelectionFont, True)
                    .SelectionLength = 0 'this line removes the selection
                    'highlight
                End With

                Start = retval + Srch.Length 'move the starting point past the
                'first occurrence

                'FindIt calls itself with new arguments
                'this is what makes it Recursive
                result = 1 + FindIt(Box, Srch, Start, HighLightColor)
            End If

            Box.Enabled = True
            'Box.SelStart = 1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FindIt Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindIt", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'Public subs.

    Public Function GetFirstLinePos(ByRef Line As Integer, ByRef Start As Integer, ByRef rtf As RichTextBox) As Integer
        Dim result As Integer = 0
        Dim c As Integer

        For i As Integer = Start To 0 Step -1
            Dim handle As GCHandle = GCHandle.Alloc(0, GCHandleType.Pinned)
            Try
                Dim tmpPtr As IntPtr = handle.AddrOfPinnedObject()
                c = SendMessage(rtf.Handle.ToInt32(), EM_LINEFROMCHAR, i, tmpPtr)
            Finally
                handle.Free()
            End Try

            If c < Line Then
                result = i + 1

                Exit For
            ElseIf i = 0 Then
                result = 0
            End If
        Next i

        Return result
    End Function

    Public Function GetLastLinePos(ByRef Line As Integer, ByRef Start As Integer, ByRef rtf As RichTextBox) As Integer
        Dim result As Integer = 0
        Dim c As Integer

        For i As Integer = Start To Strings.Len(rtf.Text)
            Dim handle As GCHandle = GCHandle.Alloc(0, GCHandleType.Pinned)
            Try
                Dim tmpPtr As IntPtr = handle.AddrOfPinnedObject()
                c = SendMessage(rtf.Handle.ToInt32(), EM_LINEFROMCHAR, i, tmpPtr)
            Finally
                handle.Free()
            End Try

            If c > Line Then
                result = i - 1

                Exit For
            ElseIf i = Strings.Len(rtf.Text) Then
                result = Strings.Len(rtf.Text)
            End If
        Next i

        Return result
    End Function

    'Public Sub ColorTags(ByRef iStart As Integer, ByRef iEnd As Integer, ByRef rtf As RichTextBox, Optional ByRef Color As Integer = ColorTranslator.ToOle(System.Drawing.Color.Magenta), Optional ByRef ErrorColor As Integer = ColorTranslator.ToOle(System.Drawing.Color.Red), Optional ByRef CommentColor As Integer = &H8000, Optional ByRef ParamColor As Integer = &H800080, Optional ByRef IncludeColor As Integer = &H40C0)
    Public Sub ColorTags(ByRef iStart As Integer, ByRef iEnd As Integer, ByRef rtf As RichTextBox, Optional ByRef Color As Object = Nothing, Optional ByRef ErrorColor As Object = Nothing, Optional ByRef CommentColor As Integer = &H8000, Optional ByRef ParamColor As Integer = &H800080, Optional ByRef IncludeColor As Integer = &H40C0)


        Try

            Dim t As Integer

            'Turn refreshing off.
            Dim tmp As Integer = LockWindowUpdate(rtf.Handle.ToInt32())

            Dim OldStart As Integer = iStart

            rtf.SelectionStart = iStart
            rtf.SelectionLength = iEnd - iStart
            rtf.SelectionColor = System.Drawing.Color.Black
            rtf.SelectionLength = 0

            iStart = Strings.InStr(iStart + 1, rtf.Text, "<")

            If iStart > 0 Then rtf.SelectionStart = iStart - 1

            Dim iFirst As Integer = iStart - 1
            'iFirst = InStr(iFirst + 1, rtf.Text, "<") - 1

            Dim c As Integer = OldStart + 1
            Dim i As Integer = Strings.InStr(OldStart + 1, rtf.Text, "<")

            Do
                c = Strings.InStr(c + 1, rtf.Text, ">")

                If c < i Then
                    rtf.SelectionStart = c - 1
                    rtf.SelectionLength = 1

                    rtf.SelectionColor = ColorTranslator.FromOle(ErrorColor)

                    If iStart > 0 Then
                        rtf.SelectionStart = iStart - 1
                    Else
                        rtf.SelectionStart = 0
                    End If
                    rtf.SelectionLength = 0

                Else
                    Exit Do
                End If
            Loop


            Dim iLast As Integer = Strings.InStr(iFirst + 1, rtf.Text, ">")

            i = iLast
            c = iLast
            Do
                'A ">" without "<".
                i = Strings.InStr(i + 1, rtf.Text, "<")
                c = Strings.InStr(c + 1, rtf.Text, ">")

                If (c < i And c > 0) And (c < iEnd) Or (c > 0 And i = 0) And (c < iEnd) Then
                    rtf.SelectionStart = c - 1
                    rtf.SelectionLength = 1
                    rtf.SelectionColor = ColorTranslator.FromOle(ErrorColor)
                    rtf.SelectionLength = 0

                    rtf.SelectionStart = iFirst
                End If

                If i = 0 Then
                    i = iLast
                End If
            Loop Until (c = 0)


            i = 0
            c = 0

            Do Until iFirst = -1
                iLast = Strings.InStr(iFirst + 1, rtf.Text, ">")
                rtf.SelectionStart = iFirst

                'A "<" without ">"
                tmp = Strings.InStr(iFirst + 2, rtf.Text, "<")

                If tmp < iLast And tmp > 0 Or iLast = 0 Then
                    If tmp = 0 Then
                        rtf.SelectionLength = Strings.Len(rtf.Text)
                    Else
                        rtf.SelectionLength = tmp - iFirst - 1
                    End If

                    rtf.SelectionColor = ColorTranslator.FromOle(ErrorColor)
                Else
                    rtf.SelectionLength = iLast - iFirst

                    If Mid(rtf.Text, iFirst + 1, 4) = "<!--" And Mid(rtf.Text, iLast - 2, 3) = "-->" Then

                        tmp = Strings.InStr(iFirst, rtf.Text, "#include", CompareMethod.Text)

                        If tmp > 0 Then
                            If Mid(rtf.Text, iFirst + 5, tmp - iFirst - 5).Trim() = "" Then
                                rtf.SelectionColor = ColorTranslator.FromOle(IncludeColor)
                            Else
                                rtf.SelectionColor = ColorTranslator.FromOle(CommentColor)
                            End If
                        Else
                            rtf.SelectionColor = ColorTranslator.FromOle(CommentColor)
                        End If
                    Else
                        rtf.SelectionColor = ColorTranslator.FromOle(Color)
                    End If

                    'Color the parameters.
                    t = iFirst

                    Do
                        tmp = Strings.InStr(t + 1, rtf.Text, "=")

                        If tmp > 0 And tmp < iLast Then
                            For i = tmp + 1 To iLast
                                If Mid(rtf.Text, i, 1) <> " " And Mid(rtf.Text, i, 1) <> Strings.Chr(13) And Mid(rtf.Text, i, 1) <> Constants.vbLf Then
                                    Exit For
                                End If
                            Next i

                            If i >= iLast Then
                                'A '=' without a parameter.
                                rtf.SelectionStart = tmp - 1
                                rtf.SelectionLength = 1
                                rtf.SelectionColor = ColorTranslator.FromOle(ErrorColor)
                                Exit Do
                            End If

                            For c = i + 1 To iLast
                                If Mid(rtf.Text, c, 1) = """" And Mid(rtf.Text, i, 1) = """" Then
                                    Exit For
                                ElseIf Mid(rtf.Text, c, 1) = " " And Mid(rtf.Text, i, 1) <> """" Or Mid(rtf.Text, c, 1) = Strings.Chr(13) And Mid(rtf.Text, i, 1) <> """" Or Mid(rtf.Text, c, 1) = Constants.vbLf And Mid(rtf.Text, i, 1) <> """" Then
                                    Exit For
                                End If
                            Next c

                            If c >= iLast And Mid(rtf.Text, i, 1) = """" Then
                                'A parameter starting with
                                ''"' and doesn't end with one.

                                rtf.SelectionStart = i - 1
                                rtf.SelectionLength = iLast - i
                                rtf.SelectionColor = ColorTranslator.FromOle(ErrorColor)

                                Exit Do
                            End If

                            'Color the parameter.
                            rtf.SelectionStart = i - 1
                            rtf.SelectionLength = c - i + 1

                            If ColorTranslator.ToOle(rtf.SelectionColor) = CommentColor Then
                                Exit Do
                            End If

                            rtf.SelectionColor = ColorTranslator.FromOle(ParamColor)

                            t = tmp + 1
                        Else
                            Exit Do
                        End If
                    Loop
                End If

                iFirst = rtf.Find("<", iFirst + 1, RichTextBoxFinds.NoHighlight)

                If iFirst > iEnd Then Exit Do
            Loop
            rtf.SelectionStart = iStart


            'Allow repainting (Refreshing).
            tmp = LockWindowUpdate(0)

            rtf.SelectionStart = OldStart

        Catch exc As System.Exception
            NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try
    End Sub

    Public Sub AddColors(ByRef RTFBox As RichTextBox, Optional ByRef iChangeOptions As Integer = 0, Optional ByRef HighLightColor As Integer = 0)
        Dim PMComment, PMRule As Integer

        Try

            ' Add Parameters For Future Use
            ' Box As RichTextBox, PMComment As Long, Optional HighLightColor As Long

            Dim retval As Integer 'Instr returns a long
            Dim Source As String = "" 'variable used in Instr
            ' Now only Comments
            Dim lTextLen, tmp As Integer

            Dim sTempStr As String = ""
            Dim lTempLoc As Integer

            Dim lOldStart As Integer

            Dim strStart, StrEnd As String
            Dim Start As Integer

            If False Then
                iChangeOptions = PMComment
            End If

            If False Then
                HighLightColor = ColorTranslator.ToOle(Color.Lime)
            End If

            lOldStart = RTFBox.SelectionStart

            'Turn refreshing off.
            tmp = LockWindowUpdate(RTFBox.Handle.ToInt32())

            Source = RTFBox.Text

            RTFBox.Enabled = False

            RTFBox.SelectionStart = 1
            RTFBox.SelectionLength = Source.Length
            RTFBox.SelectionColor = Color.Black
            RTFBox.SelectionFont = VB6.FontChangeBold(RTFBox.SelectionFont, False)
            RTFBox.SelectionLength = 0

            RTFBox.SelectionStart = 1


            Select Case iChangeOptions
                Case PMComment

                    strStart = "'"
                    StrEnd = Strings.Chr(13) & Strings.Chr(10)

                Case PMRule

            End Select

            'put the text to search into the variable
            If Start = 0 Then Start = 1 'the initial call doesn't pass a value
            'for Start, so it will equal 0
            lTextLen = Source.Length
            Do While Start <= lTextLen

                retval = Strings.InStr(Start, Source, strStart) 'do the first search,
                'starting at the beginning
                'of the text
                If retval > 0 Then 'there is at least one more occurrence of
                    'the string
                    ' Check for the end of line using (vbcrlf)
                    lTempLoc = Strings.InStr(retval, Source, Strings.Chr(13) & Strings.Chr(10))

                    ' it means the the file is about to end
                    If lTempLoc = 0 Then
                        lTempLoc = lTextLen + 1
                    End If

                    If lTempLoc > 0 Then
                        With RTFBox
                            .SelectionStart = retval - 1
                            .SelectionLength = lTempLoc - retval

                            .SelectionColor = ColorTranslator.FromOle(HighLightColor)
                            .SelectionLength = 0 'this line removes the selection highlight
                        End With

                        Start = lTempLoc + 1 'move the starting point past the
                        'first occurrence
                    ElseIf lTempLoc < 0 Then  ' Some error
                        Exit Do

                    End If
                ElseIf retval <= 0 Then
                    Exit Do
                End If

            Loop

            'Allow repainting (Refreshing).
            tmp = LockWindowUpdate(0)

            RTFBox.SelectionStart = lOldStart
            RTFBox.Enabled = True

        Catch excep As System.Exception



            MessageBox.Show(CStr(Information.Err().Number) & "-" & excep.Message, "Add Colors", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub


    Public Function InStrReverse(ByRef Start As Integer, ByRef SearchIn As String, ByRef SearchFor As String, Optional ByRef MatchCase As Boolean = False) As Integer

        Dim result As Integer = 0

        For i As Integer = Start To 1 Step -1
            If (Mid(SearchIn, i, SearchFor.Length).ToUpper() = SearchFor.ToUpper()) And Not MatchCase Then
                'Found match.
                result = i

                Exit For
            ElseIf (Mid(SearchIn, i, SearchFor.Length) = SearchFor) And MatchCase Then
                'Found match.
                result = i

                Exit For
            End If
        Next i

        Return result
    End Function


    Public Function PrintRTF(ByRef rtf As RichTextBox, Optional ByRef nnLeftMarginWidth As Integer = 0, Optional ByRef nnTopMarginHeight As Integer = 0, Optional ByRef nnRightMarginWidth As Integer = 0, Optional ByRef nnBottomMarginHeight As Integer = 0) As Boolean

        ' ************************************************
        ' * Module Name      : RTFModule
        ' * Module Filename  : RTFModule
        ' * Procedure Name   : PrintRTF
        ' * Parameters       :
        ' *                    rtf As RichTextBox
        ' *                    nnLeftMarginWidth As Long        in twips
        ' *                    nnTopMarginHeight As Long        in twips
        ' *                    nnRightMarginWidth As Long       in twips
        ' *                    nnBottomMarginHeight As Long     in twips
        ' ***************************************************************
        ' * Comments         :
        ' * Author           : Ram Chandrabose
        ' * Reference        : HOWTO: Set Up the RichTextBox Control for
        '                      WYSIWYG Printing
        '                      Article Id: Q146022
        ' *************************************************************
        Try

            Dim nLeftOffset, nTopOffset, nLeftMargin, nTopMargin, nRightMargin, nBottomMargin As Integer
            Dim fr As FormatRange = FormatRange.CreateInstance()
            Dim rcDrawTo As New RECT
            Dim rcPage As New RECT
            Dim nTextLength, nNextCharPos, nRet As Integer

            If False Then
                nnLeftMarginWidth = 100
            End If

            If False Then
                nnLeftMarginWidth = 100
            End If

            If False Then
                nnLeftMarginWidth = 100
            End If

            If False Then
                nnLeftMarginWidth = 100
            End If

            PrinterHelper.Printer.Print(New String(" "c, 1))

            'Developer Guide No. 217
            PrinterHelper.Printer.ScaleMode = 1


            'Developer Guide No. 217
            nLeftOffset = CInt(PrinterHelper.Printer.ScaleX(GetDeviceCaps(PrinterHelper.Printer.hDC, PHYSICALOFFSETX), 3, 1))



            'Developer Guide No. 217
            nTopOffset = CInt(PrinterHelper.Printer.ScaleY(GetDeviceCaps(PrinterHelper.Printer.hDC, PHYSICALOFFSETY), 3, 1))

            nLeftMargin = nnLeftMarginWidth - nLeftOffset
            nTopMargin = nnTopMarginHeight - nTopOffset
            nRightMargin = (PrinterHelper.Printer.Width - nnRightMarginWidth) - nLeftOffset

            nBottomMargin = (PrinterHelper.Printer.Height - nnBottomMarginHeight) - nTopOffset

            rcPage.Left = 0
            rcPage.Top = 0

            rcDrawTo.Left = nLeftMargin
            rcDrawTo.Top = nTopMargin
            rcDrawTo.Right = nRightMargin
            rcDrawTo.Bottom = nBottomMargin
            fr.hdc = PrinterHelper.Printer.hDC
            fr.hdcTarget = PrinterHelper.Printer.hDC
            fr.rc = rcDrawTo
            fr.rcPage = rcPage
            fr.chrg.cpMin = 0
            fr.chrg.cpMax = -1
            nTextLength = Strings.Len(rtf.Text)

            Do
                fr.hdc = PrinterHelper.Printer.hDC
                fr.hdcTarget = PrinterHelper.Printer.hDC

                ' Print the page by sending EM_FORMATRANGE message

                Dim handle As GCHandle = GCHandle.Alloc(fr, GCHandleType.Pinned)
                Try
                    Dim tmpPtr As IntPtr = handle.AddrOfPinnedObject()

                    nNextCharPos = SendMessage(rtf.Handle.ToInt32(), EM_FORMATRANGE, True, tmpPtr)
                Finally
                    handle.Free()
                End Try
                If nNextCharPos >= nTextLength Then Exit Do
                fr.chrg.cpMin = nNextCharPos
                PrinterHelper.Printer.NewPage()
                PrinterHelper.Printer.Print(New String(" "c, 1))
            Loop

            PrinterHelper.Printer.EndDoc()
            Dim handle2 As GCHandle = GCHandle.Alloc(0, GCHandleType.Pinned)
            Try
                Dim tmpPtr2 As IntPtr = handle2.AddrOfPinnedObject()
                nRet = SendMessage(rtf.Handle.ToInt32(), EM_FORMATRANGE, False, tmpPtr2)
            Finally
                handle2.Free()
            End Try


            Return True

        Catch

            Return False
        End Try
    End Function


    '********************************************************************
    ' * Author      :   Ram Chandrabose
    ' * Comments    :   GetLineColNo takes three arguments
    '                   a) Box      - A RichTextBox Object
    '                   b) lLineNo  - Line No   (Long Integer & Output)
    '                   c) lColNo   - Column No (Long Integer & Output)
    '
    '********************************************************************
    Public Function GetLineColNo(ByRef Box As RichTextBox, ByRef lLineNo As Integer, ByRef lColNo As Integer) As Object


        Try

            'Total number of lines in the Rich Text Box (Including Blank Line if Any
            Dim lineCount As Integer = SendMessageLong(Box.Handle.ToInt32(), EM_GETLINECOUNT, 0, 0)

            'Total No of Cursor position in the Rich Text box (ie. Total No of Characters
            'This Includes any CR & LF Characters
            ' Since this is Zero Base (if you want to return then ADD 1 to it)
            Dim overallCursorPos As Integer = SendMessageLong(Box.Handle.ToInt32(), EM_GETSEL, 0, 0) \ &H10000


            'Current Line No (Note: zero-based)
            Dim currLinePos As Integer = SendMessageLong(Box.Handle.ToInt32(), EM_LINEFROMCHAR, overallCursorPos, 0)
            currLinePos = (currLinePos + 1)

            ' Return the Value Back
            lLineNo = currLinePos

            'Number of Cursor Positions (ie. No of Characters) upto but before start of the current line
            '(Includes CR and LF if any)
            Dim chrsBeforeCurrLine As Integer = SendMessageLong(Box.Handle.ToInt32(), EM_LINEINDEX, currLinePos, 0)


            'Current Cursor Position in terms of current line only (Note: zero-based)
            Dim CurrLineCursorPos As Integer = overallCursorPos - chrsBeforeCurrLine
            CurrLineCursorPos += 1

            lColNo = CurrLineCursorPos

            'number of characters in current line (Note: with EM_LINELENGTH, the value of the "wParam"
            'parameter specifies the char index of a char in the line. If this parameter is -1, the
            'function returns number of unselected characters on line. "lParam" not used and set to 0).
            Dim currLineLen As Integer = SendMessageLong(Box.Handle.ToInt32(), EM_LINELENGTH, -1, 0)

        Catch exc As System.Exception
            NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try

    End Function
End Module
