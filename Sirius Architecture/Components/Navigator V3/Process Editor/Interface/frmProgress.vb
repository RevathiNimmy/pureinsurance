Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Partial Friend Class frmProgress 
	Inherits System.Windows.Forms.Form
    Implements IDisposable


    Public Sub Complete()

        'Complete the progress
        pbProgress.Value = pbProgress.Maximum

    End Sub
	
	Public Sub Increment()
		
		
		Dim iValue As Integer = pbProgress.Value
		Dim iMax As Integer = pbProgress.Maximum
		
		iValue += 1
		iMax -= 5
		
		If iValue > iMax Then
			pbProgress.Maximum = iMax + 10
		End If
		
		pbProgress.Value = iValue
		
	End Sub
	
	Public Function Initialise(ByRef sDescription As String, ByRef iStartValue As Integer, Optional ByRef iMin As Single = 0, Optional ByRef iMax As Single = 0, Optional ByRef bHide As Object = Nothing) As gPMConstants.PMEReturnCode



		
		Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'Set the description
			lblDescription.Text = sDescription
			
			'Set the minimum value

            If Not Information.IsNothing(iMin) Then
                pbProgress.Minimum = iMin
            End If

            'Set the maximum value

            If Not Information.IsNothing(iMax) Then
                pbProgress.Maximum = iMax
            End If

            'Set the start value
            pbProgress.Value = iStartValue

            'If hide selected then hide progress

            If Not Information.IsNothing(bHide) Then
                pbProgress.Visible = False
            End If

            Me.Show()
            Application.DoEvents()

            Return result

        Catch


            Return gPMConstants.PMEReturnCode.PMFalse
        End Try

    End Function

 Private disposedValue As Boolean
	Public Sub Dispose() Implements IDisposable.Dispose
		Dispose(True)
		GC.SuppressFinalize(Me)
	End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                If Not (components Is Nothing) Then
                    components.Dispose()
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    Public Sub SetProgress(Optional ByRef vDescription As String = "", Optional ByRef vValue As String = "")

        lblDescription.Text = vDescription


        If Not Information.IsNothing(vValue) Then
            pbProgress.Value = Conversion.Val(vValue)
        End If

        Me.Refresh()

    End Sub
	

	Private Sub frmProgress_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		iPMFunc.CenterForm(Me)
		
	End Sub
End Class
