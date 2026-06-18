Option Strict Off
Option Explicit On
Imports SSP.Shared


'developer Guide No 129
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

    Private lPMAuthorityLevel As Integer
    'Developer Gudie No 244
    Private m_oDocManagerWrapper As bSIRDocManagerWrapper.Interface_Renamed
    Private m_obSIRFindDocTemplate As bSIRFindDocTemplate.Form


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
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long


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

            m_obSIRFindDocTemplate = New bSIRFindDocTemplate.Form
            m_lReturn = m_obSIRFindDocTemplate.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_obSIRFindDocTemplate.InitialiseBusiness Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return result
            End If

            'Doc Manager Wrapper
            'Developer Guide No 244
            m_oDocManagerWrapper = New bSIRDocManagerWrapper.Interface_Renamed
            m_lReturn = CType(m_oDocManagerWrapper.InitialiseBusiness(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="bSIRDocManagerWrapper.Interface.InitialiseBusuiness Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                m_obSIRFindDocTemplate = Nothing
                m_oDocManagerWrapper = Nothing
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
    ' Name: GetLookUp
    '
    ' Description: get values from look up table
    '                   if we have v_sSecondaryDescFieldName then use it when
    '                   v_sDescFieldName = "" or isnull
    '
    ' ***************************************************************** '
    Public Function GetLookUp(ByVal v_sTableName As String, ByVal v_sKeyIDFieldName As String, ByVal v_sDescFieldName As String, ByRef r_vResultArray(,) As Object, Optional ByVal v_sSecondaryDescFieldName As String = "") As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            If False Or (Not False And v_sSecondaryDescFieldName = "") Then

                sSQL = ""
                sSQL = sSQL & "SELECT " & v_sKeyIDFieldName & "," & v_sDescFieldName & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "FROM " & v_sTableName & Strings.ChrW(13) & Strings.ChrW(10)

                If v_sTableName = "Source" Then
                    ' Only load branches accessible to the current user
                    sSQL = sSQL & " WHERE source_id not in (select source_id from PMUser_Source where user_id = " & CStr(m_iUserID) & ") "
                Else
                    ' Exclude deleted records if we are not loading the branches.
                    ' (if we ARE loading the branches, then we do want the deleted ones, i.e. the closed branches)
                    sSQL = sSQL & " WHERE is_deleted = 0 "
                End If

                sSQL = sSQL & "ORDER BY " & v_sDescFieldName

            Else

                sSQL = "SELECT " & v_sKeyIDFieldName & ", " & Strings.ChrW(13) & Strings.ChrW(10) &
                       v_sDescFieldName & " = CASE " & v_sDescFieldName & Strings.ChrW(13) & Strings.ChrW(10) &
                       "WHEN null THEN " & v_sSecondaryDescFieldName & Strings.ChrW(13) & Strings.ChrW(10) &
                       "WHEN '' THEN " & v_sSecondaryDescFieldName & Strings.ChrW(13) & Strings.ChrW(10) &
                       "ELSE " & v_sDescFieldName & Strings.ChrW(13) & Strings.ChrW(10) &
                       "END" & Strings.ChrW(13) & Strings.ChrW(10) &
                       "FROM " & v_sTableName & Strings.ChrW(13) & Strings.ChrW(10) &
                       "ORDER BY " & v_sDescFieldName

            End If

            m_oDatabase.Parameters.Clear()


            Return m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetLookupValues", bStoredProcedure:=False, vResultArray:=r_vResultArray)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookUp Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookUp", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetRenewalInviteList
    ' Description: Get all policies that needs renewal
    ' ***************************************************************** '
    Public Function GetRenewalInviteList(ByVal v_dtSelectionDate As Date, ByVal v_vProductID As Object, ByVal v_vAgentID As Object, ByVal v_lSortOrder As Integer, ByRef r_vResultArray(,) As Object, Optional ByRef v_lSourceID As Integer = 0) As Integer

        ' Renewal Status Type
        Dim result As Integer = 0

        ' Sort order
        Const ACSortOrderProductCode As Integer = 0
        Const ACSortOrderRenewalDate As Integer = 1
        Const ACSortOrderPolicyNumber As Integer = 2
        Const ACSortOrderClientCode As Integer = 3
        Const ACSortOrderAgentCode As Integer = 4

        Dim sSQL As String = ""
        Dim sSortOrder As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Build core SQL
            sSQL = ""
            sSQL = sSQL & "SELECT PT.party_cnt, InsFile.insurance_folder_cnt, InsFile.insurance_file_cnt, RS.renewal_status_cnt, RS.insurance_file_cnt, Prod.Is_True_Monthly_Policy, InsFile.Anniversary_Copy " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "FROM Renewal_Status RS" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "INNER JOIN Insurance_File InsFile ON RS.renewal_insurance_file_cnt = InsFile.insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "INNER JOIN Insurance_Folder InsFolder ON InsFile.insurance_folder_cnt = InsFolder.insurance_folder_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "INNER JOIN Party PT ON InsFolder.insurance_holder_cnt = PT.party_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "LEFT JOIN Party PTA ON InsFile.lead_agent_cnt = PTA.party_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "INNER JOIN Product Prod ON InsFile.product_id = Prod.product_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "Where" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "(RS.is_invite_printed = 0" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "OR  RS.is_invite_printed is null)" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "AND RS.renewal_status_type_id = 2" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "AND InsFile.cover_start_date <= {cover_start_date}" & Strings.ChrW(13) & Strings.ChrW(10)

            ' Select by agent code if selected

            If Not (Convert.IsDBNull(v_vAgentID) Or Informations.IsNothing(v_vAgentID)) Then

                sSQL = sSQL & "AND PTA.party_cnt = " & CStr(CInt(v_vAgentID)) & Strings.ChrW(13) & Strings.ChrW(10)
            End If

            ' Select by product code if selected

            If Not (Convert.IsDBNull(v_vProductID) Or Informations.IsNothing(v_vProductID)) Then

                sSQL = sSQL & "AND InsFile.product_id = " & CStr(CInt(v_vProductID)) & Strings.ChrW(13) & Strings.ChrW(10)
            End If

            ' If branch was passed in, select on branch
            If v_lSourceID <> 0 Then
                sSQL = sSQL & "AND InsFile.source_id = " & CStr(v_lSourceID) & Strings.ChrW(13) & Strings.ChrW(10)
            Else
                sSQL = sSQL & "AND InsFile.source_id  NOT IN  (select source_id from PMUser_Source where user_id = " & CStr(m_iUserID) & " ) " & Strings.ChrW(13) & Strings.ChrW(10)
            End If

            ' What are we sorting by
            Select Case v_lSortOrder
                Case ACSortOrderProductCode
                    sSortOrder = "ORDER BY Prod.code"
                Case ACSortOrderRenewalDate
                    sSortOrder = "ORDER BY InsFile.renewal_date"
                Case ACSortOrderPolicyNumber
                    sSortOrder = "ORDER BY InsFile.insurance_ref"
                Case ACSortOrderClientCode
                    sSortOrder = "ORDER BY PT.shortname"
                Case ACSortOrderAgentCode
                    sSortOrder = "ORDER BY PTA.shortname"
            End Select

            ' Add order string to sql
            sSQL = sSQL & sSortOrder

            ' Add parameter
            m_oDatabase.Parameters.Clear()
            'Developer Guide No 40
            If m_oDatabase.Parameters.Add(sName:="cover_start_date", vValue:=v_dtSelectionDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Run query
            Return m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="SelRenewalInvite", bStoredProcedure:=False, vResultArray:=r_vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get renewal invite list", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRenewalInviteList", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetRePrintList
    '
    ' Description: get renewal invite last print run
    '
    ' ***************************************************************** '
    Public Function GetRePrintList(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Modifying the inline query to make it compatible with SQL server 2005

            'sSQL = "SELECT PT.party_cnt, InsFile.insurance_folder_cnt, InsFile.insurance_file_cnt, RS.renewal_status_cnt" & Strings.ChrW(13) & Strings.ChrW(10) & _
            '       "FROM Renewal_Status RS," & Strings.ChrW(13) & Strings.ChrW(10) & _
            '       "Insurance_File InsFile," & Strings.ChrW(13) & Strings.ChrW(10) & _
            '       "Insurance_Folder InsFolder," & Strings.ChrW(13) & Strings.ChrW(10) & _
            '       "Party PT," & Strings.ChrW(13) & Strings.ChrW(10) & _
            '       "Party PTA, " & Strings.ChrW(13) & Strings.ChrW(10) & _
            '       "Product Prod" & Strings.ChrW(13) & Strings.ChrW(10) & _
            '       "WHERE " & Strings.ChrW(13) & Strings.ChrW(10) & _
            '       "RS.renewal_status_cnt in (SELECT LPR.renewal_status_cnt from Last_Print_Run LPR)" & Strings.ChrW(13) & Strings.ChrW(10) & _
            '       "AND RS.renewal_insurance_file_cnt = InsFile.insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10) & _
            '       "AND InsFile.insurance_folder_cnt = InsFolder.insurance_folder_cnt" & Strings.ChrW(13) & Strings.ChrW(10) & _
            '       "AND InsFolder.insurance_holder_cnt = PT.party_cnt" & Strings.ChrW(13) & Strings.ChrW(10) & _
            '       "AND InsFile.lead_agent_cnt *= PTA.party_cnt" & Strings.ChrW(13) & Strings.ChrW(10) & _
            '       "AND InsFile.product_id = Prod.product_id" & Strings.ChrW(13) & Strings.ChrW(10) & _
            '       "ORDER BY PTA.shortname"

            sSQL = "SELECT PT.party_cnt, InsFile.insurance_folder_cnt, InsFile.insurance_file_cnt, RS.renewal_status_cnt" & Strings.ChrW(13) & Strings.ChrW(10) &
                  "FROM Renewal_Status RS," & Strings.ChrW(13) & Strings.ChrW(10) &
                  "Insurance_Folder InsFolder," & Strings.ChrW(13) & Strings.ChrW(10) &
                  "Party PT," & Strings.ChrW(13) & Strings.ChrW(10) &
                  "Product Prod," & Strings.ChrW(13) & Strings.ChrW(10) &
                  "Insurance_File InsFile" & Strings.ChrW(13) & Strings.ChrW(10) &
                  " Left outer join Party PTA" & Strings.ChrW(13) & Strings.ChrW(10) &
                  "On InsFile.lead_agent_cnt = PTA.party_cnt" & Strings.ChrW(13) & Strings.ChrW(10) &
                  "WHERE " & Strings.ChrW(13) & Strings.ChrW(10) &
                  "RS.renewal_status_cnt in (SELECT LPR.renewal_status_cnt from Last_Print_Run LPR)" & Strings.ChrW(13) & Strings.ChrW(10) &
                  "AND RS.renewal_insurance_file_cnt = InsFile.insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10) &
                  "AND InsFile.insurance_folder_cnt = InsFolder.insurance_folder_cnt" & Strings.ChrW(13) & Strings.ChrW(10) &
                  "AND InsFolder.insurance_holder_cnt = PT.party_cnt" & Strings.ChrW(13) & Strings.ChrW(10) &
                  "AND InsFile.product_id = Prod.product_id" & Strings.ChrW(13) & Strings.ChrW(10) &
                  "ORDER BY PTA.shortname"

            m_oDatabase.Parameters.Clear()

            'execute SQL statement
            'If m_oDatabase.SQLSelect(sSQL:=sSQL, _
            'sSQLName:="SelRePrintRenewalInvite", _
            'bStoredProcedure:=False, _
            'vResultArray:=r_vResultArray) <> PMTrue Then

            ' RAG 2003-03-05
            ' Return PMAllRecords, otherwise you only get the usual 500
            If m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="SelRePrintRenewalInvite", bStoredProcedure:=False, vResultArray:=r_vResultArray, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get renewal invite re-print list", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRePrintList", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DelLastPrintRun (Private)
    '
    ' Description: delete all from last print run table
    '
    ' ***************************************************************** '
    Public Function DelLastPrintRun() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'start transaction
            If BeginTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.SQLAction(sSQL:=ACDelLastPrintRunSQL, sSQLName:=ACDelLastPrintRunName, bStoredProcedure:=ACDelLastPrintRunStored) <> gPMConstants.PMEReturnCode.PMTrue Then

                'undo changes
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse

            End If


            'commit to database

            Return CommitTrans()

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DelLastPrintRun Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DelLastPrintRun", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddLastPrintRun (Private)
    '
    ' Description:add to last print run table
    '
    ' ***************************************************************** '
    Public Function AddLastPrintRun(ByVal v_lRenewalStatusCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'start transaction
            If BeginTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add(sName:="renewal_status_cnt", vValue:=CStr(v_lRenewalStatusCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            If m_oDatabase.Parameters.Add(sName:="UserID", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            If m_oDatabase.SQLAction(sSQL:=ACAddLastPrintRunSQL, sSQLName:=ACAddLastPrintRunName, bStoredProcedure:=ACAddLastPrintRunStored) <> gPMConstants.PMEReturnCode.PMTrue Then

                'undo changes
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse

            End If


            'commit to database


            Return CommitTrans()

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddLastPrintRun Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddLastPrintRun", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateRenewalStatus (Private)
    '
    ' Description: update the renewal status set is_invite_printed = true
    '                   and renewal_status_type = "Awaiting Update"
    '
    ' ***************************************************************** '
    Public Function UpdateRenewalStatus(ByVal v_lInsuranceFileCnt As Integer, Optional ByVal v_lRenewalExceptionReasonID As Integer = 0, Optional ByVal v_sRenewalExceptionNote As String = "") As Integer


        Dim result As Integer = 0
        Dim sCreditControlStatus As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'start transaction
            If BeginTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add(sName:="renewal_insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            If Not False Then
                If m_oDatabase.Parameters.Add(sName:="renewal_exception_reason_ID", vValue:=CStr(v_lRenewalExceptionReasonID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Return gPMConstants.PMEReturnCode.PMFalse

                End If
            End If

            If Not False Then
                If m_oDatabase.Parameters.Add(sName:="renewal_exception_note", vValue:=v_sRenewalExceptionNote, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Return gPMConstants.PMEReturnCode.PMFalse

                End If
            End If

            If m_oDatabase.SQLAction(sSQL:=ACUpdRenewalStatusSQL, sSQLName:=ACUpdRenewalStatusName, bStoredProcedure:=ACUpdRenewalStatusStored) <> gPMConstants.PMEReturnCode.PMTrue Then

                'undo changes
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse

            End If
            ' fetch value for Credit Control System Option
            m_lReturn = CType(bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=5001, r_sOptionValue:=sCreditControlStatus), gPMConstants.PMEReturnCode)


            If sCreditControlStatus = "1" Then

                m_oDatabase.Parameters.Clear()

                If m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Return gPMConstants.PMEReturnCode.PMFalse

                End If

                If m_oDatabase.Parameters.Add(sName:="business_type", vValue:="REN WTG UPDATE", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If m_oDatabase.SQLAction(sSQL:=ACAddCreditControlItemInsFileSQL, sSQLName:=ACAddCreditControlItemInsFileName, bStoredProcedure:=ACAddCreditControlItemInsFileStored) <> gPMConstants.PMEReturnCode.PMTrue Then

                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'commit to database

            Return CommitTrans()

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateRenewalStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRenewalStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name : GetDocTypeID
    '
    ' Desc : get document type id using document type code
    '
    ' ***************************************************************** '
    Public Function GetDocTypeID(ByVal v_sDocCode As String, ByRef r_lDocTypeID As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add(sName:="code", vValue:=v_sDocCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            'execute SQL statement
            result = m_oDatabase.SQLSelect(sSQL:=ACSelDocTypeIDSQL, sSQLName:=ACSelDocTypeIDName, bStoredProcedure:=ACSelDocTypeIDStored, vResultArray:=vResultArray)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            r_lDocTypeID = CInt(vResultArray(0, 0))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDocTypeID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocTypeID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    Private Function BeginTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLBeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
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
    Private Function CommitTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLCommitTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
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
    Private Function RollbackTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLRollbackTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
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

    ' private Methods (End)


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



    ' ***************************************************************** '
    '
    ' Name: GetAgents
    '
    ' Description:
    '
    ' History: 10/05/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function GetAgents(ByRef r_vAgentArray(,) As Object, Optional ByRef v_lSourceID As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If v_lSourceID = 0 Then 'get all Agents
                sSQL = "SELECT party_cnt, trading_name" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "FROM Party_Agent" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "ORDER BY trading_name"

                m_oDatabase.Parameters.Clear()

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetAllAgents", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vAgentArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                'get Agents specific to selected Branch

                m_oDatabase.Parameters.Clear()


                ' Add the branch parameter
                m_lReturn = m_oDatabase.Parameters.Add(sName:="Branchid", vValue:=CStr(v_lSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Call the stored procedure
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetBranchAgentsSQL, sSQLName:=ACGetBranchAgentsName, bStoredProcedure:=ACGetBranchAgentsStored, vResultArray:=r_vAgentArray, bKeepNulls:=True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAgents Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAgents", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '************************************************************************
    ' Name : IsInstalment
    '
    ' Desc : check to see if policy has a record in pfPremiumFiance
    '
    ' Hist : Thinh Nguyen 26/04/2002 (created)
    '************************************************************************
    Public Function IsInstalment(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelIsInstalmentSQL, sSQLName:=ACSelIsInstalmentName, bStoredProcedure:=ACSelIsInstalmentStored, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsInstalment Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsInstalment", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    'Pankaj
    ' ***************************************************************** '
    ' Name: ValidateRenewalInvite
    '
    ' Description: Check existance of a document
    '
    ' ***************************************************************** '
    Public Function ValidateRenewalInvite(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lDocumentTemplateId As Integer, ByRef r_lDocumentTypeId As Integer) As Integer
        Dim result As Integer = 0
        Dim sTemplateCode As String = ""
        Dim lReportPointer As Integer
        Dim sBusinessType As String = ""
        Dim vResultArray(,) As Object
        Dim vTabArray(,) As Object
        Dim lDocumentTemplateId, lDocumentTypeId As Integer
        Dim m_sDocumentTemplateDescription As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse


            vResultArray = Nothing
            ReDim vTabArray(3, 0)


            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = "process_types_docs"
            vTabArray.SetValue("process_types_docs", gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0)

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = 6

            m_lReturn = m_obSIRFindDocTemplate.GetProcessTypesLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupSingle, vTableArray:=vTabArray, iLanguageID:=m_iLanguageID, vResultArray:=vResultArray)


            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            sTemplateCode = CStr(vResultArray(2, 0)).Trim()
            If sTemplateCode = gSIRLibrary.SIRDocTypeCodeCancel Then

                m_lReturn = m_obSIRFindDocTemplate.GetBusinessType(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_sBusinessType:=sBusinessType)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                Select Case sBusinessType
                    Case "DIRECT"
                        sTemplateCode = gSIRLibrary.SIRDocTypeCodeCancelClient
                    Case Else
                        sTemplateCode = gSIRLibrary.SIRDocTypeCodeCancelAgent
                End Select

            End If

            sTemplateCode = sTemplateCode & m_sTransactionType.Trim()

            m_lReturn = m_obSIRFindDocTemplate.GetReportPointer(v_lInsuranceFileCnt, lReportPointer)

            If lReportPointer <> 0 Then
                sTemplateCode = sTemplateCode & CStr(lReportPointer)
            End If

            'Ensure template exists. If not, apply rules until  suitable template is found.

            m_lReturn = m_obSIRFindDocTemplate.GetAvailableTemplate(sTemplateCode, lDocumentTemplateId, lDocumentTypeId, m_sDocumentTemplateDescription)

            If lDocumentTemplateId <> 0 And lDocumentTypeId <> 0 Then
                r_lDocumentTemplateId = lDocumentTemplateId
                r_lDocumentTypeId = lDocumentTypeId
                result = gPMConstants.PMEReturnCode.PMTrue
            End If

            'Set obSIRFindDocTemplate = Nothing
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateRenewalInvite Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateRenewalInvite", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    'Pankaj
    ' ***************************************************************** '
    ' Name: PrintDocument
    '
    ' Description: Print out document
    '
    ' ***************************************************************** '
    Public Function PrintDocument(ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer) As Integer
        'ByVal v_lProcessType As Long) As Long

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDocManagerWrapper

                .PartyCnt = v_lPartyCnt
                .InsuranceFileCnt = v_lInsuranceFileCnt
                .InsuranceFolderCnt = v_lInsuranceFolderCnt
                '.ProcessTypesDocsId = v_lProcessType
                .Mode = 4
                m_lReturn = CType(.Start(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PrintDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PrintDocument", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result


            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: ProcessRenewalInvitation
    '
    ' Desc: do renewal print for a single policy that need renewal and called by SAM
    '
    ' History: 11.06.08 Creaded Pankaj
    '
    ' Task: WR9 Batch Renewals - Multi-Threaded Controller
    ' ***************************************************************** '
    Public Function ProcessRenewalInvitation(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lBatchRenewalJobID As Integer, ByVal v_lRecordsCount As Integer, ByVal v_sGUID As String) As Integer

        Dim result As Integer = 0
        ' Const kMethodName As String = "ProcessRenewalInvitation"

        'Field position
        Const ACFieldPosPartyCnt As Integer = 0
        Const ACFieldPosInsuranceFolderCnt As Integer = 1
        Const ACFieldPosInsuranceFileCnt As Integer = 2
        'Const ACFieldPosRenewalStatusCnt As Integer = 3
        Const ACFieldPosProductIsTrueMonthlyPolicy As Integer = 5
        Const ACFieldPosInsuranceFileIsAnniversaryCopy As Integer = 6
        Const ACFieldPosInsuranceFileRef As Integer = 7

        Dim vResultArray(,) As Object = Nothing
        Dim lCount As Integer
        Dim sExceptionNote As String = ""

        Dim bProductIsTrueMonthlyPolicy, bIsAnniversaryCopy, bPrintInvite As Boolean
        Dim oPMLock As bPMLock.User = Nothing
        Dim sLockedBy As String = ""
        Dim sFailedText As String = ""
        Dim lRecordsCount, lBatchRenewalJobRunsID As Integer

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            sFailedText = ""

            'Get bPMLock
            oPMLock = New bPMLock.User
            m_lReturn = oPMLock.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)


            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception(sFailedText)
            End If

            'Lock the Key

            m_lReturn = oPMLock.LockKey(sKeyName:="RENINV", vKeyValue:=v_lInsuranceFileCnt, iUserID:=m_iUserID, sCurrentlyLockedBy:=sLockedBy)

            'Check Return Type. If <> PMTrue then Skip (to ensure single user mode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailedText = "This record was being locked by " & sLockedBy
                Throw New Exception(sFailedText)
            End If


            'get that policy details which need renewal
            If GetRenewalInvitationDetails(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_vResultArray:=vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailedText = "Fails to get Renewal Invitation Details"
                Throw New Exception(sFailedText)
            End If

            'do we have any data
            If Not Informations.IsArray(vResultArray) Then
                Return result
            End If

            'Add Record to Batch_Renewal_Job_Runs
            m_lReturn = CType(AddBatchRenewalJobRuns(v_lBatchRenewalJobID:=v_lBatchRenewalJobID, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_dRunDate:=CDate(DateTime.Now.ToString("dd MMMM yyyy")), v_sFailureReason:="", v_sDocumentPrinted:="", v_iIsFailed:=0, v_sGUID:=v_sGUID, r_lRecordsCount:=lRecordsCount, r_lBatchRenewalJobRunsID:=lBatchRenewalJobRunsID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception(sFailedText)
            End If

            'loop through and process each renewal invite (There will be only one policy at a time in this array)
            ' new functionality to acheive multi threaded modal

            lCount = 0

            ' get tmp related fields

            bProductIsTrueMonthlyPolicy = gPMFunctions.ToSafeBoolean(CDbl(vResultArray(ACFieldPosProductIsTrueMonthlyPolicy, lCount)) = 1)

            bIsAnniversaryCopy = gPMFunctions.ToSafeBoolean(CDbl(vResultArray(ACFieldPosInsuranceFileIsAnniversaryCopy, lCount)) = 1)
            bPrintInvite = True

            If bProductIsTrueMonthlyPolicy And Not bIsAnniversaryCopy Then
                bPrintInvite = False
                m_lReturn = gPMConstants.PMEReturnCode.PMTrue
            End If


            Dim oSIRRenewalBusiness As New bSIRRenewal.Business
            If CType(oSIRRenewalBusiness.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword,
                                                iUserID:=m_iUserID, iSourceID:=m_iSourceID,
                                                iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID,
                                                iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp,
                                                vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode) <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="bSIRRenewal.Business.Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Dim nbIsRiskQuoted As Integer
            oSIRRenewalBusiness.IsQuoted(v_lInsuranceFileCnt:=v_lInsuranceFileCnt,
                                     r_lResult:=nbIsRiskQuoted)

            If nbIsRiskQuoted <> gPMConstants.PMEReturnCode.PMTrue Then
                sExceptionNote = "All risks are not quoted."
                m_lReturn = CType(UpdateBatchRenewalJobRuns(v_lBatchRenewalJobRunsID:=lBatchRenewalJobRunsID, v_sFailureReason:=sExceptionNote, v_iIsFailed:=1, v_iInsFileCnt:=v_lInsuranceFileCnt), gPMConstants.PMEReturnCode)
                m_lReturn = CType(UpdateRenewalStatus(v_lInsuranceFileCnt:=v_lInsuranceFileCnt,
                                                  v_lRenewalExceptionReasonID:=5,
                                                  v_sRenewalExceptionNote:=sExceptionNote),
                                                  gPMConstants.PMEReturnCode)

                m_lReturn = oSIRRenewalBusiness.SetRenewalStatus(v_lRenewalCnt:=v_lInsuranceFileCnt,
                                                             v_iRenewalStatus:=1)

                m_lReturn = oPMLock.UnLockKey(sKeyName:="RENINV", vKeyValue:=v_lInsuranceFileCnt, iUserID:=m_iUserID)
                Return gPMConstants.PMEReturnCode.PMTrue
            End If




            If bPrintInvite Then

                m_lReturn = CType(PrintRenewalInvite(v_lPartyCnt:=CInt(vResultArray(ACFieldPosPartyCnt, lCount)), v_lInsuranceFolderCnt:=CInt(vResultArray(ACFieldPosInsuranceFolderCnt, lCount)), v_lInsuranceFileCnt:=CInt(vResultArray(ACFieldPosInsuranceFileCnt, lCount)), v_lBatchRenewalJobID:=v_lBatchRenewalJobID, bCalledFromSAM:=False, v_sInsuranceFileRef:=gPMFunctions.ToSafeString(vResultArray(ACFieldPosInsuranceFileRef, lCount)).Trim()), gPMConstants.PMEReturnCode)

            End If

            'update renewal status and is_invite_printed = PMTRUE
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                'UPGRADE_WARNING: (1068) vResultArray() of type Variant is being forced to Integer. More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
                m_lReturn = CType(UpdateRenewalStatus(v_lInsuranceFileCnt:=CInt(vResultArray(ACFieldPosInsuranceFileCnt, lCount))), gPMConstants.PMEReturnCode)
            Else
                'UPGRADE_WARNING: (1068) vResultArray() of type Variant is being forced to Integer. More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
                m_lReturn = CType(UpdateRenewalStatus(v_lInsuranceFileCnt:=CInt(vResultArray(ACFieldPosInsuranceFileCnt, lCount)), v_lRenewalExceptionReasonID:=4, v_sRenewalExceptionNote:=sExceptionNote), gPMConstants.PMEReturnCode)
            End If

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception(sFailedText)
            End If


        Catch ex As Exception

            If sExceptionNote.Length = 0 Then
                sExceptionNote = sFailedText
            End If

            If sExceptionNote.Length = 0 Then
                sExceptionNote = Informations.Err().Description
            End If

            If sExceptionNote.Length = 0 Then
                sExceptionNote = "Fails to Process Renewal Inivitation"
            End If

            m_lReturn = CType(UpdateRenewalStatus(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lRenewalExceptionReasonID:=5, v_sRenewalExceptionNote:=sExceptionNote), gPMConstants.PMEReturnCode)


            'Add Failure Reason to Batch_Renewal_Job_Runs
            'Add Failure Reason to Batch_Renewal_Job_Runs
            m_lReturn = CType(UpdateBatchRenewalJobRuns(v_lBatchRenewalJobRunsID:=lBatchRenewalJobRunsID, v_sFailureReason:=sExceptionNote, v_iIsFailed:=1, v_iInsFileCnt:=v_lInsuranceFileCnt), gPMConstants.PMEReturnCode)

            'UnLock the Key
            'UPGRADE_TODO: (1067) Member UnlockKey is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
            m_lReturn = oPMLock.UnLockKey(sKeyName:="RENINV", vKeyValue:=v_lInsuranceFileCnt, iUserID:=m_iUserID)


            Return result
        Finally
            'UnLock the Key
            'UPGRADE_TODO: (1067) Member UnlockKey is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
            m_lReturn = oPMLock.UnLockKey(sKeyName:="RENINV", vKeyValue:=v_lInsuranceFileCnt, iUserID:=m_iUserID)


            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: GetRenewalInvitationDetails
    ' Description: Get policy details that needs renewal
    ' ***************************************************************** '
    Public Function GetRenewalInvitationDetails(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetRenewalInvitationDetails"
        'Dim sSQL, sSortOrder As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add parameter
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(ACClass, kMethodName & " Fails to Add parameter Insurance_File_Cnt", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRenewalInvitationDetailsSQL, sSQLName:=ACGetRenewalInvitationDetailsName, bStoredProcedure:=ACGetRenewalInvitationDetailsStored, vResultArray:=r_vResultArray, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(ACClass, kMethodName & " Fails to Get Renewal Invitation Details", gPMConstants.PMELogLevel.PMLogError)
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
    ' Name: PrintRenewalInvite (Private)
    '
    ' Description: Print out invite letter
    '
    ' ***************************************************************** '
    Private Function PrintRenewalInvite(ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lBatchRenewalJobID As Integer, ByVal bCalledFromSAM As Boolean, ByVal v_sInsuranceFileRef As String) As Integer

        Dim result As Integer = 0
        ' Const kMethodName As String = "PrintRenewalInvite"
        Const kRenewalNotice As Integer = 6
        ' Dim lDocTemplateId, lDocTypeId As Integer

        ' Dim oGetDocument As Object
        ' Dim vKeyArray As Object
        Dim oRenewal As bSIRRenewal.Business
        Dim iRenewalDocDestination, iReportSortOrder As Integer


        result = gPMConstants.PMEReturnCode.PMTrue

        m_lReturn = CType(GetBatchJobPrintingOptions(v_lBatchRenewalJobID:=v_lBatchRenewalJobID, r_iRenewalDocDestination:=iRenewalDocDestination, r_iReportSortOrder:=iReportSortOrder), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        'If no document printing require than exit from docs printing process
        If iRenewalDocDestination = 0 Then
            Return result
        End If


        oRenewal = New bSIRRenewal.Business
        m_lReturn = oRenewal.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)


        'Check for any error
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        'm_lReturn = oRenewal.GenerateCustomerRenewalEmail(v_lPartyCnt:=v_lPartyCnt, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_sType:="invitation")

        If iRenewalDocDestination = gPMConstants.PMRenewalDocDestination_Print Then
            iRenewalDocDestination = gSIRLibrary.ACPrintSilentMode
        Else
            iRenewalDocDestination = gSIRLibrary.ACSpoolDocMode
        End If

        m_lReturn = oRenewal.GenerateDocument(v_iDocType:=kRenewalNotice, v_iMode:=iRenewalDocDestination, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lPartyCnt:=v_lPartyCnt, v_sSpoolDesc:="Renewal Invite - Renewal Notice", v_sTransactionType:="RNI", v_bCalledFromSAM:=bCalledFromSAM, v_sInsuranceFileRef:=v_sInsuranceFileRef)

        If Not (oRenewal Is Nothing) Then

            oRenewal.Dispose()
            oRenewal = Nothing
        End If

        Return result
    End Function

    Private Function AddBatchRenewalJobRuns(ByVal v_lBatchRenewalJobID As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_dRunDate As Date, ByVal v_sFailureReason As String, ByVal v_sDocumentPrinted As Object, ByVal v_iIsFailed As Integer, ByVal v_sGUID As String, ByRef r_lRecordsCount As Integer, ByRef r_lBatchRenewalJobRunsID As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AddBatchRenewalJobRuns"

        'Dim vResultArray As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oDatabase.SQLBeginTrans()

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="batch_renewal_job_id", vValue:=CStr(v_lBatchRenewalJobID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If
            'Developer Guide No 40
            m_lReturn = m_oDatabase.Parameters.Add(sName:="run_date", vValue:=v_dRunDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="failure_reason", vValue:=v_sFailureReason, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="document_printed", vValue:=CStr(v_sDocumentPrinted), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_failed", vValue:=CStr(v_iIsFailed), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="GUID", vValue:=v_sGUID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Batch_Renewal_Job_Runs_ID", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Record_Count", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddBatchRenewalJobRunsSQL, sSQLName:=ACAddBatchRenewalJobRunsName, bStoredProcedure:=ACAddBatchRenewalJobRunsStored)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            m_lReturn = m_oDatabase.SQLCommitTrans()

            r_lBatchRenewalJobRunsID = m_oDatabase.Parameters.Item("Batch_Renewal_Job_Runs_ID").Value
            r_lRecordsCount = m_oDatabase.Parameters.Item("Record_Count").Value


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

        Catch ex As Exception
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            m_lReturn = m_oDatabase.SQLRollbackTrans()
        Finally

        End Try

        Return result
    End Function

    Private Function UpdateBatchRenewalJobRuns(ByVal v_lBatchRenewalJobRunsID As Integer, ByVal v_sFailureReason As String, ByVal v_iIsFailed As Integer, ByVal v_iInsFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateBatchRenewalJobRuns"

        'Dim lDocTemplateId, lDocTypeId As Integer
        'Dim oGetDocument As Object
        'Dim vKeyArray As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oDatabase.SQLBeginTrans()

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="batch_renewal_job_runs_id", vValue:=CStr(v_lBatchRenewalJobRunsID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="failure_reason", vValue:=v_sFailureReason, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_failed", vValue:=CStr(v_iIsFailed), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=ToSafeDouble(v_iInsFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If


            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateBatchRenewalJobRunsSQL, sSQLName:=ACUpdateBatchRenewalJobRunsName, bStoredProcedure:=ACUpdateBatchRenewalJobRunsStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            m_lReturn = m_oDatabase.SQLCommitTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

        Catch ex As Exception
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            m_lReturn = m_oDatabase.SQLRollbackTrans()
        Finally
        End Try
        Return result
    End Function


    Private Function GetBatchJobPrintingOptions(ByVal v_lBatchRenewalJobID As Integer, ByRef r_iRenewalDocDestination As Integer, ByVal r_iReportSortOrder As Integer) As Integer

        Dim result As Integer = 0
        'Const kMethodName As String = "GetBatchJobPrintingOptions"

        Dim vResultArray(,) As Object = Nothing

        result = gPMConstants.PMEReturnCode.PMTrue

        r_iRenewalDocDestination = 0
        r_iReportSortOrder = 0

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="Batch_Renewal_Job_Id", vValue:=CStr(v_lBatchRenewalJobID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetBatchJobPrintingOptionsSQL, sSQLName:=ACGetBatchJobPrintingOptionsName, bStoredProcedure:=ACGetBatchJobPrintingOptionsStored, vResultArray:=vResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        If Informations.IsArray(vResultArray) Then
            r_iRenewalDocDestination = gPMFunctions.ToSafeLong(vResultArray(0, 0))
            r_iReportSortOrder = gPMFunctions.ToSafeLong(vResultArray(1, 0))
        End If
        Return result
    End Function
End Class