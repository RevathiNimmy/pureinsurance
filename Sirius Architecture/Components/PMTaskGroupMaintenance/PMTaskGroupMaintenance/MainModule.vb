Option Strict Off
Option Explicit On
Imports System
'Developr Guide no 129
Imports SharedFiles
Module MainModule
	
	Public Const ACApp As String = "PMTaskGroupMaintenance"
	

	Public Sub Main()
		
        'developer guide no.244(latest)
		Dim oObject As New iPMTaskGroupMaintenance.Interface_Renamed
        'Developer GUide no. 9
        Dim lReturn As gPMConstants.PMEReturnCode = oObject.Initialise()
		
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			oObject = Nothing
			Exit Sub
		End If
		
		lReturn = oObject.Start()
		
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			oObject = Nothing
			Exit Sub
		End If
		
		oObject.Dispose()
        oObject = Nothing
		
	End Sub
End Module