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
	' Date          : 17-10-2002
	' Author        : Ram Chandrabose
	' Description   : Main module for iSIRPolicySummary.dll
	'
	' Edit History  :
	' RAM20021017   : Created
	' ***************************************************************** '
	
	
	' This application
	Public Const ACApp As String = "iSIRPolicySummary"
	' This class
	Private Const ACClass As String = "MainModule"
	
	' Navigator type constant
	Public Const PMKeyNamePartyCnt As String = "party_cnt"
	Public Const PMKeyNameShortName As String = "shortname"
	Public Const PMKeyNameInsuranceFolderCnt As String = "insurance_folder_cnt"
	Public Const PMKeyNameInsuranceFileCnt As String = "insurance_file_cnt"
	Public Const PMKeyNameInsReference As String = "insurance_ref"
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
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_oFormInterface As Form
End Module