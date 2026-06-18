Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.IO
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: 16/06/1999
    '
    ' Description: Interface class. Called to start navigator process.
    '
    ' Edit History:
    ' RAW 14/02/2003 : ISS2153 : added m_oNavigatorXM and launch it from a task rather than a process
    ' ***************************************************************** '


    ' ************* { Public Events } ************************************
    Public Event NavigatorClose()
    Public Event SetProcessStatus(ByVal v_bProcessComplete As Boolean)

    ' ************* { Private Variables Begin } **************************

    ' This class
    Private Const ACClass As String = "Interface"

    ' Return value
    Private m_lReturn As SharedFiles.gPMConstants.PMEReturnCode

    ' Properties
    Private m_lProcessID As Integer
    Private m_sProcessCode As String = ""
    Private m_lPMAuthorityLevel As Integer
    Private m_sTaskCode As String = "" ' RAW 14/02/2003 : ISS2153 : added
    Private m_sNavXMLFile As String = "" ' RAW 14/02/2003 : ISS2153 : added

    Private m_vKeyArray As Object ' RAW 14/02/2003 : ISS2153 : added

    ' Have a calling app name property, so that when errors are logged they're
    ' logged to this objects parent, instead of iPMNavStart.
    Private m_sCallingAppName As String = ""

    ' Instance of Navigator

    Private WithEvents m_oNavigator As iPMNavigator.NavigateControl

    ' RAW 14/02/2003 : ISS2153 : added
    Private WithEvents m_oNavigatorXM As iPMNavigatorXM.Interface_Renamed

    ' Instance of Object Manager
    Private g_oObjectManager As bObjectManager.ObjectManager

    ' Has navigator been closed
    Private m_bNavClosed As Boolean


    ' ************* { Private Variables End } ****************************

    ' ************* { Public Properties Begin } **************************

    ' This objects product family
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return SharedFiles.gPMConstants.PMEProductFamily.pmePFSiriusArchitecture
        End Get
    End Property

    ' CallingAppName
    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            m_sCallingAppName = Value
        End Set
    End Property

    ' PMAuthorityLevel Property Let
    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = Value
        End Set
    End Property

    ' Process ID
    Public Property ProcessID() As Integer
        Get
            Return m_lProcessID
        End Get
        Set(ByVal Value As Integer)
            m_lProcessID = Value
        End Set
    End Property

    ' Process Code
    Public Property ProcessCode() As String
        Get
            Return m_sProcessCode
        End Get
        Set(ByVal Value As String)
            m_sProcessCode = Value
        End Set
    End Property

    ' RAW 14/02/2003 : ISS2153 : added
    ' Navigator XML file name
    Public Property NavXMLFile() As String
        Get
            Return m_sNavXMLFile
        End Get
        Set(ByVal Value As String)
            m_sNavXMLFile = Value
        End Set
    End Property

    ' Task Code
    Public Property TaskCode() As String
        Get
            Return m_sTaskCode
        End Get
        Set(ByVal Value As String)
            m_sTaskCode = Value
        End Set
    End Property

    Private bIsChildNavigatorON As Boolean = False
    Public Property IsChildNavigatorON() As Boolean
        Get
            Return bIsChildNavigatorON
        End Get
        Set(ByVal value As Boolean)
            bIsChildNavigatorON = value
        End Set
    End Property

    ' RAW 14/02/2003 : ISS2153 : end

    ' ************* { Public Properties End } **************************

    ' ************* { Public Procedures Begin } ************************

    ' ***************************************************************** '
    ' Name: Initialise
    '
    ' Description: Standard initialise function.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            ' RAW 14/02/2003 : ISS2153 : moved initialisation of m_oNavigator to StartProcess function


            ' Instance of object manager
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Initialise the object
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=m_sCallingAppName)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise object manager.", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetProcessID
    '
    ' Description: Gets the process_id from the current m_sProcessCode$
    '
    '
    ' ***************************************************************** '
    Private Function GetProcessID() As Integer

        Dim result As Integer = 0
        Dim oLookup As bPMLookup.Business



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get an instance of bPMLookup.Business
        Dim temp_oLookup As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oLookup, "bPMLookup.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        oLookup = temp_oLookup
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bPMLookup.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProcessID", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Return result
        End If

        ' Set the product family - use the architecture tables
        oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusArchitecture

        ' Perform the lookup
        m_lReturn = oLookup.GetEffectiveIDFromCode(v_sTableName:="PMNav_Process", v_sCode:=m_sProcessCode, v_dtEffectiveDate:=DateTime.Now, r_lID:=m_lProcessID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Lookup failed - ProcessID = " & m_lProcessID & _
                               ", Process Code = " & m_sProcessCode, vApp:=ACApp, vClass:=ACClass, vMethod:="GetProcessID", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            ' Dont exit just yet, terminate the object
        End If

        ' Terminate the object
        oLookup.Dispose()

        ' Remove the instance
        oLookup = Nothing

        Return result

    End Function

    ' RAW 14/02/2003 : ISS2153 : added
    ' ***************************************************************** '
    ' Name: GetTaskDetails
    '
    ' Description: Gets the Task details from the current m_sTaskCode
    '
    '
    ' ***************************************************************** '
    Private Function GetTaskDetails() As Integer
        Dim result As Integer = 0
        Dim oLookup As bPMLookup.Business
        Dim oTask As bPMTask.Business
        Dim lTaskID As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' First , get the Task ID
        ' ==========================

        ' Get an instance of bPMLookup.Business
        Dim temp_oLookup As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oLookup, "bPMLookup.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        oLookup = temp_oLookup
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bPMLookup.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTaskDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Return result
        End If

        ' Set the product family - use the architecture tables
        oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusArchitecture

        ' Perform the lookup
        m_lReturn = oLookup.GetEffectiveIDFromCode(v_sTableName:="PMWrk_Task", v_sCode:=m_sTaskCode, v_dtEffectiveDate:=DateTime.Now, r_lID:=lTaskID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Lookup failed - TaskID = " & lTaskID & _
                               ", Task Code = " & m_sTaskCode, vApp:=ACApp, vClass:=ACClass, vMethod:="GetTaskDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            oLookup.Dispose()
            oLookup = Nothing
            Return result
        End If

        ' Terminate the object
        oLookup.Dispose()

        ' Remove the instance
        oLookup = Nothing



        ' Now get the task details using the Task ID
        ' ==========================================

        ' Get an instance of bPMLookup.Business
        Dim temp_oTask As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oTask, "bPMTask.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        oTask = temp_oTask
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get instance of bPMTask.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTaskDetails")
            Return result
        End If

        ' Get the Task Details from the DB

        m_lReturn = oTask.GetDetails(lTaskID)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from database for task", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTaskDetails")

            oTask.Dispose()
            oTask = Nothing
            Return result
        End If

        ' Return the Task Details

        m_lReturn = oTask.GetNext(vNavXMLFile:=m_sNavXMLFile)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the task object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTaskDetails")

            oTask.Dispose()
            oTask = Nothing
            Return result
        End If

        ' Terminate the object

        oTask.Dispose()
        oTask = Nothing


        Return result

    End Function
    ' RAW 14/02/2003 : ISS2153 : end

    ' ***************************************************************** '
    ' Name: SetKeys
    '
    ' Description: Sets the navigator keys. Call before .Start
    '
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            ' RAW 14/02/2003 : ISS2153 : added
            'SetKeys = m_oNavigator.SetKeys(vKeyArray:=vKeyArray)
            result = gPMConstants.PMEReturnCode.PMTrue

            ' We haven't yet created the Navigator object so lets store this for later


            m_vKeyArray = vKeyArray

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: Start
    '
    ' Description: Standard entry point for programs.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            ' RAW 14/02/2003 : ISS2153 : transferred existing code into new function StartProcess
            '                            added StartTask


            If (m_sTaskCode <> "") Or (m_sNavXMLFile <> "") Then

                m_lReturn = CType(StartTask(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else

                m_lReturn = CType(StartProcess(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' RAW 14/02/2003 : ISS2153 : added
    ' ***************************************************************** '
    ' Name: StartProcess
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function StartProcess() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Make sure we have a process_id
        ' ==============================

        If m_lProcessID = 0 Then

            ' Check if we have a process code we can use to get the ID
            If m_sProcessCode = "" Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="A valid ProcessCode or ProcessID is required.", vApp:=ACApp, vClass:=ACClass, vMethod:="StartProcess", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            Else

                ' Go fetch the process_id
                m_lReturn = CType(GetProcessID(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

        End If


        ' Create the Navigator and launch the process
        ' ===========================================

        ' Create the instance of Navigator
        ' Note - this object will be terminated and destroyed from Terminate as normal
        If m_oNavigator Is Nothing Then
            m_oNavigator = New iPMNavigator.NavigateControl()

            m_lReturn = m_oNavigator.Initialise()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise Navigator object.", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                m_oNavigator.Dispose()
                m_oNavigator = Nothing
                Return result
            End If

            ' Set the calling app name
            m_oNavigator.CallingAppName = m_sCallingAppName
        End If

        ' Set the keys
        m_lReturn = m_oNavigator.SetKeys(vKeyArray:=m_vKeyArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        ' Authority Level
        ' If this isn't set, then its initialised to 0, which is
        ' the authority level for a "User"
        m_oNavigator.PMAuthorityLevel = m_lPMAuthorityLevel

        ' Process ID
        m_oNavigator.ProcessID = m_lProcessID

        ' Not closed yet
        m_bNavClosed = False

        ' Start up Navigator
        m_lReturn = m_oNavigator.Start()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: StartTask
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function StartTask() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Make sure we have a navigator XML file name
        '============================================

        If m_sNavXMLFile = "" Then

            ' Check if we have a task code we can use to get the details
            If m_sTaskCode = "" Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="A valid TaskCode or XML file name is required.", vApp:=ACApp, vClass:=ACClass, vMethod:="StartTask", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            Else

                ' Go fetch the task details
                m_lReturn = CType(GetTaskDetails(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

        End If


        ' Create the Navigator and launch the task
        ' ========================================

        ' Create the instance of Navigator
        ' Note - this object will be terminated and destroyed from Terminate as normal
        If m_oNavigatorXM Is Nothing Then

            m_oNavigatorXM = New iPMNavigatorXM.Interface_Renamed()

            'Developer Guide No 9
            m_lReturn = m_oNavigatorXM.Initialise()

            m_oNavigatorXM.IsChildNavigatorON = bIsChildNavigatorON

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise NavigatorXM object.", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                m_oNavigatorXM.Dispose()
                m_oNavigatorXM = Nothing
                Return result
            End If

            ' Set the calling app name
            m_oNavigatorXM.CallingAppName = m_sCallingAppName
        End If


        m_lReturn = m_oNavigatorXM.SetKeys(vKeyArray:=m_vKeyArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        ' Authority Level
        ' If this isn't set, then its initialised to 0, which is
        ' the authority level for a "User"
        m_oNavigatorXM.NavigatorV3_PMAuthorityLevel = m_lPMAuthorityLevel

        ' Navigator File name
        m_oNavigatorXM.XMLFileName = m_sNavXMLFile

        ' Not closed yet
        m_bNavClosed = False

        ' Start up Navigator
        m_lReturn = m_oNavigatorXM.Start()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function
    ' RAW 14/02/2003 : ISS2153 : added

    ' ***************************************************************** '
    ' Name: Terminate
    '
    ' Description: Standard terminate function.
    '
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
                If m_oNavigator IsNot Nothing Then
                    m_oNavigator.Dispose()
                    m_oNavigator = Nothing
                End If
                If m_oNavigatorXM IsNot Nothing Then
                    m_oNavigatorXM.Dispose()
                    m_oNavigatorXM = Nothing
                End If
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If
            End If
        End If
		Me.disposedValue = True
    End Sub


    ' ************* { Public Procedures End } ************************

    ' ************* { Navigator Events } *****************************

    Private Sub m_oNavigator_NavigatorClose() Handles m_oNavigator.NavigatorClose

        ' Raise the event
        RaiseEvent NavigatorClose()

    End Sub

    Private Sub m_oNavigator_SetProcessStatus(ByVal v_bProcessComplete As Boolean) Handles m_oNavigator.SetProcessStatus

        ' Raise the event
        ' CF - Can't seem to use named parameters?
        RaiseEvent SetProcessStatus(v_bProcessComplete)

    End Sub

    ' RAW 14/02/2003 : ISS2153 : added
    Private Sub m_oNavigatorXM_NavigatorClose() Handles m_oNavigatorXM.NavigatorClose

        ' Raise the event
        RaiseEvent NavigatorClose()

    End Sub

    ' RAW 14/02/2003 : ISS2153 : added
    Private Sub m_oNavigatorXM_SetProcessStatus(ByVal v_bProcessComplete As Boolean) Handles m_oNavigatorXM.SetProcessStatus

        ' Raise the event
        RaiseEvent SetProcessStatus(v_bProcessComplete)

    End Sub


    ' ***************************************************************** '
    ' Name: GetKeys
    '
    ' Description: Sets the navigator keys. Call before .Terminate
    '
    ' ***************************************************************** '
    ''''''
    Public Property arrayvalue1() As Object
        Get
            Return m_vKeyArray
        End Get
        Set(ByVal value As Object)
            m_vKeyArray = value
        End Set
        ''''''
    End Property
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not (m_oNavigatorXM Is Nothing) Then

                m_lReturn = m_oNavigatorXM.GetKeys(vKeyArray:=m_vKeyArray)



                vKeyArray = m_vKeyArray


            End If
            Return result


        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ************* { Navigator Events End } *************************
End Class

