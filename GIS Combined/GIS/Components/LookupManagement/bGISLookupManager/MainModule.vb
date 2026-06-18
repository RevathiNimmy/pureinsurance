Option Strict Off
Option Explicit On
Imports System.Runtime.InteropServices
Module MainModule
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    Public Const ACApp As String = "bGIS"

    Private Const ACClass As String = "MainModule"

    ' UserID

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
                                                                 _
    Structure Lookup_Header_Index
        'Developer Guide no.130

        Public InsurerID As String
        Public BusinessType As String
        Public TableName As String
        Public Header_Start_ptr As String
        Public Header_End_ptr As String
        Public Shared Function CreateInstance() As Lookup_Header_Index
            Dim result As New Lookup_Header_Index
            result.InsurerID = Space(4)
            result.BusinessType = Space(4)
            result.TableName = Space(50)
            result.Header_Start_ptr = Space(7)
            result.Header_End_ptr = Space(7)
            Return result
        End Function
    End Structure

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
                                                                 _
    Structure Lookup_Header
        'Developer Guide no.130
        Public EffectiveDateTime As String
        Public ModifiedDateTime As String
        Public Status As String
        Public Definition As String
        Public ValidConstants As String
        Public DefaultValue As String
        Public Data_Start_ptr As String
        Public Data_End_ptr As String
        Public Shared Function CreateInstance() As Lookup_Header
            Dim result As New Lookup_Header
            'Developer Guide no.130
            result.EffectiveDateTime = Space(14)
            result.ModifiedDateTime = Space(14)
            result.Status = Space(1)
            result.Definition = Space(300)
            result.ValidConstants = Space(1000)
            result.DefaultValue = Space(200)
            result.Data_Start_ptr = Space(7)
            result.Data_End_ptr = Space(7)
            Return result
        End Function
    End Structure

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
                                                                 _
    Structure Lookup_Data
        'Developer Guide no.130
        Public Level As String
        Public LevelValue As String
        Public TypeOfLevel As String

        Public Shared Function CreateInstance() As Lookup_Data
            Dim result As New Lookup_Data
            result.Level = Space(30)
            result.LevelValue = Space(200)
            result.TypeOfLevel = Space(1)
            Return result
        End Function
    End Structure

    Public Const BinarySearchBackwards As Integer = -1
    Public Const BinarySearchForwards As Integer = 1
    Public Const BinarySearchFound As Integer = 10
    Public Const BinarySearchNotFound As Integer = 20

    'Lookup header field constants
    Public Const iLHInsurer As Integer = 0
    Public Const iLHBusinessType As Integer = 1
    Public Const iLHLookupName As Integer = 2
    Public Const iLHEffectiveDate As Integer = 3
    Public Const iLHModifiedDate As Integer = 4
    Public Const iLHStatus As Integer = 5
    Public Const iLHDefinition As Integer = 6
    Public Const iLHValidConstants As Integer = 7
    Public Const iLHDefaultValue As Integer = 8

    'lookup data field constants
    Public Const iLDLevel As Integer = 0
    Public Const iLDValue As Integer = 1
    Public Const iLDType As Integer = 2

    Public Const iLDLineTypeStep As Integer = 1
    Public Const iLDLineTypeGradient As Integer = 2
    Public Const iLDLineTypeConstant As Integer = 3

    Public Const iLDTableTypeStepGradient As Integer = 1
    Public Const iLDTableTypeConstant As Integer = 2

    'other constants
    Public Const iFirstRow As Integer = 0
End Module