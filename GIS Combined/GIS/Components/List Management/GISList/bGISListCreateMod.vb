Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Runtime.InteropServices
Module MainModule
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date:  30/03/2000
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bGISList"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	
	
	
	
	'------------------------------------------------------------------------
	
	'Polaris input file record
    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
 _
    Structure InputRecord
        <VBFixedString(12)> _
        Public PropertyId As String
        <VBFixedString(72)> _
        Public Description As String
        <VBFixedString(13)> _
        Public ABICode As String
        Public Shared Function CreateInstance() As InputRecord
            Dim result As New InputRecord
            'result.PropertyId = New FixedLengthString(12)
            'result.Description = New FixedLengthString(72)
            'result.ABICode = New FixedLengthString(13)
            Return result
        End Function
    End Structure
	
	'RLDF Header record
    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
 _
    Structure RLDFHeaderRecord
        <VBFixedString(10)> _
        Public PropertyId As String
        <VBFixedString(80)> _
        Public Filler As String
        'Public Shared Function CreateInstance() As RLDFHeaderRecord
        '    Dim result As New RLDFHeaderRecord
        '    'result.PropertyId = New FixedLengthString(10)
        '    'result.Filler = New FixedLengthString(80)
        '    Return result
        'End Function
    End Structure
	
	'RLDF Detail record
    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
 _
    Structure RLDFDetailRecord
        <VBFixedString(10)> _
        Public PropertyId As String
        <VBFixedString(70)> _
        Public Description As String
        <VBFixedString(10)> _
        Public ABICode As String
        'Public Shared Function CreateInstance() As RLDFDetailRecord
        '    Dim result As New RLDFDetailRecord
        '    'result.PropertyId = New FixedLengthString(10)
        '    'result.Description = New FixedLengthString(70)
        '    'result.ABICode = New FixedLengthString(10)
        '    Return result
        'End Function
    End Structure
	
	'RLDF Index record
    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
 _
    Structure RLDFIndexRecord
        <VBFixedString(10)> _
        Public PropertyId As String
        Dim RecordNumber As Integer
        Public Shared Function CreateInstance() As RLDFIndexRecord
            Dim result As New RLDFIndexRecord
            'result.PropertyId = New FixedLengthString(10)
            Return result
        End Function
    End Structure
	
	' Polaris Property array subscripts
	Public Const ACPAPropertyId As Integer = 0
	Public Const ACPARecordIndex As Integer = 1
	Public Const ACPACustom As Integer = 2
End Module