Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Text
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("FormAdmin_NET.FormAdmin")> _
Public NotInheritable Class FormAdmin
    Implements IDisposable
    
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 6th January 1999
    '
    ' Description: Creatable Business class which contains all the
    '              methods, business rules required to administer
    '              the PMMessage table.
    '
    ' Edit History:
    ' DAK
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


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "FormAdmin"

    ' RDC 29072002 database no longer used to record messages.
    ' all messages now go to the event log

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer (Private)
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lError As Integer

    ' Return value
    Private m_lReturn As Integer

    ' CTAF 021299
    Private m_lMaxRecords As Integer

    ' PUBLIC Data Members (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusArchitecture
        End Get
    End Property

    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)
    ' PRIVATE Data Members (End)

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
    Public Function Initialise(ByVal sUserName As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long 

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            ' Set Username and Password
            m_sUsername = sUserName
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel

            ' Have we a valid Database Object Reference?

            If (Not Informations.IsNothing(vDatabase)) And (Informations.IsReference(vDatabase)) Then
                ' Yes, so use it.
                m_oDatabase = vDatabase

                ' Do NOT Close Database in Terminate() method
                m_bCloseDatabase = False
            Else
                ' NO, Create new instance of the database object
                m_oDatabase = New dPMDAO.Database()

                ' RFC250398 - Changed to use Sirius Architecture DSN
                m_lError = m_oDatabase.OpenDatabase(m_sUsername, m_iSourceID, m_iLanguageID, m_sCallingAppName, vDSN:=gPMConstants.PMSiriusArchitectureDSN)
                '        m_lError = NewDatabase(v_lPMProductFamily:=pmePFSiriusArchitecture, r_oDatabase:=m_oDatabase)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Close Database in Terminate() method
                m_bCloseDatabase = True
            End If

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Initialisation Code.

            ' Set Username and Password

            ' Set UserID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set LogLevel

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("iUserID", iUserID)
            oDict.Add("iSourceID", iSourceID)
            oDict.Add("iLanguageID", iLanguageID)
            oDict.Add("iCurrencyID", iCurrencyID)
            gPMFunctions.LogMessageToFile(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep, oDicParms:=oDict)

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
    ' Name: SelectByQuery
    '
    ' Description: Returns all or some of the messages depending on the
    '              message type
    '
    ' CF 180899 - Added ORDER BY line to query.
    '
    ' ***************************************************************** '
    Public Function SelectByQuery(ByRef r_vMessages As Object, ByVal v_lMessageType As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Construct the SQL
            'DAK081299
            sSQL = "SELECT username, " & _
                   "message_type, " & _
                   "text, " & _
                   "err_number, " & _
                   "err_description, " & _
                   "calling_app_name, " & _
                   "app_name, " & _
                   "class_name, " & _
                   "method_name, " & _
                   "log_date, " & _
                   "message_id " & _
                   "FROM pmmessage "

            ' Do we want particular messages?
            If v_lMessageType <> 0 Then
                sSQL = sSQL & "WHERE message_type = " & CStr(v_lMessageType) & " "
            End If

            sSQL = sSQL & "ORDER BY log_date DESC"

            ' Perform the SQL statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="SelectMessages", bStoredProcedure:=False, vResultArray:=vResultArray, lNumberRecords:=m_lMaxRecords)

            ' Validate the results
            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' They're ok. So return them


            r_vMessages = vResultArray

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectByQueryFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectByQuery", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteByType
    '
    ' Description: Deletes all messages of the passed type
    '
    ' ***************************************************************** '
    Public Function DeleteByType(ByVal v_lMessageType As Byte) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Construct the SQL
            sSQL = "DELETE FROM pmmessage"

            'm_oDatabase.SQLBeginTrans

            ' If we have a normal message type (ie, not all) then add that too
            If v_lMessageType <> 0 Then
                sSQL = sSQL & " WHERE message_type = " & CStr(v_lMessageType)
            End If

            ' Perform the SQL
            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="DeleteByType", bStoredProcedure:=False)

            'm_oDatabase.SQLRollbackTrans

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteByTypeFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteByType", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SelectMessages
    '
    ' Description: This was created to keep compatiblity.
    '
    ' History: 02/12/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function SelectMessages(ByRef r_vMessages As Object, ByVal v_lMessageType As Integer, ByRef v_lNumberOfRecords As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lMaxRecords = v_lNumberOfRecords


            Return SelectByQuery(r_vMessages:=r_vMessages, v_lMessageType:=v_lMessageType)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectMessages Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectMessages", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: DeleteMessages
    '
    ' Description: Deletes displayed messages only
    '
    ' History: 08/12/1999 DAK - Created.
    '
    ' ***************************************************************** '
    Public Function DeleteMessages(ByVal v_vMessageArray(,) As Object) As Integer
        Dim result As Integer = 0
        Dim sSQL As New StringBuilder


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Informations.IsArray(v_vMessageArray) Then
                Return result
            End If

            For iLoop1 As Integer = v_vMessageArray.GetLowerBound(1) To v_vMessageArray.GetUpperBound(1)

                sSQL = New StringBuilder("DELETE FROM pmmessage WHERE message_id = ")

                sSQL.Append(CStr(v_vMessageArray(10, iLoop1)))

                ' Perform the SQL
                m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL.ToString(), sSQLName:="DeleteMessages", bStoredProcedure:=False)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Next iLoop1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteMessages Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteMessages", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class
