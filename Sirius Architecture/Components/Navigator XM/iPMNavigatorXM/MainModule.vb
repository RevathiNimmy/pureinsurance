Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System

Imports SharedFiles
Module MainModule

    ' You set APPDEBUG in the project properties!!!!

    Public Declare Function ShellExecute Lib "shell32.dll" Alias "ShellExecuteA" (ByVal hwnd As Integer, ByVal lpOperation As String, ByVal lpFile As String, ByVal lpParameters As String, ByVal lpDirectory As String, ByVal nShowCmd As Integer) As Integer

    Public m_lError As Integer

    ' Root node in the tree
    Public Const ACRootNode As String = "NODE0"

    ' The class
    Private Const ACClass As String = "MainModule"
    Public Const ACApp As String = "iPMNavigatorXM"
    Public objfrmDebug As frmDebug
    ' Map Array
    Public Const ACMapWMTaskCode As Integer = 0
    Public Const ACMapWMTaskDesc As Integer = 1
    Public Const ACMapImageURL As Integer = 2
    Public Const ACMapTransactionType As Integer = 3
    Public Const ACMapProcessMode As Integer = 4
    Public Const ACMapRoadmapName As Integer = 5
    Public Const ACMapAutoClose As Integer = 6
    Public Const ACMapNavigatorDriven As Integer = 7
    Public Const ACMapTitle As Integer = 8
    Public Const ACMapResetKeysOnRestart As Integer = 9 'PN17474
    'Public Const ACMapCaptionId As Integer = 10
    Public Const ACMapResourceId As Integer = 10

    Public Const ACMapArraySize As Integer = 10

    ' Icons
    Public Const ACIconFindForm As String = "StepFind"
    Public Const ACIconDataForm As String = "StepDataForm"
    Public Const ACIconQuestion As String = "StepDecision"
    Public Const ACIconNavigate As String = "Process"
    Public Const ACIconBusiness As String = "StepNoForm"
    Public Const ACIconPrint As String = "StepPrint"
    Public Const ACIconSubMap As String = "SubMap"

    ' RDC 19062003 for secondary steps
    Public Const ACIconDiary As String = "Diary"
    Public Const ACIconEditText As String = "EditText"
    Public Const ACIconRaiseEvent As String = "RaiseEvent"
    Public Const ACIconStandardLetter As String = "StandardLetter"
    Public Const ACIconLaunchEXE As String = "LaunchEXE"
    Public Const ACIconUserComponent As String = "UserComponent"

    ' Panels
    Public Const ACPanelStatus As String = "lblstatus"
    Public Const ACPanelNavType As String = "navtype"

    ' Nav Types
    Public Const ACNavigatorV3 As String = "navigatorv3"
    Public Const ACNavigatorV2 As String = "navigatorv2"
    Public Const ACAutoForCL As String = "autoforcl"

    ' RDC 19112002
    ' Navigator XM registry settings
    Public Const ACNavigatorXMPath As String = "NavigatorXMPath"
    Public Const ACNavigatorXMLogo As String = "NavigatorXMLogo"

    ' Object Manager

    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager

    ' Our type. Just easier to group data this way instead of 2D array
    Public Structure Step_Renamed
        Dim Description As String
        Dim Component As String
        Dim Type As String
        Dim OKAction As String
        Dim CancelAction As String
        Dim OKSteps As Integer
        Dim CancelSteps As Integer
        Dim ComponentAction As Integer
        Dim ServerSide As Boolean
        Dim DefaultKeys As Object
        Dim CreateWorkManagerTask As Boolean
        Dim ResumeStep As Integer
        Dim StepID As Integer ' NEW 090702
        Dim ParentID As Integer ' NEW 110702
        Dim IsHeader As Boolean ' NEW 110702
        Dim OKNewRoadmap As String ' RFC 130603
        Dim CancelNewRoadmap As String ' RFC 130603
        Dim ShowWMTaskInterface As Boolean ' SET 27/01/2004
        Dim Action1Action As String
        Dim Action1Steps As Integer
        Dim Action2Action As String
        Dim Action2Steps As Integer

        Dim ResourceId As Integer

        Public Shared Function CreateInstance() As Step_Renamed
            Dim result As New Step_Renamed
            result.Description = String.Empty
            result.Component = String.Empty
            result.Type = String.Empty
            result.OKAction = String.Empty
            result.CancelAction = String.Empty
            result.OKNewRoadmap = String.Empty
            result.CancelNewRoadmap = String.Empty
            result.Action1Action = String.Empty
            result.Action2Action = String.Empty
            Return result
        End Function
    End Structure

    ' The steps

    <ThreadStatic()> _
    Public m_vSteps() As Step_Renamed = Nothing

    <ThreadStatic()> _
    Public m_vStepsTemp() As Step_Renamed = Nothing

    ' Global key array

    <ThreadStatic()> _
    Public m_vKeyArray As Object
    <ThreadStatic()> _
    Public m_vKeyArrayTemp As Object

    <ThreadStatic()> _
    Public m_vKeyArray_Dup As Object
    ' Current step

    <ThreadStatic()> _
    Public m_lCurrentStep As Integer
    <ThreadStatic()> _
    Public m_lCurrentStepTemp As Integer

    Public Const ACNormalMode As Integer = 0
    Public Const ACMergeMode As Integer = 1
    ' CTAF 130600 - Modes for printing
    Public Const ACPrintMode As Integer = 2
    Public Const ACPrintSilentMode As Integer = 3
    Public Const ACSpoolDocMode As Integer = 4
    Public Const ACSpoolReportMode As Integer = 5

    Public Const PMNavComponentPrintObject As String = "PO"

    Public Const ACResumeStepCurrent As Integer = -1

    'TF260202 - Transaction Types (from GIIConst.bas)
    'Global Const PMTransactionTypeNB = "G_NB"
    Public Const PMTransactionTypeMTA As String = "G_MTA"
    Public Const PMTransactionTypeReview As String = "G_REVIEW"
    Public Const PMTransactionTypeRenewals As String = "G_RENEW"
    Public Const PMTransactionTypeDefaults As String = "G_DEFAULTS"
    Public Const PMTransactionTypeMTAFullQuote As String = "G_MTA_FQ"

    ' Document codes
    Public Const PMDocumentNewBusinessQuoteDisplay As String = "NB_Q"

    ' SET 27/01/2004
    Public Const ACDefaultTaskDays As Integer = 7
    Public Const kUSLangId As Integer = 2
    Public Const kUKLangId As Integer = 1
    ' ***************************************************************** '
    '
    ' Name: CheckNav3
    '
    ' Description: Checks if an object is nav3 or nav2 compliant
    '
    ' History: 13/09/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    'Alkesh
    'Public Function CheckNav2or3(ByVal v_oObject As aPMNav.NavigatorV2, ByRef r_bNav2 As Boolean, ByRef r_bNav3 As Boolean) As Integer
    Public Function CheckNav2or3(ByVal v_oObject As Object, ByRef r_bNav2 As Boolean, ByRef r_bNav3 As Boolean) As Integer

        Dim result As Integer = 0
        Dim oNav2 As aPMNav.NavigatorV2
        Dim oNav3 As aPMNav.NavigatorV3

        'Added by Alkesh
        Dim iInforErr As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Reset status
        r_bNav2 = False
        r_bNav3 = False

        ' Commented by Alkesh
        'Try for nav2

        'Try
        '    oNav2 = v_oObject
        '   If Information.Err().Number = 0 Then
        '        r_bNav2 = True
        '    Else
        '        Information.Err().Clear()
        '         Try for nav3
        '        oNav3 = v_oObject
        '        If Information.Err().Number = 0 Then
        '            r_bNav3 = True
        '        End If
        '    End If

        Try
            oNav2 = v_oObject
        Catch
            iInforErr = Information.Err().Number
        End Try
        Try
            If iInforErr = 0 Then
                r_bNav2 = True
            Else
                iInforErr = 0
                ' Try for nav3
                Try
                    oNav3 = v_oObject
                Catch
                    iInforErr = Information.Err().Number
                End Try
                If iInforErr = 0 Then
                    r_bNav3 = True
                End If
            End If

            ' Reset error status and trapping
            Information.Err().Clear()

            Return result

Err_CheckNav2or3:

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckNav2or3 Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckNav2or3", excep:=New Exception(Information.Err().Description))

            Return result
            Return result

        Catch exc As System.Exception
            NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try
    End Function
End Module