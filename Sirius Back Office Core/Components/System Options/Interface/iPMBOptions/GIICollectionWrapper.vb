Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
'Modified by Archana Tokas on 5/12/2010 11:11:28 AM refer developer guide no. 129
Imports SharedFiles
Friend NotInheritable Class CollectionWrapper 
	
	' ************************************************
	' Added to replace global variables 27/11/2003
	Private m_sUsername As String = ""
	Private m_sPassword As String = ""
	Private m_iUserID As Integer
	Private m_sCallingAppName As String = ""
	Private m_iSourceID As Integer
	Private m_iLanguageID As Integer
	Private m_iCurrencyID As Integer
	Private m_iLogLevel As Integer
	' ************************************************
	
	
	Private Const ACClass As String = "ObjectKeys"
	
	Private _m_cCollection As Collection = Nothing
	Private Property m_cCollection() As Collection
		Get
			If _m_cCollection Is Nothing Then
				_m_cCollection = New Collection()
			End If
			Return _m_cCollection
		End Get
		Set(ByVal Value As Collection)
			_m_cCollection = value
		End Set
	End Property
	
	
	' ***************************************************************** '
	' Name: Add
	'
	' Description:
	'
	' History: 01/11/2000 sj - Created.
	'
	' ***************************************************************** '
	Public Function Add(ByVal v_vItem As Object, Optional ByVal v_vKey As Object = Nothing, Optional ByRef r_vExists As Boolean = False) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			

			If Not Information.IsNothing(v_vKey) Then
				m_cCollection.Add(v_vItem, v_vKey)
			Else
				m_cCollection.Add(v_vItem)
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			If Information.Err().Number = 457 Then

				If Not Information.IsNothing(r_vExists) Then
					r_vExists = True
				End If
				Return result
			End If
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			iPMFunc.LogMessage(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Add Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	' ***************************************************************** '
	' Name: Item
	'
	' Description:
	'
	' History: 01/11/2000 sj - Created.
	'
	' ***************************************************************** '
	Public Function Item(ByVal v_vKey As Object, Optional ByRef r_vItem As Object = Nothing, Optional ByRef r_vExists As Boolean = False) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			

			If Not Information.IsNothing(r_vExists) Then
				r_vExists = True
			End If
			


			r_vItem = m_cCollection(v_vKey)
			
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			If Information.Err().Number = 5 Or Information.Err().Number = 9 Then

				If Not Information.IsNothing(r_vExists) Then
					r_vExists = False
				End If
				Return result
			End If
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			iPMFunc.LogMessage(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Item Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Item", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: Remove
	'
	' Description:
	'
	' History: 01/11/2000 sj - Created.
	'
	' ***************************************************************** '
	Public Function Remove(ByVal v_vKey As Object, Optional ByRef r_vExists As Boolean = False) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			

			If Not Information.IsNothing(r_vExists) Then
				r_vExists = True
			End If
			
			m_cCollection.Remove(v_vKey)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			If Information.Err().Number = 5 Or Information.Err().Number = 9 Then

				If Not Information.IsNothing(r_vExists) Then
					r_vExists = False
				End If
				Return result
			End If
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			iPMFunc.LogMessage(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Remove Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Remove", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	' ***************************************************************** '
	' Name: Count
	'
	' Description:
	'
	' History: 01/11/2000 sj - Created.
	'
	' ***************************************************************** '
	Public Function Count(ByRef r_lCount As Integer) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			r_lCount = m_cCollection.Count
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			iPMFunc.LogMessage(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Count Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Count", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	Protected Overrides Sub Finalize()
		m_cCollection = Nothing
	End Sub
End Class
