Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.IO
Imports System.Windows.Forms
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("uctPMDhtmlScreen_NET.uctPMDhtmlScreen")> _
Public Partial Class uctPMDhtmlScreen
	Inherits System.Windows.Forms.UserControl
	' ***************************************************************** '
	' Control Name: uctPMDhtmlScreen
	'
	' Date: 02/03/1999
	'
	' Description: DHTML Screen
	'
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "uctPMDhtmlScreen"
	
	'Title of application
	Private Const ACTitle As String = "DHTML Screen"
	
	'Return variable
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	'Business object if it exists
	Private m_oBusiness As Object
	
	' WB Customise Object
	Private m_oWBCutom As Object
	
	'This object is used to create an instance of the current
	'document displayed in the web browser control
	Private WithEvents m_oDocMain As mshtml.HTMLDocument
	
	'Similarly for the submit button on the form
	Private WithEvents btnSubmit As mshtml.HTMLButtonElement
	
	'Similarly for the submit button on the form
	Private WithEvents btnReset As mshtml.HTMLButtonElement
	
	'Window object
	Private WithEvents m_oWinMain As mshtml.HTMLWindow2
	
	'Collection of HTML Fields
	Private m_cHTMLFields As Collection
	
	'Path of the Current Document
	Private m_sPath As String = ""
	
	'Flag to trap when the document has been downloaded
	Private m_bDownloadComplete As Boolean
	
	' Is a counter to trap if the control has timedout
	Private m_lTimerValue As Integer
	
	' ************************************************************************* '
	'Properties
	
	'The start URL
	Private m_sStartURL As String = ""
	
	'The Flag to state whether or not to use start URL
	Private m_bNavigateOnShow As Boolean
	
	'The Flag to state whether or not to Run in silent Mode
	Private m_blSilent As Boolean
	
	'The Flag to state whether or not to show the Context Menus
	Private m_blContextMenu As Boolean
	
	'The Value, in seconds, at which the WBMain control will time out
	Private m_lTimeOut As Integer
	
	
	'The location URL
	Private m_sLocationURL As String = ""
	
	' ************************************************************************* '
	' Events Declared for this Control
	Public Event onafterupdate(ByVal Sender As Object, ByVal e As onafterupdateEventArgs)
	Public Event onbeforeupdate(ByVal Sender As Object, ByVal e As onbeforeupdateEventArgs)
	Public Event onclick_Renamed(ByVal Sender As Object, ByVal e As onclickEventArgs)
	Public Event ondblclick(ByVal Sender As Object, ByVal e As ondblclickEventArgs)
	Public Event ondragstart(ByVal Sender As Object, ByVal e As ondragstartEventArgs)
	Public Event onerrorupdate(ByVal Sender As Object, ByVal e As onerrorupdateEventArgs)
	Public Event onhelp(ByVal Sender As Object, ByVal e As onhelpEventArgs)
	Public Event onkeypress_Renamed(ByVal Sender As Object, ByVal e As onkeypressEventArgs)
	Public Event onmousedown_Renamed(ByVal Sender As Object, ByVal e As onmousedownEventArgs)
	Public Event onmousemove_Renamed(ByVal Sender As Object, ByVal e As onmousemoveEventArgs)
	Public Event onmouseout(ByVal Sender As Object, ByVal e As onmouseoutEventArgs)
	Public Event onmouseover(ByVal Sender As Object, ByVal e As onmouseoverEventArgs)
	Public Event onmouseup_Renamed(ByVal Sender As Object, ByVal e As onmouseupEventArgs)
	Public Event ongotfocus_Renamed(ByVal Sender As Object, ByVal e As ongotfocusEventArgs)
	Public Event onlostfocus_Renamed(ByVal Sender As Object, ByVal e As onlostfocusEventArgs)
	Public Event onreset(ByVal Sender As Object, ByVal e As EventArgs)
	Public Event onsubmit(ByVal Sender As Object, ByVal e As EventArgs)
	
	Public Sub AddItem(ByVal sID As String, ByVal sItem As String, Optional ByRef vIndex As String = "")
		
		Dim selElement As mshtml.HTMLOptionElement
		
		Try 
			
			'Create a new option
			selElement = m_oDocMain.createElement("OPTION")
			
			'Get the Item string
			selElement.text = sItem
			selElement.value = sItem
			

			If Not Information.IsNothing(vIndex) Then
				selElement.index = CInt(Conversion.Val(vIndex))
			End If
			
			'Add Item

			m_oDocMain.all.item(sID).Options.Add(selElement)
		
		Catch 
		End Try
		
		
		
		
	End Sub
	
	' *************************************************************************** '
	'   Name : IGetTitle (Public)
	'
	'   Description : Gets the details of document title
	'
	' *************************************************************************** '
	Public Function IGetTitle(ByRef sTitle As String) As gPMConstants.PMEReturnCode
		
		Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'Get the title
			sTitle = m_oDocMain.title
			
			Return result
		
		Catch 
			
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
	
	
	Public Sub ElementGotFocus(ByRef sID As String)
		
		'Raise the Gotfocus Event
		RaiseEvent ongotfocus_Renamed(Me, New ongotfocusEventArgs(sID))
		
	End Sub
	
	
	Public Sub ElementLostFocus(ByRef sID As String)
		
		'Raise the lost focus event
		RaiseEvent onlostfocus_Renamed(Me, New onlostfocusEventArgs(sID))
		
	End Sub
	
	
	' *************************************************************************** '
	'
	' Name        : GetControlObject (Public)
	'
	' Description : Returns the DHTML Control object for use by calling app.
	'             : This is especially necessary for ActiveX objects
	'
	' *************************************************************************** '
	Public Function GetControlObject(ByRef sID As String) As Object
		
		Try 
			
			'Get the object reference
			
			Return m_oDocMain.all.item(sID)
		
		Catch 
		End Try
		
		
		
	End Function
	
	<Browsable(False)> _
	Public ReadOnly Property HTMLFields() As Collection
		Get
			
			Return m_cHTMLFields
			
		End Get
	End Property
	
	
	<Browsable(True)> _
	Public Property StartURL() As String
		Get
			
			Return m_sStartURL
			
		End Get
		Set(ByVal Value As String)
			
			m_sStartURL = Value
			
		End Set
	End Property
	
	
	
	<Browsable(True)> _
	Public Property Silent() As Boolean
		Get
			
			Return m_blSilent
			
		End Get
		Set(ByVal Value As Boolean)
			
			m_blSilent = Value
			
		End Set
	End Property
	
	
	<Browsable(True)> _
	Public Shadows Property ContextMenu() As Boolean
		Get
			
			Return m_blContextMenu
			
		End Get
		Set(ByVal Value As Boolean)
			
			m_blContextMenu = Value
			
		End Set
	End Property
	
	
	
	
	<Browsable(True)> _
	Public Property TimeOut() As Integer
		Get
			
			Return m_lTimeOut
			
		End Get
		Set(ByVal Value As Integer)
			
			m_lTimeOut = Value
			
		End Set
	End Property
	
	
	<Browsable(True)> _
	Public Property NavigateOnShow() As Boolean
		Get
			
			Return m_bNavigateOnShow
			
		End Get
		Set(ByVal Value As Boolean)
			
			m_bNavigateOnShow = Value
			
		End Set
	End Property
	
	
	' *************************************************************************** '
	'
	'   Name : IClear (Public)
	'
	'   Description : Clear all the controls on the selected document
	'
	' *************************************************************************** '
	Public Function IClear() As Integer
		
		Dim result As Integer = 0
		


		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		'Ignore errors for now
		Try 
			
			'Clear all of the controls on the Document
			
			For	Each HTMLControl As Object In m_oDocMain.all
				

				If HTMLControl.ID <> "" Then
					

					
					Select Case HTMLControl.Type
						Case ACTextControl, ACSelectOne, ACSelectMultiple, ACPasswordControl, ACTextAreaControl

							HTMLControl.Value = ""
							
						Case ACCheckboxControl, ACRadioControl

							HTMLControl.Checked = False
							
						Case ACButtonControl, ACImageControl, ACSubmitControl, ACResetControl, ACTableControl, ACActiveXControl
							'HTMLControl.Value = ""
							
						Case Else

							HTMLControl.Value = ""
							
					End Select
					
				End If
				
			Next HTMLControl
		
		Catch 
		End Try
		
		Return result
		
