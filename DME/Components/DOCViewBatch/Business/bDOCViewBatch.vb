Option Strict Off
Option Explicit On
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Form_NET.Form")>
Public NotInheritable Class Form
    Implements IDisposable
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

    ' Database Class (Private)
#If PD_EARLYBOUND = 1 Then

	Private m_oDatabase As dPMDAO.Database
#Else
    Private m_oDatabase As dPMDAO.Database
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

    ' Private Boolean ;)
    Private bIsStandAlone As Boolean

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFDocumaster

        End Get
    End Property

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
    Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef bStandAlone As Boolean = False, Optional ByRef vDatabase As dPMDAO.Database = Nothing) As Integer

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

            ' if its missing, then its been created with object manager
            ' so its not a stand alone program
            '    If (IsMissing(bStandAlone) = True) Then
            '        bStandAlone = False
            '    End If


            ' Have we a valid Database Object Reference?

            If (Not Informations.IsNothing(vDatabase)) And (Informations.IsReference(vDatabase)) Then
                ' Yes, so use it.
                m_oDatabase = vDatabase

                '        ' Do NOT Close Database in Terminate() method
                m_bCloseDatabase = False
            Else
                ' NO, Create new instance of the database object
#If PD_EARLYBOUND = 1 Then

				Set m_oDatabase = New dPMDAO.Database
#Else
                m_oDatabase = New dPMDAO.Database()
