Imports Microsoft.Win32.TaskScheduler
Imports Microsoft.Win32.TaskSchedulerEditor
Imports System
Imports System.Globalization
Imports System.Windows.Forms
Imports SharedFiles
Imports Microsoft.VisualBasic
Imports System.Security.Principal
Imports System.IO

Public Class frmFrequencyScheduler
    Inherits System.Windows.Forms.Form
    Private Const ACClass As String = "frmFrequencyScheduler"
    Private m_lStatus As gPMConstants.PMEReturnCode
    Dim m_oTaskScheduler As bSIRTaskScheduler.Business
    Dim g_oObjectManager As bObjectManager.ObjectManager
    Dim dtable As DataTable = Nothing
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_sCallingAppName As String = ""
    Private m_oBusiness As Object
    Private m_oGeneral As iSIRFrequencyScheduler.General
    Private m_oFrequency As iSIRFrequencyScheduler.Interface_Renamed
    Private m_lErrorNumber As gPMConstants.PMEReturnCode
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_lReturn As Integer
    Private m_taskDescription As String
    Private m_dtStartDate As Date
    Private m_dtExpireDate As Date
    Private m_frequencyType As Date
    Private m_idaysOccurances As Integer
    Private m_dtTriggerStartTime As TimeSpan
    Private m_dtTriggerExpireTime As TimeSpan
    Private m_chkEnabled As Boolean
    Private m_daysOfWeek As DaysOfTheWeek
    Private m_monthsOfTheYear As MonthsOfTheYear
    Private m_dayOfMonth() As Integer
    Private m_whichWeek As WhichWeek
    Private m_monthlyDaysOfWeek As DaysOfTheWeek
    Private m_jobCode As String
    Private m_jobDescription As String
    Private m_jobType As String
    Private m_iStatus As Integer
    Private m_userName As String
    Private m_vParameters(,) As Object
    Private m_vfrequencyParameters(,) As Object
    Private m_sbatchStatus As String
    Private m_sProcess As String
    Private m_sProcessDescription As String
    Private m_iBatchProcessId As Integer
    Private m_sbatchFileName As String
    Private m_sbatchContentDetails As String
    Private m_sBatchProcessName As String
    Private m_dtParameters As DataTable = Nothing
    Private m_ibatchSchedulerId As Integer
    Private m_updateMonths As Boolean = False
    Private m_updateMonthlyWeekDays As Boolean = False
    Dim m_taskfoldername As TaskFolder
    Private m_sbatchDirPath As String
    Private m_taskUserName As String
    Private m_taskPassword As String
    Dim objLogin As New TaskLogin()

    Public WriteOnly Property Business() As Object
        Set(ByVal value As Object)
            m_oTaskScheduler = value
        End Set
    End Property




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
    Public Property Task() As Integer
        Get
            ' Standard Property.
            ' Return the task.
            Return m_iTask
        End Get
        Set(ByVal Value As Integer)
            ' Standard Property.
            ' Set the objects task.
            m_iTask = Value
        End Set
    End Property

    Public Property Navigate() As Integer
        Get
            ' Standard Property.
            ' Return the navigate flag.
            Return m_lNavigate
        End Get
        Set(ByVal Value As Integer)
            ' Standard Property.
            ' Set the navigate flag.
            m_lNavigate = Value
        End Set
    End Property

    Public Property ProcessMode() As Integer
        Get
            ' Standard Property.
            ' Return the process mode.
            Return m_lProcessMode
        End Get
        Set(ByVal Value As Integer)
            ' Standard Property.
            ' Set the process mode.
            m_lProcessMode = Value
        End Set
    End Property

    Public Property TransactionType() As String
        Get
            ' Standard Property.
            ' Return the type of business.
            Return m_sTransactionType
        End Get
        Set(ByVal Value As String)
            ' Standard Property.
            ' Set the type of business.
            m_sTransactionType = Value
        End Set
    End Property

    Public Property EffectiveDate() As Date
        Get
            ' Standard Property.
            ' Return the effective date.
            Return m_dtEffectiveDate
        End Get
        Set(ByVal Value As Date)
            ' Standard Property.
            ' Set the effective date.
            m_dtEffectiveDate = Value
        End Set
    End Property

    Public Property TaskDescription() As String
        Get
            ' Standard Property.
            ' Return the effective date.
            Return m_taskDescription
        End Get
        Set(ByVal Value As String)
            ' Standard Property.
            ' Set the effective date.
            m_taskDescription = Value
        End Set
    End Property
    Public Property JobCode() As String
        Get
            ' Return any error number that might have
            ' occurred on the interface.
            Return m_jobCode
        End Get
        Set(value As String)
            m_jobCode = value
        End Set
    End Property
    Public Property JobDescription() As String
        Get
            ' Return any error number that might have
            ' occurred on the interface.
            Return m_jobDescription
        End Get
        Set(value As String)
            m_jobDescription = value
        End Set
    End Property
    Public Property JobType() As String
        Get
            ' Return any error number that might have
            ' occurred on the interface.
            Return m_jobType
        End Get
        Set(value As String)
            m_jobType = value
        End Set
    End Property
    Public Property JobStatus() As String
        Get
            ' Return any error number that might have
            ' occurred on the interface.
            Return m_sbatchStatus
        End Get
        Set(value As String)
            m_sbatchStatus = value
        End Set
    End Property
    Public Property UserName() As String
        Get
            ' Return any error number that might have
            ' occurred on the interface.
            Return m_userName
        End Get
        Set(value As String)
            m_userName = value
        End Set
    End Property
    Public Property Process() As String
        Get
            ' Return any error number that might have
            ' occurred on the interface.
            Return m_sProcess
        End Get
        Set(value As String)
            m_sProcess = value
        End Set
    End Property
    Public Property ProcessDescription() As String
        Get
            ' Return any error number that might have
            ' occurred on the interface.
            Return m_sProcessDescription
        End Get
        Set(value As String)
            m_sProcessDescription = value
        End Set
    End Property

    Public Property BatchFileName() As String
        Get
            ' Return any error number that might have
            ' occurred on the interface.
            Return m_sbatchFileName
        End Get
        Set(value As String)
            m_sbatchFileName = value
        End Set
    End Property
    Public Property BatchFileContentDetails() As String
        Get
            ' Return any error number that might have
            ' occurred on the interface.
            Return m_sbatchContentDetails
        End Get
        Set(value As String)
            m_sbatchContentDetails = value
        End Set
    End Property

    Public Property BatchProcessId() As Integer
        Get
            Return m_iBatchProcessId
        End Get
        Set(ByVal Value As Integer)
            m_iBatchProcessId = Value
        End Set
    End Property
    Public Property BatchProcessName() As String
        Get
            Return m_sBatchProcessName
        End Get
        Set(ByVal Value As String)
            m_sBatchProcessName = Value
        End Set
    End Property
    Public Property ProcessParameters() As DataTable
        Get
            Return m_dtParameters
        End Get
        Set(ByVal Value As DataTable)
            m_dtParameters = Value
        End Set
    End Property
    Public Property BatchSchedulerId() As Integer
        Get
            Return m_ibatchSchedulerId
        End Get
        Set(ByVal Value As Integer)
            m_ibatchSchedulerId = Value
        End Set
    End Property
    Public Property TaskUserName() As String
        Get
            Return m_taskUserName
        End Get
        Set(ByVal Value As String)
            m_taskUserName = Value
        End Set
    End Property
    Public Property TaskPassword() As String
        Get
            Return m_taskPassword
        End Get
        Set(ByVal Value As String)
            m_taskPassword = Value
        End Set
    End Property


    Private Sub frmFrequencyScheduler_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Const kMethodName As String = "frmFrequencyScheduler_Load"

        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        ' Set the process modes for the busines object.
        If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
            ' We have already encountered an error,
            ' so we MUST exit now.
            Exit Sub
        End If

        ' Set the mouse pointer to busy.
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to process the interface.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

            ' Raise Error
            gPMFunctions.RaiseError("Form_Load", "Failed to set the status for the business object")
            Exit Sub
        End If




        ' Set the interface default values.
        m_lReturn = SetInterfaceDefaults()

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Exit Sub
        End If

        ' Gets the interface details to be displayed.
        'm_lReturn = m_oGeneral.GetParameterDetails()

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to get the interface details.
            m_lErrorNumber = m_lReturn

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Exit Sub
        End If

        ' Set the interface status to cancelled. This is done
        ' so that any interface termination will be noted
        ' as cancelled except in the event of accepting
        ' the interface.
        m_lStatus = gPMConstants.PMEReturnCode.PMCancel

        iPMFunc.CenterForm(Me)




        '   ScheduleTaskInWin_TaskScheduler(sunday, monday,
        '  tuesday, Wednesday, Thursday, Friday, Saturday)
    End Sub
    'Public Function Initialise() As gPMConstants.PMEReturnCode

    '    Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse

    '    Try


    '        ' Set the mouse pointer to busy.
    '        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

    '        result = gPMConstants.PMEReturnCode.PMTrue

    '        ' Get Document business object
    '        'Dim temp_g_oBusiness As Object = Nothing
    '        ''m_lReturn = g_oObjectManager.GetInstance(temp_g_oBusiness, "bSIRBatchRenewalJobs.Business", vInstanceManager:="ClientManager")
    '        ''g_oBusiness = temp_g_oBusiness


    '        'm_lReturn = g_oObjectManager.GetInstance(temp_g_oBusiness, "bSIRTaskScheduler.Business", vInstanceManager:="ClientManager")
    '        'g_oTaskSchedulerBusiness = temp_g_oBusiness
    '        'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '        '    gPMFunctions.RaiseError("g_oObjectManager.GetInstance", "sClassName:='bSIRTaskScheduler.Form'", gPMConstants.PMELogLevel.PMLogError)
    '        'End If


    '        ' Set the mouse pointer to normal.
    '        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

    '        Return result

    '    Catch excep As System.Exception



    '        ' Error Section
    '        result = gPMConstants.PMEReturnCode.PMError

    '        ' Log Error.
    '        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

    '        Return result

    '    End Try
    'End Function

    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetInterfaceDefaults"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                cmdOK.Enabled = True

            Else
                cmdOK.Enabled = False
            End If

            m_lReturn = GetBatchFrequencyParameters(m_ibatchSchedulerId, m_vfrequencyParameters)
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                Dim m_selFrequency As String
                If Information.IsArray(m_vfrequencyParameters) Then
                    ' Process all treaties
                    For lCount As Integer = m_vfrequencyParameters.GetLowerBound(1) To m_vfrequencyParameters.GetUpperBound(1)

                        m_selFrequency = CStr(m_vfrequencyParameters(FrequencyParameterDetailEnum.DBMFrequency, 0))
                        If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                            Select Case m_selFrequency
                                Case "One Time"

                                    rdFrequency(0).Checked = True
                                    rdFrequency(1).Enabled = False
                                    rdFrequency(2).Enabled = False
                                    rdFrequency(3).Enabled = False


                                Case "Daily"
                                    rdFrequency(0).Enabled = False
                                    rdFrequency(1).Checked = True
                                    rdFrequency(2).Enabled = False
                                    rdFrequency(3).Enabled = False



                                Case "Weekly"
                                    rdFrequency(0).Enabled = False
                                    rdFrequency(1).Enabled = False
                                    rdFrequency(2).Checked = True
                                    rdFrequency(3).Enabled = False
                                Case "Monthly"
                                    rdFrequency(0).Enabled = False
                                    rdFrequency(1).Enabled = False
                                    rdFrequency(2).Enabled = False
                                    rdFrequency(3).Checked = True
                                Case "Monthly DOW"
                                    rdFrequency(0).Enabled = False
                                    rdFrequency(1).Enabled = False
                                    rdFrequency(2).Enabled = False
                                    rdFrequency(3).Checked = True
                            End Select
                        Else
                            Select Case m_selFrequency
                                Case "One Time"
                                    rdFrequency(0).Checked = True
                                Case "Daily"
                                    rdFrequency(1).Checked = True
                                Case "Weekly"
                                    rdFrequency(2).Checked = True
                                Case "Monthly"
                                    rdFrequency(3).Checked = True
                                Case "Monthly DOW"
                                    rdFrequency(3).Checked = True
                            End Select
                        End If
                        SetFrequencyControlValue(m_vfrequencyParameters(FrequencyParameterDetailEnum.DBMParameterName, lCount), m_vfrequencyParameters(FrequencyParameterDetailEnum.DBMCurrentIDValue, lCount), CStr(m_vfrequencyParameters(FrequencyParameterDetailEnum.DBMFrequencyType, lCount)))

                        '  m_sProcessSelected = CStr(m_vfrequencyParameters(MainModule.BatchSchedulerEnum.DBMProcessSelected, lCount))


                    Next
                End If


            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function SetFrequencyControlValue(ByVal parameterName As String, ByVal parameterValue As String, ByVal frequencyType As String) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "SetFrequencyControlValue"
        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            If parameterName = "StartDate" Then
                dateTimePickerStartDate.Value = parameterValue
            ElseIf parameterName = "StartTime" Then
                dateTimePickerTriggerTime.Value = DateTime.Parse(parameterValue)
            ElseIf parameterName = "StartTime" Then
                dateTimePickerTriggerTime.Value = DateTime.Parse(parameterValue)
            ElseIf parameterName = "Enabled" Then
                chkEnabled.Checked = parameterValue
            ElseIf parameterName = "ExpireDate" Then
                chkExpireDate.Checked = True
                dateTimePickerExpireDate.Value = parameterValue
            ElseIf parameterName = "ExpireTime" Then
                dateTimePickerExpireTime.Value = DateTime.Parse(parameterValue)
            ElseIf parameterName = "Occurance" Then
                txtDaysDaily.Value = parameterValue
            ElseIf parameterName = "Days" Then

                For iRowCnt As Integer = 0 To chkListBoxWeeklyDays.Items.Count - 1
                    If chkListBoxWeeklyDays.Items(iRowCnt).ToString() = parameterValue Then
                        chkListBoxWeeklyDays.SetItemChecked(iRowCnt, True)
                    End If

                Next
                'For Each dayVal As String In [Enum].GetNames(GetType(EnumDaysOfWeek) '[Enum].GetNames(GetType(EnumDaysOfWeek)) ' [Enum].GetValues(GetType(EnumDaysOfWeek))
                'If dayVal = parameterValue Then
                ' CType(dayVal, DaysOfTheWeek)
                ' If  Then
                'chkListBoxWeeklyDays.SetItemChecked([Enum].GetValues(GetType(EnumDaysOfWeek)) [Enum].GetName(EnumDaysOfWeek, parameterValue), True)
                ' End If

                'Next
            ElseIf parameterName = "Month" Then
                For iRowCnt As Integer = 0 To checkedListBoxMonthlyMonths.Items.Count - 1
                    If checkedListBoxMonthlyMonths.Items(iRowCnt).ToString() = parameterValue Then
                        checkedListBoxMonthlyMonths.SetItemChecked(iRowCnt, True)
                    End If

                Next
            ElseIf parameterName = "MonthDays" Then
                For iRowCnt As Integer = 0 To checkedListBoxMonthlyDays.Items.Count - 1
                    If checkedListBoxMonthlyDays.Items(iRowCnt).ToString() = parameterValue Then
                        checkedListBoxMonthlyDays.SetItemChecked(iRowCnt, True)
                    End If

                Next
            ElseIf parameterName = "On" And frequencyType = "WeekOfMonth" Then
                rdMonthDaysOfWeek.Checked = True
                For iRowCnt As Integer = 0 To chkListBoxMonthlyWeekNumber.Items.Count - 1
                    If chkListBoxMonthlyWeekNumber.Items(iRowCnt).ToString() = parameterValue Then
                        chkListBoxMonthlyWeekNumber.SetItemChecked(iRowCnt, True)
                    End If

                Next
            ElseIf parameterName = "On" And frequencyType = "DaysOfTheWeek" Then
                rdMonthDaysOfWeek.Checked = True
                For iRowCnt As Integer = 0 To chkListBoxMonthlyWeekDay.Items.Count - 1
                    If chkListBoxMonthlyWeekDay.Items(iRowCnt).ToString() = parameterValue Then
                        chkListBoxMonthlyWeekDay.SetItemChecked(iRowCnt, True)
                    End If

                Next
            ElseIf parameterName = "rdLoggedOnSecurityOption" Then
                rdLoggedOnSecurityOption.Checked = True
            ElseIf parameterName = "rdLoggedOnOrNotSecurityOption" Then
                rdLoggedOnorNotSecurityOption.Checked = True
            End If
        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
        Return result
    End Function

    Private Function BusinessToData() As Integer

        Dim result As Integer = 0
        Try

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Public Function BusinessToInterface() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' Assign the details from the business object
            ' to the data storage.
            m_lReturn = BusinessToData()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the parameter details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    'Private Function ProcessCommand() As Integer


    '    Dim result As Integer = 0
    '    Try

    '        result = gPMConstants.PMEReturnCode.PMTrue

    '        ' Check the task.
    '        If m_lStatus <> gPMConstants.PMEReturnCode.PMCancel Then


    '            Select Case m_iTask
    '                Case gPMConstants.PMEComponentAction.PMAdd

    '                    ' Update the business from the interface.
    '                    m_lReturn = InterfaceToBusiness()

    '                    ' Check for errors.
    '                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '                        ' Failed to update the details
    '                        result = gPMConstants.PMEReturnCode.PMFalse

    '                        ' Log Error.
    '                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update the details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand")
    '                    End If

    '            End Select

    '        End If

    '        Return result

    '    Catch excep As System.Exception



    '        ' Error Section.

    '        result = gPMConstants.PMEReturnCode.PMTrue

    '        ' Log Error.
    '        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process command", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

    '        Return result

    '    End Try
    'End Function

    'Public Function InterfaceToBusiness() As Integer

    '    Dim nResult As Integer = 0
    '    Dim nAuditSetID As Integer
    '    Dim dtRecurringDate As Date
    '    Dim dtLastDate As Date
    '    Dim nOccurances As Integer
    '    Dim sDocumentRef As String
    '    Dim sComment As String
    '    Dim sOffset As String = ""
    '    Dim nMonths As Byte
    '    Dim oTransArray(,) As Object
    '    Dim sReference As String = ""

    '    Try

    '        nResult = gPMConstants.PMEReturnCode.PMTrue
    '        m_lReturn = InterfaceToData()
    '        ' Check for errors
    '        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '            Return gPMConstants.PMEReturnCode.PMFalse
    '        End If

    '        If rdFrequency(0).Checked Then
    '            ' Per Period
    '            sOffset = txtOccursPer(0).Value
    '            nMonths = 0
    '        ElseIf (optOccurs(1).Checked) Then
    '            ' Per Month
    '            sOffset = txtOccursPer(1).Value

    '            m_lReturn = GetMonthsForward(r_vNextDate:=dtLastDate, v_vOffset:=sOffset, v_vMonths:=nMonths)
    '        ElseIf (optOccurs(2).Checked) Then
    '            ' Per Quarter
    '            sOffset = txtOccursPer(2).Value

    '            m_lReturn = GetMonthsForward(r_vNextDate:=dtLastDate, v_vOffset:=sOffset, v_vMonths:=nMonths)
    '        Else
    '            ' Something's wrong
    '            sOffset = CStr(0)
    '        End If

    '        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '            Return gPMConstants.PMEReturnCode.PMFalse
    '        End If

    '        ' Recurring document
    '        If m_bRecurringDocument Then
    '            m_lReturn = m_oAuditSet.DirectAdd(vAuditsetID:=nAuditSetID, vCompanyID:=g_iCompanyID, vUserID:=g_iUserID, vPostedDate:=DateTime.Now, vComment:="Recurring Document")
    '            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '                Return gPMConstants.PMEReturnCode.PMFalse
    '            End If
    '        End If

    '        ' Reversing document
    '        If m_bReversingDocument Then
    '            m_lReturn = m_oAuditSet.DirectAdd(vAuditsetID:=nAuditSetID, vCompanyID:=g_iCompanyID, vUserID:=g_iUserID, vPostedDate:=DateTime.Now, vComment:="Reversing Document")
    '        End If

    '        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '            nResult = gPMConstants.PMEReturnCode.PMFalse
    '            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
    '            Return nResult
    '        End If

    '        m_dtStartDate = dtpStartDate.Value
    '        Return nResult

    '    Catch excep As System.Exception
    '        nResult = gPMConstants.PMEReturnCode.PMError

    '        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

    '        Return nResult

    '    End Try
    'End Function

    ' ***************************************************************** '
    ' Name: InterfaceToData
    '
    ' Description: Updates the data storage from the interface details.
    '
    ' ***************************************************************** '

    'Private Function InterfaceToData() As Integer

    '    Dim result As Integer = 0
    '    Try

    '        result = gPMConstants.PMEReturnCode.PMTrue

    '        ' Update the data storage.

    '        ' {* USER DEFINED CODE (Begin) *}
    '        If Information.IsDate(dtpDocumentDate.Value) Then
    '            m_dtDocumentDate = (CDate(gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateLong, vFieldValue:=dtpDocumentDate.Value)))
    '        Else

    '            dtpDocumentDate.Value = DateTime.Today
    '            m_dtDocumentDate = (CDate(gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateLong, vFieldValue:=dtpDocumentDate.Value)))
    '        End If

    '        m_sComment = txtComment.Text.Trim()
    '        m_iDocumenttypeID = cmbDocumentType.ItemId
    '        m_iBranchID = VB6.GetItemData(cboBranches, cboBranches.SelectedIndex)

    '        ' Reversing document
    '        If dtpReverseDate.Enabled Then
    '            m_bReversingDocument = True
    '            ' Get the reverse date
    '            If Information.IsDate(dtpReverseDate.Value) Then
    '                m_dtReverseDate = (CDate(gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateLong, vFieldValue:=dtpReverseDate.Value)))
    '            End If
    '        Else
    '            m_bReversingDocument = False
    '        End If

    '        ' Recurring document
    '        If txtOccurs.Enabled Then
    '            m_bRecurringDocument = True
    '            ' Get the occurances
    '            Dim dbNumericTemp As Double
    '            If Double.TryParse(txtOccurs.Value, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
    '                m_iOccurances = CInt(txtOccurs.Text)
    '            End If
    '        Else
    '            m_bRecurringDocument = False
    '        End If

    '        ' {* USER DEFINED CODE (End) *}

    '        Return result

    '    Catch excep As System.Exception



    '        ' Error Section.

    '        result = gPMConstants.PMEReturnCode.PMError

    '        ' Log Error.
    '        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

    '        Return result

    '    End Try
    'End Function

    Public Function initializeObject() As Integer 'PN 2062-Ritu
        Dim temp_m_oBusiness As Object = Nothing
        g_oObjectManager = New bObjectManager.ObjectManager()
        m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRTaskScheduler.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        m_oBusiness = temp_m_oBusiness
        Return m_lReturn
    End Function

    Private Sub Form_Initialize_Renamed()

        Try
            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
            Const kMethodName As String = "Form_Initialize"
            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue
            ' initializeObject()

            ' Create an instance of the general interface object.
            m_oGeneral = New iSIRFrequencyScheduler.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            'Dim temp_m_oBusiness As Object = Nothing
            'm_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRTaskScheduler.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            'm_oBusiness = temp_m_oBusiness

            ' Check for errors.
            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '    gPMFunctions.RaiseError("g_oObjectManager.GetInstance", "Unable to get instance of business object")
            'End If


            'Dim temp_obSIRBatchScheduler As Object = Nothing
            'm_lReturn = g_oObjectManager.GetInstance(temp_obSIRBatchScheduler, "bSIRTaskScheduler.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)

            'm_oBusiness = temp_obSIRBatchScheduler
            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmFrequencyScheduler_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        'If OK not clicked signify cancel to calling form  PN23199
        If m_lStatus <> gPMConstants.PMEReturnCode.PMOK Then
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel
        End If
        eventArgs.Cancel = Cancel <> 0
    End Sub



    Public Function GetBatchProcess(ByVal v_vbatchprocesses_list_id As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetBatchProcess"
        Dim r_vResultArray(,) As Object

        result = gPMConstants.PMEReturnCode.PMTrue

        m_vParameters = Nothing
        'g_oObjectManager = New bObjectManager.ObjectManager
        Dim objCls As New iSIRFrequencyScheduler.Interface_Renamed() ' bSIRTaskScheduler.Business
        objCls.Initialise()
        m_oBusiness = objCls
        ' objCls = New bSIRTaskScheduler.Business()
        '  If (m_oBusiness Is Nothing) Then
        ' initializeObject()
        'End If
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("Form_initalize", "Failed to create instance of Task Scheduler")
        End If

        m_lReturn = m_oTaskScheduler.GetBatchProcess(v_vbatchprocesses_list_id:=v_vbatchprocesses_list_id, m_vParameters:=m_vParameters)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "Failed to call bSIRTaskScheduler.GetbatchJobs")
        End If

        GoTo Finally_Renamed

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to call bSIRTaskScheduler.GetbatchJobs", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBatchProcess")



