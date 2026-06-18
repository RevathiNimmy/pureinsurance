Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Friend Partial Class frmWinSock
	Inherits System.Windows.Forms.Form
	'====================================================================
	'   Class/Module: frmWinSock
	'   Description : Basic form to operate the Winsock control from
	'
	'   This is used to communicate with the third party tcp/ip
	'   interfaces.
	'
	'====================================================================
	'   Maintenance History
	'
	'    13 January 2005    Danny Davis    Created.
	'
	'====================================================================
	
	Const EchoPort As Integer = 7
	
	Const ACApp As String = "frmWinSock"
	
	Public m_sReceivedXml As String = ""
	Public m_sWinsockError As String = ""
	
	Private Sub Winsock1_DataArrival(ByVal eventSender As Object, ByVal eventArgs As AxMSWinsockLib.DMSWinsockControlEvents_DataArrivalEvent) Handles Winsock1.DataArrival
		' ---------------------------------------------------------------------------
		' PROCEDURE NAME: Winsock1_DataArrival
		' PURPOSE: Handle Data Arrival
		' AUTHOR: Danny Davis
		' DATE: 14 January 2005, 16:09:13
		' CHANGES:
		' ---------------------------------------------------------------------------
		
		
        Try

            m_sReceivedXml = New String("*", eventArgs.bytesTotal)
            Winsock1.GetData(m_sReceivedXml, VariantType.String, eventArgs.bytesTotal)



            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    m_sWinsockError = Information.Err().Description

            End Select

        Finally
        End Try

    End Sub
	
	Private Sub Winsock1_Error(ByVal eventSender As Object, ByVal eventArgs As AxMSWinsockLib.DMSWinsockControlEvents_ErrorEvent) Handles Winsock1.Error
		
		m_sWinsockError = eventArgs.description
	End Sub
	
	Private Sub Winsock1_SendComplete(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles Winsock1.SendComplete
		m_sReceivedXml = ""
		m_sWinsockError = ""
	End Sub
	Private Sub frmWinSock_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
		MemoryHelper.ReleaseMemory()
	End Sub
End Class