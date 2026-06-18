Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
'Modified by Vijay Pal on 5/19/2010 10:35:04 AM refer developer guide no. 129
Imports SharedFiles

<System.Runtime.InteropServices.ProgId("Recipient_NET.Recipient")> _
Public NotInheritable Class Recipient 
	' ***************************************************************** '
	' Class Name: Recipient
	'
	' Date: 23rd Janaury 98
	'
	' Description: Recipient of message
	'
	' Edit History:
	' ***************************************************************** '
	
	Private Const ACClass As String = "Recipient"
	
	Private m_lReturn As Integer
	Private m_oFunctions As Functions
	
	Private m_sAddress As String = ""
	Private m_sName As String = ""
	' Type is To, CC, BCC etc
	Private m_eRecipientType As gPMConstants.PMEMapiRecipientTypes
	Private m_bAddressBook As Boolean
	' SMTP: , MSFAX:
	Private m_sAddressType As String = ""
	
	
	Public Property Address() As String
		Get
			Return m_sAddress
		End Get
		Set(ByVal Value As String)
			m_sAddress = Value
		End Set
	End Property
	
	
	Public Property Name() As String
		Get
			Return m_sName
		End Get
		Set(ByVal Value As String)
			m_sName = Value
		End Set
	End Property
	
	' RDC 13062002 gPMLibraries replaced with gPM* BAS Modules. Previous Get decl not allowed
	' RDC 13062002 gPMLibraries replaced with gPM* BAS Modules. Previous Get decl not allowed
	Public Property RecipientType() As Integer
		Get 'PMEMapiRecipientTypes
			Return m_eRecipientType
		End Get
		Set(ByVal Value As Integer) 'PMEMapiRecipientTypes)
			m_eRecipientType = Value
		End Set
	End Property
	
	
	Public Property AddressBook() As Boolean
		Get

			If Convert.IsDBNull(m_bAddressBook) Or IsNothing(m_bAddressBook) Then
				m_bAddressBook = True
			End If
			Return m_bAddressBook
		End Get
		Set(ByVal Value As Boolean)
			m_bAddressBook = Value
		End Set
	End Property
	
	
	Public Property AddressType() As String
		Get
			If m_sAddressType = "" Then
				Return ""
			Else
				Return m_sAddressType & ":"
			End If
		End Get
		Set(ByVal Value As String)
			m_sAddressType = Value
		End Set
	End Property
	' ***************************************************************** '
	' Name: Initialise (Standard Method)
	'
	' Description: Entry point for any initialisation code for this
	'              object.
	'
	' ***************************************************************** '
	Public Function Initialise(ByRef oFunctions As Object) As Integer



		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Initialisation Code.
			
			m_oFunctions = oFunctions
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
End Class
