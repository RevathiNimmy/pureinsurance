Option Strict Off
Option Explicit On
Imports System.Text
Imports System.Text.RegularExpressions
Imports SSP.Shared
'<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable

    Private m_lReturn As gPMConstants.PMEReturnCode

    ' ************************************************
    ' Added to replace global variables 19/09/2003
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


    ' database class
    Private m_oDatabase As dPMDAO.Database

    Private m_oSADB As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    Private Const ACClass As String = "Business"
    Private Const ACApp As String = "bPBFindControl"

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private m_lInsurancefileCnt As Long
    Private m_lClaimId As Long
    Private m_dtEffectiveDateforUDL As Object 'Cover_from_date from NB / Loss date for Claim

    Public Property InsuranceFileCnt() As Integer
        Set(ByVal value As Integer)
            m_lInsurancefileCnt = value
        End Set
        Get
            Return m_lInsurancefileCnt
        End Get
    End Property

    Public Property ClaimCnt() As Long
        Set(ByVal value As Long)
            m_lClaimId = value
        End Set
        Get
            Return m_lClaimId
        End Get

    End Property



    ' ***************************************************************** '
    '
    ' Name: Terminate
    '
    ' Description:
    '
    ' History: 04/04/2001 CTAF - Created.
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
            End If
        End If
        Me.disposedValue = True
    End Sub



    ' ***************************************************************** '
    '
    ' Name: Start
    '
    ' Description:
    '
    ' History: 04/04/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer
        Dim result As Integer = 0

        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: Initialise
    '
    ' Description:
    '
    ' History: 04/04/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vdatabase As Object = Nothing) As Long

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



            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vdatabase), gPMConstants.PMEReturnCode)

            'architecture connection

            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oSADB, v_vDatabase:=vdatabase), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password

            ' Set the ProcessMode etc.
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

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



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function FindData(ByRef v_vSearchArray(,) As Object, ByRef v_vResultsArray(,) As Object) As Integer


        'Start (Prakash Varghese) - (Tech Spec - TRAC 3867 Wording Code Display.docx) - (6.1.1.1.1)
        'Removed the variable bIsDescription and its subsequent use since query is now sorted with Code field.
        'End (Prakash Varghese) - (Tech Spec - TRAC 3867 Wording Code Display.docx) - (6.1.1.1.1)
        Dim bIsEffeciveDate As Boolean
        Dim bIsVersion As Boolean
        Dim bUDLSearch As Boolean
        Dim dtEffectiveDate As Date

        'get find results
        'build SQL statement
        Dim sSQL As New StringBuilder
        sSQL.Append("Select ")

        If Regex.IsMatch(v_vSearchArray(ACViewName, 0).ToString.ToUpper, "UDL_*") Then
            bUDLSearch = True
            m_lReturn = GetUDLEffectiveDate()
        End If

        'columns
        For icount As Integer = 0 To v_vSearchArray.GetUpperBound(ACControl - 1)

            'Start (Prakash Varghese) - (Tech Spec - TRAC 3867 Wording Code Display.docx) - (6.1.1.1.1)
            'Removed the variable bIsDescription and its subsequent use since query is now sorted with Code field.
            'End (Prakash Varghese) - (Tech Spec - TRAC 3867 Wording Code Display.docx) - (6.1.1.1.1)

            'add comma if not the last item
            If icount = v_vSearchArray.GetUpperBound(ACControl - 1) Then

                sSQL.Append(CStr(v_vSearchArray(ACFieldName, icount)))
            Else

                sSQL.Append(CStr(v_vSearchArray(ACFieldName, icount)) & ",")
            End If

        Next icount


        'from

        sSQL.Append(" from " & CStr(v_vSearchArray(ACViewName, 0)))

        'where
        sSQL.Append(" where is_deleted = 0 and ")
        sSQL.Append("effective_date <= {effective_date} and ")

        For icount As Integer = 0 To v_vSearchArray.GetUpperBound(ACControl - 1)
            'dont add blank items

            If CStr(v_vSearchArray(ACSearchValue, icount)) <> "" Then
                If (v_vSearchArray(ACFieldName, icount)).ToString.ToLower.Trim <> "effective_date" Then


                    sSQL.Append(CStr(v_vSearchArray(ACFieldName, icount)))

                    If CStr(v_vSearchArray(ACSearchValue, icount)).IndexOf("%"c) >= 0 Then

                        'BSJ17122002
                        'PN1606 - check for %


                        sSQL.Append(" like '" & CStr(v_vSearchArray(ACSearchValue, icount)).Replace("'", "''") & "' and ")
                    Else
                        'add wildcard to text fields only

                        If CDbl(v_vSearchArray(ACControlType, icount)) = 1 Then

                            sSQL.Append(" like '" & CStr(v_vSearchArray(ACSearchValue, icount)).Replace("'", "''") & "%' and ")
                        Else

                            sSQL.Append(" = '" & CStr(v_vSearchArray(ACSearchValue, icount)).Replace("'", "''") & "' and ")
                        End If
                    End If
                    If v_vSearchArray(ACFieldName, icount).ToString.ToLower.Trim = "udl_version" Then
                        If Trim(v_vSearchArray(ACSearchValue, icount)) <> "" Then
                            bIsVersion = True
                        Else
                            bIsVersion = False
                        End If
                    End If
                ElseIf v_vSearchArray(ACFieldName, icount).ToString.ToLower.Trim = "effective_date" Then
                    If Trim(v_vSearchArray(ACSearchValue, icount)) <> "" Then
                        bIsEffeciveDate = True
                        dtEffectiveDate = ToSafeDate(v_vSearchArray(ACSearchValue, icount), ToSafeDate(m_dtEffectiveDateforUDL))
                    Else
                        bIsEffeciveDate = False
                    End If

                End If
            End If
        Next icount

        If bIsEffeciveDate = False Then
            If m_dtEffectiveDateforUDL Is Nothing Then
                dtEffectiveDate = DateTime.Now
            Else
                dtEffectiveDate = m_dtEffectiveDateforUDL
            End If
        End If

        'remove 'and' if last in the string
        If sSQL.ToString().EndsWith("and ") Then
            sSQL = New StringBuilder(sSQL.ToString().Substring(0, sSQL.ToString().Length - 4))
        End If

        If bUDLSearch And Not bIsVersion Then
            sSQL.Append(" and udl_version = (Select max(udl_version) FROM " & v_vSearchArray(ACViewName, 0) & " Where Effective_date <='" + dtEffectiveDate.ToString("yyyy-MM-dd") + "')")
        End If


        'Start (Prakash Varghese) - (Tech Spec - TRAC 3867 Wording Code Display.docx) - (6.1.1.1.1)
        'Removed the variable bIsDescription and its subsequent use since query is now sorted with Code field.

        If Not bIsVersion Then
            sSQL.Append(" order by description")
        Else
            sSQL.Append(" order by udl_version, description")
        End If

        'End (Prakash Varghese) - (Tech Spec - TRAC 3867 Wording Code Display.docx) - (6.1.1.1.1)

        'run query
        Debug.WriteLine(sSQL.ToString())

        ' No , Add Effective Parameter
        m_oDatabase.Parameters.Clear()
        If bUDLSearch Then
            m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_Date", vValue:=CStr(DateTime.Now), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
        Else
            m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_Date", vValue:=CStr(dtEffectiveDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
        End If
        'BSJ17122002
        'PN1606 - return max of 1000 rows as opposed to 100

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL.ToString(), sSQLName:="", bStoredProcedure:=False, lNumberRecords:=1000, vResultArray:=v_vResultsArray)


    End Function

    Public Function GetMappings(ByRef v_lFindControlID As Integer, ByRef v_vResultsArray(,) As Object) As Integer

        'get existing mappings if any
        Dim sSQL As String = "SELECT FindControl_ID,ControlIndex,ViewFieldName,ControlType,Fuzzy,ViewName,SearchValue,FoundValue,gis_object_id,gis_property_id,object_name,property_name,grid_caption,grid_position,grid_width FROM GIS_Find_Mapping WHERE findcontrol_id=" & v_lFindControlID & " Order by grid_position"

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="Find Items", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=v_vResultsArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        Else
            Return gPMConstants.PMEReturnCode.PMTrue
        End If

    End Function

    Public Function GetViews(ByRef v_vViews(,) As Object) As Integer

        'get lookups
        'select visible lookups in the Back Office database only i.e. pmproductid=2

        Dim sSQL As String = "SELECT p.lookup_table_name FROM PMProduct_Lookup p "
        sSQL = sSQL & " WHERE p.pmproduct_id=2 AND p.is_generic_maintenance=1"

        m_lReturn = m_oSADB.SQLSelect(sSQL:=sSQL, sSQLName:="View list", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=v_vViews)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        Else
            Return gPMConstants.PMEReturnCode.PMTrue
        End If

    End Function

    Public Function DeleteMappings(ByRef v_lFindControlID As Integer) As Integer

        'delete old mappings
        Dim sSQL As String = "Delete GIS_Find_mapping where FindControl_id=" & v_lFindControlID

        m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Delete GIS Find Mappings", bStoredProcedure:=False)

        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMTrue
        Else
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

    End Function

    ' developer guide no. 17
    Public Function GetViewFields(ByRef v_sView As String, ByRef v_vFields(,) As Object) As Integer

        Dim result As Integer = 0

        'validate entry
        If v_sView = "" Then

            result = gPMConstants.PMEReturnCode.PMError
        End If

        Dim sSQL As String = "select c.column_name FROM INFORMATION_SCHEMA.TABLES t"
        sSQL = sSQL & " ,inFORMATION_SCHEMA.COLUMNS c"
        sSQL = sSQL & " Where t.table_name=c.table_name and t.table_name='" & v_sView & "'"

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="Select View Fields", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=v_vFields)

        'return success status
        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMTrue
        Else
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result
    End Function

    ' ***************************************************************** '
    ' Name: AddMappings
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function AddMappings(ByRef v_vMappings(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AddMappings"

        'Const kMappingFindControlId As Integer = 0
        Const kMappingControlIndex As Integer = 1
        Const kMappingViewFieldName As Integer = 2
        Const kMappingControlType As Integer = 3
        Const kMappingFuzzy As Integer = 4
        Const kMappingViewName As Integer = 5
        'Const kMappingSearchValue As Integer = 6
        'Const kMappingFoundValue As Integer = 7
        Const kMappingGisObjectId As Integer = 8
        Const kMappingGisPropertyId As Integer = 9
        Const kMappingObjectName As Integer = 10
        Const kMappingPropertyName As Integer = 11
        Const kMappingGridCaption As Integer = 12
        Const kMappingGridPosition As Integer = 13
        Const kMappingGridWidth As Integer = 14

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim llBound, lUBound As Integer
        Dim vFindControlId As Object
        Dim bTransStarted As Boolean

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' if there are mappings to save
            If Informations.IsArray(v_vMappings) Then

                ' start the transaction
                lReturn = m_oDatabase.SQLBeginTrans()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "BeginTrans Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' indicate transaction started
                bTransStarted = True


                ' get array boundaries
                llBound = v_vMappings.GetLowerBound(1)
                lUBound = v_vMappings.GetUpperBound(1)

                ' default find control id to null


                vFindControlId = Nothing

                ' for each mapping item to add
                For lMappingItem As Integer = llBound To lUBound

                    ' add mapping item
                    ' NB: After first item is added the findcontrolid will have been set by the add routine
                    lReturn = CType(AddMapping(r_vFindControl:=vFindControlId, v_vControlIndex:=v_vMappings(kMappingControlIndex, lMappingItem), v_vViewFieldName:=v_vMappings(kMappingViewFieldName, lMappingItem), v_vControlType:=v_vMappings(kMappingControlType, lMappingItem), v_vFuzzy:=v_vMappings(kMappingFuzzy, lMappingItem), v_vViewName:=v_vMappings(kMappingViewName, lMappingItem), v_vGisObjectId:=v_vMappings(kMappingGisObjectId, lMappingItem), v_vGisPropertyId:=v_vMappings(kMappingGisPropertyId, lMappingItem), v_vObjectName:=v_vMappings(kMappingObjectName, lMappingItem), v_vPropertyName:=v_vMappings(kMappingPropertyName, lMappingItem), v_vGridCaption:=v_vMappings(kMappingGridCaption, lMappingItem), v_vGridPosition:=v_vMappings(kMappingGridPosition, lMappingItem), v_vGridWidth:=v_vMappings(kMappingGridWidth, lMappingItem)), gPMConstants.PMEReturnCode)

                    ' if any error occurred
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' raise it
                        gPMFunctions.RaiseError(kMethodName, "AddMapping Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                Next

                ' commit changes to database
                lReturn = m_oDatabase.SQLCommitTrans()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Commit Trans Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' indicate transaction finished
                bTransStarted = False

                ' save find control id back to array
                ' so it can be picked up in the interface


                v_vMappings(0, 0) = vFindControlId

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

            If bTransStarted Then
                m_oDatabase.SQLRollbackTrans()
            End If

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: AddMapping
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function AddMapping(ByRef r_vFindControl As Object, ByVal v_vControlIndex As Object, ByVal v_vViewFieldName As Object, ByVal v_vControlType As Object, ByVal v_vFuzzy As Object, ByVal v_vViewName As Object, ByVal v_vGisObjectId As Object, ByVal v_vGisPropertyId As Object, ByVal v_vObjectName As Object, ByVal v_vPropertyName As Object, ByVal v_vGridCaption As Object, ByVal v_vGridPosition As Object, ByVal v_vGridWidth As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AddMapping"


        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddOutputParameter(v_sName:="NEW_FindControl_Id", v_vValue:=0, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="FindControl_ID", v_vValue:=r_vFindControl, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="ControlIndex", v_vValue:=v_vControlIndex, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="ViewFieldName", v_vValue:=v_vViewFieldName, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="ControlType", v_vValue:=v_vControlType, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="Fuzzy", v_vValue:=v_vFuzzy, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="ViewName", v_vValue:=v_vViewName, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="gis_object_id", v_vValue:=v_vGisObjectId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="gis_property_id", v_vValue:=v_vGisPropertyId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="object_name", v_vValue:=v_vObjectName, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="property_name", v_vValue:=v_vPropertyName, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="grid_caption", v_vValue:=v_vGridCaption, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="grid_position", v_vValue:=v_vGridPosition, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="grid_width", v_vValue:=v_vGridWidth, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute Action Query
            If m_oDatabase.SQLAction(sSQL:=kAddMappingSQL, sSQLName:=kAddMappingName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kAddMappingSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If

            ' only get the find control when its null

            If Convert.IsDBNull(r_vFindControl) Or Informations.IsNothing(r_vFindControl) Then

                r_vFindControl = m_oDatabase.Parameters.Item("NEW_FindControl_ID").Value
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: AddInputParameter
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function AddInputParameter(ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iType As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AddInputParameter"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add Parameter to database object

            lReturn = m_oDatabase.Parameters.Add(sName:=v_sName, vValue:=CStr(v_vValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iType)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, " Failed to add parameter name:" & v_sName &
                                        ", values :" & CStr(v_vValue) & ", type:" & CStr(v_iType), gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function



    ' ***************************************************************** '
    ' Name: AddOutputParameter
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function AddOutputParameter(ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iType As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AddOutputParameter"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add Parameter to database object

            lReturn = m_oDatabase.Parameters.Add(sName:=v_sName, vValue:=CStr(v_vValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=v_iType)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, " Failed to add parameter name:" & v_sName &
                                        ", values :" & CStr(v_vValue) & ", type:" & CStr(v_iType), gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    Private Function GetUDLEffectiveDate() As Long


        Dim oBusiness As bCLMChangeClaimStatus.Business
        Dim vResultarray(,) As Object = Nothing

        If m_lClaimId > 0 Then
            oBusiness = New bCLMChangeClaimStatus.Business
            m_lReturn = oBusiness.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceId:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)


            m_lReturn = oBusiness.GetClaimDetails(v_lClaimId:=m_lClaimId, r_vResultArray:=vResultarray)
            ' Check for errors.
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                GetUDLEffectiveDate = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            If Informations.IsArray(vResultarray) Then
                m_dtEffectiveDateforUDL = ToSafeDate(vResultarray(4, 0), DateTime.Now).ToString("yyyy-MM-dd")
            End If
        ElseIf m_lInsurancefileCnt > 0 Then
            'GetinsurancefielDetails
            m_lReturn = GetInsuranceFileDetails(m_lInsurancefileCnt, vResultarray)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                GetUDLEffectiveDate = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If
            If Informations.IsArray(vResultarray) Then
                m_dtEffectiveDateforUDL = ToSafeDate(vResultarray(2, 0), DateTime.Now).ToString("yyyy-MM-dd")

            End If

        End If
        GetUDLEffectiveDate = gPMConstants.PMEReturnCode.PMTrue



    End Function

    Public Function GetInsuranceFileDetails( _
                            ByVal v_lInsuranceFileCnt As Long, _
                            ByRef r_vResults(,) As Object) As Long


        Const kMethodName As String = "GetInsuranceFileDetails"

        Dim lReturn As Long


        Try

            GetInsuranceFileDetails = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=v_lInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                GetInsuranceFileDetails = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect( _
                                    sSQL:="spu_SIR_Get_Insurance_File_Details", _
                                    sSQLName:="spu_SIR_Get_Insurance_File_Details", _
                                    bStoredProcedure:=True, _
                                    vResultArray:=r_vResults, _
                                    lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "spu_SIR_Get_Insurance_File_Details Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=GetInsuranceFileDetails, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        End Try

    End Function



    Private Shared _DefaultInstance As Business = Nothing
    Public Shared ReadOnly Property DefaultInstance() As Business
        Get
            If _DefaultInstance Is Nothing Then
                _DefaultInstance = New Business
            End If
            Return _DefaultInstance
        End Get
    End Property
End Class
