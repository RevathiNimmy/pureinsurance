Option Strict Off
Option Explicit On
Imports System
<System.Runtime.InteropServices.ProgId("PickListKey_NET.PickListKey")> _
Public NotInheritable Class PickListKey 
	
	Public KeyName As String = ""
	Public Value As Object
	Public ValueType As Integer
	'SD 05/08/2002 Scalability changes
	'Public KeyType As PMEDataType
	Public IsPK As Boolean
End Class
