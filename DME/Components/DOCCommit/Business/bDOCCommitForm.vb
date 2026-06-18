Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
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
    ' DN270802 - Change embedded SQL to reflect table name changes
    '
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Form"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)
    Private m_sSQL As String = ""

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lReturn As Integer

    ' Business objects for writing to main database

    Private m_oFolder As bDOCFolder.Form

    Private m_oDocument As bDOCDocument.Form

    Private m_oPage As bDOCpage.Form

    Private m_oDocInfo As bDOCDocInfo.Form

    Private m_oAnnotation As bDOCAnnotation.Form

    Private m_oDocKeyword As bDOCDocKeyword.Form
    Private frmTimer As frmTimer
    ' Zipper
#If PD_EARLYBOUND = 1 Then

	 'Private m_oZipper As bSIRZipper.Zipper
    Private m_oZipper As bPMZipper.Business

#Else
    'Private m_oZipper As bSIRZipper.Zipper
    Private m_oZipper As bPMZipper.Business
#End If

    ' Miscellaneous class
    Private m_oMisc As bDOCCommit.Miscellaneous

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    '***VarDataEnd***

    ' PUBLIC Property Procedures (End)

    ' ***************************************************************** '
    ' Standard Product Family Constant (Read Only)
    ' ***************************************************************** '
    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFDocumaster

        End Get
    End Property
    ' ***************************************************************** '
    ' Store the total number of docs so form can be updated
    ' ***************************************************************** '
    Public ReadOnly Property DocsTotal() As Integer
        Get

            Return g_lDocsTotal

        End Get
    End Property
    ' ***************************************************************** '
    ' Store the number of failed docs so form can be updated
    ' ***************************************************************** '
    Public ReadOnly Property DocsFailed() As Integer
        Get

            Return g_lDocsFailed

        End Get
    End Property

    ' ***************************************************************** '
    ' Store the number of docs commited so form can be updated
    ' ***************************************************************** '
    Public ReadOnly Property DocsDone() As Integer
        Get

            Return g_lDocsDone

        End Get
    End Property

    ' ***************************************************************** '
    ' Current status of commit process
    ' ***************************************************************** '
    ' ***************************************************************** '
    ' Current status of commit process
    ' ***************************************************************** '
    Public Property RunStatus() As Integer
        Get

            Return g_iRunStatus

        End Get
        Set(ByVal Value As Integer)

            g_iRunStatus = Value

        End Set
    End Property

    ' ***************************************************************** '
    ' Current status of commit process
    ' ***************************************************************** '
    ' ***************************************************************** '
    ' Current status of commit process
    ' ***************************************************************** '
    Public Property TaskCnt() As Byte
        Get

            Return g_vTaskCnt

        End Get
        Set(ByVal Value As Byte)


            g_vTaskCnt = CByte(Value)

        End Set
    End Property


    Public Property BatchAnnotation() As String
        Get

            Return g_sBatchAnnotation

        End Get
        Set(ByVal Value As String)

            g_sBatchAnnotation = Value

        End Set
    End Property


    Public Property ScanDirectory() As String
        Get

            Return g_sScanDirectory

        End Get
        Set(ByVal Value As String)

            g_sScanDirectory = Value

        End Set
    End Property

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

            'open the scan database
#If PD_EARLYBOUND = 1 Then

			Set g_oScanDB = New dPMDAO.Database
#Else
            g_oScanDB = New dPMDAO.Database()
#End If

            ' Open the Database
            m_lReturn = NewDatabase(v_sUsername:=g_sUsername, v_iSourceID:=g_iSourceID, v_iLanguageID:=g_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFDocumasterScan, r_oDatabase:=g_oScanDB)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'open the main database
#If PD_EARLYBOUND = 1 Then

			Set g_oMainDB = New dPMDAO.Database
#Else
            g_oMainDB = New dPMDAO.Database()
#End If

            ' Open the Database
            m_lReturn = NewDatabase(v_sUsername:=g_sUsername, v_iSourceID:=g_iSourceID, v_iLanguageID:=g_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFDocumaster, r_oDatabase:=g_oMainDB)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create an instance of the object manager.
#If PD_EARLYBOUND = 1 Then

			Set g_oObjectManager = New bObjectManager.ObjectManager
#Else
            g_oObjectManager = New bObjectManager.ObjectManager()
