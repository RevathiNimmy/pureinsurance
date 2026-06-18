Option Strict Off
Option Explicit On
' developer guide no. 129
Imports System.Globalization
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("UBusiness_NET.UBusiness")>
Public NotInheritable Class UBusiness
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: UBusiness
    '
    ' Date: 20/05/04
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a PMBFee for Underwriting.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 11/02/2004
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    'persist currency values
    Private m_iCurrencyID As Integer
    Private m_sCurrencyName As String = ""
    Private m_iLogLevel As Integer
    ' ************************************************


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "UBusiness"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)
    Private m_vNewArray As Object

    ' bSIRParty.Init Class (Private)
    Private m_oDatabase As dPMDAO.Database



    ' PM Lookup Business Component (Private)
    Private m_oLookup As BPMLOOKUP.Business

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_dtEffectiveDate As Date
    ' Stores the exit status of the interface.
    Private m_lStatus As Integer

    Private lPMAuthorityLevel As Integer

    'Lookup values for Loading in Edit Mode
    Private m_sTransactionTypeID As Integer
    Private m_sTransactionTypeDesc As String = ""
    Private m_lProductType As Integer
    Private m_dFeePercentage As Double
    Private m_cFeeAmount As Decimal
    Private m_sProductTypeDesc As String = ""
    Private m_nIsTaxable As Integer
    Private m_TransactionType As String = ""
    Private m_sTransactionType As String = ""
    Private m_lFeeAmountID As Integer
    Private m_lInsurance_cnt As Integer
    Private m_lIsAmmended As Integer
    Private m_lErrorNumber As Integer
    Private m_sStepStatus As String = ""

    Private m_iDatabaseStatus As Integer

    ' Primary Key to work with
    Private m_lPartyCnt As Integer

    Private m_sUnderwritingOrAgency As String = ""
    Private m_lBranch_id As Integer

    '************************************
    ' AUS005 Changes
    Private m_oCurrencyConvert As bACTCurrencyConvert.Form
    '************************************


    Public Function AddPolicyFees_u(Optional ByRef vInsurance_cnt As Integer = 0, Optional ByRef vparty_cnt As Integer = 0, Optional ByRef vfee_percentage As Double = 0, Optional ByRef vfee_percentage_amount As Double = 0, Optional ByRef vfee_amount As Double = 0, Optional ByRef vCurrency_id As Integer = 0, Optional ByRef vtransaction_type_id As Integer = 0, Optional ByRef vProduct_id As Integer = 0, Optional ByRef vBranch_id As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase


                ' Add the required INPUT parameters
                m_lReturn = CType(AddPolicyFeesInsertInputParam(vInsurance_cnt:=vInsurance_cnt, vparty_cnt:=vparty_cnt, vfee_percentage:=vfee_percentage, vfee_percentage_amount:=vfee_percentage_amount, vfee_amount:=vfee_amount, vCurrency_id:=vCurrency_id, vtransaction_type_id:=vtransaction_type_id, vProduct_id:=vProduct_id, vBranch_id:=vBranch_id), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACPolicyFees_u_DetailsSQL, sSQLName:=ACPolicyFees_u_DetailsName, bStoredProcedure:=ACPolicyFees_u_DetailsStored, lRecordsAffected:=lRecordsAffected)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddPolicyFees_u Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddPilicyFees_u", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



        End Try
    End Function

    Private Function AddPolicyFeesDeleteInputParam(Optional ByRef vInsurance_cnt As Integer = 0, Optional ByRef vtransaction_type_id As Integer = 0, Optional ByRef vProduct_id As Integer = 0) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            .Parameters.Clear()

            'insurance_cnt
            m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(vInsurance_cnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'transaction type id
            m_lReturn = .Parameters.Add(sName:="transaction_type_id", vValue:=CStr(vtransaction_type_id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'product id
            m_lReturn = .Parameters.Add(sName:="product_id", vValue:=CStr(vProduct_id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        Return result

    End Function

    Public Property BranchID() As Integer
        Get
            Return m_lBranch_id
        End Get
        Set(ByVal Value As Integer)
            m_lBranch_id = Value
        End Set
    End Property

    Public Property IsAmmended() As Integer
        Get
            Return m_lIsAmmended
        End Get
        Set(ByVal Value As Integer)
            m_lIsAmmended = Value
        End Set
    End Property
    Public Property Insurance_cnt() As Integer
        Get
            Return m_lInsurance_cnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsurance_cnt = Value
        End Set
    End Property
    Public Property FeeAmountID() As Integer
        Get
            Return m_lFeeAmountID
        End Get
        Set(ByVal Value As Integer)
            m_lFeeAmountID = Value
        End Set
    End Property
    Public Property CurrencyID() As Integer
        Get
            Return m_iCurrencyID
        End Get
        Set(ByVal Value As Integer)
            m_iCurrencyID = Value
        End Set
    End Property

    Public Property CurrencyName() As String
        Get
            Return m_sCurrencyName
        End Get
        Set(ByVal Value As String)
            m_sCurrencyName = Value
        End Set
    End Property
    Public Property PartyCnt() As Integer
        Get

            Return m_lPartyCnt

        End Get
        Set(ByVal Value As Integer)

            m_lPartyCnt = Value

        End Set
    End Property
    Public Property TransactionType() As Integer
        Get
            Return m_sTransactionTypeID
        End Get
        Set(ByVal Value As Integer)
            m_sTransactionTypeID = Value
        End Set
    End Property
    Public Property TransactionTypeDesc() As String
        Get
            Return m_sTransactionTypeDesc
        End Get
        Set(ByVal Value As String)
            m_sTransactionTypeDesc = Value
        End Set
    End Property
    Public Property ProductType() As Integer
        Get
            Return m_lProductType
        End Get
        Set(ByVal Value As Integer)
            m_lProductType = Value
        End Set
    End Property
    Public Property ProductTypeDesc() As String
        Get
            Return m_sProductTypeDesc
        End Get
        Set(ByVal Value As String)
            m_sProductTypeDesc = Value
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

    Public Property FeeAmount() As Decimal
        Get
            Return m_cFeeAmount
        End Get
        Set(ByVal Value As Decimal)

            m_cFeeAmount = CDec(Value)
        End Set
    End Property
    Public Property FeePercentage() As Double
        Get
            Return m_dFeePercentage
        End Get
        Set(ByVal Value As Double)

            m_dFeePercentage = CDbl(Value)
        End Set
    End Property
    Public Property IsTaxable() As Integer
        Get
            Return m_nIsTaxable
        End Get
        Set(ByVal Value As Integer)
            m_nIsTaxable = Value
        End Set
    End Property


    Public ReadOnly Property ErrorNumber() As Integer
        Get
            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber
        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            ' Set the calling application name.
            m_sCallingAppName = Value
        End Set
    End Property

    Public Property Status() As Integer
        Get
            ' Return the interface exit status.
            Return m_lStatus
        End Get
        Set(ByVal Value As Integer)
            m_lStatus = Value
        End Set
    End Property


    Public Property Task() As Integer
        Get
            Return m_iTask
        End Get
        Set(ByVal Value As Integer)
            m_iTask = Value
        End Set
    End Property

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)
            m_lNavigate = Value
        End Set
    End Property


    Public Property StepStatus() As String
        Get
            Return m_sStepStatus
        End Get
        Set(ByVal Value As String)
            m_sStepStatus = Value
        End Set
    End Property
    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)
            m_lProcessMode = Value
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

#Region "Public Methods"

    Private Function AddInputParamCheck() As Integer


        ' ***************************************************************** '
        ' Name: AddInputParamCheck (Private)
        '
        '
        '
        ' ***************************************************************** '
        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            'party id
            m_lReturn = .Parameters.Add(sName:="fee_amount_id", vValue:=CStr(FeeAmountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'fee percentage
            m_lReturn = .Parameters.Add(sName:="fee_percentage", vValue:=CStr(FeePercentage), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'fee amount
            m_lReturn = .Parameters.Add(sName:="fee_amount", vValue:=CStr(FeeAmount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        Return result

    End Function

    Private Function AddPolicyFeesInsertInputParam(Optional ByRef vInsurance_cnt As Integer = 0, Optional ByRef vparty_cnt As Integer = 0, Optional ByRef vfee_percentage As Double = 0, Optional ByRef vfee_percentage_amount As Double = 0, Optional ByRef vfee_amount As Double = 0, Optional ByRef vCurrency_id As Integer = 0, Optional ByRef vtransaction_type_id As Integer = 0, Optional ByRef vProduct_id As Integer = 0, Optional ByRef vBranch_id As Integer = 0) As Integer



        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            .Parameters.Clear()

            'insurance_cnt
            m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(vInsurance_cnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'party id
            m_lReturn = .Parameters.Add(sName:="party_cnt", vValue:=CStr(vparty_cnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'fee percentage
            m_lReturn = .Parameters.Add(sName:="fee_percentage", vValue:=CStr(vfee_percentage), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'fee percentage amount
            m_lReturn = .Parameters.Add(sName:="fee_percentage_amount", vValue:=CStr(vfee_percentage_amount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'fee amount
            m_lReturn = .Parameters.Add(sName:="fee_amount", vValue:=CStr(vfee_amount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'currency id
            m_lReturn = .Parameters.Add(sName:="currency_id", vValue:=CStr(vCurrency_id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'transaction type id
            m_lReturn = .Parameters.Add(sName:="transaction_type_id", vValue:=CStr(vtransaction_type_id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'product id
            m_lReturn = .Parameters.Add(sName:="product_id", vValue:=CStr(vProduct_id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'branch id
            m_lReturn = .Parameters.Add(sName:="branch_id", vValue:=CStr(vBranch_id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        Return result

    End Function


    Private Function AddInputParamUpdate() As Integer
        ' PRIVATE Methods (Begin)
        ' ***************************************************************** '
        ' Name: AddInputParamUpdate (Private)
        '
        ' Description: Adds input Params for an update
        '
        ' ***************************************************************** '
        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            .Parameters.Clear()

            'party id
            m_lReturn = .Parameters.Add(sName:="fee_amount_id", vValue:=CStr(FeeAmountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'product type
            m_lReturn = .Parameters.Add(sName:="product_group_id", vValue:=CStr(ProductType), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'fee percentage
            m_lReturn = .Parameters.Add(sName:="fee_percentage", vValue:=CStr(FeePercentage), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'fee amount
            m_lReturn = .Parameters.Add(sName:="fee_amount", vValue:=CStr(FeeAmount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'transaction type id
            m_lReturn = .Parameters.Add(sName:="transaction_type_id", vValue:=CStr(TransactionType), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'effective date
            m_lReturn = .Parameters.Add(sName:="effective_date", vValue:=EffectiveDate.ToString, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'is taxable
            m_lReturn = .Parameters.Add(sName:="is_taxable", vValue:=CStr(IsTaxable), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'is taxable
            m_lReturn = .Parameters.Add(sName:="is_ammended", vValue:=CStr(IsAmmended), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'currency id
            m_lReturn = .Parameters.Add(sName:="currency_id", vValue:=CStr(CurrencyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        Return result

    End Function

    Private Function AddInputParamUpdatePremium(ByRef vPremium As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            .Parameters.Clear()

            'insurance file cnt
            m_lReturn = .Parameters.Add(sName:="insurance_file_cnt ", vValue:=CStr(Insurance_cnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'premium

            m_lReturn = .Parameters.Add(sName:="annual_premium", vValue:=CStr(vPremium), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


        End With

        Return result

    End Function

    Public Function CheckAmmended(ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Add the required INPUT parameters
                m_lReturn = CType(AddInputParamCheck(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = .SQLSelect(sSQL:=ACGetRoadMapStepCheckValuesDetailsDetailsSQL, sSQLName:=ACGetRoadMapStepCheckValuesDetailsName, bStoredProcedure:=ACGetRoadMapStepCheckValuesDetailsStored, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With


            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckAmmended Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckAmmended", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result




        End Try
    End Function

    Public Function DeletePolicyFees_u(Optional ByRef vInsurance_cnt As Integer = 0, Optional ByRef vtransaction_type_id As Integer = 0, Optional ByRef vProduct_id As Integer = 0) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase


                ' Add the required INPUT parameters
                m_lReturn = CType(AddPolicyFeesDeleteInputParam(vInsurance_cnt:=vInsurance_cnt, vtransaction_type_id:=vtransaction_type_id, vProduct_id:=vProduct_id), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACDeletePolicyFees_u_DetailsSQL, sSQLName:=ACDeletePolicyFees_u_DetailsName, bStoredProcedure:=ACDeletePolicyFees_u_DetailsStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeletePolicyFees_u Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeletePolicyFees_u", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetEffectiveDate(ByRef vInsID As Object, ByRef vResultArray(,) As Object) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            'insurance file id

            m_lReturn = m_oDatabase.Parameters.Add(sName:="InsID", vValue:=CStr(vInsID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get from DB
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRoadMapStepCheckDateDetailsDetailsSQL, sSQLName:=ACGetRoadMapStepCheckDateDetailsName, bStoredProcedure:=ACGetRoadMapStepCheckDateDetailsStored, lNumberRecords:=0, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetEffectiveDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetEffectiveDate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function


    Public Function GetPartyExtraValue(ByRef vPartyCnt As Object, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            'insurance file id
            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(m_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get from DB
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPartyExtraDetailsSQL, sSQLName:=ACGetPartyExtraDetailsName, bStoredProcedure:=ACGetPartyExtraDetailsStored, lNumberRecords:=0, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetPartyExtraValue Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartyExtraValue", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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




            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password

            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Create PM Lookup Business Object
            m_oLookup = New BPMLOOKUP.Business()



            ' Initialise PM Lookup Business passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions


            ' get instance of bACTCurrencyConvert.Form

            m_oCurrencyConvert = New bACTCurrencyConvert.Form
            m_lReturn = m_oCurrencyConvert.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bACTCurrencyConvert.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return gPMConstants.PMEReturnCode.PMFalse
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
            Me.disposedValue = True
            If disposing Then
                If m_oLookup IsNot Nothing Then
                    m_oLookup.Dispose()
                End If
                m_oLookup = Nothing
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
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    '----------------
    '
    ' ***************************************************************** '
    ' Name: GetLookupValues (Public)
    '
    ' Description: Gets the Lookup values for a SIRPartyFee.
    '
    '
    ' ***************************************************************** '
    'developer guide no. 101
    Public Function GetLookupValues(Optional ByRef iLookupType As Integer = 0, Optional ByRef vTableArray(,) As Object = Nothing, Optional ByRef iLanguageID As Integer = 0, Optional ByRef vResultArray(,) As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim dtEffectiveDate As Date

        ' {* USER DEFINED CODE (Begin) *}
        Dim vTabArray(3, 1) As Object

        ' {* USER DEFINED CODE (End) *}

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset Result Array
            'developer guide no. 146
            vResultArray = Nothing
            ' Reset Table Array

            'developer guide no. 146
            vTableArray = Nothing

            ' {* USER DEFINED CODE (Begin) *}

            ' Setup Lookup Table Names
            'GW PN013 - array values for product and transaction type

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = "Product"

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 1) = "Transaction_Type"

            ' {* USER DEFINED CODE (End) *}

            ' Do we have any records
            ' No, we can only lookup all
            iLookupType = gPMConstants.PMELookupType.PMLookupAll

            ' Do not supply a key
            'vTabArray(PMLookupKey, 0) = m_lProductType



            ' Default Effective Date to current date/time
            dtEffectiveDate = DateTime.Now

            ' Release SIRParty reference
            'Set oSIRParty = Nothing

            ' Get the Lookup items
            m_lReturn = m_oLookup.GetLookupValues(iLookupType:=iLookupType, vTableArray:=vTabArray, iLanguageID:=iLanguageID, dtEffectiveDate:=dtEffectiveDate, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the Table Array

            vTableArray = vTabArray

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookupValues Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function




    ' ***************************************************************** '
    ' Name: DirectAdd (Public)
    '
    ' Description: Adds a single SIRPartyFee directly into the database.
    '
    ' ***************************************************************** '
    Public Function DirectAdd(ByRef vKey As Integer) As Integer

        'Dim oSIRParty As bSIRParty.Business
        Dim result As Integer = 0


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(add(vKey:=vKey), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (Begin) *}
            ' Terminate Core business Component
            'EK 16/10/99
            Dispose()
            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Add (Public)
    '
    ' Description: Adds to the Database from the Base Details.
    '
    ' ***************************************************************** '
    Public Function add(ByRef vKey As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                'add required output parameters
                m_lReturn = CType(AddKeyOutputParam(), gPMConstants.PMEReturnCode)

                ' Add the required INPUT parameters
                m_lReturn = CType(AddInputParam(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACAddFeeAmountsForUnderwritingSQL, sSQLName:=ACAddFeeAmountsForUnderwritingName, bStoredProcedure:=ACAddFeeAmountsForUnderwritingStored_u)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'now return the new primary key back to the property
                m_lReturn = CType(GetNewPrimaryKeyID(vKey:=vKey), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="Dev", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Add Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)


            Return result

        End Try
    End Function

    ' PRIVATE Methods (Begin)
    ' ***************************************************************** '
    ' Name: AddInputParam (Private)
    '
    ' Description: Adds all of the NON-KEY INPUT parameters
    '              required for an Insert or Update.
    '
    ' ***************************************************************** '
    Private Function AddInputParam() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            'party id
            m_lReturn = .Parameters.Add(sName:="party_cnt", vValue:=CStr(PartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'party id
            m_lReturn = .Parameters.Add(sName:="fee_amount_id", vValue:=CStr(FeeAmountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'product type
            m_lReturn = .Parameters.Add(sName:="product_group_id", vValue:=CStr(ProductType), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'fee percentage
            m_lReturn = .Parameters.Add(sName:="fee_percentage", vValue:=CStr(FeePercentage), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'fee amount
            m_lReturn = .Parameters.Add(sName:="fee_amount", vValue:=CStr(FeeAmount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'transaction type id
            m_lReturn = .Parameters.Add(sName:="transaction_type_id", vValue:=CStr(TransactionType), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'effective date
            m_lReturn = .Parameters.Add(sName:="effective_date", vValue:=EffectiveDate.ToString, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'is taxable
            m_lReturn = .Parameters.Add(sName:="is_taxable", vValue:=CStr(IsTaxable), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="currency_id", vValue:=CStr(CurrencyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddKeyInputParam (Private)
    '
    ' Description: Adds all of the PRIMARY KEY INPUT parameters
    '              required for a Select, Delete or Update.
    '
    ' ***************************************************************** '
    Private Function AddKeyInputParam() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            m_lReturn = .Parameters.Add(sName:="fee_amount_id", vValue:=CStr(FeeAmountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Delete (Public)
    '
    ' Description: Deletes a single record from the database.
    '
    ' ***************************************************************** '
    Public Function Delete() As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Add PrimaryKey as INPUT parameters
                m_lReturn = CType(AddKeyInputParam(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACDeleteFeeAmountsForUnderwritingSQL, sSQLName:=ACDeleteFeeAmountsForUnderwritingName, bStoredProcedure:=ACDeleteFeeAmountsForUnderwrtingStored, lRecordsAffected:=lRecordsAffected)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' If record wasn't deleted, error
                If lRecordsAffected > 0 Then
                    ' Deleted, No action required
                Else
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With



            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetNewPrimaryKeyID (Private)
    '
    ' Description: Returns the new PRIMARY KEY values from an Add.
    '
    ' ***************************************************************** '
    Private Function GetNewPrimaryKeyID(ByRef vKey As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            vKey = .Parameters.Item("fee_amount_key").Value

        End With

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: AddKeyOutputParam (Private)
    '
    ' Description: Adds all of the PRIMARY KEY OUTPUT parameters
    '              required for an Add.
    '
    ' ***************************************************************** '
    Private Function AddKeyOutputParam() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            m_lReturn = .Parameters.Add(sName:="fee_amount_key", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: Update (Public)
    '
    ' Description: Updates a single record in the database from
    '              the Base Details.
    '
    ' ***************************************************************** '
    Public Function Update() As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Add the required INPUT parameters
                m_lReturn = CType(AddInputParamUpdate(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACUpdateFeeAmountForUnderwritingDetailsSQL, sSQLName:=ACUpdateFeeAmountForUnderwritingDetailsName, bStoredProcedure:=ACUpdateFeeAmountForUnderwritingDetailsStored, lRecordsAffected:=lRecordsAffected)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Check to see that the record was updated OK
                If lRecordsAffected > 0 Then
                    ' Updated No action required
                Else
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: SetProperties (Public)
    '
    ' Description: Sets the supplied PartyFee property values for
    ' for underwriting
    '
    '
    ' ***************************************************************** '

    Public Function SetProperties(ByRef iStatus As Integer, ByRef vPartyCnt As Integer, ByRef vProductType As Integer, ByRef vTransactionType As Integer, ByRef vEffectiveDate As Date, ByRef vIsTaxable As Integer, Optional ByRef vFeePercent As Double = 0, Optional ByRef vFeeAmount As Decimal = 0) As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If Add Mode
            If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

            End If

            ' Validate Parameters
            m_lReturn = CType(Validate(vPartyCnt:=vPartyCnt, vProductType:=vProductType, vTransactionType:=vTransactionType, vFeePercentage:=vFeePercent, vFeeAmount:=vFeeAmount, vEffectiveDate:=vEffectiveDate, vIsTaxable:=vIsTaxable), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Set Data Changed Flag to False
            bDataChanged = False

            ' Set Property values.


            If (Not False) And (Not vPartyCnt.Equals(0)) Then
                PartyCnt = vPartyCnt
            End If


            If (Not False) And (Not vProductType.Equals(0)) Then
                ProductType = vProductType
            End If


            If (Not False) And (Not vTransactionType.Equals(0)) Then
                TransactionType = vTransactionType
            End If



            If (Not Informations.IsNothing(vFeePercent)) And (Not vFeePercent.Equals(0)) Then
                FeePercentage = vFeePercent
            End If



            If (Not Informations.IsNothing(vFeeAmount)) And (Not vFeeAmount.Equals(0)) Then
                FeeAmount = vFeeAmount
            End If


            If (Not False) And (Not vEffectiveDate.Equals(DateTime.FromOADate(0))) Then
                EffectiveDate = vEffectiveDate
            End If


            If (Not False) And (Not vIsTaxable.Equals(0)) Then
                IsTaxable = vIsTaxable
            End If

            ' If we have changed one of the properties, update the status
            m_iDatabaseStatus = iStatus


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProperties", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function UpdatePremium(ByRef vPremium As Object) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Add the required INPUT parameters
                m_lReturn = CType(AddInputParamUpdatePremium(vPremium:=vPremium), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACUpdatePremiumDetailsSQL, sSQLName:=ACUpdatePremiumDetailsName, bStoredProcedure:=ACUpdatePremiumDetailsStored, lRecordsAffected:=lRecordsAffected)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Check to see that the record was UpdatePremiumd OK
                If lRecordsAffected > 0 Then
                    ' UpdatePremiumd No action required
                Else
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With


            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result




        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the SIRPartyFee for Consistency.
    '
    '
    ' ***************************************************************** '
    ' PW100702 - add consultant/agent
    ' GW180504 - add extra param to see if business is underwriting or agency
    Public Function Validate(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vProductType As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vFeePercentage As Object = Nothing, Optional ByRef vFeeAmount As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef vIsTaxable As Object = Nothing) As Integer

        Dim result As Integer = 0


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Validate

            ' {* USER DEFINED CODE (Begin) *}


            If Not Informations.IsNothing(vPartyCnt) Then

                Dim dbNumericTemp As Double
                If Not Double.TryParse(CStr(vPartyCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            If Not Informations.IsNothing(vProductType) Then

                Dim dbNumericTemp2 As Double
                If Not Double.TryParse(CStr(vProductType), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            If Not Informations.IsNothing(vTransactionType) Then

                Dim dbNumericTemp3 As Double
                If Not Double.TryParse(CStr(vProductType), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'check to see tat both fee amount and fee percentage are not null

            If (Not Informations.IsNothing(vFeeAmount)) Or (Not Informations.IsNothing(vFeePercentage)) Then
                If (vFeeAmount = 0) And (vFeePercentage = 0) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If




            If Not Informations.IsNothing(vEffectiveDate) Then
                If Not Informations.IsDate(vEffectiveDate) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Validate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Validate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetFeeDetails
    '
    ' Description: Get party fee details for a given party_cnt
    '
    ' ***************************************************************** '
    Public Function GetAllFeeDetails(ByRef vPartyCnt As Object, Optional ByRef vFeeDetails(,) As Object = Nothing) As Integer

        Dim result As Integer = 0



        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()


                m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(vPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                'get details
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllFeeAmountForUnderwritingDetailsSQL, sSQLName:=ACGetAllFeeAmountForUnderwritingDetailsName, bStoredProcedure:=ACGetAllFeeAmountForUnderwritingDetailsStored, lNumberRecords:=0, vResultArray:=vFeeDetails)

                ' "'" & & "'"
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'return the data
                If Not Informations.IsArray(vFeeDetails) Then
                    result = gPMConstants.PMEReturnCode.PMNotFound
                End If

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetAllFeeDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFeeDetailsGetAllFeeDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'Added 991005 MK
    ' ***************************************************************** '
    ' Name: GetFeeDetails
    '
    ' Description: Get party fee details for a given party_cnt
    '
    ' ***************************************************************** '
    Public Function GetSpecificFeeDetails(ByRef vPartyCnt As Object, ByRef vFeeAmountID As Object, Optional ByRef vFeeDetails As Object = Nothing) As Integer

        Dim result As Integer = 0


        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If (Not False) And (Not Object.Equals(vPartyCnt, Nothing)) Then

                m_oDatabase.Parameters.Clear()

                m_lReturn = m_oDatabase.Parameters.Add(sName:="fee_amount_id", vValue:=CStr(vFeeAmountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(vPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                'Get form DB

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetSpecificFeeAmountForUnderwritingDetailsSQL, sSQLName:=ACGetSpecificFeeAmountForUnderwritingDetailsName, bStoredProcedure:=ACGetSpecificFeeAmountForUnderwritingDetailsStored, lNumberRecords:=0, vResultArray:=vFeeDetails)

                ' "'" & & "'"
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    vFeeDetails = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                End If
                'return the data
                If Not Informations.IsArray(vFeeDetails) Then
                    result = gPMConstants.PMEReturnCode.PMNotFound
                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetSpecificFeeDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSpecificFeeDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDetails
    '
    ' Description: Get details from DB, for related parties.
    '
    ' ***************************************************************** '
    Public Function GetDetails(ByRef vTransType As Object, ByRef vProdType As Object, ByRef vInsuranceFileCnt As Object, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            'transaction type id

            m_lReturn = m_oDatabase.Parameters.Add(sName:="TransType", vValue:=CStr(vTransType), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'insurance file id

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ProdType", vValue:=CStr(vProdType), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Insurance_file_cnt

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(vInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get from DB
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRoadMapStepDetailsDetailsSQL, sSQLName:=ACGetRoadMapStepDetailsName, bStoredProcedure:=ACGetRoadMapStepDetailsStored, lNumberRecords:=0, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Public Function GetProductID(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lProductID As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vResultArray(,) As Object = Nothing
            Dim sSQL As String = ""

            m_oDatabase.Parameters.Clear()

            sSQL = "SELECT product_id FROM insurance_file WHERE insurance_file_cnt = " & v_lInsuranceFileCnt

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetPolicyID", bStoredProcedure:=False, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            r_lProductID = CInt(vResultArray(0, 0))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetProductID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProductID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' AddUWFeeAmount
    ''' </summary>
    ''' <param name="v_vRiskTypeGroupId"></param>
    ''' <param name="v_vFeepercentage"></param>
    ''' <param name="v_vFeeAmount"></param>
    ''' <param name="v_vTransactionTypeId"></param>
    ''' <param name="v_vCurrencyID"></param>
    ''' <param name="v_vProductId"></param>
    ''' <param name="v_vPerilGroupId"></param>
    ''' <param name="v_vTransactionSubType"></param>
    ''' <param name="v_vTaxGroupId"></param>
    ''' <param name="v_vIsFeeAppliedToCr"></param>
    ''' <param name="v_vEffectiveDate"></param>
    ''' <param name="v_vPartyCnt"></param>
    ''' <param name="v_vIncludeToInstalment"></param>
    ''' <param name="v_vSpreadAcrossInstalment"></param>
    ''' <param name="v_oMakeLiveOptions"></param>
    ''' <param name="v_oPaymentTerm"></param>
    ''' <param name="v_oNetPremiumWithTax"></param>
    ''' <param name="v_oApplyProrated"></param>
    ''' <param name="v_oOverrideRateAmount"></param>
    ''' <param name="v_nUseWhenDeleted"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddUWFeeAmount(ByVal v_vRiskTypeGroupId As Object, ByVal v_vFeepercentage As Object, ByVal v_vFeeAmount As Object, ByVal v_vTransactionTypeId As Object, ByVal v_vCurrencyID As Object, ByVal v_vProductId As Object, ByVal v_vPerilGroupId As Object, ByVal v_vTransactionSubType As Object, ByVal v_vTaxGroupId As Object, ByVal v_vIsFeeAppliedToCr As Object, ByVal v_vEffectiveDate As Object, ByVal v_vPartyCnt As Object, ByVal v_vIncludeToInstalment As Object, ByVal v_vSpreadAcrossInstalment As Object, ByVal v_oMakeLiveOptions As Object, ByVal v_oPaymentTerm As Object, ByVal v_oNetPremiumWithTax As Object, ByVal v_oApplyProrated As Object, ByVal v_oOverrideRateAmount As Object, ByVal v_nUseWhenDeleted As Integer, Optional ByVal v_sUniqueId As String = "", Optional ByVal v_sScreenHeirarchy As String = "") As Integer

        Const kMethodName As String = "AddUWFeeAmount"

        Dim nResult As PMEReturnCode

        Try
            nResult = PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters

            nResult = CType(AddInputParameter(v_sName:="party_cnt", v_vValue:=v_vPartyCnt, v_iType:=PMEDataType.PMLong), PMEReturnCode)
            If nResult <> PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "AddInputParameter Failed", PMELogLevel.PMLogError)
            End If

            nResult = CType(AddFeeAmountsUWParams(v_vRiskTypeGroupId:=v_vRiskTypeGroupId, v_vFeepercentage:=v_vFeepercentage, v_vFeeAmount:=v_vFeeAmount, v_vTransactionTypeId:=v_vTransactionTypeId, v_vCurrencyID:=v_vCurrencyID, v_vProductId:=v_vProductId, v_vPerilGroupId:=v_vPerilGroupId, v_vTransactionSubType:=v_vTransactionSubType, v_vTaxGroupId:=v_vTaxGroupId, v_vIsFeeAppliedToCr:=v_vIsFeeAppliedToCr, v_vEffectiveDate:=v_vEffectiveDate, v_vIncludeToInstalment:=v_vIncludeToInstalment, v_vSpreadAcrossInstalment:=v_vSpreadAcrossInstalment, v_oMakeLiveOptions:=v_oMakeLiveOptions, v_oPaymentTerm:=v_oPaymentTerm, v_oNetPremiumWithTax:=v_oNetPremiumWithTax, v_oApplyProrated:=v_oApplyProrated, v_oOverrideRateAmount:=v_oOverrideRateAmount, v_nUseWhenDeleted:=v_nUseWhenDeleted, v_sUniqueId:=v_sUniqueId, v_sScreenHeirarchy:=v_sScreenHeirarchy), PMEReturnCode)
            If nResult <> PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "AddFeeAmountUWParams Failed", PMELogLevel.PMLogError)
            End If

            ' Execute Action Query
            If m_oDatabase.SQLAction(sSQL:=kAddUWFeeAmountSQL, sSQLName:=kAddUWFeeAmountName, bStoredProcedure:=True) <> PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kAddUWFeeAmountSQL & " Failed", PMELogLevel.PMLogError)

            End If

            Return nResult

        Catch ex As Exception
            nResult = PMEReturnCode.PMError            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)
            Return nResult
        End Try
    End Function

    ''' <summary>
    ''' EditUWFeeAmount
    ''' </summary>
    ''' <param name="v_vFeeAmountId"></param>
    ''' <param name="v_vRiskTypeGroupId"></param>
    ''' <param name="v_vFeepercentage"></param>
    ''' <param name="v_vFeeAmount"></param>
    ''' <param name="v_vTransactionTypeId"></param>
    ''' <param name="v_vCurrencyID"></param>
    ''' <param name="v_vProductId"></param>
    ''' <param name="v_vPerilGroupId"></param>
    ''' <param name="v_vTransactionSubType"></param>
    ''' <param name="v_vTaxGroupId"></param>
    ''' <param name="v_vIsFeeAppliedToCr"></param>
    ''' <param name="v_vEffectiveDate"></param>
    ''' <param name="v_vIncludeToInstalment"></param>
    ''' <param name="v_vSpreadAcrossInstalment"></param>
    ''' <param name="v_oMakeLiveOptions"></param>
    ''' <param name="v_oPaymentTerm"></param>
    ''' <param name="v_oNetPremiumWithTax"></param>
    ''' <param name="v_oApplyProrated"></param>
    ''' <param name="v_oOverrideRateAmount"></param>
    ''' <param name="v_nUseWhenDeleted"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function EditUWFeeAmount(ByVal v_vFeeAmountId As Object, ByVal v_vRiskTypeGroupId As Object, ByVal v_vFeepercentage As Object, ByVal v_vFeeAmount As Object, ByVal v_vTransactionTypeId As Object, ByVal v_vCurrencyID As Object, ByVal v_vProductId As Object, ByVal v_vPerilGroupId As Object, ByVal v_vTransactionSubType As Object, ByVal v_vTaxGroupId As Object, ByVal v_vIsFeeAppliedToCr As Object, ByVal v_vEffectiveDate As Object, ByVal v_vIncludeToInstalment As Object, ByVal v_vSpreadAcrossInstalment As Object, ByVal v_oMakeLiveOptions As Object, ByVal v_oPaymentTerm As Object, ByVal v_oNetPremiumWithTax As Object, ByVal v_oApplyProrated As Object, ByVal v_oOverrideRateAmount As Object, ByVal v_nUseWhenDeleted As Integer, Optional ByVal v_sUniqueId As String = "", Optional ByVal v_sScreenHeirarchy As String = "") As Integer

        Dim nResult As Integer
        Const kMethodName As String = "EditUWFeeAmount"

        Try
            nResult = PMEReturnCode.PMTrue
            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters

            nResult = CType(AddInputParameter(v_sName:="fee_amount_id", v_vValue:=CStr(v_vFeeAmountId), v_iType:=PMEDataType.PMLong), PMEReturnCode)
            If nResult <> PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "AddInputParameter Failed", PMELogLevel.PMLogError)
            End If

            ' Add Required Stored Procedure Parameters

            nResult = CType(AddFeeAmountsUWParams(v_vRiskTypeGroupId:=CStr(v_vRiskTypeGroupId), v_vFeepercentage:=CStr(v_vFeepercentage), v_vFeeAmount:=CStr(v_vFeeAmount), v_vTransactionTypeId:=CStr(v_vTransactionTypeId), v_vCurrencyID:=CStr(v_vCurrencyID), v_vProductId:=CStr(v_vProductId), v_vPerilGroupId:=CStr(v_vPerilGroupId), v_vTransactionSubType:=CStr(v_vTransactionSubType), v_vTaxGroupId:=CStr(v_vTaxGroupId), v_vIsFeeAppliedToCr:=CStr(v_vIsFeeAppliedToCr), v_vEffectiveDate:=CStr(v_vEffectiveDate), v_vIncludeToInstalment:=CStr(v_vIncludeToInstalment), v_vSpreadAcrossInstalment:=CStr(v_vSpreadAcrossInstalment), v_oMakeLiveOptions:=v_oMakeLiveOptions, v_oPaymentTerm:=v_oPaymentTerm, v_oNetPremiumWithTax:=v_oNetPremiumWithTax, v_oApplyProrated:=v_oApplyProrated, v_oOverrideRateAmount:=v_oOverrideRateAmount, v_nUseWhenDeleted:=v_nUseWhenDeleted, v_sUniqueId:=v_sUniqueId, v_sScreenHeirarchy:=v_sScreenHeirarchy), PMEReturnCode)
            If nResult <> PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "AddFeeAmountUWParams Failed", PMELogLevel.PMLogError)
            End If

            ' Execute Action Query
            If m_oDatabase.SQLAction(sSQL:=kEditUWFeeAmountSQL, sSQLName:=kEditUWFeeAmountName, bStoredProcedure:=True) <> PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kEditUWFeeAmountSQL & " Failed", PMELogLevel.PMLogError)

            End If
            Return nResult

        Catch ex As Exception
            nResult = PMEReturnCode.PMError            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)
            Return nResult
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetUWFeeAmount
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 19-04-2005 : AUS005
    ' ***************************************************************** '
    Public Function GetUWFeeAmount(ByVal v_lFeeAmountId As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetUWFeeAmount"


        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="fee_amount_id", v_vValue:=CStr(v_lFeeAmountId), v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute selection Query
            If m_oDatabase.SQLSelect(sSQL:=kGetUWFeeAmountSQL, sSQLName:=kGetUWFeeAmountName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetUWFeeAmountSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If

            Return result
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
    ' Name: GetTableLookupValues
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 19-04-2005 : AUS005
    ' ***************************************************************** '
    Public Function GetTableLookupValues(ByVal v_sTableName As String, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetTableLookupValues"


        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="table", v_vValue:=v_sTableName, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)

            ' Execute selection Query
            If m_oDatabase.SQLSelect(sSQL:=kGetTableLookupValuesSQL, sSQLName:=kGetTableLookupValuesName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetTableLookupValuesSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If

            Return result
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
    '           Created : MEvans : 19-04-2005 : AUS005
    ' ***************************************************************** '
    'developer guide no. 101
    Private Function AddInputParameter(ByVal v_sName As Object, ByVal v_vValue As Object, ByVal v_iType As Object) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "AddInputParameter"



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Add Parameter to database object
        If m_oDatabase.Parameters.Add(sName:=v_sName, vValue:=v_vValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iType) <> gPMConstants.PMEReturnCode.PMTrue Then

            ' Log Error.
            result = gPMConstants.PMEReturnCode.PMFalse

            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to add parameter name:" & v_sName &
                                          ", values :" & v_vValue & ", type:" & CStr(v_iType), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Informations.Err().Description))

        End If

        Return result

    End Function
    ''' <summary>
    ''' AddFeeAmountsUWParams
    ''' </summary>
    ''' <param name="v_vRiskTypeGroupId"></param>
    ''' <param name="v_vFeepercentage"></param>
    ''' <param name="v_vFeeAmount"></param>
    ''' <param name="v_vTransactionTypeId"></param>
    ''' <param name="v_vCurrencyID"></param>
    ''' <param name="v_vProductId"></param>
    ''' <param name="v_vPerilGroupId"></param>
    ''' <param name="v_vTransactionSubType"></param>
    ''' <param name="v_vTaxGroupId"></param>
    ''' <param name="v_vIsFeeAppliedToCr"></param>
    ''' <param name="v_vEffectiveDate"></param>
    ''' <param name="v_vIncludeToInstalment"></param>
    ''' <param name="v_vSpreadAcrossInstalment"></param>
    ''' <param name="v_oMakeLiveOptions"></param>
    ''' <param name="v_oPaymentTerm"></param>
    ''' <param name="v_oNetPremiumWithTax"></param>
    ''' <param name="v_oApplyProrated"></param>
    ''' <param name="v_oOverrideRateAmount"></param>
    ''' <param name="v_nUseWhenDeleted"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddFeeAmountsUWParams(ByVal v_vRiskTypeGroupId As Object, ByVal v_vFeepercentage As Object, ByVal v_vFeeAmount As Object, ByVal v_vTransactionTypeId As Object, ByVal v_vCurrencyID As Object, ByVal v_vProductId As Object, ByVal v_vPerilGroupId As Object, ByVal v_vTransactionSubType As Object, ByVal v_vTaxGroupId As Object, ByVal v_vIsFeeAppliedToCr As Object, ByVal v_vEffectiveDate As Object, ByVal v_vIncludeToInstalment As Object, ByVal v_vSpreadAcrossInstalment As Object, ByVal v_oMakeLiveOptions As Object, ByVal v_oPaymentTerm As Object, ByVal v_oNetPremiumWithTax As Object, ByVal v_oApplyProrated As Object, ByVal v_oOverrideRateAmount As Object, ByVal v_nUseWhenDeleted As Integer, ByVal v_sUniqueId As String, ByVal v_sScreenHeirarchy As String) As Integer

        Dim nResult As Integer
        Const kMethodName As String = "AddFeeAmountsUWParams"

        Try

            nResult = PMEReturnCode.PMTrue

            ' reset values for combo's to null if set to zero
            v_vRiskTypeGroupId = ReplaceNullWithDefault(v_vRiskTypeGroupId, DBNull.Value)

            v_vPerilGroupId = ReplaceNullWithDefault(v_vPerilGroupId, DBNull.Value)

            v_vCurrencyID = ReplaceNullWithDefault(v_vCurrencyID, DBNull.Value)

            v_vTaxGroupId = ReplaceNullWithDefault(v_vTaxGroupId, DBNull.Value)

            v_oMakeLiveOptions = ReplaceNullWithDefault(v_oMakeLiveOptions, DBNull.Value)
            v_oPaymentTerm = ReplaceNullWithDefault(v_oPaymentTerm, DBNull.Value)
            If v_vTransactionSubType = kTransSubTypeAll Then
                v_vTransactionSubType = DBNull.Value
            End If

            ' because zero is a valid value for v_vProductId
            ' but only one out of riskgroup or perilgroup or product may be set
            ' if we get a product id with a zero value but
            ' check if either the riskgroup or the perilgroup are set
            ' if they are then the productid should be reset to "null"

            If Not (Convert.IsDBNull(v_vRiskTypeGroupId) OrElse Informations.IsNothing(v_vRiskTypeGroupId)) OrElse Not (Convert.IsDBNull(v_vPerilGroupId) OrElse Informations.IsNothing(v_vPerilGroupId)) Then

                v_vProductId = Nothing
            End If

            nResult = CType(AddInputParameter("risk_type_group_id", v_vRiskTypeGroupId, gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "AddInputParameter Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            nResult = CType(AddInputParameter("fee_percentage", v_vFeepercentage, gPMConstants.PMEDataType.PMCurrency), gPMConstants.PMEReturnCode)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "AddInputParameter Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            nResult = CType(AddInputParameter("fee_amount", v_vFeeAmount, gPMConstants.PMEDataType.PMCurrency), gPMConstants.PMEReturnCode)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "AddInputParameter Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            nResult = CType(AddInputParameter("transaction_type_id", v_vTransactionTypeId, gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "AddInputParameter Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            nResult = CType(AddInputParameter("currency_id", v_vCurrencyID, gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "AddInputParameter Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            nResult = CType(AddInputParameter("product_id", v_vProductId, gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "AddInputParameter Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            nResult = CType(AddInputParameter("peril_group_id", v_vPerilGroupId, gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "AddInputParameter Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            nResult = CType(AddInputParameter("transaction_sub_type", v_vTransactionSubType, gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "AddInputParameter Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            nResult = CType(AddInputParameter("tax_group_id", v_vTaxGroupId, gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "AddInputParameter Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            nResult = CType(AddInputParameter("is_fee_applied_to_cr", v_vIsFeeAppliedToCr, gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "AddInputParameter Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            nResult = CType(AddInputParameter("effective_date", v_vEffectiveDate, gPMConstants.PMEDataType.PMDate), gPMConstants.PMEReturnCode)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "AddInputParameter Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            nResult = CType(AddInputParameter("include_fee_in_instalments", v_vIncludeToInstalment, gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "AddInputParameter Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            nResult = CType(AddInputParameter("spread_fee_across_instalments", v_vSpreadAcrossInstalment, gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "AddInputParameter Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            nResult = CType(AddInputParameter("nMakeLiveOptionsId", v_oMakeLiveOptions, gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "AddInputParameter Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            nResult = CType(AddInputParameter("nDoPaymentTermsId", v_oPaymentTerm, gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "AddInputParameter Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If ToSafeBoolean(v_oNetPremiumWithTax) Then
                v_oNetPremiumWithTax = 1
            Else
                v_oNetPremiumWithTax = 0
            End If
            If ToSafeBoolean(v_oApplyProrated) Then
                v_oApplyProrated = 1
            Else
                v_oApplyProrated = 0
            End If

            nResult = CType(AddInputParameter("nCalculationBasis", v_oNetPremiumWithTax, gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "AddInputParameter Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            nResult = CType(AddInputParameter("bIsProrated", v_oApplyProrated, gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "AddInputParameter Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            nResult = CType(AddInputParameter("nOverrideRateAmount", v_oOverrideRateAmount, gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "AddInputParameter Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            nResult = CType(AddInputParameter("nUseWhenDeleted", v_nUseWhenDeleted, gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "AddInputParameter Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            nResult = CType(AddInputParameter("nUserId", m_iUserID, gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "AddInputParameter Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            nResult = CType(AddInputParameter("nUniqueId", v_sUniqueId, gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "AddInputParameter Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            nResult = CType(AddInputParameter("nScreenHierarchy", v_sScreenHeirarchy, gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "AddInputParameter Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            Return nResult
        Catch ex As Exception
            nResult = PMEReturnCode.PMError
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)
            Return nResult
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: ReplaceNullWithDefault
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 19-04-2005 : AUS005
    ' ***************************************************************** '
    'developer guide no. 101
    Private Function ReplaceNullWithDefault(ByRef v_vValue As Object, ByVal v_vDefault As Object, Optional ByVal v_bIncludeZeroValuesAsNull As Object = Nothing) As Object

        Dim result As String = String.Empty
        Const sFunctionName As String = "ReplaceNullWithDefault"



        Dim bProcessed As Boolean

        result = CStr(gPMConstants.PMEReturnCode.PMTrue)

        If v_vValue Is DBNull.Value.ToString() OrElse Informations.IsNothing(v_vValue) OrElse v_vValue = 0 Then
            v_vValue = v_vDefault
            bProcessed = True
        End If

        If Not bProcessed Then
            If v_bIncludeZeroValuesAsNull Then
                If ToSafeDouble(v_vValue) = 0 Then
                    v_vValue = v_vDefault
                End If
            End If
        End If


        Return v_vValue


    End Function

    ' ***************************************************************** '
    ' Name: CalculateFeeTaxAmount
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 19-04-2005 : AUS005
    ' ***************************************************************** '
    Public Function CalculateFeeTaxAmount(ByVal v_lTaxGroupId As Integer, ByVal v_lCompany_id As Integer, ByVal v_lFeeCurrencyId As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_crFeeAmount As Decimal, ByRef r_crTaxAmount As Decimal) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CalculateFeeTaxAmount"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim llBound, lUBound, lIsValue As Integer
        Dim crRate, crTaxAmount, crTotalTaxAmount As Decimal
        Dim vTaxRates(,) As Object = Nothing
        Dim lRateCurrencyId As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = CType(GetTaxGroupTaxRates(v_lTaxGroupId:=v_lTaxGroupId, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_vResults:=vTaxRates), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetTaxGoupTaxRates Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Informations.IsArray(vTaxRates) Then


                llBound = vTaxRates.GetLowerBound(1)

                lUBound = vTaxRates.GetUpperBound(1)

                For lRate As Integer = llBound To lUBound

                    'tt.tax_type_id
                    'tt.description
                    'tb.tax_band_id
                    'tb.description
                    'tbr.is_value
                    'tbr.rate
                    'tt.code
                    'tbr.currency_id


                    lIsValue = CInt(vTaxRates(4, lRate))

                    crRate = CDec(vTaxRates(5, lRate))

                    lRateCurrencyId = CInt(vTaxRates(7, lRate))
                    crTaxAmount = 0

                    ' if the tax rate is a value in a given currency rather than a percentage
                    ' then this value needs to be converted into the same currency as the policy fee
                    ' before it can be applied
                    If lIsValue = 1 Then

                        ' convert value

                        lReturn = m_oCurrencyConvert.CurrencyToCurrencyConversion(v_lCurrencyIdFrom:=lRateCurrencyId, v_crCurrencyAmountFrom:=crRate, v_lCompanyId:=v_lCompany_id, v_lCurrencyIdTo:=v_lFeeCurrencyId, r_crCurrencyAmountTo:=crTaxAmount)

                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "CurrencyToCurrencyConversion Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                    Else
                        ' tax amount is a percentage (rate = %)
                        crTaxAmount = v_crFeeAmount * (crRate / 100)
                    End If

                    ' store the total tax amount
                    crTotalTaxAmount += crTaxAmount

                Next

                r_crTaxAmount = crTotalTaxAmount

            End If

            Return result
        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost
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
    ' Name: GetTaxGroupTaxRates
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 19-04-2005 : AUS005
    ' ***************************************************************** '
    Private Function GetTaxGroupTaxRates(ByVal v_lTaxGroupId As Integer, ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetTaxGroupTaxRates"

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        m_lReturn = CType(AddInputParameter(v_sName:="tax_group_id", v_vValue:=CStr(v_lTaxGroupId), v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
        m_lReturn = CType(AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=CStr(v_lInsuranceFileCnt), v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

        ' Execute selection Query
        If m_oDatabase.SQLSelect(sSQL:=kGetTaxGroupTaxRatesSQL, sSQLName:=kGetTaxGroupTaxRatesName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

            gPMFunctions.RaiseError(kMethodName, kGetTaxGroupTaxRatesSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

        End If

        Return result
    End Function

    ''' <summary>
    ''' RecalculateRiskFees
    ''' </summary>
    ''' <param name="v_lInsuranceFileCnt"></param>
    ''' <param name="v_lRiskCnt"></param>
    ''' <param name="v_lTransactionTypeId"></param>
    ''' <param name="v_bUseExistingFeeDetail"></param>
    ''' <param name="v_sTransactionType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function RecalculateRiskFees(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lRiskCnt As Integer, ByVal v_lTransactionTypeId As Integer, Optional ByVal v_bUseExistingFeeDetail As Boolean = False, Optional ByVal v_sTransactionType As String = "") As Integer

        Dim nResult As Integer
        Const kMethodName As String = "RecalculateRiskFees"
        Try

            nResult = PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            nResult = CType(AddInputParameter(v_sName:="transaction_type_id", v_vValue:=CStr(v_lTransactionTypeId), v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            nResult = CType(AddInputParameter(v_sName:="risk_cnt", v_vValue:=CStr(v_lRiskCnt), v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            nResult = CType(AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=CStr(v_lInsuranceFileCnt), v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' policy discount / loading requires fee details not to be reloaded when recalculating
            ' where only the underlying premium details will have changed
            If v_bUseExistingFeeDetail Then
                nResult = CType(AddInputParameter(v_sName:="use_existing_fee_details", v_vValue:=CStr(1), v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            End If

            nResult = AddInputParameter(v_sName:="sTransactionType", v_vValue:=v_sTransactionType, v_iType:=gPMConstants.PMEDataType.PMString)

            ' Execute selection Query
            If m_oDatabase.SQLAction(sSQL:=kRecalculateRiskFeesSQL, sSQLName:=kRecalculateRiskFeesName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kRecalculateRiskFeesSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If
            Return nResult
        Catch ex As exception
            nResult = PMEReturnCode.PMError
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)
            Return nResult
        End Try
    End Function
    ''' <summary>
    ''' RecalculatePolicyFees
    ''' </summary>
    ''' <param name="v_lInsuranceFileCnt"></param>
    ''' <param name="v_lProductId"></param>
    ''' <param name="v_lTransactionTypeId"></param>
    ''' <param name="v_bUseExistingFeeDetail"></param>
    ''' <param name="v_sMakeLivePaymentTerms"></param>
    ''' <param name="v_sMakeLivePaymentDebitOrCash"></param>
    ''' <param name="v_sTransactionType"></param>
    ''' <param name="nViaSam"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function RecalculatePolicyFees(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lProductId As Integer, ByVal v_lTransactionTypeId As Integer, Optional ByVal v_bUseExistingFeeDetail As Boolean = False, Optional ByVal v_sMakeLivePaymentTerms As String = "", Optional ByVal v_sMakeLivePaymentDebitOrCash As String = "", Optional ByVal v_sTransactionType As String = "", Optional ByVal nViaSam As Integer = 0) As Integer

        Return RecalculatePolicyFees(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lProductId:=v_lProductId, v_lTransactionTypeId:=v_lTransactionTypeId, v_bIsBackdatedMTA:=False,
                                     v_bUseExistingFeeDetail:=v_bUseExistingFeeDetail, v_sMakeLivePaymentTerms:=v_sMakeLivePaymentTerms, v_sMakeLivePaymentDebitOrCash:=v_sMakeLivePaymentDebitOrCash, v_sTransactionType:=v_sTransactionType, nViaSam:=nViaSam)

    End Function
    ''' <summary>
    ''' RecalculatePolicyFees
    ''' </summary>
    ''' <param name="v_lInsuranceFileCnt"></param>
    ''' <param name="v_lProductId"></param>
    ''' <param name="v_lTransactionTypeId"></param>
    ''' <param name="v_bIsBackdatedMTA"></param>
    ''' <param name="v_bUseExistingFeeDetail"></param>
    ''' <param name="v_sMakeLivePaymentTerms"></param>
    ''' <param name="v_sMakeLivePaymentDebitOrCash"></param>
    ''' <param name="v_sTransactionType"></param>
    ''' <param name="nViaSam"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function RecalculatePolicyFees(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lProductId As Integer, ByVal v_lTransactionTypeId As Integer, ByVal v_bIsBackdatedMTA As Boolean, Optional ByVal v_bUseExistingFeeDetail As Boolean = False, Optional ByVal v_sMakeLivePaymentTerms As String = "", Optional ByVal v_sMakeLivePaymentDebitOrCash As String = "", Optional ByVal v_sTransactionType As String = "", Optional ByVal nViaSam As Integer = 0) As Integer

        Dim nResult As Integer
        Const kMethodName As String = "RecalculatePolicyFees"
        Try
            nResult = PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            nResult = CType(AddInputParameter(v_sName:="transaction_type_id", v_vValue:=CStr(v_lTransactionTypeId),
                                                v_iType:=gPMConstants.PMEDataType.PMLong),
                              gPMConstants.PMEReturnCode)
            nResult = CType(AddInputParameter(v_sName:="product_id", v_vValue:=CStr(v_lProductId),
                                                v_iType:=gPMConstants.PMEDataType.PMLong),
                              gPMConstants.PMEReturnCode)
            nResult = CType(AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=CStr(v_lInsuranceFileCnt),
                                                v_iType:=gPMConstants.PMEDataType.PMLong),
                              gPMConstants.PMEReturnCode)

            ' policy discount / loading requires fee details not to be reloaded when recalculating
            ' where only the underlying premium details will have changed
            If v_bUseExistingFeeDetail Then
                nResult = CType(AddInputParameter(v_sName:="use_existing_fee_details", v_vValue:=CStr(1), v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            End If

            If v_sMakeLivePaymentTerms <> "" Then
                nResult = CType(AddInputParameter(v_sName:="sMakeLiveOptions", v_vValue:=v_sMakeLivePaymentTerms, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)
            End If
            If v_sMakeLivePaymentDebitOrCash <> "" Then
                nResult = CType(AddInputParameter(v_sName:="sPaymentDebitOrCash", v_vValue:=v_sMakeLivePaymentDebitOrCash, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)
            End If
            nResult = CType(AddInputParameter(v_sName:="sTransactionType", v_vValue:=v_sTransactionType, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)

            If nViaSam <> 0 Then
                nResult = CType(AddInputParameter(v_sName:="nViaSam", v_vValue:=nViaSam, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            End If

            If v_bIsBackdatedMTA Then
                nResult = CType(AddInputParameter(v_sName:="Is_Backdated_MTA", v_vValue:=CStr(1), v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            End If

            ' Execute selection Query
            If _
                m_oDatabase.SQLAction(sSQL:=kRecalculatePolicyFeesSQL, sSQLName:=kRecalculatePolicyFeesName,
                                      bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kRecalculatePolicyFeesSQL & " Failed",
                                        gPMConstants.PMELogLevel.PMLogError)

            End If
            Return nResult
        Catch ex As Exception
            nResult = PMEReturnCode.PMError
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)
            Return nResult
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetRiskFees
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 19-04-2005 : AUS005
    ' ***************************************************************** '
    Public Function GetRiskFees(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lRiskCnt As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetRiskFees"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters

            m_lReturn = CType(AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=CStr(v_lInsuranceFileCnt), v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            m_lReturn = CType(AddInputParameter(v_sName:="risk_cnt", v_vValue:=CStr(v_lRiskCnt), v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute selection Query
            If m_oDatabase.SQLSelect(sSQL:=kGetRiskFeesSQL, sSQLName:=kGetRiskFeesName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetRiskFeesSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If

            Return result
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
    ' Name: GetPolicyFees
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 19-04-2005 : AUS005
    ' ***************************************************************** '
    Public Function GetPolicyFees(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPolicyFees"


        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters

            m_lReturn = CType(AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=CStr(v_lInsuranceFileCnt), v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute selection Query
            If m_oDatabase.SQLSelect(sSQL:=kGetPolicyFeesSQL, sSQLName:=kGetPolicyFeesName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetPolicyFeesSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If

            Return result
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
    ' Name: DeletePolicyFee
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function DeletePolicyFee(ByVal v_lPolicyFeeUId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DeletePolicyFee"



        Try




            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="policy_fee_u_id", v_vValue:=CStr(v_lPolicyFeeUId), v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute Action Query
            If m_oDatabase.SQLAction(sSQL:=kDeletePolicyFeeSQL, sSQLName:=kDeletePolicyFeeName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kDeletePolicyFeeSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If

        Catch ex As Exception



            ' Do Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: UpdatePolicyFee
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function UpdatePolicyFee(ByVal v_lPolicyFeeUId As Integer, ByVal v_crFeePercentage As Decimal, ByVal v_crFeeAmount As Decimal, Optional ByVal v_crOriginalFeeRate As Decimal = 0) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdatePolicyFee"



        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="policy_fee_u_id", v_vValue:=CStr(v_lPolicyFeeUId), v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="fee_percentage", v_vValue:=CStr(v_crFeePercentage), v_iType:=gPMConstants.PMEDataType.PMCurrency), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="fee_amount", v_vValue:=CStr(v_crFeeAmount), v_iType:=gPMConstants.PMEDataType.PMCurrency), gPMConstants.PMEReturnCode)

            ' Execute Action Query
            If m_oDatabase.SQLAction(sSQL:=kUpdatePolicyFeeSQL, sSQLName:=kUpdatePolicyFeeName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kUpdatePolicyFeeSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If

            Return result
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
    ' Name: GetRiskDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function GetRiskDetails(ByVal v_lRiskCnt As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetRiskDetails"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="risk_cnt", v_vValue:=CStr(v_lRiskCnt), v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute selection Query
            If m_oDatabase.SQLSelect(sSQL:=kGetRiskDetailsSQL, sSQLName:=kGetRiskDetailsName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetRiskDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If

            Return result
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
    ' Name: CreateRenewalFees
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 18-07-2005 : PN21887
    ' ***************************************************************** '
    Public Function CreateRenewalFees(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CreateRenewalFees"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vRenewalDetails(,) As Object
        Dim llBound, lUBound, lTransactionTypeId, lProductId, lRiskCnt As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = CType(GetRenewalDetails(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_vResults:=vRenewalDetails), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetRenewalDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Informations.IsArray(vRenewalDetails) Then


                llBound = vRenewalDetails.GetLowerBound(1)

                lUBound = vRenewalDetails.GetUpperBound(1)

                For lRenewalDetail As Integer = llBound To lUBound


                    lProductId = CInt(vRenewalDetails(0, lRenewalDetail))

                    lRiskCnt = CInt(vRenewalDetails(1, lRenewalDetail))

                    lTransactionTypeId = CInt(vRenewalDetails(2, lRenewalDetail))

                    ' only create product fees once
                    If lRenewalDetail = llBound Then

                        ' calculate policy fees
                        lReturn = CType(RecalculatePolicyFees(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lProductId:=lProductId, v_lTransactionTypeId:=lTransactionTypeId), gPMConstants.PMEReturnCode)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "RecalculatePolicyFees Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                    End If

                    ' create fees for each risk
                    lReturn = CType(RecalculateRiskFees(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lRiskCnt:=lRiskCnt, v_lTransactionTypeId:=lTransactionTypeId), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "RecalculateRiskFees Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                Next

            End If

            Return result
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
    Public Function GetFeesTotalDetails(ByVal v_lInsuranceFileCnt As Integer, ByRef vArray(,) As Object) As Integer

        Dim result As Integer = 0


        Const kMethodName As String = "GetFeesTotalDetails"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue
            m_oDatabase.Parameters.Clear()

            m_lReturn = CType(AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=CStr(v_lInsuranceFileCnt), v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=kGetTotalFeeDetailsSQL, sSQLName:=kGetTotalFeeDetailsName, bStoredProcedure:=True, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetFeesTotalDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            Return result

        Catch ex As Exception


            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetRenewalDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 18-07-2005 : Renewals
    ' ***************************************************************** '
    Public Function GetRenewalDetails(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetRenewalDetails"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=CStr(v_lInsuranceFileCnt), v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute selection Query
            If m_oDatabase.SQLSelect(sSQL:=kGetRenewalDetailsSQL, sSQLName:=kGetRenewalDetailsName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetRenewalDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If

            Return result
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

    ''' <summary>
    ''' <see cref="m_oDatabase"></see>
    ''' <see cref="m_lReturn"></see>
    ''' </summary>
    ''' <param name="v_lInsuranceFileCnt"></param>
    ''' <param name="v_lRiskCnt"></param>
    ''' <param name="Rate"></param>   
    ''' <returns></returns>
    ''' <remarks>
    ''' Get the Prorata Rate with required parameters "InsuranceFileCnt","RiskCnt"
    ''' Call spu_get_policy_pro_rata_rate and return rate as double
    ''' </remarks>
    Public Function GetProRataRate(ByVal v_lInsuranceFileCnt As Integer, _
                                ByVal v_lRiskCnt As Integer, _
                                ByRef Rate As Double) As Integer


        Const kMethodName As String = "GetProRataRate"

        Dim nReturn As Integer
        Dim oResults As Object
        Dim nOrginalRiskCnt As Integer

        Try

            GetProRataRate = PMEReturnCode.PMTrue

            ''GetOriginalRiskCnt
            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            nReturn = AddInputParameter(v_sName:="nRiskCnt", v_vValue:=v_lRiskCnt, v_iType:=PMEDataType.PMLong)
            nReturn = m_oDatabase.Parameters.Add(sName:="nOriginalRiskCnt", vValue:=0, iDirection:=PMEParameterDirection.PMParamOutput, iDataType:=PMEDataType.PMLong)

            ' Execute selection Query
            nReturn = m_oDatabase.SQLSelect( _
                                    sSQL:=kGetOriginalRiskCntSQL, _
                                    sSQLName:=kGetOriginalRiskCntName, _
                                    bStoredProcedure:=True, _
                                    vResultArray:=oResults, _
                                    lNumberRecords:=PMConst.PMAllRecords)

            If nReturn <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, kUpdatePolicyFeeSQL & " Failed", PMELogLevel.PMLogError)
            End If
            oResults.len
            If Informations.IsArray(oResults) AndAlso (oResults.len) <> 0 Then
                nOrginalRiskCnt = ToSafeLong(oResults(0, 0))
            End If

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            nReturn = AddInputParameter(v_sName:="nInsuranceFileCnt", v_vValue:=v_lInsuranceFileCnt, v_iType:=PMEDataType.PMLong)
            nReturn = AddInputParameter(v_sName:="sTransactionType", v_vValue:=m_sTransactionType, v_iType:=PMEDataType.PMString)
            nReturn = m_oDatabase.Parameters.Add(sName:="crProrataRate", vValue:=0, iDirection:=PMEParameterDirection.PMParamOutput, iDataType:=PMEDataType.PMDouble)

            ' Execute selection Query
            nReturn = m_oDatabase.SQLSelect( _
                                   sSQL:=kGetProRataRateSQL, _
                                   sSQLName:=kGetProRataRateName, _
                                   bStoredProcedure:=True, _
                                   vResultArray:=oResults, _
                                   lNumberRecords:=PMConst.PMAllRecords)

            If nReturn <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, kUpdatePolicyFeeSQL & " Failed", PMELogLevel.PMLogError)
            End If

            Rate = NullToDouble(m_oDatabase.Parameters.Item("crProrataRate").Value)

            Return nReturn
        Catch ex As System.Exception
            nReturn = PMEReturnCode.PMError
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="GetProRataRate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)
            ' If you want to rollback a transaction or something, do it here
            Return nReturn
        End Try
    End Function
#End Region

End Class
