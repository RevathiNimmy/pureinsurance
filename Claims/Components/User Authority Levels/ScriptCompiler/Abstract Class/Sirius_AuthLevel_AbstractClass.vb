Option Strict Off
Option Explicit On
Imports System
<System.Runtime.InteropServices.ProgId("Rule_NET.Rule")> _
Public NotInheritable Class Rule 
	
	Public Sub SetDefaultValue(ByRef sTransactionType As Object)
	End Sub
	
	Public WriteOnly Property Extras() As Object
		Set(ByVal Value As Object)
			
		End Set
	End Property
	
	Public WriteOnly Property AuthorityLevelData() As Object
		Set(ByVal Value As Object)
			
		End Set
	End Property
	
	Public Sub Start()
		
	End Sub
	
	' adds a custom item to the collection jmf - added 9/6/2003
	Public Function AddObject(ByRef r_oObject As Object) As Object
		
	End Function
End Class
