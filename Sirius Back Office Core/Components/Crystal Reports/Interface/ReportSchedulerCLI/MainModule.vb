Option Strict On
Module MainModule

#Region " Constants "
    Const kiCurrencyId As Integer = 26
    Const kiUserID As Short = 1
    Const kScheduleReportPathOptionNo As Integer = 5078
    Public Const ACApp As String = "ReportSchedulerCLI"
    Private Const m_iDefaultLanguageID As Int32 = 1

    Public Const ACRptName_AgencyDebitingBordereau As String = "AGENCY\SUBAGENT_DEBITING_BORDEREAU_DETAILED"
    Public Const ACRptName_AgencyPaidBordereau As String = "AGENCY\SUBAGENT_PAID_BORDEREAU_DETAILED"
#End Region

#Region " Variables "
     'Objects
    Private m_oBusinessReportPrint As bSIRReportPrint.Business
     'Private m_oInterfaceDocTemplate As iPMBDocTemplate.Interface
    Private m_oBusinessDocAPI As bSIRDOCAPI.Form

    Private m_iSequence As Integer
    Private m_iCompanyId As Integer
    Private m_iSourceId As Integer
    Private m_sBranch As String
    Private m_iSubBranchId As Integer
    Private m_sFrequency As String
    Private m_sArchive As String
    Private m_sCreatePDF As String
    Private m_sCreateCSV As String
    Private m_vReportScheduler As Object
    Private m_vReportSchedulerDetail As Object
    Private m_sScheduleReportPath As String
    Private m_bSuccessfullyClose As Boolean
    Private m_sUserName As String
    Private m_iUserId As Integer
    Private m_sReportPath As String = String.Empty
#End Region

#Region " Properties "

    Public Property Branch() As String
        Get
            Return m_sBranch
        End Get
        Set(ByVal value As String)
            m_sBranch = value
        End Set
    End Property

    Public Property SourceId() As Integer
        Get
            Return m_iSourceId
        End Get
        Set(ByVal value As Integer)
            m_iSourceId = value
        End Set
    End Property

    Public Property Frequency() As String
        Get
            Return m_sFrequency
        End Get
        Set(ByVal value As String)
            m_sFrequency = value
        End Set
    End Property

    Public Property Archive() As String
        Get
            Return m_sArchive
        End Get
        Set(ByVal value As String)
            m_sArchive = value
        End Set
    End Property

    Public Property CreatePDF() As String
        Get
            Return m_sCreatePDF
        End Get
        Set(ByVal value As String)
            m_sCreatePDF = value
        End Set
    End Property

    Public Property CreateCSV() As String
        Get
            Return m_sCreateCSV
        End Get
        Set(ByVal value As String)
            m_sCreateCSV = value
        End Set
    End Property

    Public Property UserName() As String
        Get
            Return m_sUserName
        End Get
        Set(ByVal value As String)
            m_sUserName = value
        End Set
    End Property
#End Region

