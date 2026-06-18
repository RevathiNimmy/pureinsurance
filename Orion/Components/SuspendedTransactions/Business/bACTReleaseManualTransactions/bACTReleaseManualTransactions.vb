Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
'developer guide no. 129
Imports SharedFiles
Module MainModule
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bACTReleaseManualTransactions"
    Private m_oProgressBar As iPMBProgressBarWrapper.Wrapper
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	

	Public Sub Main()
		
		' Run PMLock interface
		
		Try 
            'Dim oTransDetail As Object
            Dim oTransDetail As bACTTransDetail.Form
			Dim lError As gPMConstants.PMEReturnCode
			
            ' Create the interface object
            'oTransDetail = CreateObject("bACTTransDetail.Form")
            oTransDetail = New bACTTransDetail.Form()

            Try  ' Don't error if it fails
                m_oProgressBar = New iPMBProgressBarWrapper.Wrapper()

            Catch
            End Try

            If Not (m_oProgressBar Is Nothing) Then
                m_oProgressBar.Caption = "  Processing Release Manual Transactions . . ."
                m_oProgressBar.Text = "It may take several minutes to process. Please wait..."
                m_oProgressBar.StartBar = True
            End If

			lError = oTransDetail.Initialise("sirius", "sirius", 1, 1, 1, 26, 1, ACApp)
			
			If lError <> gPMConstants.PMEReturnCode.PMTrue Then
				oTransDetail = Nothing
				Exit Sub
			End If
			
			' Start the interface process
			oTransDetail.CallingAppName = "bACTReleaseManualTransactions"
			
			lError = oTransDetail.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric)
			
			lError = oTransDetail.Start()
			
			'Run RELEASESUSPENDEDCOMMISSION method
			lError = oTransDetail.ReleaseManualTransactions()
            If lError = PMEReturnCode.PMMAlreadyInUse Then
                MsgBox("This process is already running..")
            End If
            ' Destroy the interface object
		oTransDetail.Dispose()

            

            oTransDetail = Nothing

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to execute the Main function.", vApp:=ACApp, vClass:=ACClass, vMethod:="Main", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
		
	End Sub
End Module
