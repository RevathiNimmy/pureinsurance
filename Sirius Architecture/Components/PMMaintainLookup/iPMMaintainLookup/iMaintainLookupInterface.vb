Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
'Developer Guide No. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    Implements SSP.S4I.Interfaces.ILocalInterface
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: {TodaysDate}
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History:
    ' DAK031299 - Add SetKeyArray to Launch linked object
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sNavigatorTitle As String = ""
    Private m_sStepStatus As String = ""
    Private m_lPMAuthorityLevel As Integer

    'Private WithEvents m_frmInterface As frmInterface
    Private WithEvents m_frmInterface As frmInterface
    ' LinkedObject
    Private WithEvents m_oComponent As iPMWrkComponentStarter.StartControl

    ' {* USER DEFINED CODE (Begin) *}
    Private m_sTableName As String = ""
    Private m_iProductFamily As Integer

    ' DisableExport
    Private m_bDisableExport As Boolean

    ' DisablePrint
    Private m_bDisablePrint As Boolean

    ' {* USER DEFINED CODE (End) *}

    ' Stores the exit status of the interface.
    Private m_lStatus As Integer

    ' PrivilegeLevel
    Private m_iPrivilegeLevel As Integer
    ' LinkeCaption
    Private m_sLinkedCaption As String = ""
    ' LinkedObjectName
    Private m_sLinkedObjectName As String = ""
    ' LinkedClassName
    Private m_sLinkedClassName As String = ""

    Private m_sInterface_Component As String = ""

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_bSomethingChanged As Boolean = False
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public Property DisablePrint() As Boolean
        Get
            Return m_bDisablePrint
        End Get
        Set(ByVal Value As Boolean)
            m_bDisablePrint = Value
        End Set
    End Property

    Public Property DisableExport() As Boolean
        Get
            Return m_bDisableExport
        End Get
        Set(ByVal Value As Boolean)
            m_bDisableExport = Value
        End Set
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusArchitecture
        End Get
    End Property

    Public Property ProductFamily() As Integer
        Get
            Return m_iProductFamily
        End Get
        Set(ByVal Value As Integer)
            m_iProductFamily = Value
        End Set
    End Property

    Public Property TableName() As String
        Get
            Return m_sTableName
        End Get
        Set(ByVal Value As String)
            m_sTableName = Value
        End Set
    End Property

    Public Property PMAuthorityLevel() As Integer
        Get
            Return m_lPMAuthorityLevel
        End Get
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = Value
        End Set
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property

    Public Property PrivilegeLevel() As Integer
        Get
            Return m_iPrivilegeLevel
        End Get
        Set(ByVal Value As Integer)
            m_iPrivilegeLevel = Value
        End Set
    End Property

    Public Property LinkedCaption() As String
        Get
            Return m_sLinkedCaption.Trim()
        End Get
        Set(ByVal Value As String)
            m_sLinkedCaption = Value.Trim()
        End Set
    End Property

    Public Property LinkedObjectName() As String
        Get
            Return m_sLinkedObjectName.Trim()
        End Get
        Set(ByVal Value As String)
            m_sLinkedObjectName = Value.Trim()
        End Set
    End Property

    Public Property LinkedClassName() As String
        Get
            Return m_sLinkedClassName.Trim()
        End Get
        Set(ByVal Value As String)
            m_sLinkedClassName = Value.Trim()
        End Set
    End Property

    Public Property Component() As Object
        Get
            Return m_oComponent
        End Get
        Set(ByVal Value As Object)
            If Microsoft.VisualBasic.Information.IsReference(Value) And Not (TypeOf Value Is String) Then
                m_oComponent = Value
            Else
                m_oComponent = Value
            End If
        End Set
    End Property


    Public Property Interface_Component() As String
        Get
            Return m_sInterface_Component
        End Get
        Set(ByVal Value As String)
            m_sInterface_Component = Value
        End Set
    End Property


    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}
    ' PUBLIC Property Procedures (End)

    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer Implements SSP.S4I.Interfaces.ILocalInterface.Initialise

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Failed to call the initialise method.
                result = gPMConstants.PMEReturnCode.PMFalse

                If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                    Return result
                End If

                ' Set the object manager to nothing.
                g_oObjectManager = Nothing

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return result
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
            End With

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            'DC 27/07/00 do not set to date and time just date only
            m_dtEffectiveDate = DateTime.Now 'Format$(Date$, "dd/mm/yyyy")

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

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
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: SetKeys (Standard Method)
    '
    ' Description: Stores all of the parameter members with the key
    '              array.
    '
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check we have a vaild array.
            If Not Information.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Step through the key array.
            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)
                ' Assign the parameter member with the
                ' correct key array item.

                ' {* USER DEFINED CODE (Begin) *}


                Select Case CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                    '            Case PMKeyNameID
                    '                m_iNameID% = CInt(vKeyArray(PMKeyValue, lRow&))
                End Select

                ' {* USER DEFINED CODE (End) *}
            Next lRow

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetKeys (Standard Method)
    '
    ' Description: Stores all of the key array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try


            ' {* USER DEFINED CODE (Begin) *}

            ' Initialise the key array with the number of
            ' keys needed to be returned.
            ' Note: Remember arrays are zero based.
            'ReDim vKeyArray(1, )

            ' Assign the key array with the parameter members.
            '    vKeyArray(PMKeyName, 0) = PMKeyNameID
            '    vKeyArray(PMKeyValue, 0) = m_iNameID%

            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetSummary (Standard Method)
    '
    ' Description: Stores all of the summary array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function GetSummary(ByRef vSummaryArray As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}

            ' Initialise the summary array with the number of
            ' items needed to be returned.
            ' Note: Remember arrays are zero based.
            Dim vKeyArray(1, 0) As Object

            ' Assign the key array with the parameter members.
            '    vSummaryArray(PMKeyName, 0) = PMKeyNameNavigatorTitle1
            '    vSummaryArray(PMKeyValue, 0) = m_sNavigatorTitle$

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.


            If Not Information.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Information.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Information.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Information.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Start (Standard Method)
    '
    ' Description: Entry point for the object to start its processing.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Starts the interface processing.
            m_lReturn = CType(ProcessInterface(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = m_lReturn
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: ProcessInterface (Standard Method)
    '
    ' Description: Calls the appropriate methods to process the
    '              interface.
    '
    ' ***************************************************************** '
    Private Function ProcessInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Busy mouse pointer
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        ' Load the interface into memory.
        m_lReturn = CType(LoadInterface(), gPMConstants.PMEReturnCode)

        ' Reset mouse pointer
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to load the interface.
            Return m_lReturn
        End If

        ' Display the interface.
        m_lReturn = CType(ShowInterface(lDisplayState:=FormShowConstants.Modal), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to display the inteface.
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Destroy the interface from memory.
        m_lReturn = CType(UnLoadInterface(), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to unload the interface.
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: LoadInterface (Standard Method)
    '
    ' Description: Loads the instance of the interface into memory and
    '              passes the parameters in.
    '
    ' ***************************************************************** '
    Private Function LoadInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        m_frmInterface = New frmInterface()


        ' Assign the parameters to the interface properties.
        With m_frmInterface

            ' {* USER DEFINED CODE (Begin) *}
            .ProductFamily = ProductFamily
            .TableName = TableName
            .DisableExport = m_bDisableExport
            .DisablePrint = m_bDisablePrint
            .PMAuthorityLevel = PMAuthorityLevel
            .PrivilegeLevel = PrivilegeLevel
            .LinkedCaption = LinkedCaption
            .LinkedObjectName = LinkedObjectName
            .LinkedClassName = LinkedClassName
            ' {* USER DEFINED CODE (End) *}

            .CallingAppName = m_sCallingAppName
            .Task = m_iTask
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate
            '.Interface_Component = Interface_Component

        End With

        ' Load the instance of the interface into memory.

        'Developer Guide No. 68

        ' Check if we have had an error so far.
        If (m_frmInterface.ErrorNumber = gPMConstants.PMEReturnCode.PMFalse) Or (m_frmInterface.ErrorNumber = gPMConstants.PMEReturnCode.PMCancel) Then
            ' We have already encountered an error,
            ' so we MUST return the error.
            result = m_frmInterface.ErrorNumber
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UnLoadInterface (Standard Method)
    '
    ' Description: Unloads the instance of the interface from memory.
    '
    ' ***************************************************************** '
    Private Function UnLoadInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the property members from the interface parameters.
        With m_frmInterface
            m_lStatus = .Status
            m_sStepStatus = .StepStatus

            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}
        End With

        ' Unload and destroy the instance of the interface
        ' from memory.
        m_frmInterface.Close()
        m_frmInterface = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: ShowInterface (Standard Method)
    '
    ' Description: Displays the instance of the interface using the
    '              display state.
    '
    ' ***************************************************************** '
    Private Function ShowInterface(ByRef lDisplayState As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Display the interface.
        VB6.ShowForm(m_frmInterface, lDisplayState)

        If lDisplayState = FormShowConstants.Modal Then
            ' Check for any form errors.
            If m_frmInterface.ErrorNumber <> 0 Then
                result = m_frmInterface.ErrorNumber
            End If
        End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: StartLinkedObject
    '
    ' Description:
    '
    ' History: 14/10/1999 DAK - Created.
    '
    ' ***************************************************************** '
    'DAK031299
    ' Developer Guide No. 17
    Private Function StartLinkedObject(ByVal v_sComponent As String, ByVal v_lPMAuthorityLevel As Integer, ByVal v_vSetKeyArray As Object, Optional ByRef o_bSomethingChanged As Boolean = False) As Integer

        Dim nReturn As Integer = gPMConstants.PMEReturnCode.PMFalse
        Dim oKeyArray As Object




        nReturn = gPMConstants.PMEReturnCode.PMTrue

        Component = New iPMWrkComponentStarter.StartControl()
        ' Developer Guide No. 9
        nReturn = Component.Initialise()

        If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initilaise Component Starter", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Component.CallingAppName = ACApp

        'DAK031299
        ' Developer Guide No. 17
        nReturn = m_oComponent.StartSynchronous(v_sComponent:=v_sComponent, v_lPMAuthorityLevel:=v_lPMAuthorityLevel, v_vSetKeyArray:=(v_vSetKeyArray), o_aGetKeyArray:=oKeyArray)

        If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Start Component :- " & v_sComponent)
            Return gPMConstants.PMEReturnCode.PMFalse
        ElseIf nReturn = gPMConstants.PMEReturnCode.PMTrue Then
            If IsArray(oKeyArray) Then
                If ToSafeString(oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0)) = "SomthingChanged" Then
                    o_bSomethingChanged = ToSafeBoolean(oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0))
                End If
            End If
        End If

        Return nReturn

    End Function

    'PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        ' Class Initialise Event.

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface entry class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    'UPGRADE_NOTE: (7001) The following declaration (m_frmInterfac_LaunchLinkedObject) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    Private Sub m_frmInterface_LaunchLinkedObject(ByVal v_sComponent As String, ByVal v_lPMAuthorityLevel As Integer)
    End Sub
    'UPGRADE_NOTE: (7001) The following declaration (m_frmInterface_LaunchLinkedObject) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    Public Sub m_frmInterface_LaunchLinkedObject(ByVal v_sComponent As String, ByVal v_lPMAuthorityLevel As Integer, ByVal v_vSetKeyArray(,) As Object, Optional ByRef o_bSomethingChanged As Boolean = False) Handles m_frmInterface.LaunchLinkedObject

        m_lReturn = CType(StartLinkedObject(v_sComponent, v_lPMAuthorityLevel, v_vSetKeyArray, o_bSomethingChanged), gPMConstants.PMEReturnCode)

    End Sub

    Private Sub m_oComponent_Finished(ByVal v_bComplete As Boolean) Handles m_oComponent.Finished

        m_oComponent.Dispose()
        m_oComponent = Nothing

    End Sub
End Class

