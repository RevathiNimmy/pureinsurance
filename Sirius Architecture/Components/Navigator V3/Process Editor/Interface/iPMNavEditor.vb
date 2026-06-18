Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
 Public Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 05/08/1998
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
    'developer guide no. 50
    Dim objfrmInterface As New frmInterface
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iPMNavEditor"
	
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Public source and language ID's from the
	' Object Manager.
	Public g_iSourceID As Integer
	Public g_iLanguageID As Integer
	
	' Public instance of the object manager.
	Public g_oObjectManager As bObjectManager.ObjectManager
	
	' Public instance of interface object
    Public g_oInterface As Interface_Renamed
	
	'All keys in the system
	Public g_bSystemKeysLoaded As Boolean
	Public g_cSystemKeys As Collection
	
	
	Public Sub CloseInterface()
		
		
		'Terminate the interface object
		g_oInterface.Dispose()
		
		'Unload the form
        'developer guide no. 50
        objfrmInterface.Close()

        'Set everthing to nothing
        'developer guide no. 50
        objfrmInterface = Nothing
        g_oInterface = Nothing
        Environment.Exit(0)
		
	End Sub
	

	Public Sub Main()
		
		
		'Create a new instance of the interface object
        g_oInterface = New Interface_Renamed()
		
		'Initialise it
		Dim m_lReturn As Integer = CType(g_oInterface, SSP.S4I.Interfaces.ILocalInterface).Initialise()
		
		If m_lReturn = 0 Then
			Exit Sub
		End If
		
		'Start the interface
		m_lReturn = g_oInterface.SetProcessModes(vTask:=1)
		m_lReturn = g_oInterface.Start()
		g_oInterface.Dispose()
		
	End Sub
	
	' ***************************************************************** '
	' Module Name: SubStr
	'
	' Date: 15/01/1999
	'
	' Description: Function to return a substring
	'
	' ***************************************************************** '
	Public Function SubStr(ByRef sString As String, ByRef iStrNum As Integer, Optional ByRef sSep As String = "") As String
		
		Dim result As String = String.Empty
		Dim iStartPos, iEndPos, iPos As Integer
		
		Try 
			
			result = ""
			
			If iStrNum = 0 Then
				Return result
			End If
			
			'Use comma if separator is missing
			If False Then
				sSep = ","
			End If
			
			iStartPos = 1
			iEndPos = 0
			iPos = 1
			
			For iPtr As Integer = 1 To iStrNum
				
				iPos = Strings.InStr(iPos, sString, sSep)
				
				iStartPos = iEndPos + 1
				
				iEndPos = iPos
				
				If iEndPos = 0 Then
					iPos = sString.Length + 1
					iEndPos = iPos
				End If
				
				iPos += 1
				
			Next iPtr
			
			'Get the sub string position
			
			Return Mid(sString, iStartPos, iEndPos - iStartPos)
		
		Catch 
			
			
			Return result
		End Try
	End Function
End Module