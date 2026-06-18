Option Strict Off
Option Explicit On

Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports SharedFiles
Imports System.Threading

Friend NotInheritable Class InProgTasks
    Implements IDisposable
    Implements IEnumerable
    ' ***************************************************************** '
    ' Class Name: InProgTasks
    '
    ' Date: 11/11/1998
    '
    ' Description: Maintains the InProgTasks Collection.
    '
    '
    ' Edit History:
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "InProgTasks"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)
    ' Define the InProgTask Collection
    Private m_colInProgTasks As Collection

    ' Return Code
    Private lReturn As gPMConstants.PMEReturnCode

    ' PUBLIC Events (Begin)
    Public Event InProgTaskClose(ByVal v_lPMWrkTaskInstanceCnt As Integer, ByVal v_bStatusUpdated As Boolean)
    Public Event InProgTaskUpdateStatus(ByVal v_lPMWrkTaskInstanceCnt As Integer, ByVal v_bComplete As Boolean)
    ' PUBLIC Events (End)

    ' PRIVATE Data Members (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: UpdateStatus
    '
    ' Description: Update the Status for the In Progress Task, if it is
    '              a Scheduled Task. For a 'Do Now' Task then do nothing.
    ' ***************************************************************** '
    Public Sub UpdateStatus(ByVal v_lPMWrkTaskInstanceCnt As Integer, ByVal v_bComplete As Boolean)

        Try

            ' If this is a Scheduled Task, i.e. PMWrkTaskInstanceCnt > 0,
            ' then Raise the Update Status Event.
            If v_lPMWrkTaskInstanceCnt > 0 Then
                RaiseEvent InProgTaskUpdateStatus(v_lPMWrkTaskInstanceCnt, v_bComplete)
            End If

        Catch excep As System.Exception

            ' Log Error Message
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lPMWrkTaskInstanceCnt", v_lPMWrkTaskInstanceCnt)
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateStatusFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateStatus", excep:=excep, oDicParms:=oDict)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: MinimiseNavigators (Public)
    '
    ' Description: If no Instance ID supplied, ALL running Navigators
    '              will be minimised.
    '              If an Instance Id is supplied, all instances except
    '              the one supplied will be minimised.
    '
    ' ***************************************************************** '
    Public Function MinimiseNavigators(Optional ByVal v_lPMWrkTaskInstanceCnt As Integer = -1) As Integer

        Dim result As Integer = 0
        Dim oInProgTask As PMWorkManager.InProgTask

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For Each oInProgTask2 As PMWorkManager.InProgTask In m_colInProgTasks
                oInProgTask = oInProgTask2
                With oInProgTask
                    If (.IsNavigatorInstance) And (.PMWrkTaskInstanceCnt <> v_lPMWrkTaskInstanceCnt) Then
                        'TODO
                        '.Navigator.MinimiseNavigator()
                    End If
                End With
            Next oInProgTask2

            oInProgTask = Nothing

            Return result

        Catch excep As System.Exception

            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lPMWrkTaskInstanceCnt", v_lPMWrkTaskInstanceCnt)
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MinimiseNavigators Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MinimiseNavigators", excep:=excep, oDicParms:=oDict)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: StartNavigatorTask
    '
    ' Description: Starts a Navigator Task
    '
    ' ***************************************************************** '

    Public Function StartNavigatorTask(ByVal v_lPMNavProcessID As Integer, ByVal v_lPMAuthorityLevel As Integer, ByVal v_sNavXMLfile As String, Optional ByVal v_vSetKeyArray(,) As Object = Nothing, Optional ByVal v_lPMWrkTaskInstanceCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sWhatFailed As String = ""
        Dim oInProgTask As PMWorkManager.InProgTask
        Dim oNav As Object
        Dim iIndex As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add a New In Progress Task
            oInProgTask = Add(v_bIsNavigatorInstance:=True, v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt, v_sNavXMLfile:=v_sNavXMLfile)

            If oInProgTask Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' RDC 14012003
            oInProgTask.NavXMLfile = v_sNavXMLfile

            ' RDC 15012003
            'Tracy Richards 23/10/03 Trim the Nav file as it can be a padded blank string
            If v_sNavXMLfile.Trim() <> "" Then

                sWhatFailed = "Navigator XM"

                oNav = oInProgTask.NavigatorXM

                ' Set the Authority Level

                oNav.NavigatorV3_PMAuthorityLevel = v_lPMAuthorityLevel

                oNav.XMLFileName = v_sNavXMLfile

            Else

                sWhatFailed = "Navigator"

                oNav = oInProgTask.Navigator

                ' Set the Process ID and Authority Level

                oNav.ProcessID = v_lPMNavProcessID

                oNav.PMAuthorityLevel = v_lPMAuthorityLevel

            End If

            With oNav
                'Add the PMWrkTaskInstanceCnt key to the end of the array
                If Information.IsArray(v_vSetKeyArray) Then
                    iIndex = v_vSetKeyArray.GetUpperBound(1) + 1
                    ReDim Preserve v_vSetKeyArray(1, iIndex)
                Else
                    iIndex = 0
                    ReDim v_vSetKeyArray(1, iIndex)
                End If

                v_vSetKeyArray(0, iIndex) = "TaskInstanceCnt"

                v_vSetKeyArray(1, iIndex) = v_lPMWrkTaskInstanceCnt

                ' If there are any Set Keys then Set them
                If Information.IsArray(v_vSetKeyArray) Then

                    lReturn = .SetKeys(v_vSetKeyArray)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                        oDict.Add("v_lPMNavProcessID", v_lPMNavProcessID)
                        oDict.Add("v_lPMWrkTaskInstanceCnt", v_lPMWrkTaskInstanceCnt)
                        gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=sWhatFailed & " Set Keys Failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="StartNavigatorTask", oDicParms:=oDict)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

                ' start appropriate Navigator

                Dim newthread As System.Threading.Thread

                newthread = New Thread(AddressOf openapplication)

                newthread.SetApartmentState(ApartmentState.STA)

                newthread.IsBackground = True

                newthread.Start(oNav)

            End With

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lPMNavProcessID", v_lPMNavProcessID)
                oDict.Add("v_lPMWrkTaskInstanceCnt", v_lPMWrkTaskInstanceCnt)
                gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Start " & sWhatFailed, vApp:=ACApp, vClass:=ACClass, vMethod:="StartNavigatorTask", oDicParms:=oDict)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oInProgTask = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lPMNavProcessID", v_lPMNavProcessID)
            oDict.Add("v_lPMWrkTaskInstanceCnt", v_lPMWrkTaskInstanceCnt)
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StartNavigatorTaskFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="StartNavigatorTask", excep:=excep, oDicParms:=oDict)

            Return result

        End Try
    End Function

    Public Sub openapplication(ByVal oNav As Object)
        oNav.Initialise()
        oNav.Start()
        Application.Run()
    End Sub

    '' ***************************************************************** '
    '' Name: StartNavigatorXMTask
    ''
    '' Description: Starts a Navigator XM Task
    ''
    '' History:  RDC 19112002 Created
    '' ***************************************************************** '
    'Public Function StartNavigatorXMTask( _
    ''            ByVal v_lPMNavProcessID As Long, _
    ''            ByVal v_lPMAuthorityLevel As Long, _
    ''            Optional ByVal v_vSetKeyArray As Variant, _
    ''            Optional ByVal v_lPMWrkTaskInstanceCnt As Long = 0) As Long
    '
    '    On Error GoTo Err_StartNavigatorXMTask
    '
    '    StartNavigatorXMTask = PMFalse
    '
    '    StartNavigatorXMTask = PMTrue
    '
    '    Exit Function
    '
    'Err_StartNavigatorXMTask:
    '
    '    StartNavigatorXMTask = PMError
    '
    'End Function

    ' ***************************************************************** '
    ' Name: StartComponentTask
    '
    ' Description: Starts a Single Component Task
    '
    ' ***************************************************************** '
    Public Function StartComponentTask(ByVal v_sComponent As String, ByVal v_lPMAuthorityLevel As Integer, Optional ByVal v_vSetKeyArray(,) As Object = Nothing, Optional ByVal v_lPMWrkTaskInstanceCnt As Integer = 0, Optional ByVal v_bFromSchedule As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oInProgTask As PMWorkManager.InProgTask

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add a New In Progress Task
            oInProgTask = Add(v_bIsNavigatorInstance:=False, v_lPMWrkTaskInstanceCnt:=v_lPMWrkTaskInstanceCnt)
            If oInProgTask Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'AR20050428 - PN7388
            oInProgTask.FromSchedule = v_bFromSchedule

            With oInProgTask.Component

                ' If there are any Set Keys then Set them
                If Information.IsArray(v_vSetKeyArray) Then

                    lReturn = .StartComponent(v_sComponent:=v_sComponent, v_lPMAuthorityLevel:=v_lPMAuthorityLevel, v_vSetKeyArray:=v_vSetKeyArray)
                Else
                    lReturn = .StartComponent(v_sComponent:=v_sComponent, v_lPMAuthorityLevel:=v_lPMAuthorityLevel)
                End If

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                    oDict.Add("v_lPMWrkTaskInstanceCnt", v_lPMWrkTaskInstanceCnt)
                    gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Start Component :- " & v_sComponent, oDicParms:=oDict)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            oInProgTask = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lPMWrkTaskInstanceCnt", v_lPMWrkTaskInstanceCnt)
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StartComponentTaskFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="StartComponentTask", excep:=excep, oDicParms:=oDict)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Add (Public)
    '
    ' Description: Adds a InProgTask into the InProgTask Collection
    ' Note:        If the PMWrkTaskInstanceCnt is greater than zero,
    '              then this is a Scheduled Task, otherwise it is a
    '              Quick Start Task.
    ' ***************************************************************** '
    Public Function Add(ByVal v_bIsNavigatorInstance As Boolean, Optional ByVal v_sNavXMLfile As String = "", Optional ByVal v_lPMWrkTaskInstanceCnt As Integer = 0) As PMWorkManager.InProgTask

        Dim result As PMWorkManager.InProgTask = Nothing
        Dim oInProgTask As PMWorkManager.InProgTask
        Dim sKey As String = ""
        Static lInstanceNum As Integer

        Try

            ' Get the Next Instance Number
            ' Peter Finney 09/09/2003. Make variable static as we cannot reliably tie
            ' ourselves to the count as removing items in the middle of the collection
            ' will cause this to fail!! This will be fine as long as the user doesn't
            ' launch a chain of 4,294,967,296 tasks.
            If Count() = 0 Then
                ' No items so it's safe to restart the count
                lInstanceNum = 1
            Else
                ' Increment the static variable by 1
                lInstanceNum += 1
            End If

            ' Generate Key for the InProgTask
            sKey = GenerateKey(v_lInstanceNum:=lInstanceNum)

            ' Create a new InProgTask Instance
            oInProgTask = New PMWorkManager.InProgTask()

            ' RDC 14012003
            oInProgTask.NavXMLfile = v_sNavXMLfile

            ' Initialise New InProgTask
            ' Note: This will create a new Instance of Navigator,
            '       or Single Component Creator as required.

            'Tarun modified
            'lReturn = CType(oInProgTask.Initialise(v_bIsNavigatorInstance), gPMConstants.PMEReturnCode)
            lReturn = oInProgTask.Initialise(v_bIsNavigatorInstance)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return Nothing
            End If

            ' Set the Instance Properties.
            With oInProgTask
                .PMWrkTaskInstanceCnt = v_lPMWrkTaskInstanceCnt
                .Parent = Me
                .Key = sKey
            End With

            ' Add the InProgTask to the Collection
            m_colInProgTasks.Add(oInProgTask, sKey)

            ' Return reference to InProgTask Added
            result = oInProgTask

            ' Release the local reference to InProgTask
            oInProgTask = Nothing

            Return result

        Catch excep As System.Exception

            ' Error.

            result = Nothing

            ' Log Error Message
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lPMWrkTaskInstanceCnt", v_lPMWrkTaskInstanceCnt)
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Add Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", excep:=excep, oDicParms:=oDict)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Count
    '
    ' Description: Returns the number of InProgTasks in the collection.
    '
    '
    ' ***************************************************************** '
    Public Function Count() As Integer

        Dim result As Integer = 0
        Try

            Return m_colInProgTasks.Count

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
    ' Description: Delete a InProgTask from the Collection.
    '
    '
    ' ***************************************************************** '
    Public Sub Delete(ByRef vKey As String)
        Dim oInProgTask As PMWorkManager.InProgTask

        Try

            ' If we have a string key Trim It.
            If Information.VarType(vKey) = VariantType.String Then
                vKey = vKey.Trim()
            End If

            oInProgTask = Item(vKey)

            If oInProgTask Is Nothing Then
                Exit Sub
            End If

            With oInProgTask

                ' If this is a Scheduled Task, i.e. PMWrkTaskInstanceCnt > 0,
                ' then Raise the Update Status Event.
                If .PMWrkTaskInstanceCnt > 0 Then
                    ' Raise the Close Event, returning the Scheduled
                    ' Task Instance Key to which this InProgTask relates and whether
                    ' the Status for this Task has already been updated or not.
                    RaiseEvent InProgTaskClose(.PMWrkTaskInstanceCnt, .StatusUpdated)
                End If

            End With

            oInProgTask = Nothing

            ' Remove from the Collection based on the key
            m_colInProgTasks.Remove(vKey)

        Catch

            ' If there was nothing to delete just return
            Exit Sub
        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: Item
    '
    ' Description: Returns the selected InProgTask from the Collection.
    '
    '
    ' ***************************************************************** '
    Public Function Item(ByRef vKey As String) As PMWorkManager.InProgTask

        Try

            ' If we have a string key Trim It.
            If Information.VarType(vKey) = VariantType.String Then
                vKey = vKey.Trim()
            End If

            ' Return the Item from the Collection

            Return m_colInProgTasks(vKey)

        Catch

            ' If the Item is not found Return Nothing

            Return Nothing
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: DeleteAll
    '
    ' Description: Delete all InProgTasks from the Collection
    '
    '
    ' ***************************************************************** '
    Public Sub DeleteAll()

        Dim lFieldCount As Integer

        Try

            ' Determine the number of InProgTask in the collection
            lFieldCount = Count()

            ' Delete all the items
            For lSub As Integer = 1 To lFieldCount
                Delete(CStr(1))
            Next lSub

        Catch excep As System.Exception

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Delete All Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteAll", excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: Clear
    '
    ' Description: Clear the InProgTask Collection.
    '
    '
    ' ***************************************************************** '
    Public Sub Clear()

        Try

            ' Set InProgTask Collection to Nothing
            m_colInProgTasks = Nothing
            m_colInProgTasks = New Collection()

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

            Return m_colInProgTasks.GetEnumerator

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
                m_colInProgTasks = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)
    ' ***************************************************************** '
    ' Name: GenerateKey
    '
    ' Description: GenerateKeys a Key for the supplied details.
    '
    '
    ' ***************************************************************** '
    Private Function GenerateKey(ByVal v_lInstanceNum As Integer) As String

        Dim result As String = String.Empty


        ' Derive the InProgTask Index

        Return ("I" & Conversion.Str(v_lInstanceNum).Trim()).Trim()

    End Function
    ' PRIVATE Methods (End)

    Public Sub New()
        MyBase.New()

        Try

            ' Class Initialise
            m_colInProgTasks = New Collection()

        Catch excep As System.Exception

            ' Error.

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the InProgTask class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", excep:=excep)

            Exit Sub

        End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class