Finally_Renamed:
        Return result
        Resume
        Return result
    End Function

    Public Function GetBatchFrequencyParameters(ByVal v_vbatch_scheduler_id As Object, ByRef m_vfrequencyParameters As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetBatchFrequencyParameters"
        Dim r_vResultArray(,) As Object

        result = gPMConstants.PMEReturnCode.PMTrue

        m_vfrequencyParameters = Nothing
        'g_oObjectManager = New bObjectManager.ObjectManager
        Dim objCls As New iSIRFrequencyScheduler.Interface_Renamed() ' bSIRTaskScheduler.Business
        m_lReturn = objCls.Initialise()
        m_oBusiness = objCls
        ' objCls = New bSIRTaskScheduler.Business()
        '  If (m_oBusiness Is Nothing) Then
        ' initializeObject()
        'End If
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("Form_initalize", "Failed to create instance of Task Scheduler")
        End If

        m_lReturn = m_oTaskScheduler.GetBatchFrequencyParameters(v_vbatch_scheduler_id:=v_vbatch_scheduler_id, m_vfrequencyParameters:=m_vfrequencyParameters)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "Failed to call bSIRTaskScheduler.GetbatchJobs")
        End If

        GoTo Finally_Renamed

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to call bSIRTaskScheduler.GetbatchJobs", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBatchProcess")



