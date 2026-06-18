Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Data.SqlClient
'refer Developer Guide No. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 07/05/1999
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              User Defined Data Maintenance.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 22/12/2003
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
    Private Const ACClass As String = "Business"

    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Database Class (Private)
    Private m_oArcDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Close Database Flag (Private)
    Private m_bCloseArcDatabase As Boolean

    Private m_vDataDictionary As Object

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private m_lGISDataModelId As Integer

    Private m_lStatus As Integer

    Private Const ACAdd As Integer = 1
    Private Const ACDelete As Integer = 2

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    ' Rule Properties
    Private m_sDescription As String = ""
    Private m_sCode As String = ""
    Private m_iIsDeleted As Integer
    Private m_lCaptionId As Integer


    Public Property CaptionId() As Integer
        Get
            Return m_lCaptionId
        End Get
        Set(ByVal Value As Integer)
            m_lCaptionId = Value
        End Set
    End Property


    Public Property IsDeleted() As Integer
        Get
            Return m_iIsDeleted
        End Get
        Set(ByVal Value As Integer)
            m_iIsDeleted = Value
        End Set
    End Property

    Public Property Code() As String
        Get
            Return m_sCode
        End Get
        Set(ByVal Value As String)
            m_sCode = Value
        End Set
    End Property


    Public Property Description() As String
        Get
            Return m_sDescription
        End Get
        Set(ByVal Value As String)
            m_sDescription = Value
        End Set
    End Property


    Public Property EffectiveDate() As Date
        Get
            Return m_dtEffectiveDate
        End Get
        Set(ByVal Value As Date)
            m_dtEffectiveDate = Value
        End Set
    End Property


    Public Property SourceId() As Integer
        Get
            Return m_iSourceID
        End Get
        Set(ByVal Value As Integer)
            m_iSourceID = Value
        End Set
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property



    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUserName As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceId As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUserName
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceId
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel




            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get an instance to the architecture database
            m_lReturn = CType(gPMComponentServices.NewDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_oDatabase:=m_oArcDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_bCloseArcDatabase = True


            ' Set Username and Password

            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                m_vDataDictionary = Nothing
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
                End If
                If m_bCloseArcDatabase Then

                    m_oArcDatabase.CloseDatabase()

                    m_oArcDatabase = Nothing

                End If

			End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.


            If Not Information.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Information.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Information.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Information.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: Update (Public)
    '
    ' Description: Updates all data
    '
    ' ***************************************************************** '
    Public Function Update(ByVal v_vRuleSetLinks As Object) As Integer
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)

            'First we delete all the Rule Set Link Records before adding them all back in.
            'I hate this method !!

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteAuthorityRuleLinksSQL, sSQLName:=ACDeleteAuthorityRuleLinksName, bStoredProcedure:=ACDeleteAuthorityRuleLinksStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return result
            End If

            'this need to be after the delete because we've now deleted everything from the listview - so must delete it from the database
            If Not Information.IsArray(v_vRuleSetLinks) Then
                m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)
                Return result
            End If


            For iCount As Integer = 0 To v_vRuleSetLinks.GetUpperBound(1)

                ' Clear the Database Parameters Collection
                m_oDatabase.Parameters.Clear()


                m_lReturn = m_oDatabase.Parameters.Add(sName:="product_id", vValue:=CStr(v_vRuleSetLinks(ACUARuleProductId, iCount)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                    Return result
                End If


                m_lReturn = m_oDatabase.Parameters.Add(sName:="authority_level_type_id", vValue:=CStr(v_vRuleSetLinks(ACUARuleTypeId, iCount)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                    Return result
                End If


                m_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_type_id", vValue:=CStr(v_vRuleSetLinks(ACUARuleTransTypeId, iCount)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                    Return result
                End If


                m_lReturn = m_oDatabase.Parameters.Add(sName:="is_underwriter", vValue:=CStr(v_vRuleSetLinks(ACUARuleIsUnderwriter, iCount)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                    Return result
                End If


                m_lReturn = m_oDatabase.Parameters.Add(sName:="rule_set_id", vValue:=CStr(v_vRuleSetLinks(ACUARuleId, iCount)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                    Return result
                End If

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACInsertAuthorityRuleLinksSQL, sSQLName:=ACInsertAuthorityRuleLinksName, bStoredProcedure:=ACInsertAuthorityRuleLinksStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                    Return result
                End If
            Next iCount

            m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError
            m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: GetCaptionID
    '
    ' Description: Calls the spu_pm_caption_id_return stored procedure
    '              to either get or create a caption_id
    '
    ' ***************************************************************** '
    Private Function GetCaptionID(ByVal v_sCaption As String, ByRef r_lCaptionId As Integer) As Integer

        Dim result As Integer = 0


        ' m_iLanguageID

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the parameters
        m_oArcDatabase.Parameters.Clear()

        ' Add the parameters
        m_lReturn = m_oArcDatabase.Parameters.Add(sName:="language_id", vValue:=CStr(m_iLanguageID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oArcDatabase.Parameters.Add(sName:="caption", vValue:=v_sCaption, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oArcDatabase.Parameters.Add(sName:="caption_id", vValue:=CStr(r_lCaptionId), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Perform the stored procedure
        m_lReturn = m_oArcDatabase.SQLAction(sSQL:=ACSQLCaptionReturn, sSQLName:=ACSQLCaptionReturnName, bStoredProcedure:=ACSQLCaptionReturnStored)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the returned caption_id
        r_lCaptionId = m_oArcDatabase.Parameters.Item("caption_id").Value

        Return result

    End Function


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




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
		Dispose(False)
    End Sub


    ' ***************************************************************** '
    '
    ' Name: GetLookupValues
    '
    ' Description: Retrieves array of standard PM Lookup values.
    '
    ' History: 01/11/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function GetLookupValues(ByRef v_iLanguageID As Integer, ByRef v_dtEffectiveDate As Date, ByRef v_sTableName As String, ByRef r_vLookupArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add the language id
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Language_ID", vValue:=CStr(v_iLanguageID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Add the effective date
            'Developer Guide No. 40
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Effective_Date", vValue:=v_dtEffectiveDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Add the table name
            ' RAG Fix 18/8/2002 - Parameter Type was set to PMInteger!
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Table_Name", vValue:=v_sTableName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMTableName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectAllSQL, sSQLName:=ACSelectAllName, bStoredProcedure:=ACSelectAllStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vLookupArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do we have any records ?
            If Not Information.IsArray(r_vLookupArray) Then
                ' No Records, return PMNotFound
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookupValues Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: AddCaption
    '
    ' Description: Inserts caption into Architecture database and returns
    '               new Id.
    '
    ' History: 06/11/2000 RWH - Created.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (AddCaption) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function AddCaption(ByRef v_sCaption As String, ByRef r_lCaptionId As Integer) As Integer
    '
    'Dim result As Integer = 0
    'Dim oArchDatabase As dPMDAO.Database
    '
    'Const sADD_PM_CAPTION As String = "{call spu_pm_caption_id_return (?,?,?)}"
    'Const sADD_PM_CAPTION_NAME As String = "AddPMCaption"
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    '
    'm_lReturn = CType(gPMComponentServices.NewDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_oDatabase:=oArchDatabase), gPMConstants.PMEReturnCode)
    '
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'oArchDatabase.Parameters.Clear()
    '
    'm_lReturn = oArchDatabase.Parameters.Add(sName:="language_id", vValue:=CStr(m_iLanguageID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'm_lReturn = RollbackTrans()
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = oArchDatabase.Parameters.Add(sName:="caption", vValue:=v_sCaption, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '        m_lReturn = RollbackTrans()
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = oArchDatabase.Parameters.Add(sName:="caption_id", vValue:=CStr(0), idirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '        m_lReturn = RollbackTrans()
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = oArchDatabase.SQLAction(sSQL:=sADD_PM_CAPTION, sSQLName:=sADD_PM_CAPTION_NAME, bStoredProcedure:=True)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '        m_lReturn = RollbackTrans()
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Get returned caption_id to pass into AddScheme proc.
    'r_lCaptionId = oArchDatabase.Parameters.Item("caption_id").Value
    'oArchDatabase.CloseDatabase()
    'oArchDatabase = Nothing
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddCaption Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddCaption", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function


    '' ***************************************************************** '
    ''
    '' Name: DeleteRule
    ''
    '' Description:
    ''
    '' History: 09/11/2000 RWH - Created.
    ''
    '' ***************************************************************** '
    'Public Function DeleteRule(ByVal v_lRuleId As Long, _
    ''                            ByVal v_lAuthLevelTypeId As Long, _
    ''                            ByVal v_iIsUnderwriter As Integer, _
    ''                            ByVal v_lProductId As Long, _
    ''                            ByVal v_lTransType As Long) As Long
    '
    '    On Error GoTo Err_DeleteRule
    '
    '    DeleteRule = PMTrue
    '
    '    m_lReturn = BeginTrans()
    '
    '    m_oDatabase.Parameters.Clear
    '
    '    m_lReturn = m_oDatabase.Parameters.Add(sName:="authority_level_type_id", _
    ''                                           vValue:=v_lAuthLevelTypeId, _
    ''                                           iDirection:=PMParamInput, _
    ''                                           iDataType:=PMLong)
    '
    '    If (m_lReturn& <> PMTrue) Then
    '        DeleteRule = PMFalse
    '        m_lReturn = RollbackTrans()
    '        Exit Function
    '    End If
    '
    '    m_lReturn = m_oDatabase.Parameters.Add(sName:="is_underwriter", _
    ''                                           vValue:=v_iIsUnderwriter, _
    ''                                           iDirection:=PMParamInput, _
    ''                                           iDataType:=PMInteger)
    '
    '    If (m_lReturn& <> PMTrue) Then
    '        DeleteRule = PMFalse
    '        m_lReturn = RollbackTrans()
    '        Exit Function
    '    End If
    '
    '    m_lReturn = m_oDatabase.Parameters.Add(sName:="product_id", _
    ''                                           vValue:=v_lProductId, _
    ''                                           iDirection:=PMParamInput, _
    ''                                           iDataType:=PMLong)
    '
    '    If (m_lReturn& <> PMTrue) Then
    '        DeleteRule = PMFalse
    '        m_lReturn = RollbackTrans()
    '        Exit Function
    '    End If
    '
    '    m_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_type_id", _
    ''                                           vValue:=v_lTransType, _
    ''                                           iDirection:=PMParamInput, _
    ''                                           iDataType:=PMLong)
    '
    '    If (m_lReturn& <> PMTrue) Then
    '        DeleteRule = PMFalse
    '        m_lReturn = RollbackTrans()
    '        Exit Function
    '    End If
    '
    '    m_lReturn = m_oDatabase.Parameters.Add(sName:="rule_set_id", _
    ''                                           vValue:=v_lRuleId, _
    ''                                           iDirection:=PMParamInput, _
    ''                                           iDataType:=PMLong)
    '
    '    If (m_lReturn& <> PMTrue) Then
    '        DeleteRule = PMFalse
    '        m_lReturn = RollbackTrans()
    '        Exit Function
    '    End If
    '
    '    ' Execute SQL Statement
    '    m_lReturn& = m_oDatabase.SQLAction( _
    ''        sSQL:=ACDeleteAuthorityRuleLinkSQL, _
    ''        sSQLName:=ACDeleteAuthorityRuleLinkName, _
    ''        bStoredProcedure:=ACDeleteAuthorityRuleLinkStored)
    '
    '    If (m_lReturn& <> PMTrue) Then
    '        DeleteRule = PMFalse
    '        m_lReturn = RollbackTrans()
    '        Exit Function
    '    End If
    '
    '    m_lReturn = CommitTrans()
    '
    '    Exit Function
    '
    'Err_DeleteRule:
    '
    '    DeleteRule = PMError
    '
    '    m_lReturn = RollbackTrans()
    '
    '    ' Log Error Message
    '    LogMessage m_sUsername, _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="DeleteRule Failed", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="DeleteRule", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    '
    'End Function

    ' ***************************************************************** '
    '
    ' Name: GetRuleSet
    '
    ' Description:
    '
    ' History: 22/12/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function GetRuleSet(ByRef r_vRuleSets(,) As Object, Optional ByVal v_lRuleSetId As Integer = 0) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            If v_lRuleSetId <> 0 Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="rule_set_id", vValue:=CStr(v_lRuleSetId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                'refer Developer Guide No. 85(Guide)

                m_lReturn = m_oDatabase.Parameters.Add("rule_set_type", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else

                'Developer Guide No. 85(Guide)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="rule_set_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="rule_set_type", vValue:=CStr(ACRuleSetTypeUnderwritingAuthority), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectRuleSetsSQL, sSQLName:=ACSelectRuleSetsName, bStoredProcedure:=ACSelectRuleSetsStored, vResultArray:=r_vRuleSets)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do we have any records ?
            If Not Information.IsArray(r_vRuleSets) Then
                ' No Records, return PMNotFound
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRuleSet Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRuleSet", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: GetRuleType from DB
    '
    ' ***************************************************************** '
    Public Function GetRuleType(ByVal v_nRuleType As Integer, ByRef r_oResultArray(,) As Object) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Try

            m_oDatabase.Parameters.Clear()

            nResult = m_oDatabase.Parameters.Add(sName:="rule_set_id", vValue:=CStr(v_nRuleType), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Return m_oDatabase.SQLSelect(sSQL:=ACSelectRuleSetsSQL, sSQLName:=ACSelectRuleSetsName, bStoredProcedure:=ACSelectRuleSetsStored, vResultArray:=r_oResultArray)

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRuleType  Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRuleType", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetRuleSetLinks
    '
    ' Description:
    '
    ' History: 04/01/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function GetRuleSetLinks(ByRef r_vRules(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectAuthorityRuleLinksSQL, sSQLName:=ACSelectAuthorityRuleLinksName, bStoredProcedure:=ACSelectAuthorityRuleLinksStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vRules)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do we have any records ?
            If Not Information.IsArray(r_vRules) Then
                ' No Records, return PMNotFound
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRuleSetLinks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRuleSetLinks", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: UpdateRuleSet
    '
    ' Description:
    '
    ' History: 08/01/2001 RWH - Created.
    '
    ' ***************************************************************** '

    Public Overloads Function UpdateRuleSet(ByVal v_iTask As Integer, ByRef r_lRuleSetId As Integer, _
                                           ByVal v_sCode As String, ByVal v_sDescription As String, _
                                           ByVal v_dtEffectiveDate As Date, ByVal v_sFileName As String, _
                                           ByVal v_iLive As Integer) As Integer
        Return UpdateRuleSet(v_iTask:=v_iTask, r_lRuleSetId:=r_lRuleSetId, v_sCode:=v_sCode, _
                             v_sDescription:=v_sDescription, v_dtEffectiveDate:=v_dtEffectiveDate, _
                             v_iLive:=v_iLive, v_sFileName:=v_sFileName, v_lRiskTypeRuleSetTypeID:=0)

    End Function
    Public Overloads Function UpdateRuleSet(ByVal v_iTask As Integer, ByRef r_lRuleSetId As Integer, _
                                            ByVal v_sCode As String, ByVal v_sDescription As String, _
                                            ByVal v_dtEffectiveDate As Date, ByVal v_sFileName As String, _
                                            ByVal v_iLive As Integer, ByVal v_lRiskTypeRuleSetTypeID As Integer) As Integer

        Dim result As Integer = 0
        Dim lCaptionId As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'get caption id
            m_lReturn = CType(GetCaptionID(v_sDescription, lCaptionId), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'update Risk Type Rule Set details (there should only be one record)
            m_oDatabase.Parameters.Clear()

            'Add risk type ID
            If v_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="rule_set_id", vValue:=CStr(r_lRuleSetId), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="rule_set_id", vValue:=CStr(r_lRuleSetId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="caption_id", vValue:=CStr(lCaptionId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=v_sCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="description", vValue:=v_sDescription, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_deleted", vValue:=CStr(gPMConstants.PMEReturnCode.PMFalse), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Developer Guide No. 40
            m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=v_dtEffectiveDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="rule_set_type", vValue:=CStr(ACRuleSetTypeUnderwritingAuthority), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="file_name", vValue:=v_sFileName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="live", vValue:=CStr(v_iLive), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_rule_set_type_id", vValue:=CStr(v_lRiskTypeRuleSetTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If v_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACInsertRuleSetSQL, sSQLName:=ACInsertRuleSetName, bStoredProcedure:=ACInsertRuleSetStored)

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    'get new risk_type_rule_set_id back to display on parent's listview
                    r_lRuleSetId = m_oDatabase.Parameters.Item("rule_set_id").Value
                End If
            Else
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateRuleSetSQL, sSQLName:=ACUpdateRuleSetName, bStoredProcedure:=ACUpdateRuleSetStored)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return CommitTrans()

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateRuleSet Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRuleSet", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    
        
    ' ***************************************************************** '
    '
    ' Name: GetTransactionTypeList
    '
    ' Description:
    '
    ' History: 01/04/2003 sj - Created.
    '
    ' ***************************************************************** '
    Public Function GetTransactionTypeList(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Return m_oDatabase.SQLSelect(sSQL:=ACSelectUALTransactionTypeSQL, sSQLName:=ACSelectUALTransactionTypeName, bStoredProcedure:=ACSelectUALTransactionTypeStored, vResultArray:=r_vResultArray)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTransactionTypeList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTransactionTypeList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ''' <summary>
    ''' get the Rule Type from DB
    ''' </summary>
    ''' <param name="r_oResultArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetRuleTypes(ByRef r_oResultArray(,) As Object) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue

        Try
            ' Execute SQL Statement - use array for speed
            m_oDatabase.Parameters.Clear()

            nResult = m_oDatabase.SQLSelect( _
                sSQL:=ACSaaRuleTypeSQL, _
                sSQLName:=ACSaaRuleTypeName, _
                bStoredProcedure:=ACSaaRuleTypeStored, _
                vResultArray:=r_oResultArray)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
            End If
        Catch ex As Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRuleTypes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRuleTypes", excep:=ex)
        End Try
        Return nResult
    End Function
End Class
