Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports SharedFiles
Friend NotInheritable Class Connections
    Implements IDisposable
    Implements IEnumerable
    ' ***************************************************************** '
    ' Class Name: Connections
    '
    ' Date: 09/11/1997
    '
    ' Description: Maintains the Connections Collection.
    '
    '
    ' Edit History:
    ' RFC0401998 - Database Type property added.
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Connections"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)
    ' Define the Connection Collection
    Private m_colConnections As Collection
    ' PRIVATE Data Members (End)

    ' PUBLIC Properties (Begin)
    ' PUBLIC Properties (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Add
    '
    ' Description: Adds a single Connection into the
    '              Connections Collection.
    '
    ' ***************************************************************** '
    Public Function Add(ByVal v_sDSN As String, ByVal v_sDatabase As String, ByVal v_oPMDAO As dPMDAO.Database) As Integer

        Dim result As Integer = 0
        Dim sKey As String = ""
        Dim oConnection As bPMDataControl.Connection

        Try

            ' RDC 19062002
            oConnection = New bPMDataControl.Connection()

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Generate the Connection Key
            sKey = GenerateKey(v_sDSN:=v_sDSN)

            oConnection = New bPMDataControl.Connection()

            ' Set the Connection Properties
            With oConnection
                .PMDAO = v_oPMDAO
                .DSN = v_sDSN
                .Database = v_sDatabase
            End With

            ' Add the supplied Connection into the collection
            m_colConnections.Add(oConnection, sKey)

            ' Release the Local Reference
            oConnection = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Connection to Collection", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GenerateKey
    '
    ' Description: Generates a Key from the supplied details.
    '
    '
    ' ***************************************************************** '
    Public Function GenerateKey(ByVal v_sDSN As String) As String

        Dim result As String = String.Empty
        Try

            ' Derive the Connection Key

            Return v_sDSN.Trim()

        Catch excep As System.Exception



            ' Error.
            result = ""

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GenerateKey", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateKey", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Count
    '
    ' Description: Returns the number of Connections in the collection.
    '
    '
    ' ***************************************************************** '
    Public Function Count() As Integer

        Dim result As Integer = 0
        Try


            Return m_colConnections.Count

        Catch excep As System.Exception



            ' Return zero

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Count Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Count", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Delete
    '
    ' Description: Delete a Connection from the Collection.
    '
    '
    ' ***************************************************************** '
    Public Sub Delete(ByRef v_vKey As String)

        Try

            ' If we have a string key Trim It.
            If Information.VarType(v_vKey) = VariantType.String Then
                v_vKey = v_vKey.Trim()
            End If

            ' Remove from the collection based on the Key
            m_colConnections.Remove(v_vKey)

        Catch



            ' If there is nothing to remove just return

            Exit Sub
        End Try


    End Sub

    ' ***************************************************************** '
    ' Name: Item
    '
    ' Description: Returns the selected Connection from the Collection.
    '
    '
    ' ***************************************************************** '
    Public Function Item(ByRef v_vKey As String) As bPMDataControl.Connection

        Try

            ' If we have a string key Trim It.
            If Information.VarType(v_vKey) = VariantType.String Then
                v_vKey = v_vKey.Trim()
            End If

            ' Return the Item from the Collection

            Return m_colConnections(v_vKey)

        Catch



            ' If the Item is not found return Nothing

            Return Nothing
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: DeleteAll
    '
    ' Description: Delete all Connections from the Collection
    '
    '
    ' ***************************************************************** '
    Public Sub DeleteAll()

        Dim lFieldCount As Integer

        Try

            ' Determine the number of Connection in the collection
            lFieldCount = Count()

            ' Delete all the items
            For lSub As Integer = 1 To lFieldCount
                Delete(CStr(1))
            Next lSub

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Delete All Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteAll", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: Clear
    '
    ' Description: Clear the Connection Collection.
    '
    '
    ' ***************************************************************** '
    Public Sub Clear()

        Try

            ' Set Connection Collection to Nothing
            m_colConnections = Nothing
            m_colConnections = New Collection()

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Clear Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Clear", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: NewEnum
    '
    ' Description: Provides For Each Type functionality by allowing
    '              access to the Native Collection Enumerator object.
    '
    '              This Method must be hidded and have a Procedure ID
    '              of -4. These attributes can be set via the Procedure
    '              Attributes dialog. (On Tools Menu).
    '
    ' ***************************************************************** '

    Public Function GetEnumerator() As IEnumerator Implements System.Collections.IEnumerable.GetEnumerator

        Try


            Return m_colConnections.GetEnumerator

        Catch



            Exit Function
        End Try

    End Function

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
                Clear()
                m_colConnections = Nothing
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

            m_colConnections = New Collection()

            ' Class Initialise

        Catch excep As System.Exception



            ' Error.

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the Connections class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class
