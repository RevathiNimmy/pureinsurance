Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
'developer guide no. 129
Imports SharedFiles
Friend NotInheritable Class PMNavKey 
	' ***************************************************************** '
	' Class Name: PMNavKeys
	'
	' Date: 04/01/99
	'
	' Description: PMNav_Key field
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "PMNavKeys"
	
	' PUBLIC Data Members (Begin)
	' PUBLIC Data Members (End)
	
	
	' PRIVATE Data Members (Begin)
	
	Private m_lPMNavKeyID As Integer
	Private m_sName As New FixedLengthString(30)
	Private m_sDescription As New FixedLengthString(255)
	Private m_lDataType As Integer
	Private m_iIsDeleted As Integer
	Private m_sEffectiveDate As String = ""
	
	Private m_bChanged As Boolean
	Private m_bIsNew As Boolean
	
	Private m_bHidden As Boolean
	
	
	
	Public Property IsHidden() As Boolean
		Get
			
			Return m_bHidden
			
		End Get
		Set(ByVal Value As Boolean)
			
			m_bHidden = Value
			
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
	
	
	Public Property Name() As String
		Get
			
			Return m_sName.Value
			
		End Get
		Set(ByVal Value As String)
			
			m_sName.Value = Value
			
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
	
	
	Public Property DataType() As Integer
		Get
			
			Return m_lDataType
			
		End Get
		Set(ByVal Value As Integer)
			
			m_lDataType = Value
			
		End Set
	End Property
	
	
	Public Property EffectiveDate() As String
		Get
			
			Return m_sEffectiveDate
			
		End Get
		Set(ByVal Value As String)
			
			m_sEffectiveDate = Value
			
		End Set
	End Property
	
	
	Public Property IsDeleted() As Integer
		Get
			
			Return m_iIsDeleted
			
		End Get
		Set(ByVal Value As Integer)
			
			m_iIsDeleted = Value
			
		End Set
	End Property
	
	
	Public Property HasChanged() As Boolean
		Get
			
			Return m_bChanged
			
		End Get
		Set(ByVal Value As Boolean)
			
			m_bChanged = Value
			
		End Set
	End Property
	
	Public Function SetProperties(Optional ByRef vName As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vDataType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			

            If Not Information.IsNothing(vName) Then

                m_sName.Value = CStr(vName)
            End If


            If Not Information.IsNothing(vDescription) Then

                m_sDescription.Value = CStr(vDescription)
            End If


            If Not Information.IsNothing(vDataType) Then

                m_lDataType = CInt(vDataType)
            End If


            If Not Information.IsNothing(vEffectiveDate) Then

                m_sEffectiveDate = CStr(vEffectiveDate)
            End If
			
			m_iIsDeleted = 0
			
			Return result
		
		Catch 
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
	End Function
	
	Public Sub New()
		MyBase.New()
		' Class Initialise Event.
		
		Try 
			
			m_sName.Value = ""
			m_sDescription.Value = ""
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			' Log Error Message
			gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", excep:=excep)
			
			Exit Sub
		End Try
		
	End Sub
End Class