Err_IClear: 
		Return gPMConstants.PMEReturnCode.PMError
		
	End Function
	
	
	' *************************************************************************** '
	'   Name : IRetrieveDetails (Public)
	'
	'   Description : Populate the Selected Dhtml Document
	'
	' *************************************************************************** '
	Public Function IRetrieveDetails(ByRef cDetails As Collection) As Integer
		
		Dim result As Integer = 0
        Dim oField As clsHTMLField
		Dim sStyle As String = ""
		


		
		result = gPMConstants.PMEReturnCode.PMTrue
		

		Try 
			
			cDetails = New Collection()
			
			For	Each HTMLControl As Object In m_oDocMain.all
				


				If HTMLControl.ID <> "" And HTMLControl.tagName <> "SPAN" Then
					
					oField = New clsHTMLField()
					
					'Get the Type, Name and ID

					oField.CtlType = HTMLControl.Type

					oField.CtlName = HTMLControl.Name

					oField.ID = HTMLControl.ID
					
					'Get the Value
					If oField.CtlType = ACCheckboxControl Or oField.CtlType = ACRadioControl Then
						


                        'oField.set_Value(HTMLControl.Checked)
						
						'            ElseIf HTMLControl.Type = ACSelectMultiple Or _
						''                HTMLControl.Type = ACSelectOne Then
						'
						'                oField.Value = HTMLControl.Item(HTMLControl.selectedIndex).Text
						'
					Else
						


                        'oField.set_Value(HTMLControl.Value)
					End If
					
					'Get the Label Style

					m_lReturn = CType(IGetLabelStyle(sID:=HTMLControl.ID, sStyle:=sStyle), gPMConstants.PMEReturnCode)
					
					If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						oField.LabelStyle = ""
					Else
						oField.LabelStyle = sStyle
					End If
					

					oField.Disabled = HTMLControl.Disabled
					
					'Set the Element object in the field object


                    'oField.set_DHTMLElement(HTMLControl)
					
					'Set the Parent property

                    'oField.set_Parent(Me)
					
					'Add the field

					cDetails.Add(oField, HTMLControl.ID)
					
				End If
				
			Next HTMLControl
			
			Return result
			
