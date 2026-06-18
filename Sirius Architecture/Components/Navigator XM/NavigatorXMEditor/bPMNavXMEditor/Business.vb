Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness

    ' ************************************************
    ' Added to replace global variables 18/09/2003
    ' Username.
    Private m_sUsername As String = ""

    ' Password.
    Private m_sPassword As String = ""

    ' User ID
    Private m_iUserID As Integer

    ' Calling Application
    Private m_sCallingAppName As String = ""
    ' Source ID
    Private m_iSourceID As Integer
    ' Language ID
    Private m_iLanguageID As Integer
    ' Currency ID
    Private m_iCurrencyID As Integer
    ' LogLevel
    Private m_iLogLevel As Integer
    ' ************************************************


    Private m_lReturn As gPMConstants.PMEReturnCode

    Private m_oDatabase As dPMDAO.Database

    Private Const ACClass As String = "Business"

    Public Sub New()
        MyBase.New()
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMFalse

        m_lReturn = CType(gPMComponentServices.NewDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If


        result = gPMConstants.PMEReturnCode.PMTrue
        '
        ' *******************************************************************
        ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
        ' Set Username and Password
        m_sUsername = sUsername
        m_sPassword = sPassword
        m_iUserID = iUserID
        m_sCallingAppName = sCallingAppName
        m_iLanguageID = iLanguageID
        m_iSourceID = iSourceID
        m_iCurrencyID = iCurrencyID
        m_iLogLevel = iLogLevel


        Return result



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialse", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result
    End Function

    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                m_oDatabase.CloseDatabase()

                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetTaskMaps
    '
    ' Description: Get roadmap details from PMWrk_Task
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Public Function GetTaskMaps(ByRef vMaps(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lNumRecs As Integer
        Dim sSQL As String = ""

        Try
            result = gPMConstants.PMEReturnCode.PMFalse

            sSQL = "SELECT wt.PMWrk_Task_ID, wt.code, wt.description, np.PMNavXM_Process_ID, np.file_name"
            sSQL = sSQL & " FROM PMWrk_Task wt, PMNavXM_Process np"
            sSQL = sSQL & " WHERE wt.PMNavXM_Process_ID = np.PMNavXM_Process_ID"
            sSQL = sSQL & " AND wt.effective_date <= '" & StringsHelper.Format(DateTime.Now, FORMAT_DATE) & "'"
            sSQL = sSQL & " AND wt.is_deleted <> 1"
            sSQL = sSQL & " AND wt.PMNavXM_Process_ID IS NOT NULL"
            sSQL = sSQL & " ORDER BY wt.code"

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetTaskMaps", bStoredProcedure:=False, lNumberRecords:=lNumRecs, vResultArray:=vMaps)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Information.IsArray(vMaps) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTaskMaps Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTaskMaps", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetProcessMaps
    '
    ' Description: Get roadmap details from PMNavXM_Process
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Public Function GetProcessMaps(ByVal lUserMode As Integer, ByRef vMaps(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lNumRecs As Integer
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            sSQL = "SELECT PMNavXM_Process_ID, file_name, file_version_number, file_timestamp "
            sSQL = sSQL & "FROM PMNavXM_Process "
            sSQL = sSQL & "ORDER BY file_name"

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetProcessMaps", bStoredProcedure:=False, lNumberRecords:=lNumRecs, vResultArray:=vMaps)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Information.IsArray(vMaps) Then
                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetProcessMaps Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProcessMaps", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetXMLFromPrimary
    '
    ' Description: Get roadmap XML from database
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Public Function GetXMLFromPrimary(ByVal lNavXMProcessID As Integer, ByRef sFilename As String, ByRef sXML As String) As Integer

        Dim result As Integer = 0
        Dim lNumRecs As Integer
        Dim sSQL As String = ""
        Dim vTemp As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            sSQL = "SELECT file_name, xml_definition "
            sSQL = sSQL & "FROM PMNavXM_Process "
            sSQL = sSQL & "WHERE PMNavXM_Process_ID = " & CStr(lNavXMProcessID)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetXMLFromPrimary", bStoredProcedure:=False, lNumberRecords:=lNumRecs)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            vTemp = m_oDatabase.Records.Fields()("xml_definition")

            If Convert.IsDBNull(vTemp) Or IsNothing(vTemp) Then
                Return result
            End If

            sXML = vTemp
            sFilename = m_oDatabase.Records.Fields()("file_name")


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetXMLFromPrimary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetXMLFromPrimary", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetXMLFromPrimary
    '
    ' Description: Get roadmap XML from database
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Public Function GetXMLFromSecondary(ByVal lNavXMProcessVersionID As Integer, ByRef lVersionNumber As Integer, ByRef sFilename As String, ByRef sXML As String) As Integer

        Dim result As Integer = 0
        Dim lNumRecs As Integer
        Dim sSQL As String = ""
        Dim vTemp As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            sSQL = "SELECT pr.file_name, prv.version_number, prv.xml_definition "
            sSQL = sSQL & "FROM PMNavXM_Process_Version prv, PMNavXM_Process pr "
            sSQL = sSQL & "WHERE prv.Version_ID = " & CStr(lNavXMProcessVersionID)
            sSQL = sSQL & "  AND prv.pmnavxm_process_id = pr.pmnavxm_process_id"

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetXMLFromSecondary", bStoredProcedure:=False, lNumberRecords:=lNumRecs)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            vTemp = m_oDatabase.Records.Fields()("xml_definition")

            If Convert.IsDBNull(vTemp) Or IsNothing(vTemp) Then
                Return result
            End If

            sXML = vTemp
            sFilename = m_oDatabase.Records.Fields()("file_name")
            lVersionNumber = m_oDatabase.Records.Fields()("version_number")


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetXMLFromSecondary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetXMLFromSecondary", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetXMLFromFile
    '
    ' Description: Get roadmap XML from hard disk
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Public Function GetXMLFromFile(ByVal sFilePath As String, ByRef sXML As String) As Integer

        Dim result As Integer = 0
        Dim lFileMode As gPMConstants.PMEFSOFileMode
        Dim oFSO As Object
        Dim oTS As FileStream

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            sXML = ""

            oFSO = New Object()

            ' open the file for reading
            lFileMode = gPMConstants.PMEFSOFileMode.PMFSOFileModeRead

            oTS = New FileStream(sFilePath, FileMode.Open, FileAccess.ReadWrite)

            ' all in one go
            ' file is read 'as is'. Calling app will need to check validity
            'developer guide no. 23(NO SOLUTION)
            'sXML = oTS.ReadAll()

            oTS = Nothing
            oFSO = Nothing


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetXMLFromFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetXMLFromFile", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetUserAuthority
    '
    ' Description: Gets the User Groups that the current user is a
    '              Supervisor of.
    '
    ' ***************************************************************** '
    Public Function GetUserAuthority(ByRef r_bIsAdministrator As Boolean, ByRef r_vSupervisedGroups As Object) As Integer

        Dim result As Integer = 0
        Dim oUserGroup As bPMUserGroup.Utilities

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            oUserGroup = New bPMUserGroup.Utilities
            m_lReturn = oUserGroup.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Is the User an Administrator

            m_lReturn = oUserGroup.IsUserAdministrator(v_iUserID:=m_iUserID, r_bIsAdministrator:=r_bIsAdministrator)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oUserGroup.Dispose()
                oUserGroup = Nothing
                Return result
            End If

            ' Get the Groups they Supervise

            m_lReturn = oUserGroup.GetGroupsSupervisedByUser(v_iUserID:=m_iUserID, r_vSupervisedGroups:=r_vSupervisedGroups)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                oUserGroup.Dispose()
                oUserGroup = Nothing
                Return result
            End If

            ' Terminate

            oUserGroup.Dispose()
            oUserGroup = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserAuthority Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserAuthority", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetAvailableSteps
    '
    ' Description: get secondary steps available to user
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Public Function GetAvailableSteps(ByRef vSteps(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lNumRecs As Integer
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            sSQL = "SELECT pmnavxm_available_step_id, available_step_code, available_step_description, available_step_icon "
            sSQL = sSQL & "FROM pmnavxm_available_step "
            sSQL = sSQL & "WHERE is_deleted <> 1 "
            sSQL = sSQL & "AND effective_date <= '" & StringsHelper.Format(DateTime.Now, FORMAT_DATE) & "' "
            sSQL = sSQL & "ORDER BY available_step_code"

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetAvailableSteps", bStoredProcedure:=False, lNumberRecords:=lNumRecs, vResultArray:=vSteps)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Information.IsArray(vSteps) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAvailableSteps Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAvailableSteps", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetAvailableStepDetails
    '
    ' Description: get attributes for selected step
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Public Function GetAvailableStepDetails(ByVal lAvailableStepID As Integer, ByRef vStepDetails(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lNumRecs As Integer
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            sSQL = "SELECT available_step_code, attribute_description, attribute_component, "
            sSQL = sSQL & "attribute_type, attribute_cancelaction, "
            sSQL = sSQL & "attribute_okaction, attribute_oksteps, "
            sSQL = sSQL & "attribute_cancelsteps, attribute_componentaction, "
            sSQL = sSQL & "attribute_serverside, attribute_createwmtask, "
            sSQL = sSQL & "attribute_resumestep, attribute_core, attribute_submap, "
            sSQL = sSQL & "attribute_oknewroadmap, attribute_cancelnewroadmap "
            sSQL = sSQL & "FROM PMNavXM_Available_Step "
            sSQL = sSQL & "WHERE pmnavxm_available_step_id = " & CStr(lAvailableStepID)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetAvailableStepDetails", bStoredProcedure:=False, lNumberRecords:=lNumRecs, vResultArray:=vStepDetails)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Information.IsArray(vStepDetails) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAvailableStepDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAvailableStepDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDefaultKeys
    '
    ' Description: get attributes for selected step
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Public Function GetDefaultKeys(ByVal lAvailableStepID As Integer, ByRef vKeys(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lNumRecs As Integer
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            sSQL = "SELECT nk.name "
            sSQL = sSQL & "FROM PMNavXM_Step_Default_Key df, PMNav_Key nk "
            sSQL = sSQL & "WHERE df.pmnav_key_id = nk.pmnav_key_id "
            sSQL = sSQL & "AND df.pmnavxm_available_step_id = " & CStr(lAvailableStepID)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetDefaultKeys", bStoredProcedure:=False, lNumberRecords:=lNumRecs, vResultArray:=vKeys)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Information.IsArray(vKeys) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefaultKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaultKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: SaveXMLData
    '
    ' Description: update version number, core status and XML
    '              definition for supplied Nav XM process
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Public Function SaveXMLData(ByVal lNavXMProcessID As Integer, ByVal lVersion As Integer, ByVal lCore As Integer, ByVal sXML As String) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' check for apostrophes
            sXML = sXML.Replace("'", "''")

            sSQL = "UPDATE PMNavXM_Process "
            sSQL = sSQL & "SET file_timestamp = '" & StringsHelper.Format(DateTime.Now, FORMAT_DATE) & "', "
            sSQL = sSQL & "file_version_number = " & CStr(lVersion) & ", "
            sSQL = sSQL & "is_core = " & CStr(lCore) & ", "
            sSQL = sSQL & "xml_definition = '" & sXML & "' "
            sSQL = sSQL & "WHERE pmnavxm_process_id = " & CStr(lNavXMProcessID)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="SaveXMLData", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveXMLData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveXMLData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CreateNewRoadmap
    '
    ' Description: creates new nav XM process for administrator
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Public Function CreateNewRoadmap(ByRef lNavXMProcessID As Integer, ByVal sFilename As String) As Integer

        Dim result As Integer = 0
        Dim lNumRecs As Integer
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            sSQL = "SELECT ISNULL(MAX(pmnavxm_process_id) + 1, 1) AS next_process_key "
            sSQL = sSQL & "FROM PMNavXM_Process"

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetNextProcessKey", bStoredProcedure:=False, lNumberRecords:=lNumRecs)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            lNavXMProcessID = m_oDatabase.Records.Fields("next_process_key")

            sSQL = "INSERT INTO PMNavXM_Process ("
            sSQL = sSQL & "pmnavxm_process_id, file_name, file_version_number, "
            sSQL = sSQL & "file_timestamp, is_custom, is_core, xml_definition) "
            sSQL = sSQL & " VALUES ("
            sSQL = sSQL & "" & CStr(lNavXMProcessID) & ", "
            sSQL = sSQL & "'" & sFilename & "', "
            sSQL = sSQL & "0, "
            sSQL = sSQL & "'" & StringsHelper.Format(DateTime.Now, FORMAT_DATE) & "', "
            sSQL = sSQL & "0, "
            sSQL = sSQL & "1, "
            sSQL = sSQL & "'')"

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="CreateNewRoadmap", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateNewRoadmap Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateNewRoadmap", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckRoadmapProcessExists
    '
    ' Description: returns process ID if roadmap filename exists
    '              in nav XM process table
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Public Function CheckRoadmapProcessExists(ByVal sRoadmapFilename As String, ByRef lNavXMProcessID As Integer) As Integer

        Dim result As Integer = 0
        Dim lNumRecs As Integer
        Dim sSQL As String = ""
        Dim vPID As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            sSQL = "SELECT pmnavxm_process_id "
            sSQL = sSQL & "FROM pmnavxm_process "
            sSQL = sSQL & "WHERE file_name = '" & sRoadmapFilename & "'"

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="CheckRoadmapProcessExists", bStoredProcedure:=False, lNumberRecords:=lNumRecs)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            vPID = m_oDatabase.Records.Fields()("pmnavxm_process_id")

            If Convert.IsDBNull(vPID) Or IsNothing(vPID) Then
                lNavXMProcessID = ID_NO_VALUE
            Else
                lNavXMProcessID = CInt(vPID)
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckRoadmapProcessExists failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckRoadmapProcessExists", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDocumentTypes
    '
    ' Description: get available document template types
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Public Function GetDocumentTypes(ByRef vTypes(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lNumRecs As Integer
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            sSQL = "SELECT document_type_id, code, description "
            sSQL = sSQL & "FROM document_type "
            sSQL = sSQL & "WHERE is_deleted <> 1 "
            sSQL = sSQL & "AND effective_date <= '" & StringsHelper.Format(DateTime.Now, FORMAT_DATE) & "' "
            sSQL = sSQL & "ORDER BY description"

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetDocumentTypes", bStoredProcedure:=False, lNumberRecords:=lNumRecs, vResultArray:=vTypes)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Information.IsArray(vTypes) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDocumentTypes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocumentTypes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDocumentTemplates
    '
    ' Description: get available document templates for supplied doc type
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Public Function GetDocumentTemplates(ByVal lDocTypeID As Integer, ByRef vTemplates(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lNumRecs As Integer
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            sSQL = "SELECT document_template_id, dt.code, dt.description, dty.code "
            sSQL = sSQL & "FROM document_template dt, document_type dty "
            sSQL = sSQL & "WHERE dt.is_deleted <> 1 AND dty.is_deleted <> 1 "
            sSQL = sSQL & "AND dt.document_type_id = dty.document_type_id "

            If lDocTypeID <> ID_NO_VALUE Then
                sSQL = sSQL & "AND dt.document_type_id = " & CStr(lDocTypeID) & " "
            End If

            sSQL = sSQL & "ORDER BY dt.description"

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetDocumentTemplates", bStoredProcedure:=False, lNumberRecords:=lNumRecs, vResultArray:=vTemplates)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Information.IsArray(vTemplates) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDocumentTemplates Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocumentTemplates", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetTaskMapVersions
    '
    ' Description: Get created copies of core maps
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Public Function GetTaskMapVersions(ByRef vMaps(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lNumRecs As Integer
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            sSQL = "SELECT version_id, pmnavxm_process_id, version_timestamp, version_number, description, code "
            sSQL = sSQL & "FROM pmnavxm_process_version  "
            sSQL = sSQL & "ORDER BY version_timestamp"

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetTaskMapVersions", bStoredProcedure:=False, lNumberRecords:=lNumRecs, vResultArray:=vMaps)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Information.IsArray(vMaps) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTaskMapVersions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTaskMapVersions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetVersionNumber
    '
    ' Description: get a process's version number
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Public Function GetVersionNumber(ByVal lPMNavXMProcessID As Integer, ByRef lVersion As Integer) As Integer

        Dim result As Integer = 0
        Dim lNumRecs As Integer
        Dim sSQL As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            sSQL = "SELECT ISNULL(file_version_number, 0) AS version_no FROM pmnavxm_process "
            sSQL = sSQL & "WHERE pmnavxm_process_id = " & CStr(lPMNavXMProcessID)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetProcessVersionID", bStoredProcedure:=False, lNumberRecords:=lNumRecs)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            lVersion = m_oDatabase.Records.Fields()("version_no")

            sSQL = "SELECT ISNULL(MAX(version_number), " & lVersion & ") AS version_no FROM pmnavxm_process_version "
            sSQL = sSQL & "WHERE pmnavxm_process_id = " & CStr(lPMNavXMProcessID)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetProcessVersionID", bStoredProcedure:=False, lNumberRecords:=lNumRecs)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            lVersion = m_oDatabase.Records.Fields()("version_no")

            lVersion += 1


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetVersionNumber Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetVersionNumber", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetVersionDetails
    '
    ' Description: get version number, code and details for a process
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Public Function GetVersionDetails(ByVal lNavXMProcessVersionID As Integer, ByRef sCode As String, ByRef sDesc As String) As Integer

        Dim result As Integer = 0
        Dim lNumRecs As Integer
        Dim sSQL As String = ""
        Dim vTemp As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            sSQL = "SELECT version_number, code, description "
            sSQL = sSQL & "FROM pmnavxm_process_version "
            sSQL = sSQL & "WHERE version_id = " & CStr(lNavXMProcessVersionID)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetVersionDetails", bStoredProcedure:=False, lNumberRecords:=lNumRecs)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            vTemp = m_oDatabase.Records.Fields()("version_number")

            If Convert.IsDBNull(vTemp) Or IsNothing(vTemp) Then
                Return result
            End If

            sCode = m_oDatabase.Records.Fields()("code")
            sDesc = m_oDatabase.Records.Fields()("description")


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetVersionDetails failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetVersionDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SaveRoadMapVersion
    '
    ' Description: save a user copy of a roadmap
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Public Function SaveRoadMapVersion(ByVal lNavXMProcessID As Integer, ByRef lNavXMProcessVersionID As Integer, ByVal sCode As String, ByVal sDesc As String, ByVal sXML As String) As Integer

        Dim result As Integer = 0
        Dim lNumRecs As Integer
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            sXML = sXML.Replace("'", "''")
            sCode = sCode.Replace("'", "''")
            sDesc = sDesc.Replace("'", "''")

            If lNavXMProcessVersionID <> ID_NO_VALUE Then
                ' save
                sSQL = "UPDATE pmnavxm_process_version SET "
                sSQL = sSQL & "code = '" & sCode & "', "
                sSQL = sSQL & "description = '" & sDesc & "', "
                sSQL = sSQL & "xml_definition = '" & sXML & "' "
                sSQL = sSQL & "WHERE version_id = " & CStr(lNavXMProcessVersionID)
            Else
                ' save as
                sSQL = "SELECT ISNULL(MAX(version_id) + 1, 1) AS next_key FROM pmnavxm_process_version"

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetVersionDetails", bStoredProcedure:=False, lNumberRecords:=lNumRecs)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

                lNavXMProcessVersionID = m_oDatabase.Records.Fields()("next_key")

                sSQL = "INSERT INTO pmnavxm_process_version "
                sSQL = sSQL & "(version_id, pmnavxm_process_id, version_timestamp, "
                sSQL = sSQL & "version_number, code, description, xml_definition)"
                sSQL = sSQL & " VALUES ("
                sSQL = sSQL & "" & CStr(lNavXMProcessVersionID) & ", "
                sSQL = sSQL & "" & CStr(lNavXMProcessID) & ", "
                sSQL = sSQL & "'" & StringsHelper.Format(DateTime.Now, FORMAT_DATE) & "', "
                sSQL = sSQL & "" & CStr(1) & ", "
                sSQL = sSQL & "'" & sCode & "', "
                sSQL = sSQL & "'" & sDesc & "', "
                sSQL = sSQL & "'" & sXML & "')"

            End If


            Return m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="SaveRoadMapVersion", bStoredProcedure:=False)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveRoadMapVersion failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveRoadMapVersion", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetProcessVersionID
    '
    ' Description: get version number for specified process
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Public Function GetProcessVersionID(ByVal sCode As String, ByVal sDesc As String, ByVal lNavXMProcessID As Integer, ByRef lNavXMProcessVersionID As Integer) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim lNumRecs As Integer
        Dim sSQL As String = ""

        Try


            lNavXMProcessVersionID = ID_NO_VALUE

            sSQL = "SELECT version_id "
            sSQL = sSQL & "FROM pmnavxm_process_version "
            sSQL = sSQL & "WHERE pmnavxm_process_id = " & CStr(lNavXMProcessID)
            sSQL = sSQL & " AND code = '" & sCode & "'"
            sSQL = sSQL & " AND description = '" & sDesc & "'"

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetProcessVersionID", bStoredProcedure:=False, lNumberRecords:=lNumRecs)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If lNumRecs = 0 Then
                lNavXMProcessVersionID = -1
            Else
                lNavXMProcessVersionID = m_oDatabase.Records.Fields()("version_id")
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetProcessVersionID failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProcessVersionID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: PromoteVersion
    '
    ' Description: save a process as a new PMTask
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Public Function PromoteVersion(ByVal lNavXMProcessID As Integer, ByVal lNavXMProcessVersionID As Integer, ByVal sCode As String, ByVal sDesc As String, ByVal sXML As String, ByVal sNavFilePath As String) As Integer

        Dim result As Integer = 0
        Dim lFH, lCaptionID, lParentProcessID, lPMWrkTaskID, lNumRecs As Integer
        Dim sSQL, sFilename As String

        Dim vIsDeleted As String = ""
        Dim vIsSystemTask As String = ""
        Dim vTypeOfTask As String = ""
        Dim vAutoDeleteAfterNumDays As String = ""
        Dim vDisplayIcon As String = ""
        Dim vIsViewOnlyTask As String = ""
        Dim vLinkedCaptionID As String = ""
        Dim vIsAvailableTask As String = ""
        Dim vPMWrkTaskCategoryID As String = ""
        Dim vComponentObjectName As String = ""
        Dim vComponentClassName As String = ""
        Dim vLinkedObjectName As String = ""
        Dim vLinkedClassName As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' double-up apostrophes
            sXML = sXML.Replace("'", "''")

            ' whole process needs to be wrapped in a transaction
            ' rollback if any part fails
            m_lReturn = m_oDatabase.SQLBeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            ' add description to PMcaption table
            sSQL = "spu_pm_caption " & m_iLanguageID & ", '" & sDesc & "'"

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetTaskCaption", bStoredProcedure:=False, lNumberRecords:=lNumRecs)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            lCaptionID = m_oDatabase.Records.Fields()("caption_id")

            ' get parent task details
            sSQL = "SELECT * FROM PMWrk_Task "
            sSQL = sSQL & "WHERE pmnavxm_process_id = " & CStr(lNavXMProcessID)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetTaskDetails", bStoredProcedure:=False, lNumberRecords:=lNumRecs)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            ' parent task details
            vIsDeleted = m_oDatabase.Records.Fields()("is_deleted")
            vIsSystemTask = m_oDatabase.Records.Fields()("is_system_task")
            vTypeOfTask = m_oDatabase.Records.Fields()("type_of_task")
            vComponentObjectName = m_oDatabase.Records.Fields()("component_object_name")
            vComponentClassName = m_oDatabase.Records.Fields()("component_class_name")
            vAutoDeleteAfterNumDays = m_oDatabase.Records.Fields()("auto_delete_after_num_days")
            vDisplayIcon = m_oDatabase.Records.Fields()("display_icon")
            vIsViewOnlyTask = m_oDatabase.Records.Fields()("is_view_only_task")
            vLinkedObjectName = m_oDatabase.Records.Fields()("linked_object_name")
            vLinkedClassName = m_oDatabase.Records.Fields()("linked_class_name")
            vLinkedCaptionID = m_oDatabase.Records.Fields()("linked_caption_id")
            vIsAvailableTask = m_oDatabase.Records.Fields()("is_available_task")
            vPMWrkTaskCategoryID = m_oDatabase.Records.Fields()("pmwrk_task_category_id")

            ' get next process_id
            sSQL = "SELECT ISNULL(MAX(pmnavxm_process_id) + 1, 1) AS next_process_key "
            sSQL = sSQL & "FROM PMNavXM_Process"

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetNextProcessKey", bStoredProcedure:=False, lNumberRecords:=lNumRecs)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            lParentProcessID = lNavXMProcessID
            lNavXMProcessID = m_oDatabase.Records.Fields("next_process_key")

            ' new filename for the XML file
            sFilename = sCode & ".XML"

            ' write the file
            lFH = FileSystem.FreeFile()

            FileSystem.FileOpen(lFH, sNavFilePath & sFilename, OpenMode.Output)

            FileSystem.PrintLine(lFH, sXML)

            FileSystem.FileClose(lFH)

            ' add roadmap to PMNavXM_Process
            sSQL = "INSERT INTO pmnavxm_process ("
            sSQL = sSQL & "pmnavxm_process_id, parent_id, file_name, file_version_number, "
            sSQL = sSQL & "file_timestamp, is_custom, is_core, xml_definition) "
            sSQL = sSQL & "VALUES ("
            sSQL = sSQL & "" & CStr(lNavXMProcessID) & ", "
            sSQL = sSQL & "" & CStr(lParentProcessID) & ", "
            sSQL = sSQL & "'" & sFilename & "', "
            sSQL = sSQL & "1, "
            sSQL = sSQL & "'" & StringsHelper.Format(DateTime.Now, FORMAT_DATE) & "', "
            sSQL = sSQL & "" & CStr(1) & ", "
            sSQL = sSQL & "" & CStr(0) & ", "
            sSQL = sSQL & "'" & sXML & "')"

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="AddRoadmap", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            ' get next task id
            sSQL = "SELECT ISNULL(MAX(pmwrk_task_id) + 1, 1) AS next_task_key "
            sSQL = sSQL & "FROM PMWrk_task"

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetNextTaskKey", bStoredProcedure:=False, lNumberRecords:=lNumRecs)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            lPMWrkTaskID = m_oDatabase.Records.Fields("next_task_key")

            ' add to PMWrk_Task
            sSQL = "INSERT INTO pmwrk_task ("
            sSQL = sSQL & "pmwrk_task_id, caption_id, code, description, "
            sSQL = sSQL & "is_deleted, effective_date, is_system_task, type_of_task, "
            sSQL = sSQL & "pmnav_process_id, component_object_name, component_class_name, "
            sSQL = sSQL & "auto_delete_after_num_days, display_icon, is_view_only_task, "
            sSQL = sSQL & "linked_object_name, linked_class_name, linked_caption_id, is_available_task, "
            sSQL = sSQL & "pmwrk_task_category_id, pmnavxm_process_id) "
            sSQL = sSQL & "VALUES ("
            sSQL = sSQL & "" & CStr(lPMWrkTaskID) & ", "
            sSQL = sSQL & "" & CStr(lCaptionID) & ", "
            sSQL = sSQL & "'" & sCode & "', "
            sSQL = sSQL & "'" & sDesc & "', "

            If Convert.IsDBNull(vIsDeleted) Or IsNothing(vIsDeleted) Then
                sSQL = sSQL & "NULL, "
            Else
                sSQL = sSQL & "" & vIsDeleted & ", "
            End If

            sSQL = sSQL & "'" & StringsHelper.Format(DateTime.Now, FORMAT_DATE) & "', "

            If Convert.IsDBNull(vIsSystemTask) Or IsNothing(vIsSystemTask) Then
                sSQL = sSQL & "NULL, "
            Else
                sSQL = sSQL & "" & vIsSystemTask & ", "
            End If

            If Convert.IsDBNull(vTypeOfTask) Or IsNothing(vTypeOfTask) Then
                sSQL = sSQL & "NULL, "
            Else
                sSQL = sSQL & "" & vTypeOfTask & ", "
            End If

            sSQL = sSQL & "NULL, "

            If Convert.IsDBNull(vComponentObjectName) Or IsNothing(vComponentObjectName) Then
                sSQL = sSQL & "NULL, "
            Else
                sSQL = sSQL & "'" & vComponentObjectName & "', "
            End If

            If Convert.IsDBNull(vComponentClassName) Or IsNothing(vComponentClassName) Then
                sSQL = sSQL & "NULL, "
            Else
                sSQL = sSQL & "'" & vComponentClassName & "', "
            End If

            If Convert.IsDBNull(vAutoDeleteAfterNumDays) Or IsNothing(vAutoDeleteAfterNumDays) Then
                sSQL = sSQL & "NULL, "
            Else
                sSQL = sSQL & "" & vAutoDeleteAfterNumDays & ", "
            End If

            If Convert.IsDBNull(vDisplayIcon) Or IsNothing(vDisplayIcon) Then
                sSQL = sSQL & "NULL, "
            Else
                sSQL = sSQL & "" & vDisplayIcon & ", "
            End If

            If Convert.IsDBNull(vIsViewOnlyTask) Or IsNothing(vIsViewOnlyTask) Then
                sSQL = sSQL & "NULL, "
            Else
                sSQL = sSQL & "" & vIsViewOnlyTask & ", "
            End If

            If Convert.IsDBNull(vLinkedObjectName) Or IsNothing(vLinkedObjectName) Then
                sSQL = sSQL & "NULL, "
            Else
                sSQL = sSQL & "'" & vLinkedObjectName & "', "
            End If

            If Convert.IsDBNull(vLinkedClassName) Or IsNothing(vLinkedClassName) Then
                sSQL = sSQL & "NULL, "
            Else
                sSQL = sSQL & "'" & vLinkedClassName & "', "
            End If

            If Convert.IsDBNull(vLinkedCaptionID) Or IsNothing(vLinkedCaptionID) Then
                sSQL = sSQL & "NULL, "
            Else
                sSQL = sSQL & "" & vLinkedCaptionID & ", "
            End If

            If Convert.IsDBNull(vIsAvailableTask) Or IsNothing(vIsAvailableTask) Then
                sSQL = sSQL & "NULL, "
            Else
                sSQL = sSQL & "" & vIsAvailableTask & ", "
            End If

            If Convert.IsDBNull(vPMWrkTaskCategoryID) Or IsNothing(vPMWrkTaskCategoryID) Then
                sSQL = sSQL & "NULL, "
            Else
                sSQL = sSQL & "" & vPMWrkTaskCategoryID & ", "
            End If

            sSQL = sSQL & "" & CStr(lNavXMProcessID) & ")"

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="AddWrkTask", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            sSQL = "DELETE FROM PMNavXM_Process_Version "
            sSQL = sSQL & "WHERE version_id = " & CStr(lNavXMProcessVersionID)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="DeleteProcessVersion", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            ' commit the changes
            m_lReturn = m_oDatabase.SQLCommitTrans()


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' back out any database changes
            m_oDatabase.SQLRollbackTrans()

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PromoteVersion failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PromoteVersion", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetTaskCodes
    '
    ' Description: get all codes from PMWrk_Task
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Public Function GetTaskCodes(ByRef vTaskCodes(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lNumRecs As Integer
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            sSQL = "SELECT code FROM PMWrk_Task"

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetTaskCaption", bStoredProcedure:=False, lNumberRecords:=lNumRecs, vResultArray:=vTaskCodes)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTaskCodes failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTaskCodes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetVersionCodes
    '
    ' Description: get version codes
    '
    ' History: RDC 02082003 created
    ' ***************************************************************** '
    Public Function GetVersionCodes(ByVal lMode As Integer, ByRef vCodes(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lNumRecs As Integer
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            If lMode = MSG_MODE_NEWTASK Then
                sSQL = "SELECT DISTINCT(code) FROM PMWrk_Task"
            Else
                sSQL = "SELECT DISTINCT(code) FROM PMNavXM_Process_Version"
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetVersionCodes", bStoredProcedure:=False, lNumberRecords:=lNumRecs, vResultArray:=vCodes)


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetVersionCodes failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetVersionCodes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetUpdateLog
    '
    ' Description: get roadmap update information
    '
    ' History: RDC 11082003 created
    ' ***************************************************************** '
    Public Function GetUpdateLog(ByRef vUpdateLog(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lNumRecs As Integer
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            sSQL = "SELECT update_id, update_timestamp, install_version, file_name, "
            sSQL = sSQL & "parent_code, parent_desc, child_code, child_desc "
            sSQL = sSQL & "FROM pmnavxm_roadmap_update "
            sSQL = sSQL & "ORDER BY update_id"

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetUpdateLog", bStoredProcedure:=False, lNumberRecords:=lNumRecs, vResultArray:=vUpdateLog)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Information.IsArray(vUpdateLog) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUpdateLog failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUpdateLog", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function GetDocumentTemplateDesc(ByVal lDocTempID As Integer, ByRef vTemplates(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the UserId parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="document_template_id", vValue:=CStr(lDocTempID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDatatype:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetDocumentTemplateDescSQL, sSQLName:=ACGetDocumentTemplateDescName, bStoredProcedure:=ACGetDocumentTemplateDescStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vTemplates)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Information.IsArray(vTemplates) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDocumentTemplateDesc Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocumentTemplateDesc", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
End Class
