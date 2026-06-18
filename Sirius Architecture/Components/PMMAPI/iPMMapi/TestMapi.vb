Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
Friend Partial Class Form1
	Inherits System.Windows.Forms.Form
	Private m_lReturn As Integer
	Private m_oPMMAPI As iPMMapi.PMMAPI
	
	Private Sub SendMessage()
		Dim pmeMapiToList As Object
		
		Const PMTrue As Integer = 1
		
		If m_oPMMAPI.Session Is Nothing Then

			m_lReturn = CInt(CType(m_oPMMAPI, SSP.S4I.Interfaces.ILocalInterface).Initialise())
			If m_lReturn <> PMTrue Then
				Exit Sub
			End If
		End If
		
		' Add a message to the Messages collection
		

		m_lReturn = CInt(m_oPMMAPI.Messages.AddMessage(v_vSubject:=txtSubject.Text, v_vNoteText:=txtNote.Text))
		
		If m_lReturn <> PMTrue Then
			Exit Sub
		End If
		
		' The LastItem is a Message
		
		With m_oPMMAPI.Messages.LastItem
			
			' The Recipient is in the address book so just supply name
			


			m_lReturn = CInt(.Recipients.AddRecipient(v_vName:=txtTo.Text, v_vRecipientType:=pmeMapiToList, v_vAddressBook:=True))
			
			If m_lReturn <> PMTrue Then
				Exit Sub
			End If
			
			If Not (txtFile.Text.Trim() = "") Then
				
				
				' Call the Attachment the same as the document
				

				m_lReturn = CInt(.Attachments.AddAttachment(v_vName:=txtFile.Text, v_vPath:=txtFile.Text))
				
				If m_lReturn <> PMTrue Then
					Exit Sub
				End If
				
			End If
			

			m_lReturn = CInt(.Send())
			
		End With
		
	End Sub
	
	Private Sub cmdSend_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSend.Click
		SendMessage()
		MessageBox.Show("Sent", Application.ProductName)
	End Sub
	

	Private Sub Form1_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		m_oPMMAPI = New iPMMapi.PMMAPI()
	End Sub
End Class