Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Text
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Form_NET.Form")> _
Public NotInheritable Class Form
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Form
    '
    ' Date: 3/12/97
    '
    ' Description: Creatable Form class which contains all the
    '              methods, Form rules required to manipulate
    '              a DocMan.
    '
    ' Edit History:
    '
    ' DN270802 - Change embedded SQL to reflect table changes
    '
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Form"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)
    Private m_sSQL As String = ""
    Dim asa As String = ""


    ' Database Class (Private)
#If PD_EARLYBOUND = 1 Then

	Private m_oDatabase As dPMDAO.Database
#Else
    Private m_oDatabase As dPMDAO.Database
#End If

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Effective
    Private m_dtEffectiveDate As Date

    ' Primary Keys to work with
    ' Source ID
    Private m_iSourceID As Integer

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)


    Public ReadOnly Property EffectiveDate() As Date
        Get

            Return m_dtEffectiveDate

        End Get
    End Property
    Public Property SourceID() As Integer
        Get

            Return m_iSourceID

        End Get
        Set(ByVal Value As Integer)

            m_iSourceID = Value

        End Set
    End Property
    ' ***************************************************************** '
    ' Standard Product Family Constant (Read Only)
    ' ***************************************************************** '

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            '
            Return gPMConstants.PMEProductFamily.pmePFDocumaster

        End Get
    End Property
    '***VarDataEnd***

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

            ' Initialisation Code.

            ' Set Username and Password
            g_sUsername = sUsername
            g_sPassword.Value = sPassword

            ' Set UserID
            g_iUserID = iUserID

            ' Set Calling Application
            g_sCallingAppName = sCallingAppName

            ' Set Language ID
            g_iLanguageID = iLanguageID

            ' Set Source ID
            g_iSourceID = iSourceID

            ' Set Currency ID
            g_iCurrencyID = iCurrencyID

            ' Set Log Level
            g_iLogLevel = iLogLevel

            ' Have we a valid Database Object Reference?

            If (Not Information.IsNothing(vDatabase)) And (Information.IsReference(vDatabase)) Then
                ' Yes, so use it.
                m_oDatabase = vDatabase

                ' Do NOT Close Database in Terminate() method
                m_bCloseDatabase = False
            Else
                ' NO, Create new instance of the database object
#If PD_EARLYBOUND = 1 Then

				Set m_oDatabase = New dPMDAO.Database
#Else
                m_oDatabase = New dPMDAO.Database()
