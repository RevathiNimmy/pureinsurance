Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.Windows.Forms

Imports SharedFiles

Public Module MainModule
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

#If quoteTiming Then

	Declare Function QueryPerformanceCounter Lib "kernel32" (x As Currency) As Boolean
	Declare Function QueryPerformanceFrequency Lib "kernel32" (x As Currency) As Boolean
	'***********************************************
	' TO DO: GLOBAL VARIABLE IN BAS MODULE
	Public performanceCtr(10000, 2) As Variant, performanceFreq As Currency
	'***********************************************
	'***********************************************
	' TO DO: GLOBAL VARIABLE IN BAS MODULE
	Public performancecntrCntr As Integer
	'***********************************************
#End If
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date: 23/06/1998
    '
    ' Description: Main module containing public variable/constants.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "uctRiskScreenControl"
    Public Const ACStatusActWebLoading As String = "Loading..........."

    '***********************************************
    ' TO DO: GLOBAL VARIABLE IN BAS MODULE
    '' Public interface constants used when
    '***********************************************
    ' retrieving data from the resource file.

    ' {* USER DEFINED CODE (Begin) *}

    ' General Icons

    ' Form
    Public Const ACInterfaceTitle As Integer = 100
    Public Const ACTabTitle1 As Integer = 101
    Public Const ACTabTitle2 As Integer = 102
    Public Const ACTabTitle3 As Integer = 103
    Public Const ACTabTitle4 As Integer = 104
    Public Const ACTabTitle5 As Integer = 105

    ' Buttons
    Public Const ACOKButton As Integer = 200
    Public Const ACCancelButton As Integer = 201
    Public Const ACHelpButton As Integer = 202
    Public Const ACNavigateButton As Integer = 203
    Public Const ACAddButton As Integer = 204
    Public Const ACEditButton As Integer = 205
    Public Const ACDeleteButton As Integer = 206
    Public Const ACLookupButton As Integer = 207
    Public Const ACProspectButton As Integer = 208
    Public Const ACMaintainButton As Integer = 209

    ' Messages
    Public Const ACCancelDetailsTitle As Integer = 300
    Public Const ACCancelDetails As Integer = 301
    Public Const ACBusinessFailTitle As Integer = 302
    Public Const ACBusinessFail As Integer = 303
    Public Const ACWarningUpdateRiskPolicies1 As Integer = 309
    Public Const ACWarningUpdateRiskPolicies2 As Integer = 310
    Public Const ACWarningCannotDeleteAssPerson As Integer = 311
    Public Const ACQuestionDeleteAssPerson As Integer = 312

    ' PW281003 - CQ2772
    Public Const ACErrorUpdatingRiskStatus As Integer = 313


    'Start -(Arul Stephen)-(TechSpec WR6 ClauseGrouping.doc)-(5.1.2)
    Public Const ACProductTypeNotExist As Integer = 0
    Public Const ACSelectClauseId As Integer = 0
    Public Const ACSelectClauseCode As Integer = 1
    Public Const ACSelectClauseDescription As Integer = 2
    Public Const ACSelectClauseArrayIndex As Integer = 3
    Public Const ACSelectClauseRowIndex As Integer = 2
    Public Const ACSelectClauseArrayDefaultIndex As Integer = 0
    Public Const ACScreenIndex As Integer = 2
    Public Const ACListViewTagValue As Integer = 10000
    Public Const ACTabIndex As Integer = 0
    Public Const ACObjectIndex As Integer = 1
    Public Enum ENClauseType
        ProductType = 1
        RiskType = 2
    End Enum
    'End -(Arul Stephen)-(TechSpec WR6 ClauseGrouping.doc)-(5.1.2)


    ' Menus


    ' {* USER DEFINED CODE (End) *}

    '***********************************************
    ' TO DO: GLOBAL VARIABLE IN BAS MODULE
    '' Public contants used for the start
    '***********************************************
    ' and end control indexes.
    Public Const ACControlStart As Integer = 0
    Public Const ACControlEnd As Integer = 1

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    '***********************************************
    ' TO DO: GLOBAL VARIABLE IN BAS MODULE
    '' Public source and language ID's from the
    '***********************************************
    ' Object Manager.
    '***********************************************
    ' TO DO: GLOBAL VARIABLE IN BAS MODULE
    '' Public instance of the object manager.
    '***********************************************
    '***********************************************
    ' TO DO: GLOBAL VARIABLE IN BAS MODULE

    '***********************************************
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iUserID As Integer
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_sUsername As String = ""
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_sPassword As String = ""

    Public Const ScreenHelpID As Integer = 3


    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

    'Array constants for the Data Dictionary
    Public Const ACOGISObjectId As Integer = 0
    Public Const ACOGISDataModelId As Integer = 1
    Public Const ACOObjectName As Integer = 2
    Public Const ACOTableName As Integer = 3
    Public Const ACOMaxInstances As Integer = 4
    Public Const ACOIsQuoteObject As Integer = 5
    Public Const ACOParentObjectId As Integer = 6
    Public Const ACOPolarisObjectId As Integer = 7
    Public Const ACOIsSelectable As Integer = 8
    Public Const ACOIsNonGIS As Integer = 9
    Public Const ACPGISPropertyId As Integer = 10
    Public Const ACPGISObjectId As Integer = 11
    Public Const ACPPropertyName As Integer = 12
    Public Const ACPColumnName As Integer = 13
    Public Const ACPDataType As Integer = 14
    Public Const ACPIsInputProperty As Integer = 15
    Public Const ACPIsIdentifyingProperty As Integer = 16
    Public Const ACPIsPrimaryKey As Integer = 17
    'GSD
    'Public Const ACPGISListId = 18
    Public Const ACPPolarisPropertyId As Integer = 18
    Public Const ACPIsDeleted As Integer = 19
    Public Const ACPIsSearchProperty As Integer = 20
    'Public Const ACPLookupTableName = 22
    'Public Const ACPPartyTypeId = 23
    'Public Const ACPSumInsuredType = 24
    'Public Const ACPStdWordingType = 25
    'Public Const ACPGISUserDefHeaderId = 26
    'Public Const ACPProductId = 27
    Public Const ACPEditFlags As Integer = 21
    Public Const ACPSpecialsType As Integer = 22
    Public Const ACPSpecialsTypeReference As Integer = 23
    Public Const ACO2GISObjectId As Integer = 24
    Public Const ACUParent As Integer = 25

    'Array constants for the Risk Type
    Public Const ACRRiskTypeId As Integer = 0
    Public Const ACRRiskFolderTypeId As Integer = 1
    Public Const ACRCaptionId As Integer = 2
    Public Const ACRCode As Integer = 3
    Public Const ACRDescription As Integer = 4
    Public Const ACREffectiveDate As Integer = 5
    Public Const ACRIsDeleted As Integer = 6
    Public Const ACRVarDataStructureId As Integer = 7
    Public Const ACRInterfaceObjectName As Integer = 8
    Public Const ACRInterfaceClassName As Integer = 9
    Public Const ACROverridePerilRiBand As Integer = 10
    Public Const ACROverridePerilXlBand As Integer = 11
    Public Const ACRNBPremiumProRateTypeId As Integer = 12
    Public Const ACRMTAPremiumProRateTypeId As Integer = 13
    Public Const ACRRNPremiumProRateTypeId As Integer = 14
    Public Const ACRIsShareWithCoinsurers As Integer = 15
    Public Const ACRIsShareWithReinsurers As Integer = 16
    Public Const ACRIsSuppressPublicText As Integer = 17
    Public Const ACRIsSuppressPrivateText As Integer = 18
    Public Const ACRIsSuppressTaxes As Integer = 19
    Public Const ACRReportPointer As Integer = 20
    Public Const ACRSectionMask As Integer = 21
    Public Const ACRStampDutyRate1 As Integer = 22
    Public Const ACRStampDutyRate2 As Integer = 23
    Public Const ACRPrimarySort As Integer = 24
    Public Const ACRSecondarySort As Integer = 25
    Public Const ACRHeaderClause As Integer = 26
    Public Const ACRTrailerClause As Integer = 27
    Public Const ACRIsRiAtRiskLevel As Integer = 28
    Public Const ACRIsAutoReinsured As Integer = 29
    Public Const ACRHeaderClauseId As Integer = 30
    Public Const ACRTrailerClauseId As Integer = 31
    Public Const ACRAccumulationLevel As Integer = 32
    Public Const ACRGISScreenId As Integer = 33
    Public Const ACRAssociatedClientScreenId As Integer = 34
    Public Const ACRDisclosureScreenId As Integer = 35


    '***********************************************
    ' TO DO: GLOBAL VARIABLE IN BAS MODULE
    'Public g_oGIS As Object
    '***********************************************
    '***********************************************
    ' TO DO: GLOBAL VARIABLE IN BAS MODULE
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_bGetEnableClaimVersions As Boolean
    '***********************************************

    '***********************************************
    ' TO DO: GLOBAL VARIABLE IN BAS MODULE
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_bIsInIDE As Boolean ' RAW 09/07/2004 : JIT : added
    '***********************************************

    '***********************************************
    ' TO DO: GLOBAL VARIABLE IN BAS MODULE
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_sScreenDesc As String = "" ' RAW 09/07/2004 : JIT : added
    '***********************************************


    Declare Function ShellExecute Lib "shell32.dll" Alias "ShellExecuteA" (ByVal hwnd As Integer, ByVal lpOperation As String, ByVal lpFile As String, ByVal lpParameters As String, ByVal lpDirectory As String, ByVal nShowCmd As Integer) As Integer
    ' RAW 09/07/2004 : JIT : added
