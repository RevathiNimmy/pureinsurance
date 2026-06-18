Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.IO
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmLogDrop
	Inherits System.Windows.Forms.Form
	
	Private m_oForm As frmMain
	Private m_lReturn As Integer
	Private m_sFilename As String = ""
	
	Private m_lStatus As gPMConstants.PMEReturnCode
	
	Private Const ACClass As String = "frmlogDrop"
	
	Public ReadOnly Property Filename() As String
		Get
			Return m_sFilename
		End Get
	End Property
	
    Public Property Status() As Integer
        Get
            Return m_lStatus
        End Get
        Set(ByVal Value As Integer)
            m_lStatus = Value
        End Set
    End Property
	

    Private Sub Form_OLEDragDrop(ByRef Data As DataObject, ByRef Effect As Integer, ByRef Button As Integer, ByRef Shift As Integer, ByRef x As Single, ByRef y As Single)


        'TODO check at runtime
        ' m_sFilename = Data.Files.Item(1)
        ' m_sFilename = Data

        Status = gPMConstants.PMEReturnCode.PMOK

        Me.Hide()

    End Sub
	
	' ***************************************************************** '
	'
	' Name: SelectFile
	'
	' Description:
	'
	' History: 23/07/2001 CTAF - Created.
	'
	' ***************************************************************** '
	Public Function SelectFile() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			

            'Todo check at run time
            'With dlgOpen

            '.CancelError = True

            ' .Action = 1
            ' Get the filename
            ' m_sFilename = .FileName
            'End With

            Status = gPMConstants.PMEReturnCode.PMOK

            ' Hide the form
            Me.Hide()

            Return result

        Catch excep As System.Exception



            If Information.Err().Number = 32755 Then
                Return result
            End If

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectFile", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result




            Return result
        End Try
	End Function
	
	Private Sub frmLogDrop_Paint(ByVal eventSender As Object, ByVal eventArgs As PaintEventArgs) Handles MyBase.Paint
		
		' Make the form always on top
        'TODO check at run time
        'TopMost.SetTopmost(frmForm:=Me)
		
	End Sub
	
    Private Const vbFormCode As Integer = 0
	Private Sub frmLogDrop_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
		Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
		Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
		


        If UnloadMode = vbFormCode Then
            Status = gPMConstants.PMEReturnCode.PMOK
        Else
            Status = gPMConstants.PMEReturnCode.PMCancel
        End If

        eventArgs.Cancel = Cancel <> 0
	End Sub
	

    'TODO check at run time
    'Private Sub imgLogo_OLEDragDrop(ByRef Data As DataObject, ByRef Effect As Integer, ByRef Button As Integer, ByRef Shift As Integer, ByRef x As Single, ByRef y As Single)


    '	m_sFilename = Data.Files.Item(1)

    '	Status = gPMConstants.PMEReturnCode.PMOK

    '	Me.Hide()

    'End Sub
	
	Public Sub mnuFileExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFileExit.Click
		
		Status = gPMConstants.PMEReturnCode.PMCancel
		
		Me.Hide()
		
	End Sub
	
	Public Sub mnuFileOpen_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFileOpen.Click
		
		' Chose the file
		m_lReturn = SelectFile()
		
	End Sub
End Class
