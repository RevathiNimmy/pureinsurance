Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports System
Imports SharedFiles
Module MainModule
	
	Private m_lReturn As gPMConstants.PMEReturnCode
	

	Public Sub Main()
		
        'developer guide no. 88
        Dim oObject As Object
		

		m_lReturn = oObject.Initialise
		
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			oObject = Nothing
			Exit Sub
		End If
		

		m_lReturn = oObject.Start
		
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			oObject = Nothing
			Exit Sub
		End If
		

		oObject.Dispose()
        oObject = Nothing
		
	End Sub
End Module