Option Strict Off
Option Explicit On
Imports System.Globalization
'developer guide no. 129
Imports SSP.Shared
Imports System.Text.RegularExpressions
<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable

    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 07/05/1999
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a SIRTextFileDesc.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 06/02/2004
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

    ' Error Code (Private)
    Private m_lReturn As Integer

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private lPMAuthorityLevel As Integer

    ' Primary Keys to work with
    Private m_lEntityTypeId As Integer
    Private m_sDescription As String = ""

    ' PRIVATE Data Members (End)
    Private m_sMaskCode As String = ""
    Private m_lHighestNumber As Integer

    'developer guide no. 39 (Guide)
    Private Const ACGetNumberingSchemeIdsFromProductSQL As String = "spu_get_prod_auto_num_ids"
    Private Const ACGetNumberingSchemeIdsFromProductName As String = "GetNumberingSchemeIdsFromProduct"
    Private Const ACGetNumberingSchemeFromProductStored As Boolean = True

    'developer guide no. 39 (Guide)
    Private Const ACGetNumberingSchemeSQL As String = "spu_numbering_scheme_saa"
    Private Const ACGetNumberingSchemeName As String = "GetNumberingScheme"
    Private Const ACGetNumberingSchemeStored As Boolean = True

    Private Const ACGetNumberingSchemeHistorySQL As String = "spu_numbering_scheme_history_sel"
    Private Const ACGetNumberingSchemeHistoryName As String = "GetNumberingSchemeHistory"
    Private Const ACGetNumberingSchemeHistoryStored As Boolean = True
    'End (Saurabh Agrawal) Tech spec VAL P14 Policy Numbering(5.2.1.2)


    'Start(Saurabh Agrawal) Tech spec VAL P14 Policy Numbering(5.2.1.2)
    Private Const ACGetBranchcode As String = "spu_SAM_Get_Branch_Code"
    Private Const ACGetBranchCodeName As String = "Get Branch Code"
    Private Const ACGetBranchCodeStored As Boolean = True
    'End (Saurabh Agrawal) Tech spec VAL P14 Policy Numbering(5.2.1.2)

    'Start - Renuka - (WPR87 Paralleling)
    Private Const ACGetPeiodNextNumberSQL As String = "spu_period_next_number_saa"
    Private Const ACGetPeiodNextNumberName As String = "GetPeiodNextNumber"
    Private Const ACGetPeiodNextNumberStored As Boolean = True
    'End - Renuka - (WPR87 Paralleling)

    'Start - Renuka - (WPR87 Paralleling)
    Private Const ACIncrementPeriodNumberingSchemeSQL As String = "spu_increment_period_numbering_scheme"
    Private Const ACIncrementPeriodNumberingSchemeName As String = "IncrementPeriodNumberingScheme"
    Private Const ACIncrementPeriodNumberingSchemeStored As Boolean = True
    'End - Renuka - (WPR87 Paralleling)

    Private Const ACGetAbandonedNumberingSchemeStored As Boolean = True
    Private Const ACGetAbandonedNumberingSchemeName As String = "GetAbandonedNumberingScheme"
    'developer guide no. 39 (Guide)
    Private Const ACGetAbandonedNumberingSchemeSQL As String = "spu_abandoned_numbers_saa"


    Private Const ACPolicyNumberingSchemegetandIncrementSQL As String = "spu_Policy_Numbering_scheme_GetAndIncrement"
    Private Const ACPolicyNumberingSchemegetandIncrementName As String = "Policy_Numbering_scheme_GetAndIncrement"
    Private Const ACPolicyNumberingSchemegetandIncrementStored As Boolean = True


    Private Const ACPolicyNumberingSchemegetandIncrementPeriodSQL As String = "spu_Policy_numbering_scheme_GetAndIncrement_Period"
    Private Const ACPolicyNumberingSchemegetandIncrementPeriodName As String = "Policy_numbering_scheme_GetAndIncrement_Period"
    Private Const ACPolicyNumberingSchemegetandIncrementPeriodStored As Boolean = True

    Private Const ACPartyNumberingSchemegetandIncrementSQL As String = "spu_Party_Numbering_scheme_GetAndIncrement"
    Private Const ACPartyNumberingSchemegetandIncrementName As String = "Party_Numbering_scheme_GetAndIncrement"
    Private Const ACPartyNumberingSchemegetandIncrementStored As Boolean = True


    Private m_bGenerate As Boolean
    Private m_bValidationRequired As Boolean

    Private Const iNUM_SCHEME_ID As Integer = 0
    Private Const iNUM_SCHEME_CAPTION_ID As Integer = 1
    Private Const iNUM_SCHEME_CODE As Integer = 2
    Private Const iNUM_SCHEME_DESCRIPTION As Integer = 3
    Private Const iNUM_SCHEME_IS_DEL As Integer = 4
    Private Const iNUM_SCHEME_EFF_DATE As Integer = 5
    Private Const iNUM_SCHEME_TYPE_ID As Integer = 6
    Private Const iNUM_SCHEME As Integer = 7
    Private Const iNUM_SCHEME_IS_GEN As Integer = 8
    Private Const iNUM_SCHEME_MASK As Integer = 9
    Private Const iNUM_SCHEME_FIXED_CODE As Integer = 10
    Private Const iNUM_SCHEME_NEXT_NUM As Integer = 11
    Private Const iNUM_SCHEME_HIGH_NUM As Integer = 12
    Private Const iNUM_SCHEME_STEP As Integer = 13
    Private Const iNUM_SCHEME_REUSE_ABANDONED As Integer = 14
    Private Const iNUM_SCHEME_TYPE_DESC As Integer = 15

    Private Const iProd_Num_Scheme_id As Integer = 2
    Private Const iProd_Num_Scheme_code As Integer = 0

    '(Start)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(5.4.1.2)
    Private Const iNUM_SCHEME_RESET_DAILY As Integer = 19
    Private Const iNUM_SCHEME_DATE_LAST_GENERATED As Integer = 20
    '(End)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(5.4.1.2)
    'Start - Renuka - (WPR87 Paralleling)
    Private Const iNUM_IS_RESET_NUMBER As Integer = 21
    'End - Renuka - (WPR87 Paralleling)


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




            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password

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
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
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
    ' Name: SearchAll (Public)
    '
    ' Description: Gets everything as an array
    '
    ' ***************************************************************** '
    Public Function SearchAll(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'language_id temporarily hard-coded  as this is only possible value at present.
            m_lReturn = m_oDatabase.Parameters.Add(sName:="language_id", vValue:=CStr(m_iLanguageID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllDetailsSQL, sSQLName:=ACGetAllDetailsName, bStoredProcedure:=ACGetAllDetailsStored, lNumberRecords:=0, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SearchAll Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchAll", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: WriteAll (Public)
    '
    ' Description: Writes everything from an array
    '
    ' ***************************************************************** '
    'Developer Guide No 33
    Public Function WriteAll(ByVal v_vArray(,) As Object, ByVal v_iActionArray As Object, Optional ByVal v_sUniqueId As String = "") As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Const iACTION_ADD As Integer = 1
        Const iACTION_UPDATE As Integer = 2
        Dim oArchDatabase As New dPMDAO.Database

        'developer guide no. 39 (Guide)
        Const sADD_PM_CAPTION As String = "spu_pm_caption_id_return"
        Const sADD_PM_CAPTION_NAME As String = "AddPMCaption"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Informations.IsArray(v_vArray) Then
                Return result
            End If

            '    m_lReturn = BeginTrans()
            '
            '    If (m_lReturn <> PMTrue) Then
            '        WriteAll = PMFalse
            '        Exit Function
            '    End If

            Dim iIsResetDaily As Integer
            For iRecord As Integer = v_vArray.GetLowerBound(1) To v_vArray.GetUpperBound(1)
                'Is any action required on this record.
                Dim sScreenHierarchy As String = ""
                If CDbl(v_iActionArray(iRecord)) > 0 Then

                    '** Add Scheme caption to architecture database - Start


                    m_lReturn = gPMComponentServices.NewDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_oDatabase:=oArchDatabase)


                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oArchDatabase.Parameters.Clear()

                    m_lReturn = oArchDatabase.Parameters.Add(sName:="language_id", vValue:=CStr(m_iLanguageID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        '                m_lReturn = RollbackTrans()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = oArchDatabase.Parameters.Add(sName:="caption", vValue:=CStr(v_vArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_DESCRIPTION, iRecord)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        '                m_lReturn = RollbackTrans()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = oArchDatabase.Parameters.Add(sName:="caption_id", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        '                m_lReturn = RollbackTrans()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = oArchDatabase.SQLAction(sSQL:=sADD_PM_CAPTION, sSQLName:=sADD_PM_CAPTION_NAME, bStoredProcedure:=True)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        '                m_lReturn = RollbackTrans()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Get returned caption_id to pass into AddScheme proc.

                    v_vArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_CAPTION_ID, iRecord) = oArchDatabase.Parameters.Item("caption_id").Value
                    oArchDatabase.CloseDatabase()
                    oArchDatabase = Nothing

                    '** Add Scheme caption to architecture database - Finish

                    ' Clear the Database Parameters Collection
                    m_oDatabase.Parameters.Clear()


                    m_lReturn = m_oDatabase.Parameters.Add(sName:="numbering_scheme_id", vValue:=CStr(v_vArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_SCHEME_ID, iRecord)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        '                m_lReturn = RollbackTrans()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = m_oDatabase.Parameters.Add(sName:="caption_id", vValue:=CStr(v_vArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_CAPTION_ID, iRecord)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        '                m_lReturn = RollbackTrans()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=CStr(v_vArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_CODE, iRecord)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        '                m_lReturn = RollbackTrans()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = m_oDatabase.Parameters.Add(sName:="description", vValue:=CStr(v_vArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_DESCRIPTION, iRecord)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        '                m_lReturn = RollbackTrans()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = m_oDatabase.Parameters.Add(sName:="is_deleted", vValue:=CStr(v_vArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_IS_DELETED, iRecord)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        '                m_lReturn = RollbackTrans()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=CStr(v_vArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_EFFECTIVE_DATE, iRecord)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        '                m_lReturn = RollbackTrans()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = m_oDatabase.Parameters.Add(sName:="numbering_scheme_type_id", vValue:=CStr(v_vArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_TYPE, iRecord)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        '                m_lReturn = RollbackTrans()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = m_oDatabase.Parameters.Add(sName:="numbering_scheme", vValue:=CStr(v_vArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_SCHEME, iRecord)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        '                m_lReturn = RollbackTrans()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = m_oDatabase.Parameters.Add(sName:="is_generated", vValue:=CStr(v_vArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_IS_GENERATED, iRecord)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        '                m_lReturn = RollbackTrans()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = m_oDatabase.Parameters.Add(sName:="mask_code", vValue:=CStr(v_vArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_MASK_CODE, iRecord)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        '                m_lReturn = RollbackTrans()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = m_oDatabase.Parameters.Add(sName:="fixed_code", vValue:=CStr(v_vArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_FIXED_CODE, iRecord)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        '                m_lReturn = RollbackTrans()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = m_oDatabase.Parameters.Add(sName:="next_number", vValue:=CStr(v_vArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_NEXT_NUMBER, iRecord)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        '                m_lReturn = RollbackTrans()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = m_oDatabase.Parameters.Add(sName:="highest_number", vValue:=CStr(v_vArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_HIGHEST_NUMBER, iRecord)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        '                m_lReturn = RollbackTrans()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = m_oDatabase.Parameters.Add(sName:="step", vValue:=CStr(v_vArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_STEP, iRecord)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        '                m_lReturn = RollbackTrans()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = m_oDatabase.Parameters.Add(sName:="is_reuse_abandoned", vValue:=CStr(v_vArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_IS_REUSE, iRecord)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)


                    m_lReturn = m_oDatabase.Parameters.Add(sName:="party_type_id", vValue:=CStr(v_vArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_PARTY_TYPE_ID, iRecord)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)


                    m_lReturn = m_oDatabase.Parameters.Add(sName:="is_read_only", vValue:=CStr(v_vArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_IS_READ_ONLY, iRecord)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        '                m_lReturn = RollbackTrans()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    '(Start)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(5.4.1.2)
                    Dim bTemp As Boolean = False
                    If Boolean.TryParse(v_vArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_IS_RESET_DAILY, iRecord), bTemp) AndAlso bTemp Then
                        iIsResetDaily = 1
                    Else
                        iIsResetDaily = 0
                    End If
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="is_reset_daily", vValue:=CStr(iIsResetDaily), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    '(End)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(5.4.1.2)
                    'Start - Renuka - (WPR87 Paralleling)

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="is_reset_number", vValue:=CStr(v_vArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_IS_RESET_NUMBER, iRecord)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="UserId", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="UniqueId", vValue:=v_sUniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If Not String.IsNullOrEmpty(v_sUniqueId) Then
                        sScreenHierarchy = $"Numbering Scheme({CStr(v_vArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_DESCRIPTION, iRecord)).Trim()})"
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="ScreenHierarchy", vValue:=sScreenHierarchy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    'End - Renuka - (WPR87 Paralleling)
                    'Decide which action required and execute.
                    Select Case v_iActionArray(iRecord)
                        Case iACTION_ADD
                            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACInsertNumberingSchemeSQL, sSQLName:=ACInsertNumberingSchemeName, bStoredProcedure:=ACInsertNumberingSchemeStored)
                        Case iACTION_UPDATE
                            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateNumberingSchemeSQL, sSQLName:=ACUpdateNumberingSchemeName, bStoredProcedure:=ACUpdateNumberingSchemeStored)

                        Case Else
                            'Do nothing
                            'Log error ??
                    End Select

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        '                m_lReturn = RollbackTrans()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If 'End of check if action required.
            Next iRecord

            '    m_lReturn = CommitTrans()

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="WriteAll Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="WriteAll", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

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

    ' PRIVATE Methods (End)


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


    ' JMK 15/10/2001 - add optional parameters for claim numbering
    'Renuka - (WPR87 Paralleling) - Added optional parameter v_dtTransactionDate
    Public Function GeneratePolicyNumber(ByVal v_lBusinessType As Integer, ByVal v_iBranch As Integer, ByVal v_lProductId As Integer, ByVal v_lAgent As Integer, ByRef r_sGeneratedPolicyNumber As String, Optional ByVal v_sLossYear As String = "", Optional ByVal v_sReportedYear As String = "", Optional ByVal v_dtTransactionDate As Date = #12/30/1899#, Optional ByVal v_lPartyCnt As Integer = 0) As Integer
        Dim result As Integer = 0
        Dim oACTPeriod As bACTPeriod.Form

        Dim lNumberingScheme As Integer
        Dim vResultArray(,) As Object = Nothing
        Dim sCode As String = String.Empty
        Dim sAbandonedNumber As String = String.Empty

        Dim sFixedCode, bReuseAbandoned, sSQL, sNumberToAllocate As String
        Dim nStep As Integer
        Dim iNumPos, iNumStart, iNumLength As Integer
        Dim sNumber As String = ""
        'Dim sNextNumber As String = String.Empty
        Dim bResetNumber As Boolean



        Dim sClaimMask As String = ""
        Dim sStateCode As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Interrogate product table to find numbering scheme id required.
            'Need to include select in transaction as next number may be updated
            'within it.
            m_lReturn = BeginTrans()

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="product_id", vValue:=v_lProductId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetNumberingSchemeIdsFromProductSQL, sSQLName:=ACGetNumberingSchemeIdsFromProductName, bStoredProcedure:=ACGetNumberingSchemeFromProductStored, lNumberRecords:=0, vResultArray:=vResultArray)

            'Fields are retrieved in same order as business type codes.
            'First field (0) is 'code'.
            If Not (Informations.IsArray(vResultArray)) Then
                lNumberingScheme = 0
                sCode = ""
            Else
                'vResultArray(5, 0) to check whether the Policy at Quote is checked or not

                If CDbl(vResultArray(5, 0)) = 1 And v_lBusinessType = 1 Then
                    v_lBusinessType = 2
                End If
                Dim auxVar As Object = vResultArray(v_lBusinessType, 0)


                If Not (Convert.IsDBNull(auxVar) Or Informations.IsNothing(auxVar)) Then

                    lNumberingScheme = CInt(vResultArray(v_lBusinessType, 0))

                    sCode = CStr(vResultArray(0, 0)).Trim().ToUpper()
                End If
            End If

            'If no code found then user can enter anything, but must be checked
            'in ValidatePolicyNumber that it doesn't already exist.

            If (lNumberingScheme = 0) Or Convert.IsDBNull(lNumberingScheme) Or Informations.IsNothing(lNumberingScheme) Then
                m_bGenerate = False
                m_sMaskCode = ""

                'RWH(17/10/2000) Ensure transaction is not left open.
                m_lReturn = CommitTrans()
                Return result
            End If

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            '** Get particular numbering scheme
            m_lReturn = m_oDatabase.Parameters.Add(sName:="language_id", vValue:=m_iLanguageID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="numbering_scheme_id", vValue:=lNumberingScheme, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetNumberingSchemeSQL, sSQLName:="", bStoredProcedure:=True, lNumberRecords:=0, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                m_lReturn = RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get Policy Number Rules, setting module level Properties for use in Validate method.
            m_bGenerate = CBool(vResultArray(iNUM_SCHEME_IS_GEN, 0))
            m_sMaskCode = CStr(vResultArray(iNUM_SCHEME_MASK, 0))
            sFixedCode = CStr(vResultArray(iNUM_SCHEME_FIXED_CODE, 0))
            ' sNextNumber = CStr(vResultArray(iNUM_SCHEME_NEXT_NUM, 0))
            bReuseAbandoned = CStr(vResultArray(iNUM_SCHEME_REUSE_ABANDONED, 0))
            bResetNumber = gPMFunctions.ToSafeBoolean(CStr(vResultArray(iNUM_IS_RESET_NUMBER, 0)))
            nStep = ToSafeInteger(vResultArray(iNUM_SCHEME_STEP, 0))

            If Not m_bGenerate Then

                ' RAG 24-10-01: *ROLLBACK THE TRANSACTION*
                ' otherwise you will end up on a two-day debugging wild goose chase like I did...
                m_lReturn = RollbackTrans()
                r_sGeneratedPolicyNumber = ""
                Return result
            End If

            sNumberToAllocate = m_sMaskCode

            If m_sMaskCode.IndexOf("P"c) >= 0 Then
                m_lReturn = InsertCodeIntoNumber(m_sMaskCode, sCode, "P", sNumberToAllocate)
            End If

            If m_sMaskCode.IndexOf("A"c) >= 0 Then
                sSQL = "SELECT file_code FROM party " & "WHERE party_cnt = " & v_lAgent

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = RollbackTrans()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                sCode = ""
                If Informations.IsArray(vResultArray) Then
                    sCode = CStr(vResultArray(0, 0)).Trim().ToUpper()
                End If

                m_lReturn = InsertCodeIntoNumber(m_sMaskCode, sCode, "A", sNumberToAllocate)
            End If

            ' JMK 15/10/2001 - Loss date or Reported date

            If (m_sMaskCode.IndexOf("L"c) >= 0) Or (m_sMaskCode.IndexOf("R"c) >= 0) Then

                sCode = ""
                If m_sMaskCode.IndexOf("L"c) >= 0 Then
                    sClaimMask = "L"
                    sCode = v_sLossYear
                Else
                    sClaimMask = "R"
                    sCode = v_sReportedYear
                End If
                m_lReturn = InsertCodeIntoNumber(m_sMaskCode, sCode, sClaimMask, sNumberToAllocate)
            End If
            ' JMK 15/10/2001 end

            'Start - Enhancement PM041940 - WPR63_Add the State to the Numbering Scheme (Jai 21/04/2015)
            If m_sMaskCode.IndexOf("E"c) >= 0 Then
                If v_lPartyCnt > 0 Then
                    m_oDatabase.Parameters.Clear()
                    vResultArray = Nothing

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="Party_Cnt", vValue:=v_lPartyCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_Id", vValue:=0, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                    ' Execute stored procedure
                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetStateCodeForPartySQL, sSQLName:=ACGetStateCodeForPartyName, bStoredProcedure:=ACGetStateCodeForPartyStored, lNumberRecords:=0, vResultArray:=vResultArray)

                    If (Informations.IsArray(vResultArray)) Then
                        sStateCode = gPMFunctions.ToSafeString(vResultArray.GetValue(0, 0), "").Trim.ToUpper
                    End If
                    If sStateCode.Length > 2 Then
                        sStateCode = sStateCode.Substring(0, 2)
                    End If
                End If
                If Trim(sStateCode) = "" Then
                    sStateCode = "NA"
                End If
                m_lReturn = InsertCodeIntoNumber(m_sMaskCode, sStateCode, "E", sNumberToAllocate)
            End If
            'End - Enhancement PM041940 - WPR63_Add the State to the Numbering Scheme (Jai 21/04/2015)

            If m_sMaskCode.IndexOf("B"c) >= 0 Then
                sSQL = "SELECT code FROM source " & "WHERE source_id = " & v_iBranch

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = RollbackTrans()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                sCode = ""
                If Informations.IsArray(vResultArray) Then

                    sCode = CStr(vResultArray(0, 0)).Trim().ToUpper()
                End If
                m_lReturn = InsertCodeIntoNumber(m_sMaskCode, sCode, "B", sNumberToAllocate)

            End If

            If m_sMaskCode.IndexOf("I"c) >= 0 Then
                m_lReturn = InsertCodeIntoNumber(m_sMaskCode, "0", "I", sNumberToAllocate)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Dim lPeriodId As Integer
            Dim sYearName As String = ""
            If m_sMaskCode.IndexOf("U"c) >= 0 Then

                'Create business object of bACTPeriod

                oACTPeriod = New bACTPeriod.Form
                m_lReturn = oACTPeriod.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to get an instance of the business object.
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create an instance of business object bACTPeriod.Form.", vApp:=ACApp, vClass:=ACClass, vMethod:="GeneratePolicyNumber")

                    m_lReturn = RollbackTrans()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If v_dtTransactionDate = #12/30/1899# Then
                    v_dtTransactionDate = DateTime.Parse(DateTime.Now)
                End If


                m_lReturn = oACTPeriod.GetPeriodForDate(dtDateInPeriod:=v_dtTransactionDate, lPeriodId:=lPeriodId, vYearName:=sYearName, v_bIncludeClosed:=True)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Call to method bACTPeriod.Form.GetPeriodForDate failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="GeneratePolicyNumber")
                    m_lReturn = RollbackTrans()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = InsertCodeIntoNumber(m_sMaskCode, sYearName.Trim(), "U", sNumberToAllocate)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Call to method InsertCodeIntoNumber failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="GeneratePolicyNumber")
                    m_lReturn = RollbackTrans()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Terminate the business object

                oACTPeriod.Dispose()

                ' Destroy the instance of the business object from memory.
                oACTPeriod = Nothing
            End If

            'If number is to be generated determine format of number.
            If m_sMaskCode.IndexOf("X"c) >= 0 Then
                m_lReturn = InsertCodeIntoNumber(m_sMaskCode, sFixedCode, "X", sNumberToAllocate)
            End If

            If m_sMaskCode.IndexOf("Y"c) >= 0 Then
                m_lReturn = InsertCodeIntoNumber(m_sMaskCode, DateTime.Today, "Y", sNumberToAllocate)
            End If

            'If number is to reuse abandoned number then use non-numeric portion to search
            'Abandoned number table.

            '** Get appropriate abandoned numbering scheme if one exists.

            sNumber = ""
            If bReuseAbandoned = gPMConstants.PMEReturnCode.PMTrue Then
                ' Clear the Database Parameters Collection
                m_oDatabase.Parameters.Clear()

                m_lReturn = m_oDatabase.Parameters.Add(sName:="numbering_scheme_id", vValue:=lNumberingScheme, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = RollbackTrans()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAbandonedNumberingSchemeSQL, sSQLName:="", bStoredProcedure:=True, lNumberRecords:=0, vResultArray:=vResultArray)

                If Informations.IsArray(vResultArray) Then

                    sAbandonedNumber = CStr(vResultArray(0, 0))

                    'Need to strip numeric section out of abandoned policy number.
                    iNumPos = (m_sMaskCode.IndexOf("9"c) + 1)
                    If iNumPos <> 0 Then
                        iNumStart = iNumPos
                        iNumLength = 0

                        Do While m_sMaskCode.IndexOf("9", iNumPos - 1) <> 0
                            iNumLength += 1
                            iNumPos += 1
                        Loop
                        'Store numeric section.
                        sNumber = sAbandonedNumber.Substring(iNumStart - 1, Math.Min(sAbandonedNumber.Length, iNumLength))

                        If sNumber = "0" Then
                            sNumber = ""
                        End If

                        'Remove reused Policy Number from abandoned_numbers table.
                        sSQL = "DELETE abandoned_numbers " & "WHERE numbering_scheme_id = " & lNumberingScheme & " AND abandoned_number = '" & sAbandonedNumber & "'"

                        ' Execute SQL Statement
                        m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="", bStoredProcedure:=False) ', |                        lNumberRecords:=0, |                        vResultArray:=vResultArray)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            m_lReturn = RollbackTrans()
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If

                End If

            End If

            'If no abandoned number has been retrieved use NextNumber.
            If sNumber = "" Then
                'TN20010623 start - we are using next number so increment numbering_scheme

                If Not (Convert.IsDBNull(lNumberingScheme) Or Informations.IsNothing(lNumberingScheme)) And lNumberingScheme <> 0 Then

                    If (m_sMaskCode.IndexOf("U"c) >= 0) And bResetNumber Then
                        ' Clear the Database Parameters Collection
                        m_oDatabase.Parameters.Clear()

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="nNumbering_scheme_id", vValue:=lNumberingScheme, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            m_lReturn = RollbackTrans()
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="sYear_name", vValue:=sYearName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            m_lReturn = RollbackTrans()
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="nStep", vValue:=nStep, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            m_lReturn = RollbackTrans()
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="nNextNumber", vValue:=0, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            m_lReturn = RollbackTrans()
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        ' Execute SQL Statement
                        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACPolicyNumberingSchemegetandIncrementPeriodSQL, sSQLName:=ACPolicyNumberingSchemegetandIncrementPeriodName, bStoredProcedure:=True, lNumberRecords:=0)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            m_lReturn = RollbackTrans()
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        If Not Informations.IsDBNull(m_oDatabase.Parameters.Item("nNextNumber").Value) AndAlso ToSafeInteger(m_oDatabase.Parameters.Item("nNextNumber").Value) > 0 Then
                            sNumber = m_oDatabase.Parameters.Item("nNextNumber").Value
                        Else
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Call to method IncrementNumberingScheme failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="GeneratePolicyNumber")
                        End If

                        ' m_lReturn = IncrementPeriodNumberingScheme(v_lNumberingSchemeID:=lNumberingScheme, v_sYearName:=sYearName)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Log Error Message
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Call to method IncrementPeriodNumberingScheme failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="GeneratePolicyNumber")

                            m_lReturn = RollbackTrans()
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    Else

                        'Get and Update Numbering Scheme
                        ' Clear the Database Parameters Collection
                        m_oDatabase.Parameters.Clear()

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="nNumbering_scheme_id", vValue:=lNumberingScheme, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            m_lReturn = RollbackTrans()
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="nStep", vValue:=nStep, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            m_lReturn = RollbackTrans()
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="nNextNumber", vValue:=0, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            m_lReturn = RollbackTrans()
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        ' Execute SQL Statement
                        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACPolicyNumberingSchemegetandIncrementSQL, sSQLName:=ACPolicyNumberingSchemegetandIncrementName, bStoredProcedure:=True, lNumberRecords:=0)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            m_lReturn = RollbackTrans()
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        If Not Informations.IsDBNull(m_oDatabase.Parameters.Item("nNextNumber").Value) AndAlso ToSafeInteger(m_oDatabase.Parameters.Item("nNextNumber").Value) <> 0 Then
                            sNumber = m_oDatabase.Parameters.Item("nNextNumber").Value
                        Else
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Call to method IncrementNumberingScheme failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="GeneratePolicyNumber")
                        End If

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Log Error Message
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Call to method IncrementNumberingScheme failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="GeneratePolicyNumber")
                            m_lReturn = RollbackTrans()
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                End If
            End If

            m_lReturn = InsertCodeIntoNumber(m_sMaskCode, sNumber, "9", sNumberToAllocate)

            m_lReturn = CommitTrans()

            r_sGeneratedPolicyNumber = sNumberToAllocate

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            m_lReturn = RollbackTrans()

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GeneratePolicyNumber Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GeneratePolicyNumber", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function ValidatePolicyNumber(ByVal sEnteredNumber As String, ByRef sFailureReason As String, Optional ByVal lInsuranceFolderCnt As Integer = 0) As Integer
        Dim result As Integer = 0
        Dim sMaskChar, sEnteredChar As String
        Dim iNumericLength As Integer

        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If number has been generated then skip validation.
            ' Alix: No we don't, user can override the generated number!
            'If (m_bGenerate = True) Then
            '    Exit Function
            'End If

            m_sMaskCode = m_sMaskCode.Trim().ToUpper()
            sEnteredNumber = sEnteredNumber.Trim().ToUpper()

            'If there is no mask code, then the only validation required is that
            'the entered number does not already exist.
            If m_sMaskCode <> "" Then
                'Firstly, check entry is same length as mask.
                'Start - (Sankar)- (Tech Spec - VAL P14 - Policy Numbering.doc)
                If m_sMaskCode.IndexOf("I"c) >= 0 Then
                    If sEnteredNumber.Length <> m_sMaskCode.Length Then
                        If sEnteredNumber.Length <> m_sMaskCode.Length - 1 Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            sFailureReason = "Enter Policy Number in format """ & m_sMaskCode & """"
                            Return result
                        End If
                    End If
                    'End - (Sankar)- (Tech Spec - VAL P14 - Policy Numbering.doc)
                ElseIf sEnteredNumber.Length <> m_sMaskCode.Length Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    sFailureReason = "Enter Policy Number in format """ & m_sMaskCode & """"
                    Return result
                End If

                'Check each individual character of entered number is valid.
                For iPlaceHolder As Integer = 1 To m_sMaskCode.Length - 1
                    sMaskChar = (m_sMaskCode.Substring(iPlaceHolder - 1, 1))
                    sEnteredChar = (sEnteredNumber.Substring(iPlaceHolder - 1, 1))

                    Select Case (sMaskChar)
                        Case "/", "-"
                            If sEnteredChar <> sMaskChar Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                sFailureReason = "Enter Policy Number in format """ & m_sMaskCode & """"
                                Return result
                            End If

                        Case "X"
                            If (Strings.AscW(sEnteredChar(0)) <Keys.D0) Or (Strings.AscW(sEnteredChar(0)) > Keys.D9) Then
                                Select Case (sEnteredChar(0))
                                    Case "/", "-"
                                        'Pass for "-" and "/" in fixed code
                                    Case Else
                                        'Error
                                        If (Strings.AscW(sEnteredChar(0)) <Keys.A) Or (Strings.AscW(sEnteredChar(0)) > Keys.Z) Then
                                            result = gPMConstants.PMEReturnCode.PMFalse
                                            sFailureReason = "Enter Policy Number in format """ & m_sMaskCode & """"
                                            Return result
                                        End If
                                End Select

                            End If


                        Case "9"
                            iNumericLength += 1
                            If (Strings.AscW(sEnteredChar(0)) <Keys.D0) Or (Strings.AscW(sEnteredChar(0)) > Keys.D9) Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                sFailureReason = "Enter Policy Number in format """ & m_sMaskCode & """"
                                Return result
                            End If
                            'Start - (Sankar)- (Tech Spec - VAL P14 - Policy Numbering.doc)
                        Case "I"
                            If sEnteredNumber.Length = m_sMaskCode.Length Then
                                iNumericLength += 1
                                If (Strings.AscW(sEnteredChar(0)) <Keys.D0) Or (Strings.AscW(sEnteredChar(0)) > Keys.D9) Then
                                    result = gPMConstants.PMEReturnCode.PMFalse
                                    sFailureReason = "Enter Policy Number in format """ & m_sMaskCode & """"
                                    Return result
                                End If
                            Else
                                If iPlaceHolder <= sEnteredNumber.Length Then
                                    If (Strings.AscW(sEnteredChar(0)) <Keys.D0) Or (Strings.AscW(sEnteredChar(0)) > Keys.D9) Then
                                        result = gPMConstants.PMEReturnCode.PMFalse
                                        sFailureReason = "Enter Policy Number in format """ & m_sMaskCode & """"
                                        Return result
                                    End If
                                End If
                            End If
                            'End - (Sankar)- (Tech Spec - VAL P14 - Policy Numbering.doc)
                        Case Else
                            'Error

                    End Select
                Next iPlaceHolder
            End If

            'Now make sure number doesn't already exist.
            sSQL = "SELECT insurance_file_cnt FROM insurance_file " & "WHERE insurance_ref = '" & sEnteredNumber & "'"

            'Allow the number to be changed back to what it was on a previous policy version if parameter is supplied
            If lInsuranceFolderCnt > 0 Then
                sSQL = sSQL & " AND insurance_folder_cnt != " & CStr(lInsuranceFolderCnt)
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vResultArray) Then
                'Number already exists
                result = gPMConstants.PMEReturnCode.PMFalse
                sFailureReason = "Policy Number already exists."
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            sFailureReason = "Unexpected error: " & excep.Message

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidatePolicyNumber Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidatePolicyNumber", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: InsertCodeIntoMask
    '
    ' Description:
    '
    ' History:  23/09/2000  RWH     - Created.
    '           15/10/2001  JMK     - Claims Year, get rightmost characters
    ' ***************************************************************** '
    Private Function InsertCodeIntoNumber(ByRef sMask As String, ByRef sCode As String, ByRef sChar As String, ByRef sResult As String) As Integer
        Dim result As Integer = 0
        Dim icodeStart, iPos, iCodeLength As Integer
        Dim sLeft, sRight As String
        Dim sFormatStr As String = String.Empty
        Dim iCStart As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        icodeStart = (sMask.IndexOf(sChar) + 1)
        iPos = icodeStart

        Do While sMask.IndexOf(sChar, iPos - 1) <> -1
            'Do While Strings.InStr(iPos, sMask, sChar) <> 0
            iCodeLength += 1
            iPos += 1
        Loop

        If iCodeLength <= sCode.Length Then
            'JMK 15/10/2001
                                                                              If sChar = "R" Then
                sCode = sCode.Substring(sCode.Length - iCodeLength)
            ElseIf sChar = "Y" Then
                sCode = sCode.Substring(sCode.Length - iCodeLength)
                'Start - Renuka - (WPR87 Paralleling)
            ElseIf sChar = "U" Then
                sCode = sCode.Substring(sCode.Length - iCodeLength)
                'End - Renuka - (WPR87 Paralleling)
            ElseIf sChar = "N" And sMask.IndexOf("_N") >= 0 Then
                iCStart = (sMask.IndexOf("_N") + 1) 'Position of underscore
                sCode = sCode.Substring(0, iCStart - icodeStart) & "_" & Mid(sCode, (iCStart - icodeStart) + 1, iCodeLength - (iCStart - icodeStart) - 1)
            Else
                sCode = sCode.Substring(0, iCodeLength)
            End If

        Else
            If sChar = "9" Then
                'Establish format of numeric section.
                sCode = New String("0", iCodeLength - sCode.Length) & sCode
            ElseIf sChar = "I" OrElse sChar = "E" Then
                'Do Nothing as we are not doing any formatting stuff
            Else
                sCode = sCode & New String(" "c, iCodeLength - sCode.Length)
            End If
        End If

        sLeft = sResult.Substring(0, icodeStart - 1)
        sRight = Mid(sResult, icodeStart + iCodeLength)

        sResult = Trim(sLeft) & sCode & sRight

        Return result

    End Function

    '****************************************************************************************
    ' Name : IncrementNumberingScheme
    '
    ' Desc : increment numbering scheme table
    '
    ' Hist : 25 June 2001 Created - Tinny
    '****************************************************************************************
    Private Function IncrementNumberingScheme(ByVal v_lNumberingSchemeID As Integer,
                                              Optional ByVal v_bResetNumberingScheme As Boolean = False) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        If v_bResetNumberingScheme Then

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="numbering_scheme_id", vValue:=CStr(v_lNumberingSchemeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = BeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACResetNumberingSchemeSQL, sSQLName:=ACResetNumberingSchemeName, bStoredProcedure:=ACResetNumberingSchemeStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return result
            End If

            m_lReturn = CommitTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End If

        m_oDatabase.Parameters.Clear()
        m_lReturn = m_oDatabase.Parameters.Add(sName:="numbering_scheme_id", vValue:=CStr(v_lNumberingSchemeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = BeginTrans()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACIncrementNumberingSchemeSQL, sSQLName:=ACIncrementNumberingSchemeName, bStoredProcedure:=ACIncrementNumberingSchemeStored)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_lReturn = RollbackTrans()

            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = CommitTrans()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function


    '****************************************************************************************
    ' Name :  Client Numbering
    '
    ' Desc :  MIPS Client Numbering
    '
    ' Hist :  VB
    '****************************************************************************************

    Public Function GenerateClientCode(ByVal v_sPartyType As String, ByVal v_iSourceID As Integer,
                                       ByRef r_sGeneratedClientCode As String,
                                       ByRef r_sFailureReason As String,
                                       Optional ByVal v_sInitial As String = "",
                                       Optional ByVal v_sValue As String = "",
                                       Optional ByVal v_sTitle As String = "",
                                       Optional ByVal v_sTradeName As String = "",
                                       Optional ByVal v_sType As String = "") As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim lNumberingScheme As Integer
        Dim sFixedCode, bReuseAbandoned, sNumberToAllocate, sCode, sAbandonedNumber As String
        Dim iNumPos, iNumStart, iNumLength As Integer
        Dim sDescription As String = String.Empty
        Dim sFirstName As String = String.Empty
        Dim nStep As Integer
        Const kNumberSchemeID As Integer = 0
        Const kClientDesc As Integer = 1
        Const kNumberSchemeTypeCode As Integer = 2

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Interrogate product table to find numbering scheme id required.
            'Need to include select in transaction as next number may be updated
            'within it.
            m_lReturn = BeginTrans()

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=v_sPartyType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetNumberingSchemeIdsFromPartyTypeSQL, sSQLName:=ACGetNumberingSchemeIdsFromPartyTypeName, bStoredProcedure:=ACGetNumberingSchemeIdsFromPartyTypeStored, lNumberRecords:=0, vResultArray:=vResultArray)

            StripInvalidCharacters(r_sGeneratedClientCode)
            sFirstName = r_sGeneratedClientCode
            StripInvalidCharacters(v_sInitial)
            StripInvalidCharacters(v_sTradeName)
            StripInvalidCharacters(v_sValue)

            'Fields are retrieved in same order as party type codes.
            If Not (Informations.IsArray(vResultArray)) Then
                lNumberingScheme = 0
            Else
                lNumberingScheme = gPMFunctions.ToSafeInteger(CStr(vResultArray(kNumberSchemeID, 0)))
                sDescription = gPMFunctions.ToSafeString(CStr(vResultArray(kClientDesc, 0))).Trim()
            End If


            If lNumberingScheme = 0 Then
                m_bGenerate = False
                result = gPMConstants.PMEReturnCode.PMNotFound
                'Ensure transaction is not left open.
                m_lReturn = CommitTrans()
                Return result

            ElseIf gPMFunctions.ToSafeString(CStr(vResultArray(kNumberSchemeTypeCode, 0))).Trim().ToUpper() <> "CLIENT" And gPMFunctions.ToSafeString(CStr(vResultArray(kNumberSchemeTypeCode, 0))).Trim().ToUpper() <> "PARTY" Then
                m_bGenerate = False
                r_sFailureReason = "Invalid type of number scheme has been selected for " & sDescription
                m_lReturn = CommitTrans()
                Return result
            End If

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Get particular numbering scheme
            m_lReturn = m_oDatabase.Parameters.Add(sName:="language_id", vValue:=CStr(m_iLanguageID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="numbering_scheme_id", vValue:=CStr(lNumberingScheme), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetNumberingSchemeSQL, sSQLName:=ACGetNumberingSchemeName, bStoredProcedure:=ACGetNumberingSchemeStored, lNumberRecords:=0, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                m_lReturn = RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'Get Client Number Rules, setting module level Properties for use in Validate method.

            m_bGenerate = CBool(vResultArray(iNUM_SCHEME_IS_GEN, 0))
            m_sMaskCode = CStr(vResultArray(iNUM_SCHEME_MASK, 0))
            sFixedCode = CStr(vResultArray(iNUM_SCHEME_FIXED_CODE, 0))

            'sNextNumber = CStr(vResultArray(iNUM_SCHEME_NEXT_NUM, 0))
            bReuseAbandoned = CStr(vResultArray(iNUM_SCHEME_REUSE_ABANDONED, 0))
            nStep = ToSafeInteger(vResultArray(iNUM_SCHEME_STEP, 0))
            If Not m_bGenerate Then

                ' ROLLBACK THE TRANSACTION*
                ' otherwise you will end up on a two-day debugging wild goose chase like I did...
                m_lReturn = RollbackTrans()
                'GenerateClientCode = PMFalse
                r_sGeneratedClientCode = ""
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sNumberToAllocate = m_sMaskCode

            If m_sMaskCode.IndexOf("B"c) >= 0 Then

                ' Clear the Database Parameters Collection
                m_oDatabase.Parameters.Clear()

                'Get particular numbering scheme
                m_lReturn = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=CStr(v_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = RollbackTrans()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetSourceDetailSQL, sSQLName:=ACGetSourceDetailName, bStoredProcedure:=ACGetSourceDetailStored, lNumberRecords:=0, vResultArray:=vResultArray)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or Not Informations.IsArray(vResultArray) Then
                    m_lReturn = RollbackTrans()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                sCode = gPMFunctions.ToSafeString(CStr(vResultArray(1, 0))).Trim().ToUpper()

                m_lReturn = InsertCodeIntoNumber(m_sMaskCode, sCode, "B", sNumberToAllocate)

            End If

            'If number is to be generated determine format of number.
            If m_sMaskCode.IndexOf("X"c) >= 0 Then

                m_lReturn = InsertCodeIntoNumber(m_sMaskCode, sFixedCode, "X", sNumberToAllocate)

            End If

            'Account Executive, Account Handler, Executive Handler, Personal Client
            If m_sMaskCode.IndexOf("I"c) >= 0 And (v_sPartyType.ToUpper() = "CO" Or v_sPartyType.ToUpper() = "AH" Or v_sPartyType.ToUpper() = "HC" Or v_sPartyType.ToUpper() = "PC") Then
                m_lReturn = InsertCodeIntoNumber(m_sMaskCode, v_sInitial, "I", sNumberToAllocate)
            End If

            If m_sMaskCode.IndexOf("L"c) >= 0 And (v_sPartyType.ToUpper() = "PC" Or v_sPartyType.ToUpper() = "CO" Or v_sPartyType.ToUpper() = "AH" Or v_sPartyType.ToUpper() = "HC") Then
                m_lReturn = InsertCodeIntoNumber(m_sMaskCode, v_sValue, "L", sNumberToAllocate)
            End If

            If m_sMaskCode.IndexOf("F"c) >= 0 And (v_sPartyType.ToUpper() = "PC" Or v_sPartyType.ToUpper() = "CO" Or v_sPartyType.ToUpper() = "AH" Or v_sPartyType.ToUpper() = "HC") Then
                m_lReturn = InsertCodeIntoNumber(m_sMaskCode, v_sType, "F", sNumberToAllocate)
            End If

            If m_sMaskCode.IndexOf("T"c) >= 0 And (v_sPartyType.ToUpper() = "AG" Or v_sPartyType.ToUpper() = "AGG" Or v_sPartyType.ToUpper() = "CC" Or v_sPartyType.ToUpper() = "GC" Or v_sPartyType.ToUpper() = "IN" Or v_sPartyType.ToUpper() = "OTHERPARTY") Then
                m_lReturn = InsertCodeIntoNumber(m_sMaskCode, v_sTradeName.Replace(" ", ""), "T", sNumberToAllocate)
            End If

            If m_sMaskCode.IndexOf("N"c) >= 0 And (v_sPartyType.ToUpper() = "CO" Or v_sPartyType.ToUpper() = "AH" Or v_sPartyType.ToUpper() = "HC" Or v_sPartyType.ToUpper() = "PC") Then
                If v_sPartyType.ToUpper() = "PC" Then
                    m_lReturn = InsertCodeIntoNumber(m_sMaskCode, v_sValue.Replace(" ", "") &
                                v_sType.Replace(" ", ""), "N", sNumberToAllocate)

                Else
                    m_lReturn = InsertCodeIntoNumber(m_sMaskCode, v_sTitle.Replace(" ", "") &
                                v_sInitial.Replace(" ", "") &
                                v_sValue.Replace(" ", ""), "N", sNumberToAllocate)
                End If

            End If

            If m_sMaskCode.IndexOf("G"c) >= 0 And v_sPartyType = "GC" Then
                m_lReturn = InsertCodeIntoNumber(m_sMaskCode, v_sValue, "G", sNumberToAllocate)
            End If

            Dim sPartyType As String = ""
            If m_sMaskCode.IndexOf("A"c) >= 0 And (v_sPartyType.ToUpper() = "CO" Or v_sPartyType.ToUpper() = "AH" Or v_sPartyType.ToUpper() = "HC" Or v_sPartyType.ToUpper() = "AG" Or v_sPartyType.ToUpper() = "AGG" Or v_sPartyType.ToUpper() = "CC" Or v_sPartyType.ToUpper() = "GC" Or v_sPartyType.ToUpper() = "IN" Or v_sPartyType.ToUpper() = "OTHERPARTY" Or v_sPartyType.ToUpper() = "PC") Then

                If v_sPartyType.ToUpper() = "CO" Then
                    sPartyType = "EXECUTIVE"
                ElseIf v_sPartyType.ToUpper() = "AH" Then
                    sPartyType = "HANDLER"
                ElseIf v_sPartyType.ToUpper() = "HC" Then
                    sPartyType = "EXECUTIVEHANDLER"
                ElseIf v_sPartyType.ToUpper() = "AG" Then
                    sPartyType = v_sType.Replace(" ", "")
                ElseIf v_sPartyType.ToUpper() = "AGG" Then
                    sPartyType = v_sType.Replace(" ", "")
                ElseIf v_sPartyType.ToUpper() = "CC" Then
                    sPartyType = "CORPORATE"
                ElseIf v_sPartyType.ToUpper() = "GC" Then
                    sPartyType = "GROUP"
                ElseIf v_sPartyType.ToUpper() = "IN" Then
                    sPartyType = v_sType.Replace(" ", "")
                ElseIf v_sPartyType.ToUpper() = "OTHERPARTY" Then
                    sPartyType = "OTHERPARTY"
                ElseIf v_sPartyType.ToUpper() = "PC" Then
                    sPartyType = "PERSONAL"
                End If

                m_lReturn = InsertCodeIntoNumber(m_sMaskCode, sPartyType, "A", sNumberToAllocate)
            End If

            If m_sMaskCode.IndexOf("YY") >= 0 And (v_sPartyType.ToUpper() = "CO" Or v_sPartyType.ToUpper() = "AH" Or v_sPartyType.ToUpper() = "HC" Or v_sPartyType.ToUpper() = "AG" Or v_sPartyType.ToUpper() = "AGG" Or v_sPartyType.ToUpper() = "CC" Or v_sPartyType.ToUpper() = "GC" Or v_sPartyType.ToUpper() = "IN" Or v_sPartyType.ToUpper() = "OTHERPARTY" Or v_sPartyType.ToUpper() = "PC") Then
                m_lReturn = InsertCodeIntoNumber(m_sMaskCode, CStr(DateTime.Now.Year), "Y", sNumberToAllocate)
            End If

            'If number is to reuse abandoned number then use non-numeric portion to search
            'Abandoned number table.

            '** Get appropriate abandoned numbering scheme if one exists.

            Dim sNumber As String = ""
            If bReuseAbandoned = gPMConstants.PMEReturnCode.PMTrue Then
                ' Clear the Database Parameters Collection
                m_oDatabase.Parameters.Clear()

                m_lReturn = m_oDatabase.Parameters.Add(sName:="numbering_scheme_id", vValue:=CStr(lNumberingScheme), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = RollbackTrans()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAbandonedNumberingSchemeSQL, sSQLName:=ACGetAbandonedNumberingSchemeName,
                                                  bStoredProcedure:=ACGetAbandonedNumberingSchemeStored, lNumberRecords:=0,
                                                  vResultArray:=vResultArray)

                If Informations.IsArray(vResultArray) Then

                    sAbandonedNumber = CStr(vResultArray(0, 0))

                    'Need to strip numeric section out of abandoned Client number.
                    iNumPos = (m_sMaskCode.IndexOf("9"c) + 1)
                    If iNumPos <> 0 Then
                        iNumStart = iNumPos
                        iNumLength = 0
                        Do While m_sMaskCode.IndexOf("9", iNumPos - 1) <> 0
                            iNumLength += 1
                            iNumPos += 1
                        Loop
                        'Store numeric section.
                        sNumber = sAbandonedNumber.Substring(iNumStart - 1, Math.Min(sAbandonedNumber.Length, iNumLength))

                        If sNumber = "0" Then
                            sNumber = ""
                        End If

                        ' Execute SQL Statement
                        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetNumberingSchemeIdsFromPartyTypeSQL,
                                                          sSQLName:=ACGetNumberingSchemeIdsFromPartyTypeName,
                                                          bStoredProcedure:=ACGetNumberingSchemeIdsFromPartyTypeStored)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            m_lReturn = RollbackTrans()
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                End If
            End If


            'If no abandoned number has been retrieved use NextNumber.
            If sNumber = "" Then
                ' sNumber = sNextNumber
                If lNumberingScheme <> 0 Then

                    'Get and Update Numbering Scheme
                    ' Clear the Database Parameters Collection
                    m_oDatabase.Parameters.Clear()

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="nNumbering_scheme_id", vValue:=lNumberingScheme, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = RollbackTrans()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="nStep", vValue:=nStep, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = RollbackTrans()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="nNextNumber", vValue:=0, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = RollbackTrans()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    If m_sMaskCode.IndexOf("9"c) >= 0 Then
                        ' Execute SQL Statement
                        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACPartyNumberingSchemegetandIncrementSQL, sSQLName:=ACPartyNumberingSchemegetandIncrementName, bStoredProcedure:=True, lNumberRecords:=0)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            m_lReturn = RollbackTrans()
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        If Not Informations.IsDBNull(m_oDatabase.Parameters.Item("nNextNumber").Value) AndAlso ToSafeInteger(m_oDatabase.Parameters.Item("nNextNumber").Value) <> 0 Then
                            sNumber = m_oDatabase.Parameters.Item("nNextNumber").Value
                        Else
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Call to method IncrementNumberingScheme failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="GeneratePolicyNumber")
                        End If
                    End If
                End If
            End If

            If m_sMaskCode.IndexOf("9"c) >= 0 Then
                m_lReturn = InsertCodeIntoNumber(m_sMaskCode, sNumber, "9", sNumberToAllocate)
            End If

            m_lReturn = CommitTrans()

            r_sGeneratedClientCode = sNumberToAllocate.Replace(" ", "")

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            m_lReturn = RollbackTrans()

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GenerateClientCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateClientCode", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function




    '================================
    '
    '================================
    '****************************************************************************************
    ' Name :  Case Numbering
    '
    ' Desc :  PLICO24-28 Case Numbering
    '
    ' Hist :  VB
    '****************************************************************************************

    Public Function GenerateCaseCode(ByVal v_iSourceID As Integer, ByRef r_sGeneratedCaseCode As String, ByRef r_sFailureReason As String, Optional ByVal v_iClaimId As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim lNumberingScheme As Integer
        Dim sNextNumber, sFixedCode, bReuseAbandoned, sNumberToAllocate, sCode, sNumber, sAbandonedNumber As String
        Dim iNumPos, iNumStart, iNumLength As Integer
        Dim sValue As String = ""
        Dim sStateCode As String = ""
        Dim iDigitStart As Integer
        Dim sOldNumber As String = ""

        Const kCaseNumberingSchemeOption As Integer = 5031

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=kCaseNumberingSchemeOption, r_sOptionValue:=sValue, v_iSourceID:=m_iSourceID)

            If sValue = "" Or sValue = "0" Then
                m_bGenerate = False
                result = gPMConstants.PMEReturnCode.PMNotFound
                m_lReturn = CommitTrans()
                Return result
            Else
                lNumberingScheme = gPMFunctions.ToSafeInteger(sValue)
            End If

            m_lReturn = BeginTrans()

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Get particular numbering scheme
            m_lReturn = m_oDatabase.Parameters.Add(sName:="language_id", vValue:=CStr(m_iLanguageID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="numbering_scheme_id", vValue:=CStr(lNumberingScheme), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetNumberingSchemeSQL, sSQLName:=ACGetNumberingSchemeName, bStoredProcedure:=ACGetNumberingSchemeStored, lNumberRecords:=0, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                m_lReturn = RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get Client Number Rules, setting module level Properties for use in Validate method.

            m_bGenerate = CBool(vResultArray(iNUM_SCHEME_IS_GEN, 0))

            m_sMaskCode = CStr(vResultArray(iNUM_SCHEME_MASK, 0))

            sFixedCode = CStr(vResultArray(iNUM_SCHEME_FIXED_CODE, 0))

            sNextNumber = CStr(vResultArray(iNUM_SCHEME_NEXT_NUM, 0))

            bReuseAbandoned = CStr(vResultArray(iNUM_SCHEME_REUSE_ABANDONED, 0))

            If Not m_bGenerate Then

                ' ROLLBACK THE TRANSACTION*
                ' otherwise you will end up on a two-day debugging wild goose chase like I did...
                m_lReturn = RollbackTrans()
                'GenerateClientCode = PMFalse
                r_sGeneratedCaseCode = ""
                Return result
            End If

            If m_sMaskCode.IndexOf("9") > 0 And v_iClaimId > 0 Then
                If r_sGeneratedCaseCode IsNot Nothing AndAlso r_sGeneratedCaseCode <> "" Then

                    iDigitStart = m_sMaskCode.IndexOf("9")
                    While Char.IsDigit(r_sGeneratedCaseCode.Chars(iDigitStart))
                        sOldNumber = sOldNumber + r_sGeneratedCaseCode.Chars(iDigitStart)
                        iDigitStart = iDigitStart + 1
                    End While
                    sNextNumber = sOldNumber
                End If
            End If

            sNumberToAllocate = m_sMaskCode

            If m_sMaskCode.IndexOf("B"c) >= 0 Then

                ' Clear the Database Parameters Collection
                m_oDatabase.Parameters.Clear()

                'Get particular numbering scheme
                m_lReturn = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=CStr(v_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = RollbackTrans()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetSourceDetailSQL, sSQLName:=ACGetSourceDetailName, bStoredProcedure:=ACGetSourceDetailStored, lNumberRecords:=0, vResultArray:=vResultArray)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or Not Informations.IsArray(vResultArray) Then
                    m_lReturn = RollbackTrans()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                sCode = gPMFunctions.ToSafeString(CStr(vResultArray(1, 0))).Trim().ToUpper()

                m_lReturn = InsertCodeIntoNumber(m_sMaskCode, sCode, "B", sNumberToAllocate)

            End If

            'If number is to be generated determine format of number.
            If m_sMaskCode.IndexOf("X"c) >= 0 Then

                m_lReturn = InsertCodeIntoNumber(m_sMaskCode, sFixedCode, "X", sNumberToAllocate)

            End If

            If m_sMaskCode.IndexOf("Y"c) >= 0 Then
                'developer guide no. 40
                m_lReturn = InsertCodeIntoNumber(m_sMaskCode, DateTime.Today, "Y", sNumberToAllocate)

            End If

            'Start - Enhancement PM041940 - WPR63_Add the State to the Numbering Scheme (Jai 21/04/2015)
            If m_sMaskCode.IndexOf("E"c) >= 0 Then
                If v_iClaimId > 0 Then
                    m_oDatabase.Parameters.Clear()
                    vResultArray = Nothing

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="Party_Cnt", vValue:=0, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_Id", vValue:=v_iClaimId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                    ' Execute stored procedure
                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetStateCodeForPartySQL, sSQLName:=ACGetStateCodeForPartyName, bStoredProcedure:=ACGetStateCodeForPartyStored, lNumberRecords:=0, vResultArray:=vResultArray)

                    If (Informations.IsArray(vResultArray)) Then
                        sStateCode = gPMFunctions.ToSafeString(vResultArray.GetValue(0, 0), "").Trim.ToUpper
                    End If
                    If sStateCode.Length > 2 Then
                        sStateCode = sStateCode.Substring(0, 2)
                    End If
                End If
                If Trim(sStateCode) = "" Then
                    sStateCode = "NA"
                End If
                m_lReturn = InsertCodeIntoNumber(m_sMaskCode, sStateCode, "E", sNumberToAllocate)
            End If
            'End - Enhancement PM041940 - WPR63_Add the State to the Numbering Scheme (Jai 21/04/2015)

            'If number is to reuse abandoned number then use non-numeric portion to search
            'Abandoned number table.
            '** Get appropriate abandoned numbering scheme if one exists.
            sNumber = ""
            If bReuseAbandoned = gPMConstants.PMEReturnCode.PMTrue Then
                ' Clear the Database Parameters Collection
                m_oDatabase.Parameters.Clear()

                m_lReturn = m_oDatabase.Parameters.Add(sName:="numbering_scheme_id", vValue:=CStr(lNumberingScheme), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = RollbackTrans()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAbandonedNumberingSchemeSQL, sSQLName:=ACGetAbandonedNumberingSchemeName, bStoredProcedure:=ACGetAbandonedNumberingSchemeStored, lNumberRecords:=0, vResultArray:=vResultArray)

                If Informations.IsArray(vResultArray) Then

                    sAbandonedNumber = CStr(vResultArray(0, 0))

                    'Need to strip numeric section out of abandoned Client number.
                    iNumPos = (m_sMaskCode.IndexOf("9"c) + 1)
                    If iNumPos <> 0 Then
                        iNumStart = iNumPos
                        iNumLength = 0
                        'InStr(1, sWhereClause, "Effective_date") > 0	(sWhereClause.IndexOf("Effective_date")) > 0
                        Do While m_sMaskCode.IndexOf("9", iNumPos - 1) <> 0
                            iNumLength += 1
                            iNumPos += 1
                        Loop
                        'Store numeric section.
                        sNumber = sAbandonedNumber.Substring(iNumStart - 1, Math.Min(sAbandonedNumber.Length, iNumLength))

                        If sNumber = "0" Then
                            sNumber = ""
                        End If

                        ' Execute SQL Statement
                        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetNumberingSchemeIdsFromPartyTypeSQL, sSQLName:=ACGetNumberingSchemeIdsFromPartyTypeName, bStoredProcedure:=ACGetNumberingSchemeIdsFromPartyTypeStored)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            m_lReturn = RollbackTrans()
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If

                End If

            End If

            'If no abandoned number has been retrieved use NextNumber.
            If sNumber = "" Then
                sNumber = sNextNumber

                'we are using next number so increment numbering_scheme

                If Not (Convert.IsDBNull(lNumberingScheme) Or Informations.IsNothing(lNumberingScheme)) And lNumberingScheme <> 0 Then
                    m_lReturn = IncrementNumberingScheme(v_lNumberingSchemeID:=lNumberingScheme)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

            End If

            m_lReturn = InsertCodeIntoNumber(m_sMaskCode, sNumber, "9", sNumberToAllocate)

            m_lReturn = CommitTrans()

            r_sGeneratedCaseCode = sNumberToAllocate.Replace(" ", "")

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            m_lReturn = RollbackTrans()

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GenerateCaseCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateCaseCode", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GenerateCCShortname
    '
    ' Parameters: n/a
    '
    ' Description: Determines the corporate clients shortname
    '
    ' History:
    '           Created : MEvans : 12-02-2008 : SAM Client Numbering
    ' ***************************************************************** '
    Public Function GenerateCCShortname(ByVal v_lSourceID As Integer, ByVal v_sTradingName As String, ByRef r_sShortName As String, Optional ByVal v_fName As String = "") As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GenerateCCShortname"

        Dim sFailureReason As String = String.Empty
        Dim sGeneratedClientCode, sTradingName As String

        Try


            sGeneratedClientCode = ""
            result = gPMConstants.PMEReturnCode.PMTrue

            sTradingName = v_sTradingName.Trim()

            If sTradingName <> "" Then

                sTradingName = sTradingName.Replace(" LTD.", "")
                sTradingName = sTradingName.Replace(" LTD", "")
                sTradingName = sTradingName.Replace(" LIMITED", "")
                sTradingName = sTradingName.Replace(" CO.", "")
                sTradingName = sTradingName.Replace(" CO", "")
                sTradingName = sTradingName.Replace(" COMPANY", "")
                sTradingName = sTradingName.Replace(" PLC.", "")
                sTradingName = sTradingName.Replace(" PLC", "")
                sTradingName = sTradingName.Replace(" SON", "")
                sTradingName = sTradingName.Replace(" SONS", "")
                sTradingName = sTradingName.Replace(" DAUGHTER", "")
                sTradingName = sTradingName.Replace(" DAUGHTERS", "")
                sTradingName = sTradingName.Replace(" ASSOC.", "")
                sTradingName = sTradingName.Replace(" ASSOC", "")
                sTradingName = sTradingName.Replace(" ASSOCIATION", "")
                sTradingName = sTradingName.Replace(" CLUB", "")

                StripInvalidCharacters(sTradingName)

            End If

            ' Generate Client Code
            m_lReturn = GenerateClientCode(gSIRLibrary.SIRPartyTypeCorporateClient, v_lSourceID, sGeneratedClientCode, sFailureReason, "", v_sTradeName:=sTradingName, v_sType:=v_fName)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or (sGeneratedClientCode = "") Or (sFailureReason <> "") Then
                gPMFunctions.RaiseError(kMethodName, "GenerateClientCode Failed : " & sFailureReason, gPMConstants.PMELogLevel.PMLogError)
            End If

            r_sShortName = sGeneratedClientCode

            Return result
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
    ' Name: GeneratePCShortname
    '
    ' Parameters: n/a
    '
    ' Description: Determines the personal clients shortname
    '
    ' History:
    '           Created : MEvans : 12-02-2008 : SAM Client Numbering
    ' ***************************************************************** '
    Public Function GeneratePCShortname(ByVal v_lSourceID As Integer, ByVal v_sSurName As String, ByVal v_sInitials As String, ByRef r_sShortName As String, Optional ByVal v_fName As String = "") As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GeneratePCShortname"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sGeneratedClientCode As String = String.Empty
        Dim sFailureReason As String = String.Empty

        Try

            sGeneratedClientCode = ""

            result = gPMConstants.PMEReturnCode.PMTrue

            'Restrict characters for the code
            StripInvalidCharacters(v_sSurName)
            StripInvalidCharacters(v_sInitials)

            ' Generate Client Code
            lReturn = CType(GenerateClientCode(gSIRLibrary.SIRPartyTypePersonalClient, v_lSourceID, sGeneratedClientCode, sFailureReason, v_sInitials, v_sSurName, v_sType:=v_fName), gPMConstants.PMEReturnCode)

            If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or (sGeneratedClientCode = "") Or (sFailureReason <> "") Then
                gPMFunctions.RaiseError(kMethodName, "GenerateClientCode Failed : " & sFailureReason, gPMConstants.PMELogLevel.PMLogError)
            End If

            r_sShortName = sGeneratedClientCode

            Return result
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
    ' Name: StripInvalidCharacters
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 12-02-2008 : SAM Client Numbering
    ' ***************************************************************** '
    Private Sub StripInvalidCharacters(ByRef r_sShortName As String)
        r_sShortName = r_sShortName.Trim()
        'r_sShortName = r_sShortName.Replace(" ", "")
        'r_sShortName = r_sShortName.Replace("'", "")
        'r_sShortName = r_sShortName.Replace("|", "")
        'r_sShortName = r_sShortName.Replace(",", "")

        ''These characters are not allowed for Sharepoint folders
        'r_sShortName = r_sShortName.Replace("~", "")
        'r_sShortName = r_sShortName.Replace(ChrW(34), "")
        'r_sShortName = r_sShortName.Replace("#", "")
        'r_sShortName = r_sShortName.Replace("%", "")
        'r_sShortName = r_sShortName.Replace("&", "")
        'r_sShortName = r_sShortName.Replace("*", "")
        'r_sShortName = r_sShortName.Replace(":", "")
        'r_sShortName = r_sShortName.Replace("<", "")
        'r_sShortName = r_sShortName.Replace(">", "")
        'r_sShortName = r_sShortName.Replace("?", "")
        'r_sShortName = r_sShortName.Replace("/", "")
        'r_sShortName = r_sShortName.Replace("\", "")
        'r_sShortName = r_sShortName.Replace("{", "")
        'r_sShortName = r_sShortName.Replace("}", "")
        'r_sShortName = r_sShortName.Replace(".", "")

        r_sShortName = Regex.Replace(r_sShortName, "[^0-9a-zA-Z]", "")

    End Sub


    '****************************************************************************************
    ' Name :  Case Numbering
    '
    ' Desc :  PLICO24-28 Case Numbering
    '
    ' Hist :  VB
    '****************************************************************************************

    Public Function SendClientReadOnlyDetails(ByVal v_sPartyType As String, ByRef r_bIsReadOnly As Boolean, ByRef r_bIsNumberingSchemeExists As Boolean, Optional ByRef r_sMaskCode As String = "") As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = BeginTrans()

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=v_sPartyType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetIsReadOnlyTypeSQL, sSQLName:=ACGetIsReadOnlyName, bStoredProcedure:=ACGetIsReadOnlyTypeStored, lNumberRecords:=0, vResultArray:=vResultArray)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not (Informations.IsArray(vResultArray)) Then
                r_bIsReadOnly = False
                r_bIsNumberingSchemeExists = False
                r_sMaskCode = ""
            Else

                r_bIsReadOnly = gPMFunctions.ToSafeInteger(CStr(vResultArray(knIsReadOnly, 0)))
                r_bIsNumberingSchemeExists = True

                r_sMaskCode = gPMFunctions.ToSafeString(CStr(vResultArray(knMaskCode, 0)))
            End If

            m_lReturn = CommitTrans()

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            'Log
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SendClientReadOnlyDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SendClientReadOnlyDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMFalse

        End Try
    End Function

    ''Start (Saurabh Agrawal) Tech Spec VAL P14 Policy Numbering (5.2.1.2)
    'Renuka - (WPR87 Paralleling) - Added optional parameter v_dtTransactionDate
    Public Function GenerateRenewalPolicyNumber(ByVal v_iPolicy_cnt As Integer, ByVal v_lBusinessType As Integer, ByVal v_iBranch As Integer, ByVal v_lProductId As Integer, ByVal v_lAgent As Integer, ByRef r_sGeneratedPolicyNumber As String, ByRef r_bChanged As Boolean, Optional ByVal v_sLossYear As String = "", Optional ByVal v_sReportedYear As String = "", Optional ByVal v_dtTransactionDate As Date = #12/30/1899#, Optional ByVal v_lPartyCnt As Integer = 0) As Integer
        Dim result As Integer = 0
        Dim oACTPeriod As bACTPeriod.Form

        Const kMethodName As String = "GenerateRenewalPolicyNumber"
        Const iDefaultIndex As Integer = 0
        Const iHistoryMaskIndex As Integer = 0
        'Start - Renuka - (WPR87 Paralleling)
        Const iNextNumberIndex As Integer = 2
        'End - Renuka - (WPR87 Paralleling)

        ''Declare the variables to be used.
        Dim r_vResultArray(,) As Object = Nothing
        Dim lNumberingSchemeID As Integer
        Dim sMask, sNumberToAllocate, sCode, sMask9 As String
        Dim sNumberingScheme As String = String.Empty
        Dim iPos, iCodeLength, icodeStart As Integer
        Dim sMaskI As String
        Dim sFixedCode As String = String.Empty
        Dim iCodeI As Integer
        'Start - Renuka - (WPR87 Paralleling)
        Dim bResetNumber As Boolean
        'End - Renuka - (WPR87 Paralleling)
        Dim sStateCode As String = ""
        Dim vResultArray As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            ''Call spu_get_prod_auto_num_ids to get the current numbering scheme Id.
            m_oDatabase.Parameters.Clear()

            bPMAddParameter.AddParameterLite(m_oDatabase, "product_id", v_lProductId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetNumberingSchemeIdsFromProductSQL, sSQLName:=ACGetNumberingSchemeIdsFromProductName, bStoredProcedure:=ACGetNumberingSchemeFromProductStored, lNumberRecords:=0, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "call to procedure spu_get_prod_auto_num_ids  Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            ''Exit If no numbering scheme attached.
            If Not Informations.IsArray(r_vResultArray) Then
                Return result
            Else

                lNumberingSchemeID = gPMFunctions.ToSafeLong(CStr(r_vResultArray(iProd_Num_Scheme_id, iDefaultIndex)))

                sCode = CStr(r_vResultArray(iProd_Num_Scheme_code, iDefaultIndex)).Trim().ToUpper()

                ''Exit if numbering schemeID = 0
                If lNumberingSchemeID = 0 Then
                    'Start Girija --- PN 55045
                    Return result
                    'RaiseError kMethodName, "GenerateRenewalPolicyNumber Method Failed", PMLogError
                    'End Girija --- PN 55045
                End If

            End If

            ''Get the numbering scheme mask

            m_oDatabase.Parameters.Clear()
            bPMAddParameter.AddParameterLite(m_oDatabase, "language_id", m_iLanguageID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "numbering_scheme_id", lNumberingSchemeID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetNumberingSchemeSQL, sSQLName:=ACGetNumberingSchemeName, bStoredProcedure:=ACGetNumberingSchemeStored, lNumberRecords:=0, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Call to spu_numbering_scheme_saa Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ''Exit if no mask found.
            If Not Informations.IsArray(r_vResultArray) Then
                gPMFunctions.RaiseError(kMethodName, "call to spu_numbering_scheme_saa procedure Failed", gPMConstants.PMELogLevel.PMLogError)
            Else

                sNumberingScheme = CStr(r_vResultArray(iNUM_SCHEME_MASK, iDefaultIndex))

                sFixedCode = CStr(r_vResultArray(iNUM_SCHEME_FIXED_CODE, iDefaultIndex))
                'Start - Renuka - (WPR87 Paralleling)

                bResetNumber = gPMFunctions.ToSafeBoolean(CStr(r_vResultArray(iNUM_IS_RESET_NUMBER, iDefaultIndex)))
                'End - Renuka - (WPR87 Paralleling)
            End If
            ''Exit If numbering scheme does not contains "I"

            If (sNumberingScheme.IndexOf("I"c) + 1) = 0 Then
                Return result
            End If
            ''Get the numbering scheme history on that product

            bPMAddParameter.AddParameterLite(m_oDatabase, "policy_cnt", v_iPolicy_cnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "numbering_scheme_id", lNumberingSchemeID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetNumberingSchemeHistorySQL, sSQLName:=ACGetNumberingSchemeHistoryName, bStoredProcedure:=ACGetNumberingSchemeHistoryStored, lNumberRecords:=0, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Call to spu_numbering_scheme_history_sel  Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ''If history exists generate the new policy number from history scheme and latest scheme.
            Dim lPeriodId As Integer
            Dim sYearName As String = String.Empty
            Dim sNextNumber As String = String.Empty
            If Informations.IsArray(r_vResultArray) Then

                sMask = CStr(r_vResultArray(iHistoryMaskIndex, iDefaultIndex))
                sNumberToAllocate = sNumberingScheme
                ''Insert product code to the current scheme.
                If sNumberingScheme.IndexOf("P"c) >= 0 Then
                    m_lReturn = InsertCodeIntoNumber(sNumberingScheme, sCode, "P", sNumberToAllocate)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "Call to InsertCodeIntoNumber Method Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If
                ''Insert Fixed Code to the current Scheme
                If sNumberingScheme.IndexOf("X"c) >= 0 Then
                    m_lReturn = InsertCodeIntoNumber(sNumberingScheme, sFixedCode, "X", sNumberToAllocate)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "Call to InsertCodeIntoNumber Method Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If
                ''Insert branch code to the new scheme
                If sNumberingScheme.IndexOf("B"c) >= 0 Then
                    bPMAddParameter.AddParameterLite(m_oDatabase, "sourceid", v_iBranch, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, True)

                    bPMAddParameter.AddParameterLite(m_oDatabase, "code", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMString)


                    ' Execute SQL Statement
                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetBranchcode, sSQLName:=ACGetBranchCodeName, bStoredProcedure:=ACGetBranchCodeStored, lNumberRecords:=0)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "Call to spu_SAM_Get_Branch_Code Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    If m_oDatabase.Parameters.Item("code").Value <> "" Then
                        sCode = m_oDatabase.Parameters.Item("code").Value.ToUpper()
                    Else
                        gPMFunctions.RaiseError(kMethodName, "Call to spu_SAM_Get_Branch_Code Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    m_lReturn = InsertCodeIntoNumber(sNumberingScheme, sCode, "B", sNumberToAllocate)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "Call to InsertCodeIntoNumber Method Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                    'Start - Renuka - (WPR87 Paralleling)
                    'Insert Accounting period to the numbering scheme
                End If
                If (sMask.IndexOf("U"c) >= 0 AndAlso sMask = sNumberingScheme) Or sNumberingScheme.IndexOf("U"c) >= 0 Then

                    'Create business object of bACTPeriod

                    oACTPeriod = New bACTPeriod.Form
                    m_lReturn = oACTPeriod.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                    ' Check for errors.
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "Failed to get an instance of the business object bACTPeriod.Form", gPMConstants.PMELogLevel.PMLogError)
                    End If


                    m_lReturn = oACTPeriod.GetPeriodForDate(dtDateInPeriod:=v_dtTransactionDate, lPeriodId:=lPeriodId, vYearName:=sYearName, v_bIncludeClosed:=True)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "Call to method bACTPeriod.Form.GetPeriodForDate failed.", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    m_lReturn = InsertCodeIntoNumber(sNumberingScheme, sYearName.Trim(), "U", sNumberToAllocate)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "Call to method InsertCodeIntoNumber failed.", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    ' Terminate the business object

                    oACTPeriod.Dispose()
                    ' Destroy the instance of the business object from memory.
                    oACTPeriod = Nothing
                End If


                If sNumberingScheme.IndexOf("Y"c) >= 0 Then
                    'developer guide no. 40
                    m_lReturn = InsertCodeIntoNumber(sNumberingScheme, DateTime.Today, "Y", sNumberToAllocate)

                End If
                'Extract value for mask U

                'Start - Enhancement PM041940 - WPR63_Add the State to the Numbering Scheme (Jai 21/04/2015)
                'PM044396 Jai Prakash 27 Nov 2015
                If sMask.IndexOf("E"c) >= 0 Then
                    If v_lPartyCnt > 0 Then
                        m_oDatabase.Parameters.Clear()
                        vResultArray = Nothing

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="Party_Cnt", vValue:=v_lPartyCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_Id", vValue:=0, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                        ' Execute stored procedure
                        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetStateCodeForPartySQL, sSQLName:=ACGetStateCodeForPartyName, bStoredProcedure:=ACGetStateCodeForPartyStored, lNumberRecords:=0, vResultArray:=vResultArray)

                        If (Informations.IsArray(vResultArray)) Then
                            sStateCode = gPMFunctions.ToSafeString(vResultArray.GetValue(0, 0), "").Trim.ToUpper
                        End If
                        If sStateCode.Length > 2 Then
                            sStateCode = sStateCode.Substring(0, 2)
                        End If
                    End If
                    If Trim(sStateCode) = "" Then
                        sStateCode = "NA"
                    End If
                    m_lReturn = InsertCodeIntoNumber(sNumberingScheme, sStateCode, "E", sNumberToAllocate)
                ElseIf sNumberingScheme.IndexOf("-EE") >= 0 Then
                    sNumberToAllocate = sNumberToAllocate.Replace("-EE", "")
                    sNumberingScheme = sNumberingScheme.Replace("-EE", "")
                ElseIf sNumberingScheme.IndexOf("EE-") >= 0 Then
                    sNumberToAllocate = sNumberToAllocate.Replace("EE-", "")
                    sNumberingScheme = sNumberingScheme.Replace("EE-", "")
                End If
                'End - Enhancement PM041940 - WPR63_Add the State to the Numbering Scheme (Jai 21/04/2015)

                If (sNumberingScheme.IndexOf("U"c) >= 0) And bResetNumber Then
                    ' Clear the Database Parameters Collection
                    m_oDatabase.Parameters.Clear()

                    bPMAddParameter.AddParameterLite(m_oDatabase, "numbering_scheme_id", lNumberingSchemeID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "year_name", sYearName, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                    ' Execute SQL Statement
                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPeiodNextNumberSQL, sSQLName:=ACGetPeiodNextNumberName, bStoredProcedure:=ACGetPeiodNextNumberStored, lNumberRecords:=0, vResultArray:=r_vResultArray)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "Call to procedure " & ACGetPeiodNextNumberSQL & " failed.", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    If Not Informations.IsArray(r_vResultArray) Then
                        gPMFunctions.RaiseError(kMethodName, "Call to procedure " & ACGetPeiodNextNumberSQL & " failed.", gPMConstants.PMELogLevel.PMLogError)
                    Else

                        sNextNumber = gPMFunctions.ToSafeString(CStr(r_vResultArray(iNextNumberIndex, iDefaultIndex)))
                        m_lReturn = InsertCodeIntoNumber(sNumberingScheme, sNextNumber, "U", sNumberToAllocate)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "Call to method InsertCodeIntoNumber failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                    End If
                End If
                'End - Renuka - (WPR87 Paralleling)
                ''Extract value for mask 9999..
                If sMask.Length > sNumberingScheme.Length Then
                    sMask = sNumberingScheme
                End If
                If sMask.IndexOf("9"c) >= 0 Then


                    iPos = (sMask.IndexOf("9"c) + 1)
                    icodeStart = iPos

                    Dim idx As Integer = sMask.IndexOf("9", iPos - 1)
                    Do While idx > -1
                        iCodeLength += 1
                        iPos += 1
                        idx = sMask.IndexOf("9", iPos - 1)
                    Loop

                    sMask9 = r_sGeneratedPolicyNumber.Substring(icodeStart - 1, Math.Min(r_sGeneratedPolicyNumber.Length, iCodeLength))

                    Dim dbNumericTemp As Double
                    If Double.TryParse(sMask9, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                        m_lReturn = InsertCodeIntoNumber(sNumberingScheme, sMask9, "9", sNumberToAllocate)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "Call to InsertCodeIntoNumber Method Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                    End If
                    ''Get value for mask "II"
                    iPos = 0
                    iCodeLength = 0
                    icodeStart = 0
                    If sMask.IndexOf("I"c) >= 0 Then
                        iPos = (sMask.IndexOf("I"c) + 1)
                        icodeStart = iPos

                        Dim idxI As Integer = sMask.IndexOf("I", iPos - 1)
                        Do While idxI > -1
                            iCodeLength += 1
                            iPos += 1
                            idxI = sMask.IndexOf("I", iPos - 1)
                        Loop
                        Do While (icodeStart - 1 + Math.Min(r_sGeneratedPolicyNumber.Length, iCodeLength) > r_sGeneratedPolicyNumber.Length)
                            iCodeLength = iCodeLength - 1
                        Loop
                        ' sMaskI = r_sGeneratedPolicyNumber.Substring(icodeStart - 1, Math.Min(r_sGeneratedPolicyNumber.Length, iCodeLength))
                        sMaskI = r_sGeneratedPolicyNumber.Substring(icodeStart - 1, Math.Min(r_sGeneratedPolicyNumber.Length, iCodeLength))
                        ''Increment I by 1

                        Dim dbNumericTemp2 As Double
                        If Double.TryParse(sMaskI, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                            iCodeI = gPMFunctions.ToSafeInteger(sMaskI)
                            iCodeI += 1
                        End If
                    Else
                        If sMask.EndsWith("X") And r_sGeneratedPolicyNumber.Substring(sMask.Length - 1, 1) = "0" Then
                            iCodeI = 1
                        Else
                            iCodeI = 0
                        End If
                    End If
                    sMaskI = gPMFunctions.ToSafeString(CStr(iCodeI))
                    'Insert value of I to the new number
                    m_lReturn = InsertCodeIntoNumber(sNumberingScheme, sMaskI, "I", sNumberToAllocate)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "Call to InsertCodeIntoNumber Method Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    r_sGeneratedPolicyNumber = sNumberToAllocate
                    r_bChanged = True
                Else
                    Return result
                End If

            Else
                'Start - Renuka - (WPR87 Paralleling)
                m_lReturn = GeneratePolicyNumber(v_lBusinessType:=v_lBusinessType, v_iBranch:=v_iBranch, v_lProductId:=v_lProductId, v_lAgent:=v_lAgent, r_sGeneratedPolicyNumber:=r_sGeneratedPolicyNumber, v_dtTransactionDate:=v_dtTransactionDate, v_lPartyCnt:=v_lPartyCnt)
                'End - Renuka - (WPR87 Paralleling)
                'EH011797 Jai Prakash 07 Aug 2015
                r_bChanged = True
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Call to GeneratePolicyNumber Method Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            End If

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally


        End Try
        Return result

    End Function
    ''End (Saurabh Agrawal) Tech Spec VAL P14 Policy Numbering (5.2.1.2)
    '(Start)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(5.4.1.2)
    Public Function GenerateMediaReference(ByVal v_iSourceID As Integer, ByVal v_iNumberingScheme As Integer, ByRef r_sGeneratedMediaRef As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GenerateMediaReference"


        Dim vResultArray(,) As Object = Nothing
        Dim bResetDaily As Boolean
        Dim dLastGenerated As Date
        Dim sCode, sNumber As String
        'Dim bResetNumberingScheme As String    'PN 62476

        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="language_id", vValue:=CStr(m_iLanguageID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="numbering_scheme_id", vValue:=CStr(v_iNumberingScheme), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetNumberingSchemeSQL, sSQLName:=ACGetNumberingSchemeName, bStoredProcedure:=ACGetNumberingSchemeStored, lNumberRecords:=0, vResultArray:=vResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "The stored procedure" & ACGetNumberingSchemeSQL & " failed to fetch the record", gPMConstants.PMELogLevel.PMLogError)
        End If

        If Not Informations.IsArray(vResultArray) Then
            gPMFunctions.RaiseError(kMethodName, "No Record Exist for this Numbering Scheme", gPMConstants.PMELogLevel.PMLogError)
        End If


        m_sMaskCode = CStr(vResultArray(iNUM_SCHEME_MASK, 0))

        Dim sFixedCode As String = CStr(vResultArray(iNUM_SCHEME_FIXED_CODE, 0))

        Dim sNextNumber As String = CStr(vResultArray(iNUM_SCHEME_NEXT_NUM, 0))
        Dim auxVar As Object = vResultArray(iNUM_SCHEME_RESET_DAILY, 0)




        If CStr(vResultArray(iNUM_SCHEME_RESET_DAILY, 0)) = "" Or Object.Equals(vResultArray(iNUM_SCHEME_RESET_DAILY, 0), Nothing) Or Convert.IsDBNull(auxVar) Or Informations.IsNothing(auxVar) Then
            bResetDaily = False
        Else

            bResetDaily = CBool(vResultArray(19, 0))
        End If




        If vResultArray(iNUM_SCHEME_DATE_LAST_GENERATED, 0) Is DBNull.Value Or CStr(vResultArray(iNUM_SCHEME_DATE_LAST_GENERATED, 0)) = "" Then
            dLastGenerated = DateTime.FromOADate(0)
        Else

            dLastGenerated = CDate(vResultArray(iNUM_SCHEME_DATE_LAST_GENERATED, 0))
        End If

        Dim sNumberToAllocate As String = m_sMaskCode

        If m_sMaskCode.IndexOf("B"c) >= 0 Then

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=CStr(v_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetSourceDetailSQL, sSQLName:=ACGetSourceDetailName, bStoredProcedure:=ACGetSourceDetailStored, lNumberRecords:=0, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Informations.IsArray(vResultArray) Then
                gPMFunctions.RaiseError(kMethodName, "", gPMConstants.PMELogLevel.PMLogError)
            End If


            sCode = gPMFunctions.ToSafeString(CStr(vResultArray(1, 0))).Trim().ToUpper()

            m_lReturn = InsertCodeIntoNumber(m_sMaskCode, sCode, "B", sNumberToAllocate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "InsertCodeIntoNumber method failed for the mask code" & m_sMaskCode, gPMConstants.PMELogLevel.PMLogError)
            End If
        End If

        If m_sMaskCode.IndexOf("X"c) >= 0 Then
            m_lReturn = InsertCodeIntoNumber(m_sMaskCode, sFixedCode, "X", sNumberToAllocate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "InsertCodeIntoNumber method failed for the mask code" & m_sMaskCode, gPMConstants.PMELogLevel.PMLogError)
            End If

        End If

        If m_sMaskCode.IndexOf("Y"c) >= 0 Then
            m_lReturn = InsertCodeIntoNumber(m_sMaskCode, StringsHelper.Format(DateTime.Now.Year, "0000"), "Y", sNumberToAllocate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "InsertCodeIntoNumber method failed for the mask code" & m_sMaskCode, gPMConstants.PMELogLevel.PMLogError)
            End If
        End If

        If m_sMaskCode.IndexOf("M"c) >= 0 Then
            m_lReturn = InsertCodeIntoNumber(m_sMaskCode, StringsHelper.Format(DateTime.Now.Month, "00"), "M", sNumberToAllocate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "InsertCodeIntoNumber method failed for the mask code" & m_sMaskCode, gPMConstants.PMELogLevel.PMLogError)
            End If
        End If

        If m_sMaskCode.IndexOf("D"c) >= 0 Then
            m_lReturn = InsertCodeIntoNumber(m_sMaskCode, StringsHelper.Format(DateTime.Now.Day, "00"), "D", sNumberToAllocate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "InsertCodeIntoNumber method failed for the mask code" & m_sMaskCode, gPMConstants.PMELogLevel.PMLogError)
            End If
        End If

        'bResetNumberingScheme = False  'PN 62476


        If dLastGenerated = Nothing Or dLastGenerated = CDate("12:00:00 AM") Then
            sNumber = sNextNumber
        Else
            'developer guide no. 40
            If DateTime.Parse(DateTime.Now) > DateTime.Parse(dLastGenerated) And bResetDaily Then
                sNumber = "1"
                'bResetNumberingScheme = True   'PN 62476
            Else
                sNumber = sNextNumber
            End If
        End If

        '   Start PN 62476
        '    m_lReturn = IncrementNumberingScheme(v_lNumberingSchemeID:=v_iNumberingScheme, _
        ''                                          v_bResetNumberingScheme:=bResetNumberingScheme)
        '
        '    If m_lReturn& <> PMTrue Then
        '        RaiseError kMethodName, "IncrementNumberingScheme method failed ", PMLogError
        '    End If
        '   End PN 62476

        m_lReturn = InsertCodeIntoNumber(m_sMaskCode, sNumber, "9", sNumberToAllocate)
        m_lReturn = CommitTrans()
        r_sGeneratedMediaRef = sNumberToAllocate.Replace(" ", "")

        GoTo Finally_Renamed

        bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)

Finally_Renamed:

        Return result
    End Function
    '(End)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(5.4.1.2)
    'Start (Sriram P)Tech Spec - WR19 - Cover Note Functionality
    Public Function GetMidnightRenewalOption(ByVal v_lProductId As Integer, ByRef r_bIsMidnightRenewal As Boolean) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "GetMidnightRenewalOption"



        result = gPMConstants.PMEReturnCode.PMTrue
        Dim vResultArray(,) As Object = Nothing

        Dim sSQL As String = "SELECT is_midnight_renewal FROM product " & "WHERE product_id = " & CStr(v_lProductId)

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=vResultArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "GetMidnightRenewalOption Failed", gPMConstants.PMELogLevel.PMLogError)
            result = gPMConstants.PMEReturnCode.PMFalse
        End If


        If Informations.IsArray(vResultArray) Then

            r_bIsMidnightRenewal = (CBool(CStr(vResultArray(0, 0)).Trim()))
        End If
        GoTo Finally_Renamed



        bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)

Finally_Renamed:

        Return result

    End Function

    Public Function GetCoverNoteDefaultPeriod(ByVal v_lProductId As Integer, ByRef r_lCoverNoteDefaultPeriod As Integer) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "GetCoverNoteDefaultPeriod"



        result = gPMConstants.PMEReturnCode.PMTrue
        Dim vResultArray(,) As Object = Nothing

        Dim sSQL As String = "SELECT Cover_Note_Default_Period FROM product " & "WHERE product_id = " & CStr(v_lProductId)

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=vResultArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "GetCoverNoteDefaultPeriod Failed", gPMConstants.PMELogLevel.PMLogError)
            result = gPMConstants.PMEReturnCode.PMFalse
        End If


        If Informations.IsArray(vResultArray) Then

            r_lCoverNoteDefaultPeriod = gPMFunctions.ToSafeLong(CStr(vResultArray(0, 0)).Trim())
        End If
        GoTo Finally_Renamed



        bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)

Finally_Renamed:

        Return result

    End Function
    'End (Sriram P)Tech Spec - WR19 - Cover Note Functionality
    'Start (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section()
    Public Function GenerateCoverNoteNumber(ByVal v_lSourceID As Integer, ByVal v_lProductId As Integer, ByVal v_lAgentId As Integer, ByRef r_sGeneratedCoverNoteCode As String, ByRef r_sFailureReason As String) As Integer
        Dim Catch_Renamed As Boolean = False


        Dim result As Integer = 0
        Const kMethodName As String = "GenerateCoverNoteNumber"


        Dim lNumberingScheme As Integer
        Dim vResultArray(,) As Object = Nothing
        Dim sCode As String = String.Empty
        Dim sAbandonedNumber As String = String.Empty
        Dim sNextNumber As String = String.Empty
        '    Dim bGenerate As Boolean
        '    Dim sMask As String
        Dim sFixedCode, bReuseAbandoned, sSQL, sNumberToAllocate As String
        Dim iNumPos, iNumStart, iNumLength As Integer
        Dim sNumber As String = ""

        Try
            Catch_Renamed = True




            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = BeginTrans()

            m_oDatabase.Parameters.Clear()


            bPMAddParameter.AddParameterLite(m_oDatabase, "product_id", v_lProductId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            ''''    m_lReturn = m_oDatabase.Parameters.Add(sName:="product_id", _
            '''''                                           vValue:=v_lProductId, _
            '''''                                           iDirection:=PMParamInput, _
            '''''                                           iDataType:=PMLong)
            ''''
            ''''    If m_lReturn <> PMTrue Then
            ''''         RaiseError kMethodName, " Failed", PMLogError
            ''''    End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetNumberingSchemeIdsFromProductSQL, sSQLName:=ACGetNumberingSchemeIdsFromProductName, bStoredProcedure:=ACGetNumberingSchemeFromProductStored, lNumberRecords:=0, vResultArray:=vResultArray)

            If Not (Informations.IsArray(vResultArray)) Then
                lNumberingScheme = 0
                sCode = ""
            Else
                Dim auxVar As Object = vResultArray(ACIBCNNumberingSchemeId, 0)


                If Not (Convert.IsDBNull(auxVar) Or Informations.IsNothing(auxVar)) Then

                    lNumberingScheme = CInt(vResultArray(ACIBCNNumberingSchemeId, 0))

                    sCode = CStr(vResultArray(0, 0)).Trim().ToUpper()
                End If
            End If


            If (lNumberingScheme = 0) Or Convert.IsDBNull(lNumberingScheme) Or Informations.IsNothing(lNumberingScheme) Then
                m_bGenerate = False
                m_sMaskCode = ""

                m_lReturn = CommitTrans()
                Return result
            End If


            bPMAddParameter.AddParameterLite(m_oDatabase, "language_id", m_iLanguageID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            ''''        m_oDatabase.Parameters.Clear
            ''''
            ''''        m_lReturn = m_oDatabase.Parameters.Add(sName:="language_id", _
            '''''                                               vValue:=m_iLanguageID, _
            '''''                                               iDirection:=PMParamInput, _
            '''''                                               iDataType:=PMLong)

            ''''        If (m_lReturn& <> PMTrue) Then
            ''''            m_lReturn = RollbackTrans()
            ''''            GenerateCoverNoteNumber = PMFalse
            ''''            Exit Function
            ''''        End If

            bPMAddParameter.AddParameterLite(m_oDatabase, "numbering_scheme_id", lNumberingScheme, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)

            ''''        m_lReturn = m_oDatabase.Parameters.Add(sName:="numbering_scheme_id", _
            '''''                                               vValue:=lNumberingScheme, _
            '''''                                               iDirection:=PMParamInput, _
            '''''                                               iDataType:=PMLong)
            ''''
            ''''        If (m_lReturn& <> PMTrue) Then
            ''''            m_lReturn = RollbackTrans()
            ''''            GenerateCoverNoteNumber = PMFalse
            ''''            Exit Function
            ''''        End If


            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetNumberingSchemeSQL, sSQLName:="", bStoredProcedure:=True, lNumberRecords:=0, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                m_lReturn = RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get Policy Number Rules, setting module level Properties for use in Validate method.

            m_bGenerate = CBool(vResultArray(iNUM_SCHEME_IS_GEN, 0))

            m_sMaskCode = CStr(vResultArray(iNUM_SCHEME_MASK, 0))

            sFixedCode = CStr(vResultArray(iNUM_SCHEME_FIXED_CODE, 0))

            sNextNumber = CStr(vResultArray(iNUM_SCHEME_NEXT_NUM, 0))

            bReuseAbandoned = CStr(vResultArray(iNUM_SCHEME_REUSE_ABANDONED, 0))

            If Not m_bGenerate Then
                m_lReturn = RollbackTrans()
                r_sGeneratedCoverNoteCode = ""
                Return result
            End If

            sNumberToAllocate = m_sMaskCode

            If m_sMaskCode.IndexOf("P"c) >= 0 Then
                m_lReturn = InsertCodeIntoNumber(m_sMaskCode, sCode, "P", sNumberToAllocate)
            End If

            If m_sMaskCode.IndexOf("A"c) >= 0 Then
                sSQL = "SELECT SHORTNAME FROM party " & "WHERE party_cnt = " & CStr(v_lAgentId)

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = RollbackTrans()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                sCode = ""
                If Informations.IsArray(vResultArray) Then

                    sCode = CStr(vResultArray(0, 0)).Trim().ToUpper()
                End If

                If Not (sCode.Trim().Length > 0) Then
                    sCode = ""
                End If

                m_lReturn = InsertCodeIntoNumber(m_sMaskCode, sCode, "A", sNumberToAllocate)
            End If

            If m_sMaskCode.IndexOf("B"c) >= 0 Then
                sSQL = "SELECT code FROM source " & "WHERE source_id = " & CStr(v_lSourceID)

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = RollbackTrans()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                sCode = ""
                If Informations.IsArray(vResultArray) Then

                    sCode = CStr(vResultArray(0, 0)).Trim().ToUpper()
                End If

                m_lReturn = InsertCodeIntoNumber(m_sMaskCode, sCode, "B", sNumberToAllocate)

            End If


            If m_sMaskCode.IndexOf("X"c) >= 0 Then

                m_lReturn = InsertCodeIntoNumber(m_sMaskCode, sFixedCode, "X", sNumberToAllocate)

            End If

            If m_sMaskCode.IndexOf("Y"c) >= 0 Then
                'developer guide no. 40
                m_lReturn = InsertCodeIntoNumber(m_sMaskCode, DateTime.Today, "Y", sNumberToAllocate)

            End If

            sNumber = ""
            If bReuseAbandoned = gPMConstants.PMEReturnCode.PMTrue Then


                bPMAddParameter.AddParameterLite(m_oDatabase, "numbering_scheme_id", lNumberingScheme, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
                ''''            m_oDatabase.Parameters.Clear
                ''''
                ''''            m_lReturn = m_oDatabase.Parameters.Add(sName:="numbering_scheme_id", _
                '''''                                                   vValue:=lNumberingScheme, _
                '''''                                                   iDirection:=PMParamInput, _
                '''''                                                   iDataType:=PMString)
                ''''
                ''''            If (m_lReturn& <> PMTrue) Then
                ''''                m_lReturn = RollbackTrans()
                ''''                GenerateCoverNoteNumber = PMFalse
                ''''                Exit Function
                ''''            End If

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAbandonedNumberingSchemeSQL, sSQLName:="", bStoredProcedure:=True, lNumberRecords:=0, vResultArray:=vResultArray)

                If Informations.IsArray(vResultArray) Then

                    sAbandonedNumber = CStr(vResultArray(0, 0))

                    iNumPos = (m_sMaskCode.IndexOf("9"c) + 1)
                    If iNumPos <> 0 Then
                        iNumStart = iNumPos
                        iNumLength = 0
                        Do While m_sMaskCode.IndexOf("9", iNumPos - 1) <> 0
                            iNumLength += 1
                            iNumPos += 1
                        Loop

                        sNumber = sAbandonedNumber.Substring(iNumStart - 1, Math.Min(sAbandonedNumber.Length, iNumLength))

                        If sNumber = "0" Then
                            sNumber = ""
                        End If

                        sSQL = "DELETE abandoned_numbers " & "WHERE numbering_scheme_id = " & CStr(lNumberingScheme) & " AND abandoned_number = '" & sAbandonedNumber & "'"

                        m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="", bStoredProcedure:=False)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            m_lReturn = RollbackTrans()
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If

                End If

            End If

            'If no abandoned number has been retrieved use NextNumber.
            If sNumber = "" Then
                sNumber = sNextNumber
                'vkat
                'sSQL = "SELECT Cover_Note_reused_upto FROM Product " _
                '& "WHERE product_id = " & v_lProductId

                'm_lReturn = m_oDatabase.SQLSelect( _
                'sSQL:=sSQL, _
                'sSQLName:="", _
                'bStoredProcedure:=False, _
                'lNumberRecords:=0, _
                'vResultArray:=vResultArray)
                'If IsArray(vResultArray) Then
                '    lCNReuseCount = ToSafeLong(ZeroToNull(vResultArray(0, 0)))
                'End If


                m_lReturn = InsertCodeIntoNumber(m_sMaskCode, sNumber, "9", sNumberToAllocate)


                'sSQL = "SELECT COUNT(Risk_Cover_Note_Link_Id) FROM Risk_Cover_Note_Link " _
                '& "WHERE rtrim(Cover_Note_Ref) = '" & sNumberToAllocate & "'"

                'm_lReturn = m_oDatabase.SQLSelect( _
                'sSQL:=sSQL, _
                'sSQLName:="", _
                'bStoredProcedure:=False, _
                'lNumberRecords:=0, _
                'vResultArray:=vResultArray)
                'If IsArray(vResultArray) Then
                '    lCNConsumedCount = ToSafeLong(ZeroToNull(vResultArray(0, 0)))
                'End If



                'If Not ((lCNConsumedCount + 1) < lCNReuseCount) Then

                If Not (Convert.IsDBNull(lNumberingScheme) Or Informations.IsNothing(lNumberingScheme)) And lNumberingScheme <> 0 Then
                    m_lReturn = IncrementNumberingScheme(v_lNumberingSchemeID:=lNumberingScheme)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
                'Else
                'End If

                'vkat

            End If

            'm_lReturn = InsertCodeIntoNumber(m_sMaskCode, sNumber, "9", sNumberToAllocate)

            m_lReturn = CommitTrans()

            r_sGeneratedCoverNoteCode = sNumberToAllocate

            Return result

        Catch excep As System.Exception
            If Not Catch_Renamed Then
                Throw excep
            End If


            GoTo Finally_Renamed

            If Catch_Renamed Then


                m_lReturn = RollbackTrans()

                bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=excep)

            End If
Finally_Renamed:
            Return gPMConstants.PMEReturnCode.PMFalse
        End Try
    End Function
    'End (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section()

    'Start - Renuka - (WPR87 Paralleling)
    '****************************************************************************************
    ' Name : IncrementPeriodNumberingScheme
    '
    ' Desc : Increment period next number table
    '
    ' Hist : 31 June 2009
    '****************************************************************************************
    Private Function IncrementPeriodNumberingScheme(ByVal v_lNumberingSchemeID As Integer, ByVal v_sYearName As String) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "IncrementPeriodNumberingScheme"


        result = gPMConstants.PMEReturnCode.PMTrue


        ' Call spu_get_prod_auto_num_ids to get the current numbering scheme Id.
        m_oDatabase.Parameters.Clear()

        bPMAddParameter.AddParameterLite(m_oDatabase, "numbering_scheme_id", v_lNumberingSchemeID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
        'developer guide no.101
        bPMAddParameter.AddParameterLite(m_oDatabase, "year_name", v_sYearName, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

        m_lReturn = BeginTrans()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "Call to method BeginTrans failed.", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACIncrementPeriodNumberingSchemeSQL, sSQLName:=ACIncrementPeriodNumberingSchemeName, bStoredProcedure:=ACIncrementPeriodNumberingSchemeStored)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_lReturn = RollbackTrans()

            gPMFunctions.RaiseError(kMethodName, "Call to procedure " & ACIncrementPeriodNumberingSchemeSQL & " failed.", gPMConstants.PMELogLevel.PMLogError)
        End If

        m_lReturn = CommitTrans()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "Call to method CommitTrans failed.", gPMConstants.PMELogLevel.PMLogError)
        End If

        Return result
    End Function
    'End - Renuka - (WPR87 Paralleling)
    'Start PN: 62476
    Public Function InsertMediaReference(ByVal v_iSourceID As Integer, ByVal v_iNumberingScheme As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "InsertMediaReference"


        Dim vResultArray(,) As Object = Nothing
        Dim bResetDaily As Boolean
        Dim dLastGenerated As Date

        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="language_id", vValue:=CStr(m_iLanguageID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="numbering_scheme_id", vValue:=CStr(v_iNumberingScheme), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetNumberingSchemeSQL, sSQLName:=ACGetNumberingSchemeName, bStoredProcedure:=ACGetNumberingSchemeStored, lNumberRecords:=0, vResultArray:=vResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "The stored procedure" & ACGetNumberingSchemeSQL & " failed to fetch the record", gPMConstants.PMELogLevel.PMLogError)
        End If

        If Not Informations.IsArray(vResultArray) Then
            gPMFunctions.RaiseError(kMethodName, "No Record Exist for this Numbering Scheme", gPMConstants.PMELogLevel.PMLogError)
        End If

        Dim auxVar As Object = vResultArray(iNUM_SCHEME_RESET_DAILY, 0)




        If CStr(vResultArray(iNUM_SCHEME_RESET_DAILY, 0)) = "" Or Object.Equals(vResultArray(iNUM_SCHEME_RESET_DAILY, 0), Nothing) Or Convert.IsDBNull(auxVar) Or Informations.IsNothing(auxVar) Then
            bResetDaily = False
        Else

            bResetDaily = CBool(vResultArray(19, 0))
        End If




        If vResultArray(iNUM_SCHEME_DATE_LAST_GENERATED, 0) Is DBNull.Value Or CStr(vResultArray(iNUM_SCHEME_DATE_LAST_GENERATED, 0)) = "" Then
            dLastGenerated = DateTime.FromOADate(0)
        Else

            dLastGenerated = CDate(vResultArray(iNUM_SCHEME_DATE_LAST_GENERATED, 0))
        End If

        Dim bResetNumberingScheme As String = CStr(False)


        If dLastGenerated = Nothing Or dLastGenerated = CDate("12:00:00 AM") Then

        Else
            'developer guide no. 40
            If DateTime.Parse(DateTime.Now) > DateTime.Parse(dLastGenerated) And bResetDaily Then
                bResetNumberingScheme = CStr(True)
            End If
        End If

        m_lReturn = IncrementNumberingScheme(v_lNumberingSchemeID:=v_iNumberingScheme, v_bResetNumberingScheme:=CBool(bResetNumberingScheme))

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "IncrementNumberingScheme method failed ", gPMConstants.PMELogLevel.PMLogError)
        End If

        m_lReturn = CommitTrans()

        GoTo Finally_Renamed

        bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)

Finally_Renamed:
        Return result
    End Function

    'End PN: 62476
End Class