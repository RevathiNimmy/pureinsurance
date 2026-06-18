Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
<System.Runtime.InteropServices.ProgId("iPMForms_NET.iPMForms")> _
 Public Module iPMForms
	
	' Constant for the methods to identify which class this is.
	Private Const ACClass As String = "iPMForms"
    Public g_iLanguageID As Integer
	
    'Modified by milan.rawat on 6/7/2010 10:46:13 AM refer developer guide no. 
    'Modified as per VB Code
    'Public Function GetFieldFormat(ByRef cnt As CheckBox) As Integer
    Public Function GetFieldFormat(ByRef cnt As Control) As Integer
		' ---------------------------------------------------------------------------
		' PROCEDURE NAME: GetFieldFormat
		' PURPOSE: Returns the format of a field based on the format token
		' AUTHOR: Danny Davis
		' DATE: 03/10/2002, 11:31
		' CHANGES:
		' ---------------------------------------------------------------------------
		
		Dim result As Integer = 0
		
		Try
		
		Dim sFormat As String = ""
		
		If TypeOf cnt Is CheckBox Then
			Return gPMConstants.PMEFormatStyle.PMFormatBoolean
		End If
		
		LocateTag(Convert.ToString(cnt.Tag), "FMT;", sFormat)
		
		Select Case sFormat
			Case "$"
				result = gPMConstants.PMEFormatStyle.PMFormatCurrency
			Case "DT"
				result = gPMConstants.PMEFormatStyle.PMFormatDateLong
		End Select
		
		
		'----------------------------------------------------------------------------------------
		'Only for Debugging, the code will never execute this line
		'----------------------------------------------------------------------------------------
		  
		
		Catch ex As Exception
		Select Case Information.Err().Number
			Case Else
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFieldFormat", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
				
		End Select
		
		Finally
	
		
		
		End Try
			Return result
	End Function
	
	Public Function GetFieldType(ByRef lFormat As Integer) As Integer
		' ---------------------------------------------------------------------------
		' PROCEDURE NAME: GetFieldType
		' PURPOSE: Returns the field type based on the format
		' AUTHOR: Danny Davis
		' DATE: 03/10/2002, 11:30
		' CHANGES:
		' ---------------------------------------------------------------------------
		
		Dim result As Integer = 0
		
		Try
		
		Select Case lFormat
			Case gPMConstants.PMEFormatStyle.PMFormatDateLong
				result = gPMConstants.PMEDataType.PMDate
			Case gPMConstants.PMEFormatStyle.PMFormatCurrency
				result = gPMConstants.PMEDataType.PMCurrency
			Case gPMConstants.PMEFormatStyle.PMFormatPercent
				result = gPMConstants.PMEDataType.PMDouble
			Case gPMConstants.PMEFormatStyle.PMFormatBoolean
				result = gPMConstants.PMEDataType.PMBoolean
			Case gPMConstants.PMEFormatStyle.PMFormatDecimal
				result = gPMConstants.PMEDataType.PMLong
		End Select
		
		
		'----------------------------------------------------------------------------------------
		'Only for Debugging, the code will never execute this line
		'----------------------------------------------------------------------------------------
		  
		
		Catch ex As Exception
		Select Case Information.Err().Number
			Case Else
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFieldType", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
				
		End Select
		
		Finally
	
		
		
		End Try
			Return result
	End Function
	
	Public Function GetCaptionID(ByVal v_sTag As String) As Integer
		' ---------------------------------------------------------------------------
		' PROCEDURE NAME: GetCaptionID
		' PURPOSE: Returns the Caption ID for a particular Tag
		' Returns zero if there is no caption.
		' AUTHOR: Danny Davis
		' DATE: 03/10/2002, 11:29
		' CHANGES:
		' ---------------------------------------------------------------------------
		
		Dim result As Integer = 0
		
		Try
		
		Dim sID As String = ""
		
		LocateTag(v_sTag, "CAP;", sID)
		result = CInt(Conversion.Val(sID))
		
		
		'----------------------------------------------------------------------------------------
		'Only for Debugging, the code will never execute this line
		'----------------------------------------------------------------------------------------
		  
		
		Catch ex As Exception
		Select Case Information.Err().Number
			Case Else
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCaptionID", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
				
		End Select
		
		Finally
		
		
		
		End Try
			Return result
	End Function
	
	
	Public Function GetMandatory(ByRef cnt As Control) As Integer
		' ---------------------------------------------------------------------------
		' PROCEDURE NAME: GetMandatory
		' PURPOSE: Returns PMMandatory if the M; tag appears on in the controls tag
		' AUTHOR: Danny Davis
		' DATE: 03/10/2002, 11:30
		' CHANGES:
		' ---------------------------------------------------------------------------
		
		Dim result As Integer = 0
		
		Try
		
		Dim i As Integer
		
		If LocateTag(Convert.ToString(ControlHelper.GetTag(cnt)), "M;") > 0 Then
			result = gPMConstants.PMEMandatoryStatus.PMMandatory
		Else
			result = gPMConstants.PMEMandatoryStatus.PMNonMandatory
		End If
		
		
		
		'----------------------------------------------------------------------------------------
		'Only for Debugging, the code will never execute this line
		'----------------------------------------------------------------------------------------
		  
		
		Catch ex As Exception
		Select Case Information.Err().Number
			Case Else
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMandatory", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
				
		End Select
		
		Finally
	
		
		
		End Try
			Return result
	End Function
	
	Public Function LocateTag(ByVal v_sTag As String, ByVal v_sToken As String, Optional ByRef sValue As String = "") As Integer
		' ---------------------------------------------------------------------------
		' PROCEDURE NAME: LocateTag
		' PURPOSE: Locates as tag on a form with a particular token.
		' Returns the position of the token in the tag property.
		' AUTHOR: Danny Davis
		' DATE: 03/10/2002, 11:27
		' CHANGES:
		' ---------------------------------------------------------------------------
		
		Dim result As Integer = 0
		
		Try
		
		
		Dim lPos, lEndPos As Integer
		
		lPos = (v_sTag.IndexOf(v_sToken) + 1)
		
		result = lPos
		
		If lPos > 0 And lPos + v_sToken.Length < v_sTag.Length Then
			lPos += v_sToken.Length
			lEndPos = Strings.InStr(lPos, v_sTag, ";")
			If lEndPos = 0 Then
				lEndPos = v_sTag.Length
				sValue = Mid(v_sTag, lPos, lEndPos - lPos + 1)
			Else
				sValue = Mid(v_sTag, lPos, lEndPos - lPos) ' - 1)
			End If
		End If
		
		
		'----------------------------------------------------------------------------------------
		'Only for Debugging, the code will never execute this line
		'----------------------------------------------------------------------------------------
		  
		
		Catch ex As Exception
		Select Case Information.Err().Number
			Case Else
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="LocateTag", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
				
		End Select
		
		Finally
	
		
		
		End Try
			Return result
	End Function
	
    Public Function SetFieldValidation(ByRef r_frmSource As Object, ByRef r_oFormfields As Object) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: SetFieldValidation
        ' PURPOSE: Sets the rules for validating fields.
        ' Returns PMTrue / PMError.
        ' AUTHOR: Danny Davis
        ' DATE: 03/10/2002, 11:27
        ' CHANGES: PWC - 03/10/2002 - Made generic
        '          TR27112002 - Changed r_frmSource from frm to object to support
        '                       interfaces. Added explicit ref to default properties
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0


        Try

        If r_oFormfields Is Nothing Then
            r_oFormfields = CreateLateBoundObject("iPMFormControl.FormFields")
            r_oFormfields.LanguageID = g_iLanguageID
        End If

        'Modified by milan.rawat on 6/7/2010 11:27:31 AM refer developer guide no. 224(latest guide)

        'For i As Integer = 0 To r_frmSource.Count - 1

        '	If CStr(r_frmSource.Controls(i).Tag).IndexOf("F;") >= 0 Then

        '
        '		r_oFormfields.AddNewFormField(ctlControl:=r_frmSource.Controls(i), lFormat:=CType(GetFieldFormat(r_frmSource.Controls(i)), gPMConstants.PMEFormatStyle), lMandatory:=CType(GetMandatory(r_frmSource.Controls(i)), gPMConstants.PMEMandatoryStatus))
        '	End If
        'Next i
        'For i As Integer = 0 To ReflectionHelper.GetMember(ReflectionHelper.GetMember(r_frmSource, "Controls"), "Count") - 1

        '    If Not ReflectionHelper.GetMember(r_frmSource.Controls(i), "Tag") Is Nothing Then
        '        If CStr(ReflectionHelper.GetMember(r_frmSource.Controls(i), "Tag")).IndexOf("F;") >= 0 Then
        '            r_oFormfields.AddNewFormField(r_frmSource.Controls(i), CType(GetFieldFormat(r_frmSource.Controls(i)), gPMConstants.PMEFormatStyle), , , CType(GetMandatory(r_frmSource.Controls(i)), gPMConstants.PMEMandatoryStatus))
        '        End If
        '    End If
        'Next i

        'Changes made by Deepak start'
        Dim ctlFormControl As System.Windows.Forms.Control

        'loop through all controls of the form
        For Each ctlFormControl In r_frmSource.Controls
            'check if it is Tab Control
            If (TypeOf ctlFormControl Is System.Windows.Forms.TabControl) Then
                'loop through all the controls within the TabControl
                For Each ctlTabPage As Control In ctlFormControl.Controls
                    'check if it is TabPage
                    If (TypeOf ctlTabPage Is System.Windows.Forms.TabPage) Then
                        'loop through all the controls within the TabPage
                        For Each ctl As Control In ctlTabPage.Controls
                            'check if it is GroupBox
                            If (TypeOf ctl Is GroupBox) Then
                                'loop through all the controls within the GroupBox
                                For Each ctlChild As Control In ctl.Controls
                                    If Not ctlChild.Tag Is Nothing Then
                                        If CStr(ctlChild.Tag).IndexOf("F;") >= 0 Then
                                            r_oFormfields.AddNewFormField(ctlControl:=ctlChild, lFormat:=CType(GetFieldFormat(ctlChild), gPMConstants.PMEFormatStyle), lMandatory:=CType(GetMandatory(ctlChild), gPMConstants.PMEMandatoryStatus))
                                        End If
                                    End If
                                Next
                            Else
                                'If the control is not GroupBox
                                If Not ctl.Tag Is Nothing Then
                                    If CStr(ctl.Tag).IndexOf("F;") >= 0 Then
                                        r_oFormfields.AddNewFormField(ctlControl:=ctl, lFormat:=CType(GetFieldFormat(ctl), gPMConstants.PMEFormatStyle), lMandatory:=CType(GetMandatory(ctl), gPMConstants.PMEMandatoryStatus))
                                    End If
                                End If
                            End If
                        Next
                    Else
                        'No check required as a TabControl can't contain controls other than TabPages                        
                    End If
                Next
            Else
                'Controls outside the TabControl
                'loop through all the controls within the TabPage
                If (TypeOf ctlFormControl Is GroupBox) Then
                    'loop through all the controls within the GroupBox
                    For Each ctlChild As Control In ctlFormControl.Controls
                        If Not ctlChild.Tag Is Nothing Then
                            If CStr(ctlChild.Tag).IndexOf("F;") >= 0 Then
                                r_oFormfields.AddNewFormField(ctlControl:=ctlChild, lFormat:=CType(GetFieldFormat(ctlChild), gPMConstants.PMEFormatStyle), lMandatory:=CType(GetMandatory(ctlChild), gPMConstants.PMEMandatoryStatus))
                            End If
                        End If
                    Next
                Else
                    If Not ctlFormControl.Tag Is Nothing Then
                        If CStr(ctlFormControl.Tag).IndexOf("F;") >= 0 Then
                            r_oFormfields.AddNewFormField(ctlControl:=ctlFormControl, lFormat:=CType(GetFieldFormat(ctlFormControl), gPMConstants.PMEFormatStyle), lMandatory:=CType(GetMandatory(ctlFormControl), gPMConstants.PMEMandatoryStatus))
                        End If
                    End If
                End If

            End If
        Next
        'Changes made by Deepak End'



        result = gPMConstants.PMEReturnCode.PMTrue


        Catch ex As Exception
        Select Case Information.Err().Number
            Case Else
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set field validation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        End Select

        Finally
       

        End Try
	
	 Return result
    End Function


    ' ***************************************************************** '
    ' Name:         DisplayCaptions
    '
    ' Description:  Display all language specific captions.
    ' Histroy:      TR22112002 - TR23 Changed parameter from Form to Object
    '               to support UserControls as well as Forms. Explicitly
    '               named the assumed default properties.
    ' ***************************************************************** '
    Public Function DisplayCaptions(ByRef r_frmSource As Form) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: DisplayCaptions
        ' PURPOSE:
        ' AUTHOR: Paul Cunnigham
        ' DATE: 14 October 2002, 15:21:41
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim lCaptionID As Integer
        Dim iSkip As Integer
        Dim sSetRoundOff As String = ""
        'Used in the .Tag property of ListView.CoulmnHeading
        Const ksHidden As String = "HIDDEN"


        Try

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Display all language specific captions.

        'Get the caption from the tag
        lCaptionID = GetCaptionID(Convert.ToString(r_frmSource.Tag))
        If lCaptionID > 0 Then
            'Get the caption from the res file using Id from tag property

            r_frmSource.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=lCaptionID, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

            ' Check for an error.
            If r_frmSource.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() & _
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If
        End If
        sSetRoundOff = ""


        For i As Integer = 0 To ContainerHelper.Controls(r_frmSource).Count - 1

            lCaptionID = GetCaptionID(Convert.ToString(ControlHelper.GetTag(ContainerHelper.Controls(r_frmSource)(i))))
            If lCaptionID > 0 Then


                Select Case ContainerHelper.Controls(r_frmSource)(i).GetType().Name
                    Case "SSTab"


                        'For j As Integer = 0 To ContainerHelper.Controls(r_frmSource)(i).Tabs - 1
                        For j As Integer = 0 To ReflectionHelper.GetMember(ContainerHelper.Controls(r_frmSource)(i), "Tabs") - 1



                            'Replaced with Migrated code 114
                            'ContainerHelper.Controls(r_frmSource)(i).TabCaption(j) = iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=lCaptionID + j, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString)
                            ReflectionHelper.SetMember(ContainerHelper.Controls(r_frmSource)(i), "TabCaption", New Object() {j}, iPMFunc.GetResData(g_iLanguageID, lCaptionID + j, gPMConstants.PMEResourseFileDataType.PMResString))
                        Next j
                        'Added ListView column headers
                    Case "ListView"
                        iSkip = 0


                        'For j As Integer = 1 To ContainerHelper.Controls(r_frmSource)(i).ColumnHeaders.Count
                        For j As Integer = 1 To ReflectionHelper.GetMember(ReflectionHelper.GetMember(ContainerHelper.Controls(r_frmSource)(i), "ColumnHeaders"), "Count")
                            'Test for hidden ListView columns and skip as appropriate


                            'Replaced with Migrated code 114
                            'If CStr(ContainerHelper.Controls(r_frmSource)(i).ColumnHeaders(j).Tag).IndexOf(ksHidden) >= 0 Then
                            If CStr(ReflectionHelper.GetMember(ReflectionHelper.Invoke(ContainerHelper.Controls(r_frmSource)(i), "ColumnHeaders", New Object() {j}), "Tag")).IndexOf(ksHidden) >= 0 Then
                                iSkip += 1
                            Else
                                'PWC - 16/10/2002 - No need to get caption if skipping this time




                                'Replaced with Migrated code 114
                                'ContainerHelper.Controls(r_frmSource)(i).ColumnHeaders(j).Text = iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=lCaptionID + j - iSkip - 1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString)
                                ReflectionHelper.SetMember(ReflectionHelper.Invoke(ContainerHelper.Controls(r_frmSource)(i), "ColumnHeaders", New Object() {j}), "Text", iPMFunc.GetResData(g_iLanguageID, lCaptionID + j - iSkip - 1, gPMConstants.PMEResourseFileDataType.PMResString))
                                'If ContainerHelper.Controls(r_frmSource)(i).ColumnHeaders(j).Text.Trim() = ("User").Trim() Then
                                If ReflectionHelper.GetMember(ReflectionHelper.Invoke(ContainerHelper.Controls(r_frmSource)(i), "ColumnHeaders", New Object() {j}), "Text") = ("User").Trim() Then
                                    sSetRoundOff = "Round Off Amount"
                                End If
                            End If
                            'If j = ContainerHelper.Controls(r_frmSource)(i).ColumnHeaders.Count Then
                            If j = ReflectionHelper.GetMember(ReflectionHelper.GetMember(ContainerHelper.Controls(r_frmSource)(i), "ColumnHeaders"), "Count") Then
                                If sSetRoundOff <> "" Then
                                    'ContainerHelper.Controls(r_frmSource)(i).ColumnHeaders(j).Text = sSetRoundOff
                                    ReflectionHelper.SetMember(ReflectionHelper.Invoke(ContainerHelper.Controls(r_frmSource)(i), "ColumnHeaders", New Object() {j}), "Text", sSetRoundOff)
                                    sSetRoundOff = ""
                                End If
                            End If


                        Next j
                        'Added Picklist
                    Case "PickList"




                        'Replaced with Migrated code 114
                        'ContainerHelper.Controls(r_frmSource)(i).AvailableCaption = iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=lCaptionID, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString)
                        ReflectionHelper.SetMember(ContainerHelper.Controls(r_frmSource)(i), "AvailableCaption", iPMFunc.GetResData(g_iLanguageID, lCaptionID, gPMConstants.PMEResourseFileDataType.PMResString))
                    Case Else



                        ContainerHelper.Controls(r_frmSource)(i).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=lCaptionID, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))
                End Select
            End If
        Next i


        '----------------------------------------------------------------------------------------
        'Only for Debugging, the code will never execute this line
        '----------------------------------------------------------------------------------------
         

        Catch ex As Exception
        Select Case Information.Err().Number
            Case Else
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                result = gPMConstants.PMEReturnCode.PMFalse

        End Select

        Finally
       

        End Try
	 Return result
    End Function

    ' ***************************************************************** '
    ' Name:         DisplayCaptions
    '
    ' Description:  Display all language specific captions.
    ' Histroy:      TR22112002 - TR23 Changed parameter from Form to Object
    '               to support UserControls as well as Forms. Explicitly
    '               named the assumed default properties.
    ' ***************************************************************** '
    Public Function DisplayCaptions(ByRef r_frmSource As Form, ByRef bResFile As Object) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: DisplayCaptions
        ' PURPOSE:
        ' AUTHOR: Paul Cunnigham
        ' DATE: 14 October 2002, 15:21:41
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim lCaptionID As Integer
        Dim iSkip As Integer
        'Used in the .Tag property of ListView.CoulmnHeading
        Const ksHidden As String = "HIDDEN"


        Try

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Display all language specific captions.

        'Get the caption from the tag
        lCaptionID = GetCaptionID(Convert.ToString(r_frmSource.Tag))
        If lCaptionID > 0 Then
            'Get the caption from the res file using Id from tag property


            r_frmSource.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=lCaptionID, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=bResFile))

            ' Check for an error.
            If r_frmSource.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() & _
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If
        End If



        For i As Integer = 0 To ContainerHelper.Controls(r_frmSource).Count - 1

            lCaptionID = GetCaptionID(Convert.ToString(ControlHelper.GetTag(ContainerHelper.Controls(r_frmSource)(i))))
            If lCaptionID > 0 Then


                Select Case ContainerHelper.Controls(r_frmSource)(i).GetType().Name
                    Case "SSTab"


                        'For j As Integer = 0 To ContainerHelper.Controls(r_frmSource)(i).Tabs - 1
                        For j As Integer = 0 To ReflectionHelper.GetMember(ContainerHelper.Controls(r_frmSource)(i), "Tabs") - 1



                            'Replaced with Migrated code 114
                            'ContainerHelper.Controls(r_frmSource)(i).TabCaption(j) = iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=lCaptionID + j, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString)

                            ReflectionHelper.SetMember(ContainerHelper.Controls(r_frmSource)(i), "TabCaption", New Object() {j}, iPMFunc.GetResData(g_iLanguageID, lCaptionID + j, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=bResFile))
                        Next j
                        'Added ListView column headers
                    Case "ListView"
                        iSkip = 0


                        'For j As Integer = 1 To ContainerHelper.Controls(r_frmSource)(i).ColumnHeaders.Count
                        'For j As Integer = 1 To ReflectionHelper.GetMember(ReflectionHelper.GetMember(ContainerHelper.Controls(r_frmSource)(i), "Columns"), "Count")
                        For j As Integer = 1 To ReflectionHelper.GetMember(ReflectionHelper.GetMember(ContainerHelper.Controls(r_frmSource)(i), "Columns"), "Count") - 1
                            'Test for hidden ListView columns and skip as appropriate


                            'Replaced with Migrated code 114
                            'If CStr(ContainerHelper.Controls(r_frmSource)(i).ColumnHeaders(j).Tag).IndexOf(ksHidden) >= 0 Then
                            'If CStr(ReflectionHelper.GetMember(ReflectionHelper.Invoke(ContainerHelper.Controls(r_frmSource)(i), "Columns", New Object() {j}), "Tag")).IndexOf(ksHidden) >= 0 Then
                            'If CStr(ReflectionHelper.GetMember(ReflectionHelper.GetMember(ContainerHelper.Controls(r_frmSource)(i), "Columns")(j), "Tag")).IndexOf(ksHidden) >= 0 Then

                            If Convert.ToString(ReflectionHelper.GetMember(ReflectionHelper.GetMember(ContainerHelper.Controls(r_frmSource)(i), "Columns")(j), "Tag")).IndexOf(ksHidden) >= 0 Then
                                'If CStr(ReflectionHelper.GetMember(ReflectionHelper.Invoke(ContainerHelper.Controls(r_frmSource)(i), "Columns", New Object() {j}), "Tag")).IndexOf(ksHidden) >= 0 Then
                                iSkip += 1
                            Else
                                'PWC - 16/10/2002 - No need to get caption if skipping this time




                                'Replaced with Migrated code 114
                                'ContainerHelper.Controls(r_frmSource)(i).ColumnHeaders(j).Text = iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=lCaptionID + j - iSkip - 1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString)

                                'TODO LIST
                                'ReflectionHelper.SetMember(ReflectionHelper.Invoke(ContainerHelper.Controls(r_frmSource)(i), "Columns", New Object() {j}), "Text", iPMFunc.GetResData(g_iLanguageID, lCaptionID + j - iSkip - 1, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=bResFile))
                                ContainerHelper.Controls(r_frmSource)(i).Text = iPMFunc.GetResData(g_iLanguageID, lCaptionID + j - iSkip - 1, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=bResFile)
                            End If
                        Next j
                        'Added Picklist
                    Case "PickList"




                        'Replaced with Migrated code 114
                        'ContainerHelper.Controls(r_frmSource)(i).AvailableCaption = iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=lCaptionID, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString)

                        ReflectionHelper.SetMember(ContainerHelper.Controls(r_frmSource)(i), "AvailableCaption", iPMFunc.GetResData(g_iLanguageID, lCaptionID, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=bResFile))
                    Case Else




                        ContainerHelper.Controls(r_frmSource)(i).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=lCaptionID, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=bResFile))
                End Select
            End If
        Next i

         

        '----------------------------------------------------------------------------------------
        'Only for Debugging, the code will never execute this line
        '----------------------------------------------------------------------------------------
         

	Catch ex As Exception
        Select Case Information.Err().Number
            Case Else
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                result = gPMConstants.PMEReturnCode.PMFalse


        End Select

	Finally 
	
	End Try
        Return result

    End Function

	Public Function DisplayMsgBox(ByRef r_lTitleId As Integer, ByRef r_lMessageId As Integer, ByRef r_lOptions As Integer, ParamArray ByVal r_vTokens() As Object) As Integer
		' ---------------------------------------------------------------------------
		' PROCEDURE NAME: DisplayMsgBox
		' PURPOSE: Displays a message based on passed resource file Ids
		' AUTHOR: Sirius Financial Systems Plc
		' DATE: 09 October 2002, 16:03:54
		' RETURNS: PMTrue for success
		' CHANGES: PWC 16/10/2002 - Added param array to enable substition of tokens
		' ---------------------------------------------------------------------------
		
		Dim result As Integer = 0
		Dim sTitle, sMessage As String
		
		
		Try
		
		'Get the title from the res file

		sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=r_lTitleId, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))
		
		'Get the message from the res file

		sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=r_lMessageId, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))
		
		'Replace Tokens in the message
		ReplacePlaceHolders(sMessage, New Object(){r_vTokens})
		
		'Now display the message to the user
		result = Interaction.MsgBox(sMessage, r_lOptions, sTitle)
		
		
		'----------------------------------------------------------------------------------------
		'Only for Debugging, the code will never execute this line
		'----------------------------------------------------------------------------------------
		  
		
		Catch ex As Exception
		Select Case Information.Err().Number
			Case Else
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayMsgBox", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
				
				result = gPMConstants.PMEReturnCode.PMError
				
		End Select
		
		Finally
		
		
		End Try
		Return result
	End Function
	
    Public Function ReplacePlaceHolders(ByRef r_sMessage As String, ByVal ParamArray r_vTokens() As Object) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: ReplacePlaceHolders
        ' PURPOSE: Replace place holders with values
        '          e.g. convert |Username| to "Fred Bloggs"
        ' AUTHOR: Paul Cunnigham
        ' DATE: 21 October 2002, 10:04:52
        ' RETURNS: True for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim lUpper As Integer
        Dim vToken, vValue As Object




        'This routine could be called directly like...
        '    ReplacePlaceHolders sMessage, "Usename", m sUserName
        'With the params explicitly listed

        'OR by a routine that itself accepts a ParamArray.
        '    ReplacePlaceHolders sMessage, r_vParams

        'We need to ensure that we find the 'root' ParamArray as the second
        'calling method would pass Variant(0)(0), Variant(0)(1) into this routine
        'and we need Variant(0), Variant(1)


        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Find the 'root' paramarray
            '(i.e. convert  Variant(0)(0) to Variant(0))
            Dim vParams As Object = r_vTokens

            While vParams(0).GetType().Name = "Variant()"
                If Information.Err().Number <> 0 Then
                    'No params passed at all
                    Information.Err().Clear()
                    Return result
                End If


                vParams = vParams(0)

                If vParams.GetUpperBound(0) = -1 Then ' no params
                    Return result
                End If
            End While




            'Any params actually passed?
            'We could just have a blank paramarray
            lUpper = r_vTokens.GetUpperBound(0)
            If lUpper <> -1 Then
                'Loop through the param array
                For iItem As Integer = 0 To lUpper \ 2
                    'Get the token and its replacement value


                    vToken = vParams(iItem * 2)
                    'This will bomb if developer has passed an odd number of params


                    vValue = vParams(iItem * 2 + 1)

                    'Replace the token with the value


                    r_sMessage = r_sMessage.Replace("|" & CStr(vToken) & "|", CStr(vValue))
                Next
            End If

            result = gPMConstants.PMEReturnCode.PMTrue

            GoTo Finally_Renamed

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------



Catch_Renamed:
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="ReplacePlaceHolders", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    result = gPMConstants.PMEReturnCode.PMError

                    GoTo Finally_Renamed
            End Select

Finally_Renamed:
            Return result

        Catch exc As System.Exception
            'NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try

    End Function
End Module
