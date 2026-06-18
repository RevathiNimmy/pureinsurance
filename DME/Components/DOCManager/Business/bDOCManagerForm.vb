Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.IO
Imports System.Windows.Forms
Imports SharedFiles
Imports Sspi.Common.Aws.S3

<System.Runtime.InteropServices.ProgId("Form_NET.Form")>
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
    ' MS 04/10/700         Briefcase Functionality project for DME 1.1
    '
    '       Added the following subs/functions
    '
    '       ConnectToLocalDB, DisconnectLocalDB, UpdateLocalDB,  GetChildFolder
    '       GetParentExCode, InsertFolderRec, GetFolderRec, UpdateDocuments
    '       UpdateDocInfoRecs, UpdateKeywordInfo, TransferPages, UpdateLinkedDocs
    '       SetBriefCaseDir, Pause, ProcessEnd, IsThere
    '
    '
    '       Added Modules and Classes
    '
    '       ADVReg.bas, DMEDSN.cls
    '
    ' MS 26/10/2000
    '
    '       Enhancements made to integrate DME with SBO
    '
    ' DN270802 - Change embedded SQL to reflect table changes
    '
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Form"

    ' PUBLIC Data Members (Begin)
    Const m_cComma As String = ","  'Comma char
    Const m_cQuote As String = "'" 'Single quote char
    Const m_cDataFile As String = "PMBriefcase.mdf"
    Const m_cLogFile As String = "PMBriefcase.ldf"
    Const m_cPMBriefcase As String = "PMBriefcase"
    Const m_cTempDME As String = "TempDME"

    ' PUBLIC Data Members  (End)

    ' PRIVATE Data Members (Begin)
    Private m_sSQL As String = ""

    'Temp field to validate SQL statements
    Private m_sTmp As String = ""

    ' Database Class (Private)
#If PD_EARLYBOUND = 1 Then

	Private m_oDatabase As dPMDAO.Database
#Else
    Private m_oDatabase As dPMDAO.Database
#End If

#If PD_EARLYBOUND = 1 Then

	Private m_oLocalDB As dPMDAO.Database    'Briefcase db
#Else
    Private m_oLocalDB As dPMDAO.Database
