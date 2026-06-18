Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Data.SqlClient
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Created: PW301002
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a PMUPolicy.
    '              Based on BSIRRenSelection.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 19/12/2003
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

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)
    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    'RiskData object
    Private m_oDataSet As cGISDataSetControl.Application
    'Private m_proProgress As Object

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Calling Application Name

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private lPMAuthorityLevel As Integer
    Private m_lNumberOPolicies As Integer



    ' Primary Keys to work with
    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property



    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)

            Value = Value

        End Set
    End Property

    ' ***************************************************************** '
    '
    ' Name: GetGracePeriod
    '
    ' Description: Gets the Grace Period (in days) for the product type
    '              of the passed Product ID
    '
    ' History: PW301002 - created
    '
    ' ***************************************************************** '
    Public Function GetGracePeriod(ByVal v_lProductID As Object, ByRef r_lGracePeriodDays As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear parameters collection
            m_oDatabase.Parameters.Clear()

            ' Add the Product ID parameter

            m_lReturn = m_oDatabase.Parameters.Add(sName:="product_id", vValue:=CStr(v_lProductID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Grace Period Days parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="grace_period_days", vValue:=CStr(r_lGracePeriodDays), idirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call the stored procedure
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetGracePeriodSQL, sSQLName:=ACGetGracePeriodName, bStoredProcedure:=ACGetGracePeriodStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the return parameteres
            r_lGracePeriodDays = gPMFunctions.NullToLong(m_oDatabase.Parameters.Item("grace_period_days").Value)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetGracePeriod Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetGracePeriod", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SetRisksUnquoted
    '
    ' Description: Set all risk status' to "unquoted" for the passed
    '              insurance file cnt.
    '
    ' History: PW301002 - created
    '
    ' ***************************************************************** '
    Public Function SetRisksUnquoted(ByVal v_lInsuranceFileCnt As Object, ByRef r_lRecordsAffected As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear parameters collection
            m_oDatabase.Parameters.Clear()

            ' Add the insurance file cnt parameter

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call the stored procedure
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACSetRisksUnquotedSQL, sSQLName:=ACSetRisksUnquotedName, bStoredProcedure:=ACSetRisksUnquotedStored, lRecordsAffected:=r_lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetRisksUnquoted Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetRisksUnquoted", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SetRisksStatus
    '
    ' Description: Set all risk status to passed in status for the passed
    '              insurance file cnt.
    '
    ' History: CJR 17/1/2003 created
    '
    ' ***************************************************************** '
    Public Function SetRisksStatus(ByVal v_lInsuranceFileCnt As Object, ByRef r_sStatusCode As String, ByRef r_lRecordsAffected As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear parameters collection
            m_oDatabase.Parameters.Clear()

            ' Add the insurance file cnt parameter

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the insurance file cnt parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=r_sStatusCode, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call the stored procedure
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACSetRisksStatusSQL, sSQLName:=ACSetRisksStatusName, bStoredProcedure:=ACSetRisksStatusStored, lRecordsAffected:=r_lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetRisksStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetRisksStatus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


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
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel




            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oDataSet = New cGISDataSetControl.Application()

            ' Set Username and Password

            m_lCurrentRecord = 0
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
            Me.disposedValue = True
            If disposing Then
                If m_oDataSet IsNot Nothing Then
                    m_oDataSet.Dispose()
                    m_oDataSet = Nothing
                End If
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
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
    ' Name: GetLookUp (Private)
    '
    ' Description: get values from look up table
    '
    ' ***************************************************************** '
    Public Function GetLookUp(ByVal v_sTableName As String, ByVal v_sKeyIDFieldName As String, ByVal v_sDescFieldName As String, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            sSQL = "SELECT " & v_sKeyIDFieldName & ", " & v_sDescFieldName &
                    " FROM " & v_sTableName &
                    " WHERE is_deleted = 0 " &
                   " ORDER BY " & v_sDescFieldName

            m_oDatabase.Parameters.Clear()


            Return m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetLookupValues", bStoredProcedure:=False, vresultArray:=r_vResultArray, bKeepNulls:=True)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookUp Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookUp", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



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
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
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
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
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
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function


    ' private Methods (End)


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


    Public Function GetAgentCancellationDetails(ByVal AgentCnt As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            sSQL = "SELECT date_cancelled from party_agent where party_cnt = " & AgentCnt

            m_oDatabase.Parameters.Clear()


            Return m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetAgentCancellation", bStoredProcedure:=False, vresultArray:=r_vResultArray, bKeepNulls:=True)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAgentCancellationDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAgentCancellationDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)


            Return result
        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: SetRisksQuoteStatus
    '
    ' Description: Set all risk status' to required status for the passed
    '              insurance file cnt.
    '
    ' History: PW301002 - created
    '
    ' ***************************************************************** '
    Public Function SetRisksQuoteStatus(ByVal v_lInsuranceFileCnt As Object, ByVal v_iIsMTA As Byte, ByVal v_sRiskCode As Object, ByRef r_lRecordsAffected As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear parameters collection
            m_oDatabase.Parameters.Clear()

            ' Add the insurance file cnt parameter

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the risk code parameter

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Code", vValue:=CStr(v_sRiskCode), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call the stored procedure
            If v_iIsMTA = 1 Then
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACSetRisksQuoteStatusMTASQL, sSQLName:=ACSetRisksQuoteStatusMTAName, bStoredProcedure:=ACSetRisksQuoteStatusMTAStored, lRecordsAffected:=r_lRecordsAffected)
            Else
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACSetRisksQuoteStatusMTASQL, sSQLName:=ACSetRisksQuoteStatusMTAName, bStoredProcedure:=ACSetRisksQuoteStatusNBStored, lRecordsAffected:=r_lRecordsAffected)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetRisksQuoteStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetRisksQuoteStatus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    ''' <summary>
    ''' Check if the leading agent is a sub
    ''' </summary>
    ''' <param name="v_vLeadAgentCnt"></param>
    ''' <returns></returns>
    ''' <remarks>ValidateLeadAgent</remarks>
    Public Function ValidateLeadAgent(ByVal v_vLeadAgentCnt As Integer) As Integer

        Dim nResult As Integer
        Dim sSQL As String = ""
        Dim vresultArray(,) As Object


        Try

            nResult = gPMConstants.PMEReturnCode.PMFalse

            sSQL = ""
            sSQL = "select code from party_agent_type where party_agent_type_id in "
            sSQL = sSQL & "(select party_Agent_type_id from party_agent where party_cnt = " &
                   Conversion.Str(v_vLeadAgentCnt) & ")"

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="ValidateLeadAgent", bStoredProcedure:=False,
                                              vResultArray:=vresultArray, bKeepNulls:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            If Not Information.IsArray(vresultArray) Then
                'There are no agents so carry on anyway
                Return gPMConstants.PMEReturnCode.PMTrue
            End If


            If CStr(vresultArray(0, 0)).Trim().ToUpper() = ("Sub-Agent").ToUpper() Then
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                'the agent isn't a sub so carry on
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

        Catch
        End Try


        nResult = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateLeadAgent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateLeadAgent", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return nResult
    End Function

    ' ***************************************************************** '
    ' Name : GetRenewalFrequencyDetail
    '
    ' Desc : get details from renewal frequency table
    '
    ' Hist : 29/09/2003 Thinh Nguyen - Created
    '
    ' ***************************************************************** '

    Public Function GetRenewalFrequencyDetail(ByVal v_lFrequencyID As Integer, ByRef r_vResult(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT [Code], [Description], [Number_Of_Months] FROM Renewal_Frequency WHERE renewal_frequency_id = " & v_lFrequencyID

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetRenewalFrequencyDetails", bStoredProcedure:=False, vresultArray:=r_vResult)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Information.IsArray(r_vResult) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRenewalFrequencyDetail Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRenewalFrequencyDetail", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    'Get Default Branch Agent
    Public Function GetDefaultBranchAgent(ByVal r_iSourceID As Integer, ByRef m_vAgentArray(,) As Object) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add("Source_id", CStr(r_iSourceID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)


            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetBranchDefAgentSQL, sSQLName:=ACGetBranchDefAgentName, bStoredProcedure:=False, vresultArray:=m_vAgentArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefaultBranchAgent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaultBranchAgent", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    'Start (Sriram P) Tech Spec - WR8 - Navigator DME Link.doc section(4.3.1)
    'To Get Client code from party Count
    Public Function GetClientCode(ByVal v_iPartyID As Integer, ByRef r_vClientarray(,) As Object) As Integer
        Dim Catch_Renamed As Boolean = False

        Dim result As Integer = 0
        Const kMethodName As String = "GetClientCode"
        Try
            Catch_Renamed = True



            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(v_iPartyID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "party_cnt Failed", gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetClientCodeSQL, sSQLName:=ACGetClientCode, bStoredProcedure:=ACGetClientCodeStored, vresultArray:=r_vClientarray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACGetClientCodeSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If

            Return result

        Catch excep As System.Exception
            If Not Catch_Renamed Then
                Throw excep
            End If

            GoTo Finally_Renamed

            If Catch_Renamed Then


                ' DO Not Call any functions before here or the error will be lost
                bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=excep)

                ' If you want to rollback a transaction or something, do it here

            End If
Finally_Renamed:
        End Try
    End Function

    'End (Sriram P) Tech Spec - WR8 - Navigator DME Link.doc section(4.3.1)

    'Start (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTA - Product Option.doc) - (5.1.3.1)
    Public Function BackDatedMTAsAllowed(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lRecordsAffected(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "BackDatedMTAsAllowed"

        Try




            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear parameters collection
            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACBackDatedMTAsAllowedSQL, sSQLName:=ACBackDatedMTAsAllowedName, bStoredProcedure:=ACBackDatedMTAsAllowedStored, vresultArray:=r_lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '		Return result
            '		Resume 


            '		Return result
        End Try
        Return result
    End Function
    'End (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTA - Product Option.doc) - (5.1.3.1)
    'Allowing back dated cancellation PN-63205
    Public Function BackDatedCanAllowed(ByVal v_lInsuranceFileCnt As Integer, ByRef vvalue(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "BackDatedCanAllowed"

        Try




            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear parameters collection
            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vvalue:=CStr(v_lInsuranceFileCnt), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If





            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACBackDatedCanAllowedSQL, sSQLName:=ACBackDatedCanAllowedName, bStoredProcedure:=ACBackDatedCanAllowedStored, vresultArray:=vvalue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume


            '        Return result
        End Try
        Return result
    End Function
    'Get Associated Agent
    'Defect #2748
    Public Function GetAssociatedAgent(ByVal r_iUserID As Integer, ByRef m_vAgentArray(,) As Object) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add("User_id", CStr(r_iUserID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAssociatedAgentSQL, sSQLName:=ACGetAssociatedAgent, bStoredProcedure:=False, vResultArray:=m_vAgentArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefaultBranchAgent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaultBranchAgent", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    Public Function GetAssociatedAgentWithBranch(ByVal r_iUserID As Integer, ByRef m_vAgentArray(,) As Object, Optional ByVal r_iSourceID As Integer = 0) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add("User_id", CStr(r_iUserID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            m_lReturn = m_oDatabase.Parameters.Add("Source_id", CStr(r_iSourceID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)


            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAssociatedAgentBranchSQL, sSQLName:=ACGetAssociatedAgentBranch, bStoredProcedure:=False, vResultArray:=m_vAgentArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefaultBranchAgent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaultBranchAgent", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    Public Function SetRisksInceptionDate(ByVal v_lInsuranceFileCnt As Long,
                                            ByVal v_dtCoverFromDate As Date) As Long

        Const kMethodName As String = "SetRisksInceptionDate"
        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            SetRisksInceptionDate = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt",
                                                   vValue:=v_lInsuranceFileCnt,
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="inception_date",
                                                   vValue:=v_dtCoverFromDate,
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMDate)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACSetRisksInceptionDateSQL,
                                                sSQLName:=ACSetRisksInceptionDateName,
                                                bStoredProcedure:=ACSetRisksInceptionDateStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If




        Catch excep As Exception


            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetRisksInceptionDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
            ' DO Not Call any functions before here or the error will be lost


            ' If you want to rollback a transaction or something, do it here

        End Try
    End Function



    Public Function IsBackdatedMTARequired(ByVal v_lInsuranceFolderCnt As Integer,
                                           ByVal v_dtEffectiveDate As Date,
                                           ByVal v_lNewInsuranceFileCnt As Integer) As Boolean

        Dim result As Boolean = False
        Dim lPolicyVersion, lErrorCode As Integer
        Dim bBackdatingRequired As Boolean
        Dim sInsuranceFileTypeCode As String = ""
        Dim iBDMTAAlloweddates As Integer
        Dim vArray(,) As Object
        Dim iMTAAllocation, iMTAAllowedDates As Integer
        Dim vAffectedInsuranceFileCnts(,) As Object
        Try

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="InsFileCnt", vValue:=v_lNewInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetInsFileTypeSQL, sSQLName:=ACGetInsFileTypeName, bStoredProcedure:=False, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Information.IsArray(vArray) Then
                Return result
            End If
            sInsuranceFileTypeCode = CStr(vArray(0, 0))


            If sInsuranceFileTypeCode <> "MTAQTETEMP" Then
                lErrorCode = 1
            Else
                lErrorCode = 0
            End If

            ''GetOutOfSequenceMTAProductDetails
            vArray = Nothing
            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=v_lNewInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) ' Sankar - Changed PMInteger to PMLong

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACOutOfSequenceMTADetailsSQL, sSQLName:=ACOutOfSequenceMTADetailsName, bStoredProcedure:=ACOutOfSequenceMTADetailsStored, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetOutOfSequenceMTAProductDetails", "Failed to fetch MTA allocation details", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not Information.IsArray(vArray) Then
                Return result
            End If

            iMTAAllocation = CInt(vArray(0, 0))
            iMTAAllowedDates = CInt(vArray(1, 0))




            'Find Insurance
            Dim sClassName As String
            Dim oFindInsurance As bSIRFindInsurance.Form

            sClassName = "bSIRFindInsurance.Form"
            oFindInsurance = New bSIRFindInsurance.Form
            m_lReturn = oFindInsurance.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            m_lReturn = oFindInsurance.GetVersionsByDate(r_lInsuranceFileCnt:=v_lNewInsuranceFileCnt,
                                         v_dtStartDate:=m_dtEffectiveDate,
                                         r_lPolicyVersion:=lPolicyVersion,
                                         r_lErrorCode:=lErrorCode,
                                         r_bBackdatingRequired:=bBackdatingRequired,
                                         r_vAffectedInsuranceFileCnts:=vAffectedInsuranceFileCnts)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetVersionByDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsBackDatedMTARequired")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'Bracket () Implemented in starting and before bBackdatingRequired (PN-72127) Nitesh Dwivedi
            If Information.IsArray(vAffectedInsuranceFileCnts) Then

                If (UBound(vAffectedInsuranceFileCnts, 2) > 0 And iBDMTAAlloweddates = 1) Or (UBound(vAffectedInsuranceFileCnts, 2) >= 0 And iBDMTAAlloweddates > 1) And bBackdatingRequired Then
                    'More than one version affected
                    result = True
                End If
            End If
            If Not (oFindInsurance Is Nothing) Then
                oFindInsurance.Dispose()
                oFindInsurance = Nothing
            End If

            Return result

        Catch excep As System.Exception


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsBackDatedMTARequired Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsBackDatedMTARequired", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return m_lReturn



        End Try
    End Function
    Public Function CreateBusinessObject(ByRef r_oObject As Object, ByVal v_sClassName As String) As Integer

        Dim result As Integer = 0
        Dim lReturnCode As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_oObject = Activator.CreateInstance(System.Reflection.Assembly.GetAssembly(Type.GetType(v_sClassName + "," + v_sClassName.Substring(0, v_sClassName.LastIndexOf(".")))).FullName, v_sClassName).Unwrap()


            lReturnCode = r_oObject.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            ' Check for errors.
            If lReturnCode <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Set the object to nothing

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create object instance (" & v_sClassName & ")", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBusinessObject", excep:=excep)

            Return result
        End Try
    End Function

    ''' <summary>
    ''' GetAssosiatedAgentBranch
    ''' </summary>
    ''' <param name="r_iSourceID"></param>
    ''' <param name="v_vLeadAgentCnt"></param>
    ''' <param name="v_sTransactionType"></param>
    ''' <param name="m_vBranchArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAssosiatedAgentBranch(ByVal r_iSourceID As Integer, ByVal v_vLeadAgentCnt As Object,
                                             ByVal v_sTransactionType As String, ByRef m_vBranchArray(,) As Object) As Long
        Dim nResult As Integer = 0

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            nResult = m_oDatabase.Parameters.Add("Branchid", r_iSourceID, gPMConstants.PMEParameterDirection.PMParamInput,
                                                 gPMConstants.PMEDataType.PMInteger)
            nResult = m_oDatabase.Parameters.Add("Party_Cnt", v_vLeadAgentCnt,
                                                 gPMConstants.PMEParameterDirection.PMParamInput,
                                                 gPMConstants.PMEDataType.PMInteger)
            If v_sTransactionType <> "" Then
                nResult = m_oDatabase.Parameters.Add("TransType", v_sTransactionType,
                                                     gPMConstants.PMEParameterDirection.PMParamInput,
                                                     gPMConstants.PMEDataType.PMString)
            End If
            nResult = m_oDatabase.SQLSelect(sSQL:=kGetAssosiatedAgentBranchSQL,
                                            sSQLName:=kGetAssosiatedAgentBranchName, bStoredProcedure:=True,
                                            vResultArray:=m_vBranchArray)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                GetAssosiatedAgentBranch = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            Return nResult

        Catch ex As Exception
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefaultBranchAgent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaultBranchAgent", vErrNo:=Information.Err().Number, vErrDesc:=ex.Message, excep:=ex)
            Return nResult
        End Try
    End Function
    ''' <summary>
    ''' DeleteRisksRI
    ''' </summary>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="r_nRecordsAffected"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DeleteRisksRI(ByVal nInsuranceFileCnt As Integer,
                                  ByRef r_nRecordsAffected As Integer) As Integer

        Dim nResult As Integer = 0
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Clear parameters collection
            m_oDatabase.Parameters.Clear()

            ' Add the insurance file cnt parameter
            nResult = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt",
                                                    vValue:=nInsuranceFileCnt,
                                                    iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                    iDataType:=gPMConstants.PMEDataType.PMInteger)
            If (nResult <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call the stored procedure
            nResult = m_oDatabase.SQLAction(sSQL:=ACSetRisksRISQL,
                                            sSQLName:=ACSetRisksRIName,
                                            bStoredProcedure:=ACSetRisksRIStored,
                                            lRecordsAffected:=r_nRecordsAffected)

            If (nResult <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return nResult

        Catch ex As Exception
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteRisksRI Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteRisksRI", vErrNo:=Information.Err().Number, vErrDesc:=ex.Message, excep:=ex)
            Return nResult
        End Try
    End Function
    ''' <summary>
    ''' IsPendingPortfolioTransfer
    ''' </summary>
    ''' <param name="sInsuranceFileRef"></param>
    ''' <param name="r_oResult"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function IsPendingPortfolioTransfer(ByVal sInsuranceFileRef As String, ByRef r_oResult(,) As Object) As Integer
        Dim nResult As Integer
        Try
            nResult = PMEReturnCode.PMTrue
            nResult = IsPendingPortfolioTransfer(sInsuranceFileRef, r_oResult, False)
            Return nResult
        Catch ex As Exception
            nResult = PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="DeleteRisksRI Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteRisksRI", vErrNo:=Information.Err().Number, vErrDesc:=ex.Message, excep:=ex)
            Return nResult
        End Try
    End Function
    ''' <summary>
    ''' IsPendingPortfolioTransfer
    ''' </summary>
    ''' <param name="sInsuranceFileRef"></param>
    ''' <param name="r_oResult"></param>
    ''' <param name="r_bIsPendingPortfolioTransfer"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function IsPendingPortfolioTransfer(ByVal sInsuranceFileRef As String, ByRef r_oResult(,) As Object,
                                                    ByRef r_bIsPendingPortfolioTransfer As Boolean) As Integer
        Dim nResult As Integer
        Try
            nResult = PMEReturnCode.PMTrue
            nResult = IsPendingPortfolioTransfer(sInsuranceFileRef, r_oResult, r_bIsPendingPortfolioTransfer, False)
            Return nResult
        Catch ex As Exception
            nResult = PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="DeleteRisksRI Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteRisksRI", vErrNo:=Information.Err().Number, vErrDesc:=ex.Message, excep:=ex)
            Return nResult
        End Try
    End Function

    ''' <summary>
    ''' IsPendingPortfolioTransfer
    ''' </summary>
    ''' <param name="sInsuranceFileRef"></param>
    ''' <param name="r_oResult"></param>
    ''' <param name="r_bIsPendingPortfolioTransfer"></param>
    ''' <param name="r_bIsPendingCloneTransfer"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function IsPendingPortfolioTransfer(ByVal sInsuranceFileRef As String, ByRef r_oResult(,) As Object,
                                                    ByRef r_bIsPendingPortfolioTransfer As Boolean, ByRef r_bIsPendingCloneTransfer As Boolean) As Integer

        Const kMethodName As String = "IsPendingPortfolioTransfer"
        Dim vValue As Object
        Dim nReturn As Integer
        Dim dtResult As DataTable = Nothing

        Try

            nReturn = PMEReturnCode.PMTrue

            nReturn = bPMFunc.getProductOptionValue(v_sUsername:="", v_sPassword:="", v_iUserID:=0, v_iMainSourceID:=0, v_iLanguageID:=0, v_iCurrencyID:=0, v_iLogLevel:=0, v_sCallingAppName:="", v_vOptionNumber:=SIRHiddenOptions.SIROPTOverridePortfolioTransferValidations, v_vBranch:=1, r_vUnderwriting:=vValue)
            If (nReturn <> PMEReturnCode.PMTrue) Then
                Return nReturn
            End If
            If CStr(vValue) <> "1" Then

                m_oDatabase.Parameters.Clear()

                nReturn = m_oDatabase.Parameters.Add(sName:="sPolicy_ref", vValue:=sInsuranceFileRef, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)

                If (nReturn <> PMEReturnCode.PMTrue) Then
                    Return nReturn
                End If

                nReturn = m_oDatabase.SQLSelect(sSQL:="spu_sir_Is_Pending_portfolio_transfer", sSQLName:="spu_sir_Is_Pending_portfolio_transfer", bStoredProcedure:=True, vResultArray:=r_oResult)

                If nReturn <> PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "IsPendingPortfolioTransfer Failed", PMELogLevel.PMLogError)
                    Return nReturn
                End If

                m_oDatabase.Parameters.Clear()

                nReturn = m_oDatabase.Parameters.Add(sName:="sPolicyNumber", vValue:=sInsuranceFileRef, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)

                If (nReturn <> PMEReturnCode.PMTrue) Then
                    Return nReturn
                End If

                nReturn = m_oDatabase.ExecuteDataTable(sSQL:="spu_SAM_IsPending_PT", sSQLName:="spu_SAM_IsPending_PT", bStoredProcedure:=True, oRecordset:=dtResult)

                If nReturn <> PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "IsPendingPortfolioTransfer Failed", PMELogLevel.PMLogError)
                    Return nReturn
                End If

                If dtResult IsNot Nothing AndAlso dtResult.Rows.Count > 0 Then
                    r_bIsPendingPortfolioTransfer = True
                End If

                dtResult = Nothing
                m_oDatabase.Parameters.Clear()

                nReturn = m_oDatabase.Parameters.Add(sName:="sPolicyNumber", vValue:=sInsuranceFileRef, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)

                If (nReturn <> PMEReturnCode.PMTrue) Then
                    Return nReturn
                End If

                nReturn = m_oDatabase.ExecuteDataTable(sSQL:="spu_SAM_IsPending_CloneTransfer", sSQLName:="spu_SAM_IsPending_CloneTransfer", bStoredProcedure:=True, oRecordset:=dtResult)

                If nReturn <> PMEReturnCode.PMTrue Then
                    Return nReturn
                End If
                If dtResult IsNot Nothing And dtResult.Rows.Count > 0 Then
                    r_bIsPendingCloneTransfer = True
                    Return nReturn
                End If

                dtResult = Nothing
                m_oDatabase.Parameters.Clear()

                nReturn = m_oDatabase.Parameters.Add(sName:="sPolicy_number", vValue:=sInsuranceFileRef, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)

                If (nReturn <> PMEReturnCode.PMTrue) Then
                    Return PMEReturnCode.PMFalse
                End If

                nReturn = m_oDatabase.ExecuteDataTable(sSQL:="spu_Risks_Cloned_RI_Status_Sel_amend", sSQLName:="spu_Risks_Cloned_RI_Status_Sel_amend", bStoredProcedure:=True, oRecordset:=dtResult)

                If nReturn <> PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "IsPendingPortfolioTransfer Failed", PMELogLevel.PMLogError)
                    Return nReturn
                End If

                If dtResult IsNot Nothing And dtResult.Rows.Count > 0 Then
                    r_bIsPendingCloneTransfer = True
                End If

            End If
            Return nReturn
        Catch excep As Exception
            nReturn = PMEReturnCode.PMFalse
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="IsPendingCloneTransfer Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsPendingCloneTransfer", vErrNo:=Err().Number, vErrDesc:=excep.Message, excep:=excep)
        End Try
        Return nReturn

    End Function

End Class
