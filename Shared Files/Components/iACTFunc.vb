Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports VB6 = Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
<System.Runtime.InteropServices.ProgId("iACTFunc_NET.iACTFunc")> _
 Public Module iACTFunc
	' ***************************************************************** '
	'
	' Description: System wide calls to functions in support of
	'              user interface programs.
	'
	' Edit History:
	' ***************************************************************** '
	Const ACClass As String = "iACTFunc"

	Public Function SetListIndex(ByVal cList As Control, ByVal lItemData As Integer) As Integer
		Dim result As Integer = 0
		
		Try 
			result = gPMConstants.PMEReturnCode.PMTrue
			

            'NIIT-Modified code for ComboBox
            If TypeOf cList Is ComboBox Then
                For lIndex As Integer = 0 To CInt(CType(cList, ComboBox).Items.Count - 1)

                    If CType(cList, ComboBox).Items(lIndex).ItemData = lItemData Then

                        CType(cList, ComboBox).SelectedIndex = lIndex
                        Exit For
                    End If
                Next lIndex
            Else
                For lIndex As Integer = 0 To CInt(ReflectionHelper.GetMember(cList, "ListCount") - 1)

                    If ReflectionHelper.Invoke(cList, "ItemData", New Object() {lIndex}) = lItemData Then

                        ReflectionHelper.SetMember(cList, "ListIndex", lIndex)
                        Exit For
                    End If
                Next lIndex
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="SetListIndex", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
	End Function
	' Function to position a Child form in a cascaded fashion
	
	Public Function SetChildFormPosition(ByRef frmParent As Form, ByRef frmChild As Form) As Integer
		
		Dim result As Integer = 0
		Const CStandardOffset As Integer = 600 ' Twips
		
		Dim lHorizontalOffset As Integer = 0
		If (VB6.PixelsToTwipsX(frmParent.Left) + CStandardOffset + VB6.PixelsToTwipsX(frmChild.Width)) <= VB6.PixelsToTwipsX(Screen.PrimaryScreen.Bounds.Width) Then
			lHorizontalOffset = CStandardOffset
		Else
			If (VB6.PixelsToTwipsX(frmParent.Left) - CStandardOffset) >= 0 Then
				lHorizontalOffset = -CStandardOffset
			End If
		End If
		
		Dim lVerticalOffset As Integer = 0
		If (VB6.PixelsToTwipsY(frmParent.Top) + CStandardOffset + VB6.PixelsToTwipsY(frmChild.Height)) <= VB6.PixelsToTwipsY(Screen.PrimaryScreen.Bounds.Height) Then
			lVerticalOffset = CStandardOffset
		Else
			If (VB6.PixelsToTwipsY(frmParent.Top) - CStandardOffset) >= 0 Then
				lVerticalOffset = -CStandardOffset
			End If
		End If
		
		frmChild.Left = frmParent.Left + VB6.TwipsToPixelsX(lHorizontalOffset)
		frmChild.Top = frmParent.Top + VB6.TwipsToPixelsY(lVerticalOffset)
		
		Return gPMConstants.PMEReturnCode.PMTrue
		
		
		
		' Error Section.
		
		result = gPMConstants.PMEReturnCode.PMError
		
		' Log Error.
		iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed position child form", vApp:=ACApp, vClass:=ACClass, vMethod:="SetListIndex", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
		
		Return result
		
	End Function
	
	
	' ***********************************************************************
	'
	' Function: VBsprintf(...)
	'
	' Desc.   : VB equivalent of the C sprintf function
	'
	' Note    : Target is likely to be bigger than Source when finished.
	'
	' History : 150499 - CField - Put into iACTFunc
	'
	' ***********************************************************************
	Public Function VBsprintf(ByRef sTarget As String, ByRef sSource As String, ParamArray ByVal vParams() As Object) As Integer
		
		Dim result As Integer = 0
		Dim iLen, iCurrParam As Integer
        Dim sByte1 As New VB6.FixedLengthString(1)
        Dim sByte2 As New VB6.FixedLengthString(1)
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			sTarget = ""
			
			iCurrParam = vParams.GetLowerBound(0)
			
			iLen = sSource.Length
			
			For iLoop1 As Integer = 1 To iLen
				sByte1.Value = sSource.Substring(iLoop1 - 1, 1)
				
				
				Select Case sByte1.Value
					Case "%"
						If iLoop1 < iLen Then
							iLoop1 += 1
							sByte2.Value = sSource.Substring(iLoop1 - 1, 1)
							
							Select Case sByte2.Value
								Case "s" ' String

									sTarget = sTarget & CStr(vParams(iCurrParam))
									iCurrParam += 1
								Case "c" ' Char

									sTarget = sTarget & CStr(vParams(iCurrParam))
									iCurrParam += 1
								Case "d" ' Decimal number

									sTarget = sTarget & CStr(vParams(iCurrParam))
									iCurrParam += 1
							End Select
							
						End If
						
					Case "\"
						If iLoop1 < iLen Then
							iLoop1 += 1
							sByte2.Value = sSource.Substring(iLoop1 - 1, 1)
							
							Select Case sByte2.Value
								Case "n" ' New line
									sTarget = sTarget & Environment.NewLine
								Case "r" ' Return feed
									sTarget = sTarget & Strings.Chr(10).ToString()
								Case "t" ' Tab
									sTarget = sTarget & Strings.Chr(9).ToString()
								Case "\" ' Forward Slash
									sTarget = sTarget & "\"
							End Select
						End If
						
					Case Else
						sTarget = sTarget & sByte1.Value
						
				End Select
				
			Next iLoop1
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sprintf", vApp:=ACApp, vClass:=ACClass, vMethod:="VBsprintf", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
		End Try
	End Function
End Module