#Region " Main "

    Sub Main(ByVal args() As String)

        Const kReporSchedulerId As Byte = 0
        Const kReportName As Byte = 2
        Const kExportPDF As Byte = 4
        Const kArchievePDF As Byte = 5
        Const kExportCSV As Byte = 6
        Const kReportPath As Byte = 7
        Const kSeprateBy As Byte = 8
        Const kCols As Integer = 6
        Const ACAgentShortCode As String = "AgentShortCode"
        Const ACSubAgentShortCode As String = "SubAgentCode"

        Dim bReturn As Boolean
        Dim lResult As Long
        Dim bResult As Boolean
        Dim iCount As Integer
        Dim oAllReports(,) As Object
        Dim v_ireport_scheduler_id As Integer
        Dim iExportPDF As Integer
        Dim iArchievePDF As Integer
        Dim iExportCSV As Integer
        Dim iCountDetail As Integer
        Dim oAllReportDetail As Object(,)
        Dim oAllReportDetailNew As Object(,)
        Dim sReportName As String
        Dim irowsfound As Integer
        Dim sReportPath As String
        Dim dtStartDate As DateTime
        Dim dtEndDate As DateTime
        Dim bAutomatic As Boolean
        Dim sSeprateBy As String

        Try

            If My.Application.CommandLineArgs.Count = 0 Then
                Console.WriteLine("Unable to Run Report Scheduler ! Please pass Frequency parameter")
                Exit Sub
            End If

            bReturn = OutputBadParameterPassed()
            If Not bReturn Then
                Console.WriteLine("Unable to Run Report Scheduler : As Unrecognised Bad Parameter Passed")
                Exit Sub
            End If

            bReturn = ProcessArguments()
            If Not bReturn Then
                 'OutputSyntax()
                Exit Sub
            Else
                 'Instantiate Components
                 'bResult = InstantiateComponents()
                 'If bResult = False Then
                 '    Console.WriteLine("Unable To Run Period End Process : Business Component Initialise")
                 '    Exit Sub
                 'End If

                lResult = GetSchedulerReports()
                If lResult <> PMEReturnCode.PMTrue Then
                    Throw New Exception("Unable to fetch Scheduler Reports Detail")
                End If

                bReturn = GetScheduleReportPath()
                If Not bReturn Then
                    Console.WriteLine("Report Scheduler path not set : System Option")
                    Exit Sub
                End If

                If m_sScheduleReportPath Is "" Then
                    Console.WriteLine("Report Scheduler path not set : System Option")
                    Exit Sub
                End If

                oAllReports = DirectCast(m_vReportScheduler, Object(,))

                Console.WriteLine("Report Scheduler Export : Start")

                 'Don't throw exception under some circumstances -- and proceed ahead instead
                For iCount = 0 To UBound(oAllReports, 2)
                    m_oBusinessReportPrint = Nothing
                    m_oBusinessDocAPI = Nothing
                     'Instantiate Components
                    bResult = InstantiateComponents()
                    If bResult = False Then
                        Console.WriteLine("Unable To Run Period End Process : Business Component Initialise")
                        Exit Sub
                    End If

                    v_ireport_scheduler_id = CInt(oAllReports(kReporSchedulerId, iCount))
                    iExportPDF = CInt(oAllReports(kExportPDF, iCount))
                    iArchievePDF = CInt(oAllReports(kArchievePDF, iCount))
                    iExportCSV = CInt(oAllReports(kExportCSV, iCount))
                    sReportName = CStr(oAllReports(kReportName, iCount))
                    sReportPath = CStr(oAllReports(kReportPath, iCount))
                    m_sReportPath = sReportPath
                    sSeprateBy = Convert.ToString(oAllReports(kSeprateBy, iCount))
                    bAutomatic = False

                    lResult = GetSchedulerReportDetail(v_ireport_scheduler_id:=v_ireport_scheduler_id)
                    If lResult <> PMEReturnCode.PMTrue Then
                        Throw New Exception("Unable to fetch Report Scheduler Detail")
                    End If

                    oAllReportDetail = DirectCast(m_vReportSchedulerDetail, Object(,))
                    irowsfound = UBound(oAllReportDetail, 2)
                    ReDim oAllReportDetailNew(irowsfound, kCols)
                    For iCountDetail = 0 To UBound(oAllReportDetail, 2)
                        oAllReportDetailNew(iCountDetail, 0) = oAllReportDetail(2, iCountDetail)
                         'Start Date Increment
                        If oAllReportDetail(2, iCountDetail).ToString.ToUpper = "START_DATE" Then
                            dtStartDate = CDate(oAllReportDetail(3, iCountDetail))
                            If CInt(oAllReportDetail(9, iCountDetail)) = 1 Then
                                bAutomatic = True
                                Select Case Frequency
                                    Case "DAILY"
                                        dtStartDate = DateAdd(DateInterval.Day, 1, dtStartDate)
                                    Case "MONTHLY"
                                        dtStartDate = DateAdd(DateInterval.Month, 1, dtStartDate)
                                    Case "ANNUALLY"
                                        dtStartDate = DateAdd(DateInterval.Year, 1, dtStartDate)
                                End Select
                            End If
                        End If
                         'End Date Increment   
                        If oAllReportDetail(2, iCountDetail).ToString.ToUpper = "END_DATE" Then
                            dtEndDate = CDate(oAllReportDetail(3, iCountDetail))
                            If CInt(oAllReportDetail(9, iCountDetail)) = 1 Then
                                bAutomatic = True
                                Select Case Frequency
                                    Case "DAILY"
                                        dtEndDate = DateAdd(DateInterval.Day, 1, dtEndDate)
                                    Case "MONTHLY"
                                        dtEndDate = DateAdd(DateInterval.Month, 1, dtEndDate)
                                    Case "ANNUALLY"
                                        dtEndDate = DateAdd(DateInterval.Year, 1, dtEndDate)
                                End Select
                            End If
                        End If
                        oAllReportDetailNew(iCountDetail, 1) = oAllReportDetail(3, iCountDetail)
                        oAllReportDetailNew(iCountDetail, 2) = oAllReportDetail(4, iCountDetail)
                        oAllReportDetailNew(iCountDetail, 3) = oAllReportDetail(5, iCountDetail)
                        oAllReportDetailNew(iCountDetail, 4) = oAllReportDetail(6, iCountDetail)
                        oAllReportDetailNew(iCountDetail, 5) = oAllReportDetail(7, iCountDetail)
                        oAllReportDetailNew(iCountDetail, 6) = oAllReportDetail(8, iCountDetail)
                    Next
                    Dim sAgentShortName As String = ""
                    Dim sSubAgentShortName As String = ""
                     'Here checking is Separated by has some value.if has something then execute code inside
                     'If block otherwise else block code.
                    If Not String.IsNullOrEmpty(sSeprateBy) Then
                        Select Case sSeprateBy
                             'For Agent.
                            Case ACAgentShortCode
                                Dim oResults As Object = Nothing
                                GetAgentCodesfromReports(oAllReportDetailNew, oResults)
                                If oResults IsNot Nothing Then
                                    Dim oResultAgentCodes As Object(,) = DirectCast(oResults, Object(,))
                                    For iRecordCount As Integer = 0 To UBound(oResultAgentCodes, 2)
                                        If sAgentShortName <> oResultAgentCodes(13, iRecordCount).ToString Then
                                            sAgentShortName = oResultAgentCodes(13, iRecordCount).ToString
                                            oAllReportDetailNew(2, 1) = sAgentShortName
                                             'Call This method to create PDF, Archieve And Excel file.
                                            ExecuteReport(iExportPDF, sReportPath, lResult, oAllReportDetailNew, sReportName, sAgentShortName, iArchievePDF, iExportCSV, bAutomatic, dtStartDate, dtEndDate, v_ireport_scheduler_id)
                                        End If
                                    Next
                                End If
                                 'For Sub Agent.
                            Case ACSubAgentShortCode

                                Dim oResults As Object = Nothing
                                GetSubAgentCodesfromReports(oAllReportDetailNew, oResults)
                                If oResults IsNot Nothing Then
                                    Dim oResultAgentCodes As Object(,) = DirectCast(oResults, Object(,))
                                    For iRecordCount As Integer = 0 To UBound(oResultAgentCodes, 2)
                                        If sSubAgentShortName <> oResultAgentCodes(13, iRecordCount).ToString Then
                                            sSubAgentShortName = oResultAgentCodes(13, iRecordCount).ToString
                                            oAllReportDetailNew(0, 1) = sSubAgentShortName
                                             'Call This method to create PDF, Archieve And Excel file.
                                            ExecuteReport(iExportPDF, sReportPath, lResult, oAllReportDetailNew, sReportName, sSubAgentShortName, iArchievePDF, iExportCSV, bAutomatic, dtStartDate, dtEndDate, v_ireport_scheduler_id)
                                        End If
                                    Next
                                End If
                        End Select
                         'If Separated by option has not any value.
                    Else
                         'Call This method to create PDF, Archieve And Excel file.
                        ExecuteReport(iExportPDF, sReportPath, lResult, oAllReportDetailNew, sReportName, sAgentShortName, iArchievePDF, iExportCSV, bAutomatic, dtStartDate, dtEndDate, v_ireport_scheduler_id)
                    End If
                Next

            End If
            m_bSuccessfullyClose = True
            Console.WriteLine("Report Scheduler Export : End")

        Catch ex As Exception
            Console.WriteLine("Report Print Failed - " & ex.Message)
            m_bSuccessfullyClose = False
        Finally
            If m_oBusinessReportPrint IsNot Nothing Then
                m_oBusinessReportPrint.Dispose()
            End If
        End Try
    End Sub