Err_IRetrieveDetails: 
			Return gPMConstants.PMEReturnCode.PMError
		
		Catch exc As System.Exception
			NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
		End Try
		
	End Function
	
	
	' *************************************************************************** '
	'
	'   Name        : IPopulateForm (Public)
	'
	'   Description : Populate the Selected Dhtml Document
	'
	' *************************************************************************** '
	Public Function IPopulateForm(ByRef cDetails As Collection) As Integer
		
		Dim result As Integer = 0
		Dim HTMLControl As Object

		result = gPMConstants.PMEReturnCode.PMTrue
		
		'Because we are using a general method to change the value of
		'a potentially diverse set of controls, we will ignore controls
		'that do not have a value property

		Try 
			
			'Check all the details
			For lPtr As Integer = 1 To cDetails.Count
				
				'Get the HTML control

				HTMLControl = m_oDocMain.all.item(cDetails.Item(lPtr).ID)
				
				'Set Control Type, Name and ID


				HTMLControl.Type = cDetails.Item(lPtr).CtlType


				HTMLControl.Name = cDetails.Item(lPtr).CtlName


				HTMLControl.ID = cDetails.Item(lPtr).ID
				
				'Checkboxes and Radio Buttons

				If cDetails.Item(lPtr).CtlType = ACCheckboxControl Or cDetails.Item(lPtr).CtlType = ACRadioControl Then
					
					'Set the checkbox value


					HTMLControl.Checked = Conversion.Val(cDetails.Item(lPtr).Value)
					
					'        ElseIf HTMLControl.Type = ACSelectMultiple Or _
					''            HTMLControl.Type = ACSelectOne Then
					'
					'            For lCnt& = 1 To HTMLControl.length
					'
					'                If oField.Value = HTMLControl.Item(lCnt&).Text Then
					'                    HTMLControl.selectedIndex = lCnt&
					'                End If
					'
					'            Next lCnt&
					'
				Else
					
					'Set the value


					HTMLControl.Value = cDetails.Item(lPtr).Value
					
				End If
				
				'Set the label style


				m_lReturn = CType(ISetLabelStyle(sID:=cDetails.Item(lPtr).ID, sStyle:=cDetails.Item(lPtr).LabelStyle), gPMConstants.PMEReturnCode)
				
				'Set the label style


				m_lReturn = CType(lDisableControl(sID:=cDetails.Item(lPtr).ID, Status:=cDetails.Item(lPtr).Disabled), gPMConstants.PMEReturnCode)
				
				
			Next lPtr
			
			Return result
			
