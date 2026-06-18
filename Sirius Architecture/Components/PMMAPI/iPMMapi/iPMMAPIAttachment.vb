Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
'Modified by Vijay Pal on 5/19/2010 10:33:38 AM refer developer guide no. 129
Imports SharedFiles

<System.Runtime.InteropServices.ProgId("Attachment_NET.Attachment")> _
Public NotInheritable Class Attachment 
	' ***************************************************************** '
	' Class Name: Attachment
	'
	' Date: 23rd Janaury 98
	'
	' Description: MAPI Attachment
	'
	' Edit History:
	' ***************************************************************** '
	
	Private Const ACClass As String = "Attachment"
	
	
	Private m_sName As String = ""
	Private m_sPath As String = ""
	Private m_eFileType As gPMConstants.PMEMapiAttachmentTypes
	Private m_oFunctions As Functions
	
	
	Public Property Name() As String
		Get
			Return m_sName
		End Get
		Set(ByVal Value As String)
			m_sName = Value
		End Set
	End Property
	
	
	Public Property Path() As String
		Get
			Return m_sPath
		End Get
		Set(ByVal Value As String)
			m_sPath = Value
		End Set
	End Property
	
	' RDC 13062002 gPMLibraries replaced with gPM* BAS Modules. Previous Get decl not allowed
	' RDC 13062002 gPMLibraries replaced with gPM* BAS Modules. Previous Get decl not allowed
	Public Property FileType() As Integer
		Get 'PMEMapiAttachmentTypes
			Return m_eFileType
		End Get
		Set(ByVal Value As Integer) 'PMEMapiAttachmentTypes)
			m_eFileType = Value
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
