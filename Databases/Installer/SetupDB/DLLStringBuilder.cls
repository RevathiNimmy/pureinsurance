VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CStringBuilder"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
' Class:    Joins strings much faster than normal contatenation
' Shared:   Yes (RESTRICTED)
' Needs:    Nothing
'
' THIS CLASS IS A COPY OF THE CORRESPONDING CLASS IN THE DLL.
' IT IS SHARED *ONLY* TO SUPPORT SMALL UTILITIES THAT
' CANNOT REFERENCE THE DLL. *DO NOT* ALTER THIS CODE IN ANY
' WAY UNLESS YOU ARE ALSO CHANGING THE DLL.
'
Option Explicit

Private Const knDefCapacityBase = 256&
Private Const knDefCapacityIncrement = 2&

Private m_sBuffer As String
Private m_nLength As Long
Private m_nCapacity As Long
Private m_nCapacityBase As Long
Private m_nCapacityIncrement As Long
Private m_bSeparator2 As Boolean
Private m_sSeparator1 As String
Private m_sSeparator2 As String

' Windows API declarations.
Private Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef lpDest As Any, ByRef lpSource As Any, ByVal nBytes As Long)

' Error declarations.
Const ksErrSource = "gSWLibrary.CStringBuilder"
Const knErrInvalidProcedureCall = 5
Const knErrSubscriptOutOfRange = 9
Const knErrInvalidPropertyValue = 380
Const knErrInvalidPropertyArrayIndex = 381

Public Property Get Length() As Long

    Length = m_nLength

End Property

Public Property Get Capacity() As Long

    Capacity = m_nCapacity

End Property

' Attributes: Hidden
Public Property Get CapacityBase() As Long
Attribute CapacityBase.VB_MemberFlags = "40"

    CapacityBase = m_nCapacityBase

End Property

' Attributes: Hidden
Public Property Let CapacityBase(ByVal nCapacityBase As Long)

    ' Safety checks.
    If nCapacityBase < 1 Then
        Err.Raise knErrInvalidPropertyValue, ksErrSource
    End If

    m_nCapacityBase = nCapacityBase

End Property

' Attributes: Hidden
Public Property Get CapacityIncrement() As Long
Attribute CapacityIncrement.VB_MemberFlags = "40"

    CapacityIncrement = m_nCapacityIncrement

End Property

' Attributes: Hidden
Public Property Let CapacityIncrement(ByVal nCapacityIncrement As Long)

    ' Safety checks.
    If nCapacityIncrement < 1 Then
        Err.Raise knErrInvalidPropertyValue, ksErrSource
    End If

    m_nCapacityIncrement = nCapacityIncrement

End Property

' Attributes: Default
Public Property Get Text() As String
Attribute Text.VB_UserMemId = 0

    ' Return the portion of the buffer containing valid data.
    Text = VBA.Left$(m_sBuffer, m_nLength)

End Property

' Attributes: Default
Public Property Let Text(ByVal sText As String)

    ' Calculate the text length.
    m_nLength = Len(sText)

    ' Initialise the buffer to nearest multiple of default capacity that will fit the text.
    m_nCapacity = m_nCapacityBase
    Do Until m_nCapacity >= m_nLength
        m_nCapacity = m_nCapacity * m_nCapacityIncrement
    Loop

    ' Allocate the full capacity in the buffer.
    m_sBuffer = String$(m_nCapacity, 0)

    ' Copy the text into the buffer at the start.
    Mid$(m_sBuffer, 1, m_nLength) = sText

End Property

' Add some text to the buffer at the end.
Public Sub Append(ByVal sText As String)

    Dim nAddLength As Long
    Dim nNewLength As Long
    Dim sOldBuffer As String

    ' If the buffer has not been initialised yet, initialise it here instead of
    ' appending the text. If we don't check for this then some of the code below
    ' will bomb out.
    If m_nCapacity = 0 Then
        Text = sText
        Exit Sub
    End If

    ' Calculate the new text length. If the new text is empty, then abort now
    ' because the code crashes if m_nCapacity = m_nLength and nAddLength = 0.
    nAddLength = Len(sText)
    If nAddLength = 0 Then
        Exit Sub
    End If
    nNewLength = m_nLength + nAddLength

    ' Will the new text fit in the existing buffer?
    If m_nCapacity < nNewLength Then
        ' Expand the buffer to nearest multiple of capacity that will fit the text.
        Do
            m_nCapacity = m_nCapacity * m_nCapacityIncrement
        Loop Until m_nCapacity >= nNewLength

        ' Swap the front and back buffers to avoid yet another memory copy.
        SwapBuffers m_sBuffer, sOldBuffer

        ' Allocate the new capacity in the buffer.
        m_sBuffer = String$(m_nCapacity, 0)

        ' Copy the old buffer into the new buffer at the start.
        Mid$(m_sBuffer, 1, m_nLength) = sOldBuffer

        ' Reclaim the back buffer memory used as fast as possible.
        sOldBuffer = vbNullString
    End If

    ' Copy the new text into the buffer just after the old text.
    Mid$(m_sBuffer, m_nLength + 1, nAddLength) = sText

    ' Save the new text length.
    m_nLength = nNewLength

