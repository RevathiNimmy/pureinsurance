Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Data.SqlClient
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("CLMRTInfoChkLst_NET.CLMRTInfoChkLst")> _
Public NotInheritable Class CLMRTInfoChkLst
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name:   CLMRTInfoChkLst
    ' Description:  Describes the CLMRTInfoChkLst attributes.
    ' Author:       SK
    ' Date:         06/07/2000
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
    Private Const ACClass As String = "CLMRTInfoChkLst"

    ' PRIVATE Data Members (Begin)

    ' Database Class
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag
    Private m_bCloseDatabase As Boolean

    Private m_oBackOffice As Object

    ' DataBase Attributes
    Private m_lExp_ser_id As Integer
    Private m_lRisk_type_id As Integer
    Private m_lRisk_type_Exp_ser_id As Integer

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
    Public Property Risk_type_Exp_ser_id() As Integer
        Get

            Return m_lRisk_type_Exp_ser_id

        End Get
        Set(ByVal Value As Integer)

            m_lRisk_type_Exp_ser_id = Value

        End Set
    End Property
    Public Property Exp_ser_id() As Integer
        Get

            Return m_lExp_ser_id

        End Get
        Set(ByVal Value As Integer)

            m_lExp_ser_id = Value

        End Set
    End Property
    Public Property Risk_type_id() As Integer
        Get

            Return m_lRisk_type_id

        End Get
        Set(ByVal Value As Integer)

            m_lRisk_type_id = Value

        End Set
    End Property
    ' ***************************************************************** '
    ' Name:         Initialise (Standard Method)
    ' Description:  Entry point for any initialisation code for this
    '               object.
    ' Author:       SK
    ' Date:         06/07/2000
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


            ' Create Back Office Link
            '    Set m_oBackOffice = New bBackOfficeLink.bBOLink
            If m_oBackOffice Is Nothing Then

                m_oBackOffice = New bBackOfficeLink.bBOLink()

                '******Changed Here To Make It Comatible For Client Server Model ******
                '******Added By Pandu Date :20-10-2000 ********************************

                m_lReturn = m_oBackOffice.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
                '******End of Change Here To Make It Comatible For Client Server Model ******
                '******Added By Pandu Date :20-10-2000 ***

                If m_oBackOffice Is Nothing Then


                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the BackOffice Link Object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result
                End If
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
    ' Name:         Terminate (Standard Method)
    ' Description:  Entry point for any termination code for this
    '               object.
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
    ' Name:         Add (Public)
    ' Description:  Adds to the Database from the Base Details.
    ' Author:       SK
    ' Date:         06/07/2000
    ' ***************************************************************** '
    Public Function Add() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

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

        ''Dim lRecordsAffected As Long
        ''
        ''    On Error GoTo Err_Update
        ''
        ''    Update = PMTrue
        ''
        ''    With m_oDatabase
        ''
        ''        ' Clear the Database Parameters Collection
        ''        .Parameters.Clear
        ''
        ''        ' Add the required INPUT parameters
        ''        m_lReturn& = AddInputParam()
        ''
        ''        If (m_lReturn& <> PMTrue) Then
        ''            Update = PMFalse
        ''            Exit Function
        ''        End If
        ''
        ''        ' Add PrimaryKey as INPUT parameters
        ''''        m_lReturn& = AddKeyInputParam()
        ''
        ''        If (m_lReturn& <> PMTrue) Then
        ''            Update = PMFalse
        ''            Exit Function
        ''        End If
        ''
        ''        ' Execute SQL Statement
        ''        m_lReturn& = .SQLAction( _
        '''            sSQL:=ACUpdateSQL, _
        '''            sSQLName:=ACUpdateName, _
        '''            bStoredProcedure:=ACUpdateStored, _
        '''            lRecordsAffected:=lRecordsAffected&)
        ''
        ''        If (m_lReturn& <> PMTrue) Then
        ''            Update = PMFalse
        ''            Exit Function
        ''        End If
        ''
        ''        ' Check to see that the record was updated OK
        ''        If (lRecordsAffected& > 0) Then
        ''            ' Updated No action required
        ''        Else
        ''            Update = PMFalse
        ''            Exit Function
        ''        End If
        ''
        ''    End With
        ''
        Dim result As Integer = 0
        Return result




        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

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
                m_lReturn = .SQLAction(sSQL:=ACDelSQL, sSQLName:=ACDelName, bStoredProcedure:=ACDelStored, lRecordsAffected:=lRecordsAffected)

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Delete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Delete", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SelectSingle (Public)
    '
    ' Description: Selects the required CLMRTInfoChkLst
    '
    ' ***************************************************************** '
    Public Function SelectSingle(Optional ByRef vLockMode As Object = Nothing) As Integer

        ''Dim lRecordCount As Long
        ''
        ''    On Error GoTo Err_SelectSingle
        ''
        ''    SelectSingle = PMTrue
        ''
        ''    With m_oDatabase
        ''
        ''        ' Clear the Database Parameters Collection
        ''        .Parameters.Clear
        ''
        ''        ' Default to No Lock if not supplied or not numeric
        ''        If (IsMissing(vLockMode) = True) _
        '''        Or (IsNumeric(vLockMode) = False) Then
        ''            vLockMode = PMNoLock
        ''        End If
        ''
        ''        ' Add PrimaryKey as INPUT parameters
        ''        m_lReturn& = AddKeyInputParam()
        ''
        ''        If (m_lReturn& <> PMTrue) Then
        ''            SelectSingle = PMFalse
        ''            Exit Function
        ''        End If
        ''
        ''        ' Execute SQL Statement
        ''        m_lReturn& = .SQLSelect( _
        '''            sSQL:=ACSelectSingleSQL, _
        '''            sSQLName:=ACSelectSingleName, _
        '''            bStoredProcedure:=ACSelectSingleStored, _
        '''            bKeepNulls:=True)
        ''
        ''        If (m_lReturn& <> PMTrue) Then
        ''            SelectSingle = PMFalse
        ''            Exit Function
        ''        End If
        ''
        ''        ' How many records were selected
        ''        lRecordCount& = .Records.Count
        ''
        ''          ' Do we have any records ?
        ''        If (lRecordCount& = 1) Then
        ''            ' Selected, No action required
        ''        Else
        ''            SelectSingle = PMNotFound
        ''            Exit Function
        ''        End If
        ''
        ''        ' Set properties
        ''        m_lReturn& = SetPropertiesFromDB( _
        '''            oFields:=.Records.Item(1).Fields)
        ''
        ''        If (m_lReturn& <> PMTrue) Then
        ''            SelectSingle = PMFalse
        ''            Exit Function
        ''        End If
        ''
        ''    End With
        ''
        Dim result As Integer = 0
        Return result




        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectSingle Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectSingle", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SetPropertiesFromDB (Public)
    '
    ' Description: Sets the supplied CLMRTInfoChkLst properties from a database
    '              record.
    ' ***************************************************************** '
    Public Function SetPropertiesFromDB(ByRef oFields As ADODB.Fields) As Integer

        Dim result As Integer = 0
        Try

            ''    SetPropertiesFromDB = PMTrue
            ''
            ''    ' Populate Base Details
            ''
            ''    With oFields
            ''
            ''        Exp_ser_id = .Item("Recovery_id").Value
            ''        PerilID = .Item("Peril_id").Value
            ''
            ''        Risk_type_id = .Item("Reserve_id").Value
            ''
            ''        If (IsNull(.Item("Intial_reserve").Value) = True) Then
            ''            IntialReserve = Null
            ''        Else
            ''            IntialReserve = .Item("IntialReserve").Value
            ''        End If
            ''        If (IsNull(.Item("Currency_id").Value) = True) Then
            ''            CurrencyID = Null
            ''        Else
            ''            CurrencyID = .Item("Currency_id").Value
            ''        End If
            ''
            ''        If (IsNull(.Item("RecoveryType_ID").Value) = True) Then
            ''            RecoveryTypeID = Null
            ''        Else
            ''            RecoveryTypeID = .Item("RecoveryType_ID").Value
            ''        End If
            ''
            ''    End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetPropertiesFromDB Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetPropertiesFromDB", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            m_lReturn = .Parameters.Add(sName:="Risk_type_id", vValue:=CStr(Risk_type_id), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="Exp_Ser_Id", vValue:=CStr(Exp_ser_id), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="Mode", vValue:=CStr(1), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

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

            ''        m_lReturn& = .Parameters.Add( _
            '''              sName:="recovery_id", _
            '''              vValue:=Exp_ser_id, _
            '''              iDirection:=PMParamInput, _
            '''              iDataType:=PMLong)
            m_lReturn = .Parameters.Add(sName:="Exp_Ser_Id", vValue:=CStr(Exp_ser_id), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = .Parameters.Add(sName:="Rsk_Type_Id", vValue:=CStr(Risk_type_id), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        End With


        Return result

    End Function

    ' ***************************************************************** '
    ' Name:         AddKeyOutputParam (Private)
    ' Description:  Adds all of the PRIMARY KEY OUTPUT parameters
    '               required for an Add.
    ' Author:       SK
    ' Date:         06/07/2000
    ' ***************************************************************** '
    Private Function AddKeyOutputParam() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            m_lReturn = .Parameters.Add(sName:="Rsk_Type_Exp_Ser_Id", vValue:=CStr(0), idirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

        End With

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

        With m_oDatabase

            Risk_type_Exp_ser_id = .Parameters.Item("Rsk_Type_Exp_Ser_Id").Value

        End With

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

