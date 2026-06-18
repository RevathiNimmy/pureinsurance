Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("PBFindDesign_NET.PBFindDesign")> _
Public Partial Class PBFindDesign
	Inherits System.Windows.Forms.UserControl
	'events exposed
	Public Shadows Event MouseDown(ByVal Sender As Object, ByVal e As MouseDownEventArgs)
	Public Shadows Event MouseUp(ByVal Sender As Object, ByVal e As MouseUpEventArgs)
	Public Shadows Event MouseMove(ByVal Sender As Object, ByVal e As MouseMoveEventArgs)
	Public Event btnFind_renamed(ByVal Sender As Object, ByVal e As EventArgs)
	
	Private m_lFindControlID As Integer
	Private m_vScreenControlArray( ,  ) As Object
    'Akash: New reference is created
    Private frmMapping As frmMappings

    'properties
    'find control id
    <Browsable(True)> _
    Public Property FindControlID() As Integer
        Get
            Return m_lFindControlID
        End Get
        Set(ByVal Value As Integer)
            m_lFindControlID = Value
        End Set
    End Property

    'search data array ( controltag, searchvalue, foundvalue ) passed bewteen control and risk screen
    <Browsable(True)> _
    Public Property DataArray() As Object
        Get
            Return m_vDataArray
        End Get
        Set(ByVal Value As Object)

            m_vDataArray = Value
        End Set
    End Property

    'screen control array
    <Browsable(True)> _
    Public Property ScreenControlArray() As Object
        Get
            Return m_vScreenControlArray
        End Get
        Set(ByVal Value As Object)
            ' developer guide no. 17
            m_vScreenControlArray = Value
        End Set
    End Property

    Private Sub btnClear_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles btnClear.Click
        Try
            frmMapping = New frmMappings
            'MsgBox "During run time, this button will clear the search criteria", vbInformation
            RaiseEvent btnFind_renamed(Me, Nothing)
            frmMapping.ScreenControlArray = m_vScreenControlArray
            frmMapping.FindControlID = m_lFindControlID
            frmMapping.ShowDialog()

        Catch excep As System.Exception


            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Clear Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub
    Public Sub Delete()

        Dim m_oBusiness As bPBFindControl.Business

        Dim g_oObjectManager As New bObjectManager.ObjectManager

        ' Call the initialise method.
        m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACAPP)

        'get business object
        Dim temp_m_oBusiness As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bPBFindControl.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        m_oBusiness = temp_m_oBusiness


        m_lReturn = m_oBusiness.Start()

        m_lReturn = m_oBusiness.DeleteMappings(m_lFindControlID)

		m_oBusiness.Dispose()
        g_oObjectManager.Dispose()

    End Sub


    'Private Sub btnConfigure_Click()

    'Try 
    '
    'Catch excep As System.Exception
    '
    '
    'iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACAPP, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'End Try
    '
    'End Sub





    Private Sub btnFind_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles btnFind.Click
        Try
            frmMapping = New frmMappings
            'MsgBox "During run time, this button will search and display a choice list", vbInformation
            RaiseEvent btnFind_renamed(Me, Nothing)
            frmMapping.ScreenControlArray = m_vScreenControlArray
            frmMapping.FindControlID = m_lFindControlID
            frmMapping.ShowDialog()
            m_lFindControlID = frmMapping.FindControlID


        Catch excep As System.Exception


            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACAPP, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub
	
	Private Sub PBFindDesign_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles MyBase.MouseDown
		Dim Button As Integer = CInt(eventArgs.Button)
		Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Dim X As Single = VB6.PixelsToTwipsX(eventArgs.X)
        'Dim Y As Single = VB6.PixelsToTwipsY(eventArgs.Y)

        Dim X As Single = (eventArgs.X)
        Dim Y As Single = (eventArgs.Y)
		Try 
			RaiseEvent MouseDown(Me, New MouseDownEventArgs(Button, Shift, X, Y))
		
		Catch excep As System.Exception
			
			
			iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACAPP, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
		End Try
	End Sub
	
	Private Sub PBFindDesign_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles MyBase.MouseMove
		Dim Button As Integer = CInt(eventArgs.Button)
		Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Dim X As Single = VB6.PixelsToTwipsX(eventArgs.X)
        'Dim Y As Single = VB6.PixelsToTwipsY(eventArgs.Y)

        Dim X As Single = (eventArgs.X)
        Dim Y As Single = (eventArgs.Y)
		Try 
			RaiseEvent MouseMove(Me, New MouseMoveEventArgs(Button, Shift, X, Y))
		
		Catch excep As System.Exception
			
			
			iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACAPP, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
		End Try
	End Sub
	
	Private Sub PBFindDesign_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles MyBase.MouseUp
		Dim Button As Integer = CInt(eventArgs.Button)
		Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Dim X As Single = VB6.PixelsToTwipsX(eventArgs.X)
        'Dim Y As Single = VB6.PixelsToTwipsY(eventArgs.Y)

        Dim X As Single = (eventArgs.X)
        Dim Y As Single = (eventArgs.Y)
		Try 
			RaiseEvent MouseUp(Me, New MouseUpEventArgs(Button, Shift, X, Y))
		
		Catch excep As System.Exception
			
			
			iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACAPP, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
		End Try
	End Sub
	
	Private Sub PBFindDesign_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
		
		MyBase.Height = VB6.TwipsToPixelsY(555)
		MyBase.Width = VB6.TwipsToPixelsX(2895)
		
	End Sub
End Class
