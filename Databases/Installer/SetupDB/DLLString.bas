Attribute VB_Name = "MDLLString"
' Module:   String functions
' Shared:   Yes (RESTRICTED)
' Needs:    Nothing
'
' THIS CODE IMPLEMENTS CORRESPONDING FUNCTIONS IN THE DLL.
' IT IS SHARED *ONLY* TO SUPPORT SMALL UTILITIES THAT
' CANNOT REFERENCE THE DLL. *DO NOT* ALTER THIS CODE IN ANY
' WAY UNLESS YOU ARE CHANGING THE INTERNALS OF THE DLL.
'
Option Explicit

' OBSOLETE - Use the CStringBuilder class for this sort of thing.
Public Function BuildSQLWithinTransactions(ByRef r_sBaseSQL As String, ByVal sNewSQL As String) As String

    If sNewSQL <> "" Then
        If r_sBaseSQL = "" Then
            r_sBaseSQL = sNewSQL
        Else
            r_sBaseSQL = r_sBaseSQL & "  " & vbCrLf & sNewSQL
        End If
    End If

End Function

' Left justify a string within the specified length.
Public Function AlignLeft(ByVal sText As String, _
    ByVal nLength As Long, _
    Optional ByVal sPaddingChar As String = " ") As String

    AlignLeft = Left$(Trim$(sText) & String$(nLength, sPaddingChar), nLength)

End Function

' Right justify a string within the specified length.
Public Function AlignRight(ByVal sText As String, _
    ByVal nLength As Long, _
    Optional ByVal sPaddingChar As String = " ") As String

    AlignRight = Right$(String$(nLength, sPaddingChar) & Trim$(sText), nLength)

End Function

' Remove whitespace characters from the start of the text. These include space,
' backspace, tab, newline and all other control characters.
Public Function TrimLeft(ByVal sText As String) As String

    Dim nLength As Long
    Dim iPosFirstNWS As Long

    nLength = Len(sText)
    For iPosFirstNWS = 1 To nLength
        Select Case AscW(Mid$(sText, iPosFirstNWS, 1))
        Case 0 To 32, 127
            ' Whitespace, carry on searching.
        Case Else
            ' Non-whitespace, exit loop.
            Exit For
        End Select
    Next
    TrimLeft = Right$(sText, nLength - iPosFirstNWS + 1)

End Function

' Remove whitespace characters from the end of the text. These include space,
' backspace, tab, newline and all other control characters.
Public Function TrimRight(ByVal sText As String) As String

    Dim nLength As Long
    Dim iPosLastNWS As Long

    nLength = Len(sText)
    For iPosLastNWS = nLength To 1 Step -1
        Select Case AscW(Mid$(sText, iPosLastNWS, 1))
        Case 0 To 32, 127
            ' Whitespace, carry on searching.
        Case Else
            ' Non-whitespace, exit loop.
            Exit For
        End Select
    Next
    TrimRight = Left$(sText, iPosLastNWS)

End Function

Public Function AddSlash(ByVal sPath As String) As String

    Dim sLastChar As String

    sLastChar = Right$(sPath, 1)
    If sPath = "" Or sLastChar = ":" Or sLastChar = "\" Then
        AddSlash = sPath
    Else
        AddSlash = sPath & "\"
    End If

End Function

Public Function RemoveSlash(ByVal sPath As String) As String

    If Right$(sPath, 2) = ":\" Then
        ' do nothing
    ElseIf Right$(sPath, 1) = "\" Then
        sPath = Left$(sPath, Len(sPath) - 1)
    End If

    RemoveSlash = sPath

End Function

Public Function RemoveTZ(ByVal sBuffer As String) As String

    Dim iPosZero As Long

    iPosZero = InStr(sBuffer, vbNullChar)
    If iPosZero > 0 Then
        RemoveTZ = Left$(sBuffer, iPosZero - 1)
    Else
        RemoveTZ = sBuffer
    End If

End Function

' Removes characters from the end of a string only if they are present.
Public Function RemoveChars(ByVal sText As String, ByVal sChars As String, _
    Optional ByVal bCaseSensitive As Boolean = False) As String

    Dim nTextLength As Long
    Dim nCharsLength As Long
    Dim bFound As Boolean

    RemoveChars = sText
    nTextLength = Len(sText)
    nCharsLength = Len(sChars)
    If nTextLength = 0 Or nCharsLength = 0 Then Exit Function

    If bCaseSensitive Then
        bFound = (Right$(sText, nCharsLength) = sChars)
    Else
        bFound = (UCase$(Right$(sText, nCharsLength)) = UCase$(sChars))
    End If

    If bFound Then
        RemoveChars = Left$(sText, nTextLength - nCharsLength)
    End If

