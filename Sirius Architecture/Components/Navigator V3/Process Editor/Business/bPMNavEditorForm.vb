Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Text
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Form_NET.Form")> _
Public NotInheritable Class Form
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Form
    '
    ' Date: 12/08/1998
    '
    ' Description: Creatable Form class which contains all the
    '              methods, Form rules required to manipulate
    '              a bPMNavEditor.
    '
    ' Edit History:
    ' ***************************************************************** '


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


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Form"

    'Max number of Map Levels in Process Build
    Private Const ACMaxMapLevels As Integer = 25

    ' PRIVATE Data Members (Begin)

    'The Database DSN
    Private m_sDSN As String = ""

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    'Source database used for copy if different to current database
    Private m_oSourceDatabase As dPMDAO.Database

    'Flag to indicate copy mode
    Private m_bCopyFromDB As Boolean

    'Flag to show the step's component
    Private m_bStepComponent As Boolean

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

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

    ' PRIVATE Data Members (End)

    'Current Process Map
    Private cReturnProcessMap As Collection

    'Current Map Keys
    Private cCurrentMapKeys As Collection

    'Private counter for unique process map entry key
    Private m_lProcessMapCount As Integer

    'Message log created when enforcing rules (IMPORTANT)
    Private m_sRuleLog As String = ""

    'Message log when Copying
    Private m_sCopyLog As String = ""

    'The Current Process being edited (used by Step Rules)
    Private m_lCurrentProcessID As Integer

    'IDs used for inserting Steps and Maps
    Private m_lInsertStepMapID As Integer
    Private m_lInsertStepStepID As Integer

    ' ******************************************************************
    'Flag for Copy Destination Output to
    'Script or to Destination Database
    Private m_bBuildCopyScript As Boolean

    'Script comments
    Private m_bCopyScriptComments As Boolean

    'The Script Variables
    Private m_sCopyScript As String = ""
    Private m_lCopyScriptPtr As Integer
    Private m_vCopyScript() As Object

    'Created Map, Component, Keys and Processes Steps
    Private m_cProcessIDs As Collection
    Private m_cMapIDs As Collection
    Private m_cComponentIDs As Collection
    Private m_cKeyIDs As Collection

    'Count number of lookups
    Private m_lLookUpCount As Integer
    Private m_lInsertRecordCount As Integer

    ' ***************************************************************** '
    ' Name: UpdateMapEffectiveDate (Public Method)
    '
    ' Description: Update a Map's effective date to today
    '
    ' ***************************************************************** '
    Private Function UpdateMapEffectiveDate(ByVal v_lMapID As Integer) As Integer

        Dim result As Integer = 0
        Dim sSql As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        'Make sure that the map is not 0
        If v_lMapID = 0 Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'SQL to Update the Map with today's effective date
        sSql = "UPDATE PMNav_Map SET effective_date = " & _
               "'" & DateTime.Today.ToString("MM/dd/yyyy") & "' " & _
                   "WHERE pmnav_map_id = " & CStr(v_lMapID)

        'Execute SQL
        m_lReturn = m_oDatabase.SQLAction(sSql:=sSql, sSQLName:="UpdateStepMap", bStoredProcedure:=False)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result


    End Function
    ' ******************************************************************* '
    ' Name : BuildMapKeys (Private)
    '
    ' Description : Builds all the Keys Contained in a Start Map
    '               From a Start Map up to a Step
    '               (CurrentMapKeys must be initialied before entry)
    ' ******************************************************************* '
    Private Function BuildMapKeys(ByVal v_lMapID As Integer, Optional ByRef v_vUptoMapID As String = "", Optional ByRef v_vUptoStepID As String = "") As Integer

        Dim result As Integer = 0
        Dim vStepArray(,) As Object
        Dim vResultSet(,) As Object
        Dim vStepGetKeys(,) As Object
        Dim sSql As String = ""
        Dim lStepID As Integer

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            'Get all the Map SetKeys
            sSql = "SELECT pmnav_key_id FROM PMNav_Set_Map_Key " & _
                   "WHERE pmnav_map_id = " & CStr(v_lMapID)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSql, sSQLName:="GetMapKeys", bStoredProcedure:=False, vResultArray:=vResultSet)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'Add the Map Set-Keys to the list of MapKeys
            If Information.IsArray(vResultSet) Then
                Try
                    For iPtr As Integer = vResultSet.GetLowerBound(1) To vResultSet.GetUpperBound(1)
                        cCurrentMapKeys.Add(CInt(vResultSet(0, iPtr)), CStr(vResultSet(0, iPtr)))
                    Next iPtr
                Catch ex As Exception

                End Try
            End If

            'Get all the Steps for this Map
            sSql = "SELECT pmnav_step_id, description, sub_nav_map_id " & _
                   "FROM PMNav_Step WHERE pmnav_map_id = " & CStr(v_lMapID) & _
                   " ORDER BY pmnav_step_id"

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSql, sSQLName:="GetSteps", bStoredProcedure:=False, vResultArray:=vStepArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Information.IsArray(vStepArray) Then
                Return result
            End If

            'Loop through all Steps and get any submaps or get keys


            For iPtr As Integer = vStepArray.GetLowerBound(1) To vStepArray.GetUpperBound(1)

                'Get the StepID

                lStepID = CInt(vStepArray(0, iPtr))

                'If this is the Step that we are building up to then exit function
                If (Conversion.Val(v_vUptoMapID) = v_lMapID) And (lStepID = Conversion.Val(v_vUptoStepID)) Then
                    m_lReturn = gPMConstants.PMEReturnCode.PMCancel
                    Return result
                End If

                'Check if this is a Sub Map

                If CStr(vStepArray(2, iPtr)) <> "" Then

                    m_lReturn = CType(BuildMapKeys(CInt(vStepArray(2, iPtr)), v_vUptoMapID:=v_vUptoMapID, v_vUptoStepID:=v_vUptoStepID), gPMConstants.PMEReturnCode)

                    'Check if we have hit Cancel meaning we have built upto Step
                    If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                        Return gPMConstants.PMEReturnCode.PMCancel
                    End If

                Else

                    'Get all the StepGet Keys for Each Step and add to collection
                    sSql = "SELECT pmnav_key_id FROM PMNav_Get_Step_Key " & _
                           "WHERE pmnav_map_id = " & CStr(v_lMapID) & _
                           " AND pmnav_step_id = " & CStr(lStepID) & _
                           " ORDER BY pmnav_step_id "

                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSql, sSQLName:="GetStepGetKeys", bStoredProcedure:=False, vResultArray:=vStepGetKeys)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        'An error occurred, so exit
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Add the Step Get Keys to the Current Map keys list
                    If Information.IsArray(vStepGetKeys) Then


                        Try
                            For iPtr2 As Integer = vStepGetKeys.GetLowerBound(1) To vStepGetKeys.GetUpperBound(1)
                                cCurrentMapKeys.Add(CInt(vStepGetKeys(0, iPtr2)), CStr(vStepGetKeys(0, iPtr2)))
                            Next iPtr2
                        Catch ex As Exception

                        End Try

                    End If

                End If

            Next iPtr
        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BuildMapKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildMapKeys", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
        End Try

        Return result
    End Function
    Private Sub CopyLogSQLEnd(ByRef sText As String)


        'Push on to Stack

        Dim sSql As String = "Insert INTO ##TempStack(" & _
                             "pmproduct_id," & _
                             "pmproc_lock_group_id," & _
                             "transaction_type_id," & _
                             "caption_id," & _
                             "pmnav_key_id," & _
                             "pmnav_component_id," & _
                             "ok_nav_process_id," & _
                             "cancel_nav_process_id," & _
                             "sub_nav_map_id," & _
                             "pmnav_process_id," & _
                             "pmnav_map_id," & _
                             "pmnav_step_id," & _
                             "sub_component_id," & _
                             "effective_date," & _
                             "effective_date2) "

        sSql = sSql & "VALUES(" & _
               "@pmproduct_id," & _
               "@pmproc_lock_group_id," & _
               "@transaction_type_id," & _
               "@caption_id," & _
               "@pmnav_key_id," & _
               "@pmnav_component_id," & _
               "@ok_nav_process_id," & _
               "@cancel_nav_process_id," & _
               "@sub_nav_map_id," & _
               "@pmnav_process_id," & _
               "@pmnav_map_id," & _
               "@pmnav_step_id," & _
               "@sub_component_id," & _
               "@effective_date," & _
               "@effective_date2)"

        sText = sText & Strings.Chr(13) & Strings.Chr(10) & "/*PUSH VARIABLES ONTO STACK*/" & Strings.Chr(13) & Strings.Chr(10) & sSql & Strings.Chr(13) & Strings.Chr(10)
        sText = sText & "GO" & Strings.Chr(13) & Strings.Chr(10)

        sText = sText & _
                "/* DECLARE VARIABLES */" & Strings.Chr(13) & Strings.Chr(10) & _
                "DECLARE " & _
                "@pmnav_component_id int, " & _
                "@pmnav_process_id int, " & _
                "@pmnav_map_id int, " & _
                "@pmnav_step_id int, " & _
                "@pmproduct_id int, " & _
                "@pmproc_lock_group_id int, " & _
                "@transaction_type_id int, " & _
                "@caption_id int, " & _
                "@pmnav_key_id int, " & _
                "@ok_nav_process_id int, " & _
                "@cancel_nav_process_id int, " & _
                "@sub_nav_map_id int, " & _
                "@sub_component_id int, " & _
                "@temp_id int, " & _
                "@tempstack_id int, " & _
                "@effective_date datetime, " & _
                "@effective_date2 datetime " & Strings.Chr(13) & Strings.Chr(10)

        sText = sText & "/*PULL VARIABLES OFF STACK*/" & Strings.Chr(13) & Strings.Chr(10)

        'Get the stack values
        sText = sText & "Select " & _
                "@pmproduct_id = pmproduct_id, " & _
                "@pmproc_lock_group_id = pmproc_lock_group_id, " & _
                "@transaction_type_id = transaction_type_id, " & _
                "@caption_id = caption_id, " & _
                "@pmnav_key_id = pmnav_key_id, " & _
                "@pmnav_component_id = pmnav_component_id, " & _
                "@ok_nav_process_id = ok_nav_process_id, " & _
                "@cancel_nav_process_id = cancel_nav_process_id, " & _
                "@sub_nav_map_id = sub_nav_map_id, " & _
                "@pmnav_process_id = pmnav_process_id, " & _
                "@pmnav_map_id = pmnav_map_id, " & _
                "@pmnav_step_id = pmnav_step_id, " & _
                "@tempstack_id = tempstack_id, " & _
                "@sub_component_id = sub_component_id, " & _
                "@effective_date = effective_date, " & _
                "@effective_date2 = effective_date2 " & _
                "From ##Tempstack " & _
                "WHERE tempstack_id = (SELECT Max(tempstack_id) FROM ##TempStack)" & Strings.Chr(13) & Strings.Chr(10) & _
                "DELETE FROM ##TempStack WHERE tempstack_id = @tempstack_id"

        CopyLogSQL(sText, NavProcConst.ACSQLStatement)

    End Sub

    Public ReadOnly Property CopyScript() As Object
        Get

            Return VB6.CopyArray(m_vCopyScript)

        End Get
    End Property

    Public WriteOnly Property BuildCopyScript() As Boolean
        Set(ByVal Value As Boolean)

            m_bBuildCopyScript = Value

        End Set
    End Property

    Public WriteOnly Property CopyScriptComments() As Boolean
        Set(ByVal Value As Boolean)

            m_bCopyScriptComments = Value

        End Set
    End Property
    Public WriteOnly Property CurrentProcessID() As Integer
        Set(ByVal Value As Integer)

            m_lCurrentProcessID = Value

        End Set
    End Property

    Public Property DSN() As String
        Get

            Return m_sDSN

        End Get
        Set(ByVal Value As String)

            m_sDSN = Value

        End Set
    End Property
    Public ReadOnly Property InsertRecordCount() As Integer
        Get

            Return m_lInsertRecordCount

        End Get
    End Property

    Public ReadOnly Property LookupCount() As Integer
        Get

            Return m_lLookUpCount

        End Get
    End Property

    Public ReadOnly Property RuleLog() As String
        Get

            Return m_sRuleLog

        End Get
    End Property


    Public ReadOnly Property CopyLog() As String
        Get

            Return m_sCopyLog

        End Get
    End Property


    Public WriteOnly Property StepComponent() As Boolean
        Set(ByVal Value As Boolean)

            m_bStepComponent = Value

        End Set
    End Property
    Public ReadOnly Property ProcessMap() As Collection
        Get

            Return cReturnProcessMap


        End Get
    End Property

    Public ReadOnly Property Task() As Integer
        Get

            Return m_iTask

        End Get
    End Property

    Public ReadOnly Property Navigate() As Integer
        Get

            Return m_lNavigate

        End Get
    End Property

    Public ReadOnly Property ProcessMode() As Integer
        Get

            Return m_lProcessMode

        End Get
    End Property

    Public ReadOnly Property TransactionType() As String
        Get

            Return m_sTransactionType

        End Get
    End Property

    Public ReadOnly Property EffectiveDate() As Date
        Get

            Return m_dtEffectiveDate

        End Get
    End Property

    Public ReadOnly Property StepStatus() As String
        Get

            ' Standard Property.

            ' Return the Steps Status
            Return m_sStepStatus.Value

        End Get
    End Property

    Private Function CopySQLExists(Optional ByRef vProcessID As Object = Nothing, Optional ByRef vMapID As Object = Nothing, Optional ByRef vComponentID As Object = Nothing, Optional ByRef vKeyID As Object = Nothing) As Boolean

        Dim result As Boolean = False
        Dim lID As Integer

        Try

            result = True


            If Not Information.IsNothing(vProcessID) Then


                lID = CInt(m_cProcessIDs.Item(CStr(vProcessID)))
            ElseIf (Not Information.IsNothing(vMapID)) Then


                lID = CInt(m_cMapIDs.Item(CStr(vMapID)))
            ElseIf (Not Information.IsNothing(vComponentID)) Then


                lID = CInt(m_cComponentIDs.Item(CStr(vComponentID)))
            ElseIf (Not Information.IsNothing(vKeyID)) Then


                lID = CInt(m_cKeyIDs.Item(CStr(vKeyID)))
            End If

            Return result

        Catch


            Return False
        End Try


    End Function

    ' ***************************************************************** '
    '
    ' Name: Copy (Public)
    '
    ' Description: Copies an Entire Process or Map
    '
    ' ***************************************************************** '
    Public Function Copy(ByVal v_sPMNavGroup As String, ByVal v_sDSN As String, ByVal v_lID As Integer, ByVal v_lNewID As Integer, ByVal v_sDescription As String, ByVal v_sSourceName As String) As Integer

        Dim result As Integer = 0
        Dim vResultSet As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear CopyLog
            CopyLogClear()


            'Make sure prior Databases are closed
            If Not (m_oSourceDatabase Is Nothing) Then

                'If Source is another open database then close
                If m_bCopyFromDB Then
                    m_oSourceDatabase.CloseDatabase()
                End If

                m_oSourceDatabase = Nothing
            End If

            'Check if the Source DSN is different from the current one
            If v_sDSN = m_sDSN Then

                CopyLogText("Copying from current database...")

                'Not copying from another database
                m_bCopyFromDB = False

                'Source database is same as destination
                m_oSourceDatabase = m_oDatabase

            Else

                CopyLogText("Copying from Database " & v_sDSN & "...")

                'Copy from another Database
                m_bCopyFromDB = True

                ' Open the Source Database

                m_oSourceDatabase = New dPMDAO.Database()
                ' RDC 27062002 use Comp Serv to open database
                m_lReturn = CType(gPMComponentServices.NewDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_oDatabase:=m_oSourceDatabase), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    CopyLogText("OpenDatabase " & v_sDSN & " failed.")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            'Call correct copy function
            If v_sPMNavGroup = NavProcConst.NavGrpProcess Then

                CopyLogText("Copying Process " & _
                            v_sSourceName & " To " & v_sDescription)

                'Copy the selected Process
                v_lNewID = CopyProcess(v_lProcessID:=v_lID, v_sNewProcess:=v_sDescription)

            ElseIf (v_sPMNavGroup = NavProcConst.NavGrpMap) Then

                'Check if we are outputting to a script
                If Not m_bBuildCopyScript Then

                    CopyLogText("Copying Map '" & _
                                v_sSourceName & "' To '" & v_sDescription & "'")

                End If

                'Copy the selected map
                v_lNewID = CopyMap(v_lMapID:=v_lID, v_sNewMap:=v_sDescription)

            End If

            If v_lNewID = 0 Then
                CopyLogText("Copy Failed.")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            CopyLogText("Copy Complete.")

            'If we are building a copy script then complete it
            If m_bBuildCopyScript Then
                CopyLogSQL("", NavProcConst.ACSQLComplete)
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Copy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Copy", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Private Sub CopyLogSQL(ByRef sSql As String, ByRef iSQLMode As Integer)

        Dim sOutputText As String = ""
        Static bBlockOn As Boolean




        Select Case iSQLMode
            Case NavProcConst.ACSQLStart

                'Add start information to Copy Script

            Case NavProcConst.ACSQLComment

                'Check if copy script comments are on
                If Not m_bCopyScriptComments Then
                    Exit Sub
                End If

                'SQL Script Comment
                sOutputText = Strings.Chr(13) & Strings.Chr(10) & "/*" & sSql.ToUpper() & "*/" & Strings.Chr(13) & Strings.Chr(10)

            Case NavProcConst.ACSQLStatement
                'insert a statement
                sOutputText = NavProcConst.ACTAB & sSql & Strings.Chr(13) & Strings.Chr(10)

            Case NavProcConst.ACSQLComplete

                'Create the headers and footers for all created scripts
                CreateSQLFileHeadersANDFooters()
                Exit Sub

            Case NavProcConst.ACSQLBegin
                sOutputText = sOutputText & NavProcConst.ACTAB & "BEGIN" & Strings.Chr(13) & Strings.Chr(10)

            Case Else
                sOutputText = sSql

        End Select

        'Handle Blocks
        If sSql.IndexOf("BEGIN") >= 0 Then
            bBlockOn = True
        ElseIf (sSql.IndexOf("END") >= 0) Then

            bBlockOn = False
        End If

        'Append new SQL Script commands
        CopyLogSQLAddText(sOutputText)

        'Check if we are in a block, do not split blocks accross  files
        If Not bBlockOn Then

            'Make sure the files do not exceed maximum length
            CopyLogSQLCheckFileLength()

        End If

    End Sub

    Private Sub CopyLogSQLAddText(ByRef sOutputText As String, Optional ByRef sHeader As String = "")


        m_vCopyScript(m_lCopyScriptPtr) = sHeader & CStr(m_vCopyScript(m_lCopyScriptPtr)) & sOutputText


    End Sub

    Private Sub CopyLogSQLCheckFileLength()

        If Strings.Len(CStr(m_vCopyScript(m_lCopyScriptPtr))) >= 50000 Then
            m_lCopyScriptPtr += 1
        End If

    End Sub

    Public Function CopyLogSQLClear() As Integer

        'Clear Script Files
        m_sCopyScript = ""
        m_lCopyScriptPtr = 0

        ReDim m_vCopyScript(100)

        'Create the IDs collections
        m_cProcessIDs = Nothing
        m_cMapIDs = Nothing
        m_cComponentIDs = Nothing
        m_cKeyIDs = Nothing

        m_cProcessIDs = New Collection()
        m_cMapIDs = New Collection()
        m_cComponentIDs = New Collection()
        m_cKeyIDs = New Collection()

        'Clear Count variables
        m_lLookUpCount = 0
        m_lInsertRecordCount = 0

    End Function

    ' ***************************************************************** '
    ' Name: CopyLogSQLStack (Private)
    '
    ' Description: Stack functions for the Copyoutput SQL Script
    '
    ' ***************************************************************** '
    Private Sub CopyLogSQLStack(ByRef sAction As String)

        Dim sSql As String

        Try

            'Is this a push or pull
            If sAction = NavProcConst.ACSQLPush Then

                'Push variables onto Stack

                sSql = "Insert INTO ##TempStack(" & _
                       "pmproduct_id," & _
                       "pmproc_lock_group_id," & _
                       "transaction_type_id," & _
                       "caption_id," & _
                       "pmnav_key_id," & _
                       "pmnav_component_id," & _
                       "ok_nav_process_id," & _
                       "cancel_nav_process_id," & _
                       "sub_nav_map_id," & _
                       "pmnav_process_id," & _
                       "pmnav_map_id," & _
                       "pmnav_step_id," & _
                       "sub_component_id," & _
                       "effective_date2) "

                sSql = sSql & "VALUES(" & _
                       "@pmproduct_id," & _
                       "@pmproc_lock_group_id," & _
                       "@transaction_type_id," & _
                       "@caption_id," & _
                       "@pmnav_key_id," & _
                       "@pmnav_component_id," & _
                       "@ok_nav_process_id," & _
                       "@cancel_nav_process_id," & _
                       "@sub_nav_map_id," & _
                       "@pmnav_process_id," & _
                       "@pmnav_map_id," & _
                       "@pmnav_step_id," & _
                       "@sub_component_id," & _
                       "@effective_date2)"

                CopyLogSQL("PUSH VARIABLES ONTO STACK", NavProcConst.ACSQLComment)
                CopyLogSQL(sSql, NavProcConst.ACSQLStatement)

            ElseIf (sAction = NavProcConst.ACSQLPull) Then

                CopyLogSQL("PULL VARIABLES OFF STACK", NavProcConst.ACSQLComment)

                'Get the stack values
                CopyLogSQL("Select " & _
                           "@pmproduct_id = pmproduct_id, " & _
                           "@pmproc_lock_group_id = pmproc_lock_group_id, " & _
                           "@transaction_type_id = transaction_type_id, " & _
                           "@caption_id = caption_id, " & _
                           "@pmnav_key_id = pmnav_key_id, " & _
                           "@pmnav_component_id = pmnav_component_id, " & _
                           "@ok_nav_process_id = ok_nav_process_id, " & _
                           "@cancel_nav_process_id = cancel_nav_process_id, " & _
                           "@sub_nav_map_id = sub_nav_map_id, " & _
                           "@pmnav_process_id = pmnav_process_id, " & _
                           "@pmnav_map_id = pmnav_map_id, " & _
                           "@pmnav_step_id = pmnav_step_id, " & _
                           "@tempstack_id = tempstack_id, " & _
                           "@sub_component_id = sub_component_id, " & _
                           "@effective_date2 = effective_date2 " & _
                           "From ##Tempstack " & _
                           "WHERE tempstack_id = (SELECT Max(tempstack_id) FROM ##TempStack)", NavProcConst.ACSQLStatement)

                'Remove the stack entry

                CopyLogSQL("DELETE FROM ##TempStack WHERE tempstack_id = @tempstack_id", NavProcConst.ACSQLStatement)

            End If

        Catch
        End Try




    End Sub


    ' ***************************************************************** '
    ' Name: CopyProcess (Private)
    '
    ' Description: Copies an Entire ProcessMap to a New Process
    '
    ' ***************************************************************** '
    Private Function CopyProcess(ByVal v_lProcessID As Integer, ByVal v_sNewProcess As String) As Integer

        Dim result As Integer = 0
        Dim vResultSet(,) As Object
        Dim lCaptionID As Integer
        Dim sFieldList, sFieldValues, sKeyField, sSql As String
        Dim lMapID, lNewMapID As Integer
        Dim sMapDescription As String = ""
        Dim v_lNewID As gPMConstants.PMEReturnCode
        Dim vProductID As gPMConstants.PMEReturnCode
        Dim vTransactionType As gPMConstants.PMEReturnCode




        'Set output script variables
        If m_bBuildCopyScript Then

            'Add id of Process to lookup
            m_cProcessIDs.Add(v_lProcessID, CStr(v_lProcessID))

        End If

        CopyLogText("Getting Process Start Map...")

        'Get the Start Map for this process
        sSql = "SELECT start_nav_map_id, description FROM PMNav_Process " & _
               "WHERE pmnav_process_id = " & CStr(v_lProcessID)

        m_lReturn = m_oSourceDatabase.SQLSelect(sSql:=sSql, sSQLName:="GetProcessStartMap", bStoredProcedure:=False, vResultArray:=vResultSet)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyLogText("Failed to retrieve start map for process.")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Information.IsArray(vResultSet) Then
            CopyLogText("Start Map does not exist for this process.")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        lMapID = CInt(Conversion.Val(CStr(vResultSet(0, 0))))

        sMapDescription = CStr(vResultSet(1, 0))

        'Copy the map to a new map, using the Process Description
        CopyLogText("Copying StartMap...")
        lNewMapID = CopyMap(v_lMapID:=lMapID, v_sNewMap:=sMapDescription)

        If lNewMapID = 0 Then
            CopyLogText("Failed to Copy StartMap.")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Copy the Process details to a new Process

        'Get the Caption for a new Process
        CopyLogText("Getting Caption for the Process.")
        lCaptionID = UpdateFieldLookUp("PMCaption", 0, v_sNewProcess)

        ' Check that we have a Caption
        If lCaptionID = 0 Then
            CopyLogText("Failed to Get Caption the Process")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Get the details for this Process into memory
        CopyLogText("Getting Process Source details")

        sSql = "SELECT " & _
               "pmproduct_id, " & _
               "code, " & _
               lCaptionID & " CapID, " & _
               "'" & v_sNewProcess & "' Description, " & _
               "pmproc_lock_group_id, " & _
               "transaction_type_id, " & _
               "process_mode, " & _
               lNewMapID & " StartMap, " & _
               "is_logged , " & _
               "is_user_driven, " & _
               "is_deleted, " & _
               "effective_date " & _
               "FROM PMNav_Process WHERE pmnav_process_id = " & CStr(v_lProcessID)

        m_lReturn = m_oSourceDatabase.SQLSelect(sSql:=sSql, sSQLName:="GetSourceProcess", bStoredProcedure:=False, vResultArray:=vResultSet)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyLogText("Failed to Get Process Source details.")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Make sure we have the Process
        If Not Information.IsArray(vResultSet) Then
            CopyLogText("No details exist for the Process.")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'List of Process Fields
        sFieldList = "pmproduct_id, code, caption_id, " & _
                     "description, pmproc_lock_group_id, " & _
                     "transaction_type_id, process_mode, " & _
                     "start_nav_map_id, is_logged, " & _
                     "is_user_driven, is_deleted, " & _
                     "effective_date "

        'List of Process Values
        If m_bBuildCopyScript Then

            vResultSet(2, 0) = "@caption_id"

            vResultSet(7, 0) = "@pmnav_map_id"
        End If

        'Save the caption if this is copy output
        If m_bBuildCopyScript Then
            CopyLogSQL("SAVE PROCESS CAPTION", NavProcConst.ACSQLComment)
            CopyLogSQL("Select @temp_id = @caption_id", NavProcConst.ACSQLStatement)
        End If

        'Get Lookups

        vProductID = DSNSourceLookUp("pmproduct_id", vResultSet(0, 0))

        vTransactionType = DSNSourceLookUp("transaction_type_id", vResultSet(5, 0))

        'Restore caption if this is copy output
        If m_bBuildCopyScript Then
            CopyLogSQL("RESTORE PROCESS CAPTION", NavProcConst.ACSQLComment)
            CopyLogSQL("Select @caption_id  = @temp_id", NavProcConst.ACSQLStatement)
        End If


        sFieldValues = CStr(vProductID) & ", " & _
                       "'" & CStr(vResultSet(1, 0)).Trim() & "', " & _
                       CStr(vResultSet(2, 0)) & ", " & _
                       "'" & CStr(vResultSet(3, 0)).Trim() & "', " & _
                       "Null, " & _
                       vTransactionType & ", " & _
                       CStr(vResultSet(6, 0)) & ", " & _
                       CStr(vResultSet(7, 0)) & ", " & _
                       CStr(vResultSet(8, 0)) & ", " & _
                       CStr(vResultSet(9, 0)) & ", " & _
                       CStr(vResultSet(10, 0)) & ", " & _
                       "'" & CDate(vResultSet(11, 0)).ToString("MM/dd/yyyy") & "'"


        If m_bBuildCopyScript Then

            'Check if the process exists
            CopyLogSQL("Check if we have this Process on database", NavProcConst.ACSQLComment)
            CopyLogSQL("Select @temp_id = Null", NavProcConst.ACSQLStatement)

            CopyLogSQL("Select @temp_id = pmnav_process_id FROM PMNav_Process " & _
                       "WHERE code = '" & CStr(vResultSet(1, 0)).Trim() & "' AND description = '" & v_sNewProcess.Trim() & "'", NavProcConst.ACSQLStatement)


        End If

        'Insert the fields
        CopyLogText("Inserting Destination Process Fields...")
        v_lNewID = CType(InsertFields(sPMNavGroup:=NavProcConst.NavGrpProcess, sFieldNames:=sFieldList, sFieldValues:=sFieldValues), gPMConstants.PMEReturnCode)

        If v_lNewID = gPMConstants.PMEReturnCode.PMFalse Then
            CopyLogText("Failed to insert destination Process fields.")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Check if this Process already exists
        If m_bBuildCopyScript Then
            CopyLogSQL("If this Process exists, then update", NavProcConst.ACSQLComment)
            CopyLogSQL("IF @temp_id <> Null", NavProcConst.ACSQLStatement)
            CopyLogSQL(NavProcConst.ACTAB & "BEGIN", NavProcConst.ACSQLStatement)
            CopyLogSQL(NavProcConst.ACTAB & "DELETE FROM PMNav_Process WHERE pmnav_process_id = @pmnav_process_id", NavProcConst.ACSQLStatement)
            CopyLogSQL(NavProcConst.ACTAB & "SELECT @pmnav_process_id = @temp_id", NavProcConst.ACSQLStatement)
            CopyLogSQL(NavProcConst.ACTAB & "UPDATE PMNav_Process SET start_nav_map_id = @pmnav_map_id WHERE pmnav_process_id = @pmnav_process_id", NavProcConst.ACSQLStatement)
            CopyLogSQL(NavProcConst.ACTAB & "END", NavProcConst.ACSQLStatement)

            'Remove the Process Keys
            CopyLogSQL("Remove the Existing Process Keys", NavProcConst.ACSQLComment)
            CopyLogSQL(NavProcConst.ACTAB & "DELETE FROM PMNav_Set_Process_Key WHERE pmnav_process_id = @pmnav_process_id", NavProcConst.ACSQLStatement)
            CopyLogSQL(NavProcConst.ACTAB & "DELETE FROM PMNav_Get_Process_Key WHERE pmnav_process_id = @pmnav_process_id", NavProcConst.ACSQLStatement)

        End If


        'The Key field used for copy
        sKeyField = "pmnav_process_id"

        'The list of fields
        sFieldList = "pmnav_key_id, sequence_number, " & _
                     "description, initial_key_value"

        'Copy the Process Set Keys to New Process
        CopyLogText("Copying SetKeys for this Process...")

        m_lReturn = CType(CopyTableRecords(v_sTableName:="PMNav_Set_Process_Key", v_sFields:=sFieldList, v_sKeyField:=sKeyField, v_lFieldID:=v_lProcessID, v_lNewFieldID:=v_lNewID), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyLogText("Failed to Copy SetKeys for this Process.")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'The list of fields is the only variable to change
        sFieldList = "pmnav_key_id, sequence_number, " & _
                     "description"

        'Copy the Process Get Keys to new Process
        CopyLogText("Copying GetKeys for this Process...")

        m_lReturn = CType(CopyTableRecords(v_sTableName:="PMNav_Get_Process_Key", v_sFields:=sFieldList, v_sKeyField:=sKeyField, v_lFieldID:=v_lProcessID, v_lNewFieldID:=v_lNewID), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyLogText("Failed to Copy GetKeys for this Process.")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Return v_lNewID

    End Function

    ' ***************************************************************** '
    ' Name: CopyComponent (Private)
    '
    ' Description: Copies an Entire Component to a New Component
    '
    ' *****************************************************************
    Private Function CopyComponent(ByVal v_lComponentID As Integer, ByVal v_sNewComponent As String) As Integer

        Dim result As Integer = 0
        Dim vResultSet(,) As Object
        Dim lCaptionID As Integer
        Dim sFieldList, sFieldValues, sKeyField, sSql As String
        Dim v_lNewID As gPMConstants.PMEReturnCode




        'Set output script variables
        If m_bBuildCopyScript Then

            'Set id of component
            m_cComponentIDs.Add(v_lComponentID, CStr(v_lComponentID))

        End If

        'Copy the Component details to a new Component

        'Get the Caption for a new Component
        CopyLogText("Getting Component '" & v_sNewComponent & "' Caption...")

        lCaptionID = UpdateFieldLookUp("PMCaption", 0, v_sNewComponent)

        ' Check that we have a Caption
        If lCaptionID = 0 Then
            CopyLogText("Failed to Get Component Caption.")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Get the details for this Component into memory
        CopyLogText("Getting Source Component...")

        sSql = "SELECT " & _
               "nav_component_type, " & _
               "object_name, " & _
               "class_name, " & _
               "'" & v_sNewComponent & "' Description, " & _
               lCaptionID & " CapID, " & _
               "is_server_side, " & _
               "is_deleted, " & _
               "effective_date " & _
               "FROM PMNav_Component WHERE pmnav_Component_id = " & CStr(v_lComponentID)

        m_lReturn = m_oSourceDatabase.SQLSelect(sSql:=sSql, sSQLName:="GetSourceComponent", bStoredProcedure:=False, vResultArray:=vResultSet)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyLogText("Failed to retrieve Source Component details.")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Make sure we have the Component
        If Not Information.IsArray(vResultSet) Then
            CopyLogText("No details exist for this Component.")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'List of Component Fields
        sFieldList = "nav_component_type, object_name, class_name, " & _
                     "description, caption_id, is_server_side, " & _
                     "is_deleted, effective_date "

        'check if we are building an output script
        If m_bBuildCopyScript Then

            vResultSet(4, 0) = "@caption_id"
        End If

        'List of Component Values

        sFieldValues = "'" & CStr(vResultSet(0, 0)) & "', " & _
                       "'" & CStr(vResultSet(1, 0)) & "', " & _
                       "'" & CStr(vResultSet(2, 0)) & "', " & _
                       "'" & CStr(vResultSet(3, 0)) & "', " & _
                       CStr(vResultSet(4, 0)) & ", " & _
                       CStr(vResultSet(5, 0)) & ", " & _
                       CStr(vResultSet(6, 0)) & ", " & _
                       "'" & CDate(vResultSet(7, 0)).ToString("MM/dd/yyyy") & "'"

        'Insert the fields
        CopyLogText("Inserting Destination Component details...")

        v_lNewID = CType(InsertFields(sPMNavGroup:=NavProcConst.NavGrpComponent, sFieldNames:=sFieldList, sFieldValues:=sFieldValues), gPMConstants.PMEReturnCode)

        If v_lNewID = gPMConstants.PMEReturnCode.PMFalse Then
            CopyLogText("Failed to insert destination Component details.")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'The Key field used for copy
        sKeyField = "pmnav_Component_id"

        'The list of fields
        sFieldList = "pmnav_key_id, sequence_number, " & _
                     "description, is_optional"

        'Copy the Component Set Keys to New Component
        CopyLogText("Copying Component SetKeys...")

        m_lReturn = CType(CopyTableRecords(v_sTableName:="PMNav_Set_Component_Key", v_sFields:=sFieldList, v_sKeyField:=sKeyField, v_lFieldID:=v_lComponentID, v_lNewFieldID:=v_lNewID), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyLogText("Failed to copy SetKeys for this Component.")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'The list of fields is the only varibale to change
        sFieldList = "pmnav_key_id, sequence_number, " & _
                     "description"

        'Copy the Component GetKeys to new Component
        CopyLogText("Copying Component GetKeys...")

        m_lReturn = CType(CopyTableRecords(v_sTableName:="PMNav_Get_Component_Key", v_sFields:=sFieldList, v_sKeyField:=sKeyField, v_lFieldID:=v_lComponentID, v_lNewFieldID:=v_lNewID), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyLogText("Failed to copy GetKeys for this Component.")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Return v_lNewID

    End Function

    ' ***************************************************************** '
    ' Name: CopyMap (Private)
    '
    ' Description: Copies an Entire Map to a New Map
    '
    ' ***************************************************************** '
    Private Function CopyMap(ByVal v_lMapID As Integer, ByVal v_sNewMap As String) As Integer

        Dim result As Integer = 0
        Dim vResultSet(,) As Object
        Dim lCaptionID As Integer
        Dim sFieldList, sFieldValues, sKeyField, sSql As String
        Dim v_lNewID As gPMConstants.PMEReturnCode




        'Set output script variables
        If m_bBuildCopyScript Then

            'Set the id of map
            m_cMapIDs.Add(v_lMapID, CStr(v_lMapID))

        End If

        'Get the Caption for this Map
        CopyLogText("Getting caption for this Map...")
        lCaptionID = UpdateFieldLookUp("PMCaption", 0, v_sNewMap)

        ' Check that we have a Caption and a Map
        If lCaptionID = 0 Then
            CopyLogText("Failed to get caption.")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        CopyLogText("Getting Source Map details...")

        sSql = "SELECT " & _
               "code, " & _
               lCaptionID & " CapID, " & _
               "'" & v_sNewMap & "' Description, " & _
               "is_deleted, " & _
               "effective_date, " & _
               "is_start_map " & _
               "FROM PMNav_Map WHERE pmnav_map_id = " & CStr(v_lMapID)

        m_lReturn = m_oSourceDatabase.SQLSelect(sSql:=sSql, sSQLName:="GetSourceMap", bStoredProcedure:=False, vResultArray:=vResultSet)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyLogText("Failed to get Source Map details.")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Make sure we have the Map
        If Not Information.IsArray(vResultSet) Then
            CopyLogText("This Map has no details.")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'List of Map fields
        sFieldList = "code, caption_id, " & _
                     "description, is_deleted, " & _
                     "effective_date, is_start_map "

        'List of Map Values
        If m_bBuildCopyScript Then

            vResultSet(1, 0) = "@caption_id"
        End If


        sFieldValues = "'" & CStr(vResultSet(0, 0)).Trim() & "', " & _
                       CStr(vResultSet(1, 0)) & ", " & _
                       "'" & CStr(vResultSet(2, 0)).Trim() & "', " & _
                       CStr(vResultSet(3, 0)) & ", " & _
                       "'" & CDate(vResultSet(4, 0)).ToString("MM/dd/yyyy") & "', " & _
                       CStr(vResultSet(5, 0))

        'Insert the fields (returns mapid)
        CopyLogText("Inserting Destination Map details...")

        v_lNewID = CType(InsertFields(sPMNavGroup:=NavProcConst.NavGrpMap, sFieldNames:=sFieldList, sFieldValues:=sFieldValues), gPMConstants.PMEReturnCode)

        If v_lNewID = gPMConstants.PMEReturnCode.PMFalse Then
            CopyLogText("Failed to insert Destination Map details.    ")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'The Key field used for copy (Map ID)
        sKeyField = "pmnav_map_id"

        'The list of Map SetKey Fields  (Copy Map SetKeys)
        sFieldList = "pmnav_key_id, sequence_number, " & _
                     "description, initial_key_value"

        'Copy the Map Set Keys to New Map
        CopyLogText("Copying Map SetKeys...")

        m_lReturn = CType(CopyTableRecords(v_sTableName:="PMNav_Set_Map_Key", v_sFields:=sFieldList, v_sKeyField:=sKeyField, v_lFieldID:=v_lMapID, v_lNewFieldID:=v_lNewID), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyLogText("No Map SetKeys Copied.")
        End If

        'The list of Step fields
        sFieldList = "pmnav_step_id, sub_nav_map_id, " & _
                     "pmnav_component_id, task, ok_action, " & _
                     "cancel_action, ok_no_of_steps, " & _
                     "cancel_no_of_steps, ok_nav_process_id, " & _
                     "cancel_nav_process_id, navigate_status, " & _
                     "is_hidden, is_logged, caption_id, description "

        'Copy the Map Steps to New Map
        CopyLogText("Copying Map Steps...")

        m_lReturn = CType(CopyTableRecords(v_sTableName:="PMNav_Step", v_sFields:=sFieldList, v_sKeyField:=sKeyField, v_lFieldID:=v_lMapID, v_lNewFieldID:=v_lNewID), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyLogText("No Map Steps copied.")
        End If

        'The list of Map Step SetKey Fields
        sFieldList = "pmnav_step_id, pmnav_key_id, sequence_number, " & _
                     "description, initial_key_value"

        'Copy the Map Step Set Keys to New Map
        CopyLogText("Copying Map Step SetKeys...")

        m_lReturn = CType(CopyTableRecords(v_sTableName:="PMNav_Set_Step_Key", v_sFields:=sFieldList, v_sKeyField:=sKeyField, v_lFieldID:=v_lMapID, v_lNewFieldID:=v_lNewID), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyLogText("No Map Step SetKeys Copied.")
        End If

        'The list of fields for Map Step GetKeys
        sFieldList = "pmnav_step_id, pmnav_key_id, " & _
                     "sequence_number, description"

        'Copy the Map Step Get Keys to new map
        CopyLogText("Copying Map Step GetKeys...")

        m_lReturn = CType(CopyTableRecords(v_sTableName:="PMNav_Get_Step_Key", v_sFields:=sFieldList, v_sKeyField:=sKeyField, v_lFieldID:=v_lMapID, v_lNewFieldID:=v_lNewID), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyLogText("No Map Step GetKeys Copied.")
        End If

        'Return the Map ID


        Return v_lNewID

    End Function
    ' ***************************************************************** '
    '
    ' Name: CopyTableRecords ( Private )
    '
    ' Description: Copies records from one table to a new id in same table
    '
    ' Edit History  :
    ' RAM20030123   : Check for Null code is added, for the value
    '***************************************************************** '
    Private Function CopyTableRecords(ByVal v_sTableName As String, ByVal v_sFields As String, ByVal v_sKeyField As String, ByVal v_lFieldID As Integer, ByVal v_lNewFieldID As Integer) As Integer

        Dim result As Integer = 0
        Dim sSql As String = ""
        Dim vResultSet(,) As Object = Nothing
        Dim sFieldValues As New StringBuilder
        Dim vValue As gPMConstants.PMEReturnCode
        Dim sName As String = ""
        Dim lRecordCount As Integer
        Dim cRecords As Collection
        Dim vFields As Object



        ' RDC 19062002
        cRecords = New Collection()

        result = gPMConstants.PMEReturnCode.PMTrue

        'Get the fields from the source database
        sSql = " SELECT " & v_sFields & _
               " FROM " & v_sTableName & _
               " WHERE " & v_sKeyField & " = " & CStr(v_lFieldID)

        m_lReturn = m_oSourceDatabase.SQLSelect(sSql:=sSql, sSQLName:="GetSourceFields", bStoredProcedure:=False, lNumberRecords:=0)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        lRecordCount = m_oSourceDatabase.Records.Count()

        'Check if zero records returned
        If lRecordCount = 0 Then
            Return result
        End If

        'Retrieve records from database object
        For iPtr As Integer = 1 To lRecordCount

            With m_oSourceDatabase.Records.Item(iPtr)

                ReDim vFields(1, .Count - 1)

                ' RDC 21062002 fields now zero-based

                For iFieldPtr As Integer = 0 To .Count - 1

                    'Get the name and value of the field

                    vFields(0, iFieldPtr) = .Fields()(iFieldPtr).Name

                    vFields(1, iFieldPtr) = .Fields()(iFieldPtr)

                Next iFieldPtr

            End With

            'Add the record
            cRecords.Add(vFields)

        Next iPtr

        'For each Record Build the field values to be inserted

        For iPtr As Integer = 1 To cRecords.Count

            sFieldValues = New StringBuilder("")

            For iFieldPtr As Integer = cRecords.Item(iPtr).GetLowerBound(1) To cRecords.Item(iPtr).GetUpperBound(1)

                'Get the name and value of the field

                sName = CStr((cRecords.Item(iPtr))(0, iFieldPtr))

                vValue = (cRecords.Item(iPtr))(1, iFieldPtr)

                ' RAM20030123 : Check for Null
                'Check for nulls

                If Convert.IsDBNull(vValue) Or IsNothing(vValue) Then
                    vValue = CType("Null", gPMConstants.PMEReturnCode)
                ElseIf (vValue = "") Then

                    'If "" Make value "Null"
                    vValue = CType("Null", gPMConstants.PMEReturnCode)

                    'Add quotes if this is a String field
                ElseIf Information.VarType(vValue) = VariantType.String Then

                    vValue = CType("'" & vValue & "'", gPMConstants.PMEReturnCode)

                    'Check for look up fields and get synchronised values
                ElseIf sName = "pmnav_component_id" Or sName = "ok_nav_process_id" Or sName = "cancel_nav_process_id" Or sName = "caption_id" Or sName = "pmnav_key_id" Or sName = "sub_nav_map_id" Then

                    'Get the source lookup value
                    vValue = DSNSourceLookUp(sName, vValue)

                    If (Conversion.Val(CStr(vValue)) = 0) And (Not m_bBuildCopyScript) Then
                        vValue = CType("Null", gPMConstants.PMEReturnCode)
                    End If

                End If

                'Build field values
                sFieldValues.Append(CStr(vValue) & ", ")

            Next iFieldPtr

            sFieldValues = New StringBuilder(sFieldValues.ToString().Substring(0, sFieldValues.ToString().Length - 2))


            'Capture SQL if this is copy Output Script
            If m_bBuildCopyScript Then

                m_lInsertRecordCount += 1
                CopyLogSQL("INSERT RECORD", NavProcConst.ACSQLComment)

                sSql = "INSERT INTO " & v_sTableName & "(" & _
                       v_sKeyField & ", " & v_sFields & ") " & _
                       "VALUES (@" & _
                       v_sKeyField & ", " & sFieldValues.ToString() & ")"

                'Capture SQL
                CopyLogSQL(sSql, NavProcConst.ACSQLStatement)

            Else

                'Build SQL
                sSql = "INSERT INTO " & v_sTableName & "(" & _
                       v_sKeyField & ", " & v_sFields & ") " & _
                       "VALUES (" & _
                       v_lNewFieldID & ", " & sFieldValues.ToString() & ")"

                'Execute SQL
                CopyLogText("SQL: " & sSql)
                m_lReturn = m_oDatabase.SQLAction(sSql:=sSql, sSQLName:="InsertDestRecord", bStoredProcedure:=False)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    CopyLogText("Insert Failed.")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

        Next iPtr

        Return result

    End Function


    'UPGRADE_NOTE: (7001) The following declaration (CreateSQLFileHeader) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub CreateSQLFileHeader(ByRef bStart As Boolean, ByRef sHeader As String, ByRef sScriptName As String)
    '
    'Standard header file
    'sHeader = "if exists (select * from sysobjects where id = " &  _
    '          "object_id('" & sScriptName & "') and " &  _
    '          "sysstat & 0xf = 4) " & Strings.Chr(13) & Strings.Chr(10) &  _
    '          NavProcConst.ACTAB & "drop procedure " & sScriptName & Strings.Chr(13) & Strings.Chr(10) &  _
    '          "GO " & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) &  _
    '          "/*********************************************************/" & Strings.Chr(13) & Strings.Chr(10) &  _
    '          "/* Stored Procedure Insert Process                       */" & Strings.Chr(13) & Strings.Chr(10) &  _
    '          "/*********************************************************/" & Strings.Chr(13) & Strings.Chr(10) &  _
    '          "CREATE PROCEDURE " & sScriptName & Strings.Chr(13) & Strings.Chr(10) &  _
    '          "@return_id int OUTPUT"
    '
    'If bStart Then
    '
    'sHeader = sHeader &  _
    '          NavProcConst.vbCrLf2 & "AS" & NavProcConst.vbCrLf2 &  _
    '          "BEGIN" & NavProcConst.vbCrLf2 &  _
    '          "/* DECLARE VARIABLES */" & Strings.Chr(13) & Strings.Chr(10) &  _
    '          "DECLARE " & Strings.Chr(13) & Strings.Chr(10) &  _
    '          "@pmnav_component_id int, " & Strings.Chr(13) & Strings.Chr(10) &  _
    '          "@pmnav_process_id int, " & Strings.Chr(13) & Strings.Chr(10) &  _
    '          "@pmnav_map_id int, " & Strings.Chr(13) & Strings.Chr(10) &  _
    '          "@pmnav_step_id int, " & Strings.Chr(13) & Strings.Chr(10) &  _
    '          "@pmproduct_id int, " & Strings.Chr(13) & Strings.Chr(10) &  _
    '          "@pmproc_lock_group_id int, " & Strings.Chr(13) & Strings.Chr(10) &  _
    '          "@transaction_type_id int, " & Strings.Chr(13) & Strings.Chr(10) &  _
    '          "@caption_id int, " & Strings.Chr(13) & Strings.Chr(10) &  _
    '          "@pmnav_key_id int, " & Strings.Chr(13) & Strings.Chr(10) &  _
    '          "@ok_nav_process_id int, " & Strings.Chr(13) & Strings.Chr(10) &  _
    '          "@cancel_nav_process_id int, " & Strings.Chr(13) & Strings.Chr(10) &  _
    '          "@sub_nav_map_id int, " & Strings.Chr(13) & Strings.Chr(10) &  _
    '          "@sub_component_id int, " & Strings.Chr(13) & Strings.Chr(10) &  _
    '          "@temp_id int " & Strings.Chr(13) & Strings.Chr(10)
    '
    'Else
    '
    'sHeader = sHeader &  _
    '          Strings.Chr(13) & Strings.Chr(10) & "@pmnav_component_id int, " & Strings.Chr(13) & Strings.Chr(10) &  _
    '          "@pmnav_process_id int, " & Strings.Chr(13) & Strings.Chr(10) &  _
    '          "@pmnav_map_id int, " & Strings.Chr(13) & Strings.Chr(10) &  _
    '          "@pmnav_step_id int, " & Strings.Chr(13) & Strings.Chr(10) &  _
    '          "@pmproduct_id int, " & Strings.Chr(13) & Strings.Chr(10) &  _
    '          "@pmproc_lock_group_id int, " & Strings.Chr(13) & Strings.Chr(10) &  _
    '          "@transaction_type_id int, " & Strings.Chr(13) & Strings.Chr(10) &  _
    '          "@caption_id int, " & Strings.Chr(13) & Strings.Chr(10) &  _
    '          "@pmnav_key_id int, " & Strings.Chr(13) & Strings.Chr(10) &  _
    '          "@ok_nav_process_id int, " & Strings.Chr(13) & Strings.Chr(10) &  _
    '          "@cancel_nav_process_id int, " & Strings.Chr(13) & Strings.Chr(10) &  _
    '          "@sub_nav_map_id int, " & Strings.Chr(13) & Strings.Chr(10) &  _
    '          "@sub_component_id int, " & Strings.Chr(13) & Strings.Chr(10) &  _
    '          "@temp_id int " & NavProcConst.vbCrLf2 &  _
    '          "AS" & NavProcConst.vbCrLf2 &  _
    '          "BEGIN" & NavProcConst.vbCrLf2
    '
    'End If
    '
    'End Sub

    Private Sub CreateSQLFileHeadersANDFooters()

        ' CTAF 240100 - Delete temporary table if it already exists
        ' CTAF 060400 - Table exists in tempdb not the current db
        ' RDC 20092000 Move table create to before declarations
        ' RAM20030123 - Changed Double Quotes to Single Quotes i.e Chr(34) to Chr(39)
        Dim sHeader As String = "/* REMOVE TEMPSTACK IF IT EXISTS */" & Strings.Chr(13) & Strings.Chr(10) & _
                                "IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE name = " & Strings.Chr(39).ToString() & "##TempStack" & Strings.Chr(39).ToString() & ") " & Strings.Chr(13) & Strings.Chr(10) & _
                                "DROP TABLE ##TempStack" & Strings.Chr(13) & Strings.Chr(10) & _
                                "GO" & NavProcConst.vbCrLf2

        sHeader = sHeader & _
                  "/* CREATE TEMPORARY TABLE */" & Strings.Chr(13) & Strings.Chr(10) & _
                  "CREATE TABLE ##TempStack( " & Strings.Chr(13) & Strings.Chr(10) & _
                  "tempstack_id int IDENTITY, " & Strings.Chr(13) & Strings.Chr(10) & _
                  "pmproduct_id int NULL, " & Strings.Chr(13) & Strings.Chr(10) & _
                  "pmproc_lock_group_id int NULL, " & Strings.Chr(13) & Strings.Chr(10) & _
                  "transaction_type_id int NULL, " & Strings.Chr(13) & Strings.Chr(10) & _
                  "caption_id int NULL, " & Strings.Chr(13) & Strings.Chr(10) & _
                  "pmnav_key_id int NULL, " & Strings.Chr(13) & Strings.Chr(10) & _
                  "pmnav_component_id int NULL, " & Strings.Chr(13) & Strings.Chr(10) & _
                  "ok_nav_process_id int NULL, " & Strings.Chr(13) & Strings.Chr(10) & _
                  "cancel_nav_process_id int NULL, " & Strings.Chr(13) & Strings.Chr(10) & _
                  "sub_nav_map_id int NULL, " & Strings.Chr(13) & Strings.Chr(10) & _
                  "pmnav_process_id int NULL, " & Strings.Chr(13) & Strings.Chr(10) & _
                  "pmnav_map_id int NULL, " & Strings.Chr(13) & Strings.Chr(10) & _
                  "pmnav_step_id int NULL, " & Strings.Chr(13) & Strings.Chr(10) & _
                  "sub_component_id int NULL, " & Strings.Chr(13) & Strings.Chr(10) & _
                  "effective_date datetime NULL, " & Strings.Chr(13) & Strings.Chr(10) & _
                  "effective_date2 datetime NULL) " & Strings.Chr(13) & Strings.Chr(10) & _
                  "GO" & NavProcConst.vbCrLf2

        sHeader = sHeader & _
                  "/* DECLARE VARIABLES */" & Strings.Chr(13) & Strings.Chr(10) & _
                  "DECLARE " & Strings.Chr(13) & Strings.Chr(10) & _
                  "@pmnav_component_id int, " & Strings.Chr(13) & Strings.Chr(10) & _
                  "@pmnav_process_id int, " & Strings.Chr(13) & Strings.Chr(10) & _
                  "@pmnav_map_id int, " & Strings.Chr(13) & Strings.Chr(10) & _
                  "@pmnav_step_id int, " & Strings.Chr(13) & Strings.Chr(10) & _
                  "@pmproduct_id int, " & Strings.Chr(13) & Strings.Chr(10) & _
                  "@pmproc_lock_group_id int, " & Strings.Chr(13) & Strings.Chr(10) & _
                  "@transaction_type_id int, " & Strings.Chr(13) & Strings.Chr(10) & _
                  "@caption_id int, " & Strings.Chr(13) & Strings.Chr(10) & _
                  "@pmnav_key_id int, " & Strings.Chr(13) & Strings.Chr(10) & _
                  "@ok_nav_process_id int, " & Strings.Chr(13) & Strings.Chr(10) & _
                  "@cancel_nav_process_id int, " & Strings.Chr(13) & Strings.Chr(10) & _
                  "@sub_nav_map_id int, " & Strings.Chr(13) & Strings.Chr(10) & _
                  "@sub_component_id int, " & Strings.Chr(13) & Strings.Chr(10) & _
                  "@temp_id int, " & Strings.Chr(13) & Strings.Chr(10) & _
                  "@tempstack_id int, " & Strings.Chr(13) & Strings.Chr(10) & _
                  "@effective_date datetime, " & Strings.Chr(13) & Strings.Chr(10) & _
                  "@effective_date2 datetime " & NavProcConst.vbCrLf2

        sHeader = sHeader & _
                  "BEGIN TRAN " & NavProcConst.vbCrLf2

        m_vCopyScript(0) = sHeader & CStr(m_vCopyScript(0))

        m_vCopyScript(m_lCopyScriptPtr) = CStr(m_vCopyScript(m_lCopyScriptPtr)) & _
                                          NavProcConst.vbCrLf2 & _
                                          "IF @@error = 0 " & Strings.Chr(13) & Strings.Chr(10) & _
                                          NavProcConst.ACTAB & "BEGIN" & Strings.Chr(13) & Strings.Chr(10) & _
                                          NavProcConst.ACTAB & "Print 'Script Suceeded'" & Strings.Chr(13) & Strings.Chr(10) & _
                                          NavProcConst.ACTAB & "COMMIT" & Strings.Chr(13) & Strings.Chr(10) & _
                                          NavProcConst.ACTAB & "END" & Strings.Chr(13) & Strings.Chr(10) & _
                                          "ELSE" & Strings.Chr(13) & Strings.Chr(10) & _
                                          NavProcConst.ACTAB & "BEGIN" & Strings.Chr(13) & Strings.Chr(10) & _
                                          NavProcConst.ACTAB & "Print 'Script Failed'" & Strings.Chr(13) & Strings.Chr(10) & _
                                          NavProcConst.ACTAB & "ROLLBACK" & Strings.Chr(13) & Strings.Chr(10) & _
                                          NavProcConst.ACTAB & "END" & Strings.Chr(13) & Strings.Chr(10) & _
                                          "" & NavProcConst.vbCrLf2 & "GO"



    End Sub


    ' ***************************************************************** '
    '
    ' Name: DeleteItem ( Public )
    '
    ' Description: Deletes navigator items without checking for dependencies
    '
    ' ***************************************************************** '
    Public Function DeleteItem(ByRef sPMNavGroup As String, ByRef lID As Integer, ByRef lParentID As Integer) As Integer

        Dim result As Integer = 0
        Dim sSql, sTableName, sWhereClause As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Select Case sPMNavGroup
                Case NavProcConst.NavGrpProcess
                    sTableName = "PMNav_Process"
                    sWhereClause = "pmnav_process_id = " & lID

                Case NavProcConst.NavGrpMap
                    sTableName = "PMNav_Map"
                    sWhereClause = "pmnav_map_id = " & lID

                Case NavProcConst.NavGrpComponent
                    sTableName = "PMNav_Component"
                    sWhereClause = "pmnav_component_id = " & lID

                Case NavProcConst.NavGrpStep
                    sTableName = "PMNav_Step"
                    sWhereClause = "pmnav_step_id = " & lID & _
                                   " AND pmnav_map_id = " & CStr(lParentID)
                Case Else
                    Return gPMConstants.PMEReturnCode.PMFalse
            End Select

            sSql = "DELETE FROM " & sTableName & _
                   " WHERE " & sWhereClause

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSql:=sSql, sSQLName:="DeleteItem", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch


            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function


    ' ***************************************************************** '
    ' Name: DSNProcessMaps (Public)
    '
    ' Description: Gets Processes and Maps from a specific Database
    '
    ' ***************************************************************** '
    Public Function DSNProcessMaps(ByRef v_sDSN As String, ByRef r_vProcesses(,) As Object, ByRef r_vMaps(,) As Object) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim oDatabase As dPMDAO.Database
        Dim sSql As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            oDatabase = New dPMDAO.Database()

            ' Open the Database
            ' RDC 27062002 use Comp Serv to open database
            m_lReturn = CType(gPMComponentServices.NewDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_oDatabase:=oDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the Processes
            sSql = "SELECT pmnav_process_id, description FROM PMNav_Process " & _
                   "ORDER BY description"
            m_lReturn = oDatabase.SQLSelect(sSql:=sSql, sSQLName:="GetProcesses", bStoredProcedure:=False, vResultArray:=r_vProcesses)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oDatabase.CloseDatabase()
                oDatabase = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the Maps
            sSql = "SELECT pmnav_map_id, description FROM PMNav_Map " & _
                   "ORDER BY description"
            m_lReturn = oDatabase.SQLSelect(sSql:=sSql, sSQLName:="GetMaps", bStoredProcedure:=False, vResultArray:=r_vMaps)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oDatabase.CloseDatabase()
                oDatabase = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oDatabase.CloseDatabase()
            oDatabase = Nothing

            Return result

        Catch excep As System.Exception


            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DSNProcessMaps Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DSNProcessMaps", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DSNSourceLookUp (Private)
    '
    ' Description:  Finds a LookUp record in the Source database and
    '               Synchronises that field with the Destination Database
    '
    ' Edit History  :
    ' RAM20030120   : Code changes made to check for Null first and then
    '                   for Zero
    ' ***************************************************************** '
    Private Function DSNSourceLookUp(ByRef vFieldName As String, ByRef vFieldValue As gPMConstants.PMEReturnCode) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim sDescription As String = ""
        Dim vResultSet As Object = Nothing

        Try

            CopyLogText("Looking Up Source Field: " & vFieldName)

            'Check for nulls

            If Convert.IsDBNull(vFieldValue) Or IsNothing(vFieldValue) Then
                Return CType("Null", gPMConstants.PMEReturnCode)
            End If

            'Dont allow zero lookups
            If Conversion.Val(CStr(vFieldValue)) = 0 Then
                Return CType("Null", gPMConstants.PMEReturnCode)
            End If

            'If we are copying from the current database then return same
            If Not m_bCopyFromDB And Not m_bBuildCopyScript Then
                Return vFieldValue
            End If


            result = vFieldValue


            'Simple Cases

            Select Case vFieldName
                Case "pmproduct_id"
                    vFieldValue = CType(DSNSourceProduct(vFieldValue), gPMConstants.PMEReturnCode)

                Case "pmproc_lock_group_id"
                    vFieldValue = CType(DSNSourceProc_Lock_Group(vFieldValue), gPMConstants.PMEReturnCode)

                Case "transaction_type_id"
                    vFieldValue = CType(DSNSourceTransactionType(vFieldValue), gPMConstants.PMEReturnCode)

                Case "caption_id"
                    vFieldValue = CType(DSNSourceCaption(vFieldValue), gPMConstants.PMEReturnCode)

                Case "pmnav_key_id"
                    vFieldValue = CType(DSNSourceNavKeys(vFieldValue), gPMConstants.PMEReturnCode)

                    'Complex Cases

                Case "pmnav_component_id"
                    vFieldValue = CType(DSNSourceComponent(vFieldValue), gPMConstants.PMEReturnCode)

                    'This is a sub component, so set variable name
                    vFieldName = "sub_component_id"

                Case "ok_nav_process_id", "cancel_nav_process_id"
                    vFieldValue = CType(DSNSourceProcess(vFieldValue, vFieldName), gPMConstants.PMEReturnCode)

                Case "sub_nav_map_id"
                    vFieldValue = CType(DSNSourceMap(vFieldValue, vFieldName), gPMConstants.PMEReturnCode)


            End Select

            m_lLookUpCount += 1

            'If we are copying to script, then get variable name
            If m_bBuildCopyScript Then

                'variable name for script

                Return CType("@" & vFieldName, gPMConstants.PMEReturnCode)

            End If

            'Return new value

            Return vFieldValue

        Catch


            CopyLogText("Failed to retrieve lookup field: " & vFieldName)

            Return vFieldValue
        End Try

    End Function
    ' ***************************************************************** '
    ' Name: DSNSourceProduct (Private)
    '
    ' Description: Synchronise required Product from Database Source
    '
    ' ***************************************************************** '
    Private Function DSNSourceProduct(ByRef lID As Integer) As Integer

        Dim result As Integer = 0
        Dim sSql As String = ""
        Dim vResultSet(,) As Object

        Dim lNewID As Double
        Dim lProductID, lCaption_id As Integer
        Dim sCode, sDescription As String
        Dim iIs_deleted As gPMConstants.PMEReturnCode
        Dim sEffective_date As String = ""




        'Get the compare fields
        sSql = "SELECT code, description FROM PMProduct " & _
               "WHERE pmproduct_id = " & CStr(lID)

        m_lReturn = m_oSourceDatabase.SQLSelect(sSql:=sSql, sSQLName:="GetProduct", bStoredProcedure:=False, vResultArray:=vResultSet)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        If Not Information.IsArray(vResultSet) Then
            Return result
        End If


        sCode = CStr(vResultSet(0, 0))

        sDescription = CStr(vResultSet(1, 0))

        If m_bBuildCopyScript Then

            CopyLogSQL("Select @pmproduct_id = Null", NavProcConst.ACSQLStatement)
            sSql = "SELECT @pmproduct_id = pmproduct_id FROM PMProduct " & _
                   "WHERE code = '" & sCode.Trim() & "'"

            CopyLogSQL(sSql, NavProcConst.ACSQLStatement)

            'Check if we have a match
            CopyLogSQL("IF @pmproduct_id IS NULL", NavProcConst.ACSQLStatement)
            CopyLogSQL("", NavProcConst.ACSQLBegin)

            lProductID = 0

        Else

            'Check if we have a corresponding record in current DB
            sSql = "SELECT pmproduct_id FROM PMProduct " & _
                   "WHERE code = '" & sCode.Trim() & "'"


            vResultSet = Nothing

            'Check if product exists
            m_lReturn = m_oDatabase.SQLSelect(sSql:=sSql, sSQLName:="CheckProduct", bStoredProcedure:=False, vResultArray:=vResultSet)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Information.IsArray(vResultSet) Then

                'If a corresponding record exists then get it

                lProductID = CInt(Conversion.Val(CStr(vResultSet(0, 0))))

                result = CInt(Conversion.Val(CStr(vResultSet(0, 0))))

            End If

        End If

        If lProductID = 0 Then

            'Get new ID
            lNewID = GetNewID("PMProduct", "pmproduct_id")

            'Get a new caption
            lCaption_id = UpdateFieldLookUp("PMCaption", 0, sDescription)

            ' Check that we have a Caption
            If lCaption_id = 0 Then
                Return result
            End If

            sCode = "'" & sCode & "'"
            sDescription = "'" & sDescription & "'"
            iIs_deleted = gPMConstants.PMEReturnCode.PMFalse
            sEffective_date = "'" & DateTime.Today.ToString("MM/dd/yyyy") & "'"

            'Capture SQL if copy output
            If m_bBuildCopyScript Then


                sSql = "INSERT INTO PMProduct(pmproduct_id, caption_id, " & _
                       "code, description, is_deleted, effective_date, database_name) " & _
                       "VALUES(@pmproduct_id, @caption_id, " & _
                       sCode & ", " & sDescription & ", " & CStr(iIs_deleted) & ", " & _
                       sEffective_date & ", '')"

                CopyLogSQL(sSql, NavProcConst.ACSQLStatement)

                CopyLogSQL("END", NavProcConst.ACSQLStatement)

                'Indicate success
                Return gPMConstants.PMEReturnCode.PMTrue

            End If

            sSql = "INSERT INTO PMProduct(pmproduct_id, caption_id, " & _
                   "code, description, is_deleted, effective_date) " & _
                   "VALUES(" & CStr(lNewID) & ", " & CStr(lCaption_id) & ", " & _
                   sCode & ", " & sDescription & ", " & CStr(iIs_deleted) & ", " & _
                   sEffective_date & ")"

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSql:=sSql, sSQLName:="InsertFields", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            'Return new id
            result = CInt(lNewID)

        End If

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: DSNSourceCaption (Private)
    '
    ' Description: Synchronise required Caption from Database Source
    '
    ' ***************************************************************** '
    Private Function DSNSourceCaption(ByRef lID As Integer) As Integer

        Dim result As Integer = 0
        Dim sSql As String = ""
        Dim vResultSet(,) As Object

        Dim lCaptionID As Integer
        Dim sDescription As String = ""

        Try


            'Get the compare fields
            sSql = "SELECT caption FROM PMCaption " & _
                   "WHERE caption_id = " & CStr(lID)

            m_lReturn = m_oSourceDatabase.SQLSelect(sSql:=sSql, sSQLName:="GetCaption", bStoredProcedure:=False, vResultArray:=vResultSet)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Information.IsArray(vResultSet) Then
                Return result
            End If


            sDescription = CStr(vResultSet(0, 0))

            'Get the corresponding caption
            lCaptionID = UpdateFieldLookUp("PMCaption", 0, sDescription)


            Return lCaptionID

        Catch



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DSNSourceNavKeys (Private)
    '
    ' Description: Synchronise required NavKeys from Database Source
    '
    ' ***************************************************************** '
    Private Function DSNSourceNavKeys(ByRef lID As Integer) As Integer

        Dim result As Integer = 0
        Dim sSql As String = ""
        Dim vResultSet(,) As Object

        Dim lNewID As Double
        Dim lKeyID As Double
        Dim sName, sDescription As String
        Dim iData_type As Integer
        Dim iIs_deleted As gPMConstants.PMEReturnCode
        Dim sEffective_date As String = ""




        'Get the compare fields
        sSql = "SELECT name, description, data_type FROM PMNav_Key " & _
               "WHERE pmnav_key_id = " & CStr(lID)

        m_lReturn = m_oSourceDatabase.SQLSelect(sSql:=sSql, sSQLName:="GetNavKeys", bStoredProcedure:=False, vResultArray:=vResultSet)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        If Not Information.IsArray(vResultSet) Then
            Return result
        End If

        lKeyID = 0

        sName = CStr(vResultSet(0, 0))

        sDescription = CStr(vResultSet(1, 0))

        iData_type = CInt(vResultSet(2, 0))

        vResultSet = Nothing

        If m_bBuildCopyScript Then

            'Capture SQL

            sSql = "SELECT @pmnav_key_id  = pmnav_key_id FROM PMNav_Key " & _
                   "WHERE name = '" & sName & "'" & _
                   " AND data_type = " & CStr(iData_type)

            'Check if we have already created this key, if we have then we dont need to again
            If CopySQLExists(vKeyID:=lID) Then

                CopyLogSQL(sSql, NavProcConst.ACSQLStatement)

                'Indicate success
                Return gPMConstants.PMEReturnCode.PMTrue

            End If

            CopyLogSQL("Select @pmnav_key_id = Null", NavProcConst.ACSQLStatement)
            CopyLogSQL(sSql, NavProcConst.ACSQLStatement)

            'Check if we have a match
            CopyLogSQL("IF @pmnav_key_id IS NULL", NavProcConst.ACSQLStatement)

            CopyLogSQL("", NavProcConst.ACSQLBegin)

            'Force conditional statements below to be executed
            lKeyID = 0

        Else

            'Check if we have a corresponding record in current DB
            sSql = "SELECT pmnav_key_id FROM PMNav_Key " & _
                   "WHERE name = '" & sName & "'" & _
                   " AND data_type = " & CStr(iData_type)

            'Check if NavKeys exists
            m_lReturn = m_oDatabase.SQLSelect(sSql:=sSql, sSQLName:="CheckNavKeys", bStoredProcedure:=False, vResultArray:=vResultSet)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Check if we have a value
            If Information.IsArray(vResultSet) Then

                'If a corresponding record exists then get it

                lKeyID = Conversion.Val(CStr(vResultSet(0, 0)))
                result = CInt(lKeyID)

            End If

        End If

        If lKeyID = 0 Then

            'Get new ID
            lNewID = GetNewID("PMNav_Key", "pmnav_key_id")

            sName = "'" & sName & "'"
            sDescription = "'" & sDescription & "'"
            iIs_deleted = gPMConstants.PMEReturnCode.PMFalse
            sEffective_date = "'" & DateTime.Today.ToString("MM/dd/yyyy") & "'"


            'Capture SQL if copy output
            If m_bBuildCopyScript Then


                sSql = "INSERT INTO PMNav_Key(pmnav_key_id, name, description, " & _
                       "data_type, is_deleted, effective_date) " & _
                       "VALUES(@pmnav_key_id, " & sName & ", " & _
                       sDescription & ", " & CStr(iData_type) & ", " & CStr(iIs_deleted) & ", " & _
                       sEffective_date & ")"

                CopyLogSQL(sSql, NavProcConst.ACSQLStatement)

                CopyLogSQL("END", NavProcConst.ACSQLStatement)

                'Add this key to the keys lookup list
                m_cKeyIDs.Add(lID, CStr(lID))

                'Indicate success
                Return gPMConstants.PMEReturnCode.PMTrue

            End If

            'Build SQL
            sSql = "INSERT INTO PMNav_Key(pmnav_key_id, name, description, " & _
                   "data_type, is_deleted, effective_date) " & _
                   "VALUES(" & CStr(lNewID) & ", " & sName & ", " & _
                   sDescription & ", " & CStr(iData_type) & ", " & CStr(iIs_deleted) & ", " & _
                   sEffective_date & ")"


            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSql:=sSql, sSQLName:="InsertFields", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            'Return new id
            result = CInt(lNewID)

        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DSNSourceTransactionType (Private)
    '
    ' Description: Synchronise required Product from Database Source
    '
    ' ***************************************************************** '
    Private Function DSNSourceTransactionType(ByRef lID As Integer) As Integer

        Dim result As Integer = 0
        Dim sSql As String = ""
        Dim vResultSet(,) As Object

        Dim lNewID As Double
        Dim lTransID As Double
        Dim lCaption_id As Integer
        Dim sCode, sDescription As String
        Dim iIs_deleted As gPMConstants.PMEReturnCode
        Dim sEffective_date, sTransaction_type_basis As String




        'Get the compare fields
        sSql = "SELECT code, description, transaction_type_basis FROM Transaction_Type " & _
               "WHERE transaction_type_id = " & CStr(lID)

        m_lReturn = m_oSourceDatabase.SQLSelect(sSql:=sSql, sSQLName:="GetTransType", bStoredProcedure:=False, vResultArray:=vResultSet)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        If Not Information.IsArray(vResultSet) Then
            Return result
        End If


        sCode = CStr(vResultSet(0, 0))

        sDescription = CStr(vResultSet(1, 0))

        sTransaction_type_basis = CStr(vResultSet(2, 0))

        If m_bBuildCopyScript Then

            'Capture SQL
            CopyLogSQL("Select @transaction_type_id = Null", NavProcConst.ACSQLStatement)
            sSql = "SELECT @transaction_type_id = transaction_type_id FROM Transaction_Type " & _
                   "WHERE code = '" & sCode & "'"

            CopyLogSQL(sSql, NavProcConst.ACSQLStatement)

            'Check if we have a match
            CopyLogSQL("IF @transaction_type_id IS NULL", NavProcConst.ACSQLStatement)
            CopyLogSQL("", NavProcConst.ACSQLBegin)

            lTransID = 0
        Else

            'Check if we have a corresponding record in current DB
            sSql = "SELECT transaction_type_id FROM Transaction_Type " & _
                   "WHERE code = '" & sCode & "'"

            vResultSet = Nothing

            'Check if product exists
            m_lReturn = m_oDatabase.SQLSelect(sSql:=sSql, sSQLName:="CheckProduct", bStoredProcedure:=False, vResultArray:=vResultSet)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Information.IsArray(vResultSet) Then

                'If a corresponding record exists then get it

                lTransID = Conversion.Val(CStr(vResultSet(0, 0)))
                result = CInt(lTransID)

            End If

        End If

        If lTransID = 0 Then

            'Get new ID
            lNewID = GetNewID("Transaction_Type", "transaction_type_id")

            'Get a new caption
            lCaption_id = UpdateFieldLookUp("PMCaption", 0, sDescription)

            ' Check that we have a Caption
            If lCaption_id = 0 Then
                Return result
            End If

            sCode = "'" & sCode & "'"
            sDescription = "'" & sDescription & "'"
            sTransaction_type_basis = "'" & sTransaction_type_basis & "'"

            iIs_deleted = gPMConstants.PMEReturnCode.PMFalse
            sEffective_date = "'" & DateTime.Today.ToString("MM/dd/yyyy") & "'"


            'Capture SQL if copy output
            If m_bBuildCopyScript Then

                sSql = "INSERT INTO Transaction_Type(transaction_type_id, caption_id, " & _
                       "code, description, is_deleted, effective_date, transaction_type_basis) " & _
                       "VALUES(@transaction_type_id, @caption_id, " & _
                       sCode & ", " & sDescription & ", " & CStr(iIs_deleted) & ", " & _
                       sEffective_date & ", " & sTransaction_type_basis & ")"

                CopyLogSQL(sSql, NavProcConst.ACSQLStatement)
                CopyLogSQL("END", NavProcConst.ACSQLStatement)

                'Indicate success
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            sSql = "INSERT INTO Transaction_Type(transaction_type_id, caption_id, " & _
                   "code, description, is_deleted, effective_date, transaction_type_basis) " & _
                   "VALUES(" & CStr(lNewID) & ", " & CStr(lCaption_id) & ", " & _
                   sCode & ", " & sDescription & ", " & CStr(iIs_deleted) & ", " & _
                   sEffective_date & ", " & sTransaction_type_basis & ")"


            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSql:=sSql, sSQLName:="InsertFields", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            'Return new id
            result = CInt(lNewID)

        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DSNSourceComponent (Private)
    '
    ' Description: Synchronise required Component from Database Source
    '
    ' ***************************************************************** '
    Private Function DSNSourceComponent(ByRef lID As Integer) As Integer

        Dim result As Integer = 0
        Dim sSql As String = ""
        Dim vResultSet(,) As Object
        Dim sCurrentCat As String = ""
        Dim lNewID As gPMConstants.PMEReturnCode
        Dim sObjectName, sClassName As String
        Dim sDescription As String

        Try


            'Get the compare fields
            sSql = "SELECT object_name, class_name, description FROM PMNav_Component " & _
                   "WHERE pmnav_component_id = " & CStr(lID)

            m_lReturn = m_oSourceDatabase.SQLSelect(sSql:=sSql, sSQLName:="GetTransType", bStoredProcedure:=False, vResultArray:=vResultSet)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Information.IsArray(vResultSet) Then
                Return result
            End If


            sObjectName = CStr(vResultSet(0, 0))

            sClassName = CStr(vResultSet(1, 0))

            sDescription = CStr(vResultSet(2, 0))

            vResultSet = Nothing


            If m_bBuildCopyScript Then

                'If we are outputting to file, then capture Sql

                sSql = "SELECT @sub_component_id = pmnav_component_id " & _
                       "FROM PMNav_Component " & _
                       "WHERE object_name = '" & sObjectName.Trim() & "'" & _
                       " AND class_name = '" & sClassName.Trim() & "'"

                'Check if we have already created this component, if we have then we dont need to again
                If CopySQLExists(vComponentID:=lID) Then

                    CopyLogSQL(sSql, NavProcConst.ACSQLStatement)

                    'Indicate success
                    Return gPMConstants.PMEReturnCode.PMTrue

                End If

                'Null id
                CopyLogSQL("Select @sub_component_id = Null", NavProcConst.ACSQLStatement)
                CopyLogSQL(sSql, NavProcConst.ACSQLStatement)

                'Push onto stack
                CopyLogSQLStack(NavProcConst.ACSQLPush)


                'Copy the Component
                lNewID = CType(CopyComponent(v_lComponentID:=lID, v_sNewComponent:=sDescription), gPMConstants.PMEReturnCode)


                'Get the process id
                CopyLogSQL("Save New Component", NavProcConst.ACSQLComment)
                CopyLogSQL("Select @temp_id = @pmnav_component_id", NavProcConst.ACSQLStatement)

                'Pull off stack
                CopyLogSQLStack(NavProcConst.ACSQLPull)

                CopyLogSQL("Get Stored component id", NavProcConst.ACSQLComment)
                'Check if we have a match
                CopyLogSQL("IF @sub_component_id IS NULL", NavProcConst.ACSQLStatement)
                CopyLogSQL(NavProcConst.ACTAB & "Select @sub_component_id = @temp_id", NavProcConst.ACSQLStatement)
                CopyLogSQL("Else ", NavProcConst.ACSQLStatement)
                CopyLogSQL(NavProcConst.ACTAB & "BEGIN", NavProcConst.ACSQLStatement)
                CopyLogSQL(NavProcConst.ACTAB & "DELETE FROM PMNav_Set_Component_Key WHERE pmnav_component_id = @temp_id", NavProcConst.ACSQLStatement)
                CopyLogSQL(NavProcConst.ACTAB & "DELETE FROM PMNav_Get_Component_Key WHERE pmnav_component_id = @temp_id", NavProcConst.ACSQLStatement)
                CopyLogSQL(NavProcConst.ACTAB & "DELETE FROM PMNav_Component WHERE pmnav_component_id = @temp_id", NavProcConst.ACSQLStatement)
                CopyLogSQL(NavProcConst.ACTAB & "END", NavProcConst.ACSQLStatement)

                'Push everything onto stack
                CopyLogSQLEnd("")

                'Add this component to the component lookup list
                m_cComponentIDs.Add(lID, CStr(lID))


                Return gPMConstants.PMEReturnCode.PMTrue

            Else

                'Check if we have a corresponding record in current DB
                sSql = "SELECT pmnav_component_id FROM PMNav_Component " & _
                       "WHERE object_name = '" & sObjectName.Trim() & "'" & _
                       " AND class_name = '" & sClassName.Trim() & "'"

                'Check if Component exists
                m_lReturn = m_oDatabase.SQLSelect(sSql:=sSql, sSQLName:="CheckComponent", bStoredProcedure:=False, vResultArray:=vResultSet)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Information.IsArray(vResultSet) Then

                    'If a corresponding record exists then get it

                    Return CInt(Conversion.Val(CStr(vResultSet(0, 0))))
                Else

                    'Copy the Component
                    lNewID = CType(CopyComponent(v_lComponentID:=lID, v_sNewComponent:=sDescription), gPMConstants.PMEReturnCode)

                    Return lNewID

                End If

            End If

        Catch
        End Try


        Return result
    End Function

    ' ***************************************************************** '
    ' Name: DSNSourceMap (Private)
    '
    ' Description: Synchronise required Map from Database Source
    '
    ' ***************************************************************** '
    Private Function DSNSourceMap(ByRef lID As Integer, ByRef sFieldName As String) As Integer

        Dim result As Integer = 0
        Dim sSql As String = ""
        Dim vResultSet(,) As Object

        Dim sCurrentCat As String = ""
        Dim lNewID As gPMConstants.PMEReturnCode
        Dim sCode, sDescription As String

        Try


            'Get the compare fields
            sSql = "SELECT code, description FROM PMNav_Map " & _
                   "WHERE pmnav_Map_id = " & CStr(lID)

            m_lReturn = m_oSourceDatabase.SQLSelect(sSql:=sSql, sSQLName:="GetTransType", bStoredProcedure:=False, vResultArray:=vResultSet)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Information.IsArray(vResultSet) Then
                Return result
            End If


            sCode = CStr(vResultSet(0, 0))

            sDescription = CStr(vResultSet(1, 0))

            If m_bBuildCopyScript Then

                'Construct lookup SQL
                sSql = "SELECT @sub_nav_map_id = pmnav_map_id, @effective_date2 = effective_date FROM PMNav_Map " & _
                       "WHERE code = '" & sCode.Trim() & "' AND description = '" & sDescription.Trim() & "'"

                'Check if we have already created this map, if we have then we dont need to again
                If CopySQLExists(vMapID:=lID) Then

                    CopyLogSQL(sSql, NavProcConst.ACSQLStatement)

                    'Indicate success
                    Return gPMConstants.PMEReturnCode.PMTrue

                End If

                'Null id before lookup
                CopyLogSQL("Select @sub_nav_map_id = Null", NavProcConst.ACSQLStatement)
                CopyLogSQL(sSql, NavProcConst.ACSQLStatement)

                'Push onto stack
                CopyLogSQLStack(NavProcConst.ACSQLPush)


                'Copy the Map
                lNewID = CType(CopyMap(v_lMapID:=lID, v_sNewMap:=sDescription), gPMConstants.PMEReturnCode)


                'Get the process id
                CopyLogSQL("Save New Map", NavProcConst.ACSQLComment)
                CopyLogSQL("Select @temp_id = @pmnav_map_id", NavProcConst.ACSQLStatement)

                'Pull off stack
                CopyLogSQLStack(NavProcConst.ACSQLPull)

                'Get the Effective date of the new map
                CopyLogSQL("Get the Map Effective date for comparision", NavProcConst.ACSQLComment)
                CopyLogSQL("Select @effective_date = effective_date FROM PMNav_Map " & _
                           "WHERE pmnav_map_id = @temp_id", NavProcConst.ACSQLStatement)

                'Check if we have a match
                CopyLogSQL("IF (@sub_nav_map_id IS NULL) OR (@effective_date2 <> @effective_date)", NavProcConst.ACSQLStatement)
                CopyLogSQL(NavProcConst.ACTAB & "Select @sub_nav_map_id = @temp_id", NavProcConst.ACSQLStatement)
                CopyLogSQL("Else ", NavProcConst.ACSQLStatement)
                CopyLogSQL(NavProcConst.ACTAB & "BEGIN", NavProcConst.ACSQLStatement)
                CopyLogSQL(NavProcConst.ACTAB & "DELETE FROM PMNav_Set_Map_Key WHERE pmnav_map_id = @temp_id", NavProcConst.ACSQLStatement)
                CopyLogSQL(NavProcConst.ACTAB & "DELETE FROM PMNav_Set_Step_Key WHERE pmnav_map_id = @temp_id", NavProcConst.ACSQLStatement)
                CopyLogSQL(NavProcConst.ACTAB & "DELETE FROM PMNav_Get_Step_Key WHERE pmnav_map_id = @temp_id", NavProcConst.ACSQLStatement)
                CopyLogSQL(NavProcConst.ACTAB & "DELETE FROM PMNav_Step WHERE pmnav_map_id = @temp_id", NavProcConst.ACSQLStatement)
                CopyLogSQL(NavProcConst.ACTAB & "DELETE FROM PMNav_Map WHERE pmnav_map_id = @temp_id", NavProcConst.ACSQLStatement)
                CopyLogSQL(NavProcConst.ACTAB & "END", NavProcConst.ACSQLStatement)

                CopyLogSQLEnd("")

                'Add this map to the map lookup list
                m_cMapIDs.Add(lID, CStr(lID))


                Return gPMConstants.PMEReturnCode.PMTrue

            End If

            'Check if we have a corresponding record in current DB
            sSql = "SELECT pmnav_map_id FROM PMNav_Map " & _
                   "WHERE code = '" & sCode.Trim() & "' AND description = '" & sDescription.Trim() & "'"

            vResultSet = Nothing

            'Check if Map exists
            m_lReturn = m_oDatabase.SQLSelect(sSql:=sSql, sSQLName:="CheckMap", bStoredProcedure:=False, vResultArray:=vResultSet)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If Information.IsArray(vResultSet) Then

                'If a corresponding record exists then get it

                Return CInt(Conversion.Val(CStr(vResultSet(0, 0))))

            Else

                'Copy the Map
                lNewID = CType(CopyMap(v_lMapID:=lID, v_sNewMap:=sDescription), gPMConstants.PMEReturnCode)

                Return lNewID

            End If

        Catch
        End Try



        Return result
    End Function


    ' ***************************************************************** '
    ' Name: DSNSourceProcess (Private)
    '
    ' Description: Synchronise required Process from Database Source
    '
    ' ***************************************************************** '
    Private Function DSNSourceProcess(ByRef lID As Integer, ByRef sFieldName As String) As Integer

        Dim result As Integer = 0
        Dim sCurrentCat As String = ""


        Dim lNewID As gPMConstants.PMEReturnCode
        Dim sCode, sDescription, sSql As String
        Dim vResultSet(,) As Object

        Try


            'Get the description
            sSql = "SELECT code, description FROM PMNav_Process " & _
                   "WHERE pmnav_process_id = " & CStr(lID)

            m_lReturn = m_oSourceDatabase.SQLSelect(sSql:=sSql, sSQLName:="GetTransType", bStoredProcedure:=False, vResultArray:=vResultSet)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Information.IsArray(vResultSet) Then
                Return result
            End If


            sCode = CStr(vResultSet(0, 0))

            sDescription = CStr(vResultSet(1, 0))

            If m_bBuildCopyScript Then

                CopyLogSQL("Select @" & sFieldName & " = Null", NavProcConst.ACSQLStatement)
                CopyLogSQL("SELECT @" & sFieldName & " = pmnav_process_id FROM PMNav_Process " & _
                           "WHERE code = '" & sCode.Trim() & "' AND description = '" & sDescription.Trim() & "'", NavProcConst.ACSQLStatement)

                'Check if we have already created this process, if we have then we dont need to again
                If CopySQLExists(vProcessID:=lID) Then

                    'Indicate success
                    Return gPMConstants.PMEReturnCode.PMTrue

                End If

                'Push onto stack
                CopyLogSQLStack(NavProcConst.ACSQLPush)

                'Copy the Process
                lNewID = CType(CopyProcess(v_lProcessID:=lID, v_sNewProcess:=sDescription), gPMConstants.PMEReturnCode)

                'Get the process id
                CopyLogSQL("Save New Process", NavProcConst.ACSQLComment)
                CopyLogSQL("Select @temp_id = @pmnav_process_id", NavProcConst.ACSQLStatement)


                'Pull off stack
                CopyLogSQLStack(NavProcConst.ACSQLPull)

                CopyLogSQL("Get Stored process id", NavProcConst.ACSQLComment)
                CopyLogSQL(NavProcConst.ACTAB & "Select @" & sFieldName & " = @temp_id", NavProcConst.ACSQLStatement)

                'Add this process to the process lookup list
                m_cProcessIDs.Add(lID, CStr(lID))


                Return gPMConstants.PMEReturnCode.PMTrue

            End If

            'Check if we have a corresponding record in current DB
            sSql = "SELECT pmnav_process_id FROM PMNav_Process " & _
                   "WHERE code = '" & sCode.Trim() & "' AND description = '" & sDescription.Trim() & "'"

            vResultSet = Nothing

            'Check if Process exists
            m_lReturn = m_oDatabase.SQLSelect(sSql:=sSql, sSQLName:="CheckProcess", bStoredProcedure:=False, vResultArray:=vResultSet)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If Information.IsArray(vResultSet) Then

                'If a corresponding record exists then get it

                Return CInt(Conversion.Val(CStr(vResultSet(0, 0))))

            Else

                'Copy the Process
                lNewID = CType(CopyProcess(v_lProcessID:=lID, v_sNewProcess:=sDescription), gPMConstants.PMEReturnCode)

                Return lNewID

            End If

        Catch
        End Try



        Return result
    End Function

    ' ***************************************************************** '
    ' Name: DSNSourceProc_Lock_Group (Private)
    '
    ' Description: Synchronise required ProcLock from Database Source
    '
    ' ***************************************************************** '
    Private Function DSNSourceProc_Lock_Group(ByRef lID As Integer) As Integer

        Dim sSql As String = ""
        Dim vResultSet(,) As Object = Nothing
        Dim lNewID As Integer = Nothing
        Dim sCode As String = ""
        Dim iIs_deleted As gPMConstants.PMEReturnCode = Nothing
        Dim sEffective_date As String = ""

        Dim sDescription As String = ""

        Try

            'Set the lock group to 0
            Return CInt("'Null'")

            'Get the compare fields
            sSql = "SELECT code, description FROM PMProc_Lock_Group " & _
                   "WHERE pmproc_lock_group_id = " & CStr(lID)

            m_lReturn = m_oSourceDatabase.SQLSelect(sSql:=sSql, sSQLName:="GetDB", bStoredProcedure:=False, vResultArray:=vResultSet)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Function
            End If

            If Not Information.IsArray(vResultSet) Then
                Exit Function
            End If


            sCode = CStr(vResultSet(0, 0))

            sDescription = CStr(vResultSet(1, 0))

            'Check if we have a corresponding record in current DB
            sSql = "SELECT pmproc_lock_group_id FROM PMProc_Lock_Group " & _
                   "WHERE code = '" & sCode

            vResultSet = Nothing

            'Check if ProcLock exists
            m_lReturn = m_oDatabase.SQLSelect(sSql:=sSql, sSQLName:="CheckDB", bStoredProcedure:=False, vResultArray:=vResultSet)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If Information.IsArray(vResultSet) Then

                'If a corresponding record exists then get it

                Return CInt(Conversion.Val(CStr(vResultSet(0, 0))))
            Else

                'Get new ID
                lNewID = GetNewID("PMProc_Lock_Group", "pmproc_lock_group_id")

                ' Check that we have a Caption
                If lNewID = 0 Then
                    Exit Function
                End If

                sCode = "'" & sCode & "'"
                sDescription = "'" & sDescription & "'"
                iIs_deleted = gPMConstants.PMEReturnCode.PMFalse
                sEffective_date = "'" & DateTime.Today.ToString("MM/dd/yyyy") & "'"

                sSql = "INSERT INTO PMProc_Lock_Group(" & _
                       "pmproc_lock_group_id, code, " & _
                       "description, is_deleted, effective_date, lparent_lock_group_id) " & _
                       "VALUES(" & CStr(lNewID) & ", " & sCode & ", " & _
                       sDescription & ", " & CStr(iIs_deleted) & ", " & _
                       sEffective_date & ", " & CStr(lNewID) & ")"

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLAction(sSql:=sSql, sSQLName:="InsertFields", bStoredProcedure:=False)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Function
                End If

                'Return new id
                Return lNewID

            End If

        Catch
        End Try



    End Function



    ' ***************************************************************** '
    ' Name: GetServerPMRegSetting
    '
    ' Description: Get registry setting from server
    '
    ' ***************************************************************** '
    Public Function GetServerPMRegSetting(ByVal v_sSettingName As String, ByRef r_sSettingValue As String) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
            Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
            Dim eProductFamily As gPMConstants.PMEProductFamily

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLServer
            eProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=v_sSettingName, r_sSettingValue:=r_sSettingValue), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or r_sSettingValue = "" Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetServerPMRegSetting Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetServerPMRegSetting", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: InsertStep (Public)
    '
    ' Description: Insert a step between other steps
    '
    ' ***************************************************************** '
    Public Function InsertStep(ByRef lMapID As Integer, ByRef lStepID As Integer) As Integer

        Dim result As Integer = 0
        Dim sSql As String = ""
        Dim vResultSet(,) As Object = Nothing
        Dim vGetKeys(,) As Object
        Dim vSetKeys(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get the Get Keys
            sSql = "Select pmnav_map_id, pmnav_step_id, pmnav_key_id, " & _
                   "sequence_number, description FROM PMNav_Get_Step_Key " & _
                   "WHERE pmnav_map_id = " & CStr(lMapID) & _
                   " AND pmnav_step_id >= " & CStr(lStepID)

            m_lReturn = m_oDatabase.SQLSelect(sSql:=sSql, sSQLName:="GetGetKeys", bStoredProcedure:=False, vResultArray:=vGetKeys)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Remove Get Keys
            sSql = "DELETE FROM PMNav_Get_Step_Key " & _
                   "WHERE pmnav_map_id = " & CStr(lMapID) & _
                   " AND pmnav_step_id >= " & CStr(lStepID)

            m_lReturn = m_oDatabase.SQLAction(sSql:=sSql, sSQLName:="DeleteGetKeys", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'Get the Set Keys
            sSql = "Select pmnav_map_id, pmnav_step_id, pmnav_key_id, " & _
                   "sequence_number, description, initial_key_value " & _
                   "FROM PMNav_Set_Step_Key " & _
                   "WHERE pmnav_map_id = " & CStr(lMapID) & _
                   " AND pmnav_step_id >= " & CStr(lStepID)

            m_lReturn = m_oDatabase.SQLSelect(sSql:=sSql, sSQLName:="GetSetKeys", bStoredProcedure:=False, vResultArray:=vSetKeys)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Remove the Set Keys
            sSql = "DELETE FROM PMNav_Set_Step_Key " & _
                   "WHERE pmnav_map_id = " & CStr(lMapID) & _
                   " AND pmnav_step_id >= " & CStr(lStepID)

            m_lReturn = m_oDatabase.SQLAction(sSql:=sSql, sSQLName:="DeleteSetKeys", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Update the Steps position
            sSql = "UPDATE PMNav_Step SET pmnav_step_id = pmnav_step_id + 1 " & _
                   " WHERE pmnav_map_id = " & CStr(lMapID) & _
                   " AND pmnav_step_id >= " & CStr(lStepID)

            m_lReturn = m_oDatabase.SQLAction(sSql:=sSql, sSQLName:="UpdateStepIDs", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            'Restore Get Keys

            If Information.IsArray(vGetKeys) Then


                For iPtr As Integer = vGetKeys.GetLowerBound(1) To vGetKeys.GetUpperBound(1)

                    'Update the StepID


                    vGetKeys(1, iPtr) = CDbl(vGetKeys(1, iPtr)) + 1


                    sSql = "INSERT INTO PMNav_Get_Step_Key(pmnav_map_id, pmnav_step_id, pmnav_key_id, sequence_number, description) VALUES(" & CStr(vGetKeys(0, iPtr)) & "," & CStr(vGetKeys(1, iPtr)) & "," & CStr(vGetKeys(2, iPtr)) & "," & CStr(vGetKeys(3, iPtr)) & ",'" & CStr(vGetKeys(4, iPtr)) & "')"

                    m_lReturn = m_oDatabase.SQLAction(sSql:=sSql, sSQLName:="RestoreStepGetKeys", bStoredProcedure:=False)


                Next iPtr

            End If

            'Restore Set Keys

            If Information.IsArray(vSetKeys) Then


                For iPtr As Integer = vSetKeys.GetLowerBound(1) To vSetKeys.GetUpperBound(1)

                    'Update the StepID




                    vSetKeys(1, iPtr) = Conversion.Val(CStr(vSetKeys(1, iPtr))) + 1


                    sSql = "INSERT INTO PMNav_Set_Step_Key(pmnav_map_id, pmnav_step_id, pmnav_key_id, sequence_number, description, initial_key_value) VALUES(" & CStr(vSetKeys(0, iPtr)) & "," & CStr(vSetKeys(1, iPtr)) & "," & CStr(vSetKeys(2, iPtr)) & "," & CStr(vSetKeys(3, iPtr)) & ",'" & CStr(vSetKeys(4, iPtr)) & "','" & CStr(vSetKeys(5, iPtr)) & "')"

                    m_lReturn = m_oDatabase.SQLAction(sSql:=sSql, sSQLName:="RestoreStepSetKeys", bStoredProcedure:=False)

                Next iPtr

            End If

            Return result

        Catch excep As System.Exception


            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InsertStep Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="InsertStep", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Sub InsertStepIDs(ByRef lMapID As Integer, ByRef lStepID As Integer)

        'Set the Step IDs for the step that is used on an insert

        m_lInsertStepMapID = lMapID
        m_lInsertStepStepID = lStepID

    End Sub
    Private Sub RuleLogClear()

        m_sRuleLog = ""

    End Sub

    Private Sub CopyLogClear()

        m_sCopyLog = ""

        If m_bBuildCopyScript Then

            'Clear Copy log SQL
            CopyLogSQLClear()

            'Clear the output script
            CopyLogSQL("", NavProcConst.ACSQLStart)

        End If

    End Sub
    Private Sub RuleLogText(ByVal sTextLine As String)

        m_sRuleLog = m_sRuleLog & sTextLine & Strings.Chr(13) & Constants.vbLf

    End Sub

    Private Sub CopyLogText(ByRef sTextLine As String)

        m_sCopyLog = m_sCopyLog & sTextLine & Strings.Chr(13) & Strings.Chr(10)

        If m_bBuildCopyScript Then

            'If copy output is on then log comment
            CopyLogSQL(sTextLine.ToUpper(), NavProcConst.ACSQLComment)

        End If

    End Sub
    ' ***************************************************************** '
    ' Name: RuleGSKStepMapSetKeys (Public)
    '
    ' Description: Enforce Rules for the Insert/Update of Steps
    '
    ' When a creating a step (either Sub Map or component)
    ' the Set Keys required by the Step MUST be available
    ' in the Current Map, either as
    '
    '    1 - A Map Set Key
    '    2 - Returned as a Get key from a previous component step
    '    3 - An initial value on the Step set key
    '
    ' ***************************************************************** '
    Public Function RuleGSKStepMapSetKeys(ByVal v_lMapID As Integer, ByVal v_lStepID As Integer) As Integer
        Dim Err_KeyDoesNotExist As Boolean = False
        Dim Err_RuleGSKStepMapSetKeys As Boolean = False

        Dim result As Integer = 0
        Dim sSql As String = ""
        Dim vResultSet(,) As Object
        Dim vMissingKeys(,) As Object
        Dim vValuesArray(2) As Object
        Dim lMapID, lKeyID As Integer
        Dim iMissingKeysCount As Integer

        Try
            Err_RuleGSKStepMapSetKeys = True
            Err_KeyDoesNotExist = False

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get the start map for the current process (Current Map)

            sSql = "SELECT start_nav_map_id FROM PMNav_Process " & _
                   "WHERE pmnav_process_id  = " & CStr(m_lCurrentProcessID)

            m_lReturn = m_oDatabase.SQLSelect(sSql:=sSql, sSQLName:="GetStartMap", bStoredProcedure:=False, vResultArray:=vResultSet)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Check if we have data
            If Not Information.IsArray(vResultSet) Then
                '(No map for current Process!!)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the Start Map ID

            lMapID = CInt(Conversion.Val(CStr(vResultSet(0, 0))))

            'Clear the list of Current Map Keys
            cCurrentMapKeys = Nothing
            cCurrentMapKeys = New Collection()

            'Build the Current MapKeys Upto the current step.
            m_lReturn = CType(BuildMapKeys(v_lMapID:=lMapID, v_vUptoMapID:=CStr(v_lMapID), v_vUptoStepID:=CStr(v_lStepID)), gPMConstants.PMEReturnCode)

            'PMCancel and PMTrue are valid and may be returned
            If (m_lReturn = gPMConstants.PMEReturnCode.PMFalse) Or (m_lReturn = gPMConstants.PMEReturnCode.PMError) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Log which Map we are checking
            RuleLogText("Missing Set-Keys for Current Step's Map = ")

            'Get the Set Keys for current step

            sSql = " SELECT pmnav_key_id, description, initial_key_value " & _
                   "FROM PMNav_Set_Step_Key " & _
                   "WHERE ( initial_key_value Is Null " & _
                   "OR initial_key_value = '' )" & _
                   " AND pmnav_map_id = " & CStr(v_lMapID) & _
                   " AND pmnav_step_id = " & CStr(v_lStepID)

            m_lReturn = m_oDatabase.SQLSelect(sSql:=sSql, sSQLName:="GetMissingKeys", bStoredProcedure:=False, vResultArray:=vMissingKeys)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Information.IsArray(vMissingKeys) Then
                'No missing Keys
                RuleLogText("none")
                Return result
            End If

            'Set the number of missing keys to 0
            iMissingKeysCount = 0

            'Use error handler if Key does not exist in current Map Keys
            Err_KeyDoesNotExist = True
            Err_RuleGSKStepMapSetKeys = False

            'Insert all missing keys into Map Set_Keys

            For iKeyPtr As Integer = vMissingKeys.GetLowerBound(1) To vMissingKeys.GetUpperBound(1)


                'Check if the Key exists in Current Map Keys

                lKeyID = CInt(Conversion.Val(CStr(vMissingKeys(0, iKeyPtr))))
                lKeyID = CInt(Conversion.Val(CStr(cCurrentMapKeys.Item(CStr(lKeyID)))))

                'If key does not exist in Current Map Keys collection then add
                'key to the SetKeys of the Map that this Step is in
                If lKeyID = 0 Then

                    'Add the desciption of missing keys

                    RuleLogText(CStr(vMissingKeys(1, iKeyPtr)))

                    'Get the key, description and initial key value
                    'vValuesArray(0) = vMissingKeys(0, iKeyPtr)
                    'vValuesArray(1) = vMissingKeys(1, iKeyPtr)
                    'vValuesArray(2) = vMissingKeys(2, iKeyPtr)

                    'Insert the key into the map set-keys for this Step
                    'm_lReturn = InsertGSKKeys( _
                    'v_sPMNavGroup:=NavGrpMap, _
                    'v_lID:=v_lMapID, _
                    'v_lMapID:=0, _
                    'v_vValuesArray:=vValuesArray, _
                    'v_sGSKType:=ACSetKey)

                    'If (m_lReturn <> PMTrue) Then
                    '    RuleGSKStepMapSetKeys = PMFalse
                    '    Exit Function
                    'End If

                    'Increment Missing Keys Count
                    'iMissingKeysCount = iMissingKeysCount + 1

                End If

            Next iKeyPtr

            'Log the count of missing keys
            'RuleLogText iMissingKeysCount & vbCr

            Return result

        Catch excep As System.Exception
            If Not Err_KeyDoesNotExist And Not Err_RuleGSKStepMapSetKeys Then
                Throw excep
            End If

            If Err_RuleGSKStepMapSetKeys Then

                result = gPMConstants.PMEReturnCode.PMError

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RuleGSKStepMapSetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RuleGSKStepMapSetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                Return result

            End If
            If Err_KeyDoesNotExist Or Err_RuleGSKStepMapSetKeys Then

                lKeyID = 0


                Return result
            End If
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RuleGSKMapProcessSync (Public)
    '
    ' Description: Returns missing process
    '
    '   Any Set Keys for the Start Map MUST be included
    '   as Set Keys for the Process.
    '
    ' ***************************************************************** '
    Public Function RuleGSKMapProcessSync(ByVal v_lProcessID As Integer, ByVal v_lMapID As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultSet(,) As Object
        Dim sSql As String
        Dim vValuesArray(2) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Return all missing keys

            sSql = "SELECT pnsmk.pmnav_key_id, pnsmk.description, " & _
                   "pnsmk.initial_key_value, pnk.description  " & _
                   "FROM PMNav_Set_Map_Key pnsmk, PMNav_Key pnk " & _
                   "WHERE pnk.pmnav_key_id = pnsmk.pmnav_key_id " & _
                   " AND pnsmk.pmnav_map_id = " & CStr(v_lMapID) & _
                   " AND pnsmk.pmnav_key_id NOT IN ( " & _
                   " SELECT " & _
                   "     pmnav_key_id " & _
                   " From " & _
                   "     PMNAV_Set_Process_Key " & _
                   " Where " & _
                   "     pmnav_process_id = " & CStr(v_lProcessID) & " )"

            m_lReturn = m_oDatabase.SQLSelect(sSql:=sSql, sSQLName:="GetProcessStartMap", bStoredProcedure:=False, vResultArray:=vResultSet)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Information.IsArray(vResultSet) Then

                RuleLogText("none")
                Return result

            End If

            'Insert all missing keys into Process Set_Keys

            For iPtr As Integer = vResultSet.GetLowerBound(1) To vResultSet.GetUpperBound(1)

                'Add the desciption of missing keys

                RuleLogText(CStr(vResultSet(3, iPtr)))

                '    'Get the key, description and initial key value
                '    vValuesArray(0) = vResultSet(0, iPtr)
                '    vValuesArray(1) = vResultSet(1, iPtr)
                '    vValuesArray(2) = vResultSet(2, iPtr)
                '
                '    'Insert the key into the process set keys
                '    m_lReturn = InsertGSKKeys( _
                ''        v_sPMNavGroup:=NavGrpProcess, _
                ''        v_lID:=v_lProcessID, _
                ''        v_lMapID:=0, _
                ''        v_vValuesArray:=vValuesArray, _
                ''        v_sGSKType:=ACSetKey)
                '
                '    If (m_lReturn <> PMTrue) Then
                '        RuleGSKMapProcessSync = PMFalse
                '        Exit Function
                '    End If
                '
            Next iPtr

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RuleGSKMapProcessSync Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RuleGSKMapProcessSync", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    ' ***************************************************************** '
    ' Name: RuleGSKProcessStartMapSync (Public)
    '
    ' Description: Returns missing process
    '
    '   Any Get Keys defined for the Process MUST be available in the
    '   Start Map as A Start Map Set Key, Returned as a get key from
    '   a component Step in the Start Map
    '
    ' ***************************************************************** '
    Public Function RuleGSKProcessStartMapSync(ByVal v_lMapID As Integer, ByVal v_lProcessID As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultSet(,) As Object
        Dim sSql As String
        Dim vValuesArray(2) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Return all missing keys

            sSql = "SELECT pngpk.pmnav_key_id, pngpk.Description, pnk.description " & _
                   "From PMNav_Get_Process_Key pngpk, PMNav_Key pnk " & _
                   "Where pnk.pmnav_key_id = pngpk.pmnav_key_id " & _
                   " AND pngpk.pmnav_process_id = " & CStr(v_lProcessID) & _
                   " AND pngpk.pmnav_key_id NOT IN ( " & _
                   "       SELECT pmnav_key_id " & _
                   "       From PMNav_Set_Map_Key " & _
                   "       Where pmnav_map_id = " & CStr(v_lMapID) & " ) " & _
                   "   AND pngpk.pmnav_key_id NOT IN ( " & _
                   "       SELECT pngsk.pmnav_key_id " & _
                   "       From PMNav_Step pns, PMNav_Get_Step_Key pngsk " & _
                   "       Where " & _
                   "           pns.pmnav_map_id = pngsk.pmnav_map_id " & _
                   "           AND pns.pmnav_step_id = pngsk.pmnav_step_id " & _
                   "           AND pns.pmnav_component_id is not null " & _
                   "           AND pns.pmnav_map_id = " & CStr(v_lMapID) & _
                   "   )"
            m_lReturn = m_oDatabase.SQLSelect(sSql:=sSql, sSQLName:="GetMissingKeys", bStoredProcedure:=False, vResultArray:=vResultSet)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Information.IsArray(vResultSet) Then

                RuleLogText("none")
                Return result

            End If

            'Insert all missing keys into Process Set_Keys

            For iPtr As Integer = vResultSet.GetLowerBound(1) To vResultSet.GetUpperBound(1)

                'Add the desciption of missing keys

                RuleLogText(CStr(vResultSet(2, iPtr)))

                'Get the key, description and initial key value
                'vValuesArray(0) = vResultSet(0, iPtr)
                'vValuesArray(1) = vResultSet(1, iPtr)
                'vValuesArray(2) = ""

                'Insert the key into the map set keys
                'm_lReturn = InsertGSKKeys( _
                ''    v_sPMNavGroup:=NavGrpMap, _
                ''    v_lID:=v_lMapID, _
                ''    v_lMapID:=0, _
                ''    v_vValuesArray:=vValuesArray, _
                ''    v_sGSKType:=ACSetKey)
                '
                'If (m_lReturn <> PMTrue) Then
                '    RuleGSKProcessStartMapSync = PMFalse
                '    Exit Function
                'End If

            Next iPtr

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RuleGSKProcessStartMapSync Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RuleGSKProcessStartMapSync", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RuleGSKComponent (Public)
    '
    ' Description: Updates all the Component's Steps Set/Get Keys
    '
    ' ***************************************************************** '
    Private Function RuleGSKComponent(ByRef lComponentID As Integer) As Integer

        Dim result As Integer = 0
        Dim sSql As String = ""
        Dim vResultSet(,) As Object



        result = gPMConstants.PMEReturnCode.PMTrue

        'Get all this component's steps
        sSql = "SELECT pmnav_map_id, pmnav_step_id FROM PMNav_Step " & _
               "WHERE pmnav_component_id = " & CStr(lComponentID)

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLSelect(sSql:=sSql, sSQLName:="ComponentSteps", bStoredProcedure:=False, vResultArray:=vResultSet)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Information.IsArray(vResultSet) Then


            For iPtr As Integer = vResultSet.GetLowerBound(1) To vResultSet.GetUpperBound(1)

                'Check this step component rules

                m_lReturn = CType(RuleGSKStepComponentSync(lID:=CInt(Conversion.Val(CStr(vResultSet(1, iPtr)))), lParentID:=CInt(Conversion.Val(CStr(vResultSet(0, iPtr))))), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Next iPtr

        End If

        Return result

    End Function


    Private Function RuleGSKProcess(ByRef lID As Integer) As Integer

        Dim result As Integer = 0
        Dim sSql As String = ""
        Dim vResultSet(,) As Object
        Dim lMapID As Integer
        Dim sDescription As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        'Get the start map for this process
        sSql = "SELECT start_nav_map_id,description FROM PMNav_Process " & _
               "WHERE pmnav_process_id  = " & CStr(lID)

        m_lReturn = m_oDatabase.SQLSelect(sSql:=sSql, sSQLName:="GetStartMap", bStoredProcedure:=False, vResultArray:=vResultSet)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Check if we have data
        If Not Information.IsArray(vResultSet) Then
            Return result
        End If

        'Get the Start Map

        lMapID = CInt(Conversion.Val(CStr(vResultSet(0, 0))))

        'Get the description of this process

        sDescription = CStr(vResultSet(1, 0))

        'Log rule text
        RuleLogText("Missing Keys for Process - " & _
                    sDescription & " = ")

        'Make sure Get-Keys defined for this process are
        'available in the start map
        m_lReturn = CType(RuleGSKProcessStartMapSync(v_lMapID:=lMapID, v_lProcessID:=lID), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: RuleGSKProcessSubProcSync (Public)
    '
    ' Description: Matches the SetKeys of a SubProcess to the GetKeys
    '               of parent process
    '
    ' ***************************************************************** '
    Private Function RuleGSKProcessSubProcSync(ByVal v_lMapID As Integer, ByVal v_lSubProcessID As Integer) As Integer

        Dim result As Integer = 0
        Dim sSql As String = ""
        Dim iPtr As Integer
        Dim vMissingKeys(,) As Object
        Dim vValuesArray(2) As Object



        result = gPMConstants.PMEReturnCode.PMTrue

        'Make sure that we have a current process

        If m_lCurrentProcessID <> 0 Then

            'Log which Process we are checking
            RuleLogText("Missing Get-Keys for Current Process = ")
        Else
            RuleLogText("Rules not enforced for Step Sub Process" & Strings.Chr(13) & _
                        "since a Current Process has not been selected.")
            Return result
        End If

        'Get the set-keys for the sub process that are not in the
        'get-keys for the parent process

        'Syncronise
        sSql = "SELECT pnspk.pmnav_key_id, pnspk.description, " & _
               "pnspk.initial_key_value, pnk.description " & _
               "From PMNav_Set_Process_Key pnspk, PMNav_Key pnk " & _
               "Where pnspk.pmnav_key_id = pnk.pmnav_key_id " & _
               " AND pnspk.pmnav_process_id = " & CStr(v_lSubProcessID) & _
               " AND pnspk.pmnav_key_id NOT IN ( " & _
               "SELECT pmnav_key_id " & _
               "From PMNAV_Get_Process_Key " & _
               "Where pmnav_process_id = " & CStr(m_lCurrentProcessID) & " )"

        m_lReturn = m_oDatabase.SQLSelect(sSql:=sSql, sSQLName:="GetMissingKeys", bStoredProcedure:=False, vResultArray:=vMissingKeys)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Information.IsArray(vMissingKeys) Then

            'No missing Keys
            RuleLogText("none")

        Else

            'Insert all missing keys into Process Set_Keys

            For iKeyPtr As Integer = vMissingKeys.GetLowerBound(1) To vMissingKeys.GetUpperBound(1)

                'Add the desciption of missing keys

                RuleLogText(CStr(vMissingKeys(3, iPtr)))

                'Get the key, description and initial key value
                'vValuesArray(0) = vMissingKeys(0, iKeyPtr)
                'vValuesArray(1) = vMissingKeys(1, iKeyPtr)
                'vValuesArray(2) = vMissingKeys(2, iKeyPtr)

                'Insert the key into the process set keys
                'm_lReturn = InsertGSKKeys( _
                'v_sPMNavGroup:=NavGrpProcess, _
                'v_lID:=m_lCurrentProcessID, _
                'v_lMapID:=0, _
                'v_vValuesArray:=vValuesArray, _
                'v_sGSKType:=ACGetKey)

                'If (m_lReturn <> PMTrue) Then
                '    RuleGSKProcessSubProcSync = PMFalse
                '    Exit Function
                'End If

            Next iKeyPtr

        End If

        Return result

    End Function


    Private Function RuleGSKMap(ByRef lID As Integer) As Integer

        Dim result As Integer = 0
        Dim sSql As String = ""
        Dim vResultSet(,) As Object
        Dim lProcessID As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        'Get Processes that start this map
        sSql = "SELECT pmnav_process_id, description " & _
               " FROM PMNav_Process " & _
               " WHERE start_nav_map_id = " & CStr(lID)

        m_lReturn = m_oDatabase.SQLSelect(sSql:=sSql, sSQLName:="GetMapProcesses", bStoredProcedure:=False, vResultArray:=vResultSet)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Exit if we have no processes
        If Not Information.IsArray(vResultSet) Then
            Return result
        End If

        'Synchronise StartMap and  Processes Set-Keys

        For iPtr As Integer = vResultSet.GetLowerBound(1) To vResultSet.GetUpperBound(1)

            'Log which Process we are checking

            RuleLogText("Missing Keys for Process - " & _
                        CStr(vResultSet(1, iPtr)) & " : ")

            'Get the process ID

            lProcessID = CInt(Conversion.Val(CStr(vResultSet(0, iPtr))))

            'Syncronise
            m_lReturn = CType(RuleGSKMapProcessSync(v_lProcessID:=lProcessID, v_lMapID:=lID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Next iPtr

        Return result

    End Function

    Private Function RuleGSKStep(ByRef v_lID As Integer, ByRef v_lMapID As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        'Make sure the Set Keys for this Step exist in the Current Map

        'Check for a current Process

        If m_lCurrentProcessID <> 0 Then

            m_lReturn = CType(RuleGSKStepMapSetKeys(v_lMapID:=v_lMapID, v_lStepID:=v_lID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Else

            RuleLogText("Rules not enforced for Step-Keys " & Strings.Chr(13) & _
                        "since a Current Process has not been selected.")
        End If

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: RuleGSKStepComponentSync (Private)
    '
    ' Description: Enforce Rules for the Insert/Update of Steps
    '
    ' A component Step MUST have all of the Mandatory Component
    ' Set Keys and all of the Component Get Keys.
    ' The user should then be able to select & include the optional
    ' Component Set Keys as required.
    '
    ' ***************************************************************** '
    Private Function RuleGSKStepComponentSync(ByRef lID As Integer, ByRef lParentID As Integer) As Integer

        Dim result As Integer = 0
        Dim sSql As String = ""
        Dim vResultSet(,) As Object
        Dim lComponentID As Integer
        Dim sDescription As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        'Get the Component_id for this Step

        lComponentID = 0

        sSql = "SELECT pmnav_component_id, description FROM PMNav_Step " & _
               "WHERE pmnav_map_id = " & CStr(lParentID) & _
               " AND pmnav_step_id = " & CStr(lID)

        m_lReturn = m_oDatabase.SQLSelect(sSql:=sSql, sSQLName:="SQLSelect", bStoredProcedure:=False, vResultArray:=vResultSet)

        'Store the componentID
        If Information.IsArray(vResultSet) Then

            lComponentID = CInt(Conversion.Val(CStr(vResultSet(0, 0))))

            sDescription = CStr(vResultSet(1, 0))
        End If

        'Get the missing SetKeys for component step
        sSql = "SELECT pnk.description " & _
               " FROM PMNav_Key pnk, PMNav_Set_Component_Key pnsck " & _
               " WHERE is_optional = 0 And pnsck.pmnav_component_id = " & CStr(lComponentID) & _
               " AND pnsck.pmnav_key_id = pnk.pmnav_key_id " & _
               " AND pnsck.pmnav_key_id NOT IN (" & _
               " SELECT pmnav_key_id FROM PMNav_Set_Step_Key " & _
               " WHERE pmnav_map_id = " & CStr(lParentID) & " AND pmnav_step_id = " & CStr(lID) & ")"

        m_lReturn = m_oDatabase.SQLSelect(sSql:=sSql, sSQLName:="SetKeys", bStoredProcedure:=False, vResultArray:=vResultSet)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Check if we have a resultset
        If Information.IsArray(vResultSet) Then

            RuleLogText("Step " & sDescription & " Missing Set Keys:")

            For iPtr As Integer = vResultSet.GetLowerBound(1) To vResultSet.GetUpperBound(1)

                RuleLogText(CStr(vResultSet(0, iPtr)))
            Next iPtr

        End If

        'Get missing get keys for component step
        sSql = "SELECT pnk.description " & _
               " FROM PMNav_Key pnk, PMNav_Get_Component_Key pngck " & _
               " WHERE pngck.pmnav_component_id = " & CStr(lComponentID) & _
               " AND pngck.pmnav_key_id = pnk.pmnav_key_id " & _
               " AND pngck.pmnav_key_id NOT IN (" & _
               " SELECT pmnav_key_id FROM PMNav_Get_Step_Key " & _
               " WHERE pmnav_map_id = " & CStr(lParentID) & " AND pmnav_step_id = " & CStr(lID) & ")"

        m_lReturn = m_oDatabase.SQLSelect(sSql:=sSql, sSQLName:="GetKeys", bStoredProcedure:=False, vResultArray:=vResultSet)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Check if we have a resultset
        If Information.IsArray(vResultSet) Then

            RuleLogText("Step " & sDescription & " Missing Get Keys:")

            For iPtr As Integer = vResultSet.GetLowerBound(1) To vResultSet.GetUpperBound(1)

                RuleLogText(CStr(vResultSet(0, iPtr)))
            Next iPtr

        End If

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: AddPMNavKey (Public)
    '
    ' Description: Adds a navigator key
    '
    ' ***************************************************************** '
    Public Function AddPMNavKey(ByRef sName As String, ByRef sDescription As String, ByRef lDataType As Integer, ByRef sEffectiveDate As String) As Integer

        Dim result As Integer = 0
        Dim sSql As String = ""
        Dim lID As gPMConstants.PMEReturnCode

        Try

            'Get the next PMNavKeyID
            lID = CType(GetNewID("PMNav_Key", "pmnav_key_id"), gPMConstants.PMEReturnCode)

            If lID = gPMConstants.PMEReturnCode.PMFalse Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the Fields
            Dim TempDate As Date
            sSql = "INSERT INTO PMNav_Key(pmnav_key_id, " & _
                   "name, " & _
                   "description, " & _
                   "data_type, " & _
                   "is_deleted, " & _
                   "effective_date) " & _
                   "VALUES(" & _
                   lID & ", '" & _
                   sName.Trim() & "', '" & _
                   sDescription.Trim() & "', " & _
                   lDataType & ", 0, '" & _
                   (IIf(DateTime.TryParse(sEffectiveDate, TempDate), TempDate.ToString("MM/dd/yyyy"), sEffectiveDate)) & "')"

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSql:=sSql, sSQLName:="NewPMNavKey", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return lID

        Catch


            Return 0
        End Try

    End Function
    ' ***************************************************************** '
    ' Name: EnforceRules (Private)
    '
    ' Description: Enforce Rules for the Insert/Update of Navigator Items
    '
    ' ***************************************************************** '
    Private Function EnforceRules(ByRef sPMNavGroup As String, ByRef lID As Integer, ByRef lParentID As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultSet(,) As Object
        Dim sSql As String = ""
        Dim lSubProcessID As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        'Clear Rule Log
        RuleLogClear()


        Select Case sPMNavGroup
            Case NavProcConst.NavGrpComponent

                'm_lReturn = RulePMNComponent()

            Case NavProcConst.NavGrpProcess

                'm_lReturn = RulePMNProcess()

            Case NavProcConst.NavGrpMap

                'm_lReturn = RulePMNMap()

            Case NavProcConst.NavGrpStep

                'Synchronise Step Keys with component Keys
                m_lReturn = CType(RuleGSKStepComponentSync(lID:=lID, lParentID:=lParentID), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Check if this Step has a Sub Process

                sSql = "SELECT ok_nav_process_id, cancel_nav_process_id " & _
                       "FROM PMNav_Step " & _
                       "WHERE pmnav_map_id = " & CStr(lParentID) & _
                       " AND pmnav_step_id = " & CStr(lID)

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSql:=sSql, sSQLName:="GetSubProcesses", bStoredProcedure:=False, vResultArray:=vResultSet)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'If we have no array then something is wrong
                If Not Information.IsArray(vResultSet) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Get ok processes for this step

                lSubProcessID = CInt(Conversion.Val(CStr(vResultSet(0, 0))))

                'If we have a sub process then synchronise it's keys
                'with parent process
                If lSubProcessID <> 0 Then

                    m_lReturn = CType(RuleGSKProcessSubProcSync(v_lMapID:=lParentID, v_lSubProcessID:=lSubProcessID), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

                'Get cancel processes for this step

                lSubProcessID = CInt(Conversion.Val(CStr(vResultSet(1, 0))))

                'If we have a sub process then synchronise it's keys
                'with parent process
                If lSubProcessID <> 0 Then

                    m_lReturn = CType(RuleGSKProcessSubProcSync(v_lMapID:=lParentID, v_lSubProcessID:=lSubProcessID), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

                'Enforce rules to synchronise map setkeys to step setkeys
                m_lReturn = CType(RuleGSKStepMapSetKeys(v_lMapID:=lParentID, v_lStepID:=lID), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Case Else

        End Select

        'Return status

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UpdatePMNavKey (Public)
    '
    ' Description: Adds a navigator key
    '
    ' ***************************************************************** '
    Public Function UpdatePMNavKey(ByRef lPMNavKeyID As Integer, ByRef sName As String, ByRef sDescription As String, ByRef lDataType As Integer, ByRef sEffectiveDate As String) As Integer

        Dim result As Integer = 0
        Dim sSql As String = ""
        Dim lID As Integer

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            'Get the PMNavKeyID
            lID = lPMNavKeyID

            If lID = 0 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the Fields
            Dim TempDate As Date
            sSql = "UPDATE PMNav_Key " & _
                   "SET name = '" & sName.Trim() & "', " & _
                   "description = '" & sDescription.Trim() & "', " & _
                   "data_type = " & CStr(lDataType) & ", " & _
                   "effective_date = '" & (IIf(DateTime.TryParse(sEffectiveDate, TempDate), TempDate.ToString("MM/dd/yyyy"), sEffectiveDate)) & "' " & _
                   "WHERE pmnav_key_id = " & CStr(lID)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSql:=sSql, sSQLName:="UpdatePMNavKey", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch


            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function


    ' ***************************************************************** '
    ' Name: BuildProcess (Public)
    '
    ' Description: Builds a Process
    '
    '
    ' ***************************************************************** '
    Public Function BuildProcess(ByRef lProcessID As Integer) As Integer

        Dim result As Integer = 0
        Dim sSql As String = ""
        Dim lStartMapID As Integer
        Dim sProcessMapEntry, sKey, sProcessDescription As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get the Process Name and Start Map
            sSql = "SELECT description, start_nav_map_id " & _
                   "FROM PMNav_Process " & _
                   "WHERE pmnav_process_id = " & CStr(lProcessID)

            m_lReturn = m_oDatabase.SQLSelect(sSql:=sSql, sSQLName:="GetProcessMap", bStoredProcedure:=False, lNumberRecords:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_oDatabase.Records.Count() = 0 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the process description
            sProcessDescription = m_oDatabase.Records.Item(1).Fields()("description")

            'Get the start map id
            ' To do list
            'lStartMapID = m_oDatabase.Records.Item(1).Fields()("start_nav_map_id")

            'Build Process Entry String [ParentID,(RP|PM|MS|SM)ID,Level, Description]
            sKey = ProcessMapCount() & NavProcConst.ACRootProcess & CStr(lProcessID)

            sProcessMapEntry = NavProcConst.ACRoot & "," & _
                               sKey & "," & _
                               NavProcConst.NodeProcess & "," & _
                               sProcessDescription

            'Add Process entry to Collection
            cReturnProcessMap.Add(sProcessMapEntry, sKey)

            'Build the Map
            m_lReturn = CType(BuildMap(lStartMapID, sKey), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BuildProcess Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildProcess", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: BuildMap (Public)
    '
    ' Description: Builds a Map
    '
    '
    ' ***************************************************************** '
    Public Function BuildMap(ByVal lMapID As Integer, ByRef sParent As String) As Integer

        Dim result As Integer = 0
        Dim vStepArray(,) As Object
        Dim sProcessMapEntry, sSql As String
        Dim sMapDescription As String = ""
        Dim sMapLevel, iLevel As Integer

        Dim sStepKey, sMapKey, sComponentKey As String

        Static iLevelNum As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Determine if this is a SubMap
            If Mid(sParent, 8, 2) = NavProcConst.ACMapStep Then
                sMapLevel = NavProcConst.NodeSubMap
            Else
                sMapLevel = NavProcConst.NodeMap
                iLevelNum = 0 'First Map
            End If

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'Keep a check of the number of Map Levels that we have built to.
            If iLevelNum >= ACMaxMapLevels Then
                Return result
            End If
            iLevelNum += 1
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            'Get the Map Description
            sSql = "SELECT Description FROM PMNav_Map WHERE pmnav_map_id = " & lMapID
            m_lReturn = m_oDatabase.SQLSelect(sSql:=sSql, sSQLName:="GetMapDescription", bStoredProcedure:=False, lNumberRecords:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_oDatabase.Records.Count() <> 0 Then
                ' RDC 21062002 changed from numbered to named field
                sMapDescription = m_oDatabase.Records.Item(1).Fields()("description")
            End If

            'Build Map Entry String [ParentID,(RP|PM|MS|SM)ID,Level,Description]
            'ProcessMapCount for unique key prefix (Count(7))

            sMapKey = ProcessMapCount() & NavProcConst.ACProcessMap & CStr(lMapID)

            sProcessMapEntry = sParent & "," & _
                               sMapKey & "," & _
                               sMapLevel & "," & _
                               sMapDescription

            'Add ProcessMap entry to Collection
            cReturnProcessMap.Add(sProcessMapEntry, sMapKey)

            'Get all the Steps for this Map including the component type
            'Modifying the inline query to make it compatible with SQL server 2005
            sSql = "SELECT pns.pmnav_step_id, pns.description, " & _
                   "pns.sub_nav_map_id, com.nav_component_type, " & _
                   "com.pmnav_component_id, com.description " & _
                   "FROM PMNav_Step pns LEFT OUTER JOIN PMNav_Component com " & _
                   "ON pns.pmnav_component_id = com.pmnav_component_id " & _
                   "WHERE pns.pmnav_map_id = " & CStr(lMapID) & _
                   " ORDER BY pmnav_step_id "

            m_lReturn = m_oDatabase.SQLSelect(sSql:=sSql, sSQLName:="GetSteps", bStoredProcedure:=False, vResultArray:=vStepArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Information.IsArray(vStepArray) Then
                Return result
            End If

            'Loop through all Steps and get any submaps


            For iPtr As Integer = vStepArray.GetLowerBound(1) To vStepArray.GetUpperBound(1)

                'Build Step Entry String
                '([ParentID],[RP|PM|MS|SM][ID;MAPID],Level,Description)

                sStepKey = ProcessMapCount() & NavProcConst.ACMapStep & CStr(vStepArray(0, iPtr)) & _
                           ";" & CStr(lMapID)

                'Get the Level for this Step (Type)



                Select Case CStr(vStepArray(3, iPtr))
                    Case gPMConstants.PMNavComponentFindForm
                        iLevel = NavProcConst.NodeStepFindForm

                    Case gPMConstants.PMNavComponentDecisionForm
                        iLevel = NavProcConst.NodeStepDecisionForm

                    Case gPMConstants.PMNavComponentDataForm
                        iLevel = NavProcConst.NodeStepDataForm

                    Case gPMConstants.PMNavComponentBusinessObject
                        iLevel = NavProcConst.NodeStepBusinessObject

                    Case Else
                        iLevel = NavProcConst.NodeStepSubMap

                End Select

                'Add the Node string

                sProcessMapEntry = sMapKey & "," & _
                                   sStepKey & "," & _
                                   iLevel & "," & _
                                   CStr(vStepArray(1, iPtr))

                'Add step to Collection
                cReturnProcessMap.Add(sProcessMapEntry, sStepKey)

                'Check if this is a Sub Map

                If CStr(vStepArray(2, iPtr)) <> "" Then

                    m_lReturn = CType(BuildMap(CInt(vStepArray(2, iPtr)), sStepKey), gPMConstants.PMEReturnCode)
                Else
                    'If this is a component then check if component display is on
                    If m_bStepComponent Then

                        sComponentKey = ProcessMapCount() & NavProcConst.ACStepComponent & CStr(vStepArray(4, iPtr))

                        'Add the Node string

                        sProcessMapEntry = sStepKey & "," & _
                                           sComponentKey & "," & _
                                           NavProcConst.NodeComponent & "," & _
                                           CStr(vStepArray(5, iPtr))

                        'Add step to Collection
                        cReturnProcessMap.Add(sProcessMapEntry, sComponentKey)

                    End If

                End If

            Next iPtr

            Return result

        Catch



            Return gPMConstants.PMEReturnCode.PMFalse
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: BuildComponent (Public)
    '
    ' Description: Builds a Component
    '
    '
    ' ***************************************************************** '
    Public Function BuildComponent(ByVal lComponentID As Integer, ByRef sParent As String) As Integer

        Dim result As Integer = 0
        Dim vStepArray(,) As Object
        Dim sProcessMapEntry, sSql As String
        Dim sComponentDescription As String = ""
        Dim sComponentLevel, iLevel As Integer

        Dim sStepKey, sComponentKey As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sComponentLevel = NavProcConst.NodeComponent

            'Get the Map Description
            sSql = "SELECT Description, nav_component_type " & _
                   "FROM PMNav_Component " & _
                   "WHERE pmnav_component_id = " & CStr(lComponentID)

            m_lReturn = m_oDatabase.SQLSelect(sSql:=sSql, sSQLName:="GetComponentDescription", bStoredProcedure:=False, lNumberRecords:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_oDatabase.Records.Count() <> 0 Then

                With m_oDatabase.Records.Item(1).Fields

                    'Get the component Description
                    sComponentDescription = m_oDatabase.Records.Item(1).Fields()("Description")

                    'Get the component type

                    Select Case m_oDatabase.Records.Item(1).Fields("nav_component_type")
                        Case gPMConstants.PMNavComponentFindForm
                            iLevel = NavProcConst.NodeStepFindForm

                        Case gPMConstants.PMNavComponentDecisionForm
                            iLevel = NavProcConst.NodeStepDecisionForm

                        Case gPMConstants.PMNavComponentDataForm
                            iLevel = NavProcConst.NodeStepDataForm

                        Case gPMConstants.PMNavComponentBusinessObject
                            iLevel = NavProcConst.NodeStepBusinessObject

                        Case Else

                    End Select

                End With

            End If

            'Build Component Entry String [ParentID,(RP|PM|MS|SM)ID,Level,Description]
            'ProcessMapCount for unique key prefix (Count(7))

            sComponentKey = ProcessMapCount() & NavProcConst.ACRootComponent & CStr(lComponentID)

            sProcessMapEntry = sParent & "," & _
                               sComponentKey & "," & _
                               sComponentLevel & "," & _
                               sComponentDescription

            'Add ProcessMap entry to Collection
            cReturnProcessMap.Add(sProcessMapEntry, sComponentKey)

            'Get all the Steps for this Map
            sSql = "SELECT pmnav_step_id, description, pmnav_map_id " & _
                   "FROM PMNav_Step WHERE pmnav_component_id = " & CStr(lComponentID) & _
                   " ORDER BY pmnav_step_id"

            m_lReturn = m_oDatabase.SQLSelect(sSql:=sSql, sSQLName:="GetSteps", bStoredProcedure:=False, vResultArray:=vStepArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Information.IsArray(vStepArray) Then
                Return result
            End If

            'Loop through all Steps and add


            For iPtr As Integer = vStepArray.GetLowerBound(1) To vStepArray.GetUpperBound(1)

                'Build Step Entry String ([ParentID],[RP|PM|MS|SM][ID;MAPID],[Level],[Description])


                sStepKey = ProcessMapCount() & NavProcConst.ACMapStep & CStr(vStepArray(0, iPtr)) & _
                           ";" & CStr(vStepArray(2, iPtr))


                sProcessMapEntry = sComponentKey & "," & _
                                   sStepKey & "," & _
                                   iLevel & "," & _
                                   CStr(vStepArray(1, iPtr))

                'Add step to Collection
                cReturnProcessMap.Add(sProcessMapEntry, sStepKey)

            Next iPtr


            Return result

        Catch


            Return gPMConstants.PMEReturnCode.PMFalse
        End Try

    End Function


    Public Function ClearProcessMap() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            cReturnProcessMap = Nothing
            cReturnProcessMap = New Collection()

            m_lProcessMapCount = 0

            Return result

        Catch


            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function


    ' ***************************************************************** '
    ' Name: GetAllKeys (Public)
    '
    ' Description: Get the PMNavKeys
    '
    ' ***************************************************************** '
    Public Function GetAllKeys(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSql As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get the Fields
            sSql = "SELECT pmnav_key_id, " & _
                   "name, " & _
                   "description, " & _
                   "data_type, " & _
                   "is_deleted, " & _
                   "effective_date " & _
                   "FROM PMNav_Key ORDER BY description"

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSql:=sSql, sSQLName:="GetAllKeys", bStoredProcedure:=False, vResultArray:=r_vResultArray)

            Return result

        Catch excep As System.Exception


            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetGSKKeys (Public)
    '
    ' Description: Get the GetSetKeys for a Navigator Item
    '
    ' ***************************************************************** '
    Public Function GetGSKKeys(ByVal v_sPMNavGroup As String, ByVal v_lID As Integer, ByVal v_lMapID As Integer, ByRef r_vResultArray(,) As Object, ByVal v_sGSKType As String) As Integer

        Dim result As Integer = 0
        Dim sSql, sTableName, sFieldList, sWhereClause As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Determine the Group that has been passed, and set
            'The SQL clauses


            Select Case v_sPMNavGroup
                Case NavProcConst.NavGrpComponent

                    sWhereClause = "pmnav_component_id = " & v_lID

                    If v_sGSKType = NavProcConst.ACSetKey Then
                        sTableName = "PMNav_Set_Component_Key"
                        sFieldList = "pmnav_key_id, description, is_optional"
                    Else
                        sTableName = "PMNav_Get_Component_Key"
                        sFieldList = "pmnav_key_id, description"
                    End If

                Case NavProcConst.NavGrpProcess

                    sWhereClause = "pmnav_process_id = " & v_lID

                    If v_sGSKType = NavProcConst.ACSetKey Then
                        sTableName = "PMNav_Set_Process_Key"
                        sFieldList = "pmnav_key_id, description, initial_key_value"
                    Else
                        sTableName = "PMNav_Get_Process_Key"
                        sFieldList = "pmnav_key_id, description"
                    End If

                Case NavProcConst.NavGrpMap

                    sWhereClause = "pmnav_map_id = " & v_lID

                    If v_sGSKType = NavProcConst.ACSetKey Then
                        sTableName = "PMNav_Set_Map_Key"
                        sFieldList = "pmnav_key_id, description, initial_key_value"
                    End If

                Case NavProcConst.NavGrpStep

                    sWhereClause = "pmnav_map_id = " & v_lMapID & _
                                   " AND pmnav_step_id = " & CStr(v_lID)

                    If v_sGSKType = NavProcConst.ACSetKey Then
                        sTableName = "PMNav_Set_Step_Key"
                        sFieldList = "pmnav_key_id, description, initial_key_value"
                    Else
                        sTableName = "PMNav_Get_Step_Key"
                        sFieldList = "pmnav_key_id, description"
                    End If

                Case Else
                    Return gPMConstants.PMEReturnCode.PMFalse

            End Select

            'Construct the SQL statement
            sSql = "SELECT " & sFieldList & _
                   " FROM " & sTableName & _
                   " WHERE " & sWhereClause

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSql:=sSql, sSQLName:="GetGSKKeys", bStoredProcedure:=False, vResultArray:=r_vResultArray)

            Return result

        Catch excep As System.Exception


            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetGSKKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetGSKKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EnforceGSKRules (Public)
    '
    ' Description: Applies Rules when a GSK has been updated
    '
    ' ***************************************************************** '
    Public Function EnforceGSKRules(ByVal v_sPMNavGroup As String, ByVal v_lID As Integer, ByVal v_lMapID As Integer) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear Rule Log
            RuleLogClear()

            'Determine the Group that has been passed, and set
            'The SQL clauses


            Select Case v_sPMNavGroup
                Case NavProcConst.NavGrpComponent

                    'Do not update component yet
                    m_lReturn = CType(RuleGSKComponent(v_lID), gPMConstants.PMEReturnCode)

                Case NavProcConst.NavGrpProcess

                    m_lReturn = CType(RuleGSKProcess(v_lID), gPMConstants.PMEReturnCode)

                Case NavProcConst.NavGrpMap

                    m_lReturn = CType(RuleGSKMap(v_lID), gPMConstants.PMEReturnCode)

                Case NavProcConst.NavGrpStep

                    m_lReturn = CType(RuleGSKStep(v_lID, v_lMapID), gPMConstants.PMEReturnCode)

                Case Else

                    m_lReturn = gPMConstants.PMEReturnCode.PMFalse

            End Select

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception


            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EnforceGSKRules Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EnforceGSKRules", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetKeys (Public)
    '
    ' Description: Get the GetSetKeys for a Navigator Item
    '
    ' ***************************************************************** '
    Public Function GetKeys(ByVal v_sPMNavGroup As String, ByVal v_lID As Integer, ByVal v_lMapID As Integer, ByRef r_vResultArray(,) As Object, ByVal v_sGSKType As String) As Integer

        Dim result As Integer = 0
        Dim sSql As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Determine the Group that has been passed, and set
            'The SQL clauses


            Select Case v_sPMNavGroup
                Case NavProcConst.NavGrpComponent, NavProcConst.NavGrpProcess, NavProcConst.NavGrpMap

                    'Get the Fields
                    sSql = "SELECT pmnav_key_id FROM PMNav_Key ORDER BY description"

                Case NavProcConst.NavGrpStep

                    'Set up the Select String According to Step Keys Rules

                    sSql = "SELECT" & _
                           " pnsc.pmnav_key_id " & _
                           " From " & _
                           " PMNav_Step pns, " & _
                           " PMNav_Set_Component_Key pnsc " & _
                           " Where " & _
                           " pnsc.pmnav_component_id = pns.pmnav_component_id " & _
                           " AND pns.pmnav_map_id = " & CStr(v_lMapID) & _
                           " AND pns.pmnav_step_id = " & CStr(v_lID) & _
                           " AND pnsc.is_optional = " & CStr(gPMConstants.PMEReturnCode.PMTrue)

                Case Else
                    Return gPMConstants.PMEReturnCode.PMFalse

            End Select


            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSql:=sSql, sSQLName:="GetKeys", bStoredProcedure:=False, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception


            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function





    ' ***************************************************************** '
    ' Name: DeleteGSKKeys (Public)
    '
    ' Description: Delete the GetSetKeys for a Navigator Item
    '
    ' ***************************************************************** '
    Public Function DeleteGSKKeys(ByVal v_sPMNavGroup As String, ByVal v_lID As Integer, ByVal v_lMapID As Integer, ByVal v_lPMNavKeyID As Integer, ByVal v_sGSKType As String) As Integer

        Dim result As Integer = 0
        Dim sSql, sTableName, sWhereClause As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Determine the Group that has been passed, and set
            'The SQL clauses


            Select Case v_sPMNavGroup
                Case NavProcConst.NavGrpComponent

                    sWhereClause = "pmnav_component_id = " & v_lID & _
                                   " AND pmnav_key_id = " & CStr(v_lPMNavKeyID)

                    If v_sGSKType = NavProcConst.ACSetKey Then
                        sTableName = "PMNav_Set_Component_Key"
                    Else
                        sTableName = "PMNav_Get_Component_Key"
                    End If

                Case NavProcConst.NavGrpProcess

                    sWhereClause = "pmnav_process_id = " & v_lID & _
                                   " AND pmnav_key_id = " & CStr(v_lPMNavKeyID)

                    If v_sGSKType = NavProcConst.ACSetKey Then
                        sTableName = "PMNav_Set_Process_Key"
                    Else
                        sTableName = "PMNav_Get_Process_Key"
                    End If

                Case NavProcConst.NavGrpMap

                    sWhereClause = "pmnav_map_id = " & v_lID & _
                                   " AND pmnav_key_id = " & CStr(v_lPMNavKeyID)

                    If v_sGSKType = NavProcConst.ACSetKey Then
                        sTableName = "PMNav_Set_Map_Key"
                    End If

                Case NavProcConst.NavGrpStep

                    sWhereClause = "pmnav_map_id = " & v_lMapID & _
                                   " AND pmnav_step_id = " & CStr(v_lID) & _
                                   " AND pmnav_key_id = " & CStr(v_lPMNavKeyID)

                    If v_sGSKType = NavProcConst.ACSetKey Then
                        sTableName = "PMNav_Set_Step_Key"
                    Else
                        sTableName = "PMNav_Get_Step_Key"
                    End If

                Case Else
                    Return gPMConstants.PMEReturnCode.PMFalse

            End Select

            'Construct the SQL statement
            sSql = "DELETE FROM " & sTableName & _
                   " WHERE " & sWhereClause

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSql:=sSql, sSQLName:="DeleteGSKKeys", bStoredProcedure:=False)

            Return result

        Catch excep As System.Exception


            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteGSKKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteGSKKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: InsertGSKKeys (Public)
    '
    ' Description: Insert the GetSetKeys for a Navigator Item
    '
    ' ***************************************************************** '
    Public Function InsertGSKKeys(ByVal v_sPMNavGroup As String, ByVal v_lID As Integer, ByVal v_lMapID As Integer, ByVal v_vValuesArray() As Object, ByVal v_sGSKType As String) As Integer

        Dim result As Integer = 0
        Dim sSql, sTableName, sFieldList, sFieldValues As String
        Dim lSequenceNumber As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Determine the Group that has been passed, and set
            'The SQL clauses


            Select Case v_sPMNavGroup
                Case NavProcConst.NavGrpComponent

                    If v_sGSKType = NavProcConst.ACSetKey Then

                        'Set the tablename
                        sTableName = "PMNav_Set_Component_Key"

                        'Get the next sequence number
                        lSequenceNumber = GetNewID(sTableName, "sequence_number", "pmnav_component_id = " & v_lID)

                        'List the field names
                        sFieldList = "pmnav_component_id, pmnav_key_id, " & _
                                     "sequence_number, description, is_optional"

                        'List the field values



                        sFieldValues = CStr(v_lID) & ", " & CStr(v_vValuesArray(0)) & ", " & _
                                       lSequenceNumber & ", '" & CStr(v_vValuesArray(1)) & "', " & _
                                       CStr(v_vValuesArray(2))

                    Else
                        'Set the table name
                        sTableName = "PMNav_Get_Component_Key"

                        'Get the next sequence number for this component
                        lSequenceNumber = GetNewID(sTableName, "sequence_number", "pmnav_component_id = " & v_lID)

                        'List the field names
                        sFieldList = "pmnav_component_id, pmnav_key_id, " & _
                                     "sequence_number, description"

                        'List the field values


                        sFieldValues = CStr(v_lID) & ", " & CStr(v_vValuesArray(0)) & ", " & _
                                       lSequenceNumber & ", '" & CStr(v_vValuesArray(1)) & "'"

                    End If

                Case NavProcConst.NavGrpProcess

                    If v_sGSKType = NavProcConst.ACSetKey Then

                        'Set the tablename
                        sTableName = "PMNav_Set_Process_Key"

                        'Get the next sequence number
                        lSequenceNumber = GetNewID(sTableName, "sequence_number", "pmnav_process_id = " & v_lID)

                        'List the field names
                        sFieldList = "pmnav_process_id, pmnav_key_id, " & _
                                     "sequence_number, description, initial_key_value"

                        'List the field values



                        sFieldValues = CStr(v_lID) & ", " & CStr(v_vValuesArray(0)) & ", " & _
                                       lSequenceNumber & ", '" & CStr(v_vValuesArray(1)) & "', '" & _
                                       CStr(v_vValuesArray(2)) & "'"
                    Else

                        'Set the table name
                        sTableName = "PMNav_Get_Process_Key"

                        'Get the next sequence number
                        lSequenceNumber = GetNewID(sTableName, "sequence_number", "pmnav_process_id = " & v_lID)

                        'List the field names
                        sFieldList = "pmnav_process_id, pmnav_key_id, " & _
                                     "sequence_number, description"

                        'List the field values


                        sFieldValues = CStr(v_lID) & ", " & CStr(v_vValuesArray(0)) & ", " & _
                                       lSequenceNumber & ", '" & CStr(v_vValuesArray(1)) & "'"

                    End If

                Case NavProcConst.NavGrpMap


                    If v_sGSKType = NavProcConst.ACSetKey Then

                        'Set the table name
                        sTableName = "PMNav_Set_Map_Key"

                        'Get the next sequence number
                        lSequenceNumber = GetNewID(sTableName, "sequence_number", "pmnav_map_id = " & v_lID)

                        'List the field names
                        sFieldList = "pmnav_map_id, pmnav_key_id, " & _
                                     "sequence_number, description, initial_key_value"

                        'List the field values



                        sFieldValues = CStr(v_lID) & ", " & CStr(v_vValuesArray(0)) & ", " & _
                                       lSequenceNumber & ", '" & CStr(v_vValuesArray(1)) & "', '" & _
                                       CStr(v_vValuesArray(2)) & "'"

                    End If

                Case NavProcConst.NavGrpStep

                    sFieldValues = "pmnav_map_id = " & v_lMapID & _
                                   " AND pmnav_step_id = " & CStr(v_lID)

                    If v_sGSKType = NavProcConst.ACSetKey Then

                        'Set the table name
                        sTableName = "PMNav_Set_Step_Key"

                        'Get the next sequence number
                        lSequenceNumber = GetNewID(sTableName, "sequence_number", "pmnav_map_id = " & v_lMapID & _
                                          " AND pmnav_step_id = " & CStr(v_lID))

                        'List the field names
                        sFieldList = "pmnav_map_id, pmnav_step_id, pmnav_key_id, " & _
                                     "sequence_number, description, initial_key_value"

                        'List the field values



                        sFieldValues = CStr(v_lMapID) & ", " & CStr(v_lID) & ", " & _
                                       CStr(v_vValuesArray(0)) & ", " & _
                                       lSequenceNumber & ", '" & _
                                       CStr(v_vValuesArray(1)) & "', '" & _
                                       CStr(v_vValuesArray(2)) & "'"

                    Else
                        'Set the table name
                        sTableName = "PMNav_Get_Step_Key"

                        'Get the next sequence number
                        lSequenceNumber = GetNewID(sTableName, "sequence_number", "pmnav_map_id = " & v_lMapID & _
                                          " AND pmnav_step_id = " & CStr(v_lID))


                        'List the field names
                        sFieldList = "pmnav_map_id, pmnav_step_id, pmnav_key_id, " & _
                                     "sequence_number, description"

                        'List the field values


                        sFieldValues = CStr(v_lMapID) & ", " & CStr(v_lID) & ", " & _
                                       CStr(v_vValuesArray(0)) & ", " & _
                                       lSequenceNumber & ", '" & _
                                       CStr(v_vValuesArray(1)) & "'"

                    End If

                Case Else
                    Return gPMConstants.PMEReturnCode.PMFalse

            End Select

            'Construct the SQL statement
            sSql = "INSERT INTO " & sTableName & "(" & sFieldList & ")" & _
                   " VALUES (" & sFieldValues & ")"

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSql:=sSql, sSQLName:="InsertGSKKeys", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception


            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InsertGSKKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="InsertGSKKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    ' ***************************************************************** '
    ' Name: UpdateGSKKeys (Public)
    '
    ' Description: Update the GetSetKeys for a Navigator Item
    '
    ' ***************************************************************** '
    Public Function UpdateGSKKeys(ByVal v_sPMNavGroup As String, ByVal v_lID As Integer, ByVal v_lMapID As Integer, ByVal v_lPMNavKey As Integer, ByVal v_vValuesArray() As Object, ByVal v_sGSKType As String) As Integer

        Dim result As Integer = 0
        Dim sSql, sTableName, sFieldList, sWhereClause As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Determine the Group that has been passed, and set
            'The SQL clauses


            Select Case v_sPMNavGroup
                Case NavProcConst.NavGrpComponent

                    'Set WhereClause
                    sWhereClause = "pmnav_component_id = " & v_lID & _
                                   " AND pmnav_key_id = " & CStr(v_lPMNavKey)

                    'Check the Key type
                    If v_sGSKType = NavProcConst.ACSetKey Then

                        'Set the tablename
                        sTableName = "PMNav_Set_Component_Key"

                        'Set the field list


                        sFieldList = "description = '" & CStr(v_vValuesArray(1)) & _
                                     "', is_optional = " & CStr(v_vValuesArray(2))
                    Else

                        'Set the table name
                        sTableName = "PMNav_Get_Component_Key"

                        'Set the field values for update

                        sFieldList = "description = '" & CStr(v_vValuesArray(1)) & "'"

                    End If

                Case NavProcConst.NavGrpProcess

                    'Set the where clause
                    sWhereClause = "pmnav_process_id = " & v_lID & _
                                   " AND pmnav_key_id = " & CStr(v_lPMNavKey)

                    If v_sGSKType = NavProcConst.ACSetKey Then

                        'Set the table name
                        sTableName = "PMNav_Set_Process_Key"

                        'Set the field names and values for update


                        sFieldList = "description = '" & CStr(v_vValuesArray(1)) & _
                                     "', initial_key_value = '" & CStr(v_vValuesArray(2)) & "'"
                    Else
                        sTableName = "PMNav_Get_Process_Key"

                        sFieldList = "description = '" & CStr(v_vValuesArray(1)) & "'"
                    End If

                Case NavProcConst.NavGrpMap

                    sWhereClause = "pmnav_map_id = " & v_lID & _
                                   " AND pmnav_key_id = " & CStr(v_lPMNavKey)

                    If v_sGSKType = NavProcConst.ACSetKey Then
                        sTableName = "PMNav_Set_Map_Key"



                        sFieldList = "description = '" & CStr(v_vValuesArray(1)) & _
                                     "', initial_key_value = '" & CStr(v_vValuesArray(2)) & "'"

                    End If

                Case NavProcConst.NavGrpStep

                    sWhereClause = "pmnav_map_id = " & v_lMapID & _
                                   " AND pmnav_step_id = " & CStr(v_lID) & _
                                   " AND pmnav_key_id = " & CStr(v_lPMNavKey)

                    If v_sGSKType = NavProcConst.ACSetKey Then
                        sTableName = "PMNav_Set_Step_Key"



                        sFieldList = "description = '" & CStr(v_vValuesArray(1)) & _
                                     "', initial_key_value = '" & CStr(v_vValuesArray(2)) & "'"

                    Else
                        sTableName = "PMNav_Get_Step_Key"

                        sFieldList = "description = '" & CStr(v_vValuesArray(1)) & "'"
                    End If

                Case Else
                    Return gPMConstants.PMEReturnCode.PMFalse

            End Select

            'Construct the SQL statement
            sSql = "UPDATE " & sTableName & _
                   " SET " & sFieldList & _
                   " WHERE " & sWhereClause

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSql:=sSql, sSQLName:="UpdateGSKKeys", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception


            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateGSKKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateGSKKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function



    ' ***************************************************************** '
    ' Name: GetFields (Public)
    '
    ' Description: Gets the Field values for a PMNav Group Item
    '
    ' ***************************************************************** '
    Public Function GetFields(ByRef sPMNavGroup As String, ByRef sFieldList As String, ByRef lID As Integer, ByRef r_vResultArray(,) As Object, ByRef lParentID As Integer) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim sSql, sTableName, sWhereClause As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Set the SQL Criteria for the Group


            Select Case sPMNavGroup
                Case NavProcConst.NavGrpComponent
                    sTableName = "PMNav_Component"
                    sWhereClause = "pmnav_component_id = " & lID

                Case NavProcConst.NavGrpProcess
                    sTableName = "PMNav_Process"
                    sWhereClause = "pmnav_process_id = " & lID

                Case NavProcConst.NavGrpMap
                    sTableName = "PMNav_Map"
                    sWhereClause = "pmnav_map_id = " & lID

                Case NavProcConst.NavGrpStep
                    sTableName = "PMNav_Step"
                    sWhereClause = "pmnav_map_id = " & lParentID & _
                                   " AND pmnav_step_id = " & CStr(lID)

                Case Else

            End Select

            'Get the Fields
            sSql = "SELECT " & sFieldList & " FROM " & sTableName & _
                   " WHERE " & sWhereClause

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSql:=sSql, sSQLName:="GetFields", bStoredProcedure:=False, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetFields Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFields", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetLookUpTable (Public)
    '
    ' Description: Gets the Lookup values for a bPMNavEditor.
    '
    ' ***************************************************************** '
    Public Function GetLookUpTable(ByVal v_iTableLookUp As Integer, ByRef r_vResultArray(,) As Object, Optional ByRef v_vWhereClause As String = "") As Integer

        Dim result As Integer = 0
        Dim sSqlIDName, sTableName, sSql As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Select Case v_iTableLookUp
                Case NavProcConst.ACLTabPMNav_Map, NavProcConst.ACLTabPMNav_StartMap, NavProcConst.ACLTabPMNav_SubMap

                    sSqlIDName = "pmnav_map_id, description"
                    sTableName = "PMNav_Map"

                Case NavProcConst.ACLTabPMNav_Component
                    sSqlIDName = "pmnav_component_id,description"
                    sTableName = "PMNav_Component"

                Case NavProcConst.ACLTabPMNav_Step
                    sSqlIDName = "pmnav_step_id,description"
                    sTableName = "PMNav_Step"

                Case NavProcConst.ACLTabPMNav_Process
                    sSqlIDName = "pmnav_process_id,description"
                    sTableName = "PMNav_Process"

                Case NavProcConst.ACLTabPMProduct
                    sSqlIDName = "pmproduct_id,description"
                    sTableName = "PMProduct"

                Case NavProcConst.ACLTabPMProc_Lock_Group
                    sSqlIDName = "pmproc_lock_group_id,description"
                    sTableName = "PMProc_Lock_Group"

                Case NavProcConst.ACLTabTransaction_Type
                    sSqlIDName = "transaction_type_id,description"
                    sTableName = "Transaction_Type"

            End Select

            'Build the SQL string
            sSql = "SELECT " & sSqlIDName & " FROM " & sTableName

            'Check if we have a where clause

            If Not Information.IsNothing(v_vWhereClause) Then
                sSql = sSql & " WHERE " & v_vWhereClause
            End If

            sSql = sSql & " ORDER BY Description"

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSql:=sSql, sSQLName:="GetLookUps", bStoredProcedure:=False, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception


            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookUpTable Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookUpTable", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetLookupValue (Public)
    '
    ' Description: Gets the Lookup value for a bPMNavEditor.
    '
    ' ***************************************************************** '
    Public Function GetLookUpValue(ByVal sTableName As String, ByVal lID As Integer) As String

        Dim result As String = String.Empty
        Dim sSql As String = ""

        Try

            result = ""


            Select Case sTableName
                Case "PMCaption"
                    sSql = "SELECT Caption FROM PMCaption " & _
                           "WHERE caption_id = " & CStr(lID)
                Case Else

            End Select

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSql:=sSql, sSQLName:="GetLookUpValue", bStoredProcedure:=False, lNumberRecords:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If m_oDatabase.Records.Count() = 0 Then
                Return result
            End If

            ' RDC 21062002 changed from numbered to named field

            Return m_oDatabase.Records.Item(1).Fields()("caption")

        Catch excep As System.Exception


            ' Error Section.

            result = CStr(gPMConstants.PMEReturnCode.PMError)

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookUpValue Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookUpValue", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Private Function ProcessMapCount() As String

        'Update the unique key ID
        m_lProcessMapCount += 1
        Return StringsHelper.Format(m_lProcessMapCount, "0000000")

    End Function
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
    Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As dPMDAO.Database = Nothing) As Integer




        Dim result As Integer = 0
        Try

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


            ' Initialisation Code.

            ' Set Username and Password

            ' Set UserID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level

            ' Have we a valid Database Object Reference?

            If (Not Information.IsNothing(vDatabase)) And (Information.IsReference(vDatabase)) Then
                ' Yes, so use it.
                m_oDatabase = vDatabase

                ' Do NOT Close Database in Terminate() method
                m_bCloseDatabase = False
            Else
                ' NO, Create new instance of the database object
                m_oDatabase = New dPMDAO.Database()

                ' Open the Database
                ' RDC 27062002 use Comp Serv to open database
                m_lReturn = CType(gPMComponentServices.NewDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Close Database in Terminate() method
                m_bCloseDatabase = True

            End If

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Set the ProcessMode to Generic
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric

            ' Set the Type Of Business to New Business
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric

            ' Set the Effective Date to NOW
            m_dtEffectiveDate = DateTime.Now

            ' Initialise the Status settings
            m_sProcessStatus.Value = gPMConstants.PMNavStatusUnknown
            m_sMapStatus.Value = gPMConstants.PMNavStatusUnknown
            m_sStepStatus.Value = gPMConstants.PMNavStatusUnknown



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
                If Not (m_oSourceDatabase Is Nothing) Then
                    m_oSourceDatabase.CloseDatabase()
                    m_oSourceDatabase = Nothing
                End If
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
                End If
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
    ' Name: SetStatus (Standard Method)
    '
    ' Description: Set the Process, Map and Step status.
    ' Note:        A Property Get is provided for the Step Status only
    '              as this is the only one which this component can
    '              alter directly.
    ' ***************************************************************** '
    Public Function SetStatus(ByRef sProcessStatus As String, ByRef sMapStatus As String, ByRef sStepStatus As String) As Integer

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
    ' Name: UpdateFieldLookUp (Public)
    '
    ' Description: Updates a lookup value
    '
    ' ***************************************************************** '
    Public Function UpdateFieldLookUp(ByRef sTableName As String, ByRef lID As Integer, ByRef vValue As String) As Integer

        Dim result As Integer = 0
        Dim sSql As String = ""
        Dim vResultSet(,) As Object

        Try



            Select Case sTableName
                Case "PMCaption"

                    'Capture SQL on CopyOutput
                    If m_bBuildCopyScript Then

                        CopyLogSQL("Execute spu_pm_caption_id_return 1, '" & vValue & "'," & _
                                   " @caption_id OUTPUT ", NavProcConst.ACSQLStatement)

                        'indicate success
                        Return gPMConstants.PMEReturnCode.PMTrue
                    End If

                    sSql = "{call spu_pm_caption (1, '" & vValue & "')}"

                Case Else

            End Select

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSql:=sSql, sSQLName:="UpdateFieldLookUp ", bStoredProcedure:=True, vResultArray:=vResultSet)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Return ID
            If Information.IsArray(vResultSet) Then

                result = CInt(vResultSet(0, 0))
            End If

            Return result

        Catch excep As System.Exception


            ' Error Section.

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateFieldLookUp  Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateFieldLookUp ", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function GetNewID(ByRef sTableName As String, ByRef sFieldName As String, Optional ByRef vWhereClause As String = "") As Integer
        'Function to get the Next ID from the invoice table

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        Dim sSql As String = ""

        Try


            sSql = "SELECT IsNull(MAX(" & sFieldName & "),0) MaxID FROM " & sTableName

            'Get the WhereClause if it exists

            If Not Information.IsNothing(vWhereClause) Then
                If vWhereClause <> "" Then
                    sSql = sSql & " WHERE " & vWhereClause
                End If
            End If

            'Capture SQL on copy output file
            If m_bBuildCopyScript Then

                sSql = "SELECT @" & sFieldName & " = " & "IsNull(MAX(" & sFieldName & "),0) + 1 FROM " & sTableName
                CopyLogSQL(sSql, NavProcConst.ACSQLStatement)

                'Indicate success

                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSql:=sSql, sSQLName:="GETNEWID", bStoredProcedure:=False, lNumberRecords:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return 0
            End If

            lRecordCount = m_oDatabase.Records.Count()

            ' Do we have any records ?
            If lRecordCount < 1 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set oFields to refer to one Record
            ' To do list
            'Return m_oDatabase.Records.Item(1).Fields()("MaxID") + 1

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNewID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNewID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)


            Return result
        End Try
    End Function
    Public Function UpdateFields(ByRef sPMNavGroup As String, ByRef sUpdateClause As String, ByRef lID As Integer, ByRef lParentID As Integer, Optional ByRef vEnforceRules As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim sSql, sTableName, sWhereClause As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Set the SQL Criteria for the Group


            Select Case sPMNavGroup
                Case NavProcConst.NavGrpComponent
                    sTableName = "PMNav_Component"
                    sWhereClause = "pmnav_component_id = " & lID

                Case NavProcConst.NavGrpProcess
                    sTableName = "PMNav_Process"
                    sWhereClause = "pmnav_process_id = " & lID

                Case NavProcConst.NavGrpMap
                    sTableName = "PMNav_Map"
                    sWhereClause = "pmnav_map_id = " & lID

                Case NavProcConst.NavGrpStep
                    sTableName = "PMNav_Step"
                    sWhereClause = "pmnav_map_id = " & lParentID & _
                                   " AND pmnav_step_id = " & CStr(lID)

                Case Else

            End Select

            'Get the Fields
            sSql = "UPDATE " & sTableName & " SET " & _
                   sUpdateClause & _
                   " WHERE " & sWhereClause

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSql:=sSql, sSQLName:="UpdateFields", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Enforce rules
            If vEnforceRules Then
                m_lReturn = CType(EnforceRules(sPMNavGroup:=sPMNavGroup, lID:=lID, lParentID:=lParentID), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'If this is a Step then update the Map effectivedate
            If sPMNavGroup = NavProcConst.NavGrpStep Then
                UpdateMapEffectiveDate(lParentID)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateFields Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateFields", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    ' ***************************************************************** '
    ' Name: LoadSystemDSN (public)
    '
    ' Description: Get the System DSN List
    '
    ' ***************************************************************** '
    Public Function LoadSystemDSN() As Collection

        Dim result As Collection = Nothing
        Dim Res As Object
        Dim i As Integer
        Dim sSection As String

        Try

            'New collection of system DSNs
            result = New Collection()

            'Set the section to ODBC data sources
            sSection = "SOFTWARE\ODBC\odbc.ini\ODBC Data Sources"


            Res = ADVReg.ReadRegistryGetAll(gPMConstants.HKEY_LOCAL_MACHINE, sSection, i)


            Do Until (CStr(Res(2)) = "Not Found")

                'Add only SQL Server entries

                If CStr(Res(2)).StartsWith("SQL Server") Then
                    result.Add(Res(1))
                End If

                'Get the next registry entry
                i += 1

                Res = ADVReg.ReadRegistryGetAll(gPMConstants.HKEY_LOCAL_MACHINE, sSection, i)
            Loop

            Return result

        Catch


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: InsertFields (Public)
    '
    ' Description: Insert PM Navigator Fields
    '
    ' ***************************************************************** '
    Public Function InsertFields(ByRef sPMNavGroup As String, ByRef sFieldNames As String, ByRef sFieldValues As String, Optional ByRef lParentID As Integer = 0, Optional ByRef vEnforceRules As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim sSql, sTableName, sFieldID, sWhereClause As String
        Dim lID As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            'Set the SQL Criteria for the Group

            sWhereClause = ""

            Select Case sPMNavGroup
                Case NavProcConst.NavGrpComponent
                    sTableName = "PMNav_Component"
                    sFieldID = "pmnav_component_id"

                Case NavProcConst.NavGrpProcess
                    sTableName = "PMNav_Process"
                    sFieldID = "pmnav_process_id"

                Case NavProcConst.NavGrpMap
                    sTableName = "PMNav_Map"
                    sFieldID = "pmnav_map_id"

                Case NavProcConst.NavGrpStep
                    sTableName = "PMNav_Step"
                    sFieldID = "pmnav_step_id"
                    sWhereClause = "pmnav_map_id = " & lParentID

                Case Else

            End Select

            'If step, then check for insert step IDs
            If sPMNavGroup = NavProcConst.NavGrpStep Then

                'Make sure we have insert ids
                If m_lInsertStepMapID <> 0 And m_lInsertStepStepID <> 0 Then

                    'Make sure the Map for this insert,
                    'and the insert step map match
                    If lParentID = m_lInsertStepMapID Then

                        'Move the Steps into position
                        m_lReturn = CType(InsertStep(lMapID:=m_lInsertStepMapID, lStepID:=m_lInsertStepStepID), gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If


                        'Use the InsertStepID as the new Step
                        lID = m_lInsertStepStepID

                    Else

                        'Otherwise get new id
                        lID = CType(GetNewID(sTableName, sFieldID, sWhereClause), gPMConstants.PMEReturnCode)

                    End If

                    'Clear Insert IDs
                    m_lInsertStepMapID = 0
                    m_lInsertStepStepID = 0

                Else

                    'Get the new ID for the table
                    lID = CType(GetNewID(sTableName, sFieldID, sWhereClause), gPMConstants.PMEReturnCode)

                End If

            Else

                'Get the new ID for the table
                lID = CType(GetNewID(sTableName, sFieldID, sWhereClause), gPMConstants.PMEReturnCode)

            End If

            If lID = gPMConstants.PMEReturnCode.PMFalse Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'If we are copying to script then get SQl
            If m_bBuildCopyScript Then

                CopyLogSQL("INSERT INTO " & sTableName & Strings.Chr(13) & Strings.Chr(10) & _
                           NavProcConst.ACTAB & "(" & sFieldID & ", " & sFieldNames & ") " & Strings.Chr(13) & Strings.Chr(10) & _
                           NavProcConst.ACTAB & "VALUES( @" & sFieldID & ", " & sFieldValues & ")", NavProcConst.ACSQLStatement)

                'Indicate success

                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            'Insert the new record

            sSql = "INSERT INTO " & sTableName & _
                   "(" & sFieldID & ", " & sFieldNames & ") " & _
                   "VALUES(" & CStr(lID) & ", " & sFieldValues & ")"

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSql:=sSql, sSQLName:="InsertFields", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            'Return the new ID
            result = lID

            'Enforce Rules

            If Not Information.IsNothing(vEnforceRules) Then
                If vEnforceRules Then
                    m_lReturn = CType(EnforceRules(sPMNavGroup:=sPMNavGroup, lID:=lID, lParentID:=lParentID), gPMConstants.PMEReturnCode)
                End If
            End If

            'If this is a Step then update the Map effectivedate
            If sPMNavGroup = NavProcConst.NavGrpStep Then
                UpdateMapEffectiveDate(lParentID)
            End If

            Return result

        Catch excep As System.Exception


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InsertFields Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="InsertFields", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function






    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (BeginTrans) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function BeginTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLBeginTrans()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (CommitTrans) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CommitTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLCommitTrans()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (RollbackTrans) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function RollbackTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLRollbackTrans()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function
    ' PRIVATE Methods (End)


    ' ***************************************************************** '
    ' Name: VerifyTable (Public)
    '
    ' Description: Verify that a table exists
    '
    ' ***************************************************************** '
    Public Function VerifyTable(ByRef sTableName As String) As Integer

        Dim result As Integer = 0
        Dim sSql As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSql = "SELECT * FROM " & sTableName

            m_lReturn = m_oDatabase.SQLSelect(sSql:=sSql, sSQLName:="VerifyTable", bStoredProcedure:=False, lNumberRecords:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="VerifyTable Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="VerifyTable", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
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
        ' Error.
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class
