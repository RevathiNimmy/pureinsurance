Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
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

    Dim sFolderName As String = ""
    Dim iAccessLevel As Integer
    Dim dCreateDate As Date
    Dim sDocName As String = ""
    Dim sScanUser As String = ""
    Dim dExpiryDate As Date
    Dim dDocDate As Date

    ' Database Class (Private)
#If PD_EARLYBOUND = 1 Then

	Private m_oDatabase As DPMDAO.Database
#Else
    Private m_oDatabase As dPMDAO.Database
#End If

    'Miscellaneous class
    Private m_oMisc As bDOCInformation.Miscellaneous

    ' History object
    Private m_oHistory As bDOCHistory.Form

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
                iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenameDoc", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            'call function that actually does it
            m_lReturn = CType(m_oMisc.RenameDoc(lDocNum:=lDocNum, sNewName:=sNewName), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return result
            End If

            'Everything fine so commit
            m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenameDoc", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="RenameDoc", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            ' Have we a valid Database Object Reference?

            If (Not Information.IsNothing(vDatabase)) And (Information.IsReference(vDatabase)) Then
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
                m_lReturn = CType(NewDatabase(v_sUsername:=g_sUsername, v_iSourceID:=g_iSourceID, v_iLanguageID:=g_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFDocumaster, r_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                ' Close Database in Terminate() method
                m_bCloseDatabase = True
            End If

            'Get the History business object reference for writing to the DB
            m_oHistory = New bDOCHistory.Form()

            m_lReturn = m_oHistory.Initialise(sUsername:=g_sUsername, sPassword:=g_sPassword.Value, iUserID:=g_iUserID, iSourceID:=g_iSourceID, iLanguageID:=g_iLanguageID, iCurrencyID:=g_iCurrencyID, iLogLevel:=g_iLogLevel, sCallingAppName:=g_sCallingAppName, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'instance the misc class
            m_oMisc = New bDOCInformation.Miscellaneous()

            m_lReturn = m_oMisc.Initialise(v_sUsername:=g_sUsername, v_sPassword:=g_sPassword.Value, v_iUserID:=g_iUserID, v_iSourceID:=g_iSourceID, v_iLanguageID:=g_iLanguageID, v_iCurrencyID:=g_iCurrencyID, v_iLogLevel:=g_iLogLevel, v_sCallingAppName:=g_sCallingAppName, vDatabase:=m_oDatabase, vHistory:=m_oHistory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
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
            Me.disposedValue = True
            If disposing Then
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                If m_oHistory IsNot Nothing Then
                    m_oHistory.Dispose()
                End If
                m_oDatabase = Nothing
                m_oMisc = Nothing
                m_oHistory = Nothing
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
            iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    ' Name:         GetDocInfo
    '
    ' Description:  Gets document number from document table
    '               Gets information from tables.
    ' ***************************************************************** '
    Public Function GetDocInfo(ByRef lDocNum As Integer, ByRef sFolderName As String, ByRef iAccessLevel As Integer, ByRef bExternal As Boolean, ByRef dCreateDate As Date, ByRef sDocName As String, ByRef dDocDate As Date, ByRef sScanUser As String, ByRef dExpiryDate As Date) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim sSQLStatement As String = ""
        Dim vResultArray(,) As Object


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'executes SQL statement to retrieve folder number, access
            'level, create date, doc name, from document table
            sSQLStatement = "SELECT folder_num, access_level, create_date, " & _
                            "doc_name FROM DOC_document WHERE doc_num = " & CStr(lDocNum)


            m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=sSQLStatement, sSQLName:="GetDocInfo", lNumberRecords:=1, bStoredProcedure:=False, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)




            sFolderName = CStr(vResultArray(0, 0))

            iAccessLevel = CInt(vResultArray(1, 0))

            dCreateDate = CDate(vResultArray(2, 0))

            sDocName = CStr(vResultArray(3, 0))

            'executes SQL statement to retrieve Foldername and external code from
            'Folder table

            sSQLStatement = "SELECT folder_name, ex_code FROM DOC_Folder WHERE folder_num = " & CStr(vResultArray(0, 0))

            m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=sSQLStatement, sSQLName:="GetDocInfo", lNumberRecords:=1, bStoredProcedure:=False, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)


            sFolderName = CStr(vResultArray(0, 0))


            bExternal = Not (CStr(vResultArray(1, 0)).Trim() = "")

            'executes SQL statement to retrieve Doc date, expiry date
            'scan user from doc_info table
            sSQLStatement = "SELECT doc_date, expiry_date, " & _
                            "scan_user FROM DOC_doc_info WHERE doc_num = " & CStr(lDocNum)

            m_lReturn = CType(m_oDatabase.SQLSelect(sSQL:=sSQLStatement, sSQLName:="pp", lNumberRecords:=1, bStoredProcedure:=False, vResultArray:=vResultArray), gPMConstants.PMEReturnCode)


            dDocDate = CDate(vResultArray(0, 0))

            dExpiryDate = CDate(vResultArray(1, 0))

            sScanUser = CStr(vResultArray(2, 0))

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Terminate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Terminate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         GetDocNames
    '
    ' Description:  Calls GetDocNames method from miscellaneous class
    ' ***************************************************************** '

    Public Function GetDocNames(ByRef vDocNamesArray(,) As Object) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(m_oMisc.GetDocNames(vDocNamesArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Terminate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Terminate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)


            Return result
        End Try
    End Function


    Public Function AddAnnotation(ByRef lDocNum As Integer, ByRef sAnnText As String, ByRef sUsername As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Instance of annotation
            Dim oAnnotation As bDOCAnnotation.Form

            'Get the Annotation business object reference for writing to the DB
            oAnnotation = New bDOCAnnotation.Form()

            ' initialise the object
            m_lReturn = CType(oAnnotation, SSP.S4I.Interfaces.IBusiness).Initialise(sUsername:=g_sUsername, sPassword:=g_sPassword.Value, iUserID:=g_iUserID, iSourceID:=g_iSourceID, iLanguageID:=g_iLanguageID, iCurrencyID:=g_iCurrencyID, iLogLevel:=g_iLogLevel, sCallingAppName:="iDOCInformation")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' add the annotation
            m_lReturn = oAnnotation.DirectAdd(vDocNum:=lDocNum, vAnnText:=sAnnText, vUsername:=sUsername)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' terminate the object
            oAnnotation.Dispose()
            oAnnotation = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add annotation", vApp:=ACApp, vClass:=ACClass, vMethod:="AddAnnotation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
End Class