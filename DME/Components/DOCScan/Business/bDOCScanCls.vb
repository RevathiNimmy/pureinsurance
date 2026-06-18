Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
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
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Form"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)
    Private m_sSQL As String = ""
    Dim asa As String = ""

    Private m_oMisc As bDOCScan.Miscellaneous

#If PD_EARLYBOUND = 1 Then

	Private m_oDatabase As dPMDAO.Database
	Private m_oMainDatabase As dPMDAO.Database
#Else
    Private m_oDatabase As dPMDAO.Database
    Private m_oMainDatabase As dPMDAO.Database
#End If

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Close Database Flag (Private)
    Private m_bCloseMainDatabase As Boolean

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Effective
    Private m_dtEffectiveDate As Date

    ' Primary Keys to work with
    ' Source ID
    Private m_iSourceID As Integer

    ' This is the name of the temporary file created during database optimisation
    Private Const DMETMPFILE1 As String = "C:\~DMETMP1~.OLD"
    Private Const DMETMPFILE2 As String = "C:\~DMETMP2~.OLD"

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

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            '
            Return gPMConstants.PMEProductFamily.pmePFDocumaster

        End Get
    End Property
    '

    ' *******************************************************************************
    '
    ' Function: PostInitialise
    '
    ' Desc: This must be called after the object is initialised using ObjectManager.
    '       We use this as we need to specify if running stand-alone or not, and
    '       create connections to the databases accordingly.
    '
    ' *******************************************************************************
    Public Function PostInitialise(ByRef bStandAlone As Boolean) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bIsStandAlone = bStandAlone

            ' Have we a valid Database Object Reference?
            '    If (IsMissing(vDatabase) = False) _
            ''    And (IsObject(vDatabase) = True) Then
            '        ' Yes, so use it.
            '        Set m_oDatabase = vDatabase
            '
            '        ' Do NOT Close Database in Terminate() method
            '        m_bCloseDatabase = False
            '    Else
            ' NO, Create new instance of the database object

#If PD_EARLYBOUND = 1 Then

			Set m_oDatabase = New dPMDAO.Database
#Else
            m_oDatabase = New dPMDAO.Database()
