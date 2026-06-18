Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
'developer guide no. 129
Imports SharedFiles
Friend NotInheritable Class Map
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Map
    '
    ' Date: 01/09/1998
    '
    ' Description: Describes the attributes for a single Navigator Map.
    '
    ' Edit History:
    ' DAK221099 - Terminate CurrentKeys and ParentStep in Terminate
    '             function
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Map"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' DataBase Attributes
    Private m_lMapID As Integer
    Private m_sCode As String = ""
    Private m_sCaption As String = ""
    'Start (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.11.1)

    Private m_iResourceId As Integer
    'End (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.11.1)
    Private m_iIsStartMap As Integer
    Private m_oParentStep As Step_Renamed

    Private m_oSteps As iPMNavigator.Steps
    Private m_oCurrentStep As Step_Renamed

    ' Set Keys (Read Only)
    Private m_oSetKeys As iPMNavigator.Keys
    ' Current Key Values
    Private m_oCurrentKeys As iPMNavigator.Keys

    ' Return Code
    Private m_lReturn As Integer
    'Start (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.11.1)

    Public Property ResourceId() As Integer
        Get
            Return m_iResourceId
        End Get
        Set(ByVal Value As Integer)
            m_iResourceId = Value
        End Set
    End Property
    'End (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.11.1)

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public Property MapID() As Integer
        Get
            Return m_lMapID
        End Get
        Set(ByVal Value As Integer)
            m_lMapID = Value
        End Set
    End Property

    Public Property Code() As String
        Get
            Return m_sCode
        End Get
        Set(ByVal Value As String)
            m_sCode = Value
        End Set
    End Property

    Public Property IsStartMap() As Integer
        Get
            Return m_iIsStartMap
        End Get
        Set(ByVal Value As Integer)
            m_iIsStartMap = Value
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

    Public Property ParentStep() As Step_Renamed
        Get
            ' Return the ParentStep property
            Return m_oParentStep
        End Get
        Set(ByVal Value As Step_Renamed)

            ' Set the ParentStep Property
            m_oParentStep = Value

        End Set
    End Property

    Public Property Steps() As iPMNavigator.Steps
        Get
            ' Return the Steps property
            Return m_oSteps
        End Get
        Set(ByVal Value As iPMNavigator.Steps)

            ' Set the Steps Property
            m_oSteps = Value

        End Set
    End Property

    Public Property CurrentStep() As Step_Renamed
        Get
            ' Return the CurrentStep property
            Return m_oCurrentStep
        End Get
        Set(ByVal Value As Step_Renamed)

            ' Set the CurrentStep Property
            m_oCurrentStep = Value

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

    Public Property CurrentKeys() As iPMNavigator.Keys
        Get
            Return m_oCurrentKeys
        End Get
        Set(ByVal Value As iPMNavigator.Keys)

            m_oCurrentKeys = Value

        End Set
    End Property

    ' PUBLIC Property Procedures (End)


    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Start
    '
    ' Description: Starts a Map
    ' Note: For a Start Map the Key Values will be passed in an Array.
    '       For non start maps they will be passed as a collection.
    ' ***************************************************************** '
    Public Function Start(ByVal v_eIsUserDriven As MainModule.ACENavProcessType, Optional ByVal v_colKeyValuesFrom As iPMNavigator.Keys = Nothing, Optional ByVal v_vKeyValuesFromArray As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Current Step to be the First Step in the Map
            'developer guide no. 98
            CurrentStep = Steps.Item(1)

            ' Clear any existing Current Key Values
            CurrentKeys.Clear()

            ' Are there any set Keys
            If SetKeys.Count() < 1 Then
                ' No, so dont bother with the rest of this function
                Return result
            End If

            ' Key Value Changes
            '    ' Update the CurrentKeys with Initial Values
            '    m_lReturn = UpdateFromSetKeyInitValues( _
            ''        v_colSetKeys:=SetKeys, _
            ''        v_colKeysToBeUpdated:=CurrentKeys)
            '    If (m_lReturn <> PMTrue) Then
            '        Start = PMFalse
            '        Exit Function
            '    End If

            If IsStartMap = gPMConstants.PMEReturnCode.PMTrue Then

                ' Key Value Changes
                '        If (IsMissing(v_vKeyValuesFromArray) = True) Then
                '            LogMessage _
                ''                iType:=PMLogError, _
                ''                sMsg:="No Key Values Array Supplied", _
                ''                vApp:=ACApp, _
                ''                vClass:=ACClass, _
                ''                vMethod:="Start"
                '            Start = PMFalse
                '            Exit Function
                '        End If

                ' Update the Current Key Values with the values we have been supplied.

                m_lReturn = UpdateKeyValuesFromArray(v_colKeysToUpdate:=SetKeys, v_colKeysToBeUpdated:=CurrentKeys, v_vKeyValueFrom:=v_vKeyValuesFromArray)
            Else

                ' RFC160299 - Data Capture Process Type Added.
                ' If the Process is Navigable OR Data Capture then we do
                ' NOT need to setup Keys in the Sub Map as ONLY the Start Map Keys
                ' are used for a Navigable Process.
                If (v_eIsUserDriven = MainModule.ACENavProcessType.aceProcTypeNavigable) Or (v_eIsUserDriven = MainModule.ACENavProcessType.aceProcTypeDataCapture) Then
                    Return result
                End If

                ' Key Value Changes
                '        If (v_colKeyValuesFrom Is Nothing = True) Then
                '            LogMessage _
                ''                iType:=PMLogError, _
                ''                sMsg:="No Key Values Collection Supplied", _
                ''                vApp:=ACApp, _
                ''                vClass:=ACClass, _
                ''                vMethod:="Start"
                '            Start = PMFalse
                '            Exit Function
                '        End If

                ' Update the Current Key Values with the values we have been supplied.
                m_lReturn = UpdateKeyValuesFromCollection(v_colKeysToUpdate:=SetKeys, v_colKeysToBeUpdated:=CurrentKeys, v_colKeyValuesFrom:=v_colKeyValuesFrom)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StartFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: LoadExisting (Private)
    '
    ' Description: Selects the Process information for the requested
    '              Navigator process and loads it.
    '
    ' ***************************************************************** '
    Public Function LoadExisting(ByRef v_lMapID As Integer) As Integer

        Dim result As Integer = 0
        Dim vMapDetailsArray As Object
        Dim vMapSetKeyArray, vMapStepsArray, vStepsSetKeyArray, vStepsGetKeyArray As Object
        Dim eCol As NavigatorConstants.ACEMapDetsColPos

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get a Reference to the Navigator Business
            ' if we do not have one already.
            If g_oNavBusiness Is Nothing Then
                Dim temp_g_oNavBusiness As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_g_oNavBusiness, "bPMNavigator.Business", gPMConstants.PMGetViaClientManager)
                g_oNavBusiness = temp_g_oNavBusiness
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Get the details for the supplied Map ID.

            m_lReturn = g_oNavBusiness.GetMapDetails(v_lPMNavMapID:=v_lMapID, r_vMapDetailsArray:=vMapDetailsArray, r_vMapSetKeyArray:=vMapSetKeyArray, r_vMapStepsArray:=vMapStepsArray, r_vStepsSetKeyArray:=vStepsSetKeyArray, r_vStepsGetKeyArray:=vStepsGetKeyArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="No Map Details found for Map ID - " & v_lMapID, vApp:=ACApp, vClass:=ACClass, vMethod:="LoadExisting")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' We will have finished with the Business
            ' until we need to load another Map,
            ' so terminate it and release reference

            g_oNavBusiness.Dispose()
            g_oNavBusiness = Nothing

            ' Set the Map properties

            MapID = v_lMapID
            eCol = NavigatorConstants.ACEMapDetsColPos.aceMDMapCode

            Code = CStr(vMapDetailsArray(eCol, 0))
            eCol = NavigatorConstants.ACEMapDetsColPos.aceMDMapCaption

            Caption = CStr(vMapDetailsArray(eCol, 0))
            eCol = NavigatorConstants.ACEMapDetsColPos.aceMDIsStartMap

            IsStartMap = CInt(vMapDetailsArray(eCol, 0))

            ' Load any Map Set Keys

            m_lReturn = SetKeys.CreateNew(v_vKeyArray:=vMapSetKeyArray, v_bSetKeys:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create New Steps

            m_lReturn = Steps.CreateNew(v_vMapStepsArray:=vMapStepsArray, v_vStepsSetKeyArray:=vStepsSetKeyArray, v_vStepsGetKeyArray:=vStepsGetKeyArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Release memory
            vMapDetailsArray = Nothing
            vMapSetKeyArray = Nothing
            vMapStepsArray = Nothing
            vStepsSetKeyArray = Nothing
            vStepsGetKeyArray = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadExisting Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadExisting", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ReturnStepsAsArray
    '
    ' Description: Returns the Map Step Details in an Array.
    ' ***************************************************************** '
    Public Function ReturnStepsAsArray(ByRef r_vMapStepsArray As Object) As Integer

        Dim result As Integer = 0
        Dim lRow As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Steps.Count() < 1 Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="There are NO steps for Map - " & MapID, vApp:=ACApp, vClass:=ACClass, vMethod:="ReturnStepsAsArray")
                result = gPMConstants.PMEReturnCode.PMFalse

                r_vMapStepsArray = ""
                Return result
            End If

            ' Resize the array to the number of steps
            ReDim r_vMapStepsArray(gPMConstants.PMENavCaptionArrayColPosition.PMNavCaptionComponentType, Steps.Count() - 1)

            lRow = gPMConstants.PMEReturnCode.PMFalse

            ' Return the Details for each Non Hidden Step
            For Each oStep As Step_Renamed In Steps
                With oStep
                    ' Is this a Hidden Step
                    ' Note: Sub Map Steps cannot be hidden
                    If (.IsHidden = gPMConstants.PMEReturnCode.PMTrue) And (.IsSubMap = gPMConstants.PMEReturnCode.PMFalse) Then
                        ' Hidden Step, Do Nothing
                    Else
                        ' Add Step to Array

                        r_vMapStepsArray(gPMConstants.PMENavCaptionArrayColPosition.PMNavCaptionStepKey, lRow) = .StepKey

                        r_vMapStepsArray(gPMConstants.PMENavCaptionArrayColPosition.PMNavCaptionCaption, lRow) = .Caption

                        r_vMapStepsArray(gPMConstants.PMENavCaptionArrayColPosition.PMNavCaptionIsSubMap, lRow) = .IsSubMap

                        r_vMapStepsArray(gPMConstants.PMENavCaptionArrayColPosition.PMNavCaptionComponentType, lRow) = .ComponentType
                        lRow = CType(lRow + 1, gPMConstants.PMEReturnCode)
                    End If
                End With
            Next oStep

            ' If there are no steps, log an error
            If lRow < 1 Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="A Map MUST contain at least one NON hidden step. MapID = ", vApp:=ACApp, vClass:=ACClass, vMethod:="ReturnStepsAsArray")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If the array has empty rows at the end due to hidden steps
            ' resize it to the number of non hidden rows.
            If (Steps.Count()) <> lRow Then
                ReDim Preserve r_vMapStepsArray(gPMConstants.PMENavCaptionArrayColPosition.PMNavCaptionComponentType, lRow - 1)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReturnStepsAsArrayFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReturnStepsAsArray", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

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

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialisation Code.

            ' New Steps Collection
            Steps = New iPMNavigator.Steps()
            'developer guide no. 9
            m_lReturn = Steps.Initialise()
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

            ' New CurrentKeys Collection
            CurrentKeys = New iPMNavigator.Keys()
            'developer guide no. 9
            m_lReturn = CurrentKeys.Initialise()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Set Current Step to Nothing
            CurrentStep = Nothing

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
                If Steps IsNot Nothing Then
                    Steps.Clear()
                    Steps.Dispose()
                    Steps = Nothing
                End If
                If SetKeys IsNot Nothing Then
                    SetKeys.Dispose()
                    SetKeys = Nothing
                End If
                If CurrentKeys IsNot Nothing Then
                    CurrentKeys.Dispose()
                    CurrentKeys = Nothing
                End If
                CurrentStep = Nothing
                ParentStep = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)
    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        ' Class Initialise

        Try

            m_oSteps = New iPMNavigator.Steps()

        Catch excep As System.Exception



            ' Error.

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Protected Overrides Sub Finalize()
		Dispose(False)
    End Sub

End Class
