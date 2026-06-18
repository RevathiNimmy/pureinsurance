Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
'Developer Guide No: 129
Imports SharedFiles

Friend NotInheritable Class CLMInfoChklsts
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: CLMInfoChklsts
    ' Date: 06/10/1998
    ' Description: Maintains the CLMInfoChklsts Collection.
    ' Edit History:
    ' ***************************************************************** '
    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "CLMInfoChklsts"

    Private m_CLMInfoChklsts As Collection
    Private m_lReturn As Integer

    ' ***************************************************************** '
    ' Name: Add
    ' Description: Adds a single CLMInfoChklst into the CLMInfoChklsts Collection
    ' ***************************************************************** '
    Public Function Add(ByRef oNewCLMInfoChklst As bCLMInfoChklst.CLMInfoChklst) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add the supplied CLMInfoChklst into the CLMInfoChklsts Collection
            ' Do not specify a key as we do not know the CLMInfoChklst ID for
            ' new ones entered until they are added to the DB.
            m_CLMInfoChklsts.Add(oNewCLMInfoChklst)

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add CLMInfoChklst to Collection", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Count
    ' Description: Returns the number of CLMInfoChklsts in the collection.
    ' ***************************************************************** '
    Public Function Count() As Integer

        Dim result As Integer = 0
        Try


            Return m_CLMInfoChklsts.Count

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Count Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Count", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Delete
    ' Description: Delete a CLMInfoChklst from the Collection.
    ' ***************************************************************** '
    Public Sub Delete(ByRef vKey As Integer)

        Try

            m_CLMInfoChklsts.Remove(vKey)

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error Deleting from collection, Key=" & vKey, vApp:=ACApp, vClass:=ACClass, vMethod:="Delete", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: Item
    ' Description: Returns the selected CLMInfoChklst from the Collection.
    ' ***************************************************************** '
    Public Function Item(ByRef vKey As Integer) As bCLMInfoChklst.CLMInfoChklst

        Dim result As bCLMInfoChklst.CLMInfoChklst = Nothing
        Try


            Return m_CLMInfoChklsts(vKey)

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to return key=" & vKey, vApp:=ACApp, vClass:=ACClass, vMethod:="Item", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteAll
    ' Description: Delete all CLMInfoChklsts from the Collection
    ' ***************************************************************** '
    Public Sub DeleteAll()

        Dim lFieldCount As Integer

        Try

            ' Determine the number of CLMInfoChklst in the collection
            lFieldCount = Count()

            ' Delete all the items
            For lSub As Integer = 1 To lFieldCount
                Delete(1)
            Next lSub

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Delete All Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteAll", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: Clear
    ' Description: Clear the CLMInfoChklst Collection.
    ' ***************************************************************** '
    Public Sub Clear()

        Try

            ' Set CLMInfoChklst Collection to Nothing
            m_CLMInfoChklsts = Nothing


            ' Added by Scalability Update Program - 31/07/2002
            m_CLMInfoChklsts = New Collection()

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Clear Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Clear", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    ' Description: Entry point for any initialisation code for this
    '              object.
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
    ' Description: Entry point for any termination code for this
    '              object.
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
            End If
        End If
        Me.disposedValue = True
    End Sub



    Public Sub New()
        MyBase.New()

        Try

            ' Added by Scalability Update Program - 31/07/2002
            m_CLMInfoChklsts = New Collection()

            ' Class Initialise

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the CLMInfoChklst class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class
