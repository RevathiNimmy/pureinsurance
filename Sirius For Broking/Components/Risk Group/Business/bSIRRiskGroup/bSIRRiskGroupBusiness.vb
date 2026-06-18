Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Data.SqlClient
'Developer Guide No. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 02/02/2000
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a SIRRiskGroup.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 10/12/2003
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************


    ' ************************************************
    ' Added to replace global variables 10/12/2003



    ' ************************************************


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database
    Private m_oArcDatabase As dPMDAO.Database 'CT 02/11/00 added as GetCaptionId function will need to connect to Arch DB
    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean
    Private m_bCloseArchDatabase As Boolean 'CT 02/11/00 added as GetCaptionId function will need to connect to Arch DB
    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private lPMAuthorityLevel As Integer

    ' Primary Keys to work with
    Private m_lRiskGroupId As Integer

    'Developer Guide No. 17
    Private m_vResultArray As Object
    'CT 19/10/00 new risk screen id variable to hold new field on risk_group
    Private m_lRiskScreenId As Integer
    'eck130901
    ' PM Lookup Business Component (Private)
    Private m_oLookup As bPMLookup.Business
    Private m_vRenewalDays As Object

    ' CTAF
    Private m_vOldRiskGroups(,) As Object

    Private m_lBrokerlinkPolicyTypeId As Integer

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

    Public Property RiskGroupId() As Integer
        Get

            Return m_lRiskGroupId

        End Get
        Set(ByVal Value As Integer)

            m_lRiskGroupId = Value

        End Set
    End Property

    Public Property ResultArray() As Boolean
        Get

            Return m_vResultArray

        End Get
        Set(ByVal Value As Boolean)


            m_vResultArray = CBool(Value)

        End Set
    End Property
    'eck 130901
    Public Property RenewalDays() As Object
        Get

            Return m_vRenewalDays

        End Get
        Set(ByVal Value As Object)



            m_vRenewalDays = Value

        End Set
    End Property

    'CT 19/10/00 added new property to old new field on risk_group table  - (start)

    Public Property RiskScreenId() As Integer
        Get
            Return m_lRiskScreenId
        End Get
        Set(ByVal Value As Integer)
            m_lRiskScreenId = Value
        End Set
    End Property
    'CT 19/10/00 added new property to old new field on risk_group table  - (end)



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
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise


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

            'CT 02/11/00 Added connection to architecture db so that Risk group table may
            ' be maintained. This requires new captions on the architecture DB


            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_bNewInstanceCreated:=m_bCloseArchDatabase, r_oCheckedDatabase:=m_oArcDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get connection to architecture database.", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'eck130901
            ' Create PM Lookup Business Object
            m_oLookup = New bPMLookup.Business()

            ' Initialise PM Lookup Business passing our Database Reference.
            'Developer Guide No. 67
            m_lReturn = m_oLookup.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, bStandAlone:=False, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

            '



            ' Set Username and Password

            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now


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
                End If
                m_oDatabase = Nothing
                If m_bCloseArchDatabase Then
                    m_oArcDatabase.CloseDatabase()

                End If
                m_vResultArray = Nothing
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

    ' ***************************************************************** '
    ' Name: CheckID (Public)
    '
    ' Description: Checks to see if the supplied ID is a valid record.
    '
    ' ***************************************************************** '
    Public Function CheckID(ByRef vID As Object) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the ID parameter (INPUT)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="id", vValue:=CStr(vID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckIDSQL, sSQLName:=ACCheckIDName, bStoredProcedure:=ACCheckIDStored, lNumberRecords:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            ' Do we have any records ?
            If lRecordCount < 1 Then
                ' No record found
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetDetails
    '
    ' Description:
    '
    ' History: 17/04/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function GetDetails() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_group_id", vValue:=CStr(m_lRiskGroupId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllDetailsSQL, sSQLName:=ACGetAllDetailsName, bStoredProcedure:=ACGetAllDetailsStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=m_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetRiskGroup
    '
    ' Description: Brings back risk group details including new GIS screen id
    '
    ' History: CT 1/11/00 - Created .
    '
    ' ***************************************************************** '
    Public Function GetRiskGroup(ByRef r_vDetailArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()



            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRiskGroupSQL, sSQLName:=ACGetRiskGroupName, bStoredProcedure:=ACGetRiskGroupStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vDetailArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' CTAF 141201 - Store the array for when we do an update
            m_vOldRiskGroups = r_vDetailArray

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRiskGroup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskGroup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    '
    ' Name: GetOtherDetails
    '
    ' Description:
    '
    ' History: 17/04/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function GetOtherDetails(ByRef vSources(,) As Object, ByRef vCommissionAccounts As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get sources
            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetSourceSQL, sSQLName:="GetSources", bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vSources)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get commission accounts - hard coded type for now
            '''    sSql = "SELECT party_cnt, shortname, resolved_name, source_id" & vbCrLf & _
            ''''           "FROM party" & vbCrLf & _
            ''''           "WHERE is_deleted = 0" & vbCrLf & _
            ''''           "AND party_type_id = 12" & vbCrLf & _
            ''''           "ORDER BY 4, 2"

            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_type_id", vValue:=CStr(12), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetCommissionSQL, sSQLName:="GetCommision", bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vCommissionAccounts)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOtherDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOtherDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: GetRenewalDetails
    '
    ' Description:
    '
    ' History: 13/09/2001 ECK - Created.
    '
    ' ***************************************************************** '
    Public Function GetRenewalDetails(ByRef vRenewalDetails(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase
                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="risk_group_id", vValue:=CStr(RiskGroupId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRiskRenewalDaysSQL, sSQLName:=ACGetRiskRenewalDaysName, bStoredProcedure:=ACGetRiskRenewalDaysStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vRenewalDetails)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRenewalDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRenewalDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetRiskScreenId
    '
    ' Description:
    '
    ' History: CT created 19/10/00
    '
    ' ***************************************************************** '
    Public Function GetRiskScreenId(ByRef vRiskGroup As Object) As Integer
        Dim result As Integer = 0
        Dim vRiskScreenId As Object

        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            'Get sources

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_group_id", vValue:=CStr(CInt(vRiskGroup)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRiskScreensID, sSQLName:="GetRiskScreenId", bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vRiskScreenId)

            '''    sSql = "SELECT gis_screen_id " & vbCrLf & _
            ''''           "FROM risk_group " & vbCrLf & _
            ''''           "WHERE risk_group_id = " & CStr(vRiskGroup)
            '''
            '''    m_lReturn = m_oDatabase.SQLSelect(sSql:=sSql, _
            ''''                                      sSQLName:="GetRiskScreenId", _
            ''''                                      bStoredProcedure:=False, _
            ''''                                      lNumberRecords:=PMAllRecords, _
            ''''                                      vResultArray:=vRiskScreenId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If CStr(vRiskScreenId(0, 0)) = "" Then
                RiskScreenId = 0
            Else

                RiskScreenId = CInt(vRiskScreenId(0, 0))
            End If



            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOtherDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOtherDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: GetRiskAllScreens
    '
    ' Description:
    '
    ' History: CT created 19/10/00
    '
    ' ***************************************************************** '
    Public Function GetAllRiskScreens(ByRef vRiskScreens(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get sources
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllScreensSQL, sSQLName:="GetRiskAllScreens", bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vRiskScreens)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllRiskScreens Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllRiskScreens", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    '
    ' Name: Update
    '
    ' Description:
    '
    ' History: 17/04/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function Update() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_group_id", vValue:=CStr(m_lRiskGroupId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteAllDetailsSQL, sSQLName:=ACDeleteAllDetailsName, bStoredProcedure:=ACDeleteAllDetailsStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Information.IsArray(m_vResultArray) Then
                'Nothing to add
                Return result
            End If

            'Developer Guide No. 18

            For lRow As Integer = m_vResultArray.GetLowerBound(2) To m_vResultArray.GetUpperBound(2)

                If m_vResultArray(2, lRow) Then
                    m_oDatabase.Parameters.Clear()

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_group_id", vValue:=CStr(m_lRiskGroupId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=CStr(m_vResultArray(0, lRow)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="commission_cnt", vValue:=CStr(m_vResultArray(3, lRow)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.SQLAction(sSQL:=ACInsertDetailsSQL, sSQLName:=ACInsertDetailsName, bStoredProcedure:=ACInsertDetailsStored)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

            Next lRow

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CheckRiskGroupChanged
    '
    ' Description: Checks if we need to update the risk group
    '
    ' History: 14/12/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function CheckRiskGroupChanged(ByVal v_vNewArray(,) As Object, ByVal v_lIndex As Integer, ByRef r_bChanged As Boolean) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        r_bChanged = False

        ' Check the array for changes
        For iLoop1 As Integer = 0 To ACArrayMax 'pkh 03/07/2002

            If Not v_vNewArray(iLoop1, v_lIndex).Equals(m_vOldRiskGroups(iLoop1, v_lIndex)) Then
                r_bChanged = True
                Exit For
            End If
        Next iLoop1

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateRiskGroup
    '
    ' Description: Updates the Risk_group table with the passed array
    '
    ' History: 02/11/2000 CT - Created.
    '
    ' ***************************************************************** '
    Public Function UpdateRiskGroup(ByVal v_vArray(,) As Object) As Integer

        Dim result As Integer = 0
        ' CTAF 141201
        Dim bUpdate As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Loop the array
            For iLoop1 As Integer = 0 To v_vArray.GetUpperBound(1)

                ' Do we need to do an update, or an add ?

                If CDbl(v_vArray(ACArrayCaptionID, iLoop1)) = 0 Then

                    ' Add
                    m_lReturn = CType(AddRecord(r_vArray:=v_vArray, v_iIndex:=iLoop1), gPMConstants.PMEReturnCode)

                Else

                    ' CTAF 141201 - Start
                    ' Check if they're different
                    m_lReturn = CType(CheckRiskGroupChanged(v_vNewArray:=v_vArray, v_lIndex:=iLoop1, r_bChanged:=bUpdate), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Unable to check?!? Update it anyway...
                        bUpdate = True
                    End If

                    If bUpdate Then
                        m_lReturn = CType(UpdateRecord(r_vArray:=v_vArray, v_iIndex:=iLoop1), gPMConstants.PMEReturnCode)
                    End If

                    ' CTAF 141201 - End

                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Next iLoop1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateRiskGroup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskGroup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: UpdateRenewalDays
    '
    ' Description:
    '
    ' History: 13/09/2001 ECK - Created.
    '
    ' ***************************************************************** '
    Public Function UpdateRenewalDays() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_group_id", vValue:=CStr(m_lRiskGroupId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteRiskRenewalDaysSQL, sSQLName:=ACDeleteRiskRenewalDaysName, bStoredProcedure:=ACDeleteRiskRenewalDaysStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Information.IsArray(m_vRenewalDays) Then
                'Nothing to add
                Return result
            End If


            For lRow As Integer = m_vRenewalDays.GetLowerBound(1) To m_vRenewalDays.GetUpperBound(1)

                m_oDatabase.Parameters.Clear()

                m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_group_id", vValue:=CStr(m_lRiskGroupId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = m_oDatabase.Parameters.Add(sName:="service_level_id", vValue:=CStr(CInt(m_vRenewalDays(0, lRow))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = m_oDatabase.Parameters.Add(sName:="renewal_days", vValue:=CStr(CInt(m_vRenewalDays(1, lRow))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACInsertRiskRenewalDaysSQL, sSQLName:=ACInsertRiskRenewalDaysName, bStoredProcedure:=ACInsertRiskRenewalDaysStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Next lRow

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateRenewalDays Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    '
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
    '
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
    '
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
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
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
    ' Name: AddRecord
    '
    ' Description: add new risk group record
    '
    ' History: 01/11/2000 CT - Created.
    '
    ' ***************************************************************** '
    Private Function AddRecord(ByRef r_vArray(,) As Object, ByVal v_iIndex As Integer) As Integer

        Dim result As Integer = 0
        Dim vRiskGroupId As String = ""
        Dim vCaptionID As Object
        Dim vCode As String = ""
        Dim vDescription As String = ""
        'Developer Guide No 17. 
        Dim vIsDeleted As Object
        Dim vRiskScreen As String = ""
        'CT 07/11/00 new ABI code
        Dim vABICode As String = ""
        ' CTAF 050402
        Dim vPQRiskScreen As String = ""
        Dim vEffectiveDate As Date
        Dim vFSAProduct As String = "" 'FSA Phase IV
        Dim vMidnightRenewal As String = "" '2005 - Midnight Renewal
        Dim vCountryId As String = "" 'Datasure
        Dim vBrokerlinkPolicyTypeId As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get the values out of the array

        vRiskGroupId = CStr(r_vArray(ACArrayRiskGroupID, v_iIndex))

        vCode = CStr(r_vArray(ACArrayCode, v_iIndex))

        vDescription = CStr(r_vArray(ACArrayDescription, v_iIndex))
        vIsDeleted = 0

        r_vArray(ACArrayIsDeleted, v_iIndex) = 0

        If CStr(r_vArray(ACArrayRiskScreenType, v_iIndex)) = "" Then

            vRiskScreen = Nothing
        Else

            vRiskScreen = CStr(r_vArray(ACArrayRiskScreenType, v_iIndex))
        End If
        'CT 07/11/00 get new abi_code

        If CStr(r_vArray(ACArrayABICode, v_iIndex)) = "" Then

            vABICode = Nothing
        Else

            vABICode = CStr(r_vArray(ACArrayABICode, v_iIndex))
        End If

        ' CTAF 050402 - Post Quote Risk screen

        If CStr(r_vArray(ACArrayPQRiskScreenType, v_iIndex)) = "" Then

            vPQRiskScreen = Nothing
        Else

            vPQRiskScreen = CStr(r_vArray(ACArrayPQRiskScreenType, v_iIndex))
        End If

        'DN 26/11/02 ISS1452 Pass in effective date from interface

        vEffectiveDate = CDate(r_vArray(ACArrayEffectiveDate, v_iIndex))

        'FSA Phase IV

        If CStr(r_vArray(ACArrayFSAProduct, v_iIndex)) = "" Then

            vFSAProduct = Nothing
        Else

            vFSAProduct = CStr(r_vArray(ACArrayFSAProduct, v_iIndex))
        End If

        '2005 - Midnight Renewal

        vMidnightRenewal = CStr(r_vArray(ACArrayMidnightRenewal, v_iIndex))

        'Datasure

        vCountryId = CStr(r_vArray(ACArrayCountryId, v_iIndex))


        vBrokerlinkPolicyTypeId = CStr(r_vArray(ACArrayBrokerlinkPolicyTypeId, v_iIndex))

        ' Get the caption
        m_lReturn = CType(GetCaptionID(v_vDescription:=vDescription, r_vCaptionID:=vCaptionID), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Remove the parameters
        m_oDatabase.Parameters.Clear()

        ' Add new ones
        m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_group_id", vValue:=vRiskGroupId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="caption_id", vValue:=CStr(vCaptionID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=vCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="description", vValue:=vDescription, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' CTAF 030101 - Switched the order of is_deleted and effective_date around
        m_lReturn = m_oDatabase.Parameters.Add(sName:="is_deleted", vValue:=CStr(vIsDeleted), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=DateTimeHelper.ToString(vEffectiveDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_screen_id", vValue:=vRiskScreen, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'CT 07/11/00 update record with new ABICode
        m_lReturn = m_oDatabase.Parameters.Add(sName:="abi_code", vValue:=vABICode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' CTAF 050402 - Post Quote..
        m_lReturn = m_oDatabase.Parameters.Add(sName:="post_quote_gis_screen_id", vValue:=vPQRiskScreen, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'FSA Phase IV
        m_lReturn = m_oDatabase.Parameters.Add(sName:="FSA_Product_id", vValue:=vFSAProduct, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '2005 Midnight Renewal
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Midnight_Renewal", vValue:=vMidnightRenewal, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Datasure
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Country_id", vValue:=vCountryId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="Brokerlink_Policy_Type_Id", vValue:=vBrokerlinkPolicyTypeId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute the SQL
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add new record : " & ACAddSQL, vApp:=ACApp, vClass:=ACClass, vMethod:="AddRecord", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Return result
        End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateRecord
    '
    ' Description: update the risk group record
    '
    ' History: 01/11/2000 CT - Created.
    '
    ' ***************************************************************** '
    Private Function UpdateRecord(ByRef r_vArray(,) As Object, ByVal v_iIndex As Integer) As Integer

        Dim result As Integer = 0
        Dim vRiskGroupId As String = ""
        Dim vCaptionID As Object
        Dim vCode As String = ""
        Dim vDescription As String = ""
        Dim vIsDeleted As String = ""
        Dim vRiskScreen As String = ""
        'CT 07/11/00 new ABI code
        Dim vABICode As String = ""
        ' CTAF 050402
        Dim vPQRiskScreen As String = ""
        Dim vEffectiveDate As Date
        Dim vFSAProduct As String = "" 'FSA Phase IV
        Dim vMidnightRenewal As String = "" '2005 - Midnight Renewal
        Dim vCountryId As String = "" 'Datasure
        Dim vBrokerlinkPolicyTypeId As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get the values out of the array

        vRiskGroupId = CStr(r_vArray(ACArrayRiskGroupID, v_iIndex))

        vCode = CStr(r_vArray(ACArrayCode, v_iIndex))

        vDescription = CStr(r_vArray(ACArrayDescription, v_iIndex))

        vIsDeleted = CStr(r_vArray(ACArrayIsDeleted, v_iIndex))

        If CStr(r_vArray(ACArrayRiskScreenType, v_iIndex)) = "" Then

            vRiskScreen = Nothing
        Else

            vRiskScreen = CStr(r_vArray(ACArrayRiskScreenType, v_iIndex))
        End If
        'CT 07/11/00 get new abi_code

        If CStr(r_vArray(ACArrayABICode, v_iIndex)) = "" Then

            vABICode = Nothing
        Else

            vABICode = CStr(r_vArray(ACArrayABICode, v_iIndex))
        End If
        ' CTAF 050402 - Post Quote

        If CStr(r_vArray(ACArrayPQRiskScreenType, v_iIndex)) = "" Then

            vPQRiskScreen = Nothing
        Else

            vPQRiskScreen = CStr(r_vArray(ACArrayPQRiskScreenType, v_iIndex))
        End If

        'DN 26/11/02 ISS1452 Pass in effective date from interface

        vEffectiveDate = CDate(r_vArray(ACArrayEffectiveDate, v_iIndex))

        'FSA Phase IV

        If CStr(r_vArray(ACArrayFSAProduct, v_iIndex)) = "" Then

            vFSAProduct = Nothing
        Else

            vFSAProduct = CStr(r_vArray(ACArrayFSAProduct, v_iIndex))
        End If
        'extra check for nulls

        If CStr(r_vArray(ACArrayMidnightRenewal, v_iIndex)) = "" Then

            vMidnightRenewal = Nothing
        Else

            vMidnightRenewal = CStr(r_vArray(ACArrayMidnightRenewal, v_iIndex)) '2005 - Midnight Renewal
        End If

        If CStr(r_vArray(ACArrayCountryId, v_iIndex)) = "" Then

            vCountryId = Nothing
        Else

            vCountryId = CStr(r_vArray(ACArrayCountryId, v_iIndex)) 'Datasure
        End If

        If CStr(r_vArray(ACArrayBrokerlinkPolicyTypeId, v_iIndex)) = "" Then

            vBrokerlinkPolicyTypeId = Nothing
        Else

            vBrokerlinkPolicyTypeId = CStr(r_vArray(ACArrayBrokerlinkPolicyTypeId, v_iIndex)) 'Datasure
        End If

        ' Get the caption
        m_lReturn = CType(GetCaptionID(v_vDescription:=vDescription, r_vCaptionID:=vCaptionID), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Clear the parameters
        m_oDatabase.Parameters.Clear()

        ' Add new ones
        m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_group_id", vValue:=vRiskGroupId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="caption_id", vValue:=CStr(vCaptionID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=vCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="description", vValue:=vDescription, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' CTAF 030101 - Switched the order of is_deleted and effective_date around
        m_lReturn = m_oDatabase.Parameters.Add(sName:="is_deleted", vValue:=vIsDeleted, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=DateTimeHelper.ToString(vEffectiveDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_screen_id", vValue:=vRiskScreen, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'CT 07/11/00 update record with new ABICode
        m_lReturn = m_oDatabase.Parameters.Add(sName:="abi_code", vValue:=vABICode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' CTAF 050402
        m_lReturn = m_oDatabase.Parameters.Add(sName:="post_quote_gis_screen_id", vValue:=vPQRiskScreen, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'FSA Phase IV
        m_lReturn = m_oDatabase.Parameters.Add(sName:="FSA_Product_id", vValue:=vFSAProduct, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '2005 Midnight Renewal
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Midnight_Renewal", vValue:=vMidnightRenewal, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Datasure
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Country_id", vValue:=vCountryId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="Brokerlink_Policy_Type_Id", vValue:=vBrokerlinkPolicyTypeId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute the SQL
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateSQL, sSQLName:=ACUpdateName, bStoredProcedure:=ACUpdateStored)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update record : " & ACAddSQL, vApp:=ACApp, vClass:=ACClass, vMethod:="AddRecord", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Return result
        End If

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: GetBranchBaseCountry
    '
    ' Description:  gets base country for branch
    '
    ' History: DM 09082006 created
    ' ***************************************************************** '
    Public Function GetBranchBaseCountry(ByVal v_lSourceID As Integer, ByRef r_iCountryID As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            sSQL = "SELECT country_id FROM source "
            sSQL = sSQL & "WHERE source_id = " & CStr(v_lSourceID)

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="GetBranchBaseCountry", bStoredProcedure:=False)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

                r_iCountryID = .Records.Fields("country_id")

            End With


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            'Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBranchBaseCountry failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBranchBaseCountry", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetCaptionID
    '
    ' Description:
    '
    ' History: 02/11/2000 CT - Created.
    '
    ' ***************************************************************** '
    Public Function GetCaptionID(ByVal v_vDescription As Object, ByRef r_vCaptionID As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the parameters
            m_oArcDatabase.Parameters.Clear()

            ' Add the new ones
            m_lReturn = m_oArcDatabase.Parameters.Add(sName:="language_id", vValue:=CStr(m_iLanguageID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oArcDatabase.Parameters.Add(sName:="caption", vValue:=CStr(v_vDescription), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oArcDatabase.Parameters.Add(sName:="caption_id", vValue:=CStr(r_vCaptionID), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute the SQL
            m_lReturn = m_oArcDatabase.SQLAction(sSQL:=ACGetCaptionSQL, sSQLName:=ACGetCaptionName, bStoredProcedure:=ACGetCaptionStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed on sql = " & ACGetCaptionSQL, vApp:=ACApp, vClass:=ACClass, vMethod:="GetCaptionID", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Get the caption_id

            r_vCaptionID = m_oArcDatabase.Parameters.Item("caption_id").Value

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCaptionID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCaptionID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupValues (Public)
    '
    ' Description: Gets the Lookup values for a SIRPartyPC.
    '
    ' 'eck 130901 Added for Service Levels
    ' ***************************************************************** '
    'Developer Guide No. 17
    Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim dtEffectiveDate As Date

        'Tomo210100
        'Don't include party business, as now it's an ABI list
        ' {* USER DEFINED CODE (Begin) *}
        Dim vTabArray(3, 0) As Object
        ' {* USER DEFINED CODE (End) *}

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset Result Array
            vResultArray = Nothing
            ' Reset Table Array

            vTableArray = Nothing

            ' {* USER DEFINED CODE (Begin) *}

            ' Setup Lookup Table Names

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = gSIRLibrary.SIRLookupServiceLevel

            ' {* USER DEFINED CODE (End) *}

            iLookupType = gPMConstants.PMELookupType.PMLookupAll

            ' Do not supply a key

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = ""

            ' Default Effective Date to current date/time
            dtEffectiveDate = DateTime.Now

            ' Get the Lookup items
            m_lReturn = m_oLookup.GetLookupValues(iLookupType:=iLookupType, vTableArray:=vTabArray, iLanguageID:=iLanguageID, dtEffectiveDate:=dtEffectiveDate, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the Table Array

            vTableArray = vTabArray

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookupValues Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class