Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

'Develper Guide No: 129
Imports SharedFiles

<System.Runtime.InteropServices.ProgId("CLMInfoChklst_NET.CLMInfoChklst")> _
Public NotInheritable Class CLMInfoChklst
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: CLMInfoChklst
    ' Date: 06/10/1998
    ' Description: Describes the CLMInfoChklst attributes.
    ' Edit History:
    ' ***************************************************************** '

    ' ************************************************
    ' Added to replace global variables 11/12/2003
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
    Private Const ACClass As String = "CLMSalvageRecovery"

    ' Database Class
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag
    Private m_bCloseDatabase As Boolean

    ' DataBase Attributes-Recovery Table
    Private m_lClmExpServId As Integer
    Private m_lClaim_Id As Integer
    Private m_lExpServId As Integer
    Private m_lPrtyClmId As Integer
    Private m_lServTypeId As Integer
    Private m_sService As String = ""
    Private m_sDescription As String = ""
    Private m_sReference As String = ""
    Private m_sContact As String = ""
    Private m_dtDateReq As Object
    Private m_dtDateCrit As Object
    Private m_dtDateRecv As Object

    'Database Attribute For Identifying the Table
    Private m_lTable As Integer

    ' Function Return Code
    Private m_lReturn As gPMConstants.PMEReturnCode

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
    Public Property ClmExpServId() As Integer
        Get
            Return m_lClmExpServId
        End Get
        Set(ByVal Value As Integer)
            m_lClmExpServId = Value
        End Set
    End Property
    Public Property ExpServId() As Integer
        Get
            Return m_lExpServId
        End Get
        Set(ByVal Value As Integer)
            m_lExpServId = Value
        End Set
    End Property
    Public Property ServTypeId() As Integer
        Get
            Return m_lServTypeId
        End Get
        Set(ByVal Value As Integer)
            m_lServTypeId = Value
        End Set
    End Property
    Public Property Service() As String
        Get
            Return m_sService
        End Get
        Set(ByVal Value As String)
            m_sService = Value
        End Set
    End Property
    Public Property Description() As String
        Get
            Return m_sDescription
        End Get
        Set(ByVal Value As String)
            m_sDescription = Value
        End Set
    End Property
    Public Property PrtyClmId() As Integer
        Get
            Return m_lPrtyClmId
        End Get
        Set(ByVal Value As Integer)
            m_lPrtyClmId = Value
        End Set
    End Property
    Public Property Claim_Id() As Integer
        Get
            Return m_lClaim_Id
        End Get
        Set(ByVal Value As Integer)
            m_lClaim_Id = Value
        End Set
    End Property
    Public Property Reference() As String
        Get
            Return m_sReference
        End Get
        Set(ByVal Value As String)
            m_sReference = Value
        End Set
    End Property
    'Properties for Receipt Table
    Public Property Contact() As String
        Get
            Return m_sContact
        End Get
        Set(ByVal Value As String)
            m_sContact = Value
        End Set
    End Property
    Public Property DateReq() As Object
        Get
            Return m_dtDateReq
        End Get
        Set(ByVal Value As Object)


            m_dtDateReq = Value
        End Set
    End Property
    Public Property DateCrit() As Object
        Get
            Return m_dtDateCrit
        End Get
        Set(ByVal Value As Object)


            m_dtDateCrit = Value
        End Set
    End Property
    Public Property DateRecv() As Object
        Get
            Return m_dtDateRecv
        End Get
        Set(ByVal Value As Object)


            m_dtDateRecv = Value
        End Set
    End Property

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

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the required INPUT parameters
            m_lReturn = CType(AddInputParam(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add PrimaryKey as OUTPUT parameters
            m_lReturn = CType(AddKeyOutputParam(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Primary Key of the record inserted
            m_lReturn = CType(GetNewPrimaryKeyID(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Add Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the required INPUT parameters
            m_lReturn = CType(AddInputParam(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add PrimaryKey as INPUT parameters
            m_lReturn = CType(AddKeyInputParam(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateSQL, sSQLName:=ACUpdateName, bStoredProcedure:=ACUpdateStored, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check to see that the record was updated OK
            If lRecordsAffected > 0 Then
                ' Updated No action required
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Delete (Public)
    '
    ' Description: Deletes a single record from the database.
    '   OBSELETE
    ' ***************************************************************** '
    Public Function Delete() As Integer
        Return gPMConstants.PMEReturnCode.PMTrue
    End Function

    ' ***************************************************************** '
    ' Name: SetPropertiesFromDB (Public)
    '
    ' Description: Sets the supplied SIRContact properties from a database
    '              record.
    ' ***************************************************************** '

    'Developer Guide No: 112
    Public Function SetPropertiesFromDB(ByRef oFields As DataRow) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Populate Base Details

            With oFields

                ClmExpServId = gPMFunctions.NullToLong(oFields("Recovery_id"))
                ExpServId = gPMFunctions.NullToLong(oFields("Peril_id"))

                Claim_Id = gPMFunctions.NullToLong(oFields("Recovery_Type_id"))


                If Convert.IsDBNull(oFields("Initial_reserve")) Or IsNothing(oFields("Initial_reserve")) Then

                    ServTypeId = Nothing
                Else
                    ServTypeId = oFields("Initial_Reserve")
                End If


                If Convert.IsDBNull(oFields("Revised_reserve")) Or IsNothing(oFields("Revised_reserve")) Then
                    Service = CStr(0)
                Else
                    Service = oFields("Revised_reserve")
                End If



                If Convert.IsDBNull(oFields("Currency_id")) Or IsNothing(oFields("Currency_id")) Then
                    PrtyClmId = 0
                Else
                    PrtyClmId = oFields("Currency_id")
                End If


                If Convert.IsDBNull(oFields("Received_To_Date")) Or IsNothing(oFields("Received_To_Date")) Then
                    Description = CStr(0)
                Else
                    Description = oFields("Received_To_Date")
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetPropertiesFromDB Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetPropertiesFromDB", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddInputParam (Private)
    '
    ' Description: Adds all of the NON-KEY INPUT parameters
    '              required for an Insert or Update.
    '
    ''JMK 18/05/2001 - cDate all variant dates before passing to DB
    ' ***************************************************************** '
    Private Function AddInputParam() As Integer

        Dim result As Integer = 0

        Dim vExpServId As Object
        Dim vPrtyClmId As Object



        result = gPMConstants.PMEReturnCode.PMTrue
        'develoepr guide no.85
        'start
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_id", vValue:=Claim_Id, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        If ExpServId = 0 Then

            vExpServId = Nothing
        Else
            vExpServId = ExpServId
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="Expert_Service_id", vValue:=vExpServId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        If PrtyClmId = 0 Then

            vPrtyClmId = Nothing
        Else
            vPrtyClmId = PrtyClmId
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="Party_Claim_id", vValue:=vPrtyClmId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Service_type_id", vValue:=ServTypeId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        'end
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Service", vValue:=Service, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="Description", vValue:=Description, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="Reference", vValue:=Reference, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="Contact", vValue:=Contact, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'JMK 18/05/2001 - cdate all dates - DateReq
        If Not Information.IsDate(DateReq) Then


            'Develper Guide No: 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Date_requested", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

        Else
            'developer guide no.40
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Date_requested", vValue:=DateReq, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        '10
        'JMK 18/05/2001 - cdate all dates - DateCrit
        If Not Information.IsDate(DateCrit) Then


            'Develper Guide No: 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Date_critical", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
        Else
            'developer guide no.40
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Date_critical", vValue:=DateCrit, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        '11
        'JMK 18/05/2001 - cdate all dates - DateRecv
        If Not Information.IsDate(DateRecv) Then


            'Develper Guide No: 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Date_received", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
        Else
            'developer guide no.40
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Date_received", vValue:=DateRecv, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

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

        m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_Expert_Service_id", vValue:=CStr(ClmExpServId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddKeyOutputParam (Private)
    '
    ' Description: Adds all of the PRIMARY KEY OUTPUT parameters
    '              required for an Add.
    '
    ' ***************************************************************** '
    Private Function AddKeyOutputParam() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_Expert_Service_id", vValue:=CStr(0), idirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetNewPrimaryKeyID (Private)
    '
    ' Description: Returns the new PRIMARY KEY values from an Add.
    '
    ' ***************************************************************** '
    Private Function GetNewPrimaryKeyID() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ClmExpServId = m_oDatabase.Parameters.Item("Claim_Expert_Service_id").Value

        Return result

    End Function

    ' PRIVATE Methods (End)
    Public Sub New()
        MyBase.New()
        ' Class Initialise

        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error.
        '
        ' Log Error Message
        'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class

