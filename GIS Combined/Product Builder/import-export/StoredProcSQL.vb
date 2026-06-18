Option Strict Off
Option Explicit On

Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.IO
Imports System.Text
Imports SharedFiles

Module StoredProcSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    Private Const ACClass As String = ""

    '****************
    ' MEvans : 25-07-2003 : 223 document production changes
    Private Const ACWPFFieldName As Integer = 0
    Private Const ACWPFColumnName As Integer = 1
    Private Const ACWPFColumnType As Integer = 2
    Private Const ACWPFDisplayName As Integer = 3
    '****************

    Private m_vSumInsured As Object

    '************************************************************************
    ' Name: GenerateCopyAssocClientsForPolicySP
    '
    ' Description: This function generates a stored procedure that will
    '              copy the records from one insurance file to another
    '              for all associated client tables in the system.
    '
    '              Note: it currently uses the system tables to get the fields
    '              required for each table. This will probably need changing
    '              to use the GIS object in the future to make it generic.
    '
    ' History: PW010903 - Created (CQ1912)
    '************************************************************************
    Public Function GenerateCopyAssocClientsForPolicySP(ByRef r_oDatabase As dPMDAO.Database) As gPMConstants.PMEReturnCode 

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse 
        Dim sProcedureName, sAssClientTable, sTableFields, sFieldName As String 
        Dim sSQL As New StringBuilder 
        Dim vArray, vFieldArray(,) As Object 

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sProcedureName = "spg_copy_associated_clients"

            'Drop it if it's already there
            DropExistingProcedure(sProcedureName, v_sPostFix:="", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set up header of SP
            sSQL = New StringBuilder("SET QUOTED_IDENTIFIER OFF" & Strings.Chr(13) & Strings.Chr(10) & _
                   "SET ANSI_NULLS ON" & Strings.Chr(13) & Strings.Chr(10))
            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL.ToString(), sSQLName:="Pre Create " & sProcedureName, bStoredProcedure:=False)

            sSQL = New StringBuilder("CREATE PROCEDURE " & sProcedureName & Strings.Chr(13) & Strings.Chr(10))
            sSQL.Append("                 @old_insurance_file_cnt int," & Strings.Chr(13) & Strings.Chr(10))
            sSQL.Append("                 @new_insurance_file_cnt int" & Strings.Chr(13) & Strings.Chr(10))
            sSQL.Append(Strings.Chr(13) & Strings.Chr(10))
            sSQL.Append("AS " & Strings.Chr(13) & Strings.Chr(10))

            ' Get an array of all the associated client objects
            r_oDatabase.Parameters.Clear()
            m_lReturn = r_oDatabase.SQLSelect(sSQL:=ACGetAssociatClientObjectsSQL, sSQLName:=ACGetAssociatClientObjectsName, bStoredProcedure:=ACGetAssociatClientObjectsStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If no objects, exit function
            If Not Information.IsArray(vArray) Then
                Return result
            End If

            ' Loop through all associated client objects

            For i As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)

                ' Store table name

                sAssClientTable = CStr(vArray(0, i))

                ' Get all user defined fields for the current associated client table
                sTableFields = ""
                r_oDatabase.Parameters.Clear()
                m_lReturn = r_oDatabase.Parameters.Add(sName:="table_name", vValue:=sAssClientTable, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = r_oDatabase.SQLSelect(sSQL:=ACGetAssociatClientFieldsSQL, sSQLName:=ACGetAssociatClientFieldsName, bStoredProcedure:=ACGetAssociatClientFieldsStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vFieldArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Build user fields
                If Information.IsArray(vFieldArray) Then

                    For iField As Integer = vFieldArray.GetLowerBound(1) To vFieldArray.GetUpperBound(1)

                        sFieldName = CStr(vFieldArray(0, iField)).ToLower()
                        If sFieldName <> "insurance_file_cnt" Then
                            sTableFields = sTableFields & "        " & _
                                           sFieldName & "," & Strings.Chr(13) & Strings.Chr(10)
                        End If
                    Next
                    sTableFields = sTableFields.Substring(0, sTableFields.Length - 3)

                    ' Add to stored procedure SQL
                    sSQL.Append(Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("INSERT INTO " & sAssClientTable & " (" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("        insurance_file_cnt," & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append(sTableFields)
                    sSQL.Append(")" & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("SELECT  @new_insurance_file_cnt," & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append(sTableFields & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("From " & sAssClientTable & Strings.Chr(13) & Strings.Chr(10))
                    sSQL.Append("WHERE insurance_file_cnt = @old_insurance_file_cnt" & Strings.Chr(13) & Strings.Chr(10))

                End If

                vFieldArray = Nothing

            Next

            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL.ToString(), sSQLName:="Create " & sProcedureName, bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GenerateCopyAssocClientsForPolicySP Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateCopyAssocClientsForPolicySP", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GenerateDMRelatedStoredProcedures
    '
    ' Description: Creates the "spg_<datamodelcode>_copy_sums_insured"
    '              stored procedure
    '
    ' History: 19/07/2002 CLG     - Created.
    '          27/09/2002 CMG-SJP - Moved from Business class of Maintain
    '                               Data model to common bas module so
    '                               that can shared with other projects
    '                               Eg Data Model Import Export Tool.
    '
    ' ***************************************************************** '
    Public Function GenerateDMRelatedStoredProcedures(ByVal v_sDatamodel As String, ByRef r_oDatabase As dPMDAO.Database) As Integer

        Dim result As Integer = 0
        Dim sProcedureName, sSQL As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sProcedureName = "spg_" & v_sDatamodel & "_copy_sums_insured"

            'Drop it if it's already there
            DropExistingProcedure(sProcedureName, v_sPostFix:="", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sSQL = "SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON" & Strings.Chr(13) & Strings.Chr(10)
            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Pre Create " & sProcedureName, bStoredProcedure:=False)

            sSQL = "CREATE PROCEDURE " & sProcedureName & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "                 @old_policy_link_id int," & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "                 @new_policy_link_id int" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "AS " & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "--Take advantage here of the identicalityness of gis_policy_link_id and rsa_policy_binder_id (or whatever)" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "INSERT INTO RSA_sum_insured (" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "        RSA_Policy_binder_id," & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "        sum_insured_type_id," & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "        sequence_id," & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "        description," & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "        reference," & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "        sum_insured," & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "        date_added," & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "        date_deleted," & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "        is_valuation_required," & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "        valuation_date," & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "        rate," & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "        premium" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & ")"
            sSQL = sSQL & "SELECT  @new_policy_link_id," & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "        sum_insured_type_id," & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "        sequence_id," & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "        description," & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "        reference," & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "        sum_insured," & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "        date_added," & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "        date_deleted," & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "        is_valuation_required," & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "        valuation_date," & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "        rate," & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "        premium" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "From RSA_sum_insured" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "WHERE   RSA_Policy_binder_id = @old_policy_link_id" & Strings.Chr(13) & Strings.Chr(10)

            sSQL = sSQL.Replace("RSA", v_sDatamodel)

            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & sProcedureName, bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GenerateDMRelatedStoredProcedures Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateDMRelatedStoredProcedures", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: DropExistingProcedure
    '
    ' Description:
    '
    ' History: 30/05/2002 CLG     - Created.
    '          27/09/2002 CMG-SJP - Moved from Business class of Maintain
    '                               Data model to common bas module so
    '                               that can shared with other projects
    '                               Eg Data Model Import Export Tool.
    '
    ' ***************************************************************** '
    Private Sub DropExistingProcedure(ByVal v_sProcedureName As String, ByVal v_sPostFix As String, ByRef r_lRetval As gPMConstants.PMEReturnCode, ByRef r_oDatabase As dPMDAO.Database)

        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".DropExistingProcedure")

        

            Dim sSQL As String = ""

            If r_lRetval <> gPMConstants.PMEReturnCode.PMTrue Then Exit Sub

            r_oDatabase.Parameters.Clear()

            sSQL = ACDropStoredProcedureSQL

            bPMAddParameter.AddParameter(r_oDatabase, sSQL, m_lReturn, "sName", v_sProcedureName & v_sPostFix, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:=ACDropStoredProcedureName, bStoredProcedure:=ACDropStoredProcedureStored)
            End If

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".DropExistingProcedure")


    End Sub

    ' ***************************************************************** '
    '
    ' Name: GenerateGetParentStoredProcedure
    '
    ' Description:
    '
    ' History: 25/01/2001 Tomo    - Created.
    '          27/09/2002 CMG-SJP - Moved from Business class of Maintain
    '                               Data model to common bas module so
    '                               that can shared with other projects
    '                               Eg Data Model Import Export Tool.
    '
    ' ***************************************************************** '
    Private Function GenerateGetParentStoredProcedure(ByRef sTableName As String, ByRef sPrefix As String, ByVal v_sDatamodel As String, ByRef sProcedureName As String, ByRef sProcedureNameOldStyle As String, ByRef vArray(,) As Object, ByRef r_oDatabase As dPMDAO.Database) As Integer

        Dim result As Integer = 0
        Dim sSelectList, sTableList, sWhereList1, sWhereList2, sSQL As String

        

            result = gPMConstants.PMEReturnCode.PMTrue

            'First we do the main query

            sSelectList = "SELECT DISTINCT " & sPrefix & "." & CStr(vArray(pbObjectAndPropertyConsts.ACPColumnName, 1)) & "," & Strings.Chr(13) & Strings.Chr(10)

            sTableList = "FROM " & sTableName & " " & sPrefix & ", " & Strings.Chr(13) & Strings.Chr(10) & _
                         v_sDatamodel & "_Policy_binder pb," & Strings.Chr(13) & Strings.Chr(10) & _
                         "GIS_policy_link gpl," & Strings.Chr(13) & Strings.Chr(10) & _
                         "insurance_file ifi" & Strings.Chr(13) & Strings.Chr(10)

            sWhereList1 = "WHERE ifi.insurance_file_cnt = @InsuranceFileCnt" & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND gpl.insurance_file_cnt = ifi.insurance_file_cnt" & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND gpl.Risk_id is null" & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND pb.gis_policy_link_id = gpl.gis_policy_link_id" & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND " & sPrefix & "." & v_sDatamodel & "_policy_binder_id = pb." & v_sDatamodel & "_Policy_binder_id" & Strings.Chr(13) & Strings.Chr(10)

            sWhereList2 = "WHERE ifi.insurance_file_cnt = @InsuranceFileCnt" & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND gpl.Risk_id = @RiskId" & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND pb.gis_policy_link_id = gpl.gis_policy_link_id" & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND " & sPrefix & "." & v_sDatamodel & "_policy_binder_id = pb." & v_sDatamodel & "_Policy_binder_id" & Strings.Chr(13) & Strings.Chr(10)

            'Drop it if it's already there
            DropExistingProcedure(v_sProcedureName:=sProcedureName, v_sPostFix:="_get_parent_key", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

            DropExistingProcedure(v_sProcedureName:=sProcedureNameOldStyle, v_sPostFix:="_get_parent_key", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'remove the last comma and vbcrlf
            sSelectList = sSelectList.Substring(0, sSelectList.Length - 3) & Strings.Chr(13) & Strings.Chr(10)

            sSelectList = "If @RiskId Is Null" & Strings.Chr(13) & Strings.Chr(10) & _
                          sSelectList & sTableList & sWhereList1 & _
                          "Else" & Strings.Chr(13) & Strings.Chr(10) & _
                          sSelectList & sTableList & sWhereList2

            'Create the new procedure
            sSQL = ""

            sSQL = sSQL & "CREATE PROCEDURE " & sProcedureName & "_get_parent_key @PartyCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@InsuranceFileCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@RiskId INT = NULL," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@ClaimCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@DocumentRef VARCHAR(25)," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@Instance1 INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@Instance2 INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@Instance3 INT" & Strings.Chr(13) & Strings.Chr(10) & _
                   "AS" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10)

            sSQL = sSQL & sSelectList & Strings.Chr(13) & Strings.Chr(10)

            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & sProcedureName & "_get_parent_key StoredProcedure", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set permissions

            sSQL = "GRANT EXECUTE ON dbo." & sProcedureName & "_get_parent_key TO PUBLIC" & Strings.Chr(13) & Strings.Chr(10)

            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Grant " & sProcedureName & "_get_parent_key StoredProcedure", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: GenerateGetKeysStoredProcedure
    '
    ' Description:
    '
    ' History: 25/01/2001 Tomo    - Created.
    '          27/09/2002 CMG-SJP - Moved from Business class of Maintain
    '                               Data model to common bas module so
    '                               that can shared with other projects
    '                               Eg Data Model Import Export Tool.
    '
    ' ***************************************************************** '
    Private Function GenerateGetKeysStoredProcedure(ByRef sTableName As String, ByRef sPrefix As String, ByVal v_sDatamodel As String, ByRef sProcedureName As String, ByRef sProcedureNameOldStyle As String, ByRef vArray(,) As Object, ByRef v_iIsParentMultipleInstance As Integer, ByRef r_oDatabase As dPMDAO.Database) As Integer

        Dim result As Integer = 0
        Dim sTableList As String = ""
        Dim sSelectList As New StringBuilder
        Dim sWhereList2 As New StringBuilder
        Dim sWhereList1 As New StringBuilder
        Dim lLevel As Integer
        Dim sSQL As String = ""

        

            result = gPMConstants.PMEReturnCode.PMTrue

            'First we do the main query
            sSelectList = New StringBuilder("SELECT ")

            sTableList = "FROM " & sTableName & " " & sPrefix & ", " & Strings.Chr(13) & Strings.Chr(10) & _
                         v_sDatamodel & "_Policy_binder pb," & Strings.Chr(13) & Strings.Chr(10) & _
                         "GIS_policy_link gpl," & Strings.Chr(13) & Strings.Chr(10) & _
                         "insurance_file ifi" & Strings.Chr(13) & Strings.Chr(10)

            sWhereList1 = New StringBuilder("WHERE ifi.insurance_file_cnt = @InsuranceFileCnt" & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND gpl.insurance_file_cnt = ifi.insurance_file_cnt" & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND gpl.Risk_id is null" & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND pb.gis_policy_link_id = gpl.gis_policy_link_id" & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND " & sPrefix & "." & v_sDatamodel & "_policy_binder_id = pb." & v_sDatamodel & "_Policy_binder_id" & Strings.Chr(13) & Strings.Chr(10))

            sWhereList2 = New StringBuilder("WHERE ifi.insurance_file_cnt = @InsuranceFileCnt" & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND gpl.Risk_id = @RiskId" & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND pb.gis_policy_link_id = gpl.gis_policy_link_id" & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND " & sPrefix & "." & v_sDatamodel & "_policy_binder_id = pb." & v_sDatamodel & "_Policy_binder_id" & Strings.Chr(13) & Strings.Chr(10))

            'What's the highest number key?
            lLevel = 0

            If Information.IsArray(vArray) Then
                For lTemp2 As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)

                    If (vArray(pbObjectAndPropertyConsts.ACPIsIdentifyingProperty, lTemp2)) = 1 Then
                        lLevel = lTemp2
                    End If
                Next lTemp2
            End If

            If v_iIsParentMultipleInstance > 0 Then
            End If

            If Information.IsArray(vArray) Then
                For lTemp2 As Integer = vArray.GetLowerBound(1) + 1 To vArray.GetUpperBound(1)

                    If (vArray(pbObjectAndPropertyConsts.ACPIsIdentifyingProperty, lTemp2)) = 1 Then

                        If lTemp2 < lLevel Then

                            sWhereList1.Append("AND " & sPrefix & "." & CStr(vArray(pbObjectAndPropertyConsts.ACPColumnName, lTemp2)) & " = @Instance" & CStr(lTemp2 + v_iIsParentMultipleInstance) & Strings.Chr(13) & Strings.Chr(10))

                            sWhereList2.Append("AND " & sPrefix & "." & CStr(vArray(pbObjectAndPropertyConsts.ACPColumnName, lTemp2)) & " = @Instance" & CStr(lTemp2 + v_iIsParentMultipleInstance) & Strings.Chr(13) & Strings.Chr(10))
                        Else

                            sSelectList.Append(sPrefix & "." & CStr(vArray(pbObjectAndPropertyConsts.ACPColumnName, lTemp2)) & "," & Strings.Chr(13) & Strings.Chr(10))
                        End If
                    End If

                Next lTemp2
            End If

            'Drop it if it's already there
            DropExistingProcedure(v_sProcedureName:=sProcedureName, v_sPostFix:="_get_keys", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

            DropExistingProcedure(v_sProcedureName:=sProcedureNameOldStyle, v_sPostFix:="_get_keys", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' RAG 2003-10-09
                ' Don't fail the whole thing here as the stored procs may not exist,
                ' so the delete fails delete them !
                ' E.g. Folgate import

                ' GenerateGetKeysStoredProcedure = PMFalse
                ' Exit Function
            End If

            'remove the last comma and vbcrlf
            sSelectList = New StringBuilder(sSelectList.ToString().Substring(0, sSelectList.ToString().Length - 3) & Strings.Chr(13) & Strings.Chr(10))

            sSelectList = New StringBuilder("If @RiskId Is Null" & Strings.Chr(13) & Strings.Chr(10) & _
                          sSelectList.ToString() & sTableList & sWhereList1.ToString() & _
                          "Else" & Strings.Chr(13) & Strings.Chr(10) & _
                          sSelectList.ToString() & sTableList & sWhereList2.ToString())

            'Create the new procedure
            sSQL = ""

            sSQL = sSQL & "CREATE PROCEDURE " & sProcedureName & "_get_keys @PartyCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@InsuranceFileCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@RiskId INT = NULL," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@ClaimCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@DocumentRef VARCHAR(25)"

            'dynamically generate instance variables
            generateInstanceVariables(sSQL, lLevel)
            sSQL = sSQL & Strings.Chr(13) & Strings.Chr(10) & "AS" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & sSelectList.ToString() & Strings.Chr(13) & Strings.Chr(10)

            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & sProcedureName & "_get_keys StoredProcedure", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set permissions

            sSQL = "GRANT EXECUTE ON dbo." & sProcedureName & "_get_keys TO PUBLIC" & Strings.Chr(13) & Strings.Chr(10)

            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Grant " & sProcedureName & "_get_keys StoredProcedure", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

    End Function

    '**********************************************************************
    ' Name: GenerateKeySPAssociatedClient
    '
    ' Description: Generate the get_keys and get_parent_key stored procs
    '              for associated clients / disclosures
    '
    ' History: PW250703 - PS229 - created
    '        : PW200803 - CQ1912 - associated clients now keyed by
    '                     insurance_file_cnt
    '**********************************************************************
    Private Function GenerateKeySPAssociatedClient(ByRef sTableName As String, ByRef sPrefix As String, ByVal v_sDatamodel As String, ByRef sProcedureName As String, ByRef sProcedureNameOldStyle As String, ByRef vArray As Object, ByRef v_iIsParentMultipleInstance As Integer, ByRef r_oDatabase As dPMDAO.Database) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Part 1 - get keys

            'Drop it if it's already there
            DropExistingProcedure(v_sProcedureName:=sProcedureName, v_sPostFix:="_get_keys", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

            DropExistingProcedure(v_sProcedureName:=sProcedureNameOldStyle, v_sPostFix:="_get_keys", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Create the new procedure
            sSQL = ""
            sSQL = sSQL & "CREATE PROCEDURE " & sProcedureName & "_get_keys" & Strings.Chr(13) & Strings.Chr(10) & _
                   "@PartyCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@InsuranceFileCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@RiskId INT = NULL," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@ClaimCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@DocumentRef VARCHAR(25)," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@Instance1 INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@Instance2 INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@Instance3 INT"

            sSQL = sSQL & Strings.Chr(13) & Strings.Chr(10) & "AS" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10)

            ' put the bit that does the work in
            sSQL = sSQL & "If @RiskId Is Null" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "SELECT pc.party_cnt" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "  FROM policy_client pc" & Strings.Chr(13) & Strings.Chr(10)
            ' PW200803 - CQ1912 - use file instead of folder
            sSQL = sSQL & " WHERE pc.insurance_file_cnt = @InsuranceFileCnt" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "   AND pc.risk_cnt is NULL" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "else" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "SELECT pc.party_cnt" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "  FROM policy_client pc" & Strings.Chr(13) & Strings.Chr(10)
            ' PW200803 - CQ1912 - use file instead of folder
            sSQL = sSQL & " WHERE pc.insurance_file_cnt = @InsuranceFileCnt" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "   AND (pc.risk_cnt = @RiskId OR pc.risk_cnt is NULL)" & Strings.Chr(13) & Strings.Chr(10)

            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & sProcedureName & "_get_keys StoredProcedure", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set permissions
            sSQL = "GRANT EXECUTE ON dbo." & sProcedureName & "_get_keys TO PUBLIC" & Strings.Chr(13) & Strings.Chr(10)

            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Grant " & sProcedureName & "_get_keys StoredProcedure", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Part 2 - parent key

            'Drop it if it's already there
            DropExistingProcedure(v_sProcedureName:=sProcedureName, v_sPostFix:="_get_parent_key", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

            DropExistingProcedure(v_sProcedureName:=sProcedureNameOldStyle, v_sPostFix:="_get_parent_key", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Create the new procedure
            sSQL = ""
            sSQL = sSQL & "CREATE PROCEDURE " & sProcedureName & "_get_parent_key" & Strings.Chr(13) & Strings.Chr(10) & _
                   "@PartyCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@InsuranceFileCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@RiskId INT = NULL," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@ClaimCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@DocumentRef VARCHAR(25)," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@Instance1 INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@Instance2 INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@Instance3 INT"

            sSQL = sSQL & Strings.Chr(13) & Strings.Chr(10) & "AS" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10)

            ' put the bit that does the work in
            sSQL = sSQL & "SELECT @RiskId" & Strings.Chr(13) & Strings.Chr(10)

            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & sProcedureName & "_get_parent_key StoredProcedure", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set permissions
            sSQL = "GRANT EXECUTE ON dbo." & sProcedureName & "_get_parent_key TO PUBLIC" & Strings.Chr(13) & Strings.Chr(10)

            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Grant " & sProcedureName & "_get_parent_key StoredProcedure", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

    End Function

    '***********************************************************************
    ' Name: GenerateKeySPDisclosure
    '
    ' Description: Generate the get_keys and get_parent_key stored procs
    '              for associated clients / disclosures
    '
    ' History: PW280703 - PS229 - created
    '***********************************************************************
    Private Function GenerateKeySPDisclosure(ByRef sTableName As String, ByRef sPrefix As String, ByVal v_sDatamodel As String, ByRef sProcedureName As String, ByRef sProcedureNameOldStyle As String, ByRef vArray As Object, ByRef v_iIsParentMultipleInstance As Integer, ByRef r_oDatabase As dPMDAO.Database) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Part 1 - get keys

            'Drop it if it's already there
            DropExistingProcedure(v_sProcedureName:=sProcedureName, v_sPostFix:="_get_keys", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

            DropExistingProcedure(v_sProcedureName:=sProcedureNameOldStyle, v_sPostFix:="_get_keys", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Create the new procedure
            sSQL = ""
            sSQL = sSQL & "CREATE PROCEDURE " & sProcedureName & "_get_keys" & Strings.Chr(13) & Strings.Chr(10) & _
                   "@PartyCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@InsuranceFileCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@RiskId INT = NULL," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@ClaimCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@DocumentRef VARCHAR(25)," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@Instance1 INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@Instance2 INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@Instance3 INT"

            sSQL = sSQL & Strings.Chr(13) & Strings.Chr(10) & "AS" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10)

            ' NB - these where clauses are based on those in the associated_client_sel
            ' xml stored procedure that selects associated clients and disclosures
            ' for the risk screen in cGISDataSetControl (BuildAssocClientSelSP).

            ' put the bit that does the work in
            sSQL = sSQL & "DECLARE @insurance_folder_cnt INT" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "    SELECT pcv.party_conviction_id" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "      FROM party_conviction pcv" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "INNER JOIN policy_client pc "
            sSQL = sSQL & "ON pcv.party_cnt = pc.party_cnt" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "     WHERE pc.insurance_file_cnt = @InsuranceFileCnt" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "       AND pc.is_insured = 1" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "       AND pcv.risk_cnt IS NULL" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "       AND pcv.party_cnt = @instance2" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "     UNION" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "    SELECT pcv.party_conviction_id" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "      FROM party_conviction pcv" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "INNER JOIN policy_client pc "
            sSQL = sSQL & "ON pcv.party_cnt = pc.party_cnt" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "INNER JOIN risk r "
            sSQL = sSQL & "ON pcv.risk_cnt = r.risk_cnt" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "INNER JOIN disclosure_type_risk_type dtrt "
            sSQL = sSQL & "ON r.risk_type_id = dtrt.risk_type_id" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "     WHERE pc.insurance_file_cnt = @InsuranceFileCnt" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "       AND pcv.risk_cnt = @RiskId" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "       AND pcv.disclosure_type_id = dtrt.disclosure_type_id" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "       AND pcv.party_cnt = @instance2" & Strings.Chr(13) & Strings.Chr(10)

            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & sProcedureName & "_get_keys StoredProcedure", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set permissions

            sSQL = "GRANT EXECUTE ON dbo." & sProcedureName & "_get_keys TO PUBLIC" & Strings.Chr(13) & Strings.Chr(10)

            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Grant " & sProcedureName & "_get_keys StoredProcedure", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Part 2 - parent key

            'Drop it if it's already there
            DropExistingProcedure(v_sProcedureName:=sProcedureName, v_sPostFix:="_get_parent_key", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

            DropExistingProcedure(v_sProcedureName:=sProcedureNameOldStyle, v_sPostFix:="_get_parent_key", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Create the new procedure
            sSQL = ""
            sSQL = sSQL & "CREATE PROCEDURE " & sProcedureName & "_get_parent_key" & Strings.Chr(13) & Strings.Chr(10) & _
                   "@PartyCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@InsuranceFileCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@RiskId INT = NULL," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@ClaimCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@DocumentRef VARCHAR(25)," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@Instance1 INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@Instance2 INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@Instance3 INT"

            sSQL = sSQL & Strings.Chr(13) & Strings.Chr(10) & "AS" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10)

            ' put the bit that does the work in
            sSQL = sSQL & "SELECT @Instance2" & Strings.Chr(13) & Strings.Chr(10)

            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & sProcedureName & "_get_parent_key StoredProcedure", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set permissions
            sSQL = "GRANT EXECUTE ON dbo." & sProcedureName & "_get_parent_key TO PUBLIC" & Strings.Chr(13) & Strings.Chr(10)

            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Grant " & sProcedureName & "_get_parent_key StoredProcedure", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: GenerateStoredProcedure
    '
    ' Description:
    '
    ' History: 31/03/2000 Tomo    - Created.
    '          16/10/2000 RWH     - Removed @Key and made @RiskId nullable (optional).
    '          27/09/2001 Tomo    - Rewrote to use left joins as we need to link
    '                               country to an address that may not be there.
    '          27/09/2002 CMG-SJP - Moved from Business class of Maintain
    '                               Data model to common bas module so
    '                               that can shared with other projects
    '                               Eg Data Model Import Export Tool.
    '          10-11-2003 MEvans  - Added Optional param; v_lGisDataModelType to allow
    '                               risk object added to a claim datamodel to have there
    '                               procedures created diffently than standard risk objects.
    '
    ' ***************************************************************** '
    Public Function GenerateStoredProcedure(ByRef r_vGISObject(,) As Object, ByRef r_vGISProperty() As Object, ByVal v_sDatamodel As String, ByRef r_oDatabase As dPMDAO.Database, ByVal v_lPMProductFamily As Integer, Optional ByVal v_lGisDataModelType As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim sProcedureName, sProcedureNameOldStyle, sWhereList3 As String
        Dim sFinalSelectList As New StringBuilder
        Dim sTableList As New StringBuilder
        Dim sSelectList As New StringBuilder
        Dim sParameterList As New StringBuilder
        Dim sWhereList2 As New StringBuilder
        Dim sWhereList1 As New StringBuilder
        Dim lTemp3 As Integer
        Dim sTemp, sSQL, sColumnName, sSubGroup, sSubGroup2, sSubGroup3, sLoop1, sLoop2, sLoop3 As String
        Dim sDisplayname As New StringBuilder
        Dim sFieldName As New StringBuilder
        Dim sSubGroupTemp As New StringBuilder
        Dim lFormat As gPMConstants.PMEFormatStyle
        Dim vArray(,) As Object
        Dim sPrefix As New StringBuilder
        Dim lCount As Integer
        Dim sSelect As String = ""
        Dim lOtherTableCount, lParentId, lLevel, lAddressCount As Integer
        Dim iParentIsMultipleInstance, iInstanceCount As Integer

        ' PW250703 - PS229
        Dim iObjectType As Integer
        Dim bFixedField As Boolean

        ' PW240703 - PS229 - Get data model type and set up field manager group
        Dim lGISDataModelType As Integer
        Dim vResArray(,) As Object
        Dim sMainGroup As String = ""

        r_oDatabase.Parameters.Clear()
        m_lReturn = r_oDatabase.SQLSelect(sSQL:="SELECT gis_data_model_type_id FROM " & _
                    "gis_data_model WHERE code='" & v_sDatamodel & "'", sSQLName:="GenerateStoredProcedure raw SQL", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResArray)
        If Information.IsArray(vResArray) Then

            lGISDataModelType = CInt(Microsoft.VisualBasic.Conversion.Val(CStr(vResArray(0, 0))))
        End If
        If lGISDataModelType = GISDataModelType.GISDMTypeParty Then
            sMainGroup = "Party"
        Else
            sMainGroup = "Risk"
        End If
        ' PW240703: end

        '**************
        ' MEvans : 28-07-2003 : 223 document production
        Dim lObject As Integer
        Dim sLiveTableName As String = ""
        Dim lParamCount As Integer
        Const ACClaimBasedWorkTablePrefix As String = "Work_"
        '**************

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sPrefix = New StringBuilder(v_sDatamodel)

            'Delete the records on wp_fields
            'Make sure any old style are deleted first
            sSQL = "DELETE FROM wp_fields" & Strings.Chr(13) & Strings.Chr(10) & _
                   "WHERE sql LIKE 'sp_wp_" & v_sDatamodel & "%'" & Strings.Chr(13) & Strings.Chr(10)

            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Delete " & sProcedureName & " WPFields", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Make sure any new style are deleted first
            sSQL = "DELETE FROM wp_fields" & Strings.Chr(13) & Strings.Chr(10) & _
                   "WHERE sql LIKE 'spg_wp_" & v_sDatamodel & "%'" & Strings.Chr(13) & Strings.Chr(10)

            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Delete " & sProcedureName & " WPFields", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            For lTemp As Integer = r_vGISObject.GetLowerBound(1) To r_vGISObject.GetUpperBound(1)

                iInstanceCount = 0

                If IsDBNull(r_vGISObject(pbObjectAndPropertyConsts.ACOParentObjectId, lTemp)) Or IsNothing(r_vGISObject(pbObjectAndPropertyConsts.ACOParentObjectId, lTemp)) Then

                    lParentId = CInt(r_vGISObject(pbObjectAndPropertyConsts.ACOGISObjectId, lTemp))
                End If

                sPrefix = New StringBuilder(v_sDatamodel)
                'For now don't do child objects...
                'Now we do...
                ' PW250703 - PS229 - store object type

                iObjectType = CInt(r_vGISObject(pbObjectAndPropertyConsts.ACOIsNonGIS, lTemp))

                Select Case iObjectType
                    Case GISDataModelType.GISOTClaim 'tbd

                        ' get object position within array
                        lObject = lTemp

                        ' ensure this is a child object

                        If (r_vGISObject(pbObjectAndPropertyConsts.ACOGISObjectId, lObject)) <> lParentId Then

                            '*****************************
                            ' determine the old and new format of the stored procedures
                            '*****************************

                            ' use live table

                            sProcedureName = CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOObjectName, lObject)).Substring(0).Replace(ACClaimBasedWorkTablePrefix, "").ToLower()

                            m_lReturn = CType(GetProcedureName(r_sProcedureName:=sProcedureName, v_sDatamodel:=v_sDatamodel), gPMConstants.PMEReturnCode)

                            sProcedureNameOldStyle = "sp_wp_" & v_sDatamodel & sProcedureName
                            sProcedureName = "spg_wp_" & v_sDatamodel & sProcedureName

                            iParentIsMultipleInstance = IsTopLevelParentObjectMultipleInstance(CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOObjectName, lObject)), r_vGISObject)

                            '******************
                            ' determine prefix (alias) for table name and columns
                            '******************
                            'sTemp = StripDataModelCode(v_vTheString:=r_vGISObject(ACOTableName, lObject), _
                            'v_sDataModel:=v_sDataModel)
                            'sTemp = Mid$(sTemp, InStr(sTemp, "_") + 1)

                            'sTemp = "Claim"
                            sPrefix = New StringBuilder(v_sDatamodel & "C")

                            '                While InStr(sTemp, "_") <> 0
                            '                    lUnderscorePos = InStr(sTemp, "_")
                            '                    sPrefix = sPrefix & UCase$(Left$(sTemp, 1))
                            '                    sTemp = Mid$(sTemp, lUnderscorePos + 1)
                            '                Wend

                            '                sPrefix = sPrefix & UCase$(Left$(sTemp, 1))

                            '*****************
                            ' Build SQL for Create Procedure
                            '*****************

                            ' use live claim table for document production

                            sLiveTableName = CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOTableName, lObject)).Substring(0).Replace(ACClaimBasedWorkTablePrefix, "")

                            ' intialise variables
                            lParamCount = 0
                            sSelectList = New StringBuilder("")
                            sTableList = New StringBuilder("FROM " & sLiveTableName & " " & sPrefix.ToString() & Strings.Chr(13) & Strings.Chr(10))

                            ' Get and use property array for the current object

                            vArray = r_vGISProperty(lObject)

                            '*****************
                            ' are there any object properties
                            '*****************

                            If ProcessPropertyArray(v_lObjectType:=GISDataModelType.GISOTClaim, r_vPropertyArray:=vArray, r_sSelectList:=sSelectList.ToString(), r_sTableList:=sTableList.ToString(), v_sMainGroup:="Claim", v_sProcedureName:=sProcedureName, v_lPMProductFamily:=v_lPMProductFamily, v_sDatamodel:=v_sDatamodel, r_oDatabase:=r_oDatabase, r_lParamCount:=lParamCount, v_sPrefix:=sPrefix.ToString(), v_sSubGroup:=v_sDatamodel & " Claim Details") = gPMConstants.PMEReturnCode.PMTrue Then

                                If lParamCount <> 0 Then

                                    sWhereList1 = New StringBuilder("WHERE " & sPrefix.ToString() & ".Claim_Id = @ClaimCnt")

                                    'Add the rest...
                                    sTableList = New StringBuilder(sTableList.ToString().Substring(0, sTableList.ToString().Length - 2)) '& ", " & vbCrLf & |                            v_sDataModel & "_Policy_binder pb," & vbCrLf & |                                    "GIS_policy_link gpl," & vbCrLf & |                                    "insurance_file ifi" & vbCrLf

                                    '***************
                                    ' Attempt to drop both version of the stored procedure
                                    ' both old and new
                                    '***************
                                    DropExistingProcedure(v_sProcedureName:=sProcedureName, v_sPostFix:="", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

                                    DropExistingProcedure(v_sProcedureName:=sProcedureNameOldStyle, v_sPostFix:="", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        Return gPMConstants.PMEReturnCode.PMFalse
                                    End If

                                    'remove the last comma and vbcrlf
                                    sSelectList = New StringBuilder(sSelectList.ToString().Substring(0, sSelectList.ToString().Length - 3) & Strings.Chr(13) & Strings.Chr(10))
                                    sSelectList = New StringBuilder(" SELECT " & sSelectList.ToString() & Strings.Chr(13) & Strings.Chr(10) & _
                                                  sTableList.ToString() & Strings.Chr(13) & Strings.Chr(10) & _
                                                  sWhereList1.ToString())

                                    'Create the new procedure
                                    sSQL = ""

                                    ' removed risk id as this is knackering the
                                    '"@RiskId INT = NULL," & vbCrLf &
                                    sSQL = sSQL & "CREATE PROCEDURE " & sProcedureName & Strings.Chr(13) & Strings.Chr(10) & _
                                           "@PartyCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                                           "@InsuranceFileCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                                           "@ClaimCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                                           "@DocumentRef VARCHAR(25)"

                                    'dynamically generate instance variables
                                    generateInstanceVariables(sSQL, iInstanceCount)
                                    sSQL = sSQL & Strings.Chr(13) & Strings.Chr(10) & "AS" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & sSelectList.ToString() & Strings.Chr(13) & Strings.Chr(10)

                                    m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & sProcedureName & " StoredProcedure", bStoredProcedure:=False)

                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        Return gPMConstants.PMEReturnCode.PMFalse
                                    End If

                                    'Set permissions
                                    sSQL = "GRANT EXECUTE ON dbo." & sProcedureName & " TO PUBLIC" & Strings.Chr(13) & Strings.Chr(10)

                                    m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Grant " & sProcedureName & " StoredProcedure", bStoredProcedure:=False)

                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        Return gPMConstants.PMEReturnCode.PMFalse
                                    End If

                                End If ' Are there any parameters

                            End If

                        End If

                    Case GISDataModelType.GISOTPeril 'tbd

                        '**************************
                        '*******WE ARE IN *********
                        '******** CLAIM ***********
                        '******** PERIL ***********
                        '**************************

                        ' get object position within array
                        lObject = lTemp

                        If (r_vGISObject(pbObjectAndPropertyConsts.ACOGISObjectId, lObject)) <> lParentId Then
                            '*****************************
                            ' determine the old and new format of the stored procedures
                            '*****************************

                            ' use live tables not work tables for document production

                            sProcedureName = CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOObjectName, lObject)).Substring(0).Replace(ACClaimBasedWorkTablePrefix, "").ToLower()

                            m_lReturn = CType(GetProcedureName(r_sProcedureName:=sProcedureName, v_sDatamodel:=v_sDatamodel), gPMConstants.PMEReturnCode)

                            sProcedureNameOldStyle = "sp_wp_" & v_sDatamodel & sProcedureName
                            sProcedureName = "spg_wp_" & v_sDatamodel & sProcedureName

                            iParentIsMultipleInstance = IsTopLevelParentObjectMultipleInstance(CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOObjectName, lObject)), r_vGISObject)

                            '******************
                            ' determine prefix (alias) for table name
                            '******************
                            'sTemp = StripDataModelCode(v_vTheString:=r_vGISObject(ACOTableName, lObject), _
                            'v_sDataModel:=v_sDataModel)
                            'sTemp = Mid$(sTemp, InStr(sTemp, "_") + 1)

                            sPrefix = New StringBuilder(v_sDatamodel & "CP")

                            'While InStr(sTemp, "_") <> 0
                            '    lUnderscorePos = InStr(sTemp, "_")
                            '    sPrefix = sPrefix & UCase$(Left$(sTemp, 1))
                            '    sTemp = Mid$(sTemp, lUnderscorePos + 1)
                            'Wend

                            'sPrefix = sPrefix & UCase$(Left$(sTemp, 1))

                            '*****************
                            ' Build SQL for Create Procedure
                            '*****************

                            sLiveTableName = CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOTableName, lObject)).Substring(0).Replace(ACClaimBasedWorkTablePrefix, "")

                            ' intialise variables
                            lParamCount = 0
                            sSelectList = New StringBuilder("")
                            sTableList = New StringBuilder("FROM " & sLiveTableName & " " & sPrefix.ToString() & Strings.Chr(13) & Strings.Chr(10))

                            ' Get and use property array for the current object

                            vArray = r_vGISProperty(lObject)

                            '************************
                            ' Generate Get Keys Stored Procedure
                            '************************
                            ' Use the Live claim peril table to get the keys from
                            m_lReturn = CType(GenerateClaimPerilGetKeysStoredProcedure(sTableName:=sLiveTableName, sProcedureName:=sProcedureName, sProcedureNameOldStyle:=sProcedureNameOldStyle, r_oDatabase:=r_oDatabase), gPMConstants.PMEReturnCode)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                            '************************
                            ' Generate Get Parent Keys Stored Procedure
                            '************************
                            m_lReturn = CType(GenerateClaimPerilGetParentStoredProcedure(sProcedureName:=sProcedureName, sProcedureNameOldStyle:=sProcedureNameOldStyle, r_oDatabase:=r_oDatabase), gPMConstants.PMEReturnCode)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                            '*****************
                            ' Build SQL for Create Procedure
                            '*****************

                            ' intialise variables
                            lParamCount = 0
                            sSelectList = New StringBuilder("")
                            sTableList = New StringBuilder("FROM " & sLiveTableName & " " & sPrefix.ToString() & Strings.Chr(13) & Strings.Chr(10))

                            ' Get and use property array for the current object

                            vArray = r_vGISProperty(lObject)

                            If ProcessPropertyArray(v_lObjectType:=GISDataModelType.GISOTPeril, r_vPropertyArray:=vArray, r_sSelectList:=sSelectList.ToString(), r_sTableList:=sTableList.ToString(), v_sMainGroup:="Claim", v_sProcedureName:=sProcedureName, v_lPMProductFamily:=v_lPMProductFamily, v_sDatamodel:=v_sDatamodel, r_oDatabase:=r_oDatabase, r_lParamCount:=lParamCount, v_sPrefix:=sPrefix.ToString(), v_sSubGroup:=v_sDatamodel & " Peril Items", v_sLoop1:=v_sDatamodel & "ClaimPeril") = gPMConstants.PMEReturnCode.PMTrue Then

                                If lParamCount <> 0 Then

                                    'Add the rest...
                                    sTableList = New StringBuilder(sTableList.ToString().Substring(0, sTableList.ToString().Length - 2)) '& ", " & vbCrLf & |                            v_sDataModel & "_Policy_binder pb," & vbCrLf & |                                    "GIS_policy_link gpl," & vbCrLf & |                                    "insurance_file ifi" & vbCrLf

                                    sWhereList1 = New StringBuilder("WHERE Claim_Peril_id = @Instance2")

                                    'Drop it if it's already there
                                    DropExistingProcedure(v_sProcedureName:=sProcedureName, v_sPostFix:="", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

                                    DropExistingProcedure(v_sProcedureName:=sProcedureNameOldStyle, v_sPostFix:="", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        Return gPMConstants.PMEReturnCode.PMFalse
                                    End If

                                    'remove the last comma and vbcrlf
                                    sSelectList = New StringBuilder(sSelectList.ToString().Substring(0, sSelectList.ToString().Length - 3) & Strings.Chr(13) & Strings.Chr(10))

                                    sSelectList = New StringBuilder("SELECT " & sSelectList.ToString() & " " & sTableList.ToString() & " " & sWhereList1.ToString())

                                    'Create the new procedure
                                    sSQL = ""

                                    sSQL = sSQL & "CREATE PROCEDURE " & sProcedureName & " @PartyCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                                           "@InsuranceFileCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                                           "@ClaimCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                                           "@DocumentRef VARCHAR(25)," & Strings.Chr(13) & Strings.Chr(10) & _
                                           "@Instance1  INT = NULL," & Strings.Chr(13) & Strings.Chr(10) & _
                                           "@Instance2  INT = NULL," & Strings.Chr(13) & Strings.Chr(10) & _
                                           "@Instance3  INT = NULL" & Strings.Chr(13) & Strings.Chr(10) & _
                                           "AS" & Strings.Chr(13) & Strings.Chr(10) & _
                                           ""

                                    sSQL = sSQL & Strings.Chr(13) & Strings.Chr(10) & sSelectList.ToString() & Strings.Chr(13) & Strings.Chr(10)

                                    m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & sProcedureName & " StoredProcedure", bStoredProcedure:=False)

                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        Return gPMConstants.PMEReturnCode.PMFalse
                                    End If

                                    'Set permissions
                                    sSQL = "GRANT EXECUTE ON dbo." & sProcedureName & " TO PUBLIC" & Strings.Chr(13) & Strings.Chr(10)

                                    m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Grant " & sProcedureName & " StoredProcedure", bStoredProcedure:=False)

                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        Return gPMConstants.PMEReturnCode.PMFalse
                                    End If

                                End If
                            End If
                        End If
                        '**************************
                        '*******WE ARE IN *********
                        '******** CLAIM ***********
                        '******** PERIL ***********
                        '**************************

                    Case GISDataModelType.GISOTNonGisSpecials
                        'We're something special

                        vArray = r_vGISProperty(lTemp)

                        For lTemp2 As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)
                            'It's a sum insured

                            If (vArray(pbObjectAndPropertyConsts.ACPSpecialsType, lTemp2)) = GISSharedPropertyConstants.ACOSumInsuredTypeID Then

                                m_lReturn = CType(GenerateSumInsuredFields(v_sDatamodel:=v_sDatamodel, lSumInsuredTypeId:=CInt(vArray(pbObjectAndPropertyConsts.ACPSpecialsTypeReference, lTemp2)), r_oDatabase:=r_oDatabase, v_lPMProductFamily:=v_lPMProductFamily), gPMConstants.PMEReturnCode)

                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    Return gPMConstants.PMEReturnCode.PMFalse
                                End If
                            End If

                            If (vArray(pbObjectAndPropertyConsts.ACPSpecialsType, lTemp2)) = GISSharedPropertyConstants.ACOStdWordingType Then
                                'It's a standard wording (endorsement / clause)
                                m_lReturn = CType(GenerateStandardWordingFields(), gPMConstants.PMEReturnCode)

                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    Return gPMConstants.PMEReturnCode.PMFalse
                                End If
                            End If
                        Next lTemp2

                        ' PW250703 - PS229 - do associated clients/disclosures
                    Case GISDataModelType.GISOTRisk, GISDataModelType.GISOTAssociatedClient, GISDataModelType.GISOTDisclosure

                        '*************
                        ' MEvans : 10-11-2003 : CQ3049 / CQ3143
                        '**********************************************************************
                        '**** CLAIM RISK OBJECT ***********************************************
                        '**********************************************************************
                        If v_lGisDataModelType = GISDataModelType.GISDMTypeClaim Then

                            '**********************************************************************
                            '*** This is a claim risk object so do things a little differently ****
                            '**********************************************************************

                            ' dont create wp fields objects for the ad-hoc document request tables

                            If (CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOTableName, lTemp)).IndexOf("claim_output", StringComparison.CurrentCultureIgnoreCase) + 1) = 0 And (CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOTableName, lTemp)).IndexOf("document_request", StringComparison.CurrentCultureIgnoreCase) + 1) = 0 Then

                                ' get object position within array
                                lObject = lTemp

                                If GenerateClaimRiskSP(v_lObjectId:=lObject, v_vGISObject:=r_vGISObject, v_vGISProperty:=r_vGISProperty, v_lParentId:=lParentId, v_sDatamodel:=v_sDatamodel, v_lPMProductFamily:=v_lPMProductFamily, r_oDatabase:=r_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then

                                    Return gPMConstants.PMEReturnCode.PMFalse

                                End If

                            End If
                            '**********************************************************************
                            '**** END CLAIM RISK OBJECT ***********************************************
                            '**********************************************************************
                            '*************
                        Else

                            '******************
                            ' MEvans : 11-08-2003 : 223 Document Production
                            ' Dont create stored procs or wp fields entries for the
                            ' claim output or the document request objects

                            If (CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOTableName, lTemp)).IndexOf("claim_output", StringComparison.CurrentCultureIgnoreCase) + 1) = 0 And (CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOTableName, lTemp)).IndexOf("document_request", StringComparison.CurrentCultureIgnoreCase) + 1) = 0 Then
                                '******************

                                If (r_vGISObject(pbObjectAndPropertyConsts.ACOGISObjectId, lTemp)) <> lParentId Then

                                    sProcedureName = CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOObjectName, lTemp)).ToLower()

                                    m_lReturn = CType(GetProcedureName(r_sProcedureName:=sProcedureName, v_sDatamodel:=v_sDatamodel), gPMConstants.PMEReturnCode)

                                    sProcedureNameOldStyle = "sp_wp_" & v_sDatamodel & sProcedureName
                                    sProcedureName = "spg_wp_" & v_sDatamodel & sProcedureName

                                    sTemp = StripDataModelCode(v_vTheString:=CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOTableName, lTemp)), v_sDatamodel:=v_sDatamodel)
                                    sTemp = sTemp.Substring(sTemp.IndexOf("_"c) + 1)

                                    iParentIsMultipleInstance = IsTopLevelParentObjectMultipleInstance(CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOObjectName, lTemp)), r_vGISObject)

                                    sPrefix = New StringBuilder(v_sDatamodel)
                                    While sTemp.IndexOf("_"c) >= 0
                                        lTemp3 = (sTemp.IndexOf("_"c) + 1)
                                        sPrefix.Append(sTemp.Substring(0, 1).ToUpper())
                                        sTemp = sTemp.Substring(lTemp3)
                                    End While

                                    sPrefix.Append(sTemp.Substring(0, 1).ToUpper())

                                    'First we do the main query
                                    sParameterList = New StringBuilder("")
                                    sSelectList = New StringBuilder("")
                                    sFinalSelectList = New StringBuilder("SELECT ")

                                    vArray = r_vGISProperty(lTemp)

                                    lCount = 0
                                    lOtherTableCount = 0

                                    ' PW250703 - PS229 - select/table list depends on object type
                                    Select Case iObjectType
                                        Case GISDataModelType.GISOTRisk

                                            sTableList = New StringBuilder("FROM " & CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOTableName, lTemp)) & " " & sPrefix.ToString() & _
                                                         Strings.Chr(13) & Strings.Chr(10))
                                        Case GISDataModelType.GISOTAssociatedClient
                                            sSelectList = New StringBuilder("SELECT pc.is_insured," & Strings.Chr(13) & Strings.Chr(10) & _
                                                          "       ppc.party_title_code," & Strings.Chr(13) & Strings.Chr(10) & _
                                                          "       ppc.forename," & Strings.Chr(13) & Strings.Chr(10) & _
                                                          "       p.name," & Strings.Chr(13) & Strings.Chr(10) & _
                                                          "       p.resolved_name," & Strings.Chr(13) & Strings.Chr(10) & _
                                                          "       ppc.initials," & Strings.Chr(13) & Strings.Chr(10) & _
                                                          "       pl.date_of_birth," & Strings.Chr(13) & Strings.Chr(10) & _
                                                          "       pl.gender_code," & Strings.Chr(13) & Strings.Chr(10) & _
                                                          "       party_type_code = pt.code," & Strings.Chr(13) & Strings.Chr(10) & _
                                                          "       party_type_description = pt.description," & Strings.Chr(13) & Strings.Chr(10))

                                            ' PW200803 - CQ1912 - change from file to folder
                                            sTableList = New StringBuilder("FROM policy_client pc" & Strings.Chr(13) & Strings.Chr(10) & _
                                                         "LEFT JOIN " & v_sDatamodel & "_associated_client " & sPrefix.ToString() & " " & _
                                                         "ON pc.party_cnt = " & sPrefix.ToString() & ".party_cnt " & _
                                                         "AND pc.insurance_file_cnt = " & sPrefix.ToString() & ".insurance_file_cnt" & Strings.Chr(13) & Strings.Chr(10))
                                        Case GISDataModelType.GISOTDisclosure
                                            ' PW200803 - CQ1912 - add new properties
                                            sSelectList = New StringBuilder("SELECT pcv.code," & Strings.Chr(13) & Strings.Chr(10) & _
                                                          "       pcv.conviction_date," & Strings.Chr(13) & Strings.Chr(10) & _
                                                          "       pcv.description," & Strings.Chr(13) & Strings.Chr(10) & _
                                                          "       pcv.fine_amt," & Strings.Chr(13) & Strings.Chr(10) & _
                                                          "       pcv.sentence_code," & Strings.Chr(13) & Strings.Chr(10) & _
                                                          "       pcv.sentence_description," & Strings.Chr(13) & Strings.Chr(10) & _
                                                          "       pcv.sentence_duration," & Strings.Chr(13) & Strings.Chr(10) & _
                                                          "       pcv.sentence_duration_qualifier," & Strings.Chr(13) & Strings.Chr(10) & _
                                                          "       pcv.sentence_effective_date," & Strings.Chr(13) & Strings.Chr(10) & _
                                                          "       pcv.status_code," & Strings.Chr(13) & Strings.Chr(10) & _
                                                          "       pcv.alcohol_level," & Strings.Chr(13) & Strings.Chr(10) & _
                                                          "       pcv.alcohol_measurement_method," & Strings.Chr(13) & Strings.Chr(10) & _
                                                          "       pcv.driving_licence_penalty_pts," & Strings.Chr(13) & Strings.Chr(10) & _
                                                          "       disclosure_type_id = dt.description," & Strings.Chr(13) & Strings.Chr(10) & _
                                                          "       pcv.effective_date," & Strings.Chr(13) & Strings.Chr(10) & _
                                                          "       pcv.expiry_date," & Strings.Chr(13) & Strings.Chr(10) & _
                                                          "       pcv.replaced_by_id," & Strings.Chr(13) & Strings.Chr(10))

                                            sTableList = New StringBuilder("FROM party_conviction pcv" & Strings.Chr(13) & Strings.Chr(10) & _
                                                         "LEFT JOIN " & v_sDatamodel & "_disclosure " & sPrefix.ToString() & " " & _
                                                         "ON pcv.party_cnt = " & sPrefix.ToString() & ".party_cnt " & _
                                                         "AND pcv.party_conviction_id = " & sPrefix.ToString() & ".party_conviction_id" & Strings.Chr(13) & Strings.Chr(10))
                                    End Select

                                    ' PW240703 - PS229 - different where clauses required for
                                    ' data model type of Party, because the risk object isn't
                                    ' really a risk object. But that's how it's been done.
                                    If lGISDataModelType = GISDataModelType.GISDMTypeParty Then

                                        sWhereList1 = New StringBuilder("WHERE p.party_cnt = @PartyCnt" & Strings.Chr(13) & Strings.Chr(10) & _
                                                      "AND gpl.party_cnt = p.party_cnt" & Strings.Chr(13) & Strings.Chr(10) & _
                                                      "AND gpl.Risk_id is null" & Strings.Chr(13) & Strings.Chr(10) & _
                                                      "AND pb.gis_policy_link_id = gpl.gis_policy_link_id" & Strings.Chr(13) & Strings.Chr(10) & _
                                                      "AND " & sPrefix.ToString() & "." & v_sDatamodel & "_policy_binder_id = pb." & _
                                                      v_sDatamodel & "_Policy_binder_id" & Strings.Chr(13) & Strings.Chr(10))

                                        sWhereList2 = New StringBuilder(sWhereList1.ToString())
                                    Else

                                        ' PW250703 - PS229 - where list depends on object type
                                        Select Case iObjectType
                                            Case GISDataModelType.GISOTRisk

                                                sWhereList1 = New StringBuilder("WHERE ifi.insurance_file_cnt = @InsuranceFileCnt" & Strings.Chr(13) & Strings.Chr(10) & _
                                                              "AND gpl.insurance_file_cnt = ifi.insurance_file_cnt" & Strings.Chr(13) & Strings.Chr(10) & _
                                                              "AND gpl.Risk_id is null" & Strings.Chr(13) & Strings.Chr(10) & _
                                                              "AND pb.gis_policy_link_id = gpl.gis_policy_link_id" & Strings.Chr(13) & Strings.Chr(10) & _
                                                              "AND " & sPrefix.ToString() & "." & v_sDatamodel & "_policy_binder_id = pb." & _
                                                              v_sDatamodel & "_Policy_binder_id" & Strings.Chr(13) & Strings.Chr(10))

                                                sWhereList2 = New StringBuilder("WHERE ifi.insurance_file_cnt = @InsuranceFileCnt" & Strings.Chr(13) & Strings.Chr(10) & _
                                                              "AND gpl.Risk_id = @RiskId" & Strings.Chr(13) & Strings.Chr(10) & _
                                                              "AND pb.gis_policy_link_id = gpl.gis_policy_link_id" & Strings.Chr(13) & Strings.Chr(10) & _
                                                              "AND " & sPrefix.ToString() & "." & v_sDatamodel & "_policy_binder_id = pb." & _
                                                              v_sDatamodel & "_Policy_binder_id" & Strings.Chr(13) & Strings.Chr(10))

                                            Case GISDataModelType.GISOTAssociatedClient
                                                ' PW200803 - CQ1912 - change from file to folder
                                                sWhereList1 = New StringBuilder("WHERE pc.insurance_file_cnt = @InsuranceFileCnt" & Strings.Chr(13) & Strings.Chr(10) & _
                                                              "AND pc.risk_cnt is NULL" & Strings.Chr(13) & Strings.Chr(10) & _
                                                              "AND pc.party_cnt = @instance2" & Strings.Chr(13) & Strings.Chr(10))

                                                ' PW200803 - CQ1912 - change from file to folder
                                                sWhereList2 = New StringBuilder("WHERE pc.insurance_file_cnt = @InsuranceFileCnt" & Strings.Chr(13) & Strings.Chr(10) & _
                                                              "AND (pc.risk_cnt = @RiskId or pc.risk_cnt is NULL)" & Strings.Chr(13) & Strings.Chr(10) & _
                                                              "AND pc.party_cnt = @instance2" & Strings.Chr(13) & Strings.Chr(10))

                                            Case GISDataModelType.GISOTDisclosure
                                                sWhereList1 = New StringBuilder("WHERE pcv.party_cnt = @instance2" & Strings.Chr(13) & Strings.Chr(10) & _
                                                              "AND pcv.party_conviction_id = @instance3" & Strings.Chr(13) & Strings.Chr(10))

                                                sWhereList2 = New StringBuilder(sWhereList1.ToString())

                                        End Select
                                    End If

                                    sWhereList3 = ""

                                    'We need the rest of the key information...
                                    lLevel = 0
                                    If Information.IsArray(vArray) Then

                                        For lTemp2 As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)

                                            If (vArray(pbObjectAndPropertyConsts.ACPIsIdentifyingProperty, lTemp2)) = 1 Then
                                                lLevel = lTemp2
                                            End If
                                        Next lTemp2
                                    End If

                                    ' PW250703 - PS229 - generate extra stored procs for
                                    ' associated clients / disclosures
                                    Select Case iObjectType
                                        Case GISDataModelType.GISOTAssociatedClient

                                            m_lReturn = CType(GenerateKeySPAssociatedClient(sTableName:=CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOTableName, lTemp)), sPrefix:=sPrefix.ToString(), v_sDatamodel:=v_sDatamodel, sProcedureName:=sProcedureName, sProcedureNameOldStyle:=sProcedureNameOldStyle, vArray:=vArray, v_iIsParentMultipleInstance:=iParentIsMultipleInstance, r_oDatabase:=r_oDatabase), gPMConstants.PMEReturnCode)

                                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                Return gPMConstants.PMEReturnCode.PMFalse
                                            End If

                                        Case GISDataModelType.GISOTDisclosure

                                            m_lReturn = CType(GenerateKeySPDisclosure(sTableName:=CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOTableName, lTemp)), sPrefix:=sPrefix.ToString(), v_sDatamodel:=v_sDatamodel, sProcedureName:=sProcedureName, sProcedureNameOldStyle:=sProcedureNameOldStyle, vArray:=vArray, v_iIsParentMultipleInstance:=iParentIsMultipleInstance, r_oDatabase:=r_oDatabase), gPMConstants.PMEReturnCode)

                                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                Return gPMConstants.PMEReturnCode.PMFalse
                                            End If

                                        Case GISDataModelType.GISOTRisk

                                            'We need to create the Getkeys stored procedure if we are deeper that 1 level
                                            'OR we have more than one instance of an object

                                            If lLevel > 1 Or (r_vGISObject(pbObjectAndPropertyConsts.ACOMaxInstances, lTemp)) > 1 Then

                                                m_lReturn = CType(GenerateGetKeysStoredProcedure(sTableName:=CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOTableName, lTemp)), sPrefix:=sPrefix.ToString(), v_sDatamodel:=v_sDatamodel, sProcedureName:=sProcedureName, sProcedureNameOldStyle:=sProcedureNameOldStyle, vArray:=vArray, v_iIsParentMultipleInstance:=iParentIsMultipleInstance, r_oDatabase:=r_oDatabase), gPMConstants.PMEReturnCode)

                                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                    Return gPMConstants.PMEReturnCode.PMFalse
                                                End If
                                            End If

                                            '01082002 CMG/PB Corrected error in this If logic level 2 parent
                                            'stored procedures were not being created and this was breaking
                                            'risk looping

                                            If lLevel = 2 Or ((r_vGISObject(pbObjectAndPropertyConsts.ACOMaxInstances, lTemp)) > 1) Then

                                                m_lReturn = CType(GenerateGetParentStoredProcedure(sTableName:=CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOTableName, lTemp)), sPrefix:=sPrefix.ToString(), v_sDatamodel:=v_sDatamodel, sProcedureName:=sProcedureName, sProcedureNameOldStyle:=sProcedureNameOldStyle, vArray:=vArray, r_oDatabase:=r_oDatabase), gPMConstants.PMEReturnCode)

                                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                    Return gPMConstants.PMEReturnCode.PMFalse
                                                End If
                                            End If

                                    End Select

                                    'We want to ignore all if we're top level (1)
                                    'but include all (but the first) if we're lower level
                                    'so...

                                    If lLevel > 1 Then
                                        lLevel = 0
                                    End If

                                    sSubGroup = ""
                                    sSubGroup2 = ""
                                    sSubGroup3 = ""
                                    sLoop1 = ""
                                    sLoop2 = ""
                                    sLoop3 = ""
                                    lAddressCount = 0
                                    If Information.IsArray(vArray) Then

                                        ' PW250703 - PS229 - set up groups/loops for associated client / disclosure
                                        ' object types
                                        Select Case iObjectType
                                            Case GISDataModelType.GISOTAssociatedClient
                                                sSubGroup = v_sDatamodel & "AssociatedClient"
                                                sLoop1 = sSubGroup
                                            Case GISDataModelType.GISOTDisclosure
                                                sSubGroup = v_sDatamodel & "AssociatedClient"
                                                sLoop1 = sSubGroup
                                                sSubGroup2 = v_sDatamodel & "Disclosure"
                                                sLoop2 = sSubGroup2
                                        End Select

                                        For lTemp2 As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)

                                            ' PW280703 - PS229 - set flag to indicate if this
                                            ' field is from a fixed table

                                            bFixedField = Not ((CBool(vArray(pbObjectAndPropertyConsts.ACPEditFlags, lTemp2)) And GISSharedPropertyConstants.GISDSEditNoDBColumn) = 0)

                                            If (vArray(pbObjectAndPropertyConsts.ACPIsIdentifyingProperty, lTemp2)) = 1 Then
                                                'Move the determination of the group structure to here - only
                                                'do it once
                                                sSubGroupTemp = New StringBuilder(v_sDatamodel)

                                                sTemp = StripDataModelCode(v_vTheString:=CStr(vArray(pbObjectAndPropertyConsts.ACPColumnName, lTemp2)), v_sDatamodel:=v_sDatamodel).Trim()

                                                'Remove the trailing _id
                                                sTemp = sTemp.Substring(0, sTemp.Length - 3)

                                                While sTemp.IndexOf("_"c) >= 0
                                                    lTemp3 = (sTemp.IndexOf("_"c) + 1)
                                                    sSubGroupTemp.Append(Strings.StrConv(sTemp.Substring(0, lTemp3 - 1), VbStrConv.ProperCase))
                                                    sTemp = sTemp.Substring(lTemp3)
                                                End While

                                                Select Case lTemp2
                                                    Case 0
                                                    Case 1
                                                        sSubGroup = sSubGroupTemp.ToString() & Strings.StrConv(sTemp, VbStrConv.ProperCase)
                                                        'find if parent was actually a  looping object
                                                        If lLevel = 0 Then
                                                            If iParentIsMultipleInstance = 1 And lLevel = 0 Then
                                                                sLoop1 = sSubGroup
                                                            End If
                                                        End If
                                                    Case 2
                                                        sSubGroup2 = sSubGroupTemp.ToString() & Strings.StrConv(sTemp, VbStrConv.ProperCase)
                                                        If lLevel = 0 Then
                                                            If iParentIsMultipleInstance = 1 Then
                                                                sLoop2 = sSubGroup2
                                                            Else
                                                                sLoop1 = sSubGroup2
                                                            End If
                                                        End If

                                                    Case 3
                                                        sSubGroup3 = sSubGroupTemp.ToString() & Strings.StrConv(sTemp, VbStrConv.ProperCase)
                                                        'ED 30092002 - sLoop3 set t sSubGroup3 if parent is multi instance
                                                        'orignally overwriting sLoop2 with the new sub value.
                                                        If lLevel = 0 Then
                                                            If iParentIsMultipleInstance = 1 Then
                                                                sLoop3 = sSubGroup3
                                                            Else
                                                                sLoop2 = sSubGroup3
                                                            End If
                                                        End If

                                                End Select

                                                If lTemp2 > lLevel Then

                                                    sWhereList1.Append("AND " & sPrefix.ToString() & "." & CStr(vArray(pbObjectAndPropertyConsts.ACPColumnName, lTemp2)) & " = @Instance" & CStr(lTemp2 + iParentIsMultipleInstance) & Strings.Chr(13) & Strings.Chr(10))

                                                    sWhereList2.Append("AND " & sPrefix.ToString() & "." & CStr(vArray(pbObjectAndPropertyConsts.ACPColumnName, lTemp2)) & " = @Instance" & CStr(lTemp2 + iParentIsMultipleInstance) & Strings.Chr(13) & Strings.Chr(10))
                                                    iInstanceCount = lTemp2 + iParentIsMultipleInstance
                                                End If
                                            Else

                                                If CStr(vArray(pbObjectAndPropertyConsts.ACPPropertyName, lTemp2)) <> "dElEtEd" Then

                                                    'if processing first no key attribute
                                                    If lCount = 0 Then
                                                        'if not subloop and object is multiple instance

                                                        If sLoop1 = "" And (r_vGISObject(pbObjectAndPropertyConsts.ACOMaxInstances, lTemp)) > 1 Then
                                                            'add where clause for top level multiple instance

                                                            sWhereList1.Append("AND " & sPrefix.ToString() & "." & CStr(vArray(pbObjectAndPropertyConsts.ACPColumnName, 1)) & _
                                                                               " = @Instance2" & Strings.Chr(13) & Strings.Chr(10))

                                                            sWhereList2.Append("AND " & sPrefix.ToString() & "." & CStr(vArray(pbObjectAndPropertyConsts.ACPColumnName, 1)) & _
                                                                               " = @Instance2" & Strings.Chr(13) & Strings.Chr(10))
                                                            sLoop1 = sSubGroup
                                                        End If
                                                    End If
                                                    lCount += 1

                                                    ' PW250703 - PS229
                                                    If (lCount = 1) And iObjectType = GISDataModelType.GISOTRisk Then
                                                        sSelect = "SELECT "
                                                    Else
                                                        sSelect = ""
                                                    End If

                                                    sTemp = CStr(vArray(pbObjectAndPropertyConsts.ACPColumnName, lTemp2)).Trim()

                                                    sFieldName = New StringBuilder("")
                                                    sDisplayname = New StringBuilder("")
                                                    sColumnName = sTemp

                                                    While sTemp.IndexOf("_"c) >= 0
                                                        lTemp3 = (sTemp.IndexOf("_"c) + 1)
                                                        sFieldName.Append(sTemp.Substring(0, lTemp3 - 1))
                                                        sDisplayname.Append(sTemp.Substring(0, lTemp3 - 1) & " ")
                                                        sTemp = sTemp.Substring(lTemp3)
                                                    End While

                                                    sFieldName.Append(sTemp)
                                                    sDisplayname.Append(sTemp)

                                                    'Special stuff to do here.

                                                    ' PW280703 - PS229 - If it's a field from a
                                                    ' fixed table, treat it as bog-standard

                                                    'If it's an address, need 6 lines of address and links to the address and country tables

                                                    If (CStr(vArray(pbObjectAndPropertyConsts.ACPColumnName, lTemp2)).ToUpper().StartsWith("ADDRESS_CNT")) And Not bFixedField Then
                                                        lAddressCount += 1

                                                        sParameterList.Append("@address" & lAddressCount & "_line_1 " & _
                                                                              "VARCHAR(255)," & Strings.Chr(13) & Strings.Chr(10))
                                                        sParameterList.Append("@address" & lAddressCount & "_line_2 " & _
                                                                              "VARCHAR(255)," & Strings.Chr(13) & Strings.Chr(10))
                                                        sParameterList.Append("@address" & lAddressCount & "_line_3 " & _
                                                                              "VARCHAR(255)," & Strings.Chr(13) & Strings.Chr(10))
                                                        sParameterList.Append("@address" & lAddressCount & "_line_4 " & _
                                                                              "VARCHAR(255)," & Strings.Chr(13) & Strings.Chr(10))
                                                        sParameterList.Append("@address" & lAddressCount & "_postal_code " & _
                                                                              "VARCHAR(255)," & Strings.Chr(13) & Strings.Chr(10))
                                                        sParameterList.Append("@address" & lAddressCount & "_country " & _
                                                                              "VARCHAR(255)," & Strings.Chr(13) & Strings.Chr(10))

                                                        sSelect = ""
                                                        If (sSelectList.ToString().IndexOf("SELECT") + 1) = 0 Then
                                                            sSelectList = New StringBuilder("SELECT " & sSelectList.ToString())
                                                        End If

                                                        sSelectList.Append( _
                                                                           "address" & CStr(lAddressCount) & "_line_1 = a" & CStr(lAddressCount) & ".address1," & _
                                                                           Strings.Chr(13) & Strings.Chr(10) & _
                                                                           "address" & CStr(lAddressCount) & "_line_2 = a" & CStr(lAddressCount) & ".address2," & _
                                                                           Strings.Chr(13) & Strings.Chr(10) & _
                                                                           "address" & CStr(lAddressCount) & "_line_3 = a" & CStr(lAddressCount) & ".address3," & _
                                                                           Strings.Chr(13) & Strings.Chr(10) & _
                                                                           "address" & CStr(lAddressCount) & "_line_4 = a" & CStr(lAddressCount) & ".address4," & _
                                                                           Strings.Chr(13) & Strings.Chr(10) & _
                                                                           "case a" & CStr(lAddressCount) & ".postal_code when convert(varchar(10), a" & _
                                                                           lAddressCount & ".address_id) then '' else a" & CStr(lAddressCount) & ".postal_code " & _
                                                                           "end as address" & CStr(lAddressCount) & "_postal_code," & Strings.Chr(13) & Strings.Chr(10) & _
                                                                           "address" & CStr(lAddressCount) & "_country = c" & CStr(lAddressCount) & ".description," & _
                                                                           Strings.Chr(13) & Strings.Chr(10))

                                                        sTableList.Append("LEFT JOIN address a" & lAddressCount & " ON " & _
                                                                          sPrefix.ToString() & "." & CStr(vArray(pbObjectAndPropertyConsts.ACPColumnName, lTemp2)) & " = a" & CStr(lAddressCount) & _
                                                                          ".address_cnt" & Strings.Chr(13) & Strings.Chr(10))
                                                        sTableList.Append("LEFT JOIN country c" & lAddressCount & " ON " & _
                                                                          "a" & CStr(lAddressCount) & ".country_id = c" & CStr(lAddressCount) & ".country_id" & Strings.Chr(13) & Strings.Chr(10))

                                                        For lTemp3 = 1 To 4
                                                            'Loop4 is not supported as yet.

                                                            m_lReturn = CType(AddToWPFields(sFieldName:=sPrefix.ToString() & "Address" & CStr(lAddressCount) & _
                                                                        "Line" & CStr(lTemp3), sSQL:=sProcedureName, sColumnName:="address" & lAddressCount & "_line_" & CStr(lTemp3), lColumnType:=gPMConstants.PMEFormatStyle.PMFormatString, sMainGroup:=sMainGroup, sSubGroup:=sSubGroup, sSubGroup2:=sSubGroup2, sSubGroup3:=sSubGroup3, sSubGroup4:="", sDisplayname:=CStr(vArray(pbObjectAndPropertyConsts.ACPPropertyName, lTemp2)).Trim() & " Line " & CStr(lTemp3), iIsDisplayed:=1, sLoop1:=sLoop1, sLoop2:=sLoop2, sLoop3:=sLoop3, sLoop4:="", lProductFamily:=v_lPMProductFamily, vDataModel:=DBNull.Value, vPropertyId:=DBNull.Value, r_oDatabase:=r_oDatabase), gPMConstants.PMEReturnCode)

                                                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                                Return gPMConstants.PMEReturnCode.PMFalse
                                                            End If
                                                        Next lTemp3

                                                        m_lReturn = CType(AddToWPFields(sFieldName:=sPrefix.ToString() & "Address" & CStr(lAddressCount) & _
                                                                    "PostalCode", sSQL:=sProcedureName, sColumnName:="address" & lAddressCount & "_postal_code", lColumnType:=gPMConstants.PMEFormatStyle.PMFormatString, sMainGroup:=sMainGroup, sSubGroup:=sSubGroup, sSubGroup2:=sSubGroup2, sSubGroup3:=sSubGroup3, sSubGroup4:="", sDisplayname:=CStr(vArray(pbObjectAndPropertyConsts.ACPPropertyName, lTemp2)).Trim() & " Postal Code", iIsDisplayed:=1, sLoop1:=sLoop1, sLoop2:=sLoop2, sLoop3:=sLoop3, sLoop4:="", lProductFamily:=v_lPMProductFamily, vDataModel:=DBNull.Value, vPropertyId:=DBNull.Value, r_oDatabase:=r_oDatabase), gPMConstants.PMEReturnCode)

                                                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                            Return gPMConstants.PMEReturnCode.PMFalse
                                                        End If

                                                        m_lReturn = CType(AddToWPFields(sFieldName:=sPrefix.ToString() & "Address" & CStr(lAddressCount) & _
                                                                    "Country", sSQL:=sProcedureName, sColumnName:="address" & lAddressCount & "_country", lColumnType:=gPMConstants.PMEFormatStyle.PMFormatString, sMainGroup:=sMainGroup, sSubGroup:=sSubGroup, sSubGroup2:=sSubGroup2, sSubGroup3:=sSubGroup3, sSubGroup4:="", sDisplayname:=CStr(vArray(pbObjectAndPropertyConsts.ACPPropertyName, lTemp2)).Trim() & " Country", iIsDisplayed:=1, sLoop1:=sLoop1, sLoop2:=sLoop2, sLoop3:=sLoop3, sLoop4:="", lProductFamily:=v_lPMProductFamily, vDataModel:=DBNull.Value, vPropertyId:=DBNull.Value, r_oDatabase:=r_oDatabase), gPMConstants.PMEReturnCode)

                                                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                            Return gPMConstants.PMEReturnCode.PMFalse
                                                        End If
                                                    ElseIf (vArray(pbObjectAndPropertyConsts.ACPSpecialsType, lTemp2)) = GISSharedPropertyConstants.ACOPartyTypeID And Not bFixedField Then
                                                        'If it's a party, need a link to the party table
                                                        sParameterList.Append("@" & sColumnName & " VARCHAR(255)," & Strings.Chr(13) & Strings.Chr(10))

                                                        lOtherTableCount += 1

                                                        sSelectList.Append( _
                                                                           sSelect & sColumnName & " = p" & CStr(lOtherTableCount) & ".name," & Strings.Chr(13) & Strings.Chr(10))

                                                        sTableList.Append("LEFT JOIN party p" & lOtherTableCount & " ON " & _
                                                                          sPrefix.ToString() & "." & sColumnName & " = p" & CStr(lOtherTableCount) & ".party_cnt" & Strings.Chr(13) & Strings.Chr(10))

                                                        m_lReturn = CType(AddToWPFields(sFieldName:=sPrefix.ToString() & sFieldName.ToString(), sSQL:=sProcedureName, sColumnName:=sColumnName, lColumnType:=gPMConstants.PMEFormatStyle.PMFormatString, sMainGroup:=sMainGroup, sSubGroup:=sSubGroup, sSubGroup2:=sSubGroup2, sSubGroup3:=sSubGroup3, sSubGroup4:="", sDisplayname:=sDisplayname.ToString(), iIsDisplayed:=1, sLoop1:=sLoop1, sLoop2:=sLoop2, sLoop3:=sLoop3, sLoop4:="", lProductFamily:=v_lPMProductFamily, vDataModel:=DBNull.Value, vPropertyId:=DBNull.Value, r_oDatabase:=r_oDatabase), gPMConstants.PMEReturnCode)

                                                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                            Return gPMConstants.PMEReturnCode.PMFalse
                                                        End If
                                                    ElseIf (vArray(pbObjectAndPropertyConsts.ACPSpecialsType, lTemp2)) = GISSharedPropertyConstants.ACOPMLookupTableName And Not bFixedField Then
                                                        'If it's a lookup, need a link to the lookup
                                                        sParameterList.Append("@" & sColumnName & " VARCHAR(255)," & Strings.Chr(13) & Strings.Chr(10))

                                                        lOtherTableCount += 1

                                                        sSelectList.Append( _
                                                                           sSelect & sColumnName & " = l" & CStr(lOtherTableCount) & ".description," & Strings.Chr(13) & Strings.Chr(10))

                                                        sTableList.Append("LEFT JOIN " & CStr(vArray(pbObjectAndPropertyConsts.ACPSpecialsTypeReference, lTemp2)) & " l" & CStr(lOtherTableCount) & " ON " & _
                                                                          sPrefix.ToString() & "." & CStr(vArray(pbObjectAndPropertyConsts.ACPColumnName, lTemp2)) & " = " & _
                                                                          "l" & CStr(lOtherTableCount) & "." & CStr(vArray(pbObjectAndPropertyConsts.ACPSpecialsTypeReference, lTemp2)) & "_id" & Strings.Chr(13) & Strings.Chr(10))

                                                        m_lReturn = CType(AddToWPFields(sFieldName:=sPrefix.ToString() & sFieldName.ToString(), sSQL:=sProcedureName, sColumnName:=sColumnName, lColumnType:=gPMConstants.PMEFormatStyle.PMFormatString, sMainGroup:=sMainGroup, sSubGroup:=sSubGroup, sSubGroup2:=sSubGroup2, sSubGroup3:=sSubGroup3, sSubGroup4:="", sDisplayname:=sDisplayname.ToString(), iIsDisplayed:=1, sLoop1:=sLoop1, sLoop2:=sLoop2, sLoop3:=sLoop3, sLoop4:="", lProductFamily:=v_lPMProductFamily, vDataModel:=DBNull.Value, vPropertyId:=DBNull.Value, r_oDatabase:=r_oDatabase), gPMConstants.PMEReturnCode)

                                                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                            Return gPMConstants.PMEReturnCode.PMFalse
                                                        End If
                                                    ElseIf (vArray(pbObjectAndPropertyConsts.ACPSpecialsType, lTemp2)) = GISSharedPropertyConstants.ACOGISUserDefHeaderID And Not bFixedField Then
                                                        'If it's one of our lookups, need a link to our lookup
                                                        sParameterList.Append("@" & sColumnName & " VARCHAR(255)," & Strings.Chr(13) & Strings.Chr(10))

                                                        lOtherTableCount += 1

                                                        sSelectList.Append( _
                                                                           sSelect & sColumnName & " = l" & CStr(lOtherTableCount) & ".description," & Strings.Chr(13) & Strings.Chr(10))

                                                        sTableList.Append("LEFT JOIN GIS_user_def_detail l" & lOtherTableCount & " ON " & _
                                                                          sPrefix.ToString() & "." & CStr(vArray(pbObjectAndPropertyConsts.ACPColumnName, lTemp2)) & " = " & _
                                                                          "l" & CStr(lOtherTableCount) & ".GIS_user_def_detail_id" & Strings.Chr(13) & Strings.Chr(10))

                                                        m_lReturn = CType(AddToWPFields(sFieldName:=sPrefix.ToString() & sFieldName.ToString(), sSQL:=sProcedureName, sColumnName:=sColumnName, lColumnType:=gPMConstants.PMEFormatStyle.PMFormatString, sMainGroup:=sMainGroup, sSubGroup:=sSubGroup, sSubGroup2:=sSubGroup2, sSubGroup3:=sSubGroup3, sSubGroup4:="", sDisplayname:=sDisplayname.ToString(), iIsDisplayed:=1, sLoop1:=sLoop1, sLoop2:=sLoop2, sLoop3:=sLoop3, sLoop4:="", lProductFamily:=v_lPMProductFamily, vDataModel:=DBNull.Value, vPropertyId:=DBNull.Value, r_oDatabase:=r_oDatabase), gPMConstants.PMEReturnCode)

                                                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                            Return gPMConstants.PMEReturnCode.PMFalse
                                                        End If
                                                    ElseIf (vArray(pbObjectAndPropertyConsts.ACPSpecialsType, lTemp2)) = GISSharedPropertyConstants.ACOProductID And Not bFixedField Then
                                                        'If it's a product, need a link to a policy...
                                                        sParameterList.Append("@" & sColumnName & " VARCHAR(255)," & Strings.Chr(13) & Strings.Chr(10))

                                                        lOtherTableCount += 1

                                                        sSelectList.Append( _
                                                                           sSelect & sColumnName & " = i" & CStr(lOtherTableCount) & ".insurance_ref," & Strings.Chr(13) & Strings.Chr(10))

                                                        sTableList.Append("LEFT JOIN insurance_file i" & lOtherTableCount & " ON " & _
                                                                          sPrefix.ToString() & "." & sColumnName & " = i" & CStr(lOtherTableCount) & ".insurance_file_cnt" & Strings.Chr(13) & Strings.Chr(10))

                                                        m_lReturn = CType(AddToWPFields(sFieldName:=sPrefix.ToString() & sFieldName.ToString(), sSQL:=sProcedureName, sColumnName:=sColumnName, lColumnType:=gPMConstants.PMEFormatStyle.PMFormatString, sMainGroup:=sMainGroup, sSubGroup:=sSubGroup, sSubGroup2:=sSubGroup2, sSubGroup3:=sSubGroup3, sSubGroup4:="", sDisplayname:=sDisplayname.ToString(), iIsDisplayed:=1, sLoop1:=sLoop1, sLoop2:=sLoop2, sLoop3:=sLoop3, sLoop4:="", lProductFamily:=v_lPMProductFamily, vDataModel:=DBNull.Value, vPropertyId:=DBNull.Value, r_oDatabase:=r_oDatabase), gPMConstants.PMEReturnCode)

                                                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                            Return gPMConstants.PMEReturnCode.PMFalse
                                                        End If

                                                        'else it's bog standard
                                                    Else
                                                        sParameterList.Append("@" & sColumnName & " ")

                                                        ' PW250703 - PS229  - don't add to
                                                        ' select list if field is from a fixed
                                                        ' table
                                                        If Not bFixedField Then
                                                            sSelectList.Append( _
                                                                               sSelect & sColumnName & " = " & sPrefix.ToString() & "." & sColumnName & "," & Strings.Chr(13) & Strings.Chr(10))
                                                        End If

                                                        Select Case vArray(pbObjectAndPropertyConsts.ACPDataType, lTemp2)
                                                            Case GISSharedConstants.GISDataTypeComment
                                                                sParameterList.Append("VARCHAR(4000)," & Strings.Chr(13) & Strings.Chr(10))
                                                                'sParameterList = sParameterList & "TEXT," & vbCrLf
                                                                lFormat = gPMConstants.PMEFormatStyle.PMFormatString

                                                            Case GISSharedConstants.GISDataTypeText
                                                                sTemp = "255"
                                                                sParameterList.Append("VARCHAR(" & sTemp & ")," & Strings.Chr(13) & Strings.Chr(10))
                                                                lFormat = gPMConstants.PMEFormatStyle.PMFormatString

                                                            Case GISSharedConstants.GISDataTypeDate
                                                                sParameterList.Append("DATETIME," & Strings.Chr(13) & Strings.Chr(10))
                                                                lFormat = gPMConstants.PMEFormatStyle.PMFormatDateLong

                                                            Case GISSharedConstants.GISDataTypeCurrency
                                                                sParameterList.Append("NUMERIC(19,4)," & Strings.Chr(13) & Strings.Chr(10))
                                                                lFormat = gPMConstants.PMEFormatStyle.PMFormatCurrency
                                                                'SJ 15/07/2004 - start
                                                            Case GISSharedConstants.GISDataTypeInteger
                                                                sParameterList.Append("INT," & Strings.Chr(13) & Strings.Chr(10))
                                                                lFormat = gPMConstants.PMEFormatStyle.PMFormatLong
                                                                'SJ 15/07/2004 - end
                                                            Case GISSharedConstants.GISDataTypePercentage
                                                                sParameterList.Append("NUMERIC(19,4)," & Strings.Chr(13) & Strings.Chr(10))
                                                                lFormat = gPMConstants.PMEFormatStyle.PMFormatPercent

                                                            Case GISSharedConstants.GISDataTypeNumeric
                                                                sParameterList.Append("NUMERIC(19,4)," & Strings.Chr(13) & Strings.Chr(10))
                                                                lFormat = gPMConstants.PMEFormatStyle.PMFormatLong

                                                            Case GISSharedConstants.GISDataTypeOption
                                                                sParameterList.Append("TINYINT," & Strings.Chr(13) & Strings.Chr(10))
                                                                lFormat = gPMConstants.PMEFormatStyle.PMFormatBoolean

                                                            Case Else
                                                                sParameterList.Append("VARCHAR(255)," & Strings.Chr(13) & Strings.Chr(10))
                                                                lFormat = gPMConstants.PMEFormatStyle.PMFormatString
                                                        End Select

                                                        sFinalSelectList.Append( _
                                                                                "'" & sColumnName & "' = @" & sColumnName & "," & Strings.Chr(13) & Strings.Chr(10))

                                                        'Add the record to wp_fields
                                                        ' PW240703 - PS229 - use a main type
                                                        ' of party for data model type of Party,
                                                        ' because the risk object isn't
                                                        ' really a risk object. But that's how
                                                        ' it's been done.

                                                        m_lReturn = CType(AddToWPFields(sFieldName:=sPrefix.ToString() & sFieldName.ToString(), sSQL:=sProcedureName, sColumnName:=sColumnName, lColumnType:=lFormat, sMainGroup:=sMainGroup, sSubGroup:=sSubGroup, sSubGroup2:=sSubGroup2, sSubGroup3:=sSubGroup3, sSubGroup4:="", sDisplayname:=sDisplayname.ToString(), iIsDisplayed:=1, sLoop1:=sLoop1, sLoop2:=sLoop2, sLoop3:=sLoop3, sLoop4:="", lProductFamily:=v_lPMProductFamily, vDataModel:=DBNull.Value, vPropertyId:=DBNull.Value, r_oDatabase:=r_oDatabase), gPMConstants.PMEReturnCode)

                                                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                            Return gPMConstants.PMEReturnCode.PMFalse
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        Next lTemp2

                                    End If

                                    If sParameterList.ToString() <> "" Then
                                        'Get rid of the trailing comma and vbCrLf
                                        sParameterList = New StringBuilder(sParameterList.ToString().Substring(0, sParameterList.ToString().Length - 3) & Strings.Chr(13) & Strings.Chr(10))
                                        sFinalSelectList = New StringBuilder(sFinalSelectList.ToString().Substring(0, sFinalSelectList.ToString().Length - 3) & Strings.Chr(13) & Strings.Chr(10))

                                        'Add the rest...
                                        ' PW250703 - PS229 - depending on what the object type
                                        ' is
                                        Select Case iObjectType
                                            Case GISDataModelType.GISOTRisk
                                                sTableList = New StringBuilder(sTableList.ToString().Substring(0, sTableList.ToString().Length - 2) & ", " & Strings.Chr(13) & Strings.Chr(10) & _
                                                             v_sDatamodel & "_Policy_binder pb," & Strings.Chr(13) & Strings.Chr(10) & _
                                                             "GIS_policy_link gpl," & Strings.Chr(13) & Strings.Chr(10))
                                                ' PW240703 - PS229 - party table required for
                                                ' data model type of Party, because the risk object isn't
                                                ' really a risk object. But that's how it's been done.
                                                If lGISDataModelType = GISDataModelType.GISDMTypeParty Then
                                                    sTableList.Append("party p" & Strings.Chr(13) & Strings.Chr(10))
                                                Else
                                                    sTableList.Append("insurance_file ifi" & Strings.Chr(13) & Strings.Chr(10))
                                                End If
                                            Case GISDataModelType.GISOTAssociatedClient
                                                sTableList.Append( _
                                                                  "LEFT JOIN party p ON pc.party_cnt = p.party_cnt" & Strings.Chr(13) & Strings.Chr(10) & _
                                                                  "LEFT JOIN party_type pt ON p.party_type_id = pt.party_type_id" & Strings.Chr(13) & Strings.Chr(10) & _
                                                                  "LEFT JOIN party_personal_client ppc ON pc.party_cnt = ppc.party_cnt" & Strings.Chr(13) & Strings.Chr(10) & _
                                                                  "LEFT JOIN party_lifestyle pl ON pc.party_cnt = pl.party_cnt" & Strings.Chr(13) & Strings.Chr(10))
                                            Case GISDataModelType.GISOTDisclosure
                                                sTableList.Append( _
                                                                  "LEFT JOIN disclosure_type dt ON pcv.disclosure_type_id = " & _
                                                                  "dt.disclosure_type_id" & Strings.Chr(13) & Strings.Chr(10))
                                        End Select
                                        'Drop it if it's already there
                                        DropExistingProcedure(v_sProcedureName:=sProcedureName, v_sPostFix:="", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

                                        DropExistingProcedure(v_sProcedureName:=sProcedureNameOldStyle, v_sPostFix:="", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

                                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                            Return gPMConstants.PMEReturnCode.PMFalse
                                        End If

                                        'remove the last comma and vbcrlf
                                        sSelectList = New StringBuilder(sSelectList.ToString().Substring(0, sSelectList.ToString().Length - 3) & Strings.Chr(13) & Strings.Chr(10))

                                        ' PW240703 - PS229 - don't need risk parameter for
                                        ' data model type of Party, because we're creating
                                        ' it in the party section of field manager.
                                        If lGISDataModelType <> GISDataModelType.GISDMTypeParty Then
                                            sSelectList = New StringBuilder("If @RiskId Is Null" & Strings.Chr(13) & Strings.Chr(10) & _
                                                          sSelectList.ToString() & sTableList.ToString() & sWhereList1.ToString() & sWhereList3 & _
                                                          "Else" & Strings.Chr(13) & Strings.Chr(10) & _
                                                          sSelectList.ToString() & sTableList.ToString() & sWhereList2.ToString() & sWhereList3)
                                        Else
                                            sSelectList.Append(sTableList.ToString() & sWhereList1.ToString() & sWhereList3)
                                        End If
                                        'Create the new procedure
                                        sSQL = ""

                                        ' PW240703 - PS229 - don't need risk parameter for
                                        ' data model type of Party, because we're creating
                                        ' it in the party section of field manager.
                                        sSQL = sSQL & "CREATE PROCEDURE " & sProcedureName & Strings.Chr(13) & Strings.Chr(10) & _
                                               "@PartyCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                                               "@InsuranceFileCnt INT," & Strings.Chr(13) & Strings.Chr(10)
                                        If lGISDataModelType <> GISDataModelType.GISDMTypeParty Then
                                            sSQL = sSQL & "@RiskId INT = NULL," & Strings.Chr(13) & Strings.Chr(10)
                                        End If
                                        sSQL = sSQL & "@ClaimCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                                               "@DocumentRef VARCHAR(25)"
                                        'dynamically generate instance variables
                                        generateInstanceVariables(sSQL, iInstanceCount)

                                        ' PW250703 - PS229 - need to get the insurance folder cnt
                                        ' for object type of associated client / disclosure
                                        sSQL = sSQL & Strings.Chr(13) & Strings.Chr(10) & "AS" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10)
                                        ' PW200803 - CQ1912 - only disclosure needs this now
                                        If iObjectType = GISDataModelType.GISOTDisclosure Then
                                            sSQL = sSQL & _
                                                   "DECLARE @insurance_folder_cnt INT" & Strings.Chr(13) & Strings.Chr(10) & _
                                                   "SELECT @insurance_folder_cnt = insurance_folder_cnt" & Strings.Chr(13) & Strings.Chr(10) & _
                                                   "  FROM insurance_file" & Strings.Chr(13) & Strings.Chr(10) & _
                                                   " WHERE insurance_file_cnt = @InsuranceFileCnt" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & _
                                                   sSelectList.ToString() & Strings.Chr(13) & Strings.Chr(10)
                                        Else
                                            sSQL = sSQL & sSelectList.ToString() & Strings.Chr(13) & Strings.Chr(10)
                                        End If

                                        m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & sProcedureName & " StoredProcedure", bStoredProcedure:=False)

                                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                            Return gPMConstants.PMEReturnCode.PMFalse
                                        End If

                                        'Set permissions

                                        sSQL = "GRANT EXECUTE ON dbo." & sProcedureName & " TO PUBLIC" & Strings.Chr(13) & Strings.Chr(10)

                                        m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Grant " & sProcedureName & " StoredProcedure", bStoredProcedure:=False)

                                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                            Return gPMConstants.PMEReturnCode.PMFalse
                                        End If

                                    End If

                                End If
                            End If

                        End If ' CLAIM RISK OR NORMAL RISK

                End Select

            Next lTemp

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GenerateStoredProcedure Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateStoredProcedure", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub generateInstanceVariables(ByRef sSQL As String, ByVal lLevel As Integer)

        If lLevel < 3 Then lLevel = 3
        For lCount As Integer = 1 To lLevel
            sSQL = sSQL & "," & Strings.Chr(13) & Strings.Chr(10) & "@Instance" & CStr(lCount) & " INT = NULL"
        Next
    End Sub

    ' ***************************************************************** '
    '
    ' Name: GenerateSumInsuredFields
    '
    ' Description:
    '
    ' History: 26/01/2001 Tomo    - Created.
    '          27/09/2002 CMG-SJP - Moved from Business class of Maintain
    '                               Data model to common bas module so
    '                               that can shared with other projects
    '                               Eg Data Model Import Export Tool.
    '
    ' ***************************************************************** '
    Public Function GenerateSumInsuredFields(ByVal v_sDatamodel As String, ByRef lSumInsuredTypeId As Integer, ByRef r_oDatabase As dPMDAO.Database, ByVal v_lPMProductFamily As Integer) As Integer

        Dim result As Integer = 0
        Dim sProcedureName, sProcedureNameOldStyle, sPrefix, sDescription, sSelectList, sSelectList2, sSelectList3, sTableList, sWhereList1, sWhereList2 As String
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sPrefix = v_sDatamodel & "SI"

            sDescription = ""

            If Not Information.IsArray(m_vSumInsured) Then
                m_lReturn = CType(GetALookup(iLanguageID:=1, sTableName:="sum_insured_type", vArray:=m_vSumInsured, r_oDatabase:=r_oDatabase), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            If Information.IsArray(m_vSumInsured) Then

                For lTemp As Integer = m_vSumInsured.GetLowerBound(1) To m_vSumInsured.GetUpperBound(1)

                    If (m_vSumInsured(0, lTemp)) = lSumInsuredTypeId Then

                        sDescription = CStr(m_vSumInsured(1, lTemp)) & " SI"
                        Exit For
                    End If
                Next lTemp
            End If

            'The get keys procedure...

            sProcedureNameOldStyle = "sp_wp_" & v_sDatamodel & "SumInsured" & CStr(lSumInsuredTypeId) & "_get_keys"
            sProcedureName = "spg_wp_" & v_sDatamodel & "SumInsured" & CStr(lSumInsuredTypeId) & "_get_keys"

            sSelectList = "SELECT " & sPrefix & ".sequence_id" & Strings.Chr(13) & Strings.Chr(10)

            sTableList = "FROM " & v_sDatamodel & "_sum_insured " & sPrefix & ", " & Strings.Chr(13) & Strings.Chr(10) & _
                         v_sDatamodel & "_Policy_binder pb," & Strings.Chr(13) & Strings.Chr(10) & _
                         "GIS_policy_link gpl," & Strings.Chr(13) & Strings.Chr(10) & _
                         "insurance_file ifi" & Strings.Chr(13) & Strings.Chr(10)

            sWhereList1 = "WHERE ifi.insurance_file_cnt = @InsuranceFileCnt" & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND gpl.insurance_file_cnt = ifi.insurance_file_cnt" & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND gpl.Risk_id is null" & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND pb.gis_policy_link_id = gpl.gis_policy_link_id" & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND " & sPrefix & "." & v_sDatamodel & "_policy_binder_id = pb." & v_sDatamodel & "_Policy_binder_id" & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND " & sPrefix & ".sum_insured_type_id = " & CStr(lSumInsuredTypeId) & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND " & sPrefix & ".sequence_id > 1" & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND ISNULL(" & sPrefix & ".date_deleted, '1899-12-29') = '1899-12-29'" & Strings.Chr(13) & Strings.Chr(10)

            sWhereList2 = "WHERE ifi.insurance_file_cnt = @InsuranceFileCnt" & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND gpl.Risk_id = @RiskId" & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND pb.gis_policy_link_id = gpl.gis_policy_link_id" & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND " & sPrefix & "." & v_sDatamodel & "_policy_binder_id = pb." & v_sDatamodel & "_Policy_binder_id" & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND " & sPrefix & ".sum_insured_type_id = " & CStr(lSumInsuredTypeId) & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND " & sPrefix & ".sequence_id > 1" & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND ISNULL(" & sPrefix & ".date_deleted, '1899-12-29') = '1899-12-29'" & Strings.Chr(13) & Strings.Chr(10)

            'Drop it if it's already there
            DropExistingProcedure(sProcedureName, "", m_lReturn, r_oDatabase)
            DropExistingProcedure(sProcedureNameOldStyle, "", m_lReturn, r_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sSelectList = "If @RiskId Is Null" & Strings.Chr(13) & Strings.Chr(10) & _
                          sSelectList & sTableList & sWhereList1 & _
                          "Else" & Strings.Chr(13) & Strings.Chr(10) & _
                          sSelectList & sTableList & sWhereList2

            'Create the new procedure
            sSQL = ""

            sSQL = sSQL & "CREATE PROCEDURE " & sProcedureName & " @PartyCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@InsuranceFileCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@RiskId INT = NULL," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@ClaimCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@DocumentRef VARCHAR(25)," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@Instance1 INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@Instance2 INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@Instance3 INT" & Strings.Chr(13) & Strings.Chr(10) & _
                   "AS" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10)

            sSQL = sSQL & sSelectList & Strings.Chr(13) & Strings.Chr(10)

            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & sProcedureName & " StoredProcedure", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set permissions

            sSQL = "GRANT EXECUTE ON dbo." & sProcedureName & " TO PUBLIC" & Strings.Chr(13) & Strings.Chr(10)

            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Grant " & sProcedureName & " StoredProcedure", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'The get parent key procedure - needed because of the way DP now works...
            sProcedureName = "spg_wp_" & v_sDatamodel & "SumInsured" & CStr(lSumInsuredTypeId) & "_get_parent_key"

            'Doesn't need to do a lot, just exist and pass back something that's ignored anyway
            sSelectList = "SELECT 1" & Strings.Chr(13) & Strings.Chr(10)

            sTableList = ""

            sWhereList1 = ""

            sWhereList2 = ""

            'Drop it if it's already there
            DropExistingProcedure(sProcedureName, "", m_lReturn, r_oDatabase)
            DropExistingProcedure(sProcedureNameOldStyle, "", m_lReturn, r_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sSelectList = "If @RiskId Is Null" & Strings.Chr(13) & Strings.Chr(10) & _
                          sSelectList & sTableList & sWhereList1 & _
                          "Else" & Strings.Chr(13) & Strings.Chr(10) & _
                          sSelectList & sTableList & sWhereList2

            'Create the new procedure
            sSQL = ""

            sSQL = sSQL & "CREATE PROCEDURE " & sProcedureName & " @PartyCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@InsuranceFileCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@RiskId INT = NULL," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@ClaimCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@DocumentRef VARCHAR(25)," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@Instance1 INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@Instance2 INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@Instance3 INT" & Strings.Chr(13) & Strings.Chr(10) & _
                   "AS" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10)

            sSQL = sSQL & sSelectList & Strings.Chr(13) & Strings.Chr(10)

            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & sProcedureName & " StoredProcedure", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set permissions

            sSQL = "GRANT EXECUTE ON dbo." & sProcedureName & " TO PUBLIC" & Strings.Chr(13) & Strings.Chr(10)

            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Grant " & sProcedureName & " StoredProcedure", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'The main procedure...
            sProcedureName = "spg_wp_" & v_sDatamodel & "SumInsured" & CStr(lSumInsuredTypeId)

            sSelectList = "SELECT description = " & sPrefix & ".description," & Strings.Chr(13) & Strings.Chr(10) & _
                          "reference = " & sPrefix & ".reference," & Strings.Chr(13) & Strings.Chr(10) & _
                          "sum_insured = " & sPrefix & ".sum_insured," & Strings.Chr(13) & Strings.Chr(10) & _
                          "date_added = " & sPrefix & ".date_added," & Strings.Chr(13) & Strings.Chr(10) & _
                          "date_deleted = " & sPrefix & ".date_deleted," & Strings.Chr(13) & Strings.Chr(10) & _
                          "is_valuation_required = " & sPrefix & ".is_valuation_required," & Strings.Chr(13) & Strings.Chr(10) & _
                          "valuation_date = " & sPrefix & ".valuation_date" & Strings.Chr(13) & Strings.Chr(10)

            sTableList = "FROM " & v_sDatamodel & "_sum_insured " & sPrefix & ", " & Strings.Chr(13) & Strings.Chr(10) & _
                         v_sDatamodel & "_Policy_binder pb," & Strings.Chr(13) & Strings.Chr(10) & _
                         "GIS_policy_link gpl," & Strings.Chr(13) & Strings.Chr(10) & _
                         "insurance_file ifi" & Strings.Chr(13) & Strings.Chr(10)

            sWhereList1 = "WHERE ifi.insurance_file_cnt = @InsuranceFileCnt" & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND gpl.insurance_file_cnt = ifi.insurance_file_cnt" & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND gpl.Risk_id is null" & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND pb.gis_policy_link_id = gpl.gis_policy_link_id" & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND " & sPrefix & "." & v_sDatamodel & "_policy_binder_id = pb." & v_sDatamodel & "_Policy_binder_id" & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND " & sPrefix & ".sum_insured_type_id = " & CStr(lSumInsuredTypeId) & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND " & sPrefix & ".sequence_id = @Instance2" & Strings.Chr(13) & Strings.Chr(10)

            sWhereList2 = "WHERE ifi.insurance_file_cnt = @InsuranceFileCnt" & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND gpl.Risk_id = @RiskId" & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND pb.gis_policy_link_id = gpl.gis_policy_link_id" & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND " & sPrefix & "." & v_sDatamodel & "_policy_binder_id = pb." & v_sDatamodel & "_Policy_binder_id" & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND " & sPrefix & ".sum_insured_type_id = " & CStr(lSumInsuredTypeId) & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND " & sPrefix & ".sequence_id = @Instance2" & Strings.Chr(13) & Strings.Chr(10)

            'Drop it if it's already there
            DropExistingProcedure(sProcedureName, "", m_lReturn, r_oDatabase)
            DropExistingProcedure(sProcedureNameOldStyle, "", m_lReturn, r_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sSelectList = "If @RiskId Is Null" & Strings.Chr(13) & Strings.Chr(10) & _
                          sSelectList & sTableList & sWhereList1 & _
                          "Else" & Strings.Chr(13) & Strings.Chr(10) & _
                          sSelectList & sTableList & sWhereList2

            'Create the new procedure
            sSQL = ""

            sSQL = sSQL & "CREATE PROCEDURE " & sProcedureName & " @PartyCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@InsuranceFileCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@RiskId INT = NULL," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@ClaimCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@DocumentRef VARCHAR(25)," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@Instance1 INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@Instance2 INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@Instance3 INT" & Strings.Chr(13) & Strings.Chr(10) & _
                   "AS" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10)

            sSQL = sSQL & sSelectList & Strings.Chr(13) & Strings.Chr(10)

            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & sProcedureName & " StoredProcedure", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set permissions

            sSQL = "GRANT EXECUTE ON dbo." & sProcedureName & " TO PUBLIC" & Strings.Chr(13) & Strings.Chr(10)

            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Grant " & sProcedureName & " StoredProcedure", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Now let's set up the individual wp fields
            'Delete the records on wp_fields
            sSQL = ""

            sSQL = sSQL & "DELETE FROM wp_fields" & Strings.Chr(13) & Strings.Chr(10) & _
                   "WHERE sql = '" & sProcedureName & "'" & Strings.Chr(13) & Strings.Chr(10)

            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Delete " & sProcedureName & " WPFields", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(AddToWPFields(sFieldName:=v_sDatamodel & "SumInsuredDescription" & CStr(lSumInsuredTypeId), sSQL:=sProcedureName, sColumnName:="description", lColumnType:=gPMConstants.PMEFormatStyle.PMFormatString, sMainGroup:="Risk", sSubGroup:=sDescription, sSubGroup2:="", sSubGroup3:="", sSubGroup4:="", sDisplayname:="Description", iIsDisplayed:=1, sLoop1:=v_sDatamodel & "SumInsured" & CStr(lSumInsuredTypeId), sLoop2:="", sLoop3:="", sLoop4:="", lProductFamily:=v_lPMProductFamily, vDataModel:=DBNull.Value, vPropertyId:=DBNull.Value, r_oDatabase:=r_oDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(AddToWPFields(sFieldName:=v_sDatamodel & "SumInsuredReference" & CStr(lSumInsuredTypeId), sSQL:=sProcedureName, sColumnName:="reference", lColumnType:=gPMConstants.PMEFormatStyle.PMFormatString, sMainGroup:="Risk", sSubGroup:=sDescription, sSubGroup2:="", sSubGroup3:="", sSubGroup4:="", sDisplayname:="Reference", iIsDisplayed:=1, sLoop1:=v_sDatamodel & "SumInsured" & CStr(lSumInsuredTypeId), sLoop2:="", sLoop3:="", sLoop4:="", lProductFamily:=v_lPMProductFamily, vDataModel:=DBNull.Value, vPropertyId:=DBNull.Value, r_oDatabase:=r_oDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(AddToWPFields(sFieldName:=v_sDatamodel & "SumInsuredSumInsured" & CStr(lSumInsuredTypeId), sSQL:=sProcedureName, sColumnName:="sum_insured", lColumnType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, sMainGroup:="Risk", sSubGroup:=sDescription, sSubGroup2:="", sSubGroup3:="", sSubGroup4:="", sDisplayname:="Sum Insured", iIsDisplayed:=1, sLoop1:=v_sDatamodel & "SumInsured" & CStr(lSumInsuredTypeId), sLoop2:="", sLoop3:="", sLoop4:="", lProductFamily:=v_lPMProductFamily, vDataModel:=DBNull.Value, vPropertyId:=DBNull.Value, r_oDatabase:=r_oDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(AddToWPFields(sFieldName:=v_sDatamodel & "SumInsuredDateAdded" & CStr(lSumInsuredTypeId), sSQL:=sProcedureName, sColumnName:="date_added", lColumnType:=gPMConstants.PMEFormatStyle.PMFormatDateLong, sMainGroup:="Risk", sSubGroup:=sDescription, sSubGroup2:="", sSubGroup3:="", sSubGroup4:="", sDisplayname:="Date Added", iIsDisplayed:=1, sLoop1:=v_sDatamodel & "SumInsured" & CStr(lSumInsuredTypeId), sLoop2:="", sLoop3:="", sLoop4:="", lProductFamily:=v_lPMProductFamily, vDataModel:=DBNull.Value, vPropertyId:=DBNull.Value, r_oDatabase:=r_oDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(AddToWPFields(sFieldName:=v_sDatamodel & "SumInsuredDateDeleted" & CStr(lSumInsuredTypeId), sSQL:=sProcedureName, sColumnName:="date_deleted", lColumnType:=gPMConstants.PMEFormatStyle.PMFormatDateLong, sMainGroup:="Risk", sSubGroup:=sDescription, sSubGroup2:="", sSubGroup3:="", sSubGroup4:="", sDisplayname:="Date Deleted", iIsDisplayed:=1, sLoop1:=v_sDatamodel & "SumInsured" & CStr(lSumInsuredTypeId), sLoop2:="", sLoop3:="", sLoop4:="", lProductFamily:=v_lPMProductFamily, vDataModel:=DBNull.Value, vPropertyId:=DBNull.Value, r_oDatabase:=r_oDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(AddToWPFields(sFieldName:=v_sDatamodel & "SumInsuredIsValuationRequired" & CStr(lSumInsuredTypeId), sSQL:=sProcedureName, sColumnName:="is_valuation_required", lColumnType:=gPMConstants.PMEFormatStyle.PMFormatBoolean, sMainGroup:="Risk", sSubGroup:=sDescription, sSubGroup2:="", sSubGroup3:="", sSubGroup4:="", sDisplayname:="Is valuation required", iIsDisplayed:=1, sLoop1:=v_sDatamodel & "SumInsured" & CStr(lSumInsuredTypeId), sLoop2:="", sLoop3:="", sLoop4:="", lProductFamily:=v_lPMProductFamily, vDataModel:=DBNull.Value, vPropertyId:=DBNull.Value, r_oDatabase:=r_oDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(AddToWPFields(sFieldName:=v_sDatamodel & "SumInsuredValuationDate" & CStr(lSumInsuredTypeId), sSQL:=sProcedureName, sColumnName:="valuation_date", lColumnType:=gPMConstants.PMEFormatStyle.PMFormatDateLong, sMainGroup:="Risk", sSubGroup:=sDescription, sSubGroup2:="", sSubGroup3:="", sSubGroup4:="", sDisplayname:="Valuation date", iIsDisplayed:=1, sLoop1:=v_sDatamodel & "SumInsured" & CStr(lSumInsuredTypeId), sLoop2:="", sLoop3:="", sLoop4:="", lProductFamily:=v_lPMProductFamily, vDataModel:=DBNull.Value, vPropertyId:=DBNull.Value, r_oDatabase:=r_oDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'The total procedure...
            sProcedureName = "spg_wp_" & v_sDatamodel & "SumInsuredTotal" & CStr(lSumInsuredTypeId)

            sSelectList = "SELECT @rate = " & sPrefix & ".rate," & Strings.Chr(13) & Strings.Chr(10) & _
                          "@Premium = " & sPrefix & ".premium" & Strings.Chr(13) & Strings.Chr(10)

            sSelectList2 = "SELECT @total_sum_insured = sum(" & sPrefix & ".sum_insured)" & Strings.Chr(13) & Strings.Chr(10)

            sSelectList3 = "SELECT total_sum_insured = @total_sum_insured," & Strings.Chr(13) & Strings.Chr(10) & _
                           "rate = @rate," & Strings.Chr(13) & Strings.Chr(10) & _
                           "premium = @premium" & Strings.Chr(13) & Strings.Chr(10)

            sTableList = "FROM " & v_sDatamodel & "_sum_insured " & sPrefix & ", " & Strings.Chr(13) & Strings.Chr(10) & _
                         v_sDatamodel & "_Policy_binder pb," & Strings.Chr(13) & Strings.Chr(10) & _
                         "GIS_policy_link gpl," & Strings.Chr(13) & Strings.Chr(10) & _
                         "insurance_file ifi" & Strings.Chr(13) & Strings.Chr(10)

            sWhereList1 = "WHERE ifi.insurance_file_cnt = @InsuranceFileCnt" & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND gpl.insurance_file_cnt = ifi.insurance_file_cnt" & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND gpl.Risk_id is null" & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND pb.gis_policy_link_id = gpl.gis_policy_link_id" & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND " & sPrefix & "." & v_sDatamodel & "_policy_binder_id = pb." & v_sDatamodel & "_Policy_binder_id" & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND " & sPrefix & ".sum_insured_type_id = " & CStr(lSumInsuredTypeId) & Strings.Chr(13) & Strings.Chr(10)

            sWhereList2 = "WHERE ifi.insurance_file_cnt = @InsuranceFileCnt" & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND gpl.Risk_id = @RiskId" & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND pb.gis_policy_link_id = gpl.gis_policy_link_id" & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND " & sPrefix & "." & v_sDatamodel & "_policy_binder_id = pb." & v_sDatamodel & "_Policy_binder_id" & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND " & sPrefix & ".sum_insured_type_id = " & CStr(lSumInsuredTypeId) & Strings.Chr(13) & Strings.Chr(10)

            'Drop it if it's already there
            DropExistingProcedure(sProcedureName, "", m_lReturn, r_oDatabase)
            DropExistingProcedure(sProcedureNameOldStyle, "", m_lReturn, r_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sSelectList = "If @RiskId Is Null" & Strings.Chr(13) & Strings.Chr(10) & _
                          "Begin" & Strings.Chr(13) & Strings.Chr(10) & _
                          sSelectList & sTableList & sWhereList1 & _
                          "AND " & sPrefix & ".sequence_id = 1" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & _
                          sSelectList2 & sTableList & sWhereList1 & _
                          "AND ISNULL(" & sPrefix & ".date_deleted, '1899-12-29') = '1899-12-29'" & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND " & sPrefix & ".sequence_id > 1" & Strings.Chr(13) & Strings.Chr(10) & _
                          "End" & Strings.Chr(13) & Strings.Chr(10) & _
                          "Else" & Strings.Chr(13) & Strings.Chr(10) & _
                          "Begin" & Strings.Chr(13) & Strings.Chr(10) & _
                          sSelectList & sTableList & sWhereList2 & _
                          "AND " & sPrefix & ".sequence_id = 1" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & _
                          sSelectList2 & sTableList & sWhereList2 & _
                          "AND ISNULL(" & sPrefix & ".date_deleted, '1899-12-29') = '1899-12-29'" & Strings.Chr(13) & Strings.Chr(10) & _
                          "AND " & sPrefix & ".sequence_id > 1" & Strings.Chr(13) & Strings.Chr(10) & _
                          "End" & Strings.Chr(13) & Strings.Chr(10) & _
                          sSelectList3 & Strings.Chr(13) & Strings.Chr(10)

            'Create the new procedure
            sSQL = ""

            sSQL = sSQL & "CREATE PROCEDURE " & sProcedureName & " @PartyCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@InsuranceFileCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@RiskId INT = NULL," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@ClaimCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@DocumentRef VARCHAR(25)," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@Instance1 INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@Instance2 INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@Instance3 INT" & Strings.Chr(13) & Strings.Chr(10) & _
                   "AS" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & _
                   "DECLARE @total_sum_insured NUMERIC(19,4)," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@rate NUMERIC(7,4)," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@premium NUMERIC(19,4)" & Strings.Chr(13) & Strings.Chr(10)

            sSQL = sSQL & sSelectList & Strings.Chr(13) & Strings.Chr(10)

            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & sProcedureName & " StoredProcedure", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set permissions

            sSQL = "GRANT EXECUTE ON dbo." & sProcedureName & " TO PUBLIC" & Strings.Chr(13) & Strings.Chr(10)

            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Grant " & sProcedureName & " StoredProcedure", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Now let's set up the total wp fields
            'Delete the records on wp_fields
            sSQL = ""

            sSQL = sSQL & "DELETE FROM wp_fields" & Strings.Chr(13) & Strings.Chr(10) & _
                   "WHERE sql = '" & sProcedureName & "'" & Strings.Chr(13) & Strings.Chr(10)

            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Delete " & sProcedureName & " WPFields", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(AddToWPFields(sFieldName:=v_sDatamodel & "TotalSumInsured" & CStr(lSumInsuredTypeId), sSQL:=sProcedureName, sColumnName:="total_sum_insured", lColumnType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, sMainGroup:="Risk", sSubGroup:=sDescription, sSubGroup2:="", sSubGroup3:="", sSubGroup4:="", sDisplayname:="Total Sum Insured", iIsDisplayed:=1, sLoop1:="", sLoop2:="", sLoop3:="", sLoop4:="", lProductFamily:=v_lPMProductFamily, vDataModel:=DBNull.Value, vPropertyId:=DBNull.Value, r_oDatabase:=r_oDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(AddToWPFields(sFieldName:=v_sDatamodel & "Rate" & CStr(lSumInsuredTypeId), sSQL:=sProcedureName, sColumnName:="rate", lColumnType:=gPMConstants.PMEFormatStyle.PMFormatPercent, sMainGroup:="Risk", sSubGroup:=sDescription, sSubGroup2:="", sSubGroup3:="", sSubGroup4:="", sDisplayname:="Rate", iIsDisplayed:=1, sLoop1:="", sLoop2:="", sLoop3:="", sLoop4:="", lProductFamily:=v_lPMProductFamily, vDataModel:=DBNull.Value, vPropertyId:=DBNull.Value, r_oDatabase:=r_oDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(AddToWPFields(sFieldName:=v_sDatamodel & "Premium" & CStr(lSumInsuredTypeId), sSQL:=sProcedureName, sColumnName:="premium", lColumnType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, sMainGroup:="Risk", sSubGroup:=sDescription, sSubGroup2:="", sSubGroup3:="", sSubGroup4:="", sDisplayname:="Premium", iIsDisplayed:=1, sLoop1:="", sLoop2:="", sLoop3:="", sLoop4:="", lProductFamily:=v_lPMProductFamily, vDataModel:=DBNull.Value, vPropertyId:=DBNull.Value, r_oDatabase:=r_oDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GenerateSumInsuredFields Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateSumInsuredFields", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: AddToWPFields
    '
    ' Description:
    '
    ' History: 03/04/2000 Tomo    - Created.
    '          27/09/2002 CMG-SJP - Moved from Business class of Maintain
    '                               Data model to common bas module so
    '                               that can shared with other projects
    '                               Eg Data Model Import Export Tool.
    '
    ' ***************************************************************** '
    Private Function AddToWPFields(ByRef sFieldName As String, ByRef sSQL As String, ByRef sColumnName As String, ByRef lColumnType As Integer, ByRef sMainGroup As String, ByRef sSubGroup As String, ByRef sSubGroup2 As String, ByRef sSubGroup3 As String, ByRef sSubGroup4 As String, ByRef sDisplayname As String, ByRef iIsDisplayed As Integer, ByRef sLoop1 As String, ByRef sLoop2 As String, ByRef sLoop3 As String, ByRef sLoop4 As String, ByRef lProductFamily As Integer, ByRef vDataModel As Object, ByRef vPropertyId As Object, ByRef r_oDatabase As dPMDAO.Database) As Integer

        Dim result As Integer = 0
        Dim vLoop1 As String = ""
        Dim vLoop2 As String = ""
        Dim vLoop3 As String = ""
        Dim vLoop4 As String = ""

        

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(GetValidFieldName(sFieldName:=sFieldName, r_oDatabase:=r_oDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            r_oDatabase.Parameters.Clear()

            If sLoop1.Trim() = "" Then

                vLoop1 = Nothing
            Else
                vLoop1 = sLoop1.Trim()
            End If

            If sLoop2.Trim() = "" Then

                vLoop2 = Nothing
            Else
                vLoop2 = sLoop2.Trim()
            End If

            If sLoop3.Trim() = "" Then

                vLoop3 = Nothing
            Else
                vLoop3 = sLoop3.Trim()
            End If

            If sLoop4.Trim() = "" Then

                vLoop4 = Nothing
            Else
                vLoop4 = sLoop4.Trim()
            End If

            m_lReturn = r_oDatabase.Parameters.Add(sName:="field_name", vValue:=sFieldName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = r_oDatabase.Parameters.Add(sName:="sql", vValue:=sSQL, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = r_oDatabase.Parameters.Add(sName:="column_name", vValue:=sColumnName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = r_oDatabase.Parameters.Add(sName:="column_type", vValue:=CStr(lColumnType), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = r_oDatabase.Parameters.Add(sName:="main_group", vValue:=sMainGroup, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = r_oDatabase.Parameters.Add(sName:="sub_group", vValue:=sSubGroup, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = r_oDatabase.Parameters.Add(sName:="display_name", vValue:=sDisplayname, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = r_oDatabase.Parameters.Add(sName:="is_displayed", vValue:=CStr(iIsDisplayed), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = r_oDatabase.Parameters.Add(sName:="loop1", vValue:=vLoop1, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = r_oDatabase.Parameters.Add(sName:="loop2", vValue:=vLoop2, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = r_oDatabase.Parameters.Add(sName:="loop3", vValue:=vLoop3, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = r_oDatabase.Parameters.Add(sName:="loop4", vValue:=vLoop4, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = r_oDatabase.Parameters.Add(sName:="product_family", vValue:=CStr(lProductFamily), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = r_oDatabase.Parameters.Add(sName:="data_model", vValue:=CStr(vDataModel), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = r_oDatabase.Parameters.Add(sName:="property_id", vValue:=CStr(vPropertyId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = r_oDatabase.Parameters.Add(sName:="sub_group2", vValue:=sSubGroup2, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = r_oDatabase.Parameters.Add(sName:="sub_group3", vValue:=sSubGroup3, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = r_oDatabase.Parameters.Add(sName:="sub_group4", vValue:=sSubGroup4, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = r_oDatabase.SQLAction(sSQL:=ACInsertWPFieldsSQL, sSQLName:=ACInsertWPFieldsName, bStoredProcedure:=ACInsertWPFieldsStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: GenerateStandardWordingFields
    '
    ' Description:
    '
    ' History: 26/01/2001 Tomo    - Created.
    '          27/09/2002 CMG-SJP - Moved from Business class of Maintain
    '                               Data model to common bas module so
    '                               that can shared with other projects
    '                               Eg Data Model Import Export Tool.
    '
    ' ***************************************************************** '
    Private Function GenerateStandardWordingFields() As Integer

        Dim result As Integer = 0
        

            Return gPMConstants.PMEReturnCode.PMTrue

    End Function

    ' ***************************************************************** '
    '
    ' Name: GetProcedureName
    '
    ' Description:
    '
    ' History: 19/03/2001 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function GetProcedureName(ByRef r_sProcedureName As String, ByVal v_sDatamodel As String) As Integer

        Dim result As Integer = 0
        Dim sTemp As New StringBuilder
        Dim lTemp As Integer

        

            result = gPMConstants.PMEReturnCode.PMTrue

            sTemp = New StringBuilder("")

            r_sProcedureName = StripDataModelCode(v_vTheString:=r_sProcedureName, v_sDatamodel:=v_sDatamodel)
            lTemp = (r_sProcedureName.IndexOf("_"c) + 1)

            While lTemp <> 0
                sTemp.Append(r_sProcedureName.Substring(0, lTemp - 1))
                r_sProcedureName = r_sProcedureName.Substring(lTemp)
                lTemp = (r_sProcedureName.IndexOf("_"c) + 1)
            End While

            sTemp.Append(r_sProcedureName)

            r_sProcedureName = sTemp.ToString()

            Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: StripDataModelCode
    '
    ' Description:
    '
    ' History: ??/??/???? ???     - Created.
    '          27/09/2002 CMG-SJP - Moved from Business class of Maintain
    '                               Data model to common bas module so
    '                               that can shared with other projects
    '                               Eg Data Model Import Export Tool.
    '
    ' ***************************************************************** '
    Public Function StripDataModelCode(ByRef v_vTheString As String, ByRef v_sDatamodel As String) As String

        Dim result As String = String.Empty
        result = v_vTheString
        If v_vTheString.Substring(0, v_sDatamodel.Length) = v_sDatamodel Or v_vTheString.Substring(0, v_sDatamodel.Length) = v_sDatamodel.ToLower() Then
            result = Mid(v_vTheString, v_sDatamodel.Length + 1)
        End If

        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: IsParentObjectMultipleInstance
    '
    ' Description:
    '
    ' History: 02/05/2002 CLG     - Created.
    '          27/09/2002 CMG-SJP - Moved from Business class of Maintain
    '                               Data model to common bas module so
    '                               that can shared with other projects
    '                               Eg Data Model Import Export Tool.
    '
    ' ***************************************************************** '
    Private Function IsTopLevelParentObjectMultipleInstance(ByVal v_sObjectName As String, ByRef r_vGISObject(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lParentId As Integer
        Dim lTempParentId As String = ""

        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".IsObjectMultipleInstance")

        

            lParentId = -1

            If Information.IsArray(r_vGISObject) Then
                For lTemp As Integer = r_vGISObject.GetLowerBound(1) To r_vGISObject.GetUpperBound(1)

                    If CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOObjectName, lTemp)) = v_sObjectName Then

                        lParentId = CInt(r_vGISObject(pbObjectAndPropertyConsts.ACOParentObjectId, lTemp))
                        Exit For
                    End If
                Next
            End If

            If lParentId <> -1 And Information.IsArray(r_vGISObject) Then

                lTempParentId = CStr(lParentId)
                While StringsHelper.ToDoubleSafe(lTempParentId) <> 0
                    For lTemp As Integer = r_vGISObject.GetLowerBound(1) To r_vGISObject.GetUpperBound(1)

                        If CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOGISObjectId, lTemp)) = lTempParentId Then
                            lParentId = CInt(lTempParentId)

                            lTempParentId = CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOParentObjectId, lTemp))

                            If Not (CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOObjectName, lTemp)).ToLower().EndsWith("policy_binder")) Then

                                If (r_vGISObject(pbObjectAndPropertyConsts.ACOMaxInstances, lTemp)) > 1 Then
                                    result = 1
                                Else
                                    result = 0
                                End If
                            End If
                            Exit For
                        End If
                    Next
                End While
            End If

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".IsObjectMultipleInstance")

            Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: GetALookup
    '
    ' Description:
    '
    ' History: 15/08/2000 Tomo    - Created.
    '          27/09/2002 CMG-SJP - Moved from Business class of Maintain
    '                               Data model to common bas module so
    '                               that can shared with other projects
    '                               Eg Data Model Import Export Tool.
    '
    ' ***************************************************************** '
    Public Function GetALookup(ByRef iLanguageID As Integer, ByRef sTableName As String, ByRef vArray(,) As Object, ByRef r_oDatabase As dPMDAO.Database) As Integer 

        Dim result As Integer = 0 
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_oDatabase.Parameters.Clear()

            m_lReturn = r_oDatabase.Parameters.Add(sName:="table_name", vValue:=sTableName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMTableName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = r_oDatabase.Parameters.Add(sName:="effective_date", vValue:=DateTimeHelper.ToString(DateTime.Now), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = r_oDatabase.Parameters.Add(sName:="language_id", vValue:=CStr(1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = r_oDatabase.SQLSelect(sSQL:=ACGetLookupSQL, sSQLName:=ACGetLookupName, bStoredProcedure:=ACGetLookupStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetALookup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetALookup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetValidFieldName
    '
    ' Description:
    '
    ' History: 13/03/2001 Tomo    - Created.
    '          27/09/2002 CMG-SJP - Moved from Business class of Maintain
    '                               Data model to common bas module so
    '                               that can shared with other projects
    '                               Eg Data Model Import Export Tool.
    '
    ' ***************************************************************** '
    Private Function GetValidFieldName(ByRef sFieldName As String, ByRef r_oDatabase As dPMDAO.Database) As Integer 

        Dim result As Integer = 0 
        Dim sTemp As String = "" 
        Dim lTemp As Integer 
        Dim bOK As Boolean 
        Dim vArray(,) As Object 

        

            result = gPMConstants.PMEReturnCode.PMTrue

            bOK = False
            sTemp = sFieldName

            While Not bOK

                r_oDatabase.Parameters.Clear()

                m_lReturn = r_oDatabase.Parameters.Add(sName:="field_name", vValue:=sTemp, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = r_oDatabase.SQLSelect(sSQL:=ACSelectWPFieldsSQL, sSQLName:=ACSelectWPFieldsName, bStoredProcedure:=ACSelectWPFieldsStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Information.IsArray(vArray) Then
                    lTemp += 1
                    sTemp = sFieldName & CStr(lTemp)
                Else
                    bOK = True
                End If

            End While

            sFieldName = sTemp

            Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: CreateRegistrySettings
    '
    ' Description:
    '
    ' History: 06/02/2001 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function CreateRegistrySettings(ByVal v_sGISDataModel As String) As Integer

        Dim result As Integer = 0
        Dim sDefSubKey, sSubKey, sTemp As String
        Dim vRegistryKeys As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sDefSubKey = GISSharedConstants.ACOIMGISSubKey & "\" & "NB" & "\" & "DEF"

            sSubKey = GISSharedConstants.ACOIMGISSubKey & "\" & "NB" & "\" & v_sGISDataModel

            vRegistryKeys = New Object() {"RulePath", "SchemePath", "DictPath"}

            For Each sSettingItem As String In vRegistryKeys
                'Get the value
                m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=sSettingItem, r_sSettingValue:=sTemp, v_sSubKey:=sDefSubKey), gPMConstants.PMEReturnCode)

                'Set the value
                m_lReturn = CType(gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=sSettingItem, v_sSettingValue:=sTemp, v_sSubKey:=sSubKey), gPMConstants.PMEReturnCode)
            Next sSettingItem

            'set the data model specific root items
            'some of these are duplicated in NB/data model but.....
            sDefSubKey = GISSharedConstants.ACOIMGISSubKey & "\" & "DEF"
            sSubKey = GISSharedConstants.ACOIMGISSubKey & "\" & v_sGISDataModel

            vRegistryKeys = New Object() {GISSharedConstants.GISRegDataSetPath, "Insurers", GISSharedConstants.GISRegLookupPath, GISSharedConstants.GISQEMMethodsVersionNum, "RulePath", GISSharedConstants.GISRegBOMRequired}

            For Each sSettingItem As String In vRegistryKeys
                'Get the value
                m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=sSettingItem, r_sSettingValue:=sTemp, v_sSubKey:=sDefSubKey), gPMConstants.PMEReturnCode)

                'Set the value
                m_lReturn = CType(gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=sSettingItem, v_sSettingValue:=sTemp, v_sSubKey:=sSubKey), gPMConstants.PMEReturnCode)
            Next sSettingItem

            sDefSubKey = GISSharedConstants.ACOIMGISSubKey & "\" & "DEF" & "\" & "NB"

            sSubKey = GISSharedConstants.ACOIMGISSubKey & "\" & v_sGISDataModel & "\" & "NB"

            'Get save on quote
            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="SaveOnQuote", r_sSettingValue:=sTemp, v_sSubKey:=sDefSubKey), gPMConstants.PMEReturnCode)

            'Set save on quote
            m_lReturn = CType(gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="SaveOnQuote", v_sSettingValue:=sTemp, v_sSubKey:=sSubKey), gPMConstants.PMEReturnCode)

            sDefSubKey = GISSharedConstants.ACOIMGISSubKey & "\" & "DEF" & "\" & "ListManagement"

            sSubKey = GISSharedConstants.ACOIMGISSubKey & "\" & v_sGISDataModel & "\" & "ListManagement"

            vRegistryKeys = New Object() {"ServerListFileCompressed", "ServerListFilePath", "ServerListPrefVersion", "ServerListVersion"}

            For Each sSettingItem As String In vRegistryKeys
                'Get the value
                m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=sSettingItem, r_sSettingValue:=sTemp, v_sSubKey:=sDefSubKey), gPMConstants.PMEReturnCode)

                'Set the value
                m_lReturn = CType(gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=sSettingItem, v_sSettingValue:=sTemp, v_sSubKey:=sSubKey), gPMConstants.PMEReturnCode)
            Next sSettingItem

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateRegistrySettings Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateRegistrySettings", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: CopyDefaultGISLists
    '
    ' Description:
    '
    ' History: 01/10/2002 CLG - Created.
    '
    ' ***************************************************************** '
    Public Function CopyDefaultGISLists(ByVal v_sGISDataModel As String) As Integer

        Dim result As Integer = 0
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".CopyDefaultGISLists")

        Dim sOldFile, sNewFile As String
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sPath, sDefSubKey As String

            sDefSubKey = GISSharedConstants.ACOIMGISSubKey & "\" & "DEF" & "\" & "ListManagement"

            'And let's not forget to copy the default file...
            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="ServerListFilePath", r_sSettingValue:=sPath, v_sSubKey:=sDefSubKey), gPMConstants.PMEReturnCode)

            sPath = sPath.Trim()

            'MKW300603 PN3632 Start
            ' Fix to issue "Copying of default gis lists fails if registry
            ' setting is missing a backslash"
            If sPath.Length > 0 And Not sPath.EndsWith("\") Then
                sPath = sPath & "\"
            End If
            'MKW300603 PN3632 End

            sOldFile = sPath & "DEFList"
            sNewFile = sPath & v_sGISDataModel & "List"

            File.Copy(sOldFile & "0101.txt", sNewFile & "0101.txt")
            File.Copy(sOldFile & ".dat", sNewFile & ".dat")
            File.Copy(sOldFile & ".idx", sNewFile & ".idx")

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".CopyDefaultGISLists")

            Return result

        Catch excep As System.Exception

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".CopyDefaultGISLists")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyDefaultGISLists Failed (" & sOldFile & "0101.txt) to (" & sNewFile & "0101.txt)", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyDefaultGISLists", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ProcessPropertyArray
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 28-07-2003 : 223 document production
    '           Updated : MEvans : 10-11-2003 : CQ3049, CQ3143
    '               Added additonal parameters required by claims risk
    '                   processing
    ' ***************************************************************** '
    Private Function ProcessPropertyArray(ByVal v_lObjectType As Integer, ByVal v_sMainGroup As String, ByVal v_sProcedureName As String, ByVal v_lPMProductFamily As Integer, ByVal v_sDatamodel As String, ByVal v_sPrefix As String, ByRef r_vPropertyArray(,) As Object, ByRef r_sSelectList As String, ByRef r_sTableList As String, ByRef r_oDatabase As dPMDAO.Database, ByRef r_lParamCount As Integer, Optional ByVal v_sSubGroup As String = "", Optional ByVal v_sSubGroup1 As String = "", Optional ByVal v_sSubGroup2 As String = "", Optional ByVal v_sLoop1 As String = "", Optional ByVal v_sLoop2 As String = "", Optional ByVal v_sLoop3 As String = "", Optional ByRef r_sWhereList As String = "", Optional ByRef r_iInstanceCount As Integer = 0, Optional ByVal v_lLevel As Integer = 0) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "ProcessPropertyArray"

        Dim llBound, lUbound, lAddressCount, lOtherTableCount As Integer
        Dim vWPFields(,) As Object

        Dim sTemp As String = ""
        Dim lTemp3 As Integer
        Dim sFieldName, sDisplayname, sSubGroup, sSubGroup2, sSubGroup3 As String
        Dim sSubGroupTemp As New StringBuilder
        Dim lLevel As Integer
        Dim iParentIsMultipleInstance As Integer
        Dim sLoop1, sLoop2, sLoop3 As String
        Dim sWhereList2 As New StringBuilder
        Dim sWhereList1 As New StringBuilder
        Dim iInstanceCount As Integer
        Dim sColumnName As String = ""

        

            result = gPMConstants.PMEReturnCode.PMTrue

            '***************
            ' Initialisation
            '***************

            '********
            ' MEvans : 10-11-2003 : CQ3040
            If v_lLevel > 1 Then
                lLevel = 0
            Else
                lLevel = v_lLevel
            End If
            '********

            lAddressCount = 0
            lOtherTableCount = 0
            sSubGroup = ""
            sSubGroup2 = ""
            sSubGroup3 = ""
            sLoop1 = ""
            sLoop2 = ""
            sLoop3 = ""

            '*****************
            ' If there are any properties to be added
            '*****************
            If Information.IsArray(r_vPropertyArray) Then

                llBound = r_vPropertyArray.GetLowerBound(1)
                lUbound = r_vPropertyArray.GetUpperBound(1)

                '*****************
                ' for each object property
                '*****************
                For lProperty As Integer = llBound To lUbound

                    sColumnName = CStr(r_vPropertyArray(pbObjectAndPropertyConsts.ACPColumnName, lProperty)).Trim()
                    sFieldName = sColumnName.Replace("_", "").Trim()
                    sDisplayname = sColumnName.Replace("_"c, " "c).Trim()

                    '*****************
                    ' if the property is an identity column / key
                    '*****************

                    If (r_vPropertyArray(pbObjectAndPropertyConsts.ACPIsIdentifyingProperty, lProperty)) = 1 Then

                        'Move the determination of the group structure to here - only
                        'do it once
                        sSubGroupTemp = New StringBuilder(v_sDatamodel)

                        sTemp = StripDataModelCode(v_vTheString:=CStr(r_vPropertyArray(pbObjectAndPropertyConsts.ACPColumnName, lProperty)), v_sDatamodel:=v_sDatamodel).Trim()

                        'Remove the trailing _id
                        sTemp = sTemp.Substring(0, sTemp.Length - 3)

                        While sTemp.IndexOf("_"c) >= 0
                            lTemp3 = (sTemp.IndexOf("_"c) + 1)
                            sSubGroupTemp.Append(Strings.StrConv(sTemp.Substring(0, lTemp3 - 1), VbStrConv.ProperCase))
                            sTemp = sTemp.Substring(lTemp3)
                        End While

                        Select Case lProperty
                            Case 0

                            Case 1
                                sSubGroup = sSubGroupTemp.ToString() & Strings.StrConv(sTemp, VbStrConv.ProperCase)
                                'find if parent was actually a looping object
                                If lLevel = 0 Then
                                    If iParentIsMultipleInstance = 1 And lLevel = 0 Then
                                        sLoop1 = sSubGroup
                                    End If
                                End If

                            Case 2
                                sSubGroup2 = sSubGroupTemp.ToString() & Strings.StrConv(sTemp, VbStrConv.ProperCase)
                                If lLevel = 0 Then
                                    If iParentIsMultipleInstance = 1 Then
                                        sLoop2 = sSubGroup2
                                    Else
                                        sLoop1 = sSubGroup2
                                    End If
                                End If

                            Case 3
                                sSubGroup3 = sSubGroupTemp.ToString() & Strings.StrConv(sTemp, VbStrConv.ProperCase)
                                'ED 30092002 - sLoop3 set t sSubGroup3 if parent is multi instance
                                'orignally overwriting sLoop2 with the new sub value.
                                If lLevel = 0 Then
                                    If iParentIsMultipleInstance = 1 Then
                                        sLoop3 = sSubGroup3
                                    Else
                                        sLoop2 = sSubGroup3
                                    End If
                                End If

                        End Select

                        If lProperty > lLevel Then

                            sWhereList1.Append("AND " & v_sPrefix & "." & CStr(r_vPropertyArray(pbObjectAndPropertyConsts.ACPColumnName, lProperty)) & " = @Instance" & CStr(lProperty + iParentIsMultipleInstance) & Strings.Chr(13) & Strings.Chr(10))

                            sWhereList2.Append("AND " & v_sPrefix & "." & CStr(r_vPropertyArray(pbObjectAndPropertyConsts.ACPColumnName, lProperty)) & " = @Instance" & CStr(lProperty + iParentIsMultipleInstance) & Strings.Chr(13) & Strings.Chr(10))
                            iInstanceCount = lProperty + iParentIsMultipleInstance
                        End If

                    Else

                        '*****************
                        ' if the property is not an identity / key
                        ' and is not deleted
                        '*****************

                        If CStr(r_vPropertyArray(pbObjectAndPropertyConsts.ACPPropertyName, lProperty)) <> "dElEtEd" Then

                            '*****************
                            ' if we have a special type of field
                            ' like address / party / lookup / gis table link / etc
                            '*****************
                            ' if the property already exists in another non-gis related table then

                            If CBool(r_vPropertyArray(pbObjectAndPropertyConsts.ACPEditFlags, lProperty)) And GISSharedPropertyConstants.GISDSEditNoDBColumn Then

                                ' do nothing because these fields
                                ' will already have there own entries in the
                                ' word processing fields so we dont want to
                                ' duplicate them

                            Else

                                'Special stuff to do here.

                                'If it's an address, need 6 lines of address and links to the address and country tables

                                If CStr(r_vPropertyArray(pbObjectAndPropertyConsts.ACPColumnName, lProperty)).ToUpper().StartsWith("ADDRESS_CNT") Then

                                    AddAddressProperty(v_vPropertyArray:=r_vPropertyArray, v_lItem:=lProperty, v_sPrefix:=v_sPrefix, r_sSelectList:=r_sSelectList, r_sTableList:=r_sTableList, r_lAddressCount:=lAddressCount, r_vWPFields:=vWPFields)

                                    '*****************
                                    ' else if its a party id then
                                    ' add additional party fields to the stored procedure
                                    '*****************

                                ElseIf (r_vPropertyArray(pbObjectAndPropertyConsts.ACPSpecialsType, lProperty)) = GISSharedPropertyConstants.ACOPartyTypeID Then
                                    AddPartyProperty(v_vPropertyArray:=r_vPropertyArray, v_lItem:=lProperty, v_sPrefix:=v_sPrefix, r_sSelectList:=r_sSelectList, r_sTableList:=r_sTableList, r_lOtherTableCount:=lOtherTableCount, r_vWPFields:=vWPFields)

                                    '*****************
                                    ' else if its a lookup then
                                    ' add additional lookup params to the stored procedure
                                    '*****************

                                ElseIf (r_vPropertyArray(pbObjectAndPropertyConsts.ACPSpecialsType, lProperty)) = GISSharedPropertyConstants.ACOPMLookupTableName Then
                                    AddLookupProperty(v_vPropertyArray:=r_vPropertyArray, v_lItem:=lProperty, v_sPrefix:=v_sPrefix, r_sSelectList:=r_sSelectList, r_sTableList:=r_sTableList, r_lOtherTableCount:=lOtherTableCount, r_vWPFields:=vWPFields)

                                    '*****************
                                    ' if its a link to another
                                    ' gis table
                                    '*****************

                                ElseIf (r_vPropertyArray(pbObjectAndPropertyConsts.ACPSpecialsType, lProperty)) = GISSharedPropertyConstants.ACOGISUserDefHeaderID Then
                                    AddGisUserDefinedHeaderProperty(v_vPropertyArray:=r_vPropertyArray, v_lItem:=lProperty, v_sPrefix:=v_sPrefix, r_sSelectList:=r_sSelectList, r_sTableList:=r_sTableList, r_lOtherTableCount:=lOtherTableCount, r_vWPFields:=vWPFields)

                                    '***********************
                                    '  if its a product then link
                                    ' in the product fields
                                    '***********************

                                ElseIf (r_vPropertyArray(pbObjectAndPropertyConsts.ACPSpecialsType, lProperty)) = GISSharedPropertyConstants.ACOProductID Then
                                    AddProductProperty(v_vPropertyArray:=r_vPropertyArray, v_lItem:=lProperty, v_sPrefix:=v_sPrefix, r_sSelectList:=r_sSelectList, r_sTableList:=r_sTableList, r_lOtherTableCount:=lOtherTableCount, r_vWPFields:=vWPFields)

                                    '***********************
                                    ' else it's just a normal property
                                    '***********************
                                Else
                                    AddGeneralProperty(v_vPropertyArray:=r_vPropertyArray, v_lItem:=lProperty, v_sPrefix:=v_sPrefix, r_sSelectList:=r_sSelectList, r_sTableList:=r_sTableList, r_vWPFields:=vWPFields)

                                End If

                            End If

                        End If

                    End If

                Next

            End If

            '************************
            ' default the loops if they dont follow the existing
            ' standards for keys - ie. for claims \ claim perils
            ' based on claim id not a group of keys
            If sLoop1 = "" Then
                If v_sLoop1 <> "" Then
                    sLoop1 = v_sLoop1
                End If
            End If

            If sLoop2 = "" Then
                If v_sLoop2 <> "" Then
                    sLoop2 = v_sLoop2
                End If
            End If

            If sLoop3 = "" Then
                If v_sLoop3 <> "" Then
                    sLoop3 = v_sLoop3
                End If
            End If
            '************************

            '********
            ' MEvans : 10-11-2003 : CQ3040
            ' if not passed use internal references
            If v_sSubGroup = "" Then
                v_sSubGroup = sSubGroup
            End If

            If v_sSubGroup1 = "" Then
                v_sSubGroup1 = sSubGroup2
            End If

            If v_sSubGroup2 = "" Then
                v_sSubGroup2 = sSubGroup3
            End If
            '********

            ' if we have any fields to add to the wp_fields table
            If Information.IsArray(vWPFields) Then

                ' get array boundaries

                llBound = vWPFields.GetLowerBound(1)

                lUbound = vWPFields.GetUpperBound(1)

                r_lParamCount = lUbound + 1

                ' for each field
                For lWPField As Integer = llBound To lUbound

                    ' add an entry in the wp fields table

                    If AddToWPFields(sFieldName:=CStr(vWPFields(0, lWPField)), sSQL:=v_sProcedureName, sColumnName:=CStr(vWPFields(1, lWPField)), lColumnType:=CInt(vWPFields(2, lWPField)), sMainGroup:=v_sMainGroup, sSubGroup:=v_sSubGroup, sSubGroup2:=v_sSubGroup1, sSubGroup3:=v_sSubGroup2, sSubGroup4:="", sDisplayname:=CStr(vWPFields(3, lWPField)), iIsDisplayed:=1, sLoop1:=sLoop1, sLoop2:=sLoop2, sLoop3:=sLoop3, sLoop4:="", lProductFamily:=v_lPMProductFamily, vDataModel:=DBNull.Value, vPropertyId:=DBNull.Value, r_oDatabase:=r_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then

                        result = gPMConstants.PMEReturnCode.PMFalse
                        Exit For

                    End If

                Next

            End If

            '********
            ' MEvans : 08-11-2003 : CQ3049
            r_sWhereList = sWhereList1.ToString()
            r_iInstanceCount = iInstanceCount
            '********

            Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddAddressProperty
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 25-07-2003 : Process ID
    ' ***************************************************************** '
    Private Function AddAddressProperty(ByVal v_vPropertyArray(,) As Object, ByVal v_lItem As Integer, ByVal v_sPrefix As String, ByRef r_sSelectList As String, ByRef r_sTableList As String, ByRef r_lAddressCount As Integer, ByRef r_vWPFields As Object) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "AddAddressProperty"

        Dim sColumnName, sPropertyName As String

        

            result = gPMConstants.PMEReturnCode.PMTrue

            '**************
            ' Increment the address counter
            ' to get unique address fields and link table name
            '**************
            r_lAddressCount += 1

            '*****************
            ' Build property and table strings
            '*****************

            ' get array values

            sColumnName = CStr(v_vPropertyArray(pbObjectAndPropertyConsts.ACPColumnName, v_lItem)).Trim()

            sPropertyName = CStr(v_vPropertyArray(pbObjectAndPropertyConsts.ACPPropertyName, v_lItem)).Trim()

            r_sSelectList = r_sSelectList & _
                            "address" & CStr(r_lAddressCount) & "_line_1 = a" & CStr(r_lAddressCount) & ".address1," & Strings.Chr(13) & Strings.Chr(10) & _
                            "address" & CStr(r_lAddressCount) & "_line_2 = a" & CStr(r_lAddressCount) & ".address2," & Strings.Chr(13) & Strings.Chr(10) & _
                            "address" & CStr(r_lAddressCount) & "_line_3 = a" & CStr(r_lAddressCount) & ".address3," & Strings.Chr(13) & Strings.Chr(10) & _
                            "address" & CStr(r_lAddressCount) & "_line_4 = a" & CStr(r_lAddressCount) & ".address4," & Strings.Chr(13) & Strings.Chr(10) & _
                            "case a" & CStr(r_lAddressCount) & ".postal_code when convert(varchar(10), a" & CStr(r_lAddressCount) & ".address_id) then '' else a" & CStr(r_lAddressCount) & ".postal_code end as address" & CStr(r_lAddressCount) & "_postal_code," & Strings.Chr(13) & Strings.Chr(10) & _
                            "address" & CStr(r_lAddressCount) & "_country = c" & CStr(r_lAddressCount) & ".description," & Strings.Chr(13) & Strings.Chr(10)

            r_sTableList = r_sTableList & "LEFT JOIN address a" & CStr(r_lAddressCount) & " ON " & _
                           v_sPrefix & "." & sColumnName & " = a" & CStr(r_lAddressCount) & ".address_cnt" & Strings.Chr(13) & Strings.Chr(10)

            r_sTableList = r_sTableList & "LEFT JOIN country c" & CStr(r_lAddressCount) & " ON " & _
                           "a" & CStr(r_lAddressCount) & ".country_id = c" & CStr(r_lAddressCount) & ".country_id" & Strings.Chr(13) & Strings.Chr(10)

            '**************
            ' WP Field Address Lines
            '**************
            For lAddressLine As Integer = 1 To 4

                If AddPropertyToWPFieldsArray(r_vWPFieldArray:=r_vWPFields, v_sFieldName:=v_sPrefix & "Address" & CStr(r_lAddressCount) & "Line" & CStr(lAddressLine), v_sColumnName:="address" & r_lAddressCount & "_line_" & CStr(lAddressLine), v_lDataType:=gPMConstants.PMEFormatStyle.PMFormatString, v_sDisplayName:=sPropertyName & " Line " & CStr(lAddressLine)) <> gPMConstants.PMEReturnCode.PMTrue Then

                    result = gPMConstants.PMEReturnCode.PMFalse
                    Exit For
                End If
            Next

            '**************
            ' WP Field Postal Code
            '**************

            If AddPropertyToWPFieldsArray(r_vWPFieldArray:=r_vWPFields, v_sFieldName:=v_sPrefix & "Address" & CStr(r_lAddressCount) & "PostalCode", v_sColumnName:="address" & r_lAddressCount & "_postal_code", v_lDataType:=gPMConstants.PMEFormatStyle.PMFormatString, v_sDisplayName:=sPropertyName & " Postal Code") <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

            End If

            '**************
            ' WP Field Country
            '**************

            If AddPropertyToWPFieldsArray(r_vWPFieldArray:=r_vWPFields, v_sFieldName:=v_sPrefix & "Address" & CStr(r_lAddressCount) & "Country", v_sColumnName:="address" & r_lAddressCount & "_country", v_lDataType:=gPMConstants.PMEFormatStyle.PMFormatString, v_sDisplayName:=sPropertyName & " Country") <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddPartyProperty
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 25-07-2003 : 223 document production
    ' ***************************************************************** '
    Private Function AddPartyProperty(ByVal v_vPropertyArray(,) As Object, ByVal v_lItem As Integer, ByVal v_sPrefix As String, ByRef r_sSelectList As String, ByRef r_sTableList As String, ByRef r_lOtherTableCount As Integer, ByRef r_vWPFields As Object) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "AddPartyProperty"

        Dim sFieldName, sDisplayname, sColumnName As String

        

            result = gPMConstants.PMEReturnCode.PMTrue

            '**************
            ' Increment the Other Table Count
            ' to get unique fields and link table name
            '**************
            r_lOtherTableCount += 1

            '*****************
            ' Generate the property strings
            '*****************

            sColumnName = CStr(v_vPropertyArray(pbObjectAndPropertyConsts.ACPColumnName, v_lItem)).Trim()
            sFieldName = sColumnName.Replace("_", "")
            sDisplayname = sColumnName.Replace("_"c, " "c)

            '*****************
            ' Build property and table strings
            '*****************
            r_sSelectList = r_sSelectList & _
                            sColumnName & " = p" & CStr(r_lOtherTableCount) & ".name," & Strings.Chr(13) & Strings.Chr(10)

            r_sTableList = r_sTableList & "LEFT JOIN party p" & CStr(r_lOtherTableCount) & " ON " & _
                           v_sPrefix & "." & sColumnName & " = p" & CStr(r_lOtherTableCount) & ".party_cnt" & Strings.Chr(13) & Strings.Chr(10)

            '*****************
            ' add property to the wp field array
            '*****************

            If AddPropertyToWPFieldsArray(r_vWPFieldArray:=r_vWPFields, v_sFieldName:=v_sPrefix & sFieldName, v_sColumnName:=sColumnName, v_lDataType:=gPMConstants.PMEFormatStyle.PMFormatString, v_sDisplayName:=sDisplayname) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddLookupProperty
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 28/07/2003 : 223 document production
    ' ***************************************************************** '
    Private Function AddLookupProperty(ByVal v_vPropertyArray(,) As Object, ByVal v_lItem As Integer, ByVal v_sPrefix As String, ByRef r_sSelectList As String, ByRef r_sTableList As String, ByRef r_lOtherTableCount As Integer, ByRef r_vWPFields As Object) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "AddLookupProperty"

        Dim sReference, sFieldName, sDisplayname, sColumnName As String

        

            result = gPMConstants.PMEReturnCode.PMTrue

            '**************
            ' Increment the Other Table Count
            ' to get unique fields and link table name
            '**************
            r_lOtherTableCount += 1

            '*****************
            ' Generate the property strings
            '*****************

            sColumnName = CStr(v_vPropertyArray(pbObjectAndPropertyConsts.ACPColumnName, v_lItem)).Trim()
            sFieldName = sColumnName.Replace("_", "")
            sDisplayname = sColumnName.Replace("_"c, " "c)

            sReference = CStr(v_vPropertyArray(pbObjectAndPropertyConsts.ACPSpecialsTypeReference, v_lItem)).Trim()

            '*****************
            ' Build property and table strings
            '*****************

            r_sSelectList = r_sSelectList & _
                            sColumnName & " = l" & CStr(r_lOtherTableCount) & ".description," & Strings.Chr(13) & Strings.Chr(10)

            r_sTableList = r_sTableList & "LEFT JOIN " & sReference & " l" & CStr(r_lOtherTableCount) & " ON " & _
                           v_sPrefix & "." & sColumnName & " = " & _
                           "l" & CStr(r_lOtherTableCount) & "." & sReference & "_id" & Strings.Chr(13) & Strings.Chr(10)

            '*****************
            ' add property to the wp field array
            '*****************

            If AddPropertyToWPFieldsArray(r_vWPFieldArray:=r_vWPFields, v_sFieldName:=v_sPrefix & sFieldName, v_sColumnName:=sColumnName, v_lDataType:=gPMConstants.PMEFormatStyle.PMFormatString, v_sDisplayName:=sDisplayname) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

            End If

            Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddGisUserDefinedHeaderProperty
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 28-07-2003 : 223 document production
    ' ***************************************************************** '
    Private Function AddGisUserDefinedHeaderProperty(ByVal v_vPropertyArray(,) As Object, ByVal v_lItem As Integer, ByVal v_sPrefix As String, ByRef r_sSelectList As String, ByRef r_sTableList As String, ByRef r_lOtherTableCount As Integer, ByRef r_vWPFields As Object) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "AddGisUserDefinedHeaderProperty"

        Dim sColumnName, sFieldName, sDisplayname, sReference As String

        

            result = gPMConstants.PMEReturnCode.PMTrue

            '**************
            ' Increment the Other Table Count
            ' to get unique fields and link table name
            '**************
            r_lOtherTableCount += 1

            '*****************
            ' Generate the property strings
            '*****************

            sColumnName = CStr(v_vPropertyArray(pbObjectAndPropertyConsts.ACPColumnName, v_lItem)).Trim()
            sFieldName = sColumnName.Replace("_", "")
            sDisplayname = sColumnName.Replace("_"c, " "c)

            sReference = CStr(v_vPropertyArray(pbObjectAndPropertyConsts.ACPSpecialsTypeReference, v_lItem)).Trim()

            '*****************
            ' Build property and table strings
            '*****************
            r_sSelectList = r_sSelectList & _
                            sColumnName & " = l" & CStr(r_lOtherTableCount) & ".description," & Strings.Chr(13) & Strings.Chr(10)

            r_sTableList = r_sTableList & "LEFT JOIN GIS_user_def_detail l" & CStr(r_lOtherTableCount) & " ON " & _
                           v_sPrefix & "." & sColumnName & " = " & _
                           "l" & CStr(r_lOtherTableCount) & ".GIS_user_def_detail_id" & Strings.Chr(13) & Strings.Chr(10)

            '*****************
            ' add property to the wp field array
            '*****************

            If AddPropertyToWPFieldsArray(r_vWPFieldArray:=r_vWPFields, v_sFieldName:=v_sPrefix & sFieldName, v_sColumnName:=sColumnName, v_lDataType:=gPMConstants.PMEFormatStyle.PMFormatString, v_sDisplayName:=sDisplayname) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

            End If

            Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddProductProperty
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 28/07/2003 : 223 document production
    ' ***************************************************************** '
    Private Function AddProductProperty(ByVal v_vPropertyArray(,) As Object, ByVal v_lItem As Integer, ByVal v_sPrefix As String, ByRef r_sSelectList As String, ByRef r_sTableList As String, ByRef r_lOtherTableCount As Integer, ByRef r_vWPFields As Object) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "AddProductProperty"

        Dim sFieldName, sDisplayname, sColumnName, sReference As String

        

            result = gPMConstants.PMEReturnCode.PMTrue

            '**************
            ' Increment the Other Table Count
            ' to get unique fields and link table name
            '**************
            r_lOtherTableCount += 1

            '*****************
            ' Generate the property strings
            '*****************

            sColumnName = CStr(v_vPropertyArray(pbObjectAndPropertyConsts.ACPColumnName, v_lItem)).Trim()
            sFieldName = sColumnName.Replace("_", "")
            sDisplayname = sColumnName.Replace("_"c, " "c)

            sReference = CStr(v_vPropertyArray(pbObjectAndPropertyConsts.ACPSpecialsTypeReference, v_lItem)).Trim()

            '*****************
            ' Build property and table strings
            '*****************
            r_sSelectList = r_sSelectList & _
                            sColumnName & " = i" & CStr(r_lOtherTableCount) & ".insurance_ref," & Strings.Chr(13) & Strings.Chr(10)

            r_sTableList = r_sTableList & "LEFT JOIN insurance_file i" & CStr(r_lOtherTableCount) & " ON " & _
                           v_sPrefix & "." & sColumnName & " = i" & CStr(r_lOtherTableCount) & ".insurance_file_cnt" & Strings.Chr(13) & Strings.Chr(10)

            '*****************
            ' add property to the wp field array
            '*****************

            If AddPropertyToWPFieldsArray(r_vWPFieldArray:=r_vWPFields, v_sFieldName:=v_sPrefix & sFieldName, v_sColumnName:=sColumnName, v_lDataType:=gPMConstants.PMEFormatStyle.PMFormatString, v_sDisplayName:=sDisplayname) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddGeneralProperty
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 28/07/2003 : 223 document production
    ' ***************************************************************** '
    Private Function AddGeneralProperty(ByVal v_vPropertyArray(,) As Object, ByVal v_lItem As Integer, ByVal v_sPrefix As String, ByRef r_sSelectList As String, ByRef r_sTableList As String, ByRef r_vWPFields As Object) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "AddGeneralProperty"

        Dim sDataType, sColumnName As String
        Dim lFormat As gPMConstants.PMEFormatStyle
        Dim sFieldName, sDisplayname As String

        

            result = gPMConstants.PMEReturnCode.PMTrue

            '*****************
            ' Generate the property strings
            '*****************

            sColumnName = CStr(v_vPropertyArray(pbObjectAndPropertyConsts.ACPColumnName, v_lItem)).Trim()
            sFieldName = sColumnName.Replace("_", "")
            sDisplayname = sColumnName.Replace("_"c, " "c)

            sDataType = CStr(v_vPropertyArray(pbObjectAndPropertyConsts.ACPDataType, v_lItem)).Trim()

            r_sSelectList = r_sSelectList & _
                            sColumnName & " = " & v_sPrefix & "." & sColumnName & "," & Strings.Chr(13) & Strings.Chr(10)

            '****************************
            ' determine property datatype
            '****************************
            Select Case sDataType
                Case CStr(GISSharedConstants.GISDataTypeDate)
                    lFormat = gPMConstants.PMEFormatStyle.PMFormatDateLong
                Case CStr(GISSharedConstants.GISDataTypeCurrency)
                    lFormat = gPMConstants.PMEFormatStyle.PMFormatCurrency
                Case CStr(GISSharedConstants.GISDataTypePercentage)
                    lFormat = gPMConstants.PMEFormatStyle.PMFormatPercent
                Case CStr(GISSharedConstants.GISDataTypeNumeric)
                    lFormat = gPMConstants.PMEFormatStyle.PMFormatLong
                Case CStr(GISSharedConstants.GISDataTypeOption)
                    lFormat = gPMConstants.PMEFormatStyle.PMFormatBoolean
                Case CStr(GISSharedConstants.GISDataTypeComment), CStr(GISSharedConstants.GISDataTypeText)
                    lFormat = gPMConstants.PMEFormatStyle.PMFormatString
                    'SJ 15/07/2004 - start
                Case CStr(GISSharedConstants.GISDataTypeInteger)
                    lFormat = gPMConstants.PMEFormatStyle.PMFormatLong
                    'SJ 15/07/2004 - end
                Case Else
                    lFormat = gPMConstants.PMEFormatStyle.PMFormatString
            End Select

            '*****************
            ' add property to the wp field array
            '*****************

            If AddPropertyToWPFieldsArray(r_vWPFieldArray:=r_vWPFields, v_sFieldName:=v_sPrefix & sFieldName, v_sColumnName:=sColumnName, v_lDataType:=lFormat, v_sDisplayName:=sDisplayname) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

            End If

            Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddPropertyToWPFieldsArray
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 28/07/2003 : 223 document production
    ' ***************************************************************** '
    Private Function AddPropertyToWPFieldsArray(ByRef r_vWPFieldArray(,) As Object, ByVal v_sFieldName As String, ByVal v_sColumnName As String, ByVal v_lDataType As Integer, ByVal v_sDisplayName As String) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "AddPropertyToWPFieldsArray"

        Dim lWPFieldLastPos, lArrayPos As Integer

        

            result = gPMConstants.PMEReturnCode.PMTrue

            '**************
            ' Resize Array
            '**************
            If Information.IsArray(r_vWPFieldArray) Then
                lWPFieldLastPos = r_vWPFieldArray.GetUpperBound(1)
                ReDim Preserve r_vWPFieldArray(3, (lWPFieldLastPos + 1))
            Else
                r_vWPFieldArray = Array.CreateInstance(GetType(Object), New Integer() {ACWPFDisplayName - ACWPFFieldName + 1, 1}, New Integer() {ACWPFFieldName, 0})
                lWPFieldLastPos = -1
            End If

            '**************
            ' WP Field Address Lines
            '**************
            lArrayPos = lWPFieldLastPos + 1

            r_vWPFieldArray(ACWPFFieldName, lArrayPos) = v_sFieldName

            r_vWPFieldArray(ACWPFColumnName, lArrayPos) = v_sColumnName

            r_vWPFieldArray(ACWPFColumnType, lArrayPos) = v_lDataType

            r_vWPFieldArray(ACWPFDisplayName, lArrayPos) = v_sDisplayName

            Return result

    End Function

    ' ***************************************************************** '
    ' Name: GenerateClaimPerilGetKeysStoredProcedure
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 29-07-2003 : 223 document production
    ' ***************************************************************** '
    Private Function GenerateClaimPerilGetKeysStoredProcedure(ByVal sTableName As String, ByVal sProcedureName As String, ByVal sProcedureNameOldStyle As String, ByRef r_oDatabase As dPMDAO.Database) As Integer

        Dim result As Integer = 0
        Dim sSelectList, sSQL As String

        

            result = gPMConstants.PMEReturnCode.PMTrue

            '******************
            'Drop any existing procedures
            '   - do for both old and new
            '       stored proc name formats
            '******************

            ' for some bizarre reason you have to set the return value to
            ' true for the drop procedures to do anything
            m_lReturn = gPMConstants.PMEReturnCode.PMTrue

            DropExistingProcedure(v_sProcedureName:=sProcedureName, v_sPostFix:="_get_keys", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

            ' for some bizarre reason you have to set the return value to
            ' true for the drop procedures to do anything
            m_lReturn = gPMConstants.PMEReturnCode.PMTrue

            DropExistingProcedure(v_sProcedureName:=sProcedureNameOldStyle, v_sPostFix:="_get_keys", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '***********************
            'Create the new procedure
            '***********************
            sSQL = ""

            sSQL = sSQL & "CREATE PROCEDURE " & sProcedureName & "_get_keys" & Strings.Chr(13) & Strings.Chr(10) & _
                   "@PartyCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@InsuranceFileCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@RiskId INT = NULL," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@ClaimCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@DocumentRef VARCHAR(25)," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@Instance1 INT = NULL," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@Instance2 INT = NULL," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@Instance3 INT = NULL," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@ClaimPerilId INT = NULL" & Strings.Chr(13) & Strings.Chr(10) & _
                   "AS"

            sSelectList = "If @ClaimPerilId IS NULL" & Strings.Chr(13) & Strings.Chr(10) & _
                          "SELECT Claim_Peril_Id from " & sTableName & _
                          " WHERE Claim_Id = @ClaimCnt" & Strings.Chr(13) & Strings.Chr(10) & _
                          "Else " & Strings.Chr(13) & Strings.Chr(10) & _
                          "SELECT Claim_Peril_Id from " & sTableName & _
                          " WHERE Claim_Peril_Id = @ClaimPerilId"

            ' combine sql statements to fully generate the procedure script
            sSQL = sSQL & Strings.Chr(13) & Strings.Chr(10) & sSelectList & Strings.Chr(13) & Strings.Chr(10)

            ' create the stored procedure on the server
            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & sProcedureName & "_get_keys StoredProcedure", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set permissions
            sSQL = "GRANT EXECUTE ON dbo." & sProcedureName & "_get_keys TO PUBLIC" & Strings.Chr(13) & Strings.Chr(10)

            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Grant " & sProcedureName & "_get_keys StoredProcedure", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

    End Function

    ' ***************************************************************** '
    ' Name: GenerateClaimPerilGetKeysStoredProcedure
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 29-07-2003 : 223 document production
    ' ***************************************************************** '
    Private Function GenerateClaimPerilGetParentStoredProcedure(ByVal sProcedureName As String, ByVal sProcedureNameOldStyle As String, ByRef r_oDatabase As dPMDAO.Database) As Integer

        Dim result As Integer = 0
        Dim sSelectList, sSQL As String

        

            result = gPMConstants.PMEReturnCode.PMTrue

            ' for some bizarre reason you have to set the return value to
            ' true for the drop procedures to do anything
            m_lReturn = gPMConstants.PMEReturnCode.PMTrue

            'Drop it if it's already there
            DropExistingProcedure(v_sProcedureName:=sProcedureName, v_sPostFix:="_get_parent_key", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

            ' for some bizarre reason you have to set the return value to
            ' true for the drop procedures to do anything
            m_lReturn = gPMConstants.PMEReturnCode.PMTrue

            DropExistingProcedure(v_sProcedureName:=sProcedureNameOldStyle, v_sPostFix:="_get_parent_key", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Create the new procedure
            sSQL = ""

            sSQL = sSQL & "CREATE PROCEDURE " & sProcedureName & "_get_parent_key @PartyCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@InsuranceFileCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@RiskId INT = NULL," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@ClaimCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@DocumentRef VARCHAR(25)," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@Instance1 INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@Instance2 INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@Instance3 INT" & Strings.Chr(13) & Strings.Chr(10) & _
                   "AS" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10)

            'First we do the main query
            sSelectList = "SELECT @ClaimCnt"

            sSQL = sSQL & sSelectList & Strings.Chr(13) & Strings.Chr(10)

            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & sProcedureName & "_get_parent_key StoredProcedure", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set permissions

            sSQL = "GRANT EXECUTE ON dbo." & sProcedureName & "_get_parent_key TO PUBLIC" & Strings.Chr(13) & Strings.Chr(10)

            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Grant " & sProcedureName & "_get_parent_key StoredProcedure", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

    End Function

    ' ***************************************************************** '
    ' Name: GenerateClaimRiskGetKeysSP
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 08-11-2003 : CQ3049
    ' ***************************************************************** '
    Private Function GenerateClaimRiskGetKeysSP(ByVal v_sTableName As String, ByVal v_sPrefix As String, ByVal v_sDatamodel As String, ByVal v_sProcedureName As String, ByVal v_sProcedureNameOldStyle As String, ByVal v_vArray(,) As Object, ByVal v_iIsParentMultipleInstance As Integer, ByRef r_oDatabase As dPMDAO.Database) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GenerateClaimRiskGetKeysSP"

        Dim sSelectList, sTableList As String
        Dim sWhereList As New StringBuilder
        Dim lLevel As Integer
        Dim sSQL As String = ""

        Dim llBound, lUbound As Integer

        

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Drop Existing Stored Procedure if it is there
            ' Use both old and new formats to ensure old procedure removed
            ' ignore return values as if the  procedures dont exist it errors

            ' for some bizarre reason you have to set the return value to
            ' true for the drop procedures to do anything
            m_lReturn = gPMConstants.PMEReturnCode.PMTrue

            DropExistingProcedure(v_sProcedureName:=v_sProcedureName, v_sPostFix:="_get_keys", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

            ' for some bizarre reason you have to set the return value to
            ' true for the drop procedures to do anything
            m_lReturn = gPMConstants.PMEReturnCode.PMTrue

            DropExistingProcedure(v_sProcedureName:=v_sProcedureNameOldStyle, v_sPostFix:="_get_keys", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

            'Build up the template for the query
            sTableList = "FROM XXXDATAMODELTABLENAME as XXXDATAMODELTABLEALIAS"

            sWhereList = New StringBuilder("WHERE XXXDATAMODELNAME_policy_binder_id in (" & _
                         " SELECT XXXDATAMODELNAME_policy_binder_id from " & _
                         " XXXDATAMODELNAME_Policy_Binder where gis_policy_link_id in ( " & _
                         " Select gis_policy_link_id from gis_Policy_Link " & _
                         "Where Claim_Id =@ClaimCnt))")

            'Initialise id key level indicator
            lLevel = 0

            ' get the position of the last identifying key in the array
            ' the unique key of the current table
            If Information.IsArray(v_vArray) Then

                llBound = v_vArray.GetLowerBound(1)
                lUbound = v_vArray.GetUpperBound(1)

                For lIDKeyPos As Integer = llBound To lUbound

                    If (v_vArray(pbObjectAndPropertyConsts.ACPIsIdentifyingProperty, lIDKeyPos)) = 1 Then
                        lLevel = lIDKeyPos
                    End If
                Next lIDKeyPos

                ' add the local level key to the select list and any other keys to the
                ' where clause.
                ' start from lbound + 1 to exclude the initial policy binder id as this is already
                ' added to the initial "where" clause
                For lIDKeyPos As Integer = llBound + 1 To lUbound

                    ' if item is id key

                    If (v_vArray(pbObjectAndPropertyConsts.ACPIsIdentifyingProperty, lIDKeyPos)) = 1 Then

                        ' if its not the local table's unique id key
                        If lIDKeyPos < lLevel Then
                            ' add it to the where list

                            sWhereList.Append("AND XXXDATAMODELTABLEALIAS." & CStr(v_vArray(pbObjectAndPropertyConsts.ACPColumnName, lIDKeyPos)) & " = @Instance" & CStr(lIDKeyPos + v_iIsParentMultipleInstance) & Strings.Chr(13) & Strings.Chr(10))
                        Else
                            ' else add the local tables unique id key to the select list
                            ' as this is what we want to return

                            sSelectList = "SELECT XXXDATAMODELTABLEALIAS." & CStr(v_vArray(pbObjectAndPropertyConsts.ACPColumnName, lIDKeyPos))
                        End If
                    End If

                Next lIDKeyPos

            End If

            ' build the full select list
            sSelectList = sSelectList & Strings.Chr(13) & Strings.Chr(10) & _
                          sTableList & Strings.Chr(13) & Strings.Chr(10) & _
                          sWhereList.ToString()

            ' update template sql to contain relevant datamodel, table, prefixes
            sSelectList = sSelectList.Substring(0).Replace(ACSPGenDataTempDataModelName, v_sDatamodel)
            sSelectList = sSelectList.Substring(0).Replace(ACSPGenDataTempDataModelTableName, v_sTableName)
            sSelectList = sSelectList.Substring(0).Replace(ACSPGenDataTempDataModelTableAlias, v_sPrefix)

            'Create the new procedure
            sSQL = "CREATE PROCEDURE " & v_sProcedureName & "_get_keys " & Strings.Chr(13) & Strings.Chr(10) & _
                   "@PartyCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@InsuranceFileCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@RiskId INT = NULL," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@ClaimCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                   "@DocumentRef VARCHAR(25)"

            ' dynamically generate instance variables
            ' the number of instance variables is determined by how far down the
            ' tree structure the object is..
            generateInstanceVariables(sSQL, lLevel)

            ' combine the sql statments to produce the stored procedure sql
            sSQL = sSQL & Strings.Chr(13) & Strings.Chr(10) & "AS" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & sSelectList & Strings.Chr(13) & Strings.Chr(10)

            ' add stored procedure to database
            If r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & v_sProcedureName & "_get_keys StoredProcedure", bStoredProcedure:=False) = gPMConstants.PMEReturnCode.PMTrue Then

                'Set permissions on stored procedure
                sSQL = "GRANT EXECUTE ON dbo." & v_sProcedureName & "_get_keys TO PUBLIC" & Strings.Chr(13) & Strings.Chr(10)

                If r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Grant " & v_sProcedureName & "_get_keys StoredProcedure", bStoredProcedure:=False) <> gPMConstants.PMEReturnCode.PMTrue Then

                    ' log error
                    result = gPMConstants.PMEReturnCode.PMFalse

                gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to set default permissions for " & _
                                              " stored procedure:" & v_sProcedureName, vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)

                End If

            Else
                ' log error
                result = gPMConstants.PMEReturnCode.PMFalse

            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to create stored procedure:" & sSQL, vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)

            End If

            Return result

    End Function

    ' ***************************************************************** '
    ' Name: GenerateClaimRiskGetParentKeysSP
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 08-11-2003 : CQ3049
    ' ***************************************************************** '
    Private Function GenerateClaimRiskGetParentKeysSP(ByVal v_sTableName As String, ByVal v_sPrefix As String, ByVal v_sDatamodel As String, ByVal v_sProcedureName As String, ByVal v_sProcedureNameOldStyle As String, ByVal v_vArray(,) As Object, ByRef r_oDatabase As dPMDAO.Database) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GenerateClaimRiskGetParentKeysSP"

        Dim sSelectList, sTableList, sWhereList, sSQL As String

        

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Drop Existing Stored Procedure if it is there
            ' Use both old and new formats to ensure old procedure removed

            ' for some bizarre reason you have to set the return value to
            ' true for the drop procedures to do anything
            m_lReturn = gPMConstants.PMEReturnCode.PMTrue

            DropExistingProcedure(v_sProcedureName:=v_sProcedureName, v_sPostFix:="_get_parent_key", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

            ' for some bizarre reason you have to set the return value to
            ' true for the drop procedures to do anything
            m_lReturn = gPMConstants.PMEReturnCode.PMTrue

            DropExistingProcedure(v_sProcedureName:=v_sProcedureNameOldStyle, v_sPostFix:="_get_parent_key", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                'Build up the template for the query

                sSelectList = "SELECT DISTINCT XXXDATAMODELTABLEALIAS." & CStr(v_vArray(pbObjectAndPropertyConsts.ACPColumnName, 1))

                sTableList = "FROM XXXDATAMODELTABLENAME as XXXDATAMODELTABLEALIAS"

                sWhereList = "WHERE XXXDATAMODELNAME_policy_binder_id in (" & _
                             " SELECT XXXDATAMODELNAME_policy_binder_id from " & _
                             " XXXDATAMODELNAME_Policy_Binder where gis_policy_link_id in ( " & _
                             " Select gis_policy_link_id from gis_Policy_Link " & _
                             "Where Claim_Id =@ClaimCnt))"

                ' build the full select list
                sSelectList = sSelectList & Strings.Chr(13) & Strings.Chr(10) & _
                              sTableList & Strings.Chr(13) & Strings.Chr(10) & _
                              sWhereList

                ' update template sql to contain relevant datamodel, table, prefixes
                sSelectList = sSelectList.Substring(0).Replace(ACSPGenDataTempDataModelName, v_sDatamodel)
                sSelectList = sSelectList.Substring(0).Replace(ACSPGenDataTempDataModelTableName, v_sTableName)
                sSelectList = sSelectList.Substring(0).Replace(ACSPGenDataTempDataModelTableAlias, v_sPrefix)

                'Create the new procedure
                sSQL = ""

                sSQL = sSQL & "CREATE PROCEDURE " & v_sProcedureName & "_get_parent_key @PartyCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                       "@InsuranceFileCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                       "@RiskId INT = NULL," & Strings.Chr(13) & Strings.Chr(10) & _
                       "@ClaimCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                       "@DocumentRef VARCHAR(25)," & Strings.Chr(13) & Strings.Chr(10) & _
                       "@Instance1 INT," & Strings.Chr(13) & Strings.Chr(10) & _
                       "@Instance2 INT," & Strings.Chr(13) & Strings.Chr(10) & _
                       "@Instance3 INT" & Strings.Chr(13) & Strings.Chr(10) & _
                       "AS" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10)

                sSQL = sSQL & sSelectList & Strings.Chr(13) & Strings.Chr(10)

                If r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & v_sProcedureName & "_get_parent_key StoredProcedure", bStoredProcedure:=False) = gPMConstants.PMEReturnCode.PMTrue Then
                    'Set permissions
                    sSQL = "GRANT EXECUTE ON dbo." & v_sProcedureName & "_get_parent_key TO PUBLIC" & Strings.Chr(13) & Strings.Chr(10)

                    If r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Grant " & v_sProcedureName & "_get_parent_key StoredProcedure", bStoredProcedure:=False) <> gPMConstants.PMEReturnCode.PMTrue Then

                        ' log error
                        result = gPMConstants.PMEReturnCode.PMFalse

                    gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to set default permissions for " & _
                                                  " stored procedure:" & v_sProcedureName, vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)

                    End If

                Else

                    ' log error
                    result = gPMConstants.PMEReturnCode.PMFalse

                gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to create stored procedure:" & sSQL, vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)

                End If
            Else
                ' log error
                result = gPMConstants.PMEReturnCode.PMFalse

            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to drop stored procedure:" & _
                                          v_sProcedureName & " OR " & v_sProcedureNameOldStyle, vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)

            End If

            Return result

    End Function

    ' ***************************************************************** '
    ' Name: GenerateClaimRiskSP
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 08-11-2003 : CQ3049
    ' ***************************************************************** '
    Private Function GenerateClaimRiskSP(ByVal v_lObjectId As Integer, ByVal v_vGISObject(,) As Object, ByVal v_vGISProperty() As Object, ByVal v_lParentId As Integer, ByVal v_sDatamodel As String, ByVal v_lPMProductFamily As Integer, ByRef r_oDatabase As Object) As Integer

        Dim result As Integer = 0
        Dim lTemp As Integer
        Dim sProcedureName, sProcedureNameOldStyle As String
        Dim iParentIsMultipleInstance As Integer
        Dim sTemp, sPrefix As String
        Dim lParamCount As Integer

        Dim sSelectList, sTableList, sWhereList As String
        Dim vArray(,) As Object
        Dim sAddToWhereList, sSQL, sTableName As String
        Dim iInstanceCount As Integer
        Dim llBound, lUbound, lLevel As Integer

        Const sFunctionName As String = "GenerateClaimRiskSP"

        

            result = gPMConstants.PMEReturnCode.PMTrue

            ' if not the parent object

            If (v_vGISObject(pbObjectAndPropertyConsts.ACOGISObjectId, v_lObjectId)) <> v_lParentId Then

                '*****************************
                ' determine the old and new format of the stored procedures
                '*****************************

                sProcedureName = CStr(v_vGISObject(pbObjectAndPropertyConsts.ACOObjectName, v_lObjectId)).ToLower()

                m_lReturn = CType(GetProcedureName(r_sProcedureName:=sProcedureName, v_sDatamodel:=v_sDatamodel), gPMConstants.PMEReturnCode)

                sProcedureNameOldStyle = "sp_wp_" & v_sDatamodel & sProcedureName
                sProcedureName = "spg_wp_" & v_sDatamodel & sProcedureName

                iParentIsMultipleInstance = IsTopLevelParentObjectMultipleInstance(CStr(v_vGISObject(pbObjectAndPropertyConsts.ACOObjectName, v_lObjectId)), v_vGISObject)

                sTableName = CStr(v_vGISObject(pbObjectAndPropertyConsts.ACOTableName, v_lObjectId)).Trim()

                '******************
                ' determine prefix (alias) for table name
                '******************
                sTemp = StripDataModelCode(v_vTheString:=sTableName, v_sDatamodel:=v_sDatamodel)

                sTemp = sTemp.Substring(sTemp.IndexOf("_"c) + 1)
                sPrefix = v_sDatamodel
                sPrefix = sPrefix & sTemp.Substring(0, 1).ToUpper()

                '*****************
                ' Build SQL for Create Procedure
                '*****************

                ' intialise variables
                lParamCount = 0
                sSelectList = ""
                sTableList = "FROM " & sTableName & " " & sPrefix & Strings.Chr(13) & Strings.Chr(10)

                ' Get and use property array for the current object

                vArray = v_vGISProperty(v_lObjectId)

                ' determine the level of the object
                ' as this will determine what keys stored procedures if any
                ' need to be created.
                If Information.IsArray(vArray) Then

                    llBound = vArray.GetLowerBound(1)

                    lUbound = vArray.GetUpperBound(1)

                    For lIDKeyPos As Integer = llBound To lUbound

                        If (vArray(pbObjectAndPropertyConsts.ACPIsIdentifyingProperty, lIDKeyPos)) = 1 Then
                            lLevel = lIDKeyPos
                        End If
                    Next lIDKeyPos
                End If

                '************************
                ' Generate Get Keys Stored Procedure
                '************************

                If lLevel > 1 Or (v_vGISObject(pbObjectAndPropertyConsts.ACOMaxInstances, lTemp)) > 1 Then

                    ' Use the Live claim peril table to get the keys from

                    m_lReturn = CType(GenerateClaimRiskGetKeysSP(v_sTableName:=sTableName, v_sPrefix:=sPrefix, v_sDatamodel:=v_sDatamodel, v_sProcedureName:=sProcedureName, v_sProcedureNameOldStyle:=sProcedureNameOldStyle, v_vArray:=vArray, v_iIsParentMultipleInstance:=iParentIsMultipleInstance, r_oDatabase:=r_oDatabase), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

                '************************
                ' Generate Get Parent Keys Stored Procedure
                '************************
                ' MEvans : 11-11-2003 : CQ3049
                ' Added support for 3 full levels of object to be documented
                ' excluding policy binder...
                ' eg.
                '       Policy Binder
                '           Test 1
                '               Test 2
                '                   Test 3

                If lLevel >= 2 Or ((v_vGISObject(pbObjectAndPropertyConsts.ACOMaxInstances, lTemp)) > 1) Then

                    m_lReturn = CType(GenerateClaimRiskGetParentKeysSP(v_sTableName:=sTableName, v_sPrefix:=sPrefix, v_sDatamodel:=v_sDatamodel, v_sProcedureName:=sProcedureName, v_sProcedureNameOldStyle:=sProcedureNameOldStyle, v_vArray:=vArray, r_oDatabase:=r_oDatabase), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

                '*****************
                ' Build SQL for Create Procedure
                '*****************

                ' intialise variables
                lParamCount = 0
                sSelectList = ""
                sAddToWhereList = ""
                sTableList = "FROM " & sTableName & " " & sPrefix & Strings.Chr(13) & Strings.Chr(10)

                ' Get and use property array for the current object

                vArray = v_vGISProperty(v_lObjectId)

                If ProcessPropertyArray(v_lObjectType:=GISDataModelType.GISOTRisk, r_vPropertyArray:=vArray, r_sSelectList:=sSelectList, r_sTableList:=sTableList, v_sMainGroup:="Claim", v_sProcedureName:=sProcedureName, v_lPMProductFamily:=v_lPMProductFamily, v_sDatamodel:=v_sDatamodel, r_oDatabase:=r_oDatabase, r_lParamCount:=lParamCount, v_sPrefix:=sPrefix, v_sSubGroup:="", v_sLoop1:="", r_sWhereList:=sAddToWhereList, r_iInstanceCount:=iInstanceCount, v_lLevel:=lLevel) = gPMConstants.PMEReturnCode.PMTrue Then

                    If lParamCount <> 0 Then

                        'Add the rest...
                        sTableList = sTableList.Substring(0, sTableList.Length - 2)

                        'sWhereList1 = "WHERE Claim_Peril_id = @Instance2"
                        sWhereList = "WHERE XXXDATAMODELNAME_policy_binder_id in (" & " SELECT XXXDATAMODELNAME_policy_binder_id from " & " XXXDATAMODELNAME_Policy_Binder where gis_policy_link_id in ( " & " Select gis_policy_link_id from gis_Policy_Link " & "Where Claim_Id =@ClaimCnt))"
                        ' for some bizarre reason you have to set the return value to
                        ' true for the drop procedures to do anything
                        m_lReturn = gPMConstants.PMEReturnCode.PMTrue

                        'Drop it if it's already there

                        DropExistingProcedure(v_sProcedureName:=sProcedureName, v_sPostFix:="", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

                        ' for some bizarre reason you have to set the return value to
                        ' true for the drop procedures to do anything
                        m_lReturn = gPMConstants.PMEReturnCode.PMTrue

                        DropExistingProcedure(v_sProcedureName:=sProcedureNameOldStyle, v_sPostFix:="", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

                        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                            'remove the last comma and vbcrlf
                            sSelectList = sSelectList.Substring(0, sSelectList.Length - 3) & Strings.Chr(13) & Strings.Chr(10)

                            sSelectList = "SELECT " & sSelectList & Strings.Chr(13) & Strings.Chr(10) & _
                                          sTableList & Strings.Chr(13) & Strings.Chr(10) & _
                                          sWhereList & Strings.Chr(13) & Strings.Chr(10) & _
                                          sAddToWhereList

                            sSelectList = sSelectList.Substring(0).Replace(ACSPGenDataTempDataModelName, v_sDatamodel)
                            sSelectList = sSelectList.Substring(0).Replace(ACSPGenDataTempDataModelTableName, sTableName)
                            sSelectList = sSelectList.Substring(0).Replace(ACSPGenDataTempDataModelTableAlias, sPrefix)

                            'Create the new procedure
                            sSQL = ""

                            sSQL = sSQL & "CREATE PROCEDURE " & sProcedureName & Strings.Chr(13) & Strings.Chr(10) & _
                                   "@PartyCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                                   "@InsuranceFileCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                                   "@ClaimCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                                   "@DocumentRef VARCHAR(25)" & Strings.Chr(13) & Strings.Chr(10)

                            'dynamically generate instance variables
                            generateInstanceVariables(sSQL, iInstanceCount)
                            sSQL = sSQL & Strings.Chr(13) & Strings.Chr(10) & "AS" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & sSelectList & Strings.Chr(13) & Strings.Chr(10)

                            If r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & sProcedureName & " StoredProcedure", bStoredProcedure:=False) = gPMConstants.PMEReturnCode.PMTrue Then

                                'Set permissions
                                sSQL = "GRANT EXECUTE ON dbo." & sProcedureName & " TO PUBLIC"

                                If r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Grant " & sProcedureName & " StoredProcedure", bStoredProcedure:=False) <> gPMConstants.PMEReturnCode.PMTrue Then

                                    ' log error
                                    result = gPMConstants.PMEReturnCode.PMFalse

                                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                                    oDict.Add("v_lObjectId", v_lObjectId)
                                    oDict.Add("v_lParentId", v_lParentId)
                                gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to set default permissions for " & _
                                                              " stored procedure:" & sProcedureName, vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)
                                End If
                            Else
                                ' log error
                                result = gPMConstants.PMEReturnCode.PMFalse

                            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                                oDict.Add("v_lObjectId", v_lObjectId)
                                oDict.Add("v_lParentId", v_lParentId)
                                gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to create stored procedure:" & sSQL, vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)

                            End If

                        Else
                            ' log error
                            result = gPMConstants.PMEReturnCode.PMFalse

                        Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                            oDict.Add("v_lObjectId", v_lObjectId)
                            oDict.Add("v_lParentId", v_lParentId)
                            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to drop stored procedure:" & sProcedureName & " OR " & sProcedureNameOldStyle, vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)

                        End If

                    End If ' we have parameters

                Else
                    ' log error
                    result = gPMConstants.PMEReturnCode.PMFalse

                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                    oDict.Add("v_lObjectId", v_lObjectId)
                    oDict.Add("v_lParentId", v_lParentId)
                    gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to ProcessPropertyArray", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)

                End If

            End If

            Return result

    End Function
End Module
