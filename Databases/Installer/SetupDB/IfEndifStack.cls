VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CIfEndifStack"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
' Class:    Tracks nested #IF statements in an SPP file
' Shared:   Yes (RESTRICTED)
' Needs:    Nothing
'
' THIS CODE MODULE IS NOW SHARED DIRECTLY WITH THE SIRIUS DATABASE
' INSTALLER PROJECT, DUE TO ITS UNIQUE REQUIREMENT FOR NO VB DLL
' REFERENCES. ALL SWIFT CODE SHOULD REFERENCE THIS DLL AS NORMAL.
'
' We put this code in a class module rather than a standard module
' because the procedure that uses it is recursive, and will need to
' declare a separate instance of the class per recursion.
'
' WARNING: Always call the Do???? methods regardless of whether you
' are inside a disabled section or not, because the class has to
' keep track of which else/endif keywords belong to which indenting
' level.
'
Option Explicit

Public Enum EStatementType
    knBlockNone
    knBlockIf
    knBlockRunOnce
End Enum

' Implement the "stack" of #IF statuses as an array. This is much
' faster than a collection because:
' (a) we are only ever interested in the last item of the array,
' (b) we will never need For Each Next, Add, Item, Remove or Count,
' (c) we can cache the size of the array really easily,
' (d) we don't need a separate class module to hold the user-defined type.

Private Type TIfState
    Execute As Boolean          ' are we allowed to execute code here?
    SectionDone As Boolean      ' when this is set to True, all following sections are disabled
    Code As String              ' #runonce data
    Description As String       ' #runonce data
End Type

Private m_nIfStack As Long
Private m_aIfStack() As TIfState

' Error declarations.
Private Const ksErrSource = "iSWSQLScript.CIfEndifStack"
Private Const knErrElseIfWithoutIf = vbObjectError + 1
Private Const ksErrElseIfWithoutIf = "Syntax error - #elseif without #if."
Private Const knErrElseWithoutIf = vbObjectError + 2
Private Const ksErrElseWithoutIf = "Syntax error - #else without #if."
Private Const knErrEndIfWithoutIf = vbObjectError + 3
Private Const ksErrEndIfWithoutIf = "Syntax error - #endif without #if."
Private Const knErrRunOnceEndWithoutRunOnceBegin = vbObjectError + 4
Private Const ksErrRunOnceEndWithoutRunOnceBegin = "Syntax error - #runonceend without #runoncebegin."

Private Sub Class_Initialize()

    m_nIfStack = 0
    Erase m_aIfStack

End Sub

Private Sub Class_Terminate()

    m_nIfStack = 0
    Erase m_aIfStack

End Sub

Public Sub DoIf(ByVal bExecute As Boolean)

    m_nIfStack = m_nIfStack + 1
    ReDim Preserve m_aIfStack(1 To m_nIfStack)

    With m_aIfStack(m_nIfStack)
        .Execute = bExecute
        .SectionDone = False
    End With

End Sub

Public Sub DoElseIf(ByVal bExecute As Boolean)

    If Me.StatementType <> knBlockIf Then
        Err.Raise knErrElseIfWithoutIf, ksErrSource, ksErrElseIfWithoutIf
    End If

    With m_aIfStack(m_nIfStack)
        If .SectionDone Then
            .Execute = False
        ElseIf .Execute Then
            .Execute = False
            .SectionDone = True
        Else
            .Execute = bExecute
        End If
    End With

End Sub

Public Sub DoElse()

    If Me.StatementType <> knBlockIf Then
        Err.Raise knErrElseWithoutIf, ksErrSource, ksErrElseWithoutIf
    End If

    With m_aIfStack(m_nIfStack)
        If .SectionDone Then
            .Execute = False
        ElseIf .Execute Then
            .Execute = False
            .SectionDone = True
        Else
            .Execute = True
        End If
    End With

End Sub

Public Sub DoEndIf()

    If Me.StatementType <> knBlockIf Then
        Err.Raise knErrEndIfWithoutIf, ksErrSource, ksErrEndIfWithoutIf
    End If

    m_nIfStack = m_nIfStack - 1
    If m_nIfStack < 1 Then
        Erase m_aIfStack
    Else
        ReDim Preserve m_aIfStack(1 To m_nIfStack)
    End If

End Sub

Public Sub DoRunOnceBegin(ByVal bExecute As Boolean, ByVal sCode As String, ByVal sDescription As String)

    m_nIfStack = m_nIfStack + 1
    ReDim Preserve m_aIfStack(1 To m_nIfStack)

    With m_aIfStack(m_nIfStack)
        .Execute = bExecute
        .SectionDone = False
        .Code = sCode
        .Description = sDescription
    End With

End Sub

Public Sub DoRunOnceEnd(ByRef o_bExecute As Boolean, ByRef o_sCode As String, ByRef o_sDescription As String)

    If Me.StatementType <> knBlockRunOnce Then
        Err.Raise knErrRunOnceEndWithoutRunOnceBegin, ksErrSource, ksErrRunOnceEndWithoutRunOnceBegin
    End If

    With m_aIfStack(m_nIfStack)
        o_bExecute = .Execute
        o_sCode = .Code
        o_sDescription = .Description
    End With

    m_nIfStack = m_nIfStack - 1
    If m_nIfStack < 1 Then
        Erase m_aIfStack
    Else
        ReDim Preserve m_aIfStack(1 To m_nIfStack)
    End If

End Sub

' Return True if the calling proc is allowed to execute this particular line of code.
' WARNING: Always interpret the #IF keywords regardless of this flag!
Public Property Get Execute() As Boolean

    Dim i As Long

    ' Only return True if every element in the stack is True. This
    ' ensures that an inner #IF test (that may be True) is correctly
    ' disabled by an outer #IF test that is False.

    Execute = True ' still returns correct result if array is empty
    For i = 1 To m_nIfStack
        If Not m_aIfStack(i).Execute Then
            Execute = False
            Exit For
        End If
    Next

End Property

' Return the current nesting level.
Public Property Get NestLevel() As Long

    NestLevel = m_nIfStack

End Property

' Return the type of block statement currently active.
Public Property Get StatementType() As EStatementType

    If m_nIfStack < 1 Then
        StatementType = knBlockNone
    ElseIf Len(m_aIfStack(m_nIfStack).Code) = 0 Then
        StatementType = knBlockIf
    Else
        StatementType = knBlockRunOnce
    End If

End Property
