Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports VB6 = Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Globalization
Imports System.Text
Imports System.Windows.Forms
<System.Runtime.InteropServices.ProgId("PMBGeneralFunc_NET.PMBGeneralFunc")> _
 Public Module PMBGeneralFunc
	
	Private Const ACClass As String = "PMBGeneralFunc"
	
	' New Polaris Data Types
	Public Const GEMPolUnknown As Integer = 0
	Public Const GEMPolDate As Integer = 1
	Public Const GEMPolNumeric As Integer = 2
	Public Const GEMPolShortList As Integer = 3
	Public Const GEMPolLongList As Integer = 4
	Public Const GEMPolText As Integer = 5
	Public Const GEMPolNumeric2 As Integer = 6
	Public Const GEMPolRef As Integer = 9
	' Constants for FillCombo function in GeneralScreens
	Public Const GEMRefill As Boolean = True
	Public Const GEMNoRefill As Boolean = False
	
	'sj 13/11/2002 - start
	'ISS1260
	Public Const SIRShortDate As String = "DD/MM/YYYY"
	Public Const SIRLongDate As String = "D MMMM YYYY"
	Public Const CBX_SHOWDROPDOWN As Integer = &H14Fs
	'sj 13/11/2002 - end
	
	' Gemini Date Formats
	'in GII Constants
	'Global Const SIRShortDate = "DD/MM/YYYY"
	'Global Const SIRLongDate = "D MMMM YYYY"
	
	Public g_oComponentManager As Object
	Public g_oListManager As Object
	Public g_lPolicyKey As Integer
	Public g_bInstanceChanged As Boolean
	Public g_lEdit As Integer
	'
	'Private m_lReturn As Long
	
	'in GII Functions
	'sj 13/11/2002 - start
	'ISS1260
	Public Enum CtrType
		ctrNone
		ctrLabel
		ctrTextBox
		ctrComboBox
		ctrCheckbox
		ctrYesNoCheck
		ctrCommand
	End Enum
	
	Private Declare Function SendMessage Lib "user32"  Alias "SendMessageA"(ByVal hwnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer, ByRef lParam As Integer) As Integer
	'sj 13/11/2002 - end
	
	'******************************************************************************
	' Function: DoubleCharacter
	'
	' Description: Loops through a string and doubles each instance of
	'              the passed character. Defaults to ' if no character is
	'              passed.
	'
	' Note: Modified from the "Apostrophes" function.
	'
	'******************************************************************************
	Public Function DoubleCharacter(ByRef r_sString As String, Optional ByVal v_sChar As String = "") As Integer
		
		Dim result As Integer = 0
		Dim i As Integer
		Dim sTemp As New StringBuilder
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If r_sString.Length = 0 Then
				Return result
			End If
			
			' Default to apostrophe
			If Not True Then
				v_sChar = "'"
			End If
			
			sTemp = New StringBuilder("")
			
			Do While True
				i = (r_sString.IndexOf(v_sChar) + 1)
				
				If i = 0 Then
					sTemp.Append(r_sString)
					Exit Do
				End If
				
				sTemp.Append(r_sString.Substring(0, i - 1) & v_sChar & v_sChar)
				r_sString = r_sString.Substring(i)
			Loop 
			
			r_sString = sTemp.ToString()
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to double character " & v_sChar & " in string " & r_sString, vApp:=ACApp, vClass:=ACClass, vMethod:="DoubleCharacter", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	
	Public Function ControlGotFocus(ByRef ctl As Control, Optional ByRef vReferenceType As Integer = 0) As Integer
		
		Dim result As Integer = 0
		Dim lControlType As CtrType
		Dim iPolarisType As Integer
		Dim lReturn As Integer
		Dim iRefType As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			

			If Information.IsNothing(vReferenceType) Then
				iRefType = 0
			Else
				iRefType = vReferenceType
			End If
			
			' Get the control type
			lControlType = ControlType(ctl)
			
			' Get the Polaris type from the tag

            'If Strings.Len(Convert.ToString(ControlHelper.GetTag(ctl))) > 0 Then
            If Convert.ToString(ControlHelper.GetTag(ctl)).Length > 0 Then
                ' Check if from PolVer screens as well, as they are not POLARIS
                If Convert.ToString(ControlHelper.GetTag(ctl)).StartsWith("DIARX") Then
                    iPolarisType = 1 'date
                Else
                    iPolarisType = Conversion.Val(Convert.ToString(ControlHelper.GetTag(ctl)).Substring(0, 1))
                End If
            Else
                iPolarisType = 0
            End If


            With ctl


                Select Case lControlType
                    Case CtrType.ctrTextBox


                        Select Case iPolarisType
                            Case GEMPolDate
                                ' Convert date to short format on GotFocus

                                If Information.IsDate(.Text) Then

                                    .Text = StringsHelper.Format(.Text, SIRShortDate)
                                Else
                                    .Text = "DD/MM/YYYY"
                                End If

                        End Select

                        ' Highlight the contents of the control

                        'Modified by Deepak Sharma on 5/28/2010 4:54:46 PM refer developer guide no. 47
                        '.SelStart = 0
                        ReflectionHelper.SetMember(ctl, "SelectionStart", 0)


                        'Modified by Deepak Sharma on 5/28/2010 4:55:33 PM refer developer guide no. 47
                        '.SelLength = Strings.Len(.Text)
                        ReflectionHelper.SetMember(ctl, "SelectionLength", Strings.Len(.Text))


                    Case CtrType.ctrComboBox


                        'Select Case Conversion.Val(Convert.ToString(ControlHelper.GetTag(ctl)).Substring(0, 1))
                        Dim m_sTag = String.Empty
                        If String.IsNullOrEmpty(Convert.ToString(ControlHelper.GetTag(ctl))) Then
                            m_sTag = Conversion.Val(Convert.ToString(ControlHelper.GetTag(ctl)))
                        Else
                            m_sTag = Conversion.Val(Convert.ToString(ControlHelper.GetTag(ctl)).Substring(0, 1))
                        End If
                        Select Case m_sTag
                            Case GEMPolLongList

                                lReturn = FillCombo(ctl, GEMRefill)

                            Case GEMPolShortList

                                lReturn = FillCombo(ctl, GEMNoRefill)

                            Case GEMPolRef

                                lReturn = FillRefCombo(ctl, iRefType)

                            Case Else

                        End Select

                        ' Highlight the contents of the control

                        'Modified by Deepak Sharma on 5/28/2010 4:57:06 PM refer developer guide no. 
                        'If ReflectionHelper.GetMember(ctl, "style") = ComboBoxStyle.DropDown Then
                        If ReflectionHelper.GetMember(ctl, "DropDownStyle") = ComboBoxStyle.DropDown Then

                            'Modified by Deepak Sharma on 5/28/2010 4:57:37 PM refer developer guide no. 47
                            'ReflectionHelper.SetMember(ctl, "SelStart", 0)
                            ReflectionHelper.SetMember(ctl, "SelectionStart", 0)


                            'Modified by Deepak Sharma on 5/28/2010 4:57:26 PM refer developer guide no. 47
                            'ReflectionHelper.SetMember(ctl, "SelLength", Strings.Len(.Text))
                            ReflectionHelper.SetMember(ctl, "SelectionLength", Strings.Len(.Text))
                        End If


                    Case CtrType.ctrCheckbox, CtrType.ctrYesNoCheck, CtrType.ctrCommand

                End Select

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error in ControlGotFocus [" & ctl.Name & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="ControlGotFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)


            Return result
        End Try
	End Function
	
	Public Function ControlLostFocus(ByRef ctl As Control) As Integer
		
		Dim result As Integer = 0
		Dim lControlType As CtrType
		Dim iPolarisType As Integer
		Dim bFound As Boolean
		Dim sText As String = ""
		Dim lReturn As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			
			' Get the control type
			lControlType = ControlType(ctl)
			
			' Get the Polaris type from the tag
			If Strings.Len(Convert.ToString(ControlHelper.GetTag(ctl))) > 0 Then
				' Check if from PolVer screens as well, as they are not POLARIS
				If Convert.ToString(ControlHelper.GetTag(ctl)).StartsWith("DIARX") Then
					iPolarisType = 1 'date
				Else
					iPolarisType = Conversion.Val(Convert.ToString(ControlHelper.GetTag(ctl)).Substring(0, 1))
				End If
			Else
				iPolarisType = 0
			End If
			
			
			With ctl
				
				
				Select Case lControlType
					Case CtrType.ctrTextBox
						
						
						Select Case iPolarisType
							Case GEMPolDate
								' Assume digits only is a year

								Dim dbNumericTemp As Double
								If Double.TryParse(.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

									.Text = "01/01/" & .Text
								End If
								
								' Convert date to long format on LostFocus

								If Information.IsDate(.Text) Then

									.Text = StringsHelper.Format(.Text, SIRLongDate)
								Else
									.Text = ""
									result = gPMConstants.PMEReturnCode.PMFalse
								End If
								
						End Select
						
					Case CtrType.ctrComboBox
						
						' For Combo Boxes, check if the entered item is in the list,
						' Otherwise clear the box.
						
						' If it's a long list, fill it first
                        'Modified by Deepak Sharma on 5/28/2010 11:49:38 AM refer developer guide no. 56
                        If Not String.IsNullOrEmpty(ControlHelper.GetTag(ctl)) Then
						If Conversion.Val(Convert.ToString(ControlHelper.GetTag(ctl)).Substring(0, 1)) = GEMPolLongList Then
							lReturn = FillCombo(ctl, GEMRefill)
						End If
                        End If
						
						bFound = False
						
						' Store the current value

						sText = ctl.Text.ToUpper()
                        'Added by Deepak Sharma on 5/28/2010 1:11:09 PM refer developer guide no. 56
                        If Not String.IsNullOrEmpty(sText) Then

                            'Modified by Deepak Sharma on 5/28/2010 12:46:25 PM refer developer guide no. 
                            'For lCount As Integer = 0 To CInt(ReflectionHelper.GetMember(ctl, "ListCount") - 1)
                            For lCount As Integer = 0 To CInt(ReflectionHelper.GetMember(ReflectionHelper.GetMember(ctl, "Items"), "Count") - 1)
							
							' See if the value matches the list item (Not case-sensitive)

                                'Modified by Deepak Sharma on 5/28/2010 12:46:27 PM refer developer guide no. 
                                'If ReflectionHelper.Invoke(ctl, "List", New Object() {lCount}).ToUpper() = sText Then
                                If ReflectionHelper.GetMember(ctl, "Items")(lCount).ToString.ToUpper = sText Then
								
								' If so, set the value to the list item

                                    'Modified by Deepak Sharma on 5/28/2010 12:53:52 PM refer developer guide no. 
                                    'ReflectionHelper.SetMember(ctl, "ListIndex", lCount)
                                    ReflectionHelper.SetMember(ctl, "SelectedIndex", lCount)
								bFound = True
								
							End If
							
						Next 
                        End If
						If Not bFound Then
							'Kevin Renshaw (CMG) - Text property is read only for ComboDropdownList

                            'Modified by Deepak Sharma on 5/28/2010 12:54:02 PM refer developer guide no. 
                            'If ReflectionHelper.GetMember(ctl, "style") <> ComboBoxStyle.DropDownList Then
                            If ReflectionHelper.GetMember(ctl, "DropDownStyle") <> ComboBoxStyle.DropDownList Then
								ctl.Text = ""
							End If
						End If
						
						
					Case CtrType.ctrCheckbox, CtrType.ctrYesNoCheck, CtrType.ctrCommand
						
				End Select
				
			End With
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error in ControlLostFocus [" & ctl.Name & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="ControlLostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			
			Return result
		End Try
	End Function
	
	' Fills combo box with polaris list via ComponentManager
    Public Function FillCombo(ByRef cboControl As ComboBox, ByRef bRefill As Boolean) As Integer
        Dim result As Integer = 0
        Dim vListArray() As Object = Nothing
        Dim lPropertyID As Integer
        Dim iPropertyType As Integer
        Dim sTableName As String = ""
        Dim sFieldName As String = ""
        Dim sMatchString As String = ""
        Dim lNumItems As Integer

        Dim lReturn As Integer
        Dim sText As String = ""

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            sMatchString = ""

            With cboControl

                ' Check tag
                If Strings.Len(Convert.ToString(cboControl.Tag)) < 10 Then
                    Return result
                End If

                ' Save text
                sText = .Text


                ' If it's not a refill, it only needs filling once
                If (bRefill = GEMNoRefill) And (.Items.Count > 0) Then
                    ' Return successful
                    Return result
                End If

                ' If it's a refill, then only return matching items
                If bRefill = GEMRefill Then
                    sMatchString = .Text
                End If

                ' Split the tag into its component parts
                lReturn = ParseTag(Convert.ToString(.Tag), iPropertyType, lPropertyID, sTableName, sFieldName)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'sj 15/02/99 - start

                ' Get the List from the list manager
                If sMatchString <> "" Then


                    lReturn = g_oListManager.GetList(v_sPropertyId:=CStr(lPropertyID), r_vListData:=vListArray, v_vSearchString:=sMatchString)
                Else


                    lReturn = g_oListManager.GetList(v_sPropertyId:=CStr(lPropertyID), r_vListData:=vListArray)

                End If

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get list from List Manager", vApp:=ACApp, vClass:=ACClass, vMethod:="FillCombo", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result
                End If

                ' Put the list into the Array

                lNumItems = vListArray.GetUpperBound(0)
                If Information.IsArray(vListArray) Then
                    .Items.Clear()
                    .Items.Add(" ")

                    For lItem As Integer = 0 To lNumItems

                        .Items.Add(CStr(vListArray(lItem)).Trim())

                    Next
                End If

                'sj 15/02/99 - end

                ' Restore text
                If .DropDownStyle = ComboBoxStyle.DropDown Then
                    .Text = sText
                End If

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FillCombo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FillCombo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    '
    ' Fills combo box with polaris list via ComponentManager
    '
    Public Function FillRefCombo(ByRef cboControl As ComboBox, ByRef iRefType As Integer) As Integer

        Dim result As Integer = 0
        Dim vData As Object = Nothing
        Dim lReturn, lCnt As Integer
        Dim sText As String = ""
        Dim lIndex, lRef As Integer
        Dim bSaveFlag As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If we've already filled the combo then
            ' don't bother
            If cboControl.Items.Count > 1 Then
                Return result
            End If

            If cboControl.Items.Count > 0 Then
                sText = cboControl.Text
                lRef = VB6.GetItemData(cboControl, 0)
            Else
                lRef = -1
            End If
            ' If it's not a refill, it only needs filling once
            '    If (cboControl.ListCount > 0) Then
            '        ' Return successful
            '        Exit Function
            '    End If


            lReturn = g_oComponentManager.GetRef(g_lPolicyKey, iRefType, vData)

            cboControl.Items.Clear()

            cboControl.Items.Add("")
            VB6.SetItemData(cboControl, lCnt, 0)

            lIndex = 0

            If Information.IsArray(vData) Then

                For lCnt = 0 To vData.GetUpperBound(1)

                    ' Find the corresponding ItemData Ref

                    If Conversion.Val(CStr(vData(0, lCnt))) = lRef Then
                        lIndex = lCnt + 1
                    End If

                    cboControl.Items.Add(CStr(vData(1, lCnt)).Trim())

                    VB6.SetItemData(cboControl, lCnt + 1, Conversion.Val(CStr(vData(0, lCnt))))

                Next lCnt
            End If

            'cboControl.Text = ""

            ' Save the value of the gloabal flag
            ' because it's going to get set here
            bSaveFlag = g_bInstanceChanged

            ' Now, set the correct item in the list
            cboControl.SelectedIndex = lIndex

            ' Reset the flag status

            SetChangeFlag(cboControl.FindForm(), bSaveFlag)


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FillRefCombo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FillRefCombo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
	
	Public Function FieldChange(ByRef frm As Form, Optional ByRef vKeyCode As Integer = 0, Optional ByRef vShift As Integer = 0) As Integer
		
		Dim iKeyCode, iShift As Integer
		
		Try 
			
			iKeyCode = 0
			iShift = 0
			

			If Not Information.IsNothing(vKeyCode) Then
				iKeyCode = vKeyCode
			End If
			

			If Not Information.IsNothing(vShift) Then
				iShift = vShift
			End If
			
			' Ignore Alt-C (Cancel Button)
			' Ignore Alt-O (OK Button)
			' Ignore Alt-A (Apply Button)
            If (iShift And VB6.ShiftConstants.AltMask) = 0 Then

                ' Set flag to say that a change has been made to this instance
                SetChangeFlag(frm, True)

            End If
		
		Catch excep As System.Exception
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FieldOnControlChange Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FieldOnControlChange", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
		End Try
		
	End Function
	
    'Modified by Deepak Sharma on 4/20/2010 4:03:02 PM refer developer guide no. 101(Guide)
    'Public Function FieldOnControlChange(ByRef ctl As frmCreateAccount, Optional ByRef vKeyCode As Integer = 0, Optional ByRef vShift As Integer = 0) As Integer
    Public Function FieldOnControlChange(ByRef ctl As Object, Optional ByRef vKeyCode As Integer = 0, Optional ByRef vShift As Integer = 0) As Integer
		
		Dim result As Integer = 0
		result = gPMConstants.PMEReturnCode.PMTrue
		Dim iKeyCode As Integer = 0
		Dim iShift As Integer = 0
		

		If Not Information.IsNothing(vKeyCode) Then
			iKeyCode = vKeyCode
		End If
		

		If Not Information.IsNothing(vShift) Then
			iShift = vShift
		End If
		
		' Ignore Alt-C (Cancel Button)
		' Ignore Alt-O (OK Button)
		' Ignore Alt-A (Apply Button)
        If (iShift And VB6.ShiftConstants.AltMask) = 0 Then

            ' Set flag to say that a change has been made to this instance
            SetControlChangeFlag(ctl, True)

        End If
		
		Return result
		
		
		result = gPMConstants.PMEReturnCode.PMFalse
		' Log Error.
		iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FieldOnControlChange Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FieldOnControlChange", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
		
		Return result
	End Function
	'
	' Returns as string containing the Name of the control type
	'
	Public Function ControlType(ByRef ctl As Control) As Integer
		
		Try 
			
			If TypeOf ctl Is Label Then
				Return CtrType.ctrLabel
			End If
			
			If TypeOf ctl Is TextBox Then
				Return CtrType.ctrTextBox
			End If
			
			If TypeOf ctl Is ComboBox Then
				Return CtrType.ctrComboBox
			End If
			
			If TypeOf ctl Is CheckBox Then
				Return CtrType.ctrCheckbox
			End If
			
			'    If (TypeOf ctl Is GEMControlLib.YesNoCheck) Then
			'        ControlType& = ctrYesNoCheck
			'        Exit Function
			'    End If
			
			If TypeOf ctl Is Button Then
				Return CtrType.ctrCommand
			End If
			
			' CF 020699 - Commented out because we don't use grids
			' anymore.
			'    If (TypeOf ctl Is TDBGrid) Then
			'        ControlType& = ctltdbgrid
			'        Exit Function
			'    End If
			
			
			Return CtrType.ctrNone
		
		Catch excep As System.Exception
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ControlType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ControlType", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			
		End Try
	End Function
	' **************************************************************
	' Name : ParseTag
	'
	' Description : Splits a screen control tag into the Polaris
	' Property Type and Property ID and the Database Table and
	' and Field Names.
	'
	' **************************************************************
	Public Function ParseTag(ByRef sTag As String, ByRef iPropertyType As Integer, ByRef lPropertyID As Integer, ByRef sTable As String, ByRef sField As String) As Integer
		
		Dim result As Integer = 0
        Dim sTmpPropertyType As String = ""
        Dim sTmpPropertyID As String = ""
        Dim sTmpTable As String = ""
        Dim sTmpField As String = ""
        Dim sTmpTag As String = ""
		Dim iChar As Integer
		Dim sChar As String = ""
		Dim bComma As Boolean
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Initialise variables
			sTmpPropertyType = ""
			sTmpPropertyID = ""
			sTmpTable = ""
			sTmpField = ""
			
			' Get a copy of the tag to play with
			sTmpTag = sTag
			
			' Property type is the first character
			sTmpPropertyType = sTag.Substring(0, 1)
			
			' Property ID is the next 8 characters
			sTmpPropertyID = sTmpTag.Substring(1, Math.Min(sTmpTag.Length, 8)).Trim()
			
			' Trim off the first 9 characters
			sTmpTag = sTmpTag.Substring(sTmpTag.Length - (sTmpTag.Length - 9))
			
			bComma = False
			
			iChar = (sTmpTag.IndexOf(","c) + 1)
			
			If iChar = 0 Then
				sTmpTable = sTmpTag
			Else
				sTmpTable = sTmpTag.Substring(0, iChar - 1)
				sTmpField = sTmpTag.Substring(iChar)
			End If
			
			
			' Set return Values
			iPropertyType = Conversion.Val(sTmpPropertyType)
			lPropertyID = CInt(Conversion.Val(sTmpPropertyID))
			sTable = sTmpTable.Trim()
			sField = sTmpField.Trim()
			
			Return result
		
		Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Invalid Tag : " & sTmpTag, vApp:=ACApp, vClass:=ACClass, vMethod:="ParseTag", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
		End Try
	End Function
	
	Public Sub SetChangeFlag(ByRef oForm As Form, ByRef bValue As Boolean)
		
		Try 
			
			g_bInstanceChanged = bValue
			'oForm.cmdOK.Enabled = bValue
			
			If g_lEdit = gPMConstants.PMEReturnCode.PMTrue Then

                'Replaced with Migrated code 114
                'oForm.cmdApply.Enabled = bValue
                ReflectionHelper.SetMember(ReflectionHelper.GetMember(oForm, "cmdApply"), "Enabled", bValue)
			End If
		
		Catch excep As System.Exception
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetChangeFlag.", vApp:=ACApp, vClass:=ACClass, vMethod:="SetChangeFlag", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
    'Modified by Deepak Sharma on 4/20/2010 4:13:02 PM refer developer guide no. 101(Guide)
    'Public Sub SetControlChangeFlag(ByRef oCtl As frmCreateAccount, ByRef bValue As Boolean)
    Public Sub SetControlChangeFlag(ByRef oCtl As Object, ByRef bValue As Boolean)
		
		g_bInstanceChanged = bValue
		'oForm.cmdOK.Enabled = bValue
		
		'We have no apply button, especially not on the control
		'    If (g_lEdit& = PMTrue) Then
		'        oForm.cmdApply.Enabled = bValue
		'    End If
		
	End Sub
	
	' ************************************************************************ '
	' Name: FormatPostCode
	'
	' Description: Formats the post code passed in.
	'              Example...   A12 3BC     -> A12 3BC
	'                           A 12 3BC    -> A12 3BC
	'                           A 1 2 3BC   -> A12 3BC
	'               Basically it removes all spaces, except for the last one.
	'
	' CTAF 200300 - Moved from frmInterface of iPMBAddress into PMBGeneralFunc.
	'
	' ************************************************************************ '
	Public Function FormatPostCode(ByVal v_sInString As String, ByRef r_sOutString As String) As Integer
		
		Dim result As Integer = 0
		Dim iSpaceCounter, iCurrentSpaces As Integer
        Dim sChar As New VB6.FixedLengthString(1)
        'Added by Deepak Sharma on 4/20/2010 5:26:07 PM refer developer guide no. 128(Guide)

        Dim iLoop1 As Integer

		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			iSpaceCounter = 0
			iCurrentSpaces = 0
			
			r_sOutString = ""
			
			' find out how many spaces are in the "in" string
            For iLoop1 = 1 To v_sInString.Length
				If v_sInString.Substring(iLoop1 - 1, 1) = " " Then
					iSpaceCounter += 1
				End If
			Next iLoop1
			
			' if theres just one space, then that's ok
			If iSpaceCounter = 1 Then
				r_sOutString = v_sInString
				Return result
			End If
            'Modified by Deepak Sharma on 4/20/2010 5:26:32 PM refer developer guide no. 128(Guide)
            ' loop through the in string and
            'For Each sChar.Value In v_sInString
            '    ' if its a space, then check if
            '    If sChar.Value = " " Then
            '        iCurrentSpaces += 1
            '        ' we arent at the last space
            '        If iCurrentSpaces >= iSpaceCounter Then
            '            ' if we are, then its ok to add this space
            '            r_sOutString = r_sOutString & sChar.Value
            '        End If
            '    Else
            '        ' not a space so just add it
            '        r_sOutString = r_sOutString & sChar.Value
            '    End If
            'Next sChar.Value
            For iLoop1 = 1 To Len(v_sInString)
                sChar.Value = Mid(v_sInString, iLoop1, 1)
                ' if its a space, then check if
                If (sChar.Value = " ") Then
                    iCurrentSpaces = iCurrentSpaces + 1
                    ' we arent at the last space
                    If (iCurrentSpaces >= iSpaceCounter) Then
                        ' if we are, then its ok to add this space
                        r_sOutString = r_sOutString & sChar.Value
                    End If
                Else
                    ' not a space so just add it
                    r_sOutString = r_sOutString & sChar.Value
                End If
            Next iLoop1
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to format the Postcode : " & v_sInString, vApp:=ACApp, vClass:=ACClass, vMethod:="FormatPostCode", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	'
	' Name: CheckValidPostCode
	'
	' Description: Checks to see if a postcode is in one of the following
	'              formats :
	'                           X9 9XX
	'                           X99 9XX
	'                           XX9 9XX
	'                           XX99 9XX
	'                           XX9X 9XX
	'                           X9X 9XX
	'
	' History: 20/03/2000 CTAF - Created.
	'          17/10/2000 CTAF - Added X9X 9XX
	'
	' Notes: This is different to ValidatePostcodeFormat in GIIFunc.bas
	'        as it takes a string input.
	'
	' ***************************************************************** '
	Public Function CheckValidPostCode(ByVal v_sPostCode As String, Optional ByVal v_bSpaceRequired As Boolean = False) As Integer
		
		Dim result As Integer = 0
		Dim iLen As Integer
		Dim sPostCode As String = ""
        Dim sChar As New VB6.FixedLengthString(1)
		Dim sNewPostCode As New StringBuilder
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Store the post code, so we can modify it later on
			sPostCode = v_sPostCode
			
			' Check for a minimum length
			iLen = sPostCode.Length
			
			If iLen < 5 Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			If v_bSpaceRequired Then
				If (sPostCode.IndexOf(" "c) + 1) = 0 Then
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
			End If
			
			sNewPostCode = New StringBuilder("")
			
			For iLoop1 As Integer = 1 To iLen
				
				' Grab the next letter
				sChar.Value = sPostCode.Substring(iLoop1 - 1, 1)
				
				If sChar.Value.ToUpper() Like "[A-Z]" Then
					' Convert any letters to X
					sNewPostCode.Append("X")
				ElseIf (sChar.Value Like "[0-9]") Then 
					' Convert any numbers
					sNewPostCode.Append("9")
				ElseIf (sChar.Value = " ") Then 
					' Do nothing
				Else
					' Leave anything else as it is
					sNewPostCode.Append(sChar.Value)
				End If
				
			Next iLoop1
			
			
			Select Case sNewPostCode.ToString()
				Case "X99XX", "X999XX", "XX99XX", "XX999XX", "XX9X9XX", "X9X9XX"
					' Return True
					Return gPMConstants.PMEReturnCode.PMTrue
				Case Else
					' Return False
					Return gPMConstants.PMEReturnCode.PMFalse
			End Select
		
		Catch 
		End Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMError
		
		' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckValidPostCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckValidPostCode", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
		
		Return result
		
	End Function
	
	Public Function CheckValidUSZipCode(ByVal v_sZipCode As String, Optional ByVal v_bSpaceRequired As Boolean = False) As Integer
		
		Dim result As Integer = 0
		Dim iLen As Integer
		Dim sZipCode As String = ""
        Dim sChar As New VB6.FixedLengthString(1)
		Dim sNewZipCode As New StringBuilder
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Store the post code, so we can modify it later on
			sZipCode = v_sZipCode.Trim()
			
			' Check for a minimum length
			iLen = sZipCode.Length
			
			If iLen < 5 Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			sNewZipCode = New StringBuilder("")
			
			For iLoop1 As Integer = 1 To iLen
				
				' Grab the next letter
				sChar.Value = sZipCode.Substring(iLoop1 - 1, 1)
				
				If sChar.Value.ToUpper() Like "[A-Z]" Then
					' US Zip codes cannot contain letters
					Return gPMConstants.PMEReturnCode.PMFalse
				ElseIf (sChar.Value Like "[0-9]") Then 
					' Convert any numbers
					sNewZipCode.Append("9")
				ElseIf (sChar.Value = " ") Then 
					' US Zip codes cannot contain spaces
					Return gPMConstants.PMEReturnCode.PMFalse
				ElseIf (sChar.Value = "-") Then 
					' Leave anything else as it is
					sNewZipCode.Append(sChar.Value)
				Else
					' US Zip codes can ONLY contain numbers and (-) for 9 digit zip code
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
			Next iLoop1
			
			
			Select Case sNewZipCode.ToString()
				Case "99999", "99999-9999"
					' Return True
					Return gPMConstants.PMEReturnCode.PMTrue
				Case Else
					' Return False
					Return gPMConstants.PMEReturnCode.PMFalse
			End Select
		
		Catch 
		End Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMError
		
		' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckValidUSZipCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckValidUSZipCode", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
		
		Return result
		
	End Function
	'sj 13/11/2002 - start
	'ISS1260
	'************************************************************************************************
	' Name             :   HighlightControl
	' Created by       :   Ram Chandrabose
	' Date             :   29-Oct-1999
	' Function for     :   Highlight the contents of the control
	' Called from      :   Control's Got_Focus Event
	' Input Parameters :   1.  Ctl              - Control
	'                      2.  optBoolDateField - Boolean  ( Optional Parameter )
	'                               if True     - Set the Control with 'DD/MM/YYYY' as default value.
	' Edit History     :
	'*************************************************************************************************
    Public Function HighlightContol2(ByRef ctl As Object, Optional ByRef optBoolDateField As Boolean = False, Optional ByRef optBoolDropDown As Boolean = False) As Integer

        Dim result As Integer = 0
        Try

            With ctl

                ' For date fields only
                If Not False Then
                    If optBoolDateField Then
                        ' Convert date to short format on GotFocus
                        If Information.IsDate(.Text) Then
                            .Text = StringsHelper.Format(CDate(.Text), SIRShortDate)
                        Else
                            .Text = SIRShortDate
                        End If
                    End If
                End If

                ' Highlight the contents of the control
                .SelStart = 0
                'Modified by Deepak Sharma on 5/28/2010 4:53:18 PM refer developer guide no. 47
                'ReflectionHelper.SetMember(ctl, "SelStart", 0)
                ReflectionHelper.SetMember(ctl, "SelectionStart", 0)
                'Modified by Deepak Sharma on 5/28/2010 4:53:31 PM refer developer guide no. 47
                'ReflectionHelper.SetMember(ctl, "SelLength", Strings.Len(.Text))
                ReflectionHelper.SetMember(ctl, "SelectionLength", Strings.Len(.Text))
            End With

            ' To Make explicit Drop Down using API
            If Not False Then
                If optBoolDropDown Then
                    SendMessage(ctl.hwnd, CBX_SHOWDROPDOWN, True, 0)
                End If
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error in HightlightControl [" & ctl.Name & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="HighlightContol2", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)


            Return result
        End Try
    End Function
	' ***************************************************************** '
	'
	' Name: ValidateListField2
	'
	' Description:
	'
	' History: 03/12/1999 JSB - Created.
	'
	' ***************************************************************** '
    Public Function ValidateListField2(ByRef ddList As Object) As Integer

        Dim result As Integer = 0
        Dim sText As String = ""
        Dim bFound As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bFound = False

            ' hold the current value
            sText = ddList.Text.ToUpper()

            ' Loop through the list
            For lCount As Integer = 0 To CInt(ReflectionHelper.GetMember(ddList, "ListCount") - 1)

                ' See if the value matches the list item

                If ReflectionHelper.Invoke(ddList, "List", New Object() {lCount}).ToUpper() = sText Then
                    ' If so, set the value to the list item

                    ddList.Text = ReflectionHelper.Invoke(ddList, "List", New Object() {lCount})
                    bFound = True
                End If

            Next

            If Not bFound Then
                ddList.Text = ""
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateListField2 Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateListField2", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
	'sj 13/11/2002 - end
End Module