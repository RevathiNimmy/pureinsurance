Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports VB6 = Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
<System.Runtime.InteropServices.ProgId("PBRiskScreenCommon2_NET.PBRiskScreenCommon2")> _
 Public Module PBRiskScreenCommon2
	
	Private Const ACClass As String = "PBRiskScreenCommon2"
    'Added by Deepak Sharma on 4/28/2010 11:17:14 AM refer developer guide no.

    Public g_bGetEnableClaimVersions As Boolean

    Public Const ACBlankCaption As String = "[BLANK]"

    Private m_lReturn As gPMConstants.PMEReturnCode

    ' ***************************************************************** '
    ' Name: CheckEmptyString
    ' Description:
    ' ***************************************************************** '
    Private Function CheckEmptyString(ByVal v_sString As String) As String

        Try


            If v_sString.Trim() = "" Then
                Return "(Empty)"
            Else
                Return v_sString
            End If

        Catch
        End Try



        Return v_sString

    End Function



    ' ***************************************************************** '
    '
    ' Name: CalculateLinesInCaption
    '
    ' Description:
    '
    ' History: 26/07/2001 CLG - Created, from Tom O'Toole code
    '
    ' ***************************************************************** '
    Public Function CalculateLinesInCaption(ByVal sCaption As String) As Integer

        Dim result As Integer = 0
        Dim iTemp As Integer

        Try

            iTemp = (sCaption.IndexOf(Strings.Chr(13) & Strings.Chr(10)) + 1)
            While iTemp <> 0
                sCaption = sCaption.Substring(iTemp + 1)
                result += 1
                iTemp = (sCaption.IndexOf(Strings.Chr(13) & Strings.Chr(10)) + 1)
            End While

            Return result

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".CalculateLinesInCaption")

            result = 0

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CalculateLinesInCaption Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CalculateLinesInCaption", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: simulateTriStateCheckBox
    '
    ' Description: Simulates a Gemini YesNoCheck box using a VB checkbox
    '
    ' History: 12/09/2001 CLG - Created.
    '
    ' ***************************************************************** '
    Public Sub simulateTriStateCheckBox(ByVal iCheckBoxValue As Integer, ByRef chkYesNo As Object, Optional ByVal isCheckedStateChanged As Boolean = False)

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".simulateTriStateCheckBox")

        Try

            'if we are dealing with a tri-state

            If chkYesNo.Width = VB6.TwipsToPixelsY(PBRiskScreenCommon.cCheckBoxTriStateCaptionWidth) Then
                If isCheckedStateChanged Then
                    Select Case iCheckBoxValue
                        Case 0

                            'chkYesNo.Value = 1
                            CType(chkYesNo, CheckBox).CheckState = CheckState.Checked
                        Case 1

                            'chkYesNo.Value = 2
                            CType(chkYesNo, CheckBox).CheckState = CheckState.Indeterminate

                        Case 2

                            'chkYesNo.Value = 0
                            CType(chkYesNo, CheckBox).CheckState = CheckState.Unchecked

                    End Select
                Else
                    Select Case iCheckBoxValue
                        Case 0

                            'chkYesNo.Value = 1
                            CType(chkYesNo, CheckBox).CheckState = CheckState.Unchecked
                        Case 1

                            'chkYesNo.Value = 2
                            CType(chkYesNo, CheckBox).CheckState = CheckState.Checked

                        Case 2

                            'chkYesNo.Value = 0
                            CType(chkYesNo, CheckBox).CheckState = CheckState.Indeterminate

                    End Select
                End If

                Select Case chkYesNo.CheckState
                    Case 0


                        'chkYesNo.Caption = "No"
                        'CType(chkYesNo, CheckBox).CheckState = CheckState.Unchecked
                        chkYesNo.Text = "No"
                    Case 1


                        'chkYesNo.Caption = "Yes"
                        'CType(chkYesNo, CheckBox).CheckState = CheckState.Checked
                        chkYesNo.Text = "Yes"
                    Case Else


                        'chkYesNo.Caption = "Unknown"
                        chkYesNo.Text = "Unknown"
                        'CType(chkYesNo, CheckBox).CheckState = CheckState.Indeterminate


                End Select
            Else
                chkYesNo.Text = ""
            End If


                ' Debug message
                Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".simulateTriStateCheckBox")

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".simulateTriStateCheckBox")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="simulateTriStateCheckBox Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="simulateTriStateCheckBox", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    ' ***************************************************************** '
    '
    ' Name: textLabel_MouseMove
    '
    ' Description: Moves a control in sync with its label being moved
    '              As a side effect it can be used to position a control
    '
    ' History: 12/09/2001 CLG - Created, from Tom O'Toole code.
    '
    ' ***************************************************************** '
    Public Sub textLabel_MouseMove(ByVal v_sControlName As String, ByRef Button As Integer, ByVal x As Single, ByVal y As Single, ByRef cControl As Control, ByRef cLabel As Control, ByRef cCaption As Label, ByVal v_vSnapToGrid As Object)



        Dim lLeft, lRight As Integer
        Dim chkYesNoDontKnowVerticalOffset, chkYesNoDontKnowHorizontalOffset As Integer
        Dim dValue, dYfiddleFactor As Double
        Dim lLeftFiddlefactor As Integer 'allows [BLANK] captions to almost go off the left of the frame

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".textLabel_MouseMove")

        Try


            PBRiskScreenCommon.g_sControlName = v_sControlName

            Select Case Button
                'Case VB6.MouseButtonConstants.LeftButton
                Case MouseButtons.Left

                    If PBRiskScreenCommon.g_sControlName = "chkYesNo" Then
                        ' developer guide no. 74
                        chkYesNoDontKnowVerticalOffset = VB6.TwipsToPixelsY(45) + ((cLabel.Height) / 2) - ((cControl.Height) / 2) + VB6.TwipsToPixelsY(PBRiskScreenCommon.cCheckBoxVerticalOffset)
                        chkYesNoDontKnowHorizontalOffset = VB6.TwipsToPixelsX(PBRiskScreenCommon.cCheckBoxHorizontalOffset)
                    End If
                    'calculate snapped label top
                    'make sure it doesn't go off the top
                    dYfiddleFactor = 0
                    dValue = 0
                    Do

                        If CDbl(v_vSnapToGrid) > 0 Then
                            'Modified by Alkesh Kumar on 03/25/2010 6:31:45 PM refer developer guide no.74 
                            'dValue = (VB6.PixelsToTwipsY(cLabel.Top) - PBRiskScreenCommon.g_ly + y + dYfiddleFactor) / 100.0#
                            dValue = ((cLabel.Top) - PBRiskScreenCommon.g_ly + y + dYfiddleFactor) / 100.0#
                            'dValue = Math.Round(dValue, 0) * 100
                            dValue = dValue * 100
                        Else
                            'dValue = VB6.PixelsToTwipsY(cLabel.Top) - PBRiskScreenCommon.g_ly + y + dYfiddleFactor
                            dValue = (cLabel.Top) - PBRiskScreenCommon.g_ly + y + dYfiddleFactor
                        End If
                        dYfiddleFactor += 10
                        'Modified by Alkesh Kumar on 6/3/2010 11:22:41 AM refer developer guide no. 74
                    Loop While (dValue) - Math.Round(45 / 15) + chkYesNoDontKnowVerticalOffset < Math.Round(120 / 15)
                    'Loop While dValue + chkYesNoDontKnowVerticalOffset < VB6.TwipsToPixelsY(120)

                    'make sure it doesn't go off the bottom
                    dYfiddleFactor = 0
                    'Modified by Alkesh Kumar on 03/25/2010 6:32:20 PM refer developer guide no. 74
                    'Do While dValue > (VB6.PixelsToTwipsY(cLabel.Parent.Height) - 300)
                    Do While dValue > ((cLabel.Parent.Height) - VB6.TwipsToPixelsY(300))

                        If CDbl(v_vSnapToGrid) > 0 Then
                            'Modified by Alkesh Kumar on 6/3/2010 11:54:04 AM refer developer guide no. 74
                            'dValue = (VB6.PixelsToTwipsY(cLabel.Top) - PBRiskScreenCommon.g_ly + y - dYfiddleFactor) / 100.0#
                            dValue = ((cLabel.Top) - PBRiskScreenCommon.g_ly + y - dYfiddleFactor) / 100.0#
                            'dValue = Math.Round(dValue, 0) * 100
                            dValue = dValue * 100
                            'dValue = ((cLabel.Top) - PBRiskScreenCommon.g_ly + y - dYfiddleFactor)
                            'dValue = Math.Round(dValue, 0)
                        Else
                            'Modified by Alkesh Kumar on 6/3/2010 11:54:34 AM refer developer guide no. 74
                            'dValue = VB6.PixelsToTwipsY(cLabel.Top) - PBRiskScreenCommon.g_ly + y - dYfiddleFactor
                            dValue = (cLabel.Top) - PBRiskScreenCommon.g_ly + y - dYfiddleFactor
                        End If
                        dYfiddleFactor += 10
                    Loop
                    'Modified by Alkesh Kumar on 10/05/2010 12:20:50 refer developer guide no. 74
                    'Modification Starts
                    'If VB6.PixelsToTwipsY(cLabel.Top) <> dValue Then
                    If (cLabel.Top) <> dValue Then
                        'cLabel.Top = VB6.TwipsToPixelsY(dValue)
                        cLabel.Top = (dValue)
                    End If
                    'check if attribute bound control and control not in position
                    'If CDbl(Convert.ToString(ControlHelper.GetTag(cControl))) Mod 10000 >= 0 And VB6.PixelsToTwipsY(cControl.Top) <> (dValue - 45 + chkYesNoDontKnowVerticalOffset) Then
                    '    cControl.Top = VB6.TwipsToPixelsY(dValue - 45 + chkYesNoDontKnowVerticalOffset)


                    'lLeft = CInt(VB6.PixelsToTwipsX(cLabel.Left) - PBRiskScreenCommon.g_lx + x)

                    If CDbl(Convert.ToString(ControlHelper.GetTag(cControl))) Mod 10000 >= 0 And (cControl.Top) <> (dValue - VB6.TwipsToPixelsY(45) + chkYesNoDontKnowVerticalOffset) Then
                        cControl.Top = (dValue - VB6.TwipsToPixelsY(45) + chkYesNoDontKnowVerticalOffset)
                    End If

                    lLeft = CInt((cLabel.Left) - PBRiskScreenCommon.g_lx + x)

                    If CDbl(v_vSnapToGrid) > 0 Then
                        dValue = lLeft / VB6.TwipsToPixelsX(100.0#)
                        'lLeft = CInt(Math.Round(dValue, 0) * 100)
                        lLeft = dValue * VB6.TwipsToPixelsX(100)
                    End If


                    'If cLabel.Caption = ACBlankCaption Then 'allows [BLANK] captions to almost go off the left of the frame
                    ' lLeftFiddlefactor = CInt(VB6.PixelsToTwipsX(cLabel.Width) + 50)
                    lLeftFiddlefactor = CInt((cLabel.Width) + VB6.TwipsToPixelsX(50))
                    'Else
                    '    If lLeft < 100 Then lLeft = 100 'if off the left then bring back to left edge


                    'if label off the right hand edge then move back into view
                    'If lLeft > VB6.PixelsToTwipsX(cLabel.Parent.Width) - 120 Then
                    '    lLeft = CInt(VB6.PixelsToTwipsX(cLabel.Parent.Width) - 120)


                    'lRight = CInt(lLeft + VB6.PixelsToTwipsX(cLabel.Width))
                    'If CDbl(Convert.ToString(ControlHelper.GetTag(cControl))) >= 0 Then 'check if attribute bound control
                    '    lRight = CInt(lRight + VB6.PixelsToTwipsX(cControl.Width) + PBRiskScreenCommon.cControlHorizontalOffset)


                    If lLeft > (cLabel.Parent.Width) - VB6.TwipsToPixelsX(120) Then
                        lLeft = CInt((cLabel.Parent.Width) - VB6.TwipsToPixelsX(120))
                    End If

                    lRight = CInt(lLeft + (cLabel.Width))
                    If CDbl(Convert.ToString(ControlHelper.GetTag(cControl))) >= 0 Then 'check if attribute bound control
                        lRight = CInt(lRight + (cControl.Width) + VB6.TwipsToPixelsX(PBRiskScreenCommon.cControlHorizontalOffset))
                    End If


                    If (lLeft + lLeftFiddlefactor) >= Math.Round(VB6.TwipsToPixelsX(100)) Or (0 - PBRiskScreenCommon.g_lx + x) > 0 Then
                        'If (lLeft >= 100 Or (0 - g_lx + x) > 0) Then
                        '    If (lRight < (cLabel.Container.Width - 120)) Then
                        'If VB6.PixelsToTwipsX(cLabel.Left) <> lLeft Then cLabel.Left = VB6.TwipsToPixelsX(lLeft)
                        If (cLabel.Left) <> lLeft Then cLabel.Left = (lLeft)
                        If CDbl(Convert.ToString(ControlHelper.GetTag(cControl))) >= 0 Then 'check if attribute bound control
                            'dValue = VB6.PixelsToTwipsX(cLabel.Left) + VB6.PixelsToTwipsX(cLabel.Width) + PBRiskScreenCommon.cControlHorizontalOffset + chkYesNoDontKnowHorizontalOffset
                            dValue = (cLabel.Left) + (cLabel.Width) + VB6.TwipsToPixelsX(PBRiskScreenCommon.cControlHorizontalOffset) + chkYesNoDontKnowHorizontalOffset

                            If CDbl(v_vSnapToGrid) > 0 Then
                                dValue /= VB6.TwipsToPixelsX(100.0#)
                                'dValue = Math.Round(dValue, 0) * 100
                                dValue = dValue * VB6.TwipsToPixelsX(100)
                            End If
                            'If VB6.PixelsToTwipsX(cControl.Left) <> dValue Then cControl.Left = VB6.TwipsToPixelsX(dValue)
                            If (cControl.Left) <> dValue Then cControl.Left = (dValue)
                        End If

                    End If

                    'if non database bound control, freeformat, hyperlink etc
                    If CDbl(Convert.ToString(ControlHelper.GetTag(cControl))) < 0 Then
                        cControl.Left = cLabel.Left
                        cControl.Top = cLabel.Top - VB6.TwipsToPixelsY(45)
                    End If
                    ' developer guide no. 74
                    'cCaption.Text = "Left: " & VB6.PixelsToTwipsX(cControl.Left) & ", Top: " & CStr(VB6.PixelsToTwipsY(cControl.Top) - chkYesNoDontKnowVerticalOffset)
                    cCaption.Text = "Left: " & (cControl.Left) & ", Top: " & CStr((cControl.Top) - chkYesNoDontKnowVerticalOffset)
                    'Modification End
                Case Else
                    cControl.Parent.Cursor = Cursors.Arrow
            End Select

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".textLabel_MouseMove")

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".textLabel_MouseMove")


            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="textLabel_MouseMove Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="textLabel_MouseMove", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub



    ' ***************************************************************** '
    '
    ' Name: SetInitialControlValues
    '
    ' Description:
    '
    ' History: 15/08/2001 CLG - Created.
    '
    ' ***************************************************************** '
    'Modified by Deepak Sharma on 4/20/2010 5:48:48 PM refer developer guide no. 101(Guide)
    'Public Function SetInitialControlValues(ByVal v_lControlIndex As Integer, ByRef r_controlArrary( ,  ) As Object, ByRef r_labelControl As VB6.ControlArray, ByRef r_EditControl As VB6.ControlArray, ByRef r_fraFrame As Object, ByVal v_iFrameIndex As Object, ByVal v_iTask As Integer, ByVal v_vPMFormat As Object, ByVal v_vTabSetIndex As Object, ByVal v_vColumnPosition As Object, ByVal v_lX As Integer, ByVal v_lY As Integer, ByRef v_sPropertyName As String, ByVal v_lControlTag As Object, ByVal v_lLabelTag As Object, ByVal v_bFormat As Boolean, ByVal v_lControlHeight As Integer, ByVal v_lControlWidth As Integer, ByVal v_sHelpText As String, ByVal v_lPreQuote As Integer, ByVal v_lPostQuote As Integer, ByVal v_lPurchase As Integer, ByVal v_lIsValuation As Integer, ByVal v_lIsRateAndPremium As Integer, ByVal v_lIncludeInList As Integer, ByRef v_lStage As Integer, ByRef r_oFormFields As iPMFormControl.FormFields, ByRef v_lControlCount As Integer, ByRef v_vDataDictionary( ,  ) As Object, ByRef v_vScreenValues() As Object, Optional ByVal bInScreenEdit As Boolean = False) As Integer
    Public Function SetInitialControlValues(ByVal v_lControlIndex As Integer, ByRef r_controlArrary(,) As Object, ByRef r_labelControl As Object, ByRef r_EditControl As Object, ByRef r_fraFrame As Object, ByVal v_iFrameIndex As Object, ByVal v_iTask As Integer, ByVal v_vPMFormat As Object, ByVal v_vTabSetIndex As Object, ByVal v_vColumnPosition As Object, ByVal v_lX As Integer, ByVal v_lY As Integer, ByRef v_sPropertyName As String, ByVal v_lControlTag As Object, ByVal v_lLabelTag As Object, ByVal v_bFormat As Boolean, ByVal v_lControlHeight As Integer, ByVal v_lControlWidth As Integer, ByVal v_sHelpText As String, ByVal v_lPreQuote As Integer, ByVal v_lPostQuote As Integer, ByVal v_lPurchase As Integer, ByVal v_lIsValuation As Integer, ByVal v_lIsRateAndPremium As Integer, ByVal v_lIncludeInList As Integer, ByRef v_lStage As Integer, ByRef r_oFormFields As Object, ByRef v_lControlCount As Integer, ByRef v_vDataDictionary(,) As Object, ByRef v_vScreenValues() As Object, Optional ByVal bInScreenEdit As Boolean = False, Optional ByVal ACPDataType As Integer = 4, Optional ByVal ACPIsInputProperty As Integer = 5) As Integer


        Dim result As Integer = 0
        Dim lMandatory As gPMConstants.PMEMandatoryStatus
        Dim lFieldType As Integer
        Dim lPMFieldType As gPMConstants.PMEDataType
        Dim lFormat As Integer
        Dim vArray() As Object
        ' AMB 10/01/03 - Start - IAG 217 Spec
        Dim vOrigArray As Object
        ' AMB 10/01/03 - End - IAG 217 Spec
        Dim lNoOfDecimalPlaces As Integer
        Dim sValue As String
        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".SetInitialControlValues")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'tags can be passed either as a two element array or as an encoded value
            'if passed as an array, change to encoded value

