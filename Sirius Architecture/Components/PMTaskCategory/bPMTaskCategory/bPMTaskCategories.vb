Option Strict Off
Option Explicit On
Imports System.Collections.ObjectModel
'developer guide no. 129
Imports SSP.Shared
Friend NotInheritable Class PMTaskCategories
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: PMTaskCategories
    '
    ' Date: 24th October 1996
    '
    ' Description: Maintains the PMTaskCategories Collection.
    '
    '
    ' Edit History:
    ' DAK100100 - prevent addition of duplicates.
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "PMTaskCategories"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)
    ' Define the PMTaskCategory Collection
    'Private m_PMTaskCategories As New List(Of Object)()
    'Private m_PMTaskCategories Dictionary(Of TKey, TValue)
    Private m_PMTaskCategories As Collection(Of Object)
    ' PRIVATE Data Members (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Add
    '
    ' Description: Adds a single PMTaskCategory into the PMTaskCategories Collection
    '
    '
    ' ***************************************************************** '
    Public Function Add(ByRef oNewPMTaskCategory As bPMTaskCategory.PMTaskCategory) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add the supplied PMTaskCategory into the PMTaskCategories Collection
            'DAK100100
            ' Use category code as the key - prevents adding duplicates
            m_PMTaskCategories.Add(oNewPMTaskCategory)

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add PMTaskCategory to Collection", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Count
    '
    ' Description: Returns the number of PMTaskCategories in the collection.
    '
    '
    ' ***************************************************************** '
    Public Function Count() As Integer

        Dim result As Integer = 0
        Try

            If m_PMTaskCategories.Count > 0 AndAlso m_PMTaskCategories.Item(0) Is Nothing Then
                Return m_PMTaskCategories.Count - 1
            Else
                Return m_PMTaskCategories.Count
            End If

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Count Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Count", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Delete
    '
    ' Description: Delete a PMTaskCategory from the Collection.
    '
    '
    ' ***************************************************************** '
    Public Sub Delete(ByRef vKey As Integer)

        Try

            m_PMTaskCategories.Remove(vKey)

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error Deleting from collection, Key=" & vKey, vApp:=ACApp, vClass:=ACClass, vMethod:="Delete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: Item
    '
    ' Description: Returns the selected PMTaskCategory from the Collection.
    '
    '
    ' ***************************************************************** '
    Public Function Item(ByRef vKey As Integer) As bPMTaskCategory.PMTaskCategory

        Dim result As bPMTaskCategory.PMTaskCategory = Nothing
        Try


            Return m_PMTaskCategories(vKey)

        Catch excep As System.Exception




            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to return key=" & vKey, vApp:=ACApp, vClass:=ACClass, vMethod:="Item", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteAll
    '
    ' Description: Delete all PMTaskCategories from the Collection
    '
    '
    ' ***************************************************************** '
    Public Sub DeleteAll()

        Dim iFieldCount As Integer

        Try

            ' Determine the number of PMTaskCategory in the collection
            iFieldCount = Count()

            ' Delete all the items
            For i As Integer = 1 To iFieldCount
                Delete(gPMConstants.PMEReturnCode.PMTrue)
            Next i

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Delete All Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteAll", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: Clear
    '
    ' Description: Clear the PMTaskCategory Collection.
    '
    '
    ' ***************************************************************** '
    Public Sub Clear()

        Try

            ' Set PMTaskCategory Collection to Nothing
            m_PMTaskCategories = Nothing
            'm_PMTaskCategories = New Collection()
            m_PMTaskCategories = New Collection(Of Object)

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Clear Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Clear", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Try


            ' Initialisation Code.

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' ***************************************************************** '
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

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)
    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        Try

            ' Class Initialise

            'm_PMTaskCategories = New Collection()
            m_PMTaskCategories = New Collection(Of Object)

        Catch excep As System.Exception



            ' Error.

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the PMTaskCategory class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    Private Shared _DefaultInstance As PMTaskCategories = Nothing
    Public Shared ReadOnly Property DefaultInstance() As PMTaskCategories
        Get
            If _DefaultInstance Is Nothing Then
                _DefaultInstance = New PMTaskCategories
            End If
            Return _DefaultInstance
        End Get
    End Property
End Class
