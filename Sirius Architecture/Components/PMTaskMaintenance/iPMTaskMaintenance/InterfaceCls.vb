Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'Developer Guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    ' Edit History:
    ' DAK200999 - Allow selection of task icon.
    ' DAK061099 - New fields for licencing
    ' DAK231199 - Check PMProductLookup table to see if we can continue
    ' DAK211299 - Add Task Category
    ' DAK221299 - More changes for Task Catgeory
    ' ***************************************************************** '
    'Developer Guide no. 50
    Dim frmInterface As frmInterface
    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"


    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' {* USER DEFINED CODE (Begin) *}
    'DAK231199
    ' PrivilegeLevel
    Private m_iPrivilegeLevel As gPMConstants.PMELookupEditPrivlegeLevel

    ' {* USER DEFINED CODE (End) *}

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property

    Public ReadOnly Property Task() As Integer
        Get

            ' Standard Property.

            ' Return the task.
            Return m_iTask

        End Get
    End Property

    Public ReadOnly Property Navigate() As Integer
        Get

            ' Standard Property.

            ' Return the navigate flag.
            Return m_lNavigate

        End Get
    End Property

    Public ReadOnly Property ProcessMode() As Integer
        Get

            ' Standard Property.

            ' Return the process mode.
            Return m_lProcessMode

        End Get
    End Property

    Public ReadOnly Property TransactionType() As String
        Get

            ' Standard Property.

            ' Return the type of business.
            Return m_sTransactionType

        End Get
    End Property

    Public ReadOnly Property EffectiveDate() As Date
        Get

            ' Standard Property.

            ' Return the effective date.
            Return m_dtEffectiveDate

        End Get
    End Property

    ' {* USER DEFINED CODE (Begin) *}
    'DAK231199
    Public Property PrivilegeLevel() As Integer
        Get
            Return m_iPrivilegeLevel
        End Get
        Set(ByVal Value As Integer)
            m_iPrivilegeLevel = Value
        End Set
    End Property

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
    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Dim sMessage, sTitle As String

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

                ' Set the object manager to nothing.
                g_oObjectManager = Nothing

                ' Log Error.
                'LogMessagePopup _
                'iType:=PMLogError, _
                'sMsg:="Failed to initialise the object manager", _
                'vApp:=ACApp, _
                'vClass:=ACClass, _
                'vMethod:="Initialise"

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
            '    m_sTransactionType$ = PMTypeOfBusinessGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_g_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oBusiness, "bPMTask.Business", vInstanceManager:="ClientManager")
            g_oBusiness = temp_g_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.
                sTitle = ACBusinessFailTitleText
                '        sTitle$ = iPMFunc.GetResData( _
                'iLangID:=g_iLanguageID%, _
                'lID:=ACBusinessFailTitle, _
                'iDataType:=PMResString)
                sMessage = ACBusinessFailText
                '        sMessage$ = iPMFunc.GetResData( _
                'iLangID:=g_iLanguageID%, _
                'lID:=ACBusinessFail, _
                'iDataType:=PMResString)

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Return result
            End If

            'DAK231199
            'Check if we can continue
            m_lReturn = GetPrivileges()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

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
                If g_oBusiness IsNot Nothing Then
                    g_oBusiness.Dispose()
                    g_oBusiness = Nothing
                End If
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    '
    ' Name: GetPrivileges
    '
    ' Description:
    '
    ' History: 23/11/1999 DAK - Created.
    '
    ' ***************************************************************** '
    Public Function GetPrivileges() As Integer
        Dim result As Integer = 0
        Dim oBusiness As bPMTask.Business
        Dim bIsAdministrator As Boolean
        Dim vSupervisedGroups As Object


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get the Business object
            Dim temp_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oBusiness, "bPMTask.Business", vInstanceManager:="ClientManager")
            oBusiness = temp_oBusiness
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse


                oBusiness.Dispose()
                oBusiness = Nothing

                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get bPMTask.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPrivileges")

                Return result
            End If


            m_lReturn = oBusiness.GetPrivilegeLevel(r_iPrivilegeLevel:=m_iPrivilegeLevel)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or m_iPrivilegeLevel = gPMConstants.PMELookupEditPrivlegeLevel.PMLookupNoEdit Then

                result = gPMConstants.PMEReturnCode.PMFalse


                oBusiness.Dispose()
                oBusiness = Nothing

                MessageBox.Show("You do not have permission to access " & _
                                "PM Task Maintenance." & _
                                Strings.Chr(10).ToString() & Strings.Chr(13).ToString() & Strings.Chr(10).ToString() & Strings.Chr(13).ToString() & _
                                "Please contact your System Administrator.", Application.ProductName)

                Return result
            End If

            'DAK011299
            If m_iPrivilegeLevel = gPMConstants.PMELookupEditPrivlegeLevel.PMLookupFullPrivileges Or m_iPrivilegeLevel = gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAmendCaptions Or m_iPrivilegeLevel = gPMConstants.PMELookupEditPrivlegeLevel.PMLookupViewOnly Then


                oBusiness.Dispose()
                oBusiness = Nothing
                Return result
            End If


            m_lReturn = oBusiness.GetUserAuthority(r_bIsAdministrator:=bIsAdministrator, r_vSupervisedGroups:=vSupervisedGroups)


            oBusiness.Dispose()
            oBusiness = Nothing
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            'DAK011299
            If Not bIsAdministrator Then


                Select Case m_iPrivilegeLevel
                    Case gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAdminCaptionsUserView, gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAdminFullUserView

                        m_iPrivilegeLevel = gPMConstants.PMELookupEditPrivlegeLevel.PMLookupViewOnly

                    Case gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAdminFullUserCaptions

                        m_iPrivilegeLevel = gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAmendCaptions

                    Case Else

                        result = gPMConstants.PMEReturnCode.PMFalse


                        oBusiness.Dispose()
                        oBusiness = Nothing

                        MessageBox.Show("You do not have permission to access " & _
                                        "PM Task Maintenance." & _
                                        Strings.Chr(10).ToString() & Strings.Chr(13).ToString() & Strings.Chr(10).ToString() & Strings.Chr(13).ToString() & _
                                        "Please contact your System Administrator.", Application.ProductName)

                        Return result

                End Select

            Else


                Select Case m_iPrivilegeLevel
                    Case gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAdminViewUserNone

                        m_iPrivilegeLevel = gPMConstants.PMELookupEditPrivlegeLevel.PMLookupViewOnly

                    Case gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAdminCaptionsUserView, gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAdminCaptionsUserNone

                        m_iPrivilegeLevel = gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAmendCaptions

                    Case Else

                        m_iPrivilegeLevel = gPMConstants.PMELookupEditPrivlegeLevel.PMLookupFullPrivileges

                End Select

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPrivileges Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPrivileges", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

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
                    'Case PMKeyNameInsReference
                    '    m_sClientKey$ = CStr(vKeyArray(PMKeyValue, lRow&))

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

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}

            ' Initialise the key array with the number of
            ' keys needed to be returned.
            ' Note: Remember arrays are zero based.
            ReDim vKeyArray(1, 0)

            ' Assign the key array with the parameter members.
            ' {* USER DEFINED CODE (End) *}

            Return result

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
    Public Function GetSummary(ByRef vSummaryArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}

            ' Initialise the summary array with the number of
            ' items needed to be returned.
            ' Note: Remember arrays are zero based.
            ReDim vSummaryArray(1, 0)

            ' Assign the key array with the parameter members.

            vSummaryArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = "Dummy Key"

            vSummaryArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = "Dummy Value"

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

            ' Set the process modes for the business object.
            '    If (g_oBusiness Is Nothing = False) Then
            '        m_lReturn& = g_oBusiness.SetProcessModes( _
            ''            vTask:=vTask, _
            ''            vNavigate:=vNavigate, _
            ''            vProcessMode:=vProcessMode, _
            ''            vTransactionType:=vTransactionType, _
            ''            vEffectiveDate:=vEffectiveDate)
            '
            '        ' Check for errors.
            '        If (m_lReturn& <> PMTrue) Then
            '            ' Failed to process the interface.
            '
            '            ' Log Error Message
            '            LogMessage _
            ''                iType:=PMLogOnError, _
            ''                sMsg:="Failed to set the process modes for the business object", _
            ''                vApp:=ACApp, _
            ''                vClass:=ACClass, _
            ''                vMethod:="SetProcessModes"
            '
            '            SetProcessModes = PMFalse
            '        End If
            '    End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

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
            m_lReturn = ProcessInterface()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '********************************************************************
    '
    ' Function Name: NewTask()
    '
    ' Description: Adds new task to the database
    ' DAK200999 - Allow selection of task icon.
    ' DAK061099 - New fields for licencing
    '********************************************************************

    Public Function NewTask() As Integer

        Dim result As Integer = 0
        Dim lStatus As Integer
        Dim lRow As Integer

        Select Case Information.Err().Number
            Case Is < 0
                Conversion.ErrorToString(5)
            Case 1
                GoTo err_NewTask
        End Select

        result = gPMConstants.PMEReturnCode.PMTrue

        Dim oTaskForm As New frmTask

        With oTaskForm

            .TaskCode = ""
            .EffectiveDate = DateTime.Now
            'DAK211299
            .TaskCategoryID = 1

            .ShowForm(USRAddTask)

            lStatus = .Status

            If lStatus = gPMConstants.PMEReturnCode.PMOK Then

                lRow = frmInterface.lvwTasks.Items.Count + 1

                'DAK200999
                'DAK061099
                'DAK211299

                m_lReturn = g_oBusiness.EditAdd(lRow:=lRow, vTaskId:=.TaskID, vCaptionId:=.CaptionID, vCode:=.TaskCode, vDescription:=.Description, vIsDeleted:=.IsDeleted, vEffectiveDate:=.EffectiveDate, vIsSystemTask:=.IsSystemTask, vTypeOfTask:=.TypeOfTask, vPMNavProcessId:=.PMNavProcessId, vComponentObjectName:=.ComponentObjectName, vComponentClassName:=.ComponentClassName, vAutoDeleteAfterNumDays:=.AutoDeleteAfterNumDays, vDisplayIcon:=.DisplayIcon, vIsViewOnlyTask:=.IsViewOnlyTask, vLinkedObjectName:=.LinkedObjectName, vLinkedClassName:=.LinkedClassName, vLinkedCaption:=.LinkedCaption, vIsAvailableTask:=.IsAvailableTask, vTaskCategoryID:=.TaskCategoryID)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If


                m_lReturn = g_oBusiness.Update()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

            End If

        End With

        oTaskForm.Close()
        oTaskForm = Nothing

        Return result