#End If

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Set the object manager to nothing.
                g_oObjectManager = Nothing
                Return result
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
            End With

            'Get the commit business object that does server side stuff
            'via the public object manager.
            Dim temp_g_oCommitServer As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oCommitServer, "bDOCCommitServer.Commit", vInstanceManager:="ClientManager")
            g_oCommitServer = temp_g_oCommitServer
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Set the object manager to nothing.
                g_oObjectManager = Nothing
                Return result
            End If

            'load the form with the timer
            'Dim tempLoadForm As frmTimer = frmTimer
            frmTimer = New frmTimer

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
                g_oCommitServer = Nothing

                frmTimer.Close()
            End If
        End If
        Me.disposedValue = True
    End Sub



    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    ' We must do the same on the main database too, in the
    ' commitserver object
    '
    ' ***************************************************************** '
    Private Function BeginTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = g_oScanDB.SQLBeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' begin in commitserver object too

            m_lReturn = g_oCommitServer.BeginTrans

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    ' We must do the same on the main database too, in the
    ' commitserver object
    '
    ' ***************************************************************** '
    Private Function CommitTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = g_oScanDB.SQLCommitTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' begin in commitserver object too

            m_lReturn = g_oCommitServer.CommitTrans

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: MovePageFiles (Private)
    '
    ' Description: This loops thru each each page for a scanned document
    ' and moves it to the server. It does this by first moving to a temp
    ' area, then renaming it.
    '
    ' ***************************************************************** '
    Private Function MovePageFiles(ByRef lScanDocNum As Integer, ByRef vPageList(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sFileFrom, sFileTo, sDataPath As String




        result = gPMConstants.PMEReturnCode.PMTrue


        'if we have no pages, then some problem
        If Not Information.IsArray(vPageList) Then

            result = gPMConstants.PMEReturnCode.PMFalse

            iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="No physical page files to move", vApp:=ACApp, vClass:=ACClass, vMethod:="MovePageFiles", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            Return result

        End If

        'get path to 00 tree

        m_lReturn = g_oCommitServer.GetDataPath(lVolumeID:=DOCHD1_ID, sDataPath:=sDataPath)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        'loop the each page file, copying it to the server temp area
        For i As Integer = vPageList.GetLowerBound(1) To vPageList.GetUpperBound(1)

            Application.DoEvents()

            'Construct source file name


            sFileFrom = g_sScanDirectory & "Doc" & _
                        lScanDocNum & "\" & _
                        CStr(vPageList(0, i)) & "." & _
                            CStr(vPageList(1, i))


            'Construct destination file name


            sFileTo = sDataPath & CStr(vPageList(2, i)) & "." & CStr(vPageList(1, i))


            ' Create path for file copy
            m_lReturn = MakePath(sFileTo)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            'now copy it across

            m_lReturn = CInt(m_oZipper.ZipFile(sFileFrom, sFileTo))

            If Not m_lReturn Then
                result = gPMConstants.PMEReturnCode.PMFalse

                iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed in ZipFile", vApp:=ACApp, vClass:=ACClass, vMethod:="MovePageFiles", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

        Next i

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    ' We must do the same on the main database too, in the
    ' commitserver object
    '
    ' ***************************************************************** '
    Private Function RollbackTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = g_oScanDB.SQLRollbackTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'must do same in commitserver object

            m_lReturn = g_oCommitServer.RollbackTrans

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        'iPMFunc.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub






    ' ***************************************************************** '
    ' Name: CommitBatch (Public)
    '
    ' Description: Main Commit procedure.
    '
    ' Edit History  :
    ' RAM20021218   : 1. Remvoed the Commit Locking Code, since we are using
    '                      a unique number
    '                 2. Ref. NRMA Project Changes. Sirius Process No. 189
    ' ***************************************************************** '
    Public Function CommitBatch(ByRef bCommitLocked As Boolean, Optional ByRef sDefaultAnnotation As String = "") As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get all documents from scan db
            m_sSQL = "SELECT doc_num FROM DOC_document"

            m_lReturn = g_oScanDB.SQLSelect(sSQL:=m_sSQL, sSQLName:="GETALLDOCS", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'No records ?
            If Not Information.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Definitely work to do, so lets get the business objects that will do this
            m_lReturn = GetBusinessObjects()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'indicate total docs to process

            g_lDocsTotal = vResultArray.GetUpperBound(1) - vResultArray.GetLowerBound(1) + 1

            'loop thru array, commiting each document

            For i As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

                If g_iRunStatus = DOCCommitCancelled Then

                    ClearBusinessObjects()

                    Return result

                End If

                'commit this document

                m_lReturn = CommitADocument(CInt(vResultArray(0, i)))

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    g_lDocsFailed += 1
                Else
                    g_lDocsDone += 1
                End If

            Next i


            ClearBusinessObjects()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=CStr(gPMConstants.PMELogLevel.PMLogError), vApp:=ACApp, vClass:=ACClass, vMethod:="CommitBatch", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: CommitADocument (Private)
    '
    ' Description: Write to main database and transfer physical files.
    '
    ' ***************************************************************** '
    Private Function CommitADocument(ByRef lScanDocNum As Integer) As Integer

        Dim result As Integer = 0
        Dim lMainDocNum, lFoldNum As Integer
        Dim sDocName As String = ""
        Dim dDocDate As Date
        Dim sDocType As String = ""
        Dim vPageList As Object
        Dim bDefaultScanFolder As Boolean


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'write the document details
            m_lReturn = DocumentCommit(lScanDocNum:=lScanDocNum, lMainDocNum:=lMainDocNum, lFoldNum:=lFoldNum, bDefaultScanFolder:=bDefaultScanFolder, sDocName:=sDocName, sDocType:=sDocType)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'write the doc info details
            m_lReturn = DocInfoCommit(lScanDocNum:=lScanDocNum, lMainDocNum:=lMainDocNum, dDocDate:=dDocDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = ClearJunckData(1, lMainDocNum)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'write the annotation details
            m_lReturn = AnnotationCommit(lScanDocNum:=lScanDocNum, lMainDocNum:=lMainDocNum)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = ClearJunckData(2, lMainDocNum)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' write document keyword
            'write the annotation details
            m_lReturn = KeywordCommit(lScanDocNum:=lScanDocNum, lMainDocNum:=lMainDocNum)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = ClearJunckData(3, lMainDocNum)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' write the page details

            m_lReturn = PageCommit(lScanDocNum:=lScanDocNum, lMainDocNum:=lMainDocNum, vPageList:=vPageList)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = ClearJunckData(4, lMainDocNum)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Move task to SA server database
            m_lReturn = TaskCommit(lScanDocNum:=lScanDocNum, lMainDocNum:=lMainDocNum)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = ClearJunckData(5, lMainDocNum)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Move physical documents into 00 tree on server

            m_lReturn = MovePageFiles(lScanDocNum:=lScanDocNum, vPageList:=vPageList)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = ClearJunckData(6, lMainDocNum)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'The document has commited succesfully.
            ' All's well to this point, so finally we can delete the physical files
            m_lReturn = m_oMisc.DeleteScannedPageFiles(lScanDocNum:=lScanDocNum, sScanDirectory:=g_sScanDirectory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'note the error but can happily continue
                result = gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = BeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = ClearJunckData(6, lMainDocNum)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Delete document from scan db
            m_lReturn = m_oMisc.DeleteScannedDoc(lScanDocNum:=lScanDocNum)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                m_lReturn = ClearJunckData(6, lMainDocNum)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Everything fine so commit
            m_lReturn = CommitTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            m_lReturn = RollbackTrans()

            iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="CommitADocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: ClearBusinessObjects (Private)
    '
    ' Description: Clear business objects.
    '
    ' ***************************************************************** '
    Private Sub ClearBusinessObjects()




        m_oFolder = Nothing
        m_oDocument = Nothing
        m_oPage = Nothing
        m_oDocInfo = Nothing
        m_oAnnotation = Nothing
        m_oDocKeyword = Nothing
        m_oMisc = Nothing
        m_oZipper = Nothing


    End Sub
    ' ***************************************************************** '
    ' Name: GetBusinessObjects (Private)
    '
    ' Description: Get all business objects.
    '
    ' ***************************************************************** '
    Private Function GetBusinessObjects() As Integer


        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        'Get business objects

        'via the public object manager.
        Dim temp_m_oFolder As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_m_oFolder, "bDOCFolder.Form", vInstanceManager:="ClientManager")
        m_oFolder = temp_m_oFolder

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Get the document business object reference for writing to the DB
        'via the public object manager.
        Dim temp_m_oDocument As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_m_oDocument, "bDOCDocument.Form", vInstanceManager:="ClientManager")
        m_oDocument = temp_m_oDocument

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Get the page business object reference for writing to the DB
        'via the public object manager.
        Dim temp_m_oPage As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_m_oPage, "bDOCpage.Form", vInstanceManager:="ClientManager")
        m_oPage = temp_m_oPage


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        'Get the docinfo business object reference for writing to the DB
        'via the public object manager.
        Dim temp_m_oDocInfo As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_m_oDocInfo, "bDOCDocInfo.Form", vInstanceManager:="ClientManager")
        m_oDocInfo = temp_m_oDocInfo


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Get the annotation business object reference for writing to the DB
        'via the public object manager.
        Dim temp_m_oAnnotation As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_m_oAnnotation, "bDOCAnnotation.Form", vInstanceManager:="ClientManager")
        m_oAnnotation = temp_m_oAnnotation


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Get the doc keywords business object reference for writing to the DB
        'via the public object manager.
        Dim temp_m_oDocKeyword As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_m_oDocKeyword, "bDOCDocKeyword.Form", vInstanceManager:="ClientManager")
        m_oDocKeyword = temp_m_oDocKeyword


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Get the miscellaneous class
        m_oMisc = New bDOCCommit.Miscellaneous()

        m_lReturn = m_oMisc.Initialise(v_sUsername:=g_sUsername, v_sPassword:=g_sPassword.Value, v_iUserID:=g_iUserID, v_iSourceID:=g_iSourceID, v_iLanguageID:=g_iLanguageID, v_iCurrencyID:=g_iCurrencyID, v_iLogLevel:=g_iLogLevel, v_sCallingAppName:=g_sCallingAppName, vDatabase:=g_oMainDB, vScanDatabase:=g_oScanDB)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'get the zipper
