' ************************************************************************************************************************
' Module:       Implements the legacy Sirius hash algorithm.
' Copyright:    © 1997-2007 by Sirius Financial Solutions plc. All rights reserved worldwide.
' Usage:        Customer applications and Sirius code.
'
' This code *MUST NOT* be used for any purpose other than interaction with existing code and data.
' If encryption is necessary in new code, use the official algorithms provided by the .NET class library.
'
' In addition, this code module *MUST NOT* be exposed publicly in an assembly. Instead, it should be shared
' between projects in the traditional way. The class is marked as Friend to ensure this.
' ************************************************************************************************************************

Option Explicit On
Option Strict On

Imports System
Imports System.Text

''' <summary>
''' This class implements the legacy Sirius hash algorithm.
''' </summary>
Friend NotInheritable Class SiriusLegacyHash

#Region "Constructors"

    ''' <summary>
    ''' This class cannot be instantiated.
    ''' </summary>
    Private Sub New()
    End Sub

#End Region

#Region "Public Shared Methods"

    ''' <summary>
    ''' Hash a string using the legacy Sirius hash algorithm.
    ''' </summary>
    ''' <param name="data">The input to compute the hash code for.</param>
    ''' <returns>The computed hash code.</returns>
    Public Shared Function ComputeHash(ByVal data As String) As String

        Const characterMapping As String = "aPCXADneGgH7khIJpjKtBMzmQLrRcqSEsbUv6yuVFW9xYZ2T3fd4w5N8"

        If String.IsNullOrEmpty(Data) Then
            Return String.Empty
        End If

        Dim char1 As Char = characterMapping(((Convert.ToInt32(data(0)) + data.Length) Mod characterMapping.Length))
        Dim char2 As Char = characterMapping(((Convert.ToInt32(data(data.Length - 1))) Mod characterMapping.Length))
        Dim sn As Integer = ((Convert.ToInt32(char1) + Convert.ToInt32(char2)) Mod characterMapping.Length) + 1
        Dim hash As New StringBuilder(char2, data.Length + 2)
        For dataIndex As Integer = 0 To data.Length - 1
            hash.Append(characterMapping(((Convert.ToInt32(data(dataIndex)) + sn + dataIndex + 1) Mod characterMapping.Length)))
        Next
        hash.Append(char1)

        Return hash.ToString().Trim(" "c)

    End Function

#End Region

End Class
