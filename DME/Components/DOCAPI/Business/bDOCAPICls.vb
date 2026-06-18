Option Strict Off
Option Explicit On
Imports SSP.Shared
Imports Sspi.Common.Aws.S3

<System.Runtime.InteropServices.ProgId("API_NET.API")>
Public NotInheritable Class API
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
    ' JH081098 - data merge changes
    ' DN270802 - Changed embedded SQL to reflect table name changes
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "API"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)
    Private m_sSQL As String = ""
    Dim asa As String = ""


    ' Database Class (Private)
#If PD_EARLYBOUND = 1 Then

	Private m_oDatabase As DPMDAO.Database
#Else
    Private m_oDatabase As dPMDAO.Database
#End If

    ' Miscellaneous class
    Private m_oMisc As bDOCAPI.Miscellaneous

    'Business objects
    Private m_oFolder As bDOCFolder.Form
    Private m_oDocument As bDOCDocument.Form
    Private m_oDocInfo As bDOCDocInfo.Form
    Private m_oAnnotation As bDOCAnnotation.Form
    Private m_oHistory As bDOCHistory.Form
    Private m_oPage As bDOCPage.Form

    'Zipper
    'Private m_oZipper  As bSIRZipper.Zipper

    'Error logging object if called from PMB
    Private m_oPMBLog As Object

    'For login messages to PMB log
    Private m_sLogMess() As String

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Effective
    Private m_dtEffectiveDate As Date

    ' Primary Keys to work with
    ' Source ID
    Private m_iSourceID As Integer

    'Document Number
    Private m_lDocNum As Integer 'DN 07/12/00
    'For Insurance or Claim Folder
    Private m_bInsuranceNum As Boolean

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    ' ***************************************************************** '
    ' Standard Product Family Constant (Read Only)
    ' ***************************************************************** '
    Public WriteOnly Property InsuranceNum() As Boolean
        Set(ByVal Value As Boolean)

            m_bInsuranceNum = Value

        End Set
    End Property
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
    Public ReadOnly Property DocNumber() As Integer
        Get 'DN 07/12/00

            Return m_lDocNum

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
    Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As dPMDAO.Database = Nothing) As Integer

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

            If (Not Informations.IsNothing(vDatabase)) And (Informations.IsReference(vDatabase)) Then
                ' Yes, so use it.
                m_oDatabase = vDatabase

                ' Do NOT Close Database in Terminate() method
                m_bCloseDatabase = False
            Else
                ' NO, Create new instance of the database object
#If PD_EARLYBOUND = 1 Then

				Set m_oDatabase = New DPMDAO.Database
#Else
                m_oDatabase = New dPMDAO.Database()