#If quoteTiming And quoteTiming < 2 Then

			performanceCtr(performancecntrCntr, 1) = "uctRiskScreen.AddGISComboBox.SetInitialControlValues. set array" & vbTab & vbTab
			QueryPerformanceCounter performanceCtr(performancecntrCntr, 0): performancecntrCntr = performancecntrCntr + 1
#End If


            If Information.IsArray(v_lControlTag) Then



                v_lControlTag = CDbl(v_lControlTag(0)) * 10000 + CDbl(v_lControlTag(1))



                v_lLabelTag = CDbl(v_lLabelTag(0)) * 10000 + CDbl(v_lLabelTag(1))
            End If



            r_controlArrary(PBRiskScreenCommon.ACCFrameNumber, v_lControlIndex) = v_iFrameIndex

            r_controlArrary(PBRiskScreenCommon.ACCIsDeleted, v_lControlIndex) = gPMConstants.PMEReturnCode.PMFalse

            r_controlArrary(PBRiskScreenCommon.ACCHelpText, v_lControlIndex) = v_sHelpText

            r_controlArrary(PBRiskScreenCommon.ACCPreQuote, v_lControlIndex) = v_lPreQuote

            r_controlArrary(PBRiskScreenCommon.ACCPostQuote, v_lControlIndex) = v_lPostQuote

            r_controlArrary(PBRiskScreenCommon.ACCPurchase, v_lControlIndex) = v_lPurchase

            r_controlArrary(PBRiskScreenCommon.ACCIsValuation, v_lControlIndex) = v_lIsValuation

            r_controlArrary(PBRiskScreenCommon.ACCIsRateAndPremium, v_lControlIndex) = v_lIsRateAndPremium

            r_controlArrary(PBRiskScreenCommon.ACCIncludeInList, v_lControlIndex) = v_lIncludeInList


            r_controlArrary(PBRiskScreenCommon.ACCChildId, v_lControlIndex) = DBNull.Value


            r_controlArrary(PBRiskScreenCommon.ACCGISObjectId, v_lControlIndex) = DBNull.Value


            r_controlArrary(PBRiskScreenCommon.ACCPMFormat, v_lControlIndex) = v_vPMFormat
            'strip the high bit information

            'Niit Comments : "CBool()" - is not exist in vb code. "PBRiskScreenCommon.ACFormatStandardMask" It Behaves Differt in .NET
            'v_vPMFormat = PBRiskScreenCommon.ACFormatStandardMask And CBool(v_vPMFormat)
            v_vPMFormat = v_vPMFormat



            r_controlArrary(PBRiskScreenCommon.ACCTabSetIndex, v_lControlIndex) = v_vTabSetIndex


            r_controlArrary(PBRiskScreenCommon.ACCColumnPosition, v_lControlIndex) = v_vColumnPosition
