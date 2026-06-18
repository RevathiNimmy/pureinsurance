Attribute VB_Name = "MDLLMRUCombo"
' Module:   Stores an MRU list for a dropdown combo box
' Shared:   Yes (RESTRICTED)
' Needs:    MDLLRegistry
'
' THIS CODE IMPLEMENTS CORRESPONDING FUNCTIONS IN THE DLL.
' IT IS SHARED *ONLY* TO SUPPORT SMALL UTILITIES THAT
' CANNOT REFERENCE THE DLL. *DO NOT* ALTER THIS CODE IN ANY
' WAY UNLESS YOU ARE CHANGING THE INTERNALS OF THE DLL.
'
Option Explicit

Const knMaxListItems = 100
Const ksAttribute = "Item"

' Properties.
Private m_sMRUComboRegFolder As String

' Error declarations.
Const ksErrSource = "iSWServices.GMRUCombo"
Const knErrFolderRequired = 5
Const ksErrFolderRequired = "The registry folder must be set before calling this function."
Const knErrObjectNotCombo = 13
Const ksErrObjectNotCombo = "Type Mismatch (VB ComboBox object required)."
Const knErrNameRequired = 5
Const ksErrNameRequired = "The list name must be specified."

Public Property Get MRUComboRegFolder() As String

    MRUComboRegFolder = m_sMRUComboRegFolder

End Property

' This must be set at least once at the start of the application.
' It sets the registry folder under which all MRU lists
' are stored.
Public Property Let MRUComboRegFolder(ByVal sMRUComboRegFolder As String)

    m_sMRUComboRegFolder = sMRUComboRegFolder

End Property

Public Sub MRUComboRead(ByVal cboCombo As Object, _
    ByVal sListName As String, _
    Optional ByVal sMRUComboRegFolder As String = "")

    Dim sFolder As String
    Dim sItem As String
    Dim i As Integer

    ' If called from the DLL class, override the global
    ' folder with the class instance-specific folder.
    If sMRUComboRegFolder <> "" Then
        sFolder = sMRUComboRegFolder
    Else
        sFolder = m_sMRUComboRegFolder
    End If

    ' Safety checks.
    If sFolder = "" Then
        Err.Raise knErrFolderRequired, ksErrSource, ksErrFolderRequired
    ElseIf Not IsCombo(cboCombo, False) Then
        Err.Raise knErrObjectNotCombo, ksErrSource, ksErrObjectNotCombo
    ElseIf sListName = "" Then
        Err.Raise knErrNameRequired, ksErrSource, ksErrNameRequired
    End If

    sFolder = sFolder & "\" & sListName

    ' Read items into combo. Stop when we encounter a blank line.
    ' Put an arbitary maximum limit on the number of items.
    cboCombo.Clear
    For i = 0 To knMaxListItems - 1
        sItem = RegRead(HKEY_CURRENT_USER, sFolder, ksAttribute & i, "")
        If sItem = "" Then Exit For
        cboCombo.AddItem sItem
    Next

End Sub

Public Sub MRUComboWrite(ByVal cboCombo As Object, _
    ByVal sListName As String, _
    Optional ByVal sMRUComboRegFolder As String = "")

    Dim sFolder As String
    Dim sNewItem As String
    Dim i As Integer
    Dim iMatch As Integer

    ' If called from the DLL class, override the global
    ' folder with the class instance-specific folder.
    If sMRUComboRegFolder <> "" Then
        sFolder = sMRUComboRegFolder
    Else
        sFolder = m_sMRUComboRegFolder
    End If

    ' Safety checks.
    If sFolder = "" Then
        Err.Raise knErrFolderRequired, ksErrSource, ksErrFolderRequired
    ElseIf Not IsCombo(cboCombo, False) Then
        Err.Raise knErrObjectNotCombo, ksErrSource, ksErrObjectNotCombo
    ElseIf sListName = "" Then
        Err.Raise knErrNameRequired, ksErrSource, ksErrNameRequired
    End If

    sFolder = sFolder & "\" & sListName

    ' Don't save if the user hasn't selected anything.
    sNewItem = cboCombo.Text
    If Trim$(sNewItem) = "" Then Exit Sub

    ' Does the text match a list item?
    iMatch = -1
    If cboCombo.ListIndex > -1 Then
        iMatch = cboCombo.ListIndex
    Else
        For i = 0 To cboCombo.ListCount - 1
            If UCase$(sNewItem) = UCase$(cboCombo.List(i)) Then
                iMatch = i
                Exit For
            End If
        Next
    End If

    If iMatch = -1 Then
        ' If there is no match, add it to the top as a new item.
        cboCombo.AddItem sNewItem, 0
    Else
        ' Move the matching item to the top of the list.
        cboCombo.RemoveItem iMatch
        cboCombo.AddItem sNewItem, 0
    End If
    cboCombo.ListIndex = 0

    ' Delete any items spilling over the limit.
    Do While cboCombo.ListCount > knMaxListItems
        cboCombo.RemoveItem knMaxListItems
    Loop

    ' Write list to file, marking end of list with a blank entry.
    For i = 0 To cboCombo.ListCount - 1
        RegWrite HKEY_CURRENT_USER, sFolder, ksAttribute & i, cboCombo.List(i)
    Next
    RegDelete HKEY_CURRENT_USER, sFolder, ksAttribute & i

End Sub

' Returns True for a ComboBox object or, optionally, set to
' Nothing.
Private Function IsCombo(ByVal o As Object, _
    ByVal bAllowNothing As Boolean) As Boolean

    IsCombo = False
    If o Is Nothing Then
        IsCombo = bAllowNothing
    ElseIf TypeName(o) = "ComboBox" Then
        IsCombo = True
    End If

End Function
