Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
'developer guide no. 129
Imports SharedFiles
Friend NotInheritable Class Process
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Process
    '
    ' Date: 03/04/1997
    '
    ' Description: Describes the attributes for a Navigator Process.
    '
    ' Edit History:
    ' RFC 11/12/1998 - Navigable Process Type Added.
    '                  Amended IsUserDriven to have three states.
    ' RFC160299 - Data Capture Process Type Added.
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Process"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)
    Private m_oMaps As iPMNavigator.Maps
    Private m_oStartMap As iPMNavigator.Map
    Private m_oCurrentMap As iPMNavigator.Map

    Private m_lProcessID As Integer
    Private m_lPMProductID As Integer
    Private m_sCode As New FixedLengthString(10)
    Private m_sCaption As String = ""
    Private m_vTransactionType As String = ""
    Private m_lProcessMode As Integer
    Private m_iIsLogged As Integer
    Private m_eIsUserDriven As MainModule.ACENavProcessType

    ' Set Keys (Read Only)
    Private m_oSetKeys As iPMNavigator.Keys
    ' Get Keys (Read Only)
    Private m_oGetKeys As iPMNavigator.Keys

    ' Return Code
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    ' PUBLIC Property Procedures (End)
    ' PRIVATE Property Procedures (Begin)
    Public Property Maps() As iPMNavigator.Maps
        Get
            Return m_oMaps
        End Get
        Set(ByVal Value As iPMNavigator.Maps)

            m_oMaps = Value

        End Set
    End Property


    Public Property StartMap() As iPMNavigator.Map
        Get
            Return m_oStartMap
        End Get
        Set(ByVal Value As iPMNavigator.Map)

            m_oStartMap = Value

        End Set
    End Property


    Public Property CurrentMap() As iPMNavigator.Map
        Get
            Return m_oCurrentMap
        End Get
        Set(ByVal Value As iPMNavigator.Map)

            m_oCurrentMap = Value

        End Set
    End Property



    Public Property ProcessID() As Integer
        Get
            Return m_lProcessID
        End Get
        Set(ByVal Value As Integer)
            m_lProcessID = Value
        End Set
    End Property


    Public Property PMProductID() As Integer
        Get
            Return m_lPMProductID
        End Get
        Set(ByVal Value As Integer)
            m_lPMProductID = Value
        End Set
    End Property


    Public Property Code() As String
        Get
            Return m_sCode.Value
        End Get
        Set(ByVal Value As String)
            m_sCode.Value = Value
        End Set
    End Property

    Public Property TransactionType() As String
        Get
            Return m_vTransactionType
        End Get
        Set(ByVal Value As String)

            If Convert.IsDBNull(Value) Or IsNothing(Value) Then
                m_vTransactionType = gPMConstants.PMTransactionTypeGeneric
            Else

                m_vTransactionType = CStr(Value)
            End If
        End Set
    End Property


    Public Property ProcessMode() As Integer
        Get
            Return m_lProcessMode
        End Get
        Set(ByVal Value As Integer)
            m_lProcessMode = Value
        End Set
    End Property


    Public Property Caption() As String
        Get
            Return m_sCaption
        End Get
        Set(ByVal Value As String)
            m_sCaption = Value
        End Set
    End Property


    Public Property IsLogged() As Integer
        Get
            Return m_iIsLogged
        End Get
        Set(ByVal Value As Integer)
            m_iIsLogged = Value
        End Set
    End Property


    Public Property IsUserDriven() As MainModule.ACENavProcessType
        Get
            Return m_eIsUserDriven
        End Get
        Set(ByVal Value As MainModule.ACENavProcessType)
            m_eIsUserDriven = Value
        End Set
    End Property



    Public Property SetKeys() As iPMNavigator.Keys
        Get
            Return m_oSetKeys
        End Get
        Set(ByVal Value As iPMNavigator.Keys)

            m_oSetKeys = Value

        End Set
    End Property


    Public Property GetKeys() As iPMNavigator.Keys
        Get
            Return m_oGetKeys
        End Get
        Set(ByVal Value As iPMNavigator.Keys)

            m_oGetKeys = Value

        End Set
    End Property
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

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

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialisation Code.

            ' New Maps Collection
            Maps = New iPMNavigator.Maps()
            'developer guide no. 9
            m_lReturn = Maps.Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' New SetKeys Collection
            SetKeys = New iPMNavigator.Keys()
            'developer guide no. 9
            m_lReturn = SetKeys.Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' New GetKeys Collection
            GetKeys = New iPMNavigator.Keys()
            'developer guide no. 9
            m_lReturn = GetKeys.Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Set Start Map = Nothing
            StartMap = Nothing

            Return result

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
            Me.disposedValue = True
            If disposing Then
                If Maps IsNot Nothing Then
                    Maps.Dispose()
                    Maps = Nothing
                End If
                If SetKeys IsNot Nothing Then
                    SetKeys.Dispose()
                    SetKeys = Nothing
                End If
                If GetKeys IsNot Nothing Then
                    GetKeys.Dispose()
                    GetKeys = Nothing
                End If
                StartMap = Nothing
                CurrentMap = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: StartProcess
    '
    ' Description: Starts a New Navigator Process
    '
    ' ***************************************************************** '
    Public Function StartProcess(ByRef v_lProcessID As Integer, ByRef v_vSetKeyArray As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Load the Process
            m_lReturn = CType(LoadExisting(v_lProcessID:=v_lProcessID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the Current Map to be the StartProcess Map
            CurrentMap = StartMap

            ' Start the Start Map
            m_lReturn = CType(StartMap.Start(v_eIsUserDriven:=IsUserDriven, v_vKeyValuesFromArray:=v_vSetKeyArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StartProcessFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="StartProcess", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'RFC160299
    ' ***************************************************************** '
    ' Name: RestartProcess
    '
    ' Description: Sets the Current Map to be the Start Map and sets
    '              the current strp to be the first step.
    '
    ' ***************************************************************** '
    Public Function RestartProcess() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Start Map Current Step to Step 1
            'developer guide no. 98
            StartMap.CurrentStep = StartMap.Steps.Item(1)

            ' Set the Current Map to the Start Map
            CurrentMap = StartMap

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RestartProcessFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="RestartProcess", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: StartSubMap
    '
    ' Description: Starts the Map specified.
    ' ***************************************************************** '
    Public Function StartSubMap() As Integer

        Dim result As Integer = 0
        Dim oMap As iPMNavigator.Map
        Dim oKeyValues As iPMNavigator.Keys
        Dim sKey As String = ""
        Dim lMapID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the SubMapID for the Current Step
            lMapID = CurrentMap.CurrentStep.SubMapID

            ' Get a Reference to the Key Values for the Current Map
            oKeyValues = CurrentMap.CurrentKeys

            ' Generate a Key for the Sub Map
            sKey = Maps.GenerateKey(v_lMapID:=CStr(lMapID))

            ' Get a Reference to the Sub Map
            oMap = Maps.Item(sKey)

            ' Set the Current Map to be the Sub Map
            CurrentMap = oMap

            ' Start the Sub Map, passing it the Key Values
            ' from the previous map.
            m_lReturn = CType(oMap.Start(v_eIsUserDriven:=IsUserDriven, v_colKeyValuesFrom:=oKeyValues), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Release any references
            oMap = Nothing
            oKeyValues = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StartSubMapFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="StartSubMap", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ExitSubMap
    '
    ' Description: Exits the Current Map.
    ' ***************************************************************** '
    Public Function ExitSubMap() As Integer
        Dim result As Integer = 0
        Dim oStep As Step_Renamed
        Dim oMap As iPMNavigator.Map
        Dim lItemNumber As Integer
        Dim sKey As String = ""
        Dim lMapID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If this is the Start Map, then End Process
            If CurrentMap.IsStartMap = gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMNavEndProcess
            End If

            ' Get the Parent Step
            oStep = CurrentMap.ParentStep

            ' If there is no Parent Step then this is the Start Map
            ' End Process
            If oStep Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMNavEndProcess
            End If

            ' Get a Reference to the Map that the parent Step is in
            lMapID = oStep.MapID

            sKey = Maps.GenerateKey(v_lMapID:=CStr(lMapID))

            ' Get a Reference to the Map
            oMap = Maps.Item(sKey)

            'AR20041004 - PN15002
            'When exiting a sub-map, populate the map keys of the parent step
            'with the key values held by the sub map
            m_lReturn = CType(UpdateKeyValuesFromCollection(v_colKeysToUpdate:=oStep.GetKeys, v_colKeysToBeUpdated:=oMap.CurrentKeys, v_colKeyValuesFrom:=CurrentMap.CurrentKeys), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oMap = Nothing
                oStep = Nothing
                Return m_lReturn
            End If

            ' Clear the Key Values for the Current Map
            CurrentMap.CurrentKeys.Clear()

            ' If the Sub Map Step OKAction is Exit Map,
            ' then Exit this Sub Map
            If oStep.OkAction = gPMConstants.PMNavActionExitMap Then
                oMap.CurrentStep = oStep
                CurrentMap = oMap
                oMap = Nothing
                oStep = Nothing
                Return ExitSubMap()

            Else

                ' Get the Item Number for the parent Step
                lItemNumber = oStep.ItemNumber
                ' Move on to the Next Step
                lItemNumber += 1
                ' Get a Reference to the Next Step
                'developer guide no. 98
                oStep = oMap.Steps.Item(lItemNumber)

                ' Set the Map Current Step
                oMap.CurrentStep = oStep
                ' Set the Process Current Map
                CurrentMap = oMap

                ' Release references
                oMap = Nothing
                oStep = Nothing

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ExitSubMapFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ExitSubMap", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RepeatSubMap
    '
    ' Description: Repeats the Sub
    ' ***************************************************************** '
    Public Function RepeatSubMap() As Integer
        Dim result As Integer = 0
        Dim oStep As Step_Renamed
        Dim oMap As iPMNavigator.Map
        Dim oKeyValues As iPMNavigator.Keys
        Dim lMapID As Integer
        Dim sKey As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get a Reference to the Parent Step
            oStep = CurrentMap.ParentStep

            ' Get a Reference to the Map that the Parent Step is in
            lMapID = oStep.MapID
            sKey = Maps.GenerateKey(v_lMapID:=CStr(lMapID))
            oMap = Maps.Item(sKey)

            ' Get the Key Values for the map
            oKeyValues = oMap.CurrentKeys

            ' Re-Start the Current Map, passing it the Key Values
            ' from the previous map.
            m_lReturn = CType(CurrentMap.Start(v_eIsUserDriven:=IsUserDriven, v_colKeyValuesFrom:=oKeyValues), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Release References
            oStep = Nothing
            oMap = Nothing
            oKeyValues = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RepeatSubMapFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="RepeatSubMap", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: JumpToStep
    '
    ' Description: Checks that the user casn jump to the Step selected
    '              and then sets the Current Map, Current Step
    '              properties etc.
    '
    ' ***************************************************************** '
    Public Function JumpToStep(ByVal v_sStepKey As String) As Integer

        Dim result As Integer = 0
        Dim oMap As iPMNavigator.Map
        Dim oStep As Step_Renamed
        Dim lSelectedStepIndex, lAllowedToStepIndex As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'RFC160299 - Data Capture Process Type Added.
            ' Is this a Navigable OR Data Capture Process
            If (IsUserDriven = MainModule.ACENavProcessType.aceProcTypeNavigable) Or (IsUserDriven = MainModule.ACENavProcessType.aceProcTypeDataCapture) Then

                ' Yes, So the user can Jimp to ANY step.

                ' Find the Step
                For Each oMap2 As iPMNavigator.Map In Maps
                    oMap = oMap2
                    oStep = oMap.Steps.Item(v_sStepKey)
                    If Not (oStep Is Nothing) Then
                        CurrentMap = oMap
                        CurrentMap.CurrentStep = oStep
                        oMap = Nothing
                        oStep = Nothing
                        Return result
                    End If
                Next oMap2

                ' Step Not Found
                oMap = Nothing
                oStep = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse

            Else

                ' No, so the User can only Jump To Steps in the Start Map.

                ' Get a Reference to the Step that the user Double Clicked
                oStep = StartMap.Steps.Item(v_sStepKey)

                ' If the Step is NOT in the Start Map then they cannot
                ' jump to the step.
                If oStep Is Nothing Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Get the Step Item Number
                lSelectedStepIndex = oStep.ItemNumber

                ' Get the Item number of the Step they can Jump to
                lAllowedToStepIndex = StartMap.CurrentStep.ItemNumber

                ' Is the Selected Step after the valid ones
                ' Note: For a User Driven Process the User can jump to ANY step
                ' within the Start Map
                If (lSelectedStepIndex > lAllowedToStepIndex) And (IsUserDriven = MainModule.ACENavProcessType.aceProcTypeNavDriven) Then
                    ' Yes, so just exit
                    oStep = Nothing
                    Return gPMConstants.PMEReturnCode.PMFalse
                Else
                    ' Set the Current Step to be the Selected Step
                    StartMap.CurrentStep = oStep
                    oStep = Nothing
                    ' Set the Current Map to be the Start Map
                    CurrentMap = StartMap
                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error Jumping to Step - " & v_sStepKey, vApp:=ACApp, vClass:=ACClass, vMethod:="JumpToStep", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: LoadExisting (Public)
    '
    ' Description: Selects the Process information for the requested
    '              Navigator process and loads it.
    '
    ' ***************************************************************** '
    Private Function LoadExisting(ByVal v_lProcessID As Integer) As Integer

        Dim result As Integer = 0
        Dim lStartMapID As Integer
        Dim eColPos As NavigatorConstants.ACEProcDetsColPos
        Dim vProcDetailsArray As Object
        Dim vProcSetKeyArray, vProcGetKeyArray As Object
        Dim lReturn As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' If we do not a valid Navigator Business, get one.
        If g_oNavBusiness Is Nothing Then
            Dim temp_g_oNavBusiness As Object
            lReturn = g_oObjectManager.GetInstance(temp_g_oNavBusiness, "bPMNavigator.Business", gPMConstants.PMGetViaClientManager)
            g_oNavBusiness = temp_g_oNavBusiness
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        ' Call the Navigator Business to get the Process Details

        lReturn = g_oNavBusiness.GetProcessDetails(v_lPMNavProcessID:=v_lProcessID, r_vProcessDetailsArray:=vProcDetailsArray, r_vSetKeyArray:=vProcSetKeyArray, r_vGetKeyArray:=vProcGetKeyArray)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="No Process Details found for Process ID - " & v_lProcessID, vApp:=ACApp, vClass:=ACClass, vMethod:="LoadExisting")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Set the Process Properties
        ProcessID = v_lProcessID

        eColPos = NavigatorConstants.ACEProcDetsColPos.acePDMProductID
        'developer guide no.84
        PMProductID = vProcDetailsArray(eColPos, 0)

        eColPos = NavigatorConstants.ACEProcDetsColPos.acePDProcessCode

        Code = CStr(vProcDetailsArray(eColPos, 0))

        eColPos = NavigatorConstants.ACEProcDetsColPos.acePDCaption

        Caption = CStr(vProcDetailsArray(eColPos, 0))


        eColPos = NavigatorConstants.ACEProcDetsColPos.acePDTransactionTypeCode

        TransactionType = CStr(vProcDetailsArray(eColPos, 0))

        eColPos = NavigatorConstants.ACEProcDetsColPos.acePDProcessMode

        ProcessMode = vProcDetailsArray(eColPos, 0)

        eColPos = NavigatorConstants.ACEProcDetsColPos.acePDIsLogged

        IsLogged = vProcDetailsArray(eColPos, 0)

        eColPos = NavigatorConstants.ACEProcDetsColPos.acePDIsUserDriven

        IsUserDriven = vProcDetailsArray(eColPos, 0)

        eColPos = NavigatorConstants.ACEProcDetsColPos.acePDStartMapID


        lStartMapID = vProcDetailsArray(eColPos, 0)

        ' Create the Set Keys Collection

        m_lReturn = CType(SetKeys.CreateNew(v_vKeyArray:=vProcSetKeyArray, v_bSetKeys:=True), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        ' Create the Get Keys Collection

        m_lReturn = CType(GetKeys.CreateNew(v_vKeyArray:=vProcGetKeyArray, v_bSetKeys:=False), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        ' Load the Start Map for this Process
        ' and set the Current Map to this map.
        StartMap = m_oMaps.Add(v_lMapID:=lStartMapID)

        ' If we do not have a Start Map Error!
        If StartMap Is Nothing Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Clear Up Memory
        vProcDetailsArray = Nothing
        vProcSetKeyArray = Nothing
        vProcGetKeyArray = Nothing

        ' We will have finished with the Business'
        ' until we need to load another Map,
        ' so terminate it and release reference
        If Not (g_oNavBusiness Is Nothing) Then

            g_oNavBusiness.Dispose()
            g_oNavBusiness = Nothing
        End If

        Return result

    End Function

    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        ' Class Initialise

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error.
        '
        ' Log Error Message
        'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
		Dispose(False)
    End Sub

End Class