#If quoteTiming And quoteTiming < 2 Then

			performanceCtr(performancecntrCntr, 1) = "uctRiskScreen.AddGISComboBox.SetInitialControlValues. set array" & vbTab & vbTab
			QueryPerformanceCounter performanceCtr(performancecntrCntr, 0): performancecntrCntr = performancecntrCntr + 1
#End If

#If quoteTiming And quoteTiming < 2 Then

			performanceCtr(performancecntrCntr, 1) = "uctRiskScreen.AddGISComboBox.SetInitialControlValues. set lprops" & vbTab
			QueryPerformanceCounter performanceCtr(performancecntrCntr, 0): performancecntrCntr = performancecntrCntr + 1
#End If

            With r_labelControl(v_lControlIndex)

                .Parent = r_fraFrame(v_iFrameIndex)
                '.Top = VB6.TwipsToPixelsY(v_lY) '(v_lY)
                '.Left = VB6.TwipsToPixelsX(v_lX) '(v_lX)
                .Top = (v_lY)
                .Left = (v_lX)
                .Visible = True
                '.Height = VB6.TwipsToPixelsY((CalculateLinesInCaption(v_sPropertyName) * 240) + 195)
                .Height = VB6.TwipsToPixelsY((CalculateLinesInCaption(v_sPropertyName) * 240) + 195)
                .Text = v_sPropertyName

                .Tag = CStr(v_lLabelTag) '  was m_lControlCount
                .Font = VB6.FontChangeUnderline(.Font, False)

                .AutoSize = True
            End With
