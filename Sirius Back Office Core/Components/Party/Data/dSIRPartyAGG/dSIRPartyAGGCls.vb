Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
'developer guide no. 129 (guide)
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("SIRPartyAGG_NET.SIRPartyAGG")>
Public NotInheritable Class SIRPartyAGG
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: SIRPartyAGG
    '
    ' Date: 08/07/02
    '
    ' Description: Describes the SIRPartyAGG attributes.
    '              Created from dSIRPartyAG.
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
    Private Const ACClass As String = "SIRPartyAGG"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag
    Private m_bCloseDatabase As Boolean

    ' DataBase Attributes
    Private m_lPartyCnt As Integer
    Private m_iPartyGroupActive As Integer
    Private m_sUniqueId As String = ""
    Private m_sScreenHierarchy As String = ""

    ' Function Return Code
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public WriteOnly Property Database() As dPMDAO.Database
        Set(ByVal Value As dPMDAO.Database)

            m_oDatabase = Value

        End Set
    End Property



    Public Property PartyCnt() As Integer
        Get

            Return m_lPartyCnt

        End Get
        Set(ByVal Value As Integer)

            m_lPartyCnt = Value

        End Set
    End Property
    Public Property PartyGroupActive() As Integer
        Get

            Return m_iPartyGroupActive

        End Get
        Set(ByVal Value As Integer)

            m_iPartyGroupActive = Value

        End Set
    End Property

    Public Property UniqueId() As String
        Get
            Return m_sUniqueId
        End Get
        Set(ByVal Value As String)
            m_sUniqueId = Value
        End Set
    End Property

    Public Property ScreenHierarchy() As String
        Get
            Return m_sScreenHierarchy
        End Get
        Set(ByVal Value As String)
            m_sScreenHierarchy = Value
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
    Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUserName
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
    ' Name: Add (Public)
    '
    ' Description: Adds to the Database from the Base Details.
    '
    ' ***************************************************************** '
    Public Function Add() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' CTAF 191200 - Rearranged the order in which parameters are added

                ' Add PrimaryKey as OUTPUT parameters
                m_lReturn = CType(AddKeyInputParam(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Add the required INPUT parameters
                m_lReturn = CType(AddInputParam(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Get the Primary Key of the record inserted
                m_lReturn = CType(GetNewPrimaryKeyID(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Add Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Update (Public)
    '
    ' Description: Updates a single record in the database from
    '              the Base Details.
    '
    ' ***************************************************************** '
    Public Function Update() As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' CTAF 191200 - Rearranged the order in which parameters are added

                ' Add PrimaryKey as INPUT parameters
                m_lReturn = CType(AddKeyInputParam(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Add the required INPUT parameters
                m_lReturn = CType(AddInputParam(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACUpdateSQL, sSQLName:=ACUpdateName, bStoredProcedure:=ACUpdateStored, lRecordsAffected:=lRecordsAffected)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Check to see that the record was updated OK
                If lRecordsAffected > 0 Then
                    ' Updated No action required
                Else
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Delete (Public)
    '
    ' Description: Deletes a single record from the database.
    '
    ' ***************************************************************** '
    Public Function Delete() As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Add PrimaryKey as INPUT parameters
                m_lReturn = CType(AddKeyInputParam(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACDeleteSQL, sSQLName:=ACDeleteName, bStoredProcedure:=ACDeleteStored, lRecordsAffected:=lRecordsAffected)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' If record wasn't deleted, error
                If lRecordsAffected > 0 Then
                    ' Deleted, No action required
                Else
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Delete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Delete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SelectSingle (Public)
    '
    ' Description: Selects the required SIRPartyAGG
    '
    ' ***************************************************************** '
    Public Function SelectSingle(Optional ByRef vLockMode As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Default to No Lock if not supplied or not numeric
                Dim dbNumericTemp As Double

                If (Informations.IsNothing(vLockMode)) Or (Not Double.TryParse(CStr(vLockMode), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                    vLockMode = gPMConstants.PMELockMode.PMNoLock
                End If

                ' Add PrimaryKey as INPUT parameters
                m_lReturn = CType(AddKeyInputParam(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = .SQLSelect(sSQL:=ACSelectSingleSQL, sSQLName:=ACSelectSingleName, bStoredProcedure:=ACSelectSingleStored, bKeepNulls:=True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' How many records were selected
                lRecordCount = .Records.Count()

                ' Do we have any records ?
                If lRecordCount = 1 Then
                    ' Selected, No action required
                Else
                    Return gPMConstants.PMEReturnCode.PMNotFound
                End If

                ' Set properties
                'Developer Guide no: 162
                m_lReturn = CType(SetPropertiesFromDB(oFields:= .Records.Item(0).Fields()), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectSingle Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectSingle", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetPropertiesFromDB (Public)
    '
    ' Description: Sets the supplied SIRPartyAGG properties from a database
    '              record.
    ' ***************************************************************** '
    'developer guide no. 112 (guide)
    Public Function SetPropertiesFromDB(ByRef oFields As DataRow) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Populate Base Details

            With oFields
                '02082002 CMG/PB Scalability Nullto function
                PartyCnt = gPMFunctions.NullToLong(oFields("party_cnt"))
                ' CF 050799

                'developer guide no. 131
                If oFields("active") Is DBNull.Value OrElse oFields("active") Is Nothing Then
                    PartyGroupActive = 0
                Else
                    PartyGroupActive = oFields("active")
                End If

                '        PartyAgentOriginID = .Item("party_agent_origin_id").Value
                '        If (IsNull(.Item("agency_agreement_date").Value) = True) Then
                ''EK110199
                ''            AgencyAgreementDate = Null
                '            AgencyAgreementDate = 0
                '        Else
                '            AgencyAgreementDate = .Item("agency_agreement_date").Value
                '        End If
                '        If (IsNull(.Item("agency_next_review_date").Value) = True) Then
                ' 'EK110199
                ' '           AgencyNextReviewDate = Null
                '            AgencyNextReviewDate = 0
                '        Else
                '            AgencyNextReviewDate = .Item("agency_next_review_date").Value
                '        End If
                '
                '        IsBranch = .Item("is_branch").Value
                '        IsHeadOffice = .Item("is_head_office").Value
                '        DefaultCommissionPercent = .Item("default_commission_percent").Value
                '
                '        ' CF 280699
                '        AgencyAccountNumber = .Item("agency_account_number").Value
                '        TradingName = .Item("trading_name").Value
                '
                '        If (IsNull(.Item("binder_indicator").Value) = True) Then
                '            BinderIndicator = -1
                '        Else
                '            BinderIndicator = .Item("binder_indicator").Value
                '        End If
                '
                '        If (IsNull(.Item("report_indicator").Value) = True) Then
                '            BinderIndicator = -1
                '        Else
                '            ReportIndicator = .Item("report_indicator").Value
                '        End If
                '
                '        WitholdingTax = .Item("witholding_tax").Value

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetPropertiesFromDB Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetPropertiesFromDB", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)
    ' ***************************************************************** '
    ' Name: AddInputParam (Private)
    '
    ' Description: Adds all of the NON-KEY INPUT parameters
    '              required for an Insert or Update.
    '
    ' ***************************************************************** '
    Private Function AddInputParam() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            If PartyGroupActive = 0 Then

                'developer guide no. 85 (guide)
                m_lReturn = .Parameters.Add("group_active", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="group_active", vValue:=CStr(PartyGroupActive), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="UserId", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="UniqueId", vValue:=m_sUniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ScreenHierarchy", vValue:=m_sScreenHierarchy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '        m_lReturn& = .Parameters.Add( _
            ''              sName:="party_agent_origin_id", _
            ''              vValue:=PartyAgentOriginID, _
            ''              iDirection:=PMParamInput, _
            ''              iDataType:=PMLong)
            '
            '        If (m_lReturn& <> PMTrue) Then
            '            AddInputParam = PMFalse
            '            Exit Function
            '        End If
            '
            '        m_lReturn& = .Parameters.Add( _
            ''              sName:="agency_agreement_date", _
            ''              vValue:=AgencyAgreementDate, _
            ''              iDirection:=PMParamInput, _
            ''              iDataType:=PMDate)
            '
            '        If (m_lReturn& <> PMTrue) Then
            '            AddInputParam = PMFalse
            '            Exit Function
            '        End If
            '
            '        m_lReturn& = .Parameters.Add( _
            ''              sName:="agency_next_review_date", _
            ''              vValue:=AgencyNextReviewDate, _
            ''              iDirection:=PMParamInput, _
            ''              iDataType:=PMDate)
            '
            '        If (m_lReturn& <> PMTrue) Then
            '            AddInputParam = PMFalse
            '            Exit Function
            '        End If'
            '
            '        m_lReturn& = .Parameters.Add( _
            ''              sName:="agency_account_number", _
            ''              vValue:=AgencyAccountNumber, _
            ''              iDirection:=PMParamInput, _
            ''              iDataType:=PMString)
            '
            '        If (m_lReturn& <> PMTrue) Then
            '            AddInputParam = PMFalse
            '            Exit Function
            '        End If
            '
            '        m_lReturn& = .Parameters.Add( _
            ''              sName:="is_branch", _
            ''              vValue:=IsBranch, _
            ''              iDirection:=PMParamInput, _
            ''              iDataType:=PMInteger)
            '
            '        If (m_lReturn& <> PMTrue) Then
            '            AddInputParam = PMFalse
            '            Exit Function
            '        End If
            '
            '        m_lReturn& = .Parameters.Add( _
            ''              sName:="is_head_office", _
            ''              vValue:=IsHeadOffice, _
            ''              iDirection:=PMParamInput, _
            ''              iDataType:=PMInteger)
            '
            '        If (m_lReturn& <> PMTrue) Then
            '            AddInputParam = PMFalse
            '            Exit Function
            '        End If
            '
            '        ' Changed from PMDecimal to PMDouble
            '        m_lReturn& = .Parameters.Add( _
            ''              sName:="default_commission_percent", _
            ''              vValue:=DefaultCommissionPercent, _
            ''              iDirection:=PMParamInput, _
            ''              iDataType:=PMDouble)'
            '
            '        If (m_lReturn& <> PMTrue) Then
            '            AddInputParam = PMFalse
            '            Exit Function
            '        End If
            '
            '        m_lReturn& = .Parameters.Add( _
            ''              sName:="trading_name", _
            ''              vValue:=TradingName, _
            ''              iDirection:=PMParamInput, _
            ''              iDataType:=PMString)
            '
            '        If (m_lReturn& <> PMTrue) Then
            '            AddInputParam = PMFalse
            '            Exit Function
            '        End If
            '
            '        ' If its -1 then nothing was selected in the list, so add a null
            '        If ((IsMissing(BinderIndicator) = True) Or _
            ''            (BinderIndicator = -1)) Then
            '            m_lReturn& = .Parameters.Add( _
            ''                  sName:="binder_indicator", _
            ''                  vValue:=Null, _
            ''                  iDirection:=PMParamInput, _
            ''                  iDataType:=PMLong)
            '        Else
            '            m_lReturn& = .Parameters.Add( _
            ''                  sName:="binder_indicator", _
            ''                  vValue:=BinderIndicator, _
            ''                  iDirection:=PMParamInput, _
            ''                  iDataType:=PMLong)
            '        End If
            '
            '        If (m_lReturn& <> PMTrue) Then
            '            AddInputParam = PMFalse
            '            Exit Function
            '        End If
            '
            '        ' If its -1 then nothing was selected in the list, so add a null
            '        If ((IsMissing(ReportIndicator) = True) Or _
            ''            (ReportIndicator = -1)) Then
            '            m_lReturn& = .Parameters.Add( _
            ''                  sName:="report_indicator", _
            ''                  vValue:=Null, _
            ''                  iDirection:=PMParamInput, _
            ''                  iDataType:=PMLong)
            '        Else
            '            m_lReturn& = .Parameters.Add( _
            ''                  sName:="report_indicator", _
            ''                  vValue:=ReportIndicator, _
            ''                  iDirection:=PMParamInput, _
            ''                  iDataType:=PMLong)
            '        End If
            '
            '        If (m_lReturn& <> PMTrue) Then
            '            AddInputParam = PMFalse
            '            Exit Function
            '        End If
            '
            '        ' CTAF 191200 Changed from PMDecimal to PMDouble
            '        m_lReturn& = .Parameters.Add( _
            ''              sName:="witholding_tax", _
            ''              vValue:=WitholdingTax, _
            ''              iDirection:=PMParamInput, _
            ''              iDataType:=PMDouble)
            '
            '        If (m_lReturn& <> PMTrue) Then
            '            AddInputParam = PMFalse
            '            Exit Function
            '        End If

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddKeyInputParam (Private)
    '
    ' Description: Adds all of the PRIMARY KEY INPUT parameters
    '              required for a Select, Delete or Update.
    '
    ' ***************************************************************** '
    Private Function AddKeyInputParam() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            m_lReturn = .Parameters.Add(sName:="party_cnt", vValue:=CStr(PartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddKeyOutputParam (Private)
    '
    ' Description: Adds all of the PRIMARY KEY OUTPUT parameters
    '              required for an Add.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (AddKeyOutputParam) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function AddKeyOutputParam() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'With m_oDatabase
    '
    'm_lReturn = .Parameters.Add(sName:="party_cnt", vValue:=CStr(0), idirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
    '
    'End With
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
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddKeyOutputParam Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddKeyOutputParam", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: GetNewPrimaryKeyID (Private)
    '
    ' Description: Returns the new PRIMARY KEY values from an Add.
    '
    ' ***************************************************************** '
    Private Function GetNewPrimaryKeyID() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            PartyCnt = .Parameters.Item("party_cnt").Value

        End With

        Return result

    End Function
    ' PRIVATE Methods (End)

    Public Sub New()
        MyBase.New()

        ' Class Initialise

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error.
        '
        ' Log Error Message
        'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class