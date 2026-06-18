Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
Module MainModule
	
	' Constants for the party types
	Private Const ACPartyTypePC As String = "Personal"
	Private Const ACPartyTypeCC As String = "Corporate"
	Private Const ACPartyTypeGC As String = "Group"
	
	' This application
	Public Const ACApp As String = "iPMBPartyConvert"
	' This class
	Private Const ACClass As String = "MainModule"
	
    ' Instance of object manager
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	Public Const ACArrayPartyCnt As Integer = 0
	Public Const ACArrayShortName As Integer = 1
	Public Const ACArrayResolvedName As Integer = 2
	Public Const ACArrayPartyType As Integer = 3
	Public Const ACArrayNewPartyType As Integer = 4
	'ISS1118 add new const for PartyTypeCode
	Public Const ACArrayPartyTypeCode As Integer = 5
	Public Const ACArraySize As Integer = 5
End Module