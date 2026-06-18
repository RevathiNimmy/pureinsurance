Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Module MainModule
	
	' Name of the application
	Public Const ACApp As String = "SourceMaintenanceStub"
	
	' Name of this module
	Private Const ACClass As String = "MainModule"
	
	' Return value
	Private m_lReturn As gPMConstants.PMEReturnCode
	

	Public Sub Main()


		
		' Get an instance of object manager
        Dim oObjectManager As New bObjectManager.ObjectManager
		
		' and Initialise it
        m_lReturn = oObjectManager.Initialise(sCallingAppName:=ACApp)
        If m_lReturn = gPMConstants.PMEReturnCode.PMMAlreadyInUse Then
            Exit Sub
        End If
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            If m_lReturn <> gPMConstants.PMEReturnCode.PMCancel Then
                MessageBox.Show("Failed to get instance of bObjectManager.ObjectManager", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
            oObjectManager = Nothing
            Environment.Exit(0)
        End If

        'developer guide no. 108
        Dim oObject As iPMSourceMaintenance.Interface_Renamed

        'Get an instance of the iPMSourceMaintenance interface
        Dim temp_oObject As Object
        m_lReturn = oObjectManager.GetInstance(temp_oObject, sClassName:="iPMSourceMaintenance.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
        oObject = temp_oObject
        If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
            oObject = Nothing
            oObjectManager.Dispose()
            oObjectManager = Nothing
            Environment.Exit(0)
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("Failed to get instance of iPMSourceMaintenance.Interface", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            ' Clear up
            oObjectManager.Dispose()
            oObjectManager = Nothing
            Environment.Exit(0)
        End If

        ' Initialise it


        m_lReturn = oObject.Initialise()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            oObject = Nothing
            Exit Sub
        End If

        ' Start it

        m_lReturn = oObject.Start()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            oObject = Nothing
            Exit Sub
        End If

        ' Terminate it

        oObject.Dispose()

        oObject = Nothing

        'Clear Up
        oObjectManager.Dispose()
        oObjectManager = Nothing

	End Sub
End Module