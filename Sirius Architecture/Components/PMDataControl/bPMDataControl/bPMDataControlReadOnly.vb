Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("ReadOnly_Renamed_NET.ReadOnly_Renamed")> _
Public NotInheritable Class ReadOnly_Renamed
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: ReadOnly
    '
    ' Date: 10/03/1998
    '
    ' Description: Policy Master Data Control
    '
    ' Edit History:
    ' RFC 10/03/1998 Original version.
    ' ***************************************************************** '

    ' ************************************************
    ' Added to replace global variables 18/09/2003
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


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "ReadOnly"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Sirius Environment (Private)
    'Private m_oDatabase As dPMDAO.Database
    Private m_oDatabase As Object
    Private m_colConnections As bPMDataControl.Connections = New bPMDataControl.Connections()

    ' PRIVATE Data Members (End)

    Public ReadOnly Property CurrentDSN() As String
        Get

            If m_oDatabase Is Nothing Then
                Return ""
            Else
                Return m_oDatabase.CurrentDSN
            End If

        End Get
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
            ' Set Username and Password
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel


            ' Initialisation Code.

            ' Set Username and Password

            ' Set user ID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level

            m_oDatabase = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate
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
                m_colConnections = Nothing
                m_oDatabase.CloseDatabase()
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: OpenDatabase
    '
    ' Description: Open the Sirius Database
    '
    ' Parameters : DSN - Registered ODBC DataSource
    '              Database - Default Database to use once connected.
    '              Username - Recognised user name for the database
    '              Password - Password associated with the username
    ' ***************************************************************** '
    Public Function OpenDatabase(Optional ByVal v_vUsername As Object = Nothing, Optional ByVal v_vPassword As Object = Nothing, Optional ByVal v_vDSN As Object = Nothing, Optional ByVal v_vDatabase As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Have we already got it open

            lReturn = CType(SetCurrentDatabase(v_vDSN:=CStr(v_vDSN)), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                m_oDatabase = New dPMDAO.Database()

                ' RDC 27062002 use Comp Serv to open database
                '        lReturn = m_oDatabase.OpenDatabase( _
                'vUsername:=v_vUsername, _
                'vPassword:=v_vPassword, _
                'vdsn:=v_vDSN, _
                'vDatabase:=v_vDatabase)
                lReturn = CType(gPMComponentServices.NewDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If



                lReturn = CType(m_colConnections.Add(v_sDSN:=CStr(v_vDSN), v_sDatabase:=CStr(v_vDatabase), v_oPMDAO:=m_oDatabase), gPMConstants.PMEReturnCode)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to open database", vApp:="dPMDAO", vClass:="Database", vMethod:="OpenDatabase", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: CloseDatabase
    '
    ' Description: Close the Sirius Database
    '
    '
    ' ***************************************************************** '
    Public Function CloseDatabase() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oDatabase Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return m_oDatabase.CloseDatabase()

        Catch excep As System.Exception



            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Close Database Failed", vApp:="dPMDAO", vClass:="Database", vMethod:="CloseDatabase", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetCurrentDatabase
    '
    ' Description: Sets the current Database
    '
    ' Parameters : DSN - Registered ODBC DataSource
    '              Database - Default Database to use once connected.
    ' ***************************************************************** '
    Public Function SetCurrentDatabase(ByVal v_vDSN As String, Optional ByVal v_vDatabase As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oConnection As bPMDataControl.Connection

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Have we already got it open
            oConnection = m_colConnections.Item(v_vDSN)

            If oConnection Is Nothing Then
                ' Not already open, so return false
                result = gPMConstants.PMEReturnCode.PMFalse
            Else
                ' Set to be current database
                m_oDatabase = oConnection.PMDAO
            End If

            oConnection = Nothing

            Return result

        Catch excep As System.Exception



            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to open database", vApp:="dPMDAO", vClass:="Database", vMethod:="SetCurrentDatabase", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetData
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function GetData(ByVal v_sSQL As String, ByVal v_sSQLName As String, ByRef r_vFieldArray As Object, ByRef r_vResultArray As Object, Optional ByVal v_bStoredProcedure As Boolean = False, Optional ByVal v_lNumberRecords As Integer = 0, Optional ByVal v_bKeepNulls As Boolean = False) As Integer


        Dim result As Integer = 0
        Dim sDebugSQL As String = ""
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lNoOfFields, lNoOfRecords As Integer
        Dim oRecord As ADODB.Record

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oDatabase Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lReturn = m_oDatabase.SQLSelect(sSql:=v_sSQL, sSQLName:=v_sSQLName, bStoredProcedure:=v_bStoredProcedure, lNumberRecords:=v_lNumberRecords, bKeepNulls:=v_bKeepNulls)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the number of Records
            lNoOfRecords = m_oDatabase.Records.Count()

            ' Do we have any results
            If lNoOfRecords < 1 Then

                ' No, so return empty string

                r_vFieldArray = ""

                r_vResultArray = ""

            Else

                ' Get the number of Fields
                ' RDC 21062002 fields now zero-based
                lNoOfFields = m_oDatabase.Records.Item(1).Fields().Count - 1

                ' Populate the Field Array using the fields in record one.

                ' Dimension the Array
                ReDim r_vFieldArray(lNoOfFields)

                ' Loop round all fields in record one
                For lField As Integer = 0 To lNoOfFields

                    r_vFieldArray(lField) = m_oDatabase.Records.Item(1).Fields()(lField).Name.Trim()
                Next

                ' Populate the Result Array

                ' Redim the Result Array
                ReDim r_vResultArray(lNoOfFields, lNoOfRecords - 1)

                ' Populate the Result Array from the Records Collection
                For lRecord As Integer = 1 To lNoOfRecords
                    oRecord = m_oDatabase.Records.Item(lRecord)
                    For lField As Integer = 0 To lNoOfFields

                        r_vResultArray(lField, lRecord - 1) = oRecord.Fields(lField)
                    Next
                Next

            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError




            sDebugSQL = sDebugSQL.Substring(0, 50)

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SQL = " & sDebugSQL, vApp:="dPMDAO", vClass:="Database", vMethod:="GetData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddParameter
    '
    ' Description: Adds a PMDAO parameter.
    '
    '
    ' ***************************************************************** '
    Public Function AddParameter(ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iDirection As Integer, ByVal v_iDatatype As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oDatabase Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            Return m_oDatabase.Parameters.Add(sName:=v_sName, vValue:=CStr(v_vValue), iDirection:=v_iDirection, iDatatype:=v_iDatatype)

        Catch excep As System.Exception



            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Close Database Failed", vApp:="dPMDAO", vClass:="Database", vMethod:="AddParameter", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ClearParameters
    '
    ' Description: Clears the Parameters Collection
    '
    '
    ' ***************************************************************** '
    Public Function ClearParameters() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oDatabase Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oDatabase.Parameters.Clear()

            Return result

        Catch excep As System.Exception



            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Close Database Failed", vApp:="dPMDAO", vClass:="Database", vMethod:="ClearParameters", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)
    ' PRIVATE Methods (End)
    Public Sub New()
        MyBase.New()
    End Sub

    Protected Overrides Sub Finalize()

        Dispose(False)

    End Sub
End Class
