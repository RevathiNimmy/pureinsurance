Option Strict Off
Option Explicit On
Imports System
'Developer Guide No. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed 

	  Implements IDisposable
Private m_lReturn As Integer

    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

 Private disposedValue As Boolean
	Public Sub Dispose() Implements IDisposable.Dispose
		Dispose(True)
		GC.SuppressFinalize(Me)
	End Sub


	Protected Sub Dispose(disposing As Boolean)
		If Not Me.disposedValue Then
			 If disposing Then
			End If
		End If
		Me.disposedValue = True
	End Sub


    Public Function Start() As Integer

        Dim result As Integer = 0
        Dim oForm As frmInterface

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            oForm = New frmInterface()


            'Developer Guide No. 68

            m_lReturn = oForm.Start()

            oForm.Close()

            oForm = Nothing


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function
End Class