Err_IPopulateForm: 
			Return gPMConstants.PMEReturnCode.PMError
		
		Catch exc As System.Exception
			NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
		End Try
		
	End Function
	
	
	' *************************************************************************** '
	'   Name : ISetElement (Public)
	'
	'   Description : Change the values of an element
	'
	' *************************************************************************** '
	Public Function ISetElement(ByRef sID As String, Optional ByRef vCtlName As Object = Nothing, Optional ByRef vValue As Object = Nothing, Optional ByRef vLabelStyle As Object = Nothing) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'Alter the control name

			If Not Information.IsNothing(vCtlName) Then


				m_oDocMain.all.item(sID).Name = vCtlName
			End If
			
			'Alter the value

			If Not Information.IsNothing(vValue) Then


				m_oDocMain.all.item(sID).Value = vValue
			End If
			
			'Alter the label style

			If Not Information.IsNothing(vLabelStyle) Then


				m_oDocMain.all.item("lbl" & sID).className = vLabelStyle
			End If
			
			Return result
		
		Catch 
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
	' *************************************************************************** '
	'   Name : IGetElement (Public)
	'
	'   Description : Gets the detials of an element
	'
	' *************************************************************************** '
	Public Function IGetElement(ByRef sID As String, Optional ByRef vCtlName As String = "", Optional ByRef vValue As String = "", Optional ByRef vLabelStyle As String = "") As Integer
		', _
		''    Optional vCtlType As Variant
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'Get the control name

			If Not Information.IsNothing(vCtlName) Then
				

				vCtlName = m_oDocMain.all.item(sID).Name
				
			End If
			
			'Get the value

			If Not Information.IsNothing(vValue) Then
				

				vValue = m_oDocMain.all.item(sID).Value
				
			End If
			
			'Get the label style

			If Not Information.IsNothing(vLabelStyle) Then
				

				vLabelStyle = m_oDocMain.all.item("lbl" & sID).className
				
			End If
			
			'Get the CtlType
			'    If (IsMissing(vCtlType) = False) Then
			'
			'         vCtlType = m_oDocMain.All(sID).Type
			'
			'    End If
			
			Return result
		
		Catch 
			
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
	
	' *************************************************************************** '
	'   Name : lGetListCount (Public)
	'
	'   Description : Gets the detials of an element
	'
	' *************************************************************************** '
	Public Function lGetListCount(ByVal sID As String, ByRef lListCount As Integer) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			

			If m_oDocMain.all.item(sID).Type = ACSelectOne Or m_oDocMain.all.item(sID).Type = ACSelectMultiple Then
				

				lListCount = m_oDocMain.all.item(sID).length
				
			End If
			
			'    m_oDocMain.All(sID).length = 0
			
			Return result
		
		Catch 
			
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
	
	' *************************************************************************** '
	'   Name : lClearList (Public)
	'
	'   Description : Gets the detials of an element
	'
	' *************************************************************************** '
	Public Function lClearList(ByVal sID As String) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			

			If m_oDocMain.all.item(sID).Type = ACSelectOne Or m_oDocMain.all.item(sID).Type = ACSelectMultiple Then
				

				m_oDocMain.all.item(sID).length = 0
				
			End If
			
			Return result
		
		Catch 
			
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
	
	' *************************************************************************** '
	'   Name : lGetListText (Public)
	'
	'   Description : Gets the detials of an element
	'
	' *************************************************************************** '
	Public Function lGetListText(ByVal sID As String, ByRef sText As String) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			sText = ""
			

			If m_oDocMain.all.item(sID).Type = ACSelectOne Or m_oDocMain.all.item(sID).Type = ACSelectMultiple Then
				


				sText = m_oDocMain.all.item(sID).Item(m_oDocMain.all.item(sID).SelectItem).Text
				
			End If
			
			Return result
		
		Catch 
			
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
	
	' *************************************************************************** '
	'   Name : ISetDocument (Public)
	'
	'   Description : Set the Dhtml Document including path
	'
	' *************************************************************************** '
	Public Function ISetDocument(ByRef sPath As String) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			m_bDownloadComplete = False
			m_lTimerValue = 0
			
			'Store the current path
			m_sPath = sPath
			
			'Navigate to selected path
			wbMain.Navigate(New URI(sPath))
			
			Timer1.Enabled = True
			
			Do 
				Application.DoEvents()
			Loop Until (Not wbMain.IsBusy) Or (m_lTimerValue = m_lTimeOut)
			
			Timer1.Enabled = False
			
			If m_lTimerValue = m_lTimeOut Then
				result = gPMConstants.PMEReturnCode.PMError
			Else
				m_lReturn = CType(RefreshDocument(), gPMConstants.PMEReturnCode)
			End If
			
			Return result
		
		Catch 
			
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
	Public Function ISetFocus(ByRef sID As String) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'Set the focus of the control

			m_oDocMain.all.item(sID).focus()
			
			Return result
		
		Catch 
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
	
	' *************************************************************************** '
	'   Name : lDisableControl (Public)
	'
	'   Description : Set the label style of a control
	'
	' *************************************************************************** '
	Public Function lDisableControl(ByRef sID As String, ByRef Status As Boolean) As Integer
		
		Dim result As Integer = 0

		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			

			m_oDocMain.all.item(sID).Disabled = Status
			
			If Information.Err().Number <> 0 Then
				


				

				m_oDocMain.all.item(sID).Object.Enabled = Not (Status)
				
				'MsgBox "Control """ & sID & """ Enabled = " & m_oDocMain.All(sID).object.Enabled
				
			End If
			
			Return result
			
