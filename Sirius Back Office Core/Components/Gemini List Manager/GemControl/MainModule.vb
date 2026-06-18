Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
Module MainModule
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	Public Const ACApp As String = "GemControlLib"
	
	' New Polaris Data Types
	Public Const GEMPolUnknown As Integer = 0
	Public Const GEMPolDate As Integer = 1
	Public Const GEMPolNumeric As Integer = 2
	Public Const GEMPolShortList As Integer = 3
	Public Const GEMPolLongList As Integer = 4
	Public Const GEMPolText As Integer = 5
	Public Const GEMPolNumeric2 As Integer = 6
	Public Const GEMPolRef As Integer = 9
End Module