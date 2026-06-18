Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.Windows.Forms
<System.Runtime.InteropServices.ProgId("PBTabStripCommon_NET.PBTabStripCommon")> _
 Public Module PBTabStripCommon
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
    'Added by Deepak Sharma on 4/20/2010 1:23:47 PM refer developer guide no. 

    Public ACApp As String


    Private Const ACClass As String = "PBTabStripCommon"

    ' ***************************************************************** '
    '
    ' Name: AddTab
    '
    ' Description: Allows TabStrip to be used instead of the old SSTab control
    '
    ' History: 24/06/2002 CLG - Created.
    '          12/09/2003 CLG CQ2001 Wrong Tab Displayed On Opening Risk
    '
    ' ***************************************************************** '
    Public Sub AddTab(ByRef r_TabStrip As TabControl, ByVal v_iIndex As Integer, ByVal v_sCaption As Object, Optional ByRef uFlatMenuTree As Object = Nothing)
        'Modified by Alkesh Kumar on 10/05/2010 12:34:17 refer developer guide no. 
        'Dim iSelectedItem As Double
        Dim iSelectedItem As Short
        Dim bFlag As Boolean = False
        Dim i As Integer
        Dim itempindex As Integer
        Dim j As Integer = 1

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".AddTab")

        Try

            'Modified by Alkesh Kumar on 10/05/2010 12:25:37 refer developer guide no. 
            'Dim lTab As Double
            Dim lTab As Integer

#If SwiftControls = 1 Then

			Dim bAddCaption As Boolean
			If Left(v_sCaption, 1) = "@" Then
			bAddCaption = True
			v_sCaption = Mid(v_sCaption, 2, Len(v_sCaption) - 1)
			End If
#End If


            'Modified by Alkesh Kumar on 10/05/2010 12:35:47 refer developer guide no. 156
            'For lTab = 1 To r_TabStrip.TabPages.Count
            For lTab = 0 To r_TabStrip.TabPages.Count - 1
                'Dim postion As Integer = v_sCaption.ToString().Substring(0, v_sCaption.ToString().IndexOf(" -"))
                'Modified by Alkesh Kumar on 10/05/2010 12:36:26 refer developer guide no. 159
                If CInt(r_TabStrip.TabPages(lTab).Name.Substring(1, r_TabStrip.TabPages(lTab).Name.Length - 1)) = v_iIndex Then
                    Exit Sub
                End If
                'Modified by Alkesh Kumar on 10/05/2010 12:36:26 refer developer guide no. 159
                If CInt(r_TabStrip.TabPages(lTab).Name.Substring(1, r_TabStrip.TabPages(lTab).Name.Length - 1)) > v_iIndex Then
                    'iSelectedItem = ContainerHelper.GetControlIndex(r_TabStrip.SelectedTab)
                    'Fix for TFS 5703
                    iSelectedItem = r_TabStrip.TabPages.IndexOf(r_TabStrip.SelectedTab)
                    'Modified by Alkesh Kumar on 10/05/2010 12:34:30 refer developer guide no. 155
                    'r_TabStrip.TabPages.Add(lTab, "_" & v_iIndex, v_sCaption)
                    'r_TabStrip.TabPages.Add("_" & v_iIndex, v_sCaption, lTab)
                    'Fix for TFS 5703
                    For i = 0 To r_TabStrip.TabPages.Count - 1
                        If r_TabStrip.TabPages(i).Text <> v_sCaption Then
                            bFlag = True
                        Else
                            bFlag = False
                            Exit Sub
                        End If
                    Next
                    If bFlag = True Then
                        For i = 0 To r_TabStrip.TabPages.Count - 1
                            If CInt(r_TabStrip.TabPages(i).Name.Substring(1, r_TabStrip.TabPages(i).Name.Length - 1)) > v_iIndex Then
                                itempindex = r_TabStrip.TabPages.Count - j
                                j = j + 1

                            End If
                        Next

                        r_TabStrip.TabPages.Insert(itempindex, "_" & v_iIndex, v_sCaption, lTab)

                    End If
                    If lTab <= iSelectedItem Then
                        'if we have added a tab below the one with focus the focus will move so put it back
                        r_TabStrip.SelectTab(iSelectedItem + 1)
                    End If
                    Exit Sub
                End If
            Next

            'Modified by Alkesh Kumar on 10/05/2010 12:37:19 refer developer guide no. 155
            'r_TabStrip.TabPages.Add(lTab, "_" & v_iIndex, v_sCaption)
            r_TabStrip.TabPages.Add("_" & v_iIndex, v_sCaption)
#If SwiftControls = 1 Then

			If Not IsMissing(uFlatMenuTree) Then
			AddMenuTreeItem uFlatMenuTree, v_iIndex, v_sCaption
			If bAddCaption = True Then
			uFlatMenuTree.AddSeperator v_iIndex
			End If
			End If