#If quoteTiming And quoteTiming < 2 Then

			performanceCtr(performancecntrCntr, 1) = "uctRiskScreen.AddGISComboBox.SetInitialControlValues. set lprops" & vbTab
			QueryPerformanceCounter performanceCtr(performancecntrCntr, 0): performancecntrCntr = performancecntrCntr + 1
#End If

#If quoteTiming And quoteTiming < 2 Then

			performanceCtr(performancecntrCntr, 1) = "uctRiskScreen.AddGISComboBox.SetInitialControlValues. set cprops start " & vbTab
			QueryPerformanceCounter performanceCtr(performancecntrCntr, 0): performancecntrCntr = performancecntrCntr + 1
			'    QueryPerformanceCounter performanceCtr(performancecntrCntr, 0): performancecntrCntr = performancecntrCntr + 1
#End If

            With r_EditControl(v_lControlIndex)

                .Parent = r_fraFrame(v_iFrameIndex)
                .Top = r_labelControl(v_lControlIndex).Top - VB6.TwipsToPixelsY(45)
                'Modified by Alkesh Kumar on 03/25/2010 7:04:29 PM refer developer guide no. 74
                '.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(r_labelControl(v_lControlIndex).Left) + VB6.PixelsToTwipsX(r_labelControl(v_lControlIndex).Width) + 60)
                '.Width = VB6.TwipsToPixelsX(v_lControlWidth) ' was m_lWidth
                '.Height = VB6.TwipsToPixelsY(v_lControlHeight) ' m_lHeight
                'Modified by Alkesh Kumar on 6/1/2010 4:17:52 PM refer developer guide no. Commented to adjust design
                .Left = r_labelControl(v_lControlIndex).Left + r_labelControl(v_lControlIndex).Width + VB6.TwipsToPixelsX(60)
                .Width = (v_lControlWidth) ' was m_lWidth
                .Height = (v_lControlHeight) ' m_lHeight
                .Visible = True

                .Tag = CStr(v_lControlTag)

                'Modified by Alkesh Kumar on 10/05/2010 12:13:40 refer developer guide no.  to be resolved
                '.ToolTipText = v_sHelpText

                If v_iTask = gPMConstants.PMEComponentAction.PMView Then
                    If Information.IsArray(v_vDataDictionary) Then
                        If CDbl(v_vDataDictionary(ACPDataType, CInt(CDbl(v_lControlTag) Mod 10000))) = 7 Then

                            If r_EditControl(v_lControlIndex).GetType().Name = "uctRichTextBox" Then
                                ReflectionHelper.SetMember(r_EditControl(v_lControlIndex), "Locked", True)
                            ElseIf r_EditControl(v_lControlIndex).GetType().Name = "cboGISLookup" Then
                                ReflectionHelper.SetMember(r_EditControl(v_lControlIndex), "Enabled", False)
                            Else
                                ReflectionHelper.SetMember(r_EditControl(v_lControlIndex), "ReadOnly", True)

                            End If
                        End If
                        .Enabled = True
                    End If
                Else
                    .Enabled = False
                End If

                If v_iTask <> gPMConstants.PMEComponentAction.PMView Then
                    If Information.IsArray(v_vDataDictionary) Then



                        If CDbl(v_vDataDictionary(ACPIsInputProperty, CInt(CDbl(v_lControlTag) Mod 10000))) = 1 Then
                            .Enabled = True
                        End If
                    Else
                        .Enabled = True
                    End If
                End If
            End With
#If quoteTiming And quoteTiming < 2 Then

			performanceCtr(performancecntrCntr, 1) = "uctRiskScreen.AddGISComboBox.SetInitialControlValues. set cprops end " & vbTab
			QueryPerformanceCounter performanceCtr(performancecntrCntr, 0): performancecntrCntr = performancecntrCntr + 1
			'    QueryPerformanceCounter performanceCtr(performancecntrCntr, 0): performancecntrCntr = performancecntrCntr + 1
#End If

#If quoteTiming And quoteTiming < 2 Then

			performanceCtr(performancecntrCntr, 1) = "uctRiskScreen.AddGISComboBox.SetInitialControlValues. anff pream" & vbTab
			QueryPerformanceCounter performanceCtr(performancecntrCntr, 0): performancecntrCntr = performancecntrCntr + 1
