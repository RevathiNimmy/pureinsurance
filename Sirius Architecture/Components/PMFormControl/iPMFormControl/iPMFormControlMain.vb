Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles

Module MainModule
	' BB040298 Added CVar to prevent type mismatch with user controls
	' RAW 30/06/2003 : CQ1465 : added new param to MessageThenFocus
	' VB  14/02/2005 : PN-18426 : PMAccountLookup added for 'AccountLookup' UserControl
	'                             in 'GetControlType' Function.
    ''developer guide no. Replaced iPMFunc.GetResData with GetResData in the whole document 
	
	Public Const ACApp As String = "iPMFormControl"
	Public Const ACClass As String = "MainModule"
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
	
	' Standard messages
	Public Const ACCancelMsg As Integer = 300
	Public Const ACBusinessMsg As Integer = 302
	Public Const ACInvalidDateMsg As Integer = 304
	Public Const ACInvalidDateRangeMsg As Integer = 320
	Public Const ACMandyMsg As Integer = 306
	Public Const ACInvalidTimeMsg As Integer = 310
	Public Const ACInvalidGeneralMsg As Integer = 308
	Public Const ACInvalidNumber As Integer = 312
	Public Const ACInvalidPercentage As Integer = 314
	Public Const ACInvalidYear As Integer = 316
	Public Const ACInvalidCurrency As Integer = 318
	Public Const ACSpread As gPMConstants.PMEControlType = gPMConstants.PMEControlType.PMOptionButton
	
	Public Const ACDateLowValue As Integer = -1
	
	'*******************************************************************
	' GetLabelForControl :Look for a control called lblXXX for a control
	'                     called eg txtXXX
	'*******************************************************************
	Public Function GetLabelForControl(ByRef ctlField As Control) As Control
		
		Dim sLabelName, sFieldName As String
		
		Try
		
		' Try to find a label in the form lblAbc for control xxxAbc
		sFieldName = ctlField.Name
		
		
		If sFieldName.Length > 3 Then
                sLabelName = "lbl" & Mid(sFieldName, 4)

                'developer guide no. 166(Guide)
                Try
                    Return ctlField.Parent.Controls(sLabelName)
                Catch
                    Return Nothing
                End Try
            Else
                Return Nothing
            End If
		
		Catch ex As Exception
		
		
            Return Nothing
		End Try
	End Function
	'*******************************************************************
	' GetCaptionForControl : Try to extract a caption from a label with
	'                        the same base name as this control
	'*******************************************************************
	Public Function GetCaptionForControl(ByRef oField As FormField) As String
		
		Dim i As Integer
		Dim sCaption As String = ""
		Dim ctlLabel As Control
		
		Try 
			
			With oField
				If .ControlType = gPMConstants.PMEControlType.PMGrid Then

					sCaption = .FormControl.Columns(.GridColumn).Caption
				ElseIf .ControlType = gPMConstants.PMEControlType.PMSpread Then 

					.FormControl.Row = 0

					sCaption = .FormControl.Text
				Else
					ctlLabel = GetLabelForControl(.FormControl)
					If ctlLabel Is Nothing Then
						sCaption = ""
					Else
						sCaption = ctlLabel.Text
					End If
				End If
			End With
			
			' Strip out Ampersands (from acccelerator keys)
			i = (sCaption.IndexOf("&"c) + 1)
			Do While i <> 0
				sCaption = sCaption.Substring(0, i - 1) & sCaption.Substring(sCaption.Length - (sCaption.Length - i))
				i = (sCaption.IndexOf("&"c) + 1)
			Loop 
			
			' and strip the standard colon
			i = (sCaption.IndexOf(":"c) + 1)
			Do While i <> 0
				sCaption = sCaption.Substring(0, i - 1) & sCaption.Substring(sCaption.Length - (sCaption.Length - i))
				i = (sCaption.IndexOf("&"c) + 1)
			Loop 
			
			
			Return sCaption
		
		Catch 
			
			
			
			
			Return ""
		End Try
		
	End Function
	'*************************************************************************
	' SetControlVisible : It is possible for a mandatory field to be not
	'                     visible because it's on a tab other than the
	'                     current one. This rather dubious routine tries to make
	'                     the control visible by selecting all the tabs and getting
	'                     the tab click event to make the control visible hopefully
	'                     If you can think of a better way ....
	'*************************************************************************
	Public Function SetControlVisible(ByRef ctlField As Control) As Boolean
		'   Dim ctlTab As Control
		
		Try 
			ControlHelper.SetVisible(ctlField, True)
			ControlHelper.SetEnabled(ctlField, True)
			' did it work ? - then bingo it's on the current tab
			
			'RKS 09/11/2004 PN16268
			'How can we ensure that the control is visible?
			'It will always exit from here as we are setting Visible property of control
			'to True. No need of below written code thus I commented this.
			'   If ctlField.Visible = True Then
			'       SetControlVisible = True
			'       Exit Function
			'   End If
			
            'NIIT - Replaced with the Migrated code 1144 
            If Not (ctlField.Parent Is Nothing) Then
                If TypeOf ctlField.Parent Is Control Then
                    If ctlField.Parent.GetType().Name = "SSTab" Then

                        For i As Integer = 0 To ReflectionHelper.GetMember(ctlField.Parent, "Tabs") - 1

                            If ReflectionHelper.Invoke(ctlField.Parent, "TabVisible", New Object() {i}) Then


                                ReflectionHelper.SetMember(ctlField.Parent, "Tab", i)
                                'Ref Article ID: Q178469
                                'NOTE: When a control contained in an SSTab control is
                                'on an inactive tab, the Left property of the control
                                'is negative.
                                If VB6.PixelsToTwipsX(ctlField.Left) >= 0 Then
                                    'flow comes here only if ctlField is on the active tab
                                    Exit For
                                End If
                            End If
                        Next i
                        'TODO:MILAN
                        'Changes done to handle tabs of type TabControl. The current code handles only SSTab
                    Else
                        If ctlField.Parent.GetType().Name = "TabControl" Then
                            Dim selIndex As Integer = 0
                            Dim reqIndex As Integer = 0
                            selIndex = SSTabHelper.GetSelectedIndex(ctlField.Parent)
                            For Each tb As TabPage In DirectCast(ctlField.Parent, TabControl).TabPages
                                If tb.Name = ctlField.Name Then
                                    DirectCast(ctlField.Parent, TabControl).SelectedTab = tb
                                    Exit For
                                End If
                                reqIndex += 1
                            Next
                        End If
                    End If
                    'Also make the container visible
                    If Not SetControlVisible(ctlField.Parent) Then
                        Return False
                    End If
                End If
            End If



            '    ' no so find a tab control on the form
            '    For Each ctlTab In ctlField.Parent.Controls
            '      If TypeName(ctlTab) = "ISSTabCtl" Then
            '        '  click each tab until the control becomes visible
            '        For i = 0 To ctlTab.Tabs - 1
            '          ctlTab.Tab = i
            '          DoEvents ' click event, do your thing
            '          If ctlField.Visible Then
            '            SetControlVisible = True
            '            Exit Function
            '          End If
            '        Next i
            '      End If
            '    Next ctlTab
            ' failure - no tab or click code did not make it visible
            Return True

        Catch


            Return False
        End Try
		
	End Function
	'**********************************************************
	' Determine the type of control
	'**********************************************************
	Public Function GetControlType(ByRef ctlControl As Control) As Integer
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEControlType.PMUnknownCtlType
			

			
			Select Case ctlControl.GetType().Name
                Case "TextBox", "uctRichTextBox"
                    Return gPMConstants.PMEControlType.PMTextBox
				Case "ComboBox"
					Return gPMConstants.PMEControlType.PMCombo
				Case "CheckBox"
					Return gPMConstants.PMEControlType.PMCheckBox
				Case "ListBox"
					Return gPMConstants.PMEControlType.PMListBox
				Case "OptionButton"
					Return gPMConstants.PMEControlType.PMOptionButton
				Case "TDBGrid", "ITrueDBGridCtrl", "uctPMGridControl"
					Return gPMConstants.PMEControlType.PMGrid
				Case "vaSpread Then"
					Return gPMConstants.PMEControlType.PMSpread
				Case "AccountLookup"
					Return gPMConstants.PMEControlType.PMAccountLookup
				Case Else
					' ??? does this work ??

                    'NIIT - Replaced with the Migrated code 1144 
                    'Return ctlControl.TypeOfControl
                    Return ReflectionHelper.GetMember(ctlControl, "TypeOfControl")

			End Select
		
		Catch 
		End Try
		
		
		Return result
	End Function
	
	'*******************************************************************
	' Message : Display a message from the res file for a particular
	'           control & then set focus to it
	'*******************************************************************
	' RAW 30/06/2003 : CQ1465 : added v_vNewText param
	Public Function MessageThenFocus(ByRef lMessageCode As Integer, ByRef oFormField As FormField, Optional ByVal v_vNewText As String = "") As Integer
		Dim result As Integer = 0
		Dim sMessage, sTitle As String

		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			With oFormField
				

                'sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=lMessageCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager)) & " - " & .Caption
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=lMessageCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager)) & " - " & .Caption

                'sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=lMessageCode + 1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=lMessageCode + 1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
				MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
				
				
				' RAW 30/06/2003 : CQ1465 : added
				' now replace the old value with the new - before setting focus to it

				If Not Information.IsNothing(v_vNewText) Then
					.FormControl.Text = v_vNewText
				End If
				
				
				' Now set focus
				If SetControlVisible(.FormControl) Then
					Select Case .ControlType
						Case gPMConstants.PMEControlType.PMGrid

							.FormControl.Col = .GridColumn
							.FormControl.Focus()
						Case Else
							If .FormControl.Visible And .FormControl.Enabled Then
                                .FormControl.Focus()
							End If
					End Select
				End If
				
			End With
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to MessageThenFocus", vApp:=ACApp, vClass:=ACClass, vMethod:="MessageThenFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	'*********************************************************************
	' SetListIndex : Position the current index of a listbox or combo
	'                by searching it's itemdata for a match with the key
	'*********************************************************************
	Public Function SetListIndex(ByRef cList As Control, ByRef lItemData As Integer) As Integer
		Dim result As Integer = 0
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
            'NIIT - Replaced with the Migrated code 1144 
            ' BB040298 Added CVar to prevent type mismatch with user controls

            'For lIndex As Integer = 0 To CInt(cList.ListCount - 1)

            '	If cList.ItemData(lIndex) = lItemData Then

            '		cList.ListIndex = lIndex

            For lIndex As Integer = 0 To CInt(CInt(ReflectionHelper.GetMember(ReflectionHelper.GetMember(cList, "Items"), "Count")) - 1)

                If VB6.GetItemData(cList, lIndex) = lItemData Then

                    ReflectionHelper.SetMember(cList, "SelectedIndex", lIndex)
                    Exit For
                End If
            Next lIndex

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set combo position", vApp:=ACApp, vClass:=ACClass, vMethod:="SetListIndex", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
	End Function
End Module
