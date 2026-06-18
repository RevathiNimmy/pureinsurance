Option Strict Off
Option Explicit On
Imports System.IO
Imports System.Text
Imports SSP.Shared
'refer Developer Guide No. 129
Module StoredProcSQL
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    '
    ' Edit History:
    ' RKS 27/04/2005 354-Standard Wording Control Enchancements
    ' ***************************************************************** '



    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    Private Const ACClass As String = ""

    '****************
    Private Const ACWPFFieldName As Integer = 0
    Private Const ACWPFColumnName As Integer = 1
    Private Const ACWPFColumnType As Integer = 2
    Private Const ACWPFDisplayName As Integer = 3
    '****************

    Private m_vSumInsured As Object

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
    Public Function GenerateDMRelatedStoredProcedures(ByVal v_sDatamodel As String, ByRef r_oDatabase As dPMDAO.Database, Optional ByVal v_lSwiftIntegration As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim sProcedureName, sSQL As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If v_lSwiftIntegration <> 0 Then
                ' dont do this for swift
            Else
                sProcedureName = "spg_" & v_sDatamodel & "_copy_sums_insured"

                'Drop it if it's already there
                DropExistingProcedure(sProcedureName, v_sPostFix:="", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                sSQL = "SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON" & Strings.ChrW(13) & Strings.ChrW(10)
                m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Pre Create " & sProcedureName, bStoredProcedure:=False)

                sSQL = "CREATE PROCEDURE " & sProcedureName & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "                 @old_policy_link_id int," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "                 @new_policy_link_id int" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "AS " & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "--Take advantage here of the identicalityness of gis_policy_link_id and rsa_policy_binder_id (or whatever)" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "INSERT INTO RSA_sum_insured (" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "        RSA_Policy_binder_id," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "        sum_insured_type_id," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "        sequence_id," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "        description," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "        reference," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "        sum_insured," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "        date_added," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "        date_deleted," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "        is_valuation_required," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "        valuation_date," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "        rate," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "        premium" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & ")"
                sSQL = sSQL & "SELECT  @new_policy_link_id," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "        sum_insured_type_id," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "        sequence_id," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "        description," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "        reference," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "        sum_insured," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "        date_added," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "        date_deleted," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "        is_valuation_required," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "        valuation_date," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "        rate," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "        premium" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "From RSA_sum_insured" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "WHERE   RSA_Policy_binder_id = @old_policy_link_id" & Strings.ChrW(13) & Strings.ChrW(10)

                sSQL = sSQL.Replace(" RSA_", " " & v_sDatamodel & "_")

                m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & sProcedureName, bStoredProcedure:=False)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GenerateDMRelatedStoredProcedures Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateDMRelatedStoredProcedures", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


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

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".DropExistingProcedure")

        Dim sSQL As String = ""

        If r_lRetval <> gPMConstants.PMEReturnCode.PMTrue Then Exit Sub

        r_oDatabase.Parameters.Clear()

        sSQL = ACDropStoredProcedureSQL

        bPMAddParameter.AddParameter(r_oDatabase, sSQL, m_lReturn, "sName", v_sProcedureName & v_sPostFix, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:=ACDropStoredProcedureName, bStoredProcedure:=ACDropStoredProcedureStored)
        End If

        ' Debug message
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
    Private Function GenerateGetParentStoredProcedure(ByRef sTableName As String, ByRef sPrefix As String, ByVal v_sDatamodel As String, ByRef sProcedureName As String, ByRef sProcedureNameOldStyle As String, ByRef vArray(,) As Object, ByRef r_oDatabase As dPMDAO.Database, Optional ByVal v_sUnderwritingOrAgency As String = "") As Integer

        Dim result As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim sSQL As String = ""
        Dim lGISDataModelType As Integer

        ' what type of datamodel are we processing...?
        m_lReturn = CType(GetDataModelType(r_oDatabase, v_sDatamodel, lGISDataModelType), gPMConstants.PMEReturnCode)

        'Create the new procedure
        sSQL = "CREATE PROCEDURE " & sProcedureName & "_get_parent_key " & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    @PartyCnt INT, " & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    @InsuranceFileCnt INT, " & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    @RiskId INT = NULL, " & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    @ClaimCnt INT, " & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    @DocumentRef VARCHAR(25), " & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    @Instance1 INT, " & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    @Instance2 INT, " & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    @Instance3 INT " & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "AS " & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)

        If lGISDataModelType = GISDataModelType.GISDMTypeParty Then

            sSQL = sSQL & "    SELECT DISTINCT " & sPrefix & "." & CStr(vArray(pbObjectAndPropertyConsts.ACPColumnName, 1)) & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    FROM " & sTableName & " " & sPrefix & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    JOIN " & v_sDatamodel & "_Policy_binder pb" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        ON pb." & v_sDatamodel & "_policy_binder_id = " & sPrefix & "." & v_sDatamodel & "_policy_binder_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    JOIN GIS_policy_link gpl" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        ON gpl.gis_policy_link_id = pb.gis_policy_link_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    JOIN party p" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        ON gpl.party_cnt = p.party_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    WHERE p.party_cnt = @PartyCnt" & Strings.ChrW(13) & Strings.ChrW(10)
        Else
            sSQL = sSQL & "If @RiskId Is Null " & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & "    SELECT DISTINCT " & sPrefix & "." & CStr(vArray(pbObjectAndPropertyConsts.ACPColumnName, 1)) & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    FROM " & sTableName & " " & sPrefix & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    JOIN " & v_sDatamodel & "_Policy_binder pb" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        ON pb." & v_sDatamodel & "_policy_binder_id = " & sPrefix & "." & v_sDatamodel & "_policy_binder_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    JOIN GIS_policy_link gpl" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        ON gpl.gis_policy_link_id = pb.gis_policy_link_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    JOIN insurance_file ifi" & Strings.ChrW(13) & Strings.ChrW(10)
            If v_sUnderwritingOrAgency = "U" Then
                sSQL = sSQL & "        ON gpl.insurance_file_cnt = ifi.insurance_folder_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            Else
                sSQL = sSQL & "        ON gpl.insurance_file_cnt = ifi.insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            End If
            sSQL = sSQL & "    WHERE ifi.insurance_file_cnt = @InsuranceFileCnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    AND gpl.Risk_id IS NULL" & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & "Else" & Strings.ChrW(13) & Strings.ChrW(10)


            sSQL = sSQL & "    SELECT DISTINCT " & sPrefix & "." & CStr(vArray(pbObjectAndPropertyConsts.ACPColumnName, 1)) & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    FROM " & sTableName & " " & sPrefix & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    JOIN " & v_sDatamodel & "_Policy_binder pb" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        ON pb." & v_sDatamodel & "_policy_binder_id = " & sPrefix & "." & v_sDatamodel & "_policy_binder_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    JOIN GIS_policy_link gpl" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        ON gpl.gis_policy_link_id = pb.gis_policy_link_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    JOIN insurance_file ifi" & Strings.ChrW(13) & Strings.ChrW(10)
            If v_sUnderwritingOrAgency = "U" Then
                sSQL = sSQL & "        ON gpl.insurance_file_cnt = ifi.insurance_folder_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            Else
                sSQL = sSQL & "        ON gpl.insurance_file_cnt = ifi.insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            End If
            sSQL = sSQL & "    WHERE ifi.insurance_file_cnt = @InsuranceFileCnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    AND gpl.Risk_id = @RiskId" & Strings.ChrW(13) & Strings.ChrW(10)
        End If

        'Drop it if it's already there
        DropExistingProcedure(v_sProcedureName:=sProcedureName, v_sPostFix:="_get_parent_key", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

        DropExistingProcedure(v_sProcedureName:=sProcedureNameOldStyle, v_sPostFix:="_get_parent_key", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'remove the last comma and vbcrlf

        m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & sProcedureName & "_get_parent_key StoredProcedure", bStoredProcedure:=False)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Set permissions

        sSQL = "GRANT EXECUTE ON dbo." & sProcedureName & "_get_parent_key TO PUBLIC" & Strings.ChrW(13) & Strings.ChrW(10)

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


        Dim result As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim lLevel, lGSCount As Integer
        Dim sSQL As String = ""
        Dim sSelectList As New StringBuilder
        Dim sTableList As New StringBuilder
        Dim sWhereList As New StringBuilder
        Dim iSequenceNumberField As Integer
        Dim lGISDataModelType As Integer

        'Intialise
        lLevel = 0
        lGSCount = 1
        sSQL = ""
        sSelectList = New StringBuilder("")
        sTableList = New StringBuilder("")
        sWhereList = New StringBuilder("")
        iSequenceNumberField = -1

        ' what type of datamodel are we processing...?
        m_lReturn = CType(GetDataModelType(r_oDatabase, v_sDatamodel, lGISDataModelType), gPMConstants.PMEReturnCode)

        'Check that array parameter is valid
        If Not Informations.IsArray(vArray) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        For lTemp As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)
            'What's the highest number key?

            If CDbl(vArray(pbObjectAndPropertyConsts.ACPIsIdentifyingProperty, lTemp)) = 1 Then
                lLevel = lTemp
            End If
            'do we have a sequence_id field

            If CStr(vArray(pbObjectAndPropertyConsts.ACPPropertyName, lTemp)) = GISSharedPropertyConstants.cProperty_SequenceId Then
                iSequenceNumberField = lTemp
            End If
        Next

        'Get all dynamic lines for the script
        For lTemp As Integer = vArray.GetLowerBound(1) + 1 To vArray.GetUpperBound(1)

            If CDbl(vArray(pbObjectAndPropertyConsts.ACPIsIdentifyingProperty, lTemp)) = 1 Then
                If lTemp < lLevel Then

                    sWhereList.Append("    SELECT @sql = @sql + 'AND " & sPrefix & "." & CStr(vArray(pbObjectAndPropertyConsts.ACPColumnName, lTemp)) & " = ' + CAST(@Instance" & (CStr(lTemp + v_iIsParentMultipleInstance)) & " AS VARCHAR(20)) + ' '" & Strings.ChrW(13) & Strings.ChrW(10))
                    lGSCount += 1
                    sTableList.Append("    JOIN gis_screen gs" & lGSCount & Strings.ChrW(13) & Strings.ChrW(10))
                    sTableList.Append("        ON gs" & lGSCount & ".parent_id = gs" & (CStr(lGSCount - 1)) & ".gis_screen_id" & Strings.ChrW(13) & Strings.ChrW(10))
                    'Add a check to make sure the child screen is a currently attached to the parent screen (removed child screens are not deleted, so we need to check for this).
                    sTableList.Append("    JOIN gis_screen_detail gsd" & (CStr(lGSCount - 1)) & Strings.ChrW(13) & Strings.ChrW(10))
                    sTableList.Append("        ON gsd" & (CStr(lGSCount - 1)) & ".gis_screen_id = gs" & (CStr(lGSCount - 1)) & ".gis_screen_id" & Strings.ChrW(13) & Strings.ChrW(10))
                    sTableList.Append("        AND gsd" & (CStr(lGSCount - 1)) & ".child_screen_id = gs" & CStr(lGSCount) & ".gis_screen_id" & Strings.ChrW(13) & Strings.ChrW(10))


                Else

                    sSelectList.Append("    SELECT @sql = @sql + '    " & sPrefix & "." & CStr(vArray(pbObjectAndPropertyConsts.ACPColumnName, lTemp)) & " '" & Strings.ChrW(13) & Strings.ChrW(10))
                End If
            End If
        Next

        If v_iIsParentMultipleInstance > 0 Then
        End If

        'Create the whole script into a string

        sSQL = sSQL & "CREATE PROCEDURE " & sProcedureName & "_get_keys" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    @PartyCnt INT," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    @InsuranceFileCnt INT," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    @RiskId INT = NULL," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    @ClaimCnt INT," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    @DocumentRef VARCHAR(25)"

        'Dynamically generate instance variables
        generateInstanceVariables(sSQL, lLevel)

        sSQL = sSQL & "" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "AS" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "" & Strings.ChrW(13) & Strings.ChrW(10)


        sSQL = sSQL & "DECLARE @column_name VARCHAR(70)" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "DECLARE @sql VARCHAR(8000)" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "DECLARE @sql_orderby VARCHAR(8000)" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "SELECT @sql = ''" & Strings.ChrW(13) & Strings.ChrW(10)

        'test to see if there is a sequence number field
        If iSequenceNumberField = -1 Then
            'no sequence number field so build order by from the fields on the screen
            sSQL = sSQL & "SELECT @sql_orderby = 'ORDER BY '" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "DECLARE c_column CURSOR FAST_FORWARD FOR" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    SELECT" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        gp.column_name" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    FROM risk r" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    JOIN gis_screen gs1" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        ON gs1.gis_screen_id = r.gis_screen_id" & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & sTableList.ToString()

            sSQL = sSQL & "    JOIN gis_screen_detail gsd" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        ON gsd.gis_screen_id = gs" & CStr(lGSCount) & ".gis_screen_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    JOIN gis_object go" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        ON go.gis_object_id = gsd.gis_object_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    JOIN gis_property gp" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        ON gp.gis_property_id = gsd.gis_property_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        AND gp.gis_object_id = gsd.gis_object_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    WHERE r.risk_cnt = @RiskId" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    AND go.table_name = '" & sTableName & "'" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    AND gsd.column_position > 0" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    ORDER BY gsd.column_position" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "OPEN c_column" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "FETCH NEXT FROM c_column INTO" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    @column_name" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "IF @@FETCH_STATUS <> 0" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "BEGIN" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    SELECT @sql_orderby = ''" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "END" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHILE @@FETCH_STATUS = 0" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "BEGIN" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    SELECT @sql_orderby = @sql_orderby + '" & sPrefix & ".' + @column_name" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    FETCH NEXT FROM c_column INTO" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        @column_name" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    IF @@FETCH_STATUS = 0" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    BEGIN" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        SELECT @sql_orderby = @sql_orderby + ', '" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    END" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "END" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "CLOSE c_column" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "DEALLOCATE c_column" & Strings.ChrW(13) & Strings.ChrW(10)
        Else
            'sequence number field so build order by from the sequence number column name

            sSQL = sSQL & "SELECT @sql_orderby = 'ORDER BY " & sPrefix & "." & CStr(vArray(pbObjectAndPropertyConsts.ACPColumnName, iSequenceNumberField)) & "'" & Strings.ChrW(13) & Strings.ChrW(10)
        End If
        sSQL = sSQL & "" & Strings.ChrW(13) & Strings.ChrW(10)
        If lGISDataModelType = GISDataModelType.GISDMTypeParty Then
            sSQL = sSQL & "SELECT @sql = @sql + 'SELECT '" & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & sSelectList.ToString()
            sSQL = sSQL & "SELECT @sql = @sql + 'FROM " & sTableName & " " & sPrefix & " '" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "SELECT @sql = @sql + 'JOIN " & v_sDatamodel & "_policy_binder pb '" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "SELECT @sql = @sql + '    ON pb." & v_sDatamodel & "_policy_binder_id = " & sPrefix & "." & v_sDatamodel & "_Policy_binder_id '" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "SELECT @sql = @sql + 'JOIN GIS_policy_link gpl '" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "SELECT @sql = @sql + '    ON gpl.gis_policy_link_id = pb.gis_policy_link_id '" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "SELECT @sql = @sql + 'JOIN party p '" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "SELECT @sql = @sql + '    ON p.party_cnt = gpl.party_cnt '" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "SELECT @sql = @sql + 'WHERE p.party_cnt = ' + CAST(@PartyCnt AS VARCHAR(20)) + ' '" & Strings.ChrW(13) & Strings.ChrW(10)
        Else

            sSQL = sSQL & "IF @RiskId IS NULL" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "BEGIN" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    SELECT @sql = @sql + 'SELECT '" & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & "    " & sSelectList.ToString()
            sSQL = sSQL & "    SELECT @sql = @sql + 'FROM " & sTableName & " " & sPrefix & " '" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    SELECT @sql = @sql + 'JOIN " & v_sDatamodel & "_policy_binder pb '" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    SELECT @sql = @sql + '    ON pb." & v_sDatamodel & "_policy_binder_id = " & sPrefix & "." & v_sDatamodel & "_Policy_binder_id '" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    SELECT @sql = @sql + 'JOIN GIS_policy_link gpl '" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    SELECT @sql = @sql + '    ON gpl.gis_policy_link_id = pb.gis_policy_link_id '" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    SELECT @sql = @sql + 'JOIN insurance_file ifi '" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    SELECT @sql = @sql + '    ON ifi.insurance_file_cnt = gpl.insurance_file_cnt '" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    SELECT @sql = @sql + 'WHERE gpl.Risk_id IS NULL '" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    SELECT @sql = @sql + 'AND ifi.insurance_file_cnt = ' + CAST(@InsuranceFileCnt AS VARCHAR(20)) + ' '" & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & sWhereList.ToString()

            sSQL = sSQL & "END" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ELSE" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "BEGIN" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    SELECT @sql = @sql + 'SELECT '" & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & sSelectList.ToString()

            sSQL = sSQL & "    SELECT @sql = @sql + 'FROM " & sTableName & " " & sPrefix & " '" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    SELECT @sql = @sql + 'JOIN " & v_sDatamodel & "_policy_binder pb '" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    SELECT @sql = @sql + '    ON pb." & v_sDatamodel & "_policy_binder_id = " & sPrefix & "." & v_sDatamodel & "_Policy_binder_id '" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    SELECT @sql = @sql + 'JOIN GIS_policy_link gpl '" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    SELECT @sql = @sql + '    ON gpl.gis_policy_link_id = pb.gis_policy_link_id '" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    SELECT @sql = @sql + 'WHERE gpl.Risk_id = ' + CAST(@RiskId AS VARCHAR(20)) + ' '" & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & sWhereList.ToString()

            sSQL = sSQL & "END" & Strings.ChrW(13) & Strings.ChrW(10)
        End If

        sSQL = sSQL & "" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "EXEC (@sql + @sql_orderby)" & Strings.ChrW(13) & Strings.ChrW(10)


        'Drop existing procedure of the same name
        DropExistingProcedure(v_sProcedureName:=sProcedureName, v_sPostFix:="_get_keys", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

        DropExistingProcedure(v_sProcedureName:=sProcedureNameOldStyle, v_sPostFix:="_get_keys", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

        'Create the new procedure
        m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & sProcedureName & "_get_keys StoredProcedure", bStoredProcedure:=False)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Set permissions
        sSQL = "GRANT EXECUTE ON dbo." & sProcedureName & "_get_keys TO PUBLIC" & Strings.ChrW(13) & Strings.ChrW(10)

        m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Grant " & sProcedureName & "_get_keys StoredProcedure", bStoredProcedure:=False)

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
    '          23-11-2005 CJB     - PN25916 Do not process objects lower than grandchild level.
    ' ***************************************************************** '
    Public Function GenerateStoredProcedure(ByRef r_vGISObject(,) As Object, ByRef r_vGISProperty() As Object, ByVal v_sDatamodel As String, ByRef r_oDatabase As dPMDAO.Database, ByVal v_lPMProductFamily As Integer, Optional ByVal v_lGisDataModelType As Integer = 0, Optional ByVal v_lSwiftIntegration As Integer = 0, Optional ByVal v_sUnderwritingOrAgency As String = "") As Integer

        Dim result As Integer = 0
        Dim sProcedureName As String = ""
        Dim sProcedureNameOldStyle As String = ""
        Dim sWhereList3 As String = ""
        Dim sFinalSelectList As New StringBuilder
        Dim sTableList As New StringBuilder
        Dim sSelectList As New StringBuilder
        Dim sParameterList As New StringBuilder
        Dim sWhereList2 As New StringBuilder
        Dim sWhereList1 As New StringBuilder
        Dim lTemp3 As Integer
        Dim sTemp, sSQL, sColumnName, sSubGroup, sSubGroup2, sSubGroup3, sSubGroup4, sLoop1, sLoop2, sLoop3, sLoop4 As String
        Dim sDisplayname As New StringBuilder
        Dim sFieldName As New StringBuilder
        Dim sSubGroupTemp As New StringBuilder
        Dim lFormat As gPMConstants.PMEFormatStyle
        Dim vArray(,) As Object
        Dim sPrefix As New StringBuilder
        Dim lCount As Integer
        Dim sSelect As String = ""
        Dim lOtherTableCount, lParentId, lLevel As Integer
        Dim sAddressCount As String = ""
        Dim lAddressCount As Integer
        Dim iParentIsMultipleInstance, iInstanceCount As Integer

        Dim strTableList As String
        Dim strSelectList As String

        ' PW250703 - PS229
        Dim iObjectType As Integer
        Dim bFixedField As Boolean

        ' PW240703 - PS229 - Get data model type and set up field manager group
        Dim lGISDataModelType As Integer
        Dim sMainGroup As String = ""

        m_lReturn = CType(GetDataModelType(r_oDatabase, v_sDatamodel, lGISDataModelType), gPMConstants.PMEReturnCode)
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
        Dim sTableName As String = String.Empty

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sPrefix = New StringBuilder(v_sDatamodel)

            'Delete the records on wp_fields
            'Make sure any old style are deleted first
            sSQL = "DELETE FROM wp_fields" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "WHERE sql LIKE 'sp_wp_" & v_sDatamodel & "%'" & Strings.ChrW(13) & Strings.ChrW(10)

            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Delete " & sProcedureName & " WPFields", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Make sure any new style are deleted first
            'vivek made change as Bal suggested
            sSQL = "DELETE FROM wp_fields" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "WHERE sql LIKE 'spg_wp_" & v_sDatamodel & "%'" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "AND (ISNULL(data_model,'') ='' or UPPER(RTRIM(data_model))='" & v_sDatamodel.Trim().ToUpper() & "')"

            '"DELETE FROM wp_fields" & vbCrLf & _
            '"WHERE sql LIKE 'spg_wp_" & v_sDatamodel & "%'" & vbCrLf & _
            '"AND (data_model IS NULL or UPPER(RTRIM(data_model)) = '" & UCase(Trim(v_sDatamodel)) & "')"

            '"DELETE FROM wp_fields" & Strings.ChrW(13) & Strings.ChrW(10) & _
            '       "WHERE sql LIKE 'spg_wp_" & v_sDatamodel & "%'" & Strings.ChrW(13) & Strings.ChrW(10) & _
            '       "AND column_name IN" & Strings.ChrW(13) & Strings.ChrW(10) & _
            '       "(SELECT column_name" & Strings.ChrW(13) & Strings.ChrW(10) & _
            '       "FROM gis_property" & Strings.ChrW(13) & Strings.ChrW(10) & _
            '       "INNER JOIN gis_object ON gis_object.gis_object_id=gis_property.gis_object_id" & Strings.ChrW(13) & Strings.ChrW(10) & _
            '       "INNER JOIN gis_data_model ON gis_data_model.gis_data_model_id=gis_object.gis_data_model_id" & Strings.ChrW(13) & Strings.ChrW(10) & _
            '       "WHERE gis_data_model.code='" & v_sDatamodel & "')" & Strings.ChrW(13) & Strings.ChrW(10)



            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Delete " & sProcedureName & " WPFields", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            For lTemp As Integer = r_vGISObject.GetLowerBound(1) To r_vGISObject.GetUpperBound(1)

                ' Only process children down to Great-Grandchild level as any lower than this is not supported
                ' in doc production so we don't want them in wp_fields table etc!   PN25916


                vArray = r_vGISProperty(lTemp)
                lLevel = 0
                If Informations.IsArray(vArray) Then

                    For lTemp2 As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)

                        If CDbl(vArray(pbObjectAndPropertyConsts.ACPIsIdentifyingProperty, lTemp2)) = 1 Then
                            lLevel = lTemp2
                        End If
                    Next lTemp2
                End If
                If Not (lLevel > 4) Then

                    iInstanceCount = 0

                    'If Convert.IsDBNull(r_vGISObject(pbObjectAndPropertyConsts.ACOParentObjectId, lTemp)) Or IsNothing(r_vGISObject(pbObjectAndPropertyConsts.ACOParentObjectId, lTemp)) Or (r_vGISObject(pbObjectAndPropertyConsts.ACOParentObjectId, lTemp) = 0) Then
                    If Informations.IsDBNull(r_vGISObject(pbObjectAndPropertyConsts.ACOParentObjectId, lTemp)) Or Informations.IsNothing(r_vGISObject(pbObjectAndPropertyConsts.ACOParentObjectId, lTemp)) Then

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

                            If CDbl(r_vGISObject(pbObjectAndPropertyConsts.ACOGISObjectId, lObject)) <> lParentId Then

                                '*****************************
                                ' determine the old and new format of the stored procedures
                                '*****************************

                                ' use live table

                                sProcedureName = CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOObjectName, lObject)).Substring(0).Replace(ACClaimBasedWorkTablePrefix, "").ToLower()

                                m_lReturn = CType(GetProcedureName(r_sProcedureName:=sProcedureName, v_sDatamodel:=v_sDatamodel), gPMConstants.PMEReturnCode)

                                sProcedureNameOldStyle = "sp_wp_" & v_sDatamodel & sProcedureName
                                sProcedureName = "spg_wp_" & v_sDatamodel & sProcedureName


                                iParentIsMultipleInstance = IsTopLevelParentObjectMultipleInstance(ToSafeString(r_vGISObject(pbObjectAndPropertyConsts.ACOObjectName, lObject)), r_vGISObject)

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
                                sTableList = New StringBuilder("FROM " & sLiveTableName & " " & sPrefix.ToString() & Strings.ChrW(13) & Strings.ChrW(10))

                                ' Get and use property array for the current object


                                vArray = r_vGISProperty(lObject)

                                '*****************
                                ' are there any object properties
                                '*****************
                                'UPGRADE_WARNING: (1068) vArray of type Variant is being forced to Array(Variant). More Informations: http://www.vbtonet.com/ewis/ewi1068.aspx
                                strSelectList = sSelectList.ToString()
                                strTableList = sTableList.ToString()

                                If ProcessPropertyArray(v_lObjectType:=GISDataModelType.GISOTClaim, r_vPropertyArray:=vArray, r_sSelectList:=strSelectList, r_sTableList:=strTableList, v_sMainGroup:="Claim", v_sProcedureName:=sProcedureName, v_lPMProductFamily:=v_lPMProductFamily, v_sDatamodel:=v_sDatamodel, r_oDatabase:=r_oDatabase, r_lParamCount:=lParamCount, v_sPrefix:=sPrefix.ToString(), v_sSubGroup:=v_sDatamodel & " Claim Details") = gPMConstants.PMEReturnCode.PMTrue Then

                                    If strTableList.Contains(sTableList.ToString()) Then
                                        sTableList = New StringBuilder
                                        sTableList.Append(strTableList)
                                    Else
                                        sTableList.Append(strTableList)
                                    End If
                                    If strSelectList.Contains(sSelectList.ToString()) Then
                                        sSelectList = New StringBuilder
                                        sSelectList.Append(strSelectList)
                                    Else
                                        sSelectList.Append(strSelectList)
                                    End If

                                    If lParamCount <> 0 Then

                                        sWhereList1 = New StringBuilder("WHERE " & sPrefix.ToString() & ".Claim_Id = @ClaimCnt")

                                        'Add the rest...
                                        sTableList = New StringBuilder(sTableList.ToString().Substring(0, sTableList.ToString().Length - 2)) '& ", " & vbCrLf & |                                v_sDataModel & "_Policy_binder pb," & vbCrLf & |                                "GIS_policy_link gpl," & vbCrLf & |                                "insurance_file ifi" & vbCrLf

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
                                        sSelectList = New StringBuilder(sSelectList.ToString().Substring(0, sSelectList.ToString().Length - 3) & Strings.ChrW(13) & Strings.ChrW(10))
                                        sSelectList = New StringBuilder(" SELECT " & sSelectList.ToString() & Strings.ChrW(13) & Strings.ChrW(10) &
                                                      sTableList.ToString() & Strings.ChrW(13) & Strings.ChrW(10) &
                                                      sWhereList1.ToString())

                                        'Create the new procedure

                                        sSQL = ""

                                        ' removed risk id as this is knackering the
                                        '"@RiskId INT = NULL," & vbCrLf &
                                        sSQL = sSQL & "CREATE PROCEDURE " & sProcedureName & Strings.ChrW(13) & Strings.ChrW(10) &
                                               "@PartyCnt INT," & Strings.ChrW(13) & Strings.ChrW(10) &
                                               "@InsuranceFileCnt INT," & Strings.ChrW(13) & Strings.ChrW(10) &
                                               "@RiskId INT = NULL," & Strings.ChrW(13) & Strings.ChrW(10) &
                                               "@ClaimCnt INT," & Strings.ChrW(13) & Strings.ChrW(10) &
                                               "@DocumentRef VARCHAR(25)"

                                        'dynamically generate instance variables
                                        generateInstanceVariables(sSQL, iInstanceCount)
                                        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10) & "AS" & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10) & sSelectList.ToString() & Strings.ChrW(13) & Strings.ChrW(10)

                                        m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & sProcedureName & " StoredProcedure", bStoredProcedure:=False)

                                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                            Return gPMConstants.PMEReturnCode.PMFalse
                                        End If

                                        'Set permissions
                                        sSQL = "GRANT EXECUTE ON dbo." & sProcedureName & " TO PUBLIC" & Strings.ChrW(13) & Strings.ChrW(10)

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


                            If CDbl(r_vGISObject(pbObjectAndPropertyConsts.ACOGISObjectId, lObject)) <> lParentId Then
                                '*****************************
                                ' determine the old and new format of the stored procedures
                                '*****************************

                                ' use live tables not work tables for document production

                                sProcedureName = CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOObjectName, lObject)).Substring(0).Replace(ACClaimBasedWorkTablePrefix, "").ToLower()

                                m_lReturn = CType(GetProcedureName(r_sProcedureName:=sProcedureName, v_sDatamodel:=v_sDatamodel), gPMConstants.PMEReturnCode)

                                sProcedureNameOldStyle = "sp_wp_" & v_sDatamodel & sProcedureName
                                sProcedureName = "spg_wp_" & v_sDatamodel & sProcedureName


                                iParentIsMultipleInstance = IsTopLevelParentObjectMultipleInstance(ToSafeString(r_vGISObject(pbObjectAndPropertyConsts.ACOObjectName, lObject)), r_vGISObject)

                                '******************
                                ' determine prefix (alias) for table name
                                '******************

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
                                sTableList = New StringBuilder("FROM " & sLiveTableName & " " & sPrefix.ToString() & Strings.ChrW(13) & Strings.ChrW(10))

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
                                sTableList = New StringBuilder("FROM " & sLiveTableName & " " & sPrefix.ToString() & Strings.ChrW(13) & Strings.ChrW(10))

                                ' Get and use property array for the current object


                                vArray = r_vGISProperty(lObject)

                                strSelectList = sSelectList.ToString()
                                strTableList = sTableList.ToString()

                                If ProcessPropertyArray(v_lObjectType:=GISDataModelType.GISOTPeril, r_vPropertyArray:=vArray, r_sSelectList:=strSelectList, r_sTableList:=strTableList, v_sMainGroup:="Claim", v_sProcedureName:=sProcedureName, v_lPMProductFamily:=v_lPMProductFamily, v_sDatamodel:=v_sDatamodel, r_oDatabase:=r_oDatabase, r_lParamCount:=lParamCount, v_sPrefix:=sPrefix.ToString(), v_sSubGroup:=v_sDatamodel & " Peril Items", v_sLoop1:=v_sDatamodel & "ClaimPeril") = gPMConstants.PMEReturnCode.PMTrue Then


                                    If strTableList.Contains(sTableList.ToString()) Then
                                        sTableList = New StringBuilder
                                        sTableList.Append(strTableList)
                                    Else
                                        sTableList.Append(strTableList)
                                    End If
                                    If strSelectList.Contains(sSelectList.ToString()) Then
                                        sSelectList = New StringBuilder
                                        sSelectList.Append(strSelectList)
                                    Else
                                        sSelectList.Append(strSelectList)
                                    End If


                                    If lParamCount <> 0 Then

                                        'Add the rest...
                                        sTableList = New StringBuilder(sTableList.ToString().Substring(0, sTableList.ToString().Length - 2)) '& ", " & vbCrLf & |                                v_sDataModel & "_Policy_binder pb," & vbCrLf & |                                "GIS_policy_link gpl," & vbCrLf & |                                "insurance_file ifi" & vbCrLf

                                        sWhereList1 = New StringBuilder("WHERE Claim_Peril_id = @Instance2")

                                        'Drop it if it's already there
                                        DropExistingProcedure(v_sProcedureName:=sProcedureName, v_sPostFix:="", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

                                        DropExistingProcedure(v_sProcedureName:=sProcedureNameOldStyle, v_sPostFix:="", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

                                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                            Return gPMConstants.PMEReturnCode.PMFalse
                                        End If

                                        'remove the last comma and vbcrlf
                                        If sSelectList.ToString() <> "" Then
                                            sSelectList = New StringBuilder(sSelectList.ToString().Substring(0, sSelectList.ToString().Length - 3) & Strings.ChrW(13) & Strings.ChrW(10))
                                        End If

                                        sSelectList = New StringBuilder("SELECT " & sSelectList.ToString() & " " & sTableList.ToString() & " " & sWhereList1.ToString())

                                        'Create the new procedure
                                        sSQL = ""

                                        sSQL = sSQL & "CREATE PROCEDURE " & sProcedureName & " @PartyCnt INT," & Strings.ChrW(13) & Strings.ChrW(10) &
                                               "@InsuranceFileCnt INT," & Strings.ChrW(13) & Strings.ChrW(10) &
                                               "@RiskId INT = NULL," & Strings.ChrW(13) & Strings.ChrW(10) &
                                               "@ClaimCnt INT," & Strings.ChrW(13) & Strings.ChrW(10) &
                                               "@DocumentRef VARCHAR(25)," & Strings.ChrW(13) & Strings.ChrW(10) &
                                               "@Instance1  INT = NULL," & Strings.ChrW(13) & Strings.ChrW(10) &
                                               "@Instance2  INT = NULL," & Strings.ChrW(13) & Strings.ChrW(10) &
                                               "@Instance3  INT = NULL" & Strings.ChrW(13) & Strings.ChrW(10) &
                                               "AS" & Strings.ChrW(13) & Strings.ChrW(10) &
                                               ""

                                        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10) & sSelectList.ToString() & Strings.ChrW(13) & Strings.ChrW(10)

                                        m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & sProcedureName & " StoredProcedure", bStoredProcedure:=False)

                                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                            Return gPMConstants.PMEReturnCode.PMFalse
                                        End If

                                        'Set permissions
                                        sSQL = "GRANT EXECUTE ON dbo." & sProcedureName & " TO PUBLIC" & Strings.ChrW(13) & Strings.ChrW(10)

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
                                If v_lSwiftIntegration <> 0 Then
                                    ' dont do this for swift
                                Else
                                    'It's a sum insured

                                    If CDbl(vArray(pbObjectAndPropertyConsts.ACPSpecialsType, lTemp2)) = GISSharedPropertyConstants.ACOSumInsuredTypeID Then

                                        m_lReturn = CType(GenerateSumInsuredFields(v_sDatamodel:=v_sDatamodel, lSumInsuredTypeId:=CInt(vArray(pbObjectAndPropertyConsts.ACPSpecialsTypeReference, lTemp2)), r_oDatabase:=r_oDatabase, v_lPMProductFamily:=v_lPMProductFamily), gPMConstants.PMEReturnCode)

                                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                            Return gPMConstants.PMEReturnCode.PMFalse
                                        End If
                                    End If

                                    If CDbl(vArray(pbObjectAndPropertyConsts.ACPSpecialsType, lTemp2)) = GISSharedPropertyConstants.ACOStdWordingType Then
                                        'It's a standard wording (endorsement / clause)
                                        m_lReturn = CType(GenerateStandardWordingFields(), gPMConstants.PMEReturnCode)

                                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                            Return gPMConstants.PMEReturnCode.PMFalse
                                        End If
                                    End If
                                End If
                            Next lTemp2

                        Case GISDataModelType.GISOTRisk
                            'Only do non quote objects

                            If Not CBool(r_vGISObject(pbObjectAndPropertyConsts.ACOIsQuoteObject, lTemp)) Then

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


                                        If CDbl(r_vGISObject(pbObjectAndPropertyConsts.ACOGISObjectId, lTemp)) <> lParentId Then

                                            sProcedureName = CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOTableName, lTemp)).ToLower()

                                            m_lReturn = CType(GetProcedureName(r_sProcedureName:=sProcedureName, v_sDatamodel:=v_sDatamodel), gPMConstants.PMEReturnCode)

                                            sProcedureNameOldStyle = "sp_wp_" & v_sDatamodel & sProcedureName
                                            sProcedureName = "spg_wp_" & v_sDatamodel & sProcedureName


                                            sTemp = StripDataModelCode(v_vTheString:=CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOTableName, lTemp)), v_sDatamodel:=v_sDatamodel)
                                            sTemp = sTemp.Substring(sTemp.IndexOf("_"c) + 1)


                                            iParentIsMultipleInstance = IsTopLevelParentObjectMultipleInstance(ToSafeString(r_vGISObject(pbObjectAndPropertyConsts.ACOObjectName, lTemp)), r_vGISObject)

                                            sPrefix = New StringBuilder(v_sDatamodel)
                                            While sTemp.IndexOf("_"c) >= 0
                                                lTemp3 = (sTemp.IndexOf("_"c) + 1)
                                                sPrefix.Append(sTemp.Substring(0, 1).ToUpper())
                                                sTemp = sTemp.Substring(lTemp3)
                                            End While

                                        If sTemp <> "" Then
                                            sPrefix.Append(sTemp.Substring(0, 1).ToUpper())
                                        End If

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

                                                    sTableList = New StringBuilder("FROM " & CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOTableName, lTemp)) & " " & sPrefix.ToString() & Strings.ChrW(13) & Strings.ChrW(10))
                                            End Select

                                            ' PW240703 - PS229 - different where clauses required for
                                            ' data model type of Party, because the risk object isn't
                                            ' really a risk object. But that's how it's been done.
                                            If lGISDataModelType = GISDataModelType.GISDMTypeParty Then

                                                sWhereList1 = New StringBuilder("WHERE p.party_cnt = @PartyCnt" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                              "AND gpl.party_cnt = p.party_cnt" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                              "AND gpl.Risk_id is null" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                              "AND pb.gis_policy_link_id = gpl.gis_policy_link_id" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                              "AND " & sPrefix.ToString() & "." & v_sDatamodel & "_policy_binder_id = pb." & v_sDatamodel & "_Policy_binder_id" & Strings.ChrW(13) & Strings.ChrW(10))

                                                sWhereList2 = New StringBuilder(sWhereList1.ToString())
                                            Else

                                                ' PW250703 - PS229 - where list depends on object type
                                                Select Case iObjectType
                                                    Case GISDataModelType.GISOTRisk

                                                        sWhereList1 = New StringBuilder("WHERE ifi.insurance_file_cnt = @InsuranceFileCnt" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                                      "AND gpl.insurance_file_cnt = ifi.insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                                      "AND gpl.Risk_id is null" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                                      "AND pb.gis_policy_link_id = gpl.gis_policy_link_id" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                                      "AND " & sPrefix.ToString() & "." & v_sDatamodel & "_policy_binder_id = pb." & v_sDatamodel & "_Policy_binder_id" & Strings.ChrW(13) & Strings.ChrW(10))
                                                        'PN46453
                                                        If v_sUnderwritingOrAgency = "U" Then
                                                            sWhereList2 = New StringBuilder("WHERE ifi.insurance_file_cnt = @InsuranceFileCnt" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                                          "AND gpl.Risk_id = @RiskId" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                                          "AND pb.gis_policy_link_id = gpl.gis_policy_link_id" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                                          "AND " & sPrefix.ToString() & "." & v_sDatamodel & "_policy_binder_id = pb." & v_sDatamodel & "_Policy_binder_id" & Strings.ChrW(13) & Strings.ChrW(10))
                                                        Else
                                                            sWhereList2 = New StringBuilder("WHERE ifi.insurance_file_cnt = @InsuranceFileCnt" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                                          "AND gpl.insurance_file_cnt = ifi.insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                                          "AND gpl.Risk_id = @RiskId" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                                          "AND pb.gis_policy_link_id = gpl.gis_policy_link_id" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                                          "AND " & sPrefix.ToString() & "." & v_sDatamodel & "_policy_binder_id = pb." & v_sDatamodel & "_Policy_binder_id" & Strings.ChrW(13) & Strings.ChrW(10))
                                                        End If
                                                End Select
                                            End If


                                            sWhereList3 = "ORDER BY " & sPrefix.ToString() & "." & v_sDatamodel & "_policy_binder_id," & sPrefix.ToString() & "." & CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOTableName, lTemp)) & "_id" & Strings.ChrW(13) & Strings.ChrW(10)

                                            'We need the rest of the key Informations...
                                            lLevel = 0
                                            If Informations.IsArray(vArray) Then

                                                For lTemp2 As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)

                                                    If CDbl(vArray(pbObjectAndPropertyConsts.ACPIsIdentifyingProperty, lTemp2)) = 1 Then
                                                        lLevel = lTemp2
                                                    End If
                                                Next lTemp2
                                            End If

                                            'We need to create the Getkeys stored procedure if we are deeper that 1 level
                                            'OR we have more than one instance of an object

                                            If lLevel > 1 Or CDbl(r_vGISObject(pbObjectAndPropertyConsts.ACOMaxInstances, lTemp)) > 1 Then



                                                m_lReturn = CType(GenerateGetKeysStoredProcedure(sTableName:=CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOTableName, lTemp)), sPrefix:=sPrefix.ToString(), v_sDatamodel:=v_sDatamodel, sProcedureName:=sProcedureName, sProcedureNameOldStyle:=sProcedureNameOldStyle, vArray:=vArray, v_iIsParentMultipleInstance:=iParentIsMultipleInstance, r_oDatabase:=r_oDatabase), gPMConstants.PMEReturnCode)

                                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                    Return gPMConstants.PMEReturnCode.PMFalse
                                                End If
                                            End If

                                            '01082002 CMG/PB Corrected error in this If logic level 2 parent
                                            'stored procedures were not being created and this was breaking
                                            'risk looping

                                            If lLevel > 1 Or (CDbl(r_vGISObject(pbObjectAndPropertyConsts.ACOMaxInstances, lTemp)) > 1) Then


                                                m_lReturn = CType(GenerateGetParentStoredProcedure(sTableName:=CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOTableName, lTemp)), sPrefix:=sPrefix.ToString(), v_sDatamodel:=v_sDatamodel, sProcedureName:=sProcedureName, sProcedureNameOldStyle:=sProcedureNameOldStyle, vArray:=vArray, r_oDatabase:=r_oDatabase, v_sUnderwritingOrAgency:=v_sUnderwritingOrAgency), gPMConstants.PMEReturnCode)

                                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                    Return gPMConstants.PMEReturnCode.PMFalse
                                                End If
                                            End If

                                            'We want to ignore all if we're top level (1)
                                            'but include all (but the first) if we're lower level
                                            'so...

                                            If lLevel > 1 Then
                                                lLevel = 0
                                            End If

                                            sSubGroup = ""
                                            sSubGroup2 = ""
                                            sSubGroup3 = ""
                                            sSubGroup4 = ""
                                            sLoop1 = ""
                                            sLoop2 = ""
                                            sLoop3 = ""
                                            sLoop4 = ""
                                            lAddressCount = 0

                                            If Informations.IsArray(vArray) Then


                                                For lTemp2 As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)

                                                    ' PW280703 - PS229 - set flag to indicate if this
                                                    ' field is from a fixed table

                                                    bFixedField = CBool((vArray(pbObjectAndPropertyConsts.ACPEditFlags, lTemp2) And
                                                                       GISSharedPropertyConstants.GISDSEditNoDBColumn))

                                                    If CDbl(vArray(pbObjectAndPropertyConsts.ACPIsIdentifyingProperty, lTemp2)) = 1 Then
                                                        'Move the determination of the group structure to here - only
                                                        'do it once
                                                        sSubGroupTemp = New StringBuilder(v_sDatamodel)
                                                        sTableName = ToSafeString(vArray(pbObjectAndPropertyConsts.ACPColumnName, lTemp2))
                                                        sTemp = StripDataModelCode(v_vTheString:=CStr(vArray(pbObjectAndPropertyConsts.ACPColumnName, lTemp2)), v_sDatamodel:=v_sDatamodel).Trim()

                                                        'Remove the trailing _id
                                                        sTemp = sTemp.Substring(0, sTemp.Length - 3)
                                                        sTableName = sTableName.Substring(0, sTableName.Length - 3)

                                                        While sTemp.IndexOf("_"c) >= 0
                                                            lTemp3 = (sTemp.IndexOf("_"c) + 1)
                                                            sSubGroupTemp.Append(Informations.ProperCase(sTemp.Substring(0, lTemp3 - 1)))
                                                            sTemp = sTemp.Substring(lTemp3)
                                                        End While

                                                        Select Case lTemp2
                                                            Case 0
                                                            Case 1
                                                                sSubGroup = sSubGroupTemp.ToString() & Informations.ProperCase(sTemp)
                                                                'find if parent was actually a  looping object
                                                                If lLevel = 0 Then
                                                                    If iParentIsMultipleInstance = 1 Then
                                                                        sLoop1 = sSubGroup
                                                                    End If
                                                                End If

                                                            Case 2
                                                                sSubGroup2 = sSubGroupTemp.ToString() & Informations.ProperCase(sTemp)
                                                                If lLevel = 0 Then
                                                                    If iParentIsMultipleInstance = 1 Then
                                                                        sLoop2 = sSubGroup2
                                                                    Else
                                                                        sLoop1 = sSubGroup2
                                                                    End If
                                                                End If

                                                            Case 3
                                                                sSubGroup3 = sSubGroupTemp.ToString() & Informations.ProperCase(sTemp)
                                                                If lLevel = 0 Then
                                                                    If iParentIsMultipleInstance = 1 Then
                                                                        sLoop3 = sSubGroup3
                                                                    Else
                                                                        sLoop2 = sSubGroup3
                                                                    End If
                                                                End If

                                                            Case 4
                                                                sSubGroup4 = sSubGroupTemp.ToString() & Informations.ProperCase(sTemp)
                                                                If lLevel = 0 Then
                                                                    If iParentIsMultipleInstance = 1 Then
                                                                        sLoop4 = sSubGroup4
                                                                    Else
                                                                        sLoop3 = sSubGroup4
                                                                    End If
                                                                End If
                                                        End Select

                                                        If lTemp2 > lLevel Then

                                                            sWhereList1.Append("AND " & sPrefix.ToString() & "." & CStr(vArray(pbObjectAndPropertyConsts.ACPColumnName, lTemp2)) & " = @Instance" & CStr(lTemp2 + iParentIsMultipleInstance) & Strings.ChrW(13) & Strings.ChrW(10))

                                                            sWhereList2.Append("AND " & sPrefix.ToString() & "." & CStr(vArray(pbObjectAndPropertyConsts.ACPColumnName, lTemp2)) & " = @Instance" & CStr(lTemp2 + iParentIsMultipleInstance) & Strings.ChrW(13) & Strings.ChrW(10))
                                                            iInstanceCount = lTemp2 + iParentIsMultipleInstance
                                                        End If
                                                    Else

                                                        If CStr(vArray(pbObjectAndPropertyConsts.ACPPropertyName, lTemp2)) <> "dElEtEd" Then

                                                            'if processing first no key attribute
                                                            If lCount = 0 Then
                                                                'if not subloop and object is multiple instance

                                                                If sLoop1 = "" And CDbl(r_vGISObject(pbObjectAndPropertyConsts.ACOMaxInstances, lTemp)) > 1 Then
                                                                    'add where clause for top level multiple instance

                                                                    sWhereList1.Append("AND " & sPrefix.ToString() & "." & CStr(vArray(pbObjectAndPropertyConsts.ACPColumnName, 1)) & " = @Instance2" & Strings.ChrW(13) & Strings.ChrW(10))

                                                                    sWhereList2.Append("AND " & sPrefix.ToString() & "." & CStr(vArray(pbObjectAndPropertyConsts.ACPColumnName, 1)) & " = @Instance2" & Strings.ChrW(13) & Strings.ChrW(10))
                                                                    sLoop1 = sSubGroup
                                                                End If
                                                            End If
                                                            lCount += 1

                                                            ' PW250703 - PS229
                                                            If (lCount = 1) And iObjectType = GISDataModelType.GISOTRisk Then
                                                                sSelect = "SELECT "
                                                            ElseIf sSelectList.ToString() <> "" Then
                                                                'Reset only after formation of select list started
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

                                                                sParameterList.Append("@address" & lAddressCount & "_line_1 VARCHAR(255)," & Strings.ChrW(13) & Strings.ChrW(10))
                                                                sParameterList.Append("@address" & lAddressCount & "_line_2 VARCHAR(255)," & Strings.ChrW(13) & Strings.ChrW(10))
                                                                sParameterList.Append("@address" & lAddressCount & "_line_3 VARCHAR(255)," & Strings.ChrW(13) & Strings.ChrW(10))
                                                                sParameterList.Append("@address" & lAddressCount & "_line_4 VARCHAR(255)," & Strings.ChrW(13) & Strings.ChrW(10))
                                                                sParameterList.Append("@address" & lAddressCount & "_postal_code VARCHAR(255)," & Strings.ChrW(13) & Strings.ChrW(10))
                                                                sParameterList.Append("@address" & lAddressCount & "_country VARCHAR(255)," & Strings.ChrW(13) & Strings.ChrW(10))

                                                                sParameterList.Append("@address" & lAddressCount & "_line_5 VARCHAR(255)," & Strings.ChrW(13) & Strings.ChrW(10))
                                                                sParameterList.Append("@address" & lAddressCount & "_line_6 VARCHAR(255)," & Strings.ChrW(13) & Strings.ChrW(10))
                                                                sParameterList.Append("@address" & lAddressCount & "_line_7 VARCHAR(255)," & Strings.ChrW(13) & Strings.ChrW(10))
                                                                sParameterList.Append("@address" & lAddressCount & "_line_8 VARCHAR(255)," & Strings.ChrW(13) & Strings.ChrW(10))
                                                                sParameterList.Append("@address" & lAddressCount & "_line_9 VARCHAR(255)," & Strings.ChrW(13) & Strings.ChrW(10))
                                                                sParameterList.Append("@address" & lAddressCount & "_line_10 VARCHAR(255)," & Strings.ChrW(13) & Strings.ChrW(10))



                                                                sSelect = ""
                                                                If (sSelectList.ToString().IndexOf("SELECT") + 1) = 0 Then
                                                                    sSelectList = New StringBuilder("SELECT " & sSelectList.ToString())
                                                                End If

                                                                sSelectList.Append(
                                                                                   "address" & CStr(lAddressCount) & "_line_1 = a" & CStr(lAddressCount) & ".address1," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                                                   "address" & CStr(lAddressCount) & "_line_2 = a" & CStr(lAddressCount) & ".address2," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                                                   "address" & CStr(lAddressCount) & "_line_3 = a" & CStr(lAddressCount) & ".address3," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                                                   "address" & CStr(lAddressCount) & "_line_4 = a" & CStr(lAddressCount) & ".address4," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                                                   "case a" & CStr(lAddressCount) & ".postal_code when convert(varchar(10), a" & CStr(lAddressCount) & ".address_id) then '' else a" & CStr(lAddressCount) & ".postal_code end as address" & CStr(lAddressCount) & "_postal_code," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                                                   "address" & CStr(lAddressCount) & "_country = c" & CStr(lAddressCount) & ".description," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                                                   "address" & CStr(lAddressCount) & "_line_5 = a" & CStr(lAddressCount) & ".address5," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                                                   "address" & CStr(lAddressCount) & "_line_6 = a" & CStr(lAddressCount) & ".address6," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                                                   "address" & CStr(lAddressCount) & "_line_7 = a" & CStr(lAddressCount) & ".address7," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                                                   "address" & CStr(lAddressCount) & "_line_8 = a" & CStr(lAddressCount) & ".address8," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                                                   "address" & CStr(lAddressCount) & "_line_9 = a" & CStr(lAddressCount) & ".address9," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                                                   "address" & CStr(lAddressCount) & "_line_10 = a" & CStr(lAddressCount) & ".address10," & Strings.ChrW(13) & Strings.ChrW(10))






                                                                sTableList.Append("LEFT JOIN address a" & lAddressCount & " ON " &
                                                                                  sPrefix.ToString() & "." & CStr(vArray(pbObjectAndPropertyConsts.ACPColumnName, lTemp2)) & " = a" & CStr(lAddressCount) & ".address_cnt" & Strings.ChrW(13) & Strings.ChrW(10))
                                                                sTableList.Append("LEFT JOIN country c" & lAddressCount & " ON " &
                                                                                  "a" & CStr(lAddressCount) & ".country_id = c" & CStr(lAddressCount) & ".country_id" & Strings.ChrW(13) & Strings.ChrW(10))

                                                                For lTemp3 = 1 To 10



                                                                    m_lReturn = CType(AddToWPFields(sFieldName:=sPrefix.ToString() & "Address" & CStr(lAddressCount) & "Line" & CStr(lTemp3),
                                                                                                    sSQL:=sProcedureName, sColumnName:="address" & lAddressCount & "_line_" & CStr(lTemp3),
                                                                                                    lColumnType:=gPMConstants.PMEFormatStyle.PMFormatString, sMainGroup:=sMainGroup,
                                                                                                    sSubGroup:=sSubGroup, sSubGroup2:=sSubGroup2, sSubGroup3:=sSubGroup3, sSubGroup4:=sSubGroup4,
                                                                                                    sDisplayname:=CStr(vArray(pbObjectAndPropertyConsts.ACPPropertyName, lTemp2)).Trim() & " Line " & CStr(lTemp3),
                                                                                                    iIsDisplayed:=1, sLoop1:=sLoop1, sLoop2:=sLoop2, sLoop3:=sLoop3, sLoop4:=sLoop4, lProductFamily:=v_lPMProductFamily,
                                                                                                    vDataModel:=If(Len(v_sDatamodel) > 0, v_sDatamodel, DBNull.Value), vPropertyId:=DBNull.Value,
                                                                                                    lSpecialsType:=CInt(vArray(pbObjectAndPropertyConsts.ACPSpecialsType, lTemp2)),
                                                                                                    r_oDatabase:=r_oDatabase, sTableName:=sTableName), gPMConstants.PMEReturnCode)

                                                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                                        Return gPMConstants.PMEReturnCode.PMFalse
                                                                    End If
                                                                Next lTemp3




                                                                m_lReturn = CType(AddToWPFields(sFieldName:=sPrefix.ToString() & "Address" & CStr(lAddressCount) & "PostalCode", sSQL:=sProcedureName,
                                                                                                sColumnName:="address" & lAddressCount & "_postal_code", lColumnType:=gPMConstants.PMEFormatStyle.PMFormatString,
                                                                                                sMainGroup:=sMainGroup, sSubGroup:=sSubGroup, sSubGroup2:=sSubGroup2, sSubGroup3:=sSubGroup3,
                                                                                                sSubGroup4:=sSubGroup4, sDisplayname:=CStr(vArray(pbObjectAndPropertyConsts.ACPPropertyName, lTemp2)).Trim() & " Postal Code",
                                                                                                iIsDisplayed:=1, sLoop1:=sLoop1, sLoop2:=sLoop2, sLoop3:=sLoop3, sLoop4:=sLoop4, lProductFamily:=v_lPMProductFamily,
                                                                                                vDataModel:=If(Len(v_sDatamodel) > 0, v_sDatamodel, DBNull.Value), vPropertyId:=DBNull.Value,
                                                                                                lSpecialsType:=CInt(vArray(pbObjectAndPropertyConsts.ACPSpecialsType, lTemp2)),
                                                                                                r_oDatabase:=r_oDatabase, sTableName:=sTableName), gPMConstants.PMEReturnCode)

                                                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                                    Return gPMConstants.PMEReturnCode.PMFalse
                                                                End If




                                                                m_lReturn = CType(AddToWPFields(sFieldName:=sPrefix.ToString() & "Address" & CStr(lAddressCount) & "Country", sSQL:=sProcedureName, sColumnName:="address" & lAddressCount & "_country",
                                                                                                lColumnType:=gPMConstants.PMEFormatStyle.PMFormatString, sMainGroup:=sMainGroup, sSubGroup:=sSubGroup,
                                                                                                sSubGroup2:=sSubGroup2, sSubGroup3:=sSubGroup3, sSubGroup4:=sSubGroup4,
                                                                                                sDisplayname:=CStr(vArray(pbObjectAndPropertyConsts.ACPPropertyName, lTemp2)).Trim() & " Country",
                                                                                                iIsDisplayed:=1, sLoop1:=sLoop1, sLoop2:=sLoop2, sLoop3:=sLoop3, sLoop4:=sLoop4,
                                                                                                lProductFamily:=v_lPMProductFamily, vDataModel:=If(Len(v_sDatamodel) > 0, v_sDatamodel, DBNull.Value),
                                                                                                vPropertyId:=DBNull.Value, lSpecialsType:=CInt(vArray(pbObjectAndPropertyConsts.ACPSpecialsType, lTemp2)),
                                                                                                r_oDatabase:=r_oDatabase, sTableName:=sTableName), gPMConstants.PMEReturnCode)

                                                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                                    Return gPMConstants.PMEReturnCode.PMFalse
                                                                End If
                                                            ElseIf CDbl(vArray(pbObjectAndPropertyConsts.ACPSpecialsType, lTemp2)) = GISSharedPropertyConstants.ACOPartyTypeID And Not bFixedField Then
                                                                'If it's a party, need a link to the party table
                                                                sParameterList.Append("@" & sColumnName & " VARCHAR(255)," & Strings.ChrW(13) & Strings.ChrW(10))

                                                                lOtherTableCount += 1

                                                                sSelectList.Append(
                                                                                   sSelect & sColumnName & " = p" & CStr(lOtherTableCount) & ".name," & Strings.ChrW(13) & Strings.ChrW(10))

                                                                sTableList.Append("LEFT JOIN party p" & lOtherTableCount & " ON " &
                                                                                  sPrefix.ToString() & "." & sColumnName & " = p" & CStr(lOtherTableCount) & ".party_cnt" & Strings.ChrW(13) & Strings.ChrW(10))



                                                                m_lReturn = CType(AddToWPFields(sFieldName:=sPrefix.ToString() & sFieldName.ToString(), sSQL:=sProcedureName, sColumnName:=sColumnName,
                                                                                                lColumnType:=gPMConstants.PMEFormatStyle.PMFormatString, sMainGroup:=sMainGroup, sSubGroup:=sSubGroup,
                                                                                                sSubGroup2:=sSubGroup2, sSubGroup3:=sSubGroup3, sSubGroup4:=sSubGroup4, sDisplayname:=sDisplayname.ToString(),
                                                                                                iIsDisplayed:=1, sLoop1:=sLoop1, sLoop2:=sLoop2, sLoop3:=sLoop3, sLoop4:=sLoop4, lProductFamily:=v_lPMProductFamily,
                                                                                                vDataModel:=If(Len(v_sDatamodel) > 0, v_sDatamodel, DBNull.Value), vPropertyId:=DBNull.Value,
                                                                                                lSpecialsType:=CInt(vArray(pbObjectAndPropertyConsts.ACPSpecialsType, lTemp2)), r_oDatabase:=r_oDatabase,
                                                                                                sTableName:=sTableName), gPMConstants.PMEReturnCode)

                                                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                                    Return gPMConstants.PMEReturnCode.PMFalse
                                                                End If
                                                            ElseIf (CDbl(vArray(pbObjectAndPropertyConsts.ACPSpecialsType, lTemp2)) = GISSharedPropertyConstants.ACOPMLookupTableName Or CDbl(vArray(pbObjectAndPropertyConsts.ACPSpecialsType, lTemp2)) = GISSharedPropertyConstants.ACOComboLookup) And Not bFixedField Then
                                                                'If it's a lookup, need a link to the lookup
                                                                sParameterList.Append("@" & sColumnName & " VARCHAR(255)," & Strings.ChrW(13) & Strings.ChrW(10))

                                                                lOtherTableCount += 1

                                                                sSelectList.Append(
                                                                                   sSelect & sColumnName & " = l" & CStr(lOtherTableCount) & ".description," & Strings.ChrW(13) & Strings.ChrW(10))




                                                                sTableList.Append("LEFT JOIN " & CStr(vArray(pbObjectAndPropertyConsts.ACPSpecialsTypeReference, lTemp2)) & " l" & CStr(lOtherTableCount) & " ON " &
                                                                                  sPrefix.ToString() & "." & CStr(vArray(pbObjectAndPropertyConsts.ACPColumnName, lTemp2)) & " = " &
                                                                                  "l" & CStr(lOtherTableCount) & "." & CStr(vArray(pbObjectAndPropertyConsts.ACPSpecialsTypeReference, lTemp2)) & "_id" & Strings.ChrW(13) & Strings.ChrW(10))



                                                                m_lReturn = CType(AddToWPFields(sFieldName:=sPrefix.ToString() & sFieldName.ToString(), sSQL:=sProcedureName, sColumnName:=sColumnName,
                                                                                                lColumnType:=gPMConstants.PMEFormatStyle.PMFormatString, sMainGroup:=sMainGroup, sSubGroup:=sSubGroup,
                                                                                                sSubGroup2:=sSubGroup2, sSubGroup3:=sSubGroup3, sSubGroup4:=sSubGroup4, sDisplayname:=sDisplayname.ToString(),
                                                                                                iIsDisplayed:=1, sLoop1:=sLoop1, sLoop2:=sLoop2, sLoop3:=sLoop3, sLoop4:=sLoop4,
                                                                                                lProductFamily:=v_lPMProductFamily, vDataModel:=If(Len(v_sDatamodel) > 0, v_sDatamodel, DBNull.Value),
                                                                                                vPropertyId:=DBNull.Value, lSpecialsType:=CInt(vArray(pbObjectAndPropertyConsts.ACPSpecialsType, lTemp2)),
                                                                                                r_oDatabase:=r_oDatabase, sTableName:=sTableName), gPMConstants.PMEReturnCode)

                                                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                                    Return gPMConstants.PMEReturnCode.PMFalse
                                                                End If
                                                            ElseIf CInt(vArray(pbObjectAndPropertyConsts.ACPSpecialsType, lTemp2)) = GISSharedPropertyConstants.ACOGISUserDefHeaderID And Not bFixedField Then
                                                                'If it's one of our lookups, need a link to our lookup
                                                                sParameterList.Append("@" & sColumnName & " VARCHAR(255)," & Strings.ChrW(13) & Strings.ChrW(10))

                                                                lOtherTableCount += 1

                                                                sSelectList.Append(
                                                                                   sSelect & sColumnName & " = l" & CStr(lOtherTableCount) & ".description," & Strings.ChrW(13) & Strings.ChrW(10))


                                                                sTableList.Append("LEFT JOIN GIS_user_def_detail l" & lOtherTableCount & " ON " &
                                                                                  sPrefix.ToString() & "." & CStr(vArray(pbObjectAndPropertyConsts.ACPColumnName, lTemp2)) & " = " &
                                                                                  "l" & CStr(lOtherTableCount) & ".GIS_user_def_detail_id" & Strings.ChrW(13) & Strings.ChrW(10))



                                                                m_lReturn = CType(AddToWPFields(sFieldName:=sPrefix.ToString() & sFieldName.ToString(), sSQL:=sProcedureName, sColumnName:=sColumnName,
                                                                                                lColumnType:=gPMConstants.PMEFormatStyle.PMFormatString, sMainGroup:=sMainGroup, sSubGroup:=sSubGroup,
                                                                                                sSubGroup2:=sSubGroup2, sSubGroup3:=sSubGroup3, sSubGroup4:=sSubGroup4, sDisplayname:=sDisplayname.ToString(),
                                                                                                iIsDisplayed:=1, sLoop1:=sLoop1, sLoop2:=sLoop2, sLoop3:=sLoop3, sLoop4:=sLoop4,
                                                                                                lProductFamily:=v_lPMProductFamily, vDataModel:=If(Len(v_sDatamodel) > 0, v_sDatamodel, DBNull.Value),
                                                                                                vPropertyId:=DBNull.Value, lSpecialsType:=CInt(vArray(pbObjectAndPropertyConsts.ACPSpecialsType, lTemp2)),
                                                                                                r_oDatabase:=r_oDatabase, sTableName:=sTableName), gPMConstants.PMEReturnCode)

                                                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                                    Return gPMConstants.PMEReturnCode.PMFalse
                                                                End If
                                                            ElseIf CDbl(vArray(pbObjectAndPropertyConsts.ACPSpecialsType, lTemp2)) = GISSharedPropertyConstants.ACOProductID And Not bFixedField Then
                                                                'If it's a product, need a link to a policy...
                                                                sParameterList.Append("@" & sColumnName & " VARCHAR(255)," & Strings.ChrW(13) & Strings.ChrW(10))

                                                                lOtherTableCount += 1

                                                                sSelectList.Append(
                                                                                   sSelect & sColumnName & " = i" & CStr(lOtherTableCount) & ".insurance_ref," & Strings.ChrW(13) & Strings.ChrW(10))

                                                                sTableList.Append("LEFT JOIN insurance_file i" & lOtherTableCount & " ON " &
                                                                                  sPrefix.ToString() & "." & sColumnName & " = i" & CStr(lOtherTableCount) & ".insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10))



                                                                m_lReturn = CType(AddToWPFields(sFieldName:=sPrefix.ToString() & sFieldName.ToString(), sSQL:=sProcedureName, sColumnName:=sColumnName,
                                                                                                lColumnType:=gPMConstants.PMEFormatStyle.PMFormatString, sMainGroup:=sMainGroup, sSubGroup:=sSubGroup,
                                                                                                sSubGroup2:=sSubGroup2, sSubGroup3:=sSubGroup3, sSubGroup4:=sSubGroup4, sDisplayname:=sDisplayname.ToString(),
                                                                                                iIsDisplayed:=1, sLoop1:=sLoop1, sLoop2:=sLoop2, sLoop3:=sLoop3, sLoop4:=sLoop4, lProductFamily:=v_lPMProductFamily,
                                                                                                vDataModel:=If(Len(v_sDatamodel) > 0, v_sDatamodel, DBNull.Value), vPropertyId:=DBNull.Value,
                                                                                                lSpecialsType:=CInt(vArray(pbObjectAndPropertyConsts.ACPSpecialsType, lTemp2)), r_oDatabase:=r_oDatabase,
                                                                                                sTableName:=sTableName), gPMConstants.PMEReturnCode)

                                                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                                    Return gPMConstants.PMEReturnCode.PMFalse
                                                                End If
                                                            ElseIf CDbl(vArray(pbObjectAndPropertyConsts.ACPSpecialsType, lTemp2)) = GISSharedPropertyConstants.ACOStdWordingType And Not bFixedField Then

                                                                sParameterList.Append("@" & sColumnName & " VARCHAR(255)," & vbCrLf)
                                                                m_lReturn = CType(AddToWPFields(sFieldName:=sPrefix.ToString() & sFieldName.ToString(), sSQL:=sProcedureName, sColumnName:=sColumnName,
                                                                                                lColumnType:=gPMConstants.PMEFormatStyle.PMFormatString, sMainGroup:=sMainGroup, sSubGroup:=sSubGroup,
                                                                                                sSubGroup2:=sSubGroup2, sSubGroup3:=sSubGroup3, sSubGroup4:=sSubGroup4, sDisplayname:=sDisplayname.ToString(),
                                                                                                iIsDisplayed:=1, sLoop1:=sLoop1, sLoop2:=sLoop2, sLoop3:=sLoop3, sLoop4:=sLoop4, lProductFamily:=v_lPMProductFamily,
                                                                                                vDataModel:=If(Len(v_sDatamodel) > 0, v_sDatamodel, DBNull.Value), vPropertyId:=DBNull.Value,
                                                                                                lSpecialsType:=CInt(vArray(pbObjectAndPropertyConsts.ACPSpecialsType, lTemp2)),
                                                                                                r_oDatabase:=r_oDatabase, sTableName:=sTableName), gPMConstants.PMEReturnCode)

                                                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                                    Return gPMConstants.PMEReturnCode.PMFalse
                                                                End If

                                                                If sMainGroup = "Risk" Then
                                                                    m_lReturn = CType(AddToWPFields(sFieldName:=sPrefix.ToString() & sFieldName.ToString() & "_SWDESC", sSQL:=sProcedureName, sColumnName:="SWDESC",
                                                                                                    lColumnType:=gPMConstants.PMEFormatStyle.PMFormatString, sMainGroup:=sMainGroup, sSubGroup:=sSubGroup,
                                                                                                    sSubGroup2:=sSubGroup2, sSubGroup3:=sSubGroup3, sSubGroup4:=sSubGroup4, sDisplayname:=sPrefix.ToString() & sFieldName.ToString() & " Description",
                                                                                                    iIsDisplayed:=1, sLoop1:=sLoop1, sLoop2:=sLoop2, sLoop3:=sLoop3, sLoop4:=sLoop4, lProductFamily:=v_lPMProductFamily, vDataModel:=v_sDatamodel,
                                                                                                    vPropertyId:=DBNull.Value, lSpecialsType:=CInt(vArray(pbObjectAndPropertyConsts.ACPSpecialsType, lTemp2)),
                                                                                                    r_oDatabase:=r_oDatabase, sTableName:=sTableName), gPMConstants.PMEReturnCode)

                                                                    m_lReturn = CType(AddToWPFields(sFieldName:=sPrefix.ToString() & sFieldName.ToString() & "_SWCODE", sSQL:=sProcedureName, sColumnName:="SWCODE",
                                                                                                    lColumnType:=gPMConstants.PMEFormatStyle.PMFormatString, sMainGroup:=sMainGroup, sSubGroup:=sSubGroup,
                                                                                                    sSubGroup2:=sSubGroup2, sSubGroup3:=sSubGroup3, sSubGroup4:=sSubGroup4, sDisplayname:=sPrefix.ToString() & sFieldName.ToString() & " Code",
                                                                                                    iIsDisplayed:=1, sLoop1:=sLoop1, sLoop2:=sLoop2, sLoop3:=sLoop3, sLoop4:=sLoop4, lProductFamily:=v_lPMProductFamily, vDataModel:=v_sDatamodel,
                                                                                                    vPropertyId:=DBNull.Value, lSpecialsType:=CInt(vArray(pbObjectAndPropertyConsts.ACPSpecialsType, lTemp2)),
                                                                                                    r_oDatabase:=r_oDatabase, sTableName:=sTableName), gPMConstants.PMEReturnCode)
                                                                End If

                                                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                                    Return gPMConstants.PMEReturnCode.PMFalse
                                                                End If
                                                                'else it's bog standard


                                                            Else

                                                                ' PW250703 - PS229  - don't add to
                                                                ' select list if field is from a fixed
                                                                ' table
                                                                'If Not bFixedField Then
                                                                sParameterList.Append("@" & sColumnName & " ")
                                                                sSelectList.Append(
                                                                                   sSelect & sColumnName & " = " & sPrefix.ToString() & "." & sColumnName & "," & Strings.ChrW(13) & Strings.ChrW(10))

                                                                Select Case vArray(pbObjectAndPropertyConsts.ACPDataType, lTemp2)
                                                                    Case GISSharedConstants.GISDataTypeComment
                                                                        sParameterList.Append("VARCHAR(4000)," & Strings.ChrW(13) & Strings.ChrW(10))
                                                                        'sParameterList = sParameterList & "TEXT," & vbCrLf
                                                                        lFormat = gPMConstants.PMEFormatStyle.PMFormatString

                                                                    Case GISSharedConstants.GISDataTypeText
                                                                        sTemp = "255"
                                                                        sParameterList.Append("VARCHAR(" & sTemp & ")," & Strings.ChrW(13) & Strings.ChrW(10))
                                                                        lFormat = gPMConstants.PMEFormatStyle.PMFormatString

                                                                    Case GISSharedConstants.GISDataTypeDate
                                                                        sParameterList.Append("DATETIME," & Strings.ChrW(13) & Strings.ChrW(10))
                                                                        lFormat = gPMConstants.PMEFormatStyle.PMFormatDateLong

                                                                    Case GISSharedConstants.GISDataTypeCurrency
                                                                        sParameterList.Append("NUMERIC(19,4)," & Strings.ChrW(13) & Strings.ChrW(10))
                                                                        lFormat = gPMConstants.PMEFormatStyle.PMFormatCurrency

                                                                    Case GISSharedConstants.GISDataTypeInteger
                                                                        sParameterList.Append("INT," & Strings.ChrW(13) & Strings.ChrW(10))
                                                                        lFormat = gPMConstants.PMEFormatStyle.PMFormatLong

                                                                    Case GISSharedConstants.GISDataTypePercentage
                                                                        sParameterList.Append("NUMERIC(19,4)," & Strings.ChrW(13) & Strings.ChrW(10))
                                                                        lFormat = gPMConstants.PMEFormatStyle.PMFormatPercent

                                                                    Case GISSharedConstants.GISDataTypeNumeric
                                                                        sParameterList.Append("NUMERIC(19,4)," & Strings.ChrW(13) & Strings.ChrW(10))
                                                                        lFormat = gPMConstants.PMEFormatStyle.PMFormatLong

                                                                    Case GISSharedConstants.GISDataTypeOption
                                                                        sParameterList.Append("TINYINT," & Strings.ChrW(13) & Strings.ChrW(10))
                                                                        lFormat = gPMConstants.PMEFormatStyle.PMFormatBoolean

                                                                    Case Else
                                                                        sParameterList.Append("VARCHAR(255)," & Strings.ChrW(13) & Strings.ChrW(10))
                                                                        lFormat = gPMConstants.PMEFormatStyle.PMFormatString
                                                                End Select

                                                                sFinalSelectList.Append(
                                                                                        "'" & sColumnName & "' = @" & sColumnName & "," & Strings.ChrW(13) & Strings.ChrW(10))

                                                                'Add the record to wp_fields
                                                                ' PW240703 - PS229 - use a main type
                                                                ' of party for data model type of Party,
                                                                ' because the risk object isn't
                                                                ' really a risk object. But that's how
                                                                ' it's been done.


                                                                m_lReturn = CType(AddToWPFields(sFieldName:=sPrefix.ToString() & sFieldName.ToString(), sSQL:=sProcedureName, sColumnName:=sColumnName,
                                                                                                lColumnType:=lFormat, sMainGroup:=sMainGroup, sSubGroup:=sSubGroup, sSubGroup2:=sSubGroup2,
                                                                                                sSubGroup3:=sSubGroup3, sSubGroup4:=sSubGroup4, sDisplayname:=sDisplayname.ToString(), iIsDisplayed:=1,
                                                                                                sLoop1:=sLoop1, sLoop2:=sLoop2, sLoop3:=sLoop3, sLoop4:=sLoop4, lProductFamily:=v_lPMProductFamily,
                                                                                                vDataModel:=If(Len(v_sDatamodel) > 0, v_sDatamodel, DBNull.Value), vPropertyId:=DBNull.Value,
                                                                                                lSpecialsType:=CInt(vArray(pbObjectAndPropertyConsts.ACPSpecialsType, lTemp2)),
                                                                                                r_oDatabase:=r_oDatabase, sTableName:=sTableName), gPMConstants.PMEReturnCode)

                                                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                                    Return gPMConstants.PMEReturnCode.PMFalse
                                                                End If
                                                                'End If
                                                            End If
                                                        End If
                                                    End If
                                                Next lTemp2

                                            End If

                                            If sParameterList.ToString() <> "" Then
                                                'Get rid of the trailing comma and vbCrLf
                                                sParameterList = New StringBuilder(sParameterList.ToString().Substring(0, sParameterList.ToString().Length - 3) & Strings.ChrW(13) & Strings.ChrW(10))
                                                sFinalSelectList = New StringBuilder(sFinalSelectList.ToString().Substring(0, sFinalSelectList.ToString().Length - 3) & Strings.ChrW(13) & Strings.ChrW(10))

                                                'Add the rest...
                                                ' PW250703 - PS229 - depending on what the object type
                                                ' is
                                                Select Case iObjectType
                                                    Case GISDataModelType.GISOTRisk
                                                        sTableList = New StringBuilder(sTableList.ToString().Substring(0, sTableList.ToString().Length - 2) & ", " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                                     v_sDatamodel & "_Policy_binder pb," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                                     "GIS_policy_link gpl," & Strings.ChrW(13) & Strings.ChrW(10))
                                                        ' PW240703 - PS229 - party table required for
                                                        ' data model type of Party, because the risk object isn't
                                                        ' really a risk object. But that's how it's been done.
                                                        If lGISDataModelType = GISDataModelType.GISDMTypeParty Then
                                                            sTableList.Append("party p" & Strings.ChrW(13) & Strings.ChrW(10))
                                                        Else
                                                            sTableList.Append("insurance_file ifi" & Strings.ChrW(13) & Strings.ChrW(10))
                                                        End If
                                                End Select
                                                'Drop it if it's already there
                                                DropExistingProcedure(v_sProcedureName:=sProcedureName, v_sPostFix:="", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

                                                DropExistingProcedure(v_sProcedureName:=sProcedureNameOldStyle, v_sPostFix:="", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

                                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                    Return gPMConstants.PMEReturnCode.PMFalse
                                                End If

                                                If sSelectList.ToString() <> "" Then
                                                    'remove the last comma and vbcrlf
                                                    sSelectList = New StringBuilder(sSelectList.ToString().Substring(0, sSelectList.ToString().Length - 3) & Strings.ChrW(13) & Strings.ChrW(10))

                                                    ' PW240703 - PS229 - don't need risk parameter for
                                                    ' data model type of Party, because we're creating
                                                    ' it in the party section of field manager.
                                                    If lGISDataModelType <> GISDataModelType.GISDMTypeParty Then
                                                        sSelectList = New StringBuilder("If @RiskId Is Null" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                                      sSelectList.ToString() & sTableList.ToString() & sWhereList1.ToString() & sWhereList3 &
                                                                      "Else" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                                      sSelectList.ToString() & sTableList.ToString() & sWhereList2.ToString() & sWhereList3)
                                                    Else
                                                        sSelectList.Append(sTableList.ToString() & sWhereList1.ToString() & sWhereList3)
                                                    End If
                                                    'Create the new procedure
                                                    sSQL = ""

                                                    ' The Risk parameter is added but not used by Party procedures.
                                                    ' This is because all wp procedures must have the same parameter list.
                                                    sSQL = sSQL & "CREATE PROCEDURE " & sProcedureName & Strings.ChrW(13) & Strings.ChrW(10) &
                                                           "@PartyCnt INT," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                           "@InsuranceFileCnt INT," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                           "@RiskId INT = NULL," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                           "@ClaimCnt INT," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                           "@DocumentRef VARCHAR(25)"

                                                    'dynamically generate instance variables
                                                    generateInstanceVariables(sSQL, iInstanceCount)

                                                    sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10) & "AS" & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)
                                                    sSQL = sSQL & sSelectList.ToString() & Strings.ChrW(13) & Strings.ChrW(10)

                                                    m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & sProcedureName & " StoredProcedure", bStoredProcedure:=False)

                                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                        Return gPMConstants.PMEReturnCode.PMFalse
                                                    End If

                                                    'Set permissions

                                                    sSQL = "GRANT EXECUTE ON dbo." & sProcedureName & " TO PUBLIC" & Strings.ChrW(13) & Strings.ChrW(10)

                                                    m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Grant " & sProcedureName & " StoredProcedure", bStoredProcedure:=False)

                                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                        Return gPMConstants.PMEReturnCode.PMFalse
                                                    End If

                                                End If
                                            End If
                                        End If
                                    End If ' CLAIM RISK OR NORMAL RISK
                                End If
                            End If
                        Case GISDataModelType.GISOTCase

                            'Only do non quote objects

                            If Not CBool(r_vGISObject(pbObjectAndPropertyConsts.ACOIsQuoteObject, lTemp)) Then

                                '**********************************************************************
                                '**** CASE OBJECT ***********************************************
                                '**********************************************************************
                                'If v_lGisDataModelType = GISDMTypeCase Then

                                ' dont create wp fields objects for the ad-hoc document request tables

                                ' get object position within array
                                lObject = lTemp

                                If GenerateCaseSP(v_lObjectId:=lObject, v_vGISObject:=r_vGISObject, v_vGISProperty:=r_vGISProperty, v_lParentId:=lParentId, v_sDatamodel:=v_sDatamodel, v_lPMProductFamily:=v_lPMProductFamily, r_oDatabase:=r_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then

                                    Return gPMConstants.PMEReturnCode.PMFalse

                                End If

                            End If
                    End Select

                End If
                'PN25916
            Next lTemp

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GenerateStoredProcedure Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateStoredProcedure", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub generateInstanceVariables(ByRef sSQL As String, ByVal lLevel As Integer)


        If lLevel < 3 Then lLevel = 3
        For lCount As Integer = 1 To lLevel
            sSQL = sSQL & "," & Strings.ChrW(13) & Strings.ChrW(10) & "@Instance" & CStr(lCount) & " INT = NULL"
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
        Dim sTableName As String = String.Empty

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sPrefix = v_sDatamodel & "SI"

            sDescription = ""

            If Not Informations.IsArray(m_vSumInsured) Then
                m_lReturn = CType(GetALookup(iLanguageID:=1, sTableName:="sum_insured_type", vArray:=m_vSumInsured, r_oDatabase:=r_oDatabase), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            If Informations.IsArray(m_vSumInsured) Then

                For lTemp As Integer = m_vSumInsured.GetLowerBound(1) To m_vSumInsured.GetUpperBound(1)

                    If CDbl(m_vSumInsured(0, lTemp)) = lSumInsuredTypeId Then

                        sDescription = CStr(m_vSumInsured(1, lTemp)) & " SI"
                        Exit For
                    End If
                Next lTemp
            End If

            'The get keys procedure...

            sProcedureNameOldStyle = "sp_wp_" & v_sDatamodel & "SumInsured" & CStr(lSumInsuredTypeId) & "_get_keys"
            sProcedureName = "spg_wp_" & v_sDatamodel & "SumInsured" & CStr(lSumInsuredTypeId) & "_get_keys"

            sSelectList = "SELECT " & sPrefix & ".sequence_id" & Strings.ChrW(13) & Strings.ChrW(10)

            sTableList = "FROM " & v_sDatamodel & "_sum_insured " & sPrefix & ", " & Strings.ChrW(13) & Strings.ChrW(10) &
                         v_sDatamodel & "_Policy_binder pb," & Strings.ChrW(13) & Strings.ChrW(10) &
                         "GIS_policy_link gpl," & Strings.ChrW(13) & Strings.ChrW(10) &
                         "insurance_file ifi" & Strings.ChrW(13) & Strings.ChrW(10)

            sWhereList1 = "WHERE ifi.insurance_file_cnt = @InsuranceFileCnt" & Strings.ChrW(13) & Strings.ChrW(10) &
                          "AND gpl.insurance_file_cnt = ifi.insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10) &
                          "AND gpl.Risk_id is null" & Strings.ChrW(13) & Strings.ChrW(10) &
                          "AND pb.gis_policy_link_id = gpl.gis_policy_link_id" & Strings.ChrW(13) & Strings.ChrW(10) &
                          "AND " & sPrefix & "." & v_sDatamodel & "_policy_binder_id = pb." & v_sDatamodel & "_Policy_binder_id" & Strings.ChrW(13) & Strings.ChrW(10) &
                          "AND " & sPrefix & ".sum_insured_type_id = " & CStr(lSumInsuredTypeId) & Strings.ChrW(13) & Strings.ChrW(10) &
                          "AND " & sPrefix & ".sum_insured IS NOT NULL" & Strings.ChrW(13) & Strings.ChrW(10) &
                          "AND ISNULL(" & sPrefix & ".date_deleted, '1899-12-29') = '1899-12-29'" & Strings.ChrW(13) & Strings.ChrW(10)

            sWhereList2 = "WHERE ifi.insurance_file_cnt = @InsuranceFileCnt" & Strings.ChrW(13) & Strings.ChrW(10) &
                          "AND gpl.Risk_id = @RiskId" & Strings.ChrW(13) & Strings.ChrW(10) &
                          "AND pb.gis_policy_link_id = gpl.gis_policy_link_id" & Strings.ChrW(13) & Strings.ChrW(10) &
                          "AND " & sPrefix & "." & v_sDatamodel & "_policy_binder_id = pb." & v_sDatamodel & "_Policy_binder_id" & Strings.ChrW(13) & Strings.ChrW(10) &
                          "AND " & sPrefix & ".sum_insured_type_id = " & CStr(lSumInsuredTypeId) & Strings.ChrW(13) & Strings.ChrW(10) &
                          "AND " & sPrefix & ".sum_insured IS NOT NULL" & Strings.ChrW(13) & Strings.ChrW(10) &
                          "AND ISNULL(" & sPrefix & ".date_deleted, '1899-12-29') = '1899-12-29'" & Strings.ChrW(13) & Strings.ChrW(10)

            'Drop it if it's already there
            DropExistingProcedure(sProcedureName, "", m_lReturn, r_oDatabase)
            DropExistingProcedure(sProcedureNameOldStyle, "", m_lReturn, r_oDatabase)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sSelectList = "If @RiskId Is Null" & Strings.ChrW(13) & Strings.ChrW(10) &
                          sSelectList & sTableList & sWhereList1 &
                          "Else" & Strings.ChrW(13) & Strings.ChrW(10) &
                          sSelectList & sTableList & sWhereList2

            'Create the new procedure
            sSQL = ""

            sSQL = sSQL & "CREATE PROCEDURE " & sProcedureName & " @PartyCnt INT," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "@InsuranceFileCnt INT," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "@RiskId INT = NULL," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "@ClaimCnt INT," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "@DocumentRef VARCHAR(25)," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "@Instance1 INT," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "@Instance2 INT," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "@Instance3 INT" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "AS" & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & sSelectList & Strings.ChrW(13) & Strings.ChrW(10)

            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & sProcedureName & " StoredProcedure", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set permissions

            sSQL = "GRANT EXECUTE ON dbo." & sProcedureName & " TO PUBLIC" & Strings.ChrW(13) & Strings.ChrW(10)

            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Grant " & sProcedureName & " StoredProcedure", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'The get parent key procedure - needed because of the way DP now works...
            sProcedureName = "spg_wp_" & v_sDatamodel & "SumInsured" & CStr(lSumInsuredTypeId) & "_get_parent_key"

            'Doesn't need to do a lot, just exist and pass back something that's ignored anyway
            sSelectList = "SELECT 1" & Strings.ChrW(13) & Strings.ChrW(10)

            sTableList = ""

            sWhereList1 = ""

            sWhereList2 = ""

            'Drop it if it's already there
            DropExistingProcedure(sProcedureName, "", m_lReturn, r_oDatabase)
            DropExistingProcedure(sProcedureNameOldStyle, "", m_lReturn, r_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sSelectList = "If @RiskId Is Null" & Strings.ChrW(13) & Strings.ChrW(10) &
                          sSelectList & sTableList & sWhereList1 &
                          "Else" & Strings.ChrW(13) & Strings.ChrW(10) &
                          sSelectList & sTableList & sWhereList2

            'Create the new procedure
            sSQL = ""

            sSQL = sSQL & "CREATE PROCEDURE " & sProcedureName & " @PartyCnt INT," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "@InsuranceFileCnt INT," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "@RiskId INT = NULL," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "@ClaimCnt INT," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "@DocumentRef VARCHAR(25)," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "@Instance1 INT," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "@Instance2 INT," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "@Instance3 INT" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "AS" & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & sSelectList & Strings.ChrW(13) & Strings.ChrW(10)

            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & sProcedureName & " StoredProcedure", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set permissions

            sSQL = "GRANT EXECUTE ON dbo." & sProcedureName & " TO PUBLIC" & Strings.ChrW(13) & Strings.ChrW(10)

            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Grant " & sProcedureName & " StoredProcedure", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'The main procedure...
            sProcedureName = "spg_wp_" & v_sDatamodel & "SumInsured" & CStr(lSumInsuredTypeId)

            sSelectList = "SELECT description = " & sPrefix & ".description," & Strings.ChrW(13) & Strings.ChrW(10) &
                          "reference = " & sPrefix & ".reference," & Strings.ChrW(13) & Strings.ChrW(10) &
                          "sum_insured = " & sPrefix & ".sum_insured," & Strings.ChrW(13) & Strings.ChrW(10) &
                          "date_added = " & sPrefix & ".date_added," & Strings.ChrW(13) & Strings.ChrW(10) &
                          "date_deleted = " & sPrefix & ".date_deleted," & Strings.ChrW(13) & Strings.ChrW(10) &
                          "is_valuation_required = " & sPrefix & ".is_valuation_required," & Strings.ChrW(13) & Strings.ChrW(10) &
                          "valuation_date = " & sPrefix & ".valuation_date" & Strings.ChrW(13) & Strings.ChrW(10)

            sTableList = "FROM " & v_sDatamodel & "_sum_insured " & sPrefix & ", " & Strings.ChrW(13) & Strings.ChrW(10) &
                         v_sDatamodel & "_Policy_binder pb," & Strings.ChrW(13) & Strings.ChrW(10) &
                         "GIS_policy_link gpl," & Strings.ChrW(13) & Strings.ChrW(10) &
                         "insurance_file ifi" & Strings.ChrW(13) & Strings.ChrW(10)

            sWhereList1 = "WHERE ifi.insurance_file_cnt = @InsuranceFileCnt" & Strings.ChrW(13) & Strings.ChrW(10) &
                          "AND gpl.insurance_file_cnt = ifi.insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10) &
                          "AND gpl.Risk_id is null" & Strings.ChrW(13) & Strings.ChrW(10) &
                          "AND pb.gis_policy_link_id = gpl.gis_policy_link_id" & Strings.ChrW(13) & Strings.ChrW(10) &
                          "AND " & sPrefix & "." & v_sDatamodel & "_policy_binder_id = pb." & v_sDatamodel & "_Policy_binder_id" & Strings.ChrW(13) & Strings.ChrW(10) &
                          "AND " & sPrefix & ".sum_insured_type_id = " & CStr(lSumInsuredTypeId) & Strings.ChrW(13) & Strings.ChrW(10) &
                          "AND " & sPrefix & ".sequence_id = @Instance2" & Strings.ChrW(13) & Strings.ChrW(10)

            sWhereList2 = "WHERE ifi.insurance_file_cnt = @InsuranceFileCnt" & Strings.ChrW(13) & Strings.ChrW(10) &
                          "AND gpl.Risk_id = @RiskId" & Strings.ChrW(13) & Strings.ChrW(10) &
                          "AND pb.gis_policy_link_id = gpl.gis_policy_link_id" & Strings.ChrW(13) & Strings.ChrW(10) &
                          "AND " & sPrefix & "." & v_sDatamodel & "_policy_binder_id = pb." & v_sDatamodel & "_Policy_binder_id" & Strings.ChrW(13) & Strings.ChrW(10) &
                          "AND " & sPrefix & ".sum_insured_type_id = " & CStr(lSumInsuredTypeId) & Strings.ChrW(13) & Strings.ChrW(10) &
                          "AND " & sPrefix & ".sequence_id = @Instance2" & Strings.ChrW(13) & Strings.ChrW(10)

            'Drop it if it's already there
            DropExistingProcedure(sProcedureName, "", m_lReturn, r_oDatabase)
            DropExistingProcedure(sProcedureNameOldStyle, "", m_lReturn, r_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sSelectList = "If @RiskId Is Null" & Strings.ChrW(13) & Strings.ChrW(10) &
                          sSelectList & sTableList & sWhereList1 &
                          "Else" & Strings.ChrW(13) & Strings.ChrW(10) &
                          sSelectList & sTableList & sWhereList2

            'Create the new procedure
            sSQL = ""

            sSQL = sSQL & "CREATE PROCEDURE " & sProcedureName & " @PartyCnt INT," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "@InsuranceFileCnt INT," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "@RiskId INT = NULL," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "@ClaimCnt INT," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "@DocumentRef VARCHAR(25)," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "@Instance1 INT," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "@Instance2 INT," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "@Instance3 INT" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "AS" & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & sSelectList & Strings.ChrW(13) & Strings.ChrW(10)

            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & sProcedureName & " StoredProcedure", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set permissions

            sSQL = "GRANT EXECUTE ON dbo." & sProcedureName & " TO PUBLIC" & Strings.ChrW(13) & Strings.ChrW(10)

            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Grant " & sProcedureName & " StoredProcedure", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Now let's set up the individual wp fields
            'Delete the records on wp_fields
            sSQL = ""

            sSQL = sSQL & "DELETE FROM wp_fields" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "WHERE sql = '" & sProcedureName & "'" & Strings.ChrW(13) & Strings.ChrW(10)

            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Delete " & sProcedureName & " WPFields", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = CType(AddToWPFields(sFieldName:=v_sDatamodel & "SumInsuredDescription" & CStr(lSumInsuredTypeId), sSQL:=sProcedureName, sColumnName:="description",
                                            lColumnType:=gPMConstants.PMEFormatStyle.PMFormatString, sMainGroup:="Risk", sSubGroup:=sDescription, sSubGroup2:="", sSubGroup3:="",
                                            sSubGroup4:="", sDisplayname:="Description", iIsDisplayed:=1, sLoop1:=v_sDatamodel & "SumInsured" & CStr(lSumInsuredTypeId), sLoop2:="", sLoop3:="",
                                            sLoop4:="", lProductFamily:=v_lPMProductFamily, vDataModel:=DBNull.Value, vPropertyId:=DBNull.Value, lSpecialsType:=0,
                                            r_oDatabase:=r_oDatabase, sTableName:=sTableName), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = CType(AddToWPFields(sFieldName:=v_sDatamodel & "SumInsuredReference" & CStr(lSumInsuredTypeId), sSQL:=sProcedureName, sColumnName:="reference",
                                            lColumnType:=gPMConstants.PMEFormatStyle.PMFormatString, sMainGroup:="Risk", sSubGroup:=sDescription, sSubGroup2:="", sSubGroup3:="",
                                            sSubGroup4:="", sDisplayname:="Reference", iIsDisplayed:=1, sLoop1:=v_sDatamodel & "SumInsured" & CStr(lSumInsuredTypeId), sLoop2:="", sLoop3:="",
                                            sLoop4:="", lProductFamily:=v_lPMProductFamily, vDataModel:=DBNull.Value, vPropertyId:=DBNull.Value, lSpecialsType:=0, r_oDatabase:=r_oDatabase,
                                            sTableName:=sTableName), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = CType(AddToWPFields(sFieldName:=v_sDatamodel & "SumInsuredSumInsured" & CStr(lSumInsuredTypeId), sSQL:=sProcedureName, sColumnName:="sum_insured",
                                            lColumnType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, sMainGroup:="Risk", sSubGroup:=sDescription, sSubGroup2:="", sSubGroup3:="",
                                            sSubGroup4:="", sDisplayname:="Sum Insured", iIsDisplayed:=1, sLoop1:=v_sDatamodel & "SumInsured" & CStr(lSumInsuredTypeId), sLoop2:="",
                                            sLoop3:="", sLoop4:="", lProductFamily:=v_lPMProductFamily, vDataModel:=DBNull.Value, vPropertyId:=DBNull.Value, lSpecialsType:=0,
                                            r_oDatabase:=r_oDatabase, sTableName:=sTableName), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = CType(AddToWPFields(sFieldName:=v_sDatamodel & "SumInsuredDateAdded" & CStr(lSumInsuredTypeId), sSQL:=sProcedureName, sColumnName:="date_added",
                                            lColumnType:=gPMConstants.PMEFormatStyle.PMFormatDateLong, sMainGroup:="Risk", sSubGroup:=sDescription, sSubGroup2:="", sSubGroup3:="",
                                            sSubGroup4:="", sDisplayname:="Date Added", iIsDisplayed:=1, sLoop1:=v_sDatamodel & "SumInsured" & CStr(lSumInsuredTypeId), sLoop2:="", sLoop3:="",
                                            sLoop4:="", lProductFamily:=v_lPMProductFamily, vDataModel:=DBNull.Value, vPropertyId:=DBNull.Value, lSpecialsType:=0,
                                            r_oDatabase:=r_oDatabase, sTableName:=sTableName), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = CType(AddToWPFields(sFieldName:=v_sDatamodel & "SumInsuredDateDeleted" & CStr(lSumInsuredTypeId), sSQL:=sProcedureName, sColumnName:="date_deleted",
                                            lColumnType:=gPMConstants.PMEFormatStyle.PMFormatDateLong, sMainGroup:="Risk", sSubGroup:=sDescription, sSubGroup2:="", sSubGroup3:="", sSubGroup4:="",
                                            sDisplayname:="Date Deleted", iIsDisplayed:=1, sLoop1:=v_sDatamodel & "SumInsured" & CStr(lSumInsuredTypeId), sLoop2:="", sLoop3:="", sLoop4:="",
                                            lProductFamily:=v_lPMProductFamily, vDataModel:=DBNull.Value, vPropertyId:=DBNull.Value, lSpecialsType:=0,
                                            r_oDatabase:=r_oDatabase, sTableName:=sTableName), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = CType(AddToWPFields(sFieldName:=v_sDatamodel & "SumInsuredIsValuationRequired" & CStr(lSumInsuredTypeId), sSQL:=sProcedureName, sColumnName:="is_valuation_required",
                                            lColumnType:=gPMConstants.PMEFormatStyle.PMFormatBoolean, sMainGroup:="Risk", sSubGroup:=sDescription, sSubGroup2:="", sSubGroup3:="", sSubGroup4:="",
                                            sDisplayname:="Is valuation required", iIsDisplayed:=1, sLoop1:=v_sDatamodel & "SumInsured" & CStr(lSumInsuredTypeId), sLoop2:="", sLoop3:="", sLoop4:="",
                                            lProductFamily:=v_lPMProductFamily, vDataModel:=DBNull.Value, vPropertyId:=DBNull.Value, lSpecialsType:=0,
                                            r_oDatabase:=r_oDatabase, sTableName:=sTableName), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = CType(AddToWPFields(sFieldName:=v_sDatamodel & "SumInsuredValuationDate" & CStr(lSumInsuredTypeId), sSQL:=sProcedureName, sColumnName:="valuation_date",
                                            lColumnType:=gPMConstants.PMEFormatStyle.PMFormatDateLong, sMainGroup:="Risk", sSubGroup:=sDescription, sSubGroup2:="", sSubGroup3:="", sSubGroup4:="",
                                            sDisplayname:="Valuation date", iIsDisplayed:=1, sLoop1:=v_sDatamodel & "SumInsured" & CStr(lSumInsuredTypeId), sLoop2:="", sLoop3:="", sLoop4:="",
                                            lProductFamily:=v_lPMProductFamily, vDataModel:=DBNull.Value, vPropertyId:=DBNull.Value, lSpecialsType:=0,
                                            r_oDatabase:=r_oDatabase, sTableName:=sTableName), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'The total procedure...
            sProcedureName = "spg_wp_" & v_sDatamodel & "SumInsuredTotal" & CStr(lSumInsuredTypeId)

            sSelectList = "SELECT @rate = MAX(" & sPrefix & ".rate)," & Strings.ChrW(13) & Strings.ChrW(10) &
                          "@Premium = MAX(" & sPrefix & ".premium)" & Strings.ChrW(13) & Strings.ChrW(10)

            sSelectList2 = "SELECT @total_sum_insured = sum(ISNULL(" & sPrefix & ".sum_insured,0))" & Strings.ChrW(13) & Strings.ChrW(10)

            sSelectList3 = "SELECT total_sum_insured = @total_sum_insured," & Strings.ChrW(13) & Strings.ChrW(10) &
                           "rate = @rate," & Strings.ChrW(13) & Strings.ChrW(10) &
                           "premium = @premium" & Strings.ChrW(13) & Strings.ChrW(10)


            sTableList = "FROM " & v_sDatamodel & "_sum_insured " & sPrefix & ", " & Strings.ChrW(13) & Strings.ChrW(10) &
                         v_sDatamodel & "_Policy_binder pb," & Strings.ChrW(13) & Strings.ChrW(10) &
                         "GIS_policy_link gpl," & Strings.ChrW(13) & Strings.ChrW(10) &
                         "insurance_file ifi" & Strings.ChrW(13) & Strings.ChrW(10)

            sWhereList1 = "WHERE ifi.insurance_file_cnt = @InsuranceFileCnt" & Strings.ChrW(13) & Strings.ChrW(10) &
                          "AND gpl.insurance_file_cnt = ifi.insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10) &
                          "AND gpl.Risk_id is null" & Strings.ChrW(13) & Strings.ChrW(10) &
                          "AND pb.gis_policy_link_id = gpl.gis_policy_link_id" & Strings.ChrW(13) & Strings.ChrW(10) &
                          "AND " & sPrefix & "." & v_sDatamodel & "_policy_binder_id = pb." & v_sDatamodel & "_Policy_binder_id" & Strings.ChrW(13) & Strings.ChrW(10) &
                          "AND " & sPrefix & ".sum_insured_type_id = " & CStr(lSumInsuredTypeId) & Strings.ChrW(13) & Strings.ChrW(10)

            sWhereList2 = "WHERE ifi.insurance_file_cnt = @InsuranceFileCnt" & Strings.ChrW(13) & Strings.ChrW(10) &
                          "AND gpl.Risk_id = @RiskId" & Strings.ChrW(13) & Strings.ChrW(10) &
                          "AND pb.gis_policy_link_id = gpl.gis_policy_link_id" & Strings.ChrW(13) & Strings.ChrW(10) &
                          "AND " & sPrefix & "." & v_sDatamodel & "_policy_binder_id = pb." & v_sDatamodel & "_Policy_binder_id" & Strings.ChrW(13) & Strings.ChrW(10) &
                          "AND " & sPrefix & ".sum_insured_type_id = " & CStr(lSumInsuredTypeId) & Strings.ChrW(13) & Strings.ChrW(10)

            'Drop it if it's already there
            DropExistingProcedure(sProcedureName, "", m_lReturn, r_oDatabase)
            DropExistingProcedure(sProcedureNameOldStyle, "", m_lReturn, r_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sSelectList = "If @RiskId Is Null" & Strings.ChrW(13) & Strings.ChrW(10) &
                          "Begin" & Strings.ChrW(13) & Strings.ChrW(10) &
                          sSelectList & sTableList & sWhereList1 &
                          "AND " & sPrefix & ".sequence_id = 1" & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10) &
                          sSelectList2 & sTableList & sWhereList1 &
                          "AND ISNULL(" & sPrefix & ".date_deleted, '1899-12-29') = '1899-12-29'" & Strings.ChrW(13) & Strings.ChrW(10) &
                          "End" & Strings.ChrW(13) & Strings.ChrW(10) &
                          "Else" & Strings.ChrW(13) & Strings.ChrW(10) &
                          "Begin" & Strings.ChrW(13) & Strings.ChrW(10) &
                          sSelectList & sTableList & sWhereList2 &
                          "AND " & sPrefix & ".sequence_id = 1" & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10) &
                          sSelectList2 & sTableList & sWhereList2 &
                          "AND ISNULL(" & sPrefix & ".date_deleted, '1899-12-29') = '1899-12-29'" & Strings.ChrW(13) & Strings.ChrW(10) &
                          "End" & Strings.ChrW(13) & Strings.ChrW(10) &
                          sSelectList3 & Strings.ChrW(13) & Strings.ChrW(10)

            'Create the new procedure
            sSQL = ""

            sSQL = sSQL & "CREATE PROCEDURE " & sProcedureName & " @PartyCnt INT," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "@InsuranceFileCnt INT," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "@RiskId INT = NULL," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "@ClaimCnt INT," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "@DocumentRef VARCHAR(25)," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "@Instance1 INT," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "@Instance2 INT," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "@Instance3 INT" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "AS" & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10) &
                   "DECLARE @total_sum_insured NUMERIC(19,4)," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "@rate NUMERIC(7,4)," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "@premium NUMERIC(19,4)" & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & sSelectList & Strings.ChrW(13) & Strings.ChrW(10)

            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & sProcedureName & " StoredProcedure", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set permissions

            sSQL = "GRANT EXECUTE ON dbo." & sProcedureName & " TO PUBLIC" & Strings.ChrW(13) & Strings.ChrW(10)

            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Grant " & sProcedureName & " StoredProcedure", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Now let's set up the total wp fields
            'Delete the records on wp_fields
            sSQL = ""

            sSQL = sSQL & "DELETE FROM wp_fields" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "WHERE sql = '" & sProcedureName & "'" & Strings.ChrW(13) & Strings.ChrW(10)

            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Delete " & sProcedureName & " WPFields", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = CType(AddToWPFields(sFieldName:=v_sDatamodel & "TotalSumInsured" & CStr(lSumInsuredTypeId), sSQL:=sProcedureName, sColumnName:="total_sum_insured",
                                            lColumnType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, sMainGroup:="Risk", sSubGroup:=sDescription, sSubGroup2:="", sSubGroup3:="",
                                            sSubGroup4:="", sDisplayname:="Total Sum Insured", iIsDisplayed:=1, sLoop1:="", sLoop2:="", sLoop3:="", sLoop4:="", lProductFamily:=v_lPMProductFamily,
                                            vDataModel:=DBNull.Value, vPropertyId:=DBNull.Value, lSpecialsType:=0, r_oDatabase:=r_oDatabase, sTableName:=sTableName), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = CType(AddToWPFields(sFieldName:=v_sDatamodel & "Rate" & CStr(lSumInsuredTypeId), sSQL:=sProcedureName, sColumnName:="rate",
                                            lColumnType:=gPMConstants.PMEFormatStyle.PMFormatPercent, sMainGroup:="Risk", sSubGroup:=sDescription, sSubGroup2:="", sSubGroup3:="", sSubGroup4:="",
                                            sDisplayname:="Rate", iIsDisplayed:=1, sLoop1:="", sLoop2:="", sLoop3:="", sLoop4:="", lProductFamily:=v_lPMProductFamily, vDataModel:=DBNull.Value,
                                            vPropertyId:=DBNull.Value, lSpecialsType:=0, r_oDatabase:=r_oDatabase, sTableName:=sTableName), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = CType(AddToWPFields(sFieldName:=v_sDatamodel & "Premium" & CStr(lSumInsuredTypeId), sSQL:=sProcedureName, sColumnName:="premium",
                                            lColumnType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, sMainGroup:="Risk", sSubGroup:=sDescription, sSubGroup2:="", sSubGroup3:="", sSubGroup4:="",
                                            sDisplayname:="Premium", iIsDisplayed:=1, sLoop1:="", sLoop2:="", sLoop3:="", sLoop4:="", lProductFamily:=v_lPMProductFamily, vDataModel:=DBNull.Value,
                                            vPropertyId:=DBNull.Value, lSpecialsType:=0, r_oDatabase:=r_oDatabase, sTableName:=sTableName), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GenerateSumInsuredFields Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateSumInsuredFields", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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


    Private Function AddToWPFields(ByRef sFieldName As String, ByRef sSQL As String, ByRef sColumnName As String, ByRef lColumnType As Integer,
                                   ByRef sMainGroup As String, ByRef sSubGroup As String, ByRef sSubGroup2 As String, ByRef sSubGroup3 As String,
                                   ByRef sSubGroup4 As String, ByRef sDisplayname As String, ByRef iIsDisplayed As Integer, ByRef sLoop1 As String,
                                   ByRef sLoop2 As String, ByRef sLoop3 As String, ByRef sLoop4 As String, ByRef lProductFamily As Integer,
                                   ByRef vDataModel As Object, ByRef vPropertyId As Object, ByVal lSpecialsType As Integer, ByRef r_oDatabase As dPMDAO.Database,
                                   ByVal sTableName As String) As Integer

        Dim result As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim vLoop1 As String
        Dim vLoop2 As String
        Dim vLoop3 As String
        Dim vLoop4 As String


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

        m_lReturn = r_oDatabase.Parameters.Add(sName:="loop1", vValue:=If(vLoop1 Is Nothing, DBNull.Value, vLoop1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = r_oDatabase.Parameters.Add(sName:="loop2", vValue:=If(vLoop2 Is Nothing, DBNull.Value, vLoop2), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = r_oDatabase.Parameters.Add(sName:="loop3", vValue:=If(vLoop3 Is Nothing, DBNull.Value, vLoop3), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = r_oDatabase.Parameters.Add(sName:="loop4", vValue:=If(vLoop4 Is Nothing, DBNull.Value, vLoop4), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = r_oDatabase.Parameters.Add(sName:="product_family", vValue:=CStr(lProductFamily), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If



        ' Developer Guide No. 85
        m_lReturn = r_oDatabase.Parameters.Add(sName:="data_model", vValue:=NullToString(vDataModel), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        ' Developer Guide No. 85
        m_lReturn = r_oDatabase.Parameters.Add(sName:="property_id", vValue:=NullToString(vPropertyId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

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

        ' Developer Guide No. 287
        m_lReturn = r_oDatabase.Parameters.Add(sName:="specials_type", vValue:=NullToString(lSpecialsType), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = r_oDatabase.Parameters.Add(sName:="table_name", vValue:=sTableName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

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

    Private Function GetDataModelType(ByRef r_oDatabase As dPMDAO.Database, ByVal sDatamodel As String, ByRef r_iDMType As Integer) As Integer
        Dim result As Integer = 0

        Dim lGISDataModelType As Integer
        Dim vResArray As Object = Nothing

        m_lReturn = r_oDatabase.SQLSelect(sSQL:="SELECT gis_data_model_type_id FROM " &
                    "gis_data_model WHERE code='" & sDatamodel & "'", sSQLName:="GenerateStoredProcedure raw SQL", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResArray)
        If Informations.IsArray(vResArray) Then

            lGISDataModelType = CInt(Val(CStr(vResArray(0, 0))))
        End If

        r_iDMType = lGISDataModelType



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

        Dim result As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim sTemp As New StringBuilder
        Dim lTemp As Integer

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
    If (v_vTheString.Length >= v_sDatamodel.Length) AndAlso (v_vTheString.Substring(0, v_sDatamodel.Length) = v_sDatamodel OrElse v_vTheString.Substring(0, v_sDatamodel.Length) = (v_sDatamodel).ToLower()) Then
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

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".IsObjectMultipleInstance")




        lParentId = -1

        If Informations.IsArray(r_vGISObject) Then
            For lTemp As Integer = r_vGISObject.GetLowerBound(1) To r_vGISObject.GetUpperBound(1)

                If CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOObjectName, lTemp)) = v_sObjectName Then

                    lParentId = CInt(r_vGISObject(pbObjectAndPropertyConsts.ACOParentObjectId, lTemp))
                    Exit For
                End If
            Next
        End If

        Dim lOldParentId As Integer
        Dim bExitWhile As Boolean
        If lParentId <> -1 And Informations.IsArray(r_vGISObject) Then


            lTempParentId = CStr(lParentId)
            bExitWhile = False
            ' Developer Guide No. 287
            While (NullToDouble(lTempParentId) <> 0) And Not (bExitWhile)
                lOldParentId = CInt(lTempParentId)
                For lTemp As Integer = r_vGISObject.GetLowerBound(1) To r_vGISObject.GetUpperBound(1)

                    If CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOGISObjectId, lTemp)) = lTempParentId Then
                        lParentId = CInt(lTempParentId)

                        If Not r_vGISObject(pbObjectAndPropertyConsts.ACOParentObjectId, lTemp) Is Nothing Then
                            lTempParentId = r_vGISObject(pbObjectAndPropertyConsts.ACOParentObjectId, lTemp).ToString()
                        End If
                        If Not (CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOObjectName, lTemp)).ToLower().EndsWith("policy_binder")) Then

                            If CDbl(r_vGISObject(pbObjectAndPropertyConsts.ACOMaxInstances, lTemp)) > 1 Then
                                result = 1
                            Else
                                result = 0
                            End If
                        End If
                        Exit For
                    End If
                Next
                If ToSafeDouble(lTempParentId) = lOldParentId Then bExitWhile = True
            End While
        End If

        ' Debug message
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

            m_lReturn = r_oDatabase.Parameters.Add(sName:="effective_date", vValue:=(DateTime.Now), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

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
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetALookup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetALookup", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

        Dim result As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim sTemp As String = ""
        Dim lTemp As Integer
        Dim bOK As Boolean
        Dim vArray(,) As Object = Nothing

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

            If Informations.IsArray(vArray) Then
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
        Dim sDefSubKey As String = ""
        Dim sSubKey As String = ""
        Dim sTemp As String = ""
        Dim vRegistryKeys As Object = Nothing


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
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateRegistrySettings Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateRegistrySettings", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

        ' Debug message
        Dim result As Integer = 0
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".CopyDefaultGISLists")

        Dim sOldFile As String = ""
        Dim sNewFile As String = ""
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sPath As String = ""
            Dim sDefSubKey As String = ""

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

            If Not File.Exists(sNewFile & "0101.txt") Then
                File.Copy(sOldFile & "0101.txt", sNewFile & "0101.txt")
                File.Copy(sOldFile & ".dat", sNewFile & ".dat")
                File.Copy(sOldFile & ".idx", sNewFile & ".idx")
            End If

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".CopyDefaultGISLists")

            Return result

        Catch excep As System.Exception

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".CopyDefaultGISLists")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyDefaultGISLists Failed (" & sOldFile & "0101.txt) to (" & sNewFile & "0101.txt)", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyDefaultGISLists", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Private Function ProcessPropertyArray(ByVal v_lObjectType As Integer, ByVal v_sMainGroup As String, ByVal v_sProcedureName As String, ByVal v_lPMProductFamily As Integer, ByVal v_sDatamodel As String, ByVal v_sPrefix As String, ByRef r_vPropertyArray(,) As Object, ByRef r_sSelectList As String, ByRef r_sTableList As String, ByRef r_oDatabase As dPMDAO.Database, ByRef r_lParamCount As Integer, Optional ByVal v_sSubGroup As String = "", Optional ByVal v_sSubGroup1 As String = "", Optional ByVal v_sSubGroup2 As String = "", Optional ByVal v_sSubGroup3 As String = "", Optional ByVal v_sLoop1 As String = "", Optional ByVal v_sLoop2 As String = "", Optional ByVal v_sLoop3 As String = "", Optional ByVal v_sLoop4 As String = "", Optional ByRef r_sWhereList As String = "", Optional ByRef r_iInstanceCount As Integer = 0, Optional ByVal v_lLevel As Integer = 0) As Integer

        Dim result As Integer = gPMConstants.PMEReturnCode.PMTrue

        Dim llBound, lUbound, lAddressCount, lOtherTableCount As Integer
        Dim vWPFields(,) As Object = Nothing

        Dim sTemp As String
        Dim lTemp3 As Integer
        Dim sFieldName, sDisplayname, sSubGroup, sSubGroup2, sSubGroup3, sSubGroup4 As String
        Dim sSubGroupTemp As StringBuilder
        Dim lLevel As Integer
        Dim iParentIsMultipleInstance As Integer
        Dim sLoop1, sLoop2, sLoop3, sLoop4 As String
        Dim sWhereList2 As New StringBuilder
        Dim sWhereList1 As New StringBuilder
        Dim iInstanceCount As Integer
        Dim sColumnName As String = ""
        Dim sTableName As String = String.Empty



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
        sSubGroup4 = ""
        sLoop1 = ""
        sLoop2 = ""
        sLoop3 = ""
        sLoop4 = ""

        '*****************
        ' If there are any properties to be added
        '*****************
        If Informations.IsArray(r_vPropertyArray) Then

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

                If CDbl(r_vPropertyArray(pbObjectAndPropertyConsts.ACPIsIdentifyingProperty, lProperty)) = 1 Then

                    'Move the determination of the group structure to here - only
                    'do it once
                    sSubGroupTemp = New StringBuilder(v_sDatamodel)
                    sTableName = ToSafeString(r_vPropertyArray(pbObjectAndPropertyConsts.ACPColumnName, lProperty))
                    sTemp = StripDataModelCode(v_vTheString:=CStr(r_vPropertyArray(pbObjectAndPropertyConsts.ACPColumnName, lProperty)), v_sDatamodel:=v_sDatamodel).Trim()

                    'Remove the trailing _id
                    sTemp = sTemp.Substring(0, sTemp.Length - 3)
                    sTableName = sTableName.Substring(0, sTableName.Length - 3)

                    While sTemp.IndexOf("_"c) >= 0
                        lTemp3 = (sTemp.IndexOf("_"c) + 1)
                        sSubGroupTemp.Append(Informations.ProperCase(sTemp.Substring(0, lTemp3 - 1)))
                        sTemp = sTemp.Substring(lTemp3)
                    End While


                    Select Case lProperty
                        Case 0

                        Case 1
                            sSubGroup = sSubGroupTemp.ToString() & Informations.ProperCase(sTemp)
                            'find if parent was actually a looping object
                            If lLevel = 0 Then
                                If iParentIsMultipleInstance = 1 And lLevel = 0 Then
                                    sLoop1 = sSubGroup
                                End If
                            End If

                        Case 2
                            sSubGroup2 = sSubGroupTemp.ToString() & Informations.ProperCase(sTemp)
                            If lLevel = 0 Then
                                If iParentIsMultipleInstance = 1 Then
                                    sLoop2 = sSubGroup2
                                Else
                                    sLoop1 = sSubGroup2
                                End If
                            End If

                        Case 3
                            sSubGroup3 = sSubGroupTemp.ToString() & Informations.ProperCase(sTemp)
                            If lLevel = 0 Then
                                If iParentIsMultipleInstance = 1 Then
                                    sLoop3 = sSubGroup3
                                Else
                                    sLoop2 = sSubGroup3
                                End If
                            End If

                        Case 4
                            sSubGroup4 = sSubGroupTemp.ToString() & Informations.ProperCase(sTemp)
                            If lLevel = 0 Then
                                If iParentIsMultipleInstance = 1 Then
                                    sLoop4 = sSubGroup4
                                Else
                                    sLoop3 = sSubGroup4
                                End If
                            End If
                    End Select

                    If lProperty > lLevel Then

                        sWhereList1.Append("AND " & v_sPrefix & "." & CStr(r_vPropertyArray(pbObjectAndPropertyConsts.ACPColumnName, lProperty)) & " = @Instance" & CStr(lProperty + iParentIsMultipleInstance) & Strings.ChrW(13) & Strings.ChrW(10))

                        sWhereList2.Append("AND " & v_sPrefix & "." & CStr(r_vPropertyArray(pbObjectAndPropertyConsts.ACPColumnName, lProperty)) & " = @Instance" & CStr(lProperty + iParentIsMultipleInstance) & Strings.ChrW(13) & Strings.ChrW(10))
                        iInstanceCount = lProperty + iParentIsMultipleInstance
                    End If

                Else

                    '*****************
                    ' if the property is not an identity / key
                    ' and is not deleted
                    '*****************



                    If CStr(r_vPropertyArray(pbObjectAndPropertyConsts.ACPPropertyName, lProperty)) <> "dElEtEd" And CStr(r_vPropertyArray(pbObjectAndPropertyConsts.ACPColumnName, lProperty)).ToLower().Trim() <> "case_header" And CStr(r_vPropertyArray(pbObjectAndPropertyConsts.ACPColumnName, lProperty)).ToLower().Trim() <> "case_claim_links" Then

                        '*****************
                        ' if we have a special type of field
                        ' like address / party / lookup / gis table link / etc
                        '*****************

                        'If IsDefaultProperty(v_lObjectType:=v_lObjectType, _
                        ''                      v_sColumnName:=sColumnName) Then

                        ' if the property already exists in another non-gis related table then

                        If CBool(r_vPropertyArray(pbObjectAndPropertyConsts.ACPEditFlags, lProperty)) And GISSharedPropertyConstants.GISDSEditNoDBColumn Then
                            'If r_vPropertyArray(pbObjectAndPropertyConsts.ACPEditFlags, lProperty).ToString() = 1 And GISSharedPropertyConstants.GISDSEditNoDBColumn Then

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

                            ElseIf CDbl(r_vPropertyArray(pbObjectAndPropertyConsts.ACPSpecialsType, lProperty)) = GISSharedPropertyConstants.ACOPartyTypeID Then
                                AddPartyProperty(v_vPropertyArray:=r_vPropertyArray, v_lItem:=lProperty, v_sPrefix:=v_sPrefix, r_sSelectList:=r_sSelectList, r_sTableList:=r_sTableList, r_lOtherTableCount:=lOtherTableCount, r_vWPFields:=vWPFields)

                                '*****************
                                ' else if its a lookup then
                                ' add additional lookup params to the stored procedure
                                '*****************

                            ElseIf (CDbl(r_vPropertyArray(pbObjectAndPropertyConsts.ACPSpecialsType, lProperty)) = GISSharedPropertyConstants.ACOPMLookupTableName Or CDbl(r_vPropertyArray(pbObjectAndPropertyConsts.ACPSpecialsType, lProperty)) = GISSharedPropertyConstants.ACOComboLookup) Then
                                AddLookupProperty(v_vPropertyArray:=r_vPropertyArray, v_lItem:=lProperty, v_sPrefix:=v_sPrefix, r_sSelectList:=r_sSelectList, r_sTableList:=r_sTableList, r_lOtherTableCount:=lOtherTableCount, r_vWPFields:=vWPFields)

                                '*****************
                                ' if its a link to another
                                ' gis table
                                '*****************

                            ElseIf CDbl(r_vPropertyArray(pbObjectAndPropertyConsts.ACPSpecialsType, lProperty)) = GISSharedPropertyConstants.ACOGISUserDefHeaderID Then
                                AddGisUserDefinedHeaderProperty(v_vPropertyArray:=r_vPropertyArray, v_lItem:=lProperty, v_sPrefix:=v_sPrefix, r_sSelectList:=r_sSelectList, r_sTableList:=r_sTableList, r_lOtherTableCount:=lOtherTableCount, r_vWPFields:=vWPFields)

                                '***********************
                                '  if its a product then link
                                ' in the product fields
                                '***********************

                            ElseIf CDbl(r_vPropertyArray(pbObjectAndPropertyConsts.ACPSpecialsType, lProperty)) = GISSharedPropertyConstants.ACOProductID Then
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

        If sLoop4 = "" Then
            If v_sLoop4 <> "" Then
                sLoop4 = v_sLoop4
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

        If v_sSubGroup3 = "" Then
            v_sSubGroup3 = sSubGroup4
        End If
        '********

        ' if we have any fields to add to the wp_fields table
        If Informations.IsArray(vWPFields) Then

            ' get array boundaries

            llBound = vWPFields.GetLowerBound(1)

            lUbound = vWPFields.GetUpperBound(1)

            r_lParamCount = lUbound + 1

            ' for each field
            For lWPField As Integer = llBound To lUbound
                ' add an entry in the wp fields table
                If AddToWPFields(sFieldName:=CStr(vWPFields(0, lWPField)), sSQL:=v_sProcedureName, sColumnName:=CStr(vWPFields(1, lWPField)),
                                 lColumnType:=CInt(vWPFields(2, lWPField)), sMainGroup:=v_sMainGroup, sSubGroup:=v_sSubGroup, sSubGroup2:=v_sSubGroup1,
                                 sSubGroup3:=v_sSubGroup2, sSubGroup4:=v_sSubGroup3, sDisplayname:=CStr(vWPFields(3, lWPField)), iIsDisplayed:=1, sLoop1:=sLoop1,
                                 sLoop2:=sLoop2, sLoop3:=sLoop3, sLoop4:=sLoop4, lProductFamily:=v_lPMProductFamily, vDataModel:=If(Len(v_sDatamodel) > 0, v_sDatamodel, DBNull.Value),
                                 vPropertyId:=DBNull.Value, lSpecialsType:=0, r_oDatabase:=r_oDatabase, sTableName:=sTableName) <> gPMConstants.PMEReturnCode.PMTrue Then

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

        r_sSelectList = r_sSelectList &
                        "address" & CStr(r_lAddressCount) & "_line_1 = a" & CStr(r_lAddressCount) & ".address1," & Strings.ChrW(13) & Strings.ChrW(10) &
                        "address" & CStr(r_lAddressCount) & "_line_2 = a" & CStr(r_lAddressCount) & ".address2," & Strings.ChrW(13) & Strings.ChrW(10) &
                        "address" & CStr(r_lAddressCount) & "_line_3 = a" & CStr(r_lAddressCount) & ".address3," & Strings.ChrW(13) & Strings.ChrW(10) &
                        "address" & CStr(r_lAddressCount) & "_line_4 = a" & CStr(r_lAddressCount) & ".address4," & Strings.ChrW(13) & Strings.ChrW(10) &
                        "case a" & CStr(r_lAddressCount) & ".postal_code when convert(varchar(10), a" & CStr(r_lAddressCount) & ".address_id) then '' else a" & CStr(r_lAddressCount) & ".postal_code end as address" & CStr(r_lAddressCount) & "_postal_code," & Strings.ChrW(13) & Strings.ChrW(10) &
                        "address" & CStr(r_lAddressCount) & "_country = c" & CStr(r_lAddressCount) & ".description," & Strings.ChrW(13) & Strings.ChrW(10)

        r_sTableList = r_sTableList & "LEFT JOIN address a" & CStr(r_lAddressCount) & " ON " &
                       v_sPrefix & "." & sColumnName & " = a" & CStr(r_lAddressCount) & ".address_cnt" & Strings.ChrW(13) & Strings.ChrW(10)

        r_sTableList = r_sTableList & "LEFT JOIN country c" & CStr(r_lAddressCount) & " ON " &
                       "a" & CStr(r_lAddressCount) & ".country_id = c" & CStr(r_lAddressCount) & ".country_id" & Strings.ChrW(13) & Strings.ChrW(10)

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
        r_sSelectList = r_sSelectList &
                        sColumnName & " = p" & CStr(r_lOtherTableCount) & ".name," & Strings.ChrW(13) & Strings.ChrW(10)

        r_sTableList = r_sTableList & "LEFT JOIN party p" & CStr(r_lOtherTableCount) & " ON " &
                       v_sPrefix & "." & sColumnName & " = p" & CStr(r_lOtherTableCount) & ".party_cnt" & Strings.ChrW(13) & Strings.ChrW(10)


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

        r_sSelectList = r_sSelectList &
                        sColumnName & " = l" & CStr(r_lOtherTableCount) & ".description," & Strings.ChrW(13) & Strings.ChrW(10)

        r_sTableList = r_sTableList & "LEFT JOIN " & sReference & " l" & CStr(r_lOtherTableCount) & " ON " &
                       v_sPrefix & "." & sColumnName & " = " &
                       "l" & CStr(r_lOtherTableCount) & "." & sReference & "_id" & Strings.ChrW(13) & Strings.ChrW(10)

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
        r_sSelectList = r_sSelectList &
                        sColumnName & " = l" & CStr(r_lOtherTableCount) & ".description," & Strings.ChrW(13) & Strings.ChrW(10)

        r_sTableList = r_sTableList & "LEFT JOIN GIS_user_def_detail l" & CStr(r_lOtherTableCount) & " ON " &
                       v_sPrefix & "." & sColumnName & " = " &
                       "l" & CStr(r_lOtherTableCount) & ".GIS_user_def_detail_id" & Strings.ChrW(13) & Strings.ChrW(10)

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
        r_sSelectList = r_sSelectList &
                        sColumnName & " = i" & CStr(r_lOtherTableCount) & ".insurance_ref," & Strings.ChrW(13) & Strings.ChrW(10)

        r_sTableList = r_sTableList & "LEFT JOIN insurance_file i" & CStr(r_lOtherTableCount) & " ON " &
                       v_sPrefix & "." & sColumnName & " = i" & CStr(r_lOtherTableCount) & ".insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10)


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

        r_sSelectList = r_sSelectList &
                        sColumnName & " = " & v_sPrefix & "." & sColumnName & "," & Strings.ChrW(13) & Strings.ChrW(10)

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
            Case CStr(GISSharedConstants.GISDataTypeNumeric), CStr(GISSharedConstants.GISDataTypeInteger)
                lFormat = gPMConstants.PMEFormatStyle.PMFormatLong
            Case CStr(GISSharedConstants.GISDataTypeOption)
                lFormat = gPMConstants.PMEFormatStyle.PMFormatBoolean
            Case CStr(GISSharedConstants.GISDataTypeComment), CStr(GISSharedConstants.GISDataTypeText)
                lFormat = gPMConstants.PMEFormatStyle.PMFormatString
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

        Dim lWPFieldLastPos, lArrayPos As Integer

        result = gPMConstants.PMEReturnCode.PMTrue

        '**************
        ' Resize Array
        '**************
        If Informations.IsArray(r_vWPFieldArray) Then
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
        Dim sSelectList As String
        Dim sSQL As String = ""



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

        sSQL = sSQL & "CREATE PROCEDURE " & sProcedureName & "_get_keys" & Strings.ChrW(13) & Strings.ChrW(10) &
               "@PartyCnt INT," & Strings.ChrW(13) & Strings.ChrW(10) &
               "@InsuranceFileCnt INT," & Strings.ChrW(13) & Strings.ChrW(10) &
               "@RiskId INT = NULL," & Strings.ChrW(13) & Strings.ChrW(10) &
               "@ClaimCnt INT," & Strings.ChrW(13) & Strings.ChrW(10) &
               "@DocumentRef VARCHAR(25)," & Strings.ChrW(13) & Strings.ChrW(10) &
               "@Instance1 INT = NULL," & Strings.ChrW(13) & Strings.ChrW(10) &
               "@Instance2 INT = NULL," & Strings.ChrW(13) & Strings.ChrW(10) &
               "@Instance3 INT = NULL," & Strings.ChrW(13) & Strings.ChrW(10) &
               "@ClaimPerilId INT = NULL" & Strings.ChrW(13) & Strings.ChrW(10) &
               "AS"

        sSelectList = "If @ClaimPerilId IS NULL" & Strings.ChrW(13) & Strings.ChrW(10) &
                      "SELECT Claim_Peril_Id from " & sTableName &
                      " WHERE Claim_Id = @ClaimCnt" & Strings.ChrW(13) & Strings.ChrW(10) &
                      "Else " & Strings.ChrW(13) & Strings.ChrW(10) &
                      "SELECT Claim_Peril_Id from " & sTableName &
                      " WHERE Claim_Peril_Id = @ClaimPerilId"

        ' combine sql statements to fully generate the procedure script
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10) & sSelectList & Strings.ChrW(13) & Strings.ChrW(10)

        ' create the stored procedure on the server
        m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & sProcedureName & "_get_keys StoredProcedure", bStoredProcedure:=False)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Set permissions
        sSQL = "GRANT EXECUTE ON dbo." & sProcedureName & "_get_keys TO PUBLIC" & Strings.ChrW(13) & Strings.ChrW(10)

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

        sSQL = sSQL & "CREATE PROCEDURE " & sProcedureName & "_get_parent_key @PartyCnt INT," & Strings.ChrW(13) & Strings.ChrW(10) &
               "@InsuranceFileCnt INT," & Strings.ChrW(13) & Strings.ChrW(10) &
               "@RiskId INT = NULL," & Strings.ChrW(13) & Strings.ChrW(10) &
               "@ClaimCnt INT," & Strings.ChrW(13) & Strings.ChrW(10) &
               "@DocumentRef VARCHAR(25)," & Strings.ChrW(13) & Strings.ChrW(10) &
               "@Instance1 INT," & Strings.ChrW(13) & Strings.ChrW(10) &
               "@Instance2 INT," & Strings.ChrW(13) & Strings.ChrW(10) &
               "@Instance3 INT" & Strings.ChrW(13) & Strings.ChrW(10) &
               "AS" & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)

        'First we do the main query
        sSelectList = "SELECT @ClaimCnt"

        sSQL = sSQL & sSelectList & Strings.ChrW(13) & Strings.ChrW(10)

        m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & sProcedureName & "_get_parent_key StoredProcedure", bStoredProcedure:=False)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Set permissions

        sSQL = "GRANT EXECUTE ON dbo." & sProcedureName & "_get_parent_key TO PUBLIC" & Strings.ChrW(13) & Strings.ChrW(10)

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

        Dim sSelectList As String = ""
        Dim sTableList As String = ""
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

        sWhereList = New StringBuilder("WHERE XXXDATAMODELNAME_policy_binder_id in (" &
                     " SELECT XXXDATAMODELNAME_policy_binder_id from " &
                     " XXXDATAMODELNAME_Policy_Binder where gis_policy_link_id in ( " &
                     " Select gis_policy_link_id from gis_Policy_Link " &
                     "Where Claim_Id =@ClaimCnt))")


        'Initialise id key level indicator
        lLevel = 0

        ' get the position of the last identifying key in the array
        ' the unique key of the current table
        If Informations.IsArray(v_vArray) Then

            llBound = v_vArray.GetLowerBound(1)
            lUbound = v_vArray.GetUpperBound(1)

            For lIDKeyPos As Integer = llBound To lUbound

                If CDbl(v_vArray(pbObjectAndPropertyConsts.ACPIsIdentifyingProperty, lIDKeyPos)) = 1 Then
                    lLevel = lIDKeyPos
                End If
            Next lIDKeyPos

            ' add the local level key to the select list and any other keys to the
            ' where clause.
            ' start from lbound + 1 to exclude the initial policy binder id as this is already
            ' added to the initial "where" clause
            For lIDKeyPos As Integer = llBound + 1 To lUbound

                ' if item is id key

                If CDbl(v_vArray(pbObjectAndPropertyConsts.ACPIsIdentifyingProperty, lIDKeyPos)) = 1 Then

                    ' if its not the local table's unique id key
                    If lIDKeyPos < lLevel Then
                        ' add it to the where list

                        sWhereList.Append("AND XXXDATAMODELTABLEALIAS." & CStr(v_vArray(pbObjectAndPropertyConsts.ACPColumnName, lIDKeyPos)) & " = @Instance" & CStr(lIDKeyPos + v_iIsParentMultipleInstance) & Strings.ChrW(13) & Strings.ChrW(10))
                    Else
                        ' else add the local tables unique id key to the select list
                        ' as this is what we want to return

                        sSelectList = "SELECT XXXDATAMODELTABLEALIAS." & CStr(v_vArray(pbObjectAndPropertyConsts.ACPColumnName, lIDKeyPos))
                    End If
                End If

            Next lIDKeyPos

        End If

        ' build the full select list
        sSelectList = sSelectList & Strings.ChrW(13) & Strings.ChrW(10) &
                      sTableList & Strings.ChrW(13) & Strings.ChrW(10) &
                      sWhereList.ToString()

        ' update template sql to contain relevant datamodel, table, prefixes
        sSelectList = sSelectList.Substring(0).Replace(ACSPGenDataTempDataModelName, v_sDatamodel)
        sSelectList = sSelectList.Substring(0).Replace(ACSPGenDataTempDataModelTableName, v_sTableName)
        sSelectList = sSelectList.Substring(0).Replace(ACSPGenDataTempDataModelTableAlias, v_sPrefix)

        'Create the new procedure
        sSQL = "CREATE PROCEDURE " & v_sProcedureName & "_get_keys " & Strings.ChrW(13) & Strings.ChrW(10) &
               "@PartyCnt INT," & Strings.ChrW(13) & Strings.ChrW(10) &
               "@InsuranceFileCnt INT," & Strings.ChrW(13) & Strings.ChrW(10) &
               "@RiskId INT = NULL," & Strings.ChrW(13) & Strings.ChrW(10) &
               "@ClaimCnt INT," & Strings.ChrW(13) & Strings.ChrW(10) &
               "@DocumentRef VARCHAR(25)"

        ' dynamically generate instance variables
        ' the number of instance variables is determined by how far down the
        ' tree structure the object is..
        generateInstanceVariables(sSQL, lLevel)

        ' combine the sql statments to produce the stored procedure sql
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10) & "AS" & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10) & sSelectList & Strings.ChrW(13) & Strings.ChrW(10)

        ' add stored procedure to database
        If r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & v_sProcedureName & "_get_keys StoredProcedure", bStoredProcedure:=False) = gPMConstants.PMEReturnCode.PMTrue Then

            'Set permissions on stored procedure
            sSQL = "GRANT EXECUTE ON dbo." & v_sProcedureName & "_get_keys TO PUBLIC" & Strings.ChrW(13) & Strings.ChrW(10)

            If r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Grant " & v_sProcedureName & "_get_keys StoredProcedure", bStoredProcedure:=False) <> gPMConstants.PMEReturnCode.PMTrue Then

                ' log error
                result = gPMConstants.PMEReturnCode.PMFalse

                gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to set default permissions for " &
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

            sWhereList = "WHERE XXXDATAMODELNAME_policy_binder_id in (" &
                         " SELECT XXXDATAMODELNAME_policy_binder_id from " &
                         " XXXDATAMODELNAME_Policy_Binder where gis_policy_link_id in ( " &
                         " Select gis_policy_link_id from gis_Policy_Link " &
                         "Where Claim_Id =@ClaimCnt))"

            ' build the full select list
            sSelectList = sSelectList & Strings.ChrW(13) & Strings.ChrW(10) &
                          sTableList & Strings.ChrW(13) & Strings.ChrW(10) &
                          sWhereList

            ' update template sql to contain relevant datamodel, table, prefixes
            sSelectList = sSelectList.Substring(0).Replace(ACSPGenDataTempDataModelName, v_sDatamodel)
            sSelectList = sSelectList.Substring(0).Replace(ACSPGenDataTempDataModelTableName, v_sTableName)
            sSelectList = sSelectList.Substring(0).Replace(ACSPGenDataTempDataModelTableAlias, v_sPrefix)

            'Create the new procedure
            sSQL = ""

            sSQL = sSQL & "CREATE PROCEDURE " & v_sProcedureName & "_get_parent_key @PartyCnt INT," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "@InsuranceFileCnt INT," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "@RiskId INT = NULL," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "@ClaimCnt INT," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "@DocumentRef VARCHAR(25)," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "@Instance1 INT," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "@Instance2 INT," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "@Instance3 INT" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "AS" & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & sSelectList & Strings.ChrW(13) & Strings.ChrW(10)

            If r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & v_sProcedureName & "_get_parent_key StoredProcedure", bStoredProcedure:=False) = gPMConstants.PMEReturnCode.PMTrue Then
                'Set permissions
                sSQL = "GRANT EXECUTE ON dbo." & v_sProcedureName & "_get_parent_key TO PUBLIC" & Strings.ChrW(13) & Strings.ChrW(10)

                If r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Grant " & v_sProcedureName & "_get_parent_key StoredProcedure", bStoredProcedure:=False) <> gPMConstants.PMEReturnCode.PMTrue Then

                    ' log error
                    result = gPMConstants.PMEReturnCode.PMFalse

                    gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to set default permissions for " &
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

            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to drop stored procedure:" & v_sProcedureName & " OR " & v_sProcedureNameOldStyle, vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)

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


        Dim result As Integer = gPMConstants.PMEReturnCode.PMTrue
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


        ' if not the parent object

        If CDbl(v_vGISObject(pbObjectAndPropertyConsts.ACOGISObjectId, v_lObjectId)) <> v_lParentId Then

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
            sTableList = "FROM " & sTableName & " " & sPrefix & Strings.ChrW(13) & Strings.ChrW(10)

            ' Get and use property array for the current object


            vArray = v_vGISProperty(v_lObjectId)

            ' determine the level of the object
            ' as this will determine what keys stored procedures if any
            ' need to be created.
            If Informations.IsArray(vArray) Then


                llBound = vArray.GetLowerBound(1)

                lUbound = vArray.GetUpperBound(1)

                For lIDKeyPos As Integer = llBound To lUbound

                    If CDbl(vArray(pbObjectAndPropertyConsts.ACPIsIdentifyingProperty, lIDKeyPos)) = 1 Then
                        lLevel = lIDKeyPos
                    End If
                Next lIDKeyPos
            End If

            '************************
            ' Generate Get Keys Stored Procedure
            '************************

            If lLevel > 1 Or CDbl(v_vGISObject(pbObjectAndPropertyConsts.ACOMaxInstances, lTemp)) > 1 Then

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

            If lLevel >= 2 Or (CDbl(v_vGISObject(pbObjectAndPropertyConsts.ACOMaxInstances, lTemp)) > 1) Then


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
            sTableList = "FROM " & sTableName & " " & sPrefix & Strings.ChrW(13) & Strings.ChrW(10)

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
                        sSelectList = sSelectList.Substring(0, sSelectList.Length - 3) & Strings.ChrW(13) & Strings.ChrW(10)

                        sSelectList = "SELECT " & sSelectList & Strings.ChrW(13) & Strings.ChrW(10) &
                                      sTableList & Strings.ChrW(13) & Strings.ChrW(10) &
                                      sWhereList & Strings.ChrW(13) & Strings.ChrW(10) &
                                      sAddToWhereList

                        sSelectList = sSelectList.Substring(0).Replace(ACSPGenDataTempDataModelName, v_sDatamodel)
                        sSelectList = sSelectList.Substring(0).Replace(ACSPGenDataTempDataModelTableName, sTableName)
                        sSelectList = sSelectList.Substring(0).Replace(ACSPGenDataTempDataModelTableAlias, sPrefix)

                        'Create the new procedure
                        sSQL = ""

                        sSQL = sSQL & "CREATE PROCEDURE " & sProcedureName & Strings.ChrW(13) & Strings.ChrW(10) &
                               "@PartyCnt INT," & Strings.ChrW(13) & Strings.ChrW(10) &
                               "@InsuranceFileCnt INT," & Strings.ChrW(13) & Strings.ChrW(10) &
                               "@RiskId INT," & Strings.ChrW(13) & Strings.ChrW(10) &
                               "@ClaimCnt INT," & Strings.ChrW(13) & Strings.ChrW(10) &
                               "@DocumentRef VARCHAR(25)" & Strings.ChrW(13) & Strings.ChrW(10)

                        'dynamically generate instance variables
                        generateInstanceVariables(sSQL, iInstanceCount)
                        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10) & "AS" & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10) & sSelectList & Strings.ChrW(13) & Strings.ChrW(10)


                        If r_oDatabase.SQLAction(sSQL:=ToSafeString(sSQL), sSQLName:="Create " & sProcedureName & " StoredProcedure", bStoredProcedure:=False) = gPMConstants.PMEReturnCode.PMTrue Then

                            'Set permissions
                            sSQL = "GRANT EXECUTE ON dbo." & sProcedureName & " TO PUBLIC"


                            If r_oDatabase.SQLAction(sSQL:=ToSafeString(sSQL), sSQLName:="Grant " & sProcedureName & " StoredProcedure", bStoredProcedure:=False) <> gPMConstants.PMEReturnCode.PMTrue Then

                                ' log error
                                result = gPMConstants.PMEReturnCode.PMFalse

                                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                                oDict.Add("v_lObjectId", v_lObjectId)
                                oDict.Add("v_lParentId", v_lParentId)
                                gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to set default permissions for " &
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


    ' ***************************************************************** '
    '
    ' Name: GetDocumentFilter
    '
    ' Description:
    '
    ' History: 27/04/2005 RKS - Created.
    '
    ' ***************************************************************** '
    Public Function GetDocumentFilter(ByRef vArray(,) As Object, ByRef r_oDatabase As dPMDAO.Database) As Integer

        Dim result As Integer = gPMConstants.PMEReturnCode.PMTrue
        Try

            r_oDatabase.Parameters.Clear()

            'Execute SQL Statement
            m_lReturn = r_oDatabase.SQLSelect(sSQL:=ACGetDocumentFilterSQL, sSQLName:=ACGetDocumentFilterName, bStoredProcedure:=ACGetDocumentFilterStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDocumentFilter Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocumentFilter", excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: GetGISUserDefineList
    '
    ' Description:
    '
    ' History: 06/02/2006  - Created.
    '
    ' ***************************************************************** '
    Public Function GetPMLookupList(ByRef r_vArray(,) As Object, ByRef r_oDatabase As dPMDAO.Database) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_oDatabase.Parameters.Clear()


            ' Developer Guide No. 85
            m_lReturn = r_oDatabase.Parameters.Add(sName:="pmproduct_id", vValue:=(DBNull.Value), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Execute SQL Statement
            m_lReturn = r_oDatabase.SQLSelect(sSQL:=ACGetPMLookupListSQL, sSQLName:=ACGetPMLookupListName, bStoredProcedure:=ACGetPMLookupListStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPMLookupList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPMLookupList", excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GenerateCaseSP
    ' Parameters: n/a
    ' Description:
    ' History:
    ' Created :
    ' ***************************************************************** '
    Private Function GenerateCaseSP(ByVal v_lObjectId As Integer, ByVal v_vGISObject(,) As Object, ByVal v_vGISProperty() As Object, ByVal v_lParentId As Integer, ByVal v_sDatamodel As String, ByVal v_lPMProductFamily As Integer, ByRef r_oDatabase As Object) As Integer



        Dim result As Integer = 0
        Const sFunctionName As String = "GenerateCaseSP"

        Dim sProcedureName, sProcedureNameOldStyle As String
        Dim iParentIsMultipleInstance As Integer
        Dim sTableName, sTemp, sPrefix As String
        Dim lParamCount As Integer
        Dim sSelectList, sTableList As String
        Dim vArray(,) As Object
        Dim llBound, lUbound As Integer
        Dim lLevel, lTemp As Integer
        Dim sAddToWhereList As String = ""
        Dim iInstanceCount As Integer
        Dim sWhereList, sSQL As String



        result = gPMConstants.PMEReturnCode.PMTrue

        ' if not the parent object

        If CDbl(v_vGISObject(pbObjectAndPropertyConsts.ACOGISObjectId, v_lObjectId)) <> v_lParentId Then

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
            sTableList = "FROM " & sTableName & " " & sPrefix & Strings.ChrW(13) & Strings.ChrW(10)

            ' Get and use property array for the current object


            vArray = v_vGISProperty(v_lObjectId)

            ' determine the level of the object
            ' as this will determine what keys stored procedures if any
            ' need to be created.
            If Informations.IsArray(vArray) Then


                llBound = vArray.GetLowerBound(1)

                lUbound = vArray.GetUpperBound(1)

                For lIDKeyPos As Integer = llBound To lUbound

                    If CDbl(vArray(pbObjectAndPropertyConsts.ACPIsIdentifyingProperty, lIDKeyPos)) = 1 Then
                        lLevel = lIDKeyPos
                    End If
                Next lIDKeyPos
            End If

            '************************
            ' Generate Get Keys Stored Procedure
            '************************

            If lLevel > 1 Or CDbl(v_vGISObject(pbObjectAndPropertyConsts.ACOMaxInstances, lTemp)) > 1 Then

                ' Use the Live claim peril table to get the keys from


                m_lReturn = CType(GenerateCaseGetKeysSP(v_sTableName:=sTableName, v_sPrefix:=sPrefix, v_sDatamodel:=v_sDatamodel, v_sProcedureName:=sProcedureName, v_sProcedureNameOldStyle:=sProcedureNameOldStyle, v_vArray:=vArray, r_oDatabase:=r_oDatabase), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            '************************
            ' Generate Get Parent Keys Stored Procedure
            '************************
            ' Added support for 3 full levels of object to be documented
            ' excluding policy binder...
            ' eg.
            '       Policy Binder
            '           Test 1
            '               Test 2
            '                   Test 3

            If lLevel >= 2 Or (CDbl(v_vGISObject(pbObjectAndPropertyConsts.ACOMaxInstances, lTemp)) > 1) Then


                m_lReturn = CType(GenerateCaseGetParentKeysSP(v_sTableName:=sTableName, v_sPrefix:=sPrefix, v_sDatamodel:=v_sDatamodel, v_sProcedureName:=sProcedureName, v_sProcedureNameOldStyle:=sProcedureNameOldStyle, v_vArray:=vArray, r_oDatabase:=r_oDatabase), gPMConstants.PMEReturnCode)


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
            sTableList = "FROM " & sTableName & " " & sPrefix & Strings.ChrW(13) & Strings.ChrW(10)

            ' Get and use property array for the current object


            vArray = v_vGISProperty(v_lObjectId)



            If ProcessPropertyArray(v_lObjectType:=GISDataModelType.GISOTRisk, r_vPropertyArray:=vArray, r_sSelectList:=sSelectList, r_sTableList:=sTableList, v_sMainGroup:="Claim", v_sProcedureName:=sProcedureName, v_lPMProductFamily:=v_lPMProductFamily, v_sDatamodel:=v_sDatamodel, r_oDatabase:=r_oDatabase, r_lParamCount:=lParamCount, v_sPrefix:=sPrefix, v_sSubGroup:="", v_sLoop1:="", r_sWhereList:=sAddToWhereList, r_iInstanceCount:=iInstanceCount, v_lLevel:=lLevel) = gPMConstants.PMEReturnCode.PMTrue Then

                If lParamCount <> 0 Then

                    'Add the rest...
                    sTableList = sTableList.Substring(0, sTableList.Length - 2)

                    sWhereList = "WHERE XXXDATAMODELNAME_policy_binder_id in (" &
                                 " SELECT XXXDATAMODELNAME_policy_binder_id from " & " XXXDATAMODELNAME_Policy_Binder where gis_policy_link_id in ( " &
                                 " Select gis_policy_link_id from gis_Policy_Link " &
                                 "Where Case_Id IN (Select max(case_id) from [CASE] Where Base_Case_Id IN (SELECT base_case_id FROM claim WHERE Claim_Id = @ClaimCnt))))"

                    ' for some bizarre reason you have to set the return value to
                    ' true for the drop procedures to do anything
                    m_lReturn = gPMConstants.PMEReturnCode.PMTrue

                    'Drop it if it's already there

                    DropExistingProcedure(v_sProcedureName:=sProcedureName, v_sPostFix:="", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

                    m_lReturn = gPMConstants.PMEReturnCode.PMTrue


                    DropExistingProcedure(v_sProcedureName:=sProcedureNameOldStyle, v_sPostFix:="", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                        'remove the last comma and vbcrlf
                        sSelectList = sSelectList.Substring(0, sSelectList.Length - 3) & Strings.ChrW(13) & Strings.ChrW(10)

                        sSelectList = "SELECT " & sSelectList & Strings.ChrW(13) & Strings.ChrW(10) &
                                      sTableList & Strings.ChrW(13) & Strings.ChrW(10) &
                                      sWhereList & Strings.ChrW(13) & Strings.ChrW(10) &
                                      sAddToWhereList

                        sSelectList = sSelectList.Substring(0).Replace(ACSPGenDataTempDataModelName, v_sDatamodel)
                        sSelectList = sSelectList.Substring(0).Replace(ACSPGenDataTempDataModelTableName, sTableName)
                        sSelectList = sSelectList.Substring(0).Replace(ACSPGenDataTempDataModelTableAlias, sPrefix)

                        'Create the new procedure
                        sSQL = ""

                        sSQL = sSQL & "CREATE PROCEDURE " & sProcedureName & Strings.ChrW(13) & Strings.ChrW(10) &
                               "@PartyCnt INT," & Strings.ChrW(13) & Strings.ChrW(10) &
                               "@InsuranceFileCnt INT," & Strings.ChrW(13) & Strings.ChrW(10) &
                               "@RiskId INT," & Strings.ChrW(13) & Strings.ChrW(10) &
                               "@ClaimCnt INT," & Strings.ChrW(13) & Strings.ChrW(10) &
                               "@DocumentRef VARCHAR(25)" & Strings.ChrW(13) & Strings.ChrW(10)

                        'dynamically generate instance variables
                        generateInstanceVariables(sSQL, iInstanceCount)
                        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10) & "AS" & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10) & sSelectList & Strings.ChrW(13) & Strings.ChrW(10)


                        If r_oDatabase.SQLAction(sSQL:=ToSafeString(sSQL), sSQLName:="Create " & sProcedureName & " StoredProcedure", bStoredProcedure:=False) = gPMConstants.PMEReturnCode.PMTrue Then

                            'Set permissions
                            sSQL = "GRANT EXECUTE ON dbo." & sProcedureName & " TO PUBLIC"


                            If r_oDatabase.SQLAction(sSQL:=ToSafeString(sSQL), sSQLName:="Grant " & sProcedureName & " StoredProcedure", bStoredProcedure:=False) <> gPMConstants.PMEReturnCode.PMTrue Then

                                ' log error
                                result = gPMConstants.PMEReturnCode.PMFalse

                                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                                oDict.Add("v_lObjectId", v_lObjectId)
                                oDict.Add("v_lParentId", v_lParentId)
                                gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to set default permissions for " &
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


    ' ***************************************************************** '
    ' Name: GenerateCaseGetKeysSP
    ' Parameters: n/a
    ' Description:
    ' History:
    ' Created :
    ' ***************************************************************** '
    Private Function GenerateCaseGetKeysSP(ByVal v_sTableName As String, ByVal v_sPrefix As String, ByVal v_sDatamodel As String, ByVal v_sProcedureName As String,
                                           ByVal v_sProcedureNameOldStyle As String, ByVal v_vArray(,) As Object,
                                           ByRef r_oDatabase As dPMDAO.Database) As Integer

        Dim result As Integer = gPMConstants.PMEReturnCode.PMTrue
        Const sFunctionName As String = "GenerateCaseGetKeysSP"

        Dim sSelectList As String = ""
        Dim sTableList As String = ""
        Dim sWhereList As StringBuilder
        Dim lLevel As Integer
        Dim sSQL As String = ""

        Dim llBound, lUbound As Integer

        ' Drop Existing Stored Procedure if it is there
        ' Use both old and new formats to ensure old procedure removed
        ' ignore return values as if the  procedures dont exist it errors

        m_lReturn = gPMConstants.PMEReturnCode.PMTrue

        DropExistingProcedure(v_sProcedureName:=v_sProcedureName, v_sPostFix:="_get_keys", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

        m_lReturn = gPMConstants.PMEReturnCode.PMTrue

        DropExistingProcedure(v_sProcedureName:=v_sProcedureNameOldStyle, v_sPostFix:="_get_keys", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)


        'Build up the template for the query
        sTableList = "FROM XXXDATAMODELTABLENAME as XXXDATAMODELTABLEALIAS"

        sWhereList = New StringBuilder("WHERE XXXDATAMODELNAME_policy_binder_id in (" &
                     " SELECT XXXDATAMODELNAME_policy_binder_id from " &
                     " XXXDATAMODELNAME_Policy_Binder where gis_policy_link_id in ( " &
                     " Select gis_policy_link_id from gis_Policy_Link " &
                     "Where Case_Id IN (Select max(case_id) from [CASE] Where Base_Case_Id IN (SELECT base_case_id FROM claim WHERE Claim_Id =@ClaimCnt))))")


        'Initialise id key level indicator
        lLevel = 0

        ' get the position of the last identifying key in the array
        ' the unique key of the current table
        If Informations.IsArray(v_vArray) Then

            llBound = v_vArray.GetLowerBound(1)
            lUbound = v_vArray.GetUpperBound(1)

            For lIDKeyPos As Integer = llBound To lUbound

                If CDbl(v_vArray(pbObjectAndPropertyConsts.ACPIsIdentifyingProperty, lIDKeyPos)) = 1 Then
                    lLevel = lIDKeyPos
                End If
            Next lIDKeyPos

            ' add the local level key to the select list and any other keys to the
            ' where clause.
            ' start from lbound + 1 to exclude the initial policy binder id as this is already
            ' added to the initial "where" clause
            For lIDKeyPos As Integer = llBound + 1 To lUbound

                ' if item is id key

                If CDbl(v_vArray(pbObjectAndPropertyConsts.ACPIsIdentifyingProperty, lIDKeyPos)) = 1 Then

                    ' if its not the local table's unique id key
                    If lIDKeyPos < lLevel Then
                        ' add it to the where list

                        sWhereList.Append("AND XXXDATAMODELTABLEALIAS." & CStr(v_vArray(pbObjectAndPropertyConsts.ACPColumnName, lIDKeyPos)) & " = @Instance" & CStr(lIDKeyPos) & Strings.ChrW(13) & Strings.ChrW(10))
                    Else
                        ' else add the local tables unique id key to the select list
                        ' as this is what we want to return

                        sSelectList = "SELECT XXXDATAMODELTABLEALIAS." & CStr(v_vArray(pbObjectAndPropertyConsts.ACPColumnName, lIDKeyPos))
                    End If
                End If

            Next lIDKeyPos

        End If

        ' build the full select list
        sSelectList = sSelectList & Strings.ChrW(13) & Strings.ChrW(10) &
                      sTableList & Strings.ChrW(13) & Strings.ChrW(10) &
                      sWhereList.ToString()

        ' update template sql to contain relevant datamodel, table, prefixes
        sSelectList = sSelectList.Substring(0).Replace(ACSPGenDataTempDataModelName, v_sDatamodel)
        sSelectList = sSelectList.Substring(0).Replace(ACSPGenDataTempDataModelTableName, v_sTableName)
        sSelectList = sSelectList.Substring(0).Replace(ACSPGenDataTempDataModelTableAlias, v_sPrefix)

        'Create the new procedure
        sSQL = "CREATE PROCEDURE " & v_sProcedureName & "_get_keys " & Strings.ChrW(13) & Strings.ChrW(10) &
               "@PartyCnt INT," & Strings.ChrW(13) & Strings.ChrW(10) &
               "@InsuranceFileCnt INT," & Strings.ChrW(13) & Strings.ChrW(10) &
               "@RiskId INT = NULL," & Strings.ChrW(13) & Strings.ChrW(10) &
               "@ClaimCnt INT," & Strings.ChrW(13) & Strings.ChrW(10) &
               "@DocumentRef VARCHAR(25)"

        ' dynamically generate instance variables
        ' the number of instance variables is determined by how far down the
        ' tree structure the object is..
        generateInstanceVariables(sSQL, lLevel)

        ' combine the sql statments to produce the stored procedure sql
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10) & "AS" & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10) & sSelectList & Strings.ChrW(13) & Strings.ChrW(10)

        ' add stored procedure to database
        If r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & v_sProcedureName & "_get_keys StoredProcedure", bStoredProcedure:=False) = gPMConstants.PMEReturnCode.PMTrue Then

            'Set permissions on stored procedure
            sSQL = "GRANT EXECUTE ON dbo." & v_sProcedureName & "_get_keys TO PUBLIC" & Strings.ChrW(13) & Strings.ChrW(10)

            If r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Grant " & v_sProcedureName & "_get_keys StoredProcedure", bStoredProcedure:=False) <> gPMConstants.PMEReturnCode.PMTrue Then

                ' log error
                result = gPMConstants.PMEReturnCode.PMFalse

                gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to set default permissions for " &
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
    ' Name: GenerateCaseGetParentKeysSP
    ' Parameters: n/a
    ' Description:
    ' History:
    ' Created :
    ' ***************************************************************** '
    Private Function GenerateCaseGetParentKeysSP(ByVal v_sTableName As String, ByVal v_sPrefix As String, ByVal v_sDatamodel As String, ByVal v_sProcedureName As String, ByVal v_sProcedureNameOldStyle As String, ByVal v_vArray(,) As Object, ByRef r_oDatabase As dPMDAO.Database) As Integer

        Dim result As Integer = gPMConstants.PMEReturnCode.PMTrue
        Const sFunctionName As String = "GenerateCaseGetParentKeysSP"

        Dim sSelectList, sTableList, sWhereList, sSQL As String

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

            sWhereList = "WHERE XXXDATAMODELNAME_policy_binder_id in (" &
                         " SELECT XXXDATAMODELNAME_policy_binder_id from " &
                         " XXXDATAMODELNAME_Policy_Binder where gis_policy_link_id in ( " &
                         " Select gis_policy_link_id from gis_Policy_Link " &
                         "Where Case_Id IN (Select max(case_id) from [CASE] Where Base_Case_Id IN (SELECT base_case_id FROM claim WHERE Claim_Id =@ClaimCnt))))"


            ' build the full select list
            sSelectList = sSelectList & Strings.ChrW(13) & Strings.ChrW(10) &
                          sTableList & Strings.ChrW(13) & Strings.ChrW(10) &
                          sWhereList

            ' update template sql to contain relevant datamodel, table, prefixes
            sSelectList = sSelectList.Substring(0).Replace(ACSPGenDataTempDataModelName, v_sDatamodel)
            sSelectList = sSelectList.Substring(0).Replace(ACSPGenDataTempDataModelTableName, v_sTableName)
            sSelectList = sSelectList.Substring(0).Replace(ACSPGenDataTempDataModelTableAlias, v_sPrefix)

            'Create the new procedure
            sSQL = ""

            sSQL = sSQL & "CREATE PROCEDURE " & v_sProcedureName & "_get_parent_key @PartyCnt INT," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "@InsuranceFileCnt INT," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "@RiskId INT = NULL," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "@ClaimCnt INT," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "@DocumentRef VARCHAR(25)," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "@Instance1 INT," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "@Instance2 INT," & Strings.ChrW(13) & Strings.ChrW(10) &
                   "@Instance3 INT" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "AS" & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & sSelectList & Strings.ChrW(13) & Strings.ChrW(10)

            If r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & v_sProcedureName & "_get_parent_key StoredProcedure", bStoredProcedure:=False) = gPMConstants.PMEReturnCode.PMTrue Then
                'Set permissions
                sSQL = "GRANT EXECUTE ON dbo." & v_sProcedureName & "_get_parent_key TO PUBLIC" & Strings.ChrW(13) & Strings.ChrW(10)

                If r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Grant " & v_sProcedureName & "_get_parent_key StoredProcedure", bStoredProcedure:=False) <> gPMConstants.PMEReturnCode.PMTrue Then

                    ' log error
                    result = gPMConstants.PMEReturnCode.PMFalse

                    gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to set default permissions for " &
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

            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to drop stored procedure:" &
                                          v_sProcedureName & " OR " & v_sProcedureNameOldStyle, vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)

        End If

        Return result

    End Function
End Module
