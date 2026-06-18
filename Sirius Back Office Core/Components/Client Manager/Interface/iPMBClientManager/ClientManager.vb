Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.Text
Imports System.Windows.Forms
Imports SharedFiles
Partial NotInheritable Class MainModule
    '*** Global module for MDI Notepad sample application.  ***
    '**********************************************************
    '
    ' Edit History:
    '
    '       PW231204  - PN15756 - Changed ShowRecentFile to ask user if DPA info is
    '                   required, rather than using the stored registry setting.
    '       CJB010305 - PN19063 - Changed CallGeminiII to not proceed for schemes policies.
    '       CJB150305 - PN13544 - In order to get Client Statement report (at Policy Level or
    '                   Policy Version level) to work needed to cater for optional insfilecnt
    '                   param. Changed RunReport to cater for this.
    '       CJB230305 - PN19733 - Passing extra v_sInsReference param to ShowPolicyListVersion
    '                   so we can show pol. no. in title bar.
    '       CJB180405 - PN17033 - Cater for "&" being in shortname - was showing nothing for it
    '                   in the recent files menu. Changed AddRecentFile, SaveRecentFiles and
    '                   LoadRecentFilesFromReg & ShowRecentFile.
    '       CJB270605 - PN21979 - Changed ShowEventDetail to do nothing on CLICHANGED events
    '                   rather than show 'Unknown event' msgbox.
    '       JRD160105 - PN26814 - Allowed ClientManager to call NaXM maps, and ensured any
    '                   NavXM processes are correctly terminated.  Amended GII Navigator's to
    '                   call NavXM for SiriusQuotes systems
    '*******************************************************************************
    'Developer Guide No. 69(Guide)
    'start'
    ' Member to hold instance of form interface 
    <ThreadStatic()> _
    Public m_ofrmMDI As iPMBClientManager.frmMDI
    <ThreadStatic()> _
    Public m_sShortName As String
    'End
    ' Public interface constants used when
    ' retrieving data from the resource file.
    Public Const ACApp As String = "ClientManager"

    ' {* USER DEFINED CODE (Begin) *}

    ' General Icons

    ' Form
    Public Const ACInterfaceTitle As Integer = 100
    Public Const ACTabTitle1 As Integer = 101
    Public Const ACTabTitle2 As Integer = 102
    Public Const ACTabTitle3 As Integer = 103
    Public Const ACReference As Integer = 104
    Public Const ACPostcode As Integer = 105
    Public Const ACSurname As Integer = 106
    Public Const ACForename As Integer = 107
    Public Const ACResolved As Integer = 108
    Public Const ACOccupation As Integer = 109
    Public Const ACDOB As Integer = 110
    Public Const ACEmployer As Integer = 111
    Public Const ACTitle As Integer = 112
    Public Const ACInitials As Integer = 113
    Public Const ACfraAgent As Integer = 114
    Public Const ACAgentRef As Integer = 115
    Public Const ACAgentName As Integer = 116

    ' TF031298
    Public Const ACFinancial As Integer = 150
    Public Const ACPolicy As Integer = 151
    Public Const ACClaim As Integer = 152
    Public Const ACNotes As Integer = 153
    Public Const ACLetter As Integer = 154

    ' Buttons
    Public Const ACOKButton As Integer = 200
    Public Const ACCancelButton As Integer = 201
    Public Const ACHelpButton As Integer = 202
    Public Const ACNavigateButton As Integer = 203
    Public Const ACAddButton As Integer = 204
    Public Const ACEditButton As Integer = 205
    Public Const ACDeleteButton As Integer = 206

    ' Messages
    Public Const ACCancelDetailsTitle As Integer = 300
    Public Const ACCancelDetails As Integer = 301
    Public Const ACBusinessFailTitle As Integer = 302
    Public Const ACBusinessFail As Integer = 303

    Public Const ACEmployerMissing As Integer = 304
    Public Const ACAgentMissing As Integer = 305
    Public Const ACRefExists As Integer = 306
    ' Menus


    ' {* USER DEFINED CODE (End) *}

    ' Public contants used for the start
    ' and end control indexes.
    Public Const ACControlStart As Integer = 0
    Public Const ACControlEnd As Integer = 1

    Public Const ACRiskMode As Integer = 0
    Public Const ACMarketMode As Integer = 1

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' CTAF 270900
    Public Const ACSwiftCheckInstalled As Integer = 0
    Public Const ACSwiftFuncMatch As Integer = 1
    Public Const ACSwiftFuncClientManager As Integer = 2

    'Risk Groups
    Public Const ACRGPrivateMotor As Integer = 1
    Public Const ACRGHouseholdContents As Integer = 2
    Public Const ACRGHouseholdBuildings As Integer = 3
    Public Const ACRGHouseholdCombined As Integer = 4
    Public Const ACRGMotorFleet As Integer = 5
    Public Const ACRGPrivatePublicHire As Integer = 6
    Public Const ACRGTravel As Integer = 7
    Public Const ACRGMarine As Integer = 8
    Public Const ACRGAviation As Integer = 9
    Public Const ACRGCommercialCombined As Integer = 10
    Public Const ACRGShop As Integer = 11
    Public Const ACRGPersonalAccident As Integer = 12
    Public Const ACRGCombinedLiability As Integer = 13
    Public Const ACRGPropertyOwners As Integer = 14
    Public Const ACRGOffices As Integer = 15
    Public Const ACRGFarm As Integer = 16
    Public Const ACRGCombinedMotor As Integer = 17
    Public Const ACRGMotorcycle As Integer = 18

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
    Public g_iCurrencyId As Integer
    'Developer Guide No. 107
    <ThreadStatic()> _
    Public g_iUserId As Integer 'MKW070703 PN4026
    'Developer Guide No. 107
    <ThreadStatic()> _
    Public g_iCountryID As Integer 'eck Datasure
    'Developer Guide No. 107

    Public g_sUserName As String = ""

    ' Public instance of the object manager.
    <ThreadStatic()> _
    Public g_oObjectManager As bObjectManager.ObjectManager


    <ThreadStatic()> _
    Public g_oEvent As bSIREvent.Business '2005  Sticky Notes

    Public Const ScreenHelpID As Integer = 44000

    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
    <ThreadStatic()> _
    Public g_sUnderwritingOrAgency As String = ""

    '2005 Client Manager Security

    <ThreadStatic()> _
    Public g_oUserAuthorities As bACTUserAuthorities.Business
    <ThreadStatic()> _
    Public g_bEditClientAuthority As Boolean
    <ThreadStatic()> _
    Public g_bEditPolicyAuthority As Boolean
    <ThreadStatic()> _
    Public g_bDeletePolicyAuthority As Boolean 'PN23035
    <ThreadStatic()> _
    Public g_bEditClaimAuthority As Boolean
    <ThreadStatic()> _
    Public g_bEditFinancePlanAuthority As Boolean
    <ThreadStatic()> _
    Public g_bRaiseDebitAuthority As Boolean
    <ThreadStatic()> _
    Public g_bRaiseCreditAuthority As Boolean
    <ThreadStatic()> _
    Public g_bRaiseFeeAuthority As Boolean
    <ThreadStatic()> _
    Public g_bRaiseCashAuthority As Boolean
    <ThreadStatic()> _
    Public g_bReverseTransactionsAuthority As Boolean
    <ThreadStatic()> _
    Public g_bReverseAllocationsAuthority As Boolean
    <ThreadStatic()> _
    Public g_bRaiseManualDIDAuthority As Boolean
    <ThreadStatic()> _
    Public g_bEditSchemePolicyAuthority As Boolean



    ' ND 181000
    Public Const DME_CLIENT As String = "1"
    Public Const DME_POLICY As String = "2"

    'DC091100
    'Developer Guide No. 107
    <ThreadStatic()> _
    Public g_nREADMODE As Integer = gPMConstants.PMEComponentAction.PMView '0
    'Developer Guide No. 107
    <ThreadStatic()> _
    Public g_nADDMODE As Integer = gPMConstants.PMEComponentAction.PMAdd '1
    'Developer Guide No. 107
    <ThreadStatic()> _
    Public g_nEDITMODE As Integer = gPMConstants.PMEComponentAction.PMEdit '2
    'DC091100

    'JSB 06/06/01 - Constants for GIIprocesses,
    Public Const GIIProcessTypeNB As String = "1"
    Public Const GIIProcessTypeMaintain As String = "2"
    Public Const GIIProcessTypeMTA As String = "3"
    Public Const GIIProcessTypeRebroke As String = "4"
    Public Const GIIProcessTypeReview As String = "5"

    'SD 17/07/2002 - Constants for hidden ooptions
    Public Const OPTShowPartySummaryOff As String = "0"
    Public Const OPTShowPartySummaryOn As String = "1"


    'MDI Stuff

    ' User-defined type to store information about child forms
    Public Structure FormState
        Dim Deleted As Integer
        Dim Dirty As Integer
        Dim Color As Integer
    End Structure
    <ThreadStatic()> _
    Public FState() As FormState = Nothing ' Array of user-defined types
    <ThreadStatic()> _
    Public Document() As Object ' Array of child form objects
    <ThreadStatic()> _
    Public gFindString As String = "" ' Holds the search text.
    <ThreadStatic()> _
    Public gFindCase As Integer ' Key for case sensitive search
    <ThreadStatic()> _
    Public gFindDirection As Integer ' Key for search direction.
    <ThreadStatic()> _
    Public gCurPos As Integer ' Holds the cursor location.
    <ThreadStatic()> _
    Public gFirstTime As Integer ' Key for start position.
    <ThreadStatic()> _
    Public gToolsHidden As Boolean ' Holds toolbar state.
    Public Const ThisApp As String = "Client Manager" ' Registry App constant.
    Public Const ThisKey As String = "Recent Files" ' Registry Key constant.

    'SD 12/07/2002
    <ThreadStatic()> _
    Public gPartySummaryScreenShown As Boolean 'Flag to display the summary screen once only.

    Private m_lReturn As Integer

    ' Private instance of client manager manager
    <ThreadStatic()> _
    Public g_oCMManager As Object

    ' Number of files in the Recent menu
    <ThreadStatic()> _
    Public g_iMaxRecent As Integer

    ' CTAF 280900
    <ThreadStatic()> _
    Public g_bSwiftInstalled As Boolean

    ' PSA 26/07/00
    <ThreadStatic()> _
    Public g_lInsFileCnt As Integer
    ' PSA 26/07/00

    'AK 270401 - to store renewal event type
    Private m_bRenewalEvent As Boolean

    'JSB - to identify if we need to call GII
    <ThreadStatic()> _
    Public g_bCallGII As Boolean

    'sj 03/07/2002 - start
    ' RestrictInsurerAccess
    <ThreadStatic()> _
    Public g_bRestrictInsurerAccess As Boolean
    <ThreadStatic()> _
    Public g_lUserInsurerCnt As Integer
    'sj 03/07/2002 - end

    'sj 04/10/2002 - start

    <ThreadStatic()> _
    Public g_bHidePublicPrivateNotes As Boolean
    'sj 04/10/2002 - end
    <ThreadStatic()> _
    Public m_vSourceArray As Object 'MKW070703 PN4026

    'AR20050107 - PN17865 Change DPA flags to public globals
    <ThreadStatic()> _
    Public g_bDPAIsActive As Boolean ' PN15756
    <ThreadStatic()> _
    Public g_bDPAIsEnforced As Boolean ' PN15756
    <ThreadStatic()> _
    Public g_vWarnings As Object '2005 sticky notes

    'For PN-43232
    <ThreadStatic()> _
    Public g_sResolvedName As String = ""

    'PN26814 Local reference to iPMNavStart.Interface object - this is instanciated withevents in the
    'Interface class, as the NavigatorClose event needs handling to ensure the object is
    'correctly destroyed
    Private m_objPMNavStart As iPMNavStart.Interface_Renamed
    'PN26814 Local boolean to determine if iPMNavStart.Interface object is currently running a NavXM process
    'This is required to stop the User from closing Client Manager before the NavigatorClose event
    'is handled - otherwise an orphaned out of process NavXM process will remain.
    <ThreadStatic()> _
    Private m_bIsPMNavStartRunning As Boolean
    Private m_aoPartySearchData(,) As Object = Nothing
    'Party View
    <ThreadStatic()> _
    Public g_bIsViewOnlyClientManager As Boolean

    Public Property PMNavStart() As iPMNavStart.Interface_Renamed
        Get
            Return m_objPMNavStart
        End Get
        Set(ByVal Value As iPMNavStart.Interface_Renamed)
            m_objPMNavStart = Value
        End Set
    End Property

    Public Property IsNavStartRunning() As Boolean
        Get
            Return m_bIsPMNavStartRunning
        End Get
        Set(ByVal Value As Boolean)
            m_bIsPMNavStartRunning = Value
        End Set
    End Property

    Public Function AnyPadsLeft() As Integer

        ' Counter variable

        ' Cycle through the document array.
        ' Return true if there is at least one open document.
        For i As Integer = 1 To Document.GetUpperBound(0)
            If Not FState(i).Deleted Then
                Return True
            End If
        Next

    End Function

    Sub FileNew(ByRef vPartyType As String)

        ' Find the next available index and show the child form.
        Dim fIndex As Integer = FindFreeIndex()
        Select Case vPartyType
            Case "P"
                Document(fIndex) = New frmPartyPC(m_ofrmMDI)
            Case "C"
                Document(fIndex) = New frmPartyCC(m_ofrmMDI)
            Case "G"
                Document(fIndex) = New frmPartyGC(m_ofrmMDI)
        End Select

        Document(fIndex).ModuleClass = Me

        Document(fIndex).Tag = fIndex
        Document(fIndex).ShortName = m_sShortName
        Document(fIndex).Text = "Client:" & fIndex
        'Document(fIndex).Caption = "Personal Client : [" & Trim$(Me.ResolvedName) & "]"

        Document(fIndex).Footer = "Client:" & fIndex

        Document(fIndex).Index = fIndex
        m_ofrmMDI.StatusBar1.Items.Item(0).Text = "Client:" & fIndex
        m_ofrmMDI.StatusBar1.Items.Item(1).Text = ""
        m_ofrmMDI.StatusBar1.Items.Item(2).Text = ""
        'm_ofrmMDI.Caption = "Policy Master Client Manager : [" & Trim$(Me.ResolvedName) & "]"

        Document(fIndex).WindowState = FormWindowState.Maximized

        Document(fIndex).LoadInterface()

        Document(fIndex).Show()
        ' Make sure the toolbar edit buttons are visible.

    End Sub

    Function FindFreeIndex() As Integer

        Dim result As Integer = 0

        Dim ArrayCount As Integer = Document.GetUpperBound(0)

        ' Cycle through the document array. If one of the
        ' documents has been deleted, then return that index.
        For i As Integer = 1 To ArrayCount
            If FState(i).Deleted Then
                result = i
                FState(i).Deleted = False
                Return result
            End If
        Next

        ' If none of the elements in the document array have
        ' been deleted, then increment the document and the
        ' state arrays by one and return the index to the
        ' new element.
        ReDim Preserve Document(ArrayCount + 1)
        ReDim Preserve FState(ArrayCount + 1)
        Return Document.GetUpperBound(0)

    End Function

    Sub FindIt()

        Dim intPos As Integer
        Dim strFindString, strSourceString, strMsg As String
        Dim intResponse As DialogResult
        Dim intOffset As Integer

        ' Set offset variable based on cursor position.

        'NIIT - Replaced with the Migrated code 1144
        'If gCurPos = m_ofrmMDI.ActiveMdiChild.ActiveControl.SelStart Then
        If gCurPos = ReflectionHelper.GetMember(m_ofrmMDI.ActiveMdiChild.ActiveControl, "SelStart") Then
            intOffset = 1
        Else
            intOffset = 0
        End If

        ' Read the public variable for start position.
        If gFirstTime Then intOffset = 0
        ' Assign a value to the start value.

        'NIIT - Replaced with the Migrated code 1144
        'Dim intStart As Integer = m_ofrmMDI.ActiveMdiChild.ActiveControl.SelStart + intOffset
        Dim intStart As Integer = ReflectionHelper.GetMember(m_ofrmMDI.ActiveMdiChild.ActiveControl, "SelStart") + intOffset

        ' If not case sensitive, convert the string to upper case
        If gFindCase Then
            strFindString = gFindString

            strSourceString = m_ofrmMDI.ActiveMdiChild.ActiveControl.Text
        Else
            strFindString = gFindString.ToUpper()

            strSourceString = m_ofrmMDI.ActiveMdiChild.ActiveControl.Text.ToUpper()
        End If

        ' Search for the string.
        If gFindDirection = 1 Then
            intPos = Strings.InStr(intStart + 1, strSourceString, strFindString)
        Else
            For intPos = intStart - 1 To 0 Step -1
                If intPos = 0 Then
                    Exit For
                End If
                If strSourceString.Substring(intPos - 1, Math.Min(strSourceString.Length, strFindString.Length)) = strFindString Then
                    Exit For
                End If
            Next
        End If

        ' If the string is found...
        If intPos Then

            'NIIT - Replaced with the Migrated code 1144
            'm_ofrmMDI.ActiveMdiChild.ActiveControl.SelStart = intPos - 1
            ReflectionHelper.SetMember(m_ofrmMDI.ActiveMdiChild.ActiveControl, "SelStart", intPos - 1)

            'NIIT - Replaced with the Migrated code 1144
            'm_ofrmMDI.ActiveMdiChild.ActiveControl.SelLength = strFindString.Length
            ReflectionHelper.SetMember(m_ofrmMDI.ActiveMdiChild.ActiveControl, "SelLength", strFindString.Length)
        Else
            strMsg = "Cannot find " & Strings.Chr(34).ToString() & gFindString & Strings.Chr(34).ToString()
            intResponse = MessageBox.Show(strMsg, My.Application.Info.Title, MessageBoxButtons.OK)
        End If

        ' Reset the public variables

        'NIIT - Replaced with the Migrated code 1144
        'gCurPos = m_ofrmMDI.ActiveMdiChild.ActiveControl.SelStart
        gCurPos = ReflectionHelper.GetMember(m_ofrmMDI.ActiveMdiChild.ActiveControl, "SelStart")
        gFirstTime = False

    End Sub

    ' ***************************************************************** '
    '
    '   *** THIS FUNCTION RIPPED FROM GIIFUNC, IF THAT MODULE IS      ***
    '   *** ADDED TO THIS PROJECT THEN REMOVE THIS FUNCTION FROM HERE ***
    '
    '
    ' Name: DisableFormCloseButton
    '
    ' Description: Disables 'X' button on the form using an API call
    '              Requires the form title to be passed through
    '
    ' History: 25/05/2000 JSB - Created.
    '
    ' ***************************************************************** '
    Public Function DisableFormCloseButton(ByVal sFormWindowName As String) As Integer

        Dim result As Integer = 0
        Dim hwndHandle, hMenuHandle, hClose As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            hwndHandle = GIIConstants.FindWindow(0, sFormWindowName)
            If hwndHandle <> 0 Then
                'Developer Guide No. 170
                hMenuHandle = GIIConstants.GetSystemMenu(hwndHandle, False)
                If hMenuHandle <> 0 Then
                    'Developer Guide No. 170
                    hClose = GIIConstants.DeleteMenu(hMenuHandle, GIIConstants.SC_CLOSE, GIIConstants.MF_BYCOMMAND)
                End If
            Else
                MessageBox.Show(sFormWindowName & " window not found", Application.ProductName)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DisableFormCloseButton Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableFormCloseButton", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Public Function GetRecentFiles() As Integer

        ' This procedure demonstrates the use of the GetAllSettings function,
        ' which returns an array of values from the Windows registry. In this
        ' case, the registry contains the files most recently opened.  Use the
        ' SaveSetting statement to write the names of the most recent files.
        ' That statement is used in the WriteRecentFiles procedure.

        Dim result As Integer = 0
        Dim iTemp As Integer
        Dim varFiles As Object ' Variable to store the returned array.
        Dim sTemp, sShortName, sLongName As String




        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get recent files from the registry using the GetAllSettings statement.
        ' ThisApp and ThisKey are constants defined in this module.

        If String.IsNullOrEmpty(Interaction.GetSetting(ThisApp, ThisKey, "RecentFile1", )) Then
            Return result
        End If


        varFiles = Interaction.GetAllSettings(ThisApp, ThisKey)


        For i As Integer = 0 To varFiles.GetUpperBound(0)

            ' Only show the maximum number of allowed recent files.
            If i >= g_iMaxRecent Then
                Exit For
            End If

            'MKR 29/09/2004 PN 6021 'We are now using '|' as a delimiter rather than ','


            sTemp = CStr(varFiles(i, 1))
            iTemp = (sTemp.IndexOf("|"c) + 1)
            sShortName = sTemp.Substring(0, iTemp - 1)
            sTemp = sTemp.Substring(iTemp)
            iTemp = (sTemp.IndexOf("|"c) + 1)
            sLongName = sTemp.Substring(0, iTemp - 1)
            m_ofrmMDI.mnuRecentFile(0).Available = True
            'm_ofrmMDI.mnuRecentFile(i).Caption = Left(varFiles(i, 1), InStr(varFiles(i, 1), ",") - 1)
            'm_ofrmMDI.mnuRecentFile(i).Tag = Right(varFiles(i, 1), (Len(varFiles(i, 1)) - InStr(varFiles(i, 1), ",")))

            Try
                'Developer Guide No. 69(Guide)
                ContainerHelper.LoadControl(m_ofrmMDI, "mnuRecentFile", i + 1)

            Catch
            End Try



            m_ofrmMDI.mnuRecentFile(i + 1).Text = sShortName
            'm_ofrmMDI.mnuRecentFile(i).Tag = Right$(sTemp, (Len(sTemp) - iTemp))


            m_ofrmMDI.mnuRecentFile(i + 1).Tag = varFiles(i, 1)
            m_ofrmMDI.mnuRecentFile(i + 1).Available = True

            ' Iterate through all the documents and update each menu.
            For j As Integer = 1 To Document.GetUpperBound(0)
                If Not FState(j).Deleted Then

                    Document(j).mnuRecentFile(0).Visible = True

                    Try
                        'TODO
                        'ContainerHelper.LoadControl(ReflectionHelper.Invoke(Document(j), "mnuRecentFile", New Object() {i + 1})) 

                    Catch
                    End Try




                    Document(j).mnuRecentFile(i + 1).Visible = True
                    'Document(j).mnuRecentFile(i + 1).Caption = Left(varFiles(i, 1), InStr(varFiles(i, 1), ",") - 1)
                    'Document(j).mnuRecentFile(i + 1).Tag = Right(varFiles(i, 1), (Len(varFiles(i, 1)) - InStr(varFiles(i, 1), ",")))

                    Document(j).mnuRecentFile(i + 1).Caption = sShortName


                    Document(j).mnuRecentFile(i + 1).Tag = varFiles(i, 1)

                    Document(j).mnuRecentFile(i + 1).Visible = True
                End If
            Next j

        Next i

        Return result

