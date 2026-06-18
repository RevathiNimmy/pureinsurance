Option Strict Off
Option Explicit On
Imports System.Collections.ObjectModel
Imports SSP.Shared

Friend NotInheritable Class PMProductLookups
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: PMProductLookups
    '
    ' Date: 24th October 1996
    '
    ' Description: Maintains the PMProductLookups Collection.
    '
    '
    ' Edit History:
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "PMProductLookups"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)
    ' Define the PMProductLookup Collection
    Private m_PMProductLookups As PMProductLookupKeyedCollection
    ' PRIVATE Data Members (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Add
    '
    ' Description: Adds a single PMProductLookup into the PMProductLookups Collection
    '
    '
    ' ***************************************************************** '
    Public Function Add(ByRef oNewPMProductLookup As bPMProductLookup.PMProductLookup) As Integer
        Dim result As Integer = 0
        Dim sKey As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sKey = CStr(oNewPMProductLookup.PMProductID) & oNewPMProductLookup.TableName

            m_PMProductLookups.Add(oNewPMProductLookup)

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add PMProductLookup to Collection", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Count
    '
    ' Description: Returns the number of PMProductLookups in the collection.
    '
    '
    ' ***************************************************************** '
    Public Function Count() As Integer

        Dim result As Integer = 0
        Try


            Return m_PMProductLookups.Count

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
    ' Description: Delete a PMProductLookup from the Collection.
    '
    '
    ' ***************************************************************** '
    Public Sub Delete(ByRef vKey As gPMConstants.PMEReturnCode)

        Try

            m_PMProductLookups.Remove(vKey)

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error Deleting from collection, Key=" & vKey, vApp:=ACApp, vClass:=ACClass, vMethod:="Delete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: Item
    '
    ' Description: Returns the selected PMProductLookup from the Collection.
    '
    '
    ' ***************************************************************** '
    Public Function Item(ByRef vKey As gPMConstants.PMEReturnCode) As bPMProductLookup.PMProductLookup

        Dim result As bPMProductLookup.PMProductLookup = Nothing
        Try


            Return m_PMProductLookups(vKey)

        Catch excep As System.Exception




            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to return key=" & vKey, vApp:=ACApp, vClass:=ACClass, vMethod:="Item", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteAll
    '
    ' Description: Delete all PMProductLookups from the Collection
    '
    '
    ' ***************************************************************** '
    Public Sub DeleteAll()

        Dim iFieldCount As Integer

        Try

            ' Determine the number of PMProductLookup in the collection
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
    ' Description: Clear the PMProductLookup Collection.
    '
    '
    ' ***************************************************************** '
    Public Sub Clear()

        Try

            ' Set PMProductLookup Collection to Nothing
            m_PMProductLookups = Nothing
            m_PMProductLookups = New PMProductLookupKeyedCollection

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

            m_PMProductLookups = New PMProductLookupKeyedCollection

            ' Class Initialise

        Catch excep As System.Exception



            ' Error.

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the PMProductLookup class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    Private Shared _DefaultInstance As PMProductLookups = Nothing
    Public Shared ReadOnly Property DefaultInstance() As PMProductLookups
        Get
            If _DefaultInstance Is Nothing Then
                _DefaultInstance = New PMProductLookups
            End If
            Return _DefaultInstance
        End Get
    End Property
End Class