#End If


            'This is easy,
            'PMMandatory (our 2) = 1
            'PMNonMandatory (our 1) = 0
            'PMNonVisible (our 0) = 2
            lMandatory = gPMConstants.PMEMandatoryStatus.PMNonMandatory
            Select Case v_lStage
                Case 0
                    lMandatory = CType(v_lPreQuote - 1, gPMConstants.PMEMandatoryStatus)
                Case 1
                    lMandatory = CType(v_lPostQuote - 1, gPMConstants.PMEMandatoryStatus)
                Case 2
                    lMandatory = CType(v_lPurchase - 1, gPMConstants.PMEMandatoryStatus)
            End Select

            If lMandatory < 0 Then
                lMandatory = CType(lMandatory + 3, gPMConstants.PMEMandatoryStatus)
            End If

            If Information.IsArray(v_vDataDictionary) Then



                lFieldType = CInt(v_vDataDictionary(ACPDataType, CInt(CDbl(v_lControlTag) Mod 10000)))

                If Not (Convert.IsDBNull(v_vPMFormat) Or IsNothing(v_vPMFormat)) Then

                    lFormat = CInt(v_vPMFormat)
                Else
                    Select Case lFieldType
                        Case iGISSharedConstants.GISDataTypeText
                            lFormat = gPMConstants.PMEFormatStyle.PMFormatString
                        Case iGISSharedConstants.GISDataTypeComment
                            lFormat = gPMConstants.PMEFormatStyle.PMFormatStringMultiLine
                        Case iGISSharedConstants.GISDataTypeDate
                            lFormat = gPMConstants.PMEFormatStyle.PMFormatDateLong
                        Case iGISSharedConstants.GISDataTypeCurrency
                            lFormat = gPMConstants.PMEFormatStyle.PMFormatCurrency
                        Case iGISSharedConstants.GISDataTypePercentage
                            lFormat = gPMConstants.PMEFormatStyle.PMFormatPercent
                            ' RAW 17/06/2003 : CQ1107 : added
                        Case iGISSharedConstants.GISDataTypeNumeric
                            lFormat = gPMConstants.PMEFormatStyle.PMFormatLong
                        Case iGISSharedConstants.GISDataTypeOption
                            lFormat = gPMConstants.PMEFormatStyle.PMFormatBoolean
                            'SJ 15/07/2004 - start
                        Case iGISSharedConstants.GISDataTypeInteger
                            lFormat = gPMConstants.PMEFormatStyle.PMFormatLong
                            'SJ 15/07/2004 - end
                            ' RAW 17/06/2003 : CQ1107 : end
                        Case Else 'And this covers all the other numbers
                            lFormat = gPMConstants.PMEFormatStyle.PMFormatString
                    End Select
                End If


                'Need to translate GIS Formats into PM - GISDataTypeText has the same value as PMBoolean
                Select Case lFieldType
                    Case iGISSharedConstants.GISDataTypeDate
                        lPMFieldType = gPMConstants.PMEDataType.PMDate
                    Case iGISSharedConstants.GISDataTypeNumeric
                        lPMFieldType = gPMConstants.PMEDataType.PMLong
                    Case iGISSharedConstants.GISDataTypeText, iGISSharedConstants.GISDataTypeComment
                        lPMFieldType = gPMConstants.PMEDataType.PMString
                    Case iGISSharedConstants.GISDataTypeOption
                        lPMFieldType = gPMConstants.PMEDataType.PMBoolean
                    Case iGISSharedConstants.GISDataTypeCurrency
                        lPMFieldType = gPMConstants.PMEDataType.PMCurrency
                    Case iGISSharedConstants.GISDataTypePercentage
                        lPMFieldType = gPMConstants.PMEDataType.PMCurrency
                        'SJ 15/07/2004 - start
                    Case iGISSharedConstants.GISDataTypeInteger
                        lPMFieldType = gPMConstants.PMEDataType.PMLong
                        'SJ 15/07/2004 - end
                    Case Else
                        lPMFieldType = gPMConstants.PMEDataType.PMString
                End Select

                m_lReturn = CType(iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnable6DPGISPercentage, v_vBranch:=g_iSourceID, r_vUnderwriting:=sValue), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetInitialControlValues Failed for " & gPMConstants.SIRHiddenOptions.SIROPTUnderwritingBranchEnabled, vApp:=ACApp, vClass:=ACClass, vMethod:="SetInitialControlValues")
                    Return result
                End If

                'Tracy Richards 06/10/03 Add a DecimalPlace parameter
                'At this time - for Percentage fields only
                Select Case lFieldType
                    Case iGISSharedConstants.GISDataTypePercentage
                        '-4\6 displays 2 by default, but will allow up to 4\6 decimal places
                        If ToSafeString(sValue = "1") Then
                            lNoOfDecimalPlaces = -6
                        Else
                            lNoOfDecimalPlaces = -4
                        End If
                    Case Else
                        lNoOfDecimalPlaces = 0
                End Select
#If quoteTiming And quoteTiming < 2 Then

				performanceCtr(performancecntrCntr, 1) = "uctRiskScreen.AddGISComboBox.SetInitialControlValues. anff pream" & vbTab
				QueryPerformanceCounter performanceCtr(performancecntrCntr, 0): performancecntrCntr = performancecntrCntr + 1
#End If

#If quoteTiming And quoteTiming < 2 Then

				performanceCtr(performancecntrCntr, 1) = "uctRiskScreen.AddGISComboBox.SetInitialControlValues. anff actul" & vbTab
				QueryPerformanceCounter performanceCtr(performancecntrCntr, 0): performancecntrCntr = performancecntrCntr + 1
#End If


                result = r_oFormFields.AddNewFormField(ctlControl:=r_EditControl(v_lControlIndex), lGridColumn:=v_lControlIndex, lFieldType:=lPMFieldType, lFormat:=lFormat, lMandatory:=IIf(lMandatory = gPMConstants.PMEMandatoryStatus.PMNonMandatory, gPMConstants.PMEMandatoryStatus.PMNonMandatory, gPMConstants.PMEMandatoryStatus.PMMandatory), lDecimalPlaces:=lNoOfDecimalPlaces)

                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
#If quoteTiming And quoteTiming < 2 Then

				performanceCtr(performancecntrCntr, 1) = "uctRiskScreen.AddGISComboBox.SetInitialControlValues. anff actul" & vbTab
				QueryPerformanceCounter performanceCtr(performancecntrCntr, 0): performancecntrCntr = performancecntrCntr + 1