#End If

                ' Open the Database
                m_lReturn = CType(dPMDAO.NewDatabase(v_sUsername:=g_sUsername, v_iSourceID:=g_iSourceID, v_iLanguageID:=g_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFDocumasterScan, r_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Close Database in Terminate() method
                m_bCloseDatabase = True
            End If


            ' Set the Effective Date to NOW
            m_dtEffectiveDate = DateTime.Now

            If Not False Then
                If Not bStandAlone Then

#If PD_EARLYBOUND = 1 Then

					Set m_oMainDatabase = New dPMDAO.Database
#Else
                    m_oMainDatabase = New dPMDAO.Database()
#End If

                    ' Open the main database on the server
                    m_lReturn = CType(dPMDAO.NewDatabase(v_sUsername:=g_sUsername, v_iSourceID:=g_iSourceID, v_iLanguageID:=g_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFDocumaster, r_oDatabase:=m_oMainDatabase), gPMConstants.PMEReturnCode)

                    ' Close the Database in the Terminate() method
                    m_bCloseMainDatabase = True


                Else

                    ' Do not close the Database in the Terminate() method
                    m_bCloseMainDatabase = False

                End If

                bIsStandAlone = bStandAlone

            End If

            If m_oMisc Is Nothing Then
                m_oMisc = New bDOCViewBatch.Miscellaneous()

                m_lReturn = m_oMisc.Initialise(v_sUsername:=g_sUsername, v_sPassword:=g_sPassword.Value, v_iUserID:=g_iUserID, v_iSourceID:=g_iSourceID, v_iLanguageID:=g_iLanguageID, v_iCurrencyID:=g_iCurrencyID, v_iLogLevel:=g_iLogLevel, v_sCallingAppName:=g_sCallingAppName, vDatabase:=m_oMainDatabase, vScanDatabase:=m_oDatabase)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogMessageScan(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, bStandAlone:=bIsStandAlone)

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
                End If
                m_oMisc = Nothing
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
                If m_bCloseMainDatabase Then
                    m_oMainDatabase.CloseDatabase()
                    m_oMainDatabase = Nothing
                    m_bCloseMainDatabase = False
                End If
                m_oMainDatabase = Nothing
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
    Private Function BeginTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLBeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogMessageScan(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, bStandAlone:=bIsStandAlone)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    Private Function CommitTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLCommitTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogMessageScan(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, bStandAlone:=bIsStandAlone)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    Private Function RollbackTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLRollbackTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogMessageScan(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, bStandAlone:=bIsStandAlone)

            Return result

        End Try
    End Function
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
        'LogMessageScan(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, bStandAlone:=bIsStandAlone)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    Public Function GetParentTree(ByRef lFoldNum As Integer, ByRef vFolderTree As Object) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' TODO is this right? Folder or Parent Tree

            m_lReturn = CType(m_oMisc.GetFolderTree(lFolderNum:=lFoldNum, vFolderArray:=vFolderTree), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            LogMessageScan(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetParentTree", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, bStandAlone:=bIsStandAlone)

            Return result

        End Try
    End Function

    Public Function GetNextDocument(ByRef lDocNum As Integer) As Integer

        Dim result As Integer = 0
        Try

            Dim vResultArray(,) As Object = Nothing

            result = gPMConstants.PMEReturnCode.PMTrue


            ' Perform select statement on SQL database
            m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:="SELECT MAX (doc_num) FROM DOC_document", sSQLName:="store_document_number", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Grab the result of the biggest document number

            If CStr(vResultArray(0, 0)) <> "" Then

                lDocNum = CInt(vResultArray(0, 0))
            Else
                lDocNum = 0
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.
            LogMessageScan(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get next document from database.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNextDocument", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, bStandAlone:=bIsStandAlone)

            Return result

        End Try
    End Function

    Public Function GetMaxPages(ByRef lDocNum As Integer, ByRef lDocPages As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim sStatement As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sStatement = "SELECT MAX(page_num) FROM DOC_page WHERE doc_num=" & ((lDocNum).ToString).Trim()

            ' Perform select statement on SQL database
            m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=sStatement, sSQLName:="store_maxpages", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Grab the result of the biggest document number

            If CStr(vResultArray(0, 0)) <> "" Then

                lDocPages = CInt(vResultArray(0, 0))
            Else
                lDocPages = 0
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            LogMessageScan(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get number of pages for document.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMaxPages", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, bStandAlone:=bIsStandAlone)

            Return result

        End Try
    End Function

    Public Function GetDocumentNames(ByRef vDocNames(,) As Object) As Integer

        Dim result As Integer = 0
        Dim m_sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'construct SQL
            m_sSQL = "SELECT doc_name FROM DOC_document"

            'hit DB
            m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="GETDOCNAMES", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vDocNames), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            LogMessageScan(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get document names.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocumentNames", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, bStandAlone:=bIsStandAlone)

            Return result

        End Try
    End Function

    Public Function GetDocFolderNumber(ByRef lDocNum As Integer, ByRef lFolderNum As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim m_sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'construct SQL
            m_sSQL = "SELECT folder_num FROM DOC_document WHERE doc_num=" & lDocNum

            'hit DB
            m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="GETDOCNAMES", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                lFolderNum = 0
            Else

                lFolderNum = CInt(vResultArray(0, 0))
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            LogMessageScan(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get document folder number.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocFolderNumber", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, bStandAlone:=bIsStandAlone)

            Return result

        End Try
    End Function

    Public Function GetFolderTree(ByRef lFolderNum As Integer, ByRef vFolderArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = CType(m_oMisc.GetFolderTree(lFolderNum:=lFolderNum, vFolderArray:=vFolderArray), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            LogMessageScan(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get folder tree.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFolderTree", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, bStandAlone:=bIsStandAlone)

            Return result

        End Try
    End Function

    Public Function DeleteDocument(ByRef lDocNum As Integer, ByRef sScanDirectory As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Now delete the files
            m_lReturn = CType(m_oMisc.DeleteScannedPageFiles(lScanDocNum:=lDocNum, sScanDirectory:=sScanDirectory), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                LogMessageScan(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete scanned files.", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, bStandAlone:=bIsStandAlone)
                Return result
            End If

            ' Start the transaction
            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                LogMessageScan(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to begin database transaction.", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, bStandAlone:=bIsStandAlone)
                Return result
            End If

            ' Delete the document from the database
            m_lReturn = CType(m_oMisc.DeleteScannedDoc(lDocNum), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Commit the transaction
            m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            LogMessageScan(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete the document.", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteDocument", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, bStandAlone:=bIsStandAlone)

            Return result

        End Try
    End Function

    Public Function DoesDocumentExist(ByRef lDocNum As Integer, ByRef bExists As Boolean) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            ' Select something from the document, if its there, then it exists

            'construct SQL
            m_sSQL = "SELECT folder_num FROM DOC_document WHERE doc_num=" & lDocNum

            'hit DB
            m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="GETDOCNAMES", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            bExists = (Informations.IsArray(vResultArray))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            LogMessageScan(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete the document.", vApp:=ACApp, vClass:=ACClass, vMethod:="DoesDocumentExist", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, bStandAlone:=bIsStandAlone)

            Return result

        End Try
    End Function

    ' **********************************************************************************
    '
    ' Function OpenCloseDatabase
    '
    ' Purpose: This function exists to hot-fix an apparent bug in the jet engine
    '          where data is not refreshed and data that has been deleted is returned
    '          in SELECT statements.
    '
    '   Perhaps theres a bug with caching within the Jet engine?
    '
    ' **********************************************************************************
    Public Function OpenCloseDatabase() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Close it
            m_lReturn = CType(m_oDatabase.CloseDatabase(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            ' Open the Database
            m_lReturn = CType(dPMDAO.NewDatabase(v_sUsername:=g_sUsername, v_iSourceID:=g_iSourceID, v_iLanguageID:=g_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFDocumasterScan, r_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            m_oMisc.ScanDatabase = m_oDatabase

            Return result

        Catch excep As System.Exception

            ' Error


            result = gPMConstants.PMEReturnCode.PMError

            LogMessageScan(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete the document.", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteDocument", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, bStandAlone:=bIsStandAlone)

            Return result

        End Try
    End Function
End Class