#End If

                ' Open the Database
                m_lReturn = CType(gPMComponentServices.NewDatabase(g_sUsername, g_iSourceID, g_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFDocumaster, r_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Close Database in Terminate() method
                m_bCloseDatabase = True
            End If


            ' Set the Effective Date to NOW
            m_dtEffectiveDate = DateTime.Now

            'Get the folder business object reference for writing to the DB
            m_oFolder = New bDOCFolder.Form()

            m_lReturn = m_oFolder.Initialise(sUsername:=g_sUsername, sPassword:=g_sPassword.Value, iUserID:=g_iUserID, iSourceID:=g_iSourceID, iLanguageID:=g_iLanguageID, iCurrencyID:=g_iCurrencyID, iLogLevel:=g_iLogLevel, sCallingAppName:=g_sCallingAppName, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the document business object reference for writing to the DB
            m_oDocument = New bDOCDocument.Form()

            m_lReturn = m_oDocument.Initialise(sUsername:=g_sUsername, sPassword:=g_sPassword.Value, iUserID:=g_iUserID, iSourceID:=g_iSourceID, iLanguageID:=g_iLanguageID, iCurrencyID:=g_iCurrencyID, iLogLevel:=g_iLogLevel, sCallingAppName:=g_sCallingAppName, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the docinfo business object reference for writing to the DB
            m_oDocInfo = New bDOCDocInfo.Form()

            m_lReturn = m_oDocInfo.Initialise(sUsername:=g_sUsername, sPassword:=g_sPassword.Value, iUserID:=g_iUserID, iSourceID:=g_iSourceID, iLanguageID:=g_iLanguageID, iCurrencyID:=g_iCurrencyID, iLogLevel:=g_iLogLevel, sCallingAppName:=g_sCallingAppName, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the Annotation business object reference for writing to the DB
            m_oAnnotation = New bDOCAnnotation.Form()

            m_lReturn = m_oAnnotation.Initialise(sUserName:=g_sUsername, sPassword:=g_sPassword.Value, iUserID:=g_iUserID, iSourceID:=g_iSourceID, iLanguageID:=g_iLanguageID, iCurrencyID:=g_iCurrencyID, iLogLevel:=g_iLogLevel, sCallingAppName:=g_sCallingAppName, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the history business object reference for writing to the DB
            m_oHistory = New bDOCHistory.Form()

            m_lReturn = m_oHistory.Initialise(sUsername:=g_sUsername, sPassword:=g_sPassword.Value, iUserID:=g_iUserID, iSourceID:=g_iSourceID, iLanguageID:=g_iLanguageID, iCurrencyID:=g_iCurrencyID, iLogLevel:=g_iLogLevel, sCallingAppName:=g_sCallingAppName, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the page business object reference for writing to the DB
            m_oPage = New bDOCPage.Form()

            m_lReturn = m_oPage.Initialise(sUsername:=g_sUsername, sPassword:=g_sPassword.Value, iUserID:=g_iUserID, iSourceID:=g_iSourceID, iLanguageID:=g_iLanguageID, iCurrencyID:=g_iCurrencyID, iLogLevel:=g_iLogLevel, sCallingAppName:=g_sCallingAppName, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get instance of miscellaneous class
            m_oMisc = New bDOCAPI.Miscellaneous()

            m_lReturn = m_oMisc.Initialise(v_sUsername:=g_sUsername, v_sPassword:=g_sPassword.Value, v_iUserID:=g_iUserID, v_iSourceID:=g_iSourceID, v_iLanguageID:=g_iLanguageID, v_iCurrencyID:=g_iCurrencyID, v_iLogLevel:=g_iLogLevel, v_sCallingAppName:=g_sCallingAppName, vDatabase:=m_oDatabase, vHistory:=m_oHistory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'instance zipper
            '    Set m_oZipper = New bSIRZipper.Zipper

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                m_oFolder = Nothing
                m_oDocument = Nothing
                m_oDocInfo = Nothing
                m_oAnnotation = Nothing
                m_oHistory = Nothing
                m_oPage = Nothing
                m_oMisc = Nothing
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub

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
            bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        'bPMFunc.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    ' Name: Add (Public)
    '
    ' Description: This section performs an ADD request which is to add
    ' a document. It is supplied an array of external folder codes
    ' determining the destination folder, along with the document details.
    '
    ' Firstly it goes thru the folder tree, creating each level if not
    ' present (using the external code as the name).
    '
    ' It then creates the document, docinfo, page records, adds any
    ' supplied keywords, annotations etc and copies the physical file
    ' over.
    '
    ' ***************************************************************** '
    Public Function Add(ByRef vIndexArray(,) As Object, ByRef sDocName As String, ByRef sFilename As String, ByRef sDocType As String, ByRef sPageType As String, ByRef iAccessLevel As Integer, ByRef sUsername As String, ByRef sKeywords() As String, Optional ByRef sAnnotation As String = "", Optional ByRef oPMBLog As Object = Nothing, Optional ByRef vDocumentTemplateID As Object = Nothing, Optional ByRef bVisibleFromWeb As Boolean = False) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetNextPageName"

        Dim lFolderNum, lParentNum As Integer
        Dim bCreateAllFolders As Boolean
        Dim iFolderLevel As Integer
        Dim lDocNum As Integer
        Dim sPageName As New StringsHelper.FixedLengthString(15)
        Static sDataPath As String = ""
        Dim sTmpDocRef As String = ""
        Dim dNowDate As Date
        Dim sTmpPageName, sTmp, sInsuranceFileCnt, sPartyCnt, sCompanyID As String
        Dim bStartTrans As Boolean
        Dim sFolderName As String = "" ''PN 66472
        sTmpPageName = ""
        sInsuranceFileCnt = ""
        sPartyCnt = ""
        sCompanyID = ""
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            'If passed empty array, fine, just go back
            If Not Informations.IsArray(vIndexArray) Then
                Return result
            End If

            'If reference to PMBLog object supplied, store it. The presence of
            'this will determine if we are called from PMB or not.
            m_oPMBLog = oPMBLog

            'set access level
            g_iAccessLevel = iAccessLevel

            'Commence database updates
            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("BeginTrans()", "Function failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            bStartTrans = True

            'This gets set to true as soon as a folder is created, as obviously
            'all subsequent child folders will need to be created too, so there
            'is no need to check if they already exist
            bCreateAllFolders = False

            'First folder we create will be root node, ie parent = 0 (or cabinet
            'for backward compatibility)
            lParentNum = 0
            iFolderLevel = DOCCabinet


            'loop thru each index level
            For i As Integer = vIndexArray.GetLowerBound(1) To vIndexArray.GetUpperBound(1)

                If Not bCreateAllFolders Then

                    lFolderNum = 0


                    If CStr(vIndexArray(0, i)) <> "" Then
                        'See if folder exists and if it does return the number and name

                        m_lReturn = CType(FolderExCodeExists(sExCode:=ToSafeString(CStr(vIndexArray(0, i))), iFolderLevel:=iFolderLevel, lFolderNum:=lFolderNum, lParentNum:=lParentNum, sInsuranceRef:=CStr(vIndexArray(1, i))), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                            RaiseError("FolderExCodeExists", "sExCode:=" & ToSafeString(CStr(vIndexArray(0, i))), gPMConstants.PMELogLevel.PMLogError)
                        End If
                    Else

                        m_lReturn = CType(FolderNameExists(sFolderName:=ToSafeString(CStr(vIndexArray(1, i))), lFolderNum:=lFolderNum, lParentNum:=lParentNum), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                            RaiseError("FolderNameExists", "sFolderName:=" & ToSafeString(CStr(vIndexArray(1, i))), gPMConstants.PMELogLevel.PMLogError)
                        End If

                    End If

                    If lFolderNum <> 0 Then
                        'Folder exists so store it as it will be the parent of the next folder we add.
                        lParentNum = lFolderNum

                    Else

                        'Add the new folder
                        m_lReturn = CType(AddFolder(iFolderLevel, vIndexArray, i, lParentNum, lFolderNum), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            RaiseError("AddFolder", "Function failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                        'Store the folder num, which will be the parent of the next folder we add
                        lParentNum = lFolderNum

                        'all subsequent folders in index array will need to be added.
                        bCreateAllFolders = True

                    End If

                    If iFolderLevel = 2 Then ''PN:66472

                        sFolderName = ToSafeString(CStr(vIndexArray(1, 2))) ''PN: 66472
                    Else
                        sFolderName = ""
                    End If

                Else

                    'Add the new folder
                    m_lReturn = CType(AddFolder(iFolderLevel, vIndexArray, i, lParentNum, lFolderNum), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError("AddFolder", "Function failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    'Store the folder num, which will be the parent of the next folder
                    'we add
                    lParentNum = lFolderNum

                End If

                'DJM 18/09/2003
                Select Case iFolderLevel
                    Case DOCFolderLevelBranch

                        sCompanyID = CStr(vIndexArray(0, i))
                    Case DOCFolderLevelClient

                        sPartyCnt = CStr(vIndexArray(0, i))
                    Case DOCFolderLevelPolicy

                        If CStr(vIndexArray(1, i)) <> "GENERAL" And CStr(vIndexArray(1, i)) <> "COMPLAINTS" Then

                            sInsuranceFileCnt = CStr(vIndexArray(0, i))
                        End If
                End Select

                'next level
                iFolderLevel += 1

            Next i

            'DJM 18/09/2003 : Make sure that only one document folder exists for this policy.
            If sInsuranceFileCnt <> "" Then
                m_lReturn = CType(m_oMisc.MergeFolders(sInsuranceFileCnt, sPartyCnt, sCompanyID), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError("m_oMisc.MergeFolders", "Function failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            ' Check if text, as we wont be zipping these
            If sPageType.ToUpper() = "TXT" Then
                sTmp = "N"
            Else
                sTmp = "Y"
            End If

            If sFolderName.ToUpper().Trim() <> "GENERAL" Then
                '' This Below Function developed for mapping the Renewal Document with Doc Folder Contents
                m_lReturn = CType(RenewalDocMapping(v_sFolderName:=sFolderName, r_lParentNum:=lParentNum), gPMConstants.PMEReturnCode) 'PN: 66472
            End If

            'We now have the index, so lets add the document record
            m_lReturn = m_oDocument.DirectAdd(vDocNum:=lDocNum, vFolderNum:=lParentNum, vDocName:=sDocName.Trim(), vExCode:=sDocName.Trim(), vDocType:=sDocType, vAccessLevel:=g_iAccessLevel, vZipped:=sTmp, vDocumentTemplateID:=vDocumentTemplateID, bVisibleFromWeb:=bVisibleFromWeb)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("m_oDocument.DirectAdd", "Function failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' MS 16/08/01
            ' doc ex_code is now set to the doc_num in order to make it unique instead of

            m_sSQL = "UPDATE DOC_document SET ex_code = doc_num WHERE doc_num = " & lDocNum

            ' Execute SQL Statement
            m_lReturn = CType(m_oDatabase.SQLAction(sSQL:=m_sSQL, sSQLName:="UPDATEDOCUMENTABLE", bStoredProcedure:=False), gPMConstants.PMEReturnCode)

            ' if UPDATE failed jut log message and carry on
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Could not UPDATE ex_code in DOCUMENT table", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            End If

            'Get and store now date
            dNowDate = DateTime.Now
            'now lets add the doc info record
            m_lReturn = m_oDocInfo.DirectAdd(vDocNum:=lDocNum, vExpiryDate:=dNowDate, vScanUser:=sUsername.Trim(), vDocDate:=dNowDate, vLastUser:=sUsername.Trim(), vLastDate:=dNowDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("m_oDocInfo.DirectAdd", "Function failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Add Annotations
            If sAnnotation <> "" Then

                'now lets add the annotation record
                m_lReturn = m_oAnnotation.DirectAdd(vDocNum:=lDocNum, vAnnText:=sAnnotation.Trim(), vUsername:=sUsername.Trim())

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError("m_oAnnotation.DirectAdd", "Function failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            End If

            'Now do the page add stuff

            'Get the next page name to use
            m_lReturn = CType(m_oMisc.GetNextPageName(sNextPageName:=sPageName.Value), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("m_oMisc.GetNextPageName", "Function failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            'Add the page record
            'sp todo - may need some sort of retry ? - should become avail in pmdao
            m_lReturn = m_oPage.DirectAdd(vPageName:=sPageName.Value, vDocNum:=lDocNum, vPageNum:=1, vPageType:=sPageType, vPageSize:=0, vVolumeID:=DOCHD1_ID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("m_oPage.DirectAdd", "Function failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            'DN 07/12/00 - Populate modular variable with added document number
            m_lDocNum = lDocNum

            'sp todo Create doc links in case linkfolder set . See notes?

            'Get path to where data is on live, HD volume.
            If sDataPath = "" Then
                m_lReturn = CType(m_oMisc.GetDataPath(DOCHD1_ID, sDataPath), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError("m_oMisc.GetDataPath", "Function failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            ' Create path for file copy
            m_lReturn = CType(MakePath(sDataPath & sPageName.Value), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("MakePath", sDataPath & sPageName.Value, gPMConstants.PMELogLevel.PMLogError)
            End If

            'Update the history database - note, if we are not actually linked to
            'an external database most of these params will be empty, but it matters
            'not as the call will not do anything.
            m_lReturn = CType(m_oMisc.ConstructDocRef(lDocNum, sTmpDocRef), gPMConstants.PMEReturnCode)

            m_lReturn = CType(StripSlashes(sPageName.Value, sTmpPageName), gPMConstants.PMEReturnCode)

            m_lReturn = m_oHistory.DirectAdd(vTask:=DOCADDDOCUMENT, vCabinetcode:=vIndexArray(0, 0), vCabinetname:="", vDrawercode:=vIndexArray(0, 1), vDrawername:="", vFoldercode:=vIndexArray(0, 2), vFoldername:="", vDocref:=sTmpDocRef, vRequestDate:=dNowDate.ToString("yyyyMMdd"), vRequestTime:=dNowDate.ToString("HHMMss"), vEventtype:=sDocType, vDescription:=sDocName, vVolume:=DOCHD1_NAME, vPagefile:=sTmpPageName, vDoctype:=sPageType)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("m_oHistory.DirectAdd", "Function failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Move page file from source to data tree
            ' SP240698 - no pint doing any compression in daemon, as cant compress
            ' text docs, and RTF docs will already be compressed by White Dwarf
            'If (UCase(sPageType$) = "TXT") Then

            Dim cloudHostingOptionValue As String = ""
            m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:="", v_sPassword:="", v_iUserID:=0, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=0, v_iCurrencyID:=0, v_iLogLevel:=0, v_sCallingAppName:=ACApp, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableCloudHosting, v_vBranch:=1, r_vUnderwriting:=cloudHostingOptionValue)

            If (gPMFunctions.NullToString(cloudHostingOptionValue) = "1") Then
                Dim s3NewPageName As String = (sPageName.Value & "." & sPageType).ToString.Replace("\", "/").TrimStart("/")
                'move from temp location to 00 tree
                Dim repository As IS3Repository = New S3Repository(Environment.GetEnvironmentVariable("AWS_DME_BUCKET_NAME"),
                    Environment.GetEnvironmentVariable("AWS_REGION"),
                    g_sUsername)

                m_lReturn = CType(repository.UploadFile(s3NewPageName, System.IO.File.ReadAllBytes(sFilename)).Result, gPMConstants.PMEReturnCode)
            Else
                'if text dont compress, as may need to view via UNIPLEX/History Viewer
                m_lReturn = CType(DOCGeneralFunc.CopyFile(sFileIn:=sFilename, sFileOut:=sDataPath & sPageName.Value & "." & sPageType), gPMConstants.PMEReturnCode)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'try and kill the file in case it moved something there
                m_lReturn = CType(KillFile(sDataPath & sPageName.Value & "." & sPageType), gPMConstants.PMEReturnCode)
                RaiseError("CopyFile", "sFileIn:=" & sFilename, gPMConstants.PMELogLevel.PMLogError)
            End If

            '    Else
            '        'not text so compress
            '        m_lReturn& = m_oZipper.ZipFile(sFileIn:=sFilename$, _
            ''                                    sFileOut:=sDataPath$ & sPageName$ & "." & sPageType$)
            '
            '        If (m_lReturn& <> True) Then
            '            Add = PMFalse
            '            'try and kill the file in case it moved something there
            '            m_lReturn& = KillFile(sDataPath$ & sPageName$ & "." & sPageType$)
            '            m_lReturn& = RollbackTrans()
            '            Exit Function
            '        End If
            '
            '    End If

            ' All OK so delete the original file
            If Not bVisibleFromWeb Then
                m_lReturn = CType(KillFile(sFilename), gPMConstants.PMEReturnCode)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'note file not deleted, but OK to continue
                bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed in KillFile", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            End If

            'Update the PMB Log if being used
            If Not (m_oPMBLog Is Nothing) Then

                ReDim m_sLogMess(5)
                m_sLogMess(1) = "DocuMaster API Daemon - DocuMaster Document created"

                m_sLogMess(2) = "Cabinet - " & CStr(vIndexArray(0, 0))

                m_sLogMess(3) = "Drawer - " & CStr(vIndexArray(0, 1))

                m_sLogMess(4) = "Folder - " & CStr(vIndexArray(0, 2))
                m_sLogMess(5) = "Document - " & sDocName

                m_oPMBLog.DOCbPMFunc.LogMessage(LLOG, "DMSAPI", ToSafeString(m_sLogMess))

            End If


            'commit transactions
            m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("CommitTrans()", "Function failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            'Do not call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=g_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
            If bStartTrans Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
            End If

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here

            '        Return result

            ' This is for debugging only
            '        Resume

        End Try
        Return result
    End Function


    Private Function RenewalDocMapping(ByVal v_sFolderName As String, ByRef r_lParentNum As gPMConstants.PMEReturnCode) As Integer

        ''Author : UPENDRA SINGH
        ''PurPose: This Below Function developed for mapping the Renewal Document with Doc Folder Contents Under PN:66472
        ''Create Date: 21-Apr-2010

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim sSQL As String
        Dim lReturn, lStatus As Integer

        Const iAddDocFolderPerInsRef_OptNo As Integer = 99
        Const kMethodName As String = "RenewalDocMapping"
        Const GetAddDocFolderPerInsRef_Value As String = "spu_GetHiddenOption"


        Try




            If v_sFolderName.Trim().Length = 0 Then
                Return result
            End If


            m_oDatabase.Parameters.Clear()

            lReturn = m_oDatabase.Parameters.Add(sName:="BranchID", vValue:=g_iSourceID, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMLong))

            lReturn = m_oDatabase.Parameters.Add(sName:="OptionNo", vValue:=iAddDocFolderPerInsRef_OptNo, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMLong))


            lReturn = m_oDatabase.SQLSelect(sSQL:=GetAddDocFolderPerInsRef_Value, sSQLName:=GetAddDocFolderPerInsRef_Value, bStoredProcedure:=True, vResultArray:=vResultArray)

            If Informations.IsArray(vResultArray) Then
                lStatus = ToSafeLong(vResultArray(0, 0))
            End If

            If lStatus <= 0 Then
                Return result
            End If

            result = gPMConstants.PMEReturnCode.PMTrue


            sSQL = "Select folder_num from doc_folder where folder_name = '" & v_sFolderName & "'"

            m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="RenewalDocMapping", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            If Informations.IsArray(vResultArray) Then
                r_lParentNum = CType(ToSafeLong(vResultArray(0, 0)), gPMConstants.PMEReturnCode)
            Else
                r_lParentNum = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If
            result = r_lParentNum

        Catch ex As Exception
            bPMFunc.LogError(v_sUsername:=g_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally
        End Try



        Return result

    End Function





    ' ***************************************************************** '
    ' Name: AddIndex (Public)
    '
    ' Description: This section performs an ADDINDEX request. It is supplied
    ' an array of external folder codes and names, (the first being the root
    ' folder (or cabinet, if you like) then goes thru each one with following
    ' logic.
    '
    '       If folder exists,
    '            check if name changed
    '                Update Name if different
    '       If folder does not exist,
    '                Add Folder
    '                Add remaining child folders
    '
    ' ***************************************************************** '
    Public Function AddIndex(ByRef vIndexArray(,) As Object, Optional ByRef oPMBLog As Object = Nothing, Optional ByRef bAccelerated As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim lFolderNum As Integer
        Dim sFolderName As String = ""
        Dim lParentNum As Integer
        Dim bCreateAllFolders As Boolean
        Dim iFolderLevel As Integer
        Dim sInsuranceFileCnt As String = ""
        Dim sPartyCnt As String = ""
        Dim sCompanyID As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'If passed empty array, fine, just go back
            If Not Informations.IsArray(vIndexArray) Then
                Return result
            End If

            'If reference to PMBLog object supplied, store it. The presence of
            'this will determine if we are called from PMB or not.
            m_oPMBLog = oPMBLog

            'set access level
            g_iAccessLevel = 9

            'Check if this is an accelerated API run - and perform different logic
            'if it is.
            If bAccelerated Then
                m_lReturn = CType(AddIndexAccelerated(vIndexArray), gPMConstants.PMEReturnCode)
                Return m_lReturn
            End If

            'Commence database updates
            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'This gets set to true as soon as a folder is created, as obviously
            'all subsequent child folders will need to be created too, so there
            'is no need to check if they already exist
            bCreateAllFolders = False

            'First folder we create will be root node, ie parent = 0 (or cabinet
            'for backward compatibility)
            lParentNum = 0
            iFolderLevel = DOCCabinet


            'loop thru each index level
            For i As Integer = vIndexArray.GetLowerBound(1) To vIndexArray.GetUpperBound(1)

                'some times PMB sends thru indices with no name.
                'These can be safely ignored - just commit what we've done and go

                If CStr(vIndexArray(1, i)).Trim() = "" Then

                    'commit transactions
                    m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        'Roll back
                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                        Return result
                    End If
                    Return result
                End If

                If Not bCreateAllFolders Then

                    'See if folder exists and if it does return the number and name

                    m_lReturn = CType(FolderExCodeExists(sExCode:=CStr(vIndexArray(0, i)), iFolderLevel:=iFolderLevel, lFolderNum:=lFolderNum, lParentNum:=lParentNum, sFolderName:=sFolderName, sInsuranceRef:=CStr(vIndexArray(1, i))), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                        Return result
                    End If

                    If lFolderNum <> 0 Then
                        'Folder exists

                        'check if name has changed

                        If sFolderName.Trim() <> CStr(vIndexArray(1, i)).Trim() Then

                            'Rename the folder
                            m_lReturn = CType(RenameFolder(lFolderNum, iFolderLevel, vIndexArray, i), gPMConstants.PMEReturnCode)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                                Return result
                            End If

                        End If

                        'Store the folder num, which will be the parent of the next folder
                        'we add
                        lParentNum = lFolderNum

                    Else

                        'Add the new folder
                        m_lReturn = CType(AddFolder(iFolderLevel, vIndexArray, i, lParentNum, lFolderNum), gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                            Return result
                        End If

                        'Store the folder num, which will be the parent of the next folder
                        'we add
                        lParentNum = lFolderNum

                        'all subsequent folders in index array will need to be added.
                        bCreateAllFolders = True

                    End If

                Else

                    'Add the new folder
                    m_lReturn = CType(AddFolder(iFolderLevel, vIndexArray, i, lParentNum, lFolderNum), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Store the folder num, which will be the parent of the next folder
                    'we add
                    lParentNum = lFolderNum

                End If

                'DJM 18/09/2003
                Select Case iFolderLevel
                    Case DOCFolderLevelBranch

                        sCompanyID = CStr(vIndexArray(0, i))
                    Case DOCFolderLevelClient

                        sPartyCnt = CStr(vIndexArray(0, i))
                    Case DOCFolderLevelPolicy

                        If CStr(vIndexArray(1, i)) <> "GENERAL" And CStr(vIndexArray(1, i)) <> "COMPLAINTS" Then

                            sInsuranceFileCnt = CStr(vIndexArray(0, i))
                        End If
                End Select

                'next level
                iFolderLevel += 1

            Next i

            'DJM 18/09/2003 : Make sure that only one document folder exists for this policy.
            If sInsuranceFileCnt <> "" Then
                m_lReturn = CType(m_oMisc.MergeFolders(sInsuranceFileCnt, sPartyCnt, sCompanyID), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                    Return result
                End If
            End If

            'commit transactions
            m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                'Roll back
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return result

            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="AddIndex", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddIndexAccelerated (Private)
    '
    ' Description: This section performs an ADDINDEX request. However this
    ' runs sleeker logic which only adds the last index in the array
    ' (assuming the parent indices already exist).
    '
    ' It is used when an accelerated API run is ticked in the API TImer and
    ' should only be used for system option 40 runs when DocuMaster is
    ' turned on.
    '
    ' ***************************************************************** '
    Private Function AddIndexAccelerated(ByRef vIndexArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lFolderNum As Integer
        Dim sFolderName As String = ""
        Dim lParentNum As Integer
        Dim iFolderLevel As Integer




        result = gPMConstants.PMEReturnCode.PMTrue

        'Commence database updates
        m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Check the node type we are adding
        Select Case vIndexArray.GetUpperBound(1) - vIndexArray.GetLowerBound(1)
            Case 0
                iFolderLevel = DOCCabinet
            Case 1
                iFolderLevel = DOCDrawer
            Case 2
                iFolderLevel = DOCFolder
            Case Else
                'Shouldn't be happening - log error
                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Too many node levels", vApp:=ACApp, vClass:=ACClass, vMethod:="AddIndexAccelerated", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result

        End Select


        'if we are adding a cabinet we can just add it straight away
        If iFolderLevel = DOCCabinet Then

            'Add the new folder
            m_lReturn = CType(AddFolder(iFolderLevel, vIndexArray, 0, 0, lFolderNum), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return result
            End If

        End If


        If (iFolderLevel = DOCDrawer) Or (iFolderLevel = DOCFolder) Then

            'Adding either a folder or drawer.

            'See if the parent node exists and if it does return the number and name

            m_lReturn = CType(FolderExCodeExists(sExCode:=CStr(vIndexArray(0, vIndexArray.GetUpperBound(1) - 1)), iFolderLevel:=iFolderLevel - 1, lFolderNum:=lFolderNum, sFolderName:=sFolderName), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return result
            End If

            If lFolderNum <> 0 Then
                'Parent Folder exists
                lParentNum = lFolderNum

                'Add the new folder
                m_lReturn = CType(AddFolder(iFolderLevel, vIndexArray, 0, lParentNum, lFolderNum), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                    Return result
                End If

            Else

                'parent does not exist so error (we should not be creating parents, this
                'is accelerated mode).
                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Parent does not exit.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddIndexAccelerated", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return result

            End If

        End If


        'commit transactions
        m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            result = gPMConstants.PMEReturnCode.PMFalse
            'Roll back
            m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
            Return result

        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: MergeIndex(Public)
    '
    ' Description:
    '
    '   MS 19/02/001      Data Merge
    '
    '   Merges a folder from a source company to a target company also
    '   updates all related documents and the history database for PMB
    '
    ' ***************************************************************** '

    Public Function MergeIndex(ByRef vIndexArray(,) As Object, Optional ByRef oPMBLog As Object = Nothing) As Integer
        'source
        'vIndexListData(0, 0) ' source cabinet code
        'vIndexListData(0, 1) '        drawer code
        'vIndexListData(0, 2) '        folder code
        'target
        'vIndexListData(1, 0) ' Target cabinet code
        'vIndexListData(1, 1) '        drawer code
        'vIndexListData(1, 2) '        folder code

        Dim result As Integer = 0
        Dim sSourceCabExCode, sTargetCabExCode, sSourceDrawExCode, sTargetDrawExCode, sSourceFoldExCode, sTargetFoldExCode As String

        Dim sSourceCabName As String = ""
        Dim sTargetCabName As String = ""
        Dim sSourceDrawName As String = ""
        Dim sTargetDrawName As String = ""
        Dim sSourceFoldName As String = ""
        Dim sTargetFoldName As String = ""

        Dim lTargetCoNum, lSourceCoNum, lTargetDrawNum, lSourceDrawNum, lTargetFoldNum, lSourceFoldNum As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'If passed empty array, fine, just go back
            If Not Informations.IsArray(vIndexArray) Then
                Return result
            End If

            'If reference to PMBLog object supplied, store it. The presence of
            'this will determine if we are called from PMB or not.
            m_oPMBLog = oPMBLog

            'set access level to admin level in order to force all updates
            g_iAccessLevel = 1

            'Commence database updates
            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'Parent_Num is 0 for Cabinet level folders


            sSourceCabExCode = CStr(vIndexArray(0, 0))

            sTargetCabExCode = CStr(vIndexArray(1, 0))

            'First find the source company
            m_lReturn = CType(FolderExCodeExists(sExCode:=sSourceCabExCode, iFolderLevel:=DOCCabinet, lFolderNum:=lSourceCoNum, lParentNum:=0, sFolderName:=sSourceCabName), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'then find target company
            m_lReturn = CType(FolderExCodeExists(sExCode:=sTargetCabExCode, iFolderLevel:=DOCCabinet, lFolderNum:=lTargetCoNum, lParentNum:=0, sFolderName:=sTargetCabName), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'Now for the drawer

            sSourceDrawExCode = CStr(vIndexArray(0, 1))

            sTargetDrawExCode = CStr(vIndexArray(1, 1))

            ' does source client exist
            m_lReturn = CType(FolderExCodeExists(sExCode:=sSourceDrawExCode, iFolderLevel:=DOCDrawer, lFolderNum:=lSourceDrawNum, lParentNum:=lSourceCoNum, sFolderName:=sSourceDrawName), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' does target client exist
            m_lReturn = CType(FolderExCodeExists(sExCode:=sTargetDrawExCode, iFolderLevel:=DOCDrawer, lFolderNum:=lTargetDrawNum, lParentNum:=lTargetCoNum, sFolderName:=sTargetDrawName), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            sSourceFoldExCode = CStr(vIndexArray(0, 2))

            sTargetFoldExCode = CStr(vIndexArray(1, 2))

            ' does source folder exist
            m_lReturn = CType(FolderExCodeExists(sExCode:=sSourceFoldExCode, iFolderLevel:=DOCFolder, lFolderNum:=lSourceFoldNum, lParentNum:=lSourceDrawNum, sFolderName:=sSourceFoldName), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' does target folder exist
            m_lReturn = CType(FolderExCodeExists(sExCode:=sTargetFoldExCode, iFolderLevel:=DOCFolder, lFolderNum:=lTargetFoldNum, lParentNum:=lTargetDrawNum, sFolderName:=sTargetFoldName), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'Merge the source folder into the target folder and update history database

            m_lReturn = MergeFolder(sTargetCabExCode:=sTargetCabExCode, sTargetCabName:=sTargetCabName, sTargetDrawExCode:=sTargetDrawExCode, sTargetDrawName:=sTargetDrawName, sTargetFoldExCode:=sTargetFoldExCode, sTargetFoldName:=sTargetFoldName, sSourceCabExCode:=sSourceCabExCode, sSourceCabName:=sSourceCabName, sSourceDrawExCode:=sSourceDrawExCode, sSourceDrawName:=sSourceDrawName, sSourceFoldExCode:=sSourceFoldExCode, sSourceFoldName:=sSourceFoldName, lSourceFoldNum:=lSourceFoldNum, lTargetFoldNum:=lTargetFoldNum, lSourceDrawNum:=lSourceDrawNum, oPMBLog:=m_oPMBLog)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' failed so log to DME log file
                ReDim m_sLogMess(5)
                m_sLogMess(1) = "DocuMaster API Daemon -  Merge Failed"
                m_sLogMess(2) = "Source Folder: " & lSourceFoldNum & " to Target Folder: " & CStr(lTargetFoldNum)
                m_sLogMess(3) = "Cabinet - from " & sSourceCabExCode & ", to " & sTargetCabExCode
                m_sLogMess(4) = "Drawer  - from " & sSourceDrawExCode & ", to " & sTargetDrawExCode
                m_sLogMess(5) = "Folder  - from " & sSourceFoldExCode & ", to " & sTargetFoldExCode

                m_oPMBLog.DOCbPMFunc.LogMessage(LLOG, "DMSAPI", ToSafeString(m_sLogMess))

                'Roll back
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return result
            End If


            ' Don't log as it will clog up the log file. Calling procedure will log if unsucessful

            'Everything OK so log as much.
            '    If (m_oPMBLog Is Nothing <> True) Then
            '
            '        ReDim m_sLogMess(5)
            '
            '        m_sLogMess(1) = "DocuMaster API Daemon -  folder successfuly merged"
            '        m_sLogMess(2) = "Source Folder: " & lSourceFoldNum & " to Target Folder: " & lTargetFoldNum
            '        m_sLogMess(3) = "Cabinet - from " & sSourceCabExCode$ & ", to " & sTargetCabExCode$
            '        m_sLogMess(4) = "Drawer  - from " & sSourceDrawExCode$ & ", to " & sTargetDrawExCode$
            '        m_sLogMess(5) = "Folder  - from " & sSourceFoldExCode$ & ", to " & sTargetFoldExCode$
            '        m_oPMBLog.DOCbPMFunc.LogMessage LLOG, "DMSAPI", m_sLogMess()
            '
            '    End If


            'commit transactions
            m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                'Roll back
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MergeIndexFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="MergeIndex", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: FolderExCodeExists (Private)
    '
    ' Description: Given a folder external code, folder level and
    ' parent number, this returns its folder number and name.
    ' If not present, 0 is returned as the folder number
    '
    ' ***************************************************************** '
    Private Function FolderExCodeExists(ByRef sExCode As String, ByRef iFolderLevel As Integer, ByRef lFolderNum As Integer, Optional ByRef lParentNum As Object = Nothing, Optional ByRef sFolderName As String = "", Optional ByRef sInsuranceRef As String = "") As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing



        result = gPMConstants.PMEReturnCode.PMTrue
        'When we are on policy level, then check for the insurance_ref if it is already present or not
        'Otherwise system will create sub-folders(ex_code) for all subsequent transactions of the Policy(MTA,CANC,REINS)
        If iFolderLevel = 2 And sInsuranceRef <> "" Then
            'Construct SQL
             m_sSQL = "SELECT folder_num, folder_name FROM DOC_folder " &
                         "WHERE  ex_code = '" & sExCode & "'" &
                         " AND (folder_level = " & CStr(iFolderLevel) & " OR folder_name = '" & Trim(sInsuranceRef) & "')"
        Else
            'Construct SQL
            m_sSQL = "SELECT folder_num, folder_name FROM DOC_folder " &
                         "WHERE ex_code = '" & sExCode & "'" &
                         " AND folder_level = " & CStr(iFolderLevel)
        End If



        'use the parent num parameter if supplied

        If Not Informations.IsNothing(lParentNum) Then

            m_sSQL = m_sSQL & " AND parent_num = " & CStr(CInt(lParentNum))
        End If

        'hit DB
        m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="FOLDEREXCODE", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Return the parent num.
        If Informations.IsArray(vResultArray) Then


            lFolderNum = CInt(vResultArray(0, 0))

            sFolderName = CStr(vResultArray(1, 0)).Trim()
        Else
            lFolderNum = 0
            Return result
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddFolder (Public)
    '
    ' Description: This section adds a folder to the database and updates
    ' the history table. It returns the folder number for use when adding
    ' further child folders.
    '
    ' ***************************************************************** '
    Private Function AddFolder(ByRef iFolderLevel As Integer, ByRef vIndexArray(,) As Object, ByRef iIndex As Integer, ByRef lParentNum As Integer, ByRef lFolderNum As Integer) As Integer


        Dim result As Integer = 0
        Dim iTask As Integer
        Dim lTmp As Integer
        Dim sCabExCode As String = ""
        Dim sCabName As String = ""
        Dim sDrawExCode As String = ""
        Dim sDrawName As String = ""
        Dim sFoldExCode As String = ""
        Dim sFoldName As String = ""

        Dim sNodeExCode As String = ""
        Dim sNodeName As String = ""
        result = gPMConstants.PMEReturnCode.PMTrue

        ' check to see what level node we are adding, for use if we are updating
        ' the history database for PMB.
        Select Case iFolderLevel
            Case DOCCabinet
                'Adding a cabinet

                sCabExCode = CStr(vIndexArray(0, vIndexArray.GetLowerBound(0)))

                sCabName = CStr(vIndexArray(1, vIndexArray.GetLowerBound(0)))

                iTask = DOCADDCABINET
                sNodeExCode = sCabExCode
                sNodeName = sCabName

            Case DOCDrawer
                'Adding a drawer

                sCabExCode = CStr(vIndexArray(0, vIndexArray.GetLowerBound(0)))
                'sCabName$ = vIndexArray(1, LBound(vIndexArray))

                sDrawExCode = CStr(vIndexArray(0, vIndexArray.GetLowerBound(0) + 1))

                sDrawName = CStr(vIndexArray(1, vIndexArray.GetLowerBound(0) + 1))

                iTask = DOCADDDRAWER
                sNodeExCode = sDrawExCode
                sNodeName = sDrawName

            Case DOCFolder
                'Adding a folder (ie a version 2 folder)

                sCabExCode = CStr(vIndexArray(0, vIndexArray.GetLowerBound(0)))
                'sCabName$ = vIndexArray(1, LBound(vIndexArray))

                sDrawExCode = CStr(vIndexArray(0, vIndexArray.GetLowerBound(0) + 1))
                'sDrawName$ = vIndexArray(1, LBound(vIndexArray) + 1)

                sFoldExCode = CStr(vIndexArray(0, vIndexArray.GetLowerBound(0) + 2))

                sFoldName = CStr(vIndexArray(1, vIndexArray.GetLowerBound(0) + 2))

                iTask = DOCADDFOLDER
                sNodeExCode = sFoldExCode
                sNodeName = sFoldName

            Case Else
                'any level node being added - ie this request is not from PMB
                'so just pick the code and name straight out of the array.

                sNodeExCode = CStr(vIndexArray(0, iIndex))

                sNodeName = CStr(vIndexArray(1, iIndex))

        End Select

        'Mow add the folder into the database
        m_lReturn = m_oFolder.DirectAdd(vFolderNum:=lFolderNum, vFolderName:=sNodeName.Trim(), vParentNum:=lParentNum, vExCode:=sNodeExCode.Trim(), vFolderLevel:=iFolderLevel)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Add Folder. Index = " &
                       sCabExCode & "|" &
                       sCabName & "|" &
                       sDrawExCode & "|" &
                       sDrawName & "|" &
                       sFoldExCode & "|" &
                       sFoldName & "|", vApp:=ACApp, vClass:=ACClass, vMethod:="AddFolder", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return result
        End If

        'Update the history database - note, if we are not actually linked to
        'an external database most of these params will be empty, but it matters
        'not as the call will not do anything.
        m_lReturn = m_oHistory.DirectAdd(vTask:=iTask, vCabinetcode:=sCabExCode, vCabinetname:=sCabName, vDrawercode:=sDrawExCode, vDrawername:=sDrawName, vFoldercode:=sFoldExCode, vFoldername:=sFoldName)


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update History Database. Index = " &
                       sCabExCode & "|" &
                       sCabName & "|" &
                       sDrawExCode & "|" &
                       sDrawName & "|" &
                       sFoldExCode & "|" &
                       sFoldName & "|", vApp:=ACApp, vClass:=ACClass, vMethod:="AddFolder", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return result
        End If

        'Update the PMB Log if being used
        If Not (m_oPMBLog Is Nothing) Then

            Select Case iFolderLevel
                Case DOCCabinet
                    ReDim m_sLogMess(2)
                    m_sLogMess(1) = "DocuMaster API Daemon - DocuMaster Cabinet created"
                    m_sLogMess(2) = "Cabinet - " & sCabExCode & ", " & sCabName

                Case DOCDrawer
                    ReDim m_sLogMess(3)
                    m_sLogMess(1) = "DocuMaster API Daemon - DocuMaster Drawer created"
                    m_sLogMess(2) = "Cabinet - " & sCabExCode & ", " & sCabName
                    m_sLogMess(3) = "Drawer - " & sDrawExCode & ", " & sDrawName

                Case DOCFolder
                    ReDim m_sLogMess(4)
                    m_sLogMess(1) = "DocuMaster API Daemon - DocuMaster Folder created"
                    m_sLogMess(2) = "Cabinet - " & sCabExCode & ", " & sCabName
                    m_sLogMess(3) = "Drawer - " & sDrawExCode & ", " & sDrawName
                    m_sLogMess(4) = "Folder - " & sFoldExCode & ", " & sFoldName

                Case Else

                    'Fill this in if when we develop error logging for
                    '3rd party applications that use the API

            End Select


            m_oPMBLog.DOCbPMFunc.LogMessage(LLOG, "DMSAPI", ToSafeString(m_sLogMess))

        End If

        'If we have just added a drawer, then we should add a GENERAL folder within it
        If iFolderLevel = DOCDrawer Then
            If m_bInsuranceNum Then
                Return result
            End If
            'Add the general folder into the database
            m_lReturn = m_oFolder.DirectAdd(vFolderNum:=lTmp, vFolderName:="GENERAL", vParentNum:=lFolderNum, vExCode:="GENERAL", vFolderLevel:=DOCFolder)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Add Folder. Index = " &
                           sCabExCode & "|" &
                           sCabName & "|" &
                           sDrawExCode & "|" &
                           sDrawName & "|" &
                           "GENERAL" & "|" &
                           "GENERAL" & "|", vApp:=ACApp, vClass:=ACClass, vMethod:="AddFolder", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            'Update history db for general folder
            m_lReturn = m_oHistory.DirectAdd(vTask:=DOCADDFOLDER, vCabinetcode:=sCabExCode, vCabinetname:="", vDrawercode:=sDrawExCode, vDrawername:="", vFoldercode:="GENERAL", vFoldername:="GENERAL")

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update History Database. Index = " &
                           sCabExCode & "|" &
                           sCabName & "|" &
                           sDrawExCode & "|" &
                           sDrawName & "|" &
                           "GENERAL" & "|" &
                           "GENERAL" & "|", vApp:=ACApp, vClass:=ACClass, vMethod:="AddFolder", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: RenameFolder (Public)
    '
    ' Description: This section renames a folder in the database and updates
    ' the history table.
    '
    ' ***************************************************************** '
    Private Function RenameFolder(ByRef lFolderNum As Integer, ByRef iFolderLevel As Integer, ByRef vIndexArray(,) As Object, ByRef iIndex As Integer) As Integer


        Dim result As Integer = 0
        Dim iTask As Integer

        Dim sCabExCode As String = ""
        Dim sCabName As String = ""
        Dim sDrawExCode As String = ""
        Dim sDrawName As String = ""
        Dim sFoldExCode As String = ""
        Dim sFoldName As String = ""

        Dim sNodeExCode As String = ""
        Dim sNodeName As String = ""


        result = gPMConstants.PMEReturnCode.PMTrue

        ' check to see what level node we are renaming, for use if we are updating
        ' the history database for PMB.

        Select Case iFolderLevel
            Case DOCCabinet
                'Renaming a cabinet

                sCabExCode = CStr(vIndexArray(0, vIndexArray.GetLowerBound(0)))

                sCabName = CStr(vIndexArray(1, vIndexArray.GetLowerBound(0)))

                iTask = DOCMODCABINET
                sNodeExCode = sCabExCode
                sNodeName = sCabName

            Case DOCDrawer
                'Renaming a drawer

                sCabExCode = CStr(vIndexArray(0, vIndexArray.GetLowerBound(0)))

                sDrawExCode = CStr(vIndexArray(0, vIndexArray.GetLowerBound(0) + 1))

                sDrawName = CStr(vIndexArray(1, vIndexArray.GetLowerBound(0) + 1))

                iTask = DOCMODDRAWER
                sNodeExCode = sDrawExCode
                sNodeName = sDrawName

            Case DOCFolder
                'Renaming a folder (ie a version 2 folder)

                sCabExCode = CStr(vIndexArray(0, vIndexArray.GetLowerBound(0)))

                sDrawExCode = CStr(vIndexArray(0, vIndexArray.GetLowerBound(0) + 1))

                sFoldExCode = CStr(vIndexArray(0, vIndexArray.GetLowerBound(0) + 2))

                sFoldName = CStr(vIndexArray(1, vIndexArray.GetLowerBound(0) + 2))

                iTask = DOCMODFOLDER
                sNodeExCode = sFoldExCode
                sNodeName = sFoldName

            Case Else
                'any level node being renamed - ie this request is not from PMB
                'so just pick the code and new name straight out of the array.

                sNodeName = CStr(vIndexArray(1, iIndex))

        End Select

        'Actually rename the folder now
        'sp todo - can remove this when goes in pmdao
        m_lReturn = CType(ValidateSQL(sNodeName), gPMConstants.PMEReturnCode)

        m_lReturn = CType(m_oMisc.RenameNode(iNodeType:=DOCNode_Folder, lNodeNum:=lFolderNum, sNewNodeName:=sNodeName), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Rename Folder. Index = " &
                       sCabExCode & "|" &
                       sCabName & "|" &
                       sDrawExCode & "|" &
                       sDrawName & "|" &
                       sFoldExCode & "|" &
                       sFoldName & "|", vApp:=ACApp, vClass:=ACClass, vMethod:="RenameFolder", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return result
        End If

        'Update the history database - note, if we are not actually linked to
        'an external database most of these params will be empty, but it matters
        'not as the call will not do anything.
        m_lReturn = m_oHistory.DirectAdd(vTask:=iTask, vCabinetcode:=sCabExCode, vCabinetname:=sCabName, vDrawercode:=sDrawExCode, vDrawername:=sDrawName, vFoldercode:=sFoldExCode, vFoldername:=sFoldName)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update History Database. Index = " &
                       sCabExCode & "|" &
                       sCabName & "|" &
                       sDrawExCode & "|" &
                       sDrawName & "|" &
                       sFoldExCode & "|" &
                       sFoldName & "|", vApp:=ACApp, vClass:=ACClass, vMethod:="RenameFolder", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return result
        End If

        'Update the PMB Log if supplied
        If Not (m_oPMBLog Is Nothing) Then


            Select Case iFolderLevel
                Case DOCCabinet
                    ReDim m_sLogMess(2)
                    m_sLogMess(1) = "DocuMaster API Daemon - DocuMaster Cabinet changed"
                    m_sLogMess(2) = "Cabinet - " & sCabExCode & ", " & sCabName

                    m_oPMBLog.DOCbPMFunc.LogMessage(LLOG, "DMSAPI", ToSafeString(m_sLogMess))

                Case DOCDrawer
                    ReDim m_sLogMess(3)
                    m_sLogMess(1) = "DocuMaster API Daemon - DocuMaster Drawer changed"
                    m_sLogMess(2) = "Cabinet - " & sCabExCode & ", " & sCabName
                    m_sLogMess(3) = "Drawer - " & sDrawExCode & ", " & sDrawName

                    m_oPMBLog.DOCbPMFunc.LogMessage(LLOG, "DMSAPI", ToSafeString(m_sLogMess))

                Case DOCFolder
                    ReDim m_sLogMess(4)
                    m_sLogMess(1) = "DocuMaster API Daemon - DocuMaster Folder changed"
                    m_sLogMess(2) = "Cabinet - " & sCabExCode & ", " & sCabName
                    m_sLogMess(3) = "Drawer - " & sDrawExCode & ", " & sDrawName
                    m_sLogMess(4) = "Folder - " & sFoldExCode & ", " & sFoldName

                    m_oPMBLog.DOCbPMFunc.LogMessage(LLOG, "DMSAPI", ToSafeString(m_sLogMess))

                Case Else

                    'Fill this in if when we develop error logging for
                    '3rd party Applications

            End Select

        End If


        Return result

    End Function



    ' ***************************************************************** '
    ' Name: DelIndex (Public)
    '
    ' Description: This code will delete the supplied folder index
    '
    ' MGW 18/02/03 - Removed re-set of DelIndex flag
    '                If item it's not there, that fine, that's what we want.
    ' ***************************************************************** '
    Public Function DelIndex(ByRef vIndexArray(,) As Object, ByRef iEmptyOnly As Integer, ByRef iAccessLevel As Integer, Optional ByRef oPMBLog As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lFolderNum As Integer
        Dim sExCode As String = ""
        Dim iFolderLevel As Integer
        Dim bEmpty As Boolean
        Dim sNoAccessName As String = ""
        Dim sMsgTmp As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'If passed empty array, fine, just go back
            If Not Informations.IsArray(vIndexArray) Then
                Return result
            End If

            'If reference to PMBLog object supplied, store it. The presence of
            'this will determine if we are called from PMB or not.
            m_oPMBLog = oPMBLog

            'set access level
            g_iAccessLevel = iAccessLevel

            'Commence database updates
            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return result
            End If

            'loop thru each index level to get the ex code and level for the
            'folder we actually want to delete
            iFolderLevel = DOCCabinet

            For i As Integer = vIndexArray.GetLowerBound(1) To vIndexArray.GetUpperBound(1)
                'next level
                iFolderLevel += 1

                sExCode = CStr(vIndexArray(0, i))
            Next i

            iFolderLevel -= 1


            'Now check this folder exists
            m_lReturn = CType(FolderExCodeExists(sExCode:=sExCode, iFolderLevel:=iFolderLevel, lFolderNum:=lFolderNum), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return result
            End If

            If lFolderNum = 0 Then

                ' folder does not exist
                ' MGW 18/02/03 - Removed re-set of DelIndex flag
                '                If item it's not there, that fine, that's what we want.
                ' DelIndex = PMFalse
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)



                sMsgTmp = "Folder Does Not Exist: Ex Code=" &
                          CStr(vIndexArray(0, vIndexArray.GetUpperBound(1))) &
                          ", Name=" & CStr(vIndexArray(1, vIndexArray.GetUpperBound(1)))

                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=sMsgTmp, vApp:=ACApp, vClass:=ACClass, vMethod:="DelIndex", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result

            End If

            If Not iEmptyOnly Then

                'Folder doesn't have to be empty to be deleted, so go ahead
                m_lReturn = CType(m_oMisc.DeleteFolder(lFolderNum:=lFolderNum, sNoAccessName:=sNoAccessName), gPMConstants.PMEReturnCode)


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    If sNoAccessName <> "" Then
                        'folder could not be deleted because of perms. Not an error,
                        'just log it.
                        If Not (m_oPMBLog Is Nothing) Then


                            Select Case iFolderLevel
                                Case DOCCabinet
                                    ReDim m_sLogMess(2)
                                    m_sLogMess(1) = "DocuMaster API Daemon - DocuMaster Cabinet contains data, not deleted"


                                    m_sLogMess(2) = "Cabinet - " & CStr(vIndexArray(0, vIndexArray.GetLowerBound(0))) & ", " & CStr(vIndexArray(1, vIndexArray.GetLowerBound(0)))

                                Case DOCDrawer
                                    ReDim m_sLogMess(3)
                                    m_sLogMess(1) = "DocuMaster API Daemon - DocuMaster Drawer contains data, not deleted"


                                    m_sLogMess(2) = "Cabinet - " & CStr(vIndexArray(0, vIndexArray.GetLowerBound(0))) & ", " & CStr(vIndexArray(1, vIndexArray.GetLowerBound(0)))


                                    m_sLogMess(3) = "Drawer - " & CStr(vIndexArray(0, vIndexArray.GetLowerBound(0) + 1)) & ", " & CStr(vIndexArray(1, vIndexArray.GetLowerBound(0) + 1))

                                Case DOCFolder
                                    ReDim m_sLogMess(4)
                                    m_sLogMess(1) = "DocuMaster API Daemon - DocuMaster Folder contains data, not deleted"


                                    m_sLogMess(2) = "Cabinet - " & CStr(vIndexArray(0, vIndexArray.GetLowerBound(0))) & ", " & CStr(vIndexArray(1, vIndexArray.GetLowerBound(0)))


                                    m_sLogMess(3) = "Drawer - " & CStr(vIndexArray(0, vIndexArray.GetLowerBound(0) + 1)) & ", " & CStr(vIndexArray(1, vIndexArray.GetLowerBound(0) + 1))


                                    m_sLogMess(4) = "Folder - " & CStr(vIndexArray(0, vIndexArray.GetLowerBound(0) + 2)) & ", " & CStr(vIndexArray(1, vIndexArray.GetLowerBound(0) + 2))

                                Case Else

                                    'Fill this in if when we develop error logging for
                                    '3rd party applications that use the API

                            End Select


                            m_oPMBLog.DOCbPMFunc.LogMessage(LLOG, "DMSAPI", ToSafeString(m_sLogMess))

                        End If
                    Else
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If
                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                    Return result

                End If

            Else
                'best check if empty first
                m_lReturn = CType(IsFolderEmpty(lFolderNum, bEmpty), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                    Return result
                End If

                If bEmpty Then
                    'folder empty so can delete
                    m_lReturn = CType(m_oMisc.DeleteFolder(lFolderNum:=lFolderNum, sNoAccessName:=sNoAccessName), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        If sNoAccessName <> "" Then
                            'folder could not be deleted because of perms. Not an error,
                            'just log it.
                            If Not (m_oPMBLog Is Nothing) Then


                                Select Case iFolderLevel
                                    Case DOCCabinet
                                        ReDim m_sLogMess(2)
                                        m_sLogMess(1) = "DocuMaster API Daemon - DocuMaster Cabinet contains folder API does not have permission to delete."


                                        m_sLogMess(2) = "Cabinet - " & CStr(vIndexArray(0, vIndexArray.GetLowerBound(0))) & ", " & CStr(vIndexArray(1, vIndexArray.GetLowerBound(0)))

                                    Case DOCDrawer
                                        ReDim m_sLogMess(3)
                                        m_sLogMess(1) = "DocuMaster API Daemon - DocuMaster Drawer contains folder API does not have permission to delete."


                                        m_sLogMess(2) = "Cabinet - " & CStr(vIndexArray(0, vIndexArray.GetLowerBound(0))) & ", " & CStr(vIndexArray(1, vIndexArray.GetLowerBound(0)))


                                        m_sLogMess(3) = "Drawer - " & CStr(vIndexArray(0, vIndexArray.GetLowerBound(0) + 1)) & ", " & CStr(vIndexArray(1, vIndexArray.GetLowerBound(0) + 1))

                                    Case DOCFolder
                                        ReDim m_sLogMess(4)
                                        m_sLogMess(1) = "DocuMaster API Daemon - DocuMaster Folder contains folder API does not have permission to delete."


                                        m_sLogMess(2) = "Cabinet - " & CStr(vIndexArray(0, vIndexArray.GetLowerBound(0))) & ", " & CStr(vIndexArray(1, vIndexArray.GetLowerBound(0)))


                                        m_sLogMess(3) = "Drawer - " & CStr(vIndexArray(0, vIndexArray.GetLowerBound(0) + 1)) & ", " & CStr(vIndexArray(1, vIndexArray.GetLowerBound(0) + 1))


                                        m_sLogMess(4) = "Folder - " & CStr(vIndexArray(0, vIndexArray.GetLowerBound(0) + 2)) & ", " & CStr(vIndexArray(1, vIndexArray.GetLowerBound(0) + 2))

                                    Case Else

                                        'Fill this in if when we develop error logging for
                                        '3rd party applications that use the API

                                End Select


                                m_oPMBLog.DOCbPMFunc.LogMessage(LLOG, "DMSAPI", ToSafeString(m_sLogMess))

                            End If
                        Else
                            result = gPMConstants.PMEReturnCode.PMFalse
                        End If

                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                        Return result

                    End If

                Else
                    'The folder is nicht empty, so log as much and depart swiftly
                    If Not (m_oPMBLog Is Nothing) Then


                        Select Case iFolderLevel
                            Case DOCCabinet
                                ReDim m_sLogMess(2)
                                m_sLogMess(1) = "DocuMaster API Daemon - DocuMaster Cabinet contains folder API does not have permission to delete."


                                m_sLogMess(2) = "Cabinet - " & CStr(vIndexArray(0, vIndexArray.GetLowerBound(0))) & ", " & CStr(vIndexArray(1, vIndexArray.GetLowerBound(0)))

                            Case DOCDrawer
                                ReDim m_sLogMess(3)
                                m_sLogMess(1) = "DocuMaster API Daemon - DocuMaster Drawer contains folder API does not have permission to delete."


                                m_sLogMess(2) = "Cabinet - " & CStr(vIndexArray(0, vIndexArray.GetLowerBound(0))) & ", " & CStr(vIndexArray(1, vIndexArray.GetLowerBound(0)))


                                m_sLogMess(3) = "Drawer - " & CStr(vIndexArray(0, vIndexArray.GetLowerBound(0) + 1)) & ", " & CStr(vIndexArray(1, vIndexArray.GetLowerBound(0) + 1))

                            Case DOCFolder
                                ReDim m_sLogMess(4)
                                m_sLogMess(1) = "DocuMaster API Daemon - DocuMaster Folder contains folder API does not have permission to delete."


                                m_sLogMess(2) = "Cabinet - " & CStr(vIndexArray(0, vIndexArray.GetLowerBound(0))) & ", " & CStr(vIndexArray(1, vIndexArray.GetLowerBound(0)))


                                m_sLogMess(3) = "Drawer - " & CStr(vIndexArray(0, vIndexArray.GetLowerBound(0) + 1)) & ", " & CStr(vIndexArray(1, vIndexArray.GetLowerBound(0) + 1))


                                m_sLogMess(4) = "Folder - " & CStr(vIndexArray(0, vIndexArray.GetLowerBound(0) + 2)) & ", " & CStr(vIndexArray(1, vIndexArray.GetLowerBound(0) + 2))

                            Case Else

                                'Fill this in if when we develop error logging for
                                '3rd party applications that use the API

                        End Select


                        m_oPMBLog.DOCbPMFunc.LogMessage(LLOG, "DMSAPI", ToSafeString(m_sLogMess))

                    End If

                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                    Return result

                End If

            End If

            'Everything OK so log as much.
            If Not (m_oPMBLog Is Nothing) Then


                Select Case iFolderLevel
                    Case DOCCabinet
                        ReDim m_sLogMess(2)
                        m_sLogMess(1) = "DocuMaster API Daemon - DocuMaster Cabinet deleted, " & lFolderNum


                        m_sLogMess(2) = "Cabinet - " & CStr(vIndexArray(0, vIndexArray.GetLowerBound(0))) & ", " & CStr(vIndexArray(1, vIndexArray.GetLowerBound(0)))

                    Case DOCDrawer
                        ReDim m_sLogMess(3)
                        m_sLogMess(1) = "DocuMaster API Daemon - DocuMaster Drawer deleted, " & lFolderNum


                        m_sLogMess(2) = "Cabinet - " & CStr(vIndexArray(0, vIndexArray.GetLowerBound(0))) & ", " & CStr(vIndexArray(1, vIndexArray.GetLowerBound(0)))


                        m_sLogMess(3) = "Drawer - " & CStr(vIndexArray(0, vIndexArray.GetLowerBound(0) + 1)) & ", " & CStr(vIndexArray(1, vIndexArray.GetLowerBound(0) + 1))

                    Case DOCFolder
                        ReDim m_sLogMess(4)
                        m_sLogMess(1) = "DocuMaster API Daemon - DocuMaster Folder deleted, " & lFolderNum


                        m_sLogMess(2) = "Cabinet - " & CStr(vIndexArray(0, vIndexArray.GetLowerBound(0))) & ", " & CStr(vIndexArray(1, vIndexArray.GetLowerBound(0)))


                        m_sLogMess(3) = "Drawer - " & CStr(vIndexArray(0, vIndexArray.GetLowerBound(0) + 1)) & ", " & CStr(vIndexArray(1, vIndexArray.GetLowerBound(0) + 1))


                        m_sLogMess(4) = "Folder - " & CStr(vIndexArray(0, vIndexArray.GetLowerBound(0) + 2)) & ", " & CStr(vIndexArray(1, vIndexArray.GetLowerBound(0) + 2))

                    Case Else

                        'Fill this in if when we develop error logging for
                        '3rd party applications that use the API

                End Select


                m_oPMBLog.DOCbPMFunc.LogMessage(LLOG, "DMSAPI", ToSafeString(m_sLogMess))

            End If

            'commit transactions
            m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse
                'Roll back
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return result

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="DelIndex", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)


            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: IsFolderEmpty
    '
    ' Description: This section is supplied a folder number and checks
    ' whether it contains any child folders or documents
    '
    ' ***************************************************************** '
    Private Function IsFolderEmpty(ByRef lParentNum As Integer, ByRef bEmpty As Boolean) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing




        result = gPMConstants.PMEReturnCode.PMTrue

        'check if any child folders exist for parent
        m_sSQL = "SELECT folder_num FROM DOC_folder WHERE parent_num = " & lParentNum

        'Hit DB
        m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="GETFOLDLIST", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Informations.IsArray(vResultArray) Then
            'children exist for this parent, may as well leave now
            bEmpty = False
            Return result
        End If


        'check if any docs exist for parent
        m_sSQL = "SELECT doc_num FROM DOC_document WHERE folder_num = " & lParentNum

        'Hit DB
        m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="GETDOCLIST", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Informations.IsArray(vResultArray) Then
            'no docs for this parent
            bEmpty = True
        Else
            bEmpty = False
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetFolderRec (Public)
    '
    ' Description: Get a fodler using the supplied external code,
    '              folder level and parent num
    '
    ' ***************************************************************** '


    Public Function GetFolderRec(ByRef lParentNum As Integer, ByRef sExCode As String, ByRef iFolderLevel As Integer, ByRef lFolderNum As Integer, ByRef sFolderName As String) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_sSQL = ""
            m_sSQL = m_sSQL & " SELECT folder_num, folder_name FROM DOC_folder"
            m_sSQL = m_sSQL & " WHERE ex_code = '" & sExCode & "'"
            m_sSQL = m_sSQL & " AND folder_level = " & CStr(iFolderLevel)
            m_sSQL = m_sSQL & " AND parent_num = " & CStr(lParentNum)

            'Hit the DB
            m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="GetFolderRec", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If Not Informations.IsArray(vResultArray) Then
                'this is wrong
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="bDOCAPI: Folder not found on Server, ex_code = " & sExCode, vApp:=ACApp, vClass:=ACClass, vMethod:="GetFolderRec", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Else

                lFolderNum = CInt(vResultArray(0, 0))

                sFolderName = CStr(vResultArray(1, 0))

            End If


            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetFolderRec", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function


    ' ***************************************************************** '
    ' Name: MergeFolder (Public)
    '
    ' Description:
    '
    '   Merges the source folder documents into target folder
    '   Updates the documents in source folder to point to new target folder
    '   Deletes the source folder in the old company. Also deletes the drawer
    '   if no folders exist in the source company and documents do not exist
    '   at the drawer level
    '
    ' ***************************************************************** '

    Public Function MergeFolder(ByRef sTargetCabExCode As Object, ByRef sTargetCabName As String, ByRef sTargetDrawExCode As String, ByRef sTargetDrawName As String, ByRef sTargetFoldExCode As String, ByRef sTargetFoldName As String, ByRef sSourceCabExCode As String, ByRef sSourceCabName As String, ByRef sSourceDrawExCode As String, ByRef sSourceDrawName As String, ByRef sSourceFoldExCode As String, ByRef sSourceFoldName As String, ByRef lSourceFoldNum As Integer, ByRef lTargetFoldNum As Integer, ByRef lSourceDrawNum As Integer, Optional ByRef oPMBLog As Object = Nothing) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim sDocName As String = ""
        Dim sDocRef As String = ""
        Dim sPageType As String = ""
        Dim sPageName As String = ""
        Dim sTmp As String = ""
        Dim sEventType As String = ""
        Dim dDocDate As Date
        Dim vDocArray(,) As Object = Nothing
        Dim vChildArray(,) As Object = Nothing
        Dim lLink, lTmp As Integer

        result = gPMConstants.PMEReturnCode.PMTrue

        Try


            ' Transfer all documents from source to target  folder

            m_sSQL = " SELECT doc_num FROM DOC_document WHERE folder_num = " & lSourceFoldNum

            'Hit DB
            m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="SELECTDOCUMENT", bStoredProcedure:=False, lNumberRecords:=-1, vResultArray:=vDocArray), gPMConstants.PMEReturnCode)
            If Informations.IsArray(vDocArray) Then


                For iRow As Integer = vDocArray.GetLowerBound(1) To vDocArray.GetUpperBound(1)

                    ' Application.DoEvents()

                    m_sSQL = "UPDATE DOC_document SET folder_num = " & lTargetFoldNum

                    m_sSQL = m_sSQL & " WHERE doc_num = " & CStr(CInt(vDocArray(0, iRow)))

                    'slap the database
                    m_lReturn = CType(m_oDatabase.SQLAction(sSQL:=m_sSQL, sSQLName:="MOVEDOCS", bStoredProcedure:=False), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    ' NOTE: if any errors occur in calls to oMisc, it'll get logged in Sirius log
                    ' so need to worry about error logging just continue

                    'get required info first

                    m_lReturn = CType(m_oMisc.GetDocDate(lDocNum:=CInt(vDocArray(0, iRow)), dDocDate:=dDocDate), gPMConstants.PMEReturnCode)


                    m_lReturn = CType(m_oMisc.ConstructDocRef(CInt(vDocArray(0, iRow)), sDocRef), gPMConstants.PMEReturnCode)

                    'update the history db to show source folder has each document deleted
                    'and target database has now these documents added

                    m_lReturn = m_oHistory.DirectAdd(vTask:=DOCDELDOCUMENT, vCabinetcode:=sSourceCabExCode, vCabinetname:=sSourceCabName, vDrawercode:=sSourceDrawExCode, vDrawername:=sSourceDrawName, vFoldercode:=sSourceFoldExCode, vFoldername:=sSourceFoldName, vDocref:=sDocRef, vRequestDate:=dDocDate.ToString("yyyyMMdd"), vRequestTime:=dDocDate.ToString("HHMMss"))

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    'get doc date

                    m_lReturn = CType(m_oMisc.GetDocDate(lDocNum:=CInt(vDocArray(0, iRow)), dDocDate:=dDocDate), gPMConstants.PMEReturnCode)

                    'get other doc details

                    m_lReturn = CType(m_oMisc.GetDocDetails(lDocNum:=CInt(vDocArray(0, iRow)), sDocName:=sDocName, sDocType:=sEventType), gPMConstants.PMEReturnCode)


                    m_lReturn = CType(m_oMisc.ConstructDocRef(CInt(vDocArray(0, iRow)), sDocRef), gPMConstants.PMEReturnCode)

                    'get the page file    (first check for links though)

                    m_lReturn = CType(m_oMisc.GetDocLink(CInt(vDocArray(0, iRow)), lLink), gPMConstants.PMEReturnCode)


                    If lLink <> 0 Then
                        lTmp = lLink
                    Else

                        lTmp = CInt(vDocArray(0, iRow))
                    End If

                    m_lReturn = CType(m_oMisc.GetPageName(lDocNum:=lTmp, sPageName:=sPageName), gPMConstants.PMEReturnCode)

                    'remove the obliques and soliduses
                    m_lReturn = CType(StripSlashes(sPageName, sTmp), gPMConstants.PMEReturnCode)

                    'get the page type
                    m_lReturn = CType(m_oMisc.GetPageType(lDocNum:=lTmp, sPageType:=sPageType), gPMConstants.PMEReturnCode)
                    'hit the history table
                    m_lReturn = m_oHistory.DirectAdd(vTask:=DOCADDDOCUMENT, vCabinetcode:=sTargetCabExCode, vCabinetname:=sTargetCabName, vDrawercode:=sTargetDrawExCode, vDrawername:=sTargetDrawName, vFoldercode:=sTargetFoldExCode, vFoldername:=sTargetFoldName, vDocref:=sDocRef, vRequestDate:=dDocDate.ToString("yyyyMMdd"), vRequestTime:=dDocDate.ToString("HHMMss"), vEventtype:=sEventType, vDescription:=sDocName, vVolume:=DOCHD1_NAME, vPagefile:=sTmp, vDoctype:=sPageType)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If



                Next iRow

            End If ' If IsArray(vDocArray)


            'Now delete the source folder
            m_sSQL = "DELETE FROM DOC_folder WHERE folder_num = " & lSourceFoldNum

            ' Execute SQL Statement
            m_lReturn = CType(m_oDatabase.SQLAction(sSQL:=m_sSQL, sSQLName:="DELETEGENERALFOLDER", bStoredProcedure:=False), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' update history database
            m_lReturn = m_oHistory.DirectAdd(vTask:=DOCDELFOLDER, vCabinetcode:=sSourceCabExCode, vCabinetname:=sSourceCabName, vDrawercode:=sSourceDrawExCode, vDrawername:=sSourceDrawName, vFoldercode:=sSourceFoldExCode, vFoldername:=sSourceFoldName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If


            ' if the source drawer\client has no documents in it AND no child folders exist then delete it

            m_sSQL = " SELECT folder_num FROM DOC_folder WHERE parent_num = " & lSourceDrawNum

            'Hit the DB - should be only one record match
            m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="GETCHILDFOLDER", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=vChildArray), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' no folders exist within drawer
            If Not Informations.IsArray(vChildArray) Then

                ' get all source documents from source folder
                m_sSQL = " SELECT doc_num FROM DOC_document WHERE folder_num = " & lSourceDrawNum

                'Hit the DB - should be only one record match
                m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=m_sSQL, sSQLName:="GETDOCUMENTS", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=vDocArray), gPMConstants.PMEReturnCode)


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' and no documents exist at client level
                If Not Informations.IsArray(vDocArray) Then

                    ' Now delete the drawer  - client folder does not have any folders
                    m_sSQL = "DELETE FROM DOC_folder WHERE folder_num = " & lSourceDrawNum

                    ' Execute SQL Statement
                    m_lReturn = CType(m_oDatabase.SQLAction(sSQL:=m_sSQL, sSQLName:="DELETEFOLDER", bStoredProcedure:=False), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    ' update history db
                    m_lReturn = m_oHistory.DirectAdd(vTask:=DOCDELDRAWER, vCabinetcode:=sSourceCabExCode, vCabinetname:=sSourceCabName, vDrawercode:=sSourceDrawExCode, vDrawername:=sSourceDrawName)


                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="MergeFolder", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function


    ' ***************************************************************** '
    ' Name: TransferFolder (Public)      Data Merge
    '
    ' Description:
    '
    '   Transfers content of a folder from one company to another
    '   in source company
    '   Updates the documents in source folder to point to new target folder
    '   Deletes the source folder in the old company, then deletes the drawer
    '   if no folders exist in the source company
    '
    '   THIS FUNCTION IS NOT YET USED       MS 19/02/01
    '
    ' ***************************************************************** '
    '
    'Public Function TransferFolder(sTargetCabExCode, _
    ''                                sTargetCabName As String, _
    ''                                sTargetDrawExCode As String, _
    ''                                sTargetDrawName As String, _
    ''                                sTargetFoldExCode As String, _
    ''                                sTargetFoldName As String, _
    ''                                sSourceCabExCode As String, _
    ''                                sSourceCabName As String, _
    ''                                sSourceDrawExCode As String, _
    ''                                sSourceDrawName As String, _
    ''                                sSourceFoldExCode As String, _
    ''                                sSourceFoldName As String, _
    ''                                lSourceDrawNum As Long, _
    ''                                lSourceFoldNum As Long, _
    ''                                lTargetFoldNum As Long, _
    ''                                lTargetDrawNum As Long, _
    ''                                Optional oPMBLog As Object) As Long
    '
    'Dim sDocName            As String
    'Dim sDocRef             As String
    'Dim sPageType           As String
    'Dim sPageName           As String
    'Dim sTmp                As String
    'Dim sEventType          As String
    'Dim sUniqueExCode       As String
    'Dim dDocDate            As Date
    'Dim vResultArray        As Variant
    'Dim vDocArray           As Variant
    'Dim vChildArray         As Variant
    'Dim lTargetGeneralNum   As Long
    'Dim lNewFoldNum         As Long
    'Dim lLink               As Long
    'Dim lTmp                As Long
    'Dim iRow                As Integer
    '
    '    TransferFolder = PMTrue
    '
    '    On Error GoTo Err_TransferFolder
    '
    '
    '    ' excode for folders at claim/policy level should be unique
    '    ' if a ex_code at folder level to be copied over in the target client
    '    ' already exists then log it
    '
    '    m_sSQL = ""
    '    m_sSQL = m_sSQL & " SELECT ex_code FROM DOC_folder"
    '    m_sSQL = m_sSQL & " WHERE ex_code = '" & sSourceFoldExCode & "'"
    '    m_sSQL = m_sSQL & " AND folder_level = " & DOCFolder
    '    m_sSQL = m_sSQL & " AND parent_num = " & lTargetDrawNum
    '
    '    m_lReturn& = m_oDatabase.SQLSelect(sSQL:=m_sSQL$, _
    ''                                        sSQLName:="SELECTFOLDER", _
    ''                                        bstoredprocedure:=False, _
    ''                                        lNumberRecords:=1, _
    ''                                        vResultArray:=vResultArray)
    '
    '    If (m_lReturn& <> PMTrue) Then
    '        TransferFolder = PMFalse
    '        Exit Function
    '    End If
    '
    '    If IsArray(vResultArray) Then
    '        ' log entry because there will a duplicate folder level ex_code in the target drawer
    '        If (m_oPMBLog Is Nothing <> True) Then
    '            ReDim m_sLogMess(7)
    '            m_sLogMess(1) = "DocuMaster API Daemon -  WARNING "
    '            m_sLogMess(2) = "two folders now have the same ex code in the target company"
    '            m_sLogMess(3) = "Folder  Ex Code :" & sTargetFoldExCode
    '            m_sLogMess(4) = "In"
    '            m_sLogMess(5) = "Drawer  Number  :" & lTargetDrawNum
    '            m_sLogMess(6) = "Drawer  Ex Code :" & sTargetDrawExCode
    '            m_sLogMess(7) = "Cabinet Ex Code :" & sTargetCabExCode
    '
    '            m_oPMBLog.DOCbPMFunc.LogMessage LLOG, "DMSAPI", m_sLogMess()
    '        End If
    '    End If
    '
    '    ' using source folder details add a new folder to target company
    '    m_lReturn& = m_oFolder.DirectAdd(vFolderNum:=lNewFoldNum, _
    ''                                    vFolderName:=sSourceFoldName, _
    ''                                    vParentNum:=lTargetDrawNum, _
    ''                                    vExCode:=sSourceFoldExCode, _
    ''                                    vFolderLevel:=DOCFolder)
    '    If (m_lReturn& <> PMTrue) Then
    '        TransferFolder = PMFalse
    '        Exit Function
    '    End If
    '
    '    ' Update the history database
    '    ' using source folder details add a new folder to target company
    '    m_lReturn = m_oHistory.DirectAdd(vTask:=DOCADDFOLDER, _
    ''                                    vCabinetCode:=sTargetCabExCode, _
    ''                                    vCabinetName:=sTargetCabName, _
    ''                                    vDrawerCode:=sTargetDrawExCode, _
    ''                                    vDrawerName:=sTargetDrawName, _
    ''                                    vFolderCode:=sSourceFoldExCode, _
    ''                                    vFolderName:=sSourceFoldName)
    '
    '    If (m_lReturn& <> PMTrue) Then
    '        TransferFolder = PMFalse
    '        Exit Function
    '    End If
    '
    '    ' get all source documents from source folder
    '    m_sSQL$ = " SELECT doc_num FROM DOC_document WHERE folder_num = " & lSourceFoldNum
    '
    '    'Hit DB
    '    m_lReturn& = m_oDatabase.SQLSelect(sSQL:=m_sSQL$, _
    ''                                       sSQLName:="SELECTDOCUMENT", _
    ''                                       bstoredprocedure:=False, _
    ''                                       lNumberRecords:=-1, _
    ''                                       vResultArray:=vDocArray)
    '
    '    If (m_lReturn& <> PMTrue) Then
    '        TransferFolder = PMFalse
    '        Exit Function
    '    End If
    '
    '    'update the history db to show source folder has each document deleted
    '    'and target database has now these documents added
    '
    '    If IsArray(vDocArray) = True Then
    '
    '        For iRow% = LBound(vDocArray, 2) To UBound(vDocArray, 2)
    '
    '
    '            m_sSQL$ = "UPDATE DOC_document SET folder_num = " & lNewFoldNum
    '            m_sSQL$ = m_sSQL & " WHERE doc_num = " & CLng(vDocArray(0, iRow%))
    '
    '            'slap the database
    '            m_lReturn& = m_oDatabase.SQLAction( _
    ''                sSQL:=m_sSQL$, _
    ''                sSQLName:="TRANSFERDOCS", _
    ''                bstoredprocedure:=False)
    '
    '            If (m_lReturn& <> PMTrue) Then
    '                TransferFolder = PMFalse
    '                Exit Function
    '            End If
    '
    '
    '            'get required info first
    '            m_lReturn& = m_oMisc.GetDocDate(lDocNum:=CLng(vDocArray(0, iRow%)), _
    ''                                                     dDocDate:=dDocDate)
    '
    '            If (m_lReturn& <> PMTrue) Then
    '                TransferFolder = PMFalse
    '                Exit Function
    '            End If
    '
    '            m_lReturn& = m_oMisc.ConstructDocRef(CLng(vDocArray(0, iRow%)), _
    ''                                                               sDocRef$)
    '
    '            If (m_lReturn& <> PMTrue) Then
    '                TransferFolder = PMFalse
    '                Exit Function
    '            End If
    '
    '            'hit the history table
    '            m_lReturn = m_oHistory.DirectAdd(vTask:=DOCDELDOCUMENT, _
    ''                                            vCabinetCode:=sSourceCabExCode, _
    ''                                            vCabinetName:=sSourceCabName, _
    ''                                            vDrawerCode:=sSourceDrawExCode, _
    ''                                            vDrawerName:=sSourceDrawName, _
    ''                                            vFolderCode:=sSourceFoldExCode, _
    ''                                            vFolderName:=sSourceFoldName, _
    ''                                            vDocRef:=sDocRef$, _
    ''                                            vRequestDate:=Format$(dDocDate, "yyyymmdd"), _
    ''                                            vRequestTime:=Format$(dDocDate, "hhmmss"))
    '
    '            If (m_lReturn& <> PMTrue) Then
    '                TransferFolder = PMFalse
    '                Exit Function
    '            End If
    '
    '
    '            'get doc date
    '            m_lReturn& = m_oMisc.GetDocDate(lDocNum:=CLng(vDocArray(0, iRow%)), _
    ''                                                     dDocDate:=dDocDate)
    '
    '            If (m_lReturn& <> PMTrue) Then
    '                TransferFolder = PMFalse
    '                Exit Function
    '            End If
    '
    '            'get other doc details
    '            m_lReturn& = m_oMisc.GetDocDetails(lDocNum:=CLng(vDocArray(0, iRow%)), _
    ''                                                        sDocName:=sDocName$, _
    ''                                                        sDocType:=sEventType$)
    '
    '            If (m_lReturn& <> PMTrue) Then
    '                TransferFolder = PMFalse
    '                Exit Function
    '            End If
    '
    '            m_lReturn& = m_oMisc.ConstructDocRef(CLng(vDocArray(0, iRow%)), _
    ''                                                            sDocRef$)
    '
    '            If (m_lReturn& <> PMTrue) Then
    '                TransferFolder = PMFalse
    '                Exit Function
    '            End If
    '
    '            'get the page file    (first check for links though)
    '            m_lReturn& = m_oMisc.GetDocLink(CLng(vDocArray(0, iRow%)), lLink&)
    '
    '            If (m_lReturn& <> PMTrue) Then
    '                TransferFolder = PMFalse
    '                Exit Function
    '            End If
    '
    '            If (lLink& <> 0) Then
    '                lTmp& = lLink&
    '            Else
    '                lTmp& = CLng(vDocArray(0, iRow%))
    '            End If
    '
    '            m_lReturn& = m_oMisc.GetPageName(lDocNum:=lTmp&, _
    ''                                                       sPageName:=sPageName$)
    '
    '            If (m_lReturn& <> PMTrue) Then
    '                TransferFolder = PMFalse
    '                Exit Function
    '            End If
    '
    '            'remove the obliques and soliduses
    '            m_lReturn& = StripSlashes(sPageName$, sTmp$)
    '
    '            If (m_lReturn& <> PMTrue) Then
    '                TransferFolder = PMFalse
    '                Exit Function
    '            End If
    '
    '            'get the page type
    '            m_lReturn& = m_oMisc.GetPageType(lDocNum:=lTmp&, _
    ''                                                        sPageType:=sPageType$)
    '            If (m_lReturn& <> PMTrue) Then
    '                TransferFolder = PMFalse
    '                Exit Function
    '            End If
    '
    '            ' hit the history table
    '            ' target client's folder name and excode details is same as source client's
    '            m_lReturn = m_oHistory.DirectAdd(vTask:=DOCADDDOCUMENT, _
    ''                                            vCabinetCode:=sTargetCabExCode, _
    ''                                            vCabinetName:=sTargetCabName, _
    ''                                            vDrawerCode:=sTargetDrawExCode, _
    ''                                            vDrawerName:=sTargetDrawName, _
    ''                                            vFolderCode:=sSourceFoldExCode, _
    ''                                            vFolderName:=sSourceFoldName, _
    ''                                            vDocRef:=sDocRef$, _
    ''                                            vRequestDate:=Format$(dDocDate, "yyyymmdd"), _
    ''                                            vRequestTime:=Format$(dDocDate, "hhmmss"), _
    ''                                            vEventType:=sEventType$, _
    ''                                            vDescription:=sDocName$, _
    ''                                            vVolume:=DOCHD1_NAME, _
    ''                                            vPageFile:=sTmp$, _
    ''                                            vDocType:=sPageType$)
    '
    '            If (m_lReturn& <> PMTrue) Then
    '                TransferFolder = PMFalse
    '                Exit Function
    '            End If
    '
    '
    '
    '        Next iRow%
    '
    '    End If  ' If IsArray(vDocArray)
    '
    '    ' Now delete the source clent/policy folder
    '    m_sSQL$ = "DELETE FROM DOC_folder WHERE folder_num = " & lSourceFoldNum
    '
    '    ' Execute SQL Statement
    '    m_lReturn& = m_oDatabase.SQLAction( _
    ''                            sSQL:=m_sSQL, _
    ''                            sSQLName:="DELETEFOLDER", _
    ''                            bstoredprocedure:=False)
    '
    '    If (m_lReturn& <> PMTrue) Then
    '        TransferFolder = PMFalse
    '        Exit Function
    '    End If
    '
    '    ' all docs updated, so update history database to show source folder has been deleted
    '    m_lReturn = m_oHistory.DirectAdd(vTask:=DOCDELFOLDER, _
    ''                                    vCabinetCode:=sSourceCabExCode, _
    ''                                    vCabinetName:=sSourceCabName, _
    ''                                    vDrawerCode:=sSourceDrawExCode, _
    ''                                    vDrawerName:=sSourceDrawName, _
    ''                                    vFolderCode:=sSourceFoldExCode, _
    ''                                    vFolderName:=sSourceFoldName)
    '
    '    If (m_lReturn& <> PMTrue) Then
    '        TransferFolder = PMFalse
    '        Exit Function
    '    End If
    '
    '
    '
    '    ' if the source drawer has no documents in it AND no child folders exist
    '    ' then it is safe to delete it
    '
    '    m_sSQL = " SELECT folder_num FROM DOC_folder WHERE parent_num = " & lSourceDrawNum
    '
    '    'Hit the DB - should be only one record match
    '    m_lReturn& = m_oDatabase.SQLSelect(sSQL:=m_sSQL$, _
    ''                                        sSQLName:="GETCHILDFOLDER", _
    ''                                        bstoredprocedure:=False, _
    ''                                        lNumberRecords:=1, _
    ''                                        vResultArray:=vChildArray)
    '    If (m_lReturn& <> PMTrue) Then
    '        TransferFolder = PMFalse
    '        Exit Function
    '    End If
    '
    '    If (IsArray(vChildArray) = False) Then
    '
    '        ' get all source documents from source folder
    '        m_sSQL$ = " SELECT doc_num FROM DOC_document WHERE folder_num = " & lSourceDrawNum
    '
    '        'Hit the DB - should be only one record match
    '        m_lReturn& = m_oDatabase.SQLSelect(sSQL:=m_sSQL$, _
    ''                                            sSQLName:="GETDOCUMENTS", _
    ''                                            bstoredprocedure:=False, _
    ''                                            lNumberRecords:=1, _
    ''                                            vResultArray:=vDocArray)
    '
    '
    '        If (m_lReturn& <> PMTrue) Then
    '            TransferFolder = PMFalse
    '            Exit Function
    '        End If
    '
    '        If (IsArray(vDocArray) = False) Then
    '
    '            ' Now delete the drawer  - client folder does not have any folders
    '            m_sSQL$ = "DELETE FROM DOC_folder WHERE folder_num = " & lSourceDrawNum
    '
    '            ' Execute SQL Statement
    '            m_lReturn& = m_oDatabase.SQLAction( _
    ''                                    sSQL:=m_sSQL, _
    ''                                    sSQLName:="DELETEFOLDER", _
    ''                                    bstoredprocedure:=False)
    '
    '            If (m_lReturn& <> PMTrue) Then
    '                TransferFolder = PMFalse
    '                Exit Function
    '            End If
    '            ' update history db
    '            m_lReturn = m_oHistory.DirectAdd(vTask:=DOCDELDRAWER, _
    ''                                            vCabinetCode:=sSourceCabExCode, _
    ''                                            vCabinetName:=sSourceCabName, _
    ''                                            vDrawerCode:=sSourceDrawExCode, _
    ''                                            vDrawerName:=sSourceDrawName)
    '
    '
    '            If (m_lReturn& <> PMTrue) Then
    '                TransferFolder = PMFalse
    '                Exit Function
    '            End If
    '
    '        End If
    '
    '    End If
    '
    '    Exit Function
    '
    'Err_TransferFolder:
    '
    '    ' Error.
    '    TransferFolder = PMError
    '
    '    ' Log Error Message
    '    bPMFunc.LogMessage sUsername:=g_sUsername, _
    ''        iType:=PMiPMFunc.LogError, _
    ''        sMsg:=gPMConstants.PMErrorText, _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="TransferFolder", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    '
    '
    'End Function


    ' MS 19/02/01 THIS FUNCTION IS NOT YET USED - incomplete

    'Public Function TransferGeneralFolder(sCabExCode As String, _
    ''                                sDrawerExCode As String, _
    ''                                lFolderNum As Long, _
    ''                                lSourceFoldNum As Long, _
    ''                                lDestFoldNum As Long) As Long
    '
    '
    '
    '
    '  '
    '    ' add a general folder to the target client if it does not exist
    '    '
    '
    '
    '    ' check if GENERAL folder already exists
    '    m_sSQL = ""
    '    m_sSQL = m_sSQL & " SELECT folder_num FROM DOC_folder"
    '    m_sSQL = m_sSQL & " WHERE ex_code = 'GENERAL'"
    '    m_sSQL = m_sSQL & " AND folder_level = " & DOCFolder
    '    m_sSQL = m_sSQL & " AND parent_num = " & lTargetDrawNum
    '
    '    'Hit the DB
    '    m_lReturn& = m_oDatabase.SQLSelect( _
    ''        sSQL:=m_sSQL$, _
    ''        sSQLName:="SELECTGENERALFOLDER", _
    ''        bstoredprocedure:=False, _
    ''        lNumberRecords:=1, _
    ''        vResultArray:=vResultArray)
    '
    '
    '    If (m_lReturn& <> PMTrue) Then
    '        MergeFolder = PMFalse
    '        Exit Function
    '    End If
    '
    '    'target GENERAL folder exists
    '    If IsArray(vResultArray) Then
    '        lTargetGeneralNum = CLng(vResultArray(0, 0))
    '    End If
    '
    '    'target GENERAL folder does not exist
    '    If IsArray(vResultArray) = False Then
    '
    '        'Add the general folder into the database
    '        m_lReturn& = m_oFolder.DirectAdd(vFolderNum:=lFolderNum, _
    ''                                            vFolderName:="GENERAL", _
    ''                                            vParentNum:=lTargetDrawNum, _
    ''                                            vExCode:="GENERAL", _
    ''                                            vFolderLevel:=DOCFolder)
    '
    '        If (m_lReturn& <> PMTrue) Then
    '
    '        MergeFolder = PMFalse
    '
    '        bPMFunc.LogMessage sUsername:=g_sUsername, _
    ''        iType:=PMiPMFunc.LogError, _
    ''        sMsg:="Failed to Add Folder. Index = " & _
    ''                                            sTargetCabExCode & "|" & _
    ''                                            sTargetDrawExCode & "|" & _
    ''                                            "GENERAL" & "|", _
    ''                vApp:=ACApp, _
    ''                vClass:=ACClass, _
    ''                vMethod:="AddFolder", _
    ''                vErrNo:=Err.Number, _
    ''                vErrDesc:=Err.Description
    '
    '            Exit Function
    '        End If
    '
    '        ' set newly created folder_num
    '        lTargetGeneralNum = lFolderNum
    '
    '
    '        'Update history db for general folder
    '        m_lReturn = m_oHistory.DirectAdd(vTask:=DOCADDFOLDER, _
    ''                                        vCabinetCode:=sTargetCabExCode, _
    ''                                        vCabinetName:=sTargetCabName, _
    ''                                        vDrawerCode:=sTargetDrawExCode, _
    ''                                        vDrawerName:=sTargetDrawName, _
    ''                                        vFolderCode:="GENERAL", _
    ''                                        vFolderName:="GENERAL")
    '
    '        If (m_lReturn& <> PMTrue) Then
    '
    '            MergeFolder = PMFalse
    '
    '            bPMFunc.LogMessage sUsername:=g_sUsername, _
    ''                iType:=PMiPMFunc.LogError, _
    ''                sMsg:="Failed to update History Database. Index = " & _
    ''                                                    sTargetCabExCode & "|" & _
    ''                                                    sTargetCabName & "|" & _
    ''                                                    sTargetDrawExCode & "|" & _
    ''                                                    sTargetDrawName & "|" & _
    ''                                                    "GENERAL" & "|" & _
    ''                                                    "GENERAL" & "|", _
    ''                vApp:=ACApp, _
    ''                vClass:=ACClass, _
    ''                vMethod:="MergeFolder", _
    ''                vErrNo:=Err.Number, _
    ''                vErrDesc:=Err.Description
    '
    '            Exit Function
    '        End If
    '
    '    End If  'target GENERAL folder does not exist
    '
    '
    '
    'End Function
    '


    ' *********************************************************************************** '
    ' Name: RebuildRemoteDB

    ' Description:
    '
    '       This function rebuilds the Remote History Database (RHD) from scratch.
    '       Adds cabinets, drawers, folders and documents
    '       Documents in folder level will be added only into RHD
    '       It is assumed the RHD on the UNIX is cleared down first before this is run
    '
    ' *********************************************************************************** '


    Private Function FolderNameExists(ByRef sFolderName As String, ByRef lFolderNum As Integer, ByRef lParentNum As Object) As Integer

        Dim result As Integer = 0
        'Const kMethodName As String = "FolderNameExists"

        Dim vResultArray(,) As Object = Nothing
        Dim sSQL As String = ""




        result = gPMConstants.PMEReturnCode.PMTrue

        'Construct SQL
        sSQL = ""
        sSQL = sSQL & "SELECT" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    folder_num" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "FROM doc_folder" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "WHERE folder_name = '" & sFolderName & "'" & Strings.ChrW(13) & Strings.ChrW(10)

        sSQL = sSQL & "AND parent_num = " & CStr(lParentNum) & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "AND ISNULL(ex_code, '') = ''" & Strings.ChrW(13) & Strings.ChrW(10)

        m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="FolderNameExists", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            RaiseError("m_oDatabase.SQLSelect", "Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        'Return the parent num.
        If Informations.IsArray(vResultArray) Then
            lFolderNum = ToSafeLong(vResultArray(0, 0))
        Else
            lFolderNum = 0
            Return result
        End If

        ' Do any tidy up, e.g. Set x = Nothing here
        Return result
        ' This is for debugging only
    End Function
    Public Function AddDocumentDirect(ByVal sDocName As String, ByVal sFilename As String, ByVal sDocType As String, ByVal lFolderNum As Long, ByVal iAccessLevel As Integer, ByVal sUsername As String, Optional ByVal vDocumentTemplateID As Object = Nothing, Optional ByVal bVisibleFromWeb As Boolean = False, Optional ByVal sPageType As String = "") As Integer

        Const kMethodName As String = "AddDocumentDirect"

        Dim bCreateAllFolders As Boolean
        Dim lDocNum As Long
        Dim sPageName As New StringsHelper.FixedLengthString(15)
        Static sDataPath As String
        Dim sTmpDocRef As String = ""
        Dim dNowDate As Date
        Dim sTmpPageName As String = ""
        Dim sTmp As String
        Dim bStartTrans As Boolean
        Dim sFolderName As String = "" ''PN 66472

        Try
            AddDocumentDirect = gPMConstants.PMEReturnCode.PMTrue


            'set access level
            g_iAccessLevel% = iAccessLevel%

            'Commence database updates
            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("BeginTrans()", "Function failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            bStartTrans = True

            'This gets set to true as soon as a folder is created, as obviously
            'all subsequent child folders will need to be created too, so there
            'is no need to check if they already exist
            bCreateAllFolders = False

            ' Check if text, as we wont be zipping these
            sTmp$ = "Y"



            '' This Below Function developed for mapping the Renewal Document with Doc Folder Contents
            m_lReturn = RenewalDocMapping(v_sFolderName:=sFolderName, r_lParentNum:=lFolderNum) 'PN: 66472

            'We now have the index, so lets add the document record
            m_lReturn = m_oDocument.DirectAdd(vDocNum:=lDocNum,
                                               vFolderNum:=lFolderNum,
                                               vDocName:=Trim(sDocName),
                                               vExCode:=Trim(sDocName),
                                               vDocType:=sDocType,
                                               vAccessLevel:=g_iAccessLevel,
                                               vZipped:=sTmp,
                                               vDocumentTemplateID:=vDocumentTemplateID,
                                               bVisibleFromWeb:=bVisibleFromWeb)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("m_oDocument.DirectAdd", "Function failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' MS 16/08/01
            ' doc ex_code is now set to the doc_num in order to make it unique instead of

            m_sSQL$ = "UPDATE DOC_document SET ex_code = doc_num WHERE doc_num = " & lDocNum

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=m_sSQL,
                                                sSQLName:="UPDATEDOCUMENTABLE",
                                                bStoredProcedure:=False)

            ' if UPDATE failed jut log message and carry on
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                bPMFunc.LogMessage(sUsername:=g_sUsername,
                    iType:=gPMConstants.PMELogLevel.PMLogError,
                    sMsg:="Could not UPDATE ex_code in DOCUMENT table",
                    vApp:=ACApp,
                    vClass:=ACClass,
                    vMethod:="AddDocumentDirect",
                    vErrNo:=Informations.Err.Number,
                    vErrDesc:=Informations.Err.Description)

            End If

            'Get and store now date
            dNowDate = DateTime.Now
            'now lets add the doc info record
            m_lReturn = m_oDocInfo.DirectAdd(vDocNum:=lDocNum&,
                                                vExpiryDate:=dNowDate,
                                                vScanUser:=Trim$(sUsername),
                                                vDocDate:=dNowDate,
                                                vLastUser:=Trim$(sUsername),
                                                vLastDate:=dNowDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("m_oDocInfo.DirectAdd", "Function failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Now do the page add stuff

            'Get the next page name to use
            'Get the next page name to use
            m_lReturn = CType(m_oMisc.GetNextPageName(sNextPageName:=sPageName.Value), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("m_oMisc.GetNextPageName", "Function failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            'Add the page record
            'sp todo - may need some sort of retry ? - should become avail in pmdao
            'We now have the index, so lets add the document record
            m_lReturn = m_oPage.DirectAdd(vPageName:=sPageName.Value,
                                            vDocNum:=lDocNum&,
                                            vPageNum:=1,
                                            vPageType:=sPageType,
                                            vPageSize:=0,
                                            vVolumeID:=DOCHD1_ID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("m_oPage.DirectAdd", "Function failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            'DN 07/12/00 - Populate modular variable with added document number
            m_lDocNum = lDocNum

            'sp todo Create doc links in case linkfolder set . See notes?

            'Get path to where data is on live, HD volume.
            If (sDataPath$ = "") Then
                m_lReturn = m_oMisc.GetDataPath(DOCHD1_ID, sDataPath)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError("m_oMisc.GetDataPath", "Function failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            ' Create path for file copy
            m_lReturn = CType(MakePath(sDataPath & sPageName.Value), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("MakePath", sDataPath & sPageName.Value, gPMConstants.PMELogLevel.PMLogError)
            End If

            'Update the history database - note, if we are not actually linked to
            'an external database most of these params will be empty, but it matters
            'not as the call will not do anything.
            m_lReturn = m_oMisc.ConstructDocRef(lDocNum&, sTmpDocRef$)

            m_lReturn = CType(StripSlashes(sPageName.Value, sTmpPageName), gPMConstants.PMEReturnCode)

            m_lReturn = m_oHistory.DirectAdd(vTask:=DOCADDDOCUMENT,
                                            vCabinetcode:="",
                                            vCabinetname:="",
                                            vDrawercode:="",
                                            vDrawername:="",
                                            vFoldercode:="",
                                            vFoldername:="",
                                            vDocref:=sTmpDocRef$,
                                            vRequestDate:=dNowDate.ToString("yyyyMMdd"),
                                            vRequestTime:=dNowDate.ToString("hhmmss"),
                                            vEventtype:=sDocType$,
                                            vDescription:=sDocName,
                                            vVolume:=DOCHD1_NAME,
                                            vPagefile:=sTmpPageName$,
                                            vDoctype:=sPageType$)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("m_oHistory.DirectAdd", "Function failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            Dim cloudHostingOptionValue As String = ""
            m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:="", v_sPassword:="", v_iUserID:=0, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=0, v_iCurrencyID:=0, v_iLogLevel:=0, v_sCallingAppName:=ACApp, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableCloudHosting, v_vBranch:=1, r_vUnderwriting:=cloudHostingOptionValue)

            If (gPMFunctions.NullToString(cloudHostingOptionValue) = "1") Then
                Dim s3NewPageName As String = (sPageName.Value & "." & sPageType).ToString.Replace("\", "/").TrimStart("/")
                'move from temp location to 00 tree
                Dim repository As IS3Repository = New S3Repository(Environment.GetEnvironmentVariable("AWS_DME_BUCKET_NAME"),
                    Environment.GetEnvironmentVariable("AWS_REGION"),
                    g_sUsername)

                m_lReturn = CType(repository.UploadFile(s3NewPageName, System.IO.File.ReadAllBytes(sFilename)).Result, gPMConstants.PMEReturnCode)
            Else
                'if text dont compress, as may need to view via UNIPLEX/History Viewer
                m_lReturn = CType(DOCGeneralFunc.CopyFile(sFileIn:=sFilename, sFileOut:=sDataPath & sPageName.Value & "." & sPageType), gPMConstants.PMEReturnCode)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'try and kill the file in case it moved something there
                m_lReturn = CType(KillFile(sDataPath & sPageName.Value & "." & sPageType), gPMConstants.PMEReturnCode)
                RaiseError("CopyFile", "sFileIn:=" & sFilename, gPMConstants.PMELogLevel.PMLogError)
            End If

            ' All OK so delete the original file
            m_lReturn = CType(KillFile(sFilename), gPMConstants.PMEReturnCode)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                'note file not deleted, but OK to continue
                bPMFunc.LogMessage(sUsername:=g_sUsername,
                    iType:=gPMConstants.PMELogLevel.PMLogError,
                    sMsg:="Failed in KillFile",
                    vApp:=ACApp,
                    vClass:=ACClass,
                    vMethod:="AddDocumentDirect",
                    vErrNo:=Informations.Err.Number,
                    vErrDesc:=Informations.Err.Description)
            End If


            'commit transactions
            m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("CommitTrans()", "Function failed", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As System.Exception

            'Do not call any functions before here or the error will be lost
            bPMFunc.LogError(
                v_sUsername:=g_sUsername,
                v_sClass:=ACClass,
                v_sMethod:=kMethodName,
                r_lFunctionReturn:=AddDocumentDirect,
                excep:=ex)

            ' If you want to rollback a transaction or something, do it here
            If bStartTrans Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
            End If


        End Try


    End Function
End Class
