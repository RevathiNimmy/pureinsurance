Option Strict Off
Option Explicit On
Imports System
Friend NotInheritable Class EmptyClass 
	' ***************************************************************** '
	' Class Name: EmptyClass
	'
	' Date: 16/03/2001
	'
	' Description: This Class has been added to try and stop the Its4ME
	'              freeze problem. Creating a reference to this class in
	'              Sub Main will cause this component to never be released
	'              and has the same effect as setting the retained in memory
	'              setting in VB6.
	'
	'              See the following MS KB Articles for more information:
	'              Q186273, Q264957
	'
	' Edit History: RFC Creates 16/03/01
	'
	' ***************************************************************** '
End Class
