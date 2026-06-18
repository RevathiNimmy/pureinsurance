Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Data.SqlClient
'developer guide no.129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Form
    '
    ' Date: 06/09/2000
    '
    ' Description:
    '
    ' Edit History: SR - Created
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
    Private Const ACClass As String = "Form"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    ' Task
    Private m_iTask As Integer
    ' Process Mode
    Private m_lProcessMode As Integer
    ' Type of Business
    Private m_sTransactionType As String = ""
    ' Effective
    Private m_dtEffectiveDate As Date
    ' PRIVATE Data Members (End)
    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property
    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property Task() As Integer
        Get

            Return m_iTask

        End Get
    End Property

    Public ReadOnly Property ProcessMode() As Integer
        Get

            Return m_lProcessMode

        End Get
    End Property

    Public ReadOnly Property TransactionType() As String
        Get

            Return m_sTransactionType

        End Get
    End Property

    Public ReadOnly Property EffectiveDate() As Date
        Get

            Return m_dtEffectiveDate

        End Get
    End Property

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


            ' Initialisation Code.


            ' Check that we have the right Database for our
            ' product Family

            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If




            ' Set Username and Password

            ' Set UserID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Set the ProcessMode to Generic
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric

            ' Set the Type Of Business to New Business
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric

            ' Set the Effective Date to NOW
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
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub

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
    ' Error.
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
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
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
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ''' <summary>
    ''' GetAllCommissionArrangement
    ''' </summary>
    ''' <param name="vResultArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAllCommissionArrangement(ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            Dim lNumberofRecords As Integer

            With m_oDatabase

                'Fetch the records
                m_lReturn = .SQLSelect(ACSelectAllCommissionSQL, ACSelectAllCommissionName, ACSelectAllCommissionStored, lNumberofRecords, vResultArray)

            End With

            'Return the result

            Return m_lReturn

        Catch excep As System.Exception


            'Error
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllCommissionArrangement Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllCommissionArrangement", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function
    ' Function : CalculateCommissionRate
    '
    ' Person : S.Rajan
    ' Date   : 6th September 2000
    '----------------------------------------------------------------------
    Public Function CalculateCommissionRate(ByVal lPartyTypeId As Integer, ByVal lPartyId As Integer, ByVal lRiskTypeId As Integer, ByVal lProductId As Integer, ByVal lTransactionTypeId As Integer, ByVal lCommissionBandId As Integer, ByVal dtEffectiveDate As Date, ByRef cRate As Decimal, ByRef bIsValue As Boolean) As Integer

        Dim result As Integer = 0
        Try

            Dim lNumberofRecords As Integer

            With m_oDatabase

                'Clear all the parameters
                .Parameters.Clear()

                'Add the imput and output parameters
                m_lReturn = .Parameters.Add(sName:="Party_Type_id", vValue:=CStr(lPartyTypeId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .Parameters.Add(sName:="Party_id", vValue:=CStr(lPartyId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                m_lReturn = .Parameters.Add(sName:="Risk_Type_id", vValue:=CStr(lRiskTypeId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                m_lReturn = .Parameters.Add(sName:="Product_id", vValue:=CStr(lProductId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .Parameters.Add(sName:="Transaction_type_id", vValue:=CStr(lTransactionTypeId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .Parameters.Add(sName:="Commission_band_id", vValue:=CStr(lCommissionBandId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                'developer guide no.40
                m_lReturn = .Parameters.Add(sName:="Effective_Date", vValue:=dtEffectiveDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

                m_lReturn = .Parameters.Add(sName:="Rate", vValue:=CStr(cRate), idirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

                m_lReturn = .Parameters.Add(sName:="Is_Value", vValue:=CStr(bIsValue), idirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMBoolean)


                'Fetch the records
                m_lReturn = .SQLAction(ACCalcCommissionsQL, ACCalcCommissionName, ACCalcCommissionStored, lNumberofRecords)

                'Get the output values

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                    cRate = .Parameters.Item("Rate").Value
                    bIsValue = .Parameters.Item("Is_Value").Value

                End If

            End With

            'Return the result

            Return m_lReturn

        Catch excep As System.Exception


            'Error
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CalculateCommissionRate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CalculateCommissionRate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function
    ' Function : GetallParties
    '
    ' Person : S.Rajan
    ' Date   : 7th September 2000
    '----------------------------------------------------------------------
    Public Function GetallParties(ByRef vntResult(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .SQLSelect(ACGetAllPartiesSQL, ACGetAllPatiesName, ACGetAllPartiesStored, gPMConstants.PMAllRecords, vntResult)

            End With

            'Return the result

            Return m_lReturn

        Catch excep As System.Exception


            'Error
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllParies Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllParties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' Function : GetCommissionArrangement
    ' Purpost : To get all the commission arrangements which satisfies the given criteria
    ' Person : S.Rajan
    ' Date   : 7th September 2000
    '
    '  14th Sep 2000 : Show all parameter included. i.e., if the Id is -1 , then that parameter should not be included in the where condition
    '  20th Sep 2000 : Instead of party table, party agent type table is used.
    'Start - Renuka - (WPR64 Paralleling)
    'Added a parameter maximum_rate to SQL query
    'End - Renuka - (WPR64 Paralleling)
    '--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    Public Function GetCommissionArrangement(ByVal v_lPartyTypeid As Integer, ByVal v_lPartyId As Integer, ByVal v_lRisktypeId As Integer, ByVal v_lProductId As Integer, ByVal v_lTransactionTypeId As Integer, ByVal v_lCommissionBandId As Integer, ByVal v_lCommissionGroupId As Integer, ByVal v_lTaxGroupID As Integer, ByRef vntResult(,) As Object, Optional ByVal v_lCommissionlevelID As Integer = -1) As Integer
        Dim result As Integer = 0
        Try

            Const nSHOWALL As Integer = -1
            Dim lNumberofRecords As Integer
            Dim sSQL As String = ""

            'CMG / PB 23072002 Add join to Commission Grouping Table
            'Form the SQL String to Select the commission rating

            'Modifying the inline query to make it compatible with SQL server 2005
            Dim sSQLWhere As String = ""

            sSQL = " SELECT ISNULL(PAT.Description , 'All'), ISNULL(PT.Shortname, 'All'),"
            sSQL = sSQL & " ISNULL(P.code, 'All'), ISNULL(RT.Code, 'All'),ISNULL(TT.Code , 'All'),"
            sSQL = sSQL & " ISNULL(CB.Code,'All'), ISNULL(CG.Code,'All'), CA.Rate,"
            sSQL = sSQL & " CA.Is_value, CA.Effective_Date, CA.Party_Type,"
            sSQL = sSQL & " CA.PArty_cnt, CA.Product_id, CA.Risk_type_id,"
            sSQL = sSQL & " CA.transaction_type_id, CA.Commission_band_id, CA.Commission_grouping,"
            sSQL = sSQL & " CA.Is_Deleted, ISNULL(CA.tax_group_id,0), TG.Description,CA.maximum_rate,"
            sSQL = sSQL & " ISNULL(CL.commission_level_id,-1), ISNULL(CL.description,'<None>')"

            sSQL = sSQL & " FROM Commission_Arrangement CA"
            sSQL = sSQL & " LEFT OUTER JOIN Party_Agent_Type PAT"
            sSQL = sSQL & " ON CA.Party_Type = PAT.party_agent_type_id"
            sSQL = sSQL & " LEFT OUTER JOIN Party PT"
            sSQL = sSQL & " ON CA.Party_cnt = PT.Party_cnt"
            sSQL = sSQL & " LEFT OUTER JOIN Product P"
            sSQL = sSQL & " ON CA.Product_id = P.Product_id"
            sSQL = sSQL & " LEFT OUTER JOIN Risk_Type RT"
            sSQL = sSQL & " ON CA.Risk_Type_id = RT.Risk_type_id"
            sSQL = sSQL & " LEFT OUTER JOIN Transaction_Type TT"
            sSQL = sSQL & " ON CA.Transaction_type_id = TT.Transaction_type_id"
            sSQL = sSQL & " LEFT OUTER JOIN Commission_Band CB"
            sSQL = sSQL & " ON CA.Commission_band_id = CB.Commission_band_id"
            sSQL = sSQL & " LEFT OUTER JOIN Commission_Grouping CG"
            sSQL = sSQL & " ON CA.Commission_grouping = CG.Commission_grouping_id"
            sSQL = sSQL & " LEFT OUTER JOIN Tax_Group TG"
            sSQL = sSQL & " ON CA.tax_group_id = TG.tax_group_id"
            sSQL = sSQL & " LEFT OUTER JOIN commission_level CL"
            sSQL = sSQL & " ON CA.commission_level_id = CL.commission_level_id"

            'Processing the where clause

            sSQLWhere = ""

            If v_lPartyTypeid <> nSHOWALL And v_lPartyTypeid <> 0 Then
                sSQLWhere = sSQLWhere & " AND CA.Party_type = " & CStr(v_lPartyTypeid)
            End If

            If v_lPartyId <> nSHOWALL And v_lPartyId <> 0 Then
                sSQLWhere = sSQLWhere & " AND CA.Party_cnt  = " & CStr(v_lPartyId)
            End If

            If v_lProductId <> nSHOWALL And v_lProductId <> 0 Then
                sSQLWhere = sSQLWhere & " AND CA.Product_id = " & CStr(v_lProductId)
            End If

            If v_lRisktypeId <> nSHOWALL And v_lRisktypeId <> 0 Then
                sSQLWhere = sSQLWhere & " AND CA.Risk_type_id = " & CStr(v_lRisktypeId)
            End If

            If v_lTransactionTypeId <> nSHOWALL And v_lTransactionTypeId <> 0 Then
                sSQLWhere = sSQLWhere & " AND CA.Transaction_type_id =  " & CStr(v_lTransactionTypeId)
            End If

            If v_lCommissionBandId <> nSHOWALL And v_lCommissionBandId <> 0 Then
                sSQLWhere = sSQLWhere & " AND CA.Commission_band_id = " & CStr(v_lCommissionBandId)
            End If

            'CMG / PB 23072002 New Commission Grouping Dropdown
            If v_lCommissionGroupId <> nSHOWALL And v_lCommissionGroupId <> 0 Then
                sSQLWhere = sSQLWhere & " AND CA.Commission_grouping = " & CStr(v_lCommissionGroupId)
            End If

            If v_lTaxGroupID <> nSHOWALL And v_lTaxGroupID <> 0 Then
                sSQLWhere = sSQLWhere & " AND CA.tax_group_id = " & CStr(v_lTaxGroupID)
            End If
            If v_lCommissionlevelID <> nSHOWALL And v_lCommissionlevelID <> 0 Then
                sSQLWhere = sSQLWhere & " AND CA.commission_level_id = " & v_lCommissionlevelID
            End If

            If sSQLWhere.Length > 0 Then
                sSQL = sSQL & Strings.Replace(sSQLWhere, " AND", " WHERE", 1, 1, CompareMethod.Text)
            End If


            'Fire the SQL statement to get thr records
            m_lReturn = m_oDatabase.SQLSelect(sSQL, "GetCommissionArrangement", False, lNumberofRecords, vntResult)



            Return m_lReturn

        Catch excep As System.Exception



            'Error
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCommissionArrangement  Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCommissionArrangement ", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function
    ' Function : AddCommissionArrangement
    ' Purpost : To add a new commission arrangement record
    ' Person : S.Rajan
    ' Date   : 8th September 2000
    'Start - Renuka - (WPR64 Paralleling)
    'Added a optional parameter v_cMaximumRate
    'End - Renuka - (WPR64 Paralleling)
    '----------------------------------------------------------------------
    Public Function AddCommissionArrangement(ByVal v_lPartyTypeid As Integer, ByVal v_lPartyId As Integer, ByVal v_lRisktypeId As Integer, ByVal v_lProductId As Integer, ByVal v_lTransactionTypeId As Integer, ByVal v_lCommissionBandId As Integer, ByVal v_lCommissionGroupId As Integer, ByVal v_dtEffectiveDate As Date, ByVal v_cRate As Double, ByVal v_iIsValue As Integer, ByVal v_lTaxGroupID As Integer, Optional ByVal v_cMaximumRate As Decimal = 0, Optional ByVal v_lCommissionlevelID As Integer = 0, Optional ByVal v_sUniqueId As String = "", Optional ByVal v_sscreenHeirarchy As String = "") As Integer

        Dim result As Integer = 0
        Try

            With m_oDatabase

                'Clear all the parameters
                .Parameters.Clear()

                'Add the imput and output parameters
                m_lReturn = .Parameters.Add(sName:="Party_Type_id", vValue:=CStr(v_lPartyTypeid), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .Parameters.Add(sName:="Party_id", vValue:=CStr(v_lPartyId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                m_lReturn = .Parameters.Add(sName:="Risk_Type_id", vValue:=CStr(v_lRisktypeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                m_lReturn = .Parameters.Add(sName:="Product_id", vValue:=CStr(v_lProductId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .Parameters.Add(sName:="Transaction_type_id", vValue:=CStr(v_lTransactionTypeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .Parameters.Add(sName:="Commission_band_id", vValue:=CStr(v_lCommissionBandId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                'CMG / PB 23072002 New Commission Grouping Parameter
                m_lReturn = .Parameters.Add(sName:="Commission_grouping", vValue:=CStr(v_lCommissionGroupId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                'developer guide no.40
                m_lReturn = .Parameters.Add(sName:="Effective_date", vValue:=v_dtEffectiveDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

                m_lReturn = .Parameters.Add(sName:="commission_rate", vValue:=CStr(v_cRate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

                m_lReturn = .Parameters.Add(sName:="Is_Value", vValue:=CStr(v_iIsValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If v_lTaxGroupID = 0 Then

                    'developer guide no.85
                    m_lReturn = .Parameters.Add(sName:="tax_group_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                Else
                    m_lReturn = .Parameters.Add(sName:="tax_group_id", vValue:=CStr(v_lTaxGroupID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                End If
                'Start - Renuka - (WPR64 Paralleling)
                If v_cMaximumRate > 0 Then
                    m_lReturn = .Parameters.Add(sName:="maximum_rate", vValue:=CStr(v_cMaximumRate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
                End If
                'End - Renuka - (WPR64 Paralleling)
                'WPR 14 SAGICOR
                If v_lCommissionlevelID > 0 Then
                    m_lReturn = .Parameters.Add(sName:="commission_level_id", vValue:=v_lCommissionlevelID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                End If

                m_lReturn = .Parameters.Add(sName:="UniqueId",
                                        vValue:=v_sUniqueId,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMString)

                m_lReturn = .Parameters.Add(sName:="UserId",
                                        vValue:=m_iUserID,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMLong)


                'Call the Stored procedure to add
                m_lReturn = .SQLAction(ACAddCommissionSQL, ACAddCommissionName, ACAddCommissionStored)


            End With

            Return m_lReturn

        Catch excep As System.Exception



            'Error
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddCommissionArrangement  Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddCommissionArrangement ", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    'CMG / PB 23072002 New Commission Grouping parameter
    Public Function DeleteCommissionArrangement(ByVal v_lPartyTypeid As Integer, ByVal v_lPartyId As Integer, ByVal v_lProductId As Integer, ByVal v_lRisktypeId As Integer, ByVal v_lTransactionTypeId As Integer, ByVal v_lCommissionBandId As Integer, ByVal v_lCommissionGroupId As Integer, Optional ByVal v_dtEffectiveDate As Date = #12/30/1899#, Optional ByVal v_lCommissionLevelId As Integer = 0, Optional ByVal v_suniqueId As String = "", Optional ByVal v_sscreenHeirarchy As String = "") As Integer

        'Call the main function to Delete
        Return DeleteCommissionArrangementMain(v_lPartyTypeid, v_lPartyId, v_lProductId, v_lRisktypeId, v_lTransactionTypeId, v_lCommissionBandId, v_lCommissionGroupId, False, v_dtEffectiveDate, v_lCommissionLevelId, v_suniqueId, v_sscreenHeirarchy)

    End Function

    'CMG / PB 23072002 New Commission Grouping parameter
    Public Function UnDeleteCommissionArrangement(ByVal v_lPartyTypeid As Integer, ByVal v_lPartyId As Integer, ByVal v_lProductId As Integer, ByVal v_lRisktypeId As Integer, ByVal v_lTransactionTypeId As Integer, ByVal v_lCommissionBandId As Integer, ByVal v_lCommissionGroupId As Integer, Optional ByVal v_dtEffectiveDate As Date = #12/30/1899#, Optional ByVal v_lCommissionLevelId As Integer = 0, Optional ByVal v_suniqueId As String = "", Optional ByVal v_sscreenHeirarchy As String = "") As Integer

        'Call the main function to Delete
        Return DeleteCommissionArrangementMain(v_lPartyTypeid, v_lPartyId, v_lProductId, v_lRisktypeId, v_lTransactionTypeId, v_lCommissionBandId, v_lCommissionGroupId, True, v_dtEffectiveDate, v_lCommissionLevelId, v_suniqueId, v_sscreenHeirarchy)

    End Function
    ' Function : DeleteCommissionArrangement
    ' Purpost : To Delete the commission arrangement from the database
    ' Person : S.Rajan
    ' Date   : 11th September 2000
    '----------------------------------------------------------------------
    Private Function DeleteCommissionArrangementMain(ByVal v_lPartyTypeid As Integer, ByVal v_lPartyId As Integer, ByVal v_lProductId As Integer, ByVal v_lRisktypeId As Integer, ByVal v_lTransactionTypeId As Integer, ByVal v_lCommissionBandId As Integer, ByVal v_lCommissionGroupId As Integer, ByVal v_bUnDelete As Boolean, Optional ByVal v_dtEffectiveDate As Date = #12/30/1899#, Optional ByVal v_lCommissionLevelId As Integer = 0, Optional ByVal v_sUniqueId As String = "", Optional ByVal v_sscreenHeirarchy As String = "") As Integer

        Dim result As Integer = 0


        With m_oDatabase

            'Clear all the parameters
            .Parameters.Clear()

            'Add the imput and output parameters
            m_lReturn = .Parameters.Add(sName:="Party_Type_id", vValue:=CStr(v_lPartyTypeid), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = .Parameters.Add(sName:="Party_id", vValue:=CStr(v_lPartyId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


            m_lReturn = .Parameters.Add(sName:="Risk_Type_id", vValue:=CStr(v_lRisktypeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


            m_lReturn = .Parameters.Add(sName:="Product_id", vValue:=CStr(v_lProductId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = .Parameters.Add(sName:="Transaction_type_id", vValue:=CStr(v_lTransactionTypeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = .Parameters.Add(sName:="Commission_band_id", vValue:=CStr(v_lCommissionBandId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'CMG / PB 23072002 New Commission Grouping parameter
            m_lReturn = .Parameters.Add(sName:="Commission_group_id", vValue:=CStr(v_lCommissionGroupId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            ' Alix - 21/01/2003 - PN9808
            ' Effective date is part of making a record unique
            'developer guide no.40
            m_lReturn = .Parameters.Add(sName:="Effective_Date", vValue:=v_dtEffectiveDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            ' /Alix


            m_lReturn = .Parameters.Add(sName:="commission_level_id", vValue:=v_lCommissionLevelId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = .Parameters.Add(sName:="UniqueId",
                                        vValue:=v_sUniqueId,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = .Parameters.Add(sName:="UserId",
                                        vValue:=m_iUserID,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMLong)


            m_lReturn = .Parameters.Add(sName:="ScreenHierarchy",
                                        vValue:=v_sscreenHeirarchy,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMString)

            If v_bUnDelete Then

                'Call the Stored procedure to undelete the details
                m_lReturn = .SQLAction(ACUnDeleteCommissionSQL, ACUnDeleteCommissionName, ACUnDeleteCommissionStored)


            Else

                'Call the Stored procedure to delete the details
                m_lReturn = .SQLAction(ACDeleteCommissionSQL, ACDeleteCommissionName, ACDeleteCommissionStored)

            End If

        End With

        'Return the result


        Return m_lReturn

    End Function

    '----------------------------------------------------------------------
    ' Function : EditCommissionArrangement
    ' Purpost : To Modify the commission arrangement from the database
    ' Person : S.Rajan
    ' Date   : 11th September 2000
    '
    ' Modified : 'CMG / PB 23072002 New Commission Grouping argument
    'Start - Renuka - (WPR64 Paralleling)
    'Added a optional parameter v_cMaximumRate
    'End - Renuka - (WPR64 Paralleling)
    '----------------------------------------------------------------------
    Public Function EditCommissionArrangement(ByVal v_lPartyTypeid As Integer, ByVal v_lPartyId As Integer, ByVal v_lRisktypeId As Integer, ByVal v_lProductId As Integer, ByVal v_lTransactionTypeId As Integer, ByVal v_lCommissionBandId As Integer, ByVal v_lCommissionGroupId As Integer, ByVal v_cRate As Double, ByVal v_iIsValue As Integer, ByVal v_dtEffectiveDate As Date, ByVal v_lTaxGroupID As Integer, Optional ByRef v_dtOldDate As Date = #12/30/1899#, Optional ByVal v_cMaximumRate As Decimal = 0, Optional ByVal v_lCommissionlevelID As Integer = 0, Optional ByVal v_sUniqueId As String = "") As Integer


        Dim result As Integer = 0
        Try

            With m_oDatabase

                'Clear all the parameters
                .Parameters.Clear()

                'Add the imput and output parameters
                m_lReturn = .Parameters.Add(sName:="Party_Type_id", vValue:=CStr(v_lPartyTypeid), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .Parameters.Add(sName:="Party_id", vValue:=CStr(v_lPartyId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                m_lReturn = .Parameters.Add(sName:="Risk_Type_id", vValue:=CStr(v_lRisktypeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                m_lReturn = .Parameters.Add(sName:="Product_id", vValue:=CStr(v_lProductId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .Parameters.Add(sName:="Transaction_type_id", vValue:=CStr(v_lTransactionTypeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .Parameters.Add(sName:="Commission_band_id", vValue:=CStr(v_lCommissionBandId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                'CMG / PB 23072002 New Commission Grouping parameter
                m_lReturn = .Parameters.Add(sName:="commission_grouping", vValue:=CStr(v_lCommissionGroupId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .Parameters.Add(sName:="Rate", vValue:=CStr(v_cRate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

                m_lReturn = .Parameters.Add(sName:="Is_Value", vValue:=CStr(v_iIsValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                'developer guide no.40
                m_lReturn = .Parameters.Add(sName:="Effective_date", vValue:=v_dtEffectiveDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
                'developer guide no.40
                m_lReturn = .Parameters.Add(sName:="Old_date", vValue:=v_dtOldDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

                If v_lTaxGroupID = 0 Then

                    'developer guide no.85
                    m_lReturn = .Parameters.Add(sName:="tax_group_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                Else
                    m_lReturn = .Parameters.Add(sName:="tax_group_id", vValue:=CStr(v_lTaxGroupID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                End If

                'Start - Renuka - (WPR64 Paralleling)
                If v_cMaximumRate > 0 Then
                    m_lReturn = .Parameters.Add(sName:="maximum_rate", vValue:=CStr(v_cMaximumRate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
                End If
                'End - Renuka - (WPR64 Paralleling)

                'WPR 14 SAGICOR
                m_lReturn = .Parameters.Add(sName:="commission_level_id", vValue:=v_lCommissionlevelID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .Parameters.Add(sName:="UniqueId",
                                        vValue:=v_sUniqueId,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMString)

                m_lReturn = .Parameters.Add(sName:="UserId",
                                        vValue:=m_iUserID,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMLong)

                'Call the Stored procedure to delete the details
                m_lReturn = .SQLAction(ACEditCommissionSQL, ACEditCommissionName, ACEditCommissionStored)


            End With

            'Return the result

            Return m_lReturn

        Catch excep As System.Exception



            'Error
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditCommissionArrangement  Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditCommissionArrangement ", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Public Function GetConfiguredCommissionLevel(ByRef r_vCommissionLevel(,) As Object) As Integer

        Try
            With m_oDatabase

                'Clear all the parameters
                .Parameters.Clear()

                'Call the Stored procedure
                m_lReturn = .SQLSelect(sSQL:=ACSelectCommissionLevelSQL, sSQLName:=ACSelectCommissionLevelName, bStoredProcedure:=ACSelectCommissionLevelStored, vResultArray:=r_vCommissionLevel)


            End With


            GetConfiguredCommissionLevel = m_lReturn

        Catch EXCE As Exception

            GetConfiguredCommissionLevel = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetConfiguredCommissionLevel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=GetConfiguredCommissionLevel, vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=exce)

        End Try
    End Function
    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class

