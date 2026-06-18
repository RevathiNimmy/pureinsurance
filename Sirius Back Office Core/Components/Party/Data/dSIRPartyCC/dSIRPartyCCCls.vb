Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("SIRPartyCC_NET.SIRPartyCC")> _
Public NotInheritable Class SIRPartyCC
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: SIRPartyCC
    '
    ' Date: 04/09/1998
    '
    ' Description: Describes the SIRPartyCC attributes.
    '
    ' Edit History:
    ' SP050199 - trading since date and companyreg now non-mandatory
    '
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
    Private Const ACClass As String = "SIRPartyCC"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Database Class
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag
    Private m_bCloseDatabase As Boolean

    ' DataBase Attributes
    Private m_lPartyCnt As Integer
    Private m_vCompanyReg As Object
    Private m_vTradingSinceDate As Object
    Private m_vPartyBusinessId As Object
    Private m_vLocation As Object
    Private m_vNoOfOffices As Object
    Private m_vNoOfEmployees As Object
    Private m_vFinancialYear As Object
    Private m_vTradeCode As Object
    Private m_vWageRoll As Object
    Private m_vTurnover As Object
    'Private m_vSeasonalGiftId As Variant
    'Private m_vStrengthCodeId As Variant
    Private m_vSICCodeId As Object
    'Private m_vPreviousInsurerCnt As Variant
    'Private m_vPreviousBrokerCnt As Variant
    Private m_vSalutation As Object
    ' AMB 21-Oct-03: 1.8.6 Folgate EL0037 development - trade id added
    'developer guide no. 17
    Private m_vTradeID As Object

    Private m_bEvent As Boolean

    '+++ JJ 22/07/2003 PN249
    Private m_vSource As Object
    Private m_vTPSind As Object
    '--- JJ 22/07/2003

    'DD 24/10/2003
    Private m_vMailshot As Object
    Private m_vEMPSind As Object
    Private m_vTPPassword As Object
    Private m_vIsFeeClient As Object

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

    Public Property CompanyReg() As Object
        Get

            Return m_vCompanyReg

        End Get
        Set(ByVal Value As Object)



            m_vCompanyReg = Value

        End Set
    End Property

    Public Property TradingSinceDate() As Object
        Get

            Return m_vTradingSinceDate

        End Get
        Set(ByVal Value As Object)



            m_vTradingSinceDate = Value

        End Set
    End Property

    Public Property PartyBusinessId() As Object
        Get

            Return m_vPartyBusinessId

        End Get
        Set(ByVal Value As Object)



            m_vPartyBusinessId = Value

        End Set
    End Property

    Public Property Location() As Object
        Get

            Return m_vLocation

        End Get
        Set(ByVal Value As Object)



            m_vLocation = Value

        End Set
    End Property

    Public Property NoOfOffices() As Object
        Get

            Return m_vNoOfOffices

        End Get
        Set(ByVal Value As Object)



            m_vNoOfOffices = Value

        End Set
    End Property

    Public Property NoOfEmployees() As Object
        Get

            Return m_vNoOfEmployees

        End Get
        Set(ByVal Value As Object)



            m_vNoOfEmployees = Value

        End Set
    End Property

    Public Property FinancialYear() As Object
        Get

            Return m_vFinancialYear

        End Get
        Set(ByVal Value As Object)



            m_vFinancialYear = Value

        End Set
    End Property

    Public Property TradeCode() As Object
        Get

            Return m_vTradeCode

        End Get
        Set(ByVal Value As Object)



            m_vTradeCode = Value

        End Set
    End Property

    ' CF 070799 -
    Public Property WageRoll() As Object
        Get

            Return m_vWageRoll

        End Get
        Set(ByVal Value As Object)



            m_vWageRoll = Value

        End Set
    End Property

    Public Property Turnover() As Object
        Get

            Return m_vTurnover

        End Get
        Set(ByVal Value As Object)



            m_vTurnover = Value

        End Set
    End Property



    Public Property SICCodeId() As Object
        Get

            Return m_vSICCodeId

        End Get
        Set(ByVal Value As Object)



            m_vSICCodeId = Value

        End Set
    End Property


    Public Property Salutation() As String
        Get

            Return m_vSalutation

        End Get
        Set(ByVal Value As String)


            m_vSalutation = CStr(Value)

        End Set
    End Property

    'developer guide no. 17
    Public Property TradeID() As Object
        Get
            ' AMB 21-Oct-03: 1.8.6 Folgate EL0037 development - trade id
            Return m_vTradeID
        End Get
        Set(ByVal Value As Object)
            ' AMB 21-Oct-03: 1.8.6 Folgate EL0037 development - trade id

            m_vTradeID = Value
        End Set
    End Property

    Public Property FromEvent() As Boolean
        Get

            Return m_bEvent

        End Get
        Set(ByVal Value As Boolean)

            m_bEvent = Value

        End Set
    End Property

    ' +++ JJ 22/07/2003 pn249
    Public Property Source() As String
        Get

            Return m_vSource

        End Get
        Set(ByVal Value As String)


            m_vSource = CStr(Value)

        End Set
    End Property

    Public Property TPSind() As Object
        Get

            Return m_vTPSind

        End Get
        Set(ByVal Value As Object)


            m_vTPSind = Value

        End Set
    End Property
    ' --- JJ 22/07/2003
    'DD 24/10/2003
    Public Property Mailshot() As Object
        Get
            Return m_vMailshot
        End Get
        Set(ByVal Value As Object)

            m_vMailshot = Value
        End Set
    End Property

    Public Property EMPSind() As Object
        Get
            Return m_vEMPSind
        End Get
        Set(ByVal Value As Object)

            m_vEMPSind = Value
        End Set
    End Property


    Public Property TPPassword() As String
        Get
            Return m_vTPPassword
        End Get
        Set(ByVal Value As String)

            m_vTPPassword = CStr(Value)
        End Set
    End Property

    'developer guide no. 17
    Public Property IsFeeClient() As Object
        Get
            Return m_vIsFeeClient
        End Get
        Set(ByVal Value As Object)

            m_vIsFeeClient = Value
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
    Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer


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
    '
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
    '
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
    '
    ' Name: SelectSingle (Public)
    '
    ' Description: Selects the required SIRPartyCC
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

                If FromEvent Then
                    ' Execute SQL Statement
                    m_lReturn = .SQLSelect(sSQL:=ACSelectSingleEventSQL, sSQLName:=ACSelectSingleEventName, bStoredProcedure:=ACSelectSingleEventStored, bKeepNulls:=True)
                Else
                    ' Execute SQL Statement
                    m_lReturn = .SQLSelect(sSQL:=ACSelectSingleSQL, sSQLName:=ACSelectSingleName, bStoredProcedure:=ACSelectSingleStored, bKeepNulls:=True)
                End If

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
                'developer guide no. 111 (Guide)    
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
    ' Description: Sets the supplied SIRPartyCC properties from a database
    '              record.
    ' ***************************************************************** '
    'developer guide no. 112(guide)
    Public Function SetPropertiesFromDB(ByRef oFields As DataRow) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Populate Base Details
            With oFields

                'SP050199 trading since date and companyreg now non-mandatory
                PartyCnt = gPMFunctions.NullToLong(oFields("party_cnt"))

                If Convert.IsDBNull(oFields("company_reg")) Or Informations.IsNothing(oFields("company_reg")) Then

                    CompanyReg = Nothing
                Else

                    CompanyReg = oFields("company_reg")
                End If

                If Convert.IsDBNull(oFields("trading_since_date")) Or Informations.IsNothing(oFields("trading_since_date")) Then

                    TradingSinceDate = Nothing
                Else

                    TradingSinceDate = oFields("trading_since_date")
                End If

                If Convert.IsDBNull(oFields("party_business_id")) Or Informations.IsNothing(oFields("party_business_id")) Then

                    PartyBusinessId = Nothing
                Else

                    PartyBusinessId = oFields("party_business_id")
                End If

                If Convert.IsDBNull(oFields("location")) Or Informations.IsNothing(oFields("location")) Then

                    Location = Nothing
                Else

                    Location = oFields("location")
                End If

                If Convert.IsDBNull(oFields("no_of_offices")) Or Informations.IsNothing(oFields("no_of_offices")) Then

                    NoOfOffices = Nothing
                Else

                    NoOfOffices = oFields("no_of_offices")
                End If

                If Convert.IsDBNull(oFields("no_of_employees")) Or Informations.IsNothing(oFields("no_of_employees")) Then

                    NoOfEmployees = Nothing
                Else

                    NoOfEmployees = oFields("no_of_employees")
                End If

                If Convert.IsDBNull(oFields("financial_year")) Or Informations.IsNothing(oFields("financial_year")) Then

                    FinancialYear = Nothing
                Else

                    FinancialYear = oFields("financial_year")
                End If

                If Convert.IsDBNull(oFields("trade_code")) Or Informations.IsNothing(oFields("trade_code")) Then

                    TradeCode = Nothing
                Else

                    TradeCode = oFields("trade_code")
                End If
                ' CF 070799 - Added following

                If Convert.IsDBNull(oFields("wage_roll")) Or Informations.IsNothing(oFields("wage_roll")) Then

                    WageRoll = Nothing
                Else

                    WageRoll = oFields("wage_roll")
                End If

                If Convert.IsDBNull(oFields("turnover")) Or Informations.IsNothing(oFields("turnover")) Then

                    Turnover = Nothing
                Else

                    Turnover = oFields("turnover")
                End If


                If Convert.IsDBNull(oFields("SIC_code_id")) Or Informations.IsNothing(oFields("SIC_code_id")) Then

                    SICCodeId = Nothing
                Else

                    SICCodeId = oFields("SIC_code_id")
                End If
                ' CTAF 250701

                If Convert.IsDBNull(oFields("salutation")) Or Informations.IsNothing(oFields("salutation")) Then
                    Salutation = ""
                Else
                    Salutation = oFields("salutation")
                End If

                ' +++ JJ 22/07/2003 PN249


                If Convert.IsDBNull(oFields("source")) Or Informations.IsNothing(oFields("source")) Then
                    Source = ""
                Else
                    Source = oFields("source")
                End If


                If Convert.IsDBNull(oFields("tpsind")) Or Informations.IsNothing(oFields("tpsind")) Then
                    TPSind = 0
                Else
                    TPSind = oFields("tpsind")
                End If

                ' --- JJ 22/07/2003 PN249

                ' AMB 21-Oct-03: 1.8.6 Folgate EL0037 development - trade_id

                If Convert.IsDBNull(oFields("trade_id")) Or Informations.IsNothing(oFields("trade_id")) Then
                    TradeID = 0
                Else
                    TradeID = oFields("trade_id")
                End If

                'DD 24/10/2003

                If Convert.IsDBNull(oFields("mailshot")) Or Informations.IsNothing(oFields("mailshot")) Then
                    Mailshot = 0
                Else
                    Mailshot = oFields("mailshot")
                End If


                If Convert.IsDBNull(oFields("empsind")) Or Informations.IsNothing(oFields("empsind")) Then
                    EMPSind = 0
                Else
                    EMPSind = oFields("empsind")
                End If


                If Convert.IsDBNull(oFields("tp_password")) Or Informations.IsNothing(oFields("tp_password")) Then
                    TPPassword = ""
                Else
                    TPPassword = oFields("tp_password")
                End If


                If Convert.IsDBNull(oFields("is_fee_client")) Or Informations.IsNothing(oFields("is_fee_client")) Then

                    IsFeeClient = Nothing
                Else
                    IsFeeClient = If(oFields("is_fee_client"), 1, 0)
                End If

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetPropertiesFromDB Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetPropertiesFromDB", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CopyPartyCCToEvent (Public)
    '
    ' Description: Makes a copy of the party CC on the event table.
    '
    ' ***************************************************************** '
    Public Function CopyPartyCCToEvent(ByVal v_lEventCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            'developer guide no. 98
            m_lReturn = m_oDatabase.Parameters.Add(sName:="event_cnt", vValue:=v_lEventCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'developer guide no. 98
            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=PartyCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCopyPartyCCToEventSQL, sSQLName:=ACCopyPartyCCToEventName, bStoredProcedure:=ACCopyPartyCCToEventStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyPartyCCToEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPartyCCToEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CopyPartyToEvent (Public)
    '
    ' Description: Makes a copy of the party on the event table.
    '
    ' ***************************************************************** '
    Public Function CopyPartyToEvent(ByVal v_lEventCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            'developer guide no. 98
            m_lReturn = m_oDatabase.Parameters.Add(sName:="event_cnt", vValue:=v_lEventCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'developer guide no. 98
            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=PartyCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCopyPartyToEventSQL, sSQLName:=ACCopyPartyToEventName, bStoredProcedure:=ACCopyPartyToEventStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyPartyToEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPartyToEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            m_lReturn = .Parameters.Add(sName:="company_reg", vValue:=CompanyReg, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="trading_since_date", vValue:=TradingSinceDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '        m_lReturn& = .Parameters.Add( _
            'sName:="party_business_id", _
            'vValue:=PartyBusinessId, _
            'iDirection:=PMParamInput, _
            'iDataType:=PMlong)

            m_lReturn = .Parameters.Add(sName:="party_business_id", vValue:=PartyBusinessId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="location", vValue:=Location, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="no_of_offices", vValue:=NoOfOffices, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="no_of_employees", vValue:=NoOfEmployees, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="financial_year", vValue:=FinancialYear, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="trade_code", vValue:=TradeCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="wage_roll", vValue:=WageRoll, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="turnover", vValue:=Turnover, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="SIC_code_id", vValue:=SICCodeId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' CTAF 250701
            ' CTAF 20020802 Who on earth would add a string as a Long...
            m_lReturn = .Parameters.Add(sName:="salutation", vValue:=Salutation, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="source", vValue:=Source, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'developer guide no. 98
            m_lReturn = .Parameters.Add(sName:="tpsind", vValue:=TPSind, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' AMB 21-Oct-03: 1.8.6 Folgate EL0037 development - trade_id
            'developer guide no. 98
            m_lReturn = .Parameters.Add(sName:="trade_id", vValue:=TradeID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' DD 24/10/2003
            'developer guide no. 98
            m_lReturn = .Parameters.Add(sName:="mailshot", vValue:=Mailshot, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'developer guide no. 98
            m_lReturn = .Parameters.Add(sName:="empsind", vValue:=EMPSind, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="tp_password", vValue:=TPPassword, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'developer guide no. 98
            m_lReturn = .Parameters.Add(sName:="is_fee_client", vValue:=IsFeeClient, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

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
            'developer guide no. 98
            m_lReturn = .Parameters.Add(sName:="party_cnt", vValue:=PartyCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

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
    '
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