#If PD_EARLYBOUND = 1 Then

			 'Set m_oZipper = New bSIRZipper.Zipper
            Set m_oZipper = New bPMZipper.Business 
#Else
        m_oZipper = New bPMZipper.Business()
#End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DocumentCommit (Private)
    '
    ' Description: Get document details from scan db and write to main
    ' database. Return some extra stuff which will be needed if we write
    ' to the history table later.
    '
    ' ***************************************************************** '
    Private Function DocumentCommit(ByRef lScanDocNum As Integer, ByRef lMainDocNum As Integer, ByRef lFoldNum As Integer, ByRef bDefaultScanFolder As Boolean, ByRef sDocName As String, ByRef sDocType As String) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object
        Static lScanFolderNum As Integer
        Dim vExCode As Object



        result = gPMConstants.PMEReturnCode.PMTrue

        'get details from scan database
        m_sSQL = "SELECT doc_name, folder_num, doc_type, access_level, password, " & _
                 "create_date FROM DOC_document WHERE doc_num = " & CStr(lScanDocNum)

        m_lReturn = g_oScanDB.SQLSelect(sSQL:=m_sSQL, sSQLName:="GETDOCDETAILS", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=vResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        ' if parent folder is 0, then this is the default scan folder,
        ' so get its number

        If CDbl(vResultArray(1, 0)) = 0 Then

            bDefaultScanFolder = True

            ' we are scanning to generic scan folder, so check if we
            ' already have its number
            If lScanFolderNum = 0 Then

                ' we dont, so get it

                m_lReturn = g_oCommitServer.GetScanFolderNum(lScanFolderNum)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If lScanFolderNum = 0 Then

                    ' scan folder dont exist, so create it

                    m_lReturn = m_oFolder.DirectAdd(vFolderNum:=lScanFolderNum, vFolderName:=DOCDefaultScanFolder, vParentNum:=0)


                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'this is the folder num for this doc then
                    lFoldNum = lScanFolderNum

                Else

                    'this is the folder num for this doc then
                    lFoldNum = lScanFolderNum

                End If

            Else

                lFoldNum = lScanFolderNum

            End If

        Else

            bDefaultScanFolder = False

            lFoldNum = CInt(vResultArray(1, 0))

        End If

        ' need to get excode

        m_lReturn = g_oCommitServer.getexcode(v_vExCode:=vExCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'now write to main database

        m_lReturn = m_oDocument.DirectAdd(vDocNum:=lMainDocNum, vFolderNum:=lFoldNum, vDocName:=vResultArray(0, 0), vdoctype:=vResultArray(2, 0), vAccessLevel:=vResultArray(3, 0), vPassword:=vResultArray(4, 0), vZipped:="Y", vCreateDate:=vResultArray(5, 0), vExCode:=vExCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'return the doc name and doc type for later use

        sDocName = CStr(vResultArray(0, 0))

        sDocType = CStr(vResultArray(2, 0))

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: DocInfoCommit (Private)
    '
    ' Description: Get document details from scan db and write to main
    ' database.
    '
    ' ***************************************************************** '
    Private Function DocInfoCommit(ByRef lScanDocNum As Integer, ByRef lMainDocNum As Integer, ByRef dDocDate As Date) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object




        result = gPMConstants.PMEReturnCode.PMTrue

        'get details from scan database
        m_sSQL = "SELECT expiry_date, scan_user, doc_date " & _
                 "FROM DOC_doc_info WHERE doc_num = " & CStr(lScanDocNum)

        m_lReturn = g_oScanDB.SQLSelect(sSQL:=m_sSQL, sSQLName:="GETDOCINFODETAILS", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=vResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'now write to main database

        m_lReturn = m_oDocInfo.DirectAdd(vDocNum:=lMainDocNum, vExpiryDate:=vResultArray(0, 0), vScanUser:=vResultArray(1, 0), vDocDate:=vResultArray(2, 0), vLastUser:=vResultArray(1, 0), vLastDate:=DateTime.Now)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'return doc date

        dDocDate = CDate(vResultArray(2, 0))

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: AnnotationCommit (Private)
    '
    ' Description: Get annotation details from scan db and write to main
    ' database.
    '
    ' ***************************************************************** '
    Private Function AnnotationCommit(ByRef lScanDocNum As Integer, ByRef lMainDocNum As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object



        result = gPMConstants.PMEReturnCode.PMTrue

        'write batch annotation if present
        If g_sBatchAnnotation.Trim() <> "" Then


            m_lReturn = m_oAnnotation.DirectAdd(vDocNum:=lMainDocNum, vAnnText:=g_sBatchAnnotation, vUsername:=g_sUsername, vCreateDate:=DateTime.Now)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End If

        'get annotation details from scan database
        m_sSQL = "SELECT ann_text, user_name, create_date " & _
                 "FROM DOC_annotation WHERE doc_num = " & CStr(lScanDocNum)

        m_lReturn = g_oScanDB.SQLSelect(sSQL:=m_sSQL, sSQLName:="GETANNDETAILS", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Information.IsArray(vResultArray) Then
            Return result
        End If

        'go thru each ann

        For i As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

            Application.DoEvents()

            'now write to main database

            m_lReturn = m_oAnnotation.DirectAdd(vDocNum:=lMainDocNum, vAnnText:=vResultArray(0, i), vUsername:=vResultArray(1, i), vCreateDate:=vResultArray(2, i))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Next i

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: KeywordCommit (Private)
    '
    ' Description: Get keyword details from scan db and write to main
    ' database.
    '
    ' ***************************************************************** '
    Private Function KeywordCommit(ByRef lScanDocNum As Integer, ByRef lMainDocNum As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object




        result = gPMConstants.PMEReturnCode.PMTrue

        'get keyword details from scan database
        m_sSQL = "SELECT keyword_id, user_name, create_date " & _
                 "FROM DOC_doc_keyword WHERE doc_num = " & CStr(lScanDocNum)

        m_lReturn = g_oScanDB.SQLSelect(sSQL:=m_sSQL, sSQLName:="GETKWDETAILS", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Information.IsArray(vResultArray) Then
            Return result
        End If

        'go thru each keyword

        For i As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

            Application.DoEvents()

            'now write to main database

            m_lReturn = m_oDocKeyword.DirectAdd(vKeywordID:=vResultArray(0, i), vDocNum:=lMainDocNum, vUsername:=vResultArray(1, i), vCreateDate:=vResultArray(2, i))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Next i

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: PageCommit (Private)
    '
    ' Description: Get page details from scan db and write to main
    ' database. Return array of page nums and name so can be moved
    ' into 00 tree later.
    '
    ' ***************************************************************** '
    Private Function PageCommit(ByRef lScanDocNum As Integer, ByRef lMainDocNum As Integer, ByRef vPageList(,) As Object) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object
        Dim sNextPageName As String = ""
        Dim bFirst As Boolean




        result = gPMConstants.PMEReturnCode.PMTrue


        'get page details from scan database
        m_sSQL = "SELECT page_type, page_num, page_size, create_date " & _
                 "FROM DOC_page WHERE doc_num = " & CStr(lScanDocNum)

        m_lReturn = g_oScanDB.SQLSelect(sSQL:=m_sSQL, sSQLName:="GETPAGEDETAILS", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Information.IsArray(vResultArray) Then
            'no pages ? Cant be right.
            result = gPMConstants.PMEReturnCode.PMFalse

            iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="No pages for scanned document " & lScanDocNum, vApp:=ACApp, vClass:=ACClass, vMethod:="PageCommit", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            Return result
        End If


        'Go thru each page
        bFirst = True

        For i As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

            Application.DoEvents()

            'get next page name from main db
            m_lReturn = m_oMisc.GetNextPageName(sNextPageName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'now write to main database

            m_lReturn = m_oPage.DirectAdd(vPageName:=sNextPageName, vDocNum:=lMainDocNum, vPageNum:=vResultArray(1, i), vPageType:=vResultArray(0, i), vPageSize:=vResultArray(2, i), vCreateDate:=vResultArray(3, i))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'save the page num and name in the array for moving at a later date
            If bFirst Then
                bFirst = False
                ReDim vPageList(2, 0)


                vPageList(0, 0) = vResultArray(1, i)


                vPageList(1, 0) = CStr(vResultArray(0, i)).Trim()

                vPageList(2, 0) = sNextPageName
            Else
                ReDim Preserve vPageList(2, vPageList.GetUpperBound(1) + 1)


                vPageList(0, vPageList.GetUpperBound(1)) = vResultArray(1, i)


                vPageList(1, vPageList.GetUpperBound(1)) = CStr(vResultArray(0, i)).Trim()

                vPageList(2, vPageList.GetUpperBound(1)) = sNextPageName
            End If

        Next i

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: TaskCommit (Private)
    '
    ' Description: Get task details from scan db and write to SA database
    '
    ' ***************************************************************** '
    Private Function TaskCommit(ByRef lScanDocNum As Integer, ByRef lMainDocNum As Integer) As Integer

        Dim result As Integer = 0
        Dim vTempArray(,), vResultArray(,) As Object
        Dim sParams As String = ""
        Dim vDatabase As Object
        Dim sSQL As String = ""
        Dim iTaskGroupID, iTaskID As Integer
        Dim bSBOInstalled As Boolean
        Dim vTaskCnt As Byte



        result = gPMConstants.PMEReturnCode.PMTrue

        vTaskCnt = 0

        ' check for existance of SBO

        m_lReturn = g_oCommitServer.IsSBOInstalled(bSBOInstalled)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to determine if Sirius Back-office is installed.", vApp:=ACApp, vClass:=ACClass, vMethod:="TaskCommit", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' if we don't have SBO, exit this function because we don't want to create the task.
        If Not bSBOInstalled Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If



        'get details from scan database

        sParams = "Doc_Num, customer, task_due_date, PMUser_Group_ID, " & _
                  "User_ID, Description, Task_Status, Urgent, Date_Created, Created_By_ID"


        m_sSQL = "SELECT " & sParams & _
                 " FROM DOC_task WHERE doc_num = " & CStr(lScanDocNum)

        m_lReturn = g_oScanDB.SQLSelect(sSQL:=m_sSQL, sSQLName:="GETTASKDETAILS", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=vResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get task details from scan database for document " & lScanDocNum, vApp:=ACApp, vClass:=ACClass, vMethod:="TaskCommit", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' DN 12/06/02 - Exit from funtion if the task status is set to 0

        If CInt(vResultArray(6, 0)) = 0 Then
            Return result
        End If

        'now write task to the Sirius Architecture database

        ' get SA database link

        m_lReturn = g_oCommitServer.GetSADatabase(vDatabase)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Could not get details of Sirius Architecture database", vApp:=ACApp, vClass:=ACClass, vMethod:="TaskCommit", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        ' get task group id for documaster group
        sSQL = "SELECT pmwrk_task_group_id From PMWrk_Task_Group WHERE code = 'Documaster'"

        m_lReturn = vDatabase.SQLSelect(sSQL, "Documaster", False, 100, vTempArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get Documaster task group from SA database", vApp:=ACApp, vClass:=ACClass, vMethod:="TaskCommit", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        iTaskGroupID = CInt(vTempArray(0, 0))


        ' get task for opening documaster

        m_lReturn = vDatabase.SQLSelect("SELECT pmwrk_task_id " & _
                    "From PMWrk_Task WHERE code = 'DMELOAD'", "Task ID for Documaster Load", False, 100, vTempArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get Documaster task from SA database", vApp:=ACApp, vClass:=ACClass, vMethod:="TaskCommit", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        iTaskID = CInt(vTempArray(0, 0))

        'setup task array to pass document description (doc number) to doc viewer
        Dim vTaskInstKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) As Object


        vTaskInstKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMKeyNameTaskDescription

        vTaskInstKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = lMainDocNum


        vTaskInstKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMKeyNameProcessType

        vTaskInstKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = "SCAN"

        ' create new scheduled task









        m_lReturn = g_oCommitServer.TaskCommit(r_lpmwrktaskinstancecnt:=vTaskCnt, v_lPMWrkTaskGroupID:=iTaskGroupID, v_lPMWrkTaskID:=iTaskID, v_scustomer:=CStr(vResultArray(1, 0)), v_dttaskduedate:=CDate(vResultArray(2, 0)), v_lpmusergroupid:=CInt(vResultArray(3, 0)), v_iUserID:=CInt(vResultArray(4, 0)), v_sDescription:=CStr(vResultArray(5, 0)), v_itaskstatus:=0, v_iisurgent:=CInt(vResultArray(7, 0)), v_dtdatecreated:=CDate(vResultArray(8, 0)), v_icreatedbyid:=CInt(vResultArray(9, 0)), v_vKeyArray:=vTaskInstKeyArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to create new scheduled task", vApp:=ACApp, vClass:=ACClass, vMethod:="TaskCommit", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Pass back the task cnt in case it needs deleting
        g_vTaskCnt = vTaskCnt

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: ClearJunckData (Private)
    '
    ' Description: Delete records written to main database as part of
    '              commiting a record to the database
    '
    ' Author        : Kevin Renshaw
    ' Note          : Coding done by Kevin, Checking in by Ram Chandrabose
    ' Edit History  :
    ' RAM20021218   : 1. Added this function for NRMA Project.
    '                 2. Ref. Sirius Process No. 189
    '
    ' ***************************************************************** '
    Private Function ClearJunckData(ByRef iLevel As Integer, ByRef lDocNum As Integer) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim vDatabase As Object
        Dim sSQL, sSQLDOCDocument, sSQLDOCInfoCommit, sSQLAnnotationCommit, sDocKeyworkCommit, sPageCommit, sTask As String



        result = gPMConstants.PMEReturnCode.PMTrue

        sSQLDOCDocument = " DELETE FROM DOC_Document where doc_num = " & lDocNum
        sSQLDOCInfoCommit = " DELETE FROM DOC_Doc_Info where doc_num = " & lDocNum
        sSQLAnnotationCommit = " DELETE FROM DOC_annotation where doc_num = " & lDocNum
        sDocKeyworkCommit = " DELETE FROM DOC_doc_keyword where doc_num = " & lDocNum
        sPageCommit = " DELETE FROM DOC_page where doc_num = " & lDocNum

        sTask = " DELETE FROM PMWrk_Task_Instance where pmwrk_task_instance_cnt = " & (IIf(TaskCnt.Equals(0), CStr(0), CStr(TaskCnt)))


        Select Case iLevel
            Case 1 ' DOC_Document
                sSQL = sSQLDOCDocument
            Case 2 ' DOC_Doc_Info
                sSQL = sSQLDOCInfoCommit & sSQLDOCDocument
            Case 3 ' DOC_Annotation
                sSQL = sSQLAnnotationCommit & sSQLDOCInfoCommit & sSQLDOCDocument
            Case 4 ' DOC_Doc_Keyword
                sSQL = sDocKeyworkCommit & sSQLAnnotationCommit & sSQLDOCInfoCommit & sSQLDOCDocument
            Case 5 ' DOC_Page
                sSQL = sPageCommit & sDocKeyworkCommit & sSQLAnnotationCommit & sSQLDOCInfoCommit & sSQLDOCDocument
            Case 6 ' Task
                sSQL = sTask & sPageCommit & sDocKeyworkCommit & sSQLAnnotationCommit & sSQLDOCInfoCommit & sSQLDOCDocument

            Case Else
                'Shouldn't be in here
        End Select


        ' get SA database link

        m_lReturn = g_oCommitServer.GetSADatabase(vDatabase)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = vDatabase.SQLAction(sSQL:=sSQL, sSQLName:="DELETEDOCRECS", bStoredProcedure:=False)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function
End Class