Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports System.Windows.Forms
Imports SharedFiles
'Module FileOpen
Partial NotInheritable Class MainModule
    '*** Standard module with procedures for working with   ***
    '*** files. Part of the MDI Notepad sample application. ***
    '**********************************************************

    'Private Const ACClass As String = "FileOpen"

    'Private m_lReturn As Integer
    Public m_frmParentMdiForm As frmMDI

    Function FileOpenProc(ByVal v_lPartyCnt As Integer, ByRef vPartyType As String) As Integer
        Dim result As Integer = 0
        'modified,comment
        Dim oFindParty As iPMBFindParty.Interface_Renamed
        Dim lError As gPMConstants.PMEReturnCode
        Dim vKeyArray(,) As Object
        Dim vPartyCnt As String = ""
        'Modified as Developer Guide no 33
        'Dim vPartyShortName As String = ""
        Dim vPartyShortName As Object
        'Modified as Developer Guide no 33
        'Dim vPartyResolvedName As String = ""
        Dim vPartyResolvedName As Object

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            ' Create FindPolicy object
            Dim temp_oFindParty As Object
            lError = g_oObjectManager.GetInstance(temp_oFindParty, sClassName:="iPMBFindParty.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFindParty = temp_oFindParty

            If lError <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create 'iSIRFindInsurance.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="FileOpenProc", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'Developer Guide No.9
            'lError = CType(oFindParty, SSP.S4I.Interfaces.ILocalInterface).Initialise()
            lError = oFindParty.Initialise()


            oFindParty.CallingAppName = "ClientManager"

            ReDim vKeyArray(1, 0)

            vKeyArray(0, 0) = gSIRLibrary.SIRNavKeyAgentOnly

            Select Case vPartyType
                Case "P"

                    vKeyArray(1, 0) = 1
                Case "C"

                    vKeyArray(1, 0) = 2
                Case "G"

                    vKeyArray(1, 0) = 3
            End Select


            lError = oFindParty.SetKeys(vKeyArray)

            'SD 01/08/2002 Scalability changes

            lError = oFindParty.SetProcessModes(vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vEffectiveDate:=DateTime.Now)

            ReDim vKeyArray(1, 0)


            lError = oFindParty.Start()


            lError = oFindParty.GetKeys(vKeyArray)


            oFindParty.Dispose()

            oFindParty = Nothing

            ' Here we get the passed client
            'If we have a client

            vPartyCnt = CStr(vKeyArray(1, 0))

            vPartyShortName = CStr(vKeyArray(1, 1))

            vPartyResolvedName = CStr(vKeyArray(1, 2))

            vPartyType = CStr(vKeyArray(1, 8))

            Select Case vPartyType
                Case CStr(1)
                    vPartyType = "P"
                Case CStr(2)
                    vPartyType = "C"
                Case CStr(3)
                    vPartyType = "G"
            End Select


            If CDbl(vKeyArray(1, 0)) > 0 Then

                lError = CType(CallCMManager(v_lCurrentPartyCnt:=v_lPartyCnt, v_lPartyCnt:=CInt(vPartyCnt), v_sPartyShortName:=vPartyShortName, v_sPartyResolvedName:=vPartyResolvedName, v_sPartyType:=vPartyType), gPMConstants.PMEReturnCode)

                'lError = OpenSummaryFile(vPartyCnt:=vPartyCnt, _
                ''                         vPartyShortName:=vPartyShortName, _
                ''                         vPartyType:=vPartyType, _
                ''                         vPartyResolvedName:=vPartyResolvedName)
                '
                If lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lError
                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FileOpenProc failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="FileOpenProc", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Function OnRecentFilesList(ByRef vShortName As Object) As Integer

        Dim result As Integer = 0
        ' Counter variable.

        Try


            For i As Integer = m_frmParentMdiForm.mnuRecentFile.GetLowerBound(0) To m_frmParentMdiForm.mnuRecentFile.GetUpperBound(0)

                If m_frmParentMdiForm.mnuRecentFile(i).Text = CStr(vShortName) Then
                    Return i
                End If
            Next i


            Return False

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="OnRecentFilesList failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="OnRecentFilesList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'Update History
    'Added Optional bIsIncludeClosedBranchchecked for adding the Closed branches
    Function OpenFile(ByRef vPartyCnt As Integer, ByRef vPartyShortName As String, ByRef vPartyType As String, ByRef vPartyResolvedName As String, Optional ByRef bIsIncludeClosedBranchchecked As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim fIndex As Integer
        Dim vFileName As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Open the selected file.
            'MKR 29/09/2004 PN 6021 'We are now using '|' as a delimiter rather than ','
            vFileName = vPartyShortName & "|" & vPartyResolvedName & "|" & CStr(vPartyCnt) & vPartyType

            '    Call UpdateFileMenu(vFileName)
            fIndex = FindFreeIndex()

            Select Case vPartyType
                Case "P"
                    Document(fIndex) = New frmPartyPC(m_frmParentMdiForm)
                    ' Save the old party count

                    Document(fIndex).PartyCnt = vPartyCnt

                    Document(fIndex).IsIncludeClosedBranchChecked = bIsIncludeClosedBranchchecked

                    Document(fIndex).Text = "Personal Client : [" & vPartyResolvedName.Trim() & "]"
                Case "C"
                    Document(fIndex) = New frmPartyCC(m_frmParentMdiForm)
                    ' Save the old party count

                    Document(fIndex).PartyCnt = vPartyCnt

                    Document(fIndex).IsIncludeClosedBranchChecked = bIsIncludeClosedBranchchecked

                    Document(fIndex).Text = "Corporate Client : [" & vPartyResolvedName.Trim() & "]"


                Case "G"
                    Document(fIndex) = New frmPartyGC(m_frmParentMdiForm)

                    Document(fIndex).PartyCnt = vPartyCnt

                    Document(fIndex).IsIncludeClosedBranchChecked = bIsIncludeClosedBranchchecked

                    Document(fIndex).Text = "Group Client : [" & vPartyResolvedName.Trim() & "]"

            End Select

            Document(fIndex).ModuleClass = Me

            'Note that it's the setting of the tag that loads the control on the form

            Document(fIndex).Tag = fIndex

            Document(fIndex).Footer = "Client:" & fIndex

            Document(fIndex).ShortName = vPartyShortName

            Document(fIndex).ResolvedName = vPartyResolvedName

            Document(fIndex).PartyType = vPartyType 'FSA Phase III

            Document(fIndex).Index = fIndex



            m_frmParentMdiForm.StatusBar1.Items.Item(0).Text = "Client: " & fIndex
            m_frmParentMdiForm.StatusBar1.Items.Item(1).Text = CStr(vPartyCnt)
            m_frmParentMdiForm.StatusBar1.Items.Item(2).Text = vPartyShortName
            m_frmParentMdiForm.Text = "Sirius Client Manager : [" & vPartyResolvedName.Trim() & "]"

            'Where it was done before the recent list wasn't populated as we didn't have the document
            'open yet.
            UpdateFileMenu(vFileName)

            Document(fIndex).LoadInterface()
            '2005 Sticky Notes cause main file to load in the wrong place

            Document(fIndex).Top = 0

            Document(fIndex).Left = 0
            '2005 End


            Document(fIndex).Show()

            ' Reset the mouse pointer.
            Cursor.Current = Cursors.Default

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="OpenFile failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="OpenFile", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Function OpenSummaryFile(ByRef vPartyCnt As Integer, ByRef vPartyShortName As String, ByRef vPartyType As String, ByRef vPartyResolvedName As String, Optional ByRef bIsIncludeClosedBranchchecked As Boolean = False) As Integer
        'JT PN-13238 01-11-2004
        'Added extra optional Parameter for IncludeClosed Branch Checked
        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue


        Try

            ' Open the selected file.
            'MKR 29/09/2004 PN 6021 'We are now using '|' as a delimiter rather than ','
            Dim vFileName As String = vPartyShortName & "|" & vPartyResolvedName & "|" & CStr(vPartyCnt) & vPartyType



            '    Call UpdateFileMenu(vFileName)
            Dim fIndex As Integer = FindFreeIndex()

            '    'DC150301 -start
            '    Set Document(fIndex) = New frmPartySummary
            '    If vPartyType = "P" Then
            '        Set Document(fIndex) = New frmPartyPCView
            '    End If
            '    If vPartyType = "G" Then
            '        Set Document(fIndex) = New frmPartyGCView
            '    End If
            '    If vPartyType = "C" Then
            '        Set Document(fIndex) = New frmPartyCCView
            '    End If
            '    'DC150301 -end

            'SD 12/07/02 START Show client summary screen depending on product option
            Dim vValue As String = ""

            m_lReturn = iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTClientSummary, gPMConstants.SIRBCHHeadOffice, vValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Open Summary File failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="OpenSummaryFile", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse

            End If
            If vValue = "1" Then

                If Not gPartySummaryScreenShown Then
                    If CBool(vValue) Then
                        Document(fIndex) = New frmPartySummary(m_frmParentMdiForm)
                    End If
                Else
                    If vPartyType = "P" Then
                        Document(fIndex) = New frmPartyPC(m_frmParentMdiForm)
                    End If
                    If vPartyType = "G" Then
                        Document(fIndex) = New frmPartyGC(m_frmParentMdiForm)
                    End If
                    If vPartyType = "C" Then
                        Document(fIndex) = New frmPartyCC(m_frmParentMdiForm)
                    End If
                End If

            Else
                If vPartyType = "P" Then
                    Document(fIndex) = New frmPartyPCView(m_frmParentMdiForm)
                End If
                If vPartyType = "G" Then
                    Document(fIndex) = New frmPartyGCView(m_frmParentMdiForm)
                End If
                If vPartyType = "C" Then
                    Document(fIndex) = New frmPartyCCView(m_frmParentMdiForm)
                End If
            End If

            If Not Document(fIndex) Is Nothing Then
                Document(fIndex).ModuleClass = Me
            End If


            'reset the global variable
            gPartySummaryScreenShown = False

            If Not Document(fIndex) Is Nothing Then
                Document(fIndex).PartyCnt = vPartyCnt

                Document(fIndex).IsIncludeClosedBranchChecked = bIsIncludeClosedBranchchecked
                'Note that it's the setting of the tag that loads the control on the form

                Document(fIndex).Tag = fIndex

                Document(fIndex).Text = "View: [" & vPartyResolvedName.Trim() & "]"

                Document(fIndex).Footer = "Client:" & fIndex

                Document(fIndex).ShortName = vPartyShortName

                Document(fIndex).ResolvedName = vPartyResolvedName

                Document(fIndex).Index = fIndex

                Document(fIndex).PartyType = vPartyType
                'DC150301

                Document(fIndex).Status = gPMConstants.PMEComponentAction.PMView
            End If



            m_frmParentMdiForm.StatusBar1.Items.Item(0).Text = "Client: " & fIndex
            m_frmParentMdiForm.StatusBar1.Items.Item(1).Text = CStr(vPartyCnt)
            m_frmParentMdiForm.StatusBar1.Items.Item(2).Text = vPartyShortName
            m_frmParentMdiForm.Text = "[" & vPartyResolvedName.Trim() & "]" & " Sirius Client Manager"

            m_frmParentMdiForm.PartyCnt = vPartyCnt
            m_frmParentMdiForm.ResolvedName = vPartyResolvedName 'ADDED MK 991014
            m_frmParentMdiForm.ShortName = vPartyShortName 'ADDED MK 991014
            m_frmParentMdiForm.PartyType = vPartyType 'ADDED MKR PN 17193

            g_sResolvedName = vPartyResolvedName
            'Where it was done before the recent list wasn't populated as we didn't have the document
            'open yet.

            UpdateFileMenu(vFileName)

            If Not Document(fIndex) Is Nothing Then
                Document(fIndex).LoadInterface()
                '2005 Sticky Notes cause main file to load in the wrong place

                Document(fIndex).Top = 0

                Document(fIndex).Left = 0
                '2005 End

                Document(fIndex).Show()
            End If


            'StickyNotes
            Dim fPartyIndex As Integer = fIndex '2005 Sticky Notes
            m_lReturn = OpenWarnings(vPartyCnt:=vPartyCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error.
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("vPartyCnt", vPartyCnt)
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to open warnings for " & vPartyResolvedName, vApp:=ACApp, vClass:=ACClass, vMethod:="OpenSummaryFile", oDicParms:=oDict)
                Return result
            End If

            ' Reset the mouse pointer.
            Cursor.Current = Cursors.Default

            Return result

        Catch exc As System.Exception
            'NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try

    End Function
    '2005 StickyNotes
    Function OpenWarnings(ByRef vPartyCnt As Object) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object
        Dim fIndex As Integer
        result = gPMConstants.PMEReturnCode.PMTrue


        Try

            Dim temp_g_oEvent As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oEvent, "bSIREvent.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            g_oEvent = temp_g_oEvent
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Instance of bSIREvent.", vApp:=ACApp, vClass:=ACClass, vMethod:="OpenWarnings", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            m_lReturn = g_oEvent.SearchWarnings(r_vResultArray:=vResultArray, v_vPartyCnt:=vPartyCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Client warnings.", vApp:=ACApp, vClass:=ACClass, vMethod:="OpenWarnings", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse

            End If


            If Not Information.IsArray(vResultArray) Then
                Return result
            End If


            Dim lFormLeft As Integer = 11100
            Dim lFormTop As Integer = 1300


            ReDim g_vWarnings(0, vResultArray.GetUpperBound(1))


            For lCount As Integer = 0 To vResultArray.GetUpperBound(1)
                fIndex = FindFreeIndex()

                g_vWarnings(0, lCount) = fIndex

                Document(fIndex) = New frmWarning(m_frmParentMdiForm)

                Document(fIndex).ModuleClass = Me


                Document(fIndex).EventCnt = CInt(vResultArray(0, lCount))


                Document(fIndex).EventDate = CDate(vResultArray(1, lCount))


                Document(fIndex).Subject = vResultArray(2, lCount)


                Document(fIndex).PriorityCode = vResultArray(3, lCount)


                Document(fIndex).Username = vResultArray(6, lCount)

                If (vResultArray(7, lCount) = "") Then
                    Document(fIndex).SubjectId = Nothing
                Else
                    Document(fIndex).SubjectId = vResultArray(7, lCount)
                End If



                Document(fIndex).EventType = vResultArray(8, lCount)


                Document(fIndex).Description = vResultArray(9, lCount)

                Document(fIndex).Tag = fIndex

                Dim dbNumericTemp As Double
                If Not Double.TryParse(CStr(vResultArray(4, lCount)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                    Document(fIndex).FormTop = lFormTop

                    Document(fIndex).FormLeft = lFormLeft
                    lFormTop += 200
                    lFormLeft += 200
                Else


                    Document(fIndex).FormTop = vResultArray(4, lCount)


                    Document(fIndex).FormLeft = vResultArray(5, lCount)
                End If

                Document(fIndex).LoadInterface()

                'Developer Guide No 
                'Document(fIndex).ZOrder(0)
                Document(fIndex).focused()
                'Document(fIndex).Show
            Next lCount

            Return result

        Catch exc As System.Exception
            NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try

    End Function

    Public Function UpdateFileMenu(ByRef vFileName As String) As Integer

        Dim result As Integer = 0
        Dim vShortName As String = ""

        Try


            'MKR 29/09/2004 PN 6021
            'Note : As this code was commented so didn't changed it... but anyone who uncomment this
            'Please note that now the data is seperated by "|" and not by ","

            'DC230801 -start -commented out as it does not recompile.
            '           CF not here to advise,
            '           so needs to revist this as I was not sure if it was okay to comment out.
            '    ' Update the list of the most recently opened files in the File menu control array.
            '    m_lReturn& = GetRecentFiles()
            '    If (m_lReturn& <> PMTrue) Then
            '        UpdateFileMenu = PMFalse
            '        Exit Function
            '    End If
            '
            '    ' Check if the open filename is already in the File menu control array.
            '    vShortName = Left(vFileName, InStr(vFileName, ",") - 1)
            '    intRetVal = OnRecentFilesList(vShortName)
            '
            '    ' Write open filename to the registry.
            '    m_lReturn& = WriteRecentFiles(vFileName, intRetVal)
            '    If (m_lReturn& <> PMTrue) Then
            '        UpdateFileMenu = PMFalse
            '        Exit Function
            '    End If
            '
            '    ' Update the list of the most recently opened files in the File menu control array.
            '    m_lReturn& = GetRecentFiles()
            '    If (m_lReturn& <> PMTrue) Then
            '        UpdateFileMenu = PMFalse
            '        Exit Function
            '    End If
            'DC230801 -end

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateFileMenu failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateFileMenu", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class
