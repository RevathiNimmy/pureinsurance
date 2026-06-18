Option Strict Off
Option Explicit On

Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports SharedFiles

Friend NotInheritable Class AvailableTasks
    Implements IDisposable
    Implements IEnumerable
    ' ***************************************************************** '
    ' Class Name: AvailableTasks
    '
    ' Date: 30/11/1998
    '
    ' Description: Maintains the Available Tasks Collection.
    '
    '
    ' Edit History:
    ' DAK071099 - New task columns
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "AvailableTasks"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)
    ' Define the Navigator Collection
    Private m_colAvailableTasks As Collection
    ' PRIVATE Data Members (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Add
    '
    ' Description: Adds a single Scheduled Task into the Collection.
    '
    '
    ' DAK071099 - New task columns
    ' RDC 10012003 - Add v_sNavXMLfile
    ' ***************************************************************** '
    Public Function Add(ByVal v_lPMWrkTaskGroupID As Integer, ByVal v_lPMWrkTaskID As Integer, ByVal v_sTaskCaption As String, ByVal v_iIsSupervisor As Integer, ByVal v_iTypeOfTask As Integer, ByVal v_iIsSystemTask As Integer, ByVal v_lPMNavProcessID As Integer, ByVal v_sObjectName As String, ByVal v_sClassName As String, ByVal v_lDeleteAfterDays As Integer, ByVal v_lDisplayIcon As Integer, ByVal v_iIsViewOnlyTask As Integer, ByVal v_sLinkedObjectName As String, ByVal v_sLinkedClassName As String, ByVal v_sLinkedCaption As String, ByRef r_sKey As String, ByVal v_sNavXMLfile As String) As Integer

        Dim result As Integer = 0
        Dim oAvailableTask As PMWorkManager.AvailableTask
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            oAvailableTask = New PMWorkManager.AvailableTask()

            ' Initialise the Step

            lReturn = oAvailableTask.Initialise()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Generate the Available Task Key
            r_sKey = GenerateKey(v_lTaskGroupID:=v_lPMWrkTaskGroupID, v_lTaskID:=v_lPMWrkTaskID)

            ' Set the Step Properties
            With oAvailableTask
                .TaskGroupID = v_lPMWrkTaskGroupID
                .TaskID = v_lPMWrkTaskID
                .TaskCaption = v_sTaskCaption
                .IsSupervisor = v_iIsSupervisor
                .TypeOfTask = v_iTypeOfTask
                .IsSystemTask = v_iIsSystemTask
                .PMNavProcessId = v_lPMNavProcessID
                .ComponentObjectName = v_sObjectName
                .ComponentClassName = v_sClassName
                .AutoDeleteAfterNumDays = v_lDeleteAfterDays
                .DisplayIcon = v_lDisplayIcon
                'DAK071099
                .IsViewOnlyTask = v_iIsViewOnlyTask
                .LinkedObjectName = v_sLinkedObjectName
                .LinkedClassName = v_sLinkedClassName
                .LinkedCaption = v_sLinkedCaption
                ' RDC 10012003
                .NavXMLfile = v_sNavXMLfile
            End With

            ' Add into the collection
            m_colAvailableTasks.Add(oAvailableTask, r_sKey)

            ' Release the Local Reference
            oAvailableTask = Nothing

            Return result

        Catch excep As System.Exception

            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lPMWrkTaskGroupID", v_lPMWrkTaskGroupID)
            oDict.Add("v_lPMWrkTaskID", v_lPMWrkTaskID)
            oDict.Add("v_lPMNavProcessID", v_lPMNavProcessID)
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Step to Collection", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", excep:=excep, oDicParms:=oDict)

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
    Public Function GenerateKey(ByVal v_lTaskGroupID As Integer, ByVal v_lTaskID As Integer) As String

        Dim result As String = String.Empty
        Dim sKey As String = ""

        Try

            ' Derive the Key
            sKey = (ACTaskGroupPrefix & Conversion.Str(v_lTaskGroupID).Trim()).Trim()
            sKey = sKey & (ACTaskPrefix & Conversion.Str(v_lTaskID).Trim()).Trim()

            ' Return the Key

            Return sKey

        Catch excep As System.Exception

            ' Error.
            result = ""

            ' Log Error Message
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lTaskGroupID", v_lTaskGroupID)
            oDict.Add("v_lTaskID", v_lTaskID)
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GenerateKey", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateKey", excep:=excep, oDicParms:=oDict)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Count
    '
    ' Description: Returns the number of Steps in the collection.
    '
    '
    ' ***************************************************************** '
    Public Function Count() As Integer

        Dim result As Integer = 0
        Try

            Return m_colAvailableTasks.Count

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Count Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Count", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Delete
    '
    ' Description: Delete a Step from the Collection.
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
            m_colAvailableTasks.Remove(v_vKey)

        Catch

            ' If there is nothing to remove just return

            Exit Sub
        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: Item
    '
    ' Description: Returns the selected Step from the Collection.
    '
    '
    ' ***************************************************************** '
    Public Function Item(ByRef v_vKey As String) As AvailableTask

        Try

            ' If we have a string key Trim It.
            If Information.VarType(v_vKey) = VariantType.String Then
                v_vKey = v_vKey.Trim()
            End If

            ' Return the Item from the Collection

            Return m_colAvailableTasks(v_vKey)

        Catch

            ' If the Item is not found return Nothing

            Return Nothing
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: Clear
    '
    ' Description: Clear the Navigator Collection.
    '
    '
    ' ***************************************************************** '
    Public Sub Clear()

        Try

            ' Set Step Collection to Nothing
            m_colAvailableTasks = Nothing
            m_colAvailableTasks = New Collection()

        Catch excep As System.Exception

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Clear Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Clear", excep:=excep)

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

            Return m_colAvailableTasks.GetEnumerator

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
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

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
            Clear()
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

            m_colAvailableTasks = New Collection()

        Catch excep As System.Exception

            ' Error.

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the Navigator class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", excep:=excep)

            Exit Sub

        End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class
