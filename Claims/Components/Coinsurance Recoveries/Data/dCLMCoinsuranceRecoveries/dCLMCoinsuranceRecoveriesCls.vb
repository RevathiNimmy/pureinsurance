Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
'Developer Guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Data_NET.Data")> _
Public NotInheritable Class Data
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: CLMCoinsuranceRecoveries
    '
    ' Date: {TodaysDate}
    '
    ' Description: Describes the CLMCoinsuranceRecoveries attributes.
    '
    ' Edit History:
    '               '16 May 2002: SET
    '                           GetMainShare: PartyID parameter replaced with PolicyID
    '
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
    Private m_oDatabase As dPMDAO.Database
    Private m_iTask As Integer

    Private m_sTransactionType As String = ""
    ' ************************************************


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "CLMCoinsuranceRecoveries"

    ' Close Database Flag
    Private m_bCloseDatabase As Boolean

    ' DataBase Attributes
    Private m_lPartyID As Integer
    Private m_lClaimID As Integer
    Private m_sPartyName As String = ""
    Private m_lInsuranceFileCnt As Integer
    Private m_dShare As Double
    Private m_cShareValue As Decimal

    ' Function Return Code
    Private m_lReturn As gPMConstants.PMEReturnCode

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)
            m_sTransactionType = Value
        End Set
    End Property


    Public Property ClaimID() As Integer
        Get
            Return m_lClaimID
        End Get
        Set(ByVal Value As Integer)
            m_lClaimID = Value
        End Set
    End Property


    Public Property PartyName() As String
        Get
            Return m_sPartyName
        End Get
        Set(ByVal Value As String)
            m_sPartyName = Value
        End Set
    End Property


    Public Property PartyID() As Integer
        Get
            Return m_lPartyID
        End Get
        Set(ByVal Value As Integer)
            m_lPartyID = Value
        End Set
    End Property


    Public Property InsuranceFileCnt() As Integer
        Get
            Return m_lInsuranceFileCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property


    Public Property Share() As Double
        Get
            Return m_dShare
        End Get
        Set(ByVal Value As Double)
            m_dShare = Value
        End Set
    End Property


    Public Property ShareValue() As Decimal
        Get
            Return m_cShareValue
        End Get
        Set(ByVal Value As Decimal)
            m_cShareValue = Value
        End Set
    End Property


    Public Property Task() As Integer
        Get
            Return m_iTask
        End Get
        Set(ByVal Value As Integer)
            m_iTask = Value
        End Set
    End Property

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

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal v_sUsername As String, ByVal v_sPassword As String, ByVal v_iUserID As Integer, ByVal v_iSourceID As Integer, ByVal v_iLanguageID As Integer, ByVal v_iCurrencyID As Integer, ByVal v_iLogLevel As Integer, ByVal v_sCallingAppName As String, Optional ByVal v_vDatabase As dPMDAO.Database = Nothing) As Integer



        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = v_sUsername
            m_sPassword = v_sPassword
            m_iUserID = v_iUserID
            m_sCallingAppName = v_sCallingAppName
            m_iLanguageID = v_iLanguageID
            m_iSourceID = v_iSourceID
            m_iCurrencyID = v_iCurrencyID
            m_iLogLevel = v_iLogLevel
            m_oDatabase = v_vDatabase

            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=v_vDatabase), gPMConstants.PMEReturnCode)


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

            '    With m_oDatabase

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the required INPUT parameters
            'DC141200
            'm_lReturn& = AddInputParam(Task)
            '        m_lReturn& = AddInputParam()
            '
            '        If (m_lReturn& <> PMTrue) Then
            '            Add = PMFalse
            '            Exit Function
            '        End If

            ' Add PrimaryKey as OUTPUT parameters
            '        m_lReturn& = AddKeyOutputParam()
            '
            '        If (m_lReturn& <> PMTrue) Then
            '            Add = PMFalse
            '            Exit Function
            '        End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimID", vValue:=CStr(ClaimID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="PartyID", vValue:=CStr(PartyID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Share", vValue:=CStr(Share), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Share_Value", vValue:=CStr(ShareValue), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Primary Key of the record inserted
            '        m_lReturn& = GetNewPrimaryKeyID()
            '
            '        If (m_lReturn& <> PMTrue) Then
            '            Add = PMFalse
            '            Exit Function
            '        End If

            '    End With

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

            '    With m_oDatabase

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the required INPUT parameters
            '        m_lReturn& = AddInputParam()
            '
            '        If (m_lReturn& <> PMTrue) Then
            '            Update = PMFalse
            '            Exit Function
            '        End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimID", vValue:=CStr(ClaimID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="PartyID", vValue:=CStr(PartyID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Share", vValue:=CStr(Share), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Share_Value", vValue:=CStr(ShareValue), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

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

            '    End With

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
    '
    ' ***************************************************************** '
    Public Function Delete() As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '    With m_oDatabase

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add PrimaryKey as INPUT parameters
            'DC141200
            'm_lReturn& = AddInputParam(Task)
            '        m_lReturn& = AddInputParam()
            '
            '        If (m_lReturn& <> PMTrue) Then
            '            Delete = PMFalse
            '            Exit Function
            '        End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimID", vValue:=CStr(ClaimID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="PartyID", vValue:=CStr(PartyID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteSQL, sSQLName:=ACDeleteName, bStoredProcedure:=ACDeleteStored, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If record wasn't deleted, error
            If lRecordsAffected > 0 Then
                ' Deleted, No action required
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Delete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Delete", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SelectSingle (Public)
    '
    ' Description: Selects the required CLMCoinsuranceRecoveries
    '
    ' ***************************************************************** '
    Public Function SelectSingle(Optional ByRef vLockMode As Object = Nothing) As Integer

        'Dim lRecordCount As Long
        '
        Dim result As Integer = 0
        Try


            '    With m_oDatabase
            '
            '        ' Clear the Database Parameters Collection
            '        .Parameters.Clear
            '
            '        ' Default to No Lock if not supplied or not numeric
            '        If (IsMissing(vLockMode) = True) _
            ''        Or (IsNumeric(vLockMode) = False) Then
            '            vLockMode = PMNoLock
            '        End If
            '
            '        ' Add PrimaryKey as INPUT parameters
            '        m_lReturn& = AddInputParam()
            '
            '        If (m_lReturn& <> PMTrue) Then
            '            SelectSingle = PMFalse
            '            Exit Function
            '        End If
            '
            '        ' Execute SQL Statement
            '        m_lReturn& = .SQLSelect( _
            ''            sSQL:=ACSelectSingleSQL, _
            ''            sSQLName:=ACSelectSingleName, _
            ''            bStoredProcedure:=ACSelectSingleStored)
            '
            '        If (m_lReturn& <> PMTrue) Then
            '            SelectSingle = PMFalse
            '            Exit Function
            '        End If
            '
            '        ' How many records were selected
            '        lRecordCount& = .Records.Count
            '
            '          ' Do we have any records ?
            '        If (lRecordCount& = 1) Then
            '            ' Selected, No action required
            '        Else
            '            SelectSingle = PMNotFound
            '            Exit Function
            '        End If
            '
            '        ' Set properties
            '        m_lReturn& = SetPropertiesFromDB( _
            ''            oFields:=.Records.Item(1).Fields)
            '
            '        If (m_lReturn& <> PMTrue) Then
            '            SelectSingle = PMFalse
            '            Exit Function
            '        End If
            '
            '    End With
            '
            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectSingle Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectSingle", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetPropertiesFromDB (Public)
    '
    ' Description: Sets the supplied CLMCoinsuranceRecoveries properties from a database
    '              record.
    ' ***************************************************************** '
    'Developer Guide no. 112
    Public Function SetPropertiesFromDB(ByRef oFields As DataRow) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Populate Base Details

            With oFields

                ' SET 010082002 - Scalability
                PartyName = gPMFunctions.NullToString(oFields("name"))

                If Convert.IsDBNull(oFields("Party_cnt")) Or IsNothing(oFields("Party_cnt")) Then
                    PartyID = 0
                Else
                    PartyID = oFields("Party_cnt")
                End If

                '        If Task = PMEdit Then

                If Convert.IsDBNull(oFields("Share")) Or IsNothing(oFields("Share")) Then
                    Share = 0
                Else
                    Share = oFields("Share")
                End If


                If Convert.IsDBNull(oFields("Share_Value")) Or IsNothing(oFields("Share_Value")) Then
                    ShareValue = 0
                Else
                    ShareValue = oFields("Share_Value")
                End If
                '        Else
                '            If (IsNull(.Item("Share_premium").Value) = True) Then
                '                ShareValue = 0
                '            Else
                '                ShareValue = .Item("Share_premium").Value
                '            End If
                '
                '            If (IsNull(.Item("Share_percent").Value) = True) Then
                '                Share = 0
                '            Else
                '                Share = .Item("Share_percent").Value
                '            End If
                '        End If
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
    ' ***************************************************************** '
    'DC141200
    'Private Function AddInputParam(ByVal v_iTask As Integer) As Long

    'Private Function AddInputParam() As Integer
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    '    With m_oDatabase
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimID", vValue:=CStr(ClaimID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="PartyID", vValue:=CStr(PartyID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="Share", vValue:=CStr(Share), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="Share_Value", vValue:=CStr(ShareValue), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="PolicyID", vValue:=CStr(InsuranceFileCnt), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'DC141200
    '        m_lReturn = m_oDatabase.Parameters.Add( _
    ''                sName:="Mode", _
    ''                vValue:=v_iTask, _
    ''                iDirection:=PMParamInput, _
    ''                iDatatype:=PMInteger)
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="Mode", vValue:=CStr(Task), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    '    End With
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
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddInputParam Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddInputParam", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: AddKeyInputParam (Private)
    '
    ' Description: Adds all of the PRIMARY KEY INPUT parameters
    '              required for a Select, Delete or Update.
    '
    ' ***************************************************************** '

    'Private Function AddKeyInputParam() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    '    With m_oDatabase
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="PartyID", vValue:=CStr(PartyID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    '    End With
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
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddKeyInputParam Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddKeyInputParam", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: AddKeyOutputParam (Private)
    '
    ' Description: Adds all of the PRIMARY KEY OUTPUT parameters
    '              required for an Add.
    '
    ' ***************************************************************** '

    'Private Function AddKeyOutputParam() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    '
    '    With m_oDatabase
    '
    '    End With
    '
    'Return gPMConstants.PMEReturnCode.PMTrue
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddKeyOutputParam Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddKeyOutputParam", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
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

    'Private Function GetNewPrimaryKeyID() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    '
    '    With m_oDatabase
    '
    '
    '
    '    End With
    '
    'Return gPMConstants.PMEReturnCode.PMTrue
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNewPrimaryKeyID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNewPrimaryKeyID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

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


    ' ***************************************************************** '
    ' Class name: Business
    '
    ' Name: GetParty (Public)
    '
    ' Description:Gets the list of Party Names when the Claim iD is
    '             passed
    '
    'Author : Ranjit

    ' Date : 08 June 2000
    ' ***************************************************************** '

    Public Function GetParty(ByRef r_vPartyName(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            r_vPartyName = Nothing
            ' Clear the Parameters Array
            '       With m_oDatabase
            m_oDatabase.Parameters.Clear()

            '            m_lReturn = AddInputParam()
            '            If m_lReturn <> PMTrue Then
            '               GetParty = PMFalse
            '               Exit Function
            '            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPartySQl, sSQLName:=ACGetPartyName, bStoredProcedure:=ACGetPartyStored, vResultArray:=r_vPartyName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            '       End With
            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Class name: Business
    '
    ' Name: GetDetailsShare (Public)
    '
    ' Description: Gets the Details of Share Value and Share Percent
    '               for the details screen
    '
    '
    ' Date : 08 June 2000
    '
    ' Author: Ranjit
    ' ***************************************************************** '

    Public Function GetDetailsShare(ByRef r_vShare(,) As Object) As Integer

        '***Wirte the code for passing the parameters and collecting the resultset
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'Set m_oDatabase = New dPMDAO.Database
            ' Clear the Parameters Array

            '       With m_oDatabase
            m_oDatabase.Parameters.Clear()

            ' Add the Paramters to the Input
            '            m_lReturn = AddInputParam()
            '
            '            If m_lReturn <> PMTrue Then
            '                 GetDetailsShare = PMFalse
            '                 Exit Function
            '            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimID", vValue:=CStr(ClaimID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="PartyID", vValue:=CStr(PartyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            r_vShare = Nothing
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetDetailsShareSQL, sSQLName:=ACGetDetailsShareName, bStoredProcedure:=ACGetDetailsShareStored, vResultArray:=r_vShare)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            '        End With
            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetailsShare Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetailsShare", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Class Name : Business
    '
    ' Name: GetMainShare (Public)
    '
    ' Description: Gets the Details of Share Value and Share Percent
    '               for the main screen
    '
    ' Author : Ranjit
    '
    ' Date : 08 June 2000
    '' ***************************************************************** '

    Public Function GetMainShare(ByRef r_vShare(,) As Object) As Integer

        '***Wirte the code for passing the parameters and collecting the resultset
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'Set m_oDatabase = New dPMDAO.Database
            ' Clear the Parameters Array
            '       With m_oDatabase
            m_oDatabase.Parameters.Clear()

            '            m_lReturn = AddInputParam()
            '
            '            If m_lReturn <> PMTrue Then
            '                 GetMainShare = PMFalse
            '                 Exit Function
            '            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimID", vValue:=CStr(ClaimID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'SET16052002 - PartyID parameter replaced with PolicyID
            m_lReturn = m_oDatabase.Parameters.Add(sName:="PolicyID", vValue:=CStr(InsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            'SET16052002 - End

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            r_vShare = Nothing
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetMainShareSQl, sSQLName:=ACGetMainShareName, bStoredProcedure:=ACGetMainShareStored, vResultArray:=r_vShare)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            '       End With
            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetMainShare Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMainShare", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Class Name : Business
    '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the Details of Reinsurers to populate the List View
    '
    ' Author: Ranjit
    '
    ' Date : 08 June 2000
    ' ***************************************************************** '
    Public Function GetDetails(ByRef v_vResultArray(,) As Object) As Integer
        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        '***Wirte the code for passing the parameters and collecting the resultset
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimID", vValue:=CStr(ClaimID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="PolicyID", vValue:=CStr(InsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Mode", vValue:=CStr(Task), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="TransactionType", vValue:=m_sTransactionType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetDetailsSQL, sSQLName:=ACGetDetailsName, bStoredProcedure:=ACGetDetailsStored, lNumberRecords:=lRecordsAffected, vResultArray:=v_vResultArray)

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

    Public Function GetTreatment_Values(ByRef vArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            vArray = Nothing

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACTreatment_LookupSQL, sSQLName:=ACTreatment_LookupName, bStoredProcedure:=ACTreatment_LookupStored, vResultArray:=vArray)

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
    ' Class Name : Business
    '
    ' Name: UpdateTreatment (Public)
    '
    ' Description: Updates the Claim Table records with the corresponding
    '              Treatment Values
    '
    ' Author : Ranjit
    '
    ' Date : 08 June 2000
    ' ***************************************************************** '

    Public Function UpdateTreatment(ByVal v_lClaimID As Integer, ByVal v_sDescription As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Parameters Array
            m_oDatabase.Parameters.Clear()

            ' Add the Paramters to the Input
            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimID", vValue:=CStr(v_lClaimID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Paramters to the Input
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Description", vValue:=v_sDescription, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateTreatmentSQL, sSQLName:=ACUpdateTreatmentName, bStoredProcedure:=ACUpdateTreatmentStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateTreatment Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateTreatment", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    Public Function GetTreatmentValue(ByVal v_lClaimID As Object, ByRef vArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ' Add the Paramters to the Input

            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimID", vValue:=CStr(v_lClaimID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            vArray = Nothing

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetTreatmentSQL, sSQLName:=ACGetTreatmentName, bStoredProcedure:=ACGetTreatmentStored, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTreatmentValue Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTreatmentValue", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Class Name : Business
    '
    ' Name: GetClaimNumber (Public)
    '
    ' Description: Gets the ClaimNumber from the given ClaimID
    '
    ' Author: Ranjit
    '
    ' Date : 08 June 2000
    ' ***************************************************************** '
    Public Function GetClaimNumber(ByRef r_vArray(,) As Object) As Integer

        '***Wirte the code for passing the parameters and collecting the resultset
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ' Set m_oDatabase = New dPMDAO.Database
            ' Clear the Parameters Array
            m_oDatabase.Parameters.Clear()

            ' Add the Paramters to the Input
            '       m_lReturn& = AddInputParam()
            '
            '       If m_lReturn <> PMTrue Then
            '            GetClaimNumber = PMFalse
            '            Exit Function
            '       End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimID", vValue:=CStr(ClaimID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetClaimNumberSQl, sSQLName:=ACGetClaimNumberName, bStoredProcedure:=ACGetClaimNumberStored, vResultArray:=r_vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimNumber Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimNumber", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetBusinessType
    '
    ' Description:
    '
    ' History: 03/03/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function GetBusinessType(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vArray(,) As Object) As Integer
        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT business_type_id" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "FROM insurance_file" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "WHERE insurance_file_cnt = " & CStr(v_lInsuranceFileCnt)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetBusinessType", bStoredProcedure:=False, vResultArray:=r_vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not (Information.IsArray(r_vArray)) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBusinessType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusinessType", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class