#If DebugOption Then
    Public Sub AddToDebug(ByRef r_lDepthCounter As Integer, Optional ByVal v_sText As String = "")

        ' Watchout - enabling this function via #DebugOption will add a processing overhead
        ' #DebugToClip will still be activated for run time execution - so remove the compilation option when building for release

        Static lDepthCounter As Double

        Static dLastTimer As Double
        Dim dThisTimer As Double
        Dim sDuration As String = ""

        If Not g_bIsInIDE Then
            ' we are running an exe
#If DebugToClip Then

			' eventhough this is an exe, we still want to debug because we are writing to the clipboard.
#Else
            ' debugging does not apply in this mode
            Exit Sub
#End If
        End If

        If r_lDepthCounter < 0 Then
            ' if r_lDepthCounter is negative then this is an instruction to decrease the depth by 1
            ' starting from the absolute value of r_lDepthCounter
            lDepthCounter = Math.Abs(r_lDepthCounter) - 1
            Exit Sub
        ElseIf r_lDepthCounter > 0 Then
            ' use the incoming value of depth counter
            lDepthCounter = r_lDepthCounter
        Else
            ' depth counter = 0
            ' ignore the incoming value of depth counter and increase the depth counter from the highest reached so far
            lDepthCounter += 1
        End If

        r_lDepthCounter = CInt(lDepthCounter)

        v_sText = v_sText.Trim()

        ' indent text as appropriate according to the depth
        v_sText = New String(" ", (lDepthCounter - 1) * 2) & CStr(lDepthCounter) & ": " & v_sText

        dThisTimer = DateTime.Now.TimeOfDay.TotalSeconds
        If dLastTimer = 0 Then dLastTimer = dThisTimer
        sDuration = StringsHelper.Format(dThisTimer - dLastTimer, "00.00")
        v_sText = (IIf(sDuration = "00.00", "     ", sDuration)) & " : " & g_sScreenDesc.Substring(0, 30) & " : " & v_sText
        dLastTimer = DateTime.Now.TimeOfDay.TotalSeconds

        Debug.WriteLine(v_sText)

