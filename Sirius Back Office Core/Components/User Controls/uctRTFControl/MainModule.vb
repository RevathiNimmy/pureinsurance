Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
Module MainModule
	'******************************************************************************
	' Module Name:      MainModule
	' History:          Created 22 Aug 2000
	' Description:      Main module containing public variable/constants.
	'******************************************************************************
	
	' Main public constant for all functions to identify which application this is
	Public Const ACApp As String = "uctRichTextBoxControl"
    Public Const kToolbarButtonUndo As String = "butUndo"
    Public Const kToolbarButtonRedo As String = "butRedo"
    Public Const kToolbarButtonCut As String = "butCut"
    Public Const kToolbarButtonCopy As String = "butCopy"
    Public Const kToolbarButtonPaste As String = "butPaste"
    Public Const kToolbarButtonColor As String = "butColor"
    Public Const kToolbarButtonFont As String = "butFont"
    Public Const kToolbarButtonStrikeThru As String = "butStrikeThru"
    Public Const kToolbarButtonUnderline As String = "butUnderline"
    Public Const kToolbarButtonItalic As String = "butItalic"
    Public Const kToolbarButtonBold As String = "butBold"
    Public Const kToolbarButtonLeft As String = "butLeft"
    Public Const kToolbarButtonCenter As String = "butCenter"
    Public Const kToolbarButtonRight As String = "butRight"
    Public Const kToolbarButtonIncreaseIndent As String = "butIncreaseIndent"
    Public Const kToolbarButtonDecreaseIndent As String = "butDecreaseIndent"
    Public Const kToolbarButtonBullet As String = "butBullet"
    Public Const kToolbarButtonPrint As String = "butPrint"
    Public Const kToolbarButtonPreview As String = "butPreview"
End Module