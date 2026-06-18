Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports System
Imports System.Diagnostics
Imports System.Windows.Forms
<System.Runtime.InteropServices.ProgId("uctPMResizer_NET.uctPMResizer")> _
Public Partial Class uctPMResizer
	Inherits System.Windows.Forms.UserControl
	
	Public Enum PMEControlResizeOptions
		pmeCRONormal = 0
		pmeCRONoResize = 1
		pmeCROPositionOnly = 2
		pmeCROTopOnly = 3
		pmeCROLeftOnly = 4
		pmeCROSizeOnly = 5
		pmeCROHeightOnly = 6
		pmeCROWidthOnly = 7
	End Enum
	
	Public Enum PMEControlResizeTypes
		pmeCRTProportional = 0
		pmeCRTRelativeToBottomRight = 1
	End Enum
	
	' if True, also fonts are resized
	Public ResizeFont As Boolean
	' if True, form's height/width ratio is preserved
	Public KeepRatio As Boolean
	
	Public FormMinWidth As Integer
	Public FormMinHeight As Integer
	
	Public NoResizeByDefault As Boolean
	
	Private Structure TControlInfo
		Dim ctrl As Control
		Dim Left_Renamed As Single
		Dim Top_Renamed As Single
		Dim Width_Renamed As Single
		Dim Height_Renamed As Single
		Dim DistanceFromRight As Single
		Dim DistanceFromBottom As Single
		Dim FontSize As Single
		Dim ResizeOption As PMEControlResizeOptions
		Dim ResizeType As PMEControlResizeTypes
		Public Shared Function CreateInstance() As TControlInfo
			Dim result As New TControlInfo
			Return result
		End Function
	End Structure
	
	' this array holds the original position
	' and size of all controls on parent form
	Dim Controls_Renamed() As TControlInfo = Nothing
	
	' a reference to the parent form
	Private WithEvents ParentForm_Renamed As Form
	' parent form's size at load time
	Private ParentWidth As Single
	Private ParentHeight As Single
	' ratio of original height/width
	Private HeightWidthRatio As Single
	
	Private Sub ParentForm_Renamed_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ParentForm_Renamed.Load
		' the ParentWidth variable works as a flag
		ParentWidth = 0
		' save original ratio
		HeightWidthRatio = VB6.PixelsToTwipsY(ParentForm_Renamed.Height) / VB6.PixelsToTwipsX(ParentForm_Renamed.Width)
		
		' If the specified Minimum dimensions of the Form are bigger than
		' the Initial dimensions then use the initial dimensions as the minimum.
		
		' If a Min Height has been specified
		If FormMinHeight > 0 Then
			' Check that the Min Height is NOT greater than the initial Height
			If FormMinHeight > VB6.PixelsToTwipsY(ParentForm_Renamed.Height) Then
				FormMinHeight = CInt(VB6.PixelsToTwipsY(ParentForm_Renamed.Height))
			End If
		End If
		' If a Min Width has been specified
		If FormMinWidth > 0 Then
			' Check that the Min Width is NOT greater than the initial Width
			If FormMinWidth > VB6.PixelsToTwipsX(ParentForm_Renamed.Width) Then
				FormMinWidth = CInt(VB6.PixelsToTwipsX(ParentForm_Renamed.Width))
			End If
		End If
		
	End Sub
	


    'developer guide no solution no. 1
    Private Sub UserControl_ReadProperties(ByRef PropBag As Object)
        If DesignMode Then Exit Sub
        ' store a reference to the parent form and
        ' start receiving events

        ParentForm_Renamed = FindForm()
    End Sub
	
	Private Sub uctPMResizer_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
		' refuse to resize
		Image1.SetBounds(0, 0, 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
		MyBase.Width = Image1.Width
		MyBase.Height = Image1.Height
	End Sub
	
	' trap the parent form's Resize event
	' this include the very first resize event
	' that occurs soon after form's load
	
	Private isInitializingComponent As Boolean
	Private Sub ParentForm_Renamed_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ParentForm_Renamed.Resize
		If isInitializingComponent Then
			Exit Sub
		End If
		Debug.WriteLine("Control Resize")
		If ParentWidth = 0 Then
			SaveControls()
		Else
			RefreshControls()
		End If
	End Sub
	
	' save size and position of all controls on parent form
	' you should manually invoke this method each time you add a new control
	' to the form (through Load method of a control array)
	
	Public Sub SaveControls()
		' Rebuild the internal table
		Dim ctrl As Control
		' this is necessary for controls that don't support
		' all properties (e.g. Timer controls)

		Try 
			
			If DesignMode Then Exit Sub
			
			' save a reference to the parent form, and its initial size

			ParentForm_Renamed = MyBase.FindForm()
			ParentWidth = VB6.PixelsToTwipsX(ParentForm_Renamed.ClientRectangle.Width)
			ParentHeight = VB6.PixelsToTwipsY(ParentForm_Renamed.ClientRectangle.Height)
			
			' read the position of all controls on the parent form


			ReDim Controls_Renamed(ContainerHelper.Controls(ParentForm_Renamed).Count - 1)
			


			For i As Integer = 0 To ContainerHelper.Controls(ParentForm_Renamed).Count - 1

                ctrl = ParentForm_Renamed.Controls(i)
				With Controls_Renamed(i)
					.ctrl = ctrl
					.Left_Renamed = VB6.PixelsToTwipsX(ctrl.Left)
					.Top_Renamed = VB6.PixelsToTwipsY(ctrl.Top)
					.Width_Renamed = VB6.PixelsToTwipsX(ctrl.Width)
					.Height_Renamed = VB6.PixelsToTwipsY(ctrl.Height)
					.FontSize = ctrl.Font.SizeInPoints

					.DistanceFromRight = VB6.PixelsToTwipsX(.ctrl.FindForm().ClientRectangle.Width) - (.Left_Renamed + .Width_Renamed)

					.DistanceFromBottom = VB6.PixelsToTwipsY(.ctrl.FindForm().ClientRectangle.Height) - (.Top_Renamed + .Height_Renamed)
					
					' Set the default resize option
					If NoResizeByDefault Then
						.ResizeOption = PMEControlResizeOptions.pmeCRONoResize
					Else
						.ResizeOption = PMEControlResizeOptions.pmeCRONormal
					End If
					
					' Set the default Resize Type
					.ResizeType = PMEControlResizeTypes.pmeCRTProportional
					
				End With
			Next 
			
			ctrl = Nothing
		
		Catch exc As System.Exception
            'developers guid 32
            'NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
		End Try
		
	End Sub
	
	' update size and position of controls on parent form
	
	Public Sub RefreshControls()

		Dim minFactor As Single
		Dim bResizeHeight As Boolean
		
		' inhibits recursive calls if KeepRatio = True
		Static executing As Boolean
		If executing Then Exit Sub
		
		If DesignMode Then Exit Sub
		
		' If the Window is NOT Minimized
		If ParentForm_Renamed.WindowState <> FormWindowState.Minimized Then
			' Check that the Form is not smaller than the minimum allowed.
			If VB6.PixelsToTwipsY(ParentForm_Renamed.Height) < FormMinHeight Then
				ParentForm_Renamed.Height = VB6.TwipsToPixelsY(FormMinHeight)
			End If
			If VB6.PixelsToTwipsX(ParentForm_Renamed.Width) < FormMinWidth Then
				ParentForm_Renamed.Width = VB6.TwipsToPixelsX(FormMinWidth)
			End If
		End If
		
		If (KeepRatio) And (ParentForm_Renamed.WindowState = FormWindowState.Normal) Then
			executing = True
			' we must keep original ratio
			ParentForm_Renamed.Height = VB6.TwipsToPixelsY(HeightWidthRatio * VB6.PixelsToTwipsX(ParentForm_Renamed.Width))
			executing = False
		End If
		
		' this is necessary for controls that don't support
		' all properties (e.g. Timer controls)

		Try 
			
			Dim widthFactor As Single = VB6.PixelsToTwipsX(ParentForm_Renamed.ClientRectangle.Width) / ParentWidth
			Dim heightFactor As Single = VB6.PixelsToTwipsY(ParentForm_Renamed.ClientRectangle.Height) / ParentHeight
			' take the lesser of the two
			If widthFactor < heightFactor Then
				minFactor = widthFactor
			Else
				minFactor = heightFactor
			End If
			
			' this is a regular resize
			For	Each Controls_Renamed_item As TControlInfo In Controls_Renamed
				With Controls_Renamed_item
					' Does the Control Require Resize
					If .ResizeOption = PMEControlResizeOptions.pmeCRONoResize Then
						' No, so do nothing
					Else
						' the change of font must occur *before* the resizing
						' to account for companion scrollbar of listbox
						' and other similar controls
						If ResizeFont Then
							.ctrl.Font = VB6.FontChangeSize(.ctrl.Font, .FontSize * minFactor)
						End If
						
						' If we do not want to Position the Control
						If (.ResizeOption = PMEControlResizeOptions.pmeCROSizeOnly) Or (.ResizeOption = PMEControlResizeOptions.pmeCROHeightOnly) Or (.ResizeOption = PMEControlResizeOptions.pmeCROWidthOnly) Then
							' Do Nothing
						Else
							' move and resize the controls - we can't use a Move
							' method because some controls do not support the change
							' of all the four properties (e.g. Height with comboboxes)
							
							' If Left Only
							If .ResizeOption = PMEControlResizeOptions.pmeCROLeftOnly Then
								' Do Nothing
							Else
								' If Proprtional
								If .ResizeType = PMEControlResizeTypes.pmeCRTProportional Then
									' Multiply top and left by relevant factor
									.ctrl.Top = VB6.TwipsToPixelsY(.Top_Renamed * heightFactor)
								Else
									' Otherwise, Keep at the original distance from the Bottom/Right
									.ctrl.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(ParentForm_Renamed.ClientRectangle.Height) - VB6.PixelsToTwipsY(.ctrl.Height) - .DistanceFromBottom)
								End If
								
							End If
							
							' If Top Only
							If .ResizeOption = PMEControlResizeOptions.pmeCROTopOnly Then
							Else
								' If Proprtional
								If .ResizeType = PMEControlResizeTypes.pmeCRTProportional Then
									' Multiply top and left by relevant factor
									.ctrl.Left = VB6.TwipsToPixelsX(.Left_Renamed * widthFactor)
								Else
									' Otherwise, Keep at the original distance from the Bottom/Right
									.ctrl.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(ParentForm_Renamed.ClientRectangle.Width) - VB6.PixelsToTwipsX(.ctrl.Width) - .DistanceFromRight)
								End If
							End If
							
						End If
						
						' If we do not want to Size the Control
						If (.ResizeOption = PMEControlResizeOptions.pmeCROPositionOnly) Or (.ResizeOption = PMEControlResizeOptions.pmeCROTopOnly) Or (.ResizeOption = PMEControlResizeOptions.pmeCROLeftOnly) Then
							' Do Nothing
						Else
							' If Size Height Only
							If .ResizeOption = PMEControlResizeOptions.pmeCROHeightOnly Then
								' Do nothing
							Else
								' Set the Control Width
								' If Proportional
								If .ResizeType = PMEControlResizeTypes.pmeCRTProportional Then
									' Multiply by the Factor
									.ctrl.Width = VB6.TwipsToPixelsX(.Width_Renamed * widthFactor)
								Else
									' Size to fill the space
									.ctrl.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(ParentForm_Renamed.ClientRectangle.Width) - VB6.PixelsToTwipsX(.ctrl.Left) - .DistanceFromRight)
								End If
							End If
							
							bResizeHeight = True
							
							' If Size Width Only
							If .ResizeOption = PMEControlResizeOptions.pmeCROWidthOnly Then
								' Do not resize Height
								bResizeHeight = False
							End If
							
							' Is the Control a Text Box
							If TypeOf .ctrl Is TextBox Then
								' Only set the height if it is MultiLine

                                'NIIT - Replaced with the Migrated code 1144 
                                'If Not .ctrl.MultiLine Then
                                If Not ReflectionHelper.GetMember(.ctrl, "MultiLine") Then
                                    bResizeHeight = False
                                End If
                            End If

                            If bResizeHeight Then
                                ' Set the Control Height
                                If .ResizeOption = PMEControlResizeOptions.pmeCRONormal Then
                                    .ctrl.Height = VB6.TwipsToPixelsY(.Height_Renamed * heightFactor)
                                Else
                                    .ctrl.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(ParentForm_Renamed.ClientRectangle.Height) - VB6.PixelsToTwipsY(.ctrl.Top) - .DistanceFromBottom)
                                End If
                            End If
							
						End If
					End If
				End With
			Next Controls_Renamed_item
			
			' Call the Parent Form Custom Resize Method, in case there is
			' any specific tweaking to be done after we have done our bit.

            'NIIT - Replaced with the Migrated code 1144 
            'ParentForm_Renamed.FormCustomResize()
            ReflectionHelper.Invoke(ParentForm_Renamed, "FormCustomResize", New Object() {})
		
		Catch exc As System.Exception
			NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
		End Try
		
	End Sub
	
	' ***************************************************************** '
	' Name: SetControlResizeOption
	'
	' Description:
	'
	'
	' ***************************************************************** '
    Public Sub SetControlResizeOption(ByVal v_sControlName As String, ByVal v_lResizeOption As Integer, ByVal v_lResizeType As Integer, Optional ByVal v_lControlArrayIndex As Integer = -1)

        Dim iIndex As Integer

        Try

            If DesignMode Then Exit Sub

            If ParentWidth = 0 Then
                Exit Sub
            End If

            ' Find the Control In Question
            For Each Controls_Renamed_item As TControlInfo In Controls_Renamed
                With Controls_Renamed_item
                    ' If the Name Matches
                    If v_sControlName.Trim() = .ctrl.Name.Trim() Then
                        Try
                            iIndex = -1
                            iIndex = ContainerHelper.GetControlIndex(.ctrl)
                        Catch ex As Exception

                        End Try

                        ' IF the Control is NOT a Control Array (Single Control)
                        ' OR a Control Array Index has not been specified (Do all Controls in Array)
                        ' OR the Control Array Index matches (Do specific Control in Array)
                        If (iIndex = -1) Or (v_lControlArrayIndex = -1) Or (v_lControlArrayIndex = iIndex) Then
                            ' Save the Resize Option
                            Select Case v_lResizeOption
                                Case PMEControlResizeOptions.pmeCRONoResize
                                    .ResizeOption = PMEControlResizeOptions.pmeCRONoResize
                                Case PMEControlResizeOptions.pmeCROPositionOnly
                                    .ResizeOption = PMEControlResizeOptions.pmeCROPositionOnly
                                Case PMEControlResizeOptions.pmeCROTopOnly
                                    .ResizeOption = PMEControlResizeOptions.pmeCROTopOnly
                                Case PMEControlResizeOptions.pmeCROLeftOnly
                                    .ResizeOption = PMEControlResizeOptions.pmeCROLeftOnly
                                Case PMEControlResizeOptions.pmeCROSizeOnly
                                    .ResizeOption = PMEControlResizeOptions.pmeCROSizeOnly
                                Case PMEControlResizeOptions.pmeCROHeightOnly
                                    .ResizeOption = PMEControlResizeOptions.pmeCROHeightOnly
                                Case PMEControlResizeOptions.pmeCROWidthOnly
                                    .ResizeOption = PMEControlResizeOptions.pmeCROWidthOnly
                                Case Else
                                    .ResizeOption = PMEControlResizeOptions.pmeCRONormal
                            End Select
                            ' Save the Resize Type
                            Select Case v_lResizeType
                                Case PMEControlResizeTypes.pmeCRTProportional
                                    .ResizeType = PMEControlResizeTypes.pmeCRTProportional
                                Case PMEControlResizeTypes.pmeCRTRelativeToBottomRight
                                    .ResizeType = PMEControlResizeTypes.pmeCRTRelativeToBottomRight
                                Case Else
                                    .ResizeType = PMEControlResizeTypes.pmeCRTProportional
                            End Select

                        End If
                    End If
                End With
            Next Controls_Renamed_item

        Catch ex As Exception

        End Try


    End Sub
End Class