Err_GetRecentFiles:

        result = gPMConstants.PMEReturnCode.PMError

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRecentFiles failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRecentFiles", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function

    Public Function WriteRecentFiles(ByRef vFileName As String, ByRef vId As Integer) As Integer

        ' This procedure uses the SaveSettings statement to write the names of
        ' recently opened files to the System registry. The SaveSetting
        ' statement requires three parameters. Two of the parameters are
        ' stored as constants and are defined in this module.  The GetAllSettings
        ' function is used in the GetRecentFiles procedure to retrieve the
        ' file names stored in this procedure.

        Dim result As Integer = 0
        Dim strFile, key As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Copy RecentFile1 to RecentFile2, and so on.
            If vId = g_iMaxRecent Or vId = 0 Then

                For i As Integer = g_iMaxRecent To 1 Step -1
                    key = "RecentFile" & i

                    strFile = Interaction.GetSetting(ThisApp, ThisKey, key, )
                    If strFile.Length <> 0 Then
                        key = "RecentFile" & (CStr(i + 1))
                        Interaction.SaveSetting(ThisApp, ThisKey, key, strFile)
                    End If
                Next i

            Else

                For i As Integer = vId - 1 To 1 Step -1
                    key = "RecentFile" & i

                    strFile = Interaction.GetSetting(ThisApp, ThisKey, key, )
                    If strFile.Length <> 0 Then
                        key = "RecentFile" & (CStr(i + 1))
                        Interaction.SaveSetting(ThisApp, ThisKey, key, strFile)
                    End If
                Next i

            End If

            strFile = vFileName
            Interaction.SaveSetting(ThisApp, ThisKey, "RecentFile1", strFile)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="WriteRecentFiles failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="WriteRecentFiles", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CallCMManager
    '
    ' Description: Calls CMManager to handle opening a new client,
    '              or opens the client in the current CM if it's empty.
    '
    ' ***************************************************************** '
    Public Function CallCMManager(ByVal v_lCurrentPartyCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_sPartyShortName As String, ByVal v_sPartyResolvedName As String, ByVal v_sPartyType As String) As Integer

        Dim result As Integer = 0
        Dim bIsOpen As Boolean
        Dim iCount As Integer
        Dim bIsVisible, bProceed As Boolean
        Dim iReply As DialogResult ' PN15756.
        Dim bCanOpen As Boolean
        Dim lMaxCMs As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if new partycnt is visible to user.
            m_lReturn = IsClientVisible(v_lPartyCnt, bIsVisible)
            If Not (bIsVisible) Then
                MessageBox.Show("Unable to view " & v_sPartyShortName, "Client Manager", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get party type from database in case it has changed.
            m_lReturn = GetPartyType(v_lPartyCnt, v_sPartyType)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Unable to view " & v_sPartyShortName, "Client Manager", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get party short and resolved names from database in case it has changed.
            m_lReturn = GetName(v_lPartyCnt, v_sPartyShortName, v_sPartyResolvedName)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Unable to view " & v_sPartyShortName, "Client Manager", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            bProceed = False

            ' Check if it's already open elsewhere

            m_lReturn = g_oCMManager.IsOpen(v_lPartyCnt:=v_lPartyCnt, r_bIsOpen:=bIsOpen)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check if the max. limit of Open CMs is reached    'PN 20162

            m_lReturn = g_oCMManager.CheckCMAvailability(r_bCanOpen:=bCanOpen, r_lMaxCMs:=lMaxCMs)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not bCanOpen And Not bIsOpen Then
                ' Show a message saying the maximum number of client managers
                ' has been reached
                MessageBox.Show("You have reached the maximum number of" & Environment.NewLine & _
                                "allowed Client Managers (" & CStr(lMaxCMs) & ").", "Limit reached", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Return gPMConstants.PMEReturnCode.PMTrue

            End If

            Dim sServiceLevel As String = String.Empty
            Const KPartyServiceLevelCode As Integer = 33
            'Check the service level for personal and group client. 
            If m_aoPartySearchData IsNot Nothing AndAlso m_aoPartySearchData.GetUpperBound(0) >= KPartyServiceLevelCode Then
                sServiceLevel = Convert.ToString(m_aoPartySearchData(KPartyServiceLevelCode, 0)).Trim().ToUpper()
                Dim dResult As DialogResult
                If sServiceLevel = "RESTRICTED" Then
                    dResult = MessageBox.Show("Client is Restricted, do you want to continue?", "Service Level Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)
                    If dResult <> Windows.Forms.DialogResult.OK Then
                        Exit Function
                    End If
                ElseIf sServiceLevel = "OBJECTED" Then
                    dResult = MessageBox.Show("Client has objected, do you want to continue?", "Service Level Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)
                    If dResult <> Windows.Forms.DialogResult.OK Then
                        Exit Function
                    End If
                End If
            End If
            'Ammended so that DPA info should be asked only if the client is not open.
            If (v_sPartyType = "P" Or v_sPartyType = "C") And Not bIsOpen Then

                ' Ask user if DPA information is required. PN15756.

                'JAS 07012005 PN15756 - check the DPA Settings
                GetDPASettings()

                If g_bDPAIsEnforced Then
                    iReply = System.Windows.Forms.DialogResult.Yes
                ElseIf g_bDPAIsActive Then
                    iReply = MessageBox.Show("Is DPA information required for " & _
                             v_sPartyShortName & "?", "DPA", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                Else
                    iReply = System.Windows.Forms.DialogResult.No
                End If

                Select Case iReply
                    Case System.Windows.Forms.DialogResult.Yes
                        m_lReturn = ProcessFSAAccess(lPartyCnt:=v_lPartyCnt, sPartyType:=v_sPartyType, bProceed:=bProceed)
                    Case System.Windows.Forms.DialogResult.No
                        bProceed = True
                End Select

            Else
                bProceed = True
            End If

            If bProceed Then

                ' Check how many windows are open
                iCount = 0
                For i As Integer = 0 To Application.OpenForms.Count - 1
                    If Application.OpenForms.Item(i).Name <> m_ofrmMDI.Name Then
                        iCount += 1
                    End If
                Next i

                If (bIsOpen) Or (iCount > 0) Then

                    ' Set the properties

                    g_oCMManager.CallingAppName = ACApp

                    g_oCMManager.PartyCnt = v_lPartyCnt

                    g_oCMManager.PartyShortName = v_sPartyShortName

                    g_oCMManager.PartyResolvedName = v_sPartyResolvedName

                    g_oCMManager.PartyType = v_sPartyType

                    ' Launch it

                    m_lReturn = g_oCMManager.Start()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse

                        ' Log Error Message
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start Client Manager Manager.", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowClientManager", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    End If

                Else


                    m_lReturn = g_oCMManager.ChangeParty(v_lOldPartyCnt:=v_lCurrentPartyCnt, v_lNewPartyCnt:=v_lPartyCnt)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        ' Log Error Message
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update change of party.", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowClientManager", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    End If

                    ' Open it in this client manager
                    m_lReturn = OpenSummaryFile(vPartyCnt:=v_lPartyCnt, vPartyShortName:=v_sPartyShortName, vPartyType:=v_sPartyType, vPartyResolvedName:=v_sPartyResolvedName)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        ' Log Error Message
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to open summary file.", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowClientManager", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    End If

                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CallCMManager Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CallCMManager", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'MKW070703 PN4026 START
    Private Function IsClientVisible(ByVal PartyCnt As Integer, ByRef r_bValue As Boolean) As Integer
        Dim result As Integer = 0
        Dim oObject As bSIRFindParty.Business
        Dim vReturn As Object
        Dim lReturnCount As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        Dim temp_oObject As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oObject, "bSIRFindParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        oObject = temp_oObject

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create an instance of bSIRFindParty.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="IsClientVisible", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            Return result
        End If

        m_lReturn = GetValidSources()


        m_lReturn = oObject.SearchSpecialPartyByQuery(r_vResultArray:=vReturn, r_lNumberOfRecords:=lReturnCount, v_vClientType:="<ALL>", v_vPartyCnt:=PartyCnt, v_vValidSourceArray:=m_vSourceArray)
        m_aoPartySearchData = vReturn

        r_bValue = False
        If lReturnCount > 0 Then
            r_bValue = True
        End If


        oObject.Dispose()

        oObject = Nothing

        Return result

    End Function
    'MKW070703 PN4026 END

    Private Function GetPartyType(ByVal v_lPartyCnt As Integer, ByRef r_sPartyType As String) As Integer
        Dim result As Integer = 0
        Dim oObject As bSIRFindParty.Business



        result = gPMConstants.PMEReturnCode.PMTrue

        Dim temp_oObject As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oObject, "bSIRFindParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        oObject = temp_oObject

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create an instance of bSIRFindParty.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartyType", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            Return result
        End If



        m_lReturn = oObject.GetPartyType(v_lPartyCnt, r_sPartyType)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Just want the P, G or C.
        r_sPartyType = r_sPartyType.Substring(0, 1)


        oObject.Dispose()

        oObject = Nothing

        Return result

    End Function


    'MKW070703 PN4026 START
    Public Function GetValidSources() As Integer
        Dim result As Integer = 0

        Try


            Dim oPMUser As bPMUser.Business

            result = gPMConstants.PMEReturnCode.PMTrue
            'David Kyle Thing
            'Call PMUser to get the Sources
            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_oPMUser As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPMUser, "bPMUser.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPMUser = temp_oPMUser

            '    ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                '        ' Display error stating the problem.

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bPMUser.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetValidSources", excep:=New Exception(Information.Err().Description))

                Return result
            End If


            m_lReturn = oPMUser.GetUserSources(r_vSourceArray:=m_vSourceArray, v_vUserID:=g_iUserId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get valid sources", vApp:=ACApp, vClass:=ACClass, vMethod:="GetValidSources", excep:=New Exception(Information.Err().Description))

                Return result
            End If

            '    ' Remove instance of PMUser
            If Not (oPMUser Is Nothing) Then

                oPMUser.Dispose()
                oPMUser = Nothing
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="GetValidSources", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'MKW070703 PN4026 END
    ' ***************************************************************** '
    ' Name: ShowPolicy
    '
    ' Description: Displays policy information.
    ' 'FSA Phase III pass party type
    ' ***************************************************************** '
    Public Function ShowPolicy(ByVal v_lPartyCnt As Integer, ByVal v_sShortName As String, Optional ByVal v_sPartyType As String = "") As Integer

        Dim result As Integer = 0
        Dim fIndex As Integer
        Dim lMDIWidth, lMDIHeight, lMDITop, lMDILeft As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Make sure its not already display.
            For Each frmChild As Form In m_frmParentMdiForm.MdiChildren
                Debug.WriteLine(frmChild.Name)
                lMDIHeight = CInt(VB6.PixelsToTwipsY(m_frmParentMdiForm.Height))
                lMDIWidth = CInt(VB6.PixelsToTwipsX(m_frmParentMdiForm.Width))
                lMDITop = CInt(VB6.PixelsToTwipsY(m_frmParentMdiForm.Top))
                lMDILeft = CInt(VB6.PixelsToTwipsX(m_frmParentMdiForm.Left))

                If frmChild.Name = "frmListPolicy" Then
                    ' Switch focus to it
                    m_lReturn = ReflectionHelper.Invoke(frmChild, "SwitchTo", New Object() {})
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Log Error Message
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display current PolicyList form." & Environment.NewLine & _
                                           "Please switch to form manually.", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPolicy", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    End If
                    Return result
                End If
            Next

            ' If not then create a new one
            fIndex = FindFreeIndex()

            Document(fIndex) = New frmListPolicy(m_frmParentMdiForm)

            Document(fIndex).ModuleClass = Me

            Document(fIndex).PartyCnt = v_lPartyCnt

            Document(fIndex).ShortName = v_sShortName
            'FSA Phase III
            If Not True Then

                Document(fIndex).PartyType = ""
            Else

                Document(fIndex).PartyType = v_sPartyType
            End If
            'FSA Phase III

            Document(fIndex).Tag = fIndex

            Document(fIndex).LoadInterface()

            Document(fIndex).Show()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowPolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPolicy", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'DC 101100
    ' ***************************************************************** '
    ' Name: ShowClaimList
    '
    ' Description: Displays claim list information.
    '
    ' ***************************************************************** '
    Public Function ShowClaimList(ByVal v_lPartyCnt As Integer, ByVal v_sShortName As String, Optional ByVal v_sInsReference As String = "") As Integer

        Dim result As Integer = 0
        Dim fIndex As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Make sure its not already display.
            For Each frmChid As Form In m_frmParentMdiForm.MdiChildren
                If frmChid.Name = "frmListClaim" Then
                    ' Switch focus to it

                    m_lReturn = ReflectionHelper.Invoke(frmChid, "SwitchTo", New Object() {})
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Log Error Message
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display current ClaimList form." & Environment.NewLine & _
                                           "Please switch to form manually.", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowClaimList", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    End If
                    Return result
                End If
            Next

            ' If not then create a new one
            fIndex = FindFreeIndex()

            Document(fIndex) = New frmListClaim(m_frmParentMdiForm)

            Document(fIndex).ModuleClass = Me


            Document(fIndex).PartyCnt = v_lPartyCnt

            Document(fIndex).ShortName = v_sShortName

            Document(fIndex).InsReference = v_sInsReference

            Document(fIndex).Tag = fIndex


            Document(fIndex).LoadInterface()

            Document(fIndex).Show()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowClaimList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowClaimList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'DC 101100
    ' ***************************************************************** '
    ' Name: ShowClaim
    '
    ' Description: Displays claim information.
    '
    ' ***************************************************************** '
    Public Function ShowClaim(ByVal v_sClaimNumber As String, ByVal v_lPolicyID As Integer, ByVal v_lClaimId As Integer, ByVal v_lRiskTypeId As Integer, ByVal v_dtClaimDate As Date, ByVal v_sClientName As String, ByVal v_sInsReference As String) As Integer
        Dim result As Integer = 0
        Dim iOpenClaim As Object

        Dim sSQL As String = ""
        Dim vResultArray As Object


        Dim m_oOpenClaim As iOpenClaim.Interface_Renamed


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vKeyArray(1, 6) As Object

            ' Assign the key array with the parameter members.

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameRiskTypeID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = v_lRiskTypeId


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameClaimCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = v_lClaimId


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNameClaimDate

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = v_dtClaimDate


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.PMKeyNameOperateMode

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = g_nREADMODE


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.PMKeyNamePolicyID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = v_lPolicyID


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMNavKeyConst.PMKeyNamePolicyNumber

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = v_sInsReference


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 6) = PMNavKeyConst.PMKeyNameClientHolder

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 6) = v_sClientName

            If m_oOpenClaim Is Nothing Then
                ' Get instance of Open Claim Object
                Dim temp_m_oOpenClaim As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oOpenClaim, sClassName:="iOpenClaim.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oOpenClaim = temp_m_oOpenClaim

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of iOpenClaim.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowClaim", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If
            End If


            m_lReturn = m_oOpenClaim.SetKeys(vKeyArray:=vKeyArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_oOpenClaim = Nothing
                Return result
            End If


            m_lReturn = m_oOpenClaim.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_oOpenClaim = Nothing
                Return result
            End If


            m_oOpenClaim.Dispose()
            m_oOpenClaim = Nothing
            Return result
        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowClaim Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowClaim", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ShowPolicyDetail
    '
    ' Description: Displays policy information.
    '
    ' ***************************************************************** '
    Public Function ShowPolicyDetail(ByVal v_lPartyCnt As Integer, ByVal v_sPartyType As String, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsFileCnt As Integer, ByVal v_lInsuranceFileStructureId As Integer, ByVal v_sShortName As String, ByVal v_sInsReference As String, ByVal v_bFromEvent As Boolean, ByVal v_lPolicyTypeId As Integer, ByVal v_vGeminiPolicyStatus As Object, Optional ByVal v_bCopiedPolicy As Boolean = False) As Object

        Dim result As Object = Nothing
        Dim fIndex As Integer
        Dim sFormName As String = "" 'CT 11/09/00
        Dim iSourceID As Integer 'eck180500
        Dim iBusinessTypeId As Integer 'JSB 06/05/01
        Dim lDefaultPolicyID As Integer 'JSB 06/05/01
        Dim lGISPolicyLinkID As Integer 'JSB 06/05/01
        Dim vKeyArray As Object 'JSB 06/05/01
        Dim sBusinessTypeCode As String = "" 'JSB 06/05/01
        Dim sNavProcMap As String = "" 'JSB 05/07/01
        Dim bCallNavXM, bSQ As Boolean
        'PN29502
        Dim iCountryID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bCallNavXM = False
            bSQ = False
            'Only allow one instance of the Navigator process to run, otherwise the
            'process attempts to handle to maps simultaneously
            If IsNavStartRunning Then 'PN26814
                MessageBox.Show("A Navigator Process is already running.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                m_lReturn = gPMConstants.PMEReturnCode.PMCancel
                Return m_lReturn
            End If

            'eck180500 Get the source for new policys
            '    If (v_bFromEvent = False) Then
            If v_lInsFileCnt = 0 Then
                m_lReturn = GetCompany(m_iCompanyID:=iSourceID, m_iCountryID:=iCountryID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If
            End If
            If v_lPolicyTypeId = 0 Then
                m_lReturn = GetPolicyType(v_bFromEvent:=v_bFromEvent, v_lInsuranceFileCnt:=v_lInsFileCnt, v_lPolicyTypeId:=v_lPolicyTypeId)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If
            End If

            'MSS270901 - Merge. Left all this SFORB code in. Shouldn't effect UW, but if it does,
            ' will need a switch to not look for GII policies

            'CT 11/09/00 set the name of the form to display
            If v_lPolicyTypeId = PMBConst.PMBPolicyTypeUnderwriting Then
                sFormName = "frmPolicyUnderwriting"
                ' JSB 05/06/01 - If GII Motor, household, or CV is selected then Fire up Select default
                ' policy screen then fire up corresponding road map
            ElseIf ((v_lPolicyTypeId = PMBConst.PMBPolicyTypeGIIMotor) Or (v_lPolicyTypeId = PMBConst.PMBPolicyTypeGIIHousehold) Or (v_lPolicyTypeId = PMBConst.PMBPolicyTypeGIICommercialVehicle)) Then
                'Check value of flag to see if we want to call GII
                If g_bCallGII Then
                    'set the business type id
                    Select Case v_lPolicyTypeId
                        Case PMBConst.PMBPolicyTypeGIIMotor
                            iBusinessTypeId = CInt(PMGISBusinessTypeMotorID)
                            sBusinessTypeCode = PMGISBusinessTypeMotor
                        Case PMBConst.PMBPolicyTypeGIIHousehold
                            iBusinessTypeId = CInt(PMGISBusinessTypeHouseholdID)
                            sBusinessTypeCode = PMGISBusinessTypeHousehold
                        Case PMBConst.PMBPolicyTypeGIICommercialVehicle
                            iBusinessTypeId = CInt(PMGISBusinessTypeTruckID)
                            sBusinessTypeCode = PMGISBusinessTypeTruck
                    End Select

                    'Get Default Policy ID
                    m_lReturn = GetDefaultPolicy(v_bFromEvent:=v_bFromEvent, v_lInsuranceFileCnt:=v_lInsFileCnt, v_iBusinessTypeID:=iBusinessTypeId, r_lDefaultPolicyID:=lDefaultPolicyID, r_lGISPolicyLinkID:=lGISPolicyLinkID)
                    If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                        Return result
                    End If
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display default policy screen.", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPolicyDetail", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                        g_bCallGII = False
                        Return m_lReturn
                    End If
                    'we have all the keys we need now, load them up into an array and  fire up the navigator process

                    ReDim vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 8)


                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNamePolicyKey

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = v_lInsFileCnt


                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNamePolicyNo

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = 0 ' not set for NB, so set to zero


                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNamePartyCnt

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = v_lPartyCnt


                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.PMKeyNameInsuranceFileCnt

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = v_lInsFileCnt


                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.PMKeyNameDefaultPolicyId

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = lDefaultPolicyID


                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMNavKeyConst.PMKeyNameInsuranceFolderCnt

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = v_lInsuranceFolderCnt


                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 6) = PMNavKeyConst.PMKeyNameBusinessTypeId

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 6) = iBusinessTypeId


                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 7) = PMNavKeyConst.PMKeyNameSourceId

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 7) = iSourceID


                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 8) = PMNavKeyConst.PMKeyNameBusinessTypeCode

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 8) = sBusinessTypeCode

                    'Call NavXM navigator if using SQ
                    m_lReturn = DetectSQ(bSQ)

                    If bSQ Then 'PN26814
                        bCallNavXM = True
                        'Extend the Key Array as the CurrentNavXMStep value is also required
                        ReDim Preserve vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 9)
                    End If
                    Select Case sBusinessTypeCode
                        Case PMGISBusinessTypeMotor
                            sNavProcMap = PMNavKeyConst.PMNavProcMapGIIStartMotorNB
                            If bSQ Then
                                sNavProcMap = PMNavKeyConst.PMNavProcMapSQStartMotorNB

                                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 9) = PMNavKeyConst.PMKeyNameCurrentNavXMStep

                                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 9) = PMNavKeyConst.PMNavProcMapSQStartMotorNBCurrentNavXMStep
                            End If
                        Case PMGISBusinessTypeHousehold
                            sNavProcMap = PMNavKeyConst.PMNavProcMapGIIStartHouseholdNB
                            If bSQ Then
                                sNavProcMap = PMNavKeyConst.PMNavProcMapSQStartHouseholdNB

                                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 9) = PMNavKeyConst.PMKeyNameCurrentNavXMStep

                                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 9) = PMNavKeyConst.PMNavProcMapSQStartHouseholdNBCurrentNavXMStep
                            End If
                        Case PMGISBusinessTypeTruck
                            sNavProcMap = PMNavKeyConst.PMNavProcMapGIIStartCVNB
                            If bSQ Then
                                sNavProcMap = PMNavKeyConst.PMNavProcMapSQStartCVNB

                                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 9) = PMNavKeyConst.PMKeyNameCurrentNavXMStep

                                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 9) = PMNavKeyConst.PMNavProcMapSQStartCVNBCurrentNavXMStep
                            End If
                    End Select

                    If StartNavMap(v_sProcessCode:=sNavProcMap, v_vKeyArray:=vKeyArray, v_bCallNavXM:=bCallNavXM) <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to New business process map.", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPolicyDetail", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        g_bCallGII = False
                        Return m_lReturn
                    End If

                    vKeyArray = Nothing

                    'that's enough lets get out
                    g_bCallGII = False
                    Return result
                Else
                    sFormName = "frmPolicy"
                End If
            Else
                sFormName = "frmPolicy"
            End If

            ' Make sure it's not already displayed.
            For iLoop1 As Integer = 0 To Application.OpenForms.Count - 1
                'CT 11/09/00 Use form name to open policy or Underwriting policy form
                'If (Forms(iLoop1%).Name = "frmPolicy") Then
                If Application.OpenForms.Item(iLoop1).Name = sFormName Then

                    'Is it this version?


                    If (ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "InsFileCnt") = v_lInsFileCnt) And (ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "FromEvent") = v_bFromEvent) Then
                        ' Switch focus to it

                        m_lReturn = ReflectionHelper.Invoke(Application.OpenForms.Item(iLoop1), "SwitchTo", New Object() {})
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Log Error Message
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display current Policy form." & Environment.NewLine & _
                                               "Please switch to form manually.", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPolicyDetail", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        End If
                        Return result
                    End If
                End If
            Next iLoop1

            fIndex = FindFreeIndex()

            'CT 11/09/00 Set form to either Policy or Underwriting Policy - start
            'Set Document(fIndex) = New frmPolicy
            If sFormName = "frmPolicyUnderwriting" Then
                Document(fIndex) = New frmPolicyUnderwriting(m_frmParentMdiForm)
                Document(fIndex).ModuleClass = Me
            Else
                'Set Document(fIndex) = New frmPolicy
                'PSL 10/06/2003 : Pass in new parameters so that GII policies can go into risk screens.

                Document(fIndex).PolicyTypeId = v_lPolicyTypeId


                Document(fIndex).GeminiPolicyStatus = v_vGeminiPolicyStatus
            End If
            'CT 11/09/00 - end


            Document(fIndex).PartyCnt = v_lPartyCnt

            Document(fIndex).PartyType = v_sPartyType

            Document(fIndex).InsuranceFolderCnt = v_lInsuranceFolderCnt

            Document(fIndex).InsFileCnt = v_lInsFileCnt

            Document(fIndex).ShortName = v_sShortName

            Document(fIndex).InsReference = v_sInsReference

            'AK 270401 - to mark renewal events

            Document(fIndex).RenewalEvent = m_bRenewalEvent

            'eck180500

            Document(fIndex).SourceID = iSourceID
            '
            'PN29502
            If sFormName = "frmPolicy" Then

                Document(fIndex).CountryID = iCountryID
            End If

            ' CTAF 100701 ****

            Document(fIndex).CopiedPolicy = v_bCopiedPolicy
            ' **** CTAF 100701


            Document(fIndex).FromEvent = v_bFromEvent

            Document(fIndex).Tag = fIndex
            'PN29400

            Document(fIndex).Top = 0

            Document(fIndex).Left = 0


            Document(fIndex).LoadInterface()

            Document(fIndex).Show()

            ' Set the properties
            'frmPolicy.PartyCnt = v_lPartyCnt
            'frmPolicy.InsFileCnt = v_lInsFileCnt&
            'frmPolicy.ShortName = v_sShortName
            'frmPolicy.InsReference = v_sInsReference
            'frmPolicy.Tag = fIndex%

            ' Display the form
            'frmPolicy.Show

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowPolicyDetail Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPolicy", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            'set flag to false
            g_bCallGII = False

            Return result

        End Try
    End Function

    'MSS270901 - Added function from UW for merge
    '*******************************************************************
    ' Name : ShowPolicyListVersion
    '
    ' Desc : display all versions of policy
    '
    ' Hist : 27/02/2001 Created - Tinny
    '*******************************************************************
    Public Function ShowPolicyListVersion(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_sPartyType As String, ByVal v_sShortName As String, ByVal v_sInsReference As String) As Integer 'PN19733

        Dim result As Integer = 0
        Dim lCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'if its already opened then switch to it
            For lCount = 0 To Application.OpenForms.Count - 1
                If Application.OpenForms.Item(lCount).Name = "frmListPolicyVersion" Then

                    'NIIT - Replaced with the Migrated code 1144 
                    'If Application.OpenForms.Item(lCount).InsuranceFileCnt = v_lInsuranceFileCnt Then
                    If ReflectionHelper.GetMember(Application.OpenForms.Item(lCount), "InsuranceFileCnt") = v_lInsuranceFileCnt Then

                        'NIIT - Replaced with the Migrated code 1144 
                        'If Application.OpenForms.Item(lCount).SwitchTo() <> gPMConstants.PMEReturnCode.PMTrue Then
                        If ReflectionHelper.Invoke(Application.OpenForms.Item(lCount), "SwitchTo", New Object() {}) <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Log Error Message
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display current Policy List Version form." & Environment.NewLine & _
                                               "Please switch to form manually.", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPolicyListVersion", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        End If

                        Return result

                    End If
                End If
            Next

            'create a new instance of the form

            lCount = FindFreeIndex()

            Document(lCount) = New frmListPolicyVersion(m_frmParentMdiForm)

            Document(lCount).ModuleClass = Me

            Document(lCount).InsuranceFolderCnt = v_lInsuranceFolderCnt

            Document(lCount).InsuranceFileCnt = v_lInsuranceFileCnt

            Document(lCount).PartyCnt = v_lPartyCnt

            Document(lCount).PartyType = v_sPartyType

            Document(lCount).ShortName = v_sShortName

            Document(lCount).InsuranceFileRef = v_sInsReference 'PN19733

            Document(lCount).Tag = lCount

            Document(lCount).LoadInterface()

            Document(lCount).Show()


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowPolicyListVersion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPolicyListVersion", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'MSS270901 - Merge end

    'ECK 15/6/99
    ' ***************************************************************** '
    ' Name: ShowPolicySummary
    '
    ' Description: Displays policy summary information.
    '
    ' ***************************************************************** '
    Public Function ShowPolicySummary(ByVal v_lPartyCnt As Integer, ByVal v_sPartyType As String, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsFileCnt As Integer, ByVal v_sShortName As String, ByVal v_sInsReference As String, ByVal v_lPolicyTypeId As Integer, Optional ByVal v_lRiskCnt As Integer = 0) As Object

        Dim result As Object = Nothing
        Dim fIndex As Integer
        Dim sFormName As String = ""

        Try

            If v_lPolicyTypeId = 0 Then
                'Commented the code and set the policy default to '5' to remove the use of iPMBFindPolicyType component.

                'm_lReturn = GetPolicyType(v_bFromEvent:=False, v_lInsuranceFileCnt:=v_lInsFileCnt, v_lPolicyTypeId:=v_lPolicyTypeId)
                'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Return m_lReturn
                'End If
                v_lPolicyTypeId = 5
            End If

            'MSS270901 - Added for merge
            'TN20010112 Start
            If v_lPolicyTypeId = PMBConst.PMBPolicyTypeUnderwriting Then
                sFormName = "frmPolicySummaryUnderwriting"
            Else
                sFormName = "frmPolicySummary"
            End If
            'TN20010112 End
            'MSS270901 - Merge end

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Make sure it's not already displayed.
            For iLoop1 As Integer = 0 To Application.OpenForms.Count - 1
                If Application.OpenForms.Item(iLoop1).Name = sFormName Then

                    'Is it this version?

                    If ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "InsuranceFileCnt") = v_lInsFileCnt Then
                        ' Switch focus to it

                        m_lReturn = ReflectionHelper.Invoke(Application.OpenForms.Item(iLoop1), "SwitchTo", New Object() {})
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Log Error Message
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display current Policy Summary List form." & Environment.NewLine & _
                                               "Please switch to form manually.", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPolicySummary", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        End If
                        Return result
                    End If
                End If
            Next iLoop1

            fIndex = FindFreeIndex()

            'MSS270901 - Added for merge
            'TN20010112 Start
            If v_lPolicyTypeId = PMBConst.PMBPolicyTypeUnderwriting Then
                Document(fIndex) = New frmPolicySummaryUnderwriting(m_frmParentMdiForm)

                Document(fIndex).ModuleClass = Me

                Document(fIndex).RiskCnt = v_lRiskCnt
            Else
                'Set Document(fIndex) = New frmPolicySummary
            End If
            'TN20010112 End
            'MSS270901 - Merge end


            Document(fIndex).PartyCnt = v_lPartyCnt

            Document(fIndex).PartyType = v_sPartyType

            Document(fIndex).ShortName = v_sShortName

            Document(fIndex).InsuranceFolderCnt = v_lInsuranceFolderCnt

            Document(fIndex).InsuranceFileCnt = v_lInsFileCnt

            Document(fIndex).InsReference = v_sInsReference.Trim()

            Document(fIndex).PolicyTypeId = v_lPolicyTypeId

            Document(fIndex).Tag = fIndex
            'PN29400

            Document(fIndex).Top = 0

            Document(fIndex).Left = 0


            Document(fIndex).LoadInterface()

            Document(fIndex).Show()


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowPolicySummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPolicySummary", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'MSS270901 - Added function for merge
    '*******************************************************************
    ' Name : UWNewPolicy
    '
    ' Desc : call road map to do underwriting new policy
    '
    ' History : 12/01/2001
    '*******************************************************************
    Public Function UWNewPolicy(ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_sPolicyRef As String, ByVal v_sShortName As String) As Integer

        Dim result As Integer = 0
        Dim vKeyArray(,) As Object
        Dim sRoadMap As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sRoadMap = "UNDERNBFP"

            ReDim vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5)


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNamePartyCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = v_lPartyCnt


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameInsFileCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = v_lInsuranceFileCnt


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = "insurance_ref"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = v_sPolicyRef


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.PMKeyNameInsFolderCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = v_lInsuranceFolderCnt


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.PMKeyNameShortName

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = v_sShortName


            Return StartNavMap(v_sProcessCode:=sRoadMap, v_vKeyArray:=vKeyArray)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UWNewPolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UWNewPolicy", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'MSS270901 - Merge end

    ' ***************************************************************** '
    '
    ' Name: ShowGeminiPolicyDetails
    '
    ' Description:
    '
    ' History: 14/09/1999 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function ShowGeminiPolicyDetail(ByVal v_lPartyCnt As Integer, ByVal v_sPartyType As String, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsFileCnt As Integer, ByVal v_lInsuranceFileStructureId As Integer, ByVal v_sShortName As String, ByVal v_sInsReference As String, ByVal v_bFromEvent As Boolean, ByVal v_lPolicyTypeId As Integer, ByVal v_vGeminiPolicyStatus As Object) As Integer

        Dim result As Integer = 0
        Dim lPolicyKey, lDefaultPolicyID, lBusinessTypeId, lStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Select Case v_lPolicyTypeId
                Case PMBConst.PMBPolicyTypeGIIMotor

                    m_lReturn = ShowGIIMRisk(v_lPartyCnt:=v_lPartyCnt, v_sShortName:=v_sShortName, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lInsuranceFileCnt:=v_lInsFileCnt)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return result
                    End If

                    Return result
                    ' PSA 22092000

                    ' PSA 20092000
                Case PMBConst.PMBPolicyTypeGIIHousehold

                    m_lReturn = ShowGIIHRisk(v_lPartyCnt:=v_lPartyCnt, v_sShortName:=v_sShortName, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lInsuranceFileCnt:=v_lInsFileCnt)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return result
                    End If

                    Return result
                    ' PSA 20092000

                    ' 25/04/2001 PSA - Start
                Case PMBConst.PMBPolicyTypeGIICommercialVehicle

                    m_lReturn = ShowGIITRisk(v_lPartyCnt:=v_lPartyCnt, v_sShortName:=v_sShortName, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lInsuranceFileCnt:=v_lInsFileCnt)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return result
                    End If

                    Return result
                    ' 25/04/2001 PSA - End

            End Select

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowGeminiPolicyDetail Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowGeminiPolicyDetail", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function CallGeminiII(ByVal v_lPartyCnt As Integer, ByVal v_sPartyType As String, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsFileCnt As Integer, ByVal v_sShortName As String, ByVal v_bFromEvent As Boolean, ByVal v_lPolicyTypeId As Integer, ByVal v_lGIIPolicyStatus As Integer, ByVal v_vGIIPolicyNumber As String) As Object

        Dim result As Object = Nothing
        Dim iSelectedProcess, iBusinessTypeId As Integer
        Dim sBusinessTypeCode As String = ""
        Dim lDefaultPolicyID As Integer
        Dim vKeyArray(,) As Object
        Dim iSourceID As Integer
        Dim bSQ, bCallNavXM As Boolean
        Dim sNavProcMap As String = ""
        'Developer Guide No.69
        Dim frmSelectGIIPolType As New frmSelectGIIPolType
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            frmSelectGIIPolType.ModuleClass = Me
            bSQ = False
            bCallNavXM = False
            'PN26814 - Only allow one instance of the Navigator process to run, otherwise the
            'process attempts to handle to maps simultaneously
            If IsNavStartRunning Then
                MessageBox.Show("A Navigator Process is already running.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                m_lReturn = gPMConstants.PMEReturnCode.PMCancel
                Return m_lReturn
            End If

            'MKW270503 PN4294 - 1.8.5 to 1.8.6 START
            'ISS3184 JAS 24/3/03

            If Convert.IsDBNull(v_lGIIPolicyStatus) Or IsNothing(v_lGIIPolicyStatus) Or v_lPolicyTypeId = PMBConst.PMBPolicyTypeSchemes Then 'PN19063
                result = gPMConstants.PMEReturnCode.PMError
                'KB PN 5190 4/07/03
                'Make the message more accurate
                'MsgBox "Gemini II does not appear to be installed", vbInformation, "Gemini II"
                MessageBox.Show("Unable to access Quotes from a General Policy", "Gemini II", MessageBoxButtons.OK, MessageBoxIcon.Information)
                'PN 5190 end
                Return result
            End If
            'MKW270503 PN4294 END

            'PN26814 - Call NavXM navigator if using SQ
            m_lReturn = DetectSQ(bSQ)

            'Find out what the user want to do with the risk
            'pass through policy status id
            frmSelectGIIPolType.PolicyStatus = v_lGIIPolicyStatus
            If bSQ Then
                'PN26814 - Do not display Gemini II on caption on SQ systems
                frmSelectGIIPolType.Text = "Available SQ Processes"
            End If
            'load form
            Dim tempLoadForm As frmSelectGIIPolType = frmSelectGIIPolType

            'show form
            frmSelectGIIPolType.ShowDialog()

            'if everythings alright then set the process that has been selected and carry on
            If frmSelectGIIPolType.Status = gPMConstants.PMEReturnCode.PMOK Then
                iSelectedProcess = frmSelectGIIPolType.SelectedProcess
            Else
                frmSelectGIIPolType.Close()
                Return result
            End If

            frmSelectGIIPolType.Close()

            'use policytype id to select the type of business it is
            Select Case v_lPolicyTypeId
                Case PMBConst.PMBPolicyTypeGIIMotor
                    iBusinessTypeId = CInt(PMGISBusinessTypeMotorID)
                    sBusinessTypeCode = PMGISBusinessTypeMotor
                Case PMBConst.PMBPolicyTypeGIIHousehold
                    iBusinessTypeId = CInt(PMGISBusinessTypeHouseholdID)
                    sBusinessTypeCode = PMGISBusinessTypeHousehold
                Case PMBConst.PMBPolicyTypeGIICommercialVehicle
                    iBusinessTypeId = CInt(PMGISBusinessTypeTruckID)
                    sBusinessTypeCode = PMGISBusinessTypeTruck
            End Select

            'PN26814 - If performing a Rebroke, then the InsFileCnt will not be 0
            '(as it is the reference to the original) therefore, get the Source for Rebrokes
            If (v_lInsFileCnt = 0) Or (iSelectedProcess = StringsHelper.ToDoubleSafe(GIIProcessTypeRebroke)) Then
                If GetCompany(m_iCompanyID:=iSourceID) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If
            End If

            'we have all the keys we need now, load them up into an array and fire up the navigator process
            ReDim vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 8)


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNamePolicyKey

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = v_lInsFileCnt

            'JSB 03/01/02 - If re-broke, then set policy number to 'copy-policy'
            If iSelectedProcess = StringsHelper.ToDoubleSafe(GIIProcessTypeRebroke) Then

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNamePolicyNo

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = "COPY-POLICY"
            Else

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNamePolicyNo

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = v_vGIIPolicyNumber
            End If


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNamePartyCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = v_lPartyCnt


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.PMKeyNameInsuranceFileCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = v_lInsFileCnt


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.PMKeyNameDefaultPolicyId

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = lDefaultPolicyID


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMNavKeyConst.PMKeyNameInsuranceFolderCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = v_lInsuranceFolderCnt


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 6) = PMNavKeyConst.PMKeyNameBusinessTypeId

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 6) = iBusinessTypeId


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 7) = PMNavKeyConst.PMKeyNameBusinessTypeCode

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 7) = sBusinessTypeCode


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 8) = PMNavKeyConst.PMKeyNameSourceId

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 8) = iSourceID

            If bSQ Then
                bCallNavXM = True
                'PN26814 - Extend the Key Array as the CurrentNavXMStep value is also required
                ReDim Preserve vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 9)
            End If

            'Now decide which roadmap we need to call and call it
            'PN26814 - For SQ, call the NavXM Map rather than the Navigator Process
            Select Case sBusinessTypeCode
                'motor
                Case PMGISBusinessTypeMotor
                    Select Case iSelectedProcess
                        'NB
                        Case CInt(GIIProcessTypeNB)
                            sNavProcMap = PMNavKeyConst.PMNavProcMapGIIStartMotorNB
                            If bSQ Then
                                sNavProcMap = PMNavKeyConst.PMNavProcMapSQStartMotorNB

                                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 9) = PMNavKeyConst.PMKeyNameCurrentNavXMStep

                                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 9) = PMNavKeyConst.PMNavProcMapSQStartMotorNBCurrentNavXMStep
                            End If
                            m_lReturn = StartNavMap(v_sProcessCode:=sNavProcMap, v_vKeyArray:=vKeyArray, v_bCallNavXM:=bCallNavXM)
                            'MMD
                        Case CInt(GIIProcessTypeMaintain)
                            sNavProcMap = PMNavKeyConst.PMNavProcMapGIIStartMotorMaintain
                            If bSQ Then
                                sNavProcMap = PMNavKeyConst.PMNavProcMapSQStartMotorMaintain

                                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 9) = PMNavKeyConst.PMKeyNameCurrentNavXMStep

                                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 9) = PMNavKeyConst.PMNavProcMapSQStartMotorMaintainCurrentNavXMStep
                            End If
                            m_lReturn = StartNavMap(v_sProcessCode:=sNavProcMap, v_vKeyArray:=vKeyArray, v_bCallNavXM:=bCallNavXM)
                            'MTA
                        Case CInt(GIIProcessTypeMTA)
                            sNavProcMap = PMNavKeyConst.PMNavProcMapGIIStartMotorMTA
                            If bSQ Then
                                sNavProcMap = PMNavKeyConst.PMNavProcMapSQStartMotorMTA

                                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 9) = PMNavKeyConst.PMKeyNameCurrentNavXMStep

                                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 9) = PMNavKeyConst.PMNavProcMapSQStartMotorMTACurrentNavXMStep
                            End If
                            m_lReturn = StartNavMap(v_sProcessCode:=sNavProcMap, v_vKeyArray:=vKeyArray, v_bCallNavXM:=bCallNavXM)
                            'Rebroke
                        Case CInt(GIIProcessTypeRebroke)
                            sNavProcMap = PMNavKeyConst.PMNavProcMapGIIStartMotorRebroke
                            If bSQ Then
                                sNavProcMap = PMNavKeyConst.PMNavProcMapSQStartMotorRebroke

                                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 9) = PMNavKeyConst.PMKeyNameCurrentNavXMStep

                                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 9) = PMNavKeyConst.PMNavProcMapSQStartMotorRebrokeCurrentNavXMStep
                            End If
                            m_lReturn = StartNavMap(v_sProcessCode:=sNavProcMap, v_vKeyArray:=vKeyArray, v_bCallNavXM:=bCallNavXM)
                            'Review
                        Case CInt(GIIProcessTypeReview)
                            sNavProcMap = PMNavKeyConst.PMNavProcMapGIIStartMotorReivew
                            If bSQ Then
                                sNavProcMap = PMNavKeyConst.PMNavProcMapSQStartMotorReview

                                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 9) = PMNavKeyConst.PMKeyNameCurrentNavXMStep

                                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 9) = PMNavKeyConst.PMNavProcMapSQStartMotorReviewCurrentNavXMStep
                            End If
                            m_lReturn = StartNavMap(v_sProcessCode:=sNavProcMap, v_vKeyArray:=vKeyArray, v_bCallNavXM:=bCallNavXM)
                        Case Else
                            'shouldn't be an else, somethings wrong if we've ended up here
                    End Select
                    'household
                Case PMGISBusinessTypeHousehold
                    Select Case iSelectedProcess
                        'NB
                        Case CInt(GIIProcessTypeNB)
                            sNavProcMap = PMNavKeyConst.PMNavProcMapGIIStartHouseholdNB
                            If bSQ Then
                                sNavProcMap = PMNavKeyConst.PMNavProcMapSQStartHouseholdNB

                                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 9) = PMNavKeyConst.PMKeyNameCurrentNavXMStep

                                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 9) = PMNavKeyConst.PMNavProcMapSQStartHouseholdNBCurrentNavXMStep
                            End If
                            m_lReturn = StartNavMap(v_sProcessCode:=sNavProcMap, v_vKeyArray:=vKeyArray, v_bCallNavXM:=bCallNavXM)
                            'MMD
                        Case CInt(GIIProcessTypeMaintain)
                            sNavProcMap = PMNavKeyConst.PMNavProcMapGIIStartHouseholdMaintain
                            If bSQ Then
                                sNavProcMap = PMNavKeyConst.PMNavProcMapSQStartHouseholdMaintain

                                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 9) = PMNavKeyConst.PMKeyNameCurrentNavXMStep

                                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 9) = PMNavKeyConst.PMNavProcMapSQStartHouseholdMaintainCurrentNavXMStep
                            End If
                            m_lReturn = StartNavMap(v_sProcessCode:=sNavProcMap, v_vKeyArray:=vKeyArray, v_bCallNavXM:=bCallNavXM)
                            'MTA
                        Case CInt(GIIProcessTypeMTA)
                            sNavProcMap = PMNavKeyConst.PMNavProcMapGIIStartHouseholdMTA
                            If bSQ Then
                                sNavProcMap = PMNavKeyConst.PMNavProcMapSQStartHouseholdMTA

                                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 9) = PMNavKeyConst.PMKeyNameCurrentNavXMStep

                                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 9) = PMNavKeyConst.PMNavProcMapSQStartHouseholdMTACurrentNavXMStep
                            End If
                            m_lReturn = StartNavMap(v_sProcessCode:=sNavProcMap, v_vKeyArray:=vKeyArray, v_bCallNavXM:=bCallNavXM)
                            'Rebroke
                        Case CInt(GIIProcessTypeRebroke)
                            sNavProcMap = PMNavKeyConst.PMNavProcMapGIIStartHouseholdRebroke
                            If bSQ Then
                                sNavProcMap = PMNavKeyConst.PMNavProcMapSQStartHouseholdRebroke

                                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 9) = PMNavKeyConst.PMKeyNameCurrentNavXMStep

                                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 9) = PMNavKeyConst.PMNavProcMapSQStartHouseholdRebrokeCurrentNavXMStep
                            End If
                            m_lReturn = StartNavMap(v_sProcessCode:=sNavProcMap, v_vKeyArray:=vKeyArray, v_bCallNavXM:=bCallNavXM)
                            'Review
                        Case CInt(GIIProcessTypeReview)
                            sNavProcMap = PMNavKeyConst.PMNavProcMapGIIStartHouseholdReivew
                            If bSQ Then
                                sNavProcMap = PMNavKeyConst.PMNavProcMapSQStartHouseholdReview

                                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 9) = PMNavKeyConst.PMKeyNameCurrentNavXMStep

                                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 9) = PMNavKeyConst.PMNavProcMapSQStartHouseholdReviewCurrentNavXMStep
                            End If
                            m_lReturn = StartNavMap(v_sProcessCode:=sNavProcMap, v_vKeyArray:=vKeyArray, v_bCallNavXM:=bCallNavXM)
                        Case Else
                            'shouldn't be an else, somethings wrong if we've ended up here
                    End Select
                    'CV
                Case PMGISBusinessTypeTruck
                    Select Case iSelectedProcess
                        'NB
                        Case CInt(GIIProcessTypeNB)
                            sNavProcMap = PMNavKeyConst.PMNavProcMapGIIStartCVNB
                            If bSQ Then
                                sNavProcMap = PMNavKeyConst.PMNavProcMapSQStartCVNB

                                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 9) = PMNavKeyConst.PMKeyNameCurrentNavXMStep

                                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 9) = PMNavKeyConst.PMNavProcMapSQStartCVNBCurrentNavXMStep
                            End If
                            m_lReturn = StartNavMap(v_sProcessCode:=sNavProcMap, v_vKeyArray:=vKeyArray, v_bCallNavXM:=bCallNavXM)
                            'MMD
                        Case CInt(GIIProcessTypeMaintain)
                            sNavProcMap = PMNavKeyConst.PMNavProcMapGIIStartCVMaintain
                            If bSQ Then
                                sNavProcMap = PMNavKeyConst.PMNavProcMapSQStartCVMaintain

                                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 9) = PMNavKeyConst.PMKeyNameCurrentNavXMStep

                                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 9) = PMNavKeyConst.PMNavProcMapSQStartCVMaintainCurrentNavXMStep
                            End If
                            m_lReturn = StartNavMap(v_sProcessCode:=sNavProcMap, v_vKeyArray:=vKeyArray, v_bCallNavXM:=bCallNavXM)
                            'MTA
                        Case CInt(GIIProcessTypeMTA)
                            If Not bSQ Then
                                MessageBox.Show("Commercial vehicle MTA's are currently unavailable", "Client Manager", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Else
                                sNavProcMap = PMNavKeyConst.PMNavProcMapSQStartCVMTA

                                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 9) = PMNavKeyConst.PMKeyNameCurrentNavXMStep

                                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 9) = PMNavKeyConst.PMNavProcMapSQStartCVMTACurrentNavXMStep
                            End If
                            m_lReturn = StartNavMap(v_sProcessCode:=sNavProcMap, v_vKeyArray:=vKeyArray, v_bCallNavXM:=bCallNavXM)
                            'Rebroke
                        Case CInt(GIIProcessTypeRebroke)
                            sNavProcMap = PMNavKeyConst.PMNavProcMapGIIStartCVRebroke
                            If bSQ Then
                                sNavProcMap = PMNavKeyConst.PMNavProcMapSQStartCVRebroke

                                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 9) = PMNavKeyConst.PMKeyNameCurrentNavXMStep

                                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 9) = PMNavKeyConst.PMNavProcMapSQStartCVRebrokeCurrentNavXMStep
                            End If
                            m_lReturn = StartNavMap(v_sProcessCode:=sNavProcMap, v_vKeyArray:=vKeyArray, v_bCallNavXM:=bCallNavXM)
                            'Review
                        Case CInt(GIIProcessTypeReview)
                            sNavProcMap = PMNavKeyConst.PMNavProcMapGIIStartCVReivew
                            If bSQ Then
                                sNavProcMap = PMNavKeyConst.PMNavProcMapSQStartCVReview

                                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 9) = PMNavKeyConst.PMKeyNameCurrentNavXMStep

                                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 9) = PMNavKeyConst.PMNavProcMapSQStartCVReviewCurrentNavXMStep
                            End If
                            m_lReturn = StartNavMap(v_sProcessCode:=sNavProcMap, v_vKeyArray:=vKeyArray, v_bCallNavXM:=bCallNavXM)
                        Case Else
                            'shouldn't be an else, somethings wrong if we've ended up here
                    End Select
            End Select

            vKeyArray = Nothing

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to New business process map.", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPolicyDetail", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return m_lReturn
            End If

            'that's enough lets get out
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="CallGeminiIIFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="CallGeminiII", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    'eck180500
    ' ***************************************************************** '
    ' Name: GetCompany (Standard Method)
    '
    ' Description: Gets valid Source ID's  and if nessessary displays selection
    '
    ' ***************************************************************** '
    Private Function GetCompany(ByRef m_iCompanyID As Integer, Optional ByRef m_iCountryID As Integer = 0) As Integer
        Dim result As Integer = 0
        Dim iPMBBranch As Object

        Dim m_oBranch As iPMBBranch.Interface_Renamed


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim temp_m_oBranch As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_m_oBranch, sClassName:="iPMBBranch.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
        m_oBranch = temp_m_oBranch

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        'PN29502
        'DC240706 changed to iCountryId to match changes to GetSource

        m_lReturn = m_oBranch.GetSource(iSourceID:=m_iCompanyID, iCountryID:=m_iCountryID)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_oBranch.Dispose()
        m_oBranch = Nothing

        Return result

    End Function
    'eck120500

    ' ***************************************************************** '
    '
    ' Name: GIIMNewBusiness
    '
    ' Description:
    '
    ' History: 29/03/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (GIIMNewBusiness) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GIIMNewBusiness(ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsFileCnt As Integer, ByVal v_sShortName As String, ByVal v_sInsReference As String, ByRef r_lPolicyKey As Integer, ByRef r_lDefaultPolicyID As Integer, ByRef r_lBusinessTypeId As Integer, ByRef r_lStatus As Integer) As Integer
    'Dim result As Integer = 0
    'Dim iGIIWFindBusTypeDefPol As Object
    '
    'Dim vKeyArray As Object

    'Dim oObject As iGIIWFindBusTypeDefPol.Interface_Renamed
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'It goes something like this
    'The NB roadmap calls Find Client, Find Policy, Find Business Type
    'We already know the client, and we know that we want to click the new button in
    'find policy.  Which returns nothing in the keys.  So we only need to call
    'the find business type thing.
    '
    ''ReDim vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5)
    '

    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNamePartyCnt

    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = v_lPartyCnt
    '

    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameShortName

    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = v_sShortName
    '

    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNamePolicyNo

    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = ""
    '

    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.PMKeyNameInsFileCnt

    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = v_lInsFileCnt ' 0
    '

    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.PMKeyNameInsFolderCnt

    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = v_lInsuranceFolderCnt ' 0
    '

    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMNavKeyConst.PMKeyNameBusinessTypeCode

    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = "GIIM" ' 0
    '
    'Dim temp_oObject As Object
    'm_lReturn = g_oObjectManager.GetInstance(temp_oObject, sClassName:="iGIIWFindBusTypeDefPol.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
    'oObject = temp_oObject
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '

    'm_lReturn = oObject.SetKeys(vKeyArray:=vKeyArray)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    'oObject = Nothing
    'Return result
    'End If
    '

    'm_lReturn = oObject.Start()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    'oObject = Nothing
    'Return result
    'End If
    '

    'r_lStatus = oObject.Status
    '

    'm_lReturn = oObject.GetKeys(vKeyArray:=vKeyArray)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    'oObject = Nothing
    'Return result
    'End If
    '

    'r_lPolicyKey = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0))

    'r_lDefaultPolicyID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1))

    'r_lBusinessTypeId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2))
    '

    'm_lReturn = oObject.Terminate()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    'oObject = Nothing
    'Return result
    'End If
    '
    'oObject = Nothing
    '
    'vKeyArray = Nothing
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GIIMNewBusiness Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GIIMNewBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    '
    ' Name: GetPolicyType
    '
    ' Description:
    '
    ' History: 14/09/1999 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function GetPolicyType(ByRef v_bFromEvent As Boolean, ByRef v_lInsuranceFileCnt As Integer, ByRef v_lPolicyTypeId As Integer) As Integer
        Dim result As Integer = 0
        Dim iPMBFindPolicyType As Object


        Dim oObject As Object
        Dim lStatus As gPMConstants.PMEReturnCode



        result = gPMConstants.PMEReturnCode.PMTrue

        'Dim temp_oObject As Object
        'm_lReturn = g_oObjectManager.GetInstance(temp_oObject, sClassName:="iPMBFindPolicyType.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
        'oObject = temp_oObject


        'oObject.CallingAppName = ACApp

        ''SD 01/08/2002 Scalability changes

        'm_lReturn = oObject.SetProcessModes(vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vTransactionType:="", vEffectiveDate:=DateTime.Now)


        'oObject.FromEvent = v_bFromEvent

        'oObject.InsuranceFileCnt = v_lInsuranceFileCnt


        'm_lReturn = oObject.Start()


        'lStatus = oObject.Status


        'v_lPolicyTypeId = oObject.PolicyTypeId


        'm_lReturn = oObject.Terminate()

        'oObject = Nothing

        'If lStatus = gPMConstants.PMEReturnCode.PMCancel Then
        '    result = gPMConstants.PMEReturnCode.PMCancel
        'End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: GetDefaultPolicy
    '
    ' Description:
    '
    ' History: 05/06/2001 JSB - Created.
    '
    ' ***************************************************************** '
    Private Function GetDefaultPolicy(ByVal v_bFromEvent As Boolean, ByVal v_lInsuranceFileCnt As Integer, ByVal v_iBusinessTypeID As Integer, ByRef r_lDefaultPolicyID As Integer, ByRef r_lGISPolicyLinkID As Integer) As Integer
        Dim result As Integer = 0
        Dim iGIIFindDefaultPolicy As Object


        'TODO
        'Dim oObject As iGIIFindDefaultPolicy.Interface_Renamed
        Dim oObject As Object
        Dim lStatus As gPMConstants.PMEReturnCode
        Dim vKeyArray(,) As Object
        Dim vReturnKeyArray(,) As Object
        Dim lDefaultPolicyID, lGISPolicyLinkID As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        'create an instance of default policy object
        Dim temp_oObject As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oObject, sClassName:="iGIIFindDefaultPolicy.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
        oObject = temp_oObject


        oObject.CallingAppName = ACApp

        'set process modes
        'SD 01/08/2002 Scalability changes

        m_lReturn = oObject.SetProcessModes(vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vTransactionType:="", vEffectiveDate:=DateTime.Now)

        'populate array that we are going to use to set keys
        ReDim vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1)


        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameBusinessTypeId

        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = v_iBusinessTypeID


        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameDefaultRequired

        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = "True"

        'set key

        If oObject.SetKeys(vKeyArray:=vKeyArray) <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("Setkeys failed", Application.ProductName)
            Return result
        End If

        vKeyArray = Nothing


        m_lReturn = oObject.Start()


        lStatus = oObject.Status

        'Get keys from DefaultPolicy object

        If oObject.GetKeys(vKeyArray:=vReturnKeyArray) <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("Setkeys failed", Application.ProductName)
            Return result
        End If

        'loop through keys and pick up the values that we need( should only be these 2 set, just did this to be on the safe side)

        For iCnt As Integer = vReturnKeyArray.GetLowerBound(1) To vReturnKeyArray.GetUpperBound(1)
            Select Case vReturnKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iCnt)
                Case PMNavKeyConst.PMKeyNameDefaultPolicyId

                    lDefaultPolicyID = CInt(vReturnKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iCnt))
                Case PMNavKeyConst.PMKeyNamePolicyKey

                    lGISPolicyLinkID = CInt(vReturnKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iCnt))
                Case Else
            End Select
        Next iCnt

        'set values to pass back out
        r_lDefaultPolicyID = lDefaultPolicyID
        r_lGISPolicyLinkID = lGISPolicyLinkID

        'Terminate the object

        oObject.Dispose()

        'set it to nothing
        oObject = Nothing

        'check the status returned
        If lStatus = gPMConstants.PMEReturnCode.PMCancel Then
            result = gPMConstants.PMEReturnCode.PMCancel
        End If

        Return result

    End Function
    '*******************************************************************************
    ' Name: GetDPASettings
    '
    ' Description: Get the DPA option settings.
    '
    ' Edit History:
    '
    '       PW231204 - PN15756 - Created (copied and amended from iPMBFindParty).
    '*******************************************************************************
    Public Function GetDPASettings() As Integer

        Dim result As Integer = 0
        Dim sTemp As String = ""
        Dim vTemp As String = ""
        Dim lReturn As Integer
        Dim sValue As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            g_bDPAIsActive = False
            g_bDPAIsEnforced = False

            If g_sUnderwritingOrAgency <> "A" Then
                ' not used in underwriting systems
                Return result
            End If

            ' is the FSA module switched on
            lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableFSACompliance, v_vBranch:=g_iLanguageID, r_vUnderwriting:=vTemp)


            If Convert.IsDBNull(vTemp) Or IsNothing(vTemp) Then
                Return result
            ElseIf Conversion.Val(vTemp) <> 1 Then
                Return result
            End If

            g_bDPAIsActive = True

            ' FSA Phase 3.1
            m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=93, r_sOptionValue:=sValue)

            If sValue = "1" Then
                g_bDPAIsEnforced = True
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDPASettings Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDPASettings", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result





            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: FSAPolicyDisclosure
    '
    ' Description:
    '
    ' History: FSA Phase 3.1 19/1//2004
    '
    ' ***************************************************************** '
    Public Function FSAPolicyDisclosure(ByRef v_lPartyCnt As Integer, ByRef v_lInsuranceFolderCnt As Integer, ByRef v_lInsuranceFileCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim iPMBFSAPolicyDisclosure As Object


        Dim oObject As Object
        Dim lStatus As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oObject As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oObject, sClassName:="iPMBFSAPolicyDisclosure.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oObject = temp_oObject


            oObject.CallingAppName = ACApp

            'SD 01/08/2002 Scalability changes

            m_lReturn = oObject.SetProcessModes(vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vTransactionType:="", vEffectiveDate:=DateTime.Now)


            oObject.PartyCnt = v_lPartyCnt

            oObject.InsuranceFolderCnt = v_lInsuranceFolderCnt

            oObject.InsuranceFileCnt = v_lInsuranceFileCnt


            m_lReturn = oObject.Start()


            oObject.Dispose()

            oObject = Nothing

            If lStatus = gPMConstants.PMEReturnCode.PMCancel Then
                result = gPMConstants.PMEReturnCode.PMCancel
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FSAPolicyDisclosure Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FSAPolicyDisclosure", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ShowEvents
    '
    ' Description: Displays events information.
    '
    ' ***************************************************************** '
    Public Function ShowEvents(ByVal v_lPartyCnt As Integer, ByVal v_sShortName As String, ByVal v_sPartyType As String, ByVal v_sResolvedName As String, Optional ByVal v_lInsuranceFolderCnt As Integer = 0, Optional ByVal v_lInsuranceFileCnt As Integer = 0, Optional ByVal v_sPolicyDesc As String = "", Optional ByVal v_lClaimCnt As Integer = 0, Optional ByVal v_sClaimDesc As String = "") As Integer

        Dim result As Integer = 0
        Dim fIndex As Integer
        Dim lInsuranceFileCnt, lClaimCnt As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not False Then
                lInsuranceFileCnt = v_lInsuranceFileCnt
            End If

            If Not False Then
                lClaimCnt = v_lClaimCnt
            End If

            'This bit may need a little more thought.  We could have multiple versions
            'of this form open, at client level or one for each policy.
            ' Make sure it's not already displayed.
            For Each frmChild As Form In m_frmParentMdiForm.MdiChildren
                If frmChild.Name = "frmListEvents" Then
                    'Is it this version?
                    If (ReflectionHelper.GetMember(frmChild, "InsuranceFileCnt") = lInsuranceFileCnt) And (ReflectionHelper.GetMember(frmChild, "ClaimCnt") = lClaimCnt) Then
                        ' Switch focus to it

                        m_lReturn = ReflectionHelper.Invoke(frmChild, "SwitchTo", New Object() {})
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Log Error Message
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display current Event List form." & Environment.NewLine & _
                                               "Please switch to form manually.", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowEvents", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        End If
                        Return result
                    End If
                End If
            Next

            ' If not then create a new one
            fIndex = FindFreeIndex()

            Document(fIndex) = New frmListEvents(m_frmParentMdiForm)

            Document(fIndex).ModuleClass = Me

            Document(fIndex).PartyCnt = v_lPartyCnt

            Document(fIndex).ShortName = v_sShortName

            Document(fIndex).ResolvedName = v_sResolvedName

            Document(fIndex).PartyType = v_sPartyType

            If Not False Then

                Document(fIndex).InsuranceFolderCnt = v_lInsuranceFolderCnt
            End If

            If Not False Then

                Document(fIndex).InsuranceFileCnt = v_lInsuranceFileCnt
            End If

            If Not False Then

                Document(fIndex).PolicyDesc = v_sPolicyDesc
            End If

            'MSS270901 - Changed to v_lClaimCnt. Was checking IsMissing(v_lInsuranceFolderCnt)
            ' Assuming this is a cut 'n' paste type bug
            If Not False Then

                Document(fIndex).ClaimCnt = v_lClaimCnt
            End If

            If Not False Then

                Document(fIndex).ClaimDesc = v_sClaimDesc
            End If


            Document(fIndex).Tag = fIndex


            Document(fIndex).LoadInterface()

            Document(fIndex).Show()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowEvents Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowEvents", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ShowEventDetail
    '
    ' Description: Displays event information.
    '
    ' ***************************************************************** '
    Public Function ShowEventDetail(ByVal v_lEventCnt As Integer, ByVal v_sPartyType As String, ByVal v_lPartyCnt As Integer, ByVal v_sShortName As String, ByVal v_sResolvedName As String, ByVal v_lInsuranceFileCnt As Integer, ByVal v_sInsReference As String, ByVal v_lInsuranceFileStructureId As Integer, ByVal v_lClaimCnt As Integer, ByVal v_sClaimDesc As String, ByVal v_lOldAddressCnt As Integer, ByVal v_lNewAddressCnt As Integer, ByVal v_lDocumentCnt As Integer, ByVal v_sEventType As String, ByVal v_lPolicyTypeId As Integer, ByVal v_lOldPartyTypeID As Integer, Optional ByVal v_sDocumentRef As String = "", Optional ByVal v_dtNoteDate As Date = #12/30/1899#, Optional ByVal v_lFsaComplaintFolderCnt As Integer = 0) As Object
        'JAS 10012005 PN17985
        'MSS270901 - Made last 2 optional for UW compatability
        'sj 15/09/2003 - Add v_lFsaComplaintFileCnt
        Dim result As Object = Nothing
        Dim fIndex As Integer
        'eck 150201 documaster command string
        Dim sCommand As String = ""

        Dim oDocTemplate As Object
        Dim lDocumentTemplateId, lDocumentTypeId As Integer
        Dim vKeyArray As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'MSS270901 - Added for UW merge
            'This bleeding thing is _always_ 0 if we get client events
            If v_lPolicyTypeId = 0 Then
                '        m_lReturn = GetPolicyType(v_bFromEvent:=True, _
                ''                                  v_lInsuranceFileCnt:=v_lInsuranceFileCnt, _
                ''                                  v_lPolicyTypeId:=v_lPolicyTypeId)

                'SET 10112002 ISS1256 - should pass event count not insurance file count
                m_lReturn = GetPolicyType(v_bFromEvent:=True, v_lInsuranceFileCnt:=v_lEventCnt, v_lPolicyTypeId:=v_lPolicyTypeId)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If
            End If
            'MSS270901 - Merge end

            'AK 270401
            m_bRenewalEvent = False


            'When a GII policy is MTAd or renewed a new policy is created...
            'So when it's GII and not the latest version we need to show the details

            Select Case v_sEventType.Trim()
                Case "NEWCLIENT", "NEWCLAIM", "REPORT", "MAILSHOT", "PTYPECHNG", "CLICHANGE" 'PN21979
                    Return result

                Case "NEWPOLICY"
                    Select Case v_lPolicyTypeId
                        'TF141102 - Also new Schemes policy type
                        Case PMBConst.PMBPolicyTypeGIIMotor

                            m_lReturn = CInt(ShowPolicySummary(v_lPartyCnt:=v_lPartyCnt, v_sPartyType:=v_sPartyType, v_lInsuranceFolderCnt:=v_lEventCnt, v_lInsFileCnt:=v_lEventCnt, v_sShortName:=v_sShortName, v_sInsReference:=v_sInsReference, v_lPolicyTypeId:=v_lPolicyTypeId))
                        Case PMBConst.PMBPolicyTypeSchemes


                            m_lReturn = CInt(ShowPolicyDetail(v_lPartyCnt:=v_lPartyCnt, v_sPartyType:=v_sPartyType, v_lInsuranceFolderCnt:=v_lEventCnt, v_lInsFileCnt:=v_lEventCnt, v_sShortName:=v_sShortName, v_sInsReference:=v_sInsReference, v_lInsuranceFileStructureId:=v_lInsuranceFileStructureId, v_bFromEvent:=True, v_lPolicyTypeId:=v_lPolicyTypeId, v_vGeminiPolicyStatus:=DBNull.Value))
                        Case PMBConst.PMBPolicyTypeGeneral
                            If v_lPolicyTypeId <> 0 Then
                                If v_lPolicyTypeId <> PMBConst.PMBPolicyTypeUnderwriting Then


                                    m_lReturn = CInt(ShowPolicyDetail(v_lPartyCnt:=v_lPartyCnt, v_sPartyType:=v_sPartyType, v_lInsuranceFolderCnt:=v_lEventCnt, v_lInsFileCnt:=v_lEventCnt, v_sShortName:=v_sShortName, v_sInsReference:=v_sInsReference, v_lInsuranceFileStructureId:=v_lInsuranceFileStructureId, v_bFromEvent:=True, v_lPolicyTypeId:=v_lPolicyTypeId, v_vGeminiPolicyStatus:=DBNull.Value))
                                End If
                            End If
                        Case Else
                            Return result
                    End Select

                Case "ADDCHANGE"
                    m_lReturn = ShowChangeOfAddress(v_lOldAddressCnt:=v_lOldAddressCnt, v_lNewAddressCnt:=v_lNewAddressCnt)

                    Return result

                Case "POLCHANGE"

                    'MSS270901 - Added check from UW for merge
                    If v_lPolicyTypeId <> 0 Then
                        If v_lPolicyTypeId <> PMBConst.PMBPolicyTypeUnderwriting Then


                            m_lReturn = CInt(ShowPolicyDetail(v_lPartyCnt:=v_lPartyCnt, v_sPartyType:=v_sPartyType, v_lInsuranceFolderCnt:=v_lEventCnt, v_lInsFileCnt:=v_lEventCnt, v_sShortName:=v_sShortName, v_sInsReference:=v_sInsReference, v_lInsuranceFileStructureId:=v_lInsuranceFileStructureId, v_bFromEvent:=True, v_lPolicyTypeId:=v_lPolicyTypeId, v_vGeminiPolicyStatus:=DBNull.Value))
                        End If
                    End If
                    Return result
                    'MSS270901 - Merge end

                Case "CLACHANGE", "DELCLAIM"
                    MessageBox.Show("Not yet implemented", Application.ProductName)
                    Return result

                Case "DELCLIENT"
                    MessageBox.Show("Not yet implemented", Application.ProductName)
                    Return result

                    fIndex = FindFreeIndex()

                    Select Case v_sPartyType
                        Case "P"
                            Document(fIndex) = New frmPartyPC(m_ofrmMDI)
                            ' Save the old party count

                            Document(fIndex).Caption = "Personal Client : [" & v_sResolvedName.Trim() & "]"

                        Case "C"
                            Document(fIndex) = New frmPartyCC(m_ofrmMDI)
                            ' Save the old party count

                            Document(fIndex).Caption = "Corporate Client : [" & v_sResolvedName.Trim() & "]"

                        Case "G"
                            Document(fIndex) = New frmPartyGC(m_ofrmMDI)

                            Document(fIndex).Caption = "Group Client : [" & v_sResolvedName.Trim() & "]"

                    End Select

                    Document(fIndex).ModuleClass = Me

                    Document(fIndex).FromEvent = True

                    Document(fIndex).PartyCnt = v_lEventCnt

                    Document(fIndex).ShortName = v_sShortName

                    Document(fIndex).Tag = fIndex

                    Document(fIndex).LoadInterface()

                    Document(fIndex).Show()

                    Return result

                Case "DELPOLICY"
                    MessageBox.Show("Not yet implemented", Application.ProductName)
                    Return result



                    m_lReturn = CInt(ShowPolicyDetail(v_lPartyCnt:=v_lPartyCnt, v_sPartyType:=v_sPartyType, v_lInsuranceFolderCnt:=v_lEventCnt, v_lInsFileCnt:=v_lEventCnt, v_sShortName:=v_sShortName, v_sInsReference:=v_sInsReference, v_lInsuranceFileStructureId:=v_lInsuranceFileStructureId, v_bFromEvent:=True, v_lPolicyTypeId:=v_lPolicyTypeId, v_vGeminiPolicyStatus:=DBNull.Value))

                    Return result

                Case "DOCUMENT"

                    ' Load documaster with document via document number
                    'eck 150201 Pass entity and level in case Dm is already running and
                    '           activate needs to be called

                    If v_sInsReference <> "" Then
                        sCommand = "SBO" & v_sInsReference & "2"
                    Else
                        sCommand = "SBO" & v_sShortName & "1"
                    End If

                    If v_lDocumentCnt < 1 Then
                        MessageBox.Show("Document not attached.", "Document Not Attached", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Return gPMConstants.PMEReturnCode.PMTrue
                    End If

                    m_lReturn = ShowDocumasterDocument(v_lDocumentCnt, sCommand)
                    'eck end
                    Return result

                Case "TRANSACT"

                    If v_sInsReference <> "" Then
                        sCommand = "SBO" & v_sInsReference & "2"
                    Else
                        sCommand = "SBO" & v_sShortName & "1"
                    End If

                    'DN 26/02/02 - Only cal documaster if a document exists
                    If v_lDocumentCnt < 1 Then
                        MessageBox.Show("There are no documents attached to this transaction event", "Missing Document", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Return gPMConstants.PMEReturnCode.PMTrue
                    End If

                    m_lReturn = ShowDocumasterDocument(v_lDocumentCnt, sCommand)

                    Return result


                    '    'SSL26032001 -- Applying double click option for Renewal events
                    '                                     v_lInsuranceFolderCnt:=,
                Case "RENEWAL", "RENPOLCHG"

                    'AK 270401 - As Renewal events do not store polisy versions in events table, the parameter
                    '            v_bFromEvent need to be set as 'False', so that policy information is read from
                    '            Insurance_file rather than Event_Policy, but we need to convey the event flag
                    '            so I am using RenewalEvent Flag for that

                    m_bRenewalEvent = True


                    m_lReturn = CInt(ShowPolicyDetail(v_lPartyCnt:=v_lPartyCnt, v_sPartyType:=v_sPartyType, v_lInsuranceFolderCnt:=v_lEventCnt, v_lInsFileCnt:=v_lInsuranceFileCnt, v_sShortName:=v_sShortName, v_sInsReference:=v_sInsReference, v_lInsuranceFileStructureId:=v_lInsuranceFileStructureId, v_bFromEvent:=False, v_lPolicyTypeId:=v_lPolicyTypeId, v_vGeminiPolicyStatus:=DBNull.Value))


                    Return result
                    'SSL26032001 -- End

                Case "PUBNOTES"

                    If v_lClaimCnt > 0 Then
                        m_lReturn = CallNotes(v_sEntityType:="Claim", v_lEntityCnt:=v_lClaimCnt, v_sTextType:="Public", v_lPartyCnt:=v_lPartyCnt, v_sNoteDate:=v_dtNoteDate)
                    ElseIf v_lInsuranceFileCnt > 0 Then
                        m_lReturn = CallNotes(v_sEntityType:="Policy", v_lEntityCnt:=v_lInsuranceFileCnt, v_sTextType:="Public", v_lPartyCnt:=v_lPartyCnt, v_sNoteDate:=v_dtNoteDate)
                    Else
                        m_lReturn = CallNotes(v_sEntityType:="Party", v_lEntityCnt:=v_lPartyCnt, v_sTextType:="Public", v_lPartyCnt:=0, v_sNoteDate:=v_dtNoteDate)
                    End If

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Case "POLCMPLT", "CLMCMPLT", "GENCMPLT"

                    'JAS 10012005 PN17985 now using Folder not File cnt
                    If v_lFsaComplaintFolderCnt > 0 Then
                        m_lReturn = CallComplaint(v_lFsaComplaintFolderCnt:=v_lFsaComplaintFolderCnt)
                    End If



                Case Else

                    MessageBox.Show("Unknown event", "Client Manager")
                    Return result

            End Select

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowEventDetail Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowEventDetail", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: ShowTextFiles
    '
    ' Description: Displays text file information.
    '
    ' ***************************************************************** '
    Public Function ShowTextFiles(ByVal v_lPartyCnt As Integer, ByVal v_sShortName As String, ByVal v_sPartyType As String, ByVal v_sResolvedName As String, Optional ByVal v_lInsuranceFolderCnt As Integer = 0, Optional ByVal v_lInsuranceFileCnt As Integer = 0, Optional ByVal v_sPolicyDesc As String = "", Optional ByVal v_lRiskCodeId As Integer = 0, Optional ByVal v_lRiskGroupID As Integer = 0, Optional ByVal v_lClaimCnt As Integer = 0, Optional ByVal v_sClaimDesc As String = "") As Integer

        Dim result As Integer = 0
        Dim fIndex As Integer
        Dim lInsuranceFileCnt, lClaimCnt As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not False Then
                lInsuranceFileCnt = v_lInsuranceFileCnt
            End If

            If Not False Then
                lClaimCnt = v_lClaimCnt
            End If

            'This bit may need a little more thought.  We could have multiple versions
            'of this form open, at client level or one for each policy.
            ' Make sure it's not already displayed.

            For Each frmChild As Form In m_frmParentMdiForm.MdiChildren
                If frmChild.Name = "frmTextFiles" Then
                    'Is it this version?
                    If (ReflectionHelper.GetMember(frmChild, "InsuranceFileCnt") = lInsuranceFileCnt) And (ReflectionHelper.GetMember(frmChild, "ClaimCnt") = lClaimCnt) Then
                        ' Switch focus to it

                        m_lReturn = ReflectionHelper.Invoke(frmChild, "SwitchTo", New Object() {})
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Log Error Message
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display current Text Files form." & Environment.NewLine & _
                                               "Please switch to form manually.", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowTextFiles", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        End If
                        Return result
                    End If
                End If
            Next

            ' If not then create a new one
            fIndex = FindFreeIndex()

            Document(fIndex) = New frmTextFiles(m_frmParentMdiForm)

            Document(fIndex).ModuleClass = Me

            Document(fIndex).PartyCnt = v_lPartyCnt

            Document(fIndex).ShortName = v_sShortName

            Document(fIndex).ResolvedName = v_sResolvedName
            'Document(fIndex).PartyType = v_sPartyType

            If Not False Then

                Document(fIndex).InsuranceFolderCnt = v_lInsuranceFolderCnt
            End If

            If Not False Then

                Document(fIndex).InsuranceFileCnt = v_lInsuranceFileCnt
            End If

            If Not False Then

                Document(fIndex).PolicyDesc = v_sPolicyDesc
            End If

            If Not False Then

                Document(fIndex).RiskCodeId = v_lRiskCodeId
            End If

            If Not False Then

                Document(fIndex).RiskGroupId = v_lRiskGroupID
            End If

            If Not False Then

                Document(fIndex).ClaimCnt = v_lClaimCnt
            End If

            If Not False Then

                Document(fIndex).ClaimDesc = v_sClaimDesc
            End If


            Document(fIndex).Tag = fIndex

            Document(fIndex).LoadInterface()

            Document(fIndex).Show()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowTextFiles Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowTextFiles", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ProcessTasks
    '
    ' Description: Shows Task editor for work manager
    '
    ' MSS090701
    ' ***************************************************************** '

    Public Function ProcessTasks(ByVal v_lPartyCnt As Integer, ByVal v_sShortName As String, ByVal v_sPartyType As String, ByVal v_sResolvedName As String, Optional ByVal v_lInsuranceFolderCnt As Integer = 0, Optional ByVal v_lInsuranceFileCnt As Integer = 0, Optional ByVal v_sPolicyDesc As String = "") As Integer
        Dim result As Integer = 0
        Dim bSIRDocTemplate, iPMWrkTaskInstanceTemp As Object

        Dim oWorkManager As Object

        Dim oDocBusiness As bSIRDocTemplate.Business
        Dim vPartyType As Object
        'Dim sPartyType, sResolvedName As String
        Dim sPartyType As String = "", sResolvedName As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If v_sPartyType.Length = 0 Then

                Dim temp_oDocBusiness As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oDocBusiness, "bSIRDocTemplate.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oDocBusiness = temp_oDocBusiness


                m_lReturn = oDocBusiness.GetPartyType(r_vPartytype:=vPartyType, v_lPartyCnt:=v_lPartyCnt)
            End If

            Dim temp_oWorkManager As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oWorkManager, sClassName:="iPMWrkTaskInstanceTemp.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oWorkManager = temp_oWorkManager


            m_lReturn = oWorkManager.Initialise

            If Information.IsArray(vPartyType) Then

                sPartyType = CStr(vPartyType(0, 0))

                sResolvedName = CStr(vPartyType(1, 0))
            End If

            ' Pass needed properties to the interface

            oWorkManager.InsuranceFileCnt = v_lInsuranceFileCnt

            oWorkManager.InsuranceFolderCnt = v_lInsuranceFolderCnt

            oWorkManager.PartyCnt = v_lPartyCnt

            oWorkManager.PartyType = IIf(v_sPartyType.Trim().Length, v_sPartyType.Trim(), sPartyType.Trim())
            'MKW250703 PN4344 START - Display Client Name not code switched for broking use.
            'MKW220804 PN12591 START - Submit PartyCode to form keys and resolved name to display

            oWorkManager.Party = v_sShortName


            oWorkManager.Customer = v_sShortName

            'MKW220804 PN12591 END
            'MKW250703 PN4344 END

            oWorkManager.ResolvedName = IIf(v_sResolvedName.Length, v_sResolvedName, sResolvedName.Trim())

            oWorkManager.PolicyRef = v_sPolicyDesc


            'Set the ID of the current attached task.

            oWorkManager.CallingAppName = "Raised Task"


            m_lReturn = oWorkManager.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired, vEffectiveDate:=DateTime.Now)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Start the Form

            m_lReturn = oWorkManager.Start

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                oWorkManager.Dispose()
                oWorkManager = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            oWorkManager.Dispose()
            oWorkManager = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to show Task Editor", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessTasks", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ShowTaskList
    '
    ' Description: Shows Task List current Party or Policy
    '
    ' MSS100701
    ' ***************************************************************** '
    Public Function ShowTaskList(ByVal v_lPartyCnt As Integer, Optional ByVal v_lInsuranceFileCnt As Integer = 0) As Integer
        Dim result As Integer = 0
        Dim iPMBTaskList As Object


        Dim oTaskList As Object


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oTaskList As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oTaskList, sClassName:="iPMBTaskList.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oTaskList = temp_oTaskList


            m_lReturn = oTaskList.Initialise



            ' Pass needed properties to the interface
            'PN17519 re-instated "oTaskList.InsuranceFileCnt = v_lInsuranceFileCnt" which was removed by fix for PN15525

            oTaskList.InsuranceFileCnt = v_lInsuranceFileCnt

            oTaskList.PartyCnt = v_lPartyCnt

            ' Start the Form

            m_lReturn = oTaskList.Start

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                oTaskList.Dispose()
                oTaskList = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            oTaskList.Dispose()
            oTaskList = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to show Task List", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowTaskList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ProcessRiskRegister
    '
    ' Description: Displays text file information.
    '
    ' ***************************************************************** '
    Public Function ProcessRiskRegister(ByVal v_lPartyCnt As Integer, ByVal v_lMode As Integer) As Integer
        Dim result As Integer = 0
        Dim iPMBRiskRegister As Object


        Dim oObject As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oObject As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oObject, sClassName:="iPMBRiskRegister.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oObject = temp_oObject

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oObject.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)

            oObject.PartyCnt = v_lPartyCnt


            oObject.Mode = v_lMode


            m_lReturn = oObject.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oObject = Nothing
                Return result
            End If


            oObject.Dispose()
            oObject = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessRiskRegister Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRiskRegister", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function ShowBankGuarantee(ByRef lPartyCnt As Integer) As Integer
        Dim Catch_Renamed As Boolean = False
        Dim result As Integer = 0
        Dim iSIRBankGuarantee As Object

        Const kMethodName As String = "ShowBankGuarantee"
        Try
            Catch_Renamed = True

            result = gPMConstants.PMEReturnCode.PMTrue


            Dim oBankGuarantee As iSIRBankGuarantee.Interface_Renamed
            Dim lStatus As gPMConstants.PMEReturnCode


            ' Get an instance
            Dim temp_oBankGuarantee As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oBankGuarantee, sClassName:="iSIRBankGuarantee.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oBankGuarantee = temp_oBankGuarantee
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "iSIRBankGuarantee.Interface Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            m_lReturn = oBankGuarantee.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vTransactionType:="", vEffectiveDate:=DateTime.Now)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetProcessModes Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            'Start - Sankar - Bank Guarantee Bug Fixing

            oBankGuarantee.CallFromClientManager = True
            'End - Sankar - Bank Guarantee Bug Fixing

            oBankGuarantee.PartyCnt = lPartyCnt

            m_lReturn = oBankGuarantee.Start()


            lStatus = oBankGuarantee.Status

            If lStatus = gPMConstants.PMEReturnCode.PMCancel Then

                oBankGuarantee.Dispose()
                oBankGuarantee = Nothing
                Return result
            End If

            Return result

        Catch excep As System.Exception
            If Not Catch_Renamed Then
                Throw excep
            End If


            GoTo Finally_Renamed
            If Catch_Renamed Then


                ' DO Not Call any functions before here or the error will be lost
                iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=excep)

                ' If you want to rollback a transaction or something, do it here
            End If
