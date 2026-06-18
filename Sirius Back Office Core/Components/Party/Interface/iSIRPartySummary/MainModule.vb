Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.IO
Imports System.Windows.Forms
Module MainModule
	' ***************************************************************** '
	' Modlue Name   : MainModule
	' File Name     : MainModule.bas
	' Date          : 16-10-2002
	' Author        : Ram Chandrabose
	' Description   : Main module for iSIRPartySummary.dll
	'
	' Edit History  :
	' RAM20021016   : Created
	' ***************************************************************** '
	
	
	' This application
	Public Const ACApp As String = "iSIRPartySummary"
	' This class
	Private Const ACClass As String = "MainModule"
	
	' Navigator type constant
	Public Const PMKeyNamePartyCnt As String = "party_cnt"
	Public Const PMKeyNameShortName As String = "shortname"
	Public Const PMKeyNameDisplayMode As String = "display_mode"
	

	Public ACShowFormModal As FormShowConstants = FormShowConstants.Modal

	Public ACShowFormModeLess As FormShowConstants = FormShowConstants.Modeless
	
    ' Instance of object manager
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_sUnderwriting As String = "" ' To hold the flag for Underwriting / Broking
    'developer guide no.291
    'developer guide no. 107
    <ThreadStatic()> _
    Public m_oFormInterface As Object
End Module