#End If

            ' Open the Database
            m_lReturn = CType(NewDatabase(v_sUsername:=g_sUsername, v_iSourceID:=g_iSourceID, v_iLanguageID:=g_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFDocumasterScan, r_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Close Database in Terminate() method
            m_bCloseDatabase = True

            '    End If

            If Not bStandAlone Then

#If PD_EARLYBOUND = 1 Then

				Set m_oMainDatabase = New dPMDAO.Database
#Else
                m_oMainDatabase = New dPMDAO.Database()
#End If

                ' Open the main database on the server
                m_lReturn = CType(NewDatabase(v_sUsername:=g_sUsername, v_iSourceID:=g_iSourceID, v_iLanguageID:=g_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFDocumaster, r_oDatabase:=m_oMainDatabase), gPMConstants.PMEReturnCode)

                ' Close the Database in the Terminate() method
                m_bCloseMainDatabase = True

            Else

                ' Do not close the Database in the Terminate() method
                m_bCloseMainDatabase = False

            End If

            ' Create an instance of the miscellaneous class
            m_oMisc = New bDOCScan.Miscellaneous()

            ' give it a database
            If Not bIsStandAlone Then
                m_lReturn = m_oMisc.Initialise(v_sUsername:=g_sUsername, v_sPassword:=g_sPassword.Value, v_iUserID:=g_iUserID, v_iSourceID:=g_iSourceID, v_iLanguageID:=g_iLanguageID, v_iCurrencyID:=g_iCurrencyID, v_iLogLevel:=g_iLogLevel, v_sCallingAppName:=g_sCallingAppName, vDatabase:=m_oMainDatabase, vScanDatabase:=m_oDatabase)
            Else
                m_lReturn = m_oMisc.Initialise(v_sUsername:=g_sUsername, v_sPassword:=g_sPassword.Value, v_iUserID:=g_iUserID, v_iSourceID:=g_iSourceID, v_iLanguageID:=g_iLanguageID, v_iCurrencyID:=g_iCurrencyID, v_iLogLevel:=g_iLogLevel, v_sCallingAppName:=g_sCallingAppName, vScanDatabase:=m_oDatabase)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="PostInitialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PostInitialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
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

            ' CODE WAS HERE !

            ' Set the Effective Date to NOW
            m_dtEffectiveDate = DateTime.Now

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            Me.disposedValue = True
            If disposing Then
                If m_oMisc IsNot Nothing Then
                    m_oMisc.Dispose()
                    m_oMisc = Nothing
                End If
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                    m_bCloseDatabase = False
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


    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '

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

    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' ***************************************************************** '

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    ' Name: Save Document
    '
    ' Description: Saves the passed information to the local
    '              scan station access database.
    '
    ' ND 191000    Added new optional parameters from customer to
    '              save new Work Manager task data to database to
    '              create scheduled task when document is committed
    ' ***************************************************************** '

    Public Function SaveDocument(ByRef iDocNum As Integer, ByRef sDocName As String, ByRef vPagesize() As Object, ByRef dExpiryDate As Date, ByRef dDocDate As Date, ByRef vKeywordID() As Object, ByRef vAnnotation() As Object, Optional ByRef sPageType As String = "TIF", Optional ByRef sDocType As String = "I", Optional ByRef sPassword As String = "", Optional ByRef iAccessLevel As Integer = 9, Optional ByRef lFolderNum As Integer = 0, Optional ByRef sScanUser As String = "DMSSCAN", Optional ByRef sCustomer As String = "", Optional ByRef dtTaskDueDate As Date = #12/30/1899#, Optional ByRef lPMUserGroup As Integer = 0, Optional ByRef lUserID As Integer = 0, Optional ByRef sDescription As String = "", Optional ByRef lTaskStatus As Integer = 0, Optional ByRef iUrgent As Integer = 0, Optional ByRef dtDateCreated As Date = #12/30/1899#, Optional ByRef lCreatedByID As Integer = 0) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim sParams, sStatement As String
        Dim sValues As New StringBuilder
        Dim lRecordsAffected As Integer

        Dim sTemp As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Code Starts

            ' Clear the database

            ' Build up document table data

            ' Validate the document name
            m_lReturn = CType(ValidateSQL(sDocName), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Validate the password
            m_lReturn = CType(ValidateSQL(sPassword), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            sParams = "( doc_num, doc_name, folder_num, doc_type, access_level, password, create_date )"
            sValues = New StringBuilder("( " & Conversion.Str(iDocNum) & _
                        ", '" & sDocName & _
                        "', " & Conversion.Str(lFolderNum) & _
                        ", '" & sDocType & _
                        "', " & Conversion.Str(iAccessLevel) & _
                        ", '" & sPassword & _
                      "', '" & DateTimeHelper.ToString(DateTime.Now) & "' )")

            sStatement = "INSERT INTO DOC_document " & sParams & " VALUES " & sValues.ToString()

            ' Execute SQL Statement

            m_lReturn = CType(m_oDatabase.SQLAction(sSQL:=sStatement, sSQLName:="savedocument_document", bStoredProcedure:=False, lRecordsAffected:=lRecordsAffected), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Only insert annotation if there is actually one.
            ' CTAF 20040326 - Start
            If Information.IsArray(vAnnotation) Then
                If vAnnotation.GetUpperBound(0) >= 1 Then

                    For iLoop1 As Integer = 1 To vAnnotation.GetUpperBound(0)


                        sTemp = CStr(vAnnotation(iLoop1))
                        m_lReturn = CType(ValidateSQL(sTemp), gPMConstants.PMEReturnCode)

                        vAnnotation(iLoop1) = sTemp

                        ' Build up annotation table data
                        sParams = "( doc_num, ann_text, user_name, create_date ) "
                        sValues = New StringBuilder("( " & Conversion.Str(iDocNum) & _
                                  ", '")

                        sValues.Append(CStr(vAnnotation(iLoop1)))

                        sValues.Append( _
                                        "', '" & sScanUser & _
                                       "', '" & DateTimeHelper.ToString(DateTime.Now) & " ' )")

                        sStatement = "INSERT INTO DOC_annotation " & sParams & " VALUES " & sValues.ToString()

                        ' Execute SQL Statement

                        m_lReturn = CType(m_oDatabase.SQLAction(sSQL:=sStatement, sSQLName:="savedocument_annotation", bStoredProcedure:=False, lRecordsAffected:=lRecordsAffected), gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                    Next iLoop1

                End If
            End If
            ' CTAF 20040326 - End

            ' Build up doc info table
            sParams = "( doc_num, expiry_date, scan_user, doc_date ) "
            sValues = New StringBuilder("( " & Conversion.Str(iDocNum) & _
                        ", '" & dExpiryDate.ToString("dd/MM/yyyy") & " " & DateTime.Now.ToString("HH:mm:ss") & _
                        "', '" & sScanUser & _
                      "', '" & dDocDate.ToString("dd/MM/yyyy") & " " & DateTime.Now.ToString("HH:mm:ss") & " ' )")

            sStatement = "INSERT INTO DOC_doc_info " & sParams & " VALUES " & sValues.ToString()

            ' Execute SQL Statement
            m_lReturn = CType(m_oDatabase.SQLAction(sSQL:=sStatement, sSQLName:="savedocument_docinfo", bStoredProcedure:=False, lRecordsAffected:=lRecordsAffected), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' insert the keywordid table, if theres any values
            If Information.IsArray(vKeywordID) Then
                If vKeywordID.GetUpperBound(0) > 0 Then

                    For iLoop1 As Integer = 0 To vKeywordID.GetUpperBound(0)

                        ' Build up doc keyword table
                        sParams = "( doc_num, keyword_id, user_name, create_date ) "
                        sValues = New StringBuilder("( " & Conversion.Str(iDocNum) & _
                                  ", '")


                        sValues.Append(CStr(vKeywordID(iLoop1)))

                        sValues.Append( _
                                        "', '" & sScanUser & _
                                       "', '" & DateTimeHelper.ToString(DateTime.Now) & " ' )")


                        sStatement = "INSERT INTO DOC_doc_keyword " & sParams & " VALUES " & sValues.ToString()

                        ' Execute SQL Statement
                        m_lReturn = CType(m_oDatabase.SQLAction(sSQL:=sStatement, sSQLName:="savedocument_dockeyword", bStoredProcedure:=False, lRecordsAffected:=lRecordsAffected), gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                    Next iLoop1

                End If
            End If

            For iLoop1 As Integer = 0 To vPagesize.GetUpperBound(0) - 1

                ' Build up page table
                sParams = "( doc_num, page_type, page_num, page_size, create_date ) "
                sValues = New StringBuilder("( '" & Conversion.Str(iDocNum) & _
                            "', '" & sPageType & _
                            "', " & Conversion.Str(iLoop1 + 1).Trim() & _
                            ", " & Conversion.Str(vPagesize(iLoop1)).Trim() & _
                          ", '" & DateTimeHelper.ToString(DateTime.Now) & " ' )")

                sStatement = "INSERT INTO DOC_page " & sParams & " VALUES " & sValues.ToString()

                ' Execute SQL Statement
                m_lReturn = CType(m_oDatabase.SQLAction(sSQL:=sStatement, sSQLName:="savedocument_page", bStoredProcedure:=False, lRecordsAffected:=lRecordsAffected), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Next iLoop1


            ' Add Task data to database
            ' Parameters to set
            sParams = "( Doc_Num, customer, task_due_date, PMUser_Group_ID," & _
                      "User_ID, Description, Task_Status, Urgent, Date_Created, Created_By_ID)"

            ' Values to set parameters to
            sValues = New StringBuilder("(" & iDocNum & _
                        " , '" & sCustomer & _
                        "' , '" & DateTimeHelper.ToString(dtTaskDueDate) & _
                        "' , " & CStr(lPMUserGroup) & _
                        " , " & CStr(lUserID) & _
                        " , '" & sDescription & _
                        "' , " & CStr(lTaskStatus) & _
                        " , " & CStr(iUrgent) & _
                        " , '" & DateTimeHelper.ToString(dtDateCreated) & _
                      "', " & CStr(lCreatedByID) & ")")

            ' SQL Insert statment
            sStatement = "INSERT INTO DOC_Task " & sParams & " VALUES " & sValues.ToString()

            ' Execute SQL Statement
            m_lReturn = CType(m_oDatabase.SQLAction(sSQL:=sStatement, sSQLName:="savedocument_task", bStoredProcedure:=False, lRecordsAffected:=lRecordsAffected), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Log Error Message
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("dExpiryDate", dExpiryDate)
            oDict.Add("dDocDate", dDocDate)
            oDict.Add("dtTaskDueDate", dtTaskDueDate)
            oDict.Add("lUserID", lUserID)
            oDict.Add("lCreatedByID", lCreatedByID)
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Save Document", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveDocument", excep:=excep, oDicParms:=oDict)


            Return gPMConstants.PMEReturnCode.PMError

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetNextDocNum (Public)
    '
    ' Description: Gets the next free document number from the database
    '
    ' ***************************************************************** '

    Public Function GetNextDocNum(ByRef iDocNum As Integer) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim vResultArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Perform select statement on SQL database
            m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:="SELECT MAX(doc_num) FROM DOC_document", sSQLName:="store_document_number", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Grab the result, and increment for next document number

            If CStr(vResultArray(0, 0)) <> "" Then

                iDocNum = CDbl(vResultArray(0, 0)) + 1
            Else
                iDocNum = 1
            End If

            Return result

        Catch excep As System.Exception



            Select Case Information.Err().Number
                Case 13
                    ' the data base is empty at the moment
                    result = gPMConstants.PMEReturnCode.PMTrue
                    iDocNum = 1

                Case Else

                    ' Log Error Message
                    LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to obtain the next document number", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNextDocNum", excep:=excep)

                    result = gPMConstants.PMEReturnCode.PMError

            End Select

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDocNames (Public)
    '
    ' Description: Gets the list of used document names from the server
    '
    ' ***************************************************************** '

    Public Function GetDocNames(ByRef vDocNames As Object) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim lReturn As gPMConstants.PMEReturnCode

            lReturn = CType(m_oMisc.GetDocNames(vDocNamesArray:=vDocNames), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Log Error Message
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve the document names", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocNames", excep:=excep)


            Return gPMConstants.PMEReturnCode.PMError

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDocDateOffsets (Public)
    '
    ' Description: Gets the list of used document names from the server
    '
    ' ***************************************************************** '

    Public Function GetDocDateOffSets(ByRef iDocDateOffset As Integer, ByRef iExpiryDateOffset As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(m_oMisc.GetDocDateOffSets(iDocDateOffset:=iDocDateOffset, iExpiryDateOffset:=iExpiryDateOffset), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' use a few defaults for now
                iDocDateOffset = 2
                iExpiryDateOffset = 365

                Return result
            End If

            Return result

        Catch excep As System.Exception



            ' Log Error Message
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to get the document date offset", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocDateOffsets", excep:=excep)


            Return gPMConstants.PMEReturnCode.PMError

        End Try
    End Function
End Class
