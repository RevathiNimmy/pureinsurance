Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
'developer guide no. 129
Imports SharedFiles
Friend NotInheritable Class Maps
    Implements IDisposable
    Implements IEnumerable
    ' ***************************************************************** '
    ' Class Name: Maps
    '
    ' Date: 01/09/1998
    '
    ' Description: Maintains the Maps Collection.
    '
    '
    ' Edit History:
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Maps"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)
    ' Define the Map Collection
    Private m_colMaps As Collection

    ' Return Code
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' PRIVATE Data Members (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Add (Public)
    '
    ' Description: Adds a Map into the Map Collection
    '
    ' ***************************************************************** '
    Public Function Add(ByVal v_lMapID As Integer, Optional ByVal v_oParentStep As Step_Renamed = Nothing) As iPMNavigator.Map

        Dim result As iPMNavigator.Map = Nothing
        Dim oMap As iPMNavigator.Map
        Dim sKey As String = ""

        Try


            ' Generate Key for the Map
            sKey = GenerateKey(v_lMapID:=CStr(v_lMapID))

            ' Have we already Added this Map into the Collection

            ' Try and get this map from the collection
            oMap = Nothing
            oMap = Item(sKey)

            ' We have already added it, return referece to it.
            If Not (oMap Is Nothing) Then
                Return oMap
            End If

            ' Create a new Map Instance
            oMap = New iPMNavigator.Map()

            ' Initialise New Map
            'developer guide no. 9
            m_lReturn = oMap.Initialise()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return Nothing
            End If

            ' Set the Parent Step
            oMap.ParentStep = v_oParentStep

            ' Load Map
            m_lReturn = CType(oMap.LoadExisting(v_lMapID:=v_lMapID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return Nothing
            End If

            ' Add the Map to the Collection
            m_colMaps.Add(oMap, sKey)

            ' Return reference to Map Added
            result = oMap

            ' Release the local reference to Map
            oMap = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.

            result = Nothing

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Add Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GenerateKey
    '
    ' Description: GenerateKeys a Key for the supplied details.
    '
    '
    ' ***************************************************************** '
    Public Function GenerateKey(ByVal v_lMapID As String) As String

        Dim result As String = String.Empty
        Try

            ' Derive the Map Index

            Return (gPMConstants.PMMapKeyPrefix & Conversion.Str(v_lMapID).Trim()).Trim()

        Catch excep As System.Exception



            ' Error.
            result = ""

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GenerateKey StepSummary in Collection", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateKey", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Count
    '
    ' Description: Returns the number of Maps in the collection.
    '
    '
    ' ***************************************************************** '
    Public Function Count() As Integer

        Dim result As Integer = 0
        Try


            Return m_colMaps.Count

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Count Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Count", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Delete
    '
    ' Description: Delete a Map from the Collection.
    '
    '
    ' ***************************************************************** '
    Public Sub Delete(ByRef vKey As String)

        Try

            ' If we have a string key Trim It.
            If Information.VarType(vKey) = VariantType.String Then
                vKey = vKey.Trim()
            End If

            ' Remove from the Collection based on the key
            m_colMaps.Remove(vKey)

        Catch



            ' If there was nothing to delete just return
            Exit Sub
        End Try


    End Sub

    ' ***************************************************************** '
    ' Name: Item
    '
    ' Description: Returns the selected Map from the Collection.
    '
    '
    ' ***************************************************************** '
    Public Function Item(ByRef vKey As String) As iPMNavigator.Map

        Try

            ' If we have a string key Trim It.
            If Information.VarType(vKey) = VariantType.String Then
                vKey = vKey.Trim()
            End If

            ' Return the Item from the Collection

            Return m_colMaps(vKey)

        Catch



            ' If the Item is not found Return Nothing


            Return Nothing
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: Clear
    '
    ' Description: Clear the Map Collection.
    '
    '
    ' ***************************************************************** '
    Public Sub Clear()

        Try

            ' Set Map Collection to Nothing
            m_colMaps = Nothing
            m_colMaps = New Collection()

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Clear Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Clear", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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


            Return m_colMaps.GetEnumerator

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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                m_colMaps = Nothing
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

            m_colMaps = New Collection()

            ' Class Initialise

        Catch excep As System.Exception



            ' Error.

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the Map class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class