End Function

' Returns the absolute path to a file, given the "current directory"
' and a filespec, which may include a relative path of its own.
Public Function AddPath(ByVal sCurrentPath As String, ByVal sFileSpec As String) As String

    Dim sPathOfSpec As String
    Dim sFileOfSpec As String
    Dim sPathChar1 As String
    Dim sPathChar2 As String

    ' Do the trivial cases first.
    If sCurrentPath = "" Then
        AddPath = sFileSpec
        Exit Function
    ElseIf sFileSpec = "" Then
        AddPath = sCurrentPath
        Exit Function
    End If

    sPathOfSpec = ParsePathFile(sFileSpec, sFileOfSpec)
    If sPathOfSpec = "" Then
        ' Filespec has no path, so use current folder.
        AddPath = AddSlash(sCurrentPath) & sFileSpec
    Else
        sPathChar1 = UCase$(Left$(sPathOfSpec, 1))
        sPathChar2 = Mid$(sPathOfSpec, 2, 1)
        If sPathChar1 = "\" Then
            ' Filespec has an absolute path, so use that.
            AddPath = sFileSpec
        ElseIf sPathChar1 >= "A" And sPathChar1 <= "Z" And sPathChar2 = ":" Then
            ' Filespec has an absolute path, so use that.
            AddPath = sFileSpec
        Else
            ' Filespec has a relative path, so string them together.
            AddPath = AddSlash(sCurrentPath) & sFileSpec
        End If
    End If

End Function

' Converts string to name-case form. Recognises prefixes of "O'",
' "Mc" and "Mac". Also recognises hyphens as word separators.
Public Function CapitaliseInitials(ByVal sText As String) As String

    Dim iPosPrevSep As Long
    Dim iPosNextSep As Long
    Dim iPosWordStart As Long
    Dim nLenWord As Long
    Dim sWord As String

    If sText = "" Then Exit Function

    iPosPrevSep = 0
    Do
        ' Find next word break.
        iPosNextSep = NextWordBreak(iPosPrevSep + 1, sText)

        ' If no more words, just process the last word.
        If iPosNextSep = 0 Then
            iPosNextSep = Len(sText) + 1
        End If

        ' Extract word from string.
        iPosWordStart = iPosPrevSep + 1
        nLenWord = iPosNextSep - iPosPrevSep - 1
        sWord = Mid$(sText, iPosWordStart, nLenWord)

        If nLenWord > 0 Then
            ' Capitalise initial letter of word.
            Mid$(sWord, 1) = UCase$(Left$(sWord, 1))
            If nLenWord > 1 Then
                Mid$(sWord, 2) = LCase$(Mid$(sWord, 2))
            End If
            If Left$(sWord, 2) = "Mc" And nLenWord > 3 Then
                Mid$(sWord, 3, 1) = UCase$(Mid$(sWord, 3, 1))
            End If
            If Left$(sWord, 3) = "Mac" And nLenWord > 4 Then
                Mid$(sWord, 4, 1) = UCase$(Mid$(sWord, 4, 1))
            End If
            If Left$(sWord, 2) = "O'" And nLenWord > 3 Then
                Mid$(sWord, 3, 1) = UCase$(Mid$(sWord, 3, 1))
            End If
            ' Write word back into string.
            Mid$(sText, iPosWordStart) = sWord
        End If

        ' Store word break position.
        iPosPrevSep = iPosNextSep

        ' If we have already fallen off the end, exit loop.
    Loop Until iPosPrevSep > Len(sText)

    CapitaliseInitials = sText

End Function

' Generic split function - can be used with a separator of any
' length. The bDefaultToRemainder parameter determines which of
' the two return values is filled if the separator is not found.
Public Function ParseSep(ByVal sLine As String, _
    ByRef r_sRemainder As String, _
    ByVal sSeparator As String, _
    Optional ByVal bDefaultToRemainder As Boolean = False) As String

    Dim iPos As Long

    ' Search for the separator in the string.
    iPos = InStr(sLine, sSeparator)

    If iPos > 0 Then
        ParseSep = Left$(sLine, iPos - 1)
        r_sRemainder = Mid$(sLine, iPos + Len(sSeparator))
    ElseIf bDefaultToRemainder Then
        ParseSep = ""
        r_sRemainder = sLine
    Else ' default to LHS (the most common usage)
        ParseSep = sLine
        r_sRemainder = ""
    End If

