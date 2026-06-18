Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports System.Windows.Forms
'developer guide no. 129 (guide)
Imports SharedFiles
Partial Public Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 26/07/1999
    '
    ' Description: Main interface.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"
    'developer guide no. 7
    Private Const vbFormCode As Integer = 0
    Private Const ACBankSuspenseAccount As Integer = 2
    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' {* USER DEFINED CODE (Begin) *}
    'Private m_lBankAccountId As Integer
    Private Shared m_lBankAccountId As Integer
    Private m_lAccountId As Integer
    Private m_iBankId As Integer
    Private m_sCode As String = ""
    Private m_iCurrencyId As Integer
    Private m_sBankAccountNo As String = ""
    Private m_sBankAccountName As String = ""
    Private m_sDescription As String = ""
    Private m_lNextChequeNumber As Integer
    Private m_vReconciledDate As Object

    Private m_sFinancialInstitutionCode As String = ""
    Private m_sDirectDebitSupplierName As String = ""
    Private m_lDirectDebitSupplierID As Integer
    Private m_sRemitter As String = ""
    Private m_iProcessingDays As Integer
    Private m_sBIC As String = ""
    Private m_sIBAN As String = ""


    Private m_iIsCashReceiveInThisCurrencyOnly As CheckState
    Private m_sStartChequeNumber As String = ""

    'References from Party Lookups
    Private m_sAccountHolderName As String = ""

    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iACTBankAccount.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Variables to store the lookup values/details.
    Private m_vLookupValues(,) As Object
    Private m_vLookupDetails(,) As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    'sw bank reconcilliation 03/02/2003
    Private m_vRulesData(,) As Object
    Private m_bApply As Boolean
    Private m_lDefaultBankAccount As Integer
    Private m_sDefaultBankAccount As String = ""
    Private m_iBankAccountType As Integer
    Private m_bValidationMessage As Boolean
    Private m_sUniqueId As String = ""
    Private m_sScreenHierarchy As String = ""
    Private m_sBankCode As String = ""
    ' Stores the details from the business object.

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Data Members (End)


    ' PUBLIC Property Procedures (Begin)

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

    'End (Girija chokkalingam) - (Tech Spec - PGR022 - Financial Interfaces.doc) - (5.1.1.1)

    ' {* USER DEFINED CODE (End) *}
    ' PUBLIC Property Procedures (End)

    ' PRIVATE Property Procedures (Begin)

    'Allow this to be changed by General.ProcessCommand
    Public Property Status() As Integer
        Get

            ' Return the interface exit status.
            Return m_lStatus

        End Get
        Set(ByVal Value As Integer)

            ' Set the interface exit status.
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

    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)

            m_lProcessMode = Value

        End Set
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)

            m_sTransactionType = Value

        End Set
    End Property

    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)

            m_dtEffectiveDate = Value

        End Set
    End Property

    Public Property BankID() As Integer
        Get
            Return m_iBankId
        End Get
        Set(ByVal Value As Integer)
            m_iBankId = Value
        End Set
    End Property

    Public Shared Property BankAccountId() As Integer
        Get
            Return m_lBankAccountId
        End Get
        Set(ByVal Value As Integer)
            m_lBankAccountId = Value
        End Set
    End Property
    Public ReadOnly Property BankAccountNo() As String
        Get
            Return m_sBankAccountNo
        End Get
    End Property
    Public ReadOnly Property Code() As String
        Get
            Return m_sCode
        End Get
    End Property
    Public ReadOnly Property BankAccountName() As String
        Get
            Return m_sBankAccountName
        End Get
    End Property
    Public ReadOnly Property Description() As String
        Get
            Return m_sDescription
        End Get
    End Property

    Public Property BankAccountType() As Integer
        Get
            Return m_iBankAccountType
        End Get
        Set(ByVal Value As Integer)
            m_iBankAccountType = Value
        End Set
    End Property


    'Start (Girija chokkalingam) - (Tech Spec - PGR022 - Financial Interfaces.doc) - (5.1.1.1)

    Public Property FinancialInstitutionCode() As String
        Get
            Return m_sFinancialInstitutionCode
        End Get
        Set(ByVal Value As String)
            m_sFinancialInstitutionCode = Value
        End Set
    End Property

    Public Property DirectDebitSupplierName() As String
        Get
            Return m_sDirectDebitSupplierName
        End Get
        Set(ByVal Value As String)
            m_sDirectDebitSupplierName = Value
        End Set
    End Property

    Public Property DirectDebitSupplierID() As Integer
        Get
            Return m_lDirectDebitSupplierID
        End Get
        Set(ByVal Value As Integer)
            m_lDirectDebitSupplierID = Value
        End Set
    End Property

    Public Property Remitter() As String
        Get
            Return m_sRemitter
        End Get
        Set(ByVal Value As String)
            m_sRemitter = Value
        End Set
    End Property

    Public Property ProcessingDays() As Integer
        Get
            Return m_iProcessingDays
        End Get
        Set(ByVal Value As Integer)
            m_iProcessingDays = Value
        End Set
    End Property


    ''' <summary>
    ''' Business Identifier Codes(BIC) used in Party,Instalments,Claim,Cash/Cheque Payment and Receipt
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BIC() As String
        Get
            Return m_sBIC
        End Get
        Set(ByVal value As String)
            m_sBIC = value
        End Set
    End Property

    ''' <summary>
    ''' International Bank Account Number(IBAN) used in Party,Instalments,Claim,Cash/Cheque Payment and Receipt
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IBAN() As String
        Get
            Return m_sIBAN
        End Get
        Set(ByVal value As String)
            m_sIBAN = value
        End Set
    End Property

    Public Property UniqueId() As String
        Get
            Return m_sUniqueId
        End Get
        Set(ByVal value As String)
            m_sUniqueId = value
        End Set
    End Property

    Public Property ScreenHierarchy() As String
        Get
            Return m_sScreenHierarchy
        End Get
        Set(ByVal value As String)
            m_sScreenHierarchy = value
        End Set
    End Property

    Public Property BankCode() As String
        Get
            Return m_sBankCode
        End Get
        Set(ByVal value As String)
            m_sBankCode = value
        End Set
    End Property
    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)
    ' ***************************************************************** '
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields.
    '
    ' ***************************************************************** '

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetFieldValidation() As Integer

        Dim result As Integer = PMEReturnCode.PMTrue
        Try

            m_oFormFields = New iPMFormControl.FormFields()
            m_oFormFields.LanguageID = g_iLanguageID

            If m_iBankAccountType <> ACBankSuspenseAccount Then

                m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAccountNumber, lFieldType:=PMEDataType.PMString, _
                                                          lFormat:=PMEFormatStyle.PMFormatStringUpper, lMandatory:=PMEMandatoryStatus.PMMandatory)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    Return PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAccountCode, lFieldType:=PMEDataType.PMString, _
                                                          lFormat:=PMEFormatStyle.PMFormatString, lMandatory:=PMEMandatoryStatus.PMMandatory)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    Return PMEReturnCode.PMFalse
                End If
            End If


            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAccountName, lFieldType:=PMEDataType.PMString, _
                                                      lFormat:=PMEFormatStyle.PMFormatString, lMandatory:=PMEMandatoryStatus.PMNonMandatory)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAccountDescription, lFieldType:=PMEDataType.PMString, _
                                                      lFormat:=PMEFormatStyle.PMFormatString, lMandatory:=PMEMandatoryStatus.PMNonMandatory)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtnextChequeNumber, lFieldType:=PMEDataType.PMString, _
                                                      lFormat:=PMEFormatStyle.PMFormatInteger, lMandatory:=PMEMandatoryStatus.PMNonMandatory)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=pnlReconciledDate, lFieldType:=PMEDataType.PMString, _
                                                      lFormat:=PMEFormatStyle.PMFormatDateMedium, lMandatory:=PMEMandatoryStatus.PMNonMandatory)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtStartChequeNumber, lFieldType:=PMEDataType.PMString, _
                                                      lFormat:=PMEFormatStyle.PMFormatString, lMandatory:=PMEMandatoryStatus.PMNonMandatory)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtFinancialInstitutionCode, lFieldType:=PMEDataType.PMString, _
                                                      lFormat:=PMEFormatStyle.PMFormatString, lMandatory:=PMEMandatoryStatus.PMNonMandatory)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDirectDebitSupplierName, lFieldType:=PMEDataType.PMString, _
                                                      lFormat:=PMEFormatStyle.PMFormatString, lMandatory:=PMEMandatoryStatus.PMNonMandatory)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDirectDebitSupplierID, lFieldType:=PMEDataType.PMString, _
                                                      lFormat:=PMEFormatStyle.PMFormatString, lMandatory:=PMEMandatoryStatus.PMNonMandatory)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtRemitter, lFieldType:=PMEDataType.PMString, _
                                                      lFormat:=PMEFormatStyle.PMFormatString, lMandatory:=PMEMandatoryStatus.PMNonMandatory)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtProcessingDays, lFieldType:=PMEDataType.PMString, _
                                                      lFormat:=PMEFormatStyle.PMFormatString, lMandatory:=PMEMandatoryStatus.PMNonMandatory)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtBIC, lFieldType:=PMEDataType.PMString, _
                                                      lFormat:=PMEFormatStyle.PMFormatString, lMandatory:=PMEMandatoryStatus.PMNonMandatory)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtIBAN, lFieldType:=PMEDataType.PMString, _
                                                      lFormat:=PMEFormatStyle.PMFormatString, lMandatory:=PMEMandatoryStatus.PMNonMandatory)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception
            result = PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the details from the business object.

            ' {* USER DEFINED CODE (Begin) *}


            m_lReturn = m_oBusiness.GetDetails(vBankAccountId:=m_lBankAccountId)

            ' {* USER DEFINED CODE (End) *}

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                Return result
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    '''  Updates all interface details from the business object.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function BusinessToInterface() As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim bExist As Boolean
        Try

            ' Update the interface details.

            ' Assign the details from the business object
            ' to the data storage.
            m_lReturn = BusinessToData()

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            ' Assign the details to the interface.
            If m_iBankAccountType = ACBankSuspenseAccount Then
                txtAccountNumber.Text = gPMFunctions.ToSafeString(m_sBankAccountNo).Trim()
                txtAccountCode.Text = gPMFunctions.ToSafeString(m_sCode).Trim()
            Else
                m_lReturn = m_oFormFields.FormatControl(txtAccountNumber, gPMFunctions.ToSafeString(m_sBankAccountNo))
                m_lReturn = m_oFormFields.FormatControl(txtAccountCode, gPMFunctions.ToSafeString(m_sCode))
                m_lDefaultBankAccount = 0
            End If

            m_lReturn = m_oFormFields.FormatControl(txtAccountName, gPMFunctions.ToSafeString(m_sBankAccountName))
            m_lReturn = m_oFormFields.FormatControl(txtAccountDescription, gPMFunctions.ToSafeString(m_sDescription))
            m_lReturn = m_oFormFields.FormatControl(txtnextChequeNumber, gPMFunctions.ToSafeLong(CStr(m_lNextChequeNumber)))

            pnlReconciledDate.Name = CDate(m_vReconciledDate).ToString("dd MMM yyyy")
            pnlAccountHolder.Tag = gPMFunctions.ToSafeString(m_lAccountId)
            pnlAccountHolder.Name = m_sAccountHolderName
            lblAccountHolder.Text = m_sAccountHolderName
            pnlDefaultBankAccount.Tag = CStr(gPMFunctions.ToSafeLong(CStr(m_lDefaultBankAccount)))
            Me.chkReceiveCashIntoThisCurr.CheckState = m_iIsCashReceiveInThisCurrencyOnly

            If gPMFunctions.ToSafeLong(CStr(m_lDefaultBankAccount)) > 0 Then
                m_lReturn = m_oBusiness.GetOtherDetails(vAccountHolderId:=m_lDefaultBankAccount, vAccountHolderName:=m_sDefaultBankAccount)
                pnlDefaultBankAccount.Name = m_sDefaultBankAccount
            End If

            If m_sStartChequeNumber = "" Then
                m_lReturn = m_oBusiness.IsChequeExistForBankAccount(m_lBankAccountId, bExist)
                If Not bExist Then
                    txtStartChequeNumber.Enabled = True
                    lblStartChequeNumber.Enabled = True
                Else
                    txtStartChequeNumber.Enabled = False
                    lblStartChequeNumber.Enabled = False
                End If
            Else
                txtStartChequeNumber.Enabled = False
                lblStartChequeNumber.Enabled = False
            End If

            m_lReturn = m_oFormFields.FormatControl(txtStartChequeNumber, gPMFunctions.ToSafeString(m_sStartChequeNumber))
            m_lReturn = m_oFormFields.FormatControl(txtFinancialInstitutionCode, gPMFunctions.ToSafeString(m_sFinancialInstitutionCode))
            m_lReturn = m_oFormFields.FormatControl(txtDirectDebitSupplierName, gPMFunctions.ToSafeString(m_sDirectDebitSupplierName))
            m_lReturn = m_oFormFields.FormatControl(txtDirectDebitSupplierID, gPMFunctions.ToSafeString(m_lDirectDebitSupplierID))
            m_lReturn = m_oFormFields.FormatControl(txtRemitter, gPMFunctions.ToSafeString(m_sRemitter))
            m_lReturn = m_oFormFields.FormatControl(txtProcessingDays, gPMFunctions.ToSafeString(m_iProcessingDays))
            m_lReturn = m_oFormFields.FormatControl(txtBIC, gPMFunctions.ToSafeString(m_sBIC))
            m_lReturn = m_oFormFields.FormatControl(txtIBAN, gPMFunctions.ToSafeString(m_sIBAN))

            Return nResult
        Catch excep As System.Exception
            nResult = PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
        End Try
    End Function

    ''' <summary>
    ''' Updates all business members from the interface details.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function InterfaceToBusiness() As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim nBusinessDataID As Integer

        Try

            ' Update the business object.

            ' Assign the details from the interface to the data storage.
            m_lReturn = InterfaceToData()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the business data ID to one because we are only
            ' dealing with one record item only.
            nBusinessDataID = 1
            If String.IsNullOrEmpty(m_sUniqueId) Then
                m_sUniqueId = GetUniqueID()
            End If
            m_sScreenHierarchy = $"Accounts({m_sBankAccountNo.Trim()})"
            Select Case (m_iTask)
                Case gPMConstants.PMEComponentAction.PMAdd
                    m_lReturn = m_oBusiness.EditAdd(lRow:=nBusinessDataID,
                                                    vBankAccountId:=m_lBankAccountId,
                                                    vCurrencyId:=m_iCurrencyId,
                                                    vCompanyId:=g_iSourceID,
                                                    vAccountId:=m_lAccountId,
                                                    vBankId:=m_iBankId,
                                                    vCode:=m_sCode,
                                                    vBankAccountNo:=m_sBankAccountNo,
                                                    vBankAccountName:=m_sBankAccountName,
                                                    vDescription:=m_sDescription,
                                                    vNextChequeNumber:=m_lNextChequeNumber,
                                                    vReconciledDate:=m_vReconciledDate,
                                                    vDefaultBankAccountID:=m_lDefaultBankAccount,
                                                    v_iIsCashReceiveInThisCurrencyOnly:=m_iIsCashReceiveInThisCurrencyOnly,
                                                    vStartChequeNumber:=m_sStartChequeNumber,
                                                    vFinancialInstitutionCode:=m_sFinancialInstitutionCode,
                                                    vDirectDebitSupplierName:=m_sDirectDebitSupplierName,
                                                    vDirectDebitSupplierID:=m_lDirectDebitSupplierID,
                                                    vRemitter:=m_sRemitter,
                                                    vProcessingDays:=m_iProcessingDays,
                                                    r_sBIC:=m_sBIC,
                                                    r_sIBAN:=m_sIBAN,
                                                    vUniqueId:=m_sUniqueId, vScreenHierarchy:=m_sScreenHierarchy)
                Case gPMConstants.PMEComponentAction.PMEdit
                    m_lReturn = m_oBusiness.EditUpdate(lRow:=nBusinessDataID,
                                                       vBankAccountId:=m_lBankAccountId,
                                                       vCurrencyId:=m_iCurrencyId,
                                                       vCompanyId:=g_iSourceID,
                                                       vAccountId:=m_lAccountId,
                                                       vBankId:=m_iBankId,
                                                       vCode:=m_sCode,
                                                       vBankAccountNo:=m_sBankAccountNo,
                                                       vBankAccountName:=m_sBankAccountName,
                                                       vDescription:=m_sDescription,
                                                       vNextChequeNumber:=m_lNextChequeNumber,
                                                       vReconciledDate:=m_vReconciledDate,
                                                       vDefaultBankAccountID:=m_lDefaultBankAccount,
                                                       v_iIsCashReceiveInThisCurrencyOnly:=m_iIsCashReceiveInThisCurrencyOnly,
                                                       vStartChequeNumber:=m_sStartChequeNumber,
                                                       vFinancialInstitutionCode:=m_sFinancialInstitutionCode,
                                                       vDirectDebitSupplierName:=m_sDirectDebitSupplierName,
                                                       vDirectDebitSupplierID:=m_lDirectDebitSupplierID,
                                                       vRemitter:=m_sRemitter,
                                                       vProcessingDays:=m_iProcessingDays,
                                                       r_sBIC:=m_sBIC,
                                                       r_sIBAN:=m_sIBAN,
                                                       vUniqueId:=m_sUniqueId, vScreenHierarchy:=m_sScreenHierarchy)
                Case gPMConstants.PMEComponentAction.PMDelete
                    Dim sScreenHierarchy As String = $"Bank({m_sBankCode})\Accounts({m_sBankAccountNo.Trim()})"
                    m_lReturn = m_oBusiness.EditDelete(lRow:=nBusinessDataID, sUniqueId:=m_sUniqueId, sScreenHierarchy:=sScreenHierarchy)
            End Select

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, _
                                   sMsg:="Failed to assign the interface details to business object", _
                                   vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
            End If

            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, _
                               sMsg:="Failed to update the business object", _
                               vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", _
                               vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayLookupDetails
    '
    ' Description: Displays all of the lookup details using the lookup
    '              values/details.
    '
    ' ***************************************************************** '
    Public Function DisplayLookupDetails() As Integer
        Dim result As Integer = 0
        Dim oBusiness As bSIRInsuranceFile.Business

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.

            m_lReturn = GetLookupValues()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get all of the lookup details.

            ' {* USER DEFINED CODE (Begin) *}
            m_lReturn = GetLookupDetails(sLookupTable:=gACTLibrary.ACTLookupCurrency, ctlLookup:=cboCurrency)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' CTAF 20040615 - START - Default to GBP
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then

                Dim temp_oBusiness As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oBusiness, "bSIRInsuranceFile.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oBusiness = temp_oBusiness

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = oBusiness.GetBranchBaseCurrency(vSourceID:=g_iSourceID, iCurrencyID:=m_iCurrencyId)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Branch Base Currency", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If

                For iLoop1 As Integer = 0 To cboCurrency.Items.Count - 1
                    'If LCase(cboCurrency.List(iLoop1)) = ACPoundsSterling Then
                    If VB6.GetItemData(cboCurrency, iLoop1) = m_iCurrencyId Then
                        cboCurrency.SelectedIndex = iLoop1
                        Exit For
                    End If
                Next iLoop1


                'cboCurrency.CurrencyID =
                oBusiness = Nothing

            End If

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: BusinessToData
    '
    ' Description: Updates the data storage from the business object.
    '
    ' ***************************************************************** '
    Private Function BusinessToData() As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Try

            m_lReturn = m_oBusiness.GetNext(vBankAccountId:=m_lBankAccountId, _
                                            vCurrencyId:=m_iCurrencyId, _
                                            vCompanyId:=g_iSourceID, _
                                            vAccountId:=m_lAccountId, _
                                            vBankId:=m_iBankId, _
                                            vCode:=m_sCode, _
                                            vBankAccountNo:=m_sBankAccountNo, _
                                            vBankAccountName:=m_sBankAccountName, _
                                            vDescription:=m_sDescription, _
                                            vNextChequeNumber:=m_lNextChequeNumber, _
                                            vReconciledDate:=m_vReconciledDate, _
                                            vDefaultBankAccountID:=m_lDefaultBankAccount, _
                                            v_iIsCashReceiveInThisCurrencyOnly:=m_iIsCashReceiveInThisCurrencyOnly, _
                                            vStartChequeNumber:=m_sStartChequeNumber, _
                                            vFinancialInstitutionCode:=m_sFinancialInstitutionCode, _
                                            vDirectDebitSupplierName:=m_sDirectDebitSupplierName, _
                                            vDirectDebitSupplierID:=m_lDirectDebitSupplierID, _
                                            vRemitter:=m_sRemitter, _
                                            vProcessingDays:=m_iProcessingDays, _
                                            r_sBIC:=m_sBIC, _
                                            r_sIBAN:=m_sIBAN)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retreive the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            m_lReturn = m_oBusiness.GetOtherDetails(vAccountHolderId:=m_lAccountId, vAccountHolderName:=m_sAccountHolderName)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retreive the Account details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If
            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
        End Try
    End Function

    ''' <summary>
    ''' InterfaceToData
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function InterfaceToData() As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Try

            If BankAccountType = ACBankSuspenseAccount Then
                m_lDefaultBankAccount = gPMFunctions.ToSafeLong(Convert.ToString(pnlDefaultBankAccount.Tag))
                m_sBankAccountNo = gPMFunctions.ToSafeString(txtAccountNumber).Trim()
                m_sCode = gPMFunctions.ToSafeString(txtAccountCode).Trim()
            Else

                m_sBankAccountNo = CStr(m_oFormFields.UnformatControl(txtAccountNumber)).Trim()

                m_sCode = CStr(m_oFormFields.UnformatControl(txtAccountCode)).Trim()
                m_lDefaultBankAccount = 0
            End If

            m_iCurrencyId = VB6.GetItemData(cboCurrency, cboCurrency.SelectedIndex)
            m_sBankAccountName = CStr(m_oFormFields.UnformatControl(txtAccountName)).Trim()
            m_sDescription = CStr(m_oFormFields.UnformatControl(txtAccountDescription)).Trim()
            m_lAccountId = CInt(Convert.ToString(pnlAccountHolder.Tag))
            m_iIsCashReceiveInThisCurrencyOnly = Me.chkReceiveCashIntoThisCurr.CheckState

            If m_sBankAccountName = "" Then
                m_sBankAccountName = pnlAccountHolder.Name.Trim()
            End If

            m_lNextChequeNumber = CInt(CStr(m_oFormFields.UnformatControl(txtnextChequeNumber)).Trim())
            m_sStartChequeNumber = gPMFunctions.ToSafeString(m_oFormFields.UnformatControl(txtStartChequeNumber))
            m_sFinancialInstitutionCode = gPMFunctions.ToSafeString(m_oFormFields.UnformatControl(txtFinancialInstitutionCode))
            m_sDirectDebitSupplierName = gPMFunctions.ToSafeString(m_oFormFields.UnformatControl(txtDirectDebitSupplierName))

            m_lDirectDebitSupplierID = gPMFunctions.ToSafeLong(CStr(m_oFormFields.UnformatControl(txtDirectDebitSupplierID)))
            m_sRemitter = gPMFunctions.ToSafeString(m_oFormFields.UnformatControl(txtRemitter))

            m_iProcessingDays = gPMFunctions.ToSafeLong(CStr(m_oFormFields.UnformatControl(txtProcessingDays)))
            m_sBIC = gPMFunctions.ToSafeString(m_oFormFields.UnformatControl(txtBIC))
            m_sIBAN = gPMFunctions.ToSafeString(m_oFormFields.UnformatControl(txtIBAN))

            Return nResult

        Catch excep As System.Exception
            nResult = PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the status of the Navigate button.
            Select Case (m_lNavigate)
                Case gPMConstants.PMENavigateButtonStatus.PMNavigateEnabled
                    cmdNavigate.Visible = True
                    cmdNavigate.Enabled = True

                Case gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled
                    cmdNavigate.Visible = True
                    cmdNavigate.Enabled = False

                Case Else
                    cmdNavigate.Visible = False
            End Select
            m_lReturn = SetFirstLastControls()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DC281103 PN8662 not to be shown yet
            'Select Case m_iTask
            'Case PMAdd
            SSTabHelper.SetTabVisible(tabMainTab, 1, False)
            'Case PMEdit
            '    tabMainTab.TabVisible(1) = True
            'End Select
            If Not lblnextChequeNumber.Visible Then
                lblDefaultBankAccount.Top = lblnextChequeNumber.Top
                lblDefaultBankAccount.Left = lblnextChequeNumber.Left
                pnlDefaultBankAccount.Top = txtnextChequeNumber.Top
                pnlDefaultBankAccount.Left = pnlDefaultBankAccount.Left
                cmdFindAccount.Top = pnlDefaultBankAccount.Top
                cmdFindAccount.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(pnlDefaultBankAccount.Left) + VB6.PixelsToTwipsX(pnlDefaultBankAccount.Width) + 10)
            ElseIf Not lblReconciledDate.Visible Then
                lblDefaultBankAccount.Top = lblReconciledDate.Top
                lblDefaultBankAccount.Left = lblReconciledDate.Left
                pnlDefaultBankAccount.Top = pnlReconciledDate.Top
                pnlDefaultBankAccount.Left = pnlReconciledDate.Left
                cmdFindAccount.Top = pnlDefaultBankAccount.Top
                cmdFindAccount.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(pnlDefaultBankAccount.Left) + VB6.PixelsToTwipsX(pnlDefaultBankAccount.Width) + 10)
            End If


            'DJM 30/01/2004 : Only need to OK and Cancel when deleting
            If m_iTask = gPMConstants.PMEComponentAction.PMDelete Then
                cmdApply.Enabled = False
                VB6.SetDefault(cmdCancel, True)
            End If

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetFirstLastControls
    '
    ' Description: Sets the first and last data entry controls for
    '              each tab to the control array, for use with the
    '              keyboard navigation.
    '
    ' ***************************************************************** '
    Private Function SetFirstLastControls() As Integer

        Dim result As Integer = 0
        Try


            ' Initialise the control array with the number of
            ' tabs which contain data entry fields on (Remember
            ' that arrays start from zero, therefore you must
            ' subtract one from the number of tabs).
            ' ReDim m_ctlTabFirstLast(1, )

            ' Set the first and last data entry controls for
            ' all of the tabs.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to set the first and last data entry
            ' controls for all of the tabs.
            '
            ' Example:-
            '
            '    Set m_ctlTabFirstLast(ACControlStart, 0) = txtName
            '    Set m_ctlTabFirstLast(ACControlEnd, 0) = txtAge
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFirstLastControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayCaptions
    '
    ' Description: Display all language specific captions.
    '
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.


            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() & _
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If


            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblAccountNumber.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblAccountCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLabTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblCurrency.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLabTitle3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblAccountName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLabTitle4, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblAccountDescription.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLabTitle5, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdAccountHolder.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAccountHolderButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdNavigate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNavigateButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblnextChequeNumber.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNextChequeNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblReconciledDate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACReconciledDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to display all language specific
            ' captions.
            ' The GetResData function will allow you to do this.
            '
            ' Example:-
            '
            '    lblDesc.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACDesc, _
            ''        iDataType:=PMResString)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupValues
    '
    ' Description: Gets all of the lookup values, ready to be used by
    '              the lookup function.
    '
    ' ***************************************************************** '
    Private Function GetLookupValues() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Gets all of the lookup values.

            ' Check the task.
            Select Case (m_iTask)
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Get all of the lookup values.

                    m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)

                Case gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMDelete
                    ' Get all of the lookup values with the correct
                    ' effective date.

                    m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)

                Case gPMConstants.PMEComponentAction.PMView
                    ' Get lookup values for viewing only.

                    m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupSingle, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
            End Select

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")

                Return result
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupDetails
    '
    ' Description: Gets all of the lookup details using the lookup
    '              values, then assigns them to the control passed.
    '
    ' ***************************************************************** '

    'developer guide no. 30
    Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As ComboBox) As Integer
        Dim result As Integer = 0
        Dim lRow As Integer
        Dim bFoundMatch As Boolean

        ' Lookup value contants.
        Const ACValueTableName As Integer = 0
        Const ACValueID As Integer = 1
        Const ACValueStartPos As Integer = 2
        Const ACValueNumber As Integer = 3

        ' Lookup detail contants.
        Const ACDetailKey As Integer = 0
        Const ACDetailDesc As Integer = 1

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.

            bFoundMatch = False

            For lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
                ' Check for a match of the table name.
                If CStr(m_vLookupValues(ACValueTableName, lRow)).Trim() = sLookupTable.Trim() Then
                    ' Found a match
                    bFoundMatch = True
                    Exit For
                End If
            Next lRow

            ' Check if there has been a table match.
            If Not bFoundMatch Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for the table, " & sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails")

                Return result
            End If

            ' Using the lookup values, populate the control with
            ' the details from the lookup details array.

            For lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)

                Dim NewIndex As Integer = ctlLookup.Items.Add(New VB6.ListBoxItem(Trim(m_vLookupDetails(ACDetailDesc, lCntr)), m_vLookupDetails(ACDetailKey, lCntr)))

                'ReflectionHelper.SetMember(ctlLookup, "ItemData", New Object() {ReflectionHelper.GetMember(ctlLookup, "NewIndex")}, CInt(m_vLookupDetails(ACDetailKey, lCntr)))

                'SP150998 - compare long value not string
                ' Check if this is the selected index.
                If CStr(m_vLookupValues(ACValueID, lRow)) <> "" Then
                    If CDbl(m_vLookupValues(ACValueID, lRow)) = CInt(m_vLookupDetails(ACDetailKey, lCntr)) Then


                        'developer guide no. 30
                        ctlLookup.SelectedIndex = NewIndex

                    End If
                End If

            Next lCntr

            ' Check if the selected index is blank. If so,
            ' we set the controls index to zero.
            If CStr(m_vLookupValues(ACValueID, lRow)) = "" Then

                'developer guide no. 30
                ctlLookup.SelectedIndex = 0

            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Private Function ValidateOK() As gPMConstants.PMEReturnCode
        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse


        result = gPMConstants.PMEReturnCode.PMTrue

        'developer guide no. 51 (guide)
        If txtAccountCode.Text.Trim() = "" Then
            MessageBox.Show("Please Enter an AccountCode", "Bank Account", MessageBoxButtons.OK, MessageBoxIcon.Information)
            result = gPMConstants.PMEReturnCode.PMFalse
        End If
        If txtAccountNumber.Text.Trim() = "" Then
            MessageBox.Show("Please Enter an Account Number", "Bank Account", MessageBoxButtons.OK, MessageBoxIcon.Information)
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        If (Convert.ToString(pnlAccountHolder.Tag)) = "" Then
            MessageBox.Show("Please Enter an Account Holder", "Bank Account", MessageBoxButtons.OK, MessageBoxIcon.Information)
            result = gPMConstants.PMEReturnCode.PMFalse
            'PN10697 eck 020304
        Else

            m_lReturn = m_oBusiness.CheckAccount(vBankAccountId:=m_lBankAccountId, vAccountId:=CInt(Convert.ToString(pnlAccountHolder.Tag)))
            If m_lReturn = gPMConstants.PMEReturnCode.PMError Then
                MessageBox.Show("Account Holder already linked to Bank Account", "Bank Account", MessageBoxButtons.OK, MessageBoxIcon.Information)
                result = gPMConstants.PMEReturnCode.PMFalse
            End If
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GoTo Err_ValidateOK
            End If

            'PN10697End
        End If

        Dim dbNumericTemp As Double
        If Not Double.TryParse(txtStartChequeNumber.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) And txtStartChequeNumber.Text <> "" Then
            MessageBox.Show("Number specified is not valid. Only numeric values up to 10 digits are allowed", "Bank Account", MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtStartChequeNumber.Focus()
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        'PN4552-start
        If txtStartChequeNumber.Text <> "" AndAlso txtStartChequeNumber.Text = 0 Then
            MessageBox.Show("Zero is not allowed as the default cheque number.", "Bank Account", MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtStartChequeNumber.Focus()
            result = gPMConstants.PMEReturnCode.PMFalse
        End If
        'PN4552-End

        ''PN58680 (Saurabh Agrawal)
        Dim dbNumericTemp2 As Double
        If Not Double.TryParse(txtDirectDebitSupplierID.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) And txtDirectDebitSupplierID.Text <> "" Then
            MessageBox.Show("Number specified is not valid. Only numeric values are allowed", "Bank Account", MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtDirectDebitSupplierID.Focus()
            result = gPMConstants.PMEReturnCode.PMFalse
        End If
        ''PN58680 (Saurabh Agrawal)
        Dim dbNumericTemp3 As Double
        If Not Double.TryParse(txtProcessingDays.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) And txtProcessingDays.Text <> "" Then
            MessageBox.Show("Number specified is not valid. Only numeric values are allowed", "Bank Account", MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtProcessingDays.Focus()
            result = gPMConstants.PMEReturnCode.PMFalse
        End If
        Dim iChequeNo As Integer
        If txtStartChequeNumber.Text <> "" And txtStartChequeNumber.Enabled = True Then
            iChequeNo = CInt(txtStartChequeNumber.Text)
            If iChequeNo = 0 Then
                MessageBox.Show("Zero is not allowed as the default cheque number.", "Bank Account", MessageBoxButtons.OK, MessageBoxIcon.Information)
                txtStartChequeNumber.Focus()
                result = gPMConstants.PMEReturnCode.PMFalse
            End If
        End If
        Return result
        'PN10697
Err_ValidateOK:

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error reading Bank Account details", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateOK", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result
        'PN10697End


    End Function
    ' PRIVATE Methods (End)


    ' ***************************************************************** '
    ' Name: GetAndDisplayRules
    '
    ' Description: gets and displays the bank account rules
    '
    ' ***************************************************************** '
    Public Function GetAndDisplayRules() As Integer
        Dim result As Integer = 0
        Dim lRowCount As Integer
        Dim sType As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'if this function has been called then the apply button is not needed
            'as the bank account has already been created
            cmdApply.Enabled = False


            m_lReturn = m_oBusiness.GetBankAccountRules(m_lBankAccountId, m_vRulesData)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'clear the list
            lvwRules.Items.Clear()

            If Information.IsArray(m_vRulesData) Then
                lRowCount = m_vRulesData.GetUpperBound(1)

                For lRow As Integer = 0 To lRowCount
                    lvwRules.Items.Add(CStr(m_vRulesData(ACMediaDescription, lRow)))
                    Select Case Conversion.Val(CStr(m_vRulesData(ACMatchToTransdetail, lRow)))
                        Case 0
                            sType = "Receipt/Payments"
                        Case 1
                            sType = "Transaction"
                        Case Else
                            sType = "Instalments"
                    End Select
                    ListViewHelper.GetListViewSubItem(lvwRules.Items.Item(lRow), 1).Text = sType
                Next

            End If

            Select Case m_iTask
                Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit
                    If lvwRules.Items.Count > 0 Then
                        cmdAddRule.Enabled = True
                        cmdEditRule.Enabled = True
                        cmdDeleteRule.Enabled = True
                        cmdViewRule.Enabled = True
                    Else
                        cmdAddRule.Enabled = True
                        cmdEditRule.Enabled = False
                        cmdDeleteRule.Enabled = False
                        cmdViewRule.Enabled = False
                    End If
                Case gPMConstants.PMEComponentAction.PMDelete

            End Select

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get and display the bank account rules", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAndDisplayRules", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Private Sub cmdAccountHolder_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAccountHolder.Click
        Const kFindAccount As String = "cmdAccountHolder_Click"

        Dim lAccountId As Integer
        Dim sAccountName As String = ""

        Try



            m_lReturn = FindAccount(r_lAccountId:=lAccountId, r_sAccountName:=sAccountName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMCancel Then
                gPMFunctions.RaiseError(kFindAccount, "cmdAccountHolder_Click Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            pnlAccountHolder.Tag = CStr(lAccountId)

            'developer guide no. 51 (guide)
            pnlAccountHolder.Name = sAccountName
            lblAccountHolder.Text = sAccountName
            txtAccountName.Text = sAccountName



        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAccountHolderLookUp_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally


        End Try
        Exit Sub
    End Sub


    'UPGRADE_NOTE: (7001) The following declaration (cmdAddAcc_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub cmdAddAcc_Click()
    '
    'End Sub

    Private Sub cmdAddDelay_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddDelay.Click
        cmdApply_Click(cmdApply, New EventArgs())
        If m_bApply Then
            ProcessBankAccountDelay(gPMConstants.PMEComponentAction.PMAdd)
        End If
    End Sub

    Private Sub cmdAddRule_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddRule.Click
        Dim frmRule As frmBankAccountRuleEdit

        Try

            frmRule = New frmBankAccountRuleEdit()

            frmRule.Task = gPMConstants.PMEComponentAction.PMAdd
            frmRule.BankAccountName = m_sBankAccountName
            frmRule.BankAccountNo = m_sBankAccountNo
            frmRule.BankAccountId = CStr(m_lBankAccountId)
            frmRule.BankAccountRuleID = 0


            'developer guide no. 24
            frmRule.Business = m_oBusiness
            frmRule.ProcessComplete = False

            frmRule.SetCaptions()

            frmRule.ShowDialog()

            If frmRule.ProcessComplete Then
                GetAndDisplayRules()
            End If

            frmRule.Close()
            frmRule = Nothing

        Catch excep As System.Exception



            frmRule.Close()
            frmRule = Nothing

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process cmdAddRuleClick", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddRule_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try

    End Sub

    Private Sub cmdApply_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdApply.Click

        ' Click event of the Apply button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Check mandatory controls have been entered into.
            m_lReturn = m_oFormFields.CheckMandatoryControls()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If
            'Validate some  stuff double check
            m_lReturn = ValidateOK()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'PN: 41586
            If Not ValidateMediaDetails() Then
                Exit Sub
            End If

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand()

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                'DC281103 PN8662 not to be visible yet
                'Me.tabMainTab.TabVisible(1) = True
                SSTabHelper.SetTabVisible(Me.tabMainTab, 1, False)
                cmdEditRule.Enabled = False
                cmdDeleteRule.Enabled = False
                cmdViewRule.Enabled = False
                cmdAddRule.Enabled = True
                cmdApply.Enabled = False
                cmdCancel.Enabled = False
                'PN: 47315
                'txtStartChequeNumber.Enabled = False
                'lblStartChequeNumber.Enabled = False
                m_bApply = True
            Else
                SSTabHelper.SetTabVisible(Me.tabMainTab, 1, False)
            End If


            If m_lBankAccountId <> 0 Then

                'Save the pick lists
                'Put in the picklist PK values
                SSTabHelper.SetTabEnabled(Me.tabMainTab, 3, True)


                uctPickListBranches.ForeignKeys.Item("BankAccountID").Value = m_lBankAccountId
                uctPickListBranches.ForeignKeys.Item("user_id").Value = Nothing
                uctPickListBranches.ForeignKeys.Item("unique_id").Value = m_sUniqueId
                uctPickListBranches.ForeignKeys.Item("screen_hierarchy").Value = m_sScreenHierarchy

                m_lReturn = uctPickListBranches.Save()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If
            End If

            'sw now change the task to PMEdit

            m_iTask = gPMConstants.PMEComponentAction.PMEdit

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Apply command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdApply_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdDeleteDelay_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteDelay.Click
        If lvwBankAccountDelay.Items.Count <= 0 Then Exit Sub
        ProcessBankAccountDelay(gPMConstants.PMEComponentAction.PMDelete)
    End Sub

    Private Sub cmdDeleteRule_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteRule.Click
        Dim lRow As Integer

        Try

            If lvwRules.FocusedItem Is Nothing Then
                MessageBox.Show("Select a rule before proceeding", "Invalid Delete", MessageBoxButtons.OK)
                Exit Sub
            End If

            lRow = Me.lvwRules.FocusedItem.Index + 1 - 1


            m_lReturn = m_oBusiness.DeleteBankAccountRule(CInt(m_vRulesData(ACBankAccountRulesID, lRow)))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Unable to delete the selected rule", "Invalid Delete", MessageBoxButtons.OK)
                Exit Sub
            End If

            GetAndDisplayRules()

        Catch excep As System.Exception



            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process cmdDeleteRule_Click Event", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDeleteRule_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdEditDelay_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEditDelay.Click
        If lvwBankAccountDelay.Items.Count <= 0 Then Exit Sub
        ProcessBankAccountDelay(gPMConstants.PMEComponentAction.PMEdit)
    End Sub

    Private Sub cmdEditRule_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEditRule.Click
        Dim frmRule As frmBankAccountRuleEdit
        Dim lRow As Integer

        Try

            lRow = lvwRules.FocusedItem.Index + 1 - 1

            If Me.lvwRules.FocusedItem Is Nothing Then
                MessageBox.Show("Please select an item off the list before proceeding", "Edit " & _
                                "Bank Account Rule", MessageBoxButtons.OK)
                Exit Sub
            End If

            frmRule = New frmBankAccountRuleEdit()

            frmRule.Task = gPMConstants.PMEComponentAction.PMEdit
            frmRule.BankAccountName = m_sBankAccountName
            frmRule.BankAccountNo = m_sBankAccountNo
            frmRule.BankAccountId = CStr(m_lBankAccountId)
            frmRule.BankAccountRuleID = CInt(m_vRulesData(ACBankAccountRulesID, lRow))


            'developer guide no. 24
            frmRule.Business = m_oBusiness
            frmRule.ProcessComplete = False

            m_lReturn = frmRule.DisplayRule(CInt(m_vRulesData(ACMediaTypeID, lRow)), CInt(m_vRulesData(ACMatchToTransdetail, lRow)), CInt(m_vRulesData(ACMatchAccountCode, lRow)), CInt(m_vRulesData(ACCodeIsMerchantNumber, lRow)), CInt(m_vRulesData(ACMatchBatchNumber, lRow)), CInt(m_vRulesData(ACBatchIsRemitCode, lRow)), CInt(m_vRulesData(ACMatchChequeNumber, lRow)), CInt(m_vRulesData(ACMatchAmount, lRow)), CInt(m_vRulesData(ACMatchDate, lRow)), CInt(m_vRulesData(ACSkipIfReasonNull, lRow)))

            frmRule.SetCaptions()

            frmRule.ShowDialog()

            If frmRule.ProcessComplete Then
                GetAndDisplayRules()
            End If

            frmRule.Close()
            frmRule = Nothing

        Catch excep As System.Exception



            frmRule.Close()
            frmRule = Nothing

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process cmdEditRule_Click Event", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditRule_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdViewRule_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdViewRule.Click
        Dim frmRule As frmBankAccountRuleEdit
        Dim lRow As Integer

        Try

            lRow = lvwRules.FocusedItem.Index + 1 - 1

            If Me.lvwRules.FocusedItem Is Nothing Then
                MessageBox.Show("Please select an item off the list before proceeding", "Edit " & _
                                "Bank Account Rule", MessageBoxButtons.OK)
                Exit Sub
            End If

            frmRule = New frmBankAccountRuleEdit()

            frmRule.Task = gPMConstants.PMEComponentAction.PMView
            frmRule.BankAccountName = m_sBankAccountName
            frmRule.BankAccountNo = m_sBankAccountNo
            frmRule.BankAccountId = CStr(m_lBankAccountId)
            frmRule.BankAccountRuleID = CInt(m_vRulesData(ACBankAccountRulesID, lRow))


            'developer guide no. 24
            frmRule.Business = m_oBusiness
            frmRule.ProcessComplete = False

            m_lReturn = frmRule.DisplayRule(CInt(m_vRulesData(ACMediaTypeID, lRow)), CInt(m_vRulesData(ACMatchToTransdetail, lRow)), CInt(m_vRulesData(ACMatchAccountCode, lRow)), CInt(m_vRulesData(ACCodeIsMerchantNumber, lRow)), CInt(m_vRulesData(ACMatchBatchNumber, lRow)), CInt(m_vRulesData(ACBatchIsRemitCode, lRow)), CInt(m_vRulesData(ACMatchChequeNumber, lRow)), CInt(m_vRulesData(ACMatchAmount, lRow)), CInt(m_vRulesData(ACMatchDate, lRow)), CInt(m_vRulesData(ACSkipIfReasonNull, lRow)))

            frmRule.SetCaptions()

            frmRule.ShowDialog()

            frmRule.Close()
            frmRule = Nothing

        Catch excep As System.Exception



            frmRule.Close()
            frmRule = Nothing

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process cmdViewRuleClick", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdViewRule_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try

    End Sub

    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()

        Dim sMessage, sTitle As String

        ' Forms initialise event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bACTBankAccount.Form", vInstanceManager:="ClientManager")
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub
            End If

            ' Create an instance of the general interface object.
            m_oGeneral = New iACTBankAccount.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Create an instance of the form control object.
            m_oFormFields = New iPMFormControl.FormFields()

            ' Set language
            m_oFormFields.LanguageID = g_iLanguageID

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception




            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Forms load event.
        Dim Key As uctPickList.PickListKey
        Dim iTabNum As Integer

        Try

            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the process modes for the busines object.

            m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")

                Exit Sub
            End If

            ' Set the business keys.
            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}

            ' Validate fields using Forms Control
            m_lReturn = SetFieldValidation()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Gets the interface details to be displayed.
            m_lReturn = m_oGeneral.GetInterfaceDetails()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            GetBankAccountDelay()

            'Pankaj

            Key = New uctPickList.PickListKey()
            Key.KeyName = "BankAccountID"
            Key.ValueType = gPMConstants.PMEDataType.PMLong
            uctPickListBranches.ForeignKeys.Add(Key, Key:="BankAccountID")

            uctPickListBranches.ForeignKeys.Item("BankAccountID").Value = m_lBankAccountId

            Key = New uctPickList.PickListKey()
            Key.KeyName = "user_id"
            Key.ValueType = gPMConstants.PMEDataType.PMLong
            uctPickListBranches.ForeignKeys.Add(Key, Key:="user_id")

            Key = New uctPickList.PickListKey()
            Key.KeyName = "unique_id"
            Key.ValueType = gPMConstants.PMEDataType.PMString
            uctPickListBranches.ForeignKeys.Add(Key, Key:="unique_id")

            Key = New uctPickList.PickListKey()
            Key.KeyName = "screen_hierarchy"
            Key.ValueType = gPMConstants.PMEDataType.PMString
            uctPickListBranches.ForeignKeys.Add(Key, Key:="screen_hierarchy")

            'developer guide no. 68 (guide)
            m_lReturn = uctPickListBranches.Load_Renamed()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to load list of Branches", "Bank Account", MessageBoxButtons.OK, MessageBoxIcon.Information)
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            iTabNum = 1
            For iTabCount As Integer = 0 To SSTabHelper.GetTabCount(tabMainTab) - 1
                If SSTabHelper.GetTabVisible(tabMainTab, iTabCount) Then
                    SSTabHelper.SetTabCaption(tabMainTab, iTabCount, "" & iTabNum & Mid(SSTabHelper.GetTabCaption(tabMainTab, iTabCount), 3))
                    iTabNum += 1
                End If
            Next iTabCount

            If m_lBankAccountId = 0 Then
                SSTabHelper.SetTabEnabled(Me.tabMainTab, 3, False)
            End If

            'Disable start cheque number when editing bank account
            '    If m_iTask = PMEdit Then
            '        txtStartChequeNumber.Enabled = False
            '        lblStartChequeNumber.Enabled = False
            '    End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.



            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = m_oGeneral.ProcessCommand()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    'developer guide no. 7
                    eventArgs.Cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
                ' Set the interface status.
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
            End If

            ' Terminate the general object.
            m_oGeneral.Dispose()

            

            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing

            ' Terminate the business object

            m_oBusiness.Dispose()

            

            ' Destroy the instance of the business object
            ' from memory.
            m_oBusiness = Nothing

            ' Terminate the form control object.
            m_oFormFields.Dispose()

           

            ' Destroy the instance of the form control object
            ' from memory.
            m_oFormFields = Nothing

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception




            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    Private Sub frmInterface_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        Dim iCtrlDown As Integer

        Const ACCtrlMask As Integer = 2


        Try

            ' Set the control key value.
            iCtrlDown = (Shift And ACCtrlMask) > 0

            With tabMainTab
                ' Check the key pressed.
                Select Case KeyCode
                    Case Keys.PageUp
                        ' Page Up key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the first tab.
                            SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                        Else
                            ' Check we are not on the
                            ' first tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) > 0 Then
                                ' Display the previous tab.
                                SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) - 1)
                            End If
                        End If

                    Case Keys.PageDown
                        ' Page Down key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the last tab.
                            SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetTabCount(tabMainTab) - 1)
                        Else
                            ' Check we are not on the
                            ' last tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) < (SSTabHelper.GetTabCount(tabMainTab) - 1) Then
                                ' Display the next tab.
                                SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) + 1)
                            End If
                        End If

                    Case Keys.Home
                        ' Home key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                                m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                            End If
                        End If

                    Case Keys.End
                        ' End key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                                m_ctlTabFirstLast(ACControlEnd, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                            End If
                        End If
                End Select
            End With
            'Developer Guide No 293
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                tabMainTab.SelectedIndex = 0
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D2 Then
                'tabMainTab.SelectedIndex = 2
                tabMainTab.SelectedIndex = 1
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D3 Then
                'tabMainTab.SelectedIndex = 1
                tabMainTab.SelectedIndex = 2
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D4 Then
                tabMainTab.SelectedIndex = 3
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D5 Then
                tabMainTab.SelectedIndex = 4
            End If

            'RemoveHandler cmdAccountHolder.Click, AddressOf cmdAccountHolder_Click
            'RemoveHandler cmdApply.Click, AddressOf cmdApply_Click
            'If eventArgs.Alt And eventArgs.KeyCode = Keys.A Then
            '    If Not cmdAccountHolder.Focused = True Then
            '        cmdAccountHolder.Select()
            '        cmdAccountHolder.Focus()

            '    Else
            '        cmdApply.Select()
            '        cmdApply.Focus()
            '    End If
            '    RemoveHandler cmdAccountHolder.Click, AddressOf cmdAccountHolder_Click
            '    RemoveHandler cmdApply.Click, AddressOf cmdApply_Click
            'End If




        Catch




            Exit Sub
        End Try


    End Sub

    Private Sub lvwBankAccountDelay_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwBankAccountDelay.DoubleClick
        cmdEditDelay_Click(cmdEditDelay, New EventArgs())
    End Sub

    Private Sub lvwRules_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwRules.DoubleClick
        cmdEditRule_Click(cmdEditRule, New EventArgs())
    End Sub

    Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged

        Try

            With tabMainTab
                ' Set the default button.
                VB6.SetDefault(cmdOK, True)

                ' Now I know this is crap, this goes against
                ' all my principles, but for some reason when
                ' using the mouse to select a tab the setfocus
                ' code below doesn't work. The cursor sticks,
                ' and you can't tab off. Therefore I've used
                ' this to get around the problem.
                Application.DoEvents()

                ' Set focus to the first control on the tab.
                If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                    m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                End If
            End With

        Catch





            tabMainTabPreviousTab = tabMainTab.SelectedIndex
        End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Check mandatory controls have been entered into.
            m_lReturn = m_oFormFields.CheckMandatoryControls()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If
            'Validate some  stuff double check
            m_lReturn = ValidateOK()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'Skip validation when deleting
            If Not (m_iTask = gPMConstants.PMEComponentAction.PMDelete) Then
                If Not ValidateMediaDetails() Then
                    Exit Sub
                End If
            End If

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'Save the pick lists
            'Put in the picklist PK values

            uctPickListBranches.ForeignKeys.Item("BankAccountID").Value = m_lBankAccountId
            uctPickListBranches.ForeignKeys.Item("user_id").Value = Nothing
            uctPickListBranches.ForeignKeys.Item("unique_id").Value = m_sUniqueId
            uctPickListBranches.ForeignKeys.Item("screen_hierarchy").Value = m_sScreenHierarchy

            m_lReturn = uctPickListBranches.Save()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdNavigate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNavigate.Click

        ' Click event of the Navigate button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMNavigate

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    'UPGRADE_NOTE: (7001) The following declaration (cmdNext_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub cmdNext_Click(ByRef Index As Integer)
    '
    'Try 
    '
    ' Change to the next tab.
    'If SSTabHelper.GetSelectedIndex(tabMainTab) < SSTabHelper.GetTabCount(tabMainTab) - 1 Then
    'SSTabHelper.SetSelectedIndex(tabMainTab, Index + 1)
    'End If
    '
    ' Set focus to the first control on the tab.
    'If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
    'm_ctlTabFirstLast(ACControlStart, Index + 1).Focus()
    'End If
    '
    'Catch 
    '
    '
    '
    '
    'Exit Sub
    'End Try
    '
    '
    'End Sub
    ' PRIVATE Events (End)


    Private Sub GetBankAccountDelay()
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetBankAccountDelay
        ' PURPOSE: Retrieve the bank account delays and populate the listview
        ' AUTHOR: Danny Davis
        ' DATE: 02 October 2006, 13:42:41
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim vResults As Object
        Dim lvwItem As ListViewItem


        Try


            m_lReturn = m_oBusiness.SelectBankAccountDelay(lBankAccountID:=m_lBankAccountId, lMediaTypeID:=0, r_vBankAccountDelay:=vResults)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(m_lReturn.ToString() + ", " + +", Select Bank Account Delay failed.")
            End If

            lvwBankAccountDelay.Items.Clear()

            If Information.IsArray(vResults) Then

                For lRow As Integer = vResults.GetLowerBound(1) To vResults.GetUpperBound(1)


                    lvwItem = lvwBankAccountDelay.Items.Add("K" & CStr(vResults(0, lRow)), CStr(vResults(0, lRow)), "")

                    ListViewHelper.GetListViewSubItem(lvwItem, 1).Text = CStr(vResults(1, lRow))

                    ListViewHelper.GetListViewSubItem(lvwItem, 2).Text = CStr(vResults(3, lRow))

                    ListViewHelper.GetListViewSubItem(lvwItem, 3).Text = CStr(vResults(2, lRow))
                Next lRow
            End If



            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBankAccountDelay", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    Exit Sub
            End Select

        Finally


        End Try
        Exit Sub
    End Sub


    Private Sub ProcessBankAccountDelay(ByRef iMode As Integer)
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: ProcessBankAccountDelay
        ' PURPOSE: Add/Edit/Delete bank account delay records
        ' AUTHOR: Danny Davis
        ' DATE: 02 October 2006, 14:14:51
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim frm As frmBankAccountDelay
        Dim lBankAccountDelayID As Integer
        Dim sScreenHierarchy As String = ""

        If String.IsNullOrEmpty(m_sUniqueId) Then
            m_sUniqueId = GetUniqueID()
        End If

        Try

            If Not (lvwBankAccountDelay.FocusedItem Is Nothing) Then
                lBankAccountDelayID = CInt(lvwBankAccountDelay.FocusedItem.Text)
            End If

            If iMode = gPMConstants.PMEComponentAction.PMAdd Or iMode = gPMConstants.PMEComponentAction.PMEdit Then
                'Show the empty form
                frm = New frmBankAccountDelay()
                frm.Mode = iMode
            Else
                If MessageBox.Show("Are you sure you want to delete this delay entry?", "Delete Delay", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = System.Windows.Forms.DialogResult.No Then
                    Exit Sub
                End If
            End If

            Select Case iMode
                Case gPMConstants.PMEComponentAction.PMAdd

                    'developer guide no. 68
                    'Load(frm)
                    frm.ShowDialog()
                    If frm.Status <> gPMConstants.PMEReturnCode.PMCancel Then

                        sScreenHierarchy = $"Accounts({txtAccountNumber.Text.Trim()})\Receipt Delay({frm.MediaTypeDesc})"

                        m_lReturn = m_oBusiness.AddBankAccountDelay(lBankAccountID:=m_lBankAccountId, lMediaTypeID:=frm.MediaTypeID, iDelay:=frm.DaysDelay, v_sUniqueId:=m_sUniqueId, v_sScreenHierarchy:=sScreenHierarchy)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New System.Exception(m_lReturn.ToString() + ", " + +", Failed to delete Bank Account Delay.")
                        End If
                    End If
                Case gPMConstants.PMEComponentAction.PMEdit
                    'Show the form using the data

                    'developer guide no. 52
                    frm.MediaTypeID = CInt(lvwBankAccountDelay.FocusedItem.SubItems(1).Text)
                    frm.DaysDelay = CInt(lvwBankAccountDelay.FocusedItem.SubItems(3).Text)

                    frm.ShowDialog()

                    sScreenHierarchy = $"Account({txtAccountNumber.Text.Trim()})\Receipt Delay({frm.MediaTypeDesc})"

                    m_lReturn = m_oBusiness.UpdateBankAccountDelay(lBankAccountDelayID:=lBankAccountDelayID, lBankAccountID:=m_lBankAccountId, lMediaTypeID:=frm.MediaTypeID, iDelay:=frm.DaysDelay, v_sUniqueId:=m_sUniqueId, v_sScreenHierarchy:=sScreenHierarchy)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(m_lReturn.ToString() + ", " + +", Failed to delete Bank Account Delay.")
                    End If

                Case gPMConstants.PMEComponentAction.PMDelete

                    If Not (lvwBankAccountDelay.FocusedItem Is Nothing) Then
                        sScreenHierarchy = $"Bank({m_sBankCode})\Account({txtAccountNumber.Text.Trim()})\Receipt Delay({lvwBankAccountDelay.FocusedItem.SubItems(2).Text()})"
                    End If

                    m_lReturn = m_oBusiness.DeleteBankAccountDelay(lBankAccountDelayID:=lBankAccountDelayID, v_sUniqueId:=m_sUniqueId, v_sScreenHierarchy:=sScreenHierarchy)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(m_lReturn.ToString() + ", " + +", Failed to delete Bank Account Delay.")
                    End If
                Case Else
            End Select

            GetBankAccountDelay()



            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessBankAccountDelay", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    Exit Sub
            End Select

        Finally
            If iMode = gPMConstants.PMEComponentAction.PMAdd Or iMode = gPMConstants.PMEComponentAction.PMEdit Then
                frm.Close()
                frm = Nothing
            End If




        End Try
        Exit Sub
    End Sub

    ''' <summary>
    ''' ValidateMediaDetails
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ValidateMediaDetails() As Boolean
        Dim nResult As Boolean = PMEReturnCode.PMTrue
        Dim bSIRMediaTypeValidation As Object
        Dim oValidation As bSIRMediaTypeValidation.Business
        Dim olMediaID As Object
        Dim olCountryID As Object
        Dim oValid As Object
        Dim sBankName, sAddress1, sAddress2, sAddress3, sAddress4, sPostalCode As String
        Dim oValidationMessage As Object
        Dim bValidationOverridable As Boolean
        Dim sStrippedString As String = ""

        Try

            If AlphanumericValidation(Trim(txtBIC.Text)) <> PMEReturnCode.PMTrue Then
                MessageBox.Show("Only alphanumeric characters allowed in BIC field.", "Bank Validation", _
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return PMEReturnCode.PMFalse
            End If

            If AlphanumericValidation(Trim(txtIBAN.Text)) <> PMEReturnCode.PMTrue Then
                MessageBox.Show("Only alphanumeric characters allowed in IBAN field.", "Bank Validation", _
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return PMEReturnCode.PMFalse
            End If

            olCountryID = g_oObjectManager.CountryID
            Dim temp_oValidation As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oValidation, "bSIRMediaTypeValidation.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oValidation = temp_oValidation

            sStrippedString = txtAccountCode.Text.Replace(" ", "") & "|" &
                              txtAccountNumber.Text.Replace(" ", "") & "|" & ""


            oValidation.ValidateNumber(olMediaID, olCountryID, sStrippedString, _
                                       oValid, sBankName, sAddress1, sAddress2, sAddress3, sAddress4, _
                                       sPostalCode, oValidationMessage, bValidationOverridable, "Bank", _
                                       sBIC:=(txtBIC.Text).Trim(), sIBAN:=(txtIBAN.Text).Trim())

            'if vValid = false then check for ValidationMessage and store all of
            'them in a string

            Dim sMessage, IsValid As String
            If Not oValid And Not oValid.Equals(0) Then
                If Information.IsArray(oValidationMessage) Then

                    For iErrCount As Integer = 0 To oValidationMessage.GetUpperBound(0)

                        sMessage = sMessage & Strings.Chr(13) & Strings.Chr(10) & CStr(oValidationMessage(iErrCount))
                    Next
                Else
                    'if there is no message then store the generic message
                    sMessage = "Bank details have failed validation"
                End If

                'if validation are overridable then show the message with vbYesNo
                'PN48811
                If bValidationOverridable And Not m_bValidationMessage Then
                    sMessage = sMessage & Strings.Chr(13) & Strings.Chr(10) & "Do you wish to override the bank validation?"
                    IsValid = CStr(MessageBox.Show(sMessage, "Bank Validation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
                    If IsValid = System.Windows.Forms.DialogResult.Yes Then
                        m_bValidationMessage = True
                        nResult = True
                    Else
                        nResult = False
                    End If
                ElseIf Not bValidationOverridable Then
                    MessageBox.Show(sMessage, "Bank Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    nResult = False
                    'cmdOK.Enabled = False
                End If
            Else
                nResult = True
            End If

            oValidation = Nothing

        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, _
                               sMsg:="Failed to process ValidateMediaDetails", _
                               vApp:=ACApp, vClass:=ACClass, _
                               vMethod:="ValidateMediaDetails", vErrNo:=Information.Err().Number, _
                               vErrDesc:=Information.Err().Description, excep:=ex)
            nResult = gPMConstants.PMEReturnCode.PMFalse
        Finally
        End Try
        Return nResult
    End Function

    ' ***************************************************************** '
    ' Name: FindAccount
    ' Parameters: n/a
    ' Description:
    ' History:
    ' Created : VB : 12-03-2008 :
    ' ***************************************************************** '
    Public Function FindAccount(ByRef r_lAccountId As Integer, ByRef r_sAccountName As String) As Integer
        Dim result As Integer = 0
        Dim iACTFindAccount As Object

        Const kFindAccount As String = "FindAccount"
        Const kKeyName As Integer = 0

        Dim lReturn, lStatus As Integer
        Dim vAccountId As String = ""
        Dim vName As String = ""
        Dim vKeyArray(,) As Object

        'developer guide no. todo list
        Dim oFindAccount As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue
            Dim temp_oFindAccount As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oFindAccount, sClassName:="iACTFindAccount.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFindAccount = temp_oFindAccount

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kFindAccount, "g_oObjectManager.GetInstance Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            m_lReturn = CType(oFindAccount, SSP.S4I.Interfaces.ILocalInterface).Initialise()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kFindAccount, "Initialise Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            oFindAccount.CallingAppName = "iACTBankAccount.Interface"


            m_lReturn = oFindAccount.SetProcessModes(vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vEffectiveDate:=DateTime.Now)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kFindAccount, "SetProcessModes Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            m_lReturn = oFindAccount.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kFindAccount, "oFindAccount.Start() Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            If oFindAccount.Status = gPMConstants.PMEReturnCode.PMOK Then

                m_lReturn = oFindAccount.GetKeys(vKeyArray)

                If Information.IsArray(vKeyArray) Then

                    For iRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)
                        Select Case vKeyArray(kKeyName, iRow)
                            Case PMNavKeyConst.ACTKeyNameAccountID

                                vAccountId = CStr(vKeyArray(1, iRow))
                            Case PMNavKeyConst.ACTKeyNameAccountName

                                vName = CStr(vKeyArray(1, iRow))
                        End Select
                    Next iRow
                End If

            Else


                If oFindAccount.Status = gPMConstants.PMEReturnCode.PMCancel Then
                    result = gPMConstants.PMEReturnCode.PMCancel
                    Return result
                Else
                    gPMFunctions.RaiseError(kFindAccount, kFindAccount & " Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If


            r_lAccountId = gPMFunctions.ToSafeLong(vAccountId)
            r_sAccountName = vName


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sUsername:=g_oObjectManager.UserName, v_sClass:=ACClass, v_sMethod:=kFindAccount, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
            If Not (oFindAccount Is Nothing) Then

                oFindAccount.Dispose()
            End If
            oFindAccount = Nothing

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    Private Sub txtDirectDebitSupplierID_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtDirectDebitSupplierID.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        '    If (KeyAscii < 48 Or KeyAscii > 57) Then
        '        KeyAscii = 0
        '
        '    End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub txtStartChequeNumber_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtStartChequeNumber.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        If KeyAscii <> 8 Then
            If KeyAscii < 48 Or KeyAscii > 57 Then
                KeyAscii = 0
            End If
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub
    Private Sub txtAccountNumber_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtAccountNumber.KeyPress
        If Char.IsNumber(e.KeyChar) Or e.KeyChar = vbBack Then
        Else
            e.Handled = True
        End If

    End Sub

    Private Sub txtAccountCode_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtAccountCode.KeyPress
        If Char.IsNumber(e.KeyChar) Or e.KeyChar = vbBack Then
        Else
            e.Handled = True
        End If

    End Sub

    ''' <summary>
    ''' Called on KeyPress event of textox to validate only alphanumeric input
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub AlphanumericValidation(sender As Object, e As KeyPressEventArgs) Handles txtBIC.KeyPress, txtIBAN.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(e.KeyChar)
        If (KeyAscii >= 48 AndAlso KeyAscii <= 57) OrElse _
            (KeyAscii >= 65 AndAlso KeyAscii <= 90) OrElse _
            (KeyAscii >= 97 And KeyAscii <= 122) _
            OrElse KeyAscii = 8 OrElse KeyAscii = 127 Then
        Else
            KeyAscii = 0
        End If
        If KeyAscii = 0 Then
            e.Handled = True
        End If
        e.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    ''' <summary>
    ''' Called to validate alphanumeric validation via Copy/Paste 
    ''' </summary>
    ''' <param name="sInput"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function AlphanumericValidation(ByVal sInput As String) As Integer
        If System.Text.RegularExpressions.Regex.IsMatch(sInput, "^[a-zA-Z0-9]*$") Then
            Return PMEReturnCode.PMTrue
        Else
            Return PMEReturnCode.PMFalse
        End If
    End Function

End Class