#End If



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".AddTab")

        Catch



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".AddTab")

            Exit Sub
        End Try


    End Sub

    ' ***************************************************************** '
    '
    ' Name: HideTab
    '
    ' Description: Allows TabStrip to be used instead of the old SSTab control
    '
    ' History: 24/06/2002 CLG - Created.
    '        : 20/06/2003 CQ1067 SMJB - Added code to reselect required tab after removing tab
    '          12/09/2003 CLG CQ2001 Wrong Tab Displayed On Opening Risk
    '
    ' ***************************************************************** '
    Public Function HideTab(ByRef r_TabStrip As TabControl, ByVal v_iIndex As Integer, Optional ByRef r_uFlatMenuTree As Object = Nothing) As Boolean

        ' Debug message
        Dim result As Boolean = False
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".HideTab")

        Try

            'Modified by Alkesh Kumar on 10/05/2010 12:37:37 refer developer guide no. 
            'Dim lSelectedTab As Double
            Dim lSelectedTab As Integer

            'Modified by Alkesh Kumar on 10/05/2010 12:37:59 refer developer guide no. 156
            'For lTab As Double = 1 To r_TabStrip.TabPages.Count
            For lTab As Integer = 0 To r_TabStrip.TabPages.Count - 1
                If r_TabStrip.TabPages(lTab).Name = "_" & v_iIndex Then
                    'Store the tab that we were on
                    'Modified by Alkesh Kumar on 10/05/2010 12:38:22 refer developer guide no. 66
                    'lSelectedTab = ContainerHelper.GetControlIndex(r_TabStrip.SelectedTab)
                    lSelectedTab = r_TabStrip.SelectedIndex
                    'remove tab. This sets focus to first tab!
                    'Modified by Alkesh Kumar on 10/05/2010 12:39:26 refer developer guide no. 156
                    'r_TabStrip.TabPages.RemoveAt(lTab - 1)
                    r_TabStrip.TabPages.RemoveAt(lTab)



                    If lSelectedTab = lTab Then
                        'if we removed the tab we are on, set focus to first tab
                        'Modified by Alkesh Kumar on 10/05/2010 12:39:45 refer developer guide no. 156
                        'r_TabStrip.SelectTab(1)
                        r_TabStrip.SelectTab(0)
#If SwiftControls = 1 Then

						If Not IsMissing(r_uFlatMenuTree) Then
						r_uFlatMenuTree.CurrentSelectionKey = r_TabStrip.Tabs(1).Key
						End If
#End If
                    ElseIf lTab < lSelectedTab Then
                        'if we removed a lower tab, current tab has moved down so re-focus on it
                        r_TabStrip.SelectTab(lSelectedTab - 1)
                    Else
                        'removed higher tab so just reset focus
                        'this needs to be done to ensure correct frames are displayed.
                        r_TabStrip.SelectTab(lSelectedTab)
                    End If
                    Return True
                End If
            Next

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".HideTab")

            Return result

        Catch



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".HideTab")

            Return result
        End Try

    End Function


    ' ***************************************************************** '
    '
    ' Name: SelectTab
    '
    ' Description: Allows TabStrip to be used instead of the old SSTab control
    '              If a tab cannot be found the first tab is set
    '
    ' History: 08/07/2002 CLG - Created.
    '          12/09/2003 CLG CQ2001 Wrong Tab Displayed On Opening Risk
    '
    ' ***************************************************************** '
    Public Sub SelectTab(ByRef r_TabStrip As TabControl, ByVal v_iIndex As Integer, Optional ByRef UFlatMenuTree1 As Object = Nothing, Optional ByRef r_swfCaption As Object = Nothing, Optional ByRef r_lblCaption As Object = Nothing, Optional ByRef r_cmdNavigation As Object = Nothing)

        Dim i As String = ""

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".SelectTab")

        Try



#If SwiftControls = 1 Then

			If Not IsMissing(r_cmdNavigation) Then
			r_cmdNavigation(0).Visible = False
			r_cmdNavigation(1).Visible = False
			End If
#End If
            'Modified by Alkesh Kumar on 10/05/2010 12:40:31 refer developer guide no. 156
            'For lTab As Double = 1 To r_TabStrip.TabPages.Count
            For lTab As Integer = 0 To r_TabStrip.TabPages.Count - 1

                'Modified by Alkesh Kumar on 10/05/2010 12:40:58 refer developer guide no. 159
                If CInt(r_TabStrip.TabPages(lTab).Name.Substring(r_TabStrip.TabPages(lTab).Name.Length - (Strings.Len(r_TabStrip.TabPages(lTab).Name) - 1))) = v_iIndex Then
                    'If CInt(r_TabStrip.TabPages(lTab).Name.Substring(r_TabStrip.TabPages(lTab).Name.Length - 1)) = v_iIndex Then

#If SwiftControls = 1 Then

					If Not IsMissing(r_cmdNavigation) Then
					'If lTab > 1 Then r_cmdNavigation(0).Visible = True
					'If lTab < r_TabStrip.Tabs.Count Then r_cmdNavigation(1).Visible = True
					End If
#End If
                    r_TabStrip.SelectTab(lTab)