#If DebugToClip Then

		On Error Resume Next
		Clipboard.SetText Right$(Clipboard.GetText, 500000) & vbCrLf & v_sText
		On Error GoTo 0
#End If


    End Sub
#End If


    ' ***************************************************************** '
    '
    ' Name: lvwListViewColumnClick
    '
    ' Description:
    '
    ' History: 07/01/2003 CLG - Created.
    '
    ' ***************************************************************** '
    Public Sub lvwListViewColumnClick(ByRef lvwListView() As Object, ByVal Index As Integer, ByRef ColumnHeader As ColumnHeader)

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".lvwListViewColumnClick")

        Try

            Static lastIndex, lastColumn As Integer
            Static lastOrder As SortOrder
            Dim iOrder As SortOrder


            If lastIndex = Index And lastColumn = ColumnHeader.Index + 1 Then
                iOrder = Math.Abs(lastOrder - 1)
            Else
                iOrder = SortOrder.Ascending
            End If



            lvwListView(Index).Sorted = True
            If ColumnHeader.Text.ToLower().IndexOf("date") + 1 Then

                ListViewFunc.ListViewSortByDate(lvwListView(Index), ColumnHeader.Index + 1 - 1, iOrder)
            Else

                lvwListView(Index).Sorted = False

                lvwListView(Index).SortOrder = iOrder

                lvwListView(Index).SortKey = ColumnHeader.Index + 1 - 1

                lvwListView(Index).Sorted = True
            End If

            lastIndex = Index
            lastColumn = ColumnHeader.Index + 1
            lastOrder = iOrder




            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".lvwListViewColumnClick")

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".lvwListViewColumnClick")


            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="lvwListViewColumnClick Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwListViewColumnClick", excep:=excep)

            Exit Sub

        End Try

    End Sub
End Module
