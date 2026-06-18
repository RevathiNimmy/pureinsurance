Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Friend NotInheritable Class PMNavKeyGSK 
	' ***************************************************************** '
	' Class Name: PMNavKeysGSK
	'
	' Description: Get Set Key Field
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "PMNavKeysGSK"
	
	' PUBLIC Data Members (Begin)
	' PUBLIC Data Members (End)
	
	
	' PRIVATE Data Members (Begin)
	
	Private m_lPMNavKeyID As Integer
	Private m_sDescription As New FixedLengthString(255)
	Private m_sInitialKeyValue As New FixedLengthString(255)
	Private m_iIsOptional As CheckState
	Private m_sGSKType As String = ""
	
	'Has this GSK been deleted
	Private m_bIsDeleted As Boolean
	
	'Has this GSK changed
	Private m_bChanged As Boolean
	
	'Is this a new GSK
	Private m_bIsNew As Boolean
	
	
	Public Property HasChanged() As Boolean
		Get
			
			Return m_bChanged
			
		End Get
		Set(ByVal Value As Boolean)
			
			m_bChanged = Value
			
		End Set
	End Property
	
	
	Public Property IsDeleted() As Boolean
		Get
			
			Return m_bIsDeleted
			
		End Get
		Set(ByVal Value As Boolean)
			
			m_bIsDeleted = Value
			
		End Set
	End Property
	
	
	
	Public Property IsNew() As Boolean
		Get
			
			Return m_bIsNew
			
		End Get
		Set(ByVal Value As Boolean)
			
			m_bIsNew = Value
			
		End Set
	End Property
	
	
	Public Property PMNavKeyID() As Integer
		Get
			
			Return m_lPMNavKeyID
			
		End Get
		Set(ByVal Value As Integer)
			
			m_lPMNavKeyID = Value
			
		End Set
	End Property
	
	
	Public Property InitialKeyValue() As String
		Get
			
			Return m_sInitialKeyValue.Value
			
		End Get
		Set(ByVal Value As String)
			
			m_sInitialKeyValue.Value = Value
			
		End Set
	End Property
	
	
	Public Property Description() As String
		Get
			
			Return m_sDescription.Value
			
		End Get
		Set(ByVal Value As String)
			
			m_sDescription.Value = Value
			
		End Set
	End Property
	
	
	Public Property IsOptional() As Integer
		Get
			
			Return m_iIsOptional
			
		End Get
		Set(ByVal Value As Integer)
			
			m_iIsOptional = Value
			
		End Set
	End Property
	
	
	Public Property GSKType() As String
		Get
			
			Return m_sGSKType
			
		End Get
		Set(ByVal Value As String)
			
			m_sGSKType = Value
			
		End Set
	End Property
	
	' PRIVATE Data Members (End)
	Public Function SetProperties(Optional ByRef vPMNavKeyID As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vInitialKeyValue As Object = Nothing, Optional ByRef vIsOptional As Object = Nothing, Optional ByRef vGSKType As Object = Nothing) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			

            If Not Information.IsNothing(vPMNavKeyID) Then

                m_lPMNavKeyID = CInt(vPMNavKeyID)
            End If


            If Not Information.IsNothing(vDescription) Then

                m_sDescription.Value = CStr(vDescription)
            End If


            If Not Information.IsNothing(vInitialKeyValue) Then

                m_sInitialKeyValue.Value = CStr(vInitialKeyValue)
            End If


            If Not Information.IsNothing(vIsOptional) Then

                m_iIsOptional = CInt(vIsOptional)
            End If


            If Not Information.IsNothing(vGSKType) Then

                m_sGSKType = CStr(vGSKType)
            End If
			
			Return result
		
		Catch 
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
	End Function
	
	Public Sub New()
		MyBase.New()
		' Class Initialise Event.
		
		Try 
			
			m_sDescription.Value = ""
			m_sInitialKeyValue.Value = ""
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			' Log Error Message
			gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", excep:=excep)
			
		End Try
		
	End Sub
End Class
