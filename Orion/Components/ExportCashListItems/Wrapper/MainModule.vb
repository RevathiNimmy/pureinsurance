Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Module MainModule
	
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	Public Const ACApp As String = "ACTExportCashListItems"
	

	Public Sub Main()
		
        Dim oInterface As iACTExportCashListItems.Interface_Renamed
		
		Try 
			
			oInterface = New iACTExportCashListItems.Interface_Renamed()
			
			If oInterface Is Nothing Then
				MessageBox.Show("Failed to create iACTExportCastListItems.Interface", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Error)
				Exit Sub
			End If
			
			' being called via the wrapper, not via W/Mgr task
			oInterface.CallingAppName = ACApp
			' for testing purposes, the following code can be used to force the interface
			' to display, as it would normally ony display whan called from W/Mgr
			'    oInterface.CallingAppName = ACApp & "not the wrapper!"
			
            'developer guide no. 9
            m_lReturn = oInterface.Initialise()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				oInterface = Nothing
                MessageBox.Show("Initialise failed in iACTExportCashListItems.Interface", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Error)
				Exit Sub
			End If
			
			m_lReturn = oInterface.Start()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Start failed in iACTExportCashListItems.Interface", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Error)
			End If
			
		oInterface.Dispose()

            oInterface = Nothing
		
		Catch 
			
			
			
			oInterface = Nothing
			
			MessageBox.Show("Failed to create iACTExportCashListItems.Interface", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try
		
		
	End Sub
End Module