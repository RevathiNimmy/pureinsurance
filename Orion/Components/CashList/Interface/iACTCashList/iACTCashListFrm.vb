Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Globalization
Imports System.Windows.Forms
'developer guide no.129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 10th September 1997
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

    ' Status members
    Private m_sProcessStatus As New FixedLengthString(2)
    Private m_sMapStatus As New FixedLengthString(2)
    Private m_sStepStatus As New FixedLengthString(2)

    ' {* USER DEFINED CODE (Begin) *}

    Private m_sCashListRoadmap As String = ""
    Private m_sDebitCredit As String = ""

    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iACTCashList.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Form control
    Private m_oFormfields As iPMFormControl.FormFields

    ' Variables to store the lookup values/details.
    Private m_vLookupValues As Object
    Private m_vLookupDetails As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    ' Stores the details from the business object.

    ' {* USER DEFINED CODE (Begin) *}
    Private m_lCashlistID As Integer
    Private m_lCashListStatusID As Integer
    Private m_lCashlistTypeID As Integer
    Private m_sCashListRef As New FixedLengthString(25)
    Private m_iCompanyID As Integer
    Private m_lBankAccountID As Integer
    Private m_iCurrencyID As Integer
    Private m_dtListDate As Date
    Private m_cControlTotal As Decimal
    Private m_lItemCount As Integer
    Private m_iBaseCurrencyID As Integer
    Private m_sScreenMode As String = ""
    Private m_lMediaTypeId As Integer
    Private m_bMediaTypeSpecifiedInKeys As Boolean
    Private m_vDocumentIds As Object
    Private m_lAccountId As Integer
    Private m_crAmount As Decimal
    Private m_bSetDefaultCurrencyOnly As Boolean
    Private m_bMulitCurrencyFlag As Boolean
    Private m_iForceDepositeCurrencyID As Integer

    Public WriteOnly Property ForceDepositeCurrencyID() As Integer
        Set(ByVal Value As Integer)
            m_iForceDepositeCurrencyID = Value
        End Set
    End Property
    Public ReadOnly Property Amount() As Decimal
        Get
            Return m_crAmount
        End Get
    End Property

    Public WriteOnly Property AccountId() As Integer
        Set(ByVal Value As Integer)
            m_lAccountId = Value
        End Set
    End Property

    Public WriteOnly Property DocumentIds() As Object
        Set(ByVal Value As Object)


            m_vDocumentIds = Value
        End Set
    End Property

    Public WriteOnly Property MediaTypeSpecifiedInKeys() As Boolean
        Set(ByVal Value As Boolean)
            m_bMediaTypeSpecifiedInKeys = Value
        End Set
    End Property


    Public Property MediaTypeID() As Integer
        Get
            Return m_lMediaTypeId
        End Get
        Set(ByVal Value As Integer)
            m_lMediaTypeId = Value
        End Set
    End Property

    Public WriteOnly Property ScreenMode() As String
        Set(ByVal Value As String)
            m_sScreenMode = Value
        End Set
    End Property

    Public Property MulitCurrencyFlag() As Boolean
        Get
            Return m_bMulitCurrencyFlag
        End Get
        Set(ByVal value As Boolean)
            m_bMulitCurrencyFlag = value
        End Set
    End Property


    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Data Members (End)

    Public Property Status() As Integer
        Get
            ' Return the interface exit status.
            Return m_lStatus
        End Get
        Set(ByVal Value As Integer)
            m_lStatus = Value
        End Set
    End Property

    ' PUBLIC Property Procedures (Begin)
    Public WriteOnly Property CashListRoadmap() As String
        Set(ByVal Value As String)
            m_sCashListRoadmap = Value
        End Set
    End Property

    Public WriteOnly Property DebitCredit() As String
        Set(ByVal Value As String)
            m_sDebitCredit = Value
        End Set
    End Property

    Public ReadOnly Property ErrorNumber() As Integer
        Get

            ' Standard Property.
            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber

        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property



    Public Property Task() As Integer
        Get

            ' Return the objects task.
            Return m_iTask

        End Get
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the objects task.
            m_iTask = Value

        End Set
    End Property

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the navigate flag.
            m_lNavigate = Value

        End Set
    End Property

    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the process mode.
            m_lProcessMode = Value

        End Set
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the type of business.
            m_sTransactionType = Value

        End Set
    End Property

    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)

            ' Standard Property.

            ' Set the effective date.
            m_dtEffectiveDate = Value

        End Set
    End Property

    Public ReadOnly Property StepStatus() As String
        Get

            ' Standard Property.

            ' Return the Steps Status
            Return m_sStepStatus.Value

        End Get
    End Property

    ' {* USER DEFINED CODE (Begin) *}
    Public Property CashListRef() As String
        Get
            Return m_sCashListRef.Value
        End Get
        Set(value As String)
            m_sCashListRef.Value = value
        End Set
    End Property

    Public Property CashlistID() As Integer
        Get

            ' Return the Cash List ID
            Return m_lCashlistID

        End Get
        Set(ByVal Value As Integer)

            ' Set the Cash List ID
            m_lCashlistID = Value

        End Set
    End Property
    Public Property CashlistTypeID() As Integer
        Get

            ' Return the Cash List Type ID
            Return m_lCashlistTypeID

        End Get
        Set(ByVal Value As Integer)

            ' Set the Cash List Type ID
            m_lCashlistTypeID = Value

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
    'eck100500
    ' {* USER DEFINED CODE (End) *}
    ' PUBLIC Property Procedures (End)

    Public Property CompanyID() As Integer
        Get
            Return m_iCompanyID
        End Get
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the Company ID.
            m_iCompanyID = Value

        End Set
    End Property

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: SetStatus (Standard Method)
    '
    ' Description: Set the Process, Map and Step status.
    ' Note:        A Property Get is provided for the Step Status only
    '              as this is the only one which this component can
    '              alter directly.
    ' ***************************************************************** '
    Public Function SetStatus(ByRef sProcessStatus As String, ByRef sMapStatus As String, ByRef sStepStatus As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the current Status settings.
            m_sProcessStatus.Value = sProcessStatus.Trim()
            m_sMapStatus.Value = sMapStatus.Trim()
            m_sStepStatus.Value = sStepStatus.Trim()

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'eck090500
    ' ***************************************************************** '
    ' Name: GetCompany (Standard Method)
    '
    ' Description: Gets valid Source ID's  and if nessessary displays selection
    '
    ' ***************************************************************** '
    Public Function GetCompany(ByRef m_iCompanyID As Integer) As Integer
        Dim result As Integer = 0

        Dim m_oBranch As iPMBBranch.Interface_Renamed
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_m_oBranch As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBranch, sClassName:="iPMBBranch.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            m_oBranch = temp_m_oBranch

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'RKS PN14431
            'changed from GetSource to GetSourceAccounts
            'GetSourceAccounts will take care of selection of
            'closed branches also if accounting is allowed on that Branch

            m_lReturn = m_oBranch.GetSourceAccounts(iSourceID:=m_iCompanyID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_oBranch.Dispose()
            m_oBranch = Nothing
            Return result

        Catch excep As System.Exception



            ' Error Section

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Company", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCompany", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'eck090500

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


            m_lReturn = m_oBusiness.GetDetails(vCashListID:=m_lCashlistID)

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



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: BusinessToInterface
    '
    ' Description: Updates all interface details from the business
    '              object.
    '
    ' ***************************************************************** '
    Public Function BusinessToInterface() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' Assign the details from the business object
            ' to the data storage.
            m_lReturn = BusinessToData()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Assign the details to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to assign the all of the interface
            ' details from the business object, using the FormatField
            ' function for any type conversion.
            '
            ' Example:-
            '
            '    txtDesc.Text = FormatField( _
            ''        iFormatType:=PMFormatString, _
            ''        vFieldValue:=m_sDDesc$)
            '
            '    optChoice.Value = CBool(FormatField( _
            ''        iFormatType:=PMFormatBoolean, _
            ''        vFieldValue:=m_iDChoice%))
            '
            '    txtDate.Text = FormatField( _
            ''        iFormatType:=PMFormatDateLong, _
            ''        vFieldValue:=m_dtDDate)
            '
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************
            txtReference.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=m_sCashListRef.Value)


            SelectcboItem(CType(uctType, Object), m_lCashlistTypeID)

            'uctType.ItemId = m_lCashlistTypeID

            uctBankAccount.Id = m_lBankAccountID

            uctCurrency.CurrencyId = m_iCurrencyID

            txtDate.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateLong, vFieldValue:=m_dtListDate)

            txtTotalAmount.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=m_cControlTotal)

            txtTotalItems.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatInteger, vFieldValue:=m_lItemCount)

            panStatus.Text = uctStatus.ItemDescription(m_lCashListStatusID)

            If MulitCurrencyFlag = True Then
                MessageBox.Show("The transactions that you have selected for payment have different transaction currencies", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: InterfaceToBusiness
    '
    ' Description: Updates all business members from the interface
    '              details.
    '
    ' ***************************************************************** '
    Public Function InterfaceToBusiness() As Integer

        Dim result As Integer = 0
        Dim lBusinessDataID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the business object.

            ' Assign the details from the interface to the data storage.
            m_lReturn = InterfaceToData()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Set the business data ID to one because we are only
            ' dealing with one record item only.
            lBusinessDataID = 1

            ' Check the task.
            Select Case (m_iTask)
                Case gPMConstants.PMEComponentAction.PMAdd

                    ' Inform the business object with a new data item.

                    ' {* USER DEFINED CODE (Begin) *}
                    ' Usen DirectAdd as we need to get the ID back

                    m_lReturn = m_oBusiness.DirectAdd(vCashListID:=m_lCashlistID, vCashliststatusID:=m_lCashListStatusID, vCashListTypeID:=m_lCashlistTypeID, vCashlistRef:=m_sCashListRef.Value, vCompanyID:=m_iCompanyID, vBankAccountID:=m_lBankAccountID, vCurrencyID:=m_iCurrencyID, vListDate:=m_dtListDate, vControlTotal:=m_cControlTotal, vItemCount:=m_lItemCount)
                    ' {* USER DEFINED CODE (End) *}

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Inform the business object with an updated data item.

                    ' {* USER DEFINED CODE (Begin) *}

                    m_lReturn = m_oBusiness.EditUpdate(lRow:=lBusinessDataID, vCashListID:=m_lCashlistID, vCashliststatusID:=m_lCashListStatusID, vCashListTypeID:=m_lCashlistTypeID, vCashlistRef:=m_sCashListRef.Value, vCompanyID:=m_iCompanyID, vBankAccountID:=m_lBankAccountID, vCurrencyID:=m_iCurrencyID, vListDate:=m_dtListDate, vControlTotal:=m_cControlTotal, vItemCount:=m_lItemCount)
                    ' {* USER DEFINED CODE (End) *}
            End Select

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '' ***************************************************************** '
    '' Name: DisplayLookupDetails
    ''
    '' Description: Displays all of the lookup details using the lookup
    ''              values/details.
    ''
    '' ***************************************************************** '
    'Public Function DisplayLookupDetails() As Long
    '
    '    On Error GoTo Err_DisplayLookupDetails
    '
    '    DisplayLookupDetails = PMTrue
    '
    '    ' Get the lookup values.
    '
    '    m_lReturn& = GetLookupValues()
    '
    '    ' Check for errors.
    '    If (m_lReturn& <> PMTrue) Then
    '        DisplayLookupDetails = PMFalse
    '        Exit Function
    '    End If
    '
    '    ' Get all of the lookup details.
    '
    '    ' {* USER DEFINED CODE (Begin) *}
    '
    '    ' ************************************************************
    '    ' Enter your code here to retreive all of the lookup
    '    ' descriptions for a given lookup type.
    '    ' The GetLookupDetails function will allow you to do this.
    '    '
    '    ' Example:-
    '    '
    '    '    m_lReturn& = GetLookupDetails( _
    ''    '        sLookupTable:=PMLookupCodeName, _
    ''    '        ctlLookup:=cmbCodeName)
    '    '
    '    '    ' Check for errors.
    '    '    If (m_lReturn& <> PMTrue) Then
    '    '        DisplayLookupDetails = PMFalse
    '    '        Exit Function
    '    '    End If
    '    '
    '    ' NOTE: Replace this section with your new code.
    '    ' ************************************************************
    '
    '    ' {* USER DEFINED CODE (End) *}
    '
    '    Exit Function
    '
    'Err_DisplayLookupDetails:
    '
    '    ' Error Section
    '
    '    DisplayLookupDetails = PMError
    '
    '    ' Log Error.
    '    LogMessage _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="Failed to display the lookup details", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="DisplayLookupDetails", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    '
    'End Function

    ' ***************************************************************************
    '
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields.
    '
    ' ***************************************************************************
    Public Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try

            m_oFormfields = New iPMFormControl.FormFields()

            m_oFormfields.LanguageID = g_iLanguageID

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add the controls...

            ' Reference
            m_lReturn = m_oFormfields.AddNewFormField(ctlControl:=txtReference, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Date
            m_lReturn = m_oFormfields.AddNewFormField(ctlControl:=txtDate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Amounts total
            m_lReturn = m_oFormfields.AddNewFormField(ctlControl:=txtTotalAmount, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatMoney, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Total Items
            m_lReturn = m_oFormfields.AddNewFormField(ctlControl:=txtTotalItems, lFieldType:=gPMConstants.PMEDataType.PMLong, lFormat:=gPMConstants.PMEFormatStyle.PMFormatLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            Return result

        Catch excep As System.Exception



            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the details to the data storage.

            ' {* USER DEFINED CODE (Begin) *}


            m_lReturn = m_oBusiness.GetNext(vCashListID:=m_lCashlistID, vCashliststatusID:=m_lCashListStatusID, vCashListTypeID:=m_lCashlistTypeID, vCashlistRef:=m_sCashListRef.Value, vCompanyID:=m_iCompanyID, vBankAccountID:=m_lBankAccountID, vCurrencyID:=m_iCurrencyID, vListDate:=m_dtListDate, vControlTotal:=m_cControlTotal, vItemCount:=m_lItemCount)


            ' {* USER DEFINED CODE (End) *}

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retreive the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: InterfaceToData
    '
    ' Description: Updates the data storage from the interface details.
    '
    ' ***************************************************************** '
    Private Function InterfaceToData() As Integer

        Dim result As Integer = 0
        Try

            Dim iCurrencyID As Integer

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the data storage.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to assign all of the details from the
            ' interface to the data storage.
            '
            ' Example:-
            '
            '    m_DName$ = trim$(txtName.Text)
            '    m_DDate = CDate(txtDate.Text)
            '    m_iDCodeID% = cmbCode.ItemData(cmbCode.ListIndex)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            m_sCashListRef.Value = txtReference.Text.Trim()

            m_lCashlistTypeID = uctType.ItemId

            m_lBankAccountID = uctBankAccount.Id

            'DD 15/08/2002 - CurrencyID property was not set if the control
            ' was not clicked
            iCurrencyID = uctCurrency.ItemData(uctCurrency.ListIndex)

            ' if there are allocated documents to match
            If Information.IsArray(m_vDocumentIds) Then

                ' get the payment amount in specified currency
                m_lReturn = GetConvertedPaymentAmount(v_iCurrencyIdTO:=iCurrencyID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            ' use the new currency id
            m_iCurrencyID = iCurrencyID

            If Information.IsDate(txtDate.Text) Then
                m_dtListDate = CDate(txtDate.Text)
            Else
                m_dtListDate = DateTime.Now
            End If

            ' This use of CCur is temporary as it is locale specific
            Dim dbNumericTemp As Double
            If Double.TryParse(txtTotalAmount.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                m_cControlTotal = CDec(txtTotalAmount.Text)
            Else
                m_cControlTotal = 0
            End If

            'eck040601 replace cInt with cLng
            Dim dbNumericTemp2 As Double
            If Double.TryParse(txtTotalItems.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                m_lItemCount = CInt(txtTotalItems.Text)
            Else
                m_lItemCount = 0
            End If

            ' Set Company ID to Source ID
            'eck090500 This has been obtained
            '   m_iCompanyID = g_iSourceID

            '    m_lCashListStatusID initial value set but not modifiable

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

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
        Dim lCashlistTypeId As Integer
        Dim vUnderwriting As String = ""
        Dim lMediaTypeID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            iPMFunc.getUnderwritingOrAgency(vUnderwriting)

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

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}
            VB6.SetDefault(cmdOK, True)

            If m_iCurrencyID > 0 Then
                m_bSetDefaultCurrencyOnly = True
            End If
            'If this is called from Insurer payment  
            If m_sScreenMode = "ACTINSPAY2" Then
                uctType.Enabled = False
            End If
            ' Set to the default specified by the roadmap
            ' NB: this is a key passed in via the roadmap,
            ' not the roadmaps name

            Select Case m_sCashListRoadmap
                Case "PAYMENT", "PAYMENTS"

                    If m_sScreenMode = "CLP" Then
                        uctType.ListIndex = 0
                        lCashlistTypeId = 3
                        uctType.Enabled = False
                    Else
                        uctType.ListIndex = 1
                        lCashlistTypeId = 1
                    End If

                Case "RECEIPT", "RECEIPTS"

                    uctType.ListIndex = 2
                    lCashlistTypeId = 2


                Case "PAYNOW"
                    'Float Balance and Pre-Payment (RC)
                    uctType.Enabled = False

                Case Else
                    'DD 05/09/2002
                    'Handle the call from Premium Finance for the deposit
                    If m_sTransactionType = "PFCASH" Then
                        uctType.ListIndex = 2
                        lCashlistTypeId = 2
                    End If
            End Select

            ' PR - SFS 1707 - add debit_credit key
            ' Set to the debit_credit specified value if exists
            Select Case m_sDebitCredit
                Case "credit"
                    uctType.ListIndex = 1
                    lCashlistTypeId = 1
                Case "debit"
                    uctType.ListIndex = 1
                    lCashlistTypeId = 2
                Case Else
                    'do nothing
            End Select

            ' Setup default data for Add
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then

                ' AB 19/03/02 - add call to get default bank account for cashlist

                m_lReturn = m_oBusiness.GetBankAccountDefault(vSourceID:=m_iCompanyID, vCashListTypeID:=lCashlistTypeId, vDefaultAccountID:=m_lBankAccountID, vMediaTypeId:=lMediaTypeID)
                ' Check for errors

                Select Case m_lReturn
                    Case gPMConstants.PMEReturnCode.PMTrue
                        uctBankAccount.Id = m_lBankAccountID

                        'PN28175 fix
                        If m_sScreenMode = "CLP" Then
                            uctBankAccount.Enabled = False
                            lblBank.Enabled = False
                        End If
                    Case gPMConstants.PMEReturnCode.PMNotFound
                        ' Do nothing, leave the current selection
                    Case Else
                        Return m_lReturn
                End Select

                ' if the media type has not already been specified in the keys
                If Not m_bMediaTypeSpecifiedInKeys Then
                    ' use the media type default specified on the bank account default
                    m_lMediaTypeId = lMediaTypeID
                End If




                txtReference.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=IIf(m_sCashListRef.Value = "", "", m_sCashListRef.Value))

                If m_iCompanyID <> 0 Then
                    uctCurrency.CompanyId = m_iCompanyID
                Else
                    uctCurrency.CompanyId = g_iSourceID
                End If

                uctCurrency.RefreshList()

                If m_iCurrencyID = 0 Then
                    m_iCurrencyID = uctBankAccount.CurrencyId(uctBankAccount.Id)
                    'Get the Base curreny currency Id from the business object

                    m_lReturn = m_oBusiness.GetBranchBaseCurrency(v_lSourceID:=m_iCompanyID, r_iCurrencyID:=m_iBaseCurrencyID)

                    uctCurrency.CurrencyId = m_iBaseCurrencyID
                    For nIndex As Integer = 0 To uctCurrency.ListCount - 1
                        If uctCurrency.ItemData(nIndex) = uctBankAccount.CurrencyId(uctBankAccount.Id) Then
                            If uctBankAccount.IsCashReceiveInThisCurrencyOnly(uctBankAccount.Id) = 1 Then
                                uctCurrency.CurrencyId = uctBankAccount.CurrencyId(uctBankAccount.Id)
                                'uctCurrency.Enabled = True
                                uctCurrency.Enabled = False
                            End If
                            Exit For
                        End If
                    Next nIndex
                End If


                If m_iCurrencyID <> 0 Then
                    uctCurrency.CurrencyId = m_iCurrencyID
                Else
                    uctCurrency.CurrencyId = g_iCurrencyID
                End If

                If m_lCashlistTypeID <> 0 Then
                    uctType.ItemId = m_lCashlistTypeID
                    'uctType.Enabled = False
                End If
                If m_iForceDepositeCurrencyID <> 0 Then
                    uctCurrency.CurrencyId = m_iForceDepositeCurrencyID
                    uctCurrency.Enabled = False
                    m_bSetDefaultCurrencyOnly = True
                End If

                txtDate.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateLong, vFieldValue:=DateTime.Now)

                txtTotalAmount.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=0)

                txtTotalItems.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatInteger, vFieldValue:=0)

                m_lCashListStatusID = gACTLibrary.ACTCashListStatusEntered
                panStatus.Text = uctStatus.ItemDescription(m_lCashListStatusID)

            End If

            ' Disable control amounts/items totals as does not fit in with initial
            '   expected use of the component.
            lblTotalAmount.Visible = False
            lblTotalItems.Visible = False
            txtTotalAmount.Visible = False
            txtTotalAmount.Enabled = False
            txtTotalItems.Visible = False
            txtTotalItems.Enabled = False
            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            'Developer Guide No 32

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

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise the control array with the number of
            ' tabs which contain data entry fields on (Remember
            ' that arrays start from zero, therefore you must
            ' subtract one from the number of tabs).
            ReDim m_ctlTabFirstLast(1, 0)

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

            Return result

        Catch excep As System.Exception



            ' Error Section.

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


            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() & _
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If


            cmdNavigate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNavigateCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMainTabTitle0, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblBank.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBankCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblTotalAmount.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTotalAmountCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblCurrency.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCurrencyCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblTotalItems.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTotalItemsCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblReference.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACReferenceCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblType.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTypeCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblStatus.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACStatusCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblDate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDateCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


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



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ValidateLookups
    '
    ' Description: Validate form controls (lookups)
    ' ***************************************************************** '
    Private Function ValidateLookups() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            If uctType.ListIndex = -1 Then
                MessageBox.Show("You must select a Cash List Type", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return result
            End If

            If uctBankAccount.ListIndex = -1 Then
                MessageBox.Show("You must select a Bank Account", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return result
            End If

            If uctCurrency.ListIndex = -1 Then
                MessageBox.Show("You must select a Currency", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to validate form controls", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateLookups", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
        ' Fire up the help screen
        'developer guide no.185
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = PMHelpFunc.ShowHelp(cmdHelp, ScreenHelpID)

    End Sub

    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()

        Dim sMessage, sTitle As String

        ' Forms initialise event.

        Try
            'developer guide no. 220
            Me.uctType.FirstItem = ""
            Me.uctStatus.FirstItem = "()"
            Me.uctCurrency.FirstItem = ""
            Me.uctBankAccount.FirstItem = ""

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bACTCashList.Form", vInstanceManager:="ClientManager")
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
            m_oGeneral = New iACTCashList.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Error Section

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Forms load event.

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

            m_lReturn = SetFieldValidation()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' Set the business keys.
            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}

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

            uctBankAccount.CompanyId = m_iCompanyID
            uctBankAccount.RefreshList()
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd And m_lBankAccountID <> 0 Then
                uctBankAccount.Id = m_lBankAccountID
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Error Section

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

            m_oFormfields.Dispose()

            m_oFormfields = Nothing


            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            ' Error Section.

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
        Catch



            ' Error Section.

            Exit Sub
        End Try


    End Sub



    Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged

        Try

            With tabMainTab
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



            ' Error Section.


            tabMainTabPreviousTab = tabMainTab.SelectedIndex
        End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.

        Try

            ' Check that everything is ok with form control
            m_lReturn = m_oFormfields.CheckMandatoryControls()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' PWF - Validate the lists
            m_lReturn = ValidateLookups()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            If Not CheckCurrencyExistsInBranch() Then
                Exit Sub
            End If

            m_lReturn = CheckCurrencyRatesExists()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'PN 49700
                If Me.uctCurrency.Enabled Then
                    Me.uctCurrency.Focus()
                End If
                Exit Sub
            End If

            If uctBankAccount.IsCashReceiveInThisCurrencyOnly(uctBankAccount.Id) = 1 Then
                If uctCurrency.CurrencyId <> uctBankAccount.CurrencyId(uctBankAccount.Id) Then
                    MessageBox.Show("Selected Bank can not receive cash in currency - '" & uctCurrency.CurrencyName & "'", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Me.uctCurrency.Focus()
                    Exit Sub
                End If
            End If

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If
            If MulitCurrencyFlag = True Then
                MessageBox.Show("The transactions that you have selected for payment have different transaction currencies", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If

        Catch excep As System.Exception



            ' Error Section.

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



            ' Error Section.

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



            ' Error Section.

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
    ' Error Section
    '
    'Exit Sub
    'End Try
    '
    '
    'End Sub
    Private Sub txtReference_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtReference.Enter
        iPMFunc.SelectText(txtReference)

        m_lReturn = m_oFormfields.GotFocus(ctlControl:=txtReference)

    End Sub
    Private Sub txtDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDate.Enter
        ' Check date.
        iPMValidate.CheckDateGotFocus(txtDate)

        ' Hightlight any text.
        iPMFunc.SelectText(txtDate)

        ' Check form control
        m_lReturn = m_oFormfields.GotFocus(ctlControl:=txtDate)

    End Sub
    Private Sub txtDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDate.Leave
        Dim sMessage As String = ""

        'PN17203

        txtDate.Text = txtDate.Text.Replace("."c, "/"c)
        Dim sYear As String
        'PN 30097
        If txtDate.Text.Trim().Length > 0 Then
            m_lReturn = m_oFormfields.LostFocus(ctlControl:=txtDate)
            sYear = txtDate.Text.Substring(txtDate.Text.Length - 4)
        End If


        Dim dbNumericTemp As Double
        If Not Double.TryParse(sYear, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
            sMessage = "Please enter date in valid format DD/MM/YYYY."
            txtDate.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateShort, DateTime.Today)
            MessageBox.Show(sMessage, "Invalid Date", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            txtDate.Focus()
            Exit Sub
        End If

        Dim eDateType As gSIRLibrary.SIREDateType = gSIRLibrary.SIRDateType(txtDate.Text)

        Select Case eDateType
            Case gSIRLibrary.SIREDateType.sireNullDate
                sMessage = "Date must be later than " & gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateShort, gSIRLibrary.SIRSystemLowDate) & "."
            Case gSIRLibrary.SIREDateType.sireInvalidDate
                sMessage = "Please enter a valid date."
                txtDate.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateShort, DateTime.Today)
        End Select

        If eDateType <> gSIRLibrary.SIREDateType.sireValidDate Then
            MessageBox.Show(sMessage, "Invalid Date", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            txtDate.Focus()
        Else
            iPMValidate.CheckDateLostFocus(txtDate)
            m_lReturn = m_oFormfields.LostFocus(ctlControl:=txtDate)
        End If

    End Sub

    Private Sub txtReference_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtReference.Leave

        m_lReturn = m_oFormfields.LostFocus(ctlControl:=txtReference)

    End Sub

    Private Sub txtTotalAmount_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTotalAmount.Enter
        iPMFunc.SelectText(txtTotalAmount)

        m_lReturn = m_oFormfields.GotFocus(ctlControl:=txtTotalAmount)

    End Sub
    Private Sub txtTotalAmount_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTotalAmount.Leave
        'BB Temporary fix until we have proper validation routines
        'txtTotalAmount.Text = FormatField( _
        ''                    iFormatType:=PMFormatCurrency, _
        ''                    vFieldValue:=txtTotalAmount.Text)

        ' CF proper validation implemented 04/08/98
        m_lReturn = m_oFormfields.GotFocus(ctlControl:=txtTotalAmount)

    End Sub
    Private Sub txtTotalItems_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTotalItems.Enter
        iPMFunc.SelectText(txtTotalItems)

        m_lReturn = m_oFormfields.GotFocus(ctlControl:=txtTotalItems)

    End Sub
    Private Sub txtTotalItems_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTotalItems.Leave
        'BB Temporary fix until we have proper validation routines
        'txtTotalItems.Text = FormatField( _
        ''                    iFormatType:=PMFormatInteger, _
        ''                    vFieldValue:=txtTotalItems.Text)

        ' CF proper validation implemented 04/08/98
        m_lReturn = m_oFormfields.LostFocus(ctlControl:=txtTotalItems)

    End Sub

    Private Sub uctBankAccount_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles uctBankAccount.Click


        ' only if the currency id has not already set
        ' should it be defaulted from the bank.
        'If m_iCurrencyID = 0 Then
        'Reinstated for Multi-Currency
        If CheckCurrencyExistsInBranch() Then

            If Not m_bSetDefaultCurrencyOnly Then
                uctCurrency.CurrencyId = uctBankAccount.CurrencyId(uctBankAccount.Id)
                uctCurrency.Enabled = True
            End If


            If uctBankAccount.IsCashReceiveInThisCurrencyOnly(uctBankAccount.Id) = 1 Then
                If uctCurrency.CurrencyId = uctBankAccount.CurrencyId(uctBankAccount.Id) Then
                    Me.uctCurrency.Enabled = False
                End If
            End If

        ElseIf m_iBaseCurrencyID <> 0 Then
            uctCurrency.CurrencyId = m_iBaseCurrencyID
        ElseIf m_iCurrencyID <> 0 Then
            uctCurrency.CurrencyId = m_iCurrencyID
        End If


    End Sub

    ' PRIVATE Events (End)

    Private Sub uctBankAccount_LostFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles uctBankAccount.LostFocus




        ' only if the currency id has not already set
        ' should it be defaulted from the bank.
        If m_iCurrencyID = 0 Then
            'Reinstated for Multi-Currency
            If CheckCurrencyExistsInBranch() Then
                If Not m_bSetDefaultCurrencyOnly Then
                    uctCurrency.CurrencyId = uctBankAccount.CurrencyId(uctBankAccount.Id)
                End If

                uctCurrency.Enabled = True

                If uctBankAccount.IsCashReceiveInThisCurrencyOnly(uctBankAccount.Id) = 1 Then
                    If uctCurrency.CurrencyId = uctBankAccount.CurrencyId(uctBankAccount.Id) Then
                        Me.uctCurrency.Enabled = False
                    End If
                End If
            Else
                'Reset the uctAccount
                uctBankAccount.ListIndex = m_lBankAccountID
            End If
        End If



    End Sub

    ' Public Methods (Begin)

    ' ***************************************************************** '
    ' Name: CheckCurrencyExistsInBranch
    '
    ' Description: Checks that whether selected Branch deals in
    ' selected Bankcurrency or Not.
    ' Author: Jitendra
    ' Date  : 16-09-2004
    ' Purpose '****JT Fixed PN 13545
    ''  Changed by SP to supress the message box for broking
    ' ***************************************************************** '
    Public Function CheckCurrencyExistsInBranch() As Integer

        Dim result As Integer = 0
        Dim vUnderwriting As String = ""

        Try

            iPMFunc.getUnderwritingOrAgency(vUnderwriting)
            result = gPMConstants.PMEReturnCode.PMFalse

            For nIndex As Integer = 0 To uctCurrency.ListCount - 1
                If uctCurrency.ItemData(nIndex) = uctBankAccount.CurrencyId(uctBankAccount.Id) Then
                    result = True
                    Exit For
                End If
            Next nIndex


            Return result

        Catch excep As System.Exception


            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to CheckCurrencyExistsInBranch", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckCurrencyExistsInBranch", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SelectcboItem
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 03-10-2003 : 229
    ' ***************************************************************** '
    Private Function SelectcboItem(ByRef r_oCbo As ComboBox, ByVal v_lSelectedId As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "SelectcboItem"

        Dim bItemNotFound As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bItemNotFound = True

            ' if the item id is valid
            If v_lSelectedId <> -1 Then

                ' for each item in the list
                For lItem As Integer = 0 To r_oCbo.Items.Count
                    ' search the item data array for a match
                    If VB6.GetItemData(r_oCbo, lItem) = v_lSelectedId Then

                        ' found a match - select the item
                        r_oCbo.SelectedIndex = lItem
                        bItemNotFound = False
                        Exit For
                    End If

                Next lItem

            End If

            If bItemNotFound Then

                ' log that we havent found the specified item
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lSelectedId", v_lSelectedId)
                gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to find item with id:" & CStr(v_lSelectedId) & " in :" & r_oCbo.Name, vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lSelectedId", v_lSelectedId)
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
            '*******************************

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ActionCashListTypeSelect
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 23-01-2006 : Cheque Production Workflow
    ' ***************************************************************** '
    Public Function ActionCashListTypeSelect() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ActionCashListTypeSelect"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lCashlistTypeId, lMediaTypeID As Integer
        Dim vUnderwriting As String = ""

        Try



            result = gPMConstants.PMEReturnCode.PMTrue
            iPMFunc.getUnderwritingOrAgency(vUnderwriting)

            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then

                ' determine the selected cash list type

                Select Case uctType.ListIndex
                    Case 0
                        lCashlistTypeId = gACTLibrary.ACTCashListTypeClaimPayments
                    Case 1
                        lCashlistTypeId = gACTLibrary.ACTCashListTypePayments
                    Case 2
                        lCashlistTypeId = gACTLibrary.ACTCashListTypeReceipts
                    Case Else
                        Return result
                End Select



                ' get the bank account defaults for the selected type

                lReturn = m_oBusiness.GetBankAccountDefault(vSourceID:=m_iCompanyID, vCashListTypeID:=lCashlistTypeId, vDefaultAccountID:=m_lBankAccountID, vMediaTypeId:=lMediaTypeID)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    If lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                        gPMFunctions.RaiseError(kMethodName, "GetBankAccountDefault Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                Else
                    uctBankAccount.Id = m_lBankAccountID
                End If

                ' if the media type has not already been specified in the keys
                If Not m_bMediaTypeSpecifiedInKeys Then
                    ' use the media type default specified on the bank account default
                    m_lMediaTypeId = lMediaTypeID
                End If

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function

    Private Sub uctType_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles uctType.Click
        ActionCashListTypeSelect()
    End Sub

    ' ***************************************************************** '
    ' Name: GetConvertedPaymentAmount
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 01-02-2006 : Claims Document Production
    ' ***************************************************************** '
    Public Function GetConvertedPaymentAmount(ByVal v_iCurrencyIdTO As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetConvertedPaymentAmount"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim crPaymentAmount As Decimal

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get the total amount to pay in the selected payment currency

            lReturn = m_oBusiness.ConvertPaymentAmount(v_iCurrencyTo:=v_iCurrencyIdTO, v_vDocumentIds:=m_vDocumentIds, v_lAccountId:=m_lAccountId, r_crPaymentAmount:=crPaymentAmount)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                MessageBox.Show("GetConvertedPaymentAmount Failed.", Application.ProductName)
            End If

            ' store the converted payment amount
            m_crAmount = crPaymentAmount


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function


    Public Function CheckCurrencyRatesExists() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CheckCurrencyRatesExists"

        Try
            Dim bCurrencyRateExist As Boolean


            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.CheckCurrencyRate(v_lCurrencyID:=uctCurrency.CurrencyId, r_bCurrencyRateExist:=bCurrencyRateExist)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Fails to CheckCurrencyRate", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not bCurrencyRateExist Then
                MessageBox.Show("Rates are not defined for the selected currency, Please define the rates first.", "Currency Rates", MessageBoxButtons.OK, MessageBoxIcon.Information)
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function
    Private Sub frmList_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
      cmdOK.focus()       
    End Sub

    Private Sub cmdOK_Enter(sender As Object, e As EventArgs) Handles cmdOK.Enter
        If txtReference.Enabled Then
            txtReference.Focus()
        End If
    End Sub
End Class