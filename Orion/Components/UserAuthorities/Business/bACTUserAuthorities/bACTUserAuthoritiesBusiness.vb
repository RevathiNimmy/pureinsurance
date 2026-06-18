Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
Imports System.Text
'developer guide no 129. 
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable

    '*******************************************************************************
    ' Class Name:  Business
    '
    ' Date:        14/02/2000
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a ACTUserAuthorities.
    '
    ' Edit History:
    '
    ' AMB 05/02/2003 - Added parameters for IAG 220 Manage Debtors development
    '
    ' PW 091204 - PN16854 - In IsValidAmount change check to allow up to and
    '                      including the write off authority limit. Previously would
    '                      not allow a write off equalling the limit.
    ' CJB090805 - PN 23035 - New Client Manager Security option for 'Can Delete Policy'.
    '                      Changed GetNext, EditAdd, EditUpdate & DirectAdd.
    '
    '*******************************************************************************


    ' ************************************************
    ' Added to replace global variables 24/09/2004
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

    ' Collection of ACTUserAuthoritiess (Private)
    Private m_oACTUserAuthoritiess As bACTUserAuthorities.ACTUserAuthoritiess

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As Integer

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private lPMAuthorityLevel As Integer

    Private m_iCanBackdateCollectionDate As Integer
    ' Primary Keys to work with

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFOrion

        End Get
    End Property

    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)

            Value = Value

        End Set
    End Property

    Public Property CurrentRecord() As Integer
        Get

            Return m_lCurrentRecord

        End Get
        Set(ByVal Value As Integer)

            Select Case Value
                Case Is < 1
                    m_lCurrentRecord = 0
                Case Is > m_oACTUserAuthoritiess.Count()
                    m_lCurrentRecord = m_oACTUserAuthoritiess.Count()
                Case Else
                    m_lCurrentRecord = Value
            End Select

        End Set
    End Property

    Public ReadOnly Property RecordCount() As Integer
        Get

            ' Return Number in Collection
            Return m_oACTUserAuthoritiess.Count()

        End Get
    End Property


    Public Property UserID() As Integer
        Get

            Return m_iUserID

        End Get
        Set(ByVal Value As Integer)

            m_iUserID = Value

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


            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password

            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Create ACTUserAuthoritiess Collection
            m_oACTUserAuthoritiess = New bACTUserAuthorities.ACTUserAuthoritiess()


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
                m_oACTUserAuthoritiess = Nothing
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


    ' ***************************************************************** '
    ' Name: DirectAdd (Public)
    '
    ' Description: Adds a single ACTUserAuthorities directly into the database.
    '        Note: The ACTUserAuthorities will NOT be added to the collection.
    '
    ' AMB 05/02/2003 - Added parameters for IAG 220 Manage Debtors development
    ' 19/04/2005 - 2005 Staff Control Added New Client Manager Security fields
    ' 13/10/2005 - New Option to switch of Editing of Schemes Type policies'
    ' ***************************************************************** '
    Public Function DirectAdd(Optional ByRef vUserID As Object = Nothing, Optional ByRef vHasWriteOffAuthority As Object = Nothing, Optional ByRef vWriteOffAmount As Object = Nothing, Optional ByRef vHasUnrestrictedEnquiry As Object = Nothing, Optional ByRef vHasUnrestrictedUpdate As Object = Nothing, Optional ByRef vHasTransWriteOffAuthority As Object = Nothing, Optional ByRef vTransWriteOffAmount As Object = Nothing, Optional ByRef vHasRefundAuthority As Object = Nothing, Optional ByRef vHasTransferAuthority As Object = Nothing, Optional ByRef vIsViewClient As Object = Nothing, Optional ByRef vIsEditClient As Object = Nothing, Optional ByRef vIsEditPolicy As Object = Nothing, Optional ByRef vIsEditClaim As Object = Nothing, Optional ByRef vIsEditFinancePlan As Object = Nothing, Optional ByRef vIsRaiseDebit As Object = Nothing, Optional ByRef vIsRaiseCredit As Object = Nothing, Optional ByRef vIsRaiseFee As Object = Nothing, Optional ByRef vIsRaiseCash As Object = Nothing, Optional ByRef vIsReverseTransactions As Object = Nothing, Optional ByRef vIsReverseAllocations As Object = Nothing, Optional ByRef vIsRaiseManualDID As Object = Nothing, Optional ByRef vIsDeleteClient As Object = Nothing, Optional ByRef vIsPerformAllocations As Object = Nothing, Optional ByRef vCanPerformBrokerTransfer As Object = Nothing, Optional ByRef vIsDeletePolicy As Object = Nothing, Optional ByRef vIsEditSchemePolicy As Object = Nothing, Optional ByRef vCanReverseReplaceTransactions As Integer = 0, Optional ByRef vHasViewBatchProcessStatus As Object = Nothing) As Integer
        ' AMB 05/02/2003 - Added parameters above

        Dim result As Integer = 0
        Dim oACTUserAuthorities As bACTUserAuthorities.ACTUserAuthorities

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new ACTUserAuthorities
            oACTUserAuthorities = New bACTUserAuthorities.ACTUserAuthorities()
            m_lReturn = oACTUserAuthorities.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            ' Populate ACTUserAuthorities Attributes
            '19042005 2005 Staff Control Added New Client Manager Security Fields


























            'developer guide no. 98
            m_lReturn = oACTUserAuthorities.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMAdd, vUserID:=vUserID, vHasWriteOffAuthority:=vHasWriteOffAuthority, vWriteOffAmount:=vWriteOffAmount, vHasUnrestrictedEnquiry:=vHasUnrestrictedEnquiry, vHasUnrestrictedUpdate:=vHasUnrestrictedUpdate, vHasTransWriteOffAuthority:=vHasTransWriteOffAuthority, vTransWriteOffAmount:=vTransWriteOffAmount, vHasRefundAuthority:=vHasRefundAuthority, vHasTransferAuthority:=vHasTransferAuthority, vIsViewClient:=vIsViewClient, vIsEditClient:=vIsEditClient, vIsEditPolicy:=vIsEditPolicy, vIsEditClaim:=vIsEditClaim, vIsEditFinancePlan:=vIsEditFinancePlan, vIsRaiseDebit:=vIsRaiseDebit, vIsRaiseCredit:=vIsRaiseCredit, vIsRaiseFee:=vIsRaiseFee, vIsRaiseCash:=vIsRaiseCash, vIsReverseTransactions:=vIsReverseTransactions, vIsReverseAllocations:=vIsReverseAllocations, vIsRaiseManualDID:=vIsRaiseManualDID, vIsDeleteClient:=vIsDeleteClient, vIsPerformAllocations:=vIsPerformAllocations, vCanPerformBrokerTransfer:=vCanPerformBrokerTransfer, vIsDeletePolicy:=vIsDeletePolicy, vIsEditSchemePolicy:=vIsEditSchemePolicy, vViewBatchProcessStatus:=vHasViewBatchProcessStatus)
            ' AMB 05/02/2003 - Added parameters above

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oACTUserAuthorities = Nothing
                Return result
            End If

            ' Add the ACTUserAuthorities to the Database
            m_lReturn = oACTUserAuthorities.AddItem()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oACTUserAuthorities = Nothing
                Return result
            End If

            ' Retain the Primary Key of the ACTUserAuthorities Added
            With oACTUserAuthorities
                UserID = .UserID
            End With

            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}

            oACTUserAuthorities = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectDelete (Public)
    '
    ' Description: Deletes a single ACTUserAuthorities directly from the database.
    '        Note: The ACTUserAuthorities will NOT be deleted from the collection.
    '
    ' ***************************************************************** '

    Public Function DirectDelete() As Integer
        Return DirectDelete(vUserID:=Nothing)
    End Function

    Public Function DirectDelete(ByRef vUserID As Object) As Integer

        Dim result As Integer = 0
        Dim oACTUserAuthorities As bACTUserAuthorities.ACTUserAuthorities

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new ACTUserAuthorities
            oACTUserAuthorities = New bACTUserAuthorities.ACTUserAuthorities()
            m_lReturn = oACTUserAuthorities.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            ' Set ACTUserAuthorities Primary Key

            'developer guide no. 98
            m_lReturn = oACTUserAuthorities.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMDelete, vUserID:=vUserID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oACTUserAuthorities = Nothing
                Return result
            End If

            ' Delete the ACTUserAuthorities from the Database
            m_lReturn = oACTUserAuthorities.DeleteItem()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oACTUserAuthorities = Nothing
                Return result
            End If

            oACTUserAuthorities = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectDelete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckID (Public)
    '
    ' Description: Checks to see if the supplied ID is a valid record.
    '
    ' ***************************************************************** '
    Public Function CheckID(ByRef vID As Object) As Integer

        Dim result As Integer = 0
        Dim lSub As Integer = 0, lRecordCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the ID parameter (INPUT)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="id", vValue:=CStr(vID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckIDSQL, sSQLName:=ACCheckIDName, bStoredProcedure:=ACCheckIDStored, lNumberRecords:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            ' Do we have any records ?
            If lRecordCount < 1 Then
                ' No record found
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the required ACTUserAuthoritiess and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetDetails(Optional ByRef vLockMode As Integer = 0, Optional ByRef vUserID As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        'developer guide no 21. 
        Dim oFields As DataRow
        Dim oACTUserAuthorities As bACTUserAuthorities.ACTUserAuthorities

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Collection
            m_oACTUserAuthoritiess.Clear()

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Default to No Lock if not supplied or not numeric
            Dim dbNumericTemp As Double

            If (Informations.IsNothing(vLockMode)) Or (Not Double.TryParse(CStr(vLockMode), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                vLockMode = 0
            End If

            ' Check for Valid Primary Key
            Dim dbNumericTemp2 As Double

            If (Not Informations.IsNothing(vUserID)) And (Not Double.TryParse(CStr(vUserID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2)) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vUserID=" & vUserID, vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                Return result
            End If


            If Not Informations.IsNothing(vUserID) Then

                ' Create New ACTUserAuthorities
                oACTUserAuthorities = New bACTUserAuthorities.ACTUserAuthorities()
                m_lReturn = oACTUserAuthorities.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

                ' Set component primary keys
                With oACTUserAuthorities
                    .UserID = vUserID

                    m_lReturn = .SelectItem()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End With

                ' Add ACTUserAuthorities to collection
                If (m_oACTUserAuthoritiess.Count = 0) Then
                    m_oACTUserAuthoritiess.Add(Nothing)
                End If
                m_lReturn = m_oACTUserAuthoritiess.Add(oNewACTUserAuthorities:=oACTUserAuthorities)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                oACTUserAuthorities = Nothing

            Else

                ' No Key, Get All Records

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllDetailsSQL, sSQLName:=ACGetAllDetailsName, bStoredProcedure:=ACGetAllDetailsStored, lNumberRecords:=0)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' How many records were selected
                lRecordCount = m_oDatabase.Records.Count()

                ' Do we have any records ?
                If lRecordCount < 1 Then
                    ' No Records, return PMFalse
                    Return gPMConstants.PMEReturnCode.PMNotFound
                End If

                ' Yes, load them into the collection
                'Developer Guide No 162
                For lSub As Integer = 0 To lRecordCount - 1

                    ' Create New
                    oACTUserAuthorities = New bACTUserAuthorities.ACTUserAuthorities()
                    m_lReturn = Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)

                    ' Set oFields to refer to one Record
                    oFields = m_oDatabase.Records.Item(lSub).Fields()

                    ' Set component primary keys from current record
                    With oACTUserAuthorities
                        'AK 230702 check for null value

                        If Not (Convert.IsDBNull(oFields("user_id")) Or Informations.IsNothing(oFields("user_id"))) Then
                            .UserID = oFields("user_id")
                        Else
                            .UserID = 0
                        End If

                        m_lReturn = .SelectItem()

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End With

                    ' Add ACTUserAuthorities to collection
                    If (m_oACTUserAuthoritiess.Count = 0) Then
                        m_oACTUserAuthoritiess.Add(Nothing)
                    End If
                    m_lReturn = m_oACTUserAuthoritiess.Add(oNewACTUserAuthorities:=oACTUserAuthorities)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oACTUserAuthorities = Nothing
                Next lSub
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetNext (Public)
    '
    ' Description: Gets the required ACTUserAuthoritiess and populate the Collection
    '
    ' 19/04/2005 - 2005 Staff Control Added New Client Manager Security fields
    ' 13/10/2005 - New Option to switch of Editing of Schemes Type policies'
    ' ***************************************************************** '
    Public Function GetNext(Optional ByRef vUserID As Object = Nothing, Optional ByRef vHasWriteOffAuthority As Object = Nothing, Optional ByRef vWriteOffAmount As Object = Nothing, Optional ByRef vHasUnrestrictedEnquiry As Object = Nothing, Optional ByRef vHasUnrestrictedUpdate As Object = Nothing, Optional ByRef vFeeDiscount As Object = Nothing, Optional ByRef vHasTransWriteOffAuthority As Object = Nothing, Optional ByRef vTransWriteOffAmount As Object = Nothing, Optional ByRef vHasRefundAuthority As Object = Nothing, Optional ByRef vHasTransferAuthority As Object = Nothing, Optional ByRef vHasPaymentsAuthority As Object = Nothing, Optional ByRef vPaymentsAmount As Object = Nothing, Optional ByRef vHasClaimPaymentsAuthority As Object = Nothing, Optional ByRef vClaimPaymentsAmount As Object = Nothing, Optional ByRef vOverrideDate As Object = Nothing, Optional ByRef vOverrideRate As Object = Nothing, Optional ByRef vOverridePrePolicyDate As Object = Nothing, Optional ByRef vOverridePrePolicyRate As Object = Nothing, Optional ByRef vWriteOffCurrencyID As Object = Nothing, Optional ByRef vTransWriteOffCurrencyID As Object = Nothing, Optional ByRef vPaymentsCurrencyID As Object = Nothing, Optional ByRef vClaimPaymentsCurrencyID As Object = Nothing, Optional ByRef vIsViewClient As Object = Nothing, Optional ByRef vIsEditClient As Object = Nothing, Optional ByRef vIsEditPolicy As Object = Nothing, Optional ByRef vIsEditClaim As Object = Nothing, Optional ByRef vIsEditFinancePlan As Object = Nothing, Optional ByRef vIsRaiseDebit As Object = Nothing, Optional ByRef vIsRaiseCredit As Object = Nothing, Optional ByRef vIsRaiseFee As Object = Nothing, Optional ByRef vIsRaiseCash As Object = Nothing, Optional ByRef vIsReverseTransactions As Object = Nothing, Optional ByRef vIsReverseAllocations As Object = Nothing, Optional ByRef vIsRaiseManualDID As Object = Nothing, Optional ByRef vIsDeleteClient As Object = Nothing, Optional ByRef vIsPerformAllocations As Object = Nothing, Optional ByRef vCanPerformBrokerTransfer As Object = Nothing, Optional ByRef vDuplicateClaimOverride As Object = Nothing, Optional ByRef vIsDeletePolicy As Object = Nothing, Optional ByRef vIsEditSchemePolicy As Object = Nothing, Optional ByRef vCanMakeLiveInvoice As Integer = 0, Optional ByRef vCAnMakeLiveInstalments As Integer = 0, Optional ByRef vCanMakeLivePayNow As Integer = 0, Optional ByRef vHasPaynowWriteoffAuthority As Integer = 0, Optional ByRef vPaynowWriteOffCurrencyID As Integer = 0, Optional ByRef vPayNowWriteoffAmount As Decimal = 0, Optional ByRef vPaynowBankAccount As Integer = 0, Optional ByRef vPostingPeriod As Object = Nothing, Optional ByRef vUserCanChangeReserves As Object = Nothing, Optional ByRef vUserCanAddRemoveRatingSections As Object = Nothing, Optional ByRef vUserCanEditExistingRatingSections As Object = Nothing, Optional ByRef vIsViewClientManager As Object = Nothing, Optional ByRef vIsViewAgentMaintenance As Object = Nothing, Optional ByRef vIsViewAccountHandler As Object = Nothing, Optional ByRef vIsViewAccountExecutive As Object = Nothing, Optional ByRef vIsViewInsurerMaintenance As Object = Nothing, Optional ByRef vIsViewOtherParty As Object = Nothing, Optional ByRef vParams As Object = Nothing, Optional ByRef vCurrencyLossGainLimit As Object = Nothing, Optional ByRef vLossGainCurrencyID As Object = Nothing, Optional ByRef vHasManualJournalAuthority As Object = Nothing, Optional ByRef vManualJournalAmount As Double = 0, Optional ByRef vManualJournalCurrencyID As Object = Nothing, Optional ByRef vVoidTransaction As String = "") As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer = 0
        Dim oACTUserAuthorities As bACTUserAuthorities.ACTUserAuthorities
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oACTUserAuthoritiess.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord += 1
            Else
                Return gPMConstants.PMEReturnCode.PMEOF
            End If

            oACTUserAuthorities = m_oACTUserAuthoritiess.Item(m_lCurrentRecord)

            ' Get the ACTUserAuthorities Property Values
            ' 19/04/2005 - 2005 Staff Control Added New Client Manager Security fields



















































            'developer guide no. 98
            m_lReturn = oACTUserAuthorities.GetProperties(iStatus, vUserID:=vUserID, vHasWriteOffAuthority:=vHasWriteOffAuthority, vWriteOffAmount:=vWriteOffAmount, vHasUnrestrictedEnquiry:=vHasUnrestrictedEnquiry, vHasUnrestrictedUpdate:=vHasUnrestrictedUpdate, vFeeDiscount:=vFeeDiscount, vHasTransWriteOffAuthority:=vHasTransWriteOffAuthority, vTransWriteOffAmount:=vTransWriteOffAmount, vHasRefundAuthority:=vHasRefundAuthority, vHasTransferAuthority:=vHasTransferAuthority, vHasPaymentsAuthority:=vHasPaymentsAuthority, vPaymentsAmount:=vPaymentsAmount, vHasClaimPaymentsAuthority:=vHasClaimPaymentsAuthority, vClaimPaymentsAmount:=vClaimPaymentsAmount, vOverrideDate:=vOverrideDate, vOverrideRate:=vOverrideRate, vOverridePrePolicyDate:=vOverridePrePolicyDate, vOverridePrePolicyRate:=vOverridePrePolicyRate, vWriteOffCurrencyID:=vWriteOffCurrencyID, vTransWriteOffCurrencyID:=vTransWriteOffCurrencyID, vPaymentsCurrencyID:=vPaymentsCurrencyID, vClaimPaymentsCurrencyID:=vClaimPaymentsCurrencyID, vIsViewClient:=vIsViewClient, vIsEditClient:=vIsEditClient, vIsEditPolicy:=vIsEditPolicy, vIsEditClaim:=vIsEditClaim, vIsEditFinancePlan:=vIsEditFinancePlan, vIsRaiseDebit:=vIsRaiseDebit, vIsRaiseCredit:=vIsRaiseCredit, vIsRaiseFee:=vIsRaiseFee, vIsRaiseCash:=vIsRaiseCash, vIsReverseTransactions:=vIsReverseTransactions, vIsReverseAllocations:=vIsReverseAllocations, vIsRaiseManualDID:=vIsRaiseManualDID, vIsDeleteClient:=vIsDeleteClient, vIsPerformAllocations:=vIsPerformAllocations, vCanPerformBrokerTransfer:=vCanPerformBrokerTransfer, vDuplicateClaimOverride:=vDuplicateClaimOverride, vIsDeletePolicy:=vIsDeletePolicy, vIsEditSchemePolicy:=vIsEditSchemePolicy, vCanMakeLiveInvoice:=vCanMakeLiveInvoice, vCAnMakeLiveInstalments:=vCAnMakeLiveInstalments, vCanMakeLivePayNow:=vCanMakeLivePayNow, vHasPaynowWriteoffAuthority:=vHasPaynowWriteoffAuthority, vPaynowWriteOffCurrencyID:=vPaynowWriteOffCurrencyID, vPayNowWriteoffAmount:=vPayNowWriteoffAmount, vPaynowBankAccount:=vPaynowBankAccount, vPostingPeriod:=vPostingPeriod, vUserCanChangeReserves:=vUserCanChangeReserves, vUserCanAddRemoveRatingSections:=vUserCanAddRemoveRatingSections, vUserCanEditExistingRatingSections:=vUserCanEditExistingRatingSections, vIsViewClientManager:=vIsViewClientManager, vIsViewAgentMaintenance:=vIsViewAgentMaintenance, vIsViewAccountHandler:=vIsViewAccountHandler, vIsViewAccountExecutive:=vIsViewAccountExecutive, vIsViewInsurerMaintenance:=vIsViewInsurerMaintenance, vIsViewOtherParty:=vIsViewOtherParty, vParams:=vParams, vCurrencyLossGainLimit:=vCurrencyLossGainLimit, vLossGainCurrencyID:=vLossGainCurrencyID,
                                                            vHasManualJournalAuthority:=vHasManualJournalAuthority,
                                                    vManualJournalAmount:=vManualJournalAmount,
                                                    vManualJournalCurrencyID:=vManualJournalCurrencyID, vVoidTransaction:=vVoidTransaction)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oACTUserAuthorities = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNext", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditAdd (Public)
    '
    ' Description: Adds the supplied ACTUserAuthorities into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    '
    ' 19/04/2005 - 2005 Staff Control Added New Client Manager Security fields
    ' 13/10/2005 - New Option to switch of Editing of Schemes Type policies'
    ' ***************************************************************** '
    Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vUserID As Object = Nothing, Optional ByRef vHasWriteOffAuthority As Object = Nothing, Optional ByRef vWriteOffAmount As Object = Nothing, Optional ByRef vHasUnrestrictedEnquiry As Object = Nothing, Optional ByRef vHasUnrestrictedUpdate As Object = Nothing, Optional ByRef vHasTransWriteOffAuthority As Object = Nothing, Optional ByRef vTransWriteOffAmount As Object = Nothing, Optional ByRef vHasRefundAuthority As Object = Nothing, Optional ByRef vHasTransferAuthority As Object = Nothing, Optional ByRef vOverrideDate As Object = Nothing, Optional ByRef vOverrideRate As Object = Nothing, Optional ByRef vOverridePrePolicyDate As Object = Nothing, Optional ByRef vOverridePrePolicyRate As Object = Nothing, Optional ByRef vWriteOffCurrencyID As Object = Nothing, Optional ByRef vTransWriteOffCurrencyID As Object = Nothing, Optional ByRef vPaymentsCurrencyID As Object = Nothing, Optional ByRef vClaimPaymentsCurrencyID As Object = Nothing, Optional ByRef vIsViewClient As Object = Nothing, Optional ByRef vIsEditClient As Object = Nothing, Optional ByRef vIsEditPolicy As Object = Nothing, Optional ByRef vIsEditClaim As Object = Nothing, Optional ByRef vIsEditFinancePlan As Object = Nothing, Optional ByRef vIsRaiseDebit As Object = Nothing, Optional ByRef vIsRaiseCredit As Object = Nothing, Optional ByRef vIsRaiseFee As Object = Nothing, Optional ByRef vIsRaiseCash As Object = Nothing, Optional ByRef vIsReverseTransactions As Object = Nothing, Optional ByRef vIsReverseAllocations As Object = Nothing, Optional ByRef vIsRaiseManualDID As Object = Nothing, Optional ByRef vIsDeleteClient As Object = Nothing, Optional ByRef vIsPerformAllocations As Object = Nothing, Optional ByRef vCanPerformBrokerTransfer As Object = Nothing, Optional ByRef vIsDeletePolicy As Object = Nothing, Optional ByRef vIsEditSchemePolicy As Object = Nothing, Optional ByRef vCanMakeLiveInvoice As Integer = 0, Optional ByRef vCAnMakeLiveInstalments As Integer = 0, Optional ByRef vCanMakeLivePayNow As Integer = 0, Optional ByRef vHasPaynowWriteoffAuthority As Integer = 0, Optional ByRef vPaynowWriteOffCurrncyID As Integer = 0, Optional ByRef vPayNowWriteoffAmount As Decimal = 0, Optional ByRef vPaynowBankAccount As Integer = 0, Optional ByRef vCanReverseReplaceTransactions As Integer = 0, Optional ByRef vHasViewBatchProcessStatus As Object = Nothing, Optional ByRef vCurrencyLossGainLimit As Object = Nothing, Optional ByRef vLossGainCurrencyID As Object = Nothing, Optional ByRef vVoidTransaction As String = "") As Integer


        Dim result As Integer = 0
        Dim oACTUserAuthorities As bACTUserAuthorities.ACTUserAuthorities

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oACTUserAuthoritiess.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new ACTUserAuthorities
            oACTUserAuthorities = New bACTUserAuthorities.ACTUserAuthorities()
            m_lReturn = oACTUserAuthorities.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            ' Populate ACTUserAuthorities Attributes
            ' 19/04/2005 - 2005 Staff Control Added New Client Manager Security fields


































            'developer guide no. 98
            m_lReturn = oACTUserAuthorities.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMAdd, vUserID:=vUserID, vHasWriteOffAuthority:=vHasWriteOffAuthority, vWriteOffAmount:=vWriteOffAmount, vHasUnrestrictedEnquiry:=vHasUnrestrictedEnquiry, vHasUnrestrictedUpdate:=vHasUnrestrictedUpdate, vHasTransWriteOffAuthority:=vHasTransWriteOffAuthority, vTransWriteOffAmount:=vTransWriteOffAmount, vHasRefundAuthority:=vHasRefundAuthority, vHasTransferAuthority:=vHasTransferAuthority, vOverrideDate:=vOverrideDate, vOverrideRate:=vOverrideRate, vOverridePrePolicyDate:=vOverridePrePolicyDate, vOverridePrePolicyRate:=vOverridePrePolicyRate, vWriteOffCurrencyID:=vWriteOffCurrencyID, vTransWriteOffCurrencyID:=vTransWriteOffCurrencyID, vPaymentsCurrencyID:=vPaymentsCurrencyID, vClaimPaymentsCurrencyID:=vClaimPaymentsCurrencyID, vIsViewClient:=vIsViewClient, vIsEditClient:=vIsEditClient, vIsEditPolicy:=vIsEditPolicy, vIsEditClaim:=vIsEditClaim, vIsEditFinancePlan:=vIsEditFinancePlan, vIsRaiseDebit:=vIsRaiseDebit, vIsRaiseCredit:=vIsRaiseCredit, vIsRaiseFee:=vIsRaiseFee, vIsRaiseCash:=vIsRaiseCash, vIsReverseTransactions:=vIsReverseTransactions, vIsReverseAllocations:=vIsReverseAllocations, vIsRaiseManualDID:=vIsRaiseManualDID, vIsDeleteClient:=vIsDeleteClient, vIsPerformAllocations:=vIsPerformAllocations, vCanPerformBrokerTransfer:=vCanPerformBrokerTransfer, vIsDeletePolicy:=vIsDeletePolicy, vIsEditSchemePolicy:=vIsEditSchemePolicy, vCanMakeLiveInvoice:=vCanMakeLiveInvoice, vCAnMakeLiveInstalments:=vCAnMakeLiveInstalments, vCanMakeLivePayNow:=vCanMakeLivePayNow, vHasPaynowWriteoffAuthority:=vHasPaynowWriteoffAuthority, vPaynowWriteOffCurrencyID:=vPaynowWriteOffCurrncyID, vPayNowWriteoffAmount:=vPayNowWriteoffAmount, vPaynowBankAccount:=vPaynowBankAccount, vViewBatchProcessStatus:=vHasViewBatchProcessStatus, vCurrencyLossGainLimit:=vCurrencyLossGainLimit, vLossGainCurrencyID:=vLossGainCurrencyID, vVoidTransaction:=vVoidTransaction)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oACTUserAuthorities = Nothing
                Return result
            End If

            ' Add ACTUserAuthorities to collection
            If (m_oACTUserAuthoritiess.Count = 0) Then
                m_oACTUserAuthoritiess.Add(Nothing)
            End If
            m_lReturn = m_oACTUserAuthoritiess.Add(oNewACTUserAuthorities:=oACTUserAuthorities)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oACTUserAuthorities = Nothing
                Return result
            End If

            oACTUserAuthorities = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditUpdate (Public)
    '
    ' Description: Validates that this action is valid on the ACTUserAuthorities
    '              specified and updates the ACTUserAuthorities with the new values.
    '
    ' 19/04/2005 - 2005 Staff Control Added New Client Manager Security fields
    ' 13/10/2005 - New Option to switch of Editing of Schemes Type policies'
    ' ***************************************************************** '
    Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vUserID As Object = Nothing, Optional ByRef vHasWriteOffAuthority As Object = Nothing, Optional ByRef vWriteOffAmount As Object = Nothing, Optional ByRef vHasUnrestrictedEnquiry As Object = Nothing, Optional ByRef vHasUnrestrictedUpdate As Object = Nothing, Optional ByRef vFeeDiscount As Object = Nothing, Optional ByRef vHasTransWriteOffAuthority As Object = Nothing, Optional ByRef vTransWriteOffAmount As Object = Nothing, Optional ByRef vHasRefundAuthority As Object = Nothing, Optional ByRef vHasTransferAuthority As Object = Nothing, Optional ByRef vHasPaymentsAuthority As Object = Nothing, Optional ByRef vPaymentsAmount As Object = Nothing, Optional ByRef vHasClaimPaymentsAuthority As Object = Nothing, Optional ByRef vClaimPaymentsAmount As Object = Nothing, Optional ByRef vOverrideDate As Object = Nothing, Optional ByRef vOverrideRate As Object = Nothing, Optional ByRef vOverridePrePolicyDate As Object = Nothing, Optional ByRef vOverridePrePolicyRate As Object = Nothing, Optional ByRef vWriteOffCurrencyID As Object = Nothing, Optional ByRef vTransWriteOffCurrencyID As Object = Nothing, Optional ByRef vPaymentsCurrencyID As Object = Nothing, Optional ByRef vClaimPaymentsCurrencyID As Object = Nothing, Optional ByRef vIsViewClient As Object = Nothing, Optional ByRef vIsEditClient As Object = Nothing, Optional ByRef vIsEditPolicy As Object = Nothing, Optional ByRef vIsEditClaim As Object = Nothing, Optional ByRef vIsEditFinancePlan As Object = Nothing, Optional ByRef vIsRaiseDebit As Object = Nothing, Optional ByRef vIsRaiseCredit As Object = Nothing, Optional ByRef vIsRaiseFee As Object = Nothing, Optional ByRef vIsRaiseCash As Object = Nothing, Optional ByRef vIsReverseTransactions As Object = Nothing, Optional ByRef vIsReverseAllocations As Object = Nothing, Optional ByRef vIsRaiseManualDID As Object = Nothing, Optional ByRef vIsDeleteClient As Object = Nothing, Optional ByRef vIsPerformAllocations As Object = Nothing, Optional ByRef vCanPerformBrokerTransfer As Object = Nothing, Optional ByRef vDuplicateClaimOverride As Object = Nothing, Optional ByRef vIsDeletePolicy As Object = Nothing, Optional ByRef vIsEditSchemePolicy As Object = Nothing, Optional ByRef vCanMakeLiveInvoice As Integer = 0, Optional ByRef vCAnMakeLiveInstalments As Integer = 0, Optional ByRef vCanMakeLivePayNow As Integer = 0, Optional ByRef vHasPaynowWriteoffAuthority As Integer = 0, Optional ByRef vPaynowWriteOffCurrencyID As Integer = 0, Optional ByRef vPayNowWriteoffAmount As Decimal = 0, Optional ByRef vPaynowBankAccount As Integer = 0, Optional ByRef vPostingPeriod As Object = Nothing, Optional ByRef vUserCanChangeReserves As Object = Nothing, Optional ByRef vUserCanAddRemoveRatingSections As Object = Nothing, Optional ByRef vUserCanEditExistingRatingSections As Object = Nothing, Optional ByRef vIsViewClientManager As Object = Nothing, Optional ByRef vIsViewAgentMaintenance As Object = Nothing, Optional ByRef vIsViewAccountHandler As Object = Nothing, Optional ByRef vIsViewAccountExecutive As Object = Nothing, Optional ByRef vIsViewInsurerMaintenance As Object = Nothing, Optional ByRef vIsViewOtherParty As Object = Nothing, Optional ByRef vParams As Object = Nothing, Optional ByRef vHasViewbatchProcessStatus As Object = Nothing, Optional ByRef vCurrencyLossGainLimit As Object = Nothing, Optional ByRef vLossGainCurrencyID As Object = Nothing, Optional ByRef vHasManualJournalAuthority As Object = Nothing, Optional ByRef vManualJournalAmount As Double = 0, Optional ByRef vManualJournalCurrencyID As Object = Nothing, Optional ByVal sUniqueId As String = "", Optional ByVal sScreenHierarchy As String = "", Optional ByVal iModifiedByUserId As Integer = 0, Optional ByRef vVoidTransaction As String = "") As Integer


        Dim result As Integer = 0
        Dim oACTUserAuthorities As bACTUserAuthorities.ACTUserAuthorities
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oACTUserAuthoritiess.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oACTUserAuthorities = m_oACTUserAuthoritiess.Item(lRow)

            ' Check the Status of the ACTUserAuthorities

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oACTUserAuthorities.DatabaseStatus
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Leave Status as Add
                    iStatus = gPMConstants.PMEComponentAction.PMAdd
                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                    ' Error
                    result = gPMConstants.PMEReturnCode.PMFalse
                Case Else
                    ' Set Edit (Update) Status
                    iStatus = gPMConstants.PMEComponentAction.PMEdit
            End Select

            ' Update ACTUserAuthorities Attributes
            ' 19/04/2005 - 2005 Staff Control Added New Client Manager Security fields


            'developer guide no. 98
            m_lReturn = oACTUserAuthorities.SetProperties(iStatus:=iStatus, vUserID:=vUserID, vHasWriteOffAuthority:=vHasWriteOffAuthority,
                                                          vWriteOffAmount:=vWriteOffAmount, vHasUnrestrictedEnquiry:=vHasUnrestrictedEnquiry,
                                                          vHasUnrestrictedUpdate:=vHasUnrestrictedUpdate, vFeeDiscount:=vFeeDiscount,
                                                          vHasTransWriteOffAuthority:=vHasTransWriteOffAuthority, vTransWriteOffAmount:=vTransWriteOffAmount,
                                                          vHasRefundAuthority:=vHasRefundAuthority, vHasTransferAuthority:=vHasTransferAuthority,
                                                          vHasPaymentsAuthority:=vHasPaymentsAuthority, vPaymentsAmount:=vPaymentsAmount,
                                                          vHasClaimPaymentsAuthority:=vHasClaimPaymentsAuthority, vClaimPaymentsAmount:=vClaimPaymentsAmount,
                                                          vOverrideDate:=vOverrideDate, vOverrideRate:=vOverrideRate, vOverridePrePolicyDate:=vOverridePrePolicyDate,
                                                          vOverridePrePolicyRate:=vOverridePrePolicyRate, vWriteOffCurrencyID:=vWriteOffCurrencyID,
                                                          vTransWriteOffCurrencyID:=vTransWriteOffCurrencyID, vPaymentsCurrencyID:=vPaymentsCurrencyID,
                                                          vClaimPaymentsCurrencyID:=vClaimPaymentsCurrencyID, vIsViewClient:=vIsViewClient,
                                                          vIsEditClient:=vIsEditClient, vIsEditPolicy:=vIsEditPolicy, vIsEditClaim:=vIsEditClaim,
                                                          vIsEditFinancePlan:=vIsEditFinancePlan, vIsRaiseDebit:=vIsRaiseDebit, vIsRaiseCredit:=vIsRaiseCredit,
                                                          vIsRaiseFee:=vIsRaiseFee, vIsRaiseCash:=vIsRaiseCash, vIsReverseTransactions:=vIsReverseTransactions,
                                                          vIsReverseAllocations:=vIsReverseAllocations, vIsRaiseManualDID:=vIsRaiseManualDID, vIsDeleteClient:=vIsDeleteClient,
                                                          vIsPerformAllocations:=vIsPerformAllocations, vCanPerformBrokerTransfer:=vCanPerformBrokerTransfer,
                                                          vDuplicateClaimOverride:=vDuplicateClaimOverride, vIsDeletePolicy:=vIsDeletePolicy, vIsEditSchemePolicy:=vIsEditSchemePolicy,
                                                          vCanMakeLiveInvoice:=vCanMakeLiveInvoice, vCAnMakeLiveInstalments:=vCAnMakeLiveInstalments,
                                                          vCanMakeLivePayNow:=vCanMakeLivePayNow, vHasPaynowWriteoffAuthority:=vHasPaynowWriteoffAuthority,
                                                          vPaynowWriteOffCurrencyID:=vPaynowWriteOffCurrencyID, vPayNowWriteoffAmount:=vPayNowWriteoffAmount,
                                                          vPaynowBankAccount:=vPaynowBankAccount, vPostingPeriod:=vPostingPeriod, vUserCanChangeReserves:=vUserCanChangeReserves,
                                                          vUserCanAddRemoveRatingSections:=vUserCanAddRemoveRatingSections, vUserCanEditExistingRatingSections:=vUserCanEditExistingRatingSections,
                                                          vIsViewClientManager:=vIsViewClientManager, vIsViewAgentMaintenance:=vIsViewAgentMaintenance, vIsViewAccountHandler:=vIsViewAccountHandler,
                                                          vIsViewAccountExecutive:=vIsViewAccountExecutive, vIsViewInsurerMaintenance:=vIsViewInsurerMaintenance,
                                                          vIsViewOtherParty:=vIsViewOtherParty, vParams:=vParams, vViewBatchProcessStatus:=vHasViewbatchProcessStatus,
                                                          vCurrencyLossGainLimit:=vCurrencyLossGainLimit, vLossGainCurrencyID:=vLossGainCurrencyID,
                                                            vHasManualJournalAuthority:=vHasManualJournalAuthority,
                                                    vManualJournalAmount:=vManualJournalAmount,
                                                    vManualJournalCurrencyID:=vManualJournalCurrencyID, sUniqueId:=sUniqueId, sScreenHierarchy:=sScreenHierarchy, iModifiedByUserId:=iModifiedByUserId,vVoidTransaction:=vVoidTransaction)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oACTUserAuthorities = Nothing
                Return result
            End If

            ' Release reference to ACTUserAuthorities
            oACTUserAuthorities = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditUpdate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditDelete (Public)
    '
    ' Description: Validate that the specified ACTUserAuthorities can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oACTUserAuthorities As bACTUserAuthorities.ACTUserAuthorities

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oACTUserAuthoritiess.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oACTUserAuthorities = m_oACTUserAuthoritiess.Item(lRow)

            ' Check the Status of the ACTUserAuthorities

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oACTUserAuthorities.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oACTUserAuthorities.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                oACTUserAuthorities.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
            End If

            ' Release reference to ACTUserAuthorities
            oACTUserAuthorities = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditDelete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Cancel (Public)
    '
    ' Description: Checks the Collection to see if Cancel is OK.
    '              i.e. Do we need any Adding, Deleting or Updating.
    '
    '              Returns PMTrue if all items are clean
    '                      (PMView or PMDummyDelete)
    '              Otherwise returns PMDataChanged.
    ' ***************************************************************** '
    Public Function Cancel() As Integer
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Loop round Collection
            For lSub As Integer = 1 To m_oACTUserAuthoritiess.Count()
                Select Case m_oACTUserAuthoritiess.Item(lSub).DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing
                    Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMDelete
                        result = gPMConstants.PMEReturnCode.PMDataChanged
                        Exit For
                End Select
            Next lSub

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cancel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Cancel", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Update (Public)
    '
    ' Description: Loops round the collection, doing any required
    '              Adds, Deletes or Updates.
    '
    ' ***************************************************************** '
    Public Function Update() As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer = 0, lSub As Integer
        Dim oACTUserAuthorities As bACTUserAuthorities.ACTUserAuthorities = Nothing
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub = 1 To m_oACTUserAuthoritiess.Count()
                oACTUserAuthorities = m_oACTUserAuthoritiess.Item(lSub)


                Select Case oACTUserAuthorities.DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing

                    Case gPMConstants.PMEComponentAction.PMAdd

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = BeginTrans()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Add Item
                        m_lReturn = oACTUserAuthorities.AddItem()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                    Case gPMConstants.PMEComponentAction.PMEdit

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = BeginTrans()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Update Item
                        m_lReturn = oACTUserAuthorities.UpdateItem()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                    Case gPMConstants.PMEComponentAction.PMDelete

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = BeginTrans()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Delete Item
                        m_lReturn = oACTUserAuthorities.DeleteItem()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                End Select

            Next lSub

            ' Retain the Primary Key of the ACTUserAuthorities
            With oACTUserAuthorities
                UserID = .UserID
            End With

            ' Release last reference
            oACTUserAuthorities = Nothing

            ' Check to see if we started a Transaction (i.e. Were any updates done)
            If bTransStarted Then

                ' Commit if OK, or Rollback any errors
                If result = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CommitTrans()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Refresh the Collection Items D/B Status
                    lSub = 1

                    ' For each item in the collection
                    Do While lSub <= m_oACTUserAuthoritiess.Count()

                        ' With the item
                        With m_oACTUserAuthoritiess.Item(lSub)


                            Select Case .DatabaseStatus
                                ' Delete or Dummy Delete
                                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                                    ' Remove from Collection
                                    m_oACTUserAuthoritiess.Delete(lSub)

                                    ' Anything Else
                                Case Else
                                    ' Set Status to view
                                    .DatabaseStatus = gPMConstants.PMEComponentAction.PMView
                                    lSub += 1

                            End Select

                        End With

                    Loop

                Else

                    m_lReturn = RollbackTrans()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetOption (Private)
    '
    ' Description: Get an option.
    ' 19/04/2005 - 2005 Staff Control Added New Client Manager Security
    '
    ' ***************************************************************** '
    Public Function GetOption(ByVal v_iOptionNumber As Integer, ByRef r_sOptionValue As String, ByVal v_iCompany As Integer) As Integer
        Dim result As Integer = 0
        Dim oSystemOption As bSIROptions.Business = Nothing
        Dim bCloseDatabase As Boolean
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If oSystemOption Is Nothing Then

                ' Get Reference to Database
                m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, r_bNewInstanceCreated:=bCloseDatabase, r_oCheckedDatabase:=m_oDatabase)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Get Option Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'Get Instance of System Option Business

                oSystemOption = New bSIROptions.Business
                m_lReturn = oSystemOption.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


            End If



            m_lReturn = oSystemOption.GetOption(iOptionNumber:=v_iOptionNumber, sValue:=r_sOptionValue, v_iSourceID:=v_iCompany)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the option", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            oSystemOption.Dispose()
            oSystemOption = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOption Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckMandatory (Private)
    '
    ' Description: Check Mandatory parameters have been passed.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (CheckMandatory) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CheckMandatory(Optional ByRef vUserID As Object = Nothing, Optional ByRef vHasWriteOffAuthority As Object = Nothing, Optional ByRef vWriteOffAmount As Object = Nothing) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' {* USER DEFINED CODE (Begin) *}
    '


    'If (Informations.IsNothing(vHasWriteOffAuthority)) Or (Object.Equals(vHasWriteOffAuthority, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '
    '
    ' {* USER DEFINED CODE (End) *}
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
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckMandatory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMandatory", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
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


    'developer guide no. 101
    Public Function ValidateAmounts(ByVal v_iCurrencyID As Integer, ByVal v_cAmount As Decimal, ByVal v_lCompanyID As Integer, ByRef r_vTransWriteOffValid As Object) As Integer
        Return ValidateAmounts(v_iCurrencyID:=v_iCurrencyID, v_cAmount:=v_cAmount, v_lCompanyID:=v_lCompanyID, r_vWriteOffValid:=Nothing, r_vTransWriteOffValid:=r_vTransWriteOffValid, r_vPaymentValid:=Nothing, r_vClaimPaymentValid:=Nothing, r_cAuthorityAmount:=0, r_sCurrency:="")
    End Function

    Public Function ValidateAmounts(ByVal v_iCurrencyID As Integer, ByVal v_cAmount As Decimal, ByVal v_lCompanyID As Integer, ByRef r_vWriteOffValid As Object, ByRef r_cAuthorityAmount As Decimal, ByRef r_sCurrency As String, Optional ByRef bIsCurrencyDiff As Boolean = False) As Integer
        Return ValidateAmounts(v_iCurrencyID:=v_iCurrencyID, v_cAmount:=v_cAmount, v_lCompanyID:=v_lCompanyID, r_vWriteOffValid:=r_vWriteOffValid, r_vTransWriteOffValid:=Nothing, r_vPaymentValid:=Nothing, r_vClaimPaymentValid:=Nothing, r_cAuthorityAmount:=r_cAuthorityAmount, r_sCurrency:=r_sCurrency, bIsCurrencyDiff:=bIsCurrencyDiff)
    End Function

    Public Function ValidateAmounts(ByVal v_iCurrencyID As Integer, ByVal v_cAmount As Decimal, ByVal v_lCompanyID As Integer, ByRef r_vWriteOffValid As Object, ByRef r_vTransWriteOffValid As Object, ByRef r_vPaymentValid As Object, ByRef r_vClaimPaymentValid As Object, ByRef r_cAuthorityAmount As Decimal, ByRef r_sCurrency As String, Optional ByRef bIsCurrencyDiff As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim oCurrency As bACTCurrency.Form

        Dim vAuthority As Object = Nothing, vCurrencyID As Object = Nothing
        Dim vAmount As Decimal
        Dim bValid As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Set business objects

            oCurrency = New bACTCurrency.Form
            m_lReturn = oCurrency.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the current users user authority details
            m_lReturn = GetDetails(vUserID:=m_iUserID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If bIsCurrencyDiff = True Then
                vAuthority = 1
                Dim vWriteoffAmount As Decimal
                Dim vWriteOffAuthority As Object = Nothing
                Dim vWriteoffCurrency As Object = Nothing
                Dim vCurrencyLossGainAmount As Object = Nothing
                m_lReturn = GetNext(vCurrencyLossGainLimit:=vCurrencyLossGainAmount, vLossGainCurrencyID:=vCurrencyID, vHasWriteOffAuthority:=vWriteOffAuthority, vWriteOffCurrencyID:=vWriteoffCurrency, vWriteOffAmount:=vWriteoffAmount)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                If Informations.IsNothing(vCurrencyLossGainAmount) OrElse String.IsNullOrEmpty(vCurrencyLossGainAmount) Then
                    bIsCurrencyDiff = False
                    vAuthority = vWriteOffAuthority
                    vAmount = vWriteoffAmount
                    vCurrencyID = vWriteoffCurrency
                Else
                    vAmount = gPMFunctions.ToSafeDecimal(vCurrencyLossGainAmount)
                End If
            Else
                If Not Informations.IsNothing(r_vWriteOffValid) Then
                    m_lReturn = GetNext(vHasWriteOffAuthority:=vAuthority, vWriteOffCurrencyID:=vCurrencyID, vWriteOffAmount:=vAmount)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If


            If Not Informations.IsNothing(r_vTransWriteOffValid) Then
                m_lReturn = GetNext(vHasTransWriteOffAuthority:=vAuthority, vTransWriteOffCurrencyID:=vCurrencyID, vTransWriteOffAmount:=vAmount)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            If Not Informations.IsNothing(r_vPaymentValid) Then
                m_lReturn = GetNext(vHasPaymentsAuthority:=vAuthority, vPaymentsCurrencyID:=vCurrencyID, vPaymentsAmount:=vAmount)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            If Not Informations.IsNothing(r_vClaimPaymentValid) Then
                m_lReturn = GetNext(vHasClaimPaymentsAuthority:=vAuthority, vClaimPaymentsCurrencyID:=vCurrencyID, vClaimPaymentsAmount:=vAmount)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            If CStr(vAuthority) = "1" Then

                'developer guide no. 98
                m_lReturn = IsValidAmount(v_lCompanyID:=v_lCompanyID, v_iCheckCurrencyID:=v_iCurrencyID, v_cCheckAmount:=v_cAmount, v_iAuthorityCurrencyID:=vCurrencyID, v_cAuthorityAmount:=vAmount, r_bValid:=bValid)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Set all values although only one will be used.
                r_vWriteOffValid = bValid
                r_vTransWriteOffValid = bValid
                r_vPaymentValid = bValid
                r_vClaimPaymentValid = bValid
            Else
                r_vWriteOffValid = False
                r_vTransWriteOffValid = False
                r_vPaymentValid = False
                r_vClaimPaymentValid = False
            End If

            'Get authority currency description

            m_lReturn = oCurrency.GetDetails(vCurrencyID:=vCurrencyID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oCurrency.GetNext(vDescription:=r_sCurrency)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set authority amount
            r_cAuthorityAmount = vAmount

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateAmounts Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateAmounts", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function IsValidAmount(ByVal v_lCompanyID As Integer, ByVal v_iCheckCurrencyID As Integer, ByVal v_cCheckAmount As Decimal, ByVal v_iAuthorityCurrencyID As Integer, ByVal v_cAuthorityAmount As Decimal, ByRef r_bValid As Boolean) As Integer

        Dim result As Integer = 0
        Dim oCurrencyConvert As bACTCurrencyConvert.Form
        Dim cAuthorityAmountInCheckCurrency, cAuthorityAmountInBaseCurrency As Decimal
        Dim iBaseCurrency As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        'Set defaults
        r_bValid = False

        'Set business object

        oCurrencyConvert = New bACTCurrencyConvert.Form
        m_lReturn = oCurrencyConvert.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Get base currency

        m_lReturn = oCurrencyConvert.GetBaseCurrency(v_lCompanyId:=v_lCompanyID, r_iBaseCurrencyID:=iBaseCurrency)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If v_iCheckCurrencyID = v_iAuthorityCurrencyID Then 'Same currencies so no conversion
            cAuthorityAmountInCheckCurrency = v_cAuthorityAmount
        ElseIf v_iAuthorityCurrencyID = iBaseCurrency Then  'Authority amount in base so convert to check amounts currency.

            m_lReturn = oCurrencyConvert.Convert(v_bConvertToBase:=False, v_lCurrencyID:=v_iCheckCurrencyID, v_lCompanyId:=v_lCompanyID, r_cOriginalAmount:=v_cAuthorityAmount, r_cConvertedAmount:=cAuthorityAmountInCheckCurrency)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        ElseIf v_iCheckCurrencyID = iBaseCurrency Then  'Check amount in base so convert Authority amount to base.

            m_lReturn = oCurrencyConvert.Convert(v_bConvertToBase:=True, v_lCurrencyID:=v_iAuthorityCurrencyID, v_lCompanyId:=v_lCompanyID, r_cOriginalAmount:=v_cAuthorityAmount, r_cConvertedAmount:=cAuthorityAmountInCheckCurrency)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        Else
            'Both amounts non-base so convert authority amount to base and then into check amounts currency.

            m_lReturn = oCurrencyConvert.Convert(v_bConvertToBase:=True, v_lCurrencyID:=v_iAuthorityCurrencyID, v_lCompanyId:=v_lCompanyID, r_cOriginalAmount:=v_cAuthorityAmount, r_cConvertedAmount:=cAuthorityAmountInBaseCurrency)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oCurrencyConvert.Convert(v_bConvertToBase:=False, v_lCurrencyID:=v_iCheckCurrencyID, v_lCompanyId:=v_lCompanyID, r_cOriginalAmount:=cAuthorityAmountInBaseCurrency, r_cConvertedAmount:=cAuthorityAmountInCheckCurrency)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        ' Allow up to and including the authority limit amount. PN16854
        If Math.Abs(v_cCheckAmount) <= Math.Abs(cAuthorityAmountInCheckCurrency) Then
            r_bValid = True
        End If

        Return result

    End Function

    'developer guide no.17
    Public Function GetUserWriteOffDetails(ByVal user_id As Integer, ByRef vResult As Object) As Integer

        Dim result As Integer = 0
        'developer guide no.17
        Dim v_result(,) As Object = Nothing
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="uid", vValue:=CStr(user_id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetUserWriteOffDetailsSQL, sSQLName:=ACGetUserWriteOffDetailsName, bStoredProcedure:=False, vResultArray:=v_result)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            Else
                vResult = ""
                vResult = v_result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserWriteOffDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserWriteOffDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetPartyViewOptions (Public)
    '
    ' Description: Gets the Party View option
    ' ***************************************************************** '
    Public Function GetPartyViewOptions(ByVal v_lUserId As Integer, ByRef r_bIsViewOnlyClientManager As Boolean) As Integer
        Return GetPartyViewOptions(v_lUserId:=v_lUserId, r_bIsViewOnlyClientManager:=r_bIsViewOnlyClientManager, r_bIsViewOnlyAgentMaintenance:=False, r_bIsViewOnlyAccountHandlerMaintenance:=False, r_bIsViewOnlyAccountExecutiveMaintenace:=False, r_bIsViewOnlyInsurerMaintenance:=False, r_bIsViewOnlyOtherPartyMaintenance:=False)
    End Function

    Public Function GetPartyViewOptions(ByVal v_lUserId As Integer, ByRef r_bIsViewOnlyClientManager As Boolean, ByRef r_bIsViewOnlyAgentMaintenance As Boolean, ByRef r_bIsViewOnlyAccountHandlerMaintenance As Boolean, ByRef r_bIsViewOnlyAccountExecutiveMaintenace As Boolean, ByRef r_bIsViewOnlyInsurerMaintenance As Boolean, ByRef r_bIsViewOnlyOtherPartyMaintenance As Boolean) As Integer

        Dim result As Integer = 0
        Dim v_result(,) As Object = Nothing


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(v_lUserId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetViewOptionSQL, sSQLName:=ACGetViewOptionName, bStoredProcedure:=True, vResultArray:=v_result)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Informations.IsArray(v_result) Then
                r_bIsViewOnlyClientManager = gPMFunctions.ToSafeBoolean(v_result(0, 0))
                r_bIsViewOnlyAgentMaintenance = gPMFunctions.ToSafeBoolean(v_result(1, 0))
                r_bIsViewOnlyAccountHandlerMaintenance = gPMFunctions.ToSafeBoolean(v_result(2, 0))
                r_bIsViewOnlyAccountExecutiveMaintenace = gPMFunctions.ToSafeBoolean(v_result(3, 0))
                r_bIsViewOnlyInsurerMaintenance = gPMFunctions.ToSafeBoolean(v_result(4, 0))
                r_bIsViewOnlyOtherPartyMaintenance = gPMFunctions.ToSafeBoolean(v_result(5, 0))
            End If


        Catch ex As Exception

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPartyViewOptions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartyViewOptions", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

            result = gPMConstants.PMEReturnCode.PMFalse

        Finally

            m_oDatabase.Parameters.Clear()


        End Try
        Return result
    End Function


    Public Function GetUserAuthoritiesDetails(ByVal v_lUserId As Integer, ByRef r_vResults(,) As Object) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "GetUserAuthoritiesDetails"

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(v_lUserId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectSingleSQL, sSQLName:=ACSelectSingleName, bStoredProcedure:=True, vResultArray:=r_vResults)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            End If


        Catch ex As Exception

            ' Log Error Message
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally

            m_oDatabase.Parameters.Clear()


        End Try
        Return result
    End Function

    '****************************************************************************
    ' Get values from specified columns (v_vReturnColumn)
    ' if v_sKeyColumn and v_sKeyValue is passed in then use it as criteria otherwise
    ' whole table is selected
    ' if v_vReturnColumn is not an array then single value is returned
    ' Note: v_lColumnType = PMLong, PMString etc
    '
    '****************************************************************************
    'developer guide no 101
    Public Function GetValueFromTable(ByVal v_sTableName As String, ByVal v_vReturnColumn As Object, ByVal v_sKeyColumn As String, ByVal v_sKeyValue As String, ByVal v_iDataType As Integer, ByRef r_vResult As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As New StringBuilder
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If v_sKeyColumn = "user_id" Then v_sKeyValue = CStr(m_iUserID)

            sSQL = New StringBuilder("SELECT DISTINCT ")

            If Informations.IsArray(v_vReturnColumn) Then

                For Each v_vReturnColumn_item As Object In v_vReturnColumn

                    sSQL.Append(CStr(v_vReturnColumn_item) & ",")
                Next v_vReturnColumn_item

                'get rid of last comma
                sSQL = New StringBuilder(sSQL.ToString().Substring(0, sSQL.ToString().Length - 1))

            Else
                'Developer Guide No.149
                sSQL.Append(Convert.ToString(v_vReturnColumn))
            End If

            sSQL.Append(Strings.ChrW(13) & Strings.ChrW(10) & "FROM " & v_sTableName & Strings.ChrW(13) & Strings.ChrW(10))

            If v_sKeyColumn <> "" Then
                sSQL.Append("WHERE " & v_sKeyColumn & " = {KeyValue}")

                m_oDatabase.Parameters.Clear()

                'Dim myDateTime As DateTime = v_sKeyValue
                'Dim formattedDate As String = myDateTime.ToString("yyyy-MM-dd HH:mm:ss")

                Select Case v_iDataType
                    Case gPMConstants.PMEDataType.PMString
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=v_sKeyValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iDataType)
                    Case gPMConstants.PMEDataType.PMLong
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=CStr(CInt(v_sKeyValue)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iDataType)
                    Case gPMConstants.PMEDataType.PMInteger
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=CStr(CInt(v_sKeyValue)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iDataType)

                    Case gPMConstants.PMEDataType.PMDouble
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=CStr(CDbl(v_sKeyValue)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iDataType)

                    Case gPMConstants.PMEDataType.PMDate
                        Dim myDateTime As DateTime = v_sKeyValue
                        Dim formattedDate As String = myDateTime.ToString("yyyy-MM-dd HH:mm:ss")
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=formattedDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iDataType)

                    Case gPMConstants.PMEDataType.PMBoolean
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=CStr(CBool(v_sKeyValue)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iDataType)

                    Case gPMConstants.PMEDataType.PMCurrency
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=CStr(CDec(v_sKeyValue)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iDataType)

                End Select

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                End If
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL.ToString(), sSQLName:="Get single value from table", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray, bKeepNulls:=False)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            'are we returning an array or a single value?
            If Informations.IsArray(v_vReturnColumn) Then

                r_vResult = vResultArray
            Else
                If Informations.IsArray(vResultArray) Then

                    If CStr(vResultArray(0, 0)) <> "" Then

                        r_vResult = vResultArray(0, 0)

                    End If
                Else
                    result = gPMConstants.PMEReturnCode.PMNotFound
                End If
            End If


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get value from " & v_sTableName & "." & v_sKeyColumn & " = " & v_sKeyValue, vApp:=ACApp, vClass:=ACClass, vMethod:="GetValueFromTable", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)


        Finally



        End Try
        Return result
    End Function

    Public Function CheckIsRecommendClaimPaymentEnabledatProduct(ByRef r_vResults(,) As Object) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "CheckIsRecommendClaimPaymentEnabledatProduct"

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckIsRecommendClaimPaymentEnabledatProductSQL, sSQLName:=ACCheckIsRecommendClaimPaymentEnabledatProductName, bStoredProcedure:=True, vResultArray:=r_vResults)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            End If


        Catch ex As Exception

            ' Log Error Message
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally

            m_oDatabase.Parameters.Clear()


        End Try
        Return result
    End Function
    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Terminate() As Integer

        Dim result As Integer = 0
        Static bTerminated As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If we have already Terminated then exit

            If bTerminated Then
                Return result
            Else
                bTerminated = True
            End If


            ' Relase ACTUserAuthoritiess Reference
            m_oACTUserAuthoritiess = Nothing

            ' If this class opened the database, close it
            If m_bCloseDatabase Then
                ' Close the Database
                m_lReturn = m_oDatabase.CloseDatabase()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Release reference to PM Data Access Object
            m_oDatabase = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Terminate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Terminate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function
End Class
