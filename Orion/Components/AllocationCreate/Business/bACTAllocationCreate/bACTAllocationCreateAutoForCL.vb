Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
'Devloper Guide No 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("AutoForCL_NET.AutoForCL")> _
Public NotInheritable Class AutoForCL
    Implements IDisposable
    Implements aPMNav.NavigatorV2
    ' ***************************************************************** '
    ' Class Name: AutoForCL
    ' Date: 03/12/1997
    '
    ' Description: Batch allocate methods
    '
    ' Edit History:
    ' ***************************************************************** '

    ' ************************************************
    ' Added to replace global variables 03/04/2007
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************



    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "AutoForCL"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_oAutomated As Automated


    ' Process Mode Properties
    ' Task
    Private m_iTask As Integer
    ' Navigate
    Private m_lNavigate As Integer
    ' Process Mode
    Private m_lProcessMode As Integer
    ' Type of Business
    Private m_sTransactionType As String = ""
    ' Effective
    Private m_dtEffectiveDate As Date

    ' Status members
    Private m_sProcessStatus As New FixedLengthString(2)
    Private m_sMapStatus As New FixedLengthString(2)
    Private m_sStepStatus As New FixedLengthString(2)
    Private m_lStatus As Integer

    Private m_vCashListID As Integer
    'Devloper Guide No 101
    'Private m_vCashListItemID As Integer
    Private m_vCashListItemID As Object
    'eck100500
    Private m_iCompanyID As Integer

    Private m_lAllocationID As Integer

    Private m_lBatchID As Integer
    Private m_lAccountID As Integer
    Private m_dtAccountingDate As Date
    Private m_lAllocationTransType As Integer

    ' PUBLIC Property Procedures (Begin)

    ' ***************************************************************** '
    ' The following property is set by Navigator                        '
    ' ***************************************************************** '

    ' The Calling application or component name.
    Public WriteOnly Property NavigatorV2_CallingAppName() As String Implements aPMNav.NavigatorV2.CallingAppName
        Set(ByVal Value As String)

            m_sCallingAppName = Value

        End Set
    End Property
    'End Property


    ' ***************************************************************** '
    ' The following properties are set by Navigator via the             '
    ' SetProcessModes method and tell the component what                '
    ' mode of operation it is in.                                       '
    ' ***************************************************************** '

    ' The Task that the form is to perform
    ' i.e. Add, Edit, View, Delete
    Public ReadOnly Property NavigatorV2_Task() As Integer Implements aPMNav.NavigatorV2.Task
        Get

            Return m_iTask

        End Get
    End Property

    ' The status of the Navigator button on the form.
    ' i.e. Not Required, Enabled, Disabled
    Public ReadOnly Property NavigatorV2_Navigate() As Integer Implements aPMNav.NavigatorV2.Navigate
        Get

            Return m_lNavigate

        End Get
    End Property

    ' The type of process that is being performed
    ' i.e. Generic, Enquiry, Quotation, Make Live
    Public ReadOnly Property NavigatorV2_ProcessMode() As Integer Implements aPMNav.NavigatorV2.ProcessMode
        Get

            Return m_lProcessMode

        End Get
    End Property

    ' The type of transaction that is being performed
    ' i.e. Quotation, New Business, Renewal, MTA
    Public ReadOnly Property NavigatorV2_TransactionType() As String Implements aPMNav.NavigatorV2.TransactionType
        Get

            Return m_sTransactionType

        End Get
    End Property

    ' The effective date that we are working to
    Public ReadOnly Property NavigatorV2_EffectiveDate() As Date Implements aPMNav.NavigatorV2.EffectiveDate
        Get

            Return m_dtEffectiveDate

        End Get
    End Property

    ' ***************************************************************** '
    ' The following properties need to be set by the                    '
    ' component so that Navigator can tell what happened.               '
    ' ***************************************************************** '

    ' The user status of the form on exit
    ' i.e. PMOK, PMCancel or PMNavigate.
    ' Business objects will normally just set this to PMOK,
    ' unless they want the ability to adjust the route through a process.
    Public ReadOnly Property NavigatorV2_Status() As Integer Implements aPMNav.NavigatorV2.Status
        Get

            Return m_lStatus

        End Get
    End Property

    ' The Completion Status of the Step
    ' i.e.Complete , Incomplete, Inactive
    Public ReadOnly Property NavigatorV2_StepStatus() As String Implements aPMNav.NavigatorV2.StepStatus
        Get

            Return m_sStepStatus.Value

        End Get
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFOrion
        End Get
    End Property

    ' PUBLIC Property Procedures (End)


    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' The following methods are called by Navigator BEFORE              '
    ' the component is told to Start its job.                           '
    ' ***************************************************************** '

    ' ***************************************************************** '
    ' Name: SetKeys (Navigator Standard Method)
    '
    ' Description: Accepts an Array in the format KeyName, KeyValue.
    '              The array will contain the key values required by the
    '              component to do its job.
    '
    ' ***************************************************************** '
    'Developer Guide No 33
    'Public Function NavigatorV2_SetKeys(ByRef vKeyArray(,) As Object) As Integer Implements aPMNav.NavigatorV2.SetKeys
    Public Function NavigatorV2_SetKeys(ByRef vKeyArray(,) As Object) As Integer Implements aPMNav.NavigatorV2.SetKeys

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_vCashListID = Nothing

            ' Check we have a vaild array.
            If Not Information.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Step through the key array.
            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)
                ' Assign the parameter member with the
                ' correct key array item.


                Select Case CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                    Case PMNavKeyConst.PMKeyNameBatchID

                        m_lBatchID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.ACTKeyNameAccountID

                        m_lAccountID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.ACTKeyNameAccountingDate

                        m_dtAccountingDate = CDate(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        'eck040601 replace Cint with cLng
                    Case PMNavKeyConst.ACTKeyNameAllocationTransType

                        m_lAllocationTransType = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.ACTKeyNameAllocationId

                        m_lAllocationID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.ACTKeyNameCashListId

                        m_vCashListID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.ACTKeyNameCashListItemId

                        m_vCashListItemID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        'eck100500
                    Case PMNavKeyConst.ACTKeyNameBranchID

                        m_iCompanyID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                End Select

            Next lRow

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetProcessModes (Navigator Standard Method)
    '
    ' Description: Sets the mode of operation for the Component.
    '              The properties are described individually above.
    '
    ' ***************************************************************** '
    Public Function NavigatorV2_SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer Implements aPMNav.NavigatorV2.SetProcessModes

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



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetStatus (Navigator Standard Method)
    '
    ' Description: Set the Process, Map and Step Completion Status.
    ' Note:        A Property Get is provided for the Step Status only
    '              as this is the only one which this component can
    '              alter directly.
    ' ***************************************************************** '
    Public Function NavigatorV2_SetStatus(ByRef sProcessStatus As String, ByRef sMapStatus As String, ByRef sStepStatus As String) As Integer Implements aPMNav.NavigatorV2.SetStatus

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the current Status settings.
            m_sProcessStatus.Value = sProcessStatus.Trim()
            m_sMapStatus.Value = sMapStatus.Trim()
            m_sStepStatus.Value = sStepStatus.Trim()

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Start (Navigator Standard Method)
    '
    ' Description: Tells the Component to Start its job.
    '
    ' ***************************************************************** '
    Public Function NavigatorV2_Start() As Integer Implements aPMNav.NavigatorV2.Start

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oAutomated
                m_lReturn = CType(.GetBatchDetails(lBatchID:=m_lBatchID), gPMConstants.PMEReturnCode)

                m_lReturn = CType(.CreateAllocationForCashlist(v_vCashListID:=m_vCashListID, r_lAllocationId:=m_lAllocationID, v_vCashListItemId:=m_vCashListItemID), gPMConstants.PMEReturnCode)
            End With

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Save the allocation_id

            ' Create a new instance of component services

            ' Update the temp storage value
            m_lReturn = CType(gPMComponentServices.UpdateUserProperty(v_sUserName:=m_sUsername, v_sPropertyName:=PMNavKeyConst.ACTKeyNameAllocationId, v_vPropertyValue:=m_lAllocationID), gPMConstants.PMEReturnCode)

            ' Remove the instance

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' The following methods are called by Navigator AFTER               '
    ' the component has done its job.                                   '
    ' ***************************************************************** '

    ' ***************************************************************** '
    ' Name: GetKeys (Navigator Standard Method)
    '
    ' Description: Accepts an Array in the format KeyName, KeyValue.
    '              The component populates the array with
    '              key values. i.e. If the component is
    '              FindParty it will return the PartyCnt of the Party
    '              selected by the user.
    '
    ' ***************************************************************** '
    'Developer Guide No 33
    'Public Function NavigatorV2_GetKeys(ByRef vKeyArray(,) As Object) As Integer Implements aPMNav.NavigatorV2.GetKeys
    Public Function NavigatorV2_GetKeys(ByRef vKeyArray(,) As Object) As Integer Implements aPMNav.NavigatorV2.GetKeys

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        ReDim vKeyArray(1, 0)

        '    ' Assign the key array with the parameter members.

        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameAllocationId

        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lAllocationID

        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetSummary (Navigator Standard Method)
    '
    ' Description: Accepts an Array in the format Summary Level, Summary
    '              Heading, Summary Value.
    '
    '              The component populates the array with any
    '              summary information it wants to return to Navigator.
    '
    '              There are three levels of Summary, Process,
    '              Map Instance and Map.
    ' ***************************************************************** '
    Public Function NavigatorV2_GetSummary(ByRef vSummaryArray As Object) As Integer Implements aPMNav.NavigatorV2.GetSummary


        Return gPMConstants.PMEReturnCode.PMTrue
    End Function

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserId As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyId As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer




        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserId
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyId
            m_iLogLevel = iLogLevel


            m_oAutomated = New Automated()
            m_lReturn = CType(m_oAutomated.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserId:=iUserId, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyId:=iCurrencyId, iLogLevel:=iLogLevel, sCallingAppName:=sCallingAppName, vDatabase:=vDatabase), gPMConstants.PMEReturnCode)


            Return m_lReturn

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                m_oAutomated = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub

End Class
