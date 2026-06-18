Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
<System.Runtime.InteropServices.ProgId("gArrays_NET.gArrays")> _
 Public Module gArrays
	
	'Determines which array dimension contains rows and columns
	Public Const klColDimension As Integer = 1
	Public Const klRowDimension As Integer = 2
	
	Public Function IsArrayDimensioned(ByRef r_vArray As Object) As Integer
		' ---------------------------------------------------------------------------
		' PROCEDURE NAME: IsArrayDimensioned
		' PURPOSE: Determines if an array has been dimensioned
		' AUTHOR: Paul Cunnigham
		' DATE: 21 October 2002, 09:56:45
		' RETURNS: PMTrue for success
		' CHANGES:
		' ---------------------------------------------------------------------------
		
		Dim result As Integer = 0
		Dim lLower As Integer
		
		
		Try
		
		'This will force an error if array is not dimensioned

		lLower = r_vArray.GetLowerBound(0)
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		
		'----------------------------------------------------------------------------------------
		'Only for Debugging, the code will never execute this line
		'----------------------------------------------------------------------------------------
	 
		
		Catch ex As Exception
		result = gPMConstants.PMEReturnCode.PMFalse
		
		Finally
	
		
		
		End Try
		Return result
	End Function
	
	Public Function GetArrayBounds(ByRef r_vArray As Object, ByRef r_lDimension As Integer, ByRef r_lLower As Integer, ByRef r_lUpper As Integer) As Boolean
		
		' ---------------------------------------------------------------------------
		' PROCEDURE NAME: GetArrayBounds
		' PURPOSE: Gets the lower and upper bounds of an array
		' AUTHOR: Sirius Financial Systems Plc
		' OUT: r_lLower - id of the first element (or -1)
		'      r_lUpper - id of the last element  (or -1)
		' RETURNS: True if array dimensioned, false otherwise
		' DATE: 08 October 2002, 15:03:56
		' CHANGES:
		' ---------------------------------------------------------------------------
		

		Try 
			
			'Default the values
			r_lLower = -1
			r_lUpper = -1
			
			'Get array limits

			r_lLower = r_vArray.GetLowerBound(r_lDimension - 1)

			r_lUpper = r_vArray.GetUpperBound(r_lDimension - 1)
			
			'Determine if the array has been dimensioned
			If r_lUpper = -1 And r_lUpper = -1 Then
				Information.Err().Clear()
				Return False
			Else
				Return True
			End If
		
		Catch exc As System.Exception
            'NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
		End Try
		
	End Function
End Module
