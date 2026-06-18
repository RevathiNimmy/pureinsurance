Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports bACTCashList
Imports Microsoft.VisualBasic
Imports System
Imports System.Data.SqlClient
Imports System.Globalization

Imports SharedFiles
Imports SSP.Shared
Imports System.Data
Imports SSP.Shared.gACTLibrary

<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable

    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a SIRRiskData.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 11/12/2003
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

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Calling Application Name

    ' Error Code (Private)
    Private m_lReturn As Integer

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private lPMAuthorityLevel As Integer

    Private m_sNewClaimNumber As String = ""

    Private m_lClaimId As Integer

    Private m_oEvent As bSIREvent.Business
    Private m_nIsCloned As Integer
    Private m_bIsCloneReversal As Boolean
    ' WPR085 - Cash List Item
    Public Enum eCashListItemWPR085
        CashlistitemID
        AllocationStatusID
        MediaTypeID
        MediaTypeIssuerID
        CashlistID
        AccountID
        MediaRef
        OurRef
        TheirRef
        Amount
        TransdetailID
        ContactName
        Address1
        Address2
        Address3
        Address4
        PostalCode
        AddressCountry
        PaymentName
        PaymentAccountCode
        PaymentBranchCode
        PaymentExpiryDate
        PaymentReference1
        PaymentReference2
        Letter
        Batch_id
        pmuser_id
        Transaction_Date
        Original_Amount
        Amount_Tendered
        Change
        CashListItem_receipt_type_id
        CashListItem_receipt_status_id
        CashListItem_bank_id
        Cheque_Date
        CC_Name
        CC_Number
        CC_Expiry_Date
        CC_Start_Date
        CC_Issue
        CC_Pin
        CC_Auth_Code
        CC_Customer
        CC_Manual_Auth_Code
        CC_Transaction_Code
        Receipt_Details
        CashListItem_Reverse_PMUser_id
        CashListItem_Reverse_Reason_id
        CashListItem_Payment_Type_id
        CashListItem_Payment_Status_id
        Date_Presented
        Cheque_in_Possession
        Stop_Requested_Date
        Stop_Printed_Date
        Stop_Confirmation_Date
        Reason
        Replaces_CashListItem_id
        XML_Object
        InstalmentArray
        SalvageArray
        CLMUSRecoveryArray
        CLMRVRecoveryArray
        UnderwritingYearID
        CurrencyBaseDate
        CurrencyBaseXRate
        AccountBaseDate
        AccountBaseXrate
        SystemBaseDate
        SystemBaseXrate
        OverrideReason
        CashListItem_Comments_Array
        PartyBankId
        CollectionDate
        Comments
        BGPolicies
        BankLocation
        BankBranch
        ChequeTypeId
        CCBankId
        CardTypeId
        CardTransSlipNo
        ChequeClearingTypeId
        IsLeadAccount
        SplitTotal
        TaxBandId
        TaxAmount
        PMNavBatchKey
        BIC
        IBAN
        LastItem
    End Enum

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

    Public ReadOnly Property NewClaimNumber() As String
        Get
            Return m_sNewClaimNumber
        End Get
    End Property

    Public Property ClaimId() As Integer
        Get
            Return m_lClaimId
        End Get
        Set(ByVal Value As Integer)
            m_lClaimId = Value
        End Set
    End Property

    Public Property IsCloned() As Integer
        Get
            Return m_nIsCloned
        End Get
        Set(value As Integer)
            m_nIsCloned = value
        End Set
    End Property

    Public Property IsCloneReversal() As Boolean
        Get
            Return m_bIsCloneReversal
        End Get
        Set(value As Boolean)
            m_bIsCloneReversal = value
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
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceId As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long

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
            m_iSourceID = iSourceId
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel


            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password

            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            'Temporary fix - only for State in NZ
            If sCallingAppName <> "SiriusTransactionService" Then
                m_lReturn = CommitTrans()
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



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
                If m_oEvent IsNot Nothing Then
                    m_oEvent.Dispose()
                    m_oEvent = Nothing
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

    ''****************************************************************
    '' Name : CopyWorkToClaim
    ''
    '' Desc : update policy type
    ''
    ''****************************************************************
    'Public Function CopyWorkToClaim() As Long
    '
    'Dim lStatus As Long
    'Dim bTransOpen As Boolean
    'Dim lErrNumber As Long
    'Dim sErrDescription As String
    '
    '    On Error GoTo Err_CopyWorkToClaim
    '
    '    CopyWorkToClaim = PMTrue
    '
    '    'Keep this policy stuff in case we need to 'adapt' any of it...
    ''    'assign current InsuranceFileCnt to object
    ''    m_oInsuranceFile.InsuranceFileCnt = v_lInsuranceFileCnt
    ''
    ''    'get details of current policy
    ''    If m_oInsuranceFile.GetDetails() <> PMTrue Then
    ''        CopyWorkToClaim = PMFalse
    ''        Exit Function
    ''    End If
    ''
    ''    'store current policy number
    ''    m_sNewClaimNumber = m_oInsuranceFile.InsuranceRef
    ''
    ''    Select Case UCase$(m_oInsuranceFile.InsuranceFileType)
    ''        Case "QUOTE"
    ''            m_oInsuranceFile.InsuranceFileType = "POLICY"
    ''
    ''            'get new policy number
    ''            If GetNewClaimNumber(v_lBusinessType:=2, _
    '''                                 v_iBranch:=m_oInsuranceFile.SourceID, _
    '''                                 v_lProductId:=m_oInsuranceFile.ProductID, _
    '''                                 v_lAgent:=if(IsNull(m_oInsuranceFile.LeadAgentCnt), 0, m_oInsuranceFile.LeadAgentCnt), _
    '''                                 r_sGeneratedPolicyNumber:=m_sNewClaimNumber) <> PMTrue Then
    ''
    ''                CopyWorkToClaim = PMFalse
    ''                Exit Function
    ''
    ''            End If
    ''
    ''
    ''            'do we have new policy number
    ''            If m_sNewClaimNumber <> "" Then
    ''                'assign new policy number
    ''                m_oInsuranceFile.InsuranceRef = m_sNewClaimNumber
    ''            End If
    ''
    ''        Case "MTAQUOTE"
    ''            m_oInsuranceFile.InsuranceFileType = "MTA PERM"
    ''        Case "MTAQTETEMP"
    ''            m_oInsuranceFile.InsuranceFileType = "MTA TEMP"
    ''    End Select
    ''
    ''    'change policy to new type
    ''    m_lReturn = m_oInsuranceFile.UpdatePolicy()
    ''
    ''    If (m_lReturn <> PMTrue) Then
    ''        CopyWorkToClaim = PMFalse
    ''        Exit Function
    ''    End If
    ''
    ''    'let's generate an event record...
    ''    m_oInsuranceFile.EventDescription = "Policy made live"
    ''
    ''    m_lReturn = m_oInsuranceFile.MakeEvent()
    ''
    ''    If (m_lReturn <> PMTrue) Then
    ''        CopyWorkToClaim = PMFalse
    ''        Exit Function
    ''    End If
    '
    '
    '    m_lReturn = m_oDatabase.SQLBeginTrans()
    '    bTransOpen = True
    '
    '    ' get original claim id
    '    m_lReturn = GetOriginalClaimID(v_lWorkClaimId:=m_lWorkClaimId, _
    ''                                   r_lOriginalClaimID:=m_lClaimId)
    '
    '    If (m_lReturn <> PMTrue) Then
    '
    '        m_lReturn = m_oDatabase.SQLRollbackTrans()
    '
    '        CopyWorkToClaim = PMFalse
    '
    '        LogMessage m_sUsername, _
    ''            iType:=PMLogOnError, _
    ''            sMsg:="Failed to get original claim id", _
    ''            vApp:=ACApp, _
    ''            vClass:=ACClass, _
    ''            vMethod:="CopyWorkToClaim", _
    ''            vErrNo :=informations.Err.Number, _
    ''            vErrDesc :=informations.Err.Description
    '
    '        Exit Function
    '
    '    End If
    '
    '    If (m_lClaimId <> 0) Then
    '        'delete original claim details except for claim itself
    '        m_lReturn = DeleteClaimAssociate(v_lClaimId:=m_lClaimId)
    '
    '        If (m_lReturn <> PMTrue) Then
    '
    '            m_lReturn = m_oDatabase.SQLRollbackTrans()
    '
    '            LogMessage m_sUsername, _
    ''                iType:=PMLogOnError, _
    ''                sMsg:="Failed to delete claim associates", _
    ''                vApp:=ACApp, _
    ''                vClass:=ACClass, _
    ''                vMethod:="CopyWorkToClaim", _
    ''                vErrNo :=informations.Err.Number, _
    ''                vErrDesc :=informations.Err.Description
    '
    '            CopyWorkToClaim = PMFalse
    '
    '            Exit Function
    '
    '        End If
    '    End If
    '
    '    'get ready to copy work data to claim
    '    m_oDatabase.Parameters.Clear
    '
    '    m_lReturn = m_oDatabase.Parameters.Add(sName:="work_claim_id", _
    ''                                           vValue:=m_lWorkClaimId, _
    ''                                           idirection:=PMParamInput, _
    ''                                           iDataType:=PMLong)
    '    If (m_lReturn <> PMTrue) Then
    '        m_lReturn = m_oDatabase.SQLRollbackTrans()
    '        CopyWorkToClaim = PMFalse
    '        Exit Function
    '    End If
    '
    '    m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_id", _
    ''                                           vValue:=m_lClaimId, _
    ''                                           idirection:=PMParamInputOutput, _
    ''                                           iDataType:=PMLong)
    '    If (m_lReturn <> PMTrue) Then
    '        m_lReturn = m_oDatabase.SQLRollbackTrans()
    '        CopyWorkToClaim = PMFalse
    '        Exit Function
    '    End If
    '
    '    m_lReturn = m_oDatabase.Parameters.Add(sName:="status", _
    ''                                           vValue:=lStatus, _
    ''                                           idirection:=PMParamInputOutput, _
    ''                                           iDataType:=PMLong)
    '    If (m_lReturn <> PMTrue) Then
    '        m_lReturn = m_oDatabase.SQLRollbackTrans()
    '        CopyWorkToClaim = PMFalse
    '        Exit Function
    '    End If
    '
    '    m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCopyWorkToClaimSQL, _
    ''                                      sSQLName:=ACCopyWorkToClaimName, _
    ''                                      bStoredProcedure:=ACCopyWorkToClaimStored)
    '    If (m_lReturn <> PMTrue) Then
    '        m_lReturn = m_oDatabase.SQLRollbackTrans()
    '        CopyWorkToClaim = PMFalse
    '        Exit Function
    '    End If
    '
    '    lStatus = NullToLong(m_oDatabase.Parameters.Item("status").Value)
    '    If (lStatus <> 0) Then
    '        m_lReturn = m_oDatabase.SQLRollbackTrans()
    '        CopyWorkToClaim = PMFalse
    '        Exit Function
    '    End If
    '
    '    m_lClaimId = NullToLong(m_oDatabase.Parameters.Item("claim_id").Value)
    '    '**************
    '
    '    ' RVH - 26/2/2003   Check if claimsbuilder is enabled and if it IS
    '    '                   then we need to copy some GIS related data from
    '    '                   work to live
    '    If ClaimBuilderIsEnabled Then
    '        m_lReturn = CopyWorkToClaimGIS(m_lClaimId, m_lWorkClaimId)
    '
    '        If (m_lReturn <> PMTrue) Then
    '
    '            m_lReturn = m_oDatabase.SQLRollbackTrans()
    '
    '            CopyWorkToClaim = PMFalse
    '            ' Log Error Message
    '            LogMessage m_sUsername, _
    ''                iType:=PMLogError, _
    ''                sMsg:="Failed to copy GIS-related claim details from live to work (CopyClaimToWorkGIS)", _
    ''                vApp:=ACApp, _
    ''                vClass:=ACClass, _
    ''                vMethod:="CopyWorkToClaim"
    '            Exit Function
    '        End If
    '    End If
    '
    '    m_lReturn = m_oDatabase.SQLCommitTrans()
    '
    '    If (m_lReturn <> PMTrue) Then
    '        m_lReturn = m_oDatabase.SQLRollbackTrans()
    '        CopyWorkToClaim = PMFalse
    '        Exit Function
    '    End If
    '
    '
    '    Exit Function
    '
    'Err_CopyWorkToClaim:
    '
    '    ' store error details
    '    lErrNumber = Err.Number
    '    sErrDescription = Err.Description
    '
    '    ' rollback transaction if one is still open
    '    If bTransOpen = True Then
    '        m_lReturn = m_oDatabase.SQLRollbackTrans
    '    End If
    '
    '    CopyWorkToClaim = PMError
    '
    '    LogMessage m_sUsername, _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="CopyWorkToClaim Failed", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="CopyWorkToClaim", _
    ''        vErrNo:=lErrNumber, _
    ''        vErrDesc:=sErrDescription
    '
    '    Exit Function
    '
    'End Function

    ' ***************************************************************** '
    '
    ' Name: DeleteClaim
    '
    ' Description:
    '
    ' History: 08/05/2001 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function DeleteClaim(ByVal v_lClaimId As Integer) As Integer

        Dim result As Integer = 0
        Dim lStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="status", vValue:=CStr(lStatus), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLBeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteClaimSQL, sSQLName:=ACDeleteClaimName, bStoredProcedure:=ACDeleteClaimStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = m_oDatabase.SQLRollbackTrans()

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lStatus = m_oDatabase.Parameters.Item("status").Value

            If lStatus <> 0 Then
                m_lReturn = m_oDatabase.SQLRollbackTrans()

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLCommitTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteClaim Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteClaim", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' RaiseTransactions
    ''' </summary>
    ''' <param name="v_lClaimId"></param>
    ''' <param name="v_bSavedStats"></param>
    ''' <param name="r_lDocumentId"></param>
    ''' <param name="r_bFromSAM"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function RaiseTransactions(ByVal v_lClaimId As Integer) As Integer
        Return RaiseTransactions(v_lClaimId:=v_lClaimId, v_bSavedStats:=False, r_lDocumentId:=0, r_bFromSAM:=False, PerilId:=0)
    End Function

    ''' <summary>
    ''' RaiseTransactions
    ''' </summary>
    ''' <param name="v_lClaimId"></param>
    ''' <param name="r_lDocumentId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function RaiseTransactions(ByVal v_lClaimId As Integer, ByRef r_lDocumentId As Integer) As Integer
        Return RaiseTransactions(v_lClaimId:=v_lClaimId, v_bSavedStats:=False, r_lDocumentId:=r_lDocumentId, r_bFromSAM:=False, PerilId:=0)
    End Function

    ''' <summary>
    ''' RaiseTransactions
    ''' </summary>
    ''' <param name="v_lClaimId"></param>
    ''' <param name="r_lDocumentId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function RaiseTransactions(ByVal v_lClaimId As Integer, ByRef r_lDocumentId As Integer, ByVal PerilId As Integer) As Integer
        Return RaiseTransactions(v_lClaimId:=v_lClaimId, v_bSavedStats:=False, r_lDocumentId:=r_lDocumentId, r_bFromSAM:=False, PerilId:=PerilId)
    End Function

    ''' <summary>
    ''' RaiseTransactions
    ''' </summary>
    ''' <param name="v_lClaimId"></param>
    ''' <param name="v_bSavedStats"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function RaiseTransactions(ByVal v_lClaimId As Integer, ByVal v_bSavedStats As Boolean) As Integer
        Return RaiseTransactions(v_lClaimId:=v_lClaimId, v_bSavedStats:=v_bSavedStats, r_lDocumentId:=0, r_bFromSAM:=False, PerilId:=0)
    End Function

    ''' <summary>
    ''' RaiseTransactions
    ''' </summary>
    ''' <param name="v_lClaimId"></param>
    ''' <param name="v_bSavedStats"></param>
    ''' <param name="r_lDocumentId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function RaiseTransactions(ByVal v_lClaimId As Integer, ByVal v_bSavedStats As Boolean, ByRef r_lDocumentId As Integer) As Integer
        Return RaiseTransactions(v_lClaimId:=v_lClaimId, v_bSavedStats:=v_bSavedStats, r_lDocumentId:=r_lDocumentId, r_bFromSAM:=False, PerilId:=0)
    End Function

    ''' <summary>
    ''' RaiseTransactions
    ''' </summary>
    ''' <param name="v_lClaimId"></param>
    ''' <param name="v_bSavedStats"></param>
    ''' <param name="r_lDocumentId"></param>
    ''' <param name="r_bFromSAM"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function RaiseTransactions(ByVal v_lClaimId As Integer, ByVal v_bSavedStats As Boolean, ByRef r_lDocumentId As Integer, ByRef r_bFromSAM As Boolean, ByRef PerilId As Integer) As Integer

        Dim nResult As Integer
        Const kMethodName As String = "RaiseTransactions"

        Dim oObject As bControlTransClaims.Automated
        Dim oArray(,) As Object
        Dim nStatsFolderCnt As Integer
        Dim bStatsSuppressed As Boolean
        Dim oStatsFolderCnt(,) As Object = Nothing
        Dim bPaymentRefCheckEnabled As Boolean
        Dim nRecordsAffected As Integer
        Dim nReserveAmount As Decimal
        Try

            nResult = PMEReturnCode.PMTrue

            oObject = New bControlTransClaims.Automated
            m_lReturn = oObject.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            oObject.ClaimID = v_lClaimId

            'Need to get the transaction type id from the code...
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=m_sTransactionType, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetTransactionTypeIDSQL, sSQLName:=ACGetTransactionTypeIDName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=oArray)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(oArray) Then

                oObject.TransactionTypeID = oArray(0, 0)
            Else
                Return PMEReturnCode.PMFalse
            End If

            oObject.TransactionTypeCode = m_sTransactionType

            m_lReturn = GetProductDetails(v_lProductId:=v_lClaimId, r_bPaymentRefCheck:=bPaymentRefCheckEnabled)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "GetSystemOption Payment Ref Check Failed", PMELogLevel.PMLogError)
            End If

            'WPR11(a)(b)(c) CHECK FOR Payment, We assume if payment is locked from screen We should take Script payment in account
            m_lClaimId = v_lClaimId
            If (m_sTransactionType = "C_CP" AndAlso GetIsPaymentsReadOnly() = False) OrElse
                 ((m_sTransactionType = "C_CO" OrElse m_sTransactionType = "C_CR") AndAlso GetIsReservesReadOnly() = False) OrElse
                 m_sTransactionType = "C_SA" OrElse m_sTransactionType = "C_RV" Then


                m_oDatabase.Parameters.Clear()

                m_lReturn = m_oDatabase.Parameters.Add(sName:="ReserveAmount",
                                                       vValue:=0,
                                                       iDirection:=PMEParameterDirection.PMParamOutput,
                                                       iDataType:=PMEDataType.PMCurrency)

                If (m_lReturn <> PMEReturnCode.PMTrue) Then
                    Return PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="TransactionType",
                                                       vValue:=m_sTransactionType,
                                                       iDirection:=PMEParameterDirection.PMParamInput,
                                                       iDataType:=PMEDataType.PMString)
                If (m_lReturn <> PMEReturnCode.PMTrue) Then
                    Return PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimId",
                                                       vValue:=v_lClaimId,
                                                       iDirection:=PMEParameterDirection.PMParamInput,
                                                       iDataType:=PMEDataType.PMLong)

                If (m_lReturn <> PMEReturnCode.PMTrue) Then
                    Return PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetReserveAmountSQL,
                                                  sSQLName:=ACGetReserveAmountName,
                                                  bStoredProcedure:=True,
                                                  lRecordsAffected:=nRecordsAffected)

                nReserveAmount = m_oDatabase.Parameters.Item("ReserveAmount").Value
            End If
            'WPR101112 CHECK Ends Here
            ' Lets check there are work_stats before trying to
            'process them.
            Dim nClaimPaymentID As Integer = 0
            Dim sCreditAccountCode As String = ""
            Dim oTaxAmountByTaxType As Object = Nothing
            Dim sTaxTypeCode As String
            Dim crTaxAmount As Decimal
            Dim nlBound As Integer
            Dim nUBound As Integer
            Dim nTaxTypeItem As Integer
            Dim bThisRevesionPresent As Boolean

            Dim nStatsFolderCnt_for_ThisRevesionPresent As Integer = 0
            Dim nC_CP_DocumentID As Integer = 0

            Dim dtResult As DataTable
            Dim bIsReversalCreated As Boolean = False

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=v_lClaimId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="nIgnoreDocRef", vValue:=CStr(1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' Execute selection Query
            m_lReturn = m_oDatabase.ExecuteDataTable(sSQL:=KGetCLMGetStatsFolderForClaimSQL, sSQLName:=KGetCLMGetStatsFolderForClaim, bStoredProcedure:=True, oRecordset:=dtResult)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed to Get Stats Folder from spu_CLM_Get_Stats_Folder_For_Claim", gPMConstants.PMELogLevel.PMLogError)
            End If

            If dtResult IsNot Nothing AndAlso dtResult.Rows.Count > 0 Then
                bIsReversalCreated = True
            End If

            If nReserveAmount <> 0 Or (IsCloned = 1 And bIsreversalCreated) Then
                ''Get stats folder cnt
                If m_sTransactionType = "C_CO" OrElse m_sTransactionType = "C_CR" Then
                    'lTransactionTypeID = 26 'claim open
                    If IsCloned = 1 Then
                        m_lReturn = CreateStatsFolder(r_lStatsFolderCnt:=nStatsFolderCnt, v_sTransactionTypeCode:=m_sTransactionType, v_lClaimId:=v_lClaimId)

                        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                            gPMFunctions.RaiseError(kMethodName, "CreateStatsFolder  Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                        sCreditAccountCode = "CLMRES"

                        '' Create GRS stats detail entries
                        m_lReturn = CreateStatsDetails(nStatsFolderCnt, v_lClaimId, "GRS", sCreditAccountCode, 0, False)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "CreateStatsDetails Payment Ref Check Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                    Else
                        m_lReturn = GetStatsFolderForClaim(v_lClaimId, oStatsFolderCnt)
                        If oStatsFolderCnt Is Nothing Then
                            m_lReturn = CreateStatsFolder(r_lStatsFolderCnt:=nStatsFolderCnt, v_sTransactionTypeCode:=m_sTransactionType, v_lClaimId:=v_lClaimId)

                            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                                gPMFunctions.RaiseError(kMethodName, "CreateStatsFolder  Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If

                            sCreditAccountCode = "CLMRES"

                            '' Create GRS stats detail entries
                            m_lReturn = CreateStatsDetails(nStatsFolderCnt, v_lClaimId, "GRS", sCreditAccountCode, 0, False)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError(kMethodName, "CreateStatsDetails Payment Ref Check Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If
                        End If
                    End If


                Else

                    m_lReturn = GetStatsFolderForClaim(v_lClaimId:=v_lClaimId, r_vStatsFolder:=oStatsFolderCnt)
                    If oStatsFolderCnt Is Nothing Then
                        m_lReturn = CreateStatsFolder(r_lStatsFolderCnt:=nStatsFolderCnt, v_sTransactionTypeCode:=m_sTransactionType, v_lClaimId:=v_lClaimId)

                        If m_lReturn <> PMEReturnCode.PMTrue Then
                            RaiseError(kMethodName, "CreateStatsFolder Payment Ref Check Failed", PMELogLevel.PMLogError)
                        End If

                        '' Create GRS stats detail entries
                        m_lReturn = CreateStatsDetails(nStatsFolderCnt, v_lClaimId, "GRS", sCreditAccountCode, nClaimPaymentID, bThisRevesionPresent, PerilId:=PerilId)

                        If m_lReturn <> PMEReturnCode.PMTrue Then
                            RaiseError(kMethodName, "CreateStatsDetails Payment Ref Check Failed", PMELogLevel.PMLogError)
                        End If
                    End If
                    If IsCloned <> 1 Then
                        ' get tax lines grouped by tax type for this payment
                        If m_sTransactionType = "C_CP" Then
                            m_lReturn = GetClaimTaxAmountsByTaxType(v_iClaimPaymentId:=nClaimPaymentID,
                                                    r_vResults:=oTaxAmountByTaxType)
                        ElseIf m_sTransactionType = "C_SA" OrElse m_sTransactionType = "C_RV" Then
                            m_lReturn = GetClaimTaxAmountsByTaxType(v_iClaimReceiptId:=nClaimPaymentID,
                                                    r_vResults:=oTaxAmountByTaxType)
                        End If
                    End If

                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "GetClaimPaymentTaxAmountsByTaxType Failed", PMELogLevel.PMLogError)
                    End If

                    Dim m_vClaimDetails As Object = Nothing
                    Dim bPostClaimTax As Boolean

                    m_lReturn = GetClaimDetails(v_lClaimId, m_vClaimDetails)

                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "GetClaimDetails Failed", PMELogLevel.PMLogError)
                    End If

                    If Not Informations.IsArray(m_vClaimDetails) Then
                        RaiseError(kMethodName, "GetClaimDetails Failed to return any data", PMELogLevel.PMLogError)
                    Else
                        bPostClaimTax = CBool(ToSafeLong(m_vClaimDetails(kClaimDetailPostClaimsTaxes, 0), 0))
                    End If

                    If (m_sTransactionType = "C_RV" OrElse m_sTransactionType = "C_SA") AndAlso bPostClaimTax Then
                        ' Create stats for gross tax amount

                        If Informations.IsArray(oTaxAmountByTaxType) Then

                            nlBound = DirectCast(oTaxAmountByTaxType, Object(,)).GetLowerBound(1) ' LBound(oTaxAmountByTaxType, 2)
                            nUBound = DirectCast(oTaxAmountByTaxType, Object(,)).GetUpperBound(1) 'UBound(oTaxAmountByTaxType, 2)

                            For nTaxTypeItem = nlBound To nUBound
                                crTaxAmount = crTaxAmount + oTaxAmountByTaxType(kTaxTypeArrayPosTaxAmount, nTaxTypeItem)
                            Next
                            nTaxTypeItem = 0

                            ' Insert stats details records for Tax (One gross line for each tax type)
                            If crTaxAmount <> 0 Then
                                m_lReturn = CreateStatsDetails(nStatsFolderCnt, v_lClaimId, "TAG", sCreditAccountCode, 0, False, crTaxAmount)

                                If m_lReturn <> PMEReturnCode.PMTrue Then
                                    gPMFunctions.RaiseError(kMethodName, "bControlTransClaims.Automated.CreateStatsDetails Failed", PMELogLevel.PMLogError)
                                End If
                            End If

                        End If
                        ' process the tax rows..
                        If Informations.IsArray(oTaxAmountByTaxType) Then

                            nlBound = DirectCast(oTaxAmountByTaxType, Object(,)).GetLowerBound(1) ' LBound(oTaxAmountByTaxType, 2)
                            nUBound = DirectCast(oTaxAmountByTaxType, Object(,)).GetUpperBound(1) 'UBound(oTaxAmountByTaxType, 2)

                            For nTaxTypeItem = nlBound To nUBound

                                ' get the tax type details
                                sTaxTypeCode = (oTaxAmountByTaxType(kTaxTypeArrayPosCode, nTaxTypeItem)).ToString.Trim
                                crTaxAmount = oTaxAmountByTaxType(kTaxTypeArrayPosTaxAmount, nTaxTypeItem)

                                ' Insert stats details records for Tax (One gross line for each tax type)
                                If crTaxAmount <> 0 Then

                                    ' set tan / tag account code
                                    sCreditAccountCode = "NOTA" & sTaxTypeCode & "IN"

                                    ' Create stats for TAN amount
                                    m_lReturn = CreateStatsDetails(nStatsFolderCnt, v_lClaimId, "TAN", sCreditAccountCode, 0, False, -crTaxAmount)

                                    If m_lReturn <> PMEReturnCode.PMTrue Then
                                        RaiseError(kMethodName, "bControlTransClaims.Automated.CreateStatsDetails Failed", PMELogLevel.PMLogError)
                                    End If

                                End If

                            Next

                        End If

                    ElseIf m_sTransactionType = "C_CP" Then

                        ' process the tax rows..
                        If Informations.IsArray(oTaxAmountByTaxType) Then

                            nlBound = DirectCast(oTaxAmountByTaxType, Object(,)).GetLowerBound(1) ' LBound(oTaxAmountByTaxType, 2)
                            nUBound = DirectCast(oTaxAmountByTaxType, Object(,)).GetUpperBound(1) 'UBound(oTaxAmountByTaxType, 2)

                            For nTaxTypeItem = nlBound To nUBound

                                ' get the tax type details
                                sTaxTypeCode = (oTaxAmountByTaxType(kTaxTypeArrayPosCode, nTaxTypeItem)).ToString.Trim
                                crTaxAmount = oTaxAmountByTaxType(kTaxTypeArrayPosTaxAmount, nTaxTypeItem)


                                ' Insert stats details records for Tax (One gross line for each tax type)
                                If crTaxAmount <> 0 AndAlso bPostClaimTax Then

                                    ' set tan / tag account code
                                    sCreditAccountCode = "NOTA" & sTaxTypeCode & "IN"

                                    ' Create stats for gross tax amount
                                    m_lReturn = CreateStatsDetails(nStatsFolderCnt, v_lClaimId, "TAG", sCreditAccountCode, nClaimPaymentID, False, crTaxAmount)

                                    If m_lReturn <> PMEReturnCode.PMTrue Then
                                        RaiseError(kMethodName, "CreateStatsDetails Payment Ref Check Failed", PMELogLevel.PMLogError)
                                    End If

                                End If

                            Next

                        End If
                    End If

                    If bThisRevesionPresent Then

                        m_lReturn = CreateStatsFolder(r_lStatsFolderCnt:=nStatsFolderCnt, v_sTransactionTypeCode:="C_CR", v_lClaimId:=v_lClaimId)

                        If m_lReturn <> PMEReturnCode.PMTrue Then
                            RaiseError(kMethodName, "CreateStatsFolder Payment Ref Check Failed", PMELogLevel.PMLogError)
                        End If

                        nStatsFolderCnt_for_ThisRevesionPresent = nStatsFolderCnt

                        '' Create GRS stats detail entries
                        m_lReturn = CreateStatsDetails(nStatsFolderCnt, v_lClaimId, "GRS", "CLMRES", 0, False, 0, "C_CR")

                        If m_lReturn <> PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "CreateStatsDetails Payment Ref Check Failed", PMELogLevel.PMLogError)
                        End If

                    End If
                End If

            End If
            'AK 040603 - get the stats folder only if it has not been passed
            If Not v_bSavedStats Then
                ' Lets check there are work_stats before trying to
                'process them.

                m_lReturn = oObject.GetStatsFolderForClaim(oStatsFolderCnt)

                If (m_lReturn = PMEReturnCode.PMTrue) AndAlso (Informations.IsArray(oStatsFolderCnt)) Then

                    For iFolderIndex As Integer = oStatsFolderCnt.GetLowerBound(1) To oStatsFolderCnt.GetUpperBound(1)

                        If ToSafeInteger(oStatsFolderCnt(0, iFolderIndex)) <> 0 Then

                            ' set the document type based on the transaction type
                            ' in c_cp only claim payments can be made
                            ' in c_cr and c_co only reserve adjustments can be made
                            If IsCloned = 1 Then
                                oObject.DocumentTypeID = kClonedDocumentTypeID
                                oObject.IsCloned = 1
                            ElseIf m_sTransactionType = "C_CP" Then
                                oObject.DocumentTypeID = kClaimPaymentDocumentTypeID
                            ElseIf m_sTransactionType = "C_SA" OrElse m_sTransactionType = "C_RV" Then
                                oObject.DocumentTypeID = kClaimReceiptDocumentTypeID
                            End If

                            ' Apply coinsurance and reinsurance to create stats.
                            m_lReturn = oObject.CreateStatsForCoinsReins(CInt(oStatsFolderCnt(0, iFolderIndex)))

                            ' get the stats folder cnt
                            nStatsFolderCnt = CInt(oStatsFolderCnt(0, iFolderIndex))

                            ' finalise the stats folders details and determine whether
                            ' the transactions should be suppressed

                            m_lReturn = oObject.FinaliseStats(nStatsFolderCnt, bStatsSuppressed)

                            If m_lReturn <> PMEReturnCode.PMTrue Then
                                Return PMEReturnCode.PMFalse
                            End If

                            ' if the stats have not been suppressed
                            If Not bStatsSuppressed Then

                                m_lReturn = oObject.CreateTransactions(nStatsFolderCnt, r_lDocumentId)
                                If m_lReturn <> PMEReturnCode.PMTrue AndAlso m_lReturn <> PMEReturnCode.PMNotFound Then
                                    Return PMEReturnCode.PMFalse
                                ElseIf m_lReturn = PMEReturnCode.PMNotFound Then
                                    Return PMEReturnCode.PMNotFound
                                End If

                                Dim sAutoReceipt As String = ""
                                m_lReturn = bPMFunc.GetSystemOption(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, 5117, sAutoReceipt)

                                Dim bAutomateReceiptGeneration As Boolean
                                bAutomateReceiptGeneration = ToSafeInteger(sAutoReceipt, "0") = "1"

                                If (IsCloned <> 1) Then
                                    ' WPR085 If we are automatically creating cashlists and items, then do so here.
                                    If bAutomateReceiptGeneration AndAlso (m_sTransactionType = "C_RV" OrElse m_sTransactionType = "C_SA") Then
                                        m_lReturn = GenerateCashList(nStatsFolderCnt, v_lClaimId, r_lDocumentId, r_bFromSAM)

                                        If m_lReturn <> PMEReturnCode.PMTrue Then
                                            RaiseError(kMethodName, "GenerateCashListItem Failed", PMELogLevel.PMLogError)
                                        End If
                                    End If
                                End If

                                ' update the payment associated with the stats folder (if there is one)
                                ' with the associated document id; creating a direct link between
                                ' payments and accounts.
                                If IsCloned <> 1 Then
                                    If m_sTransactionType = "C_CP" Then
                                        m_lReturn = UpdatePaymentDocumentDetails(v_lStatsFolderCnt:=nStatsFolderCnt)
                                        If m_lReturn <> PMEReturnCode.PMTrue Then
                                            Return PMEReturnCode.PMFalse
                                        End If

                                        If bPaymentRefCheckEnabled Then
                                            m_lReturn = UpdatePaymentReference(v_lDocument_Id:=r_lDocumentId)
                                            If m_lReturn <> PMEReturnCode.PMTrue Then
                                                Return PMEReturnCode.PMFalse
                                            End If
                                        End If
                                    Else
                                        m_lReturn = UpdateReceiptDocumentDetails(v_lStatsFolderCnt:=nStatsFolderCnt)
                                        If m_lReturn <> PMEReturnCode.PMTrue Then
                                            Return PMEReturnCode.PMFalse
                                        End If
                                    End If
                                End If
                            End If
                            If nStatsFolderCnt_for_ThisRevesionPresent = nStatsFolderCnt Then
                                If nC_CP_DocumentID <> 0 Then
                                    r_lDocumentId = nC_CP_DocumentID
                                End If
                            Else
                                nC_CP_DocumentID = r_lDocumentId
                            End If
                        End If
                    Next iFolderIndex
                End If

            Else
                ' Get the list of stats folder for this claim
                'If r_bFromSAM Then
                '    m_lReturn = GetStatsFolderForClaim(v_lClaimId, oStatsFolderCnt, "C_CP")
                'Else
                m_lReturn = GetStatsFolderForClaim(v_lClaimId, oStatsFolderCnt)
                'End If
                If Informations.IsArray(oStatsFolderCnt) Then

                    For iFolderIndex As Integer = oStatsFolderCnt.GetLowerBound(1) To oStatsFolderCnt.GetUpperBound(1)

                        If CInt(oStatsFolderCnt(0, iFolderIndex)) <> 0 Then

                            oObject.DocumentTypeID = 28

                            ' Apply coinsurance and reinsurance to create stats.

                            m_lReturn = oObject.CreateStatsForCoinsReins(CInt(oStatsFolderCnt(0, iFolderIndex)))

                            nStatsFolderCnt = CInt(oStatsFolderCnt(0, iFolderIndex))

                            ' finalise the stats folders details and determine whether
                            ' the transactions should be suppressed

                            m_lReturn = oObject.FinaliseStats(nStatsFolderCnt, bStatsSuppressed)
                            If m_lReturn <> PMEReturnCode.PMTrue Then
                                Return PMEReturnCode.PMFalse
                            End If

                            ' if the stats have not been suppressed
                            If Not bStatsSuppressed Then

                                ' just raise these transactions now...
                                ' all the other steps have already been processed before

                                m_lReturn = oObject.CreateTransactions(nStatsFolderCnt, r_lDocumentId)

                                If m_lReturn <> PMEReturnCode.PMTrue AndAlso m_lReturn <> PMEReturnCode.PMNotFound Then
                                    Return PMEReturnCode.PMFalse
                                ElseIf m_lReturn = PMEReturnCode.PMNotFound Then
                                    Return PMEReturnCode.PMNotFound
                                End If

                                ' update the payment associated with the stats folder (if there is one)
                                ' with the associated document id; creating a direct link between
                                ' payments and accounts.
                                If bThisRevesionPresent = True Then

                                    Dim vDocumentType As Object
                                    Dim sql As String

                                    sql = "select dt.code from DocumentType dt join Document d on d.documenttype_id =  dt.documenttype_id where d.document_id = {document_id}"

                                    m_oDatabase.Parameters.Clear()

                                    m_lReturn = m_oDatabase.Parameters.Add(sName:="document_id",
                                                                                vValue:=r_lDocumentId,
                                                                                iDirection:=PMEParameterDirection.PMParamInput,
                                                                                iDataType:=PMEDataType.PMLong)

                                    If m_oDatabase.SQLSelect(
                                                            sSQL:=sql,
                                                            sSQLName:="select document_type from Document",
                                                            bStoredProcedure:=False,
                                                            vResultArray:=vDocumentType,
                                                            lNumberRecords:=gPMConstants.PMAllRecords) <> PMEReturnCode.PMTrue Then

                                        Return PMEReturnCode.PMFalse
                                    End If

                                    If (vDocumentType(0, 0)).ToString.Trim <> "CLA" Then
                                        m_lReturn = UpdatePaymentDocumentDetails(v_lStatsFolderCnt:=nStatsFolderCnt)
                                    End If
                                Else
                                    m_lReturn = UpdatePaymentDocumentDetails(v_lStatsFolderCnt:=nStatsFolderCnt)
                                End If

                                If m_lReturn <> PMEReturnCode.PMTrue Then
                                    Return PMEReturnCode.PMFalse
                                End If

                                If bPaymentRefCheckEnabled Then
                                    m_lReturn = UpdatePaymentReference(v_lDocument_Id:=r_lDocumentId)
                                    If m_lReturn <> PMEReturnCode.PMTrue Then
                                        Return PMEReturnCode.PMFalse
                                    End If
                                End If
                            End If

                            If nStatsFolderCnt_for_ThisRevesionPresent = nStatsFolderCnt Then
                                If nC_CP_DocumentID <> 0 Then
                                    r_lDocumentId = nC_CP_DocumentID
                                End If
                            Else
                                nC_CP_DocumentID = r_lDocumentId
                            End If

                        End If
                    Next iFolderIndex
                End If
            End If

            Return nResult

        Catch excep As Exception

            nResult = PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="RaiseTransactions Failed", vApp:=ACApp,
                               vClass:=ACClass, vMethod:="RaiseTransactions", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult
        Finally
            If oObject IsNot Nothing Then
                oObject.Dispose()
                oObject = Nothing
            End If
        End Try
    End Function

    ''' <summary>
    ''' WPR085 - To get reciept for a stats folder
    ''' </summary>
    ''' <param name="v_nStatsFolderCnt">StatsFolderCnt</param>
    ''' <param name="r_oReceipt">Array of reciepts</param>
    ''' <param name="r_oReceiptItems">Array of reciept items</param>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Public Function GetReceiptFromStatsFolder(ByVal v_nStatsFolderCnt As Integer, ByRef r_oReceipt(,) As Object,
                                          ByRef r_oReceiptItems(,) As Object) As Integer
        Return GetReceiptFromStatsFolder(v_nStatsFolderCnt:=v_nStatsFolderCnt, r_oReceipt:=r_oReceipt,
                                          r_oReceiptItems:=r_oReceiptItems, r_vSource_Id:=1)
    End Function

    Public Function GetReceiptFromStatsFolder(ByVal v_nStatsFolderCnt As Integer, ByRef r_oReceipt(,) As Object,
                                              ByRef r_oReceiptItems(,) As Object, ByRef r_vSource_Id As Integer) As Integer

        Const kMethodName As String = "GetReceiptFromStatsFolder"

        Dim nResult As Integer = 0

        Try

            nResult = PMEReturnCode.PMTrue

            Dim vReceiptId(,) As Object = Nothing
            Dim sql As String


            ' Determine the receipt Id.
            sql = "select Receipt_Id,source_id  from Stats_Folder where stats_folder_cnt =" & v_nStatsFolderCnt

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.SQLSelect(
                                    sSQL:=sql,
                                    sSQLName:="select Receipt_Id from Stats_Folder",
                                    bStoredProcedure:=False,
                                    vResultArray:=vReceiptId,
                                    lNumberRecords:=gPMConstants.PMAllRecords) <> PMEReturnCode.PMTrue Then

                Return PMEReturnCode.PMFalse

            End If
            r_vSource_Id = vReceiptId(1, 0)

            ' Get the receipt.
            sql = "select claim_receipt_id, claim_id, party_cnt, PayeeMediaType, PayeeMediaRef, currency_id from Claim_Receipt where claim_receipt_id =" & vReceiptId(0, 0)

            m_oDatabase.Parameters.Clear()


            If m_oDatabase.SQLSelect(
                                    sSQL:=sql,
                                    sSQLName:="select * from Claim_Receipt",
                                    bStoredProcedure:=False,
                                    vResultArray:=r_oReceipt,
                                    lNumberRecords:=gPMConstants.PMAllRecords) <> PMEReturnCode.PMTrue Then

                Return PMEReturnCode.PMFalse

            End If

            ' Get the receipt items for this receipt.

            m_oDatabase.Parameters.Clear()

            nResult = m_oDatabase.Parameters.Add(sName:="nClaim_Receipt_Id",
                                                    vValue:=vReceiptId(0, 0),
                                                    iDirection:=PMEParameterDirection.PMParamInput,
                                                    iDataType:=PMEDataType.PMInteger)

            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If

            nResult = m_oDatabase.SQLSelect(sSQL:=kGetThisClaimReceiptItemSQL,
                                            sSQLName:=kGetThisClaimReceiptItem,
                                            bStoredProcedure:=True,
                                            vResultArray:=r_oReceiptItems,
                                            lNumberRecords:=gPMConstants.PMAllRecords)

            If nResult <> PMEReturnCode.PMTrue Then

                Return PMEReturnCode.PMFalse

            End If

            Return nResult

        Catch ex As Exception

            nResult = PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:=kMethodName + " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)

            Return nResult

        End Try


    End Function

    ''' <summary>
    ''' WPR085 - To Get document reference for a document id
    ''' </summary>
    ''' <param name="v_nDocumentId"></param>
    ''' <param name="r_oDocumentRef"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDocumentRefFromDocumentId(ByVal v_nDocumentId As Integer, ByRef r_oDocumentRef(,) As Object) As Integer

        Const kMethodName As String = "GetDocumentRefFromDocumentId"

        Dim nResult As Integer
        Dim sSql As String

        Try

            nResult = PMEReturnCode.PMTrue

            ' Get the receipt.
            sSql = "select document_ref from Document where document_id = {document_id}"

            m_oDatabase.Parameters.Clear()

            nResult = m_oDatabase.Parameters.Add(sName:="document_id",
                                                        vValue:=v_nDocumentId,
                                                        iDirection:=PMEParameterDirection.PMParamInput,
                                                        iDataType:=PMEDataType.PMLong)

            If m_oDatabase.SQLSelect(
                                    sSQL:=sSql,
                                    sSQLName:=sSql,
                                    bStoredProcedure:=False,
                                    vResultArray:=r_oDocumentRef,
                                    lNumberRecords:=gPMConstants.PMAllRecords) <> PMEReturnCode.PMTrue Then

                Return PMEReturnCode.PMFalse

            End If

            Return nResult

        Catch ex As Exception

            nResult = PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:=kMethodName + " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)

            Return nResult

        End Try

    End Function


    ''' <summary>
    ''' WPR 85 - To get default cash list item reciept type
    ''' </summary>
    ''' <param name="r_oResults"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDefaultCashListItemReceiptType(ByRef r_oResults(,) As Object) As Integer

        Const kMethodName As String = "GetDefaultCashListItemReceiptType"

        Dim nResult As Integer = 0

        Try

            nResult = PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            Dim sSql As String

            sSql = "select cashlistitem_receipt_type_id, code from CashListItem_Receipt_Type"
            sSql = sSql & " where description='Claim Receipt'"
            sSql = sSql & " and effective_date <= GetDate()"
            sSql = sSql & " and is_Deleted = 0"

            If m_oDatabase.SQLSelect(
                                    sSQL:=sSql,
                                    sSQLName:="GetDefaultCashListItemReceiptType",
                                    bStoredProcedure:=False,
                                    vResultArray:=r_oResults,
                                    lNumberRecords:=gPMConstants.PMAllRecords) <> PMEReturnCode.PMTrue Then

                Return PMEReturnCode.PMFalse

            End If

            Return nResult

        Catch ex As Exception

            nResult = PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:=kMethodName + " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)

            Return nResult

        End Try

    End Function

    ''' <summary>
    ''' WPR 85 - To generate a cash list for claim
    ''' </summary>
    ''' <param name="v_nClaimId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GenerateClaimCashList(ByVal v_nClaimId As Integer) As Integer

        Const kMethodName As String = "GenerateClaimCashList"

        Dim oControlTransClaims As bControlTransClaims.Automated
        Dim nDocumentId As Integer
        Dim vArray(,) As Object = Nothing
        Dim vStatsFolderCnt As Object = Nothing
        Dim bPaymentRefCheckEnabled As Boolean
        Dim nFolderIndex As Integer
        Dim sAutoReceipt As String = ""
        Dim nStatsFolderCnt As Integer
        Dim nResult As Integer

        Try
            nResult = PMEReturnCode.PMTrue

            m_lReturn = bPMFunc.GetSystemOption(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID,
                                        m_iCurrencyID, m_iLogLevel, m_sCallingAppName, 5117, sAutoReceipt)
            Dim bAutomateReceiptGeneration As Boolean
            bAutomateReceiptGeneration = ToSafeLong(sAutoReceipt, "0") = "1"
            If Not bAutomateReceiptGeneration Then
                Return nResult
            End If

            ' WPR085 If we are automatically creating cashlists and items, then do so here.
            oControlTransClaims = New bControlTransClaims.Automated

            m_lReturn = oControlTransClaims.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID,
                                 iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If (m_lReturn <> PMEReturnCode.PMTrue) Then
                nResult = PMEReturnCode.PMFalse
                Return nResult
            End If

            oControlTransClaims.ClaimID = v_nClaimId

            'Need to get the transaction type id from the code...
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="code",
                                                   vValue:=m_sTransactionType,
                                                   iDirection:=PMEParameterDirection.PMParamInput,
                                                   iDataType:=PMEDataType.PMString)
            If (m_lReturn <> PMEReturnCode.PMTrue) Then
                nResult = PMEReturnCode.PMFalse
                Return nResult
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetTransactionTypeIDSQL,
                                              sSQLName:=ACGetTransactionTypeIDName,
                                              bStoredProcedure:=True,
                                              lNumberRecords:=gPMConstants.PMAllRecords,
                                              vResultArray:=vArray)
            If (m_lReturn <> PMEReturnCode.PMTrue) Then
                nResult = PMEReturnCode.PMFalse
                Return nResult
            End If

            If Informations.IsArray(vArray) Then
                oControlTransClaims.TransactionTypeID = vArray(0, 0)
            Else
                nResult = PMEReturnCode.PMFalse
                Return nResult
            End If

            oControlTransClaims.TransactionTypeCode = m_sTransactionType

            m_lReturn = GetProductDetails(v_lProductId:=v_nClaimId,
                                                r_bPaymentRefCheck:=bPaymentRefCheckEnabled)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "GetSystemOption Payment Ref Check Failed", PMELogLevel.PMLogError)
            End If

            ' Lets check there are work_stats before trying to
            'process them.
            m_lReturn = oControlTransClaims.GetStatsFolderForClaim(vStatsFolderCnt)

            If (m_lReturn = PMEReturnCode.PMTrue) And (Informations.IsArray(vStatsFolderCnt)) Then

                For nFolderIndex = DirectCast(vStatsFolderCnt, Object(,)).GetLowerBound(1) To DirectCast(vStatsFolderCnt, Object(,)).GetUpperBound(1)

                    If (vStatsFolderCnt(0, nFolderIndex) <> 0) Then

                        ' get the stats folder cnt
                        nStatsFolderCnt = CInt(vStatsFolderCnt(0, nFolderIndex))

                        Dim vDocIdArray(,) As Object = Nothing
                        m_oDatabase.Parameters.Clear()
                        Dim sSQL As String
                        sSQL = "select d.document_id from Stats_Folder sf inner join Document d on d.document_ref = sf.document_ref where stats_folder_cnt = {stats_folder_cnt}"
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="stats_folder_cnt",
                                                                    vValue:=nStatsFolderCnt,
                                                                    iDirection:=PMEParameterDirection.PMParamInput,
                                                                    iDataType:=PMEDataType.PMInteger)
                        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL,
                                                          sSQLName:="GetDocRefIdFromStatsFolderCnt",
                                                          bStoredProcedure:=False,
                                                          lNumberRecords:=gPMConstants.PMAllRecords,
                                                          vResultArray:=vDocIdArray)
                        If m_lReturn <> PMEReturnCode.PMNotFound Then
                            If Informations.IsArray(vDocIdArray) Then
                                nDocumentId = vDocIdArray(0, 0)
                            End If
                        End If

                        m_lReturn = GenerateCashList(nStatsFolderCnt, v_nClaimId, nDocumentId)
                        If m_lReturn <> PMEReturnCode.PMTrue Then
                            RaiseError(kMethodName, "GenerateClaimCashList Failed", PMELogLevel.PMLogError)
                        End If
                    End If
                Next
            End If


        Catch ex As Exception
            nResult = PMEReturnCode.PMError

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:=kMethodName + " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)

        Finally
            If oControlTransClaims IsNot Nothing Then
                oControlTransClaims.Dispose()
            End If
            oControlTransClaims = Nothing
        End Try

        Return nResult

    End Function

    ''' <summary>
    '''  WPR085- To Automante SRP during salvage/third party
    ''' </summary>
    ''' <param name="v_nStatsFolderCnt"></param>
    ''' <param name="v_nClaimId"></param>
    ''' <param name="v_nDocumentId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GenerateCashList(ByVal v_nStatsFolderCnt As Integer, ByVal v_nClaimId As Integer, v_nDocumentId As Integer, Optional ByVal bViaSAM As Boolean = False) As Integer
        Const kMethodName As String = "GenerateCashList"

        Dim bRolledBack As Boolean
        Dim nReturn As Integer
        Dim oArray(,) As Object
        Dim nCurrencyBaseXRate As Double
        Dim dtCurrencyBaseDate As Date
        Dim dAccountBaseXRate As Double
        Dim dtAccountBaseDate As Date
        Dim dSystemBaseXRate As Double
        Dim dtSystemBaseDate As Date
        Dim nReturnStatus As Integer

        Try

            nReturn = PMEReturnCode.PMTrue

            bRolledBack = False
            Dim oCashList As New Form

            m_oDatabase.SQLBeginTrans()

            nReturn = oCashList.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID,
                                 iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If nReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            ' oCashList.DirectAdd creates a new "cash list" object, an populates all the variables with their defaults.
            ' Then we create 1 or more cash list items that are associated with the cash list.

            Dim oCashListID As Object = Nothing
            Dim oCashListStatusID As Object = Nothing
            Dim oCashListTypeID As Object = Nothing
            Dim oCashListRef As Object = Nothing
            Dim oCompanyID As Object = Nothing
            Dim oBankAccountID As Object = Nothing
            Dim oCurrencyID As Object = Nothing
            Dim oListDate As Object = Nothing
            Dim dControlTotal As Decimal = 0
            Dim oItemCount As Object = Nothing
            Dim oCashlist_drawer_id As Object = Nothing
            Dim oBatch_id As Object = Nothing
            Dim oPMUser_id As Object = Nothing
            Dim oConfirm_PMUser_id As Object = Nothing
            Dim oConfirm2_PMUser_id As Object = Nothing
            Dim oDate_Approved As Object = Nothing
            Dim oBanking_Total As Object = Nothing
            Dim oCash_Float_Amount As Object = Nothing
            Dim oDepositDate As Object = Nothing
            Dim oSubBranchID As Object = Nothing
            Dim oReceipt As Object = Nothing
            Dim oReceiptItems As Object = Nothing

            Dim sSQL As String
            Dim oBankAccount As Object
            Dim nSourceID As Integer
            nReturn = GetReceiptFromStatsFolder(v_nStatsFolderCnt, oReceipt, oReceiptItems, nSourceID)

            oCashListStatusID = 1
            oCashListTypeID = 2
            oCashListRef = "Recovery Receipt"
            oCompanyID = m_iSourceID
            oBankAccountID = 5
            oCurrencyID = oReceipt(5, 0)
            oListDate = DateTime.Now()
            dControlTotal = 0
            oItemCount = DirectCast(oReceiptItems, Object(,)).GetUpperBound(1) ' UBound(oReceiptItems, 1)
            oPMUser_id = m_iUserID
            oSubBranchID = m_iSourceID

            sSQL = "select bankaccount_id from BankAccount_Default " & vbCrLf
            sSQL = sSQL & "where BankAccount_Default.source_id={SourceId} and BankAccount_Default.mediatype_id={mediaTypeId} and " & vbCrLf
            sSQL = sSQL & "BankAccount_Default.effective_date <= GetDate() and BankAccount_Default.is_deleted <> 1 " & vbCrLf
            sSQL = sSQL & "and (BankAccount_Default.product_id Is Null Or BankAccount_Default.product_id = " & vbCrLf
            sSQL = sSQL & "(select TOP 1 product_id from Stats_Folder where Receipt_Id ={ReceiptId})) order by product_id desc " & vbCrLf

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="SourceId",
                                                   vValue:=nSourceID,
                                                   iDirection:=PMEParameterDirection.PMParamInput,
                                                   iDataType:=PMEDataType.PMInteger)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="mediaTypeId",
                                                   vValue:=oReceipt(3, 0),
                                                   iDirection:=PMEParameterDirection.PMParamInput,
                                                   iDataType:=PMEDataType.PMInteger)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ReceiptId",
                                                  vValue:=oReceipt(0, 0),
                                                  iDirection:=PMEParameterDirection.PMParamInput,
                                                  iDataType:=PMEDataType.PMInteger)

            If m_oDatabase.SQLSelect(
                                   sSQL:=sSQL,
                                   sSQLName:="select BankAccount",
                                   bStoredProcedure:=False,
                                   vResultArray:=oBankAccount,
                                   lNumberRecords:=gPMConstants.PMAllRecords) <> PMEReturnCode.PMTrue Then

                RaiseError(kMethodName, sSQL & " Failed", PMELogLevel.PMLogError)

            End If
            If Informations.IsArray(oBankAccount) Then
                oBankAccountID = oBankAccount(0, 0)
            End If

            ' Calculate control total
            Dim sExcludeTaxOption As String = ""
            m_lReturn = bPMFunc.RetrieveSingleSystemOption(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, 5067, sExcludeTaxOption)

            Dim bExcludeTaxOptionOn As Boolean
            bExcludeTaxOptionOn = (ToSafeLong(sExcludeTaxOption, "0") = "1")

            If bExcludeTaxOptionOn = True Then
                dControlTotal = ToSafeCurrency(oReceiptItems(1, 0)) - ToSafeCurrency(oReceiptItems(2, 0))
            Else
                dControlTotal = ToSafeCurrency(oReceiptItems(1, 0))
            End If


            If dControlTotal < 0 Then
                oCashListTypeID = 1
            End If


            nReturn = oCashList.DirectAdd(vCashListID:=oCashListID,
                                        vCashListStatusID:=oCashListStatusID,
                                        vCashListTypeID:=oCashListTypeID,
                                        vCashListRef:=oCashListRef,
                                        vCompanyID:=oCompanyID,
                                        vBankAccountID:=oBankAccountID,
                                        vCurrencyID:=oCurrencyID,
                                        vListDate:=oListDate,
                                        vControlTotal:=dControlTotal,
                                        vItemCount:=oItemCount,
                                        vCashlist_drawer_id:=oCashlist_drawer_id,
                                        vBatch_id:=oBatch_id,
                                        vPMUser_id:=oPMUser_id,
                                        vConfirm_PMUser_id:=oConfirm_PMUser_id,
                                        vConfirm2_PMUser_id:=oConfirm2_PMUser_id,
                                        vDate_Approved:=oDate_Approved,
                                        vBanking_Total:=oBanking_Total,
                                        vCash_Float_Amount:=oCash_Float_Amount,
                                        vDepositDate:=oDepositDate,
                                        vSubBranchID:=oSubBranchID)

            If nReturn <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "oCashList.DirectAdd Failed", PMELogLevel.PMLogError)
            End If

            ' Create a cash list item for each recovery receipt.
            Dim oCashListItem As New bACTCashListItem.Form

            m_lReturn = oCashListItem.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID,
                                 iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            Dim oCashListItems As Object
            Dim i As Integer
            ReDim oCashListItems(DirectCast(oReceiptItems, Object(,)).GetUpperBound(1)) 'UBound(oReceiptItems, 2))

            For i = 0 To DirectCast(oReceiptItems, Object(,)).GetUpperBound(1)
                Dim nPartyCount As Integer
                Dim nAccountID As Integer
                sSQL = String.Empty
                Dim oAccountDetail As Object = Nothing
                If oReceipt(2, 0) <> "" And oReceipt(2, 0) <> "0" Then
                    nPartyCount = oReceipt(2, 0)

                    nReturn = oCashList.GetAccountFromParty(v_lPartyCnt:=nPartyCount, v_lSourceID:=m_iSourceID, r_lAccountID:=nAccountID)

                    If nReturn <> PMEReturnCode.PMTrue Then
                        Return PMEReturnCode.PMFalse
                    End If
                ElseIf m_sTransactionType = "C_RV" Or m_sTransactionType = "C_SA" Then

                    sSQL = "select *  from Account where short_code like 'CLMRECEIVABLE'"

                    If m_oDatabase.SQLSelect(
                                            sSQL:=sSQL,
                                            sSQLName:="select * from Account",
                                            bStoredProcedure:=False,
                                            vResultArray:=oAccountDetail,
                                            lNumberRecords:=gPMConstants.PMAllRecords) <> PMEReturnCode.PMTrue Then

                        Return PMEReturnCode.PMFalse
                    End If
                    If Informations.IsArray(oAccountDetail) Then
                        nAccountID = oAccountDetail(0, 0)
                    End If
                ElseIf m_sTransactionType = "C_CP" Then

                    sSQL = "select *  from Account where short_code like 'CLMPAYABLE'"

                    If m_oDatabase.SQLSelect(
                                            sSQL:=sSQL,
                                            sSQLName:="select * from Account",
                                            bStoredProcedure:=False,
                                            vResultArray:=oAccountDetail,
                                            lNumberRecords:=gPMConstants.PMAllRecords) <> PMEReturnCode.PMTrue Then

                        Return PMEReturnCode.PMFalse
                    End If
                    If Informations.IsArray(oAccountDetail) Then
                        nAccountID = oAccountDetail(0, 0)
                    End If
                End If

                m_oDatabase.Parameters.Clear()
                m_lReturn = m_oDatabase.Parameters.Add(sName:="account_id", vValue:=nAccountID, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMLong)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=oCompanyID, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMLong)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="currency_id", vValue:=oCurrencyID, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMLong)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="currency_amount_unrounded", vValue:=dControlTotal, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMCurrency)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="currency_base_xrate", vValue:=nCurrencyBaseXRate, iDirection:=PMEParameterDirection.PMParamOutput, iDataType:=PMEDataType.PMDouble)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="currency_base_date", vValue:=dtCurrencyBaseDate, iDirection:=PMEParameterDirection.PMParamOutput, iDataType:=PMEDataType.PMDate)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="account_base_xrate", vValue:=dAccountBaseXRate, iDirection:=PMEParameterDirection.PMParamOutput, iDataType:=PMEDataType.PMDouble)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="account_base_date", vValue:=dtAccountBaseDate, iDirection:=PMEParameterDirection.PMParamOutput, iDataType:=PMEDataType.PMDate)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="system_base_xrate", vValue:=dSystemBaseXRate, iDirection:=PMEParameterDirection.PMParamOutput, iDataType:=PMEDataType.PMDouble)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="system_base_date", vValue:=dtSystemBaseDate, iDirection:=PMEParameterDirection.PMParamOutput, iDataType:=PMEDataType.PMDate)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="return_status", vValue:=nReturnStatus, iDirection:=PMEParameterDirection.PMParamOutput, iDataType:=PMEDataType.PMLong)

                oArray = Nothing
                ' get exchange rates
                If m_oDatabase.SQLSelect(
                                sSQL:=ACDoCurrencyConversionSQL,
                                sSQLName:=ACDoCurrencyConversionName,
                                bStoredProcedure:=True,
                                vResultArray:=oArray,
                                lNumberRecords:=gPMConstants.PMAllRecords) <> PMEReturnCode.PMTrue Then

                    Return PMEReturnCode.PMFalse
                End If

                nCurrencyBaseXRate = ToSafeDouble(m_oDatabase.Parameters.Item("currency_base_xrate").Value, 1)
                dtCurrencyBaseDate = ToSafeDate(m_oDatabase.Parameters.Item("currency_base_date").Value, DateTime.MinValue)
                dAccountBaseXRate = ToSafeDouble(m_oDatabase.Parameters.Item("account_base_xrate").Value, 1)
                dtAccountBaseDate = ToSafeDate(m_oDatabase.Parameters.Item("account_base_date").Value, DateTime.MinValue)
                dSystemBaseXRate = ToSafeDouble(m_oDatabase.Parameters.Item("system_base_xrate").Value, 1)
                dtSystemBaseDate = ToSafeDate(m_oDatabase.Parameters.Item("system_base_date").Value, DateTime.MinValue)

                Dim oDefaultReceiptType As Object = Nothing
                nReturn = GetDefaultCashListItemReceiptType(oDefaultReceiptType)

                If nReturn <> PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "GetDefaultCashListItemReceiptType Failed", PMELogLevel.PMLogError)
                End If

                Dim vCashListItem As Object
                ReDim vCashListItem(eCashListItemWPR085.LastItem)

                vCashListItem(eCashListItemWPR085.AllocationStatusID) = 1
                vCashListItem(eCashListItemWPR085.MediaTypeID) = oReceipt(3, 0)
                vCashListItem(eCashListItemWPR085.CashlistID) = oCashListID
                vCashListItem(eCashListItemWPR085.AccountID) = nAccountID
                vCashListItem(eCashListItemWPR085.MediaRef) = oReceipt(4, 0)
                vCashListItem(eCashListItemWPR085.OurRef) = ""
                vCashListItem(eCashListItemWPR085.TheirRef) = ""
                vCashListItem(eCashListItemWPR085.Amount) = dControlTotal
                vCashListItem(eCashListItemWPR085.Original_Amount) = dControlTotal
                vCashListItem(eCashListItemWPR085.Amount_Tendered) = dControlTotal
                vCashListItem(eCashListItemWPR085.Transaction_Date) = DateTime.Now()
                If dControlTotal > 0 Then
                    vCashListItem(eCashListItemWPR085.CashListItem_receipt_type_id) = oDefaultReceiptType(0, 0)
                    vCashListItem(eCashListItemWPR085.CashListItem_receipt_status_id) = 1
                Else
                    vCashListItem(eCashListItemWPR085.CashListItem_Payment_Type_id) = 1
                    vCashListItem(eCashListItemWPR085.CashListItem_Payment_Status_id) = 1
                End If
                vCashListItem(eCashListItemWPR085.Batch_id) = String.Empty
                vCashListItem(eCashListItemWPR085.pmuser_id) = m_iUserID
                vCashListItem(eCashListItemWPR085.PaymentName) = ""
                vCashListItem(eCashListItemWPR085.Comments) = ""
                vCashListItem(eCashListItemWPR085.ContactName) = ""
                vCashListItem(eCashListItemWPR085.Address1) = ""
                vCashListItem(eCashListItemWPR085.Address2) = ""
                vCashListItem(eCashListItemWPR085.Address3) = ""
                vCashListItem(eCashListItemWPR085.Address4) = ""
                vCashListItem(eCashListItemWPR085.PostalCode) = ""
                vCashListItem(eCashListItemWPR085.PaymentAccountCode) = ""
                vCashListItem(eCashListItemWPR085.PaymentBranchCode) = ""
                vCashListItem(eCashListItemWPR085.PaymentName) = ""
                vCashListItem(eCashListItemWPR085.PaymentReference1) = ""
                vCashListItem(eCashListItemWPR085.PaymentReference2) = ""
                vCashListItem(eCashListItemWPR085.CC_Auth_Code) = ""
                vCashListItem(eCashListItemWPR085.CC_Customer) = ""
                vCashListItem(eCashListItemWPR085.CC_Number) = ""
                vCashListItem(eCashListItemWPR085.CC_Expiry_Date) = ""
                vCashListItem(eCashListItemWPR085.CC_Start_Date) = ""
                vCashListItem(eCashListItemWPR085.CC_Issue) = ""
                vCashListItem(eCashListItemWPR085.CC_Pin) = ""
                vCashListItem(eCashListItemWPR085.CC_Name) = ""
                vCashListItem(eCashListItemWPR085.CC_Manual_Auth_Code) = ""
                vCashListItem(eCashListItemWPR085.CC_Transaction_Code) = ""
                vCashListItem(eCashListItemWPR085.Reason) = ""
                vCashListItem(eCashListItemWPR085.XML_Object) = ""
                vCashListItem(eCashListItemWPR085.BankLocation) = ""
                vCashListItem(eCashListItemWPR085.BankBranch) = ""
                vCashListItem(eCashListItemWPR085.Receipt_Details) = ""
                vCashListItem(eCashListItemWPR085.CurrencyBaseXRate) = nCurrencyBaseXRate
                vCashListItem(eCashListItemWPR085.CurrencyBaseDate) = dtCurrencyBaseDate
                vCashListItem(eCashListItemWPR085.AccountBaseXrate) = dAccountBaseXRate
                vCashListItem(eCashListItemWPR085.AccountBaseDate) = dtAccountBaseDate
                vCashListItem(eCashListItemWPR085.SystemBaseXrate) = dSystemBaseXRate
                vCashListItem(eCashListItemWPR085.SystemBaseDate) = dtSystemBaseDate
                vCashListItem(eCashListItemWPR085.BIC) = ""
                vCashListItem(eCashListItemWPR085.IBAN) = ""

                nReturn = oCashListItem.DirectAdd(r_vCashListItem:=vCashListItem)

                If nReturn <> PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "oCashListItem.DirectAdd Failed", PMELogLevel.PMLogError)
                End If

                oCashListItems(i) = vCashListItem(0)
            Next i

            ' Post and allocate the cash list items

            Dim oCashListPost As New bACTCashListPost.Automated

            nReturn = oCashListPost.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID,
                                 iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If nReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If


            Dim vClaimDetails As Object = Nothing
            nReturn = GetClaimDetails(v_lClaimId:=v_nClaimId, r_vResultArray:=vClaimDetails)

            If (nReturn <> PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "bCLMChangeClaimStatus.GetClaimDetails Failed", PMELogLevel.PMLogError)
            End If

            Dim nInsuranceFilecnt As Integer
            nInsuranceFilecnt = CInt(vClaimDetails(1, 0))

            Dim sDocumentRef As Object = Nothing
            nReturn = GetDocumentRefFromDocumentId(v_nDocumentId, sDocumentRef)
            If nReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            Dim vKeyArray(,) As Object
            ReDim vKeyArray(1, 1)

            For i = 0 To oCashListItems.GetUpperBound(0)
                vKeyArray(0, 0) = "cashlist_id"
                vKeyArray(1, 0) = oCashListID

                vKeyArray(0, 1) = "cashlistitem_id"
                vKeyArray(1, 1) = oCashListItems(i)

                If oCashListPost.SetKeys(vKeyArray:=vKeyArray) <> PMEReturnCode.PMTrue Then
                    ' Failed to get an instance of the business object.
                    RaiseError(kMethodName, "Failed to set keys" & vbCrLf & "bACTCashListPost.Automated", PMELogLevel.PMLogError)
                End If

                Dim iAllocationStatus As Integer
                Dim sDocumentReference As String = ""
                If v_nDocumentId <> 0 Then
                    sDocumentReference = (sDocumentRef(0, 0)).ToString.Trim
                End If

                'Post and allocate the individual receipt
                If oCashListPost.PostAllocatedCashListItem(
                    lCashListID:=oCashListID,
                    lCashListItemId:=oCashListItems(i),
                    lInsuranceFileCnt:=nInsuranceFilecnt,
                    sDocumentRef:=sDocumentReference,
                    lWriteOffReasonID:=0,
                    cWriteOffAmount:=0,
                    bCurrencyWriteOff:=False,
                    r_iAllocationStatus:=iAllocationStatus) <> PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "Failed to Post Cash List items" & vbCrLf & "bACTCashListPost.Automated", PMELogLevel.PMLogError)
                End If
            Next i


        Catch ex As Exception

            nReturn = PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:=kMethodName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)

            m_oDatabase.SQLRollbackTrans()

            bRolledBack = True

        Finally

            If bRolledBack = False Then
                m_oDatabase.SQLCommitTrans()
            End If

        End Try

        Return nReturn
    End Function


    ' ***************************************************************** '
    ' Name: CreateEvent (Private)
    '
    ' Description: Create an event record.
    '
    ' ***************************************************************** '
    Public Function CreateEvent(ByVal v_lClaimId As Integer, ByVal v_lEventType As Integer, ByVal v_sDescription As String) As Integer

        Dim result As Integer = 0
        Dim lPartyCnt, lInsuranceFolderCnt, lInsuranceFileCnt, lEventCnt As Integer
        Dim vArray(,) As Object


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetCountsSQL, sSQLName:=ACGetCountsName, bStoredProcedure:=ACGetCountsStored, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            lPartyCnt = CInt(vArray(0, 0))

            lInsuranceFolderCnt = CInt(vArray(1, 0))

            lInsuranceFileCnt = CInt(vArray(2, 0))


            vArray = Nothing

            If m_oEvent Is Nothing Then


                m_oEvent = New bSIREvent.Business
                m_lReturn = m_oEvent.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the event object", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEvent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result
                End If
            End If



            m_lReturn = m_oEvent.DirectAdd(vEventCnt:=lEventCnt, vPartyCnt:=lPartyCnt, vInsuranceFolderCnt:=lInsuranceFolderCnt, vInsuranceFileCnt:=lInsuranceFileCnt, vClaimCnt:=v_lClaimId, vDocumentCnt:=DBNull.Value, vOldAddressCnt:=DBNull.Value, vNewAddressCnt:=DBNull.Value, vCampaignId:=DBNull.Value, vDocumentType:=DBNull.Value, vReportType:=DBNull.Value, vEventType:=v_lEventType, vUserId:=m_iUserID, vEventDate:=DateTime.Today, vDescription:=v_sDescription)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add the event", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEvent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name : IsClaimDateChanged
    '
    ' Desc : return pmtrue if claim date has been changed
    '
    ' Hist : TN 22 June 2001 - Created
    ' ***************************************************************** '
    Public Function IsClaimDateChanged(ByVal v_lClaimId As Integer, ByRef r_lChanged As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = m_oDatabase.SQLSelect(sSQL:=ACGetLossDateSQL, sSQLName:=ACGetLossDateName, bStoredProcedure:=True, vResultArray:=vResultArray)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            '0 - current_claim.Lost_From_Date
            '1 - current_claim.Lost_To_Date
            '2 - previous claim.Lost_From_Date
            '3 - previous claim.Lost_To_Date


            If Not vResultArray(0, 0).Equals(vResultArray(2, 0)) Or Not vResultArray(1, 0).Equals(vResultArray(3, 0)) Then
                r_lChanged = gPMConstants.PMEReturnCode.PMTrue
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsClaimDateChanged Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsClaimDateChanged", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name : IsAddTask
    '
    ' Desc : return true if we need to add external handler tasks
    '
    ' Hist : 22 June 2001 Tinny - Created
    ' ***************************************************************** '
    Public Function IsAddTask(ByVal v_lClaimId As Integer, ByVal v_sTransactionType As String, ByRef r_lAddTask As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = m_oDatabase.SQLSelect(sSQL:=ACGetClaimFlagSQL, sSQLName:=ACGetClaimFlagName, bStoredProcedure:=True, vResultArray:=vResultArray)


            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return result
            End If

            '0 - current_claim.info_only
            '1 - current_claim.Claim_Status_id
            '2 - previous claim.info_only

            ' don't add task if its salvage/third party recovery
            If v_sTransactionType = "C_SA" Or v_sTransactionType = "C_RV" Or v_sTransactionType = "C_CP" Then
                Return result
            End If

            ' don't add task if its an info only claim

            If CBool(vResultArray(0, 0)) Then
                Return result
            End If

            ' don't add task if we are closing a claim

            If CDbl(vResultArray(1, 0)) = 3 Then
                Return result
            End If

            ' don't add task if this claim wasn't an info only claim

            If v_sTransactionType = "C_CR" And Not CBool(vResultArray(2, 0)) Then
                Return result
            End If

            r_lAddTask = gPMConstants.PMEReturnCode.PMTrue

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsAddTask Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsAddTask", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name : GetClaimDetails
    '
    ' Desc : get claim details
    '
    ' Hist : 23 June 2001 Tinny - Created
    ' ***************************************************************** '
    Public Function GetClaimDetails(ByVal v_lClaimId As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = m_oDatabase.SQLSelect(sSQL:=ACGetClaimDetailSQL, sSQLName:=ACGetClaimDetailName, bStoredProcedure:=True, vResultArray:=r_vResultArray)



            If result = gPMConstants.PMEReturnCode.PMTrue Then
                If Not Informations.IsArray(r_vResultArray) Then
                    result = gPMConstants.PMEReturnCode.PMNotFound
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    Public Function BeginTrans() As Integer

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    Public Function CommitTrans() As Integer

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    Public Function RollbackTrans() As Integer

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' private Methods (End)


    Public Sub New()
        MyBase.New()


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
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(informations.Err().Number), vErrDesc:=excep.Message)
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
    ' Name: UpdateInsuranceFileSystem
    '
    ' Description:
    '
    ' History: 28/06/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function UpdateInsuranceFileSystem(ByVal v_lClaimId As Integer) As Integer
        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object
        Dim lTransTypeId As Integer
        Dim sTransTypeDescription As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get the required transaction details first.
            sSQL = "SELECT transaction_type_id, description" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "FROM Transaction_Type" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE LTRIM(RTRIM(code)) = '" & m_sTransactionType.Trim() & "'"

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetTransactionTypeDetails", bStoredProcedure:=False, vResultArray:=vResultArray)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            lTransTypeId = CInt(vResultArray(0, 0))

            sTransTypeDescription = CStr(vResultArray(1, 0))

            sSQL = "UPDATE Insurance_File_System" & Strings.ChrW(13) & Strings.ChrW(10)
            'sSQL = sSQL & "SET modified_by_id = " & m_iUserID% & vbCrLf
            'sSQL = sSQL & ", last_modified = {last_modified}" & vbCrLf
            sSQL = sSQL & " SET last_trans_date = {last_trans_date}" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & ",last_trans_type_id = " & CStr(lTransTypeId) & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & ",last_trans_description = IsNull(last_trans_description,'" & sTransTypeDescription & "')" & Strings.ChrW(13) & Strings.ChrW(10)
            'last_trans_debit_credit
            sSQL = sSQL & "FROM insurance_file_system ifs, claim c" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE c.claim_id = " & CStr(v_lClaimId) & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "AND ifs.insurance_file_cnt = c.policy_id"

            m_oDatabase.Parameters.Clear()
            'developer guide no.40
            m_lReturn = m_oDatabase.Parameters.Add(sName:="last_modified", vValue:=DateTime.Now, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            'developer guide no.40
            m_lReturn = m_oDatabase.Parameters.Add(sName:="last_trans_date", vValue:=DateTime.Now, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="UpdateInsuranceFileSystem", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateInsuranceFileSystem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateInsuranceFileSystem", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetInsFileCntProductId
    '
    ' Description:  Get the ProductId for the supplied insurancefilecnt
    '
    ' Hist : 06052003 - Ajit Kumar - Created
    '
    ' ***************************************************************** '
    Public Function GetInsFileCntProductId(ByVal v_lInsuranceFilecnt As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=CStr(v_lInsuranceFilecnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetInsFileCntProductIdSQL, sSQLName:=ACGetInsFileCntProductIdName, bStoredProcedure:=True, vResultArray:=r_vResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result



        result = gPMConstants.PMEReturnCode.PMFalse
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInsFileCntProductId Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInsFileCntProductId", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result
    End Function


    'AK 27052003 - function to set the status of pending payments to 'Referred'/'Processed'
    Public Function SetPaymentReferred(ByVal v_lClaimId As Integer, ByVal v_iStatus As Integer) As Object

        Dim result As Object = Nothing
        result = gPMConstants.PMEReturnCode.PMTrue
        Try

            m_oDatabase.Parameters.Clear()

            ' add claim id parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="claimid", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If
            ' add status parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="status", vValue:=CStr(v_iStatus), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' run SP
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSetReferredPaymentSQL, sSQLName:=ACSetReferredPaymentName, bStoredProcedure:=ACSetReferredPaymentStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetPaymentReferred Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetPaymentReferred", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function

    Public Function SetPaymentRecommendation(ByVal v_lClaimId As Integer, ByVal v_iStatus As Integer) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "SetPaymentRecommendation"

        Try


            result = gPMConstants.PMEReturnCode.PMTrue
            m_oDatabase.Parameters.Clear()

            ' add claim id parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="claimid", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                Return result
            End If
            ' add status parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="status", vValue:=CStr(v_iStatus), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                Return result
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSetPaymentForRecommendationSQL, sSQLName:=ACSetPaymentForRecommendationName, bStoredProcedure:=ACSetPaymentForRecommendationStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                Return result
            End If


        Catch ex As Exception

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    'AK 28052003 - function to get total payment amount for a claim
    Public Function GetTotalPayment(ByVal v_lClaimId As Integer, ByRef r_cAmount As Decimal) As Object

        Dim result As Object = Nothing
        Dim vArray(,) As Object

        result = gPMConstants.PMEReturnCode.PMTrue
        Try

            m_oDatabase.Parameters.Clear()

            ' add claim id parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="claimid", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' run SP
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetTotalPaymentSQL, sSQLName:=ACGetTotalPaymentName, bStoredProcedure:=ACGetTotalPaymentStored, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            If Not Informations.IsArray(vArray) Then
                r_cAmount = 0
                Return result
            End If

            'Thinh Nguyen 29/06/2003 (start) - make sure we have a valid value

            Dim dbNumericTemp As Double
            If Double.TryParse(CStr(vArray(0, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                r_cAmount = CDec(vArray(0, 0))
            End If
            'Thinh Nguyen 29/06/2003 (end) - make sure we have a valid value

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTotalPayment Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTotalPayment", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'AK 28052003 - function to get user group id for claim supervisor group
    Public Function GetClaimAdminGroup(ByRef r_lGroupId As Integer) As Object
        Return GetClaimAdminGroup(r_lGroupId:=r_lGroupId, v_sGroupCode:="CLMSUPER")
    End Function

    Public Function GetClaimAdminGroup(ByRef r_lGroupId As Integer, ByVal v_sGroupCode As String) As Object

        Dim result As Object = Nothing
        Dim vArray(,) As Object

        result = gPMConstants.PMEReturnCode.PMTrue
        Try

            m_oDatabase.Parameters.Clear()

            ' add claim id parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Code", vValue:=v_sGroupCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' run SP
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetUserGroupIdSQL, sSQLName:=ACGetUserGroupIdName, bStoredProcedure:=ACGetUserGroupIdStored, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            If Not Informations.IsArray(vArray) Then
                Return result
            End If


            r_lGroupId = CInt(vArray(0, 0))

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimAdminGroup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimAdminGroup", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetClaimNumber(ByVal v_lClaimId As Integer, ByRef r_sClaimNumber As String) As Integer

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim vArray(,) As Object

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            With m_oDatabase
                .Parameters.Clear()

                ' add claim id parameter
                m_lReturn = .Parameters.Add(sName:="Claim_ID", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimNumber failed to add Claim_ID parameter.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimNumber", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                ' run SP
                m_lReturn = .SQLSelect(sSQL:="SELECT Claim_Number FROM Claim WHERE Claim_id = {Claim_ID}", sSQLName:="Get Claim Number", bStoredProcedure:=False, vResultArray:=vArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimAdminGroup failed to run the Stored Proc.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimNumber", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If
            End With

            If Not Informations.IsArray(vArray) Then
                r_sClaimNumber = ""
                Return result
            End If


            r_sClaimNumber = CStr(vArray(0, 0)).Trim()

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimAdminGroup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimNumber", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    'AK - 040603 - function to get a list of stats folder for a claim
    Public Function GetStatsFolderForClaim(ByVal v_lClaimId As Integer, ByRef r_vStatsFolder(,) As Object) As Integer
        Return GetStatsFolderForClaim(v_lClaimId:=v_lClaimId, r_vStatsFolder:=r_vStatsFolder, r_sTransactionType:="")
    End Function

    Public Function GetStatsFolderForClaim(ByVal v_lClaimId As Integer, ByRef r_vStatsFolder(,) As Object, ByRef r_sTransactionType As String) As Integer
        Dim result As Integer
        Dim sSQL As String
        Dim vResultArray(,) As Object

        Try
            sSQL = String.Empty
            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            AddInputParameter(v_sName:="claim_id", v_vValue:=v_lClaimId, v_iType:=gPMConstants.PMEDataType.PMLong)

            If r_sTransactionType = "" Then

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=kGetStatsFolderForClaimSQL, sSQLName:=kGetStatsFolderForClaimName, bStoredProcedure:=True, vResultArray:=vResultArray)
            Else
                AddInputParameter(v_sName:="transaction_type_code", v_vValue:=r_sTransactionType, v_iType:=gPMConstants.PMEDataType.PMString)
                m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_CLM_Get_Stats_Folder", sSQLName:=kGetStatsFolderForClaimName, bStoredProcedure:=True, vResultArray:=vResultArray)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            r_vStatsFolder = vResultArray

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetStatsFolderForClaim Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStatsFolderForClaim", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    'AK - 100603 - function to remove existing authorisation task instances
    Public Function RemoveAuthTasks(ByRef v_sDescription As Object) As Integer
        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add(sName:="description", vValue:=CStr(v_sDescription), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACRemoveAuthTasksSQL, sSQLName:=ACRemoveAuthTasksName, bStoredProcedure:=ACRemoveAuthTasksStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RemoveAuthTasks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RemoveAuthTasks", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetPaymentAmount
    '
    ' Parameters: n/a
    '
    ' Description: Returns the amount of any payments made against
    '               the current claim id in this session
    '
    ' History:
    '           Created : MEvans : 17-03-2005 : PN19467
    ' ***************************************************************** '
    Public Function GetPaymentAmount(ByVal v_lClaimId As Integer, ByRef r_crPaymentAmount As Decimal, ByRef r_crThisPaymentAmount As Decimal, ByRef r_lCurrencyId As Integer, Optional ByRef r_lOriginalPaymentAmount As Decimal = 0, Optional ByRef r_lOriginalCurrencyId As Integer = 0) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPaymentAmount"

        Dim lReturn As Integer
        Dim vResults(,) As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters

            ' claim id
            m_lReturn = AddInputParameter(v_sName:="claim_id", v_vValue:=v_lClaimId, v_iType:=gPMConstants.PMEDataType.PMLong)

            ' user id
            m_lReturn = AddInputParameter(v_sName:="user_id", v_vValue:=m_iUserID, v_iType:=gPMConstants.PMEDataType.PMLong)

            ' Execute selection Query
            If m_oDatabase.SQLSelect(sSQL:=kGetPaymentAmountSQL, sSQLName:=kGetPaymentAmountName, bStoredProcedure:=True, vResultArray:=vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetPaymentAmountSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If

            If Informations.IsArray(vResults) Then

                r_crPaymentAmount = CDec(vResults(0, 0))

                r_crThisPaymentAmount = CDec(vResults(1, 0))

                r_lCurrencyId = CInt(vResults(2, 0))

                'Original Payment
                r_lOriginalPaymentAmount = CDec(vResults(3, 0))

                'Original Currency Code
                r_lOriginalCurrencyId = CInt(vResults(4, 0))
            Else
                r_crPaymentAmount = 0
                r_crThisPaymentAmount = 0
                r_lCurrencyId = 26
                r_lOriginalPaymentAmount = 0
                r_lOriginalCurrencyId = 26
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


    ' ***************************************************************** '
    ' Name: AddInputParameter
    '
    ' Parameters: v_sName   : Parameter Name
    '             v_vValue  : Parameter Value
    '             v_iType   : Parameter DataType
    '
    ' Description: Adds an input parameter to the database parameters
    '
    ' History:
    '           Created : MEvans : 18-12-2002 : 202
    ' ***************************************************************** '
    Private Function AddInputParameter(ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iType As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "AddInputParameter"



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Add Parameter to database object

        'Developer Guide no. 85
        If m_oDatabase.Parameters.Add(sName:=v_sName, vValue:=v_vValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iType) <> gPMConstants.PMEReturnCode.PMTrue Then

            ' Log Error.
            result = gPMConstants.PMEReturnCode.PMFalse



            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to add parameter name:" & v_sName &
                                      ", values :" & CStr(v_vValue) & ", type:" & CStr(v_iType), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Informations.Err().Description))

        End If

        Return result

    End Function

    '*****************************************************************************************************
    'Get outstanding for Reserve and recover
    '*****************************************************************************************************
    Public Function GetReserveRecoveryOS(ByVal v_lClaimId As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sMessage As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add("ClaimID", CStr(v_lClaimId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to add ClaimID param"
                Throw New Exception(sMessage)
            End If

            If m_oDatabase.SQLSelect(sSQL:=ACGetReserveRecoveryOSSQL, sSQLName:=ACGetReserveRecoveryOSName, bStoredProcedure:=ACGetReserveRecoveryOSStored, vResultArray:=r_vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to execute stored procedure to get outstanding amount for reserve and recovery"
                Throw New Exception(sMessage)
            End If


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            If sMessage = "" Then
                sMessage = "Failed to get outstanding amounts for reserve and recovery"
            End If


            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lClaimId", v_lClaimId)
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="GetReserveRecoveryOS()", excep:=ex, oDicParms:=oDict)

        Finally

        End Try
        Return result
    End Function

    '*******************************************************************************************************
    'update claim.claim status to live (optional value default to live)
    '*******************************************************************************************************
    Public Function UpdateClaimStatus(ByVal v_lClaimId As Integer) As Integer
        Return UpdateClaimStatus(v_lClaimId:=v_lClaimId, v_lClaimStatusID:=2)
    End Function

    Public Function UpdateClaimStatus(ByVal v_lClaimId As Integer, ByVal v_lClaimStatusID As Integer) As Integer

        Dim result As Integer = 0
        Dim sMessage As String = ""

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add("claim_status_id", CStr(v_lClaimStatusID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to add claim_status_id param"
                Throw New Exception(sMessage)
            End If

            If m_oDatabase.Parameters.Add("claim_id", CStr(v_lClaimId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to add claim_id param"
                Throw New Exception(sMessage)
            End If

            If m_oDatabase.SQLAction(sSQL:=ACUpdClaimStatusSQL, sSQLName:=ACUpdClaimStatusName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to update database with new  claim status"
                Throw New Exception(sMessage)
            End If


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            If sMessage = "" Then
                sMessage = "Failed to update _claim status"
            End If


            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lClaimId", v_lClaimId)
            oDict.Add("v_lClaimStatusID", v_lClaimStatusID)
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateClaimStatus()", excep:=ex, oDicParms:=oDict)

        Finally


        End Try
        Return result
    End Function

    Public Function UpdateClaimDesc(ByVal v_lClaimId As Integer, ByVal v_sClaimVersionDescription As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateClaimDesc"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = AddInputParameter(v_sName:="claim_id", v_vValue:=v_lClaimId, v_iType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = AddInputParameter(v_sName:="description", v_vValue:=v_sClaimVersionDescription, v_iType:=gPMConstants.PMEDataType.PMString)

            ' Execute Action Query
            lReturn = m_oDatabase.SQLAction(sSQL:=kUpdateClaimDescSQL, sSQLName:=kUpdateClaimDescName, bStoredProcedure:=True)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kUpdateClaimDescSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

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


    ' ***************************************************************** '
    ' Name: CreateDMEClaimFolder
    '
    ' Parameters: n/a
    '
    ' Description: Creates default DME or Sharepoint Folder
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function CreateDMEClaimFolder(ByVal v_lClaimId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CreateDMEClaimFolder"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sOptionValue As String = ""
        Dim bDocumasterEnabled As Boolean
        Dim oSIRDOCAPI As bSIRDOCAPI.Form
        Dim vClaimDMEFolderDetails As Object

        Dim lClaimID As Integer
        Dim sClaimNumber, sPolicyReference As String
        Dim lInsuranceFolderCnt, lInsuredCnt As Integer
        Dim sClientShortname As String = ""
        Dim iSourceId As Integer
        Dim sDescription As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' check if documaster is installed...
            ' no point doing this if it isnt installed

            lReturn = CType(bPMFunc.GetSystemOption(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, kSysOptDMEInstalled, sOptionValue, m_iSourceID), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetSystemOption DMEInstalled Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get details required to create folder
            ' party cnt
            ' party shortname
            ' source cnt
            ' insurance file
            ' insurance folder
            ' claim id
            ' claim reference

            lReturn = CType(GetClaimDMEFolderDetails(v_lClaimId:=v_lClaimId, r_vResults:=vClaimDMEFolderDetails), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetClaimDMEFolderDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Informations.IsArray(vClaimDMEFolderDetails) Then

                lClaimID = CInt(vClaimDMEFolderDetails(0, 0))

                sClaimNumber = CStr(vClaimDMEFolderDetails(1, 0)).Trim()

                sPolicyReference = CStr(vClaimDMEFolderDetails(2, 0)).Trim()

                lInsuranceFolderCnt = CInt(vClaimDMEFolderDetails(3, 0))

                lInsuredCnt = CInt(vClaimDMEFolderDetails(4, 0))

                sClientShortname = CStr(vClaimDMEFolderDetails(5, 0)).Trim()

                iSourceId = CInt(vClaimDMEFolderDetails(6, 0))

                sDescription = CStr(vClaimDMEFolderDetails(7, 0))
            End If

            If sOptionValue = "1" Then
                ' create instance of documaster link

                oSIRDOCAPI = New bSIRDOCAPI.Form
                lReturn = oSIRDOCAPI.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed to get instance of bSIRDocAPI", gPMConstants.PMELogLevel.PMLogError)
                End If

                lReturn = oSIRDOCAPI.ProcessIndex(lMode:=1, iSourceID:=iSourceId, lPartyId:=lInsuredCnt, sPartyName:=sClientShortname, lInsuranceFolderId:=lInsuranceFolderCnt, sInsuranceFileRef:=sPolicyReference, lClaimId:=lClaimID, sClaimRef:=sClaimNumber & New String(" "c, 3) & sDescription)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed to create dme claim folder", gPMConstants.PMELogLevel.PMLogError)
                End If
            ElseIf sOptionValue = "2" Then
                'Generate a default Sharepoint folder (if Sharepoint is enabled)
                Dim Sharepoint As bSIRSharepoint.Business
                Sharepoint = New bSIRSharepoint.Business
                Sharepoint.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                Sharepoint.GenerateDefaultPath(lInsuredCnt, 0, lClaimID, 0)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
            If oSIRDOCAPI IsNot Nothing Then
                oSIRDOCAPI.Dispose()
            End If
            oSIRDOCAPI = Nothing



            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetClaimDMEFolderDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 22-06-2005 : PN19235
    ' ***************************************************************** '
    Private Function GetClaimDMEFolderDetails(ByVal v_lClaimId As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimDMEFolderDetails"

        Dim lReturn As Integer




        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        m_lReturn = AddInputParameter(v_sName:="claim_id", v_vValue:=v_lClaimId, v_iType:=gPMConstants.PMEDataType.PMLong)

        ' Execute selection Query
        If m_oDatabase.SQLSelect(sSQL:=kGetClaimDMEFolderDetailsSQL, sSQLName:=kGetClaimDMEFolderDetailsName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

            gPMFunctions.RaiseError(kMethodName, kGetClaimDMEFolderDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

        End If

        Return result
    End Function

    ' ***************************************************************** '
    ' Name: UpdatePaymentDocumentDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function UpdatePaymentDocumentDetails(ByVal v_lStatsFolderCnt As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdatePaymentDocumentDetails"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = AddInputParameter(v_sName:="stats_folder_cnt", v_vValue:=v_lStatsFolderCnt, v_iType:=gPMConstants.PMEDataType.PMLong)

            ' Execute Action Query
            If m_oDatabase.SQLAction(sSQL:=kUpdatePaymentDocumentDetailsSQL, sSQLName:=kUpdatePaymentDocumentDetailsName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kUpdatePaymentDocumentDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

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

    ' ***************************************************************** '
    ' Name: UpdateReceiptDocumentDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function UpdateReceiptDocumentDetails(ByVal v_lStatsFolderCnt As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateReceiptDocumentDetails"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = AddInputParameter(v_sName:="stats_folder_cnt", v_vValue:=v_lStatsFolderCnt, v_iType:=gPMConstants.PMEDataType.PMLong)

            ' Execute Action Query
            If m_oDatabase.SQLAction(sSQL:=kUpdateReceiptDocumentDetailsSQL, sSQLName:=kUpdateReceiptDocumentDetailsName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kUpdateReceiptDocumentDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

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

    ' ***************************************************************** '
    ' Name: GetClaimPaymentAccountsDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 24-01-2006 : Cheque Production Workflow (ATD16)
    ' ***************************************************************** '
    Public Function GetClaimPaymentAccountsDetails(ByVal v_lClaimId As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimPaymentAccountsDetails"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = AddInputParameter(v_sName:="claim_id", v_vValue:=v_lClaimId, v_iType:=gPMConstants.PMEDataType.PMLong)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetClaimPaymentAccountsDetailsSQL, sSQLName:=kGetClaimPaymentAccountsDetailsName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kGetClaimPaymentAccountsDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
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


    ' ***************************************************************** '
    ' Name: UpdateClaimIsDirty
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 22-2-2006 : Claims Versioning
    ' ***************************************************************** '
    Public Function UpdateClaimIsDirty(ByVal v_lClaimId As Integer, ByVal v_lIsDirty As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateClaimIsDirty"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = AddInputParameter(v_sName:="claim_id", v_vValue:=v_lClaimId, v_iType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = AddInputParameter(v_sName:="is_dirty", v_vValue:=v_lIsDirty, v_iType:=gPMConstants.PMEDataType.PMLong)

            ' Execute Action Query
            lReturn = m_oDatabase.SQLAction(sSQL:=kUpdateClaimIsDirtySQL, sSQLName:=kUpdateClaimIsDirtyName, bStoredProcedure:=True)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kUpdateClaimIsDirtySQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

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

    ' ***************************************************************** '
    ' Name: FinaliseClaimDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 03-03-2006 : Claims Versioning
    ' ***************************************************************** '
    Public Function FinaliseClaimDetails(ByVal v_lClaimId As Integer, ByVal v_sClaimVersionDescription As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "FinaliseClaimDetails"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = AddInputParameter(v_sName:="claim_id", v_vValue:=v_lClaimId, v_iType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = AddInputParameter(v_sName:="claim_version_description", v_vValue:=v_sClaimVersionDescription, v_iType:=gPMConstants.PMEDataType.PMString)

            ' Execute Action Query
            lReturn = m_oDatabase.SQLAction(sSQL:=kFinaliseClaimDetailsSQL, sSQLName:=kFinaliseClaimDetailsName, bStoredProcedure:=True)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kFinaliseClaimDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

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

    ' ***************************************************************** '
    ' Name: GetOriginalClaimId
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 13-03-2006 : Claims Versioning Changes
    ' ***************************************************************** '
    Public Function GetOriginalClaimId(ByVal v_lClaimId As Integer, ByRef r_lOriginalClaimId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetOriginalClaimId"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vResults(,) As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = AddInputParameter(v_sName:="claim_id", v_vValue:=v_lClaimId, v_iType:=gPMConstants.PMEDataType.PMLong)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetOriginalClaimIdSQL, sSQLName:=kGetOriginalClaimIdName, bStoredProcedure:=True, vResultArray:=vResults, lNumberRecords:=gPMConstants.PMAllRecords)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kGetOriginalClaimIdSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Informations.IsArray(vResults) Then

                r_lOriginalClaimId = CInt(vResults(0, 0))
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

    Public Function UpdateClaimSuppression(ByVal lClaimID As Integer, ByVal lSuppressReserves As Integer, ByVal lSuppressPayments As Integer, ByVal lSuppressRecoveries As Integer) As Integer
        Return UpdateClaimSuppression(lClaimID:=lClaimID, lSuppressReserves:=lSuppressReserves, lSuppressPayments:=lSuppressPayments, lSuppressRecoveries:=lSuppressRecoveries, lOriginalSuppressReserves:=0, lOriginalSuppressPayments:=0, lOriginalSuppressRecoveries:=0)
    End Function

    Public Function UpdateClaimSuppression(ByVal lClaimID As Integer, ByVal lSuppressReserves As Integer, ByVal lSuppressPayments As Integer, ByVal lSuppressRecoveries As Integer, ByRef lOriginalSuppressReserves As Integer) As Integer
        Return UpdateClaimSuppression(lClaimID:=lClaimID, lSuppressReserves:=lSuppressReserves, lSuppressPayments:=lSuppressPayments, lSuppressRecoveries:=lSuppressRecoveries, lOriginalSuppressReserves:=lOriginalSuppressReserves, lOriginalSuppressPayments:=0, lOriginalSuppressRecoveries:=0)
    End Function

    Public Function UpdateClaimSuppression(ByVal lClaimID As Integer, ByVal lSuppressReserves As Integer, ByVal lSuppressPayments As Integer, ByVal lSuppressRecoveries As Integer, ByRef lOriginalSuppressReserves As Integer, ByRef lOriginalSuppressPayments As Integer) As Integer
        Return UpdateClaimSuppression(lClaimID:=lClaimID, lSuppressReserves:=lSuppressReserves, lSuppressPayments:=lSuppressPayments, lSuppressRecoveries:=lSuppressRecoveries, lOriginalSuppressReserves:=lOriginalSuppressReserves, lOriginalSuppressPayments:=lOriginalSuppressPayments, lOriginalSuppressRecoveries:=0)
    End Function

    Public Function UpdateClaimSuppression(ByVal lClaimID As Integer, ByVal lSuppressReserves As Integer, ByVal lSuppressPayments As Integer, ByVal lSuppressRecoveries As Integer, ByRef lOriginalSuppressReserves As Integer, ByRef lOriginalSuppressPayments As Integer, ByRef lOriginalSuppressRecoveries As Integer) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: UpdateClaimSuppression
        ' PURPOSE: Updates the suppression fields on the Claim Record
        '          Pass in 0/1 to set the value or -1 to leave it as-is
        ' AUTHOR: Danny Davis
        ' DATE: 02 March 2007, 17:17:23
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateClaimSuppression"

        Dim lReturn As gPMConstants.PMEReturnCode


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            With m_oDatabase
                .Parameters.Clear()

                ' Add Required Stored Procedure Parameters
                AddInputParameter(v_sName:="claim_id", v_vValue:=lClaimID, v_iType:=gPMConstants.PMEDataType.PMLong)

                AddInputParameter(v_sName:="suppress_reserves", v_vValue:=If(lSuppressReserves = -1, DBNull.Value, lSuppressReserves), v_iType:=gPMConstants.PMEDataType.PMLong)

                AddInputParameter(v_sName:="suppress_payments", v_vValue:=If(lSuppressPayments = -1, DBNull.Value, lSuppressPayments), v_iType:=gPMConstants.PMEDataType.PMLong)

                AddInputParameter(v_sName:="suppress_recoveries", v_vValue:=If(lSuppressRecoveries = -1, DBNull.Value, lSuppressRecoveries), v_iType:=gPMConstants.PMEDataType.PMLong)


                'Developer Guide No.: 85
                .Parameters.Add(sName:="original_suppress_reserves", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)


                'Developer Guide No.: 85
                .Parameters.Add(sName:="original_suppress_payments", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)



                'Developer Guide No.: 85
                .Parameters.Add(sName:="original_suppress_recoveries", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

                ' Execute selection Query
                lReturn = .SQLAction(sSQL:="spu_CLM_Update_Suppression", sSQLName:="Update Claim Suppression", bStoredProcedure:=True)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "spu_CLM_Update_Suppression Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                'Get the original values before the change was applied
                lOriginalSuppressReserves = gPMFunctions.ToSafeLong(.Parameters.Item("original_suppress_reserves").Value)
                lOriginalSuppressPayments = gPMFunctions.ToSafeLong(.Parameters.Item("original_suppress_payments").Value)
                lOriginalSuppressRecoveries = gPMFunctions.ToSafeLong(.Parameters.Item("original_suppress_recoveries").Value)
            End With


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally



        End Try
        Return result
    End Function
    'Qbenz005
    Public Function CreateReverseTransactions(ByRef lClaim_id As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CreateReverseTransactions"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vResults As Object
        Dim lStatsFolderCnt As Integer
        Dim vResultArray As Object
        Dim bReversalDone As Boolean

        Try
            Dim oObject As bControlTransClaims.Automated



            result = gPMConstants.PMEReturnCode.PMTrue


            oObject = New bControlTransClaims.Automated
            m_lReturn = oObject.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            oObject.ClaimID = lClaim_id

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = AddInputParameter(v_sName:="claim_id", v_vValue:=lClaim_id, v_iType:=gPMConstants.PMEDataType.PMLong)

            m_oDatabase.Parameters.Add(sName:="do_reversal", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_oDatabase.Parameters.Add(sName:="Stats_folder_cnt_new", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kReverseStstsTransactionsSQL, sSQLName:=kReverseStstsTransactionsName, bStoredProcedure:=True)


            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kReverseStstsTransactionsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            bReversalDone = gPMFunctions.ToSafeBoolean(m_oDatabase.Parameters.Item("Do_Reversal").Value)
            lStatsFolderCnt = gPMFunctions.ToSafeLong(m_oDatabase.Parameters.Item("Stats_folder_cnt_new").Value)

            If bReversalDone Then


                m_lReturn = oObject.FinaliseStats(gPMFunctions.ToSafeLong(lStatsFolderCnt, 0), False)


                m_lReturn = oObject.CreateTransactions(lStatsFolderCnt, 0)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
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

    Public Function RepostClaimTransactions(ByRef lClaim_id As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "RepostClaimTransactions"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vResults As Object
        Dim lStatsFolderCnt As Integer
        Dim vResultArray(,) As Object
        Dim bReversalDone As Boolean
        Try
            Dim oObject As bControlTransClaims.Automated



            result = gPMConstants.PMEReturnCode.PMTrue


            oObject = New bControlTransClaims.Automated
            m_lReturn = oObject.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            oObject.ClaimID = lClaim_id

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = AddInputParameter(v_sName:="copy_claim_id", v_vValue:=lClaim_id, v_iType:=gPMConstants.PMEDataType.PMLong)


            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:="spu_add_claims_stats_details_reins_process", sSQLName:=kReverseStstsTransactionsName, bStoredProcedure:=True, vResultArray:=vResultArray)




            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kRepostTransactionsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Informations.IsArray(vResultArray) Then

                For iLoop As Integer = 0 To vResultArray.GetUpperBound(1)


                    m_lReturn = oObject.FinaliseStats(gPMFunctions.ToSafeLong(vResultArray(0, iLoop), 0), False)


                    m_lReturn = oObject.CreateTransactions(vResultArray(0, iLoop), 0)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Next
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


    ' ***************************************************************** '
    ' Name: IsClaimReversalRequired
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : Gaurav Arora : 10-04-2007
    ' ***************************************************************** '
    Public Function IsClaimReversalRequired(ByVal v_lClaimId As Integer, ByRef r_bClaimReversalRequired As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "IsClaimReversalRequired"

        Dim lReturn As gPMConstants.PMEReturnCode
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = AddInputParameter(v_sName:="claim_id", v_vValue:=v_lClaimId, v_iType:=gPMConstants.PMEDataType.PMLong)
            ' m_lReturn = AddInputParameter(v_sName:="reversal_required", v_vValue:=0, v_iType:=PMinteger)
            m_oDatabase.Parameters.Add(sName:="reversal_required", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kIsReversalRequiredSQL, sSQLName:=kIsReversalRequiredName, bStoredProcedure:=True)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kIsReversalRequiredSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            r_bClaimReversalRequired = gPMFunctions.ToSafeBoolean(m_oDatabase.Parameters.Item("reversal_required").Value)

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


    ' ***************************************************************** '
    ' Name: GetClaimRIArrangementDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : Deepak Arora : 10-04-2007
    ' ***************************************************************** '
    Public Function GetClaimRIArrangementDetails(ByVal v_lClaimId As Integer, ByRef r_lVersionId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimRIArrangementDetails"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vResultArray(,) As Object
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = AddInputParameter(v_sName:="claim_id", v_vValue:=v_lClaimId, v_iType:=gPMConstants.PMEDataType.PMLong)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetClaimRiArrangementSQL, sSQLName:=kGetClaimRiArrangementName, bStoredProcedure:=True, vResultArray:=vResultArray)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kIsReversalRequiredSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Informations.IsArray(vResultArray) Then
                r_lVersionId = gPMFunctions.ToSafeLong(vResultArray(15, 0), 0)
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

    ' ***************************************************************** '
    ' Name: UpdateClaimEvents (Public)
    '
    ' Description: Update event record.
    '
    ' ***************************************************************** '
    Public Function UpdateClaimEvents(ByVal v_lClaimId As Integer) As Integer

        Dim result As Integer = 0
        Dim lPartyCnt, lInsuranceFolderCnt, lInsuranceFileCnt As Integer
        Dim vArray(,) As Object


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetCountsSQL, sSQLName:=ACGetCountsName, bStoredProcedure:=ACGetCountsStored, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            lPartyCnt = CInt(vArray(0, 0))

            lInsuranceFolderCnt = CInt(vArray(1, 0))

            lInsuranceFileCnt = CInt(vArray(2, 0))


            vArray = Nothing

            If m_oEvent Is Nothing Then


                m_oEvent = New bSIREvent.Business
                m_lReturn = m_oEvent.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the event object", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateClaimEvents", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result
                End If
            End If


            m_lReturn = m_oEvent.UpdateClaimEvents(vPartyCnt:=lPartyCnt, vInsuranceFolderCnt:=lInsuranceFolderCnt, vInsuranceFileCnt:=lInsuranceFileCnt, vClaimCnt:=v_lClaimId)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add the event", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateClaimEvents", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateClaimEvents Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateClaimEvents", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    Public Function GetClaimOldPolicy(ByVal v_lClaimId As Integer, ByRef r_sClaimOldPolicyRef As String) As Integer

        Dim result As Integer = 0
        Dim lPartyCnt, lInsuranceFolderCnt, lInsuranceFileCnt As Integer
        Dim vArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetCountsSQL, sSQLName:=ACGetCountsName, bStoredProcedure:=ACGetCountsStored, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            lPartyCnt = CInt(vArray(0, 0))

            lInsuranceFolderCnt = CInt(vArray(1, 0))

            lInsuranceFileCnt = CInt(vArray(2, 0))


            vArray = Nothing


            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_folder_cnt", vValue:=CStr(lInsuranceFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_old_policy_ref", vValue:=r_sClaimOldPolicyRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetClaimOldPolicySQL, sSQLName:=ACGetClaimOldPolicyName, bStoredProcedure:=ACGetClaimOldPolicyStored, bKeepNulls:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            r_sClaimOldPolicyRef = gPMFunctions.NullToString(m_oDatabase.Parameters.Item("claim_old_policy_ref").Value)


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimOldPolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimOldPolicy", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdatePaymentDocumentDetails
    '
    ' Description: Recording of Panel Solicitors Reference against a payment/fee (WR08)
    '
    ' ***************************************************************** '
    Public Function UpdatePaymentReference(ByVal v_lDocument_Id As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdatePaymentReference"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = AddInputParameter(v_sName:="document_id", v_vValue:=v_lDocument_Id, v_iType:=gPMConstants.PMEDataType.PMLong)

            ' Execute Action Query
            If m_oDatabase.SQLAction(sSQL:=kUpdCLMPaymentRefSQL, sSQLName:=kUpdCLMPaymentRefName, bStoredProcedure:=kUpdCLMPaymentRefStored) <> gPMConstants.PMEReturnCode.PMTrue Then

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdatePaymentReference failed to run the Stored Proc.", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePaymentReference", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If
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
    Public Function ChangeClaimStatusForSAM() As Integer
        Dim Catch_Renamed As Boolean = False

        Dim result As Integer = 0
        Dim sClaimOldPolicyRef As String = ""

        Const kMethodName As String = "ChangeClaimStatusForSAM"

        'Const kSpoolSilentMode As Integer = 4
        'Const kClaimIsFullyProcessed As Integer = 0

        Dim lReturn, lEventType As Integer
        Dim sDescription As String = ""
        Dim lClaimDateChanged As gPMConstants.PMEReturnCode
        Dim lAddTask As gPMConstants.PMEReturnCode
        Dim bUserAuthorised As Boolean
        Dim bSavedStats As Boolean
        Dim sClaimVersionDescription As String
        Dim lDocumentId As Integer

        Try
            Catch_Renamed = True



            result = gPMConstants.PMEReturnCode.PMTrue

            lClaimDateChanged = gPMConstants.PMEReturnCode.PMFalse
            lAddTask = gPMConstants.PMEReturnCode.PMFalse

            bSavedStats = True

            ' determine event type from transaction type
            lEventType = PMBConst.PMBEventClaChange

            bUserAuthorised = True


            ' Update insurance_file_system table before doing transactions.
            lReturn = UpdateInsuranceFileSystem(m_lClaimId)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.RaiseError(kMethodName, "bCLMChangeClaimStatus.UpdateInsuranceFileSystem Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Raise Transactions
            lReturn = RaiseTransactions(v_lClaimId:=m_lClaimId, v_bSavedStats:=bSavedStats, r_lDocumentId:=lDocumentId, r_bFromSAM:=True, PerilId:=0)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.RaiseError(kMethodName, "bCLMChangeClaimStatus.RaiseTransactions Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            ' reset the referred payment

            lReturn = CInt(SetPaymentReferred(m_lClaimId, 0))
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'uday
                'On Error GoTo LogError
                gPMFunctions.RaiseError(kMethodName, "SetPaymentReferred Failed", gPMConstants.PMELogLevel.PMLogError)
                result = gPMConstants.PMEReturnCode.PMFalse
                Catch_Renamed = True
            Else

                sClaimVersionDescription = "Payment of Claim - Payment Authorised"
            End If

            Return result

        Catch excep As System.Exception
            If Not Catch_Renamed Then
                Throw excep
            End If




            If Catch_Renamed Then


                ' DO Not Call any functions before here or the error will be lost
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ChangeClaimStatusForSAM Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ChangeClaimStatusForSAM", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                ' If you want to rollback a transaction or something, do it here

                Return result


                Return result
            End If
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetProductDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : Prabodh
    ' ***************************************************************** '
    Public Function GetProductDetails(ByVal v_lProductId As Integer, ByRef r_bPaymentRefCheck As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetProductDetails"

        Const kIPaymentRefCheck As Integer = 8

        Dim lReturn As Integer
        Dim vResultArray(,) As Object
        Dim oBusiness As bSIRProduct.Business
        Dim bCloseDatabase As Boolean
        Dim oDatabase As Object
        Dim lProductID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'open architecture database

            If gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_bNewInstanceCreated:=bCloseDatabase, r_oCheckedDatabase:=oDatabase, v_vDatabase:=oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, "CheckDatabase Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            oBusiness = New bSIRProduct.Business
            If oBusiness.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, "Failed to get an instance of bSIRProduct.Business", gPMConstants.PMELogLevel.PMLogError)
            End If

            'get product details

            m_lReturn = oBusiness.GetProductLevelOptionsForClaim(v_lClaimID:=v_lProductId, r_bIs_Payment_Ref_Check_Enabled:=r_bPaymentRefCheck)
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetProductDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            oBusiness.Dispose()


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here

        Finally


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetClaimStatus
    '
    ' Parameters: n/a
    '
    ' Description: Get Status of claim
    '
    ' History:
    '           PN-71999 - Sushil Kumar
    ' ***************************************************************** '

    Public Function GetClaimStatus(ByVal v_lClaimId As Integer, ByRef r_sStatus As String) As Integer
        Dim result As Integer = 0
        Dim vArray(,) As Object

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            With m_oDatabase
                .Parameters.Clear()

                ' add claim id parameter
                m_lReturn = .Parameters.Add(sName:="Claim_ID", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimStatus failed to add Claim_ID parameter.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimStatus", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                ' run SP
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetClaimStatusSQL, sSQLName:=ACGetClaimStatusName, bStoredProcedure:=ACGetClaimStatusStored, vResultArray:=vArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimStatus failed to run the Stored Proc.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimStatus", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If
            End With

            If Not Informations.IsArray(vArray) Then
                r_sStatus = ""
                Return result
            End If

            r_sStatus = CStr(vArray(0, 0)).Trim()

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    ''' <summary>
    ''' To create stats folder
    ''' </summary>
    ''' <param name="v_sTransactionTypeCode"></param>
    ''' <param name="v_nClaimId"></param>
    ''' <param name="r_nStatsFolderCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CreateStatsFolder(ByVal v_sTransactionTypeCode As String,
                                    ByVal v_lClaimId As Integer,
                                    ByRef r_lStatsFolderCnt As Integer) As Integer

        Dim lRecordsAffected As Long
        Dim result As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue
        Try

            With m_oDatabase
                .Parameters.Clear()

                ' add claim id parameter
                m_lReturn = .Parameters.Add(sName:="stats_folder_cnt", vValue:=CStr("0"), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimStatus failed to add Claim_ID parameter.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimStatus", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If
                m_lReturn = .Parameters.Add(sName:="transaction_type_code", vValue:=CStr(v_sTransactionTypeCode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimStatus failed to add Claim_ID parameter.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimStatus", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                m_lReturn = .Parameters.Add(sName:="user_id", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimStatus failed to add Claim_ID parameter.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimStatus", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                m_lReturn = .Parameters.Add(sName:="claim_id", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimStatus failed to add Claim_ID parameter.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimStatus", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If
                ' run SP
                m_lReturn = BeginTrans()

                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddStatsFolderSQL, sSQLName:=ACAddStatsFolderName, bStoredProcedure:=True, lRecordsAffected:=lRecordsAffected)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimStatus failed to run the Stored Proc.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimStatus", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

            End With

            ' Get the Cnt of the record inserted
            r_lStatsFolderCnt = m_oDatabase.Parameters.Item("stats_folder_cnt").Value

            If (r_lStatsFolderCnt < 1) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = RollbackTrans()
            End If

            result = CommitTrans()

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
        Return result
    End Function

    ''' <summary>
    ''' To get claim tax amount for a tax type
    ''' </summary>
    ''' <param name="r_oResults"></param>
    ''' <param name="v_nClaimPaymentId"></param>
    ''' <param name="v_nClaimReceiptId"></param>
    ''' <param name="v_nClaimReceiptItemId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetClaimTaxAmountsByTaxType(
                            ByRef r_vResults(,) As Object,
                   Optional ByVal v_iClaimPaymentId As Integer = 0,
                   Optional ByVal v_iClaimReceiptId As Integer = 0,
                   Optional ByVal v_iClaimReceiptItemId As Integer = 0) As Integer

        Const kMethodName As String = "GetClaimTaxAmountsByTaxType"

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase
            .Parameters.Clear()

            If v_iClaimPaymentId <> 0 Then
                m_lReturn = .Parameters.Add(sName:="claim_payment_id", vValue:=CStr(v_iClaimPaymentId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimStatus failed to add Claim_ID parameter.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimStatus", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If
            End If

            If v_iClaimReceiptId <> 0 Then
                m_lReturn = .Parameters.Add(sName:="claim_receipt_id", vValue:=CStr(v_iClaimReceiptId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimStatus failed to add Claim_ID parameter.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimStatus", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If
            End If
            If v_iClaimReceiptItemId <> 0 Then
                m_lReturn = .Parameters.Add(sName:="claim_receipt_item_id", vValue:=CStr(v_iClaimReceiptItemId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimStatus failed to add Claim_ID parameter.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimStatus", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If
            End If
            ' Execute selection Query

            If m_oDatabase.SQLSelect(sSQL:=kGetClaimTaxAmountsByTaxTypeSQL, sSQLName:=kGetClaimTaxAmountsByTaxTypeName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResults) <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kGetClaimTaxAmountsByTaxTypeSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With
        Return result
    End Function

    ''' <summary>
    '''  To create stats detail
    ''' </summary>
    ''' <param name="v_lStatsFolderCnt"></param>
    ''' <param name="v_lClaimId"></param>
    ''' <param name="v_sStatsDetailType"></param>
    ''' <param name="sCreditAccountCode"></param>
    ''' <param name="lClaimPaymentID"></param>
    ''' <param name="r_bThisRevesionPresent"></param>
    ''' <param name="dTaxamount"></param>
    ''' <param name="v_sTransactionTypeCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CreateStatsDetails(ByVal v_lStatsFolderCnt As Long,
                                       ByVal v_lClaimId As Long,
                                       ByVal v_sStatsDetailType As String,
                                       ByVal sCreditAccountCode As String,
                                       ByRef lClaimPaymentID As Long,
                                        ByRef r_bThisRevesionPresent As Boolean,
                                       Optional ByVal dTaxamount As Double = 0,
                                       Optional ByVal v_sTransactionTypeCode As String = "", Optional ByVal PerilId As Integer = 0) As Long


        Dim nRecordsAffected As Integer
        Dim nResult As Integer = 0
        nResult = gPMConstants.PMEReturnCode.PMTrue

        Try

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="claimpayment_id",
                                               vValue:=0,
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput,
                                               iDataType:=gPMConstants.PMEDataType.PMLong)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ThisRevesionPresent",
                                                    vValue:=0,
                                                    iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput,
                                                    iDataType:=gPMConstants.PMEDataType.PMLong)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If
            ' Add StatsFolderCnt as an INPUT param for an insert
            m_lReturn = m_oDatabase.Parameters.Add(sName:="stats_folder_cnt",
                                                   vValue:=v_lStatsFolderCnt,
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMLong)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            If v_sTransactionTypeCode <> "" Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_type_code",
                                                   vValue:=v_sTransactionTypeCode,
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMString)

            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_type_code",
                                                      vValue:=m_sTransactionType,
                                                      iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                      iDataType:=gPMConstants.PMEDataType.PMString)
            End If

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_id",
                                                   vValue:=v_lClaimId,
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMLong)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="StatsDetailType",
                                                  vValue:=v_sStatsDetailType,
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMString)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="CreditAccountCode",
                                                  vValue:=sCreditAccountCode,
                                                  iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                  iDataType:=gPMConstants.PMEDataType.PMString)

            If PerilId > 0 Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="PerilId",
                                              vValue:=PerilId,
                                              iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                              iDataType:=gPMConstants.PMEDataType.PMLong)
            End If


            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            If v_sStatsDetailType = "TAG" Or v_sStatsDetailType = "TAN" Then

                m_lReturn = m_oDatabase.Parameters.Add(sName:="TaxAmount",
                                                      vValue:=dTaxamount,
                                                      iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                      iDataType:=gPMConstants.PMEDataType.PMDouble)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If
            End If


            m_lReturn = BeginTrans()

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddStatsDetailsSQL,
                                              sSQLName:=ACAddStatsDetailsName,
                                              bStoredProcedure:=True,
                                              lRecordsAffected:=nRecordsAffected)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            If v_sTransactionTypeCode = "" Then

                r_bThisRevesionPresent = ToSafeBoolean(m_oDatabase.Parameters.Item("ThisRevesionPresent").Value)

                If m_sTransactionType = "C_CP" Or m_sTransactionType = "C_SA" Or m_sTransactionType = "C_RV" Then
                    lClaimPaymentID = m_oDatabase.Parameters.Item("ClaimPayment_Id").Value
                    If (lClaimPaymentID < 1) Then
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                        m_lReturn = RollbackTrans()
                        Exit Function
                    End If
                End If
            End If
            nResult = CommitTrans()
        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try
        Return nResult

    End Function

    'WPR022
    Public Function GetClaimTransactionType(ByVal v_lClaimId As Long,
                                              ByRef r_vResults As Object) As Long

        Const kMethodName As String = "GetClaimTransactionType"
        Dim iReturn As gPMConstants.PMEReturnCode
        Dim vResults As Object
        iReturn = gPMConstants.PMEReturnCode.PMTrue

        Try
            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' claim id
            iReturn = AddInputParameter(v_sName:="claim_id", v_vValue:=v_lClaimId, v_iType:=gPMConstants.PMEDataType.PMLong)

            If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, kMethodName & " failed to add Claim_ID parameter.", gPMConstants.PMELogLevel.PMLogError)
                Return iReturn
            End If

            ' Execute selection Query
            iReturn = m_oDatabase.SQLSelect(
                                    sSQL:=kGetClaimTransactionTypeSQL,
                                    sSQLName:=kGetClaimTransactionTypeName,
                                    bStoredProcedure:=True,
                                    vResultArray:=vResults,
                                    lNumberRecords:=gPMConstants.PMAllRecords)
            If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, kGetClaimTransactionTypeSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
                Return iReturn
            End If

            r_vResults = vResults
            Return iReturn
        Catch ex As Exception
            iReturn = gPMConstants.PMEReturnCode.PMError
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(
                 v_sUsername:=m_sUsername,
                 v_sClass:=ACClass,
                 v_sMethod:=kMethodName,
                r_lFunctionReturn:=GetClaimTransactionType,
                excep:=ex)

            ' If you want to rollback a transaction or something, do it here
            Return iReturn
        End Try

    End Function

    ''' <summary>
    ''' RaiseClonedTransactions
    ''' </summary>
    ''' <param name="nClaimId"></param>
    ''' <param name="nStatsFolderCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function RaiseClonedTransactions(ByVal nClaimId As Integer, ByVal nStatsFolderCnt As Integer) As Integer

        Dim oObject As bControlTransClaims.Automated
        Dim dtArray As DataTable
        Dim bStatsSuppressed As Boolean
        Dim nResult As Integer
        Dim oAllocationManual As bACTAllocationManual.Business
        Dim dtCreditTransactions As DataTable
        Dim vKeyArray(1, 3) As Object
        Dim nDocumentId As Integer
        Dim vTrans(0) As Object
        Dim dtTransactions As DataTable

        Try

            nResult = PMEReturnCode.PMTrue

            oObject = New bControlTransClaims.Automated
            m_lReturn = oObject.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            oObject.ClaimID = nClaimId

            'Need to get the transaction type id from the code...
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=m_sTransactionType,
                                                   iDirection:=PMEParameterDirection.PMParamInput,
                                                   iDataType:=PMEDataType.PMString)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.ExecuteDataTable(sSQL:=ACGetTransactionTypeIDSQL,
                                                     sSQLName:=ACGetTransactionTypeIDName, bStoredProcedure:=True,
                                                     oRecordset:=dtArray)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            If dtArray IsNot Nothing AndAlso dtArray.Rows.Count > 0 Then
                oObject.TransactionTypeID = dtArray.Rows(0).Item(0)
            Else
                Return PMEReturnCode.PMFalse
            End If

            oObject.TransactionTypeCode = m_sTransactionType

            If IsCloneReversal Then
                oObject.DocumentTypeID = kClonedReversedDocumentTypeId
                oObject.IsClonedReversal = 1
            End If

            ' finalise the stats folders details and determine whether
            ' the transactions should be suppressed
            m_lReturn = oObject.FinaliseStats(nStatsFolderCnt, bStatsSuppressed)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            ' if the stats have not been suppressed
            If Not bStatsSuppressed Then

                m_lReturn = oObject.CreateTransactions(nStatsFolderCnt, nDocumentId)

                If m_lReturn = PMEReturnCode.PMNotFound Then
                    Return PMEReturnCode.PMNotFound
                ElseIf m_lReturn <> PMEReturnCode.PMTrue Then
                    Return PMEReturnCode.PMFalse
                End If

            End If

            'Get an instance of bACTAllocationManual component to do the allocation
            oAllocationManual = New bACTAllocationManual.Business
            m_lReturn = oAllocationManual.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="nDocument_id", vValue:=nDocumentId,
                                                   iDirection:=PMEParameterDirection.PMParamInput,
                                                   iDataType:=PMEDataType.PMInteger)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.ExecuteDataTable(sSQL:=kGetTransDetailsSQL, sSQLName:=kGetTransDetailsName,
                                                     bStoredProcedure:=True, oRecordset:=dtTransactions)

            If m_lReturn = PMEReturnCode.PMNotFound Then
                Return PMEReturnCode.PMNotFound
            ElseIf m_lReturn <> PMEReturnCode.PMTrue AndAlso m_lReturn <> PMEReturnCode.PMNotFound Then
                Return PMEReturnCode.PMFalse
            End If

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="nStatsFolderCnt", vValue:=nStatsFolderCnt,
                                                    iDirection:=PMEParameterDirection.PMParamInput,
                                                    iDataType:=PMEDataType.PMInteger)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="nClaimId", vValue:=nClaimId,
                                                   iDirection:=PMEParameterDirection.PMParamInput,
                                                   iDataType:=PMEDataType.PMInteger)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="sTransactionType", vValue:=m_sTransactionType,
                                                  iDirection:=PMEParameterDirection.PMParamInput,
                                                  iDataType:=PMEDataType.PMString)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.ExecuteDataTable(sSQL:=kGetRITransDetailsSQL, sSQLName:=kGetRITransDetailsName,
                                                     bStoredProcedure:=True, oRecordset:=dtCreditTransactions)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If
            If dtTransactions IsNot Nothing AndAlso dtTransactions.Rows.Count > 0 Then
                For iCount As Integer = 0 To dtTransactions.Rows.Count - 1

                    For iRow As Integer = 0 To dtCreditTransactions.Rows.Count - 1

                        If dtTransactions.Rows(iCount).Item(0) = dtCreditTransactions.Rows(iRow).Item(0) Then
                            If Math.Abs(CDbl(dtTransactions.Rows(iCount).Item(2))) = Math.Abs(CDbl(dtCreditTransactions.Rows(iRow).Item(2))) AndAlso
                                dtTransactions.Rows(iCount).Item(3) = dtCreditTransactions.Rows(iRow).Item(3) Then

                                vTrans(0) = dtTransactions.Rows(iCount).Item(1) & "|" &
                                            dtTransactions.Rows(iCount).Item(2)

                                vKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 0) = "account_id"
                                vKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 0) =
                                    dtCreditTransactions.Rows(iRow).Item(0)

                                vKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 1) = "trans_detail_id"
                                vKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 1) =
                                    dtCreditTransactions.Rows(iRow).Item(1) & "|" &
                                    dtCreditTransactions.Rows(iRow).Item(2)

                                vKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 2) = "trans_detail_ids"
                                vKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 2) = vTrans

                                m_lReturn = oAllocationManual.SetKeys(vKeyArray:=vKeyArray)

                                If m_lReturn <> PMEReturnCode.PMTrue Then
                                    Return PMEReturnCode.PMFalse
                                End If

                                m_lReturn = oAllocationManual.Start()

                                If m_lReturn <> PMEReturnCode.PMTrue Then
                                    Return PMEReturnCode.PMFalse
                                End If
                            End If
                        End If
                    Next
                Next
            End If

        Catch excep As Exception

            nResult = PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError,
                               sMsg:="RaiseClonedTransactions Failed", vApp:=ACApp, vClass:=ACClass,
                               vMethod:="RaiseClonedTransactions", vErrNo:=Informations.Err().Number,
                               vErrDesc:=excep.Message, excep:=excep)
        Finally
            If oObject IsNot Nothing Then
                oObject.Dispose()
                oObject = Nothing
            End If

            If oAllocationManual IsNot Nothing Then
                oAllocationManual.Dispose()
                oAllocationManual = Nothing
            End If
        End Try

        Return nResult

    End Function

    ''' <summary>
    ''' Used to get the Payment script configuration from the product maintenance.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetIsPaymentsReadOnly() As Boolean
        Dim nProductID As Integer
        Dim oProduct As bSIRProduct.Business = Nothing
        Dim oIsPaymentsReadonly(,) As Object = Nothing
        Dim bReturnValue As Boolean = False
        Dim nInsuranceFileCnt As Integer

        If GetInsuranceFileDetails(m_lClaimId, nInsuranceFileCnt, 0, 0) <> 1 OrElse nInsuranceFileCnt = 0 Then
            Return bReturnValue
        End If

        m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oProduct, v_sClassName:="bSIRProduct.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New ApplicationException("Failed to get Product Business  object.")
        End If

        m_lReturn = oProduct.GetProductid(ifilecnt:=nInsuranceFileCnt, vProduct_id:=nProductID)
        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            m_lReturn = oProduct.GetProductValue(nProductID, "is_Payments_read_only", oIsPaymentsReadonly)
        End If

        If Informations.IsArray(oIsPaymentsReadonly) AndAlso (oIsPaymentsReadonly(0, 0)).ToString.Trim = "1" Then
            bReturnValue = True
        End If
        oProduct.Dispose()
        oProduct = Nothing

        Return bReturnValue

    End Function
    ''' <summary>
    ''' this will be used to get the insurance details.
    ''' </summary>
    ''' <param name="nClaimId"></param>
    ''' <param name="r_nInsurance_file_cnt"></param>
    ''' <param name="r_nInsurance_folder_cnt"></param>
    ''' <param name="r_nParty_cnt"></param>
    ''' <returns></returns>
    Function GetInsuranceFileDetails(ByVal nClaimId As Integer, ByRef r_nInsurance_file_cnt As Integer,
                                    ByRef r_nInsurance_folder_cnt As Integer,
                                    ByRef r_nParty_cnt As Integer) As Integer
        Dim oArray(,) As Object

        Try
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=nClaimId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetCountsSQL, sSQLName:=ACGetCountsName, bStoredProcedure:=ACGetCountsStored, vResultArray:=oArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(oArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            r_nParty_cnt = ToSafeInteger(oArray(0, 0))
            r_nInsurance_folder_cnt = ToSafeInteger(oArray(1, 0))
            r_nInsurance_file_cnt = ToSafeInteger(oArray(2, 0))

            m_lReturn = gPMConstants.PMEReturnCode.PMTrue
        Catch excep As Exception
            ' Log Error Message
            m_lReturn = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError,
                               sMsg:="GetInsuranceFileDetails Failed", vApp:=ACApp, vClass:=ACClass,
                               vMethod:="GetInsuranceFileDetails", excep:=excep)
        End Try
        Return m_lReturn
    End Function
    ''' <summary>
    ''' Used to get the reserve script configuration from the product maintenance.
    ''' </summary>
    ''' <returns></returns>
    Private Function GetIsReservesReadOnly() As Boolean
        Dim nProductID As Integer
        Dim oProduct As bSIRProduct.Business = Nothing
        Dim oIsReservesReadonly As Object = Nothing
        Dim bReturnValue As Boolean = False
        Dim nInsuranceFileCnt As Integer

        If GetInsuranceFileDetails(m_lClaimId, nInsuranceFileCnt, 0, 0) <> 1 OrElse nInsuranceFileCnt = 0 Then
            Return bReturnValue
        End If

        m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oProduct, v_sClassName:="bSIRProduct.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New ApplicationException("Failed to get bSIRProduct Business object.")
        End If

        If oProduct.GetProductid(ifilecnt:=nInsuranceFileCnt, vProduct_id:=nProductID) <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New ApplicationException("Call to oProduct.GetProductid failed.")
        End If
        If oProduct.GetProductValue(nProductID, "is_reserves_read_only", oIsReservesReadonly) <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New ApplicationException("Call to oProduct.GetProductValue failed.")
        End If

        If Informations.IsArray(oIsReservesReadonly) AndAlso CStr(oIsReservesReadonly(0, 0)) = "1" Then
            bReturnValue = True
        End If

        oProduct.Dispose()
        oProduct = Nothing
        oIsReservesReadonly = Nothing

        Return bReturnValue
    End Function

    Public Function GetUserIDForTaskCompleteIntimation(ByVal v_lClaimID As Integer, ByRef v_lPreviousUserID As Integer, ByRef v_sUserName As String, ByRef v_dReserveEntered As Decimal) As Integer
        Dim result As Integer = 0
        Dim sSQL As String = String.Empty
        Dim vResults(,) As Object = Nothing
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=v_lClaimID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="TransactionType", vValue:=m_sTransactionType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetDetailsForCompletionIntimationSQL, sSQLName:=ACGetDetailsForCompletionIntimation, bStoredProcedure:=True, vResultArray:=vResults)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            v_lPreviousUserID = gPMFunctions.ToSafeInteger(vResults(0, 0))
            v_sUserName = gPMFunctions.ToSafeString(vResults(1, 0))
            v_dReserveEntered = gPMFunctions.ToSafeDecimal(vResults(2, 0))

        Catch excep As Exception
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to retrieve the supervisor user group.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSupervisorUserGroup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        End Try
        Return result
    End Function
End Class