End Function

' A copy of ParseSep() that finds the last occurrence of the
' separator in the string, not the first. In all other respects
' it works the same way.
Public Function ParseSepRev(ByVal sLine As String, _
    ByRef r_sRemainder As String, _
    ByVal sSeparator As String, _
    Optional ByVal bDefaultToRemainder As Boolean = False) As String

    Dim iPos As Long

    ' Search for the separator in the string.
    iPos = InStrRev(sLine, sSeparator)

    If iPos > 0 Then
        ParseSepRev = Left$(sLine, iPos - 1)
        r_sRemainder = Mid$(sLine, iPos + Len(sSeparator))
    ElseIf bDefaultToRemainder Then
        ParseSepRev = ""
        r_sRemainder = sLine
    Else ' default to LHS (the most common usage)
        ParseSepRev = sLine
        r_sRemainder = ""
    End If

End Function

' This works just like the ParseSep() function, but the separator
' parameter is a list of all possible single-character separators
' to be searched for. The first one to be found determines where
' the string is split.
Public Function ParseSepFirstChar(ByVal sLine As String, _
    ByRef r_sRemainder As String, _
    ByVal sSeparatorChars As String, _
    Optional ByVal bDefaultToRemainder As Boolean = False) As String

    Dim iPos As Long
    Dim i As Long

    ' Roll-our-own search function that finds all the possible
    ' separators simultaneously.
    iPos = 0
    For i = 1 To Len(sLine)
        If InStr(sSeparatorChars, Mid$(sLine, i, 1)) > 0 Then
            iPos = i
            Exit For
        End If
    Next

    If iPos > 0 Then
        ParseSepFirstChar = Left$(sLine, iPos - 1)
        r_sRemainder = Mid$(sLine, iPos + 1) ' Len(sSep) is defined to be 1 in this case
    ElseIf bDefaultToRemainder Then
        ParseSepFirstChar = ""
        r_sRemainder = sLine
    Else ' default to LHS (the most common usage)
        ParseSepFirstChar = sLine
        r_sRemainder = ""
    End If

End Function