End Sub

' Insert some text to the buffer at the specified position.
Public Sub Insert(ByVal sText As String, Optional ByVal nStart As Long = 1)

    Dim nAddLength As Long
    Dim nNewLength As Long
    Dim sOldBuffer As String

    ' If the buffer has not been initialised yet, initialise it here instead of
    ' inserting the text. If we don't check for this then some of the code below
    ' will bomb out.
    If m_nCapacity = 0 Then
        ' However, it only makes sense to do this when inserting at the start.
        If nStart <> 1 Then
            Err.Raise knErrSubscriptOutOfRange, ksErrSource
        End If
        Text = sText
        Exit Sub
    End If

    ' Safety checks.
    If nStart < 1 Then
        Err.Raise knErrSubscriptOutOfRange, ksErrSource
    ElseIf nStart > m_nLength + 1 Then
        Err.Raise knErrSubscriptOutOfRange, ksErrSource
    End If

    ' Calculate the new text length. If the new text is empty, then abort now
    ' because the code crashes if m_nCapacity = m_nLength and nAddLength = 0.
    nAddLength = Len(sText)
    If nAddLength = 0 Then
        Exit Sub
    End If
    nNewLength = m_nLength + nAddLength

    ' Swap the front and back buffers to avoid yet another memory copy.
    SwapBuffers m_sBuffer, sOldBuffer

    ' Will the new text fit in the existing buffer?
    If m_nCapacity < nNewLength Then
        ' Expand the buffer to nearest multiple of capacity that will fit the text.
        Do
            m_nCapacity = m_nCapacity * m_nCapacityIncrement
        Loop Until m_nCapacity >= nNewLength
    End If

    ' Allocate the new capacity in the buffer.
    m_sBuffer = String$(m_nCapacity, 0)

    ' Copy the new text into the buffer at the specified position.
    Mid$(m_sBuffer, nStart, nAddLength) = sText

    ' Copy part of the old buffer into the new buffer at the start.
    Mid$(m_sBuffer, 1, nStart - 1) = VBA.Left$(sOldBuffer, nStart - 1)

    ' Copy the remainder of the old buffer into the new buffer just after the new text.
    Mid$(m_sBuffer, nStart + nAddLength, m_nLength - nStart + 1) = VBA.Mid$(sOldBuffer, nStart, m_nLength - nStart + 1)

    ' Reclaim the back buffer memory used as fast as possible.
    sOldBuffer = vbNullString

    ' Save the new text length.
    m_nLength = nNewLength

End Sub

' Free all the string memory and reset the class for next use.
Public Sub Dispose()

    m_sBuffer = vbNullString
    m_nLength = 0
    m_nCapacity = 0
    m_nCapacityBase = knDefCapacityBase
    m_nCapacityIncrement = knDefCapacityIncrement
    m_bSeparator2 = False
    m_sSeparator1 = vbNullString
    m_sSeparator2 = vbNullString

End Sub

' Initialise separators.
Public Sub InitialiseSeparator(ByVal sSeparator2 As String)

    m_bSeparator2 = False
    m_sSeparator1 = vbNullString
    m_sSeparator2 = sSeparator2

End Sub

' Initialise separators.
Public Sub InitialiseSeparators(ByVal sSeparator1 As String, ByVal sSeparator2 As String)

    m_bSeparator2 = False
    m_sSeparator1 = sSeparator1
    m_sSeparator2 = sSeparator2

End Sub

' Add a separator to the end.
Public Sub AppendSeparator()

    If m_bSeparator2 Then
        Append m_sSeparator2
    Else
        Append m_sSeparator1
        m_bSeparator2 = True
    End If

End Sub

' Insert a separator at the specified position.
Public Sub InsertSeparator(Optional ByVal nStart As Long = 1)

    If m_bSeparator2 Then
        Insert m_sSeparator2, nStart
    Else
        Insert m_sSeparator1, nStart
        m_bSeparator2 = True
    End If

End Sub

' Read one character in the string using array indexer syntax.
Public Property Get Char(ByVal nPosition As Long) As String

    ' Safety checks.
    If nPosition < 1 Then
        Err.Raise knErrInvalidPropertyArrayIndex, ksErrSource
    ElseIf nPosition > m_nLength Then
        Err.Raise knErrInvalidPropertyArrayIndex, ksErrSource
    End If

    Char = VBA.Mid$(m_sBuffer, nPosition, 1)

End Property

' Write one character in the string using array indexer syntax.
Public Property Let Char(ByVal nPosition As Long, ByVal sChar As String)

    ' Safety checks.
    If nPosition < 1 Then
        Err.Raise knErrInvalidPropertyArrayIndex, ksErrSource
    ElseIf nPosition > m_nLength Then
        Err.Raise knErrInvalidPropertyArrayIndex, ksErrSource
    End If

    Mid$(m_sBuffer, nPosition, 1) = sChar

