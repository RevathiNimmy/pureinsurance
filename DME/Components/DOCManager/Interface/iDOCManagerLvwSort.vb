Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports System
Module iDOCManagerLvwSort
    '
    'SUMMARY
    '=======
    '
    'When using a Listview control, you can set the Sorted property for the
    'control to sort the list alphabetically. However, the Listview control does
    'not expose a property or method for sorting a list by date. This article
    'presents a method that you can use to sort a Listview control by date.
    '

    Public Structure POINT
        Dim x As Integer
        Dim y As Integer
    End Structure

    Public Structure LV_FINDINFO
        Dim flags As Integer
        Dim psz As String
        Dim lParam As Integer
        Dim pt As POINT
        Dim vkDirection As Integer
        Public Shared Function CreateInstance() As LV_FINDINFO
            Dim result As New LV_FINDINFO
            result.psz = String.Empty
            Return result
        End Function
    End Structure

    Public Structure LV_ITEM
        Dim mask As Integer
        Dim iItem As Integer
        Dim iSubItem As Integer
        Dim State As Integer
        Dim stateMask As Integer
        Dim pszText As Integer
        Dim cchTextMax As Integer
        Dim iImage As Integer
        Dim lParam As Integer
        Dim iIndent As Integer
    End Structure

    ' Variables
    Public bSortAccending As Boolean

    'Constants
    Private Const LVFI_PARAM As Integer = 1
    Private Const LVIF_TEXT As Integer = &H1S

    Private Const LVM_FIRST As Integer = &H1000S
    Private Const LVM_FINDITEM As Integer = LVM_FIRST + 13
    Private Const LVM_GETITEMTEXT As Integer = LVM_FIRST + 45
    Public Const LVM_SORTITEMS As Integer = LVM_FIRST + 48

    'API declarations
    Declare Function SendMessage Lib "USER32" Alias "SendMessageA" (ByVal hWnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer

    'Module Functions and Procedures

    'CompareDates: This is the sorting routine that gets passed to the
    'ListView control to provide the comparison test for date values.

    Public Function CompareDates(ByVal lngParam1 As Integer, ByVal lngParam2 As Integer, ByVal hWnd As Integer) As Integer

        Dim strName1, strName2 As String
        Dim dDate1, dDate2 As Date

        'Obtain the item names and dates corresponding to the
        'input parameters

        ListView_GetItemData(lngParam1, hWnd, strName1, dDate1)
        ListView_GetItemData(lngParam2, hWnd, strName2, dDate2)

        'Compare the dates
        'Return 0 ==> Less Than
        '       1 ==> Equal
        '       2 ==> Greater Than

        ' Decide which way we want to sort
        If bSortAccending Then

            If dDate1 < dDate2 Then
                Return 0
            ElseIf dDate1 = dDate2 Then
                Return 1
            Else
                Return 2
            End If

        Else

            If dDate1 > dDate2 Then
                Return 0
            ElseIf dDate1 = dDate2 Then
                Return 1
            Else
                Return 2
            End If

        End If

    End Function

    'GetItemData - Given Retrieves

    Public Sub ListView_GetItemData(ByRef lngParam As Integer, ByRef hWnd As Integer, ByRef strName As String, ByRef dDate As Date)
        Dim objFind As LV_FINDINFO = LV_FINDINFO.CreateInstance()
        Dim objItem As New LV_ITEM
        Dim baBuffer(32) As Byte

        '
        ' Convert the input parameter to an index in the list view
        '
        objFind.flags = LVFI_PARAM
        objFind.lParam = lngParam

        'TODO: Needs to be uncommented
        'Dim lngIndex As Integer = SendMessage(hWnd, CInt(LVM_FINDITEM), -1, VarPtr(objFind))
        Dim lngIndex As Integer

        '
        ' Obtain the name of the specified list view item
        '
        objItem.mask = LVIF_TEXT
        objItem.iSubItem = 0

        'developer guid no. 121
        objItem.pszText = CType((baBuffer(0)), IntPtr)
        objItem.cchTextMax = baBuffer.GetUpperBound(0)

        Dim lngLength As Integer = SendMessage(hWnd, CInt(LVM_GETITEMTEXT), lngIndex, objItem.pszText)

        strName = StringsHelper.StrConv(StringsHelper.ByteArrayToString(baBuffer), StringsHelper.VbStrConvEnum.vbUnicode).Substring(0, lngLength)

        '
        ' Obtain the modification date of the specified list view item
        '
        objItem.mask = LVIF_TEXT
        objItem.iSubItem = 1


        objItem.pszText = CType((baBuffer(0)), IntPtr)
        objItem.cchTextMax = baBuffer.GetUpperBound(0)

        lngLength = SendMessage(hWnd, CInt(LVM_GETITEMTEXT), lngIndex, objItem.pszText)
        If lngLength > 0 Then

            dDate = CDate(StringsHelper.StrConv(StringsHelper.ByteArrayToString(baBuffer), StringsHelper.VbStrConvEnum.vbUnicode).Substring(0, lngLength))
        End If

    End Sub

    'GetListItem - This is a modified version of ListView_GetItemData
    ' It takes an index into the list as a parameter and returns
    ' the appropriate values in the strName and dDate parameters.

    Public Sub ListView_GetListItem(ByRef lngIndex As Integer, ByRef hWnd As Integer, ByRef strName As String, ByRef dDate As Date)
        Dim objItem As New LV_ITEM
        Dim baBuffer(32) As Byte

        '
        ' Obtain the name of the specified list view item
        '
        objItem.mask = LVIF_TEXT
        objItem.iSubItem = 0

        'developer guid no. 121
        objItem.pszText = CType(baBuffer(0), IntPtr)
        objItem.cchTextMax = baBuffer.GetUpperBound(0)

        'developer guid no. 121
        Dim lngLength As Integer = SendMessage(hWnd, CInt(LVM_GETITEMTEXT), lngIndex, objItem.pszText)

        strName = StringsHelper.StrConv(StringsHelper.ByteArrayToString(baBuffer), StringsHelper.VbStrConvEnum.vbUnicode).Substring(0, lngLength)

        '
        ' Obtain the modification date of the specified list view item
        '
        objItem.mask = LVIF_TEXT
        objItem.iSubItem = 1

        'developer guid no. 121
        objItem.pszText = CType(baBuffer(0), IntPtr)
        objItem.cchTextMax = baBuffer.GetUpperBound(0)

        lngLength = SendMessage(hWnd, CInt(LVM_GETITEMTEXT), lngIndex, objItem.pszText)
        If lngLength > 0 Then

            dDate = CDate(StringsHelper.StrConv(StringsHelper.ByteArrayToString(baBuffer), StringsHelper.VbStrConvEnum.vbUnicode).Substring(0, lngLength))
        End If

    End Sub
End Module