#End If

                'if screen editing display greyed {Blank] captions, if in runtime then hide them

                If Information.IsNothing(bInScreenEdit) Then bInScreenEdit = False
                SetBlankCaption(r_labelControl(v_lControlIndex), bInScreenEdit)
                Dim nfontsize As Single = ToSafeInteger(r_labelControl(v_lControlIndex).Font.Size)
                Dim nfontstyle As String = ToSafeString(r_labelControl(v_lControlIndex).Font.FontFamily)
                If v_lPreQuote = 2 Then
                    r_labelControl(v_lControlIndex).ForeColor = SystemColors.ControlText
                    r_labelControl(v_lControlIndex).Font = New Font(nfontstyle, nfontsize, FontStyle.Bold)
                ElseIf (v_lPostQuote = 2) Then
                    r_labelControl(v_lControlIndex).ForeColor = SystemColors.MenuHighlight
                ElseIf (v_lPurchase = 2) Then
                    r_labelControl(v_lControlIndex).ForeColor = SystemColors.MenuHighlight
                    r_labelControl(v_lControlIndex).Font = New Font(nfontstyle, nfontsize, FontStyle.Bold)
                End If

                'Modified by Alkesh Kumar on 03/25/2010 7:06:03 PM refer developer guide no. 74
                'r_labelControl(v_lControlIndex).Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(r_EditControl(v_lControlIndex).Left) - VB6.PixelsToTwipsX(r_labelControl(v_lControlIndex).Width) - 60)
                r_labelControl(v_lControlIndex).Left = (r_EditControl(v_lControlIndex).Left) - (r_labelControl(v_lControlIndex).Width) - VB6.TwipsToPixelsX(60)

                If v_bFormat Then
#If quoteTiming And quoteTiming < 2 Then

					performanceCtr(performancecntrCntr, 1) = "uctRiskScreen.AddGISComboBox.SetInitialControlValues. fca preamb" & vbTab
					QueryPerformanceCounter performanceCtr(performancecntrCntr, 0): performancecntrCntr = performancecntrCntr + 1
#End If



                    vArray = v_vScreenValues(v_lControlCount)

                    ' AMB 10/01/03 - Start - IAG 217 Spec, check original value
                    If g_bGetEnableClaimVersions Then


                        vOrigArray = PBRiskScreenCommon.g_vOriginalScreenValues(v_lControlCount)


                        If Not vArray(0).Equals(vOrigArray(0)) Then
                            r_EditControl(v_lControlIndex).BackColor = SystemColors.Info
                            ' set the control's tooltip, but it may already have one so;


                            r_EditControl(v_lControlIndex).ToolTipText = r_EditControl(v_lControlIndex).ToolTipText & _
                                                                        "[" & PBRiskScreenCommon.ORIGINAL_VALUE_STR & _
                                                                        CheckEmptyString(CStr(vOrigArray(0))) & "]"
                        End If
                    End If
                    ' AMB 10/01/03 - End - IAG 217 Spec, check original value
                    'when running from test harness this array is sometimes empty!
                    If Information.IsArray(vArray) Then

                        If (lPMFieldType = gPMConstants.PMEDataType.PMBoolean) And CStr(vArray(0)) = "" Then

                            vArray(0) = CheckState.Unchecked
                        End If

#If quoteTiming And quoteTiming < 2 Then

						performanceCtr(performancecntrCntr, 1) = "uctRiskScreen.AddGISComboBox.SetInitialControlValues. fca preamb" & vbTab
						QueryPerformanceCounter performanceCtr(performancecntrCntr, 0): performancecntrCntr = performancecntrCntr + 1
#End If
#If quoteTiming And quoteTiming < 2 Then

						performanceCtr(performancecntrCntr, 1) = "uctRiskScreen.AddGISComboBox.SetInitialControlValues. fca actual" & vbTab
						QueryPerformanceCounter performanceCtr(performancecntrCntr, 0): performancecntrCntr = performancecntrCntr + 1
#End If

                        'result = r_oFormFields.FormatControlArray(ctlControl:=r_EditControl(v_lControlIndex), vControlValue:=vArray(0))
                        result = r_oFormFields.FormatControlArray(ctlControl:=r_EditControl(v_lControlIndex), vControlIndex:=v_lControlIndex, vControlValue:=vArray(0))
                    End If


#If quoteTiming And quoteTiming < 2 Then

					performanceCtr(performancecntrCntr, 1) = "uctRiskScreen.AddGISComboBox.SetInitialControlValues. fca actual" & vbTab
					QueryPerformanceCounter performanceCtr(performancecntrCntr, 0): performancecntrCntr = performancecntrCntr + 1
