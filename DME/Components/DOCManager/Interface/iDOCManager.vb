Option Strict Off
Option Explicit On
Imports System
Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date: 24/11/1997
    '
    ' Description: Main module containing public variable/constants.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "iDOCManager"


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

    ' RDC 24062005
    Public Const OPTIONS_VIEWER_ALLOW_CUT_PASTE As String = "ALLOW_COPY_PASTE"

    ' Declare an instance of the splash object.

    Public g_oSplash As iDOCSplash.Interface_Renamed

    ' Declare an instance of the Business object.
    Public g_oBusiness As Object

    'MS 10/05/01
    Public g_lArchiveDocNum As Integer


    'Labels
    Public Const ACTitle1 As Integer = 500

    'Constants used in node keys (values, positions in key etc)
    ' Key structure is <F/D><P/' '><99999><Node Number>
    Public Const ACFolder As String = "F"
    Public Const ACDocument As String = "D"
    Public Const ACPassword As String = "P"
    Public Const ACPasswordStart As Integer = 2
    Public Const ACPasswordLen As Integer = 1
    Public Const ACDateStart As Integer = 3
    Public Const ACDateLen As Integer = 5

    ' {* USER DEFINED CODE (End) *}


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' Public source and language ID's from the
    ' Object Manager.
    Public g_iSourceID As Integer
    Public g_iLanguageID As Integer
    Public g_sUserName As String = ""

    'user access and admin levels
    Public g_iAccessLevel As Integer
    Public g_iAdminLevel As Integer
    Public g_bUserIsAdministrator As Boolean
    'ND 081100 - Delete and move levels
    Public g_iFileDeleteLevel As Integer
    Public g_iFolderDeleteLevel As Integer
    Public g_iFileMoveLevel As Integer
    Public g_iFolderMoveLevel As Integer
    Public g_iFileCopyLevel As Integer
    Public g_iFolderCopyLevel As Integer

    ' Public instance of the object manager.

    Public g_oObjectManager As bObjectManager.ObjectManager
    'Public g_oDOCManagerInterface As ClassInterface
    Public g_oDOCManagerInterface As Interface_Renamed


    ' Stores the return value for a function call.
    Private m_lReturn As Integer


    'Max folders returned before SelectFolders form shown
    Public g_lMaxAutoExpand As Integer

    'WR77 Documaster Enhancements START
    Public Const k_BCDocFileName As Integer = 1
    Public Const k_BCDocFileSize As Integer = 2
    'WR77 Documaster Enhancements END
    Public m_oInterface As Interface_Renamed



    Sub Main()
        ' If there is already a DocManager exe running then exit
        Dim iInstances As Integer = 0
        Dim Procesos() As Process = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName)
        If Procesos.Length > 1 Then
            For index As Integer = 0 To Procesos.Length - 1
                If Procesos(index).SessionId = Process.GetCurrentProcess.SessionId Then
                    iInstances += 1
                End If
            Next
            If iInstances > 1 Then
                Exit Sub
            End If

        End If

        g_oDOCManagerInterface = New iDOCManager.Interface_Renamed
        If g_oDOCManagerInterface.Initialise() = 1 Then
            g_oDOCManagerInterface.Start()
        Else
            Exit Sub
        End If
    End Sub
    'Author: Tariq Rashid
    Public Function GetSeletedNode(ByRef trv As TreeView) As TreeNode
        Dim upLevel0Index As Integer
        Dim upLevel1Index As Integer
        Dim upLevel2Index As Integer
        Dim upLevel3Index As Integer

        If trv.SelectedNode.Level = 0 Then
            upLevel0Index = trv.SelectedNode.Index
            GetSeletedNode = trv.Nodes(upLevel0Index)
        ElseIf trv.SelectedNode.Level = 1 Then
            upLevel0Index = trv.SelectedNode.Index
            upLevel1Index = trv.SelectedNode.Parent.Index
            GetSeletedNode = trv.Nodes(upLevel1Index).Nodes(upLevel0Index)
        ElseIf trv.SelectedNode.Level = 2 Then
            upLevel0Index = trv.SelectedNode.Index
            upLevel1Index = trv.SelectedNode.Parent.Index
            upLevel2Index = trv.SelectedNode.Parent.Parent.Index
            GetSeletedNode = trv.Nodes(upLevel2Index).Nodes(upLevel1Index).Nodes(upLevel0Index)
        ElseIf trv.SelectedNode.Level = 3 Then
            upLevel0Index = trv.SelectedNode.Index
            upLevel1Index = trv.SelectedNode.Parent.Index
            upLevel2Index = trv.SelectedNode.Parent.Parent.Index
            upLevel3Index = trv.SelectedNode.Parent.Parent.Parent.Index
            GetSeletedNode = trv.Nodes(upLevel3Index).Nodes(upLevel2Index).Nodes(upLevel1Index).Nodes(upLevel0Index)
        End If
    End Function
End Module