Finally_Renamed:
        Return result
        Resume
        Return result
    End Function


    Public Function ScheduleTaskInWindowsTaskScheduler() As gPMConstants.PMEReturnCode
        Dim cntParam As Integer = 0
        Dim m_iBatchProcessId As Integer
        'Dim m_sProcess As String
        'Dim m_sJobDescription As String
        Dim m_sFrequencyType As String = "OneTimeOnly"
        Dim m_sFrequencySubType As String = String.Empty
        Dim m_sFrequencyDescription As String = String.Empty
        Dim v_resultArray(,) As Object
        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim isNewTask As Boolean = True
        Dim m_sCurrentProcessDescription As String = String.Empty
        Dim m_sCurrentBatchFileName As String = String.Empty
        Try


            m_iBatchProcessId = BatchProcessId
            '  m_sProcess = "Renewal" + " " + JobType
            ' m_sJobDescription = JobCode + "" + JobDescription

            m_dtStartDate = dateTimePickerStartDate.Value.Date
            m_dtTriggerStartTime = dateTimePickerTriggerTime.Value.TimeOfDay

            dtable.LoadDataRow(New String(4) {String.Empty, "StartDate", DateTime.Now.Date.ToString(), "Date", m_dtStartDate.ToString()}, True)
            dtable.LoadDataRow(New String(4) {String.Empty, "StartTime", DateTime.Now.TimeOfDay.ToString(), "Time", m_dtTriggerStartTime.ToString()}, True)

            If (chkExpireDate.Checked) Then
                m_dtExpireDate = dateTimePickerExpireDate.Value.Date
                m_dtTriggerExpireTime = dateTimePickerExpireTime.Value.TimeOfDay
                dtable.LoadDataRow(New String(4) {String.Empty, "ExpireDate", DateTime.Now.Date.ToString(), "Date", m_dtExpireDate.ToString()}, True)
                dtable.LoadDataRow(New String(4) {String.Empty, "ExpireTime", DateTime.Now.TimeOfDay.ToString(), "Time", m_dtTriggerExpireTime.ToString()}, True)


                If Not m_dtStartDate.Equals(DateTime.FromOADate(0)) And Not m_dtExpireDate.Equals(DateTime.FromOADate(0)) Then
                    Dim numberOfDays As Integer = DateAndTime.DateDiff("d", m_dtStartDate, m_dtExpireDate, FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1)
                    If numberOfDays < 0 Then
                        m_lReturn = MessageBox.Show("Expires Date and Time should be greater than Start Date and Time.", "BatchJob Scheduler Configuration", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                        Exit Function
                    ElseIf numberOfDays = 0 Then
                        Dim m_sStartTime As String = String.Empty
                        Dim m_sEndTime As String = String.Empty
                        If m_dtTriggerStartTime.ToString().LastIndexOf(".") > 0 Then
                            m_sStartTime = m_dtTriggerStartTime.ToString().Substring(0, m_dtTriggerStartTime.ToString().LastIndexOf("."))
                        Else
                            m_sStartTime = m_dtTriggerStartTime.ToString()
                        End If
                        If m_dtTriggerExpireTime.ToString().LastIndexOf(".") > 0 Then
                            m_sEndTime = m_dtTriggerExpireTime.ToString().Substring(0, m_dtTriggerExpireTime.ToString().LastIndexOf("."))
                        Else
                            m_sEndTime = m_dtTriggerExpireTime.ToString()
                        End If
                        If DateTime.Parse(m_sStartTime).TimeOfDay() >= DateTime.Parse(m_sEndTime).TimeOfDay() Then
                            'If TimeFormat(m_dtTriggerExpireTime) > TimeFormat(m_dtTriggerStartTime) Then 'DateTime.Parse(m_dtTriggerStartTime.ToString("hh:mm:ss")).TimeOfDay() > DateTime.Parse(m_dtTriggerExpireTime.ToString("hh:mm:ss")).TimeOfDay() Then
                            m_lReturn = MessageBox.Show("Expires Date and Time should be greater than Start Date and Time.", "BatchJob Scheduler Configuration", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                            Exit Function
                        End If
                    End If

                End If
            End If


            m_chkEnabled = chkEnabled.Checked
            dtable.LoadDataRow(New String(4) {String.Empty, "Enabled", "1", "Integer", m_chkEnabled.ToString()}, True)

            Dim userId As String = String.Empty
            Dim userPassword As String = String.Empty
            'Dim tSetting As TriggerEditDialog()
            Dim taskSetting As TaskSettings
            'Dim tSecurity As TaskSecurity
            'Dim tCondition As TaskServiceConnectDialog
            'Dim triggerCollection As TriggerCollection
            'Dim taskConfigurefor As TaskOptionsEditor
            Using tService As New TaskService()


                Dim tDefinition As TaskDefinition = tService.NewTask

                ' Environment.MachineName & "\" & "SiriusOnline" 'System.Security.Principal.WindowsIdentity.GetCurrent().Name 'String.Concat(Environment.UserDomainName, Chr(34) & "\\" & Chr(34), Environment.UserName)
                tDefinition.RegistrationInfo.Description =
                       JobDescription
                tDefinition.Settings.ExecutionTimeLimit = TimeSpan.FromMinutes(15)


                taskSetting = tDefinition.Settings
                taskSetting.Enabled = m_chkEnabled
                taskSetting.ExecutionTimeLimit = TimeSpan.FromDays(3)
                ' taskSetting.DisallowStartIfOnBatteries = True
                ' taskSetting.RunOnlyIfLoggedOn = False
                'tDefinition.Settings.Compatibility = TaskCompatibility.V2_3
                'tDefinition.Principal.RunLevel = TaskRunLevel.Highest
                Dim tOneTimeOnly As New TimeTrigger
                Dim tDaily As New DailyTrigger()
                Dim tWeeklyTrigger As New WeeklyTrigger()
                Dim tMonthlyTrigger As New MonthlyTrigger()
                Dim tMonthlyDOWTrigger As New MonthlyDOWTrigger()
                If rdFrequency(0).Checked Then
                    '   m_sFrequencyType = "One Time"
                    'tOneTimeOnly.RandomDelay = TimeSpan.FromSeconds(30)
                    tOneTimeOnly.StartBoundary = m_dtStartDate + m_dtTriggerStartTime
                    If (chkExpireDate.Checked) Then
                        tOneTimeOnly.EndBoundary = m_dtExpireDate + m_dtTriggerExpireTime
                    End If
                    m_sFrequencyDescription = m_sFrequencyType + " starting on " + m_dtStartDate.ToString("MMM dd yyyy")
                    tDefinition.Triggers.Add(tOneTimeOnly)
                ElseIf rdFrequency(1).Checked Then  'Daily 
                    m_sFrequencyType = "Daily"
                    tDaily.StartBoundary = m_dtStartDate + m_dtTriggerStartTime
                    If txtDaysDaily.Text <> String.Empty AndAlso gPMFunctions.ToSafeInteger(txtDaysDaily.Text) > 0 AndAlso gPMFunctions.ToSafeInteger(txtDaysDaily.Text) < 366 Then
                        m_idaysOccurances = txtDaysDaily.Value
                    Else
                        m_lReturn = MessageBox.Show("Enter a numeric value between 1 to 365.", "BatchJob Scheduler Configuration", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                        Exit Function
                    End If
                    tDaily.DaysInterval = m_idaysOccurances
                    If (chkExpireDate.Checked) Then
                        tDaily.EndBoundary = m_dtExpireDate + m_dtTriggerExpireTime
                    End If
                    m_sFrequencyDescription = m_sFrequencyType + " starting on " + m_dtStartDate.ToString("MMM dd yyyy") + " on every " + m_idaysOccurances.ToString() + " days"
                    ' tDaily.RandomDelay = TimeSpan.FromHours(2)
                    tDefinition.Triggers.Add(tDaily)

                    dtable.LoadDataRow(New String(4) {"Days", "Occurance", "1", "Integer", m_idaysOccurances.ToString()}, True)

                ElseIf rdFrequency(2).Checked Then 'weekly
                    m_sFrequencyType = "Weekly"
                    tWeeklyTrigger.StartBoundary = m_dtStartDate + m_dtTriggerStartTime
                    If (chkExpireDate.Checked) Then
                        tWeeklyTrigger.EndBoundary = m_dtExpireDate + m_dtTriggerExpireTime ' DateTime.Today + TimeSpan.FromHours(10)
                    End If


                    If txtDaysDaily.Text <> String.Empty AndAlso gPMFunctions.ToSafeInteger(txtDaysDaily.Text) > 0 AndAlso gPMFunctions.ToSafeInteger(txtDaysDaily.Text) < 53 Then
                        m_idaysOccurances = txtDaysDaily.Value
                    Else
                        m_lReturn = MessageBox.Show("Enter a numeric value between 1 to 52.", "BatchJob Scheduler Configuration", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                        Exit Function
                    End If
                    tWeeklyTrigger.WeeksInterval = m_idaysOccurances ' 5
                    dtable.LoadDataRow(New String(4) {"Weeks", "Occurance", "1", "Integer", m_idaysOccurances.ToString()}, True)

                    If chkListBoxWeeklyDays.CheckedItems.Count > 0 Then
                        ' Set the active days for weekly trigger
                        For cnt As Integer = 0 To 6 ' Set the active Days
                            If chkListBoxWeeklyDays.GetItemChecked(cnt) Then
                                GetWeeklyMonthlyDays(cnt)

                            End If
                        Next
                        For chk As Integer = 0 To chkListBoxWeeklyDays.CheckedItems.Count - 1 ' Weekly Days Insert
                            dtable.LoadDataRow(New String(4) {"DaysOfWeek", "Days", "AllDays", "String", chkListBoxWeeklyDays.CheckedItems(chk).ToString()}, True)
                        Next
                    Else
                        MessageBox.Show("Day of week is not selected", "BatchJob Scheduler Configuration", MessageBoxButtons.OK, MessageBoxIcon.Error)

                        Exit Function

                    End If
                    tWeeklyTrigger.DaysOfWeek = m_daysOfWeek
                    m_sFrequencyDescription = m_sFrequencyType + " starting on " + m_dtStartDate.ToString("MMM dd yyyy") + " , on every " + m_idaysOccurances.ToString() + " weeks every (" + m_daysOfWeek.ToString() + ")"

                    tDefinition.Triggers.Add(tWeeklyTrigger)
                ElseIf rdFrequency(3).Checked Then
                    Dim m_sdays As String = String.Empty
                    m_sFrequencyType = "Monthly"
                    tMonthlyTrigger.StartBoundary = m_dtStartDate + m_dtTriggerStartTime 'DateTime.Today.AddDays(2)
                    If (chkExpireDate.Checked) Then
                        tMonthlyTrigger.EndBoundary = m_dtExpireDate + m_dtTriggerExpireTime ' DateTime.Today + TimeSpan.FromHours(10)
                    End If

                    If checkedListBoxMonthlyMonths.CheckedItems.Count > 0 Then

                        For month As Integer = 0 To checkedListBoxMonthlyMonths.CheckedItems.Count - 1

                            GetMonth(checkedListBoxMonthlyMonths.CheckedItems(month))

                            'triggerItem.TriggerSettings.Monthly.Month[month]() = checkedListBoxMonthlyMonths.GetItemChecked(month);
                            dtable.LoadDataRow(New String(4) {"MonthsOfYear", "Month", "AllMonth", "String", checkedListBoxMonthlyMonths.CheckedItems(month).ToString()}, True)

                        Next
                    Else


                        MessageBox.Show("Months Selection required", "BatchJob Scheduler Configuration", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Function
                    End If


                    tMonthlyTrigger.MonthsOfYear = m_monthsOfTheYear
                    Dim excludeLastCount As Integer = 0
                    If rdMonthsDays.Checked Then
                        m_sFrequencySubType = "MonthDays"
                        If checkedListBoxMonthlyDays.CheckedItems.Count > 0 Then

                            For Each item As String In checkedListBoxMonthlyDays.CheckedItems
                                If item.Contains("Last") Then

                                Else
                                    excludeLastCount = excludeLastCount + 1
                                End If

                            Next

                            ReDim m_dayOfMonth(excludeLastCount - 1)
                            '// Set active Days (0..30 = Days, 31=last Day) for monthly trigger
                            For dayOfMonth As Integer = 0 To checkedListBoxMonthlyDays.CheckedItems.Count - 1
                                If checkedListBoxMonthlyDays.CheckedItems(dayOfMonth) = "Last Day" Then
                                    tMonthlyTrigger.RunOnLastDayOfMonth = True
                                    If m_sdays = String.Empty Then
                                        m_sdays = "Last Day"
                                    Else
                                        m_sdays += "Last Day"
                                    End If

                                Else
                                    m_dayOfMonth(dayOfMonth) = checkedListBoxMonthlyDays.CheckedItems(dayOfMonth)
                                    If m_sdays = String.Empty Then
                                        m_sdays = checkedListBoxMonthlyDays.CheckedItems(dayOfMonth).ToString()
                                    Else
                                        m_sdays += "," + checkedListBoxMonthlyDays.CheckedItems(dayOfMonth).ToString()
                                    End If

                                End If
                                dtable.LoadDataRow(New String(4) {"DaysOfMonth", "MonthDays", "AllDays", "String", checkedListBoxMonthlyDays.CheckedItems(dayOfMonth).ToString()}, True)

                            Next
                            tMonthlyTrigger.DaysOfMonth = m_dayOfMonth
                            m_sFrequencyDescription = m_sFrequencyType + " starting on " + m_dtStartDate.ToString("MMM dd yyyy") + " on " + m_sdays + " days of " + "(" + m_monthsOfTheYear.ToString() + ") month"
                        Else
                            MessageBox.Show("Days are not selected", "BatchJob Scheduler Configuration", MessageBoxButtons.OK, MessageBoxIcon.Error)

                            Exit Function

                        End If
                        tDefinition.Triggers.Add(tMonthlyTrigger)
                    End If


                    If rdMonthDaysOfWeek.Checked Then
                        m_sFrequencyType = "Monthly DOW"
                        m_sFrequencySubType = "MonthDaysOfWeek"
                        tMonthlyDOWTrigger.StartBoundary = m_dtStartDate + m_dtTriggerStartTime 'DateTime.Today.AddDays(2)
                        If (chkExpireDate.Checked) Then
                            tMonthlyDOWTrigger.EndBoundary = m_dtExpireDate + m_dtTriggerExpireTime ' DateTime.Today + TimeSpan.FromHours(10)
                        End If
                        tMonthlyDOWTrigger.MonthsOfYear = m_monthsOfTheYear
                        If chkListBoxMonthlyWeekNumber.CheckedItems.Count > 0 Then

                            For weeknumber As Integer = 0 To chkListBoxMonthlyWeekNumber.CheckedItems.Count - 1
                                If chkListBoxMonthlyWeekNumber.CheckedItems(weeknumber) = "Last" Then
                                    tMonthlyDOWTrigger.RunOnLastWeekOfMonth = True
                                    m_whichWeek = m_whichWeek Or WhichWeek.LastWeek
                                Else
                                    'm_whichWeek = m_whichWeek Or CType(chkListBoxMonthlyWeekNumber.CheckedItems(weeknumber), WhichWeek)
                                    GetDOWWhichWeek(chkListBoxMonthlyWeekNumber.CheckedItems(weeknumber))
                                End If
                                dtable.LoadDataRow(New String(4) {"WeekOfMonth", "On", "All", "String", chkListBoxMonthlyWeekNumber.CheckedItems(weeknumber).ToString()}, True)

                                'cntParam += 1
                                '    triggerItem.TriggerSettings.Monthly.WeekDay.DayOfWeek[Day] = checkedListBoxMonthlyWeekDay.GetItemChecked(Day);
                            Next

                            tMonthlyDOWTrigger.WeeksOfMonth = m_whichWeek
                        Else
                            MessageBox.Show("Week of Month is not selected", "BatchJob Scheduler Configuration", MessageBoxButtons.OK, MessageBoxIcon.Error)

                            Exit Function

                            'm_whichWeek = WhichWeek.AllWeeks
                            'tMonthlyDOWTrigger.WeeksOfMonth = WhichWeek.AllWeeks
                            'dtable.LoadDataRow(New String(4) {"WeekOfMonth", "On", "AllWeeks", "String", "AllWeeks"}, True)

                        End If



                        If chkListBoxMonthlyWeekDay.CheckedItems.Count > 0 Then
                            For day As Integer = 0 To chkListBoxMonthlyWeekDay.CheckedItems.Count - 1
                                If chkListBoxMonthlyWeekDay.CheckedItems(day) = "ALL" Then
                                    m_monthlyDaysOfWeek = DaysOfTheWeek.AllDays
                                    Exit For
                                Else
                                    'm_monthlyDaysOfWeek = m_monthlyDaysOfWeek Or CType(chkListBoxMonthlyWeekDay.CheckedItems(day), DaysOfTheWeek)
                                    GetMonthlyDaysOfWeek(chkListBoxMonthlyWeekDay.CheckedItems(day))
                                    dtable.LoadDataRow(New String(4) {"DaysOfTheWeek", "On", "AllDays", "String", chkListBoxMonthlyWeekDay.CheckedItems(day).ToString()}, True)
                                End If

                            Next
                            If m_monthlyDaysOfWeek = DaysOfTheWeek.AllDays Then
                                For day As Integer = 0 To chkListBoxMonthlyWeekDay.CheckedItems.Count - 1
                                    dtable.LoadDataRow(New String(4) {"DaysOfTheWeek", "On", "AllDays", "String", chkListBoxMonthlyWeekDay.CheckedItems(day).ToString()}, True)
                                Next
                            End If

                            tMonthlyDOWTrigger.DaysOfWeek = m_monthlyDaysOfWeek
                        Else
                            MessageBox.Show("Days of the week are not selected", "BatchJob Scheduler Configuration", MessageBoxButtons.OK, MessageBoxIcon.Error)

                            Exit Function

                            'm_monthlyDaysOfWeek = DaysOfTheWeek.AllDays
                            'tMonthlyDOWTrigger.DaysOfWeek = DaysOfTheWeek.AllDays
                            'dtable.LoadDataRow(New String(4) {"DaysOfTheWeek", "On", "AllDays", "String", "AllDays"}, True)

                        End If
                        m_sFrequencyDescription = m_sFrequencyType + " starting on " + m_dtStartDate.ToString("MMM dd yyyy") + " on " + m_whichWeek.ToString() + "(" + m_monthlyDaysOfWeek.ToString() + ")"
                        tDefinition.Triggers.Add(tMonthlyDOWTrigger)

                    End If

                    '    triggerItem.TriggerSettings.Monthly.DaysOfMonth[Day] = checkedListBoxMonthlyDays.GetItemChecked(Day);


                End If

                Dim taskFolder As TaskFolder = GetTaskFolder(tService)
                Dim applicationPath As String = Application.StartupPath()

                CreateBatchFile()


                tDefinition.Actions.Add(New ExecAction(m_sbatchDirPath & "\" & BatchFileName))
                ' tDefinition.Actions.Add(New ExecAction("notepad.exe", "C:\\test.log",))
                'tService.RootFolder.RegisterTaskDefinition(JobDescription & " Task",
                '   tDefinition)
                'applicationPath & "\" & BatchLogFileName))
                ' If isNewTask Then
                If m_iTask = PMEComponentAction.PMEdit Then

                    GetCurrentBatchProcess(r_vBatchProcessesDescription:=m_sCurrentProcessDescription, v_ibatchSchedulerId:=BatchSchedulerId, r_vCurrentFileName:=m_sCurrentBatchFileName)
                    If m_sCurrentProcessDescription IsNot Nothing Then

                        Dim currentTask As String = "" 'tService.GetFolder("PureTaskFolder") 'FindTask(m_sCurrentProcessDescription)
                        For Each task As Task In taskFolder.Tasks
                            currentTask = task.Name
                            If currentTask = m_sCurrentProcessDescription Then
                                isNewTask = False
                            End If

                        Next
                        If isNewTask = False Then

                            'tService.RootFolder.DeleteTask(m_sCurrentProcessDescription.Trim())
                            taskFolder.DeleteTask(m_sCurrentProcessDescription.Trim())

                        End If
                        If m_sCurrentBatchFileName <> String.Empty Then
                            If File.Exists(m_sbatchDirPath & "\" & m_sCurrentBatchFileName & ".bat") Then

                                File.Delete(m_sbatchDirPath & "\" & m_sCurrentBatchFileName & ".bat")
                            End If
                        End If

                    End If
                    End If
                Dim flagUserIsAnAdmin As Boolean = CurrentUserIsAdmin(Environment.MachineName)
                If rdLoggedOnSecurityOption.Checked = True Then
                    taskFolder.RegisterTaskDefinition(ProcessDescription,
                    tDefinition)
                    dtable.LoadDataRow(New String(4) {String.Empty, "rdLoggedOnSecurityOption", "1", "Integer", "True"}, True)
                Else



                    objLogin.Username = System.Security.Principal.WindowsIdentity.GetCurrent().Name
                    objLogin.Status = m_lStatus
                    objLogin.ShowDialog()


                    'With My.Settings
                    '    result = gPMConstants.PMEReturnCode.PMTrue
                    '    userId = .TaskUserName
                    '    userPassword = .TaskUserPassword
                    'End With
                    dtable.LoadDataRow(New String(4) {String.Empty, "rdLoggedOnOrNotSecurityOption", "1", "Integer", "True"}, True)

                    taskFolder.RegisterTaskDefinition(ProcessDescription, tDefinition, TaskCreation.CreateOrUpdate, objLogin.Username, objLogin.Password, TaskLogonType.Password, Nothing)

                End If


                If m_iTask = PMEComponentAction.PMEdit Then
                    UpdateBatchProcessesRecords(v_sProcessName:=Process, v_sJobDescription:=ProcessDescription, v_sFrequency:=m_sFrequencyType, v_sFrequencySubType:=m_sFrequencySubType, v_sFrequencyDescription:=m_sFrequencyDescription, v_batchSchedulerId:=BatchSchedulerId, v_datatable:=dtable, v_dtParameters:=ProcessParameters, v_sBatchFileName:=BatchFileName)
                Else
                    AddSchedulerRecord(v_sProcessName:=Process, v_sJobDescription:=ProcessDescription, v_sFrequency:=m_sFrequencyType, v_sFrequencySubType:=m_sFrequencySubType, v_sFrequencyDescription:=m_sFrequencyDescription, v_batchProcessId:=BatchProcessId, v_datatable:=dtable, v_dtParameters:=ProcessParameters, v_sBatchFileName:=BatchFileName)
                End If

                Me.Close()

                m_lStatus = gPMConstants.PMEReturnCode.PMOK
            End Using
            result = gPMConstants.PMEReturnCode.PMTrue
        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            If objLogin.Status <> gPMConstants.PMEReturnCode.PMCancel Then
                MessageBox.Show(ex.Message & ". The batch job could not be scheduled. Please contact the system administrator", "BatchJob Scheduler Configuration", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
            ' Log Error.
            '  iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Schedule Task Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=ex.Message, excep:=ex)
            bPMFunc.LogMessage(sUsername:=UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Schedule Task Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ScheduleTaskInWindowsTaskScheduler", vErrNo:=Information.Err().Number, vErrDesc:=ex.Message, excep:=ex)
            Return result

        End Try
        Return result
    End Function



    Private Function CurrentUserIsAdmin(ByVal computerName As String) As Boolean

        Dim principal As WindowsPrincipal = New WindowsPrincipal(WindowsIdentity.GetCurrent())
        Return principal.IsInRole(WindowsBuiltInRole.Administrator)
    End Function

    Private Function CreateBatchDirectory() As String
        If m_sbatchDirPath = "" Then
            gPMFunctions.GetPMRegSetting(gPMConstants.HKEY_LOCAL_MACHINE, 0, gPMConstants.PMERegSettingLevel.pmeRSLBase, "PMDIR", m_sbatchDirPath)
            m_sbatchDirPath &= "Pure\PureBatchFile"
            Dim di As New DirectoryInfo(m_sbatchDirPath)
            If Not di.Exists() Then
                Directory.CreateDirectory(m_sbatchDirPath)
            End If

        End If

        Return m_sbatchDirPath
    End Function

    Private Function GetTaskFolder(ByVal ts As TaskService) As TaskFolder
        Dim m_sfindTaskFolder As Boolean = False

        If ts.RootFolder.SubFolders.Count > 0 Then
            For icnt As Integer = 0 To ts.RootFolder.SubFolders.Count - 1
                If ts.RootFolder.SubFolders(icnt).Name = "PureTaskFolder" Then
                    m_sfindTaskFolder = True
                    m_taskfoldername = ts.RootFolder.SubFolders(icnt)
                    Exit For
                Else
                    m_sfindTaskFolder = False

                End If

            Next
            If m_sfindTaskFolder = False Then
                m_taskfoldername = ts.RootFolder.CreateFolder("PureTaskFolder")

            Else
                Return m_taskfoldername
            End If
        End If
        Return m_taskfoldername
    End Function




    Public Function AddSchedulerRecord(v_sProcessName, v_sJobDescription, v_sFrequency, v_sFrequencySubType, v_sFrequencyDescription, v_batchProcessId, v_datatable, v_dtParameters, v_sBatchFileName) As Boolean


        m_lReturn = m_oTaskScheduler.AddBatchScheduler(v_vProcessName:=v_sProcessName, v_vJobDescription:=v_sJobDescription, v_vFrequencyType:=v_sFrequency, v_vFrequencySubType:=v_sFrequencySubType, v_sFrequencyDescription:=v_sFrequencyDescription, v_ibatchProcessId:=v_batchProcessId, v_vdataTable:=dtable, v_dtParameters:=ProcessParameters, v_sBatchFileName:=BatchFileName)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("AddSchedulerRecord Record", "bSIRTaskScheduler.Interface Failed", gPMConstants.PMELogLevel.PMLogError)
        End If
        Return m_lReturn
    End Function

    Public Function UpdateBatchProcessesRecords(v_sProcessName, v_sJobDescription, v_sFrequency, v_sFrequencySubType, v_sFrequencyDescription, v_batchSchedulerId, v_datatable, v_dtParameters, v_sBatchFileName) As Boolean


        m_lReturn = m_oTaskScheduler.UpdateBatchProcessesRecords(v_vProcessName:=v_sProcessName, v_vJobDescription:=v_sJobDescription, v_vFrequencyType:=v_sFrequency, v_vFrequencySubType:=v_sFrequencySubType, v_sFrequencyDescription:=v_sFrequencyDescription, v_ibatchSchedulerId:=v_batchSchedulerId, v_vdataTable:=dtable, v_dtParameters:=ProcessParameters, v_sBatchFileName:=BatchFileName)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("UpdateBatchProcessesRecords Record", "bSIRTaskScheduler.Interface Failed", gPMConstants.PMELogLevel.PMLogError)
        End If
        Return m_lReturn
    End Function


    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "GetBusiness"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the details from the business object.

            lReturn = m_oBusiness.GetBatchProcess(v_vbatchprocesses_list_id:=BatchProcessId, m_vParameters:=m_vParameters)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oBusiness.GetBatchProcess", "Unable to find Scheduled batch")
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function

    Public Function GetMonth(month As String) As MonthsOfTheYear
        Select Case month
            Case "January"
                m_monthsOfTheYear = m_monthsOfTheYear Or MonthsOfTheYear.January
            Case "February"
                m_monthsOfTheYear = m_monthsOfTheYear Or MonthsOfTheYear.February
            Case "March"
                m_monthsOfTheYear = m_monthsOfTheYear Or MonthsOfTheYear.March
            Case "April"
                m_monthsOfTheYear = m_monthsOfTheYear Or MonthsOfTheYear.April
            Case "May"
                m_monthsOfTheYear = m_monthsOfTheYear Or MonthsOfTheYear.May
            Case "June"
                m_monthsOfTheYear = m_monthsOfTheYear Or MonthsOfTheYear.June
            Case "July"
                m_monthsOfTheYear = m_monthsOfTheYear Or MonthsOfTheYear.July
            Case "August"
                m_monthsOfTheYear = m_monthsOfTheYear Or MonthsOfTheYear.August
            Case "September"
                m_monthsOfTheYear = m_monthsOfTheYear Or MonthsOfTheYear.September
            Case "October"
                m_monthsOfTheYear = m_monthsOfTheYear Or MonthsOfTheYear.October
            Case "November"
                m_monthsOfTheYear = m_monthsOfTheYear Or MonthsOfTheYear.November
            Case "December"
                m_monthsOfTheYear = m_monthsOfTheYear Or MonthsOfTheYear.December
            Case Else
                m_monthsOfTheYear = MonthsOfTheYear.AllMonths
        End Select

        Return m_monthsOfTheYear
    End Function

    Public Function GetWeeklyMonthlyDays(day As Integer) As DaysOfTheWeek
        Select Case day
            Case 0
                m_daysOfWeek = m_daysOfWeek Or DaysOfTheWeek.Sunday
            Case 1
                m_daysOfWeek = m_daysOfWeek Or DaysOfTheWeek.Monday
            Case 2
                m_daysOfWeek = m_daysOfWeek Or DaysOfTheWeek.Tuesday
            Case 3
                m_daysOfWeek = m_daysOfWeek Or DaysOfTheWeek.Wednesday
            Case 4
                m_daysOfWeek = m_daysOfWeek Or DaysOfTheWeek.Thursday
            Case 5
                m_daysOfWeek = m_daysOfWeek Or DaysOfTheWeek.Friday
            Case 6
                m_daysOfWeek = m_daysOfWeek Or DaysOfTheWeek.Saturday

            Case Else
                m_daysOfWeek = DaysOfTheWeek.AllDays

        End Select
        Return m_daysOfWeek
    End Function
    Public Function GetMonthlyDaysOfWeek(day As String) As DaysOfTheWeek
        Select Case day
            Case "Sunday"
                m_monthlyDaysOfWeek = m_monthlyDaysOfWeek Or DaysOfTheWeek.Sunday
            Case "Monday"
                m_monthlyDaysOfWeek = m_monthlyDaysOfWeek Or DaysOfTheWeek.Monday
            Case "Tuesday"
                m_monthlyDaysOfWeek = m_monthlyDaysOfWeek Or DaysOfTheWeek.Tuesday
            Case "Wednesday"
                m_monthlyDaysOfWeek = m_monthlyDaysOfWeek Or DaysOfTheWeek.Wednesday
            Case "Thursday"
                m_monthlyDaysOfWeek = m_monthlyDaysOfWeek Or DaysOfTheWeek.Thursday
            Case "Friday"
                m_monthlyDaysOfWeek = m_monthlyDaysOfWeek Or DaysOfTheWeek.Friday
            Case "Saturday"
                m_monthlyDaysOfWeek = m_monthlyDaysOfWeek Or DaysOfTheWeek.Saturday

            Case Else
                m_monthlyDaysOfWeek = DaysOfTheWeek.AllDays

        End Select
        Return m_monthlyDaysOfWeek
    End Function



    Public Function GetDOWWhichWeek(weekNumber As String) As WhichWeek
        Select Case weekNumber
            Case "First"
                m_whichWeek = m_whichWeek Or WhichWeek.FirstWeek
            Case "Second"
                m_whichWeek = m_whichWeek Or WhichWeek.SecondWeek
            Case "Third"
                m_whichWeek = m_whichWeek Or WhichWeek.ThirdWeek
            Case "Fourth"
                m_whichWeek = m_whichWeek Or WhichWeek.FourthWeek
        End Select

        Return m_whichWeek
    End Function

    Private Sub CreateFrequencyParameter()

        dtable = New DataTable("Scheduler")
        dtable.Columns.AddRange(New DataColumn(4) {New DataColumn("FrequencyType", System.Type.GetType("System.String")),
                                                   New DataColumn("ParameterName", System.Type.GetType("System.String")),
                                                   New DataColumn("DefaultValue", System.Type.GetType("System.String")),
                                                   New DataColumn("DataType", System.Type.GetType("System.String")),
                                                   New DataColumn("CurrentValue", System.Type.GetType("System.String"))})


    End Sub
    Public Function CreateBatchFile() As Integer
        CreateBatchDirectory()
        ' IO.File.WriteAllText(Application.StartupPath() & "\" & BatchFileName & ".bat", BatchFileContentDetails.ToString())
        IO.File.WriteAllText(m_sbatchDirPath & "\" & BatchFileName & ".bat", BatchFileContentDetails.ToString())

    End Function

    Private Sub GroupBox1_Enter(sender As Object, e As EventArgs)

    End Sub

    Private Sub rdOneTimeFrequency_CheckedChanged(sender As Object, e As EventArgs) Handles rdOneTimeFrequency.CheckedChanged
        lblDailyRecurDays.Visible = False
        lblDays.Visible = False
        txtDaysDaily.Visible = False
        chkListBoxWeeklyDays.Visible = False
        lblRecurWeeksOn.Visible = False
        chkListBoxMonthlyWeekNumber.Visible = False
        chkListBoxMonthlyWeekDay.Visible = False
        checkedListBoxMonthlyDays.Visible = False
        checkedListBoxMonthlyMonths.Visible = False
        rdMonthsDays.Visible = False
        rdMonthDaysOfWeek.Visible = False
        lblMonths.Visible = False

    End Sub

    Private Sub rdDailyFrequency_CheckedChanged(sender As Object, e As EventArgs) Handles rdDailyFrequency.CheckedChanged
        lblDailyRecurDays.Visible = True
        lblDays.Visible = True
        txtDaysDaily.Visible = True
        chkListBoxWeeklyDays.Visible = False
        lblRecurWeeksOn.Visible = False
        chkListBoxMonthlyWeekNumber.Visible = False
        chkListBoxMonthlyWeekDay.Visible = False
        checkedListBoxMonthlyDays.Visible = False
        checkedListBoxMonthlyMonths.Visible = False
        rdMonthsDays.Visible = False
        rdMonthDaysOfWeek.Visible = False
        lblMonths.Visible = False
        txtDaysDaily.Value = 1

    End Sub

    Private Sub rdWeeklyFrequency_CheckedChanged(sender As Object, e As EventArgs) Handles rdWeeklyFrequency.CheckedChanged
        lblDailyRecurDays.Visible = True
        lblDays.Visible = False
        txtDaysDaily.Visible = True
        chkListBoxWeeklyDays.Visible = True
        lblRecurWeeksOn.Visible = True
        chkListBoxMonthlyWeekNumber.Visible = False
        chkListBoxMonthlyWeekDay.Visible = False
        checkedListBoxMonthlyDays.Visible = False
        checkedListBoxMonthlyMonths.Visible = False
        rdMonthsDays.Visible = False
        rdMonthDaysOfWeek.Visible = False
        lblMonths.Visible = False
        txtDaysDaily.Value = 1
    End Sub

    Private Sub rdMonthlyFrequency_CheckedChanged(sender As Object, e As EventArgs) Handles rdMonthlyFrequency.CheckedChanged

        lblRecurWeeksOn.Visible = False
        lblDailyRecurDays.Visible = False
        txtDaysDaily.Visible = False
        chkListBoxWeeklyDays.Visible = False
        chkListBoxMonthlyWeekNumber.Visible = True
        chkListBoxMonthlyWeekDay.Visible = True
        checkedListBoxMonthlyDays.Visible = True
        checkedListBoxMonthlyMonths.Visible = True
        rdMonthsDays.Visible = True
        rdMonthsDays.Checked = True
        rdMonthDaysOfWeek.Visible = True
        lblMonths.Visible = True
        chkListBoxMonthlyWeekNumber.Enabled = False
        chkListBoxMonthlyWeekDay.Enabled = False
        checkedListBoxMonthlyDays.Enabled = True

    End Sub

    Private Sub cmdOK_Click(sender As Object, e As EventArgs) Handles cmdOK.Click

        m_lStatus = gPMConstants.PMEReturnCode.PMOK

        ' m_iTask = gPMConstants.PMEComponentAction.PMAdd
        ' m_lReturn = GetBatchProcess(BatchProcessId)
        'm_oGeneral = New iSIRFrequencyScheduler.General()

        '' Call the initialise method passing this interface
        '' and the business object as parameters.
        'm_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)

        '' Check for errors.
        'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
        '    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
        '    Exit Sub
        'End If
        'm_lReturn = m_oGeneral.GetInterfaceDetails()
        'If m_lReturn Then

        'End If
        'Only do this if Adding
        'If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then

        '    m_lReturn = ValidateFormData()
        '    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
        '        'PN32241
        '        m_iTask = gPMConstants.PMEComponentAction.PMView
        '        Exit Sub
        '    End If

        '    '   m_iDocumenttypeID = cmbDocumentType.ItemId
        'End If
        m_lReturn = gPMConstants.PMEReturnCode.PMTrue
        Try

            CreateFrequencyParameter()

            ScheduleTaskInWindowsTaskScheduler()
            If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                Me.Hide()
            End If

        Catch excep As System.Exception



            m_lReturn = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ScheduleTask Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ScheduleTaskInWindowsTaskScheduler", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)



        End Try
    End Sub


    Public Function GetCurrentBatchProcess(ByRef r_vBatchProcessesDescription As String, ByRef r_vCurrentFileName As String, ByVal v_ibatchSchedulerId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetCurrentBatchProcess"
        Dim r_vResultArray(,) As Object = Nothing
        '  Dim m_sCurrentProcessDescription As String = String.Empty
        result = gPMConstants.PMEReturnCode.PMTrue

        m_vParameters = Nothing

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("Form_initalize", "Failed to create instance of Task Scheduler")
        End If

        m_lReturn = m_oTaskScheduler.GetCurrentBatchProcess(v_ibatchSchedulerId:=BatchSchedulerId, r_vBatchProcesses:=r_vResultArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "Failed to call bSIRTaskScheduler.GetCurrentBatchProcess")
        End If
        If Information.IsArray(r_vResultArray) Then
            ' Process all treaties
            For lCount As Integer = r_vResultArray.GetLowerBound(1) To r_vResultArray.GetUpperBound(1)

                r_vBatchProcessesDescription = CStr(r_vResultArray(2, lCount))
                r_vCurrentFileName = CStr(r_vResultArray(4, lCount))
            Next
        End If

        Return result
    End Function

    Private Function ValidateFormData() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Return result
    End Function

    Private Sub chkExpireDate_CheckedChanged(sender As Object, e As EventArgs) Handles chkExpireDate.CheckedChanged
        If (chkExpireDate.Checked) Then
            dateTimePickerExpireDate.Enabled = True
            dateTimePickerExpireTime.Enabled = True
        Else
            dateTimePickerExpireDate.Enabled = False
            dateTimePickerExpireTime.Enabled = False
        End If
    End Sub

    Private Sub rdMonthsDays_CheckedChanged(sender As Object, e As EventArgs) Handles rdMonthsDays.CheckedChanged
        chkListBoxMonthlyWeekNumber.Enabled = False
        chkListBoxMonthlyWeekDay.Enabled = False
        checkedListBoxMonthlyDays.Enabled = True

        For iRowCnt As Integer = 0 To chkListBoxMonthlyWeekNumber.Items.Count - 1
            chkListBoxMonthlyWeekNumber.SetItemChecked(iRowCnt, False)
        Next
        For iRowCnt As Integer = 0 To chkListBoxMonthlyWeekDay.Items.Count - 1
            chkListBoxMonthlyWeekDay.SetItemChecked(iRowCnt, False)
        Next
    End Sub

    Private Sub rdMonthDaysOfWeek_CheckedChanged(sender As Object, e As EventArgs) Handles rdMonthDaysOfWeek.CheckedChanged
        checkedListBoxMonthlyDays.Enabled = False
        chkListBoxMonthlyWeekNumber.Enabled = True
        chkListBoxMonthlyWeekDay.Enabled = True
        For iRowCnt As Integer = 0 To checkedListBoxMonthlyDays.Items.Count - 1
            checkedListBoxMonthlyDays.SetItemChecked(iRowCnt, False)
        Next

    End Sub

    Private Sub cmdCancel_Click(sender As Object, e As EventArgs) Handles cmdCancel.Click

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel
            If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                If MessageBox.Show("Are you sure you want to Cancel your changes?", "Cancel Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes Then

                    ' Everything OK, so we can hide the interface.
                    Me.Hide()

                End If
            Else
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
    End Sub

    Private Sub checkedListBoxMonthlyMonths_SelectedIndexChanged(sender As Object, e As EventArgs)
        'Dim m_sMonthValue As String
        'For i As Integer = 0 To checkedListBoxMonthlyMonths.Items.Count - 1
        '    m_sMonthValue = checkedListBoxMonthlyMonths.CheckedItems(i)
        '    If m_sMonthValue = "All" Then
        '        checkedListBoxMonthlyMonths.SetItemChecked(i, chkAll.Checked)
        '    End If

        'Next
    End Sub

    Private Sub checkedListBoxMonthlyMonths_KeyUp(sender As Object, e As KeyEventArgs)

    End Sub

    Private Sub checkedListBoxMonthlyMonths_ItemCheck(sender As Object, e As ItemCheckEventArgs) Handles checkedListBoxMonthlyMonths.ItemCheck
        If m_updateMonths Then Return

        If e.Index = 0 Then
            Dim newCheckedState As CheckState = e.NewValue
            m_updateMonths = True
            For idx As Integer = 1 To checkedListBoxMonthlyMonths.Items.Count - 1
                Me.checkedListBoxMonthlyMonths.SetItemCheckState(idx, newCheckedState)
            Next

        Else
            Dim checked As Boolean = e.NewValue = CheckState.Checked
            If Not checked Then
                m_updateMonths = True
                Me.checkedListBoxMonthlyMonths.SetItemCheckState(0, CheckState.Unchecked)
            End If
        End If
        m_updateMonths = False
    End Sub

    Private Sub chkListBoxMonthlyWeekDay_ItemCheck(sender As Object, e As ItemCheckEventArgs) Handles chkListBoxMonthlyWeekDay.ItemCheck
        If m_updateMonthlyWeekDays Then Return

        If e.Index = 0 Then
            Dim newCheckedState As CheckState = e.NewValue
            m_updateMonthlyWeekDays = True
            For idx As Integer = 1 To chkListBoxMonthlyWeekDay.Items.Count - 1
                Me.chkListBoxMonthlyWeekDay.SetItemCheckState(idx, newCheckedState)
            Next

        Else
            Dim checked As Boolean = e.NewValue = CheckState.Checked
            If Not checked Then
                m_updateMonthlyWeekDays = True
                Me.chkListBoxMonthlyWeekDay.SetItemCheckState(0, CheckState.Unchecked)
            End If
        End If
        m_updateMonthlyWeekDays = False


    End Sub
End Class