err_NewTask:

        'Error Section

        result = gPMConstants.PMEReturnCode.PMError

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Task", vApp:=ACApp, vClass:=ACClass, vMethod:="NewTask", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

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


        ' Load the interface into memory.
        m_lReturn = LoadInterface()

        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to load the interface.
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Display the interface.
        m_lReturn = ShowInterface(lDisplayState:=FormShowConstants.Modal)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to display the inteface.
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Destroy the interface from memory.
        m_lReturn = UnLoadInterface()

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
        'Developer Guide no. 50
        frmInterface = New frmInterface
        ' Assign the parameters to the interface properties.
        With frmInterface
            .CallingAppName = m_sCallingAppName
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate
            'DAK231199
            .PrivilegeLevel = PrivilegeLevel

            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}
        End With


        ' Load the instance of the interface into memory.
        Dim tempLoadForm As frmInterface = frmInterface

        ' Check if we have had an error so far.
        If frmInterface.ErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
            ' We have already encountered an error,
            ' so we MUST return the error.
            result = frmInterface.ErrorNumber
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
        With frmInterface
            m_lStatus = .Status

            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}
        End With

        ' Unload and destroy the instance of the interface
        ' from memory.
        frmInterface.Close()
        frmInterface = Nothing

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
        VB6.ShowForm(frmInterface, lDisplayState)

        If lDisplayState = FormShowConstants.Modal Then
            ' Check for any form errors.
            If frmInterface.ErrorNumber <> 0 Then
                result = frmInterface.ErrorNumber
            End If
        End If

        Return result

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
        ' Error Section.
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

End Class

