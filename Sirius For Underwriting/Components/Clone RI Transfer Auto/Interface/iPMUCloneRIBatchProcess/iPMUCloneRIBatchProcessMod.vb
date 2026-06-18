Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 25/09/2000
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iPMUCloneRIBatchProcess"

    ' Public interface constants used when
    ' retrieving data from the resource file.

    ' {* USER DEFINED CODE (Begin) *}

    ' General Icons
    ' Form
    ' Buttons

    ' Messages
    Public Const ACBusinessFailTitle As Short = 302
    Public Const ACBusinessFail As Short = 303
    Public Const ACStatusSearching As Short = 304
    Public Const ACStatusFound As Short = 305
    Public Const ACAmendTitle As Short = 315
    Public Const ACAmendProcessFail As Short = 319
    Public Const ACStatusFoundClaim As Short = 320

    Public Const ACDateColumn As Short = 2

    ' Public contants used for the start
    ' and end control indexes.
    Public Const ACControlStart As Short = 0
    Public Const ACControlEnd As Short = 1

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' Public source and language ID's from the
    ' Object Manager.
    Public g_iSourceID As Short
    Public g_iLanguageID As Short
    Public g_lInsuranceFileCnt As Integer

    ' Public instance of the object manager.
    Public g_oObjectManager As bObjectManager.ObjectManager
End Module