Finally_Renamed:
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ShowChangeOfAddress
    '
    ' Description:
    '
    ' History: 05/09/1999 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function ShowChangeOfAddress(ByVal v_lOldAddressCnt As Integer, ByVal v_lNewAddressCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim iPMBAddressChange As Object


        Dim oAddressChange As Object
        'Dim oAddressChange As iPMBAddressChange.Interface

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oAddressChange As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oAddressChange, sClassName:="iPMBAddressChange.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oAddressChange = temp_oAddressChange

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            oAddressChange.Task = gPMConstants.PMEComponentAction.PMView


            oAddressChange.OldAddressCnt = v_lOldAddressCnt

            oAddressChange.NewAddressCnt = v_lNewAddressCnt


            m_lReturn = oAddressChange.Start()


            oAddressChange.Dispose()

            oAddressChange = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowChangeOfAddress Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowChangeOfAddress", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: ShowListofRisks
    '
    ' Description: Displays list of risks on a policy
    '              'CT 13/09/00 added function to display the risklist user control (underwriting poliices)
    ' ***************************************************************** '
    Public Function ShowListofRisks(ByVal v_lPartyCnt As Integer, ByVal v_sShortName As String, ByVal v_sPartyType As String, ByVal v_sResolvedName As String, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_sPolicyDesc As String, ByVal v_lRiskCodeId As Integer, ByVal v_lRiskGroupID As Integer, ByVal v_sInsuranceRef As String, ByVal v_bFromEvent As Boolean) As Integer




        Dim result As Integer = 0
        Dim fIndex As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Make sure its not already display.
            For iLoop1 As Integer = 0 To Application.OpenForms.Count - 1
                If Application.OpenForms.Item(iLoop1).Name = "frmListRiskUnderwriting" Then
                    ' Switch focus to it

                    m_lReturn = ReflectionHelper.Invoke(Application.OpenForms.Item(iLoop1), "SwitchTo", New Object() {})
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Log Error Message
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display current Risk List form." & Environment.NewLine & _
                                           "Please switch to form manually.", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowListofRisks", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    End If
                    Return result
                End If
            Next iLoop1

            ' If not then create a new one
            fIndex = FindFreeIndex()

            Document(fIndex) = New frmListRiskUnderWriting(m_frmParentMdiForm)

            Document(fIndex).ModuleClass = Me

            Document(fIndex).ShortName = v_sShortName
            ' Document(fIndex).ResolvedName = v_sResolvedName


            ' Document(fIndex).InsuranceFolderCnt = v_lInsuranceFolderCnt

            Document(fIndex).PartyCnt = v_lPartyCnt

            Document(fIndex).InsuranceFileCnt = v_lInsuranceFileCnt

            Document(fIndex).InsuranceRef = v_sInsuranceRef

            Document(fIndex).Tag = fIndex

            Document(fIndex).FromEvent = v_bFromEvent


            Document(fIndex).LoadInterface()

            Document(fIndex).Show()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowListofRisks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowListofRisks", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function




    ' ***************************************************************** '
    ' Name: ShowRisk
    '
    ' Description: Displays risk information.
    '
    ' ***************************************************************** '
    ' 19/10/00 CT new argument for risk screen id
    'eck070301 Add risk cnt option
    'ISS1498 JAS 05/12/02 add PolicyTypeId
    Public Function ShowRisk(ByVal v_lPartyCnt As Integer, ByVal v_sShortName As String, ByVal v_sPartyType As String, ByVal v_sResolvedName As String, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_sPolicyDesc As String, ByVal v_lRiskCodeId As Integer, ByVal v_lRiskGroupID As Integer, ByVal v_bFromEvent As Boolean, Optional ByVal v_lRiskScreenId As Integer = 0, Optional ByVal v_lRiskCnt As Integer = 0, Optional ByVal v_lPolicyTypeId As Integer = 0) As Object

        Dim result As Object = Nothing
        Dim fIndex As Integer
        Dim sName As String = ""
        ' CTAF 20020806
        Dim lPostQuoteRiskScreenID As Integer

        Try

            'CT 15/10/00 do nothing if they have not associated a screen with a  risk group
            If v_lRiskScreenId = 0 Then
                MessageBox.Show("Cannot show the risk as no risk screen has been associated with it.", Application.ProductName)
                Return result
            End If


            result = gPMConstants.PMEReturnCode.PMTrue

            If m_ofrmMDI.LiveForm = "frmPolicySummary" Then
                'We need to lock the policy
                m_lReturn = LockThePolicy(v_lInsuranceFileCnt:=v_lInsuranceFileCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

            End If

            ' CT 19/10/00 Change to use just 1 SBO risk screen (start)
            '    Select Case v_lRiskGroupId
            '    Case ACRGAviation, _
            ''         ACRGCombinedLiability, _
            ''         ACRGHouseholdBuildings, _
            ''         ACRGHouseholdCombined, _
            ''         ACRGHouseholdContents, _
            ''         ACRGPersonalAccident, _
            ''         ACRGPrivatePublicHire, _
            ''         ACRGPropertyOwners, _
            ''         ACRGTravel

            '        sName = "frmRisk"
            '
            '    Case ACRGCombinedMotor, _
            ''         ACRGMarine, _
            ''         ACRGOffices, _
            ''         ACRGShop
            '
            '        sName = "frmRisk2"
            '
            '    Case ACRGCommercialCombined
            '
            '        sName = "frmRisk3"
            '
            '    Case ACRGPrivateMotor, _
            ''         ACRGMotorFleet, _
            ''         ACRGMotorcycle
            '
            '        sName = "frmRisk4"

            '    Case ACRGFarm

            '        sName = "frmRisk5"

            '    Case Else

            '        sName = "frmRiskN"

            '    End Select

            sName = "frmSBORiskScreen"
            ' CT 19/10/00 Change to use just 1 SBO risk screen (end)

            'This bit may need a little more thought.  We could have multiple versions
            'of this form open, one for each policy.
            ' Make sure it's not already displayed.
            For iLoop1 As Integer = 0 To Application.OpenForms.Count - 1
                If Application.OpenForms.Item(iLoop1).Name = sName Then

                    'Is it this version?


                    If (ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "InsuranceFileCnt") = v_lInsuranceFileCnt) And (ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "FromEvent") = v_bFromEvent) Then
                        ' Switch focus to it

                        m_lReturn = ReflectionHelper.Invoke(Application.OpenForms.Item(iLoop1), "SwitchTo", New Object() {})
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Log Error Message
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display current Risk form." & Environment.NewLine & _
                                               "Please switch to form manually.", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowRisk", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        End If
                        Return result
                    End If
                End If
            Next iLoop1

            ' If not then create a new one
            fIndex = FindFreeIndex()

            ' CT 19/10/00 Change to use just 1 SBO risk screen (start)
            '    Select Case v_lRiskGroupId
            '    Case ACRGAviation, _
            ''         ACRGCombinedLiability, _
            ''         ACRGHouseholdBuildings, _
            ''         ACRGHouseholdCombined, _
            ''         ACRGHouseholdContents, _
            ''         ACRGPersonalAccident, _
            ''         ACRGPrivatePublicHire, _
            ''         ACRGPropertyOwners, _
            ''         ACRGTravel
            '
            '        Set Document(fIndex) = New frmRisk
            '
            '    Case ACRGCombinedMotor, _
            ''         ACRGMarine, _
            ''         ACRGOffices, _
            ''         ACRGShop
            '
            '        Set Document(fIndex) = New frmRisk2
            '
            '    Case ACRGCommercialCombined
            '
            '        Set Document(fIndex) = New frmRisk3
            '
            '    Case ACRGPrivateMotor, _
            ''         ACRGMotorFleet, _
            ''         ACRGMotorcycle
            '
            '        Set Document(fIndex) = New frmRisk4
            '
            '    Case ACRGFarm
            '
            '        Set Document(fIndex) = New frmRisk5
            '
            '    Case Else
            '
            '        Set Document(fIndex) = New frmRiskN
            '
            '    End Select

            Document(fIndex) = New frmSBORiskScreen(m_frmParentMdiForm)

            Document(fIndex).ModuleClass = Me

            ' CT 19/10/00 Change to use just 1 SBO risk screen (end)

            'TN20000809
            If m_ofrmMDI.LiveForm = "frmPolicy" Then

                Document(fIndex).PMRaiseEventState = PMBConst.PMBRaiseEventInParentObject
            Else

                Document(fIndex).PMRaiseEventState = PMBConst.PMBRaiseEventInChildObject
            End If


            Document(fIndex).PartyCnt = v_lPartyCnt

            Document(fIndex).ShortName = v_sShortName

            Document(fIndex).ResolvedName = v_sResolvedName
            'Document(fIndex).PartyType = v_sPartyType


            Document(fIndex).InsuranceFolderCnt = v_lInsuranceFolderCnt


            Document(fIndex).InsFileCnt = v_lInsuranceFileCnt


            Document(fIndex).InsReference = v_sPolicyDesc



            Document(fIndex).RiskCodeId = v_lRiskCodeId
            'eck070301
            If Not False Then

                Document(fIndex).RiskCnt = v_lRiskCnt
            End If
            'CT 23/10/00 Screen control used expects a RiskType as it  was an Underwriting
            'control. We just use Our risk group as the value
            'Document(fIndex).RiskGroupId = v_lRiskGroupId

            Document(fIndex).RiskTypeId = v_lRiskGroupID


            Document(fIndex).FromEvent = v_bFromEvent


            Document(fIndex).Task = gPMConstants.PMEComponentAction.PMEdit

            ' CTAF 20020806 start
            ' CTAF - Pass in the PostQuote screen id
            m_lReturn = GetPostQuoteScreenID(v_lRiskGroupID:=v_lRiskGroupID, r_lScreenID:=lPostQuoteRiskScreenID)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or lPostQuoteRiskScreenID = 0 Then
                ' Just use the normal screen id
                lPostQuoteRiskScreenID = v_lRiskScreenId
            End If

            'CT 19/10/00 need to pass in a gis screen id

            Document(fIndex).ScreenId = lPostQuoteRiskScreenID
            ' CTAF 20020806 end

            'ISS1498 JAS 05/12/02 - pass policytypeID so that form can disable OK button
            If Not False Then

                Document(fIndex).PolicyTypeId = v_lPolicyTypeId
            End If


            Document(fIndex).Tag = fIndex

            Document(fIndex).LoadInterface()

            Document(fIndex).Show()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowRisk", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: ShowRiskUnderwriting
    '
    ' Description: Displays risk information for underwriting policies.
    ' 'CT 14/09/00 created
    ' PM035142 - Modified (18/03/14) for Short Name
    ' ***************************************************************** '
    Public Function ShowRiskUnderwriting(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lRiskCodeId As Integer, ByVal v_lRiskGisScreenId As Integer, ByVal v_lRiskTypeId As Integer, ByVal v_bFromEvent As Boolean, Optional ByVal v_lIsReInsuranceAtRiskLevel As Integer = 0, Optional ByVal v_lInsuranceFolderCnt As Integer = 0, Optional ByVal v_sShortName As String = "", Optional ByVal nPartyCnt As Integer = 0) As Integer
        'MSS270901 - Added optional params for UW
        'PM035142 - Added optional params - v_sShortName
        Dim result As Integer = 0
        Dim fIndex As Integer
        Dim sName As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Dim objfrmRiskUnderwriting As New frmRiskUnderwriting(m_frmParentMdiForm)

            sName = "frmRiskUnderwriting"


            'For Each frm As Object In Application.OpenForms
            '    If Not frm Is Nothing Then
            '        If frm.Text = "Risk" OrElse frm.GetType().ToString() = objfrmRiskUnderwriting.GetType().ToString() Then
            '            MessageBox.Show("The Risk form is already open. Cannot open another instance.", "Risk Screen")
            '            Return result
            '        End If
            '    End If
            'Next
            'objfrmRiskUnderwriting = Nothing

            'This bit may need a little more thought.  We could have multiple versions
            'of this form open, one for each policy.
            ' Make sure it's not already displayed.
            For iLoop1 As Integer = 0 To Application.OpenForms.Count - 1
                If Application.OpenForms.Item(iLoop1).Name = sName Then

                    'Is it this version?


                    If (ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "InsFileCnt") = v_lInsuranceFileCnt) And (ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "FromEvent") = v_bFromEvent) Then
                        ' Switch focus to it

                        m_lReturn = ReflectionHelper.Invoke(Application.OpenForms.Item(iLoop1), "SwitchTo", New Object() {})
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Log Error Message
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display current Risk form." & Environment.NewLine & _
                                               "Please switch to form manually.", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowRiskUnderwriting", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        End If
                        Return result
                    End If
                End If
            Next iLoop1

            ' If not then create a new one
            fIndex = FindFreeIndex()

            Document(fIndex) = New frmRiskUnderwriting(m_frmParentMdiForm)

            Document(fIndex).ModuleClass = Me

            Document(fIndex).InsFileCnt = v_lInsuranceFileCnt


            Document(fIndex).RiskCodeId = v_lRiskCodeId

            Document(fIndex).RiskTypeId = v_lRiskTypeId

            'PM035142 - Added for Short Name
            Document(fIndex).shortName = v_sShortName

            Document(fIndex).ScreenId = v_lRiskGisScreenId

            Document(fIndex).FromEvent = v_bFromEvent

            Document(fIndex).partycnt = nPartyCnt
            'MSS270901 - Added for UW merge
            'TN20010111 Start
            If Not False Then

                Document(fIndex).IsReInsuranceAtRiskLevel = v_lIsReInsuranceAtRiskLevel
            End If
            'TN20010111 End

            'TN20010111 Start
            If Not False Then

                Document(fIndex).InsuranceFolderCnt = v_lInsuranceFolderCnt
            End If
            'TN20010111 End
            'MSS270901 - Merge end


            Document(fIndex).Task = gPMConstants.PMEComponentAction.PMView


            Document(fIndex).Tag = fIndex

            Document(fIndex).ShortName = m_sShortName
            Document(fIndex).LoadInterface()

            Document(fIndex).Show()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowRiskUnderwriting Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowRiskUnderwriting", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PSA 20092000

    ' ***************************************************************** '
    ' Name: ShowGIIHRisk
    '
    ' Description: Displays Gemini Household risk information.
    '
    ' ***************************************************************** '
    Public Function ShowGIIHRisk(ByVal v_lPartyCnt As Integer, ByVal v_sShortName As String, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim fIndex As Integer
        Dim sName As String = ""

        Try

            ' Make sure it's not already displayed.
            For iLoop1 As Integer = 0 To Application.OpenForms.Count - 1
                If Application.OpenForms.Item(iLoop1).Name = "frmGIIHRisk" Then

                    'Is it this version?

                    If ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "InsuranceFileCnt") = v_lInsuranceFileCnt Then
                        ' Switch focus to it

                        m_lReturn = ReflectionHelper.Invoke(Application.OpenForms.Item(iLoop1), "SwitchTo", New Object() {})
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Log Error Message
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display current Gemini Household Risk form." & Environment.NewLine & _
                                               "Please switch to form manually.", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowGIIHRisk", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        End If
                        Return result
                    End If
                End If
            Next iLoop1

            fIndex = FindFreeIndex()


            Document(fIndex) = New frmGIIHRisk(m_frmParentMdiForm)

            Document(fIndex).ModuleClass = Me

            Document(fIndex).InsFileCnt = v_lInsuranceFileCnt

            Document(fIndex).ShortName = v_sShortName

            Document(fIndex).PartyCnt = v_lPartyCnt

            Document(fIndex).InsuranceFolderCnt = v_lInsuranceFolderCnt

            Document(fIndex).Tag = fIndex

            Document(fIndex).LoadInterface()

            Document(fIndex).Show()

            Return result


            result = gPMConstants.PMEReturnCode.PMTrue

        Catch
        End Try



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowGIIHRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowGIIHRisk", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function

    ' PSA 20092000

    ' ***************************************************************** '
    ' Name: ShowGIIMRisk
    '
    ' Description: Displays Gemini Motor risk information.
    '
    ' ***************************************************************** '
    Public Function ShowGIIMRisk(ByVal v_lPartyCnt As Integer, ByVal v_sShortName As String, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim fIndex As Integer
        Dim sName As String = ""

        Try

            ' Make sure it's not already displayed.
            For iLoop1 As Integer = 0 To Application.OpenForms.Count - 1
                If Application.OpenForms.Item(iLoop1).Name = "frmGIIMRisk" Then

                    'Is it this version?

                    'NIIT - Replaced with the Migrated code 1144 
                    'If Application.OpenForms.Item(iLoop1).InsuranceFileCnt = v_lInsuranceFileCnt Then
                    If ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "InsuranceFileCnt") = v_lInsuranceFileCnt Then
                        ' Switch focus to it

                        m_lReturn = ReflectionHelper.Invoke(Application.OpenForms.Item(iLoop1), "SwitchTo", New Object() {})
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Log Error Message
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display current Gemini Motor Risk form." & Environment.NewLine & _
                                               "Please switch to form manually.", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowGIIMRisk", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        End If
                        Return result
                    End If
                End If
            Next iLoop1

            fIndex = FindFreeIndex()


            Document(fIndex) = New frmGIIMRisk(m_frmParentMdiForm)

            Document(fIndex).ModuleClass = Me

            Document(fIndex).InsFileCnt = v_lInsuranceFileCnt

            Document(fIndex).ShortName = v_sShortName

            Document(fIndex).PartyCnt = v_lPartyCnt

            Document(fIndex).InsuranceFolderCnt = v_lInsuranceFolderCnt

            Document(fIndex).Tag = fIndex

            Document(fIndex).LoadInterface()

            Document(fIndex).Show()

            Return result

            result = gPMConstants.PMEReturnCode.PMTrue

        Catch
        End Try



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowGIIMRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowGIIMRisk", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: ShowGIITRisk
    '
    ' Description: Displays Gemini Commercial Vehicle risk information.
    '
    ' PSA 25/04/2001 - Created
    ' ***************************************************************** '
    Public Function ShowGIITRisk(ByVal v_lPartyCnt As Integer, ByVal v_sShortName As String, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim fIndex As Integer
        Dim sName As String = ""

        Try

            ' Make sure it's not already displayed.
            For iLoop1 As Integer = 0 To Application.OpenForms.Count - 1
                If Application.OpenForms.Item(iLoop1).Name = "frmGIITRisk" Then

                    'Is it this version?

                    If ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "InsuranceFileCnt") = v_lInsuranceFileCnt Then
                        ' Switch focus to it

                        m_lReturn = ReflectionHelper.Invoke(Application.OpenForms.Item(iLoop1), "SwitchTo", New Object() {})
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Log Error Message
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display current Gemini CV Risk form." & Environment.NewLine & _
                                               "Please switch to form manually.", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowGIITRisk", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        End If
                        Return result
                    End If
                End If
            Next iLoop1

            fIndex = FindFreeIndex()


            Document(fIndex) = New frmGIITRisk(m_frmParentMdiForm)

            Document(fIndex).ModuleClass = Me

            Document(fIndex).InsFileCnt = v_lInsuranceFileCnt

            Document(fIndex).ShortName = v_sShortName

            Document(fIndex).PartyCnt = v_lPartyCnt

            Document(fIndex).InsuranceFolderCnt = v_lInsuranceFolderCnt

            Document(fIndex).Tag = fIndex

            Document(fIndex).LoadInterface()

            Document(fIndex).Show()

            Return result


            result = gPMConstants.PMEReturnCode.PMTrue

        Catch
        End Try



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowGIITRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowGIITRisk", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: CheckRisk
    '
    ' Description: Checks if the risk screen is open.
    '
    ' ***************************************************************** '
    Public Function CheckRisk(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lRiskGroupID As Integer, ByVal v_bFromEvent As Boolean, ByRef r_bThere As Boolean) As Integer

        Dim result As Integer = 0
        Dim fIndex As Integer
        Dim sName As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Select Case v_lRiskGroupID
                Case ACRGAviation, ACRGCombinedLiability, ACRGHouseholdBuildings, ACRGHouseholdCombined, ACRGHouseholdContents, ACRGPersonalAccident, ACRGPrivatePublicHire, ACRGPropertyOwners, ACRGTravel

                    sName = "frmRisk"

                Case ACRGCombinedMotor, ACRGMarine, ACRGOffices, ACRGShop

                    sName = "frmRisk2"

                Case ACRGCommercialCombined

                    sName = "frmRisk3"

                Case ACRGPrivateMotor, ACRGMotorFleet, ACRGMotorcycle

                    sName = "frmRisk4"

                Case ACRGFarm

                    sName = "frmRisk5"

                Case Else

                    sName = "frmRiskN"

            End Select

            ' Is it already displayed.
            For iLoop1 As Integer = 0 To Application.OpenForms.Count - 1
                If Application.OpenForms.Item(iLoop1).Name = sName Then

                    'Is it this version?


                    'NIIT - Replaced with the Migrated code 1144 
                    'If (Application.OpenForms.Item(iLoop1).InsFileCnt = v_lInsuranceFileCnt) And (Application.OpenForms.Item(iLoop1).FromEvent = v_bFromEvent) Then
                    If (ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "InsFileCnt") = v_lInsuranceFileCnt) And (ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "FromEvent") = v_bFromEvent) Then
                        ' Yes it is
                        r_bThere = True
                        Return result
                    End If
                End If
            Next iLoop1

            r_bThere = False

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckRisk", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckText
    '
    ' Description: Checks if the text screen is open.
    '
    ' ***************************************************************** '
    Public Function CheckText(ByVal v_lInsuranceFileCnt As Integer, ByVal v_bFromEvent As Boolean, ByRef r_bThere As Boolean) As Integer

        Dim result As Integer = 0
        Dim fIndex As Integer
        Dim sName As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sName = "frmTextFiles"

            ' Is it already displayed.
            For iLoop1 As Integer = 0 To Application.OpenForms.Count - 1
                If Application.OpenForms.Item(iLoop1).Name = sName Then

                    'Is it this version?


                    'NIIT - Replaced with the Migrated code 1144 
                    'If (Application.OpenForms.Item(iLoop1).InsuranceFileCnt = v_lInsuranceFileCnt) And (Application.OpenForms.Item(iLoop1).FromEvent = v_bFromEvent) Then
                    If (ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "InsuranceFileCnt") = v_lInsuranceFileCnt) And (ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "FromEvent") = v_bFromEvent) Then
                        ' Yes it is
                        r_bThere = True
                        Return result
                    End If
                End If
            Next iLoop1

            r_bThere = False

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckText Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckText", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckEvent
    '
    ' Description: Checks if the event list screen is open.
    '
    ' ***************************************************************** '
    Public Function CheckEvent(ByVal v_lInsuranceFileCnt As Integer, ByRef r_bThere As Boolean) As Integer

        Dim result As Integer = 0
        Dim fIndex As Integer
        Dim sName As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sName = "frmListEvents"

            ' Is it already displayed.
            For iLoop1 As Integer = 0 To Application.OpenForms.Count - 1
                If Application.OpenForms.Item(iLoop1).Name = sName Then

                    'Is it this version?

                    If ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "InsuranceFileCnt") = v_lInsuranceFileCnt Then
                        ' Yes it is
                        r_bThere = True
                        Return result
                    End If
                End If
            Next iLoop1

            r_bThere = False

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckEvent", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetPolicyRaiseEvent
    '
    ' Description: we've change data in the risk so let the parent object know
    '
    ' History : 09/08/2000 Tinny - Created
    ' ***************************************************************** '
    Public Function SetPolicyRaiseEvent(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Find the policy.
            For iCount As Integer = 0 To Application.OpenForms.Count - 1
                If Application.OpenForms.Item(iCount).Name = "frmPolicy" Then

                    'Is it this version?


                    If (ReflectionHelper.GetMember(Application.OpenForms.Item(iCount), "InsFileCnt") = v_lInsuranceFileCnt) And (Not ReflectionHelper.GetMember(Application.OpenForms.Item(iCount), "FromEvent")) Then
                        ' Yes it is

                        m_lReturn = ReflectionHelper.GetMember(Application.OpenForms.Item(iCount), "SetRaiseEvent")
                        Return result
                    End If
                End If
            Next iCount

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetPolicyRaiseEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetPolicyRaiseEvent", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetPolicyEventRaised
    '
    ' Description: We've raised an event in the risk, so let's let the policy know.
    '
    ' ***************************************************************** '
    Public Function SetPolicyEventRaised(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Find the policy.
            For iLoop1 As Integer = 0 To Application.OpenForms.Count - 1
                If Application.OpenForms.Item(iLoop1).Name = "frmPolicy" Then

                    'Is it this version?


                    If (ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "InsFileCnt") = v_lInsuranceFileCnt) And (Not ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "FromEvent")) Then
                        ' Yes it is

                        m_lReturn = ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "SetEventRaised")
                        Return result
                    End If
                End If
            Next iLoop1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetPolicyEventRaised Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetPolicyEventRaised", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RunReport
    '
    ' Description: Runs Crystal reports.
    '
    ' ***************************************************************** '
    Public Function RunReport(ByVal v_lPartyCnt As Integer, ByVal v_sReportName As String, Optional ByVal v_lInsuranceFileCnt As Integer = 0) As Integer
        Dim result As Integer = 0
        Dim iPMBReportPrint As Object 'PN13544



        Dim oReportPrint As iPMBReportPrint.Interface_Renamed

        Dim vKeyArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oReportPrint As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oReportPrint, sClassName:="iPMBReportPrint.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oReportPrint = temp_oReportPrint

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            If v_lInsuranceFileCnt = 0 Then 'PN13544
                ReDim vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3)
            Else
                ReDim vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5)
            End If


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = "report_name"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = v_sReportName

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = "report_print_options"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = 0

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = "param_name1"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = PMNavKeyConst.PMKeyNamePartyCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.PMKeyNamePartyCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = v_lPartyCnt

            If v_lInsuranceFileCnt > 0 Then 'PN13544


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = "param_name2"

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = PMNavKeyConst.PMKeyNameInsuranceFileCnt


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMNavKeyConst.PMKeyNameInsuranceFileCnt

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = v_lInsuranceFileCnt
            End If


            m_lReturn = oReportPrint.SetKeys(vKeyArray:=vKeyArray)


            m_lReturn = oReportPrint.Start

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
            End If


            oReportPrint.Dispose()

            oReportPrint = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RunReport Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RunReport", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function SetToolbar(ByVal v_sFormName As String, Optional ByVal v_bFromEvent As Boolean = False) As Integer

        ' CTAF 270900
        Dim result As Integer = 0
        Dim bInstalled As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            If m_ofrmMDI.Toolbar1.Items.Count = 0 Then
                Return result
            End If
            Select Case v_sFormName.ToUpper()
                'DC071100 added frmListClaim
                'MSB251001 start - Added case statement for the PolicySummaryUnderwriting form - copied it from the sirius for underwriting version
                Case "FRMPOLICYUNDERWRITING", "FRMPOLICYSUMMARYUNDERWRITING"
                    m_ofrmMDI.Toolbar1.Items.Item("Accounts").Visible = Not v_bFromEvent
                    'Start - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - 5.1.2.1
                    m_ofrmMDI.Toolbar1.Items.Item("InsuredAccounts").Visible = Not v_bFromEvent
                    'End - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - 5.1.2.1
                    m_ofrmMDI.Toolbar1.Items.Item("Sep1").Visible = Not v_bFromEvent
                    m_ofrmMDI.Toolbar1.Items.Item("Debit").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Credit").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Cash").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Sep2").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Policy").Visible = Not v_bFromEvent
                    m_ofrmMDI.Toolbar1.Items.Item("Sep3").Visible = Not v_bFromEvent
                    m_ofrmMDI.Toolbar1.Items.Item("Event").Visible = Not v_bFromEvent
                    m_ofrmMDI.Toolbar1.Items.Item("Sep4").Visible = Not v_bFromEvent
                    m_ofrmMDI.Toolbar1.Items.Item("Risk").Visible = Not v_bFromEvent
                    m_ofrmMDI.Toolbar1.Items.Item("Sep5").Visible = Not v_bFromEvent
                    m_ofrmMDI.Toolbar1.Items.Item("Text").Visible = Not v_bFromEvent
                    m_ofrmMDI.Toolbar1.Items.Item("Sep6").Visible = Not v_bFromEvent
                    'sj 04/10/2002 - start
                    If g_bHidePublicPrivateNotes Then
                        m_ofrmMDI.Toolbar1.Items.Item("Public").Visible = False
                        m_ofrmMDI.Toolbar1.Items.Item("Sep7").Visible = False
                    Else
                        'TF201103 - PN7730 - Reinstate Public Notes only
                        m_ofrmMDI.Toolbar1.Items.Item("Public").Visible = Not v_bFromEvent
                        m_ofrmMDI.Toolbar1.Items.Item("Sep7").Visible = Not v_bFromEvent
                    End If
                    'sj 04/10/2002 - end
                    m_ofrmMDI.Toolbar1.Items.Item("Letter").Visible = Not v_bFromEvent
                    m_ofrmMDI.Toolbar1.Items.Item("Email").Visible = False
                    'DC041204
                    m_ofrmMDI.Toolbar1.Items.Item("Sep11").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("iMarket").Visible = False

                    m_ofrmMDI.Toolbar1.Items.Item("Sep12").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("StickyNote").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("CashDeposit").Visible = False 'Sankar - (WPR85_Cash_Deposit_Process) - Paralleling

                    'MSB251001 - End
                Case "FRMLISTCLAIM", "FRMLISTCLAIM_SFB"
                    m_ofrmMDI.Toolbar1.Items.Item("Accounts").Visible = False
                    'Start - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - 5.1.2.1
                    m_ofrmMDI.Toolbar1.Items.Item("InsuredAccounts").Visible = False
                    'End - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - 5.1.2.1
                    m_ofrmMDI.Toolbar1.Items.Item("Sep1").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Debit").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Credit").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Cash").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Sep2").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Policy").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Sep3").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Sep3a").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Claim").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Sep3a").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Event").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Sep4").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Risk").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Sep5").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Text").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Sep6").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Public").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Sep7").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Letter").Visible = True
                    'eck131100
                    m_ofrmMDI.Toolbar1.Items.Item("Sep10").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("FinancePlan").Visible = False
                    'DC041204
                    m_ofrmMDI.Toolbar1.Items.Item("Sep11").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("iMarket").Visible = False

                    m_ofrmMDI.Toolbar1.Items.Item("Sep12").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("StickyNote").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("CashDeposit").Visible = False 'Sankar - (WPR85_Cash_Deposit_Process) - Paralleling

                Case "FRMLISTPOLICY", "FRMLISTPOLICYVERSION", "FRMMDI" ', "FRMLISTEVENTS"
                    m_ofrmMDI.Toolbar1.Items.Item("Accounts").Visible = False
                    'Start - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - 5.1.2.1
                    m_ofrmMDI.Toolbar1.Items.Item("InsuredAccounts").Visible = False
                    'End - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - 5.1.2.1
                    m_ofrmMDI.Toolbar1.Items.Item("Sep1").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Debit").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Credit").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Cash").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Sep2").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Policy").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Sep3").Visible = False
                    'DC 101100
                    m_ofrmMDI.Toolbar1.Items.Item("Claim").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Sep3a").Visible = False
                    'DC 101100
                    m_ofrmMDI.Toolbar1.Items.Item("Event").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Sep4").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Risk").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Sep5").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Text").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Sep6").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Public").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Sep7").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Letter").Visible = False
                    'eck200700
                    m_ofrmMDI.Toolbar1.Items.Item("Email").Visible = False

                    ' CTAF 280900
                    m_ofrmMDI.Toolbar1.Items.Item("Swift").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("MatchSwift").Visible = False
                    'eck131100
                    m_ofrmMDI.Toolbar1.Items.Item("Sep10").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("FinancePlan").Visible = False
                    'DC041204
                    m_ofrmMDI.Toolbar1.Items.Item("Sep11").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("iMarket").Visible = False

                    m_ofrmMDI.Toolbar1.Items.Item("Sep12").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("StickyNote").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("CashDeposit").Visible = False 'Sankar - (WPR85_Cash_Deposit_Process) - Paralleling

                Case "FRMLISTEVENTS" 'JSB 08/06/01 - moved events for list events to here as displays are different from above
                    m_ofrmMDI.Toolbar1.Items.Item("Accounts").Visible = False
                    'Start - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - 5.1.2.1
                    m_ofrmMDI.Toolbar1.Items.Item("InsuredAccounts").Visible = False
                    'End - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - 5.1.2.1
                    m_ofrmMDI.Toolbar1.Items.Item("Sep1").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Debit").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Credit").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Cash").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Sep2").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Policy").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Sep3").Visible = False
                    'DC 101100
                    m_ofrmMDI.Toolbar1.Items.Item("Claim").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Sep3a").Visible = False
                    'DC 101100
                    m_ofrmMDI.Toolbar1.Items.Item("Event").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Sep4").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Risk").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Sep5").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Text").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Sep6").Visible = False
                    'TF201103 - PN7730 - Reinstate Public Notes only
                    If g_bHidePublicPrivateNotes Then
                        m_ofrmMDI.Toolbar1.Items.Item("Public").Visible = False
                        m_ofrmMDI.Toolbar1.Items.Item("Sep7").Visible = False
                    Else
                        m_ofrmMDI.Toolbar1.Items.Item("Public").Visible = True
                        m_ofrmMDI.Toolbar1.Items.Item("Sep7").Visible = True
                    End If
                    m_ofrmMDI.Toolbar1.Items.Item("Letter").Visible = False
                    'eck200700
                    m_ofrmMDI.Toolbar1.Items.Item("Email").Visible = False

                    ' CTAF 280900
                    m_ofrmMDI.Toolbar1.Items.Item("Swift").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("MatchSwift").Visible = False
                    'eck131100
                    m_ofrmMDI.Toolbar1.Items.Item("Sep10").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("FinancePlan").Visible = False
                    'DC041204
                    m_ofrmMDI.Toolbar1.Items.Item("Sep11").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("iMarket").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("StickyNote").Visible = True
                    m_ofrmMDI.Toolbar1.Items.Item("CashDeposit").Visible = False 'Sankar - (WPR85_Cash_Deposit_Process) - Paralleling

                    'SD 10/07/02 - added frmPartySimmary to this case set
                Case "FRMPARTYCC", "FRMPARTYGC", "FRMPARTYPC", "FRMPARTYPCVIEW", "FRMPARTYCCVIEW", "FRMPARTYGCVIEW", "FRMPARTYSUMMARY"
                    'JSB 04/06/01 - added check for 3 view forms, as they it wasn't already here

                    m_ofrmMDI.Toolbar1.Items.Item("Accounts").Visible = True
                    'Start - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - 5.1.2.1
                    m_ofrmMDI.Toolbar1.Items.Item("InsuredAccounts").Visible = True
                    'End - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - 5.1.2.1
                    m_ofrmMDI.Toolbar1.Items.Item("Sep1").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Debit").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Credit").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Cash").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Sep2").Visible = True
                    m_ofrmMDI.Toolbar1.Items.Item("Policy").Visible = True
                    'DC 101100
                    m_ofrmMDI.Toolbar1.Items.Item("Claim").Visible = True
                    m_ofrmMDI.Toolbar1.Items.Item("Sep3a").Visible = True
                    'DC 101100
                    m_ofrmMDI.Toolbar1.Items.Item("Sep3").Visible = True
                    m_ofrmMDI.Toolbar1.Items.Item("Event").Visible = True
                    m_ofrmMDI.Toolbar1.Items.Item("Sep4").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Risk").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Sep5").Visible = True
                    m_ofrmMDI.Toolbar1.Items.Item("Text").Visible = True
                    m_ofrmMDI.Toolbar1.Items.Item("Sep6").Visible = True
                    'TF201103 - PN7730 - Reinstate Public Notes only
                    If g_bHidePublicPrivateNotes Then
                        m_ofrmMDI.Toolbar1.Items.Item("Public").Visible = False
                        m_ofrmMDI.Toolbar1.Items.Item("Sep7").Visible = False
                    Else
                        m_ofrmMDI.Toolbar1.Items.Item("Public").Visible = True
                        m_ofrmMDI.Toolbar1.Items.Item("Sep7").Visible = True
                    End If
                    m_ofrmMDI.Toolbar1.Items.Item("Letter").Visible = True
                    'eck200700
                    m_ofrmMDI.Toolbar1.Items.Item("Email").Visible = True

                    ' CTAF 280900 Hide or show the Swift Buttons
                    m_ofrmMDI.Toolbar1.Items.Item("Swift").Visible = g_bSwiftInstalled
                    m_ofrmMDI.Toolbar1.Items.Item("MatchSwift").Visible = g_bSwiftInstalled
                    'eck131100
                    m_ofrmMDI.Toolbar1.Items.Item("Sep10").Visible = True
                    m_ofrmMDI.Toolbar1.Items.Item("FinancePlan").Visible = True
                    'DC041204
                    m_ofrmMDI.Toolbar1.Items.Item("Sep11").Visible = True
                    m_ofrmMDI.Toolbar1.Items.Item("iMarket").Visible = True

                    m_ofrmMDI.Toolbar1.Items.Item("Sep12").Visible = True
                    m_ofrmMDI.Toolbar1.Items.Item("StickyNote").Visible = True
                    'Start - Sankar - (WPR85_Cash_Deposit_Process) - Paralleling
                    m_ofrmMDI.Toolbar1.Items.Item("CashDeposit").Visible = v_sFormName.ToUpper() = "FRMPARTYPCVIEW" Or v_sFormName.ToUpper() = "FRMPARTYCCVIEW" Or v_sFormName.ToUpper() = "FRMPARTYGCVIEW"
                    'End - Sankar - (WPR85_Cash_Deposit_Process) - Paralleling
                Case "FRMPOLICY", "FRMPOLICYSUMMARY" 'CT 11/09/00 added FRMPOLICYUNDERWRITING
                    m_ofrmMDI.Toolbar1.Items.Item("Accounts").Visible = Not v_bFromEvent
                    'Start - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - 5.1.2.1
                    m_ofrmMDI.Toolbar1.Items.Item("InsuredAccounts").Visible = Not v_bFromEvent
                    'End - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - 5.1.2.1
                    m_ofrmMDI.Toolbar1.Items.Item("Sep1").Visible = Not v_bFromEvent
                    m_ofrmMDI.Toolbar1.Items.Item("Debit").Visible = Not v_bFromEvent
                    m_ofrmMDI.Toolbar1.Items.Item("Credit").Visible = Not v_bFromEvent
                    m_ofrmMDI.Toolbar1.Items.Item("Cash").Visible = Not v_bFromEvent
                    m_ofrmMDI.Toolbar1.Items.Item("Sep2").Visible = Not v_bFromEvent
                    m_ofrmMDI.Toolbar1.Items.Item("Policy").Visible = Not v_bFromEvent
                    m_ofrmMDI.Toolbar1.Items.Item("Sep3").Visible = Not v_bFromEvent
                    'DC 101100
                    m_ofrmMDI.Toolbar1.Items.Item("Claim").Visible = Not v_bFromEvent
                    m_ofrmMDI.Toolbar1.Items.Item("Sep3a").Visible = Not v_bFromEvent
                    'DC 101100
                    m_ofrmMDI.Toolbar1.Items.Item("Event").Visible = Not v_bFromEvent
                    m_ofrmMDI.Toolbar1.Items.Item("Sep4").Visible = Not v_bFromEvent
                    m_ofrmMDI.Toolbar1.Items.Item("Risk").Visible = Not v_bFromEvent
                    m_ofrmMDI.Toolbar1.Items.Item("Sep5").Visible = Not v_bFromEvent
                    m_ofrmMDI.Toolbar1.Items.Item("Text").Visible = Not v_bFromEvent
                    m_ofrmMDI.Toolbar1.Items.Item("Sep6").Visible = Not v_bFromEvent
                    'TF201103 - PN7730 - Reinstate Public Notes only
                    If g_bHidePublicPrivateNotes Then
                        m_ofrmMDI.Toolbar1.Items.Item("Public").Visible = False
                        m_ofrmMDI.Toolbar1.Items.Item("Sep7").Visible = False
                    Else
                        m_ofrmMDI.Toolbar1.Items.Item("Public").Visible = Not v_bFromEvent
                        m_ofrmMDI.Toolbar1.Items.Item("Sep7").Visible = Not v_bFromEvent
                    End If
                    m_ofrmMDI.Toolbar1.Items.Item("Letter").Visible = Not v_bFromEvent
                    'eck200700
                    m_ofrmMDI.Toolbar1.Items.Item("Email").Visible = True

                    ' CTAF 280900 Hide or show the Swift Buttons
                    m_ofrmMDI.Toolbar1.Items.Item("Swift").Visible = g_bSwiftInstalled
                    m_ofrmMDI.Toolbar1.Items.Item("MatchSwift").Visible = g_bSwiftInstalled
                    'DC041204
                    m_ofrmMDI.Toolbar1.Items.Item("Sep11").Visible = True
                    m_ofrmMDI.Toolbar1.Items.Item("iMarket").Visible = True

                    m_ofrmMDI.Toolbar1.Items.Item("Sep12").Visible = True
                    m_ofrmMDI.Toolbar1.Items.Item("StickyNote").Visible = True
                    'Sankar - (WPR85_Cash_Deposit_Process) - Paralleling
                    m_ofrmMDI.Toolbar1.Items.Item("CashDeposit").Visible = False

                    ' PSA 22092000
                    'CT 13/09/00 added rovision for Underwriting Risk screen
                    'Case "FRMRISK", "FRMRISK2", "FRMRISK3", "FRMRISK4", "FRMRISK5", "FRMRISKN"
                    'Case "FRMRISK", "FRMRISK2", "FRMRISK3", "FRMRISK4", "FRMRISK5", "FRMRISKN", "FRMLISTRISKUNDERWRITING"
                    ' 25/04/2001 PSA - Start
                    'Case "FRMRISK", "FRMRISK2", "FRMRISK3", "FRMRISK4", "FRMRISK5", "FRMRISKN", "FRMLISTRISKUNDERWRITING", "FRMGIIHRISK", "FRMGIIMRISK"
                Case "FRMRISK", "FRMRISK2", "FRMRISK3", "FRMRISK4", "FRMRISK5", "FRMRISKN", "FRMGIIHRISK", "FRMGIIMRISK", "FRMGIITRISK"
                    ' 25/04/2001 PSA - End
                    ' PSA 22092000

                    m_ofrmMDI.Toolbar1.Items.Item("Accounts").Visible = True
                    'Start - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - 5.1.2.1
                    m_ofrmMDI.Toolbar1.Items.Item("InsuredAccounts").Visible = True
                    'End - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - 5.1.2.1
                    m_ofrmMDI.Toolbar1.Items.Item("Sep1").Visible = True
                    m_ofrmMDI.Toolbar1.Items.Item("Debit").Visible = True
                    m_ofrmMDI.Toolbar1.Items.Item("Credit").Visible = True
                    m_ofrmMDI.Toolbar1.Items.Item("Cash").Visible = True
                    m_ofrmMDI.Toolbar1.Items.Item("Sep2").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Policy").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Sep3").Visible = True
                    'DC 101100
                    m_ofrmMDI.Toolbar1.Items.Item("Claim").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Sep3a").Visible = False
                    'DC 101100
                    m_ofrmMDI.Toolbar1.Items.Item("Event").Visible = True
                    m_ofrmMDI.Toolbar1.Items.Item("Sep4").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Risk").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Sep5").Visible = True
                    m_ofrmMDI.Toolbar1.Items.Item("Text").Visible = True
                    m_ofrmMDI.Toolbar1.Items.Item("Sep6").Visible = True
                    'TF201103 - PN7730 - Reinstate Public Notes only
                    If g_bHidePublicPrivateNotes Then
                        m_ofrmMDI.Toolbar1.Items.Item("Public").Visible = False
                        m_ofrmMDI.Toolbar1.Items.Item("Sep7").Visible = False
                    Else
                        m_ofrmMDI.Toolbar1.Items.Item("Public").Visible = True
                        m_ofrmMDI.Toolbar1.Items.Item("Sep7").Visible = True
                    End If
                    m_ofrmMDI.Toolbar1.Items.Item("Letter").Visible = True
                    'eck200700
                    m_ofrmMDI.Toolbar1.Items.Item("Email").Visible = False

                    ' CTAF 280900
                    m_ofrmMDI.Toolbar1.Items.Item("Swift").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("MatchSwift").Visible = False
                    'DC041204
                    m_ofrmMDI.Toolbar1.Items.Item("Sep11").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("iMarket").Visible = False

                    m_ofrmMDI.Toolbar1.Items.Item("Sep12").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("StickyNote").Visible = False
                    'Sankar - (WPR85_Cash_Deposit_Process) - Paralleling
                    m_ofrmMDI.Toolbar1.Items.Item("CashDeposit").Visible = False

                Case "FRMLISTRISKUNDERWRITING"
                    m_ofrmMDI.Toolbar1.Items.Item("Accounts").Visible = True
                    'Start - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - 5.1.2.1
                    m_ofrmMDI.Toolbar1.Items.Item("InsuredAccounts").Visible = True
                    'End - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - 5.1.2.1
                    m_ofrmMDI.Toolbar1.Items.Item("Sep1").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Debit").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Credit").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Cash").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Sep2").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Policy").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Sep3").Visible = True
                    m_ofrmMDI.Toolbar1.Items.Item("Claim").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Sep3a").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Event").Visible = True
                    m_ofrmMDI.Toolbar1.Items.Item("Sep4").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Risk").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Sep5").Visible = True
                    m_ofrmMDI.Toolbar1.Items.Item("Text").Visible = True
                    m_ofrmMDI.Toolbar1.Items.Item("Sep6").Visible = True
                    If g_bHidePublicPrivateNotes Then
                        m_ofrmMDI.Toolbar1.Items.Item("Public").Visible = False
                        m_ofrmMDI.Toolbar1.Items.Item("Sep7").Visible = False
                    Else
                        m_ofrmMDI.Toolbar1.Items.Item("Public").Visible = True
                        m_ofrmMDI.Toolbar1.Items.Item("Sep7").Visible = True
                    End If
                    m_ofrmMDI.Toolbar1.Items.Item("Letter").Visible = True
                    m_ofrmMDI.Toolbar1.Items.Item("Email").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Swift").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("MatchSwift").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Sep11").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("iMarket").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Sep12").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("StickyNote").Visible = False
                    'Sankar - (WPR85_Cash_Deposit_Process) - Paralleling
                    m_ofrmMDI.Toolbar1.Items.Item("CashDeposit").Visible = False

                Case "FRMTEXTFILES"
                    m_ofrmMDI.Toolbar1.Items.Item("Accounts").Visible = True
                    'Start - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - 5.1.2.1
                    m_ofrmMDI.Toolbar1.Items.Item("InsuredAccounts").Visible = True
                    'End - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - 5.1.2.1
                    m_ofrmMDI.Toolbar1.Items.Item("Sep1").Visible = True
                    'SD 19/07/2002 Disable CR /DR buttons for underwriting

                    m_ofrmMDI.Toolbar1.Items.Item("Debit").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Credit").Visible = False

                    m_ofrmMDI.Toolbar1.Items.Item("Cash").Visible = True
                    m_ofrmMDI.Toolbar1.Items.Item("Sep2").Visible = True
                    m_ofrmMDI.Toolbar1.Items.Item("Policy").Visible = True
                    m_ofrmMDI.Toolbar1.Items.Item("Sep3").Visible = True
                    'DC 101100
                    m_ofrmMDI.Toolbar1.Items.Item("Claim").Visible = True
                    m_ofrmMDI.Toolbar1.Items.Item("Sep3a").Visible = True
                    'DC 101100
                    m_ofrmMDI.Toolbar1.Items.Item("Event").Visible = True
                    m_ofrmMDI.Toolbar1.Items.Item("Sep4").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Risk").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Sep5").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Text").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("Sep6").Visible = True
                    'TF201103 - PN7730 - Reinstate Public Notes only
                    If g_bHidePublicPrivateNotes Then
                        m_ofrmMDI.Toolbar1.Items.Item("Public").Visible = False
                        m_ofrmMDI.Toolbar1.Items.Item("Sep7").Visible = False
                    Else
                        m_ofrmMDI.Toolbar1.Items.Item("Public").Visible = True
                        m_ofrmMDI.Toolbar1.Items.Item("Sep7").Visible = True
                    End If
                    m_ofrmMDI.Toolbar1.Items.Item("Letter").Visible = True
                    'eck200700
                    m_ofrmMDI.Toolbar1.Items.Item("Email").Visible = False

                    ' CTAF 280900 Hide or show the Swift Buttons
                    m_ofrmMDI.Toolbar1.Items.Item("Swift").Visible = g_bSwiftInstalled
                    m_ofrmMDI.Toolbar1.Items.Item("MatchSwift").Visible = g_bSwiftInstalled
                    'DC041204
                    m_ofrmMDI.Toolbar1.Items.Item("Sep11").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("iMarket").Visible = False

                    m_ofrmMDI.Toolbar1.Items.Item("Sep12").Visible = False
                    m_ofrmMDI.Toolbar1.Items.Item("StickyNote").Visible = False
                    'Sankar - (WPR85_Cash_Deposit_Process) - Paralleling
                    m_ofrmMDI.Toolbar1.Items.Item("CashDeposit").Visible = False
                Case Else
                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

            'TF201103 - PN7730 - Reinstate Public Notes only
            m_ofrmMDI.Toolbar1.Items.Item("Private").Visible = False

            m_ofrmMDI.LiveForm = v_sFormName

            ' Alix - 20/01/2003 - PN9811
            ' Hide broking functionnality

            m_ofrmMDI.Toolbar1.Items.Item("iMarket").Visible = False
            'm_ofrmMDI.Toolbar1.Buttons("StickyNote").Visible = False


            ' /Alix

            If g_iCountryID <> 1 Then
                m_ofrmMDI.Toolbar1.Items.Item("iMarket").Visible = False
            End If



            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetToolbar Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetToolbar", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function SetRestrictedToolbar(ByVal v_sFormName As String, Optional ByVal v_bFromEvent As Boolean = False) As Integer

        ' CTAF 270900
        Dim result As Integer = 0
        Dim bInstalled As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            If m_ofrmMDI.Toolbar1.Items.Count = 0 Then
                Return result
            End If

            Dim iCount As Integer

            iCount = m_ofrmMDI.Toolbar1.Items.Count

            For i As Integer = 1 To iCount
                m_ofrmMDI.Toolbar1.Items.Item(i - 1).Visible = False
            Next i


            Select Case v_sFormName.ToUpper()
                Case "FRMLISTPOLICY", "FRMLISTPOLICYVERSION", "FRMMDI" ', "FRMLISTEVENTS"


                Case "FRMLISTEVENTS"


                Case "FRMPARTYCC", "FRMPARTYGC", "FRMPARTYPC", "FRMPARTYPCVIEW", "FRMPARTYCCVIEW", "FRMPARTYGCVIEW" 'JSB 04/06/01 - added check for 3 view forms, as they it wasn't already here


                    m_ofrmMDI.Toolbar1.Items.Item("Policy").Visible = True
                    m_ofrmMDI.Toolbar1.Items.Item("Sep3").Visible = True
                    m_ofrmMDI.Toolbar1.Items.Item("Event").Visible = True

                Case "FRMPOLICY", "FRMPOLICYSUMMARY" 'CT 11/09/00 added FRMPOLICYUNDERWRITING

                    m_ofrmMDI.Toolbar1.Items.Item("Policy").Visible = Not v_bFromEvent
                    m_ofrmMDI.Toolbar1.Items.Item("Sep3").Visible = Not v_bFromEvent
                    m_ofrmMDI.Toolbar1.Items.Item("Sep3a").Visible = Not v_bFromEvent
                    m_ofrmMDI.Toolbar1.Items.Item("Event").Visible = Not v_bFromEvent
                    m_ofrmMDI.Toolbar1.Items.Item("Sep4").Visible = Not v_bFromEvent
                    m_ofrmMDI.Toolbar1.Items.Item("Risk").Visible = Not v_bFromEvent

                Case "FRMRISK", "FRMRISK2", "FRMRISK3", "FRMRISK4", "FRMRISK5", "FRMRISKN", "FRMLISTRISKUNDERWRITING", "FRMGIIHRISK", "FRMGIIMRISK", "FRMGIITRISK"

                    m_ofrmMDI.Toolbar1.Items.Item("Event").Visible = True


                Case Else
                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

            m_ofrmMDI.LiveForm = v_sFormName

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetRestrictedToolbar Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetRestrictedToolbar", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: LockThePolicy
    '
    ' Description: We're entering the risk screen from summary, so let's lock the policy.
    '
    ' ***************************************************************** '
    Public Function LockThePolicy(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Find the policy.
            For iLoop1 As Integer = 0 To Application.OpenForms.Count - 1
                If Application.OpenForms.Item(iLoop1).Name = "frmPolicySummary" Then

                    'Is it this version?
                    '            If ((Forms(iLoop1%).InsFileCnt = v_lInsuranceFileCnt) _
                    'And (Forms(iLoop1%).FromEvent = False)) Then

                    If ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "InsuranceFileCnt") = v_lInsuranceFileCnt Then
                        ' Yes it is

                        'NIIT - Replaced with the Migrated code 1144 
                        'm_lReturn = Application.OpenForms.Item(iLoop1).LockThePolicy
                        m_lReturn = ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "LockThePolicy")


                        Return m_lReturn
                    End If
                End If
            Next iLoop1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LockThePolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LockThePolicy", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UnlockThePolicy
    '
    ' Description: We're closing the risk screen, so let's unlock the policy.
    '
    ' ***************************************************************** '
    Public Function UnlockThePolicy(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Find the policy.
            For iLoop1 As Integer = 0 To Application.OpenForms.Count - 1
                If Application.OpenForms.Item(iLoop1).Name = "frmPolicySummary" Then

                    'Is it this version?
                    '            If ((Forms(iLoop1%).InsuranceFileCnt = v_lInsuranceFileCnt) _
                    'And (Forms(iLoop1%).FromEvent = False)) Then

                    'NIIT - Replaced with the Migrated code 1144 
                    'If Application.OpenForms.Item(iLoop1).InsuranceFileCnt = v_lInsuranceFileCnt Then
                    If ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "InsuranceFileCnt") = v_lInsuranceFileCnt Then
                        ' Yes it is

                        'NIIT - Replaced with the Migrated code 1144 
                        'm_lReturn = Application.OpenForms.Item(iLoop1).UnlockThePolicy
                        m_lReturn = ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "UnlockThePolicy")
                        Return result
                    End If
                End If
            Next iLoop1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnlockThePolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockThePolicy", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: OpenClient
    '
    ' Description: Calls Client Manager Wrapper to open up find and client manager
    '
    ' History: 18/02/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function OpenClient() As Integer
        Dim result As Integer = 0
        Dim iPMBClientManagerWrapper As Object


        Dim oCMWrapper As iPMBClientManagerWrapper.Interface_Renamed

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the wrapper
            Dim temp_oCMWrapper As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oCMWrapper, sClassName:="iPMBClientManagerWrapper.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oCMWrapper = temp_oCMWrapper
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of iPMBClientManagerWrapper.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="OpenClient", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Start it

            m_lReturn = oCMWrapper.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start oCMWrapper", vApp:=ACApp, vClass:=ACClass, vMethod:="OpenClient", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                oCMWrapper = Nothing
                Return result
            End If

            ' Terminate it

            oCMWrapper.Dispose()

            oCMWrapper = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="OpenClient Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="OpenClient", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ShowSBOAbout
    '
    ' Description:
    '
    ' History: 02/03/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function ShowSBOAbout() As Integer
        Dim result As Integer = 0
        Dim iPMAbout As Object


        Dim oAbout As iPMAbout.Interface_Renamed
        Dim sVersion, sRelease, sSiriusType, sInstallDate As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = gPMFunctions.GetSiriusVersion(sVersion, sRelease, sSiriusType, sInstallDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Dim temp_oAbout As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oAbout, sClassName:="iPMAbout.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oAbout = temp_oAbout
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oAbout.Show(sTitle:="Sirius Back-Office", sVersionNumber:="Sirius for " & sSiriusType & _
                        " v" & sVersion & " sr" & sRelease, sVersionDate:=sInstallDate, sSupportEmail:="support@siriusgroup.co.uk", sComponent:="Client Manager")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            oAbout.Dispose()

            oAbout = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowSBOAbout Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowSBOAbout", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ShowBrokerlinkRisk
    '
    ' Description:
    '
    ' History: 14/09/2006 MKW - Created.
    '
    ' ***************************************************************** '
    Public Function ShowBrokerlinkRisk(ByVal v_sGisDataModelCode As String, ByVal v_lInsuranceFileCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim SYS4BCOM, bGIS As Object

        Dim oSys4bcom As Object
        Dim sSubKey, sValue As String

        Dim oGis As bGIS.Application
        Dim sXML, sXMLDef As String
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oGis As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oGis, "bGIS.Application", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oGis = temp_oGis

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get bGIS", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowBrokerlinkRisk", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If


            m_lReturn = oGis.LoadFromDB(r_sXMLDataSetDef:=sXMLDef, r_sXMLDataset:=sXML, v_sGisDataModelCode:=v_sGisDataModelCode, r_vInsuranceFileCnt:=v_lInsuranceFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load risk", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowBrokerlinkRisk", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            If Not (oGis Is Nothing) Then

                oGis.Dispose()
                oGis = Nothing
            End If

            'oSys4bcom = New SYS4BCOM.S4BCOM() 'deepak commented as currently working on PC flow only


            oSys4bcom.P_S4B = True '&& Signal that we are being called from S4B

            oSys4bcom.P_USERNAME = "Sirius"

            sSubKey = "GIS\" & v_sGisDataModelCode.Trim()

            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="P_APPDIR", r_sSettingValue:=sValue, v_sSubKey:=sSubKey)
            If sValue = "" Then

                oSys4bcom.P_APPDIR = "d:\dbase\mpac"
            Else

                oSys4bcom.P_APPDIR = sValue
            End If

            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="P_DATABASE_ID", r_sSettingValue:=sValue, v_sSubKey:=sSubKey)
            If sValue = "" Then

                oSys4bcom.P_DATABASE_ID = "2"
            Else

                oSys4bcom.P_DATABASE_ID = sValue
            End If


            If Not oSys4bcom.P_LOGGED_ON Then

                oSys4bcom.m_logon()
            End If


            oSys4bcom.p_xml_DataSetDef = sXMLDef

            oSys4bcom.p_xml = sXML


            'Set whether creating new policy

            oSys4bcom.p_new_policy = False


            'Set Transaction type on brokerlink

            oSys4bcom.p_policy_transaction_type = "NIL"


            'Call update

            m_lReturn = oSys4bcom.m_policy_update()


            If Not (oSys4bcom Is Nothing) Then

                If oSys4bcom.P_LOGGED_ON Then

                    oSys4bcom.m_logoff()
                End If
                oSys4bcom = Nothing
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowBrokerlinkRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowBrokerlinkRisk", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: ShowBrokerlinkRisk
    '
    ' Description:
    '
    ' History: 14/09/2006 MKW - Created.
    '
    ' ***************************************************************** '
    Public Function CheckIfBrokerlinkRisk(ByVal v_lInsuranceFileCnt As Integer, ByRef r_bBrokerlink As Boolean, Optional ByRef r_sDmCode As String = "", Optional ByRef r_sQemCode As String = "") As Integer
        Dim result As Integer = 0
        Dim bGIS As Object


        Dim oGis As bGIS.Application
        Dim sQemCode, sDmCode As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oGis As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oGis, "bGIS.Application", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oGis = temp_oGis

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get bGIS", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowBrokerlinkRisk", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If


            m_lReturn = oGis.GetQemDmCode(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_sQemCode:=sQemCode, r_sDmCode:=sDmCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load risk", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowBrokerlinkRisk", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            If Not (oGis Is Nothing) Then

                oGis.Dispose()
                oGis = Nothing
            End If

            If sQemCode = "BROKERLINK" Then
                r_bBrokerlink = True
            End If

            r_sDmCode = sDmCode
            r_sQemCode = sQemCode

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckIfBrokerlinkRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckIfBrokerlinkRisk", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    ' Name: StartNavMap
    '
    ' Description: Starts the navigator process
    '
    ' ***************************************************************** '
    Private Function StartNavMap(ByVal v_sProcessCode As String, Optional ByVal v_vKeyArray(,) As Object = Nothing, Optional ByVal v_bCallNavXM As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim oNavStart As iPMNavStart.Interface_Renamed



        result = gPMConstants.PMEReturnCode.PMTrue

        If v_bCallNavXM Then 'PN26814
            'For NavXM processes, use the reference handled by the Interface class
            oNavStart = PMNavStart
        Else
            ' Create a new instance of navigator
            oNavStart = New iPMNavStart.Interface_Renamed()
        End If

        m_lReturn = CType(oNavStart, SSP.S4I.Interfaces.ILocalInterface).Initialise()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise iPMNavStart object.", vApp:=ACApp, vClass:=ACClass, vMethod:="StartNavMap", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Return result
        End If

        ' Set the navigator keys

        If Not Information.IsNothing(v_vKeyArray) Then
            m_lReturn = oNavStart.SetKeys(vKeyArray:=v_vKeyArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set keys.", vApp:=ACApp, vClass:=ACClass, vMethod:="StartNavMap", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If
        End If

        ' Set its properties
        oNavStart.CallingAppName = ACApp

        If v_bCallNavXM Then
            'Set the NavXMLFile to call NavXM process
            oNavStart.NavXMLFile = v_sProcessCode
        Else
            ' Set the process to start
            oNavStart.ProcessCode = v_sProcessCode
        End If

        If v_bCallNavXM Then 'PN26814
            'Flag that NavXM process is running
            'This is reset to False when the NavigatorClose event is handled
            'in the Interface class
            IsNavStartRunning = True
        End If
        ' Start it
        m_lReturn = oNavStart.Start()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start oNavStart", vApp:=ACApp, vClass:=ACClass, vMethod:="StartNavMap", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Return result
        End If

        Return result

    End Function

    '---------------------------------------------------------------------------------------
    ' Procedure   : DetectSQ
    ' DateTime    : 13/01/2006
    ' Author      : JDurnall
    ' Description : Detect the presence of SiriusQuotes (determined by a server-side
    '               registry setting).  Sirius Quotes uses NavXM navigator
    ' Parameters  : r_bSQEnabled - Returns True if SQ enabled, else False
    ' Returns     : Long - PMEReturnCode
    '---------------------------------------------------------------------------------------
    Private Function DetectSQ(ByRef r_bSQEnabled As Boolean) As Integer
        Dim result As Integer = 0
        Dim bGIIRegistry As Object
        Const kFUNCTION_NAME As String = "DetectSQ"
        Dim sRegVal As String = ""

        'Dim oGIIRegistry As bGIIRegistry.Business 
        Dim oGIIRegistry As Object

        result = gPMConstants.PMEReturnCode.PMTrue
        r_bSQEnabled = False

        'Detect SQ from Server registry setting
        Dim temp_oGIIRegistry As Object
        result = g_oObjectManager.GetInstance(temp_oGIIRegistry, "bGIIRegistry.Business", gPMConstants.PMGetViaClientManager)
        oGIIRegistry = temp_oGIIRegistry
        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("ObjectManager.GetInstance of bGIIRegistry.Business", "GetInstance failed, returning a result of " & result, gPMConstants.PMELogLevel.PMLogError)
        End If

        result = oGIIRegistry.ReadServerRegString(v_lSection:=gPMConstants.HKEY_LOCAL_MACHINE, v_sKey:="Software\PM\Aquarius", v_sName:="G2V2", r_sValue:=sRegVal)
        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("bGIIRegistry.ReadServerRegString", "ReadServerRegString failed to read " & _
                                    "HKLM\Software\PM\Aquarius\G2V2 setting, returning a result of " & CStr(result), gPMConstants.PMELogLevel.PMLogError)
        End If

        Select Case sRegVal.ToUpper()
            Case "YES"
                r_bSQEnabled = True
            Case Else
                r_bSQEnabled = False
        End Select


        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: ShowDocumaster
    '
    ' Description: Receives link code consisting of client code or policy
    '              description with client (1) or policy (2) level as right
    '              most character. An instance of Documaster is created or
    '              if one already exists this is used to display either the
    '              client or policy documents.
    '
    ' History: 17/10/00 ND - Created.
    '
    ' ***************************************************************** '


    Function ShowDocumaster(ByRef v_sLinkCode As String) As Integer

        Dim result As Integer = 0
        Dim iDocManager As Object
        Dim lWinHand As Integer
        Dim sLinkCodeAndLevel As String = ""
        Dim iInstances As Integer = 0


        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            Dim iSessionID As Integer = Process.GetCurrentProcess.SessionId
            Dim Procesos() As Process = Process.GetProcessesByName("iDOCManager")
            If Procesos.Length > 0 Then
                For index As Integer = 0 To Procesos.Length - 1
                    If Procesos(index).SessionId = iSessionID Then
                        iInstances += 1
                    End If
                Next
                If iInstances > 0 Then
                    MessageBox.Show("Instance of Documaster that is already open", "DocuMaster EnterPrise", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Function
                End If

            End If

            ' Check link level is correct i.e. 1 = client (drawer) level,
            '                                  2 = policy (folder) level
            If (Not v_sLinkCode.EndsWith("1")) And (Not v_sLinkCode.EndsWith("2")) Then
                ' error
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Link Code must be 1(client) or 2(policy)", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowDocumaster", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            ' and link code not blank
            If v_sLinkCode = "" Then
                ' error
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Link Code incorrectly set to blank", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowDocumaster", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create Link Code and Level to pass to documaster
            sLinkCodeAndLevel = "SBO" & v_sLinkCode
            'sLinkCodeAndLevel = "SBO" & Left$(v_sLinkCode, Len(v_sLinkCode) - 1)


            ' See if Documaster is already running
            'SP040898 - see above
            lWinHand = GIIConstants.FindWindow(0, "DocuMaster Enterprise  ")

            If lWinHand <> 0 Then

                'DocuMaster is already running
                'iDocManager = System.Runtime.InteropServices.Marshal.GetActiveObject("iDOCManager.Interface")
                iDocManager = CreateLateBoundObject("iDOCManager.Interface_Renamed")


                'Show the interface
                m_lReturn = iDocManager.Activate(sLinkCodeAndLevel)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'error
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed To Active DocManager", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowDocumaster", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    result = gPMConstants.PMEReturnCode.PMFalse
                    iDocManager.Dispose()
                    iDocManager = Nothing
                    Return result

                End If

            Else

                'DocuMaster is not already running
                iDocManager = CreateLateBoundObject("iDOCManager.Interface_Renamed")

                'initialise the main interface
                m_lReturn = iDocManager.Initialise()


                Select Case m_lReturn
                    Case gPMConstants.PMEReturnCode.PMTrue

                    Case gPMConstants.PMEReturnCode.PMCancel
                        iDocManager.Dispose()
                        iDocManager = Nothing
                        Return result
                    Case Else
                        'error
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed To Initilise DocManager", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowDocumaster", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                        result = gPMConstants.PMEReturnCode.PMFalse
                        iDocManager.Dispose()
                        iDocManager = Nothing
                        Return result
                End Select

                'Start the interface
                m_lReturn = iDocManager.Start(sLinkCodeAndLevel)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'error
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed To Start DocManager", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowDocumaster", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    result = gPMConstants.PMEReturnCode.PMFalse
                    iDocManager.Dispose()
                    iDocManager = Nothing
                    Return result
                End If

            End If

            'Finished now
            iDocManager = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowDocumaster Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowDocumaster", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ProcessFinancePlanFunction
    '
    ' Description: Handler for iPMBFindFinancePlan.Interface
    '
    ' History: 13/11/2000 ECK - Created.
    '
    ' ***************************************************************** '
    Public Function ProcessFinancePlanFunction(ByVal v_lPartyCnt As Integer, ByVal v_sShortName As String, ByVal v_sLongname As String) As Integer
        Dim result As Integer = 0
        Dim iPMBFinancePlanMaint, iPMBFindFinancePlan As Object


        Dim oFindFinancePlan As iPMBFindFinancePlan.Interface_Renamed


        Dim oFinancePlanMaint As iPMBFinancePlanMaint.Interface_Renamed
        Dim vKeyArray(,) As Object
        Dim lStatus As gPMConstants.PMEReturnCode
        Dim lFinanceCnt, lFinanceVersion As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance
            Dim temp_oFindFinancePlan As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oFindFinancePlan, sClassName:="iPMBFindFinancePlan.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFindFinancePlan = temp_oFindFinancePlan
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of iPMBFindFinancePlan.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessFinancePlanFunction", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            oFindFinancePlan.CallingAppName = ACApp


            ReDim vKeyArray(1, 2)

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNamePartyCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = v_lPartyCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameClientCode

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = v_sShortName

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNameClientName

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = v_sLongname


            m_lReturn = oFindFinancePlan.SetKeys(vKeyArray)

            'SD 01/08/2002 Scalability changes

            m_lReturn = oFindFinancePlan.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vEffectiveDate:=DateTime.Now)



            m_lReturn = oFindFinancePlan.Start()


            lStatus = oFindFinancePlan.Status

            If lStatus = gPMConstants.PMEReturnCode.PMCancel Then

                oFindFinancePlan.Dispose()
                oFindFinancePlan = Nothing
                Return result
            End If


            m_lReturn = oFindFinancePlan.GetKeys(vKeyArray)
            oFindFinancePlan = Nothing

            lFinanceCnt = CInt(vKeyArray(1, 3))

            lFinanceVersion = CInt(vKeyArray(1, 4))
            Select Case lFinanceCnt
                Case 0
                    m_lReturn = ProcessNewFinancePlan(v_lPartyCnt:=v_lPartyCnt, v_sShortName:=v_sShortName)
                Case Else
                    ' Get an instance
                    Dim temp_oFinancePlanMaint As Object
                    m_lReturn = g_oObjectManager.GetInstance(temp_oFinancePlanMaint, sClassName:="iPMBFinancePlanMaint.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                    oFinancePlanMaint = temp_oFinancePlanMaint
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        ' Log Error Message
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of iPMBFinancePlanMaint.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessFinancePlanFunction", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return result
                    End If

                    oFinancePlanMaint.CallingAppName = ACApp
                    ReDim vKeyArray(1, 2)

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameFinancePlanCnt

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = lFinanceCnt

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameFinancePlanVersion

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = lFinanceVersion

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNameFinancePlanTransactions

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = ""


                    m_lReturn = oFinancePlanMaint.SetKeys(vKeyArray)

                    'SD 01/08/2002 Scalability changes

                    m_lReturn = oFinancePlanMaint.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vEffectiveDate:=DateTime.Now)



                    m_lReturn = oFinancePlanMaint.Start()


                    lStatus = oFinancePlanMaint.Status

                    If lStatus = gPMConstants.PMEReturnCode.PMCancel Then

                        oFinancePlanMaint.Dispose()
                        oFinancePlanMaint = Nothing
                        Return result
                    End If


                    m_lReturn = oFinancePlanMaint.GetKeys(vKeyArray)
                    oFinancePlanMaint = Nothing

            End Select

            m_lReturn = UpdatePartyScreen()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdatePartyScreen Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessFinancePlanFunction", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessFinancePlanFunction Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessFinancePlanFunction", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'eck131100
    ' ***************************************************************** '
    '
    ' Name: ProcessNewFinancePlan
    '
    ' Description: Handler for New Finance Plan
    '
    ' History: 13/11/2000 ECK - Created.
    '
    ' ***************************************************************** '
    Public Function ProcessNewFinancePlan(ByVal v_lPartyCnt As Integer, ByVal v_sShortName As String) As Integer
        Dim result As Integer = 0
        Dim iPMBFinancePlanMaint, iPMBFinancePlanQuote, iPMBFinanceTransactions As Object



        Dim oFinanceTransactions As iPMBFinanceTransactions.Interface_Renamed



        Dim oFinancePlanQuote As iPMBFinancePlanQuote.Interface_Renamed



        Dim oFinancePlanMaint As iPMBFinancePlanMaint.Interface_Renamed

        'Developer Guide No. 71(Guide)
        Dim vKeyArray(,) As Object
        Dim lStatus As gPMConstants.PMEReturnCode
        Dim lFinanceCnt, lFinanceVersion As Integer
        Dim vFinanceTransactions As String = ""
        Dim lInsuranceFileCnt As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get Transactions

            Dim temp_oFinanceTransactions As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oFinanceTransactions, sClassName:="iPMBFinanceTransactions.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFinanceTransactions = temp_oFinanceTransactions
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of iPMBFinanceTransactions.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessNewFinancePlan", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            oFinanceTransactions.CallingAppName = ACApp


            ReDim vKeyArray(1, 1)

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNamePartyCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = v_lPartyCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameClientCode

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = v_sShortName


            m_lReturn = oFinanceTransactions.SetKeys(vKeyArray)

            'SD 01/08/2002 Scalability changes

            m_lReturn = oFinanceTransactions.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vTypeOfBusiness:=gPMConstants.PMTypeOfBusinessNB, vEffectiveDate:=DateTime.Now)


            m_lReturn = oFinanceTransactions.Start()


            lStatus = oFinanceTransactions.Status


            m_lReturn = oFinanceTransactions.GetKeys(vKeyArray)

            oFinanceTransactions.Dispose()
            oFinanceTransactions = Nothing

            If lStatus = gPMConstants.PMEReturnCode.PMCancel Then
                Return result
            End If



            vFinanceTransactions = CStr(vKeyArray(1, 0))
            If Not Information.IsArray(vFinanceTransactions) Then
                Return result
            End If

            'PN4877 - TR - Get the InsuranceFileCnt from find screen and pass to quote

            lInsuranceFileCnt = gPMFunctions.NullToLong(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1)))

            'Do Quote
            Dim temp_oFinancePlanQuote As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oFinancePlanQuote, sClassName:="iPMBFinancePlanQuote.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFinancePlanQuote = temp_oFinancePlanQuote
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of iPMBFinancePlanQuote.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessNewFinancePlan", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            oFinancePlanQuote.CallingAppName = ACApp
            ReDim vKeyArray(1, 3)

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNamePartyCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = v_lPartyCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameClientCode

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = v_sShortName

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNameFinancePlanTransactions

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = vFinanceTransactions
            'PN4877 - TR - Get the InsuranceFileCnt from find screen and pass to quote

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.PMKeyNameInsuranceFileCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = lInsuranceFileCnt


            m_lReturn = oFinancePlanQuote.SetKeys(vKeyArray)

            'SD 01/08/2002 Scalability changes

            m_lReturn = oFinancePlanQuote.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vEffectiveDate:=DateTime.Now)



            m_lReturn = oFinancePlanQuote.Start()


            lStatus = oFinancePlanQuote.Status


            m_lReturn = oFinancePlanQuote.GetKeys(vKeyArray)


            oFinancePlanQuote.Dispose()
            oFinancePlanQuote = Nothing

            If lStatus = gPMConstants.PMEReturnCode.PMCancel Then
                Return result
            End If

            oFinancePlanQuote = Nothing
            'PN4877 - TR - Change the Order in the Array

            lFinanceCnt = gPMFunctions.NullToLong(CStr(CInt(vKeyArray(1, 3))))

            lFinanceVersion = gPMFunctions.NullToLong(CStr(CInt(vKeyArray(1, 4))))

            'Save Quote

            Dim temp_oFinancePlanMaint As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oFinancePlanMaint, sClassName:="iPMBFinancePlanMaint.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFinancePlanMaint = temp_oFinancePlanMaint
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of iPMBFinancePlanMaint.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessNewFinancePlan", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            oFinancePlanMaint.CallingAppName = ACApp
            'eck310102
            '   ReDim vKeyArray(1, 2)
            ReDim vKeyArray(1, 3)


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameFinancePlanCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = lFinanceCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameFinancePlanVersion

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = lFinanceVersion

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNameFinancePlanTransactions

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = ""
            'eck310102

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.PMKeyNameTaskGroupCode

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = "New Business"
            '


            m_lReturn = oFinancePlanMaint.SetKeys(vKeyArray)

            'SD 01/08/2002 Scalability changes

            m_lReturn = oFinancePlanMaint.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vEffectiveDate:=DateTime.Now)



            m_lReturn = oFinancePlanMaint.Start()


            lStatus = oFinancePlanMaint.Status


            m_lReturn = oFinancePlanMaint.GetKeys(vKeyArray)

            oFinancePlanMaint = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessNewFinancePlan Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessNewFinancePlan", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ShowDocumasterDocumentDocument
    '
    ' Description: Receives document number, creates an instance of
    '              documaster and then shows document level in tree and
    '              then opens the document to view
    '
    ' History: 19/10/00 ND - Created.
    ' ECK 150201 Added New Parameter
    ' ***************************************************************** '


    Function ShowDocumasterDocument(ByRef v_lDocNum As Integer, ByRef sCommand As String) As Integer

        Dim result As Integer = 0
        Dim iDocManager As Object
        Dim lWinHand As Integer
        Dim sLinkCodeAndLevel As String = ""
        Dim vTaskInstKeyArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' check link code is not zero
            If v_lDocNum = 0 Then
                ' error
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Link Code incorrectly set to blank", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowDocumasterDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create Navigator keys to pass to Documaster
            ReDim vTaskInstKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0)

            vTaskInstKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameTaskDescription

            vTaskInstKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = v_lDocNum


            ' See if Documaster is already running
            'SP040898 - see above
            'lWinHand = GIIConstants.FindWindow(0, "DocuMaster Enterprise  ")

            'If lWinHand <> 0 Then

            '    'DocuMaster is already running
            '    iDocManager = System.Runtime.InteropServices.Marshal.GetActiveObject("iDOCManager.Interface")

            '    'Set nav keys so documaster will load document
            '    m_lReturn = iDocManager.SetKeys(vTaskInstKeyArray)

            '    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '        'error
            '        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed To Set Navigator Keys", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowDocumasterDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            '        result = gPMConstants.PMEReturnCode.PMFalse
            '        m_lReturn = iDocManager.Terminate()
            '        iDocManager = Nothing
            '        Return result

            '    End If

            '    'Show the interface
            '    'eck150201 pass new parameter

            '    m_lReturn = iDocManager.Activate(sCommand)

            '    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '        'error
            '        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed To Active DocManager", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowDocumasterDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            '        result = gPMConstants.PMEReturnCode.PMFalse
            '        m_lReturn = iDocManager.Terminate()
            '        iDocManager = Nothing
            '        Return result

            '    End If

            'Else

            'DocuMaster is not already running
            iDocManager = CreateLateBoundObject("iDOCManager.Interface_Renamed")

            'initialise the main interface
            ' m_lReturn = CType(iDocManager, SSP.S4I.Interfaces.ILocalInterface).Initialise()
            m_lReturn = iDocManager.Initialise()


            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMTrue

                Case gPMConstants.PMEReturnCode.PMCancel
                    iDocManager.Dispose()
                    iDocManager = Nothing
                    Return result
                Case Else
                    'error
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed To Initilise DocManager", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowDocumasterDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    result = gPMConstants.PMEReturnCode.PMFalse
                    iDocManager.Dispose()
                    iDocManager = Nothing
                    Return result
            End Select

            'Set nav keys so documaster will load document
            m_lReturn = iDocManager.SetKeys(vTaskInstKeyArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'error
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed To Set Navigator Keys", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowDocumasterDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                result = gPMConstants.PMEReturnCode.PMFalse
                iDocManager.Dispose()
                iDocManager = Nothing
                Return result

            End If

            'Start the interface
            If (sCommand Is Nothing Or sCommand = "") Then
                m_lReturn = iDocManager.Start()
            Else
                m_lReturn = iDocManager.Start(sCommand)
            End If


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'error
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed To Start DocManager", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowDocumasterDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                result = gPMConstants.PMEReturnCode.PMFalse
                iDocManager.Dispose()
                iDocManager = Nothing
                Return result
            End If

            'End If

            'Finished now
            iDocManager.Dispose()
            iDocManager = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowDocumasterDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowDocumasterDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: LoadRecentFiles
    '
    ' Description: Written to use proper use of the registry
    '
    ' History: 26/06/2001 CTAF - Created.
    '          12/08/2002 CTAF - Removed excess debug messages
    '
    ' ***************************************************************** '
    Public Function LoadRecentFiles(ByRef r_oForm As Form) As Integer

        Dim result As Integer = 0
        Dim sKey, sShortName As String
        Dim iTemp As Integer
        Dim sFile As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            For lLoop As Integer = 1 To 5
                If m_ofrmMDI.mnuRecentFile(lLoop).Available Then
                    ReflectionHelper.SetMember(ReflectionHelper.GetMember(r_oForm, "mnuRecentFile", New Object() {lLoop}), "Text", m_ofrmMDI.mnuRecentFile(lLoop).Text)
                    ReflectionHelper.SetMember(ReflectionHelper.GetMember(r_oForm, "mnuRecentFile", New Object() {lLoop}), "Tag", Convert.ToString(m_ofrmMDI.mnuRecentFile(lLoop).Tag))
                    ReflectionHelper.SetMember(ReflectionHelper.GetMember(r_oForm, "mnuRecentFile", New Object() {lLoop}), "Visible", True)
                Else
                    ReflectionHelper.SetMember(ReflectionHelper.GetMember(r_oForm, "mnuRecentFile", New Object() {lLoop}), "Visible", False)
                End If
            Next
            Return result

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & "LoadRecentFiles")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "LoadRecentFiles Failed", ACApp, ACClass, "LoadRecentFiles", Information.Err().Number, excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    '
    ' Name: SaveRecentFiles
    '
    ' Description:
    '
    ' Recent File Functions : LoadRecentFiles
    '                         SaveRecentFiles
    '                         AddRecentFile
    '
    ' These can be taken wholesale and used in other programs
    ' (like ClientManager)
    '
    ' History: 26/06/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function SaveRecentFiles(ByRef r_oForm As frmMDI) As Integer

        Dim result As Integer = 0
        Dim sKey, sValue As String

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".SaveRecentFiles")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For iLoop1 As Integer = 1 To 5

                sKey = "RecentFile" & iLoop1

                If r_oForm.mnuRecentFile(iLoop1).Available Then
                    sValue = Convert.ToString(r_oForm.mnuRecentFile(iLoop1).Tag)

                    ' If we have 2 "&" (for display purposes) then replace with 1   PN17033
                    sValue = sValue.Replace("&&", "&")
                Else
                    sValue = ""
                End If

                m_lReturn = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=sKey, v_sSettingValue:=sValue, v_sSubKey:="RecentFiles")
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Next iLoop1

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".SaveRecentFiles")

            Return result

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".SaveRecentFiles")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveRecentFiles Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveRecentFiles", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: AddRecentFile
    '
    ' Description: Adds a file to the recent file menu
    '
    ' History: 26/06/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function AddRecentFile(ByRef r_oForm As frmMDI, ByVal v_vFileName As String) As Integer

        Dim result As Integer = 0
        Dim sShortName As String = ""
        Dim iTemp, iFound As Integer
        Dim lNewPartyCnt, lOldPartyCnt As Integer
        Dim sTemp As String = ""

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".AddRecentFile")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            iFound = 0

            ' Check its not already in the list
            'MKR 29/09/2004 PN 6021 'We are now using '|' as a delimiter rather than ','
            sTemp = Mid(v_vFileName, (IIf(v_vFileName = "" And "|" = "", 0, (v_vFileName.LastIndexOf("|") + 1))) + 1)
            If sTemp.Length = 0 Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If
            lNewPartyCnt = CInt(sTemp.Substring(0, sTemp.Length - 1))

            For iLoop1 As Integer = 1 To 5
                sTemp = Convert.ToString(r_oForm.mnuRecentFile(iLoop1).Tag)
                sTemp = Mid(sTemp, (IIf(sTemp = "" And "|" = "", 0, (sTemp.LastIndexOf("|") + 1))) + 1)
                lOldPartyCnt = CInt(sTemp.Substring(0, sTemp.Length - 1))

                If lNewPartyCnt = lOldPartyCnt Then
                    iFound = iLoop1
                    Exit For
                End If
            Next iLoop1

            If iFound = 0 Then
                iFound = 5
            End If

            ' Shuffle 1 to Found down to 2 to Found + 1
            For iLoop1 As Integer = iFound - 1 To 1 Step -1
                r_oForm.mnuRecentFile(iLoop1 + 1).Text = r_oForm.mnuRecentFile(iLoop1).Text
                r_oForm.mnuRecentFile(iLoop1 + 1).Available = r_oForm.mnuRecentFile(iLoop1).Available
                r_oForm.mnuRecentFile(iLoop1 + 1).Tag = Convert.ToString(r_oForm.mnuRecentFile(iLoop1).Tag)
            Next iLoop1

            ' Add the new entry
            iTemp = (v_vFileName.IndexOf("|"c) + 1)
            sShortName = v_vFileName.Substring(0, iTemp - 1)

            ' If there is an ampersand then double it up so that it displays  PN17033
            iTemp = (sShortName.IndexOf("&"c) + 1)
            If iTemp > 0 Then
                sShortName = sShortName.Substring(0, iTemp - 1) & "&&" & Mid(sShortName, iTemp + 1, sShortName.Length - iTemp)
            End If

            r_oForm.mnuRecentFile(1).Text = sShortName
            r_oForm.mnuRecentFile(1).Tag = v_vFileName
            r_oForm.mnuRecentFile(1).Available = True

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".AddRecentFile")

            Return result

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".AddRecentFile")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddRecentFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddRecentFile", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ProcessCNICSpecific
    '
    ' Description: Hides/Displays any CNIC specific items (ie. FISH)
    '
    ' History: 25/07/2002 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function ProcessCNICSpecific(ByRef r_oForm As Form) As Integer

        Dim result As Integer = 0
        Dim vValue As String = ""
        Dim bFish As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'Modified by vijay pal,Add a if condition 
            If Not Information.IsNothing(g_oObjectManager) Then
                ' Get the product option for Fish
                m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTFishInstalled, v_vBranch:=g_oObjectManager.SourceID, r_vUnderwriting:=vValue)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    vValue = ""
                End If

                If vValue = "" Then
                    vValue = "0"
                End If

                ' Do we want fish?
                bFish = Not (vValue = "0")

                ' Display or hide the Fish menu item

                'NIIT - Replaced with the Migrated code 1144
                'r_oForm.mnuGoToFish.Visible = bFish
                ReflectionHelper.SetMember(ReflectionHelper.GetMember(r_oForm, "mnuGoToFish"), "Visible", bFish)
            End If
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessCNICSpecific Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCNICSpecific", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result




            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ProcessFish
    '
    ' Description: Starts Fish with the passed client and policy details
    '
    ' History: 16/05/2002 CTAF - Created.
    '          24/07/2002 CTAF - Merged into tip code from CNIC
    '
    ' ***************************************************************** '
    Public Function ProcessFish(Optional ByVal v_vClientID As Object = Nothing, Optional ByVal v_vPolicyID As Object = Nothing) As Integer
        Dim result As Integer = 0
        Dim iPMBExtDocLink As Object


        Dim oFish As iPMBExtDocLink.Interface_Renamed


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Notes to FISH users...
            ' You need a registry setting for it to select the Fish class :
            ' HKEY_LOCAL_MACHINE\Software\PM\SiriusSolutions\Client
            ' DocumentInterface = Fish

            ' Create the object
            'TF011101 - Create ExtDocLink object
            Dim temp_oFish As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oFish, sClassName:="iPMBExtDocLink.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFish = temp_oFish
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of iPMBExtDocLink.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessFish", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Pass in ClientID

            If Not Information.IsNothing(v_vClientID) Then


                oFish.ClientID = CInt(v_vClientID)
            End If

            ' Pass in PolicyID

            If Not Information.IsNothing(v_vPolicyID) Then


                oFish.PolicyID = CInt(v_vPolicyID)
            End If


            m_lReturn = oFish.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            oFish.Dispose()
            oFish = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessFish Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessFish", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result




            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetPostQuoteScreenID
    '
    ' Description: Gets the post screen id for a risk group
    '
    ' History: 25/07/2002 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function GetPostQuoteScreenID(ByVal v_lRiskGroupID As Integer, ByRef r_lScreenID As Integer) As Integer
        Dim result As Integer = 0
        Dim bSIRRiskGroup As Object

        Const ACArrayRiskGroupID As Integer = 0
        Const ACArrayPQRiskScreenType As Integer = 8


        Dim oRiskGroup As Object
        Dim vRiskGroups(,) As Object



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get an instance of the risk group object
        Dim temp_oRiskGroup As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oRiskGroup, "bSIRRiskGroup.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        oRiskGroup = temp_oRiskGroup
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bSIRRiskGroup.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPostQuoteScreen", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Return result
        End If

        ' GetRiskGroup

        m_lReturn = oRiskGroup.GetRiskGroup(r_vDetailArray:=vRiskGroups)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bSIRRiskGroup.GetRiskGroup failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPostQuoteScreen", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Return result
        End If

        ' Terminate and clear up

        oRiskGroup.Dispose()
        oRiskGroup = Nothing

        ' Get the screens

        For lLoop1 As Integer = 0 To vRiskGroups.GetUpperBound(1)


            If CInt(vRiskGroups(ACArrayRiskGroupID, lLoop1)) = v_lRiskGroupID Then

                ' Get the right one
                'sj 15/08/2002 - start
                'r_lScreenID = CLng(vRiskGroups(ACArrayPQRiskScreenType, lLoop1))

                r_lScreenID = CInt(Conversion.Val(CStr(vRiskGroups(ACArrayPQRiskScreenType, lLoop1))))
                'sj 15/08/2002 - end
                ' exit the loop
                Exit For

            End If

        Next lLoop1

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: CallComplaint
    '
    ' Description:
    '
    ' History: 16/09/2003 SJ - Created.
    '
    ' ***************************************************************** '
    'sj 15/09/2003 - Start
    'JAS 10012005 PN17985 now using Folder not File cnt
    Private Function CallComplaint(ByVal v_lFsaComplaintFolderCnt As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim oComplaint As Object

        Dim vKeys(1, 0) As Object

        'JAS 10012005 PN17985 now using Folder not File cnt

        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameFSAComplaintFolderCnt

        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = v_lFsaComplaintFolderCnt

        oComplaint = CreateLateBoundObject("iPMBComplaint.NavigatorV3")

        With oComplaint
            m_lReturn = .Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="iPMBComplaint.NavigatorV3.initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CallComplaint")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .NavigatorV3_SetKeys(vKeys)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="iPMBComplaint.NavigatorV3.NavigatorV3_SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CallComplaint")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .NavigatorV3_SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="iPMBComplaint.NavigatorV3.NavigatorV3_SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CallComplaint")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .NavigatorV3_Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="iPMBComplaint.NavigatorV3.NavigatorV3_Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CallComplaint")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            .Dispose()
        End With

        Return result

    End Function
    'sj 15/09/2003 - End

    ' ***************************************************************** '
    '
    ' Name: ProcessFSAAccess
    '
    ' Description: Routine to process fsa access routin if fsa in use
    '
    ' History: 12/01/04 DC - Created.
    '
    ' ***************************************************************** '

    Public Function ProcessFSAAccess(ByVal lPartyCnt As Integer, ByVal sPartyType As String, ByRef bProceed As Boolean) As Integer
        Dim result As Integer = 0
        Dim iPMBFindParty As Object


        Dim oFindParty As iPMBFindParty.Interface_Renamed

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oFindParty As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oFindParty, sClassName:="iPMBFindParty.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFindParty = temp_oFindParty

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of iPMBFindParty.frmInterface", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessFSAAccess", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If


            m_lReturn = oFindParty.FSACustomerVal(lPartyCnt, sPartyType, bProceed)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            oFindParty.Dispose()
            oFindParty = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessFSAAccess Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessFSAAccess", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result




            Return result
        End Try
    End Function

    Private Function GetName(ByVal v_lPartyCnt As Integer, ByRef r_sPartyShortName As String, ByRef r_sPartyResolvedName As String) As Integer
        Dim result As Integer = 0
        Dim bSIRFindParty As Object


        Dim oObject As bSIRFindParty.Business



        result = gPMConstants.PMEReturnCode.PMTrue

        Dim temp_oObject As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oObject, "bSIRFindParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        oObject = temp_oObject

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create an instance of bSIRFindParty.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetName", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            Return result
        End If



        m_lReturn = oObject.GetName(v_lPartyCnt, r_sPartyShortName)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = oObject.GetResolvedName(v_lPartyCnt, r_sPartyResolvedName)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        oObject.Dispose()

        oObject = Nothing

        Return result

    End Function

    Public Function LoadRecentFilesFromReg() As Integer

        Dim result As Integer = 0
        Dim iLoop1 As Integer
        Dim sKey, sShortName As String
        Dim iTemp As Integer
        Dim sFile As String = ""
        Dim lMenuCount, lPartyCnt As Integer
        Dim sTemp, sResolvedName As String
        Dim sNames As New StringBuilder

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise
            iLoop1 = 1
            lMenuCount = 1
            sNames = New StringBuilder("#")

            sFile = ":-)"

            Do While (sFile <> "")

                ' Construct the key to check
                sKey = "RecentFile" & iLoop1

                m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=sKey, r_sSettingValue:=sFile, v_sSubKey:="RecentFiles")

                'MKR 29/09/2004 PN 6021 'We are now using '|' as a delimiter rather than ','

                iTemp = (sFile.IndexOf("|"c) + 1)

                ' Check its a valid record
                If iTemp > 0 Then

                    sTemp = Mid(sFile, (IIf(sFile = "" And "|" = "", 0, (sFile.LastIndexOf("|") + 1))) + 1)
                    lPartyCnt = CInt(sTemp.Substring(0, sTemp.Length - 1))

                    'Get party short and resolved names from database in case it has changed.
                    m_lReturn = GetName(lPartyCnt, sShortName, sResolvedName)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Do not show duplicate names.
                    If (sNames.ToString().IndexOf("#" & sShortName & "#") + 1) = 0 Then

                        sNames.Append(sShortName & "#")

                        ' If there is an ampersand then double it up so that it displays  PN17033
                        iTemp = (sShortName.IndexOf("&"c) + 1)
                        If iTemp > 0 Then
                            sShortName = sShortName.Substring(0, iTemp - 1) & "&&" & Mid(sShortName, iTemp + 1, sShortName.Length - iTemp)
                        End If

                        m_ofrmMDI.mnuRecentFile(lMenuCount).Text = sShortName
                        m_ofrmMDI.mnuRecentFile(lMenuCount).Tag = sShortName & "|" & sResolvedName & "|" & CStr(lPartyCnt) & sFile.Substring(sFile.Length - 1)
                        m_ofrmMDI.mnuRecentFile(lMenuCount).Available = True

                        sNames.Append(sShortName & "#")
                        lMenuCount += 1
                    End If

                End If

                ' Increment to the next file
                iLoop1 += 1

            Loop

            m_lReturn = SaveRecentFiles(m_ofrmMDI)

            Return result

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".LoadRecentFilesFromReg")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadRecentFilesFromReg Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadRecentFilesFromReg", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function ShowRecentFile(ByVal iIndex As Integer, ByRef r_oForm As Form) As Integer

        Dim result As Integer = 0

        '*****************************************************************************************************
        'MKR 29/09/04 PN 6021.
        'We are now using '|' as a delimiter rather than ',' as seen in the bug fix of this issue in 1.8.5
        '*****************************************************************************************************


        Dim sTag As String
        Dim oMenu As Object
        'Select Case r_oForm.Name
        '    Case "frmPartyPCView"
        '        Dim frmObj As frmPartyPCView = DirectCast(r_oForm, frmPartyPCView)
        '        sTag = frmObj.mnuRecentFile(iIndex).Tag

        '    Case "frmPartyPC"
        '        Dim frmObj As frmPartyPC = DirectCast(r_oForm, frmPartyPC)
        '        sTag = frmObj.mnuRecentFile(iIndex).Tag
        'End Select

        'Modified by Ankit Jain 7-7-2010
        If r_oForm Is Nothing Then
            Return gPMConstants.PMEReturnCode.PMTrue
        End If
        Try
            oMenu = ReflectionHelper.GetMember(r_oForm, "mnuRecentFile", New Object() {iIndex})
        Catch ex As Exception
            Return gPMConstants.PMEReturnCode.PMTrue
        End Try

        If Not oMenu Is Nothing Then
            sTag = ReflectionHelper.GetMember(oMenu, "Tag")
        End If


        'Select Case DirectCast(r_oForm.ActiveControl, System.Windows.Forms.Control).Name
        '    Case "frmPartyPCView"
        '        Dim frmObj As frmPartyPCView = DirectCast(DirectCast(r_oForm.ActiveControl, System.Windows.Forms.Control), frmPartyPCView)
        '        sTag = frmObj.mnuRecentFile(iIndex).Tag

        '    Case "frmPartyPC"
        '        Dim frmObj As frmPartyPC = DirectCast(DirectCast(r_oForm.ActiveControl, System.Windows.Forms.Control), frmPartyPC)
        '        sTag = frmObj.mnuRecentFile(iIndex).Tag

        '    Case "frmPartyCCView"
        '        Dim frmObj As frmPartyCCView = DirectCast(DirectCast(r_oForm.ActiveControl, System.Windows.Forms.Control), frmPartyCCView)
        '        sTag = frmObj.mnuRecentFile(iIndex).Tag

        '    Case "frmPartyCC"
        '        Dim frmObj As frmPartyCC = DirectCast(DirectCast(r_oForm.ActiveControl, System.Windows.Forms.Control), frmPartyCC)
        '        sTag = frmObj.mnuRecentFile(iIndex).Tag

        '    Case "frmPartyGCView"
        '        Dim frmObj As frmPartyGCView = DirectCast(DirectCast(r_oForm.ActiveControl, System.Windows.Forms.Control), frmPartyGCView)
        '        sTag = frmObj.mnuRecentFile(iIndex).Tag

        '    Case "frmPartyGC"
        '        Dim frmObj As frmPartyGC = DirectCast(DirectCast(r_oForm.ActiveControl, System.Windows.Forms.Control), frmPartyGC)
        '        sTag = frmObj.mnuRecentFile(iIndex).Tag
        'End Select

        Dim iTemp As Integer = (sTag.IndexOf("|"c) + 1)
        Dim vPartyShortName As String = sTag.Substring(0, iTemp - 1)

        Dim sTemp As String = Mid(sTag, iTemp + 1)
        iTemp = (sTemp.IndexOf("|"c) + 1)
        Dim vPartyResolvedName As String = sTemp.Substring(0, iTemp - 1)

        sTemp = Mid(sTemp, iTemp + 1)
        Dim vPartyType As String = sTemp.Substring(sTemp.Length - 1)
        Dim vPartyCnt As String = sTemp.Substring(0, sTemp.Length - 1)

        'Shuffle lines down.
        For lLoop As Integer = iIndex - 1 To 1 Step -1
            m_ofrmMDI.mnuRecentFile(lLoop + 1).Text = m_ofrmMDI.mnuRecentFile(lLoop).Text
            m_ofrmMDI.mnuRecentFile(lLoop + 1).Available = m_ofrmMDI.mnuRecentFile(lLoop).Available
            m_ofrmMDI.mnuRecentFile(lLoop + 1).Tag = Convert.ToString(m_ofrmMDI.mnuRecentFile(lLoop).Tag)
        Next

        'Move selected entry to the top.
        m_ofrmMDI.mnuRecentFile(1).Text = vPartyShortName
        m_ofrmMDI.mnuRecentFile(1).Tag = sTag
        m_ofrmMDI.mnuRecentFile(1).Available = True

        'Save the changes.
        m_lReturn = SaveRecentFiles(m_ofrmMDI)

        ' If we have 2 "&" (for display purposes) then replace with 1   PN17033
        vPartyShortName = vPartyShortName.Replace("&&", "&")

        'Show the client

        'NIIT - Replaced with the Migrated code 1144 
        'm_lReturn = CallCMManager(v_lCurrentPartyCnt:=r_oForm.PartyCnt, v_lPartyCnt:=CInt(vPartyCnt), v_sPartyShortName:=vPartyShortName, v_sPartyResolvedName:=vPartyResolvedName, v_sPartyType:=vPartyType)
        'Modified by Ankit Jain 7-7-2010
        'm_lReturn = CallCMManager(ReflectionHelper.GetMember(r_oForm, "PartyCnt"), CInt(vPartyCnt), vPartyShortName, vPartyResolvedName, vPartyType)
        m_lReturn = CallCMManager(ReflectionHelper.GetMember(DirectCast(r_oForm, System.Windows.Forms.Control), "PartyCnt"), CInt(vPartyCnt), vPartyShortName, vPartyResolvedName, vPartyType)

        m_lReturn = LoadRecentFiles(r_oForm:=r_oForm)

        Return result



        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".ShowRecentFile")

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowRecentFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowRecentFile", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function

    Public Function UpdatePartyScreen() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdatePartyScreen"


        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            For Each frmChild As Form In m_frmParentMdiForm.MdiChildren
                Select Case frmChild.Name
                    Case "frmPartyPC", "frmPartyPCView"
                        If ReflectionHelper.GetMember(frmChild, "PartyCnt") = m_ofrmMDI.PartyCnt Then
                            If ReflectionHelper.Invoke(ReflectionHelper.GetMember(frmChild, "uctPartyPCControl1"), "UpdateTurnoverDetails", New Object() {}) <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError("Forms(lCount).uctPartyPCControl1.UpdateTurnoverDetails()", "PartyCnt = " & m_ofrmMDI.PartyCnt, gPMConstants.PMELogLevel.PMLogError)
                                Exit For
                            End If
                        End If
                    Case "frmPartyGC", "frmPartyGCView"
                        If ReflectionHelper.GetMember(frmChild, "PartyCnt") = m_ofrmMDI.PartyCnt Then
                            If ReflectionHelper.Invoke(ReflectionHelper.GetMember(frmChild, "uctPartyGCControl1"), "UpdateTurnoverDetails", New Object() {}) <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError("Forms(lCount).uctPartyGCControl1.UpdateTurnoverDetails()", "PartyCnt = " & m_ofrmMDI.PartyCnt, gPMConstants.PMELogLevel.PMLogError)
                                Exit For
                            End If
                        End If
                        'Case "frmPartyCC", "frmPartyCCView"
                        '    If ReflectionHelper.GetMember(frmChild, "PartyCnt") = m_ofrmMDI.PartyCnt Then
                        '        If ReflectionHelper.Invoke(ReflectionHelper.GetMember(frmChild, "uctPartyCCControl1"), "UpdateTurnoverDetails", New Object() {}) <> gPMConstants.PMEReturnCode.PMTrue Then
                        '            gPMFunctions.RaiseError("Forms(lCount).uctPartyCCControl1.UpdateTurnoverDetails()", "PartyCnt = " & m_ofrmMDI.PartyCnt, gPMConstants.PMELogLevel.PMLogError)
                        '            Exit For
                        '        End If
                        '    End If
                    Case "frmPartyCC"
                        Dim frm As frmPartyCC = DirectCast(frmChild, frmPartyCC)
                        If frm.PartyCnt = m_ofrmMDI.PartyCnt Then
                            If frm.uctPartyCCControl1.UpdateTurnoverDetails() <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError("Forms(lCount).uctPartyCCControl1.UpdateTurnoverDetails()", "PartyCnt = " & m_ofrmMDI.PartyCnt, gPMConstants.PMELogLevel.PMLogError)
                                Exit For
                            End If
                        End If
                    Case "frmPartyCCView"
                        Dim frm As frmPartyCCView = DirectCast(frmChild, frmPartyCCView)
                        If frm.PartyCnt = m_ofrmMDI.PartyCnt Then
                            If frm.uctPartyCCControl1.UpdateTurnoverDetails() <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError("Forms(lCount).uctPartyCCControl1.UpdateTurnoverDetails()", "PartyCnt = " & m_ofrmMDI.PartyCnt, gPMConstants.PMELogLevel.PMLogError)
                                Exit For
                            End If
                        End If
                End Select
            Next

            'For lCount As Integer = 0 To Application.OpenForms.Count - 1
            '    If Application.OpenForms.Item(lCount).Name = "frmPartyPC" Or Application.OpenForms.Item(lCount).Name = "frmPartyPCView" Then

            '        If ReflectionHelper.GetMember(Application.OpenForms.Item(lCount), "PartyCnt") = m_ofrmMDI.PartyCnt Then

            '            If ReflectionHelper.Invoke(ReflectionHelper.GetMember(Application.OpenForms.Item(lCount), "uctPartyPCControl1"), "UpdateTurnoverDetails", New Object() {}) <> gPMConstants.PMEReturnCode.PMTrue Then
            '                gPMFunctions.RaiseError("Forms(lCount).uctPartyPCControl1.UpdateTurnoverDetails()", "PartyCnt = " & m_ofrmMDI.PartyCnt, gPMConstants.PMELogLevel.PMLogError)
            '            End If

            '            Exit For

            '        End If
            '    ElseIf Application.OpenForms.Item(lCount).Name = "frmPartyGC" Or Application.OpenForms.Item(lCount).Name = "frmPartyGCView" Then

            '        If ReflectionHelper.GetMember(Application.OpenForms.Item(lCount), "PartyCnt") = m_ofrmMDI.PartyCnt Then

            '            If ReflectionHelper.Invoke(ReflectionHelper.GetMember(Application.OpenForms.Item(lCount), "uctPartyGCControl1"), "UpdateTurnoverDetails", New Object() {}) <> gPMConstants.PMEReturnCode.PMTrue Then
            '                gPMFunctions.RaiseError("Forms(lCount).uctPartyGControl1.UpdateTurnoverDetails()", "PartyCnt = " & m_ofrmMDI.PartyCnt, gPMConstants.PMELogLevel.PMLogError)
            '            End If

            '            Exit For

            '        End If
            '    ElseIf Application.OpenForms.Item(lCount).Name = "frmPartyCC" Or Application.OpenForms.Item(lCount).Name = "frmPartyCCView" Then

            '        If ReflectionHelper.GetMember(Application.OpenForms.Item(lCount), "PartyCnt") = m_ofrmMDI.PartyCnt Then

            '            If ReflectionHelper.Invoke(ReflectionHelper.GetMember(Application.OpenForms.Item(lCount), "uctPartyCCControl1"), "UpdateTurnoverDetails", New Object() {}) <> gPMConstants.PMEReturnCode.PMTrue Then
            '                gPMFunctions.RaiseError("Forms(lCount).uctPartyCControl1.UpdateTurnoverDetails()", "PartyCnt = " & m_ofrmMDI.PartyCnt, gPMConstants.PMELogLevel.PMLogError)
            '            End If

            '            Exit For

            '        End If
            '    End If

            'Next




        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(ACClass, kMethodName, result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here

            '        Return result

            ' This is for debugging only
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    Public Function GetDocumentLibrary(ByVal v_lPartyCnt As Integer, ByRef r_lDocumentLibrary As String, Optional ByVal v_lPartyShortName As String = "") As Integer
        Dim bSIRFindParty As Object
        Dim result As Integer = 0

        Dim oObject As bSIRFindParty.Business

        Dim temp_oObject As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oObject, "bSIRFindParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        oObject = temp_oObject

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create an instance of bSIRFindParty.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetName", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        result = oObject.GetDocumentLibraryFromDB(v_lParty_Cnt:=v_lPartyCnt, r_lDocument_Library:=r_lDocumentLibrary)

        oObject.Dispose()
        oObject = Nothing

        Return result

    End Function
End Class