' Extracts first string from a comma-separated list, returning
' the rest of the list by reference. List items are separated by
' commas surrounded by white space. If item is enclosed in "'s,
' contents are taken literally.
Public Function ParseCSV(ByVal sLine As String, _
    ByRef r_sRemainder As String) As String

    Dim sListHead As String
    Dim sListTail As String
    Dim sTemp As String
    Dim iPosHeadStart As Long
    Dim iPosHeadEnd As Long
    Dim iPosTailStart As Long
    Dim iPosNextComma As Long
    Dim iPosNextQuote As Long

    sLine = Trim$(sLine)
    iPosNextComma = InStr(sLine, ",")
    iPosNextQuote = InStr(sLine, """")

    If iPosNextComma = 0 And iPosNextQuote = 0 Then
        ' No more separators found. Return whole string as head.
        sListHead = sLine
        sListTail = ""

    ElseIf iPosNextComma > 0 And (iPosNextQuote = 0 Or iPosNextComma < iPosNextQuote) Then
        ' There are multiple list items, and at least the first one has
        ' no quotes in it. Therefore we just split at the first comma.
        sListHead = Trim$(Left$(sLine, iPosNextComma - 1))
        sListTail = Trim$(Mid$(sLine, iPosNextComma + 1))

    ElseIf iPosNextQuote > 1 Then
        ' First item is quoted, but the quote isn't the first character.
        ' This is illegal, so we assume there is a comma immediately
        ' before the first quote and treat that as the second item.
        sListHead = Left$(sLine, iPosNextQuote - 1)
        sListTail = Mid$(sLine, iPosNextQuote)

    Else ' iPosNextQuote = 1
        ' First item starts with a quote. Therefore we scan through
        ' to find the finishing quote, taken double-quotes into account
        ' along the way. If the finishing quote is not followed by a
        ' comma, assume there is one.
        iPosHeadStart = iPosNextQuote + 1

        ' This used to be a simple search for the next quote.
        ' It now becomes a loop which rejects double-quotes
        ' while converting them into singles.
        Do
            iPosNextQuote = InStr(iPosNextQuote + 1, sLine, """")
            ' Exit now if next quote cannot be double.
            If iPosNextQuote = 0 Or iPosNextQuote = Len(sLine) Then
                Exit Do
            End If
            ' Test for double-quote.
            If Mid$(sLine, iPosNextQuote + 1, 1) = """" Then
                ' Reduce to single quote.
                sLine = Left$(sLine, iPosNextQuote - 1) & Mid$(sLine, iPosNextQuote + 1)
                ' If we cannot now continue, then exit.
                If iPosNextQuote = Len(sLine) Then
                    iPosNextQuote = 0
                    Exit Do
                End If
            Else
                ' Single quote found, so exit.
                Exit Do
            End If
        Loop

        If iPosNextQuote = 0 Then
            ' No further quotes, so take whole string as head.
            sListHead = Mid$(sLine, iPosHeadStart)
            sListTail = ""
        Else
            ' Mark end of field.
            iPosHeadEnd = iPosNextQuote - 1
            iPosTailStart = iPosNextQuote + 1
            ' Get string after the quote, and test for comma.
            sTemp = LTrim$(Mid$(sLine, iPosTailStart))
            ' If comma does not exist, assume one next.
            If Left$(sTemp, 1) = "," Then
                iPosTailStart = iPosTailStart + 1
            End If
            If iPosHeadEnd - iPosHeadStart + 1 < 1 Then
                sListHead = ""
            Else
                sListHead = Mid$(sLine, iPosHeadStart, iPosHeadEnd - iPosHeadStart + 1)
            End If
            sListTail = LTrim$(Mid$(sLine, iPosTailStart))
        End If

    End If

    ParseCSV = sListHead
    r_sRemainder = sListTail

End Function

Public Function ParsePathFile(ByVal sFileSpec As String, _
    ByRef r_sFileName As String) As String

    Dim i As Long
    Dim iPosSlash As Long

    ' Find the last backslash or colon in string. 0 if not found.
    For i = Len(sFileSpec) To 1 Step -1
        If Mid$(sFileSpec, i, 1) = ":" Or Mid$(sFileSpec, i, 1) = "\" Then
            Exit For
        End If
    Next
    iPosSlash = i

    ' Separate out the path and the filename. Be careful not to delete
    ' the slash in T:\SYSVARS.DAT.
    If iPosSlash > 0 Then
        ParsePathFile = RemoveSlash(Left$(sFileSpec, iPosSlash))
        r_sFileName = Mid$(sFileSpec, iPosSlash + 1)
    Else
        ParsePathFile = ""
        r_sFileName = sFileSpec
    End If

End Function

Public Function ParseFileExt(ByVal sFileName As String, _
    ByRef r_sExtension As String) As String

    Dim i As Long
    Dim iPosDot As Long

    ' Find the last dot in string. 0 if not found.
    For i = Len(sFileName) To 1 Step -1
        If Mid$(sFileName, i, 1) = "." Then
            Exit For
        End If
    Next
    iPosDot = i

    ' Separate out the filename and the extension.
    If iPosDot > 0 Then
        ParseFileExt = Left$(sFileName, iPosDot - 1)
        r_sExtension = Mid$(sFileName, iPosDot + 1)
    Else
        ParseFileExt = sFileName
        r_sExtension = ""
    End If

End Function

' Takes a multi-line block of text and adds the requested number
' of spaces to the start of each line. If there is a terminating
' newline, it is removed.
Public Function Indent(ByVal nSpaces As Integer, ByVal sText As String) As String

    sText = RemoveChars(sText, vbCrLf)
    sText = Space$(nSpaces) & Replace$(sText, vbCrLf, vbCrLf & Space$(nSpaces))
    Indent = sText

End Function

' Return true if the text starts with the specified characters.
' Optionally return the remainder of the text by reference.
Public Function StartsWith(ByVal sText As String, ByVal sChars As String, _
    Optional ByRef o_sRemainder As String, _
    Optional ByVal bCaseSensitive As Boolean = False) As Boolean

    Dim nLenRemainder As Long

    If bCaseSensitive Then
        StartsWith = (Left$(sText, Len(sChars)) = sChars)
    Else
        StartsWith = (Left$(UCase$(sText), Len(sChars)) = UCase$(sChars))
    End If

    If StartsWith Then
        If Not IsMissing(o_sRemainder) Then
            nLenRemainder = Len(sText) - Len(sChars)
            If nLenRemainder > 0 Then
                o_sRemainder = Right$(sText, nLenRemainder)
            Else
                o_sRemainder = ""
            End If
        End If
    End If

End Function

' Return true if the text ends with the specified characters.
' Optionally return the remainder of the text by reference.
Public Function EndsWith(ByVal sText As String, ByVal sChars As String, _
    Optional ByRef o_sRemainder As String, _
    Optional ByVal bCaseSensitive As Boolean = False) As Boolean

    Dim nLenRemainder As Long

    If bCaseSensitive Then
        EndsWith = (Right$(sText, Len(sChars)) = sChars)
    Else
        EndsWith = (Right$(UCase$(sText), Len(sChars)) = UCase$(sChars))
    End If

    If EndsWith Then
        If Not IsMissing(o_sRemainder) Then
            nLenRemainder = Len(sText) - Len(sChars)
            If nLenRemainder > 0 Then
                o_sRemainder = Left$(sText, nLenRemainder)
            Else
                o_sRemainder = ""
            End If
        End If
    End If

End Function

' This is now obsolete since VB6 provides us with one already.
Public Function SearchReplace(ByVal sLine As String, _
    ByVal sSearch As String, _
    ByVal sReplace As String) As String

    SearchReplace = Replace$(sLine, sSearch, sReplace)

End Function

' Faster version of all the different functions in modTools.
' nTab starts at 0, to match a multi-column list box control.
Public Property Get NthColumn(ByRef r_sLine As String, _
    ByVal nCol As Integer) As Variant

    Dim sLineCopy As String
    Dim sColText As String
    Dim i As Integer

    sLineCopy = r_sLine
    sColText = ""
    For i = 0 To nCol
        sColText = ParseSep(sLineCopy, sLineCopy, vbTab)
    Next
    NthColumn = sColText

End Property

' Inserts a new value into a listbox row without disturbing the
' other columns.
Public Property Let NthColumn(ByRef r_sLine As String, _
    ByVal nCol As Integer, _
    ByVal vNewValue As Variant)

    Dim sLineCopy As String
    Dim sColText As String
    Dim i As Integer
    Dim sNewLine As String
    Dim vsNewValue As Variant ' String or Null

    ' Only read this parameter once, and ensure it is never Null.
    ' Do this manually instead of using GDataTypes to avoid
    ' unnecessary cross-class calls.
    vsNewValue = vNewValue
    If IsNull(vsNewValue) Then
        vsNewValue = ""
    Else
        vsNewValue = CStr(vsNewValue)
    End If

    ' Copy sLine into sNewLine up to but not including the column we
    ' want to change.
    sLineCopy = r_sLine
    sNewLine = ""
    For i = 0 To nCol - 1
        sColText = ParseSep(sLineCopy, sLineCopy, vbTab)
        If sNewLine <> "" Then sNewLine = sNewLine & vbTab
        sNewLine = sNewLine & sColText
    Next

    ' Add the new value in at this point.
    If sNewLine <> "" Then sNewLine = sNewLine & vbTab
    sNewLine = sNewLine & vsNewValue

    ' Skip over the old value in the line.
    ParseSep sLineCopy, sLineCopy, vbTab

    ' Add the remainder of sLine to sNewLine.
    If sNewLine <> "" Then sNewLine = sNewLine & vbTab
    sNewLine = sNewLine & sLineCopy

    r_sLine = sNewLine

End Property

' Helper function for CapitaliseInitials().
Private Function NextWordBreak(ByVal iPosStart As Long, _
    ByVal sSearch As String) As Long

    Const ksSeparatorChars = " .,-:;&()[]{}+"""

    Dim iPosFirstFound As Long
    Dim i As Long

    If iPosStart < 1 Then
        iPosStart = 1
    End If

    ' Roll-our-own search function that finds all the possible
    ' separators simultaneously.
    iPosFirstFound = 0
    For i = iPosStart To Len(sSearch)
        If InStr(ksSeparatorChars, Mid$(sSearch, i, 1)) > 0 Then
            iPosFirstFound = i
            Exit For
        End If
    Next

    NextWordBreak = iPosFirstFound

End Function
