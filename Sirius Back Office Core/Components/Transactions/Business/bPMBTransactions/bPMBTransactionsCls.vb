Option Strict Off
Option Explicit On
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Automated_NET.Automated")>
Public NotInheritable Class Automated
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Automated
    '
    ' Date: 09/11/1998
    '
    ' Description: Creatable Automated class which contains all the
    '              Automated methods which can be called by Navigator.
    '
    ' Edit History: TF091198 - Created
    '               TF270799 - Avoid Posting if Premium = 0.
    '               ECK201200 - Premium in TF270799 needs to include all extras
    '               SJP04072002 Account Key now party count to allow for more than 8 branches
    ' ***************************************************************** '

    ' ************************************************
    ' Added to replace global variables 10/12/2003
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************
    Private m_sIsUnderwritingOrAgency As String = ""

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Automated"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Component Services object

    ' Authority level
    Private m_lPMAuthorityLevel As Integer

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lReturn As Integer

    ' Calling Application Name

    ' Exit Status
    Private m_lStatus As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Primary Keys to work with
    Private m_lInsuranceFileCnt As Integer
    Private m_sDebitCredit As String = ""
    'DC 20/09/00
    Private m_dtPolicyStartDate As Date
    'EK 220200

    Private m_vLastTransType As Object
    'eck030500
    Private m_dtLastTransDate As Date
    'eck0060601
    Private m_sReason As String = ""

    'DM24/08/06 30299
    Private m_dtCoverStartDate As Date
    'eck080800
    Private m_bManualAJ As Boolean
    Private m_cManualAJValue As Decimal

    Private m_sDocumentRef As String = ""
    Private m_vPolicyPremium As Double 'TF270799
    Private m_lTransactionExportFolderCnt As Integer 'EK 13/10/99
    'eck020600
    Private m_iPolicySourceID As Integer
    'eck201200

    Private m_vPostingAmount As Object
    'EK 220200
    Private m_lDocumentNo As Integer
    'Start (Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
    Private m_sReference As String = ""
    Private m_sGroupCode As String = ""
    Private m_sRangeCode As String = ""
    'End(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
    'EK 200300 Moved from ExportFolder and made public
    Private m_lPartyID As Integer
    Private m_lInsuranceHolderCnt As Integer
    Private m_lAccountHandlerCnt As Integer
    Private m_lAgentCnt As Integer
    Private m_lInsuranceHolderAccountKey As Integer
    Private m_lAccountHandlerAccountKey As Integer
    Private m_lAgentAccountKey As Integer
    'Posting Values
    Private m_cAgentAmount As Decimal
    Private m_cAgentAmountCalc As Decimal
    Private m_cFeesAmount As Decimal
    Private m_cExtrasAmount As Decimal
    Private m_cDiscountAmount As Decimal
    Private m_cExtrasCommission As Decimal
    Private m_cExtrasIPT As Decimal
    Private m_cCoinsurersAmount As Decimal
    Private m_cCoinsurersCommission As Decimal
    Private m_cCoinsurersIPT As Decimal
    Private m_cCoinsurersFee As Decimal
    Private m_cInsurersAmount As Decimal
    Private m_cInsurersCommission As Decimal
    Private m_cInsurersIPT As Decimal
    Private m_cInsurersFee As Decimal
    'EK 29/11/99
    Private m_cSubAgentCommission As Decimal
    Private m_vPaidDirect As Object
    'DC151204
    Private m_cIntroducerAmount As Decimal
    Private m_cIntroducerAmountCalc As Decimal

    'DC260105 : Introducer Transaction Processing
    Private m_lIntroducerCnt As Integer
    Private m_vIntroducers(,) As Object

    'eck291001
    Private m_sOriginalDoc As String = ""
    'sj 05/07/2002 - start
    Private m_bAsynchronousPosting As Boolean
    'sj 05/07/2002 - end

    'sj 15/08/2002 - start
    Private m_lRealInsuranceFileCnt As Integer
    'sj 15/08/2002 - end
    'S4BDAT004
    Private m_lTermsOfPaymentId As Integer

    'PN37017 Update debit entry when premium is zero
    Private m_bIsContra As Boolean

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public WriteOnly Property IsContra() As Boolean
        Set(ByVal Value As Boolean)

            m_bIsContra = Value

        End Set
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property

    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = Value
        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the exit status.
            Return m_lStatus

        End Get
    End Property
    Public ReadOnly Property PaidDirect() As Object
        Get
            Return m_vPaidDirect

        End Get
    End Property
    Public WriteOnly Property DebitCredit() As String
        Set(ByVal Value As String)

            m_sDebitCredit = Value

        End Set
    End Property
    'eck060601
    Public WriteOnly Property Reason() As String
        Set(ByVal Value As String)

            m_sReason = Value

        End Set
    End Property

    'DC 20/09/00
    Public WriteOnly Property PolicyStartDate() As Date
        Set(ByVal Value As Date)

            m_dtPolicyStartDate = Value

        End Set
    End Property
    'Starts
    'Public WriteOnly Property LastTransType() As Byte
    '	Set(ByVal Value As Byte)

    '		m_vLastTransType = Value

    '	End Set
    'End Property
    Public WriteOnly Property LastTransType() As Object
        Set(ByVal Value As Object)

            m_vLastTransType = Value

        End Set
    End Property
    'Ends
    Public WriteOnly Property LastTransDate() As Date
        Set(ByVal Value As Date)

            m_dtLastTransDate = Value

        End Set
    End Property
    Public Property InsuranceFileCnt() As Integer
        Get

            Return m_lInsuranceFileCnt

        End Get
        Set(ByVal Value As Integer)

            m_lInsuranceFileCnt = Value

        End Set
    End Property
    'eck080800
    Public Property ManualAJ() As Boolean
        Get

            Return m_bManualAJ
        End Get
        Set(ByVal Value As Boolean)

            m_bManualAJ = Value

        End Set
    End Property
    Public Property ManualAJValue() As Decimal
        Get

            Return m_cManualAJValue
        End Get
        Set(ByVal Value As Decimal)

            m_cManualAJValue = Value

        End Set
    End Property

    Public Property DocumentRef() As String
        Get
            Return m_sDocumentRef
        End Get
        Set(ByVal Value As String)
            m_sDocumentRef = Value
        End Set
    End Property

    ' TF270799
    Public ReadOnly Property PolicyPremium() As Double
        Get

            Return m_vPolicyPremium

        End Get
    End Property
    'eck201200

    Public Property PostingAmount() As Object
        Get

            Return m_vPostingAmount
        End Get
        Set(ByVal Value As Object)

            m_vPostingAmount = Value

        End Set
    End Property
    'Ends
    'sj 15/08/2002 - start
    Public WriteOnly Property RealInsuranceFileCnt() As Integer
        Set(ByVal Value As Integer)
            m_lRealInsuranceFileCnt = Value
        End Set
    End Property
    'sj 15/08/2002 - end
    'S4BDAT004
    Public Property TermsOfPaymentId() As Integer
        Get

            Return m_lTermsOfPaymentId
        End Get
        Set(ByVal Value As Integer)

            m_lTermsOfPaymentId = Value

        End Set
    End Property
    'PN30299
    Public WriteOnly Property CoverStartDate() As Date
        Set(ByVal Value As Date)

            m_dtCoverStartDate = Value

        End Set
    End Property

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
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long

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

            Dim sValue As String = ""

            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password

            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            'sj 05/07/2002 - start
            m_lReturn = bPMFunc.getProductOptionValue(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTAsynchronousPosting, v_vBranch:=m_iSourceID, r_vUnderwriting:=sValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed for Multi Branch Accounting", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If sValue = "1" Then
                m_bAsynchronousPosting = True
            End If
            'sj 05/07/2002 - end

            m_lReturn = bPMFunc.getUnderwritingOrAgency(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, r_vUnderwriting:=m_sIsUnderwritingOrAgency)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.RaiseError("getUnderwritingOrAgency", "Failed", gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            If Not Informations.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If

            If Not Informations.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If

            If Not Informations.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If

            If Not Informations.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If

            If Not Informations.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetKeys (Standard Method)
    '
    ' Description: Stores all of the parameter members with the key
    '              array.
    '
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check we have a vaild array.
            If Not Informations.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Step through the key array.
            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)
                ' Assign the parameter member with the
                ' correct key array item.

                Select Case CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                    Case PMNavKeyConst.PMKeyNameInsFileCnt

                        m_lInsuranceFileCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                End Select

            Next lRow

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetKeys (Standard Method)
    '
    ' Description: Stores all of the key array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            ' Initialise the key array with the number of
            ' keys needed to be returned.
            ' Note: Remember arrays are zero based.
            '    ReDim vKeyArray(1, 0)

            ' Assign the key array with the parameter members.
            '    vKeyArray(PMKeyName, 0) = PMKeyNameInsFileCnt
            '    vKeyArray(PMKeyValue, 0) = m_lInsuranceFileCnt&

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Start (Public)
    '
    ' Description: Performs the Automated Action dependant on the Task
    '              Process Mode etc.
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Perform the Transactions Process
            m_lReturn = ProcessTransactions()

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> ACTConst.ACInsurerStopped) Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="ProcessTransactions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
                Return m_lReturn
            ElseIf (m_lReturn = ACTConst.ACInsurerStopped) Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetSummary (Standard Method)
    '
    ' Description: Stores all of the summary array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function GetSummary(ByRef vSummaryArray As Object) As Integer

        Dim result As Integer = 0

        Try

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'eck290801
    ' ***************************************************************** '
    ' Name: GetAccountKey (Standard Method)
    '
    ' Description: Gets AccountKey
    '
    ' ***************************************************************** '
    Public Function GetAccountKey(ByVal v_iSourceID As Integer, ByVal v_lPartyId As Integer, ByRef v_lAccountKey As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = gPMComponentServices.calccombinedkey(v_lSourceID:=v_iSourceID, v_lKeyID:=v_lPartyId, r_lCombinedKeyID:=v_lAccountKey)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountKey Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountKey", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'DC260105 : Introducer Transaction Processing

    ' ***************************************************************** '
    ' Name: GetAllIntroducers (Standard Method)
    '
    ' Description: Get All Introducers For Policy
    '
    ' ***************************************************************** '
    Public Function GetAllIntroducers() As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim oDatabase As New dPMDAO.Database

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, r_bNewInstanceCreated:=True, r_oCheckedDatabase:=oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sSQL = "select epa.agent_cnt " & Strings.ChrW(13) & Strings.ChrW(10) &
                   "from event_insurance_file eif " & Strings.ChrW(13) & Strings.ChrW(10) &
                   "join event_policy_agents epa " & Strings.ChrW(13) & Strings.ChrW(10) &
                   "on eif.insurance_file_cnt = epa.insurance_file_cnt " & Strings.ChrW(13) & Strings.ChrW(10) &
                   "join party p on p.party_cnt = epa.agent_cnt " & Strings.ChrW(13) & Strings.ChrW(10) &
                   "join account a on a.account_key = p.party_cnt " & Strings.ChrW(13) & Strings.ChrW(10) &
                   "join ledger l on a.ledger_id = l.ledger_id " & Strings.ChrW(13) & Strings.ChrW(10) &
                   "where eif.insurance_file_cnt = {insurance_file_cnt}" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "and l.ledger_short_name = 'TR'"

            With oDatabase

                m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(m_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="GetAllIntroducers", bStoredProcedure:=gPMConstants.PMEReturnCode.PMFalse, vResultArray:=m_vIntroducers)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    Throw New Exception()

                End If

            End With

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllIntroducers Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllIntroducers", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'eck temporary debug code
    ' ***************************************************************** '
    ' Name: AmICommited (Standard Method)
    '
    ' Description: Gets AccountKey
    '
    ' ***************************************************************** '
    Public Function AmICommited(ByVal sDocumentRef As String) As Integer

        Dim result As Integer = 0
        Dim oDatabase As New dPMDAO.Database

        Dim sText, sSQL As String
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, r_bNewInstanceCreated:=True, r_oCheckedDatabase:=oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sSQL = "select transaction_export_folder_cnt from transaction_export_folder where document_ref = {document_ref}"
            With oDatabase
                m_lReturn = .Parameters.Add(sName:="document_ref", vValue:=sDocumentRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="AmICommited", bStoredProcedure:=gPMConstants.PMEReturnCode.PMFalse, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                Else
                    If Not Informations.IsArray(vResultArray) Then
                        result = gPMConstants.PMEReturnCode.PMNotFound

                        sText = " ALERT - Not commited Transaction Export Folder  " & sDocumentRef

                        sSQL = "INSERT into sirius_architecture..pmmessage(username,log_date,message_type,text) VALUES ('sirius',getdate(),2,'" & sText & "')"

                        m_lReturn = .SQLAction(sSQL:=sSQL, sSQLName:="PMErrorMessage", bStoredProcedure:=False)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                    End If
                End If
            End With

            ' Destroy Component Services object

            m_lReturn = oDatabase.CloseDatabase()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Release reference to PM Data Access Object
            oDatabase = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AmICommited Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AmICommited", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)
    ' ***************************************************************** '
    ' Name: ProcessTransactions (Private)
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function ProcessTransactions() As Integer

        Dim result As Integer = 0
        Dim lTransactionExportFolderCnt As Integer
        Dim sOptionValue As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Create the Transactions Export tables
        m_lReturn = CreateExport(r_lTransactionExportFolderCnt:=lTransactionExportFolderCnt)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="CreateExport Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessTransactions")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' TF270799 - End if Policy Premium = 0
        'eck201200 check agianst total including extras
        '    If (m_vPolicyPremium = 0) Then
        'DC180401 check also that PolicyPremium Is Zero Before Exitting
        'If (m_vPostingAmount = 0) Then
        If (m_vPostingAmount = 0 And m_vPolicyPremium = 0) And Not m_bIsContra Then
            Return result
        End If
        m_lTransactionExportFolderCnt = lTransactionExportFolderCnt

        'sj 11/07/2002 - start
        'Multi Branch Accounting
        'If multi branch accounting is turned on then write out a record in the
        'accounts_transaction_queue table and exit
        If m_bAsynchronousPosting Then
            m_lReturn = AccountsTransactionQueueAdd(r_lTransactionExportFolderCnt:=lTransactionExportFolderCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="AccountsTransactionQueueAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessTransactions")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        Else
            'sj 11/07/2002 - end
            m_lReturn = SendToOrion(v_lTransactionFolderCnt:=lTransactionExportFolderCnt)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> ACTConst.ACInsurerStopped) Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="SendToOrion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessTransactions")

                Return m_lReturn
            ElseIf (m_lReturn = ACTConst.ACInsurerStopped) Then  'mkw 101203 pn9003
                Return m_lReturn
            End If
        End If

        Return result

    End Function

    'EK 200300
    ' ***************************************************************** '
    ' Name: ProcessAJTransactions (Private)
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function ProcessAJTransactions() As Integer

        Dim result As Integer = 0
        Dim lTransactionExportFolderCnt As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_sOriginalDoc = DocumentRef
            ' Create the Transactions Export tables
            m_lReturn = CreateAJExport(r_lTransactionExportFolderCnt:=lTransactionExportFolderCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_vPostingAmount = 0 And m_vPolicyPremium = 0 And Not m_bIsContra Then
                Return result
            End If

            m_lTransactionExportFolderCnt = lTransactionExportFolderCnt
            m_lReturn = SendToOrion(v_lTransactionFolderCnt:=lTransactionExportFolderCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessAJTransactions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAJTransactions", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'DC260105 : Introducer Transaction Processing

    ' ***************************************************************** '
    ' Name: ProcessIntroducerTrans (Private)
    '
    ' Description:
    '
    ' ***************************************************************** '

    Public Function ProcessIntroducerTrans() As Integer

        Dim result As Integer = 0
        Dim lTransactionExportFolderCnt As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_sOriginalDoc = DocumentRef

            m_lReturn = GetAllIntroducers()

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                If Informations.IsArray(m_vIntroducers) Then

                    For lCount As Integer = m_vIntroducers.GetLowerBound(1) To m_vIntroducers.GetUpperBound(1)

                        m_lIntroducerCnt = CInt(m_vIntroducers(0, lCount))

                        ' Create the Transactions Export tables
                        m_lReturn = CreateIntroducerExport(r_lTransactionExportFolderCnt:=lTransactionExportFolderCnt)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        m_lTransactionExportFolderCnt = lTransactionExportFolderCnt

                        m_lReturn = SendToOrion(v_lTransactionFolderCnt:=lTransactionExportFolderCnt)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                    Next lCount

                End If
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessIntroducerTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessIntroducerTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'DC060801 -start -process its4me transactions
    'DC240402 -enhanced
    ' ***************************************************************** '
    ' Name: ProcessIts4MeTransactions (Public)
    '
    ' Description:
    '
    ' ***************************************************************** '

    Public Function ProcessIts4MeTransactions() As Integer

        Dim result As Integer = 0
        Dim vAccountArray(,) As Object = Nothing
        Dim vResultArray(,) As Object = Nothing
        Dim dDepositPC As Double
        Dim cDepositAmount, cPostingAmount As Decimal
        Dim lDepositAccountId As Integer
        Dim cPFAmount As Decimal
        Dim lAccountId As Integer
        Dim sAccountShortCode As String = ""
        Dim sContactName As New StringsHelper.FixedLengthString(60)
        Dim sAddress1 As New StringsHelper.FixedLengthString(40)
        Dim sAddress2 As New StringsHelper.FixedLengthString(40)
        Dim sAddress3 As New StringsHelper.FixedLengthString(40)
        Dim sAddress4 As New StringsHelper.FixedLengthString(40)
        Dim sPostalCode As New StringsHelper.FixedLengthString(20)
        Dim sPaymentName As New StringsHelper.FixedLengthString(60)
        Dim sPaymentAccountCode As New StringsHelper.FixedLengthString(60)
        Dim sPaymentBranchCode As New StringsHelper.FixedLengthString(30)
        Dim sPaymentReference1 As New StringsHelper.FixedLengthString(30)
        Dim sPaymentReference2 As New StringsHelper.FixedLengthString(30)
        Dim sSQL As String = ""
        Dim lPFAccountId As Integer
        Dim oPremiumFinance As bACTPremiumFinance.Business
        Dim vTransactIdArray(,) As Object = Nothing
        Dim vClientAccountIdArray(,) As Object = Nothing
        Dim vDepositAccountIdArray(,) As Object = Nothing

        Dim lDepositTransId As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the OrionLink business object

            '   GET DEPOSIT ACCOUNT ID
            '----------------------------------

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                m_lReturn = .SQLSelect(sSQL:=ACGetIts4MeDepositAccountIdSQL, sSQLName:=ACGetIts4MeDepositAccountIdName, bStoredProcedure:=ACGetIts4MeDepositAccountIdStored, vResultArray:=vDepositAccountIdArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the Deposit Account Id ", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessIts4MeTransactions", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result

                End If

            End With

            If Not Informations.IsArray(vDepositAccountIdArray) Then
                result = gPMConstants.PMEReturnCode.PMFalse
            Else

                lDepositAccountId = CInt(vDepositAccountIdArray(0, 0))
            End If

            '   GET PREMIUM FINANCE ACCOUNT ID
            '---------------------------------

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Get deposit percentage
                m_lReturn = .SQLSelect(sSQL:=ACGetIts4MePFAccountIdSQL, sSQLName:=ACGetIts4MePFAccountIdName, bStoredProcedure:=ACGetIts4MePFAccountIdStored, vResultArray:=vAccountArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get PROMPT Account Id For Premium Finance ", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessIts4MeTransactions", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result

                End If

            End With

            If Not Informations.IsArray(vAccountArray) Then
                result = gPMConstants.PMEReturnCode.PMFalse
            Else

                lPFAccountId = CInt(vAccountArray(0, 0))
            End If

            '   GET CLIENT ACCOUNT TRANS DETAIL ID
            '----------------------------------------

            sSQL = "SELECT t.account_id " &
                   "FROM orion_for_broking..transdetail t, " &
                   "orion_for_broking..document d, " &
                   "orion_for_broking..account a, " &
                   "orion_for_broking..Ledger l " &
                   "WHERE d.document_ref = '" & m_sDocumentRef & "' " &
                   "AND t.document_id = d.document_id " &
                   "AND a.account_id = t.account_id " &
                   "AND a.ledger_id = l.ledger_id " &
                   "AND l.ledger_name = 'Client'"

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Get deposit percentage
                m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="GetAccountIdFromAccountKey", bStoredProcedure:=False, vResultArray:=vClientAccountIdArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Debit Transaction Id For Premium Finance", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessIts4MeTransactions", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result

                End If

            End With

            If Not Informations.IsArray(vClientAccountIdArray) Then
                result = gPMConstants.PMEReturnCode.PMFalse
            Else

                lAccountId = CInt(vClientAccountIdArray(0, 0))
            End If

            '   GET DEBIT TRANSACTION TRANS DETAIL ID
            '----------------------------------------

            sSQL = "SELECT t.transdetail_id " &
                   "FROM orion_for_broking..transdetail t, " &
                   "orion_for_broking..document d " &
                   "WHERE t.document_id = d.document_id " &
                   "AND d.document_ref = '" & m_sDocumentRef & "' " &
                   "AND t.document_sequence = 1 "

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Get deposit percentage
                m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="GetAccountIdFromAccountKey", bStoredProcedure:=False, vResultArray:=vTransactIdArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Debit Transaction Id For Premium Finance", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessIts4MeTransactions", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result

                End If

            End With

            '   GET DEPOSIT PERCENTAGE
            '-------------------------

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Get deposit percentage
                m_lReturn = .SQLSelect(sSQL:=ACGetIts4MePFDepositPCSQL, sSQLName:=ACGetIts4MePFDepositPCName, bStoredProcedure:=ACGetIts4MePFDepositPCStored, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            cPostingAmount = m_vPostingAmount

            dDepositPC = CDbl(vResultArray(0, 0))
            cDepositAmount = m_vPostingAmount * (dDepositPC / 100)
            cPFAmount = m_vPostingAmount - cDepositAmount

            '   IF THERE IS A DEPOSIT - PROCESS DEPOSIT TRANSACTION
            '------------------------------------------------------

            If Informations.IsArray(vResultArray) Then

                '   PROCESS DEPOSIT TRANSACTION
                '------------------------------------------

                If Not Informations.IsArray(vAccountArray) Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                Else

                    '       POST PREMIUM FINANCE TRANSACTION
                    '---------------------------------------


                    oPremiumFinance = New bACTPremiumFinance.Business
                    m_lReturn = oPremiumFinance.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMError

                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise the Premium Finance", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessIts4MeTransactions", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                        Return result
                    End If

                    '       SET TRANSACT PLAN PARAMETERS
                    '-----------------------------------

                    m_lReturn = oPremiumFinance.TransactIts4meDeposit(v_lClientAccount:=lAccountId, v_lPremFinanceAccount:=lDepositAccountId, v_cFinanceAmount:=cPFAmount, v_vTransactionIDs:=vTransactIdArray, v_iCompanyID:=m_iSourceID, v_iDaysDelay:=0, v_iInstallments:=1, v_cDeposit:=cDepositAmount, v_lDepositTransId:=lDepositTransId)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        result = gPMConstants.PMEReturnCode.PMError

                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create Its4me Deposit", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessIts4MeTransactions", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                        Return result

                    End If

                End If

            End If

            '   PROCESS PREMIUM FINANCE TRANSACTION
            '--------------------------------------

            If Not Informations.IsArray(vTransactIdArray) Then

                result = gPMConstants.PMEReturnCode.PMFalse

            Else

                '       PREMIUM FINANCE TRANSACTION
                '---------------------------------------


                oPremiumFinance = New bACTPremiumFinance.Business
                m_lReturn = oPremiumFinance.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMError

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise the Premium Finance", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessIts4MeTransactions", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result
                End If

                '       SET TRANSACT PLAN PARAMETERS
                '-----------------------------------

                m_lReturn = oPremiumFinance.TransactPremiumFinance(v_lClientAccount:=lAccountId, v_lPremFinanceAccount:=lPFAccountId, v_cFinanceAmount:=cPFAmount, v_vTransactionIDs:=vTransactIdArray, v_iCompanyID:=m_iSourceID, v_iDaysDelay:=0, v_iInstallments:=1, v_cDeposit:=cDepositAmount, v_lDepositTransId:=lDepositTransId)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    result = gPMConstants.PMEReturnCode.PMError

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise Transact Premium Finance", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessIts4MeTransactions", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result

                End If

            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessIts4MeTransactions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessIts4MeTransactions", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'DC060801 -end
    'DC250402 -start -process its4me transactions -debit/credit card
    ' ***************************************************************** '
    ' Name: ProcessIts4MeTransDC (Public)
    '
    ' Description:
    '
    ' ***************************************************************** '

    Public Function ProcessIts4MeTransDC() As Integer

        Dim result As Integer = 0
        Dim lDepositAccountId As Integer
        Dim cPFAmount As Decimal
        Dim lAccountId As Integer
        Dim sSQL As String = ""
        Dim oPremiumFinance As bACTPremiumFinance.Business
        Dim vTransactIdArray(,) As Object = Nothing
        Dim vClientAccountIdArray(,) As Object = Nothing
        Dim vDepositAccountIdArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the OrionLink business object

            '   GET DEPOSIT ACCOUNT ID
            '----------------------------------

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                m_lReturn = .SQLSelect(sSQL:=ACGetIts4MeDepositAccountIdSQL, sSQLName:=ACGetIts4MeDepositAccountIdName, bStoredProcedure:=ACGetIts4MeDepositAccountIdStored, vResultArray:=vDepositAccountIdArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the Deposit Account Id ", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessIts4MeTransactions", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result

                End If

            End With

            If Not Informations.IsArray(vDepositAccountIdArray) Then
                result = gPMConstants.PMEReturnCode.PMFalse
            Else

                lDepositAccountId = CInt(vDepositAccountIdArray(0, 0))
            End If

            '   GET CLIENT ACCOUNT TRANS DETAIL ID
            '----------------------------------------

            sSQL = "SELECT t.account_id " &
                   "FROM orion_for_broking..transdetail t, " &
                   "orion_for_broking..document d, " &
                   "orion_for_broking..account a, " &
                   "orion_for_broking..Ledger l " &
                   "WHERE d.document_ref = '" & m_sDocumentRef & "' " &
                   "AND t.document_id = d.document_id " &
                   "AND a.account_id = t.account_id " &
                   "AND a.ledger_id = l.ledger_id " &
                   "AND l.ledger_name = 'Client'"

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Get deposit percentage
                m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="GetAccountIdFromAccountKey", bStoredProcedure:=False, vResultArray:=vClientAccountIdArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Debit Transaction Id For Premium Finance", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessIts4MeTransDC", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result

                End If

            End With

            If Not Informations.IsArray(vClientAccountIdArray) Then
                result = gPMConstants.PMEReturnCode.PMFalse
            Else

                lAccountId = CInt(vClientAccountIdArray(0, 0))
            End If

            '   GET DEBIT TRANSACTION TRANS DETAIL ID
            '----------------------------------------

            sSQL = "SELECT t.transdetail_id " &
                   "FROM orion_for_broking..transdetail t, " &
                   "orion_for_broking..document d " &
                   "WHERE t.document_id = d.document_id " &
                   "AND d.document_ref = '" & m_sDocumentRef & "' " &
                   "AND t.document_sequence = 1 "

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Get deposit percentage
                m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="GetAccountIdFromAccountKey", bStoredProcedure:=False, vResultArray:=vTransactIdArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Debit Transaction Id For Premium Finance", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessIts4MeTransDC", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result

                End If

            End With

            cPFAmount = m_vPostingAmount

            If Not Informations.IsArray(vTransactIdArray) Then

                result = gPMConstants.PMEReturnCode.PMFalse

            Else

                '       PREMIUM FINANCE TRANSACTION
                '---------------------------------------


                oPremiumFinance = New bACTPremiumFinance.Business
                m_lReturn = oPremiumFinance.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMError

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise the Premium Finance", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessIts4MeTransDC", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result
                End If

                '       SET TRANSACT PLAN PARAMETERS
                '-----------------------------------

                m_lReturn = oPremiumFinance.TransactPremiumFinance(v_lClientAccount:=lAccountId, v_lPremFinanceAccount:=lDepositAccountId, v_cFinanceAmount:=cPFAmount, v_vTransactionIDs:=vTransactIdArray, v_iCompanyID:=m_iSourceID, v_iDaysDelay:=0, v_iInstallments:=1, v_cDeposit:=0, v_lDepositTransId:=0)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    result = gPMConstants.PMEReturnCode.PMError

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise Transact Premium Finance", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessIts4MeTransDC", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result

                End If

            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessIts4MeTransDC Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessIts4MeTransDC", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'DC250402 -end

    ' ***************************************************************** '
    ' Name: CreateExport (Private)
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function CreateExport(ByRef r_lTransactionExportFolderCnt As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        m_lReturn = CreateExportFolder(r_lTransactionExportFolderCnt:=r_lTransactionExportFolderCnt)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="CreateExportFolder Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateExport")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' TF270799 - Log Message and End if Premium = 0.
        'eck201200 check agianst total including extras
        '    If (m_vPolicyPremium = 0) Then
        'DC180401 check also that PolicyPremium Is Zero Before Exitting
        'If (m_vPostingAmount = 0) Then
        If (m_vPostingAmount = 0 And m_vPolicyPremium = 0) And Not m_bIsContra Then
            'DC180401
        End If

        m_lReturn = CreateExportDetails(v_lTransactionExportFolderCnt:=r_lTransactionExportFolderCnt)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="CreateExportDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateExport")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Remove the call to RoundupExportFolder
        'If rounding was required, this call was rounding the last entry for this
        'transaction. By removing this, rounding will be performed by
        'the ImportSiriusTrans component which will post to GLDEBIT instead

        '    m_lReturn = RoundupExportFolder( _
        ''                    v_lTransactionExportFolderCnt:=r_lTransactionExportFolderCnt)
        '
        '    If (m_lReturn <> PMTrue) Then
        '        LogMessage m_sUsername, _
        ''            iType:=PMLogError, _
        ''            sMsg:="RoundupExportFolder Failed", _
        ''            vApp:=ACApp, _
        ''            vClass:=ACClass, _
        ''            vMethod:="CreateExport"
        '        CreateExport = PMFalse
        '        Exit Function
        '    End If
        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CreateExportFolder (Private)
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function CreateExportFolder(ByRef r_lTransactionExportFolderCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim oInsuranceFile As bSIRInsuranceFile.Services
        Dim oParty As Object = Nothing 'bSIRParty.Services
        Dim lRecordsAffected As Integer
        Dim sSQL As String = ""
        Dim r_vResultArray(,) As Object = Nothing
        Dim dtPaymentDueDate As Date
        'DC070706
        Dim lDays, lEventLogId As Integer
        Dim vResults(,) As Object = Nothing


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get Party IDs from Insurance_File

        oInsuranceFile = New bSIRInsuranceFile.Services
        m_lReturn = oInsuranceFile.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to Create 'bSIRInsuranceFile.Services'.", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateExportFolder", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        oInsuranceFile.FromEvent = True

        oInsuranceFile.InsuranceFileCnt = m_lInsuranceFileCnt

        m_lInsuranceHolderCnt = oInsuranceFile.InsuranceHolderCnt
        'EK 17/10/99

        If Not (Convert.IsDBNull(oInsuranceFile.AccountHandlerCnt) Or Informations.IsNothing(oInsuranceFile.AccountHandlerCnt)) Then

            m_lAccountHandlerCnt = oInsuranceFile.AccountHandlerCnt
        End If

        If Not (Convert.IsDBNull(oInsuranceFile.LeadAgentCnt) Or Informations.IsNothing(oInsuranceFile.LeadAgentCnt)) Then

            m_lAgentCnt = oInsuranceFile.LeadAgentCnt
        End If
        ' TF270799

        m_vPolicyPremium = oInsuranceFile.ThisPremium

        'eck020600 Pass branch

        m_iPolicySourceID = oInsuranceFile.SourceID

        'm_lReturn = oInsuranceFile.Terminate()
        oInsuranceFile.Dispose()
        oInsuranceFile = Nothing

        ' TF270799 - End if Policy Premium = 0
        'eck201200 check agianst total including extras
        '    If (m_vPolicyPremium = 0) Then
        'DC180401 check also that PolicyPremium Is Zero Before Exitting
        'If (m_vPostingAmount = 0) Then
        If (m_vPostingAmount = 0 And m_vPolicyPremium = 0) And Not m_bIsContra Then
            'DC180401
            Return result
        End If

        ' Set Party account keys
        m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oParty, v_sClassName:="bSIRParty.Services", v_sCallingAppName:=m_sCallingAppName, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to Create 'bSIRParty.Services'.", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateExportFolder", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        oParty.PartyCnt = m_lInsuranceHolderCnt

        m_lReturn = oParty.GetDetails()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to GetDetails for Insurance Holder " & m_lInsuranceHolderCnt & "'.", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateExportFolder", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        '   SJP 04072002 - Account Key is now = Party Count
        '       Still passed into CalcCombinedKey but should just be
        '           returned.

        m_lReturn = gPMComponentServices.calccombinedkey(v_lSourceID:=oParty.SourceID, v_lKeyID:=oParty.PartyID, r_lCombinedKeyID:=m_lInsuranceHolderAccountKey)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="gPMComponentServices.calccombinedkey failed for insurance folder", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateExportFolder")
            Return result
        End If

        oParty.Dispose()
        oParty = Nothing

        ' Get Account Handler details if > 1
        If m_lAccountHandlerCnt > 1 Then
            m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oParty, v_sClassName:="bSIRParty.Services", v_sCallingAppName:=m_sCallingAppName, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to Create 'bSIRParty.Services'.", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateExportFolder", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            oParty.PartyCnt = m_lAccountHandlerCnt

            m_lReturn = oParty.GetDetails()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to GetDetails for Account Handler " & m_lAccountHandlerCnt & "'.", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateExportFolder", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            '   SJP 04072002 - Account Key is now = Party Count
            '       Still passed into CalcCombinedKey but should just be
            '       returned.

            m_lAccountHandlerAccountKey = oParty.PartyCnt

            m_lReturn = gPMComponentServices.calccombinedkey(v_lSourceID:=oParty.SourceID, v_lKeyID:=oParty.PartyID, r_lCombinedKeyID:=m_lAccountHandlerAccountKey)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="gPMComponentServices.calccombinedkey failed for account handler", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateExportFolder")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oParty.Dispose()
            oParty = Nothing
        Else
            m_lAccountHandlerAccountKey = 0
        End If
        ' Get Agent Cnt details if > 1
        If m_lAgentCnt > 1 Then
            m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oParty, v_sClassName:="bSIRParty.Services", v_sCallingAppName:=m_sCallingAppName, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to Create 'bSIRParty.Services'.", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateExportFolder", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            oParty.PartyCnt = m_lAgentCnt

            m_lReturn = oParty.GetDetails()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to GetDetails for Agent " & m_lAgentCnt & "'.", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateExportFolder", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            '   SJP 04072002 - Account Key is now = Party Count
            '      Still passed into CalcCombinedKey but should just be
            '      returned.

            m_lAgentAccountKey = oParty.PartyCnt

            m_lReturn = gPMComponentServices.calccombinedkey(v_lSourceID:=oParty.SourceID, v_lKeyID:=oParty.PartyID, r_lCombinedKeyID:=m_lAgentAccountKey)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="gPMComponentServices.calccombinedkey failed for agent ", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateExportFolder")
                Return result
            End If

            oParty.Dispose()
            oParty = Nothing
        Else
            m_lAgentAccountKey = 0
        End If
        'EK 220200 Derive Document Type
        'EK 130300 Extended this
        Select Case m_sDebitCredit
            Case "D"
                Select Case m_vLastTransType
                    Case 1
                        m_sGroupCode = ACTConst.ACTAutoNumberGroupCodeDocumentRef4
                        m_sRangeCode = ACTConst.ACTAutoNumberRangeCodeSnd
                    Case 2
                        m_sGroupCode = ACTConst.ACTAutoNumberGroupCodeDocumentRef15
                        m_sRangeCode = ACTConst.ACTAutoNumberRangeCodeSrd
                    Case 3
                        m_sGroupCode = ACTConst.ACTAutoNumberGroupCodeDocumentRef17
                        m_sRangeCode = ACTConst.ACTAutoNumberRangeCodeSed
                    Case 4
                        'eck020500
                        m_sGroupCode = ACTConst.ACTAutoNumberGroupCodeDocumentRef31
                        m_sRangeCode = ACTConst.ACTAutoNumberRangeCodeShd
                    Case 5
                        m_sGroupCode = ACTConst.ACTAutoNumberGroupCodeDocumentRef35
                        m_sRangeCode = ACTConst.ACTAutoNumberRangeCodeTrd
                End Select
            Case "C"
                Select Case m_vLastTransType
                    Case 1
                        m_sGroupCode = ACTConst.ACTAutoNumberGroupCodeDocumentRef5
                        m_sRangeCode = ACTConst.ACTAutoNumberRangeCodeSnc
                    Case 2
                        m_sGroupCode = ACTConst.ACTAutoNumberGroupCodeDocumentRef16
                        m_sRangeCode = ACTConst.ACTAutoNumberRangeCodeSrc
                    Case 3
                        m_sGroupCode = ACTConst.ACTAutoNumberGroupCodeDocumentRef18
                        m_sRangeCode = ACTConst.ACTAutoNumberRangeCodeSec
                        'EK 130300
                    Case 4
                        'eck020500
                        m_sGroupCode = ACTConst.ACTAutoNumberGroupCodeDocumentRef32
                        m_sRangeCode = ACTConst.ACTAutoNumberRangeCodeShc
                    Case 5
                        m_sGroupCode = ACTConst.ACTAutoNumberGroupCodeDocumentRef36
                        m_sRangeCode = ACTConst.ACTAutoNumberRangeCodeTrc

                End Select
        End Select

        ' Get Document Ref
        '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
        m_lReturn = GenerateDocumentRef(v_sGroupCode:=m_sGroupCode, v_sRangeCode:=m_sRangeCode, v_iUserID:=m_iUserID, v_iCompanyID:=m_iPolicySourceID, r_sDocumentRef:=m_sReference)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Generate New Document Ref.", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateExportFolder", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'S4BDAT004
        'DC070706 Datasure corrected as if no terms of payment set, it crashed
        lDays = 0
        If m_lTermsOfPaymentId <> 0 Then

            m_lReturn = GetTermsOfPayment(v_lTermsOfPaymentId:=m_lTermsOfPaymentId, r_vResult:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get Terms of Payment", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateExportFolder", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lDays = CInt(r_vResultArray(6, 0))
        End If

        dtPaymentDueDate = m_dtCoverStartDate.AddDays(lDays)
        ' (Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
        m_sDocumentRef = m_sRangeCode & m_sReference

        bPMAddParameter.AddParameterLite(v_oDatabase:=m_oDatabase, v_sName:="user_id", v_vValue:=m_iUserID, v_iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, v_iDataType:=gPMConstants.PMEDataType.PMInteger, v_bClearParameters:=True)

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=AC_SQL_GetEventLogId_Sql, sSQLName:=AC_SQL_GetEventLogId_Name, bStoredProcedure:=AC_SQL_GetEventLogId_Stored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResults)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to retrieve the Event_Log identity field", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateExportFolder", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        lEventLogId = gPMFunctions.ToSafeLong(vResults(0, 0))

        With m_oDatabase

            ' Clear the Database Parameters Collection
            .Parameters.Clear()

            ' Add ExportFolderCnt as an OUTPUT param
            m_lReturn = .Parameters.Add(sName:="transaction_export_folder_cnt", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add InsuranceFileCnt as an INPUT param
            m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(m_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add UserID as an INPUT param
            m_lReturn = .Parameters.Add(sName:="user_id", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add UserName as an INPUT param
            m_lReturn = .Parameters.Add(sName:="user_name", vValue:=m_sUsername, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add InsuranceHolder account key as an INPUT param
            m_lReturn = .Parameters.Add(sName:="insurance_holder_account_key", vValue:=CStr(m_lInsuranceHolderAccountKey), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add AccountHandler account key as an INPUT param
            m_lReturn = .Parameters.Add(sName:="account_handler_account_key", vValue:=CStr(m_lAccountHandlerAccountKey), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add Agent account key as an INPUT param
            m_lReturn = .Parameters.Add(sName:="agent_account_key", vValue:=CStr(m_lAgentAccountKey), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add Document Ref as an INPUT param
            m_lReturn = .Parameters.Add(sName:="document_ref", vValue:=m_sDocumentRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' EK 26/11/99 Add Debit/Credit Indicator as an INPUT param
            m_lReturn = .Parameters.Add(sName:="debit_credit", vValue:=m_sDebitCredit, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'eck030500
            m_lReturn = .Parameters.Add(sName:="document_date", vValue:=Informations.FormatDateTime(m_dtLastTransDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'eck030500
            'eck060601
            m_lReturn = .Parameters.Add(sName:="reason", vValue:=m_sReason, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'S4BDAT004
            m_lReturn = .Parameters.Add(sName:="terms_of_payment_id", vValue:=CStr(m_lTermsOfPaymentId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="payment_due_date", vValue:=Informations.FormatDateTime(dtPaymentDueDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            bPMAddParameter.AddParameterLite(v_oDatabase:=m_oDatabase, v_sName:="event_log_id", v_vValue:=lEventLogId, v_iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, v_iDataType:=gPMConstants.PMEDataType.PMLong)

            'End MSS12062001

            ' Execute Add Trans Export Folder SQL Statement
            m_lReturn = .SQLAction(sSQL:=ACAddPMBExportFolderSQL, sSQLName:=ACAddPMBExportFolderName, bStoredProcedure:=ACAddPMBExportFolderStored, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.SQLAction failed for spu_PMB_trans_folder_add", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateExportFolder")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Cnt of the record inserted
            r_lTransactionExportFolderCnt = .Parameters.Item("transaction_export_folder_cnt").Value
            If r_lTransactionExportFolderCnt < 1 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the returned transaction_export_folder_cnt to the event associated with this transaction
            ' Start MSS12062001

            sSQL = "Update Event_Log Set Transaction_export_folder_cnt = " & r_lTransactionExportFolderCnt & " Where Event_Cnt = " & CStr(lEventLogId)

            m_lReturn = .SQLAction(sSQL:=sSQL, sSQLName:="ACUpdateEvent", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.SQLAction failed to update event log", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateExportFolder")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'End MSS12062001

        End With

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: PopulateStoredProcedures
    '
    ' Description: Populates the passed in array with the values
    '              that used to live in the transaction_options table
    '
    ' History: 20/12/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function PopulateStoredProcedures(ByRef r_vStoredProcedures(,) As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' 10 columns, 10 rows
        ReDim r_vStoredProcedures(9, 9)

        r_vStoredProcedures(0, 0) = "agent_cnt" ' field

        r_vStoredProcedures(1, 0) = ">" ' comparison_indicator

        r_vStoredProcedures(2, 0) = "0" ' comparison_value

        r_vStoredProcedures(3, 0) = "spu_pmb_trans_det_agent" ' stored_proc

        r_vStoredProcedures(4, 0) = "agent_amount" ' return_value_1

        r_vStoredProcedures(5, 0) = "" ' return_value_2

        r_vStoredProcedures(6, 0) = "" ' return_value_3

        r_vStoredProcedures(7, 0) = "" ' return_value_4

        r_vStoredProcedures(8, 0) = "" ' return_value_5

        r_vStoredProcedures(9, 0) = " " ' blank

        r_vStoredProcedures(0, 1) = "coinsurers_count" ' field

        r_vStoredProcedures(1, 1) = "=" ' comparison_indicator

        r_vStoredProcedures(2, 1) = "0" ' comparison_value

        r_vStoredProcedures(3, 1) = "spu_pmb_trans_det_insurer" ' stored_proc

        r_vStoredProcedures(4, 1) = "total_insurers" ' return_value_1

        r_vStoredProcedures(5, 1) = "total_insurers_commission" ' return_value_2

        r_vStoredProcedures(6, 1) = "total_insurers_ipt" ' return_value_3

        r_vStoredProcedures(7, 1) = "total_insurers_fee" ' return_value_4

        r_vStoredProcedures(8, 1) = "" ' return_value_5

        r_vStoredProcedures(9, 1) = " " ' blank

        r_vStoredProcedures(0, 2) = "coinsurers_count" ' field

        r_vStoredProcedures(1, 2) = ">" ' comparison_indicator

        r_vStoredProcedures(2, 2) = "0" ' comparison_value

        r_vStoredProcedures(3, 2) = "spu_pmb_trans_det_coinsurer" ' stored_proc

        r_vStoredProcedures(4, 2) = "total_coinsurers" ' return_value_1

        r_vStoredProcedures(5, 2) = "total_coinsurers_commission" ' return_value_2

        r_vStoredProcedures(6, 2) = "total_coinsurers_ipt" ' return_value_3

        r_vStoredProcedures(7, 2) = "total_coinsurers_fee" ' return_value_4

        r_vStoredProcedures(8, 2) = "" ' return_value_5

        r_vStoredProcedures(9, 2) = " " ' blank
        'Datasure removed fields used for IPT calculation

        r_vStoredProcedures(0, 3) = "extras_count" ' field

        r_vStoredProcedures(1, 3) = ">" ' comparison_indicator

        r_vStoredProcedures(2, 3) = "0" ' comparison_value

        r_vStoredProcedures(3, 3) = "spu_pmb_trans_det_extra" ' stored_proc

        r_vStoredProcedures(4, 3) = "total_extras" ' return_value_1

        r_vStoredProcedures(5, 3) = "total_extras_commission" ' return_value_2

        r_vStoredProcedures(6, 3) = "" ' return_value_3

        r_vStoredProcedures(7, 3) = "" ' return_value_4

        r_vStoredProcedures(8, 3) = "" ' return_value_5

        r_vStoredProcedures(9, 3) = " " ' blank

        r_vStoredProcedures(0, 4) = "fees_count" ' field

        r_vStoredProcedures(1, 4) = ">" ' comparison_indicator

        r_vStoredProcedures(2, 4) = "0" ' comparison_value

        r_vStoredProcedures(3, 4) = "spu_pmb_trans_det_fee" ' stored_proc

        r_vStoredProcedures(4, 4) = "total_fees" ' return_value_1

        r_vStoredProcedures(5, 4) = "" ' return_value_2

        r_vStoredProcedures(6, 4) = "" ' return_value_3

        r_vStoredProcedures(7, 4) = "" ' return_value_4

        r_vStoredProcedures(8, 4) = "" ' return_value_5

        r_vStoredProcedures(9, 4) = " " ' blank

        r_vStoredProcedures(0, 5) = "discount_count" ' field

        r_vStoredProcedures(1, 5) = ">" ' comparison_indicator

        r_vStoredProcedures(2, 5) = "0" ' comparison_value

        r_vStoredProcedures(3, 5) = "spu_pmb_trans_det_discount" ' stored_proc

        r_vStoredProcedures(4, 5) = "total_discount" ' return_value_1

        r_vStoredProcedures(5, 5) = "" ' return_value_2

        r_vStoredProcedures(6, 5) = "" ' return_value_3

        r_vStoredProcedures(7, 5) = "" ' return_value_4

        r_vStoredProcedures(8, 5) = "" ' return_value_5

        r_vStoredProcedures(9, 5) = " " ' blank

        r_vStoredProcedures(0, 6) = "policyshares_count" ' field

        r_vStoredProcedures(1, 6) = "=" ' comparison_indicator

        r_vStoredProcedures(2, 6) = "0" ' comparison_value

        r_vStoredProcedures(3, 6) = "spu_pmb_trans_det_client" ' stored_proc

        r_vStoredProcedures(4, 6) = "" ' return_value_1

        r_vStoredProcedures(5, 6) = "" ' return_value_2

        r_vStoredProcedures(6, 6) = "" ' return_value_3

        r_vStoredProcedures(7, 6) = "" ' return_value_4

        r_vStoredProcedures(8, 6) = "" ' return_value_5

        r_vStoredProcedures(9, 6) = " " ' blank

        r_vStoredProcedures(0, 7) = "policyshares_count" ' field

        r_vStoredProcedures(1, 7) = ">" ' comparison_indicator

        r_vStoredProcedures(2, 7) = "0" ' comparison_value

        r_vStoredProcedures(3, 7) = "spu_pmb_trans_det_shares" ' stored_proc

        r_vStoredProcedures(4, 7) = "" ' return_value_1

        r_vStoredProcedures(5, 7) = "" ' return_value_2

        r_vStoredProcedures(6, 7) = "" ' return_value_3

        r_vStoredProcedures(7, 7) = "" ' return_value_4

        r_vStoredProcedures(8, 7) = "" ' return_value_5

        r_vStoredProcedures(9, 7) = " " ' blank

        r_vStoredProcedures(0, 8) = "subagent_cnt" ' field

        r_vStoredProcedures(1, 8) = ">" ' comparison_indicator

        r_vStoredProcedures(2, 8) = "0" ' comparison_value

        r_vStoredProcedures(3, 8) = "spu_pmb_trans_det_subagent" ' stored_proc

        r_vStoredProcedures(4, 8) = "subagent_commission" ' return_value_1

        r_vStoredProcedures(5, 8) = "" ' return_value_2

        r_vStoredProcedures(6, 8) = "" ' return_value_3

        r_vStoredProcedures(7, 8) = "" ' return_value_4

        r_vStoredProcedures(8, 8) = "" ' return_value_5

        r_vStoredProcedures(9, 8) = " " ' blank

        r_vStoredProcedures(0, 9) = "last_trans_type_id" ' field

        r_vStoredProcedures(1, 9) = ">" ' comparison_indicator

        r_vStoredProcedures(2, 9) = "0" ' comparison_value

        r_vStoredProcedures(3, 9) = "spu_pmb_trans_det_commission" ' stored_proc

        r_vStoredProcedures(4, 9) = "" ' return_value_1

        r_vStoredProcedures(5, 9) = "" ' return_value_2

        r_vStoredProcedures(6, 9) = "" ' return_value_3

        r_vStoredProcedures(7, 9) = "" ' return_value_4

        r_vStoredProcedures(8, 9) = "" ' return_value_5

        r_vStoredProcedures(9, 9) = " " ' blank

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: SetTransactionParameters
    '
    ' Description: Sets the correct parameters for the stored procedure
    '
    ' History: 21/12/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function SetTransactionParameters(ByVal v_sStoredProc As String, ByVal v_lTransactionExportFolderCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim sProc As String = ""
        Dim iStart, iQuestion As Integer

        Const FCCall As String = "{call "



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get the name of the procedure from the passed in {call ... string
        iStart = (v_sStoredProc.IndexOf(FCCall) + 1)
        If iStart <> 0 Then

            iQuestion = (v_sStoredProc.IndexOf("(?") + 1)

            If iQuestion > 0 Then
                sProc = v_sStoredProc.Substring(iStart + FCCall.Length - 1, Math.Min(v_sStoredProc.Length, iQuestion - iStart - FCCall.Length))
            Else
                sProc = v_sStoredProc
            End If

        Else

            sProc = v_sStoredProc

        End If

        ' Add the common parameters

        bPMAddParameter.AddParameterLite(m_oDatabase, "transaction_export_folder_cnt", v_lTransactionExportFolderCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
        bPMAddParameter.AddParameterLite(m_oDatabase, "transaction_type", m_sDebitCredit, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

        Select Case (sProc)
            Case "spu_pmb_trans_det_agent"
                bPMAddParameter.AddParameterLite(m_oDatabase, "agent_amount", m_cAgentAmount, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency)

            Case "spu_pmb_trans_det_insurer"
                bPMAddParameter.AddParameterLite(m_oDatabase, "total_insurers", m_cInsurersAmount, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency)
                bPMAddParameter.AddParameterLite(m_oDatabase, "total_insurers_commission", m_cInsurersCommission, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency)
                bPMAddParameter.AddParameterLite(m_oDatabase, "total_insurers_ipt", m_cInsurersIPT, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency)
                bPMAddParameter.AddParameterLite(m_oDatabase, "total_insurers_fee", m_cInsurersFee, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency)

            Case "spu_pmb_trans_det_coinsurer"
                bPMAddParameter.AddParameterLite(m_oDatabase, "total_coinsurers", m_cCoinsurersAmount, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency)
                bPMAddParameter.AddParameterLite(m_oDatabase, "total_coinsurers_commission", m_cCoinsurersCommission, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency)
                bPMAddParameter.AddParameterLite(m_oDatabase, "total_coinsurers_ipt", m_cCoinsurersIPT, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency)
                bPMAddParameter.AddParameterLite(m_oDatabase, "total_coinsurers_fee", m_cCoinsurersFee, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency)

            Case "spu_pmb_trans_det_extra"
                'Datasure - remove parameters used for calculating IPT
                '   AddParameterLite m_oDatabase, "policy_start_date", m_dtPolicyStartDate, PMParamInput, PMDate
                bPMAddParameter.AddParameterLite(m_oDatabase, "total_extras", m_cExtrasAmount, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency)
                bPMAddParameter.AddParameterLite(m_oDatabase, "total_extras_commission", m_cExtrasCommission, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency)
                '    AddParameterLite m_oDatabase, "total_extras_ipt", m_cExtrasIPT, PMParamOutput, PMCurrency

            Case "spu_pmb_trans_det_fee"
                bPMAddParameter.AddParameterLite(m_oDatabase, "total_fees", m_cFeesAmount, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency)

            Case "spu_pmb_trans_det_discount"
                bPMAddParameter.AddParameterLite(m_oDatabase, "total_discount", m_cDiscountAmount, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency)

            Case "spu_pmb_trans_det_client", "spu_pmb_trans_det_shares"
                bPMAddParameter.AddParameterLite(m_oDatabase, "total_extras_gross", m_cExtrasAmount + m_cExtrasCommission + m_cExtrasIPT, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
                bPMAddParameter.AddParameterLite(m_oDatabase, "total_fees_calc", m_cFeesAmount, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
                bPMAddParameter.AddParameterLite(m_oDatabase, "total_discount_calc", m_cDiscountAmount, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
                bPMAddParameter.AddParameterLite(m_oDatabase, "total_insurer_fee_calc", m_cInsurersFee + m_cCoinsurersFee, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)

            Case "spu_pmb_trans_det_subagent"
                bPMAddParameter.AddParameterLite(m_oDatabase, "total_extras_gross", m_cExtrasAmount + m_cExtrasCommission + m_cExtrasIPT, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
                bPMAddParameter.AddParameterLite(m_oDatabase, "total_fees_calc", m_cFeesAmount, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
                bPMAddParameter.AddParameterLite(m_oDatabase, "total_discount_calc", m_cDiscountAmount, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
                bPMAddParameter.AddParameterLite(m_oDatabase, "total_insurer_fee_calc", m_cInsurersFee + m_cCoinsurersFee, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
                bPMAddParameter.AddParameterLite(m_oDatabase, "subagent_commission", m_cSubAgentCommission, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency)

            Case "spu_pmb_trans_det_commission"

                bPMAddParameter.AddParameterLite(m_oDatabase, "agent_amount_calc", m_cAgentAmount, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
                bPMAddParameter.AddParameterLite(m_oDatabase, "total_extras_comm_calc", m_cExtrasCommission, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
                bPMAddParameter.AddParameterLite(m_oDatabase, "total_insurers_comm_calc", m_cInsurersCommission, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
                bPMAddParameter.AddParameterLite(m_oDatabase, "total_coinsurers_comm_calc", m_cCoinsurersCommission, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
                bPMAddParameter.AddParameterLite(m_oDatabase, "subagent_comm_calc", m_cSubAgentCommission, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)

            Case Else

                ' If you reach here, then you need to add some code to
                ' support your stored_procedure. Either that or there
                ' is a bug in getting the procedure name...

                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unknown stored procedure." & Environment.NewLine &
                                   "v_sProcedure:=" & v_sStoredProc &
                                   "sProc:=" & sProc, vApp:=ACApp, vClass:=ACClass, vMethod:="SetTransactionParameters", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result

        End Select

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CreateExportDetails (Private)
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function CreateExportDetails(ByRef v_lTransactionExportFolderCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim s As String = ""
        Dim lRecordsAffected As Integer
        Dim vStoredProcedures(,) As Object = Nothing
        Dim sReturnParameter As String = ""
        Dim cReturnValue As Decimal
        'SR 24/03/00 Marsh
        Dim sTemp As String = ""
        ' CTAF 211200
        Dim sSQL As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase
            'Check the registry setting for the marsh accounting
            m_lReturn = gPMFunctions.GetPMRegSetting(gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, gPMConstants.PMEProductFamily.pmePFSiriusSolutions, gPMConstants.PMERegSettingLevel.pmeRSLCommon, "Marsh Accounting", sTemp)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                'Set the marsh accounting to False
                sTemp = "0"

            End If

            If sTemp = "1" Then 'Transactions for Marsh Accounts

                'Clear the parameters
                .Parameters.Clear()

                'Add the Insurance File count as the input parameter
                m_lReturn = .Parameters.Add(sName:="transaction_export_folder_cnt", vValue:=CStr(v_lTransactionExportFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                'Call the SP to add the records into the Export Detail
                m_lReturn = .SQLAction(sSQL:=ACAddMSHExportDetailsControlSQL, sSQLName:=ACAddMSHExportDetailsControlName, bStoredProcedure:=ACAddMSHExportDetailsControlStored, lRecordsAffected:=lRecordsAffected)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else
                ' Marsh accounting is not enabled.
                ' So, Generate default Back Office transactions

                ' CTAF 201200
                ' This seems a little OTT for our purposes and is somewhat
                ' extremely difficult to maintain.
                ' Removed stored procedure in favour of manually shoving the data
                ' into the vStoredProcedures variable

                'm_lReturn = .SQLSelect( _
                ''    sSQL:=ACSelectPMBTransactionOptionsSQL, _
                ''    sSQLNAME:=ACSelectPMBTransactionOptionsName, _
                ''    bStoredprocedure:=ACSelectPMBTransactionOptionsStored, _
                ''    vResultArray:=vStoredProcedures)
                'If IsArray(vStoredProcedures) = False Then
                '    CreateExportDetails = PMFalse
                '    Exit Function
                'End If

                m_lReturn = PopulateStoredProcedures(r_vStoredProcedures:=vStoredProcedures)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Clear the Database Parameters Collection
                m_oDatabase.Parameters.Clear()

                m_lReturn = SetPropertiesParameters()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Call controlling transaction query
                'TF140802 - Removed RecordsAffected
                m_lReturn = .SQLAction(sSQL:=ACSelectPMBExportPropertiesSQL, sSQLName:=ACSelectPMBExportPropertiesName, bStoredProcedure:=ACSelectPMBExportPropertiesStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_vPaidDirect = .Parameters.Item("paid_direct").Value

                For i As Integer = vStoredProcedures.GetLowerBound(1) To vStoredProcedures.GetUpperBound(1)

                    vStoredProcedures(9, i) = gPMConstants.PMEReturnCode.PMFalse

                    s = CStr(vStoredProcedures(0, i))

                    Select Case vStoredProcedures(1, i)
                        Case "="

                            If .Parameters.Item(s).Value = CStr(vStoredProcedures(2, i)) Then

                                vStoredProcedures(9, i) = gPMConstants.PMEReturnCode.PMTrue
                            End If
                        Case "<>"

                            If .Parameters.Item(s).Value <> CStr(vStoredProcedures(2, i)) Then

                                vStoredProcedures(9, i) = gPMConstants.PMEReturnCode.PMTrue
                            End If
                        Case "<"

                            If .Parameters.Item(s).Value < CStr(vStoredProcedures(2, i)) Then

                                vStoredProcedures(9, i) = gPMConstants.PMEReturnCode.PMTrue
                            End If
                        Case ">"

                            If .Parameters.Item(s).Value > CStr(vStoredProcedures(2, i)) Then

                                vStoredProcedures(9, i) = gPMConstants.PMEReturnCode.PMTrue
                            End If
                    End Select
                Next i

                For i As Integer = vStoredProcedures.GetLowerBound(1) To vStoredProcedures.GetUpperBound(1)

                    If vStoredProcedures(9, i) = gPMConstants.PMEReturnCode.PMTrue Then

                        sSQL = CStr(vStoredProcedures(3, i))

                        ' Decide which parameters we need to add
                        m_lReturn = SetTransactionParameters(v_sStoredProc:=sSQL, v_lTransactionExportFolderCnt:=v_lTransactionExportFolderCnt)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        m_lReturn = .SQLAction(sSQL:=sSQL, sSQLName:=ACAddPMBExportDetailsControlName, bStoredProcedure:=ACAddPMBExportDetailsControlStored, lRecordsAffected:=lRecordsAffected)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        For x As Integer = 4 To 8
                            Dim auxVar As Object = vStoredProcedures(x, i)

                            If Not (Convert.IsDBNull(auxVar) Or Informations.IsNothing(auxVar)) Then

                                sReturnParameter = CStr(vStoredProcedures(x, i))
                                If sReturnParameter <> "" Then

                                    If Convert.IsDBNull(.Parameters.Item(sReturnParameter).Value) Or Informations.IsNothing(.Parameters.Item(sReturnParameter).Value) Then
                                        cReturnValue = 0
                                    Else
                                        cReturnValue = .Parameters.Item(sReturnParameter).Value
                                    End If
                                    m_lReturn = GetTransactionOutputParameters(sReturnParameter:=sReturnParameter, cReturnValue:=cReturnValue)
                                End If
                            End If
                        Next x
                    End If
                Next i
            End If
        End With

        Return result

    End Function
    'EK 200200 Three New Methods to Generate Direct Debit AJ's
    ' ***************************************************************** '
    ' Name: CreateAJExport (Private)
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function CreateAJExport(ByRef r_lTransactionExportFolderCnt As Integer) As Integer

        Dim result As Integer = 0
        'Const kMethodName As String = "CreateAJExport"


        result = gPMConstants.PMEReturnCode.PMTrue

        m_lReturn = CreateAJExportFolder(r_lTransactionExportFolderCnt:=r_lTransactionExportFolderCnt)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("CreateAJExportDetails", "v_lTransactionExportFolderCnt:=" & r_lTransactionExportFolderCnt, gPMConstants.PMELogLevel.PMLogError)
        End If

        If (m_vPostingAmount = 0 And m_vPolicyPremium = 0) And Not m_bIsContra Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Transaction Not Posted as Premium Is Zero.", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateAJExport", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        m_lReturn = CreateAJExportDetails(v_lTransactionExportFolderCnt:=r_lTransactionExportFolderCnt)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("CreateAJExportDetails", "v_lTransactionExportFolderCnt:=" & r_lTransactionExportFolderCnt, gPMConstants.PMELogLevel.PMLogError)
        End If

        m_lReturn = RoundupExportFolder(v_lTransactionExportFolderCnt:=r_lTransactionExportFolderCnt)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("RoundupExportFolder", "r_lTransactionExportFolderCnt:=" & r_lTransactionExportFolderCnt, gPMConstants.PMELogLevel.PMLogError)
        End If

        ' Do any tidy up, e.g. Set x = Nothing here
        Return result
        ' This is for debugging only
    End Function

    'DC260105 : Introducer Transaction Processing
    ' ***************************************************************** '
    ' Name: CreateIntroducerExport (Private)
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function CreateIntroducerExport(ByRef r_lTransactionExportFolderCnt As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        m_lReturn = CreateIntroducerExportFolder(r_lTransactionExportFolderCnt:=r_lTransactionExportFolderCnt)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If (m_vPostingAmount = 0 And m_vPolicyPremium = 0) And Not m_bIsContra Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Transaction Not Posted as Premium Is Zero.", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateIntroducerExport", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        m_lReturn = CreateIntroducerExportDetails(v_lTransactionExportFolderCnt:=r_lTransactionExportFolderCnt)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = RoundupExportFolder(v_lTransactionExportFolderCnt:=r_lTransactionExportFolderCnt)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    Private Function CreateAJExportFolder(ByRef r_lTransactionExportFolderCnt As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CreateAJExportFolder"

        Dim sSQL As String = ""
        Dim oInsuranceFile As New bSIRInsuranceFile.Services
        Dim oParty As Object = Nothing
        Dim lRecordsAffected As Integer
        Dim dtPaymentDueDate As Date
        Dim r_vResultArray(,) As Object = Nothing
        Dim lEventLogId As Integer
        Dim vResults(,) As Object = Nothing

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            If m_bManualAJ Then

                'Get Party IDs from Insurance_File

                oInsuranceFile = New bSIRInsuranceFile.Services
                m_lReturn = oInsuranceFile.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("gPMComponentServices.CreateBusinessObject", "v_sClassName:=bSIRInsuranceFile.Services", gPMConstants.PMELogLevel.PMLogError)
                End If

                oInsuranceFile.FromEvent = True

                oInsuranceFile.InsuranceFileCnt = m_lInsuranceFileCnt

                m_lInsuranceHolderCnt = oInsuranceFile.InsuranceHolderCnt

                If Not (Convert.IsDBNull(oInsuranceFile.AccountHandlerCnt) Or Informations.IsNothing(oInsuranceFile.AccountHandlerCnt)) Then

                    m_lAccountHandlerCnt = oInsuranceFile.AccountHandlerCnt
                End If

                If Not (Convert.IsDBNull(oInsuranceFile.LeadAgentCnt) Or Informations.IsNothing(oInsuranceFile.LeadAgentCnt)) Then

                    m_lAgentCnt = oInsuranceFile.LeadAgentCnt
                End If

                m_vPolicyPremium = gPMFunctions.ToSafeCurrency(oInsuranceFile.ThisPremium)

                m_iPolicySourceID = oInsuranceFile.SourceID

                If m_vPostingAmount = 0 And m_vPolicyPremium = 0 And Not m_bIsContra Then
                    Return result
                End If

            Else
                If m_vPostingAmount = 0 And m_vPolicyPremium = 0 And Not m_bIsContra Then
                    Return result
                End If
            End If

            Select Case m_sDebitCredit
                Case "D"
                    m_sGroupCode = ACTConst.ACTAutoNumberGroupCodeDocumentRef33
                    m_sRangeCode = ACTConst.ACTAutoNumberRangeCodeDid
                Case "C"
                    m_sGroupCode = ACTConst.ACTAutoNumberGroupCodeDocumentRef34
                    m_sRangeCode = ACTConst.ACTAutoNumberRangeCodeDic
            End Select

            'Get Document Ref
            '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
            m_lReturn = GenerateDocumentRef(v_sGroupCode:=m_sGroupCode, v_sRangeCode:=m_sRangeCode, v_iUserID:=m_iUserID, v_iCompanyID:=m_iPolicySourceID, r_sDocumentRef:=m_sReference)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GenerateDocumentRef", "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
            m_sDocumentRef = m_sRangeCode & m_sReference

            m_lReturn = GetTermsOfPayment(v_lTermsOfPaymentId:=m_lTermsOfPaymentId, r_vResult:=r_vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetTermsOfPayment", "v_lTermsOfPaymentId:=" & m_lTermsOfPaymentId, gPMConstants.PMELogLevel.PMLogError)
            End If

            If Informations.IsArray(r_vResultArray) Then

                dtPaymentDueDate = m_dtEffectiveDate.AddDays(CDbl(r_vResultArray(6, r_vResultArray.GetUpperBound(1))))
            End If

            bPMAddParameter.AddParameterLite(m_oDatabase, "user_id", m_iUserID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, True)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=AC_SQL_GetEventLogId_Sql, sSQLName:=AC_SQL_GetEventLogId_Name, bStoredProcedure:=AC_SQL_GetEventLogId_Stored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResults)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "sSQLName:=AC_SQL_GetEventLogId_Name", gPMConstants.PMELogLevel.PMLogError)
            End If

            lEventLogId = gPMFunctions.ToSafeLong(vResults(0, 0))

            bPMAddParameter.AddParameterLite(m_oDatabase, "transaction_export_folder_cnt", 0, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "insurance_file_cnt", m_lInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "user_id", m_iUserID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "user_name", m_sUsername, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "insurance_holder_account_key", m_lInsuranceHolderCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "account_handler_account_key", m_lAccountHandlerCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "agent_account_key", m_lAgentCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "document_ref", m_sDocumentRef, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "debit_credit", m_sDebitCredit, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "document_date", m_dtLastTransDate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            bPMAddParameter.AddParameterLite(m_oDatabase, "reason", m_sReason, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

            bPMAddParameter.AddParameterLite(m_oDatabase, "terms_of_payment_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "payment_due_date", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            bPMAddParameter.AddParameterLite(m_oDatabase, "event_log_id", lEventLogId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddPMBExportFolderSQL, sSQLName:=ACAddPMBExportFolderName, bStoredProcedure:=ACAddPMBExportFolderStored, lRecordsAffected:=lRecordsAffected)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLAction", "sSQL:=ACAddPMBExportFolderSQL", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Get the Cnt of the record inserted
            r_lTransactionExportFolderCnt = m_oDatabase.Parameters.Item("transaction_export_folder_cnt").Value
            If r_lTransactionExportFolderCnt < 1 Then
                gPMFunctions.RaiseError("r_lTransactionExportFolderCnt < 1", "ACAddPMBExportFolderSQL Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Add the returned transaction_export_folder_cnt to the event associated with this transaction
            sSQL = "UPDATE Event_Log SET Transaction_export_folder_cnt = " & r_lTransactionExportFolderCnt & " WHERE Event_Cnt = " & CStr(lEventLogId)

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="ACUpdateEvent", bStoredProcedure:=False)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("oTransactionBusiness.ProcessAJTransactions", "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            Return result
        Catch ex As Exception
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
            m_lStatus = gPMConstants.PMEReturnCode.PMFail
        Finally
            If Not (oInsuranceFile Is Nothing) Then

                oInsuranceFile.Dispose()
                oInsuranceFile = Nothing
            End If

        End Try






        Return result
    End Function

    'DC260105 : Introducer Transaction Processing
    ' ***************************************************************** '
    ' Name: CreateIntroducerExportFolder (Private)
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function CreateIntroducerExportFolder(ByRef r_lTransactionExportFolderCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim lRecordsAffected As Integer
        Dim dtPaymentDueDate As Date
        Dim r_vResultArray(,) As Object = Nothing
        'DC140706 Datasure
        Dim lDays, lEventLogId As Integer
        Dim vResults(,) As Object = Nothing



        result = gPMConstants.PMEReturnCode.PMTrue

        If (m_vPostingAmount = 0 And m_vPolicyPremium = 0) And Not m_bIsContra Then
            Return result
        End If

        m_sGroupCode = ACTConst.ACTAutoNumberGroupCodeDocumentRef1
        m_sRangeCode = ACTConst.ACTAutoNumberRangeCodeJn

        ' Get Document Ref
        ' (Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
        m_lReturn = GenerateDocumentRef(v_sGroupCode:=m_sGroupCode, v_sRangeCode:=m_sRangeCode, v_iUserID:=m_iUserID, v_iCompanyID:=m_iPolicySourceID, r_sDocumentRef:=m_sReference)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Generate New Document Ref.", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateAJExportFolder", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        'Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
        m_sDocumentRef = m_sRangeCode & m_sReference

        'S4BDAT004

        'DC070706 Datasure corrected as if no terms of payment set, it crashed
        lDays = 0
        If m_lTermsOfPaymentId <> 0 Then

            m_lReturn = GetTermsOfPayment(v_lTermsOfPaymentId:=m_lTermsOfPaymentId, r_vResult:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get Terms of Payment", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateExportFolder", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lDays = CInt(r_vResultArray(6, 0))

        End If

        dtPaymentDueDate = m_dtEffectiveDate.AddDays(lDays)

        bPMAddParameter.AddParameterLite(v_oDatabase:=m_oDatabase, v_sName:="user_id", v_vValue:=m_iUserID, v_iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, v_iDataType:=gPMConstants.PMEDataType.PMInteger, v_bClearParameters:=True)

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=AC_SQL_GetEventLogId_Sql, sSQLName:=AC_SQL_GetEventLogId_Name, bStoredProcedure:=AC_SQL_GetEventLogId_Stored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResults)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to retrieve the Event_Log identity field", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateExportFolder", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        lEventLogId = gPMFunctions.ToSafeLong(vResults(0, 0))

        With m_oDatabase

            ' Clear the Database Parameters Collection
            .Parameters.Clear()

            ' Add ExportFolderCnt as an OUTPUT param
            m_lReturn = .Parameters.Add(sName:="transaction_export_folder_cnt", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add InsuranceFileCnt as an INPUT param
            m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(m_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add UserID as an INPUT param
            m_lReturn = .Parameters.Add(sName:="user_id", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add UserName as an INPUT param
            m_lReturn = .Parameters.Add(sName:="user_name", vValue:=m_sUsername, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add InsuranceHolder account key as an INPUT param
            m_lReturn = .Parameters.Add(sName:="insurance_holder_account_key", vValue:=CStr(m_lInsuranceHolderAccountKey), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add AccountHandler account key as an INPUT param
            m_lReturn = .Parameters.Add(sName:="account_handler_account_key", vValue:=CStr(m_lAccountHandlerAccountKey), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add Agent account key as an INPUT param
            m_lReturn = .Parameters.Add(sName:="agent_account_key", vValue:=CStr(m_lAgentAccountKey), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add Document Ref as an INPUT param
            m_lReturn = .Parameters.Add(sName:="document_ref", vValue:=m_sDocumentRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="debit_credit", vValue:=m_sDebitCredit, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="document_date", vValue:=Informations.FormatDateTime(m_dtLastTransDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="reason", vValue:="", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'S4BDAT004

            ' Developer Guide No.85
            m_lReturn = .Parameters.Add(sName:="terms_of_payment_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="payment_due_date", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            bPMAddParameter.AddParameterLite(v_oDatabase:=m_oDatabase, v_sName:="event_log_id", v_vValue:=lEventLogId, v_iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, v_iDataType:=gPMConstants.PMEDataType.PMLong)

            ' Execute Add Trans Export Folder SQL Statement
            m_lReturn = .SQLAction(sSQL:=ACAddPMBExportFolderSQL, sSQLName:=ACAddPMBExportFolderName, bStoredProcedure:=ACAddPMBExportFolderStored, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Cnt of the record inserted
            r_lTransactionExportFolderCnt = .Parameters.Item("transaction_export_folder_cnt").Value
            If r_lTransactionExportFolderCnt < 1 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        Return result

    End Function

    Private Function CreateAJExportDetails(ByRef v_lTransactionExportFolderCnt As Integer) As Integer

        Dim result As Integer = 0
        'Const kMethodName As String = "AMethod"


        result = gPMConstants.PMEReturnCode.PMTrue

        bPMAddParameter.AddParameterLite(m_oDatabase, "transaction_export_folder_cnt", v_lTransactionExportFolderCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
        bPMAddParameter.AddParameterLite(m_oDatabase, "transaction_type", m_sDebitCredit, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
        bPMAddParameter.AddParameterLite(m_oDatabase, "total_extras_gross", m_cExtrasAmount + m_cExtrasCommission + m_cExtrasIPT, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
        bPMAddParameter.AddParameterLite(m_oDatabase, "total_fees_calc", m_cFeesAmount, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
        bPMAddParameter.AddParameterLite(m_oDatabase, "total_discount_calc", m_cDiscountAmount, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
        bPMAddParameter.AddParameterLite(m_oDatabase, "total_insurer_fee_calc", m_cInsurersFee, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)

        If m_bManualAJ Then
            bPMAddParameter.AddParameterLite(m_oDatabase, "manual_aj", m_cManualAJValue, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
        Else
            bPMAddParameter.AddParameterLite(m_oDatabase, "manual_aj", 0, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
        End If

        bPMAddParameter.AddParameterLite(m_oDatabase, "docref", m_sOriginalDoc, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDoPaidDirectSQL, sSQLName:=ACDoPaidDirectName, bStoredProcedure:=ACDoPaidDirectStored)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oDatabase.SQLAction", "sSQL:=ACDoPaidDirectSQL", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' Do any tidy up, e.g. Set x = Nothing here
        Return result
        ' This is for debugging only
    End Function

    'DC260105 : Introducer Transaction Processing

    ' ***************************************************************** '
    ' Name: CreateIntroducerExportDetails (Private)
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function CreateIntroducerExportDetails(ByRef v_lTransactionExportFolderCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim s As String = ""
        Dim lRecordsAffected As Integer
        Dim sReturnParameter As String = ""

        'TEMPORARY BIT TO SET UP DATA WHICH WILL BE HELD IN THE NEW TABLE


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase
            '        ' Clear the Database Parameters Collection

            m_oDatabase.Parameters.Clear()
            'Add Folder Id for Details Postings
            m_lReturn = .Parameters.Add(sName:="transaction_export_folder_cnt", vValue:=CStr(v_lTransactionExportFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Add Folder Id for Details Postings
            m_lReturn = .Parameters.Add(sName:="transaction_type", vValue:=m_sDebitCredit, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="orig_doc_ref", vValue:=m_sOriginalDoc, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="introducer_cnt", vValue:=CStr(m_lIntroducerCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .SQLAction(sSQL:=ACDoIntroducerSQL, sSQLName:=ACDoIntroducerName, bStoredProcedure:=ACDoIntroducerStored, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        Return result

    End Function

    'EK 160200
    ' ***************************************************************** '
    ' Name: RoundupExportFolder (Private)
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function RoundupExportFolder(ByVal v_lTransactionExportFolderCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim lRecordsAffected As Integer
        Dim vResultArray(,) As Object = Nothing
        Dim lTransactionExportDetailId As Integer
        Dim cTransactionAmount, cTransactionAmountRounded, cRoundedTotal As Decimal


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            ' Clear the Database Parameters Collection
            .Parameters.Clear()

            ' Add ExportFolderCnt as an OUTPUT param
            m_lReturn = .Parameters.Add(sName:="transaction_export_folder_cnt", vValue:=CStr(v_lTransactionExportFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            sSQL = "SELECT transaction_export_detail_id,transaction_amount " & " FROM transaction_export_detail WHERE transaction_export_folder_cnt = {transaction_export_folder_cnt}"

            ' Get the Transactions posted
            m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="GetTransactionValues", bStoredProcedure:=False, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End With

        If Not Informations.IsArray(vResultArray) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        cRoundedTotal = 0

        For lRow As Integer = 0 To vResultArray.GetUpperBound(1)

            lTransactionExportDetailId = CInt(vResultArray(0, lRow))

            cTransactionAmount = CDec(vResultArray(1, lRow))
            cTransactionAmountRounded = gPMMaths.PMRoundupValueVDecimal(v_vWholeValue:=cTransactionAmount, v_eNumberOfDP:=gPMConstants.PMEVDecimalNoOfDP.pmeVDecimalDPTwo, v_eRoundingFactor:=gPMConstants.PMERoundupFactor.pmeRFactor50Up)
            cRoundedTotal += cTransactionAmountRounded
        Next lRow
        'Batch Balances OK
        If cRoundedTotal = 0 Then
            Return result
        End If
        'EK More than 1p is not acceptable
        If cRoundedTotal > 0.01 Or cRoundedTotal < -0.01 Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        'Update the commission entry
        With m_oDatabase

            ' Clear the Database Parameters Collection
            .Parameters.Clear()

            ' Add ExportFolderCnt as
            m_lReturn = .Parameters.Add(sName:="transaction_export_folder_cnt", vValue:=CStr(v_lTransactionExportFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            ' Add Transaction Export Detail Id
            m_lReturn = .Parameters.Add(sName:="transaction_export_detail_id", vValue:=CStr(lTransactionExportDetailId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            ' Add New Transaction Amount
            m_lReturn = .Parameters.Add(sName:="transaction_amount", vValue:=(CStr(cTransactionAmountRounded - cRoundedTotal)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            sSQL = "UPDATE transaction_export_detail SET transaction_amount = {transaction_amount}" & " WHERE transaction_export_folder_cnt = {transaction_export_folder_cnt}" & " AND transaction_export_detail_id = {transaction_export_detail_id}"

            ' Get the Transactions posted
            m_lReturn = .SQLAction(sSQL:=sSQL, sSQLName:="RoundupCommissionTransaction", bStoredProcedure:=False, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            If lRecordsAffected <> 1 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End With

        Return result

    End Function
    Private Function SetPropertiesParameters() As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue



        With m_oDatabase

            ' Add InsuranceFileCnt as an INPUT param for an insert

            m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(m_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'Add Transaction type parameter
            m_lReturn = .Parameters.Add(sName:="last_trans_type_id", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'Add taxAmount as Output Parameter
            m_lReturn = .Parameters.Add(sName:="tax_amount", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMDouble)

            'Add Commission Amount as Output Parameter
            m_lReturn = .Parameters.Add(sName:="commission_amount", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMDouble)

            'Add Agent as Output Parameter
            m_lReturn = .Parameters.Add(sName:="agent_cnt", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
            'EK 16/11/99 Sub Agent
            'Add Agent as Output Parameter
            m_lReturn = .Parameters.Add(sName:="subagent_cnt", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
            'Add coinsurer count as Output Parameter
            m_lReturn = .Parameters.Add(sName:="coinsurers_count", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'Add shares count as Output Parameter
            'TF140802 - Fix to PMLong
            m_lReturn = .Parameters.Add(sName:="policyshares_count", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'Add fees count as Output Parameter
            m_lReturn = .Parameters.Add(sName:="fees_count", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'Add extras count as Output Parameter
            m_lReturn = .Parameters.Add(sName:="extras_count", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'Add discount count as Output Parameter
            m_lReturn = .Parameters.Add(sName:="discount_count", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'DC151204
            m_lReturn = .Parameters.Add(sName:="introducer_cnt", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'EK 30/11/99 Get Paid Direct Indicator
            m_lReturn = .Parameters.Add(sName:="paid_direct", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        Return result

    End Function

    Private Function GetTransactionOutputParameters(ByRef sReturnParameter As String, ByRef cReturnValue As Decimal) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue



        Select Case sReturnParameter
            Case "agent_amount"
                m_cAgentAmount = cReturnValue

            Case "total_fees"
                m_cFeesAmount = cReturnValue

            Case "total_discount"
                m_cDiscountAmount = cReturnValue

            Case "total_extras"
                m_cExtrasAmount = cReturnValue

            Case "total_extras_commission"
                m_cExtrasCommission = cReturnValue

            Case "total_extras_ipt"
                m_cExtrasIPT = cReturnValue

            Case "total_coinsurers"
                m_cCoinsurersAmount = cReturnValue

            Case "total_coinsurers_commission"
                m_cCoinsurersCommission = cReturnValue

            Case "total_coinsurers_ipt"
                m_cCoinsurersIPT = cReturnValue

            Case "total_coinsurers_fee"
                m_cCoinsurersFee = cReturnValue

            Case "total_insurers"
                m_cInsurersAmount = cReturnValue

            Case "total_insurers_commission"
                m_cInsurersCommission = cReturnValue

            Case "total_insurers_ipt"
                m_cInsurersIPT = cReturnValue

            Case "total_insurers_fee"
                m_cInsurersFee = cReturnValue

            Case "subagent_commission"
                m_cSubAgentCommission = cReturnValue

            Case "introducer_amount"
                m_cIntroducerAmount = cReturnValue

        End Select

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GenerateDocumentRef
    '
    ' Description: Wrapper call to the bACTAutoNumber functions
    'EK 220200 Passed Group Code and Range Code
    ' ***************************************************************** '
    '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
    'Note:- r_lNumber is changed with r_sDocumentRef
    Public Function GenerateDocumentRef(ByRef v_sGroupCode As String, ByRef v_sRangeCode As String, ByRef v_iUserID As Integer, ByRef v_iCompanyID As Integer, ByRef r_sDocumentRef As String) As Integer

        Dim result As Integer = 0
        Dim oPMAutoNumber As bACTAutoNumber.Business
        Dim lNumberRangeID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the autonumber object
            oPMAutoNumber = New bACTAutoNumber.Business()

            ' Intialise it
            m_lReturn = (oPMAutoNumber).Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the number range
            'EK 220200 Pass Range and Group Codes
            m_lReturn = oPMAutoNumber.GetNumberRange(v_sGroupCode:=v_sGroupCode, v_sRangeCode:=v_sRangeCode, r_lNumberRangeID:=lNumberRangeID)
            'eck020600 Pass Branch Code
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Generate the next number
                ' Start(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                m_lReturn = oPMAutoNumber.GenerateDocumentReferenceNumber(v_lNumberRangeID:=lNumberRangeID, v_iUserID:=v_iUserID, v_iCompanyID:=v_iCompanyID, r_sDocumentRef:=r_sDocumentRef, v_sRangeCode:=v_sRangeCode)
                'End(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
            End If

            ' terminate the object
            oPMAutoNumber.Dispose()
            oPMAutoNumber = Nothing

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GenerateDocumentRef Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateDocumentRef", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: PoolDocumentRef
    '
    ' Description: Wrapper call to the bACTAutoNumber function
    'EK 290200 Pool unused number
    ' ***************************************************************** '
    Public Function PoolDocumentRef() As Integer

        Dim result As Integer = 0
        Dim oPMAutoNumber As bACTAutoNumber.Business
        Dim lNumberRangeID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the autonumber object
            oPMAutoNumber = New bACTAutoNumber.Business()

            ' Intialise it
            m_lReturn = (oPMAutoNumber).Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the number range
            m_lReturn = oPMAutoNumber.GetNumberRange(v_sGroupCode:=m_sGroupCode, v_sRangeCode:=m_sRangeCode, r_lNumberRangeID:=lNumberRangeID)
            'eck020600
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Generate the next number
                m_lReturn = oPMAutoNumber.PoolNumber(v_lNumberRangeID:=lNumberRangeID, v_iCompanyID:=m_iPolicySourceID, v_iUserID:=m_iUserID, r_lNumber:=m_lDocumentNo)
            End If

            ' terminate the object
            oPMAutoNumber.Dispose()
            oPMAutoNumber = Nothing

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PoolDocumentRef Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PoolDocumentRef", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: RenumberExportTrans
    '
    ' Description: Wrapper call to the bPMAutoNumber functions
    '
    ' ***************************************************************** '
    Public Function RenumberExportTrans(ByRef v_lEventCnt As Integer, ByRef v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            With m_oDatabase

                ' CTAF 020101
                ' Need to clear the parameters
                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="folder_cnt", vValue:=CStr(m_lTransactionExportFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .Parameters.Add(sName:="event_cnt", vValue:=CStr(v_lEventCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .Parameters.Add(sName:="insurancefile_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .SQLAction(sSQL:=ACPMBExportFolderRenumberSQL, sSQLName:=ACPMBExportFolderRenumberName, bStoredProcedure:=ACPMBExportFolderRenumberStored, lRecordsAffected:=lRecordsAffected)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="REnumberExportTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="REnumberExportTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SendToOrion (Public)
    '
    ' Description: Sends a transaction set to the Orion system.
    '
    ' ***************************************************************** '

    Public Function SendToOrion(ByVal v_lTransactionFolderCnt As Integer, Optional ByVal v_bTransferAuthorised As Boolean = False, Optional ByRef r_lDocumentId As Integer = 0, Optional ByRef r_sFailureReason As String = "") As Integer

        Dim result As Integer = 0
        Dim oOrionLink As bSirOrionLink.Form
        Dim lReturnStatus As gPMConstants.PMEReturnCode

        Dim sDocRef As String = String.Empty
        Dim sDocDebitCredit As String = String.Empty
        Dim sDocTransactionTypeCode As String = String.Empty
        Dim dtDocDate, dtDocAccountingDate As Date
        Dim sDocComments As String = String.Empty
        Dim sDocCurrencyCode As String = String.Empty
        Dim sDocBusinessTypeCode As String = String.Empty
        Dim sDocInsuranceRef As String = String.Empty
        Dim sDocProductCode As String = String.Empty
        Dim sDocBranchCode As String = String.Empty
        Dim sDocLeadAgentShortName As String = String.Empty
        Dim sDocInsuranceHolderShortName As String = String.Empty
        Dim dtDocInsuranceEffectiveDate As Date
        Dim iDocOperatorID As Integer
        Dim vTransactionsArray(,) As Object = Nothing
        Dim iDocSourceID, iDocIsPayableByInstalments As Integer
        Dim lDocInsuranceHolderID, lDocAgentID, lAccountKey As Integer

        Dim lDocAgentKey As Integer

        Dim lSalesNodeID, lPurchaseNodeID As Integer

        Dim lInsurerNodeID, lAgentNodeID, lFeeNodeID, lCommissionNodeID, lDiscountNodeID, lPremiumFinanceNodeID As Integer

        Dim lSubAgentNodeId As Integer

        Dim lNominalNodeId As Integer
        Dim lOtherPartyPayNodeId, lOtherPartyRecNodeId As Integer

        Dim lReturn2 As Integer

        Dim lPostingPeriodNumber As Integer

        Dim lInsuranceFileCnt As Integer
        Dim sReason As String = ""

        Dim lAccountStatus As Integer

        Dim vTermsOfPaymentId As Object = Nothing
        Dim vPaymentDueDate As Object = Nothing
        Dim bCashDepositPayment As Boolean
        Dim lCashDepositAccountID As Integer
        Dim bCashDepositAccountStatus As Boolean
        Dim lCashDepositBaseAccountKey As Integer
        Dim sCashDepositBaseAccountAgentType As String = ""

        Dim lIntroducerNodeId As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the transaction data required by Orion
            m_lReturn = GetTransactionData(v_lFolderCnt:=v_lTransactionFolderCnt, r_sDocRef:=sDocRef, r_sDocDebitCredit:=sDocDebitCredit,
                                           r_sDocTransactionTypeCode:=sDocTransactionTypeCode, r_dtDocDate:=dtDocDate, r_dtDocAccountingDate:=dtDocAccountingDate,
                                           r_sDocComments:=sDocComments, r_sDocCurrencyCode:=sDocCurrencyCode, r_sDocBusinessTypeCode:=sDocBusinessTypeCode,
                                           r_sDocInsuranceRef:=sDocInsuranceRef, r_sDocProductCode:=sDocProductCode, r_sDocBranch_Code:=sDocBranchCode,
                                           r_sDocLeadAgentShortName:=sDocLeadAgentShortName, r_sDocInsuranceHolderShortName:=sDocInsuranceHolderShortName,
                                           r_dtDocInsuranceEffectiveDate:=dtDocInsuranceEffectiveDate, r_iDocOperatorID:=iDocOperatorID,
                                           r_vTransactionsArray:=vTransactionsArray, r_iDocSourceID:=iDocSourceID, r_iDocIsPayableByInstalments:=iDocIsPayableByInstalments,
                                           r_lDocInsuranceHolderID:=lDocInsuranceHolderID, r_lDocAgentID:=lDocAgentID, r_lPostingPeriodNumber:=lPostingPeriodNumber,
                                           r_lInsuranceFileCnt:=lInsuranceFileCnt, r_sReason:=sReason, r_lRealInsuranceFileCnt:=m_lRealInsuranceFileCnt,
                                           r_vTermsOfPaymentId:=vTermsOfPaymentId, r_vPaymentDueDate:=vPaymentDueDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue AndAlso m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get Transaction Data", vApp:=ACApp, vClass:=ACClass, vMethod:="SendToOrion", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                result = gPMConstants.PMEReturnCode.PMNotFound
                Return result
            End If

            ' Create an instance of the OrionLink business object




            oOrionLink = New bSirOrionLink.Form
            m_lReturn = oOrionLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise the Sirius/Orion Link", vApp:=ACApp, vClass:=ACClass, vMethod:="SendToOrion", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            m_lReturn = GetCDAccountDetailsForPolicy(v_lInsuranceFileCnt:=lInsuranceFileCnt, v_bCashDepositPayment:=bCashDepositPayment,
                                                     v_lCashDepositAccountId:=lCashDepositAccountID, v_bActiveCashDepsoitAccount:=bCashDepositAccountStatus,
                                                     v_lCashDepositBaseAccountKey:=lCashDepositBaseAccountKey, v_sCashDepositBaseAccountAgentType:=sCashDepositBaseAccountAgentType)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCDAccountDetailsForPolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SendToOrion", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            If bCashDepositPayment Then
                If lCashDepositAccountID = 0 Or lCashDepositBaseAccountKey = 0 Then
                    result = gPMConstants.PMEReturnCode.PMError

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CashDeposit Account Validation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SendToOrion", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result
                Else
                    If Not bCashDepositAccountStatus Then
                        result = gPMConstants.PMEReturnCode.PMError

                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CashDeposit Account Status is Not Active", vApp:=ACApp, vClass:=ACClass, vMethod:="SendToOrion", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                        Return result
                    End If
                End If
            End If

            ' Check transaction type

            'ECK 2/08/99 Loop through the transactions and create accounts for any oher accounts
            ' Get Account IDs for Insurer Postings
            'ECK PN6169
            'If we are multi-branch (structure tree) then we need to have different accounts
            'for each company per party
            'This seems like a good place to create any new accounts we need
            'The details we need are:

            '                       company of the party
            ' m_iPolicySourceId     company of policy
            ' m_iSourceID           current company

            'To fill in gaps when I work out the check
            'There will be two situations:
            'the policy is being raised in the party's own company
            '   Current processing is fine
            'the policy is being raised in another company and will go to the party's
            '  account in this company
            '   Need to check if there is already an account for the client in this company
            '   If so post to it
            '   If not create it then post to it

            For i As Integer = vTransactionsArray.GetLowerBound(1) To vTransactionsArray.GetUpperBound(1)

                If CDbl(vTransactionsArray(ACTBatchConst.ACTTransImportAccountKey, i)) > 0 Then

                    lAccountKey = CInt(vTransactionsArray(ACTBatchConst.ACTTransImportAccountKey, i))

                    If CStr(vTransactionsArray(1, i)) = "SL" Or CStr(vTransactionsArray(1, i)) = "AG" Then
                        If bCashDepositPayment Then
                            If sCashDepositBaseAccountAgentType.Trim().ToLower() <> "intermed" And lAccountKey <> lCashDepositBaseAccountKey Then

                                result = gPMConstants.PMEReturnCode.PMError

                                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CashDeposit Account validation failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SendToOrion", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                                Return result
                            End If
                        End If
                    End If

                    lSalesNodeID = 0
                    lPurchaseNodeID = 0
                    lInsurerNodeID = 0
                    lAgentNodeID = 0
                    lFeeNodeID = 0
                    lCommissionNodeID = 0
                    lDiscountNodeID = 0
                    lPremiumFinanceNodeID = 0
                    lSubAgentNodeId = 0
                    lNominalNodeId = 0
                    lOtherPartyPayNodeId = 0
                    lOtherPartyRecNodeId = 0

                    m_lReturn = oOrionLink.GetAccountIDs(r_lSalesAccountID:=lSalesNodeID, r_lPurchaseAccountID:=lPurchaseNodeID,
                                                         r_lInsurerAccountID:=lInsurerNodeID, r_lAgentAccountID:=lAgentNodeID,
                                                         r_lFeeAccountID:=lFeeNodeID, r_lCommissionAccountID:=lCommissionNodeID,
                                                         r_lDiscountAccountID:=lDiscountNodeID, r_lPremiumFinanceAccountID:=lPremiumFinanceNodeID,
                                                         r_lSubAgentAccountID:=lSubAgentNodeId, r_lIntroducerAccountId:=lIntroducerNodeId,
                                                         r_lNominalAccountID:=lNominalNodeId, r_lOtherPartyPayAccountID:=lOtherPartyPayNodeId,
                                                         r_lOtherPartyRecAccountID:=lOtherPartyRecNodeId, v_vAccountKey:=lAccountKey)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oOrionLink.GetAccountIDs Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="SendToOrion")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Create Purchase Account if not present

                    If lInsurerNodeID > 0 Then
                        'Check Insurer has not been stopped
                        m_lReturn = oOrionLink.GetAccountStatusFromAccountID(v_lAccountID:=lInsurerNodeID, r_lAccountStatus:=lAccountStatus)
                        If lAccountStatus <> ACTConst.ACTAccountStatusActive Then
                            'Account Not Active
                            Return ACTConst.ACInsurerStopped
                        End If
                    End If

                    Select Case vTransactionsArray(1, i)

                        Case "NO"
                            If lNominalNodeId < 1 Then
                                m_lReturn = oOrionLink.CreateOrionAccounts(r_lAccountID:=lNominalNodeId, v_sLedgerFlag:="N", v_lAccountKey:=lAccountKey)
                            End If

                            vTransactionsArray(ACTBatchConst.ACTTransImportAccountID, i) = lNominalNodeId
                        Case "PL"
                            If lPurchaseNodeID < 1 Then

                                m_lReturn = oOrionLink.CreateOrionAccounts(r_lAccountID:=lPurchaseNodeID, v_sLedgerFlag:=gSIRLibrary.SIRACTPurchaseLedgerShortName, v_lAccountKey:=lAccountKey)
                            End If

                            vTransactionsArray(ACTBatchConst.ACTTransImportAccountID, i) = lPurchaseNodeID
                        Case "SL"

                            If bCashDepositPayment Then
                                If sCashDepositBaseAccountAgentType.Trim().ToLower() <> "intermed" Then
                                    lSalesNodeID = lCashDepositAccountID
                                Else
                                    If lAccountKey = lCashDepositBaseAccountKey Then
                                        lSalesNodeID = lCashDepositAccountID
                                    End If
                                End If
                            End If

                            If lSalesNodeID < 1 Then

                                m_lReturn = oOrionLink.CreateOrionAccounts(r_lAccountID:=lSalesNodeID, v_sLedgerFlag:=gSIRLibrary.SIRACTSalesLedgerShortName, v_lAccountKey:=lAccountKey)
                            End If

                            vTransactionsArray(ACTBatchConst.ACTTransImportAccountID, i) = lSalesNodeID

                        Case "IN"
                            If lInsurerNodeID < 1 Then

                                m_lReturn = oOrionLink.CreateOrionAccounts(r_lAccountID:=lInsurerNodeID, v_sLedgerFlag:="I", v_lAccountKey:=lAccountKey)
                            End If

                            vTransactionsArray(ACTBatchConst.ACTTransImportAccountID, i) = lInsurerNodeID
                        Case "AG"

                            If bCashDepositPayment Then
                                If sCashDepositBaseAccountAgentType.Trim().ToLower() <> "intermed" Then
                                    lAgentNodeID = lCashDepositAccountID
                                Else
                                    If lAccountKey = lCashDepositBaseAccountKey Then
                                        lAgentNodeID = lCashDepositAccountID
                                    End If
                                End If
                            End If

                            If lAgentNodeID < 1 Then
                                m_lReturn = oOrionLink.CreateOrionAccounts(r_lAccountID:=lAgentNodeID, v_sLedgerFlag:="A", v_lAccountKey:=lAccountKey)
                            End If

                            vTransactionsArray(ACTBatchConst.ACTTransImportAccountID, i) = lAgentNodeID
                        Case "FE"
                            If lFeeNodeID < 1 Then
                                m_lReturn = oOrionLink.CreateOrionAccounts(r_lAccountID:=lFeeNodeID, v_sLedgerFlag:="F", v_lAccountKey:=lAccountKey)
                            End If

                            vTransactionsArray(ACTBatchConst.ACTTransImportAccountID, i) = lFeeNodeID
                        Case "CO"
                            If lCommissionNodeID < 1 Then
                                m_lReturn = oOrionLink.CreateOrionAccounts(r_lAccountID:=lCommissionNodeID, v_sLedgerFlag:="C", v_lAccountKey:=lAccountKey)
                            End If

                            vTransactionsArray(ACTBatchConst.ACTTransImportAccountID, i) = lCommissionNodeID
                        Case "DI"
                            If lDiscountNodeID < 1 Then
                                m_lReturn = oOrionLink.CreateOrionAccounts(r_lAccountID:=lDiscountNodeID, v_sLedgerFlag:="D", v_lAccountKey:=lAccountKey)
                            End If

                            vTransactionsArray(ACTBatchConst.ACTTransImportAccountID, i) = lDiscountNodeID
                        Case "PF"
                            If lPremiumFinanceNodeID < 1 Then
                                m_lReturn = oOrionLink.CreateOrionAccounts(r_lAccountID:=lPremiumFinanceNodeID, v_sLedgerFlag:="R", v_lAccountKey:=lAccountKey)
                            End If

                            vTransactionsArray(ACTBatchConst.ACTTransImportAccountID, i) = lPremiumFinanceNodeID

                        Case "SB", "UB"
                            If lSubAgentNodeId < 1 Then

                                m_lReturn = oOrionLink.CreateOrionAccounts(r_lAccountID:=lSubAgentNodeId, v_sLedgerFlag:="U", v_lAccountKey:=lAccountKey)
                            End If

                            vTransactionsArray(ACTBatchConst.ACTTransImportAccountID, i) = lSubAgentNodeId

                        Case "OP"
                            If lOtherPartyPayNodeId < 1 Then

                                m_lReturn = oOrionLink.CreateOrionAccounts(r_lAccountID:=lOtherPartyPayNodeId, v_sLedgerFlag:="OP", v_lAccountKey:=lAccountKey)
                            End If

                            vTransactionsArray(ACTBatchConst.ACTTransImportAccountID, i) = lOtherPartyPayNodeId

                        Case "OR"
                            If lOtherPartyRecNodeId < 1 Then

                                m_lReturn = oOrionLink.CreateOrionAccounts(r_lAccountID:=lOtherPartyRecNodeId, v_sLedgerFlag:="OR", v_lAccountKey:=lAccountKey)
                            End If

                            vTransactionsArray(ACTBatchConst.ACTTransImportAccountID, i) = lOtherPartyRecNodeId
                        Case "TR"
                            If lIntroducerNodeId < 1 Then

                                m_lReturn = oOrionLink.CreateOrionAccounts(r_lAccountID:=lIntroducerNodeId, v_sLedgerFlag:="T", v_lAccountKey:=lAccountKey)
                            End If

                            vTransactionsArray(ACTBatchConst.ACTTransImportAccountID, i) = lIntroducerNodeId

                    End Select

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oOrionLink.CreateOrionAccounts Failed for account " & lAccountKey & "ledger type " & CStr(vTransactionsArray(1, i)), vApp:=ACApp, vClass:=ACClass, vMethod:="SendToOrion")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If
            Next i

            ' Set the transaction status to 'sent'
            m_lReturn = oOrionLink.UpdateAccExportStatus(v_lTransactionFolderCnt:=v_lTransactionFolderCnt, v_sAccountsExportStatus:=gSIRLibrary.SIRAccExportStatusSent)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Update the Export Status", vApp:=ACApp, vClass:=ACClass, vMethod:="SendToOrion")

                Return result
            End If

            oOrionLink.PostingPeriodNumber = lPostingPeriodNumber
            oOrionLink.TransactionExportFolderCnt = v_lTransactionFolderCnt

            ' m_lRealInsuranceFileCnt is not being set by underwriting, so need to
            ' use the InsuranceFileCnt that comes from
            ' the Trans_Exp_Folder. This is correct for underwriting
            If m_lRealInsuranceFileCnt = 0 Then
                m_lRealInsuranceFileCnt = lInsuranceFileCnt
            End If

            'PN37017 Set for post commission when premium is zero and earned commission is when client pays
            oOrionLink.IsPostCommission = m_bIsContra

            m_lReturn = oOrionLink.PostDocument(v_sDocRef:=sDocRef, v_sDocDebitCredit:=sDocDebitCredit, v_sDocTransactionTypeCode:=sDocTransactionTypeCode,
                                                v_dtDocDate:=dtDocDate, v_dtDocAccountingDate:=dtDocAccountingDate, v_sDocComments:=sDocComments,
                                                v_sDocCurrencyCode:=sDocCurrencyCode, v_sDocBusinessTypeCode:=sDocBusinessTypeCode,
                                                v_sDocInsuranceRef:=sDocInsuranceRef, v_sDocProductCode:=sDocProductCode, v_sDocBranchCode:=sDocBranchCode,
                                                v_sDocLeadAgentShortName:=sDocLeadAgentShortName, v_sDocInsuranceHolderShortName:=sDocInsuranceHolderShortName,
                                                v_dtDocInsuranceEffectiveDate:=dtDocInsuranceEffectiveDate, v_iDocOperatorID:=iDocOperatorID,
                                                v_vTransactionsArray:=vTransactionsArray, v_iDocIsPayableByInstalments:=iDocIsPayableByInstalments,
                                                v_lDocInsuranceHolderKey:=lDocInsuranceHolderID, v_lDocAgentKey:=lDocAgentKey, r_lDocPostedStatus:=lReturnStatus,
                                                v_iDocSourceID:=iDocSourceID, v_lInsuranceFileCnt:=m_lRealInsuranceFileCnt, v_sReason:=sReason,
                                                r_vNewDocumentId:=r_lDocumentId, v_vTermsOfPaymentId:=vTermsOfPaymentId, v_vPaymentDueDate:=vPaymentDueDate, r_sFailureReason:=r_sFailureReason)

            lReturn2 = m_lReturn

            Select Case lReturnStatus
                Case gPMConstants.PMEReturnCode.PMSucceed
                    m_lReturn = oOrionLink.UpdateAccExportStatus(v_lTransactionFolderCnt:=v_lTransactionFolderCnt, v_sAccountsExportStatus:=gSIRLibrary.SIRAccExportStatusCompleted)
                Case Else
                    m_lReturn = oOrionLink.UpdateAccExportStatus(v_lTransactionFolderCnt:=v_lTransactionFolderCnt, v_sAccountsExportStatus:=gSIRLibrary.SIRAccExportStatusFailed)
                    result = gPMConstants.PMEReturnCode.PMError
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Orion Failed to Post the Document", vApp:=ACApp, vClass:=ACClass, vMethod:="SendToOrion")
            End Select

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Update the Export Status", vApp:=ACApp, vClass:=ACClass, vMethod:="SendToOrion")
                Return result
            End If

            If lReturn2 <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn2
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Send Transaction To Orion", vApp:=ACApp, vClass:=ACClass, vMethod:="SendToOrion", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function
    ''' <summary>
    ''' GetTransactionData
    ''' </summary>
    ''' <param name="v_lFolderCnt"></param>
    ''' <param name="r_sDocRef"></param>
    ''' <param name="r_sDocDebitCredit"></param>
    ''' <param name="r_sDocTransactionTypeCode"></param>
    ''' <param name="r_dtDocDate"></param>
    ''' <param name="r_dtDocAccountingDate"></param>
    ''' <param name="r_sDocComments"></param>
    ''' <param name="r_sDocCurrencyCode"></param>
    ''' <param name="r_sDocBusinessTypeCode"></param>
    ''' <param name="r_sDocInsuranceRef"></param>
    ''' <param name="r_sDocProductCode"></param>
    ''' <param name="r_sDocBranch_Code"></param>
    ''' <param name="r_sDocLeadAgentShortName"></param>
    ''' <param name="r_sDocInsuranceHolderShortName"></param>
    ''' <param name="r_dtDocInsuranceEffectiveDate"></param>
    ''' <param name="r_iDocOperatorID"></param>
    ''' <param name="r_vTransactionsArray"></param>
    ''' <param name="r_iDocSourceID"></param>
    ''' <param name="r_iDocIsPayableByInstalments"></param>
    ''' <param name="r_lDocInsuranceHolderID"></param>
    ''' <param name="r_lDocAgentID"></param>
    ''' <param name="r_lPostingPeriodNumber"></param>
    ''' <param name="r_lInsuranceFileCnt"></param>
    ''' <param name="r_sReason"></param>
    ''' <param name="r_lRealInsuranceFileCnt"></param>
    ''' <param name="r_vTermsOfPaymentId"></param>
    ''' <param name="r_vPaymentDueDate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetTransactionData(ByVal v_lFolderCnt As Integer, ByRef r_sDocRef As String, ByRef r_sDocDebitCredit As String, ByRef r_sDocTransactionTypeCode As String, ByRef r_dtDocDate As Date, ByRef r_dtDocAccountingDate As Date, ByRef r_sDocComments As String, ByRef r_sDocCurrencyCode As String, ByRef r_sDocBusinessTypeCode As String, ByRef r_sDocInsuranceRef As String, ByRef r_sDocProductCode As String, ByRef r_sDocBranch_Code As String, ByRef r_sDocLeadAgentShortName As String, ByRef r_sDocInsuranceHolderShortName As String, ByRef r_dtDocInsuranceEffectiveDate As Date, ByRef r_iDocOperatorID As Integer, ByRef r_vTransactionsArray(,) As Object, ByRef r_iDocSourceID As Integer, ByRef r_iDocIsPayableByInstalments As Integer, ByRef r_lDocInsuranceHolderID As Integer, ByRef r_lDocAgentID As Integer, ByRef r_lPostingPeriodNumber As Integer, ByRef r_lInsuranceFileCnt As Integer, ByRef r_sReason As String, ByRef r_lRealInsuranceFileCnt As Integer, ByRef r_vTermsOfPaymentId As Object, ByRef r_vPaymentDueDate As Object) As Integer

        Dim nResult As Integer
        Dim nRecordCount As Integer
        Dim oExportControl As bSirExportControl.Business
        Dim sMappingCode As String
        Dim sString As String
        Dim sReturnString As String = String.Empty
        Dim oACTLedger As bACTLedger.Form
        Dim nNodeID As Integer
        Dim oTempResultsSubset As Object



        nResult = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            ' Clear the Database Parameters Collection
            .Parameters.Clear()

            ' Add the folder_cnt INPUT parameter
            m_lReturn = .Parameters.Add(sName:="transaction_export_folder_cnt", vValue:=CStr(v_lFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            ' Execute Select All Transaction Sets SQL Statement
            m_lReturn = .SQLSelect(sSQL:=ACSelectTransDataSQL, sSQLName:=ACSelectTransDataName, bStoredProcedure:=ACSelectTransDataStored)

            ' Database error encountered
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMError
            End If

            ' Set return value
            nRecordCount = .Records.Count()
            ' Record Count includes Doc and Transactions so we need at least 1 of each
            If nRecordCount < 1 Then
                ' No enough rows retreived
                nResult = gPMConstants.PMEReturnCode.PMNotFound
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Stored Procedure '" & ACSelectTransDataSQL & "' returned 0 records, cannot continue", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTransactionData", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return nResult
            Else
                ' Rows retrieved successfully
                nResult = gPMConstants.PMEReturnCode.PMTrue
            End If


            oExportControl = New bSirExportControl.Business
            m_lReturn = oExportControl.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMError

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Create bSirExportControl.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTransactionData", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return nResult
            End If

            ' Create an instance of the Orion Ledger business object

            oACTLedger = New bACTLedger.Form
            m_lReturn = oACTLedger.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMError

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Create bACTLedger.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTransactionData", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return nResult
            End If

            ' Get Header fields
            With m_oDatabase.Records.Item(0).Fields
                r_sDocRef = gPMFunctions.NullToString(m_oDatabase.Records.Item(0).Fields()("document_ref")).Trim()
                r_sDocDebitCredit = gPMFunctions.NullToString(m_oDatabase.Records.Item(0).Fields("debit_credit")).Trim()
                r_sDocTransactionTypeCode = gPMFunctions.NullToString(m_oDatabase.Records.Item(0).Fields("transaction_type_code")).Trim()
                r_dtDocDate = m_oDatabase.Records.Item(0).Fields("document_date")
                r_dtDocAccountingDate = m_oDatabase.Records.Item(0).Fields("accounting_date")
                r_sDocComments = gPMFunctions.NullToString(m_oDatabase.Records.Item(0).Fields("document_comment")).Trim()
                r_sDocCurrencyCode = gPMFunctions.NullToString(m_oDatabase.Records.Item(0).Fields("currency_code")).Trim()
                r_sDocBusinessTypeCode = gPMFunctions.NullToString(m_oDatabase.Records.Item(0).Fields("business_type_code")).Trim()
                r_sDocInsuranceRef = gPMFunctions.NullToString(m_oDatabase.Records.Item(0).Fields("insurance_ref")).Trim()
                r_sDocProductCode = gPMFunctions.NullToString(m_oDatabase.Records.Item(0).Fields("product_code")).Trim()
                r_sDocBranch_Code = gPMFunctions.NullToString(m_oDatabase.Records.Item(0).Fields("branch_code")).Trim()
                r_sDocLeadAgentShortName = gPMFunctions.NullToString(m_oDatabase.Records.Item(0).Fields("agent_shortname")).Trim()
                r_sDocInsuranceHolderShortName = gPMFunctions.NullToString(m_oDatabase.Records.Item(0).Fields("insurance_holder_shortname")).Trim()
                r_dtDocInsuranceEffectiveDate = m_oDatabase.Records.Item(0).Fields("effective_date")
                r_iDocOperatorID = gPMFunctions.NullToInteger(m_oDatabase.Records.Item(0).Fields("created_by_user_id"))
                r_iDocSourceID = gPMFunctions.NullToInteger(m_oDatabase.Records.Item(0).Fields("source_id"))
                r_iDocIsPayableByInstalments = gPMFunctions.NullToInteger(m_oDatabase.Records.Item(0).Fields("is_payable_by_instalments"))
                r_lDocInsuranceHolderID = gPMFunctions.NullToLong(m_oDatabase.Records.Item(0).Fields("insurance_holder_id"))
                r_lDocAgentID = gPMFunctions.NullToLong(m_oDatabase.Records.Item(0).Fields("agent_id"))
                r_lPostingPeriodNumber = gPMFunctions.NullToLong(m_oDatabase.Records.Item(0).Fields("posting_period_number"))
                r_lInsuranceFileCnt = gPMFunctions.NullToLong(m_oDatabase.Records.Item(0).Fields("insurance_file_cnt"))
                r_sReason = gPMFunctions.NullToString(m_oDatabase.Records.Item(0).Fields("reason")).Trim()
                r_lRealInsuranceFileCnt = gPMFunctions.NullToLong(m_oDatabase.Records.Item(0).Fields("real_insurance_file_cnt"))

                r_vTermsOfPaymentId = gPMFunctions.NullToLong(m_oDatabase.Records.Item(0).Fields("terms_of_payment_id"))

                r_vPaymentDueDate = gPMFunctions.NullToDate(m_oDatabase.Records.Item(0).Fields("payment_due_date"))

            End With

            ' Array from zero
            ReDim r_vTransactionsArray(ACTBatchConst.ACTTransImportArraySize, nRecordCount - 1)
            ReDim oTempResultsSubset(ACTBatchConst.ACCTTempResultsLastItem, nRecordCount - 1)

            'copy results subset into array as it will be trashed by the call to ORION
            For iRow As Integer = 1 To nRecordCount

                With m_oDatabase.Records.Item(iRow - 1).Fields

                    oTempResultsSubset(ACTBatchConst.ACCTTempResultsAccountTypeCode, iRow - 1) = m_oDatabase.Records.Item(iRow - 1).Fields()("account_type_code")

                    oTempResultsSubset(ACTBatchConst.ACCTTempResultsTransactionLedgerCode, iRow - 1) = m_oDatabase.Records.Item(iRow - 1).Fields("transaction_ledger_code")

                    oTempResultsSubset(ACTBatchConst.ACCTTempResultsTransactionAmount, iRow - 1) = m_oDatabase.Records.Item(iRow - 1).Fields("transaction_amount")

                    oTempResultsSubset(ACTBatchConst.ACCTTempResultsMapping_code, iRow - 1) = m_oDatabase.Records.Item(iRow - 1).Fields("mapping_code")

                    oTempResultsSubset(ACTBatchConst.ACCTTempResultsTransactionAccountKey, iRow - 1) = m_oDatabase.Records.Item(iRow - 1).Fields("transaction_account_key")

                    oTempResultsSubset(ACTBatchConst.ACCTTempResultsSpare, iRow - 1) = m_oDatabase.Records.Item(iRow - 1).Fields("spare")

                    oTempResultsSubset(ACTBatchConst.ACCTTempResultsTaxesTotal, iRow - 1) = m_oDatabase.Records.Item(iRow - 1).Fields("taxes_total")

                    oTempResultsSubset(ACTBatchConst.ACCTTempResultsChargesTotal, iRow - 1) = m_oDatabase.Records.Item(iRow - 1).Fields("charges_total")

                    oTempResultsSubset(ACTBatchConst.ACCTTempResultsPurchaseOrderNo, iRow - 1) = m_oDatabase.Records.Item(iRow - 1).Fields("purchase_order_no")

                    oTempResultsSubset(ACTBatchConst.ACCTTempResultsPurchaseInvoiceNo, iRow - 1) = m_oDatabase.Records.Item(iRow - 1).Fields("purchase_invoice_no")

                    oTempResultsSubset(ACTBatchConst.ACCTTempResultsUWYearID, iRow - 1) = m_oDatabase.Records.Item(iRow - 1).Fields("underwriting_year_id")

                    oTempResultsSubset(ACTBatchConst.ACCTTempResultsSuspended, iRow - 1) = m_oDatabase.Records.Item(iRow - 1).Fields("suspended")

                    oTempResultsSubset(ACTBatchConst.ACCTTempResultsReleaseToIncome, iRow - 1) = m_oDatabase.Records.Item(iRow - 1).Fields("release_to_income")

                    oTempResultsSubset(ACTBatchConst.ACCTTempResultsReleaseAccountCode, iRow - 1) = m_oDatabase.Records.Item(iRow - 1).Fields("release_account_code")

                    oTempResultsSubset(ACTBatchConst.ACCTTempResultsTransdetailTypeCode, iRow - 1) = m_oDatabase.Records.Item(iRow - 1).Fields("transdetail_type_code")

                    oTempResultsSubset(ACTBatchConst.ACCTTempResultsTaxGroupID, iRow - 1) = m_oDatabase.Records.Item(iRow - 1).Fields("tax_group_id")

                    oTempResultsSubset(ACTBatchConst.ACCTTempResultsTaxBandID, iRow - 1) = m_oDatabase.Records.Item(iRow - 1).Fields("tax_band_id")

                    oTempResultsSubset(ACTBatchConst.ACCTTempResultsManuallyReleased, iRow - 1) = m_oDatabase.Records.Item(iRow - 1).Fields("manually_released")

                    oTempResultsSubset(ACTBatchConst.ACCTTempResultsReleasedOnFullSettlement, iRow - 1) = m_oDatabase.Records.Item(iRow - 1).Fields("released_on_full_settlement")

                    oTempResultsSubset(ACTBatchConst.ACCTTempResultsReleasedForWholePosting, iRow - 1) = m_oDatabase.Records.Item(iRow - 1).Fields("released_for_whole_posting")

                    oTempResultsSubset(ACTBatchConst.ACCTTempResultsReleasedOnPolicyEffective, iRow - 1) = m_oDatabase.Records.Item(iRow - 1).Fields("released_on_policy_effective")
                    oTempResultsSubset(ACTBatchConst.kCTTempResultsFeeType, iRow - 1) = m_oDatabase.Records.Item(iRow - 1).Fields("fee_type")
                End With

            Next

            For iRow As Integer = 1 To nRecordCount
                ' Build details from mapping tables
                sMappingCode = gPMFunctions.NullToString(oTempResultsSubset(ACTBatchConst.ACCTTempResultsAccountTypeCode, iRow - 1))

                ' Call a function in Export Control to build record split required info from returned record
                m_lReturn = oExportControl.GetRecordFromMapping(v_sModelCode:="ORION", v_sDetailCode:=sMappingCode, v_sSourceTableName:="Transaction_Export_Folder", r_sReturnString:=sReturnString)

                ' Set Orion Mapping code
                sString = Mid(sReturnString, (sReturnString.IndexOf("\"c) + 1) + 1)

                m_lReturn = oACTLedger.GetLedgerNodeId(sString, nNodeID)

                r_vTransactionsArray(ACTBatchConst.ACTTransImportParentNode, iRow - 1) = nNodeID

                ' Set Orion Ledger Code

                r_vTransactionsArray(ACTBatchConst.ACTTransImportLedgerCode, iRow - 1) = gPMFunctions.NullToString(oTempResultsSubset(ACTBatchConst.ACCTTempResultsTransactionLedgerCode, iRow - 1)).Trim()

                ' Set Orion Account Type
                If sReturnString = "" Then

                Else
                    sString = Mid(sReturnString, 1, sReturnString.IndexOf("\"c))
                End If
                r_vTransactionsArray(ACTBatchConst.ACTTransImportAccountTypeCode, iRow - 1) = sString.Trim()

                r_vTransactionsArray(ACTBatchConst.ACTTransImportCurrencyAmount, iRow - 1) = gPMFunctions.NullToDecimal(oTempResultsSubset(ACTBatchConst.ACCTTempResultsTransactionAmount, iRow - 1))
                'Currently no individual trans comments passed

                r_vTransactionsArray(ACTBatchConst.ACTTransImportDescription, iRow - 1) = ""

                If CStr(r_vTransactionsArray(ACTBatchConst.ACTTransImportLedgerCode, iRow - 1)).Substring(0, 1) <> gSIRLibrary.SIRACTSalesLedgerShortName And CStr(r_vTransactionsArray(ACTBatchConst.ACTTransImportLedgerCode, iRow - 1)).Substring(0, 1) <> gSIRLibrary.SIRACTPurchaseLedgerShortName Then

                    r_vTransactionsArray(ACTBatchConst.ACTTransImportRelativeCode, iRow - 1) = gPMFunctions.NullToString(oTempResultsSubset(ACTBatchConst.ACCTTempResultsMapping_code, iRow - 1)).Trim()
                Else

                    r_vTransactionsArray(ACTBatchConst.ACTTransImportRelativeCode, iRow - 1) = gPMFunctions.NullToString(oTempResultsSubset(ACTBatchConst.ACCTTempResultsMapping_code, iRow - 1)).Trim()
                End If

                ' Key by Account Code if code is present

                r_vTransactionsArray(ACTBatchConst.ACTTransImportAccountKey, iRow - 1) = gPMFunctions.NullToLong(oTempResultsSubset(ACTBatchConst.ACCTTempResultsTransactionAccountKey, iRow - 1))

                r_vTransactionsArray(ACTBatchConst.ACTTransImportSpare, iRow - 1) = gPMFunctions.NullToString(oTempResultsSubset(ACTBatchConst.ACCTTempResultsSpare, iRow - 1))

                r_vTransactionsArray(ACTBatchConst.ACTTransImportIPTTotal, iRow - 1) = gPMFunctions.NullToDecimal(oTempResultsSubset(ACTBatchConst.ACCTTempResultsTaxesTotal, iRow - 1))

                r_vTransactionsArray(ACTBatchConst.ACTTransImportVATTotal, iRow - 1) = gPMFunctions.NullToDecimal(oTempResultsSubset(ACTBatchConst.ACCTTempResultsChargesTotal, iRow - 1))

                r_vTransactionsArray(ACTBatchConst.ACTTransImportPurchaseOrderNo, iRow - 1) = gPMFunctions.NullToString(oTempResultsSubset(ACTBatchConst.ACCTTempResultsPurchaseOrderNo, iRow - 1))

                r_vTransactionsArray(ACTBatchConst.ACTTransImportPurchaseInvoiceNo, iRow - 1) = gPMFunctions.NullToString(oTempResultsSubset(ACTBatchConst.ACCTTempResultsPurchaseInvoiceNo, iRow - 1))

                r_vTransactionsArray(ACTBatchConst.ACTTransImportUWYearID, iRow - 1) = gPMFunctions.NullToString(oTempResultsSubset(ACTBatchConst.ACCTTempResultsUWYearID, iRow - 1))

                r_vTransactionsArray(ACTBatchConst.ACTTransImportSuspended, iRow - 1) = gPMFunctions.NullToInteger(oTempResultsSubset(ACTBatchConst.ACCTTempResultsSuspended, iRow - 1))

                r_vTransactionsArray(ACTBatchConst.ACTTransImportReleaseToIncome, iRow - 1) = gPMFunctions.NullToInteger(oTempResultsSubset(ACTBatchConst.ACCTTempResultsReleaseToIncome, iRow - 1))

                r_vTransactionsArray(ACTBatchConst.ACTTransImportReleaseAccountCode, iRow - 1) = gPMFunctions.NullToString(oTempResultsSubset(ACTBatchConst.ACCTTempResultsReleaseAccountCode, iRow - 1))

                r_vTransactionsArray(ACTBatchConst.ACTTransImportTransdetailTypeCode, iRow - 1) = gPMFunctions.NullToString(oTempResultsSubset(ACTBatchConst.ACCTTempResultsTransdetailTypeCode, iRow - 1))

                r_vTransactionsArray(ACTBatchConst.ACTTransImportTaxGroupID, iRow - 1) = gPMFunctions.ToSafeLong(oTempResultsSubset(ACTBatchConst.ACCTTempResultsTaxGroupID, iRow - 1))

                r_vTransactionsArray(ACTBatchConst.ACTTransImportTaxBandID, iRow - 1) = gPMFunctions.ToSafeLong(oTempResultsSubset(ACTBatchConst.ACCTTempResultsTaxBandID, iRow - 1))

                r_vTransactionsArray(ACTBatchConst.ACTTransImportManuallyReleased, iRow - 1) = gPMFunctions.ToSafeLong(oTempResultsSubset(ACTBatchConst.ACCTTempResultsManuallyReleased, iRow - 1))

                r_vTransactionsArray(ACTBatchConst.ACTTransImportReleasedOnFullSettlement, iRow - 1) = gPMFunctions.ToSafeLong(oTempResultsSubset(ACTBatchConst.ACCTTempResultsReleasedOnFullSettlement, iRow - 1))

                r_vTransactionsArray(ACTBatchConst.ACTTransImportReleasedForWholePosting, iRow - 1) = gPMFunctions.ToSafeLong(oTempResultsSubset(ACTBatchConst.ACCTTempResultsReleasedForWholePosting, iRow - 1))

                r_vTransactionsArray(ACTBatchConst.ACTTransImportReleasedOnPolicyEffective, iRow - 1) = gPMFunctions.ToSafeLong(oTempResultsSubset(ACTBatchConst.ACCTTempResultsReleasedOnPolicyEffective, iRow - 1))

                r_vTransactionsArray(ACTBatchConst.kTTransImportFeeType, iRow - 1) = gPMFunctions.NullToString(oTempResultsSubset(ACTBatchConst.kCTTempResultsFeeType, iRow - 1))

            Next iRow

        End With

        Return nResult

    End Function

    ' ***************************************************************** '
    '
    ' Name: AccountsTransactionQueueAdd
    '
    ' Description:
    '
    ' History: 05/07/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function AccountsTransactionQueueAdd(ByVal r_lTransactionExportFolderCnt As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Const kAddAccountsPartyQueueSQL As String = "spu_accounts_transaction_queue_add"
        Const kAddAccountsPartyQueueName As String = "AddAccountsPartyQueue"
        With m_oDatabase

            .Parameters.Clear()

            'transaction_export_folder_cnt
            m_lReturn = .Parameters.Add(sName:="transaction_export_folder_cnt", vValue:=CStr(r_lTransactionExportFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Create_date
            m_lReturn = .Parameters.Add(sName:="create_date", vValue:=Informations.FormatDateTime(DateTime.Now), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'commit_ind
            m_lReturn = .Parameters.Add(sName:="commit_ind", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'commit_date

            ' Developer Guide No. 85
            m_lReturn = .Parameters.Add(sName:="commit_date", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .SQLAction(sSQL:=kAddAccountsPartyQueueSQL, sSQLName:=kAddAccountsPartyQueueName, bStoredProcedure:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CheckBeforePosting
    '
    ' Description: Use to check known reasons why transaction fails to post
    '
    ' DC011104 PN14916
    ' ***************************************************************** '
    Public Function CheckBeforePosting(ByVal v_lInsuranceFileCnt As Integer, ByVal v_dtEffectiveDate As Date, ByRef r_sChecks As String) As Integer

        Dim result As Integer = 0
        Dim vCriteriaArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Parameters.Add(sName:="effective_date", vValue:=Informations.FormatDateTime(v_dtEffectiveDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .SQLSelect(sSQL:=ACGetPostingCriteriaSQL, sSQLName:=ACGetPostingCriteriaName, bStoredProcedure:=ACGetPostingCriteriaStored, vResultArray:=vCriteriaArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the posting criteria ", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckBeforePosting", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result

                End If

            End With

            If Informations.IsArray(vCriteriaArray) Then

                r_sChecks = CStr(vCriteriaArray(0, 0))
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckBeforePosting Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckBeforePosting", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: GetTermsOfPayment
    '
    ' Description:
    '
    ' History: 02/05/2006 Deepak - Created.
    '
    ' ***************************************************************** '
    Public Function GetTermsOfPayment(ByVal v_lTermsOfPaymentId As Integer, ByRef r_vResult(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim lNumberOfRecords As Integer

            lNumberOfRecords = 0

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="terms_of_payment_id", vValue:=CStr(v_lTermsOfPaymentId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .SQLSelect(sSQL:=ACGetTermsOfPaymentSQL, sSQLName:=ACGetTermsOfPaymentName, bStoredProcedure:=ACGetTermsOfPaymentStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResult)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed to GetTermsOfPayment", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTermsOfPayment")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTermsOfPayment Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTermsOfPayment", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Sub New()
        MyBase.New()

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Informations: http://www.vbtonet.com/ewis/ewi7001.aspx
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
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    ' Name          : AddCreditControlItem (Private)
    '
    ' Description   : Method to add an entry in the Credit Control Item Table
    '                   based for the supplied
    '                   a) insurance_file_cnt
    '                   b) business_type        (NB / MTA / REN / TRANS)
    '
    ' Note          : Uses the 'spu_ACT_Add_Credit_Control_Item_InsFile' stored proc
    '
    ' Reference     : Credit Control
    '
    ' Author        : Vijay Bhushan
    '
    ' Date          : 2006-09-18
    '
    ' Edit History  :
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (AddCreditControlItem) seems to be dead code More Informations: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function AddCreditControlItem(ByVal v_lInsuranceFileCnt As Integer, ByVal v_sBusinessType As String) As Integer
    '
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'With m_oDatabase
    '
    '.Parameters.Clear()
    '
    'result = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
    '
    'If result <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return result
    'End If
    '
    'result = .Parameters.Add(sName:="business_type", vValue:=v_sBusinessType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
    '
    'If result <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return result
    'End If
    '
    'result = .SQLAction(sSQL:=ACAddCreditControlItemInsuranceFileSQL, sSQLName:=ACAddCreditControlItemInsuranceFileName, bStoredProcedure:=ACAddCreditControlItemInsuranceFileStored)
    '
    'End With
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
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddCreditControlItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddCreditControlItem", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: Get Payment Methods
    '
    ' Description: get all payment methods
    '
    ' History : 25/09/2006 (Created)
    '
    ' ***************************************************************** '
    Public Function GetPaymentMethod(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = ""
            sSQL = sSQL & "SELECT" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    payment_method_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    description," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    direct_to_insurer" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "FROM payment_method" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE is_deleted = 0" & Strings.ChrW(13) & Strings.ChrW(10)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetPaymentMethod", bStoredProcedure:=False, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' No values, so return not found
            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' Return the array

            r_vResultArray = vResultArray

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPaymentMethod Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPaymentMethod", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateProspectStatus (Public)
    '
    ' Description: Update status of prospect to client if .
    '
    ' ***************************************************************** '
    Public Function UpdateProspectStatus(ByRef v_lPartyCnt As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateProspectStatus"

        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "UPDATE party SET is_prospect=0 WHERE party_cnt=" & v_lPartyCnt & " AND is_prospect=1"

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="ACUpdateProspect", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update prospect status.", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName)

                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If
            Return result

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally

        End Try
        Return result
    End Function

    Public Function GetCDAccountDetailsForPolicy(ByVal v_lInsuranceFileCnt As Integer, ByRef v_bCashDepositPayment As Boolean, ByRef v_lCashDepositAccountId As Integer, ByRef v_bActiveCashDepsoitAccount As Boolean, ByRef v_lCashDepositBaseAccountKey As Integer, ByRef v_sCashDepositBaseAccountAgentType As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetCDAccountDetailsForPolicy"

        Const kBalanceTypeCol As Integer = 0
        Const kCashDepositAccountIDCol As Integer = 1
        Const kCashDepositBaseAccountKeyCol As Integer = 2
        Const kCashDepositAccountStatusCol As Integer = 3
        Const kCashDepositBaseAccountAgentTypeCol As Integer = 4

        Dim vResults(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                bPMAddParameter.AddParameterLite(m_oDatabase, "Insurance_File_Cnt", v_lInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                m_lReturn = .SQLSelect(sSQL:=ACGetCDAccountDetailsForPolicySQL, sSQLName:=ACGetCDAccountDetailsForPolicyName, bStoredProcedure:=ACGetCDAccountDetailsForPolicyStored, vResultArray:=vResults)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, ACGetCDAccountDetailsForPolicySQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                If Not Informations.IsArray(vResults) Then
                    gPMFunctions.RaiseError(kMethodName, "No record found for the insurance file cnt: " & v_lInsuranceFileCnt, gPMConstants.PMELogLevel.PMLogError)
                Else
                    v_bCashDepositPayment = (gPMFunctions.ToSafeString(vResults(kBalanceTypeCol, 0)) = "CD")
                    v_lCashDepositAccountId = gPMFunctions.ToSafeLong(vResults(kCashDepositAccountIDCol, 0))
                    v_bActiveCashDepsoitAccount = (gPMFunctions.ToSafeString(vResults(kCashDepositAccountStatusCol, 0)).Trim() = "ACTIVE")
                    v_lCashDepositBaseAccountKey = gPMFunctions.ToSafeLong(vResults(kCashDepositBaseAccountKeyCol, 0))
                    v_sCashDepositBaseAccountAgentType = gPMFunctions.ToSafeString(vResults(kCashDepositBaseAccountAgentTypeCol, 0))
                End If
            End With

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally

        End Try

        Return result
    End Function

    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class