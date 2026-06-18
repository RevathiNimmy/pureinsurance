Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports SharedFiles
Friend NotInheritable Class ScheduledTasks
    Implements IDisposable
    Implements IEnumerable
    ' ***************************************************************** '
    ' Class Name: ScheduledTasks
    '
    ' Date: 09/11/1998
    '
    ' Description: Maintains the Scheduled Tasks Collection.
    '
    '
    ' Edit History:
    ' DAK071099 - Add new columns to scheduled tasks
    ' DAK141299 - Tasks started from Available tasks should not be visisble
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "ScheduledTasks"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)
    ' Define the Navigator Collection
    Private m_colScheduledTasks As Collection
    ' PRIVATE Data Members (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Add
    '
    ' Description: Adds a single Scheduled Task into the Collection.
    '
    '
    ' ***************************************************************** '
    'DAK141299
    Public Function Add(ByVal v_lPMWrkTaskInstanceCnt As Integer, ByVal v_sCustomer As String, ByVal v_dtTaskDueDate As Date, ByVal v_lPmuserGroupID As Integer, ByVal v_vUserID As String, ByVal v_sDescription As String, ByVal v_iTaskStatus As Integer, ByVal v_iIsUrgent As Integer, ByVal v_iTypeOfTask As Integer, ByVal v_iIsSystemTask As Integer, ByVal v_sUser As String, ByVal v_sUserGroup As String, ByVal v_lNavProcessID As Integer, ByVal v_sComponentObjectName As String, ByVal v_sComponentClassName As String, ByVal v_lDisplayIcon As Integer, ByVal v_iIsViewOnlyTask As Integer, ByVal v_sLinkedObjectName As String, ByVal v_sLinkedClassName As String, ByVal v_sLinkedCaption As String, ByVal v_iIsVisible As Integer, ByRef r_sKey As String) As Integer

        Dim result As Integer = 0
        Dim oSchedTask As ScheduledTask
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            oSchedTask = New ScheduledTask()

            ' Initialise the Step
            lReturn = CType(oSchedTask, SSP.S4I.Interfaces.ILocalInterface).Initialise()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Generate the Step Key
            r_sKey = GenerateKey(v_lScheduledTaskCnt:=v_lPMWrkTaskInstanceCnt)

            ' Set the Step Properties
            With oSchedTask
                .PMWrkTaskInstanceCnt = v_lPMWrkTaskInstanceCnt
                .Customer = v_sCustomer
                .TaskDueDate = v_dtTaskDueDate
                .PmuserGroupID = v_lPmuserGroupID
                .UserID = v_vUserID
                .Description = v_sDescription
                .TaskStatus = v_iTaskStatus
                .IsUrgent = v_iIsUrgent
                .TypeOfTask = v_iTypeOfTask
                .IsSystemTask = v_iIsSystemTask
                .User = v_sUser
                .UserGroup = v_sUserGroup
                .PMNavProcessId = v_lNavProcessID
                .ComponentObjectName = v_sComponentObjectName
                .ComponentClassName = v_sComponentClassName
                .IsViewOnlyTask = v_iIsViewOnlyTask
                .LinkedObjectName = v_sLinkedObjectName
                .LinkedClassName = v_sLinkedClassName
                .LinkedCaption = v_sLinkedCaption
                'DAK141299
                .IsVisible = v_iIsVisible
                .Key = r_sKey
            End With

            ' Add the supplied step into the collection
            m_colScheduledTasks.Add(oSchedTask, r_sKey)

            ' Release the Local Reference
            oSchedTask = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lPMWrkTaskInstanceCnt", v_lPMWrkTaskInstanceCnt)
            oDict.Add("v_dtTaskDueDate", v_dtTaskDueDate)
            oDict.Add("v_lPmuserGroupID", v_lPmuserGroupID)
            oDict.Add("v_vUserID", v_vUserID)
            oDict.Add("v_lNavProcessID", v_lNavProcessID)
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
    Public Function GenerateKey(ByVal v_lScheduledTaskCnt As Integer) As String

        Dim result As String = String.Empty
        Try

            ' Derive the Key

            Return (ACSchedTaskPrefix & Conversion.Str(v_lScheduledTaskCnt).Trim()).Trim()

        Catch excep As System.Exception



            ' Error.
            result = ""

            ' Log Error Message
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lScheduledTaskCnt", v_lScheduledTaskCnt)
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


            Return m_colScheduledTasks.Count

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
            m_colScheduledTasks.Remove(v_vKey)

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
    Public Function Item(ByRef v_vKey As String) As ScheduledTask

        Try

            ' If we have a string key Trim It.
            If Information.VarType(v_vKey) = VariantType.String Then
                v_vKey = v_vKey.Trim()
            End If

            ' Return the Item from the Collection

            Return m_colScheduledTasks(v_vKey)

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
            m_colScheduledTasks = Nothing
            m_colScheduledTasks = New Collection()

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


            Return m_colScheduledTasks.GetEnumerator

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
            If disposing Then
                Clear()
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

            m_colScheduledTasks = New Collection()

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
