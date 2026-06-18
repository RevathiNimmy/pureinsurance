Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed

    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: {TodaysDate}
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History:
    '
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Interface"

    '********************************
    'Process Mode Variables
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As gPMConstants.PMENavigateButtonStatus
    Private m_lProcessMode As gPMConstants.PMEProcessMode
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    '********************************

    '********************************
    ' NavV3 property variables
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lPMAuthorityLevel As Integer
    '********************************

    ' Stores the return value for the a function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    Private m_frmInterface As frmInterface
    Private m_oFormFields As iPMFormControl.FormFields
    Private m_lTaskInstanceCnt As Integer

    '*****************************************************************
    ' retained for consistency with existing interface in           '*
    ' pmwrkmanager                                                  '*
    Private m_lPMWrkTaskInstanceCnt As Integer
    Private m_sCustomer As String = ""
    Private m_sDescription As String = ""
    Private m_dtTaskDueDate As Date
    Private m_iIsUrgent As Integer
    Private m_iTaskStatus As Integer
    Private m_lPMuserGroupID As Integer
    Private m_iUserId As Integer
    Private m_lPMNavProcessID As Integer
    Private m_sComponentObjectName As String = ""
    Private m_sComponentClassName As String = ""
    Private m_lDisplayIcon As Integer
    Private m_iIsViewOnlyTask As Integer
    Private m_sLinkedObjectName As String = ""
    Private m_sLinkedClassName As String = ""
    Private m_sLinkedCaption As String = ""
    Private m_sWorkflowInformation As String = ""
    ' end of retained for consistency with existing interface in    '*
    ' pmwrkmanager                                                  '*
    '*****************************************************************

    Private m_lPartyCnt As Integer
    Private m_lInsuranceFolderCnt As Integer
    Private m_lInsuranceFileCnt As Integer
    Private m_lClaimId As Integer
    Private m_lAccountKey As Integer

    '********************************
    ' NavV3 Interface Properties
    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            m_sCallingAppName = Value
        End Set
    End Property
    Public WriteOnly Property PMAuthoritylevel() As Integer
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = Value
        End Set
    End Property
    Public ReadOnly Property Status() As Integer
        Get
            Return m_lStatus
        End Get
    End Property
    '********************************
    '*****************************************************************
    ' retained for consistency with existing interface in           '*
    ' pmwrkmanager                                                  '* '*
    Public Property PMWrkTaskInstanceCnt() As Integer
        Get '*
            Return m_lPMWrkTaskInstanceCnt '*
        End Get
        Set(ByVal Value As Integer)
            m_lPMWrkTaskInstanceCnt = Value '*
        End Set
    End Property '*

    Public ReadOnly Property Customer() As String
        Get '*
            Return m_sCustomer '*
        End Get
    End Property '*
    Public ReadOnly Property description() As String
        Get '*
            Return m_sDescription '*
        End Get
    End Property '*
    Public ReadOnly Property DueDate() As Date
        Get '*
            Return m_dtTaskDueDate '*
        End Get
    End Property '*
    Public ReadOnly Property IsUrgent() As Integer
        Get '*
            Return m_iIsUrgent '*
        End Get
    End Property '*
    Public ReadOnly Property TaskStatus() As Integer
        Get '*
            Return m_iTaskStatus '*
        End Get
    End Property '*
    Public ReadOnly Property PMUserGroupId() As Integer
        Get '*
            Return m_lPMuserGroupID '*
        End Get
    End Property '*
    Public ReadOnly Property UserId() As Integer
        Get '*
            Return m_iUserId '*
        End Get
    End Property '*
    Public ReadOnly Property PMNavProcessId() As Integer
        Get '*
            Return m_lPMNavProcessID '*
        End Get
    End Property '*
    Public ReadOnly Property ComponentObjectName() As String
        Get '*
            Return m_sComponentObjectName '*
        End Get
    End Property '*
    Public ReadOnly Property ComponentClassName() As String
        Get '*
            Return m_sComponentClassName '*
        End Get
    End Property '*
    Public ReadOnly Property DisplayIcon() As Integer
        Get '*
            Return m_lDisplayIcon '*
        End Get
    End Property '*
    Public ReadOnly Property IsViewOnlyTask() As Integer
        Get '*
            Return m_iIsViewOnlyTask '*
        End Get
    End Property '*
    Public ReadOnly Property LinkedObjectName() As String
        Get '*
            Return m_sLinkedObjectName '*
        End Get
    End Property '*
    Public ReadOnly Property LinkedClassName() As String
        Get '*
            Return m_sLinkedClassName '*
        End Get
    End Property '*
    Public ReadOnly Property LinkedCaption() As String
        Get '*
            Return m_sLinkedCaption '*
        End Get
    End Property '*
    Public ReadOnly Property WorkflowInformation() As String
        Get '*
            Return m_sWorkflowInformation '*
        End Get
    End Property '*
    '*****************************************************************
    ' end of retained for consistency with existing interface in    '*
    ' pmwrkmanager                                                  '*
    '*****************************************************************


















































    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try

            ' {* USER DEFINED CODE (Begin) *}
            result = gPMConstants.PMEReturnCode.PMTrue

            m_oFormFields = New iPMFormControl.FormFields()

            m_lReturn = CType(m_oFormFields, SSP.S4I.Interfaces.ILocalInterface).Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the language ID
            m_oFormFields.LanguageID = g_oObjectManager.LanguageID

            ' ************************************************************
            ' Enter your code here to assign all of the controls to
            ' PMFormControl
            '
            ' Example:-
            '
            '        ' Pass control and required settings to FormControl
            '        m_lReturn = m_oFormFields.AddNewFormField( _
            ''                       ctlControl:=<Control Name>, _
            ''                       lFieldType:=<PM field type>, _
            ''                       lFormat:=<PM format string>, _
            ''                       lMandatory:=<PMMandatory or PMNonMandatory)
            '
            '        'Error checking
            '        If m_lReturn <> PMTrue Then
            '          SetFieldValidation = PMFalse
            '          Exit Function
            '        End If
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            ' Example
            '    m_lReturn& = m_oFormFields.AddNewFormField( _
            ''                    ctlControl:=txtPreRenSel, _
            ''                    lFormat:=PMFormatLong, _
            ''                    lFieldType:=PMLong, _
            ''                    lMandatory:=PMNonMandatory)


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function




    Public Function Initialise() As Integer




        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                    Return result
                End If

                ' Set the object manager to nothing.
                g_oObjectManager = Nothing

                ' Log Error.
                gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=New Exception(Information.Err().Description))
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
            m_dtEffectiveDate = DateTime.Now

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

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

            ' Check we have a valid array.
            If Not Information.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Step through the key array.
            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)


                Select Case vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)
                    Case PMNavKeyConst.PMKeyNameTaskInstanceCnt

                        m_lPMWrkTaskInstanceCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyVAlue, lRow))

                    Case PMNavKeyConst.PMKeyNamePartyCnt

                        m_lPartyCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyVAlue, lRow))

                    Case PMNavKeyConst.PMKeyNameInsuranceFolderCnt

                        m_lInsuranceFolderCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyVAlue, lRow))

                    Case PMNavKeyConst.PMKeyNameInsuranceFileCnt

                        m_lInsuranceFileCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyVAlue, lRow))

                    Case PMNavKeyConst.PMKeyNameClaimCnt

                        m_lClaimId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyVAlue, lRow))

                    Case PMNavKeyConst.ACTKeyNameAccountID

                        m_lAccountKey = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyVAlue, lRow))

                End Select

            Next lRow

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", excep:=excep)

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


            'returns no keys

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", excep:=excep)

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

            ' Initialise the summary array with the number of
            ' items needed to be returned.
            ' Note: Remember arrays are zero based.
            Dim vKeyArray(1, 0) As Object

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", excep:=excep)

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

                m_iTask = CType(CInt(vTask), gPMConstants.PMEComponentAction)
            End If


            If Not Information.IsNothing(vNavigate) Then

                m_lNavigate = CType(CInt(vNavigate), gPMConstants.PMENavigateButtonStatus)
            End If


            If Not Information.IsNothing(vProcessMode) Then

                m_lProcessMode = CType(CInt(vProcessMode), gPMConstants.PMEProcessMode)
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
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("vEffectiveDate", vEffectiveDate)
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", excep:=excep, oDicParms:=oDict)

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
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", excep:=excep)

            Return result

        End Try
    End Function

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
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Reset mouse pointer
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        ' Display the interface.
        m_lReturn = CType(ShowInterface(lDisplayState:=FormShowConstants.Modal), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Destroy the interface from memory.
        m_lReturn = CType(UnLoadInterface(), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
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

        With m_frmInterface
            .PMWrkTaskInstanceCnt = m_lPMWrkTaskInstanceCnt

            .Task = m_iTask
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate
            .PartyCnt = m_lPartyCnt

            .InsuranceFolderCnt = m_lInsuranceFolderCnt
            .InsuranceFIlecnt = m_lInsuranceFileCnt
            .ClaimId = m_lClaimId
            .AccountKey = m_lAccountKey

        End With

        ' Load the instance of the interface into memory.

        'Load(m_frmInterface)

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
        End With

        ' Unload and destroy the instance of the interface from memory.
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
            If m_frmInterface.Error_Renamed Then
                result = gPMConstants.PMEReturnCode.PMFalse
            Else

                With m_frmInterface

                    ' must be returned
                    ' so if called by pmworkmanager the online
                    ' details can be kept up-to-date
                    m_lPMWrkTaskInstanceCnt = .PMWrkTaskInstanceCnt
                    m_lPMuserGroupID = .PMUserGroupId
                    m_iUserId = .PMUserId
                    m_lPMNavProcessID = .PMNavProcessId
                    m_sComponentObjectName = .ComponentObjectName
                    m_sComponentClassName = .ComponentClassName
                    m_iTaskStatus = .TaskStatus
                    m_lDisplayIcon = .DisplayIcon
                    m_iIsViewOnlyTask = .IsViewOnlyTask
                    m_sLinkedObjectName = .LinkedObjectName
                    m_sLinkedClassName = .LinkedClassName
                    m_sLinkedCaption = .LinkedCaption
                    m_sWorkflowInformation = .WorkflowInformation

                    ' can be updated by event task component
                    m_sCustomer = .Customer
                    m_sDescription = .description
                    m_dtTaskDueDate = .DueDate
                    m_iIsUrgent = .IsUrgent

                End With

            End If
        End If

        Return result

    End Function

    Public Sub New()
        MyBase.New()

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Log Error Message
        'gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface entry class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class