#End If

    Private m_oDestDSN As DmeDSN

    ' RAM20021223 : Changed all variable to late binding
    'Business objects
    Private m_oFolder As bDOCFolder.Form  ' bDOCFolder.Form
    Private m_oDocument As bDOCDocument.Form  ' bDOCdocument.Form
    Private m_oDocInfo As bDOCDocInfo.Form  ' bDOCDocInfo.Form
    Private m_oAnnotation As bDOCAnnotation.Form  ' bDOCAnnotation.Form
    Private m_oDocKeyword As bDOCDocKeyword.Form  ' bDOCDocKeyword.Form
    Private m_oPage As bDOCPage.Form  ' bDOCPage.Form
    Private m_oHistory As bDOCHistory.Form  ' bDOCHistory.Form

    'Miscellaeous class
    Private m_oMiscellaneous As bDOCManager.Miscellaneous

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

    ' ***************************************************************** '
    ' Standard Product Family Constant (Read Only)
    ' ***************************************************************** '

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            '
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

    Public Property AdminLevel() As Integer
        Get

            Return g_iAdminLevel

        End Get
        Set(ByVal Value As Integer)

            g_iAdminLevel = Value

        End Set
    End Property


    Public Property AccessLevel() As Integer
        Get

            Return g_iAccessLevel

        End Get
        Set(ByVal Value As Integer)

            g_iAccessLevel = Value

        End Set
    End Property

    ' CTAF 20040519 - Changed from Integer to Long
    Public ReadOnly Property HomeFolder() As Integer
        Get
            Return g_lHomeFolder

        End Get
    End Property



    Public Property FileCopyLevel() As Integer
        Get
            Return g_iFileCopyLevel
        End Get
        Set(ByVal Value As Integer)
            g_iFileCopyLevel = Value
        End Set
    End Property

    Public Property FileDeleteLevel() As Integer
        Get
            Return g_iFileDeleteLevel
        End Get
        Set(ByVal Value As Integer)
            g_iFileDeleteLevel = Value
        End Set
    End Property

    Public Property FileMoveLevel() As Integer
        Get
            Return g_iFileMoveLevel
        End Get
        Set(ByVal Value As Integer)
            g_iFileMoveLevel = Value
        End Set
    End Property

    Public Property folderCopyLevel() As Integer
        Get
            Return g_iFolderCopyLevel
        End Get
        Set(ByVal Value As Integer)
            g_iFolderCopyLevel = Value
        End Set
    End Property

    Public Property folderDeleteLevel() As Integer
        Get
            Return g_iFolderDeleteLevel
        End Get
        Set(ByVal Value As Integer)
            g_iFolderDeleteLevel = Value
        End Set
    End Property

    Public Property folderMoveLevel() As Integer
        Get
            Return g_iFolderMoveLevel
        End Get
        Set(ByVal Value As Integer)
            g_iFolderMoveLevel = Value
        End Set
    End Property



    '***VarDataEnd***

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


            'Get the folder business object reference for writing to the DB
            'Set m_oFolder = New bDOCFolder.Form

            ' RAM20021223 : Use CreateObject Method instead of New (Late Binding)
            m_oFolder = New bDOCFolder.Form()

            m_lReturn = m_oFolder.Initialise(sUsername:=g_sUsername, sPassword:=g_sPassword.Value, iUserID:=g_iUserID, iSourceID:=g_iSourceID, iLanguageID:=g_iLanguageID, iCurrencyID:=g_iCurrencyID, iLogLevel:=g_iLogLevel, sCallingAppName:=g_sCallingAppName, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the document business object reference for writing to the DB
            'Set m_oDocument = New bDOCdocument.Form

            ' RAM20021223 : Use CreateObject Method instead of New (Late Binding)
            m_oDocument = New bDOCDocument.Form()

            m_lReturn = m_oDocument.Initialise(sUsername:=g_sUsername, sPassword:=g_sPassword.Value, iUserID:=g_iUserID, iSourceID:=g_iSourceID, iLanguageID:=g_iLanguageID, iCurrencyID:=g_iCurrencyID, iLogLevel:=g_iLogLevel, sCallingAppName:=g_sCallingAppName, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the docinfo business object reference for writing to the DB
            'Set m_oDocInfo = New bDOCDocInfo.Form

            ' RAM20021223 : Use CreateObject Method instead of New (Late Binding)
            m_oDocInfo = New bDOCDocInfo.Form()

            m_lReturn = m_oDocInfo.Initialise(sUsername:=g_sUsername, sPassword:=g_sPassword.Value, iUserID:=g_iUserID, iSourceID:=g_iSourceID, iLanguageID:=g_iLanguageID, iCurrencyID:=g_iCurrencyID, iLogLevel:=g_iLogLevel, sCallingAppName:=g_sCallingAppName, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the docinfo business object reference for writing to the DB
            'Set m_oPage = New bDOCPage.Form

            ' RAM20021223 : Use CreateObject Method instead of New (Late Binding)
            m_oPage = New bDOCPage.Form()

            m_lReturn = m_oPage.Initialise(sUsername:=g_sUsername, sPassword:=g_sPassword.Value, iUserID:=g_iUserID, iSourceID:=g_iSourceID, iLanguageID:=g_iLanguageID, iCurrencyID:=g_iCurrencyID, iLogLevel:=g_iLogLevel, sCallingAppName:=g_sCallingAppName, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the Annotation business object reference for writing to the DB
            'Set m_oAnnotation = New bDOCAnnotation.Form

            ' RAM20021223 : Use CreateObject Method instead of New (Late Binding)
            m_oAnnotation = New bDOCAnnotation.Form()

            m_lReturn = m_oAnnotation.Initialise(sUsername:=g_sUsername, sPassword:=g_sPassword.Value, iUserID:=g_iUserID, iSourceID:=g_iSourceID, iLanguageID:=g_iLanguageID, iCurrencyID:=g_iCurrencyID, iLogLevel:=g_iLogLevel, sCallingAppName:=g_sCallingAppName, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the keyword business object reference for writing to the DB
            'Set m_oDocKeyword = New bDOCDocKeyword.Form

            ' RAM20021223 : Use CreateObject Method instead of New (Late Binding)
            m_oDocKeyword = New bDOCDocKeyword.Form()

            m_lReturn = m_oDocKeyword.Initialise(sUsername:=g_sUsername, sPassword:=g_sPassword.Value, iUserID:=g_iUserID, iSourceID:=g_iSourceID, iLanguageID:=g_iLanguageID, iCurrencyID:=g_iCurrencyID, iLogLevel:=g_iLogLevel, sCallingAppName:=g_sCallingAppName, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the History business object reference for writing to the DB
            'Set m_oHistory = New bDOCHistory.Form

            ' RAM20021223 : Use CreateObject Method instead of New (Late Binding)
            m_oHistory = New bDOCHistory.Form()

            m_lReturn = m_oHistory.Initialise(sUsername:=g_sUsername, sPassword:=g_sPassword.Value, iUserID:=g_iUserID, iSourceID:=g_iSourceID, iLanguageID:=g_iLanguageID, iCurrencyID:=g_iCurrencyID, iLogLevel:=g_iLogLevel, sCallingAppName:=g_sCallingAppName, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Instance the miscellaneous class
            m_oMiscellaneous = New bDOCManager.Miscellaneous()

            m_lReturn = m_oMiscellaneous.Initialise(v_sUsername:=g_sUsername, v_sPassword:=g_sPassword.Value, v_iUserID:=g_iUserID, v_iSourceID:=g_iSourceID, v_iLanguageID:=g_iLanguageID, v_iCurrencyID:=g_iCurrencyID, v_iLogLevel:=g_iLogLevel, v_sCallingAppName:=g_sCallingAppName, vDatabase:=m_oDatabase, vHistory:=m_oHistory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the users access level
            m_lReturn = CType(GetUserAccessLevel(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the global admin level
            m_lReturn = CType(GetAdminLevel(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the users home folder
            m_lReturn = CType(GetHomeFolder(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the access levels for file and folder move, copy and delete
            m_lReturn = GetFileAndFolderAccess()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
                If m_oFolder IsNot Nothing Then
                    m_oFolder.Dispose()
                    m_oFolder = Nothing
                End If
                If m_oDocument IsNot Nothing Then
                    m_oDocument.Dispose()
                End If
                m_oDocument = Nothing
                If m_oDocInfo IsNot Nothing Then
                    m_oDocInfo.Dispose()
                End If
                m_oDocInfo = Nothing
                If m_oPage IsNot Nothing Then
                    m_oPage.Dispose()
                End If
                m_oPage = Nothing
                If m_oAnnotation IsNot Nothing Then
                    m_oAnnotation.Dispose()
                End If
                m_oAnnotation = Nothing
                If m_oDocKeyword IsNot Nothing Then
                    m_oDocKeyword.Dispose()
                End If
                m_oDocKeyword = Nothing
                If m_oHistory IsNot Nothing Then
                    m_oHistory.Dispose()
                End If
                m_oHistory = Nothing
                m_oMiscellaneous = Nothing

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
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetPageList
    '
    ' Description: Wrapper to function proper in miscellaneous class.
    '
    ' ***************************************************************** '
    Public Function GetPageList(ByRef lDocNum As Integer, ByRef vPageArray As Object) As Integer

        Dim result As Integer = 0
        Dim lLinkNum As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'first check for links
            m_lReturn = CType(m_oMiscellaneous.GetDocLink(lDocNum, lLinkNum), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'now get the pages
            If lLinkNum = 0 Then

                m_lReturn = CType(m_oMiscellaneous.GetPageList(lDocNum:=lDocNum, vPageArray:=vPageArray), gPMConstants.PMEReturnCode)

            Else

                m_lReturn = CType(m_oMiscellaneous.GetPageList(lDocNum:=lLinkNum, vPageArray:=vPageArray), gPMConstants.PMEReturnCode)

            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetPageList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        'iPMFunc.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    ' ***************************************************************** '
    ' Name: GetFolderList
    '
    ' Description: This function hits the DB for a list of child folders
    ' for a given parent folder. A filter can be supplied to select folders
    ' starting with a certain string. The number of recs returned is
    ' configured in the interface.
    '
    ' ***************************************************************** '
    Public Function GetFolderList(ByRef lParentNum As Integer, ByRef sFilter As String, ByRef lMaxFoldersReturned As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lTmp As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '   Construct SQL
            '    m_sSQL$ = "SELECT folder_num, folder_name, password, create_date FROM DOC_folder "
            '    m_sSQL$ = m_sSQL$ & "WHERE parent_num = " & lParentNum & " AND folder_name > '"
            '    m_sSQL$ = m_sSQL$ & sFilter$ & "' AND access_level >= " & g_iAccessLevel%
            '    m_sSQL$ = m_sSQL$ = " ORDER BY folder_name"

            'Check max returned
            m_oDatabase.Parameters.Clear()
            If lMaxFoldersReturned = 0 Then
                lTmp = gPMConstants.PMAllRecords
            Else
                lTmp = lMaxFoldersReturned
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="parent_num", vValue:=lParentNum, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="filter", vValue:=sFilter, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="access_level", vValue:=g_iAccessLevel, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=g_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_sSQL = "spu_DOC_select_folder"

            'Hit the DB
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="GETFOLDERLIST", bStoredProcedure:=True, lNumberRecords:=lTmp, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetFolderList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetMatchedFolderList
    '
    ' Description: This function hits the DB for a list of child folders
    ' for a given parent folder that match the given string sFolderName.
    ' The number of recs returned is configured in the interface.
    '
    '
    ' ***************************************************************** '
    Public Function GetMatchedFolderList(ByRef sFolderName As String, ByRef lMaxFoldersReturned As Integer, ByRef lParentNum As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lTmp As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Check max returned
            If lMaxFoldersReturned = 0 Then
                lTmp = gPMConstants.PMAllRecords
            Else
                lTmp = lMaxFoldersReturned
            End If

            'sFolderName = "%" & sFolderName & "%"
            'let them use a wildcard *, but SQL Server uses %
            'put wildcard on the end anyway.
            m_oDatabase.Parameters.Clear()
            If sFolderName <> "*" Then
                If sFolderName.StartsWith("*") Then
                    sFolderName = "%" & sFolderName.Substring(sFolderName.Length - (sFolderName.Length - 1))
                End If

                If sFolderName.EndsWith("*") Then
                    sFolderName = sFolderName.Substring(0, sFolderName.Length - 1) & "%"
                Else
                    sFolderName = sFolderName & "%"
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="parent_num", vValue:=lParentNum, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = m_oDatabase.Parameters.Add(sName:="folder_name", vValue:=sFolderName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = m_oDatabase.Parameters.Add(sName:="access_level", vValue:=g_iAccessLevel, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_sSQL = "spu_DOC_select_matched_folders"


                'Hit the DB
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="GETMATCHEDFOLDERLIST", bStoredProcedure:=True, lNumberRecords:=lTmp, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="Failed to Get Matched Folder List", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetNodeKey
    '
    ' Description: This function hits the DB for info required to
    ' construct a node key for the supplied external code and level
    '
    ' ***************************************************************** '
    Public Function GetNodeKey(ByRef sExCode As String, ByRef iFolderLevel As Integer, ByRef lParentNum As Integer, ByRef sFolderNum As Integer, ByRef sPassword As String, ByRef dCreateDate As Date, ByRef bNoAccess As Boolean) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Construct SQL
            m_sSQL = "SELECT folder_num, password, create_date, access_level " &
                     "FROM DOC_folder WHERE ex_code = '" & sExCode & "'" &
                     " AND folder_level = " & CStr(iFolderLevel) &
                     " AND parent_num = " & CStr(lParentNum)

            'Hit the DB - should be only one record match, but request more than one
            'as will show up any errors.
            m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="GETFOLDERKEY", bStoredProcedure:=False, lNumberRecords:=2, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' return values if rec found - ensure only one record returned as we need
            ' to check for duplicate external codes
            If Information.IsArray(vResultArray) Then


                If vResultArray.GetLowerBound(1) = vResultArray.GetUpperBound(1) Then

                    'check access level

                    If CDbl(vResultArray(3, 0)) < g_iAccessLevel Then
                        bNoAccess = True
                    Else
                        bNoAccess = False

                        sFolderNum = CInt(vResultArray(0, 0))

                        sPassword = CStr(vResultArray(1, 0))

                        dCreateDate = CDate(vResultArray(2, 0))
                    End If

                Else
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Node not unique, ex_code = " & sExCode, vApp:=ACApp, vClass:=ACClass, vMethod:="GetNodeKey", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                End If

            Else
                'this is wrong
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Node not found, ex_code = " & sExCode, vApp:=ACApp, vClass:=ACClass, vMethod:="GetNodeKey", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If


            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetNodeKey", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDocList
    '
    ' Description: This fucntion hits the DB for a list of documents in
    ' a given parent folder. A filter can be supplied to select docs
    ' starting with a certain string. The number of recs returned is
    ' configured in the interface
    '
    ' ***************************************************************** '
    Public Function GetDocList(ByRef lParentNum As Integer, ByRef sFilter As String, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Construct SQL
            '    m_sSQL$ = "SELECT doc_num, doc_name, password, doc_type, create_date FROM DOC_document "
            '    m_sSQL$ = m_sSQL$ & "WHERE folder_num = " & lParentNum & " AND doc_name > '"
            '    m_sSQL$ = m_sSQL$ & sFilter$ & "' AND access_level >= " & g_iAccessLevel%
            '    m_sSQL$ = m_sSQL$ & " ORDER BY doc_name"
            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="folder_num", vValue:=lParentNum, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="filter", vValue:=sFilter, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="access_level", vValue:=g_iAccessLevel, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_sSQL = "spu_DOC_select_document"

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="GETDOCLIST", bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function
    ' ***************************************************************** '
    ' Name: AmIZippedUp
    '
    ' Description: Check if a document is zipped or not
    '
    ' ***************************************************************** '
    Public Function AmIZippedUp(ByRef lDocNum As Integer, ByRef bZipped As Boolean) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Construct SQL
            m_sSQL = "SELECT zipped FROM DOC_document WHERE doc_num = " & lDocNum

            m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="AMIZIPPEDUP", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'return the value
            If Information.IsArray(vResultArray) Then


                If CStr(vResultArray(0, 0)) = "Y" Then
                    bZipped = True
                Else

                    If CStr(vResultArray(0, 0)) = "N" Then
                        bZipped = False
                    Else
                        result = gPMConstants.PMEReturnCode.PMFalse
                        ' Log Error Message
                        iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unknown Zip Value", vApp:=ACApp, vClass:=ACClass, vMethod:="AmIZippedUp")
                    End If
                End If
            Else
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="No data returned", vApp:=ACApp, vClass:=ACClass, vMethod:="AmIZippedUp")
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="AmIZippedUp", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDocKeywordList
    '
    ' Description: This fucntion hits the DB for a list of keywords
    ' for the supplied document num.
    '
    ' ***************************************************************** '
    Public Function GetDocKeywordList(ByRef lDocNum As Integer, ByRef vResultArray(,) As Object) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Construct SQL
            m_sSQL = "SELECT DOC_doc_keyword.doc_keyword_id, keyword, user_name, create_date " &
                     "FROM DOC_keyword, DOC_doc_keyword WHERE doc_num = " & CStr(lDocNum) &
                     " AND DOC_doc_keyword.keyword_id = DOC_keyword.keyword_id"

            'Hit DB
            m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="GETDOCKEYWORDLIST", bStoredProcedure:=False, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocKeywordList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function
    ' ***************************************************************** '
    ' Name: DeleteAnnotation
    '
    ' Description: This function deletes the supplied annotation.
    '
    ' ***************************************************************** '
    Public Function DeleteAnnotation(ByRef lAnnId As Integer) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Construct SQL
            m_sSQL = "DELETE FROM DOC_annotation WHERE annotation_id = " & lAnnId

            m_lReturn = CType(m_oDatabase.SQLAction(sSQL:=m_sSQL, sSQLName:="DELANN", bStoredProcedure:=False), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteAnnotation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: DeleteDocKeyword
    '
    ' Description: Wrapper to function proper in miscellaneous
    '
    ' ***************************************************************** '
    Public Function DeleteDocKeyword(ByRef lDocKeywordID As Integer) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'call proper function
            m_lReturn = CType(m_oMiscellaneous.DeleteDocKeyword(lDocKeywordID:=lDocKeywordID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteDocKeyword", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddAnnotation
    '
    ' Description: This function adds an annotation for a document.
    '
    ' ***************************************************************** '
    Public Function AddAnnotation(ByRef lDocNum As Integer, ByRef sAnnText As String) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oAnnotation.DirectAdd(vDocNum:=lDocNum, vAnnText:=sAnnText, vUsername:=g_sUsername)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="AddAnnotation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: SetHomeFolder
    '
    ' Description: This function adds an annotation for a document.
    '
    ' ***************************************************************** '
    Public Function SetHomeFolder(ByRef lFolderNum As Integer) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_sSQL = "UPDATE DOC_doc_user SET home_folder_num = " & lFolderNum &
                     " WHERE user_id = " & CStr(g_iUserID)

            'slap the database
            m_lReturn = CType(m_oDatabase.SQLAction(sSQL:=m_sSQL, sSQLName:="SETHOMEFOLDER", bStoredProcedure:=False), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            g_lHomeFolder = lFolderNum

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="SetHomeFolder", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetAnnotationList
    '
    ' Description: This fucntion hits the DB for a list of annotations
    ' for the supplied document num.
    '
    ' ***************************************************************** '
    Public Function GetAnnotationList(ByRef lDocNum As Integer, ByRef vResultArray(,) As Object) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Construct SQL
            m_sSQL = "SELECT annotation_id, ann_text, user_name , create_date " &
                     "FROM DOC_annotation WHERE doc_num = " & CStr(lDocNum)

            'Hit DB
            m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="GETANNOTATIONLIST", bStoredProcedure:=False, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetAnnotationList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: CopyAnnotations
    '
    ' Description: This copies all the annotations for a doc to the new,
    ' doc, when a copy document(s) transaction takes place. Supplied are
    ' the original doc number, and the new doc num.
    '
    ' ***************************************************************** '
    Private Function CopyAnnotations(ByRef lSourceDocNum As Integer, ByRef lDestDocNum As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object




        result = gPMConstants.PMEReturnCode.PMTrue

        'First get the annotations for the source document

        'Construct SQL
        m_sSQL = "SELECT ann_text, user_name, create_date " &
                 "FROM DOC_annotation WHERE doc_num = " & CStr(lSourceDocNum)

        'Hit the DB
        m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="COPYANNOTATIONS", bStoredProcedure:=False, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Go thru annotations, writing new ones for the new doc
        If Information.IsArray(vResultArray) Then


            For I As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

                Application.DoEvents()

                m_lReturn = m_oAnnotation.DirectAdd(vDocNum:=lDestDocNum, vAnnText:=vResultArray(0, I), vUsername:=vResultArray(1, I), vCreateDate:=vResultArray(2, I))

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Next I

        End If

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: CopyKeywords
    '
    ' Description: This copies all the keywords for a doc to the new,
    ' doc, when a copy document(s) transaction takes place. Supplied are
    ' the original doc number, and the new doc num.
    '
    ' ***************************************************************** '
    Private Function CopyKeywords(ByRef lSourceDocNum As Integer, ByRef lDestDocNum As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object




        result = gPMConstants.PMEReturnCode.PMTrue

        'First get the keywords for the source document

        'Construct SQL
        m_sSQL = "SELECT keyword_id, user_name, create_date " &
                 "FROM DOC_doc_keyword WHERE doc_num = " & CStr(lSourceDocNum)

        'Hit the DB
        m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="COPYKW", bStoredProcedure:=False, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Go thru keywords, writing new ones for the new doc
        If Information.IsArray(vResultArray) Then


            For I As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

                Application.DoEvents()

                m_lReturn = m_oDocKeyword.DirectAdd(vKeywordID:=vResultArray(0, I), vDocNum:=lDestDocNum, vUsername:=vResultArray(1, I), vCreateDate:=vResultArray(2, I))

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Next I

        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetFolderTree
    '
    ' Description: This is a rapper to the function in the miscellaneous
    ' class.
    '
    ' ***************************************************************** '
    Public Function GetFolderTree(ByRef lFolderNum As Integer, ByRef vFolderArray(,) As Object) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get the supplied folder name
            m_lReturn = CType(m_oMiscellaneous.GetFolderTree(lFolderNum, vFolderArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetFolderTree", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetNodeParent
    '
    ' Description: This is a rapper to the function in the miscellaneous
    ' class.
    '
    ' ***************************************************************** '
    Public Function GetNodeParent(ByRef iNodeType As Integer, ByRef lNodeNum As Integer, ByRef lParentNum As Integer) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get the supplied folder name
            m_lReturn = CType(m_oMiscellaneous.GetNodeParent(iNodeType:=iNodeType, lNodeNum:=lNodeNum, lParentNum:=lParentNum), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetNodeParent", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetFullFolderTree
    '
    ' Description: Given a specific node number this function returns
    ' the parent folder ancestry as follows :
    '
    '       vFolderArray(0,Ubound(vFolderArray)) = root folder num
    '       vFolderArray(1,Ubound(vFolderArray)) = root folder name
    '       vFolderArray(2,Ubound(vFolderArray)) = root folder password
    '       vFolderArray(3,Ubound(vFolderArray)) = root folder create date
    '
    ' ***************************************************************** '
    Public Function GetFullFolderTree(ByRef lNodeNum As Integer, ByRef iNodeType As Integer, ByRef vFolderArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lParentNum, lFolderNum As Integer
        Dim vResultArray(,) As Object


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If iNodeType = DOCNode_Document Then

                'get documents parent
                m_lReturn = CType(GetNodeParent(DOCNode_Document, lNodeNum, lFolderNum), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else
                lFolderNum = lNodeNum
            End If


            'Get the parent folders parent, name, password and create date
            m_sSQL = "SELECT parent_num, folder_name, password, create_date " &
                     "FROM DOC_folder WHERE folder_num = " & CStr(lFolderNum)

            'Hit the DB
            m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="GETFOLDERDETAILS", lNumberRecords:=1, bStoredProcedure:=False, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'if parent folder is 0, then it is deleted - just exit ok
            If lFolderNum = 0 Then
                Return result
            End If

            'Set up first line of array
            ReDim vFolderArray(3, 0)

            vFolderArray(0, 0) = lFolderNum


            vFolderArray(1, 0) = vResultArray(1, 0)


            vFolderArray(2, 0) = vResultArray(2, 0)


            vFolderArray(3, 0) = vResultArray(3, 0)


            lParentNum = CInt(vResultArray(0, 0))

            'loop until we have the root folder
            While lParentNum <> 0

                lFolderNum = lParentNum

                'Get the  folders parent, name, password and create date
                m_sSQL = "SELECT parent_num, folder_name, password, create_date " &
                         "FROM DOC_folder WHERE folder_num = " & CStr(lFolderNum)

                'Hit the DB
                m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="GETFOLDERDETAILS", lNumberRecords:=1, bStoredProcedure:=False, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Set up next line of array
                ReDim Preserve vFolderArray(3, vFolderArray.GetUpperBound(1) + 1)

                vFolderArray(0, vFolderArray.GetUpperBound(1)) = lFolderNum


                vFolderArray(1, vFolderArray.GetUpperBound(1)) = vResultArray(1, 0)


                vFolderArray(2, vFolderArray.GetUpperBound(1)) = vResultArray(2, 0)


                vFolderArray(3, vFolderArray.GetUpperBound(1)) = vResultArray(3, 0)


                lParentNum = CInt(vResultArray(0, 0))

            End While


            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetFullFolderTree", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: MoveEachDoc
    '
    ' Description: Moves a dcoument to a new folder, and updates
    ' the history table according to whether ex_codes for the source
    ' and destination folders are passed.
    '
    ' ***************************************************************** '
    Private Function MoveEachDoc(ByRef lDestFolder As Integer, ByRef vDocArray(,) As Object, Optional ByRef vSourceExCodes As Object = Nothing, Optional ByRef vDestExCodes As Object = Nothing, Optional ByRef vPastedDocsArray(,) As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim sDocName, sEventType As String
        Dim dDocDate As Date
        Dim sDocRef, sPageType, sTmp, sPageName As String
        Dim lLink, lTmp As Integer




        result = gPMConstants.PMEReturnCode.PMTrue

        'Go thru list of documents being moved, updating the parent
        ReDim vPastedDocsArray(0, vDocArray.GetUpperBound(1))
        For I As Integer = vDocArray.GetLowerBound(1) To vDocArray.GetUpperBound(1)

            Application.DoEvents()

            m_sSQL = "UPDATE DOC_document SET folder_num = " & lDestFolder

            m_sSQL = m_sSQL & " WHERE doc_num = " & CStr(vDocArray(0, I))

            'slap the database
            m_lReturn = CType(m_oDatabase.SQLAction(sSQL:=m_sSQL, sSQLName:="MOVEDOCS", bStoredProcedure:=False), gPMConstants.PMEReturnCode)

            vPastedDocsArray(0, I) = vDocArray(0, I)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'is source folder external, version 2 folder?

            If Not Object.Equals(vSourceExCodes, Nothing) Then
                'source folder was external, so need to delete these
                'docs from history database

                'get required info first

                m_lReturn = CType(m_oMiscellaneous.GetDocDate(lDocNum:=CInt(vDocArray(0, I)), dDocDate:=dDocDate), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = CType(m_oMiscellaneous.ConstructDocRef(CInt(vDocArray(0, I)), sDocRef), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'hit the history table
                m_lReturn = m_oHistory.DirectAdd(vTask:=DOCDELDOCUMENT, vCabinetcode:=vSourceExCodes(2), vCabinetname:="", vDrawercode:=vSourceExCodes(1), vDrawername:="", vFoldercode:=vSourceExCodes(0), vFoldername:="", vDocref:=sDocRef, vRequestDate:=dDocDate.ToString("yyyyMMdd"), vRequestTime:=dDocDate.ToString("HHMMss"))

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If


            'is destination folder external, version 2 folder?

            If Not Object.Equals(vDestExCodes, Nothing) Then
                'destination folder was external, so need to add these
                'docs to history database

                'get additonal info required first

                'get doc date

                m_lReturn = CType(m_oMiscellaneous.GetDocDate(lDocNum:=CInt(vDocArray(0, I)), dDocDate:=dDocDate), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'get other doc details

                m_lReturn = CType(m_oMiscellaneous.GetDocDetails(lDocNum:=CInt(vDocArray(0, I)), sDocName:=sDocName, sDocType:=sEventType), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = CType(m_oMiscellaneous.ConstructDocRef(CInt(vDocArray(0, I)), sDocRef), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'get the page file    (first check for links though)

                m_lReturn = CType(m_oMiscellaneous.GetDocLink(CInt(vDocArray(0, I)), lLink), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If lLink <> 0 Then
                    lTmp = lLink
                Else

                    lTmp = CInt(vDocArray(0, I))
                End If

                m_lReturn = CType(m_oMiscellaneous.GetPageName(lDocNum:=lTmp, sPageName:=sPageName), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'remove the obliques and soliduses
                m_lReturn = CType(StripSlashes(sPageName, sTmp), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'get the page type
                m_lReturn = CType(m_oMiscellaneous.GetPageType(lDocNum:=lTmp, sPageType:=sPageType), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'hit the history table
                m_lReturn = m_oHistory.DirectAdd(vTask:=DOCADDDOCUMENT, vCabinetcode:=vDestExCodes(2), vCabinetname:="", vDrawercode:=vDestExCodes(1), vDrawername:="", vFoldercode:=vDestExCodes(0), vFoldername:="", vDocref:=sDocRef, vRequestDate:=dDocDate.ToString("yyyyMMdd"), vRequestTime:=dDocDate.ToString("HHMMss"), vEventtype:=sEventType, vDescription:=sDocName, vVolume:=DOCHD1_NAME, vPagefile:=sTmp, vDoctype:=sPageType)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

        Next I

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AmIExternal
    '
    ' Description: Wrapper to miscellaneous.
    '
    ' ***************************************************************** '
    Public Function AmIExternal(ByRef iNodeType As Integer, ByRef lNodeNum As Integer, ByRef bExternal As Boolean) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(m_oMiscellaneous.AmIExternal(iNodeType:=iNodeType, lNodeNum:=lNodeNum, bExternal:=bExternal), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="AmIExternal", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: CopyEachDoc
    '
    ' Description: Given a list of doc nums and a destination folder
    ' this procedure copies  all the documents to the destination.
    '
    ' If the destination folder is an external version 2 folder, then the
    ' destination ex codes are optionally passed such that the history can
    ' be updated.
    '
    ' ***************************************************************** '
    Private Function CopyEachDoc(ByRef lDestFolder As Integer, ByRef vDocArray(,) As Object, Optional ByRef vDestExCodes As Object = Nothing, Optional ByRef vPastedDocsArray(,) As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lNewDocNum, lLink As Integer
        Dim vResultArray(,) As Object
        Dim sDocRef As String = ""
        Dim dDocDate As Date
        Dim sEventType, sDocName, sTmp, sPageName, sPageType As String



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Go thru list of documents being copied, writing a new document for
        ' each
        ReDim vPastedDocsArray(0, vDocArray.GetUpperBound(1))
        For I As Integer = vDocArray.GetLowerBound(1) To vDocArray.GetUpperBound(1)

            Application.DoEvents()

                'First get details of and write the new document record

                'Construct SQL

                m_sSQL = "SELECT doc_name, access_level, doc_type, " &
                         "password, link, zipped FROM DOC_document WHERE doc_num = " & CStr(vDocArray(0, I))

                'slap the database
                m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="COPYDOCS", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Write the new document record, linking to the original
                If Information.IsArray(vResultArray) Then

                    'check if source already linked to a another doc

                    If CDbl(vResultArray(4, 0)) = 0 Then
                        'Link to source doc.

                        lLink = CInt(vDocArray(0, I))
                    Else
                        'Link to source doc's link.

                        lLink = CInt(vResultArray(4, 0))
                    End If

                    'Write the doc to DB. We do not want to copy the external code.
                    m_lReturn = m_oDocument.DirectAdd(vDocNum:=lNewDocNum, vFolderNum:=lDestFolder, vDocName:=vResultArray(0, 0), vDocType:=vResultArray(2, 0), vAccessLevel:=vResultArray(1, 0), vPassword:=vResultArray(3, 0), vZipped:=vResultArray(5, 0), vLink:=lLink)

                vPastedDocsArray(0, I) = lNewDocNum

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'save some stuff for later writing to history

                sDocName = CStr(vResultArray(0, 0)).Trim()

                    sEventType = CStr(vResultArray(2, 0)).Trim()

                End If


                'Now get details of and write the new document info record

                'Construct SQL

                m_sSQL = "SELECT expiry_date, scan_user, doc_date, last_user, last_date " &
                         "FROM DOC_doc_info WHERE doc_num = " & CStr(vDocArray(0, I))

                'slap the database
                m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="COPYDOCINFO", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Write the new doc info record
                If Information.IsArray(vResultArray) Then

                    'Write the doc info to DB.
                    m_lReturn = m_oDocInfo.DirectAdd(vDocNum:=lNewDocNum, vExpiryDate:=vResultArray(0, 0), vScanUser:=vResultArray(1, 0), vDocDate:=vResultArray(2, 0), vLastUser:=vResultArray(3, 0), vLastDate:=vResultArray(4, 0))

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'save date for writing to history

                    dDocDate = CDate(vResultArray(2, 0))

                End If

                'DocuMasterV2 works like this - when a doc copied, copy current keys and anns,
                'after that they are separate - the link is used just for pages

                'Copy the annotations across for this document to the new one.

                m_lReturn = CType(CopyAnnotations(CInt(vDocArray(0, I)), lNewDocNum), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Copy the keywords across for this document to the new one.

                m_lReturn = CType(CopyKeywords(CInt(vDocArray(0, I)), lNewDocNum), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Update the history table now, depending on whether destination
                'folders ex codes passed. Need to get some udder details first, though.

                If Not Information.IsNothing(vDestExCodes) Then

                    'get the doc reference for history database
                    m_lReturn = CType(m_oMiscellaneous.ConstructDocRef(lNewDocNum, sDocRef), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'get the page file
                    m_lReturn = CType(m_oMiscellaneous.GetPageName(lDocNum:=lLink, sPageName:=sPageName), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'remove the obliques and soliduses
                    m_lReturn = CType(StripSlashes(sPageName, sTmp), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'get the page type
                    m_lReturn = CType(m_oMiscellaneous.GetPageType(lDocNum:=lLink, sPageType:=sPageType), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'now add record to history table
                    m_lReturn = m_oHistory.DirectAdd(vTask:=DOCADDDOCUMENT, vCabinetcode:=vDestExCodes(2), vCabinetname:="", vDrawercode:=vDestExCodes(1), vDrawername:="", vFoldercode:=vDestExCodes(0), vFoldername:="", vDocref:=sDocRef, vRequestDate:=dDocDate.ToString("yyyyMMdd"), vRequestTime:=dDocDate.ToString("HHMMss"), vEventtype:=sEventType, vDescription:=sDocName, vVolume:=DOCHD1_NAME, vPagefile:=sTmp, vDoctype:=sPageType)


                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

            Next I

            Return result

    End Function
    ' ***************************************************************** '
    ' Name: CopyDocs
    '
    ' Description: Given a list of doc nums and a destination folder
    ' this procedure calls the function to copy all the documents to
    ' the destination.
    '
    ' ***************************************************************** '
    Public Function CopyDocs(ByRef lDestFolder As Integer, ByRef vDocArray(,) As Object, Optional ByRef vPastedDocs(,) As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim iFoldLevel As Integer
        Dim vExCodes As Object
        Dim bUpdateHistory As Boolean
        Dim lTmp, lParentNum As Integer
        Dim sExCode As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'First check if destination is external version 2 folder, in which
            'case we will want to update the history database at a later stage
            'so get the external codes now for efficency
            m_lReturn = CType(m_oMiscellaneous.GetNodeExCode(iNodeType:=DOCNode_Folder, lNodeNum:=lDestFolder, sExCode:=sExCode, iFolderLevel:=iFoldLevel), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If (sExCode <> "") And (iFoldLevel = DOCFolder) Then

                'its a version 2 folder, so get ex codes of draw and cabinet
                bUpdateHistory = True

                'save folder ex code
                ReDim vExCodes(2)

                vExCodes(0) = sExCode

                m_lReturn = CType(GetNodeParent(iNodeType:=DOCNode_Folder, lNodeNum:=lDestFolder, lParentNum:=lParentNum), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = CType(m_oMiscellaneous.GetNodeExCode(iNodeType:=DOCNode_Folder, lNodeNum:=lParentNum, sExCode:=sExCode), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'save drawer ex code

                vExCodes(1) = sExCode

                m_lReturn = CType(GetNodeParent(iNodeType:=DOCNode_Folder, lNodeNum:=lParentNum, lParentNum:=lTmp), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = CType(m_oMiscellaneous.GetNodeExCode(iNodeType:=DOCNode_Folder, lNodeNum:=lTmp, sExCode:=sExCode), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'save cabinet ex code

                vExCodes(2) = sExCode

            End If

            'Begin a set of DB transactions
            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyDocs", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            ' Do the actual document copying
            If bUpdateHistory Then
                m_lReturn = CType(CopyEachDoc(lDestFolder:=lDestFolder, vDocArray:=vDocArray, vDestExCodes:=vExCodes, vPastedDocsArray:=vPastedDocs), gPMConstants.PMEReturnCode)
            Else
                m_lReturn = CType(CopyEachDoc(lDestFolder, vDocArray, vPastedDocsArray:=vPastedDocs), gPMConstants.PMEReturnCode)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                'problem, so roll back.
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            'Everything fine so commit them to Hell.
            m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="CommitTrans Failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyDocs", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="CopyDocs", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AmIV2Folder
    '
    ' Description: Given a folder number, this function determines
    ' whether its a version2 folder, using the folder level.
    '
    ' (ie a 'folder' in the cabinet/drawer/folder/document terminology)
    '
    ' ***************************************************************** '
    Public Function AmIV2Folder(ByRef lFolderNum As Integer, ByRef bV2Folder As Boolean) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Construct SQL
            m_sSQL = "SELECT folder_level FROM DOC_folder WHERE folder_num = " & lFolderNum

            m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="AMIV2FOLDER", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If Object.Equals(vResultArray, Nothing) Then
                'this is no good
                result = gPMConstants.PMEReturnCode.PMFalse

                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Folder number " & lFolderNum & " does not exist", vApp:=ACApp, vClass:=ACClass, vMethod:="AmIV2Folder", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            Else


                bV2Folder = (CInt(vResultArray(0, 0)) = DOCFolder)

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="AmIV2Folder", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function


    ' ***************************************************************** '
    ' Name: CopyFoldersRecursively
    '
    ' Description: Given a list of folder nums and a destination folder
    ' this procedure copies  all the folders to the destination. It is a
    ' recursive procedure so each child is copied over too
    '
    ' ***************************************************************** '
    Private Function CopyFoldersRecursively(ByRef lDestFolder As Integer, ByRef vFolderArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object
        Dim lNewFolderNum As Integer




        result = gPMConstants.PMEReturnCode.PMTrue


        ' Go thru list of folders being copied, writing a new folder for each
        For I As Integer = vFolderArray.GetLowerBound(1) To vFolderArray.GetUpperBound(1)

            Application.DoEvents()

            'Write the new folder record. We do not want to copy the external code.
            m_lReturn = m_oFolder.DirectAdd(vFolderNum:=lNewFolderNum, vFolderName:=vFolderArray(1, I), vParentNum:=lDestFolder, vAccessLevel:=vFolderArray(2, I), vPassword:=vFolderArray(3, I))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the child folders for folder just copied

            'Construct SQL

            m_sSQL = "SELECT folder_num, folder_name, access_level, password " &
                     "FROM DOC_folder WHERE parent_num = " & CStr(vFolderArray(0, I))

            'Hit the DB
            m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="COPYFOLDERS", bStoredProcedure:=False, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Copy the children recursively
            If Information.IsArray(vResultArray) Then

                m_lReturn = CType(CopyFoldersRecursively(lNewFolderNum, vResultArray), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'Now transfer the child docs

            'Get array of child docs

            m_sSQL = "SELECT doc_num FROM DOC_document WHERE folder_num = " & CStr(vFolderArray(0, I))

            'Hit DB
            m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="GETDOCS", bStoredProcedure:=False, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Now use the CopyDocs method to copy them to the new folder
            If Information.IsArray(vResultArray) Then


                m_lReturn = CType(CopyEachDoc(lNewFolderNum, vResultArray), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

        Next I


        Return result

    End Function
    ' ***************************************************************** '
    ' Name: CopyFolders
    '
    ' Description: Given a list of folder nums and a destination folder
    ' this procedure copies  all the folders to the destination.
    '
    ' ***************************************************************** '
    Public Function CopyFolders(ByRef lDestFolder As Integer, ByRef vFolderArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Begin a set of DB transactions
            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyFolders", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            ' Go thru list of folders, recursively copying them
            For I As Integer = vFolderArray.GetLowerBound(1) To vFolderArray.GetUpperBound(1)

                Application.DoEvents()

                'Construct SQL

                m_sSQL = "SELECT folder_num, folder_name, access_level, password " &
                         "FROM DOC_folder WHERE folder_num = " & CStr(vFolderArray(0, I))

                'Hit the DB
                m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="GETFOLDERLIST", bStoredProcedure:=False, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                    Return result
                End If

                'Copy folder recursively
                If Information.IsArray(vResultArray) Then

                    m_lReturn = CType(CopyFoldersRecursively(lDestFolder, vResultArray), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                        Return result
                    End If
                End If

            Next I

            'Everything fine so commit them to Hell.
            m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyFolders", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="CopyFolders", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    ' Name: MoveDocs
    '
    ' Description: Given a list of document nums and a destination folder
    ' this procedure moves all the documents to the destination.
    '
    ' First though it checks if the source and destination folders are
    ' external version 2 folders, and gets the ex_codes for later
    ' history table updates.
    '
    ' ***************************************************************** '
    Public Function MoveDocs(ByRef lDestFolder As Integer, ByRef vDocArray(,) As Object, Optional ByRef vPastedDocs(,) As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim iFoldLevel As Integer
        Dim vDestExCodes As Object
        Dim lSourceFolder As Integer
        Dim vSourceExCodes As Object
        Dim lTmp, lParentNum As Integer
        Dim sExCode As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            'First check if destination is external version 2 folder, as when we
            'move the docs in we will want to update the history table
            m_lReturn = CType(m_oMiscellaneous.GetNodeExCode(iNodeType:=DOCNode_Folder, lNodeNum:=lDestFolder, sExCode:=sExCode, iFolderLevel:=iFoldLevel), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If (sExCode <> "") And (iFoldLevel = DOCFolder) Then

                'it is external, and its an old version 2 folder

                'save folder ex code
                ReDim vDestExCodes(2)

                vDestExCodes(0) = sExCode

                m_lReturn = CType(GetNodeParent(iNodeType:=DOCNode_Folder, lNodeNum:=lDestFolder, lParentNum:=lParentNum), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = CType(m_oMiscellaneous.GetNodeExCode(iNodeType:=DOCNode_Folder, lNodeNum:=lParentNum, sExCode:=sExCode), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'save drawer ex code

                vDestExCodes(1) = sExCode

                m_lReturn = CType(GetNodeParent(iNodeType:=DOCNode_Folder, lNodeNum:=lParentNum, lParentNum:=lTmp), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = CType(m_oMiscellaneous.GetNodeExCode(iNodeType:=DOCNode_Folder, lNodeNum:=lTmp, sExCode:=sExCode), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'save cabinet ex code

                vDestExCodes(2) = sExCode

            End If


            'Now check if source folder is external version 2 folder, as when we
            'move the docs out we will want to update the history table

            'get node num of source folder - ie parent of any doc to be moved

            m_lReturn = CType(m_oMiscellaneous.GetNodeParent(iNodeType:=DOCNode_Document, lNodeNum:=CInt(vDocArray(0, 0)), lParentNum:=lSourceFolder), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'get ex code of source folder
            m_lReturn = CType(m_oMiscellaneous.GetNodeExCode(iNodeType:=DOCNode_Folder, lNodeNum:=lSourceFolder, sExCode:=sExCode, iFolderLevel:=iFoldLevel), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If (sExCode <> "") And (iFoldLevel = DOCFolder) Then

                'its a version 2 folder, so get ex codes of draw and cabinet

                'save folder ex code
                ReDim vSourceExCodes(2)

                vSourceExCodes(0) = sExCode

                m_lReturn = CType(GetNodeParent(iNodeType:=DOCNode_Folder, lNodeNum:=lSourceFolder, lParentNum:=lParentNum), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = CType(m_oMiscellaneous.GetNodeExCode(iNodeType:=DOCNode_Folder, lNodeNum:=lParentNum, sExCode:=sExCode), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'save drawer ex code

                vSourceExCodes(1) = sExCode

                m_lReturn = CType(GetNodeParent(iNodeType:=DOCNode_Folder, lNodeNum:=lParentNum, lParentNum:=lTmp), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = CType(m_oMiscellaneous.GetNodeExCode(iNodeType:=DOCNode_Folder, lNodeNum:=lTmp, sExCode:=sExCode), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'save cabinet ex code

                vSourceExCodes(2) = sExCode

            End If


            'Begin a set of DB transactions
            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MoveDocs", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result

            End If

            'call function that actually does the work

            m_lReturn = CType(MoveEachDoc(lDestFolder:=lDestFolder, vDocArray:=vDocArray, vSourceExCodes:=vSourceExCodes, vDestExCodes:=vDestExCodes, vPastedDocsArray:=vPastedDocs), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Everything fine so commit
            m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MoveDocs", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="MoveDocs", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    ' Name: MoveFolders
    '
    ' Description: Given a list of folder nums and a destination folder
    ' this procedure moves all the folders to the destination.
    '
    ' ***************************************************************** '
    Public Function MoveFolders(ByRef lDestFolder As Integer, ByRef vFolderArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Begin a set of DB transactions
            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MoveFolders", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            'Go thru list of folders being moved, updating the parent
            For I As Integer = vFolderArray.GetLowerBound(1) To vFolderArray.GetUpperBound(1)

                Application.DoEvents()

                m_sSQL = "UPDATE DOC_folder SET parent_num = " & lDestFolder

                m_sSQL = m_sSQL & " WHERE folder_num = " & CStr(vFolderArray(0, I))

                'slap the database
                m_lReturn = CType(m_oDatabase.SQLAction(sSQL:=m_sSQL, sSQLName:="MOVEFOLDERS", bStoredProcedure:=False), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                    Return result
                End If

            Next I

            'Everything fine so commit
            m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MoveFolders", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="MoveFolders", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteFolders
    '
    ' Description: This function loops thru array of folder nums and
    ' calls the miscellaneous class function to delete them.
    '
    ' ***************************************************************** '
    Public Function DeleteFolders(ByRef vFolderArray As Object, ByRef sNoAccessName As String) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Begin a set of DB transactions
            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteFolders", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If


            For I As Integer = vFolderArray.GetLowerBound(1) To vFolderArray.GetUpperBound(1)

                Application.DoEvents()

                'call proper function

                m_lReturn = CType(m_oMiscellaneous.DeleteFolder(lFolderNum:=CInt(vFolderArray(0, I)), sNoAccessName:=sNoAccessName), gPMConstants.PMEReturnCode)


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                    Return result
                End If

            Next I


            'Everything fine so commit them.
            m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="CommitTrans Failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteFolders", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteFolders", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: DeleteDocuments
    '
    ' Description: This function loops thru array of doc nums and
    ' calls the miscellaneous class function to delete them.
    '
    ' ***************************************************************** '
    Public Function DeleteDocuments(ByRef vDocArray(,) As Object, ByRef bExternal As Boolean, ByRef sNoAccessName As String) As Integer

        Dim result As Integer = 0


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Begin a set of DB transactions
            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteDocuments", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result

            End If

            'loop thru each doc

            For I As Integer = vDocArray.GetLowerBound(1) To vDocArray.GetUpperBound(1)

                Application.DoEvents()

                'call proper function

                m_lReturn = CType(m_oMiscellaneous.DeleteDoc(lDocNum:=CInt(vDocArray(0, I)), bExternal:=bExternal, sNoAccessName:=sNoAccessName), gPMConstants.PMEReturnCode)


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                    Return result
                End If

            Next I


            'Everything fine so commit them.
            m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="CommitTrans Failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteDocuments", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteDocuments", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RenameFolder
    '
    ' Description: This is essentially a wrapper to the misc method
    ' that actually renames a folder.
    '
    ' ***************************************************************** '
    Public Function RenameFolder(ByRef lFoldNum As Integer, ByRef sNewName As String) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'call funtion that actually does it
            m_lReturn = CType(m_oMiscellaneous.RenameNode(iNodeType:=DOCNode_Folder, lNodeNum:=lFoldNum, sNewNodeName:=sNewName), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="RenameFolder", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: NewFolder
    '
    ' Description: Given a parent folder, folder name and create date,
    ' this procedure adds the folder and returns its number.
    '
    ' ***************************************************************** '
    Public Function NewFolder(ByRef lParentNum As Integer, ByRef sFolderName As String, ByRef dCreateDate As Date, ByRef lFolderNum As Integer) As Integer


        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oFolder.DirectAdd(vFolderNum:=lFolderNum, vFolderName:=sFolderName, vParentNum:=lParentNum, vCreateDate:=dCreateDate)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="NewFolder", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RenameDoc
    '
    ' Description: This is a wrapper to the misc method.
    '
    ' ***************************************************************** '
    Public Function RenameDoc(ByRef lDocNum As Integer, ByRef sNewName As String) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Begin a set of DB transactions
            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenameDoc", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            'call function that actually does it
            m_lReturn = CType(m_oMiscellaneous.RenameDoc(lDocNum:=lDocNum, sNewName:=sNewName), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return result
            End If

            'Everything fine so commit
            m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenameDoc", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="RenameDoc", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetUserAccessLevel
    '
    ' Description: This gets the users access level from the user table.
    ' If not there, then we have a new user, so add them in with a default
    ' access level of 9. If present, check if the name still is the same.
    ' If it isn't, assume this is a new user, re-using a UserID so change
    ' the name accordingly and reset access level to 9.
    '
    ' ***************************************************************** '
    Private Function GetUserAccessLevel() As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object




        result = gPMConstants.PMEReturnCode.PMTrue

        m_sSQL = "SELECT access_level, user_name FROM DOC_doc_user WHERE user_id = " &
                 g_iUserID

        'Hit the DB
        m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="GETUSERLEVEL", lNumberRecords:=1, bStoredProcedure:=False, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'did we get anything back?
        If Information.IsArray(vResultArray) Then

            'We have it, so check if the user name is correct

            If g_sUsername.Trim() = CStr(vResultArray(1, 0)).Trim() Then

                'still the same, so set access level and go

                g_iAccessLevel = CInt(vResultArray(0, 0))
                Return result
            Else

                'different user name, so treat as new user
                m_sSQL = "UPDATE DOC_doc_user SET user_name = '" & g_sUsername &
                         "', access_level = 9, home_folder_num = 0, " &
                         "retired = 'N' WHERE user_id = " & CStr(g_iUserID)

                'Hit the DB
                m_lReturn = CType(m_oDatabase.SQLAction(sSQL:=m_sSQL, sSQLName:="ADDUSER", bStoredProcedure:=False), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                g_iAccessLevel = 9

            End If

        Else

            m_lReturn = CType(ValidateSQL(sSQLStatement:=g_sUsername), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'User not found so add 'em in with lowest access level.
            'm_sSQL$ = "{spu_DOC_Add_doc_user (" & g_iUserID% & ", 9, 0, 'N')}"
            m_sSQL = "INSERT INTO DOC_doc_user (user_id, user_name, access_level, " &
                     "home_folder_num, retired) VALUES (" & CStr(g_iUserID) & ", '" &
                     g_sUsername.Trim() & "' , 9, 0, 'N')"

            'Hit the DB
            m_lReturn = CType(m_oDatabase.SQLAction(sSQL:=m_sSQL, sSQLName:="ADDUSER", bStoredProcedure:=False), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            g_iAccessLevel = 9

        End If


        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetHomeFolder
    '
    ' Description: This gets the users home folder number and stores
    ' it in the property variable.
    '
    ' ***************************************************************** '
    Private Function GetHomeFolder() As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object




        result = gPMConstants.PMEReturnCode.PMTrue

        m_sSQL = "SELECT home_folder_num FROM DOC_doc_user WHERE user_id = " &
                 g_iUserID

        'Hit the DB
        m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="GETHOMEFOLDER", lNumberRecords:=1, bStoredProcedure:=False, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'did we get anything back?
        If Information.IsArray(vResultArray) Then

            g_lHomeFolder = CInt(vResultArray(0, 0))
        Else
            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get home folder num for userID = " & g_iUserID, vApp:=ACApp, vClass:=ACClass, vMethod:="GetHomeFolder", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            g_lHomeFolder = 0
        End If


        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetAdminLevel
    '
    ' Description: Wrapper to function proper in miscellaneous.
    '
    ' ***************************************************************** '
    Private Function GetAdminLevel() As Integer

        Dim result As Integer = 0
        Dim iAdminLevel As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        'call main function
        m_lReturn = CType(m_oMiscellaneous.GetAdminLevel(iAdminLevel), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        g_iAdminLevel = iAdminLevel

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: ImportDocument
    '
    ' Description: Save chosen file into DocuMaster system.
    '
    ' ***************************************************************** '
    Public Function ImportDocument(ByRef sDocName As String, ByRef sPageType As String, ByRef lFoldNum As Integer, ByRef lPageSize As Integer, ByRef sTmpPageName As String, ByRef lDocNum As Integer, ByRef dDocDate As Date, ByRef sZipped As String) As Integer

        Dim result As Integer = 0
        Dim sDocType, sNextPageName, sDataPath, sNewPageName As String


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Begin a set of DB transactions
            'had problems with pmdao transactions
            'removed 18 8 99
            '    m_lReturn& = BeginTrans()
            '
            '    If (m_lReturn <> PMTrue) Then
            '
            '        ImportDocument = PMFalse
            '        iPMFunc.LogMessage sUsername:="", _
            ''            iType:=PMLogError, _
            ''            sMsg:="BeginTrans Failed", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="ImportDocument", _
            ''            vErrNo:=Err.Number, _
            ''            vErrDesc:=Err.Description
            '
            '        Exit Function
            '    End If

            'SOB050599 Added Doc, Html doc types and changed the else from producing
            'an error to setting an unknown so that you can import any document type
            'get doc type
            '        Case "TXT"
            '            sDocType$ = "T"
            '
            '        Case "RTF"
            '            sDocType$ = "W"
            '
            '        Case "TIF"
            '            sDocType$ = "I"
            '
            '        Case "DOC"
            '            sDocType$ = "D" 'Ms Word Docs
            '
            '        Case "HTM", "HTML" 'HTML
            '            sDocType$ = "H"
            '
            '        Case Else
            '            sDocType$ = "U" 'Unknown document type

            Select Case sPageType.ToUpper()
                Case "TIF", "TIFF"
                    sDocType = kDocFileTypeTIF
                Case "RTF"
                    sDocType = kDocFileTypeRTF
                Case "BMP"
                    sDocType = kDocFileTypeBMP
                Case "TXT", "TEXT", "ASCI"
                    sDocType = kDocFileTypeTXT
                Case "DOC", "DOCX", "DOT", "DOTX", "ASC", "ANS", "MCW", "WPS"  'SOB 01/06/99 WORD FILES
                    sDocType = kDocFileTypeWRD
                Case "XLS", "XLSX", "XLT", "XLS", "CSV", "WK1", "WK2", "WK3", "WK4", "WQ1", "PRN", "DIF", "SLK", "XLA", "TAB"  'SOB 01/06/99 EXCEL Files
                    sDocType = kDocFileTypeEXL
                Case "PPT", "PPTX", "POT", "POTX", "PPS", "PPSX", "PPA"  'SOB 01/06/99 Power Point Files
                    sDocType = kDocFileTypePWP
                Case "MDB", "ADP", "MDW", "MDA", "MDE", "ADE", "DBF", "DB"  'SOB 01/06/99 Ms Access Files
                    sDocType = kDocFileTypeACC
                Case "HTM", "HTML", "SHTM", "SHTML", "STM", "ASP", "HTT", "CSS", "CFML", "XML"  'SOB 01/06/99 IE, Netscape Files
                    sDocType = kDocFileTypeHTM
                Case "GIF", "GIFF"
                    sDocType = kDocFileTypeGIF  'SOB 01/06/99 GIF Files
                Case "JPEG", "JPG"
                    sDocType = kDocFileTypeJPG
                Case "EML", "OFT", "MSG", "EML"  'SOB 01/06/99 E-Mail Doc
                    sDocType = kDocFileTypeEML
                Case "PDF"
                    sDocType = kDocFileTypePDF  'SOB 01/06/99 Adobe Accrobat Files
                Case "HLP"
                    sDocType = kDocFileTypeHLP  'SOB 01/06/99 Help Files
                Case "ZIP", "GZ"
                    sDocType = kDocFileTypeZIP  'SOB 01/06/99 ZIP Files
                Case Else
                    sDocType = kDocFileTypeUnknown
            End Select

            'Write the document record
            m_lReturn = m_oDocument.DirectAdd(vDocNum:=lDocNum, vFolderNum:=lFoldNum, vDocName:=sDocName, vDocType:=sDocType, vZipped:=sZipped, vCreateDate:=dDocDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Document :- Direct Add Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ImportDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return result
            End If

            'Write the doc info record
            m_lReturn = m_oDocInfo.DirectAdd(vDocNum:=lDocNum, vExpiryDate:=dDocDate, vScanUser:=g_sUsername, vDocDate:=dDocDate, vLastUser:=g_sUsername, vLastDate:=dDocDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Doc Info :- Direct Add Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ImportDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return result
            End If

            'get next page name from main db
            m_lReturn = CType(m_oMiscellaneous.GetNextPageName(sNextPageName), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Misc :- Get Next Page Name Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ImportDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return result
            End If

            'now write to main database
            m_lReturn = m_oPage.DirectAdd(vPageName:=sNextPageName, vDocNum:=lDocNum, vPageNum:=1, vPageType:=sPageType, vPageSize:=lPageSize, vCreateDate:=dDocDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Page Direct Add Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ImportDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return result
            End If

            'Add record to history table if required
            m_lReturn = CType(m_oMiscellaneous.AddDocToHistory(lDocNum:=lDocNum, lFoldNum:=lFoldNum, sDocName:=sDocName, dDocDate:=dDocDate, sPageName:=sNextPageName, sDocType:=sDocType, sPageType:=sPageType), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Misc Add Doc to History Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ImportDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return result
            End If

            'get path to 00 tree, so we can construct the page path so the
            'interface can move it across
            m_lReturn = CType(m_oMiscellaneous.GetDataPath(lVolumeID:=DOCHD1_ID, sDataPath:=sDataPath), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Misc Get Data Path Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ImportDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return result
            End If

            sNewPageName = sDataPath & sNextPageName & "." & sPageType

            ' Create path for file copy
            m_lReturn = CType(MakePath(sNewPageName), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Make Path Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ImportDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return result
            End If

            Dim cloudHostingOptionValue As String = ""
            m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:="", v_sPassword:="", v_iUserID:=0, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=0, v_iCurrencyID:=0, v_iLogLevel:=0, v_sCallingAppName:=ACApp, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableCloudHosting, v_vBranch:=1, r_vUnderwriting:=cloudHostingOptionValue)

            If (gPMFunctions.NullToString(cloudHostingOptionValue) = "1") Then
                Dim s3NewPageName As String = Replace(sNextPageName & "." & sPageType, "\", "/").TrimStart("/")
                'move from temp location to 00 tree
                Dim repository As IS3Repository = New S3Repository(Environment.GetEnvironmentVariable("AWS_DME_BUCKET_NAME"),
                    Environment.GetEnvironmentVariable("AWS_REGION"),
                    g_sUsername)

                repository.UploadFile(s3NewPageName, File.ReadAllBytes(sTmpPageName))
                KillFile(sTmpPageName)
            Else
                FileSystem.Rename(sTmpPageName, sNewPageName)
            End If
            'had problems with pmdao transactions
            'sob removed 18 8 99
            'Everything fine so commit
            '    m_lReturn& = CommitTrans()
            '
            '    If (m_lReturn <> PMTrue) Then
            '
            '        ImportDocument = PMFalse
            '        iPMFunc.LogMessage sUsername:="", _
            ''            iType:=PMLogError, _
            ''            sMsg:="CommitTrans Failed", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="ImportDocument", _
            ''            vErrNo:=Err.Number, _
            ''            vErrDesc:=Err.Description
            '
            '        Exit Function
            '    End If

            Return result

        Catch excep As System.Exception



            m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="ImportDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDataPath
    '
    ' Description: Wrapper to function proper in miscellaneous.
    '
    ' ***************************************************************** '
    Public Function GetDataPath(ByRef lVolumeID As Integer, ByRef sDataPath As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            'call proper function
            m_lReturn = CType(m_oMiscellaneous.GetDataPath(lVolumeID:=lVolumeID, sDataPath:=sDataPath), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetDataPath", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetFolderInformation(ByRef lNodeNum As Integer, ByRef sExCode As String, ByRef iFolderLevel As Integer, ByRef iAccessLevel As Integer, ByRef sPassword As String, ByRef dCreateDate As Date) As Integer

        Dim result As Integer = 0
        Dim vResults(,) As Object

        result = gPMConstants.PMEReturnCode.PMTrue

        m_sSQL = "SELECT * FROM DOC_folder WHERE folder_num = " & lNodeNum

        m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="GETFOLDERINFORMATION", bStoredProcedure:=False, vResultArray:=vResults), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        sExCode = CStr(vResults(3, 0))

        iFolderLevel = CInt(vResults(4, 0))

        iAccessLevel = CInt(vResults(5, 0))

        sPassword = CStr(vResults(6, 0))

        dCreateDate = CDate(vResults(7, 0))


        Return result



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetFolderInformation", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function

    Public Function GetDocumentInformation(ByRef lNodeNum As Integer, ByRef lFolderNum As Integer, ByRef sExCode As String, ByRef sDocType As String, ByRef iAccessLevel As Integer, ByRef sPassword As String, ByRef dCreateDate As Date, ByRef lLink As Integer, ByRef sZipped As String, ByRef dExpiryDate As Date, ByRef sScanUser As String, ByRef dDocDate As Date, ByRef sLastUser As String, ByRef dLastDate As Date, ByRef vPageList As Object, ByRef iVolumeID As Integer) As Integer

        Dim result As Integer = 0
        Dim vResults(,) As Object


        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            m_sSQL = "SELECT doc_num, folder_num, doc_name, ex_code, doc_type, " &
                     "access_level, password, create_date, link, zipped FROM " &
                     "DOC_document WHERE doc_num = " & CStr(lNodeNum)

            m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="GETDOCUMENTINFORMATION", bStoredProcedure:=False, vResultArray:=vResults), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            lFolderNum = CInt(vResults(1, 0))

            sExCode = CStr(vResults(3, 0))

            sDocType = CStr(vResults(4, 0))

            iAccessLevel = CInt(vResults(5, 0))

            sPassword = CStr(vResults(6, 0))

            dCreateDate = CDate(vResults(7, 0))

            lLink = CInt(vResults(8, 0))

            sZipped = CStr(vResults(9, 0))

            m_sSQL = "SELECT * FROM DOC_doc_info WHERE doc_num = " & lNodeNum
            m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="GETDOCUMENTINFOINFORMATION", bStoredProcedure:=False, vResultArray:=vResults), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            dExpiryDate = CDate(vResults(1, 0))

            sScanUser = CStr(vResults(2, 0))

            dDocDate = CDate(vResults(3, 0))

            sLastUser = CStr(vResults(4, 0))

            dLastDate = CDate(vResults(5, 0))

            'check link
            If lLink = 0 Then


                m_lReturn = CType(m_oMiscellaneous.GetPageList(lDocNum:=lNodeNum, vPageArray:=vPageList), gPMConstants.PMEReturnCode)
            Else

                m_lReturn = CType(m_oMiscellaneous.GetPageList(lDocNum:=lLink, vPageArray:=vPageList), gPMConstants.PMEReturnCode)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            'get volume ID
            If lLink = 0 Then
                m_sSQL = "SELECT volume_ID FROM DOC_page WHERE doc_num = " & lNodeNum
            Else
                m_sSQL = "SELECT volume_ID FROM DOC_page WHERE doc_num = " & lLink
            End If



            m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="GETVOLID", bStoredProcedure:=False, vResultArray:=vResults), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            iVolumeID = CInt(vResults(0, 0))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocumentInformation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CountChildren
    '
    ' Description: Count the children folders of a given folder
    ' returns number which is then compared to MaxAutoExpand
    '
    '
    ' ***************************************************************** '
    Public Function CountChildren(ByRef lFolder_Num As Integer, ByRef lChildren As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            m_oDatabase.Parameters.Clear()
            'use select count function
            m_lReturn = m_oDatabase.Parameters.Add(sName:="parent_num", vValue:=lFolder_Num, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_sSQL = "spu_DOC_count_children"

            'Hit the DB
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="COUNTCHILDREN", bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            lChildren = CInt(vResultArray(0, 0))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="CountChildren", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetFolderValues
    '
    ' Description: get properties for specific folder
    '
    ' ***************************************************************** '
    Public Function GetFolderValues(ByRef lFolderNum As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_sSQL = "SELECT folder_num, folder_name, password, " &
                     "create_date FROM " &
                     "DOC_folder WHERE folder_num = " & CStr(lFolderNum)

            'Hit the DB
            m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="GETFOLDERVALUES", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetFolderValuesFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFolderValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: BriefcaseUser
    '
    ' Description: Function which either gets or sets the registry
    '              for the current Briefcase user
    '
    ' ***************************************************************** '

    Public Function BriefcaseUser(ByRef sMode As String, ByRef sUser As String) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If sMode = "GET" Then

                ' GET Briefcase user
                m_lReturn = CType(GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFDocumaster, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="BriefcaseUser", r_sSettingValue:=sUser), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If


            If sMode = "SET" Then

                ' Set Briefcase user
                m_lReturn = CType(SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFDocumaster, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="BriefcaseUser", v_sSettingValue:=sUser), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to read registry 'BriefcaseUser'", vApp:=ACApp, vClass:=ACClass, vMethod:="BriefcaseUser", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         ConnectToLocalDB
    '
    ' Description:  Function copies the PMBriefcase data & log files from the
    '               laptop PC to the Server PC. The Server copies the data across
    '               from the DME db to this local PMBriefcase db.
    '               The DisconnectLocalDB function copies the PMBriefcase db back to
    '               the laptop once the copy process is over.
    '
    ' ***************************************************************** '
    Public Function ConnectToLocalDB(ByRef sPCName As String) As Integer

        Dim result As Integer = 0
        Dim sSQL7Path, sMSDEPath As String
        Dim lDBSpace, lDBLogSpace As Integer
        Dim dDiskSpace As Double
        Dim lMinDiskSpace As Integer
        Dim sSQLServerName, sPassword As String
        Dim lProcessID As Integer
        Dim dAppID As Double
        Dim bFound As Boolean
        Dim oSQLServer As SQLDMO.SQLServer


        On Error GoTo Err_ConnectToLocalDB

        result = gPMConstants.PMEReturnCode.PMTrue

        oSQLServer = New SQLDMO.SQLServer()


        ' The MSDE (MSSQL7) dir on the laptop (SQLDataRoot registry). The dir must be shared as MSSQL7.
        sMSDEPath = "\\" & sPCName & "\MSSQL7\Data\"

        ' Get the Server PC's MSSQL7 (SQLServer7) data dir
        ' This gets the path of where SQL7 was installed i.e. "<Drive>:\MSSQL7"

        'Get the SQL Data path
        'sSQL7Path = ReadRegistry(HKEY_LOCAL_MACHINE, "SOFTWARE\Microsoft\MSSQLServer\MSSQLServer\Parameters", "SQLArg0")
        sSQL7Path = ReadRegistry(gpmConstants.HKEY_LOCAL_MACHINE, "SOFTWARE\Microsoft\MSSQLServer\MSSQLServer\Parameters", "SQLArg0")
        If sSQL7Path = "Not Found" Then
            sSQL7Path = ""
        End If

        If sSQL7Path.Substring(0, Math.Min(sSQL7Path.Length, 2)) = "-d" Then
            sSQL7Path = sSQL7Path.Substring(sSQL7Path.Length - (sSQL7Path.Length - 2))
        End If

        Do While sSQL7Path.Length > 0
            If sSQL7Path.EndsWith("\") Then
                Exit Do
            End If
            sSQL7Path = sSQL7Path.Substring(0, sSQL7Path.Length - 1)
        Loop


        ' Ensure there is enough disk space on briefcase drive before proceeding, need 6MB (4+2)

        lDBSpace = 4
        lDBLogSpace = 2

        lMinDiskSpace = lDBSpace + lDBLogSpace
        m_lReturn = CType(GetDiskSpace(sSQL7Path.Substring(0, 1), dDiskSpace), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        dDiskSpace = Math.Floor(dDiskSpace / (2 ^ 20))
        If dDiskSpace < lMinDiskSpace Then
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Not enough disk space on Server PC to create temp database. Free space and retry", vApp:=ACApp, vClass:=ACClass, vMethod:="ConnectToLocalDB", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        sSQLServerName = "(local)"
        sPassword = ""


Login_Resume:

        result = gPMConstants.PMEReturnCode.PMTrue


        ' Log onto the SQL Server. If service not started then this will run again
        oSQLServer.Connect(sSQLServerName, "sa", sPassword)


        ' if PMBriefcase does exists
        If IsThere(oSQLServer.Databases, m_cPMBriefcase) Then

            oSQLServer.DetachDB(m_cPMBriefcase, True)

            ' kill any files that already exist
            If FileSystem.Dir(sSQL7Path & m_cDataFile, FileAttribute.Normal) <> "" Then
                File.Delete(sSQL7Path & m_cDataFile)
            End If

            If FileSystem.Dir(sSQL7Path & m_cLogFile, FileAttribute.Normal) <> "" Then
                File.Delete(sSQL7Path & m_cLogFile)
            End If

        End If



        ' check if both  data and log files exist on local PC
        bFound = False
        If FileSystem.Dir(sMSDEPath & m_cDataFile, FileAttribute.Normal) <> "" And FileSystem.Dir(sMSDEPath & m_cLogFile, FileAttribute.Normal) <> "" Then
            bFound = True
        End If


        If Not bFound Then  ' if data or log file is missing
            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PMBriefcase Data and log files do not exist in : " & sMSDEPath, vApp:=ACApp, vClass:=ACClass, vMethod:="ConnectToLocalDB", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)


            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' both files exist so copy them across from briefcase to Server
        File.Copy(sMSDEPath & m_cDataFile, sSQL7Path & m_cDataFile)
        File.Copy(sMSDEPath & m_cLogFile, sSQL7Path & m_cLogFile)

        ' create Briefcase db on the Server by attaching files
        oSQLServer.AttachDB(m_cPMBriefcase, sSQL7Path & m_cDataFile & ", " &
                            sSQL7Path & m_cLogFile)


        'Create the Destination DSN Object for Briefcase
        m_oDestDSN = New DmeDSN()

        m_lReturn = CType(m_oDestDSN.Initialise(m_cTempDME), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="The '" & m_cTempDME & "' DSN could not be initialised.", vApp:=ACApp, vClass:=ACClass, vMethod:="Main")
            Return result
        End If

        ' Write the DSN info in ODBC. Overwrites the current one if it exists
        m_lReturn = CType(m_oDestDSN.CreateDSN(sDatabase:=m_cPMBriefcase, vDSN:=m_cTempDME), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="The " & m_cTempDME & " DSN could not be created.", vApp:=ACApp, vClass:=ACClass, vMethod:="Main")
            Return result
        End If

        ' Set Database object for SQL Statements
#If PD_EARLYBOUND = 1 Then

		Set m_oLocalDB = New dPMDAO.Database
#Else
        m_oLocalDB = New dPMDAO.Database()
#End If

        m_lReturn = CType(NewDatabase(v_sUsername:=g_sUsername, v_iSourceID:=g_iSourceID, v_iLanguageID:=g_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

Err_ConnectToLocalDB:

        If Information.Err().Number = -2147221504 Then

            ' MSDE service was not started so start that and resume
            ' DAK150600 - net start doesn't work on Windows 95


            dAppID = Process.Start("net start MSSQLServer").Id

            dAppID = Process.Start(sSQL7Path & "\Binn\scm.exe -Action 1 -Silent 1").Id
            If dAppID = 0 Then
                GoTo Err_ConnectToLocalDB
            End If
            'wait for process to end
            ProcessEnd(CInt(dAppID))
            Resume Login_Resume


        ElseIf Information.Err().Number = -2147204362 Then


            dAppID = Process.Start(sSQL7Path & "\Binn\sqlmangr.exe").Id
            If dAppID = 0 Then
                GoTo Err_ConnectToLocalDB
            End If
            ProcessEnd(CInt(dAppID))
            Resume Login_Resume

        Else
            '
        End If


        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Connect to briefcase database failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ConnectToLocalDB", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result


    End Function

    ' ***************************************************************** '
    ' Name:         DisconnectLocalDB
    '
    ' Description:  Function copies the PMBriefcase db from the Server back to
    '               the laptop PC. Deletes Server copy and temporary DSNs created
    '               in the ConnectToLocalDB function.
    '
    ' ***************************************************************** '
    Public Function DisconnectLocalDB(ByRef sPCName As String) As Integer

        Dim result As Integer = 0
        Dim sSQL7Path, sMSDEPath, sSQLServerName, sPassword As String
        Dim lProcessID As Integer
        Dim dAppID As Double
        Dim oSQLServer As SQLDMO.SQLServer

        On Error GoTo Err_DisconnectLocalDB

        result = gPMConstants.PMEReturnCode.PMTrue

        oSQLServer = New SQLDMO.SQLServer()

        ' The MSDE (MSSQL7) dir on the laptop (SQLDataRoot registry). The dir must be shared.
        sMSDEPath = "\\" & sPCName & "\MSSQL7\Data\"

        ' Get the Server PC's MSSQL7 (SQLServer7) data dir
        ' This gets the path of where SQL7 was installed i.e. "<Drive>:\MSSQL7"

        'Get the SQL Data path
        sSQL7Path = ReadRegistry(gPMConstants.HKEY_LOCAL_MACHINE, "SOFTWARE\Microsoft\MSSQLServer\MSSQLServer\Parameters", "SQLArg0")
        If sSQL7Path = "Not Found" Then
            sSQL7Path = ""
        End If

        If sSQL7Path.Substring(0, Math.Min(sSQL7Path.Length, 2)) = "-d" Then
            sSQL7Path = sSQL7Path.Substring(sSQL7Path.Length - (sSQL7Path.Length - 2))
        End If

        Do While sSQL7Path.Length > 0
            If sSQL7Path.EndsWith("\") Then
                Exit Do
            End If
            sSQL7Path = sSQL7Path.Substring(0, sSQL7Path.Length - 1)
        Loop


        sSQLServerName = "(local)"
        sPassword = ""


Login_Resume:

        result = gPMConstants.PMEReturnCode.PMTrue


        ' Log onto the SQL Server. If service not started then this will run again
        oSQLServer.Connect(sSQLServerName, "sa", sPassword)


        ' if PMBriefcase does not exists then exit sub
        If Not IsThere(oSQLServer.Databases, m_cPMBriefcase) Then
            Return result
        End If

        ' close reference to PMBriefcase
        m_lReturn = m_oLocalDB.CloseDatabase()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Release reference Object
        m_oLocalDB = Nothing

        'didn't always detach the DB first time - loops now till it does
Detach_DB:

        ' detach Briefcase db on Server
        oSQLServer.DetachDB(m_cPMBriefcase, True)

        ' copy PMBriefcase data and log files from Server PC back to laptop PC
        File.Copy(sSQL7Path & m_cDataFile, sMSDEPath & m_cDataFile)
        File.Copy(sSQL7Path & m_cLogFile, sMSDEPath & m_cLogFile)

        ' delete the files from the Server
        File.Delete(sSQL7Path & m_cDataFile)
        File.Delete(sSQL7Path & m_cLogFile)

        ' Remove the Destination DSN reference to PMBriefcase
        m_lReturn = CType(m_oDestDSN.RemoveDSN(), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="The DSN '" & m_cTempDME & "' could not be removed.", vApp:=ACApp, vClass:=ACClass, vMethod:="DisconnectLocalDB")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Return result

Err_DisconnectLocalDB:


        If Information.Err().Number = -2147221504 Then

            ' MSDE service was not started so start that and resume
            ' DAK150600 - net start doesn't work on Windows 95


            dAppID = Process.Start("net start MSSQLServer").Id

            dAppID = Process.Start(sSQL7Path & "\Binn\scm.exe -Action 1 -Silent 1").Id
            If dAppID = 0 Then
                GoTo Err_DisconnectLocalDB
            End If
            'wait for process to end
            ProcessEnd(CInt(dAppID))
            Resume Login_Resume


        ElseIf Information.Err().Number = -2147204362 Then


            dAppID = Process.Start(sSQL7Path & "\Binn\sqlmangr.exe").Id
            If dAppID = 0 Then
                GoTo Err_DisconnectLocalDB
            End If
            ProcessEnd(CInt(dAppID))
            Resume Login_Resume

        ElseIf Information.Err().Number = 70 Then

            Resume Detach_DB

        Else
            '
        End If

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DisconnectLocalDBFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="DisconnectLocalDB", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result


    End Function


    ' ***************************************************************** '
    ' Name:         UpdateLocalDB
    '
    ' Description:  Updates the briefcase database by clearing the whole db
    '               and copies over the relevant tables from Server Database

    '
    ' ***************************************************************** '
    Public Function UpdateLocalDB() As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object ' 2D array (Col,Row) for db SELECT 
        ' Row of db
        Dim sQuote As String = ""  ' Single Quote char 
        Dim sComma As String = ""  ' Comma Char 
        Dim sCurrentUser As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sQuote = "'"
            sComma = ","
            ' build SQL to delete all tables in briefcase DB
            m_sSQL = ""
            m_sSQL = m_sSQL & "DELETE FROM DOC_annotation; "
            m_sSQL = m_sSQL & "DELETE FROM DOC_device; "
            m_sSQL = m_sSQL & "DELETE FROM DOC_doc_info; "
            m_sSQL = m_sSQL & "DELETE FROM DOC_doc_keyword ;"
            m_sSQL = m_sSQL & "DELETE FROM DOC_doc_name; "
            m_sSQL = m_sSQL & "DELETE FROM DOC_doc_user; "
            m_sSQL = m_sSQL & "DELETE FROM DOC_document; "
            m_sSQL = m_sSQL & "DELETE FROM DOC_folder; "
            m_sSQL = m_sSQL & "DELETE FROM DOC_history; "
            m_sSQL = m_sSQL & "DELETE FROM DOC_keyword; "
            m_sSQL = m_sSQL & "DELETE FROM DOC_page; "
            m_sSQL = m_sSQL & "DELETE FROM DOC_system; "
            m_sSQL = m_sSQL & "DELETE FROM DOC_volume; "
            ' Hit the briefcase db
            m_lReturn = CType(m_oLocalDB.SQLAction(sSQL:=m_sSQL, sSQLName:="DELETEALLTABLES", bStoredProcedure:=False), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Copy contents of system, keyword, device and volume from Server db to Local db


            'SYSTEM TABLE

            m_sSQL = "SELECT system_id, doc_date, expiry_date, admin_level, next_page, update_history FROM DOC_system"
            m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="SELECTFROMSYSTEM", bStoredProcedure:=False, lNumberRecords:=-1, vResultArray:=vArray), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            If Information.IsArray(vArray) Then


                For iRow As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)

                    'validate strings

                    m_sTmp = CStr(vArray(4, iRow))
                    m_lReturn = CType(ValidateSQL(m_sTmp), gPMConstants.PMEReturnCode)

                    vArray(4, iRow) = m_sTmp


                    m_sTmp = CStr(vArray(5, iRow))
                    m_lReturn = CType(ValidateSQL(m_sTmp), gPMConstants.PMEReturnCode)

                    vArray(5, iRow) = m_sTmp

                    m_sSQL = ""
                    m_sSQL = m_sSQL & " INSERT DOC_system (system_id, doc_date, expiry_date,"
                    m_sSQL = m_sSQL & " admin_level, next_page, update_history)"
                    m_sSQL = m_sSQL & " VALUES ("






                    m_sSQL = m_sSQL &
                             CInt(vArray(0, iRow)) & sComma &
                             CInt(vArray(1, iRow)) & sComma &
                             CInt(vArray(2, iRow)) & sComma &
                             CInt(vArray(3, iRow)) & sComma &
                             sQuote & CStr(vArray(4, iRow)) & sQuote & sComma &
                             sQuote & CStr(vArray(5, iRow)) & sQuote
                    m_sSQL = m_sSQL & ")"
                    ' copy Server data to Local db
                    m_lReturn = CType(m_oLocalDB.SQLAction(sSQL:=m_sSQL, sSQLName:="COPYSYSTEMTABLE", bStoredProcedure:=False), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If

                Next iRow
            End If

            'KEYWORD TABLE

            m_sSQL = "SELECT keyword_id, keyword, deleted FROM DOC_keyword"
            m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="SELECTFROMKEYWORD", bStoredProcedure:=False, lNumberRecords:=-1, vResultArray:=vArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If


            If Information.IsArray(vArray) Then


                For iRow As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)


                    m_sTmp = CStr(vArray(1, iRow))
                    m_lReturn = CType(ValidateSQL(m_sTmp), gPMConstants.PMEReturnCode)

                    vArray(1, iRow) = m_sTmp


                    m_sTmp = CStr(vArray(2, iRow))
                    m_lReturn = CType(ValidateSQL(m_sTmp), gPMConstants.PMEReturnCode)

                    vArray(2, iRow) = m_sTmp

                    ' insert server records into briefcase db
                    m_sSQL = ""
                    m_sSQL = m_sSQL & " INSERT DOC_keyword (keyword_id, keyword, deleted)"
                    m_sSQL = m_sSQL & " VALUES ("



                    m_sSQL = m_sSQL &
                             CInt(vArray(0, iRow)) & sComma &
                             sQuote & CStr(vArray(1, iRow)) & sQuote & sComma &
                             sQuote & CStr(vArray(2, iRow)) & sQuote
                    m_sSQL = m_sSQL & ")"
                    ' copy Server data to Local db
                    m_lReturn = CType(m_oLocalDB.SQLAction(sSQL:=m_sSQL, sSQLName:="COPYKEYWORDTABLE", bStoredProcedure:=False), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If
                Next iRow
            End If


            'DEVICE TABLE

            m_sSQL = "SELECT device_id, device_name, server_unc, share_name, drive FROM DOC_device"
            m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="SELECTFROMDEVICE", bStoredProcedure:=False, lNumberRecords:=-1, vResultArray:=vArray), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

            End If

            If Information.IsArray(vArray) Then


                For iRow As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)


                    m_sTmp = CStr(vArray(1, iRow))
                    m_lReturn = CType(ValidateSQL(m_sTmp), gPMConstants.PMEReturnCode)

                    vArray(1, iRow) = m_sTmp


                    m_sTmp = CStr(vArray(2, iRow))
                    m_lReturn = CType(ValidateSQL(m_sTmp), gPMConstants.PMEReturnCode)

                    vArray(2, iRow) = m_sTmp


                    m_sTmp = CStr(vArray(3, iRow))
                    m_lReturn = CType(ValidateSQL(m_sTmp), gPMConstants.PMEReturnCode)

                    vArray(3, iRow) = m_sTmp


                    m_sTmp = CStr(vArray(4, iRow))
                    m_lReturn = CType(ValidateSQL(m_sTmp), gPMConstants.PMEReturnCode)

                    vArray(4, iRow) = m_sTmp

                    m_sSQL = ""
                    m_sSQL = m_sSQL & " INSERT DOC_device (device_id, device_name, server_unc, "
                    m_sSQL = m_sSQL & " share_name, drive)"
                    m_sSQL = m_sSQL & " VALUES ("





                    m_sSQL = m_sSQL &
                             CInt(vArray(0, iRow)) & sComma &
                             sQuote & CStr(vArray(1, iRow)) & sQuote & sComma &
                             sQuote & CStr(vArray(2, iRow)) & sQuote & sComma &
                             sQuote & CStr(vArray(3, iRow)) & sQuote & sComma &
                             sQuote & CStr(vArray(4, iRow)) & sQuote
                    m_sSQL = m_sSQL & ")"
                    ' copy Server data to Local db
                    m_lReturn = CType(m_oLocalDB.SQLAction(sSQL:=m_sSQL, sSQLName:="COPYDEVICETABLE", bStoredProcedure:=False), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If

                Next iRow
            End If

            ' VOLUME TABLE

            m_sSQL = "SELECT volume_id, volume_name, directory, device_id FROM DOC_volume"
            m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="SELECTFROMVOLUME", bStoredProcedure:=False, lNumberRecords:=-1, vResultArray:=vArray), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

            End If

            ' copy system data from Server db to Briefcase Local db
            If Information.IsArray(vArray) Then


                For iRow As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)


                    m_sTmp = CStr(vArray(1, iRow))
                    m_lReturn = CType(ValidateSQL(m_sTmp), gPMConstants.PMEReturnCode)

                    vArray(1, iRow) = m_sTmp


                    m_sTmp = CStr(vArray(2, iRow))
                    m_lReturn = CType(ValidateSQL(m_sTmp), gPMConstants.PMEReturnCode)

                    vArray(2, iRow) = m_sTmp

                    m_sSQL = ""
                    m_sSQL = m_sSQL & " INSERT DOC_volume (volume_id, volume_name,"
                    m_sSQL = m_sSQL & " directory, device_id)"
                    m_sSQL = m_sSQL & " VALUES ("




                    m_sSQL = m_sSQL &
                             CInt(vArray(0, iRow)) & sComma &
                             sQuote & CStr(vArray(1, iRow)) & sQuote & sComma &
                             sQuote & CStr(vArray(2, iRow)) & sQuote & sComma &
                             CInt(vArray(3, iRow))

                    m_sSQL = m_sSQL & ")"

                    ' copy Server data to Local db
                    m_lReturn = CType(m_oLocalDB.SQLAction(sSQL:=m_sSQL, sSQLName:="COPYVOLUMETABLE", bStoredProcedure:=False), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If

                Next iRow

            End If

            'Transfer the doc_user records for current user and sirius

            sCurrentUser = CStr(ValidateSQL(g_sUsername))

            ' copy current user and sirius user to briefcase briefcase db
            m_sSQL = "SELECT user_id, access_level, user_name, home_folder_num, retired FROM DOC_doc_user"
            m_sSQL = m_sSQL & " WHERE user_name = " & sQuote & sCurrentUser & sQuote
            m_sSQL = m_sSQL & " OR user_name = 'sirius'"

            m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="SELECTFROMDOCUSR", bStoredProcedure:=False, lNumberRecords:=-1, vResultArray:=vArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If


            ' copy system data from Server db to Briefcase Local db
            If Information.IsArray(vArray) Then


                For iRow As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)


                    m_sTmp = CStr(vArray(2, iRow))
                    m_lReturn = CType(ValidateSQL(m_sTmp), gPMConstants.PMEReturnCode)

                    vArray(2, iRow) = m_sTmp


                    m_sTmp = CStr(vArray(4, iRow))
                    m_lReturn = CType(ValidateSQL(m_sTmp), gPMConstants.PMEReturnCode)

                    vArray(4, iRow) = m_sTmp


                    m_sSQL = ""
                    m_sSQL = m_sSQL & " INSERT DOC_doc_user (user_id, access_level, user_name,"
                    m_sSQL = m_sSQL & " home_folder_num, retired)"
                    m_sSQL = m_sSQL & " VALUES ("





                    m_sSQL = m_sSQL &
                             CInt(vArray(0, iRow)) & sComma &
                             CInt(vArray(1, iRow)) & sComma &
                             sQuote & CStr(vArray(2, iRow)) & sQuote & sComma &
                             CInt(vArray(3, iRow)) & sComma &
                             sQuote & CStr(vArray(4, iRow)) & sQuote
                    m_sSQL = m_sSQL & ")"
                    ' copy data to Local db
                    m_lReturn = CType(m_oLocalDB.SQLAction(sSQL:=m_sSQL, sSQLName:="COPYDOCUSERTABLE", bStoredProcedure:=False), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If
                Next iRow
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateLocalDBFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateLocalDB", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    ' Name: GetChildFolder
    '
    ' Description: Return the child folder records for a parent
    '
    '
    ' ***************************************************************** '

    Public Function GetChildFolder(ByRef lFolderNum As Integer, ByRef vArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Construct SQL

            m_sSQL = " SELECT folder_num, folder_name, parent_num, ex_code, folder_level,"
            m_sSQL = m_sSQL & " access_level, password, create_date FROM DOC_folder"
            m_sSQL = m_sSQL & " WHERE parent_num = " & CStr(lFolderNum)

            'Hit the DB - should be only one record match, but request more than one
            'as will show up any errors.

            m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="GetChildFolder", bStoredProcedure:=False, lNumberRecords:=-1, vResultArray:=vArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetChildFolder", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function



    ' ***************************************************************** '
    ' Name: GetParentExCode
    '
    ' Description: Get the parent ex_code for a child in Server db
    '
    ' ***************************************************************** '

    Public Function GetParentExCode(ByRef sChildExCode As String, ByRef sParentExCode As String, ByRef lChildFoldLevel As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'get parent ex_code by matching child's parent_num with parent's fold_num
            m_sSQL = "SELECT p.ex_code FROM DOC_folder c, DOC_folder p WHERE"
            m_sSQL = m_sSQL & " c.ex_code = " & "'" & sChildExCode & "'" & " AND "
            m_sSQL = m_sSQL & " c.folder_level = " & CStr(lChildFoldLevel) & " AND "
            m_sSQL = m_sSQL & " c.parent_num = p.folder_num "

            'Hit server DB - should be only one record match, but request more than one
            'as will show up any errors.
            m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="GetParentExCode", bStoredProcedure:=False, lNumberRecords:=2, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' record found
            If Information.IsArray(vResultArray) Then

                sParentExCode = CStr(vResultArray(0, 0))
            End If


            ' no record found
            If Not Information.IsArray(vResultArray) Then

                'this is wrong
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Record not found, ex_code = " & sChildExCode, vApp:=ACApp, vClass:=ACClass, vMethod:="GetParentExCode", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If


            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetFolderNum", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function


    ' ***************************************************************** '
    ' Name: InsertFolderRec
    '
    ' Description: Insert a record into the folder table in briefcase db
    '
    ' ***************************************************************** '
    Public Function InsertFolderRec(ByRef vArray(,) As Object, ByRef iRow As Integer) As Integer

        Dim result As Integer = 0
        Dim vTempArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'table folder contains the following 8 fields
            'folder_num, folder_name, parent_num, ex_code, folder_level, access_level, password, create_date
            'Construct SQL
            If Information.IsArray(vArray) Then

                ' only insert if the folder rec not already in briefcase db

                m_sSQL = " SELECT folder_num FROM DOC_folder WHERE" &
                         " folder_num = " & CStr(CInt(vArray(0, iRow)))

                m_lReturn = CType(m_oLocalDB.SQLSelect(sSQL:=m_sSQL, sSQLName:="SelectFolder", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=vTempArray), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' folder already exists so exit function without inserting
                If Information.IsArray(vTempArray) Then
                    Return result
                End If

                'format date  create_date


                vArray(7, iRow) = CDate(vArray(7, iRow)).ToString("MMM dd yyyy HH:MM")

                'validate strings

                m_sTmp = CStr(vArray(1, iRow))
                m_lReturn = CType(ValidateSQL(m_sTmp), gPMConstants.PMEReturnCode)

                vArray(1, iRow) = m_sTmp


                m_sTmp = CStr(vArray(3, iRow))
                m_lReturn = CType(ValidateSQL(m_sTmp), gPMConstants.PMEReturnCode)

                vArray(3, iRow) = m_sTmp


                m_sTmp = CStr(vArray(6, iRow))
                m_lReturn = CType(ValidateSQL(m_sTmp), gPMConstants.PMEReturnCode)

                vArray(6, iRow) = m_sTmp


                m_sSQL = ""
                m_sSQL = m_sSQL & " INSERT DOC_folder (folder_num, folder_name, parent_num, ex_code,"
                m_sSQL = m_sSQL & " folder_level, access_level, password, create_date)"
                m_sSQL = m_sSQL & " VALUES ("








                m_sSQL = m_sSQL &
                         CInt(vArray(0, iRow)) & m_cComma &
                         m_cQuote & CStr(vArray(1, iRow)) & m_cQuote & m_cComma &
                         CInt(vArray(2, iRow)) & m_cComma &
                         m_cQuote & CStr(vArray(3, iRow)) & m_cQuote & m_cComma &
                         CInt(vArray(4, iRow)) & m_cComma &
                         CInt(vArray(5, iRow)) & m_cComma &
                         m_cQuote & CStr(vArray(6, iRow)) & m_cQuote & m_cComma &
                         m_cQuote & CStr(vArray(7, iRow)) & m_cQuote
                m_sSQL = m_sSQL & ")"
                ' copy Server data to Local db
                m_lReturn = CType(m_oLocalDB.SQLAction(sSQL:=m_sSQL, sSQLName:="InsertFolderRec", bStoredProcedure:=False), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse

                End If

            End If


            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="InsertFolderRec", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetFolderRecFromExCode
    '
    ' Description: Get a folder record from server db matching the excode and folder level
    '
    ' ***************************************************************** '

    Public Function GetFolderRecFromExCode(ByRef sExCode As String, ByRef lFolderLevel As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_sSQL = " SELECT folder_num, folder_name, parent_num, ex_code, folder_level,"
            m_sSQL = m_sSQL & " access_level, password, create_date FROM DOC_folder"
            m_sSQL = m_sSQL & " WHERE ex_code = '" & sExCode & "'"
            m_sSQL = m_sSQL & " AND folder_level = " & CStr(lFolderLevel)

            'Hit the DB
            m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="GetFolderRec", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If Not Information.IsArray(vResultArray) Then
                'this is wrong
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="DME to Briefcase Download: Folder not found on Server, ex_code = " & sExCode & " folder_level = " & CStr(lFolderLevel), vApp:=ACApp, vClass:=ACClass, vMethod:="GetFolderRecFromExCode", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If


            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetFolderRecFromExCode", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetFolderRec
    '
    ' Description: Get a folder record from server db matching the parentnum
    '
    ' ***************************************************************** '

    Public Function GetFolderRec(ByRef sExCode As String, ByRef lFolderLevel As Integer, ByRef lParentNum As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_sSQL = " SELECT folder_num, folder_name, parent_num, ex_code, folder_level,"
            m_sSQL = m_sSQL & " access_level, password, create_date FROM DOC_folder"
            m_sSQL = m_sSQL & " WHERE ex_code = '" & sExCode & "'"
            m_sSQL = m_sSQL & " AND folder_level = " & CStr(lFolderLevel)
            m_sSQL = m_sSQL & " AND parent_num = " & CStr(lParentNum)

            'Hit the DB
            m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="GetFolderRec", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If Not Information.IsArray(vResultArray) Then
                'this is wrong
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="DME to Briefcase Download: Folder not found on Server, ex_code = " & sExCode & " folder_level = " & CStr(lFolderLevel), vApp:=ACApp, vClass:=ACClass, vMethod:="GetFolderRec", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If


            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetFolderRec", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetGeneralFolder
    '
    ' Description: Get the GENERAL folder for a given drawer
    '              (parentnum)
    '
    ' ***************************************************************** '

    Public Function GetGeneralFolder(ByRef lDrawerNum As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_sSQL = " SELECT folder_num, folder_name, parent_num, ex_code, folder_level,"
            m_sSQL = m_sSQL & " access_level, password, create_date FROM DOC_folder"
            m_sSQL = m_sSQL & " WHERE folder_name = 'GENERAL'"
            m_sSQL = m_sSQL & " AND folder_level = " & CStr(DOCFolder)
            m_sSQL = m_sSQL & " AND parent_num = " & CStr(lDrawerNum)

            'Hit the DB
            m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="GetGeneralFolder", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If Not Information.IsArray(vResultArray) Then
                'this is wrong
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GENERAL Folder not found for Drawer folder_num, " & lDrawerNum, vApp:=ACApp, vClass:=ACClass, vMethod:="GetGeneralFolder", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If


            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetGeneralFolder", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateDocuments
    '
    ' Description: For each folder written to briefcase db, obtain details of
    '              all child DOCUMENT and DOC_INFO records on server. Write
    '              equivalent records to briefcase db.
    '
    ' ***************************************************************** '

    Public Function UpdateDocuments() As Integer

        Dim result As Integer = 0
        Dim vDocArray(,) As Object
        Dim vFolderNumArray(,) As Object
        Dim vTempArray(,) As Object
        Dim lFolderNum As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Store only those document records that are related to folder records in briefcase db
            '
            ' Step 1.Read briefcase folder recs into array
            ' Step 2.For each folder rec, find any document recs related to it on server db
            ' Step 3.For each document found, store it in briefcase db (if it does not exist already)


            ' Step 1.
            ' read folder in briefcase db  in order to extract the documents related to the folder(s)
            ' from the server

            m_sSQL = " SELECT folder_num FROM DOC_folder"

            m_lReturn = CType(m_oLocalDB.SQLSelect(sSQL:=m_sSQL, sSQLName:="UpdateDocuments", bStoredProcedure:=False, lNumberRecords:=-1, vResultArray:=vFolderNumArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Step 2.
            ' folder recs exist in briefcase db
            If Information.IsArray(vFolderNumArray) Then

                ' read each folder

                For iInd As Integer = vFolderNumArray.GetLowerBound(1) To vFolderNumArray.GetUpperBound(1)

                    ' folder.folder_num

                    lFolderNum = CInt(vFolderNumArray(0, iInd))

                    ' get document from server that matches this folder in  briefcase db
                    m_sSQL = ""
                    m_sSQL = m_sSQL & " SELECT doc_num , folder_num, doc_name, ex_code, doc_type, "
                    m_sSQL = m_sSQL & " access_level, password, create_date, zipped, link "
                    m_sSQL = m_sSQL & " FROM DOC_document WHERE folder_num = " & CStr(lFolderNum)

                    m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="UpdateDocuments", bStoredProcedure:=False, lNumberRecords:=-1, vResultArray:=vDocArray), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Step 3.
                    ' found documents belonging to folder in server db
                    If Information.IsArray(vDocArray) Then

                        ' read each document found and store in briefcase db

                        For iRow As Integer = vDocArray.GetLowerBound(1) To vDocArray.GetUpperBound(1)

                            ' ensure the row is not already in briefcase db

                            m_sSQL = " SELECT doc_num FROM DOC_document" &
                                     " WHERE doc_num = " & CStr(CInt(vDocArray(0, iRow)))

                            m_lReturn = CType(m_oLocalDB.SQLSelect(sSQL:=m_sSQL, sSQLName:="UpdateDocuments", bStoredProcedure:=False, lNumberRecords:=-1, vResultArray:=vTempArray), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                            End If

                            ' not a duplicate row so proceed to insert
                            If Not Information.IsArray(vTempArray) Then

                                'validate strings


                                m_sTmp = CStr(vDocArray(2, iRow))
                                m_lReturn = CType(ValidateSQL(m_sTmp), gPMConstants.PMEReturnCode)

                                vDocArray(2, iRow) = m_sTmp


                                m_sTmp = CStr(vDocArray(3, iRow))
                                m_lReturn = CType(ValidateSQL(m_sTmp), gPMConstants.PMEReturnCode)

                                vDocArray(3, iRow) = m_sTmp


                                m_sTmp = CStr(vDocArray(4, iRow))
                                m_lReturn = CType(ValidateSQL(m_sTmp), gPMConstants.PMEReturnCode)

                                vDocArray(4, iRow) = m_sTmp


                                m_sTmp = CStr(vDocArray(6, iRow))
                                m_lReturn = CType(ValidateSQL(m_sTmp), gPMConstants.PMEReturnCode)

                                vDocArray(6, iRow) = m_sTmp


                                m_sTmp = CStr(vDocArray(8, iRow))
                                m_lReturn = CType(ValidateSQL(m_sTmp), gPMConstants.PMEReturnCode)

                                vDocArray(8, iRow) = m_sTmp


                                ' format date field create_date


                                vDocArray(7, iRow) = CDate(vDocArray(7, iRow)).ToString("MMM dd yyyy HH:MM")

                                m_sSQL = ""
                                m_sSQL = m_sSQL & " INSERT DOC_document(doc_num ,folder_num, doc_name, ex_code,"
                                m_sSQL = m_sSQL & " doc_type, access_level, password, create_date,"
                                m_sSQL = m_sSQL & " zipped, link)"
                                m_sSQL = m_sSQL & " VALUES ("










                                m_sSQL = m_sSQL &
                                         CInt(vDocArray(0, iRow)) & m_cComma &
                                         CInt(vDocArray(1, iRow)) & m_cComma &
                                         m_cQuote & CStr(vDocArray(2, iRow)) & m_cQuote & m_cComma &
                                         m_cQuote & CStr(vDocArray(3, iRow)) & m_cQuote & m_cComma &
                                         m_cQuote & CStr(vDocArray(4, iRow)) & m_cQuote & m_cComma &
                                         CInt(vDocArray(5, iRow)) & m_cComma &
                                         m_cQuote & CStr(vDocArray(6, iRow)) & m_cQuote & m_cComma &
                                         m_cQuote & CStr(vDocArray(7, iRow)) & m_cQuote & m_cComma &
                                         m_cQuote & CStr(vDocArray(8, iRow)) & m_cQuote & m_cComma &
                                         CInt(vDocArray(9, iRow))

                                m_sSQL = m_sSQL & ")"
                                ' insert into Local db
                                m_lReturn = CType(m_oLocalDB.SQLAction(sSQL:=m_sSQL, sSQLName:="UpdateDocuments", bStoredProcedure:=False), gPMConstants.PMEReturnCode)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    result = gPMConstants.PMEReturnCode.PMFalse
                                End If
                            End If
                        Next iRow

                    End If

                Next iInd

            End If  ' IsAarray


            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateDocuments", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: UpdateDocInfoRecs
    '
    ' Description: For each document written to briefcase db, obtain details of
    '              DOC_INFO records on server. Write equivalent records to briefcase db
    '
    ' ***************************************************************** '

    Public Function UpdateDocInfoRecs() As Integer

        Dim result As Integer = 0
        Dim vDocInfoArray(,) As Object
        Dim vDocNumArray(,) As Object
        Dim vTempArray(,) As Object
        Dim lDocNum As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Store only those doc_info records that are related to documents records in briefcase db
            '
            ' Step 1.Read briefcase document recs into array
            ' Step 2.For each document rec, find any doc_info recs related to it on server db
            ' Step 3.For each doc_info found, store it in briefcase db

            'Step 1.

            m_sSQL = " SELECT doc_num FROM DOC_document"
            ' Hit briefcase db
            m_lReturn = CType(m_oLocalDB.SQLSelect(sSQL:=m_sSQL, sSQLName:="UpdateDocInfoRecs", bStoredProcedure:=False, lNumberRecords:=-1, vResultArray:=vDocNumArray), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Step 2.
            ' found document(s)
            If Information.IsArray(vDocNumArray) Then

                ' read each document

                For iInd As Integer = vDocNumArray.GetLowerBound(1) To vDocNumArray.GetUpperBound(1)

                    ' document.doc_num

                    lDocNum = CInt(vDocNumArray(0, iInd))

                    ' get doc_info rec from server that matches this document in  briefcase db
                    m_sSQL = "SELECT doc_num, expiry_date, scan_user, doc_date, last_user, last_date"
                    m_sSQL = m_sSQL & " FROM DOC_doc_info WHERE doc_num = " & CStr(lDocNum)

                    m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="UpdateDocInfoRecs", bStoredProcedure:=False, lNumberRecords:=-1, vResultArray:=vDocInfoArray), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' found doc_info recs
                    If Information.IsArray(vDocInfoArray) Then

                        ' store each doc_info recs found onto briefcase db

                        For iRow As Integer = vDocInfoArray.GetLowerBound(1) To vDocInfoArray.GetUpperBound(1)

                            ' ensure the row is not already in briefcase briefcase db

                            m_sSQL = " SELECT doc_num FROM DOC_doc_info" &
                                     " WHERE doc_num = " & CStr(CInt(vDocInfoArray(0, iRow)))

                            m_lReturn = CType(m_oLocalDB.SQLSelect(sSQL:=m_sSQL, sSQLName:="UpdateDocInfoRecs", bStoredProcedure:=False, lNumberRecords:=-1, vResultArray:=vTempArray), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                            End If

                            'not a duplicate row so proceed to insert
                            If Not Information.IsArray(vTempArray) Then

                                'format date fields   - expirydate, doc_date, last_date


                                vDocInfoArray(1, iRow) = CDate(vDocInfoArray(1, iRow)).ToString("MMM dd yyyy HH:MM")


                                vDocInfoArray(3, iRow) = CDate(vDocInfoArray(3, iRow)).ToString("MMM dd yyyy HH:MM")


                                vDocInfoArray(5, iRow) = CDate(vDocInfoArray(5, iRow)).ToString("MMM dd yyyy HH:MM")

                                'format strings

                                m_sTmp = CStr(vDocInfoArray(2, iRow))
                                m_lReturn = CType(ValidateSQL(m_sTmp), gPMConstants.PMEReturnCode)

                                vDocInfoArray(2, iRow) = m_sTmp


                                m_sTmp = CStr(vDocInfoArray(4, iRow))
                                m_lReturn = CType(ValidateSQL(m_sTmp), gPMConstants.PMEReturnCode)

                                vDocInfoArray(4, iRow) = m_sTmp

                                m_sSQL = ""
                                m_sSQL = m_sSQL & " INSERT DOC_doc_info (doc_num, expiry_date, scan_user,"
                                m_sSQL = m_sSQL & " doc_date, last_user, last_date)"
                                m_sSQL = m_sSQL & " VALUES ("






                                m_sSQL = m_sSQL &
                                         CInt(vDocInfoArray(0, iRow)) & m_cComma &
                                         m_cQuote & CStr(vDocInfoArray(1, iRow)) & m_cQuote & m_cComma &
                                         m_cQuote & CStr(vDocInfoArray(2, iRow)) & m_cQuote & m_cComma &
                                         m_cQuote & CStr(vDocInfoArray(3, iRow)) & m_cQuote & m_cComma &
                                         m_cQuote & CStr(vDocInfoArray(4, iRow)) & m_cQuote & m_cComma &
                                         m_cQuote & CStr(vDocInfoArray(5, iRow)) & m_cQuote
                                m_sSQL = m_sSQL & ")"
                                ' insert into Local db
                                m_lReturn = CType(m_oLocalDB.SQLAction(sSQL:=m_sSQL, sSQLName:="UpdateDocInfoRecs", bStoredProcedure:=False), gPMConstants.PMEReturnCode)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    result = gPMConstants.PMEReturnCode.PMFalse
                                End If
                            End If
                        Next iRow

                    End If

                Next iInd

            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateDocInfoRecs", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function


    ' ***************************************************************** '
    ' Name: UpdatePageInfo
    '
    ' Description: For each document written to briefcase db, obtain details of
    '              all PAGE records from server. Write equivalent records
    '              to briefcase db
    '
    ' ***************************************************************** '
    Public Function UpdatePageInfo() As Integer

        Dim result As Integer = 0
        Dim vPageArray(,) As Object
        Dim vDocNumArray(,) As Object
        Dim vTempArray(,) As Object
        Dim lDocNum As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Store only those page records that are related to documents records in briefcase db
            '
            ' Step 1.Read briefcase document recs into array
            ' Step 2.For each document rec, find any page recs related to it on server db
            ' Step 3.For each page found, store it in briefcase db


            ' Step 1.
            m_sSQL = " SELECT doc_num FROM DOC_document"
            ' Hit briefcase db
            m_lReturn = CType(m_oLocalDB.SQLSelect(sSQL:=m_sSQL, sSQLName:="UpdatePageInfo", bStoredProcedure:=False, lNumberRecords:=-1, vResultArray:=vDocNumArray), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Step 2.
            ' found document(s)
            If Information.IsArray(vDocNumArray) Then

                ' read each document

                For iInd As Integer = vDocNumArray.GetLowerBound(1) To vDocNumArray.GetUpperBound(1)

                    ' document.doc_num

                    lDocNum = CInt(vDocNumArray(0, iInd))

                    ' get page rec from server that matches this document in  briefcase db
                    m_sSQL = " SELECT page_name, doc_num, page_num, page_type, create_date, page_size, volume_id"
                    m_sSQL = m_sSQL & " FROM DOC_page WHERE doc_num = " & CStr(lDocNum)

                    m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="UpdatePageInfo", bStoredProcedure:=False, lNumberRecords:=-1, vResultArray:=vPageArray), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' found page recs,
                    If Information.IsArray(vPageArray) Then

                        ' store pages to briefcase db

                        For iRow As Integer = vPageArray.GetLowerBound(1) To vPageArray.GetUpperBound(1)

                            ' ensure the row is not already in briefcase briefcase db

                            m_sSQL = " SELECT page_name FROM DOC_page" &
                                     " WHERE page_name = " & m_cQuote & CStr(vPageArray(0, iRow)) & m_cQuote

                            m_lReturn = CType(m_oLocalDB.SQLSelect(sSQL:=m_sSQL, sSQLName:="UpdatePageInfo", bStoredProcedure:=False, lNumberRecords:=-1, vResultArray:=vTempArray), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                            End If

                            'not a duplicate row so proceed to insert
                            If Not Information.IsArray(vTempArray) Then

                                'format date create_date


                                vPageArray(4, iRow) = CDate(vPageArray(4, iRow)).ToString("MMM dd yyyy HH:MM")


                                m_sTmp = CStr(vPageArray(0, iRow))
                                m_lReturn = CType(ValidateSQL(m_sTmp), gPMConstants.PMEReturnCode)

                                vPageArray(0, iRow) = m_sTmp


                                m_sTmp = CStr(vPageArray(3, iRow))
                                m_lReturn = CType(ValidateSQL(m_sTmp), gPMConstants.PMEReturnCode)

                                vPageArray(3, iRow) = m_sTmp

                                m_sSQL = ""
                                m_sSQL = m_sSQL & " INSERT DOC_page (page_name, doc_num, page_num, page_type,"
                                m_sSQL = m_sSQL & " create_date, page_size, volume_id)"
                                m_sSQL = m_sSQL & " VALUES ("







                                m_sSQL = m_sSQL &
                                         m_cQuote & CStr(vPageArray(0, iRow)) & m_cQuote & m_cComma &
                                         CInt(vPageArray(1, iRow)) & m_cComma &
                                         CInt(vPageArray(2, iRow)) & m_cComma &
                                         m_cQuote & CStr(vPageArray(3, iRow)) & m_cQuote & m_cComma &
                                         m_cQuote & CStr(vPageArray(4, iRow)) & m_cQuote & m_cComma &
                                         CInt(vPageArray(5, iRow)) & m_cComma &
                                         CInt(vPageArray(6, iRow))
                                m_sSQL = m_sSQL & ")"
                                ' insert into Local db
                                m_lReturn = CType(m_oLocalDB.SQLAction(sSQL:=m_sSQL, sSQLName:="UpdatePageInfo", bStoredProcedure:=False), gPMConstants.PMEReturnCode)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    result = gPMConstants.PMEReturnCode.PMFalse
                                End If
                            End If
                        Next iRow

                    End If

                Next iInd

            End If


            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePageInfo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateKeywordInfo
    '
    ' Description: For each document written to briefcase db, obtain details of
    '              all DOC_KEYWORD records from server. Write equivalent records
    '              to briefcase db
    '
    ' ***************************************************************** '

    Public Function UpdateKeywordInfo() As Integer

        Dim result As Integer = 0
        Dim vDocKeyArray(,) As Object
        Dim vDocNumArray(,) As Object
        Dim vTempArray(,) As Object
        Dim lDocNum As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Store only those doc_keyword records that are related to documents records in briefcase db
            '
            ' Step 1.Read briefcase document recs into array
            ' Step 2.For each document rec, find any doc_keyword recs related to it on server db
            ' Step 3.For each doc_keyword found, store it in briefcase db

            m_sSQL = " SELECT doc_num FROM DOC_document"

            m_lReturn = CType(m_oLocalDB.SQLSelect(sSQL:=m_sSQL, sSQLName:="UpdateKeywordInfo", bStoredProcedure:=False, lNumberRecords:=-1, vResultArray:=vDocNumArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Step 2.
            ' document recs exist in briefcase db
            If Information.IsArray(vDocNumArray) Then

                ' read document folder

                For iInd As Integer = vDocNumArray.GetLowerBound(1) To vDocNumArray.GetUpperBound(1)

                    ' document.doc_num

                    lDocNum = CInt(vDocNumArray(0, iInd))

                    ' get doc_keyword from server that matches this document in  briefcase db
                    m_sSQL = " SELECT doc_num, doc_keyword_id, keyword_id, user_name, create_date FROM DOC_doc_keyword WHERE doc_num = " & lDocNum

                    m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="UpdateKeywordInfo", bStoredProcedure:=False, lNumberRecords:=-1, vResultArray:=vDocKeyArray), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Step 3.
                    ' found doc_keyword belonging to document in server db
                    If Information.IsArray(vDocKeyArray) Then

                        ' read each document found and store in briefcase db

                        For iRow As Integer = vDocKeyArray.GetLowerBound(1) To vDocKeyArray.GetUpperBound(1)

                            ' ensure the row is not already in briefcase briefcase db

                            m_sSQL = " SELECT doc_num FROM DOC_doc_keyword" &
                                     " WHERE doc_num = " & CStr(CInt(vDocKeyArray(0, iRow)))

                            m_lReturn = CType(m_oLocalDB.SQLSelect(sSQL:=m_sSQL, sSQLName:="UpdateKeywordInfo", bStoredProcedure:=False, lNumberRecords:=-1, vResultArray:=vTempArray), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                            End If

                            'not a duplicate row so proceed to insert
                            If Not Information.IsArray(vTempArray) Then

                                'format date  create_date


                                vDocKeyArray(4, iRow) = CDate(vDocKeyArray(4, iRow)).ToString("MMM dd yyyy HH:MM")


                                m_sTmp = CStr(vDocKeyArray(3, iRow))
                                m_lReturn = CType(ValidateSQL(m_sTmp), gPMConstants.PMEReturnCode)

                                vDocKeyArray(3, iRow) = m_sTmp

                                m_sSQL = ""
                                m_sSQL = m_sSQL & " INSERT DOC_doc_keyword (doc_num, doc_keyword_id, "
                                m_sSQL = m_sSQL & " keyword_id, user_name , create_date)"
                                m_sSQL = m_sSQL & " VALUES ("





                                m_sSQL = m_sSQL &
                                         CInt(vDocKeyArray(0, iRow)) & m_cComma &
                                         CInt(vDocKeyArray(1, iRow)) & m_cComma &
                                         CInt(vDocKeyArray(2, iRow)) & m_cComma &
                                         m_cQuote & CStr(vDocKeyArray(3, iRow)) & m_cQuote & m_cComma &
                                         m_cQuote & CStr(vDocKeyArray(4, iRow)) & m_cQuote
                                m_sSQL = m_sSQL & ")"
                                ' insert into Local db
                                m_lReturn = CType(m_oLocalDB.SQLAction(sSQL:=m_sSQL, sSQLName:="UpdateKeywordInfo", bStoredProcedure:=False), gPMConstants.PMEReturnCode)

                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    result = gPMConstants.PMEReturnCode.PMFalse
                                End If
                            End If
                        Next iRow

                    End If

                Next iInd

            End If


            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateKeywordInfo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function



    ' ***************************************************************** '
    ' Name: UpdateAnnotation
    '
    ' Description: For each document written to briefcase db, obtain details of
    '              all ANNOTATION records from server. Write equivalent records
    '              to briefcase db
    '
    ' ***************************************************************** '


    Public Function UpdateAnnotation() As Integer

        Dim result As Integer = 0
        Dim vDocAnnotArray(,) As Object
        Dim vDocNumArray(,) As Object
        Dim vTempArray(,) As Object
        Dim lDocNum As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Store only those doc_keyword records that are related to documents records in briefcase db
            '
            ' Step 1.Read briefcase document recs into array
            ' Step 2.For each document rec, find any doc_keyword recs related to it on server db
            ' Step 3.For each doc_keyword found, store it in briefcase db

            m_sSQL = " SELECT doc_num FROM DOC_document"

            m_lReturn = CType(m_oLocalDB.SQLSelect(sSQL:=m_sSQL, sSQLName:="UpdateKeywordInfo", bStoredProcedure:=False, lNumberRecords:=-1, vResultArray:=vDocNumArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Step 2.
            ' document recs exist in briefcase db
            If Information.IsArray(vDocNumArray) Then

                ' read document folder

                For iInd As Integer = vDocNumArray.GetLowerBound(1) To vDocNumArray.GetUpperBound(1)

                    ' document.doc_num

                    lDocNum = CInt(vDocNumArray(0, iInd))

                    ' get annotation from server that matches this document in  briefcase db
                    m_sSQL = "SELECT doc_num, annotation_id, ann_text, user_name, create_date "
                    m_sSQL = m_sSQL & "FROM DOC_annotation WHERE doc_num = " & CStr(lDocNum)

                    m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="UpdateKeywordInfo", bStoredProcedure:=False, lNumberRecords:=-1, vResultArray:=vDocAnnotArray), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Step 3.
                    ' found annotation belonging to document in server db
                    If Information.IsArray(vDocAnnotArray) Then

                        ' copy the annotation recs to briefcase db

                        For iRow As Integer = vDocAnnotArray.GetLowerBound(1) To vDocAnnotArray.GetUpperBound(1)

                            ' ensure the row is not already in briefcase briefcase db

                            m_sSQL = " SELECT doc_num FROM DOC_annotation" &
                                     " WHERE doc_num = " & CStr(CInt(vDocAnnotArray(0, iRow)))

                            m_lReturn = CType(m_oLocalDB.SQLSelect(sSQL:=m_sSQL, sSQLName:="UpdateAnnotation", bStoredProcedure:=False, lNumberRecords:=-1, vResultArray:=vTempArray), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                            End If

                            'not a duplicate row so proceed to insert
                            If Not Information.IsArray(vTempArray) Then


                                m_sTmp = CStr(vDocAnnotArray(2, iRow))
                                m_lReturn = CType(ValidateSQL(m_sTmp), gPMConstants.PMEReturnCode)

                                vDocAnnotArray(2, iRow) = m_sTmp


                                m_sTmp = CStr(vDocAnnotArray(3, iRow))
                                m_lReturn = CType(ValidateSQL(m_sTmp), gPMConstants.PMEReturnCode)

                                vDocAnnotArray(3, iRow) = m_sTmp

                                ' format date create_date


                                vDocAnnotArray(4, iRow) = CDate(vDocAnnotArray(4, iRow)).ToString("MMM dd yyyy HH:MM")

                                m_sSQL = ""
                                m_sSQL = m_sSQL & " INSERT DOC_annotation (doc_num, annotation_id, ann_text,"
                                m_sSQL = m_sSQL & " user_name, create_date)"
                                m_sSQL = m_sSQL & " VALUES ("





                                m_sSQL = m_sSQL &
                                         CInt(vDocAnnotArray(0, iRow)) & m_cComma &
                                         CInt(vDocAnnotArray(1, iRow)) & m_cComma &
                                         m_cQuote & CStr(vDocAnnotArray(2, iRow)) & m_cQuote & m_cComma &
                                         m_cQuote & CStr(vDocAnnotArray(3, iRow)) & m_cQuote & m_cComma &
                                         m_cQuote & CStr(vDocAnnotArray(4, iRow)) & m_cQuote
                                m_sSQL = m_sSQL & ")"

                                ' insert into Local db
                                m_lReturn = CType(m_oLocalDB.SQLAction(sSQL:=m_sSQL, sSQLName:="UpdateAnnotation", bStoredProcedure:=False), gPMConstants.PMEReturnCode)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    result = gPMConstants.PMEReturnCode.PMFalse
                                End If
                            End If
                        Next iRow

                    End If

                Next iInd

            End If


            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateAnnotation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    ' Name: TransferPages
    '
    ' Description:  Get details of all pages that are to be copied
    '               to the laptop. Get Server directory path
    '
    ' ***************************************************************** '

    Public Function TransferPages(ByRef vResultArray(,) As Object, ByRef sDataPath As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            'Get page name, type and volume from page table
            m_sSQL = "SELECT page_name, page_type, volume_id FROM DOC_page "

            'hit briefcase db
            m_lReturn = CType(m_oLocalDB.SQLSelect(sSQL:=m_sSQL, sSQLName:="SELECTPAGE", bStoredProcedure:=False, lNumberRecords:=-1, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'No pages found so exit
            If Not Information.IsArray(vResultArray) Then
                Return result
            End If


            ' Get dir path of the pages stored on SERVER PC
            '           vResultArray(2, 0) = volume_id    builds the path and returns in sDatapath

            m_lReturn = CType(GetDataPath(CInt(vResultArray(2, 0)), sDataPath), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception




            ' file not found then carry on with next file
            If Information.Err().Number = 53 Then


            End If

            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="TransferPages", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function



    ' ***************************************************************** '
    ' Name: FolderRecursiveCopy
    '
    ' Description: Given  a folder this procedure recursively copies all child
    '              folders which are underneath it
    '
    ' ***************************************************************** '
    Public Function FolderRecursiveCopy(ByRef vFolderArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object


        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            ' Go thru list of folders being copied, writing a new folder for each one
            For iInd As Integer = vFolderArray.GetLowerBound(1) To vFolderArray.GetUpperBound(1)

                Application.DoEvents()

                'copy this record to briefcase db
                m_lReturn = CType(InsertFolderRec(vArray:=vFolderArray, iRow:=iInd), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Get the child folders for folder just copied
                'Construct SQL      'folder.folder_num = folder.parent_num

                m_sSQL = " SELECT folder_num, folder_name, parent_num, ex_code, folder_level,"
                m_sSQL = m_sSQL & " access_level, password, create_date FROM DOC_folder"

                m_sSQL = m_sSQL & " WHERE parent_num =  " & CStr(vFolderArray(0, iInd))

                'Hit the DB
                m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="COPYFOLDERS", bStoredProcedure:=False, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                'COPY THE CHILDREN RECURSIVELY
                If Information.IsArray(vResultArray) Then


                    m_lReturn = CType(FolderRecursiveCopy(vResultArray), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If


            Next iInd


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="FolderRecursiveCopy", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function


    ' ***************************************************************** '
    ' Name: UpdateLinkedDocs
    '
    ' Description: Copy to briefcase db the document records that were referenced via the LINK
    '              Note: The related folders and parent folders have to be inserted in hierarchical
    '              order  becasue of the integrity structure of the db, otherwise the document
    '              cannot be inserted
    '
    ' ***************************************************************** '
    Public Function UpdateLinkedDocs() As Integer

        Dim result As Integer = 0
        Dim lFolderNum As Integer
        Dim vResultArray(,) As Object
        Dim vFolderArray(,) As Object
        Dim vDocumentArray(,) As Object
        Dim vFolderList() As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'get documents recs which have a link
            m_sSQL = "SELECT doc_num, link, folder_num FROM DOC_document WHERE link > 0"

            m_lReturn = CType(m_oLocalDB.SQLSelect(sSQL:=m_sSQL, sSQLName:="SELECTDOCUMENT", bStoredProcedure:=False, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' no linked documents, exit
            If Not Information.IsArray(vResultArray) Then
                Return result
            End If

            'linked recs found


            For iInd As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

                Application.DoEvents()

                'intialise array to keep track of folders to create
                ReDim vFolderList(0)

                ' check if the document rec referenced exists in briefcase db (& = document.link)

                m_sSQL = "SELECT doc_num FROM DOC_document WHERE doc_num = " & CInt(vResultArray(1, iInd))

                ' hit briefcase db
                m_lReturn = CType(m_oLocalDB.SQLSelect(sSQL:=m_sSQL, sSQLName:="SELECTDOCUMENT", bStoredProcedure:=False, vResultArray:=vDocumentArray), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If

                ' documents are not briefcase so get from server. If exist in briefcase, will be picked automatically
                If Not Information.IsArray(vDocumentArray) Then

                    ' document rec referenced exists in Server  (& = document.link)
                    m_sSQL = ""
                    m_sSQL = m_sSQL & "SELECT doc_num, folder_num FROM DOC_document "

                    m_sSQL = m_sSQL & "WHERE doc_num = " & CStr(CInt(vResultArray(1, iInd)))

                    ' hit server db
                    m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="SELECTDOCUMENT", bStoredProcedure:=False, vResultArray:=vDocumentArray), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If Information.IsArray(vDocumentArray) Then


                        lFolderNum = CInt(vDocumentArray(1, 0))  ' document.foldernum from result above

                        ' get folder for this document (and parent folders if they don't exist locally)
                        ' where link referenced document resides in,  keep track in order to create them
                        ' in db structure order before inserting the documnet rec into briefcase db

                        m_sSQL = ""
                        m_sSQL = m_sSQL & "SELECT folder_num, parent_num FROM DOC_folder "
                        m_sSQL = m_sSQL & "WHERE folder_num =  " & CStr(lFolderNum)

                        'Hit the sever db
                        m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="SELECTFOLDER", bStoredProcedure:=False, vResultArray:=vFolderArray), gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                        End If


                        If Information.IsArray(vFolderArray) Then
                            ' update the folder list with current child folder found (0,0 = folder_num)

                            vFolderList(vFolderList.GetUpperBound(0)) = CInt(vFolderArray(0, 0))
                            ' preserve the contents of the array and expand it by one for the next update
                            ReDim Preserve vFolderList(vFolderList.GetUpperBound(0) + 1)
                            ' store folder.PARENT_NUM

                            lFolderNum = CInt(vFolderArray(1, 0))

                            ' whilst there exists a parent for this folder      root folder will be 0
                            Do While lFolderNum > 0

                                ' check if this folder is already on briefcase db then we don't need to
                                ' update from server
                                m_sSQL = ""
                                m_sSQL = m_sSQL & "SELECT folder_num, parent_num FROM DOC_folder "
                                m_sSQL = m_sSQL & "WHERE folder_num =  " & CStr(lFolderNum)
                                'Hit the briefcase db
                                m_lReturn = CType(m_oLocalDB.SQLSelect(sSQL:=m_sSQL, sSQLName:="SELECTFOLDER", bStoredProcedure:=False, vResultArray:=vFolderArray), gPMConstants.PMEReturnCode)

                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    result = gPMConstants.PMEReturnCode.PMFalse
                                End If

                                ' folder exists in briefcase db
                                If Information.IsArray(vFolderArray) Then
                                    Exit Do
                                End If

                                ' not in briefcase db so get it from server
                                m_sSQL = ""
                                m_sSQL = m_sSQL & "SELECT folder_num, parent_num FROM DOC_folder "
                                m_sSQL = m_sSQL & "WHERE folder_num =  " & CStr(lFolderNum)
                                'Hit the server db
                                m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="SELECTFOLDER", bStoredProcedure:=False, vResultArray:=vFolderArray), gPMConstants.PMEReturnCode)

                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    result = gPMConstants.PMEReturnCode.PMFalse
                                End If

                                If Information.IsArray(vFolderArray) Then
                                    ' update the folder list with current child folder found

                                    vFolderList(vFolderList.GetUpperBound(0)) = CInt(vFolderArray(0, 0))
                                    ' preserve the contents of the array and expand it by one for the next update
                                    ReDim Preserve vFolderList(vFolderList.GetUpperBound(0) + 1)
                                    ' store folder.PARENT_NUM

                                    lFolderNum = CInt(vFolderArray(1, 0))

                                End If

                            Loop

                        End If  'IsArray


                        '
                        ' INSERT FOLDER RECORD
                        '

                        ' Get from server and insert the folder records into briefcase db in correct order
                        ' (they were stored  backwards i.e. last folder was first in array))
                        For iCount As Integer = vFolderList.GetUpperBound(0) To vFolderList.GetLowerBound(0) Step -1

                            ' check if folder already inserted in  briefcase db
                            m_sSQL = " SELECT folder_num, folder_name, parent_num, ex_code, folder_level,"
                            m_sSQL = m_sSQL & " access_level, password, create_date FROM DOC_folder"
                            m_sSQL = m_sSQL & " WHERE folder_num = " & CStr(vFolderList(iCount))
                            ' hit briefcase db
                            m_lReturn = CType(m_oLocalDB.SQLSelect(sSQL:=m_sSQL, sSQLName:="SELECTFOLDER", bStoredProcedure:=False, vResultArray:=vFolderArray), gPMConstants.PMEReturnCode)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                            End If

                            ' not in briefcase db so it's safe to insert from server
                            If Not Information.IsArray(vFolderArray) Then

                                m_sSQL = " SELECT folder_num, folder_name, parent_num, ex_code, folder_level,"
                                m_sSQL = m_sSQL & " access_level, password, create_date"
                                m_sSQL = m_sSQL & " FROM DOC_folder WHERE folder_num = " & CStr(vFolderList(iCount))
                                ' hit server db
                                m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="SELECTFOLDER", bStoredProcedure:=False, vResultArray:=vFolderArray), gPMConstants.PMEReturnCode)

                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    result = gPMConstants.PMEReturnCode.PMFalse
                                End If

                                ' insert folder rec into briefcase db
                                If Information.IsArray(vFolderArray) Then
                                    '  only one row will be returned in the array (row 0)

                                    m_lReturn = CType(InsertFolderRec(vArray:=vFolderArray, iRow:=0), gPMConstants.PMEReturnCode)

                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        result = gPMConstants.PMEReturnCode.PMFalse
                                    End If

                                End If

                            End If

                        Next iCount

                    End If

                    ' The hierachy of folders have been inserted it is now safe to insert the
                    ' document rec which has a link referenced to it

                    '
                    ' INSERT LINKED REFERENCED DOCUMENT RECORD into briefcase db
                    '

                    ' get link referenced record from server and insert to briefcase briefcase db
                    m_sSQL = ""
                    m_sSQL = m_sSQL & " SELECT doc_num, folder_num, doc_name, ex_code, doc_type, "
                    m_sSQL = m_sSQL & " access_level , Password, create_date, zipped, link"

                    m_sSQL = m_sSQL & " FROM DOC_document WHERE doc_num = " & CStr(CInt(vResultArray(1, iInd)))

                    m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="SELECTFOLDER", bStoredProcedure:=False, vResultArray:=vDocumentArray), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If Information.IsArray(vDocumentArray) Then

                        ' validate doc_name string

                        m_sTmp = CStr(vDocumentArray(2, 0))
                        m_lReturn = CType(ValidateSQL(m_sTmp), gPMConstants.PMEReturnCode)

                        vDocumentArray(2, 0) = m_sTmp


                        m_sTmp = CStr(vDocumentArray(3, 0))
                        m_lReturn = CType(ValidateSQL(m_sTmp), gPMConstants.PMEReturnCode)

                        vDocumentArray(3, 0) = m_sTmp


                        m_sTmp = CStr(vDocumentArray(4, 0))
                        m_lReturn = CType(ValidateSQL(m_sTmp), gPMConstants.PMEReturnCode)

                        vDocumentArray(4, 0) = m_sTmp


                        m_sTmp = CStr(vDocumentArray(6, 0))
                        m_lReturn = CType(ValidateSQL(m_sTmp), gPMConstants.PMEReturnCode)

                        vDocumentArray(6, 0) = m_sTmp


                        m_sTmp = CStr(vDocumentArray(8, 0))
                        m_lReturn = CType(ValidateSQL(m_sTmp), gPMConstants.PMEReturnCode)

                        vDocumentArray(8, 0) = m_sTmp


                        ' format date field create_date


                        vDocumentArray(7, 0) = CDate(vDocumentArray(7, 0)).ToString("MMM dd yyyy HH:MM")

                        m_sSQL = ""
                        m_sSQL = m_sSQL & " INSERT DOC_document(doc_num , folder_num, doc_name, ex_code,"
                        m_sSQL = m_sSQL & " doc_type, access_level, password, create_date,"
                        m_sSQL = m_sSQL & " zipped, link)"
                        m_sSQL = m_sSQL & " VALUES ("










                        m_sSQL = m_sSQL &
                                 CInt(vDocumentArray(0, 0)) & m_cComma &
                                 CInt(vDocumentArray(1, 0)) & m_cComma &
                                 m_cQuote & CStr(vDocumentArray(2, 0)) & m_cQuote & m_cComma &
                                 m_cQuote & CStr(vDocumentArray(3, 0)) & m_cQuote & m_cComma &
                                 m_cQuote & CStr(vDocumentArray(4, 0)) & m_cQuote & m_cComma &
                                 CInt(vDocumentArray(5, 0)) & m_cComma &
                                 m_cQuote & CStr(vDocumentArray(6, 0)) & m_cQuote & m_cComma &
                                 m_cQuote & CStr(vDocumentArray(7, 0)) & m_cQuote & m_cComma &
                                 m_cQuote & CStr(vDocumentArray(8, 0)) & m_cQuote & m_cComma &
                                 CInt(vDocumentArray(9, 0))

                        m_sSQL = m_sSQL & ")"
                        ' insert into Local db
                        m_lReturn = CType(m_oLocalDB.SQLAction(sSQL:=m_sSQL, sSQLName:="UpdateDocuments", bStoredProcedure:=False), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                        End If


                    End If  ' IsArray(vDocumentArray)

                End If  ' IsArray

            Next iInd


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateLinkedDocs", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    'developer guide no.101
    Private Function IsThere(ByRef cObjects As Object, ByRef sName As String) As Boolean
        Dim result As Boolean = False
        Dim oObject As Object




        For Each oObject2 As Object In cObjects
            oObject = oObject2


            If oObject.Name.ToUpper() = sName.ToUpper() Then
                Return True
            End If

        Next oObject2

        oObject = Nothing

        Return result

    End Function



    ' ***************************************************************** '
    ' Name: ProcessEnd(lProcessID as long)
    '
    ' Description:
    ' this loops around until a process whose id has been passed across
    ' is no longer existent
    ' ***************************************************************** '
    Public Sub ProcessEnd(ByRef lProcessID As Integer)
        Dim lCnt As Integer

        Try

            'Give the process a chance to start
            Pause(2)

            Do

                Interaction.AppActivate(CStr(lProcessID))

                Pause(1)

                lCnt += 1

                If (lCnt / 900) = Math.Floor(lCnt / 900) Then
                    'If this has been ongoing for more than 15 minutes we'd
                    'better warn the user
                    '            MsgBox "Process (" & lProcessID & " )still exists"
                End If

            Loop

        Catch



            'Process has ended so we can go back
            Exit Sub
        End Try


    End Sub


    ' ***************************************************************** '
    ' Name: Pause
    '
    ' Description:
    ' Waits for seconds seconds
    '
    ' ***************************************************************** '
    Private Sub Pause(ByRef Seconds As Single)


        Dim StartTime As Single = DateTime.Now.TimeOfDay.TotalSeconds



        While DateTime.Now.TimeOfDay.TotalSeconds < StartTime + Seconds

            Application.DoEvents()

        End While


    End Sub


    ' ******************************************************************************* '
    '
    '   GetSBOClient: function that gets a Folder using the external code and
    '
    ' ******************************************************************************* '
    Public Function GetSBOClient(ByRef sFolderName As String, ByRef lFolderLevel As Integer, ByRef sExCode As String, ByRef lFolderNum As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            If sFolderName.IndexOf("'"c) + 1 Then
                sFolderName = sFolderName.Replace("'", "''")
            End If

            ' if client folder, search using the unique folder name provided by SBO
            If lFolderLevel = DOCDrawer Then

                'Find the client folder, looking in the branch that the user is in first.
                sSQL = ""
                sSQL = sSQL & "SELECT " & Strings.Chr(13) & Strings.Chr(10)
                sSQL = sSQL & "    f1.ex_code, " & Strings.Chr(13) & Strings.Chr(10)
                sSQL = sSQL & "    f1.folder_num" & Strings.Chr(13) & Strings.Chr(10)
                sSQL = sSQL & "FROM doc_folder f1" & Strings.Chr(13) & Strings.Chr(10)
                sSQL = sSQL & "JOIN doc_folder f0" & Strings.Chr(13) & Strings.Chr(10)
                sSQL = sSQL & "    ON f0.folder_num = f1.parent_num" & Strings.Chr(13) & Strings.Chr(10)
                sSQL = sSQL & "LEFT JOIN party p" & Strings.Chr(13) & Strings.Chr(10)
                sSQL = sSQL & "    ON p.party_cnt = f1.ex_code" & Strings.Chr(13) & Strings.Chr(10)
                sSQL = sSQL & "    AND p.source_id = f0.ex_code" & Strings.Chr(13) & Strings.Chr(10)
                sSQL = sSQL & "WHERE f1.folder_name = '" & sFolderName & "'" & Strings.Chr(13) & Strings.Chr(10)
                sSQL = sSQL & "AND f1.folder_level = 1" & Strings.Chr(13) & Strings.Chr(10)
                sSQL = sSQL & "ORDER BY" & Strings.Chr(13) & Strings.Chr(10)
                sSQL = sSQL & "    ISNULL(p.source_id, -1) DESC," & Strings.Chr(13) & Strings.Chr(10)
                sSQL = sSQL & "    f1.ex_code" & Strings.Chr(13) & Strings.Chr(10)

            End If

            ' if policy or claim folder etc
            If lFolderLevel = DOCFolder Then

                ' DME's folder name is different for same client in SBO
                ' DME's folder name has description concatenated, hence LIKE operator used
                sSQL = ""
                sSQL = sSQL & " SELECT ex_code, folder_num FROM DOC_folder"
                sSQL = sSQL & " WHERE folder_name LIKE '" & sFolderName & "%'"
                sSQL = sSQL & " AND folder_level = " & CStr(lFolderLevel)

            End If

            ' hit DB
            m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="DRAWER EX_CODE", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Information.IsArray(vResultArray) Then


                sExCode = CStr(vResultArray(0, 0))

                lFolderNum = CInt(vResultArray(1, 0))

            Else
                ' not found
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception





            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSBOClientFAILED", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSBOClient", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ******************************************************************************* '
    '
    '   GetDrawerCabExCode: Get drawer and cabinet external codes using the drawer details
    '
    ' ******************************************************************************* '
    Public Function GetDrawerCabExCode(ByRef lDrawerNum As Integer, ByRef sDrawerExCode As String, ByRef sDrawerName As String, ByRef sCabExCode As String) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' c = child, p = parent
            sSQL = ""
            sSQL = sSQL & " SELECT c.ex_code, c.folder_name, p.ex_code"
            sSQL = sSQL & " FROM DOC_folder c, DOC_folder p"
            sSQL = sSQL & " WHERE c.folder_num = " & CStr(lDrawerNum)
            sSQL = sSQL & " AND c.parent_num = p.folder_num"


            m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="DRAWER_CAB_DETAILS", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If Information.IsArray(vResultArray) Then


                sDrawerExCode = CStr(vResultArray(0, 0))

                sDrawerName = CStr(vResultArray(1, 0))

                sCabExCode = CStr(vResultArray(2, 0))

            Else
                ' not found
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDrawerCabExCodeFAILED", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDrawerCabExCode", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function





    ' ******************************************************************************* '
    '
    '   GetParentFolder: Get the parent of a child folder
    '
    ' ******************************************************************************* '
    Public Function GetParentFolder(ByRef lFolderLevel As Integer, ByRef lFolderNum As Integer, ByRef lParentNum As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = ""
            sSQL = sSQL & " SELECT parent_num FROM DOC_folder"
            sSQL = sSQL & " WHERE folder_num = " & CStr(lFolderNum)
            sSQL = sSQL & " AND folder_level = " & CStr(lFolderLevel)

            ' hit DB
            m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="DRAWER EX_CODE", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Information.IsArray(vResultArray) Then


                lParentNum = CInt(vResultArray(0, 0))

            Else
                ' not found
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception





            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetParentFolderFAILED", vApp:=ACApp, vClass:=ACClass, vMethod:="GetParentFolder", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDocInfo
    '
    ' Description: For a given document, this function returns
    '              the document name and folder number in which it belongs to
    '
    ' ***************************************************************** '
    Public Function GetDocInfo(ByRef lDocNum As Integer, ByRef sDocName As String, ByRef lFolderNum As Integer, ByRef sDocExCode As String) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get page names and volume
            m_sSQL = "SELECT doc_name, folder_num, ex_code FROM DOC_document WHERE doc_num = " & lDocNum

            'hit DB
            m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="GetDocInfo", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'No records ?
            If Not Information.IsArray(vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get document details.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocInfo", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result

            Else

                'return the doc name, folder num and external code for document

                sDocName = CStr(vResultArray(0, 0)).Trim()

                lFolderNum = CInt(vResultArray(1, 0))

                sDocExCode = CStr(vResultArray(2, 0)).Trim()

            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocInfo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function GetFileAndFolderAccess() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object



        result = gPMConstants.PMEReturnCode.PMTrue

        m_sSQL = "spu_DOC_select_access_levels"

        'Hit the DB
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="GETFILEANDFOLDERACCESS", bStoredProcedure:=True, lNumberRecords:=6, vResultArray:=vResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMTrue
        End If

        ' file access

        g_iFileCopyLevel = CInt(vResultArray(0, 0))

        g_iFileDeleteLevel = CInt(vResultArray(1, 0))

        g_iFileMoveLevel = CInt(vResultArray(2, 0))

        'folder access

        g_iFolderCopyLevel = CInt(vResultArray(3, 0))

        g_iFolderDeleteLevel = CInt(vResultArray(4, 0))

        g_iFolderMoveLevel = CInt(vResultArray(5, 0))


        Return result

    End Function


    Public Function UpdateFileAndFolderAccess() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="v1", vValue:=g_iFileCopyLevel, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="v2", vValue:=g_iFileDeleteLevel, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="v3", vValue:=g_iFileMoveLevel, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="v4", vValue:=g_iFolderCopyLevel, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="v5", vValue:=g_iFolderDeleteLevel, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="v6", vValue:=g_iFolderMoveLevel, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_sSQL = "spu_DOC_update_access_levels"

            'Hit the DB
            m_lReturn = m_oDatabase.SQLAction(sSQL:=m_sSQL, sSQLName:="SETFILEANDFOLDERACCESS", bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to get file and folder access", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateFileAndFolderAccess", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    'MS 10/05/01
    ' ***************************************************************** '
    ' Name: CreateEventInSBO
    '
    ' Description: Creates an event in SBO when an archive of a document
    '               is performed
    '
    ' ***************************************************************** '

    Public Function CreateEventInSBO(ByRef lEventCnt As Integer, ByVal lPartyCnt As Integer, ByVal vInsuranceFolderCnt As Object, ByVal vInsuranceFileCnt As Object, ByVal vClaimCnt As Object, ByVal lDocNum As Integer, ByVal vOldAddressCnt As Object, ByVal vNewAddressCnt As Object, ByVal vCampaignId As Object, ByVal vDocumentTypeId As Object, ByVal vReportTypeId As Object, ByVal lEventTypeId As Integer, ByVal dtEventDate As Date, ByVal sDescription As String) As Integer

        Dim result As Integer = 0
        Dim oEvent As bSIREvent.Business
        Dim bDestroyBusiness As Boolean


        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            If oEvent Is Nothing Then

                oEvent = New bSIREvent.Business()

                m_lReturn = CType(CType(oEvent, SSP.S4I.Interfaces.IBusiness).Initialise(sUsername:=g_sUsername, sPassword:=g_sPassword.Value, iUserID:=g_iUserID, iSourceID:=g_iSourceID, iLanguageID:=g_iLanguageID, iCurrencyID:=g_iCurrencyID, iLogLevel:=g_iLogLevel, sCallingAppName:=ACApp), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the event object", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEvent", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    oEvent = Nothing

                    Return result

                End If

                bDestroyBusiness = True

            End If

            'WR77 Documaster Enhancements START
            If lDocNum <> 0 Then
                m_lReturn = CType(oEvent.DirectAdd(vEventCnt:=lEventCnt, vPartyCnt:=lPartyCnt, vInsuranceFolderCnt:=vInsuranceFolderCnt, vInsuranceFileCnt:=vInsuranceFileCnt, vClaimCnt:=vClaimCnt, vDocumentCnt:=lDocNum, vOldAddressCnt:=vOldAddressCnt, vNewAddressCnt:=vNewAddressCnt, vCampaignId:=vCampaignId, vDocumentType:=vDocumentTypeId, vReportType:=vReportTypeId, vEventType:=lEventTypeId, vUserId:=g_iUserID, vEventDate:=dtEventDate, vDescription:=sDescription), gPMConstants.PMEReturnCode)
            Else
                m_lReturn = CType(oEvent.DirectAdd(vEventCnt:=lEventCnt, vPartyCnt:=lPartyCnt, vInsuranceFolderCnt:=vInsuranceFolderCnt, vInsuranceFileCnt:=vInsuranceFileCnt, vClaimCnt:=vClaimCnt, vOldAddressCnt:=vOldAddressCnt, vNewAddressCnt:=vNewAddressCnt, vCampaignId:=vCampaignId, vDocumentType:=vDocumentTypeId, vReportType:=vReportTypeId, vEventType:=lEventTypeId, vUserId:=g_iUserID, vEventDate:=dtEventDate, vDescription:=sDescription), gPMConstants.PMEReturnCode)
                'vDocumentCnt:=lDocNum,
            End If
            'WR77 Documaster Enhancements END
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add the Event in SBO", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEvent", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                oEvent.Dispose()
                oEvent = Nothing

                Return result
            End If


            If bDestroyBusiness Then
                oEvent.Dispose()
                oEvent = Nothing
            End If


            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error in attempting to add an event in SBO", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEventInSBO", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: CopyEventInSBO
    '
    ' Description: Copies an event in SBO to indicate a reprint or email
    ' ***************************************************************** '
    Public Function CopyEventInSBO(ByRef lEventCnt As Integer, ByVal lDocNum As Integer, ByVal dtEventDate As Date, ByVal sDescriptionPrefix As String) As Integer

        Dim result As Integer = 0
        Dim oEvent As bSIREvent.Business
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "CopyEventInSBO"





        result = gPMConstants.PMEReturnCode.PMTrue

        ' Try to get existing object
        Try
            oEvent = System.Runtime.InteropServices.Marshal.GetActiveObject("bSIREvent.Business")

        Catch
        End Try



        ' If not, create one
        If oEvent Is Nothing Then
            oEvent = New bSIREvent.Business()
            lReturn = oEvent.Initialise(sUsername:=g_sUsername, sPassword:=g_sPassword.Value, iUserID:=CShort(g_iUserID), iSourceID:=CShort(g_iSourceID), iLanguageID:=CShort(g_iLanguageID), iCurrencyID:=CShort(g_iCurrencyID), iLogLevel:=CShort(g_iLogLevel), sCallingAppName:=ACApp)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("oEvent.Initialise", "Failed to initialise the event object")
            End If
        End If

        ' Copy the document (directly return result)
        result = oEvent.CopyEventByDocumentCnt(v_lDocumentCnt:=lDocNum, v_dtEventDate:=dtEventDate, v_sDescriptionPrefix:=sDescriptionPrefix, r_lEventCnt:=lEventCnt)

        GoTo Finally_Renamed
Catch_Renamed:
        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sUsername:=g_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)

Finally_Renamed:
        ' Shutdown business, if we need to
        If Not (oEvent Is Nothing) Then
            oEvent.Dispose()
            oEvent = Nothing
        End If

        Return result



        Return result
    End Function


    ' ***************************************************************** '
    ' Name: GetNodeExCode
    '
    ' Description: This is a wrapper to the function in the miscellaneous
    ' class.
    '
    ' ***************************************************************** '
    Public Function GetNodeExCode(ByRef iNodeType As Integer, ByRef lNodeNum As Integer, ByRef sExCode As String) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get the supplied folder name
            m_lReturn = CType(m_oMiscellaneous.GetNodeExCode(iNodeType:=iNodeType, lNodeNum:=lNodeNum, sExCode:=sExCode), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetNodeParent", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetInsuranceFolderCnt
    '
    ' Description: This is a wrapper to the function in the miscellaneous
    ' class.
    '
    ' ***************************************************************** '
    Public Function GetInsuranceFolderCnt(ByRef vClaimCnt As Object, ByRef vInsuranceFolderCnt As Object) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get the supplied folder name
            m_lReturn = CType(m_oMiscellaneous.GetInsuranceFolderCnt(vClaimCnt:=vClaimCnt, vInsuranceFolderCnt:=vInsuranceFolderCnt), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetInsuranceFolderCnt", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class

