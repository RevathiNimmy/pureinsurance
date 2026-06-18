Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Imports SharedFiles

Module StoredProcSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	
	' Error Code (Private)
	Private m_lReturn As Integer
	
	Private Const ACClass As String = ""
	
	'****************
	' MEvans : 25-07-2003 : 223 document production changes
	Private Const ACWPFFieldName As Short = 0
	Private Const ACWPFColumnName As Short = 1
	Private Const ACWPFColumnType As Short = 2
	Private Const ACWPFDisplayName As Short = 3
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
	Public Function GenerateCopyAssocClientsForPolicySP(ByRef r_oDatabase As dPMDAO.Database) As Object 
		
		Dim sProcedureName As String 
		Dim sAssClientTable As String 
		Dim sTableFields As String 
		Dim sSQL As String 
		Dim sFieldName As String 
		Dim vArray(,) As Object 
		Dim vFieldArray(,) As Object 
		Dim i As Short 
		Dim iField As Short 
		
		Try
		
		'UPGRADE_WARNING: Couldn't resolve default property of object GenerateCopyAssocClientsForPolicySP. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		GenerateCopyAssocClientsForPolicySP = gPMConstants.PMEReturnCode.PMTrue
		
		sProcedureName = "spg_copy_associated_clients"
		
		'Drop it if it's already there
		DropExistingProcedure(sProcedureName, v_sPostFix:="", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)
		
		If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
			'UPGRADE_WARNING: Couldn't resolve default property of object GenerateCopyAssocClientsForPolicySP. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			GenerateCopyAssocClientsForPolicySP = gPMConstants.PMEReturnCode.PMFalse
			Exit Function
		End If
		
		' Set up header of SP
		sSQL = "SET QUOTED_IDENTIFIER OFF" & vbCrLf & "SET ANSI_NULLS ON" & vbCrLf
		m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Pre Create " & sProcedureName, bStoredProcedure:=False)
		
		sSQL = "CREATE PROCEDURE " & sProcedureName & vbCrLf
		sSQL = sSQL & "                 @old_insurance_file_cnt int," & vbCrLf
		sSQL = sSQL & "                 @new_insurance_file_cnt int" & vbCrLf
		sSQL = sSQL & vbCrLf
		sSQL = sSQL & "AS " & vbCrLf
		
		' Get an array of all the associated client objects
		r_oDatabase.Parameters.Clear()
        m_lReturn = r_oDatabase.SQLSelect(sSQL:=ACGetAssociatClientObjectsSQL, sSQLName:=ACGetAssociatClientObjectsName, bStoredProcedure:=ACGetAssociatClientObjectsStored, lnumberrecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            'UPGRADE_WARNING: Couldn't resolve default property of object GenerateCopyAssocClientsForPolicySP. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            GenerateCopyAssocClientsForPolicySP = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        ' If no objects, exit function
        If Not IsArray(vArray) Then
            Exit Function
        End If

        ' Loop through all associated client objects
        For i = LBound(vArray, 2) To UBound(vArray, 2)

            ' Store table name
            'UPGRADE_WARNING: Couldn't resolve default property of object vArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            sAssClientTable = vArray(0, i)

            ' Get all user defined fields for the current associated client table
            sTableFields = ""
            r_oDatabase.Parameters.Clear()
            m_lReturn = r_oDatabase.Parameters.Add(sName:="table_name", vValue:=sAssClientTable, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'UPGRADE_WARNING: Couldn't resolve default property of object GenerateCopyAssocClientsForPolicySP. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                GenerateCopyAssocClientsForPolicySP = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = r_oDatabase.SQLSelect(sSQL:=ACGetAssociatClientFieldsSQL, sSQLName:=ACGetAssociatClientFieldsName, bStoredProcedure:=ACGetAssociatClientFieldsStored, lnumberrecords:=gPMConstants.PMAllRecords, vResultArray:=vFieldArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'UPGRADE_WARNING: Couldn't resolve default property of object GenerateCopyAssocClientsForPolicySP. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                GenerateCopyAssocClientsForPolicySP = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' Build user fields
            If IsArray(vFieldArray) Then
                For iField = LBound(vFieldArray, 2) To UBound(vFieldArray, 2)
                    'UPGRADE_WARNING: Couldn't resolve default property of object vFieldArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    sFieldName = LCase(vFieldArray(0, iField))
                    If sFieldName <> "insurance_file_cnt" Then
                        sTableFields = sTableFields & "        " & sFieldName & "," & vbCrLf
                    End If
                Next
                sTableFields = Left(sTableFields, Len(sTableFields) - 3)

                ' Add to stored procedure SQL
                sSQL = sSQL & vbCrLf
                sSQL = sSQL & "INSERT INTO " & sAssClientTable & " (" & vbCrLf
                sSQL = sSQL & "        insurance_file_cnt," & vbCrLf
                sSQL = sSQL & sTableFields
                sSQL = sSQL & ")" & vbCrLf
                sSQL = sSQL & "SELECT  @new_insurance_file_cnt," & vbCrLf
                sSQL = sSQL & sTableFields & vbCrLf
                sSQL = sSQL & "From " & sAssClientTable & vbCrLf
                sSQL = sSQL & "WHERE insurance_file_cnt = @old_insurance_file_cnt" & vbCrLf

            End If

            'UPGRADE_WARNING: Couldn't resolve default property of object vFieldArray. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            vFieldArray = Nothing

        Next

        m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & sProcedureName, bStoredProcedure:=False)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            'UPGRADE_WARNING: Couldn't resolve default property of object GenerateCopyAssocClientsForPolicySP. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            GenerateCopyAssocClientsForPolicySP = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        Exit Function

		Catch ex As Exception

        'UPGRADE_WARNING: Couldn't resolve default property of object GenerateCopyAssocClientsForPolicySP. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        GenerateCopyAssocClientsForPolicySP = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GenerateCopyAssocClientsForPolicySP Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateCopyAssocClientsForPolicySP", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

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

        Dim sProcedureName As String
        Dim sSQL As String

        Try

        GenerateDMRelatedStoredProcedures = gPMConstants.PMEReturnCode.PMTrue

        sProcedureName = "spg_" & v_sDatamodel & "_copy_sums_insured"

        'Drop it if it's already there
        DropExistingProcedure(sProcedureName, v_sPostFix:="", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateDMRelatedStoredProcedures = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        sSQL = "SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON" & vbCrLf
        m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Pre Create " & sProcedureName, bStoredProcedure:=False)

        sSQL = "CREATE PROCEDURE " & sProcedureName & vbCrLf
        sSQL = sSQL & "                 @old_policy_link_id int," & vbCrLf
        sSQL = sSQL & "                 @new_policy_link_id int" & vbCrLf
        sSQL = sSQL & vbCrLf
        sSQL = sSQL & "AS " & vbCrLf
        sSQL = sSQL & vbCrLf
        sSQL = sSQL & "--Take advantage here of the identicalityness of gis_policy_link_id and rsa_policy_binder_id (or whatever)" & vbCrLf
        sSQL = sSQL & "INSERT INTO RSA_sum_insured (" & vbCrLf
        sSQL = sSQL & "        RSA_Policy_binder_id," & vbCrLf
        sSQL = sSQL & "        sum_insured_type_id," & vbCrLf
        sSQL = sSQL & "        sequence_id," & vbCrLf
        sSQL = sSQL & "        description," & vbCrLf
        sSQL = sSQL & "        reference," & vbCrLf
        sSQL = sSQL & "        sum_insured," & vbCrLf
        sSQL = sSQL & "        date_added," & vbCrLf
        sSQL = sSQL & "        date_deleted," & vbCrLf
        sSQL = sSQL & "        is_valuation_required," & vbCrLf
        sSQL = sSQL & "        valuation_date," & vbCrLf
        sSQL = sSQL & "        rate," & vbCrLf
        sSQL = sSQL & "        premium" & vbCrLf
        sSQL = sSQL & ")"
        sSQL = sSQL & "SELECT  @new_policy_link_id," & vbCrLf
        sSQL = sSQL & "        sum_insured_type_id," & vbCrLf
        sSQL = sSQL & "        sequence_id," & vbCrLf
        sSQL = sSQL & "        description," & vbCrLf
        sSQL = sSQL & "        reference," & vbCrLf
        sSQL = sSQL & "        sum_insured," & vbCrLf
        sSQL = sSQL & "        date_added," & vbCrLf
        sSQL = sSQL & "        date_deleted," & vbCrLf
        sSQL = sSQL & "        is_valuation_required," & vbCrLf
        sSQL = sSQL & "        valuation_date," & vbCrLf
        sSQL = sSQL & "        rate," & vbCrLf
        sSQL = sSQL & "        premium" & vbCrLf
        sSQL = sSQL & "From RSA_sum_insured" & vbCrLf
        sSQL = sSQL & "WHERE   RSA_Policy_binder_id = @old_policy_link_id" & vbCrLf

        sSQL = Replace(sSQL, "RSA", v_sDatamodel)

        m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & sProcedureName, bStoredProcedure:=False)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            GenerateDMRelatedStoredProcedures = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        Exit Function

        Catch ex As Exception

        GenerateDMRelatedStoredProcedures = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GenerateDMRelatedStoredProcedures Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateDMRelatedStoredProcedures", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Exit Function
        

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
    Private Sub DropExistingProcedure(ByVal v_sProcedureName As String, ByVal v_sPostFix As String, ByRef r_lRetval As Object, ByRef r_oDatabase As dPMDAO.Database)

        ' Debug message
        Debug.Print(VB.Timer() & ": Entering " & ACApp & "." & ACClass & ".DropExistingProcedure")


        Dim sSQL As String

        If r_lRetval <> gPMConstants.PMEReturnCode.PMTrue Then Exit Sub

        r_oDatabase.Parameters.Clear()

        sSQL = ACDropStoredProcedureSQL

        AddParameter(r_oDatabase, sSQL, m_lReturn, "sName", v_sProcedureName & v_sPostFix, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:=ACDropStoredProcedureName, bStoredProcedure:=ACDropStoredProcedureStored)
        End If

        ' Debug message
        Debug.Print(VB.Timer() & ": Exiting " & ACApp & "." & ACClass & ".DropExistingProcedure")

        Exit Sub

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

        Dim sSelectList As String
        Dim sTableList As String
        Dim sWhereList1 As String
        Dim sWhereList2 As String
        Dim sSQL As String


        GenerateGetParentStoredProcedure = gPMConstants.PMEReturnCode.PMTrue

        'First we do the main query
        'UPGRADE_WARNING: Couldn't resolve default property of object vArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        sSelectList = "SELECT DISTINCT " & sPrefix & "." & vArray(ACPColumnName, 1) & "," & vbCrLf

        sTableList = "FROM " & sTableName & " " & sPrefix & ", " & vbCrLf & v_sDatamodel & "_Policy_binder pb," & vbCrLf & "GIS_policy_link gpl," & vbCrLf & "insurance_file ifi" & vbCrLf

        sWhereList1 = "WHERE ifi.insurance_file_cnt = @InsuranceFileCnt" & vbCrLf & "AND gpl.insurance_file_cnt = ifi.insurance_file_cnt" & vbCrLf & "AND gpl.Risk_id is null" & vbCrLf & "AND pb.gis_policy_link_id = gpl.gis_policy_link_id" & vbCrLf & "AND " & sPrefix & "." & v_sDatamodel & "_policy_binder_id = pb." & v_sDatamodel & "_Policy_binder_id" & vbCrLf

        sWhereList2 = "WHERE ifi.insurance_file_cnt = @InsuranceFileCnt" & vbCrLf & "AND gpl.Risk_id = @RiskId" & vbCrLf & "AND pb.gis_policy_link_id = gpl.gis_policy_link_id" & vbCrLf & "AND " & sPrefix & "." & v_sDatamodel & "_policy_binder_id = pb." & v_sDatamodel & "_Policy_binder_id" & vbCrLf

        'Drop it if it's already there
        DropExistingProcedure(v_sProcedureName:=sProcedureName, v_sPostFix:="_get_parent_key", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

        DropExistingProcedure(v_sProcedureName:=sProcedureNameOldStyle, v_sPostFix:="_get_parent_key", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateGetParentStoredProcedure = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        'remove the last comma and vbcrlf
        sSelectList = Left(sSelectList, Len(sSelectList) - 3) & vbCrLf

        sSelectList = "If @RiskId Is Null" & vbCrLf & sSelectList & sTableList & sWhereList1 & "Else" & vbCrLf & sSelectList & sTableList & sWhereList2

        'Create the new procedure
        sSQL = ""

        sSQL = sSQL & "CREATE PROCEDURE " & sProcedureName & "_get_parent_key @PartyCnt INT," & vbCrLf & "@InsuranceFileCnt INT," & vbCrLf & "@RiskId INT = NULL," & vbCrLf & "@ClaimCnt INT," & vbCrLf & "@DocumentRef VARCHAR(25)," & vbCrLf & "@Instance1 INT," & vbCrLf & "@Instance2 INT," & vbCrLf & "@Instance3 INT" & vbCrLf & "AS" & vbCrLf & vbCrLf

        sSQL = sSQL & sSelectList & vbCrLf

        m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & sProcedureName & "_get_parent_key StoredProcedure", bStoredProcedure:=False)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateGetParentStoredProcedure = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        'Set permissions

        sSQL = "GRANT EXECUTE ON dbo." & sProcedureName & "_get_parent_key TO PUBLIC" & vbCrLf

        m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Grant " & sProcedureName & "_get_parent_key StoredProcedure", bStoredProcedure:=False)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateGetParentStoredProcedure = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        Exit Function

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
    Private Function GenerateGetKeysStoredProcedure(ByRef sTableName As String, ByRef sPrefix As String, ByVal v_sDatamodel As String, ByRef sProcedureName As String, ByRef sProcedureNameOldStyle As String, ByRef vArray(,) As Object, ByRef v_iIsParentMultipleInstance As Short, ByRef r_oDatabase As dPMDAO.Database) As Integer

        Dim sSelectList As String
        Dim sTableList As String
        Dim sWhereList1 As String
        Dim sWhereList2 As String
        Dim lLevel As Integer
        Dim lTemp2 As Integer
        Dim sSQL As String


        GenerateGetKeysStoredProcedure = gPMConstants.PMEReturnCode.PMTrue

        'First we do the main query
        sSelectList = "SELECT "

        sTableList = "FROM " & sTableName & " " & sPrefix & ", " & vbCrLf & v_sDatamodel & "_Policy_binder pb," & vbCrLf & "GIS_policy_link gpl," & vbCrLf & "insurance_file ifi" & vbCrLf

        sWhereList1 = "WHERE ifi.insurance_file_cnt = @InsuranceFileCnt" & vbCrLf & "AND gpl.insurance_file_cnt = ifi.insurance_file_cnt" & vbCrLf & "AND gpl.Risk_id is null" & vbCrLf & "AND pb.gis_policy_link_id = gpl.gis_policy_link_id" & vbCrLf & "AND " & sPrefix & "." & v_sDatamodel & "_policy_binder_id = pb." & v_sDatamodel & "_Policy_binder_id" & vbCrLf

        sWhereList2 = "WHERE ifi.insurance_file_cnt = @InsuranceFileCnt" & vbCrLf & "AND gpl.Risk_id = @RiskId" & vbCrLf & "AND pb.gis_policy_link_id = gpl.gis_policy_link_id" & vbCrLf & "AND " & sPrefix & "." & v_sDatamodel & "_policy_binder_id = pb." & v_sDatamodel & "_Policy_binder_id" & vbCrLf

        'What's the highest number key?
        lLevel = 0

        If IsArray(vArray) Then
            For lTemp2 = LBound(vArray, 2) To UBound(vArray, 2)
                'UPGRADE_WARNING: Couldn't resolve default property of object vArray(ACPIsIdentifyingProperty, lTemp2). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If (vArray(ACPIsIdentifyingProperty, lTemp2) = 1) Then
                    lLevel = lTemp2
                End If
            Next lTemp2
        End If

        If v_iIsParentMultipleInstance > 0 Then
        End If

        If IsArray(vArray) Then
            For lTemp2 = LBound(vArray, 2) + 1 To UBound(vArray, 2)

                'UPGRADE_WARNING: Couldn't resolve default property of object vArray(ACPIsIdentifyingProperty, lTemp2). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If (vArray(ACPIsIdentifyingProperty, lTemp2) = 1) Then

                    If (lTemp2 < lLevel) Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object vArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        sWhereList1 = sWhereList1 & "AND " & sPrefix & "." & vArray(ACPColumnName, lTemp2) & " = @Instance" & lTemp2 + v_iIsParentMultipleInstance & vbCrLf
                        'UPGRADE_WARNING: Couldn't resolve default property of object vArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        sWhereList2 = sWhereList2 & "AND " & sPrefix & "." & vArray(ACPColumnName, lTemp2) & " = @Instance" & lTemp2 + v_iIsParentMultipleInstance & vbCrLf
                    Else
                        'UPGRADE_WARNING: Couldn't resolve default property of object vArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        sSelectList = sSelectList & sPrefix & "." & vArray(ACPColumnName, lTemp2) & "," & vbCrLf
                    End If
                End If

            Next lTemp2
        End If

        'Drop it if it's already there
        DropExistingProcedure(v_sProcedureName:=sProcedureName, v_sPostFix:="_get_keys", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

        DropExistingProcedure(v_sProcedureName:=sProcedureNameOldStyle, v_sPostFix:="_get_keys", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            ' RAG 2003-10-09
            ' Don't fail the whole thing here as the stored procs may not exist,
            ' so the delete fails delete them !
            ' E.g. Folgate import

            ' GenerateGetKeysStoredProcedure = PMFalse
            ' Exit Function
        End If

        'remove the last comma and vbcrlf
        sSelectList = Left(sSelectList, Len(sSelectList) - 3) & vbCrLf

        sSelectList = "If @RiskId Is Null" & vbCrLf & sSelectList & sTableList & sWhereList1 & "Else" & vbCrLf & sSelectList & sTableList & sWhereList2

        'Create the new procedure
        sSQL = ""

        sSQL = sSQL & "CREATE PROCEDURE " & sProcedureName & "_get_keys @PartyCnt INT," & vbCrLf & "@InsuranceFileCnt INT," & vbCrLf & "@RiskId INT = NULL," & vbCrLf & "@ClaimCnt INT," & vbCrLf & "@DocumentRef VARCHAR(25)"

        'dynamically generate instance variables
        generateInstanceVariables(sSQL, lLevel)
        sSQL = sSQL & vbCrLf & "AS" & vbCrLf & vbCrLf & sSelectList & vbCrLf

        m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & sProcedureName & "_get_keys StoredProcedure", bStoredProcedure:=False)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateGetKeysStoredProcedure = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        'Set permissions

        sSQL = "GRANT EXECUTE ON dbo." & sProcedureName & "_get_keys TO PUBLIC" & vbCrLf

        m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Grant " & sProcedureName & "_get_keys StoredProcedure", bStoredProcedure:=False)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateGetKeysStoredProcedure = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        Exit Function

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
    Private Function GenerateKeySPAssociatedClient(ByRef sTableName As String, ByRef sPrefix As String, ByVal v_sDatamodel As String, ByRef sProcedureName As String, ByRef sProcedureNameOldStyle As String, ByRef vArray As Object, ByRef v_iIsParentMultipleInstance As Short, ByRef r_oDatabase As dPMDAO.Database) As Integer

        Dim sSQL As String


        GenerateKeySPAssociatedClient = gPMConstants.PMEReturnCode.PMTrue

        ' Part 1 - get keys

        'Drop it if it's already there
        DropExistingProcedure(v_sProcedureName:=sProcedureName, v_sPostFix:="_get_keys", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

        DropExistingProcedure(v_sProcedureName:=sProcedureNameOldStyle, v_sPostFix:="_get_keys", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateKeySPAssociatedClient = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        'Create the new procedure
        sSQL = ""
        sSQL = sSQL & "CREATE PROCEDURE " & sProcedureName & "_get_keys" & vbCrLf & "@PartyCnt INT," & vbCrLf & "@InsuranceFileCnt INT," & vbCrLf & "@RiskId INT = NULL," & vbCrLf & "@ClaimCnt INT," & vbCrLf & "@DocumentRef VARCHAR(25)," & vbCrLf & "@Instance1 INT," & vbCrLf & "@Instance2 INT," & vbCrLf & "@Instance3 INT"

        sSQL = sSQL & vbCrLf & "AS" & vbCrLf & vbCrLf

        ' put the bit that does the work in
        sSQL = sSQL & "If @RiskId Is Null" & vbCrLf
        sSQL = sSQL & "SELECT pc.party_cnt" & vbCrLf
        sSQL = sSQL & "  FROM policy_client pc" & vbCrLf
        ' PW200803 - CQ1912 - use file instead of folder
        sSQL = sSQL & " WHERE pc.insurance_file_cnt = @InsuranceFileCnt" & vbCrLf
        sSQL = sSQL & "   AND pc.risk_cnt is NULL" & vbCrLf
        sSQL = sSQL & "else" & vbCrLf
        sSQL = sSQL & "SELECT pc.party_cnt" & vbCrLf
        sSQL = sSQL & "  FROM policy_client pc" & vbCrLf
        ' PW200803 - CQ1912 - use file instead of folder
        sSQL = sSQL & " WHERE pc.insurance_file_cnt = @InsuranceFileCnt" & vbCrLf
        sSQL = sSQL & "   AND (pc.risk_cnt = @RiskId OR pc.risk_cnt is NULL)" & vbCrLf

        m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & sProcedureName & "_get_keys StoredProcedure", bStoredProcedure:=False)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateKeySPAssociatedClient = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        'Set permissions
        sSQL = "GRANT EXECUTE ON dbo." & sProcedureName & "_get_keys TO PUBLIC" & vbCrLf

        m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Grant " & sProcedureName & "_get_keys StoredProcedure", bStoredProcedure:=False)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateKeySPAssociatedClient = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        ' Part 2 - parent key

        'Drop it if it's already there
        DropExistingProcedure(v_sProcedureName:=sProcedureName, v_sPostFix:="_get_parent_key", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

        DropExistingProcedure(v_sProcedureName:=sProcedureNameOldStyle, v_sPostFix:="_get_parent_key", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateKeySPAssociatedClient = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        'Create the new procedure
        sSQL = ""
        sSQL = sSQL & "CREATE PROCEDURE " & sProcedureName & "_get_parent_key" & vbCrLf & "@PartyCnt INT," & vbCrLf & "@InsuranceFileCnt INT," & vbCrLf & "@RiskId INT = NULL," & vbCrLf & "@ClaimCnt INT," & vbCrLf & "@DocumentRef VARCHAR(25)," & vbCrLf & "@Instance1 INT," & vbCrLf & "@Instance2 INT," & vbCrLf & "@Instance3 INT"

        sSQL = sSQL & vbCrLf & "AS" & vbCrLf & vbCrLf

        ' put the bit that does the work in
        sSQL = sSQL & "SELECT @RiskId" & vbCrLf

        m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & sProcedureName & "_get_parent_key StoredProcedure", bStoredProcedure:=False)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateKeySPAssociatedClient = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        'Set permissions
        sSQL = "GRANT EXECUTE ON dbo." & sProcedureName & "_get_parent_key TO PUBLIC" & vbCrLf

        m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Grant " & sProcedureName & "_get_parent_key StoredProcedure", bStoredProcedure:=False)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateKeySPAssociatedClient = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        Exit Function

    End Function

    '***********************************************************************
    ' Name: GenerateKeySPDisclosure
    '
    ' Description: Generate the get_keys and get_parent_key stored procs
    '              for associated clients / disclosures
    '
    ' History: PW280703 - PS229 - created
    '***********************************************************************
    Private Function GenerateKeySPDisclosure(ByRef sTableName As String, ByRef sPrefix As String, ByVal v_sDatamodel As String, ByRef sProcedureName As String, ByRef sProcedureNameOldStyle As String, ByRef vArray As Object, ByRef v_iIsParentMultipleInstance As Short, ByRef r_oDatabase As dPMDAO.Database) As Integer

        Dim sSQL As String


        GenerateKeySPDisclosure = gPMConstants.PMEReturnCode.PMTrue

        ' Part 1 - get keys

        'Drop it if it's already there
        DropExistingProcedure(v_sProcedureName:=sProcedureName, v_sPostFix:="_get_keys", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

        DropExistingProcedure(v_sProcedureName:=sProcedureNameOldStyle, v_sPostFix:="_get_keys", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateKeySPDisclosure = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        'Create the new procedure
        sSQL = ""
        sSQL = sSQL & "CREATE PROCEDURE " & sProcedureName & "_get_keys" & vbCrLf & "@PartyCnt INT," & vbCrLf & "@InsuranceFileCnt INT," & vbCrLf & "@RiskId INT = NULL," & vbCrLf & "@ClaimCnt INT," & vbCrLf & "@DocumentRef VARCHAR(25)," & vbCrLf & "@Instance1 INT," & vbCrLf & "@Instance2 INT," & vbCrLf & "@Instance3 INT"

        sSQL = sSQL & vbCrLf & "AS" & vbCrLf & vbCrLf

        ' NB - these where clauses are based on those in the associated_client_sel
        ' xml stored procedure that selects associated clients and disclosures
        ' for the risk screen in cGISDataSetControl (BuildAssocClientSelSP).

        ' put the bit that does the work in
        sSQL = sSQL & "DECLARE @insurance_folder_cnt INT" & vbCrLf
        sSQL = sSQL & vbCrLf
        sSQL = sSQL & "    SELECT pcv.party_conviction_id" & vbCrLf
        sSQL = sSQL & "      FROM party_conviction pcv" & vbCrLf
        sSQL = sSQL & "INNER JOIN policy_client pc "
        sSQL = sSQL & "ON pcv.party_cnt = pc.party_cnt" & vbCrLf
        sSQL = sSQL & "     WHERE pc.insurance_file_cnt = @InsuranceFileCnt" & vbCrLf
        sSQL = sSQL & "       AND pc.is_insured = 1" & vbCrLf
        sSQL = sSQL & "       AND pcv.risk_cnt IS NULL" & vbCrLf
        sSQL = sSQL & "       AND pcv.party_cnt = @instance2" & vbCrLf
        sSQL = sSQL & "     UNION" & vbCrLf
        sSQL = sSQL & "    SELECT pcv.party_conviction_id" & vbCrLf
        sSQL = sSQL & "      FROM party_conviction pcv" & vbCrLf
        sSQL = sSQL & "INNER JOIN policy_client pc "
        sSQL = sSQL & "ON pcv.party_cnt = pc.party_cnt" & vbCrLf
        sSQL = sSQL & "INNER JOIN risk r "
        sSQL = sSQL & "ON pcv.risk_cnt = r.risk_cnt" & vbCrLf
        sSQL = sSQL & "INNER JOIN disclosure_type_risk_type dtrt "
        sSQL = sSQL & "ON r.risk_type_id = dtrt.risk_type_id" & vbCrLf
        sSQL = sSQL & "     WHERE pc.insurance_file_cnt = @InsuranceFileCnt" & vbCrLf
        sSQL = sSQL & "       AND pcv.risk_cnt = @RiskId" & vbCrLf
        sSQL = sSQL & "       AND pcv.disclosure_type_id = dtrt.disclosure_type_id" & vbCrLf
        sSQL = sSQL & "       AND pcv.party_cnt = @instance2" & vbCrLf

        m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & sProcedureName & "_get_keys StoredProcedure", bStoredProcedure:=False)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateKeySPDisclosure = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        'Set permissions

        sSQL = "GRANT EXECUTE ON dbo." & sProcedureName & "_get_keys TO PUBLIC" & vbCrLf

        m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Grant " & sProcedureName & "_get_keys StoredProcedure", bStoredProcedure:=False)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateKeySPDisclosure = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        ' Part 2 - parent key

        'Drop it if it's already there
        DropExistingProcedure(v_sProcedureName:=sProcedureName, v_sPostFix:="_get_parent_key", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

        DropExistingProcedure(v_sProcedureName:=sProcedureNameOldStyle, v_sPostFix:="_get_parent_key", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateKeySPDisclosure = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        'Create the new procedure
        sSQL = ""
        sSQL = sSQL & "CREATE PROCEDURE " & sProcedureName & "_get_parent_key" & vbCrLf & "@PartyCnt INT," & vbCrLf & "@InsuranceFileCnt INT," & vbCrLf & "@RiskId INT = NULL," & vbCrLf & "@ClaimCnt INT," & vbCrLf & "@DocumentRef VARCHAR(25)," & vbCrLf & "@Instance1 INT," & vbCrLf & "@Instance2 INT," & vbCrLf & "@Instance3 INT"

        sSQL = sSQL & vbCrLf & "AS" & vbCrLf & vbCrLf

        ' put the bit that does the work in
        sSQL = sSQL & "SELECT @Instance2" & vbCrLf

        m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & sProcedureName & "_get_parent_key StoredProcedure", bStoredProcedure:=False)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateKeySPDisclosure = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        'Set permissions
        sSQL = "GRANT EXECUTE ON dbo." & sProcedureName & "_get_parent_key TO PUBLIC" & vbCrLf

        m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Grant " & sProcedureName & "_get_parent_key StoredProcedure", bStoredProcedure:=False)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateKeySPDisclosure = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        Exit Function

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
    Public Function GenerateStoredProcedure(ByRef r_vGISObject As Object, ByRef r_vGISProperty As Object, ByVal v_sDatamodel As String, ByRef r_oDatabase As dPMDAO.Database, ByVal v_lPMProductFamily As Integer, Optional ByVal v_lGisDataModelType As Integer = 0) As Integer

        Dim sProcedureName As String 
        Dim sProcedureNameOldStyle As String 
        Dim sParameterList As String 
        Dim sTableList As String 
        Dim sWhereList1 As String 
        Dim sWhereList2 As String 
        Dim sWhereList3 As String 
        Dim sSelectList As String 
        Dim sFinalSelectList As String 
        Dim lTemp As Integer 
        Dim lTemp2 As Integer 
        Dim lTemp3 As Integer 
        Dim sTemp As String 
        Dim sSQL As String 
        Dim sFieldName As String 
        Dim sColumnName As String 
        Dim sSubGroup As String 
        Dim sSubGroup2 As String 
        Dim sSubGroup3 As String 
        Dim sSubGroupTemp As String 
        Dim sLoop1 As String 
        Dim sLoop2 As String 
        Dim sLoop3 As String 
        Dim sDisplayname As String 
        Dim lFormat As Integer 
        Dim vArray As Object 
        Dim sPrefix As String 
        Dim lCount As Integer 
        Dim sSelect As String 
        Dim lOtherTableCount As Integer 
        Dim lParentId As Integer 
        Dim lLevel As Integer 
        Dim lAddressCount As Integer 
        Dim iParentIsMultipleInstance As Short 
        Dim iInstanceCount As Short 

        ' PW250703 - PS229
        Dim iObjectType As Short 
        Dim bFixedField As Boolean 

        ' PW240703 - PS229 - Get data model type and set up field manager group
        Dim lGISDataModelType As Integer 
        Dim vResArray(,) As Object 
        Dim sMainGroup As String 

        r_oDatabase.Parameters.Clear()
        m_lReturn = r_oDatabase.SQLSelect(sSQL:="SELECT gis_data_model_type_id FROM " & "gis_data_model WHERE code='" & v_sDatamodel & "'", sSQLName:="GenerateStoredProcedure raw SQL", bStoredProcedure:=False, lnumberrecords:=gPMConstants.PMAllRecords, vResultArray:=vResArray)
        If IsArray(vResArray) Then
            'UPGRADE_WARNING: Couldn't resolve default property of object vResArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            lGISDataModelType = Val(vResArray(0, 0))
        End If
        If lGISDataModelType = GISDMTypeParty Then
            sMainGroup = "Party"
        Else
            sMainGroup = "Risk"
        End If
        ' PW240703: end

        '**************
        ' MEvans : 28-07-2003 : 223 document production
        Dim lObject As Integer 
        Dim lUnderscorePos As Integer 
        Dim sLiveTableName As String 
        Dim lParamCount As Integer 
        Const ACClaimBasedWorkTablePrefix As String = "Work_"
        '**************

        Try

        GenerateStoredProcedure = gPMConstants.PMEReturnCode.PMTrue

        sPrefix = v_sDatamodel

        'Delete the records on wp_fields
        'Make sure any old style are deleted first
        sSQL = "DELETE FROM wp_fields" & vbCrLf & "WHERE sql LIKE 'sp_wp_" & v_sDatamodel & "%'" & vbCrLf

        m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Delete " & sProcedureName & " WPFields", bStoredProcedure:=False)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateStoredProcedure = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        'Make sure any new style are deleted first
        sSQL = "DELETE FROM wp_fields" & vbCrLf & "WHERE sql LIKE 'spg_wp_" & v_sDatamodel & "%'" & vbCrLf

        m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Delete " & sProcedureName & " WPFields", bStoredProcedure:=False)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateStoredProcedure = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        For lTemp = LBound(r_vGISObject, 2) To UBound(r_vGISObject, 2)

            iInstanceCount = 0
            'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
            If IsDbNull(r_vGISObject(ACOParentObjectId, lTemp)) Then
                'UPGRADE_WARNING: Couldn't resolve default property of object r_vGISObject(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                lParentId = r_vGISObject(ACOGISObjectId, lTemp)
            End If

            sPrefix = v_sDatamodel
            'For now don't do child objects...
            'Now we do...
            ' PW250703 - PS229 - store object type
            'UPGRADE_WARNING: Couldn't resolve default property of object r_vGISObject(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iObjectType = r_vGISObject(ACOIsNonGIS, lTemp)
            Select Case iObjectType

                Case GISOTClaim 'tbd

                    ' get object position within array
                    lObject = lTemp

                    ' ensure this is a child object
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_vGISObject(ACOGISObjectId, lObject). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If (r_vGISObject(ACOGISObjectId, lObject) <> lParentId) Then

                        '*****************************
                        ' determine the old and new format of the stored procedures
                        '*****************************

                        ' use live table
                        'UPGRADE_WARNING: Couldn't resolve default property of object r_vGISObject(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        sProcedureName = LCase(Replace(r_vGISObject(ACOObjectName, lObject), ACClaimBasedWorkTablePrefix, "", 1))

                        m_lReturn = GetProcedureName(r_sProcedureName:=sProcedureName, v_sDatamodel:=v_sDatamodel)

                        sProcedureNameOldStyle = "sp_wp_" & v_sDatamodel & sProcedureName
                        sProcedureName = "spg_wp_" & v_sDatamodel & sProcedureName

                        'UPGRADE_WARNING: Couldn't resolve default property of object r_vGISObject(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        iParentIsMultipleInstance = IsTopLevelParentObjectMultipleInstance(r_vGISObject(ACOObjectName, lObject), r_vGISObject)

                        '******************
                        ' determine prefix (alias) for table name and columns
                        '******************
                        'sTemp = StripDataModelCode(v_vTheString:=r_vGISObject(ACOTableName, lObject), _
                        'v_sDataModel:=v_sDataModel)
                        'sTemp = Mid$(sTemp, InStr(sTemp, "_") + 1)

                        'sTemp = "Claim"
                        sPrefix = v_sDatamodel & "C"

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
                        'UPGRADE_WARNING: Couldn't resolve default property of object r_vGISObject(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        sLiveTableName = Replace(r_vGISObject(ACOTableName, lObject), ACClaimBasedWorkTablePrefix, "", 1)

                        ' intialise variables
                        lParamCount = 0
                        sSelectList = ""
                        sTableList = "FROM " & sLiveTableName & " " & sPrefix & vbCrLf

                        ' Get and use property array for the current object
                        'UPGRADE_WARNING: Couldn't resolve default property of object r_vGISProperty(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object vArray. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        vArray = r_vGISProperty(lObject)

                        '*****************
                        ' are there any object properties
                        '*****************
                        If ProcessPropertyArray(v_lObjectType:=GISOTClaim, r_vPropertyArray:=vArray, r_sSelectList:=sSelectList, r_sTableList:=sTableList, v_sMainGroup:="Claim", v_sProcedureName:=sProcedureName, v_lPMProductFamily:=v_lPMProductFamily, v_sDatamodel:=v_sDatamodel, r_oDatabase:=r_oDatabase, r_lParamCount:=lParamCount, v_sPrefix:=sPrefix, v_sSubGroup:=v_sDatamodel & " Claim Details") = gPMConstants.PMEReturnCode.PMTrue Then

                            If (lParamCount <> 0) Then

                                sWhereList1 = "WHERE " & sPrefix & ".Claim_Id = @ClaimCnt"

                                'Add the rest...
                                sTableList = Left(sTableList, Len(sTableList) - 2) '& ", " & vbCrLf & |                            v_sDataModel & "_Policy_binder pb," & vbCrLf & |                                    "GIS_policy_link gpl," & vbCrLf & |                                    "insurance_file ifi" & vbCrLf

                                '***************
                                ' Attempt to drop both version of the stored procedure
                                ' both old and new
                                '***************
                                DropExistingProcedure(v_sProcedureName:=sProcedureName, v_sPostFix:="", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

                                DropExistingProcedure(v_sProcedureName:=sProcedureNameOldStyle, v_sPostFix:="", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

                                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                                    GenerateStoredProcedure = gPMConstants.PMEReturnCode.PMFalse
                                    Exit Function
                                End If

                                'remove the last comma and vbcrlf
                                sSelectList = Left(sSelectList, Len(sSelectList) - 3) & vbCrLf
                                sSelectList = " SELECT " & sSelectList & vbCrLf & sTableList & vbCrLf & sWhereList1

                                'Create the new procedure
                                sSQL = ""

                                ' removed risk id as this is knackering the
                                '"@RiskId INT = NULL," & vbCrLf &
                                sSQL = sSQL & "CREATE PROCEDURE " & sProcedureName & vbCrLf & "@PartyCnt INT," & vbCrLf & "@InsuranceFileCnt INT," & vbCrLf & "@ClaimCnt INT," & vbCrLf & "@DocumentRef VARCHAR(25)"

                                'dynamically generate instance variables
                                generateInstanceVariables(sSQL, iInstanceCount)
                                sSQL = sSQL & vbCrLf & "AS" & vbCrLf & vbCrLf & sSelectList & vbCrLf

                                m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & sProcedureName & " StoredProcedure", bStoredProcedure:=False)

                                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                                    GenerateStoredProcedure = gPMConstants.PMEReturnCode.PMFalse
                                    Exit Function
                                End If

                                'Set permissions
                                sSQL = "GRANT EXECUTE ON dbo." & sProcedureName & " TO PUBLIC" & vbCrLf

                                m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Grant " & sProcedureName & " StoredProcedure", bStoredProcedure:=False)

                                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                                    GenerateStoredProcedure = gPMConstants.PMEReturnCode.PMFalse
                                    Exit Function
                                End If

                            End If ' Are there any parameters

                        End If

                    End If

                Case GISOTPeril 'tbd

                    '**************************
                    '*******WE ARE IN *********
                    '******** CLAIM ***********
                    '******** PERIL ***********
                    '**************************

                    ' get object position within array
                    lObject = lTemp

                    'UPGRADE_WARNING: Couldn't resolve default property of object r_vGISObject(ACOGISObjectId, lObject). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If (r_vGISObject(ACOGISObjectId, lObject) <> lParentId) Then
                        '*****************************
                        ' determine the old and new format of the stored procedures
                        '*****************************

                        ' use live tables not work tables for document production
                        'UPGRADE_WARNING: Couldn't resolve default property of object r_vGISObject(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        sProcedureName = LCase(Replace(r_vGISObject(ACOObjectName, lObject), ACClaimBasedWorkTablePrefix, "", 1))

                        m_lReturn = GetProcedureName(r_sProcedureName:=sProcedureName, v_sDatamodel:=v_sDatamodel)

                        sProcedureNameOldStyle = "sp_wp_" & v_sDatamodel & sProcedureName
                        sProcedureName = "spg_wp_" & v_sDatamodel & sProcedureName

                        'UPGRADE_WARNING: Couldn't resolve default property of object r_vGISObject(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        iParentIsMultipleInstance = IsTopLevelParentObjectMultipleInstance(r_vGISObject(ACOObjectName, lObject), r_vGISObject)

                        '******************
                        ' determine prefix (alias) for table name
                        '******************
                        'sTemp = StripDataModelCode(v_vTheString:=r_vGISObject(ACOTableName, lObject), _
                        'v_sDataModel:=v_sDataModel)
                        'sTemp = Mid$(sTemp, InStr(sTemp, "_") + 1)

                        sPrefix = v_sDatamodel & "CP"

                        'While InStr(sTemp, "_") <> 0
                        '    lUnderscorePos = InStr(sTemp, "_")
                        '    sPrefix = sPrefix & UCase$(Left$(sTemp, 1))
                        '    sTemp = Mid$(sTemp, lUnderscorePos + 1)
                        'Wend

                        'sPrefix = sPrefix & UCase$(Left$(sTemp, 1))

                        '*****************
                        ' Build SQL for Create Procedure
                        '*****************
                        'UPGRADE_WARNING: Couldn't resolve default property of object r_vGISObject(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        sLiveTableName = Replace(r_vGISObject(ACOTableName, lObject), ACClaimBasedWorkTablePrefix, "", 1)

                        ' intialise variables
                        lParamCount = 0
                        sSelectList = ""
                        sTableList = "FROM " & sLiveTableName & " " & sPrefix & vbCrLf

                        ' Get and use property array for the current object
                        'UPGRADE_WARNING: Couldn't resolve default property of object r_vGISProperty(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object vArray. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        vArray = r_vGISProperty(lObject)

                        '************************
                        ' Generate Get Keys Stored Procedure
                        '************************
                        ' Use the Live claim peril table to get the keys from
                        m_lReturn = GenerateClaimPerilGetKeysStoredProcedure(sTableName:=sLiveTableName, sProcedureName:=sProcedureName, sProcedureNameOldStyle:=sProcedureNameOldStyle, r_oDatabase:=r_oDatabase)

                        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                            GenerateStoredProcedure = gPMConstants.PMEReturnCode.PMFalse
                            Exit Function
                        End If

                        '************************
                        ' Generate Get Parent Keys Stored Procedure
                        '************************
                        m_lReturn = GenerateClaimPerilGetParentStoredProcedure(sProcedureName:=sProcedureName, sProcedureNameOldStyle:=sProcedureNameOldStyle, r_oDatabase:=r_oDatabase)

                        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                            GenerateStoredProcedure = gPMConstants.PMEReturnCode.PMFalse
                            Exit Function
                        End If

                        '*****************
                        ' Build SQL for Create Procedure
                        '*****************

                        ' intialise variables
                        lParamCount = 0
                        sSelectList = ""
                        sTableList = "FROM " & sLiveTableName & " " & sPrefix & vbCrLf

                        ' Get and use property array for the current object
                        'UPGRADE_WARNING: Couldn't resolve default property of object r_vGISProperty(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object vArray. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        vArray = r_vGISProperty(lObject)

                        If ProcessPropertyArray(v_lObjectType:=GISOTPeril, r_vPropertyArray:=vArray, r_sSelectList:=sSelectList, r_sTableList:=sTableList, v_sMainGroup:="Claim", v_sProcedureName:=sProcedureName, v_lPMProductFamily:=v_lPMProductFamily, v_sDatamodel:=v_sDatamodel, r_oDatabase:=r_oDatabase, r_lParamCount:=lParamCount, v_sPrefix:=sPrefix, v_sSubGroup:=v_sDatamodel & " Peril Items", v_sLoop1:=v_sDatamodel & "ClaimPeril") = gPMConstants.PMEReturnCode.PMTrue Then

                            If (lParamCount <> 0) Then

                                'Add the rest...
                                sTableList = Left(sTableList, Len(sTableList) - 2) '& ", " & vbCrLf & |                            v_sDataModel & "_Policy_binder pb," & vbCrLf & |                                    "GIS_policy_link gpl," & vbCrLf & |                                    "insurance_file ifi" & vbCrLf

                                sWhereList1 = "WHERE Claim_Peril_id = @Instance2"

                                'Drop it if it's already there
                                DropExistingProcedure(v_sProcedureName:=sProcedureName, v_sPostFix:="", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

                                DropExistingProcedure(v_sProcedureName:=sProcedureNameOldStyle, v_sPostFix:="", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

                                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                                    GenerateStoredProcedure = gPMConstants.PMEReturnCode.PMFalse
                                    Exit Function
                                End If

                                'remove the last comma and vbcrlf
                                sSelectList = Left(sSelectList, Len(sSelectList) - 3) & vbCrLf

                                sSelectList = "SELECT " & sSelectList & " " & sTableList & " " & sWhereList1

                                'Create the new procedure
                                sSQL = ""

                                sSQL = sSQL & "CREATE PROCEDURE " & sProcedureName & " @PartyCnt INT," & vbCrLf & "@InsuranceFileCnt INT," & vbCrLf & "@ClaimCnt INT," & vbCrLf & "@DocumentRef VARCHAR(25)," & vbCrLf & "@Instance1  INT = NULL," & vbCrLf & "@Instance2  INT = NULL," & vbCrLf & "@Instance3  INT = NULL" & vbCrLf & "AS" & vbCrLf & ""

                                sSQL = sSQL & vbCrLf & sSelectList & vbCrLf

                                m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & sProcedureName & " StoredProcedure", bStoredProcedure:=False)

                                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                                    GenerateStoredProcedure = gPMConstants.PMEReturnCode.PMFalse
                                    Exit Function
                                End If

                                'Set permissions
                                sSQL = "GRANT EXECUTE ON dbo." & sProcedureName & " TO PUBLIC" & vbCrLf

                                m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Grant " & sProcedureName & " StoredProcedure", bStoredProcedure:=False)

                                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                                    GenerateStoredProcedure = gPMConstants.PMEReturnCode.PMFalse
                                    Exit Function
                                End If

                            End If
                        End If
                    End If
                    '**************************
                    '*******WE ARE IN *********
                    '******** CLAIM ***********
                    '******** PERIL ***********
                    '**************************


                Case GISOTNonGisSpecials
                    'We're something special
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_vGISProperty(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Couldn't resolve default property of object vArray. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    vArray = r_vGISProperty(lTemp)

                    For lTemp2 = LBound(vArray, 2) To UBound(vArray, 2)
                        'It's a sum insured
                        'UPGRADE_WARNING: Couldn't resolve default property of object vArray(ACPSpecialsType, lTemp2). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        If vArray(ACPSpecialsType, lTemp2) = ACOSumInsuredTypeID Then
                            'UPGRADE_WARNING: Couldn't resolve default property of object vArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            m_lReturn = GenerateSumInsuredFields(v_sDatamodel:=v_sDatamodel, lSumInsuredTypeId:=CInt(vArray(ACPSpecialsTypeReference, lTemp2)), r_oDatabase:=r_oDatabase, v_lPMProductFamily:=v_lPMProductFamily)

                            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                                GenerateStoredProcedure = gPMConstants.PMEReturnCode.PMFalse
                                Exit Function
                            End If
                        End If
                        'UPGRADE_WARNING: Couldn't resolve default property of object vArray(ACPSpecialsType, lTemp2). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        If vArray(ACPSpecialsType, lTemp2) = ACOStdWordingType Then
                            'It's a standard wording (endorsement / clause)
                            m_lReturn = GenerateStandardWordingFields()

                            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                                GenerateStoredProcedure = gPMConstants.PMEReturnCode.PMFalse
                                Exit Function
                            End If
                        End If
                    Next lTemp2

                    ' PW250703 - PS229 - do associated clients/disclosures
                Case GISOTRisk, GISOTAssociatedClient, GISOTDisclosure

                    '*************
                    ' MEvans : 10-11-2003 : CQ3049 / CQ3143
                    '**********************************************************************
                    '**** CLAIM RISK OBJECT ***********************************************
                    '**********************************************************************
                    If v_lGisDataModelType = GISDMTypeClaim Then

                        '**********************************************************************
                        '*** This is a claim risk object so do things a little differently ****
                        '**********************************************************************

                        ' dont create wp fields objects for the ad-hoc document request tables
                        'UPGRADE_WARNING: Couldn't resolve default property of object r_vGISObject(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        If InStr(1, r_vGISObject(ACOTableName, lTemp), "claim_output", CompareMethod.Text) = 0 And InStr(1, r_vGISObject(ACOTableName, lTemp), "document_request", CompareMethod.Text) = 0 Then

                            ' get object position within array
                            lObject = lTemp

                            If GenerateClaimRiskSP(v_lObjectId:=lObject, v_vGISObject:=r_vGISObject, v_vGISProperty:=r_vGISProperty, v_lParentId:=lParentId, v_sDatamodel:=v_sDatamodel, v_lPMProductFamily:=v_lPMProductFamily, r_oDatabase:=r_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then

                                GenerateStoredProcedure = gPMConstants.PMEReturnCode.PMFalse
                                Exit Function

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
                        'UPGRADE_WARNING: Couldn't resolve default property of object r_vGISObject(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        If InStr(1, r_vGISObject(ACOTableName, lTemp), "claim_output", CompareMethod.Text) = 0 And InStr(1, r_vGISObject(ACOTableName, lTemp), "document_request", CompareMethod.Text) = 0 Then
                            '******************

                            'UPGRADE_WARNING: Couldn't resolve default property of object r_vGISObject(ACOGISObjectId, lTemp). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            If (r_vGISObject(ACOGISObjectId, lTemp) <> lParentId) Then
                                'UPGRADE_WARNING: Couldn't resolve default property of object r_vGISObject(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                sProcedureName = LCase(r_vGISObject(ACOObjectName, lTemp))

                                m_lReturn = GetProcedureName(r_sProcedureName:=sProcedureName, v_sDatamodel:=v_sDatamodel)

                                sProcedureNameOldStyle = "sp_wp_" & v_sDatamodel & sProcedureName
                                sProcedureName = "spg_wp_" & v_sDatamodel & sProcedureName

                                'UPGRADE_WARNING: Couldn't resolve default property of object StripDataModelCode(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                sTemp = StripDataModelCode(v_vTheString:=r_vGISObject(ACOTableName, lTemp), v_sDatamodel:=v_sDatamodel)
                                sTemp = Mid(sTemp, InStr(sTemp, "_") + 1)

                                'UPGRADE_WARNING: Couldn't resolve default property of object r_vGISObject(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                iParentIsMultipleInstance = IsTopLevelParentObjectMultipleInstance(r_vGISObject(ACOObjectName, lTemp), r_vGISObject)

                                sPrefix = v_sDatamodel
                                While InStr(sTemp, "_") <> 0
                                    lTemp3 = InStr(sTemp, "_")
                                    sPrefix = sPrefix & UCase(Left(sTemp, 1))
                                    sTemp = Mid(sTemp, lTemp3 + 1)
                                End While

                                sPrefix = sPrefix & UCase(Left(sTemp, 1))

                                'First we do the main query
                                sParameterList = ""
                                sSelectList = ""
                                sFinalSelectList = "SELECT "

                                'UPGRADE_WARNING: Couldn't resolve default property of object r_vGISProperty(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                'UPGRADE_WARNING: Couldn't resolve default property of object vArray. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                vArray = r_vGISProperty(lTemp)

                                lCount = 0
                                lOtherTableCount = 0

                                ' PW250703 - PS229 - select/table list depends on object type
                                Select Case iObjectType
                                    Case GISOTRisk
                                        'UPGRADE_WARNING: Couldn't resolve default property of object r_vGISObject(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                        sTableList = "FROM " & r_vGISObject(ACOTableName, lTemp) & " " & sPrefix & vbCrLf
                                    Case GISOTAssociatedClient
                                        sSelectList = "SELECT pc.is_insured," & vbCrLf & "       ppc.party_title_code," & vbCrLf & "       ppc.forename," & vbCrLf & "       p.name," & vbCrLf & "       p.resolved_name," & vbCrLf & "       ppc.initials," & vbCrLf & "       pl.date_of_birth," & vbCrLf & "       pl.gender_code," & vbCrLf & "       party_type_code = pt.code," & vbCrLf & "       party_type_description = pt.description," & vbCrLf

                                        ' PW200803 - CQ1912 - change from file to folder
                                        sTableList = "FROM policy_client pc" & vbCrLf & "LEFT JOIN " & v_sDatamodel & "_associated_client " & sPrefix & " " & "ON pc.party_cnt = " & sPrefix & ".party_cnt " & "AND pc.insurance_file_cnt = " & sPrefix & ".insurance_file_cnt" & vbCrLf
                                    Case GISOTDisclosure
                                        ' PW200803 - CQ1912 - add new properties
                                        sSelectList = "SELECT pcv.code," & vbCrLf & "       pcv.conviction_date," & vbCrLf & "       pcv.description," & vbCrLf & "       pcv.fine_amt," & vbCrLf & "       pcv.sentence_code," & vbCrLf & "       pcv.sentence_description," & vbCrLf & "       pcv.sentence_duration," & vbCrLf & "       pcv.sentence_duration_qualifier," & vbCrLf & "       pcv.sentence_effective_date," & vbCrLf & "       pcv.status_code," & vbCrLf & "       pcv.alcohol_level," & vbCrLf & "       pcv.alcohol_measurement_method," & vbCrLf & "       pcv.driving_licence_penalty_pts," & vbCrLf & "       disclosure_type_id = dt.description," & vbCrLf & "       pcv.effective_date," & vbCrLf & "       pcv.expiry_date," & vbCrLf & "       pcv.replaced_by_id," & vbCrLf

                                        sTableList = "FROM party_conviction pcv" & vbCrLf & "LEFT JOIN " & v_sDatamodel & "_disclosure " & sPrefix & " " & "ON pcv.party_cnt = " & sPrefix & ".party_cnt " & "AND pcv.party_conviction_id = " & sPrefix & ".party_conviction_id" & vbCrLf
                                End Select

                                ' PW240703 - PS229 - different where clauses required for
                                ' data model type of Party, because the risk object isn't
                                ' really a risk object. But that's how it's been done.
                                If lGISDataModelType = GISDMTypeParty Then

                                    sWhereList1 = "WHERE p.party_cnt = @PartyCnt" & vbCrLf & "AND gpl.party_cnt = p.party_cnt" & vbCrLf & "AND gpl.Risk_id is null" & vbCrLf & "AND pb.gis_policy_link_id = gpl.gis_policy_link_id" & vbCrLf & "AND " & sPrefix & "." & v_sDatamodel & "_policy_binder_id = pb." & v_sDatamodel & "_Policy_binder_id" & vbCrLf

                                    sWhereList2 = sWhereList1
                                Else

                                    ' PW250703 - PS229 - where list depends on object type
                                    Select Case iObjectType
                                        Case GISOTRisk

                                            sWhereList1 = "WHERE ifi.insurance_file_cnt = @InsuranceFileCnt" & vbCrLf & "AND gpl.insurance_file_cnt = ifi.insurance_file_cnt" & vbCrLf & "AND gpl.Risk_id is null" & vbCrLf & "AND pb.gis_policy_link_id = gpl.gis_policy_link_id" & vbCrLf & "AND " & sPrefix & "." & v_sDatamodel & "_policy_binder_id = pb." & v_sDatamodel & "_Policy_binder_id" & vbCrLf

                                            sWhereList2 = "WHERE ifi.insurance_file_cnt = @InsuranceFileCnt" & vbCrLf & "AND gpl.Risk_id = @RiskId" & vbCrLf & "AND pb.gis_policy_link_id = gpl.gis_policy_link_id" & vbCrLf & "AND " & sPrefix & "." & v_sDatamodel & "_policy_binder_id = pb." & v_sDatamodel & "_Policy_binder_id" & vbCrLf

                                        Case GISOTAssociatedClient
                                            ' PW200803 - CQ1912 - change from file to folder
                                            sWhereList1 = "WHERE pc.insurance_file_cnt = @InsuranceFileCnt" & vbCrLf & "AND pc.risk_cnt is NULL" & vbCrLf & "AND pc.party_cnt = @instance2" & vbCrLf

                                            ' PW200803 - CQ1912 - change from file to folder
                                            sWhereList2 = "WHERE pc.insurance_file_cnt = @InsuranceFileCnt" & vbCrLf & "AND (pc.risk_cnt = @RiskId or pc.risk_cnt is NULL)" & vbCrLf & "AND pc.party_cnt = @instance2" & vbCrLf

                                        Case GISOTDisclosure
                                            sWhereList1 = "WHERE pcv.party_cnt = @instance2" & vbCrLf & "AND pcv.party_conviction_id = @instance3" & vbCrLf

                                            sWhereList2 = sWhereList1

                                    End Select
                                End If

                                sWhereList3 = ""

                                'We need the rest of the key information...
                                lLevel = 0
                                If IsArray(vArray) Then
                                    For lTemp2 = LBound(vArray, 2) To UBound(vArray, 2)
                                        'UPGRADE_WARNING: Couldn't resolve default property of object vArray(ACPIsIdentifyingProperty, lTemp2). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                        If (vArray(ACPIsIdentifyingProperty, lTemp2) = 1) Then
                                            lLevel = lTemp2
                                        End If
                                    Next lTemp2
                                End If

                                ' PW250703 - PS229 - generate extra stored procs for
                                ' associated clients / disclosures
                                Select Case iObjectType
                                    Case GISOTAssociatedClient

                                        'UPGRADE_WARNING: Couldn't resolve default property of object r_vGISObject(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                        m_lReturn = GenerateKeySPAssociatedClient(sTableName:=CStr(r_vGISObject(ACOTableName, lTemp)), sPrefix:=sPrefix, v_sDatamodel:=v_sDatamodel, sProcedureName:=sProcedureName, sProcedureNameOldStyle:=sProcedureNameOldStyle, vArray:=vArray, v_iIsParentMultipleInstance:=iParentIsMultipleInstance, r_oDatabase:=r_oDatabase)

                                        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                                            GenerateStoredProcedure = gPMConstants.PMEReturnCode.PMFalse
                                            Exit Function
                                        End If

                                    Case GISOTDisclosure

                                        'UPGRADE_WARNING: Couldn't resolve default property of object r_vGISObject(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                        m_lReturn = GenerateKeySPDisclosure(sTableName:=CStr(r_vGISObject(ACOTableName, lTemp)), sPrefix:=sPrefix, v_sDatamodel:=v_sDatamodel, sProcedureName:=sProcedureName, sProcedureNameOldStyle:=sProcedureNameOldStyle, vArray:=vArray, v_iIsParentMultipleInstance:=iParentIsMultipleInstance, r_oDatabase:=r_oDatabase)

                                        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                                            GenerateStoredProcedure = gPMConstants.PMEReturnCode.PMFalse
                                            Exit Function
                                        End If

                                    Case GISOTRisk

                                        'We need to create the Getkeys stored procedure if we are deeper that 1 level
                                        'OR we have more than one instance of an object
                                        'UPGRADE_WARNING: Couldn't resolve default property of object r_vGISObject(ACOMaxInstances, lTemp). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                        If (lLevel > 1 Or r_vGISObject(ACOMaxInstances, lTemp) > 1) Then

                                            'UPGRADE_WARNING: Couldn't resolve default property of object r_vGISObject(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                            m_lReturn = GenerateGetKeysStoredProcedure(sTableName:=CStr(r_vGISObject(ACOTableName, lTemp)), sPrefix:=sPrefix, v_sDatamodel:=v_sDatamodel, sProcedureName:=sProcedureName, sProcedureNameOldStyle:=sProcedureNameOldStyle, vArray:=vArray, v_iIsParentMultipleInstance:=iParentIsMultipleInstance, r_oDatabase:=r_oDatabase)

                                            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                                                GenerateStoredProcedure = gPMConstants.PMEReturnCode.PMFalse
                                                Exit Function
                                            End If
                                        End If

                                        '01082002 CMG/PB Corrected error in this If logic level 2 parent
                                        'stored procedures were not being created and this was breaking
                                        'risk looping
                                        'UPGRADE_WARNING: Couldn't resolve default property of object r_vGISObject(ACOMaxInstances, lTemp). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                        If lLevel = 2 Or (r_vGISObject(ACOMaxInstances, lTemp) > 1) Then
                                            'UPGRADE_WARNING: Couldn't resolve default property of object r_vGISObject(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                            m_lReturn = GenerateGetParentStoredProcedure(sTableName:=CStr(r_vGISObject(ACOTableName, lTemp)), sPrefix:=sPrefix, v_sDatamodel:=v_sDatamodel, sProcedureName:=sProcedureName, sProcedureNameOldStyle:=sProcedureNameOldStyle, vArray:=vArray, r_oDatabase:=r_oDatabase)

                                            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                                                GenerateStoredProcedure = gPMConstants.PMEReturnCode.PMFalse
                                                Exit Function
                                            End If
                                        End If

                                End Select

                                'We want to ignore all if we're top level (1)
                                'but include all (but the first) if we're lower level
                                'so...

                                If (lLevel > 1) Then
                                    lLevel = 0
                                End If

                                sSubGroup = ""
                                sSubGroup2 = ""
                                sSubGroup3 = ""
                                sLoop1 = ""
                                sLoop2 = ""
                                sLoop3 = ""
                                lAddressCount = 0
                                If IsArray(vArray) Then

                                    ' PW250703 - PS229 - set up groups/loops for associated client / disclosure
                                    ' object types
                                    Select Case iObjectType
                                        Case GISOTAssociatedClient
                                            sSubGroup = v_sDatamodel & "AssociatedClient"
                                            sLoop1 = sSubGroup
                                        Case GISOTDisclosure
                                            sSubGroup = v_sDatamodel & "AssociatedClient"
                                            sLoop1 = sSubGroup
                                            sSubGroup2 = v_sDatamodel & "Disclosure"
                                            sLoop2 = sSubGroup2
                                    End Select

                                    For lTemp2 = LBound(vArray, 2) To UBound(vArray, 2)

                                        ' PW280703 - PS229 - set flag to indicate if this
                                        ' field is from a fixed table
                                        If (vArray(ACPEditFlags, lTemp2) And GISDSEditNoDBColumn) = 0 Then
                                            bFixedField = False
                                        Else
                                            bFixedField = True
                                        End If


                                        'UPGRADE_WARNING: Couldn't resolve default property of object vArray(ACPIsIdentifyingProperty, lTemp2). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                        If (vArray(ACPIsIdentifyingProperty, lTemp2) = 1) Then
                                            'Move the determination of the group structure to here - only
                                            'do it once
                                            sSubGroupTemp = v_sDatamodel
                                            'UPGRADE_WARNING: Couldn't resolve default property of object StripDataModelCode(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                            sTemp = Trim(StripDataModelCode(v_vTheString:=vArray(ACPColumnName, lTemp2), v_sDatamodel:=v_sDatamodel))

                                            'Remove the trailing _id
                                            sTemp = Left(sTemp, Len(sTemp) - 3)

                                            While InStr(sTemp, "_") <> 0
                                                lTemp3 = InStr(sTemp, "_")
                                                sSubGroupTemp = sSubGroupTemp & StrConv(Left(sTemp, lTemp3 - 1), VbStrConv.ProperCase)
                                                sTemp = Mid(sTemp, lTemp3 + 1)
                                            End While

                                            Select Case lTemp2
                                                Case 0
                                                Case 1
                                                    sSubGroup = sSubGroupTemp & StrConv(sTemp, VbStrConv.ProperCase)
                                                    'find if parent was actually a  looping object
                                                    If lLevel = 0 Then
                                                        If iParentIsMultipleInstance = 1 And lLevel = 0 Then
                                                            sLoop1 = sSubGroup
                                                        End If
                                                    End If
                                                Case 2
                                                    sSubGroup2 = sSubGroupTemp & StrConv(sTemp, VbStrConv.ProperCase)
                                                    If (lLevel = 0) Then
                                                        If (iParentIsMultipleInstance = 1) Then
                                                            sLoop2 = sSubGroup2
                                                        Else
                                                            sLoop1 = sSubGroup2
                                                        End If
                                                    End If

                                                Case 3
                                                    sSubGroup3 = sSubGroupTemp & StrConv(sTemp, VbStrConv.ProperCase)
                                                    'ED 30092002 - sLoop3 set t sSubGroup3 if parent is multi instance
                                                    'orignally overwriting sLoop2 with the new sub value.
                                                    If (lLevel = 0) Then
                                                        If (iParentIsMultipleInstance = 1) Then
                                                            sLoop3 = sSubGroup3
                                                        Else
                                                            sLoop2 = sSubGroup3
                                                        End If
                                                    End If

                                            End Select

                                            If (lTemp2 > lLevel) Then
                                                'UPGRADE_WARNING: Couldn't resolve default property of object vArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                                sWhereList1 = sWhereList1 & "AND " & sPrefix & "." & vArray(ACPColumnName, lTemp2) & " = @Instance" & lTemp2 + iParentIsMultipleInstance & vbCrLf
                                                'UPGRADE_WARNING: Couldn't resolve default property of object vArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                                sWhereList2 = sWhereList2 & "AND " & sPrefix & "." & vArray(ACPColumnName, lTemp2) & " = @Instance" & lTemp2 + iParentIsMultipleInstance & vbCrLf
                                                iInstanceCount = lTemp2 + iParentIsMultipleInstance
                                            End If
                                        Else
                                            'UPGRADE_WARNING: Couldn't resolve default property of object vArray(ACPPropertyName, lTemp2). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                            If vArray(ACPPropertyName, lTemp2) <> "dElEtEd" Then

                                                'if processing first no key attribute
                                                If lCount = 0 Then
                                                    'if not subloop and object is multiple instance
                                                    'UPGRADE_WARNING: Couldn't resolve default property of object r_vGISObject(ACOMaxInstances, lTemp). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                                    If sLoop1 = "" And r_vGISObject(ACOMaxInstances, lTemp) > 1 Then
                                                        'add where clause for top level multiple instance
                                                        'UPGRADE_WARNING: Couldn't resolve default property of object vArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                                        sWhereList1 = sWhereList1 & "AND " & sPrefix & "." & vArray(ACPColumnName, 1) & " = @Instance2" & vbCrLf
                                                        'UPGRADE_WARNING: Couldn't resolve default property of object vArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                                        sWhereList2 = sWhereList2 & "AND " & sPrefix & "." & vArray(ACPColumnName, 1) & " = @Instance2" & vbCrLf
                                                        sLoop1 = sSubGroup
                                                    End If
                                                End If
                                                lCount = lCount + 1

                                                ' PW250703 - PS229
                                                If (lCount = 1) And iObjectType = GISOTRisk Then
                                                    sSelect = "SELECT "
                                                Else
                                                    sSelect = ""
                                                End If

                                                'UPGRADE_WARNING: Couldn't resolve default property of object vArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                                sTemp = Trim(vArray(ACPColumnName, lTemp2))

                                                sFieldName = ""
                                                sDisplayname = ""
                                                sColumnName = sTemp

                                                While InStr(sTemp, "_") <> 0
                                                    lTemp3 = InStr(sTemp, "_")
                                                    sFieldName = sFieldName & Left(sTemp, lTemp3 - 1)
                                                    sDisplayname = sDisplayname & Left(sTemp, lTemp3 - 1) & " "
                                                    sTemp = Mid(sTemp, lTemp3 + 1)
                                                End While

                                                sFieldName = sFieldName & sTemp
                                                sDisplayname = sDisplayname & sTemp

                                                'Special stuff to do here.

                                                ' PW280703 - PS229 - If it's a field from a
                                                ' fixed table, treat it as bog-standard

                                                'If it's an address, need 6 lines of address and links to the address and country tables
                                                'UPGRADE_WARNING: Couldn't resolve default property of object vArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                                If (UCase(Left(vArray(ACPColumnName, lTemp2), 11)) = "ADDRESS_CNT") And Not bFixedField Then
                                                    lAddressCount = lAddressCount + 1

                                                    sParameterList = sParameterList & "@address" & lAddressCount & "_line_1 " & "VARCHAR(255)," & vbCrLf
                                                    sParameterList = sParameterList & "@address" & lAddressCount & "_line_2 " & "VARCHAR(255)," & vbCrLf
                                                    sParameterList = sParameterList & "@address" & lAddressCount & "_line_3 " & "VARCHAR(255)," & vbCrLf
                                                    sParameterList = sParameterList & "@address" & lAddressCount & "_line_4 " & "VARCHAR(255)," & vbCrLf
                                                    sParameterList = sParameterList & "@address" & lAddressCount & "_postal_code " & "VARCHAR(255)," & vbCrLf
                                                    sParameterList = sParameterList & "@address" & lAddressCount & "_country " & "VARCHAR(255)," & vbCrLf

                                                    sSelect = ""
                                                    If InStr(sSelectList, "SELECT") = 0 Then
                                                        sSelectList = "SELECT " & sSelectList
                                                    End If

                                                    sSelectList = sSelectList & "address" & lAddressCount & "_line_1 = a" & lAddressCount & ".address1," & vbCrLf & "address" & lAddressCount & "_line_2 = a" & lAddressCount & ".address2," & vbCrLf & "address" & lAddressCount & "_line_3 = a" & lAddressCount & ".address3," & vbCrLf & "address" & lAddressCount & "_line_4 = a" & lAddressCount & ".address4," & vbCrLf & "case a" & lAddressCount & ".postal_code when convert(varchar(10), a" & lAddressCount & ".address_id) then '' else a" & lAddressCount & ".postal_code " & "end as address" & lAddressCount & "_postal_code," & vbCrLf & "address" & lAddressCount & "_country = c" & lAddressCount & ".description," & vbCrLf

                                                    'UPGRADE_WARNING: Couldn't resolve default property of object vArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                                    sTableList = sTableList & "LEFT JOIN address a" & lAddressCount & " ON " & sPrefix & "." & vArray(ACPColumnName, lTemp2) & " = a" & lAddressCount & ".address_cnt" & vbCrLf
                                                    sTableList = sTableList & "LEFT JOIN country c" & lAddressCount & " ON " & "a" & lAddressCount & ".country_id = c" & lAddressCount & ".country_id" & vbCrLf

                                                    For lTemp3 = 1 To 4
                                                        'Loop4 is not supported as yet.
                                                        'UPGRADE_WARNING: Couldn't resolve default property of object vArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                                        'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
                                                        m_lReturn = AddToWPFields(sFieldName:=sPrefix & "Address" & lAddressCount & "Line" & lTemp3, sSQL:=sProcedureName, sColumnName:="address" & lAddressCount & "_line_" & lTemp3, lColumnType:=gPMConstants.PMEFormatStyle.PMFormatString, sMainGroup:=sMainGroup, sSubGroup:=sSubGroup, sSubGroup2:=sSubGroup2, sSubGroup3:=sSubGroup3, sSubGroup4:="", sDisplayname:=Trim(vArray(ACPPropertyName, lTemp2)) & " Line " & lTemp3, iIsDisplayed:=1, sLoop1:=sLoop1, sLoop2:=sLoop2, sLoop3:=sLoop3, sLoop4:="", lProductFamily:=v_lPMProductFamily, vDataModel:=System.DBNull.Value, vPropertyId:=System.DBNull.Value, r_oDatabase:=r_oDatabase)

                                                        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                                                            GenerateStoredProcedure = gPMConstants.PMEReturnCode.PMFalse
                                                            Exit Function
                                                        End If
                                                    Next lTemp3

                                                    'UPGRADE_WARNING: Couldn't resolve default property of object vArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                                    'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
                                                    m_lReturn = AddToWPFields(sFieldName:=sPrefix & "Address" & lAddressCount & "PostalCode", sSQL:=sProcedureName, sColumnName:="address" & lAddressCount & "_postal_code", lColumnType:=gPMConstants.PMEFormatStyle.PMFormatString, sMainGroup:=sMainGroup, sSubGroup:=sSubGroup, sSubGroup2:=sSubGroup2, sSubGroup3:=sSubGroup3, sSubGroup4:="", sDisplayname:=Trim(vArray(ACPPropertyName, lTemp2)) & " Postal Code", iIsDisplayed:=1, sLoop1:=sLoop1, sLoop2:=sLoop2, sLoop3:=sLoop3, sLoop4:="", lProductFamily:=v_lPMProductFamily, vDataModel:=System.DBNull.Value, vPropertyId:=System.DBNull.Value, r_oDatabase:=r_oDatabase)

                                                    If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                                                        GenerateStoredProcedure = gPMConstants.PMEReturnCode.PMFalse
                                                        Exit Function
                                                    End If

                                                    'UPGRADE_WARNING: Couldn't resolve default property of object vArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                                    'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
                                                    m_lReturn = AddToWPFields(sFieldName:=sPrefix & "Address" & lAddressCount & "Country", sSQL:=sProcedureName, sColumnName:="address" & lAddressCount & "_country", lColumnType:=gPMConstants.PMEFormatStyle.PMFormatString, sMainGroup:=sMainGroup, sSubGroup:=sSubGroup, sSubGroup2:=sSubGroup2, sSubGroup3:=sSubGroup3, sSubGroup4:="", sDisplayname:=Trim(vArray(ACPPropertyName, lTemp2)) & " Country", iIsDisplayed:=1, sLoop1:=sLoop1, sLoop2:=sLoop2, sLoop3:=sLoop3, sLoop4:="", lProductFamily:=v_lPMProductFamily, vDataModel:=System.DBNull.Value, vPropertyId:=System.DBNull.Value, r_oDatabase:=r_oDatabase)

                                                    If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                                                        GenerateStoredProcedure = gPMConstants.PMEReturnCode.PMFalse
                                                        Exit Function
                                                    End If
                                                    'UPGRADE_WARNING: Couldn't resolve default property of object vArray(ACPSpecialsType, lTemp2). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                                ElseIf vArray(ACPSpecialsType, lTemp2) = ACOPartyTypeID And Not bFixedField Then
                                                    'If it's a party, need a link to the party table
                                                    sParameterList = sParameterList & "@" & sColumnName & " VARCHAR(255)," & vbCrLf

                                                    lOtherTableCount = lOtherTableCount + 1

                                                    sSelectList = sSelectList & sSelect & sColumnName & " = p" & lOtherTableCount & ".name," & vbCrLf

                                                    sTableList = sTableList & "LEFT JOIN party p" & lOtherTableCount & " ON " & sPrefix & "." & sColumnName & " = p" & lOtherTableCount & ".party_cnt" & vbCrLf

                                                    'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
                                                    m_lReturn = AddToWPFields(sFieldName:=sPrefix & sFieldName, sSQL:=sProcedureName, sColumnName:=sColumnName, lColumnType:=gPMConstants.PMEFormatStyle.PMFormatString, sMainGroup:=sMainGroup, sSubGroup:=sSubGroup, sSubGroup2:=sSubGroup2, sSubGroup3:=sSubGroup3, sSubGroup4:="", sDisplayname:=sDisplayname, iIsDisplayed:=1, sLoop1:=sLoop1, sLoop2:=sLoop2, sLoop3:=sLoop3, sLoop4:="", lProductFamily:=v_lPMProductFamily, vDataModel:=System.DBNull.Value, vPropertyId:=System.DBNull.Value, r_oDatabase:=r_oDatabase)

                                                    If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                                                        GenerateStoredProcedure = gPMConstants.PMEReturnCode.PMFalse
                                                        Exit Function
                                                    End If
                                                    'UPGRADE_WARNING: Couldn't resolve default property of object vArray(ACPSpecialsType, lTemp2). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                                ElseIf vArray(ACPSpecialsType, lTemp2) = ACOPMLookupTableName And Not bFixedField Then
                                                    'If it's a lookup, need a link to the lookup
                                                    sParameterList = sParameterList & "@" & sColumnName & " VARCHAR(255)," & vbCrLf

                                                    lOtherTableCount = lOtherTableCount + 1

                                                    sSelectList = sSelectList & sSelect & sColumnName & " = l" & lOtherTableCount & ".description," & vbCrLf

                                                    'UPGRADE_WARNING: Couldn't resolve default property of object vArray(ACPSpecialsTypeReference, lTemp2). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                                    'UPGRADE_WARNING: Couldn't resolve default property of object vArray(ACPColumnName, lTemp2). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                                    'UPGRADE_WARNING: Couldn't resolve default property of object vArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                                    sTableList = sTableList & "LEFT JOIN " & vArray(ACPSpecialsTypeReference, lTemp2) & " l" & lOtherTableCount & " ON " & sPrefix & "." & vArray(ACPColumnName, lTemp2) & " = " & "l" & lOtherTableCount & "." & vArray(ACPSpecialsTypeReference, lTemp2) & "_id" & vbCrLf

                                                    'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
                                                    m_lReturn = AddToWPFields(sFieldName:=sPrefix & sFieldName, sSQL:=sProcedureName, sColumnName:=sColumnName, lColumnType:=gPMConstants.PMEFormatStyle.PMFormatString, sMainGroup:=sMainGroup, sSubGroup:=sSubGroup, sSubGroup2:=sSubGroup2, sSubGroup3:=sSubGroup3, sSubGroup4:="", sDisplayname:=sDisplayname, iIsDisplayed:=1, sLoop1:=sLoop1, sLoop2:=sLoop2, sLoop3:=sLoop3, sLoop4:="", lProductFamily:=v_lPMProductFamily, vDataModel:=System.DBNull.Value, vPropertyId:=System.DBNull.Value, r_oDatabase:=r_oDatabase)

                                                    If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                                                        GenerateStoredProcedure = gPMConstants.PMEReturnCode.PMFalse
                                                        Exit Function
                                                    End If
                                                    'UPGRADE_WARNING: Couldn't resolve default property of object vArray(ACPSpecialsType, lTemp2). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                                ElseIf vArray(ACPSpecialsType, lTemp2) = ACOGISUserDefHeaderID And Not bFixedField Then
                                                    'If it's one of our lookups, need a link to our lookup
                                                    sParameterList = sParameterList & "@" & sColumnName & " VARCHAR(255)," & vbCrLf

                                                    lOtherTableCount = lOtherTableCount + 1

                                                    sSelectList = sSelectList & sSelect & sColumnName & " = l" & lOtherTableCount & ".description," & vbCrLf

                                                    'UPGRADE_WARNING: Couldn't resolve default property of object vArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                                    sTableList = sTableList & "LEFT JOIN GIS_user_def_detail l" & lOtherTableCount & " ON " & sPrefix & "." & vArray(ACPColumnName, lTemp2) & " = " & "l" & lOtherTableCount & ".GIS_user_def_detail_id" & vbCrLf

                                                    'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
                                                    m_lReturn = AddToWPFields(sFieldName:=sPrefix & sFieldName, sSQL:=sProcedureName, sColumnName:=sColumnName, lColumnType:=gPMConstants.PMEFormatStyle.PMFormatString, sMainGroup:=sMainGroup, sSubGroup:=sSubGroup, sSubGroup2:=sSubGroup2, sSubGroup3:=sSubGroup3, sSubGroup4:="", sDisplayname:=sDisplayname, iIsDisplayed:=1, sLoop1:=sLoop1, sLoop2:=sLoop2, sLoop3:=sLoop3, sLoop4:="", lProductFamily:=v_lPMProductFamily, vDataModel:=System.DBNull.Value, vPropertyId:=System.DBNull.Value, r_oDatabase:=r_oDatabase)

                                                    If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                                                        GenerateStoredProcedure = gPMConstants.PMEReturnCode.PMFalse
                                                        Exit Function
                                                    End If
                                                    'UPGRADE_WARNING: Couldn't resolve default property of object vArray(ACPSpecialsType, lTemp2). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                                ElseIf vArray(ACPSpecialsType, lTemp2) = ACOProductID And Not bFixedField Then
                                                    'If it's a product, need a link to a policy...
                                                    sParameterList = sParameterList & "@" & sColumnName & " VARCHAR(255)," & vbCrLf

                                                    lOtherTableCount = lOtherTableCount + 1

                                                    sSelectList = sSelectList & sSelect & sColumnName & " = i" & lOtherTableCount & ".insurance_ref," & vbCrLf

                                                    sTableList = sTableList & "LEFT JOIN insurance_file i" & lOtherTableCount & " ON " & sPrefix & "." & sColumnName & " = i" & lOtherTableCount & ".insurance_file_cnt" & vbCrLf

                                                    'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
                                                    m_lReturn = AddToWPFields(sFieldName:=sPrefix & sFieldName, sSQL:=sProcedureName, sColumnName:=sColumnName, lColumnType:=gPMConstants.PMEFormatStyle.PMFormatString, sMainGroup:=sMainGroup, sSubGroup:=sSubGroup, sSubGroup2:=sSubGroup2, sSubGroup3:=sSubGroup3, sSubGroup4:="", sDisplayname:=sDisplayname, iIsDisplayed:=1, sLoop1:=sLoop1, sLoop2:=sLoop2, sLoop3:=sLoop3, sLoop4:="", lProductFamily:=v_lPMProductFamily, vDataModel:=System.DBNull.Value, vPropertyId:=System.DBNull.Value, r_oDatabase:=r_oDatabase)

                                                    If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                                                        GenerateStoredProcedure = gPMConstants.PMEReturnCode.PMFalse
                                                        Exit Function
                                                    End If

                                                    'else it's bog standard
                                                Else
                                                    sParameterList = sParameterList & "@" & sColumnName & " "

                                                    ' PW250703 - PS229  - don't add to
                                                    ' select list if field is from a fixed
                                                    ' table
                                                    If Not bFixedField Then
                                                        sSelectList = sSelectList & sSelect & sColumnName & " = " & sPrefix & "." & sColumnName & "," & vbCrLf
                                                    End If

                                                    Select Case vArray(ACPDataType, lTemp2)
                                                        Case GISSharedConstants.GISDataTypeComment
                                                            sParameterList = sParameterList & "VARCHAR(4000)," & vbCrLf
                                                            'sParameterList = sParameterList & "TEXT," & vbCrLf
                                                            lFormat = gPMConstants.PMEFormatStyle.PMFormatString

                                                        Case GISSharedConstants.GISDataTypeText
                                                            sTemp = "255"
                                                            sParameterList = sParameterList & "VARCHAR(" & sTemp & ")," & vbCrLf
                                                            lFormat = gPMConstants.PMEFormatStyle.PMFormatString

                                                        Case GISSharedConstants.GISDataTypeDate
                                                            sParameterList = sParameterList & "DATETIME," & vbCrLf
                                                            lFormat = gPMConstants.PMEFormatStyle.PMFormatDateLong

                                                        Case GISSharedConstants.GISDataTypeCurrency
                                                            sParameterList = sParameterList & "NUMERIC(19,4)," & vbCrLf
                                                            lFormat = gPMConstants.PMEFormatStyle.PMFormatCurrency
                                                            'SJ 15/07/2004 - start
                                                        Case GISSharedConstants.GISDataTypeInteger
                                                            sParameterList = sParameterList & "INT," & vbCrLf
                                                            lFormat = gPMConstants.PMEFormatStyle.PMFormatLong
                                                            'SJ 15/07/2004 - end
                                                        Case GISSharedConstants.GISDataTypePercentage
                                                            sParameterList = sParameterList & "NUMERIC(19,4)," & vbCrLf
                                                            lFormat = gPMConstants.PMEFormatStyle.PMFormatPercent

                                                        Case GISSharedConstants.GISDataTypeNumeric
                                                            sParameterList = sParameterList & "NUMERIC(19,4)," & vbCrLf
                                                            lFormat = gPMConstants.PMEFormatStyle.PMFormatLong

                                                        Case GISSharedConstants.GISDataTypeOption
                                                            sParameterList = sParameterList & "TINYINT," & vbCrLf
                                                            lFormat = gPMConstants.PMEFormatStyle.PMFormatBoolean

                                                        Case Else
                                                            sParameterList = sParameterList & "VARCHAR(255)," & vbCrLf
                                                            lFormat = gPMConstants.PMEFormatStyle.PMFormatString
                                                    End Select

                                                    sFinalSelectList = sFinalSelectList & "'" & sColumnName & "' = @" & sColumnName & "," & vbCrLf

                                                    'Add the record to wp_fields
                                                    ' PW240703 - PS229 - use a main type
                                                    ' of party for data model type of Party,
                                                    ' because the risk object isn't
                                                    ' really a risk object. But that's how
                                                    ' it's been done.
                                                    'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
                                                    m_lReturn = AddToWPFields(sFieldName:=sPrefix & sFieldName, sSQL:=sProcedureName, sColumnName:=sColumnName, lColumnType:=lFormat, sMainGroup:=sMainGroup, sSubGroup:=sSubGroup, sSubGroup2:=sSubGroup2, sSubGroup3:=sSubGroup3, sSubGroup4:="", sDisplayname:=sDisplayname, iIsDisplayed:=1, sLoop1:=sLoop1, sLoop2:=sLoop2, sLoop3:=sLoop3, sLoop4:="", lProductFamily:=v_lPMProductFamily, vDataModel:=System.DBNull.Value, vPropertyId:=System.DBNull.Value, r_oDatabase:=r_oDatabase)

                                                    If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                                                        GenerateStoredProcedure = gPMConstants.PMEReturnCode.PMFalse
                                                        Exit Function
                                                    End If
                                                End If
                                            End If
                                        End If
                                    Next lTemp2

                                End If

                                If (sParameterList <> "") Then
                                    'Get rid of the trailing comma and vbCrLf
                                    sParameterList = Left(sParameterList, Len(sParameterList) - 3) & vbCrLf
                                    sFinalSelectList = Left(sFinalSelectList, Len(sFinalSelectList) - 3) & vbCrLf

                                    'Add the rest...
                                    ' PW250703 - PS229 - depending on what the object type
                                    ' is
                                    Select Case iObjectType
                                        Case GISOTRisk
                                            sTableList = Left(sTableList, Len(sTableList) - 2) & ", " & vbCrLf & v_sDatamodel & "_Policy_binder pb," & vbCrLf & "GIS_policy_link gpl," & vbCrLf
                                            ' PW240703 - PS229 - party table required for
                                            ' data model type of Party, because the risk object isn't
                                            ' really a risk object. But that's how it's been done.
                                            If lGISDataModelType = GISDMTypeParty Then
                                                sTableList = sTableList & "party p" & vbCrLf
                                            Else
                                                sTableList = sTableList & "insurance_file ifi" & vbCrLf
                                            End If
                                        Case GISOTAssociatedClient
                                            sTableList = sTableList & "LEFT JOIN party p ON pc.party_cnt = p.party_cnt" & vbCrLf & "LEFT JOIN party_type pt ON p.party_type_id = pt.party_type_id" & vbCrLf & "LEFT JOIN party_personal_client ppc ON pc.party_cnt = ppc.party_cnt" & vbCrLf & "LEFT JOIN party_lifestyle pl ON pc.party_cnt = pl.party_cnt" & vbCrLf
                                        Case GISOTDisclosure
                                            sTableList = sTableList & "LEFT JOIN disclosure_type dt ON pcv.disclosure_type_id = " & "dt.disclosure_type_id" & vbCrLf
                                    End Select
                                    'Drop it if it's already there
                                    DropExistingProcedure(v_sProcedureName:=sProcedureName, v_sPostFix:="", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

                                    DropExistingProcedure(v_sProcedureName:=sProcedureNameOldStyle, v_sPostFix:="", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

                                    If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                                        GenerateStoredProcedure = gPMConstants.PMEReturnCode.PMFalse
                                        Exit Function
                                    End If

                                    'remove the last comma and vbcrlf
                                    sSelectList = Left(sSelectList, Len(sSelectList) - 3) & vbCrLf

                                    ' PW240703 - PS229 - don't need risk parameter for
                                    ' data model type of Party, because we're creating
                                    ' it in the party section of field manager.
                                    If lGISDataModelType <> GISDMTypeParty Then
                                        sSelectList = "If @RiskId Is Null" & vbCrLf & sSelectList & sTableList & sWhereList1 & sWhereList3 & "Else" & vbCrLf & sSelectList & sTableList & sWhereList2 & sWhereList3
                                    Else
                                        sSelectList = sSelectList & sTableList & sWhereList1 & sWhereList3
                                    End If
                                    'Create the new procedure
                                    sSQL = ""

                                    ' PW240703 - PS229 - don't need risk parameter for
                                    ' data model type of Party, because we're creating
                                    ' it in the party section of field manager.
                                    sSQL = sSQL & "CREATE PROCEDURE " & sProcedureName & vbCrLf & "@PartyCnt INT," & vbCrLf & "@InsuranceFileCnt INT," & vbCrLf
                                    If lGISDataModelType <> GISDMTypeParty Then
                                        sSQL = sSQL & "@RiskId INT = NULL," & vbCrLf
                                    End If
                                    sSQL = sSQL & "@ClaimCnt INT," & vbCrLf & "@DocumentRef VARCHAR(25)"
                                    'dynamically generate instance variables
                                    generateInstanceVariables(sSQL, iInstanceCount)

                                    ' PW250703 - PS229 - need to get the insurance folder cnt
                                    ' for object type of associated client / disclosure
                                    sSQL = sSQL & vbCrLf & "AS" & vbCrLf & vbCrLf
                                    ' PW200803 - CQ1912 - only disclosure needs this now
                                    If iObjectType = GISOTDisclosure Then
                                        sSQL = sSQL & "DECLARE @insurance_folder_cnt INT" & vbCrLf & "SELECT @insurance_folder_cnt = insurance_folder_cnt" & vbCrLf & "  FROM insurance_file" & vbCrLf & " WHERE insurance_file_cnt = @InsuranceFileCnt" & vbCrLf & vbCrLf & sSelectList & vbCrLf
                                    Else
                                        sSQL = sSQL & sSelectList & vbCrLf
                                    End If


                                    m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & sProcedureName & " StoredProcedure", bStoredProcedure:=False)

                                    If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                                        GenerateStoredProcedure = gPMConstants.PMEReturnCode.PMFalse
                                        Exit Function
                                    End If

                                    'Set permissions

                                    sSQL = "GRANT EXECUTE ON dbo." & sProcedureName & " TO PUBLIC" & vbCrLf

                                    m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Grant " & sProcedureName & " StoredProcedure", bStoredProcedure:=False)

                                    If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                                        GenerateStoredProcedure = gPMConstants.PMEReturnCode.PMFalse
                                        Exit Function
                                    End If

                                End If

                            End If
                        End If

                    End If ' CLAIM RISK OR NORMAL RISK

            End Select

        Next lTemp

        Exit Function

        Catch ex As Exception

        GenerateStoredProcedure = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GenerateStoredProcedure Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateStoredProcedure", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Exit Function
        
        End Try
    End Function

    Private Sub generateInstanceVariables(ByRef sSQL As String, ByVal lLevel As Integer)

        Dim lCount As Integer

        If lLevel < 3 Then lLevel = 3
        For lCount = 1 To lLevel
            sSQL = sSQL & "," & vbCrLf & "@Instance" & lCount & " INT = NULL"
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

        Dim sProcedureName As String
        Dim sProcedureNameOldStyle As String
        Dim sPrefix As String
        Dim sDescription As String
        Dim sSelectList As String
        Dim sSelectList2 As String
        Dim sSelectList3 As String
        Dim sTableList As String
        Dim sWhereList1 As String
        Dim sWhereList2 As String
        Dim lTemp As Integer
        Dim sSQL As String

        Try

        GenerateSumInsuredFields = gPMConstants.PMEReturnCode.PMTrue

        sPrefix = v_sDatamodel & "SI"

        sDescription = ""

        If Not IsArray(m_vSumInsured) Then
            m_lReturn = GetALookup(iLanguageID:=1, sTableName:="sum_insured_type", vArray:=m_vSumInsured, r_oDatabase:=r_oDatabase)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                GenerateSumInsuredFields = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

        End If

        If IsArray(m_vSumInsured) Then
            For lTemp = LBound(m_vSumInsured, 2) To UBound(m_vSumInsured, 2)
                'UPGRADE_WARNING: Couldn't resolve default property of object m_vSumInsured(0, lTemp). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If (m_vSumInsured(0, lTemp) = lSumInsuredTypeId) Then
                    'UPGRADE_WARNING: Couldn't resolve default property of object m_vSumInsured(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    sDescription = m_vSumInsured(1, lTemp) & " SI"
                    Exit For
                End If
            Next lTemp
        End If

        'The get keys procedure...

        sProcedureNameOldStyle = "sp_wp_" & v_sDatamodel & "SumInsured" & lSumInsuredTypeId & "_get_keys"
        sProcedureName = "spg_wp_" & v_sDatamodel & "SumInsured" & lSumInsuredTypeId & "_get_keys"

        sSelectList = "SELECT " & sPrefix & ".sequence_id" & vbCrLf

        sTableList = "FROM " & v_sDatamodel & "_sum_insured " & sPrefix & ", " & vbCrLf & v_sDatamodel & "_Policy_binder pb," & vbCrLf & "GIS_policy_link gpl," & vbCrLf & "insurance_file ifi" & vbCrLf

        sWhereList1 = "WHERE ifi.insurance_file_cnt = @InsuranceFileCnt" & vbCrLf & "AND gpl.insurance_file_cnt = ifi.insurance_file_cnt" & vbCrLf & "AND gpl.Risk_id is null" & vbCrLf & "AND pb.gis_policy_link_id = gpl.gis_policy_link_id" & vbCrLf & "AND " & sPrefix & "." & v_sDatamodel & "_policy_binder_id = pb." & v_sDatamodel & "_Policy_binder_id" & vbCrLf & "AND " & sPrefix & ".sum_insured_type_id = " & lSumInsuredTypeId & vbCrLf & "AND " & sPrefix & ".sequence_id > 1" & vbCrLf & "AND ISNULL(" & sPrefix & ".date_deleted, '1899-12-29') = '1899-12-29'" & vbCrLf

        sWhereList2 = "WHERE ifi.insurance_file_cnt = @InsuranceFileCnt" & vbCrLf & "AND gpl.Risk_id = @RiskId" & vbCrLf & "AND pb.gis_policy_link_id = gpl.gis_policy_link_id" & vbCrLf & "AND " & sPrefix & "." & v_sDatamodel & "_policy_binder_id = pb." & v_sDatamodel & "_Policy_binder_id" & vbCrLf & "AND " & sPrefix & ".sum_insured_type_id = " & lSumInsuredTypeId & vbCrLf & "AND " & sPrefix & ".sequence_id > 1" & vbCrLf & "AND ISNULL(" & sPrefix & ".date_deleted, '1899-12-29') = '1899-12-29'" & vbCrLf

        'Drop it if it's already there
        DropExistingProcedure(sProcedureName, "", m_lReturn, r_oDatabase)
        DropExistingProcedure(sProcedureNameOldStyle, "", m_lReturn, r_oDatabase)


        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateSumInsuredFields = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        sSelectList = "If @RiskId Is Null" & vbCrLf & sSelectList & sTableList & sWhereList1 & "Else" & vbCrLf & sSelectList & sTableList & sWhereList2

        'Create the new procedure
        sSQL = ""

        sSQL = sSQL & "CREATE PROCEDURE " & sProcedureName & " @PartyCnt INT," & vbCrLf & "@InsuranceFileCnt INT," & vbCrLf & "@RiskId INT = NULL," & vbCrLf & "@ClaimCnt INT," & vbCrLf & "@DocumentRef VARCHAR(25)," & vbCrLf & "@Instance1 INT," & vbCrLf & "@Instance2 INT," & vbCrLf & "@Instance3 INT" & vbCrLf & "AS" & vbCrLf & vbCrLf

        sSQL = sSQL & sSelectList & vbCrLf

        m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & sProcedureName & " StoredProcedure", bStoredProcedure:=False)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateSumInsuredFields = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        'Set permissions

        sSQL = "GRANT EXECUTE ON dbo." & sProcedureName & " TO PUBLIC" & vbCrLf

        m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Grant " & sProcedureName & " StoredProcedure", bStoredProcedure:=False)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateSumInsuredFields = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        'The get parent key procedure - needed because of the way DP now works...
        sProcedureName = "spg_wp_" & v_sDatamodel & "SumInsured" & lSumInsuredTypeId & "_get_parent_key"

        'Doesn't need to do a lot, just exist and pass back something that's ignored anyway
        sSelectList = "SELECT 1" & vbCrLf

        sTableList = ""

        sWhereList1 = ""

        sWhereList2 = ""

        'Drop it if it's already there
        DropExistingProcedure(sProcedureName, "", m_lReturn, r_oDatabase)
        DropExistingProcedure(sProcedureNameOldStyle, "", m_lReturn, r_oDatabase)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateSumInsuredFields = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        sSelectList = "If @RiskId Is Null" & vbCrLf & sSelectList & sTableList & sWhereList1 & "Else" & vbCrLf & sSelectList & sTableList & sWhereList2

        'Create the new procedure
        sSQL = ""

        sSQL = sSQL & "CREATE PROCEDURE " & sProcedureName & " @PartyCnt INT," & vbCrLf & "@InsuranceFileCnt INT," & vbCrLf & "@RiskId INT = NULL," & vbCrLf & "@ClaimCnt INT," & vbCrLf & "@DocumentRef VARCHAR(25)," & vbCrLf & "@Instance1 INT," & vbCrLf & "@Instance2 INT," & vbCrLf & "@Instance3 INT" & vbCrLf & "AS" & vbCrLf & vbCrLf

        sSQL = sSQL & sSelectList & vbCrLf

        m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & sProcedureName & " StoredProcedure", bStoredProcedure:=False)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateSumInsuredFields = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        'Set permissions

        sSQL = "GRANT EXECUTE ON dbo." & sProcedureName & " TO PUBLIC" & vbCrLf

        m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Grant " & sProcedureName & " StoredProcedure", bStoredProcedure:=False)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateSumInsuredFields = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        'The main procedure...
        sProcedureName = "spg_wp_" & v_sDatamodel & "SumInsured" & lSumInsuredTypeId

        sSelectList = "SELECT description = " & sPrefix & ".description," & vbCrLf & "reference = " & sPrefix & ".reference," & vbCrLf & "sum_insured = " & sPrefix & ".sum_insured," & vbCrLf & "date_added = " & sPrefix & ".date_added," & vbCrLf & "date_deleted = " & sPrefix & ".date_deleted," & vbCrLf & "is_valuation_required = " & sPrefix & ".is_valuation_required," & vbCrLf & "valuation_date = " & sPrefix & ".valuation_date" & vbCrLf

        sTableList = "FROM " & v_sDatamodel & "_sum_insured " & sPrefix & ", " & vbCrLf & v_sDatamodel & "_Policy_binder pb," & vbCrLf & "GIS_policy_link gpl," & vbCrLf & "insurance_file ifi" & vbCrLf

        sWhereList1 = "WHERE ifi.insurance_file_cnt = @InsuranceFileCnt" & vbCrLf & "AND gpl.insurance_file_cnt = ifi.insurance_file_cnt" & vbCrLf & "AND gpl.Risk_id is null" & vbCrLf & "AND pb.gis_policy_link_id = gpl.gis_policy_link_id" & vbCrLf & "AND " & sPrefix & "." & v_sDatamodel & "_policy_binder_id = pb." & v_sDatamodel & "_Policy_binder_id" & vbCrLf & "AND " & sPrefix & ".sum_insured_type_id = " & lSumInsuredTypeId & vbCrLf & "AND " & sPrefix & ".sequence_id = @Instance2" & vbCrLf

        sWhereList2 = "WHERE ifi.insurance_file_cnt = @InsuranceFileCnt" & vbCrLf & "AND gpl.Risk_id = @RiskId" & vbCrLf & "AND pb.gis_policy_link_id = gpl.gis_policy_link_id" & vbCrLf & "AND " & sPrefix & "." & v_sDatamodel & "_policy_binder_id = pb." & v_sDatamodel & "_Policy_binder_id" & vbCrLf & "AND " & sPrefix & ".sum_insured_type_id = " & lSumInsuredTypeId & vbCrLf & "AND " & sPrefix & ".sequence_id = @Instance2" & vbCrLf

        'Drop it if it's already there
        DropExistingProcedure(sProcedureName, "", m_lReturn, r_oDatabase)
        DropExistingProcedure(sProcedureNameOldStyle, "", m_lReturn, r_oDatabase)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateSumInsuredFields = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        sSelectList = "If @RiskId Is Null" & vbCrLf & sSelectList & sTableList & sWhereList1 & "Else" & vbCrLf & sSelectList & sTableList & sWhereList2

        'Create the new procedure
        sSQL = ""

        sSQL = sSQL & "CREATE PROCEDURE " & sProcedureName & " @PartyCnt INT," & vbCrLf & "@InsuranceFileCnt INT," & vbCrLf & "@RiskId INT = NULL," & vbCrLf & "@ClaimCnt INT," & vbCrLf & "@DocumentRef VARCHAR(25)," & vbCrLf & "@Instance1 INT," & vbCrLf & "@Instance2 INT," & vbCrLf & "@Instance3 INT" & vbCrLf & "AS" & vbCrLf & vbCrLf

        sSQL = sSQL & sSelectList & vbCrLf

        m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & sProcedureName & " StoredProcedure", bStoredProcedure:=False)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateSumInsuredFields = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        'Set permissions

        sSQL = "GRANT EXECUTE ON dbo." & sProcedureName & " TO PUBLIC" & vbCrLf

        m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Grant " & sProcedureName & " StoredProcedure", bStoredProcedure:=False)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateSumInsuredFields = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        'Now let's set up the individual wp fields
        'Delete the records on wp_fields
        sSQL = ""

        sSQL = sSQL & "DELETE FROM wp_fields" & vbCrLf & "WHERE sql = '" & sProcedureName & "'" & vbCrLf

        m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Delete " & sProcedureName & " WPFields", bStoredProcedure:=False)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateSumInsuredFields = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
        m_lReturn = AddToWPFields(sFieldName:=v_sDatamodel & "SumInsuredDescription" & lSumInsuredTypeId, sSQL:=sProcedureName, sColumnName:="description", lColumnType:=gPMConstants.PMEFormatStyle.PMFormatString, sMainGroup:="Risk", sSubGroup:=sDescription, sSubGroup2:="", sSubGroup3:="", sSubGroup4:="", sDisplayname:="Description", iIsDisplayed:=1, sLoop1:=v_sDatamodel & "SumInsured" & lSumInsuredTypeId, sLoop2:="", sLoop3:="", sLoop4:="", lProductFamily:=v_lPMProductFamily, vDataModel:=System.DBNull.Value, vPropertyId:=System.DBNull.Value, r_oDatabase:=r_oDatabase)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateSumInsuredFields = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
        m_lReturn = AddToWPFields(sFieldName:=v_sDatamodel & "SumInsuredReference" & lSumInsuredTypeId, sSQL:=sProcedureName, sColumnName:="reference", lColumnType:=gPMConstants.PMEFormatStyle.PMFormatString, sMainGroup:="Risk", sSubGroup:=sDescription, sSubGroup2:="", sSubGroup3:="", sSubGroup4:="", sDisplayname:="Reference", iIsDisplayed:=1, sLoop1:=v_sDatamodel & "SumInsured" & lSumInsuredTypeId, sLoop2:="", sLoop3:="", sLoop4:="", lProductFamily:=v_lPMProductFamily, vDataModel:=System.DBNull.Value, vPropertyId:=System.DBNull.Value, r_oDatabase:=r_oDatabase)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateSumInsuredFields = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
        m_lReturn = AddToWPFields(sFieldName:=v_sDatamodel & "SumInsuredSumInsured" & lSumInsuredTypeId, sSQL:=sProcedureName, sColumnName:="sum_insured", lColumnType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, sMainGroup:="Risk", sSubGroup:=sDescription, sSubGroup2:="", sSubGroup3:="", sSubGroup4:="", sDisplayname:="Sum Insured", iIsDisplayed:=1, sLoop1:=v_sDatamodel & "SumInsured" & lSumInsuredTypeId, sLoop2:="", sLoop3:="", sLoop4:="", lProductFamily:=v_lPMProductFamily, vDataModel:=System.DBNull.Value, vPropertyId:=System.DBNull.Value, r_oDatabase:=r_oDatabase)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateSumInsuredFields = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
        m_lReturn = AddToWPFields(sFieldName:=v_sDatamodel & "SumInsuredDateAdded" & lSumInsuredTypeId, sSQL:=sProcedureName, sColumnName:="date_added", lColumnType:=gPMConstants.PMEFormatStyle.PMFormatDateLong, sMainGroup:="Risk", sSubGroup:=sDescription, sSubGroup2:="", sSubGroup3:="", sSubGroup4:="", sDisplayname:="Date Added", iIsDisplayed:=1, sLoop1:=v_sDatamodel & "SumInsured" & lSumInsuredTypeId, sLoop2:="", sLoop3:="", sLoop4:="", lProductFamily:=v_lPMProductFamily, vDataModel:=System.DBNull.Value, vPropertyId:=System.DBNull.Value, r_oDatabase:=r_oDatabase)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateSumInsuredFields = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
        m_lReturn = AddToWPFields(sFieldName:=v_sDatamodel & "SumInsuredDateDeleted" & lSumInsuredTypeId, sSQL:=sProcedureName, sColumnName:="date_deleted", lColumnType:=gPMConstants.PMEFormatStyle.PMFormatDateLong, sMainGroup:="Risk", sSubGroup:=sDescription, sSubGroup2:="", sSubGroup3:="", sSubGroup4:="", sDisplayname:="Date Deleted", iIsDisplayed:=1, sLoop1:=v_sDatamodel & "SumInsured" & lSumInsuredTypeId, sLoop2:="", sLoop3:="", sLoop4:="", lProductFamily:=v_lPMProductFamily, vDataModel:=System.DBNull.Value, vPropertyId:=System.DBNull.Value, r_oDatabase:=r_oDatabase)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateSumInsuredFields = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
        m_lReturn = AddToWPFields(sFieldName:=v_sDatamodel & "SumInsuredIsValuationRequired" & lSumInsuredTypeId, sSQL:=sProcedureName, sColumnName:="is_valuation_required", lColumnType:=gPMConstants.PMEFormatStyle.PMFormatBoolean, sMainGroup:="Risk", sSubGroup:=sDescription, sSubGroup2:="", sSubGroup3:="", sSubGroup4:="", sDisplayname:="Is valuation required", iIsDisplayed:=1, sLoop1:=v_sDatamodel & "SumInsured" & lSumInsuredTypeId, sLoop2:="", sLoop3:="", sLoop4:="", lProductFamily:=v_lPMProductFamily, vDataModel:=System.DBNull.Value, vPropertyId:=System.DBNull.Value, r_oDatabase:=r_oDatabase)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateSumInsuredFields = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
        m_lReturn = AddToWPFields(sFieldName:=v_sDatamodel & "SumInsuredValuationDate" & lSumInsuredTypeId, sSQL:=sProcedureName, sColumnName:="valuation_date", lColumnType:=gPMConstants.PMEFormatStyle.PMFormatDateLong, sMainGroup:="Risk", sSubGroup:=sDescription, sSubGroup2:="", sSubGroup3:="", sSubGroup4:="", sDisplayname:="Valuation date", iIsDisplayed:=1, sLoop1:=v_sDatamodel & "SumInsured" & lSumInsuredTypeId, sLoop2:="", sLoop3:="", sLoop4:="", lProductFamily:=v_lPMProductFamily, vDataModel:=System.DBNull.Value, vPropertyId:=System.DBNull.Value, r_oDatabase:=r_oDatabase)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateSumInsuredFields = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        'The total procedure...
        sProcedureName = "spg_wp_" & v_sDatamodel & "SumInsuredTotal" & lSumInsuredTypeId

        sSelectList = "SELECT @rate = " & sPrefix & ".rate," & vbCrLf & "@Premium = " & sPrefix & ".premium" & vbCrLf

        sSelectList2 = "SELECT @total_sum_insured = sum(" & sPrefix & ".sum_insured)" & vbCrLf

        sSelectList3 = "SELECT total_sum_insured = @total_sum_insured," & vbCrLf & "rate = @rate," & vbCrLf & "premium = @premium" & vbCrLf


        sTableList = "FROM " & v_sDatamodel & "_sum_insured " & sPrefix & ", " & vbCrLf & v_sDatamodel & "_Policy_binder pb," & vbCrLf & "GIS_policy_link gpl," & vbCrLf & "insurance_file ifi" & vbCrLf

        sWhereList1 = "WHERE ifi.insurance_file_cnt = @InsuranceFileCnt" & vbCrLf & "AND gpl.insurance_file_cnt = ifi.insurance_file_cnt" & vbCrLf & "AND gpl.Risk_id is null" & vbCrLf & "AND pb.gis_policy_link_id = gpl.gis_policy_link_id" & vbCrLf & "AND " & sPrefix & "." & v_sDatamodel & "_policy_binder_id = pb." & v_sDatamodel & "_Policy_binder_id" & vbCrLf & "AND " & sPrefix & ".sum_insured_type_id = " & lSumInsuredTypeId & vbCrLf

        sWhereList2 = "WHERE ifi.insurance_file_cnt = @InsuranceFileCnt" & vbCrLf & "AND gpl.Risk_id = @RiskId" & vbCrLf & "AND pb.gis_policy_link_id = gpl.gis_policy_link_id" & vbCrLf & "AND " & sPrefix & "." & v_sDatamodel & "_policy_binder_id = pb." & v_sDatamodel & "_Policy_binder_id" & vbCrLf & "AND " & sPrefix & ".sum_insured_type_id = " & lSumInsuredTypeId & vbCrLf

        'Drop it if it's already there
        DropExistingProcedure(sProcedureName, "", m_lReturn, r_oDatabase)
        DropExistingProcedure(sProcedureNameOldStyle, "", m_lReturn, r_oDatabase)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateSumInsuredFields = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        sSelectList = "If @RiskId Is Null" & vbCrLf & "Begin" & vbCrLf & sSelectList & sTableList & sWhereList1 & "AND " & sPrefix & ".sequence_id = 1" & vbCrLf & vbCrLf & sSelectList2 & sTableList & sWhereList1 & "AND ISNULL(" & sPrefix & ".date_deleted, '1899-12-29') = '1899-12-29'" & vbCrLf & "AND " & sPrefix & ".sequence_id > 1" & vbCrLf & "End" & vbCrLf & "Else" & vbCrLf & "Begin" & vbCrLf & sSelectList & sTableList & sWhereList2 & "AND " & sPrefix & ".sequence_id = 1" & vbCrLf & vbCrLf & sSelectList2 & sTableList & sWhereList2 & "AND ISNULL(" & sPrefix & ".date_deleted, '1899-12-29') = '1899-12-29'" & vbCrLf & "AND " & sPrefix & ".sequence_id > 1" & vbCrLf & "End" & vbCrLf & sSelectList3 & vbCrLf

        'Create the new procedure
        sSQL = ""

        sSQL = sSQL & "CREATE PROCEDURE " & sProcedureName & " @PartyCnt INT," & vbCrLf & "@InsuranceFileCnt INT," & vbCrLf & "@RiskId INT = NULL," & vbCrLf & "@ClaimCnt INT," & vbCrLf & "@DocumentRef VARCHAR(25)," & vbCrLf & "@Instance1 INT," & vbCrLf & "@Instance2 INT," & vbCrLf & "@Instance3 INT" & vbCrLf & "AS" & vbCrLf & vbCrLf & "DECLARE @total_sum_insured NUMERIC(19,4)," & vbCrLf & "@rate NUMERIC(7,4)," & vbCrLf & "@premium NUMERIC(19,4)" & vbCrLf

        sSQL = sSQL & sSelectList & vbCrLf

        m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & sProcedureName & " StoredProcedure", bStoredProcedure:=False)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateSumInsuredFields = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        'Set permissions

        sSQL = "GRANT EXECUTE ON dbo." & sProcedureName & " TO PUBLIC" & vbCrLf

        m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Grant " & sProcedureName & " StoredProcedure", bStoredProcedure:=False)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateSumInsuredFields = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        'Now let's set up the total wp fields
        'Delete the records on wp_fields
        sSQL = ""

        sSQL = sSQL & "DELETE FROM wp_fields" & vbCrLf & "WHERE sql = '" & sProcedureName & "'" & vbCrLf

        m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Delete " & sProcedureName & " WPFields", bStoredProcedure:=False)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateSumInsuredFields = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
        m_lReturn = AddToWPFields(sFieldName:=v_sDatamodel & "TotalSumInsured" & lSumInsuredTypeId, sSQL:=sProcedureName, sColumnName:="total_sum_insured", lColumnType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, sMainGroup:="Risk", sSubGroup:=sDescription, sSubGroup2:="", sSubGroup3:="", sSubGroup4:="", sDisplayname:="Total Sum Insured", iIsDisplayed:=1, sLoop1:="", sLoop2:="", sLoop3:="", sLoop4:="", lProductFamily:=v_lPMProductFamily, vDataModel:=System.DBNull.Value, vPropertyId:=System.DBNull.Value, r_oDatabase:=r_oDatabase)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateSumInsuredFields = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
        m_lReturn = AddToWPFields(sFieldName:=v_sDatamodel & "Rate" & lSumInsuredTypeId, sSQL:=sProcedureName, sColumnName:="rate", lColumnType:=gPMConstants.PMEFormatStyle.PMFormatPercent, sMainGroup:="Risk", sSubGroup:=sDescription, sSubGroup2:="", sSubGroup3:="", sSubGroup4:="", sDisplayname:="Rate", iIsDisplayed:=1, sLoop1:="", sLoop2:="", sLoop3:="", sLoop4:="", lProductFamily:=v_lPMProductFamily, vDataModel:=System.DBNull.Value, vPropertyId:=System.DBNull.Value, r_oDatabase:=r_oDatabase)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateSumInsuredFields = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
        m_lReturn = AddToWPFields(sFieldName:=v_sDatamodel & "Premium" & lSumInsuredTypeId, sSQL:=sProcedureName, sColumnName:="premium", lColumnType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, sMainGroup:="Risk", sSubGroup:=sDescription, sSubGroup2:="", sSubGroup3:="", sSubGroup4:="", sDisplayname:="Premium", iIsDisplayed:=1, sLoop1:="", sLoop2:="", sLoop3:="", sLoop4:="", lProductFamily:=v_lPMProductFamily, vDataModel:=System.DBNull.Value, vPropertyId:=System.DBNull.Value, r_oDatabase:=r_oDatabase)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateSumInsuredFields = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        Exit Function

        Catch ex As Exception

        GenerateSumInsuredFields = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GenerateSumInsuredFields Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateSumInsuredFields", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Exit Function

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
    Private Function AddToWPFields(ByRef sFieldName As String, ByRef sSQL As String, ByRef sColumnName As String, ByRef lColumnType As Integer, ByRef sMainGroup As String, ByRef sSubGroup As String, ByRef sSubGroup2 As String, ByRef sSubGroup3 As String, ByRef sSubGroup4 As String, ByRef sDisplayname As String, ByRef iIsDisplayed As Short, ByRef sLoop1 As String, ByRef sLoop2 As String, ByRef sLoop3 As String, ByRef sLoop4 As String, ByRef lProductFamily As Integer, ByRef vDataModel As Object, ByRef vPropertyId As Object, ByRef r_oDatabase As dPMDAO.Database) As Integer

        Dim vLoop1 As Object
        Dim vLoop2 As Object
        Dim vLoop3 As Object
        Dim vLoop4 As Object


        AddToWPFields = gPMConstants.PMEReturnCode.PMTrue

        m_lReturn = GetValidFieldName(sFieldName:=sFieldName, r_oDatabase:=r_oDatabase)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            AddToWPFields = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        r_oDatabase.Parameters.Clear()

        If (Trim(sLoop1) = "") Then
            'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
            'UPGRADE_WARNING: Couldn't resolve default property of object vLoop1. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            vLoop1 = System.DBNull.Value
        Else
            'UPGRADE_WARNING: Couldn't resolve default property of object vLoop1. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            vLoop1 = Trim(sLoop1)
        End If

        If (Trim(sLoop2) = "") Then
            'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
            'UPGRADE_WARNING: Couldn't resolve default property of object vLoop2. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            vLoop2 = System.DBNull.Value
        Else
            'UPGRADE_WARNING: Couldn't resolve default property of object vLoop2. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            vLoop2 = Trim(sLoop2)
        End If

        If (Trim(sLoop3) = "") Then
            'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
            'UPGRADE_WARNING: Couldn't resolve default property of object vLoop3. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            vLoop3 = System.DBNull.Value
        Else
            'UPGRADE_WARNING: Couldn't resolve default property of object vLoop3. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            vLoop3 = Trim(sLoop3)
        End If

        If (Trim(sLoop4) = "") Then
            'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
            'UPGRADE_WARNING: Couldn't resolve default property of object vLoop4. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            vLoop4 = System.DBNull.Value
        Else
            'UPGRADE_WARNING: Couldn't resolve default property of object vLoop4. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            vLoop4 = Trim(sLoop4)
        End If

        m_lReturn = r_oDatabase.Parameters.Add(sName:="field_name", vValue:=sFieldName, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            AddToWPFields = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = r_oDatabase.Parameters.Add(sName:="sql", vValue:=sSQL, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            AddToWPFields = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = r_oDatabase.Parameters.Add(sName:="column_name", vValue:=sColumnName, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            AddToWPFields = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = r_oDatabase.Parameters.Add(sName:="column_type", vValue:=lColumnType, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            AddToWPFields = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = r_oDatabase.Parameters.Add(sName:="main_group", vValue:=sMainGroup, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            AddToWPFields = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = r_oDatabase.Parameters.Add(sName:="sub_group", vValue:=sSubGroup, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            AddToWPFields = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = r_oDatabase.Parameters.Add(sName:="display_name", vValue:=sDisplayname, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            AddToWPFields = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = r_oDatabase.Parameters.Add(sName:="is_displayed", vValue:=iIsDisplayed, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            AddToWPFields = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = r_oDatabase.Parameters.Add(sName:="loop1", vValue:=vLoop1, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            AddToWPFields = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = r_oDatabase.Parameters.Add(sName:="loop2", vValue:=vLoop2, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            AddToWPFields = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = r_oDatabase.Parameters.Add(sName:="loop3", vValue:=vLoop3, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            AddToWPFields = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = r_oDatabase.Parameters.Add(sName:="loop4", vValue:=vLoop4, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            AddToWPFields = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = r_oDatabase.Parameters.Add(sName:="product_family", vValue:=lProductFamily, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            AddToWPFields = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = r_oDatabase.Parameters.Add(sName:="data_model", vValue:=vDataModel, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            AddToWPFields = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = r_oDatabase.Parameters.Add(sName:="property_id", vValue:=vPropertyId, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            AddToWPFields = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = r_oDatabase.Parameters.Add(sName:="sub_group2", vValue:=sSubGroup2, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            AddToWPFields = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = r_oDatabase.Parameters.Add(sName:="sub_group3", vValue:=sSubGroup3, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            AddToWPFields = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = r_oDatabase.Parameters.Add(sName:="sub_group4", vValue:=sSubGroup4, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            AddToWPFields = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = r_oDatabase.SQLAction(sSQL:=ACInsertWPFieldsSQL, sSQLName:=ACInsertWPFieldsName, bStoredProcedure:=ACInsertWPFieldsStored)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            AddToWPFields = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        Exit Function

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


        GenerateStandardWordingFields = gPMConstants.PMEReturnCode.PMTrue

        Exit Function

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

        Dim sTemp As String
        Dim lTemp As Integer


        GetProcedureName = gPMConstants.PMEReturnCode.PMTrue

        sTemp = ""

        'UPGRADE_WARNING: Couldn't resolve default property of object StripDataModelCode(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        r_sProcedureName = StripDataModelCode(v_vTheString:=r_sProcedureName, v_sDatamodel:=v_sDatamodel)
        lTemp = InStr(r_sProcedureName, "_")

        While lTemp <> 0
            sTemp = sTemp & Left(r_sProcedureName, lTemp - 1)
            r_sProcedureName = Mid(r_sProcedureName, lTemp + 1)
            lTemp = InStr(r_sProcedureName, "_")
        End While

        sTemp = sTemp & r_sProcedureName

        r_sProcedureName = sTemp

        Exit Function

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
    Public Function StripDataModelCode(ByRef v_vTheString As Object, ByRef v_sDatamodel As String) As Object

        'UPGRADE_WARNING: Couldn't resolve default property of object v_vTheString. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Couldn't resolve default property of object StripDataModelCode. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        StripDataModelCode = v_vTheString
        'UPGRADE_WARNING: Couldn't resolve default property of object v_vTheString. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Left(v_vTheString, Len(v_sDatamodel)) = v_sDatamodel Or Left(v_vTheString, Len(v_sDatamodel)) = LCase(v_sDatamodel) Then
            'UPGRADE_WARNING: Couldn't resolve default property of object v_vTheString. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            StripDataModelCode = Mid(v_vTheString, Len(v_sDatamodel) + 1)
        End If

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
    Private Function IsTopLevelParentObjectMultipleInstance(ByVal v_sObjectName As String, ByRef r_vGISObject As Object) As Integer

        Dim lTemp As Integer
        Dim lParentId As Integer
        Dim lTempParentId As Object

        ' Debug message
        Debug.Print(VB.Timer() & ": Entering " & ACApp & "." & ACClass & ".IsObjectMultipleInstance")


        IsTopLevelParentObjectMultipleInstance = 0

        lParentId = -1

        If IsArray(r_vGISObject) Then
            For lTemp = LBound(r_vGISObject, 2) To UBound(r_vGISObject, 2)
                'UPGRADE_WARNING: Couldn't resolve default property of object r_vGISObject(ACOObjectName, lTemp). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If r_vGISObject(ACOObjectName, lTemp) = v_sObjectName Then
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_vGISObject(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    lParentId = r_vGISObject(ACOParentObjectId, lTemp)
                    Exit For
                End If
            Next
        End If

        If lParentId <> -1 And IsArray(r_vGISObject) Then

            'UPGRADE_WARNING: Couldn't resolve default property of object lTempParentId. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            lTempParentId = lParentId
            'UPGRADE_WARNING: Couldn't resolve default property of object lTempParentId. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            While lTempParentId <> 0
                For lTemp = LBound(r_vGISObject, 2) To UBound(r_vGISObject, 2)
                    'UPGRADE_WARNING: Couldn't resolve default property of object lTempParentId. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_vGISObject(ACOGISObjectId, lTemp). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If r_vGISObject(ACOGISObjectId, lTemp) = lTempParentId Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object lTempParentId. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        lParentId = lTempParentId
                        'UPGRADE_WARNING: Couldn't resolve default property of object r_vGISObject(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object lTempParentId. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        lTempParentId = r_vGISObject(ACOParentObjectId, lTemp)
                        'UPGRADE_WARNING: Couldn't resolve default property of object r_vGISObject(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        If False = (Right(LCase(r_vGISObject(ACOObjectName, lTemp)), 13) = "policy_binder") Then
                            'UPGRADE_WARNING: Couldn't resolve default property of object r_vGISObject(ACOMaxInstances, lTemp). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            If r_vGISObject(ACOMaxInstances, lTemp) > 1 Then
                                IsTopLevelParentObjectMultipleInstance = 1
                            Else
                                IsTopLevelParentObjectMultipleInstance = 0
                            End If
                        End If
                        Exit For
                    End If
                Next
            End While
        End If

        ' Debug message
        Debug.Print(VB.Timer() & ": Exiting " & ACApp & "." & ACClass & ".IsObjectMultipleInstance")

        Exit Function

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
    Public Function GetALookup(ByRef iLanguageID As Short, ByRef sTableName As String, ByRef vArray(,) As Object, ByRef r_oDatabase As dPMDAO.Database) As Integer 

        Try

        GetALookup = gPMConstants.PMEReturnCode.PMTrue

        r_oDatabase.Parameters.Clear()

        m_lReturn = r_oDatabase.Parameters.Add(sName:="table_name", vValue:=sTableName, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMTableName)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GetALookup = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = r_oDatabase.Parameters.Add(sName:="effective_date", vValue:=Now, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GetALookup = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = r_oDatabase.Parameters.Add(sName:="language_id", vValue:=1, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GetALookup = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        ' Execute SQL Statement
        m_lReturn = r_oDatabase.SQLSelect(sSQL:=ACGetLookupSQL, sSQLName:=ACGetLookupName, bStoredProcedure:=ACGetLookupStored, lnumberrecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GetALookup = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        Exit Function

        Catch ex As Exception

        GetALookup = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetALookup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetALookup", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Exit Function

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

        Dim sTemp As String 
        Dim lTemp As Integer 
        Dim bOK As Boolean 
        Dim vArray(,) As Object 


        GetValidFieldName = gPMConstants.PMEReturnCode.PMTrue

        bOK = False
        sTemp = sFieldName

        While bOK = False

            r_oDatabase.Parameters.Clear()

            m_lReturn = r_oDatabase.Parameters.Add(sName:="field_name", vValue:=sTemp, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                GetValidFieldName = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = r_oDatabase.SQLSelect(sSQL:=ACSelectWPFieldsSQL, sSQLName:=ACSelectWPFieldsName, bStoredProcedure:=ACSelectWPFieldsStored, lnumberrecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                GetValidFieldName = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            If IsArray(vArray) Then
                lTemp = lTemp + 1
                sTemp = sFieldName & lTemp
            Else
                bOK = True
            End If

        End While

        sFieldName = sTemp

        Exit Function

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

        Dim sDefSubKey As String
        Dim sSubKey As String
        Dim sTemp As String
        Dim vRegistryKeys As Object
        Dim sSettingItem As Object


        Try

        CreateRegistrySettings = gPMConstants.PMEReturnCode.PMTrue

        sDefSubKey = GISSharedConstants.ACOIMGISSubKey & "\" & "NB" & "\" & "DEF"

        sSubKey = GISSharedConstants.ACOIMGISSubKey & "\" & "NB" & "\" & v_sGISDataModel

        'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        'UPGRADE_WARNING: Couldn't resolve default property of object vRegistryKeys. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        vRegistryKeys = New Object() {"RulePath", "SchemePath", "DictPath"}

        For Each sSettingItem In vRegistryKeys
            'Get the value
            'UPGRADE_WARNING: Couldn't resolve default property of object sSettingItem. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=sSettingItem, r_sSettingValue:=sTemp, v_sSubKey:=sDefSubKey)

            'Set the value
            'UPGRADE_WARNING: Couldn't resolve default property of object sSettingItem. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=sSettingItem, v_sSettingValue:=sTemp, v_sSubKey:=sSubKey)
        Next sSettingItem


        'set the data model specific root items
        'some of these are duplicated in NB/data model but.....
        sDefSubKey = GISSharedConstants.ACOIMGISSubKey & "\" & "DEF"
        sSubKey = GISSharedConstants.ACOIMGISSubKey & "\" & v_sGISDataModel

        'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        'UPGRADE_WARNING: Couldn't resolve default property of object vRegistryKeys. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        vRegistryKeys = New Object() {GISSharedConstants.GISRegDataSetPath, "Insurers", GISSharedConstants.GISRegLookupPath, GISSharedConstants.GISQEMMethodsVersionNum, "RulePath", GISSharedConstants.GISRegBOMRequired}

        For Each sSettingItem In vRegistryKeys
            'Get the value
            'UPGRADE_WARNING: Couldn't resolve default property of object sSettingItem. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=sSettingItem, r_sSettingValue:=sTemp, v_sSubKey:=sDefSubKey)

            'Set the value
            'UPGRADE_WARNING: Couldn't resolve default property of object sSettingItem. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=sSettingItem, v_sSettingValue:=sTemp, v_sSubKey:=sSubKey)
        Next sSettingItem

        sDefSubKey = GISSharedConstants.ACOIMGISSubKey & "\" & "DEF" & "\" & "NB"

        sSubKey = GISSharedConstants.ACOIMGISSubKey & "\" & v_sGISDataModel & "\" & "NB"

        'Get save on quote
        m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="SaveOnQuote", r_sSettingValue:=sTemp, v_sSubKey:=sDefSubKey)

        'Set save on quote
        m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="SaveOnQuote", v_sSettingValue:=sTemp, v_sSubKey:=sSubKey)

        sDefSubKey = GISSharedConstants.ACOIMGISSubKey & "\" & "DEF" & "\" & "ListManagement"

        sSubKey = GISSharedConstants.ACOIMGISSubKey & "\" & v_sGISDataModel & "\" & "ListManagement"

        'UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        'UPGRADE_WARNING: Couldn't resolve default property of object vRegistryKeys. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        vRegistryKeys = New Object() {"ServerListFileCompressed", "ServerListFilePath", "ServerListPrefVersion", "ServerListVersion"}

        For Each sSettingItem In vRegistryKeys
            'Get the value
            'UPGRADE_WARNING: Couldn't resolve default property of object sSettingItem. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=sSettingItem, r_sSettingValue:=sTemp, v_sSubKey:=sDefSubKey)

            'Set the value
            'UPGRADE_WARNING: Couldn't resolve default property of object sSettingItem. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=sSettingItem, v_sSettingValue:=sTemp, v_sSubKey:=sSubKey)
        Next sSettingItem


        Exit Function

        Catch ex As Exception

        CreateRegistrySettings = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateRegistrySettings Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateRegistrySettings", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Exit Function

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
        Debug.Print(VB.Timer() & ": Entering " & ACApp & "." & ACClass & ".CopyDefaultGISLists")

        Try

        CopyDefaultGISLists = gPMConstants.PMEReturnCode.PMTrue

        Dim sPath As String
        Dim sDefSubKey As String
        Dim sOldFile As String
        Dim sNewFile As String

        sDefSubKey = GISSharedConstants.ACOIMGISSubKey & "\" & "DEF" & "\" & "ListManagement"

        'And let's not forget to copy the default file...
        m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="ServerListFilePath", r_sSettingValue:=sPath, v_sSubKey:=sDefSubKey)

        sPath = Trim(sPath)

        'MKW300603 PN3632 Start
        ' Fix to issue "Copying of default gis lists fails if registry
        ' setting is missing a backslash"
        If Len(sPath) > 0 And Right(sPath, 1) <> "\" Then
            sPath = sPath & "\"
        End If
        'MKW300603 PN3632 End

        sOldFile = sPath & "DEFList"
        sNewFile = sPath & v_sGISDataModel & "List"

        FileCopy(sOldFile & "0101.txt", sNewFile & "0101.txt")
        FileCopy(sOldFile & ".dat", sNewFile & ".dat")
        FileCopy(sOldFile & ".idx", sNewFile & ".idx")


        ' Debug message
        Debug.Print(VB.Timer() & ": Exiting " & ACApp & "." & ACClass & ".CopyDefaultGISLists")

        Exit Function

        Catch ex As Exception

        ' Debug message
        Debug.Print(VB.Timer() & ": Errored in " & ACApp & "." & ACClass & ".CopyDefaultGISLists")

        CopyDefaultGISLists = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyDefaultGISLists Failed (" & sOldFile & "0101.txt) to (" & sNewFile & "0101.txt)", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyDefaultGISLists", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Exit Function

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
    Private Function ProcessPropertyArray(ByVal v_lObjectType As Integer, ByVal v_sMainGroup As String, ByVal v_sProcedureName As String, ByVal v_lPMProductFamily As Integer, ByVal v_sDatamodel As String, ByVal v_sPrefix As String, ByRef r_vPropertyArray(,) As Object, ByRef r_sSelectList As String, ByRef r_sTableList As String, ByRef r_oDatabase As dPMDAO.Database, ByRef r_lParamCount As Integer, Optional ByVal v_sSubGroup As String = "", Optional ByVal v_sSubGroup1 As String = "", Optional ByVal v_sSubGroup2 As String = "", Optional ByVal v_sLoop1 As String = "", Optional ByVal v_sLoop2 As String = "", Optional ByVal v_sLoop3 As String = "", Optional ByRef r_sWhereList As String = "", Optional ByRef r_iInstanceCount As Short = 0, Optional ByVal v_lLevel As Integer = 0) As Integer

        Const sFunctionName As String = "ProcessPropertyArray"

        Dim llBound As Integer
        Dim lUbound As Integer
        Dim lProperty As Integer
        Dim lAddressCount As Integer
        Dim lOtherTableCount As Integer
        Dim vWPFields As Object
        Dim lWPField As Integer

        Dim sTemp As String
        Dim lTemp3 As Integer
        Dim sFieldName As String
        Dim sDisplayname As String
        Dim sSubGroupTemp As String
        Dim sSubGroup As String
        Dim sSubGroup2 As String
        Dim sSubGroup3 As String
        Dim lLevel As Integer
        Dim iParentIsMultipleInstance As Short
        Dim sLoop1 As String
        Dim sLoop2 As String
        Dim sLoop3 As String
        Dim sWhereList1 As String
        Dim sWhereList2 As String
        Dim iInstanceCount As Short
        Dim sColumnName As String


        ProcessPropertyArray = gPMConstants.PMEReturnCode.PMTrue

        '***************
        ' Initialisation
        '***************

        '********
        ' MEvans : 10-11-2003 : CQ3040
        If (v_lLevel > 1) Then
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
        If IsArray(r_vPropertyArray) Then

            llBound = LBound(r_vPropertyArray, 2)
            lUbound = UBound(r_vPropertyArray, 2)

            '*****************
            ' for each object property
            '*****************
            For lProperty = llBound To lUbound

                'UPGRADE_WARNING: Couldn't resolve default property of object r_vPropertyArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                sColumnName = Trim(r_vPropertyArray(ACPColumnName, lProperty))
                sFieldName = Trim(Replace(sColumnName, "_", ""))
                sDisplayname = Trim(Replace(sColumnName, "_", " "))

                '*****************
                ' if the property is an identity column / key
                '*****************
                'UPGRADE_WARNING: Couldn't resolve default property of object r_vPropertyArray(ACPIsIdentifyingProperty, lProperty). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If (r_vPropertyArray(ACPIsIdentifyingProperty, lProperty) = 1) Then

                    'Move the determination of the group structure to here - only
                    'do it once
                    sSubGroupTemp = v_sDatamodel
                    'UPGRADE_WARNING: Couldn't resolve default property of object StripDataModelCode(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    sTemp = Trim(StripDataModelCode(v_vTheString:=r_vPropertyArray(ACPColumnName, lProperty), v_sDatamodel:=v_sDatamodel))

                    'Remove the trailing _id
                    sTemp = Left(sTemp, Len(sTemp) - 3)

                    While InStr(sTemp, "_") <> 0
                        lTemp3 = InStr(sTemp, "_")
                        sSubGroupTemp = sSubGroupTemp & StrConv(Left(sTemp, lTemp3 - 1), VbStrConv.ProperCase)
                        sTemp = Mid(sTemp, lTemp3 + 1)
                    End While

                    Select Case lProperty

                        Case 0

                        Case 1
                            sSubGroup = sSubGroupTemp & StrConv(sTemp, VbStrConv.ProperCase)
                            'find if parent was actually a looping object
                            If lLevel = 0 Then
                                If iParentIsMultipleInstance = 1 And lLevel = 0 Then
                                    sLoop1 = sSubGroup
                                End If
                            End If

                        Case 2
                            sSubGroup2 = sSubGroupTemp & StrConv(sTemp, VbStrConv.ProperCase)
                            If (lLevel = 0) Then
                                If (iParentIsMultipleInstance = 1) Then
                                    sLoop2 = sSubGroup2
                                Else
                                    sLoop1 = sSubGroup2
                                End If
                            End If

                        Case 3
                            sSubGroup3 = sSubGroupTemp & StrConv(sTemp, VbStrConv.ProperCase)
                            'ED 30092002 - sLoop3 set t sSubGroup3 if parent is multi instance
                            'orignally overwriting sLoop2 with the new sub value.
                            If (lLevel = 0) Then
                                If (iParentIsMultipleInstance = 1) Then
                                    sLoop3 = sSubGroup3
                                Else
                                    sLoop2 = sSubGroup3
                                End If
                            End If

                    End Select

                    If (lProperty > lLevel) Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object r_vPropertyArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        sWhereList1 = sWhereList1 & "AND " & v_sPrefix & "." & r_vPropertyArray(ACPColumnName, lProperty) & " = @Instance" & lProperty + iParentIsMultipleInstance & vbCrLf
                        'UPGRADE_WARNING: Couldn't resolve default property of object r_vPropertyArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        sWhereList2 = sWhereList2 & "AND " & v_sPrefix & "." & r_vPropertyArray(ACPColumnName, lProperty) & " = @Instance" & lProperty + iParentIsMultipleInstance & vbCrLf
                        iInstanceCount = lProperty + iParentIsMultipleInstance
                    End If

                Else

                    '*****************
                    ' if the property is not an identity / key
                    ' and is not deleted
                    '*****************

                    'UPGRADE_WARNING: Couldn't resolve default property of object r_vPropertyArray(ACPPropertyName, lProperty). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If r_vPropertyArray(ACPPropertyName, lProperty) <> "dElEtEd" Then

                        '*****************
                        ' if we have a special type of field
                        ' like address / party / lookup / gis table link / etc
                        '*****************
                        ' if the property already exists in another non-gis related table then
                        If r_vPropertyArray(ACPEditFlags, lProperty) And GISDSEditNoDBColumn Then

                            ' do nothing because these fields
                            ' will already have there own entries in the
                            ' word processing fields so we dont want to
                            ' duplicate them

                        Else

                            'Special stuff to do here.

                            'If it's an address, need 6 lines of address and links to the address and country tables
                            'UPGRADE_WARNING: Couldn't resolve default property of object r_vPropertyArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            If (UCase(Left(r_vPropertyArray(ACPColumnName, lProperty), 11)) = "ADDRESS_CNT") Then

                                Call AddAddressProperty(v_vPropertyArray:=r_vPropertyArray, v_lItem:=lProperty, v_sPrefix:=v_sPrefix, r_sSelectList:=r_sSelectList, r_sTableList:=r_sTableList, r_lAddressCount:=lAddressCount, r_vWPFields:=vWPFields)

                                '*****************
                                ' else if its a party id then
                                ' add additional party fields to the stored procedure
                                '*****************

                                'UPGRADE_WARNING: Couldn't resolve default property of object r_vPropertyArray(ACPSpecialsType, lProperty). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            ElseIf r_vPropertyArray(ACPSpecialsType, lProperty) = ACOPartyTypeID Then
                                Call AddPartyProperty(v_vPropertyArray:=r_vPropertyArray, v_lItem:=lProperty, v_sPrefix:=v_sPrefix, r_sSelectList:=r_sSelectList, r_sTableList:=r_sTableList, r_lOtherTableCount:=lOtherTableCount, r_vWPFields:=vWPFields)

                                '*****************
                                ' else if its a lookup then
                                ' add additional lookup params to the stored procedure
                                '*****************

                                'UPGRADE_WARNING: Couldn't resolve default property of object r_vPropertyArray(ACPSpecialsType, lProperty). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            ElseIf r_vPropertyArray(ACPSpecialsType, lProperty) = ACOPMLookupTableName Then
                                Call AddLookupProperty(v_vPropertyArray:=r_vPropertyArray, v_lItem:=lProperty, v_sPrefix:=v_sPrefix, r_sSelectList:=r_sSelectList, r_sTableList:=r_sTableList, r_lOtherTableCount:=lOtherTableCount, r_vWPFields:=vWPFields)

                                '*****************
                                ' if its a link to another
                                ' gis table
                                '*****************

                                'UPGRADE_WARNING: Couldn't resolve default property of object r_vPropertyArray(ACPSpecialsType, lProperty). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            ElseIf r_vPropertyArray(ACPSpecialsType, lProperty) = ACOGISUserDefHeaderID Then
                                Call AddGisUserDefinedHeaderProperty(v_vPropertyArray:=r_vPropertyArray, v_lItem:=lProperty, v_sPrefix:=v_sPrefix, r_sSelectList:=r_sSelectList, r_sTableList:=r_sTableList, r_lOtherTableCount:=lOtherTableCount, r_vWPFields:=vWPFields)

                                '***********************
                                '  if its a product then link
                                ' in the product fields
                                '***********************

                                'UPGRADE_WARNING: Couldn't resolve default property of object r_vPropertyArray(ACPSpecialsType, lProperty). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            ElseIf r_vPropertyArray(ACPSpecialsType, lProperty) = ACOProductID Then
                                Call AddProductProperty(v_vPropertyArray:=r_vPropertyArray, v_lItem:=lProperty, v_sPrefix:=v_sPrefix, r_sSelectList:=r_sSelectList, r_sTableList:=r_sTableList, r_lOtherTableCount:=lOtherTableCount, r_vWPFields:=vWPFields)

                                '***********************
                                ' else it's just a normal property
                                '***********************
                            Else
                                Call AddGeneralProperty(v_vPropertyArray:=r_vPropertyArray, v_lItem:=lProperty, v_sPrefix:=v_sPrefix, r_sSelectList:=r_sSelectList, r_sTableList:=r_sTableList, r_vWPFields:=vWPFields)

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
        If IsArray(vWPFields) Then

            ' get array boundaries
            llBound = LBound(vWPFields, 2)
            lUbound = UBound(vWPFields, 2)

            r_lParamCount = lUbound + 1

            ' for each field
            For lWPField = llBound To lUbound

                ' add an entry in the wp fields table
                'UPGRADE_WARNING: Couldn't resolve default property of object vWPFields(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
                If AddToWPFields(sFieldName:=CStr(vWPFields(0, lWPField)), sSQL:=v_sProcedureName, sColumnName:=CStr(vWPFields(1, lWPField)), lColumnType:=CInt(vWPFields(2, lWPField)), sMainGroup:=v_sMainGroup, sSubGroup:=v_sSubGroup, sSubGroup2:=v_sSubGroup1, sSubGroup3:=v_sSubGroup2, sSubGroup4:="", sDisplayname:=CStr(vWPFields(3, lWPField)), iIsDisplayed:=1, sLoop1:=sLoop1, sLoop2:=sLoop2, sLoop3:=sLoop3, sLoop4:="", lProductFamily:=v_lPMProductFamily, vDataModel:=System.DBNull.Value, vPropertyId:=System.DBNull.Value, r_oDatabase:=r_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then

                    ProcessPropertyArray = gPMConstants.PMEReturnCode.PMFalse
                    Exit For

                End If

            Next

        End If

        '********
        ' MEvans : 08-11-2003 : CQ3049
        r_sWhereList = sWhereList1
        r_iInstanceCount = iInstanceCount
        '********

        Exit Function

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

        Const sFunctionName As String = "AddAddressProperty"

        Dim lAddressLine As Integer
        Dim sColumnName As String
        Dim sPropertyName As String


        AddAddressProperty = gPMConstants.PMEReturnCode.PMTrue

        '**************
        ' Increment the address counter
        ' to get unique address fields and link table name
        '**************
        r_lAddressCount = r_lAddressCount + 1

        '*****************
        ' Build property and table strings
        '*****************

        ' get array values
        'UPGRADE_WARNING: Couldn't resolve default property of object v_vPropertyArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        sColumnName = Trim(v_vPropertyArray(ACPColumnName, v_lItem))
        'UPGRADE_WARNING: Couldn't resolve default property of object v_vPropertyArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        sPropertyName = Trim(v_vPropertyArray(ACPPropertyName, v_lItem))

        r_sSelectList = r_sSelectList & "address" & r_lAddressCount & "_line_1 = a" & r_lAddressCount & ".address1," & vbCrLf & "address" & r_lAddressCount & "_line_2 = a" & r_lAddressCount & ".address2," & vbCrLf & "address" & r_lAddressCount & "_line_3 = a" & r_lAddressCount & ".address3," & vbCrLf & "address" & r_lAddressCount & "_line_4 = a" & r_lAddressCount & ".address4," & vbCrLf & "case a" & r_lAddressCount & ".postal_code when convert(varchar(10), a" & r_lAddressCount & ".address_id) then '' else a" & r_lAddressCount & ".postal_code end as address" & r_lAddressCount & "_postal_code," & vbCrLf & "address" & r_lAddressCount & "_country = c" & r_lAddressCount & ".description," & vbCrLf

        r_sTableList = r_sTableList & "LEFT JOIN address a" & r_lAddressCount & " ON " & v_sPrefix & "." & sColumnName & " = a" & r_lAddressCount & ".address_cnt" & vbCrLf

        r_sTableList = r_sTableList & "LEFT JOIN country c" & r_lAddressCount & " ON " & "a" & r_lAddressCount & ".country_id = c" & r_lAddressCount & ".country_id" & vbCrLf

        '**************
        ' WP Field Address Lines
        '**************
        For lAddressLine = 1 To 4
            If AddPropertyToWPFieldsArray(r_vWPFieldArray:=r_vWPFields, v_sFieldName:=v_sPrefix & "Address" & r_lAddressCount & "Line" & lAddressLine, v_sColumnName:="address" & r_lAddressCount & "_line_" & lAddressLine, v_lDataType:=gPMConstants.PMEFormatStyle.PMFormatString, v_sDisplayName:=sPropertyName & " Line " & lAddressLine) <> gPMConstants.PMEReturnCode.PMTrue Then

                AddAddressProperty = gPMConstants.PMEReturnCode.PMFalse
                Exit For
            End If
        Next

        '**************
        ' WP Field Postal Code
        '**************
        If AddPropertyToWPFieldsArray(r_vWPFieldArray:=r_vWPFields, v_sFieldName:=v_sPrefix & "Address" & r_lAddressCount & "PostalCode", v_sColumnName:="address" & r_lAddressCount & "_postal_code", v_lDataType:=gPMConstants.PMEFormatStyle.PMFormatString, v_sDisplayName:=sPropertyName & " Postal Code") <> gPMConstants.PMEReturnCode.PMTrue Then

            AddAddressProperty = gPMConstants.PMEReturnCode.PMFalse

        End If

        '**************
        ' WP Field Country
        '**************
        If AddPropertyToWPFieldsArray(r_vWPFieldArray:=r_vWPFields, v_sFieldName:=v_sPrefix & "Address" & r_lAddressCount & "Country", v_sColumnName:="address" & r_lAddressCount & "_country", v_lDataType:=gPMConstants.PMEFormatStyle.PMFormatString, v_sDisplayName:=sPropertyName & " Country") <> gPMConstants.PMEReturnCode.PMTrue Then

            AddAddressProperty = gPMConstants.PMEReturnCode.PMFalse
        End If

        Exit Function

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

        Const sFunctionName As String = "AddPartyProperty"


        Dim sFieldName As String
        Dim sDisplayname As String
        Dim sColumnName As String


        AddPartyProperty = gPMConstants.PMEReturnCode.PMTrue

        '**************
        ' Increment the Other Table Count
        ' to get unique fields and link table name
        '**************
        r_lOtherTableCount = r_lOtherTableCount + 1

        '*****************
        ' Generate the property strings
        '*****************

        'UPGRADE_WARNING: Couldn't resolve default property of object v_vPropertyArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        sColumnName = Trim(v_vPropertyArray(ACPColumnName, v_lItem))
        sFieldName = Replace(sColumnName, "_", "")
        sDisplayname = Replace(sColumnName, "_", " ")

        '*****************
        ' Build property and table strings
        '*****************
        r_sSelectList = r_sSelectList & sColumnName & " = p" & r_lOtherTableCount & ".name," & vbCrLf

        r_sTableList = r_sTableList & "LEFT JOIN party p" & r_lOtherTableCount & " ON " & v_sPrefix & "." & sColumnName & " = p" & r_lOtherTableCount & ".party_cnt" & vbCrLf


        '*****************
        ' add property to the wp field array
        '*****************
        If AddPropertyToWPFieldsArray(r_vWPFieldArray:=r_vWPFields, v_sFieldName:=v_sPrefix & sFieldName, v_sColumnName:=sColumnName, v_lDataType:=gPMConstants.PMEFormatStyle.PMFormatString, v_sDisplayName:=sDisplayname) <> gPMConstants.PMEReturnCode.PMTrue Then

            AddPartyProperty = gPMConstants.PMEReturnCode.PMFalse
        End If

        Exit Function

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

        Const sFunctionName As String = "AddLookupProperty"

        Dim sReference As String
        Dim sFieldName As String
        Dim sDisplayname As String
        Dim sColumnName As String


        AddLookupProperty = gPMConstants.PMEReturnCode.PMTrue

        '**************
        ' Increment the Other Table Count
        ' to get unique fields and link table name
        '**************
        r_lOtherTableCount = r_lOtherTableCount + 1

        '*****************
        ' Generate the property strings
        '*****************

        'UPGRADE_WARNING: Couldn't resolve default property of object v_vPropertyArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        sColumnName = Trim(v_vPropertyArray(ACPColumnName, v_lItem))
        sFieldName = Replace(sColumnName, "_", "")
        sDisplayname = Replace(sColumnName, "_", " ")
        'UPGRADE_WARNING: Couldn't resolve default property of object v_vPropertyArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        sReference = Trim(v_vPropertyArray(ACPSpecialsTypeReference, v_lItem))

        '*****************
        ' Build property and table strings
        '*****************

        r_sSelectList = r_sSelectList & sColumnName & " = l" & r_lOtherTableCount & ".description," & vbCrLf

        r_sTableList = r_sTableList & "LEFT JOIN " & sReference & " l" & r_lOtherTableCount & " ON " & v_sPrefix & "." & sColumnName & " = " & "l" & r_lOtherTableCount & "." & sReference & "_id" & vbCrLf

        '*****************
        ' add property to the wp field array
        '*****************
        If AddPropertyToWPFieldsArray(r_vWPFieldArray:=r_vWPFields, v_sFieldName:=v_sPrefix & sFieldName, v_sColumnName:=sColumnName, v_lDataType:=gPMConstants.PMEFormatStyle.PMFormatString, v_sDisplayName:=sDisplayname) <> gPMConstants.PMEReturnCode.PMTrue Then
            AddLookupProperty = gPMConstants.PMEReturnCode.PMFalse

        End If

        Exit Function

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
    Private Function AddGisUserDefinedHeaderProperty(ByVal v_vPropertyArray As Object, ByVal v_lItem As Integer, ByVal v_sPrefix As String, ByRef r_sSelectList As String, ByRef r_sTableList As String, ByRef r_lOtherTableCount As Integer, ByRef r_vWPFields As Object) As Integer

        Const sFunctionName As String = "AddGisUserDefinedHeaderProperty"

        Dim sColumnName As String
        Dim sFieldName As String
        Dim sDisplayname As String
        Dim sReference As String


        AddGisUserDefinedHeaderProperty = gPMConstants.PMEReturnCode.PMTrue

        '**************
        ' Increment the Other Table Count
        ' to get unique fields and link table name
        '**************
        r_lOtherTableCount = r_lOtherTableCount + 1

        '*****************
        ' Generate the property strings
        '*****************

        'UPGRADE_WARNING: Couldn't resolve default property of object v_vPropertyArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        sColumnName = Trim(v_vPropertyArray(ACPColumnName, v_lItem))
        sFieldName = Replace(sColumnName, "_", "")
        sDisplayname = Replace(sColumnName, "_", " ")
        'UPGRADE_WARNING: Couldn't resolve default property of object v_vPropertyArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        sReference = Trim(v_vPropertyArray(ACPSpecialsTypeReference, v_lItem))

        '*****************
        ' Build property and table strings
        '*****************
        r_sSelectList = r_sSelectList & sColumnName & " = l" & r_lOtherTableCount & ".description," & vbCrLf

        r_sTableList = r_sTableList & "LEFT JOIN GIS_user_def_detail l" & r_lOtherTableCount & " ON " & v_sPrefix & "." & sColumnName & " = " & "l" & r_lOtherTableCount & ".GIS_user_def_detail_id" & vbCrLf

        '*****************
        ' add property to the wp field array
        '*****************
        If AddPropertyToWPFieldsArray(r_vWPFieldArray:=r_vWPFields, v_sFieldName:=v_sPrefix & sFieldName, v_sColumnName:=sColumnName, v_lDataType:=gPMConstants.PMEFormatStyle.PMFormatString, v_sDisplayName:=sDisplayname) <> gPMConstants.PMEReturnCode.PMTrue Then

            AddGisUserDefinedHeaderProperty = gPMConstants.PMEReturnCode.PMFalse

        End If

        Exit Function

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

        Const sFunctionName As String = "AddProductProperty"

        Dim sFieldName As String
        Dim sDisplayname As String
        Dim sColumnName As String
        Dim sReference As String


        AddProductProperty = gPMConstants.PMEReturnCode.PMTrue

        '**************
        ' Increment the Other Table Count
        ' to get unique fields and link table name
        '**************
        r_lOtherTableCount = r_lOtherTableCount + 1

        '*****************
        ' Generate the property strings
        '*****************
        'UPGRADE_WARNING: Couldn't resolve default property of object v_vPropertyArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        sColumnName = Trim(v_vPropertyArray(ACPColumnName, v_lItem))
        sFieldName = Replace(sColumnName, "_", "")
        sDisplayname = Replace(sColumnName, "_", " ")
        'UPGRADE_WARNING: Couldn't resolve default property of object v_vPropertyArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        sReference = Trim(v_vPropertyArray(ACPSpecialsTypeReference, v_lItem))

        '*****************
        ' Build property and table strings
        '*****************
        r_sSelectList = r_sSelectList & sColumnName & " = i" & r_lOtherTableCount & ".insurance_ref," & vbCrLf

        r_sTableList = r_sTableList & "LEFT JOIN insurance_file i" & r_lOtherTableCount & " ON " & v_sPrefix & "." & sColumnName & " = i" & r_lOtherTableCount & ".insurance_file_cnt" & vbCrLf


        '*****************
        ' add property to the wp field array
        '*****************
        If AddPropertyToWPFieldsArray(r_vWPFieldArray:=r_vWPFields, v_sFieldName:=v_sPrefix & sFieldName, v_sColumnName:=sColumnName, v_lDataType:=gPMConstants.PMEFormatStyle.PMFormatString, v_sDisplayName:=sDisplayname) <> gPMConstants.PMEReturnCode.PMTrue Then

            AddProductProperty = gPMConstants.PMEReturnCode.PMFalse
        End If

        Exit Function

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

        Const sFunctionName As String = "AddGeneralProperty"

        Dim sDataType As String
        Dim sColumnName As String
        Dim lFormat As Integer
        Dim sFieldName As String
        Dim sDisplayname As String


        AddGeneralProperty = gPMConstants.PMEReturnCode.PMTrue

        '*****************
        ' Generate the property strings
        '*****************
        'UPGRADE_WARNING: Couldn't resolve default property of object v_vPropertyArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        sColumnName = Trim(v_vPropertyArray(ACPColumnName, v_lItem))
        sFieldName = Replace(sColumnName, "_", "")
        sDisplayname = Replace(sColumnName, "_", " ")
        'UPGRADE_WARNING: Couldn't resolve default property of object v_vPropertyArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        sDataType = Trim(v_vPropertyArray(ACPDataType, v_lItem))

        r_sSelectList = r_sSelectList & sColumnName & " = " & v_sPrefix & "." & sColumnName & "," & vbCrLf

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

            AddGeneralProperty = gPMConstants.PMEReturnCode.PMFalse

        End If

        Exit Function

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

        Const sFunctionName As String = "AddPropertyToWPFieldsArray"

        Dim lWPFieldLastPos As Integer
        Dim lArrayPos As Integer


        AddPropertyToWPFieldsArray = gPMConstants.PMEReturnCode.PMTrue

        '**************
        ' Resize Array
        '**************
        If IsArray(r_vWPFieldArray) Then
            lWPFieldLastPos = UBound(r_vWPFieldArray, 2)
            ReDim Preserve r_vWPFieldArray(3, (lWPFieldLastPos + 1))
        Else
            'UPGRADE_WARNING: Lower bound of array r_vWPFieldArray was changed from ACWPFFieldName,0 to 0,0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
            ReDim r_vWPFieldArray(ACWPFDisplayName, 0)
            lWPFieldLastPos = -1
        End If

        '**************
        ' WP Field Address Lines
        '**************
        lArrayPos = lWPFieldLastPos + 1

        'UPGRADE_WARNING: Couldn't resolve default property of object r_vWPFieldArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        r_vWPFieldArray(ACWPFFieldName, lArrayPos) = v_sFieldName
        'UPGRADE_WARNING: Couldn't resolve default property of object r_vWPFieldArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        r_vWPFieldArray(ACWPFColumnName, lArrayPos) = v_sColumnName
        'UPGRADE_WARNING: Couldn't resolve default property of object r_vWPFieldArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        r_vWPFieldArray(ACWPFColumnType, lArrayPos) = v_lDataType
        'UPGRADE_WARNING: Couldn't resolve default property of object r_vWPFieldArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        r_vWPFieldArray(ACWPFDisplayName, lArrayPos) = v_sDisplayName

        Exit Function

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

        Dim sSelectList As String
        Dim sSQL As String


        GenerateClaimPerilGetKeysStoredProcedure = gPMConstants.PMEReturnCode.PMTrue

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

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateClaimPerilGetKeysStoredProcedure = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        '***********************
        'Create the new procedure
        '***********************
        sSQL = ""

        sSQL = sSQL & "CREATE PROCEDURE " & sProcedureName & "_get_keys" & vbCrLf & "@PartyCnt INT," & vbCrLf & "@InsuranceFileCnt INT," & vbCrLf & "@RiskId INT = NULL," & vbCrLf & "@ClaimCnt INT," & vbCrLf & "@DocumentRef VARCHAR(25)," & vbCrLf & "@Instance1 INT = NULL," & vbCrLf & "@Instance2 INT = NULL," & vbCrLf & "@Instance3 INT = NULL," & vbCrLf & "@ClaimPerilId INT = NULL" & vbCrLf & "AS"

        sSelectList = "If @ClaimPerilId IS NULL" & vbCrLf & "SELECT Claim_Peril_Id from " & sTableName & " WHERE Claim_Id = @ClaimCnt" & vbCrLf & "Else " & vbCrLf & "SELECT Claim_Peril_Id from " & sTableName & " WHERE Claim_Peril_Id = @ClaimPerilId"

        ' combine sql statements to fully generate the procedure script
        sSQL = sSQL & vbCrLf & sSelectList & vbCrLf

        ' create the stored procedure on the server
        m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & sProcedureName & "_get_keys StoredProcedure", bStoredProcedure:=False)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateClaimPerilGetKeysStoredProcedure = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        'Set permissions
        sSQL = "GRANT EXECUTE ON dbo." & sProcedureName & "_get_keys TO PUBLIC" & vbCrLf

        m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Grant " & sProcedureName & "_get_keys StoredProcedure", bStoredProcedure:=False)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateClaimPerilGetKeysStoredProcedure = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        Exit Function

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

        Dim sSelectList As String
        Dim sSQL As String


        GenerateClaimPerilGetParentStoredProcedure = gPMConstants.PMEReturnCode.PMTrue

        ' for some bizarre reason you have to set the return value to
        ' true for the drop procedures to do anything
        m_lReturn = gPMConstants.PMEReturnCode.PMTrue

        'Drop it if it's already there
        DropExistingProcedure(v_sProcedureName:=sProcedureName, v_sPostFix:="_get_parent_key", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

        ' for some bizarre reason you have to set the return value to
        ' true for the drop procedures to do anything
        m_lReturn = gPMConstants.PMEReturnCode.PMTrue

        DropExistingProcedure(v_sProcedureName:=sProcedureNameOldStyle, v_sPostFix:="_get_parent_key", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateClaimPerilGetParentStoredProcedure = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        'Create the new procedure
        sSQL = ""

        sSQL = sSQL & "CREATE PROCEDURE " & sProcedureName & "_get_parent_key @PartyCnt INT," & vbCrLf & "@InsuranceFileCnt INT," & vbCrLf & "@RiskId INT = NULL," & vbCrLf & "@ClaimCnt INT," & vbCrLf & "@DocumentRef VARCHAR(25)," & vbCrLf & "@Instance1 INT," & vbCrLf & "@Instance2 INT," & vbCrLf & "@Instance3 INT" & vbCrLf & "AS" & vbCrLf & vbCrLf

        'First we do the main query
        sSelectList = "SELECT @ClaimCnt"

        sSQL = sSQL & sSelectList & vbCrLf

        m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & sProcedureName & "_get_parent_key StoredProcedure", bStoredProcedure:=False)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateClaimPerilGetParentStoredProcedure = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        'Set permissions

        sSQL = "GRANT EXECUTE ON dbo." & sProcedureName & "_get_parent_key TO PUBLIC" & vbCrLf

        m_lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Grant " & sProcedureName & "_get_parent_key StoredProcedure", bStoredProcedure:=False)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GenerateClaimPerilGetParentStoredProcedure = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        Exit Function

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
    Private Function GenerateClaimRiskGetKeysSP(ByVal v_sTableName As String, ByVal v_sPrefix As String, ByVal v_sDatamodel As String, ByVal v_sProcedureName As String, ByVal v_sProcedureNameOldStyle As String, ByVal v_vArray(,) As Object, ByVal v_iIsParentMultipleInstance As Short, ByRef r_oDatabase As dPMDAO.Database) As Integer

        Const sFunctionName As String = "GenerateClaimRiskGetKeysSP"

        Dim sSelectList As String
        Dim sTableList As String
        Dim sWhereList As String
        Dim lLevel As Integer
        Dim sSQL As String

        Dim lIDKeyPos As Integer
        Dim llBound As Integer
        Dim lUbound As Integer


        GenerateClaimRiskGetKeysSP = gPMConstants.PMEReturnCode.PMTrue

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

        sWhereList = "WHERE XXXDATAMODELNAME_policy_binder_id in (" & " SELECT XXXDATAMODELNAME_policy_binder_id from " & " XXXDATAMODELNAME_Policy_Binder where gis_policy_link_id in ( " & " Select gis_policy_link_id from gis_Policy_Link " & "Where Claim_Id =@ClaimCnt))"


        'Initialise id key level indicator
        lLevel = 0

        ' get the position of the last identifying key in the array
        ' the unique key of the current table
        If IsArray(v_vArray) Then

            llBound = LBound(v_vArray, 2)
            lUbound = UBound(v_vArray, 2)

            For lIDKeyPos = llBound To lUbound
                'UPGRADE_WARNING: Couldn't resolve default property of object v_vArray(ACPIsIdentifyingProperty, lIDKeyPos). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If (v_vArray(ACPIsIdentifyingProperty, lIDKeyPos) = 1) Then
                    lLevel = lIDKeyPos
                End If
            Next lIDKeyPos

            ' add the local level key to the select list and any other keys to the
            ' where clause.
            ' start from lbound + 1 to exclude the initial policy binder id as this is already
            ' added to the initial "where" clause
            For lIDKeyPos = llBound + 1 To lUbound

                ' if item is id key
                'UPGRADE_WARNING: Couldn't resolve default property of object v_vArray(ACPIsIdentifyingProperty, lIDKeyPos). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If (v_vArray(ACPIsIdentifyingProperty, lIDKeyPos) = 1) Then

                    ' if its not the local table's unique id key
                    If (lIDKeyPos < lLevel) Then
                        ' add it to the where list
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_vArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        sWhereList = sWhereList & "AND XXXDATAMODELTABLEALIAS." & v_vArray(ACPColumnName, lIDKeyPos) & " = @Instance" & lIDKeyPos + v_iIsParentMultipleInstance & vbCrLf
                    Else
                        ' else add the local tables unique id key to the select list
                        ' as this is what we want to return
                        'UPGRADE_WARNING: Couldn't resolve default property of object v_vArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        sSelectList = "SELECT XXXDATAMODELTABLEALIAS." & v_vArray(ACPColumnName, lIDKeyPos)
                    End If
                End If

            Next lIDKeyPos

        End If

        ' build the full select list
        sSelectList = sSelectList & vbCrLf & sTableList & vbCrLf & sWhereList

        ' update template sql to contain relevant datamodel, table, prefixes
        sSelectList = Replace(sSelectList, ACSPGenDataTempDataModelName, v_sDatamodel, 1)
        sSelectList = Replace(sSelectList, ACSPGenDataTempDataModelTableName, v_sTableName, 1)
        sSelectList = Replace(sSelectList, ACSPGenDataTempDataModelTableAlias, v_sPrefix, 1)

        'Create the new procedure
        sSQL = "CREATE PROCEDURE " & v_sProcedureName & "_get_keys " & vbCrLf & "@PartyCnt INT," & vbCrLf & "@InsuranceFileCnt INT," & vbCrLf & "@RiskId INT = NULL," & vbCrLf & "@ClaimCnt INT," & vbCrLf & "@DocumentRef VARCHAR(25)"

        ' dynamically generate instance variables
        ' the number of instance variables is determined by how far down the
        ' tree structure the object is..
        generateInstanceVariables(sSQL, lLevel)

        ' combine the sql statments to produce the stored procedure sql
        sSQL = sSQL & vbCrLf & "AS" & vbCrLf & vbCrLf & sSelectList & vbCrLf

        ' add stored procedure to database
        If r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & v_sProcedureName & "_get_keys StoredProcedure", bStoredProcedure:=False) = gPMConstants.PMEReturnCode.PMTrue Then

            'Set permissions on stored procedure
            sSQL = "GRANT EXECUTE ON dbo." & v_sProcedureName & "_get_keys TO PUBLIC" & vbCrLf

            If r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Grant " & v_sProcedureName & "_get_keys StoredProcedure", bStoredProcedure:=False) <> gPMConstants.PMEReturnCode.PMTrue Then

                ' log error
                GenerateClaimRiskGetKeysSP = gPMConstants.PMEReturnCode.PMFalse

                gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to set default permissions for " & " stored procedure:" & v_sProcedureName, vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)

            End If

        Else
            ' log error
            GenerateClaimRiskGetKeysSP = gPMConstants.PMEReturnCode.PMFalse

            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to create stored procedure:" & sSQL, vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)

        End If

        Exit Function

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

        Const sFunctionName As String = "GenerateClaimRiskGetParentKeysSP"

        Dim sSelectList As String
        Dim sTableList As String
        Dim sWhereList As String
        Dim sSQL As String


        GenerateClaimRiskGetParentKeysSP = gPMConstants.PMEReturnCode.PMTrue

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

        If (m_lReturn = gPMConstants.PMEReturnCode.PMTrue) Then

            'Build up the template for the query
            'UPGRADE_WARNING: Couldn't resolve default property of object v_vArray(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            sSelectList = "SELECT DISTINCT XXXDATAMODELTABLEALIAS." & v_vArray(ACPColumnName, 1)

            sTableList = "FROM XXXDATAMODELTABLENAME as XXXDATAMODELTABLEALIAS"

            sWhereList = "WHERE XXXDATAMODELNAME_policy_binder_id in (" & " SELECT XXXDATAMODELNAME_policy_binder_id from " & " XXXDATAMODELNAME_Policy_Binder where gis_policy_link_id in ( " & " Select gis_policy_link_id from gis_Policy_Link " & "Where Claim_Id =@ClaimCnt))"

            ' build the full select list
            sSelectList = sSelectList & vbCrLf & sTableList & vbCrLf & sWhereList

            ' update template sql to contain relevant datamodel, table, prefixes
            sSelectList = Replace(sSelectList, ACSPGenDataTempDataModelName, v_sDatamodel, 1)
            sSelectList = Replace(sSelectList, ACSPGenDataTempDataModelTableName, v_sTableName, 1)
            sSelectList = Replace(sSelectList, ACSPGenDataTempDataModelTableAlias, v_sPrefix, 1)

            'Create the new procedure
            sSQL = ""

            sSQL = sSQL & "CREATE PROCEDURE " & v_sProcedureName & "_get_parent_key @PartyCnt INT," & vbCrLf & "@InsuranceFileCnt INT," & vbCrLf & "@RiskId INT = NULL," & vbCrLf & "@ClaimCnt INT," & vbCrLf & "@DocumentRef VARCHAR(25)," & vbCrLf & "@Instance1 INT," & vbCrLf & "@Instance2 INT," & vbCrLf & "@Instance3 INT" & vbCrLf & "AS" & vbCrLf & vbCrLf

            sSQL = sSQL & sSelectList & vbCrLf

            If r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & v_sProcedureName & "_get_parent_key StoredProcedure", bStoredProcedure:=False) = gPMConstants.PMEReturnCode.PMTrue Then
                'Set permissions
                sSQL = "GRANT EXECUTE ON dbo." & v_sProcedureName & "_get_parent_key TO PUBLIC" & vbCrLf

                If r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Grant " & v_sProcedureName & "_get_parent_key StoredProcedure", bStoredProcedure:=False) <> gPMConstants.PMEReturnCode.PMTrue Then

                    ' log error
                    GenerateClaimRiskGetParentKeysSP = gPMConstants.PMEReturnCode.PMFalse

                    gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to set default permissions for " & " stored procedure:" & v_sProcedureName, vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)

                End If

            Else

                ' log error
                GenerateClaimRiskGetParentKeysSP = gPMConstants.PMEReturnCode.PMFalse

                gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to create stored procedure:" & sSQL, vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)

            End If
        Else
            ' log error
            GenerateClaimRiskGetParentKeysSP = gPMConstants.PMEReturnCode.PMFalse

            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to drop stored procedure:" & v_sProcedureName & " OR " & v_sProcedureNameOldStyle, vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)

        End If

        Exit Function

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
    Private Function GenerateClaimRiskSP(ByVal v_lObjectId As Integer, ByVal v_vGISObject As Object, ByVal v_vGISProperty As Object, ByVal v_lParentId As Integer, ByVal v_sDatamodel As String, ByVal v_lPMProductFamily As Integer, ByRef r_oDatabase As Object) As Integer


        Dim lTemp As Integer
        Dim sProcedureName As String
        Dim sProcedureNameOldStyle As String
        Dim iParentIsMultipleInstance As Short
        Dim sTemp As String
        Dim sPrefix As String
        Dim lParamCount As Integer

        Dim sSelectList As String
        Dim sTableList As String
        Dim sWhereList As String
        Dim vArray(,) As Object
        Dim sAddToWhereList As String
        Dim sSQL As String
        Dim sTableName As String
        Dim iInstanceCount As Short
        Dim lIDKeyPos As Short
        Dim llBound As Integer
        Dim lUbound As Integer
        Dim lLevel As Integer

        Const sFunctionName As String = "GenerateClaimRiskSP"


        GenerateClaimRiskSP = gPMConstants.PMEReturnCode.PMTrue

        ' if not the parent object
        'UPGRADE_WARNING: Couldn't resolve default property of object v_vGISObject(ACOGISObjectId, v_lObjectId). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If (v_vGISObject(ACOGISObjectId, v_lObjectId) <> v_lParentId) Then

            '*****************************
            ' determine the old and new format of the stored procedures
            '*****************************
            'UPGRADE_WARNING: Couldn't resolve default property of object v_vGISObject(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            sProcedureName = LCase(v_vGISObject(ACOObjectName, v_lObjectId))

            m_lReturn = GetProcedureName(r_sProcedureName:=sProcedureName, v_sDatamodel:=v_sDatamodel)

            sProcedureNameOldStyle = "sp_wp_" & v_sDatamodel & sProcedureName
            sProcedureName = "spg_wp_" & v_sDatamodel & sProcedureName

            'UPGRADE_WARNING: Couldn't resolve default property of object v_vGISObject(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iParentIsMultipleInstance = IsTopLevelParentObjectMultipleInstance(v_vGISObject(ACOObjectName, v_lObjectId), v_vGISObject)

            'UPGRADE_WARNING: Couldn't resolve default property of object v_vGISObject(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            sTableName = Trim(v_vGISObject(ACOTableName, v_lObjectId))

            '******************
            ' determine prefix (alias) for table name
            '******************
            'UPGRADE_WARNING: Couldn't resolve default property of object StripDataModelCode(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            sTemp = StripDataModelCode(v_vTheString:=sTableName, v_sDatamodel:=v_sDatamodel)

            sTemp = Mid(sTemp, InStr(sTemp, "_") + 1)
            sPrefix = v_sDatamodel
            sPrefix = sPrefix & UCase(Left(sTemp, 1))

            '*****************
            ' Build SQL for Create Procedure
            '*****************

            ' intialise variables
            lParamCount = 0
            sSelectList = ""
            sTableList = "FROM " & sTableName & " " & sPrefix & vbCrLf

            ' Get and use property array for the current object
            'UPGRADE_WARNING: Couldn't resolve default property of object v_vGISProperty(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Couldn't resolve default property of object vArray. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            vArray = v_vGISProperty(v_lObjectId)

            ' determine the level of the object
            ' as this will determine what keys stored procedures if any
            ' need to be created.
            If IsArray(vArray) Then

                llBound = LBound(vArray, 2)
                lUbound = UBound(vArray, 2)

                For lIDKeyPos = llBound To lUbound
                    'UPGRADE_WARNING: Couldn't resolve default property of object vArray(ACPIsIdentifyingProperty, lIDKeyPos). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If (vArray(ACPIsIdentifyingProperty, lIDKeyPos) = 1) Then
                        lLevel = lIDKeyPos
                    End If
                Next lIDKeyPos
            End If

            '************************
            ' Generate Get Keys Stored Procedure
            '************************
            'UPGRADE_WARNING: Couldn't resolve default property of object v_vGISObject(ACOMaxInstances, lTemp). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If (lLevel > 1 Or v_vGISObject(ACOMaxInstances, lTemp) > 1) Then

                ' Use the Live claim peril table to get the keys from
                'UPGRADE_WARNING: Couldn't resolve default property of object r_oDatabase. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                m_lReturn = GenerateClaimRiskGetKeysSP(v_sTableName:=sTableName, v_sPrefix:=sPrefix, v_sDatamodel:=v_sDatamodel, v_sProcedureName:=sProcedureName, v_sProcedureNameOldStyle:=sProcedureNameOldStyle, v_vArray:=vArray, v_iIsParentMultipleInstance:=iParentIsMultipleInstance, r_oDatabase:=r_oDatabase)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    GenerateClaimRiskSP = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
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
            'UPGRADE_WARNING: Couldn't resolve default property of object v_vGISObject(ACOMaxInstances, lTemp). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If lLevel >= 2 Or (v_vGISObject(ACOMaxInstances, lTemp) > 1) Then
                'UPGRADE_WARNING: Couldn't resolve default property of object r_oDatabase. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                m_lReturn = GenerateClaimRiskGetParentKeysSP(v_sTableName:=sTableName, v_sPrefix:=sPrefix, v_sDatamodel:=v_sDatamodel, v_sProcedureName:=sProcedureName, v_sProcedureNameOldStyle:=sProcedureNameOldStyle, v_vArray:=vArray, r_oDatabase:=r_oDatabase)


                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    GenerateClaimRiskSP = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If
            End If

            '*****************
            ' Build SQL for Create Procedure
            '*****************

            ' intialise variables
            lParamCount = 0
            sSelectList = ""
            sAddToWhereList = ""
            sTableList = "FROM " & sTableName & " " & sPrefix & vbCrLf

            ' Get and use property array for the current object
            'UPGRADE_WARNING: Couldn't resolve default property of object v_vGISProperty(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Couldn't resolve default property of object vArray. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            vArray = v_vGISProperty(v_lObjectId)

            'UPGRADE_WARNING: Couldn't resolve default property of object r_oDatabase. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If ProcessPropertyArray(v_lObjectType:=GISOTRisk, r_vPropertyArray:=vArray, r_sSelectList:=sSelectList, r_sTableList:=sTableList, v_sMainGroup:="Claim", v_sProcedureName:=sProcedureName, v_lPMProductFamily:=v_lPMProductFamily, v_sDatamodel:=v_sDatamodel, r_oDatabase:=r_oDatabase, r_lParamCount:=lParamCount, v_sPrefix:=sPrefix, v_sSubGroup:="", v_sLoop1:="", r_sWhereList:=sAddToWhereList, r_iInstanceCount:=iInstanceCount, v_lLevel:=lLevel) = gPMConstants.PMEReturnCode.PMTrue Then

                If (lParamCount <> 0) Then

                    'Add the rest...
                    sTableList = Left(sTableList, Len(sTableList) - 2)

                    'sWhereList1 = "WHERE Claim_Peril_id = @Instance2"
                    sWhereList = "WHERE XXXDATAMODELNAME_policy_binder_id in (" & " SELECT XXXDATAMODELNAME_policy_binder_id from " & " XXXDATAMODELNAME_Policy_Binder where gis_policy_link_id in ( " & " Select gis_policy_link_id from gis_Policy_Link " & "Where Claim_Id =@ClaimCnt))"
                    ' for some bizarre reason you have to set the return value to
                    ' true for the drop procedures to do anything
                    m_lReturn = gPMConstants.PMEReturnCode.PMTrue

                    'Drop it if it's already there
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_oDatabase. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    DropExistingProcedure(v_sProcedureName:=sProcedureName, v_sPostFix:="", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

                    ' for some bizarre reason you have to set the return value to
                    ' true for the drop procedures to do anything
                    m_lReturn = gPMConstants.PMEReturnCode.PMTrue

                    'UPGRADE_WARNING: Couldn't resolve default property of object r_oDatabase. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    DropExistingProcedure(v_sProcedureName:=sProcedureNameOldStyle, v_sPostFix:="", r_lRetval:=m_lReturn, r_oDatabase:=r_oDatabase)

                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                        'remove the last comma and vbcrlf
                        sSelectList = Left(sSelectList, Len(sSelectList) - 3) & vbCrLf

                        sSelectList = "SELECT " & sSelectList & vbCrLf & sTableList & vbCrLf & sWhereList & vbCrLf & sAddToWhereList

                        sSelectList = Replace(sSelectList, ACSPGenDataTempDataModelName, v_sDatamodel, 1)
                        sSelectList = Replace(sSelectList, ACSPGenDataTempDataModelTableName, sTableName, 1)
                        sSelectList = Replace(sSelectList, ACSPGenDataTempDataModelTableAlias, sPrefix, 1)

                        'Create the new procedure
                        sSQL = ""

                        sSQL = sSQL & "CREATE PROCEDURE " & sProcedureName & vbCrLf & "@PartyCnt INT," & vbCrLf & "@InsuranceFileCnt INT," & vbCrLf & "@ClaimCnt INT," & vbCrLf & "@DocumentRef VARCHAR(25)" & vbCrLf

                        'dynamically generate instance variables
                        generateInstanceVariables(sSQL, iInstanceCount)
                        sSQL = sSQL & vbCrLf & "AS" & vbCrLf & vbCrLf & sSelectList & vbCrLf

                        'UPGRADE_WARNING: Couldn't resolve default property of object r_oDatabase.SQLAction. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        If r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Create " & sProcedureName & " StoredProcedure", bStoredProcedure:=False) = gPMConstants.PMEReturnCode.PMTrue Then

                            'Set permissions
                            sSQL = "GRANT EXECUTE ON dbo." & sProcedureName & " TO PUBLIC"

                            'UPGRADE_WARNING: Couldn't resolve default property of object r_oDatabase.SQLAction. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            If r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Grant " & sProcedureName & " StoredProcedure", bStoredProcedure:=False) <> gPMConstants.PMEReturnCode.PMTrue Then

                                ' log error
                                GenerateClaimRiskSP = gPMConstants.PMEReturnCode.PMFalse

                                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                                oDict.Add("v_lObjectId", v_lObjectId)
                                oDict.Add("v_lParentId", v_lParentId)
                                gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to set default permissions for " & " stored procedure:" & sProcedureName, vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)
                            End If
                        Else
                            ' log error
                            GenerateClaimRiskSP = gPMConstants.PMEReturnCode.PMFalse

                            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                            oDict.Add("v_lObjectId", v_lObjectId)
                            oDict.Add("v_lParentId", v_lParentId)
                            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to create stored procedure:" & sSQL, vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)

                        End If

                    Else
                        ' log error
                        GenerateClaimRiskSP = gPMConstants.PMEReturnCode.PMFalse

                        Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                        oDict.Add("v_lObjectId", v_lObjectId)
                        oDict.Add("v_lParentId", v_lParentId)
                        gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to drop stored procedure:" & sProcedureName & " OR " & sProcedureNameOldStyle, vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)

                    End If

                End If ' we have parameters

            Else
                ' log error
                GenerateClaimRiskSP = gPMConstants.PMEReturnCode.PMFalse

                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lObjectId", v_lObjectId)
                oDict.Add("v_lParentId", v_lParentId)
                gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to ProcessPropertyArray", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)

            End If

        End If

        Exit Function

    End Function
End Module
