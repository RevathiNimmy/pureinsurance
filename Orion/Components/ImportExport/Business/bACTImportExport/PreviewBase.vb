Option Strict Off
Option Explicit On
Imports System
Imports System.Xml
Public Interface PreviewBase
    'Developer Guide no.33,Made one dimentional array to 2-dimensional
    Function GetPreview(ByVal oRoot As XmlNode, ByRef vHeader() As Object, ByRef vDetail(,) As Object) As Integer
    Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer
    'Developer Guide no. 33,Made one dimentional array to 2-dimensional
    Function Update(ByVal oRoot As XmlNode, ByRef vHeader() As Object, ByRef vDetail(,) As Object) As Integer
End Interface
<System.Runtime.InteropServices.ProgId("PreviewBase_CoClass_NET.PreviewBase_CoClass")> _
Public NotInheritable Class PreviewBase_CoClass
    Implements IDisposable, PreviewBase

    ' Returns a preview of the supplied xml
    Public Function GetPreview(ByVal oRoot As XmlNode, ByRef vHeader() As Object, ByRef vDetail(,) As Object) As Integer Implements PreviewBase.GetPreview

    End Function

    ' Initialise the class
    Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer Implements PreviewBase.Initialise

    End Function

    ' Clean up resources
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' Updates the supplied xml with any changes
    Public Function Update(ByVal oRoot As XmlNode, ByRef vHeader() As Object, ByRef vDetail(,) As Object) As Integer Implements PreviewBase.Update

    End Function
End Class
