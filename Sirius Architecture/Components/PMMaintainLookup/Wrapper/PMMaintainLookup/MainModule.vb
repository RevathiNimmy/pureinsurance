Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
' developer guide no. 129
Imports SharedFiles
Module MainModule
	
	' Name of the application
	Public Const ACApp As String = "MaintainLookupsStub"
	
	' Name of this module
	Private Const ACClass As String = "MainModule"
	
	' Return value
	Private m_lReturn As gPMConstants.PMEReturnCode
	

	Public Sub main()
		

        ' developer guide no. 88
        Dim oMaintainLookupWrapper As Object
		
		' Get an instance of object manager
		Dim oObjectManager As New bObjectManager.ObjectManager
		
		' and Initialise it
		m_lReturn = oObjectManager.Initialise(sCallingAppName:=ACApp)
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			If m_lReturn <> gPMConstants.PMEReturnCode.PMCancel Then
				MessageBox.Show("Failed to get instance of bObjectManager.ObjectManager", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
			End If
			oObjectManager = Nothing
			Environment.Exit(0)
		End If
		
		' Get an instance of the wrapper interface
		Dim temp_oMaintainLookupWrapper As Object
		m_lReturn = oObjectManager.GetInstance(temp_oMaintainLookupWrapper, sClassName:="iPMMaintainLookupWrapper.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
		oMaintainLookupWrapper = temp_oMaintainLookupWrapper
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			MessageBox.Show("Failed to get instance of iPMMaintainLookupWrapper.Interface", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
			' Clear up
            oObjectManager.Dispose()
			oObjectManager = Nothing
			Environment.Exit(0)
		End If
		
		' Start it

		m_lReturn = oMaintainLookupWrapper.Start()
		If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMMNoAccess) Then 'PN19406
			MessageBox.Show("Failed to start iPMMaintainLookupWrapper", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End If
		
		' Terminate it

		oMaintainLookupWrapper.Dispose()
		
		oMaintainLookupWrapper = Nothing
		
		' Clear up
		oObjectManager.Dispose()
		oObjectManager = Nothing
		
	End Sub
End Module