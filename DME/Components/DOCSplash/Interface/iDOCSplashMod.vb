Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: {17/2/98}
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iDOCSplash"
	
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	
	
	' {* USER DEFINED CODE (End) *}
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	Private m_lreturn As gPMConstants.PMEReturnCode
	
	' Indicat finis
	
	Sub Main_Renamed()
		'test code

		
        Dim i As New Interface_Renamed
		Dim a As String = CStr(1)
a: 
		
		m_lreturn = CType(i.Show(CInt(a)), gPMConstants.PMEReturnCode)
		a = Interaction.InputBox("again")
		
		If a <> "`" Then
			m_lreturn = CType(i.Hide(), gPMConstants.PMEReturnCode)
			GoTo a
		End If
		
	End Sub
End Module