#End If

                Else
                End If

            Else
            End If



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".SetInitialControlValues")

            Return result

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".SetInitialControlValues")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetInitialControlValues Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInitialControlValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: addTab
    '
    ' Description:
    '
    ' History: 07/01/2003 CLG - Created.
    '
    ' ***************************************************************** '
    Public Sub addTabControl(ByRef r_lTabIndex As Integer, ByRef r_vTabArray(,) As Object, ByRef r_vScreenDetails(,) As Object, ByVal lTemp As Integer, ByRef r_TabStrip As TabControl)
        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".addTabControl")

        Try

            r_lTabIndex = CInt(r_vScreenDetails(PBDatabaseConsts.ACDTabNumber, lTemp))

            If r_lTabIndex > 0 Then

                ReDim Preserve r_vTabArray(PBRiskScreenCommon.ACFLastArrayPosition, r_lTabIndex)
            Else
                ReDim r_vTabArray(PBRiskScreenCommon.ACFLastArrayPosition, r_lTabIndex)
            End If

            If r_lTabIndex = 0 Then
                'Modified by Alkesh Kumar on 10/05/2010 12:12:33 refer developer guide no. 81
                'r_TabStrip.TabPages.RemoveAt(0) 'remove default tab
                r_TabStrip.TabPages.Remove(r_TabStrip.TabPages(0)) 'remove default tab
            End If

            PBTabStripCommon.AddTab(r_TabStrip, r_lTabIndex, r_vScreenDetails(PBDatabaseConsts.ACDCaption, lTemp))

            'lErrorCount = 0


            r_vTabArray(PBRiskScreenCommon.ACFTabNumber, r_lTabIndex) = r_lTabIndex

            r_vTabArray(PBRiskScreenCommon.ACFIsDeleted, r_lTabIndex) = gPMConstants.PMEReturnCode.PMFalse


            r_vTabArray(PBRiskScreenCommon.ACFChildId, r_lTabIndex) = DBNull.Value


            r_vTabArray(PBRiskScreenCommon.ACFGISObjectId, r_lTabIndex) = DBNull.Value


            r_vTabArray(PBRiskScreenCommon.ACFTabSetIndex, r_lTabIndex) = r_vScreenDetails(PBDatabaseConsts.ACDTabSetIndex, lTemp)


            r_vTabArray(PBRiskScreenCommon.ACFTabSnapToGrid, r_lTabIndex) = r_vScreenDetails(PBDatabaseConsts.ACDPMFormat, lTemp)


            r_vTabArray(PBRiskScreenCommon.ACFHelpText, r_lTabIndex) = r_vScreenDetails(PBDatabaseConsts.ACDHelpText, lTemp) 'swift

            If Convert.IsDBNull(r_vTabArray(PBRiskScreenCommon.ACFTabSnapToGrid, r_lTabIndex)) Or IsNothing(r_vTabArray(PBRiskScreenCommon.ACFTabSnapToGrid, r_lTabIndex)) Then

                r_vTabArray(PBRiskScreenCommon.ACFTabSnapToGrid, r_lTabIndex) = 0
            End If

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".addTabControl")

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".addTabControl")


            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="addTabControl Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="addTabControl", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    '
    ' Name: addFrameControl
    '
    ' Description:
    '
    ' History: 07/01/2003 CLG - Created.
    '
    ' ***************************************************************** '
    Public Function addFrameControl(ByRef r_lFrameIndex As Integer, ByRef r_vFrameArray(,) As Object, ByRef r_vScreenDetails(,) As Object, ByVal lTemp As Integer, ByRef fraFrame As Object) As Integer

        ' Debug message
        Dim result As Integer = 0
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".addFrameControl")

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            r_lFrameIndex += 1
            If r_lFrameIndex > 0 Then
                'Modified by Alkesh Kumar on 10/05/2010 12:11:08 refer developer guide no. 
                'ContainerHelper.LoadControl(frmInterface, "fraFrame", r_lFrameIndex)
                'ContainerHelper.LoadControl(fraFrame(r_lFrameIndex).TopLevelControl, "fraFrame", r_lFrameIndex, True)
                ReDim Preserve r_vFrameArray(PBRiskScreenCommon.ACFLastArrayPosition, r_lFrameIndex)
            Else
                ReDim r_vFrameArray(PBRiskScreenCommon.ACFLastArrayPosition, r_lFrameIndex)
            End If

            r_vFrameArray(PBRiskScreenCommon.ACFDataModelType, r_lFrameIndex) = GISDataModelType.GISDMTypeRisk * 10000


            If Convert.IsDBNull(r_vScreenDetails(PBDatabaseConsts.ACDParentId, lTemp)) Or IsNothing(r_vScreenDetails(PBDatabaseConsts.ACDParentId, lTemp)) Then
                'Set fraFrame(r_lFrameIndex).Container = tabScreen
                'UPGRADE_WARNING: (1049) Use of Null/IsNull() detected. More Information: http://www.vbtonet.com/ewis/ewi1049.aspx
                'UPGRADE_WARNING: (1037) Couldn't resolve default property of object r_vFrameArray(ACFFrameNumber, r_lFrameIndex). More Information: http://www.vbtonet.com/ewis/ewi1037.aspx
                r_vFrameArray(PBRiskScreenCommon.ACFFrameNumber, r_lFrameIndex) = DBNull.Value
                fraFrame(r_lFrameIndex).Top = r_vScreenDetails(PBDatabaseConsts.ACDTop, lTemp) - Math.Round(330 / 15)
                fraFrame(r_lFrameIndex).Left = r_vScreenDetails(PBDatabaseConsts.ACDLeft, lTemp) - Math.Round(90 / 15)
            Else

                'ReflectionHelper.SetMember(fraFrame(r_lFrameIndex), "Container", fraFrame(r_vScreenDetails(PBDatabaseConsts.ACDParentId, lTemp)))
                'fraFrame(r_lFrameIndex).Container = fraFrame(r_vScreenDetails(ACDParentId, lTemp))
                fraFrame(r_vScreenDetails(ACDParentId, lTemp)).Controls.Add(fraFrame(r_lFrameIndex))
                Dim oGrpBox As GroupBox

                oGrpBox = fraFrame(r_vScreenDetails(PBDatabaseConsts.ACDParentId, lTemp))
                oGrpBox.SendToBack()

                oGrpBox = fraFrame(r_lFrameIndex)
                oGrpBox.BringToFront()
                'fraFrame(r_lFrameIndex).BringToFront()
                'fraFrame(r_vScreenDetails(ACDParentId, lTemp)).SendToBack()
                'UPGRADE_WARNING: (1068) r_vScreenDetails(ACDParentId, lTemp) of type Variant is being forced to Scalar. More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
                'UPGRADE_WARNING: (1037) Couldn't resolve default property of object r_vFrameArray(ACFFrameNumber, r_lFrameIndex). More Information: http://www.vbtonet.com/ewis/ewi1037.aspx
                r_vFrameArray(PBRiskScreenCommon.ACFFrameNumber, r_lFrameIndex) = r_vScreenDetails(PBDatabaseConsts.ACDParentId, lTemp)
                fraFrame(r_lFrameIndex).Top = r_vScreenDetails(PBDatabaseConsts.ACDTop, lTemp)
                fraFrame(r_lFrameIndex).Left = r_vScreenDetails(PBDatabaseConsts.ACDLeft, lTemp)
            End If



            'Modified by Alkesh Kumar on 10/05/2010 12:10:15 refer developer guide no. 8
            'fraFrame(r_lFrameIndex).Top = r_vScreenDetails(PBDatabaseConsts.ACDTop, lTemp) - Math.Round(330 / 15)
            'fraFrame(r_lFrameIndex).Top = VB6.TwipsToPixelsY(CDbl(r_vScreenDetails(PBDatabaseConsts.ACDTop, lTemp)))


            'Modified by Alkesh Kumar on 10/05/2010 12:10:07 refer developer guide no. 74
            'fraFrame(r_lFrameIndex).Left = r_vScreenDetails(PBDatabaseConsts.ACDLeft, lTemp) - Math.Round(90 / 15)
            'fraFrame(r_lFrameIndex).Left = VB6.TwipsToPixelsX(CDbl(r_vScreenDetails(PBDatabaseConsts.ACDLeft, lTemp)))


            'Modified by Alkesh Kumar on 10/05/2010 12:09:33 refer developer guide no. 74
            fraFrame(r_lFrameIndex).Height = r_vScreenDetails(PBDatabaseConsts.ACDHeight, lTemp)
            'fraFrame(r_lFrameIndex).Height = VB6.TwipsToPixelsY(CDbl(r_vScreenDetails(PBDatabaseConsts.ACDHeight, lTemp)))


            ''Modified by Alkesh Kumar on 10/05/2010 12:09:23 refer developer guide no. 74
            fraFrame(r_lFrameIndex).Width = r_vScreenDetails(PBDatabaseConsts.ACDWidth, lTemp)
            'fraFrame(r_lFrameIndex).Width = VB6.TwipsToPixelsX(CDbl(r_vScreenDetails(PBDatabaseConsts.ACDWidth, lTemp)))


            If CStr(r_vScreenDetails(PBDatabaseConsts.ACDCaption, lTemp)) = ACBlankCaption Then


                'fraFrame(r_lFrameIndex).Caption = ""
                fraFrame(r_lFrameIndex).Text = ""
            Else



                'fraFrame(r_lFrameIndex).Caption = r_vScreenDetails(PBDatabaseConsts.ACDCaption, lTemp)
                fraFrame(r_lFrameIndex).Text = r_vScreenDetails(PBDatabaseConsts.ACDCaption, lTemp)
            End If
            'Modified by Alkesh Kumar on 10/05/2010 11:54:22 refer developer guide no. 59
            fraFrame(r_lFrameIndex).AllowDrop = True

            fraFrame(r_lFrameIndex).Visible = True

            'Alkesh: To be handled later for ZOrder
            'fraFrame(r_lFrameIndex).ZOrder()


            fraFrame(r_lFrameIndex).Tag = gPMFunctions.NullToInteger(r_vScreenDetails(PBDatabaseConsts.ACDTabNumber, lTemp))


            r_vFrameArray(PBRiskScreenCommon.ACFTabNumber, r_lFrameIndex) = r_vScreenDetails(PBDatabaseConsts.ACDTabNumber, lTemp)

            r_vFrameArray(PBRiskScreenCommon.ACFIsDeleted, r_lFrameIndex) = gPMConstants.PMEReturnCode.PMFalse


            r_vFrameArray(PBRiskScreenCommon.ACFChildId, r_lFrameIndex) = DBNull.Value


            r_vFrameArray(PBRiskScreenCommon.ACFGISObjectId, r_lFrameIndex) = r_vScreenDetails(PBDatabaseConsts.ACDGISObjectId, lTemp)


            r_vFrameArray(PBRiskScreenCommon.ACFTabSetIndex, r_lFrameIndex) = r_vScreenDetails(PBDatabaseConsts.ACDTabSetIndex, lTemp)


            r_vFrameArray(PBRiskScreenCommon.ACFHelpText, r_lFrameIndex) = r_vScreenDetails(PBDatabaseConsts.ACDHelpText, lTemp) 'swift


            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".addFrameControl")

            Return result

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".addFrameControl")


            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="addFrameControl Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="addFrameControl", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: GetTag
    '
    ' Description:
    '
    ' History: 28/06/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    'Akash : 
    'Public Function GetTag(ByRef lGISObjectId As Integer, ByRef lGISPropertyId As Integer, ByRef lTag As Integer, ByRef r_vDataDictionary( ,  ) As Integer) As Integer
    'Modified by Alkesh Kumar on 6/2/2010 1:17:30 PM refer developer guide no. 
    'Public Function GetTag(ByRef lGISObjectId As Integer, ByRef lGISPropertyId As Integer, ByRef lTag As Integer, ByRef r_vDataDictionary As Object) As Integer
    Public Function GetTag(ByRef lGISObjectId As Integer, ByRef lGISPropertyId As Integer, ByRef lTag As Integer, ByRef r_vDataDictionary As Object, Optional ByVal ACOGISObjectId As Integer = 0, Optional ByVal ACPGISPropertyId As Integer = 0) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lTag = -1

            If Not Information.IsArray(r_vDataDictionary) Then
                Return result
            End If

            For lTemp As Integer = r_vDataDictionary.GetLowerBound(1) To r_vDataDictionary.GetUpperBound(1)
                If lGISPropertyId = -1 Then

                    If CDbl(r_vDataDictionary(ACOGISObjectId, lTemp)) = lGISObjectId Then
                        lTag = lTemp
                        Exit For
                    End If
                Else

                    If Not (Convert.IsDBNull(r_vDataDictionary(ACPGISPropertyId, lTemp)) Or IsNothing(r_vDataDictionary(ACPGISPropertyId, lTemp))) Then

                        If CStr(r_vDataDictionary(ACPGISPropertyId, lTemp)) <> "" AndAlso CStr(r_vDataDictionary(ACOGISObjectId, lTemp)) <> "" Then

                            If CDbl(r_vDataDictionary(ACPGISPropertyId, lTemp)) = lGISPropertyId AndAlso CDbl(r_vDataDictionary(ACOGISObjectId, lTemp)) = lGISObjectId Then
                                lTag = lTemp
                                Exit For
                            End If
                        End If
                    End If
                End If
            Next lTemp

            If lTag = -1 Then
            End If
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTag Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTag", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function



    ' ***************************************************************** '
    '
    ' Name: IsInputControl
    '
    ' Description: Return false if control is not for input.
    '
    ' History: 20/03/2003 Chris Ridgard - Created.
    '
    ' ***************************************************************** '
    Public Function IsInputControl(ByRef r_oCtrl As Object, ByVal v_vDataDictionary() As Object, Optional ByVal InputPropertyIndex As Integer = 0) As Boolean
        'default to true for controls with no tag
        Dim result As Boolean = False
        result = True
        With r_oCtrl

            If Conversion.Val(.Tag) > 0 Then



                'result = (CDbl(v_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACPIsInputProperty, .Tag Mod 10000)) = 1)
                If InputPropertyIndex = 0 Then
                    result = (CDbl(v_vDataDictionary(GISDataModelType.GISDMTypeRisk)(ACPIsInputProperty, .Tag Mod 10000)) = 1)

                Else
                    result = (CDbl(v_vDataDictionary(GISDataModelType.GISDMTypeRisk)(InputPropertyIndex, .Tag Mod 10000)) = 1)

                End If
            End If
        End With
        Return result
    End Function
	
	'PRIVATE Methods (Begin)
	
	' ***************************************************************** '
	' Name: DisableForm
	'
	' Description: Sets all of the interface details to the disable
	'              state passed.
	'
	' ***************************************************************** '
	Public Function DisableForm(ByRef lDisabled As Integer, ByRef vForm As Object) As Integer
		
		Dim result As Integer = 0
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Set all of the forms controls to the disable state.

			For	Each ctlFormControl As Control In vForm.Controls
				' Check the type of the control.
				If (TypeOf ctlFormControl Is TextBox) Or (TypeOf ctlFormControl Is ComboBox) Or (TypeOf ctlFormControl Is CheckBox) Then
					ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
				End If
#If CodeVariant = "Swift" Then

				If TypeOf ctlFormControl Is UEditDate Then
				ctlFormControl.Enabled = Not lDisabled&
				End If
#End If
				
			Next ctlFormControl
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the form disabling", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	
	Public Sub SetBlankCaption(ByRef r_cLabel As Control, ByVal bInScreenEdit As Boolean)
		If bInScreenEdit Then
			r_cLabel.ForeColor = IIf(r_cLabel.Text = ACBlankCaption, ColorTranslator.ToOle(SystemColors.GrayText), ColorTranslator.ToOle(SystemColors.ControlText))
		Else
			ControlHelper.SetVisible(r_cLabel, Not (r_cLabel.Text = ACBlankCaption))
		End If
	End Sub
End Module