#If SwiftControls = 1 Then

					If Not IsMissing(UFlatMenuTree1) Then
					SelectMenuTreeItem UFlatMenuTree1, r_swfCaption, r_TabStrip.Tabs(lTab).Key, r_TabStrip.Tabs(lTab).Caption, r_lblCaption
					End If
#End If
                    Exit Sub
                End If
            Next

            'if can't display requested tab, display first visible tab
            'Modified by Alkesh Kumar on 10/05/2010 12:42:28 refer developer guide no. 156
            'r_TabStrip.SelectTab(1)
            r_TabStrip.SelectTab(0)
#If SwiftControls = 1 Then

			If Not IsMissing(UFlatMenuTree1) Then
			'r_cmdNavigation(1).Visible = True
			SelectMenuTreeItem UFlatMenuTree1, r_swfCaption, r_TabStrip.Tabs(1).Key, r_TabStrip.Tabs(1).Caption, r_lblCaption
			End If
#End If



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".SelectTab")

        Catch



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".SelectTab")
        End Try



    End Sub

    ' ***************************************************************** '
    '
    ' Name: IsTabVisible
    '
    ' Description:
    '
    ' History: 08/07/2002 CLG - Created.
    '
    ' ***************************************************************** '
    Public Function IsTabVisible(ByRef r_TabStrip As TabControl, ByVal v_iIndex As Integer) As Boolean

        ' Debug message
        Dim result As Boolean = False
        Dim tabName As String = ""
        'Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".IsTabVisible")

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            'Modified by Alkesh Kumar on 10/05/2010 12:44:42 refer developer guide no. 156
            'For lTab As Double = 1 To r_TabStrip.TabPages.Count
            For lTab As Integer = 0 To r_TabStrip.TabPages.Count - 1
                'Modified by Alkesh Kumar on 10/05/2010 12:45:00 refer developer guide no. 159
                'If CInt(r_TabStrip.TabPages(lTab).Name.Substring(r_TabStrip.TabPages(lTab).Name.Length - (Strings.Len(r_TabStrip.TabPages(lTab).Name) - 1))) = v_iIndex Then
                tabName = r_TabStrip.TabPages(lTab).Name
                If Not String.IsNullOrEmpty(tabName) Then
                    tabName = tabName.Replace("_", "")
                    If CInt(tabName) = v_iIndex Then
                        Return True
                    End If
                End If

            Next


            ' Debug message
            'Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".IsTabVisible")

            Return result

        Catch



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".IsTabVisible")


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: TabSetCaption
    '
    ' Description:
    '
    ' History: 09/07/2002 CLG - Created.
    '
    ' ***************************************************************** '
    Public Sub TabSetCaption(ByRef r_TabStrip As TabControl, ByVal v_iIndex As Integer, ByRef r_vValue As String)

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".TabSetCaption")

        Try


            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".TabSetCaption")

            For lTab As Integer = 0 To r_TabStrip.TabPages.Count - 1
                If CInt(r_TabStrip.TabPages(lTab).Name.Substring(1)) = v_iIndex Then
                    If r_TabStrip.TabPages(lTab).Text <> r_vValue Then
                        r_TabStrip.TabPages(lTab).Text = r_vValue
                    End If
                    Exit Sub
                End If
            Next

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".TabSetCaption")


            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="TabSetCaption Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TabSetCaption", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
	' ***************************************************************** '
	'
	' Name: TabGetCaption
	'
	' Description:
	'
	' History: 09/07/2002 CLG - Created.
	'
	' ***************************************************************** '
	Public Function TabGetCaption(ByRef r_TabStrip As TabControl, ByVal v_iIndex As Integer) As String
		
		' Debug message
		Dim result As String = String.Empty
		Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".TabGetCaption")
		
		Try 
			
			result = ""
			
          For lTab As Integer = 0 To r_TabStrip.TabPages.Count - 1
                If CInt(r_TabStrip.TabPages(lTab).Name.Substring(1)) = v_iIndex Then
                    Return r_TabStrip.TabPages(lTab).Text
                End If
            Next
			
			
			' Debug message
			Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".TabGetCaption")
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Debug message
			Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".TabGetCaption")
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="TabGetCaption Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TabGetCaption", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	'
	' Name: GetCurrentTab
	'
	' Description:
	'
	' History: 30/06/2003 CLG - Created.
	'
	' ***************************************************************** '
	Public Function GetCurrentTab(ByRef r_TabStrip As TabControl) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			
			For result = 1 To r_TabStrip.TabPages.Count
				If r_TabStrip.TabPages(result).Focused Then
                    'Modified by Alkesh Kumar on 03/25/2010 7:17:13 PM refer developer guide no. 159
                    'Return CInt(r_TabStrip.TabPages(result).Name.Substring(r_TabStrip.TabPages(result).Name.Length - (Strings.Len(r_TabStrip.TabPages(result).Name) - 1))) + 1
                    Return CInt(r_TabStrip.TabPages(result).Name.Substring(r_TabStrip.TabPages(result).Name.Length - 1)) + 1
				End If
			Next 
			
			
			Return -1
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCurrentTab Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCurrentTab", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
End Module
