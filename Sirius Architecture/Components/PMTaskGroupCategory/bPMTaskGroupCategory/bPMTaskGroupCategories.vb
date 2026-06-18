Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports SharedFiles
Friend NotInheritable Class Categories
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: PMTaskGroupCategories
    '
    ' Date: 24th October 1996
    '
    ' Description: Maintains the PMTaskGroupCategories Collection.
    '
    '
    ' Edit History:
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "PMTaskGroupCategories"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)
    ' Define the PMTaskGroupCategory Collection
    Private m_PMTaskGroupCategories As Collection
    ' PRIVATE Data Members (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Add
    '
    ' Description: Adds a single PMTaskGroupCategory into the PMTaskGroupCategories Collection
    '
    '
    ' ***************************************************************** '
    Public Function Add(ByRef oNewPMTaskGroupCategory As bPMTaskGroupCategory.Category) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add the supplied PMTaskGroupCategory into the PMTaskGroupCategories Collection
            ' Do not specify a key as we do not know the PMTaskGroupCategory ID for
            ' new ones entered until they are added to the DB.
            m_PMTaskGroupCategories.Add(oNewPMTaskGroupCategory)

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add PMTaskGroupCategory to Collection", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Count
    '
    ' Description: Returns the number of PMTaskGroupCategories in the collection.
    '
    '
    ' ***************************************************************** '
    Public Function Count() As Integer

        Dim result As Integer = 0
        Try


            Return m_PMTaskGroupCategories.Count

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Count Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Count", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Delete
    '
    ' Description: Delete a PMTaskGroupCategory from the Collection.
    '
    '
    ' ***************************************************************** '
    Public Sub Delete(ByRef vKey As gPMConstants.PMEReturnCode)

        Try

            m_PMTaskGroupCategories.Remove(vKey)

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error Deleting from collection, Key=" & vKey, vApp:=ACApp, vClass:=ACClass, vMethod:="Delete", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: Item
    '
    ' Description: Returns the selected PMTaskGroupCategory from the Collection.
    '
    '
    ' ***************************************************************** '
    Public Function Item(ByRef vKey As gPMConstants.PMEReturnCode) As bPMTaskGroupCategory.Category

        Dim result As bPMTaskGroupCategory.Category = Nothing
        Try


            Return m_PMTaskGroupCategories(vKey)

        Catch excep As System.Exception




            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to return key=" & vKey, vApp:=ACApp, vClass:=ACClass, vMethod:="Item", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteAll
    '
    ' Description: Delete all PMTaskGroupCategories from the Collection
    '
    '
    ' ***************************************************************** '
    Public Sub DeleteAll()

        Dim iFieldCount As gPMConstants.PMEReturnCode

        Try

            ' Determine the number of PMTaskGroupCategory in the collection
            iFieldCount = Count()

            ' Delete all the items
            For i As Integer = 1 To iFieldCount
                Delete(gPMConstants.PMEReturnCode.PMTrue)
            Next i

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Delete All Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteAll", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: Clear
    '
    ' Description: Clear the PMTaskGroupCategory Collection.
    '
    '
    ' ***************************************************************** '
    Public Sub Clear()

        Try

            ' Set PMTaskGroupCategory Collection to Nothing
            m_PMTaskGroupCategories = Nothing
            m_PMTaskGroupCategories = New Collection()

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Clear Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Clear", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            m_PMTaskGroupCategories = New Collection()

        Catch excep As System.Exception



            ' Error.

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the PMTaskGroupCategory class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    Private Shared _DefaultInstance As Categories = Nothing
    Public Shared ReadOnly Property DefaultInstance() As Categories
        Get
            If _DefaultInstance Is Nothing Then
                _DefaultInstance = New Categories
            End If
            Return _DefaultInstance
        End Get
    End Property
End Class