#End If

                ' Open the Database
                m_lReturn = CType(NewDatabase(v_sUsername:=g_sUsername, v_iSourceID:=g_iSourceID, v_iLanguageID:=g_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFDocumaster, r_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Close Database in Terminate() method
                m_bCloseDatabase = True
            End If


            ' Set the Effective Date to NOW
            m_dtEffectiveDate = DateTime.Now

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
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
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
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
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
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
        ' Error.
        '
        ' Log Error Message
        'LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************************
    ' Function:   GetAdminLevel
    '
    ' Description: Gets admin level for users
    '
    ' ***************************************************************************

    Public Function GetAdminLevel(ByRef iAdminLevel As Integer) As Integer

        Dim result As Integer = 0
        Dim oMisc As bDOCUserAdmin.Miscellaneous

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            oMisc = New bDOCUserAdmin.Miscellaneous()

            ' Initialise the miscellaneous class
            m_lReturn = oMisc.Initialise(v_sUsername:=g_sUsername, v_sPassword:=g_sPassword.Value, v_iUserID:=g_iUserID, v_iSourceID:=g_iSourceID, v_iLanguageID:=g_iLanguageID, v_iCurrencyID:=g_iCurrencyID, v_iLogLevel:=g_iLogLevel, v_sCallingAppName:=g_sCallingAppName, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the admin level
            m_lReturn = CType(oMisc.GetAdminLevel(iAdminLevel:=iAdminLevel), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Terminate the class
            'm_lReturn = CType(oMisc.Terminate(), gPMConstants.PMEReturnCode)
            oMisc.Dispose()
            Return result

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get admin level.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAdminLevel", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************************
    ' Function:   SetAdminLevel
    '
    ' Description:  sets admin level for users
    '
    ' ***************************************************************************
    Public Function SetAdminLevel(ByRef iAdminLevel As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            ' construct the sql statement
            sSQL = "UPDATE DOC_system SET admin_level = " & iAdminLevel

            ' hit the db
            m_lReturn = CType(m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="SETADMINLEVEL", bStoredProcedure:=False), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the admin level.", vApp:=ACApp, vClass:=ACClass, vMethod:="SetAdminLevel", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************************
    ' Function:         GetUserNames
    '
    ' Description:      Gets a list of users and their access levels
    '
    ' Edit History :
    ' RAM20021204  : 1. NRMA Project Changes. Sirius Process No. 189
    '                2. Modified the SQL to have a left join with PMUser Table
    ' ***************************************************************************
    Public Function GetUserNames(ByRef vUserNames As Object) As Integer

        Dim result As Integer = 0
        Dim vNames(,) As Object
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' construct the sql statement
            'sSQL = "SELECT * FROM DOC_doc_user"

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'RAM20021204 : Modified above SQL to have a Left Join with PMUser Table
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '    sSQL = ""
            sSQL = sSQL & "SELECT * "
            sSQL = sSQL & "FROM PMUser LEFT JOIN DOC_doc_user ON "
            sSQL = sSQL & "     PMUser.user_id = DOC_doc_user.user_id"
            '
            '    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            sSQL = "SELECT " & _
                   "pmuser.user_id , party_cnt, language_id, username, password, password_change_date, date_created, lastlogin, is_pmb_link_required, logged_on_at_client, server_printer, is_printer_changeable, is_deleted, effective_date, TimeStamp, email_address, initials, full_name, signature_file, Title, telephone_number, mobile_number, extension_number, fax_number, job_title_id, claim_handler_id, party_handler_id, alternative_identifier, PMUser.user_id, access_level, user_name, home_folder_num, retired " & _
                   "FROM PMUser LEFT JOIN DOC_doc_user ON PMUser.user_id = DOC_doc_user.user_id"

            ' hit the db
            m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="SELECTUSERNAMES", bStoredProcedure:=False, lNumberRecords:=-1, vResultArray:=vNames), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            vUserNames = vNames

            Return result

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the user names.", vApp:=ACApp, vClass:=ACClass, vMethod:="SetAdminLevel", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************************
    ' Function:     RemoveUser
    '
    ' Description:  Removes user from user list
    '
    ' ***************************************************************************
    Public Function RemoveUser(ByRef iUser As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' construct the sql statement
            sSQL = "UPDATE DOC_doc_user SET retired = 'Y' WHERE user_id = " & iUser

            ' hit the db
            m_lReturn = CType(m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="REMOVEUSER", bStoredProcedure:=False), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the user to retired.", vApp:=ACApp, vClass:=ACClass, vMethod:="RemoveUser", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************************
    ' Function:         UpdateUsers
    '
    ' Description:      Updates/sets users access levels that have been changed
    '
    ' RAM20021204  : 1. NRMA Project Changes. Sirius Process No. 189
    '                2. Modified the SQL add an entry to DOC_doc_user table if
    '                   the supplied user_id is not available
    ' ***************************************************************************
    Public Function UpdateUsers(ByRef vAccessLevels(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As New StringBuilder

        result = gPMConstants.PMEReturnCode.PMTrue

        ' loop for each user
        For iLoop1 As Integer = 1 To vAccessLevels.GetUpperBound(0)

            ' construct the sql statement
            'sSQL = "UPDATE DOC_doc_user SET access_level = " & CStr(vAccessLevels(iLoop1, 2)) & _
            '" WHERE user_id = " & CStr(vAccessLevels(iLoop1, 1))

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20021204  : 1. NRMA Project Changes. Sirius Process No. 189
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            sSQL = New StringBuilder("")

            sSQL.Append("IF exists (select null from doc_doc_user where user_id = " & CStr(vAccessLevels(iLoop1, 1)) & ") " & Strings.Chr(13) & Strings.Chr(10))


            sSQL.Append("     UPDATE DOC_doc_user SET access_level = " & CStr(vAccessLevels(iLoop1, 2)) & " WHERE user_id = " & CStr(vAccessLevels(iLoop1, 1)) & " " & Strings.Chr(13) & Strings.Chr(10))
            sSQL.Append("ELSE " & Strings.Chr(13) & Strings.Chr(10))
            'sSQL = sSQL & "     INSERT INTO DOC_doc_user values (" & CStr(vAccessLevels(iLoop1, 1)) & ", " & CStr(vAccessLevels(iLoop1, 2)) & ", 0, 'N') "



            sSQL.Append("     INSERT INTO DOC_doc_user SELECT " & CStr(vAccessLevels(iLoop1, 1)) & ", " & CStr(vAccessLevels(iLoop1, 2)) & ", username, 0, 'N' FROM pmuser WHERE user_id = " & CStr(vAccessLevels(iLoop1, 1)))
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            ' hit the db
            m_lReturn = CType(m_oDatabase.SQLAction(sSQL:=sSQL.ToString(), sSQLName:="UPDATEUSERS", bStoredProcedure:=False), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Next iLoop1

        Return result



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Log Error Message
        iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update the user access levels.", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateUsers", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result
    End Function
End Class
