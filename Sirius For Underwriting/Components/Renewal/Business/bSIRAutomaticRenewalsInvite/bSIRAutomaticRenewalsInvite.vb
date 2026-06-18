Option Strict Off
Option Explicit On
Imports System.IO
'Modified by Sudhanshu Behera on 5/26/2010 12:20:03 PM refer developer guide no. 129
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 02/09/2000
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a SIRRenSelection.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 19/12/2003
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
    Private Const ACClass As String = "Business"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)


    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Calling Application Name

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_dtSelectionDate As Date
    'Modified by Sudhanshu Behera on 5/26/2010 12:20:57 PM refer developer guide no. 108
    'Private m_oDocManagerWrapper As bSIRDocManagerWrapper.Interface
    Private m_oDocManagerWrapper As bSIRDocManagerWrapper.Interface_Renamed
    Private m_oFindDocTemplate As bSIRFindDocTemplate.Form
    Private m_oBusiness As bSIRRenInvitePrint.Business
    Private m_oReport As Object

#If IN_DEBUG > 0 Then

	Private m_oDebugTimings As Object
#End If
    Private lPMAuthorityLevel As Integer

    Public WriteOnly Property dtSelectionDate() As Date
        Set(ByVal Value As Date)
            m_dtSelectionDate = Value
        End Set
    End Property


    ' Primary Keys to work with
    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)

            Value = Value

        End Set
    End Property


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
    Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer





        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel




            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password

            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            'Renewal Invite Print
            m_oBusiness = New bSIRRenInvitePrint.Business()
            m_lReturn = m_oBusiness.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oBusiness.Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Document Manager Wrapper (Business)
            'Modified by Sudhanshu Behera on 5/26/2010 12:25:44 PM refer developer guide no. 108
            'm_oDocManagerWrapper = New bSIRDocManagerWrapper.Interface()
            m_oDocManagerWrapper = New bSIRDocManagerWrapper.Interface_Renamed()
            m_lReturn = CType(m_oDocManagerWrapper.InitialiseBusiness(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDocManagerWrapper.InitialiseBusiness Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Document Template
            m_oFindDocTemplate = New bSIRFindDocTemplate.Form()
            m_lReturn = m_oFindDocTemplate.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oFindDocTemplate.Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create instance of bSIRReportPrint.Business
     'm_oReport = New bSIRReportPrint.Business()
    'm_lReturn = m_oReport.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="bSIRReportPrint.Business.Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
    '    Return gPMConstants.PMEReturnCode.PMFalse
    'End If

    result = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oReport, v_sClassName:="bSIRReportPrint.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUserName, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
    If result <> gPMConstants.PMEReturnCode.PMTrue Then
         Dim r_sMessage As String = "Failed to create an instance of bSIRReportPrint.Business"
         bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="bSIRReportPrint.Business", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
         Return result
    End If

#If IN_DEBUG > 0 Then

			'Debug Timings
			Set m_oDebugTimings = CreateLateBoundObject("bSIRDebugTimings.Interface")
			m_oDebugTimings.CallingAppName = ACApp
#End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


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
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()

                End If
                m_oDatabase = Nothing
                If m_oBusiness IsNot Nothing Then
                    m_oBusiness.Dispose()
                    m_oBusiness = Nothing
                End If
                If m_oDocManagerWrapper IsNot Nothing Then
                    m_oDocManagerWrapper.Dispose()
                    m_oDocManagerWrapper = Nothing
                End If
                If m_oFindDocTemplate IsNot Nothing Then
                    m_oFindDocTemplate.Dispose()
                    m_oFindDocTemplate = Nothing
                End If
                If m_oReport IsNot Nothing Then
                    m_oReport.Dispose()
                    m_oReport = Nothing
                End If
#If IN_DEBUG > 0 Then

			Set m_oDebugTimings = Nothing
#End If

            End If
        End If
        Me.disposedValue = True
    End Sub


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


            If Not Informations.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Informations.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Informations.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Informations.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Informations.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: Start
    '
    ' Description:
    '
    ' History: 09/10/2002 sj - Created.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Const ACFieldPosPartyCnt As Integer = 0
            Const ACFieldPosInsuranceFolderCnt As Integer = 1
            Const ACFieldPosInsuranceFileCnt As Integer = 2
            Const ACFieldPosRenewalStatusCnt As Integer = 3

            Dim vResultArray(,) As Object = Nothing
            Dim lInsuranceFileCnt, lPartyCnt, lInsuranceFolderCnt, lRenewalStatus As Integer
            Dim dtSelectionDate As Date

#If IN_DEBUG > 0 Then

			m_oDebugTimings.StartTiming "Start"
			m_oDebugTimings.StartTiming "GetRenewalInviteList"
#End If

            dtSelectionDate = m_dtSelectionDate.AddYears(3)
            'Get the list of records to invite

            m_lReturn = m_oBusiness.GetRenewalInviteList(v_dtSelectionDate:=dtSelectionDate, v_vProductID:=DBNull.Value, v_vAgentID:=DBNull.Value, v_lSortOrder:=1, r_vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oBusiness.GetRenewalInviteList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

#If IN_DEBUG > 0 Then

			m_oDebugTimings.EndTiming "GetRenewalInviteList"
#End If

            If Not Informations.IsArray(vResultArray) Then
                Return result
            End If

#If IN_DEBUG > 0 Then

			m_oDebugTimings.StartTiming "DelLastPrintRun"
#End If
            'delete existing data on last_print_run table
            m_lReturn = m_oBusiness.DelLastPrintRun()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oBusiness.DelLastPrintRun Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
#If IN_DEBUG > 0 Then

			m_oDebugTimings.EndTiming "DelLastPrintRun"
#End If

            'loop thro and process each renewal invite

            For lCount As Integer = 0 To vResultArray.GetUpperBound(1)


                lInsuranceFileCnt = CInt(vResultArray(ACFieldPosInsuranceFileCnt, lCount))

                lPartyCnt = CInt(vResultArray(ACFieldPosPartyCnt, lCount))

                lInsuranceFolderCnt = CInt(vResultArray(ACFieldPosInsuranceFolderCnt, lCount))

                lRenewalStatus = CInt(vResultArray(ACFieldPosRenewalStatusCnt, lCount))

                'Start a database transaction
                m_lReturn = CType(BeginTrans(v_vInsuranceFileCnt:=lInsuranceFileCnt), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Print the invitation document
#If IN_DEBUG > 0 Then

				m_oDebugTimings.StartTiming "PrintDocument"
#End If
                m_lReturn = CType(PrintDocument(v_lPartyCnt:=lPartyCnt, v_lInsuranceFolderCnt:=lInsuranceFolderCnt, v_lInsuranceFileCnt:=lInsuranceFileCnt, v_lProcessType:=ACDocTypeRenewal), gPMConstants.PMEReturnCode)
#If IN_DEBUG > 0 Then

				m_oDebugTimings.EndTiming "PrintDocument"
#End If
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PrintDocument failed for InsuranceFileCnt " & lInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
                    'Rollback transaction
                    m_lReturn = RollbackTrans()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Else
                    'update renewal status to await printing and is_invite_printed = PMTRUE
#If IN_DEBUG > 0 Then

					m_oDebugTimings.StartTiming "UpdateRenewalStatus"
#End If
                    m_lReturn = m_oBusiness.UpdateRenewalStatus(v_lInsuranceFileCnt:=lInsuranceFileCnt)
#If IN_DEBUG > 0 Then

					m_oDebugTimings.EndTiming "UpdateRenewalStatus"
#End If
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oBusiness.UpdateRenewalStatus failed for InsuranceFileCnt " & lInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
                        'Rollback transaction
                        m_lReturn = CType(RollbackTrans(v_vInsuranceFileCnt:=lInsuranceFileCnt), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    Else
#If IN_DEBUG > 0 Then

						m_oDebugTimings.StartTiming "AddLastPrintRun"
#End If
                        'add to last_print_run table
                        m_lReturn = m_oBusiness.AddLastPrintRun(v_lRenewalStatusCnt:=lRenewalStatus)
#If IN_DEBUG > 0 Then

						m_oDebugTimings.EndTiming "AddLastPrintRun"
#End If
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oBusiness.AddLastPrintRun failed for InsuranceFileCnt " & lInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
                            'Rollback transaction
                            m_lReturn = CType(RollbackTrans(v_vInsuranceFileCnt:=lInsuranceFileCnt), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                        Else
                            'Commit the transaction
                            m_lReturn = CType(CommitTrans(v_vInsuranceFileCnt:=lInsuranceFileCnt), gPMConstants.PMEReturnCode)

                        End If

                    End If

                End If

            Next lCount

#If IN_DEBUG > 0 Then

			m_oDebugTimings.StartTiming "PrintRenewalInviteReport"
#End If

            'Spool the report
            m_lReturn = PrintRenewalInviteReport()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="PrintRenewalInviteReport Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

#If IN_DEBUG > 0 Then

			m_oDebugTimings.EndTiming "PrintRenewalInviteReport"
			m_oDebugTimings.EndTiming "Start"
			m_oDebugTimings.Report
#End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: PrintDocument (Private)
    '
    ' Description: Print out document
    '
    ' ***************************************************************** '
    Private Function PrintDocument(ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lProcessType As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDocManagerWrapper

            .PartyCnt = v_lPartyCnt
            .InsuranceFileCnt = v_lInsuranceFileCnt
            .InsuranceFolderCnt = v_lInsuranceFolderCnt
            .ProcessTypesDocsId = v_lProcessType
            .Mode = 4
            m_lReturn = CType(.Start(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        Return result

    End Function
    ' ***************************************************************** '
    '
    ' Name: GetAgents
    '
    ' Description:
    '
    ' History: 10/05/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function GetAgents(ByRef r_vAgentArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT party_cnt, trading_name" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "FROM Party_Agent" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ORDER BY trading_name"

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetAllAgents", bStoredProcedure:=False, vResultArray:=r_vAgentArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAgents Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAgents", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: PrintRenewalInviteReport
    '
    ' Description: Print Renewal Reports
    '
    ' ***************************************************************** '
    Private Function PrintRenewalInviteReport() As Integer

        Dim result As Integer = 0
        Dim sExportFile As Object = ""
        Dim lDocTypeID As Integer
        Dim sReportOutputLocation As String = ""

        Const ACReportAgentRenewalList As String = "AgentRenewalList"



        result = gPMConstants.PMEReturnCode.PMTrue

        'Get the path of the report
        m_lReturn = CType(GetReportPath(r_sReportOutputLocation:=sReportOutputLocation), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed To Get Document Type ID For Code (REPORT)", vApp:=ACApp, vClass:=ACClass, vMethod:="PrintRenewalInviteReport")

            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'delete previous version report
        If Directory.GetFiles(sReportOutputLocation & ACReportAgentRenewalList & ".*", FileAttribute.Normal)(0) <> "" Then
            File.Delete(sReportOutputLocation & ACReportAgentRenewalList & ".*")
        End If

        m_oReport.reportName = ACReportAgentRenewalList

        'export to word format
        m_lReturn = m_oReport.ExportToDisk(r_ExportFile:=sExportFile, v_iFormatType:=0)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed To Export Agent List To Word Format", vApp:=ACApp, vClass:=ACClass, vMethod:="PrintRenewalInviteReport")

            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get document type id
        m_lReturn = m_oBusiness.GetDocTypeID(v_sDocCode:="REPORT", r_lDocTypeID:=lDocTypeID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed To Get Document Type ID For Code (REPORT)", vApp:=ACApp, vClass:=ACClass, vMethod:="PrintRenewalInviteReport")

            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'spool the report

        ' Build the full path of the report
        sExportFile = sReportOutputLocation &
                      ACReportAgentRenewalList &
                      m_iUserID &
                      ".doc"

        'Use the document manager Wrapper to spool the report
        With m_oDocManagerWrapper
            'Initialise variables from previous run
            .PartyCnt = 0
            .InsuranceFileCnt = 0
            .InsuranceFolderCnt = 0
            .ProcessTypesDocsId = 0
            .DocumentTemplateId = 0

            'Set up properties for report run
            .DocName = sExportFile
            .DocumentTypeId = lDocTypeID
            .SpoolDesc = "Renewal Invite Agent List"
            .Mode = gSIRLibrary.ACSpoolReportMode
            m_lReturn = CType(.Start(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to spool Renewal Invite Agent List", vApp:=ACApp, vClass:=ACClass, vMethod:="PrintRenewalInviteReport")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetReportPath
    '
    ' Description: Gets the Report Templates location from the registry.
    '
    ' ***************************************************************** '
    Private Function GetReportPath(ByRef r_sReportOutputLocation As String) As Integer

        Dim result As Integer = 0
        Dim sRegPath As String = ""

        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily



        result = gPMConstants.PMEReturnCode.PMTrue

        r_sReportOutputLocation = ""

        ' Set to LocalMachine/Sirius/Client
        eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
        eProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

        ' Location for Exported Reports
        sRegPath = ""
        m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="PrntFileDir", r_sSettingValue:=sRegPath), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get Report Destination directory from Registry.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReportPath")
            Return gPMConstants.PMEReturnCode.PMFalse
        Else
            r_sReportOutputLocation = sRegPath
        End If

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    Private Function BeginTrans(Optional ByVal v_vInsuranceFileCnt As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            Dim sMessage As String = ""

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLBeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                If Not Informations.IsNothing(v_vInsuranceFileCnt) Then

                    sMessage = "Failed to Commit database transaction for " & CStr(v_vInsuranceFileCnt)
                Else
                    sMessage = "Failed to Commit database transaction"
                End If
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    Private Function CommitTrans(Optional ByVal v_vInsuranceFileCnt As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            Dim sMessage As String = ""

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLCommitTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                If Not Informations.IsNothing(v_vInsuranceFileCnt) Then

                    sMessage = "Failed to Commit database transaction for " & CStr(v_vInsuranceFileCnt)
                Else
                    sMessage = "Failed to Commit database transaction"
                End If
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    Private Function RollbackTrans(Optional ByVal v_vInsuranceFileCnt As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            Dim sMessage As String = ""

            result = gPMConstants.PMEReturnCode.PMTrue


            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLRollbackTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                If Not Informations.IsNothing(v_vInsuranceFileCnt) Then

                    sMessage = "Failed to Rollback database transaction for " & CStr(v_vInsuranceFileCnt)
                Else
                    sMessage = "Failed to Rollback database transaction"
                End If
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Sub New()
        MyBase.New()

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        ' Class Initialise
        '
        'Catch excep As System.Exception
        '
        '
        '
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class
