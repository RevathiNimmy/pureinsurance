Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
'Modified by Vijay Pal on 5/19/2010 10:33:54 AM refer developer guide no. 129
Imports SharedFiles
Friend NotInheritable Class Functions 
	' ***************************************************************** '
	' Class Name: Functions
	'
	' Date: 02 February 98
	'
	' Description: MAPI Message
	'
	' Edit History:
	' ***************************************************************** '
	
	Private Const ACClass As String = "Functions"
	
	Private m_oPMMAPI As PMMAPI
	
	' ***************************************************************** '
	' Name: Initialise (Standard Method)
	'
	' Description: Entry point for any initialisation code for this
	'              object.
	'
	' ***************************************************************** '
	Public Function Initialise(ByRef oPMMAPI As PMMAPI) As Integer



		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Initialisation Code.
			
			m_oPMMAPI = oPMMAPI
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ********************************************************************
	' Attempt to resolve an email recipient name
	' Ignore any errors as they will be trivial
	' ********************************************************************
	Private Sub ResolveName(ByRef m_oPMMAPI As PMMAPI, ByRef oRcp As Recipient)
		 
			
            With m_oPMMAPI.MessageControl
                .RecipDisplayName = oRcp.Name.Trim()
                .ResolveName()
            End With

	End Sub
	
	' ********************************************************************
	' Send a single message
	' ********************************************************************
	Public Function Send(ByRef oMsg As Message) As Integer
		
		Dim result As Integer = 0
		Dim oRcp As Recipient
		Dim oAtt As Attachment
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
            With m_oPMMAPI.MessageControl
                .Compose()
                .MsgSubject = oMsg.Subject
                .MsgNoteText = oMsg.NoteText
                For j As Integer = 1 To oMsg.Recipients.Count()
                    oRcp = oMsg.Recipients.Item(j)
                    .RecipIndex = j - 1
                    .RecipType = CShort(oRcp.RecipientType)
                    ' Get Address from Address Book
                    If oRcp.AddressBook Then
                        ResolveName(m_oPMMAPI, oRcp)
                    Else
                        .RecipDisplayName = oRcp.Name.Trim()
                        .RecipAddress = oRcp.AddressType & oRcp.Address
                    End If
                Next j
                For j As Integer = 1 To oMsg.Attachments.Count()
                    oAtt = oMsg.Attachments.Item(j)
                    .AttachmentIndex = j - 1
                    .MsgNoteText = .MsgNoteText & Strings.Chr(13).ToString() & Strings.Chr(10).ToString() & " "
                    .AttachmentPosition = Strings.Len(.MsgNoteText) - 1
                    .AttachmentName = oAtt.Name
                    .AttachmentPathName = oAtt.Path
                    .AttachmentType = CShort(oAtt.FileType)
                Next j
                .send()
            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFail

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Send", vApp:=ACApp, vClass:=ACClass, vMethod:="Send", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try



    End Function
End Class

