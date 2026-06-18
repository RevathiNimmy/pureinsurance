Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports System
Imports System.Windows.Forms

<System.Runtime.InteropServices.ProgId("clsHTMLField_NET.clsHTMLField")> _
Public NotInheritable Class clsHTMLField 
	
	'Constants to determine the DHTML Element
	Private Const ACDHTMLNone As Integer = -1
	Private Const ACInputText As Integer = 0
	Private Const ACInputButton As Integer = 1
	Private Const ACOptionButton As Integer = 2
	Private Const ACSelect As Integer = 3
	Private Const ACInputImage As Integer = 4
	Private Const ACButton As Integer = 5
	Private Const ACTextArea As Integer = 6
	Private Const ACComboBox As Integer = 7
	
	'Element Type
	Private m_lHTMLElementType As Integer
	
	'Parent control
	Private m_oParent As uctPMDhtmlScreen
	
	'************************************************************'
	'Standard Properties
	Private m_sCtlType As String = ""
	Private m_sCtlName As String = ""
	Private m_sID As String = ""
	Private m_vValue As Object
	Private m_sLabelStyle As String = ""
	Private m_Disabled As Boolean
	
	'Supported Types of Standard Controls for onblur and onfocus
	Private WithEvents ctlHTMLInputText As mshtml.HTMLInputTextElement
	Private WithEvents ctlHTMLInputButton As mshtml.HTMLInputButtonElement
	Private WithEvents ctlHTMLOptionButton As mshtml.HTMLOptionButtonElement
	Private WithEvents ctlHTMLSelect As mshtml.HTMLSelectElement
	Private WithEvents ctlHTMLInputImage As mshtml.htmlInputImage
	Private WithEvents ctlHTMLButton As mshtml.HTMLButtonElement
	Private WithEvents ctlHTMLTextArea As mshtml.HTMLTextAreaElement
	Private WithEvents ctlComboBox As ComboBox
	
	
	
	Public Property Disabled() As Boolean
		Get
			
			Return m_Disabled
			
		End Get
		Set(ByVal Value As Boolean)
			
			m_Disabled = Value
			
		End Set
	End Property
	
	
	
	Public Property CtlName() As String
		Get
			
			Return m_sCtlName
			
		End Get
		Set(ByVal Value As String)
			
			m_sCtlName = Value
			
		End Set
	End Property
	
	
	
	
	Public Property CtlType() As String
		Get
			
			Return m_sCtlType
			
		End Get
		Set(ByVal Value As String)
			
			m_sCtlType = Value
			
		End Set
	End Property
	
	Public WriteOnly Property DHTMLElement() As Object
		Set(ByVal Value As Object)
			

			Try 
				
				'Clear Element
				m_lHTMLElementType = ACDHTMLNone
				
				'Input Button Element
				If TypeOf Value Is mshtml.IHTMLInputButtonElement Then
					ctlHTMLInputButton = Value
					m_lHTMLElementType = ACInputButton
					
					'Option Button Element
				ElseIf TypeOf Value Is mshtml.IHTMLOptionButtonElement Then 
					ctlHTMLOptionButton = Value
					m_lHTMLElementType = ACOptionButton
					
					'Select Element
				ElseIf TypeOf Value Is mshtml.IHTMLSelectElement Then 
					ctlHTMLSelect = Value
					m_lHTMLElementType = ACSelect
					
					'Input Image Element
				ElseIf TypeOf Value Is mshtml.IHTMLInputImage Then 
					ctlHTMLInputImage = Value
					m_lHTMLElementType = ACInputImage
					
					'button Element
				ElseIf TypeOf Value Is mshtml.IHTMLButtonElement Then 
					ctlHTMLButton = Value
					m_lHTMLElementType = ACButton
					
					'Text Input Element
				ElseIf TypeOf Value Is mshtml.IHTMLInputTextElement Then 
					ctlHTMLInputText = Value
					m_lHTMLElementType = ACInputText
					
				ElseIf TypeOf Value Is mshtml.IHTMLTextAreaElement Then 
					ctlHTMLTextArea = Value
					m_lHTMLElementType = ACTextArea
					
					'ComboBox
				ElseIf TypeOf Value Is ComboBox Then 
					ctlComboBox = Value
					m_lHTMLElementType = ACComboBox
					
				End If
			
			Catch exc As System.Exception
				NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
			End Try
			
		End Set
	End Property
	
	
	Public Property ID() As String
		Get
			
			Return m_sID
			
		End Get
		Set(ByVal Value As String)
			
			m_sID = Value
			
		End Set
	End Property
	
	
	
	Public Property LabelStyle() As String
		Get
			
			Return m_sLabelStyle
			
		End Get
		Set(ByVal Value As String)
			
			m_sLabelStyle = Value
			
		End Set
	End Property
	
	
	
	Public WriteOnly Property Parent() As uctPMDhtmlScreen
		Set(ByVal Value As uctPMDhtmlScreen)
			
			m_oParent = Value
			
		End Set
	End Property
	
	
	Public Property Value() As Object
		Get
			
			Return m_vValue
			
		End Get
		Set(ByVal Value As Object)
			


			m_vValue = Value
			
		End Set
	End Property
	
	Protected Overrides Sub Finalize()
		

		Try 
			ctlHTMLInputText = Nothing
			ctlHTMLInputButton = Nothing
			ctlHTMLOptionButton = Nothing
			ctlHTMLSelect = Nothing
			ctlHTMLInputImage = Nothing
			ctlHTMLButton = Nothing
		
		Catch exc As System.Exception
			NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
		End Try
		
	End Sub
	
	Private Sub ctlComboBox_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ctlComboBox.Leave
		
		m_oParent.ElementLostFocus(m_sID)
		
	End Sub
	
	Private Sub ctlComboBox_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ctlComboBox.Enter
		
		m_oParent.ElementGotFocus(m_sID)
		
	End Sub
	
	Private Sub ctlHTMLButton_onblur() Handles ctlHTMLButton.onblur
		
		m_oParent.ElementLostFocus(m_sID)
		
	End Sub
	
	Private Sub ctlHTMLButton_onfocus() Handles ctlHTMLButton.onfocus
		
		m_oParent.ElementGotFocus(m_sID)
		
	End Sub
	
	Private Sub ctlHTMLInputButton_onblur() Handles ctlHTMLInputButton.onblur
		
		m_oParent.ElementLostFocus(m_sID)
		
	End Sub
	
	Private Sub ctlHTMLInputButton_onfocus() Handles ctlHTMLInputButton.onfocus
		
		m_oParent.ElementGotFocus(m_sID)
		
	End Sub
	
	
	Private Sub ctlHTMLInputImage_onblur() Handles ctlHTMLInputImage.onblur
		
		m_oParent.ElementLostFocus(m_sID)
		
	End Sub
	
	Private Sub ctlHTMLInputImage_onfocus() Handles ctlHTMLInputImage.onfocus
		
		m_oParent.ElementGotFocus(m_sID)
		
	End Sub
	
	
	Private Sub ctlHTMLInputText_onblur() Handles ctlHTMLInputText.onblur
		
		m_oParent.ElementLostFocus(m_sID)
		
	End Sub
	
	Private Sub ctlHTMLInputText_onfocus() Handles ctlHTMLInputText.onfocus
		
		m_oParent.ElementGotFocus(m_sID)
		
	End Sub
	
	Private Sub ctlHTMLOptionButton_onblur() Handles ctlHTMLOptionButton.onblur
		
		m_oParent.ElementLostFocus(m_sID)
		
	End Sub
	
	Private Sub ctlHTMLOptionButton_onfocus() Handles ctlHTMLOptionButton.onfocus
		
		m_oParent.ElementGotFocus(m_sID)
		
	End Sub
	
	
	Private Sub ctlHTMLSelect_onblur() Handles ctlHTMLSelect.onblur
		
		m_oParent.ElementLostFocus(m_sID)
		
	End Sub
	
	Private Sub ctlHTMLSelect_onfocus() Handles ctlHTMLSelect.onfocus
		
		m_oParent.ElementGotFocus(m_sID)
		
	End Sub
	
	
	'UPGRADE_NOTE: (7001) The following declaration (ctlHTMLTable_onhelp) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function ctlHTMLTable_onhelp() As Boolean
		'
	'End Function
	
	
	'UPGRADE_NOTE: (7001) The following declaration (ctlHTMLTable_onkeydown) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Sub ctlHTMLTable_onkeydown()
		'
	'End Sub
	
	
	'UPGRADE_NOTE: (7001) The following declaration (m_ctlHTMLButton_onhelp) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function m_ctlHTMLButton_onhelp() As Boolean
		'
	'End Function
	
	
	'UPGRADE_NOTE: (7001) The following declaration (m_ctlHTMLButton_onkeydown) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Sub m_ctlHTMLButton_onkeydown()
		'
	'End Sub
	
	
	Private Sub ctlHTMLTextArea_onblur() Handles ctlHTMLTextArea.onblur
		
		m_oParent.ElementLostFocus(m_sID)
		
	End Sub
	
	Private Sub ctlHTMLTextArea_onfocus() Handles ctlHTMLTextArea.onfocus
		
		m_oParent.ElementGotFocus(m_sID)
		
	End Sub
End Class
