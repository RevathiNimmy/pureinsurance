Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
'developer guide no. 129
Imports SharedFiles
Friend NotInheritable Class Propertys 
	Implements IEnumerable
	' ***************************************************************** '
	' Class Name: Params
	'
	' Date:  11/06/1999
	'
	' Description: Contains a collection of property objects.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "Propertys"
	
	
	
	' PUBLIC Data Members (Begin)
	' PUBLIC Data Members (End)
	
	' PRIVATE Data Members (Begin)
	
	
	Private m_colProperties As New Collection
	
	' PRIVATE Data Members (End)
	
	' PUBLIC Properties (Begin)
	' PUBLIC Properties (End)
	
	' PUBLIC Methods (Begin)
	
	Public Function Add(ByVal v_iRequired As Integer, ByVal v_sKey As String) As Integer
		
		'  Adds a Property object to the properties collection.
		
		Dim result As Integer = 0
        Dim oProperty As Property_Renamed
		
		Dim sKey As String = ""
		
		
		Try 
			
			oProperty = New Property_Renamed()
			
			oProperty.Required = v_iRequired
			
			m_colProperties.Add(oProperty, v_sKey)
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Add Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
		End Try
	End Function
	
	Public Function Remove(ByVal v_vIndex As String) As Integer
		
		'  Removes a Property object from the collection addressed by an index
		'  that is either a position index or a key value.
		
		Dim result As Integer = 0
		Try 
			
			m_colProperties.Remove(v_vIndex)
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Remove Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Remove", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			
			Return result
		End Try
	End Function
	
	Public Function Item(ByVal v_vIndex As String) As Property_Renamed
		
		'  Returns a Property object addressed by index which is either a position
		'  index or a key value.
		
		Try 
			
			
			Return m_colProperties(v_vIndex)
		
		Catch 
			
			
			
			Return Nothing
		End Try
		
	End Function
	
	Public Function Count() As Integer
		
		'  Returns number of properties in collection.
		
		Dim tempAuxVar As Integer = m_colProperties.Count
		
	End Function
	
	
	Public Function GetEnumerator() As IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
		
		'  NewEnum method allows the For Each ... Next loop of Param objects
		'  to be used outside of this class.
		'  Procedure Attributes are used to set the procedure id to -4
		'  (minus four) and the method is set to be hiden.
		
		Try 
			
			
			Return m_colProperties.GetEnumerator
		
		Catch 
			
			
			
			Return Nothing
		End Try
		
	End Function
	
	
	' PUBLIC Methods (End)
	
	' PRIVATE Methods (Begin)
	
	Public Sub New()
		MyBase.New()
	End Sub
	
	Protected Overrides Sub Finalize()
		
		m_colProperties = Nothing
		
	End Sub
	
	' PRIVATE Methods (End)
	Private Shared _DefaultInstance As Propertys = Nothing
	Public Shared ReadOnly Property DefaultInstance() As Propertys
		Get
			If _DefaultInstance Is Nothing Then
				_DefaultInstance = New Propertys
			End If
			Return _DefaultInstance
		End Get
	End Property
End Class
