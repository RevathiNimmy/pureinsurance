Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
'Modified by Sudhanshu Behera on 5/21/2010 1:28:33 PM refer developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness

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


    Private Const ACClass As String = "Business"

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' PRIVATE Data Members (Begin)


    Private m_sUnderwritingOrAgency As String = ""


    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

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

    'Developer Guide No.: 39
    'Private Const ACGetListTypesSQL As String = "{call spu_List_Types_List}"
    Private Const ACGetListTypesSQL As String = "spu_List_Types_List"
    Private Const ACGetListTypesName As String = "spu_List_Types_List"
    Private Const ACGetListTypesStored As Boolean = True

    'Developer Guide No.: 39
    'Private Const ACGetSchemesSQL As String = "{call spu_Get_Schemes}"
    Private Const ACGetSchemesSQL As String = "spu_Get_Schemes"
    Private Const ACGetSchemesName As String = "spu_Get_Schemes"
    Private Const ACGetSchemesStored As Boolean = True

    'Developer Guide No.: 39
    'Private Const ACGetVersionsSQL As String = "{call spu_Get_Versions_For_Scheme (?)}"
    Private Const ACGetVersionsSQL As String = "spu_Get_Versions_For_Scheme"
    Private Const ACGetVersionsName As String = "spu_Get_Versions_For_Scheme"
    Private Const ACGetVersionsStored As Boolean = True

    ' CTAF 070602
    'Developer Guide No.: 39
    'Private Const ACDeleteGroupsSQL As String = "{call spu_Delete_Groups_For_Scheme (?,?)}"
    Private Const ACDeleteGroupsSQL As String = "spu_Delete_Groups_For_Scheme"
    Private Const ACDeleteGroupsName As String = "spu_Delete_Groups_For_Scheme"
    Private Const ACDeleteGroupsStored As Boolean = True

    'Developer Guide No.: 39
    'Private Const ACClearupGroupsSQL As String = "{call spu_Clearup_Groups (?)}"
    Private Const ACClearupGroupsSQL As String = "spu_Clearup_Groups"
    Private Const ACClearupGroupsName As String = "spu_Clearup_Groups"
    Private Const ACClearupGroupsStored As Boolean = True

    'Developer Guide No.: 39
    'Private Const ACImportGroupSQL As String = "{call spu_Import_Group (?,?,?,?)}"
    Private Const ACImportGroupSQL As String = "spu_Import_Group"
    Private Const ACImportGroupName As String = "spu_Import_Group"
    Private Const ACImportGroupStored As Boolean = True

    'Jes 02 October 2002 ---
    Private Const ACABICode As Integer = 0
    Private Const ACGroupCode As Integer = 1

    Public Structure GroupRecord
        Dim ABICode As String
        Dim GroupCode As String
        Public Shared Function CreateInstance() As GroupRecord
            Dim result As New GroupRecord
            result.ABICode = String.Empty
            result.GroupCode = String.Empty
            Return result
        End Function
    End Structure

    Public WriteOnly Property Database() As dPMDAO.Database
        Set(ByVal Value As dPMDAO.Database)

            m_oDatabase = Value

        End Set
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise





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




            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)


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




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetListTypes(ByRef r_vResultArray(,) As Object, ByRef r_lRecordsFound As Integer) As Integer


        Dim result As Integer = 0
        Try

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetListTypesSQL, sSQLName:=ACGetListTypesName, bStoredProcedure:=ACGetListTypesStored, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If NO Claim Tabs were found return Not Found
            If Not Information.IsArray(r_vResultArray) Then
                r_lRecordsFound = gPMConstants.PMEReturnCode.PMNotFound
            Else
                r_lRecordsFound = gPMConstants.PMEReturnCode.PMOK
            End If



            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Get List Types Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListTypes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function GetSchemes(ByRef lListTypeID As Integer, ByRef r_vResultArray(,) As Object, ByRef r_lRecordsFound As Integer) As Integer


        Dim result As Integer = 0
        Try

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            '    'Add the TabID parameter (INPUT)
            '    m_lReturn = m_oDatabase.Parameters.Add( _
            ''            sName:="List_Type_ID", _
            ''            vValue:=lListTypeID, _
            ''            iDirection:=PMParamInput, _
            ''            iDataType:=PMLong)
            '
            '    If (m_lReturn& <> PMTrue) Then
            '        ' Log Error Message
            '        LogMessage m_sUsername, _
            ''            iType:=PMLogError, _
            ''            sMsg:="oParameters.Add failed", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="GetSchemes"
            '
            '        GetSchemes = PMFalse
            '        Exit Function
            '    End If
            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetSchemesSQL, sSQLName:=ACGetSchemesName, bStoredProcedure:=ACGetSchemesStored, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If NO Claim Tabs were found return Not Found
            If Not Information.IsArray(r_vResultArray) Then
                r_lRecordsFound = gPMConstants.PMEReturnCode.PMNotFound
            Else
                r_lRecordsFound = gPMConstants.PMEReturnCode.PMOK
            End If



            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Get List Types Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSchemes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function GetVersions(ByRef lSchemeNo As Integer, ByRef r_vResultArray(,) As Object, ByRef r_lRecordsFound As Integer) As Integer


        'On Error GoTo Err_GetVersions

        'Clear the Database Parameters Collection
        Dim result As Integer = 0
        m_oDatabase.Parameters.Clear()

        'Add the TabID parameter (INPUT)
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Scheme_No", vValue:=CStr(lSchemeNo), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetVersions")

            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        'Execute SQL Statement
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetVersionsSQL, sSQLName:=ACGetVersionsName, bStoredProcedure:=ACGetVersionsStored, vResultArray:=r_vResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' If NO Claim Tabs were found return Not Found
        If Not Information.IsArray(r_vResultArray) Then
            r_lRecordsFound = gPMConstants.PMEReturnCode.PMNotFound
        Else
            r_lRecordsFound = gPMConstants.PMEReturnCode.PMOK
        End If



        Return gPMConstants.PMEReturnCode.PMTrue



        ' Error Section.

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Get List Types Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetVersions", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result
    End Function

    Public Function BeginTrans() As Integer

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    Public Function CommitTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Commit the Transaction
            m_lReturn = m_oDatabase.SQLCommitTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    Public Function RollbackTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' roolback the Transaction
            m_lReturn = m_oDatabase.SQLRollbackTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' CTAF - 070602 - Added  v_lListTypeID parameter
    Public Function DeleteGroups(ByRef lSchemeID As Integer, ByVal v_lListTypeID As Integer) As Integer

        Dim result As Integer = 0
        Try

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()


            'Add the SchemeID parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Scheme_ID", vValue:=CStr(lSchemeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteGroups")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' CTAF 070602 - Check value
            m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_list_type_id", vValue:=CStr(v_lListTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteGroups")

                Return result
            End If

            'Update Record
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteGroupsSQL, sSQLName:=ACDeleteGroupsName, bStoredProcedure:=ACDeleteGroupsStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If


            Return m_lReturn

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Delete Groups Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteGroups", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function ClearupGroups(ByRef lSchemeID As Integer) As Integer

        Dim result As Integer = 0
        Try

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()


            'Add the SchemeID parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Scheme_ID", vValue:=CStr(lSchemeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearupGroups")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Update Record
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACClearupGroupsSQL, sSQLName:=ACClearupGroupsName, bStoredProcedure:=ACClearupGroupsStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If


            Return m_lReturn

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Clearup Groups Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearupGroups", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    Public Function ImportGroups(ByRef lListTypeID As Integer, ByRef lSchemeID As Integer, ByRef Groups As Object) As Integer

        Dim result As Integer = 0

        Try

            For l As Integer = 1 To Groups.GetUpperBound(1)

                'Clear the Database Parameters Collection
                m_oDatabase.Parameters.Clear()

                'Add the List Type parameter (INPUT)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_list_type_id", vValue:=CStr(lListTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ImportGroup")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Add the SchemeID parameter (INPUT)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_Scheme_ID", vValue:=CStr(lSchemeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ImportGroup")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                'Add the Group Code parameter (INPUT)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="group_code", vValue:=CStr(Groups(ACGroupCode, l)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ImportGroup")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                'Add the ABI Code parameter (INPUT)
                'UPGRADE_WARNING: (1068) Groups() of type Variant is being forced to String. More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
                m_lReturn = m_oDatabase.Parameters.Add(sName:="ABICode", vValue:=CStr(Groups(ACABICode, l)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ImportGroup")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Update Record
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACImportGroupSQL, sSQLName:=ACImportGroupName, bStoredProcedure:=ACImportGroupStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New Exception()
                End If

            Next l


            Return m_lReturn

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Import Group Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ImportGroup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
End Class