#End Region

#Region " Functions "

    Private Function ProcessArguments() As Boolean

        Dim iItem As Integer = 0
        Dim sArg As String
        Dim sArgValues() As String

        Try
            ProcessArguments = True

            For iItem = 0 To My.Application.CommandLineArgs.Count - 1
                sArg = My.Application.CommandLineArgs.Item(iItem)
                sArgValues = sArg.Split(CChar("="))
                Select Case sArgValues(0).ToUpper
                    Case "FREQUENCY"
                        If String.IsNullOrEmpty(sArgValues(1)) Then
                            ProcessArguments = False
                            Console.WriteLine("Invalid argument for Frequcny - " & sArgValues(1))
                        Else
                            Frequency = sArgValues(1).ToString.ToUpper
                        End If
                    Case "ARCHIVE"
                        Archive = sArgValues(1).ToString.ToUpper
                    Case "CREATEPDF"
                        CreatePDF = sArgValues(1).ToString.ToUpper
                    Case "CREATECSV"
                        CreateCSV = sArgValues(1).ToString.ToUpper
                    Case "BRANCH"
                        Branch = sArgValues(1).ToString.ToCharArray
                    Case "USERNAME"
                        UserName = sArgValues(1).ToString.ToCharArray
                End Select
            Next

            If Frequency = "DAILY" Or Frequency = "MONTHLY" Or Frequency = "ANNUALLY" Then
                'Do Nothing
            Else
                Console.WriteLine("Invalid Argument passed, Should be either of Annually/Monthly/Daily")
                ProcessArguments = False
            End If

            If Archive = "TRUE" Then
                'Do Nothing
            Else
                Archive = "FALSE" 'Default
            End If

            If CreatePDF = "TRUE" Then
                'Do Nothing
            Else
                CreatePDF = "FALSE" 'Default
            End If

            If CreateCSV = "TRUE" Then
                'Do Nothing
            Else
                CreateCSV = "FALSE" 'Default
            End If

            ''Validation
            'If CreatePDF = "FALSE" And CreateCSV = "FALSE" Then
            '    Console.WriteLine("One of CreatePDF Or CreateCSV must be True")
            '    ProcessArguments = False
            'End If

        Catch ex As Exception
            Console.WriteLine("ProcessArguments Failed - " + ex.Message)
            Throw New Exception("ProcessArguments Failed - " + ex.Message)
        End Try

    End Function

    Private Function GetSchedulerReports() As Integer

        Dim oDatabase As dPMDAO.Database = Nothing 
        Dim vResults As Object = Nothing
        Dim iReturnCode As Integer 
        Dim sFrequency As String 

        Try
            GetSchedulerReports = 1
            sFrequency = Frequency
            DBConnect(oDatabase)
            AddParameterLite(oDatabase, "frequency", sFrequency, PMEParameterDirection.PMParamInput, PMEDataType.PMString, True)

            iReturnCode = oDatabase.SQLSelect("spu_sir_scheduled_reports_sel", "spu_sir_scheduled_reports_sel", True, vResultArray:=vResults)

            If IsNothing(vResults) Or Not IsArray(vResults) Then
                Console.WriteLine("Report Scheduler Details not found")
                GetSchedulerReports = 0
            Else
                m_vReportScheduler = vResults
            End If

            DBDisconnect(oDatabase)
            oDatabase = Nothing

        Catch ex As Exception
            System.Console.WriteLine("GetSchedulerReports Failed - " + ex.Message)
            Throw ex
        End Try

    End Function

    Public Function GetSystemOption(ByVal v_iOptionNumber As Integer) As String
        Dim lResult As Integer = 0
        Dim oSystemOptions As bSIROptions.Business = Nothing
        Dim sOptionValue As String = String.Empty

        Try

             ' Create the System Options Object
            oSystemOptions = New bSIROptions.Business
            If (oSystemOptions Is Nothing) Then
                Throw New Exception("Unable to create bSIROptions.Business")
            End If

             ' Initialise
            lResult = CInt(oSystemOptions.Initialise(sUsername:="", sPassword:="", iUserID:=0, iSourceID:=1, iLanguageID:=1, iCurrencyID:=26, iLogLevel:=CShort(PMELogLevel.PMLogError), sCallingAppName:=ACApp))
            If lResult <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to initialise bSIROptions.Business")
            End If

             ' Get the system option
            lResult = oSystemOptions.GetOption( _
                iOptionNumber:=CShort(v_iOptionNumber), _
                sValue:=sOptionValue, _
                v_iSourceID:=1)
            If lResult <> PMEReturnCode.PMTrue Then
                Throw New Exception(String.Format("Unable to retrieve system option '{0}'", v_iOptionNumber))
            End If

             ' Return the option value
            Return sOptionValue

        Catch ex As Exception
            Throw New Exception("Unable to retrieve system option", ex)

        Finally
            If Not oSystemOptions Is Nothing Then
                oSystemOptions.Dispose()
            End If
            oSystemOptions = Nothing
        End Try
    End Function

    Private Function OutputBadParameterPassed() As Boolean

        Dim iItem As Integer = 0
        Dim sArg As String
        Dim sArgValues() As String

        Try
            OutputBadParameterPassed = True

            If My.Application.CommandLineArgs.Count > 0 Then
                For iItem = 0 To My.Application.CommandLineArgs.Count - 1
                    sArg = My.Application.CommandLineArgs.Item(iItem)
                    sArgValues = sArg.Split(CChar("="))
                    If sArgValues(0).ToUpper = "FREQUENCY" Or _
                            sArgValues(0).ToUpper = "ARCHIVE" Or _
                            sArgValues(0).ToUpper = "CREATEPDF" Or _
                            sArgValues(0).ToUpper = "CREATECSV" Or _
                            sArgValues(0).ToUpper = "BRANCH" Or _
                            sArgValues(0).ToUpper = "USERNAME" Then
                        'Do Nothing
                    Else
                        OutputBadParameterPassed = False
                        Exit Function
                    End If
                Next
            End If

        Catch ex As Exception
            Console.WriteLine("OutputBadParameterPassed Failed - " + ex.Message)
            Throw New Exception("OutputBadParameterPassed Failed - " + ex.Message)
        Finally
        End Try
    End Function

    Private Function InstantiateComponents() As Boolean

        Dim lResult As Long

        Try
            InstantiateComponents = True

            m_oBusinessReportPrint = New bSIRReportPrint.Business
            'm_oInterfaceDocTemplate = New iPMBDocTemplate.interface
            m_oBusinessDocAPI = New bSIRDOCAPI.Form

            Dim vResults As Object = Nothing
            Dim iUserId As Integer
            Dim sUserName As String
            If Not String.IsNullOrEmpty(UserName) Then
                getUserIDFromCode(UserName, vResults)
                Dim vUserIds As Object(,)
                vUserIds = DirectCast(vResults, Object(,))
                iUserId = CInt(vUserIds(0, 0))
                sUserName = UserName
            Else
                iUserId = kiUserID
                sUserName = "sirius"
            End If

            lResult = m_oBusinessReportPrint.Initialise(sUsername:=sUserName, _
                sPassword:="sirius", _
                iUserID:=iUserId, _
                iSourceID:=1, _
                iLanguageID:=m_iDefaultLanguageID, _
                iCurrencyID:=kiCurrencyId, _
                iLogLevel:=Convert.ToInt16(PMELogLevel.PMLogError), _
                sCallingAppName:=ACApp)
            If lResult <> PMEReturnCode.PMTrue Then
                InstantiateComponents = False
                Throw New Exception("Unable to initialise bSIRReportPrint.Business")
            End If

            lResult = m_oBusinessDocAPI.Initialise(sUsername:=sUserName, _
                sPassword:="sirius", _
                iUserID:=iUserId, _
                iSourceID:=1, _
                iLanguageID:=m_iDefaultLanguageID, _
                iCurrencyID:=kiCurrencyId, _
                iLogLevel:=Convert.ToInt16(PMELogLevel.PMLogError), _
                sCallingAppName:=ACApp)
            If lResult <> PMEReturnCode.PMTrue Then
                InstantiateComponents = False
                Throw New Exception("Unable to initialise bSIRReportPrint.Business")
            End If

        Catch ex As Exception
            Console.WriteLine("InstantiateComponents Failed - " + ex.Message)
            Throw New Exception("InstantiateComponents Failed - " + ex.Message)
            InstantiateComponents = False
        End Try
    End Function

    Private Function GetScheduleReportPath() As Boolean

        Dim sScheduleReportPathOptionVal As String

        Try
            GetScheduleReportPath = True
            sScheduleReportPathOptionVal = GetSystemOption(v_iOptionNumber:=kScheduleReportPathOptionNo)

            If CStr(sScheduleReportPathOptionVal) <> "" Then
                m_sScheduleReportPath = CStr(sScheduleReportPathOptionVal)
            Else
                m_sScheduleReportPath = ""
                GetScheduleReportPath = False
            End If

        Catch ex As Exception
            System.Console.WriteLine("GetScheduleReportPath Failed - " + ex.Message)
            Throw ex
        End Try

    End Function

    Private Function GetSchedulerReportDetail(ByVal v_ireport_scheduler_id As Integer) As Long

        Dim oDatabase As dPMDAO.Database = Nothing
        Dim vResults As Object = Nothing
        Dim iReturnCode As Integer

        Try
            GetSchedulerReportDetail = 1
            DBConnect(oDatabase)
            AddParameterLite(oDatabase, "report_scheduler_id", v_ireport_scheduler_id, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, True)

            iReturnCode = oDatabase.SQLSelect("spu_sir_scheduled_reports_detail_sel", "spu_sir_scheduled_reports_detail_sel", True, vResultArray:=vResults)

            If IsNothing(vResults) Then
                GetSchedulerReportDetail = 0
            Else
                m_vReportSchedulerDetail = vResults
            End If

            DBDisconnect(oDatabase)
            oDatabase = Nothing

        Catch ex As Exception
            System.Console.WriteLine("GetSchedulerReportDetail Failed - " + ex.Message)
            Throw ex
        End Try

    End Function

    Private Function IncrementDatesAutomatic(ByVal v_dtStartDate As DateTime, ByVal v_dtEndDate As DateTime, _
                                                        ByVal v_ireport_scheduler_id As Integer) As Long

        Dim oDatabase As dPMDAO.Database = Nothing
        Dim vResults As Object = Nothing
        Dim iReturnCode As Integer

        Try
            IncrementDatesAutomatic = 1
            DBConnect(oDatabase)
            AddParameterLite(oDatabase, "report_scheduler_id", v_ireport_scheduler_id, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, True)
            AddParameterLite(oDatabase, "start_date", v_dtStartDate.ToString(), PMEParameterDirection.PMParamInput, PMEDataType.PMString, False)
            AddParameterLite(oDatabase, "end_date", v_dtEndDate.ToString(), PMEParameterDirection.PMParamInput, PMEDataType.PMString, False)

            iReturnCode = oDatabase.SQLSelect("spu_sir_scheduled_reports_automatic_dates_update", "spu_sir_scheduled_reports_automatic_dates_update", True)
            IncrementDatesAutomatic = iReturnCode

            DBDisconnect(oDatabase)
            oDatabase = Nothing

        Catch ex As Exception
            System.Console.WriteLine("IncrementDatesAutomatic Failed - " + ex.Message)
            Throw ex
        End Try

    End Function

    'Get Agent code from report based on below given different parameters.
    Private Function GetAgentCodesfromReports(ByVal vParameterArray As Object(,), ByRef v_oResult As Object) As Integer

        Dim oDatabase As dPMDAO.Database = Nothing
        Dim vResults As Object = Nothing
        Dim iReturnCode As Integer
        Dim sFrequency As String

        Try
            GetAgentCodesfromReports = 1
            sFrequency = Frequency
            DBConnect(oDatabase)
            If String.IsNullOrEmpty(vParameterArray(1, 1).ToString) Then
                AddParameterLite(oDatabase, "branch_id", 0, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)
            Else
                AddParameterLite(oDatabase, "branch_id", vParameterArray(1, 1), PMEParameterDirection.PMParamInput, PMEDataType.PMString, True)
            End If
            If vParameterArray(12, 1).ToString = "<ALL>" Then
                AddParameterLite(oDatabase, "AgentShortName", "ALL", PMEParameterDirection.PMParamInput, PMEDataType.PMString, False)
            Else
                AddParameterLite(oDatabase, "AgentShortName", vParameterArray(2, 1), PMEParameterDirection.PMParamInput, PMEDataType.PMString, False)
            End If
            AddParameterLite(oDatabase, "start_Date", vParameterArray(3, 1), PMEParameterDirection.PMParamInput, PMEDataType.PMDate, False)
            If m_sReportPath.ToUpper() <> ACRptName_AgencyDebitingBordereau And m_sReportPath.ToUpper() <> ACRptName_AgencyPaidBordereau Then
                AddParameterLite(oDatabase, "End_date", CDate(CStr(vParameterArray(4, 1)) & " 23:59:59"), PMEParameterDirection.PMParamInput, PMEDataType.PMDate, False)
            Else
                AddParameterLite(oDatabase, "End_date", vParameterArray(4, 1), PMEParameterDirection.PMParamInput, PMEDataType.PMDate, False)
            End If
            AddParameterLite(oDatabase, "Basis", vParameterArray(5, 1), PMEParameterDirection.PMParamInput, PMEDataType.PMString, False)
            If vParameterArray(6, 1).ToString = "<ALL>" Then
                AddParameterLite(oDatabase, "Underwriting_Year", "ALL", PMEParameterDirection.PMParamInput, PMEDataType.PMString, False)
            Else
                AddParameterLite(oDatabase, "Underwriting_Year", vParameterArray(6, 1), PMEParameterDirection.PMParamInput, PMEDataType.PMString, False)
            End If
            AddParameterLite(oDatabase, "TypeOfCurrency", vParameterArray(7, 1), PMEParameterDirection.PMParamInput, PMEDataType.PMString, False)
            AddParameterLite(oDatabase, "GroupByCode", vParameterArray(8, 1), PMEParameterDirection.PMParamInput, PMEDataType.PMString, False)
            AddParameterLite(oDatabase, "IncludeBalanceAccount", vParameterArray(9, 1), PMEParameterDirection.PMParamInput, PMEDataType.PMString, False)
            AddParameterLite(oDatabase, "TransactionType", vParameterArray(10, 1), PMEParameterDirection.PMParamInput, PMEDataType.PMString, False)
            If vParameterArray(12, 1).ToString = "<ALL>" Then
                AddParameterLite(oDatabase, "AgentGroupCode", "ALL", PMEParameterDirection.PMParamInput, PMEDataType.PMString, False)
            Else
                AddParameterLite(oDatabase, "AgentGroupCode", vParameterArray(12, 1), PMEParameterDirection.PMParamInput, PMEDataType.PMString, False)
            End If

            iReturnCode = oDatabase.SQLSelect("spu_Report_Agent_Statemt_SFU", "spu_Report_Agent_Statemt_SFU", True, vResultArray:=vResults)

            If IsNothing(vResults) Or Not IsArray(vResults) Then
                Console.WriteLine("Report Scheduler Details not found")
                GetAgentCodesfromReports = 0
            Else
                v_oResult = vResults
            End If

            DBDisconnect(oDatabase)
            oDatabase = Nothing

        Catch ex As Exception
            System.Console.WriteLine("GetAgentCodesfromReports Failed - " + ex.Message)
            Throw ex
        End Try

    End Function

    'Get Sub Agent code from report based on below given different parameters.
    Private Function GetSubAgentCodesfromReports(ByVal vParameterArray As Object(,), ByRef v_oResult As Object) As Integer

        Dim oDatabase As dPMDAO.Database = Nothing
        Dim vResults As Object = Nothing
        Dim iReturnCode As Integer
        Dim sFrequency As String

        Try
            GetSubAgentCodesfromReports = 1
            sFrequency = Frequency
            DBConnect(oDatabase)
            If vParameterArray(0, 1).ToString = "<ALL>" Then
                AddParameterLite(oDatabase, "SubAgentShortName", "ALL", PMEParameterDirection.PMParamInput, PMEDataType.PMString, True)
            Else
                AddParameterLite(oDatabase, "SubAgentShortName", vParameterArray(0, 1), PMEParameterDirection.PMParamInput, PMEDataType.PMString, True)
            End If
            If vParameterArray(1, 1).ToString = "<ALL>" Then
                AddParameterLite(oDatabase, "Underwriting_Year", "ALL", PMEParameterDirection.PMParamInput, PMEDataType.PMString, False)
            Else
                AddParameterLite(oDatabase, "Underwriting_Year", vParameterArray(1, 1), PMEParameterDirection.PMParamInput, PMEDataType.PMString, False)
            End If
            AddParameterLite(oDatabase, "start_Date", vParameterArray(2, 1), PMEParameterDirection.PMParamInput, PMEDataType.PMDate, False)
            AddParameterLite(oDatabase, "End_date", vParameterArray(3, 1), PMEParameterDirection.PMParamInput, PMEDataType.PMDate, False)
            AddParameterLite(oDatabase, "TypeOfCurrency", vParameterArray(4, 1), PMEParameterDirection.PMParamInput, PMEDataType.PMString, False)
            AddParameterLite(oDatabase, "GroupBy", vParameterArray(5, 1), PMEParameterDirection.PMParamInput, PMEDataType.PMString, False)
            If vParameterArray(6, 1).ToString = "<ALL>" Then
                AddParameterLite(oDatabase, "AgentGroupCode", "ALL", PMEParameterDirection.PMParamInput, PMEDataType.PMString, False)
            Else
                AddParameterLite(oDatabase, "AgentGroupCode", vParameterArray(6, 1), PMEParameterDirection.PMParamInput, PMEDataType.PMString, False)
            End If

            iReturnCode = oDatabase.SQLSelect("spu_Report_SubAgent_Statemt_SFU", "spu_Report_SubAgent_Statemt_SFU", True, vResultArray:=vResults)

            If IsNothing(vResults) Or Not IsArray(vResults) Then
                Console.WriteLine("GetSubAgentCodesfromReports not found")
                GetSubAgentCodesfromReports = 0
            Else
                v_oResult = vResults
            End If

            DBDisconnect(oDatabase)
            oDatabase = Nothing

        Catch ex As Exception
            System.Console.WriteLine("GetSubAgentCodesfromReports Failed - " + ex.Message)
            Throw ex
        End Try

    End Function
    'This method is used to create PDF, Archieve And Excel files.
    Private Function ExecuteReport(ByVal iExportPDF As Integer, ByVal sReportPath As String, ByVal lResult As Long, ByRef oAllReportDetailNew As Object(,), ByVal sReportName As String, ByVal sSeparatedByName As String, ByVal iArchievePDF As Integer, ByVal iExportCSV As Integer, ByVal bAutomatic As Boolean, ByVal dtStartDate As DateTime, ByVal dtEndDate As DateTime, ByVal v_ireport_scheduler_id As Integer) As Long
        Dim v_iFormatType As Integer
        Dim aRepName As Array
        Dim iRepName As Integer
        Dim sRepName As String
        Dim vDefaultValues As Object = Nothing
        Dim sTempReportPath1 As String
        Dim sTempReportPath2 As String
        Dim sCopyReportPathName As String
        Dim sKeywords() As String = Nothing
        m_oBusinessReportPrint.reportName = sReportPath
        m_oBusinessReportPrint.CalledFromCLI = True
        m_oBusinessReportPrint.PrintReport = 4 'AC_EXPORT_TO_PDF    
        m_oBusinessReportPrint.ScheduleReportPath = m_sScheduleReportPath
        aRepName = Split(sReportPath, "\")
        iRepName = aRepName.GetUpperBound(0)
        sRepName = aRepName.GetValue(iRepName).ToString
        m_oBusinessReportPrint.ScheduleReportName = sRepName
        sTempReportPath1 = m_sScheduleReportPath
        sTempReportPath2 = m_sScheduleReportPath
        If iExportPDF = 1 Then  'Export to PDF
            v_iFormatType = 3   'For HTML

            'This code add broker name as prefix to report file if separated By option has some value.
            If Not String.IsNullOrEmpty(sSeparatedByName) Then
                sRepName = sSeparatedByName + "-" + sRepName
            End If
            lResult = m_oBusinessReportPrint.GetParameters( _
                            r_vParameters:=CObj(oAllReportDetailNew), _
                            r_vDefaultValues:=vDefaultValues)
            If lResult <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to Get Parameters")
                Console.WriteLine("GetParameters failed - " + sReportName)
            End If

            'This method is used to Created PDF file in sTempReportPath1 location.
            lResult = m_oBusinessReportPrint.SendToPrint( _
                                    v_sReportTitle:=sRepName, _
                                    r_sCompiledReportPath:=sTempReportPath1, _
                                    v_vParameters:=oAllReportDetailNew)
            If lResult <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to Export file")
                Console.WriteLine("SendToPrint failed - " + sReportName)
            End If
            Console.WriteLine("Report - " + sReportName + " Exported")

        End If

        'Excel Conversion
        If iExportCSV = 1 Then
            lResult = m_oBusinessReportPrint.GetParameters( _
                                        r_vParameters:=CObj(oAllReportDetailNew), _
                                        r_vDefaultValues:=vDefaultValues)
            If lResult <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to Get Parameters")
                Console.WriteLine("GetParameters failed - " + sReportName)
            End If

            v_iFormatType = 200   'For CSV
            'This method is used to Created Excel file.
            lResult = m_oBusinessReportPrint.ExportToDisk(r_ExportFile:=sTempReportPath1, _
                                    v_iFormatType:=CShort(v_iFormatType), _
                                    v_vParameters:=oAllReportDetailNew, _
                                    v_sExt:=".xls", v_sSeparatedByName:=sSeparatedByName)
            If lResult <> PMEReturnCode.PMTrue Then
                'Throw New Exception("Unable to fetch Report Scheduler Detail")
                Console.WriteLine("Export To Disk failed : Report Name - " + sReportName)
            End If
            Console.WriteLine("Export To Disk as Excel file : Report Name - " + sReportName)
        End If

        'Archive Document
        If iArchievePDF = 1 Then
            Dim sDestinationPath As String
            Dim sOutputFormat As String = String.Empty
            If iExportPDF = 1 Then
                sOutputFormat = "PDF"
            ElseIf iExportCSV = 1 Then
                sOutputFormat = "xls"
            End If
            Dim vResults As Object = Nothing
            If String.IsNullOrEmpty(UserName) Then
                m_iUserId = 1
            Else
                getUserIDFromCode(UserName, vResults)
                Dim vUserIds As Object(,)
                vUserIds = DirectCast(vResults, Object(,))
                m_iUserId = CInt(vUserIds(0, 0))
            End If

            Dim sOptionVal As String
            sOptionVal = GetSystemOption(v_iOptionNumber:=10)
            'check system option for sharepoint configure {2} for sharepoint
            If sOptionVal = "2" Then
                If String.IsNullOrEmpty(Branch) Then
                    sDestinationPath = "HeadOff/General/Reports/" & Format(Date.Today, "dd-MM-yyyy") & "/" & System.IO.Path.GetFileName(sTempReportPath1)
                Else
                    sDestinationPath = Branch.Trim & "/General/Reports/" & Format(Date.Today, "dd-MM-yyyy") & "/" & System.IO.Path.GetFileName(sTempReportPath1)
                End If
                'set up the initial XML for the job
                Dim xlJob As String = "<BACKGROUND_JOB> <JOB jobtype=""DOCUPACK"">    " & _
                           "<PARAMETERS> " & _
                               "<PARAMETER name=""destination"" value=""archive"" />    " & _
                               "<PARAMETER name=""archive"" value=""true"" />    " & _
                               "<PARAMETER name=""Path"" value=""" & sTempReportPath1 & """ />    " & _
                               "<PARAMETER name=""OutputFormat"" value=""" & sOutputFormat & """ />    " & _
                               "<PARAMETER name=""DocumentTemplateGroupID"" value=""0"" />    " & _
                               "<PARAMETER name=""DocumentTemplateSubGroupID"" value=""0"" />    " & _
                               "<PARAMETER name=""DestinationFilename"" value=""" & sDestinationPath & """ />    " & _
                           "</PARAMETERS>    " & _
                           "<ITEMS>    " & _
                           "</ITEMS>    " & _
                       "</JOB>    " & _
                   "</BACKGROUND_JOB>"
                CreateBackgroundJob(xlJob)
            Else
                sCopyReportPathName = m_sScheduleReportPath & "\" & "Temp-" & sRepName
                lResult = m_oBusinessDocAPI.CopyFile(sFileIn:=sReportPath, _
                                                    sFileOut:=sCopyReportPathName)
                If lResult <> PMEReturnCode.PMTrue Then
                    Console.WriteLine("Copying Archive document to Server failed : Report Name - " + sReportName)
                End If

                lResult = m_oBusinessDocAPI.AddDocument(lPartyId:=0, _
                            sPartyName:="", _
                            lInsuranceFolderId:=0, _
                            sInsuranceFileRef:="", _
                            lClaimId:=0, _
                            sClaimRef:="", _
                            lFSAComplaintFolderCnt:=0, _
                            sFSAComplaintReference:="", _
                            sDocType:="F", _
                            sPageType:="PDF", _
                            sDocName:=sReportName, _
                            sFilename:=sCopyReportPathName, _
                            sAnnotation:="", _
                            sKeywords:=sKeywords, _
                            lDocNumber:=0, _
                            vDocumentTemplateID:=0, _
                            bVisibleFromWeb:=False, _
                            bCalledFromReportScheduler:=True, _
                            sFrequency:=Frequency)

                If lResult <> PMEReturnCode.PMTrue Then
                    Console.WriteLine("Copying Archive document to Server failed : Report Name - " + sReportName)
                End If
            End If
            Console.WriteLine("Document Archieved Successfully : Report Name - " + sReportName)
        End If
        'Increment Dates Automatically
        If bAutomatic = True Then
            lResult = IncrementDatesAutomatic(v_dtStartDate:=dtStartDate, _
                            v_dtEndDate:=dtEndDate, v_ireport_scheduler_id:=v_ireport_scheduler_id)
            If lResult <> PMEReturnCode.PMTrue Then
                Console.WriteLine("Increment Dates Automatically failed : Report Name - " + sReportName)
            End If
        End If
        Return lResult
    End Function

    'Here Background job is created for create Archieve file on sharepoint.
    Private Function CreateBackgroundJob(ByVal v_sXML As String) As Integer

        Dim oDatabase As dPMDAO.Database = Nothing
        Dim vResults As Object = Nothing
        Dim iReturnCode As Integer
        Dim oBackgroundjobID As Integer


        Try
            CreateBackgroundJob = 1
            DBConnect(oDatabase)

            AddParameterLite(oDatabase, "background_job_id", Nothing, PMEParameterDirection.PMParamOutput, PMEDataType.PMInteger, True)
            AddParameterLite(oDatabase, "description", "Archive documents", PMEParameterDirection.PMParamInput, PMEDataType.PMString, False)
            AddParameterLite(oDatabase, "job_xml", v_sXML, PMEParameterDirection.PMParamInput, PMEDataType.PMString, False)
            AddParameterLite(oDatabase, "job_when_to_start", Date.Today, PMEParameterDirection.PMParamInput, PMEDataType.PMDate, False)
            AddParameterLite(oDatabase, "job_user_id", m_iUserId, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, False)

            iReturnCode = oDatabase.SQLSelect("spu_SIR_Background_Job_add", "spu_SIR_Background_Job_add", True, vResultArray:=vResults)
            oBackgroundjobID = Convert.ToInt32(oDatabase.Parameters.Item("background_job_id").Value)
            DBDisconnect(oDatabase)
            oDatabase = Nothing
            Return iReturnCode

        Catch ex As Exception
            System.Console.WriteLine("CreateBackgroundJob Failed - " + ex.Message)
            Throw ex
        End Try

    End Function

    Private Function getUserIDFromCode(ByVal v_sUserName As String, ByRef vResults As Object) As Integer

        Dim oDatabase As dPMDAO.Database = Nothing
        Dim iReturnCode As Integer


        Try
            getUserIDFromCode = 1
            DBConnect(oDatabase)
            AddParameterLite(oDatabase, "UserName", v_sUserName, PMEParameterDirection.PMParamInput, PMEDataType.PMString, True)
            iReturnCode = oDatabase.SQLSelect("spu_Get_UserID_From_Code", "spu_Get_UserID_From_Code", True, vResultArray:=vResults)

            If IsNothing(vResults) Or Not IsArray(vResults) Then
                Console.WriteLine("UserID From Code not found")
                getUserIDFromCode = 0
            End If

            DBDisconnect(oDatabase)
            oDatabase = Nothing

            Return iReturnCode
        Catch ex As Exception
            System.Console.WriteLine("getUserIDFromCode Failed - " + ex.Message)
            Throw ex
        End Try

    End Function
#End Region

#Region " Procedures "

    Private Sub OutputSyntax()
        'Console.WriteLine("Unable To Run Report Scheduler")
    End Sub

#End Region

End Module