Err_lDisableControl: 
			result = gPMConstants.PMEReturnCode.PMError

			MessageBox.Show("Control """ & sID & """ Failed and is set to " & m_oDocMain.all.item(sID).Object.Enabled, Application.ProductName)
			Return result
		
		Catch exc As System.Exception
			NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
		End Try
	End Function
	
	
	' *************************************************************************** '
	'   Name : ISetLabelStyle (Public)
	'
	'   Description : Set the label style of a control
	'
	' *************************************************************************** '
	Public Function ISetLabelStyle(ByRef sID As String, ByRef sStyle As String) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'Set te label style
			
			Return ISetElement(sID:=sID, vLabelStyle:=sStyle)
		
		Catch 
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
	
	' *************************************************************************** '
	'   Name : IGetLabelStyle (Public)
	'
	'   Description : Set the label style of a control
	'
	' *************************************************************************** '
	Public Function IGetLabelStyle(ByVal sID As String, ByRef sStyle As String) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'Get the label style
			
			Return IGetElement(sID:=sID, vLabelStyle:=sStyle)
		
		Catch 
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
	Public Sub RemoveItem(ByRef sID As String, ByRef lIndex As Integer)
		
		Try 
			
			'Remove the index
			If lIndex <> -1 Then

				m_oDocMain.all.item(sID).Options.Remove(lIndex)
			End If
		
		Catch 
		End Try
		
		
		
		
	End Sub
	
	Public Function RefreshDocument() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'This method will be called more than once in some cases
			If wbMain.Document.DomDocument Is Nothing Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			
			'Set up document instance

			m_oDocMain = wbMain.Document.DomDocument.frames(ACMainFrame).Document
			
			'Set the window instance
			m_oWinMain = m_oDocMain.parentWindow
			
			'Get the submit button object
			btnSubmit = m_oDocMain.all.item(ACSubmitButton)
			
			'Get the reset button object
			btnReset = m_oDocMain.all.item(ACResetButton)
			
			
			'Initialise Document
			InitialiseDocument()
			
			Return result
		
		Catch 
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
	
	
	
	
	'UPGRADE_NOTE: (7001) The following declaration (btnReset_onclick) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function btnReset_onclick() As Boolean
		'
		'RaiseEvent onreset(Me, Nothing)
		'
	'End Function
	
	'UPGRADE_NOTE: (7001) The following declaration (btnSubmit_onclick) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function btnSubmit_onclick() As Boolean
		'
		'RaiseEvent onsubmit(Me, Nothing)
		'
	'End Function
	
	Private Sub m_oDocMain_onafterupdate() Handles m_oDocMain.onafterupdate
		
		
		'Get the ID of the control that generated the event
		Dim sID As String = m_oWinMain.event.srcElement.id
		
		'Raise the onclick event
		RaiseEvent onafterupdate(Me, New onafterupdateEventArgs(sID))
		
	End Sub
	
	'UPGRADE_NOTE: (7001) The following declaration (m_oDocMain_onbeforeupdate) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function m_oDocMain_onbeforeupdate() As Boolean
		'
		'
		'Do not cancel the event
		'Dim bExecute As Boolean = True
		'
		'Get the ID of the control that generated the event
		'Dim sID As String = m_oWinMain.event.srcElement.id
		'
		'Raise the onclick event
		'RaiseEvent onbeforeupdate(Me, New onbeforeupdateEventArgs(sID, bExecute))
		'
		'Set the execute event flag
		'Return bExecute
		'
	'End Function
	
	
	'UPGRADE_NOTE: (7001) The following declaration (m_oDocMain_onclick) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function m_oDocMain_onclick() As Boolean
		'
		'
		'Do not cancel the event
		'Dim bExecute As Boolean = True
		'
		'Get the ID of the control that generated the event
		'Dim sID As String = m_oWinMain.event.srcElement.id
		'
		'Raise the onclick event
		'RaiseEvent onclick_Renamed(Me, New onclickEventArgs(sID, bExecute))
		'
		'Set the execute event flag
		'Return bExecute
		'
	'End Function
	
	'UPGRADE_NOTE: (7001) The following declaration (m_oDocMain_ondblclick) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function m_oDocMain_ondblclick() As Boolean
		'
		'
		'Do not cancel the event
		'Dim bExecute As Boolean = True
		'
		'Get the ID of the control that generated the event
		'Dim sID As String = m_oWinMain.event.srcElement.id
		'
		'Raise the onclick event
		'RaiseEvent ondblclick(Me, New ondblclickEventArgs(sID, bExecute))
		'
		'Set the execute event flag
		'Return bExecute
		'
	'End Function
	
	
	'UPGRADE_NOTE: (7001) The following declaration (m_oDocMain_ondragstart) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function m_oDocMain_ondragstart() As Boolean
		'
		'
		'Do not cancel the event
		'Dim bExecute As Boolean = True
		'
		'Get the ID of the control that generated the event
		'Dim sID As String = m_oWinMain.event.srcElement.id
		'
		'Raise the onclick event
		'RaiseEvent ondragstart(Me, New ondragstartEventArgs(sID, bExecute))
		'
		'Set the execute event flag
		'Return bExecute
		'
	'End Function
	
	
	'UPGRADE_NOTE: (7001) The following declaration (m_oDocMain_onerrorupdate) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function m_oDocMain_onerrorupdate() As Boolean
		'
		'
		'Do not cancel the event
		'Dim bExecute As Boolean = True
		'
		'Get the ID of the control that generated the event
		'Dim sID As String = m_oWinMain.event.srcElement.id
		'
		'Raise the onclick event
		'RaiseEvent onerrorupdate(Me, New onerrorupdateEventArgs(sID, bExecute))
		'
		'Set the execute event flag
		'Return bExecute
		'
	'End Function
	
	
	'UPGRADE_NOTE: (7001) The following declaration (m_oDocMain_onhelp) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function m_oDocMain_onhelp() As Boolean
		'
		'
		'Do not cancel the event
		'Dim bExecute As Boolean = True
		'
		'Get the ID of the control that generated the event
		'Dim sID As String = m_oWinMain.event.srcElement.id
		'
		'Raise the onclick event
		'RaiseEvent onhelp(Me, New onhelpEventArgs(sID, bExecute))
		'
		'Set the execute event flag
		'Return bExecute
		'
	'End Function
	
	
	Private Sub m_oDocMain_onkeydown() Handles m_oDocMain.onkeydown
		
		
		'Do not Cancel the Event
		Dim bExecute As Boolean = True
		
		'Get the ID of the control that generated the event
		Dim sID As String = m_oWinMain.event.srcElement.id
		
		'Get the Keycode
		Dim iKeyCode As Keys = m_oWinMain.event.keyCode
		
		'Raise the event
		RaiseEvent onkeypress_Renamed(Me, New onkeypressEventArgs(sID, iKeyCode, bExecute))
		
		'The execute event flag
		'm_oDocMain_onkeydown = bExecute
		
		If iKeyCode >= Keys.F1 And iKeyCode <= Keys.F12 Then

			m_oWinMain.event.returnValue = False
		End If
		
	End Sub
	
	'UPGRADE_NOTE: (7001) The following declaration (m_oDocMain_KeyDown) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Sub m_oDocMain_KeyDown(ByRef KeyCode As Integer, ByRef Shift As Integer)
		'
		'
		'Do not Cancel the Event
		'Dim bExecute As Boolean = True
		'
		'Get the ID of the control that generated the event
		'Dim sID As String = m_oWinMain.event.srcElement.id
		'
		'Get the Keycode
		'Dim iKeyCode As Keys = m_oWinMain.event.keyCode
		'
		'Raise the event
		'RaiseEvent onkeypress_Renamed(Me, New onkeypressEventArgs(sID, iKeyCode, bExecute))
		'
		'The execute event flag
		'm_oDocMain_onkeydown = bExecute
		'
		'If iKeyCode >= Keys.F1 And iKeyCode <= Keys.F12 Then

			'm_oWinMain.event.returnValue = False
		'End If
		'
	'End Sub
	
	'Private Function m_oDocMain_onkeypress() As Boolean
	
	'Dim bExecute As Boolean
	'Dim sID As String
	'Dim iKeyCode As Integer
	'
	'    'Do not Cancel the Event
	'    bExecute = True
	'
	'    'Get the ID of the control that generated the event
	'    sID = m_oWinMain.event.srcElement.ID
	'
	'    'Get the Keycode
	'    iKeyCode = m_oWinMain.event.keyCode
	'
	'    'Raise the event
	'    RaiseEvent onkeypress(sID, iKeyCode, bExecute)
	'
	'    'The execute event flag
	'm_oDocMain_onkeypress = bExecute
	
	'End Function
	
	
	Private Sub m_oDocMain_onmousedown() Handles m_oDocMain.onmousedown
		
		
		'Get the ID of the control that generated the event
		Dim sID As String = m_oWinMain.event.srcElement.id
		
		'Get the mouse button
		Dim lButton As Integer = m_oWinMain.event.button
		
		'Raise the onclick event
		RaiseEvent onmousedown_Renamed(Me, New onmousedownEventArgs(sID, lButton))
		
	End Sub
	
	Private Sub m_oDocMain_onmousemove() Handles m_oDocMain.onmousemove
		
		
		'Get the ID of the control that generated the event
		Dim sID As String = m_oWinMain.event.srcElement.id
		
		'Raise the onclick event
		RaiseEvent onmousemove_Renamed(Me, New onmousemoveEventArgs(sID))
		
	End Sub
	
	Private Sub m_oDocMain_onmouseout() Handles m_oDocMain.onmouseout
		
		
		'Get the ID of the control that generated the event
		Dim sID As String = m_oWinMain.event.srcElement.id
		
		'Raise the onclick event
		RaiseEvent onmouseout(Me, New onmouseoutEventArgs(sID))
		
	End Sub
	
	
	Private Sub m_oDocMain_onmouseover() Handles m_oDocMain.onmouseover
		
		
		'Get the ID of the control that generated the event
		Dim sID As String = m_oWinMain.event.srcElement.id
		
		'Raise the onclick event
		RaiseEvent onmouseover(Me, New onmouseoverEventArgs(sID))
		
	End Sub
	
	
	Private Sub m_oDocMain_onmouseup() Handles m_oDocMain.onmouseup
		
		
		'Get the ID of the control that generated the event
		Dim sID As String = m_oWinMain.event.srcElement.id
		
		'Get the mouse button
		Dim lButton As Integer = m_oWinMain.event.button
		
		'Raise the onclick event
		RaiseEvent onmouseup_Renamed(Me, New onmouseupEventArgs(sID, lButton))
		
	End Sub
	
	
	Private Sub Timer1_Tick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles Timer1.Tick
		
		m_lTimerValue += 1
		
	End Sub
	


    'TO BE DISSCUSSED
    'Private Sub UserControl_ReadProperties(ByRef PropBag As PropertyBag)



    '	m_sStartURL = CStr(PropBag.ReadProperty("StartURL", ACDefaultURL))


    '	m_bNavigateOnShow = CBool(PropBag.ReadProperty("NavigateOnShow", ACNavigateOnShow))


    '	m_blSilent = CBool(PropBag.ReadProperty("Silent", ACSilent))


    '	m_blContextMenu = CBool(PropBag.ReadProperty("ContextMenu", ACContextMenu))


    '	m_lTimeOut = CInt(PropBag.ReadProperty("TimeOut", ACTimeOut))

    'End Sub
	
	Private Sub uctPMDhtmlScreen_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
		
		wbMain.SetBounds(CInt(VB6.TwipsToPixelsX(10)), 0, CInt(ClientRectangle.Width - VB6.TwipsToPixelsX(10)), CInt(ClientRectangle.Height))
		
	End Sub
	
	Private Sub UserControl_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		'    Set m_oWBCutom = CreateObject("WBCustomizerLib.WBCustomizer")
		
		'    m_oWBCutom.EnableContextMenus = m_blContextMenu
		'    Set m_oWBCutom.WebBrowser = wbMain
		
		If m_bNavigateOnShow Then
			wbMain.Navigate(New URI(m_sStartURL))
		End If
		
	End Sub
	


    'TO BE DISCUSSED
    'Private Sub UserControl_WriteProperties(ByRef PropBag As PropertyBag)


    '	PropBag.WriteProperty("StartURL", m_sStartURL, ACDefaultURL)

    '	PropBag.WriteProperty("NavigateOnShow", m_bNavigateOnShow, ACNavigateOnShow)

    '	PropBag.WriteProperty("Silent", m_blSilent, ACSilent)

    '	PropBag.WriteProperty("ContextMenu", m_blContextMenu, ACContextMenu)

    '	PropBag.WriteProperty("TimeOut", m_lTimeOut, ACTimeOut)

    'End Sub
	
	

	Private Sub wbMain_Navigating(ByVal eventSender As Object, ByVal eventArgs As WebBrowserNavigatingEventArgs) Handles wbMain.Navigating
        Dim URL As String = eventArgs.Url.ToString()
		Dim TargetFrameName As String = eventArgs.TargetFrameName
		Dim Cancel As String = eventArgs.Cancel
		
		'close and destroy our current document object before navigating to a new document
		
		If Not (m_oDocMain Is Nothing) Then
			m_oDocMain.close()
		End If
		
		m_oDocMain = Nothing
		
	End Sub
	
	Private Sub InitialiseDocument()
		
		'insert some text into the document after it has been loaded
		'in the browser
		
		'm_oDocMain.body.insertAdjacentHTML "BeforeEnd", "<P>Hello sodding World</P>"
		'm_oDocMain.body.insertAdjacentHTML "BeforeEnd", "<P id=""XXX"" type=""TEXT"">Enter your name"
		'm_oDocMain.body.insertAdjacentHTML "BeforeEnd", "<input id=""xxx:yyy:1234"" type=""TEXT"" name=""txtInput"" size=""25"" maxlength=""30"">"
		'm_oDocMain.body.insertAdjacentHTML "BeforeEnd", "</P>"
		'm_oDocMain.body.insertAdjacentHTML "BeforeEnd", "<input type=""SUBMIT"" name=""btnSubmit"" value=""Submit"">"
		'm_oDocMain.All("txtInput").Value = "Jason"
		
		'use the All Collection of the document to find the HTML Elements we are interested in.
		'If the element has a valid NAME property then we can reference it directly, otherwise we
		'have to iterate through the collection looking for the ID of the element.
		
		'We could also use the Forms Collection of the document to get the elements of all HTML
		'forms declared in the document, in exactly the same manner.
		'
		'Syntax : m_oDocMain.Forms("frmMain").All("txtInput").value
		
		'set our instance of the Submit button to capture button events
		
		'Set btnSubmit = m_oDocMain.All("btnSubmit")
		
	End Sub
	
	

	Private Sub wbMain_DocumentCompleted(ByVal eventSender As Object, ByVal eventArgs As WebBrowserDocumentCompletedEventArgs) Handles wbMain.DocumentCompleted
        Dim URL As String = eventArgs.Url.ToString()
		
		'Refresh the document variables
		m_lReturn = CType(RefreshDocument(), gPMConstants.PMEReturnCode)
		
	End Sub
	

	Private Sub wbMain_DownloadBegin()
		
		' If m_blSilent is True then Kill web browser dialogs
		' If it's False the Show Dialogs

        'wbMain.Silent = m_blSilent
		
	End Sub
	

	Private Sub wbMain_DownloadComplete()
        m_bDownloadComplete = True
		
		'Refresh the document variables
		m_lReturn = CType(RefreshDocument(), gPMConstants.PMEReturnCode)
		
	End Sub
End Class