End Property

' Implements the equivalent VB string function, without the overhead of converting
' the class to a normal string first.
Public Function Left(ByVal nLength As Long) As String

    ' Safety checks.
    If nLength < 0 Then
        Err.Raise knErrSubscriptOutOfRange, ksErrSource
    ElseIf nLength > m_nLength Then
        nLength = m_nLength
    End If

    Left = VBA.Left$(m_sBuffer, nLength)

End Function

' Implements the equivalent VB string function, without the overhead of converting
' the class to a normal string first.
Public Function Right(ByVal nLength As Long) As String

    ' Safety checks.
    If nLength < 0 Then
        Err.Raise knErrSubscriptOutOfRange, ksErrSource
    ElseIf nLength > m_nLength Then
        nLength = m_nLength
    End If

    Right = VBA.Mid$(m_sBuffer, m_nLength - nLength + 1, nLength)

End Function

' Implements the equivalent VB string function, without the overhead of converting
' the class to a normal string first.
Public Function Mid(ByVal nStart As Long, Optional ByVal nLength As Long = -1) As String

    ' Safety checks.
    If nStart < 1 Then
        Err.Raise knErrSubscriptOutOfRange, ksErrSource
    ElseIf nStart > m_nLength + 1 Then
        nStart = m_nLength + 1
    End If
    If nLength < -1 Then
        Err.Raise knErrSubscriptOutOfRange, ksErrSource
    ElseIf nLength = -1 Then
        nLength = m_nLength - nStart + 1
    ElseIf nLength > m_nLength - nStart + 1 Then
        nLength = m_nLength - nStart + 1
    End If

    Mid = VBA.Mid$(m_sBuffer, nStart, nLength)

End Function

' Implements the equivalent VB string function, but operates on the text in-place, and
' without the overhead of converting the class to a normal string first.
Public Sub Replace(ByVal sFind As String, ByVal sReplace As String, _
    Optional ByVal nStart As Long = 1, _
    Optional ByVal nCount As Long = -1, _
    Optional ByVal nCompare As VBA.VbCompareMethod = vbBinaryCompare)

    Dim sText As String

    ' Only incur the massive overhead of a text replacement if we determine that the
    ' replacement would actually happen.
    If Len(sFind) <> 0 Then
        sText = VBA.Left$(m_sBuffer, m_nLength)
        If InStr(nStart, sText, sFind, nCompare) <> 0 Then
            Text = VBA.Replace$(sText, sFind, sReplace, nStart, nCount, nCompare)
        End If
    End If

End Sub

' Removes whitespace characters from the start of the text in-place. These include space,
' backspace, tab, newline and all other control characters.
Public Sub TrimLeft()

    Dim iPosFirstNWS As Long

    For iPosFirstNWS = 1 To m_nLength
        Select Case AscW(VBA.Mid$(m_sBuffer, iPosFirstNWS, 1))
        Case 0 To 32, 127
            ' Whitespace, carry on searching.
        Case Else
            ' Non-whitespace, exit loop.
            Exit For
        End Select
    Next

    ' Subtract the whitespace characters from the text length.
    m_nLength = m_nLength - iPosFirstNWS + 1

    ' Move the remainder of the text back to the start.
    Mid$(m_sBuffer, 1, m_nLength) = VBA.Mid$(m_sBuffer, iPosFirstNWS, m_nLength)

End Sub

' Removes whitespace characters from the end of the text in-place. These include space,
' backspace, tab, newline and all other control characters.
Public Sub TrimRight()

    Dim iPosLastNWS As Long

    For iPosLastNWS = m_nLength To 1 Step -1
        Select Case AscW(VBA.Mid$(m_sBuffer, iPosLastNWS, 1))
        Case 0 To 32, 127
            ' Whitespace, carry on searching.
        Case Else
            ' Non-whitespace, exit loop.
            Exit For
        End Select
    Next

    ' Set the text length to the last non-whitespace character found.
    m_nLength = iPosLastNWS

End Sub

Private Sub Class_Initialize()

    Dispose

End Sub

Private Sub Class_Terminate()

    Dispose

End Sub

' Copies the current text to the clipboard for easier debugging.
Friend Sub CopyToClipboard()

    Clipboard.Clear
    Clipboard.SetText Text

End Sub

' Swap two string buffer pointers without copying the data.
Private Sub SwapBuffers(ByRef r_sBuffer1 As String, ByRef r_sBuffer2 As String)

    Dim lpBuffer1 As Long

    ' StrPtr returns the address of the first character in a string. VarPtr returns the
    ' address of a string's descriptor, which is 4 bytes long and contains the address
    ' of the first character in the string. StrPtr and VarPtr are undocumented functions.
    lpBuffer1 = StrPtr(r_sBuffer1)
    CopyMemory ByVal VarPtr(r_sBuffer1), ByVal VarPtr(r_sBuffer2), 4
    CopyMemory ByVal VarPtr(r_sBuffer2), lpBuffer1, 4

End Sub
