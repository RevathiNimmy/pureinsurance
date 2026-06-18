Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Name: bSIRTaskList (Business Tier)
    '
    ' Description: Business object for displaying tasks
    '               attached to a specific Party or Policy
    '
    ' Edit History:
    ' MSS100701 - Created
    ' ***************************************************************** '

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    Private m_lPartyCnt As Integer
    Private m_InsuranceFileCnt As Integer

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise
        Dim result As Integer = 0
        Dim ACClass As Object
        Dim m_iCurrencyID, m_iLanguageID, m_iLogLevel, m_iSourceID, m_iUserID As Integer
        Dim m_sCallingAppName, m_sPassword, m_sUsername As String


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


            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

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
                Dim ACClass As Object
                Dim m_cEntityFieldNames, m_cEntityFields As Object
                Dim m_oOrionDatabase As Object
                Dim m_sUsername As String = ""
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
                End If
                If Not (m_oOrionDatabase Is Nothing) Then
                    m_oOrionDatabase.CloseDatabase()
                    m_oOrionDatabase = Nothing

                End If
                m_cEntityFields = Nothing
                m_cEntityFieldNames = Nothing
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
        Dim ACClass As Object
        Dim m_sTransactionType As String = ""
        Dim m_lProcessMode, m_lNavigate As Integer
        Dim m_iTask As Integer
        Dim m_dtEffectiveDate As Date
        Dim m_sUsername As String = ""

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'FSA Phase III Pass Optional Parameter for Complaint
    Public Function GetAvailableTasks(ByRef r_vArray(,) As Object, ByVal m_lPartyCnt As Integer, Optional ByVal m_lInsuranceFileCnt As Integer = 0, Optional ByVal m_lFSAComplaintFolderCnt As Integer = 0) As Integer
        Dim result As Integer = 0
        Dim ACClass As Object
        Dim m_sUsername, sSQL As String

        Try

            'DJM 18/02/2004 : Add XML Navigator functionality.
            sSQL = "SELECT ti.pmwrk_task_instance_cnt, " & _
                    "ti.is_urgent , ti.task_status , t.type_of_task , " & _
                    "t.is_system_task , ti.task_due_date , ti.customer , " & _
                    "ti.description , ug.description , u.username , " & _
                    "t.pmnav_process_id , t.component_object_name , t.component_class_name , " & _
                    "t.display_icon , t.is_view_only_task , t.linked_object_name , " & _
                    "t.linked_class_name , t.linked_caption_id , ti.is_visible, ug.pmuser_group_id, x.file_name " & _
                    "FROM PMNav_Key nk JOIN PMWrk_Task_Inst_Key tik ON nk.pmnav_key_id = tik.pmnav_key_id " & _
                    "JOIN PMWrk_Task_Instance ti ON tik.pmwrk_task_instance_cnt = ti.pmwrk_task_instance_cnt " & _
                    "JOIN PMWrk_Task t ON ti.pmwrk_task_id = t.pmwrk_task_id " & _
                    "LEFT JOIN PMUser u ON ti.User_ID = u.User_ID " & _
                    "JOIN PMUser_Group ug ON ti.pmuser_group_id = ug.pmuser_group_id " & _
                   "LEFT JOIN PMNavXM_Process x ON x.pmnavxm_process_id = t.pmnavxm_process_id"

            ' If we are a policy, refine otherwise go for just the party
            If m_lInsuranceFileCnt > 0 Then
                sSQL = sSQL & " WHERE nk.name = 'insurance_file_cnt' AND tik.key_value = '" & CStr(m_lInsuranceFileCnt) & "'"
            Else
                'FSA Phase III
                If m_lFSAComplaintFolderCnt > 0 Then
                    sSQL = sSQL & " WHERE nk.name = 'fsa_complaint_folder_cnt' AND tik.key_value = '" & CStr(m_lFSAComplaintFolderCnt) & "'"
                Else

                    'TR - 18/03/2004 - Added single quote marks to be SQL 2000 compliant
                    sSQL = sSQL & " WHERE nk.name = 'party_cnt' AND tik.key_value = '" & CStr(m_lPartyCnt) & "'"
                End If

            End If

            ' Action SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetAvailableTasks", bStoredProcedure:=False, lNumberRecords:=-1, vResultArray:=r_vArray)


            Return m_lReturn

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAvailableTasks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAvailableTasks", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class